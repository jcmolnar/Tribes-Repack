//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Land speeder
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$VehicleInvList[LAPCVehicle] = 1;
$DataBlockName[LAPCVehicle] = LAPC;
$VehicleToItem[LAPC] = LAPCVehicle;
$ItemToVehicle[LAPCVehicle] = LAPC;
$VehicleSlots[LAPC] = 2;

$TeamItemMax[LAPCVehicle] = 8;

$DamageScale[LAPC, $ImpactDamageType] = 1.0;
$DamageScale[LAPC, $BulletDamageType] = 1.0;
$DamageScale[LAPC, $PlasmaDamageType] = 1.0;
$DamageScale[LAPC, $EnergyDamageType] = 1.0;
$DamageScale[LAPC, $ExplosionDamageType] = 1.0;
$DamageScale[LAPC, $ShrapnelDamageType] = 1.0;
$DamageScale[LAPC, $DebrisDamageType] = 1.0;
$DamageScale[LAPC, $MissileDamageType] = 0.4;
$DamageScale[LAPC, $LaserDamageType] = 0.5;
$DamageScale[LAPC, $MortarDamageType] = 0.7;
$DamageScale[LAPC, $BlasterDamageType] = 0.5;
$DamageScale[LAPC, $ElectricityDamageType] = 1.0;
$DamageScale[LAPC, $MineDamageType] = 1.0;
$DamageScale[LAPC, $SniperDamageType] = 1.0;
$DamageScale[LAPC, $PsiDamageType] = 1.0;
$DamageScale[LAPC, $ChemDamageType] = 1.0;
$DamageScale[LAPC, $KrakenDamageType] = 1.0;
$DamageScale[LAPC, $MeltaDamageType] = 1.0;
$DamageScale[LAPC, $DeathDamageType] = 1.0;
$DamageScale[LAPC, $DDamageType] = 1.0;
$DamageScale[LAPC, $FlamerDamageType] = 1.0;
$DamageScale[LAPC, $ShellDamageType] = 1.0;
$DamageScale[LAPC, $ShurikenDamageType] = 1.0;
$DamageScale[LAPC, $ReaperDamageType] = 1.0;

function vehicleLAPC::Initialize()
{
  $TeamItemCount[0 @ LAPCVehicle] = 0;
  $TeamItemCount[1 @ LAPCVehicle] = 0;
  $TeamItemCount[2 @ LAPCVehicle] = 0;
  $TeamItemCount[3 @ LAPCVehicle] = 0;
  $TeamItemCount[4 @ LAPCVehicle] = 0;
  $TeamItemCount[5 @ LAPCVehicle] = 0;
  $TeamItemCount[6 @ LAPCVehicle] = 0;
  $TeamItemCount[7 @ LAPCVehicle] = 0;
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

BulletData ChainBullet 
{
  bulletShapeName = "bullet.dts";
  explosionTag = bulletExp0;
  expRandCycle = 3;
  mass = 0.05;
  bulletHoleIndex = 0;
  damageClass = 0;
  damageValue = 0.1;
  damageType = $BulletDamageType;
  aimDeflection = 0.003;
  muzzleVelocity = 425.0;
  totalTime = 1.5;
  inheritedVelocityScale = 1.0;
  isVisible = False;
  tracerPercentage = 1.0;
  tracerLength = 30;
};

ItemData LAPCVehicle 
{
  description = "Land Speeder";
  className = "Vehicle";
  heading = $InvHead[ihVeh];
  price = 50;
};

FlierData LAPC 
{
  explosionId = flashExpLarge;
  debrisId = flashDebrisLarge;
  className = "Vehicle";
  shapeFile = "hover_apc_sml";
  shieldShapeName = "shield_large";
  mass = 18.0;
  drag = 1.0;
  density = 1.2;
  maxBank = 0.6;
  maxPitch = 0.6;
  maxSpeed = 40;
  minSpeed = -25;
  lift = 0.9;
  maxAlt = 15;
  maxVertical = 9;
  maxDamage = 1.5;
  damageLevel = {1.0, 1.0};
  destroyDamage = 1.0;
  maxEnergy = 100;
  accel = 0.25;
  groundDamageScale = 0.50;
  repairRate = 0;
  ramDamage = 2;
  ramDamageType = -1;
  mapFilter = 2;
  mapIcon = "M_vehicle";
//  projectileType = ChainBullet;
  fireSound = SoundFireBlaster;
  reloadDelay = 0.05;
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

function LAPC::onPilot(%this, %player)
{
  //
}

function LAPC::onUnPilot(%this, %player)
{
  //
}

function LAPC::onFire(%vehicle, %slot) 
{
	%trans = GameBase::getmuzzleTransform(%vehicle);
	%rotater = gamebase::getrotation(%vehicle);
	%vect = Vector::getFromRot(%rotater, 7, -2);
	for(%i=0; %i<3; %i++) %vecttemp[%i] = getword(%vect, %i);

	if(%vehicle.lastfire == "") %vehicle.lastfire = 0;
	for(%i=0; %i<12; %i++) %weptemp[%i] = getword(%trans, %i);
	%weptemp[9] += %vecttemp[0];
	%weptemp[10] += %vecttemp[1];
	%weptemp[11] += %vecttemp[2];
	%trans = %weptemp[0] @ " " @ %weptemp[1] @ " " @ %weptemp[2] @ " " @ %weptemp[3] @ " " @ %weptemp[4] @ " " @ %weptemp[5] @ " " @ %weptemp[6] @ " " @ %weptemp[7] @ " " @ %weptemp[8] @ " " @ %weptemp[9] @ " " @ %weptemp[10] @ " " @ %weptemp[11];
	%vel = Item::getVelocity(%vehicle);
	if (%vehicle.weap == 0)
	{
		Projectile::spawnProjectile("ChainBullet",%trans,%vehicle,%vel);
	}
	if (%vehicle.weap == 1)
	{
		Projectile::spawnProjectile("MegaFlame",%trans,%vehicle,%vel);
	}
	if (%vehicle.weap == 2)
	{
		%time = getIntegerTime(true) >> 5;
		%diff = %time - %vehicle.lastfire;
		if (%diff > 0.8) 
		{
			Projectile::spawnProjectile("MeltaJet",%trans,%vehicle,%vel);
			%vehicle.lastfire = %time;
		}
	}
}

