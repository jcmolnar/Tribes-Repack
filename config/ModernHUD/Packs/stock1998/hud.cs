// pack: stock1998  (Stock (1998))    -- HAND-AUTHORED ("authoring": "manual")
//
// The game as it shipped: every 1998 retained HUD control on, nothing drawn over
// them, nothing of the 1.40 config era left standing. This pack DRAWS NOTHING --
// its whole job is what it turns on, what it takes down, and one engine request.
//
//   - every stock control visible (health, jetpack, weapon list, compass, clock,
//     sensor ping, chat, reticle compass, crosshair); each keeps a K-panel row so
//     a player can still switch one off. The Minimap is ours, not 1998's, so it
//     defaults OFF and has a row.
//   - the 1.40 community containers (GHealth, CTFHUD, ItemHUD, Toasty...) are
//     DETACHED, not hidden, exactly as Basic does -- they are what every config
//     since 2000 stacked on top of the stock bars.
//   - $Hud::StockChrome = 1 asks HealthHud.cpp / FearGuiJetHud.cpp for the original
//     icon-and-chrome bars. Every ModernHUD pack sets Config::Name, and those two
//     controls answer a set Config::Name with the config-era clean bar; without the
//     request a "stock" pack could never show the 1998 art. Session global,
//     cleared by the engine on pack unload (modernHudPacks.cpp MHPacks_unload).
//   - the Presto/Kronos suite loads on every boot regardless of pack (main.cpp);
//     its $KH::hideStock* switches are session globals it reads before hiding the
//     stock bars on a Kronos server, so they are turned off here. Its own bars can
//     still draw on an RPG server -- that is the suite's business, not this pack's.
//
// What stock did NOT have, and this pack therefore does not show: flag status,
// grenade/beacon counts, kill feed, a minimap. Stock (2026) is the pack that
// redraws this look as movable, scalable parts.

exec("ModernHUD/Framework.cs");

$ModernHUD::Enabled = true;
$ModernHUD::Pack = "Stock (1998)";
$ModernHUD::PackId = "stock1998";

// This pack draws nothing, so it owns no slot.
function ModernHUDPack::ownsSlot(%value)
{
   return false;
}

// The 1.40 config-era retained containers. Union of what Basic, ProConfig,
// v0dkA and xLoader detach -- detachContainer is null-safe, so naming one that
// this install never created costs nothing.
function ModernHUDPack::detachRetained()
{
   ModernHUD::detachContainer("AmmoHUD::Container");
   ModernHUD::detachContainer("ChatOverlay::Container");
   ModernHUD::detachContainer("clock::Container");
   ModernHUD::detachContainer("CTFHUD::Container");
   ModernHUD::detachContainer("CtfHUD::Container");
   ModernHUD::detachContainer("GAmmo::Container");
   ModernHUD::detachContainer("GEnergy::Container");
   ModernHUD::detachContainer("GHealth::Container");
   ModernHUD::detachContainer("GSpeed::Container");
   ModernHUD::detachContainer("ItemHUD::Container");
   ModernHUD::detachContainer("killHUD::Container");
   ModernHUD::detachContainer("LegendzFPSHUD::Container");
   ModernHUD::detachContainer("RadarOverlay::Container");
   ModernHUD::detachContainer("RadarRing::Container");
   ModernHUD::detachContainer("ToastyHUD::Container");
}

function ModernHUDPack::prefs()
{
   // No forced look. The stock controls sit where play.gui authored them.
}

//------------------------------------------------------------------------------
// Lifecycle.
//------------------------------------------------------------------------------
function ModernHUDPack::stockHuds()
{
   // The WHOLE stock set, all default-on; each gets an overridable K row.
   ModernHUD::stock(clockHud,       true);
   ModernHUD::stock(sensorHUD,      true);
   ModernHUD::stock(compassHud,     true);
   ModernHUD::stock(jetPackHud,     true);
   ModernHUD::stock(healthHud,      true);
   ModernHUD::stock(weaponHud,      true);
   ModernHUD::stock(chatDisplayHud, true);
   ModernHUD::stock(Minimap,        false);
   // Not in the framework's row set (it is reticle plumbing, not a panel), but it
   // IS 1998: the little heading strip under the reticle. Unguarded like the
   // framework's own stock() calls -- isObject() cannot see play.gui controls.
   Control::SetVisible(reticleCompass, true);
   // crosshairHud is force-shown by ModernHUD::stock for every gameplay pack.

   // Presto/Kronos: keep its hands off the stock bars this session.
   $KH::hideStockHealth = false;
   $KH::hideStockEnergy = false;
   $KH::hideStockWeapon = false;
   if(isFunction("kronos::showStockHuds"))
      kronos::showStockHuds();

   S98::layout();
}

//------------------------------------------------------------------------------
// The 1998 layout.
//------------------------------------------------------------------------------
// The retained controls are created by config\play.gui, which ships JOE'S layout
// (it is his play tree's file), and every config since has dragged them about --
// measured: the clock and the sensor light mid-screen. So this pack places them
// where the original PlayGui authored them, as fractions of the live canvas from
// the 1920x1080 conversion (health 50,7 / jet 50,28 / clock 0,1062 / sensor 1504,0
// / compass 1856,376 / weapons 0,761 / reticle strip 1736,241). Edges that were
// pinned stay pixel-pinned (left, top, bottom, right); the two mid-screen ones
// keep their vertical fraction.
//
// A K row switches this off, for a player who would rather drag them (the
// framework's stock rows make every one of these an editor target).
// Hud::setSessionPos, not Control::setPosition: every stock hud in this build derives
// from HudCtrl (HealthHud.cpp:17, clockhud.cpp:24...), and a HudCtrl re-anchors itself
// from its fractional position on the next render -- measured: a plain setPosition
// left the clock exactly where it was. The session setter is the one that also
// clears that re-anchor.
// By NAME, not by id: the play.gui controls are found through the canvas tree
// (SimGui::findControl), which is the setter's own fallback, while isObject() and
// Control::getId go through the manager's name table and answer False / 0 for them
// (measured: isObject(playGui) true, isObject(healthHud) false).
function S98::place(%ctrl, %x, %y)
{
   %ok = Hud::setSessionPos(%ctrl, floor(%x), floor(%y));
   if($pref::ModernHUD::stock1998::Diag != "")
      echo("[STOCK1998] place " @ %ctrl @ " -> " @ floor(%x) @ "," @ floor(%y)
           @ " ok=" @ %ok @ " now@" @ Control::getPosition(%ctrl));
}

function S98::layout()
{
   %on = $pref::ModernHUD::stock1998::Layout;
   %ext = Control::GetExtent(playGui);
   %w = getWord(%ext, 0);
   %h = getWord(%ext, 1);
   if($pref::ModernHUD::stock1998::Diag != "")
      echo("[STOCK1998] layout on=" @ %on @ " isObj=" @ isObject(playGui) @ " ext=" @ %ext);
   if(%on == "0" || %on == "false")
      return;
   if(%w <= 0 || %h <= 0)
      return;
   S98::place(healthHud,      50,        7);
   S98::place(jetPackHud,     50,        28);
   S98::place(clockHud,       0,         %h - 18);
   S98::place(sensorHUD,      %w - 416,  0);
   S98::place(compassHud,     %w - 64,   %h * 0.348);
   S98::place(weaponHud,      0,         %h * 0.705);
   S98::place(reticleCompass, %w - 184,  %h * 0.223);
}

function ModernHUDPack::init()
{
   $Hud::StockChrome = "1";
   // The stock reticle is drawn by crosshairHud only while this GLOBAL pref is off.
   // Packs that draw their own reticle (Ascend, Vector) leave it at 1 behind them,
   // and "stock with no crosshair" is not stock. The common K row still offers it.
   $pref::hideCrosshairArt = "0";
}

function ModernHUDPack::draw(%screen)
{
   // Nothing. The stock controls render themselves; the K panel and its rows are
   // drawn by the framework.
}

function ModernHUDPack::onPlayGuiOpen()
{
   // The stock objects exist once PlayGui is up; the Kronos suite re-asserts its
   // hiding from the same event, so re-assert ours a beat later.
   schedule("ModernHUDPack::stockHuds();", 0.1);
   schedule("S98::diag();", 0.5);
}

// One line per play-gui open with the state this pack depends on, so a missing
// reticle or a stray layout can be read off console.log instead of guessed at.
function S98::diag()
{
   echo("[STOCK1998] xhairArt=" @ $pref::hideCrosshairArt
        @ " xhairVis=" @ Control::getVisible(crosshairHud)
        @ " chrome=" @ $Hud::StockChrome
        @ " layout=" @ $pref::ModernHUD::stock1998::Layout
        @ " health@" @ Control::getPosition(healthHud)
        @ " clock@" @ Control::getPosition(clockHud)
        @ " xhair@" @ Control::getPosition(crosshairHud) @ "/" @ Control::GetExtent(crosshairHud)
        @ " playGui=" @ Control::GetExtent(playGui));
   if($pref::ModernHUD::stock1998::Diag != "")
   {
      HudEditor::dumpTree();
      S98::layout();
      echo("[STOCK1998] after layout: clock@" @ Control::getPosition(clockHud)
           @ " health@" @ Control::getPosition(healthHud));
   }
}

// Both firers (observer/hud.cs has the story): a shell-menu join raises Presto's
// eventGuiOpen_PlayGui; a join that sets the content control directly raises only
// the engine's eventGuiOpen with the control's real name. This pack has a placement
// BEHAVIOUR on play-gui open, so it must hear both.
function ModernHUDPack::onGuiOpen(%gui)
{
   if(%gui == "playGui" || %gui == "PlayGui")
      ModernHUDPack::onPlayGuiOpen();
}

function ModernHUDPack::menuReset()
{
   ModernHUDPack::stockHuds();
}

$ModernHUD::MenuTitle = "STOCK (1998)";

ModernHUD::setting("bool", "pref::ModernHUD::stock1998::Layout",
   "Original 1998 layout", "1", "", "ModernHUDPack::stockHuds();");

ModernHUD::attach("eventGuiOpen_PlayGui", "ModernHUDPack::onPlayGuiOpen");
ModernHUD::attach("eventGuiOpen", "ModernHUDPack::onGuiOpen");
ModernHUDPack::prefs();
ModernHUDPack::stockHuds();
ModernHUDPack::init();
echo("[STOCK1998] pack ready");
$ModernHUD::LoadComplete = "stock1998";
