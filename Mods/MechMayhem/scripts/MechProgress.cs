//----------------------------------------------------------------------------
// Mech Mayhem -- pilot progression (Stage 6).
// Server-side flat files, RPG-precedent export(): one config\mm_pilots\<key>.cs
// per pilot, loaded at connect, exported on disconnect / round end / autosave.
// No web service; Kronos is one community server.
//
//   currency  Salvage = authentic Combat Value of what you kill (underdog
//             bonus built in: an Executioner is worth 3600 from any seat)
//   ladder    the mined Starsiege Tech_Level column: T1 chassis are free,
//             higher tech unlocks with salvage at CV x 2
//   picks     remoteEval(2048, MMPick, "<Chassis>");  from the client console
//             remoteEval(2048, MMBuy,  "<Chassis>");
//             remoteEval(2048, MMStats);
//----------------------------------------------------------------------------

$MM::PilotDir = "config\\mm_pilots\\";
$MM::FreeTech = 1;            // tech level playable from the first boot
$MM::UnlockCostMult = 2;      // unlock price = CV x this
$MM::AutosaveSecs = 60;

// filename-safe key from the client name: alnum only, lowercased-ish (the
// dialect has no tolower; alnum filter is enough for uniqueness in practice)
function MechProgress::key(%cl)
{
   %name = Client::getName(%cl);
   %n = String::len(%name);
   %key = "";
   for (%i = 0; %i < %n; %i++) {
      %c = String::getSubStr(%name, %i, 1);
      if ((%c >= "a" && %c <= "z") || (%c >= "A" && %c <= "Z") || (%c >= "0" && %c <= "9"))
         %key = %key @ %c;
   }
   if (%key == "")
      %key = "pilot" @ %cl;
   return %key;
}

function MechProgress::file(%key)
{
   return $MM::PilotDir @ %key @ ".cs";
}

function MechProgress::load(%cl)
{
   %key = MechProgress::key(%cl);
   %cl.mmKey = %key;
   %f = MechProgress::file(%key);
   if (isFile(%f))
      exec(%f);
   if ($MMP::salvage[%key] == "")
      $MMP::salvage[%key] = 0;
   Client::sendMessage(%cl, 1, "Pilot record " @ %key @ ": " @ $MMP::salvage[%key]
                       @ " salvage, " @ $MMP::kills[%key] @ " kills, best wave "
                       @ $MMP::highWave[%key]);
   echo("[MECHPILOT] loaded " @ %key @ " salvage=" @ $MMP::salvage[%key]);
}

function MechProgress::save(%cl)
{
   if (%cl.mmKey == "")
      return;
   %f = MechProgress::file(%cl.mmKey);
   // export() takes a VARIABLE PATTERN; array vars flatten with the key in
   // the spelling, so a per-key wildcard keeps each pilot's file self-contained
   export("MMP::*" @ %cl.mmKey @ "*", %f, false);
   echo("[MECHPILOT] saved " @ %cl.mmKey);
}

function MechProgress::saveAll()
{
   for (%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
      MechProgress::save(%cl);
   schedule("MechProgress::saveAll();", $MM::AutosaveSecs);
}

//--- earning ------------------------------------------------------------------

function MechProgress::creditKill(%killerCl, %victimChassis)
{
   if (%killerCl <= 0 || %killerCl.mmKey == "")
      return;
   %cv = $MM::CV[%victimChassis];
   if (%cv == "")
      %cv = 500;
   %key = %killerCl.mmKey;
   $MMP::salvage[%key] = $MMP::salvage[%key] + %cv;
   $MMP::kills[%key] = $MMP::kills[%key] + 1;
   Client::sendMessage(%killerCl, 1, "+" @ %cv @ " salvage (" @ $MMP::salvage[%key] @ " total)");
}

function MechProgress::creditWave(%wave)
{
   for (%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) {
      if (%cl.mmKey != "" && %wave > $MMP::highWave[%cl.mmKey])
         $MMP::highWave[%cl.mmKey] = %wave;
   }
}

//--- unlock ladder ------------------------------------------------------------

function MechProgress::canUse(%cl, %chassis)
{
   %tech = $MM::Tech[%chassis];
   if (%tech == "")
      return false;         // unknown chassis
   if (%tech <= $MM::FreeTech)
      return true;
   if (%tech >= 100)
      return false;         // Prometheus-class: never player-usable
   if (%cl.mmKey != "" && $MMP::unlocked[%cl.mmKey, %chassis] == 1)
      return true;
   return false;
}

function remoteMMBuy(%cl, %chassis)
{
   if (%cl.mmKey == "")
      return;
   if (MechProgress::canUse(%cl, %chassis)) {
      Client::sendMessage(%cl, 1, %chassis @ " is already yours.");
      return;
   }
   %tech = $MM::Tech[%chassis];
   if (%tech == "" || %tech >= 100) {
      Client::sendMessage(%cl, 1, "That chassis cannot be piloted.");
      return;
   }
   %cost = $MM::CV[%chassis] * $MM::UnlockCostMult;
   %key = %cl.mmKey;
   if ($MMP::salvage[%key] < %cost) {
      Client::sendMessage(%cl, 1, %chassis @ " costs " @ %cost @ " salvage; you have " @ $MMP::salvage[%key] @ ".");
      return;
   }
   $MMP::salvage[%key] = $MMP::salvage[%key] - %cost;
   $MMP::unlocked[%key, %chassis] = 1;
   MechProgress::save(%cl);
   Client::sendMessage(%cl, 1, %chassis @ " UNLOCKED (" @ $MMP::salvage[%key] @ " salvage left).");
   echo("[MECHPILOT] " @ %key @ " unlocked " @ %chassis);
}

function remoteMMPick(%cl, %chassis)
{
   if (!MechProgress::canUse(%cl, %chassis)) {
      Client::sendMessage(%cl, 1, "You have not unlocked " @ %chassis
                          @ " (T" @ $MM::Tech[%chassis] @ "). MMBuy it with salvage.");
      return;
   }
   %cl.mmChassis = %chassis;
   Client::sendMessage(%cl, 1, "Next drop: " @ %chassis @ ".");
   echo("[MECHPILOT] " @ %cl.mmKey @ " picked " @ %chassis);
}

function remoteMMStats(%cl)
{
   %key = %cl.mmKey;
   Client::sendMessage(%cl, 1, "Pilot " @ %key @ ": " @ $MMP::salvage[%key]
                       @ " salvage, " @ $MMP::kills[%key] @ " kills, "
                       @ $MMP::deaths[%key] @ " deaths, best wave " @ $MMP::highWave[%key]);
}

echo("[MECH] MechProgress loaded.");
