// Toasty HUD (Mortal Kombat)
// Made by -|DF|- Link
// ver: 0.0.4

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


// Begin Script. DO NOT TOUCH!

$ToastyHUD::PlayInDemo = false;
$ToastyHUD::Animate = false;
$ToastyHUD::matchStarted = false;
$ToastyHUD::NewRecord = false;
$ToastyHUD::NewTop5Found = false;
$ToastyHUD::FoundAllTop5 = false;

$pref::ToastyScreenshot = false;
$pref::ToastyCustomMode = true;

$ToastyHUD::Count = 0;
$ToastyHUD::BMCount = 0;
$ToastyHUD::Top5Count = 0;

$ToastyHUD::AccoladeMsg = "";

// Set your own custom image and sounds
// Don't forget to change the $IMAGE_WIDTH
// variable to the width of your custom image
$CUSTOM_IMAGE_NAME = "scorpion.png";
$IMAGE_NAME = "scorpion.png";
$IMAGE_WIDTH = 400;

$CUSTOM_SND_NAME = "mk.getoverhere.ogg";
$SND_NAME = "mk.getoverhere.ogg";

// MA variables
$MA_VICTIM = "";
$MA_SHOOTER = "";
$MA_METER_DIST = 50;
$MA_GET_METER = 0;

function ToastyHUD::GameBinds::Init() after GameBinds::Init
{
	$GameBinds::CurrentMapHandle = GameBinds::GetActionMap2( "actionMap.sae");
	$GameBinds::CurrentMap = "actionMap.sae";
	GameBinds::addBindCommand( "ToastyHUD Toggle Screenshot", "ToastyHUD::ScreenshotToggle();");
	GameBinds::addBindCommand( "ToastyHUD Toggle Leaderboard", "ToastyHUD::ShowLeaderboard();", "remoteEP('');");
}

function ToastyHUD::ScreenshotToggle() {
	$pref::ToastyScreenshot = !$pref::ToastyScreenshot;
	remoteCP( 2048, "<JC><F1>Toasty screenshots are " @ ($pref::ToastyScreenshot ? "enabled" : "disabled") @ "!", 3 );
}

function ToastyHUD::LoadStats()
{
	if(isfile("config\\toasty_stats.cs"))
		if(!exec("config\\toasty_stats.cs"))
			exec("config\\toasty_stats.cs");
}

function ToastyHUD::Init()
{
	if($ToastyHUD::Loaded)
		return;
	
	$ToastyHUD::Loaded = true;
	
	// Load toasty stats
	ToastyHUD::LoadStats();
	
	// Get window dimensions
	ToastyHUD::GetWindowDims();
	
	HUD::New("ToastyHUD::Container", ($ToastyHUD::ScreenX - $IMAGE_WIDTH), 0, 405, 405, ToastyHUD::Wake, ToastyHUD::Sleep);
	
	newObject("ToastyHUD::Image0", FearGuiFormattedText, 0, 0, 405, 405);
	
	HUD::Add("ToastyHUD::Container", "ToastyHUD::Image0");
}

function ToastyHUD::Wake() { ToastyHUD::Update(); }
function ToastyHUD::Sleep() { return false; }

function ToastyHUD::GetWindowDims()
{
	if(isObject(playGui))
		%gui = "playGui";
	
	%guiObject = Control::getId(%gui);

	$ToastyHUD::ScreenX = getWord(%guiObject.extent,0);
	$ToastyHUD::ScreenY = getWord(%guiObject.extent,1);
}

function ToastyHUD::Update()
{
	if(!$ToastyHUD::Animate)
		ToastyHUD::GetWindowDims();
	
	$ToastyHUD::ScreenY = $ToastyHUD::ScreenY - $IMAGE_WIDTH;
	
	%obj = Control::getId("ToastyHUD::Container");

	// Move container off screen
	%obj.position = $ToastyHUD::ScreenX @ " " @ $ToastyHUD::ScreenY;
}

function ToastyHUD::Animate(%dir)
{
	$ToastyHUD::Animate = true;
		
	%ScreenX = $ToastyHUD::ScreenX;
	%ScreenY = $ToastyHUD::ScreenY;
	
	%obj = Control::getId("ToastyHUD::Container");
				
	switch (%dir)
	{
		case "out":
			// Make 'toasty' guy visible
			$ToastyHUD::Count++;
			
			// Do some scheduling magic
			if($ToastyHUD::Count >= $IMAGE_WIDTH) {
				Schedule::Cancel("animate");
				$ToastyHUD::Animate = false;
			}
			else
				Schedule::Add("ToastyHUD::Animate('out');", 0.0002, "animate");
			
			// Need the last X in order to hide him
			if ($ToastyHUD::Count >= $IMAGE_WIDTH)
				$GetLastX = getWord(%obj.position, 0);
			
			%scrDir = (%ScreenX - $ToastyHUD::Count);
			break;
		case "in":	
			// Make 'toasty' guy hide
			$ToastyHUD::Count--;
			
			if ($ToastyHUD::Count <= 0) {
				Schedule::Cancel("animate");
				$ToastyHUD::Animate = false;
			}
			else
				Schedule::Add("ToastyHud::Animate('in');", 0.0002, "animate");

			%scrDir = ($GetLastX + ($IMAGE_WIDTH - ($ToastyHUD::Count - 1)));
			break;
	}
	
	// Make it TOASTY!
	%obj.position = %scrDir @ " " @ %ScreenY;
}

function ToastyHUD::PlayAnim()
{
	// Move toasty off screen
	ToastyHUD::Update();
	
	// Check for custom image and sound
	ToastyHUD::GetImageAndSound();
	
	Schedule::Add("playSound(\"Toasty.wav\");", 0.25);
	Schedule::Add("playSound(\"turretoff4\");", 0.4);
	
	// TOOASTY!
	Schedule::Add("ToastyHUD::Animate('out');", 0.2);
	Schedule::Add("ToastyHUD::CenterPrint();", 0.2);
	Schedule::Add("ToastyHUD::Screenshot();", 0.2); // toggles on true only
	Schedule::Add("localSound($SND_NAME);", 0.3);
	Schedule::Add("ToastyHUD::Animate('in');", 1.3);
	Schedule::Add("$ToastyHUD::AccoladeMsg = '';", 1.6);
}

function ToastyHUD::TestMsg(%val)
{
	// Fix to not count toasty's while playing a demo
	if ($playingDemo)
		return;
	
	// Fake random Dummy names
	%dumbName = "Dummy" @ floor(getRandom() * (100 - 0.01));
	
	//%msg = sprintf("%1 lands [ 50 meter ] mid-air on %2!", "Dummy", "Doofus");
	%msg = sprintf("%1 lands [ " @ %val @ " meter ] mid-air on %2!", $PCFG::Name, %dumbName);
	//%msg = sprintf("%1 lands [ 50 meter ] mid-air on %2!", "Dummy", $PCFG::Name);
	
	if( (String::findSubStr( %msg, "mid-air")!=-1) && String::Starts( %msg, $PCFG::Name) )
	{
		// Get meters and shooter/victim pairs
		%msg = String::getSubStr( %msg, 0, String::Length( %msg )-1 ); //remove "!"
		%meters = $MA_GET_METER = GetMidAirMeter(%msg);
		%pairs = String::Trim( String::Replace( %msg, sprintf(" lands [ %1 meter ] mid-air on ", %meters), ", " ) );
		String::explode(%pairs, ", ", "player");
		
		// Gather data
		%mission = $ServerMission;
		%victim = $MA_VICTIM = $player[1];
		%shooter = $MA_SHOOTER = $player[0];
		%timestamp = ToastyHUD::Timestamp();
		
		// Count total MA for map
		$ToastyHUD::_MapTotal[%mission]++;
		//$ToastyHUD::_MidAirMeter[%mission] = %meters;
	
		if (%meters >= $MA_METER_DIST)
		{
			// Play the animation!
			ToastyHUD::PlayAnim();
		
			// Check for new record
			ToastyHUD::CheckNewRecord(%meters);
			
			// Add first 5 MA'S per map
			ToastyHud::AddTopFive(%mission, %meters, %victim);
			
			// Check for top 5 toasty's
			ToastyHUD::CheckForTop5(%meters, %victim);
		
			// Display some coolness!
			ToastyHUD::CenterPrint();
		
			// Export TOASTY stats
			ToastyHUD::ExportStats(%mission, %victim, %timestamp, %meters);
		}
	}
}

function ToastyHUD::MidAirMsg( %msg )
{
	// Fix to not count toasty's while playing a demo
	if ($playingDemo)
		return;
	
	if( (String::findSubStr( %msg, "mid-air")!=-1) && String::Starts( %msg, $PCFG::Name) )
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
		
		// Count total MA for map
		//$ToastyHUD::Count[%mission]++;
		$ToastyHUD::_MapTotal[%mission]++;
		$ToastyHUD::_MidAirMeter[%mission] = %meters;
	
		if (%meters >= $MA_METER_DIST)
		{
			// Play the animation!
			ToastyHUD::PlayAnim();
		
			// Check for new record
			ToastyHUD::CheckNewRecord(%meters);
			
			// Add first 5 MA'S per map
			ToastyHud::AddTopFive(%mission, %meters, %victim);
			
			// Check for top 5 toasty's
			ToastyHUD::CheckForTop5(%meters, %victim);
		
			// Display some coolness!
			ToastyHUD::CenterPrint();
		
			// Export TOASTY stats
			ToastyHUD::ExportStats(%mission, %victim, %timestamp, %meters);
		}
	}
}

function ToastyHUD::ShowLeaderboard()
{ 
	%dup = String::Dup("\t", 0);
	%top5 = ToastyHUD::GetTop5();
	
	%menu = "<JC>" @ %dup @ "\n=== Toasty Top 5 Scoreboard ===\n\n" @
			"-------------------------------------------\n\n" @
			"Top 5 toasty's on <f2>" @ $ServerMission @ "<f1>\n\n" @
			"Meters\t\t\tVictim        \n\n" @
			%top5 @ "" @
			"<JC>\n-------------------------------------------\n\n" @
			$ServerMission @ " Toasty Ratings:\n\n" @
			"Blah\n\n" @
			"Blah\n";
			
	
	remoteEP( %menu, 9999, true, 25, 20, 600 );
}

function ToastyHUD::GetTop5()
{
	%str = "";

	for(%i = 5; %i >= 1; %i--)
	{
		if($ToastyHUD::_Top5[$ServerMission, %i] != "") {
			%meter = String::AlignTextLeft( String::lpad( $ToastyHUD::_Top5[$ServerMission, %i], 3, "0") );
			%names = "<f2>" @ $ToastyHUD::_Top5Victim[$ServerMission, %i] @ "<f1>";
			%str = %str @ "  " @ %meter @ "       " @  %names @ "\n";
		} else {
			%str = %str @ "    ---       <f2>NOT FOUND<f1>\n";
		}
	}
	
	return %str;
}

function String::AlignTextLeft(%text)
{
	%textWidth = 12;
	%dup = String::Dup("\t", 13);
	
	for(%i=0; String::getSubStr(%text,%i,1) != ""; %i++)
		if(%i % %textWidth == 0)
			%str = %dup @ "<JL>" @ " " @ %str @ String::Trim( String::getSubStr(%text, %i, %textWidth) );
	
	return %str;
}

function ToastyHUD::CenterPrint()
{
	// Get random ownage messages
	%praises = "spanked smacked pwned obliterated popped hit owned";
	String::explode(%praises, " ", "praise");
	
	// Gather data
	%mission = $ServerMission;
	%timestamp = ToastyHUD::Timestamp();
	%toastys = ToastyHUD::TotalPerMap(%mission);
	%meters = $MA_GET_METER;
	
	// Get shooter and victim
    %shooter = ($MA_SHOOTER == $PCFG::Name ? "You" : $MA_SHOOTER);
	%victim = ($MA_VICTIM == $PCFG::Name ? "you" : $MA_VICTIM);

	// Random stuff (literally)
	%randNum = floor(getRandom() * 6);
	%rmsg = sprintf("TOOASTY! <F2>%1<F1> %2<F2> %3<F1> at<F2> %4<F1> meters!", %shooter, $praise[%randNum], %victim, %meters);
	
	if($ToastyHUD::NewTop5Found && $ToastyHUD::NewRecord) {
		// Por que no los dos? :)
		$ToastyHUD::NewTop5Found = false;
		$ToastyHUD::NewRecord = false;
		$ToastyHUD::AccoladeMsg = "\n\n New Top 5 and Record Found!";
	} else if($ToastyHUD::NewRecord) {
		$ToastyHUD::NewRecord = false;
		$ToastyHUD::AccoladeMsg = "\n\n New Record Found!";
	} else if($ToastyHUD::NewTop5Found) {
		$ToastyHUD::NewTop5Found = false;
		$ToastyHUD::AccoladeMsg = "\n\n New Top 5 Found!";
	}
	
	// Center print the text (Screenshot or default)
	if($pref::ToastyScreenshot)
		%text = "<JC><F1>" @ %rmsg @ "\n\n<f2> " @ %mission @ " <f1>\n\n" @ %timestamp @ String::toUpper( $ToastyHUD::AccoladeMsg );	
	else
		%text = "<JC><F1>" @ %rmsg @ "\n\n<f2> " @ %mission @ "<f1> Toasty's: " @ %toastys @ String::toUpper( $ToastyHUD::AccoladeMsg );
	
	// Display the output
	remoteBP( 2048, %text, 4 );
	
	// Less clutter
	remoteEval(2048, scoresOff);
}

// Code borrowed from Anubus MidAir SS script
// Modified by Link
function ToastyHUD::Screenshot()
{
	if($pref::ToastyScreenshot && $MA_SHOOTER == $PCFG::Name) 
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
		Schedule::Add("screenShot(MainWindow);", 0.1);
		Schedule::Add("canvas::repaint();", 0.1);
	}
}

function ToastyHUD::Timestamp()
{
	// Create timestamp array
	if($Time["mo"] == "")
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

function ToastyHUD::ExportStats(%mission, %victim, %time, %meters)
{
	if(%mission == "" && %victim == "" && %time == "" && %meters == "")
	{
		// Export the stats ONLY
		export("$ToastyHUD::_*", "config\\toasty_stats.cs", false);
		return;
	}
	
	if($MA_SHOOTER == $PCFG::Name)
	{
		%num = $ToastyHUD::BMCount++;
		
		// Check if recording, timestamp the demo
		%DemoTime = ($recordDemo == true ? " | " @ $ToastyHUD::DemoTime : "");
		
		// Format the string
		$ToastyBM[%num] = sprintf("%1 | %2 | %3 | %4M%5", %mission, %victim, %time, %meters, %DemoTime);
		
		// Export toasty bookmarks
		export(ToastyBM@%num, "config\\toasty_bookmark.cs", true);
		
		// Export the stats
		export("$ToastyHUD::_*", "config\\toasty_stats.cs", false);
	}
}

function ToastyHUD::BackupStats()
{
	File::copy("config\\toasty_stats.cs", "config\\toasty_stats_bak.cs");
}

function ToastyHUD::onMatchStarted() 
{
   Schedule::Add("ToastyHUD::checkTime();", 30);
}

function ToastyHUD::checkTime( %when ) 
{
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
   }
   
   if($ToastyHUD::secondsLeft == 0)
	   $ToastyHUD::matchStarted = true;

   $ToastyHUD::secondsLeft--;
   Schedule::Add("ToastyHUD::checkTime(" @ %when @ ");", 1);
}

function ToastyHUD::TotalPerMap(%map)
{	
	if($ToastyHUD::_MapTotal[%map] == "") {
		$ToastyHUD::_MapTotal[%map] = "";
		$ToastyHUD::_Record[%map] = 0;
		$ToastyHUD::_MidAirMeter[%map] = 0;
		
		// Does this exist already?
		if($ToastyHUD::_Top5[%map, 1] == "") {
			for(%i = 1; %i <= 5; %i++) {
				$ToastyHUD::_Top5[%map, %i] = 0;
				$ToastyHUD::_Top5Victim[%map, %i] = "";
			}
		}
	}
	
	return $ToastyHUD::_MapTotal[%map];
}

function ToastyHUD::CheckNewRecord(%val)
{
	%mission = $ServerMission;
	
	if($ToastyHUD::_Top5[%mission, 5] < %val && !$ToastyHUD::NewRecord) {
		$ToastyHUD::NewRecord = true;
		$ToastyHUD::_Record[%mission] = %val;
	}
}

// This will check against the first 5 MA'S that you get
// and adds them to the mission variable. The next one
// to come behind is ToastyHUD::CheckForTop5(); which
// will check every toasty and update accordingly.
function ToastyHud::AddTopFive(%map, %val, %vic)
{
	if($ToastyHUD::FoundAllTop5)
		return;
	
	// We reached all top 5, let's sort them out
	if($ToastyHUD::Top5Count++ > 5) {
		ToastyHUD::Sort( $ToastyHUD::_Top5[%map, 1],
			$ToastyHUD::_Top5[%map, 2],
			$ToastyHUD::_Top5[%map, 3],
			$ToastyHUD::_Top5[%map, 4],
			$ToastyHUD::_Top5[%map, 5],
			true
		);
		
		$ToastyHUD::Top5Count = 0;
		$ToastyHUD::FoundAllTop5 = true;
		return;
	}
	
	%cnt = $ToastyHUD::Top5Count;
	
	// Let's gather the top 5 toasty's
	if($ToastyHUD::_Top5[%map, %cnt] == "") {
		$ToastyHUD::_Top5[%map, %cnt] = %val;
		$ToastyHUD::_Top5Victim[%map, %cnt] = %vic;
	}
}

function ToastyHUD::Top5Test()
{
	%a1 = 69;
	%a2 = 72;
	%a3 = 84;
	%a4 = 97;
	%a5 = 120;
	
	ToastyHud::AddTopFive( $ServerMission, %a1 );
	ToastyHud::AddTopFive( $ServerMission, %a2 );
	ToastyHud::AddTopFive( $ServerMission, %a3 );
	ToastyHud::AddTopFive( $ServerMission, %a4 );
	ToastyHud::AddTopFive( $ServerMission, %a5 );
}

function ToastyHUD::Sort(%num0, %num1, %num2, %num3, %num4, %write)
{
	%str = "";
	
	$Toasty::Sort[1] = %num0;
	$Toasty::Sort[2] = %num1;
	$Toasty::Sort[3] = %num2;
	$Toasty::Sort[4] = %num3;
	$Toasty::Sort[5] = %num4;
	
	for(%i = 1; %i <= 5; %i++)
	{
		if($Toasty::Sort[%i] > $Toasty::Sort[(%i+1)]) {
			%temp = $Toasty::Sort[%i];
			$Toasty::Sort[%i] = $Toasty::Sort[(%i+1)];
			$Toasty::Sort[(%i+1)] = %temp;
		}
		
		%str = %str @ $Toasty::Sort[%i] @ ", ";
		
		if(%write) {
			$ToastyHUD::_Top5[$ServerMission, %i] = $Toasty::Sort[%i];
			if(%i == 5) { ToastyHUD::ExportStats(); }
		}
	}
}

function ToastyHUD::CheckForTop5(%val, %vic)
{
	// Have we found all top 5 toasty's yet?
	if(!$ToastyHUD::FoundAllTop5)
		return;
	
	%map = $ServerMission;

	$ToastyHUD::_MidAirMeter[%map] = %val;
	
	for(%i = 5; %i >= 1; %i--)
	{
		if($ToastyHUD::_Top5[%map, %i] < %val) {
			// We found a new top 5! Let's get it settled
			//echoc(3, "NEW TOP 5 FOUND!");
			//echoc(2, %i);
			$ToastyHUD::_Top5[%map, %i] = %val;
			$ToastyHUD::_Top5Victim[%map, %i] = %vic;
			$ToastyHUD::NewTop5Found = true;
			break;
		}
	}
	
	// Sort Top 5 scores and export stats
	if($ToastyHUD::NewTop5Found && %val > 0) {
		ToastyHUD::Sort( $ToastyHUD::_Top5[%map, 1],
			$ToastyHUD::_Top5[%map, 2],
			$ToastyHUD::_Top5[%map, 3],
			$ToastyHUD::_Top5[%map, 4],
			$ToastyHUD::_Top5[%map, 5],
			true
		);
	}
}

function ToastyHUD::CheckTop5NextMission()
{
	// Let's see if this exists...	
	if($ToastyHUD::_Top5[$ServerMission, 5] != "")
	{
		echo("Found Top5 stats for this map.");
		$ToastyHUD::Top5Count = 6; // needs to be +1 to work
		$ToastyHUD::FoundAllTop5 = true;
		return;
	}
	
	// No top 5 found
	$ToastyHUD::Top5Count = 0;
	$ToastyHUD::FoundAllTop5 = false;
}

function ToastyHUD::GetImageAndSound()
{
	if(!$pref::ToastyCustomMode) {
		// Custom image
		if($IMAGE_NAME == "")
			$IMAGE_NAME = "toasty.png";
		
		// Custom sound
		if($SND_NAME == "")
			$SND_NAME = "mk.toasty.ogg";
	}
	
	// Set the image (Custom or default)
	Control::SetValue("ToastyHUD::Image0", "<B0,0:Modules/ToastyHUD/"@$IMAGE_NAME@">");
}

function ToastyHUD::Cleanup()
{
	DeleteVariables("$ToastyHUD::_*");
	if(exec("Modules\\ToastyHUD.acs.cs")) {
		echoc(3, "ToastyHUD: Clean-up complete");
		exec("toasty_stats.cs");
	}
}

function ToastyHUD::onUpdateTime( %min, %secs ) 
{
   $ToastyHUD::secondsLeft = %min * 60 + %secs;
   $ToastyHUD::when = getSimTime();
   $ToastyHUD::secondsLeft = floor($ToastyHUD::secondsLeft);
   Schedule::Add("ToastyHUD::checkTime(" @ $ToastyHUD::when @ ");", 1);   
}

function GetMidAirMeter(%val)
{
	for (%i = 0; (%num = String::Trim( getWord(%val, %i) ) ) != -1; %i++)
		if(isNum(%num)) return %num;
	
	return -1;
}

function isNum(%val)
{
	return(chr(%val)!="" ? true : false);
}

function canvas::repaint()
{
	for(%i = 0; %i <= $css::num; %i++)
		Control::setVisible($css::visibleObjects[%i], true);
	deleteVariables("$css::*");
}

ToastyHUD::Init();

Event::Attach( eventServerMessage, ToastyHUD::MidAirMsg );
Event::Attach( eventMatchStarted, ToastyHUD::onMatchStarted );
Event::Attach( eventUpdateTime, ToastyHUD::onUpdateTime);
Event::Attach( eventExit, ToastyHUD::BackupStats );
Event::Attach( eventConnectionAccepted, ToastyHUD::CheckTop5NextMission );
Event::Attach( eventChangeMission, ToastyHUD::CheckTop5NextMission );