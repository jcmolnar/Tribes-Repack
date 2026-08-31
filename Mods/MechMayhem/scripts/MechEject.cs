//----------------------------------------------------------------------------
// Mech Mayhem -- LAST STAND ejection system.
//
// Perk of Harabec's Apocalypse (HercHaapoc, the T6 hero chassis): when the
// hull takes the killing blow, the pilot punches out THROUGH the canopy and
// keeps fighting on foot in a MechPilot suit (MechPilot.cs kit).
//
// Sequencing is the whole trick, and it runs entirely inside the fatal
// MechDamage::apply call, BEFORE the killing damage lands:
//   1. MechTickets::charge(mech)  -- account the loss while the pilot still
//      owns the hull: real name in the message, full salvage to the killer,
//      CV drained, mmCounted set so the tick sweep cannot double-charge.
//   2. spawnPlayer(MechPilot) above the hull roof + Client::setOwnedObject/
//      setControlObject -- the client never dies, so no death GUI, no respawn
//      wait, no $MMP death on the pilot record. That is the perk.
//   3. Kill the abandoned shell for real (setDamageLevel to max) and run
//      MechDeath::spectacle on it -- the FULL native reactor death: staged
//      pops, blowUp fireball + debris, area damage, persistent wreck.
//   4. Manual scoring/obit. Client::onKilled is deliberately NOT called: it
//      guiLocks the victim client (death screen), and our victim is alive.
//      The killer gets the mech kill; killing the escaped pilot afterwards
//      is a second, ordinary kill plus a salvage bounty (MechEject::bounty,
//      hooked from Game::clientKilled).
//
// Not covered by design: bots never eject (a BotBrain slot stays bound to its
// mech object), and a mech killed by another mech's reactor splash dies
// outright (that path writes damage directly, not through onDamage).
//----------------------------------------------------------------------------

$MM::EjectVel = 42;        // vertical eject velocity, u/s (pilot mass is 9)
$MM::PilotBounty = 400;    // salvage for confirming the escaped-pilot kill
$MM::EjectTech = 5;        // ejection seats are a high-tech human feature

// Perk roster (Joe: "all human-faction chassis at T5+"): computed from the
// chassis registry so a stat regen keeps it honest. Cybrids never eject --
// they ARE the mech: that excludes the Cy and Mg liveries, plus Pl, which
// the mod's own bot roster fields on the Cybrid side (pl_exec = CYB_Elite).
// Bosses and the never-pilotable (tech >= 100) are out regardless.
// Currently resolves to: HercHaapoc (T6), HercKnapoc (T5), HercKngorg (T6).
function MechEject::buildRoster()
{
   %n = 0;
   for (%i = 0; %i < $MM::RosterCount; %i++) {
      %db = $MM::Roster[%i];
      %fac = String::getSubStr(%db, 4, 2);
      if (%fac == "Cy" || %fac == "Mg" || %fac == "Pl")
         continue;
      if ($MM::Class[%db] == "boss")
         continue;
      %tech = $MM::Tech[%db];
      if (%tech == "" || %tech < $MM::EjectTech || %tech >= 100)
         continue;
      $MM::EjectChassis[%db] = 1;
      %n++;
   }
   echo("[EJECT] perk roster: " @ %n @ " human-faction T" @ $MM::EjectTech @ "+ chassis carry ejection seats.");
}
MechEject::buildRoster();

// ★bot reps HAVE owner-client ids (measured: 2049), so getClient(%pl) > 0
// does NOT mean human. Client::getFirst/getNext enumerates humans only --
// membership there is the humanity test.★
function MechEject::isHumanClient(%cl)
{
   if (%cl <= 0)
      return 0;
   for (%c = Client::getFirst(); %c != -1; %c = Client::getNext(%c))
      if (%c == %cl)
         return 1;
   return 0;
}

function MechEject::canEject(%pl)
{
   %base = MechHeat::baseChassis(Player::getArmor(%pl));
   if ($MM::EjectChassis[%base] != 1)
      return 0;
   // humans only -- a BotBrain slot stays bound to its mech object and would
   // orphan the spawned pilot. $MM::EjectTest=1 lets the harness exercise the
   // spawn/kit/shell path with a bot victim (no client transfer).
   if (!MechEject::isHumanClient(Player::getClient(%pl)) && $MM::EjectTest != 1)
      return 0;
   return 1;
}

// returns 1 if the ejection ran; 0 falls back to the normal death path
function MechEject::fire(%mech, %cl, %shooterCl, %type)
{
   %armor = Player::getArmor(%mech);
   %base = MechHeat::baseChassis(%armor);
   %pos = GameBase::getPosition(%mech);
   %rot = GameBase::getRotation(%mech);

   // hatch position: just clear of the hull roof
   %x = getWord(%pos, 0);
   %y = getWord(%pos, 1);
   %z = getWord(%pos, 2) + %armor.boxNormalHeight + 1.5;
   %pilot = spawnPlayer(MechPilot, %x @ " " @ %y @ " " @ %z, %rot);
   if (%pilot == -1 || %pilot == "") {
      echo("[EJECT] spawnPlayer FAILED -- normal death for " @ %mech);
      return 0;
   }

   // account the mech loss while the corpse-to-be still resolves its pilot
   MechTickets::charge(%mech);

   // bot reps carry client ids too -- only a HUMAN client gets the transfer
   %human = MechEject::isHumanClient(%cl);
   GameBase::setTeam(%pilot, GameBase::getTeam(%mech));
   if (%human) {
      Client::setOwnedObject(%cl, %pilot);
      Client::setControlObject(%cl, %pilot);
      %cl.mmOnFoot = 1;
      // Out of the hull: drop the herc turn-rate cap or the pilot walks around
      // turning like a 90-ton Executioner (the engine treats 0 as uncapped).
      MechMayhem::pushTurnCap(%cl, "");
   }
   MechEject::grantKit(%pilot, %cl, %human);
   Player::applyImpulse(%pilot, "0 0 " @ (9.0 * $MM::EjectVel));

   // the abandoned shell dies for real: full native reactor death + wreck
   GameBase::setDamageLevel(%mech, %armor.maxDamage);
   MechDeath::spectacle(%mech);

   // manual scoring + obit (see header for why not Client::onKilled)
   %vname = "A";
   if (%cl > 0)
      %vname = Client::getName(%cl) @ "'s";
   if (%human && %shooterCl > 0 && %shooterCl != %cl
       && !($teamplay && Client::getTeam(%shooterCl) == Client::getTeam(%cl))) {
      %shooterCl.scoreKills++;
      %shooterCl.score++;
      Game::refreshClientScore(%shooterCl);
      Client::sendMessage(%shooterCl, 1, "Mech destroyed -- but the pilot punched out. Hunt them down to confirm the kill.");
   }
   messageAll(0, %vname @ " " @ %base @ " is destroyed -- the pilot EJECTS!");
   if (%human) {
      Client::sendMessage(%cl, 1, "REACTOR CRITICAL -- EJECT EJECT EJECT");
      bottomPrint(%cl, "<jc><f2>EJECTED<f1> -- last stand: the CV is already paid. Fight on foot and make them bleed.", 6);
   }
   echo("[EJECT] " @ %base @ " cl=" @ %cl @ " pilot=" @ %pilot @ " at " @ %pos);
   return 1;
}

function MechEject::grantKit(%pilot, %cl, %human)
{
   // findPlayerObject accepts the player object as the client-id arg (the
   // same contract grantLoadout relies on for conscripted bots)
   %id = %cl;
   if (%human != 1)
      %id = %pilot;
   Player::setItemCount(%id, PilotSidearm, 1);
   Player::setItemCount(%id, PilotLance, 1);
   Player::setItemCount(%id, PilotCharge, 1);
   Player::setItemCount(%id, EnergyPack, 1);
   Player::setItemCount(%id, RepairKit, 1);
   Player::setItemCount(%id, Grenade, 3);
   Player::useItem(%pilot, EnergyPack);
   Player::useItem(%pilot, PilotLance);
}

// hooked from Game::clientKilled (MechGame.cs): the escaped pilot was hunted
// down -- pay the killer the confirmation bounty. The pilot's death itself
// runs the ordinary stock path (scores, obit, respawn into a fresh mech).
function MechEject::bounty(%victimCl, %killerCl)
{
   if (%victimCl == -1 || %victimCl.mmOnFoot != 1)
      return;
   %victimCl.mmOnFoot = "";
   if (%killerCl <= 0 || %killerCl == %victimCl)
      return;
   if ($teamplay && Client::getTeam(%killerCl) == Client::getTeam(%victimCl))
      return;
   messageAll(0, Client::getName(%victimCl) @ "'s pilot has been silenced.");
   if (%killerCl.mmKey == "" || %killerCl.mmAuth != 1)
      return;
   %key = %killerCl.mmKey;
   $MMP::salvage[%key] = $MMP::salvage[%key] + $MM::PilotBounty;
   Client::sendMessage(%killerCl, 1, "PILOT KILL CONFIRMED: +" @ $MM::PilotBounty @ " salvage (" @ $MMP::salvage[%key] @ " total)");
}

echo("[MECH] MechEject loaded (Last Stand ejection armed).");
