//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Landspeeder Var.3 
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$VehicleInvList[SpeederVehicle] = 1;
$DataBlockName[SpeederVehicle] = Speeder;
$VehicleToItem[Speeder] = SpeederVehicle;
$VehicleSlots[Speeder] = 2;

$TeamItemMax[SpeederVehicle] = 2;

$DamageScale[Speeder, $ImpactDamageType] = 1.0;
$DamageScale[Speeder, $BulletDamageType] = 1.0;
$DamageScale[Speeder, $PlasmaDamageType] = 1.0;
$DamageScale[Speeder, $EnergyDamageType] = 1.0;
$DamageScale[Speeder, $ExplosionDamageType] = 1.0;
$DamageScale[Speeder, $ShrapnelDamageType] = 1.0;
$DamageScale[Speeder, $DebrisDamageType] = 1.0;
$DamageScale[Speeder, $MissileDamageType] = 0.4;
$DamageScale[Speeder, $LaserDamageType] = 1.0;
$DamageScale[Speeder, $MortarDamageType] = 1.0;
$DamageScale[Speeder, $BlasterDamageType] = 1.0;
$DamageScale[Speeder, $ElectricityDamageType] = 1.0;
$DamageScale[Speeder, $MineDamageType]        = 1.0;
$DamageScale[Speeder, $SniperDamageType]        = 1.0;
$DamageScale[Speeder, $PsiDamageType] = 1.0;
$DamageScale[Speeder, $ChemDamageType] = 1.0;
$DamageScale[Speeder, $KrakenDamageType] = 1.0;
$DamageScale[Speeder, $MeltaDamageType] = 1.0;
$DamageScale[Speeder, $DeathDamageType] = 1.0;
$DamageScale[Speeder, $DDamageType] = 1.0;
$DamageScale[Speeder, $FlamerDamageType] = 1.0;
$DamageScale[Speeder, $ShellDamageType] = 1.0;
$DamageScale[Speeder, $ShurikenDamageType] = 1.0;
$DamageScale[Speeder, $ReaperDamageType] = 1.0;

function vehicleSpeeder::Initialize()
{
  $TeamItemCount[0 @ SpeederVehicle] = 0;
  $TeamItemCount[1 @ SpeederVehicle] = 0;
  $TeamItemCount[2 @ SpeederVehicle] = 0;
  $TeamItemCount[3 @ SpeederVehicle] = 0;
  $TeamItemCount[4 @ SpeederVehicle] = 0;
  $TeamItemCount[5 @ SpeederVehicle] = 0;
  $TeamItemCount[6 @ SpeederVehicle] = 0;
  $TeamItemCount[7 @ SpeederVehicle] = 0;
}

BulletData MeltaJet 
{
  bulletShapeName = "fusionbolt.dts";
  explosionTag = energyExp;
  damageClass = 0;
  damageValue = 0.65;
  damageType = $MeltaDamageType;
  muzzleVelocity = 100.0;
  totalTime = 3.0;
  liveTime = 3.0;
  lightRange = 3.0;
  lightColor = { 1, 1, 0 };
  inheritedVelocityScale = 0.3;
  isVisible = True;
  soundId = SoundJetLight;
};

ItemData SpeederVehicle 
{
  description = "Land Speeder Var.3";
  className = "Vehicle";
  heading = $InvHead[ihVeh];
  price = 50;
};

FlierData Speeder
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
   	shapeFile = "hover_apc_sml";
   	shieldShapeName = "shield_large";
   	mass = 16.0;
   	drag = 1.0;
   	density = 1.2;
   	maxBank = 0.6;
   	maxPitch = 0.6;
  	maxSpeed = 40;
  	minSpeed = -25;
	lift = 0.9;
	maxAlt = 25;
	maxVertical = 10;
	maxDamage = 1.5;
	damageLevel = {1.0, 1.0};
	destroyDamage = 1.0;
	maxEnergy = 100;
	accel = 0.25;

	groundDamageScale = 0.50;

	projectileType = MeltaJet;
	reloadDelay = 0.4;
	repairRate = 0;
	fireSound = SoundFireBlaster;
	damageSound = SoundFlierCrash;

	ramDamage = 2;
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
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

function Speeder::onPilot(%this, %player)
{
  //
}

function Speeder::onUnPilot(%this, %player)
{
  //
}