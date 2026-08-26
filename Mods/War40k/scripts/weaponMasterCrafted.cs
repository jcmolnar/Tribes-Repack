//=-=-=-==-=-=-==-=-=-=-=-=-=-=-=-=-=-=-=-=-
// MASTER CRAFTED WEAPONS for VETERAN SYSTEM
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=


//-=-=-=-=-=-==-=-=-=-=-=-=-=-=-=-=-=-=-
//   MARINE MASTER CRAFTED WEAPONS
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Master Crafted Shotgun
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[MShotgun] = 1;
$RemoteInvList[MShotgun] = 1;
$WeaponAmmo[MShotgun] = ShotgunAmmo;
$AutoUse[MShotgun] = False;

addWeapon(MShotgun);

//======================================================================== MShotgun Blast

BulletData MShotgunBlast
{
   bulletShapeName    = "bullet.dts";
   explosionTag       = debrisExpsmall;
   expRandCycle       = 3;
   mass               = 0.07;
   bulletHoleIndex    = 0;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.21;
   explosionradius = 4.0;
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

BulletData MShotgunBack
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


function MShotgunBack::onAdd(%this)
{
	ShellEject(%this);
}

//======================================================================== Boom Stick
ItemImageData MShotgunImage 
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

ItemData MShotgun
{
    description = "M.Crafted Shotgun";
	shapeFile = "shotgun";
	hudIcon = "blaster";
	heading = $InvHead[ihWma];
    className = "Weapon";
    shadowDetailMask = 4;
    imageType = MShotgunImage;
	showWeaponBar = true;
    price = 11;
};


function MShotgunImage::onFire(%player, %slot) 
{
 %AmmoCount = Player::getItemCount(%player, $WeaponAmmo[MShotgun]);
	 if(%AmmoCount) 
	 {
		 %client = GameBase::getOwnerClient(%player);
		 Player::decItemCount(%player,$WeaponAmmo[MShotgun],1);
		 %trans = GameBase::getMuzzleTransform(%player);
	     %vel = Item::getVelocity(%player);
	
			Projectile::spawnProjectile("MShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("MShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("MShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("MShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("MShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("MShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("MShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("MShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("MShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("MShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("MShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("MShotgunBlast",%trans,%player,%vel);
			Projectile::spawnProjectile("MShotgunBack",%trans,%player,%vel);
		}
	else
		Client::sendMessage(Player::getClient(%player), 0,"Out Of Shells");

}

function MShotgun::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Master Crafted Shotgun<f1>\nThe standard shotgun model has been improved upon, and made flawlessly. Very deadly.");
}



//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Master Crafted Bolter
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[IonBolter] = 1;
$RemoteInvList[IonBolter] = 1;
$AutoUse[IonBolter] = False;
$WeaponAmmo[IonBolter] = "";

addWeapon(IonBolter);

RocketData IBolt
{
  bulletShapeName = "shotgunbolt.dts";
  explosionTag = blasterExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.23;
  damageType = $DeathDamageType;
  explosionRadius = 6;
  kickBackStrength = 0.0;
  muzzleVelocity = 200.0;
  terminalVelocity = 600.0;
  acceleration = 5.0;
  totalTime = 2.0;
  liveTime = 2.0;
  lightRange = 5.0;
  lightColor = { 1.0, 0.7, 0.5 };
  inheritedVelocityScale = 0.5;
  trailType = 1;
  trailLength = 50;
  trailWidth = 0.3;
  soundId = SoundJetHeavy;
};
ItemImageData IonBolterImage
{
  shapeFile = "bolter";
  mountPoint = 0;
  weaponType = 0;
  reloadTime = 0.1;
  fireTime = 0.1;
  minEnergy = 6;
  maxEnergy = 12;
  projectileType = IBolt;
  accuFire = true;
  sfxFire = SoundFusionFire;
  sfxActivate = SoundPickUpWeapon;
};

ItemData IonBolter 
{
  heading = $InvHead[ihWma];
  description = "Master Crafted Boltgun";
  className = "Weapon";
  shapeFile = "bolter";
  hudIcon = "shotgun";
  shadowDetailMask = 4;
  imageType = IonBolterImage;
  price = 12;
  showWeaponBar = true;
};

function IonBolter::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>Master Crafted Boltgun<f1>\nThe upgraded bolter, modified to use energy blasts, and flawlessly constructed.");
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Master Crafted Storm Bolter 
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$AutoUse[MStBolter] = False;
$WeaponAmmo[MStBolter] = StBolterAmmo;
$InvList[MStBolter] = 1;
$RemoteInvList[MStBolter] = 1;

addWeapon(MStBolter);

RocketData MStBolterBullet 
{
  bulletShapeName = "shotgunbolt.dts";
  explosionTag = blasterExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.18;
  damageType = $DeathDamageType;
  explosionRadius = 6;
  kickBackStrength = 0.0;
  muzzleVelocity = 200.0;
  terminalVelocity = 600.0;
  acceleration = 5.0;
  totalTime = 2.0;
  liveTime = 2.0;
  lightRange = 5.0;
  lightColor = { 1.0, 0.7, 0.5 };
  inheritedVelocityScale = 0.5;
  trailType = 1;
  trailLength = 50;
  trailWidth = 0.3;
  soundId = SoundJetHeavy;
};

ItemImageData MStBolterImage 
{
  shapeFile = "sbolter";
  mountPoint = 0;
  weaponType = 0;
  reloadTime = 0.08;
  fireTime = 0.08;
  ammoType= StBolterAmmo;
  //projectileType = "Undefined";
  accuFire = false;
  lightType = 3;
  lightRadius = 3;
  lightTime = 1;
  lightColor = { 0.6, 1, 1 };  
  sfxFire = SoundBolterFire;
  sfxActivate = SoundPickUpWeapon;
};

function MStBolterImage::onFire(%player,%slot)
{
	 	 %Ammo = Player::getItemCount(%player, $WeaponAmmo[MStBolter]);
		
		 %playerId = Player::getClient(%player);
		 if(%Ammo) 
		 {
			 %client = GameBase::getOwnerClient(%player);
			 Player::decItemCount(%player,$WeaponAmmo[MStBolter],1);
			 %trans = GameBase::getMuzzleTransform(%player);
		     %vel = Item::getVelocity(%player);
Projectile::spawnProjectile("MStBolterBullet",%trans,%player,(%vel+5));
Projectile::spawnProjectile("MStBolterBullet",%trans,%player,(%vel-5));
			}
			}


ItemData MStBolter 
{
  description = "Master Crafted Storm Bolter";
  className = "Weapon";
  shapeFile = "sbolter";
  hudIcon = "shotgun";
  heading = $InvHead[ihWma];
  shadowDetailMask = 4;
  imageType = MStBolterImage;
  price = 14;
  showWeaponBar = true;
};

function MStBolter::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>Master Crafted Storm Bolter<f1>\nThe ultimate stormbolter, crafted flawlessly, and modified to eject ion rounds.");
}


//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// ELDAR MASTER CRAFTED WEAPONS
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Spirit Catapult
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[MShuriken] = 1;
$RemoteInvList[MShuriken] = 1;
$AutoUse[MShuriken] = False;
$WeaponAmmo[MShuriken] = "";

addWeapon(MShuriken);

RocketData MShurikenBolt
{
  bulletShapeName = "enbolt.dts";
  explosionTag = turretExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.23;
  damageType = $ShurikenDamageType;
  explosionRadius = 6;
  kickBackStrength = 0.0;
  muzzleVelocity = 200.0;
  terminalVelocity = 600.0;
  acceleration = 5.0;
  totalTime = 2.0;
  liveTime = 2.0;
  lightRange = 5.0;
  lightColor = { 1.0, 0.7, 0.5 };
  inheritedVelocityScale = 0.5;
  trailType = 1;
  trailLength = 50;
  trailWidth = 0.3;
  soundId = SoundJetHeavy;
};
ItemImageData MShurikenImage
{
  shapeFile = "shur";
  mountPoint = 0;
  weaponType = 0;
  reloadTime = 0.1;
  fireTime = 0.1;
  minEnergy = 6;
  maxEnergy = 12;
  projectileType = MShurikenBolt;
  accuFire = true;
  sfxFire = SoundFusionFire;
  sfxActivate = SoundPickUpWeapon;
};

ItemData MShuriken 
{
  heading = $InvHead[ihWel];
  description = "Spirit Catapult";
  className = "Weapon";
  shapeFile = "shur";
  hudIcon = "shotgun";
  shadowDetailMask = 4;
  imageType = MShurikenImage;
  price = 12;
  showWeaponBar = true;
};

function MShuriken::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>Spirit Catapult<f1>\nModified to eject bolts charged by the spirit of Kaela Mensha Khaine.");
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Vibro Cannon
//  by <[DC]>Paladin 
//
//  For installation information, see Install.txt
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$InvList[VibroCannon] = 1;
$RemoteInvList[VibroCannon] = 1;
$AutoUse[VibroCannon] = False;
$WeaponAmmo[VibroCannon] = VibroCannonAmmo;

addWeapon(VibroCannon);

RocketData VibroCannonShot
{
  bulletShapeName = "shield.dts";
  explosionTag = mortarExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.85;
  damageType = $DDamageType;
  explosionRadius = 20.5;
  kickBackStrength = -270.0;
  muzzleVelocity = 65.0;
  terminalVelocity = 130.0;
  acceleration = 5.0;
  totalTime = 10.0;
  liveTime = 10.0;
  lightRange = 2.0;
  lightColor = { 1.20, 1.7, 1.5 };
  inheritedVelocityScale = 0.0;
  trailType = 2;
  trailString = "snowplume.dts";
  smokeDist = 1.0;
  soundId = SoundJetHeavy;
};

ItemImageData VibroCannonImage 
{
  shapeFile = "GrenadeL";
  mountPoint = 0;
  weaponType = 0;
  ammoType = VibroCannonAmmo;
  projectileType = VibroCannonShot;
  accuFire = true;
  reloadTime = 2.0;
  fireTime = 0.0;
  lightType = 3;
  lightRadius = 6;
  lightTime = 2;
  lightColor = { 0, 0, 3.0 };
  sfxFire = SoundFireMortar;
  sfxActivate = SoundPickUpWeapon;
  sfxReady = SoundMortarIdle;
};

ItemData VibroCannon
{
  description = "Vibro Cannon";
  className = "Weapon";
  shapeFile = "GrenadeL";
  hudIcon = "targetlaser";
  heading = $InvHead[ihWel];
  shadowDetailMask = 4;
  imageType = VibroCannonImage;
  price = 19;
  showWeaponBar = true;
};

function VibroCannon::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Vibro Cannon<f1>\nSonic blasts which amplify sound, creating a deadly vortex which distorts the area of impact.");
}


//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// ELDAR MASTER CRAFTED WEAPONS
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Prism Launcher
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Prism] = 1;
$RemoteInvList[Prism] = 1;
$AutoUse[Prism] = False;
$WeaponAmmo[Prism] = "";

addWeapon(Prism);

RocketData PrismBolt
{
  bulletShapeName = "laserhit.dts";
  explosionTag = rocketExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.01;
  damageType = $DDamageType;
  explosionRadius = 45;
  kickBackStrength = 150.0;
  muzzleVelocity = 1000.0;
  terminalVelocity = 2000.0;
  acceleration = 5.0;
  totalTime = 8.0;
  liveTime = 8.0;
  lightRange = 5.0;
  lightColor = { 1.0, 0.7, 0.5 };
  inheritedVelocityScale = 0.5;
  trailType = 1;
  trailLength = 2000;
  trailWidth = 1.5;
  soundId = SoundJetHeavy;
};
ItemImageData PrismImage
{
  shapeFile = "mortargun";
  mountPoint = 0;
  weaponType = 0;
  reloadTime = 1.0;
  fireTime = 0.0;
  minEnergy = 24;
  maxEnergy = 36;
  projectileType = PrismBolt;
  accuFire = true;
  sfxFire = SoundFusionFire;
  sfxActivate = SoundPickUpWeapon;
};

ItemData Prism 
{
  heading = $InvHead[ihWel];
  description = "Prism Launcher";
  className = "Weapon";
  shapeFile = "mortargun";
  hudIcon = "shotgun";
  shadowDetailMask = 4;
  imageType = PrismImage;
  price = 12;
  showWeaponBar = true;
};

function Prism::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>Prism Launcher<f1>\nA brutal laser weapon, which superheats matter on impact, detonating.");
}