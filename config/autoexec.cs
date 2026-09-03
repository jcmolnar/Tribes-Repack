//Don't write your own scripts in here, just place execs.
//Keep in mind this file may be replaced in future Tribes Repack updates.
// Native rebuild: corrected defaults (net send budget, AI nav + acquisition, server log buffer).
// Kept in its own file so a repack update to autoexec.cs cannot silently drop the fixes.
// NATIVE FIX 2026-08-27: one flag for "this boot is plain base, no -mod".
// Same predicate console.cs:241 uses to pick saeModern.cs, and it is a word-count
// test ON PURPOSE: a launcher that passes "-mod base" explicitly builds the list as
// `%mod @ " " @ $modList` and leaves a TRAILING SPACE, so "base " != "base".
// Read by the RPG-flavoured binds below and by presto's JobMenu in Install.cs.
$KV::isBase = false;
if(getWord($modList, 0) == "base" && getWord($modList, 1) == -1)
	$KV::isBase = true;

exec("nativeDefaults.cs");
exec("SinConnect.cs");
exec("presto\\install.cs");
// CustomConfigs v2: CmdHUD is an in-game hud -- yielded to an active config overlay too.
if($Config::Name == "")
	Include("cowboy\\CmdHUD.cs");

// NATIVE FIX 2026-09-02 (Joe: "hit MAP in the command screen and everything disappeared").
// command.gui bakes "CmdHUD::CmdButtonPress();" on the MAP tab, and cowboy's version toggles
// its 1.40 small-map mode: it deletes the live FearGui::TSCommander, news a 204x204 one
// inside SmallMapFrame and shows the world behind it. Under the native full-screen layout
// (CmdHUD::FullLayout resizes BOTH frames to the whole screen) that leaves a bare world
// with the TeeniCam in the corner and no map at all. Retail's MAP tab is the tab you are
// already on: make it exactly that. Must sit AFTER the Include -- same-named console
// functions silently replace, last definition wins.
function CmdHUD::CmdButtonPress() {
	if ($CmdHUD::guiOpen == 2)
		return;
	remoteEval(2048, CommandMode);
	CmdHUD::SetGui(2);
	}

// FIX: game boots in the Software renderer even though ClientPrefs
// saves $pref::VideoWindowedDriver = "OpenGL". console.cs applies the
// video prefs (OptionsVideo::apply) immediately after creating the
// window, which is too early for the OpenGL device to take - the same
// call works fine later (hitting Apply in the options screen). So:
// re-assert the windowed driver a few seconds after startup. We hook
// OptionsVideo::validate (called right before the startup apply, when
// the console scheduler exists) and arm a one-shot delayed re-apply.
// Body below mirrors the stock Options.cs validate exactly.
function OptionsVideo::validate()
{
	if($pref::VideoFullScreen == "") $pref::VideoFullScreen = "FALSE";
	if($pref::VideoFullScreenDriver == "")
	{
		if(isGfxDriver(MainWindow, "Glide")) $pref::VideoFullScreenDriver = "Glide";
		else $pref::VideoFullScreenDriver = "Software";
	}
	if($pref::VideoWindowedDriver == "") $pref::VideoWindowedDriver = "Software";
	if($pref::VideoFullScreenRes == "") $pref::VideoFullScreenRes = "640x480";
	if($pref::VideoGamma == "") $pref::VideoGamma = "0.65";

	if(!$KV::reapplyArmed)
	{
		$KV::reapplyArmed = true;
		schedule("KronosVideo::reapply();", 3);
	}

	// Which keybind set this session booted into (BindSet::report, nativeDefaults.cs).
	// Armed HERE for the same reason KronosVideo::reapply is: this is the first point in the
	// boot chain where ConsoleScheduler exists -- console.cs creates it AFTER exec'ing
	// autoexec.cs, so scheduling from a file's top level is silently dropped. The delay also
	// puts the line past the point where console logging opens.
	if(!$BindSet::reportArmed)
	{
		$BindSet::reportArmed = true;
		schedule("BindSet::report();", 3);
	}
}

function KronosVideo::reapply()
{
	// only re-assert in windowed mode, and never downgrade to Software
	if(String::ICompare($pref::VideoFullScreen, "TRUE") == 0)
		return;
	if($pref::VideoWindowedDriver == "" || $pref::VideoWindowedDriver == "Software")
		return;
	echo("KronosVideo: re-applying windowed driver '" @ $pref::VideoWindowedDriver @ "'");
	setWindowedDevice(MainWindow, $pref::VideoWindowedDriver);
}

// Auto-attack toggle variable
$PrestoPref::AutoAttack = false;

// Auto-attack loop function (called repeatedly while enabled)
function PrestoAutoAttackLoop() {
	if($PrestoPref::AutoAttack) {
		postAction(2048, IDACTION_FIRE1, 1);
		// Schedule next call for continuous firing
		schedule("PrestoAutoAttackLoop();", 0.1);
	}
}

// Auto-attack toggle function
function PrestoAutoAttackToggle() {
	if($PrestoPref::AutoAttack) {
		$PrestoPref::AutoAttack = false;
		postAction(2048, IDACTION_BREAK1, 1);
		echo("Auto-attack: OFF");
		Client::centerPrint("<jc><f0>Auto-attack: <f1>OFF", 1);
		Schedule("Client::centerPrint(\"\", 1);", 2);
	}
	else {
		$PrestoPref::AutoAttack = true;
		echo("Auto-attack: ON");
		Client::centerPrint("<jc><f0>Auto-attack: <f1>ON", 1);
		Schedule("Client::centerPrint(\"\", 1);", 2);
		// Start the auto-attack loop
		PrestoAutoAttackLoop();
	}
}

// Edit action map for keybinds
editActionMap("playMap.sae");

// Mouse wheel weapon switching -- a ONE-TIME seed, not a boot-time reclaim.
//
// NATIVE FIX 2026-09-01 (Joe: "unbinding next/prev weapon in Controls looks unbound but the
// wheel still switches weapons"): this file runs LAST in the boot chain, after the saved
// config.cs has rebuilt playMap.sae exactly as the player left it, and it used to bindCommand
// the wheel here unconditionally -- so a deliberate unbind (or a rebind of the wheel to
// something else) was overwritten on the very next boot. The Options rows for Next/Prev
// Weapon edit playMap.sae, which is where this bound, so the row even SHOWED "Wheel Up"
// again after the restart.
//
// Now: seed the wheel ONLY on a fresh install -- no saved config.cs yet -- and only once
// (bindCommandDefault never displaces an existing bind for the command or the key; the
// persisted pref stops it running again). A returning player's config.cs already carries
// whatever the wheel does for them, because the old unconditional bind was re-exported into
// it on every quit -- so an update changes nothing for them, and an unbind made in Controls
// is never put back. saeModern.cs still ships the same pair inside the preset, so
// "Defaults" restores it.
if($pref::wheelWeaponSeeded == "")
{
	if(File::findFirst("config\\config.cs") == "")
	{
		bindCommandDefault(mouse0, zaxis0, TO, "nextWeapon();"); //Wheel forward
		bindCommandDefault(mouse0, zaxis1, TO, "prevWeapon();"); //Wheel backward
	}
	$pref::wheelWeaponSeeded = 1;
}

// Auto-attack toggle on J.
//
// WAS x, which silently ate a core action in every preset. This file is exec'd LAST
// (console.cs:232 -- after sae*.cs, after config.cs, after extra-controls.cs), so a bindCommand
// here overwrites whatever the active keybind set put on that key. x is IDACTION_VIEW in
// saeBase and IDACTION_CROUCH in saeRPG/saeModern; both were being lost with no error.
// j is bound nowhere in any play map (editorconfig.cs uses it, but that is the mission-editor
// map under -edit only). Not l -- config.cs has it on LocalSkin::menu().
// NATIVE FIX 2026-08-27: autofire is an RPG-grind convenience, not a base-game
// control, and this file binds it AFTER every preset -- so in base it was taking a
// key away from the stock set for a feature base players do not use. Mod boots keep
// it exactly as before.
if(!$KV::isBase)
	bindCommandDefault(keyboard0, make, "j", TO, "PrestoAutoAttackToggle();");

// CustomConfigs v2: when a 1.4 config overlay is active ($Config::Name set by the exe
// before boot), the CONFIG owns the in-game HUD -- skip the whole KronosHUD suite so
// the two don't fight (double chat boxes, K/TAB menu grabs, duplicate vhud bars).
// Blank curconfig.txt = normal Kronos HUD, exactly as before.
if($Config::Name == "")
{
//If you would like to try the ScriptGL example HUDs, uncomment the next line:
exec("scriptgl2.cs");
exec("Presto\\KronosHUD.cs");

// Chat category filters - toggle with ChatFilter::toggle(adv) etc.
// Set $ChatFilter::Hide["adv"] = true; here (before the exec) to
// persist a filter across sessions.
exec("Presto\\ChatFilter.cs");

// Shared ScriptGL text-input field (keyboard capture for chat composer,
// search/amount boxes). Requires the native build's glTextInput + key
// forwarding (scriptGL.cpp / simGame.cpp). Load before the consumers.
exec("Presto\\KronosInput.cs");

// Modern ScriptGL TAB menu (KronosMenu::disable() reverts to stock)
exec("Presto\\KronosMenu.cs");

// Modern shop/inventory screen (must load AFTER KronosMenu.cs - it
// overrides the shared ScriptGL::playGui::onPostDraw hook)
exec("Presto\\KronosShop.cs");

// Custom ScriptGL chat overlay (adjustable font size + line count,
// draggable; hides the stock chat). Loads AFTER the others.
exec("Presto\\KronosChat.cs");

// Modern NPC dialogue window (server pushes lines + options for HUD
// clients via KronosNPC_Server.cs).
exec("Presto\\KronosNPC.cs");

// ScriptGL quick command menu (V) - replaces the engine chat menu that the
// Kronos overlay's chat-hiding made invisible.
exec("Presto\\KronosCM.cs");

// Session stats panel (XP/hr, gold/hr) - client-side, draggable.
exec("Presto\\KronosStats.cs");
}

//scriptgl2.cs contains an implementation of "variable sized HUDs",or vhuds,
// i.e. the HUDs resize themselves to your resolution; they will look the same
// in 640x480 or 1600x1200, just like their Half Life 2 counterparts.
// --quoted from, and more info at, http://www.team5150.com/~andrew/project.hudbot/

// Suppress the "Presto Pack" + "CmdHUD 1.3a" advertising boxes on the main menu.
// MainMenuGui::onOpen (config\Presto\Install.cs:301) creates both boxes and early-outs
// on `if ($PrestoPrefs::ShowPackStatus == false) return;` -- but that gate reads the
// TYPO'd var $PrestoPref(s):: (extra 's') while every setter writes $PrestoPref:: (no
// 's'), so Presto's own toggle never worked. Set the variable the gate actually reads.
$PrestoPrefs::ShowPackStatus = false;

