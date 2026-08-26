//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Falcon
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$VehicleInvList[HAPCVehicle] = 1;
$DataBlockName[HAPCVehicle] = HAPC;
$VehicleToItem[HAPC] = HAPCVehicle;
$ItemToVehicle[HAPCVehicle] = HAPC;
$VehicleSlots[HAPC] = 4;

$TeamItemMax[HAPCVehicle] = 2;

$DamageScale[HAPC, $ImpactDamageType] = 1.0;
$DamageScale[HAPC, $BulletDamageType] = 1.0;
$DamageScale[HAPC, $PlasmaDamageType] = 1.0;
$DamageScale[HAPC, $EnergyDamageType] = 1.0;
$DamageScale[HAPC, $ExplosionDamageType] = 1.0;
$DamageScale[HAPC, $ShrapnelDamageType] = 1.0;
$DamageScale[HAPC, $DebrisDamageType] = 1.0;
$DamageScale[HAPC, $MissileDamageType] = 1.0;
$DamageScale[HAPC, $LaserDamageType] = 0.5;
$DamageScale[HAPC, $MortarDamageType] = 1.0;
$DamageScale[HAPC, $BlasterDamageType] = 0.5;
$DamageScale[HAPC, $ElectricityDamageType] = 1.0;
$DamageScale[HAPC, $MineDamageType] = 1.0;
$DamageScale[HAPC, $SniperDamageType] = 1.0;
$DamageScale[HAPC, $PsiDamageType] = 1.0;
$DamageScale[HAPC, $ChemDamageType] = 1.0;
$DamageScale[HAPC, $KrakenDamageType] = 1.0;
$DamageScale[HAPC, $MeltaDamageType] = 1.0;
$DamageScale[HAPC, $DeathDamageType] = 1.0;
$DamageScale[HAPC, $DDamageType] = 1.0;
$DamageScale[HAPC, $FlamerDamageType] = 1.0;
$DamageScale[HAPC, $ShellDamageType] = 1.0;
$DamageScale[HAPC, $ShurikenDamageType] = 1.0;
$DamageScale[HAPC, $ReaperDamageType] = 1.0;

function vehicleHAPC::Initialize()
{
  $TeamItemCount[0 @ HAPCVehicle] = 0;
  $TeamItemCount[1 @ HAPCVehicle] = 0;
  $TeamItemCount[2 @ HAPCVehicle] = 0;
  $TeamItemCount[3 @ HAPCVehicle] = 0;
  $TeamItemCount[4 @ HAPCVehicle] = 0;
  $TeamItemCount[5 @ HAPCVehicle] = 0;
  $TeamItemCount[6 @ HAPCVehicle] = 0;
  $TeamItemCount[7 @ HAPCVehicle] = 0;
}

RocketData Prism
{
  bulletShapeName = "laserhit.dts";
  explosionTag = mortarexp;
  isVisible = False;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 2.0;
  damageType = $DDamageType;
  explosionRadius = 20.0;
  kickBackStrength = 150.0;
  muzzleVelocity = 1000.0;
  terminalVelocity = 1000.0;
  acceleration = 5.0;
  totalTime = 10.0;
  liveTime = 10.0;
  lightRange = 5.0;
  lightColor = { 0.4, 0.4, 1.0 };
  inheritedVelocityScale = 0.5;
  trailType = 1;
  trailLength = 1000;
  trailWidth = 3.0;
  soundId = SoundDiscSpin;
};

ItemData HAPCVehicle 
{
  description = "Falcon";
  className = "Vehicle";
  heading = $InvHead[ihVeh];
  price = 80;
};

FlierData HAPC 
{
	explosionId = flashExpLarge;
  debrisId = flashDebrisLarge;
  className = "Vehicle";
  shapeFile = "hover_apc";
  shieldShapeName = "shield_large";
  mass = 25.0;
  drag = 1.0;
  density = 1.2;
  maxBank = 0.45;
  maxPitch = 0.475;
  maxSpeed = 26;
  minSpeed = -13;
  lift = 0.3;
  maxAlt = 50;
  maxVertical = 6;
  maxDamage = 6.0;
  damageLevel = {1.0, 1.0};
  maxEnergy = 100;
  accel = 0.25;
  groundDamageScale = 0.125;
  projectileType = prism;
  reloadDelay = 2.0;
  repairRate = 0;
  ramDamage = 2;
  ramDamageType = -1;
  mapFilter = 2;
  mapIcon = "M_vehicle";
  fireSound = SoundFireFlierRocket;
  reloadDelay = 3.0;
  damageSound = SoundTankCrash;
  visibleToSensor = true; 
  shadowDetailMask = 2;
  mountSound = SoundFlyerMount;
  dismountSound = SoundFlyerDismount;
  idleSound = SoundFlyerIdle;
  moveSound = SoundFlyerActive;
  visibleDriver = true;
  driverPose = 23;
};

function HAPC::onAdd(%this) 
{
  %this.shieldStrength = 0.1;
  GameBase::setRechargeRate (%this, 10);
  GameBase::setMapName (%this, "Falcon");
}

function HAPC::onPilot(%this, %player)
{
  //
}

function HAPC::onUnPilot(%this, %player)
{
  //
}