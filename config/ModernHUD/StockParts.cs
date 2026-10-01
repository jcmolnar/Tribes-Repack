// Shared immediate-mode replacements for the stock controls used by converted
// configs. Native code supplies data only; every visible pixel is ScriptGL.
// Framework owns visibility/placement migration and calls the active draw helpers.
// Each part has its own qualified handle and pack-scoped settings, so position,
// anchor, size, opacity, hide, and these appearance options work in the designer.

function ModernHUDStock::key(%kind, %leaf)
{
   return ModernHUD::packSettingKey("Stock" @ %kind @ "::" @ %leaf);
}

function ModernHUDStock::value(%kind, %leaf)
{
   return getVariable(ModernHUDStock::key(%kind, %leaf));
}

function ModernHUDStock::setting(%kind, %type, %leaf, %label, %default, %spec)
{
   %key = ModernHUDStock::key(%kind, %leaf);
   if(!ModernHUD::hasSetting(%key))
      ModernHUD::setting(%type, %key, %label, %default, %spec, "", "ModernHUD::Stock" @ %kind);
}

// Idempotent, including after a pack unload clears the setting registry. Framework
// can call this when it registers a stock replacement, before the first draw.
function ModernHUDStock::settings(%kind)
{
   if(%kind == "")
   {
      %parts = "Health Energy Weapon Clock Compass Sensor";
      for(%i = 0; %i < 6; %i++)
      {
         %part = getWord(%parts, %i);
         ModernHUDStock::settings(%part);
         // Shared panels are not manifest parts. Register their tint before a
         // fresh preset matches saved setting names, even when a panel is off.
         ModernHUD::partTint("ModernHUD::Stock" @ %part);
      }
      return;
   }
   %first = (%kind == "Health" || %kind == "Energy") ? "Chrome" : "Frame";
   if(ModernHUD::hasSetting(ModernHUDStock::key(%kind, %first))) return;
   %colors = "Green|green;Cyan|cyan;White|white;Amber|yellow;Red|red;Blue|blue";
   if(%kind == "Health" || %kind == "Energy")
   {
      %chrome = ($Hud::StockChrome == "1") ? "1" : "0";
      ModernHUDStock::setting(%kind, "bool", "Chrome", "Stock icon and frame", %chrome, "");
      ModernHUDStock::setting(%kind, "enum", "FrameColor", "Frame color", "green", %colors);
      ModernHUDStock::setting(%kind, "bool", "Number", "Show percentage", "0", "");
      ModernHUDStock::setting(%kind, "int", "BarWidth", "Bar width (px)", "85", "30|300|5");
      ModernHUDStock::setting(%kind, "int", "BarHeight", "Bar height (px)", "13", "4|40|1");
      if(%kind == "Health")
      {
         ModernHUDStock::setting(%kind, "enum", "Color", "Bar color", "auto", "Health thresholds|auto;" @ %colors);
         ModernHUDStock::setting(%kind, "bool", "Flash", "Flash critical health icon", "1", "");
      }
      else
         ModernHUDStock::setting(%kind, "enum", "Color", "Bar color", "cyan", %colors);
   }
   else if(%kind == "Weapon")
   {
      ModernHUDStock::setting(%kind, "bool", "Frame", "Show weapon frame", "1", "");
      ModernHUDStock::setting(%kind, "bool", "Ammo", "Show ammunition", "1", "");
      ModernHUDStock::setting(%kind, "int", "Spacing", "Weapon row spacing (px)", "4", "0|16|1");
      ModernHUDStock::setting(%kind, "enum", "Color", "Selected weapon text", "green", %colors);
      ModernHUDStock::setting(%kind, "int", "Background", "Weapon backing opacity (%)", "43", "0|100|5");
   }
   else if(%kind == "Clock")
   {
      ModernHUDStock::setting(%kind, "bool", "Frame", "Show clock frame", "1", "");
      ModernHUDStock::setting(%kind, "bool", "Tenths", "Show tenths of a second", "1", "");
      ModernHUDStock::setting(%kind, "enum", "Color", "Clock text color", "white", %colors);
      ModernHUDStock::setting(%kind, "int", "Background", "Clock backing opacity (%)", "43", "0|100|5");
   }
   else if(%kind == "Compass")
   {
      ModernHUDStock::setting(%kind, "bool", "Frame", "Show compass dial art", "1", "");
      ModernHUDStock::setting(%kind, "bool", "Waypoint", "Show command arrow and distance", "1", "");
      ModernHUDStock::setting(%kind, "enum", "Color", "Compass markings", "green", %colors);
      ModernHUDStock::setting(%kind, "int", "LineWidth", "Compass line width (px)", "1", "1|4|1");
   }
   else if(%kind == "Sensor")
   {
      ModernHUDStock::setting(%kind, "bool", "Frame", "Show sensor backing", "1", "");
      ModernHUDStock::setting(%kind, "bool", "Animate", "Animate sensor ping", "1", "");
      ModernHUDStock::setting(%kind, "bool", "Label", "Show sensor status text", "0", "");
   }
   if(%kind == "Weapon" || %kind == "Clock" || %kind == "Compass")
   {
      ModernHUDStock::setting(%kind, "enum", "Font", "Text font", "Verdana", "Verdana|Verdana;Segoe UI|Segoe UI;Consolas|Consolas;Arial|Arial");
      ModernHUDStock::setting(%kind, "int", "FontSize", "Text size (px)", "10", "8|24|1");
   }
}

function ModernHUDStock::on(%kind, %leaf)
{
   %v = ModernHUDStock::value(%kind, %leaf);
   return (%v != "0" && %v != "false" && %v != "False" && %v != "");
}

function ModernHUDStock::color(%name, %alpha)
{
   %alpha = ModernHUD::alphaOf(%alpha);
   if(%name == "green")       glColor4ub(0, 255, 0, %alpha);
   else if(%name == "dim")    glColor4ub(0, 136, 0, %alpha);
   else if(%name == "yellow") glColor4ub(255, 255, 0, %alpha);
   else if(%name == "red")    glColor4ub(255, 0, 0, %alpha);
   else if(%name == "cyan")   glColor4ub(4, 197, 252, %alpha);
   else if(%name == "blue")   glColor4ub(60, 140, 255, %alpha);
   else if(%name == "dark")   glColor4ub(0, 0, 0, %alpha);
   else                       glColor4ub(255, 255, 255, %alpha);
}

function ModernHUDStock::image(%x, %y, %w, %h, %path)
{
   // Stock resources (including mod hudIcon bitmaps) are already mounted. No
   // dependency on a particular pack's extracted asset directory is required.
   glDrawImage(%x, %y, %w, %h, %path, ModernHUD::alphaOf(255));
}

// The paired stock bars have interleaving top/bottom chrome. Draw their frame
// explicitly so colors and proportions remain editable at every bar size.
function ModernHUDStock::vitalFrame(%kind, %x, %y, %w, %h, %flip)
{
   ModernHUDStock::color("dark", 110);
   ModernHUDStock::quad(%x + 3, %y + 7, %x + 10, %y + 2, %x + %w - 9, %y + 2, %x + %w - 2, %y + 7);
   glRectangle(%x + 3, %y + 7, %w - 5, %h - 14);
   ModernHUDStock::quad(%x + 3, %y + %h - 7, %x + %w - 2, %y + %h - 7, %x + %w - 9, %y + %h - 2, %x + 10, %y + %h - 2);
   ModernHUDStock::color(ModernHUDStock::value(%kind, "FrameColor"), 255);
   if(%flip)
   {
      ModernHUDStock::line(%x + 2, %y + %h - 8, %x + 9, %y + %h - 2, 1);
      glRectangle(%x + 9, %y + %h - 2, %w - 18, 1);
      ModernHUDStock::line(%x + %w - 9, %y + %h - 2, %x + %w - 2, %y + %h - 8, 1);
   }
   else
   {
      ModernHUDStock::line(%x + 2, %y + 8, %x + 9, %y + 2, 1);
      glRectangle(%x + 9, %y + 2, %w - 18, 1);
      ModernHUDStock::line(%x + %w - 9, %y + 2, %x + %w - 2, %y + 8, 1);
   }
}

function ModernHUDStock::font(%kind)
{
   glSetFont(ModernHUDStock::value(%kind, "Font"), ModernHUDStock::value(%kind, "FontSize"));
}

// Guard winding because the immediate quad primitive inherits GL_CULL_FACE.
function ModernHUDStock::quad(%x1, %y1, %x2, %y2, %x3, %y3, %x4, %y4)
{
   %area = (%x1 * %y2 - %x2 * %y1) + (%x2 * %y3 - %x3 * %y2) +
           (%x3 * %y4 - %x4 * %y3) + (%x4 * %y1 - %x1 * %y4);
   if(%area >= 0)
      glAngledPolygon(%x1, %y1, %x2, %y2, %x3, %y3, %x4, %y4);
   else
      glAngledPolygon(%x4, %y4, %x3, %y3, %x2, %y2, %x1, %y1);
}

function ModernHUDStock::line(%x1, %y1, %x2, %y2, %width)
{
   %dx = %x2 - %x1;
   %dy = %y2 - %y1;
   %length = sqrt(%dx * %dx + %dy * %dy);
   if(%length <= 0) return;
   %nx = -%dy * %width / (%length * 2);
   %ny = %dx * %width / (%length * 2);
   ModernHUDStock::quad(%x1 + %nx, %y1 + %ny, %x2 + %nx, %y2 + %ny,
                        %x2 - %nx, %y2 - %ny, %x1 - %nx, %y1 - %ny);
}

function ModernHUDStock::bar(%x, %y, %w, %h, %level, %color, %chrome)
{
   %fill = floor(%w * %level);
   if(%level > 0 && %fill < 1) %fill = 1;
   if(!%chrome)
   {
      ModernHUDStock::color("dark", 255);
      glRectangle(%x, %y, %w + 1, %h + 1);
      if(%fill <= 0) return;
      ModernHUDStock::color(%color, 255);
      glRectangle(%x + 1, %y + 1, floor((%w - 2) * %level + 0.5), %h - 1);
      return;
   }
   // The stock end caps are solid chevrons. Row spans crop the complete shape
   // rather than stretching it when the resource is nearly depleted.
   ModernHUDStock::color(%color, 255);
   for(%row = 0; %row < %h; %row++)
   {
      %inset = 0;
      if(%row < %h * 0.31) %inset = floor(6 * (1 - %row / (%h * 0.31)));
      if(%row >= %h * 0.69) %inset = floor(6 * (%row - %h * 0.69) / (%h * 0.31));
      %end = %w - %inset;
      if(%end > %fill) %end = %fill;
      if(%end > %inset) glRectangle(%x + %inset, %y + %row, %end - %inset, 1);
   }
}

function ModernHUDStock::drawHealth(%screen)
{
   ModernHUDStock::settings("Health");
   %w = ModernHUDStock::value("Health", "BarWidth");
   %h = ModernHUDStock::value("Health", "BarHeight");
   %boxH = %h + 22; if(%boxH < 35) %boxH = 35;
   %at = ModernHUDStock::place("healthHud", "ModernHUD::StockHealth", "top-left", 50, 7, %w + 46, %boxH, %screen);
   %x = getWord(%at, 0); %y = getWord(%at, 1);
   %level = $health / 100;
   if(%level < 0) %level = 0;
   if(%level > 1) %level = 1;
   %color = ModernHUDStock::value("Health", "Color");
   if(%color == "auto")
   {
      if(%level >= 0.67) %color = "green";
      else if(%level >= 0.33) %color = "yellow";
      else %color = "red";
   }
   %chrome = ModernHUDStock::on("Health", "Chrome");
   if(%chrome)
   {
      ModernHUDStock::vitalFrame("Health", %x, %y, %w + 46, %boxH, false);
      %icon = "HH_Icon_Green.bmp";
      if(%level <= 0) %icon = "HH_Icon_Red.bmp";
      else if(%level < 0.33)
      {
         %phase = floor(glTicks() / 512);
         if(!ModernHUDStock::on("Health", "Flash") || (%phase - floor(%phase / 2) * 2) == 0)
            %icon = "HH_Icon_Red.bmp";
      }
      ModernHUDStock::image(%x + 5, %y + 3, 24, 27, %icon);
   }
   ModernHUDStock::bar(%x + 37, %y + 10, %w, %h, %level, %color, %chrome);
   if(ModernHUDStock::on("Health", "Number"))
   {
      glSetFont("Verdana", 10);
      %text = floor(%level * 100) @ "%";
      ModernHUDStock::color("white", 255);
      glDrawString(%x + 37 + floor((%w - getWord(glGetStringDimensions(%text), 0)) / 2), %y + 9, %text);
   }
}

function ModernHUDStock::drawEnergy(%screen)
{
   ModernHUDStock::settings("Energy");
   %w = ModernHUDStock::value("Energy", "BarWidth");
   %h = ModernHUDStock::value("Energy", "BarHeight");
   %boxH = %h + 22; if(%boxH < 35) %boxH = 35;
   %at = ModernHUDStock::place("jetPackHud", "ModernHUD::StockEnergy", "top-left", 50, 28, %w + 46, %boxH, %screen);
   %x = getWord(%at, 0); %y = getWord(%at, 1);
   %level = $energy / 100;
   if(%level < 0) %level = 0;
   if(%level > 1) %level = 1;
   %chrome = ModernHUDStock::on("Energy", "Chrome");
   if(%chrome)
   {
      ModernHUDStock::vitalFrame("Energy", %x, %y, %w + 46, %boxH, true);
      ModernHUDStock::image(%x + 5, %y + 8, 24, 21, "HJ_Icon_Green.bmp");
   }
   ModernHUDStock::bar(%x + 37, %y + 10 + %chrome, %w, %h, %level, ModernHUDStock::value("Energy", "Color"), %chrome);
   if(ModernHUDStock::on("Energy", "Number"))
   {
      glSetFont("Verdana", 10);
      %text = floor(%level * 100) @ "%";
      ModernHUDStock::color("white", 255);
      glDrawString(%x + 37 + floor((%w - getWord(glGetStringDimensions(%text), 0)) / 2), %y + 9, %text);
   }
}

function ModernHUDStock::drawWeapon(%screen)
{
   ModernHUDStock::settings("Weapon");
   %n = getHudWeaponCount();
   if(%n <= 0)
   {
      ModernHUD::hide("ModernHUD::StockWeapon");
      return;
   }
   ModernHUDStock::font("Weapon");
   %fontH = getWord(glGetStringDimensions("888"), 1);
   %rowH = 18; if(%fontH > %rowH) %rowH = %fontH;
   %spacing = ModernHUDStock::value("Weapon", "Spacing");
   %pitch = %rowH + %spacing;
   %ammo = ModernHUDStock::on("Weapon", "Ammo");
   %textW = getWord(glGetStringDimensions("888"), 0) + 8;
   if(%textW < 32) %textW = 32;
   %w = %ammo ? 32 + %textW : 32;
   %h = %n * %pitch - %spacing + 14;
   %at = ModernHUDStock::place("weaponHud", "ModernHUD::StockWeapon", "top-left", 0, getWord(%screen, 1) * 0.705, %w, %h, %screen);
   %x = getWord(%at, 0); %y = getWord(%at, 1);
   ModernHUDStock::color("dark", ModernHUDStock::value("Weapon", "Background") * 2.55);
   glRectangle(%x + 2, %y + 7, 28, %h - 14);
   ModernHUDStock::quad(%x + 2, %y + 8, %x + 8, %y + 2, %x + 24, %y + 2, %x + 30, %y + 8);
   ModernHUDStock::quad(%x + 2, %y + %h - 8, %x + 30, %y + %h - 8, %x + 24, %y + %h - 2, %x + 8, %y + %h - 2);
   if(ModernHUDStock::on("Weapon", "Frame"))
   {
      ModernHUDStock::color(ModernHUDStock::value("Weapon", "Color"), 255);
      ModernHUDStock::quad(%x, %y + 7, %x + 7, %y, %x + 10, %y, %x, %y + 10);
      ModernHUDStock::quad(%x, %y + %h - 8, %x + 7, %y + %h - 1, %x + 10, %y + %h - 1, %x, %y + %h - 11);
      glRectangle(%x + 7, %y, 11, 2);
      glRectangle(%x + 7, %y + %h - 2, 11, 2);
      glRectangle(%x, %y + 7, 2, %h - 14);
   }
   %iy = %y + 7;
   for(%i = 0; %i < %n; %i++)
   {
      %info = getHudWeaponInfo(%i);
      if(%info != "")
      {
         %selected = getWord(%info, 1);
         ModernHUDStock::image(%x + 4, %iy, 24, 18, getHudWeaponInfo(%i, "icon"));
         if(%ammo && getWord(%info, 3))
         {
            %tx = %x + 32;
            ModernHUDStock::image(%tx, %iy, %textW, %rowH, "ammoSh.bmp");
            %count = getWord(%info, 2);
            if(%count < 0)
               ModernHUDStock::image(%tx + 4, %iy + 5, 13, 6, %selected ? "I_Infinity_on.bmp" : "I_Infinity_off.bmp");
            else
            {
               if(%count > 999) %count = 999;
               ModernHUDStock::color(%selected ? ModernHUDStock::value("Weapon", "Color") : "dim", 255);
               glDrawString(%tx + floor((%textW - getWord(glGetStringDimensions(%count), 0)) / 2), %iy, %count);
            }
         }
      }
      %iy += %pitch;
   }
}

function ModernHUDStock::pad2(%value)
{
   if(%value < 10) return "0" @ %value;
   return %value;
}

function ModernHUDStock::drawClock(%screen)
{
   ModernHUDStock::settings("Clock");
   %time = getHudTimer(); if(%time < 0) %time = -%time;
   %whole = floor(%time);
   %hours = floor(%whole / 3600);
   %mins = floor((%whole - %hours * 3600) / 60);
   %secs = %whole - %hours * 3600 - %mins * 60;
   %text = ModernHUDStock::pad2(%hours) @ ":" @ ModernHUDStock::pad2(%mins) @ ":" @ ModernHUDStock::pad2(%secs);
   %sample = "88:88:88";
   if(ModernHUDStock::on("Clock", "Tenths"))
   {
      %text = %text @ "." @ floor((%time - %whole) * 10);
      %sample = %sample @ ".8";
   }
   ModernHUDStock::font("Clock");
   %size = glGetStringDimensions(%sample);
   %w = getWord(%size, 0) + 12; %h = getWord(%size, 1) + 6;
   %at = ModernHUDStock::place("clockHud", "ModernHUD::StockClock", "bottom-left", 0, 0, %w, %h, %screen);
   %x = getWord(%at, 0); %y = getWord(%at, 1);
   ModernHUDStock::color("dark", ModernHUDStock::value("Clock", "Background") * 2.55);
   glRectangle(%x, %y, %w, %h);
   if(ModernHUDStock::on("Clock", "Frame"))
   {
      ModernHUDStock::color("green", 255);
      glRectangle(%x, %y, 2, %h);
      glRectangle(%x, %y, 6, 1);
      glRectangle(%x, %y + %h - 1, 6, 1);
      glRectangle(%x + %w - 2, %y, 2, %h);
      glRectangle(%x + %w - 6, %y, 6, 1);
      glRectangle(%x + %w - 6, %y + %h - 1, 6, 1);
   }
   ModernHUDStock::color(ModernHUDStock::value("Clock", "Color"), 255);
   glDrawString(%x + 5, %y + 2, %text);
}

// CompassHud's authored N/E/W/S polylines and diagonal ticks. Keeping line
// geometry instead of font glyphs makes the dial rotate exactly with heading.
function ModernHUDStock::compassSegments()
{
   %i = 0;
   %i = ModernHUDStock::segment(%i, -2,-18, -2,-24); %i = ModernHUDStock::segment(%i, -2,-24, 2,-18); %i = ModernHUDStock::segment(%i, 2,-18, 2,-24);
   %i = ModernHUDStock::segment(%i, 18,2, 18,-2); %i = ModernHUDStock::segment(%i, 18,-2, 24,-2); %i = ModernHUDStock::segment(%i, 24,-2, 24,2);
   %i = ModernHUDStock::segment(%i, 21,-2, 21,2);
   %i = ModernHUDStock::segment(%i, -24,4, -18,2); %i = ModernHUDStock::segment(%i, -18,2, -24,0); %i = ModernHUDStock::segment(%i, -24,0, -18,-2); %i = ModernHUDStock::segment(%i, -18,-2, -24,-4);
   %i = ModernHUDStock::segment(%i, 2,20, 1,19); %i = ModernHUDStock::segment(%i, 1,19, -1,19); %i = ModernHUDStock::segment(%i, -1,19, -2,20);
   %i = ModernHUDStock::segment(%i, -2,20, -2,21); %i = ModernHUDStock::segment(%i, -2,21, -1,22); %i = ModernHUDStock::segment(%i, -1,22, 1,22);
   %i = ModernHUDStock::segment(%i, 1,22, 2,23); %i = ModernHUDStock::segment(%i, 2,23, 2,24); %i = ModernHUDStock::segment(%i, 2,24, 1,25);
   %i = ModernHUDStock::segment(%i, 1,25, -1,25); %i = ModernHUDStock::segment(%i, -1,25, -2,24);
   %i = ModernHUDStock::segment(%i, 15,-14, 19,-18); %i = ModernHUDStock::segment(%i, 15,14, 19,18);
   %i = ModernHUDStock::segment(%i, -14,14, -18,18); %i = ModernHUDStock::segment(%i, -14,-14, -18,-18);
   $ModernHUDStock::SegmentCount = %i;
}

function ModernHUDStock::segment(%i, %ax, %ay, %bx, %by)
{
   $ModernHUDStock::Segment[%i] = %ax @ " " @ %ay @ " " @ %bx @ " " @ %by;
   return %i + 1;
}

function ModernHUDStock::drawCompass(%screen)
{
   ModernHUDStock::settings("Compass");
   if($ModernHUDStock::SegmentCount == "") ModernHUDStock::compassSegments();
   %at = ModernHUDStock::place("compassHud", "ModernHUD::StockCompass", "top-right", 0, getWord(%screen, 1) * 0.348, 64, 64, %screen);
   %x = getWord(%at, 0); %y = getWord(%at, 1);
   if(ModernHUDStock::on("Compass", "Frame")) ModernHUDStock::image(%x, %y, 64, 64, "compass.bmp");
   %cx = %x + 31.5; %cy = %y + 31.5;
   %s = $compassSin; %c = $compassCos;
   if(%s == "") %s = 0;
   if(%c == "") %c = 1;
   %lineWidth = ModernHUDStock::value("Compass", "LineWidth");
   ModernHUDStock::color(ModernHUDStock::value("Compass", "Color"), 255);
   for(%i = 0; %i < $ModernHUDStock::SegmentCount; %i++)
   {
      %seg = $ModernHUDStock::Segment[%i];
      %ax = getWord(%seg, 0); %ay = getWord(%seg, 1); %bx = getWord(%seg, 2); %by = getWord(%seg, 3);
      ModernHUDStock::line(%cx + %ax * %c - %ay * %s, %cy + %ax * %s + %ay * %c,
                          %cx + %bx * %c - %by * %s, %cy + %bx * %s + %by * %c, %lineWidth);
   }
   ModernHUDStock::color("red", 255);
   glRectangle(%cx - %lineWidth / 2, %cy - 29, %lineWidth, 15);
   if(ModernHUDStock::on("Compass", "Waypoint"))
   {
      %waypoint = getHudCompassWaypoint();
      %distance = "000";
      ModernHUDStock::color(ModernHUDStock::value("Compass", "Color"), 255);
      if(%waypoint != "")
      {
         %s = getWord(%waypoint, 0); %c = getWord(%waypoint, 1);
         %distance = getWord(%waypoint, 2);
         ModernHUDStock::quad(%cx + 24 * %s, %cy - 24 * %c,
                              %cx + 6 * %c + 16 * %s, %cy + 6 * %s - 16 * %c,
                              %cx - 6 * %c + 16 * %s, %cy - 6 * %s - 16 * %c,
                              %cx - 6 * %c + 16 * %s, %cy - 6 * %s - 16 * %c);
      }
      ModernHUDStock::font("Compass");
      %size = glGetStringDimensions(%distance);
      glDrawString(%cx + 1 - getWord(%size, 0) / 2, %cy - getWord(%size, 1) + 3, %distance);
   }
}

function ModernHUDStock::drawSensor(%screen)
{
   ModernHUDStock::settings("Sensor");
   %label = ModernHUDStock::on("Sensor", "Label");
   %at = ModernHUDStock::place("sensorHUD", "ModernHUD::StockSensor", "top-right", 384, 0, %label ? 86 : 32, 32, %screen);
   %x = getWord(%at, 0); %y = getWord(%at, 1);
   if(ModernHUDStock::on("Sensor", "Frame")) ModernHUDStock::image(%x, %y, 32, 32, "ping.bmp");
   %ping = $sensorPing; %bmp = ""; %text = "CLEAR";
   if(%ping == 2)
   {
      %bmp = "HP_Supressed.bmp"; %bw = 24; %bh = 15; %text = "JAMMED";
      $ModernHUDStock::PingStart = "";
   }
   else if(%ping == 1)
   {
      %text = "PINGED";
      %now = glTicks();
      if($ModernHUDStock::PingStart == "" || %now < $ModernHUDStock::PingStart) $ModernHUDStock::PingStart = %now;
      %step = floor((%now - $ModernHUDStock::PingStart) / 100);
      %frame = %step - floor(%step / 6) * 6;
      if(!ModernHUDStock::on("Sensor", "Animate")) %frame = 2;
      if(%frame == 0) { %bmp = "HP_PingedLo.bmp"; %bw = 11; %bh = 5; }
      else if(%frame == 1) { %bmp = "HP_PingedMed.bmp"; %bw = 18; %bh = 11; }
      else { %bmp = "HP_PingedHi.bmp"; %bw = 24; %bh = 15; }
   }
   else $ModernHUDStock::PingStart = "";
   if(%bmp != "") ModernHUDStock::image(%x + floor((32 - %bw) / 2), %y + floor((23 - %bh) / 2), %bw, %bh, %bmp);
   if(%label)
   {
      glSetFont("Verdana", 10);
      ModernHUDStock::color(%ping == 1 ? "red" : "green", 255);
      glDrawString(%x + 35, %y + 6, %text);
   }
}
