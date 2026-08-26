//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Attack Jet: Modded from BOP Dogfighter
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$VehicleInvList[DogFighterVehicle] = 1;
$DataBlockName[DogFighterVehicle] = DogFighter;
$VehicleToItem[DogFighter] = DogFighterVehicle;
$VehicleSlots[DogFighter] = 0;

$TeamItemMax[DogFighterVehicle] = 2;

$DamageScale[DogFighter, $ImpactDamageType] = 1.0;
$DamageScale[DogFighter, $BulletDamageType] = 1.0;
$DamageScale[DogFighter, $PlasmaDamageType] = 1.0;
$DamageScale[DogFighter, $EnergyDamageType] = 1.0;
$DamageScale[DogFighter, $ExplosionDamageType] = 1.0;
$DamageScale[DogFighter, $ShrapnelDamageType] = 1.0;
$DamageScale[DogFighter, $DebrisDamageType] = 1.0;
$DamageScale[DogFighter, $MissileDamageType] = 1.0;
$DamageScale[DogFighter, $LaserDamageType] = 1.0;
$DamageScale[DogFighter, $MortarDamageType] = 1.0;
$DamageScale[DogFighter, $BlasterDamageType] = 1.0;
$DamageScale[DogFighter, $ElectricityDamageType] = 1.0;
$DamageScale[DogFighter, $MineDamageType]        = 1.0;
$DamageScale[DogFighter, $SniperDamageType] = 1.0;
$DamageScale[DogFighter, $PsiDamageType] = 1.0;
$DamageScale[DogFighter, $ChemDamageType] = 1.0;
$DamageScale[DogFighter, $KrakenDamageType] = 1.0;
$DamageScale[DogFighter, $MeltaDamageType] = 1.0;
$DamageScale[DogFighter, $DeathDamageType] = 1.0;
$DamageScale[DogFighter, $DDamageType] = 1.0;
$DamageScale[DogFighter, $FlamerDamageType] = 1.0;
$DamageScale[DogFighter, $ShellDamageType] = 1.0;
$DamageScale[DogFighter, $ShurikenDamageType] = 1.0;
$DamageScale[DogFighter, $ReaperDamageType] = 1.0;

function vehicleDogFighter::Initialize()
{
  $TeamItemCount[0 @ DogFighterVehicle] = 0;
  $TeamItemCount[1 @ DogFighterVehicle] = 0;
  $TeamItemCount[2 @ DogFighterVehicle] = 0;
  $TeamItemCount[3 @ DogFighterVehicle] = 0;
  $TeamItemCount[4 @ DogFighterVehicle] = 0;
  $TeamItemCount[5 @ DogFighterVehicle] = 0;
  $TeamItemCount[6 @ DogFighterVehicle] = 0;
  $TeamItemCount[7 @ DogFighterVehicle] = 0;
}

RocketData FusionZap
{
  bulletShapeName = "paint.dts";
  explosionTag = turretExp;
  isVisible = False;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.13;
  damageType = $BlasterDamageType;
  explosionRadius = 5.0;
  kickBackStrength = 40.0;
  muzzleVelocity = 350.0;
  terminalVelocity = 450.0;
  acceleration = 5.0;
  totalTime = 10.0;
  liveTime = 10.0;
  lightRange = 5.0;
  lightColor = { 0.4, 0.4, 1.0 };
  inheritedVelocityScale = 0.5;
  trailType = 1;
  trailLength = 60;
  trailWidth = 2.0;
  soundId = SoundDiscSpin;
}
;

ItemData DogFighterVehicle 
{
  description = "Vyper Var.3";
  className = "Vehicle";
  heading = $InvHead[ihVeh];
  price = 30;
};
FlierData DogFighter
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
   	shapeFile = "vyper";
   	shieldShapeName = "shield_medium";
   	mass = 9.0;
   	drag = 1.0;
   	density = 1;
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

	projectileType = FusionZap;
	reloadDelay = 0.1;
	repairRate = 0;
	fireSound = SoundFireLaser;
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

function DogFighter::onPilot(%this, %player)
{
  //
}

function DogFighter::onUnPilot(%this, %player)
{
  //
}