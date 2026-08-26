
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Vyper Variant 2 moded from Wraith (renegades)
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$VehicleInvList[WraithVehicle] = 1;
$DataBlockName[WraithVehicle] = Wraith;
$VehicleToItem[Wraith] = WraithVehicle;
$VehicleSlots[Wraith] = 0;

$TeamItemMax[WraithVehicle] = 2;

$DamageScale[Wraith, $ImpactDamageType] = 1.0;
$DamageScale[Wraith, $BulletDamageType] = 1.0;
$DamageScale[Wraith, $PlasmaDamageType] = 1.0;
$DamageScale[Wraith, $EnergyDamageType] = 1.0;
$DamageScale[Wraith, $ExplosionDamageType] = 1.0;
$DamageScale[Wraith, $ShrapnelDamageType] = 1.0;
$DamageScale[Wraith, $DebrisDamageType] = 1.0;
$DamageScale[Wraith, $MissileDamageType] = 1.0;
$DamageScale[Wraith, $LaserDamageType] = 1.0;
$DamageScale[Wraith, $MortarDamageType] = 1.0;
$DamageScale[Wraith, $BlasterDamageType] = 0.5;
$DamageScale[Wraith, $ElectricityDamageType] = 1.0;
$DamageScale[Wraith, $MineDamageType] = 1.0;
$DamageScale[Wraith, $SniperDamageType] = 1.0;
$DamageScale[Wraith, $MeltaDamageType] = 1.0;
$DamageScale[Wraith, $DeathDamageType] = 1.0;
$DamageScale[Wraith, $DDamageType] = 1.0;
$DamageScale[Wraith, $FlamerDamageType] = 1.0;
$DamageScale[Wraith, $ShellDamageType] = 1.0;
$DamageScale[Wraith, $ShurikenDamageType] = 1.0;
$DamageScale[Wraith, $ReaperDamageType] = 1.0;

function vehicleWraith::Initialize()
{
  $TeamItemCount[0 @ WraithVehicle] = 0;
  $TeamItemCount[1 @ WraithVehicle] = 0;
  $TeamItemCount[2 @ WraithVehicle] = 0;
  $TeamItemCount[3 @ WraithVehicle] = 0;
  $TeamItemCount[4 @ WraithVehicle] = 0;
  $TeamItemCount[5 @ WraithVehicle] = 0;
  $TeamItemCount[6 @ WraithVehicle] = 0;
  $TeamItemCount[7 @ WraithVehicle] = 0;
}

RocketData PlasmaMissile 
{
  bulletShapeName = "rocket.dts";
  explosionTag = mortarExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.55;
  damageType = $PlasmaDamageType;
  explosionRadius = 20.5;
  kickBackStrength = 150.0;
  muzzleVelocity = 65.0;
  terminalVelocity = 1000.0;
  acceleration = 200.0;
  totalTime = 6.5;
  liveTime = 10.0;
  lightRange = 2.0;
  lightColor = { 1.0, 0.7, 0.5 };
  inheritedVelocityScale = 0.5;
  trailType = 2;
  trailString = "plastrail.dts";
  smokeDist = 1.8;
  soundId = SoundJetHeavy;
};

ItemData WraithVehicle 
{
  description = "Vyper Var.2";
  className = "Vehicle";
  heading = $InvHead[ihVeh];
  price = 50;
};

FlierData Wraith 
{
  explosionId = flashExpLarge;
  debrisId = flashDebrisLarge;
  className = "Vehicle";
  shapeFile = "vyper";
  shieldShapeName = "shield_medium";
  mass = 9.0;
  drag = 1.0;
  density = 1.2;
  maxBank = 1.6;
   maxPitch = 1.6;
   maxSpeed = 60;
   minSpeed = -30;
	lift = 0.9;
	maxAlt = 155;
	maxVertical = 12;
	maxDamage = 0.8;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 0.8;
  groundDamageScale = 1.0;
  projectileType = PlasmaMissile;
  reloadDelay = 1.5;
  repairRate = 0;
  fireSound = SoundFireFlierRocket;
  damageSound = SoundFlierCrash;
  ramDamage = 1.5;
  ramDamageType = -1;
  mapFilter = 2;
  mapIcon = "M_vehicle";
  visibleToSensor = false;
  shadowDetailMask = 2;
  mountSound = SoundFlyerMount;
  dismountSound = SoundFlyerDismount;
  idleSound = SoundFlyerIdle;
  moveSound = SoundFlyerActive;
  visibleDriver = false;
  driverPose = 22;
};

function Wraith::onPilot(%this, %player)
{
  //GameBase::startFadeout(%this);
}

function Wraith::onUnPilot(%this, %player)
{
  //GameBase::startFadein(%this);
}