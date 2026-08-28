// !! CmdHUD 1.3a -> Commander/Inventory/Inv. Station LOOK-AROUND <- 5/9/99 !!
// 
// Original CmdHUD code, and -idea- by:
//    Zear
//    zear@planetstarsiege.com
//    http://www.cetisp.com/~thammock/scripts/
// 
// InvHUD, CmdHUD rework, additional code by:
//    Cowboy
//    cowboy@planetstarsiege.com
//    http://www.planetstarsiege.com/cowboy/
//    ICQ# 31184463
// 
// CmdHUD 1.3a: got fed up, and got rid of the disappearing/reappearing Station
//             inventory screen! (it's always on now), small caveat, but this
//             way it doesn't interfere with Writer's fast_favorites.cs
//
//      NOTE:  If you want to, you can even just install the 2 .gui files as
//             mentioned, and not install the CmdHUD.cs script (if you like the
//             way the GUIs look, but don't want the script installed...
//
//             MAKE SURE you set $PrestoPref::InvCamera = false in pretoprefs.cs
//
// CmdHUD 1.2: more bug fixes, added Teeni-Cam! This little HUD (inspired by
//             CamHUD) displays a teeni playGui screen under the green Command
//             panel when you have CmdHUD's full-size map displayed!
//
// CmdHUD 1.1: lotsa bug-fixes, added $CmdHUD::DebugMode setting
//
// Because of the way Writer's fast_favorites.cs works, sometimes you have to
// press 'i' twice to view the inventory screen while at a station. This has to
// do with the way he uses remoteval(2048, GuiMode). (maybe he can fix this?)
//
// ********************> NOW SUPPORTS RESIZE-ON-THE-FLY! <*********************
//
// Well, I saw what Zear did with his CmdHUD, so I figured I'd do up an InvHUD
// so people can have a much larger view around them at an inventory station!
// Then, I looked at the CmdHUD and cleaned it up a little, and also made some
// of the windows the same size/location between the 2 GUIs.
//
// Next was trying to get it to resize! I got everything to resize, but much to
// my dismay, every time you resized the window, all the playGui objects (clock
// display, sensor ping, compass, etc) would do a 'snap to edge' and get fully
// messed up. Presto had an idea: to place all of these 'wandering Huds' in a
// separate frame, that wouldn't resize like the playGui. I had already tried
// this but I didn't succeed.. but after a few days, I tried it again, because
// I knew if I didn't do it, someone else might have and _they_ would get all
// the credit... and lo! It worked! Wooooo baby! :)
//
// Then.. people wanted to be able to use both the new small map AND the large
// one from the original command.gui, I tried many different things.. i had it
// so that it actually GuiPushDialog()'ed the old Command Gui screen onto the
// MainWindow, but this was unreliable, not to mention _every_ time you wanted
// to go back to that it had to be reloaded and rebuilt (even if it was the
// last map displayed). So then I had the script create 2 FearGui::TSCommander
// objects, one for each map size, in Simgui::Control objects, but the large
// map couldn't resize, and if you made the window too small it would crash
// Tribes (and although this worked fine for me and other people, i wanted it
// to be able to FULLY resize on-the-fly).. So what I finally did is code into
// the Gui 2 different Simgui::Control objects (one for each map, both opaque,
// and the large one resizes) then I add the appropriate FearGui::TSCommander
// maps into each window and rebuild.
//
// The real trouble with that is that Tribes wouldn't give me the Object IDs
// of the Simgui::Control objects (even though I named them, and knew where
// they were) This was very frustrating.. I was ready to call it quits when
// Presto suggested that since Simgui::Control objects are also sets (where
// the objects it owns are the members), maybe I could get individual IDs of
// members of a set whose ID I _could+ get (CommandGui). This worked! so, here
// ya go!
//
// NOTE: The Zoom/Center buttons and Zoom key do not work on the Commander Map!
//       I don't know why, but they crash Tribes. if you know how to fix this,
//       let me know ASAP! - but for now I disabled both the bottons and the
//       'z' key in the Commander screen. Sorry!
//
// What I did is take the 'center' button and change the binding to allow you
// to change to Free Look / shut off the Commander Panel by clicking it...
// 
// Also, because the key bindings have changed, you might want to go through
// your config.cs file and delete all the ones that were used by version 0.98
// or older of CmdHUD.
//
// Requires PrestoHUD, tested with 0.93! Also, this replaces Zear's CmdHUD.
// Please do not run _both_ this CmdHUD and his old version concurrently!
// 
// (ok?)
// 
// I think I'm out of 'beta' here.. so Enjoy!
// 
// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
// 
// Installation notes:
// 
// Unzip *.gui into the tribes\gui directory. If you don't have this directory,
// create it! Unzip all other files into \config\cowboy directory and add line:
// Include("cowboy\\CmdHUD.cs"); to your autoexec.cs file somewhere AFTER the
// exec("presto\\install.cs"); line.
// 
// Also, set $PrestoPref::InvCamera = false in Presto's prestoprefs.cs!
//
// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
// 
// This was originally done by Zear, as was the idea. I want to take no credit
// for the originality of it! I _did_ however do the Inventory screen... but 
// only by first using his as a template...
// 
// (c) 1999 Zear/Cowboy, whatever... I hope you enjoy this, and all that i ask
// is that you give Zear props for it.. He's the man... enjoy!
//
// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//
// User settings - please read this section carefully!
//
// There are so many settings! Well - I got so many suggestions, I wanted to 
// implement as many as I could... I'll explain the best I can...
//
// Note: Clicking on the Commander Gui's Commander buton (top left button on
//       the CmdHUD screen) will toggle map sizes now!
//
//       For both Commander and Inventory screens, pressing <ENTER> while
//       displayed toggles between mouse cursor/movement modes. you can change
//       this and other key bindings below.
//
// I basically re-did the way the key bindings work. Now, anyone with a little
// experience (or anyone who can follow instructions) can bind _ANY_ key to
// _ANY_ map size/mouse mode. This is something I personally wanted to do,
// because I wanted to first of all use c, control-c, and alt-c for my map
// functions, and didn't ever even want to use the 'Default' mode (although
// some of you out there might) - you can see my personal key bindings after
// the others...
//
// This way offers _far_ more flexibilty and custimization. Hope you like it!
//
// ** bindCommand commands are in the format: **
//
// bindcommand(keyboard0, make, [modifer key,] "key", TO "function();");
//
// Where modifier key is optional and can be control, alt, etc.. "key" is
// the key to assign (in quotes), and "function();" can be one of:
//
// "CmdHUD::turnOn(%size, %mouse);"
// "InvHUD::turnOn(%mouse);"
//
// where %size is either:
//    Default (remember last size used)
//    Large (large size)
//    Small (small size)
//
// and %mouse is either:
//    0 (remember last mode used)
//    1 (open CmdHUDs in Mouse Cursor mode)
//    2 (open CmdHUDs in Mouse Free Look mode)
//
// see below for examples...

EditActionMap("ActionMap.sae");

// View Commander screen, with last used map, in default mode 0
//bindCommand(keyboard0, make, "c", TO, "CmdHud::turnOn(Default, 0);");
//The previous key bind causes a glitch if the server disabled the command screen.
//This is the original "c" bind, placed here to force reset. --phantom
// NATIVE FIX 2026-08-27: REMOVED. saeModern.cs:70, saeBase.cs, saeRPG.cs and
// Mods\RPG\scripts\sae.cs all bind "c" to this exact command already, so this
// only ever re-stamped it -- destructively, over any key the player had rebound.
//bindCommand(keyboard0, make, "c", TO, "remoteEval(2048, ToggleCommandMode);");

// View Commander screen with Large Map, in Mouse Cursor mode 1
bindCommandDefault(keyboard0, make, "]", TO, "CmdHud::turnOn(Large, 1);");

// View Commander screen with Small Map, in Free Look mode 2
bindCommandDefault(keyboard0, make, "[", TO, "CmdHud::turnOn(Small, 2);");

// Key used to view Inventory Screen, "i" is the Tribes default. Here I open
// it in Default mode (0)
// NATIVE FIX 2026-08-27: REMOVED. Every preset binds "i" to
// remoteEval(2048, ToggleInventoryMode) -- the stock path the native gui-mode code in
// dlgPlay.cpp drives -- and this stomped all of them from autoexec.cs:10 on every boot.
// main.cpp:1946-1950 records the fallout: CmdHUD's private guiMode is not synchronised
// with a config-owned play GUI. The RPG server does not need the KEY: shopping.cs:41,87
// reaches this screen with remoteEval(%clientId, InvHUD::turnOn, 3), which only needs the
// function defined, and it still is.
//bindCommand(keyboard0, make, "i", TO, "InvHud::turnOn(0);");

// These are what *I* actually use for my command screen now...
// "c" opens the Small map in Free Look mode, "alt-c" opens the Large map
// in Mouse Cursor mode, "control-c" opens the Small map in mouse cursor mode,
// "i" opens the Inventory screen in Mouse Cursor mode, and "control-i" opens
// the Inventory screen in Free Look mode. (yeesh!)
//
// Notice that I don't even use the 'Default' map size or cursor modes...
//
// bindCommand(keyboard0, make, "c", TO, "CmdHud::turnOn(Small, 2);");
// bindCommand(keyboard0, make, alt, "c", TO, "CmdHud::turnOn(Large, 1);");
// bindCommand(keyboard0, make, control, "c", TO, "CmdHud::turnOn(Small, 1);");
// bindCommand(keyboard0, make, "i", TO, "InvHud::turnOn(1);");
// bindCommand(keyboard0, make, control, "i", TO, "InvHud::turnOn(2);");

// I don't have an Objectives GUI, but this is important to define
// NATIVE FIX 2026-08-27: REMOVED, same reason as "i" above -- every preset binds "o" to
// remoteEval(2048, ToggleObjectivesMode) and this overwrote it on every boot.
//bindCommand(keyboard0, make, "o", TO, "CmdHud::ObjectiveOn();");

// Key to toggle mouse cursor/movement modes
// NATIVE FIX 2026-08-27: NewActionMap CLEARS the map. Running from autoexec.cs:10 on every
// boot, it wiped the whole PDA map and left only the enter bind below -- which is why the
// z zoom binds every preset puts there (saeModern.cs:193-194, sae.cs:129-130) have never
// survived a single boot, and why the saved config.cs pdaMap block held nothing but enter.
// EditActionMap appends instead.
EditActionMap("pdaMap.sae");
bindCommandDefault(keyboard0, make, "enter", TO, "$CmdHUD::State = CmdHUD::Cursor(!$CmdHUD::State);");

//
// Other settings...
// 
$CmdHUD::CmdStaLarge = false;	// Do you want it to open a full-size map when
				// you enter a Command Station? true _forces_
				// the Large map, false uses the Last Used map

$CmdHUD::InvTurnAround = true;	// Wanna be turned around when entering these
$CmdHUD::CmdTurnAround = false;	// stations? (if true, you are required to set
				// $PrestoPref::CamHudTurnAround = true in
				// presto\\prestoprefs.cs)


$CmdHUD::MouseOffTime = 0.3;	// (time in seconds) Try making this larger if
				// The mouse is not shutting off when you
				// select Free Look mode or it's not accurately
				// changing the map sizes and updating them
				// (this shouldn't need to be changed)

$CmdHUD::InvDelay = 0.3;	// you shouldn't need to change this.

$CmdHUD::TeeniCam = true;	// enable/disable the CmdHUD Large Map TeeniCam

$CmdHUD::DebugMode = false;	// If you want to see how CmdHUD works, set this
				// to true and watch the console!

//
// If you want this to work right, use this setting to override what's in
// Presto's pack
//
$PrestoPref::CamHudTurnAround = false;

Include("presto\\Event.cs");
Include("presto\\Camhud.cs");
Include("presto\\Inventory.cs");

//
// For Presto Pack 0.93+, this displays script info in a box on the main
// menu screen, kind of an 'Advertisement/Info Box' for installed scripts.
//

if ($Presto::version >= 0.93) {
	Presto::AddScriptBanner(CmdHUD,	"<jc><f2>CmdHUD 1.3a <f1>Commander &\n" @
		"<f1>Inventory Sceen <f2>Resizing<f1> GUI\n" @
		"\n" @
		"<f0>Please see <f1>cowboy\\cmdhud.cs\n" @
		"<f0>for settings! Thanks for using\n"@
		"this, and good luck...\n" @
		"\n" @
		"<f1>Created by <f2>Zear<f1>, modified and\n" @
		"totally reworked by <f2>Cowboy<f1>...");
	}

function CmdHUD::DebugEcho(%string) {
	if ($CmdHUD::DebugMode)
		echo("CmdHUD: " @ %string);
	}

function CmdHUD::SetGui(%gui) {
	$CmdHUD::guiMode = %gui;
	CmdHUD::DebugEcho("$CmdHUD::guiMode = " @ $CmdHUD::guiMode);
	return %gui;
	}

function LobbyGui::onOpen()
{
	Control::setValue(LobbyServerName, $ServerName);
	Control::setValue(LobbyServerType, $ServerMod);
	Control::setValue(LobbyMissionName, $ServerMission);
	Control::setValue(LobbyMissionType, $ServerMissionType);
	Control::setValue(LobbyVersion, $ServerVersion);
	Control::setValue(LobbyAddress, $Server::Address);

	Control::setValue(LobbyServerText, $ServerInfo);
	Control::setValue(LobbyMissionDesc, $ServerText);

	Event::Trigger(eventGuiOpen, LobbyGui);
}

function CmdHUD::GuiOpen(%gui) {
	$CmdHUD::lastGuiOpen = $CmdHUD::guiOpen;
	if (%gui == playGui && $CmdHUD::lastGuiOpen != 6)
		$CmdHUD::guiOpen = 1;
	else if (%gui == CommandGui)
		$CmdHUD::guiOpen = 2;
	else if (%gui == CmdInventoryGui)
		$CmdHUD::guiOpen = 4;
	else if (%gui == LobbyGui)
		$CmdHUD::guiOpen = 6;

	if ($CmdHUD::lastGuiOpen == 6) {
		if ($CmdHUD::guiOpen == 2)
			CmdHUD::Cursor($CmdHUD::State);
		else if ($CmdHUD::guiOpen == 4)
			CmdHUD::Cursor($CmdHUD::State);
		}
	
	// if you enter the playGui while at a command station (turrets / cameras)
	if ($CmdHUD::Station == 2 && $CmdHUD::guiOpen == 1 && $CmdHUD::lastGuiOpen == 2 && $CmdHUD::guiMode == 2) {
		CmdHUD::DebugEcho("$CmdHUD::CommandTurret = true");
		CmdHUD::turnOn(Default, 0);
		$CmdHUD::CommandTurret = true;
		}

	// PlayGui on screen means the full-screen Command/Inventory/Objectives gui
	// is gone, but Escape closes those guis through the canvas (server-
	// confirmed PlayMode) without ever running the CmdHUD/InvHUD toggles, so
	// guiMode goes stale at 2/4/5 and the next hotkey press is a dead no-op
	// "toggle" back to 1. Resync here. Must stay BELOW the turret check (which
	// needs the stale 2 and already resets it via turnOn on that path);
	// station exit restores come from $CmdHUD::EnterStationGuiMode, so this
	// does not disturb them. Mode 5 is safe to include: ObjectivesMode always
	// swaps the content control to CmdObjectives.gui (dlgPlay.cpp), so PlayGui
	// opening while guiMode says 5 always means the objectives screen closed.
	if (%gui == playGui && ($CmdHUD::guiMode == 2 || $CmdHUD::guiMode == 4 || $CmdHUD::guiMode == 5))
		CmdHUD::SetGui(1);

	// NATIVE-PORT (beta, verified manually): full-screen command map on every
	// command-screen open -- sizes the map frames to the live screen and re-fits
	// the map natively (cmdMapResync; no world rebuild). $pref::cmdMapFull = 0
	// opts back into the authored small-map layout.
	if ($CmdHUD::guiOpen == 2 && $pref::cmdMapFull != "0") {
		schedule("CmdHUD::FullLayout();", 0.1);
		}

	CmdHUD::DebugEcho("$CmdHUD::guiOpen = " @ $CmdHUD::guiOpen @ ", $CmdHUD::lastGuiOpen = " @ $CmdHUD::lastGuiOpen);
	}

Event::Attach(eventGuiOpen, CmdHUD::GuiOpen);

function CmdHUD::onClientMessage(%client, %message) {
	if (%client != 0)
		return;

	if (%message == "Command Access On") {
		Event::Trigger(eventEnterCommand);
		return;
		}
	if (%message == "Command Access Off") {
		Event::Trigger(eventExitCommand);
		return;
		}
	if (%message == "--ACCESS DENIED-- Wrong Team ") {
		Event::Trigger(eventAccessEnemyStation);
		return;
		}
	}

Event::Attach(eventClientMessage, CmdHUD::onClientMessage);

function CmdHUD::InitalizeGuiObjects() {
	CmdHUD::DebugEcho("Gui Objects Initialized");

	$CmdHUD::SmallMapFrame = Group::GetObject(NameToId(CommandGui), 4);
	$CmdHUD::BigMapFrame = Group::GetObject(NameToId(CommandGui), 5);
	$CmdHUD::CommandMap = Group::GetObject($CmdHUD::SmallMapFrame, 0);
	$CmdHUD::BigMapChat = Group::GetObject($CmdHUD::BigMapFrame, 0);
	}

function CmdHUD::SmallMap() {
	Control::SetVisible("BigMapFrame", false);
	RemoveFromSet($CmdHUD::BigMapFrame, $CmdHUD::CommandMap);

	$CmdHUD::CommandMap = newObject(SmallMap, FearGui::TSCommander, 0, 0, 204, 204);
	addToSet($CmdHUD::SmallMapFrame, $CmdHUD::CommandMap);

	Control::SetVisible("SmallMapFrame", true);
	RebuildCommandMap();
	
	if ($CmdHUD::TeeniCam)
		HUD::Display(CmdHUDCam, false);

	CmdHUD::DebugEcho("executing CmdHUD::SmallMap(), $CmdHUD::CommandMap = " @ $CmdHUD::CommandMap);
	}

function CmdHUD::BigMap() {
	if (!$CmdHUD::BeenResized) {
		CmdHUD::InitalizeGuiObjects();
		$CmdHUD::BeenResized = true;
		}

	Control::SetVisible("SmallMapFrame", false);
	RemoveFromSet($CmdHUD::SmallMapFrame, $CmdHUD::CommandMap);

	$CmdHUD::CommandMap = newObject(BigMap, FearGui::TSCommander, 0, 0, 420, 420);
	addToSet($CmdHUD::BigMapFrame, $CmdHUD::CommandMap);

	removeFromSet($CmdHUD::BigMapFrame, $CmdHUD::BigMapChat);
	addToSet($CmdHUD::BigMapFrame, $CmdHUD::BigMapChat);

	Control::SetVisible("BigMapFrame", true);
	RebuildCommandMap();

	if ($CmdHUD::TeeniCam)
		HUD::Display(CmdHUDCam, true);

	CmdHUD::DebugEcho("executing CmdHUD::BigMap(), $CmdHUD::CommandMap = " @ $CmdHUD::CommandMap);
	}

function CmdHUD::MapToggle(%state) {
	client::centerprint("<jc>CmdHUD Resizing Map...", 0);
	schedule("client::centerprint(\"\", 0);", 2);

	if (%state)
		schedule("CmdHUD::SmallMap();", 0.1);
	else
		schedule("CmdHUD::BigMap();", 0.1);

	return %state;
	}

//
// This is what happens when you press the Commander button at the top left of
// the screen while in Commander mode or press the SizeToggle key in PDA mode.
// (Toggles the map size small/big)
//
function CmdHUD::CmdButtonPress() {
	if ($CmdHUD::guiOpen == 2)
		$CmdHUD::MapState = CmdHUD::MapToggle(!$CmdHUD::MapState);

	CmdHUD::DebugEcho("executing CmdHUD::CmdButtonPress(), $CmdHUD::MapState = " @ $CmdHUD::MapState);

	}
//
// Turn the cursor on/off & set CommandPanel state accordingly
// returns the cursor state
//
function CmdHUD::Cursor(%state) {

	if ($CmdHUD::MapState || !$CmdHUD::TeeniCam)
		Control::SetVisible("CommandPanel", %state);

	if (%state) {
		cursorOn(MainWindow);
		schedule("Control::SetVisible(\"InvMouseToggle\", true);", $CmdHUD::InvDelay);
		}
	else {
		schedule("cursorOff(MainWindow);", $CmdHUD::MouseOffTime);
		schedule("Control::SetVisible(\"InvMouseToggle\", false);", $CmdHUD::InvDelay);
		}

	CmdHUD::DebugEcho("Cursor = " @ %state);
	return %state;
	}

function CmdHUD::ObjectiveOn() {
	CmdHUD::DebugEcho("executing CmdHUD::ObjectiveOn()");
	if ($CmdHUD::guiMode == 5) {
		remoteEval(2048, PlayMode);
		CmdHUD::SetGui(1);
		}
	else {
		remoteEval(2048, ObjectivesMode);
		CmdHUD::SetGui(5);
		}

	}

//
// Turn on the Commander Screen
//
function CmdHUD::turnOn(%size, %mouse) {
	if($repackKeyOverride & 4){
		if(%size == "Large")
			%key = "]";
		else if(%size == "Small")
			%key = "[";
		if(%key != "")
			sendControl(%key);
		return;
	}
	
	if ($CmdHUD::CommandTurret) {
		$CmdHUD::CommandTurret = false;
		%size = Default;
		%mouse = 1;
	}

	CmdHUD::DebugEcho("executing CmdHUD::TurnOn(" @ %size @ ", " @ %mouse @ ")");
	
	if ($CmdHUD::guiMode != 2) {
		remoteEval(2048, CommandMode);
		CmdHUD::SetGui(2);
		if (%size == Small) {
			if (!$CmdHUD::MapState)
				schedule("CmdHUD::CmdButtonPress();", $CmdHUD::MouseOffTime);
			}
		else if (%size == Large) {
			if ($CmdHUD::MapState)
				schedule("CmdHUD::CmdButtonPress();", $CmdHUD::MouseOffTime);
			}
		else if ($CmdHUD::EnterStationMap && $CmdHUD::ResizeMap && !$CmdHUD::Station) {
			schedule("CmdHUD::CmdButtonPress();", $CmdHUD::MouseOffTime);
			$CmdHUD::ResizeMap = false;
			}
		}

	else {
		if (%size == Small && !$CmdHUD::MapState)
			schedule("CmdHUD::CmdButtonPress();", $CmdHUD::MouseOffTime);
		else if (%size == Large && $CmdHUD::MapState)
			schedule("CmdHUD::CmdButtonPress();", $CmdHUD::MouseOffTime);
		else {
			remoteEval(2048, PlayMode);
			CmdHUD::SetGui(1);
			return;
			}
		}

	if (%mouse == 0 || %mouse == ""){
		$CmdHUD::State = True;//Forcing cursor on because it is disorienting otherwise
		CmdHUD::Cursor($CmdHUD::State);
	}
	else if (%mouse == 1)
		$CmdHUD::State = CmdHUD::Cursor(true);
	else if (%mouse == 2)
		$CmdHUD::State = CmdHUD::Cursor(false);
	else if ($CmdHUD::Station)
		CmdHUD::Cursor($CmdHUD::State);
	}

//Sent by the server when the server brings up the inventory screen.
//Most servers probably won't know to send it.
//%mode = 3 for cursor on, 1 for cursor off, 5 for client preference
function remoteInvHUD::turnOn(%server, %mode){
	if(%server != 2048)
		return;
	if(%mode & 1){
		CmdHUD::SetGui(4);
		if(%mode & 4)
			CmdHUD::Cursor($CmdHUD::State);
		else if(%mode & 2)
			$CmdHUD::State = CmdHUD::Cursor(true);
		else
			$CmdHUD::State = CmdHUD::Cursor(false);
	}
	else
		CmdHUD::SetGui(1);
}
//
// Turn on the Inventory Screen
//
function InvHUD::turnOn(%mouse) {
	if($repackKeyOverride & 4){
		sendControl("i");
		return;
	}
	CmdHUD::DebugEcho("executing InvHUD::TurnOn(" @ %mouse @ ")");

	if ($CmdHUD::guiMode != 4) {
		remoteEval(2048, InventoryMode);
		CmdHUD::SetGui(4);
		
		if ($PrestoPref::InvCamera)
    			schedule("HUD::Display(hudcam, false);", $CmdHUD::InvDelay);

		if ($CmdHUD::Station)
			CmdHUD::Cursor($CmdHUD::State);
		else if (%mouse == 0 || %mouse == "")
			CmdHUD::Cursor($CmdHUD::State);
		else if (%mouse == 1)
			$CmdHUD::State = CmdHUD::Cursor(true);
		else if (%mouse == 2)
			$CmdHUD::State = CmdHUD::Cursor(false);
		}
	else {
		remoteEval(2048, PlayMode);
		CmdHUD::SetGui(1);
		}
	}

//
//  What the code below does:
//
// 	- Now uses Presto's Camhud.cs script to turn you around in
//	  an inventory station (why rewrite what's already written?)
// 	- Doesn't turn you around if you are repairing the station
// 	- Restores your previous Gui mode upon exiting either inventory or
// 	  command stations
//	- Detects if you get close enough to an enemy station to 'activate'
//	  it and keeps you in the correct Gui mode!
//

//
// What to do when you enter a Command Station
//
function CmdHUD::EnterStation() {
	CmdHUD::DebugEcho("Enter Command Station");

	$CmdHUD::Station = 2;
	$CmdHUD::ResizeMap = false;
	$CmdHUD::EnterStationMap = $CmdHUD::MapState;
	$CmdHUD::EnterStationMouse = $CmdHUD::State;
	$CmdHUD::EnterStationGuiMode = $CmdHUD::guiMode;

	CmdHUD::SetGui(2);

	if ($CmdHUD::CmdStaLarge && $CmdHUD::MapState) {
		CmdHUD::DebugEcho("$CmdHUD::CmdStaLarge, Resize Map");
		$CmdHUD::ResizeMap = true;
		schedule("CmdHUD::CmdButtonPress();", $CmdHUD::MouseOffTime);
		}

	$CmdHUD::State = CmdHUD::Cursor(true);
	
	if ($CmdHUD::CmdTurnAround) {
		CmdHUD::DebugEcho("Turnaround (CamHUD)");
		CamHud::EnterStation();
		}
	}

//
// What to do when you exit a Command Station
//
function CmdHUD::ExitStation() {
	CmdHUD::DebugEcho("Exit Command Station");

	CursorOff(MainWindow);

	if ($CmdHUD::CmdTurnAround) {
		CmdHUD::DebugEcho("Turnaround (CamHUD)");
		CamHud::ExitStation();
		}

	remoteEval(2048, PlayMode);
	CmdHUD::SetGui(1);
	$CmdHUD::State = $CmdHUD::EnterStationMouse;

	if ($CmdHUD::EnterStationGuiMode == 2) {
		CmdHUD::DebugEcho("Restore Map State (on when enter)");
		if ($CmdHUD::EnterStationMap)
			CmdHUD::turnOn(Small, $CmdHUD::EnterStationMouse);
		else 
			CmdHUD::turnOn(Large, $CmdHUD::EnterStationMouse);
		}
	else if ($CmdHUD::EnterStationGuiMode == 4) {
		CmdHUD::DebugEcho("Restore Inventory State (on when enter)");
		InvHUD::turnOn($CmdHUD::EnterStationMouse);
		}

	$CmdHUD::Station = 0;
	}

//
// What to do when you enter an Inventory Station
//
function InvHUD::EnterStation() {
	CmdHUD::DebugEcho("Enter Inventory Station");
	$CmdHUD::Station = 1;
	$CmdHUD::EnterStationMouse = $CmdHUD::State;
	$CmdHUD::EnterStationGuiMode = $CmdHUD::guiMode;

	CmdHUD::SetGui(4);

	if ($pref::noEnterInvStation && $CmdHUD::LastGuiOpen == 4) {
		$CmdHUD::State = CmdHUD::Cursor(true);
		remoteEval(2048, InventoryMode);
		}
	else if ($pref::noEnterInvStation && $CmdHUD::LastGuiOpen == 2)
		CmdHUD::turnOn(Default, 0);
	else if (!$pref::noEnterInvStation)
		$CmdHUD::State = CmdHUD::Cursor(true);

	if ($PrestoPref::InvCamera)
    		schedule("HUD::Display(hudcam, false);", $CmdHUD::InvDelay);

	if ($CmdHUD::InvTurnAround) {
		CmdHUD::DebugEcho("Turnaround (CamHUD)");
		CamHud::EnterStation();
		}
	}
	

//
// What to do when you exit an Inventory Station
//
function InvHUD::ExitStation() {
	CmdHUD::DebugEcho("Exit Inventory Station");

	CursorOff(MainWindow);

	if ($CmdHUD::InvTurnAround) {
		CmdHUD::DebugEcho("Turnaround (CamHUD)");
		CamHud::ExitStation();
		}
		
	remoteEval(2048, PlayMode);
	CmdHUD::SetGui(1);
	$CmdHUD::State = $CmdHUD::EnterStationMouse;

	$CmdHUD::Station = 0;

	if ($CmdHUD::EnterStationGuiMode == 2) {
		CmdHUD::DebugEcho("Restore Map State (on when enter)");
		CmdHUD::turnOn(Default, $CmdHUD::EnterStationMouse);
		}
	else if ($CmdHUD::EnterStationGuiMode == 4) {
		CmdHUD::DebugEcho("Restore Inventory State (on when enter)");
		InvHUD::turnOn($CmdHUD::EnterStationMouse);
		}

	$CmdHUD::BeenInInv = true;
	}

//
// Restore settings to defaults
//
function CmdHUD::Reset() {
	CmdHUD::DebugEcho("Reset");
	CmdHUD::SetGui(1);
	$CmdHUD::State = true;
	$CmdHUD::BeenInInv = false;
	$CmdHUD::CommandTurret = false;
	$CmdHUD::Station = 0;
	}

//
// If you get killed while in CmdHUD, let it know you're back in the
// play gui
//
function CmdHUD::KillTrak(%killer, %victim, %weapon) {
	if (%victim == getManagerId()) {
		CmdHUD::DebugEcho("You got killed");
		CmdHUD::SetGui(1);
		}
	}

//
// set up all-new TeeniCam (that displays ONLY when you have the big map on!)
//

if ($CmdHUD::TeeniCam) {
	HUD::NewFrame(CmdHUDCam, "", "100%-33 100%-20 166 62");
	HUD::SetGui(CmdHUDCam, CommandGui);
	HUD::AddObject(CmdHUDCam, SimGui::TSControl, 4, 2, 158, 60);
	HUD::Display(CmdHUDCam, false);
	}

$CmdHUD::MapState = true;
$CmdHUD::BeenResized = false;

EditActionMap("ActionMap.sae");

Event::Attach(eventKillTrak, CmdHUD::KillTrak);

Event::Attach(eventEnterCommand, CmdHUD::EnterStation);
Event::Attach(eventExitCommand, CmdHUD::ExitStation);
Event::Attach(eventEnterStation, InvHUD::EnterStation);
Event::Attach(eventExitStation, InvHUD::ExitStation);

Event::Attach(eventConnected, CmdHUD::Reset);
Event::Attach(eventChangeMission, CmdHUD::Reset);

Event::Attach(eventClientMessage, CamHud::onClientMessage);

if ($PrestoPref::InvCamera) {
	Event::Detach(eventExitStation, CamHud::ExitStation);
	Event::Detach(eventEnterStation, CamHud::EnterStation);
	Event::Detach(eventGuiOpen, CamHud::EnterGui);
	Event::Detach(eventGuiClose, CamHud::ExitGui);
	Event::Detach(eventClientMessage, CamHud::onClientMessage);
	}

// ============================================================================
// NATIVE-PORT (beta, staged v2): full-screen command map -- MANUAL TEST FUNCTION.
//
// v1 died inside RebuildCommandMap() (the software world re-render; [CMDMAP]
// stopped at step 7). v2 never rebuilds and never creates objects: it resizes
// the frame the LIVE map already sits in and calls the new native
// cmdMapResync(), which re-fits the map control to the frame and recomputes
// the overlay scale -- the GPU plan view then simply draws bigger.
//
// With the command screen open, run from the console:
//
//    CmdHUD::FullLayout();
//
// If anything misbehaves, the LAST [CMDMAP] line names the step.
// ============================================================================
function CmdHUD::FullLayout() {
	%sw = String::getWord(Control::getExtent(CommandGui), 0);
	%sh = String::getWord(Control::getExtent(CommandGui), 1);
	echo("[CMDMAP] 1 screen ext = " @ %sw @ " x " @ %sh);
	if (%sw == "" || %sw < 640) {
		echo("[CMDMAP] abort: no usable screen extent");
		return;
		}

	%pw = String::getWord(Control::getExtent(CommandPanel), 0);
	%px = String::getWord(Control::getPosition(CommandPanel), 0);
	%py = String::getWord(Control::getPosition(CommandPanel), 1);
	echo("[CMDMAP] 2 panel ext.w = " @ %pw @ "  pos = " @ %px @ "," @ %py);
	if (%pw == "") {
		echo("[CMDMAP] abort: CommandPanel not found");
		return;
		}

	%mapX = 0;
	if (%px + (%pw / 2) < (%sw / 2)) {
		Control::setPosition(CommandPanel, 0, %py);
		%mapX = %pw;
		echo("[CMDMAP] 3 panel pinned LEFT, map starts at x=" @ %mapX);
		}
	else {
		Control::setPosition(CommandPanel, %sw - %pw, %py);
		echo("[CMDMAP] 3 panel pinned RIGHT, map starts at x=0");
		}
	%mapW = %sw - %pw;
	%mapH = %sh;

	// Resize BOTH map frames to the same big rect -- whichever one the live map
	// is in becomes the fullscreen host; the hidden one is harmless.
	echo("[CMDMAP] 4 sizing map frames to " @ %mapX @ ",0 " @ %mapW @ " x " @ %mapH);
	Control::setPosition(SmallMapFrame, %mapX, 0);
	Control::setExtent(SmallMapFrame, %mapW, %mapH);
	Control::setPosition(BigMapFrame, %mapX, 0);
	Control::setExtent(BigMapFrame, %mapW, %mapH);

	echo("[CMDMAP] 5 refitting the live map (cmdMapResync, no rebuild)...");
	%ok = cmdMapResync();
	echo("[CMDMAP] 6 resync returned " @ %ok);
	}
