//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Vyper: modded form Interceptor(renegades)
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$VehicleInvList[InterceptorVehicle] = 1;
$DataBlockName[InterceptorVehicle] = Interceptor;
$VehicleToItem[Interceptor] = InterceptorVehicle;
$ItemToVehicle[InterceptorVehicle] = Interceptor;
//$VehicleSlots[Interceptor] = 1;

$TeamItemMax[InterceptorVehicle] = 6;

$DamageScale[Interceptor, $ImpactDamageType] = 1.0;
$DamageScale[Interceptor, $BulletDamageType] = 1.0;
$DamageScale[Interceptor, $PlasmaDamageType] = 1.0;
$DamageScale[Interceptor, $EnergyDamageType] = 1.0;
$DamageScale[Interceptor, $ExplosionDamageType] = 1.0;
$DamageScale[Interceptor, $ShrapnelDamageType] = 1.0;
$DamageScale[Interceptor, $DebrisDamageType] = 1.0;
$DamageScale[Interceptor, $MissileDamageType] = 1.0;
$DamageScale[Interceptor, $LaserDamageType] = 1.0;
$DamageScale[Interceptor, $MortarDamageType] = 1.0;
$DamageScale[Interceptor, $BlasterDamageType] = 0.5;
$DamageScale[Interceptor, $ElectricityDamageType] = 1.0;
$DamageScale[Interceptor, $MineDamageType] = 1.0;
$DamageScale[Interceptor, $SniperDamageType] = 1.0;
$DamageScale[Interceptor, $PsiDamageType] = 1.0;
$DamageScale[Interceptor, $ChemDamageType] = 1.0;
$DamageScale[Interceptor, $KrakenDamageType] = 1.0;
$DamageScale[Interceptor, $MeltaDamageType] = 1.0;
$DamageScale[Interceptor, $DeathDamageType] = 1.0;
$DamageScale[Interceptor, $DDamageType] = 1.0;
$DamageScale[Interceptor, $FlamerDamageType] = 1.0;
$DamageScale[Interceptor, $ShellDamageType] = 1.0;
$DamageScale[Interceptor, $ShurikenDamageType] = 1.0;
$DamageScale[Interceptor, $ReaperDamageType] = 1.0;

function vehicleInterceptor::Initialize()
{
  $TeamItemCount[0 @ InterceptorVehicle] = 0;
  $TeamItemCount[1 @ InterceptorVehicle] = 0;
  $TeamItemCount[2 @ InterceptorVehicle] = 0;
  $TeamItemCount[3 @ InterceptorVehicle] = 0;
  $TeamItemCount[4 @ InterceptorVehicle] = 0;
  $TeamItemCount[5 @ InterceptorVehicle] = 0;
  $TeamItemCount[6 @ InterceptorVehicle] = 0;
  $TeamItemCount[7 @ InterceptorVehicle] = 0;
}

ItemData InterceptorVehicle 
{
  description = "Vyper";
  className = "Vehicle";
  heading = $InvHead[ihVeh];
  price = 50;
};

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

RocketData JetPlasmaMissile 
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

BulletData VulcanIntBullet 
{
 bulletShapeName = "discb.dts";
  explosionTag = bulletExp0;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 0;
  damageValue = 0.2;
  damageType = $ShurikenDamageType;
  kickBackStrength = 0.0;
  muzzleVelocity = 685.0;
  terminalVelocity = 685.0;
  acceleration = 20.0;
  totalTime = 6.5;
  liveTime = 6.5;
  lightRange = 5.0;
  lightColor = { 0.4, 0.4, 1.0 };
  inheritedVelocityScale = 1.0;
  trailType = 1;
  trailLength = 15;
  trailWidth = 0.3;
  soundId = SoundDiscSpin;
};

FlierData Interceptor 
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
//  projectileType = VulcanIntBullet;
  reloadDelay = 0.2;
  repairRate = 0;
  fireSound = SoundFireMortar;
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

function Interceptor::onPilot(%this, %player)
{
  //
}

function Interceptor::onUnPilot(%this, %player)
{
  //
}

function Interceptor::onFire(%vehicle, %slot) 
{
		 %trans = GameBase::getMuzzleTransform(%vehicle);
		   if(%vehicle.lastfire == "") %vehicle.lastfire = 0;
//			for(%i=0; %i<12; %i++) %weptemp[%i] = getword(%trans, %i);
//			%weptemp[8] -= 2;
//			%trans = %weptemp[0] @ " " @ %weptemp[1] @ " " @ %weptemp[2] @ " " @ %weptemp[3] @ " " @ %weptemp[4] @ " " @ %weptemp[5] @ " " @ %weptemp[6] @ " " @ %weptemp[7] @ " " @ %weptemp[8] @ " " @ %weptemp[9] @ " " @ %weptemp[10] @ " " @ %weptemp[11];
		 %vel = Item::getVelocity(%vehicle);
			if (%vehicle.weap == 0)
			{
				Projectile::spawnProjectile("VulcanIntBullet",%trans,%vehicle,%vel);
			}
			if (%vehicle.weap == 1)
			{
				%time = getIntegerTime(true) >> 5;
				%diff = %time - %vehicle.lastfire;
				if (%diff > 1.0) 
				{
					Projectile::spawnProjectile("JetPlasmaMissile",%trans,%vehicle,%vel);
					%vehicle.lastfire = %time;
				}
			}
			if (%vehicle.weap == 2)
			{
				%time = getIntegerTime(true) >> 5;
				%diff = %time - %vehicle.lastfire;
				if (%diff > 0.13) 
				{
					Projectile::spawnProjectile("FusionZap",%trans,%vehicle,%vel);
					%vehicle.lastfire = %time;
				}
			}
}

