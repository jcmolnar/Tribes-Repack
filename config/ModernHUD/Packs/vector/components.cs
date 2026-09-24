//==============================================================================
// Vector -- BORROWABLE PARTS (components.cs)
//
// Split out of hud.cs by tools/modernhud_components.py so another pack can draw these
// parts: the HUD designer's Provider choice sets $pref::HudSlot::<slot> to
// "vector/<slot>", and ModernHUD::drawSlot execs this file on demand. It must define only
// Vector:: names -- ModernHUDPack:: belongs to whichever pack is the base.
// hud.cs execs this file too, so the pack's own draw uses the same code.
//==============================================================================

ModernHUD::require("ModernHUD/Core/Data/Team.cs");
ModernHUD::require("ModernHUD/Core/Data/Timer.cs");


//------------------------------------------------------------------------------
// Palette / themes.
//
// $pref::Vector::Theme persists for free: the client's exit-time
// export("pref::*") sweep saves every $pref:: variable, so the pack needs no
// export() of its own -- which pack format v1 section 8 forbids anyway.
//------------------------------------------------------------------------------
// Per-frame prep for anything that draws Vector: its own draw() and every borrowed
// Vector part (compReady).
function Vector::compPrep()
{
   Vector::palette();

   // One place computes the scale, every draw helper reads it. Clamped here so a
   // hand-edited pref cannot produce a HUD too small to read or big enough to fill
   // the screen -- the same range the Options slider offers.
   %k = $pref::Vector::Scale;
   if(%k == "" || %k <= 0) %k = 100;
   if(%k < 50)  %k = 50;
   if(%k > 300) %k = 300;
   $Vector::K = %k / 100;

   %o = $pref::Vector::Opacity;
   if(%o == "" || %o <= 0) %o = 100;
   if(%o > 100) %o = 100;
   $Vector::A = %o / 100;

   %ro = $pref::Vector::ReticleOpacity;
   if(%ro == "" || %ro <= 0) %ro = 100;
   if(%ro > 100) %ro = 100;
   $Vector::AR = %ro / 100;
}

function Vector::palette()
{
   %theme = $pref::Vector::Theme;

   // Shared defaults. Themes below may override all six semantic colours.
   $Vector::Warn = "255 60 60";
   $Vector::Text = "235 245 255";
   $Vector::Pass = "255 105 180";

   if(%theme == 1)            // Cyberpunk
   {
      $Vector::Primary = "255 0 127";
      $Vector::Dim     = "74 0 38";
      $Vector::Accent  = "255 230 0";
   }
   else if(%theme == 2)       // Tactical amber
   {
      $Vector::Primary = "255 170 0";
      $Vector::Dim     = "74 50 0";
      $Vector::Accent  = "0 255 204";
   }
   else if(%theme == 3)       // Minimal white
   {
      $Vector::Primary = "240 245 255";
      $Vector::Dim     = "50 55 60";
      $Vector::Accent  = "255 255 0";
   }
   else if(%theme == 4)       // Stock Tribes
   {
      // feargui.pal entries 2 and 250: normal and bright interface green.
      $Vector::Primary = "0 191 0";
      $Vector::Dim     = "0 45 0";
      $Vector::Accent  = "0 255 0";
      $Vector::Warn    = "255 96 32";
      $Vector::Text    = "190 232 190";
      $Vector::Pass    = "0 255 0";
   }
   else if(%theme == 5)       // Royal Forge
   {
      $Vector::Primary = "232 175 64";
      $Vector::Dim     = "71 28 22";
      $Vector::Accent  = "220 58 38";
      $Vector::Warn    = "255 70 50";
      $Vector::Text    = "255 240 205";
      $Vector::Pass    = "255 211 97";
   }
   else if(%theme == 6)       // Voidglass
   {
      $Vector::Primary = "154 92 255";
      $Vector::Dim     = "38 17 70";
      $Vector::Accent  = "58 228 255";
      $Vector::Warn    = "255 72 132";
      $Vector::Text    = "235 225 255";
      $Vector::Pass    = "91 255 200";
   }
   else if(%theme == 7)       // Biohazard
   {
      $Vector::Primary = "174 255 0";
      $Vector::Dim     = "37 55 0";
      $Vector::Accent  = "255 232 0";
      $Vector::Warn    = "255 76 0";
      $Vector::Text    = "236 255 196";
      $Vector::Pass    = "93 255 67";
   }
   else if(%theme == 8)       // Icewire
   {
      $Vector::Primary = "148 224 255";
      $Vector::Dim     = "23 53 74";
      $Vector::Accent  = "255 255 255";
      $Vector::Warn    = "255 106 134";
      $Vector::Text    = "226 247 255";
      $Vector::Pass    = "93 255 226";
   }
   else if(%theme == 9)       // Bloodmoon
   {
      $Vector::Primary = "235 35 65";
      $Vector::Dim     = "75 8 20";
      $Vector::Accent  = "255 132 48";
      $Vector::Warn    = "255 205 68";
      $Vector::Text    = "255 224 226";
      $Vector::Pass    = "255 91 155";
   }
   else if(%theme == 10)      // Solar Flare
   {
      $Vector::Primary = "255 119 0";
      $Vector::Dim     = "82 25 0";
      $Vector::Accent  = "255 232 64";
      $Vector::Warn    = "255 56 31";
      $Vector::Text    = "255 239 210";
      $Vector::Pass    = "255 176 44";
   }
   else if(%theme == 11)      // Synthwave
   {
      $Vector::Primary = "255 54 220";
      $Vector::Dim     = "55 12 75";
      $Vector::Accent  = "33 236 255";
      $Vector::Warn    = "255 85 120";
      $Vector::Text    = "249 225 255";
      $Vector::Pass    = "114 255 214";
   }
   else if(%theme == 12)      // Phosphor CRT
   {
      $Vector::Primary = "79 255 116";
      $Vector::Dim     = "10 55 24";
      $Vector::Accent  = "193 255 174";
      $Vector::Warn    = "255 195 61";
      $Vector::Text    = "182 255 194";
      $Vector::Pass    = "79 255 116";
   }
   else if(%theme == 13)      // Imperial Blueprint
   {
      $Vector::Primary = "55 143 255";
      $Vector::Dim     = "11 38 82";
      $Vector::Accent  = "255 197 61";
      $Vector::Warn    = "255 92 72";
      $Vector::Text    = "225 238 255";
      $Vector::Pass    = "100 224 255";
   }
   else                       // 0 / unset: Vector cyan
   {
      $Vector::Primary = "0 200 255";
      $Vector::Dim     = "0 62 78";
      $Vector::Accent  = "255 190 60";
   }

   if($pref::Vector::ColorPrimary != "") $Vector::Primary = $pref::Vector::ColorPrimary;
   if($pref::Vector::ColorAccent  != "") $Vector::Accent  = $pref::Vector::ColorAccent;

   // Publish the same six colours to the framework's settings panel, so the K
   // menu is themed by whatever theme the HUD is wearing. This is the ENTIRE
   // colour contract between a pack and the shared menu engine -- a pack that
   // sets nothing gets the framework's default blue.
   $ModernHUD::MenuPrimary = $Vector::Primary;
   $ModernHUD::MenuDim     = $Vector::Dim;
   $ModernHUD::MenuAccent  = $Vector::Accent;
   $ModernHUD::MenuText    = $Vector::Text;
   $ModernHUD::MenuWarn    = $Vector::Warn;
   $ModernHUD::MenuTitle   = "VECTOR";
}

// Switch theme live: repaint the pack AND the engine-wide colours it drives.
// Console-callable, which is the only UI a pack gets today.
function Vector::theme(%n)
{
   $pref::Vector::Theme = %n;
   Vector::palette();
   Vector::applyColors();
   echo("Vector: theme " @ %n);
}

//------------------------------------------------------------------------------
// Draw helpers.
//
// ★No out-of-combat dimming.★ An earlier build faded the whole HUD to 35% after
// 5s idle. Play-tested and removed: the moment you are NOT shooting is exactly
// when you are reading your ammo and picking a target, so the fade hid the
// information at the only time there was time to read it. A HUD that is legible
// only while you are already busy is backwards.
//------------------------------------------------------------------------------
// ★One multiplier, applied at the two places every pixel goes through.★
// $Vector::A is the pack opacity; the reticle swaps in its own value around its
// own draw so the aim point can stay solid while the readouts fade -- those are
// different jobs and players weight them differently.
function Vector::color(%rgb, %alpha)
{
   %a = $Vector::A;
   if(%a == "") %a = 1;
   glColor4ub(getWord(%rgb, 0), getWord(%rgb, 1), getWord(%rgb, 2), %alpha * %a);
}

//------------------------------------------------------------------------------
// ★Text is drawn TrueType, not as .pft bitmap glyphs -- that is the whole reason
// this HUD stays sharp when you scale it up.★
//
// A .pft is a fixed-size bitmap font. Making a part bigger scales its draw
// (glPartScale is a modelview scale), so a 10px glyph gets stretched and goes
// visibly blocky -- exactly the "they start getting pixellated" report.
// glSetFont(name, pixelHeight) instead rasterizes a FRESH GDI atlas at whatever
// size is asked for (scriptGL.cpp, GGO_GRAY8_BITMAP = 65-level AA), so scaling
// re-renders the glyphs rather than magnifying them. Same reason the minimap
// compass letters are sharp at every map size.
//
// Cost: one atlas per (name, px) pair, cached. Our sizes come from a pref that
// changes only when the player drags a slider, so the cache stays tiny.
//
// Justification is done here because glDrawString has no <jc>/<jr> -- those tags
// belong to the markup path. glGetStringDimensions gives the measured width in
// the CURRENT font, which is why glSetFont has to come first.
//   %just: "l" left, "c" centre, "r" right, within %width from %x.
function Vector::tt(%x, %y, %width, %rgb, %str, %alpha, %px, %just)
{
   if(%px < 6) %px = 6;

   // ★Quantise the pixel height.★ Every distinct (font, px) pair is its own GDI
   // atlas, and the size here is DERIVED from a scale slider -- so a continuous
   // slider would mint a new atlas per step and churn the cache. Snapping to even
   // sizes halves the distinct heights at no visible cost (a 1px difference in a
   // 13px glyph is not readable), and keeps the working set small enough that the
   // LRU never has to evict anything that is still on screen.
   %px = floor(%px / 2) * 2;
   if(%px < 6) %px = 6;

   %font = $pref::Vector::Font;
   if(%font == "") %font = "Verdana";

   glSetFont(%font, %px);

   %sw = getWord(glGetStringDimensions(%str), 0);
   if(%just == "c")      %x = %x + floor((%width - %sw) / 2);
   else if(%just == "r") %x = %x + %width - %sw;

   %am = $Vector::A;
   if(%am == "") %am = 1;
   %alpha = %alpha * %am;

   // ScriptGL's own inline colour markup: <rrggbbaa>, parsed by sglDrawStringTex.
   %tag = "<" @ Vector::hex(%rgb) @ Vector::hex2(%alpha) @ ">";

   // Cheap 1px drop shadow -- the HUD sits over terrain and sky, and dark-on-light
   // is otherwise unreadable at small sizes.
   glDrawString(%x + 1, %y + 1, "<000000" @ Vector::hex2(%alpha * 0.65) @ ">" @ %str);
   glDrawString(%x, %y, %tag @ %str);
}

// Measured width of %str in the pack font at %px. Used to place things RELATIVE
// to text instead of guessing an offset -- the header subtitle was first laid out
// with leading spaces, which does nothing dependable in a proportional font and
// simply drew the two strings on top of each other.
function Vector::ttWidth(%str, %px)
{
   %px = floor(%px / 2) * 2;
   if(%px < 6) %px = 6;
   %font = $pref::Vector::Font;
   if(%font == "") %font = "Verdana";
   glSetFont(%font, %px);
   return getWord(glGetStringDimensions(%str), 0);
}

// The two legacy helpers, kept so the CTF and items parts do not have to change.
// They now route to the TrueType path at the pack's scaled sizes.
function Vector::text(%x, %y, %width, %rgb, %str, %alpha)
{
   Vector::tt(%x, %y, %width, %rgb, Vector::stripJust(%str), %alpha,
              floor(14 * $Vector::K), Vector::justOf(%str));
}

function Vector::textSmall(%x, %y, %width, %rgb, %str, %alpha)
{
   Vector::tt(%x, %y, %width, %rgb, Vector::stripJust(%str), %alpha,
              floor(11 * $Vector::K), Vector::justOf(%str));
}

// The existing call sites pass "<jc>"/"<jr>"/"<jl>" prefixes (markup vocabulary).
// Translate rather than rewrite forty call sites: pull the tag off the front and
// hand the letter to Vector::tt.
function Vector::justOf(%str)
{
   if(String::findSubStr(%str, "<jc>") == 0) return "c";
   if(String::findSubStr(%str, "<jr>") == 0) return "r";
   return "l";
}

function Vector::stripJust(%str)
{
   if(String::findSubStr(%str, "<jc>") == 0 ||
      String::findSubStr(%str, "<jr>") == 0 ||
      String::findSubStr(%str, "<jl>") == 0)
      return String::getSubStr(%str, 4, 1000);
   return %str;
}

// "r g b" -> "rrggbb". The console has no printf, so this is a nibble table.
function Vector::hex(%rgb)
{
   return Vector::hex2(getWord(%rgb, 0)) @ Vector::hex2(getWord(%rgb, 1)) @
          Vector::hex2(getWord(%rgb, 2));
}

// ★FLOOR FIRST -- this must always emit exactly two characters.★
//
// The alpha reaching here is multiplied by the opacity setting, so it is
// routinely fractional: 235 * 0.5 = 117.5. Without the floor the low nibble came
// out as 117.5 - 112 = 5.5, and Vector::nib returns anything <= 9 unchanged, so
// the tag became "<00c8ff75.5>" -- eleven characters where the parser expects
// eight or ten. sglDrawStringTex then fails to read it as a colour, strips it as
// an unknown tag, and the string renders in whatever colour was last set.
//
// Reported as "changing opacity makes the numbers dance a bit and change colour":
// both symptoms are this one malformed tag -- the colour from the failed parse,
// the dancing from the shadow pass failing differently to the main pass.
function Vector::hex2(%v)
{
   %v = floor(%v);
   if(%v < 0)   %v = 0;
   if(%v > 255) %v = 255;
   %hi = floor(%v / 16);
   return Vector::nib(%hi) @ Vector::nib(%v - %hi * 16);
}

function Vector::nib(%n)
{
   %n = floor(%n);
   if(%n < 0)  %n = 0;
   if(%n > 15) %n = 15;
   if(%n <= 9)
      return %n;
   if(%n == 10) return "a";
   if(%n == 11) return "b";
   if(%n == 12) return "c";
   if(%n == 13) return "d";
   if(%n == 14) return "e";
   return "f";
}

// A glass bed: the pack colour at %alpha fading to nothing downward. One draw
// call (glGradientRect), where a flat-rect fake needs ~20 stepped rects.
function Vector::glass(%x, %y, %w, %h, %rgb, %alpha)
{
   Vector::color(%rgb, %alpha);
   glGradientRect(%x, %y, %w, %h, getWord(%rgb, 0), getWord(%rgb, 1),
                  getWord(%rgb, 2), 0);
}

//------------------------------------------------------------------------------
// A vertical segmented meter, filling bottom-up like a fuel gauge.
//
// Segmented rather than continuous on purpose: at the sizes this cluster uses a
// smooth bar moves less than a pixel per point of health, so it reads as static.
// Discrete cells make a single disc hit visible in peripheral vision, which is
// the whole reason the readout is next to the crosshair.
//------------------------------------------------------------------------------
function Vector::vmeter(%x, %y, %w, %h, %segs, %frac, %rgb, %alpha)
{
   if(%frac < 0) %frac = 0;
   if(%frac > 1) %frac = 1;

   %gap  = 2;
   %segH = floor((%h - (%segs - 1) * %gap) / %segs);
   if(%segH < 1) %segH = 1;

   %active = floor(%segs * %frac + 0.5);
   // ★Never round a living player down to an empty gauge.★ At 8 segments
   // anything under 6% rounds to zero cells, and "no cells" is the same picture
   // as "dead" -- the one reading that must never be wrong.
   if(%active < 1 && %frac > 0)
      %active = 1;

   for(%i = 0; %i < %segs; %i++)
   {
      %sy = %y + %h - %segH - %i * (%segH + %gap);
      if(%i < %active)
         Vector::color(%rgb, %alpha);
      else
         Vector::color($Vector::Dim, %alpha * 0.35);
      glRectangle(%x, %sy, %w, %segH);
   }
}

//------------------------------------------------------------------------------
// ★Every angled quad in this pack goes through here, and it is load-bearing.★
//
// glAngledPolygon does NOT disable GL_CULL_FACE (scriptGL.cpp c_glAngledPolygon
// pushes GL_ENABLE_BIT and clears only TEXTURE_2D and ALPHA_TEST), so it
// INHERITS the frame's culling -- which in the ModernHUD pass is ON. A quad
// wound the wrong way is dropped silently. The function's own comment claims the
// opposite ("Winding is not enforced: the surface's 2D pass runs with culling
// off"), which is why this went unnoticed for so long; glRectangle is immune
// because it derives its four corners itself, always in the same order.
//
// ★This pack was SHIPPING with the damage.★ Measured 2026-08-22 at 200% reticle
// scale, one frame, four predictions in two directions -- all four correct:
//   Vector::caps  dir=1  (left/health)  shoelace -48 -> cap MISSING   [observed]
//   Vector::caps  dir=-1 (right/energy) shoelace +48 -> cap present   [observed]
//   drawReticle style 2, left chevron   shoelace +2LT -> present      [observed]
//   drawReticle style 2, right chevron  shoelace -2LT -> MISSING      [observed]
// So the health bracket had no end caps and the "Chevrons" reticle had only its
// left half, in every session since those shapes were written.
//
// ★The test is the SHOELACE over all four vertices, not (v1-v0)x(v2-v0).★ The
// menu-frame chamfers and the caps are triangles written with a repeated vertex,
// where the three-point cross product is exactly 0 -- a three-point test waves
// through the very shapes that go missing.
//
// Reversing v0..v3 to v3..v0 flips the winding and leaves the polygon identical.
// This becomes a harmless no-op if the engine ever disables culling there.
//------------------------------------------------------------------------------
function Vector::quad(%x1, %y1, %x2, %y2, %x3, %y3, %x4, %y4)
{
   %a = (%x1 * %y2 - %x2 * %y1) + (%x2 * %y3 - %x3 * %y2) +
        (%x3 * %y4 - %x4 * %y3) + (%x4 * %y1 - %x1 * %y4);

   if(%a >= 0)
      glAngledPolygon(%x1, %y1, %x2, %y2, %x3, %y3, %x4, %y4);
   else
      glAngledPolygon(%x4, %y4, %x3, %y3, %x2, %y2, %x1, %y1);
}

// Angled caps above and below a meter column. %dir is 1 for the left bracket
// (points left) and -1 for the right. This is what glAngledPolygon was added
// for -- as axis-aligned rects the same shape is a visible staircase.
function Vector::caps(%x, %y, %w, %h, %dir, %rgb, %alpha)
{
   Vector::color(%rgb, %alpha);

   %k  = 8;                      // how far the cap leans outward
   %t  = 3;                      // cap thickness
   %ox = %x - %dir * %k;         // outer edge, away from the crosshair

   // top cap
   Vector::quad(%x, %y - 4, %ox, %y - 4 - %k, %ox, %y - 4 - %k + %t, %x, %y - 4 + %t);
   // bottom cap
   Vector::quad(%x, %y + %h + 4 - %t, %ox, %y + %h + 4 + %k - %t,
                   %ox, %y + %h + 4 + %k, %x, %y + %h + 4);
}

//------------------------------------------------------------------------------
// The reticle itself -- drawn, not the stock crosshair bitmap.
//
// $pref::Vector::Reticle picks: 0 leaves the stock crosshairHud alone, 1..3 hide
// it and draw one of these. Four ticks around a centre gap, so the exact aim
// point stays UNOCCLUDED -- the pixel you are shooting at is the one pixel that
// must not have HUD on it. The centre dot is 1px and optional per style.
//
// The ticks share the kinetic spread with the brackets, so the whole reticle
// breathes with velocity rather than the brackets moving against a fixed cross.
//------------------------------------------------------------------------------
function Vector::drawReticle(%cx, %cy, %spread, %rgb)
{
   %style = $pref::Vector::Reticle;
   if(%style == 0)
      return;

   %k = $Vector::K;
   if(%k == "" || %k <= 0) %k = 1;
   %inner = floor(6 * %k) + floor(%spread / 6);   // gap half-width: opens as you accelerate
   %len   = floor(7 * %k);
   %t     = floor(2 * %k);
   if(%len < 2) %len = 2;
   if(%t < 1)   %t = 1;

   Vector::color(%rgb, 235);

   if(%style == 2)
   {
      // Chevrons: angled ticks pointing inward at the aim point. This is the
      // shape glAngledPolygon exists for -- as rects it is a staircase.
      %kk = floor(5 * %k);
      if(%kk < 1) %kk = 1;
      Vector::quad(%cx - %inner - %len, %cy - %kk, %cx - %inner, %cy,
                      %cx - %inner, %cy + %t, %cx - %inner - %len, %cy - %kk + %t);
      Vector::quad(%cx + %inner + %len, %cy - %kk, %cx + %inner, %cy,
                      %cx + %inner, %cy + %t, %cx + %inner + %len, %cy - %kk + %t);
      glRectangle(%cx - floor(%t / 2), %cy - %inner - %len, %t, %len);
      return;
   }

   if(%style == 3)
   {
      // Dot only -- the least occlusion there is.
      Vector::color(%rgb, 255);
      glRectangle(%cx - 1, %cy - 1, 3, 3);
      return;
   }

   // style 1: four ticks + centre dot
   glRectangle(%cx - %inner - %len, %cy - floor(%t / 2), %len, %t);   // left
   glRectangle(%cx + %inner,        %cy - floor(%t / 2), %len, %t);   // right
   glRectangle(%cx - floor(%t / 2), %cy - %inner - %len, %t, %len);   // top
   glRectangle(%cx - floor(%t / 2), %cy + %inner,        %t, %len);   // bottom

   Vector::color(%rgb, 255);
   glRectangle(%cx - 1, %cy - 1, 2, 2);
}

//------------------------------------------------------------------------------
// PART: reticle -- the cluster. Reticle, health, energy, speed, grenades, the
// weapon cycle bar and ammo, all hung off the aim point.
//
// LAYOUT (play-tested, revised 2026-07-27):
//
//                        SPEED            <- above; read while skiing, not aiming
//         [HP]| |         +          | |[EN]
//          72                          88   <- exact values, under each bracket
//                     ============           <- next-shot bar
//                          24                <- ammo, under the bar
//
// ★Speed above and ammo below is not arbitrary.★ Speed matters while you are
// travelling and looking UP at terrain; ammo matters at the moment you fire,
// which is when your eye is already on the bar telling you when you next can.
// Putting ammo out to the left meant looking away from the thing it belongs to.
//
// Owns BOTH the healthenergy and weapon slots (see pack.json): the ammo readout
// is in the same plate as the health bracket, so a player who swapped only the
// weapon slot would otherwise get that pack's weapon plate drawn on top of ours.
//------------------------------------------------------------------------------
function Vector::Reticle(%x, %y, %w)
{
   %health = $health;
   %energy = $energy;
   if(%health == "") %health = 0;
   if(%energy == "") %energy = 0;

   %speed = $speed;
   if(%speed == "") %speed = 0;

   %flash = $damageFlash;
   if(%flash == "") %flash = 0;

   %cx = %x + floor(%w / 2);
   %cy = %y + 75;                // the part box is 150 tall, centred on the aim point

   // ★Everything below is multiplied by %k, and the CENTRE never moves.★ The
   // cluster is laid out as offsets from (%cx,%cy), so scaling the offsets grows
   // it symmetrically about the aim point -- which is what a reticle has to do.
   // This is why the part deliberately has no drag handle: a HUD you resize by
   // dragging a corner grows from that corner, and a reticle that drifts off
   // centre as you enlarge it is broken. $pref::Vector::Scale, 50-300%.
   %k = $Vector::K;

   // -- kinetic expansion ----------------------------------------------------
   // $speed is world units/sec (kronosNativeCmds.cpp CfgSyncHudVars_now:
   // int(getLinearVelocity().len() + 0.5)) -- NOT km/h. Walking sits near 25,
   // a held ski line runs 100-200. /4 maps that onto 0..45px of travel, which is
   // enough to see without the brackets leaving the useful centre of the screen.
   %spread = floor(%speed / 4);
   if(%spread > 45) %spread = 45;
   %spread = floor(%spread * %k);

   %gap = floor((52 * %k) + %spread);   // inner edge of each bracket, from the aim point
   %bw  = floor(12 * %k);               // bracket column width
   %bh  = floor(46 * %k);
   %by  = %cy - floor(%bh / 2);

   // -- health, left ---------------------------------------------------------
   // ★Flash white on the frame the server says we were hit.★ $damageFlash is the
   // live wire value (0 clean, up to 0.76 under fire), exported for exactly this.
   // Watching $health drop instead would miss chip damage absorbed by armour and
   // is a frame late by construction.
   if(%flash > 0.02)     %hc = "255 255 255";
   else if(%health > 66) %hc = $Vector::Primary;
   else if(%health > 33) %hc = $Vector::Accent;
   else                  %hc = $Vector::Warn;

   %hx = %cx - %gap - %bw;
   Vector::glass(%hx - floor(2*%k), %by - floor(2*%k), %bw + floor(4*%k), %bh + floor(4*%k), $Vector::Dim, 120);
   Vector::vmeter(%hx, %by, %bw, %bh, 8, %health / 100, %hc, 235);
   Vector::caps(%hx, %by, %bw, %bh, 1, %hc, 200);

   // -- energy, right --------------------------------------------------------
   %ex = %cx + %gap;
   Vector::glass(%ex - floor(2*%k), %by - floor(2*%k), %bw + floor(4*%k), %bh + floor(4*%k), $Vector::Dim, 120);
   Vector::vmeter(%ex, %by, %bw, %bh, 8, %energy / 100, $Vector::Primary, 215);
   Vector::caps(%ex + %bw, %by, %bw, %bh, -1, $Vector::Primary, 180);

   // -- the exact numbers, centred under their own bracket -------------------
   // ★A segmented gauge is for peripheral vision; the number is for decisions.★
   // Eight cells cannot tell 62 from 71, and "do I survive one more disc" is a
   // question about the number. Both, each doing the job it is good at.
   Vector::text(%hx - floor(19*%k), %cy + floor(27*%k), %bw + floor(38*%k), %hc,
                "<jc>" @ floor(%health), 250);
   Vector::text(%ex - floor(19*%k), %cy + floor(27*%k), %bw + floor(38*%k), $Vector::Primary,
                "<jc>" @ floor(%energy), 235);

   // -- speed, ABOVE the aim point -------------------------------------------
   %sc = (%speed >= 100) ? $Vector::Accent : $Vector::Primary;
   Vector::textSmall(%cx - floor(60*%k), %cy - floor(48*%k), floor(120*%k), %sc,
                     "<jc>" @ floor(%speed), 230);

   // -- the reticle ----------------------------------------------------------
   // Swap in the reticle's own opacity for the duration of its draw only.
   %savedA = $Vector::A;
   $Vector::A = $Vector::AR;
   Vector::drawReticle(%cx, %cy, %spread, $Vector::Primary);
   $Vector::A = %savedA;

   // -- item column, outboard of the energy bracket --------------------------
   // ★Moved in from the old top-left plate.★ The point of this pack is that you
   // never look away from the aim point; a counter in the screen corner is the one
   // thing that forces you to. Four rows, stacked on the bracket, so inventory is
   // read with the same glance as health and ammo.
   //
   // "G|0" not "G 0": at HUD sizes a space between a letter and a zero reads as
   // the word GO. The bar is a separator you cannot misread as a glyph.
   %ix = %ex + %bw + floor(18 * %k);
   %iw = floor(56 * %k);
   %istep = floor(15 * %k);
   %iy = %cy - floor(24 * %k);

   Vector::itemLine(%ix, %iy,              %iw, "G", "Grenade");
   Vector::itemLine(%ix, %iy + %istep,     %iw, "B", "Beacon");
   Vector::itemLine(%ix, %iy + %istep * 2, %iw, "M", "Mine");
   Vector::itemLine(%ix, %iy + %istep * 3, %iw, "K", "Repair Kit");

   // -- next-shot bar, then ammo directly beneath it -------------------------
   %ammo = $Weapon::Ammo;
   %wep  = GetItemDesc(GetMountedItem(0));

   Vector::cycleBar(%cx, %cy + floor(44*%k), %wep, %ammo);

   // ★"" is not the empty case -- 0 and -1 are.★ The export always writes an
   // integer: -1 when nothing is mounted, 0 for a mounted weapon with no ammo
   // type (energy weapons). `%ammo != "" && %ammo >= 0` would be true forever,
   // and would paint a red 0 while you are holding a blaster.
   if(%wep != "" && %ammo != "" && %ammo > 0)
   {
      %ac = (%ammo <= 2) ? $Vector::Warn : $Vector::Text;
      Vector::text(%cx - floor(40*%k), %cy + floor(52*%k), floor(80*%k), %ac,
                   "<jc>" @ %ammo, 255);
   }
   else if(%wep != "")
   {
      // Energy weapon: say so once, rather than a counter that is always 0.
      Vector::textSmall(%cx - floor(40*%k), %cy + floor(55*%k), floor(80*%k), $Vector::Dim,
                        "<jc>--", 190);
   }
}

// One inventory row. Dim at zero rather than hidden: a row that vanishes makes the
// column jump, and "I have none" is information too.
function Vector::itemLine(%x, %y, %w, %tag, %item)
{
   %n = GetItemCount(%item);
   if(%n == "") %n = 0;

   if(%n > 0) { %c = $Vector::Accent; %a = 225; }
   else       { %c = $Vector::Dim;    %a = 170; }

   Vector::textSmall(%x, %y, %w, %c, "<jl>" @ %tag @ "|" @ %n, %a);
}

//------------------------------------------------------------------------------
// The "disc ready" bar.
//
// ★What this can and cannot know.★ There is no fire-time or reload-state export
// -- the client is told the ammo COUNT and nothing else. So the bar starts on an
// observed drop of exactly one round while the same weapon stays mounted, which
// is what firing looks like from here. Requiring the weapon to be unchanged and
// the drop to be exactly 1 is what keeps a respawn, an inventory swap or a
// station restock (all of which move ammo by more than one, or change the
// weapon) from starting a phantom cycle.
//
// The cycle LENGTH is a preference, not a measurement: $pref::Vector::CycleMs,
// default 1250 for the spinfusor. It is wrong for every other weapon, and it is
// labelled a knob rather than pretending otherwise.
//------------------------------------------------------------------------------
function Vector::cycleBar(%cx, %y, %wep, %ammo)
{
   %ms = $pref::Vector::CycleMs;
   if(%ms == "" || %ms <= 0) %ms = 1250;

   %now = glTicks();

   if(%wep == $Vector::LastWep && $Vector::LastAmmo != "" &&
      %ammo == $Vector::LastAmmo - 1)
      $Vector::FiredAt = %now;

   $Vector::LastWep  = %wep;
   $Vector::LastAmmo = %ammo;

   if($Vector::FiredAt == "")
      return;

   %elapsed = %now - $Vector::FiredAt;
   if(%elapsed < 0)                 // clock reset on a map change -- do not lurch
   {
      $Vector::FiredAt = "";
      return;
   }

   %k = $Vector::K;
   if(%k == "" || %k <= 0) %k = 1;
   %w = floor(80 * %k);
   %h = floor(3 * %k);
   if(%h < 2) %h = 2;
   %x = %cx - floor(%w / 2);

   if(%elapsed < %ms)
   {
      Vector::color($Vector::Dim, 170);
      glRectangle(%x, %y, %w, %h);
      Vector::color($Vector::Primary, 240);
      glRectangle(%x, %y, floor(%w * (%elapsed / %ms)), %h);
   }
   else if(%elapsed < %ms + 120)    // ready flash
   {
      Vector::color("255 255 255", 255);
      glRectangle(%x, %y, %w, %h);
   }
   else
   {
      // Stop redrawing a bar nobody is waiting on.
      $Vector::FiredAt = "";
   }
}

//------------------------------------------------------------------------------
// PART: ctf -- both scores and both flag states, top centre.
// Uses the shared Team.cs data layer from the framework's Core/Data.
//------------------------------------------------------------------------------
function Vector::Ctf(%x, %y, %w)
{
   %mine   = Team::Friendly();
   %theirs = Team::Enemy();
   if(%mine == "" || %theirs == "")
      return;

   %s0 = Team::Score(%mine);
   %s1 = Team::Score(%theirs);
   if(%s0 == "") %s0 = 0;
   if(%s1 == "") %s1 = 0;

   // ★Names, not bare numbers.★ Two digits either side of a slash tells you the
   // score but not whose -- you have to already know which side you are on, which
   // is exactly the thing a scoreboard should not assume. TEAMNAME: n reads
   // correctly at a glance and matches the reference layout.
   //
   // $Team::Name is 0-indexed at READ time: Team::onTeamAdd stores at [%team - 1]
   // because PlayerManager.cpp:2516 sends a 1-based index (`++numTeams`), while
   // Team::Friendly() returns the 0-based client team. So [%mine] is correct and
   // [%mine - 1] would be off by one -- the mismatch is only apparent if you read
   // both ends.
   %n0 = $Team::Name[%mine];
   %n1 = $Team::Name[%theirs];
   if(%n0 == "") %n0 = "TEAM 1";
   if(%n1 == "") %n1 = "TEAM 2";

   // Team names come off the WIRE. escapeFormatting so a name containing markup
   // cannot rewrite the rest of this string -- the same treatment flagState
   // already gives player names.
   %n0 = String::toUpper(String::escapeFormatting(%n0));
   %n1 = String::toUpper(String::escapeFormatting(%n1));

   %half = floor(%w / 2);

   Vector::glass(%x, %y - 5, %w, 28, $Vector::Dim, 120);

   Vector::text(%x, %y, %half - 12, $Vector::Primary, "<jr>" @ %n0 @ ": " @ %s0, 255);
   Vector::text(%x + %half + 12, %y, %half - 12, $Vector::Warn, "<jl>" @ %n1 @ ": " @ %s1, 255);
   Vector::textSmall(%x, %y + 3, %w, $Vector::Dim, "<jc>/", 210);

   Vector::flagState(%x, %y + 20, %half - 12, %mine, $Vector::Primary, "<jr>");
   Vector::flagState(%x + %half + 12, %y + 20, %half - 12, %theirs, $Vector::Warn, "<jl>");
}

function Vector::flagState(%x, %y, %w, %team, %rgb, %just)
{
   %loc = Team::Flag::Location(%team);
   if(%loc == "")
      return;

   if(%loc == "home")
      Vector::textSmall(%x, %y, %w, $Vector::Dim, %just @ "home", 190);
   else if(%loc == "field")
      Vector::textSmall(%x, %y, %w, $Vector::Accent, %just @ "dropped", 235);
   else
      Vector::textSmall(%x, %y, %w, %rgb,
         %just @ String::escapeFormatting(Client::GetName(%loc)), 255);
}

//------------------------------------------------------------------------------
// First-person weapon opacity.
//
// The engine already does the work: playerInventory.cpp reads $mj::DrawWeapon and
// $mj::WeaponAlpha every frame and drives ShapeBase's own setAlphaAlways/
// alphaLevel fade. Two of the sixteen $mj:: knobs the legacy packs ship DEAD.
// This just gives them a slider.
//
// ★$mj::WeaponAlpha is a float OPACITY 0..1, not a 0-255 byte.★ ProConfig ships
// 0.5; Basic/v0dkA/Overstep ship 2 (>1 clamps to opaque). Our pref is a whole
// PERCENT because that is what a slider should show, and is divided here.
//
// One control, two knobs: dragging to 0 sets DrawWeapon false rather than an
// alpha of zero, so the weapon is genuinely skipped instead of drawn invisible.
//
// ★$mj:: is NOT persisted★ -- it is not in the exit-time export("pref::*") sweep
// (checked, no $mj:: export anywhere), which is why the pref that survives a
// restart is ours and this function pushes it into $mj:: on load and on change.
function Vector::minimapAlpha()
{
   %v = $pref::Vector::MinimapOpacity;
   if(%v == "") %v = 100;
   if(%v < 5)   %v = 5;
   if(%v > 100) %v = 100;
   $pref::miniMapAlpha = %v / 100;
}

function Vector::weapon()
{
   %v = $pref::Vector::WeaponAlpha;
   if(%v == "") %v = 100;
   if(%v < 0)   %v = 0;
   if(%v > 100) %v = 100;

   if(%v <= 0)
   {
      $mj::DrawWeapon  = "False";
      $mj::WeaponAlpha = "0";
      return;
   }

   $mj::DrawWeapon  = "True";
   $mj::WeaponAlpha = %v / 100;
}

// Split out of apply() so Vector::theme() can repaint the engine-wide colours
// without re-running (and re-snapshotting) everything else.
function Vector::applyColors()
{
   $pref::Hud::ColorPrimary = $Vector::Primary;
   $pref::Hud::ColorDim     = $Vector::Dim;
   $pref::Hud::ColorAccent  = $Vector::Accent;
   $pref::Hud::ColorWarn    = $Vector::Warn;
   $pref::Hud::ColorText    = $Vector::Text;
   $pref::Hud::ColorPass    = $Vector::Pass;
}

function Vector::restore()
{
   if($Vector::Saved == "")
   {
      echo("Vector: nothing to restore.");
      return;
   }

   $pref::Hud::ColorPrimary = $Vector::Sav::ColorPrimary;
   $pref::Hud::ColorDim     = $Vector::Sav::ColorDim;
   $pref::Hud::ColorAccent  = $Vector::Sav::ColorAccent;
   $pref::Hud::ColorWarn    = $Vector::Sav::ColorWarn;
   $pref::Hud::ColorText    = $Vector::Sav::ColorText;
   $pref::Hud::ColorPass    = $Vector::Sav::ColorPass;

   $mj::shownames        = $Vector::Sav::ShowNames;
   $mj::showhpbars       = $Vector::Sav::ShowHpBars;
   $mj::showjetbars      = $Vector::Sav::ShowJetBars;
   $mj::showhptext       = $Vector::Sav::ShowHpText;
   $mj::barscrouch       = $Vector::Sav::BarsCrouch;
   $mj::bar_width        = $Vector::Sav::BarW;
   $mj::bar_height       = $Vector::Sav::BarH;
   $mj::bar_border_width = $Vector::Sav::BarB;
   $mj::fontdefault      = $Vector::Sav::FontDefault;
   $mj::fontpass         = $Vector::Sav::FontPass;
   $mj::passhelper       = $Vector::Sav::PassHelper;
   $mj::passhelpermm     = $Vector::Sav::PassHelperMM;
   $mj::DrawWeapon       = $Vector::Sav::DrawWeapon;
   $mj::WeaponAlpha      = $Vector::Sav::WeaponAlpha;
   $pref::hideCrosshairArt = $Vector::Sav::HideXhairArt;

   $xChat::HiderEnabled  = $Vector::Sav::HiderEnabled;
   $xChat::HiderTimeout  = $Vector::Sav::HiderTimeout;
   $xChat::ScrollTimeout = $Vector::Sav::ScrollTimeout;
   $xChat::HideCmdMsg    = $Vector::Sav::HideCmdMsg;
   $xChat::TransChat     = $Vector::Sav::TransChat;

   $pref::ChatDisplayModMethodX = $Vector::Sav::ChatModX;
   $pref::ChatDisplayX          = $Vector::Sav::ChatX;
   $pref::ChatDisplayWidth      = $Vector::Sav::ChatWidth;

   deleteVariables("$Vector::Sav::*");
   $Vector::Saved = "";
   echo("Vector: client settings restored.");
}

//------------------------------------------------------------------------------
// The font list, built from what is ACTUALLY INSTALLED.
//
// ★Not a hard-coded five.★ glSetFont takes any installed family, so the old list
// was a limit I imposed, not one the engine has. But CreateFontA silently
// SUBSTITUTES for an unknown family rather than failing -- so offering a name
// that is not present would appear to work and quietly render as something else.
// glFontExists (scriptGL.cpp) asks Windows, and only the ones that answer are
// offered. That makes the list honest on every machine instead of correct on
// mine.
//
// Curated rather than "every installed family": a raw enumeration is 200+ entries
// including symbol and script faces, which is worse UI than five. These are faces
// that ship with Windows or Office and actually suit a game HUD -- condensed and
// geometric grotesques for readability at speed, monospaced faces for numerics
// that must not jitter as digits change.
function Vector::fontScan()
{
   if($Vector::FontCount != "")
      return;

   %cand = "Bahnschrift Condensed;Bahnschrift;Agency FB;Eurostile;Franklin Gothic Medium;" @
           "Trebuchet MS;Verdana;Tahoma;Segoe UI;Segoe UI Semibold;Calibri;Candara;Corbel;" @
           "Century Gothic;Arial;Arial Narrow;Impact;Rockwell;Microsoft Sans Serif;" @
           "Consolas;Cascadia Mono;Lucida Console;OCR A Extended;Copperplate Gothic Bold";

   %n = 0;
   %i = 0;
   %cur = "";
   %len = String::Length(%cand);

   // No split-on-string helper here, so walk it. String::Explode exists but returns
   // a packed string we would have to walk anyway.
   for(%i = 0; %i <= %len; %i++)
   {
      %c = String::getSubStr(%cand, %i, 1);
      if(%c == ";" || %i == %len)
      {
         if(%cur != "" && glFontExists(%cur) == 1)
         {
            $Vector::FontName[%n] = %cur;
            %n++;
         }
         %cur = "";
      }
      else
         %cur = %cur @ %c;
   }

   // Verdana ships on every Windows and is the atlas fallback, so it is the floor.
   if(%n == 0)
   {
      $Vector::FontName[0] = "Verdana";
      %n = 1;
   }
   $Vector::FontCount = %n;
   echo("Vector: " @ %n @ " HUD fonts available");

   // Build the enum spec for the font row from the SAME scanned list.
   // ★The two used to disagree.★ The old K panel stepped an index over this array
   // (every installed candidate), while the Options row carried a hand-written
   // five-entry spec -- so the same setting offered a different set of fonts
   // depending on which surface you opened, and a face picked in one could not be
   // reached from the other. One list, built once, feeds both.
   %spec = "";
   for(%f = 0; %f < %n; %f++)
   {
      if(%f > 0) %spec = %spec @ ";";
      %spec = %spec @ $Vector::FontName[%f] @ "|" @ $Vector::FontName[%f];
   }
   $Vector::FontSpec = %spec;
}

//------------------------------------------------------------------------------
// Draw dispatch.
//------------------------------------------------------------------------------
// ★The reticle is NOT a movable part, and that is the whole point of it.★
//
// Every other part goes through ModernHUD::part, which creates a retained handle
// so the player can drag it and the K editor can select it. This one uses
// ModernHUD::place directly -- the anchor maths without the handle -- so it is
// recomputed to dead screen centre on every frame, at every resolution.
//
// Why: a reticle that can be dragged off the aim point is a broken reticle. It
// was draggable, so it got dragged, and then "reset positions" could not put it
// back either (the handle it needed to reposition was the one the framework's
// counter bug had left nameless). Both failure modes disappear if the thing is
// simply not movable -- there is no position to save, restore, reset or lose.
//
// The manifest still lists the part and its handle: that entry is what makes the
// slot ownership and the K-editor slot list correct. It just never gets a
// retained control, and ModernHUD::hide is a no-op for a handle that was never
// created (Framework.cs:622 checks isObject first).
function Vector::draw_reticle(%screen)
{
   %partW = 420;
   %at = ModernHUD::place("center", 0, 0, 420, 150, %screen);
   Vector::Reticle(getWord(%at, 0), getWord(%at, 1), %partW);
}

function Vector::draw_ctf(%screen)
{
   %partW = 420;
   %at = ModernHUD::part("ModernHUD::VectorCtf", "top-center", 0, 14, 420, 44, %screen);
   Vector::Ctf(getWord(%at, 0), getWord(%at, 1), %partW);
}



function Vector::shapeName(%v)
{
   if(%v == 0) return "CIRCULAR";
   return "SQUARE";
}

function Vector::onOff(%v)
{
   if(%v == 0) return "OFF";
   return "ON";
}

function Vector::themeName(%v)
{
   if(%v == 1) return "CYBERPUNK";
   if(%v == 2) return "TACTICAL AMBER";
   if(%v == 3) return "MINIMAL WHITE";
   if(%v == 4) return "STOCK TRIBES";
   if(%v == 5) return "ROYAL FORGE";
   if(%v == 6) return "VOIDGLASS";
   if(%v == 7) return "BIOHAZARD";
   if(%v == 8) return "ICEWIRE";
   if(%v == 9) return "BLOODMOON";
   if(%v == 10) return "SOLAR FLARE";
   if(%v == 11) return "SYNTHWAVE";
   if(%v == 12) return "PHOSPHOR CRT";
   if(%v == 13) return "IMPERIAL BLUEPRINT";
   return "VECTOR CYAN";
}

function Vector::reticleName(%v)
{
   if(%v == 0) return "STOCK";
   if(%v == 2) return "CHEVRONS";
   if(%v == 3) return "DOT";
   return "TICKS";
}

function Vector::fontIndex(%n)
{
   Vector::fontScan();
   for(%i = 0; %i < $Vector::FontCount; %i++)
      if($Vector::FontName[%i] == %n)
         return %i;
   return 0;
}

function Vector::fontName(%i)
{
   Vector::fontScan();
   if(%i < 0) %i = 0;
   if(%i >= $Vector::FontCount) %i = $Vector::FontCount - 1;
   return $Vector::FontName[%i];
}

//------------------------------------------------------------------------------
// Components: one per slot, each drawing that slot's parts (generated).
//------------------------------------------------------------------------------
function Vector::compReady()
{
   if(isFunction("Vector::compPrep"))
      Vector::compPrep();
}

function Vector::comp_healthenergy(%screen)
{
   Vector::compReady();
   Vector::draw_reticle(%screen);
}

function Vector::comp_ctf(%screen)
{
   Vector::compReady();
   Vector::draw_ctf(%screen);
}

ModernHUD::component("vector", "healthenergy", "healthenergy", "Vector::comp_healthenergy");
ModernHUD::component("vector", "ctf", "ctf", "Vector::comp_ctf");
