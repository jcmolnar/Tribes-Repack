// ModernHUD Phase A framework
//
// Immediate-mode by design: no SimGui controls, authored extents, fracPos or
// persistent clip boxes. A pack implements ModernHUDPack::draw(%screenSize).
// The native ScriptGL hook calls ModernHUD::onDraw once per playGui frame.

// 2 = the generated-pack runtime (ModernHUD::part/handle/markup/digitsBox/
// imageRect/detachContainer) required by pack format v1's generated hud.cs.
$ModernHUD::FrameworkVersion = 2;
$ModernHUD::ResetGeneration = 0;

//----------------------------------------------------------------------------
// PHASE 1 -- ONE PERSISTENCE IDENTITY PER PACK
//
// ★The defect.★ Persistence was keyed on the handle name alone, so
// $pref::hudPositionsModernHUD::GEnergy_Container is ONE variable shared by
// basic, proconfig and vodka -- three packs that all generate a container of
// that name. Drag Basic's energy bar, switch to ProConfig, and ProConfig's bar
// is where you left Basic's, at Basic's scale, against different authored
// dimensions. Eight container names collide across the shipped packs.
//
// ★Why qualify the KEY and not the object name.★ The alternative was renaming
// the retained SimObject per pack, which would have made every native writer
// correct for free (they all key off getName()). It also orphans every
// ModernHUD entry in every saved preset -- pos/posName records embed the
// current object names -- and rewrites $Hud::Huds, SimGui::findControl lookups
// and the export path at the same time. Qualifying the key touches the reads
// here plus ONE native writer (HudCtrl scale, which qualifies the same way),
// and leaves preset records, control lookups and legacy huds untouched.
//
// Key form: "<packId>::<handleName>", so
//   $pref::hudPositionsbasic::ModernHUD::GEnergy_Container
// With no pack active the name is returned unchanged, so nothing that is not a
// ModernHUD handle is affected by any of this.
//----------------------------------------------------------------------------
function ModernHUD::qualify(%name)
{
   %pack = $ModernHUD::PackId;
   if(%pack == "" || %name == "")
      return %name;
   return %pack @ "::" @ %name;
}

// ★One-time migration of the ambiguous pre-Phase-1 value, claimable ONCE.★
//
// The old unqualified key does not record which pack authored it, so it is
// meaningful for at most one pack. The naive rule ("if the qualified key is
// unset and the old one is set, copy it") fans that single layout into every
// pack the player visits -- basic claims it, then proconfig claims the same
// value, then vodka. The claim marker is what makes it once.
//
// ★The marker must SURVIVE reset.★ It records that an ambiguous historical
// value was consumed; it is not a statement about the current layout. Clearing
// it on reset makes the old value eligible again on the next load, so a reset
// part comes back at its pre-reset position after a restart -- the exact
// resurrection the acceptance test exists to catch. Reset clears the qualified
// keys and leaves both the legacy value (for rollback) and this marker alone.
// C5: a pack-scoped setting key. Falls back to the historical global name when
// no pack is active so a pref written before this still resolves.
function ModernHUD::packSettingKey(%leaf)
{
   %pack = $ModernHUD::PackId;
   if(%pack == "")
      return "pref::ModernHUD::" @ %leaf;
   return "pref::ModernHUD::" @ %pack @ "::" @ %leaf;
}

function ModernHUD::claimLegacyLayout(%name)
{
   %pack = $ModernHUD::PackId;
   if(%pack == "" || %name == "")
      return;

   // Already consumed by somebody -- including by this pack on an earlier boot.
   if($pref::ModernHUD::LegacyClaimed[%name] != "")
      return;

   %q = ModernHUD::qualify(%name);

   // Only claim into a pack that has nothing of its own yet.
   if($pref::hudPositions[%q] != "" || $pref::hudScale[%q] != "")
      return;

   %oldPos   = $pref::hudPositions[%name];
   %oldScale = $pref::hudScale[%name];
   if(%oldPos == "" && %oldScale == "")
      return;

   if(%oldPos != "")   { $pref::hudPositions[%q] = %oldPos; }
   if(%oldScale != "") { $pref::hudScale[%q]     = %oldScale; }

   // Records WHO consumed it, so the log can explain a surprising layout later.
   // The legacy keys are deliberately left in place: a player who rolls back to
   // a pre-Phase-1 build keeps their layout.
   $pref::ModernHUD::LegacyClaimed[%name] = %pack;
}

// Read helpers. Every reader goes through these so there is one definition of
// "where is this part's layout kept", and the migration happens on first touch
// rather than needing a separate pass over a registry that may not be built yet.
function ModernHUD::posOf(%name)
{
   ModernHUD::claimLegacyLayout(%name);
   return $pref::hudPositions[ModernHUD::qualify(%name)];
}

function ModernHUD::scaleOfPart(%name)
{
   ModernHUD::claimLegacyLayout(%name);
   return $pref::hudScale[ModernHUD::qualify(%name)];
}

// ★Framework-owned store, replacing the dependency on legacy Hud::Store.★
// main.cpp runs `Hud::Store(%i)` over every hud on a config swap, and that
// function is defined ONLY in the five CustomConfigs/*/config/Core/Hud.cs
// files. Boot straight into a master pack with no legacy config ever executed
// and the swap-time save calls a function that does not exist -- a live latent
// defect today, and unconditional once CustomConfigs is retired.
// "x y||fx fy fz" that says nothing: every field zero (or absent). A part that has
// never been moved reports this, and it must never be mistaken for a real layout.
// Used at BOTH ends -- storePos refuses to write one, restorePos refuses to apply
// one that an older build already wrote to disk.
function ModernHUD::isEmptyLayout(%val)
{
   if(%val == "")
      return true;
   if(String::Explode(%val, "||", "elFields") != 2)
      return false;
   %pos  = $elFields[0];
   %frac = $elFields[1];
   if(getWord(%pos, 0) != 0 || getWord(%pos, 1) != 0)
      return false;
   // An absent fracPos reads as "" here, which is not != 0, so it counts as empty.
   if(getWord(%frac, 0) != 0 || getWord(%frac, 1) != 0)
      return false;
   return true;
}

function ModernHUD::storePos(%i)
{
   %handle = $Hud::Huds[%i];
   %name   = $Hud::Huds[%i, name];
   if(%name == "" || !isObject(%handle))
      return;
   // Only ModernHUD handles: a legacy hud keeps using the legacy path, which is
   // still the correct owner of its own key while CustomConfigs exists.
   if($ModernHUD::HandleRegistered[%name] == "")
      return;
   %val = %handle.position @ "||" @ %handle.fracPos;

   // ★Never persist an all-zero entry.★ "0 0||0 0 0" carries no layout
   // information -- it is what a part that has never been dragged reports -- but
   // writing it turns "use the pack's authored default" into "pin to the top-left
   // corner" on every future load, because restorePos cannot tell the two apart.
   //
   // This is the bug described in the comment below, finally fixed: Basic shipped
   // a config/hudLayout_ModernHUD_basic.cs consisting ENTIRELY of "0 0||0 0 0",
   // and its eight parts drew stacked on top of each other in the corner while the
   // legacy runtime laid them out in a neat column. Screenshot-confirmed.
   //
   // fearGuiHudCtrl.cpp:174 already applies exactly this rule on the C++ side
   // ("frac 0,0 = no layout" -> skip); the script side simply never matched it.
   if(ModernHUD::isEmptyLayout(%val))
   {
      if($pref::hudSlotDiag)
         echo("[MHPOS] store " @ ModernHUD::qualify(%name) @ " SKIPPED (no layout)");
      return;
   }

   $pref::hudPositions[ModernHUD::qualify(%name)] = %val;

   // $pref::hudSlotDiag: the store/restore pair is the only thing that carries a
   // ModernHUD layout across a swap, and a bad round-trip is SILENT -- parts just
   // appear at the wrong place with nothing in the log. Reported live: Basic's
   // parts collapsed to the top-left after basic -> proconfig -> basic, with no
   // error of any kind in that session. Name the value at both ends so the next
   // occurrence says which end wrote the wrong thing.
   if($pref::hudSlotDiag)
      echo("[MHPOS] store " @ ModernHUD::qualify(%name) @ " = [" @ %val @ "]");
}

// Every registered ModernHUD handle, stored. Called from the native swap path.
function ModernHUD::storeAll()
{
   %n = $Hud::Count;
   if(%n == "") %n = 0;
   for(%i = 0; %i < %n; %i++)
      ModernHUD::storePos(%i);
}

// Request every immediate-mode handle to return to the authored position
// supplied by its pack on the next draw. The generation makes reset safe for
// dynamic HUD parts: each handle consumes it exactly once when it is present.
// Handle names are registered by the pack's handle() helper so the normal
// $pref::hudPositions namespace is cleared as well and the reset survives a
// restart instead of being overwritten by an old saved position.
function ModernHUD::resetPositions()
{
   $ModernHUD::ResetGeneration++;
   for(%i = 0; %i < $ModernHUD::HandleCount; %i++)
   {
      %name = $ModernHUD::HandleName[%i];
      if(%name != "")
      {
         // Phase 1: clear the QUALIFIED keys. The unqualified legacy value and
         // the claim marker are deliberately left alone -- see
         // ModernHUD::claimLegacyLayout. Clearing the marker here would let the
         // old value be re-imported on the next load and a reset part would come
         // back at its pre-reset position after a restart.
         %q = ModernHUD::qualify(%name);
         $pref::hudPositions[%q] = "";
         $pref::hudScale[%q] = "";

         // The pre-Phase-1 keys are also cleared for the ACTIVE pack only when it
         // is the pack that claimed them, so "reset" visibly resets for the one
         // pack whose layout that value actually became.
         if($pref::ModernHUD::LegacyClaimed[%name] == $ModernHUD::PackId)
            $pref::hudPositions[%name] = "";

         // ★A reset must undo the RESIZE too.★ Dragging a hud's corner writes
         // $pref::hudScale<name> (HudCtrl::cfgSaveUserScale) and nothing ever
         // cleared it -- not this function, not HudSlot::reset(). So a part that
         // had been resized stayed resized through every reset, which reads as
         // "reset does nothing": the part is the size you are complaining about,
         // and moving it back to its authored spot does not change that. Measured
         // on a live tree: $pref::hudScaleModernHUD::VectorReticle = "2.2679"
         // survived both reset buttons.
         //
         // Clearing the pref is sufficient for immediate-mode parts: ModernHUD::part
         // re-reads it every frame and treats "" as 1.
         //
         // Phase 1: the qualified scale is cleared above. This clears the
         // unqualified one too, but ONLY for the pack that claimed it -- the
         // native resize writer also qualifies now, so an unqualified scale can
         // only be pre-Phase-1 data.
         if($pref::ModernHUD::LegacyClaimed[%name] == $ModernHUD::PackId)
            $pref::hudScale[%name] = "";

         // Reset live, not merely on the next render. Options temporarily
         // removes playGui from the canvas, but its retained HUD handles still
         // exist and can be placed safely. This also makes the command's effect
         // independent of whether a preset was just saved/loaded.
         %handle = $ModernHUD::Handle[%name];
         %defaultPos = $ModernHUD::DefaultPos[%name];
         if(isObject(%handle) && %defaultPos != "")
         {
            Hud::setSessionPos(%handle,
               getWord(%defaultPos, 0), getWord(%defaultPos, 1));
            $ModernHUD::HandlePos[%name] = %defaultPos;
            $ModernHUD::AppliedReset[%name] = $ModernHUD::ResetGeneration;
         }
      }
   }
}

// Remove the retained edit handles when their pack is unloaded. The immediate
// art stops when Enabled is false, but leaving its handles in playGui would
// leave invisible K-editor targets and stale $Hud entries for the next pack.
// Walk backwards because Module::hudRemove compacts the indexed Hud inventory.
function ModernHUD::unload()
{
   for(%i = $Hud::Count - 1; %i >= 0; %i--)
   {
      %name = $Hud::Huds[%i, name];
      if($ModernHUD::HandleRegistered[%name])
         Module::hudRemove(%i);
   }

   for(%i = 0; %i < $ModernHUD::HandleCount; %i++)
   {
      %name = $ModernHUD::HandleName[%i];
      $ModernHUD::Handle[%name] = "";
      $ModernHUD::HandlePos[%name] = "";
      $ModernHUD::HandleRegistered[%name] = "";
      $ModernHUD::AppliedReset[%name] = "";
      $ModernHUD::DefaultPos[%name] = "";
      // Armed-but-unfired fit check: a pack torn down before its first drawn frame
      // would otherwise leave this set, and the NEXT pack's part of the same name
      // would inherit an arm it never asked for.
      $ModernHUD::FitPending[%name] = "";
      $ModernHUD::HandleName[%i] = "";
   }
   $ModernHUD::HandleCount = 0;
   $ModernHUD::Enabled = false;

   // Same reason as the handle table above: a swapped-away pack's part boxes must
   // not survive into the next pack, or the parity harness measures the new pack
   // against the old one's rectangles.
   ModernHUD::clearRects();

   // Revoke everything the pack's data layer registered. Without this a swapped
   // -away pack's handlers keep firing under the next one -- the exact legacy
   // defect this framework exists to remove.
   ModernHUD::detachAll();

   // ...and forget which data modules were exec'd, so the next pack's require()
   // actually runs its own (same names, different pack).
   DeleteVariables("ModernHUD::Required*");

   // ...and the pack's own settings rows, so the next pack does not inherit
   // this one's options on the Configs tab.
   ModernHUD::clearSettings();

   // ★Then delete any edit handle still parented into playGui, by TYPE.★ The loop
   // above can only remove handles this framework can still SEE -- it walks its own
   // registry -- so anything that fell out of the registry (a blank index, a pack
   // that changed which parts it creates) survived every unload and accumulated one
   // orphan per reload, each an invisible click-eating box. Measured on a live tree:
   // two 'ModernHUD::VectorReticle' controls, one of them 2161x771, larger than the
   // screen. A type sweep needs no registry to be correct.
   if(isFunction("ModernHUD::purgeHandles"))
      ModernHUD::purgeHandles();

   // E2 addendum: drop every editor-target id this pack registered. The native
   // load path clears too; this belt covers a script-driven unload so a stale
   // id can never carry into the next pack's session.
   HudEditor::clearTargets();

   ModernHUD::sweepDeadHuds();
}

//------------------------------------------------------------------------------
// PACK SETTINGS -- a pack's own options, rendered natively on the Configs tab.
//
// A pack declares them at load:
//
//   ModernHUD::setting(<type>, <prefKey>, <label>, <default>, <spec>, <apply>)
//
//     type    "enum" | "bool" | "int"
//     prefKey a $pref:: variable WITHOUT the '$' ("pref::Vector::Theme")
//     spec    enum: "Label|value;Label|value;..."   int: "min|max|step"
//             bool: unused
//     apply   console command run whenever the value CHANGES (may be "")
//
// ★Declared from script, not from pack.json.★ The client's manifest reader
// (modernHudPacks.cpp:69) is deliberately a strstr scan for four scalars, and
// says why: a hand-rolled JSON parser in the client would be a second, weaker
// definition of the format. Settings would have forced one. hud.cs already runs
// at load, already owns the pack's behaviour, and already unloads cleanly -- so
// the registry lives here and the native side only reads console globals.
//
// ★The default is SEEDED when the pref is unset.★ That is not a convenience: it
// is the fix for a whole bug class. compare() (eval.cpp:377) promotes a
// comparison to float as soon as either side is a numeric literal, so a pack
// testing `if($pref::Foo == 0)` matches an UNSET pref and ships its feature in
// the off state. Seeding means no pack has to get that right.
//
// One namespace, so a swapped-in pack cannot inherit the last pack's rows:
// ModernHUD::unload() clears the whole registry.
//------------------------------------------------------------------------------
function ModernHUD::setting(%type, %key, %label, %default, %spec, %apply)
{
   %i = $ModernHUD::SettingCount;
   if(%i == "") %i = 0;

   $ModernHUD::Setting[%i, type]    = %type;
   $ModernHUD::Setting[%i, key]     = %key;
   $ModernHUD::Setting[%i, label]   = %label;
   $ModernHUD::Setting[%i, dflt]    = %default;
   $ModernHUD::Setting[%i, spec]    = %spec;
   $ModernHUD::Setting[%i, apply]   = %apply;

   $ModernHUD::SettingCount = %i + 1;

   // ★A pack with settings OWNS the K key.★ dlgPlay.cpp keeps the stock checkbox
   // list hidden when this is set and hands the pointer over instead, so the
   // framework panel draws in its place. Declared here rather than by each pack:
   // "has settings" is exactly the condition under which there is something to
   // show, and a pack that registers none changes nothing.
   $Config::HudListOwned = 1;

   // Seed the default only when the pref has never been set. A string test, so
   // it cannot be fooled by the numeric promotion described above.
   if(ModernHUD::settingGet(%key) == "")
      ModernHUD::settingSet(%key, %default);
}

// Is a pref key already registered? Used by commonSettings so a pack that owns a
// concept keeps it -- two opacity rows that both multiply the same pixels is worse
// than none.
function ModernHUD::hasSetting(%key)
{
   %n = $ModernHUD::SettingCount;
   if(%n == "") { %n = 0; }
   for(%i = 0; %i < %n; %i++)
   {
      if($ModernHUD::Setting[%i, key] == %key) { return true; }
   }
   return false;
}

//------------------------------------------------------------------------------
// UNIVERSAL ROWS -- what every pack gets for free.
//
// Called by the native loader once the pack's own hud.cs has run
// (modernHudPacks.cpp MHPacks_load), so these land at the END of the registry and
// can see what the pack already declared.
//
// ★Why these two and no others.★ They are the only settings the framework can
// honour without knowing anything about a pack's layout: every generated pack
// draws through ModernHUD::imageRect/bar/markup/digitsBox (which take an alpha
// this multiplies) and positions through ModernHUD::part (which sets the part
// scale this multiplies). Anything else -- what a part shows, where it anchors --
// is the pack's business and must be the pack's row.
//
// A pack that already owns one of these declares it and the generic row is
// suppressed rather than stacked on top -- two controls multiplying the same
// pixels is worse than none:
//
//   $ModernHUD::OwnOpacity = 1;   // I have my own opacity control
//   $ModernHUD::OwnScale   = 1;   // I have my own size control
//
// Both are cleared on unload with the rest of the registry.
//------------------------------------------------------------------------------
function ModernHUD::commonSettings()
{
   // ★Opt-out is DECLARED, not inferred.★ The first cut sniffed the pack's row
   // labels for the word "opacity". That is guessing twice over: it depends on a
   // label a pack is free to word differently, and on String::findSubStr being
   // registered -- which it is not in every boot, and an unknown command returns
   // "" that compares numerically equal to 0, so the sniff silently decided the
   // opposite of what it meant and Vector got a second opacity row. A pack that
   // owns a concept says so.
   // ★C5: PACK-SCOPED, not one global.★ These were flat globals, so every pack
   // shared one opacity and one size -- set Basic to 70% and Vector inherited it
   // against completely different authored art. That is the same collision class
   // as the layout keys, and it is the example the audit named first.
   %opacityKey = ModernHUD::packSettingKey("Opacity");
   %scaleKey   = ModernHUD::packSettingKey("Scale");

   if(!$ModernHUD::OwnOpacity && !ModernHUD::hasSetting(%opacityKey))
   {
      ModernHUD::setting("int", %opacityKey, "HUD opacity (%)",
                         "100", "20|100|5", "");
   }

   if(!$ModernHUD::OwnScale && !ModernHUD::hasSetting(%scaleKey))
   {
      ModernHUD::setting("int", %scaleKey, "HUD size (%)",
                         "100", "50|200|5", "");
   }

   // The two stock-HUD switches that are prefs rather than controls, so they have
   // no ModernHUD::stock row. See the block below for why they are the ONLY safe
   // way to offer the old K list's "Crosshair" and "Sniper crosshair" rows.
   //
   // ★Both are global engine prefs, deliberately NOT pack-scoped.★ Every other
   // stock row is per-pack because a pack authored a reason to hide it; these two
   // describe the player's aim, which does not change meaning when the HUD skin
   // does. A pack that drives either from its own row opts out, as with the
   // opacity/size pair above.
   if(!$ModernHUD::OwnCrosshairArt && !ModernHUD::hasSetting("pref::hideCrosshairArt"))
   {
      ModernHUD::setting("enum", "pref::hideCrosshairArt", "Crosshair art",
                         "0", "On|0;Off|1", "");
   }

   // TRUE/FALSE, not 1/0: FearGuiHudList.cpp:114-123 and the crosshair renderer
   // both string-compare against "FALSE", so an enum carrying the engine's own
   // spelling is the only form that round-trips.
   if(!$ModernHUD::OwnSniperCross && !ModernHUD::hasSetting("pref::SniperCrosshair"))
   {
      ModernHUD::setting("enum", "pref::SniperCrosshair", "Sniper crosshair",
                         "TRUE", "Off|FALSE;On|TRUE", "");
   }

   // ★Fonts, in the K menu, for THIS pack.★ Reported by Joe: the Options tabs had a
   // font row but the K menu had nowhere to change fonts at all, which is the one
   // place a player is actually looking at the HUD while they judge it.
   //
   // The option list comes from ModernHUD::fontSets() -- the native scan of
   // config\Fonts\Sets -- so the K menu, Options > Interface and Options > Configs all
   // offer THE SAME list from ONE scan. A second hand-maintained list here would be a
   // second definition of "what font sets exist", which is exactly the duplication
   // this whole store exists to remove.
   //
   // The key is PACK-SCOPED and is the same key the Configs tab writes
   // (pref::ModernHUD::FontSet::<pack>), so the two rows are two views of one setting
   // rather than two competing settings. Empty value = the pack's own fonts.
   //
   // apply calls ModernHUD::applyFonts(), which republishes the font context; nothing
   // is mounted or unmounted, so this is safe mid-frame.
   // ★Guarded by the RESULT, not by isFunction.★ isFunction only knows SCRIPT
   // functions -- it answers False for a registered native command (isFunction("exec")
   // is False too), so guarding on it skipped this row on every pack and the K menu
   // gained nothing. Calling it and testing the spec is self-guarding: an unregistered
   // command evaluates to "" and the row is simply not offered.
   // 2026-09-02 (Joe): the per-config "Font set" row is GONE from here and from
   // Options > Configs > Fonts -- it swapped .pft bitmap sets and read as doing
   // nothing. Chat text is now ScriptGL in every config (FearGuiChatDisplay.cpp),
   // and these two GLOBAL rows are its controls: face + size, separate from any
   // pack's own "HUD font". The engine rewraps the live log the frame a value
   // changes, so no apply command is needed.
   %cfSpec = ModernHUD::ttfSpec();
   if(%cfSpec != "" && !ModernHUD::hasSetting("pref::ChatFont"))
   {
      ModernHUD::setting("enum", "pref::ChatFont", "Chat font", "Segoe UI",
                         %cfSpec, "");
   }
   if(!ModernHUD::hasSetting("pref::ChatFontSize"))
   {
      ModernHUD::setting("int", "pref::ChatFontSize", "Chat font size (px, 0 = auto)",
                         "0", "0|48|1", "");
   }
   if(!ModernHUD::hasSetting("pref::ChatFontBold"))
   {
      ModernHUD::setting("enum", "pref::ChatFontBold", "Chat font weight", "1",
                         "Semibold|1;Regular|0", "");
   }
}

// Installed TrueType families as an enum spec ("Name|Name;..."), scanned ONCE per
// session from the same candidate list Vector uses (Vector::fontScan), gated on
// glFontExists so only faces this machine can rasterize are offered.
function ModernHUD::ttfSpec()
{
   if($ModernHUD::TtfSpec != "")
      return $ModernHUD::TtfSpec;
   %cand = "Segoe UI;Segoe UI Semibold;Verdana;Tahoma;Trebuchet MS;Calibri;Candara;Corbel;" @
           "Bahnschrift;Bahnschrift Condensed;Franklin Gothic Medium;Century Gothic;Arial;" @
           "Arial Narrow;Consolas;Cascadia Mono;Lucida Console;Impact;Rockwell;Eurostile;Agency FB";
   %spec = "";
   %cur = "";
   %len = String::Length(%cand);
   for(%i = 0; %i <= %len; %i++)
   {
      %c = String::getSubStr(%cand, %i, 1);
      if(%c == ";" || %i == %len)
      {
         if(%cur != "" && glFontExists(%cur) == 1)
         {
            if(%spec != "") %spec = %spec @ ";";
            %spec = %spec @ %cur @ "|" @ %cur;
         }
         %cur = "";
      }
      else
         %cur = %cur @ %c;
   }
   $ModernHUD::TtfSpec = %spec;
   return %spec;
}

//------------------------------------------------------------------------------
// STOCK HUD ROWS -- the old K list, restored as registry rows.
//
// ★This exists because Phase 0 took the K key away from the stock list.★
// ModernHUD::setting sets $Config::HudListOwned, and dlgPlay.cpp:120-127 then
// force-hides IDCTG_HUD_LIST and returns early. Before the universal
// opacity/size rows existed only Vector declared any setting, so only Vector took
// the key; commonSettings() gives every pack a row, so EVERY pack now hides the
// stock list. The ten per-hud checkboxes behind it became unreachable in one
// commit, for every config. This puts them back on the same key, in the panel
// that replaced them.
//
// ★A pack DECLARES the default; the player OVERRIDES it.★ stockHuds() asserts the
// whole set on every call by design ("a pack that lists only its own leaves the
// rest wherever the previous pack put them" -- vector/hud.cs), which is exactly
// why the old checkboxes could not have been left as they were: a raw
// Control::SetVisible from a list row is overwritten on the next assertion. So
// the assertion itself reads the pref, and the row only writes it.
//
// ★Pack-scoped keys.★ A pack hides healthHud because it draws its own health;
// the next pack may not. One global "health off" would follow the player into a
// config where it means something else entirely -- the same collision class the
// consolidation plan's Phase 1 is about, so it is not reintroduced here.
//
// Two things the old list had that are NOT rows here:
//   crosshairHud -- refused below; hiding it takes the whole nameplate system
//                   down with it (fearGuiCrosshair.cpp:585). "Crosshair art"
//                   above is the safe equivalent.
//   the logo     -- IDCTG_HUD_LOGO has no script-reachable control name, so it
//                   needs a native accessor before it can be offered. Not
//                   carried; recorded here so it is not silently forgotten.
//------------------------------------------------------------------------------
function ModernHUD::stockLabel(%ctrl)
{
   if(%ctrl == "clockHud")       { return "Clock"; }
   if(%ctrl == "compassHud")     { return "Compass"; }
   if(%ctrl == "weaponHud")      { return "Weapon"; }
   if(%ctrl == "healthHud")      { return "Health"; }
   if(%ctrl == "jetPackHud")     { return "Jetpack"; }
   if(%ctrl == "chatDisplayHud") { return "Chat display"; }
   if(%ctrl == "sensorHUD")      { return "Sensor"; }
   if(%ctrl == "Minimap")        { return "Minimap"; }
   return %ctrl;
}

// Assert a stock control's visibility, with a player row in front of it.
//
// Call this INSTEAD of Control::SetVisible for anything a player should be able
// to switch. A control the pack drives from its own logic (Vector's minimap) or
// one that is plumbing rather than a HUD element (ProConfig's remoteEP pair)
// keeps its direct Control::SetVisible call and gets no row -- that is the
// opt-out, and it needs no new mechanism.
function ModernHUD::stock(%ctrl, %default)
{
   // ★Never.★ FearGui::Crosshair::onRender drives names, health and jet bars, the
   // pass helper, friend/foe skulls and target acquisition. A row that hid this
   // would read "Crosshair: OFF" and silently disable half the HUD.
   if(%ctrl == "crosshairHud")
   {
      Control::SetVisible(crosshairHud, true);
      return;
   }

   // E2 addendum: stock() is an approved editor-target chokepoint. A stock HUD
   // a pack intentionally exposes becomes movable in the owned K editor by
   // exact current id (the native command resolves the plain name).
   // Registration is independent of the player's ON/OFF choice -- visibility
   // still decides whether a registered target can actually be hit. Transient
   // message controls, crosshair plumbing, and displaced legacy containers
   // never come through here, so they are never registered. Delegates to the
   // explicit API so a pack with its own visibility logic can register the
   // control WITHOUT taking a settings row (ModernHUD::editTarget).
   ModernHUD::editTarget(%ctrl);

   // ★Compare against QUOTED strings, never against a bare 0.★ The generator emits
   // `true`/`false` here, and compare() (eval.cpp:367-380) promotes the whole
   // comparison to float the moment either side is a numeric LITERAL -- so
   // `%default == 0` evaluates atof("true"), gets 0, and reports that "true"
   // equals zero. Every default-ON row would have registered as OFF. With two
   // string operands compare() instead takes its boolchk path (:392-418), which
   // understands "true"/"false"/"1"/"0" as the same three-valued thing.
   %dflt = "1";
   if(%default == "false") { %dflt = "0"; }
   if(%default == "")      { %dflt = "0"; }

   %key = "pref::ModernHUD::" @ $ModernHUD::PackId @ "::Stock::" @ %ctrl;

   // Registered once per load. stockHuds() is called again on every gui open and
   // as this row's own apply, so without the guard the registry would grow by the
   // whole stock set on each call and MHSettings_tick would see the count change
   // and reseed its cache mid-session.
   if(!ModernHUD::hasSetting(%key))
   {
      ModernHUD::setting("bool", %key, ModernHUD::stockLabel(%ctrl), %dflt, "",
                         "ModernHUDPack::stockHuds();");
   }

   // ModernHUD::setting seeded the default when the pref was unset, so a read
   // that still comes back empty means the pref was set to empty by something
   // else -- treat that as the pack's default rather than as OFF, which is what
   // rowShown() would display it as.
   %v = getVariable(%key);
   if(%v == "") { %v = %dflt; }

   // Same rule as above: quoted, so a stored "false" and a stored "0" both read
   // as off and neither is decided by a float promotion.
   if(%v == "0" || %v == "false") { Control::SetVisible(%ctrl, false); }
   else                           { Control::SetVisible(%ctrl, true);  }
}

// The multipliers the universal rows drive. Read per draw call rather than
// cached: a pref written from the menu, the Options page or the console must take
// effect on the very next frame, and this is two string reads against a HUD that
// is already issuing GL calls.
function ModernHUD::alphaOf(%alpha)
{
   %o = getVariable(ModernHUD::packSettingKey("Opacity"));
   if(%o == "" || %o <= 0) { return %alpha; }
   if(%o >= 100) { return %alpha; }
   return %alpha * %o / 100;
}

function ModernHUD::scaleOf(%scale)
{
   %s = getVariable(ModernHUD::packSettingKey("Scale"));
   if(%s == "" || %s <= 0) { return %scale; }
   return %scale * %s / 100;
}

// Read/write a pref by NAME. The console has no dynamic variable assignment
// (`*expr(args)` is a dynamic CALL and the only indirection the grammar has), so
// these bounce through the native getVariable/setVariable bridge instead of
// trying to build an assignment out of the name.
function ModernHUD::settingGet(%key)
{
   return getVariable(%key);
}

function ModernHUD::settingSet(%key, %value)
{
   setVariable(%key, %value);
}

function ModernHUD::clearSettings()
{
   %n = $ModernHUD::SettingCount;
   if(%n == "") %n = 0;
   for(%i = 0; %i < %n; %i++)
   {
      $ModernHUD::Setting[%i, type]    = "";
      $ModernHUD::Setting[%i, key]     = "";
      $ModernHUD::Setting[%i, label]   = "";
      $ModernHUD::Setting[%i, dflt]    = "";
      $ModernHUD::Setting[%i, spec]    = "";
      $ModernHUD::Setting[%i, apply]   = "";
   }
   $ModernHUD::SettingCount = 0;

   // Hand K back to the stock hud list, and drop this pack's menu look, so the
   // next pack starts from the framework defaults rather than inheriting a
   // palette (or an owned key) it never asked for.
   $Config::HudListOwned      = "";
   $ModernHUD::OwnOpacity     = "";
   $ModernHUD::OwnScale       = "";
   $ModernHUD::OwnCrosshairArt = "";
   $ModernHUD::OwnSniperCross  = "";
   $ModernHUD::MenuPage   = 0;
   $ModernHUD::MenuDrag   = "";
   $ModernHUD::MenuDown   = "";
   ModernHUD::menuPalette();

   // Phase 4: the component registry belongs to the pack set that registered it.
   // Leaving it would let a swapped-in base pack draw the previous pack's
   // components -- the same last-definition-wins failure the registry exists to
   // remove, just one level up.
   ModernHUD::clearComponents();
}

//------------------------------------------------------------------------------
// THE IN-GAME SETTINGS MENU -- framework-owned, registry-driven.
//
// ★One menu engine for every pack.★ This was authored inside Vector's hud.cs and
// worked, but it was 250 lines of drag/hit-test/stepper code that only Vector had:
// every other pack's settings were reachable only from the native Options page,
// three navigations deep, which is a discoverability failure -- a setting a player
// cannot find is a setting that does not exist.
//
// Nothing here is pack-specific. The rows ARE the ModernHUD::setting registry the
// packs already declare (and that the native Options page already renders), so a
// pack gets this menu by declaring settings and changing nothing else.
//
// Interaction rides glMousePos ("x y lmb rmb"). Rows are hit-tested against the
// same rectangles they are drawn with, in the same surface pixels, so there is no
// coordinate mapping to get wrong.
//
// ★The pref write is the whole action.★ Applies are NOT run from here: the native
// per-frame watcher (modernHudPacks.cpp MHSettings_tick) notices any registered
// pref changing and runs that row's apply command -- the same path the Options
// page relies on. A second applier here would double-fire every one of them.
//
// A pack may override two things and nothing else:
//   ModernHUDPack::menuFrame(%x,%y,%w,%h,%head)   its own panel chrome
//   $ModernHUD::MenuPrimary/Dim/Accent/Text/Warn  the palette (set from its theme)
//   $ModernHUD::MenuTitle / MenuFont              heading text, TrueType face
//------------------------------------------------------------------------------

// Palette defaults, re-seeded on every pack unload so a swapped-in pack that sets
// no colours does not inherit the last pack's theme.
function ModernHUD::menuPalette()
{
   $ModernHUD::MenuPrimary = "90 190 255";
   $ModernHUD::MenuDim     = "16 26 38";
   $ModernHUD::MenuAccent  = "150 225 255";
   $ModernHUD::MenuText    = "235 245 255";
   $ModernHUD::MenuWarn    = "255 90 90";
   $ModernHUD::MenuTitle   = "";
   $ModernHUD::MenuFont    = "Verdana";
}

function ModernHUD::mColor(%rgb, %alpha)
{
   glColor4ub(getWord(%rgb, 0), getWord(%rgb, 1), getWord(%rgb, 2), %alpha);
}

// TrueType, not .pft: the panel is drawn at whatever size the layout asks for and
// a bitmap font goes blocky the moment anything scales it. glSetFont rasterizes a
// fresh atlas per (face, px) pair -- quantised to even sizes here so the cache
// cannot be churned by a size that varies continuously.
function ModernHUD::mText(%x, %y, %width, %rgb, %str, %alpha, %px, %just)
{
   %px = floor(%px / 2) * 2;
   if(%px < 6) { %px = 6; }

   %font = $ModernHUD::MenuFont;
   if(%font == "") { %font = "Verdana"; }
   glSetFont(%font, %px);

   %sw = getWord(glGetStringDimensions(%str), 0);
   if(%just == "c")      { %x = %x + floor((%width - %sw) / 2); }
   else if(%just == "r") { %x = %x + %width - %sw; }

   ModernHUD::mColor(%rgb, %alpha);
   glDrawString(%x, %y, %str);
}

function ModernHUD::mHit(%mx, %my, %x, %y, %w, %h)
{
   if(%mx < %x || %mx >= %x + %w) { return false; }
   if(%my < %y || %my >= %y + %h) { return false; }
   return true;
}

// Edge-triggered: a held button must not run a stepper sixty times a second.
function ModernHUD::mClicked(%mx, %my, %x, %y, %w, %h)
{
   if($ModernHUD::MenuClick != 1) { return false; }
   return ModernHUD::mHit(%mx, %my, %x, %y, %w, %h);
}

//------------------------------------------------------------------------------
// Registry readers. An enum spec is "Label|value;Label|value;..." and an int spec
// is "min|max|step" -- the same two formats the native side parses
// (modernHudPacks.cpp mhSpecEntry), so the menu and the Options page can never
// disagree about what a row offers.
//
// String::getWord's 3-argument form takes the delimiter, so no hand-rolled
// scanning is needed. Labels may contain spaces; only ';' and '|' separate.
//------------------------------------------------------------------------------
function ModernHUD::specCount(%spec)
{
   if(%spec == "") { return 0; }
   %n = 0;
   while(String::getWord(%spec, ";", %n) != "")
   {
      %n++;
      if(%n > 64) { return 64; }
   }
   return %n;
}

function ModernHUD::specLabel(%spec, %idx)
{
   return String::getWord(String::getWord(%spec, ";", %idx), "|", 0);
}

function ModernHUD::specValue(%spec, %idx)
{
   return String::getWord(String::getWord(%spec, ";", %idx), "|", 1);
}

// What this row currently reads as. An unrecognised value shows as ITSELF rather
// than as whatever option 0 happens to be -- displaying a value the pack does not
// offer as a valid choice would hide the fact that the pref holds something odd.
function ModernHUD::rowShown(%i)
{
   %type = $ModernHUD::Setting[%i, type];
   %cur  = getVariable($ModernHUD::Setting[%i, key]);

   if(%type == "bool")
   {
      if(%cur == "0" || %cur == "") { return "OFF"; }
      return "ON";
   }
   if(%type == "enum")
   {
      %spec = $ModernHUD::Setting[%i, spec];
      %n = ModernHUD::specCount(%spec);
      for(%o = 0; %o < %n; %o++)
      {
         if(ModernHUD::specValue(%spec, %o) == %cur)
            return ModernHUD::specLabel(%spec, %o);
      }
      return %cur;
   }
   return %cur;
}

// Step a row by %dir (-1 / +1). Enums and bools WRAP -- with one stepper pair and
// no keyboard, a player who steps past the end must be able to get back without
// walking the whole list. Ints CLAMP: wrapping a size from 300% to 50% on one
// extra click is a change nobody asked for.
function ModernHUD::rowStep(%i, %dir)
{
   %type = $ModernHUD::Setting[%i, type];
   %key  = $ModernHUD::Setting[%i, key];
   %cur  = getVariable(%key);

   if(%type == "bool")
   {
      if(%cur == "0" || %cur == "") { setVariable(%key, "1"); }
      else                          { setVariable(%key, "0"); }
      return;
   }

   if(%type == "enum")
   {
      %spec = $ModernHUD::Setting[%i, spec];
      %n = ModernHUD::specCount(%spec);
      if(%n <= 0) { return; }

      %at = 0;
      for(%o = 0; %o < %n; %o++)
      {
         if(ModernHUD::specValue(%spec, %o) == %cur) { %at = %o; }
      }
      %at = %at + %dir;
      if(%at < 0)   { %at = %n - 1; }
      if(%at >= %n) { %at = 0; }
      setVariable(%key, ModernHUD::specValue(%spec, %at));
      return;
   }

   // int: spec "min|max|step". A missing spec still steps by 1 rather than
   // freezing the row -- an unparseable spec is an authoring bug, not a reason
   // to make the control dead.
   %spec = $ModernHUD::Setting[%i, spec];
   %lo   = String::getWord(%spec, "|", 0);
   %hi   = String::getWord(%spec, "|", 1);
   %st   = String::getWord(%spec, "|", 2);
   if(%st == "" || %st == 0) { %st = 1; }
   if(%lo == "") { %lo = 0; }
   if(%hi == "") { %hi = 100; }
   if(%cur == "") { %cur = %lo; }

   %cur = %cur + %dir * %st;
   if(%cur < %lo) { %cur = %lo; }
   if(%cur > %hi) { %cur = %hi; }
   setVariable(%key, %cur);
}

// Restore every registered row to the default its pack declared.
//
// A pack that owns state OUTSIDE its settings rows (engine prefs it drives, a
// derived palette) defines ModernHUDPack::menuReset and gets called after the
// rows are restored -- the registry cannot know about anything it was not told.
function ModernHUD::menuDefaults()
{
   %n = $ModernHUD::SettingCount;
   if(%n == "") { %n = 0; }
   for(%i = 0; %i < %n; %i++)
   {
      %key = $ModernHUD::Setting[%i, key];
      if(%key != "")
         setVariable(%key, $ModernHUD::Setting[%i, dflt]);
   }

   // Put the panel itself back where it started too: a player who dragged it
   // somewhere awkward has no other way to undo that.
   $pref::ModernHUD::MenuX = "";
   $pref::ModernHUD::MenuY = "";
   $ModernHUD::MenuPage    = 0;

   if(isFunction("ModernHUDPack::menuReset"))
      ModernHUDPack::menuReset();
}

// A stepper button: 1px outline that FILLS under the pointer, so it reads as
// pressable before it is clicked. Returns 1 on a fresh click.
function ModernHUD::mStep(%x, %y, %s, %glyph, %mx, %my)
{
   if(ModernHUD::mHit(%mx, %my, %x, %y, %s, %s))
   {
      ModernHUD::mColor($ModernHUD::MenuPrimary, 255);
      glRectangle(%x, %y, %s, %s);
      ModernHUD::mText(%x, %y + 1, %s, "16 20 24", %glyph, 255, 13, "c");
   }
   else
   {
      ModernHUD::mColor($ModernHUD::MenuPrimary, 130);
      glRectangle(%x, %y, %s, 1);
      glRectangle(%x, %y + %s - 1, %s, 1);
      glRectangle(%x, %y, 1, %s);
      glRectangle(%x + %s - 1, %y, 1, %s);
      ModernHUD::mText(%x, %y + 1, %s, $ModernHUD::MenuPrimary, %glyph, 230, 13, "c");
    }

   if(ModernHUD::mClicked(%mx, %my, %x, %y, %s, %s)) { return 1; }
   return 0;
}

// One row: label on the left margin, a single grouped stepper [-][ value ][+] on
// the right so the control reads as one object rather than three scattered ones.
function ModernHUD::mRow(%x, %y, %w, %i, %mx, %my)
{
   %rowH = 24;
   %pad  = 14;
   %bs   = 16;
   %vw   = 104;

   %bx = %x + %w - %pad - %bs;
   %vx = %bx - %vw;
   %ax = %vx - %bs;
   %by = %y + floor((%rowH - %bs) / 2);

   %over = ModernHUD::mHit(%mx, %my, %x, %y, %w, %rowH);
   if(%over)
   {
      ModernHUD::mColor($ModernHUD::MenuPrimary, 55);
      glGradientRect(%x + 2, %y, %w - 4, %rowH,
                     getWord($ModernHUD::MenuPrimary, 0),
                     getWord($ModernHUD::MenuPrimary, 1),
                     getWord($ModernHUD::MenuPrimary, 2), 0, "h");
      ModernHUD::mColor($ModernHUD::MenuPrimary, 255);
      glRectangle(%x + 2, %y + 4, 3, %rowH - 8);
   }

   %lc = $ModernHUD::MenuText;
   if(!%over) { %lc = "150 165 180"; }

   ModernHUD::mText(%x + %pad, %y + 5, %vx - %x - %pad, %lc,
                    $ModernHUD::Setting[%i, label], 245, 12, "l");
   ModernHUD::mText(%vx, %y + 5, %vw, $ModernHUD::MenuAccent,
                    ModernHUD::rowShown(%i), 255, 12, "c");

   if(ModernHUD::mStep(%ax, %by, %bs, "-", %mx, %my) == 1) { ModernHUD::rowStep(%i, -1); }
   if(ModernHUD::mStep(%bx, %by, %bs, "+", %mx, %my) == 1) { ModernHUD::rowStep(%i,  1); }
}

// Default chrome: opaque dark base, thin theme tint, accent spine, hairlines.
// ★Opaque on purpose.★ A settings panel is modal furniture, not a HUD element:
// contrast has to come from the panel, not from whatever happens to be behind it.
function ModernHUD::mFrame(%x, %y, %w, %h, %head)
{
   if(isFunction("ModernHUDPack::menuFrame"))
   {
      ModernHUDPack::menuFrame(%x, %y, %w, %h, %head);
      return;
   }

   ModernHUD::mColor("10 13 16", 252);
   glRectangle(%x, %y, %w, %h);
   ModernHUD::mColor($ModernHUD::MenuDim, 90);
   glRectangle(%x, %y, %w, %h);

   ModernHUD::mColor($ModernHUD::MenuPrimary, 255);
   glRectangle(%x, %y, 3, %h);
   ModernHUD::mColor($ModernHUD::MenuPrimary, 90);
   glRectangle(%x + %w - 1, %y, 1, %h);
   glRectangle(%x, %y, %w, 1);
   glRectangle(%x, %y + %h - 1, %w, 1);

   ModernHUD::mColor($ModernHUD::MenuPrimary, 40);
   glGradientRect(%x + 3, %y + 1, %w - 4, %head - 2,
                  getWord($ModernHUD::MenuPrimary, 0),
                  getWord($ModernHUD::MenuPrimary, 1),
                  getWord($ModernHUD::MenuPrimary, 2), 0);
   ModernHUD::mColor($ModernHUD::MenuPrimary, 220);
   glRectangle(%x + 3, %y + %head - 2, %w - 4, 2);
}

//------------------------------------------------------------------------------
// The panel. Drawn LAST, over the pack's own art, while the K panel is open.
//------------------------------------------------------------------------------
function ModernHUD::menu(%screen)
{
   %n = $ModernHUD::SettingCount;
   if(%n == "") { %n = 0; }
   if(%n <= 0) { return; }

   if($Config::HudListVisible != 1)
   {
      $ModernHUD::MenuDown = "";
      $ModernHUD::MenuDrag = "";
      return;
   }

   %sw = getWord(%screen, 0);
   %sh = getWord(%screen, 1);

   // ★Reset the part scale before drawing ANYTHING.★ ModernHUD::part pushes a
   // glPartScale per part and it stays active for the rest of the ScriptGL pass,
   // so a menu drawn after the last part inherits that part's scale -- pixels
   // scaled, hit rects not, which is exactly where a pointer mismatch comes from.
   // glPartScale with scale 1 pops the active one and short-circuits before
   // pushing, so this is an identity reset.
   glPartScale(0, 0, 1);

   %rowH = 24;
   %head = 34;
   %foot = 34;
   %w    = 348;

   // Page when the rows cannot fit the screen. A pack may register up to 32 rows
   // and the panel must not run off a 768-high display.
   %perPage = floor((%sh - 140 - %head - %foot) / %rowH);
   if(%perPage < 4) { %perPage = 4; }
   if(%perPage > %n) { %perPage = %n; }
   %pages = floor((%n + %perPage - 1) / %perPage);

   %page = $ModernHUD::MenuPage;
   if(%page == "" || %page < 0) { %page = 0; }
   if(%page >= %pages) { %page = %pages - 1; }

   %first = %page * %perPage;
   %count = %n - %first;
   if(%count > %perPage) { %count = %perPage; }

   %h = %head + %count * %rowH + %foot;

   // Right-aligned by default: centre is where reticles live, so the panel would
   // cover the exact thing it configures. Drag the header to move it; the
   // position persists like any other pref.
   %x = $pref::ModernHUD::MenuX;
   %y = $pref::ModernHUD::MenuY;
   if(%x == "") { %x = %sw - %w - 40; }
   if(%y == "") { %y = floor((%sh - %h) / 2); }

   %m   = glMousePos();
   %mx  = getWord(%m, 0);
   %my  = getWord(%m, 1);
   %lmb = getWord(%m, 2);

   $ModernHUD::MenuClick = "";
   if(%lmb == 1 && $ModernHUD::MenuDown != 1) { $ModernHUD::MenuClick = 1; }
   $ModernHUD::MenuDown = %lmb;

   // Drag by the header. The grab OFFSET is stored, not the centre, so the panel
   // does not jump under the cursor on the first frame.
   if(%lmb != 1)
   {
      $ModernHUD::MenuDrag = "";
   }
   else if($ModernHUD::MenuDrag == 1)
   {
      %x = %mx - $ModernHUD::MenuDragDX;
      %y = %my - $ModernHUD::MenuDragDY;
   }
   else if($ModernHUD::MenuClick == 1 &&
           ModernHUD::mHit(%mx, %my, %x, %y, %w, %head))
   {
      $ModernHUD::MenuDrag   = 1;
      $ModernHUD::MenuDragDX = %mx - %x;
      $ModernHUD::MenuDragDY = %my - %y;
   }

   // Clamp AFTER the drag so the header always stays grabbable.
   if(%x < 0)           { %x = 0; }
   if(%y < 0)           { %y = 0; }
   if(%x > %sw - 60)    { %x = %sw - 60; }
   if(%y > %sh - %head) { %y = %sh - %head; }

   $pref::ModernHUD::MenuX = %x;
   $pref::ModernHUD::MenuY = %y;

   ModernHUD::mFrame(%x, %y, %w, %h, %head);

   %title = $ModernHUD::MenuTitle;
   if(%title == "") { %title = String::toUpper(ModernHUD::current()); }
   if(%title == "") { %title = "HUD"; }

   glSetFont($ModernHUD::MenuFont, 16);
   %tw = getWord(glGetStringDimensions(%title), 0);
   ModernHUD::mText(%x + 14, %y + 8, 200, $ModernHUD::MenuPrimary, %title, 255, 16, "l");
   ModernHUD::mText(%x + 14 + %tw + 10, %y + 13, 200, "120 135 150",
                    "HUD SETTINGS", 210, 10, "l");
   ModernHUD::mText(%x - 14, %y + 13, %w, "120 135 150", "K to close", 200, 10, "r");

   %ry = %y + %head + 1;
   for(%r = 0; %r < %count; %r++)
   {
      ModernHUD::mRow(%x, %ry, %w, %first + %r, %mx, %my);
      %ry = %ry + %rowH;
   }

   // -- footer: reset, and the pager when there is more than one page ----------
   %fy = %y + %h - %foot;
   ModernHUD::mColor($ModernHUD::MenuPrimary, 70);
   glRectangle(%x + 3, %fy, %w - 4, 1);

   %bw = 118;
   %bh = 18;
   %bx = %x + 14;
   %by = %fy + floor((%foot - %bh) / 2);
   %over = ModernHUD::mHit(%mx, %my, %bx, %by, %bw, %bh);

   if(%over)
   {
      ModernHUD::mColor($ModernHUD::MenuWarn, 255);
      glRectangle(%bx, %by, %bw, %bh);
      ModernHUD::mText(%bx, %by + 3, %bw, "16 20 24", "RESET DEFAULTS", 255, 10, "c");
   }
   else
   {
      ModernHUD::mColor($ModernHUD::MenuWarn, 150);
      glRectangle(%bx, %by, %bw, 1);
      glRectangle(%bx, %by + %bh - 1, %bw, 1);
      glRectangle(%bx, %by, 1, %bh);
      glRectangle(%bx + %bw - 1, %by, 1, %bh);
      ModernHUD::mText(%bx, %by + 3, %bw, $ModernHUD::MenuWarn, "RESET DEFAULTS", 240, 10, "c");
   }
   if(%over && $ModernHUD::MenuClick == 1) { ModernHUD::menuDefaults(); }

   if(%pages > 1)
   {
      %ps = 16;
      %py = %fy + floor((%foot - %ps) / 2);
      %pxR = %x + %w - 14 - %ps;
      %pxL = %pxR - 74;

      if(ModernHUD::mStep(%pxL, %py, %ps, "<", %mx, %my) == 1) { %page--; }
      if(ModernHUD::mStep(%pxR, %py, %ps, ">", %mx, %my) == 1) { %page++; }
      if(%page < 0)       { %page = %pages - 1; }
      if(%page >= %pages) { %page = 0; }

      ModernHUD::mText(%pxL + %ps, %py + 2, 58, $ModernHUD::MenuAccent,
                       (%page + 1) @ " / " @ %pages, 235, 10, "c");
   }
   else
   {
      ModernHUD::mText(%x - 14, %by + 5, %w, "90 105 118",
                       "drag title to move", 195, 9, "r");
   }

   $ModernHUD::MenuPage = %page;
}

// Compact out any $Hud entry whose control is gone.
//
// ★Measured: 9480 lines of ": Unknown command." in one session's console.log★,
// alongside `addToSet: Object "ModernHUD::CtfClock" doesn't exist`. A hud whose
// object had been deleted elsewhere kept its slot in the shared $Hud inventory, so
// every gui open re-ran Hud::OnGuiOpen over it: addToSet on a dead handle, then
// `*$Hud::Huds[%i, wake]()` on an empty string -- a dispatch of the empty name,
// once per stale entry per gui transition, forever.
//
// Whoever deleted the control is not the point: an entry pointing at nothing is
// broken regardless, and this is the one place that can promise the inventory is
// consistent after a pack goes away. Walk BACKWARDS -- removal moves the last
// entry into the hole.
function ModernHUD::sweepDeadHuds()
{
   %removed = 0;
   for(%i = $Hud::Count - 1; %i >= 0; %i--)
   {
      %handle = $Hud::Huds[%i];
      if(%handle != "" && isObject(%handle))
         continue;

      %name = $Hud::Huds[%i, name];
      %last = $Hud::Count - 1;
      if(%name != "")
      {
         $Hud::Huds[%name] = "";
         $Module::hudOwner[%name] = "";
      }
      if(%i != %last)
      {
         $Hud::Huds[%i] = $Hud::Huds[%last];
         $Hud::Huds[%i, name] = $Hud::Huds[%last, name];
         $Hud::Huds[%i, wake] = $Hud::Huds[%last, wake];
         $Hud::Huds[%i, sleep] = $Hud::Huds[%last, sleep];
      }
      $Hud::Huds[%last] = "";
      $Hud::Huds[%last, name] = "";
      $Hud::Huds[%last, wake] = "";
      $Hud::Huds[%last, sleep] = "";
      $Hud::Count = %last;
      %removed++;
   }
   if(%removed > 0)
      echo("[MODERNHUD] swept " @ %removed @ " dead hud entr(ies) from $Hud::Huds");
   return %removed;
}

function ModernHUD::debug(%enabled)
{
   $ModernHUD::Debug = (%enabled != 0);
   echo("[MH-DEBUG] " @ ($ModernHUD::Debug ? "BEGIN" : "END"));
}

function ModernHUD::place(%anchor, %offsetX, %offsetY, %contentW, %contentH, %screen)
{
   %screenW = getWord(%screen, 0);
   %screenH = getWord(%screen, 1);

   if(%anchor == "top-left")
      return %offsetX @ " " @ %offsetY;
   if(%anchor == "top-center")
      return floor((%screenW - %contentW) / 2) + %offsetX @ " " @ %offsetY;
   if(%anchor == "top-right")
      return %screenW - %contentW - %offsetX @ " " @ %offsetY;
   if(%anchor == "center-left")
      return %offsetX @ " " @ floor((%screenH - %contentH) / 2) + %offsetY;
   if(%anchor == "center")
      return floor((%screenW - %contentW) / 2) + %offsetX @ " " @
             floor((%screenH - %contentH) / 2) + %offsetY;
   if(%anchor == "center-right")
      return %screenW - %contentW - %offsetX @ " " @
             floor((%screenH - %contentH) / 2) + %offsetY;
   if(%anchor == "bottom-left")
      return %offsetX @ " " @ %screenH - %contentH - %offsetY;
   if(%anchor == "bottom-center")
      return floor((%screenW - %contentW) / 2) + %offsetX @ " " @
             %screenH - %contentH - %offsetY;
   if(%anchor == "bottom-right")
      return %screenW - %contentW - %offsetX @ " " @
             %screenH - %contentH - %offsetY;

   return %offsetX @ " " @ %offsetY;
}

function ModernHUD::image(%anchor, %offsetX, %offsetY, %path, %alpha, %style, %screen)
{
   %size = glGetImageDimensions(%path);
   %w = getWord(%size, 0);
   %h = getWord(%size, 1);
   if(%w <= 0 || %h <= 0)
      return "0 0";

   %at = ModernHUD::place(%anchor, %offsetX, %offsetY, %w, %h, %screen);
   glDrawImage(getWord(%at, 0), getWord(%at, 1), %w, %h, %path,
               ModernHUD::alphaOf(%alpha), %style);
   return %w @ " " @ %h;
}

function ModernHUD::imageAt(%x, %y, %path, %alpha, %style)
{
   %size = glGetImageDimensions(%path);
   %w = getWord(%size, 0);
   %h = getWord(%size, 1);
   if(%w > 0 && %h > 0)
      glDrawImage(%x, %y, %w, %h, %path, ModernHUD::alphaOf(%alpha), %style);
   return %w @ " " @ %h;
}

function ModernHUD::digitsWidth(%folder, %value, %spacing)
{
   %width = 0;
   %count = String::len(%value);
   for(%i = 0; %i < %count; %i++)
   {
      %digit = String::getSubStr(%value, %i, 1);
      %size = glGetImageDimensions(%folder @ "/" @ %digit @ ".png");
      %dw = getWord(%size, 0);
      if(%dw > 0)
      {
         if(%width > 0)
            %width += %spacing;
         %width += %dw;
      }
   }
   return %width;
}

function ModernHUD::digitsAt(%x, %y, %folder, %value, %alpha, %spacing)
{
   %count = String::len(%value);
   for(%i = 0; %i < %count; %i++)
   {
      %digit = String::getSubStr(%value, %i, 1);
      %path = %folder @ "/" @ %digit @ ".png";
      %size = glGetImageDimensions(%path);
      %dw = getWord(%size, 0);
      %dh = getWord(%size, 1);
      if(%dw > 0 && %dh > 0)
      {
         glDrawImage(%x, %y, %dw, %dh, %path, ModernHUD::alphaOf(%alpha));
         %x += %dw + %spacing;
      }
   }
   return %x;
}

//----------------------------------------------------------------------------
// Generated-pack runtime (ModernHUD pack format v1).
//
// Everything below is what a GENERATED hud.cs calls. It is deliberately the same
// machinery Tribes_Overstep.phase_a.cs proved by hand -- the handle identity
// rule, the reset generation, the setSessionPos-not-position rule -- lifted here
// once so five generated packs cannot drift from it.
// Format: re/modernhud_pack_format_v1.md
//----------------------------------------------------------------------------

// Restore one hud's saved position from $pref::hudPositions.
//
// This is a MODERNHUD-NAMESPACED copy of the legacy packs' Hud::Restore
// (config/Core/Hud.cs:45), with identical semantics -- read "x y||fx fy fz",
// apply position AND fracPos.
//
// It exists because ModernHUD::handle used to call Hud::Restore directly, and that
// function is defined ONLY in a legacy pack's Core/Hud.cs. Under a ModernHUD pack
// that file never runs, so the call logged "Hud::Restore: Unknown command." every
// time a part was created and ★the player's dragged position was silently never
// restored★ -- parts snapped back to their authored default.
//
// It must NOT be named Hud::Restore. The console has ONE namespace, a legacy pack
// defines that name too, and last-loaded would win -- that is defect class 4
// (function collisions) in re/modern_hud_master_config_architecture.md.
function ModernHUD::restorePos(%i)
{
   %handle = $Hud::Huds[%i];
   %name   = $Hud::Huds[%i, name];
   // Phase 1: pack-qualified, with the one-time claim of any pre-Phase-1 value.
   %saved  = ModernHUD::posOf(%name);
   if($pref::hudSlotDiag)
      echo("[MHPOS] restore " @ ModernHUD::qualify(%name) @ " = [" @ %saved @ "]");
   if(%saved == "")
      return;
   // An all-zero entry written by an older build says "never moved", not "put this
   // in the corner". Applying it is what collapsed Basic's eight parts onto each
   // other at the top-left; leaving it alone lets the pack's authored anchor+offset
   // stand. See ModernHUD::isEmptyLayout.
   if(ModernHUD::isEmptyLayout(%saved))
   {
      if($pref::hudSlotDiag)
         echo("[MHPOS] restore " @ ModernHUD::qualify(%name) @ " IGNORED (no layout)");
      return;
   }
   if(String::Explode(%saved, "||", "fields") != 2)
      return;
   %handle.position = $fields[0];
   %handle.fracPos  = $fields[1];
}

// A movable, responsive hit target for one part. Returns "x y" to draw at.
//
// Identity is the numeric SimObject id, NEVER the name: object names containing
// "::" do not round-trip through isObject(name) in this console, and testing the
// name recreated the handle every draw -- thousands of same-named controls, with
// K moving the newest while Control::GetPosition resolved an older one.
function ModernHUD::handle(%name, %defaultPos, %w, %h)
{
   $ModernHUD::DefaultPos[%name] = %defaultPos;

   %handle = $ModernHUD::Handle[%name];
   if(!isObject(%handle))
   {
      %handle = newObject(%name, FearGui::ModernHudHandle,
                          getWord(%defaultPos, 0), getWord(%defaultPos, 1), %w, %h);
      $ModernHUD::Handle[%name] = %handle;

      // ★An UNSET counter is not zero -- it is the empty string, and it makes a
      // DIFFERENT VARIABLE.★ `$A[%i]` names its global by concatenating the base
      // with the evaluated index (VarNode::eval, eval.cpp:570-586), so with %i ""
      // the name is plain `A`, not `A0`. The first registration therefore went to
      // an unindexed `$ModernHUD::HandleName`, `++` turned "" into 1, and index 0
      // was never written at all.
      //
      // Measured in the field: with three parts registered the inventory read
      //   mh0 [] def=            <- the FIRST part, invisible to every loop
      //   mh1 [ModernHUD::VectorCtf]
      //   mh2 [ModernHUD::VectorItems]
      // ModernHUD::resetPositions walks 0..count-1 and skips a blank name, so
      // "Reset HUD positions" silently did nothing for whichever part registered
      // first -- reported against Vector, whose first part is the whole reticle.
      //
      // ★Why this pack and not the converted ones:★ the counters are normally
      // already numeric because a legacy config's own Hud.cs modules populated
      // them first. A pack with no legacy modules is the first thing to touch
      // them, so it is the first to hit the unset case.
      %hc = $ModernHUD::HandleCount;
      if(%hc == "") %hc = 0;
      if(!$ModernHUD::HandleRegistered[%name])
      {
         $ModernHUD::HandleName[%hc] = %name;
         $ModernHUD::HandleCount = %hc + 1;
         $ModernHUD::HandleRegistered[%name] = true;
      }

      // Join the pack's normal Hud inventory so the existing Store/Restore/exit
      // hooks persist this part's position like any other hud. Same unset-counter
      // trap as above -- $Hud::Count is equally empty on a pack with no modules.
      %n = $Hud::Count;
      if(%n == "") %n = 0;
      $Hud::Huds[%n] = %handle;
      $Hud::Huds[%n, name] = %name;
      $Hud::Huds[%n, wake] = "ModernHUD::noop";
      $Hud::Huds[%n, sleep] = "ModernHUD::noop";
      $Hud::Huds[%name] = %handle;
      $Hud::Count = %n + 1;
      ModernHUD::restorePos(%n);
      addToSet(playGui, %handle);

      // E2 addendum: this creation is one of the TWO approved registration
      // chokepoints (the other is ModernHUD::stock). The exact numeric id joins
      // the native editor-target registry; while a ModernHUD-owned K panel is
      // open, nothing outside that registry can hover, outline, or capture.
      HudEditor::addTarget(%handle);

      $ModernHUD::AppliedReset[%name] = $ModernHUD::ResetGeneration;

      // A saved position from a bigger canvas can be off-screen entirely; place it
      // where this screen can show it. Only ONCE per creation -- a position the
      // player dragged this session is never second-guessed.
      //
      // ★DEFERRED, because the position is not readable yet.★ Calling it here read
      // an empty Control::GetPosition and therefore snapped EVERY part of EVERY
      // pack to its authored default on every single load -- the check never once
      // did its real job. Arm it instead, and run it on the first draw where the
      // control actually answers, which is the first moment the question
      // "does this fit on screen" has an answer at all.
      $ModernHUD::FitPending[%name] = 1;
   }

   Control::SetVisible(%handle, true);
   Control::SetExtent(%handle, %w, %h);

   // Reset consumes the authored position computed by THIS draw, so responsive
   // defaults stay correct at every resolution. setSessionPos, not a bare
   // position write: the latter leaves a stale fracPos and the next resize
   // walks the part back.
   if($ModernHUD::AppliedReset[%name] != $ModernHUD::ResetGeneration)
   {
      Hud::setSessionPos(%handle, getWord(%defaultPos, 0), getWord(%defaultPos, 1));
      $ModernHUD::AppliedReset[%name] = $ModernHUD::ResetGeneration;
   }

   // The armed fits-check, run on the first draw where the control answers with a
   // real position (see the FitPending comment above). Exactly once per creation.
   if($ModernHUD::FitPending[%name] != "" && Control::GetPosition(%handle) != "")
   {
      $ModernHUD::FitPending[%name] = "";
      ModernHUD::fitOnScreen(%name, %handle, %defaultPos, %w, %h);
   }

   %published = $ModernHUD::HandlePos[%name];
   if(%published == "")
      %published = Control::GetPosition(%handle);

   ModernHUD::recordRect(%name, %published, %w, %h);
   return %published;
}

function ModernHUD::noop()
{
}

//----------------------------------------------------------------------------
// Where each part actually drew, for the parity harness.
//
// ★A whole-frame diff cannot localise anything.★ The one parity number this
// project had was "Overstep 10.03% changed vs 2% threshold" -- a fail nobody
// could act on, because it could not say WHICH part moved, and it counted the
// minimap, the fps counter and the match clock as pack error. The thresholds file
// has always specified per-part boxes and masked regions; nothing published the
// boxes, so neither could be evaluated.
//
// Recorded here rather than derived from pack.json because the manifest holds an
// anchor and an offset, not a rectangle: the actual box depends on the live
// canvas, the player's dragged position, and any per-part scale. Only the draw
// knows it.
//----------------------------------------------------------------------------
function ModernHUD::recordRect(%name, %pos, %w, %h)
{
   if(%name == "" || %pos == "")
      return;
   if($ModernHUD::RectSeen[%name] == "")
   {
      $ModernHUD::RectName[$ModernHUD::RectCount] = %name;
      $ModernHUD::RectCount++;
      $ModernHUD::RectSeen[%name] = 1;
   }
   $ModernHUD::Rect[%name] = getWord(%pos, 0) @ " " @ getWord(%pos, 1) @ " " @ %w @ " " @ %h;
}

// Cleared with the rest of the per-pack state so a swap cannot leave the previous
// pack's boxes behind and have the harness measure against the wrong rectangles.
function ModernHUD::clearRects()
{
   for(%i = 0; %i < $ModernHUD::RectCount; %i++)
   {
      %n = $ModernHUD::RectName[%i];
      $ModernHUD::Rect[%n] = "";
      $ModernHUD::RectSeen[%n] = "";
      $ModernHUD::RectName[%i] = "";
   }
   $ModernHUD::RectCount = 0;
}

// JSON so the harness needs no parser of its own:
//   [{"name":"ModernHUD::Health","x":260,"y":526,"w":120,"h":40}, ...]
function ModernHUD::rectsJson()
{
   %out = "[";
   %first = true;
   for(%i = 0; %i < $ModernHUD::RectCount; %i++)
   {
      %n = $ModernHUD::RectName[%i];
      %r = $ModernHUD::Rect[%n];
      if(%r == "")
         continue;
      // Track "have I emitted one yet", not the loop index: a skipped empty entry
      // would otherwise put a comma before the first real object and produce JSON
      // the harness cannot parse.
      if(!%first)
         %out = %out @ ",";
      %first = false;
      %out = %out @ "{\"name\":\"" @ %n @ "\",\"x\":" @ getWord(%r, 0)
                  @ ",\"y\":" @ getWord(%r, 1) @ ",\"w\":" @ getWord(%r, 2)
                  @ ",\"h\":" @ getWord(%r, 3) @ "}";
   }
   return %out @ "]";
}

// If a just-restored handle would sit off-screen, put it at the authored default
// computed for THIS screen. Returns "" when it was left alone.
//
// ★The bug this fixes, reported from a live session: after switching back to
// Tribes_Overstep most of its HUD did not appear until the window was resized,
// then "popped in".★ Overstep is authored against a 2560x1440 canvas
// (play.gui.cs), and an immediate-mode handle is created LAZILY ON THE FIRST DRAW
// -- after the swap has finished -- so every anchor pass that runs during a swap
// finds nothing to anchor. Hud::Restore then applies the saved ABSOLUTE position,
// which on a smaller window is simply outside it. Nothing was broken; the parts
// were being drawn where nobody could see them.
//
// The resize "fixed" it because HudCtrl::parentResized re-projects any hud that
// does not fit (fearGuiHudCtrl.cpp) -- so this uses that same fits-entirely rule
// rather than inventing a second one, and the result is that a pack load now
// behaves like the resize the player had to do by hand.
//
// Generated packs never showed this: their parts recompute anchor+offset from
// %screen on every draw, so they are on-screen by construction. That difference
// is what identified the cause.
function ModernHUD::fitOnScreen(%name, %handle, %defaultPos, %w, %h)
{
   %screen = Control::GetExtent(playGui);
   %sw = getWord(%screen, 0);
   %sh = getWord(%screen, 1);
   if(%sw <= 0 || %sh <= 0)
      return "";

   %pos = Control::GetPosition(%handle);

   // ★An UNAVAILABLE position is not an off-screen position.★ This runs one line
   // after ModernHUD::restorePos on a handle that was created and parented in the
   // same breath, and a control with no realized position yet answers
   // Control::GetPosition with the EMPTY STRING. getWord("", 0) then returns the
   // literal "-1" (FearPlugin.cpp c_getWord returns "-1" past the end of a string,
   // and an empty string is always past its end) -- so the fits-check below read
   // -1 -1, decided the part was off-screen, and snapped it to %defaultPos,
   // destroying the position restorePos had just applied.
   //
   // ★Measured, not reasoned:★ with $ModernHUD::Debug on, every Basic part logged
   //   [MH-FIT] ModernHUD::GHealth_Container restored to -1 -1 (100x20)
   //            which is outside 954x698; placed at 0 0
   // immediately after
   //   [MHPOS] restore basic::ModernHUD::GHealth_Container = [300 400||...]
   //
   // This is why "Basic's HUD collapsed to the top-left after basic -> proconfig
   // -> basic": Basic's legacy modules position themselves, so its converted
   // manifest carries anchor "top-left" offset 0,0 for all eight parts and its
   // %defaultPos IS the origin -- the snap stacks the entire HUD at 0 0. Other
   // packs were snapped just as wrongly (proconfig logged "placed at 16 132") but
   // their authored anchors are real, so they landed close enough to pass unnoticed.
   //
   // Same class as the getWord trap already documented in ModernHUD::drawBorrowed.
   if(%pos == "")
      return "";

   %px = getWord(%pos, 0);
   %py = getWord(%pos, 1);
   if(%px >= 0 && %py >= 0 && %px + %w <= %sw && %py + %h <= %sh)
      return "";

   // setSessionPos, not a bare position write: it updates the retained resize
   // state too, so the next resize does not walk the part straight back out.
   Hud::setSessionPos(%handle, getWord(%defaultPos, 0), getWord(%defaultPos, 1));
   if($ModernHUD::Debug)
      echo("[MH-FIT] " @ %name @ " restored to " @ %px @ " " @ %py @
           " (" @ %w @ "x" @ %h @ ") which is outside " @ %sw @ "x" @ %sh @
           "; placed at " @ %defaultPos);
   return %defaultPos;
}

//----------------------------------------------------------------------------
// Framework-owned event lifecycle.
//
// A converted pack's DATA layer (the carried Core/*.cs modules that maintain
// $Team::Score, flag locations, timers) is event-driven, so it must register
// handlers -- but ★a pack's own handlers survived its teardown in the legacy
// runtime and kept firing under the next pack loaded★ (documented, commit
// ec1b8c0). So the framework registers them and the framework revokes them:
// ModernHUD::attach is the only way in, and unload guarantees the way out.
//
// Presto's Event::Attach defaults a handler's tag to the handler NAME, and
// Event::Detach removes by tag, so (event, function) is a complete identity --
// no tag bookkeeping of our own is needed, and re-attaching is idempotent.
//----------------------------------------------------------------------------
function ModernHUD::attach(%event, %fn)
{
   if($ModernHUD::AttachSeen[%event, %fn])
      return;
   $ModernHUD::AttachSeen[%event, %fn] = true;
   $ModernHUD::AttachEvent[$ModernHUD::AttachCount] = %event;
   $ModernHUD::AttachFn[$ModernHUD::AttachCount] = %fn;
   $ModernHUD::AttachCount++;
   Event::Attach(%event, %fn);
}

function ModernHUD::detachAll()
{
   for(%i = 0; %i < $ModernHUD::AttachCount; %i++)
   {
      %event = $ModernHUD::AttachEvent[%i];
      %fn = $ModernHUD::AttachFn[%i];
      if(%event != "")
      {
         Event::Detach(%event, %fn);
         $ModernHUD::AttachSeen[%event, %fn] = "";
      }
      $ModernHUD::AttachEvent[%i] = "";
      $ModernHUD::AttachFn[%i] = "";
   }
   $ModernHUD::AttachCount = 0;
}

// exec a data module once. Re-exec is harmless (last definition wins) but it
// would re-run top-level statements, so guard it.
function ModernHUD::require(%file)
{
   if($ModernHUD::Required[%file])
      return;
   $ModernHUD::Required[%file] = true;
   exec(%file);
}

// anchor+offset -> a placed, movable part box. The one call a generated part
// makes before drawing its elements.
function ModernHUD::part(%name, %anchor, %offsetX, %offsetY, %w, %h, %screen)
{
   %at = ModernHUD::place(%anchor, %offsetX, %offsetY, %w, %h, %screen);
   %at = ModernHUD::handle(%name, %at, %w, %h);

   // Apply the handle's saved resize as a DRAW scale about the part origin.
   // The retained handle already scales its own grab box (HudCtrl
   // cfgApplyUserExtent); without this the content ignored the resize -- "it
   // just resizes the bounding box". One glPartScale is active at a time: the
   // next part's call (or the end of the ModernHUD pass) replaces it.
   %scale = ModernHUD::scaleOfPart(%name);
   if(%scale == "")
      %scale = 1;
   // ...times the pack-wide size multiplier (the universal "HUD size" row), so
   // one control scales every part while a per-part drag still adjusts one.
   %scale = ModernHUD::scaleOf(%scale);
   glPartScale(getWord(%at, 0), getWord(%at, 1), %scale);
   return %at;
}

// A part that DOCKS to another control instead of to a screen anchor.
//
// v0dkA's ChatOverlay is a frame drawn around the chat: its legacy module read
// chatDisplayHud's position every 0.1s and moved itself to it (ChatBorder.acs.cs
// ChatOverlay::Update). A ModernHUD part is drawn every frame, so the timer is
// unnecessary -- but the DOCKING is not, and without it the frame just sits wherever
// it was last placed, 664x89 and empty-looking, nowhere near the chat.
//
// Returns the position to draw at: the docked one when the target exists, otherwise
// %fallback (the normal anchor result) so a part never vanishes because its target is
// absent -- chatDisplayHud does not exist on every gui.
//
// Dragging a docked part is intentionally futile: the legacy version overwrote its own
// position every 0.1s too, so this is faithful, not a new limitation.
function ModernHUD::dockTo(%name, %target, %dx, %dy, %fallback, %partW, %partH)
{
   %pos = Control::GetPosition(%target);
   if(%pos == "")
      return %fallback;

   %x = getWord(%pos, 0) + %dx;
   %y = getWord(%pos, 1) + %dy;

   %handle = $ModernHUD::Handle[%name];
   if(isObject(%handle))
   {
      Hud::setSessionPos(%handle, %x, %y);

      // ★A docked part is a DECORATION, and a decoration is not an editor
      // target★ (chat/minimap resize handoff): its geometry is derived from the
      // owner every draw, so a grip on it would be a second, lying handle on
      // one visible HUD -- vodka's chat border and radar art must follow the
      // real chat/minimap, never compete with them. The handle registered
      // itself in ModernHUD::handle; revoke it here, every draw, so no path
      // that re-registers survives. removeTarget is idempotent.
      ModernHUD::editorDecoration(%name);
   }

   // SIZE to the live target too, not just position: the authored frame (e.g.
   // v0dkA's 664x89 ChatBG) was drawn for the pack's authored chat extent, so on
   // a live chat of any other size it floats too wide/tall. Map the authored
   // part box onto (target extent + the authored borders) -- the dock offsets
   // ARE the borders (frame origin sits -dx,-dy outside the target) -- with a
   // non-uniform draw scale about the docked origin. Replaces any part() scale.
   if(%partW > 0 && %partH > 0)
   {
      %ext = Control::getExtent(%target);
      if(%ext != "")
      {
         %w = getWord(%ext, 0) - (2 * %dx);
         %h = getWord(%ext, 1) - (2 * %dy);
         if(%w > 0 && %h > 0)
            glPartScale(%x, %y, %w / %partW, %h / %partH);
      }
   }

   // `@ " " @`, never SPC: this console's grammar defines '@' for concatenation
   // (engine/console/code/gram.y:119,333) and has NO SPC/TAB/NL operators -- those
   // are TorqueScript, a later engine. SPC here is a hard Syntax error that aborts
   // the REST OF THE FILE, which is why it also took out detachContainer below.
   return %x @ " " @ %y;
}

function ModernHUD::hide(%name)
{
   %handle = $ModernHUD::Handle[%name];
   if(isObject(%handle))
      Control::SetVisible(%handle, false);
}

//----------------------------------------------------------------------------
// Explicit editor-target API (chat/minimap resize handoff).
//
// ModernHUD::stock couples registration to the visibility/settings-row
// machinery, but a pack may drive a native control's visibility from its own
// logic (Overstep's chat, Vector's minimap) and that control still needs to be
// editable. editTarget is registration ALONE; stock delegates to it.
//----------------------------------------------------------------------------
function ModernHUD::editTarget(%ctrl)
{
   HudEditor::addTarget(%ctrl);
}

// The inverse: a decorative handle (a border or ring that follows a real
// owner) must not be a competing editor target. One visible HUD, one grip.
function ModernHUD::editorDecoration(%name)
{
   %handle = $ModernHUD::Handle[%name];
   if(%handle != "")
      HudEditor::removeTarget(%handle);
}

// Fixed-size image draw (0 w/h = native size).
function ModernHUD::imageRect(%x, %y, %w, %h, %path, %alpha, %tint)
{
   if(%w <= 0 || %h <= 0)
   {
      %size = glGetImageDimensions(%path);
      if(%w <= 0) %w = getWord(%size, 0);
      if(%h <= 0) %h = getWord(%size, 1);
   }
   if(%w > 0 && %h > 0)
      glDrawImage(%x, %y, %w, %h, %path, ModernHUD::alphaOf(%alpha), %tint);
   return %w @ " " @ %h;
}

// A progress bar: reveal the left %w pixels of the art, do not squash it.
// The retained packs animate a bar by shrinking a control's EXTENT over a static
// <B0,0:bar.png>, which CROPS the bitmap through the clip rect -- so the
// immediate equivalent is a source crop, not a stretch.
function ModernHUD::bar(%x, %y, %w, %h, %path, %alpha)
{
   if(%w <= 0)
      return 0;
   glDrawImagePart(%x, %y, 0, 0, %w, %h, %path, ModernHUD::alphaOf(%alpha));
   return %w;
}

// The retained packs' text vocabulary, immediate. glDrawMarkup runs the engine's
// own SimGui::TextFormat, so all five xFont tags render exactly as they do in a
// FearGuiFormattedText -- see re/modernhud_pack_format_v1.md section 4.
function ModernHUD::markup(%x, %y, %width, %value, %alpha)
{
   return glDrawMarkup(%x, %y, %width, %value, ModernHUD::alphaOf(%alpha));
}

// digits inside a fixed field: align left/center/right within %box pixels.
function ModernHUD::digitsBox(%x, %y, %folder, %value, %alpha, %spacing, %box, %align)
{
   if(%box > 0 && %align != "" && %align != "left")
   {
      %w = ModernHUD::digitsWidth(%folder, %value, %spacing);
      if(%align == "center")
         %x += floor((%box - %w) / 2);
      else if(%align == "right")
         %x += %box - %w;
   }
   return ModernHUD::digitsAt(%x, %y, %folder, %value, %alpha, %spacing);
}

// Detach one retained container a generated part replaces.
//
// ★Resolve the NUMERIC handle from the Hud registry, never isObject(name):★
// every retained container name carries "::" (CTFHUD::Container, ...), and names
// containing "::" do not round-trip through isObject(name) in this console (see
// ModernHUD::handle above). The old guard therefore skipped ALL work, and the
// invisible legacy box stayed in playGui winning editor hit tests over the
// ModernHUD part that replaced it.
//
// SetVisible(false) BEFORE removal, always: while the control still has a
// root, hiding it contributes its old rectangle to the canvas damage list.
// Removing first clears root, so those pixels can never be invalidated and the
// old HUD stays on screen as a frozen second copy.
//
// Remove through Module::hudRemove so the dense $Hud registry compacts and
// Hud::OnGuiOpen cannot re-adopt the container on the next gui transition; it
// also deletes the object and clears the name map and module owner. Idempotent:
// once removed the name-map entry is blank and the call does nothing.
function ModernHUD::detachContainer(%name)
{
   %h = $Hud::Huds[%name];
   if(%h == "")
      return;
   if(!isObject(%h))
   {
      // Stale name-map entry pointing at an already-deleted object.
      $Hud::Huds[%name] = "";
      return;
   }

   Control::SetVisible(%h, false);

   for(%i = $Hud::Count - 1; %i >= 0; %i--)
   {
      if($Hud::Huds[%i, name] == %name)
      {
         if($pref::hudSlotDiag)
            echo("[HUDPOS] detachContainer " @ %name @ " id=" @ %h @ " idx=" @ %i);
         Module::hudRemove(%i);
         return;
      }
   }

   // In the name map but not the dense index (already compacted out): finish
   // the removal directly.
   removeFromSet(playGui, %h);
   deleteObject(%h);
   $Hud::Huds[%name] = "";
}

// Read-only inventory of every registered editor-interactive HUD: index, name,
// numeric id, liveness, geometry, owning module, and whether it is a ModernHUD
// edit handle. For diagnosing invisible click-eating containers (E2).
function ModernHUD::hudInventory()
{
   echo("[HUDINV] count=" @ $Hud::Count);
   for(%i = 0; %i < $Hud::Count; %i++)
   {
      %nm = $Hud::Huds[%i, name];
      %h = $Hud::Huds[%i];
      echo("[HUDINV] " @ %i @ " [" @ %nm @ "] id=" @ %h
         @ " live=" @ isObject(%h)
         @ " pos=" @ Control::GetPosition(%h)
         @ " ext=" @ Control::GetExtent(%h)
         @ " owner=" @ $Module::hudOwner[%nm]
         @ " mhHandle=" @ ($ModernHUD::HandleRegistered[%nm] != ""));
   }
}

//----------------------------------------------------------------------------
// PHASE 4 -- THE COMPONENT COMPOSITOR
//
// ★What this replaces.★ Cross-pack HUD mixing works today by having
// configModules.cpp execute a LEGACY donor module out of CustomConfigs while a
// ModernHUD pack is selected. Retiring CustomConfigs therefore removes the
// feature unless something else can draw one pack's part while another pack is
// the base. Joe's own saved preset composes six slots across three source
// configs, and the picker currently offers 50 implementations across 17 slots,
// so this is preserving configured behaviour, not adding a feature.
//
// ★Why a registry and not "load two packs".★ Every generated pack defines the
// same globals -- ModernHUDPack::draw, ::ownsSlot, ::draw_<part>. The console has
// ONE namespace, so exec'ing a second pack's hud.cs overwrites the first pack's
// dispatcher and the base pack simply stops drawing. Components must therefore be
// addressable individually, which means a registry keyed on
// <providerPack>/<componentId> and a compositor that calls only the selected ones.
//
// ★Base pack vs component provider.★ One selected pack still owns the default
// composition, global styling and pack settings. A slot may then be overridden by
// a component from a different provider. Registration is what a provider pack does
// at load; selection is a pref; drawing is this compositor.
//----------------------------------------------------------------------------

// Register one drawable component. Called by a pack for each part it is willing
// to let another base pack borrow.
//
//   %provider  pack id that owns the art and the code ("overstep")
//   %component stable logical id within that provider ("weapon")
//   %slot      the native HUD slot it answers ($pref::HudSlot::<slot>)
//   %fn        the draw function, ALREADY pack-qualified by the provider
// ★Legacy slot value -> component id.★ The picker and every saved preset hold
// values like "Tribes_Overstep::WeaponsHud" -- the legacy FOLDER name and MODULE
// name. Those keep working by mapping them here instead of rewriting the
// preference file, so a player who never resaves a preset still gets their
// choice, and a rollback still reads its own data.
//
// Two spellings of the same pack are already persisted on live trees (presets
// store "ProConfigVol4-1.41", HudSlotPos stores the punctuation-stripped
// "ProConfigVol4141"), so both forms are mapped where they differ.
function ModernHUD::mapLegacy(%legacyValue, %componentKey)
{
   $ModernHUD::LegacyComponent[%legacyValue] = %componentKey;
}

function ModernHUD::legacyMap()
{
   // Health / energy
   ModernHUD::mapLegacy("Basic::aHENum",                      "basic/healthenergy");
   ModernHUD::mapLegacy("ProConfigVol4-1.41::HealthNrg",      "proconfig/healthenergy");
   ModernHUD::mapLegacy("ProConfigVol4141::HealthNrg",        "proconfig/healthenergy");
   ModernHUD::mapLegacy("Tribes - Minimalist - v0dkA::HeEnHUD","vodka/healthenergy");
   ModernHUD::mapLegacy("Tribes_Overstep::HeEnHUD",           "overstep/status");
   // Weapon / ammo
   ModernHUD::mapLegacy("Tribes_Overstep::WeaponsHud",        "overstep/weapon");
   ModernHUD::mapLegacy("ProConfigVol4-1.41::AmmoHud",        "proconfig/weapon");
   ModernHUD::mapLegacy("Tribes - Minimalist - v0dkA::WeaponHUD","vodka/weapon");
   // CTF
   ModernHUD::mapLegacy("Tribes_Overstep::numHUD",            "overstep/ctfclock");
   ModernHUD::mapLegacy("Basic::CTFHud",                      "basic/ctf");
   ModernHUD::mapLegacy("ProConfigVol4-1.41::CTFHud",         "proconfig/ctf");
   ModernHUD::mapLegacy("Tribes - Minimalist - v0dkA::CTFHud","vodka/ctf");
   ModernHUD::mapLegacy("Tribes 1.40.655 xLoader::CTFHud",    "xloader/ctf");
   // Items
   ModernHUD::mapLegacy("Tribes_Overstep::ItemHUD",           "overstep/items");
   ModernHUD::mapLegacy("Basic::ItemHUD",                     "basic/items");
   ModernHUD::mapLegacy("Tribes 1.40.655 xLoader::ItemHUD",   "xloader/items");
   // Toasty
   ModernHUD::mapLegacy("Basic::ToastyHUD",                   "basic/toasty");
   ModernHUD::mapLegacy("Tribes - Minimalist - v0dkA::ToastyHUD","vodka/toasty");
   ModernHUD::mapLegacy("Tribes_Overstep::ToastyHUD",         "overstep/toasty");
   // Minimap -- native geometry, so the component is the FRAME only
   ModernHUD::mapLegacy("Tribes_Overstep::minimap",           "overstep/minimap");
}

function ModernHUD::component(%provider, %component, %slot, %fn)
{
   %key = %provider @ "/" @ %component;
   if($ModernHUD::Comp[%key, fn] != "")
      return;                                  // idempotent: re-exec must not duplicate

   %n = $ModernHUD::CompCount;
   if(%n == "") %n = 0;
   $ModernHUD::CompKey[%n]      = %key;
   $ModernHUD::Comp[%key, fn]       = %fn;
   $ModernHUD::Comp[%key, slot]     = %slot;
   $ModernHUD::Comp[%key, provider] = %provider;
   $ModernHUD::CompCount = %n + 1;

   // Reference-count the provider so shared assets/handlers are set up once and
   // torn down only when the LAST selected component from that provider goes.
   %rc = $ModernHUD::Provider[%provider, refs];
   if(%rc == "") %rc = 0;
   $ModernHUD::Provider[%provider, refs] = %rc + 1;
}

// What is selected for a slot, as "<provider>/<component>", or "" for the base.
//
// ★Reads the SAME pref the legacy picker writes.★ $pref::HudSlot::<slot> holds a
// legacy value like "Tribes_Overstep::WeaponsHud" on an unmigrated tree, so the
// legacy id is mapped to a component id here rather than requiring the preference
// file to be rewritten before anything works.
function ModernHUD::slotSelection(%slot)
{
   %sel = getVariable("pref::HudSlot::" @ %slot);
   if(%sel == "" || %sel == "off")
      return "";
   %mapped = $ModernHUD::LegacyComponent[%sel];
   if(%mapped != "")
      return %mapped;
   return %sel;
}

// Draw the component that owns %slot, if it is NOT the base pack's own.
// Returns true when a borrowed component drew, so the base pack can skip its own.
function ModernHUD::drawSlot(%slot, %screen)
{
   %sel = ModernHUD::slotSelection(%slot);
   if(%sel == "")
      return false;

   %fn = $ModernHUD::Comp[%sel, fn];
   if(%fn == "")
   {
      // ★Missing components degrade PREDICTABLY.★ A preset can reference a
      // component that is no longer installed or never passed its gate. Log the
      // stable id once and fall back to the base pack rather than drawing
      // nothing and calling it a feature.
      if($ModernHUD::Warned[%sel] == "")
      {
         $ModernHUD::Warned[%sel] = 1;
         echo("[MODERNHUD] slot '" @ %slot @ "' wants '" @ %sel @
              "' which is not registered; using the base pack");
      }
      return false;
   }

   // The provider must be active for its component to draw.
   %prov = $ModernHUD::Comp[%sel, provider];
   if($ModernHUD::Provider[%prov, refs] == "")
      return false;

   *%fn(%screen);
   return true;
}

// True when the BASE pack should draw this slot itself: nothing borrowed, or the
// borrowed component could not be resolved. Packs call this instead of ownsSlot
// once they are compositor-aware; ownsSlot remains for generated packs.
function ModernHUD::baseOwns(%slot)
{
   %sel = ModernHUD::slotSelection(%slot);
   if(%sel == "")
      return true;
   return $ModernHUD::Comp[%sel, fn] == "";
}

// Draw every borrowed component, in a DETERMINISTIC order.
//
// Registration order is not deterministic across packs, and two components from
// different providers can overlap; the slot table below is the draw order, so the
// result does not depend on which pack happened to register first.
function ModernHUD::drawBorrowed(%screen)
{
   if($ModernHUD::CompCount == "" || $ModernHUD::CompCount == 0)
      return;
   // ★getWord returns "-1" PAST THE END, not "".★ (FearPlugin.cpp c_getWord: an
   // out-of-range index returns the literal string "-1".) A
   // `while(getWord(..) != "")` therefore never terminates -- and this runs every
   // frame, so the client hangs on the first frame after any pack that registers
   // components loads. Measured: selecting Overstep froze the client, log ending
   // mid-frame right after "loaded master pack 'overstep'".
   //
   // Bounded loop, and the terminator tests BOTH sentinels so a future getWord
   // that returns "" is equally safe.
   %order = "minimap chat ctf clock healthenergy weapon items fps toasty killfeed flagpopup enemy hit lowhealth ratings repkit scoreboard";
   for(%i = 0; %i < 17; %i++)
   {
      %slot = getWord(%order, %i);
      if(%slot == "" || %slot == "-1")
         break;
      ModernHUD::drawSlot(%slot, %screen);
   }
}

// Drop the whole registry. Called from ModernHUD::unload with the settings
// registry, so a swapped-in pack cannot inherit the last pack's components.
function ModernHUD::clearComponents()
{
   %n = $ModernHUD::CompCount;
   if(%n == "") %n = 0;
   for(%i = 0; %i < %n; %i++)
   {
      %key = $ModernHUD::CompKey[%i];
      %prov = $ModernHUD::Comp[%key, provider];
      $ModernHUD::Comp[%key, fn] = "";
      $ModernHUD::Comp[%key, slot] = "";
      $ModernHUD::Comp[%key, provider] = "";
      $ModernHUD::Provider[%prov, refs] = "";
      $ModernHUD::CompKey[%i] = "";
   }
   $ModernHUD::CompCount = 0;
}

function ModernHUD::onDraw(%screen)
{
   if(!$ModernHUD::Enabled)
      return;

   ModernHUDPack::draw(%screen);

   // Components borrowed from other providers draw AFTER the base pack, so a
   // borrowed part is never covered by the base pack's own art for that slot.
   // The base pack yields the slot itself (ownsSlot / baseOwns).
   ModernHUD::drawBorrowed(%screen);

   // The settings panel draws LAST, over the pack's own art -- it is modal
   // furniture and must not be occluded by the HUD it configures. No-ops when the
   // K panel is closed or the pack registered no settings.
   ModernHUD::menu(%screen);
}

// Seed the menu palette at framework load, so the globals exist before any pack
// runs. A pack that overrides them does so in its own load; unload restores these.
ModernHUD::menuPalette();
