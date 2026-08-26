//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Wraith Cannon
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Bolt] = 1;
$RemoteInvList[Bolt] = 1;
$AutoUse[Bolt] = False;
$WeaponAmmo[Bolt] = "";

addWeapon(Bolt);

RocketData EnergyCharge 
{
  bulletShapeName = "shockwave.dts";
  explosionTag = LargeShockwave;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 2.0;
  damageType = $DDamageType;
  explosionRadius = 10.5;
  kickBackStrength = 0.0;
  muzzleVelocity = 385.0;
  terminalVelocity = 1000.0;
  acceleration = 5.0;
  totalTime = 0.3;
  liveTime = 0.3;
  lightRange = 2.0;
  lightColor = { 1.20, 1.7, 1.5 };
  inheritedVelocityScale = 0.0;
  trailType = 1;
  trailLength = 2000;
  trailWidth = 3.0;
};

ItemImageData BoltImage 
{
  shapeFile = "shotgun";
  mountPoint = 0;
  weaponType = 0;
  projectileType = EnergyCharge;
  minEnergy = 30;
  maxEnergy = 35;
  reloadTime = 3.0;
  accuFire = true;
  lightType = 3;
  lightRadius = 2;
  lightTime = 1;
  lightColor = { 0.25, 0.25, 0.85 };
  sfxActivate = SoundPickUpWeapon;
  sfxFire = SoundELFIdle;
};

ItemData Bolt 
{
  description = "Wraith Cannon";
  shapeFile = "shotgun";
  hudIcon = "energyRifle";
  className = "Weapon";
  heading = $InvHead[ihWel];
  shadowDetailMask = 4;
  imageType = BoltImage;
  showWeaponBar = true;
  price = 12;
};

function Bolt::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Wraith Cannon<f1>\nA close range horror, infinitely effective against heavy units.");
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Isolanth : Storm Demon Aspects weapon
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Demogun] = 1;
$RemoteInvList[Demogun] = 1;
$AutoUse[Demogun] = False;
$WeaponAmmo[Demogun] = "DemoGunAmmo";

addWeapon(Demogun);

//======================================================================== Isolanth

RocketData IsoBurst
{
  bulletShapeName = "shield.dts";
  explosionTag = isoExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.462;
  damageType = $DDamageType;
  explosionRadius = 7.2;
  kickBackStrength = 100.0;
  muzzleVelocity = 285.0;
  terminalVelocity = 830.0;
  acceleration = 5.0;
  totalTime = 10.28;
  liveTime = 10.28;
  lightRange = 2.0;
  lightColor = { 0.50, 0.0, 0.50 };
  inheritedVelocityScale = 0.0;
  trailType = 2;
  trailString = "shield.dts";
  smokeDist = 1.0;
};

ItemImageData DemoGunImage 
{
  shapeFile = "isolanth";
  mountPoint = 0;
  weaponType = 0;
  reloadTime = 0.8;
  fireTime = 0.8;
  ammoType = DemoGunAmmo;
  //projectileType = "Undefined";
  accuFire = true;
  sfxFire = SoundIsolanthFire;
  sfxActivate = SoundPickUpWeapon;
};

function DemoGunImage::onFire(%player, %slot) 
 {
	 	 %Ammo = Player::getItemCount(%player,$WeaponAmmo[DemoGun]);
		
		 %playerId = Player::getClient(%player);
		 if(%Ammo) 
		 {
			 %client = GameBase::getOwnerClient(%player);
			 Player::decItemCount(%player,DemoGunAmmo,1);
			 %trans = GameBase::getMuzzleTransform(%player);
		     %vel = Item::getVelocity(%player);


			if (%playerId.IsoOpt == 0)
			{
Projectile::spawnProjectile("IsoBurst",%trans,%player,%vel);
			}
			else if (%playerId.IsoOpt == 1)
			{
				Projectile::spawnProjectile("IsoCharge",%trans,%player,%vel);
Player::decItemCount(%player,$WeaponAmmo[DemoGun],2);
			}
			else if (%playerId.IsoOpt == 2)
			{
Projectile::spawnProjectile("IsoSpread",%trans,%player,(%vel +10));
Projectile::spawnProjectile("IsoSpread",%trans,%player,(%vel + 5));
Projectile::spawnProjectile("IsoSpread",%trans,%player,(%vel + 1));
Projectile::spawnProjectile("IsoSpread",%trans,%player,(%vel - 5));
Projectile::spawnProjectile("IsoSpread",%trans,%player,(%vel - 10));
Player::decItemCount(%player,$WeaponAmmo[DemoGun],2);
			}
			else if (%playerId.IsoOpt == 3)
			{
Projectile::spawnProjectile("IsoForce",%trans,%player,%vel);
Player::decItemCount(%player,$WeaponAmmo[DemoGun],2);
			}
			else if (%playerId.IsoOpt == 4)
			{
Projectile::spawnProjectile("IsoLash",%trans,%player,%vel);
			}
			else if (%playerId.IsoOpt == 5)
			{
Projectile::spawnProjectile("IsoTerror",%trans,%player,%vel);
Player::decItemCount(%player,$WeaponAmmo[DemoGun],7);
			}
			else if (%playerId.IsoOpt == 6)
			{
Projectile::spawnProjectile("IsoScorch",%trans,%player,%vel);
			}
			
		}
}
ItemData DemoGun 
{
  heading = $InvHead[ihWel];
  description = "Isolanth";
  className = "Weapon";
  shapeFile = "isolanth";
  hudIcon = "plasma";
  shadowDetailMask = 4;
  imageType = DemoGunImage;
  price = 17;
  showWeaponBar = true;
};

function DemoGun::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Isolanth<f1>\nThe weapon of the Storm Daemon. Fires a variety of charged particle types, each deadly in it's own right.");
}


RocketData IsoSpread
{
  bulletShapeName = "shield.dts";
   explosionTag    = isoExp;
   collisionRadius = 0.0;
   mass            = 2.0;
   damageClass      = 0;
   damageValue      = 0.24136;
   damageType       = $DDamageType;
   kickBackStrength = 0.0;
   muzzleVelocity   = 270.0;
   terminalVelocity = 450.0;
   acceleration     = 0.0;
   totalTime        = 10;
   liveTime         = 10;
   lightRange       = 5.0;
   lightColor       = { 0, 0, 1 };
   inheritedVelocityScale = 1;
   soundId = SoundJetHeavy;
};

RocketData IsoCharge
{
  bulletShapeName = "shield.dts";
  explosionTag = LargeShockwave;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.36;
  damageType = $DeathDamageType;
  explosionRadius = 15.3;
  kickBackStrength = 55.0;
  muzzleVelocity = 185.0;
  terminalVelocity = 330.0;
  acceleration = 5.0;
  totalTime = 10.28;
  liveTime = 10.28;
  lightRange = 1.0;
  lightColor = { 0.50, 0.0, 0.50 };
  inheritedVelocityScale = 0.0;
  trailType = 2;
  trailString = "hflame.dts";
  smokeDist = 1.0;
};

GrenadeData IsoForce
{
  bulletShapeName = "shield.dts";
  explosionTag = LargeShockwave;
  collideWithOwner = True;
  ownerGraceMS = 250;
  collisionRadius = 0.3;
  mass = 0.6;
  elasticity = 0.15;
  damageClass = 1;
  damageValue = 0.8;
  damageType = $DDamageType;
  explosionRadius = 20.0;
  kickBackStrength = 50.0;
  maxLevelFlightDist = 150;
  totalTime = 30.0;
  liveTime = 1.0;
  projSpecialTime = 0.05;
  inheritedVelocityScale = 0.5;
  smokeName = "shield.dts";
}
;

RocketData IsoLash
{
  bulletShapeName = "shield.dts";
  explosionTag = isoExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.468;
  damageType = $BlasterDamageType;
  explosionRadius = 16;
  muzzleVelocity = 285.0;
  terminalVelocity = 830.0;
  acceleration = 5.0;
  totalTime = 10.28;
  liveTime = 10.28;
  lightRange = 1.0;
  lightColor = { 0.50, 0.0, 0.50 };
  inheritedVelocityScale = 0.0;
  trailType = 2;
  trailString = "shockwave.dts";
  smokeDist = 1.0;
};

RocketData IsoTerror
{
  bulletShapeName = "shield.dts";
  explosionTag = isoExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 1.5;
  damageType = $DDamageType;
  explosionRadius = 20.0;
  muzzleVelocity = 15.0;
  terminalVelocity = 20.0;
  acceleration = 5.0;
  totalTime = 10.28;
  liveTime = 10.28;
  lightRange = 1.0;
  lightColor = { 0.50, 0.0, 0.50 };
  inheritedVelocityScale = 0.0;
  trailType = 2;
  trailString = "dustplume.dts";
  smokeDist = 1.0;
};

RocketData IsoScorch
{
  bulletShapeName = "shield.dts";
  explosionTag = isoExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.365;
  damageType = $PlasmaDamageType;
  explosionRadius = 10.0;
  muzzleVelocity = 185.0;
  terminalVelocity = 430.0;
  acceleration = 5.0;
  totalTime = 10.28;
  liveTime = 10.28;
  lightRange = 1.0;
  lightColor = { 0.50, 0.0, 0.50 };
  inheritedVelocityScale = 0.0;
  trailType = 2;
  trailString = "plasmabolt.dts";
  smokeDist = 1.0;
};

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Haywire Launcher
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[EMP] = 1;
$RemoteInvList[EMP] = 1;
$AutoUse[EMP] = False;
$WeaponAmmo[EMP] = EMPAmmo;

addWeapon(EMP);

GrenadeData ShockShell 
{
  bulletShapeName = "mortar.dts";
  explosionTag = Shockwave;
  collideWithOwner = True;
  ownerGraceMS = 250;
  collisionRadius = 0.3;
  mass = 1.0;
  elasticity = 0.25;
  damageClass = 1;
  damageValue = 0.1;
  damageType = $FlashDamageType;
  explosionRadius = 30.0;
  kickBackStrength = 0.0;
  maxLevelFlightDist = 150;
  totalTime = 30.0;
  liveTime = 1.0;
  projSpecialTime = 0.05;
  inheritedVelocityScale = 0.5;
  smokeName = "mortartrail.dts";
}
;

ItemImageData EMPImage
{
	shapeFile = "grenadeL";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	ammoType = EMPAmmo;
	projectileType = ShockShell;
	accuFire = false;
	reloadTime = 1.5;
	fireTime = 1.5;

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = SoundFireSeeking;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundMortarReload;
};

ItemData EMP
{
	description = "Haywire Launcher";
	className = "Weapon";
	shapeFile = "grenadeL";
	hudIcon = "plasma";
   heading = $InvHead[ihWel];
	shadowDetailMask = 4;
	imageType = EMPImage;
	price = 15;
	showWeaponBar = true;
};

function EMP::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Haywire Launcher<f1>\nFires an Electromagnetic bomblet, disrupting energy systems.");
}


//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Grenade Launcher (GrenadeLauncher)
//  By Dynamix
//
//  Alliance version by Mjolnir, 
//    see Contrib.txt
//
//  For installation information, see Install.txt
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$InvList[GrenadeLauncher] = 1;
$RemoteInvList[GrenadeLauncher] = 1;
$AutoUse[GrenadeLauncher] = False;
$WeaponAmmo[GrenadeLauncher] = GrenadeAmmo;

addWeapon(GrenadeLauncher);

GrenadeData GrenadeShell 
{
  bulletShapeName = "grenade.dts";
  explosionTag = grenadeExp;
  collideWithOwner = True;
  ownerGraceMS = 250;
  collisionRadius = 0.2;
  mass = 1.0;
  elasticity = 0.45;
  damageClass = 1;
  damageValue = 0.4;
  damageType = $ShrapnelDamageType;
  explosionRadius = 15;
  kickBackStrength = 150.0;
  maxLevelFlightDist = 150;
  totalTime = 30.0;
  liveTime = 1.0;
  projSpecialTime = 0.05;
  inheritedVelocityScale = 0.5;
  smokeName = "smoke.dts";
};

ItemImageData GrenadeLauncherImage 
{
  shapeFile = "grenadeL";
  mountPoint = 0;
  weaponType = 0;
  ammoType = GrenadeAmmo;
  //projectileType = "Undefined";
  accuFire = false;
  reloadTime = 0.5;
  fireTime = 0.5;
  lightType = 3;
  lightRadius = 3;
  lightTime = 1;

  lightColor = { 0.6, 1, 1.0 };
  sfxFire = SoundFireGrenade;
  sfxActivate = SoundPickUpWeapon;
  sfxReload = SoundDryFire;
};

function GrenadeLauncherImage::onFire(%player, %slot) 
{
	 	 %Ammo = Player::getItemCount(%player, $WeaponAmmo[GrenadeLauncher]);
		
		 %playerId = Player::getClient(%player);
		 if(%Ammo) 
		 {
			 %client = GameBase::getOwnerClient(%player);
			 Player::decItemCount(%player,$WeaponAmmo[GrenadeLauncher],1);
			 %trans = GameBase::getMuzzleTransform(%player);
		     %vel = Item::getVelocity(%player);


			if (%playerId.GLOpt == 0)
			{
				Projectile::spawnProjectile("GrenadeShell",%trans,%player,%vel);
			}
			else if (%playerId.GLOpt == 1)
			{
				Projectile::spawnProjectile("HaywireGrenade",%trans,%player,%vel);
			}
			else if (%playerId.GLOpt == 2)
			{
				Projectile::spawnProjectile("HellfireGrenade",%trans,%player,(%vel + 24));
Projectile::spawnProjectile("HellfireGrenade",%trans,%player,(%vel + 12));
Projectile::spawnProjectile("HellfireGrenade",%trans,%player,(%vel + 1));
Projectile::spawnProjectile("HellfireGrenade",%trans,%player,(%vel - 12));
Projectile::spawnProjectile("HellfireGrenade",%trans,%player,(%vel - 24));
Player::decItemCount(%player,$WeaponAmmo[GrenadeLauncher],4);
			}
			else if (%playerId.GLOpt == 3)
			{
				Projectile::spawnProjectile("PlasmaGrenade",%trans,%player,%vel);
			}
			else if (%playerId.GLOpt == 4)
			{
				Projectile::spawnProjectile("KrakGrenade",%trans,%player,%vel);
Player::decItemCount(%player,$WeaponAmmo[GrenadeLauncher],2);
			}
			else if (%playerId.GLOpt == 5)
			{
				Projectile::spawnProjectile("InfernoGrenade",%trans,%player,%vel);
			}
		}
}

ItemData GrenadeLauncher 
{
  description = "Grenade Launcher";
  className = "Weapon";
  shapeFile = "grenadeL";
  hudIcon = "grenade";
  heading = $InvHead[ihWea];
  shadowDetailMask = 4;
  imageType = GrenadeLauncherImage;
  price = 15;
  showWeaponBar = true;
};

function GrenadeLauncher::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Grenade Launcher<f1>\nFiring several different types of grenades gives this weapon a feared reputation.");
}

//-=-=-=-=-NEW GRENADE AMMO VARIANTS
//Haywire Grenades
GrenadeData HaywireGrenade
{
  bulletShapeName = "paint.dts";
  explosionTag = turretExp;
  collideWithOwner = True;
  ownerGraceMS = 250;
  collisionRadius = 0.2;
  mass = 1.0;
  elasticity = 0.45;
  damageClass = 1;
  damageValue = 0.1;
  damageType = $FlashDamageType;
  explosionRadius = 15;
  kickBackStrength = 150.0;
  maxLevelFlightDist = 150;
  totalTime = 30.0;
  liveTime = 1.0;
  projSpecialTime = 0.05;
  inheritedVelocityScale = 0.5;
  smokeName = "smoke.dts";
};
//Mirv grenades

GrenadeData HellfireGrenade
{
bulletShapeName = "grenade.dts";
  explosionTag = grenadeExp;
  collideWithOwner = True;
  ownerGraceMS = 250;
  collisionRadius = 0.2;
  mass = 1.0;
  elasticity = 0.45;
  damageClass = 1;
  damageValue = 0.4;
  damageType = $ShrapnelDamageType;
  explosionRadius = 15;
  kickBackStrength = 150.0;
  maxLevelFlightDist = 150;
  totalTime = 30.0;
  liveTime = 1.0;
  projSpecialTime = 0.05;
  inheritedVelocityScale = 0.5;
  smokeName = "smoke.dts";
};

//Plasma Grenades
GrenadeData PlasmaGrenade
{
  bulletShapeName = "plasmabolt.dts";
  explosionTag = PlasCanExp;
  collideWithOwner = True;
  ownerGraceMS = 250;
  collisionRadius = 0.2;
  mass = 1.0;
  elasticity = 0.32;
  damageClass = 1;
  damageValue = 0.5;
  damageType = $FlamerDamageType;
  explosionRadius = 25;
  kickBackStrength = 150.0;
  maxLevelFlightDist = 150;
  totalTime = 30.0;
  liveTime = 1.0;
  projSpecialTime = 0.05;
  inheritedVelocityScale = 0.5;
  smokeName = "plasmatrail.dts";
};
  
//Krak Grenades
GrenadeData KrakGrenade
{
  bulletShapeName = "grenade.dts";
  explosionTag = bulletExp0;
  collideWithOwner = True;
  ownerGraceMS = 250;
  collisionRadius = 0.2;
  mass = 1.0;
  elasticity = 0.4;
  damageClass = 1;
  damageValue = 1.6;
  damageType = $KrakenDamageType;
  explosionRadius = 15.0;
  kickBackStrength = 150.0;
  maxLevelFlightDist = 150;
  totalTime = 30.0;
  liveTime = 1.0;
  projSpecialTime = 0.05;
  inheritedVelocityScale = 0.5;
  smokeName = "dustplume.dts";
};
//Inferno Grenades

GrenadeData InfernoGrenade
{
  bulletShapeName = "grenade.dts";
  explosionTag = FireExp;
  collideWithOwner = True;
  ownerGraceMS = 250;
  collisionRadius = 0.2;
  mass = 1.0;
  elasticity = 0.45;
  damageClass = 1;
  damageValue = 0.4;
  damageType = $PlasmaDamageType;
  explosionRadius = 15;
  kickBackStrength = 150.0;
  maxLevelFlightDist = 150;
  totalTime = 30.0;
  liveTime = 1.0;
  projSpecialTime = 0.05;
  inheritedVelocityScale = 0.5;
  smokeName = "snowplume.dts";
};

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Webgun/Deathspinner
//
//  For installation information, see Install.txt
//  Created by C|one
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Webgun] = 1;
//$InvList[WebAmmo] = 1;
$RemoteInvList[Webgun] = 1;
//$RemoteInvList[WebAmmo] = 1;
//$SellAmmo[webAmmo] = 15;
//$WeaponAmmo[Webgun] = "";
$AutoUse[Webgun] = true;

addWeapon(Webgun);
//addAmmo(Webgun, WebAmmo, 15);


//======================================================================== Shotgun Blast

RocketData Webber
{
  bulletShapeName = "breath.dts";
  explosionTag = webExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 0;
  damageValue = 0.187;
  damageType = $BlasterDamageType;
  kickBackStrength = 0.0;
  muzzleVelocity = 285.0;
  terminalVelocity = 530.0;
  acceleration = 5.0;
  totalTime = 0.18;
  liveTime = 0.18;
  lightRange = 2.0;
  lightColor = { 1.20, 1.7, 1.5 };
  inheritedVelocityScale = 0.0;
  trailType = 2;
  trailString = "breath.dts";
  smokeDist = 1.0;
};

//======================================================================== Web Shells
//ItemData WebAmmo
//{
//	description = "Web Fluid";
//	className = "Ammo";
 //   heading = $InvHead[ihAmm];
//	shapeFile = "ammo1";
//	shadowDetailMask = 4;
//	price = 0;
//};

ItemImageData WebgunImage 
{
  shapeFile = "mortargun";
  mountPoint = 0;
  weaponType = 0;
  reloadTime = 0.157;
  fireTime = 0.0;
//  ammoType = "";
  minEnergy = 1;
  maxEnergy = 1;
  projectileType = Webber;
  accuFire = true;
  lightType = 3;
  lightRadius = 3;
  lightTime = 1;
  lightColor = { 0.1, 0.3, 0.6 };
  //sfxFire = SoundDeathspin;
};

ItemData Webgun
{
    description = "Deathspinner";
	shapeFile = "mortargun";
	hudIcon = "blaster";
	heading = $InvHead[ihWel];
    className = "Weapon";
    shadowDetailMask = 4;
    imageType = WebgunImage;
	showWeaponBar = true;
    price = 23;
};

function WebGun::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Deathspinner<f1>\nThe Warpspiders prized weapon. Capable of decimating any target with its rapid beams of destruction.");
}



