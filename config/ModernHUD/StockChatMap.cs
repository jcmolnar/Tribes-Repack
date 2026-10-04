// Shared ScriptGL chat and minimap parts. Native code supplies state and the
// terrain texture only; every visible HUD element is authored here.

function ModernHUDStock::chatMapSettings()
{
   ModernHUD::setting("int", ModernHUD::packSettingKey("ChatWidth"), "Chat width (0 = config)", "0", "0|1600|20", "", "ModernHUD::StockChat");
   ModernHUD::setting("int", ModernHUD::packSettingKey("ChatLines"), "Chat lines (0 = config)", "0", "0|40|1", "", "ModernHUD::StockChat");
   ModernHUD::setting("enum", ModernHUD::packSettingKey("ChatBackground"), "Chat background", "-1", "Config|-1;Off|0;On|1", "", "ModernHUD::StockChat");
   ModernHUD::setting("int", ModernHUD::packSettingKey("ChatBackgroundAlpha"), "Chat background opacity (%)", "45", "0|100|5", "", "ModernHUD::StockChat");
   ModernHUD::setting("enum", ModernHUD::packSettingKey("ChatBorder"), "Chat brackets", "-1", "Config|-1;Off|0;On|1", "", "ModernHUD::StockChat");
   ModernHUD::setting("int", ModernHUD::packSettingKey("MapSize"), "Minimap size (0 = config)", "0", "0|1024|16", "", "ModernHUD::StockMinimap");
   ModernHUD::setting("bool", ModernHUD::packSettingKey("MapTerrain"), "Minimap terrain", "1", "", "", "ModernHUD::StockMinimap");
   ModernHUD::setting("bool", ModernHUD::packSettingKey("MapBorder"), "Minimap border", "1", "", "", "ModernHUD::StockMinimap");
   ModernHUD::setting("int", ModernHUD::packSettingKey("MapMarkerSize"), "Minimap marker size", "3", "1|12|1", "", "ModernHUD::StockMinimap");
}

function ModernHUDStock::chatMapValue(%leaf)
{
   return getVariable(ModernHUD::packSettingKey(%leaf));
}

function ModernHUDStock::ink(%rgb, %alpha)
{
   glColor4ub(getWord(%rgb, 0), getWord(%rgb, 1), getWord(%rgb, 2), ModernHUD::alphaOf(%alpha));
}

function ModernHUDStock::chatInk(%type)
{
   if(%type == 1 || %type == 4) return "255 95 95";
   if(%type == 2) return "255 226 80";
   if(%type == 3) return "110 235 120";
   return "235 242 250";
}

function ModernHUDStock::chatText(%x, %y, %text, %type)
{
   ModernHUDStock::ink("0 0 0", 170);
   glDrawString(%x + 1, %y + 2, %text);
   ModernHUDStock::ink(ModernHUDStock::chatInk(%type), 255);
   glDrawString(%x, %y + 1, %text);
}

function ModernHUDStock::drawChat(%screen)
{
   %font = $pref::ChatFont;
   if(%font == "") %font = "Segoe UI";
   %px = $pref::ChatFontSize;
   if(%px <= 0) %px = floor(14 * getWord(%screen, 1) / 720 + 0.5);
   %px = max(11, min(48, %px));
   %weight = 600;
   if(String::ICompare($pref::ChatFontBold, "0") == 0) %weight = 400;
   glSetFontWeight(%font, %px, %weight);
   %lh = getWord(glGetStringDimensions("M"), 1) + 2;
   %ext = Control::getExtent(chatDisplayHud);
   %w = ModernHUDStock::chatMapValue("ChatWidth");
   if(%w <= 0) %w = getWord(%ext, 0);
   if(%w < 48) %w = min(440, getWord(%screen, 0) - 24);
   %lines = ModernHUDStock::chatMapValue("ChatLines");
   %h = getWord(%ext, 1);
   %demo = $ModernHUD::Preview && $ModernHUD::PvDemo;
   if(!%demo)
   {
      ChatDisplay::snapshot(%w - 24, %lines);
      $ModernHUDStock::ChatSnapshotReady = true;
      %lh = $MHChat::LineHeight;
      if(%lines <= 0) %lines = $MHChat::Lines;
      glSetFontWeight($MHChat::Font, $MHChat::FontSize, $MHChat::FontWeight);
   }
   if(%lines <= 0) %lines = 6;
   if(ModernHUDStock::chatMapValue("ChatLines") > 0 || %h < %lh + 6) %h = %lines * %lh + 6;
   %at = ModernHUDStock::place(chatDisplayHud, "ModernHUD::StockChat", "top-left", 8, 24, %w, %h, %screen);
   %x = getWord(%at, 0); %y = getWord(%at, 1);
   %hidden = !%demo && $MHChat::Hidden;
   %background = ModernHUDStock::chatMapValue("ChatBackground");
   if(%background < 0) %background = !$xChat::TransChat;
   if(%background && !%hidden)
   {
      ModernHUDStock::ink("0 0 0", ModernHUDStock::chatMapValue("ChatBackgroundAlpha") * 2.55);
      glRectangle(%x, %y, %w, %h);
   }
   %border = ModernHUDStock::chatMapValue("ChatBorder");
   if(%border < 0) %border = !$pref::Groove::NoHudBrackets && !$GreenLines::Dis && !$mj::greenlines;
   if(%border && !%hidden)
   {
      ModernHUDStock::ink("40 210 120", 210);
      glRectangle(%x, %y, 1, %h); glRectangle(%x + %w - 1, %y, 1, %h);
      glRectangle(%x, %y, 5, 1); glRectangle(%x, %y + %h - 1, 5, 1);
      glRectangle(%x + %w - 5, %y, 5, 1); glRectangle(%x + %w - 5, %y + %h - 1, 5, 1);
   }
   %bottom = %y + %h - 4;
   if(%demo)
   {
      ModernHUDStock::chatText(%x + 6, %bottom - %lh, "Teammate: flag is out, cover me!", 3);
      ModernHUDStock::chatText(%x + 6, %bottom - 2 * %lh, "You: on my way", 2);
      ModernHUDStock::chatText(%x + 6, %bottom - 3 * %lh, "Match starts in 10 seconds.", 0);
   }
   else
   {
      // CommandVisible, not CommandActive: a hidden command ($xChat::HideCmdMsg) takes no row
      if($MHChat::CommandVisible) %bottom = %bottom - %lh;
      for(%i = 0; %i < $MHChat::RowCount; %i++)
      {
         %ry = %bottom - (%i + 1) * %lh + $MHChat::ScrollOffset;
         if(%ry >= %y && %ry + %lh <= %bottom)
         {
            ModernHUDStock::chatText(%x + 6, %ry, $MHChat::RowText[%i], $MHChat::RowType[%i]);
            %icon = $MHChat::RowIcon[%i];
            if(%icon != "")
            {
               %dim = glGetImageDimensions(%icon);
               glDrawImage(%x + 6 - $xChat::Icon::xOffset - getWord(%dim, 0), %ry + $xChat::Icon::yOffset, 0, 0, %icon, ModernHUD::alphaOf(255));
            }
         }
      }
      if($MHChat::Command != "") ModernHUDStock::chatText(%x + 6, %y + %h - %lh, $MHChat::Command, $MHChat::CommandType);
      if($MHChat::Page > 0 && !%hidden) ModernHUDStock::chatText(%x + %w - 18, %y + %h - %lh, "v", 3);
   }
}

// Called independently of chat-log visibility: hiding the log must not hide
// Y/T input or the native V menu while those responders are active.
function ModernHUDStock::drawChatInput(%screen)
{
   if($ModernHUD::Preview && $ModernHUD::PvDemo)
   {
      $ModernHUDStock::ChatSnapshotReady = false;
      return;
   }
   if(!$ModernHUDStock::ChatSnapshotReady) ChatDisplay::snapshot(428, 0);
   $ModernHUDStock::ChatSnapshotReady = false;
   glPartScale(0, 0, 1); glPartStyle(0, 1);
   glSetFontWeight($MHChat::Font, $MHChat::FontSize, $MHChat::FontWeight);
   %lh = $MHChat::LineHeight;
   %r = $ModernHUD::ScriptStockRect[chatDisplayHud];
   %x = getWord(%r, 0); %y = getWord(%r, 1); %h = getWord(%r, 3) * getWord(%r, 4);
   if(%r == "")
   {
      %pos = Control::getPosition(chatDisplayHud);
      %x = getWord(%pos, 0); %y = getWord(%pos, 1);
      %h = getWord(Control::getExtent(chatDisplayHud), 1);
   }
   %my = %y + %h + 2;
   if(%my + $MHChat::MenuCount * %lh > getWord(%screen, 1)) %my = max(0, %y - $MHChat::MenuCount * %lh - 8);
   for(%i = 0; %i < $MHChat::MenuCount; %i++) ModernHUDStock::chatText(%x + 6, %my + %i * %lh, $MHChat::MenuText[%i], 3);
   if($MHChat::InputOpen)
      {
         // The composer keeps its native placement and responder. Chat log part
         // scaling must not move its paint away from the native mouse hit area.
         glPartScale(0, 0, 1); glPartStyle(0, 1);
         %ix = getWord($MHChat::InputRect, 0); %iy = getWord($MHChat::InputRect, 1);
         %iw = getWord($MHChat::InputRect, 2);
         %rows = min(max(1, $MHChat::InputCount), max(1, floor((getWord(%screen, 1) - 12) / %lh)));
         %first = max(0, min($MHChat::InputCount - %rows, floor($MHChat::InputCaretY / %lh) - %rows + 1));
         %ih = %rows * %lh + 6;
         if(%iy + %ih > getWord(%screen, 1)) %iy = max(0, getWord(%screen, 1) - %ih - 4);
         if(!$ModernHUD::Preview)
         {
            $MHChat::InputDrawRect = floor(%ix) @ " " @ floor(%iy) @ " " @ floor(%iw) @ " " @ floor(%ih);
            $MHChat::InputDrawStart = %first;
         }
         if(!$xChat::TransInput)
         {
            ModernHUDStock::ink("0 0 0", 150); glRectangle(%ix, %iy, %iw, %ih);
         }
         if(($pref::ChatInputBrackets == "" || $pref::ChatInputBrackets) && !$pref::Groove::NoHudBrackets)
         {
            ModernHUDStock::ink("40 210 120", 210);
            glRectangle(%ix, %iy, 1, %ih); glRectangle(%ix + %iw - 1, %iy, 1, %ih);
            glRectangle(%ix, %iy, 5, 1); glRectangle(%ix, %iy + %ih - 1, 5, 1);
            glRectangle(%ix + %iw - 5, %iy, 5, 1); glRectangle(%ix + %iw - 5, %iy + %ih - 1, 5, 1);
         }
         %type = 2; if($pref::msgChannel > 0) %type = 3;
         for(%i = %first; %i < %first + %rows; %i++) ModernHUDStock::chatText(%ix + 12, %iy + 3 + (%i - %first) * %lh, $MHChat::InputRow[%i], %type);
         if(floor(glTicks() / 500) % 2 == 0)
         {
            ModernHUDStock::ink("110 235 120", 255);
            glRectangle(%ix + 12 + $MHChat::InputCaretX, %iy + 3 + $MHChat::InputCaretY - %first * %lh, 1, %lh - 2);
         }
   }
}

function ModernHUDStock::drawMinimap(%screen)
{
   %size = ModernHUDStock::chatMapValue("MapSize");
   if(%size <= 0) %size = $pref::miniMapWidth;
   %size = max(48, min(1024, %size));
   %full = %size + 16;
   %at = ModernHUDStock::place(Minimap, "ModernHUD::StockMinimap", "top-right", -8, 8, %full, %full, %screen);
   %x = getWord(%at, 0) + 8; %y = getWord(%at, 1) + 8;
   %cx = %x + %size / 2; %cy = %y + %size / 2;
   %square = $pref::miniMapSquare;
   %demo = $ModernHUD::Preview && $ModernHUD::PvDemo;
   %a = $pref::miniMapAlpha;
   if(%a == "") %a = $pref::miniMapOpacity;
   if(%a == "") %a = 1;
   if(%a > 1) %a = %a / 255;
   %terrain = 0;
   if(!%demo && ModernHUDStock::chatMapValue("MapTerrain")) %terrain = glDrawMinimapTerrain(%x, %y, %size, %square, ModernHUD::alphaOf(%a * 255) / 255);
   if(!%terrain)
   {
      ModernHUDStock::ink("12 22 18", %a * 255);
      if(%square) glRectangle(%x, %y, %size, %size);
      else
         for(%row = 0; %row < %size; %row++)
         {
            %dy = %row - %size / 2;
            %hw = sqrt(max(0, %size * %size / 4 - %dy * %dy));
            glRectangle(%cx - %hw, %y + %row, %hw * 2, 1);
         }
   }
   if(ModernHUDStock::chatMapValue("MapBorder"))
   {
      ModernHUDStock::ink("235 242 250", 230);
      if(%square)
      {
         glRectangle(%x, %y, %size, 1); glRectangle(%x, %y + %size - 1, %size, 1);
         glRectangle(%x, %y, 1, %size); glRectangle(%x + %size - 1, %y, 1, %size);
      }
      else
         for(%i = 0; %i < 64; %i++)
            ModernHUDStock::line(%cx + mCos(%i * 5.625) * %size / 2, %cy + mSin(%i * 5.625) * %size / 2, %cx + mCos((%i + 1) * 5.625) * %size / 2, %cy + mSin((%i + 1) * 5.625) * %size / 2, 1.5);
   }
   if(!%demo) MiniMap::snapshot(%size);
   %sin = $MHMap::Sin; %cos = $MHMap::Cos;
   if(%demo) { %sin = 0; %cos = 1; }
   if($pref::miniMapCompass == "" || $pref::miniMapCompass)
   {
      %px = max(9, min(18, floor(%size / 14)));
      glSetFont("Verdana", %px);
      for(%i = 0; %i < 4; %i++)
      {
         %dx = getWord("0 1 0 -1", %i); %dy = getWord("1 0 -1 0", %i);
         %rx = %dx * %cos - %dy * %sin; %ry = %dx * %sin + %dy * %cos;
         %edge = %size / 2 - 1;
         if(%square) %edge = %edge / max(abs(%rx), abs(%ry));
         %tx = %cx + %rx * (%edge - %px * 0.75);
         %ty = %cy - %ry * (%edge - %px * 0.75) - %px / 2;
         %letter = getWord("N E S W", %i);
         %tx = %tx - getWord(glGetStringDimensions(%letter), 0) / 2;
         ModernHUDStock::ink("0 0 0", 180); glDrawString(%tx + 1, %ty + 1, %letter);
         ModernHUDStock::ink("255 255 255", 235); glDrawString(%tx, %ty, %letter);
      }
   }
   if($pref::miniMapHeadingLine == "" || $pref::miniMapHeadingLine)
   {
      %hx = $MHMap::HeadingX; %hy = $MHMap::HeadingY;
      if(%demo) { %hx = 0.3; %hy = -0.95; }
      ModernHUDStock::ink("110 235 120", 255);
      ModernHUDStock::line(%cx, %cy, %cx + %hx * %size / 6, %cy + %hy * %size / 6, 1.5);
   }
   %count = $MHMap::Count; if(%demo) %count = 4;
   for(%i = 0; %i < %count; %i++)
   {
      %mx = $MHMap::X[%i]; %my = $MHMap::Y[%i]; %kind = $MHMap::Kind[%i];
      if(%demo) { %mx = getWord("0.5 0.31 0.72 0.42", %i); %my = getWord("0.5 0.33 0.62 0.76", %i); %kind = %i; }
      %mx = %x + %mx * %size; %my = %y + %my * %size;
      %color = "255 255 255"; %icon = "minimap.waypoint.png";
      if(%kind == 1) { %color = "110 235 120"; %icon = "minimap.friend.png"; }
      if(%kind == 2) { %color = "255 80 80"; %icon = "minimap.foe.png"; }
      if(%kind == 3) { %color = "255 100 220"; %icon = "minimap.friend.flag.png"; }
      %marker = ModernHUDStock::chatMapValue("MapMarkerSize");
      %dim = "";
      %icon = MiniMap::markerArt(%kind);
      if(%icon != "") %dim = glGetImageDimensions(%icon);
      if(getWord(%dim, 0) > 0)
         glDrawImage(%mx - getWord(%dim, 0) / 2, %my - getWord(%dim, 1) / 2, 0, 0, %icon, ModernHUD::alphaOf(255));
      else
      {
         if(%kind == 3) %marker = %marker + 2;
         ModernHUDStock::ink(%color, 255); glRectangle(%mx - %marker / 2, %my - %marker / 2, %marker, %marker);
      }
   }
   // The existing minimapZoom command owns the player's persistent zoom. Mouse
   // hit testing follows the transformed part rather than the old native rect.
   %mouse = glMousePos();
   if(%mouse != "" || $ModernHUD::Preview)
   {
      %bs = max(12, min(20, floor(%size / 10)));
      %scale = getWord($ModernHUD::ScriptStockRect[Minimap], 4);
      if(%scale <= 0) %scale = 1;
      %lx = (getWord(%mouse, 0) - getWord(%at, 0)) / %scale + getWord(%at, 0);
      %ly = (getWord(%mouse, 1) - getWord(%at, 1)) / %scale + getWord(%at, 1);
      %down = getWord(%mouse, 2);
      for(%i = 0; %i < 2; %i++)
      {
         %bx = %x + 3; %by = %y + %size - 3 - %bs * (2 - %i) - (1 - %i) * 2;
         ModernHUDStock::ink("0 0 0", 200); glRectangle(%bx, %by, %bs, %bs);
         ModernHUDStock::ink("235 242 250", 255);
         glRectangle(%bx + 3, %by + %bs / 2, %bs - 6, 1);
         if(%i == 0) glRectangle(%bx + %bs / 2, %by + 3, 1, %bs - 6);
         if(!$ModernHUD::Preview && %down && !$ModernHUDStock::MapMouseDown && ModernHUD::mHit(%lx, %ly, %bx, %by, %bs, %bs)) minimapZoom(1 - %i * 2);
      }
      if(!$ModernHUD::Preview) $ModernHUDStock::MapMouseDown = %down;
   }
}
