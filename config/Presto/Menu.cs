// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// Menu.CS									Presto, March '99 
//
//	A generic interface for creating on-screen one-key menus.
//
//	It was kind of unfair that Dynamix only gave us one menu to customize,
//	the V chat menu.  I wanted to write multipart commands, so I took 
//	their idea and wrote these menuing functions.
//
//	Because this function destroys the existing V chat menu, if you
//	load this you also have to load CHAT.CS, my reimplemented V chat
//	menu.
//
//	Usage examples:
//
//		Menu::New(menuPresto, "Presto menu:");
//			This creates a new menu called menuPresto.  When the menu
//			is displayed it will show the title "Presto menu:"
//
//		Menu::AddChoice(menuPresto, "hHello", "echo(hello);");
//			Adds a menu choice labelled "Hello".  The first letter 'h'
//			indicates that the user will press H to pick this choice.
//			When he does, it will run the code "echo(hello);" 
//		Menu::AddChoice(menuPresto, "gGoodbye", "echo(\"good bye\");");
//			Note that you have to properly quote strings in the
//			command.  (\")
//		Menu::AddChoice(menuPresto, "q", "quit();");
//			Because this choice has only a single letter, it will
//			be invisible on the screen.  But if the user presses
//			Q he will quit.
//		Menu::AddMenu(menuPresto, "m", menuOther);
//			If the user hits M he will be directed to a second menu.
//			Technically you could call this a submenu, but there's
//			nothing that says you can't jump back to menuPresto
//			with some key in menuOther.
//			The default name of menuOther will be used as the
//			menu text in menuPresto.
//		Menu::AddMenu(menuPresto, "zZap!", menuZap);
//			Just like above, but this time I specified a label to
//			use instead of the default name of menuZap.
//
//		Menu::Display(menuPresto);
//			This displays the menu on the screen.  As soon as it's
//			displayed, the keys are active.
//
//	Initialization function example:
//
//		function InitMyMenu()
//		{
//		Menu::SetEnabled(menuPresto, 0, false);
//			Disables item 0 (they're numbered in add order).  When
//			an item is disabled it will not appear on the screen
//			at all.
//		Menu::SetEnabled(menuPresto, 1, true);
//			You don't really have to do this because all items
//			are enabled by default.
//		}
//
//	This would then be attached to the menu by calling
//
//		Menu::New(menuPresto, "Presto menu:", "InitMyMenu();");
//			Works just like the other Menu::New example above,
//			but before the menu is displayed it will call
//			InitMyMenu();
//
// ---------------------------------------------------------------------------
Include("presto\\List.cs");

function Menu::GetNumChoices(%menu) {
	return List::Count(%menu);
	}
function Menu::SetTitle(%menu, %title) {
	$Menu::[%menu, title] = %title;
	}
function Menu::GetTitle(%menu) {
	return $Menu::[%menu, title];
	}
function Menu::SetInitAction(%menu, %init) {
	$Menu::[%menu, init] = %init;
	}
function Menu::GetInitAction(%menu) {
	return $Menu::[%menu, init];
	}
function Menu::SetAutoEntry(%menu, %auto) {
	$Menu::[%menu, auto] = %auto;
	}
function Menu::GetAutoEntry(%menu) {
	return $Menu::[%menu, auto];
	}

function Menu::SetEnabled(%menu, %entry, %flag) {
	List::SetProperty(%menu, %entry, enabled, %flag);
	}
function Menu::GetEnabled(%menu, %entry) {
	return List::GetProperty(%menu, %entry, enabled);
	}

function Menu::GetLetter(%menu, %entry) {
	return List::GetProperty(%menu, %entry, letter);
	}
function Menu::SetLetter(%menu, %entry, %letter) {
	if (%letter ==  Menu::GetLetter(%menu, %entry))
		return;
	List::SetProperty(%menu, %entry, letter, %letter);
	List::Resort(%menu, %entry, byLetter);
	}

function Menu::GetText(%menu, %entry) {
	return List::GetProperty(%menu, %entry, text);
	}
function Menu::SetText(%menu, %entry, %text) {
	if (%text ==  Menu::GetText(%menu, %entry))
		return;
	List::SetProperty(%menu, %entry, text, %text);
	List::Resort(%menu, %entry, byText);
	}

function Menu::SetChoice(%menu, %entry, %choice) {
	Menu::SetLetter(%menu, %entry, String::GetSubStr(%choice,0,1));
	Menu::SetText(%menu, %entry, String::GetSubStr(%choice,1,10000));
	}
function Menu::GetChoice(%menu, %entry) {
	return Menu::GetLetter(%menu, %entry) @ Menu::GetText(%menu, %entry);
	}

function Menu::SetAction(%menu, %entry, %action) {
	List::SetProperty(%menu, %entry, action, %action);
	}
function Menu::GetAction(%menu, %entry) {
	return List::GetProperty(%menu, %entry, action);
	}

function Menu::SortText(%menu, %a,%b) {
	%textA = Menu::GetText(%menu,%a);
	%textB = Menu::GetText(%menu,%b);
	return String::NCompare(%textA,%textB,10000);
	}
function Menu::SortLetter(%menu, %a,%b) {
	%letterA = Menu::GetLetter(%menu,%a);
	%letterB = Menu::GetLetter(%menu,%b);
	return String::NCompare(%letterA,%letterB,1);
	}
function Menu::New(%menu, %title, %init, %autoEntry) {
	List::New(%menu);

	Menu::SetTitle(%menu, %title);
	Menu::SetAutoEntry(%menu, %autoEntry);
	Menu::SetInitAction(%menu, %init);
	List::NewSort(%menu, byEntry, "");
	List::NewSort(%menu, byText, Menu::SortText);
	List::NewSort(%menu, byLetter, Menu::SortLetter);
	}

function Menu::AddChoice(%menu, %choice, %action) {
	%letter = String::GetSubStr(%choice, 0, 1);
	%text = String::GetSubStr(%choice, 1, 10000);

	if (Menu::GetAutoEntry(%menu) == true)
		Menu::AddLetter(%menu, %letter, %text, %action);
	else	Menu::AddEntry(%menu, Menu::GetNumChoices(%menu), %letter, %text, %action);
	}
function Menu::AddLetter(%menu, %letter, %text, %action) {
	Menu::AddEntry(%menu, %letter, %letter, %text, %action);
	}
function Menu::AddEntry(%menu, %entry, %letter, %text, %action) {
	Menu::SetEnabled(%menu, %entry, true);
	Menu::SetLetter(%menu, %entry, %letter);
	Menu::SetText(%menu, %entry, %text);
	Menu::SetAction(%menu, %entry, %action);

	List::Add(%menu, %entry);
	return %entry;
	}
function Menu::AddMenu(%menu, %choice, %submenu) {
	if (String::GetSubStr(%choice, 1,1) == "") {
		%title = Menu::GetTitle(%submenu);
		if (%title == "")
			%title = "...";
		%choice = %choice @ %title;
		}
	return Menu::AddChoice(%menu, %choice, "Menu::Display("@%submenu@");");
	}

function Menu::Choose(%menu, %entry) {
	SetCMMode(PlayChatMenu, 0);
	SetCMMode(PlayChatMenuInvisible, 0);
	eval(Menu::GetAction(%menu, %entry));
	}
function Menu::AddToRealMenu(%menu, %entry) {
	if (!Menu::GetEnabled(%menu, %entry)) {
		Menu::SetEnabled(%menu, %entry, true);
		return;
		}

	%letter = Menu::GetLetter(%menu, %entry);
	%text = Menu::GetText(%menu, %entry);
	if (%text == "")
		addCMCommand(PlayChatMenuInvisible, %letter @ %entry, Menu::Choose, %menu, %entry);
	else	addCMCommand(PlayChatMenuVisible, %letter @ %text, Menu::Choose, %menu, %entry);
	}

function Menu::DisplayDefault() {
	if (isObject(OldPlayChatMenu)) {
		if (isObject(PlayChatMenu))
			DeleteObject(PlayChatMenu);
		renameObject(OldPlayChatMenu, PlayChatMenu);
		}
	setCMMode(PlayChatMenu,2);
	}
function Menu::Display(%menu, %sort) {
	if (isObject(PlayChatMenuVisible)) {
		setCMMode(PlayChatMenuVisible, 0);
		deleteObject(PlayChatMenuVisible);
		}
	if (isObject(PlayChatMenuInvisible)) {
		setCMMode(PlayChatMenuInvisible, 0);
		deleteObject(PlayChatMenuInvisible);
		}

	eval(Menu::GetInitAction(%menu));
	newObject(PlayChatMenuVisible, ChatMenu, Menu::GetTitle(%menu)@":");
	newObject(PlayChatMenuInvisible, ChatMenu, Menu::GetTitle(%menu)@"(invisible):");

	if (!List::ValidSort(%menu, %sort))
		%sort = byEntry;
	List::CallSorted(%menu, %sort, Menu::AddToRealMenu);

	// why schedule?  Because we might be in the middle of closing another menu when we
	// display this one.  The GUI gets confused when we delete the old one out from
	// under it -- and closes the one we just created instead!
	// Instead, I just schedule the menu to be created right after this call returns.
	SetCMMode(PlayChatMenuVisible, 2);
	SetCMMode(PlayChatMenuInvisible, 2);
	schedule("menu::fix();", 0);
	}
function Menu::Fix() {
	if (!isObject(PlayChatMenuVisible))
		return;
	if (isObject(PlayChatMenu)) {
		if (isObject(OldPlayChatMenu))
			deleteObject(PlayChatMenu);
		else	renameObject(PlayChatMenu, OldPlayChatMenu);
		}
	renameObject(PlayChatMenuVisible, PlayChatMenu);
	}

// need a function called Menu::EnterMode( ) which will only create an
// invisible menu - lets you add new keymaps!  sorta.

Include("presto\\chat.cs");	// Once you use menus, you have to use the new chat menu.

// ---------------------------------------------------------------------------
