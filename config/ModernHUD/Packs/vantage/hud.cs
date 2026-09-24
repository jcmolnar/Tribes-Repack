//==============================================================================
// VANTAGE -- hand-authored ModernHUD pack.       (manifest: pack.json)
//
// "authoring": "manual", so tools/modernhud_pack.py --generate REFUSES to
// overwrite this file. It is not converted from a legacy pack; it is written
// against this client's capabilities. Design and rationale: re/vantage_hud_design.md.
//
// THE TWO THINGS THAT MAKE THIS DIFFERENT FROM EVERY CONVERTED PACK
//
// 1. ★No art. At all.★ Every pixel is glRectangle + glColor4ub + stock .pft
//    fonts. The five legacy packs each ship their own PNG set, and a pack is
//    only correct in a tree that has its art -- measured, only ONE art file
//    appears in 3+ of the five. Vantage has no missing-asset failure mode
//    because it has no assets.
//
// 2. ★It configures the WORLD layer, not just the overlay.★ Nameplates, health
//    and jet bars over players, and the flag-pass helper are client features
//    ($mj::*) that no 1.40 plugin pack could reach -- hrfixfix.dll had to byte
//    -patch the client to fake them. Same for the engine-wide colour theme,
//    which is what bluelines.dll existed to do. Installing Vantage sets both.
//
// REVERSIBILITY: everything Vantage writes outside its own namespace is saved
// first and restored by Vantage::restore(). A pack that permanently rewrites
// client-wide globals is a pack you cannot uninstall.
//==============================================================================

exec("ModernHUD/Framework.cs");
exec("ModernHUD/Packs/vantage/components.cs");   // borrowable parts (HUD designer mix-and-match)

$ModernHUD::Enabled = true;
$ModernHUD::Pack = "Vantage";
$ModernHUD::PackId = "vantage";

//------------------------------------------------------------------------------
// Slot ownership. A part is ours unless the player picked another pack's module
// for its slot. ModernHUD is immediate-mode, so hiding a retained control cannot
// switch one of our parts off -- the draw dispatch has to yield the slot.
//------------------------------------------------------------------------------
function ModernHUDPack::ownsSlot(%value)
{
   if(%value == "")
      return true;
   if(%value == "off")
      return false;
   return String::findSubStr(%value, "Vantage::") == 0;
}

//------------------------------------------------------------------------------
// The client-wide settings Vantage seeds -- and how to get them back.
//
// ★Saved before written, every one.★ These are not the pack's own state: they
// are the player's client. A pack that cannot be uninstalled cleanly is a pack
// that should not be installed.
//------------------------------------------------------------------------------
// ★Written out longhand on purpose.★ The obvious shape here is a
// Vantage::set(%name, %value) helper that assigns through the name -- and the
// console has no such thing. `*expr(args)` (DynCallExprNode, eval.cpp:1021) is
// a dynamic CALL and is the ONLY indirection the grammar has; there is no
// dynamic variable assignment node, so `*%var = %value` does not parse as an
// assignment. Two long explicit lists are correct; a clever one is silently
// broken, and "silently" is the problem -- an unparsed assignment leaves the
// player's setting overwritten with no way back.
function Vantage::apply()
{
   Vantage::palette();

   if($Vantage::Saved == "")
   {
      $Vantage::Saved = 1;

      $Vantage::Sav::ColorPrimary  = $pref::Hud::ColorPrimary;
      $Vantage::Sav::ColorDim      = $pref::Hud::ColorDim;
      $Vantage::Sav::ColorAccent   = $pref::Hud::ColorAccent;
      $Vantage::Sav::ColorWarn     = $pref::Hud::ColorWarn;
      $Vantage::Sav::ColorText     = $pref::Hud::ColorText;
      $Vantage::Sav::ColorPass     = $pref::Hud::ColorPass;

      $Vantage::Sav::ShowNames     = $mj::shownames;
      $Vantage::Sav::ShowHpBars    = $mj::showhpbars;
      $Vantage::Sav::ShowJetBars   = $mj::showjetbars;
      $Vantage::Sav::ShowHpText    = $mj::showhptext;
      $Vantage::Sav::BarsCrouch    = $mj::barscrouch;
      $Vantage::Sav::BarW          = $mj::bar_width;
      $Vantage::Sav::BarH          = $mj::bar_height;
      $Vantage::Sav::BarB          = $mj::bar_border_width;
      $Vantage::Sav::FontDefault   = $mj::fontdefault;
      $Vantage::Sav::FontPass      = $mj::fontpass;
      $Vantage::Sav::PassHelper    = $mj::passhelper;
      $Vantage::Sav::PassHelperMM  = $mj::passhelpermm;

      $Vantage::Sav::HiderEnabled  = $xChat::HiderEnabled;
      $Vantage::Sav::HiderTimeout  = $xChat::HiderTimeout;
      $Vantage::Sav::ScrollTimeout = $xChat::ScrollTimeout;
      $Vantage::Sav::HideCmdMsg    = $xChat::HideCmdMsg;
      $Vantage::Sav::TransChat     = $xChat::TransChat;

      $Vantage::Sav::ChatModX      = $pref::ChatDisplayModMethodX;
      $Vantage::Sav::ChatX         = $pref::ChatDisplayX;
      $Vantage::Sav::ChatWidth     = $pref::ChatDisplayWidth;
   }

   // -- the engine-wide colour theme (what bluelines.dll byte-patched) --------
   $pref::Hud::ColorPrimary = $Vantage::Primary;
   $pref::Hud::ColorDim     = $Vantage::Dim;
   $pref::Hud::ColorAccent  = $Vantage::Accent;
   $pref::Hud::ColorWarn    = $Vantage::Warn;
   $pref::Hud::ColorText    = $Vantage::Text;
   $pref::Hud::ColorPass    = $Vantage::Pass;

   // -- the world layer (what hrfixfix.dll byte-patched) ---------------------
   // Bars are NOT crouch-gated: $mj::barscrouch means "only show when
   // crouching" (the shipped module's own comment), and a bar you only get
   // when the enemy crouches is a bar you never get.
   $mj::shownames        = "True";
   $mj::showhpbars       = "True";
   $mj::showjetbars      = "True";
   $mj::showhptext       = "False";        // the bar is the message
   $mj::barscrouch       = "False";
   $mj::bar_width        = "20";           // the packs' own geometry
   $mj::bar_height       = "5";
   $mj::bar_border_width = "1";
   $mj::fontdefault      = "sf_white_7.pft";
   $mj::fontpass         = "if_g_10b.pft";
   $mj::passhelper       = "True";
   $mj::passhelpermm     = "True";

   // -- chat that gets out of the way ---------------------------------------
   $xChat::HiderEnabled  = "True";
   $xChat::HiderTimeout  = "12";
   $xChat::ScrollTimeout = "5";
   $xChat::HideCmdMsg    = "True";
   $xChat::TransChat     = "True";

   // -- chat log placement, matching the layout ------------------------------
   $pref::ChatDisplayModMethodX = "1";
   $pref::ChatDisplayX          = "18";
   $pref::ChatDisplayWidth      = "440";
}

// Called by ModernHUD::unload (Framework.cs) when this pack is swapped away -- see the
// same hook in ascend/hud.cs for why.
function ModernHUDPack::restore()
{
   if($Vantage::Saved != "")
      Vantage::restore();
}

//------------------------------------------------------------------------------
// Pack lifecycle.
//------------------------------------------------------------------------------
function ModernHUDPack::prefs()
{
   $pref::miniMapSquare  = "True";
   $pref::miniMapWidth   = "180";
   $pref::miniMapZoom    = "6";
   $pref::miniMapRotate  = "False";
   $pref::miniMapCompass = "True";
}

function ModernHUDPack::stockHuds()
{
   // ★The WHOLE set, not just the ones we want on.★ Stock visibility is global
   // client state; a pack that lists only its own leaves the rest wherever the
   // previous pack put them -- a measured defect, not a hypothetical.
   // ModernHUD::stock is Control::SetVisible with a player row in front of it:
   // the value here is this pack's DEFAULT, and the K panel's row overrides it.
   Control::SetVisible(crosshairHud,   true);
   ModernHUD::stock(chatDisplayHud, true);
   ModernHUD::stock(Minimap,        true);
   // Off: Vantage draws its own clock part now (getHudTimer, the same client clock
   // the stock ClockHud reads). The K panel's Clock row still brings the stock one back.
   ModernHUD::stock(clockHud,       false);
   ModernHUD::stock(healthHud,      false);
   ModernHUD::stock(jetPackHud,     false);
   ModernHUD::stock(weaponHud,      false);
   ModernHUD::stock(compassHud,     false);
   ModernHUD::stock(sensorHUD,      false);
}

function ModernHUDPack::detachRetained()
{
   // Vantage replaces no legacy container -- it has no legacy ancestor. Left
   // defined because the framework calls it unconditionally.
}

function ModernHUDPack::init()
{
   Vantage::apply();
}

ModernHUD::require("ModernHUD/Core/Data/Team.cs");
ModernHUD::require("ModernHUD/Core/Data/Timer.cs");

function ModernHUDPack::draw(%screen)
{
   if(ModernHUDPack::ownsSlot($pref::HudSlot::healthenergy))
      Vantage::draw_vitals(%screen);
   else
      ModernHUD::hide("ModernHUD::VantageVitals");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::weapon))
      Vantage::draw_weapon(%screen);
   else
      ModernHUD::hide("ModernHUD::VantageWeapon");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::ctf))
      Vantage::draw_ctf(%screen);
   else
      ModernHUD::hide("ModernHUD::VantageCtf");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::items))
      Vantage::draw_items(%screen);
   else
      ModernHUD::hide("ModernHUD::VantageItems");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::clock))
      Vantage::draw_clock(%screen);
   else
      ModernHUD::hide("ModernHUD::VantageClock");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::killfeed))
      Vantage::draw_killfeed(%screen);
   else
      ModernHUD::hide("ModernHUD::VantageKillFeed");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::scoreboard))
      Vantage::draw_scoreboard(%screen);
   else
      ModernHUD::hide("ModernHUD::VantageScoreboard");
}

// ★Bound to eventGuiOpen_PlayGui, NOT eventGuiOpen plus a gui-name test.★
// TWO independent firers raise eventGuiOpen with DIFFERENT spellings: the
// ENGINE, with the control's real name -- `playGui` (simGuiCanvas.cpp:907,
// kronosFireEvent1) -- and PRESTO, with a hardcoded bare word -- `PlayGui`
// (Presto/events.cs:681 via OpenAGui(PlayGui) at :737). A string VALUE
// comparison is case-SENSITIVE even though NAME lookup is not (compare() falls
// through to strcmp for two non-numeric strings, eval.cpp), so the old
// `%gui == "playGui"` test matched the engine's spelling and ignored Presto's.
// It WORKED -- verified live: oldSeen=playGui, oldHits=1 per transition -- but
// only because the two spellings happen to differ. Presto fires an
// argument-free eventGuiOpen_PlayGui from the same function (events.cs:746);
// binding that removes the string test altogether, at the same frequency
// (verified newHits=1). A robustness change, not a bug fix.
function ModernHUDPack::onPlayGuiOpen()
{
   Schedule::Add("ModernHUDPack::stockHuds();", 0);
}

// ModernHUD::attach, not a raw Event::Attach: the framework revokes tracked
// handlers in detachAll() on unload, so this cannot outlive its own pack.
ModernHUD::attach("eventGuiOpen_PlayGui", "ModernHUDPack::onPlayGuiOpen");
ModernHUDPack::prefs();
ModernHUDPack::stockHuds();
ModernHUDPack::init();

// Font-scope Stage 3: load-completion sentinel -- MUST stay the final statement.
$ModernHUD::LoadComplete = "vantage";
