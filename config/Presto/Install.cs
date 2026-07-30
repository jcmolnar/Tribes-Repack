// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// Install.CS								EnablePresto, March '99 
//
//	This script contains the functions I use to install the pack on
//	your machine.  So I'm not really going to document it very well,
//	because no one else need to use it.
//
//	Two important variables to mention:
//		$Presto::installed is set to true when my script is installed.
//		$Presto::version is set to the current version number...
//
//	Also script writers might want to add banners to their scripts.  I will
//	rotate through them on the main page (starting at a random one to even
//	the playing field ;) ).  Use the call
//		Presto::AddScriptBanner(<tag>, <message>);
//	where <tag> is a unique tag (you could use the name of your script, or
//	your own name, or whatever.)  If you re-run the function with the same
//	tag, the text will be replaced!
//	You have about 9 lines of text to work with, and maybe 30 characters
//	across each line.
//
//	Script banners are formatted text, so you can use font changes, bitmaps,
//	etc.  But keep in mind that the GUI pallette is different than the play
//	pallette so in-game bitmaps won't look right.
//
// ---------------------------------------------------------------------------
if (!$dedicated) {	// DEDICATED SERVER

exec("presto\\Include.cs");
Include("presto\\Event.cs");
include("presto\\schedule.cs");
Include("presto\\upgrade\\clientmessage.cs");
Include("presto\\upgrade\\extra.cs");
Include("presto\\upgrade\\clientmessage.cs", force);

// Load previous prefs, so we can notice changes later.

function UninstallPrestoPack() {
	$PrestoPref::installStage = uninstall;
	exec("presto\\install.cs");
	}
function UninstallPrestoPak () { UninstallPrestoPack(); }

function InstallStage(%stage) {
	return $PrestoPref::installStage == %stage;
	}
$Presto::notice = "";
function Presto::AddNoticeLine(%line) {
	$Presto::notice = $Presto::notice @ " " @ %line @ "\n";
	}
$Presto::status = "";
function Presto::AddStatusLine(%line) {
	$Presto::status = $Presto::status @ " " @ %line @ "\n";
	}
function Presto::AddOptionStatusLine(%option, %line) {
	if (String::FindSubStr($Presto::InterestingOptions, " "@%option@" ") != -1)
		Presto::AddStatusLine(%line);
	}
function Presto::AddScriptBanner(%name, %str) {
	%num = $Presto::bannerNum[%name];
	if (%num != "")
		$Presto::banner[%num] = %str;
	else	{
		$Presto::bannerNum[%name] = ($Presto::banners+0);
		$Presto::banner[$Presto::bannerNum[%name]] = %str;
		$Presto::banners++;
		}
	}
$Presto::screenSize["320x240(V)"] = "320 240";
$Presto::screenSize["400x300(V)"] = "400 300";
$Presto::screenSize["480x360(V)"] = "480 360";
$Presto::screenSize["512x384"] = "512 384";
$Presto::screenSize["640x400"] = "640 400";
$Presto::screenSize["640x480"] = "640 480";
$Presto::screenSize["800x600"] = "800 600";
$Presto::screenSize["1024x768"] = "1024 768";
//===
$Presto::screenSize["1152x864"] = "1152 864"; //Added by Deus_ex_Machina -note I dunno why those screensizes where left out...
$Presto::screenSize["1280x720"] = "1280 720";
$Presto::screenSize["1280x960"] = "1280 960";
$Presto::screenSize["1280x1024"] = "1280 1024";
$Presto::screenSize["1600x900"] = "1600 900";
$Presto::screenSize["1600x1200"] = "1600 1200";
$Presto::screenSize["1600x1024"] = "1600 1024";
//===
function Presto::ScreenSize() {
	//res mod by phantom, only works in play.gui
	%val = Control::getExtent(PlayGui);
	if(getWord(%val,1) > 100){
		return %val;
	}
	%res = $pref::videoFullScreenRes;
	if ($pref::VideoFullScreen) {
		%posRes = $Presto::screenSize[%res];
		if (%posRes != "")
			return %posRes;
		//res mod by phantom
		%res = string::replace(%res, "x", " ");
		if(getWord(%res,1) > 100)
			return %res;
	}
	return $Presto::screenSize["640x480"];
}

EvalSearchPath();
if (!InstallStage(uninstall)) {
	if (isFile("config\\_lastPrestoPrefs.cs"))
		exec("_lastPrestoPrefs.cs");
	}

if (InstallStage("")) {
	$PrestoPref::ShowPackStatus = true;	// don't let them turn it off yet, in case of errors.

	if (isFile("config\\config.cs"))
		File::copy("config\\config.cs", "config\\config.pre");
	if (isFile("config\\autoexec.cs"))
		File::copy("config\\autoexec.cs", "config\\autoexec.pre");
	$Presto::FirstTime = true;

	Presto::AddNoticeLine("<f2>Welcome to the Presto Pack!<f0>");
	Presto::AddNoticeLine("See <f1>README.TXT<f0> for info if");
	Presto::AddNoticeLine("you're having trouble with");
	Presto::AddNoticeLine("installation or config.\n");
	Presto::AddNoticeLine("If you haven't already, why");
	Presto::AddNoticeLine("not go set your preferences?");
	Presto::AddNoticeLine("(config\\presto\\PrestoPrefs.cs)");

	$PrestoPref::installStage = errorcheck;
	}

if (InstallStage(errorcheck)) {
// Maybe someday I can do error checking - but I'm not sure
// what to check for right now.
//	Presto::AddNoticeLine("<f1>Everything seems to be");
//	Presto::AddNoticeLine("installed correctly...<f0>");
	$PrestoPref::installStage = installed;
	}

if (InstallStage(installed)) {
	$Presto::installed = true;
	$Presto::version = "0.933";	// numbers have to be quoted.  otherwise, they get
		//	weird values like 0.9299999999 because of stupid float inaccuracy. :(

	function Presto::Enabled(%option) {
		if ($PrestoPref::[%option] == "")
			$PrestoPref::[%option] = false;
		if ($PrestoPref::[%option] != false)
			return true;
		return false;
		}
	function bindkey(%map, %key, %functionMake, %functionBreak) {
		editActionMap(%map@"Map.sae");
	
		%i = 0;
		while (getWord(%key, %i) != -1) {
			%p[%i] = getWord(%key, %i);
			%i++;
			}
		%p[%i] = "TO";
		%p[%i + 1] = %functionMake;
		bindCommand(keyboard0, make, %p0,%p1,%p2,%p3,%p4,%p5,%p6,%p7,%p8);
		%p[%i + 1] = %functionBreak;
		bindCommand(keyboard0, break, %p0,%p1,%p2,%p3,%p4,%p5,%p6,%p7,%p8);
		return true;
		}
	function PrestoKeyChanged(%option) {
		if ($PrestoPref::[%option,was] != "" &&
		    $PrestoPref::[%option,was] != false &&
		    $PrestoPref::[%option,was] != $PrestoPref::[%option]) {
			return true;
			}
		return false;
		}
	function Presto::EnableOption(%option) {
		if (Presto::Enabled(%option)) {
			Presto::AddOptionStatusLine(%option, " <f1>"@%option@"<f0> is <f1>enabled<f0>.");
			return true;
			}
		Presto::AddOptionStatusLine(%option, " <f1>"@%option@"<f0> is disabled.");
		return false;
		}
	function Presto::EnableKey(%option, %map, %functionMake, %functionBreak) {
		%key = $PrestoPref::[%option];
	
		if (PrestoKeyChanged(%option)) {
			// They changed a key.  Unbind the old one.
			Presto::AddNoticeLine("Unbound old <f1>"@%option@"<f0> key.");
			bindKey(%map, $PrestoPref::[%option,was]);
			}
		$PrestoPref::[%option,was] = %key;
	
		if (Presto::Enabled(%option)) {
			Presto::AddOptionStatusLine(%option, " <f1>"@%option@"<f0> is on <f1>"@%key@"<f0>.");
			bindKey(%map, %key, %functionMake, %functionBreak);
			return true;
			}
		Presto::AddOptionStatusLine(%option, " <f1>"@%option@"<f0> is disabled.");
		return false;
		}
	function Presto::OptionDisplay(%option,%label) {
		if (Presto::EnableOption(%option))
			return "<f1>"@%label;
		return "<f0>"@%label;
		}

	exec("presto\\PrestoPrefs.cs");
	$Presto::InterestingOptions = " InvCamera DynHud OldChat NewChat DropMenu JobMenu TeamHud MuteMenu ";

	if (Presto::EnableOption(InvCamera)) {
		Include("presto\\CamHud.cs");
		Presto::EnableKey(CamHudFreeLook, pda, "CamHUD::FreeLook(true);", "CamHUD::FreeLook(false);");
		}
	
	for (%i=0;%i<1000;%i++) {} //some people have been having problems that only a slight delay seems to fix :o\
	Presto::EnableKey(OldChat, play, "Menu::DisplayDefault();");
	if (Presto::EnableKey(NewChat, play, "Menu::Display(menuChat);"))
	{
		Include("presto\\Say.cs");
		Include("presto\\Chat.cs");
	}
	
	for (%i=0;%i<1000;%i++) {} //some people have been having problems that only a slight delay seems to fix :o\
	if (Presto::EnableKey(DropMenu, play, "Menu::Display(menuDrop);"))
		Include("presto\\Drop.cs");
	
	for (%i=0;%i<1000;%i++) {} //some people have been having problems that only a slight delay seems to fix :o\
	if (Presto::EnableKey(JobMenu, play, "Job::Start();"))
		Include("presto\\Chores.cs");

	for (%i=0;%i<1000;%i++) {} //some people have been having problems that only a slight delay seems to fix :o\
	if (Presto::EnableKey(DynHud, play, "HUD::ToggleDisplay(hudDyn);"))
		Include("presto\\DynHud.cs");
	
	for (%i=0;%i<1000;%i++) {} //some people have been having problems that only a slight delay seems to fix :o\
	if (Presto::EnableKey(TeamHud, play, "TeamHUD::ToggleDisplay();"))
		Include("presto\\TeamHud.cs");

	for (%i=0;%i<1000;%i++) {} //some people have been having problems that only a slight delay seems to fix :o\
	if (Presto::EnableKey(MuteMenu, play, "Menu::Display(menuMute);"))
		Include("presto\\MuteMenu.cs");
	}

// Auto-save on level up (always enabled)
Include("presto\\LevelUpSave.cs");

// Floating damage numbers (always enabled)
Include("presto\\ATKText.cs");

// Save these current keys so we can recognize edits later.
export("$PrestoPref::*", "config\\_lastPrestoPrefs.cs", false);

function CycleScriptBanner(%crc, %update) {
	// cancel the schedule if we closed the main menu
	if (%crc != $Presto::mainMenuCrc)
		return;

	if (($Presto::banners+0)==0)
		return;

	%favorite = $Presto::bannerNum[$PrestoPref::FavoriteBanner];
	if (%favorite != "") {
		$Presto::currentBanner = %favorite;
		return;
		}

	if ($Presto::currentBanner == "")
		$Presto::currentBanner = floor(getRandom * $Presto::banners);
	else	{
		$Presto::currentBanner++;
		if ($Presto::currentBanner == $Presto::banners)
			$Presto::currentBanner=0;
		}

	if (%update)
		Control::Setvalue(PrestoNoticeText, $Presto::banner[$Presto::currentBanner]);
	schedule("CycleScriptBanner("@%crc@", true);", $PrestoPref::BannerCycleTime);
	}
function rpStartup()
{//here because repack.cs is too early (important files not yet loaded)
	if($pref::PacketFrame == "")return;
	if($pref::PlayerFov < 1)return;
	%rpver = 35;
	if($repackver > 0)
		%rpver = $repackver;
	if($pref::lastRepack >= %rpver)return;
	$pref::packetrate=30;
	editActionMap("actionMap.sae");
	bindCommand(keyboard0, make, "n", TO, "sendControl(\"n\");");
	bindCommand(keyboard0, make, "q", TO, "sendControl(\"q\");");
	//if($pref::lastRepack < %rpver){
		updateListFilterFinal(8,"RPG MOD:","RPG MOD:5,0,rpg:3,1,Custom Ghetto");//Malicious flooding, harms players, needs to be stopped
		//updateListBanFinal("IP:174.59.0.42");//DoS attacks, connection spam, attempted exploits
		//updateListBanFinal("IP:24.224.203.178 . IP:174.54.226.130");//serverlist spam . ^newip
		$Server::Master1 = "t1m1.pu.net:28000 tribes.lock-load.org:28000 t1m1.kigen.co:28000 t1m2.kigen.co:28000 t1m1.tribesmasterserver.com:28000 t1m1.tribes0.com:28000 t1m1.masters.dynamix.com:28000 t1m2.masters.dynamix.com:28000 t1m3.masters.dynamix.com:28000";
		$Server::MasterAddressN0 = "t1m1.pu.net:28000 tribes.lock-load.org:28000 t1m1.kigen.co:28000 t1m2.kigen.co:28000 t1m1.tribesmasterserver.com:28000 t1m1.tribes0.com:28000 t1m1.masters.dynamix.com:28000 t1m2.masters.dynamix.com:28000 t1m3.masters.dynamix.com:28000";
		$Server::MasterAddressN1 = "t1m1.pu.net:28000 tribes.lock-load.org:28000 t1m1.kigen.co:28000 t1m2.kigen.co:28000 t1m1.tribesmasterserver.com:28000 t1m1.tribes0.com:28000 t1ukm1.masters.dynamix.com:28000 t1ukm2.masters.dynamix.com:28000 t1ukm3.masters.dynamix.com:28000";
		$Server::MasterAddressN2 = "t1m1.pu.net:28000 tribes.lock-load.org:28000 t1m1.kigen.co:28000 t1m2.kigen.co:28000 t1m1.tribesmasterserver.com:28000 t1m1.tribes0.com:28000 t1aum1.masters.dynamix.com:28000 t1aum2.masters.dynamix.com:28000 t1aum3.masters.dynamix.com:28000";
	//}
	$pref::lastRepack=%rpver;
}rpStartup();
function MainMenuGui::onOpen() {
	if ($PrestoPrefs::ShowPackStatus == false)
		return;

	%screenSize = Presto::ScreenSize();
	%width = getWord(%screenSize,0);
	%height = getWord(%screenSize, 1);

	%boxHeight = 125;

	if (!isObject(PrestoStatus))
		newObject(PrestoStatus, FearGui::FearGuiBox, 50,%height-45 - %boxHeight, 200,%boxHeight);
	if (!isObject(PrestoStatusText))
		newObject(PrestoStatusText, FearGuiFormattedText, 1,0,190,400);
	AddToSet(PrestoStatus, PrestoStatusText);
	AddToSet(MainMenuGui, PrestoStatus);

	Control::SetValue(PrestoStatusText, 
		" <f2>Presto Pack<jr><f0>version "@ $Presto::version @" <jl>\n" @ $Presto::status);

	$Presto::mainMenuCrc++;
	CycleScriptBanner($Presto::mainMenuCrc,false);
	if ($Presto::notice != "" || $Presto::currentBanner != "") {
		if (!isObject(PrestoNotice))
			newObject(PrestoNotice, FearGui::FearGuiBox, %width - 250,%height-45 - %boxHeight, 200,%boxHeight);
		if (!isObject(PrestoNoticeText))
			newObject(PrestoNoticeText, FearGuiFormattedText, 1,0,190,400);
		AddToSet(PrestoNotice, PrestoNoticeText);
		AddToSet(MainMenuGui, PrestoNotice);

		if ($Presto::notice != "")
			Control::SetValue(PrestoNoticeText, $Presto::notice);
		else	Control::Setvalue(PrestoNoticeText, $Presto::banner[$Presto::currentBanner]);
		}
	}
function MainMenuGui::OnClose() {
	$Presto::mainMenuCrc++;
	}

if (InstallStage(uninstall)) {
	if (isFile("config\\config.pre")) {
		File::copy("config\\config.pre", "config\\config.cs");
		File::delete("config\\config.pre");
		}
	if (isFile("config\\autoexec.pre")) {
		File::copy("config\\autoexec.pre", "config\\autoexec.cs");
		File::delete("config\\autoexec.pre");
		}
	if (isFile("config\\_lastPrestoPrefs.cs"))
		File::delete("config\\_lastPrestoPrefs.cs");
	if (isFile("temp\\tempCFG.cs"))
		File::delete("temp\\tempCFG.cs");
	function onExit() { }
	quit();
	}

}	// DEDICATED SERVER