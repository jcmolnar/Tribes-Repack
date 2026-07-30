// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// List.CS									Presto, March '99 
//
//	This will eventually be available for scripters, but I don't trust it
//	yet.  When I tried to reimplement my menuing system using lists,
//	something caused tribes to crash!  It might have been the new menu
//	code, or it might have been lists - I use lists elsewhere without
//	crashing, though.  Still, to be on the safe side, please don't use
//	these functions until I can prove they're stable enough.
//
//	With Lists you will be able to add data to a list and retrieve it
//	sorted in several ways.  For instance my new menus implemented three
//	sorts:  byIndex (order of insertion), byKey (key press letter) and
//	byText (text of menu choice).  This would let a user choose a sort
//	and sort to their preference!  The same flexibility will be offered
//	to other features when I get this finished.
//
// ---------------------------------------------------------------------------
function List::Reset(%list) {
	deleteVariables("$List::"@%list@"_*");
	}
function List::New(%list) {
	List::Reset(%list);
	$List::[%list, sorts] = 0;
	$List::[%list, count] = 0;
	$List::[%list, maxNode] = 0;
	$List::[%list, free] = -1;
	}
function List::NewSort(%list, %sort, %func) {
	if (%func == "")
		%func = "List::DefaultSort";
	%sortNum = $List::[%list, sorts];
	$List::[%list, sort, %sortNum] = %sort;
	$List::[%list, sortFunc, %sort] = %func;
	$List::[%list, first, %sort] = -1;
	$List::[%list, last, %sort] = -1;
	$List::[%list, sorts]++;
	}

function List::PopFree(%list, %elm) {
	%node = $List::[%list, free];
	if (%node == -1)
		%node = $List::[%list, count];
	else	$List::[%list, free] = $List::[%list, node, %node, free];
	$List::[%list, count]++;
	if (%node >= $List::[%list, maxNode])
		$List::[%list, maxNode] = %node;
	$List::[%list, node,  %node] = %elm;
	$List::[%list, nodeFrom, %elm] = %node;
	return %node;
	}
function List::PushFree(%list, %node) {
	%elm = $List::[%list, node,  %node];
	$List::[%list, node,  %node] = "";
	$List::[%list, nodeFrom, %elm] = "";

	$List::[%list, node, %node, free] = $List::[%list, free];
	$List::[%list, free] = %node;
	$List::[%list, count]--;
	if (%node >= $List::[%list, maxNode]) {
		while (%node > 0 && List::GetNode(%list, %node) == "")
			%node--;
		$List::[%list, maxNode] = %node;
		}
	}
function List::MaxNode(%list) {
	return $List::[%list, maxNode];
	}
function List::GetNode(%list, %n) {
	return $List::[%list, node, %n];
	}
function List::FindNode(%list, %elm) {
	return $List::[%list, nodeFrom, %elm];
	}
function List::ValidSort(%list, %sort) {
	return $List::[%list, sortFunc, %sort] != "";
	}

function List::InsertSorted(%list, %node, %sort) {
	%func = $List::[%list, sortFunc, %sort];
	if (%func == "")
		return;

	%elm = List::GetNode(%list, %node);

	%nodePrev = -1;
	%nodeNext = $List::[%list, first, %sort];
	if (%nodeNext == "")
		%nodeNext = -1;
	if (%nodeNext != -1) {
		%elmNext = List::GetNode(%list, %nodeNext);
		while (eval(%func@"(%list, %elm, %elmNext);") >= 0) {
			%nodePrev = %nodeNext;
			%nodeNext = $List::[%list, node, %nodeNext, next, %sort];
			if (%nodeNext == "") {
				echo("bad list");
				return;
				}
			if (%nodeNext == -1)
				break;
			%elmNext = List::GetNode(%list, %nodeNext);
			}
		}

	$List::[%list, node, %node, prev, %sort] = %nodePrev;
	if (%nodePrev == -1)
		$List::[%list, first, %sort] = %node;
	else	$List::[%list, node, %nodePrev, next, %sort] = %node;

	$List::[%list, node, %node, next, %sort] = %nodeNext;
	if (%nodeNext == -1)
		$List::[%list, last, %sort] = %node;
	else	$List::[%list, node, %nodeNext, prev, %sort] = %node;
	}
function List::DeleteSorted(%list, %node, %sort) {
	if (!List::ValidSort(%list, %sort))
		return;

	%nodePrev = $List::[%list, node, %node, prev, %sort];
	%nodeNext = $List::[%list, node, %node, next, %sort];

	if (%nodePrev == -1)
		$List::[%list, first, %sort] = %nodeNext;
	else	$List::[%list, node, %nodePrev, next, %sort] = %nodeNext;

	if (%nodeNext == -1)
		$List::[%list, last, %sort] = %nodePrev;
	else	$List::[%list, node, %nodeNext, prev, %sort] = %nodePrev;

	$List::[%list, node, %node, next, %sort] = "";
	$List::[%list, node, %node, prev, %sort] = "";
	}
function List::Resort(%list, %elm, %sort) {
	%node = List::FindNode(%list, %elm);
	if (%node == "")
		return;
	List::DeleteSorted(%list, %node, %sort);
	List::InsertSorted(%list, %node, %sort);
	}

function List::Add(%list, %elm) {
	if (List::FindNode(%list, %elm) != "")
		return false;
	%node = List::PopFree(%list, %elm);
	for (%i = 0; %i < $List::[%list, sorts]; %i++)
		List::InsertSorted(%list, %node, $List::[%list, sort, %i]);
	return true;
	}
function List::Remove(%list, %elm) {
	%node = List::FindNode(%list, %elm);
	if (%node == "")
		return false;

	for (%i = 0; %i < $List::[%list, sorts]; %i++)
		List::DeleteSorted(%list, %node, $List::[%list, sort, %i]);
	List::PushFree(%list, %node);

	deleteVariables("$List::"@%list@"_property_"@%elm@"*");
	return true;
	}
function List::SetProperty(%list, %elm, %prop, %value) {
	$List::[%list, property, %elm, %prop] = %value;
	}
function List::GetProperty(%list, %elm, %prop) {
	return $List::[%list, property, %elm, %prop];
	}

function List::Count(%list) {
	return $List::[%list, count];
	}
function List::CallSorted(%list, %sort, %func, %arg0,%arg1,%arg2,%arg3,%arg4,%arg5,%arg6,%arg7,%arg8,%arg9) {
	%node = $List::[%list, first, %sort];
	if (%node == "")
		return;

	while (%node != -1) {
		%elm = List::GetNode(%list, %node);
		eval(%func@"(%list,%elm,%arg0,%arg1,%arg2,%arg3,%arg4,%arg5,%arg6,%arg7,%arg8,%arg9);");
		%node = $List::[%list, node,  %node, next, %sort];
		}
	}
function List::DefaultSort(%list, %a,%b) {
	return 0;	// Everything is equal:  Elements will be reported in insertion order.
	}

function temp() {
	// forward order sort
	function temp_sort(%a,%b) {if (%a < %b) return -1; if (%a > %b) return 1; return 0; }
	// reverse order sort
	function temp_sort2(%a,%b) {if (%a < %b) return 1; if (%a > %b) return -1; return 0; }

	List::New(listTeam);
	List::NewSort(listTeam, numerical, temp_sort);
	List::NewSort(listTeam, numerical2, temp_sort2);
	List::Add(listTeam, 10);
	List::Add(listTeam, 5);
	List::Add(listTeam, 16);
	List::Add(listTeam, 8);
	List::Add(listTeam, 30);
	List::CallSorted(listTeam, numerical, echo);
	List::CallSorted(listTeam, numerical2, echo);
	List::Remove(listTeam, 16);
	List::Remove(listTeam, 8);
	List::Remove(listTeam, 10);
	List::Add(listTeam, 14);
	List::CallSorted(listTeam, numerical, echo);
	List::CallSorted(listTeam, numerical2, echo);
	}
// ---------------------------------------------------------------------------
