//----------------------------------------------------------------------------
// Mech Mayhem -- GROUNDWAR zone domination (Stage 7).
// Active when the mission tail sets $MM::Mode = "groundwar" and defines
// $MM::ZoneCount + $MM::Zone[i] ("x y z") + $MM::ZoneRadius.
//
// Majority presence owns a zone; the team holding MORE zones drains the
// other's CV ticket pool ($MM::DrainPerTick per zone of advantage, on the
// zone tick). Ticket pools / endMatch come from MechGame's ESCALATION kit.
//
// Bots: BotBrain::Order (native, team-wide defend orders with 60s expiry)
// pushes each team at the most contested zone on a rotating cadence -- no
// new bot roles needed.
//----------------------------------------------------------------------------

$MM::ZoneTickSecs = 5;
$MM::OrderSecs = 20;
$MM::DrainPerTick = 120;     // CV per zone-advantage per tick

function MechZones::start()
{
   if ($MM::ZoneCount == "" || $MM::ZoneCount < 1) {
      echo("[MECHZONE] no zones defined; groundwar inert");
      return;
   }
   for (%i = 0; %i < $MM::ZoneCount; %i++)
      $MMZ::owner[%i] = -1;
   schedule("MechZones::tick();", $MM::ZoneTickSecs);
   schedule("MechZones::orders();", 10);
   messageAll(0, "GROUNDWAR: hold the majority of " @ $MM::ZoneCount @ " zones to bleed the enemy's CV.");
   echo("[MECHZONE] " @ $MM::ZoneCount @ " zones armed.");
}

function MechZones::censusVisit(%obj)
{
   %data = GameBase::getDataName(%obj);
   if (String::getSubStr(%data, 0, 4) != "Herc")
      return;
   if (Player::isDead(%obj))
      return;
   %pos = GameBase::getPosition(%obj);
   %team = GameBase::getTeam(%obj);
   if (%team != 0 && %team != 1)
      return;
   %d = Vector::getDistance(%pos, $MMZ::checkPos);
   if (%d <= $MM::ZoneRadius)
      $MMZ::count[%team]++;
}

function MechZones::tick()
{
   if ($MM::MatchOver == 1)
      return;
   %h0 = 0; %h1 = 0;
   for (%i = 0; %i < $MM::ZoneCount; %i++) {
      $MMZ::checkPos = $MM::Zone[%i];
      $MMZ::count[0] = 0;
      $MMZ::count[1] = 0;
      Group::iterateRecursive(MissionCleanup, "MechZones::censusVisit");
      %owner = $MMZ::owner[%i];
      if ($MMZ::count[0] > $MMZ::count[1])
         %newOwner = 0;
      else if ($MMZ::count[1] > $MMZ::count[0])
         %newOwner = 1;
      else
         %newOwner = %owner;   // ties hold
      if (%newOwner != %owner && %newOwner != -1) {
         $MMZ::owner[%i] = %newOwner;
         messageAll(0, "Zone " @ (%i + 1) @ " captured by " @ getTeamName(%newOwner) @ "!");
         echo("[MECHZONE] zone " @ %i @ " -> team " @ %newOwner);
      }
      if ($MMZ::owner[%i] == 0) %h0++;
      if ($MMZ::owner[%i] == 1) %h1++;
   }
   if (%h0 > %h1)
      MechZones::drain(1, %h0 - %h1);
   else if (%h1 > %h0)
      MechZones::drain(0, %h1 - %h0);
   schedule("MechZones::tick();", $MM::ZoneTickSecs);
}

function MechZones::drain(%team, %zones)
{
   $MM::Tickets[%team] = $MM::Tickets[%team] - $MM::DrainPerTick * %zones;
   $teamScore[%team] = $MM::Tickets[%team];
   echo("[MECHZONE] drain team " @ %team @ " -" @ ($MM::DrainPerTick * %zones)
        @ " -> " @ $MM::Tickets[%team]);
   if ($MM::Tickets[%team] <= 0)
      MechTickets::endMatch(%team);
}

// rotate each team's bots onto the most useful zone: the lowest-index zone
// they do not own (contested or enemy-held); everyone defends if all owned
function MechZones::orders()
{
   if ($MM::MatchOver == 1)
      return;
   for (%t = 0; %t < 2; %t++) {
      %target = -1;
      for (%i = 0; %i < $MM::ZoneCount; %i++) {
         if ($MMZ::owner[%i] != %t) { %target = %i; break; }
      }
      if (%target == -1)
         %target = %t;   // own everything: garrison a home zone
      %pos = $MM::Zone[%target];
      BotBrain::Order("team" @ %t, "defend", getWord(%pos, 0), getWord(%pos, 1), getWord(%pos, 2));
   }
   schedule("MechZones::orders();", $MM::OrderSecs);
}

echo("[MECH] MechZones loaded.");
