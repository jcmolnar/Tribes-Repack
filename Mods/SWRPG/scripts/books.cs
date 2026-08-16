//Began: 5:41 PM, Tuesday, August 14th, 2007.

function remoteVoteYes(%clientId) //Moved Vote Yes/No here for convenience.
{
	echo("did the function");
	if(%clientId.book != "" && %clientId.reading == 1)
	{
		%page = %clientId.page;
		if(%page == $Book[%clientId.book, p])
			bottomprint(%client, "", 1);
		else
		{
	echo("did the else");
			if(%page == $Book[%clientId.book, p])
				%ptag = "\n <jl><" @ %page - 1 @ "<jc>[" @ %page @ "]<jr>x";
			else if(%page == 1)
				%ptag = "\n <jl>x<jc>[" @ %page @ "]<jr>" @ %page + 1 @ ">";
			else
				%ptag = "\n <jl><" @ %page - 1 @ "<jc>[" @ %page @ "]<jr>" @ %page + 1 @ ">";
			bottomprint(%client, $Book[%clientId.book, %clientId.page + 1] @ %ptag, 999);
		}
	}
	//else if(%clientId.instructions screen prompt thing != "")
		//{}//do stuff.
	else
	{
		%clientId.vote = "yes";
		centerprint(%clientId, "", 0);
	}
}

function remoteVoteNo(%clientId)
{
	if(%clientId.book != "" && %clientId.reading == 1)
	{
		%page = %clientId.page;
		if(%page == 1)
			bottomprint(%client, "", 1);
		else
		{
			if(%page == $Book[%clientId.book, p])
				%ptag = "\n <jl><" @ %page - 1 @ "<jc>[" @ %page @ "]<jr>x";
			else if(%page == 1)
				%ptag = "\n <jl>x<jc>[" @ %page @ "]<jr>" @ %page + 1 @ ">";
			else
				%ptag = "\n <jl><" @ %page - 1 @ "<jc>[" @ %page @ "]<jr>" @ %page + 1 @ ">";
			bottomprint(%client, $Book[%clientId.book, %clientId.page - 1] @ %ptag, 999);
		}
	}
	//else if(%clientId.instructions screen prompt thing != "")
		//{}//do more stuff.
	else
	{
		%clientId.vote = "no";
		centerprint(%clientId, "", 0);
	}
}

function omgbook(%clientId)
{
	%clientid.reading = 1;
	%clientid.book = 1;
	%clientId.page = 1;
	bottomprint(%clientId, $Book[%clientId.book, 1] @ %ptag, 999);

}


function ToggleBook(%clientId)
{
	if(%clientId.reading == 1)
	{
		%clientId.reading = 0;
		%clientId.book = ""; %clientId.page = "";
		centerprint(%clientId, "", 0);
	}
	else if(%clientId.reading == 0)
	{
		%clientId.reading = 1;
		CreateBookMenu(%clientId);
	}

}

function CreateBookMenu(%clientId, %page)
{
	dbecho($dbechoMode, "CreateBookMenu(" @ %clientId @ ", " @ %page @ ")");

	Client::buildMenu(%clientId, "Here are your books", "Book", true);

	%clientId.bulkNum = "";

	%l = 6;
	%ns = CountObjInCommaList(FetchData(%ClientId, "Books"));
	%np = floor(%ns / %l);
	
	%lb = (%page * %l) - (%l-1);
	%ub = %lb + (%l-1);
	if(%ub > %ns)
		%ub = %ns;

//	for(%i = %lb; %i <= %ub && %i <= %ns; %doesntserveapurpose++)
	%i = %lb;
	while(%i <= %ub && %i <= %ns)
		if(HasBook(%clientId, %i))
		{
			Client::addMenuItem(%clientId, %cnt++ @ "$Book[%i]", %i @ " " @ %page);
			%i++;
		}
		if(%i > $NumBooks) {
			break; messegeall(1, "Error #1 with the books (list generator). Please tell Hazor."); }

	if(%page == 1)
	{
		Client::addMenuItem(%clientId, "nNext >>", "page " @ %page+1);
		Client::addMenuItem(%clientId, "xDone", "done");
	}
	else if(%page == %np)//+1)
	{
		Client::addMenuItem(%clientId, "p<< Prev", "page " @ %page-1);
		Client::addMenuItem(%clientId, "xDone", "done");
	}
	else
	{
		Client::addMenuItem(%clientId, "nNext >>", "page " @ %page+1);
		Client::addMenuItem(%clientId, "p<< Prev", "page " @ %page-1);
	}

	return;
}
function processMenuBook(%clientId, %opt)
{
	dbecho($dbechoMode, "processMenusp(" @ %clientId @ ", " @ %opt @ ")");

	%o = GetWord(%opt, 0);
	%p = GetWord(%opt, 1);

	if(%o != "page" && %o != "done")
	{
		%clientid.reading = 1;
		%clientid.book = %o;
		%clientId.page = 1;
		bottomprint(%clientId, $Book[%clientId.book, 1] @ %ptag, 999);

	}

	if(%o != "done")
		CreateBookMenu(%clientId, %p);
}

function GiveBook(%client, %number)
{
	if($Book[%number] == "" || HasBook(%client, %number) == "True") return;
	client::sendMessage(%client, 0, "You now have the \"" @ $Book[%number] @ "\" book.");
	AddToCommaList($ClientData[%client, "Books"], %number);
}
function AddBook(%client, %number)
{
	GiveBook(%client, %number);
}

function TakeBook(%client, %number)
{
	RemoveFromCommaList($ClientData[%client, "Book"], %number);
}
function RemoveBook(%client, %number)
{
	TakeBook(%client, %number);
}

function HasBook(%client, %number)
{
	if(IsInCommaList($ClientData[%client, "Books"], %number))
		return True;
	else
		return False;
}

function MenuSmithList(%clientId, %page)
{
	dbecho($dbechoMode, "MenuSP(" @ %clientId @ ", " @ %page @ ")");

	Client::buildMenu(%clientId, "Select an item:", "sl", true);

	%clientId.bulkNum = "";

	%l = 6;
	%ns = $SmithCombos;
	%np = floor(%ns / %l);
	
	%lb = (%page * %l) - (%l-1);
	%ub = %lb + (%l-1);
	if(%ub > %ns)
		%ub = %ns;

	for(%i = %lb; %i <= %ub; %i++)
		Client::addMenuItem(%clientId, %cnt++ @ getword($SmithComboResult[%i], 0).description, %i @ " " @ %page);

	if(%page == 1)
	{
		Client::addMenuItem(%clientId, "nNext >>", "page " @ %page+1);
		Client::addMenuItem(%clientId, "xBack to books...", "done");
	}
	else if(%page == %np+1)
	{
		Client::addMenuItem(%clientId, "p<< Prev", "page " @ %page-1);
		Client::addMenuItem(%clientId, "xBack to books...", "done");
	}
	else
	{
		Client::addMenuItem(%clientId, "nNext >>", "page " @ %page+1);
		Client::addMenuItem(%clientId, "p<< Prev", "page " @ %page-1);
	}

	return;
}
function processMenuSL(%clientId, %opt)
{
	dbecho($dbechoMode, "processMenuSL(" @ %clientId @ ", " @ %opt @ ")");

	%o = GetWord(%opt, 0);
	%p = GetWord(%opt, 1);

	if(%o == "done")
	{
		processMenuselectbelt(%clientId, books);
		return;
	}
	else if(%o != "page")
		bottomprint(%clientId, "<jc><f1>" @ $SmithComboResult[%o] @ " <f0>: <f1>" @ $SmithCombo[%o]);
	MenuSmithList(%clientId, %p);
}

function MenuSpellList(%clientId, %page)
{
	dbecho($dbechoMode, "MenuSpellList(" @ %clientId @ ", " @ %page @ ")");

	if(%page == "") %page = 1;

	Client::buildMenu(%clientId, "Select a spell:", "SS", true);

	%clientId.bulkNum = "";

	%f = GetWord(%page, 1);
	%page = GetWord(%page, 0);

	%l = 6;
	%ns = 0;
	for(%i = 1; %i < $si; %i++) // $si = number of spells. See force_neutral.cs
		if(%f == $SkillType[$Spell::keyword[%i]])
			%ns++;
	%np = floor(%ns / %l);
	
	%lb = (%page * %l) - (%l-1);
	%ub = %lb + (%l-1);
	if(%ub > %ns)
		%ub = %ns;

	%c = 0;
	for(%i = %lb; %i <= %ub; %i++)
		//if(%f == $SkillType[$Spell::keyword[%i]])
		//{
			Client::addMenuItem(%clientId, %cnt++ @ $Spell::keyword[%i], %i @ " " @ %page @ " " @ %f);
		//	%c++;
		//}

	if(%page == 1)
	{
		Client::addMenuItem(%clientId, "nNext >>", "page " @ %page+1);
		Client::addMenuItem(%clientId, "xBack to books...", "done");
	}
	else if(%page == %np+1)
	{
		Client::addMenuItem(%clientId, "p<< Prev", "page " @ %page-1);
		Client::addMenuItem(%clientId, "xBack to books...", "done");
	}
	else
	{
		Client::addMenuItem(%clientId, "nNext >>", "page " @ %page+1);
		Client::addMenuItem(%clientId, "p<< Prev", "page " @ %page-1);
	}

	return;
}
function processMenuSS(%clientId, %opt)
{
	dbecho($dbechoMode, "processMenuSS(" @ %clientId @ ", " @ %opt @ ")");

	%o = GetWord(%opt, 0);
	%p = GetWord(%opt, 1);
	%f = GetWord(%opt, 2);

	if(%o == "done")
	{
		processMenuselectbelt(%clientId, books);
		return;
	}
	else if(%o != "page")
	{
		%msg = WhatIs(GetWord($Spell::keyword[%o], 0));
		bottomprint(%clientId, %msg, floor(String::len(%msg) / 20));
	}
	MenuSpellList(%clientId, %p @ " " @ %f);
}

//$book[x] is the book title
//$book[x, p] is the number of pages in the book (I'll probably make this unnecessary)
//$book[x, y] is each of the pages, where y is the number of the page, just see below for examples.

$NumBooks = 2;

%p = 1;
$book[1] = "Conquests of Nevmin the Cookie Destroyer";
$book[1, %p++] = "Long ago, in a land far from the shores of Nearfar, there was a conquerer called Nevmin. Many people in the land of Farnear called him the Cookie Destroyer, for he destroyed many cookies.";
$book[1, %p++] = "Nevmin delighted in destroying cookies, because it made all of the little children cry. The sorrow-filled cries of all the children made a gnome named Lollipop become sorrowful";
$book[1, %p++] = "Lollipop the gnome decided that he was going to put an end to Nevmin's cookie destruction, so as to let the children be happy and free again(Because no cookies means no freedom. Somehow. It's sensical.";
$book[1, %p++] = "So Lollipop ventured in the direction of Nevmin's evil keep, which was constructed with brownie bricks by enslaved brownies. Upon his arrival, Lollipop realised that he had no weapons.";
$book[1, %p++] = "So he went and got an enormous container of milk, and started spraying it on the brownie-castle with a water gun. The brownies began softening and disintegrating, and eventually a wall collapsed.";
$book[1, %p++] = "Then Lollipop was roasted by Nevmin and given to the Draegoni for a snack.";
$book[1, %p++] = "The end.";
$book[1, p] = %p;

%p = 1;
$Book[2] = "Woot";
$book[2, %p++] = "omg yayness";
$book[2, %p++] = "Ain't it leet, man?";
$book[2, %p++] = "You bet.";
$Book[2, p] = %p;
//wth? :/
$omghi = "seriously.";