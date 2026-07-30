// Overstep Phase A image/animation gate.
//
// This is intentionally a small, measurable slice, not the final pack. It
// proves native PNG alpha, GIF animation, content-sized layout, per-draw fade
// and per-digit number art before the other HUD parts are converted.

exec("ModernHUD/Framework.cs");

$ModernHUD::Enabled = true;
$ModernHUD::Pack = "Tribes_Overstep";

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
   Control::SetVisible(clockHud, false);
   Control::SetVisible(sensorHUD, false);
   Control::SetVisible(compassHud, false);
   Control::SetVisible(jetPackHud, false);
   Control::SetVisible(healthHud, false);
   Control::SetVisible(weaponHud, false);
   Control::SetVisible(ChatDisplayHUD, true);
   Control::SetVisible(Minimap, true);
   Control::SetVisible(reticleCompass, false);
}

function ModernHUDPack::onGuiOpen(%gui)
{
   if(%gui == "playGui")
   {
      Schedule::Add("ModernHUDPack::detachRetained();", 0);
      // Re-applied per gui open: the stock controls are recreated with the gui, so
      // setting them once at load does not survive the first transition.
      Schedule::Add("ModernHUDPack::stockHuds();", 0);
   }
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
      Hud::Restore(%index);
      addToSet(playGui, %handle);
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
   if(%published != "")
      return %published;
   return %controlPos;
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

Event::Attach(eventGuiOpen, ModernHUDPack::onGuiOpen);
ModernHUDPack::stockHuds();
ModernHUDPack::detachRetained();
