// The native crosshair still acquires targets and draws nameplates. Its read-only
// snapshot selects the original regular/sniper art and animated zoom geometry;
// this part owns only the reticle pixels and their customizable appearance.

function ModernHUDStock::reticleSettings()
{
   %kind = "Reticle";
   if(ModernHUD::hasSetting(ModernHUDStock::key(%kind, "Shape")))
   {
      ModernHUD::partTint("ModernHUD::StockReticle");
      return;
   }
   // A pack/preset reload supplies a complete position+scale pair. Do not
   // mistake that restored pair for an interactive resize of the old handle.
   %q = ModernHUD::qualify("ModernHUD::StockReticle");
   $ModernHUDStock::ReticleSource[%q] = "";
   $ModernHUDStock::ReticleSourceScale[%q] = "";
   ModernHUDStock::setting(%kind, "enum", "Shape", "Reticle shape", "texture", "Original texture|texture;Cross|cross;Dot|dot;Ring|ring");
   ModernHUDStock::setting(%kind, "enum", "Color", "Shape and scope color", "green", "Green|green;Cyan|cyan;White|white;Amber|yellow;Red|red;Blue|blue");
   ModernHUDStock::setting(%kind, "int", "Thickness", "Line thickness (px)", "1", "1|8|1");
   ModernHUDStock::setting(%kind, "int", "Length", "Cross arm length (px)", "7", "1|32|1");
   ModernHUDStock::setting(%kind, "int", "Gap", "Cross center gap (px)", "3", "0|20|1");
   ModernHUDStock::setting(%kind, "int", "DotSize", "Dot size (px)", "3", "1|20|1");
   ModernHUDStock::setting(%kind, "int", "Radius", "Ring radius (px)", "8", "2|40|1");
   ModernHUDStock::setting(%kind, "bool", "NativeZoom", "Keep original zoom reticle", "1", "");
   ModernHUDStock::setting(%kind, "bool", "ZoomGuide", "Show zoom adjustment guide", "1", "");
   ModernHUD::partTint("ModernHUD::StockReticle");
}

function ModernHUDStock::reticleShape(%shape, %cx, %cy)
{
   %t = ModernHUDStock::value("Reticle", "Thickness");
   ModernHUDStock::color(ModernHUDStock::value("Reticle", "Color"), 255);
   if(%shape == "dot")
   {
      %d = ModernHUDStock::value("Reticle", "DotSize");
      glRectangle(%cx - floor(%d / 2), %cy - floor(%d / 2), %d, %d);
   }
   else if(%shape == "ring")
   {
      %radius = ModernHUDStock::value("Reticle", "Radius");
      for(%i = 0; %i < 48; %i++)
         ModernHUDStock::line(%cx + mCos(%i * 7.5) * %radius,
                              %cy + mSin(%i * 7.5) * %radius,
                              %cx + mCos((%i + 1) * 7.5) * %radius,
                              %cy + mSin((%i + 1) * 7.5) * %radius, %t);
   }
   else
   {
      %gap = ModernHUDStock::value("Reticle", "Gap");
      %len = ModernHUDStock::value("Reticle", "Length");
      %half = floor(%t / 2);
      glRectangle(%cx - %gap - %len, %cy - %half, %len, %t);
      glRectangle(%cx + %gap, %cy - %half, %len, %t);
      glRectangle(%cx - %half, %cy - %gap - %len, %t, %len);
      glRectangle(%cx - %half, %cy + %gap, %t, %len);
   }
}

function ModernHUDStock::reticleScope(%cx, %cy, %hw, %hh)
{
   // Native scope: a one-pixel cross with three-pixel outer thirds. Respect
   // the animated extents from the camera without changing targeting state.
   %t = ModernHUDStock::value("Reticle", "Thickness");
   %half = floor(%t / 2);
   %wide = %t + 2;
   ModernHUDStock::color(ModernHUDStock::value("Reticle", "Color"), 255);
   glRectangle(%cx - %half, %cy - %hh, %t, 2 * %hh + 1);
   glRectangle(%cx - %hw, %cy - %half, 2 * %hw + 1, %t);
   glRectangle(%cx - %half - 1, %cy - %hh, %wide, %hh - floor(%hh / 3) + 1);
   glRectangle(%cx - %half - 1, %cy + floor(%hh / 3), %wide, %hh - floor(%hh / 3) + 1);
   glRectangle(%cx - %hw, %cy - %half - 1, %hw - floor(%hw / 3) + 1, %wide);
   glRectangle(%cx + floor(%hw / 3), %cy - %half - 1, %hw - floor(%hw / 3) + 1, %wide);
}

function ModernHUDStock::reticleGuide(%cx, %cy, %left, %top, %right, %bottom, %factor, %low)
{
   %length = %low ? 4 : 7;
   %t = ModernHUDStock::value("Reticle", "Thickness");
   ModernHUDStock::color(ModernHUDStock::value("Reticle", "Color"), 255);
   glRectangle(%left, %top, %length + 1, %t);
   glRectangle(%left, %top, %t, %length + 1);
   glRectangle(%right - %length, %top, %length + 1, %t);
   glRectangle(%right - %t + 1, %top, %t, %length + 1);
   glRectangle(%left, %bottom - %t + 1, %length + 1, %t);
   glRectangle(%left, %bottom - %length, %t, %length + 1);
   glRectangle(%right - %length, %bottom - %t + 1, %length + 1, %t);
   glRectangle(%right - %t + 1, %bottom - %length, %t, %length + 1);
   %font = %low ? "sf_white_5.pft" : "sf_white_7.pft";
   %offset = %low ? 18 : 30;
   glDrawMarkup(%cx - 100, %cy + %offset, 200,
                "<jc><f:" @ %font @ ":ffffff>ZOOM: " @ %factor @ "x", ModernHUD::alphaOf(255));
}

// Read the position in REAL canvas units, never a projected preview rectangle.
function ModernHUDStock::reticleSource(%realScreen)
{
   %name = "ModernHUD::StockReticle";
   %q = ModernHUD::qualify(%name);
   %handle = $ModernHUD::Handle[%name];
   %source = "";
   %live = isObject(%handle);
   if(%live)
   {
      %pg = Control::getExtent(playGui);
      %live = getWord(%pg, 0) == getWord(%realScreen, 0) && getWord(%pg, 1) == getWord(%realScreen, 1);
   }
   if(%live)
   {
      // During play Control::getPosition answers "" for a retained handle (measured);
      // the native handle publishes its real position here every render, the same
      // fallback Framework.cs uses. Without it play never saw a source, so a resize
      // took the uncompensated path and moved the aim point.
      %source = Control::getPosition(%handle);
      if(%source == "")
         %source = $ModernHUD::HandlePos[%name];
   }
   else if(!ModernHUD::isEmptyLayout($pref::hudPositions[%q]))
   {
      String::Explode($pref::hudPositions[%q], "||", "reticleSource");
      %source = $reticleSource[0];
   }
   if($ModernHUD::ResetPending[%name] != "" ||
      (%live && $ModernHUD::AppliedReset[%name] != $ModernHUD::ResetGeneration))
      %source = "";
   if(%source != "")
      %source = (getWord(%source, 0) + 0) @ " " @ (getWord(%source, 1) + 0);
   return %source;
}

// Pure resolver shared by drawing and preset capture. Compensate BEFORE the
// responsive projection or bounds check: using the new size with the old origin
// can incorrectly move a reticle from an edge third into the center third.
function ModernHUDStock::reticleOrigin(%source, %screen, %box, %scale)
{
   %q = ModernHUD::qualify("ModernHUD::StockReticle");
   if(%source != "")
   {
      %oldScale = %scale;
      if(%source == $ModernHUDStock::ReticleSource[%q] && $ModernHUDStock::ReticleSourceScale[%q] != "")
         %oldScale = $ModernHUDStock::ReticleSourceScale[%q];
      %delta = %box * (%oldScale - %scale) / 2;
      %at = (getWord(%source, 0) + %delta) @ " " @ (getWord(%source, 1) + %delta);
   }
   else
   {
      %anchor = $pref::ModernHUD::PartAnchor[%q];
      if(%anchor == "") %anchor = "center";
      %at = ModernHUD::place(%anchor, 0, 0, %box * %scale, %box * %scale, %screen);
   }
   return ModernHUD::clampPartPos(%at, %box * %scale, %box * %scale, getWord(%screen, 0), getWord(%screen, 1));
}

// Read-only native preset callback: "x y displayedWidth displayedHeight realW realH" in
// real GUI pixels. Also valid for scratch comparisons. No preference, handle,
// or resize-cache writes, even when the current preview uses another resolution.
function ModernHUDStock::reticleCapture(%realScreen)
{
   if(!$ModernHUD::ScriptStock[crosshairHud]) return "";
   if(%realScreen == "") %realScreen = $ModernHUDStock::ReticleRealScreen;
   if(%realScreen == "") %realScreen = $ModernHUD::PreviewReal;
   if(%realScreen == "") %realScreen = Control::getExtent(playGui);
   if(getWord(%realScreen, 0) <= 0 || getWord(%realScreen, 1) <= 0) return "";
   %q = ModernHUD::qualify("ModernHUD::StockReticle");
   // scaleOfPart may import an old unqualified key; capture must remain pure.
   %scale = $pref::hudScale[%q];
   if(%scale == "") %scale = 1;
   %scale = ModernHUD::scaleOf(%scale);
   %at = ModernHUDStock::reticleOrigin(ModernHUDStock::reticleSource(%realScreen), %realScreen, 64, %scale);
   return %at @ " " @ (64 * %scale) @ " " @ (64 * %scale) @ " " @ %realScreen;
}

function ModernHUDStock::reticlePlace(%screen, %box, %scale)
{
   %name = "ModernHUD::StockReticle";
   %q = ModernHUD::qualify(%name);
   %realScreen = $ModernHUD::Preview ? $ModernHUD::PreviewReal : %screen;
   $ModernHUDStock::ReticleRealScreen = %realScreen;
   %source = ModernHUDStock::reticleSource(%realScreen);
   // Keep the source scale across read-only preview frames. A drag gives us a
   // new source position; a reset uses the authored anchor at the current size.
   if(%source == "" || %source != $ModernHUDStock::ReticleSource[%q])
   {
      $ModernHUDStock::ReticleSource[%q] = %source;
      $ModernHUDStock::ReticleSourceScale[%q] = %scale;
   }
   %oldScale = $ModernHUDStock::ReticleSourceScale[%q];
   if(%oldScale == "") %oldScale = %scale;
   %realAt = ModernHUDStock::reticleOrigin(%source, %realScreen, %box, %scale);

   %anchor = $pref::ModernHUD::PartAnchor[%q];
   if(%anchor == "") %anchor = "center";
   %extra = %box * (%scale - 1);
   %dx = 0; %dy = 0;
   if(%anchor == "top-right" || %anchor == "center-right" || %anchor == "bottom-right") %dx = %extra;
   else if(%anchor == "top-center" || %anchor == "center" || %anchor == "bottom-center") %dx = -%extra / 2;
   if(%anchor == "bottom-left" || %anchor == "bottom-center" || %anchor == "bottom-right") %dy = %extra;
   else if(%anchor == "center-left" || %anchor == "center" || %anchor == "center-right") %dy = -%extra / 2;
   %at = ModernHUDStock::place(crosshairHud, %name, "center", %dx, %dy, %box, %box, %screen);
   if(%source != "")
   {
      %placed = %at;
      %at = %realAt;
      if($ModernHUD::Preview)
      {
         %px = ModernHUD::previewAxis(getWord(%at, 0), %box * %scale, getWord(%realScreen, 0), getWord(%screen, 0));
         %py = ModernHUD::previewAxis(getWord(%at, 1), %box * %scale, getWord(%realScreen, 1), getWord(%screen, 1));
         %at = ModernHUD::clampPartPos(%px @ " " @ %py, %box * %scale, %box * %scale, getWord(%screen, 0), getWord(%screen, 1));
      }
      else if(%at != %placed)
      {
         %handle = $ModernHUD::Handle[%name];
         Hud::setSessionPos(%handle, getWord(%at, 0), getWord(%at, 1));
         // Same "" answer during play: keep the position just set when the control is silent.
         %pos = Control::getPosition(%handle);
         if(%pos != "")
            %at = %pos;
         $ModernHUD::HandlePos[%name] = %at;
         if(%oldScale != %scale)
            $pref::hudPositions[%q] = %handle.position @ "||" @ %handle.fracPos;
      }
      // Replace this part's transform and rectangle in the same frame. Calling
      // part() again would register a duplicate selectable preview part.
      glPartScale(getWord(%at, 0), getWord(%at, 1), %scale);
      ModernHUD::partAppearance(%name);
      if($ModernHUD::Preview)
         $ModernHUD::PvRect[$ModernHUD::PvCount - 1] = %at @ " " @ (%box * %scale) @ " " @ (%box * %scale);
      else
         ModernHUD::recordRect(%name, %at, %box, %box);
      $ModernHUD::ScriptStockRect[crosshairHud] = %at @ " " @ %box @ " " @ %box @ " " @ %scale;
   }
   if(!$ModernHUD::Preview)
   {
      $ModernHUDStock::ReticleSource[%q] = (getWord(%at, 0) + 0) @ " " @ (getWord(%at, 1) + 0);
      $ModernHUDStock::ReticleSourceScale[%q] = %scale;
   }
   return %at;
}

function ModernHUDStock::drawReticle(%screen)
{
   ModernHUDStock::reticleSettings();
   Reticle::snapshot();
   %demo = $ModernHUD::Preview && $ModernHUD::PvDemo;
   if(!$MHReticle::Visible && !%demo)
   {
      ModernHUD::hide("ModernHUD::StockReticle");
      return;
   }
   %visible = $MHReticle::ArtVisible;
   %zoomed = $MHReticle::Zoomed;
   %texture = $MHReticle::Texture;
   %w = $MHReticle::Width; %h = $MHReticle::Height;
   %scope = $MHReticle::SniperLines;
   %guide = $MHReticle::ZoomBox;
   if(%demo)
   {
      // The designer's fixture has no live camera transition. Its normal art
      // follows the same user visibility preference as play, and remains an
      // editable part even while that preference has hidden the artwork.
      %visible = !$pref::hideCrosshairArt;
      %zoomed = false; %scope = false; %guide = false;
      %texture = "Interface\\H_Reticle.bmp";
      %w = 9; %h = 9;
   }

   // Keep a compact, stable editor target even when the scope spans the screen.
   // Framework scales about the top-left: compensate the default anchor so a
   // changed HUD size still aims at the exact center until the player drags it.
   %box = 64;
   %scale = ModernHUD::scaleOfPart("ModernHUD::StockReticle");
   if(%scale == "") %scale = 1;
   %scale = ModernHUD::scaleOf(%scale);
   %at = ModernHUDStock::reticlePlace(%screen, %box, %scale);
   %cx = getWord(%at, 0) + %box / 2;
   %cy = getWord(%at, 1) + %box / 2;
   %shape = ModernHUDStock::value("Reticle", "Shape");
   %native = %shape == "texture" || (%zoomed && ModernHUDStock::on("Reticle", "NativeZoom"));
   // An authored shape does not depend on the original bitmap being installed.
   // Preserve the same regular-art preference and effective sniper-mode gate.
   if(!%native) %visible = %zoomed || !$pref::hideCrosshairArt;
   if(%visible)
   {
      if(!%native)
         ModernHUDStock::reticleShape(%shape, %cx, %cy);
      else if(%texture != "" && %w > 0 && %h > 0)
         // Use native REFERENCE dimensions, never the decoded HD texture size.
         glDrawImage(%cx - floor(%w / 2), %cy - floor(%h / 2), %w, %h,
                      %texture, ModernHUD::alphaOf(255));
      else if(%scope)
         ModernHUDStock::reticleScope(%cx, %cy, $MHReticle::HalfWidth, $MHReticle::HalfHeight);
   }
   if(%guide && ModernHUDStock::on("Reticle", "ZoomGuide"))
      ModernHUDStock::reticleGuide(%cx, %cy,
         %cx + $MHReticle::BoxLeft - $MHReticle::CenterX,
         %cy + $MHReticle::BoxTop - $MHReticle::CenterY,
         %cx + $MHReticle::BoxRight - $MHReticle::CenterX,
         %cy + $MHReticle::BoxBottom - $MHReticle::CenterY,
         $MHReticle::ZoomFactor, $MHReticle::LowRes);
}
