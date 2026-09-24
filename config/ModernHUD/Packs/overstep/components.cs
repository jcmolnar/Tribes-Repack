//==============================================================================
// Tribes Overstep -- BORROWABLE PARTS (components.cs)
//
// Split out of hud.cs by tools/modernhud_components.py so another pack can draw these
// parts: the HUD designer's Provider choice sets $pref::HudSlot::<slot> to
// "overstep/<slot>", and ModernHUD::drawSlot execs this file on demand. It must define only
// Overstep:: names -- ModernHUDPack:: belongs to whichever pack is the base.
// hud.cs execs this file too, so the pack's own draw uses the same code.
//==============================================================================

ModernHUD::require("ModernHUD/Core/Data/Team.cs");
ModernHUD::require("ModernHUD/Core/Data/Timer.cs");


function Overstep::handle(%name, %defaultPos, %w, %h)
{
   // ★The Options HUD designer preview never creates, moves or resets a handle.★ This
   // pack keeps its own copy of handle() (it predates the framework's), so it must
   // defer to the framework's side-effect-free preview placement itself: the saved
   // or live spot, clamped onto the screen and mapped to the preview's size.
   if($ModernHUD::Preview)
   {
      %at = ModernHUD::handle(%name, %defaultPos, %w, %h);
      %s = ModernHUD::partStyle(%name, %at);
      ModernHUD::pvRecord(%name, %at, %w * %s, %h * %s, "");   // selectable in the designer
      return %at;
   }

   // Keep the current resolution's authored placement available to the
   // options reset command even while playGui is covered by the menu.
   $ModernHUD::DefaultPos[%name] = %defaultPos;

   // Object names containing "::" do not round-trip through isObject(name)
   // in this console implementation. Testing the name recreated every handle
   // on every draw, leaving thousands of same-named controls; K moved the
   // newest while Control::GetPosition resolved an older one. Retain and use
   // the numeric SimObject ID as the sole identity.
   %handle = $ModernHUD::Handle[%name];
   if(!isObject(%handle))
   {
      %x = getWord(%defaultPos, 0);
      %y = getWord(%defaultPos, 1);

      // Register a responsive immediate-mode hit target in the pack's normal
      // Hud::* inventory so its existing Store/Restore/exit hooks persist it.
      %handle = newObject(%name, FearGui::ModernHudHandle, %x, %y, %w, %h);
      $ModernHUD::Handle[%name] = %handle;
      if(!$ModernHUD::HandleRegistered[%name])
      {
         $ModernHUD::HandleName[$ModernHUD::HandleCount] = %name;
         $ModernHUD::HandleCount++;
         $ModernHUD::HandleRegistered[%name] = true;
      }
      $Hud::Huds[$Hud::Count] = %handle;
      $Hud::Huds[$Hud::Count, name] = %name;
      $Hud::Huds[$Hud::Count, wake] = "ModernHUDPack::noop";
      $Hud::Huds[$Hud::Count, sleep] = "ModernHUDPack::noop";
      $Hud::Huds[%name] = %handle;
      $Hud::Count++;

      %index = $Hud::Count - 1;
      // ★Hud::Restore is LEGACY-ONLY.★ It is defined in
      // CustomConfigs/<pack>/config/Core/Hud.cs, which ran only because Overstep
      // used to boot as a hybrid AFTER the legacy loader. Phase 3 stops the legacy
      // tree from running, so this logged "Hud::Restore: Unknown command." once per
      // handle and no saved position was ever applied. The framework's own copy has
      // identical semantics and is pack-qualified (Phase 1).
      ModernHUD::restorePos(%index);
      addToSet(playGui, %handle);

      // E2 addendum: this pack keeps its own copy of handle() (it predates the
      // framework's), so it must also register its handle ids as editor
      // targets -- the framework chokepoint never sees them.
      HudEditor::addTarget(%handle);

      $ModernHUD::AppliedReset[%name] = $ModernHUD::ResetGeneration;

      // ★This pack is authored against a 2560x1440 canvas, so a restored ABSOLUTE
      // position can land entirely outside a smaller window -- and the handle is
      // created here, on the first draw, AFTER every anchor pass the swap ran.
      // Reported live: most of Overstep's HUD did not appear until the window was
      // resized, then popped in (the resize re-projects anything that does not fit,
      // HudCtrl::parentResized). Place it where this screen can show it instead.
      ModernHUD::fitOnScreen(%name, %handle, %defaultPos, %w, %h);
   }

   Control::SetVisible(%handle, true);
   Control::SetExtent(%handle, %w, %h);

   // Reset uses the authored position calculated by this exact draw call, so
   // responsive defaults remain correct at every resolution. Hud::setSessionPos
   // updates both the pixel and the retained handle's resize state; assigning
   // position alone leaves a stale fracPos and the next resize moves it back.
   if($ModernHUD::AppliedReset[%name] != $ModernHUD::ResetGeneration)
   {
      Hud::setSessionPos(%handle, getWord(%defaultPos, 0), getWord(%defaultPos, 1));
      $ModernHUD::AppliedReset[%name] = $ModernHUD::ResetGeneration;
   }

   %published = $ModernHUD::HandlePos[%name];
   %controlPos = Control::GetPosition(%handle);
   if($ModernHUD::Debug &&
      ($ModernHUD::LastConsumed[%name] != %published ||
       $ModernHUD::LastControlPos[%name] != %controlPos))
   {
      echo("[MH-CONSUME] name=" @ %name @
           " control=" @ %controlPos @
           " published=" @ %published @
           " extent=" @ Control::GetExtent(%handle));
      $ModernHUD::LastConsumed[%name] = %published;
      $ModernHUD::LastControlPos[%name] = %controlPos;
   }
   if(%published == "")
      %published = %controlPos;

   // This pack keeps its own copy of handle() (it predates the framework's), so it
   // has to publish its part boxes too -- otherwise Overstep is the one pack the
   // parity harness can see no rectangles for, and every one of its parts counts
   // as "drawn outside any declared part".
   ModernHUD::recordRect(%name, %published, %w, %h);
   // The part's size, visibility and opacity (the designer's rows, the K resize), as
   // ModernHUD::part applies them. A caller that fetches several handles before drawing
   // (drawStatus) re-issues partStyle ahead of each part's own draws.
   ModernHUD::partStyle(%name, %published);
   return %published;
}

function Overstep::minimapAlpha()
{
   // miniMapAlpha is the original 1.40 config contract and is already driven
   // by every pack's HUD-options slider. New packs may use the 0..255 alias.
   if($pref::miniMapAlpha != "")
      %alpha = floor($pref::miniMapAlpha * 255);
   else if($pref::miniMapOpacity != "")
   {
      %alpha = $pref::miniMapOpacity;
      if(%alpha <= 1)
         %alpha = floor(%alpha * 255);
   }
   else
      %alpha = 255;

   if(%alpha < 0)
      %alpha = 0;
   if(%alpha > 255)
      %alpha = 255;
   return %alpha;
}

function Overstep::drawMinimapFrame(%screen)
{
   if(!isObject(Minimap))
      return;

   %pos = Control::GetPosition("Minimap");
   %extent = Control::GetExtent("Minimap");
   %side = getWord(%extent, 0) - 10;
   if(%side < 22)
      return;

   %alpha = Overstep::minimapAlpha();

   // The frame follows the native Minimap control, not a part of this pack: drop the
   // previous part's scale and style (Toasty draws just before this), or hiding or
   // fading that part in the HUD designer would take the minimap frame with it.
   glPartScale(0, 0, 1);

   // Native Minimap: authored at 35,36 with a 318px extent. Overstep R1:
   // authored at 40,41 and 308px. Deriving the frame from the live native
   // control preserves that exact 5px inset and follows K-dragging/resizing.
   glDrawImage(getWord(%pos, 0) + 5, getWord(%pos, 1) + 5,
               %side, %side, "Modules/minimap/R1.png", %alpha);
}

function Overstep::drawStatus(%screen)
{
   // Three content-sized status plates, anchored as one group. This preserves
   // the intended Overstep bottom-centre relationship at every resolution.
   %plateW = 163;
   %plateH = 73;
   %gap = -1;
   %groupW = (%plateW * 3) + (%gap * 2);
   %at = ModernHUD::place("bottom-center", 0, 110, %groupW, %plateH, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);

   %healthAt = Overstep::handle("ModernHUD::Health", %x @ " " @ %y,
                                     %plateW, %plateH);
   %speedAt = Overstep::handle("ModernHUD::Speed",
                                    (%x + %plateW + %gap) @ " " @ %y,
                                    %plateW, %plateH);
   %energyAt = Overstep::handle("ModernHUD::Energy",
                                     (%x + ((%plateW + %gap) * 2)) @ " " @ %y,
                                     %plateW, %plateH);
   %hx = getWord(%healthAt, 0);
   %hy = getWord(%healthAt, 1);
   %sx = getWord(%speedAt, 0);
   %sy = getWord(%speedAt, 1);
   %ex = getWord(%energyAt, 0);
   %ey = getWord(%energyAt, 1);

   %digits = "Modules/numHUD/White";
   %healthW = ModernHUD::digitsWidth(%digits, $health, 0);
   %speedW = ModernHUD::digitsWidth(%digits, $Speed, 0);
   %energyW = ModernHUD::digitsWidth(%digits, $energy, 0);

   // One plate at a time, each under its own part style (size / opacity / hide from the
   // HUD designer): the three handles were fetched above, which left the LAST one's
   // style active, so each block re-issues its own first.
   ModernHUD::partStyle("ModernHUD::Health", %healthAt);
   glDrawImage(%hx, %hy, %plateW, %plateH, "Modules/HeEnHUD/Hring.png", 255);
   ModernHUD::digitsAt(%hx + 72 + floor((82 - %healthW) / 2), %hy + 24,
                       %digits, $health, 255, 0);

   // ★The repair-kit "+" belongs ON the health plate, and must be drawn AFTER it.★
   // It was left as the legacy RETAINED control (aaRepKitHUD::Container, authored at
   // 1057,1254 on this pack's 2560x1440 canvas): retained controls render in the GUI
   // pass, which finishes before the immediate ModernHUD pass, so the health plate
   // was painted over it -- reported as "it goes behind it if I move it to it".
   // Drawing it here makes it part of the health component: on top, and it moves
   // with the plate instead of being a second thing to place.
   //
   // blankdot when empty is the legacy module's own behaviour (aarepkithud.acs.cs),
   // kept so the slot does not blink in and out as kits are used.
   if(getItemCount("Repair Kit") > 0)
      %kitArt = "Modules/aaRepKitHUD/rkit.png";
   else
      %kitArt = "Modules/aaRepKitHUD/blankdot.png";
   ModernHUD::imageAt(%hx + 12, %hy + 16, %kitArt, 255);

   ModernHUD::partStyle("ModernHUD::Speed", %speedAt);
   glDrawImage(%sx, %sy, %plateW, %plateH, "Modules/HeEnHUD/Sring.png", 255);
   ModernHUD::digitsAt(%sx + 72 + floor((82 - %speedW) / 2), %sy + 24,
                       %digits, $Speed, 255, 0);

   ModernHUD::partStyle("ModernHUD::Energy", %energyAt);
   glDrawImage(%ex, %ey, %plateW, %plateH, "Modules/HeEnHUD/Ering.png", 255);
   ModernHUD::digitsAt(%ex + 72 + floor((82 - %energyW) / 2), %ey + 24,
                       %digits, $energy, 255, 0);
}

function Overstep::drawWeapons(%screen)
{
   %screenH = getWord(%screen, 1);
   %slotCount = 0;

   // Count owned weapons first so the complete stack can remain bottom-anchored
   // as loadouts grow and shrink.
   for(%i = 0; %i < $Weapon::Count; %i++)
      if(getItemCount($Weapon::Name[%i]) > 0)
         %slotCount++;

   %height = %slotCount * 56;
   if(%height < 56)
      %height = 56;
   %defaultY = %screenH - 112 - %height;
   if(%defaultY < 12)
      %defaultY = 12;
   %at = Overstep::handle("ModernHUD::Weapons",
                               "62 " @ %defaultY, 164, %height);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);

   for(%i = 0; %i < $Weapon::Count; %i++)
   {
      %weapon = $Weapon::Name[%i];
      if(getItemCount(%weapon) <= 0)
         continue;

      %file = $Weapon::File[%i];
      %path = "Modules/WeaponsHud/" @ %file @ ".png";
      if(getItemType(%weapon) == getMountedItem(0))
      {
         %activePath = "Modules/WeaponsHud/" @ %file @ "on.png";
         %activeSize = glGetImageDimensions(%activePath);
         if(getWord(%activeSize, 0) > 0)
            %path = %activePath;
      }

      %size = glGetImageDimensions(%path);
      %w = getWord(%size, 0);
      %h = getWord(%size, 1);
      // Source resolution is intentionally independent of layout resolution.
      // The replacement plates are authored at 4x (656x224) so they remain
      // crisp when a pack or accessibility setting chooses a larger HUD scale.
      if(%w > 0 && %h > 0)
         glDrawImage(%x, %y, 164, 56, %path, 255);

      %ammo = $Weapon::Ammo[%i];
      if(%ammo != "")
         %ammoValue = getItemCount(%ammo);
      else
         // Blaster, Laser Rifle and ELF consume suit energy rather than an
         // inventory ammo datablock. The retained WH table leaves their ammo
         // name empty; display the effective available energy like the
         // original Overstep visual reference does.
         %ammoValue = $energy;
      ModernHUD::digitsAt(%x + 12, %y + 19, "Modules/numHUD/Ammo",
                          %ammoValue, 255, 0);
      %y += 56;
   }
}

function Overstep::drawItems(%screen)
{
   %screenH = getWord(%screen, 1);
   %grenades = getItemCount("Grenade");
   %beacons = getItemCount("Beacon");
   %at = Overstep::handle("ModernHUD::Items",
                               "20 " @ (%screenH - 72), 240, 66);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);

   %grenPath = (%grenades > 0) ?
      "Modules/ItemHUD/gren.png" : "Modules/ItemHUD/gren0.png";
   %beaconPath = (%beacons > 0) ?
      "Modules/ItemHUD/beacon.png" : "Modules/ItemHUD/beacon0.png";

   glDrawImage(%x, %y, 50, 66, %grenPath, 255);
   ModernHUD::digitsAt(%x + 62, %y + 25, "Modules/numHUD/Clock",
                       %grenades, 255, 0);
   glDrawImage(%x + 128, %y + 11, 50, 44, %beaconPath, 255);
   ModernHUD::digitsAt(%x + 190, %y + 25, "Modules/numHUD/Clock",
                       %beacons, 255, 0);
}

function Overstep::twoDigits(%value)
{
   if(%value < 10)
      return "0" ~ %value;
   return %value;
}

function Overstep::drawCtfClock(%screen)
{
   %at = ModernHUD::place("top-center", 0, 35, 300, 100, %screen);
   %at = Overstep::handle("ModernHUD::CtfClock", %at, 306, 100);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);

   glDrawImage(%x, %y, 300, 70, "Modules/numHUD/CTFHud/bg.png", 255);

   %friendly = Team::Friendly();
   %enemy = Team::Enemy();
   %score0 = Team::Score(%friendly);
   %score1 = Team::Score(%enemy);
   if(%score0 == "")
      %score0 = 0;
   if(%score1 == "")
      %score1 = 0;

   %scoreFolder = "Modules/numHUD/Black";
   %score0W = ModernHUD::digitsWidth(%scoreFolder, %score0, 0);
   %score1W = ModernHUD::digitsWidth(%scoreFolder, %score1, 0);
   ModernHUD::digitsAt(%x + 39 - floor(%score0W / 2), %y + 15,
                       %scoreFolder, %score0, 255, 0);
   ModernHUD::digitsAt(%x + 263 - floor(%score1W / 2), %y + 15,
                       %scoreFolder, %score1, 255, 0);

   %clockFolder = "Modules/numHUD/Clock";
   // ClockHud and the native client both advance cg.clockTime every frame.
   // Read it directly so a live pack swap starts at the real remaining time;
   // the legacy eventUpdateTime bridge only refreshes at :00/:20/:40.
   %clockTime = getHudTimer();
   if (%clockTime < 0)
      %clockTime = -%clockTime;
   %clockMin = floor(%clockTime / 60);
   %clockSec = floor(%clockTime - (%clockMin * 60));
   %minutes = Overstep::twoDigits(%clockMin);
   %seconds = Overstep::twoDigits(%clockSec);
   %minutesW = ModernHUD::digitsWidth(%clockFolder, %minutes, 0);
   %secondsW = ModernHUD::digitsWidth(%clockFolder, %seconds, 0);
   %clockW = %minutesW + 7 + %secondsW;
   %clockX = %x + floor((300 - %clockW) / 2);
   ModernHUD::digitsAt(%clockX, %y + 23, %clockFolder, %minutes, 255, 0);
   glDrawImage(%clockX + %minutesW, %y + 23, 7, 25,
               "Modules/numHUD/Clock/colon.png", 255);
   ModernHUD::digitsAt(%clockX + %minutesW + 7, %y + 23,
                       %clockFolder, %seconds, 255, 0);

   // The supplied home art is absent in this pack. Its field/player plates are
   // visually identical colored state strips, so use those shipped images for
   // all locations and add the return timer when the flag is on the field.
   %friendlyLoc = Team::Flag::Location(%friendly);
   %enemyLoc = Team::Flag::Location(%enemy);
   %friendlyState = (%friendlyLoc == "field") ? "empty" : "player";
   %enemyState = (%enemyLoc == "field") ? "empty" : "player";
   glDrawImage(%x - 4, %y + 70, 154, 29,
               "Modules/numHUD/CTFHud/friendly." ~ %friendlyState ~ ".png", 255);
   glDrawImage(%x + 150, %y + 70, 156, 30,
               "Modules/numHUD/CTFHud/enemy." ~ %enemyState ~ ".png", 255);

   if(%friendlyLoc == "field")
      Overstep::drawFlagTimer(%x + 73, %y + 76, %friendly);
   if(%enemyLoc == "field")
      Overstep::drawFlagTimer(%x + 228, %y + 76, %enemy);
}

// The flag timer is decimal text ("47.5" -- Timer::FormatSeconds always emits one
// decimal place) and the Ammo digit font has no decimal-point image, so feeding it
// through digitsAt asked for 'Modules/numHUD/Ammo/..png' on every dropped flag.
// Draw the whole and fractional digits separately with a 2x2 dot between, centred
// on %centerX. (TribesMod handoff, 2026-09-06.)
function Overstep::drawFlagTimer(%centerX, %y, %team)
{
   %timer = Team::Flag::Timer(%team);
   %folder = "Modules/numHUD/Ammo";
   %dot = String::findSubStr(%timer, ".");
   if(%dot < 0)
   {
      %timerW = ModernHUD::digitsWidth(%folder, %timer, 0);
      ModernHUD::digitsAt(%centerX - floor(%timerW / 2), %y, %folder, %timer, 255, 0);
      return;
   }
   %whole = String::getSubStr(%timer, 0, %dot);
   %fraction = String::getSubStr(%timer, %dot + 1, String::len(%timer));
   %wholeW = ModernHUD::digitsWidth(%folder, %whole, 0);
   %fractionW = ModernHUD::digitsWidth(%folder, %fraction, 0);
   %x = %centerX - floor((%wholeW + 5 + %fractionW) / 2);
   ModernHUD::digitsAt(%x, %y, %folder, %whole, 255, 0);
   glColor4ub(255, 255, 255, 255);
   glRectangle(%x + %wholeW + 1, %y + 15, 2, 2);
   ModernHUD::digitsAt(%x + %wholeW + 5, %y, %folder, %fraction, 255, 0);
}

function Overstep::drawScore(%screen)
{
   %name = Client::GetName(getManagerId());
   %score = $Collector::Score[%name];
   if(%score == "")
      %score = 0;

   %negative = (%score < 0);
   %value = %negative ? -%score : %score;
   %folder = "Modules/numHUD/score";
   %digitsW = ModernHUD::digitsWidth(%folder, %value, 0);
   %signW = 0;
   if(%negative)
      %signW = getWord(glGetImageDimensions(%folder @ "/NSign.png"), 0);
   %numberW = %digitsW + %signW;
   %w = (%numberW > 91) ? %numberW : 91;
   %h = 76;

   // Preserve the pack's authored score location on first conversion, then
   // let the responsive ModernHUD handle own it.
   if(isObject("RatingsHUD::Container"))
      %defaultAt = Control::GetPosition("RatingsHUD::Container");
   else
      %defaultAt = ModernHUD::place("bottom-right", 70, 45, %w, %h, %screen);
   %at = Overstep::handle("ModernHUD::Score", %defaultAt, %w, %h);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);

   glDrawImage(%x + floor((%w - 91) / 2), %y, 91, 18,
               "Modules/RatingsHUD/borderbar.png", 255);
   %digitX = %x + floor((%w - %numberW) / 2);
   if(%negative)
   {
      glDrawImage(%digitX, %y + 25, %signW, 50,
                  %folder @ "/NSign.png", 255);
      %digitX += %signW;
   }
   ModernHUD::digitsAt(%digitX, %y + 25, %folder, %value, 255, 0);
}

//----------------------------------------------------------------------------
// PHASE 4: offer these parts as BORROWABLE components.
//
// Each entry is <provider>/<component> answering one native slot, with a draw
// function that is already namespaced to this pack. Registering does not change
// how Overstep draws when it is the base pack -- ModernHUDPack::draw still owns
// that -- it makes these parts addressable when ANOTHER pack is the base, which
// is what Joe's saved cross-pack preset needs (four of its six slots are
// Overstep's).
//----------------------------------------------------------------------------

//----------------------------------------------------------------------------
// PHASE 3b -- the three legacy Overstep HUDs that had no converted replacement.
//
// ★Each drops a persistent polling schedule.★ The legacy modules kept themselves
// up to date with Schedule::Add loops that ran forever whether or not anything had
// changed -- aaRepKitHUD polled once a second AND attached three event handlers;
// LHHud rescheduled itself every 0.1s for the whole session. An immediate-mode part
// is already called once per frame with the live game state, so the poll, the event
// hooks and the cached-value comparison all become dead weight: the value is simply
// read where it is drawn. Functionally identical, strictly less work, and MORE
// responsive (frame-accurate instead of up to 1s stale).
//----------------------------------------------------------------------------


// aaRepKitHUD: repair-kit carried / not carried.
//
// Legacy: a 1Hz Schedule::Add loop, three Event::Attach handlers (received/dropped/
// used), and a $aaRepKitHUD::Kits cache so the 1Hz loop could skip redundant
// Control::SetValue calls. All of that existed to answer one question at some point
// after it changed. Here the question is answered at draw time, so the answer is
// never stale and none of the machinery is needed.
function Overstep::drawRepKit(%screen)
{
   %at = Overstep::handle("ModernHUD::RepKit", "200 200", 130, 40);
   %icon = (getItemCount("Repair Kit") > 0) ? "rkit.png" : "blankdot.png";
   glDrawImage(getWord(%at, 0), getWord(%at, 1), 130, 40,
               "Modules/aaRepKitHUD/" @ %icon, 255);
}

// LHHud: the low-health warning -- a bar that shrinks as health drops, pulsing
// through low1..low6 below 50 health.
//
// ★The pulse is derived from the clock, not driven by a timer.★ Legacy advanced a
// $low counter up 1..6 then back down, rescheduling itself every 0.1s forever --
// a permanent timer whose only job was to make a number oscillate. getSimTime()
// already oscillates; the frame index is a function of it. Same six frames, same
// ~0.1s cadence, same ping-pong order, no schedule.
//
// Legacy also drove the bar with Control::SetExtent(72, 125-$health/100*142), which
// is the extent of the ART, so it is reproduced as the draw height rather than as a
// control resize.
function Overstep::drawLowHealth(%screen)
{
   %at = Overstep::handle("ModernHUD::LowHealth", "0 0", 72, 72);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);

   %h = 125 - $health / 100 * 142;
   if(%h <= 0)
      return;                       // full health: legacy drew a zero-extent control

   if($health >= 50)
   {
      glDrawImage(%x, %y, 72, %h, "Modules/LowHealth/h1.png", 255);
      return;
    }

   // 0..5..0 ping-pong over 10 steps of 0.1s, i.e. the legacy $low 1..6..1 walk.
   %step = getSimTime() / 100;
   %phase = %step - (%step / 10) * 10;          // console has no modulo operator
   if(%phase > 5)
      %phase = 10 - %phase;
   glDrawImage(%x, %y, 72, %h,
               "Modules/LowHealth/low" @ (%phase + 1) @ ".png", 255);
}

function Overstep::flagPopupShow(%team, %cl)
{
   if(%cl != getManagerId())
      return;
   $Overstep::FlagPopupAt = getSimTime();
}

function Overstep::flagPopupReset()
{
   $Overstep::FlagPopupAt = "";
}

function Overstep::flagPopupEnd(%team, %cl)
{
   if(%cl != getManagerId())
      return;
   if(%team != Client::GetTeam(getManagerId()))
      Overstep::flagPopupReset();
}

function Overstep::drawFlagPopup(%screen)
{
   %at = $Overstep::FlagPopupAt;
   if(%at == "")
      return;

   %elapsed = getSimTime() - %at;
   // A mission change restarts sim time, which would otherwise leave the banner
   // pinned on screen forever with a negative age.
   if(%elapsed < 0)
   {
      Overstep::flagPopupReset();
      return;
   }

   %alpha = 255;
   if(%elapsed > $Overstep::FlagPopupHold)
   {
      %fade = %elapsed - $Overstep::FlagPopupHold;
      if(%fade >= $Overstep::FlagPopupFade)
      {
         Overstep::flagPopupReset();
         return;
      }
      %alpha = 255 - (255 * %fade / $Overstep::FlagPopupFade);
   }

   // ★bottom/right offsets are INSETS★ (place() computes screenH - contentH - offsetY),
   // so a POSITIVE value lifts it off the edge. This was -60 and put the banner at
   // y=691 on a 698-tall canvas -- a 7px cyan sliver along the bottom, caught only by
   // triggering the event and looking.
   %default = ModernHUD::place("bottom-center", 0, 60, 1560, 67, %screen);
   %pos = Overstep::handle("ModernHUD::FlagPopup", %default, 1560, 67);
   glDrawImage(getWord(%pos, 0), getWord(%pos, 1), 1560, 67,
               "Modules/FlagPopup/flag.png", %alpha);
}

function Overstep::toastyMeters(%msg)
{
   // Legacy GetMidAirMeter walked words until one was numeric. getWord past the
   // end returns the literal "-1", which is what terminates the walk.
   for(%i = 0; String::Trim(getWord(%msg, %i)) != -1; %i++)
   {
      %w = getWord(%msg, %i);
      if(chr(%w) == "")
         return %w;
   }
   return -1;
}

function Overstep::toastyMessage(%msg)
{
   if($playingDemo)
      return;
   if(String::findSubStr(%msg, "mid-air") == -1)
      return;

   %meters = Overstep::toastyMeters(%msg);
   if(%meters < $Overstep::ToastyMinMeters)
      return;

   %pairs = String::Trim(String::Replace(String::Replace(%msg,
               sprintf(" lands [ %1 meter ] mid-air on ", %meters), ", "), "!", ""));
   // ★String::Explode, not String::explode -- see the deviation note above.★
   String::Explode(%pairs, ", ", "toastyName");
   %shooter = $toastyName[0];
   %victim  = $toastyName[1];

   if(%shooter != $PCFG::Name && %victim != $PCFG::Name)
      return;

   $Overstep::ToastyAt = getSimTime();
   $Overstep::ToastyShooter = %shooter;
   $Overstep::ToastyVictim = %victim;
   $Overstep::ToastyMeters = %meters;
   $ToastyHUD::PTotal++;

   Overstep::toastyCenterPrint();
   localSound("mk.toasty.ogg");
}

function Overstep::toastyCenterPrint()
{
   %praises = "spanked smacked pwned obliterated popped hit owned";
   String::Explode(%praises, " ", "toastyPraise");
   %rand = floor(getRandom() * 6);

   %shooter = ($Overstep::ToastyShooter == $PCFG::Name)
              ? "You" : $Overstep::ToastyShooter;
   %victim  = ($Overstep::ToastyVictim == $PCFG::Name)
              ? "you" : $Overstep::ToastyVictim;

   remoteBP(2048, sprintf("<JC><F1>TOOASTY! <F2>%1<F1> %2<F2> %3<F1> at<F2> %4<F1> meters!"
            @ "\n\nTotal Toasty's: %5",
            %shooter, $toastyPraise[%rand], %victim,
            $Overstep::ToastyMeters, $ToastyHUD::PTotal), 3);
}

function Overstep::drawToasty(%screen)
{
   %at = $Overstep::ToastyAt;
   if(%at == "")
      return;

   %elapsed = getSimTime() - %at;
   if(%elapsed < 0)
   {
      $Overstep::ToastyAt = "";
      return;
   }

   %w = $Overstep::ToastyWidth;
   %slide = $Overstep::ToastySlide;
   %total = %slide + $Overstep::ToastyHold + %slide;
   if(%elapsed >= %total)
   {
      $Overstep::ToastyAt = "";
      return;
   }

   // How far out of the right edge the art still is: full width before the slide
   // completes, zero while held, back out again on the way off.
   if(%elapsed < %slide)
      %hidden = %w - (%w * %elapsed / %slide);
   else if(%elapsed < %slide + $Overstep::ToastyHold)
      %hidden = 0;
   else
      %hidden = %w * (%elapsed - %slide - $Overstep::ToastyHold) / %slide;

   %default = ModernHUD::place("bottom-right", 0, 40, %w, %w, %screen);
   %pos = Overstep::handle("ModernHUD::Toasty", %default, %w, %w);
   glDrawImage(getWord(%pos, 0) + %hidden, getWord(%pos, 1), %w, %w,
               "Modules/ToastyHUD/toasty.png", 255);
}

ModernHUD::component("overstep", "status",   "healthenergy", "Overstep::drawStatus");
ModernHUD::component("overstep", "weapon",   "weapon",       "Overstep::drawWeapons");
ModernHUD::component("overstep", "items",    "items",        "Overstep::drawItems");
ModernHUD::component("overstep", "ctfclock", "ctf",          "Overstep::drawCtfClock");
ModernHUD::component("overstep", "minimap",  "minimap",      "Overstep::drawMinimapFrame");
ModernHUD::component("overstep", "score",    "ratings",      "Overstep::drawScore");
ModernHUD::component("overstep", "repkit",     "repkit",     "Overstep::drawRepKit");
ModernHUD::component("overstep", "lowhealth",  "lowhealth",  "Overstep::drawLowHealth");
ModernHUD::component("overstep", "flagpopup",  "flagpopup",  "Overstep::drawFlagPopup");
ModernHUD::component("overstep", "toasty",     "toasty",     "Overstep::drawToasty");

// ModernHUD::attach, not Event::Attach: the framework records these and drops them
// in detachAll() when the pack unloads. A raw Event::Attach would survive a config
// swap and keep firing this pack's handlers under the next pack.
// Canonical Presto TeamTrak names: the tracker emits eventFlagTaken/Dropped/
// Captured (args teamFlag, client); the Grab/Pickup/Drop/Cap aliases have no
// installed emitter, so the popup never fired.
ModernHUD::attach("eventFlagTaken",     "Overstep::flagPopupShow");
ModernHUD::attach("eventFlagDropped",   "Overstep::flagPopupEnd");
ModernHUD::attach("eventFlagCaptured",  "Overstep::flagPopupEnd");
ModernHUD::attach("eventChangeMission", "Overstep::flagPopupReset");
ModernHUD::attach("eventServerMessage", "Overstep::toastyMessage");
