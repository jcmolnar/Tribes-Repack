//==============================================================================
// Stock (2026) -- BORROWABLE PARTS (components.cs)
//
// Split out of hud.cs by tools/modernhud_components.py so another pack can draw these
// parts: the HUD designer's Provider choice sets $pref::HudSlot::<slot> to
// "stock2026/<slot>", and ModernHUD::drawSlot execs this file on demand. It must define only
// S26:: names -- ModernHUDPack:: belongs to whichever pack is the base.
// hud.cs execs this file too, so the pack's own draw uses the same code.
//==============================================================================



//------------------------------------------------------------------------------
// Tables. Seeded once; a re-exec re-seeds.
//------------------------------------------------------------------------------
// The stock weapon list draws every carried item whose datablock has a hudIcon and
// showWeaponBar (base\scripts\item.cs), in datablock order. Script cannot walk the
// datablocks, so the stock table is carried here: display name (what getItemCount
// takes), icon stem (I_<stem>_on/off.png), and the ammo item's display name ("" =
// no ammo type = the infinity glyph, which is what stock shows for packs too).
function S26::tables()
{
   S26::wep(0,  "Blaster",           "blaster",         "");
   S26::wep(1,  "Chaingun",          "chain",           "Bullet");
   S26::wep(2,  "Plasma Gun",        "plasma",          "Plasma Bolt");
   S26::wep(3,  "Grenade Launcher",  "grenade",         "Grenade Ammo");
   S26::wep(4,  "Mortar",            "mortar",          "Mortar Ammo");
   S26::wep(5,  "Disc Launcher",     "disk",            "Disc");
   S26::wep(6,  "Laser Rifle",       "sniper",          "");
   S26::wep(7,  "ELF Gun",           "energyRifle",     "");
   S26::wep(8,  "Inventory Station", "deployable",      "");
   S26::wep(9,  "Ammo Station",      "deployable",      "");
   S26::wep(10, "Energy Pack",       "energypack",      "");
   S26::wep(11, "Repair Pack",       "repairpack",      "");
   S26::wep(12, "Shield Pack",       "shieldpack",      "");
   S26::wep(13, "Sensor Jammer Pack","sensorjamerpack", "");
   S26::wep(14, "Motion Sensor",     "deployable",      "");
   S26::wep(15, "Ammo Pack",         "ammopack",        "");
   S26::wep(16, "Pulse Sensor",      "deployable",      "");
   S26::wep(17, "Sensor Jammer",     "deployable",      "");
   S26::wep(18, "Camera",            "deployable",      "");
   S26::wep(19, "Turret",            "deployable",      "");
   $S26::WepCount = 20;

   // Compass glyphs: compasshud.cpp gCompassPoints, as line segments about the
   // dial centre (+y is DOWN, screen space). N E W S, then the four 45-degree marks.
   %s = 0;
   // N
   %s = S26::seg(%s, -2,-18, -2,-24);  %s = S26::seg(%s, -2,-24, 2,-18);  %s = S26::seg(%s, 2,-18, 2,-24);
   // E
   %s = S26::seg(%s, 18,2, 18,-2);     %s = S26::seg(%s, 18,-2, 24,-2);   %s = S26::seg(%s, 24,-2, 24,2);
   %s = S26::seg(%s, 21,-2, 21,2);
   // W
   %s = S26::seg(%s, -24,4, -18,2);    %s = S26::seg(%s, -18,2, -24,0);   %s = S26::seg(%s, -24,0, -18,-2);
   %s = S26::seg(%s, -18,-2, -24,-4);
   // S
   %s = S26::seg(%s, 2,20, 1,19);   %s = S26::seg(%s, 1,19, -1,19);  %s = S26::seg(%s, -1,19, -2,20);
   %s = S26::seg(%s, -2,20, -2,21); %s = S26::seg(%s, -2,21, -1,22); %s = S26::seg(%s, -1,22, 1,22);
   %s = S26::seg(%s, 1,22, 2,23);   %s = S26::seg(%s, 2,23, 2,24);   %s = S26::seg(%s, 2,24, 1,25);
   %s = S26::seg(%s, 1,25, -1,25);  %s = S26::seg(%s, -1,25, -2,24);
   // marks
   %s = S26::seg(%s, 15,-14, 19,-18); %s = S26::seg(%s, 15,14, 19,18);
   %s = S26::seg(%s, -14,14, -18,18); %s = S26::seg(%s, -14,-14, -18,-18);
   $S26::SegCount = %s;
}

function S26::wep(%i, %name, %icon, %ammo)
{
   $S26::WepName[%i] = %name;
   $S26::WepIcon[%i] = %icon;
   $S26::WepAmmo[%i] = %ammo;
}

function S26::seg(%i, %x1, %y1, %x2, %y2)
{
   $S26::Seg[%i, 0] = %x1; $S26::Seg[%i, 1] = %y1;
   $S26::Seg[%i, 2] = %x2; $S26::Seg[%i, 3] = %y2;
   return %i + 1;
}

//------------------------------------------------------------------------------
// Drawing primitives.
//------------------------------------------------------------------------------
// The stock palette's HUD colours. GREEN/YELLOW/RED are palette indices in the
// retained code; these are the colours they resolve to on the stock palette.
function S26::colour(%name, %a)
{
   if(%name == "green")       glColor4ub(0, 255, 0, %a);
   else if(%name == "yellow") glColor4ub(255, 255, 0, %a);
   else if(%name == "red")    glColor4ub(255, 0, 0, %a);
   else if(%name == "cyan")   glColor4ub(4, 197, 252, %a);   // HJ_End_Green.png's actual pixel
   else if(%name == "dark")   glColor4ub(0, 0, 0, %a);
   else                       glColor4ub(255, 255, 255, %a);
}

// ★Every angled quad goes through this.★ glAngledPolygon inherits GL_CULL_FACE, so a
// quad wound the wrong way is silently dropped (glangledpolygon-winding-cull: Ascend's
// hexagons rendered as houses). Shoelace over all four vertices; negative area is
// re-wound. Same guard as Ascend::quad.
function S26::quad(%x1, %y1, %x2, %y2, %x3, %y3, %x4, %y4)
{
   %a = (%x1 * %y2 - %x2 * %y1) + (%x2 * %y3 - %x3 * %y2) +
        (%x3 * %y4 - %x4 * %y3) + (%x4 * %y1 - %x1 * %y4);
   if(%a >= 0)
      glAngledPolygon(%x1, %y1, %x2, %y2, %x3, %y3, %x4, %y4);
   else
      glAngledPolygon(%x4, %y4, %x3, %y3, %x2, %y2, %x1, %y1);
}

// A 2 px line as a quad -- glAngledPolygon is the only free-angle primitive. The
// first cut was 1.5 px and unguarded: half the compass glyphs were culled and the
// rest were too thin to survive the blend.
function S26::line(%x1, %y1, %x2, %y2)
{
   %dx = %x2 - %x1;
   %dy = %y2 - %y1;
   %len = sqrt(%dx * %dx + %dy * %dy);
   if(%len <= 0)
      return;
   %nx = -%dy / %len;
   %ny =  %dx / %len;
   S26::quad(%x1 + %nx, %y1 + %ny, %x2 + %nx, %y2 + %ny,
             %x2 - %nx, %y2 - %ny, %x1 - %nx, %y1 - %ny);
}

function S26::pad2(%v)
{
   if(%v < 10)
      return "0" @ %v;
   return %v;
}

//------------------------------------------------------------------------------
// PART: health + jetpack (HealthHud.cpp / FearGuiJetHud.cpp, hi-res branch).
//------------------------------------------------------------------------------
// One 84 px bar at (+37,+10): a 6x13 pointed end cap, a flat run, and the same cap
// MIRRORED (the stock control draws the right one with GFX_FLIP_X). The cap art
// (HH_End_*.png / HJ_End_Green.png) is a solid one-colour chevron -- rows 4..8 full
// width, tapering to 2 px at the top and bottom -- so it is drawn here as geometry in
// the bar colour rather than as an image, which is what makes the mirror possible
// (glDrawImage cannot flip). The stock control composes the full bar and CLIPS it to
// `energyWidth`; here each piece is drawn only as far as the level reaches.
function S26::bar(%bx, %by, %w, %colour)
{
   if(%w < 1)
      return;
   S26::colour(%colour, 255);
   // left cap: point on the left
   if(%w >= 6)
      S26::quad(%bx + 6, %by, %bx + 6, %by + 13, %bx, %by + 9, %bx, %by + 4);
   else
      glRectangle(%bx + 6 - %w, %by + 4, %w, 5);
   if(%w > 6)
   {
      %run = %w - 6; if(%run > 72) %run = 72;
      glRectangle(%bx + 6, %by, %run, 13);
   }
   if(%w > 78)
   {
      // right cap: the mirror image, revealed from its flat side
      %c2 = %w - 78; if(%c2 > 6) %c2 = 6;
      %rx = %bx + 78;
      S26::quad(%rx, %by, %rx + %c2, %by + 4 * %c2 / 6, %rx + %c2, %by + 13 - 4 * %c2 / 6, %rx, %by + 13);
   }
}

function S26::health(%x, %y)
{
   glDrawImage(%x + 1, %y + 1, 128, 32, "hudTrans.png", 255);

   %lvl = $health / 100;
   if(%lvl < 0) %lvl = 0;
   if(%lvl > 1) %lvl = 1;
   %w = floor(%lvl * 85);
   if(%lvl > 0 && %w < 1) %w = 1;
   if(%w >= 57)      %colour = "green";
   else if(%w >= 28) %colour = "yellow";
   else              %colour = "red";

   // The icon: green when healthy, red when critical, and FLASHING between the
   // two in the band just above critical (512 ms period, off the wall clock).
   if(%w >= 1 && %w <= 27)
   {
      %phase = floor(glTicks() / 512);
      %icon = ((%phase - floor(%phase / 2) * 2) == 1) ? "HH_Icon_Green.png" : "HH_Icon_Red.png";
   }
   else if(%w >= 28)
      %icon = "HH_Icon_Green.png";
   else
      %icon = "HH_Icon_Red.png";
   glDrawImage(%x + 5, %y + 3, 24, 27, %icon, 255);

   if(%lvl <= 0)
      return;
   S26::bar(%x + 37, %y + 10, %w, %colour);
}

function S26::jet(%x, %y)
{
   glDrawImage(%x + 1, %y + 1, 128, 32, "hudTrans.png", 255);
   glDrawImage(%x + 5, %y + 8, 24, 21, "HJ_Icon_Green.png", 255);

   %lvl = $energy / 100;
   if(%lvl < 0) %lvl = 0;
   if(%lvl > 1) %lvl = 1;
   %w = floor(%lvl * 85);
   if(%lvl > 0 && %w < 1) %w = 1;
   if(%lvl <= 0)
      return;
   S26::bar(%x + 37, %y + 11, %w, "cyan");   // the stock energy bar is cyan, not green
}

// Two parts, two handles: PlayGui authored health at 50,7 and jet at 50,28, each 35
// tall, so they overlap by 14 px by design (the art's chrome interleaves). Separate
// handles let a player pull them apart in the K editor, which one shared box could not.
function S26::status(%screen)
{
   %at = ModernHUD::part("ModernHUD::S26Health", "top-left", 50, 7, 131, 35, %screen);
   S26::health(getWord(%at, 0), getWord(%at, 1));
   %at = ModernHUD::part("ModernHUD::S26Jet", "top-left", 50, 28, 131, 35, %screen);
   S26::jet(getWord(%at, 0), getWord(%at, 1));
}

//------------------------------------------------------------------------------
// PART: the weapon list (CurWeapHud.cpp, hi-res branch).
//------------------------------------------------------------------------------
// Box 36 px of translucent backing + a 24 px icon column; one 18 px icon per
// carried item at a 22 px pitch, 7 px in from the top and bottom. Green bracket
// frame on the left. Beside each icon: the ammo backdrop, then the count (or the
// infinity glyph for anything without an ammo type).
//
// The carried set is rescanned every 300 ms (one getItemCount per table row); the
// counts are read live.
function S26::weapons(%screen)
{
   // A borrowed weapon list never runs this pack's init/draw, which seed the table.
   if($S26::WepCount == "")
      S26::tables();
   %now = glTicks();
   if($S26::WepAt == "" || %now < $S26::WepAt || %now - $S26::WepAt >= 300)
   {
      $S26::WepAt = %now;
      %n = 0;
      for(%i = 0; %i < $S26::WepCount; %i++)
      {
         if(getItemCount($S26::WepName[%i]) > 0)
         {
            $S26::Own[%n] = %i;
            %n++;
         }
      }
      $S26::OwnCount = %n;
   }
   %n = $S26::OwnCount;
   if(%n == "" || %n <= 0)
   {
      ModernHUD::hide("ModernHUD::S26Weapons");
      return;
   }

   %w = 62;
   %h = %n * 22 - 4 + 14;
   %at = ModernHUD::part("ModernHUD::S26Weapons", "bottom-left", 0, 150, %w, %h, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);

   // translucent backing (stock: an 8-vertex chamfered poly at alpha-fill)
   S26::colour("dark", 110);
   glRectangle(%x + 2, %y + 2, 28, %h - 4);

   // the green frame: chamfered top and bottom corners, a 2 px left edge, and a
   // short top/bottom lip
   S26::colour("green", 255);
   S26::quad(%x, %y + 7, %x + 7, %y, %x + 10, %y, %x, %y + 10);
   S26::quad(%x, %y + %h - 8, %x + 7, %y + %h - 1, %x + 10, %y + %h - 1, %x, %y + %h - 11);
   glRectangle(%x + 7, %y, 11, 2);
   glRectangle(%x + 7, %y + %h - 2, 11, 2);
   glRectangle(%x, %y + 7, 2, %h - 14);

   %mounted = GetItemDesc(GetMountedItem(0));
   glSetFont("Verdana", 10);
   %w888 = getWord(glGetStringDimensions("888"), 0);

   %iy = %y + 7;
   for(%k = 0; %k < %n; %k++)
   {
      %i = $S26::Own[%k];
      %sel = ($S26::WepName[%i] == %mounted);
      %icon = "I_" @ $S26::WepIcon[%i] @ (%sel ? "_on.png" : "_off.png");
      glDrawImage(%x + 4, %iy, 24, 18, %icon, 255);

      %tx = %x + 4 + 24 + 4;
      glDrawImage(%tx, %iy, 32, 18, "ammoSh.png", 255);
      if($S26::WepAmmo[%i] == "")
      {
         glDrawImage(%tx + 4, %iy + 5, 13, 6, (%sel ? "I_Infinity_on.png" : "I_Infinity_off.png"), 255);
      }
      else
      {
         %count = getItemCount($S26::WepAmmo[%i]);
         if(%count > 999) %count = 999;
         %sw = getWord(glGetStringDimensions(%count), 0);
         glDrawString(%tx + 2 + floor((%w888 - %sw) / 2), %iy - 1,
                      (%sel ? "<00ff00>" : "<008800>") @ %count);
      }
      %iy += 22;
   }
}

//------------------------------------------------------------------------------
// PART: the clock (clockhud.cpp). "HH:MM:SS.t" of the client clock's magnitude.
//------------------------------------------------------------------------------
function S26::clock(%screen)
{
   %at = ModernHUD::part("ModernHUD::S26Clock", "bottom-left", 0, 0, 79, 18, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);

   S26::colour("dark", 110);
   glRectangle(%x, %y, 79, 18);
   S26::colour("green", 255);
   glRectangle(%x, %y, 2, 18);
   glRectangle(%x, %y, 6, 1);
   glRectangle(%x, %y + 17, 6, 1);
   glRectangle(%x + 77, %y, 2, 18);
   glRectangle(%x + 73, %y, 6, 1);
   glRectangle(%x + 73, %y + 17, 6, 1);

   %t = getHudTimer();
   if(%t < 0) %t = -%t;
   %whole = floor(%t);
   %tenths = floor((%t - %whole) * 10);
   %hours = floor(%whole / 3600);
   %mins = floor((%whole - %hours * 3600) / 60);
   %secs = %whole - %hours * 3600 - %mins * 60;
   glSetFont("Verdana", 10);
   glDrawString(%x + 5, %y + 2, "<ffffff>" @ S26::pad2(%hours) @ ":" @ S26::pad2(%mins) @ ":" @ S26::pad2(%secs) @ "." @ %tenths);
}

//------------------------------------------------------------------------------
// PART: the compass (compasshud.cpp). Dial art, the four glyphs and four marks
// rotated by the player's heading, a fixed red north-up mark at the top.
//------------------------------------------------------------------------------
function S26::compass(%screen)
{
   %at = ModernHUD::part("ModernHUD::S26Compass", "top-right", 0, 376, 64, 64, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   glDrawImage(%x, %y, 64, 64, "compass.png", 255);

   %cx = %x + 31.5;
   %cy = %y + 31.5;
   %c = $compassCos; if(%c == "") %c = 1;
   %s = $compassSin; if(%s == "") %s = 0;

   S26::colour("green", 255);
   for(%i = 0; %i < $S26::SegCount; %i++)
   {
      %ax = $S26::Seg[%i, 0]; %ay = $S26::Seg[%i, 1];
      %bx = $S26::Seg[%i, 2]; %by = $S26::Seg[%i, 3];
      S26::line(%cx + (%ax * %c - %ay * %s), %cy + (%ax * %s + %ay * %c),
                %cx + (%bx * %c - %by * %s), %cy + (%bx * %s + %by * %c));
   }
   S26::colour("red", 255);
   glRectangle(%cx - 0.5, %cy - 29, 1.5, 15);
}

//------------------------------------------------------------------------------
// PART: the sensor ping light (FearHudRadarPing.cpp). Base plate always; while
// pinged the light cycles Lo/Med/Hi/Hi/Hi/Hi at 100 ms; suppressed shows its own.
//------------------------------------------------------------------------------
function S26::sensor(%screen)
{
   %at = ModernHUD::part("ModernHUD::S26Sensor", "top-right", 384, 0, 32, 23, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   glDrawImage(%x, %y, 32, 32, "ping.png", 255);

   %ping = $sensorPing;
   %bmp = "";
   if(%ping == 2)
   {
      %bmp = "HP_Supressed.png"; %bw = 24; %bh = 15;
      $S26::PingStart = "";
   }
   else if(%ping == 1)
   {
      %now = glTicks();
      if($S26::PingStart == "" || %now < $S26::PingStart)
         $S26::PingStart = %now;
      %step = floor((%now - $S26::PingStart) / 100);
      %v = %step - floor(%step / 6) * 6;
      if(%v == 0)      { %bmp = "HP_PingedLo.png";  %bw = 11; %bh = 5;  }
      else if(%v == 1) { %bmp = "HP_PingedMed.png"; %bw = 18; %bh = 11; }
      else             { %bmp = "HP_PingedHi.png";  %bw = 24; %bh = 15; }
   }
   else
      $S26::PingStart = "";

   if(%bmp != "")
      glDrawImage(%x + floor((32 - %bw) / 2), %y + floor((23 - %bh) / 2), %bw, %bh, %bmp, 255);
}

// The pack's own part switches. Unset = on; quoted compares so a stored "false"
// and a stored "0" both read as off without a float promotion.
function S26::on(%part)
{
   %v = getVariable("pref::ModernHUD::stock2026::" @ %part);
   if(%v == "" || %v == "0" || %v == "false" || %v == "False")
      return %v == "";
   return true;
}

// Parts other packs may borrow through the slot picker.
ModernHUD::component("stock2026", "healthenergy", "healthenergy", "S26::status");
ModernHUD::component("stock2026", "weapon",       "weapon",       "S26::weapons");
ModernHUD::component("stock2026", "clock",        "clock",        "S26::clock");
