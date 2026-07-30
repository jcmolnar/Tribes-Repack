// ModernHUD Phase A framework
//
// Immediate-mode by design: no SimGui controls, authored extents, fracPos or
// persistent clip boxes. A pack implements ModernHUDPack::draw(%screenSize).
// The native ScriptGL hook calls ModernHUD::onDraw once per playGui frame.

// 2 = the generated-pack runtime (ModernHUD::part/handle/markup/digitsBox/
// imageRect/detachContainer) required by pack format v1's generated hud.cs.
$ModernHUD::FrameworkVersion = 2;
$ModernHUD::ResetGeneration = 0;

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
      $ModernHUD::HandleName[%i] = "";
   }
   $ModernHUD::HandleCount = 0;
   $ModernHUD::Enabled = false;

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

   // Seed the default only when the pref has never been set. A string test, so
   // it cannot be fooled by the numeric promotion described above.
   if(ModernHUD::settingGet(%key) == "")
      ModernHUD::settingSet(%key, %default);
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
   glDrawImage(getWord(%at, 0), getWord(%at, 1), %w, %h, %path, %alpha, %style);
   return %w @ " " @ %h;
}

function ModernHUD::imageAt(%x, %y, %path, %alpha, %style)
{
   %size = glGetImageDimensions(%path);
   %w = getWord(%size, 0);
   %h = getWord(%size, 1);
   if(%w > 0 && %h > 0)
      glDrawImage(%x, %y, %w, %h, %path, %alpha, %style);
   return %w @ " " @ %h;
}

function ModernHUD::digitsWidth(%folder, %value, %spacing)
{
   %width = 0;
   %count = String::len(%value);
   for(%i = 0; %i < %count; %i++)
   {
      %digit = String::GetSubStr(%value, %i, 1);
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
      %digit = String::GetSubStr(%value, %i, 1);
      %path = %folder @ "/" @ %digit @ ".png";
      %size = glGetImageDimensions(%path);
      %dw = getWord(%size, 0);
      %dh = getWord(%size, 1);
      if(%dw > 0 && %dh > 0)
      {
         glDrawImage(%x, %y, %dw, %dh, %path, %alpha);
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
   %saved  = $pref::hudPositions[%name];
   if(%saved == "")
      return;
   if(String::explode(%saved, "||", "fields") != 2)
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
      $ModernHUD::AppliedReset[%name] = $ModernHUD::ResetGeneration;

      // A saved position from a bigger canvas can be off-screen entirely; place it
      // where this screen can show it. Only on creation -- a position the player
      // dragged this session is never second-guessed.
      ModernHUD::fitOnScreen(%name, %handle, %defaultPos, %w, %h);
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

   %published = $ModernHUD::HandlePos[%name];
   if(%published != "")
      return %published;
   return Control::GetPosition(%handle);
}

function ModernHUD::noop()
{
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
   %scale = $pref::hudScale[%name];
   if(%scale == "")
      %scale = 1;
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
      Hud::setSessionPos(%handle, %x, %y);

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
      glDrawImage(%x, %y, %w, %h, %path, %alpha, %tint);
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
   glDrawImagePart(%x, %y, 0, 0, %w, %h, %path, %alpha);
   return %w;
}

// The retained packs' text vocabulary, immediate. glDrawMarkup runs the engine's
// own SimGui::TextFormat, so all five xFont tags render exactly as they do in a
// FearGuiFormattedText -- see re/modernhud_pack_format_v1.md section 4.
function ModernHUD::markup(%x, %y, %width, %value, %alpha)
{
   return glDrawMarkup(%x, %y, %width, %value, %alpha);
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
// SetVisible(false) BEFORE removeFromSet, always: while the control still has a
// root, hiding it contributes its old rectangle to the canvas damage list.
// Removing first clears root, so those pixels can never be invalidated and the
// old HUD stays on screen as a frozen second copy.
function ModernHUD::detachContainer(%name)
{
   if(isObject(%name))
   {
      Control::SetVisible(%name, false);
      removeFromSet(playGui, %name);
   }
}

function ModernHUD::onDraw(%screen)
{
   if($ModernHUD::Enabled)
      ModernHUDPack::draw(%screen);
}
