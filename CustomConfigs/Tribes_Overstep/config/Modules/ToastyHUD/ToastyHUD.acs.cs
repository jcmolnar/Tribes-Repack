// Toasty HUD (Mortal Kombat)
// Made by -|DF|- Link
// ver: 0.0.2

// Dedicated to:
//			v0dka
//			WiseFool (Milk-Man)
//			lucy in the sky (emjay)
//			LEMON
//			isuk@tribes
//			InvaderJim
//			...and lots more!

// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

// Inspired by Mortal Kombat, hitting an MA further than 50+ meters will pop-up none other than Dan Forden
// on the right side of your screen. Brag to your friends, or enemies, on the amount of toasty's you've hit!
// Share your screenshots and videos with the community on playt1.com discord!

// Don't forget to set your keybind to take a screenshot with your toasty in Options > Binds > Game

// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

// Change log:

//  7.08.23
//			[x] Fix so a toasty will not count while watching a demo
//			[x] Fixed demo time and is now precise to game time (00:00:00 format)
//			[x] Added the 'clockHud' to screenshots so you have the time in game
//			[x] Found quicker method to extract MA meters


//	6.11.23
//		-	[x] Screenshot feature added for toasty's
//		-	[x] Toasty stats are now saving mission, victim, date/time, meters, and demo time (if recording)
//		-	[x] A quick center print now shows who you hit, how far, and how many toasty's you have thus far

// Future:
//		-	[ ] Toasty top 3 leaderboard
//		-	[ ] Toasty HUD stats pop-up (show your work!)
//		-	...

// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


// Begin Script. DO NOT TOUCH!

$ToastyHUD::Animate = false;
$ToastyHUD::matchStarted = false;
$ToastyHUD::PScreenshot = false;

$ToastyHUD::Count = 0;

$MA_VICTIM = "";
$MA_SHOOTER = "";
$IMAGE_WIDTH = 400;
$MA_METER_DIST = 50;
$MA_GET_METER = 0;

function ToastyHUD::GameBinds::Init() after GameBinds::Init
{
	$GameBinds::CurrentMapHandle = GameBinds::GetActionMap2( "actionMap.sae");
	$GameBinds::CurrentMap = "actionMap.sae";
	GameBinds::addBindCommand( "ToastyHUD Toggle Screenshot", "ToastyHUD::ScreenshotToggle();");
}

function ToastyHUD::ScreenshotToggle() {
	$ToastyHUD::PScreenshot = !$ToastyHUD::PScreenshot;
	remoteCP( 2048, "<JC><F1>Toasty HUD screenshots are " @ ($ToastyHUD::PScreenshot ? "enabled" : "disabled") @ "!", 3 );
}

function ToastyHUD::Init()
{
	if($ToastyHUD::Loaded)
		return;
	
	$ToastyHUD::Loaded = true;
	
	// Load stats
	ToastyHUD::LoadStats();
	
	// Get window dimensions
	ToastyHUD::GetWindowDims();
	
	HUD::New("ToastyHUD::Container", ($ScreenX - $IMAGE_WIDTH), 0, 405, 405, ToastyHUD::Wake, ToastyHUD::Sleep);
	
	newObject("ToastyHUD::Image0", FearGuiFormattedText, 0, 0, 405, 405);
	
	HUD::Add("ToastyHUD::Container", "ToastyHUD::Image0");

	Control::SetValue("ToastyHUD::Image0", "<B0,0:Modules/ToastyHUD/toasty.png>");
}

function ToastyHUD::Wake() { ToastyHUD::Update(); }
function ToastyHUD::Sleep() { return; }

function ToastyHUD::GetWindowDims()
{
	if(isObject(playGui))
		%gui = "playGui";
	
	%guiObject = Control::getId(%gui);

	$ScreenX = getWord(%guiObject.extent,0);
	$ScreenY = getWord(%guiObject.extent,1);
}

function ToastyHUD::Update() 
{
	if(!$ToastyHUD::Animate)
		ToastyHUD::GetWindowDims();
	
	$ScreenY = $ScreenY - $IMAGE_WIDTH;
	
	%obj = Control::getId("ToastyHUD::Container");

	// Move container off screen	
	%obj.position = $ScreenX @ " " @ $ScreenY;
}

function ToastyHUD::Animate(%dir)
{
	$ToastyHUD::Animate = true;
		
	%scrX = $ScreenX;
	%scrY = $ScreenY;
	
	%obj = Control::getId("ToastyHUD::Container");
				
	switch (%dir)
	{
		case "out":
			// Make 'toasty' guy visible
			$ToastyHUD::Count++;
			
			// Do some scheduling magic
			if($ToastyHUD::Count >= $IMAGE_WIDTH)
				schedule::cancel("animate");
			else
				schedule::add("ToastyHUD::Animate('out');", 0.0002, "animate");
			
			// Need the last X in order to hide him
			if ($ToastyHUD::Count >= $IMAGE_WIDTH)
				$GetLastX = getWord(%obj.position,0);
			
			%scrDir = (%scrX - $ToastyHUD::Count);
			break;
		case "in":	
			// Make 'toasty' guy hide
			$ToastyHUD::Count--;
			
			if ($ToastyHUD::Count <= 0)
				schedule::cancel("animate");
			else
				schedule::add("ToastyHud::Animate('in');", 0.0002, "animate");

			%scrDir = ($GetLastX + ($IMAGE_WIDTH - ($ToastyHUD::Count - 1)));
			break;
	}
	
	// Make it TOASTY!
	%obj.position = %scrDir @ " " @ %scrY;
}

function ToastyHUD::PlayAnim()
{
	// Get window dimensions
	ToastyHUD::Update();
	
	// TOOASTY!
	schedule("ToastyHUD::Animate('out');", 0.2);
	schedule("ToastyHUD::CenterPrint();", 0.2);
	schedule("ToastyHUD::Screenshot();", 0.2); // toggled true only
	schedule("localSound(\"mk.toasty.ogg\");", 0.3);
	schedule("ToastyHUD::Animate('in');", 1.0);
	schedule("$ToastyHUD::Animate = false;", 1.1);
}

function ToastyHUD::TestMsg()
{
	// Fix to not count toasty's playing a demo
	if ($playingDemo)
		return;
	
	//%msg = sprintf("%1 lands [ 50 meter ] mid-air on %2!", "Dummy", "Doofus");
	%msg = sprintf("%1 lands [ 50 meter ] mid-air on %2!", $PCFG::Name, "Dummy");
	//%msg = sprintf("%1 lands [ 50 meter ] mid-air on %2!", "Dummy", $PCFG::Name);
	
	if(String::findSubStr( %msg, "mid-air")!=-1)
	{
		// Get meters and shooter/victim pairs
		%meters = $MA_GET_METER = GetMidAirMeter(%msg);
		%pairs = String::Trim( String::Replace( String::Replace( %msg, sprintf(" lands [ %1 meter ] mid-air on ", %meters), ", " ), "!", "") );
		String::explode(%pairs, ", ", "player");
		
		%shooter = $MA_SHOOTER = $player[0];
		%victim = $MA_VICTIM = $player[1];
		
		echoc(2, "TIME: " @ ConvertToTime("1.99453"));
		
		// Add mission, victim, and timestamp
		%mission = $ServerMission;
		%victim = $MA_VICTIM;
		%timestamp = ToastyHUD::Timestamp();
		%meters = $MA_GET_METER = GetMidAirMeter(%msg);
	
	if ((%shooter == $PCFG::Name) || (%victim == $PCFG::Name ))
		if (%meters >= $MA_METER_DIST)
		{
			// Play the animation!
			ToastyHUD::PlayAnim();
			
			// Show centerprint info
			// Check for screenshot
			ToastyHUD::CenterPrint("SS");
		
			// Export TOASTY stats
			ToastyHUD::ExportStats(%mission, %victim, %timestamp, %meters);
		}
	}
}

function ToastyHUD::MidAirMsg( %msg )
{
	// Fix to not count toasty's playing a demo
	if ($playingDemo)
		return;
	
	if(String::findSubStr( %msg, "mid-air")!=-1)
	{
		// Get meters and shooter/victim pairs
		%meters = $MA_GET_METER = GetMidAirMeter(%msg);
		%pairs = String::Trim( String::Replace( String::Replace( %msg, sprintf(" lands [ %1 meter ] mid-air on ", %meters), ", " ), "!", "") );
		String::explode(%pairs, ", ", "player");
		
		%shooter = $MA_SHOOTER = $player[0];
		%victim = $MA_VICTIM = $player[1];
		
		// Add mission, victim, and timestamp
		%mission = $ServerMission;
		%victim = $MA_VICTIM;
		%timestamp = ToastyHUD::Timestamp();
	
	if ((%shooter == $PCFG::Name) || (%victim == $PCFG::Name ))
		if (%meters >= $MA_METER_DIST)
		{
			// Play the animation!
			ToastyHUD::PlayAnim();
			
			// Show centerprint info
			// Check for screenshot
			ToastyHUD::CenterPrint("SS");
		
			// Export TOASTY stats
			ToastyHUD::ExportStats(%mission, %victim, %timestamp, %meters);
		}
	}
}

function ToastyHUD::ShowLeaderboard() 
{ 
	// Future...
}

function ToastyHUD::CenterPrint(%type)
{
	// Get random ownage messages
	%praises = "spanked smacked pwned obliterated popped hit owned";
	deleteVariables("*$praise*");
	String::explode(%praises, " ", "praise");
	
	// Add mission, timestamp, and meters
	%mission = $ServerMission;
	%timestamp = ToastyHUD::Timestamp();
	%meters = $MA_GET_METER;
	
    %shooter = ($MA_SHOOTER == $PCFG::Name ? "You" : $MA_SHOOTER);
	%victim = ($MA_VICTIM == $PCFG::Name ? "you" : $MA_VICTIM);

	// Random stuff (literally)
	%randNum = floor(getRandom() * 6);
	%rmsg = sprintf("TOOASTY! <F2>%1<F1> %2<F2> %3<F1> at<F2> %4<F1> meters!", %shooter, $praise[%randNum], %victim, %meters);
	//"Toasty! <F2>" @ %shooter @ "<F1> " @ $praise[%randNum] @ " <F2>" @ %victim @ "<F1> at <F2>" @ %meters @ "<F1> meters!";
	
	// Center print the text
	if(%type == "SS" && $ToastyHUD::PScreenshot)
		remoteBP( 2048, "<JC><F1>" @ %rmsg @ "\n\n" @ %mission @ "\n" @ %timestamp, 3 );		
	else if(%type == "")
		remoteBP( 2048, "<JC><F1>" @ %rmsg @ "\n\nTotal Toasty's: " @ $ToastyHUD::PTotal, 3 );
	
	// Less clutter
	//eval( "cls();" );
	remoteEval(2048, scoresOff);
}

// Code borrowed from Anubus MidAir SS script
// Modified by Link
function ToastyHUD::Screenshot()
{
	if($ToastyHUD::PScreenshot) //&& $MA_SHOOTER == $PCFG::Name) 
	{
		for (%i = 0; %i <= Group::objectCount(playGui); %i++)
		{
			%obj = Object::getName(Group::getObject(playGui, %i));
			if((%obj != "ToastyHUD::Container") && (%obj != "clockHud"))
			{
				if(%obj != false) if(Control::getVisible(%obj))
				{
					$css::visibleObjects[$css::num++] = %obj;
					Control::setVisible(%obj, false);
				}
			}
		}
		schedule("screenShot(MainWindow);", 0.1);
		schedule("canvas::repaint();", 0.1);
	}
}

function canvas::repaint()
{
	for(%i = 0; %i <= $css::num; %i++)
		Control::setVisible($css::visibleObjects[%i], true);
	deleteVariables("$css::*");
}

function ToastyHUD::Timestamp()
{
	deleteVariables("$Time*");
	
	// Create timestamp array
	timestamp::array();
	
	// Create months array
	%months = "January February March April May June July August September October November December";

	// Get the corresponding month from months array
	%month = getWord(%months, ($Time["mo"]-1));
	
	// Grab the time suffix
	%suffix = ($Time["hr"] > 12 ? "PM" : "AM");

	// Format the hour
	%hour = (($Time["hr"] - 12) < 10 ? $Time["hr"] : ($Time["hr"] - 12));

	// Format date & time
	%display = sprintf("%1 %2, %3 at %4:%5 %6", %month, $Time["dy"], $Time["yr"], %hour, $Time["mn"], %suffix);

	return %display;
}

function ToastyHUD::LoadStats()
{
	if(isfile("config\\toasty_prefs.cs"))
		include("config\\toasty_prefs.cs");
}

function ToastyHUD::ExportStats(%mission, %victim, %time, %meters)
{
	if($MA_SHOOTER == $PCFG::Name)
	{
		%num = $ToastyHUD::PTotal++;
		
		// Check if recording, timestamp the demo
		%DemoTime = ($recordDemo == true ? $ToastyHUD::DemoTime : "00:00:00");
		
		// Format the string
		$ToastyBM[%num] = %mission @ "|" @ %victim @ "|" @ %time @ "|" @ %meters @ "|" @ %DemoTime;
		
		// Export toasty bookmarks
		export(ToastyBM@%num, "config\\toasty_bookmark.cs", true);
		
		// Export toasty totals
		File::delete("config\\toasty_prefs.cs");
		export("$ToastyHUD::P*", "config\\toasty_prefs.cs", false);
	}
}

function ToastyHUD::BackupStats()
{
	File::copy("config\\toasty_prefs.cs", "config\\toasty_prefs_bak.cs");
}

function GetMidAirMeter(%val)
{
	for (%i = 0; String::Trim( getWord(%val, %i) ) != -1; %i++)
		if(isNum(getWord(%val, %i))==0) return getWord(%val, %i);
	
	return -1;
}

function isNum(%val)
{
	return(chr(%val)!="" ? 0 : -1);
}

function ToastyHUD::onMatchStarted() {
   schedule("ToastyHUD::checkTime();", 30);
}

function ToastyHUD::checkTime( %when ) {
   if ($playingDemo)
	   return;
   
   if (%when != $ToastyHUD::when) {
      // The client has received a more recent time update
      return;
   }
   
   if($ToastyHUD::matchStarted && $recordDemo)
   {
	   %secs = floor($ToastyHUD::secondsLeft * -1 % 60);
	   %hours = floor(($ToastyHUD::secondsLeft * -1 % 60) / 3600);
	   %mins = floor(($ToastyHUD::secondsLeft * -1) / 60);
	   %demo_time = sprintf( "%1:%2:%3", String::lpad(%hours, 2, "0"), String::lpad(%mins, 2, "0"), String::lpad(%secs, 2, "0") );

	   // Get the time in game
	   $ToastyHUD::DemoTime = %demo_time;
	   echoc(2, $ToastyHUD::DemoTime);
   }
   
   if($ToastyHUD::secondsLeft == 0)
	   $ToastyHUD::matchStarted = true;

   $ToastyHUD::secondsLeft--;
   schedule("ToastyHUD::checkTime(" @ %when @ ");", 1);
}

function ToastyHUD::onUpdateTime( %min, %secs ) 
{
   $ToastyHUD::secondsLeft = %min * 60 + %secs;
   $ToastyHUD::when = getSimTime();
   // We seem to get whole numbers, but just in case
   $ToastyHUD::secondsLeft = floor($ToastyHUD::secondsLeft);
   schedule("ToastyHUD::checkTime(" @ $ToastyHUD::when @ ");", 1);   
}

ToastyHUD::Init();

Event::Attach( eventServerMessage, ToastyHUD::MidAirMsg );
Event::Attach( eventMatchStarted, ToastyHUD::onMatchStarted );
Event::Attach( eventUpdateTime, ToastyHUD::onUpdateTime);
Event::Attach( eventExit, ToastyHUD::BackupStats );