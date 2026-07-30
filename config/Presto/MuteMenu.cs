// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// MuteMenu.CS								Presto, April '99 
//
//	A new menu for enacting client-side mutes
//
//	This menu, by default bound to control M, lets the user interactively
//	change his muting settings.  From this menu you can add mutings of type
//	"noise", "repeat", "all", and "pack" (if any packs are installed.)
//	You can also repeat the action to turn off the mute.
//
//	Finally, the last five players to speak are always displayed in the
//	menu.  This lets you assign client mutings to those players, or
//	remove them.  There's also a separate menu for undoing client-side
//	mutes (in case the client is no longer on your most-recent-speakers
//	list.
//
//	Sorry, there's no way to add a client muting to someone who hasn't yet
//	spoken.  But what would be the point? :)
//
//	This menu is the first example of my new Menu:: routines.  I haven't
//	really documented them yet, but you can see examples within. They let
//	you refer to menus by letter rather than by order of insertion...
//
// ---------------------------------------------------------------------------
Include("presto\\Menu.cs");
Include("presto\\Mute.cs");
Include("presto\\Event.cs");

function MuteMenu::ClearMuteListClient(%list, %client) {
	Mute::Clear(client, %client);
	}
function MuteMenu::ClearMuteList() {
	if (List::Count(listMutes) > 0)
		List::CallSorted(listMutes, sortMutes, MuteMenu::ClearMuteListClient);
	List::New(listMutes);
	List::NewSort(listMutes, sortMutes, MuteMenu::Sort);
	}
function MuteMenu::ClearAll() {
	Mute::Clear(all,"");
	Mute::Clear(team,friendly);
	Mute::Clear(team,enemy);
	MuteMenu::ClearMuteList();
	}

function MuteMenu::Clear(%client) {
	Mute::Clear(client, %client);
	List::Remove(listMutes, %client);
	}
function MuteMenu::Remove(%class, %member, %kind) {
	Mute::Remove(%class,%member,%kind);
	if (%class == client && Mute::IsEmpty(client, %member))
		List::Remove(listMutes, %member);
	}
function MuteMenu::Add(%class, %member, %kind) {
	if (%class == client)
		List::Add(listMutes, %member);
	Mute::Add(%class,%member,%kind);
	}
function MuteMenu::Target(%class, %member) {
	$MuteMenu::class = %class;
	$MuteMenu::member = %member;
	Menu::Display(menuMuteTarget);
	}
function MuteMenu::Mute(%kind) {
	%class = $MuteMenu::class;
	%member = $MuteMenu::member;
	if (Mute::IsMuted(%class, %member, %kind))
		MuteMenu::Remove(%class, %member, %kind);
	else	MuteMenu::Add(%class, %member, %kind);
	}

function InitMuteMenu() {
	for (%i=0; %i < $MuteMenu::last; %i++) {
		%last = $MuteMenu::last[%i];

		%letter = (%i + 1);
		if (%last != "" && (Client::GetName(%last)!= "")) {
			Menu::SetText(menuMute, %letter, Client::GetName(%last));
			Menu::SetAction(menuMute, %letter, "MuteMenu::Target(client,"@%last@");");
			Menu::SetEnabled(menuMute, %letter, true);
			}
		else	Menu::SetEnabled(menuMute, %letter, false);
		}
	Menu::SetEnabled(menuMute, "u", List::Count(listMutes) > 0);
	}

function MuteMenu::AddToUnMuteMenu(%list, %client) {
	%letter = String::GetSubStr("123456789abcdefghijklmnopqrstuvwxy", $MuteMenu::orderInList,1);
	if (%letter != "") {
		$MuteMenu::orderInList++;
		Menu::AddLetter(menuUnMute, %letter, Client::GetName(%client), "MuteMenu::Clear("@%client@");");
		}
	}
function MuteMenu::UnMuteMenu() {
	Menu::New(menuUnMute, "Un-mute player");
	$MuteMenu::orderInList = 0;
	List::CallSorted(listMutes, sortMutes, MuteMenu::AddToUnMuteMenu);
	Menu::AddLetter(menuUnMute, "z", "(all players!)", "MuteMenu::ClearMuteList();");
	Menu::Display(menuUnMute);
	}

function InitMuteTargetMenuText(%letter, %kind) {
	if (Mute::IsMuted($MuteMenu::class, $MuteMenu::member, %kind))
		Menu::SetText(menuMuteTarget, %letter, "Unmute "@%kind);
	else	Menu::SetText(menuMuteTarget, %letter, "Mute "@%kind);
	Menu::SetEnabled(menuMuteTarget, "p", $Mute::packs > 0);
	}
function InitMuteTargetMenu() {
	InitMuteTargetMenuText("a", all);
	InitMuteTargetMenuText("n", noise);
	InitMuteTargetMenuText("r", repeat);
	InitMuteTargetMenuText("p", packs);
	}
Menu::New(menuMuteTarget, "Mute Target", "InitMuteTargetMenu();");
 Menu::AddLetter(menuMuteTarget, "a","All", "MuteMenu::Mute(all);");
 Menu::AddLetter(menuMuteTarget, "n","Noises", "MuteMenu::Mute(noise);");
 Menu::AddLetter(menuMuteTarget, "r","Repeats", "MuteMenu::Mute(repeat);");
 Menu::AddLetter(menuMuteTarget, "p","Packs", "MuteMenu::Mute(packs);");

function MuteMenu::OnClientMessage(%client, %msg) {
	if (%client == 0 || %client == getManagerId())
		return;
 
	for (%i = 0; %i < $MuteMenu::last; %i++)
		if (%client == $MuteMenu::last[%i])
			break;
	while (%i != 0) {
		$MuteMenu::last[%i] = $MuteMenu::last[%i-1];
		%i--;
		}
	$MuteMenu::last[0] = %client;
	return;
	}

function MuteMenu::Sort(%list, %clientA, %clientB) {
	%nameA = Client::GetName(%clientA);
	%nameB = Client::GetName(%clientB);
	return String::NCompare(%nameA,%nameB,10000);
	}
function MuteMenu::Reset() {
	deleteVariables("$MuteMenu::*");
	$MuteMenu::last = 5;
	MuteMenu::ClearMuteList();

	// create it here since this is dependant on #last
	Menu::New(menuMute, "Mute", "InitMuteMenu();", true);
 	 for (%i = 1;%i <= $MuteMenu::last; %i++)	
		Menu::AddLetter(menuMute, %i);
	 Menu::AddLetter(menuMute, "a", "All", "MuteMenu::Target(all);");
	 Menu::AddLetter(menuMute, "e", "Enemy team", "MuteMenu::Target(team, enemy);");
	 Menu::AddLetter(menuMute, "f", "Friendly team", "MuteMenu::Target(team, friendly);");
	 Menu::AddLetter(menuMute, "u", "Un-mute player...", "MuteMenu::UnMuteMenu();");
	 Menu::AddLetter(menuMute, "z", "Clear all mutes", "MuteMenu::ClearAll();");
	}

function MuteMenu::Init() {
	Event::Attach(eventClientMessage, MuteMenu::OnClientMessage);
	Event::Attach(eventConnected, MuteMenu::Reset);

	MuteMenu::Reset();
	}

if (Presto::Enabled(MuteMenu)) {
	MuteMenu::Init();
	}

// ---------------------------------------------------------------------------