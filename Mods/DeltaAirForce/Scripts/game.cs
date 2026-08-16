exec("comchat.cs");
$SensorNetworkEnabled = true;

$GuiModePlay = 1;
$GuiModeCommand = 2;
$GuiModeVictory = 3;
$GuiModeInventory = 4;
$GuiModeObjectives = 5;
$GuiModeLobby = 6;
$ExportNum = 0;

$fa_armor = "";
$fa_pack = "";

//  Global Variables

//$TournyMode
//$TeamJetAvail[%team]
//$TeamChopperAvail[%team]
//$TeamTankAvail[%team]
//$TeamJetAvail[%team]
//$TeamJetAvail[%team]
//$TeamOwnedJet[%team]
//$TeamOwnedChopper[%team]
//$TeamOwnedTank[%team]


//---------------------------------------------------------------------------------
//    Fuel pack constants
//---------------------------------------------------------------------------------

$FuelPackBlowMethod = 1;
$FuelPackBlowRadius = 1.5;
$FuelPackBlowHeight = 3.0;


//---------------------------------------------------------------------------------
// Disable/enable globalchat.
//---------------------------------------------------------------------------------
$NoTalk = false;

//---------------------------------------------------------------------------------
// Displays the name of the person whom deployed the sandbag, when it is shot.
//---------------------------------------------------------------------------------
$SandBagOwner = false;


//---------------------------------------------------------------------------------
// Should we nuke through the prefab shapes? Turn to false for missions that are D&D
//---------------------------------------------------------------------------------
$NukeThroughPrefabs = true;

//---------------------------------------------------------------------------------
// Shell stuff: what we want outputted. Most are set to true at first.
//---------------------------------------------------------------------------------
$STEAMTALK = true;
$STALK = true;
$SKILL = true;
$SSPAWN = true;
$STKILL = true;
$STHROW = false;
$SDEPLOY = false;
function SAllOff()
{
	$STEAMTALK = false;
	$STALK = false;
	$SKILL = false;
	$SSPAWN = false;
	$STKILL = false;
	$STHROW = false;
	$SDEPLOY = false;
}

function SAllOn()
{
	$STEAMTALK = true;
	$STALK = true;
	$SKILL = true;
	$SSPAWN = true;
	$STKILL = true;
	$STHROW = true;
	$SDEPLOY = true;
}

function SDefault()
{
	$STEAMTALK = true;
	$STALK = true;
	$SKILL = true;
	$SSPAWN = true;
	$STKILL = true;
	$STHROW = false;
	$SDEPLOY = false;
}

//---------------------------------------------------------------------------------
// TKcontrol variables. Set to 0 if you wish to deactivate them. If activated, when 
// the abuser reaches the specified number, they will be delt with as listed.
//---------------------------------------------------------------------------------
$TKWARNING = 3;
$TKKILL = 5;
$TKKICK = 7;


//---------------------------------------------------------------------------------
// If this is true, then points arnt given for vehicle kills, and vehicle kill 
// messages are not displayed (advised to keap this turned off...)
//---------------------------------------------------------------------------------
$NoVehicleTk = true;

//---------------------------------------------------------------------------------
// Energy each team is given at beginning of game
//---------------------------------------------------------------------------------
$DefaultTeamEnergy = "Infinite";

//---------------------------------------------------------------------------------
// Team Energy variables
//---------------------------------------------------------------------------------
$TeamEnergy[-1] = $DefaultTeamEnergy; 
$TeamEnergy[0]  = $DefaultTeamEnergy; 
$TeamEnergy[1]  = $DefaultTeamEnergy; 
$TeamEnergy[2]  = $DefaultTeamEnergy; 
$TeamEnergy[3]  = $DefaultTeamEnergy; 
$TeamEnergy[4]  = $DefaultTeamEnergy; 
$TeamEnergy[5]  = $DefaultTeamEnergy; 
$TeamEnergy[6]  = $DefaultTeamEnergy; 				
$TeamEnergy[7]  = $DefaultTeamEnergy; 

//---------------------------------------------------------------------------------
// Time in sec player must wait before he can throw a Grenade or Mine after leaving
//	a station.
//---------------------------------------------------------------------------------
$WaitThrowTime = 2;

//---------------------------------------------------------------------------------
// If 1 then Team Spending Ignored -- Team Energy is set to $MaxTeamEnergy every
// 	$secTeamEnergy.
//---------------------------------------------------------------------------------
$TeamEnergyCheat = 0;

//---------------------------------------------------------------------------------
// MAX amount team energy can reach
//---------------------------------------------------------------------------------
$MaxTeamEnergy = 700000;

//---------------------------------------------------------------------------------
//  Time player has to put flag in flagstand before it gets returned to its last
//  location. 
//---------------------------------------------------------------------------------
$flagToStandTime = 180;	  

//---------------------------------------------------------------------------------
// Amount to inc team energy every ($secTeamEnergy) seconds
//---------------------------------------------------------------------------------
$incTeamEnergy = 700;

//---------------------------------------------------------------------------------
// (Rate is sec's) Set how often TeamEnergy is incremented
//---------------------------------------------------------------------------------
$secTeamEnergy = 30;

//---------------------------------------------------------------------------------
// (Rate is sec's) Items respwan
//---------------------------------------------------------------------------------
$ItemRespawnTime = 30;

//---------------------------------------------------------------------------------
//Amount of Energy remote stations start out with
//---------------------------------------------------------------------------------
$RemoteAmmoEnergy = 2500; 
$RemoteInvEnergy = 10000;

//---------------------------------------------------------------------------------
// TEAM ENERGY -  Warn team when teammate has spent x amount - Warn team that 
//				  energy level is low when it reaches x amount 
//---------------------------------------------------------------------------------
$TeammateSpending = -4000;  //Set = to 0 if don't want the warning message
$WarnEnergyLow = 4000;	    //Set = to 0 if don't want the warning message

//---------------------------------------------------------------------------------
// Amount added to TeamEnergy when a player joins a team
//---------------------------------------------------------------------------------
$InitialPlayerEnergy = 5000;

//---------------------------------------------------------------------------------
// REMOTE TURRET
//---------------------------------------------------------------------------------
$MaxNumTurretsInBox = 2;     //Number of remote turrets allowed in the area
$TurretBoxMaxLength = 50;    //Define Max Length of the area
$TurretBoxMaxWidth =  50;    //Define Max Width of the area
$TurretBoxMaxHeight = 25;    //Define Max Height of the area

$TurretBoxMinLength = 10;	  //Define Min Length from another turret
$TurretBoxMinWidth =  10;	  //Define Min Width from another turret
$TurretBoxMinHeight = 10;    //Define Min Height from another turret

//---------------------------------------------------------------------------------
//	Object Types	
//---------------------------------------------------------------------------------
$SimTerrainObjectType    = 1 << 1;
$SimInteriorObjectType   = 1 << 2;
$SimPlayerObjectType     = 1 << 7;

$MineObjectType		    = 1 << 26;	
$MoveableObjectType	    = 1 << 22;
$VehicleObjectType	 	 = 1 << 29;  
$StaticObjectType			 = 1 << 23;	   
$ItemObjectType			 = 1 << 21;	  

//---------------------------------------------------------------------------------
// CHEATS
//---------------------------------------------------------------------------------
$ServerCheats = 0;
$TestCheats = 0;

//---------------------------------------------------------------------------------
//Respawn automatically after X sec's -  If 0..no respawn
//---------------------------------------------------------------------------------
$AutoRespawn = 0;

//---------------------------------------------------------------------------------
// Player death messages - %1 = killer's name, %2 = victim's name
//       %3 = killer's gender pronoun (his/her), %4 = victim's gender pronoun
//---------------------------------------------------------------------------------
$deathMsg[$LandingDamageType, 0]      = "%2 falls to %3 death.";
$deathMsg[$LandingDamageType, 1]      = "%2 forgot to tie %3 bungie cord.";
$deathMsg[$LandingDamageType, 2]      = "%2 bites the dust in a forceful manner.";
$deathMsg[$LandingDamageType, 3]      = "%2 fall down go boom.";
$deathMsg[$ImpactDamageType, 0]      = "%1 makes quite an impact on %2.";
$deathMsg[$ImpactDamageType, 1]      = "%2 becomes the victim of a drive-by from %1.";
$deathMsg[$ImpactDamageType, 2]      = "%2 leaves a nasty dent in %1's fender.";
$deathMsg[$ImpactDamageType, 3]      = "%1 says, 'Hey %2, you scratched my paint job!'";
$deathMsg[$BulletDamageType, 0]      = "%1 ventilates %2.";
$deathMsg[$BulletDamageType, 1]      = "%1 gives %2 an overdose of lead.";
$deathMsg[$BulletDamageType, 2]      = "%1 fills %2 full of holes.";
$deathMsg[$BulletDamageType, 3]      = "%1 guns down %2.";
$deathMsg[$EnergyDamageType, 0]      = "%2 dies of turret trauma.";
$deathMsg[$EnergyDamageType, 1]      = "%2 is chewed to pieces by a turret.";
$deathMsg[$EnergyDamageType, 2]      = "%2 walks into a stream of turret fire.";
$deathMsg[$EnergyDamageType, 3]      = "%2 ends up on the wrong side of a turret.";
$deathMsg[$PlasmaDamageType, 0]      = "%1 makes %2 extra crispy.";
$deathMsg[$PlasmaDamageType, 1]      = "%1 lights %2's fire.";
$deathMsg[$PlasmaDamageType, 2]      = "%1 makes %2 burn with anger.";
$deathMsg[$PlasmaDamageType, 3]      = "%1 teaches %2 a lesson in fire safety.";
$deathMsg[$ExplosionDamageType, 0]   = "%2 catches a rocket launched by %1.";
$deathMsg[$ExplosionDamageType, 1]   = "%1 blasts %2 with a well-placed rocket.";
$deathMsg[$ExplosionDamageType, 2]   = "%1's warhead caught %2 by surprise.";
$deathMsg[$ExplosionDamageType, 3]   = "%2 falls victim to %1's rocket launcher.";
$deathMsg[$ShrapnelDamageType, 0]    = "%1 blows %2 up real good.";
$deathMsg[$ShrapnelDamageType, 1]    = "%2 gets a taste of %1's explosive temper.";
$deathMsg[$ShrapnelDamageType, 2]    = "%1 gives %2 a fatal concussion.";
$deathMsg[$ShrapnelDamageType, 3]    = "%2 never saw it coming from %1.";
$deathMsg[$LaserDamageType, 0]       = "%1 adds %2 to %3 list of sniper victims.";
$deathMsg[$LaserDamageType, 1]       = "%1 fells %2 with a sniper shot.";
$deathMsg[$LaserDamageType, 2]       = "%2 becomes a victim of %1's sniper rifle.";
$deathMsg[$LaserDamageType, 3]       = "%2 stayed in %1's crosshairs for too long.";
$deathMsg[$MortarDamageType, 0]      = "%1 blew %2 into oblivion.";
$deathMsg[$MortarDamageType, 1]      = "%2 didn't see that last artillery shell from %1.";
$deathMsg[$MortarDamageType, 2]      = "%1 inflicts a mortal artillery wound on %2.";
$deathMsg[$MortarDamageType, 3]      = "%1's artillery takes out %2.";
$deathMsg[$BlasterDamageType, 0]     = "%2 gets a blast out of %1.";
$deathMsg[$BlasterDamageType, 1]     = "%2 succumbs to %1's rain of blaster fire.";
$deathMsg[$BlasterDamageType, 2]     = "%1's puny blaster shows %2 a new world of pain.";
$deathMsg[$BlasterDamageType, 3]     = "%2 meets %1's master blaster.";
$deathMsg[$ElectricityDamageType, 0] = "%2 gets zapped with %1's ELF gun.";
$deathMsg[$ElectricityDamageType, 1] = "%1 gives %2 a nasty jolt.";
$deathMsg[$ElectricityDamageType, 2] = "%2 gets a real shock out of meeting %1.";
$deathMsg[$ElectricityDamageType, 3] = "%1 short-circuits %2's systems.";
$deathMsg[$CrushDamageType, 0]		 = "%2 didn't stay away from the moving parts.";
$deathMsg[$CrushDamageType, 1]		 = "%2 is crushed.";
$deathMsg[$CrushDamageType, 2]		 = "%2 gets smushed flat.";
$deathMsg[$CrushDamageType, 3]		 = "%2 gets caught in the machinery.";
$deathMsg[$DebrisDamageType, 0]		 = "%2 is a victim among the wreckage.";
$deathMsg[$DebrisDamageType, 1]		 = "%2 is killed by debris.";
$deathMsg[$DebrisDamageType, 2]		 = "%2 becomes a victim of collateral damage.";
$deathMsg[$DebrisDamageType, 3]		 = "%2 got too close to the exploding stuff.";
$deathMsg[$MissileDamageType, 0]	    = "%2 takes a rocket up the keister.";
$deathMsg[$MissileDamageType, 1]	    = "%2 gets shot down.";
$deathMsg[$MissileDamageType, 2]	    = "%2 gets real friendly with a rocket.";
$deathMsg[$MissileDamageType, 3]	    = "%2 feels the burn from a warhead.";
$deathMsg[$MineDamageType, 0]	       = "%1 blows %2 up real good.";
$deathMsg[$MineDamageType, 1]	       = "%2 gets a taste of %1's explosive temper.";
$deathMsg[$MineDamageType, 2]	       = "%1 gives %2 a fatal concussion.";
$deathMsg[$MineDamageType, 3]	       = "%2 never saw it coming from %1.";
// DELTA FORCE
$deathMsg[$ChargeDamageType, 0]      = "%2 didn't get away fast enough from the plastic explosive.";
$deathMsg[$ChargeDamageType, 1]      = "%2 was blown into small pieces.";
$deathMsg[$ChargeDamageType, 2]      = "%2 was torn up by a shaped charge.";
$deathMsg[$ChargeDamageType, 3]      = "%2 tried to throw his body in front of the blast.";
$deathMsg[$AirstrikeDamageType, 0]      = "%1 called down the wrath of God upon %2.";
$deathMsg[$AirstrikeDamageType, 1]      = "%2 was burned to a cinder by the inferno called down by %1.";
$deathMsg[$AirstrikeDamageType, 2]      = "%1 loves the smell of napalm in the morning, but %2 doesn't.";
$deathMsg[$AirstrikeDamageType, 3]      = "%2 lost at a game of Catch The Bomb initiated by %1.";
$deathMsg[$BleedDamageType, 0]      = "%2 bleeds to death.";
$deathMsg[$BleedDamageType, 1]      = "%2 dies from bloodloss.";
$deathMsg[$BleedDamageType, 2]      = "%2's fluids leak all over the ground.";
$deathMsg[$BleedDamageType, 3]      = "%2 didn't cork that leak in time.";
$deathMsg[$GrenadeDamageType, 0]      = "%2 caught %1's grenade.";
$deathMsg[$GrenadeDamageType, 1]      = "%1's blast caught %2.";
$deathMsg[$GrenadeDamageType, 2]      = "%2 is blown into itty bitty pieces by %1.";
$deathMsg[$GrenadeDamageType, 3]      = "%2 forgot to toss the grenade back to %1.";
$deathMsg[$PoisonDamageType, 0]      = "%2 spine melts away.";
$deathMsg[$PoisonDamageType, 1]      = "%2 dies from shock.";
$deathMsg[$PoisonDamageType, 2]      = "%2 loves the smell of Nerve Gas.";
$deathMsg[$PoisonDamageType, 3]      = "%2 forgot his gas mask.";
$deathMsg[$StabDamageType, 0]      = "%2 is stabbed to death.";
$deathMsg[$StabDamageType, 1]      = "%2 was disemboweled.";
$deathMsg[$StabDamageType, 2]      = "%2 had %4 throat slit.";
$deathMsg[$StabDamageType, 3]      = "%2 was turned into a pincushion.";
// END DELTA FORCE

// "you just killed yourself" messages
//   %1 = player name,  %2 = player gender pronoun

$deathMsg[-2,0]						 = "%1 ends it all.";
$deathMsg[-2,1]						 = "%1 takes %2 own life.";
$deathMsg[-2,2]						 = "%1 kills %2 own dumb self.";
$deathMsg[-2,3]						 = "%1 decides to see what the afterlife is like.";

$numDeathMsgs = 4;
//---------------------------------------------------------------------------------

$spawnBuyList[0] = InfantryArmor;
$spawnBuyList[1] = Knife;
$spawnBuyList[2] = SOCOM;
$spawnBuyList[3] = OICW;
$spawnBuyList[4] = RepairKit;

function GameBase::getClassName(%this)
{
	return GameBase::getDataName(%this).className;
}

function remotePlayMode(%clientId)
{
   if(!%clientId.guiLock)
   {
      remoteSCOM(%clientId, -1);
      Client::setGuiMode(%clientId, $GuiModePlay);
   }
}

function remoteCommandMode(%clientId)
{
   // can't switch to command mode while a server menu is up
   if(!%clientId.guiLock)
   {
      remoteSCOM(%clientId, -1);  // force the bandwidth to be full command
		if(%clientId.observerMode != "pregame")
		   checkControlUnmount(%clientId);
		Client::setGuiMode(%clientId, $GuiModeCommand);
   }
}

function remoteInventoryMode(%clientId)
{
   if(!%clientId.guiLock && !Observer::isObserver(%clientId))
   {
      remoteSCOM(%clientId, -1);
      Client::setGuiMode(%clientId, $GuiModeInventory);
   }
}

function remoteObjectivesMode(%clientId)
{
   if(!%clientId.guiLock)
   {
      remoteSCOM(%clientId, -1);
      Client::setGuiMode(%clientId, $GuiModeObjectives);
   }
}

function remoteScoresOn(%clientId)
{
   if(!%clientId.menuMode)
      Game::menuRequest(%clientId);
}

function remoteScoresOff(%clientId)
{
   Client::cancelMenu(%clientId);
}

function remoteToggleCommandMode(%clientId)
{
	if (Client::getGuiMode(%clientId) != $GuiModeCommand)
		remoteCommandMode(%clientId);
	else
		remotePlayMode(%clientId);
}

function remoteToggleInventoryMode(%clientId)
{
	if (Client::getGuiMode(%clientId) != $GuiModeInventory)
		remoteInventoryMode(%clientId);
	else
		remotePlayMode(%clientId);
}

function remoteToggleObjectivesMode(%clientId)
{
	if (Client::getGuiMode(%clientId) != $GuiModeObjectives)
		remoteObjectivesMode(%clientId);
	else
		remotePlayMode(%clientId);
}

function Time::getMinutes(%simTime)
{
   return floor(%simTime / 60);
}

function Time::getSeconds(%simTime)
{
   return %simTime % 60;
}

// DELTAFORCE
function getNearestTeammate(%client) {
	%comrade = -1;
	%comradeDist = 9999.0;
	%clientPos = GameBase::getPosition(%client);
	for(%i = Client::getFirst(); %i != -1; %i = Client::getNext(%i)) {
		if(GameBase::getTeam(%i) == GameBase::getTeam(%client)) {
			%distance = GameBase::getPosition(%i);
			%compDist = Vector::getDistance(%clientPos, %distance);
			if(%compDist < %comradeDist) {
				%comrade = %i;
				%comradeDist = %compDist;
			}
		}
	}

	return %comrade;
}

function getNearestAITeammate(%client) {
	%comrade = -1;
	%comradeDist = 9999.0;
	%clientPos = GameBase::getPosition(%client);
	for(%i = 2049; %i < 2054; %i = %i + 1) {  // HACCCCCKKKKK!!!!
		if(%i != %client) {
			if(GameBase::getTeam(%i) == GameBase::getTeam(%client)) {
				%distance = GameBase::getPosition(%i);
				%compDist = Vector::getDistance(%clientPos, %distance);
				if(%compDist < %comradeDist) {
					%comrade = %i;
					%comradeDist = %compDist;
				}
			}
		}
	}

	return %comrade;
}
// END DELTAFORCE

function Game::pickRandomSpawn(%team)
{
   %group = nameToID("MissionGroup/Teams/team" @ %team @ "/DropPoints/Random");
   %count = Group::objectCount(%group);
   if(!%count)
      return -1;
  	%spawnIdx = floor(getRandom() * (%count - 0.1));
  	%value = %count;
	for(%i = %spawnIdx; %i < %value; %i++) {
		%set = newObject("set",SimSet);
		%obj = Group::getObject(%group, %i);
		if(containerBoxFillSet(%set,$SimPlayerObjectType|$VehicleObjectType,GameBase::getPosition(%obj),2,2,4,0) == 0) {
			deleteObject(%set);
			return %obj;
		}
		if(%i == %count - 1) {
			%i = -1;
			%value = %spawnIdx;
		}
		deleteObject(%set);
	}
   return false;
}

function Game::pickStartSpawn(%team)
{
   %group = nameToID("MissionGroup\\Teams\\team" @ %team @ "\\DropPoints\\Start");
   %count = Group::objectCount(%group);
   if(!%count)
      return -1;

   %spawnIdx = $lastTeamSpawn[%team] + 1;
   if(%spawnIdx >= %count)
      %spawnIdx = 0;
   $lastTeamSpawn[%team] = %spawnIdx;
   return Group::getObject(%group, %spawnIdx);
}

function Game::pickTeamSpawn(%team, %respawn)
{
   if(%respawn)
      return Game::pickRandomSpawn(%team);
   else
   {
      %spawn = Game::pickStartSpawn(%team);
      if(%spawn == -1)
         return Game::pickRandomSpawn(%team);
      return %spawn;
   }
}

function Game::pickObserverSpawn(%client)
{
   %group = nameToID("MissionGroup\\ObserverDropPoints");
   %count = Group::objectCount(%group);
   if(%group == -1 || !%count)
      %group = nameToID("MissionGroup\\Teams\\team" @ Client::getTeam(%client) @ "\\DropPoints\\Random");
   %count = Group::objectCount(%group);
   if(%group == -1 || !%count)
      %group = nameToID("MissionGroup\\Teams\\team0\\DropPoints\\Random");
   %count = Group::objectCount(%group);
   if(%group == -1 || !%count)
      return -1;
   %spawnIdx = %client.lastObserverSpawn + 1;
   if(%spawnIdx >= %count)
      %spawnIdx = 0;
   %client.lastObserverSpawn = %spawnIdx;
	return Group::getObject(%group, %spawnIdx);
}

function UpdateClientTimes(%time)
{
   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
      remoteEval(%cl, "setTime", -%time);
}

function Game::notifyMatchStart(%time)
{
   messageAll(0, "Match starts in " @ %time @ " seconds.");
   UpdateClientTimes(%time);
}

function Game::startMatch()
{
   $matchStarted = true;
   $missionStartTime = getSimTime();
   messageAll(0, "Match started.");
	Game::resetScores();	

   %numTeams = getNumTeams();
   for(%i = 0; %i < %numTeams; %i = %i + 1) {
		if($TeamEnergy[%i] != "Infinite")
			schedule("replenishTeamEnergy(" @ %i @ ");", $secTeamEnergy);
	}

   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
   {
		if(%cl.observerMode == "pregame")
      {
         %cl.observerMode = "";
         Client::setControlObject(%cl, Client::getOwnedObject(%cl));
      }
   	Game::refreshClientScore(%cl);
	}
   Game::checkTimeLimit();
}

function Game::pickPlayerSpawn(%clientId, %respawn)
{
   return Game::pickTeamSpawn(Client::getTeam(%clientId), %respawn);
}

function MissionIntro::init(%clientId)
{
    centerprint(%clientId, "<jc><f0>Mission: <f1>" @ $missionName @ "<f0>Mission Type: <f1>" @ $Game::missionType @ "\n<f0>Press <f1>'O'<f0> for specific objectives.", 5);
}

function Game::playerSpawn(%clientId, %respawn)
{
   if(!$ghosting)
      return false;
	%clientid.inVehicle = false;
	Client::clearItemShopping(%clientId);
	  %spawnMarker = Game::pickPlayerSpawn(%clientId, %respawn);
   if(!%respawn)
   {
      // initial drop
      MissionIntro::init(%clientId);
   }
	if(%spawnMarker) {   
		%clientId.guiLock = "";
	 	%clientId.dead = "";
	   if(%spawnMarker == -1)
	   {
	      %spawnPos = "0 0 300";
	      %spawnRot = "0 0 0";
	   }
	   else
	   {
	      %spawnPos = GameBase::getPosition(%spawnMarker);
	      %spawnRot = GameBase::getRotation(%spawnMarker);
	   }

		if(!String::ICompare(Client::getGender(%clientId), "Male"))
		{
	      if(%clientId == $Hunted)
		  {
			%armor = "larmor";
		  } else
		  {
			 %armor = "iarmor";
		  }
	   } else
	   {
		  if(%clientId == $Hunted)
		  {
			%armor = "lfemale";
		  } else
		  {
			 %armor = "ifemale";
		  }
	   }
		
       // DELTAFORCE
	   %pl = spawnPlayer(%armor, %spawnPos, %spawnRot);
	   if($SSPAWN) {
		   echo("SPAWN: cl:" @ %clientId @ " pl:" @ %pl @ " marker:" @ %spawnMarker @ " armor:" @ %armor);
		
		}
	   if(%pl != -1)
	   {
	      GameBase::setTeam(%pl, Client::getTeam(%clientId));
	      Client::setOwnedObject(%clientId, %pl);
		  // DELTAFORCE
		  if($Game::missionType == "Assassination") {
			  if( $Hunted == %clientId )
				  Player::mountItem(%pl, Flag, 2);
		  }
		  // END DELTAFORCE
	      Game::playerSpawned(%pl, %clientId, %armor, %respawn);
	      
	      if($matchStarted)
	         Client::setControlObject(%clientId, %pl);
	      else
	      {
	         %clientId.observerMode = "pregame";
	         Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
	         Observer::setOrbitObject(%clientId, %pl, 3, 3, 3);

	      }
	   }
      return true;
	}
	else {
		Client::sendMessage(%clientId,0,"Sorry No Respawn Positions Are Empty - Try again later ");
      return false;
	}
}

function Game::playerSpawned(%pl, %clientId, %armor)
{						  
	%clientId.spawn= 1;
	%max = getNumItems();
   for(%i = 0; (%item = $spawnBuyList[%i]) != ""; %i++)
   {
	   // DELTAFORCE
	   if($Game::missionType == "Assassination") {
		   if($Hunted == %clientId) {
			   if (%i == 2) %i++;
		   }
	   }
	   // END DELTAFORCE
		buyItem(%clientId,%item);	
		if(%item.className == Weapon) 
			%clientId.spawnWeapon = %item;
	}
	%clientId.spawn= "";
	$Reloading[%clientId] = 0;
	%clientid.inVehicle = false;
	if(%clientId.spawnWeapon != "") {
		Player::useItem(%pl,%clientId.spawnWeapon);	
   	%clientId.spawnWeapon="";
	}
} 

function Game::autoRespawn(%client)
{
	if(%client.dead == 1)
		Game::playerSpawn(%client, "true");
}

function onServerGhostAlwaysDone()
{
}

function Game::initialMissionDrop(%clientId)
{
	
	ClearComm(%clientId, true);
	ClearFromComm(%clientID, true);

	Client::setGuiMode(%clientId, $GuiModePlay);

   if($Server::TourneyMode)
      GameBase::setTeam(%clientId, -1);
   else
   {
      if(%clientId.observerMode == "observerFly" || %clientId.observerMode == "observerOrbit")
      {
	      %clientId.observerMode = "observerOrbit";
	      %clientId.guiLock = "";
         Observer::jump(%clientId);
         return;
      }
      %numTeams = getNumTeams();
      %curTeam = Client::getTeam(%clientId);

      if(%curTeam >= %numTeams || (%curTeam == -1 && (%numTeams < 2 || $Server::AutoAssignTeams)) )
         Game::assignClientTeam(%clientId);
   }    
	Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
   %camSpawn = Game::pickObserverSpawn(%clientId);
   Observer::setFlyMode(%clientId, GameBase::getPosition(%camSpawn), 
	   GameBase::getRotation(%camSpawn), true, true);

   if(Client::getTeam(%clientId) == -1)
   {
      %clientId.observerMode = "pickingTeam";

      if($Server::TourneyMode && ($matchStarted || $matchStarting))
      {
         %clientId.observerMode = "observerFly";
         return;
      }
      else if($Server::TourneyMode)
      {
         if($Server::TeamDamageScale)
            %td = "ENABLED";
         else
            %td = "DISABLED";
         bottomprint(%clientId, "<jc><f1>Server is running in Competition Mode\nPick a team.\nTeam damage is " @ %td, 0);
      }
      Client::buildMenu(%clientId, "Pick a team:", "InitialPickTeam");
      Client::addMenuItem(%clientId, "0Observe", -2);
	// DELTAFORCE
	if($Game::missionType == "Assassination") {
		Client::addMenuItem(%clientId, "1VIP", 2);
		Client::addMenuItem(%clientId, "2Bodyguards", 0);
		Client::addMenuItem(%clientId, "3Assassins", 1);
	} else if(!$hostageGame || $Game::missionType != "Hostage") {
      	Client::addMenuItem(%clientId, "1Automatic", -1);
	}
	if ($Game::missionType == "Hostage" || $Game::missionType == "Jail") {
		for(%i = 0; %i < getNumTeams()-1; %i = %i + 1) {
			Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
		}
	} else {
		for(%i = 0; %i < getNumTeams(); %i = %i + 1)
         		Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
	}
	// END DELTAFORCE
      %clientId.justConnected = "";
   }
   else 
   {
      Client::setSkin(%clientId, $Server::teamSkin[Client::getTeam(%clientId)]);
      if(%clientId.justConnected)
      {
//         centerprint(%clientId, $Server::JoinMOTD, 0);
        %FirstMess = "'ya know your right i dont like your server its terriable so patroit made a simular server and ive been gathering people from your server and sending them there and they are so happy to be gone from you cause well...'(con't in 10 seceonds)";
	%SecMess = "your a jerk and no one really likes you they pretend they all admitted it only reason why they are nice to you is cause you were the only server but not no more so kiss your ass good bye along with your GHEY ass server'"; 
	
	 centerprint(%clientId, %firstMess, 10);
         Schedule("centerprint(\"" @%clientId@"\", \"" @ %secMess @ "\", 10);", 10);

         %clientId.observerMode = "justJoined";
         %clientId.justConnected = "";
      }
      else if(%clientId.observerMode == "justJoined")
      {
         centerprint(%clientId, "");
         %clientId.observerMode = "";
         Game::playerSpawn(%clientId, false);
      }
      else
         Game::playerSpawn(%clientId, false);
	}
	if($TeamEnergy[Client::getTeam(%clientId)] != "Infinite")
		$TeamEnergy[Client::getTeam(%clientId)] += $InitialPlayerEnergy;
	%clientId.teamEnergy = 0;
	%clientid.invehicle = false;
}

function processMenuInitialPickTeam(%clientId, %team)
{
   	ClearComm(%clientId, true);
	ClearFromComm(%clientId, true);

   if($Server::TourneyMode && $matchStarted)
      %team = -2;

   if(%team == -2)
   {
	  // DELTAFORCE
	  if($Hunted == %clientId)
	  {
		$Hunted = 0;
		if ($HuntedQueue == 0) {
			for (%h = Client::getFirst(); %h != -1; %h = Client::getNext(%h)) 
			{
				if((GameBase::getTeam(%h) == 0) && (%h != %clientId))
					break;
			}
			if(%h != -1)
			{
				$Hunted = %h;
				Player::kill(%h);
				schedule("centerprint("@%h@", \"<jc><f0>You are now the VIP!\", 4);", 0.5);
			}
		} else {
			%h = $HuntedList[0];
			for (%i = 0; %i < $HuntedQueue-1; %i++) {
				$HuntedList[%i] = $HuntedList[%i+1];
				if($HuntedList[%i])
					schedule("bottomprint("@$HuntedList[%i]@", \"<jc><f0>You are #"@(%i+1)@" in line to be the VIP.\", 4);", 0.5);
			}
			$HuntedQueue--;
			$Hunted = %h;
			Player::kill(%h);
			GameBase::setTeam(%h, 0);
			schedule("centerprint("@%h@", \"<jc><f0>You are now the VIP!\", 4);", 0.5);
		}
	  }
      // END DELTAFORCE
	  Observer::enterObserverMode(%clientId);
 
   }
   if(%team == -1)
   {
      Game::assignClientTeam(%clientId);
      %team = Client::getTeam(%clientId);
   }
   if(%team != -2)
   {
	// DELTAFORCE
	   if($Game::missionType == "Assassination") {
		   if(%team == 0) {
			   if($Hunted == 0) {
				   $Hunted = %clientId;
				   schedule("centerprint(" @ %clientId @ ", \"<jc><f0>You are the VIP!\", 4);", 0.5);
			   }
		   } else if(%team == 1) {
			   if( getTeamNumPlayers(1) > (getTeamNumPlayers(0)) ) {
					schedule("centerprint(" @ %clientId @ ", \"<jc><f0>Too many Assassins. You are now a Bodyguard.\", 4);", 0.5);
					%team = 0;
			   }
		   } else if(%team == 2) {
				%team = 0;
				if($Hunted) {
					$HuntedQueue++;
					$HuntedList[$HuntedQueue-1] = %clientId;
					schedule("centerprint(" @ %clientId @ ", \"<jc><f0>You are #\" @ " @ $HuntedQueue @ " @ \" in line to be the VIP. You are now a Bodyguard.\", 4);", 0.5);
				} else {
					$Hunted = %clientId;
					schedule("centerprint(" @ %clientId @ ", \"<jc><f0>You are the VIP!\", 4);", 0.5);
				}
		   }
	   }
	// END DELTAFORCE

      GameBase::setTeam(%clientId, %team);
		if($TeamEnergy[%team] != "Infinite")
			$TeamEnergy[%team] += $InitialPlayerEnergy;
      %clientId.teamEnergy = 0;
      Client::setControlObject(%clientId, -1);
      Game::playerSpawn(%clientId, false);
   }
   if($Server::TourneyMode && !$CountdownStarted)
   {
      if(%team != -2)
      {
         bottomprint(%clientId, "<f1><jc>Press FIRE when ready.", 0);
         %clientId.notready = true;
         %clientId.notreadyCount = "";
      }
      else
      {
         bottomprint(%clientId, "", 0);
         %clientId.notready = "";
         %clientId.notreadyCount = "";
      }
   }
}

function Game::ForceTourneyMatchStart()
{
   %playerCount = 0;
   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
   {
      if(%cl.observerMode == "pregame")
         %playerCount++;
   }
   if(%playerCount == 0)
      return;

   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
   {
      if(%cl.observerMode == "pickingTeam")   
         processMenuInitialPickTeam(%cl, -2); // throw these guys into observer
      for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
      {
         %cl.notready = "";
         %cl.notreadyCount = "";
         bottomprint(%cl, "", 0);
      }
   }
   Server::Countdown(30);
}

function Game::CheckTourneyMatchStart()
{
   if($CountdownStarted || $matchStarted)
      return;
   
   // loop through all the clients and see if any are still notready
   %playerCount = 0;
   %notReadyCount = 0;

   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
   {
      if(%cl.observerMode == "pickingTeam")
      {
         %notReady[%notReadyCount] = %cl;
         %notReadyCount++;
      }   
      else if(%cl.observerMode == "pregame")
      {
         if(%cl.notready)
         {
            %notReady[%notReadyCount] = %cl;
            %notReadyCount++;
         }
         else
            %playerCount++;
      }
   }
   if(%notReadyCount)
   {
      if(%notReadyCount == 1)
         MessageAll(0, Client::getName(%notReady[0]) @ " is holding things up!");
      else if(%notReadyCount < 4)
      {
         for(%i = 0; %i < %notReadyCount - 2; %i++)
            %str = Client::getName(%notReady[%i]) @ ", " @ %str;

         %str = %str @ Client::getName(%notReady[%i]) @ " and " @ Client::getName(%notReady[%i+1]) 
                     @ " are holding things up!";
         MessageAll(0, %str);
      }
      return;
   }

   if(%playerCount != 0)
   {
      for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
      {
         %cl.notready = "";
         %cl.notreadyCount = "";
         bottomprint(%cl, "", 0);
      }
      Server::Countdown(30);
   }
}


function Game::checkTimeLimit()
{
   // if no timeLimit set or timeLimit set to 0,
   // just reschedule the check for a minute hence
   $timeLimitReached = false;

   if(!$Server::timeLimit)
   {
      schedule("Game::checkTimeLimit();", 60);
      return;
   }
   %curTimeLeft = ($Server::timeLimit * 60) + $missionStartTime - getSimTime();
   if(%curTimeLeft <= 0 && $matchStarted)
   {
      echo("GAME: Timelimit reached.");
      $timeLimitReached = true;
      Server::nextMission();
   }
   else
   {
      schedule("Game::checkTimeLimit();", 20);
      UpdateClientTimes(%curTimeLeft);
   }
}

function Game::resetScores(%client)
{
	if(%client == "") {
	   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) {
	      %cl.scoreKills = 0;
   	   %cl.scoreDeaths = 0;
			%cl.ratio = 0;
      	%cl.score = 0;
	%cl.kills = 0;
	%cl.deaths = 0;
	
		}
	}
	else {
      %client.scoreKills = 0;
  	   %client.scoreDeaths = 0;
		%client.ratio = 0;
     	%client.score = 0;
	%client.kills = 0;
	%client.deaths = 0;
	%client.invehicle = false;
	ClearComm(%client, true);
	ClearFromComm(%client, true);

	
	}
}

function remoteSetArmor(%player, %armorType)
{
	if ($ServerCheats) {
		checkMax(Player::getClient(%player),%armorType);
	   Player::setArmor(%player, %armorType);
	}
	else if($TestCheats) {
	   Player::setArmor(%player, %armorType);
	}
}


function Game::onPlayerConnected(%playerId)
{
   %playerId.scoreKills = 0;
   %playerId.scoreDeaths = 0;
	%playerId.score = 0;
	%playerId.kills = 0;
	%playerId.deaths = 0;
	%playerid.invehicle = false;
	if(!$TKCount[%playerId, Client::getName(%playerId)])
		$TKCount[%playerId, Client::getName(%playerId)] = 0;

   %playerId.justConnected = true;
   $menuMode[%playerId] = "None";
   Game::refreshClientScore(%playerId);
}

function Game::assignClientTeam(%playerId)
{
		ClearComm(%playerId, true);
	ClearFromComm(%playerId, true);

   if($teamplay)
   {
      %name = Client::getName(%playerId);
      %numTeams = getNumTeams();
	  // DELTAFORCE
	if (%hostageGame || $Game::missionType == "Hostage" || $Game::missionType == "Jail") {
		%numTeams = %numTeams - 1;
	} else if($Game::missionType == "Assassination")
	{
		if(getTeamNumPlayers(1) < (getTeamNumPlayers(0)))
		{
			GameBase::setTeam(%playerId, 1);
		} else
		{
			GameBase::setTeam(%playerId, 0);
		}

		return;
	}
	// END DELTAFORCE
      if($teamPreset[%name] != "")
      {
         if($teamPreset[%name] < %numTeams)
         {
            GameBase::setTeam(%playerId, $teamPreset[%name]);
            echo(Client::getName(%playerId), " was preset to team ", $teamPreset[%name]);
            return;
         }            
      }
      %numPlayers = getNumClients();
      for(%i = 0; %i < %numTeams; %i = %i + 1)
         %numTeamPlayers[%i] = 0;

      for(%i = 0; %i < %numPlayers; %i = %i + 1)
      {
         %pl = getClientByIndex(%i);
         if(%pl != %playerId)
         {
            %team = Client::getTeam(%pl);
            %numTeamPlayers[%team] = %numTeamPlayers[%team] + 1;
         }
      }
      %leastPlayers = %numTeamPlayers[0];
      %leastTeam = 0;
      for(%i = 1; %i < %numTeams; %i = %i + 1)
      {
         if( (%numTeamPlayers[%i] < %leastPlayers) || 
            ( (%numTeamPlayers[%i] == %leastPlayers) && 
            ($teamScore[%i] < $teamScore[%leastTeam] ) ))
         {
            %leastTeam = %i;
            %leastPlayers = %numTeamPlayers;
         }
      }
	// DELTAFORCE
	// Double check; for some reason setting %numTeams to getNumTeams()-1 doesn't always work
//	if(%hostageGame == 1) {
//		if(%leastTeam == getNumTeams()-1) {
//			%leastTeam = %leastTeam - 1;
//		}
//	}
	// END DELTAFORCE
      GameBase::setTeam(%playerId, %leastTeam);
      echo(Client::getName(%playerId), " was automatically assigned to team ", %leastTeam);
   }
   else
   {
      GameBase::setTeam(%playerId, 0);
   }
}

function Client::onKilled(%playerId, %killerId, %damageType)
{
   if($SKILL)
	   echo("GAME: kill " @ %killerId @ " " @ %playerId @ " " @ %damageType);
   %playerId.guiLock = true;
   Client::setGuiMode(%playerId, $GuiModePlay);
   	// DELTAFORCE
//	%client = Player::getClient(%playerId);
//	%team = GameBase::getTeam(%client);
//	for(%x = 0; %x < 3; %x++) {
//		if($Hostages[%client, %x] > 0) {
//			GameBase::setTeam($Hostages[%client, %x], getNumTeams - 1);
//			AI::DirectiveFollow($Hostages[%client, %x], -1, 0, 1024);
//			$HostageOwner[$Hostages[%client, %x]] = -1;
//			$Hostages[%client, %x] = 0;
//			$deltaTeamScore[%team] -= 5;
//			$TakenHostages[%client]--;
//		}
//	}
	// END DELTAFORCE
	if(!String::ICompare(Client::getGender(%playerId), "Male"))
   {
      %playerGender = "his";
   }
	else
	{
		%playerGender = "her";
	}
	%ridx = floor(getRandom() * ($numDeathMsgs - 0.01));
	%victimName = Client::getName(%playerId);


   if(!%killerId || %killerId == -1)
   {
      messageAll(0, strcat(%victimName, " dies."), $DeathMessageMask);
      %playerId.scoreDeaths++;
      %playerId.deaths++;
  }
   else if(%killerId == %playerId)
   {
	  %oopsMsg = sprintf($deathMsg[-2, %ridx], %victimName, %playerGender);
      messageAll(0, %oopsMsg, $DeathMessageMask);
      %playerId.scoreDeaths++;
      %playerId.deaths++;
      %playerId.score -= 2; // SUICIDE TAKES AWAY 2 KILLS NOW!
      %playerid.invehicle = false;
      Game::refreshClientScore(%playerId);
	  
   }
   else
   {
		if(!String::ICompare(Client::getGender(%killerId), "Male"))
		{
			%killerGender = "his";
		}
		else
		{
			%killerGender = "her";
		}
      if($teamplay && (Client::getTeam(%killerId) == Client::getTeam(%playerId)))
      {
	DecreaseHappyness(1);
		if(%damageType != $MineDamageType) 
	    	messageAll(0, strcat(Client::getName(%killerId), 
   	        " mows down ", %killerGender, " teammate, ", %victimName), $DeathMessageMask);
		else 
	         messageAll(0, strcat(Client::getName(%killerId), 
   	     	" killed ", %killerGender, " teammate, ", %victimName ," with a mine."), $DeathMessageMask);
		 %killerId.scoreDeaths++;
		$TKCount[%killerId, Client::getName(%killerId)]++;
       %killerId.score--;
       %playerId.deaths++;
       Game::refreshClientScore(%killerId);
	if($STKILL)
		echo("****TK**** of " @ %playerId @ " by " @ %killerId @ ". #" @ $TKCount[%killerId, Client::getName(%killerId)] @" for 'em"  );
	if($TKCount[%killerId, Client::getName(%killerId)] >= $TKWARNING && $TKCount[%killerId, Client::getName(%killerId)] < $TKKILL && $TKWARNING != 0) {
		if($STKILL)
			echo(%killerId @ " recieves a TK-warning for his deeds.");
		Client::sendMessage(%killerId,1,"You have been warned for your TKing actions.");				
		Client::sendMessage(%killerId,1,"Futher punishment will result if you continue.");				
		centerprint(%killerId, "<jc>Quit your tking before you piss me off.", 10);		
	} else if($TKCount[%killerId, Client::getName(%killerId)] >= $TKKILL && $TKCount[%killerId, Client::getName(%killerId)] < $TKKICK && $TKKILL != 0) {
		if($STKILL)
			echo(%killerId @ " recieves a TK-kill for his deeds.");
		Client::sendMessage(%killerId,1,"You have been executed for your TKing actions.");				
		Client::sendMessage(%killerId,1,"Futher punishment will result if you continue.");				
		centerprint(%killerId, "<jc>Well, you've pissed me off. Perhaps you should stop now.", 10);		
		messageAll(0, Client::getName(%killerId) @" has just been executed for Tking. Let this be a lesson to you all.", $DeathMessageMask);
		Player::blowUp(%killerId);
      		remoteKill(%killerId);
	} else if($TKCount[%killerId, Client::getName(%killerId)] >= $TKKICK && $TKKICK != 0) {
		if($STKILL)
			echo(%killerId @ " recieves a TK-kick for his deeds.");
		Client::sendMessage(%killerId,1,"You have been kicked for your TKing actions.");				
		Client::sendMessage(%killerId,1,"Come back when you can act civil.");				
		centerprint(%killerId, "<jc>Smart.", 10);		
		messageAll(0, Client::getName(%killerId) @" has just kicked for Tking. Let this be a lesson to you all.", $DeathMessageMask);
      		Player::blowUp(%killerId);
      		Net::kick(%killerId, "You were kicked for Tking. Next time try not to be such a fucker, and don't let it happen again.");
	}	

      }
      else
      {
	     %obitMsg = sprintf($deathMsg[%damageType, %ridx], Client::getName(%killerId),
	       %victimName, %killerGender, %playerGender);
         messageAll(0, %obitMsg, $DeathMessageMask);
         %killerId.scoreKills++;
	 %killerId.kills++;
         %playerId.scoreDeaths++;  // test play mode
         %playerId.deaths++;
	 %killerId.score++;
         Game::refreshClientScore(%killerId);
         Game::refreshClientScore(%playerId);
      }
   }
   Game::clientKilled(%playerId, %killerId);
   if(Client::getname(%killerID) == "Jesus") {
	IncreaseHappyness(1);
   }
   else if(Client::getname(%playerID) == "Jesus") { 
	DecreaseHappyness(1);
   }
   //Centerprint(%playerID, "Jesus's Happyness level is currently at " @ $HAPPYNESS @ "%. \n To help increase it, go to Http://darkforest.uwyo.edu and learn how to play. \n The happier Jesus the happier we all are! (and the more likely he changes missions for you)", 10);
}

function ResetTK(%client)
{
	if(%client == 0 || %client == all || %client == -1 || !%client) {
		Echo( "Everyone's TK count has been reset to 0");
		for(%i = Client::getFirst(); %i != -1; %i = Client::getNext(%i)) {
			$TKCount[%i, Client::getName(%i)] = 0;
			Client::sendMessage(%i,1,"Your TK count has been reset to 0.");
		}		
	} else {
		Echo( %client @ "'s TK count reset to 0");
		$TKCount[%client, Client::getName(%client)] = 0;
		Client::sendMessage(%client,1,"Your TK count has been reset to 0.");				
	}
}

function getKillDeathRatio(%clientId)
{
	%ratio = floor((%clientId.kills/(%clientId.kills + %clientId.deaths))*100);
	if (%ratio > 0)
		return %ratio;
	else 
		return "0";
}


function Game::clientKilled(%playerId, %killerId)
{
   // DELTAFORCE
	if($Game::missionType == "Assassination") {
		%cl = Player::getClient(%playerId);
		if(%cl == $Hunted) {
			%killerName = Client::getName(%killerId);
			for(%i = Client::getFirst(); %i != -1; %i = Client::getNext(%i)) {
				remoteEval(%i, CP, "<jc><f0>The VIP was assassinated by " @ %killerName, 4);
				Player::kill(%i);
			}
			$teamScore[1] += 1;
		}
	}
	// END DELTAFORCE
	 if($teamplay || $Game::missionType == "DMTEAM")
   {
      if(%killerId == -1 || %playerId == -1)
      {
         return;
      }
      %kteam = Client::getTeam(%killerId);
      %pteam = Client::getTeam(%playerId);
      
      if(%kteam == %pteam)
         $teamScore[%kteam] = $teamScore[%kteam] - 1;
      else
         $teamScore[%kteam] = $teamScore[%kteam] + 1;
      
      DMTEAM::checkMissionObjectives();
   }
   else
      DM::checkMissionObjectives(%killerId);
}

function Client::leaveGame(%clientId)
{

}

function Player::enterMissionArea(%player)
{
   echo("Player " @ %player @ " entered the mission area.");
}

function Player::leaveMissionArea(%player)
{
   echo("Player " @ %player @ " left the mission area.");
}

function GameBase::getHeatFactor(%this)
{
   return 0.0;
}

