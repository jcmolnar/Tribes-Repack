//----------------------------------------------------------------------------
// Mech Mayhem -- heat system (Stage 2).
// Energy IS inverted heat: weapons drain the pool (ItemImageData maxEnergy =
// heat per shot), engine recharge = dissipation. This file adds the DRAMA:
// pool near zero -> SHUTDOWN (Player::setArmor to the <Chassis>Down twin,
// ~6s statue) -> restart with the pool restored to 30%.
//
// The tick is kicked from Game::startMatch (MechGame.cs) -- NEVER from boot
// scope: ConsoleScheduler is recreated at mission load and boot-scope
// schedules are silently dropped.
//
// Works for humans AND bots: iterates MissionCleanup for Herc* players.
//----------------------------------------------------------------------------

$MM::HeatTickSecs = 0.4;      // fast poll: engine recharge (~12/s) refills a
$MM::ShutdownSecs = 6.0;      // transient zero before a slow tick can see it
$MM::ShutdownAt = 6;          // pool floor that trips the overheat
$MM::RestartEnergyPct = 0.30;

function MechHeat::isMech(%data)
{
   // all mech datablocks start "Herc"
   return String::getSubStr(%data, 0, 4) == "Herc";
}

// strip a trailing Down/Crip to get the base chassis datablock name
function MechHeat::baseChassis(%data)
{
   %n = String::len(%data);
   if (String::getSubStr(%data, %n - 4, 4) == "Down")
      return String::getSubStr(%data, 0, %n - 4);
   if (String::getSubStr(%data, %n - 4, 4) == "Crip")
      return String::getSubStr(%data, 0, %n - 4);
   return %data;
}

function MechHeat::visit(%obj)
{
   %data = GameBase::getDataName(%obj);
   if (!MechHeat::isMech(%data))
      return;
   if (Player::isDead(%obj))
      return;

   // BOT CONSCRIPTION INIT: BotBrain spawns its bots directly (AI_spawnBot),
   // bypassing Game::playerSpawned -- a roster-mech bot arrives here with no
   // shields and no mounted loadout. Idempotent via mmInit (humans get it set
   // in Game::playerSpawned). findPlayerObject resolves the player object for
   // both grantLoadout args, so %obj serves as the client id.
   if (%obj.mmInit != 1) {
      %obj.mmInit = 1;
      %baseC = MechHeat::baseChassis(%data);
      MechShield::init(%obj, %baseC);
      MechMayhem::grantLoadout(%obj, %obj, %baseC);
      echo("[MECH] conscript init: " @ %obj @ " (" @ %baseC @ ")");
   }

   // per-mission dissipation preset ($MM::DissipScale from the mission tail;
   // 1.0 = engine recharge alone, <1 bleeds heat capacity, >1 cools faster --
   // Whiteout ice maps run 1.3, Monsoon storms run 0.6)
   if ($MM::DissipScale != "" && $MM::DissipScale != 1) {
      %base = MechHeat::baseChassis(%data);
      %delta = (%base.maxEnergy * 0.02) * ($MM::DissipScale - 1) * $MM::HeatTickSecs;
      %e = GameBase::getEnergy(%obj) + %delta;
      if (%e < 0) %e = 0;
      if (%e > %base.maxEnergy) %e = %base.maxEnergy;
      GameBase::setEnergy(%obj, %e);
   }

   MechShield::regen(%obj);

   if (%obj.mmShutdown == 1)
      return;   // restart is on its own schedule

   %energy = GameBase::getEnergy(%obj);
   if (%energy <= $MM::ShutdownAt)
      MechHeat::shutdown(%obj, %data);
}

function MechHeat::shutdown(%obj, %data)
{
   %base = MechHeat::baseChassis(%data);
   %obj.mmShutdown = 1;
   %obj.mmChassis = %base;
   // remember hull -- setArmor behavior on damage is verified in the Stage 2
   // boot test; belt-and-braces restore either way
   %obj.mmDamage = GameBase::getDamageLevel(%obj);
   Player::setArmor(%obj, %base @ "Down");
   GameBase::setDamageLevel(%obj, %obj.mmDamage);
   %cl = Player::getClient(%obj);
   if (%cl > 0)
      Client::sendMessage(%cl, 1, "REACTOR OVERHEAT -- EMERGENCY SHUTDOWN");
   echo("[MECHHEAT] shutdown: " @ %obj @ " (" @ %base @ ")");
   schedule("MechHeat::restart(" @ %obj @ ");", $MM::ShutdownSecs, %obj);
}

function MechHeat::restart(%obj)
{
   if (%obj == "" || %obj <= 0)
      return;
   %data = GameBase::getDataName(%obj);
   if (%data == "" || Player::isDead(%obj)) {
      return;   // died while shut down; respawn resets everything
   }
   %base = %obj.mmChassis;
   %dmg = GameBase::getDamageLevel(%obj);
   Player::setArmor(%obj, %base);
   GameBase::setDamageLevel(%obj, %dmg);
   GameBase::setEnergy(%obj, %base.maxEnergy * $MM::RestartEnergyPct);
   %obj.mmShutdown = 0;
   %cl = Player::getClient(%obj);
   if (%cl > 0)
      Client::sendMessage(%cl, 1, "Reactor restart complete.");
   echo("[MECHHEAT] restart: " @ %obj @ " (" @ %base @ ")");
}

function MechHeat::tick()
{
   Group::iterateRecursive(MissionCleanup, "MechHeat::visit");
   schedule("MechHeat::tick();", $MM::HeatTickSecs);
}

echo("[MECH] MechHeat loaded.");
