//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Cyclone Missile Launcher
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Cyclone] = 1;
$RemoteInvList[Cyclone] = 1;
$AutoUse[Cyclone] = False;
$WeaponAmmo[Cyclone] = CycloneAmmo;

addWeapon(Cyclone);

 RocketData CycloneMissileR
{
   bulletShapeName = "rocket.dts";
   explosionTag    = debrisExpSmall;
   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;
   damageValue      = 0.365;
   damageType       = $MissileDamageType;

   explosionRadius  = 9.5;
   kickBackStrength = 50.0;

   muzzleVelocity   = 270.0;
   terminalVelocity = 1000.0;
   acceleration     = 0.0;

   totalTime        = 10;
   liveTime         = 10;

   lightRange       = 5.0;
   lightColor       = { 0, 0, 1 };

   inheritedVelocityScale = 1;

   soundId = SoundJetHeavy;
};

ItemImageData CycloneImage
{
	shapeFile = "mortargun";
	mountPoint = 0;
	weaponType = 0;
	mountRotation = { 0, 3.14, 0};
	ammoType = CycloneAmmo;
	projectileType = CycloneMissileR;
	accuFire = True;
	reloadTime = 0.6;
	fireTime = 0.0;

	lightType = 2;
	lightRadius = 1;
	lightTime = 1;
	lightColor = { 1, 0, 0 };

	sfxFire = SoundFireCyclone;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundMortarReload;
};

ItemData Cyclone
{
	description = "Cyclone";
	className = "Weapon";
	shapeFile = "mortargun";
	hudIcon = "grenade";
	heading = $InvHead[ihWma];
	shadowDetailMask = 4;
	imageType = CycloneImage;
	price = 20;
	showWeaponBar = true;
};

function Cyclone::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Cyclone<f1>\nA useful beast of a weapon. It's rockets fire rapidy, and are an effictive anti-infantry weapon.");
}

//-=-=-=-=-=-=-=-=-=--=-=-=
// ELDAR Missile Launcher
//-=-=-=-=-=-=-=-=-=-=-=-=-

$InvList[ERocketLauncher] = 1;
$RemoteInvList[ERocketLauncher] = 1;
$AutoUse[ERocketLauncher] = False;
$WeaponAmmo[ERocketLauncher] = RocketAmmo;

addWeapon(ERocketLauncher);

RocketData PlasmaMissile 
{
  bulletShapeName = "rocket.dts";
  explosionTag = plasmaExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.55;
  damageType = $PlasmaDamageType;
  explosionRadius = 20.5;
  kickBackStrength = 220.0;
  muzzleVelocity = 165.0;
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

ItemImageData ERocketLauncherImage 
{
  shapeFile = "mortargun";
  mountPoint = 0;
  weaponType = 0;
  ammoType = RocketAmmo;
//  projectileType = "Undefined";
  accuFire = true;
  reloadTime = 1.0;
  fireTime = 1.0;
  lightType = 3;
  lightRadius = 3;
  lightTime = 1;
  lightColor = { 0.6, 1, 1.0 };
  sfxFire = SoundMissileTurretFire;
  sfxActivate = SoundPickUpWeapon;
  sfxReload = SoundMortarReload;
  sfxReady = SoundMortarIdle;
};

function ERocketLauncherImage::onFire(%player, %slot) 
{
	 	 %Ammo = Player::getItemCount(%player, $WeaponAmmo[ERocketLauncher]);
		
		 %playerId = Player::getClient(%player);
		 if(%Ammo) 
		 {
			 %client = GameBase::getOwnerClient(%player);
			 Player::decItemCount(%player,$WeaponAmmo[ERocketLauncher],1);
			 %trans = GameBase::getMuzzleTransform(%player);
		     %vel = Item::getVelocity(%player);


			if (%playerId.ERLOpt == 0)
			{
				Projectile::spawnProjectile("PlasmaMissile",%trans,%player,%vel);
			}
			else if (%playerId.ERLOpt == 1)
			{
				Projectile::spawnProjectile("PlagueMissile",%trans,%player,%vel);
			}
			else if (%playerId.ERLOpt == 2)
			{
				Projectile::spawnProjectile("KrakMissile",%trans,%player,%vel);
			}
		}
}

ItemData ERocketLauncher 
{
  description = "Eldar Mis. Launcher";
  className = "Weapon";
  shapeFile = "mortargun";
  hudIcon = "mortar";
  heading = $InvHead[ihWel];
  shadowDetailMask = 4;
  imageType = ERocketLauncherImage;
  price = 20;
  showWeaponBar = true;
};

function ERocketLauncher::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Eldar Missile Launcher<f1>\nFires multiple varieties of missiles. Deadly and versatile.");
}

//-=-=-=-=-=-NEW ELDAR MISSILE AMMO VARIANTS
//Eldar Plague Missile

RocketData PlagueMissile 
{
  bulletShapeName = "rocket.dts";
  explosionTag = mortarExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.25;
  damageType = $EnergyDamageType;
  explosionRadius = 15.5;
  kickBackStrength = 220.0;
  muzzleVelocity = 165.0;
  terminalVelocity = 1000.0;
  acceleration = 200.0;
  totalTime = 6.5;
  liveTime = 10.0;
  lightRange = 2.0;
  lightColor = { 1.0, 0.7, 0.5 };
  inheritedVelocityScale = 0.5;
  trailType = 2;
  trailString = "mortartrail.dts";
  smokeDist = 0.5;
  soundId = SoundJetHeavy;
};

RocketData KrakMissile 
{
  bulletShapeName = "rocket.dts";
  explosionTag = flashExpLarge;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 1.8;
  damageType = $MissileDamageType; 
  explosionRadius = 4.5; 
  kickBackStrength = 150.0;
  muzzleVelocity = 165.0;
  terminalVelocity = 1000.0;
  acceleration = 200.0;
  totalTime = 6.5;
  liveTime = 10.0;
  lightRange = 2.0;
  lightColor = { 1.0, 0.7, 0.5 };
  inheritedVelocityScale = 0.5;
  trailType = 2;
  trailString = "smoke.dts";
  smokeDist = 0.5;
  soundId = SoundJetHeavy;
};

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Rocket Launcher
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[RocketLauncher] = 1;
$RemoteInvList[RocketLauncher] = 1;
$AutoUse[RocketLauncher] = False;
$WeaponAmmo[RocketLauncher] = RocketAmmo;

addWeapon(RocketLauncher);

SeekingMissileData StingerMissile
{
   bulletShapeName = "rocket.dts";
   explosionTag    = rocketExp;
   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.839;
   damageType       = $MissileDamageType;
   explosionRadius  = 30.5;
   kickBackStrength = 200.0;

   muzzleVelocity    = 185.0;
   totalTime         = 10;
   liveTime          = 10;
   seekingTurningRadius    = 9;
   nonSeekingTurningRadius = 75.0;
   proximityDist     = 1.5;
   smokeDist         = 1.75;

   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };

   inheritedVelocityScale = 0.5;

   soundId = SoundJetHeavy;
};

RocketData StingerMissileW
{
  bulletShapeName = "rocket.dts";
  explosionTag = rocketExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.839;
  damageType = $MissileDamageType;
  explosionRadius = 30.5;
  kickBackStrength = 200.0;
  muzzleVelocity = 185.0;
  terminalVelocity = 1000.0;
  acceleration = 5.0;
  totalTime = 10.0;
  liveTime = 10.0;
  lightRange = 1.0;
  lightColor = { 0.3, 0.7, 0.5 };
  inheritedVelocityScale = 0.5;
  trailType = 2;
  trailString = "smoke.dts";
  smokeDist = 0.6;
  soundId = SoundJetHeavy;
};

ItemImageData RocketImage
{
	shapeFile = "mortargun";
	mountPoint = 0;
	weaponType = 0; // Single Shot
	ammoType = RocketAmmo;
//	projectileType = StingerMissile;
	accuFire = true;
	reloadTime = 3.0;
	fireTime = 0.0;

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = SoundFireSeeking;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundMortarReload;
};

ItemData RocketLauncher
{
	description = "Missile Launcher";
	className = "Weapon";
	shapeFile = "mortargun";
	hudIcon = "grenade";
   heading = $InvHead[ihWma];
	shadowDetailMask = 4;
	imageType = RocketImage;
	price = 20;
	showWeaponBar = true;
};

function RocketLauncher::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Missile Launcher<f1>\nAble to lock onto targets and seek them out, this destructive tool is seldom innefective.");
}

function RocketImage::onFire(%player, %slot)
{
	%AmmoCount = Player::getItemCount(%player, $WeaponAmmo[RocketLauncher]);
	if(%AmmoCount > 0)
	{
		%client = GameBase::getOwnerClient(%player);
		%clientName = Player::getClient(%player);
		%clientId = Client::getName(%client);
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);
		if(GameBase::getLOSInfo(%player,3000))
		{
			%object = getObjectType($los::object);
			%targeted = GameBase::getOwnerClient($los::object);
				if(%object == "Player" || %object == "Flier")
				{
					%targetP = Client::getName(%targeted);
					Client::sendMessage(%client,0,"HUD-CPU: Missile lock acquired "@ %targetP @"~wpda_on.wav");
					Client::sendMessage(%targeted,0,"HUD-CPU: Missile lock detected - " @ %clientId @ "~waccess_denied.wav");
					Projectile::spawnProjectile("StingerMissile",%trans,%player,%vel,$los::object);
									Player::decItemCount(%player,$WeaponAmmo[RocketLauncher],1);
				}
		else
			{
			Projectile::spawnProjectile("StingerMissileW",%trans,%player,%vel,%player);
			
			Player::decItemCount(%player,$WeaponAmmo[RocketLauncher],1);
			}
		}
	else
		{
		Projectile::spawnProjectile("StingerMissileW",%trans,%player,%vel,%player);
		
	Player::decItemCount(%player,$WeaponAmmo[RocketLauncher],1);
		}
	}
	else Client::sendMessage(Player::getClient(%player),0,"Missile Launcher out of ammo.");
}


//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Reaper Cannon
//  by <[DC]>Paladin :modded from Mass Driver(al_renegades)
//
//  For installation information, see Install.txt
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$InvList[Reaper] = 1;
$RemoteInvList[Reaper] = 1;
$AutoUse[Reaper] = False;
$WeaponAmmo[Reaper] = CycloneAmmo;

addWeapon(Reaper);

RocketData DarkRocket
{
  bulletShapeName = "paint.dts";
  explosionTag = rocketExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.42;
  damageType = $ReaperDamageType;
  explosionRadius = 8.5;
  kickBackStrength = 5.0;
  muzzleVelocity = 385.0;
  terminalVelocity = 2000.0;
  acceleration = 5.0;
  totalTime = 10.0;
  liveTime = 10.0;
  lightRange = 1.0;
  lightColor = { 1.0, 1.0, 1.0 };
  inheritedVelocityScale = 0.0;
  trailType = 2;
  trailString = "breath.dts";
  smokeDist = 0.0;
  soundId = SoundBeaconUSe;
};

ItemImageData ReaperImage 
{
  shapeFile = "brightlance";
  mountPoint = 0;
  weaponType = 0;
  ammoType = CycloneAmmo;
  projectileType = DarkRocket;
  accuFire = true;
  reloadTime = 0.3;
  fireTime = 0.0;
  lightType = 3;
  lightRadius = 6;
  lightTime = 2;
  lightColor = { 0, 0, 3.0 };
  sfxFire = SoundFireMortar;
  sfxActivate = SoundPickUpWeapon;
  sfxReady = SoundMortarIdle;
};

ItemData Reaper
{
  description = "Reaper Launcher";
  className = "Weapon";
  shapeFile = "brightlance";
  hudIcon = "targetlaser";
  heading = $InvHead[ihWel];
  shadowDetailMask = 4;
  imageType = ReaperImage;
  price = 14;
  showWeaponBar = true;
};

function Reaper::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Reaper Launcher<f1>\nThe signature weapon of the Dark Reaper, this beautifully crafted rocket weapon has a high fire rate and flawless killing ability.");
}