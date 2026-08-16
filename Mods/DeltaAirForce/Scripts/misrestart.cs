// Reset all the custom variables specific to a map
//$Lightning = 0;
$Server::timeLimit = 60;
$HuntedQueue = 0;
$Hunted = 0;
$Server::TourneyMode = "false";
$VIPText = "The VIP has escaped!";
$IsTourny = false;

$NukeThroughPrefabs = true;

DecreaseHappyness(5);

for(%i = Client::getFirst(); %i != -1; %i = Client::getNext(%i))
{
	//%i.deaths = 0;
	//%i.kills = 0;
	//$TKCount[%i, Client::getName(%i)] = 0;
	schedule(%i@".deaths = 0;", 10, %i);  
	schedule(%i@".kills = 0;", 10, %i);  
	schedule("$TKCount["@%i@", Client::getName("@%i@")] = 0;", 10, %i);  
}


for(%i = Client::getFirst(); %i != -1; %i = Client::getNext(%i))
{
	for(%x = 0; %x < 3; %x++)
	{
		$Hostages[%i, %x] = 0;
	}
	$TakenHostages[%i] = 0;
}

$spawnBuyList[0] = InfantryArmor;
$spawnBuyList[1] = Knife;
$spawnBuyList[2] = SOCOM;
$spawnBuyList[3] = OICW;
$spawnBuyList[4] = RepairKit;

// List of all items available to buy from inventory station
$InvList[Blaster] = 0;
$InvList[Chaingun] = 0;
$InvList[Disclauncher] = 0;
$InvList[GrenadeLauncher] = 0;
$InvList[Mortar] = 0;
$InvList[PlasmaGun] = 0;
$InvList[LaserRifle] = 0;
$InvList[EnergyRifle] = 0;
$InvList[TargetingLaser] = 1;
$InvList[BouncingMinePack] = 1;
$InvList[APMinePack] = 1;
$InvList[AAMinePack] = 1;
$InvList[Grenade] = 1;
// DELTA FORCE
$InvList[Knife] = 1;
$InvList[SOCOM] = 1;
$InvList[OICW] = 1;
$InvList[SAW] = 1;
$InvList[MP5] = 1;
$InvList[PSG1] = 1;
$InvList[FiftyCal] = 1;
$InvList[LAW] = 1;
$InvList[Howitzer] = 0;
$InvList[Airstrike] = 0;
$InvList[GrappleHook] = 0;
$InvList[Minigun] = 0;
$InvList[TwentyFivemmgun] = 0;
$InvList[Fortymmgun] = 0;
$InvList[AbramsGun] = 0;
$InvList[AutoShotgun] = 1;
$InvList[Stinger] = 1;
$InvList[Flamethrower] = 1;
// END DELTA FORCE

$InvList[BulletAmmo] = 0;
$InvList[PlasmaAmmo] = 0;
$InvList[DiscAmmo] = 0;
$InvList[GrenadeAmmo] = 0;
$InvList[MortarAmmo] = 0;
// DELTA FORCE
$InvList[SOCOMAmmo] = 1;
$InvList[OICWAmmo] = 1;
$InvList[SAWAmmo] = 1;
$InvList[MP5Ammo] = 1;
$InvList[PSG1Ammo] = 1;
$InvList[FiftyCalAmmo] = 1;
$InvList[LAWAmmo] = 1;
$InvList[HowitzerAmmo] = 0;
$InvList[AutoShotgunAmmo] = 1;
$InvList[StingerAmmo] = 1;
$InvList[FlameAmmo] = 1;

$InvList[SOCOMClip] = 1;
$InvList[OICWClip] = 1;
$InvList[SAWClip] = 1;
$InvList[MP5Clip] = 1;
$InvList[PSG1Clip] = 1;
$InvList[FiftyCalClip] = 1;
$InvList[AutoShotgunClip] = 1;
$InvList[StingerClip] = 1;
// END DELTA FORCE
  
$InvList[EnergyPack] = 0;
$InvList[RepairPack] = 1;
$InvList[ShieldPack] = 0;
$InvList[SensorJammerPack] = 1;
$InvList[MotionSensorPack] = 1;
$InvList[PulseSensorPack] = 1;
$InvList[DeployableSensorJammerPack] = 1;
$InvList[CameraPack] = 1;
//$InvList[TurretPack] = 1;
$InvList[RepairKit] = 1;
$InvList[DeployableInvPack] = 1;
$InvList[DeployableAmmoPack] = 1;
$InvList[DeployableHealthPack] = 1;
// DELTA FORCE
$InvList[SAMPack] = 1;
$InvList[HowitzerPack] = 1;
$InvList[TwentyPack] = 1;
$InvList[Charge] = 1;
$InvList[AirstrikePack] = 0;
$InvList[ReloaderPack] = 1;
$InvList[GrapplePack] = 1;
$InvList[Parachute] = 1;
$InvList[FuelPack] = 1;
$InvList[PortGenPack] = 1;
$InvList[AAPack] = 1;
$InvList[AmmoPackSmall] = 1;
$InvList[AmmoPackHeavy] = 1;
$InvList[AmmoPackExp] = 1;
$InvList[MedicPack] = 1;
$InvList[SandBagpack] = 1;
// END DELTA FORCE

//VehicleModule list
$InvList[SideWinderModule] = 1;
$InvList[PhoenixModule] = 1;
$InvList[HellFireModule] = 1;
$InvList[HydraModule] = 1;
$InvList[MaverickModule] = 1;
$InvList[HARMModule] = 1;
$InvList[Mk82Module] = 1;
$InvList[Mk84Module] = 1;
$InvList[Mk82PackageModule] = 1;
$InvList[Mk84PackageModule] = 1;
$InvList[APClusterModule] = 1;
$InvList[MineClusterModule] = 1;
$InvList[BombletClusterModule] = 1;
$InvList[BunkerBusterModule] = 1;
$InvList[TankSmokeModule] = 1;
$InvList[TankNerveGasModule] = 1;
$InvList[IncendiaryNapamModule] = 1;
$InvList[NuclearModule] = 1;
$InvList[DaisyCutterModule] = 1;
$InvList[SupplyCrateModule] = 1;
$InvList[EMCModule] = 1;

//----------------------------------------------------------------------------

// List of all items available to buy from Remote Station
$RemoteInvList[Blaster] = 0;
$RemoteInvList[Chaingun] = 0;
$RemoteInvList[Disclauncher] = 0;
$RemoteInvList[GrenadeLauncher] = 0;
$RemoteInvList[Mortar] = 0;
$RemoteInvList[PlasmaGun] = 0;
$RemoteInvList[LaserRifle] = 0;
$RemoteInvList[EnergyRifle] = 0;
$RemoteInvList[TargetingLaser] = 1;
$RemoteInvList[BouncingMinePack] = 1;
$RemoteInvList[APMinePack] = 1;
$RemoteInvList[AAMinePack] = 1;
$RemoteInvList[Grenade] = 1;
// DELTA FORCE
$RemoteInvList[Knife] = 1;
$RemoteInvList[SOCOM] = 1;
$RemoteInvList[OICW] = 1;
$RemoteInvList[SAW] = 1;
$RemoteInvList[MP5] = 1;
$RemoteInvList[PSG1] = 1;
$RemoteInvList[FiftyCal] = 1;
$RemoteInvList[LAW] = 1;
$RemoteInvList[Howitzer] = 0;
$RemoteInvList[Airstrike] = 0;
$RemoteInvList[GrappleHook] = 0;
$RemoteInvList[Minigun] = 0;
$RemoteInvList[TwentyFivemmgun] = 0;
$RemoteInvList[Fortymmgun] = 0;
$RemoteInvList[AbramsGun] = 0;
$RemoteInvList[AutoShotgun] = 1;
$RemoteInvList[Stinger] = 1;
$RemoteInvList[Flamethrower] = 1;
// END DELTA FORCE

$RemoteInvList[BulletAmmo] = 0;
$RemoteInvList[PlasmaAmmo] = 0;
$RemoteInvList[DiscAmmo] = 0;
$RemoteInvList[GrenadeAmmo] = 0;
$RemoteInvList[MortarAmmo] = 0;
// DELTA FORCE
$RemoteInvList[SOCOMAmmo] = 1;
$RemoteInvList[OICWAmmo] = 1;
$RemoteInvList[SAWAmmo] = 1;
$RemoteInvList[MP5Ammo] = 1;
$RemoteInvList[PSG1Ammo] = 1;
$RemoteInvList[FiftyCalAmmo] = 1;
$RemoteInvList[LAWAmmo] = 1;
$RemoteInvList[HowitzerAmmo] = 0;
$RemoteInvList[AutoShotgunAmmo] = 1;
$RemoteInvList[StingerAmmo] = 1;
$RemoteInvList[FlameAmmo] = 1;

$RemoteInvList[SOCOMClip] = 1;
$RemoteInvList[OICWClip] = 1;
$RemoteInvList[SAWClip] = 1;
$RemoteInvList[MP5Clip] = 1;
$RemoteInvList[PSG1Clip] = 1;
$RemoteInvList[FiftyCalClip] = 1;
$RemoteInvList[AutoShotgunClip] = 1;
$RemoteInvList[StingerClip] = 1;

// END DELTA FORCE
  
$RemoteInvList[EnergyPack] = 0;
$RemoteInvList[RepairPack] = 1;
$RemoteInvList[ShieldPack] = 0;
$RemoteInvList[SensorJammerPack] = 1;
$RemoteInvList[MotionSensorPack] = 1;
$RemoteInvList[PulseSensorPack] = 1;
$RemoteInvList[DeployableSensorJammerPack] = 1;
$RemoteInvList[CameraPack] = 1;
//$RemoteInvList[TurretPack] = 1;
$RemoteInvList[RepairKit] = 1;
// DELTA FORCE
$RemoteInvList[DeployableHealthPack] = 0;
$RemoteInvList[SAMPack] = 0;
$RemoteInvList[HowitzerPack] = 0;
$RemoteInvList[TwentyPack] = 0;
$RemoteInvList[Charge] = 1;
$RemoteInvList[AirstrikePack] = 0;
$RemoteInvList[ReloaderPack] = 1;
$RemoteInvList[GrapplePack] = 0;
$RemoteInvList[Parachute] = 0;
$RemoteInvList[FuelPack] = 1;
$RemoteInvList[PortGenPack] = 1;
$RemoteInvList[AAPack] = 0;
$RemoteInvList[AmmoPackSmall] = 1;
$RemoteInvList[AmmoPackHeavy] = 1;
$RemoteInvList[AmmoPackExp] = 1;
$RemoteInvList[MedicPack] = 1;
$RemoteInvList[SandBagpack] = 1;
//VehicleModule list
$RemoteInvList[SideWinderModule] = 1;
$RemoteInvList[PhoenixModule] = 1;
$RemoteInvList[HellFireModule] = 1;
$RemoteInvList[HydraModule] = 1;
$RemoteInvList[TankSmokeModule] = 1;
$RemoteInvList[TankNerveGasModule] = 1;
$RemoteInvList[EMCModule] = 1;
// END DELTA FORCE

//----------------------------------------------------------------------------

// List of all items available to buy from Vehicle station
// DELTA FORCE
$VehicleInvList[FighterVehicle] = 1;
$VehicleInvList[Fighter2Vehicle] = 1;
//$VehicleInvList[DiveBomberVehicle] = 1;
$VehicleInvList[WarthogVehicle] = 1;
$VehicleInvList[BomberVehicle] = 1;
$VehicleInvList[HerculesVehicle] = 1;
$VehicleInvList[GunshipVehicle] = 1;
$VehicleInvList[ApacheVehicle] = 1;
$VehicleInvList[HindVehicle] = 1;
$VehicleInvList[BlackhawkVehicle] = 1;
$VehicleInvList[HumveeVehicle] = 1;
$VehicleInvList[AbramsVehicle] = 1;
$VehicleInvList[BradleyVehicle] = 1;
$VehicleInvList[LineBackerVehicle] = 1;
$VehicleInvList[MLRSVehicle] = 1;
$VehicleInvList[MineLayerVehicle] = 1;
// List of all items available to buy from Jet station
$JetInvList[FighterVehicle] = 1;
$JetInvList[Fighter2Vehicle] = 1;
//$JetInvList[DiveBomberVehicle] = 1;
$JetInvList[WarthogVehicle] = 1;
$JetInvList[BomberVehicle] = 1;
$JetInvList[HerculesVehicle] = 1;
$JetInvList[GunshipVehicle] = 1;

//Modules available from Jet and Chopper station, as well as inv.
$JetInvList[SideWinderModule] = 1;		$ChopperInvList[SideWinderModule] = 1;
$JetInvList[PhoenixModule] = 1;			$ChopperInvList[PhoenixModule] = 1;
$JetInvList[HellFireModule] = 1;		$ChopperInvList[HellFireModule] = 1;
						$ChopperInvList[HydraModule] = 1;

$JetInvList[MaverickModule] = 1;		
$JetInvList[HARMModule] = 1;			
$JetInvList[Mk82Module] = 1;			$ChopperInvList[Mk82Module] = 1;
$JetInvList[Mk84Module] = 1;
$JetInvList[Mk82PackageModule] = 1;
$JetInvList[Mk84PackageModule] = 1;			
$JetInvList[APClusterModule] = 1;		$ChopperInvList[APClusterModule] = 1;
$JetInvList[MineClusterModule] = 1;		$ChopperInvList[MineClusterModule] = 1;
$JetInvList[BombletClusterModule] = 1;		$ChopperInvList[BombletClusterModule] = 1;
$JetInvList[BunkerBusterModule] = 1;		
$JetInvList[TankSmokeModule] = 1;		$ChopperInvList[TankSmokeModule] = 1;
$JetInvList[TankNerveGasModule] = 1;		$ChopperInvList[TankNerveGasModule] = 1;
$JetInvList[IncendiaryNapamModule] = 1;		$ChopperInvList[IncendiaryNapamModule] = 1;
$JetInvList[NuclearModule] = 1;			
$JetInvList[DaisyCutterModule] = 1;			
$JetInvList[SupplyCrateModule] = 1;
$JetInvList[EMCModule] = 1;			$ChopperInvList[EMCModule] = 1;
$JetInvList[ReloaderPack] = 1;			$ChopperInvList[ReloaderPack] = 1;
$JetInvList[RepairPack] = 1;			$ChopperInvList[RepairPack] = 1;


//----------------------------------------------------------------------------

// Limit on number of special Items you can buy
$TeamItemMax[DeployableAmmoPack] = 7;
$TeamItemMax[DeployableInvPack] = 5;
$TeamItemMax[TurretPack] = 10;
$TeamItemMax[CameraPack] = 15;
$TeamItemMax[DeployableSensorJammerPack] = 8;
$TeamItemMax[PulseSensorPack] = 15;
$TeamItemMax[MotionSensorPack] = 15;
$TeamItemMax[ScoutVehicle] = 3;
$TeamItemMax[HAPCVehicle] = 1;
$TeamItemMax[LAPCVehicle] = 2;
$TeamItemMax[Beacon] = 40;
$TeamItemMax[BouncingMinePack] = 20;
$TeamItemMax[APMinePack] = 12;
$TeamItemMax[AAMinePack] = 35;
$TeamItemMax[SandBagpack] = 75;
// DELTA FORCE
$TeamItemMax[DeployableHealthPack] = 5;
$TeamItemMax[ReloaderPack] = 5;
$TeamItemMax[PortGenPack] = 3;
$TeamItemMax[AirstrikePack] = 4;
$TeamItemMax[GrapplePack] = 5;
$TeamItemMax[SAMPack] = 4;
$TeamItemMax[HowitzerPack] = 3;
$TeamItemMax[AAPack] = 2;
$TeamItemMax[TwentyPack] = 3;
$TeamItemMax[FighterVehicle] = 3;
//$TeamItemMax[DiveBomberVehicle] = 3;
$TeamItemMax[Fighter2Vehicle] = 3;
$TeamItemMax[ApacheVehicle] = 3;
$TeamItemMax[WarthogVehicle] = 2;
$TeamItemMax[BlackhawkVehicle] = 2;
$TeamItemMax[HumveeVehicle] = 3;
$TeamItemMax[AbramsVehicle] = 3;
$TeamItemMax[BradleyVehicle] = 3;
$TeamItemMax[LineBackerVehicle] = 3;
$TeamItemMax[MLRSVehicle] = 3;
$TeamItemMax[MineLayerVehicle] = 2;
$TeamItemMax[BomberVehicle] = 2;
$TeamItemMax[HerculesVehicle] = 2;
$TeamItemMax[GunshipVehicle] = 2;
$TeamItemMax[HindVehicle] = 3;
$TeamItemMax[Charge] = 3;
// END DELTA FORCE

function Flag::onDrop(%player, %type)
{
   %playerTeam = GameBase::getTeam(%player);
   %flag = %player.carryFlag;
   %flagTeam = GameBase::getTeam(%flag);
   %playerClient = Player::getClient(%player);
   %dropClientName = Client::getName(%playerClient);

   if(%flagTeam == -1)
   {
      MessageAllExcept(%playerClient, 1, %dropClientName @ " dropped " @ %flag.objectiveName @ "!");
      Client::sendMessage(%playerClient, 1, "You dropped "  @ %flag.objectiveName @ "!");
   }
   else
   {
      MessageAllExcept(%playerClient, 0, %dropClientName @ " dropped the " @ getTeamName(%flagTeam) @ " flag!");
      Client::sendMessage(%playerClient, 0, "You dropped the " @ getTeamName(%flagTeam) @ " flag!");
      TeamMessages(1, %flagTeam, "Your flag was dropped in the field.", -2, "", "The " @ getTeamName(%flagTeam) @ " flag was dropped in the field.");
   }
   GameBase::throw(%flag, %player, 10, false);
   Item::hide(%flag, false);
   Player::setItemCount(%player, "Flag", 0);
   %flag.carrier = -1;
   %player.carryFlag = "";
   Flag::clearWaypoint(%playerClient, false);

   schedule("Flag::checkReturn(" @ %flag @ ", " @ %flag.pickupSequence @ ");", $flagReturnTime);
	%flag.dropFade = 1;
   ObjectiveMission::ObjectiveChanged(%flag);
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
	if(%clientId.spawnWeapon != "") {
		Player::useItem(%pl,%clientId.spawnWeapon);	
   	%clientId.spawnWeapon="";
	}
}

function alertPlayer(%player, %count)
{
	if(%player.outArea == 1) {
		if(%count > 0) {
		  	Client::sendMessage(Player::getClient(%player),0,"~wLeftMissionArea.wav");
		   schedule("alertPlayer(" @ %player @ ", " @ %count - 1 @ ");",1.5,%player);
		}
		else { 
			%set = nameToID("MissionCleanup/ObjectivesSet");
			for(%i = 0; (%obj = Group::getObject(%set, %i)) != -1; %i++)
	  			GameBase::virtual(%obj, "playerLeaveMissionArea", %player);
		}
	}
}

SensorData MediumPulseSensor
{
   description = "Medium Radar Tower";
   shapeFile = "sensor_pulse_med";
//   explosionId = DebrisExp;
   maxDamage = 1.0;
   range = 250;
   dopplerVelocity = 0;
   castLOS = true;
   supression = false;
	visibleToSensor = true;
	sequenceSound[0] = { "power", SoundSensorPower };
	mapFilter = 4;
	mapIcon = "M_Radar";
	debrisId = flashDebrisLarge;
   shieldShapeName = "shield_medium";
	maxEnergy = 100;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
};

//TurretData IndoorTurret
//{
//	className = "Turret";
//	shapeFile = "indoorgun";
//	projectileType = SAWBullet;
//	maxDamage = 1.5;
//	maxEnergy = 60;
//	minGunEnergy = 20;
//	maxGunEnergy = 6;
//	reloadDelay = 0.4;
//	speed = 5.0;
//	speedModifier = 1.0;
//	range = 25;
//	visibleToSensor = true;
//	dopplerVelocity = 2;
//	castLOS = true;
//	supression = false;
//	supressable = false;
//	pinger = false;
//	mapFilter = 2;
//	mapIcon = "M_turret";
//	debrisId = defaultDebrisMedium;
//	shieldShapeName = "shield";
//	fireSound = SoundMortarTurretFire;
//	activationSound = SoundEnergyTurretOn;
//	deactivateSound = SoundEnergyTurretOff;
//	damageSkinData = "objectDamageSkins";
//	shadowDetailMask = 8;
//	explosionId = debrisExpMedium;
//	description = "Indoor Machine Gun";
//
//};

function AI::setWeapons(%aiName)
{
	%aiId = AI::getId(%aiName);
	
   if(Game::missionType == "DM")
   {	
      dbecho(2, "giving DM weapon select...");
      //Player::setItemCount(%aiId, SOCOM, 1);
   }
   // DELTAFORCE
   else if(Game::missionType == "Hostage") {
	dbecho(2, "Hostages are unarmed.");
   }
   // END DELTAFORCE
   else
   {
      //dbecho(2, "giving normal weapon select...");
      //Player::setItemCount(%aiId, SOCOM, 1);
	  // Player::setItemCount(%aiId, OICW, 1);
	  // Player::setItemCount(%aiId, OICWAmmo, 200);
   }

   // Player::mountItem(%aiId, SOCOM, 0);
   AI::SetVar(%aiName, triggerPct, 0.03 );
   AI::setVar(%aiName, iq, 70 );
   AI::setVar(%aiName, attackMode, 1);
   AI::setAutomaticTargets( %aiName );
   //ai::callbackPeriodic(%aiName, 5, ai::periodicWeaponChange);
}
function Player::onKilled(%this)
{
	%cl = GameBase::getOwnerClient(%this);
	%cl.dead = 1;
	if($AutoRespawn > 0)
		schedule("Game::autoRespawn(" @ %cl @ ");",$AutoRespawn,%cl);
	if(%this.outArea==1)	
		leaveMissionAreaDamage(%cl);
	Player::setDamageFlash(%this,0.75);
	for (%i = 0; %i < 8; %i = %i + 1) {
		%type = Player::getMountedItem(%this,%i);
		if (%type != -1) {
			if (%i != $WeaponSlot || !Player::isTriggered(%this,%i) || getRandom() > "0.2") 
				Player::dropItem(%this,%type);
		}
	}
	// DELTAFORCE
	%team = GameBase::getTeam(%cl);
	//echo("Hostage freed!");
	if($Game::missionType == "Hostage" || $hostageGame == 1) {
		for(%x = 0; %x < 3; %x++) {
			if($Hostages[%cl, %x] != 0) {
				//echo("I'm called! Yay!");
				GameBase::setTeam($Hostages[%cl, %x], getNumTeams() - 1);
				$hostageTeam[$Hostages[%cl, %x]] = getNumTeams() - 1;
				AI::DirectiveFollow(Client::getName($Hostages[%cl, %x]), getNearestAITeammate($Hostages[%cl, %x]), 0, 1024);
				messageAll(1, Client::getName(%cl) @ " lost " @ Client::getName($Hostages[%cl, %x]) @ "!");
				//AI::DirectiveWaypoint(Client::getName($Hostages[%cl, %x]), $AISpawn[$Hostages[%cl, %x]] , 0);
				$HostageOwner[$Hostages[%cl, %x]] = -1;
				$Hostages[%cl, %x] = 0;
				if($deltaTeamScore[%team] >= 5)
					$deltaTeamScore[%team] -= 5;
				$TakenHostages[%cl]--;
			}
		}
	}

	// Sniper limitation code
	if(Player::getArmor(%this) == "sarmor" || Player::getArmor(%this) == "sfemale" ||Player::getArmor(%this) == "sarmor2" || Player::getArmor(%this) == "sfemale2" || Player::getArmor(%this) == "sarmor3" || Player::getArmor(%this) == "sfemale3")
		$SniperCount[%team]--;

	if(Player::getItemCount(%this, FuelPack) == 1) {
		%trans = GameBase::getMuzzleTransform(%cl);
		Projectile::spawnProjectile("FlameExplosion", %trans, %cl, Item::getVelocity(%cl));
	}
	$PlayerBurning[%cl] = 0;
	$PlayerBleeding[%cl] = 0;
	$woundedWeaponArm[%cl] = 0;
	$woundedLegs[%cl] = 0;

	if($Game::missionType == "Assassination") {
		if(%cl == $Hunted) {
			if($HuntedEscaped != 1) {
				//%killerName = Client::getName(%killerId);
				for(%i = Client::getFirst(); %i != -1; %i = Client::getNext(%i)) {
					remoteEval(%i, CP, "<jc><f0>The VIP was assassinated!", 4);
					Player::kill(%i);
				}
				$teamScore[1] += 1;
			}
		}
	}
	// END DELTAFORCE

   if(%cl != -1)
   {
		if(%this.vehicle != "")	{
			if(%this.driver != "") {
				%this.driver = "";
        	 	Client::setControlObject(Player::getClient(%this), %this);
        	 	Player::setMountObject(%this, -1, 0);
			}
			else {
				%this.vehicle.Seat[%this.vehicleSlot-2] = "";
				%this.vehicleSlot = "";
			}
			%this.vehicle = "";		
		}
      schedule("GameBase::startFadeOut(" @ %this @ ");", $CorpseTimeoutValue, %this);
      Client::setOwnedObject(%cl, -1);
      Client::setControlObject(%cl, Client::getObserverCamera(%cl));
      Observer::setOrbitObject(%cl, %this, 5, 5, 5);
      schedule("deleteObject(" @ %this @ ");", $CorpseTimeoutValue + 2.5, %this);
      %cl.observerMode = "dead";
      %cl.dieTime = getSimTime();
   }

   if( $Game::missionType == "Jail")
	if( %team == 1 )
		$teamScore[0]++;

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
	if (%hostageGame || $Game::missionType == "Hostage") {
		%numTeams = %numTeams - 1;
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
function Game::menuRequest(%clientId)
{
   %curItem = 0;
   Client::buildMenu(%clientId, "Options", "options", true);
   if(!%clientId.selClient){
	if(!$matchStarted || !$Server::TourneyMode)
  	 {
	    	if(%clientId.observerMode == "dead"){
			if( %client.dieTime + $Server::respawnTime >= getSimTime() )
				Client::addMenuItem(%clientId, %curItem++ @ "Change Teams/Observe", "changeteams");
		} else 
			Client::addMenuItem(%clientId, %curItem++ @ "Change Teams/Observe", "changeteams");
		
		// DELTAFORCE
		Client::addMenuItem(%clientId, %curItem++ @ "Weapon Options", "weaponoptions");
		// END DELTAFORCE
		Client::addMenuItem(%clientId, %curItem++ @ "Current Stats", "currentstats");
	   }
   }
   if(%clientId.selClient)
   {
      %sel = %clientId.selClient;
      %name = Client::getName(%sel);

      Client::addMenuItem(%clientId, %curItem++ @ "Set as commander " @ %name, "scomm " @ %sel);
      if($curVoteTopic == "" && !%clientId.isAdmin)
      {
         Client::addMenuItem(%clientId, %curItem++ @ "Vote to admin " @ %name, "vadmin " @ %sel);
         Client::addMenuItem(%clientId, %curItem++ @ "Vote to kick " @ %name, "vkick " @ %sel);
      }
      if(%clientId.isAdmin)
      {
         Client::addMenuItem(%clientId, %curItem++ @ "Kick " @ %name, "kick " @ %sel);
         Client::addMenuItem(%clientId, %curItem++ @ "Kill " @ %name, "killpass1 " @ %sel);
         if(%clientId.isSuperAdmin)
         {
            Client::addMenuItem(%clientId, %curItem++ @ "Ban " @ %name, "ban " @ %sel);
            Client::addMenuItem(%clientId, %curItem++ @ "Admin " @ %name, "admin " @ %sel);
 	    if(%sel.notalk == true)
         	Client::addMenuItem(%clientId, %curItem++ @ "UnGmute " @ %name, "unGmute " @ %sel);
      	    else
         	Client::addMenuItem(%clientId, %curItem++ @ "GMute " @ %name, "Gmute " @ %sel);
	 }
         Client::addMenuItem(%clientId, %curItem++ @ "Change " @ %name @ "'s team", "fteamchange " @ %sel);
      }
      if(%clientId.muted[%sel])
         Client::addMenuItem(%clientId, %curItem++ @ "Unmute " @ %name, "unmute " @ %sel);
      else
         Client::addMenuItem(%clientId, %curItem++ @ "Mute " @ %name, "mute " @ %sel);
      if(%clientId.observerMode == "observerOrbit")
         Client::addMenuItem(%clientId, %curItem++ @ "Observe " @ %name, "observe " @ %sel);
   }
   if($curVoteTopic != "" && %clientId.vote == "")
   {
      Client::addMenuItem(%clientId, %curItem++ @ "Vote YES to " @ $curVoteTopic, "voteYes " @ $curVoteCount);
      Client::addMenuItem(%clientId, %curItem++ @ "Vote NO to " @ $curVoteTopic, "voteNo " @ $curVoteCount);
   }
   else if($curVoteTopic == "" && !%clientId.isAdmin)
   {
      Client::addMenuItem(%clientId, %curItem++ @ "Vote to change mission", "vcmission");
      //if($Server::TeamDamageScale == 1.0)
      //   Client::addMenuItem(%clientId, %curItem++ @ "Vote to disable team damage", "vdtd");
      //else
      //   Client::addMenuItem(%clientId, %curItem++ @ "Vote to enable team damage", "vetd");
      Client::addMenuItem(%clientId, %curItem++ @ "Vote to set team damage scale", "vtdc");
	   Client::addMenuItem(%clientId, %curItem++ @ "Vote to set sniper limitation", "vsni");         
      if($Server::TourneyMode)
      {
         Client::addMenuItem(%clientId, %curItem++ @ "Vote to enter FFA mode", "vcffa");
         if(!$CountdownStarted && !$matchStarted)
            Client::addMenuItem(%clientId, %curItem++ @ "Vote to start the match", "vsmatch");
      }
      else
         Client::addMenuItem(%clientId, %curItem++ @ "Vote to enter Tournament mode", "vctourney");

   }
   else if(%clientId.isAdmin)
   {
      Client::addMenuItem(%clientId, %curItem++ @ "Change mission", "cmission");
      //if($Server::TeamDamageScale == 1.0)
      //   Client::addMenuItem(%clientId, %curItem++ @ "Disable team damage", "dtd");
      //else
      //   Client::addMenuItem(%clientId, %curItem++ @ "Enable team damage", "etd");
	  Client::addMenuItem(%clientId, %curItem++ @ "Set Team Damage Scale", "tdc");
	  Client::addMenuItem(%clientId, %curItem++ @ "Set Sniper Limitation", "sni");
      if($Server::TourneyMode)
      {
         Client::addMenuItem(%clientId, %curItem++ @ "Change to FFA mode", "cffa");
         if(!$CountdownStarted && !$matchStarted)
            Client::addMenuItem(%clientId, %curItem++ @ "Start the match", "smatch");
      }
      else
         Client::addMenuItem(%clientId, %curItem++ @ "Change to Tournament mode", "ctourney");
      Client::addMenuItem(%clientId, %curItem++ @ "Set Time Limit", "ctimelimit");
      Client::addMenuItem(%clientId, %curItem++ @ "Reset Server Defaults", "reset");
   }
}
function processMenuOptions(%clientId, %option)
{
   %opt = getWord(%option, 0);
   %cl = getWord(%option, 1);

	$Options[%clientId] = 0;

   if(%opt == "fteamchange")
   {
      %clientId.ptc = %cl;
      Client::buildMenu(%clientId, "Pick a team:", "FPickTeam", true);
      Client::addMenuItem(%clientId, "0Observer", -2);
      $Options[%clientId]++;
      // DELTAFORCE
	if($Game::missionType == "Assassination") {
		Client::addMenuItem(%clientId, "1VIP", 2);
		$Options[%clientId]++;
		Client::addMenuItem(%clientId, "2Bodyguards", 0);
		$Options[%clientId]++;
		Client::addMenuItem(%clientId, "3Assassins", 1);
		$Options[%clientId]++;
	} else if ((!$hostageGame || $Game::missionType != "Hostage") && $Game::missionType != "Jail") {
		Client::addMenuItem(%clientId, "1Automatic", -1);
		$Options[%clientId]++;
	}
	if($Game::missionType == "Hostage" || $Game::missionType == "Jail" ) {
		for(%i = 0; %i < getNumTeams()-1; %i = %i + 1)
			Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
			$Options[%clientId]++;
   	} else {
      		%i = checkTeams();
        	Client::addMenuItem(%clientId, (2) @ getTeamName(%i), %i);
          	$Options[%clientId]++;
	//	for(%i = 0; %i < getNumTeams(); %i = %i + 1)
        // 		Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
   	}
   	// END DELTAFORCE
   	return;
   }
   // DELTAFORCE
   else if(%opt == "weaponoptions")
   {
   	Client::buildMenu(%clientId, "Weapon Options:", "WOptions", true);
	Client::addMenuItem(%clientId, "0Airstrike", "airstrike");
	Client::addMenuItem(%clientId, "1Auto Shotgun", "autoshotgun");
	Client::addMenuItem(%clientId, "2Grenade", "grenade");
	Client::addMenuItem(%clientId, "3Silenced MP5", "mp5");
	Client::addMenuItem(%clientId, "4Locking Method", "lock");
	Client::addMenuItem(%clientId, "5MineLayer Type", "mine");
	Client::addMenuItem(%clientId, "6Countermeasure Type", "counter");
	if(%clientid.scomm >= floor(getNumTeamPlayers(Client::getTeam(%clientID))/2))
	{
		if($istourny)
			Client::addMenuItem(%clientId, "7DropBase Options", "dropbase");
	}
	
	return;
   } // END DELTAFORCE
   else if(%opt == "currentstats")
   {
	Client::buildMenu(%clientId, "Current Stat Options:", "CStats", true);
	Client::addMenuItem(%clientId, "0Kills", "Kstats");
	Client::addMenuItem(%clientId, "1Deaths", "Dstats");
	Client::addMenuItem(%clientId, "2K-D Ratio", "KDstats");
	Client::addMenuItem(%clientId, "3TKs", "TKstats");
	Client::addMenuItem(%clientId, "4Full Stats", "Fstats");
	return;
   }
   else if(%opt == "changeteams")
   {
      if(!$matchStarted || !$Server::TourneyMode)
      {
         Client::buildMenu(%clientId, "Pick a team:", "PickTeam", true);
         Client::addMenuItem(%clientId, "0Observer", -2);
	$Options[%clientId]++;
  	   // DELTAFORCE
	   	if($Game::missionType == "Assassination") {
			Client::addMenuItem(%clientId, "1VIP", 2);
			$Options[%clientId]++;
			Client::addMenuItem(%clientId, "2Bodyguards", 0);
			$Options[%clientId]++;
			Client::addMenuItem(%clientId, "3Assassins", 1);
			$Options[%clientId]++;
		} else if ((!$hostageGame || $Game::missionType != "Hostage") && $Game::missionType != "Jail") {
			Client::addMenuItem(%clientId, "1Automatic", -1);
			$Options[%clientId]++;
		}
		if($Game::missionType == "Hostage" || $Game::missionType == "Jail") {
			for(%i = 0; %i < getNumTeams()-1; %i = %i + 1)
				Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
				$Options[%clientId]++;
   		} else {
      			//%j = checkTeams();
	      
	  		//if($Shifter::KeepBalanced)
      			//{
      	  			%i = checkTeams();
          			Client::addMenuItem(%clientId, (2) @ getTeamName(%i), %i);
				$Options[%clientId]++;
          		//}
      	  		//else
          		//{
          		//	for(%i = 0; %i < getNumTeams(); %i = %i + 1)
          		//		Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
      	 		//}
			
   	    }
   	   // END DELTAFORCE
         return;
      }
   }
   else if(%opt == "mute")
      %clientId.muted[%cl] = true;
   else if(%opt == "unmute")
      %clientId.muted[%cl] = "";
   else if(%opt == "Gmute") {
   	Client::buildMenu(%clientId, "Global Mute?:", "GMaffirm", true);
    	Client::addMenuItem(%clientId, "1Mute " @ Client::getName(%cl)@ "?", "yes " @ %cl);
    	Client::addMenuItem(%clientId, "2Dont Mute " @ Client::getName(%cl)@ "1", "no " @ %cl);
	return;
   } else if(%opt == "unGmute") {
   	Client::buildMenu(%clientId, "Global Unmute?:", "GUMaffirm", true);
      	Client::addMenuItem(%clientId, "1Unmute " @ Client::getName(%cl)@ "?", "yes " @ %cl);
      	Client::addMenuItem(%clientId, "2Dont Unmute " @ Client::getName(%cl)@ "1", "no " @ %cl);
	return;
   } else if(%opt == "vkick")
   {
      %cl.voteTarget = true;
      Admin::startVote(%clientId, "kick " @ Client::getName(%cl), "kick", %cl);
   }
   else if(%opt == "vadmin")
   {
      %cl.voteTarget = true;
      Admin::startVote(%clientId, "admin " @ Client::getName(%cl), "admin", %cl);
   }
   else if(%opt == "scomm")
   {
      Client::buildMenu(%clientId, "Set commander:", "caffirm", true);
      Client::addMenuItem(%clientId, "1Set " @ Client::getName(%cl)@ " as commander?", "yes " @ %cl);
      Client::addMenuItem(%clientId, "2Don't set " @ Client::getName(%cl)@ " as commander?", "no " @ %cl);
      return;
   }
   else if(%opt == "vsmatch")
      Admin::startVote(%clientId, "start the match", "smatch", 0);
   //else if(%opt == "vetd")
   //   Admin::startVote(%clientId, "enable team damage", "etd", 0);
   //else if(%opt == "vdtd")
   //   Admin::startVote(%clientId, "disable team damage", "dtd", 0);
   else if(%opt == "vtdc") {
	  Client::buildMenu(%clientId, "Vote on Team Damage Scale", "vtdscale", true);
	  Client::addMenuItem(%clientId, "10%", 0);
	  Client::addMenuItem(%clientId, "210%", 10);
	  Client::addMenuItem(%clientId, "325%", 25);
	  Client::addMenuItem(%clientId, "450%", 50);
	  Client::addMenuItem(%clientId, "575%", 75);
	  Client::addMenuItem(%clientId, "690%", 90);
	  Client::addMenuItem(%clientId, "7100%", 100);
	  return;
   }
   else if(%opt == "vsni") {
	  Client::buildMenu(%clientId, "Vote on Sniper Limitation", "vtdsni", true);
	  Client::addMenuItem(%clientId, "110%", 10);
	  Client::addMenuItem(%clientId, "220%", 20);
	  Client::addMenuItem(%clientId, "330%", 30);
	  Client::addMenuItem(%clientId, "440%", 40);
	  Client::addMenuItem(%clientId, "550%", 50);
	  return;
   }
   //else if(%opt == "etd")
   //   Admin::setTeamDamageEnable(%clientId, true);
   //else if(%opt == "dtd")
   //   Admin::setTeamDamageEnable(%clientId, false);
   else if(%opt == "vcffa")
      Admin::startVote(%clientId, "change to Free For All mode", "ffa", 0);
   else if(%opt == "vctourney")
      Admin::startVote(%clientId, "change to Tournament mode", "tourney", 0);
   else if(%opt == "cffa")
      Admin::setModeFFA(%clientId);
   else if(%opt == "ctourney")
      Admin::setModeTourney(%clientId);
   else if(%opt == "voteYes" && %cl == $curVoteCount)
   {
      %clientId.vote = "yes";
      centerprint(%clientId, "", 0);
   }
   else if(%opt == "voteNo" && %cl == $curVoteCount)
   {
      %clientId.vote = "no";
      centerprint(%clientId, "", 0);
   }
   else if(%opt == "kick")
   {
      Client::buildMenu(%clientId, "Confirm kick:", "kaffirm", true);
      Client::addMenuItem(%clientId, "1Kick " @ Client::getName(%cl), "yes " @ %cl);
      Client::addMenuItem(%clientId, "2Don't kick " @ Client::getName(%cl), "no " @ %cl);
      return;
   }
   else if(%opt == "admin")
   {
      Client::buildMenu(%clientId, "Confirm admim:", "aaffirm", true);
      Client::addMenuItem(%clientId, "1Admin " @ Client::getName(%cl), "yes " @ %cl);
      Client::addMenuItem(%clientId, "2Don't admin " @ Client::getName(%cl), "no " @ %cl);
      return;
   }
   else if(%opt == "ban")
   {
      Client::buildMenu(%clientId, "Confirm Ban:", "baffirm", true);
      Client::addMenuItem(%clientId, "1Ban " @ Client::getName(%cl), "yes " @ %cl);
      Client::addMenuItem(%clientId, "2Don't ban " @ Client::getName(%cl), "no " @ %cl);
      return;
   }
  else if(%opt == "killpass1") //======================================================================== Admin Kill Player
   {
   	Client::buildMenu(%clientId, "Kill?:", "Killaffirm", true);
      	Client::addMenuItem(%clientId, "1Kill " @ Client::getName(%cl)@ "?", "yes " @ %cl);
	Client::addMenuItem(%clientId, "2Dont Kill " @ Client::getName(%cl)@ "!", "no " @ %cl);
	return;
   }
   // DELTAFORCE
   else if(%opt == "tdc") {
	  Client::buildMenu(%clientId, "Set Team Damage Scale", "tdscale", true);
	  Client::addMenuItem(%clientId, "10%", 0);
	  Client::addMenuItem(%clientId, "210%", 10);
	  Client::addMenuItem(%clientId, "325%", 25);
	  Client::addMenuItem(%clientId, "450%", 50);
	  Client::addMenuItem(%clientId, "575%", 75);
	  Client::addMenuItem(%clientId, "690%", 90);
	  Client::addMenuItem(%clientId, "7100%", 100);
	  return;
   }
   else if(%opt == "sni") {
	  Client::buildMenu(%clientId, "Set Sniper Limitation", "tdsni", true);
	  Client::addMenuItem(%clientId, "110%", 10);
	  Client::addMenuItem(%clientId, "220%", 20);
	  Client::addMenuItem(%clientId, "330%", 30);
	  Client::addMenuItem(%clientId, "440%", 40);
	  Client::addMenuItem(%clientId, "550%", 50);
	  return;
   }
   // END DELTAFORCE
   else if(%opt == "smatch")
      Admin::startMatch(%clientId);
   else if(%opt == "vcmission" || %opt == "cmission")
   {
      Admin::changeMissionMenu(%clientId, %opt == "cmission");
      return;
   }
   else if(%opt == "ctimelimit")
   {
      Client::buildMenu(%clientId, "Change Time Limit:", "ctlimit", true);
      Client::addMenuItem(%clientId, "110 Minutes", 10);
      Client::addMenuItem(%clientId, "215 Minutes", 15);
      Client::addMenuItem(%clientId, "320 Minutes", 20);
      Client::addMenuItem(%clientId, "425 Minutes", 25);
      Client::addMenuItem(%clientId, "530 Minutes", 30);
      Client::addMenuItem(%clientId, "645 Minutes", 45);
      Client::addMenuItem(%clientId, "760 Minutes", 60);
      Client::addMenuItem(%clientId, "8No Time Limit", 0);
      return;
   }
   else if(%opt == "reset")
   {
      Client::buildMenu(%clientId, "Confirm Reset:", "raffirm", true);
      Client::addMenuItem(%clientId, "1Reset", "yes");
      Client::addMenuItem(%clientId, "2Don't Reset", "no");
      return;
   }
   else if(%opt == "observe")
   {
      Observer::setTargetClient(%clientId, %cl);
      return;
   }
   Game::menuRequest(%clientId);
}
function Game::initialMissionDrop(%clientId)
{
		ClearComm(%clientId, true);
	ClearFromComm(%clientId, true);

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
         centerprint(%clientId, $Server::JoinMOTD, 0);
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
	if(%clientId.spawnWeapon != "") {
		Player::useItem(%pl,%clientId.spawnWeapon);	
   	%clientId.spawnWeapon="";
	}
} 
function TowerSwitch::onCollision(%this, %object)
{
   //echo("switch collision ", %object);
   if(getObjectType(%object) != "Player")
      return;

   if(Player::isDead(%object))
      return;
	// DELTAFORCE
	if($Game::missionType == "Assassination") {
		if(GameBase::getOwnerClient(%object) == $Hunted) {
			for(%i = Client::getFirst(); %i != -1; %i = Client::getNext(%i)) {
				// The VIP has escaped!
				remoteEval(%i, CP, "<jc><f0>" @ $VIPText, 4);
				$HuntedEscaped = 1;
				Player::kill(%i);
				$HuntedEscaped = 0;
			}
			$teamScore[0] += 1;
		}
		return;
	}
	// END DELTAFORCE

   %playerTeam = GameBase::getTeam(%object);
   %oldTeam = GameBase::getTeam(%this);
   if(%oldTeam == %playerTeam)
      return;

   %this.trainingObjectiveComplete = true;
   
   %playerClient = Player::getClient(%object);
   %touchClientName = Client::getName(%playerClient);
   %group = GetGroup(%this);
   Group::iterateRecursive(%group, GameBase::setTeam, %playerTeam);

   %dropPoints = nameToID(%group @ "/DropPoints");
   %oldDropSet = nameToID("MissionCleanup/TeamDrops" @ %oldTeam);
   %newDropSet = nameToID("MissionCleanup/TeamDrops" @ %playerTeam);

   $deltaTeamScore[%oldTeam] -= %this.deltaTeamScore;
   $deltaTeamScore[%playerTeam] += %this.deltaTeamScore;
   $teamScore[%oldTeam] -= %this.scoreValue;
   $teamScore[%playerTeam] += %this.scoreValue;

   if(%dropPoints != -1)
   {
      for(%i = 0; (%dropPoint = Group::getObject(%dropPoints, %i)) != -1; %i++)
      {
         if(%oldDropSet != -1)
            removeFromSet(%oldDropSet, %dropPoint);
         addToSet(%newDropSet, %dropPoint);
      }
   }

   if(%oldTeam == -1)
   {
      MessageAllExcept(%playerClient, 0, %touchClientName @ " claimed " @ %this.objectiveName @ " for the " @ getTeamName(%playerTeam) @ " team!");
      Client::sendMessage(%playerClient, 0, "You claimed " @ %this.objectiveName @ " for the " @ getTeamName(%playerTeam) @ " team!");
 	}
   else
   {
      if(%this.objectiveLine)
      {
         MessageAllExcept(%playerClient, 0, %touchClientName @ " captured " @ %this.objectiveName @ " from the " @ getTeamName(%oldTeam) @ " team!");
         Client::sendMessage(%playerClient, 0, "You captured " @ %this.objectiveName @ " from the " @ getTeamName(%oldTeam) @ " team!");
			%this.numSwitchTeams++;	
			schedule("TowerSwitch::timeLimitCheckPoints(" @ %this @ "," @ %playerClient @ "," @ %this.numSwitchTeams @ ");",60);
      }
   }
   if(%this.objectiveLine)
   {
      TeamMessages(1, %playerTeam, "Your team has taken an objective.~wCapturedTower.wav");
		TeamMessages(0, %playerTeam, "The " @ getTeamName(%playerTeam) @ " has taken an objective.");
		if(%oldTeam != -1)
	      TeamMessages(1, %oldTeam, "The " @ getTeamName(%playerTeam) @ " team has taken your objective.~wLostTower.wav");
      ObjectiveMission::ObjectiveChanged(%this);
   }
   ObjectiveMission::checkScoreLimit();
}