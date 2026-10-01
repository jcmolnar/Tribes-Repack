// Shared stock panels use the same immediate-mode parts as authored packs.
// Retained chat/map objects remain data/input owners; their native pixels are suppressed.
function ModernHUDStock::partName(%ctrl)
{
   if(%ctrl == "healthHud") return "ModernHUD::StockHealth";
   if(%ctrl == "jetPackHud") return "ModernHUD::StockEnergy";
   if(%ctrl == "weaponHud") return "ModernHUD::StockWeapon";
   if(%ctrl == "clockHud") return "ModernHUD::StockClock";
   if(%ctrl == "compassHud") return "ModernHUD::StockCompass";
   if(%ctrl == "sensorHUD") return "ModernHUD::StockSensor";
   if(%ctrl == "chatDisplayHud") return "ModernHUD::StockChat";
   if(%ctrl == "Minimap") return "ModernHUD::StockMinimap";
   if(%ctrl == "crosshairHud") return "ModernHUD::StockReticle";
   return "";
}

function ModernHUDStock::slot(%ctrl)
{
   if(%ctrl == "healthHud" || %ctrl == "jetPackHud") return "healthenergy";
   if(%ctrl == "weaponHud") return "weapon";
   if(%ctrl == "clockHud") return "clock";
   if(%ctrl == "chatDisplayHud") return "chat";
   if(%ctrl == "Minimap") return "minimap";
   return "";
}

// Dynamic pack switches (Vector's radar, Overstep's chat) use this without
// creating a second preference that competes with the pack's own setting.
function ModernHUD::stockVisible(%ctrl, %visible)
{
   %part = ModernHUDStock::partName(%ctrl);
   if(%part == "")
   {
      Control::SetVisible(%ctrl, %visible);
      return;
   }
   // Preserve the native state before routing changes it, once per load. A
   // lobby-only load can precede these controls, so an absent object must not
   // become a saved false state. Control lookups work where isObject does not.
   if($ModernHUD::StockNativeVisible[%ctrl] == "")
   {
      %extent = Control::getExtent(%ctrl);
      if(%extent != "" && getWord(%extent, 0) > 0 && getWord(%extent, 1) > 0)
         $ModernHUD::StockNativeVisible[%ctrl] = Control::getVisible(%ctrl);
   }
   $ModernHUD::ScriptStock[%ctrl] = true;
   $ModernHUD::StockEnabled[%ctrl] = %visible;
   $ModernHUD::PartSlots[%part] = ModernHUDStock::slot(%ctrl);
   HudEditor::removeTarget(%ctrl);
   if(%ctrl == "chatDisplayHud" || %ctrl == "Minimap" || %ctrl == "crosshairHud")
      Control::SetVisible(%ctrl, true);
   else
      Control::SetVisible(%ctrl, false);
}

function ModernHUDStock::clear()
{
   %names = "healthHud jetPackHud weaponHud clockHud compassHud sensorHUD chatDisplayHud Minimap crosshairHud";
   for(%i = 0; %i < 9; %i++)
   {
      %ctrl = getWord(%names, %i);
      %visible = $ModernHUD::StockNativeVisible[%ctrl];
      if(%visible != "") Control::SetVisible(%ctrl, %visible);
   }
   DeleteVariables("ModernHUD::StockNativeVisible*");
   DeleteVariables("ModernHUD::ScriptStock*");
   DeleteVariables("ModernHUD::StockEnabled*");
}

function ModernHUDStock::registerAll()
{
   %names = "healthHud jetPackHud weaponHud clockHud compassHud sensorHUD chatDisplayHud Minimap crosshairHud";
   for(%i = 0; %i < 9; %i++)
   {
      %ctrl = getWord(%names, %i);
      // At the main menu the full-screen targeting control may not exist yet.
      // Register art independently, without overriding an observer pack's
      // deliberate control visibility. The snapshot honors that live state.
      if(%ctrl == "crosshairHud")
      {
         $ModernHUD::ScriptStock[%ctrl] = true;
         $ModernHUD::StockEnabled[%ctrl] = true;
         HudEditor::removeTarget(%ctrl);
      }
      else if($ModernHUD::ScriptStock[%ctrl] == "")
         ModernHUD::stockVisible(%ctrl, Control::getVisible(%ctrl));
      ModernHUD::partTint(ModernHUDStock::partName(%ctrl));
   }
}

function ModernHUDStock::place(%ctrl, %part, %anchor, %dx, %dy, %w, %h, %screen)
{
   // Migrate an existing saved layout once, preserving it for rollback. Preview
   // never writes layout: a different preview resolution must not alter play.
   %q = ModernHUD::qualify(%part);
   %original = $ModernHUD::PackId == "stock1998" &&
      $pref::ModernHUD::stock1998::Layout != "0" && $pref::ModernHUD::stock1998::Layout != "false" &&
      %ctrl != "chatDisplayHud" && %ctrl != "Minimap";
   %old = "";
   if($pref::ModernHUD::StockMigrated[%q] == "" && !%original && %ctrl != "crosshairHud")
   {
      if(%ctrl == "chatDisplayHud" || %ctrl == "Minimap")
         %old = ModernHUD::legacyStockLayout(%ctrl, %w, %h);
      else
         %old = $pref::hudPositions[%ctrl];
   }
   // Read legacy coordinates in the preview without writing migration state.
   // The first real play frame imports exactly the same authoritative source.
   if($ModernHUD::Preview && $pref::hudPositions[%q] == "" && !ModernHUD::isEmptyLayout(%old))
   {
      String::Explode(%old, "||", "stockLegacy");
      %anchor = "top-left";
      %dx = getWord($stockLegacy[0], 0); %dy = getWord($stockLegacy[0], 1);
   }
   %sourceReady = (%ctrl != "chatDisplayHud" && %ctrl != "Minimap") || %old != "";
   if(!$ModernHUD::Preview && $pref::ModernHUD::StockMigrated[%q] == "" && %sourceReady)
   {
      if($pref::hudPositions[%q] == "" && !ModernHUD::isEmptyLayout(%old))
      {
         $pref::hudPositions[%q] = %old;
         $pref::ModernHUD::StockMigrationPos[%q] = %old;
      }
      // Native chat and minimap resize through width/line settings, not scale.
      // Their historical generic hudScale keys must never stretch text twice.
      if(%ctrl != "chatDisplayHud" && %ctrl != "Minimap" && %ctrl != "crosshairHud" &&
         $pref::hudScale[%q] == "" && $pref::hudScale[%ctrl] != "")
      {
         $pref::hudScale[%q] = $pref::hudScale[%ctrl];
         $pref::ModernHUD::StockMigrationScale[%q] = $pref::hudScale[%ctrl];
      }
      $pref::ModernHUD::StockMigrated[%q] = 1;
   }
   // Preview the scale that the first play frame will import, without writing
   // preferences. The part/preview helpers share this temporary read override.
   $ModernHUD::StockPreviewScale[%q] = "";
   if($ModernHUD::Preview && $pref::ModernHUD::StockMigrated[%q] == "" &&
      %ctrl != "chatDisplayHud" && %ctrl != "Minimap" && %ctrl != "crosshairHud" &&
      $pref::hudScale[%q] == "" && $pref::hudScale[%ctrl] != "")
      $ModernHUD::StockPreviewScale[%q] = $pref::hudScale[%ctrl];
   %scale = ModernHUD::scaleOfPart(%part);
   if(%scale == "") %scale = 1;
   %scale = ModernHUD::scaleOf(%scale);
   // Anchor the displayed box, not its unscaled extent. In particular a saved
   // large clock must grow up from the bottom edge, not below the canvas. The
   // reticle already compensates its central aim point in its own renderer.
   %effectiveAnchor = $pref::ModernHUD::PartAnchor[%q];
   if(%effectiveAnchor == "") %effectiveAnchor = %anchor;
   if(%ctrl != "crosshairHud")
   {
      %extraW = %w * (%scale - 1); %extraH = %h * (%scale - 1);
      if(%effectiveAnchor == "top-right" || %effectiveAnchor == "center-right" || %effectiveAnchor == "bottom-right")
         %dx += %extraW;
      else if(%effectiveAnchor == "top-center" || %effectiveAnchor == "center" || %effectiveAnchor == "bottom-center")
         %dx -= %extraW / 2;
      if(%effectiveAnchor == "bottom-left" || %effectiveAnchor == "bottom-center" || %effectiveAnchor == "bottom-right")
         %dy += %extraH;
      else if(%effectiveAnchor == "center-left" || %effectiveAnchor == "center" || %effectiveAnchor == "center-right")
         %dy -= %extraH / 2;
   }
   $ModernHUD::PartSlots[%part] = ModernHUDStock::slot(%ctrl);
   %at = ModernHUD::part(%part, %anchor, %dx, %dy, %w, %h, %screen);
   $ModernHUD::StockPreviewScale[%q] = "";
   $ModernHUD::ScriptStockRect[%ctrl] = %at @ " " @ %w @ " " @ %h @ " " @ %scale;
   return %at;
}

function ModernHUDStock::wanted(%ctrl)
{
   if(!$ModernHUD::ScriptStock[%ctrl])
      return false;
   %slot = ModernHUDStock::slot(%ctrl);
   if(%slot == "") return $ModernHUD::StockEnabled[%ctrl];
   %sel = getVariable("pref::HudSlot::" @ %slot);
   if(%sel == "off") return false;
   if(%sel == "") return $ModernHUD::StockEnabled[%ctrl];
   // A borrowed component owns its whole slot; don't leave stock bars or chat
   // underneath it. An unmapped legacy choice remains the native loader's job.
   %selected = ModernHUD::slotSelection(%slot);
   // These shipped components supply the frame only. Their selected slot still
   // needs its real chat/map core, even if the base pack defaults that core off.
   if(%selected == "vodka/chat" || %selected == "vodka/minimap" ||
      %selected == "overstep/minimap") return true;
   if(%selected == "basic/healthenergy" &&
      (%ctrl == "healthHud" || %ctrl == "jetPackHud")) return true;
   if(!$ModernHUD::StockEnabled[%ctrl]) return false;
   if($ModernHUD::Comp[%selected, fn] != "") return false;
   if(String::findSubStr(%selected, "/") != -1) return false;
   return ModernHUDPack::ownsSlot(%sel);
}

function ModernHUDStock::drawOne(%ctrl, %fn, %screen)
{
   if(ModernHUDStock::wanted(%ctrl))
      *%fn(%screen);
   else
      ModernHUD::hide(ModernHUDStock::partName(%ctrl));
}

function ModernHUDStock::draw(%screen)
{
   DeleteVariables("ModernHUD::ScriptStockRect*");
   ModernHUDStock::drawOne("healthHud", "ModernHUDStock::drawHealth", %screen);
   ModernHUDStock::drawOne("jetPackHud", "ModernHUDStock::drawEnergy", %screen);
   ModernHUDStock::drawOne("weaponHud", "ModernHUDStock::drawWeapon", %screen);
   ModernHUDStock::drawOne("clockHud", "ModernHUDStock::drawClock", %screen);
   ModernHUDStock::drawOne("compassHud", "ModernHUDStock::drawCompass", %screen);
   ModernHUDStock::drawOne("sensorHUD", "ModernHUDStock::drawSensor", %screen);
   ModernHUDStock::drawOne("chatDisplayHud", "ModernHUDStock::drawChat", %screen);
   ModernHUDStock::drawOne("Minimap", "ModernHUDStock::drawMinimap", %screen);
   ModernHUDStock::drawOne("crosshairHud", "ModernHUDStock::drawReticle", %screen);
   glPartScale(0, 0, 1);
   glPartStyle(0, 1);
}
