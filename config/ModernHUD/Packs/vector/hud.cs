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

$ModernHUD::Enabled = true;
$ModernHUD::Pack = "Vector";
$ModernHUD::PackId = "vector";

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
// Palette / themes.
//
// $pref::Vector::Theme persists for free: the client's exit-time
// export("pref::*") sweep saves every $pref:: variable, so the pack needs no
// export() of its own -- which pack format v1 section 8 forbids anyway.
//------------------------------------------------------------------------------
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
   glAngledPolygon(%x, %y - 4, %ox, %y - 4 - %k, %ox, %y - 4 - %k + %t, %x, %y - 4 + %t);
   // bottom cap
   glAngledPolygon(%x, %y + %h + 4 - %t, %ox, %y + %h + 4 + %k - %t,
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
      glAngledPolygon(%cx - %inner - %len, %cy - %kk, %cx - %inner, %cy,
                      %cx - %inner, %cy + %t, %cx - %inner - %len, %cy - %kk + %t);
      glAngledPolygon(%cx + %inner + %len, %cy - %kk, %cx + %inner, %cy,
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
   "20|100|5", "Vector::minimapAlpha();");

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
ModernHUD::setting("bool", "pref::Vector::Minimap", "Minimap", "1",
   "", "ModernHUDPack::stockHuds();");
ModernHUD::setting("bool", "pref::miniMapCompass", "Minimap compass (NESW)", "1", "", "");

// Shape is an ENGINE pref the minimap already honours (mmPrefB "pref::miniMapSquare",
// minimap.cpp) -- it drives both the panel fill and where the compass letters sit
// on the edge. Declared here rather than forced in ModernHUDPack::prefs so the
// player's choice survives a pack reload.
ModernHUD::setting("enum", "pref::miniMapSquare", "Minimap shape", "1",
   "Circular|0;Square|1", "");

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
function ModernHUDPack::draw_reticle(%screen)
{
   %partW = 420;
   %at = ModernHUD::place("center", 0, 0, 420, 150, %screen);
   Vector::Reticle(getWord(%at, 0), getWord(%at, 1), %partW);
}

function ModernHUDPack::draw_ctf(%screen)
{
   %partW = 420;
   %at = ModernHUD::part("ModernHUD::VectorCtf", "top-center", 0, 14, 420, 44, %screen);
   Vector::Ctf(getWord(%at, 0), getWord(%at, 1), %partW);
}



function ModernHUDPack::draw(%screen)
{
   Vector::palette();

   // The shared settings panel renders in the pack's chosen face. Set per frame,
   // not in palette(): the font is its own setting and changes without a theme
   // change, and a stale menu font would be the one thing on screen still wearing
   // the old face.
   %mf = $pref::Vector::Font;
   if(%mf == "") %mf = "Verdana";
   $ModernHUD::MenuFont = %mf;

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

   // The reticle plate answers BOTH slots, so it yields if EITHER is borrowed --
   // otherwise the borrowed control renders underneath ours.
   if(ModernHUDPack::ownsSlot($pref::HudSlot::healthenergy) &&
      ModernHUDPack::ownsSlot($pref::HudSlot::weapon) &&
      ModernHUDPack::ownsSlot($pref::HudSlot::items))
      ModernHUDPack::draw_reticle(%screen);
   else
      ModernHUD::hide("ModernHUD::VectorReticle");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::ctf))
      ModernHUDPack::draw_ctf(%screen);
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

function ModernHUDPack::onGuiOpen(%gui)
{
   if(%gui == "playGui")
      Schedule::Add("ModernHUDPack::stockHuds();", 0);
}

Event::Attach(eventGuiOpen, ModernHUDPack::onGuiOpen);
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
      glAngledPolygon(%x, %y, %x + 24, %y, %x + 12, %y + %head, %x, %y + %head);
      glAngledPolygon(%x + %w - 24, %y, %x + %w, %y, %x + %w, %y + %head, %x + %w - 12, %y + %head);
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
         glAngledPolygon(%sx, %y, %sx + 13, %y, %sx + 5, %y + 6, %sx - 8, %y + 6);
         glAngledPolygon(%sx, %y + %h - 6, %sx + 13, %y + %h - 6,
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
      glAngledPolygon(%x, %y, %x + 28, %y, %x + 10, %y + 10, %x, %y + 30);
      glAngledPolygon(%x + %w - 28, %y, %x + %w, %y,
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
         glAngledPolygon(%rx, %y + 3, %rx + 8, %y + 3,
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
   glAngledPolygon(%x + %w - 18, %y, %x + %w, %y,
                   %x + %w, %y + 18, %x + %w - 18, %y);
   Vector::color($Vector::Primary, 200);
   glAngledPolygon(%x + %w - 18, %y, %x + %w, %y + 18,
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
