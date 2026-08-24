// pack: observer  (Observer (filming))    -- HAND-AUTHORED ("authoring": "manual")
//
// A "config" for shooting gameplay footage and screenshots:
//   - every stock HUD hidden by default (each one still gets a K-panel row, so a
//     clock or the chat can be flipped back on for a particular shot)
//   - crosshair + nameplates hidden (crosshairHud owns BOTH; this pack hides the
//     control deliberately -- for footage the nameplate system going with it is
//     the point, and a K row brings it back)
//   - server prints (centerprint/bottomprint/topprint -- "Observing X", MOTDs)
//     suppressed through the engine's $HideServerText session global
//     (FearGuiCenterPrint.cpp; cleared natively when the pack is swapped away)
//   - auto-requests observer mode from the server (the stock Options ->
//     "Change Teams/Observe" -> Observer menu walk, driven by remoteEval), once,
//     guarded so it never fires into a foreign server menu
//
// Camera controls while observing (engine, for reference):
//   SPACE       toggle free-fly / orbit  (one toggle per press --
//               observerCamera.cpp edge-detects jumpAction)
//   RMB hold    3x fly speed             (jumpJet keeps only its jetting half in
//               observer -- kronosNativeCmds.cpp c_jumpJet -- so RMB no longer
//               yanks the camera between fly and orbit)
//   LMB         next player (orbit) / new vantage point (fly)
//   W/S         orbit distance in orbit mode, move in fly mode

exec("ModernHUD/Framework.cs");

$ModernHUD::Enabled = true;
$ModernHUD::Pack = "Observer (filming)";
$ModernHUD::PackId = "observer";

// This pack draws nothing, so it owns no slot; another pack's slot pref changes
// nothing here either way.
function ModernHUDPack::ownsSlot(%value)
{
   return false;
}

function ModernHUDPack::detachRetained()
{
   // Nothing replaced -- the stock huds are hidden, not superseded. Defined
   // because the framework calls it unconditionally.
}

function ModernHUDPack::prefs()
{
   // No forced look. Everything player-facing is a setting row.
}

//------------------------------------------------------------------------------
// Setting applies.
//------------------------------------------------------------------------------
// Shared bool read, framework idiom: an unset pref means "pack default", and the
// comparison stays on quoted strings so compare() never float-promotes
// (Framework.cs ModernHUD::stock has the whole story).
function ObserverCfg::prefOn(%name, %dflt)
{
   %v = getVariable(%name);
   if(%v == "") { return %dflt; }
   if(%v == "0" || %v == "false" || %v == "False") { return "0"; }
   return "1";
}

function ObserverCfg::applyNameplates()
{
   // crosshairHud drives the reticle AND the whole nameplate layer
   // (fearGuiCrosshair.cpp) -- hiding it is exactly what clean footage wants,
   // which is why this bypasses ModernHUD::stock (that chokepoint refuses to
   // hide crosshairHud, correctly, for every gameplay pack).
   if(ObserverCfg::prefOn("pref::ModernHUD::observer::Nameplates", "0") == "1")
      Control::SetVisible(crosshairHud, true);
   else
      Control::SetVisible(crosshairHud, false);
}

function ObserverCfg::applyServerText()
{
   // SESSION global, deliberately not a $pref:: -- the exit sweep would persist
   // it and a forgotten toggle would eat server text in normal play forever.
   // The engine clears it on pack unload too (modernHudPacks.cpp MHPacks_unload).
   if(ObserverCfg::prefOn("pref::ModernHUD::observer::ServerText", "0") == "1")
      $HideServerText = "";
   else
      $HideServerText = "1";
}

//------------------------------------------------------------------------------
// Force observer -- the stock menu walk, sent directly.
//------------------------------------------------------------------------------
// Server side this is: remoteScoresOn -> Game::menuRequest builds the "Options"
// menu; "changeteams" -> processMenuOptions builds "Pick a team:"; "-2" ->
// processMenuPickTeam -> Observer::enterObserverMode. Every step degrades to a
// no-op on servers that lack it (tourney lock, RPG mods without team menus).
// The leading "-2" handles the case where the initial "Pick a team:" menu is
// already up (its first entry IS Observe) -- ObserverCfg::maybeForce never gets
// here with a menu open, so that path only runs on a manual ObserverCfg::force().
function ObserverCfg::force()
{
   if(isObject(CurServerMenu))
      remoteEval(2048, "MenuSelect", "-2");
   remoteEval(2048, "ScoresOn");
   remoteEval(2048, "MenuSelect", "changeteams");
   remoteEval(2048, "MenuSelect", "-2");
   $ObserverCfg::SettleTries = 8;
   schedule("ObserverCfg::settle();", 1);
}

// The walk's own "Pick a team:" menu lands on OUR screen as a server push we
// never click, and it can arrive after any fixed cleanup delay (measured: a 1s
// deleteObject lost the race in the harness). Deleting the client-side ChatMenu
// alone is not enough anyway -- the server's menuMode stays set, which keeps the
// score panel and the chat area up. So resolve it the way a real click does:
// clientMenuSelect (menu.cs) deletes the menu AND sends the selection, and the
// observer code is idempotent server-side (enterObserverMode returns false once
// observing). Gated on isObserving() and bounded to the seconds right after our
// own request, so a foreign server menu is never eaten.
function ObserverCfg::settle()
{
   if($ObserverCfg::SettleTries == "" || $ObserverCfg::SettleTries <= 0)
      return;
   $ObserverCfg::SettleTries = $ObserverCfg::SettleTries - 1;
   if(isObserving() && isObject(CurServerMenu))
   {
      echo("[OBSERVER] resolving leftover pick-team menu");
      clientMenuSelect("-2");
   }
   schedule("ObserverCfg::settle();", 1);
}

// The guarded automatic attempt: only when the row says so, only when the server
// has not already made us an observer (isObserving -- native, kronosNativeCmds),
// never into an open server menu (an RPG dialog would eat the selections), and
// rate-limited so play-gui reopens (inventory screen flips etc.) cannot spam it.
// Echoes throughout: one line per gui transition at most, and they are the only
// way to see WHICH guard held a request back after the fact (the request itself
// is silent on both ends -- a wrong guess here cost a full harness cycle).
function ObserverCfg::maybeForce()
{
   if(ObserverCfg::prefOn("pref::ModernHUD::observer::AutoObserver", "1") == "0")
   {
      echo("[OBSERVER] auto request off");
      return;
   }
   if(isObserving())
   {
      echo("[OBSERVER] already observing");
      return;
   }
   if(isObject(CurServerMenu))
   {
      echo("[OBSERVER] server menu open -- not requesting");
      return;
   }
   %now = getRealMillis();
   if($ObserverCfg::LastTry != "" && (%now - $ObserverCfg::LastTry) < 8000)
   {
      echo("[OBSERVER] request cooldown");
      return;
   }
   $ObserverCfg::LastTry = %now;
   echo("[OBSERVER] requesting observer mode from server");
   ObserverCfg::force();
}

// Flipping the K row ON is also the "do it now" button.
function ObserverCfg::autoChanged()
{
   if(ObserverCfg::prefOn("pref::ModernHUD::observer::AutoObserver", "1") == "1")
   {
      $ObserverCfg::LastTry = "";
      schedule("ObserverCfg::maybeForce();", 0.25);
   }
}

//------------------------------------------------------------------------------
// Lifecycle.
//------------------------------------------------------------------------------
function ModernHUDPack::stockHuds()
{
   // The WHOLE stock set, all default-off; each gets an overridable K row.
   ModernHUD::stock(clockHud,       false);
   ModernHUD::stock(sensorHUD,      false);
   ModernHUD::stock(compassHud,     false);
   ModernHUD::stock(jetPackHud,     false);
   ModernHUD::stock(healthHud,      false);
   ModernHUD::stock(weaponHud,      false);
   ModernHUD::stock(chatDisplayHud, false);
   ModernHUD::stock(Minimap,        false);
   Control::SetVisible(reticleCompass, false);
   ObserverCfg::applyNameplates();
   ObserverCfg::applyServerText();
}

function ModernHUDPack::init()
{
}

function ModernHUDPack::draw(%screen)
{
   // Nothing. A filming config's whole job is an empty screen; the K panel and
   // its rows are drawn by the framework.
}

function ModernHUDPack::onPlayGuiOpen()
{
   echo("[OBSERVER] playGui open");
   schedule("ModernHUDPack::stockHuds();", 0.1);
   // Give the join a beat to settle (control object + any server join menu
   // arrive first), then ask.
   schedule("ObserverCfg::maybeForce();", 1.5);
}

// ★Both firers, because this pack has a BEHAVIOUR on play-gui open, not just
// drawing.★ eventGuiOpen_PlayGui is raised by Presto's OpenAGui wrapper
// (events.cs:746) -- the path every shell-menu join takes -- but a join that
// sets the content control without that wrapper (measured: the automation
// seam's missionJoin) only raises the ENGINE event, eventGuiOpen with the
// control's real name (simGuiCanvas.cpp:949). A draw-only pack can shrug that
// off; the auto-observer request cannot. Value compares are case-SENSITIVE, so
// test both spellings; double-fire on normal joins is absorbed by maybeForce's
// cooldown latch and stockHuds being idempotent.
function ModernHUDPack::onGuiOpen(%gui)
{
   echo("[OBSERVER] eventGuiOpen arg=" @ %gui);
   if(%gui == "playGui" || %gui == "PlayGui")
      ModernHUDPack::onPlayGuiOpen();
}

// Re-seed the pack's shipped defaults (first launch froze them into
// ClientPrefs.cs -- Vector::defaults has the full why).
function ObserverCfg::defaults()
{
   $pref::ModernHUD::observer::AutoObserver = "1";
   $pref::ModernHUD::observer::Nameplates = "0";
   $pref::ModernHUD::observer::ServerText = "0";
   $pref::ModernHUD::observer::Stock::clockHud = "0";
   $pref::ModernHUD::observer::Stock::sensorHUD = "0";
   $pref::ModernHUD::observer::Stock::compassHud = "0";
   $pref::ModernHUD::observer::Stock::jetPackHud = "0";
   $pref::ModernHUD::observer::Stock::healthHud = "0";
   $pref::ModernHUD::observer::Stock::weaponHud = "0";
   $pref::ModernHUD::observer::Stock::chatDisplayHud = "0";
   $pref::ModernHUD::observer::Stock::Minimap = "0";
   ModernHUDPack::stockHuds();
   echo("Observer: settings reset to pack defaults.");
}

// Extra work RESET DEFAULTS cannot know about (the session global).
function ModernHUDPack::menuReset()
{
   ObserverCfg::applyServerText();
   ObserverCfg::applyNameplates();
}

$ModernHUD::MenuTitle = "OBSERVER";

ModernHUD::setting("bool", "pref::ModernHUD::observer::AutoObserver",
   "Auto observer", "1", "", "ObserverCfg::autoChanged();");
ModernHUD::setting("bool", "pref::ModernHUD::observer::Nameplates",
   "Nameplates + reticle", "0", "", "ObserverCfg::applyNameplates();");
ModernHUD::setting("bool", "pref::ModernHUD::observer::ServerText",
   "Server text", "0", "", "ObserverCfg::applyServerText();");

ModernHUD::attach("eventGuiOpen_PlayGui", "ModernHUDPack::onPlayGuiOpen");
ModernHUD::attach("eventGuiOpen", "ModernHUDPack::onGuiOpen");
ModernHUDPack::prefs();
ModernHUDPack::stockHuds();
ModernHUDPack::init();
echo("[OBSERVER] pack ready, auto request " @ ObserverCfg::prefOn("pref::ModernHUD::observer::AutoObserver", "1"));
$ModernHUD::LoadComplete = "observer";
