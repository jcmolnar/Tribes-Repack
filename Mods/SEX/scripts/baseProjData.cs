# Damage Types
#
$ImpactDamageType		  = -1;
$LandingDamageType	  =  0;
$BulletDamageType      =  1;
$EnergyDamageType      =  2;
$PlasmaDamageType      =  3;
$ExplosionDamageType   =  4;
$ShrapnelDamageType    =  5;
$LaserDamageType       =  6;
$MortarDamageType      =  7;
$BlasterDamageType     =  8;
$ElectricityDamageType =  9;
$CrushDamageType       = 10;
$DebrisDamageType      = 11;
$MissileDamageType     = 12;
$MineDamageType        = 13;
$MissyDamageType       = 14;
$NoPlayerDamageTYPE	     = 15;
$Kickback = 20;
//-------------------------------------- 
// These are kinda oddball dat's
// the lasers really don't fit into
// the typical projectile catagories...
//--------------------------------------


//drop weapon code fingered out by 


function Lightning::damageTarget(%target, %timeSlice, %damPerSec, %enDrainPerSec, %pos, %vec, %mom, %shooterId)
{
   %damVal = %timeSlice * %damPerSec;
   %enVal  = %timeSlice * %enDrainPerSec;

   GameBase::applyDamage(%target, $ElectricityDamageType, %damVal, %pos, %vec, %mom, %shooterId);
	

   %energy = GameBase::getEnergy(%target);
   %energy = %energy - %enVal;
   if (%energy < 0) {
      

//==================drop weapon code fingered out by Plasmatic
// also edited the throw script in item.cs to make ammo droping more interesting
// all the ifs are to reduce blaster, elf cheats, and plug server text spams


%c = Player::getClient(%target);
//echo(%target);
%item = Player::getMountedItem(%c,$WeaponSlot);
if(%item.className == Weapon) {

		//%name = GameBase::getDataName(%Item); 
		%state = Player::getItemState(%c,$WeaponSlot);
		%it = Player::getMountedItem(%c,$WeaponSlot);
	//echo(%state);
	
	if(%state == "Fire" && (%it != "Blaster" && %it != "EnergyRifle")) {
		%item = %item.imageType.ammoType;
		Player::dropItem(%c,%item);
		}
//Player::trigger(%player, $WeaponSlot, false);
	//echo(%it);
	if(%it != "Blaster" && %it != "EnergyRifle" && %it != "") {
		Player::dropItem(%c,%item);
		}

		//Hackety hack attack
	if(%it == "Blaster" || %it == "EnergyRifle") {

		//
		//so they wanna keep the flag and fire their energy weapon too, huh?
		%fl = Player::getMountedItem(%c,$FlagSlot);
	if(%fl != -1 && %state != "Fire")
      	Player::dropItem(%c, %item);
	if(%fl == -1 && %state != "Fire")
      	Player::dropItem(%c, %item);
		//drop flag below
	if(%fl != -1 && %state == "Fire")
      	Player::dropItem(%c, %fl);
		}


		%it = Player::getMountedItem(%c,$WeaponSlot);// spam check
		
	if(%it == -1){

	 	%mf =Client::getGender(%c);
		if (%mf == "Female") {
		 messageAll(0,Client::getName(%c)@" fumbles her " @ %item.description);}
		else 
			messageAll(0,Client::getName(%c)@" fumbles his " @ %item.description);
		}			
}
if(%item == "RepairGun")
		{
	 	Player::dropItem(%c,Player::getMountedItem(%c,$BackpackSlot));
		messageAll(0,Client::getName(%c)@" droped a Fixit Gun");
		}
%energy = 0;
   }
   GameBase::setEnergy(%target, %energy);	
}

RocketData ShotDiscShell 
{
   //bulletShapeName = "discb.dts";//deleted for sex0r
   //explosionTag    = rocketExp;//deleted for sex0r

   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 0;       // 0 impact, 1, radius
   damageValue      = 0;
   damageType       = $ExplosionDamageType;

   explosionRadius  = 0;//0.1
   kickBackStrength = 0;

   muzzleVelocity   = 200.0;
   terminalVelocity = 200.0;
   acceleration     = 5.0;

   totalTime        = 4.0;
   liveTime         = 4.0;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

};



GrenadeData VolcanoBolt
{
   bulletShapeName    = "dirarrows.dts";//grenade.dts
   explosionTag       = VolcanoExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.2;
   mass               = 1.0;
   elasticity         = 0.45;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.3;
   damageType         = $PlasmaDamageType;//$ShrapnelDamageType

   explosionRadius    = 15;
   kickBackStrength   = 150.0;
   maxLevelFlightDist = 150;
   totalTime          = 30.0;    // special meaning for grenades...
   liveTime           = 1.0; // grenade time live after contact
   projSpecialTime    = 0.05;
soundId = SoundJetLight;
   inheritedVelocityScale = 0.5;

   smokeName              = "plasmabolt.dts";//smoke.dts
};

GrenadeData BushSmoke 
{ 
	bulletShapeName = "smoke.dts"; 
	explosionTag = KickbackExp; 
	collideWithOwner = false; 
	ownerGraceMS = 500; 
	collisionRadius = 0.1; 
	mass = 0.1; 
	elasticity = 0.1; 
	damageClass = 0; 
	damageValue = 0.0; 
	damageType = $noDamageType; 
	explosionRadius = 1; 
	kickBackStrength = 0.0; 
	maxLevelFlightDist = 2; 
	totalTime = 0.2; 
	liveTime = 0.2; // grenade time live after contact
	projSpecialTime = 0.05; 
	inheritedVelocityScale = 1.0; 
	smokeName = "smoke.dts"; //rsmoke
}; 



GrenadeData ArmChan 
{ 
	bulletShapeName = "rsmoke.dts"; 
	explosionTag = armChanExp; 
	collideWithOwner = false; 
	ownerGraceMS = 500; 
	collisionRadius = 0.0; 
	mass = 0.0; 
	elasticity = 0.1; 
	damageClass = 0; 
	damageValue = 0.01; 
	damageType = $ImpactDamageType; 
	explosionRadius = 1; 
	kickBackStrength = 0.0; 
	maxLevelFlightDist = 4; 
	totalTime = 0.55; 
	liveTime = 0.55; // grenade time live after contact
	projSpecialTime = 0.05; 
	inheritedVelocityScale = 1.0; 
	smokeName = "rsmoke.dts"; 
}; 


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
   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "rsmoke.dts";
   smokeDist   = 1.8;

   soundId = SoundJetHeavy;
};

RepairEffectData RepairBolt
{
   bitmapName       = "lightningNew.bmp";//repairadd.bmp
   boltLength       = 35.0;
   segmentDivisions = 9;
   beamWidth        = 0.25;

   updateTime   = 450;
   skipPercent  = 0.6;
   displaceBias = 0.15;

   lightRange = 3.0;
   lightColor = { 1.25, 1.25, 0.85 };//lightColor = { 0.85, 0.25, 0.25 };
};

//modified to fix objects faster then players, and to work with spiffy admin options.. -Plasmatic
function RepairBolt::onAcquire(%this, %player, %target)
{
	%client = Player::getClient(%player);
	%player.fixingDisabled = false;
	if (%target == %player) {
	   %player.repairTarget = -1;
		if (GameBase::getDamageLevel(%player) != 0) {
			%player.repairRate = 0.2;
			%player.repairTarget = %player;
			Client::sendMessage(%client, 0, "Repairing Yourself");
		}
		else {
			Client::sendMessage(%client,0,"Nothing to Fix");
			Player::trigger(%player, $WeaponSlot, false);
			return;
		}
	}
	else {
      %player.repairTarget = %target;
		%player.repairRate   = 7.5;
		if (getObjectType(%player.repairTarget) == "Player") {
			%rclient = Player::getClient(%player.repairTarget);
			%name = Client::getName(%rclient);
		}
		else if(fixable(%player,%target)){ 
			%name = GameBase::getMapName(%target);
			if(%name == "") {
				%name = (GameBase::getDataName(%player.repairTarget)).description;
			}
		}
		else {%player.fixingDisabled = true;Player::trigger(%player,$WeaponSlot,false);return;}
		if (GameBase::getDamageLevel(%player.repairTarget) == 0) {
			Client::sendMessage(%client,0,%name @ " is not broken");
			Player::trigger(%player,$WeaponSlot,false);
			%player.repairTarget = -1;
			return;
		}
		if (getObjectType(%player.repairTarget) == "Player") {
			Client::sendMessage(%rclient,0,"Being Fixed by " @ Client::getName(%client));
		}
		Client::sendMessage(%client,0,"Fixing " @ %name);
	}
	%rate = GameBase::getAutoRepairRate(%player.repairTarget) + %player.repairRate;
	GameBase::setAutoRepairRate(%player.repairTarget,%rate);
}

function RepairBolt::onRelease(%this, %player)
{
	%object = %player.repairTarget;
	if(%player.fixingDisabled)return;
	if (%object != -1) {
		%client = Player::getClient(%player);
		if (%object == %player) {
			Client::sendMessage(%client,0,"Fixit Pack Off");
		}
		else {
			if (GameBase::getDamageLevel(%object) == 0) {
				Client::sendMessage(%client,0,"Repair Done");
			}
			else {
				Client::sendMessage(%client,0,"Repair Interupted");
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

SeekingMissileData TurretMissile1
{
   bulletShapeName = "mortar.dts";//rocket.dts
   explosionTag    = rocketExp;
   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.5;
   damageType       = $MissileDamageType;
   explosionRadius  = 9.5;
   kickBackStrength = 50.0;

   muzzleVelocity    = 150.0;
   totalTime         = 6.0;
   liveTime          = 4.0;
   seekingTurningRadius    = 20;
   nonSeekingTurningRadius = 75.0;
   proximityDist     = 1.5;
   smokeDist         = 1.75;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

   soundId = SoundJetHeavy;
};

function SeekingMissile::updateTargetPercentage(%target)
{
   return GameBase::virtual(%target, "getHeatFactor");
}



SeekingMissileData TurretMissile
{
   bulletShapeName = "rocket.dts";//
   explosionTag    = rocketExp;
   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.5;
   damageType       = $MissileDamageType;
   explosionRadius  = 9.5;
   kickBackStrength = 175.0;//375

   muzzleVelocity    = 52.0;//22
   totalTime         = 15;//50
   liveTime          = 15;//50
   seekingTurningRadius    = 0.1;//2
   nonSeekingTurningRadius = 5.0;//15
   proximityDist     = 1.5;
   smokeDist         = 0.75;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0;//0.5

   soundId = SoundJetHeavy;
};

function SeekingMissile::updateTargetPercentage(%target)
{
   return GameBase::virtual(%target, "getHeatFactor");
}

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



LightningData turretCharge
{
   bitmapName       = "repairadd.bmp"; //lightningNew.bmp
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
GrenadeData MortarTurretShell
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = mortarExp;
   collideWithOwner   = True;
   ownerGraceMS       = 400;
   collisionRadius    = 1.0;
   mass               = 5.0;
   elasticity         = 0.2;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 1.32;
   damageType         = $MortarDamageType;

   explosionRadius    = 30.0;
   kickBackStrength   = 250.0;
   maxLevelFlightDist = 1800;
   totalTime          = 1000.0;
   liveTime           = 0.1;   // grenade time live after contact
   projSpecialTime    = 0.005;

   inheritedVelocityScale = 0.5;
   smokeName              = "plastrail.dts";//mortartrail
};

BulletData MiniFusionBolt
{
   bulletShapeName    = "shotgunbolt.dts";
   explosionTag       = energyExp;

   damageClass        = 0;
   damageValue        = 0.1;
   damageType         = $EnergyDamageType;

   muzzleVelocity     = 80.0;
   totalTime          = 4.0;
   liveTime           = 2.0;

   lightRange         = 3.0;
   lightColor         = { 0.25, 0.25, 1.0 };
   inheritedVelocityScale = 0.5;
   isVisible          = True;

   rotationPeriod = 1;
};

BulletData oldChaingunBullet
{
   bulletShapeName    = "bullet.dts";//fusionex.dts,fusionbolt.dts//shotgunbolt.dts
   validateShape      = true;
   explosionTag       = turretExp;
   expRandCycle       = 3;
   mass               = 0.05;
//  bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.15;
   damageType         = $BulletDamageType;

   aimDeflection      = 0.005;
   muzzleVelocity     = 250.0;
   totalTime          = 0.6;
   inheritedVelocityScale = 1.0;
   isVisible          = True;

   tracerPercentage   = 80.0;
   tracerLength       = 200;
};

RocketData ChaingunBullet
{
   bulletShapeName = "bullet.dts";
   explosionTag    = ChainExp;
   collisionRadius = 0.0;
   mass            = 0.05;
bulletHoleIndex    = 0;
   damageClass      = 0;       // 0 impact, 1, radius
   damageValue      = 0.25;	//0.5;
   damageType       = $BulletDamageType;
//   explosionRadius  = 7.5;
   kickBackStrength = 10.0;
   muzzleVelocity   = 1000.0;
   terminalVelocity = 1000.0;
   acceleration     = 1.0;
   totalTime        = 0.9;
   liveTime         = 0.9;
//   lightRange       = 5.0;
//   lightColor       = { 0.4, 0.4, 1.0 };
   inheritedVelocityScale = 0.5;
   // rocket specific
   trailType   = 1;
   trailLength = 2000;
   trailWidth  = 0.1;

//   trailType   = 2;                // smoke trail
//   trailString = "breath.dts";
//   smokeDist   = 10;


};


RocketData DiscShell
{
   bulletShapeName = "discb.dts";
   explosionTag    = rocketExp;

   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.5;	//0.5;
   damageType       = $ExplosionDamageType;

   explosionRadius  = 7.5;
   kickBackStrength = 20.0;

   muzzleVelocity   = 65.0;
   terminalVelocity = 80.0;
   acceleration     = 5.0;

   totalTime        = 6.5;
   liveTime         = 8.0;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 1;
   trailLength = 35;
   trailWidth  = 0.3;

   soundId = SoundDiscSpin;
};



//--------------------------------------

rocketData ShotDiscBullet
{
   bulletShapeName    = "discb.dts";//discb.dts";
   explosionTag       = rocketExp;
   expRandCycle       = 3;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.075;
   damageType         = $ShotgunDamageType;

   aimDeflection      = 0.0200;
   muzzleVelocity     = 100.0;
   totalTime          = 3;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 1.0;
   tracerLength       = 30;
};
$Kickback = 20;


GrenadeData KickBackShell
{
   explosionTag    = KickbackExp;
   collideWithOwner   = True;
   ownerGraceMS       = 0.0;
   collisionRadius    = 0.3;
   mass               = 5.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.0;
   damageType         = $Kickback;

   explosionRadius    = 20.0;
   kickBackStrength   = 20.0;
   maxLevelFlightDist = 275;
   totalTime          = 0.01;
   liveTime           = 0.01; // grenade time live after contact
   projSpecialTime    = 1.0;	// smoke interval

   inheritedVelocityScale = 1.0;
   smokeName          = "breath.dts";
};

GrenadeData KickforwardShell
{
explosionTag = KickbackExp;
collideWithOwner = True;
ownerGraceMS = 0.0;
collisionRadius = 0.3;
mass = 5.0;
elasticity = 0.1;

damageClass = 1; // 0 impact, 1, radius
damageValue = 0.0;
damageType = $Kickback;

explosionRadius = 20.0;
kickBackStrength = -20.0;
maxLevelFlightDist = 275;
totalTime = 0.01;
liveTime = 0.01; // grenade time live after contact
projSpecialTime = 1.0; // smoke interval

inheritedVelocityScale = 1.0;
smokeName = "breath.dts";

};



RocketData RockShell
{
   bulletShapeName = "mortar.dts";//mortar
   explosionTag    = LargeShockwave;

   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.1;
   damageType       = $NoPlayerDamagetype;//$ImpactDamageType;//$ExplosionDamageType

   explosionRadius  = 15;
   kickBackStrength = 220.0;

   muzzleVelocity   = 50.0;
   terminalVelocity = 180.0;
   acceleration     = 15.0;

   totalTime        = 6.5;	//6.5
   liveTime         = 8.0;	//8.0

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

   // rocket specific
  // trailType   = 1;
  // trailLength = 15;
  // trailWidth  = 0.3;
	trailType = 2;
	trailString = "plasmatrail.dts";	//plasmatrail.dts
	smokeDist = 4.5;
   soundId = SoundDiscSpin;
};



RocketData AgedonShell
{
   bulletShapeName = "mortar.dts";//armorkit.dts
   explosionTag    = FlakExp;//mineExp;
 //	explosionTag = BuildExp;
   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 0;       // 0 impact, 1, radius
   damageValue      = 0;
   damageType       = $ShrapnelDamageType;//$ExplosionDamageType

   explosionRadius  = 15;
   kickBackStrength = 80.0;

   muzzleVelocity   = 180.0;
   terminalVelocity = 180.0;
   acceleration     = 15.0;

   totalTime        = 6.5;
   liveTime         = 5.0;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

   // rocket specific
//	trailType = 2;
//	trailString = "plasmatrail.dts";
//	smokeDist = 4.5;
   trailType   = 1;
   trailLength = 50;
   trailWidth  = 0.3;
  // soundId = SoundDiscSpin;
};



RocketData IonEEBolt 
{	bulletShapeName = "mortartrail.dts";
	explosionTag = GblasterExp;
	collisionRadius = 0.0;
	mass = 2.0;
	damageClass = 1;
	damageValue = 0.00;
	damageType = $TracDamageType;
	explosionRadius = 5;
	kickBackStrength = -300.0;
	muzzleVelocity = 200.0;
	terminalVelocity = 200.0;
	acceleration = 5.0;
	totalTime = 10.0;
	liveTime = 11.0;
	lightRange = 5.0;
	lightColor = { 0.0, 1.0, 0.0 };
	inheritedVelocityScale = 0.5;
	trailType = 2;
	trailString = "mortartrail.dts";
	smokeDist = 4.5;
	soundId = SoundJetHeavy;
};
RocketData PencilTurretBullet
{
   bulletShapeName = "bullet.dts";//
   explosionTag    = bulletExp0;
   collisionRadius = 0.0;
   mass            = 2.0;
   //ownerGraceMS       = 250;

   damageClass      = 0;       // 0 impact, 1, radius
   damageValue      = 0.05;
   damageType       = $BulletDamageType;

   explosionRadius  = 2.5;
   kickBackStrength = 80.0;

   muzzleVelocity   = 300.0;
   terminalVelocity = 700.0;
   acceleration     = 2000.0;

   totalTime        = 1.5;
   liveTime         = 1.7;

   lightRange       = 10.0;
   lightColor       = { 0.1, 1.0, 1.0 };

   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 1;
   trailLength = 15;
   trailWidth  = 0.1;

   soundId = SoundDiscSpin;
};

GrenadeData ShotDicer
{
   bulletShapeName    = "plasmabolt.dts";	//discb.dts
   explosionTag       = FlakExp;
   collideWithOwner   = true;
   ownerGraceMS       = 150;
   collisionRadius    = 1.0;
   mass               = 5.0;
   elasticity         = 0.01;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.3;
   damageType         = $PlasmaDamageType;

   explosionRadius    = 15;
   kickBackStrength   = 10.0;
   maxLevelFlightDist = 900;
   totalTime          = 3.0;    // special meaning for grenades...
   liveTime           = 0.1;    // grenade time live after contact
   projSpecialTime    = 1.5;	//smoke interval

   inheritedVelocityScale = 0.75;
   soundId = SoundJetLight;
   smokeName              = "breath.dts";//smoke.dts,lfemale.dts
};

GrenadeData HoShell
{
   bulletShapeName    = "lfemale.dts";
   explosionTag       = BlownUpHo;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 10.2;
   mass               = 1.0;
   elasticity         = 0.45;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 1.5;
   damageType         = $ShrapnelDamageType;

   explosionRadius    = 15;
   kickBackStrength   = 150.0;
   maxLevelFlightDist = 150;
   totalTime          = 30.0;    // special meaning for grenades...
   liveTime           = 1.0;    // grenade time live after contact
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 0.5;

   smokeName              = "breath.dts";//smoke.dts,lfemale.dts
};
GrenadeData MortarShell
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = mortarExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.3;
   mass               = 5.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.2;
   damageType         = $MortarDamageType;

   explosionRadius    = 20.0;
   kickBackStrength   = 250.0;
   maxLevelFlightDist = 275;
   totalTime          = 30.0;
   liveTime           = 2.0; // grenade time live after contact
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 0.5;
   smokeName              = "plastrail.dts";//mortartrail
};

GrenadeData MinerShell
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = mortarExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.3;
//   mass               = 5.0;
//   elasticity         = 0.1;
   mass               = 1.0;
   elasticity         = 0.2;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.2;
   damageType         = $ShrapnelDamageType; //$MortarDamageType;

   explosionRadius    = 20.0;
   kickBackStrength   = 250.0;
   maxLevelFlightDist = 275;
   totalTime          = 20.0;
   liveTime           = 20.0; // grenade time live after contact
   projSpecialTime    = 0.01; // smoke interval

   inheritedVelocityScale = 0.5;
   smokeName     = "enex.dts";
//plasmaex.dts,fusionex.dts
//enbolt.dts,mortartrail,plastrail,breath
//fusionbolt.dts
//enex.dts
};

GrenadeData MineFloaters
{
   bulletShapeName    = "mine.dts";//mortar
   explosionTag       = mortarExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.3;
   mass               = 1.0;	//5.0
   elasticity         = 0.45;	//0.1

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.2;
   damageType         = $ShrapnelDamageType; //$MortarDamageType;

   explosionRadius    = 20.0;
   kickBackStrength   = 250.0;
   maxLevelFlightDist = 275;
   totalTime          = 2.1;
   liveTime           = 2.1; // grenade time live after contact
   projSpecialTime    = 0.1;

   inheritedVelocityScale = 0.5;
   smokeName              = "smoke.dts";//breath.dts";//mortartrail,plastrail
};
GrenadeData RocketKilla
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = WickedBadExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.3;
   mass               = 5.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 5.0;//10.2
   damageType         = $MissileDamageType;//$ImpactDamageType,$PlasmaDamageType

   explosionRadius    = 25.0;//25
   kickBackStrength   = 200.0;
   maxLevelFlightDist = 275;
   totalTime          = 0.01;
   liveTime           = 0.01; // grenade time live after contact
   projSpecialTime    = 0.01;  //smoke interval

   inheritedVelocityScale = 0.5;
   smokeName              = "breath.dts";//mortartrail
};

GrenadeData suicideShell
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = mortarExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.3;
   mass               = 5.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 1.0;
   damageType         = $PlasmaDamageType;

   explosionRadius    = 20.0;
   kickBackStrength   = 250.0;
   maxLevelFlightDist = 275;
   totalTime          = 0.01;
   liveTime           = 0.01; // grenade time live after contact
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 1.0;
   smokeName              = "plastrail.dts";//mortartrail
};

GrenadeData JumpGunBoltwang
{
   bulletShapeName    = "dirarrows.dts";//grenade.dts
   explosionTag       = JumpGunExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.2;
   mass               = 1.0;
   elasticity         = 0.45;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.3;
   damageType         = $PlasmaDamageType;//$ShrapnelDamageType

   explosionRadius    = 15;
   kickBackStrength   = 150.0;
   maxLevelFlightDist = 150;
   totalTime          = 30.0;    // special meaning for grenades...
   liveTime           = 1.0;     // grenade time live after contact
   projSpecialTime    = 0.05;
soundId = SoundJetLight;
   inheritedVelocityScale = 0.5;

   smokeName              = "plasmabolt.dts";//smoke.dts
};


GrenadeData BuildShock 
{ 
	bulletShapeName = "breath.dts";
 	explosionTag = BuildExp;
 	collideWithOwner = false;
 	ownerGraceMS = 500;
 	collisionRadius = 0.0;
 	mass = 0.0;
 	elasticity = 0.1;
 	damageClass = 0;
 	damageValue = 0.01;
 	damageType = $NullDamageType;
 	explosionRadius = 10;
 	kickBackStrength = 0.0;
 	maxLevelFlightDist = 1;
 	totalTime = 0.05;
 	liveTime = 0.05;     // grenade time live after contact
 	projSpecialTime = 0.05;
 	inheritedVelocityScale = 1.0;
 	smokeName = "breath.dts";
 };



GrenadeData TeleportShock 
{ 
	bulletShapeName = "breath.dts";
 	explosionTag = TeleportExp;
 	collideWithOwner = false;
 	ownerGraceMS = 500;
 	collisionRadius = 0.0;
 	mass = 0.0;
 	elasticity = 0.1;
 	damageClass = 0;
 	damageValue = 0.01;
 	damageType = $NullDamageType;
 	explosionRadius = 10;
 	kickBackStrength = 0.0;
 	maxLevelFlightDist = 1;
 	totalTime = 0.05;
 	liveTime = 0.05;    // grenade time live after contact
 	projSpecialTime = 0.05;
 	inheritedVelocityScale = 1.0;
 	smokeName = "breath.dts";
 };

MineData ShotDiscFlak
{
   mass = 1.0;
   drag = 1.0;
	elasticity = 0.1;
	friction = 1.0;

	className = "Mine";
   description = "ShotDiscFlak";
   shapeFile = "discb";
   shadowDetailMask = 4;
   explosionId = FlakExp;
	explosionRadius = 10.0;
	damageValue = 0;
	damageType = $PlasmaDamageType;
	kickBackStrength = 0;
	triggerRadius = 2.5;
	maxDamage = 0.5;
	shadowDetailMask = 0;
	destroyDamage = 0.5;
	damageLevel = {1.0, 1.0};
};
GrenadeData PrettySplit
{
   bulletShapeName    = "Mortar.dts";//grenade.dts
   explosionTag       = AgedonSplitExp;//FlakExp;//VolcanoExp;//plasmaExp
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.2;
   mass               = 1.0;
   elasticity         = 0.45;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.1;
   damageType         = $ShrapnelDamageType;//$PlasmaDamageType;//

   explosionRadius    = 15;
   kickBackStrength   = 10.0;
   maxLevelFlightDist = 150;
   totalTime          = 0.01;    // special meaning for grenades...
   liveTime           = 0.01; // grenade time live after contact
   projSpecialTime    = 0.05;
soundId = SoundJetLight;
   inheritedVelocityScale = 0.5;

   smokeName              = "plasmabolt.dts";//smoke.dts
};




BulletData PlasmaticShell
{
   bulletShapeName    = "shotgunbolt.dts";//fusionbolt.dts//shotgunbolt.dts//discb.dts

   explosionTag       = blasterExp;//rocketExp;
   expRandCycle       = 0.02;
   aimDeflection      = 0.02;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.07;
   damageType         = $BlasterDamageType;//$ExplosionDamageType;//$PlasmaDamageType;
   muzzleVelocity     = 300.0;

   totalTime          = 6.5;
	liveTime = 8.0;
   lightRange         = 3.0;
   lightColor         = { 1.0, 0.25, 0.25 };
   inheritedVelocityScale = 0.5;
   isVisible          = True;

   rotationPeriod = 1;

   tracerPercentage   = 10.0;
   tracerLength       = 30;

};

BulletData PlasmaticBuildShell
{
   bulletShapeName    = "shotgunbolt.dts";//fusionbolt.dts//shotgunbolt.dts//discb.dts

   explosionTag       = blasterExp;//rocketExp;
   expRandCycle       = 0.01;
   aimDeflection      = 0.01;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 15.0;
   damageType         = $NoPlayerDamageTYPE;//$ExplosionDamageType;//$PlasmaDamageType;
   muzzleVelocity     = 300.0;

   totalTime          = 2.5;
	liveTime = 8.0;
   lightRange         = 3.0;
   lightColor         = { 1.0, 0.25, 0.25 };
   inheritedVelocityScale = 0.5;
   isVisible          = True;

   rotationPeriod = 1;
};


BulletData Trail
{
   bulletShapeName    = "shotgunbolt.dts";
   explosionTag       = blasterExp;

   damageClass        = 0;
   damageValue        = 0.0;
   damageType         = $BlasterDamageType;

   muzzleVelocity     = 200.0;
   totalTime          = 1.0;
   liveTime           = 1.125;

   lightRange         = 3.0;
   lightColor         = { 1.0, 0.25, 0.25 };
   inheritedVelocityScale = 0.5;
   isVisible          = True;

   rotationPeriod = 1;
};


BulletData BlasterBolt
{
   bulletShapeName    = "shotgunbolt.dts";
   explosionTag       = blasterExp;

   damageClass        = 0;
   damageValue        = 0.25;
   damageType         = $BlasterDamageType;

   muzzleVelocity     = 300.0;
   totalTime          = 0.15;
   liveTime           = 0.15;
   lightRange         = 3.0;
   lightColor         = { 1.0, 0.25, 0.25 };
   inheritedVelocityScale = 0.5;
   isVisible          = True;

   rotationPeriod = 1;
};

MineData Firegrenade
{
   mass = 0.3;
   drag = 1.0;
   density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
   description = "Handgrenade";
   shapeFile = "discb";
   shadowDetailMask = 4;
   explosionId = FlakExp;//FlakExp
	explosionRadius = 10.0;
	damageValue = 0.5;
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 2;
};


BulletData PlasmaBolt
{
   bulletShapeName    = "plasmabolt.dts";
   explosionTag       = plasmaExp;

   damageClass        = 1;
   damageValue        = 0.45;
   damageType         = $PlasmaDamageType;
   explosionRadius    = 4.0;

   muzzleVelocity     = 100.0;	//55
   totalTime          = 2.0;
   liveTime           = 2.0;
   lightRange         = 3.0;
   lightColor         = { 1, 1, 0 };
   inheritedVelocityScale = 0.3;
   isVisible          = True;

   soundId = SoundJetLight;
};

BulletData HuckerBullet
{
   bulletShapeName    = "grenade.dts";
   explosionTag       = HuckExp;

   damageClass        = 0;
   damageValue        = 0.2;	//0.1
   damageType         = $EnergyDamageType;

   muzzleVelocity     = 80.0;
   totalTime          = 4.0;
   liveTime           = 2.0;

   lightRange         = 3.0;
   lightColor         = { 0.25, 0.25, 1.0 };
   inheritedVelocityScale = 0.5;
   isVisible          = True;

   rotationPeriod = 0.25;
};

RocketData kicker  //MiniFusionBolt //BulletData
{
   bulletShapeName = "mortar.dts";//
   explosionTag    = LargeShockwave;

   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.1;
   damageType       = $ExplosionDamageType;

   explosionRadius  = 15;
   kickBackStrength = 220.0;

   muzzleVelocity   = 50.0;
   terminalVelocity = 180.0;
   acceleration     = 15.0;

   totalTime        = 6.5;
   liveTime         = 8.0;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 1;
   trailLength = 15;
   trailWidth  = 0.3;

   soundId = SoundDiscSpin;
};
TargetLaserData targetLaser
{
   laserBitmapName   = "paintglow.bmp";	//paintpulse

   damageConversion  = 0.0;
   baseDamageType    = 0;

   lightRange        = 20.0;
   lightColor        = { 1,1,1};//{ 0.25, 1.0, 0.25 }

   detachFromShooter = false;
};

TargetLaserData ScopeLaser
{
   laserBitmapName   = "blue.flag.bmp";	//paintpulse

   damageConversion  = 0.0;
   baseDamageType    = 0;

   lightRange        = 3.0;
   lightColor        = { 1,1,1};//{ 0.25, 1.0, 0.25 }

   detachFromShooter = true;
};

LaserData sniperLaser
{
   laserBitmapName   = "laserPulse.bmp";
   hitName           = "laserhit.dts";

   damageConversion  = 0.007;
   baseDamageType    = $LaserDamageType;

   beamTime          = 0.5;

   lightRange        = 2.0;
   lightColor        = { 1.0, 0.25, 0.25 };

   detachFromShooter = false;
   hitSoundId        = SoundLaserHit;
};

LaserData PhrailterLaser
{
   laserBitmapName   = "pulse.bmp";
   hitName           = "laserhit.dts";

   damageConversion  = 0.0;		//0.007
   baseDamageType    = $LaserDamageType;

   beamTime          = 0.7;

   lightRange        = 2.0;
   lightColor        = { 1.0, 0.25, 0.25 };

   detachFromShooter = true;
   hitSoundId        = SoundLaserHit;
};

TargetLaserData oldPhrailterLaser
{
   laserBitmapName   = "pulse.bmp";	
//paintpulse, paintglow, plasma

   damageConversion  = 0.0;
   baseDamageType    = 0;

   lightRange        = 2.0;
   lightColor        = { 1,1,1};//{ 0.25, 1.0, 0.25 }

   detachFromShooter = true;	//false
};


LightningData lightningCharge
{
   bitmapName       = "paintPulse.bmp";// lightningNew.bmp //repairadd.bmp

   damageType       = $ElectricityDamageType;
   boltLength       = 40.0;
   coneAngle        = 5.0;//35
   damagePerSec      = 0.06;
   energyDrainPerSec = 60.0;
   segmentDivisions = 4;
   numSegments      = 5;
   beamWidth        = 0.125;//075//
   updateTime   = 120;
   skipPercent  = 0.5;
   displaceBias = 0.15;

   lightRange = 3.0;
   lightColor = { 0.25, 0.25, 0.85 };

   soundId = SoundELFFire;
};
GrenadeData AgedonGrenadeShell
{
   bulletShapeName    = "grenade.dts";
   explosionTag       = grenadeExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.2;
   mass               = 0.3;
   elasticity         = 0.2;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.5;
   damageType         = $ShrapnelDamageType;

   explosionRadius    = 15;
   kickBackStrength   = 100.0;
   maxLevelFlightDist = 150;
   totalTime          = 2.0;    // special meaning for grenades...
   liveTime           = 2.0;     // grenade time live after contact
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 0.5;

   smokeName              = "smoke.dts";//plasmaex.dts";//

};


//-----------------------
// here to make base happy -plasmatic

GrenadeData GrenadeShell
{
   bulletShapeName    = "grenade.dts";
   explosionTag       = grenadeExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.2;
   mass               = 1.0;
   elasticity         = 0.45;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.4;
   damageType         = $ShrapnelDamageType;

   explosionRadius    = 15;
   kickBackStrength   = 150.0;
   maxLevelFlightDist = 150;
   totalTime          = 30.0;    // special meaning for grenades...
   liveTime           = 1.0;     // grenade time live after contact
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 0.5;

   smokeName              = "smoke.dts";
};


BulletData oldDiscShell
{
   bulletShapeName    = "discb.dts";//fusionbolt.dts//shotgunbolt.dts

   explosionTag       = rocketExp;
   expRandCycle       = 0.02;
   aimDeflection      = 0.02;
 //  mass               = 0.5;
 //  bulletHoleIndex    = 0;
	collisionRadius = 0.0;
	mass = 2.0;
   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.5;  //1.0
   damageType         = $ExplosionDamageType;//$PlasmaDamageType;
	explosionRadius = 7.5;
	kickBackStrength = 150.0;

   muzzleVelocity     = 65.0;
	terminalVelocity = 80.0;
	acceleration = 5.0;
   totalTime          = 6.5;
	liveTime = 8.0;
	lightRange = 5.0;
	lightColor = { 0.4, 0.4, 1.0 };
   inheritedVelocityScale = 0.5;
   isVisible          = True;
	soundId = SoundDiscSpin;
   tracerPercentage   = 10.0;
   tracerLength       = 30;
};


 RocketData SniperShell
{
   bulletShapeName = "tracer.dts";
  explosionTag    = flashExpMedium;//rocketExp;bulletExp1
   collisionRadius = 0.0;
   mass            = 2.0;
   damageClass      = 0;       // 0 impact, 1, radius
   damageValue      = 1.1;
   damageType       = $LaserDamageType;//$ExplosionDamageType;
   explosionRadius  = 0.5;
   kickBackStrength = 0.0;
   muzzleVelocity   = 2000.0;
   terminalVelocity = 2000.0;
   acceleration     = 0.0;
   totalTime        = 0.5;
   liveTime         = 0.5;
   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };
   inheritedVelocityScale = 0.5;
   // rocket specific
   trailType   = 1;
   trailLength = 1000;
   trailWidth  = 0.2;
//   soundId = SoundDiscSpin;
};



RocketData PencilShell
{
   bulletShapeName = "bullet.dts";//
   explosionTag    = bulletExp0;
   
collideWithOwner   = false;
   collisionRadius = 0.0;
   mass            = 2.0;
   //ownerGraceMS       = 250;

   damageClass      = 0;       // 0 impact, 1, radius
   damageValue      = 0.3;
   damageType       = $BulletDamageType;

   explosionRadius  = 2.5;
   kickBackStrength = 80.0;

   muzzleVelocity   = 300.0;
   terminalVelocity = 700.0;
   acceleration     = 2000.0;

   totalTime        = 3.5;
   liveTime         = 3.7;

   lightRange       = 10.0;
   lightColor       = { 0.1, 1.0, 1.0 };

   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 1;
   trailLength = 20;
   trailWidth  = 0.1;

   soundId = SoundDiscSpin;
};


GrenadeData PlasmaBurn
{
   bulletShapeName = "plasmabolt.dts";
   explosionTag       = BurnExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.3;
   mass               = 5.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.15;
   damageType         = $PlasmaDamageType;

   explosionRadius    = 20.0;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 275;
   totalTime          = 0.01;
   liveTime           = 0.01; // grenade time live after contact
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 1.0;
   smokeName              = "plastrail.dts";//mortartrail
};

GrenadeData RatPoison
{
   bulletShapeName = "plasmabolt.dts";
   explosionTag       = RatPoisonExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.3;
   mass               = 5.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.15;
   damageType         = $PlasmaDamageType;

   explosionRadius    = 20.0;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 275;
   totalTime          = 0.01;
   liveTime           = 0.01; // grenade time live after contact
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 1.0;
   smokeName              = "plastrail.dts";//mortartrail
};

 











