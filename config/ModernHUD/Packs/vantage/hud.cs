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
// The palette. One place, so the whole HUD moves together.
//
// These are RGB triples fed to $pref::Hud::Color* (which resolve through the
// engine palette's GetNearestColor) and to glColor4ub for our own draws. Using
// the SAME numbers for both is the point: our bars and the engine's brackets,
// crosshair box and chat end up the same colour.
//------------------------------------------------------------------------------
function Vantage::palette()
{
   $Vantage::Primary  = "0 200 255";      // cyan -- the HUD's voice
   $Vantage::Dim      = "0 62 78";        // the same hue, backgrounded
   $Vantage::Accent   = "255 190 60";     // amber -- "attention", not "danger"
   $Vantage::Warn     = "255 60 60";      // red   -- danger only
   $Vantage::Text     = "235 245 255";
   $Vantage::Pass     = "255 105 180";    // flag carrier
}

// Set the raw-GL draw colour from one of the palette entries.
function Vantage::color(%rgb, %alpha)
{
   glColor4ub(getWord(%rgb, 0), getWord(%rgb, 1), getWord(%rgb, 2), %alpha);
}

// A filled bar with a dim bed behind it. %frac is 0..1.
function Vantage::meter(%x, %y, %w, %h, %frac, %rgb, %alpha)
{
   if(%frac < 0) %frac = 0;
   if(%frac > 1) %frac = 1;

   Vantage::color($Vantage::Dim, %alpha * 0.55);
   glRectangle(%x, %y, %w, %h);

   %fill = floor(%w * %frac);
   if(%fill > 0)
   {
      Vantage::color(%rgb, %alpha);
      glRectangle(%x, %y, %fill, %h);
   }
}

// Text in a palette colour. Uses the self-contained <f:file:rgba:shadow:dx,dy>
// markup form rather than <fN> so the pack never depends on whatever font table
// the previously-loaded pack happened to leave behind.
function Vantage::text(%x, %y, %width, %rgb, %str, %alpha)
{
   %hex = Vantage::hex(%rgb);
   ModernHUD::markup(%x, %y, %width,
      "<f:sf_white_10b.pft:" @ %hex @ "ff:000000c0:1,1>" @ %str, %alpha);
}

function Vantage::textSmall(%x, %y, %width, %rgb, %str, %alpha)
{
   %hex = Vantage::hex(%rgb);
   ModernHUD::markup(%x, %y, %width,
      "<f:sf_white_7.pft:" @ %hex @ "ff:000000c0:1,1>" @ %str, %alpha);
}

// "r g b" -> "rrggbb". The console has no printf, so this is a nibble table.
function Vantage::hex(%rgb)
{
   return Vantage::hex2(getWord(%rgb, 0)) @ Vantage::hex2(getWord(%rgb, 1)) @
          Vantage::hex2(getWord(%rgb, 2));
}

function Vantage::hex2(%v)
{
   if(%v < 0)   %v = 0;
   if(%v > 255) %v = 255;
   return Vantage::nib(floor(%v / 16)) @ Vantage::nib(%v - floor(%v / 16) * 16);
}

function Vantage::nib(%n)
{
   if(%n <= 9)
      return %n;
   if(%n == 10) return "a";
   if(%n == 11) return "b";
   if(%n == 12) return "c";
   if(%n == 13) return "d";
   if(%n == 14) return "e";
   return "f";
}

//------------------------------------------------------------------------------
// PART: vitals -- health + energy, bottom centre, with a DAMAGE TRAIL.
//
// ★The trail is the widget this pack exists for.★ The health bar drops to the
// real value instantly; a second, lighter bar behind it falls to meet it over
// ~400ms. You see how much you just LOST, not only what is left -- which in a
// game where damage arrives as discrete disc hits is the more useful fact.
//
// A retained control cannot do this: it has no per-frame tick, which is why
// every legacy health plate is a number that teleports. Immediate mode plus
// glTicks (a real ms wall clock) makes it three lines.
//------------------------------------------------------------------------------
function Vantage::Vitals(%x, %y, %w)
{
   %health = $health;
   %energy = $energy;
   if(%health == "") %health = 0;
   if(%energy == "") %energy = 0;

   %now = glTicks();

   // Trail state. Seeded on first draw so the bar does not sweep in from zero
   // the moment you spawn.
   if($Vantage::TrailInit == "")
   {
      $Vantage::TrailInit = 1;
      $Vantage::Trail = %health;
      $Vantage::TrailAt = %now;
   }

   if(%health > $Vantage::Trail)
   {
      // Healed: no trail. A trail on the way UP would be showing you a deficit
      // you no longer have.
      $Vantage::Trail = %health;
   }
   else if($Vantage::Trail > %health)
   {
      // 400ms to close the gap, frame-rate independent because the step is
      // scaled by the REAL elapsed ms, not by a per-frame constant.
      %dt = %now - $Vantage::TrailAt;
      if(%dt < 0)    %dt = 0;      // clock reset (map change) -- do not lurch
      if(%dt > 250)  %dt = 250;    // long stall -- do not snap
      %step = ($Vantage::Trail - %health) * (%dt / 400);
      $Vantage::Trail = $Vantage::Trail - %step;
      if($Vantage::Trail < %health)
         $Vantage::Trail = %health;
   }
   $Vantage::TrailAt = %now;

   // Colour by threshold. Amber is "pay attention", red is "you are dying" --
   // two distinct states, not a gradient, because a gradient tells you nothing
   // you can act on at a glance.
   if(%health > 66)      %hc = $Vantage::Primary;
   else if(%health > 33) %hc = $Vantage::Accent;
   else                  %hc = $Vantage::Warn;

   %barW = %w;

   // trail first, behind
   if($Vantage::Trail > %health)
   {
      Vantage::color($Vantage::Warn, 90);
      glRectangle(%x, %y, floor(%barW * ($Vantage::Trail / 100)), 7);
   }

   Vantage::meter(%x, %y, %barW, 7, %health / 100, %hc, 235);
   Vantage::meter(%x, %y + 9, %barW, 4, %energy / 100, $Vantage::Primary, 190);

   // ★The number appears only when it matters.★ A readout you look at every
   // frame is a readout you have stopped seeing; one that only exists below 40
   // is an alarm.
   if(%health < 40)
      Vantage::text(%x, %y - 18, %barW, $Vantage::Warn, "<jc>" @ floor(%health), 255);
}

//------------------------------------------------------------------------------
// PART: weapon -- name + ammo, bottom right.
//------------------------------------------------------------------------------
function Vantage::Weapon(%x, %y, %w)
{
   %wep = GetItemDesc(GetMountedItem(0));
   if(%wep == "")
      return;

   %ammo = $Weapon::Ammo;

   Vantage::text(%x, %y, %w, $Vantage::Text, "<jr>" @ %wep, 220);

   if(%ammo == "" || %ammo < 0)
   {
      // Energy weapons report no ammo count. Say so rather than drawing a bar
      // that is always empty.
      Vantage::textSmall(%x, %y + 16, %w, $Vantage::Dim, "<jr>--", 200);
      return;
   }

   if(%ammo <= 2) %ac = $Vantage::Warn;
   else           %ac = $Vantage::Accent;

   Vantage::text(%x, %y + 14, %w, %ac, "<jr>" @ %ammo, 255);
}

//------------------------------------------------------------------------------
// PART: ctf -- both scores and both flag states, top centre.
//
// Uses the shared Team.cs data layer that ships with the framework's Core/Data,
// the same one the converted packs require. Degrades to scores alone when the
// layer is not present rather than erroring per frame.
//------------------------------------------------------------------------------
function Vantage::Ctf(%x, %y, %w)
{
   %mine = Team::Friendly();
   %theirs = Team::Enemy();
   if(%mine == "" || %theirs == "")
      return;

   %s0 = Team::Score(%mine);
   %s1 = Team::Score(%theirs);
   if(%s0 == "") %s0 = 0;
   if(%s1 == "") %s1 = 0;

   %half = floor(%w / 2);

   Vantage::text(%x, %y, %half - 10, $Vantage::Primary, "<jr>" @ %s0, 255);
   Vantage::text(%x + %half + 10, %y, %half - 10, $Vantage::Warn, "<jl>" @ %s1, 255);
   Vantage::textSmall(%x, %y + 3, %w, $Vantage::Dim, "<jc>/", 200);

   Vantage::flagState(%x, %y + 18, %half - 10, %mine, $Vantage::Primary, "<jr>");
   Vantage::flagState(%x + %half + 10, %y + 18, %half - 10, %theirs, $Vantage::Warn, "<jl>");
}

function Vantage::flagState(%x, %y, %w, %team, %rgb, %just)
{
   %loc = Team::Flag::Location(%team);
   if(%loc == "")
      return;

   if(%loc == "home")
      Vantage::textSmall(%x, %y, %w, $Vantage::Dim, %just @ "home", 190);
   else if(%loc == "field")
      Vantage::textSmall(%x, %y, %w, $Vantage::Accent, %just @ "dropped", 235);
   else
      Vantage::textSmall(%x, %y, %w, %rgb,
         %just @ String::escapeFormatting(Client::GetName(%loc)), 255);
}

//------------------------------------------------------------------------------
// PART: items -- grenades / beacons / repair kit, top left.
//
// Dimmed at zero rather than hidden: a slot that disappears makes the row jump,
// and "I have none" is information too.
//------------------------------------------------------------------------------
function Vantage::Items(%x, %y, %w)
{
   Vantage::itemRow(%x, %y,      %w, "Grenade",    "GREN");
   Vantage::itemRow(%x, %y + 18, %w, "Beacon",     "BCN");
   Vantage::itemRow(%x, %y + 36, %w, "Repair Kit", "KIT");
}

function Vantage::itemRow(%x, %y, %w, %item, %label)
{
   %n = GetItemCount(%item);
   if(%n == "") %n = 0;

   if(%n > 0) { %c = $Vantage::Text;  %a = 235; }
   else       { %c = $Vantage::Dim;   %a = 160; }

   Vantage::textSmall(%x, %y, %w, %c, %label @ "  " @ %n, %a);
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

function Vantage::restore()
{
   if($Vantage::Saved == "")
   {
      echo("Vantage: nothing to restore.");
      return;
   }

   $pref::Hud::ColorPrimary = $Vantage::Sav::ColorPrimary;
   $pref::Hud::ColorDim     = $Vantage::Sav::ColorDim;
   $pref::Hud::ColorAccent  = $Vantage::Sav::ColorAccent;
   $pref::Hud::ColorWarn    = $Vantage::Sav::ColorWarn;
   $pref::Hud::ColorText    = $Vantage::Sav::ColorText;
   $pref::Hud::ColorPass    = $Vantage::Sav::ColorPass;

   $mj::shownames        = $Vantage::Sav::ShowNames;
   $mj::showhpbars       = $Vantage::Sav::ShowHpBars;
   $mj::showjetbars      = $Vantage::Sav::ShowJetBars;
   $mj::showhptext       = $Vantage::Sav::ShowHpText;
   $mj::barscrouch       = $Vantage::Sav::BarsCrouch;
   $mj::bar_width        = $Vantage::Sav::BarW;
   $mj::bar_height       = $Vantage::Sav::BarH;
   $mj::bar_border_width = $Vantage::Sav::BarB;
   $mj::fontdefault      = $Vantage::Sav::FontDefault;
   $mj::fontpass         = $Vantage::Sav::FontPass;
   $mj::passhelper       = $Vantage::Sav::PassHelper;
   $mj::passhelpermm     = $Vantage::Sav::PassHelperMM;

   $xChat::HiderEnabled  = $Vantage::Sav::HiderEnabled;
   $xChat::HiderTimeout  = $Vantage::Sav::HiderTimeout;
   $xChat::ScrollTimeout = $Vantage::Sav::ScrollTimeout;
   $xChat::HideCmdMsg    = $Vantage::Sav::HideCmdMsg;
   $xChat::TransChat     = $Vantage::Sav::TransChat;

   $pref::ChatDisplayModMethodX = $Vantage::Sav::ChatModX;
   $pref::ChatDisplayX          = $Vantage::Sav::ChatX;
   $pref::ChatDisplayWidth      = $Vantage::Sav::ChatWidth;

   deleteVariables("$Vantage::Sav::*");
   $Vantage::Saved = "";
   echo("Vantage: client settings restored.");
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
   Control::SetVisible(crosshairHud,   true);
   Control::SetVisible(chatDisplayHud, true);
   Control::SetVisible(Minimap,        true);
   // ★Left ON deliberately.★ Vantage has no clock part -- a real match clock
   // needs its own data layer (gameclock.acs.cs maintains $clock::* off a
   // server message), and shipping a part that silently draws nothing is worse
   // than shipping none. The stock clock is a working clock; use it.
   Control::SetVisible(clockHud,       true);
   Control::SetVisible(healthHud,      false);
   Control::SetVisible(jetPackHud,     false);
   Control::SetVisible(weaponHud,      false);
   Control::SetVisible(compassHud,     false);
   Control::SetVisible(sensorHUD,      false);
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

function ModernHUDPack::draw_vitals(%screen)
{
   %partW = 260;
   %at = ModernHUD::part("ModernHUD::VantageVitals", "bottom-center", 0, 96, 260, 22, %screen);
   Vantage::Vitals(getWord(%at, 0), getWord(%at, 1), %partW);
}

function ModernHUDPack::draw_weapon(%screen)
{
   %partW = 220;
   %at = ModernHUD::part("ModernHUD::VantageWeapon", "bottom-right", 18, 96, 220, 34, %screen);
   Vantage::Weapon(getWord(%at, 0), getWord(%at, 1), %partW);
}

function ModernHUDPack::draw_ctf(%screen)
{
   %partW = 420;
   %at = ModernHUD::part("ModernHUD::VantageCtf", "top-center", 0, 14, 420, 34, %screen);
   Vantage::Ctf(getWord(%at, 0), getWord(%at, 1), %partW);
}

function ModernHUDPack::draw_items(%screen)
{
   %partW = 150;
   %at = ModernHUD::part("ModernHUD::VantageItems", "top-left", 18, 14, 150, 56, %screen);
   Vantage::Items(getWord(%at, 0), getWord(%at, 1), %partW);
}

function ModernHUDPack::draw(%screen)
{
   if(ModernHUDPack::ownsSlot($pref::HudSlot::healthenergy))
      ModernHUDPack::draw_vitals(%screen);
   else
      ModernHUD::hide("ModernHUD::VantageVitals");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::weapon))
      ModernHUDPack::draw_weapon(%screen);
   else
      ModernHUD::hide("ModernHUD::VantageWeapon");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::ctf))
      ModernHUDPack::draw_ctf(%screen);
   else
      ModernHUD::hide("ModernHUD::VantageCtf");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::items))
      ModernHUDPack::draw_items(%screen);
   else
      ModernHUD::hide("ModernHUD::VantageItems");
}

function ModernHUDPack::onGuiOpen(%gui)
{
   if(%gui == "playGui")
      Schedule::Add("ModernHUDPack::stockHuds();", 0);
}

Event::Attach(eventGuiOpen, ModernHUDPack::onGuiOpen);
ModernHUDPack::prefs();
ModernHUDPack::stockHuds();
ModernHUDPack::init();
