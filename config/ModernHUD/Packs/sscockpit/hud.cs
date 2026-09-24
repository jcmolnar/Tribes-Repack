//------------------------------------------------------------------------------
// Starsiege Cockpit -- ModernHUD pack (hand-authored), forked from mechcockpit.
//
// The 1999 Starsiege HERC cockpit rebuilt from the ORIGINAL art (shell.vol, via
// the converted corpus -> Assets/Packs/sscockpit) plus vector drawing for what
// Starsiege's engine drew procedurally. Geometry, colours and positions follow
// STARSIEGE-HUD-RE-2026-09-21.md (Ghidra + pixel-matched screenshots):
//   top-left      dam_self ring + own HERC damage display + "STATUS"
//   top-right     dam_tar ring + target damage display + target name
//   centre-left   weapon table (index | abbrev | 3 fire-group dots | charge bar)
//                 with the "1 2 3" group header, then equipment lines
//   centre        retical (bright / dim / +lock box), red corner brackets on the
//                 target's projected bbox, lead marker + "%dm" range on the target
//   bottom-left   shields ring art, bright inner strength arc, yellow needle,
//                 "SHIELDS:" / "n%"
//   bottom-right  radar_high (frame 2 = opaque) ring, energy arc over the baked
//                 amber track, heading line, blips, "ACTIVE %dm", "%dKph",
//                 "ENERGY:" / "n%"
// Art RGB (authoritative, embedded PBMP palettes): bright 30,172,0  dim 26,146,0
// panel fill 24,48,63  energy track 66,54,4. Runtime yellow ~255,181,0, red ~191,0,0.
//
// State channels are IDENTICAL to mechcockpit, so it works unchanged on Mech
// Mayhem and Herc Havoc servers: remoteMMState / remoteMMShields / remoteMMRack /
// remoteMMShake / remoteHHState (Mods\*\scripts\MechGame.cs). Draws only while a
// server is feeding those; otherwise every slot is declined.
//
// Engine side ($pref::ssHudExport, program/code/fearGuiCrosshair.cpp): while the
// cockpit is live the stock crosshair's target pass publishes $SSC::hdg (own
// heading), $SSC::contacts (sensor-visible radar contacts) and $SSC::tgt (the
// acquired target with its projected extent + damage). mSin/mCos/mAtan2 are
// kronosNativeCmds.cpp additions for this pack.
//
// Layout is Starsiege's high-res set 1:1 at 1920x1080, corner-anchored; here it
// is scaled by screen height so 1080p is exact and other heights keep the look.
// $pref::ssHudDemo = 1 forces the cockpit live with placeholder data on any
// server (layout work / verification without a mech mod).
//
// NOT YET (needs engine work, see the RE report 10.1): the 3D HERC models inside
// the two damage rings (a stylised glyph stands in), per-part damage, the
// weapon-aim-point reticle (Tribes aims down the view axis, so centre is right
// here), the nav marker.
//------------------------------------------------------------------------------

exec("ModernHUD/Framework.cs");

$ModernHUD::Enabled = true;
$ModernHUD::Pack = "SSCockpit";
$ModernHUD::PackId = "sscockpit";

if($pref::ModernHUD::FontSet::sscockpit == "")
   $pref::ModernHUD::FontSet::sscockpit = "Terminal";
if($pref::ssHudFont == "")
   $pref::ssHudFont = "Verdana";
if($pref::ssRadarRange == "")
   $pref::ssRadarRange = 800;
// damage-display model view: yaw/pitch in degrees (Starsiege framed its own herc
// from the front-left, slightly above); $pref::ssHudShape = 0 falls back to the glyph
if($pref::ssHudShape == "")
   $pref::ssHudShape = 1;
if($pref::ssHudShapeYaw == "")
   $pref::ssHudShapeYaw = 135;
if($pref::ssHudShapePitch == "")
   $pref::ssHudShapePitch = -15;
if($pref::ssHudShapeSelfTurn == "")
   $pref::ssHudShapeSelfTurn = 0;   // 1 = own model turns with your heading

$SSC::art = "Assets/Packs/sscockpit/";

//--- state channel (same wire contract as mechcockpit) -----------------------

function remoteMMState(%server, %heatPct, %shield, %shieldMax, %legs, %guns,
                       %sens, %rctr, %chassis, %cv, %t0, %t1, %down,
                       %kills, %deaths, %wave, %salv)
{
   $MMC::heat = %heatPct;
   $MMC::shield = %shield;
   $MMC::shieldMax = %shieldMax;
   $MMC::legs = %legs;
   $MMC::guns = %guns;
   $MMC::sens = %sens;
   $MMC::rctr = %rctr;
   $MMC::chassis = %chassis;
   $MMC::cv = %cv;
   $MMC::t0 = %t0;
   $MMC::t1 = %t1;
   $MMC::down = %down;
   $MMC::kills = %kills;
   $MMC::deaths = %deaths;
   $MMC::wave = %wave;
   $MMC::salv = %salv;
   $MMC::stamp = getSimTime();
}

function remoteMMShields(%server, %str)
{
   $MMC::shields = %str;
}

function remoteMMRack(%server, %n, %w0, %w1, %w2, %w3, %w4, %w5)
{
   $MMC::rackN = %n;
   $MMC::rack[0] = %w0;
   $MMC::rack[1] = %w1;
   $MMC::rack[2] = %w2;
   $MMC::rack[3] = %w3;
   $MMC::rack[4] = %w4;
   $MMC::rack[5] = %w5;
}

function remoteMMShake(%server, %amp)
{
   if (%amp > $MM::camShake)
      $MM::camShake = %amp;
}

function remoteHHState(%server, %energy, %rsv, %c0, %c1, %c2, %c3, %pack,
                       %mine, %mines, %mode, %f0, %f1)
{
   $HHC::energy = %energy;
   $HHC::rsv = %rsv;
   $HHC::c0 = %c0;
   $HHC::c1 = %c1;
   $HHC::c2 = %c2;
   $HHC::c3 = %c3;
   $HHC::pack = %pack;
   $HHC::mine = %mine;
   $HHC::mines = %mines;
   $HHC::mode = %mode;
   $HHC::f0 = %f0;
   $HHC::f1 = %f1;
   $HHC::stamp = getSimTime();
}

// Placeholder data: the player's $pref::ssHudDemo, or the Options HUD designer drawing its
// demo preview ($ModernHUD::PvDemo, Framework.cs previewBegin) -- so the cockpit shows on the
// preview without a mech mod.
function SSC::demo()
{
   if ($pref::ssHudDemo == 1)
      return true;
   return ($ModernHUD::PvDemo == 1);
}

function SSC::active()
{
   if (SSC::demo())
      return true;
   if ($MMC::stamp == "")
      return false;
   return (getSimTime() - $MMC::stamp) < 3;
}

function SSC::herc()
{
   if ($HHC::stamp == "")
      return false;
   return (getSimTime() - $HHC::stamp) < 3;
}

//--- palette (RE report 6.1) --------------------------------------------------

$SSC::green    = "30 172 0";     // bright line art + text
$SSC::dim      = "26 146 0";     // dim variants
$SSC::fill     = "24 48 63";     // panel fill
$SSC::yellow   = "255 181 0";    // needle / energy arc / lead triangle
$SSC::red      = "191 0 0";      // lock brackets, destroyed parts
$SSC::blue     = "40 120 255";   // friendly blip
$SSC::grey     = "134 139 138";  // undamaged model shading

//--- drawing helpers ----------------------------------------------------------

function SSC::hex2(%v)
{
   %v = floor(%v);
   if (%v < 0) %v = 0;
   if (%v > 255) %v = 255;
   %hi = floor(%v / 16);
   return SSC::nib(%hi) @ SSC::nib(%v - %hi * 16);
}

function SSC::nib(%n)
{
   %n = floor(%n);
   if (%n <= 9) return %n;
   if (%n == 10) return "a";
   if (%n == 11) return "b";
   if (%n == 12) return "c";
   if (%n == 13) return "d";
   if (%n == 14) return "e";
   return "f";
}

function SSC::tag(%rgb)
{
   return "<" @ SSC::hex2(getWord(%rgb, 0)) @ SSC::hex2(getWord(%rgb, 1))
        @ SSC::hex2(getWord(%rgb, 2)) @ "ff>";
}

function SSC::font(%px)
{
   // small windows scale the 10px Starsiege text below legibility; floor it
   if (%px < 9) %px = 9;
   glSetFont($pref::ssHudFont, %px);
}

function SSC::text(%x, %y, %rgb, %str, %px)
{
   SSC::font(%px);
   glDrawString(%x + 1, %y + 1, "<000000a0>" @ %str);
   glDrawString(%x, %y, SSC::tag(%rgb) @ %str);
}

// right-aligned: %x is the RIGHT edge
function SSC::textR(%x, %y, %rgb, %str, %px)
{
   SSC::font(%px);
   %w = getWord(glGetStringDimensions(%str), 0);
   glDrawString(%x - %w + 1, %y + 1, "<000000a0>" @ %str);
   glDrawString(%x - %w, %y, SSC::tag(%rgb) @ %str);
}

// centred on %x
function SSC::textC(%x, %y, %rgb, %str, %px)
{
   SSC::font(%px);
   %w = getWord(glGetStringDimensions(%str), 0);
   %x = floor(%x - %w / 2);
   glDrawString(%x + 1, %y + 1, "<000000a0>" @ %str);
   glDrawString(%x, %y, SSC::tag(%rgb) @ %str);
}

function SSC::color(%rgb, %a)
{
   glColor4ub(getWord(%rgb, 0), getWord(%rgb, 1), getWord(%rgb, 2), %a);
}

// 1px outline rectangle
function SSC::frame(%x, %y, %w, %h, %rgb, %a)
{
   SSC::color(%rgb, %a);
   glRectangle(%x, %y, %w, 1);
   glRectangle(%x, %y + %h - 1, %w, 1);
   glRectangle(%x, %y, 1, %h);
   glRectangle(%x + %w - 1, %y, 1, %h);
}

// black-keyed art draw (the Starsiege bitmaps are palette PNGs on black)
function SSC::img(%x, %y, %w, %h, %name, %alpha)
{
   glDrawImage(%x, %y, %w, %h, $SSC::art @ %name @ ".png", %alpha, "keyblack");
}

// elliptical arc of a ring. Angles in degrees, 0 = up, clockwise; %a0 < %a1.
// %rx/%ry = outer radii, %thick = ring thickness. 10-degree convex quads.
function SSC::earc(%cx, %cy, %rx, %ry, %thick, %a0, %a1, %rgb, %alpha)
{
   SSC::earc2(%cx, %cy, %rx, %ry, %rx - %thick, %ry - %thick, %a0, %a1, %rgb, %alpha);
}

// elliptical band between an outer (%rx,%ry) and inner (%rxi,%ryi) ellipse
function SSC::earc2(%cx, %cy, %rx, %ry, %rxi, %ryi, %a0, %a1, %rgb, %alpha)
{
   SSC::color(%rgb, %alpha);
   %step = 10;
   for (%a = %a0; %a < %a1; %a = %a + %step) {
      %b = %a + %step;
      if (%b > %a1) %b = %a1;
      %s0 = mSin(%a); %c0 = mCos(%a);
      %s1 = mSin(%b); %c1 = mCos(%b);
      glAngledPolygon(floor(%cx + %rxi * %s0), floor(%cy - %ryi * %c0),
                      floor(%cx + %rx  * %s0), floor(%cy - %ry  * %c0),
                      floor(%cx + %rx  * %s1), floor(%cy - %ry  * %c1),
                      floor(%cx + %rxi * %s1), floor(%cy - %ryi * %c1));
   }
}

function SSC::arc(%cx, %cy, %r, %thick, %a0, %a1, %rgb, %alpha)
{
   SSC::earc(%cx, %cy, %r, %r, %thick, %a0, %a1, %rgb, %alpha);
}

function SSC::ring(%cx, %cy, %r, %thick, %rgb, %alpha)
{
   SSC::earc(%cx, %cy, %r, %r, %thick, 0, 360, %rgb, %alpha);
}

// small solid triangle pointing along %deg (0 = up), tip at (%x,%y)
function SSC::tri(%x, %y, %deg, %len, %rgb, %alpha)
{
   SSC::color(%rgb, %alpha);
   %bx = %x - %len * mSin(%deg);
   %by = %y + %len * mCos(%deg);
   %hw = %len * 0.55;
   %px = %hw * mCos(%deg);
   %py = %hw * mSin(%deg);
   glAngledPolygon(floor(%x), floor(%y), floor(%bx + %px), floor(%by + %py),
                   floor(%bx - %px), floor(%by - %py), floor(%x), floor(%y));
}

// a 2px line from (x0,y0) to (x1,y1) as a thin quad. glAngledPolygon culls one
// winding (memory: glangledpolygon-winding-cull) and a line's winding flips with
// its direction, so the quad is issued both ways -- one of them always survives.
function SSC::line(%x0, %y0, %x1, %y1, %rgb, %alpha)
{
   SSC::color(%rgb, %alpha);
   %dx = %x1 - %x0; %dy = %y1 - %y0;
   %len = sqrt(%dx * %dx + %dy * %dy);
   if (%len < 1) return;
   %nx = -%dy / %len; %ny = %dx / %len;
   %ax = floor(%x0 + %nx); %ay = floor(%y0 + %ny);
   %bx = floor(%x1 + %nx); %by = floor(%y1 + %ny);
   %cx = floor(%x1 - %nx); %cy = floor(%y1 - %ny);
   %ex = floor(%x0 - %nx); %ey = floor(%y0 - %ny);
   glAngledPolygon(%ax, %ay, %bx, %by, %cx, %cy, %ex, %ey);
   glAngledPolygon(%ex, %ey, %cx, %cy, %bx, %by, %ax, %ay);
}

// words %n.. of %str joined by single spaces
function SSC::restFrom(%str, %n)
{
   %out = "";
   %cnt = getWordCount(%str);
   for (%i = %n; %i < %cnt; %i++) {
      if (%out == "") %out = getWord(%str, %i);
      else %out = %out @ " " @ getWord(%str, %i);
   }
   return %out;
}

// component lamp state (0 ok / 1 degraded / 2 out) -> part tint
function SSC::stateRGB(%state)
{
   if (%state == 2) return $SSC::red;
   if (%state == 1) return $SSC::yellow;
   return $SSC::grey;
}

//--- HERC damage display ------------------------------------------------------
// Starsiege rendered the vehicle's own 3D shape here, grey-shaded, with damaged
// external components (Pelvis/Head/Legs/Calves/Feet/Pods) tinted yellow then red.
// Until the engine grows that primitive, a stylised HERC figure carries the
// state we DO have: head = sensors, pods = guns, torso = reactor, legs = legs
// (MMState lamps), all grey when healthy exactly like the real model.
function SSC::hercGlyph(%cx, %cy, %s, %head, %guns, %rctr, %legs)
{
   %u = 5 * %s;
   SSC::color(SSC::stateRGB(%rctr), 230);
   glRectangle(floor(%cx - 1.5 * %u), floor(%cy - 1.5 * %u), floor(3 * %u), floor(2 * %u));
   SSC::color(SSC::stateRGB(%head), 230);
   glRectangle(floor(%cx - 0.7 * %u), floor(%cy - 3 * %u), floor(1.4 * %u), floor(1.3 * %u));
   SSC::color(SSC::stateRGB(%guns), 230);
   glRectangle(floor(%cx - 3.2 * %u), floor(%cy - 1.9 * %u), floor(1.4 * %u), floor(2.2 * %u));
   glRectangle(floor(%cx + 1.8 * %u), floor(%cy - 1.9 * %u), floor(1.4 * %u), floor(2.2 * %u));
   SSC::color(SSC::stateRGB(%legs), 230);
   glRectangle(floor(%cx - 1.6 * %u), floor(%cy + 0.6 * %u), floor(%u), floor(3 * %u));
   glRectangle(floor(%cx + 0.6 * %u), floor(%cy + 0.6 * %u), floor(%u), floor(3 * %u));
}

// dam_self.bmp 83x51 at (75,49); "STATUS" centred on the ring, y 95
function SSC::drawStatus(%screen, %s)
{
   %w = floor(83 * %s);
   %h = floor(51 * %s);
   %at = ModernHUD::part("ModernHUD::SSStatus", "top-left", floor(75 * %s), floor(49 * %s), %w, floor(66 * %s), %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   SSC::img(%x, %y, %w, %h, "dam_self", 1);
   %cx = %x + floor(41 * %s);
   %cy = %y + floor(24 * %s);
   // the real thing: our own armour shape rendered into the ring, grey-shaded,
   // damaged parts on the yellow/red ramp (engine ssDrawShape); glyph fallback
   // Part states: the server's crit lamps (legs crippled / gun lost / sensor out /
   // reactor hit -- rare events, 0 or 2) OR'd with a progressive read of hull
   // damage, the same ramp the TARGET display uses from the ghost damage fraction.
   // Without the hull term the own display stayed grey through a whole fight.
   %dmg = 1 - $health / 100;
   if (SSC::demo() && $MMC::stamp == "") %dmg = 0;
   %legs = 0; %pods = 0; %head = 0; %torso = 0;
   if (%dmg >= 0.2) %legs = 1;
   if (%dmg >= 0.4) { %legs = 2; %pods = 1; }
   if (%dmg >= 0.6) { %pods = 2; %head = 1; %torso = 1; }
   if (%dmg >= 0.8) { %head = 2; %torso = 2; }
   if ($MMC::legs > %legs) %legs = $MMC::legs;
   if ($MMC::guns > %pods) %pods = $MMC::guns;
   if ($MMC::sens > %head) %head = $MMC::sens;
   if ($MMC::rctr > %torso) %torso = $MMC::rctr;
   // view: Starsiege framed the own herc from a fixed angle; ssHudShapeSelfTurn
   // makes it turn with your heading instead
   %yaw = $pref::ssHudShapeYaw;
   if ($pref::ssHudShapeSelfTurn == 1 && $SSC::hdg != "")
      %yaw = %yaw + $SSC::hdg;
   %drawn = 0;
   if ($SSC::shapePrim == 1 && $pref::ssHudShape != 0)
      %drawn = ssDrawShape(%cx - floor(30 * %s), %cy - floor(30 * %s), floor(60 * %s), floor(60 * %s),
                           "self", %yaw, $pref::ssHudShapePitch, %legs, %pods, %head, %torso);
   if (%drawn != 1)
      SSC::hercGlyph(%cx, %cy, %s, %head, %pods, %torso, %legs);
   SSC::textC(%cx, %y + floor(46 * %s), $SSC::green, "STATUS", floor(11 * %s));
   if ($MMC::down == 1)
      SSC::textC(%cx, %y + floor(60 * %s), $SSC::red, "OFFLINE", floor(11 * %s));
}

// dam_tar.bmp 83x51, 78 px from the right, y 49; name centred, y 95
function SSC::drawTarget(%screen, %s)
{
   %w = floor(83 * %s);
   %h = floor(51 * %s);
   %at = ModernHUD::part("ModernHUD::SSTarget", "top-right", floor(78 * %s), floor(49 * %s), %w, floor(66 * %s), %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   %t = $SSC::tgt;
   if (%t == "" && !SSC::demo()) {
      // Starsiege hides the whole display without a target
      return;
   }
   SSC::img(%x, %y, %w, %h, "dam_tar", 1);
   %cx = %x + floor(43 * %s);
   %cy = %y + floor(24 * %s);
   if (%t == "") {
      SSC::hercGlyph(%cx, %cy, %s, 0, 0, 0, 0);
      SSC::textC(%cx, %y + floor(46 * %s), $SSC::green, "Gorgon", floor(11 * %s));
      return;
   }
   %friendly = getWord(%t, 1);
   %dmg = getWord(%t, 7);
   if (%dmg < 0) %dmg = 0;
   %name = SSC::restFrom(%t, 8);
   // one damage fraction for the whole target until per-part data exists:
   // legs first, then pods, then the head go yellow/red as it accumulates
   %legs = 0; %pods = 0; %head = 0; %torso = 0;
   if (%dmg >= 0.2) %legs = 1;
   if (%dmg >= 0.4) { %legs = 2; %pods = 1; }
   if (%dmg >= 0.6) { %pods = 2; %head = 1; %torso = 1; }
   if (%dmg >= 0.8) { %head = 2; %torso = 2; }
   %drawn = 0;
   if ($SSC::shapePrim == 1 && $pref::ssHudShape != 0 && $SSC::tgtShape != "")
      %drawn = ssDrawShape(%cx - floor(30 * %s), %cy - floor(30 * %s), floor(60 * %s), floor(60 * %s),
                           $SSC::tgtShape, $pref::ssHudShapeYaw + $SSC::tgtYaw, $pref::ssHudShapePitch,
                           %legs, %pods, %head, %torso);
   if (%drawn != 1)
      SSC::hercGlyph(%cx, %cy, %s, %head, %pods, %torso, %legs);
   SSC::textC(%cx, %y + floor(46 * %s), $SSC::green, %name, floor(11 * %s));
}

//--- weapon table + equipment (RE report 2) -------------------------------------
//   block x 86..282 (197 wide) from y 351: header row 351..366, weapon rows
//   13 px pitch from 367, equipment lines 13 px pitch from 399 (value right-aligned)
function SSC::drawWeapons(%screen, %s)
{
   %w = floor(197 * %s);
   %rowH = floor(13 * %s);
   %n = $MMC::rackN;
   if (%n == "" || %n < 0) %n = 0;
   if (%n > 6) %n = 6;
   if (SSC::demo() && %n == 0) {
      %n = 2;
      $MMC::rack[0] = "HLAS";
      $MMC::rack[1] = "HLAS";
   }
   %eq = 0;
   if (SSC::herc()) {
      if ($HHC::c0 != "") %eq++;
      if ($HHC::c1 != "") %eq++;
      if ($HHC::c2 != "") %eq++;
      if ($HHC::c3 != "") %eq++;
      if ($HHC::mine != "") %eq++;
   }
   else if (SSC::demo())
      %eq = 2;
   %h = floor(16 * %s) + %rowH * %n + floor(7 * %s) + %rowH * %eq;
   %at = ModernHUD::part("ModernHUD::SSWeapons", "top-left", floor(86 * %s), floor(351 * %s), %w, %h, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   %px = floor(10 * %s);

   // columns: index cell 14 | abbrev 48 | 3 group cells 11 each | bar (rest)
   %numW  = floor(14 * %s);
   %nameW = floor(48 * %s);
   %gw    = floor(11 * %s);
   %gx    = %x + %numW + %nameW;
   %bx    = %gx + 3 * %gw;
   %bw    = %x + %w - %bx;

   // header: "1 2 3" over the group cells; the selected group (1) is boxed
   %hy = %y;
   %hh = floor(15 * %s);
   SSC::frame(%gx, %hy, %gw, %hh, $SSC::green, 230);
   SSC::textC(%gx + %gw / 2,           %hy + 2, $SSC::green, "1", %px);
   SSC::textC(%gx + %gw + %gw / 2,     %hy + 2, $SSC::green, "2", %px);
   SSC::textC(%gx + 2 * %gw + %gw / 2, %hy + 2, $SSC::green, "3", %px);

   %sel = getItemDesc(getMountedItem(0));
   %charge = 1 - $MMC::heat;
   if (SSC::herc()) %charge = $HHC::energy;
   if (SSC::demo() && $MMC::stamp == "") %charge = 0.88;
   if (%charge < 0) %charge = 0;
   if (%charge > 1) %charge = 1;

   %ry = %y + %hh + 1;
   for (%i = 0; %i < %n; %i++) {
      %nm = $MMC::rack[%i];
      if (%nm == "") %nm = "---";
      %isSel = (%sel != "" && String::findSubStr(%sel, %nm) == 0);
      %rgb = $SSC::green;
      if (!%isSel && %n > 1) %rgb = $SSC::dim;
      // row box with cell dividers
      SSC::frame(%x, %ry, %w, %rowH + 1, $SSC::green, 230);
      SSC::color($SSC::green, 230);
      glRectangle(%x + %numW, %ry, 1, %rowH + 1);
      glRectangle(%gx, %ry, 1, %rowH + 1);
      glRectangle(%gx + %gw, %ry, 1, %rowH + 1);
      glRectangle(%gx + 2 * %gw, %ry, 1, %rowH + 1);
      glRectangle(%bx, %ry, 1, %rowH + 1);
      SSC::textC(%x + %numW / 2, %ry + 2, %rgb, %i + 1, %px);
      SSC::text(%x + %numW + floor(3 * %s), %ry + 2, %rgb, String::toUpper(SSC::abbrev(%nm)), %px);
      // group membership dots: chained fire = every weapon in every group
      for (%g = 0; %g < 3; %g++) {
         SSC::color($SSC::green, 240);
         glRectangle(%gx + %g * %gw + floor(%gw / 2) - 1, %ry + floor(%rowH / 2) - 1, 3, 3);
      }
      // charge bar: bright fill left-aligned, dark remainder
      SSC::color($SSC::fill, 200);
      glRectangle(%bx + 2, %ry + 2, %bw - 3, %rowH - 3);
      SSC::color($SSC::green, 240);
      glRectangle(%bx + 2, %ry + 2, floor((%bw - 3) * %charge), %rowH - 3);
      %ry = %ry + %rowH;
   }

   // equipment lines: "<name>            <On/Off>"
   %ry = %ry + floor(7 * %s);
   %right = %x + %w;
   if (SSC::herc()) {
      if ($HHC::c0 != "") { SSC::eqRow(%x, %ry, %right, $HHC::c0, %px, %s); %ry = %ry + %rowH; }
      if ($HHC::c1 != "") { SSC::eqRow(%x, %ry, %right, $HHC::c1, %px, %s); %ry = %ry + %rowH; }
      if ($HHC::c2 != "") { SSC::eqRow(%x, %ry, %right, $HHC::c2, %px, %s); %ry = %ry + %rowH; }
      if ($HHC::c3 != "") { SSC::eqRow(%x, %ry, %right, $HHC::c3, %px, %s); %ry = %ry + %rowH; }
      if ($HHC::mine != "") {
         SSC::text(%x + floor(14 * %s), %ry + 1, $SSC::green, $HHC::mine, %px);
         SSC::textR(%right, %ry + 1, $SSC::green, $HHC::mines, %px);
         %ry = %ry + %rowH;
      }
   }
   else if (SSC::demo()) {
      SSC::eqRow(%x, %ry, %right, "ECM  off", %px, %s); %ry = %ry + %rowH;
      SSC::eqRow(%x, %ry, %right, "Shield Modulator  ON", %px, %s);
   }
}

// weapon abbreviation for the table: first word, at most 5 chars ("HLAS")
function SSC::abbrev(%name)
{
   %w = getWord(%name, 0);
   if (String::len(%w) > 5)
      %w = String::getSubStr(%w, 0, 5);
   return %w;
}

// "Chameleon Cloak  ON" -> name left, state right-aligned
function SSC::eqRow(%x, %y, %right, %line, %px, %s)
{
   if (%line == "")
      return;
   %cnt = getWordCount(%line);
   %state = getWord(%line, %cnt - 1);
   %name = %line;
   if (%cnt > 1 && (%state == "ON" || %state == "off" || String::findSubStr(%state, "%") != -1)) {
      %name = "";
      for (%i = 0; %i < %cnt - 1; %i++) {
         if (%name == "") %name = getWord(%line, %i);
         else %name = %name @ " " @ getWord(%line, %i);
      }
      if (%state == "ON") %state = "On";
      if (%state == "off") %state = "Off";
   }
   else
      %state = "";
   %rgb = $SSC::green;
   if (String::findSubStr(%line, "(inert)") != -1) %rgb = $SSC::dim;
   SSC::text(%x + floor(14 * %s), %y + 1, %rgb, %name, %px);
   if (%state != "")
      SSC::textR(%right, %y + 1, %rgb, %state, %px);
}

//--- reticle, lock brackets, lead marker (RE report 3) --------------------------
function SSC::drawReticle(%screen, %s)
{
   %w = getWord(%screen, 0);
   %h = getWord(%screen, 1);
   %cx = floor(%w / 2);
   %cy = floor(%h / 2);
   %t = $SSC::tgt;
   // frame 1 = dim cross (nothing acquired), 0 = bright (target), 2 = + lock box
   %frame = "retical_001";
   if (%t != "") {
      %friendly = getWord(%t, 1);
      %dist = getWord(%t, 2);
      %frame = "retical_000";
      if (%friendly != 1 && %dist <= 400) %frame = "retical_002";
   }
   %rs = floor(64 * %s);
   SSC::img(%cx - floor(%rs / 2), %cy - floor(%rs / 2), %rs, %rs, %frame, 1);

   if (%t == "")
      return;
   %tx = getWord(%t, 3);
   %ty = getWord(%t, 4);
   %hw = getWord(%t, 5);
   %th = getWord(%t, 6);
   if (%hw < 12 * %s) %hw = floor(12 * %s);
   if (%th < 16 * %s) %th = floor(16 * %s);

   // four red L brackets, 18x18, 2 px, on the projected bbox corners
   if (%friendly != 1) {
      %m = floor(14 * %s);
      %x0 = %tx - %hw - %m;
      %x1 = %tx + %hw + %m;
      %y0 = %ty - %m;
      %y1 = %ty + %th + %m;
      %len = floor(18 * %s);
      SSC::color($SSC::red, 240);
      glRectangle(%x0, %y0, %len, 2); glRectangle(%x0, %y0, 2, %len);
      glRectangle(%x1 - %len, %y0, %len, 2); glRectangle(%x1 - 2, %y0, 2, %len);
      glRectangle(%x0, %y1 - 2, %len, 2); glRectangle(%x0, %y1 - %len, 2, %len);
      glRectangle(%x1 - %len, %y1 - 2, %len, 2); glRectangle(%x1 - 2, %y1 - %len, 2, %len);
   }

   // lead.pba (59x58: circle centre (27,26) dia 34, tick right, triangle below)
   // on the target's centre, range "%dm" above-right, triangle overdrawn yellow
   %lw = floor(59 * %s);
   %lh = floor(58 * %s);
   %lcx = %tx;
   %lcy = %ty + floor(%th / 2);
   %lx = %lcx - floor(27 * %s);
   %ly = %lcy - floor(26 * %s);
   SSC::img(%lx, %ly, %lw, %lh, "lead_000", 1);
   SSC::tri(%lcx, %lcy + floor(19 * %s), 0, floor(8 * %s), $SSC::yellow, 240);
   SSC::text(%lcx + floor(14 * %s), %lcy - floor(30 * %s), $SSC::green, floor(%dist) @ "m", floor(10 * %s));
}

//--- shields gauge (RE report 4) ------------------------------------------------
//   shields.bmp 128x101 at BL (179, 49 from bottom); inner ring (centre 74,46,
//   rx 48 ry 28 in art space) redrawn thick+bright as the strength arc; yellow
//   needle outside the rim = shield direction (top = ahead); "SHIELDS:" / "n%"
//   centred two lines from y 1000 (art y + 70)
function SSC::drawShields(%screen, %s)
{
   %w = floor(128 * %s);
   %h = floor(101 * %s);
   %at = ModernHUD::part("ModernHUD::SSShields", "bottom-left", floor(179 * %s), floor(49 * %s), %w, %h, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   %frac = 0;
   if ($MMC::shieldMax > 0) %frac = $MMC::shield / $MMC::shieldMax;
   if (SSC::demo() && $MMC::stamp == "") %frac = 1;
   if (%frac < 0) %frac = 0;
   if (%frac > 1) %frac = 1;
   SSC::img(%x, %y, %w, %h, "shields", 1);
   %icx = %x + floor(74 * %s);
   %icy = %y + floor(46 * %s);
   %rx = floor(48 * %s);
   %ry = floor(28 * %s);
   // strength arc: the inner ring, bright and 5px, swept clockwise from the top
   if (%frac > 0)
      SSC::earc(%icx, %icy, %rx + 2, %ry + 2, floor(5 * %s), 0, floor(360 * %frac), $SSC::green, 240);
   // needle: filled yellow triangle just outside the outer rim, pointing in
   %ocx = %x + floor(66 * %s);
   %ocy = %y + floor(50 * %s);
   %nx = %ocx;
   %ny = %ocy - floor(46 * %s) - 3;
   SSC::tri(%nx, %ny + floor(9 * %s), 180, floor(9 * %s), $SSC::yellow, 240);
   // front / rear split inside the ring (both halves of an unfocused shield)
   if (%frac > 0) {
      %half = floor(50 * %frac);
      SSC::textC(%icx, %icy - floor(14 * %s), $SSC::green, %half, floor(10 * %s));
      SSC::textC(%icx, %icy + floor(2 * %s),  $SSC::green, %half, floor(10 * %s));
   }
   SSC::textC(%x + floor(62 * %s), %y + floor(70 * %s), $SSC::green, "SHIELDS:", floor(10 * %s));
   SSC::textC(%x + floor(62 * %s), %y + floor(85 * %s), $SSC::green, floor(100 * %frac) @ "%", floor(10 * %s));
}

//--- radar (RE report 5) ----------------------------------------------------------
//   radar_high frame 2 (opaque) 220x133 at BR (167 from right, 47 from bottom).
//   Dish ellipse centre (103,49) rx 71 ry 42 in art space; the baked amber
//   track runs 181..254 deg (0 = up, clockwise) at ~1.45x the dish radii.
function SSC::drawRadar(%screen, %s)
{
   %w = floor(220 * %s);
   %h = floor(133 * %s);
   %at = ModernHUD::part("ModernHUD::SSRadar", "bottom-right", floor(167 * %s), floor(47 * %s), %w, %h, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   SSC::img(%x, %y, %w, %h, "radar_high_002", 1);
   %cx = %x + floor(103 * %s);
   %cy = %y + floor(49 * %s);
   %rx = floor(71 * %s);
   %ry = floor(42 * %s);

   // ENERGY arc over the baked amber track
   %en = 1 - $MMC::heat;
   if (SSC::herc()) %en = $HHC::energy;
   if (SSC::demo() && $MMC::stamp == "") %en = 0.96;
   if (%en < 0) %en = 0;
   if (%en > 1) %en = 1;
   // the baked track occupies 1.27..1.63 of the dish radii, 180..244 degrees
   // (measured on frame 2 and confirmed on the live frame)
   if (%en > 0)
      SSC::earc2(%cx, %cy, floor(%rx * 1.63), floor(%ry * 1.63),
                 floor(%rx * 1.27), floor(%ry * 1.27),
                 180, 180 + floor(64 * %en), $SSC::yellow, 240);

   // heading line: yellow diameter across the dish, rotating with own heading,
   // with a small tick at each rim end
   %hdg = $SSC::hdg;
   if (%hdg == "") %hdg = 0;
   %sh = mSin(%hdg);
   %ch = mCos(%hdg);
   %ex = %cx + %rx * %sh; %ey = %cy - %ry * %ch;
   %wx = %cx - %rx * %sh; %wy = %cy + %ry * %ch;
   SSC::line(%wx, %wy, %ex, %ey, $SSC::yellow, 200);
   SSC::tri(%ex, %ey, %hdg + 180, floor(5 * %s), $SSC::yellow, 230);
   SSC::tri(%wx, %wy, %hdg, floor(5 * %s), $SSC::yellow, 230);
   // own vehicle: small orange square at the dish centre
   SSC::color("255 120 20", 240);
   glRectangle(%cx - 1, %cy - 1, 3, 3);

   // contacts, north-up on the dish (the heading line shows where we point)
   %range = $pref::ssRadarRange;
   if (%range <= 0) %range = 800;
   %list = $SSC::contacts;
   %n = getWordCount(%list);
   %bs = floor(3 * %s);
   if (%bs < 3) %bs = 3;
   for (%i = 0; %i + 3 < %n; %i = %i + 4) {
      %f  = getWord(%list, %i + 1);
      %dx = getWord(%list, %i + 2);
      %dy = getWord(%list, %i + 3);
      %d = sqrt(%dx * %dx + %dy * %dy);
      if (%d > %range) continue;
      %bx = %cx + %dx / %range * %rx;
      %by = %cy - %dy / %range * %ry;
      if (%f == 1) SSC::color($SSC::blue, 240);
      else SSC::color($SSC::red, 240);
      glRectangle(floor(%bx - %bs / 2), floor(%by - %bs / 2), %bs, %bs);
   }
   if (SSC::demo() && %n == 0) {
      SSC::color($SSC::red, 240);
      glRectangle(%cx + floor(20 * %s), %cy - floor(12 * %s), %bs, %bs);
      glRectangle(%cx - floor(30 * %s), %cy + floor(8 * %s), %bs, %bs);
      SSC::color($SSC::blue, 240);
      glRectangle(%cx + floor(8 * %s), %cy + floor(20 * %s), %bs, %bs);
   }

   // readouts: "ACTIVE %dm" centred above, "%dKph" left, "ENERGY:" / "n%" lower right
   %px = floor(10 * %s);
   SSC::textC(%cx, %y - floor(14 * %s), $SSC::green, "ACTIVE  " @ %range @ "m", %px);
   %kph = floor($speed * 3.6);
   if (SSC::demo() && $speed == "") %kph = 44;
   SSC::textR(%x - floor(4 * %s), %y + floor(31 * %s), $SSC::green, %kph @ "Kph", %px);
   SSC::textR(%x + floor(230 * %s), %y + floor(100 * %s), $SSC::green, "ENERGY:", %px);
   SSC::textR(%x + floor(230 * %s), %y + floor(115 * %s), $SSC::green, floor(100 * %en) @ "%", %px);
}

//--- top line: mode score, warnings ----------------------------------------------
function SSC::drawTop(%screen, %s)
{
   %w = getWord(%screen, 0);
   %herc = SSC::herc();
   %hf0 = $HHC::f0;  if (%hf0 == "") %hf0 = 0;
   %hf1 = $HHC::f1;  if (%hf1 == "") %hf1 = 0;
   if (%herc && $HHC::mode == "ctf")
      %tmsg = "FLAGS  " @ %hf0 @ "  -  " @ %hf1;
   else if (%herc && $HHC::mode == "dm")
      %tmsg = "KILLS  " @ %hf1 @ "     LEADER  " @ %hf0;
   else if ($MMC::wave != "")
      %tmsg = "WAVE " @ $MMC::wave @ "   SALVAGE " @ $MMC::salv;
   else if ($MMC::stamp != "")
      %tmsg = $MMC::t0 @ "  CV  " @ $MMC::t1;
   else
      %tmsg = "";
   if (%tmsg != "")
      SSC::textC(floor(%w / 2), floor(12 * %s), $SSC::green, %tmsg, floor(12 * %s));
   %wy = floor(getWord(%screen, 1) * 0.4);
   if ($MMC::down == 1)
      SSC::textC(floor(%w / 2), %wy, $SSC::red, "REACTOR OFFLINE", floor(26 * %s));
   else if (!%herc && $MMC::heat > 0.85)
      SSC::textC(floor(%w / 2), %wy, $SSC::yellow, "HEAT CRITICAL", floor(18 * %s));
   else if (%herc && $HHC::energy < 0.15)
      SSC::textC(floor(%w / 2), %wy, $SSC::yellow, "REACTOR LOW", floor(18 * %s));
}

//--- the frame ------------------------------------------------------------------

function SSC::enter()
{
   $SSC::mjSaveNames = $mj::shownames;
   $SSC::mjSaveHp = $mj::showhpbars;
   $SSC::mjSaveEye = $mj::eyesight;
   $SSC::saveXhair = $pref::hideCrosshairArt;
   $SSC::saveIff = $pref::hidePlayerIFFMarker;
   $mj::shownames = true;
   $mj::showhpbars = true;
   $mj::showshieldbars = true;
   $mj::eyesight = true;
   $mj::eyesightbars = true;
   $pref::hideCrosshairArt = 1;      // our reticle art replaces the stock one
   $pref::hidePlayerIFFMarker = 1;   // our lock brackets replace the skull
   $pref::ssHudExport = 1;           // fearGuiCrosshair.cpp publishes $SSC::*
   $SSC::on = 1;
}

function SSC::leave()
{
   if ($SSC::on != 1)
      return;
   $mj::shownames = $SSC::mjSaveNames;
   $mj::showhpbars = $SSC::mjSaveHp;
   $mj::showshieldbars = "";
   $mj::eyesight = $SSC::mjSaveEye;
   $mj::eyesightbars = "";
   $MMC::shields = "";
   $pref::hideCrosshairArt = $SSC::saveXhair;
   $pref::hidePlayerIFFMarker = $SSC::saveIff;
   $pref::ssHudExport = 0;
   $SSC::tgt = "";
   $SSC::contacts = "";
   $SSC::on = 0;
}

function ModernHUDPack::draw(%screen)
{
   // ★A preview pass never enters or leaves cockpit mode★ -- SSC::enter/leave swap live
   // prefs (crosshair art, IFF marker, $mj::*), and the Options preview must leave the game
   // exactly as it found it.
   %preview = ($ModernHUD::Preview == 1);
   if (!SSC::active()) {
      if (!%preview)
         SSC::leave();
      ModernHUD::hide("ModernHUD::SSStatus");
      ModernHUD::hide("ModernHUD::SSTarget");
      ModernHUD::hide("ModernHUD::SSWeapons");
      ModernHUD::hide("ModernHUD::SSShields");
      ModernHUD::hide("ModernHUD::SSRadar");
      return;
   }
   if ($SSC::on != 1 && !%preview)
      SSC::enter();

   // Starsiege's high-res set is 1:1 at 1080 lines; scale by height elsewhere
   // Below ~0.85 the 10px Starsiege text no longer fits its 13px rows once the
   // font floors at 9px (text walked out of the weapon table and shields labels
   // at 720p), so the whole HUD stops shrinking there -- Starsiege itself swapped
   // to a half-size art set instead of scaling, so a floor is in character.
   %s = getWord(%screen, 1) / 1080;
   if (%s < 0.85) %s = 0.85;

   SSC::drawStatus(%screen, %s);
   SSC::drawTarget(%screen, %s);
   SSC::drawWeapons(%screen, %s);
   SSC::drawShields(%screen, %s);
   SSC::drawRadar(%screen, %s);
   glPartScale(0, 0, 1);
   SSC::drawReticle(%screen, %s);
   SSC::drawTop(%screen, %s);
}

//--- pack contract --------------------------------------------------------------

function ModernHUDPack::ownsSlot(%value)
{
   if (!SSC::active())
      return false;
   if (%value == "")
      return true;
   if (%value == "off")
      return false;
   return String::findSubStr(%value, "SSCockpit::") == 0;
}

function ModernHUDPack::detachRetained()
{
}

function ModernHUDPack::prefs()
{
}

function ModernHUDPack::stockHuds()
{
   // the stock huds this pack keeps are declared through stock() too: that is
   // what registers them as K-editor drag targets (the minimap was immovable)
   ModernHUD::stock("Minimap", true);
   ModernHUD::stock("clockHud", true);
   // Presto's KronosChat overlay replaces the stock chat when it is enabled and
   // hides chatDisplayHud on PlayGui open; forcing it back on here (this event
   // handler runs after Presto's) put TWO chat boxes on screen.
   if ($pref::Kronos::chatEnabled)
      ModernHUD::stock("chatDisplayHud", false);
   else
      ModernHUD::stock("chatDisplayHud", true);
   if (SSC::active()) {
      ModernHUD::stock("healthHud", false);
      ModernHUD::stock("weaponHud", false);
      ModernHUD::stock("compassHud", false);
      ModernHUD::stock("sensorHUD", false);
      ModernHUD::stock("jetPackHud", false);
   }
   else {
      ModernHUD::stock("healthHud", true);
      ModernHUD::stock("weaponHud", true);
      ModernHUD::stock("compassHud", true);
      ModernHUD::stock("sensorHUD", true);
      ModernHUD::stock("jetPackHud", true);
   }
}

function ModernHUDPack::init()
{
}

// unload: put every client-wide knob back (ModernHUD::unload calls this first)
function ModernHUDPack::restore()
{
   SSC::leave();
}

ModernHUD::attach("eventGuiOpen_PlayGui", "ModernHUDPack::stockHuds");

ModernHUD::setting("int", "pref::hercCamScale", "Mech chase camera distance", "8",
   "3|14|1", "");
ModernHUD::setting("int", "pref::ssRadarRange", "Radar range (m)", "800",
   "200|2000|100", "", "ModernHUD::SSRadar");
ModernHUD::setting("bool", "pref::ssHudShapeSelfTurn", "Status model turns with heading", "0",
   "", "", "ModernHUD::SSStatus");

$ModernHUD::LoadComplete = "sscockpit";
echo("[SSHUD] Starsiege Cockpit pack loaded.");
