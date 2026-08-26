//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Assault Cannon
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$AutoUse[Autogun] = False;
$InvList[Autogun] = 1;
$WeaponAmmo[Autogun] = AutoAmmo;
$RemoteInvList[Autogun] = 1;

addWeapon(Autogun);

BulletData AutogunBullet
{
  bulletShapeName = "bullet.dts";
  explosionTag = bulletExp0;
  expRandCycle = 3;
  mass = 0.05;
  bulletHoleIndex = 0;
  damageClass = 0;
  damageValue = 0.25;
  damageType = $BulletDamageType;
  aimDeflection = 0.003;
  muzzleVelocity = 425.0;
  totalTime = 1.5;
  liveTime = 1.5;
  inheritedVelocityScale = 1.0;
  isVisible = False;
  tracerPercentage = 1.0;
  tracerLength = 30;
};

function AutoGunBullet::onAdd(%this)
{
	ShellEject(%this);
}

function ShellEject(%this)
{
	%normalrot = "0 0 -0.785";
	%rand = getRandom();
	if(%rand <= 0.5)
	{
		%rot = GameBase::getRotation(%this);
		%startpos = Vector::getFromRot(%rot, -0.35);
		%rot = Vector::add(%rot, %normalrot);
		%casingpos = Vector::getFromRot(%rot, 1.0);
		%casingpos = Vector::add(%casingpos, %startpos);
	
		%rotx = getWord(%rot, 0) - %rand;
		%roty = getWord(%rot, 1);
		%rotz = getWord(%rot, 2) - 0.785;
		%newrot = %rotx@" "@%roty@" "@%rotz;

		%pos = GameBase::getPosition(%this);
		%pos = Vector::add(%pos, %casingpos);
	
		%this = newObject("","Mine","shell");
		addToSet("MissionCleanup", %this);
		GameBase::setPosition(%this, %pos);
		GameBase::setRotation(%this, %newrot);
	
		%strength = 0.2;
		%obj = newObject("","Mine","shell");
 		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj,%this,%strength,false);
	
		deleteObject(%this);
	}
}
ItemImageData AutogunImage
{
  shapeFile = "assault";
  mountPoint = 0;
  weaponType = 1;
  reloadTime = 0;
  spinUpTime = 0;
  spinDownTime = 3;
  fireTime = 0.1;
  ammoType = AutoAmmo;
  projectileType = AutogunBullet;
  accuFire = false;
  lightType = 3;
  lightRadius = 3;
  lightTime = 1;
  lightColor = { 0.6, 1, 1 };
  sfxFire = SoundAssaultFire;
  sfxActivate = SoundPickUpWeapon;
  sfxSpinUp = SoundAssaultSpinUp;
  sfxSpinDown = SoundAssaultSpinDown;
};

ItemData Autogun
{
  description = "Assault Cannon";
  className = "Weapon";
  shapeFile = "assault";
  hudIcon = "chain";
  heading = $InvHead[ihWma];
  shadowDetailMask = 4;
  imageType = AutogunImage;
  price = 15;
  showWeaponBar = true;
};


function Autogun::onMount(%player,%item,$WeaponSlot)
{
        %client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Assault Cannon<f1>\nA brutal chaingun, firing more than 1000 rounds per minute.");
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Needler: Modded from Dart Rifle
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[TranqGun] = 1;
$RemoteInvList[TranqGun] = 1;
$AutoUse[TranqGun] = False;
$WeaponAmmo[TranqGun] = TranqAmmo;

addWeapon(TranqGun);

BulletData TranqDart 
{
  bulletShapeName = "bullet.dts";
  explosionTag = bulletExp0;
  expRandCycle = 3;
  mass = 0.05;
  bulletHoleIndex = 0;
  damageClass = 0;
  damageValue = 0.264;
  damageType = $ChemDamageType;
  muzzleVelocity = 625.0;
  totalTime = 1.5;
  inheritedVelocityScale = 1.0;
  isVisible = True;
  tracerPercentage = 100.0;
  tracerLength = 30;
};

ItemImageData TranqGunImage 
{
  shapeFile = "needler";
  mountPoint = 0;
  weaponType = 0;
  ammoType = TranqAmmo;
  projectileType = TranqDart;
  accuFire = true;
  reloadTime = 1.5;
  fireTime = 0;
  lightType = 3;
  lightRadius = 6;
  lightTime = 2;
  lightColor = { 1.0, 0, 0 };
  sfxFire = SoundFireNeedler;
  sfxActivate = SoundPickUpWeapon;
};

ItemData TranqGun 
{
  description = "Needle Rifle";
  className = "Weapon";
  shapeFile = "needler";
  hudIcon = "blaster";
  heading = $InvHead[ihWel];
  shadowDetailMask = 4;
  imageType = TranqGunImage;
  price = 12;
  showWeaponBar = true;
};

function TranqGun::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Needle Rifle<f1>\nAn Eldar sniping weapon. It fires poison tipped needles at unfortunte targets.");
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Long Rifle
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[LongRifle] = 1;
$RemoteInvList[LongRifle] = 1;
$AutoUse[LongRifle] = False;
$WeaponAmmo[LongRifle] = SniperAmmo;

addWeapon(LongRifle);

RocketData LongRound 
{
  bulletShapeName = "enbolt.dts";
  explosionTag = bulletExp0;
  isVisible = False;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 0;
  damageValue = 0.45;
  damageType = $SniperDamageType;
  kickBackStrength = 0.0;
  muzzleVelocity = 1000.0;
  terminalVelocity = 1000.0;
  acceleration = 5.0;
  totalTime = 10.0;
  liveTime = 10.0;
  lightRange = 5.0;
  lightColor = { 0.4, 0.4, 1.0 };
  inheritedVelocityScale = 0.5;
  trailType = 1;
  trailLength = 100;
  trailWidth = 1.0;
  soundId = SoundDiscSpin;
};

ItemImageData LongRifleImage 
{
  shapeFile = "sniper";
  mountPoint = 0;
  weaponType = 0;
  ammoType = SniperAmmo;
  projectileType = LongRound;
  accuFire = true;
  reloadTime = 1.7;
  fireTime = 0.0;
  lightType = 3;
  lightRadius = 2;
  lightTime = 2;
  lightColor = 
  {
    1.0, 0, 0 }
  ;
  sfxFire = SoundPackFail;
  sfxReload = SoundPickupAmmo;
  sfxActivate = SoundPickupWeapon;
};


ItemData LongRifle 
{
  description = "Long Rifle";
  className = "Weapon";
  shapeFile = "sniper";
  hudIcon = "targetlaser";
  heading = $InvHead[ihWel];
  shadowDetailMask = 4;
  imageType = LongRifleImage;
  price = 15;
  showWeaponBar = true;
};

function LongRifle::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Long Rifle<f1>\nThe Eldar Ranger's main armament, it has a high payload, and a highly effective firing rate. A top notch sniping weapon.");
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-===
// Sniper Rifle
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[SniperRifle] = 1;
$RemoteInvList[SniperRifle] = 1;
$AutoUse[SniperRifle] = False;
$WeaponAmmo[SniperRifle] = SniperAmmo;

addWeapon(SniperRifle);

RocketData SniperRound 
{
  bulletShapeName = "bullet.dts";
  explosionTag = bulletExp0;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 0;
  damageValue = 0.6;
  damageType = $SniperDamageType;
  explosionRadius = 0.1;
  kickBackStrength = 600.0;
  muzzleVelocity = 1000.0;
  terminalVelocity = 1000.0;
  acceleration = 5.0;
  totalTime = 10.0;
  liveTime = 11.0;
  lightRange = 10.0;
  lightColor = 
  {
    0.25, 0.25, 1 }
  ;
  inheritedVelocityScale = 1.0;
  soundId = SoundJetHeavy;
};


function SniperRound::onAdd(%this)
{
	ShellEject(%this);
}
ItemImageData SniperRifleImage 
{
  shapeFile = "sniper";
  mountPoint = 0;
  weaponType = 0;
  ammoType = SniperAmmo;
  projectileType = SniperRound;
  accuFire = true;
  reloadTime = 2.3;
  fireTime = 0;
  lightType = 3;
  lightRadius = 6;
  lightTime = 2;
  lightColor = 
  {
    1.0, 0, 0 }
  ;
  sfxFire = SoundSnipeRifle;
  sfxActivate = SoundPickUpWeapon;
};

ItemData SniperRifle 
{
  description = "Sniper Rifle";
  className = "Weapon";
  shapeFile = "sniper";
  hudIcon = "targetlaser";
  heading = $InvHead[ihWma];
  shadowDetailMask = 4;
  imageType = SniperRifleImage;
  price = 13;
  showWeaponBar = true;
};

function SniperRifle::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Sniper Rifle<f1>\nThe marine Scouts trademark weapon. A headshot can kill almost any unit in a single blow.");
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Shotgun
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Shotgun] = 1;
$RemoteInvList[Shotgun] = 1;
$WeaponAmmo[Shotgun] = ShotgunAmmo;
$AutoUse[Shotgun] = False;

addWeapon(Shotgun);

//======================================================================== Shotgun Blast

BulletData ShotgunBlast
{
   bulletShapeName    = "bullet.dts";
   explosionTag       = bulletExp0;
   expRandCycle       = 3;
   mass               = 0.07;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.11;
   damageType         = $ShellDamageType;

   aimDeflection      = 0.019;
   muzzleVelocity     = 200.0;
   totalTime          = 1;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 2.0;
   tracerLength       = 30;
   soundId = SoundJetLight;
   
};

BulletData ShotgunBack
{
   bulletShapeName    = "";
   explosionTag       = bulletExp0;
   expRandCycle       = 3;
   mass               = 0.07;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.0;
   damageType         = $ShellDamageType;

   aimDeflection      = 0.019;
   muzzleVelocity     = 200.0;
   totalTime          = 1;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 2.0;
   tracerLength       = 30;
   soundId = SoundJetLight;
   
};


function ShotgunBack::onAdd(%this)
{
	ShellEject(%this);
}

//======================================================================== Boom Stick
ItemImageData ShotgunImage 
{
	shapeFile = "shotgun";
    mountPoint = 0;

	ammoType = ShotgunAmmo;
	//projectileType = "Undefined";
	weaponType = 0; // Single Shot
	reloadTime = 0.5;
	fireTime = 1.1;
	minEnergy = 5;
	maxEnergy = 6;
                        
	accuFire = false;

	 lightType = 3;
	 lightRadius = 3;
	 lightTime = 1;
	 lightColor = { 1.0, 0.7, 0.5 };

	sfxActivate = SoundPickUpWeapon;
	sfxFire     = SoundShotgunFire;
	sfxReload   = SoundMortarReload;
   
};

ItemData Shotgun
{
    description = "Shotgun";
	shapeFile = "shotgun";
	hudIcon = "blaster";
	heading = $InvHead[ihWma];
    className = "Weapon";
    shadowDetailMask = 4;
    imageType = ShotgunImage;
	showWeaponBar = true;
    price = 11;
};


function ShotgunImage::onFire(%player, %slot) 
{
 %AmmoCount = Player::getItemCount(%player, $WeaponAmmo[Shotgun]);
	 if(%AmmoCount) 
	 {
		 %client = GameBase::getOwnerClient(%player);
		 Player::decItemCount(%player,$WeaponAmmo[Shotgun],1);
		 %trans = GameBase::getMuzzleTransform(%player);
	     %vel = Item::getVelocity(%player);
	
			Projectile::spawnProjectile("ShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("ShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("ShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("ShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("ShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("ShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("ShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("ShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("ShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("ShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("ShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("ShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("ShotgunBack",%trans,%player,%vel);
		}
	else
		Client::sendMessage(Player::getClient(%player), 0,"Out Of Shells");

}

function Shotgun::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Shotgun<f1>\nOne of the few ancient human weapons that is still used in the 41st millenium. Short ranged, yet extremely deadly.");
}

function ShellEject(%this)
{
	%normalrot = "0 0 -0.785";
	%rand = getRandom();
	if(%rand <= 0.5)
	{
		%rot = GameBase::getRotation(%this);
		%startpos = Vector::getFromRot(%rot, -0.35);
		%rot = Vector::add(%rot, %normalrot);
		%casingpos = Vector::getFromRot(%rot, 1.0);
		%casingpos = Vector::add(%casingpos, %startpos);
	
		%rotx = getWord(%rot, 0) - %rand;
		%roty = getWord(%rot, 1);
		%rotz = getWord(%rot, 2) - 0.785;
		%newrot = %rotx@" "@%roty@" "@%rotz;

		%pos = GameBase::getPosition(%this);
		%pos = Vector::add(%pos, %casingpos);
	
		%this = newObject("","Mine","shell");
		addToSet("MissionCleanup", %this);
		GameBase::setPosition(%this, %pos);
		GameBase::setRotation(%this, %newrot);
	
		%strength = 0.2;
		%obj = newObject("","Mine","shell");
 		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj,%this,%strength,false);
	
		deleteObject(%this);
	}
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Eversor Gun A: modded from Boltgun
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[EvBolter] = 1;
$RemoteInvList[EvBolter] = 1;
$AutoUse[EvBolter] = False;
$WeaponAmmo[EvBolter] = EvBolterAmmo;

addWeapon(EvBolter);

BulletData EversorShot
{
   bulletShapeName    = "rocket.dts";
   explosionTag       = debrisExpSmall;
   expRandCycle = 1;
   damageClass        = 1;
   damageValue        = 0.24;
   damageType         = $MissileDamageType;
   explosionRadius    = 3.0;
   muzzleVelocity     = 500.0;
   totalTime          = 0.32;
   liveTime           = 0.32;

   lightRange         = 3.0;
   lightColor         = { 1.0, 0.25, 0.25 };
   inheritedVelocityScale = 0.5;
   isVisible          = True;

   rotationPeriod = 1;
};

function EversorShot::onAdd(%this)
{
	ShellEject(%this);
}

function ShellEject(%this)
{
	%normalrot = "0 0 -0.785";
	%rand = getRandom();
	if(%rand <= 0.5)
	{
		%rot = GameBase::getRotation(%this);
		%startpos = Vector::getFromRot(%rot, -0.35);
		%rot = Vector::add(%rot, %normalrot);
		%casingpos = Vector::getFromRot(%rot, 1.0);
		%casingpos = Vector::add(%casingpos, %startpos);
	
		%rotx = getWord(%rot, 0) - %rand;
		%roty = getWord(%rot, 1);
		%rotz = getWord(%rot, 2) - 0.785;
		%newrot = %rotx@" "@%roty@" "@%rotz;

		%pos = GameBase::getPosition(%this);
		%pos = Vector::add(%pos, %casingpos);
	
		%this = newObject("","Mine","shell");
		addToSet("MissionCleanup", %this);
		GameBase::setPosition(%this, %pos);
		GameBase::setRotation(%this, %newrot);
	
		%strength = 0.2;
		%obj = newObject("","Mine","shell");
 		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj,%this,%strength,false);
	
		deleteObject(%this);
	}
}
ItemImageData EvBolterImage 
{
  shapeFile = "eversor";
  mountPoint = 0;
  weaponType = 0;
  ammoType = EvBolterAmmo;
  projectileType = EversorShot;
  accuFire = true;
  reloadTime = 0.05;
  fireTime = 0.05;
  lightType = 3;
  lightRadius = 6;
  lightTime = 2;
  lightColor = { 0, 0, 0 };
  sfxFire = SoundEversorFire;
  sfxActivate = SoundPickUpWeapon;
};

ItemData EvBolter 
{
  description = "Eversor Autopistol";
  className = "Weapon";
  shapeFile = "eversor";
  hudIcon = "targetlaser";
  heading = $InvHead[ihWma];
  shadowDetailMask = 4;
  imageType = EvBolterImage;
  price = 12;
  showWeaponBar = true;
};

function EvBolter::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Eversor Autopistol<f1>\nThe upper barrel of the Eversor's pistol fires armor piercing rounds at a rapid rate.");
}

//-=-==-=-SHELL CASINGS-=-=-=-=-=-
MineData shell
{
   mass = 0.3;
   drag = 1.0;
   density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "shell";
   description = "shell";
   shapeFile = "force";
   shadowDetailMask = 4;
   explosionId = mineExp;
	explosionRadius = 0.0;
	damageValue = 0.0;
	damageType = $ShrapnelDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 2;
};

function shell::onAdd(%this)
{
	%data = GameBase::getDataName(%this);
	schedule("GameBase::startFadeOut("@%this@");", 0.0,%this);
	schedule("deleteObject("@%this@");", 2.5,%this);
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Poison Gun: modded from Boltgun
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Poison] = 1;
$RemoteInvList[Poison] = 1;
$AutoUse[Poison] = False;
$WeaponAmmo[Poison] =PoisonAmmo;

addWeapon(Poison);

BulletData PoisonBullet 
{
   bulletShapeName    = "laserhit.dts";
   explosionTag       = EverPoisExp;
   expRandCycle = 1;
   damageClass        = 0;
   damageValue        = 0.4;
   damageType         = $ChemDamageType;
   muzzleVelocity     = 1500.0;
   totalTime          = 0.8;
   liveTime           = 0.8;

   lightRange         = 3.0;
   lightColor         = { 1.0, 0.25, 0.25 };
   inheritedVelocityScale = 0.5;
   isVisible          = True;

   rotationPeriod = 1;
};

ItemImageData PoisonImage 
{
  shapeFile = "eversor";
  mountPoint = 0;
  weaponType = 0;
  ammoType = PoisonAmmo;
  projectileType = PoisonBullet;
  accuFire = true;
  reloadTime = 0.8;
  fireTime = 0.8;
  lightType = 3;
  lightRadius = 6;
  lightTime = 2;
  lightColor = { 0, 0, 0 };
  sfxFire = SoundFireBlaster;
  sfxActivate = SoundPickUpWeapon;
};

ItemData Poison 
{
  description = "Eversor Needler";
  className = "Weapon";
  shapeFile = "eversor";
  hudIcon = "targetlaser";
  heading = $InvHead[ihWma];
  shadowDetailMask = 4;
  imageType = PoisonImage;
  price = 35;
  showWeaponBar = true;
};

function Poison::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Eversor Needler<f1>\nThe lower barrel of the Eversor's pistol fires a biotoxin tipped shell. Lethal in the extreme.");
}



