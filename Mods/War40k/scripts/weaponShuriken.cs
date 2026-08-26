
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Shuriken Pistol
//  By <[DC]>Paladin
//
//
//  For installation information, see Install.txt
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$InvList[ShurPist] = 1;
$RemoteInvList[ShurPist] = 1;
$WeaponAmmo[ShurPist] = BoltPistAmmo;

addWeapon(ShurPist);

BulletData ShurPistBolt
{
   bulletShapeName    = "discb.dts";
   explosionTag       = bulletExp0;
   mass = 0.05;
   damageClass        = 0;
   damageValue        = 0.12;
   damageType         = $ShurikenDamageType;
   aimDeflection =0.004;
   muzzleVelocity     = 300.0;
   inheritedVelocityScale = 1.0;
   totalTime          = 4.0;
   liveTime           = 4.0;

   lightRange         = 3.0;
   lightColor         = { 1.0, 0.25, 0.25 };
   inheritedVelocityScale = 0.5;
   isVisible          = True;

   rotationPeriod = 1;
};

ItemImageData ShurPistImage 
{
  shapeFile = "shurpistol";
  mountPoint = 0;
  weaponType = 3;
  ammoType = BoltPistAmmo;
  //projectileType = ShurPistBolt;
  accuFire = true;
  reloadTime = 0.008;
  fireTime = 0.008;
  spinuptime = 0.15;
  lightType = 3;
  lightRadius = 2;
  lightTime = 1;
  lightColor = { 4, 6, 2 };
  sfxFire = SoundFireShuriken;
  sfxActivate = SoundPickUpWeapon;
};

function ShurPistImage::onFire(%player, %slot) 
{
 %AmmoCount = Player::getItemCount(%player, $WeaponAmmo[ShurPist]);
	 if(%AmmoCount > 0) 
	 {
		 %client = GameBase::getOwnerClient(%player);
                 %clientName = Player::getClient(%player);
                 %clientId = Client::getName(%client);
		 Player::decItemCount(%player,$WeaponAmmo[ShurPist],1);
		 %trans = GameBase::getMuzzleTransform(%player);
	     %vel = Item::getVelocity(%player);
	
	
			Projectile::spawnProjectile("ShurPistBolt",%trans,%player,%vel);
			Projectile::spawnProjectile("ShurPistBolt",%trans,%player,%vel);
			
	}
	else
		Client::sendMessage(Player::getClient(%player), 0,"Out Of Shuriken");

}

ItemData ShurPist 
{
  description = "Shuriken Pistol";
  className = "Weapon";
  shapeFile = "shurpistol";
  hudIcon = "sniper";
  heading = $InvHead[ihWel];
  shadowDetailMask = 4;
  imageType = ShurPistImage;
  price = 2;
  showWeaponBar = true;
};

function ShurPist::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Shuriken Pistol<f1>\nA pistol version of the Shuriken Catapult.");
}


//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Shuriken Catapult 
//  By <[DC]>Paladin
//
//   
//    see Contrib.txt
//
//  For installation information, see Install.txt
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$InvList[ShurCata] = 1;
$RemoteInvList[ShurCata] = 1;
$WeaponAmmo[ShurCata] = BolterAmmo;

addWeapon(ShurCata);

BulletData Shuriken
{
   bulletShapeName    = "discb.dts";
   explosionTag       = bulletExp0;
   mass = 0.05;
   damageClass        = 0;
   damageValue        = 0.134;
   damageType         = $ShurikenDamageType;
   aimDeflection =0.002;
   muzzleVelocity     = 300.0;
   inheritedVelocityScale = 1.0;
   totalTime          = 4.0;
   liveTime           = 4.0;

   lightRange         = 3.0;
   lightColor         = { 1.0, 0.25, 0.25 };
   inheritedVelocityScale = 0.5;
   isVisible          = True;

   rotationPeriod = 1;
};

ItemImageData ShurCataImage 
{
  shapeFile = "shur";
  mountPoint = 0;
  weaponType = 3;
  ammoType = BolterAmmo;
  //projectileType = Shuriken;
  accuFire = true;
  reloadTime = 0.008;
  fireTime = 0.008;
  spinUpTime = 0.15;
  sfxFire = SoundFireShuriken;
  sfxActivate = SoundPickUpWeapon;
  sfxReload = SoundDiscReload;
  sfxReady = SoundDiscSpin;
};

function ShurCataImage::onFire(%player, %slot) 
{
 %AmmoCount = Player::getItemCount(%player, $WeaponAmmo[ShurCata]);
	 if(%AmmoCount > 0) 
	 {
		 %client = GameBase::getOwnerClient(%player);
                 %clientName = Player::getClient(%player);
                 %clientId = Client::getName(%client);
		 Player::decItemCount(%player,$WeaponAmmo[ShurCata],1);
		 %trans = GameBase::getMuzzleTransform(%player);
	     %vel = Item::getVelocity(%player);
	
	
			Projectile::spawnProjectile("Shuriken",%trans,%player,(%vel +1));
			Projectile::spawnProjectile("Shuriken",%trans,%player,(%vel -1));
			
	}
	else
		Client::sendMessage(Player::getClient(%player), 0,"Out Of Shuriken");

}

ItemData ShurCata 
{
  description = "Shuriken Catapult";
  className = "Weapon";
  shapeFile = "shur";
  hudIcon = "disk";
  heading = $InvHead[ihWel];
  shadowDetailMask = 4;
  imageType = ShurCataImage;
  price = 7;
  showWeaponBar = true;
};


function shurCata::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Shuriken Catapult<f1>\nUtilizing superior technology, this weapon fires rapid bursts of shuriken that rip through armor.");
}


//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Shuriken Cannon
//  By <[DC]>Paladin
//
//  
//    see Contrib.txt
//
//  For installation information, see Install.txt
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$InvList[ShurCannon] = 1;
$RemoteInvList[ShurCannon] = 1;
$WeaponAmmo[ShurCannon] = HvyBolterAmmo;
$AutoUse[ShurCannon] = False;

addWeapon(ShurCannon);

BulletData HShuriken
{
   bulletShapeName    = "discb.dts";
   explosionTag       = bulletExp0;
   mass = 0.05;
   damageClass        = 0;
   damageValue        = 0.2;
   damageType         = $ShurikenDamageType;
   aimDeflection = 0.003;
   muzzleVelocity     = 300.0;
   inheritedVelocityScale = 1.0;
   totalTime          = 4.0;
   liveTime           = 4.0;

   lightRange         = 3.0;
   lightColor         = { 1.0, 0.25, 0.25 };
   inheritedVelocityScale = 0.5;
   isVisible          = True;

   rotationPeriod = 1;
};

ItemImageData ShurCannonImage 
{
  shapeFile = "GrenadeL";
  mountPoint = 0;
  weaponType = 3;
  ammoType = HvyBolterAmmo;
  //projectileType = HShuriken;
  accuFire = true;
  reloadTime = 0.03;
  fireTime = 0.03;
  spinUpTime = 0.15;
  sfxFire = SoundFireShuriken;
  sfxActivate = SoundPickUpWeapon;
  sfxReload = SoundDiscReload;
  sfxReady = SoundDiscSpin;
};

function ShurCannonImage::onFire(%player, %slot) 
{
 %AmmoCount = Player::getItemCount(%player, $WeaponAmmo[ShurCannon]);
	 if(%AmmoCount > 0) 
	 {
		 %client = GameBase::getOwnerClient(%player);
                 %clientName = Player::getClient(%player);
                 %clientId = Client::getName(%client);
		 Player::decItemCount(%player,$WeaponAmmo[ShurCannon],1);
		 %trans = GameBase::getMuzzleTransform(%player);
	     %vel = Item::getVelocity(%player);
	
	
			Projectile::spawnProjectile("HShuriken",%trans,%player,%vel);
			Projectile::spawnProjectile("HShuriken",%trans,%player,%vel);
			
	}
	else
		Client::sendMessage(Player::getClient(%player), 0,"Out Of Shuriken");

}

ItemData ShurCannon 
{
  description = "Shuriken Cannon";
  className = "Weapon";
  shapeFile = "GrenadeL";
  hudIcon = "disk";
  heading = $InvHead[ihWel];
  shadowDetailMask = 4;
  imageType = ShurCannonImage;
  price = 18;
  showWeaponBar = true;
};

function ShurCannon::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Shuriken Cannon<f1>\nThe largest variety of Shuriken weaponry available to the Eldar. Very deadly.");
}

