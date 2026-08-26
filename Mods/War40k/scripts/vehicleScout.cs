//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Jet Bike
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$VehicleInvList[ScoutVehicle] = 1;
$DataBlockName[ScoutVehicle] = Scout;
$VehicleToItem[Scout] = ScoutVehicle;
$ItemToVehicle[ScoutVehicle] = Scout;
$VehicleSlots[Scout] = 0;

$TeamItemMax[ScoutVehicle] = 3;

$DamageScale[Scout, $ImpactDamageType] = 1.0;
$DamageScale[Scout, $BulletDamageType] = 1.0;
$DamageScale[Scout, $PlasmaDamageType] = 1.0;
$DamageScale[Scout, $EnergyDamageType] = 1.0;
$DamageScale[Scout, $ExplosionDamageType] = 1.0;
$DamageScale[Scout, $ShrapnelDamageType] = 1.0;
$DamageScale[Scout, $DebrisDamageType] = 1.0;
$DamageScale[Scout, $MissileDamageType] = 1.0;
$DamageScale[Scout, $LaserDamageType] = 1.0;
$DamageScale[Scout, $MortarDamageType] = 1.0;
$DamageScale[Scout, $BlasterDamageType] = 0.5;
$DamageScale[Scout, $ElectricityDamageType] = 1.0;
$DamageScale[Scout, $MineDamageType] = 1.0;
$DamageScale[Scout, $SniperDamageType] = 1.0;
$DamageScale[Scout, $MeltaDamageType] = 1.0;
$DamageScale[Scout, $DeathDamageType] = 1.0;
$DamageScale[Scout, $DDamageType] = 1.0;
$DamageScale[Scout, $FlamerDamageType] = 1.0;
$DamageScale[Scout, $ShellDamageType] = 1.0;
$DamageScale[Scout, $ShurikenDamageType] = 1.0;
$DamageScale[Scout, $ReaperDamageType] = 1.0;
$DamageScale[Scout, $PsiDamageType] = 1.0;
$DamageScale[Scout, $ChemDamageType] = 1.0;
$DamageScale[Scout, $KrakenDamageType] = 1.0;

function vehicleScout::Initialize()
{
  $TeamItemCount[0 @ ScoutVehicle] = 0;
  $TeamItemCount[1 @ ScoutVehicle] = 0;
  $TeamItemCount[2 @ ScoutVehicle] = 0;
  $TeamItemCount[3 @ ScoutVehicle] = 0;
  $TeamItemCount[4 @ ScoutVehicle] = 0;
  $TeamItemCount[5 @ ScoutVehicle] = 0;
  $TeamItemCount[6 @ ScoutVehicle] = 0;
  $TeamItemCount[7 @ ScoutVehicle] = 0;
}

ItemData ScoutVehicle 
{
  description = "Jet Bike";
  className = "Vehicle";
  heading = $InvHead[ihVeh];
  price = 20;
};

FlierData Scout 
{
  explosionId = flashExpLarge;
  debrisId = flashDebrisLarge;
  className = "Vehicle";
  shapeFile = "flyer";
  shieldShapeName = "shield_medium";
  mass = 9.0;
  drag = 1.0;
  density = 1.2;
  maxBank = 0.9;
   	maxPitch = 0.9;
   	maxSpeed = 80;
   	minSpeed = -80;
	lift = 0.6;
	maxAlt = 25;
	maxVertical = 10;
	maxDamage = 0.6;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 1.0;
  groundDamageScale = 1.0;
  projectileType = MiniFusionBolt;
  reloadDelay = 0.05;
  repairRate = 0;
  fireSound = SoundFirePlasma;
  damageSound = SoundFlierCrash;
  ramDamage = 1.5;
  ramDamageType = -1;
  mapFilter = 2;
  mapIcon = "M_vehicle";
  visibleToSensor = true;
  shadowDetailMask = 2;
  mountSound = SoundFlyerMount;
  dismountSound = SoundFlyerDismount;
  idleSound = SoundFlyerIdle;
  moveSound = SoundFlyerActive;
  visibleDriver = true;
  driverPose = 22;
};

function Scout::onPilot(%this, %player)
{
  //
}

function Scout::onUnPilot(%this, %player)
{
  //
}