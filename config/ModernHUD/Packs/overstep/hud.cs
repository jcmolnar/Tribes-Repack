// HAND-AUTHORED pack (authoring: manual) -- the generator does not own this file.
// Graduated from CustomConfigs\Tribes_Overstep\config\modern.hud.cs by Phase 3:
// same script, but living under Packs/overstep with its art imported to
// Assets/Packs/overstep so it no longer needs the legacy tree on the search path.
// Overstep Phase A image/animation gate.
//
// This is intentionally a small, measurable slice, not the final pack. It
// proves native PNG alpha, GIF animation, content-sized layout, per-draw fade
// and per-digit number art before the other HUD parts are converted.

exec("ModernHUD/Framework.cs");

$ModernHUD::Enabled = true;
$ModernHUD::Pack = "Tribes_Overstep";
// Phase 1 identity: every position/scale key is qualified with this.
$ModernHUD::PackId = "overstep";

function ModernHUDPack::noop()
{
}

// A legacy module borrowed from another pack owns the selected slot until the
// player changes it again.  ModernHUD is immediate-mode, so hiding/unloading
// retained controls alone cannot disable one of its parts: the draw dispatch
// must yield that slot explicitly.
function ModernHUDPack::ownsSlot(%value)
{
   if(%value == "")
      return true;
   if(%value == "off")
      return false;
   return String::findSubStr(%value, "Tribes_Overstep::") == 0;
}

function ModernHUDPack::hideHandle(%name)
{
   %handle = $ModernHUD::Handle[%name];
   if(isObject(%handle))
      Control::SetVisible(%handle, false);
}

function ModernHUDPack::detachRetained()
{
   %status = ModernHUDPack::ownsSlot($pref::HudSlot::healthenergy);
   %weapon = ModernHUDPack::ownsSlot($pref::HudSlot::weapon);
   %items = ModernHUDPack::ownsSlot($pref::HudSlot::items);
   %ctf = ModernHUDPack::ownsSlot($pref::HudSlot::ctf);
   %clock = ModernHUDPack::ownsSlot($pref::HudSlot::clock);
   %score = ModernHUDPack::ownsSlot($pref::HudSlot::ratings);

   if(%status)
   {
      Schedule::Cancel("GHealth::Update();");
      Schedule::Cancel("GSpeed::Update();");
      Schedule::Cancel("GEnergy::Update();");
   }
   if(%weapon)
      Schedule::Cancel("WH::Update();");
   if(%items)
      Schedule::Cancel("ItemHUD::Update();");
   // Hiding these in the immediate draw callback is too late: retained GUI
   // rendering and K-editor hit-testing have already happened. Detach the
   // converted containers from playGui so only their data-producing scripts
   // remain and there is exactly one movable identity per component.
   //
   // SetVisible(false) must happen BEFORE removeFromSet: it contributes the
   // control's old rectangle to the canvas damage list while it still has a
   // root. Removing first clears root, so the old retained pixels can never
   // be invalidated and appear as a frozen second HUD.
   if(%status)
   {
      Control::SetVisible("GHealth::Container", false);
      Control::SetVisible("GSpeed::Container", false);
      Control::SetVisible("GEnergy::Container", false);
      removeFromSet(playGui, "GHealth::Container");
      removeFromSet(playGui, "GSpeed::Container");
      removeFromSet(playGui, "GEnergy::Container");
      // The repair-kit "+" is drawn by drawStatus now (on top of the health plate,
      // where it belongs); its retained control has to go with the rest of the
      // status group or both would be on screen at once.
      Schedule::Cancel("aaRepKitHUD::Update();");
      Control::SetVisible("aaRepKitHUD::Container", false);
      removeFromSet(playGui, "aaRepKitHUD::Container");
   }
   if(%weapon)
   {
      Control::SetVisible("WeaponHUD::Container", false);
      removeFromSet(playGui, "WeaponHUD::Container");
   }
   if(%items)
   {
      Control::SetVisible("ItemHUD::Container", false);
      removeFromSet(playGui, "ItemHUD::Container");
   }
   if(%ctf)
   {
      Control::SetVisible("CtfHUD::Container", false);
      removeFromSet(playGui, "CtfHUD::Container");
   }
   if(%clock)
   {
      Control::SetVisible("clock::Container", false);
      removeFromSet(playGui, "clock::Container");
   }
   if(%score)
   {
      Control::SetVisible("RatingsHUD::Container", false);
      removeFromSet(playGui, "RatingsHUD::Container");
   }
   Control::SetVisible("HUDOverlay::Container", false);
   removeFromSet(playGui, "HUDOverlay::Container");
}

// The STOCK huds this pack turns on and off, from its own play.gui.cs.
//
// ★A pack must state its whole stock-HUD set, not just the ones it wants.★ Our
// client never executed any pack's play.gui.cs -- it is parsed for text only -- so
// stock visibility was simply inherited from whichever pack loaded before. Once the
// GENERATED packs began applying theirs, this hand-authored one became the hole:
// selecting xLoader turned its stock huds on, and coming back to Overstep left them
// on, because nothing here ever turned them off. Reported live.
//
// Visibility only. Placement rides these controls' own $pref::hudPositions, which the
// engine re-anchors; their play.gui.cs geometry is absolute pixels against this
// pack's 2560x1440 canvas and would be wrong on any other screen.
function ModernHUDPack::stockHuds()
{
   Control::SetVisible(crosshairHud, true);
   ModernHUD::stock(clockHud, false);
   ModernHUD::stock(sensorHUD, false);
   ModernHUD::stock(compassHud, false);
   ModernHUD::stock(jetPackHud, false);
   ModernHUD::stock(healthHud, false);
   ModernHUD::stock(weaponHud, false);
   Control::SetVisible(ChatDisplayHUD, true);
   // Chat/minimap resize handoff: this pack drives chat visibility directly
   // (no settings row), so it never passed the stock() chokepoint and the chat
   // was NOT an editor target here. Explicit registration, visibility logic
   // unchanged.
   ModernHUD::editTarget(ChatDisplayHUD);
   ModernHUD::stock(Minimap, true);
   Control::SetVisible(reticleCompass, false);
}

// The pack's own stock-HUD PREFERENCES, carried from its ClientPrefs.cs.
//
// ★A pack must state its minimap the same way it states stock visibility.★ Every
// GENERATED pack emits a prefs() with its $pref::miniMap* values; this hand-authored
// one had none, so the minimap was simply whatever the PREVIOUS config left behind --
// the identical predecessor-dependent hole that stockHuds() above exists to close, and
// it is just as invisible when the previous pack happens to agree.
//
// Measured in the parity run: 14.86% of the frame differed OUTSIDE every declared part,
// almost all of it the minimap drawn at a different size. These are Overstep's own
// authored values (CustomConfigs\Tribes_Overstep\config\ClientPrefs.cs:87-92), so this
// makes explicit what the pack always intended rather than changing its look.
function ModernHUDPack::prefs()
{
   $pref::miniMapAlpha = "1";
   $pref::miniMapAutosize = "False";
   $pref::miniMapRotate = "True";
   $pref::miniMapSquare = "False";
   $pref::miniMapWidth = "302";
   $pref::miniMapZoom = "1.75";
}

// ★Bound to eventGuiOpen_PlayGui, NOT eventGuiOpen plus a gui-name test.★
// TWO independent firers raise eventGuiOpen with DIFFERENT spellings: the
// ENGINE, with the control's real name -- `playGui` (simGuiCanvas.cpp:907,
// kronosFireEvent1) -- and PRESTO, with a hardcoded bare word -- `PlayGui`
// (Presto/events.cs:681 via OpenAGui(PlayGui) at :737). A string VALUE
// comparison is case-SENSITIVE even though NAME lookup is not (compare() falls
// through to strcmp for two non-numeric strings, eval.cpp), so the old
// `%gui == "playGui"` test matched the engine's spelling and ignored Presto's.
// It WORKED -- verified live: oldSeen=playGui, oldHits=1 per transition -- but
// only because the two spellings happen to differ. Presto fires an
// argument-free eventGuiOpen_PlayGui from the same function (events.cs:746);
// binding that removes the string test altogether, at the same frequency
// (verified newHits=1). A robustness change, not a bug fix.
function ModernHUDPack::onPlayGuiOpen()
{
   Schedule::Add("ModernHUDPack::detachRetained();", 0);
   // Re-applied per gui open: the stock controls are recreated with the gui, so
   // setting them once at load does not survive the first transition.
   Schedule::Add("ModernHUDPack::stockHuds();", 0);
}

function ModernHUDPack::handle(%name, %defaultPos, %w, %h)
{
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
   return %published;
}

function ModernHUDPack::minimapAlpha()
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

function ModernHUDPack::drawMinimapFrame(%screen)
{
   if(!isObject(Minimap))
      return;

   %pos = Control::GetPosition("Minimap");
   %extent = Control::GetExtent("Minimap");
   %side = getWord(%extent, 0) - 10;
   if(%side < 22)
      return;

   %alpha = ModernHUDPack::minimapAlpha();

   // Native Minimap: authored at 35,36 with a 318px extent. Overstep R1:
   // authored at 40,41 and 308px. Deriving the frame from the live native
   // control preserves that exact 5px inset and follows K-dragging/resizing.
   glDrawImage(getWord(%pos, 0) + 5, getWord(%pos, 1) + 5,
               %side, %side, "Modules/minimap/R1.png", %alpha);
}

function ModernHUDPack::drawStatus(%screen)
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

   %healthAt = ModernHUDPack::handle("ModernHUD::Health", %x @ " " @ %y,
                                     %plateW, %plateH);
   %speedAt = ModernHUDPack::handle("ModernHUD::Speed",
                                    (%x + %plateW + %gap) @ " " @ %y,
                                    %plateW, %plateH);
   %energyAt = ModernHUDPack::handle("ModernHUD::Energy",
                                     (%x + ((%plateW + %gap) * 2)) @ " " @ %y,
                                     %plateW, %plateH);
   %hx = getWord(%healthAt, 0);
   %hy = getWord(%healthAt, 1);
   %sx = getWord(%speedAt, 0);
   %sy = getWord(%speedAt, 1);
   %ex = getWord(%energyAt, 0);
   %ey = getWord(%energyAt, 1);

   glDrawImage(%hx, %hy, %plateW, %plateH, "Modules/HeEnHUD/Hring.png", 255);
   glDrawImage(%sx, %sy, %plateW, %plateH,
               "Modules/HeEnHUD/Sring.png", 255);
   glDrawImage(%ex, %ey, %plateW, %plateH,
               "Modules/HeEnHUD/Ering.png", 255);

   %digits = "Modules/numHUD/White";
   %healthW = ModernHUD::digitsWidth(%digits, $health, 0);
   %speedW = ModernHUD::digitsWidth(%digits, $Speed, 0);
   %energyW = ModernHUD::digitsWidth(%digits, $energy, 0);

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
   ModernHUD::digitsAt(%sx + 72 + floor((82 - %speedW) / 2), %sy + 24,
                       %digits, $Speed, 255, 0);
   ModernHUD::digitsAt(%ex + 72 + floor((82 - %energyW) / 2), %ey + 24,
                       %digits, $energy, 255, 0);
}

function ModernHUDPack::drawWeapons(%screen)
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
   %at = ModernHUDPack::handle("ModernHUD::Weapons",
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

function ModernHUDPack::drawItems(%screen)
{
   %screenH = getWord(%screen, 1);
   %grenades = getItemCount("Grenade");
   %beacons = getItemCount("Beacon");
   %at = ModernHUDPack::handle("ModernHUD::Items",
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

function ModernHUDPack::twoDigits(%value)
{
   if(%value < 10)
      return "0" ~ %value;
   return %value;
}

function ModernHUDPack::drawCtfClock(%screen)
{
   %at = ModernHUD::place("top-center", 0, 35, 300, 100, %screen);
   %at = ModernHUDPack::handle("ModernHUD::CtfClock", %at, 306, 100);
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
   %minutes = ModernHUDPack::twoDigits(%clockMin);
   %seconds = ModernHUDPack::twoDigits(%clockSec);
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
   {
      %timer = Team::Flag::Timer(%friendly);
      %timerW = ModernHUD::digitsWidth("Modules/numHUD/Ammo", %timer, 0);
      ModernHUD::digitsAt(%x + 73 - floor(%timerW / 2), %y + 76,
                          "Modules/numHUD/Ammo", %timer, 255, 0);
   }
   if(%enemyLoc == "field")
   {
      %timer = Team::Flag::Timer(%enemy);
      %timerW = ModernHUD::digitsWidth("Modules/numHUD/Ammo", %timer, 0);
      ModernHUD::digitsAt(%x + 228 - floor(%timerW / 2), %y + 76,
                          "Modules/numHUD/Ammo", %timer, 255, 0);
   }
}

function ModernHUDPack::drawScore(%screen)
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
   %at = ModernHUDPack::handle("ModernHUD::Score", %defaultAt, %w, %h);
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

function ModernHUDPack::draw(%screen)
{
   ModernHUDPack::detachRetained();

   // Phase 3b parts. No ownsSlot gate: these three have no competing implementation
   // in any other pack, so there is nothing to yield to -- adding a gate would only
   // create a slot the picker would list with a single entry.
   ModernHUDPack::drawRepKit(%screen);
   ModernHUDPack::drawLowHealth(%screen);
   // Same reasoning: no other pack implements a flag banner or a toasty, so there
   // is no slot to yield. Both early-out unless their event has armed them, so the
   // cost when idle is one variable read each.
   ModernHUDPack::drawFlagPopup(%screen);
   ModernHUDPack::drawToasty(%screen);

   if(ModernHUDPack::ownsSlot($pref::HudSlot::minimap))
      ModernHUDPack::drawMinimapFrame(%screen);

   // CTF and clock share one authored Overstep plate. If either half is
   // replaced, yield the composite so the borrowed control is never covered.
   if(ModernHUDPack::ownsSlot($pref::HudSlot::ctf) &&
      ModernHUDPack::ownsSlot($pref::HudSlot::clock))
      ModernHUDPack::drawCtfClock(%screen);
   else
      ModernHUDPack::hideHandle("ModernHUD::CtfClock");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::healthenergy))
      ModernHUDPack::drawStatus(%screen);
   else
   {
      ModernHUDPack::hideHandle("ModernHUD::Health");
      ModernHUDPack::hideHandle("ModernHUD::Speed");
      ModernHUDPack::hideHandle("ModernHUD::Energy");
   }

   if(ModernHUDPack::ownsSlot($pref::HudSlot::weapon))
      ModernHUDPack::drawWeapons(%screen);
   else
      ModernHUDPack::hideHandle("ModernHUD::Weapons");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::items))
      ModernHUDPack::drawItems(%screen);
   else
      ModernHUDPack::hideHandle("ModernHUD::Items");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::ratings))
      ModernHUDPack::drawScore(%screen);
   else
      ModernHUDPack::hideHandle("ModernHUD::Score");
}

// ModernHUD::attach, not a raw Event::Attach: the framework revokes tracked
// handlers in detachAll() on unload, so this cannot outlive its own pack.
ModernHUD::attach("eventGuiOpen_PlayGui", "ModernHUDPack::onPlayGuiOpen");
ModernHUDPack::stockHuds();
ModernHUDPack::detachRetained();

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
function ModernHUDPack::drawRepKit(%screen)
{
   %at = ModernHUDPack::handle("ModernHUD::RepKit", "200 200", 130, 40);
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
function ModernHUDPack::drawLowHealth(%screen)
{
   %at = ModernHUDPack::handle("ModernHUD::LowHealth", "0 0", 72, 72);
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

//----------------------------------------------------------------------------
// FlagPopup -- the 1560x67 banner shown when YOU take a flag.
// Converted from CustomConfigs\Tribes_Overstep\config\Modules\FlagPopup\flagpopup.acs.cs
//
// ★The fade is arithmetic on the clock, not a timer.★ Legacy showed the banner,
// scheduled FadeOut at $popupTime, and then FadeOut rescheduled ITSELF every
// 0.006s stepping $fadeOutCount 100 -> 0 purely to walk %fade.alpha down. That is
// ~167 console evaluations a second whose only job is to make a number decrease.
// getSimTime() already increases; alpha is a pure function of it, so the entire
// loop collapses into three lines in the draw and leaves no schedule to leak.
//
// Faithful details kept: 5s hold at full opacity ($popupTime), ~0.6s fade
// (100 steps x 0.006s), only for the local player, and the legacy Drop/Cap reset
// condition -- which fires when the flag whose team is NOT mine leaves my hands.
//----------------------------------------------------------------------------
$ModernHUDPack::FlagPopupHold = 5000;    // legacy $popupTime = 5 seconds
$ModernHUDPack::FlagPopupFade = 600;     // legacy 100 steps x 0.006s

function ModernHUDPack::flagPopupShow(%team, %cl)
{
   if(%cl != getManagerId())
      return;
   $ModernHUDPack::FlagPopupAt = getSimTime();
}

function ModernHUDPack::flagPopupReset()
{
   $ModernHUDPack::FlagPopupAt = "";
}

function ModernHUDPack::flagPopupEnd(%team, %cl)
{
   if(%cl != getManagerId())
      return;
   if(%team != Client::GetTeam(getManagerId()))
      ModernHUDPack::flagPopupReset();
}

function ModernHUDPack::drawFlagPopup(%screen)
{
   %at = $ModernHUDPack::FlagPopupAt;
   if(%at == "")
      return;

   %elapsed = getSimTime() - %at;
   // A mission change restarts sim time, which would otherwise leave the banner
   // pinned on screen forever with a negative age.
   if(%elapsed < 0)
   {
      ModernHUDPack::flagPopupReset();
      return;
   }

   %alpha = 255;
   if(%elapsed > $ModernHUDPack::FlagPopupHold)
   {
      %fade = %elapsed - $ModernHUDPack::FlagPopupHold;
      if(%fade >= $ModernHUDPack::FlagPopupFade)
      {
         ModernHUDPack::flagPopupReset();
         return;
      }
      %alpha = 255 - (255 * %fade / $ModernHUDPack::FlagPopupFade);
   }

   // ★bottom/right offsets are INSETS★ (place() computes screenH - contentH - offsetY),
   // so a POSITIVE value lifts it off the edge. This was -60 and put the banner at
   // y=691 on a 698-tall canvas -- a 7px cyan sliver along the bottom, caught only by
   // triggering the event and looking.
   %default = ModernHUD::place("bottom-center", 0, 60, 1560, 67, %screen);
   %pos = ModernHUDPack::handle("ModernHUD::FlagPopup", %default, 1560, 67);
   glDrawImage(getWord(%pos, 0), getWord(%pos, 1), 1560, 67,
               "Modules/FlagPopup/flag.png", %alpha);
}

//----------------------------------------------------------------------------
// ToastyHUD -- Dan Forden slides in from the right on a 50m+ mid-air.
// Converted from CustomConfigs\Tribes_Overstep\config\Modules\ToastyHUD\ToastyHUD.acs.cs
//
// ★The slide is derived from the clock too.★ Legacy animated by rescheduling
// ToastyHUD::Animate every 0.0002s -- a 5000Hz timer walking a counter to 400 and
// assigning %obj.position each step. Same reasoning as the fade above, but far
// worse: it also mutated a retained control's position, which is exactly the
// second movable identity the framework exists to remove.
//
// ★DEVIATION (logged): String::explode -> String::Explode.★ The legacy parser
// called String::explode, which the engine does not register under any casing, so
// $player[0]/$player[1] were always empty, %shooter/%victim never matched
// $PCFG::Name, and this feature has never once triggered in a shipped Overstep.
// Converting it faithfully would mean converting dead code. Same fix in the praise
// list used by the centre print.
//
// Not carried across: the screenshot mode. It worked by walking playGui's children
// and hiding every control except its own, which has no meaning for immediate-mode
// parts that are not playGui children. See CONVERSION_NOTES.md.
//----------------------------------------------------------------------------
$ModernHUDPack::ToastyWidth = 401;       // the shipped toasty.png is 401x401
$ModernHUDPack::ToastySlide = 200;       // ms for the slide in/out
$ModernHUDPack::ToastyHold  = 800;       // ms fully on screen (legacy: anim in at
                                         // 0.2s, out at 1.0s)
$ModernHUDPack::ToastyMinMeters = 50;    // legacy $MA_METER_DIST

function ModernHUDPack::toastyMeters(%msg)
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

function ModernHUDPack::toastyMessage(%msg)
{
   if($playingDemo)
      return;
   if(String::findSubStr(%msg, "mid-air") == -1)
      return;

   %meters = ModernHUDPack::toastyMeters(%msg);
   if(%meters < $ModernHUDPack::ToastyMinMeters)
      return;

   %pairs = String::Trim(String::Replace(String::Replace(%msg,
               sprintf(" lands [ %1 meter ] mid-air on ", %meters), ", "), "!", ""));
   // ★String::Explode, not String::explode -- see the deviation note above.★
   String::Explode(%pairs, ", ", "toastyName");
   %shooter = $toastyName[0];
   %victim  = $toastyName[1];

   if(%shooter != $PCFG::Name && %victim != $PCFG::Name)
      return;

   $ModernHUDPack::ToastyAt = getSimTime();
   $ModernHUDPack::ToastyShooter = %shooter;
   $ModernHUDPack::ToastyVictim = %victim;
   $ModernHUDPack::ToastyMeters = %meters;
   $ToastyHUD::PTotal++;

   ModernHUDPack::toastyCenterPrint();
   localSound("mk.toasty.ogg");
}

function ModernHUDPack::toastyCenterPrint()
{
   %praises = "spanked smacked pwned obliterated popped hit owned";
   String::Explode(%praises, " ", "toastyPraise");
   %rand = floor(getRandom() * 6);

   %shooter = ($ModernHUDPack::ToastyShooter == $PCFG::Name)
              ? "You" : $ModernHUDPack::ToastyShooter;
   %victim  = ($ModernHUDPack::ToastyVictim == $PCFG::Name)
              ? "you" : $ModernHUDPack::ToastyVictim;

   remoteBP(2048, sprintf("<JC><F1>TOOASTY! <F2>%1<F1> %2<F2> %3<F1> at<F2> %4<F1> meters!"
            @ "\n\nTotal Toasty's: %5",
            %shooter, $toastyPraise[%rand], %victim,
            $ModernHUDPack::ToastyMeters, $ToastyHUD::PTotal), 3);
}

function ModernHUDPack::drawToasty(%screen)
{
   %at = $ModernHUDPack::ToastyAt;
   if(%at == "")
      return;

   %elapsed = getSimTime() - %at;
   if(%elapsed < 0)
   {
      $ModernHUDPack::ToastyAt = "";
      return;
   }

   %w = $ModernHUDPack::ToastyWidth;
   %slide = $ModernHUDPack::ToastySlide;
   %total = %slide + $ModernHUDPack::ToastyHold + %slide;
   if(%elapsed >= %total)
   {
      $ModernHUDPack::ToastyAt = "";
      return;
   }

   // How far out of the right edge the art still is: full width before the slide
   // completes, zero while held, back out again on the way off.
   if(%elapsed < %slide)
      %hidden = %w - (%w * %elapsed / %slide);
   else if(%elapsed < %slide + $ModernHUDPack::ToastyHold)
      %hidden = 0;
   else
      %hidden = %w * (%elapsed - %slide - $ModernHUDPack::ToastyHold) / %slide;

   %default = ModernHUD::place("bottom-right", 0, 40, %w, %w, %screen);
   %pos = ModernHUDPack::handle("ModernHUD::Toasty", %default, %w, %w);
   glDrawImage(getWord(%pos, 0) + %hidden, getWord(%pos, 1), %w, %w,
               "Modules/ToastyHUD/toasty.png", 255);
}

ModernHUD::component("overstep", "status",   "healthenergy", "ModernHUDPack::drawStatus");
ModernHUD::component("overstep", "weapon",   "weapon",       "ModernHUDPack::drawWeapons");
ModernHUD::component("overstep", "items",    "items",        "ModernHUDPack::drawItems");
ModernHUD::component("overstep", "ctfclock", "ctf",          "ModernHUDPack::drawCtfClock");
ModernHUD::component("overstep", "minimap",  "minimap",      "ModernHUDPack::drawMinimapFrame");
ModernHUD::component("overstep", "score",    "ratings",      "ModernHUDPack::drawScore");
ModernHUD::component("overstep", "repkit",     "repkit",     "ModernHUDPack::drawRepKit");
ModernHUD::component("overstep", "lowhealth",  "lowhealth",  "ModernHUDPack::drawLowHealth");
ModernHUD::component("overstep", "flagpopup",  "flagpopup",  "ModernHUDPack::drawFlagPopup");
ModernHUD::component("overstep", "toasty",     "toasty",     "ModernHUDPack::drawToasty");

// ModernHUD::attach, not Event::Attach: the framework records these and drops them
// in detachAll() when the pack unloads. A raw Event::Attach would survive a config
// swap and keep firing this pack's handlers under the next pack.
// Canonical Presto TeamTrak names: the tracker emits eventFlagTaken/Dropped/
// Captured (args teamFlag, client); the Grab/Pickup/Drop/Cap aliases have no
// installed emitter, so the popup never fired.
ModernHUD::attach("eventFlagTaken",     "ModernHUDPack::flagPopupShow");
ModernHUD::attach("eventFlagDropped",   "ModernHUDPack::flagPopupEnd");
ModernHUD::attach("eventFlagCaptured",  "ModernHUDPack::flagPopupEnd");
ModernHUD::attach("eventChangeMission", "ModernHUDPack::flagPopupReset");
ModernHUD::attach("eventServerMessage", "ModernHUDPack::toastyMessage");

ModernHUDPack::prefs();
ModernHUD::legacyMap();

// Font-scope Stage 3: load-completion sentinel -- MUST stay the final statement.
$ModernHUD::LoadComplete = "overstep";
