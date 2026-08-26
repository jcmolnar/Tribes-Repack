//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Bolt Pistol
//
//  For installation information, see Install.txt
//  Created by Edgecrusher
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[BoltPist] = 1;
$RemoteInvList[BoltPist] = 1;
$AutoUse[BoltPist] = False;
$WeaponAmmo[BoltPist] = BoltPistAmmo;

addWeapon(BoltPist);

RocketData BoltPistBullet 
{
  bulletShapeName = "shotgunbolt.dts";
   explosionTag    = debrisExpSmall;
   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;
   damageValue      = 0.25;
   damageType       = $MissileDamageType;

   explosionRadius  = 3.0;
   kickBackStrength = 0.0;

   muzzleVelocity   = 270.0;
   terminalVelocity = 2000.0;
   acceleration     = 0.0;

   totalTime        = 0.3;
   liveTime         = 0.3;

   lightRange       = 5.0;
   lightColor       = { 0, 0, 1 };

   inheritedVelocityScale = 1;

   soundId = SoundJetHeavy;
};

ItemImageData BoltPistImage 
{
  shapeFile = "boltpist";
  mountPoint = 0;
  weaponType = 0;
  MountOffset = { 0.0, 0.0, -0.05 };
  ammoType = BoltPistAmmo;
//  projectileType = "Undefined";
  accuFire = false;
  reloadTime = 0.1;
  fireTime = 0.05;
  lightType = 3;
  lightRadius = 6;
  lightTime = 2;
  lightColor = { 0, 0, 0 };
  sfxFire = SoundBolterFire;
  sfxActivate = SoundPickUpWeapon;
};

function BoltPistImage::onFire(%player, %slot) 
{
	 	 %Ammo = Player::getItemCount(%player,$WeaponAmmo[BoltPist]);
		
		 %playerId = Player::getClient(%player);
		 if(%Ammo) 
		 {
			 %client = GameBase::getOwnerClient(%player);
			 Player::decItemCount(%player,BoltPistAmmo,1);
			 %trans = GameBase::getMuzzleTransform(%player);
		     %vel = Item::getVelocity(%player);


			if (%playerId.MOOpt == 0)
			{
				Projectile::spawnProjectile("BoltPistBullet",%trans,%player,%vel);
Projectile::spawnProjectile("BolterTracer",%trans,%player,%vel);
			}
			else if (%playerId.MOOpt == 1)
			{
				Projectile::spawnProjectile("BoltPistHellfire",%trans,%player,%vel);
			}
			else if (%playerId.MOOpt == 2)
			{
				Projectile::spawnProjectile("BoltPistKrak",%trans,%player,%vel);
Projectile::spawnProjectile("BolterTracer",%trans,%player,%vel);
			}
			
		}
}
ItemData BoltPist 
{
  description = "Bolt Pistol";
  className = "Weapon";
  shapeFile = "boltpist";
  hudIcon = "shotgun";
  heading = $InvHead[ihWma];
  shadowDetailMask = 4;
  imageType = BoltPistImage;
  price = 3;
  showWeaponBar = true;
};

function BoltPist::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Bolt Pistol<f1>\nA pistol version of the versatile Boltgun.");
}

RocketData BoltPistKrak 
{
  bulletShapeName = "shotgunbolt.dts";
  explosionTag = debrisExpSmall;
  expRandCycle = 1;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.12;
  damageType = $KrakenDamageType;
  explosionRadius = 3.5;
  kickBackStrength = 0.0;
  muzzleVelocity = 165.0;
  terminalVelocity = 2000.0;
  aimDeflection = 0.003;
  acceleration = 100.0;
  totalTime = 0.3;
  liveTime = 0.3;
  lightRange = 5.0;
  lightColor = { 0.0, 0.0, 0.0 };
  inheritedVelocityScale = 0.5;
  trailType = 2;
  trailString = "rsmoke.dts";
  smokeDist = 1.0;
  soundId = SoundJetHeavy;
};
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Boltgun
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Bolter] = 1;
$RemoteInvList[Bolter] = 1;
$AutoUse[Bolter] = False;
$WeaponAmmo[Bolter] = BolterAmmo;

addWeapon(Bolter);

RocketData BolterBullet 
{
  bulletShapeName = "shotgunbolt.dts";
   explosionTag    = debrisExpSmall;
   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;
   damageValue      = 0.3;
   damageType       = $MissileDamageType;

   explosionRadius  = 3.0;
   kickBackStrength = 0.0;

   muzzleVelocity   = 270.0;
   terminalVelocity = 2000.0;
   acceleration     = 0.0;

   totalTime        = 0.9;
   liveTime         = 0.9;

   lightRange       = 5.0;
   lightColor       = { 0, 0, 1 };

   inheritedVelocityScale = 1;

   soundId = SoundJetHeavy;
};

BulletData BolterTracer
{
   bulletShapeName    = "";
   explosionTag       = bulletExp0;
   expRandCycle       = 3;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.1;
   damageType         = $missileDamageType;

   aimDeflection      = 0.006;
   muzzleVelocity     = 325.0;
   totalTime          = 0.75;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 0.0;
   tracerLength       = 0;
};


ItemImageData BolterImage 
{
  shapeFile = "bolter";
  mountPoint = 0;
  weaponType = 0;
  MountOffset = { 0.0, 0.0, -0.05 };
  ammoType = BolterAmmo;
  //projectileType = "Undefined";
  accuFire = false;
  reloadTime = 0.08;
  fireTime = 0.08;
  lightType = 3;
  lightRadius = 6;
  lightTime = 2;
  lightColor = { 0, 0, 0 };
  sfxFire = SoundBolterFire;
  sfxActivate = SoundPickUpWeapon;
};

function BolterImage::onFire(%player, %slot) 
{
	 	 %Ammo = Player::getItemCount(%player, $WeaponAmmo[Bolter]);
		
		 %playerId = Player::getClient(%player);
		 if(%Ammo) 
		 {
			 %client = GameBase::getOwnerClient(%player);
			 Player::decItemCount(%player,$WeaponAmmo[Bolter],1);
			 %trans = GameBase::getMuzzleTransform(%player);
		     %vel = Item::getVelocity(%player);


			if (%playerId.isgod == 1)
			{
				Projectile::spawnProjectile("BolterBullet",%trans,%player,%vel);
				Projectile::spawnProjectile("BolterInferno",%trans,%player,%vel);
				Projectile::spawnProjectile("BolterSlug",%trans,%player,%vel);
				Projectile::spawnProjectile("BolterHellfire",%trans,%player,%vel);
				Projectile::spawnProjectile("BolterKraken",%trans,%player,%vel);
			}
			else if (%playerId.BOpt == 0)
			{
				Projectile::spawnProjectile("BolterBullet",%trans,%player,%vel);
Projectile::spawnProjectile("BolterTracer",%trans,%player,%vel);

			}
			else if (%playerId.BOpt == 1)
			{
				Projectile::spawnProjectile("BolterInferno",%trans,%player,%vel);
			 Player::decItemCount(%player,$WeaponAmmo[Bolter],1);
			}
			else if (%playerId.BOpt == 2)
			{
				Projectile::spawnProjectile("BolterSlug",%trans,%player,%vel);
			 Player::decItemCount(%player,$WeaponAmmo[Bolter],1);
			}
			else if (%playerId.BOpt == 3)
			{
				Projectile::spawnProjectile("BolterHellfire2",%trans,%player,%vel);
			 Player::decItemCount(%player,$WeaponAmmo[Bolter],1);
			}
			else if (%playerId.BOpt == 4)
			{
				Projectile::spawnProjectile("BolterKraken",%trans,%player,%vel);
Projectile::spawnProjectile("BolterTracer",%trans,%player,%vel);

			 Player::decItemCount(%player,$WeaponAmmo[Bolter],1);
			}
		}
}

ItemData Bolter 
{
  description = "Boltgun";
  className = "Weapon";
  shapeFile = "bolter";
  hudIcon = "shotgun";
  heading = $InvHead[ihWma];
  shadowDetailMask = 4;
  imageType = BolterImage;
  price = 7;
  showWeaponBar = true;
};

function Bolter::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Boltgun(Bolter)<f1>\nHeralded as the most versatile weapon in the galaxy. It fires various types of mini missiles and is the mainstay of the Marine armament.");
}


//--=-=-==-=-=-=-=-=-=-NEW BOLTER AMMO VARIANTS

//Bolter "Kraken" Rounds(armor piercing)

RocketData BolterKraken 
{
  bulletShapeName = "shotgunbolt.dts";
  explosionTag = debrisExpSmall;
  expRandCycle = 1;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.12;
  damageType = $KrakenDamageType;
  explosionRadius = 3.5;
  kickBackStrength = 0.0;
  muzzleVelocity = 165.0;
  terminalVelocity = 2000.0;
  aimDeflection = 0.003;
  acceleration = 100.0;
  totalTime = 0.9;
  liveTime = 0.9;
  lightRange = 5.0;
  lightColor = { 0.0, 0.0, 0.0 };
  inheritedVelocityScale = 0.5;
  trailType = 2;
  trailString = "rsmoke.dts";
  smokeDist = 1.0;
  soundId = SoundJetHeavy;
};

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Storm Bolter 
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$AutoUse[StBolter] = False;
$WeaponAmmo[StBolter] = StBolterAmmo;
$InvList[StBolter] = 1;
$RemoteInvList[StBolter] = 1;

addWeapon(StBolter);

RocketData StBolterBullet 
{
  bulletShapeName = "shotgunbolt.dts";
  explosionTag = debrisExpSmall;
  expRandCycle = 1;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.3;
  damageType = $MissileDamageType;
  explosionRadius = 3.5;
  kickBackStrength = 0.0;
  muzzleVelocity = 165.0;
  terminalVelocity = 2000.0;
  aimDeflection = 0.003;
  acceleration = 100.0;
  totalTime = 0.9;
  liveTime = 0.9;
  lightRange = 5.0;
  lightColor = { 0.0, 0.0, 0.0 };
  inheritedVelocityScale = 0.5;
  trailType = 2;
  trailString = "rsmoke.dts";
  smokeDist = 0.0;
  soundId = SoundJetHeavy;
};

ItemImageData StBolterImage 
{
  shapeFile = "sbolter";
  mountPoint = 0;
  weaponType = 0;
  reloadTime = 0.08;
  fireTime = 0.08;
  ammoType = StBolterAmmo;
  //projectileType = "Undefined";
  accuFire = false;
  lightType = 3;
  lightRadius = 3;
  lightTime = 1;
  lightColor = { 0.6, 1, 1 };  
  sfxFire = SoundBolterFire;
  sfxActivate = SoundPickUpWeapon;
};

function StBolterImage::onFire(%player, %slot) 
{
	 	 %Ammo = Player::getItemCount(%player, $WeaponAmmo[StBolter]);
		
		 %playerId = Player::getClient(%player);
		 if(%Ammo) 
		 {
			 %client = GameBase::getOwnerClient(%player);
			 Player::decItemCount(%player,$WeaponAmmo[StBolter],1);
			 %trans = GameBase::getMuzzleTransform(%player);
		     %vel = Item::getVelocity(%player);


			if (%playerId.SBOpt == 0)
			{
				Projectile::spawnProjectile("StBolterBullet",%trans,%player,(%vel +5));			Projectile::spawnProjectile("StBolterBullet",%trans,%player,(%vel -5));
Projectile::spawnProjectile("BolterTracer",%trans,%player,%vel);
Projectile::spawnProjectile("BolterTracer",%trans,%player,%vel);
			}
			else if (%playerId.SBOpt == 1)
			{
				Projectile::spawnProjectile("StBolterInferno",%trans,%player,%vel);
				Projectile::spawnProjectile("StBolterInferno",%trans,%player,%vel);
Projectile::spawnProjectile("BolterTracer",%trans,%player,%vel);
Projectile::spawnProjectile("BolterTracer",%trans,%player,%vel);
			 Player::decItemCount(%player,$WeaponAmmo[StBolter],1);
			}
			else if (%playerId.SBOpt == 2)
			{
				Projectile::spawnProjectile("StBolterSlug",%trans,%player,%vel);
				Projectile::spawnProjectile("StBolterSlug",%trans,%player,%vel);
				
			 Player::decItemCount(%player,$WeaponAmmo[StBolter],1);
			}
			else if (%playerId.SBOpt == 3)
			{
				Projectile::spawnProjectile("StBolterHellfire",%trans,%player,%vel);
				Projectile::spawnProjectile("StBolterHellfire",%trans,%player,%vel);
			 Player::decItemCount(%player,$WeaponAmmo[StBolter],1);
			}
			else if (%playerId.SBOpt == 4)
			{
				Projectile::spawnProjectile("StBolterKraken",%trans,%player,(%vel +5));
Projectile::spawnProjectile("BolterTracer",%trans,%player,(%vel -5));
				Projectile::spawnProjectile("StBolterKraken",%trans,%player,%vel);
Projectile::spawnProjectile("BolterTracer",%trans,%player,%vel);
				
			 Player::decItemCount(%player,$WeaponAmmo[StBolter],1);
			}
		}
}

ItemData StBolter 
{
  description = "Storm Bolter";
  className = "Weapon";
  shapeFile = "sbolter";
  hudIcon = "shotgun";
  heading = $InvHead[ihWma];
  shadowDetailMask = 4;
  imageType = StBolterImage;
  price = 14;
  showWeaponBar = true;
};

function StBolter::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Storm Bolter<f1>\nA Terminators Boltgun. Essentially two boltguns in one solid unit.");
}

//-=-=-=-=-STORM BOLTER AMMO VARIANTS

//Bolter "Kraken" Rounds(armor piercing)

RocketData StBolterKraken 
{
 bulletShapeName = "shotgunbolt.dts";
  explosionTag = debrisExpSmall;
  expRandCycle = 1;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.12;
  damageType = $KrakenDamageType;
  explosionRadius = 3.5;
  kickBackStrength = 0.0;
  muzzleVelocity = 165.0;
  terminalVelocity = 2000.0;
  aimDeflection = 0.003;
  acceleration = 100.0;
  totalTime = 0.9;
  liveTime = 0.9;
  lightRange = 5.0;
  lightColor = { 0.0, 0.0, 0.0 };
  inheritedVelocityScale = 0.5;
  trailType = 2;
  trailString = "rsmoke.dts";
  smokeDist = 1.0;
  soundId = SoundJetHeavy;
};

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Heavy Bolter 
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[HvyBolter] = 1;
$RemoteInvList[HvyBolter] = 1;
$AutoUse[HvyBolter] = False;
$WeaponAmmo[HvyBolter] = HvyBolterAmmo;

addWeapon(HvyBolter);

$Needs[HvyBolter] = FeedPack;

RocketData HvyBolterRound 
{
  bulletShapeName = "shotgunbolt.dts";
  explosionTag = debrisExpSmall;
  expRandCycle = 1;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.43;
  damageType = $MissileDamageType;
  explosionRadius = 3.5;
  kickBackStrength = 0.0;
  muzzleVelocity = 165.0;
  terminalVelocity = 2000.0;
  aimDeflection = 0.003;
  acceleration = 100.0;
  totalTime = 3.5;
  liveTime = 3.5;
  lightRange = 5.0;
  lightColor = { 0.0, 0.0, 0.0 };
  inheritedVelocityScale = 0.5;
  trailType = 2;
  trailString = "rsmoke.dts";
  smokeDist = 0.0;
  soundId = SoundJetHeavy;
};

ItemImageData HvyBolterImage 
{
  shapeFile = "hbolter";
  mountPoint = 0;
  weaponType = 0;
  ammoType = HvyBolterAmmo;
  //projectileType = "Undefined";
  accuFire = false;
  reloadTime = 0.08;
  fireTime = 0.08;
  lightType = 3;
  lightRadius = 6;
  lightTime = 2;
  lightColor = { 1.0, 2.0, 5.0 };
  sfxFire = SoundHeavyBolterFire;
  sfxActivate = SoundPickUpWeapon;
};

function HvyBolterImage::onFire(%player, %slot) 
{
	 	 %Ammo = Player::getItemCount(%player, $WeaponAmmo[HvyBolter]);
		
		 %playerId = Player::getClient(%player);
		 if(%Ammo) 
		 {
			 %client = GameBase::getOwnerClient(%player);
			 Player::decItemCount(%player,$WeaponAmmo[HvyBolter],1);
			 %trans = GameBase::getMuzzleTransform(%player);
		     %vel = Item::getVelocity(%player);


			if (%playerId.HBOpt == 0)
			{
				Projectile::spawnProjectile("HvyBolterRound",%trans,%player,%vel);
Projectile::spawnProjectile("BolterTracer",%trans,%player,%vel);
				
			}
			else if (%playerId.HBOpt == 1)
			{
				Projectile::spawnProjectile("HvBolterInferno",%trans,%player,%vel);
Projectile::spawnProjectile("BolterTracer",%trans,%player,%vel);
				
Player::decItemCount(%player,$WeaponAmmo[HvyBolter],1);
			}
			else if (%playerId.HBOpt == 2)
			{
				Projectile::spawnProjectile("HvBolterSlug",%trans,%player,%vel);
Projectile::spawnProjectile("BolterTracer",%trans,%player,%vel);
				
Player::decItemCount(%player,$WeaponAmmo[HvyBolter],1);
				
			}
			else if (%playerId.HBOpt == 3)
			{
				
Projectile::spawnProjectile("HvBolterHellfire",%trans,%player,%vel);
Projectile::spawnProjectile("BolterTracer",%trans,%player,%vel);
Player::decItemCount(%player,$WeaponAmmo[HvyBolter],1);
			}
			else if (%playerId.HBOpt == 4)
			{
				Projectile::spawnProjectile("HvBolterKraken",%trans,%player,%vel);
Projectile::spawnProjectile("BolterTracer",%trans,%player,%vel);
				
Player::decItemCount(%player,$WeaponAmmo[HvyBolter],1);
				
			}
		}
}

ItemData HvyBolter 
{
  description = "Heavy Bolter";
  className = "Weapon";
  shapeFile = "hbolter";
  hudIcon = "targetlaser";
  heading = $InvHead[ihWma];
  shadowDetailMask = 4;
  imageType = HvyBolterImage;
  price = 23;
  showWeaponBar = true;
};

function HvyBolter::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Heavy Bolter<f1>\nThe largest type of Boltgun. It has the same versatility, and nearly 3 times the power. Requires a Belt Feeder.");
}

function HvyBolter::onUse(%player,%item)
{
	if(Player::getMountedItem(%player,$BackpackSlot) == FeedPack)
		Weapon::onUse(%player,%item);
	else
		Client::sendMessage(Player::getClient(%player),0,
			"You need the ammo Belt Feeder to use the Heavy Bolter."); }

//-=-=-=---=-=-=-=-=-=-=-=-=NeW HVY BOLTER AMMO VARIANTS

//HV Bolter "Inferno" Round(incendiary)

BulletData HvBolterInferno 
{
  bulletShapeName = "shotgunbolt.dts";
  explosionTag = PlasmaExp;
  expRandCycle = 0;
  mass = 0.05;
  bulletHoleIndex = 0;
  damageClass = 0;
  damageValue = 0.2;
  damageType = $PlasmaDamageType;
  aimDeflection = 0.003;
  muzzleVelocity = 425.0;
  totalTime = 1.5;
  inheritedVelocityScale = 1.0;
  isVisible = true;
  tracerPercentage = 1.0;
  tracerLength = 30;
};

//HVBolter Slug Round(bullet)

BulletData HvBolterSlug 
{
   bulletShapeName    = "paint.dts";
   explosionTag       = rocketExp;
   expRandCycle = 1;
   damageClass        = 1;
   damageValue        = 0.24;
   damageType         = $BlasterDamageType;
   explosionRadius    = 2.0;
   muzzleVelocity     = 200.0;
   totalTime          = 1.0;
   liveTime           = 1.0;

   lightRange         = 3.0;
   lightColor         = { 1.0, 0.25, 0.25 };
   inheritedVelocityScale = 0.5;
   isVisible          = True;

   rotationPeriod = 1;
};
   

//Bolter "Hellfire" Rounds(acid)

BulletData HvBolterHellfire 
{
  bulletShapeName = "bullet.dts";
  explosionTag = flashExpSmall;
  expRandCycle = 1;
  mass = 0.05;
  bulletHoleIndex = 0;
  damageClass = 0;
  damageValue = 0.24;
  damageType = $AcidDamageType;
  aimDeflection = 0.003;
  muzzleVelocity = 425.0;
  totalTime = 1.5;
  inheritedVelocityScale = 1.0;
  isVisible = False;
  tracerPercentage = 1.0;
  tracerLength = 30;
};

//Bolter "Kraken" Rounds(armor piercing)

RocketData HvBolterKraken 
{
  bulletShapeName = "shotgunbolt.dts";
  explosionTag = debrisExpSmall;
  expRandCycle = 1;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.26;
  damageType = $KrakenDamageType;
  explosionRadius = 3.5;
  kickBackStrength = 0.0;
  muzzleVelocity = 165.0;
  terminalVelocity = 2000.0;
  aimDeflection = 0.003;
  acceleration = 100.0;
  totalTime = 2.0;
  liveTime = 2.0;
  lightRange = 5.0;
  lightColor = { 0.0, 0.0, 0.0 };
  inheritedVelocityScale = 0.5;
  trailType = 2;
  trailString = "rsmoke.dts";
  smokeDist = 1.0;
  soundId = SoundJetHeavy;
};
