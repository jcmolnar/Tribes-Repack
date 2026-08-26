//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Tempest
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$VehicleInvList[TempestVehicle] = 1;
$DataBlockName[TempestVehicle] = Tempest;
$VehicleToItem[Tempest] = TempestVehicle;
$ItemToVehicle[TempestVehicle] = Tempest;
$VehicleSlots[Tempest] = 4;

$TeamItemMax[TempestVehicle] = 2;

$DamageScale[Tempest, $ImpactDamageType] = 1.0;
$DamageScale[Tempest, $BulletDamageType] = 1.0;
$DamageScale[Tempest, $PlasmaDamageType] = 1.0;
$DamageScale[Tempest, $EnergyDamageType] = 1.0;
$DamageScale[Tempest, $ExplosionDamageType] = 1.0;
$DamageScale[Tempest, $ShrapnelDamageType] = 1.0;
$DamageScale[Tempest, $DebrisDamageType] = 1.0;
$DamageScale[Tempest, $MissileDamageType] = 1.0;
$DamageScale[Tempest, $LaserDamageType] = 0.5;
$DamageScale[Tempest, $MortarDamageType] = 1.0;
$DamageScale[Tempest, $BlasterDamageType] = 0.5;
$DamageScale[Tempest, $ElectricityDamageType] = 1.0;
$DamageScale[Tempest, $MineDamageType] = 1.0;
$DamageScale[Tempest, $SniperDamageType] = 1.0;
$DamageScale[Tempest, $PsiDamageType] = 1.0;
$DamageScale[Tempest, $ChemDamageType] = 1.0;
$DamageScale[Tempest, $KrakenDamageType] = 1.0;
$DamageScale[Tempest, $MeltaDamageType] = 1.0;
$DamageScale[Tempest, $DeathDamageType] = 1.0;
$DamageScale[Tempest, $DDamageType] = 1.0;
$DamageScale[Tempest, $FlamerDamageType] = 1.0;
$DamageScale[Tempest, $ShellDamageType] = 1.0;
$DamageScale[Tempest, $ShurikenDamageType] = 1.0;
$DamageScale[Tempest, $ReaperDamageType] = 1.0;

function vehicleTempest::Initialize()
{
  $TeamItemCount[0 @ TempestVehicle] = 0;
  $TeamItemCount[1 @ TempestVehicle] = 0;
  $TeamItemCount[2 @ TempestVehicle] = 0;
  $TeamItemCount[3 @ TempestVehicle] = 0;
  $TeamItemCount[4 @ TempestVehicle] = 0;
  $TeamItemCount[5 @ TempestVehicle] = 0;
  $TeamItemCount[6 @ TempestVehicle] = 0;
  $TeamItemCount[7 @ TempestVehicle] = 0;
}

RocketData WarpHit 
{
  bulletShapeName = "fusionbolt.dts";
  explosionTag = LargeShockwave;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 2.5;
  damageType = $DDamageType;
  explosionRadius = 35.0;
  kickBackStrength = 450.0;
  muzzleVelocity = 40.0;
  terminalVelocity = 40.0;
  acceleration = 5.0;
  totalTime = 15.0;
  liveTime = 15.0;
  lightRange = 10.0;
  lightColor = { 1.0, 6.7, 9.5 };
  inheritedVelocityScale = 0.5;
  soundId = SoundJetHeavy;
  trailType = 1;
  trailLength = 10;
  trailWidth = 0.4;
};

ItemData TempestVehicle 
{
  description = "Tempest";
  className = "Vehicle";
  heading = $InvHead[ihVeh];
  price = 120;
};

FlierData Tempest 
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
  projectileType = WarpHit;
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

function Tempest::onPilot(%this, %player)
{
  //
}

function Tempest::onUnPilot(%this, %player)
{
  //
}