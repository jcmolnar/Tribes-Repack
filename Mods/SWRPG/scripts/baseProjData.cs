$ImpactDamageType		= -1;
$LandingDamageType	=  0;
$BulletDamageType		=  1;
$EnergyDamageType		=  2;
$PlasmaDamageType		=  3;
$ExplosionDamageType	=  4;
$ShrapnelDamageType	=  5;
$LaserDamageType		=  6;
$MortarDamageType		=  7;
$BlasterDamageType	=  8;
$ElectricityDamageType	=  9;
$CrushDamageType		= 10;
$DebrisDamageType		= 11;
$MissileDamageType	= 12;
$MineDamageType		= 13;
$NullDamageType		= 14;
$SpellDamageType		= 15;

//--------------------------------------
RocketData LandingSnow
{
   bulletShapeName  = "";
   explosionTag     = LandingDustExp1;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 0;
   damageValue      = 0.0;
   damageType       = $LandingDamageType; //does not actually matter, this isn't determined here. I put it here just because.

   explosionRadius  = 0;
   kickBackStrength = 0;
   muzzleVelocity   = 0;
   terminalVelocity = 0;
   acceleration     = 0;
   totalTime        = 0;
   liveTime         = 1;
   inheritedVelocityScale = 0;
};

//--------------------------------------
RocketData LandingDust
{
   bulletShapeName  = "";
   explosionTag     = LandingDustExp;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 0;
   damageValue      = 0.0;
   damageType       = $LandingDamageType;

   explosionRadius  = 0;
   kickBackStrength = 0;
   muzzleVelocity   = 0;
   terminalVelocity = 0;
   acceleration     = 0;
   totalTime        = 0;
   liveTime         = 1;
   inheritedVelocityScale = 0;
};

//--------------------------------------
BulletData BlasterRepeaterBullet
{
   bulletShapeName    = "blastred.dts";
   explosionTag       = bulletExp0;//redblast
   expRandCycle       = 1;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.15;
   damageType         = $BulletDamageType;

   aimDeflection      = 0.01;
   muzzleVelocity     = 250.0;
   totalTime          = 1.5;
   inheritedVelocityScale = 1.0;
   isVisible          = true;

   tracerPercentage   = 1.0;
   tracerLength       = 60;
};

//--------------------------------------
BulletData Flak
{
   bulletShapeName    = "bullet.dts";
   explosionTag       = bulletExp0;
   expRandCycle       = 3;
   mass               = 0.03;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.09;
   damageType         = $BulletDamageType;

   aimDeflection      = 0.035;
   muzzleVelocity     = 600.0;
   totalTime          = 0.75;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 1.0;
   tracerLength       = 30;
};

//--------------------------------------
RocketData gunsmoke
{
   bulletShapeName  = "";
   explosionTag     = gunsmokeExp;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 0;       // 0 impact, 1, radius
   damageValue      = 0.0;     // 8 normal
   damageType       = $MortarDamageType;


   explosionRadius  = 0.0;
   kickBackStrength = 0.0;
   muzzleVelocity   = 100.0;
   terminalVelocity = 100.0;
   acceleration     = 0;
   totalTime        = 0.010;
   liveTime         = 0.010;
   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };
   inheritedVelocityScale = 1.0;
   soundId = none;
};

//--------------------------------------
BulletData BlasterRifleBullet
{
   bulletShapeName    = "blastred.dts";
   explosionTag       = bulletExp0;//redblast
   mass               = 0.05;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.15;
   damageType         = $LaserDamageType;

   muzzleVelocity     = 750.0;
   totalTime          = 6.0;
   liveTime           = 4.0;
   isVisible          = True;

   rotationPeriod = 1.5;
};

//--------------------------------------
BulletData BlasterGuardGunBullet
{
   bulletShapeName    = "blastgrn.dts";
   explosionTag       = bulletExp0;//grnblast
   mass               = 0.05;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.25;
   damageType         = $LaserDamageType;

   muzzleVelocity     = 750.0;
   totalTime          = 6.0;
   liveTime           = 4.0;
   isVisible          = True;
  lightRange        = 2.0;
   lightColor        = { 0.25, 1.0, 0.25 };
   rotationPeriod = 1.5;
};

//--------------------------------------
RocketData BlasterDesertRifleBullet
{
   bulletShapeName  = "";
   explosionTag     = DesertExp;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 0;       // 0 impact, 1, radius
   damageValue      = 0.4;     // 8 normal
   damageType       = $MortarDamageType;


   explosionRadius  = 0.0;
   kickBackStrength = 0.0;
   muzzleVelocity   = 9000.0;
   terminalVelocity = 9000.0;
   acceleration     = 0;
   totalTime        = 15.0;
   liveTime         = 16.0;
   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };
   inheritedVelocityScale = 1.0;
   soundId = SoundJetHeavy;
};

//--------------------------------------
RocketData TurretBlast
{
   bulletShapeName = "blastred.dts";
   explosionTag    = redblast;

   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 0;       // 0 impact, 1, radius
   damageValue      = 0.5;
   damageType       = $LaserDamageType;

   muzzleVelocity   = 300.0;
   terminalVelocity = 300.0;
   acceleration     = 50.0;

   totalTime        = 5.0;
   liveTime         = 4.0;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };
   isVisible          = True;
   inheritedVelocityScale = 1.0;
   soundId = SoundJetLight;
//   // rocket specific
 //  trailType   = 1;
  // trailLength = 60;
   // trailWidth  = 1.0;
 
  };

//--------------------------------------
RocketData Blasterred
{
	bulletShapeName = "blastred.dts";
	explosionTag    = blasterExp;

	collisionRadius = 0.0;
	mass            = 2.0;

	damageClass      = 0;
	damageValue      = 0.35;
	damageType       = $LaserDamageType;

	muzzleVelocity   = 200.0;
	terminalVelocity = 200.0;
	acceleration     = 50.0;

	totalTime        = 5.0;
	liveTime         = 4.0;

	lightRange        = 2.0;
	lightColor        = { 1, 0.25, 0.25 };

	isVisible          = True;
	inheritedVelocityScale = 1.0;
	soundId = SoundJetLight;
 };

//--------------------------------------
RocketData Blastergreen
{
	bulletShapeName = "blastgrn.dts";
	explosionTag    = greenBlasterExp;

	collisionRadius = 0.0;
	mass            = 2.0;

	damageClass      = 0;
	damageValue      = 0.35;
	damageType       = $LaserDamageType;

	muzzleVelocity   = 200.0;
	terminalVelocity = 200.0;
	acceleration     = 50.0;

	totalTime        = 5.0;
	liveTime         = 4.0;

	lightRange        = 2.0;
	lightColor        = { 0.25, 1, 0.25 };

	isVisible          = True;
	inheritedVelocityScale = 1;
	soundId = SoundJetLight;
};

//--------------------------------------
RocketData Blasterblue
{
	bulletShapeName = "breath.dts";
	explosionTag    = blublast;

	collisionRadius = 0.0;
	mass            = 2.0;

	damageClass      = 0;
	damageValue      = 0.35;
	damageType       = $LaserDamageType;

	muzzleVelocity   = 200.0;
	terminalVelocity = 200.0;
	acceleration     = 50.0;

	totalTime        = 5.0;
	liveTime         = 4.0;

	lightRange        = 2.0;
	lightColor        = { 0.25, 0.25, 1 };

	isVisible          = True;
	inheritedVelocityScale = 1;

	trailType   = 1;
	trailLength = 5;
	trailWidth  = 0.3;

	soundId = SoundJetLight;
};

//--------------------------------------
RocketData XWINGBlast
{
	bulletShapeName = "blastvehred.dts";
	explosionTag = rvblast;

	collisionRadius = 0.0;
	mass = 5.0;

	damageClass      = 0;
	damageValue      = 0.35;
	damageType       = $AWINGDamageType;

	muzzleVelocity   = 400.0;
	terminalVelocity = 400.0;
	acceleration     = 2.0;

	totalTime        = 3.0;
	liveTime         = 3.0;

	lightRange        = 4.0;
	lightColor        = { 1.0, 0.25, 0.25 };
	isVisible          = True;
	inheritedVelocityScale = 1.0;
	soundId = SoundJetLight;
};

//--------------------------------------
RocketData TIEBlast
{
	bulletShapeName = "blastvehgrn.dts";
	explosionTag    = gvblast;

	collisionRadius = 0.0;
	mass = 5.0;

	damageClass = 0;
	damageValue = 0.35;
	damageType = $TIEDamageType;

	muzzleVelocity = 400.0;
	terminalVelocity = 400.0;
	acceleration = 2.0;

	totalTime = 3.0;
	liveTime = 3.0;

	lightRange = 4.0;
	lightColor = { 0.25, 1.0, 0.25 };
	isVisible = True;
	inheritedVelocityScale = 1.0;
	soundId = SoundJetLight; 
};

//--------------------------------------
GrenadeData TIEBomberShell
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = mortarExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.3;
   mass               = 5.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 2.0;
   damageType         = $TIEBOMBDamageType;

   explosionRadius    = 50.0;
   kickBackStrength   = 650.0;
   maxLevelFlightDist = 5;
   totalTime          = 8.0;
   liveTime           = 0.4;
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 0.5;
   trailType   = 1;
   trailLength = 60;
   trailWidth  = 1.0;

   smokeName              = "fusionex.dts";
};

//--------------------------------------
BulletData FusionBolt
{
   bulletShapeName    = "fusionbolt.dts";
   explosionTag       = turretExp;
   mass               = 0.05;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.25;
   damageType         = $EnergyDamageType;

   muzzleVelocity     = 50.0;
   totalTime          = 6.0;
   liveTime           = 4.0;
   isVisible          = True;

   rotationPeriod = 1.5;
};

//--------------------------------------
BulletData MiniFusionBolt
{
   bulletShapeName    = "enbolt.dts";
   explosionTag       = energyExp;

   damageClass        = 0;
   damageValue        = 0.1;
   damageType         = $EnergyDamageType;

   muzzleVelocity     = 80.0;
   totalTime          = 4.0;
   liveTime           = 2.0;

   lightRange         = 3.0;
   lightColor         = { 0.25, 0.25, 1.0 };
   //inheritedVelocityScale = 0.5;
   isVisible          = True;

   rotationPeriod = 1;
};
function MiniFusionBolt::onAdd(%this)
{
}

//--------------------------------------
GrenadeData MortarTurretShell
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = mortarExp;
   collideWithOwner   = True;
   ownerGraceMS       = 400;
   collisionRadius    = 1.0;
   mass               = 5.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 1.32;
   damageType         = $NullDamageType;

   explosionRadius    = 30.0;
   kickBackStrength   = 250.0;
   maxLevelFlightDist = 400;
   totalTime          = 1000.0;
   liveTime           = 2.0;
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 0.5;
   smokeName              = "mortartrail.dts";
};

//--------------------------------------
RocketData FlierRocket
{
   bulletShapeName  = "rocket.dts";
   explosionTag     = rocketExp;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.5;
   damageType       = $MissileDamageType;

   explosionRadius  = 9.5;
   kickBackStrength = 250.0;
   muzzleVelocity   = 65.0;
   terminalVelocity = 80.0;
   acceleration     = 5.0;
   totalTime        = 10.0;
   liveTime         = 11.0;
   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };
   //inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "rsmoke.dts";
   smokeDist   = 1.8;

   soundId = SoundJetHeavy;
};

//--------------------------------------
SeekingMissileData TurretMissile
{
   bulletShapeName = "rocket.dts";
   explosionTag    = rocketExp;
   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.5;
   damageType       = $MissileDamageType;
   explosionRadius  = 9.5;
   kickBackStrength = 175.0;

   muzzleVelocity    = 72.0;
   totalTime         = 10;
   liveTime          = 10;
   seekingTurningRadius    = 9;
   nonSeekingTurningRadius = 75.0;
   proximityDist     = 1.5;
   smokeDist         = 1.75;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

   soundId = SoundJetHeavy;
};

//--------------------------------------
//--------------------------------------
//--------------------------------------
//Spell projectile datas.
//--------------------------------------
//--------------------------------------
//--------------------------------------

LaserData sniperLaser
{
	laserBitmapName   = "forcefield.bmp";
	hitName           = "laserhit.dts";

	damageConversion  = 0.0;
	baseDamageType    = $LaserDamageType;

 	beamTime          = 1.5;

	lightRange        = 10.0;
	lightColor        = { 0.2, 0.2, 1.0 };

	detachFromShooter = false;
	hitSoundId        = NoSound;
};

function SeekingMissile::updateTargetPercentage(%target)
{
	dbecho($dbechoMode, "SeekingMissile::updateTargetPercentage(" @ %target @ ")");

	return GameBase::virtual(%target, "getHeatFactor");
}

//--------------------------------------
//Force Lightning
//--------------------------------------

LightningData lightning1
{
   bitmapName       = "lightningNew.bmp";

   damageType       = $ElectricityDamageType;
   boltLength       = 40.0;
   coneAngle        = 35.0;
   damagePerSec      = 0.06;
   energyDrainPerSec = 60.0;
   segmentDivisions = 4;
   numSegments      = 1;
   beamWidth        = 0.08;//075;

   updateTime   = 120;
   skipPercent  = 0.5;
   displaceBias = 0.15;

   lightRange = 3.0;
   lightColor = { 0.25, 0.25, 0.85 };

   soundId = SoundELFFire;
};

LightningData lightning2
{
   bitmapName       = "lightningNew.bmp";

   damageType       = $ElectricityDamageType;
   boltLength       = 40.0;
   coneAngle        = 35.0;
   damagePerSec      = 0.06;
   energyDrainPerSec = 60.0;
   segmentDivisions = 4;
   numSegments      = 2;
   beamWidth        = 0.09;//075;

   updateTime   = 120;
   skipPercent  = 0.5;
   displaceBias = 0.15;

   lightRange = 3.0;
   lightColor = { 0.25, 0.25, 0.85 };

   soundId = SoundELFFire;
};

LightningData lightning3
{
   bitmapName       = "lightningNew.bmp";

   damageType       = $ElectricityDamageType;
   boltLength       = 40.0;
   coneAngle        = 35.0;
   damagePerSec      = 0.06;
   energyDrainPerSec = 60.0;
   segmentDivisions = 4;
   numSegments      = 3;
   beamWidth        = 0.1;//075;

   updateTime   = 120;
   skipPercent  = 0.5;
   displaceBias = 0.15;

   lightRange = 3.0;
   lightColor = { 0.25, 0.25, 0.85 };

   soundId = SoundELFFire;
};

LightningData lightning4
{
   bitmapName       = "lightningNew.bmp";

   damageType       = $ElectricityDamageType;
   boltLength       = 40.0;
   coneAngle        = 35.0;
   damagePerSec      = 0.06;
   energyDrainPerSec = 60.0;
   segmentDivisions = 4;
   numSegments      = 4;
   beamWidth        = 0.11;//075;

   updateTime   = 120;
   skipPercent  = 0.5;
   displaceBias = 0.15;

   lightRange = 3.0;
   lightColor = { 0.25, 0.25, 0.85 };

   soundId = SoundELFFire;
};

LightningData lightning5
{
   bitmapName       = "lightningNew.bmp";

   damageType       = $ElectricityDamageType;
   boltLength       = 40.0;
   coneAngle        = 35.0;
   damagePerSec      = 0.06;
   energyDrainPerSec = 60.0;
   segmentDivisions = 4;
   numSegments      = 5;
   beamWidth        = 0.12;//075;

   updateTime   = 120;
   skipPercent  = 0.5;
   displaceBias = 0.15;

   lightRange = 3.0;
   lightColor = { 0.25, 0.25, 0.85 };

   soundId = SoundELFFire;
};
//--------------------------------------
//Force Drain //Use the repair bolts for this? o0
//--------------------------------------

LightningData turretCharge
{
   bitmapName       = "lightningNew.bmp";

   damageType       = $ElectricityDamageType;
   boltLength       = 40.0;
   coneAngle        = 35.0;
   damagePerSec      = 0.06;
   energyDrainPerSec = 60.0;
   segmentDivisions = 4;
   numSegments      = 5;
   beamWidth        = 0.125;

   updateTime   = 120;
   skipPercent  = 0.5;
   displaceBias = 0.15;

   lightRange = 3.0;
   lightColor = { 0.25, 0.25, 0.85 };

   soundId = SoundELFFire;
};

function Lightning::damageTarget(%target, %timeSlice, %damPerSec, %enDrainPerSec, %pos, %vec, %mom, %shooterId)
{
	dbecho($dbechoMode, "Lightning::damageTarget(" @ %target @ ", " @ %timeSlice @ ", " @ %damPerSec @ ", " @ %enDrainPerSec @ ", " @ %pos @ ", " @ %vec @ ", " @ %mom @ ", " @ %shooterId @ ")");

   %damVal = %timeSlice * %damPerSec;
   %enVal  = %timeSlice * %enDrainPerSec;

   GameBase::applyDamage(%target, $ElectricityDamageType, %damVal, %pos, %vec, %mom, %shooterId);

   %energy = GameBase::getEnergy(%target);
   %energy = %energy - %enVal;
   if (%energy < 0) {
      %energy = 0;
   }
   GameBase::setEnergy(%target, %energy);
}

RepairEffectData RepairBolt
{
   bitmapName       = "repairadd.bmp";
   boltLength       = 5.0;
   segmentDivisions = 4;
   beamWidth        = 0.125;

   updateTime   = 450;
   skipPercent  = 0.6;
   displaceBias = 0.15;

   lightRange = 3.0;
   lightColor = { 0.85, 0.25, 0.25 };
};

function RepairBolt::onAcquire(%this, %player, %target)
{
	%client = Player::getClient(%player);

	if (%target == %player) {
	   %player.repairTarget = -1;
		if (GameBase::getDamageLevel(%player) != 0) {
			%player.repairRate = 0.05;
			%player.repairTarget = %player;
			Client::sendMessage(%client, 0, "AutoRepair On");
		}
		else {
			Client::sendMessage(%client,0,"Nothing in range");
			Player::trigger(%player, $WeaponSlot, false);
			return;
		}
	}
	else {
      %player.repairTarget = %target;
		%player.repairRate   = 0.1;
		if (getObjectType(%player.repairTarget) == "Player") {
			%rclient = Player::getClient(%player.repairTarget);
			%name = Client::getName(%rclient);
		}
		else { 
			%name = GameBase::getMapName(%target);
			if(%name == "") {
				%name = (GameBase::getDataName(%player.repairTarget)).description;
			}
		}
		if (GameBase::getDamageLevel(%player.repairTarget) == 0) {
			Client::sendMessage(%client,0,%name @ " is not damaged");
			Player::trigger(%player,$WeaponSlot,false);
			%player.repairTarget = -1;
			return;
		}
		if (getObjectType(%player.repairTarget) == "Player") {
			Client::sendMessage(%rclient,0,"Being repaired by " @ Client::getName(%client));
		}
		Client::sendMessage(%client,0,"Repairing " @ %name);
	}
	%rate = GameBase::getAutoRepairRate(%player.repairTarget) + %player.repairRate;
	GameBase::setAutoRepairRate(%player.repairTarget,%rate);
}

function RepairBolt::onRelease(%this, %player)
{
	%object = %player.repairTarget;
	if (%object != -1) {
		%client = Player::getClient(%player);
		if (%object == %player) {
			Client::sendMessage(%client,0,"AutoRepair Off");
		}
		else {
			if (GameBase::getDamageLevel(%object) == 0) {
				Client::sendMessage(%client,0,"Repair Done");
			}
			else {
				Client::sendMessage(%client,0,"Repair Stopped");
			}
		}
		%rate = GameBase::getAutoRepairRate(%object) - %player.repairRate;
      if (%rate < 0)
         %rate = 0;
      
		GameBase::setAutoRepairRate(%object,%rate);
	}
}

function RepairBolt::checkDone(%this, %player)
{
	if (Player::isTriggered(%player,$WeaponSlot) && 
       Player::getMountedItem(%player,$WeaponSlot) == RepairGun &&
		 %player.repairTarget != -1) {
		%object = %player.repairTarget;
		if (%object == %player) {
			if (GameBase::getDamageLevel(%player) == 0) {
				Player::trigger(%player,$WeaponSlot,false);
				return;
			}
		}
		else {
			if (GameBase::getDamageLevel(%object) == 0) {
				Player::trigger(%player,$WeaponSlot,false);
				return;
			}
		}
	}
}