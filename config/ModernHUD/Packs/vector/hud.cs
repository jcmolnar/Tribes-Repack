//==============================================================================
// VECTOR -- hand-authored ModernHUD pack.        (manifest: pack.json)
//
// "authoring": "manual", so tools/modernhud_pack.py --generate REFUSES to
// overwrite this file. Design: re/vector_hud_buildout.md.
//
// THE IDEA: a fighter-jet cockpit instead of four numbers in four corners.
// Health, energy, ammo, grenades and speed all live within ~110px of the
// crosshair, so reading your own state costs no eye movement. The brackets
// EXPAND outward with velocity, which turns the cluster itself into a
// speedometer you read peripherally while looking somewhere else.
//
// Like Vantage this pack has ★no art at all★ -- every pixel is glRectangle,
// glAngledPolygon, glGradientRect and stock .pft fonts, so it has no
// missing-asset failure mode. The two shape primitives are additions made for
// this pack (scriptGL.cpp, NATIVE-PORT (ModernHUD)); an older client without
// them draws the meters and text but no angled caps or gradient beds.
//
// REVERSIBILITY: everything Vector writes outside its own namespace is saved
// first and restored by Vector::restore().
//==============================================================================

exec("ModernHUD/Framework.cs");
exec("ModernHUD/Packs/vector/components.cs");   // borrowable parts (HUD designer mix-and-match)

$ModernHUD::Enabled = true;
$ModernHUD::Pack = "Vector";
$ModernHUD::PackId = "vector";
// Options > CONFIGS/HUDS says this on the preview: the centre cluster (bars, ammo, counters)
// is deliberately not a movable part -- components.cs Vector::draw_reticle has why -- so
// clicking it selecting nothing needs saying.
$ModernHUD::PreviewNote = "The centre cluster stays on your aim point, so it cannot be moved.";

//------------------------------------------------------------------------------
// Slot ownership.
//------------------------------------------------------------------------------
function ModernHUDPack::ownsSlot(%value)
{
   if(%value == "")
      return true;
   if(%value == "off")
      return false;
   return String::findSubStr(%value, "Vector::") == 0;
}


//------------------------------------------------------------------------------
// The client-wide settings Vector seeds -- and how to get them back.
//
// ★Saved before written, every one.★ Written out longhand because the console
// has no dynamic variable ASSIGNMENT: `*expr(args)` (DynCallExprNode) is a
// dynamic CALL and the only indirection the grammar has, so a tidy
// Vector::set(%name,%value) helper would silently not assign -- leaving the
// player's setting overwritten with no way back. Same reasoning as Vantage.
//------------------------------------------------------------------------------
function Vector::apply()
{
   Vector::palette();

   if($Vector::Saved == "")
   {
      $Vector::Saved = 1;

      $Vector::Sav::ColorPrimary  = $pref::Hud::ColorPrimary;
      $Vector::Sav::ColorDim      = $pref::Hud::ColorDim;
      $Vector::Sav::ColorAccent   = $pref::Hud::ColorAccent;
      $Vector::Sav::ColorWarn     = $pref::Hud::ColorWarn;
      $Vector::Sav::ColorText     = $pref::Hud::ColorText;
      $Vector::Sav::ColorPass     = $pref::Hud::ColorPass;

      $Vector::Sav::ShowNames     = $mj::shownames;
      $Vector::Sav::ShowHpBars    = $mj::showhpbars;
      $Vector::Sav::ShowJetBars   = $mj::showjetbars;
      $Vector::Sav::ShowHpText    = $mj::showhptext;
      $Vector::Sav::BarsCrouch    = $mj::barscrouch;
      $Vector::Sav::BarW          = $mj::bar_width;
      $Vector::Sav::BarH          = $mj::bar_height;
      $Vector::Sav::BarB          = $mj::bar_border_width;
      $Vector::Sav::FontDefault   = $mj::fontdefault;
      $Vector::Sav::FontPass      = $mj::fontpass;
      $Vector::Sav::PassHelper    = $mj::passhelper;
      $Vector::Sav::PassHelperMM  = $mj::passhelpermm;
      $Vector::Sav::DrawWeapon    = $mj::DrawWeapon;
      $Vector::Sav::WeaponAlpha   = $mj::WeaponAlpha;
      $Vector::Sav::HideXhairArt  = $pref::hideCrosshairArt;

      $Vector::Sav::HiderEnabled  = $xChat::HiderEnabled;
      $Vector::Sav::HiderTimeout  = $xChat::HiderTimeout;
      $Vector::Sav::ScrollTimeout = $xChat::ScrollTimeout;
      $Vector::Sav::HideCmdMsg    = $xChat::HideCmdMsg;
      $Vector::Sav::TransChat     = $xChat::TransChat;

      $Vector::Sav::ChatModX      = $pref::ChatDisplayModMethodX;
      $Vector::Sav::ChatX         = $pref::ChatDisplayX;
      $Vector::Sav::ChatWidth     = $pref::ChatDisplayWidth;
   }

   Vector::applyColors();

   // -- the world layer ------------------------------------------------------
   $mj::shownames        = "True";
   $mj::showhpbars       = "True";
   $mj::showjetbars      = "True";
   $mj::showhptext       = "False";
   $mj::barscrouch       = "False";
   // ★Wider than the engine default (27), not narrower.★ The 20/5/1 this pack
   // shipped with was below the stock damage-box size and read as a smudge at any
   // real engagement range -- "very hard to see". 44x7 with a 2px border is legible
   // across a field, and border 2 is what lets the nameplate carry BOTH the team
   // colour (outer ring) and the health colour (inner ring) -- see the bar draw in
   // fearGuiCrosshair.cpp.
   $mj::bar_width        = "44";
   $mj::bar_height       = "7";
   $mj::bar_border_width = "2";
   $mj::fontdefault      = "sf_white_7.pft";
   $mj::fontpass         = "if_g_10b.pft";
   $mj::passhelper       = "True";
   $mj::passhelpermm     = "True";

   // Push the persisted opacity prefs into the knobs the engine reads.
   Vector::weapon();
   Vector::minimapAlpha();

   // -- chat that gets out of the way ---------------------------------------
   $xChat::HiderEnabled  = "True";
   $xChat::HiderTimeout  = "12";
   $xChat::ScrollTimeout = "5";
   $xChat::HideCmdMsg    = "True";
   $xChat::TransChat     = "True";

   // -- chat log placement, clear of the top-left items plate ----------------
   $pref::ChatDisplayModMethodX = "1";
   $pref::ChatDisplayX          = "18";
   $pref::ChatDisplayWidth      = "440";
}

//------------------------------------------------------------------------------
// Vector::defaults() -- force every $pref::Vector::* back to the pack's CURRENT
// shipped default, then re-apply.
//
// ★Why this has to exist.★ ModernHUD::setting seeds a default only when the pref
// has never been set, which is correct -- it must not stamp on a choice the
// player made. But the client's exit-time export("pref::*") sweep persists every
// pref, so the FIRST launch of a pack freezes its defaults into ClientPrefs.cs
// forever. Ship a better default afterwards and nobody who already ran the pack
// will ever see it; the change is invisible, and looks exactly like the feature
// not working. That happened three times in one session here (minimap, weapon
// opacity, and the seeds generally).
//
// So: one command that says "forget what I have, give me the pack's values".
// Deliberately NOT run automatically on load -- that would be the opposite bug,
// silently discarding the player's settings on every launch.
function Vector::defaults()
{
   $pref::Vector::Theme       = "0";
   $pref::Vector::Reticle     = "1";
   $pref::Vector::CycleMs     = "1250";
   $pref::Vector::WeaponAlpha = "65";
   $pref::Vector::Scale       = "100";
   $pref::Vector::Opacity        = "100";
   $pref::Vector::ReticleOpacity = "100";
   $pref::Vector::MinimapOpacity = "100";
   $pref::Vector::Font        = "Verdana";
   $pref::Vector::MenuX       = "";
   $pref::Vector::MenuY       = "";
   $pref::Vector::Minimap     = "1";
   $pref::Vector::ColorPrimary = "";
   $pref::Vector::ColorAccent  = "";

   // Engine prefs this pack drives, back to what it asks for.
   $pref::miniMapCompass = "True";
   $pref::miniMapSquare  = "1";
   $pref::miniMapVisible = "False";   // the legacy overlay -- never ours

   Vector::palette();
   Vector::applyColors();
   Vector::weapon();
   Vector::minimapAlpha();
   ModernHUDPack::prefs();
   ModernHUDPack::stockHuds();

   echo("Vector: settings reset to pack defaults.");
}

// Called by ModernHUD::unload (Framework.cs) when this pack is swapped away -- see the
// same hook in ascend/hud.cs for why.
function ModernHUDPack::restore()
{
   if($Vector::Saved != "")
      Vector::restore();
}

//------------------------------------------------------------------------------
// Pack lifecycle.
//------------------------------------------------------------------------------
// ★What belongs here, versus in a setting.★ prefs() re-runs on EVERY pack load,
// so anything listed here is forced back to the pack's value at every boot. That
// is right for the pack's LOOK (shape, size, zoom) and wrong for anything the
// player is given a switch for: a user who turned the minimap off would find it
// on again next launch, and would reasonably report the switch as broken.
//
// So miniMapVisible and miniMapCompass are deliberately NOT set here. They are
// declared as settings instead, which seeds each default exactly once -- when the
// pref has never been set -- and leaves the player's choice alone after that.
function ModernHUDPack::prefs()
{
   $pref::miniMapWidth   = "180";
   $pref::miniMapZoom    = "6";
   $pref::miniMapRotate  = "False";

   // ★Actively OFF, not merely left alone.★ This is the legacy canvas overlay
   // (minimap.cpp:445), a second undraggable minimap on top of the real control.
   // An earlier build of this pack seeded it to 1 as a setting, and the client's
   // exit-time export("pref::*") sweep PERSISTED that -- so it has to be written
   // false here to undo it on the machines that already saved it. Leaving it to
   // the engine default would fix only fresh installs.
   $pref::miniMapVisible = "False";
}

function ModernHUDPack::stockHuds()
{
   // ★The WHOLE set, not just the ones we want on.★ Stock visibility is global
   // client state; a pack that lists only its own leaves the rest wherever the
   // previous pack put them.
   // ★crosshairHud is ALWAYS visible -- never hide it to remove the crosshair.★
   // FearGui::Crosshair::onRender also drives the entire nameplate system: names,
   // health and jet bars, the pass helper, friend/foe skulls, target acquisition
   // (fearGuiCrosshair.cpp, gNameplate.refresh() and the bar draws). Hiding the
   // control to get rid of the stock reticle takes all of that down with it --
   // which is exactly what an earlier build of this pack did, reported as
   // "something broke player nameplates". $pref::hideCrosshairArt suppresses the
   // reticle BITMAP only, which is the part we are replacing.
   Control::SetVisible(crosshairHud, true);
   if($pref::Vector::Reticle == 0)
      $pref::hideCrosshairArt = "0";
   else
      $pref::hideCrosshairArt = "1";

   ModernHUD::stock(chatDisplayHud, true);

   // The minimap switch drives the CONTROL -- see the note on the setting. It
   // keeps the direct call and gets no ModernHUD::stock row: this pack already
   // owns the concept under its own name, and two rows switching one control
   // would fight on every assertion.
   if($pref::Vector::Minimap == 0)
      Control::SetVisible(Minimap, false);
   else
      Control::SetVisible(Minimap, true);
   // Chat/minimap resize handoff: the dynamic visibility above never passes the
   // stock() chokepoint, so the minimap was NOT an editor target in this pack.
   // Register explicitly -- visibility still gates hit testing, so a hidden
   // minimap stays untargetable and the switch's semantics are unchanged.
   ModernHUD::editTarget(Minimap);

   // Defaults; the K panel's stock rows override them per player.
   ModernHUD::stock(clockHud,       true);   // Vector has no clock part
   ModernHUD::stock(healthHud,      false);
   ModernHUD::stock(jetPackHud,     false);
   ModernHUD::stock(weaponHud,      false);
   ModernHUD::stock(compassHud,     false);
   ModernHUD::stock(sensorHUD,      false);
}

function ModernHUDPack::detachRetained()
{
   // Vector replaces no legacy container -- it has no legacy ancestor. Left
   // defined because the framework calls it unconditionally.
}

function ModernHUDPack::init()
{
   Vector::apply();
}

ModernHUD::require("ModernHUD/Core/Data/Team.cs");
ModernHUD::require("ModernHUD/Core/Data/Timer.cs");

//------------------------------------------------------------------------------
// Options rows. These render natively on the Configs tab under "Vector Settings"
// and are captured by HUD presets; the framework seeds each default when the
// pref is unset, so nothing below has to guess at an empty value.
//------------------------------------------------------------------------------
ModernHUD::setting("enum", "pref::Vector::Theme", "Colour Theme", "0",
   "Vector Cyan|0;Cyberpunk|1;Tactical Amber|2;Minimal White|3;" @
   "Stock Tribes|4;Royal Forge|5;Voidglass|6;Biohazard|7;Icewire|8;" @
   "Bloodmoon|9;Solar Flare|10;Synthwave|11;Phosphor CRT|12;" @
   "Imperial Blueprint|13",
   "Vector::theme($pref::Vector::Theme);");

// Changing this re-runs stockHuds() so crosshairHud is hidden/shown to match.
ModernHUD::setting("enum", "pref::Vector::Reticle", "Reticle", "1",
   "Stock crosshair|0;Vector ticks|1;Chevrons|2;Dot only|3",
   "ModernHUDPack::stockHuds();");

// Only right for the spinfusor -- there is no per-weapon reload export to read.
ModernHUD::setting("int", "pref::Vector::CycleMs", "Weapon cycle (ms)", "1250",
   "200|3000|50", "");

// ★Reticle size, replacing the drag-to-resize this part deliberately does not
// have.★ A dragged corner grows a HUD from that corner; a reticle has to grow
// about its CENTRE or it stops pointing where you are aiming. So the size is a
// number, and the layout maths multiplies by it -- see Vector::Reticle.
ModernHUD::setting("int", "pref::Vector::Scale", "Reticle size (%)", "100",
   "50|300|5", "");

// Any installed TrueType face. Rasterized fresh at each size (see Vector::tt), so
// this is what keeps the HUD sharp instead of pixellating as it grows. The spec is
// SCANNED (Vector::fontScan -> $Vector::FontSpec), not hard-coded: it lists only
// faces this machine actually has, so no row can offer a font that would silently
// fall back to Verdana.
Vector::fontScan();
ModernHUD::setting("enum", "pref::Vector::Font", "HUD font", "Verdana",
   $Vector::FontSpec, "");

// Opacity is TWO settings on purpose: some players want the aim point solid and
// the readouts ghosted, which one slider cannot express.
//
// This pack multiplies its own alpha ($Vector::A), so it DECLINES the framework's
// generic "HUD opacity" row -- two controls scaling the same pixels would fight.
// Size is deliberately NOT declined: "Reticle size" is about the aim point alone,
// so the framework's part-wide "HUD size" row remains a distinct, useful control.
$ModernHUD::OwnOpacity = 1;

// Same reason, for the crosshair: "Reticle" below already drives
// $pref::hideCrosshairArt through stockHuds(), so the framework's generic
// "Crosshair art" row would be a second switch on the same pref, and whichever
// ran last would win.
$ModernHUD::OwnCrosshairArt = 1;

ModernHUD::setting("int", "pref::Vector::Opacity", "HUD opacity (%)", "100",
   "20|100|5", "");
ModernHUD::setting("int", "pref::Vector::ReticleOpacity", "Reticle opacity (%)", "100",
   "20|100|5", "");

// Minimap opacity. ★The engine already blends it -- rt.cpp's minimap compositor
// reads $pref::miniMapAlpha (0..1), falling back to $pref::miniMapOpacity
// (0..255), and every shipped pack sets the former.★ So this is a percent slider
// driving the established contract, NOT a new render path.
//
// A percent cannot be written into miniMapAlpha directly: that reader treats any
// value above 1 as a 0..255 byte, so "80" would come out as 80/255 = 31%. The
// apply converts.
ModernHUD::setting("int", "pref::Vector::MinimapOpacity", "Minimap opacity (%)", "100",
   "20|100|5", "Vector::minimapAlpha();", "Minimap");

// Fade your own first-person weapon out of the way. 0 hides it outright.
// ★Default 65, not 100.★ The point of this pack is an unobstructed view, and a
// default of "fully opaque" is indistinguishable from the feature not working --
// which is how it was first reported. 65 is clearly translucent and still leaves
// the weapon readable enough to tell what you are holding.
ModernHUD::setting("int", "pref::Vector::WeaponAlpha", "Weapon opacity (%)", "65",
   "0|100|5", "Vector::weapon();");

// ★$pref::miniMapVisible is NOT "show the minimap" -- it is the LEGACY CANVAS
// OVERLAY, and turning it on gave everyone TWO minimaps.★ There are two entirely
// separate implementations (minimap.cpp):
//
//   * Minimap_render(srf), gated on $pref::miniMapVisible (:445) -- drawn straight
//     to the surface for stock play.gui users. It is not a control, so it has no
//     HudCtrl, which is why the second map could not be dragged.
//   * FearGui::Minimap : HudCtrl (:465) -- the real 1.40 control on the play gui,
//     which this pack already turns on through its stockHuds list.
//
// The file says so at :441 ("Legacy opt-in canvas overlay ... The 1.40 script play
// GUI uses the real FearGui::Minimap control below instead") and I turned it on
// anyway. The minimap switch has to drive the CONTROL.
// Tagged "Minimap": the HUD designer lists these under the minimap on the preview.
ModernHUD::setting("bool", "pref::Vector::Minimap", "Minimap", "1",
   "", "ModernHUDPack::stockHuds();", "Minimap");
ModernHUD::setting("bool", "pref::miniMapCompass", "Minimap compass (NESW)", "1", "", "", "Minimap");

// Shape is an ENGINE pref the minimap already honours (mmPrefB "pref::miniMapSquare",
// minimap.cpp) -- it drives both the panel fill and where the compass letters sit
// on the edge. Declared here rather than forced in ModernHUDPack::prefs so the
// player's choice survives a pack reload.
ModernHUD::setting("enum", "pref::miniMapSquare", "Minimap shape", "1",
   "Circular|0;Square|1", "", "Minimap");



function ModernHUDPack::draw(%screen)
{
   // Palette, scale and opacities: the same per-frame prep a borrowed Vector part runs
   // (components.cs Vector::compPrep).
   Vector::compPrep();

   // The shared settings panel renders in the pack's chosen face. Set per frame,
   // not in palette(): the font is its own setting and changes without a theme
   // change, and a stale menu font would be the one thing on screen still wearing
   // the old face.
   %mf = $pref::Vector::Font;
   if(%mf == "") %mf = "Verdana";
   $ModernHUD::MenuFont = %mf;

   // The reticle plate answers BOTH slots, so it yields if EITHER is borrowed --
   // otherwise the borrowed control renders underneath ours.
   if(ModernHUDPack::ownsSlot($pref::HudSlot::healthenergy) &&
      ModernHUDPack::ownsSlot($pref::HudSlot::weapon) &&
      ModernHUDPack::ownsSlot($pref::HudSlot::items))
      Vector::draw_reticle(%screen);
   else
      ModernHUD::hide("ModernHUD::VectorReticle");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::ctf))
      Vector::draw_ctf(%screen);
   else
      ModernHUD::hide("ModernHUD::VectorCtf");

   // The items slot is answered by the reticle plate now -- the counters moved
   // into the cluster, so there is no separate top-left part to draw. The handle
   // is hidden unconditionally so an old one cannot linger after an upgrade.
   ModernHUD::hide("ModernHUD::VectorItems");

   // The settings panel is drawn by the FRAMEWORK, after this returns
   // (ModernHUD::onDraw -> ModernHUD::menu), so it paints on top of the HUD it
   // configures without this pack owning a menu engine.
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

//==============================================================================
// THE K PANEL -- now the FRAMEWORK's, not this pack's.
//
// ★Why it exists.★ The settings were reachable only through Options -> Configs,
// and only by selecting the config, escaping, and re-entering it. In practice
// nobody finds them -- a setting a player cannot discover is a setting that does
// not exist. K already means "configure my HUD", so a pack answers it:
// $Config::HudListOwned tells the engine to keep the stock checkbox list hidden
// (dlgPlay.cpp) and the panel draws in its place.
//
// ★Why the engine moved.★ This was 250 lines of drag/hit-test/stepper code that
// only Vector had, driving rows that duplicated -- by hand, with their own
// min/max/step literals -- the ModernHUD::setting registry declared below. Every
// other pack declared the same kind of rows and got no menu at all. The engine is
// now Framework.cs (ModernHUD::menu), the rows ARE the registry, and this pack
// contributes only what is genuinely its own: the palette and the themed frame.
//
// Interaction rides glMousePos ("x y lmb rmb"), added for this. Rows are
// hit-tested against the same rectangles they are drawn with, in the same surface
// pixels, so there is no coordinate mapping to get wrong.
//
// $Config::HudListOwned is set by ModernHUD::setting as soon as a pack registers
// its first row, and cleared on unload -- it is no longer declared here.
//==============================================================================

// The framework's RESET DEFAULTS button restores every registered row; this hook
// covers what the registry cannot know about -- the engine prefs this pack drives
// and the derived palette.
function ModernHUDPack::menuReset()
{
   Vector::defaults();
}



//------------------------------------------------------------------------------
// The panel.
//
// ★Opaque, dark, and edge-lit.★ The first pass used the theme's Dim colour at
// alpha 240 and the HUD showed straight through it -- you could read the reticle
// numbers behind the settings. A settings panel is modal furniture, not a HUD
// element: it gets a solid near-black base, then a thin tint, so contrast comes
// from the panel rather than from whatever happens to be behind it.
//------------------------------------------------------------------------------
function ModernHUDPack::menuFrame(%x, %y, %w, %h, %head)
{
   %theme = $pref::Vector::Theme;

   // Every style starts from an opaque reading surface. Decorations are kept
   // inside the same rectangle so changing themes never changes hit testing.
   Vector::color("10 13 16", 252);
   glRectangle(%x, %y, %w, %h);
   Vector::color($Vector::Dim, 90);
   glRectangle(%x, %y, %w, %h);

   if(%theme == 4) // Stock Tribes: feargui green, square military framing.
   {
      Vector::color("3 14 5", 245);
      glRectangle(%x + 2, %y + 2, %w - 4, %h - 4);
      Vector::color($Vector::Primary, 230);
      glRectangle(%x, %y, %w, 2);
      glRectangle(%x, %y + %h - 2, %w, 2);
      glRectangle(%x, %y, 2, %h);
      glRectangle(%x + %w - 2, %y, 2, %h);
      Vector::color($Vector::Accent, 95);
      glRectangle(%x + 5, %y + 5, %w - 10, 1);
      glRectangle(%x + 5, %y + %h - 6, %w - 10, 1);
      glRectangle(%x + 8, %y + %head - 2, %w - 16, 2);
      for(%i = 0; %i < 4; %i++)
      {
         %cx = (%i < 2) ? %x + 5 : %x + %w - 9;
         %cy = (%i % 2 == 0) ? %y + 5 : %y + %h - 9;
         glRectangle(%cx, %cy, 4, 4);
      }
      return;
   }

   if(%theme == 5) // Royal Forge: heated plate, gold rails, rivets.
   {
      Vector::color("38 12 10", 235);
      glGradientRect(%x, %y, %w, %h, 10, 7, 8, 255);
      Vector::color($Vector::Primary, 230);
      glRectangle(%x, %y, %w, 3);
      glRectangle(%x, %y + %h - 3, %w, 3);
      Vector::color($Vector::Accent, 165);
      Vector::quad(%x, %y, %x + 24, %y, %x + 12, %y + %head, %x, %y + %head);
      Vector::quad(%x + %w - 24, %y, %x + %w, %y, %x + %w, %y + %head, %x + %w - 12, %y + %head);
      Vector::color($Vector::Primary, 180);
      for(%i = 0; %i < 4; %i++)
      {
         %rx = (%i < 2) ? %x + 7 : %x + %w - 10;
         %ry = (%i % 2 == 0) ? %y + 7 : %y + %h - 10;
         glRectangle(%rx, %ry, 3, 3);
      }
      return;
   }

   if(%theme == 6) // Voidglass: nested luminous rails and a spectral core.
   {
      Vector::color("12 5 28", 220);
      glGradientRect(%x, %y, %w, %h, 2, 18, 28, 245, "h");
      Vector::color($Vector::Primary, 215);
      glRectangle(%x, %y, 2, %h);
      glRectangle(%x + %w - 2, %y, 2, %h);
      Vector::color($Vector::Accent, 150);
      glRectangle(%x + 6, %y + 7, 1, %h - 14);
      glRectangle(%x + %w - 7, %y + 7, 1, %h - 14);
      Vector::color($Vector::Primary, 35);
      glGradientRect(%x + floor(%w * 0.30), %y, floor(%w * 0.40), %h,
                     58, 228, 255, 10, "h");
      return;
   }

   if(%theme == 7) // Biohazard: warning chevrons and containment bars.
   {
      Vector::color("12 18 3", 246);
      glRectangle(%x + 2, %y + 2, %w - 4, %h - 4);
      Vector::color($Vector::Primary, 220);
      glRectangle(%x, %y, 3, %h);
      glRectangle(%x + %w - 3, %y, 3, %h);
      Vector::color($Vector::Accent, 180);
      for(%i = 0; %i < 7; %i++)
      {
         %sx = %x + 8 + (%i * floor((%w - 16) / 7));
         Vector::quad(%sx, %y, %sx + 13, %y, %sx + 5, %y + 6, %sx - 8, %y + 6);
         Vector::quad(%sx, %y + %h - 6, %sx + 13, %y + %h - 6,
                         %sx + 5, %y + %h, %sx - 8, %y + %h);
      }
      Vector::color($Vector::Primary, 90);
      glRectangle(%x + 8, %y + %head - 1, %w - 16, 1);
      return;
   }

   if(%theme == 8) // Icewire: faceted corners and frozen inner grid.
   {
      Vector::color("7 20 31", 238);
      glGradientRect(%x, %y, %w, %h, 22, 58, 78, 245);
      Vector::color($Vector::Primary, 210);
      Vector::quad(%x, %y, %x + 28, %y, %x + 10, %y + 10, %x, %y + 30);
      Vector::quad(%x + %w - 28, %y, %x + %w, %y,
                      %x + %w, %y + 30, %x + %w - 10, %y + 10);
      Vector::color($Vector::Accent, 75);
      for(%i = 1; %i < 5; %i++)
         glRectangle(%x + floor(%i * %w / 5), %y + %head, 1, %h - %head);
      Vector::color($Vector::Primary, 160);
      glRectangle(%x + 10, %y + %h - 2, %w - 20, 2);
      return;
   }

   if(%theme == 9) // Bloodmoon: crimson spine and ritual tick marks.
   {
      Vector::color("24 3 8", 244);
      glGradientRect(%x, %y, %w, %h, 68, 4, 17, 238, "h");
      Vector::color($Vector::Primary, 245);
      glRectangle(%x, %y, 4, %h);
      glRectangle(%x + %w - 1, %y, 1, %h);
      Vector::color($Vector::Accent, 170);
      for(%i = 0; %i < 9; %i++)
      {
         %tx = %x + 8 + (%i * floor((%w - 16) / 9));
         glRectangle(%tx, %y, 2, 5);
         glRectangle(%tx, %y + %h - 5, 2, 5);
      }
      Vector::color($Vector::Primary, 90);
      glRectangle(%x + 4, %y + %head - 1, %w - 5, 1);
      return;
   }

   if(%theme == 10) // Solar Flare: hot horizon and radiating header.
   {
      Vector::color("38 10 0", 242);
      glGradientRect(%x, %y, %w, %h, 8, 12, 18, 250);
      Vector::color($Vector::Primary, 220);
      glRectangle(%x, %y, %w, 3);
      Vector::color($Vector::Accent, 120);
      for(%i = 0; %i < 5; %i++)
      {
         %rx = %x + %w - 16 - (%i * 18);
         Vector::quad(%rx, %y + 3, %rx + 8, %y + 3,
                         %rx - 3, %y + %head, %rx - 11, %y + %head);
      }
      Vector::color($Vector::Primary, 155);
      glRectangle(%x, %y + %h - 2, %w, 2);
      return;
   }

   if(%theme == 11) // Synthwave: split neon frame and horizon bands.
   {
      Vector::color("19 5 33", 246);
      glGradientRect(%x, %y, %w, %h, 3, 21, 34, 246, "h");
      Vector::color($Vector::Primary, 235);
      glRectangle(%x, %y, floor(%w / 2), 2);
      glRectangle(%x, %y, 2, %h);
      Vector::color($Vector::Accent, 235);
      glRectangle(%x + floor(%w / 2), %y, %w - floor(%w / 2), 2);
      glRectangle(%x + %w - 2, %y, 2, %h);
      Vector::color($Vector::Primary, 45);
      for(%i = 1; %i < 5; %i++)
         glRectangle(%x + 3, %y + %head + (%i * floor((%h - %head) / 5)),
                     %w - 6, 1);
      Vector::color($Vector::Accent, 190);
      glRectangle(%x + 2, %y + %h - 2, %w - 4, 2);
      return;
   }

   if(%theme == 12) // Phosphor CRT: scanlines, bloom rail, terminal corners.
   {
      Vector::color("1 10 4", 250);
      glRectangle(%x, %y, %w, %h);
      Vector::color($Vector::Primary, 34);
      for(%sy = %y + 3; %sy < %y + %h; %sy = %sy + 4)
         glRectangle(%x + 2, %sy, %w - 4, 1);
      Vector::color($Vector::Primary, 225);
      glRectangle(%x, %y, 2, %h);
      glRectangle(%x, %y, %w, 1);
      Vector::color($Vector::Accent, 150);
      glRectangle(%x + 6, %y + 6, 16, 2);
      glRectangle(%x + 6, %y + 6, 2, 16);
      glRectangle(%x + %w - 22, %y + %h - 8, 16, 2);
      glRectangle(%x + %w - 8, %y + %h - 22, 2, 16);
      return;
   }

   if(%theme == 13) // Imperial Blueprint: drafting grid with gold datum marks.
   {
      Vector::color("4 20 48", 247);
      glRectangle(%x, %y, %w, %h);
      Vector::color($Vector::Primary, 38);
      for(%i = 1; %i < 6; %i++)
      {
         glRectangle(%x + floor(%i * %w / 6), %y, 1, %h);
         glRectangle(%x, %y + floor(%i * %h / 6), %w, 1);
      }
      Vector::color($Vector::Primary, 220);
      glRectangle(%x, %y, 2, %h);
      glRectangle(%x, %y, %w, 2);
      glRectangle(%x + %w - 2, %y, 2, %h);
      glRectangle(%x, %y + %h - 2, %w, 2);
      Vector::color($Vector::Accent, 210);
      for(%i = 0; %i < 5; %i++)
      {
         %dx = %x + 12 + (%i * floor((%w - 24) / 5));
         glRectangle(%dx, %y, 2, 7);
      }
      glRectangle(%x + 8, %y + %head - 2, %w - 16, 2);
      return;
   }

   // Original four Vector themes retain the established edge-lit chamfer.
   Vector::color($Vector::Primary, 255);
   glRectangle(%x, %y, 3, %h);
   Vector::color($Vector::Primary, 90);
   glRectangle(%x + %w - 1, %y, 1, %h);
   glRectangle(%x, %y, %w, 1);
   glRectangle(%x, %y + %h - 1, %w, 1);
   Vector::color("10 13 16", 252);
   Vector::quad(%x + %w - 18, %y, %x + %w, %y,
                   %x + %w, %y + 18, %x + %w - 18, %y);
   Vector::color($Vector::Primary, 200);
   Vector::quad(%x + %w - 18, %y, %x + %w, %y + 18,
                   %x + %w - 1, %y + 18, %x + %w - 17, %y);
   Vector::color($Vector::Primary, 40);
   glGradientRect(%x + 3, %y + 1, %w - 4, %head - 2,
                  getWord($Vector::Primary, 0), getWord($Vector::Primary, 1),
                  getWord($Vector::Primary, 2), 0);
   Vector::color($Vector::Primary, 220);
   glRectangle(%x + 3, %y + %head - 2, %w - 4, 2);
}


// Font-scope Stage 3: load-completion sentinel -- MUST stay the final statement.
$ModernHUD::LoadComplete = "vector";
