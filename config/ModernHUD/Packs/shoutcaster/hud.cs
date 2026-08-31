// pack: shoutcaster  (Shoutcaster)    -- HAND-AUTHORED ("authoring": "manual")
//
// The broadcast config. Sibling of "observer" (filming), but where that one wants an
// EMPTY screen, a caster needs the opposite of empty: match clock, comms, and the
// telestrator on top of whatever camera they are cutting to.
//
// ★WHY THIS PACK EXISTS AT ALL -- it is a load-order fix, not just a look.★
// Telestrator drawing used to hang off ScriptGL::playGui::onPostDraw, whose only
// owner is config\Presto\KronosShop.cs -- and autoexec.cs execs that file INSIDE
// `if($Config::Name == "")` (:128-165). So on any active 1.4 custom config the
// telestrator silently did not render: the keys worked, the strokes were stored, and
// nothing appeared. ModernHUD packs are loaded NATIVELY (modernHudPacks.cpp
// MHPacks_scan scans config\ModernHUD\Packs\* from C++), and the framework's draw is
// dispatched by fixed name from ScriptGL_renderHook -> ModernHUD::onDraw ->
// ModernHUDPack::draw. Neither path passes through autoexec.cs, so a pack draw runs
// under EVERY config. That is the whole point of putting the telestrator here.
//
// The trade, accepted deliberately (Joe, 2026-08-29): drawing now depends on
// ModernHUD being enabled instead of on the config name. "A caster won't run without
// the Shoutcaster HUD enabled, realistically -- if they do they can deal with the
// consequences."
//
// The caster FUNCTIONS themselves are always present regardless of pack or config --
// nativeDefaults.cs is exec'd at autoexec.cs:14, outside both config gates. This pack
// supplies the draw window, not the feature.
//
// Controls (identical whether the key is a numpad key or the top row held under the
// assignable "Caster Modifier (hold)" bind in Options -> Controls -> View):
//   1 / 2   follow team 0 / 1 flag, retargeting to the carrier
//   4 / 5   team 0 / 1 flag-stand camera
//   + / -   orbit distance +-5m (clamped 3-30)
//   0       detach          *   help
//   7       telestrator: off -> draw (LMB draws) -> pinned -> off

exec("ModernHUD/Framework.cs");

$ModernHUD::Enabled = true;
$ModernHUD::Pack = "Shoutcaster";
$ModernHUD::PackId = "shoutcaster";

// Draws only the telestrator overlay, which is not a canonical HUD slot.
function ModernHUDPack::ownsSlot(%value)
{
   return false;
}

function ModernHUDPack::detachRetained()
{
   // Nothing replaced -- stock huds are hidden or kept, never superseded.
   // Defined because the framework calls it unconditionally.
}

function ModernHUDPack::prefs()
{
   // No forced look. Everything player-facing is a setting row.
}

//------------------------------------------------------------------------------
// Setting applies.
//------------------------------------------------------------------------------
// Framework idiom (see Framework.cs ModernHUD::stock): an unset pref means "pack
// default", and comparisons stay on quoted strings so compare() never float-promotes.
function CasterCfg::prefOn(%name, %dflt)
{
   %v = getVariable(%name);
   if(%v == "") { return %dflt; }
   if(%v == "0" || %v == "false" || %v == "False") { return "0"; }
   return "1";
}

// Live-play telestrator is opt-in on purpose: an unconditional numpad7 grab would eat
// the key from RPG mods that bind it. It is ALWAYS live during demo playback, where
// nothing else wants the key -- that gate is in nativeDefaults.cs sendControl.
function CasterCfg::applyDraw()
{
   $pref::casterDraw = CasterCfg::prefOn("pref::ModernHUD::shoutcaster::Draw", "1");
}

// Records every match plus the .events.cs moment sidecar (MA / CK / MA-CK / GRAB /
// CAP / RETURN). Read ONCE at connect (netCSDelegate.cpp) -- changing it mid-match
// takes effect from the NEXT connect, which is why the row says so.
function CasterCfg::applyRecord()
{
   $pref::casterAutoRecord = CasterCfg::prefOn("pref::ModernHUD::shoutcaster::AutoRecord", "0");
}

function CasterCfg::applyNameplates()
{
   // crosshairHud drives the reticle AND the whole nameplate layer
   // (fearGuiCrosshair.cpp). A caster usually WANTS nameplates -- that is how you
   // name the player you are talking about -- so this defaults ON, the opposite of
   // the filming pack. Bypasses ModernHUD::stock, which correctly refuses to hide
   // crosshairHud for gameplay packs.
   if(CasterCfg::prefOn("pref::ModernHUD::shoutcaster::Nameplates", "1") == "1")
      Control::SetVisible(crosshairHud, true);
   else
      Control::SetVisible(crosshairHud, false);
}

//------------------------------------------------------------------------------
// Force observer -- every caster command is gated on team -1 server-side.
//------------------------------------------------------------------------------
// The stock menu walk, sent directly: remoteScoresOn -> Game::menuRequest builds
// "Options"; "changeteams" -> processMenuOptions builds "Pick a team:"; "-2" ->
// processMenuPickTeam -> Observer::enterObserverMode. Every step degrades to a no-op
// on servers that lack it. Proven in the observer pack; kept in step with it.
function CasterCfg::force()
{
   if(isObject(CurServerMenu))
      remoteEval(2048, "MenuSelect", "-2");
   remoteEval(2048, "ScoresOn");
   remoteEval(2048, "MenuSelect", "changeteams");
   remoteEval(2048, "MenuSelect", "-2");
   $CasterCfg::SettleTries = 8;
   schedule("CasterCfg::settle();", 1);
}

// The walk's own "Pick a team:" menu lands on our screen as a server push we never
// click, and it can arrive after any fixed cleanup delay. Resolve it the way a real
// click does -- clientMenuSelect deletes the menu AND sends the selection, and
// enterObserverMode is idempotent server-side. Gated on isObserving() and bounded, so
// a foreign server menu is never eaten.
function CasterCfg::settle()
{
   if($CasterCfg::SettleTries == "" || $CasterCfg::SettleTries <= 0)
      return;
   $CasterCfg::SettleTries = $CasterCfg::SettleTries - 1;
   if(isObserving() && isObject(CurServerMenu))
   {
      echo("[CASTER] resolving leftover pick-team menu");
      clientMenuSelect("-2");
   }
   schedule("CasterCfg::settle();", 1);
}

function CasterCfg::maybeForce()
{
   if(CasterCfg::prefOn("pref::ModernHUD::shoutcaster::AutoObserver", "1") == "0")
   {
      echo("[CASTER] auto observer off");
      return;
   }
   if(isObserving())
   {
      echo("[CASTER] already observing");
      return;
   }
   if(isObject(CurServerMenu))
   {
      echo("[CASTER] server menu open -- not requesting");
      return;
   }
   %now = getRealMillis();
   if($CasterCfg::LastTry != "" && (%now - $CasterCfg::LastTry) < 8000)
   {
      echo("[CASTER] request cooldown");
      return;
   }
   $CasterCfg::LastTry = %now;
   echo("[CASTER] requesting observer mode from server");
   CasterCfg::force();
}

function CasterCfg::autoChanged()
{
   if(CasterCfg::prefOn("pref::ModernHUD::shoutcaster::AutoObserver", "1") == "1")
   {
      $CasterCfg::LastTry = "";
      schedule("CasterCfg::maybeForce();", 0.25);
   }
}

//------------------------------------------------------------------------------
// Lifecycle.
//------------------------------------------------------------------------------
function ModernHUDPack::stockHuds()
{
   // A caster is not playing: their own health, jets and weapon are meaningless.
   // The match clock and the chat are the two things they genuinely read, so those
   // stay on. Every one still gets an overridable K row.
   ModernHUD::stock(healthHud,      false);
   ModernHUD::stock(jetPackHud,     false);
   ModernHUD::stock(weaponHud,      false);
   ModernHUD::stock(sensorHUD,      false);
   ModernHUD::stock(compassHud,     false);
   ModernHUD::stock(Minimap,        false);
   ModernHUD::stock(clockHud,       true);
   ModernHUD::stock(chatDisplayHud, true);
   Control::SetVisible(reticleCompass, false);
   CasterCfg::applyNameplates();
}

function ModernHUDPack::init()
{
   CasterCfg::applyDraw();
   CasterCfg::applyRecord();
}

// ★THE LOAD-ORDER FIX.★ %screen is "w h" in surface pixels, the same shape
// Telestrator::paint expects. Telestrator::render (the KronosShop onPostDraw entry)
// stands down while this pack is loaded, so the strokes are painted exactly once even
// on a default config where BOTH callers exist -- see nativeDefaults.cs.
function ModernHUDPack::draw(%screen)
{
   if(isFunction("Telestrator::paint"))
      Telestrator::paint(%screen);
}

function ModernHUDPack::onPlayGuiOpen()
{
   echo("[CASTER] playGui open");
   schedule("ModernHUDPack::stockHuds();", 0.1);
   schedule("CasterCfg::maybeForce();", 1.5);
}

// ★Both firers, because this pack has BEHAVIOUR on play-gui open, not just drawing.★
// eventGuiOpen_PlayGui is raised by Presto's OpenAGui wrapper -- the path every
// shell-menu join takes -- but a join that sets the content control without that
// wrapper (the automation seam's missionJoin) raises only the ENGINE event,
// eventGuiOpen with the control's real name. Value compares are case-SENSITIVE, so
// test both spellings; double-fire is absorbed by the cooldown latch and by
// stockHuds being idempotent.
function ModernHUDPack::onGuiOpen(%gui)
{
   if(%gui == "playGui" || %gui == "PlayGui")
      ModernHUDPack::onPlayGuiOpen();
}

// Re-seed the pack's shipped defaults (first launch freezes them into ClientPrefs.cs).
function CasterCfg::defaults()
{
   $pref::ModernHUD::shoutcaster::AutoObserver = "1";
   $pref::ModernHUD::shoutcaster::Draw = "1";
   $pref::ModernHUD::shoutcaster::AutoRecord = "0";
   $pref::ModernHUD::shoutcaster::Nameplates = "1";
   $pref::ModernHUD::shoutcaster::Stock::healthHud = "0";
   $pref::ModernHUD::shoutcaster::Stock::jetPackHud = "0";
   $pref::ModernHUD::shoutcaster::Stock::weaponHud = "0";
   $pref::ModernHUD::shoutcaster::Stock::sensorHUD = "0";
   $pref::ModernHUD::shoutcaster::Stock::compassHud = "0";
   $pref::ModernHUD::shoutcaster::Stock::Minimap = "0";
   $pref::ModernHUD::shoutcaster::Stock::clockHud = "1";
   $pref::ModernHUD::shoutcaster::Stock::chatDisplayHud = "1";
   ModernHUDPack::stockHuds();
   CasterCfg::applyDraw();
   CasterCfg::applyRecord();
   echo("Shoutcaster: settings reset to pack defaults.");
}

// Extra work RESET DEFAULTS cannot know about.
function ModernHUDPack::menuReset()
{
   CasterCfg::applyNameplates();
   CasterCfg::applyDraw();
   CasterCfg::applyRecord();
}

$ModernHUD::MenuTitle = "SHOUTCASTER";

ModernHUD::setting("bool", "pref::ModernHUD::shoutcaster::AutoObserver",
   "Auto observer", "1", "", "CasterCfg::autoChanged();");
ModernHUD::setting("bool", "pref::ModernHUD::shoutcaster::Draw",
   "Telestrator in live play", "1", "", "CasterCfg::applyDraw();");
ModernHUD::setting("bool", "pref::ModernHUD::shoutcaster::AutoRecord",
   "Auto-record matches (next connect)", "0", "", "CasterCfg::applyRecord();");
ModernHUD::setting("bool", "pref::ModernHUD::shoutcaster::Nameplates",
   "Nameplates + reticle", "1", "", "CasterCfg::applyNameplates();");

ModernHUD::attach("eventGuiOpen_PlayGui", "ModernHUDPack::onPlayGuiOpen");
ModernHUD::attach("eventGuiOpen", "ModernHUDPack::onGuiOpen");
ModernHUDPack::prefs();
ModernHUDPack::stockHuds();
ModernHUDPack::init();
echo("[CASTER] shoutcaster pack ready, telestrator draw " @ CasterCfg::prefOn("pref::ModernHUD::shoutcaster::Draw", "1"));
$ModernHUD::LoadComplete = "shoutcaster";
