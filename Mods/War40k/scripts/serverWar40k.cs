$ItemFavoritesKey = "War40k139";
$Welcome="<jc><f2>Warhammer 40K\n http://www.planetstarsiege.com/warhammer\n\n";
$DefaultArmor[Male] = armormTactical;
$DefaultArmor[Female] = armorfTactical;

 // Initial buy list
$spawnBuyList[2, 0] = iarmorTactical;
$spawnBuyList[2, 1] = Sword;
$spawnBuyList[2, 2] = Plasmagun;
$spawnBuyList[2, 3] = GrenadeLauncher;
$spawnBuyList[2, 4] = Bolter;
$spawnBuyList[2, 5] = RepairKit;
$spawnBuyList[2, 6] = Grenade;
$spawnBuyList[2, 7] = Grenade;
$spawnBuyList[2, 8] = Grenade;
$spawnBuyList[2, 9] = Grenade;
$spawnBuyList[2, 10] = Beacon;
$spawnBuyList[2, 11] = Beacon;
$spawnBuyList[2, 12] = Beacon;
$spawnBuyList[2, 13] = Beacon;
$spawnBuyList[2, 14] = RepairPack;
$spawnBuyList[2, 15] = "";

$spawnBuyList[1, 0] = iarmorTactical;
$spawnBuyList[1, 1] = Sword;
$spawnBuyList[1, 2] = Plasmagun;
$spawnBuyList[1, 3] = GrenadeLauncher;
$spawnBuyList[1, 4] = Bolter;
$spawnBuyList[1, 5] = RepairKit;
$spawnBuyList[1, 6] = Grenade;
$spawnBuyList[1, 7] = Grenade;
$spawnBuyList[1, 8] = Grenade;
$spawnBuyList[1, 9] = Grenade;
$spawnBuyList[1, 10] = Beacon;
$spawnBuyList[1, 11] = Beacon;
$spawnBuyList[1, 12] = Beacon;
$spawnBuyList[1, 13] = Beacon;
$spawnBuyList[1, 14] = RepairPack;
$spawnBuyList[1, 15] = "";

$spawnBuyList[0, 0] = iarmorGuardian;
$spawnBuyList[0, 1] = Sword;
$spawnBuyList[0, 2] = Plasmagun;
$spawnBuyList[0, 3] = GrenadeLauncher;
$spawnBuyList[0, 4] = ShurCata;
$spawnBuyList[0, 5] = RepairKit;
$spawnBuyList[0, 6] = Grenade;
$spawnBuyList[0, 7] = Grenade;
$spawnBuyList[0, 8] = Grenade;
$spawnBuyList[0, 9] = Grenade;
$spawnBuyList[0, 10] = Beacon;
$spawnBuyList[0, 11] = Beacon;
$spawnBuyList[0, 12] = Beacon;
$spawnBuyList[0, 13] = Beacon;
$spawnBuyList[0, 14] = RepairPack;
$spawnBuyList[0, 15] = "";

function serverWar40k::Start()
{
echo('>> Usage');
        exec(serverWar40k_ItemUsage);
   echo('>> Loading armor classes');
        exec(armorScout);
        exec(armorAssault);
        exec(armorTactical);
        exec(armorTech);
        exec(armorTerm);
        exec(armorApoth);
        exec(armorEversor);
        exec(armorRanger);
        exec(armorSwHawk);
        exec(armorGuardian);
        exec(armorWraith);
        exec(armorDReaper);
        exec(armorLib);
        exec(armorBonesinger);
        exec(armorSDaemon);
        exec(armorFiDrgn);
        exec(armorDiAvg);
        exec(armorWarlock);
        exec(armorWarpSpider);
        

   echo('>> Loading weapons');
        exec(weaponBoltguns);
	exec(weaponIncendiary);
	exec(weaponLasers);
	exec(weaponPsionics);
	exec(weaponMelee);
	exec(weaponBallistic);
	exec(weaponRockets);
	exec(weaponShuriken);
	exec(weaponTools);
	exec(weaponOther);
        exec(weaponMasterCrafted);

   echo('>> Loading packs');
        exec(packAmmo);
        exec(packCommand);
        exec(packEnergy);
        exec(packRegeneration);
        exec(packRepair);
        exec(packShield);
        exec(packCloak);
        exec(packJammer);
        exec(packLightening);
        exec(packFlamePack);
        exec(packMind);
        exec(packFeeder);
        exec(packLaserPack);
        exec(packWarpPack);
        exec(packStarCannon);
        exec(packEversor);


// ADMIN TOYS
exec(packArkfire);
exec(packSpy);

//-=-=---FLIGHT PACKS-=-=-=-=-=-

        exec(packAssault);
        exec(packHawk);
//=-==-=-=-==-=-=-=-=----=-=-=-=       
   echo('>> Targeting systems');

   echo('>> Loading misc');
        exec(miscBeacon);
        exec(miscGrenade);
        exec(miscMine);
        exec(miscRepairKit);

   echo('>> Loading deployable sensors');
        exec(deployMotionSensor);
        exec(deployPulseSensor);
        exec(deployCamera);
        exec(deploySensorJammer);

   echo('>> Loading deployable objects');
        exec(deploySmallInventoryStation);
        exec(deploySmallAmmoStation);
        exec(deploySmallCommandStation);
        exec(deployBlastWall);
        exec(deployAirPlat);
        exec(deployForceField);
        exec(deployBigField);
        exec(deployTeleporter);
        exec(deploySpringboard);
        exec(deploySatchelCharge);
        exec(deployLRSensorJammerPack);
        exec(deployLRMotionSensorPack);
        exec(deployLRPulseSensorPack);
        exec(deployDoorPack);
        exec(deployLargeForceDoor);
        exec(deploySatelliteUplink);

        //-=-=-=-=-=DEPLOYABLE PILLBOX/EMPLACEMENT
        exec(deployPB);
        exec(deployPB2);
         
   echo('>> Loading Defensive Armaments');
        exec(deployFusionTurret);
        exec(deployLaserTurret);
        //exec(deployRailTurret);
        exec(deployScatTurret);
        exec(deployPlasmaTurret);
        exec(deployGunBatTurret);
        exec(deployShurTurret);
        exec(deployBoltTurret);
        exec(deployBoltCanTurret);
        exec(deployFlameTurret);
        exec(deployHFlameTurret);
        exec(deployPartTurret);
        exec(deployMissileTurret);
          
   echo('>> Loading Assault Craft');
        exec(Vehicle);
        exec(vehicleScout);
        exec(vehicleInterceptor);
        exec(vehicleHAPC);
        exec(vehicleTempest);
        exec(vehicleLAPC);
}

function serverWar40k::InitializeMission()
{
         
         // Initialize deployables
        deploySmallInventoryStation::Initialize();
        deploySmallAmmoStation::Initialize();
        deploySmallCommandStation::Initialize();
        deployForceField::Initialize();
        deployBigField::Initialize();
        deployBlastWall::Initialize();
        deployTeleporter::Initialize();
        deploySpringboard::Initialize();
        deployAirPlat::Initialize();
        deployLRSensorJammerPack::Initialize();
        deployLRMotionSensorPack::Initialize();
        deployLRPulseSensorPack::Initialize();
        deployDoorPack::Initialize();
        deployLargeForceDoor::Initialize();
        //deployAccelerator::Initialize();
        deploySatelliteUplinkPack::Initialize();
        CommandCenterPack::Initialize();
        CommandCenterPack2::Initialize();

        miscMine::Initialize();
        miscBeacon::Initialize();

        deploySensorJammer::Initialize();
        deployCamera::Initialize();
        deployPulseSensor::Initialize();
        deployMotionSensor::Initialize();

        deployFusionTurret::Initialize();
        deployLaserTurret::Initialize();
        //deployRailTurret::Initialize();
        deployScatTurret::Initialize();
        deployPlasmaTurret::Initialize();
        deployGunBatTurret::Initialize();
        deployShurTurret::Initialize();
        deployBoltTurret::Initialize();
        deployBoltCanTurret::Initialize();
        deployFlameTurret::Initialize();
        deployHFlameTurret::Initialize();
        deployPartTurret::Initialize();
        deployMissileTurret::Initialize();
        
        deploySatchelCharge::Initialize();

        // Initialize vehicles
        vehicleScout::Initialize();
        vehicleInterceptor::Initialize();
        vehicleHAPC::Initialize();
        vehicleTempest::Initialize();
        vehicleLAPC::Initialize();
	$TotalItems = getNumItems();
}
