//==============================================================================
// ASCEND -- hand-authored ModernHUD pack.            (manifest: pack.json)
//
// A 1:1 recreation of the Tribes: Ascend combat HUD on this client, drawn with
// ScriptGL primitives and wired to real Tribes 1 state.
//
// "authoring": "manual", so tools/modernhud_pack.py --generate REFUSES to
// overwrite this file. It is not converted from a legacy pack.
//
// WHAT IS BEING COPIED, ELEMENT BY ELEMENT
//
//   top-center strip   flag pip | flag | SCORE | [clock MM:SS] | SCORE | flag |
//                      flag pip, with the two thin guide rules running outward
//                      from the strip and fading to nothing.
//   bottom-left        two item hexagons (grenade + pack) with a key cap over
//                      the first, and the segmented health / energy bars with
//                      their numbers and glyphs to the right.
//   bottom-center      the weapon tray: a numbered tab over a chamfered plate
//                      carrying the weapon name and its ammo count, the mounted
//                      weapon lit.
//   center             the bracket reticle -- two facing chevrons around a
//                      diamond, aim point left clear.
//
// ★No art. At all.★ Every pixel is glRectangle / glAngledPolygon /
// glGradientRect / glSetFont, for the reason Vantage gives: a pack that ships
// PNGs has a missing-asset failure mode in every tree that does not have them,
// and this one has no assets to miss. It is also why the whole HUD recolours
// from one palette function.
//
// ★THREE PLACES ASCEND HAS A DATUM TRIBES 1 DOES NOT. Stated, not faked:★
//
//  1. The outer hex pips are Ascend's GENERATOR status. Tribes has no such wire
//     value, so they carry the FLAG STATE instead -- dim when home, amber with
//     the return countdown when the flag is in the field, bright when carried.
//     The slot keeps its shape and gains information the player actually has.
//  2. Ascend health runs to 1500. Here it is 0..100, which is the real number.
//     $pref::Ascend::Health = 1 draws it x15 for the screenshot look; that is
//     labelled a cosmetic scale in Options, not a different reading.
//  3. Ascend's tray is exactly three weapons because Ascend's loadout is three.
//     This draws every weapon you are carrying, numbered with the key that
//     selects it (sae.cs binds 1..8), so the tab is a usable instruction
//     rather than an ordinal.
//
// ★One deliberate departure from the reference screenshot: the minimap is ON.★
// The shot is of a mode without one; Tribes CTF without the radar is a
// downgrade, and it is one row in the K panel to turn off.
//
// REVERSIBILITY: everything Ascend writes outside its own namespace is saved
// first and restored by Ascend::restore(). A pack that permanently rewrites
// client-wide globals is a pack you cannot uninstall.
//==============================================================================

exec("ModernHUD/Framework.cs");
exec("ModernHUD/Packs/ascend/components.cs");   // borrowable parts (HUD designer mix-and-match)

$ModernHUD::Enabled = true;
$ModernHUD::Pack = "Ascend";
$ModernHUD::PackId = "ascend";

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
   return String::findSubStr(%value, "Ascend::") == 0;
}

//==============================================================================
// CLIENT-WIDE SETTINGS -- and how to get them back
//
// ★Saved before written, every one.★ These are not the pack's own state: they
// are the player's client.
//
// ★Written out longhand on purpose.★ The obvious shape is an Ascend::set(name,
// value) helper that assigns through the name, and the console has no such
// thing: `*expr(args)` is a dynamic CALL and is the only indirection the grammar
// has, so `*%var = %value` does not parse as an assignment. Two long explicit
// lists are correct; a clever one is silently broken, and silently is the
// problem -- an unparsed assignment leaves the player's setting overwritten with
// no way back.
//==============================================================================
function Ascend::apply()
{
   Ascend::palette();

   if($Ascend::Saved == "")
   {
      $Ascend::Saved = 1;

      $Ascend::Sav::ColorPrimary = $pref::Hud::ColorPrimary;
      $Ascend::Sav::ColorDim     = $pref::Hud::ColorDim;
      $Ascend::Sav::ColorAccent  = $pref::Hud::ColorAccent;
      $Ascend::Sav::ColorWarn    = $pref::Hud::ColorWarn;
      $Ascend::Sav::ColorText    = $pref::Hud::ColorText;
      $Ascend::Sav::ColorPass    = $pref::Hud::ColorPass;

      $Ascend::Sav::ShowNames    = $mj::shownames;
      $Ascend::Sav::ShowHpBars   = $mj::showhpbars;
      $Ascend::Sav::ShowJetBars  = $mj::showjetbars;
      $Ascend::Sav::ShowHpText   = $mj::showhptext;
      $Ascend::Sav::BarsCrouch   = $mj::barscrouch;
      $Ascend::Sav::BarW         = $mj::bar_width;
      $Ascend::Sav::BarH         = $mj::bar_height;
      $Ascend::Sav::BarB         = $mj::bar_border_width;
      $Ascend::Sav::PassHelper   = $mj::passhelper;
      $Ascend::Sav::PassHelperMM = $mj::passhelpermm;
      $Ascend::Sav::HidePlayerIFF= $pref::hidePlayerIFFMarker;
      $Ascend::Sav::HitMarkerPulse= $pref::hitMarkerReticlePulse;

      $Ascend::Sav::HideXArt     = $pref::hideCrosshairArt;

      $Ascend::Sav::HiderEnabled = $xChat::HiderEnabled;
      $Ascend::Sav::HiderTimeout = $xChat::HiderTimeout;
      $Ascend::Sav::ScrollTimeout= $xChat::ScrollTimeout;
      $Ascend::Sav::HideCmdMsg   = $xChat::HideCmdMsg;
      $Ascend::Sav::TransChat    = $xChat::TransChat;

      $Ascend::Sav::ChatModX     = $pref::ChatDisplayModMethodX;
      $Ascend::Sav::ChatModY     = $pref::ChatDisplayModMethodY;
      $Ascend::Sav::ChatX        = $pref::ChatDisplayX;
      $Ascend::Sav::ChatY        = $pref::ChatDisplayY;
      $Ascend::Sav::ChatWidth    = $pref::ChatDisplayWidth;

      $Ascend::Sav::ChatInModY   = $pref::ChatInputModMethodY;
      $Ascend::Sav::ChatInY      = $pref::ChatInputY;
   }

   // -- the engine-wide colour theme -----------------------------------------
   // The same numbers our own draws use, so the engine's brackets, chat and
   // target box end up the same colour as the HUD instead of fighting it.
   $pref::Hud::ColorPrimary = $Ascend::Primary;
   $pref::Hud::ColorDim     = $Ascend::Dim;
   $pref::Hud::ColorAccent  = $Ascend::Accent;
   $pref::Hud::ColorWarn    = $Ascend::Warn;
   $pref::Hud::ColorText    = $Ascend::Text;
   $pref::Hud::ColorPass    = $Ascend::Bright;

   // -- the world layer ------------------------------------------------------
   // Ascend puts a name and health/energy bars over every visible player.
   // These are independent of the stock friend/foe IFF marker drawn below them.
   $mj::shownames        = "True";
   $mj::showhpbars       = "True";
   $mj::showjetbars      = "True";
   $mj::showhptext       = "False";
   $mj::barscrouch       = "False";
   $mj::bar_width        = "22";
   $mj::bar_height       = "4";
   $mj::bar_border_width = "1";
   $mj::passhelper       = "True";
   $mj::passhelpermm     = "True";
   // Keep the stock friend/foe IFF marker.  It is the actual team indicator;
   // Ascend's name and health/energy bars complement it rather than replace it.
   $pref::hidePlayerIFFMarker = "0";

   // -- get the chat log out from under the top strip ------------------------
   // ★Measured on the first render: chatDisplayHud sits at 7,6 and is 440x60★
   // (guiDump), which is exactly the band the strip occupies -- its translucent
   // bed showed through as a slab across the left half of the bar. The control
   // reads its own placement from these prefs (fearGuiChatDisplay.cpp:781-818):
   // ModMethodY 2 means "y IS ChatDisplayY" absolutely, which is the only form
   // that can put it below a fixed-height element. The strip ends at 14+76=90.
   $pref::ChatDisplayModMethodX = "1";
   $pref::ChatDisplayX          = "18";
   $pref::ChatDisplayWidth      = "420";
   $pref::ChatDisplayModMethodY = "2";
   $pref::ChatDisplayY          = "100";

   // ★And it must FADE, or the bed is permanent furniture.★ Moving the control
   // stopped it colliding with the strip but left a 420x60 translucent slab
   // parked in the sky with nothing in it -- the box draws its bed whether or
   // not there is chat. The hider is what makes it appear only when someone
   // speaks, which is also what the reference HUD does.
   // -- lift the TALK BOX clear of the weapon tray ---------------------------
   // The chat log above and the chat INPUT are different controls, and only the
   // log was moved. Stock places the input centred at
   //    y = parent->extent.y - 40 - lineCount * msgFont->getHeight()
   // (FearGuiChat.cpp:265-266), so it occupies roughly 35..56 px above the
   // bottom. This pack's weapon tray is part(..., "bottom-center", 0, 18, 576,
   // 78, ...) -- the band 18..96 px above the bottom. The input sits entirely
   // inside the tray, and the tray draws over it: typing was invisible.
   //
   // ModMethodY 1 is "stock MINUS Y" (y -= ChatInputY, FearGuiChat.cpp:268), not
   // an absolute -- so this stays correct at every resolution, where an absolute
   // y would drift. 72 puts the input's bottom edge ~112 px above the screen
   // bottom, clearing the tray's 96 px top with a 16 px gap.
   $pref::ChatInputModMethodY = "1";
   $pref::ChatInputY          = "72";

   $xChat::HiderEnabled  = "True";
   $xChat::HiderTimeout  = "12";
   $xChat::ScrollTimeout = "5";
   $xChat::HideCmdMsg    = "True";
   $xChat::TransChat     = "True";

   $Ascend::HitMarkerPulse = $net::hitMarkerPulse;
   $Ascend::HitMarkerUntil = 0;
   Ascend::crosshair();
}

// Called by ModernHUD::unload (Framework.cs) when this pack is swapped away, so the
// crosshair-art, nameplate and chat globals it rewrote do not follow the player into
// the next pack. Before this hook existed, Ascend::restore was reachable only from the
// console, and every pack loaded after Ascend ran with the stock reticle hidden.
function ModernHUDPack::restore()
{
   // Guarded here too: this fixed name outlives the pack in the console dictionary,
   // so a later pack's unload would otherwise echo "nothing to restore" every swap.
   if($Ascend::Saved != "")
      Ascend::restore();
}

// ★A pack's first launch freezes its defaults into ClientPrefs.cs forever.★
// ModernHUD::setting seeds a default only when the pref is UNSET, so a better
// default shipped later is invisible to anyone who already ran the pack. This
// force-writes the current shipped values; it is the only way to hand them out
// after the fact. Console: Ascend::defaults();
function Ascend::defaults()
{
   $pref::Ascend::Theme          = "0";
   $pref::Ascend::Reticle        = "1";
   $pref::hitMarker              = "1";
   $pref::hidePlayerIFFMarker    = "0";
   $pref::Ascend::Scale          = "100";
   $pref::Ascend::ReticleOpacity = "100";
   $pref::Ascend::Opacity        = "100";
   $pref::Ascend::Health         = "0";
   $pref::Ascend::Rules          = "1";
   $pref::Ascend::GrenadeKey     = "G";
   Ascend::fontScan();
   $pref::Ascend::Font           = $Ascend::FontName[0];
   Ascend::palette();
   Ascend::crosshair();
   ModernHUDPack::stockHuds();
   echo("Ascend: shipped defaults re-applied.");
}

//==============================================================================
// PACK LIFECYCLE
//==============================================================================
function ModernHUDPack::prefs()
{
   // Ascend's map panel is a small square in the corner. NOT listed as a
   // ModernHUD::setting: a pref that is both forced here and offered as a row
   // would be pushed back to this value on every load, so the player's choice
   // would appear not to stick.
   $pref::miniMapSquare  = "True";
   $pref::miniMapWidth   = "176";
   $pref::miniMapZoom    = "6";
   $pref::miniMapRotate  = "False";
}

function ModernHUDPack::stockHuds()
{
   // ★The WHOLE set, not just the ones we want on.★ Stock visibility is global
   // client state; a pack that lists only its own leaves the rest wherever the
   // previous pack put them -- a measured defect, not a hypothetical.
   // ModernHUD::stock is Control::SetVisible with a player row in front of it:
   // the value here is this pack's DEFAULT, and the K panel's row overrides it.
   Control::SetVisible(crosshairHud, true);
   ModernHUD::stock(chatDisplayHud, true);
   ModernHUD::stock(Minimap,        true);
   ModernHUD::stock(clockHud,       false);   // the top strip carries the clock
   ModernHUD::stock(healthHud,      false);
   ModernHUD::stock(jetPackHud,     false);
   ModernHUD::stock(weaponHud,      false);
   ModernHUD::stock(compassHud,     false);
   ModernHUD::stock(sensorHUD,      false);

   Ascend::crosshair();
}

function ModernHUDPack::detachRetained()
{
   // Ascend replaces no legacy container -- it has no legacy ancestor. Left
   // defined because the framework calls it unconditionally, and a missing
   // lifecycle function is a per-frame console error.
}

function ModernHUDPack::init()
{
   Ascend::tables();
   Ascend::apply();
}

ModernHUD::require("ModernHUD/Core/Data/Team.cs");
ModernHUD::require("ModernHUD/Core/Data/Timer.cs");


//==============================================================================
// OPTIONS / K-MENU ROWS
//
// One declaration each: they render on Options > Configs > "Ascend Settings"
// AND as rows in the in-game K panel, and are captured by HUD presets. The
// framework seeds each default when the pref is unset.
//
// ★Applies are not ours to run.★ MHSettings_tick notices a registered pref
// changing and runs that row's apply -- calling it from a handler as well
// double-fires every one.
//==============================================================================
Ascend::fontScan();

ModernHUD::setting("enum", "pref::Ascend::Theme", "Colour Theme", "0",
   "Ascend Teal|0;Diamond Blue|1;Blood Eagle Gold|2;Phosphor Green|3",
   "Ascend::apply();");

// Changing this re-runs crosshair() so $pref::hideCrosshairArt matches.
// 1 is the reference reticle, X and all; 2 is the same thing with the aim point
// left uncovered.
ModernHUD::setting("enum", "pref::Ascend::Reticle", "Reticle", "1",
   "Stock crosshair|0;Ascend (reference)|1;Ascend, open centre|2;Diamond only|3",
   "Ascend::crosshair();");

// Modern 1.50 SHOT_FEEDBACK is rendered natively over every HUD, but its
// presentation pref defaults off unless a pack exposes and seeds it. Ascend's
// combat reticle opts in while leaving the player free to turn it back off.
ModernHUD::setting("bool", "pref::hitMarker", "Server hit marker", "1", "", "");
ModernHUD::setting("bool", "pref::hidePlayerIFFMarker", "Hide stock IFF marker", "0", "", "");

// ★Reticle size is a number, not a drag handle.★ A dragged corner grows a HUD
// from that corner; a reticle has to grow about its CENTRE or it stops pointing
// where you are aiming -- which is why the reticle is not a movable part.
ModernHUD::setting("int", "pref::Ascend::Scale", "Reticle size (%)", "100",
   "50|300|5", "");

// Opacity is TWO rows on purpose: the aim point solid with the readouts ghosted
// is a common preference and one slider cannot express it.
//
// This pack multiplies its own alpha ($Ascend::A) because it draws with raw
// glColor4ub, which the framework's generic row cannot reach (that one
// multiplies the alpha argument of imageRect / bar / markup / digitsBox). So it
// DECLINES the generic row rather than stacking a second control on the same
// pixels. Size is NOT declined: the framework's part-wide "HUD size" row still
// scales the three plates, which is a distinct and useful control.
$ModernHUD::OwnOpacity = 1;

// Same reason for the crosshair: "Reticle" above already drives
// $pref::hideCrosshairArt, so the framework's generic "Crosshair art" row would
// be a second switch on one pref and whichever ran last would win.
$ModernHUD::OwnCrosshairArt = 1;

ModernHUD::setting("int", "pref::Ascend::Opacity", "HUD opacity (%)", "100",
   "20|100|5", "");
ModernHUD::setting("int", "pref::Ascend::ReticleOpacity", "Reticle opacity (%)",
   "100", "20|100|5", "");

// ★Cosmetic, and labelled as such.★ Ascend health runs to 1500; this client's is
// 0..100 and that is the real number. The x15 option exists so the HUD matches
// the reference screenshot, not because the reading changes.
ModernHUD::setting("enum", "pref::Ascend::Health", "Health readout", "0",
   "Real (0-100)|0;Ascend scale (x15)|1", "", "ModernHUD::AscendVitals");

ModernHUD::setting("bool", "pref::Ascend::Rules", "Top bar guide lines", "1",
   "", "", "ModernHUD::AscendTop");

// There is no script accessor for a live key binding, so the cap over the
// grenade hex is a LABEL, not a reading. "G" is what base/scripts/sae.cs:89
// binds throwRelease("Grenade") to; a player who rebound it picks their key
// here.
ModernHUD::setting("enum", "pref::Ascend::GrenadeKey", "Grenade key cap", "G",
   "G|G;F|F;Q|Q;E|E;R|R;C|C;V|V;X|X;Z|Z;B|B", "", "ModernHUD::AscendWeapons");

// Any installed TrueType face, rasterized fresh at each size -- which is what
// keeps the HUD sharp instead of pixellating. The spec is SCANNED, so no row can
// offer a font that would silently fall back to something else, and the DEFAULT
// is the first survivor of a list ordered by closeness to Ascend's own face.
ModernHUD::setting("enum", "pref::Ascend::Font", "HUD font",
   $Ascend::FontName[0], $Ascend::FontSpec, "");

function ModernHUDPack::draw(%screen)
{
   // Palette, opacity and tables: the same per-frame prep a borrowed Ascend part runs
   // (components.cs Ascend::compPrep).
   Ascend::compPrep();

   %mf = $pref::Ascend::Font;
   if(%mf == "") %mf = "Verdana";
   $ModernHUD::MenuFont = %mf;

   // The reticle first, while no part scale is active. Drawn through
   // ModernHUD::place, so there is no handle to hide when a slot is borrowed --
   // it belongs to no slot.
   Ascend::Reticle(%screen);

   // The top strip answers BOTH ctf and clock, so it yields if EITHER is
   // borrowed -- otherwise the borrowed control renders underneath ours.
   if(ModernHUDPack::ownsSlot($pref::HudSlot::ctf) &&
      ModernHUDPack::ownsSlot($pref::HudSlot::clock))
      Ascend::draw_topbar(%screen);
   else
      ModernHUD::hide("ModernHUD::AscendTop");

   // Same for the bottom-left cluster: health/energy and the item hexes are one
   // plate, so replacing either yields the whole thing.
   if(ModernHUDPack::ownsSlot($pref::HudSlot::healthenergy) &&
      ModernHUDPack::ownsSlot($pref::HudSlot::items))
      Ascend::draw_vitals(%screen);
   else
      ModernHUD::hide("ModernHUD::AscendVitals");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::weapon))
      Ascend::draw_weapons(%screen);
   else
      ModernHUD::hide("ModernHUD::AscendWeapons");
}

//==============================================================================
// K-PANEL CHROME
//
// The framework owns the settings panel and its rows ARE the registry above;
// only the frame and the reset hook are ours.
//==============================================================================
function ModernHUDPack::menuFrame(%x, %y, %w, %h, %head)
{
   Ascend::palette();

   // Body: the same near-black glass the HUD plates use, chamfered at the top
   // corners so the panel is recognisably part of this HUD.
   Ascend::color($Ascend::Plate, 246);
   glRectangle(%x, %y + 8, %w, %h - 8);
   Ascend::quad(%x + 8, %y, %x + %w - 8, %y, %x + %w, %y + 8, %x, %y + 8);

   // Header band, lit along its lower edge.
   Ascend::color($Ascend::Dim, 235);
   glRectangle(%x, %y + 8, %w, %head - 8);
   Ascend::color($Ascend::Primary, 220);
   glRectangle(%x, %y + %head, %w, 2);

   // Outline.
   Ascend::color($Ascend::Edge, 205);
   glRectangle(%x, %y + 8, 1, %h - 8);
   glRectangle(%x + %w - 1, %y + 8, 1, %h - 8);
   glRectangle(%x, %y + %h - 1, %w, 1);
}

// RESET DEFAULTS restores every registered row to the default the pack
// declared; these are the things it cannot know about -- the engine prefs this
// pack drives and the derived palette.
function ModernHUDPack::menuReset()
{
   Ascend::palette();
   Ascend::crosshair();
   ModernHUDPack::prefs();
   ModernHUDPack::stockHuds();
}

//==============================================================================
// BOOT
//==============================================================================
// ★Bound to eventGuiOpen_PlayGui, NOT eventGuiOpen plus a gui-name test.★ TWO
// independent firers raise eventGuiOpen with DIFFERENT spellings: the ENGINE
// with the control's real name (`playGui`, simGuiCanvas.cpp:907) and PRESTO with
// a hardcoded bare word (`PlayGui`, Presto/events.cs:681). A string VALUE
// comparison is case-SENSITIVE even though NAME lookup is not, so a test against
// either spelling silently ignores the other firer. Presto fires an
// argument-free eventGuiOpen_PlayGui from the same function (events.cs:746);
// binding that removes the string test altogether, at the same frequency.
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
$ModernHUD::LoadComplete = "ascend";
