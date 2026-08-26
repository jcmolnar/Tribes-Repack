//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Land Speeder Variant 2 modded from BOP
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$VehicleInvList[QuickLPCVehicle] = 1;
$DataBlockName[QuickLPCVehicle] = QuickLPC;
$VehicleToItem[QuickLPC] = QuickLPCVehicle;
$VehicleSlots[QuickLPC] = 2;

$TeamItemMax[QuickLPCVehicle] = 2;

$DamageScale[QuickLPC, $ImpactDamageType] = 1.0;
$DamageScale[QuickLPC, $BulletDamageType] = 1.0;
$DamageScale[QuickLPC, $PlasmaDamageType] = 1.0;
$DamageScale[QuickLPC, $EnergyDamageType] = 1.0;
$DamageScale[QuickLPC, $ExplosionDamageType] = 1.0;
$DamageScale[QuickLPC, $ShrapnelDamageType] = 1.0;
$DamageScale[QuickLPC, $DebrisDamageType] = 1.0;
$DamageScale[QuickLPC, $MissileDamageType] = 0.4;
$DamageScale[QuickLPC, $LaserDamageType] = 1.0;
$DamageScale[QuickLPC, $MortarDamageType] = 1.0;
$DamageScale[QuickLPC, $BlasterDamageType] = 1.0;
$DamageScale[QuickLPC, $ElectricityDamageType] = 1.0;
$DamageScale[QuickLPC, $MineDamageType]        = 1.0;
$DamageScale[QuickLPC, $SniperDamageType]        = 1.0;
$DamageScale[QuickLPC, $PsiDamageType] = 1.0;
$DamageScale[QuickLPC, $ChemDamageType] = 1.0;
$DamageScale[QuickLPC, $KrakenDamageType] = 1.0;
$DamageScale[QuickLPC, $MeltaDamageType] = 1.0;
$DamageScale[QuickLPC, $DeathDamageType] = 1.0;
$DamageScale[QuickLPC, $DDamageType] = 1.0;
$DamageScale[QuickLPC, $FlamerDamageType] = 1.0;
$DamageScale[QuickLPC, $ShellDamageType] = 1.0;
$DamageScale[QuickLPC, $ShurikenDamageType] = 1.0;
$DamageScale[QuickLPC, $ReaperDamageType] = 1.0;

function vehicleQuickLPC::Initialize()
{
  $TeamItemCount[0 @ QuickLPCVehicle] = 0;
  $TeamItemCount[1 @ QuickLPCVehicle] = 0;
  $TeamItemCount[2 @ QuickLPCVehicle] = 0;
  $TeamItemCount[3 @ QuickLPCVehicle] = 0;
  $TeamItemCount[4 @ QuickLPCVehicle] = 0;
  $TeamItemCount[5 @ QuickLPCVehicle] = 0;
  $TeamItemCount[6 @ QuickLPCVehicle] = 0;
  $TeamItemCount[7 @ QuickLPCVehicle] = 0;
}

BulletData MegaFlame 
{
  bulletShapeName = "tumult_large.dts";
  explosionTag = plasmaExp;
  damageClass = 1;
  damageValue = 0.2;
  damageType = $PlasmaDamageType;
  explosionRadius = 6.0;
  muzzleVelocity = 30.0;
  totalTime = 3.55;
  liveTime = 3.55;
  lightRange = 3.0;
  lightColor = { 1, 1, 0 };
  inheritedVelocityScale = 0.3;
  isVisible = True;
  soundId = SoundFirePlasma;
};

ItemData QuickLPCVehicle 
{
  description = "Land Speeder Var.2";
  className = "Vehicle";
  heading = $InvHead[ihVeh];
  price = 50;
};

FlierData QuickLPC
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

	projectileType = MegaFlame;
	reloadDelay = 0.05;
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

function QuickLPC::onPilot(%this, %player)
{
  //
}

function QuickLPC::onUnPilot(%this, %player)
{
  //
}