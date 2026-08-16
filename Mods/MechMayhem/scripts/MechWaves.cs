//----------------------------------------------------------------------------
// Mech Mayhem -- INCURSION wave director (Stage 5).
// Active when the mission tail sets $MM::Mode = "incursion". Team 0 =
// defenders (humans + friendly bots), team 1 = the Cybrid horde, spawn-gated
// natively (BotBrain::HoldTeam / BotBrain::Release -- released slots spawn,
// re-gate on death). Wave composition IS the roster order: list swarm chassis
// first and Prometheus last in botbrain.cfg and escalation comes for free.
//
// v1 win/loss: survive $MM::MaxWave waves / full defender wipe mid-wave.
// (The generator objective + attack-object bot logic land with GROUNDWAR's
// zone-hold native work in Stage 7.)
//
// All scheduling starts from Game::startMatch -- never boot scope.
//----------------------------------------------------------------------------

$MM::MaxWave = 10;
$MM::WaveBase = 3;        // wave N releases WaveBase + N - 1 cybrids
$MM::SalvageSecs = 45;    // breather between waves
$MM::FirstWaveSecs = 20;

function MechWaves::start()
{
   $MMW::wave = 0;
   $MMW::over = 0;
   BotBrain::HoldTeam(1, 1);
   messageAll(0, "INCURSION: Cybrid signatures inbound. First wave in " @ $MM::FirstWaveSecs @ " seconds.");
   schedule("MechWaves::launch();", $MM::FirstWaveSecs);
   echo("[MECHWAVE] director armed.");
}

function MechWaves::launch()
{
   if ($MMW::over == 1)
      return;
   $MMW::wave++;
   %n = $MM::WaveBase + $MMW::wave - 1;
   %tail = 0;
   if ($MMW::wave % 5 == 0) {
      %n = %n + 2;
      %tail = 1;     // boss waves release TAIL-first: Prometheus + elites lead
      messageAll(0, "*** WAVE " @ $MMW::wave @ ": COMMAND SIGNATURE DETECTED ***");
   }
   else
      messageAll(0, "WAVE " @ $MMW::wave @ " INBOUND");
   %got = BotBrain::Release(1, %n, %tail);
   echo("[MECHWAVE] wave " @ $MMW::wave @ " released " @ %got @ "/" @ %n);
   schedule("MechWaves::watch();", 8);
}

// census both sides via the usual MissionCleanup sweep
function MechWaves::census(%obj)
{
   %data = GameBase::getDataName(%obj);
   if (String::getSubStr(%data, 0, 4) != "Herc")
      return;
   if (Player::isDead(%obj))
      return;
   %team = GameBase::getTeam(%obj);
   if (%team == 1)
      $MMW::alive1++;
   else
      $MMW::alive0++;
}

function MechWaves::watch()
{
   if ($MMW::over == 1)
      return;
   $MMW::alive0 = 0;
   $MMW::alive1 = 0;
   Group::iterateRecursive(MissionCleanup, "MechWaves::census");

   if ($MMW::alive0 == 0) {
      $MMW::over = 1;
      messageAll(0, "THE OUTPOST HAS FALLEN. Cybrid incursion succeeded at wave " @ $MMW::wave @ ".");
      echo("[MECHWAVE] DEFEAT at wave " @ $MMW::wave);
      $timeLimitReached = true;
      $timeReached = 1;
      schedule("Server::nextMission();", 12);
      return;
   }

   if ($MMW::alive1 == 0) {
      echo("[MECHWAVE] wave " @ $MMW::wave @ " cleared");
      MechProgress::creditWave($MMW::wave);
      if ($MMW::wave >= $MM::MaxWave) {
         $MMW::over = 1;
         messageAll(0, "INCURSION REPELLED. " @ $MM::MaxWave @ " waves survived -- the outpost holds.");
         echo("[MECHWAVE] VICTORY");
         $timeLimitReached = true;
         $timeReached = 1;
         schedule("Server::nextMission();", 15);
         return;
      }
      messageAll(0, "Wave " @ $MMW::wave @ " destroyed. Salvage phase: " @ $MM::SalvageSecs @ " seconds.");
      schedule("MechWaves::launch();", $MM::SalvageSecs);
      return;
   }

   schedule("MechWaves::watch();", 5);
}

echo("[MECH] MechWaves loaded.");
