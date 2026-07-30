// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// CamHUD.CS								Presto, March '99 
//
//	Display your first-person view inside a HUD!
//
//	This script lets you pop up a HUD window which displays your view.
//	Why would you want to do this, when your view already fills the whole
//	screen?  Well, it doesn't when you're at an inventory station or in the
//	commander window!  This will let you keep an eye on things while
//	you're taking care of business.
//
//	Some technical info:  This uses my HUD system's ability to put objects
//	in a hud.  The particular object it's putting in is one called
//	SimGui::TSControl, which is the first-person view.  Unfortunately, I
//	can't find any way to manipulate the view of a TSControl from a
//	script.  What I *really* want to do is to make the camera look behind
//	you, so that when you enter an inventory station, you can see if 
//	people are lined up behind you.
//
//	Instead, I've just written the HUD and a command to turn you around.
//	The turnaround command isn't very accurate, though, so any help
//	would be appreciated.
//
//	Now, since you've been good and read all the way to the bottom of this
//	explanation, I'll give you a hint.  There are four more objects I've
//	found which work very well in HUDs, besides SimGUI::TSControl.
//
//	#1 is obviously the FearGuiFormattedText, which I use to do normal
//	text HUDs.
//
//	#2 is FearGui::FearGuiBox, which makes a transparent outline border.
//	Kind of boring, though.
//
//	#3 is FearGui::SkinView.  Once you've added this object to the HUD,
//	you can call
//		FGSkin::set(Object::GetName(%skinviewObject), %skin, %gender);
//			where %skin is for example "dsword" or "beagle"
//			and %gender is 0 or 1, I think.
//		FGSkin::cycleArmor(Object::GetName(%skinviewObject));
//			which will toggle between light / medium / heavy.
//		There's a function called "FGSkin::discoBoogie" listed
//		in the tribes.exe, I'd love to see what this does, but
//		it isn't recognized by the console! :((
//
//	#4 is the most useful, I think.  It's FearGui::ShapeView.
//		ShapeView::setItem(Object::GetName(%shapeviewItem), %item);
//			where %item is a standard item number from the
//			inventory list.
//
//	That's right, now you can make animated HUDs, which have the
//	potential to be really cool or really annoying!  Also, if you're
//	at all into GUI object programming, you know how to construct
//	a multi-part object with addToSet, so you can create complex
//	HUDs.
//
//	So, it would be cool if anyone can come up with the discoBoogie
//	HUD.  And then, if anyone figures out how to use FearGui::FGBitmapCtrl, 
//	please let me know.  Right now, it just comes up with a default bitmap 
//	and I can't change it.
//
//	Other classes worth investigating:
//		FearGui::TSCommander		(the commander map)
//		FearGui::CommandTeamCtrl	(the list of team members)
//		FearGui::FGComboBox		(combo boxes :) )
//		FearGui::FGUniversalButton	(buttons...)
//		FearGui::FGSimpleText		(unformatted text lines)
//		FearGui::FearGuiRadio		(what's a radio?)
//
//	Also, it would probably be possible to set up a HUD with buttons on it.
//	You could then use "cursorOn(mainWindow);" and "cursorOff(mainWindow);"
//	to enable a cursor on the main screen, and you could probably click
//	on the HUDs!  It might be possible to rebuild the entire Commander
//	GUI as a pop-up HUD.
//
// ---------------------------------------------------------------------------
Include("presto\\HUD.cs");
Include("presto\\Inventory.cs");
Include("presto\\Match.cs");

// Why so complex?  I hate it when I'm repairing a station, and it turns me around because
// I accessed it.
$camHUD::noTurnaround = false;
$camHUD::doTurnaround = false;
function CamHUD::NoTurnaround(%reason, %flag) {
	$camHUD::noTurnaround = %flag;
	if (!%flag && $camHUD::doTurnaround)
		CamHUD::DoTurnaround(true);
	}
function CamHUD::DoTurnaround(%flag) {
	$camHUD::doTurnaround = %flag;
	if (%flag && !$camHUD::noTurnaround)
		{
		if ($PrestoPref::CamHudTurnAround) {
			postAction(2048, IDACTION_TURNLEFT, $PrestoPref::TurnAroundSpeed);
			schedule("postAction(2048, IDACTION_TURNLEFT, -0);", $PrestoPref::TurnAroundTime);
			}
		$camHUD::doTurnaround = false;
		}
	}
function CamHUD::EnterStation() {
	CamHUD::DoTurnaround(true);
	}
function CamHUD::ExitStation() {
	CamHUD::DoTurnaround(false);
	}
function CamHUD::FreeLook(%flag) {
	if ($PrestoPref::CamHudFreeLookOnByDefault)
		%flag = !%flag;
	if (%flag)
		cursorOff(mainWindow);
	else	cursorOn(mainWindow);
	}
function CamHUD::EnterGui(%gui) {
	if (%gui != CmdInventoryGui) 
		return;
	$camHUD::prevFOV = $pref::PlayerFOV;
	$pref::PlayerFOV = $PrestoPref::CamHudFOV;
	schedule("CamHUD::FreeLook(false);",0.1);
	}
function CamHUD::ExitGui(%gui) {
	if (%gui != CmdInventoryGui) 
		return;
	$pref::PlayerFOV = $camHUD::prevFOV;
	}

$camHUD::prevFOV = $pref::PlayerFOV;
function CamHUD::onClientMessage(%client, %message) {
	if (%client != 0)
		return;

	if (Match::String(%message, "Repairing *")) {
		%repairing = Match::Result(0);
		// This is to catch someone going by the name "Repairing whatever" and
		// messing with the script.  True repair strings don't have
		// punctuation at the end.  I didn't put it under StrictNameChecking
		// because it's really serious if it happens (one person could cause
		// everyone in the game to stop turning around)
		// I'll probably put a timeout for no-turnaround 
		%idx = Match::strLen(%repairing);
		%c = String::GetSubStr(%repairing, %idx - 1, %idx);
		while (%c = " ") {
			%idx--;	// Chop off trailing spaces.
			%c = String::GetSubStr(%repairing, %idx - 1, %idx);
			}
		if (%c == "." || %c == "!")
			return;
		// started
		CamHUD::NoTurnaround(repairing, true);
		return;
		}
	if (%message == "Repair Stopped" || %message == "Repair Done") {
		// stopped or done
		CamHUD::NoTurnaround(repairing, false);
		return;
		}
	}

function CamHUD::Init() {
	HUD::NewFrame(hudCam, "", $PrestoPref::CamHudPosition);
	HUD::SetGui(hudCam, CmdInventoryGui);	// displays on the inventory page.
	HUD::AddObject(hudCam, SimGui::TSControl, 4,2, HUD::Width(hudCam)-8, HUD::Height(hudCam)-4);
	HUD::Display(hudCam, true);

	Event::Attach(eventExitStation, CamHud::ExitStation);
	Event::Attach(eventEnterStation, CamHud::EnterStation);
	Event::Attach(eventGuiOpen, CamHud::EnterGui);
	Event::Attach(eventGuiClose, CamHud::ExitGui);
	Event::Attach(eventClientMessage, CamHud::onClientMessage);
	}

if (Presto::Enabled(InvCamera)) 
	CamHUD::Init();