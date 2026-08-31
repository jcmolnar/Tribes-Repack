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
// PALETTE
//
// Ascend's HUD is two colours and a bed: your team's light teal, the enemy's
// red, and a near-black glass plate. Everything else is a tint of those, which
// is why a theme here is four numbers rather than a table.
//==============================================================================
function Ascend::palette()
{
   // ★The opacity multiplier must never be "".★ Every draw here is
   // colour * $Ascend::A, and an empty multiplier evaluates to 0 -- an invisible
   // HUD, not a dim one. draw() sets it per frame, but menuFrame() is also
   // called by the framework and palette() is the one function both share.
   if($Ascend::A == "") $Ascend::A = 1;

   %t = $pref::Ascend::Theme;
   if(%t == "") %t = 0;

   // Ascend teal -- the reference.
   $Ascend::Primary = "125 226 208";
   $Ascend::Bright  = "196 250 240";
   $Ascend::Dim     = "20 62 60";
   $Ascend::Edge    = "78 158 150";
   $Ascend::Enemy   = "232 70 62";
   $Ascend::EnemyDim= "74 24 22";

   if(%t == 1)          // Ascend Blue -- the Diamond Sword palette
   {
      $Ascend::Primary = "108 186 255";
      $Ascend::Bright  = "196 228 255";
      $Ascend::Dim     = "18 48 76";
      $Ascend::Edge    = "70 128 190";
   }
   else if(%t == 2)     // Ascend Gold -- the Blood Eagle palette
   {
      $Ascend::Primary = "255 196 88";
      $Ascend::Bright  = "255 232 186";
      $Ascend::Dim     = "70 50 14";
      $Ascend::Edge    = "180 138 62";
      $Ascend::Enemy   = "120 150 255";   // red against gold is unreadable
      $Ascend::EnemyDim= "28 38 78";
   }
   else if(%t == 3)     // Phosphor -- Tribes' own green, in Ascend's shapes
   {
      $Ascend::Primary = "126 240 130";
      $Ascend::Bright  = "206 255 208";
      $Ascend::Dim     = "20 62 24";
      $Ascend::Edge    = "80 160 82";
   }

   $Ascend::Plate  = "8 20 24";
   $Ascend::Text   = "232 246 244";
   $Ascend::Accent = "255 190 74";
   $Ascend::Warn   = "255 76 66";

   // The shared K panel wears the pack's colours.
   $ModernHUD::MenuPrimary = $Ascend::Primary;
   $ModernHUD::MenuDim     = $Ascend::Dim;
   $ModernHUD::MenuAccent  = $Ascend::Accent;
   $ModernHUD::MenuText    = $Ascend::Text;
   $ModernHUD::MenuWarn    = $Ascend::Warn;
   $ModernHUD::MenuTitle   = "ASCEND";
}

// Set the raw-GL draw colour, with the pack opacity folded in.
function Ascend::color(%rgb, %alpha)
{
   %a = %alpha * $Ascend::A;
   if(%a < 0)   %a = 0;
   if(%a > 255) %a = 255;
   glColor4ub(getWord(%rgb, 0), getWord(%rgb, 1), getWord(%rgb, 2), floor(%a));
}

//------------------------------------------------------------------------------
// "r g b" -> "rrggbb". The console has no printf, so this is a nibble table.
//
// ★hex2 floors.★ Alpha here is a product of an opacity fraction, so it arrives
// fractional; an un-floored value builds a colour tag that is not 8 or 10
// characters, and a malformed tag is stripped as unknown -- the text then draws
// in whatever colour was last set, which is a very confusing bug to look at.
//------------------------------------------------------------------------------
function Ascend::hex(%rgb)
{
   return Ascend::hex2(getWord(%rgb, 0)) @ Ascend::hex2(getWord(%rgb, 1)) @
          Ascend::hex2(getWord(%rgb, 2));
}

function Ascend::hex2(%v)
{
   %v = floor(%v);
   if(%v < 0)   %v = 0;
   if(%v > 255) %v = 255;
   return Ascend::nib(floor(%v / 16)) @ Ascend::nib(%v - floor(%v / 16) * 16);
}

function Ascend::nib(%n)
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

//==============================================================================
// TEXT
//
// glSetFont + glDrawString, never .pft markup: a .pft is a fixed-size bitmap
// font, so a scaled part MAGNIFIES its glyphs, while glSetFont rasterizes a
// fresh GDI atlas at the requested size. Ascend's look is a condensed grotesque,
// which only exists as TrueType anyway.
//
//   %just: "l" left, "c" centre, "r" right, within %width from %x.
//------------------------------------------------------------------------------
function Ascend::tt(%x, %y, %width, %rgb, %str, %alpha, %px, %just)
{
   // ★Quantise the pixel height.★ Every distinct (family, px) pair is its own GDI
   // atlas in a 32-entry LRU. Snapping to even sizes halves the distinct heights
   // at no visible cost and keeps the working set inside the cache.
   %px = floor(%px / 2) * 2;
   if(%px < 6) %px = 6;

   %font = $pref::Ascend::Font;
   if(%font == "") %font = "Verdana";

   glSetFont(%font, %px);

   %sw = getWord(glGetStringDimensions(%str), 0);
   if(%just == "c")      %x = %x + floor((%width - %sw) / 2);
   else if(%just == "r") %x = %x + %width - %sw;

   %a = %alpha * $Ascend::A;
   if(%a < 0)   %a = 0;
   if(%a > 255) %a = 255;

   // The HUD sits over terrain and sky; a 1px drop shadow is what keeps light
   // text readable against a snowfield.
   glDrawString(%x + 1, %y + 1, "<000000" @ Ascend::hex2(%a * 0.7) @ ">" @ %str);
   glDrawString(%x, %y, "<" @ Ascend::hex(%rgb) @ Ascend::hex2(%a) @ ">" @ %str);
}

// Measured width in the pack font at %px -- used to place things RELATIVE to
// text rather than guessing an offset in a proportional face.
function Ascend::ttWidth(%str, %px)
{
   %px = floor(%px / 2) * 2;
   if(%px < 6) %px = 6;
   %font = $pref::Ascend::Font;
   if(%font == "") %font = "Verdana";
   glSetFont(%font, %px);
   return getWord(glGetStringDimensions(%str), 0);
}

// Zero-padded two-digit field, for the clock.
function Ascend::pad2(%v)
{
   %v = floor(%v);
   if(%v < 0)  %v = 0;
   if(%v < 10) return "0" @ %v;
   return %v;
}

//==============================================================================
// SHAPES -- the Ascend chrome vocabulary
//==============================================================================

//------------------------------------------------------------------------------
// ★EVERY angled quad in this pack goes through here, and it is load-bearing.★
//
// glAngledPolygon does NOT disable GL_CULL_FACE (scriptGL.cpp c_glAngledPolygon
// pushes GL_ENABLE_BIT and clears only TEXTURE_2D and ALPHA_TEST), so it
// INHERITS whatever culling the frame left on -- and in the ModernHUD pass that
// is face culling ON. A quad wound the wrong way is silently dropped.
//
// The function's own comment says the opposite -- "Winding is not enforced: the
// surface's 2D pass runs with culling off, so a clockwise and a counter-clockwise
// quad both fill" -- and that is why this was not obvious. glRectangle never
// exposed it because it derives its four corners itself, always in the same
// order, so it is always on the surviving side.
//
// ★Measured, not reasoned.★ First render of this pack, at 997x736:
//   * hexagons drew as HOUSES -- pointed top cap present, bottom cap gone
//   * the friendly score plate's left chevron was missing; the enemy's right
//     chevron, the mirror image, drew
//   * the enemy flag was a bare pole; the friendly pennant drew
//   * one of the two lightning-bolt strokes drew
// Four mirrored pairs, four times exactly one half survived. Computing the
// shoelace area of each of those eight quads predicts the surviving four with no
// exceptions: positive draws, negative is culled.
//
// ★The test is the SHOELACE over all four vertices, not a cross product of the
// first three.★ Several of these quads are triangles expressed with a repeated
// vertex (the chevrons are), and there (v1-v0)x(v2-v0) is exactly 0 -- so a
// three-point test waves through the very shapes that were missing.
//
// Reversing v0..v3 to v3..v0 flips the winding and leaves the polygon identical.
//------------------------------------------------------------------------------
function Ascend::quad(%x1, %y1, %x2, %y2, %x3, %y3, %x4, %y4)
{
   %a = (%x1 * %y2 - %x2 * %y1) + (%x2 * %y3 - %x3 * %y2) +
        (%x3 * %y4 - %x4 * %y3) + (%x4 * %y1 - %x1 * %y4);

   if(%a >= 0)
      glAngledPolygon(%x1, %y1, %x2, %y2, %x3, %y3, %x4, %y4);
   else
      glAngledPolygon(%x4, %y4, %x3, %y3, %x2, %y2, %x1, %y1);
}

// A chamfered plate. %style: "flat" square, "left" / "right" a chevron point on
// that edge, "both" chamfered top corners. Fill first, edge line on top, so the
// outline always reads even where the fill is nearly transparent.
function Ascend::plate(%x, %y, %w, %h, %style, %rgb, %alpha, %edge, %edgeA)
{
   %k = floor(%h / 2);          // how far a pointed edge leans out
   %c = 6;                      // corner chamfer

   // Where the FLAT top and bottom of this shape actually start and stop. The
   // edge lines below are drawn over that span, not over %w: on a pointed plate
   // the last %k pixels are the triangle, and a full-width rule there draws a
   // line hanging in empty space beyond the fill.
   %ex = %x;   %ew = %w;        // flat span of the TOP edge
   %bx = %x;   %bw = %w;        // flat span of the BOTTOM edge

   Ascend::color(%rgb, %alpha);

   if(%style == "left")
   {
      glRectangle(%x + %k, %y, %w - %k, %h);
      Ascend::quad(%x + %k, %y, %x, %y + %k, %x, %y + %k, %x + %k, %y + %h);
      %ex = %x + %k;   %ew = %w - %k;
      %bx = %ex;       %bw = %ew;
   }
   else if(%style == "right")
   {
      glRectangle(%x, %y, %w - %k, %h);
      Ascend::quad(%x + %w - %k, %y, %x + %w, %y + %k,
                      %x + %w, %y + %k, %x + %w - %k, %y + %h);
      %ew = %w - %k;
      %bw = %ew;
   }
   else if(%style == "both")
   {
      glRectangle(%x, %y + %c, %w, %h - %c);
      Ascend::quad(%x + %c, %y, %x + %w - %c, %y,
                      %x + %w, %y + %c, %x, %y + %c);
      %ex = %x + %c;   %ew = %w - %c * 2;
   }
   else
      glRectangle(%x, %y, %w, %h);

   if(%edgeA <= 0)
      return;

   // Ascend plates are lit along the top edge and grounded along the bottom.
   Ascend::color(%edge, %edgeA);
   glRectangle(%ex, %y, %ew, 1);
   Ascend::color(%edge, %edgeA * 0.45);
   glRectangle(%bx, %y + %h - 1, %bw, 1);
}

// A pointy-top hexagon: one centre band and two caps. Half-width %rw,
// half-height %rh. Three draws, and the outline version is six.
function Ascend::hexFill(%cx, %cy, %rw, %rh, %rgb, %alpha)
{
   %q = floor(%rh / 2);
   Ascend::color(%rgb, %alpha);
   glRectangle(%cx - %rw, %cy - %q, %rw * 2, %q * 2);
   Ascend::quad(%cx - %rw, %cy - %q, %cx, %cy - %rh, %cx + %rw, %cy - %q,
                   %cx, %cy - %q);
   Ascend::quad(%cx - %rw, %cy + %q, %cx, %cy + %rh, %cx + %rw, %cy + %q,
                   %cx, %cy + %q);
}

// Hexagon with a border: the border colour at full size, the fill inset by %t.
//
// ★NOT called Ascend::hex.★ That name is the "r g b" -> "rrggbb" converter
// above, and this console has ONE function namespace -- a second definition
// placement-news over the first (Dictionary::addFunction), so the colour helper
// would silently become a shape and every text colour tag in the pack would
// break at once. Different job, different name.
function Ascend::hexRing(%cx, %cy, %rw, %rh, %edge, %edgeA, %fill, %fillA, %t)
{
   Ascend::hexFill(%cx, %cy, %rw, %rh, %edge, %edgeA);
   Ascend::hexFill(%cx, %cy, %rw - %t, %rh - %t * 2, %fill, %fillA);
}

// A horizontal segmented meter -- Ascend's health and energy bars.
//
// Segmented, not continuous, for the reason Vector's vertical one is: at these
// sizes a smooth bar moves under a pixel per point and reads as static, while a
// cell going dark is visible in peripheral vision.
function Ascend::segbar(%x, %y, %w, %h, %segs, %frac, %rgb, %alpha)
{
   if(%frac < 0) %frac = 0;
   if(%frac > 1) %frac = 1;

   %gap = 2;

   // The bed, as one draw behind the cells.
   Ascend::color($Ascend::Plate, %alpha * 0.72);
   glRectangle(%x - 2, %y - 2, %w + 4, %h + 4);

   %active = floor(%segs * %frac + 0.5);
   // ★Never round a living player down to an empty gauge.★ At 14 cells anything
   // under 3.5% rounds to zero, and "no cells" is the same picture as "dead" --
   // the one reading that must never be wrong.
   if(%active < 1 && %frac > 0)
      %active = 1;

   // ★Each cell's edges are derived from its own fraction of %w, not from one
   // floor()ed cell width.★ A single rounded width leaves the remainder
   // unpainted at the right end, so two bars with DIFFERENT cell counts over the
   // same %w end at different x -- measured on the first render: the 14-cell
   // health bar ran 194px and the 12-cell energy bar 190px, and the ragged right
   // edge is the first thing the eye picks up on a pair of stacked bars.
   for(%i = 0; %i < %segs; %i++)
   {
      %x0 = %x + floor(%i * (%w + %gap) / %segs);
      %x1 = %x + floor((%i + 1) * (%w + %gap) / %segs) - %gap;
      if(%x1 <= %x0) %x1 = %x0 + 1;

      if(%i < %active)
         Ascend::color(%rgb, %alpha);
      else
         Ascend::color($Ascend::Dim, %alpha * 0.55);
      glRectangle(%x0, %y, %x1 - %x0, %h);
   }
}

// One of the thin guide rules that run outward from the top strip, fading out.
// %dir 1 fades to the right, -1 fades to the left.
function Ascend::rule(%x, %y, %w, %dir, %rgb, %alpha)
{
   %r = getWord(%rgb, 0);
   %g = getWord(%rgb, 1);
   %b = getWord(%rgb, 2);
   %a = %alpha * $Ascend::A;
   if(%a > 255) %a = 255;

   if(%dir > 0)
   {
      Ascend::color(%rgb, %alpha);
      glGradientRect(%x, %y, %w, 1, %r, %g, %b, 0, "h");
   }
   else
   {
      glColor4ub(%r, %g, %b, 0);
      glGradientRect(%x, %y, %w, 1, %r, %g, %b, floor(%a), "h");
   }
}

//==============================================================================
// GLYPHS -- the four icons the reference uses, drawn rather than shipped
//==============================================================================

// Lightning bolt: two leaning limbs with a horizontal JOG between them.
//
// ★The jog is the whole glyph.★ The first cut put the waist at cy±1, which
// makes the two limbs very nearly collinear -- and they rendered as one thick
// diagonal slash, not a bolt. Overlapping them across a real band (cy±%q) with
// the upper limb ending LEFT of centre and the lower starting RIGHT of it is
// what produces the kink the eye reads as lightning.
function Ascend::glyphBolt(%cx, %cy, %s, %rgb, %alpha)
{
   Ascend::color(%rgb, %alpha);

   %w = floor(%s * 0.50);   if(%w < 3) %w = 3;
   %t = floor(%s * 0.42);   if(%t < 2) %t = 2;
   %q = floor(%s * 0.30);   if(%q < 1) %q = 1;

   // upper limb: top-right down to the waist, finishing left of centre
   Ascend::quad(%cx + %w, %cy - %s, %cx - %w, %cy + %q,
                %cx - %w + %t, %cy + %q, %cx + %w + %t, %cy - %s);
   // lower limb: the waist, right of centre, down to the bottom-left
   Ascend::quad(%cx + %w, %cy - %q, %cx - %w, %cy + %s,
                %cx - %w + %t, %cy + %s, %cx + %w + %t, %cy - %q);
}

// Flag: a pole with a pennant. The pennant leans right for the friendly side
// and left for the enemy's, which is how the reference mirrors them.
function Ascend::glyphFlag(%x, %y, %h, %dir, %rgb, %alpha)
{
   Ascend::color(%rgb, %alpha);
   glRectangle(%x, %y, 2, %h);
   %w = floor(%h * 0.62);
   %f = floor(%h * 0.48);
   Ascend::quad(%x + 2, %y, %x + 2 + %dir * %w, %y + floor(%f / 2),
                   %x + 2, %y + %f, %x + 2, %y + %f);
}

// Clock: an octagonal ring with two hands. A circle at 16px is an octagon.
function Ascend::glyphClock(%cx, %cy, %r, %rgb, %alpha)
{
   Ascend::hexFill(%cx, %cy, %r, %r, %rgb, %alpha);
   Ascend::hexFill(%cx, %cy, %r - 2, %r - 3, $Ascend::Plate, 255);
   Ascend::color(%rgb, %alpha);
   glRectangle(%cx - 1, %cy - %r + 3, 2, %r - 2);        // long hand, up
   glRectangle(%cx - 1, %cy - 1, floor(%r * 0.6), 2);    // short hand, right
}

// Health cross -- the "+" beside the health number in the reference.
function Ascend::glyphCross(%cx, %cy, %s, %rgb, %alpha)
{
   Ascend::color(%rgb, %alpha);
   glRectangle(%cx - %s, %cy - 2, %s * 2, 4);
   glRectangle(%cx - 2, %cy - %s, 4, %s * 2);
}

// Grenade: a chamfered body with a cap and a spoon. Reads at 20px, which is all
// it has to do.
function Ascend::glyphGrenade(%cx, %cy, %s, %rgb, %alpha)
{
   Ascend::color(%rgb, %alpha);
   %b = %s;
   glRectangle(%cx - %b, %cy - %b + 2, %b * 2, %b * 2 - 2);
   Ascend::quad(%cx - %b + 2, %cy - %b, %cx + %b - 2, %cy - %b,
                   %cx + %b, %cy - %b + 2, %cx - %b, %cy - %b + 2);
   glRectangle(%cx - 2, %cy - %b - 3, 4, 3);             // cap
   glRectangle(%cx + 2, %cy - %b - 3, %b - 1, 2);        // spoon
}

// Backpack: a rounded box with two straps -- the second hex in the reference.
function Ascend::glyphPack(%cx, %cy, %s, %rgb, %alpha)
{
   Ascend::color(%rgb, %alpha);
   glRectangle(%cx - %s, %cy - %s + 2, %s * 2, %s * 2 - 2);
   Ascend::quad(%cx - %s + 2, %cy - %s, %cx + %s - 2, %cy - %s,
                   %cx + %s, %cy - %s + 2, %cx - %s, %cy - %s + 2);
   Ascend::color($Ascend::Plate, %alpha);
   glRectangle(%cx - %s + 3, %cy - %s + 4, 2, %s * 2 - 7);
   glRectangle(%cx + %s - 5, %cy - %s + 4, 2, %s * 2 - 7);
}

//==============================================================================
// PART: the top strip -- flags, scores, clock
//
// Part box 760x76. Everything is laid out symmetrically about the box centre so
// the strip stays centred at any resolution and any part scale.
//==============================================================================
function Ascend::TopBar(%x, %y, %w)
{
   %mine   = Team::Friendly();
   %theirs = Team::Enemy();

   %cx = %x + floor(%w / 2);
   %top = %y + 14;              // strip top edge inside the part box
   %h   = 34;
   %mid = %top + floor(%h / 2);

   // -- the clock capsule, dead centre ---------------------------------------
   // getHudTimer() is cg.clockTime, the same continuously-advanced client clock
   // ClockHud draws -- authoritative immediately after a config swap, where the
   // legacy eventUpdateTime bridge only refreshes at :00/:20/:40.
   %t = getHudTimer();
   if(%t == "") %t = 0;
   if(%t < 0)   %t = -%t;
   %min = floor(%t / 60);
   %sec = floor(%t - %min * 60);
   %clockStr = Ascend::pad2(%min) @ ":" @ Ascend::pad2(%sec);

   %cw = 132;
   %clx = %cx - floor(%cw / 2);
   Ascend::plate(%clx, %top, %cw, %h, "both", $Ascend::Plate, 205,
                 $Ascend::Edge, 190);
   Ascend::glyphClock(%clx + 20, %mid, 9, $Ascend::Primary, 235);
   Ascend::tt(%clx + 34, %top + 5, %cw - 44, $Ascend::Bright, %clockStr, 250,
              24, "c");

   // -- score plates, pointing away from the clock ---------------------------
   %sw = 66;
   %gap = 4;
   Ascend::sideBlock(%clx - %gap - %sw, %top, %sw, %h, %mine, -1,
                     $Ascend::Primary, $Ascend::Dim);
   Ascend::sideBlock(%clx + %cw + %gap, %top, %sw, %h, %theirs, 1,
                     $Ascend::Enemy, $Ascend::EnemyDim);

   // -- the two guide rules, fading outward ----------------------------------
   // ★Compare against QUOTED strings, never a bare 0.★ compare() promotes the
   // whole comparison to float the moment either side is a numeric literal, and
   // evalFloat("") is 0 -- so `%rules == 0` is TRUE for a pref that was never
   // set, and the rules would be off for anyone who had not touched the row.
   // This is the framework's own bool test (ModernHUD::stock).
   %rules = $pref::Ascend::Rules;
   if(%rules != "0" && %rules != "false")
   {
      %edgeL = %clx - %gap - %sw - 92;    // outer edge of the friendly block
      %edgeR = %clx + %cw + %gap + %sw + 92;
      Ascend::rule(%x, %mid, %edgeL - %x - 10, -1, $Ascend::Primary, 130);
      Ascend::rule(%edgeR + 10, %mid, %x + %w - %edgeR - 10, 1,
                   $Ascend::Primary, 130);
   }
}

// One side of the strip: score plate, flag glyph, and the outer pip.
// %dir -1 is the left (friendly) side, 1 the right (enemy) side.
function Ascend::sideBlock(%x, %y, %w, %h, %team, %dir, %rgb, %dimRgb)
{
   %score = Team::Score(%team);
   if(%score == "") %score = 0;

   %style = (%dir < 0) ? "left" : "right";
   Ascend::plate(%x, %y, %w, %h, %style, $Ascend::Plate, 205, %rgb, 200);
   Ascend::tt(%x + 6, %y + 5, %w - 12, $Ascend::Text, %score, 250, 22, "c");

   // Flag glyph, outboard of the score.
   %loc = Team::Flag::Location(%team);
   if(%loc == "home")       { %fc = %rgb;             %fa = 235; }
   else if(%loc == "field") { %fc = $Ascend::Accent;  %fa = 255; }
   else                     { %fc = $Ascend::Bright;  %fa = 255; }

   // The pennant leans AWAY from the strip centre, so it mirrors like the
   // reference does. Written as a branch rather than `-%dir`: a unary minus on a
   // variable inside an argument list is one more thing this parser has to get
   // right, and a parse error here would take the rest of the file with it.
   %fdir = 1;
   if(%dir > 0) %fdir = -1;

   %fx = (%dir < 0) ? (%x - 26) : (%x + %w + 20);
   Ascend::glyphFlag(%fx, %y + 8, 20, %fdir, %fc, %fa);

   // -- the outer pip --------------------------------------------------------
   // ★Ascend's generator light, carrying the only comparable Tribes datum.★
   // There is no generator state on this wire, so the pip reports the flag:
   // idle when home, amber with the return countdown when it is lying in the
   // field, lit when somebody is carrying it.
   %px = (%dir < 0) ? (%x - 62) : (%x + %w + 56);
   %py = %y + floor(%h / 2);

   if(%loc == "home")
   {
      Ascend::hexRing(%px, %py, 15, 19, %dimRgb, 220, $Ascend::Plate, 215, 2);
      Ascend::glyphBolt(%px, %py, 8, %rgb, 150);
   }
   else if(%loc == "field")
   {
      Ascend::hexRing(%px, %py, 15, 19, $Ascend::Accent, 245, $Ascend::Plate, 225, 2);
      %left = Team::Flag::Timer(%team);
      if(%left != "")
         Ascend::tt(%px - 15, %py - 7, 30, $Ascend::Accent, %left, 250, 12, "c");
      else
         Ascend::glyphBolt(%px, %py, 8, $Ascend::Accent, 245);
   }
   else
   {
      // Carried. Pulse the pip so a grab is visible without reading anything.
      // A triangle wave off glTicks (a real ms wall clock), so the rate is
      // frame-rate independent: 190..250 and back over 700ms.
      %ms = glTicks();
      %ph = %ms - floor(%ms / 700) * 700;
      if(%ph > 350) %ph = 700 - %ph;
      %pulse = 190 + floor(60 * %ph / 350);

      Ascend::hexRing(%px, %py, 15, 19, $Ascend::Bright, %pulse, $Ascend::Plate, 225, 2);
      Ascend::glyphBolt(%px, %py, 8, $Ascend::Bright, 255);

      // Who has it. Player names come off the network and can contain markup,
      // so this goes through escapeFormatting before it reaches a draw.
      %who = String::escapeFormatting(Client::GetName(%loc));
      if(%who != "")
         Ascend::tt(%px - 90, %py + 22, 180, $Ascend::Bright, %who, 225, 12, "c");
   }
}

//==============================================================================
// PART: the bottom-left cluster -- items, health, energy
//
// Part box 430x104: two item hexagons on the left with a key cap over the
// first, and the two segmented bars with their numbers to the right.
//==============================================================================
function Ascend::Vitals(%x, %y, %w)
{
   %health = $health;
   %energy = $energy;
   if(%health == "") %health = 0;
   if(%energy == "") %energy = 0;

   %flash = $damageFlash;
   if(%flash == "") %flash = 0;

   // ★Every colour/alpha choice is hoisted into a local before the call.★ A
   // ternary is a valid expression here, but this parser is 1998 and a syntax
   // error is not reported per-statement -- it aborts the REST OF THE FILE,
   // taking every function defined below it with it. Nothing in a draw path is
   // worth that risk for one saved line.
   //
   // -- grenade hexagon, with its key cap ------------------------------------
   %gren = getItemCount("Grenade");
   if(%gren == "") %gren = 0;

   %gcx = %x + 34;
   %gcy = %y + 44;

   if(%gren > 0) { %gc = $Ascend::Primary; %ga = 240; %gia = 250; %gta = 245; }
   else          { %gc = $Ascend::Dim;     %ga = 200; %gia = 150; %gta = 160; }

   Ascend::hexRing(%gcx, %gcy, 25, 28, %gc, %ga, $Ascend::Plate, 215, 3);
   Ascend::glyphGrenade(%gcx, %gcy + 1, 9, %gc, %gia);
   Ascend::tt(%gcx + 29, %gcy - 8, 44, $Ascend::Text, "x" @ %gren, %gta, 16, "l");

   // The key cap. There is no script accessor for a live binding, so the label
   // is a pref -- default "G", which is what sae.cs binds throwRelease to.
   %key = $pref::Ascend::GrenadeKey;
   if(%key == "") %key = "G";
   Ascend::plate(%gcx - 9, %y, 18, 14, "flat", $Ascend::Plate, 215,
                 $Ascend::Edge, 185);
   Ascend::tt(%gcx - 9, %y + 1, 18, $Ascend::Text, %key, 230, 10, "c");

   // -- pack hexagon ---------------------------------------------------------
   // Which pack you are carrying is a scan over the pack item names, so it is
   // cached; the COUNT beside it is read live.
   %pcx = %x + 20;
   %pcy = %y + 86;
   %pack = Ascend::packCarried();

   if(%pack != "") { %pc = $Ascend::Primary; %pa = 235; %pia = 245; }
   else            { %pc = $Ascend::Dim;     %pa = 195; %pia = 150; }

   Ascend::hexRing(%pcx, %pcy, 15, 17, %pc, %pa, $Ascend::Plate, 215, 3);
   Ascend::glyphPack(%pcx, %pcy, 7, %pc, %pia);

   %kit = getItemCount("Repair Kit");
   if(%kit == "") %kit = 0;
   // Dim at zero, but not INVISIBLE at zero: over bright terrain the first cut's
   // 165-alpha Dim was unreadable, and "I have none" is information the player
   // still has to be able to read.
   if(%kit > 0) { %kc = $Ascend::Text;    %ka = 240; }
   else         { %kc = $Ascend::Primary; %ka = 175; }
   // Clear of the hexagon: %pcx + %rw + padding, not %pcx + a guessed offset --
   // the first render had this label sitting on top of the hex.
   Ascend::tt(%pcx + 24, %pcy - 7, 70, %kc, "KIT " @ %kit, %ka, 12, "l");

   // -- the bars -------------------------------------------------------------
   %bx = %x + 124;
   %bw = 196;

   // ★Flash white on the frame the server says we were hit.★ $damageFlash is the
   // live wire value (0 clean, up to 0.76 under fire). Watching $health drop
   // instead misses chip damage absorbed by armour and is a frame late.
   if(%flash > 0.02)     %hc = "255 255 255";
   else if(%health > 66) %hc = $Ascend::Primary;
   else if(%health > 33) %hc = $Ascend::Accent;
   else                  %hc = $Ascend::Warn;

   Ascend::segbar(%bx, %y + 34, %bw, 18, 14, %health / 100, %hc, 240);
   Ascend::segbar(%bx, %y + 62, %bw, 14, 12, %energy / 100, $Ascend::Primary, 215);

   // -- the numbers, right of the bars, each with its glyph ------------------
   // The x15 is a COSMETIC scale on the display only -- the row that turns it on
   // says so. The underlying reading is still 0..100.
   %shown = %health;
   %hs = $pref::Ascend::Health;
   if(%hs != "" && %hs == 1)
      %shown = floor(%health * 15);

   %nx = %bx + %bw + 12;
   Ascend::tt(%nx, %y + 32, 58, $Ascend::Text, floor(%shown), 250, 20, "r");
   Ascend::glyphCross(%nx + 72, %y + 43, 6, %hc, 240);

   Ascend::tt(%nx, %y + 60, 58, $Ascend::Text, floor(%energy), 235, 16, "r");
   Ascend::glyphBolt(%nx + 72, %y + 69, 7, $Ascend::Primary, 235);
}

//------------------------------------------------------------------------------
// Which backpack the player is carrying, or "".
//
// ★Rescanned on a timer, not per frame.★ It is one getItemCount per pack type,
// and a pack only changes at an inventory station -- so a 400ms cache turns a
// dozen native calls per frame into a dozen every twenty-fifth frame, which is
// the difference between "arithmetic and draw calls" and walking a table in the
// render path.
//------------------------------------------------------------------------------
function Ascend::packCarried()
{
   %now = glTicks();
   if($Ascend::PackAt != "" && %now >= $Ascend::PackAt && %now - $Ascend::PackAt < 400)
      return $Ascend::PackName;

   $Ascend::PackAt = %now;
   $Ascend::PackName = "";

   for(%i = 0; %i < $Ascend::PackCount; %i++)
   {
      if(getItemCount($Ascend::PackItem[%i]) > 0)
      {
         $Ascend::PackName = $Ascend::PackItem[%i];
         return $Ascend::PackName;
      }
   }
   return "";
}

//==============================================================================
// PART: the weapon tray
//
// Part box 576x78. One plate per carried weapon: a numbered tab above, the
// weapon name, and the ammo count. The mounted weapon is lit.
//
// ★The tab number is the key that selects it★ (sae.cs binds 1..8 to
// use("<weapon>")), not an ordinal -- which makes the tray an instruction
// rather than a list. Ascend shows three because Ascend's loadout is three;
// Tribes carries as many as the armour allows, so the plates share the box.
//==============================================================================
function Ascend::Weapons(%x, %y, %w)
{
   %mounted = GetItemDesc(GetMountedItem(0));

   // Which weapons are carried. Same cache argument as the backpack scan: the
   // set changes at a station, the AMMO does not, and ammo is read live below.
   %now = glTicks();
   if($Ascend::WepAt == "" || %now < $Ascend::WepAt || %now - $Ascend::WepAt >= 300)
   {
      $Ascend::WepAt = %now;
      %n = 0;
      for(%i = 0; %i < $Ascend::WepCount; %i++)
      {
         if(getItemCount($Ascend::WepName[%i]) > 0)
         {
            $Ascend::OwnIdx[%n] = %i;
            %n++;
         }
      }
      $Ascend::OwnCount = %n;
   }

   %n = $Ascend::OwnCount;
   if(%n == "" || %n <= 0)
      return;

   %gap = 8;
   %pw = floor((%w - (%n - 1) * %gap) / %n);
   if(%pw > 176) %pw = 176;
   if(%pw < 64)  %pw = 64;

   %total = %n * %pw + (%n - 1) * %gap;
   %px = %x + floor((%w - %total) / 2);
   %py = %y + 22;               // the tab lives in the 22px above the plate
   %ph = 52;

   for(%s = 0; %s < %n; %s++)
   {
      %i = $Ascend::OwnIdx[%s];

      // Hoisted, and reduced to 1/0. A comparison EXPRESSION in an argument
      // list is one more thing to get right in a parser whose syntax errors
      // abort the rest of the file, and a comparison here yields the strings
      // "True"/"False" rather than a number.
      %act = 0;
      if($Ascend::WepName[%i] == %mounted)
         %act = 1;

      Ascend::weaponPlate(%px, %py, %pw, %ph, %i, %act);
      %px = %px + %pw + %gap;
   }
}

function Ascend::weaponPlate(%x, %y, %w, %h, %i, %active)
{
   %ammoItem = $Ascend::WepAmmo[%i];

   if(%active)
   {
      %edge = $Ascend::Bright; %ea = 250; %fillA = 230; %ta = 255;
      %label = $Ascend::Bright;
   }
   else
   {
      %edge = $Ascend::Edge;   %ea = 165; %fillA = 180; %ta = 205;
      %label = $Ascend::Text;
   }

   // The numbered tab: a trapezoid seated on the plate's top edge.
   %tw = 30;
   %tx = %x + floor((%w - %tw) / 2);
   Ascend::color($Ascend::Plate, %fillA);
   Ascend::quad(%tx + 5, %y - 16, %tx + %tw - 5, %y - 16,
                   %tx + %tw, %y, %tx, %y);
   Ascend::color(%edge, %ea);
   glRectangle(%tx + 5, %y - 16, %tw - 10, 1);
   Ascend::tt(%tx, %y - 15, %tw, %label, $Ascend::WepKey[%i], %ta, 12, "c");

   // The plate.
   Ascend::plate(%x, %y, %w, %h, "both", $Ascend::Plate, %fillA, %edge, %ea);
   if(%active)
   {
      // A lit weapon gets a wash of the team colour, so which one is up reads
      // from the shape of the tray rather than from reading the names.
      Ascend::color($Ascend::Primary, 46);
      glRectangle(%x, %y + 6, %w, %h - 7);
   }

   // ★The label is precomputed, not String::toUpper'd here.★ This runs once per
   // weapon per frame; the uppercase form never changes, so it is built once in
   // Ascend::tables(). (It also keeps the draw path off a String:: call, three
   // of which Presto shadows with script reimplementations at runtime.)
   //
   // ★And it has a short form.★ The plates share a fixed box, so a heavy armour
   // carrying all eight weapons gets ~65px each -- and "GRENADE LAUNCHER" at
   // 12px is 95px, which does not overflow harmlessly: an immediate-mode draw
   // has no clip box, so it would run straight over the neighbouring plate.
   %nm = $Ascend::WepLabel[%i];
   %npx = 12;
   if(%w < 130)
   {
      %nm = $Ascend::WepShort[%i];
      %npx = 11;
   }
   Ascend::tt(%x + 4, %y + 6, %w - 8, %label, %nm, %ta, %npx, "c");

   // -- ammo -----------------------------------------------------------------
   // ★"" is not the empty case for the mounted weapon -- 0 and -1 are.★
   // $Weapon::Ammo always writes an integer: -1 nothing mounted, 0 mounted with
   // no ammo type. For the weapons that are NOT mounted there is no such export,
   // so the count comes from the ammo ITEM, which is the same number the stock
   // weapon bar draws (CurWeapHud.cpp reads imageData->ammoType through
   // psc->itemCount).
   if(%ammoItem == "")
   {
      // Blaster, Laser Rifle and ELF draw from suit energy and have no ammo
      // datablock. Show the energy they actually spend, with its glyph, rather
      // than a counter that is always zero.
      %e = $energy;
      if(%e == "") %e = 0;
      Ascend::tt(%x + 4, %y + 22, %w - 20, $Ascend::Primary, floor(%e),
                 %ta, 22, "c");
      Ascend::glyphBolt(%x + %w - 14, %y + 34, 7, $Ascend::Primary, %ta);
      return;
   }

   %ammo = getItemCount(%ammoItem);
   if(%ammo == "") %ammo = 0;

   if(%ammo <= 0)     %ac = $Ascend::Warn;
   else if(%ammo <= 3) %ac = $Ascend::Accent;
   else                %ac = $Ascend::Text;

   Ascend::tt(%x + 4, %y + 22, %w - 8, %ac, %ammo, %ta, 22, "c");
}

//==============================================================================
// The reticle -- two facing brackets around a diamond.
//
// ★Not a movable part, and that is the point.★ Every other part goes through
// ModernHUD::part, which creates a retained handle so it can be dragged. A
// reticle that can be dragged off the aim point is a broken reticle, and a
// dragged one cannot be recovered by "reset positions" either. This uses
// ModernHUD::place -- the anchor maths without the handle -- so it is
// recomputed to dead screen centre every frame, at every resolution.
//
// The aim pixel itself is left unoccluded: the diamond is an outline.
//==============================================================================
function Ascend::Reticle(%screen)
{
   %style = $pref::Ascend::Reticle;
   if(%style == "") %style = 1;
   if(%style == 0)
      return;

   // ★glPartScale persists to the END of the ScriptGL pass.★ This draw does not
   // go through ModernHUD::part, so whatever scale the last part pushed would
   // still be active and would move the reticle off the aim point.
   glPartScale(0, 0, 1);

   // Content size 0x0, so this is exactly screenW/2, screenH/2 -- the aim point,
   // not a box centred near it.
   %at = ModernHUD::place("center", 0, 0, 0, 0, %screen);
   %cx = getWord(%at, 0);
   %cy = getWord(%at, 1);

   %k = $pref::Ascend::Scale;
   if(%k == "" || %k <= 0) %k = 100;
   if(%k < 50)  %k = 50;
   if(%k > 300) %k = 300;
   %k = %k / 100;

   %savedA = $Ascend::A;
   %ro = $pref::Ascend::ReticleOpacity;
   if(%ro == "" || %ro <= 0) %ro = 100;
   if(%ro > 100) %ro = 100;
   $Ascend::A = %ro / 100;

   %gap = floor(24 * %k);       // the aim-point gap: where the arms STOP
   %arm = floor(18 * %k);       // how far each bracket reaches outward
   %hh  = floor(14 * %k);       // half height of a bracket
   %t   = floor(3 * %k);
   if(%t < 2) %t = 2;

   %rgb = $Ascend::Primary;
   if(%style == 3)
      %rgb = $Ascend::Bright;

   // -- the brackets ---------------------------------------------------------
   if(%style != 3)
   {
      Ascend::bracket(%cx - %gap, %cy, %arm, %hh, %t, 1, %rgb);
      Ascend::bracket(%cx + %gap, %cy, %arm, %hh, %t, -1, %rgb);
   }

   // -- the centre diamond, as an OUTLINE ------------------------------------
   // Four leaning strokes rather than a filled rhombus, so the pixel you are
   // actually shooting at is not painted over by your own HUD.
   %d = floor(10 * %k);
   if(%d < 5) %d = 5;
   Ascend::color(%rgb, 245);
   Ascend::quad(%cx, %cy - %d, %cx + %t, %cy - %d + %t,
                   %cx, %cy - %d + %t * 2, %cx - %t, %cy - %d + %t);
   Ascend::quad(%cx, %cy + %d, %cx + %t, %cy + %d - %t,
                   %cx, %cy + %d - %t * 2, %cx - %t, %cy + %d - %t);
   Ascend::quad(%cx - %d, %cy, %cx - %d + %t, %cy + %t,
                   %cx - %d + %t * 2, %cy, %cx - %d + %t, %cy - %t);
   Ascend::quad(%cx + %d, %cy, %cx + %d - %t, %cy + %t,
                   %cx + %d - %t * 2, %cy, %cx + %d - %t, %cy - %t);

   // ★Style 1 is the reference and it DOES cross the aim point.★ The screenshot
   // has a small X inside the diamond, so a faithful recreation draws it -- and
   // style 2 is the same reticle with that X removed, for anyone who would
   // rather see the pixel than the shape. Offering both is the honest way to
   // copy a design decision you might not agree with.
   if(%style == 1)
   {
      // ★The X is drawn THINNER than the diamond, not at the same %t.★ At the
      // diamond's own 3px stroke the two crossing arms merge into a solid blob
      // at this size, which is the opposite of a precision aim mark.
      %e = floor(%d * 0.54);
      if(%e < 3) %e = 3;
      %xt = floor(2 * %k);
      if(%xt < 2) %xt = 2;
      Ascend::color($Ascend::Bright, 250);
      Ascend::quad(%cx - %e, %cy - %e, %cx - %e + %xt, %cy - %e,
                   %cx + %e, %cy + %e, %cx + %e - %xt, %cy + %e);
      Ascend::quad(%cx + %e, %cy - %e, %cx + %e - %xt, %cy - %e,
                   %cx - %e, %cy + %e, %cx - %e + %xt, %cy + %e);
   }

   $Ascend::A = %savedA;
}

// One bracket: an outer vertical stem with two arms running INWARD toward the
// aim point and converging as they go, which is the shape in the reference.
// They stop at %xin -- the aim-point gap -- so the centre stays clear.
//
// %dir 1 is the left bracket (its stem is to the LEFT of %xin), -1 the right.
function Ascend::bracket(%xin, %cy, %arm, %hh, %t, %dir, %rgb)
{
   Ascend::color(%rgb, 235);

   %xout = %xin - %dir * %arm;
   %lift = floor(%hh * 0.45);
   if(%lift < 2) %lift = 2;

   // The outer stem, drawn on the far side of %xout so the bracket's total
   // reach is exactly %arm + %t on both sides.
   %sx = %xout;
   if(%dir > 0) %sx = %xout - %t;
   glRectangle(%sx, %cy - %hh, %t, %hh * 2);

   // Top arm: from the stem's top corner in toward the aim line.
   Ascend::quad(%xout, %cy - %hh, %xin, %cy - %hh + %lift,
                   %xin, %cy - %hh + %lift + %t, %xout, %cy - %hh + %t);

   // Bottom arm, mirrored.
   Ascend::quad(%xout, %cy + %hh - %t, %xin, %cy + %hh - %lift - %t,
                   %xin, %cy + %hh - %lift, %xout, %cy + %hh);
}

//==============================================================================
// THE WEAPON AND PACK TABLES
//
// ★Read out of base/scripts/item.cs, not from memory.★ The pairing is
// ItemData.imageType -> ItemImage.ammoType -> that ammo item's description,
// which is the chain CurWeapHud.cpp walks in C++ to draw the stock bar. There is
// no script accessor for it, so the mapping is written out here:
//
//   Blaster           BlasterImage        (no ammoType -- suit energy)
//   Plasma Gun        PlasmaGunImage      ammoType = PlasmaAmmo   "Plasma Bolt"
//   Chaingun          ChaingunImage       ammoType = BulletAmmo   "Bullet"
//   Disc Launcher     DiscLauncherImage   ammoType = DiscAmmo     "Disc"
//   Grenade Launcher  GrenadeLauncherImage ammoType = GrenadeAmmo "Grenade Ammo"
//   Laser Rifle       LaserRifleImage     (no ammoType -- suit energy)
//   ELF Gun           EnergyRifleImage    (no ammoType -- suit energy)
//   Mortar            MortarImage         ammoType = MortarAmmo   "Mortar Ammo"
//
// The numbers are sae.cs's own use() binds, which is why they are not 0..7.
//
// ★Targeting Laser is deliberately absent★: item.cs:1290 sets
// showWeaponBar = false on it, which is the engine's own statement that it does
// not belong in a weapon bar. Repair Gun likewise never sets showWeaponBar.
//
// A mod with different weapons simply shows the ones it shares; getItemCount
// returns "0" for a description this client does not know, so an unknown entry
// costs a lookup and draws nothing.
//==============================================================================
function Ascend::tables()
{
   $Ascend::WepName[0] = "Blaster";           $Ascend::WepAmmo[0] = "";
   $Ascend::WepKey[0]  = "1";
   $Ascend::WepLabel[0] = "BLASTER";          $Ascend::WepShort[0] = "BLASTER";
   $Ascend::WepName[1] = "Plasma Gun";        $Ascend::WepAmmo[1] = "Plasma Bolt";
   $Ascend::WepKey[1]  = "2";
   $Ascend::WepLabel[1] = "PLASMA GUN";       $Ascend::WepShort[1] = "PLASMA";
   $Ascend::WepName[2] = "Chaingun";          $Ascend::WepAmmo[2] = "Bullet";
   $Ascend::WepKey[2]  = "3";
   $Ascend::WepLabel[2] = "CHAINGUN";         $Ascend::WepShort[2] = "CHAIN";
   $Ascend::WepName[3] = "Disc Launcher";     $Ascend::WepAmmo[3] = "Disc";
   $Ascend::WepKey[3]  = "4";
   $Ascend::WepLabel[3] = "DISC LAUNCHER";    $Ascend::WepShort[3] = "DISC";
   $Ascend::WepName[4] = "Grenade Launcher";  $Ascend::WepAmmo[4] = "Grenade Ammo";
   $Ascend::WepKey[4]  = "5";
   $Ascend::WepLabel[4] = "GRENADE LAUNCHER"; $Ascend::WepShort[4] = "GREN LAUNCH";
   $Ascend::WepName[5] = "Laser Rifle";       $Ascend::WepAmmo[5] = "";
   $Ascend::WepKey[5]  = "6";
   $Ascend::WepLabel[5] = "LASER RIFLE";      $Ascend::WepShort[5] = "LASER";
   $Ascend::WepName[6] = "ELF Gun";           $Ascend::WepAmmo[6] = "";
   $Ascend::WepKey[6]  = "7";
   $Ascend::WepLabel[6] = "ELF GUN";          $Ascend::WepShort[6] = "ELF";
   $Ascend::WepName[7] = "Mortar";            $Ascend::WepAmmo[7] = "Mortar Ammo";
   $Ascend::WepKey[7]  = "8";
   $Ascend::WepLabel[7] = "MORTAR";           $Ascend::WepShort[7] = "MORTAR";
   $Ascend::WepCount = 8;

   // The backpacks, in the order item.cs declares them. Only one can be worn,
   // so the scan stops at the first hit.
   $Ascend::PackItem[0]  = "Energy Pack";
   $Ascend::PackItem[1]  = "Repair Pack";
   $Ascend::PackItem[2]  = "Shield Pack";
   $Ascend::PackItem[3]  = "Sensor Jammer Pack";
   $Ascend::PackItem[4]  = "Ammo Pack";
   $Ascend::PackItem[5]  = "Inventory Station";
   $Ascend::PackItem[6]  = "Ammo Station";
   $Ascend::PackItem[7]  = "Motion Sensor";
   $Ascend::PackItem[8]  = "Pulse Sensor";
   $Ascend::PackItem[9]  = "Sensor Jammer";
   $Ascend::PackItem[10] = "Camera";
   $Ascend::PackItem[11] = "Turret";
   $Ascend::PackCount = 12;
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
   // Ascend puts a name and a health bar over every player it can see, so this
   // pack turns the same client features on. NOT crouch-gated: $mj::barscrouch
   // means "only while crouching", and a bar you only get when the enemy
   // crouches is a bar you never get.
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

   Ascend::crosshair();
}

// The reticle setting drives the stock crosshair ART, never the crosshairHud
// CONTROL: FearGui::Crosshair::onRender also draws the whole nameplate system,
// so hiding the control to remove the reticle takes names, bars, the pass
// helper and target acquisition with it.
function Ascend::crosshair()
{
   %style = $pref::Ascend::Reticle;
   if(%style == "") %style = 1;
   $pref::hideCrosshairArt = (%style == 0) ? "0" : "1";
}

function Ascend::restore()
{
   if($Ascend::Saved == "")
   {
      echo("Ascend: nothing to restore.");
      return;
   }

   $pref::Hud::ColorPrimary = $Ascend::Sav::ColorPrimary;
   $pref::Hud::ColorDim     = $Ascend::Sav::ColorDim;
   $pref::Hud::ColorAccent  = $Ascend::Sav::ColorAccent;
   $pref::Hud::ColorWarn    = $Ascend::Sav::ColorWarn;
   $pref::Hud::ColorText    = $Ascend::Sav::ColorText;
   $pref::Hud::ColorPass    = $Ascend::Sav::ColorPass;

   $mj::shownames        = $Ascend::Sav::ShowNames;
   $mj::showhpbars       = $Ascend::Sav::ShowHpBars;
   $mj::showjetbars      = $Ascend::Sav::ShowJetBars;
   $mj::showhptext       = $Ascend::Sav::ShowHpText;
   $mj::barscrouch       = $Ascend::Sav::BarsCrouch;
   $mj::bar_width        = $Ascend::Sav::BarW;
   $mj::bar_height       = $Ascend::Sav::BarH;
   $mj::bar_border_width = $Ascend::Sav::BarB;
   $mj::passhelper       = $Ascend::Sav::PassHelper;
   $mj::passhelpermm     = $Ascend::Sav::PassHelperMM;

   $pref::hideCrosshairArt = $Ascend::Sav::HideXArt;

   $xChat::HiderEnabled  = $Ascend::Sav::HiderEnabled;
   $xChat::HiderTimeout  = $Ascend::Sav::HiderTimeout;
   $xChat::ScrollTimeout = $Ascend::Sav::ScrollTimeout;
   $xChat::HideCmdMsg    = $Ascend::Sav::HideCmdMsg;
   $xChat::TransChat     = $Ascend::Sav::TransChat;

   $pref::ChatDisplayModMethodX = $Ascend::Sav::ChatModX;
   $pref::ChatDisplayModMethodY = $Ascend::Sav::ChatModY;
   $pref::ChatDisplayX          = $Ascend::Sav::ChatX;
   $pref::ChatDisplayY          = $Ascend::Sav::ChatY;
   $pref::ChatDisplayWidth      = $Ascend::Sav::ChatWidth;

   $pref::ChatInputModMethodY   = $Ascend::Sav::ChatInModY;
   $pref::ChatInputY            = $Ascend::Sav::ChatInY;

   deleteVariables("$Ascend::Sav::*");
   $Ascend::Saved = "";
   echo("Ascend: client settings restored.");
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

//------------------------------------------------------------------------------
// The font list, built from what is ACTUALLY INSTALLED.
//
// ★CreateFontA never fails on an unknown family -- it SUBSTITUTES.★ So a
// hard-coded list would offer faces this machine does not have and quietly
// render as something else. glFontExists asks Windows; only the faces that
// answer are offered, which makes the list honest on every machine instead of
// correct on mine.
//
// Curated, not enumerated: a raw enumeration is 200+ entries including symbol
// and script faces. These are the condensed geometric grotesques that carry
// Ascend's look, best first, so bestFont() picks the closest one present.
//------------------------------------------------------------------------------
function Ascend::fontScan()
{
   if($Ascend::FontCount != "")
      return;

   %cand = "Bahnschrift Condensed;Bahnschrift;Agency FB;Eurostile;" @
           "Franklin Gothic Medium;Arial Narrow;Segoe UI Semibold;Segoe UI;" @
           "Trebuchet MS;Tahoma;Verdana;Calibri;Candara;Corbel;" @
           "Century Gothic;Arial;Impact;Microsoft Sans Serif;Consolas";

   %n = 0;
   %cur = "";
   %len = String::Length(%cand);

   for(%i = 0; %i <= %len; %i++)
   {
      %c = String::getSubStr(%cand, %i, 1);
      if(%c == ";" || %i == %len)
      {
         if(%cur != "" && glFontExists(%cur) == 1)
         {
            $Ascend::FontName[%n] = %cur;
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
      $Ascend::FontName[0] = "Verdana";
      %n = 1;
   }
   $Ascend::FontCount = %n;
   echo("Ascend: " @ %n @ " HUD fonts available");

   // The enum spec for the font row, from the SAME scanned list -- so the K menu
   // and the Options page offer one set of faces rather than two that disagree.
   %spec = "";
   for(%f = 0; %f < %n; %f++)
   {
      if(%f > 0) %spec = %spec @ ";";
      %spec = %spec @ $Ascend::FontName[%f] @ "|" @ $Ascend::FontName[%f];
   }
   $Ascend::FontSpec = %spec;
}


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
   "Real (0-100)|0;Ascend scale (x15)|1", "");

ModernHUD::setting("bool", "pref::Ascend::Rules", "Top bar guide lines", "1",
   "", "");

// There is no script accessor for a live key binding, so the cap over the
// grenade hex is a LABEL, not a reading. "G" is what base/scripts/sae.cs:89
// binds throwRelease("Grenade") to; a player who rebound it picks their key
// here.
ModernHUD::setting("enum", "pref::Ascend::GrenadeKey", "Grenade key cap", "G",
   "G|G;F|F;Q|Q;E|E;R|R;C|C;V|V;X|X;Z|Z;B|B", "");

// Any installed TrueType face, rasterized fresh at each size -- which is what
// keeps the HUD sharp instead of pixellating. The spec is SCANNED, so no row can
// offer a font that would silently fall back to something else, and the DEFAULT
// is the first survivor of a list ordered by closeness to Ascend's own face.
ModernHUD::setting("enum", "pref::Ascend::Font", "HUD font",
   $Ascend::FontName[0], $Ascend::FontSpec, "");

//==============================================================================
// DRAW DISPATCH
//==============================================================================
function ModernHUDPack::draw_topbar(%screen)
{
   %partW = 760;
   %at = ModernHUD::part("ModernHUD::AscendTop", "top-center", 0, 14,
                         760, 76, %screen);
   Ascend::TopBar(getWord(%at, 0), getWord(%at, 1), %partW);
}

function ModernHUDPack::draw_vitals(%screen)
{
   %partW = 430;
   %at = ModernHUD::part("ModernHUD::AscendVitals", "bottom-left", 26, 34,
                         430, 104, %screen);
   Ascend::Vitals(getWord(%at, 0), getWord(%at, 1), %partW);
}

function ModernHUDPack::draw_weapons(%screen)
{
   %partW = 576;
   %at = ModernHUD::part("ModernHUD::AscendWeapons", "bottom-center", 0, 18,
                         576, 78, %screen);
   Ascend::Weapons(getWord(%at, 0), getWord(%at, 1), %partW);
}

function ModernHUDPack::draw(%screen)
{
   // The palette is a function of a pref, so it is resolved per frame: a theme
   // change from the K panel or the console has to land on the next frame, and
   // this is four string reads against a HUD already issuing GL calls.
   Ascend::palette();

   %mf = $pref::Ascend::Font;
   if(%mf == "") %mf = "Verdana";
   $ModernHUD::MenuFont = %mf;

   %o = $pref::Ascend::Opacity;
   if(%o == "" || %o <= 0) %o = 100;
   if(%o > 100) %o = 100;
   $Ascend::A = %o / 100;

   // The tables are seeded by init(), but a pack can be re-exec'd without it.
   if($Ascend::WepCount == "")
      Ascend::tables();

   // The reticle first, while no part scale is active. Drawn through
   // ModernHUD::place, so there is no handle to hide when a slot is borrowed --
   // it belongs to no slot.
   Ascend::Reticle(%screen);

   // The top strip answers BOTH ctf and clock, so it yields if EITHER is
   // borrowed -- otherwise the borrowed control renders underneath ours.
   if(ModernHUDPack::ownsSlot($pref::HudSlot::ctf) &&
      ModernHUDPack::ownsSlot($pref::HudSlot::clock))
      ModernHUDPack::draw_topbar(%screen);
   else
      ModernHUD::hide("ModernHUD::AscendTop");

   // Same for the bottom-left cluster: health/energy and the item hexes are one
   // plate, so replacing either yields the whole thing.
   if(ModernHUDPack::ownsSlot($pref::HudSlot::healthenergy) &&
      ModernHUDPack::ownsSlot($pref::HudSlot::items))
      ModernHUDPack::draw_vitals(%screen);
   else
      ModernHUD::hide("ModernHUD::AscendVitals");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::weapon))
      ModernHUDPack::draw_weapons(%screen);
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
