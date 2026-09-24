//==============================================================================
// OPS -- BORROWABLE PARTS (components.cs)
//
// A ModernHUD REDRAW of Opsaya's config ("hyung" ScriptGL HUD), not a conversion:
// the same functions and layout, rebuilt as immediate-mode parts. Source of truth
// for every number here is the shared config (2026-09-24):
//   Modules/ScriptGL/vHealth.acs.cs, vEnergy.acs.cs  -> OPS::Vitals
//   Modules/ScriptGL/vClockrkitHUD.acs.cs             -> OPS::Clock
//   Modules/ScriptGL/vCtfHUD.acs.cs                   -> OPS::Ctf
//   Modules/EnemyHUD.acs.cs                           -> OPS::Enemy
//   Modules/WeaponsHud/weaponsHud.acs.cs              -> OPS::Weapons
// Art is Opsaya's own: the two bar strips (pre-scaled to the 160x20 box they were
// drawn into), the flag shields, the kit dot and the weapon icons.
//
// ★The top cluster hangs off the CHAT BOX, as the original did.★ Vitals and clock
// sit left of chatDisplayHud, the flag panel right of it; move the chat and they
// follow. ModernHUD::dockTo makes them decorations of the chat, so the chat is the
// one handle for all three.
//
// The HUD designer's Provider choice loads this file on its own to borrow a part,
// so it must define only OPS:: names, and every event hook a part needs is here.
//==============================================================================

ModernHUD::require("ModernHUD/Core/Data/Team.cs");
ModernHUD::require("ModernHUD/Core/Data/Timer.cs");

$OPS::Art = "Assets/Packs/ops/";

// Text in a TrueType face with an inline colour, as ScriptGL drew it.
function OPS::text(%x, %y, %font, %px, %rgb, %str)
{
   glSetFont(%font, %px);
   glDrawString(%x, %y, "<" @ %rgb @ ">" @ %str);
}

// The original's frame: black at 187 behind every ScriptGL block.
function OPS::frame(%x, %y, %w, %h)
{
   glColor4ub(0, 0, 0, 187);
   glRectangle(%x, %y, %w, %h);
}

// Re-seat a docked part: dockTo moves the box, so the part's size/opacity style
// must be re-applied about the docked corner, not the undocked fallback.
function OPS::dock(%name, %dx, %dy, %at)
{
   %at = ModernHUD::dockTo(%name, "chatDisplayHud", %dx, %dy, %at, 0, 0);
   ModernHUD::partStyle(%name, %at);
   return %at;
}

function OPS::chatW()
{
   %ext = Control::getExtent("chatDisplayHud");
   %w = getWord(%ext, 0);
   if(%ext == "" || %w <= 0)
      %w = 370;
   return %w;
}

//------------------------------------------------------------------------------
// PART: vitals -- health over energy, 160x20 each, left of the chat.
// vHealth/vEnergy: a 187-alpha frame, then the strip revealed to $health*160/100.
//------------------------------------------------------------------------------
function OPS::Vitals(%x, %y)
{
   %h = $health;  if(%h == "") %h = 0;
   %e = $energy;  if(%e == "") %e = 0;
   if(%h > 100) %h = 100;
   if(%e > 100) %e = 100;

   OPS::frame(%x, %y, 160, 20);
   ModernHUD::bar(%x, %y, floor(%h * 160 / 100), 20, $OPS::Art @ "bars/healthhud.png", 255);

   OPS::frame(%x, %y + 20, 160, 20);
   ModernHUD::bar(%x, %y + 20, floor(%e * 160 / 100), 20, $OPS::Art @ "bars/energyhud.png", 255);
}

//------------------------------------------------------------------------------
// PART: clock -- repair-kit dot and the match clock, under the vitals.
// vClockrkitHUD kept its own clock off eventUpdateTime + getSimTime; getHudTimer()
// is the engine's continuously-advanced copy of the same server time, so it is
// right the moment the pack loads instead of at the next :00/:20/:40 pulse.
// Format "MM:SS.t", minutes uncapped (a 30-minute match reads 30:00.0).
//------------------------------------------------------------------------------
function OPS::Clock(%x, %y)
{
   OPS::frame(%x, %y, 160, 21);

   if(getItemCount("Repair Kit") > 0)
      ModernHUD::imageAt(%x + 5, %y + 4, $OPS::Art @ "flags/kitdot.png", 255);

   %t = getHudTimer();
   if(%t == "") %t = 0;
   if(%t < 0) %t = -%t;
   %m = floor(%t / 60);
   %s = %t - %m * 60;
   %whole = floor(%s);
   %tenth = floor((%s - %whole) * 10);
   %str = OPS::pad2(%m) @ ":" @ OPS::pad2(%whole) @ "." @ %tenth;
   OPS::text(%x + 113, %y + 4, "Verdana", 11, "ffffff", %str);
}

function OPS::pad2(%n)
{
   return (%n < 10) ? "0" @ %n : %n;
}

//------------------------------------------------------------------------------
// PART: ctf -- both flags, right of the chat (vCtfHUD). One row per team:
// shield at +5, score at +35, state at +55. Friendly on top.
//   home      white "home"
//   dropped   yellow "Dropped 19.5" (the return countdown)
//   carried   the carrier's name: RED when they have OUR flag, GREEN when we have theirs
//------------------------------------------------------------------------------
function OPS::Ctf(%x, %y)
{
   OPS::frame(%x, %y, 160, 61);
   OPS::ctfRow(%x, %y + 8,  Team::Friendly(), true);
   OPS::ctfRow(%x, %y + 32, Team::Enemy(),    false);
}

function OPS::ctfRow(%x, %y, %team, %friendly)
{
   %loc = Team::Flag::Location(%team);
   %score = Team::Score(%team);
   if(%score == "") %score = 0;

   if(%loc == "field")
   {
      %icon = "flag.dropped.png";
      %rgb = "f7fa05";
      %str = "Dropped " @ Team::Flag::Timer(%team);
   }
   else if(%loc == "home")
   {
      %icon = %friendly ? "flag.friend.png" : "flag.nmy.png";
      %rgb = "ffffff";
      %str = "home";
   }
   else
   {
      %icon = %friendly ? "flag.friend.taken.png" : "flag.nmy.taken.png";
      %rgb = %friendly ? "ff0000" : "00ff00";
      %str = Client::getName(%loc);
   }

   ModernHUD::imageAt(%x + 5, %y, $OPS::Art @ "flags/" @ %icon, 255);
   OPS::text(%x + 35, %y + 4, "Arial", 11, "ffffff", %score);
   OPS::text(%x + 55, %y + 4, "Arial", 11, %rgb, %str);
}

//------------------------------------------------------------------------------
// PART: enemy -- EnemyHUD: the first five enemies and how long each has been
// alive, in seconds, left of the minimap. A shaded 150x85 box, name at +5, timer
// at +125, 16px rows.
//
// The original counted with a 1 Hz schedule and reset a slot on that player's
// death. Here a death stamps glTicks() and the draw shows the elapsed seconds --
// the same number without a timer loop. A player seen for the first time starts
// from that moment (the original started everyone at 0 on connect / match start).
//------------------------------------------------------------------------------
function OPS::enemyReset()
{
   deleteVariables("$OPS::Alive*");
}

function OPS::enemyDied(%killer, %victim, %weapon)
{
   $OPS::Alive[%victim] = glTicks();
}

function OPS::Enemy(%x, %y)
{
   // Stock ShadedHudCtrl backdrop (alpha-palette 254, as xLoader's CTF panel).
   glColor4ub(0, 0, 0, 110);
   glRectangle(%x, %y, 150, 85);

   if($ModernHUD::PvDemo)
   {
      OPS::enemyRow(%x, %y,      "Enemy Heavy", 42);
      OPS::enemyRow(%x, %y + 16, "Enemy Light", 7);
      return;
   }

   %enemy = Team::Enemy();
   %now = glTicks();
   %row = 0;
   %cl = Client::getFirst();
   for(%guard = 0; %guard < 128 && %row < 5 && %cl != -1 && %cl != ""; %guard++)
   {
      if(Client::getTeam(%cl) == %enemy && %cl != getManagerId())
      {
         if($OPS::Alive[%cl] == "")
            $OPS::Alive[%cl] = %now;
         // EnemyHUD::Toggle(n) hid a slot's controls, leaving its gap; the K panel's
         // "Enemy row n" rows do the same ($pref::OPS::EnemyRow<n>, absent = shown).
         if($pref::OPS::EnemyRow[%row + 1] != "0")
            OPS::enemyRow(%x, %y + %row * 16, Client::getName(%cl), floor((%now - $OPS::Alive[%cl]) / 1000));
         %row++;
      }
      %cl = Client::getNext(%cl);
   }
}

function OPS::enemyRow(%x, %y, %name, %secs)
{
   OPS::text(%x + 5,   %y + 2, "Arial", 11, "ffffff", %name);
   OPS::text(%x + 125, %y + 2, "Arial", 11, "ffffff", %secs);
}

//------------------------------------------------------------------------------
// PART: weapons -- every weapon, pack and deployable you carry, in item-id order,
// 70px apart along the bottom-left (WH::Update). The mounted weapon uses its "on"
// art; weapons with ammo show the count under the icon.
//
// The inventory scan runs at most every 150 ms (the original polled every 0.5 s,
// 0.1 s while firing); drawing reads the cached row.
//------------------------------------------------------------------------------
$OPS::WN = 0;
function OPS::wAdd(%item, %file, %ammo, %hasOn)
{
   $OPS::WName[$OPS::WN] = %item;
   $OPS::WFile[$OPS::WN] = %file;
   $OPS::WAmmo[$OPS::WN] = %ammo;
   $OPS::WOn[$OPS::WN]   = %hasOn;
   $OPS::WN++;
}

// WH::Init's table. Two entries point at art that exists: the original's
// "repair" (Repair Gun) and "motionsensor" files were never in the config, so
// those drew nothing. Rebuilt on every exec ($OPS::WN reset above).
   OPS::wAdd("Blaster",           "blaster",          "",             true);
   OPS::wAdd("ChainGun",           "chaingun",         "Bullet",       true);
   OPS::wAdd("Disc Launcher",      "disk",             "Disc",         true);
   OPS::wAdd("Elf Gun",            "elf",              "",             true);
   OPS::wAdd("Grenade Launcher",   "grenade",          "Grenade Ammo", true);
   OPS::wAdd("Laser Rifle",        "sniper",           "",             true);
   OPS::wAdd("Mortar",             "mortar",           "Mortar Ammo",  true);
   OPS::wAdd("Plasma Gun",         "plasma",           "Plasma Bolt",  true);
   OPS::wAdd("Repair Gun",         "repairpack",       "",             true);
   OPS::wAdd("Inventory Station",  "inventory",        "",             false);
   OPS::wAdd("Ammo Station",       "ammostation",      "",             false);
   OPS::wAdd("Ammo Pack",          "ammopack",         "",             false);
   OPS::wAdd("Camera",             "camera",           "",             false);
   OPS::wAdd("Energy Pack",        "energypack",       "",             false);
   OPS::wAdd("Motion Sensor",      "motion",           "",             false);
   OPS::wAdd("Pulse Sensor",       "pulse",            "",             false);
   OPS::wAdd("Repair Pack",        "repairpack",       "",             true);
   OPS::wAdd("Shield Pack",        "shieldpack",       "",             true);
   OPS::wAdd("Sensor Jammer Pack", "sensorjammerpack", "",             true);
   OPS::wAdd("Sensor Jammer",      "sensorjammer",     "",             false);
   OPS::wAdd("Turret",             "turret",           "",             false);
   // Renegades Classic, mapped onto the base art exactly as the original did.
   OPS::wAdd("Hyper Blaster",      "plasma",           "",             true);
   OPS::wAdd("Rocket Launcher",    "mortar",           "Rockets",      true);
   OPS::wAdd("Sniper Rifle",       "sniper",           "Sniper Bullet", true);
   OPS::wAdd("Dart Rifle",         "sniper",           "Poison Dart",  true);
   OPS::wAdd("Magnum",             "blaster",          "Magnum Bullets", true);
   OPS::wAdd("Shockwave Cannon",   "elf",              "",             true);
   OPS::wAdd("Railgun",            "sniper",           "Railgun Bolt", true);
   OPS::wAdd("Vulcan",             "chaingun",         "Vulcan Bullet", true);
   OPS::wAdd("Flame Thrower",      "grenade",          "",             true);
   OPS::wAdd("Ion Rifle",          "grenade",          "",             true);
   OPS::wAdd("Omega Cannon",       "elf",              "",             true);
   OPS::wAdd("Thunderbolt",        "elf",              "",             true);
   OPS::wAdd("Engineer Repair-Gun","repairpack",       "",             true);
   OPS::wAdd("Cloaking Device",    "sensorjammerpack", "",             true);
   OPS::wAdd("StealthShield Pack", "shieldpack",       "",             true);
   OPS::wAdd("Regeneration Pack",  "repairpack",       "",             true);
   OPS::wAdd("Lightning Pack",     "energypack",       "",             false);
   OPS::wAdd("Suicide DetPack",    "ammopack",         "",             false);
   OPS::wAdd("Command Station",    "inventory",        "",             false);
   OPS::wAdd("Ion Turret",         "turret",           "",             false);
   OPS::wAdd("Laser Turret",       "camera",           "",             false);
   OPS::wAdd("Shock Turret",       "camera",           "",             false);
   OPS::wAdd("Mortar Turret",      "turret",           "",             false);
   OPS::wAdd("Plasma Turret",      "turret",           "",             false);
   OPS::wAdd("Vulcan Turret",      "turret",           "",             false);
   OPS::wAdd("Rail Turret",        "turret",           "",             false);
   OPS::wAdd("Missile Turret",     "turret",           "",             false);
   OPS::wAdd("Force Field",        "ammopack",         "",             false);
   OPS::wAdd("Large Force Field",  "ammopack",         "",             false);
   OPS::wAdd("Blast Wall",         "ammopack",         "",             false);
   OPS::wAdd("Hologram",           "ammopack",         "",             false);
   OPS::wAdd("Mechanical Tree",    "ammopack",         "",             false);
   OPS::wAdd("Springboard",        "ammopack",         "",             false);
   OPS::wAdd("Deployable Platform","ammopack",         "",             false);
   OPS::wAdd("Teleport Pad",       "ammopack",         "",             false);
   OPS::wAdd("Interceptor Pack",   "ammopack",         "",             false);
   OPS::wAdd("StealthHPC Pack",    "ammopack",         "",             false);

// Rebuild $OPS::Slot* : carried entries, sorted by item type id (WH::Sort).
function OPS::wScan()
{
   %n = 0;
   for(%i = 0; %i < $OPS::WN; %i++)
   {
      if(getItemCount($OPS::WName[%i]) <= 0)
         continue;
      %type = getItemType($OPS::WName[%i]);
      if(%type == "" || %type == -1)
         continue;
      // insertion by type id; 11 slots, as the original
      %pos = %n;
      for(%j = 0; %j < %n; %j++)
         if(%type < $OPS::SlotType[%j]) { %pos = %j; break; }
      if(%pos >= 11)
         continue;
      if(%n < 11)
         %n++;
      for(%j = %n - 1; %j > %pos; %j--)
      {
         $OPS::SlotType[%j] = $OPS::SlotType[%j - 1];
         $OPS::SlotIdx[%j]  = $OPS::SlotIdx[%j - 1];
      }
      $OPS::SlotType[%pos] = %type;
      $OPS::SlotIdx[%pos]  = %i;
   }
   $OPS::SlotN = %n;
}

function OPS::Weapons(%x, %y)
{
   %now = glTicks();
   %demo = $ModernHUD::PvDemo ? 1 : 0;
   if(%now - $OPS::WScanAt > 150 || %now < $OPS::WScanAt || $OPS::WScanDemo != %demo)
   {
      OPS::wScan();
      $OPS::WScanAt = %now;
      $OPS::WScanDemo = %demo;
   }

   %mounted = getMountedItem(0);
   for(%s = 0; %s < $OPS::SlotN; %s++)
   {
      %i = $OPS::SlotIdx[%s];
      %sx = %x + 70 * %s;
      %file = $OPS::WFile[%i];
      if($OPS::WOn[%i] && $OPS::SlotType[%s] == %mounted)
         %file = %file @ "on";
      ModernHUD::imageAt(%sx, %y + 5, $OPS::Art @ "weapons/" @ %file @ ".png", 255);

      if($OPS::WAmmo[%i] != "")
         OPS::text(%sx + 29, %y + 25, "Arial", 11, "ffc83c", getItemCount($OPS::WAmmo[%i]));
   }
}

//------------------------------------------------------------------------------
// Event hooks (here, not hud.cs, so a borrowed part still gets them).
//------------------------------------------------------------------------------
ModernHUD::attach("eventClientKilled",       "OPS::enemyDied");
ModernHUD::attach("eventClientTeamKilled",   "OPS::enemyDied");
ModernHUD::attach("eventConnectionAccepted", "OPS::enemyReset");
ModernHUD::attach("eventMatchStarted",       "OPS::enemyReset");

//------------------------------------------------------------------------------
// Part wrappers. The top cluster's fallback anchors reproduce the chat-relative
// layout around a centred 370px chat at y 6, for when the chat is not on screen.
//------------------------------------------------------------------------------
function OPS::draw_vitals(%screen)
{
   %at = ModernHUD::part("ModernHUD::OpsVitals", "top-center", -265, 6, 160, 40, %screen);
   %at = OPS::dock("ModernHUD::OpsVitals", -160, 0, %at);
   OPS::Vitals(getWord(%at, 0), getWord(%at, 1));
}

function OPS::draw_clock(%screen)
{
   %at = ModernHUD::part("ModernHUD::OpsClock", "top-center", -265, 46, 160, 21, %screen);
   %at = OPS::dock("ModernHUD::OpsClock", -160, 40, %at);
   OPS::Clock(getWord(%at, 0), getWord(%at, 1));
}

function OPS::draw_ctf(%screen)
{
   %at = ModernHUD::part("ModernHUD::OpsCtf", "top-center", 265, 6, 160, 61, %screen);
   %at = OPS::dock("ModernHUD::OpsCtf", OPS::chatW(), 0, %at);
   OPS::Ctf(getWord(%at, 0), getWord(%at, 1));
}

function OPS::draw_enemy(%screen)
{
   %at = ModernHUD::part("ModernHUD::OpsEnemy", "top-right", 313, 8, 150, 85, %screen);
   OPS::Enemy(getWord(%at, 0), getWord(%at, 1));
}

function OPS::draw_weapons(%screen)
{
   %at = ModernHUD::part("ModernHUD::OpsWeapons", "bottom-left", 0, 0, 770, 50, %screen);
   OPS::Weapons(getWord(%at, 0), getWord(%at, 1));
}

//------------------------------------------------------------------------------
// Components: one per slot.
//------------------------------------------------------------------------------
function OPS::comp_healthenergy(%screen) { OPS::draw_vitals(%screen); }
function OPS::comp_clock(%screen)        { OPS::draw_clock(%screen); }
function OPS::comp_ctf(%screen)          { OPS::draw_ctf(%screen); }
function OPS::comp_enemy(%screen)        { OPS::draw_enemy(%screen); }
function OPS::comp_weapon(%screen)       { OPS::draw_weapons(%screen); }

ModernHUD::component("ops", "healthenergy", "healthenergy", "OPS::comp_healthenergy");
ModernHUD::component("ops", "clock",        "clock",        "OPS::comp_clock");
ModernHUD::component("ops", "ctf",          "ctf",          "OPS::comp_ctf");
ModernHUD::component("ops", "enemy",        "enemy",        "OPS::comp_enemy");
ModernHUD::component("ops", "weapon",       "weapon",       "OPS::comp_weapon");
