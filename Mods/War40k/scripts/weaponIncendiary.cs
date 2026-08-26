//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Fire Pike 
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Firepike] = 1;
$RemoteInvList[Firepike] = 1;
$AutoUse[Firepike] = False;
$WeaponAmmo[Firepike] = "";

addWeapon(Firepike);

$Needs[Firepike] = FlamePack;

BulletData FirepikeBolt
{
  bulletShapeName = "plasmabolt.dts";
  explosionTag = plasmaExp;
  damageClass = 1;
  damageValue = 0.18;
  damageType = $MeltaDamageType;
  explosionRadius = 6.0;
  muzzleVelocity = 30.0;
  totalTime = 1.25;
  liveTime = 1.25;
  lightRange = 3.0;
  lightColor = { 1, 1, 0 };
  inheritedVelocityScale = 0.3;
  isVisible = True;
};

ItemImageData FirepikeImage 
{
  shapeFile = "sniper";
  mountPoint = 0;
  weaponType = 0;
  reloadTime = 0.09;
  fireTime = 0.0;
  minEnergy = 15;
  maxEnergy = 20;
  projectileType = FirepikeBolt;
  accuFire = false;
  sfxFire = SoundFirePlasma;
  sfxActivate = SoundPickUpWeapon;
};

ItemData Firepike 
{
  heading = $InvHead[ihWel];
  description = "Firepike";
  className = "Weapon";
  shapeFile = "shotgun";
  hudIcon = "plasma";
  shadowDetailMask = 4;
  imageType = FirepikeImage;
  price = 30;
  showWeaponBar = true;
};

function Firepike::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Firepike<f1>\nAn advanced form of Melta weapon, the Firepike has greater range due to better energy filtering. Requires a Napalm Pack.");
}

function Firepike::onUse(%player,%item)
{
	if(Player::getMountedItem(%player,$BackpackSlot) == FlamePack)
		Weapon::onUse(%player,%item);
	else
		Client::sendMessage(Player::getClient(%player),0,
			"Must have a Napalm Pack to use a Firepike."); }

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Fusion Gun: Modded from Ion Gun
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[FusionGun] = 1;
$RemoteInvList[FusionGun] = 1;
$AutoUse[FusionGun] = False;
$WeaponAmmo[FusionGun] = "";

addWeapon(FusionGun);

RocketData FusionBoltx
{
  bulletShapeName = "plasmaex.dts";
  explosionTag = plasmaExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.45;
  damageType = $DeathDamageType;
  explosionRadius = 6;
  kickBackStrength = 0.0;
  muzzleVelocity = 200.0;
  terminalVelocity = 200.0;
  acceleration = 5.0;
  totalTime = 0.3;
  liveTime = 0.3;
  lightRange = 5.0;
  lightColor = { 1.0, 0.7, 0.5 };
  inheritedVelocityScale = 0.5;
  trailType = 1;
  trailLength = 30;
  trailWidth = 0.3;
};

ItemImageData FusionGunImage
{
  shapeFile = "GrenadeL";
  mountPoint = 0;
  weaponType = 0;
  reloadTime = 0.4;
  fireTime = 0.01;
  minEnergy = 6;
  maxEnergy = 12;
  projectileType = FusionBoltx;
  accuFire = true;
  sfxFire = SoundFusionFire;
  sfxActivate = SoundPickUpWeapon;
};

ItemData FusionGun 
{
  heading = $InvHead[ihWel];
  description = "Fusion Gun";
  className = "Weapon";
  shapeFile = "GrenadeL";
  hudIcon = "blaster";
  shadowDetailMask = 4;
  imageType = FusionGunImage;
  price = 9;
  showWeaponBar = true;
};

function FusionGun::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Fusion Gun<f1>\nA rapid-fire energy beam weapon, and the Firedragons main armament.");
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Hand Flamer
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[SmallFlamer] = 1;
$RemoteInvList[SmallFlamer] = 1;
$WeaponAmmo[SmallFlamer] = "";
$AutoUse[SmallFlamer] = False;

addWeapon(SmallFlamer);

GrenadeData SmallFlamerbolt 
{
  bulletShapeName = "plasmatrail.dts";
  explosionTag = plasmaExp;
  collideWithOwner = True;
  ownerGraceMS = 250;
  collisionRadius = 0.3;
  mass = 5.0;
  elasticity = 0.35;
  damageClass = 1;
  damageValue = 0.06;
  damageType = $PlasmaDamageType;
  explosionRadius = 4.0;
  kickBackStrength = 0.0;
  maxLevelFlightDist = 45;
  totalTime = 5.0;
  liveTime = 0.2;
  projSpecialTime = 0.01;
  inheritedVelocityScale = 0.5;
  smokeName = "plasmatrail.dts";
};
ItemImageData SmallFlamerImage 
{
  shapeFile = "flamer2";
  mountPoint = 0;
  weaponType = 0;
  reloadTime = 0.05;
  fireTime = 0.05;
  minEnergy = 2;
  maxEnergy = 5;
  projectileType = SmallFlamerBolt;
  accuFire = true;
  sfxFire = SoundFlameFire;
  sfxActivate = SoundPickUpWeapon;
};
ItemData SmallFlamer 
{
  heading = $InvHead[ihWea];
  description = "Hand Flamer";
  className = "Weapon";
  shapeFile = "flamer2";
  hudIcon = "plasma";
  shadowDetailMask = 4;
  imageType = SmallFlamerImage;
  price = 6;
  showWeaponBar = true;
};

function SmallFlamer::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Hand Flamer<f1>\nSpouts a blast of superheated napalm at the enemy, igniting them on impact.");
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Flamer
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Flamer] = 1;
$RemoteInvList[Flamer] = 1;
$WeaponAmmo[Flamer] = "";
$AutoUse[Flamer] = False;

addWeapon(Flamer);

GrenadeData Flamerbolt 
{
  bulletShapeName = "plasmatrail.dts";
  explosionTag = plasmaExp;
  collideWithOwner = True;
  ownerGraceMS = 300;
  collisionRadius = 0.3;
  mass = 5.0;
  elasticity = 0.35;
  damageClass = 1;
  damageValue = 0.06;
  damageType = $PlasmaDamageType;
  explosionRadius = 4.0;
  kickBackStrength = 0.0;
  maxLevelFlightDist = 75;
  totalTime = 5.0;
  liveTime = 0.2;
  projSpecialTime = 0.01;
  inheritedVelocityScale = 0.5;
  smokeName = "plasmatrail.dts";
};
ItemImageData FlamerImage 
{
  shapeFile = "flamer2";
  mountPoint = 0;
  weaponType = 0;
  reloadTime = 0.05;
  fireTime = 0.05;
  minEnergy = 5;
  maxEnergy = 6;
  projectileType = FlamerBolt;
  accuFire = true;
  sfxFire = SoundFlameFire;
  sfxActivate = SoundPickUpWeapon;
};
ItemData Flamer 
{
  heading = $InvHead[ihWea];
  description = "Flamer";
  className = "Weapon";
  shapeFile = "flamer2";
  hudIcon = "plasma";
  shadowDetailMask = 4;
  imageType = FlamerImage;
  price = 8;
  showWeaponBar = true;
};

function Flamer::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Flamer<f1>\nSpouts a blast of superheated napalm at the enemy, igniting them on impact.");
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Heavy Flamer
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[HFlamer] = 1;
$RemoteInvList[HFlamer] = 1;
$WeaponAmmo[HFlamer] = "";
$AutoUse[HFlamer] = False;

addWeapon(HFlamer);

$Needs[HFlamer] = FlamePack;

GrenadeData HFlamerBolt 
{
  bulletShapeName = "plasmatrail.dts";
  explosionTag = plasmaExp;
  collideWithOwner = True;
  ownerGraceMS = 335;
  collisionRadius = 0.3;
  mass = 5.0;
  elasticity = 0.35;
  damageClass = 1;
  damageValue = 0.06;
  damageType = $PlasmaDamageType;
  explosionRadius = 4.0;
  kickBackStrength = 0.0;
  maxLevelFlightDist = 105;
  totalTime = 5.0;
  liveTime = 0.2;
  projSpecialTime = 0.01;
  inheritedVelocityScale = 0.5;
  smokeName = "plasmatrail.dts";
};
ItemImageData HFlamerImage 
{
  shapeFile = "hflamer";
  mountPoint = 0;
  weaponType = 0;
  reloadTime = 0.05;
  fireTime = 0.05;
  minEnergy = 2;
  maxEnergy = 4;
  projectileType = HFlamerBolt;
  accuFire = true;
  sfxFire = SoundFlameFire;
  sfxActivate = SoundPickUpWeapon;
};
ItemData HFlamer 
{
  heading = $InvHead[ihWma];
  description = "Heavy Flamer";
  className = "Weapon";
  shapeFile = "hflamer";
  hudIcon = "plasma";
  shadowDetailMask = 4;
  imageType = HFlamerImage;
  price = 15;
  showWeaponBar = true;
};

function HFlamer::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Heavy Flamer<f1>\nA brutal flame weapon, igniting targets and melting flesh with horrific ease.Requires a Napalm Pack.");
}

function HFlamer::onUse(%player,%item)
{
	if(Player::getMountedItem(%player,$BackpackSlot) == FlamePack)
		Weapon::onUse(%player,%item);
	else
		Client::sendMessage(Player::getClient(%player),0,
			"Must have a Napalm Pack to use a Flamer."); }

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Meltagun
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Melt] = 1;
$RemoteInvList[Melt] = 1;
$AutoUse[Melt] = False;
$WeaponAmmo[Melt] = "";

addWeapon(Melt);

BulletData MeltBolt
{
  bulletShapeName = "plasmabolt.dts";
  explosionTag = plasmaExp;
  damageClass = 1;
  damageValue = 0.21;
  damageType = $MeltaDamageType;
  explosionRadius = 6.0;
  muzzleVelocity = 30.0;
  totalTime = 0.68;
  liveTime = 0.68;
  lightRange = 3.0;
  lightColor = { 1, 1, 0 };
  inheritedVelocityScale = 0.3;
  isVisible = True;
};

ItemImageData MeltImage 
{
  shapeFile = "meltagun";
  mountPoint = 0;
  weaponType = 0;
  reloadTime = 0.1;
  fireTime = 0.01;
  minEnergy = 10;
  maxEnergy = 12;
  projectileType = MeltBolt;
  accuFire = true;
  sfxFire = SoundFireMelta;
  sfxActivate = SoundPickUpWeapon;
};

ItemData Melt 
{
  heading = $InvHead[ihWma];
  description = "Meltagun";
  className = "Weapon";
  shapeFile = "meltagun";
  hudIcon = "plasma";
  shadowDetailMask = 4;
  imageType = MeltImage;
  price = 13;
  showWeaponBar = true;
};

function Melt::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Meltagun<f1>\nDesigned as a Tank Killer, this weapon melts anything it hits, but has a short range.");
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Multi-Melta
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Melta] = 1;
$RemoteInvList[Melta] = 1;
$AutoUse[Melta] = False;
$WeaponAmmo[Melta] = MeltaAmmo;

addWeapon(Melta);

ItemImageData MeltaImage 
{
  shapeFile = "meltagun";
  mountPoint = 0;
  weaponType = 0;
  reloadTime = 0.1;
  fireTime = 0.0;
  minEnergy = 20;
  maxEnergy = 25;
  ammoType = MeltaAmmo;
  //projectileType = "Undefined";
  accuFire = false;
  sfxFire = SoundFireMelta;
  sfxActivate = SoundPickUpWeapon;
};

BulletData MeltaBolt
{
  bulletShapeName = "plasmabolt.dts";
  explosionTag = plasmaExp;
  damageClass = 1;
  damageValue = 0.21;
  damageType = $MeltaDamageType;
  explosionRadius = 6.0;
  muzzleVelocity = 30.0;
  totalTime = 0.65;
  liveTime = 0.65;
  lightRange = 3.0;
  lightColor = { 1, 1, 0 };
  inheritedVelocityScale = 0.3;
  isVisible = True;
};


ItemData Melta 
{
  heading = $InvHead[ihWma];
  description = "Multi-Melta";
  className = "Weapon";
  shapeFile = "meltagun";
  hudIcon = "plasma";
  shadowDetailMask = 4;
  imageType = MeltaImage;
  price = 25;
  showWeaponBar = true;
};

function Melta::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Multi-Melta<f1>\nA modified Meltagun, which fires two burst instead of one. Built specifically for Terminators.");
}

function MeltaImage::onFire(%player, %slot) 
{
 %AmmoCount = Player::getItemCount(%player, $WeaponAmmo[Melta]);
	 if(%AmmoCount > 0) 
	 {
		 %client = GameBase::getOwnerClient(%player);
                 %clientName = Player::getClient(%player);
                 %clientId = Client::getName(%client);
		 Player::decItemCount(%player,$WeaponAmmo[Melta],1);
		 %trans = GameBase::getMuzzleTransform(%player);
	     %vel = Item::getVelocity(%player);
	
	
			Projectile::spawnProjectile("MeltaBolt",%trans,%player,(%vel + 4));
			Projectile::spawnProjectile("MeltaBolt",%trans,%player,(%vel - 4));
			
	}
	else
		Client::sendMessage(Player::getClient(%player), 0,"Out Of Charges");

}


//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Plasma Pistol
//  By <[DC]>Paladin
//
//   
//    
//
//  For installation information, see Install.txt
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$InvList[PlasPist] = 1;
$RemoteInvList[PlasPist] = 1;
$AutoUse[PlasPist] = False;
$Use[PlasPist] = True;
$WeaponAmmo[PlasPist] = "";

addWeapon(PlasPist);

RocketData PlasPistBolt
{
  bulletShapeName = "plasmabolt.dts";
  explosionTag = PlasCanExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.372;
  damageType = $FlamerDamageType;
  explosionRadius = 2.0;
  kickBackStrength = 0.0;
  muzzleVelocity = 55.0;
  terminalVelocity = 80.0;
  acceleration = 5.0;
  totalTime = 1.0;
  liveTime = 1.0;
  lightRange = 1.0;
  lightColor = { 0.5, 0.2, 0.2 };
  inheritedVelocityScale = 0.5;
  trailType = 2;
  trailString = "plasmatrail.dts";
  smokeDist = 0.0;
  soundId = SoundJetHeavy;
};


ItemImageData PlasPistImage
{
   shapeFile  = "plaspist";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0.3;
	fireTime = 0.0;
	minEnergy = 5;
	maxEnergy = 6;

	projectileType = PlasPistBolt;
	accuFire = true;

	sfxFire = SoundFirePlas;
	sfxActivate = SoundPickUpWeapon;
};

ItemData PlasPist
{
   heading = $InvHead[ihWea];
	description = "Plasma Pistol";
	className = "Weapon";
   shapeFile  = "plaspist";
	hudIcon = "blaster";
	shadowDetailMask = 4;
	imageType = PlasPistImage;
	price = 5;
	showWeaponBar = true;
};

function PlasPist::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Plasma Pistol<f1>\nThough limited in range, the Plasma Pistol has a high fire rate and is a very effective weapon.");
}
 

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Plasma Gun (PlasmaGun)
//  By Dynamix
//
//  Alliance version by Mjolnir, 
//    see Contrib.txt
//
//  For installation information, see Install.txt
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$InvList[PlasmaGun] = 1;
$RemoteInvList[PlasmaGun] = 1;
$AutoUse[PlasmaGun] = False;
$WeaponAmmo[PlasmaGun] = PlasmaAmmo;

addWeapon(PlasmaGun);

RocketData PlasmaBolt 
{
  bulletShapeName = "plasmabolt.dts";
  explosionTag = PlasCanExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.48;
  damageType = $FlamerDamageType;
  explosionRadius = 5.0;
  kickBackStrength = 0.0;
  muzzleVelocity = 55.0;
  terminalVelocity = 80.0;
  acceleration = 5.0;
  totalTime = 3.0;
  liveTime = 2.0;
  lightRange = 1.0;
  lightColor = { 0.5, 0.2, 0.2 };
  inheritedVelocityScale = 0.5;
  trailType = 2;
  trailString = "plasmatrail.dts";
  smokeDist = 1.0;
  soundId = SoundJetHeavy;
};

ItemImageData PlasmaGunImage
{
  shapeFile = "mplasma";
  mountPoint = 0;
  weaponType = 0;
  ammoType = PlasmaAmmo;
  projectileType = PlasmaBolt;
  accuFire = true;
  reloadTime = 0.3;
  fireTime = 0.3;
  lightType = 3;
  lightRadius = 3;
  lightTime = 1;
  lightColor = { 1, 1, 0.2 };
  sfxFire = SoundFirePlas;
  sfxActivate = SoundPickUpWeapon;
  sfxReload = SoundDryFire;
};

ItemData PlasmaGun
{
  description = "Plasma Gun";
  className = "Weapon";
  shapeFile = "mplasma";
  hudIcon = "plasma";
  heading = $InvHead[ihWea];
  shadowDetailMask = 4;
  imageType = PlasmaGunImage;
  price = 7;
  showWeaponBar = true;
};

function PlasmaGun::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Plasma Gun<f1>\nFiring pure heat energy, this weapon is effective against nearly any troop type.");
}
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Heavy Plasma Gun
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[PlasCan] = 1;
$RemoteInvList[PlasCan] = 1;
$AutoUse[PlasCan] = False;
$WeaponAmmo[PlasCan] = PlasCanAmmo;

addWeapon(PlasCan);

$Needs[PlasCan] = EnergyPack;

RocketData PlasCanBolt 
{
  bulletShapeName = "plasmabolt.dts";
  explosionTag = PlasCanExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 1.5;
  damageType = $FlamerDamageType;
  explosionRadius = 30.0;
  kickBackStrength = 0.0;
  muzzleVelocity = 50.0;
  terminalVelocity = 120.0;
  acceleration = 5.0;
  totalTime = 5.0;
  liveTime = 5.0;
  lightRange = 5.0;
  lightColor = { 1.0, 0.7, 0.5 };
  inheritedVelocityScale = 0.5;
  trailType = 2;
  trailString = "plasmatrail.dts";
  smokeDist = 2.0;
  soundId = SoundJetHeavy;
};

ItemImageData PlasCanImage 
{
  shapeFile = "plasmacannon";
  mountPoint = 0;
  weaponType = 0;
  ammoType = PlasCanAmmo;
  projectileType = PlasCanBolt;
  accuFire = true;
  reloadTime = 2.0;
  fireTime = 1.0;
  sfxFire = SoundPlasmaTurretFire;
  sfxActivate = SoundPickUpWeapon;
};

ItemData PlasCan 
{
  description = "Heavy Plasma Gun";
  className = "Weapon";
  shapeFile = "plasmacannon";
  hudIcon = "disk";
  heading = $InvHead[ihWea];
  shadowDetailMask = 4;
  imageType = PlasCanImage;
  price = 18;
  showWeaponBar = true;
};

function PlasCan::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Heavy Plasma gun<f1>\nFires extremely concentrated bursts of plasma, with a wide heat radius. Requires an Energy Pack.");
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Fusion Cannon
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Warp] = 1;
$RemoteInvList[Warp] = 1;
$AutoUse[Warp] = False;
$WeaponAmmo[Warp] = "";

addWeapon(Warp);

RocketData WarpShell2
{
  bulletShapeName = "plasmaex.dts";
  explosionTag = plasmaExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.55;
  damageType = $DeathDamageType;
  explosionRadius = 6;
  kickBackStrength = 0.0;
  muzzleVelocity = 200.0;
  terminalVelocity = 200.0;
  acceleration = 5.0;
  totalTime = 0.6;
  liveTime = 0.6;
  lightRange = 5.0;
  lightColor = { 1.0, 0.7, 0.5 };
  inheritedVelocityScale = 0.5;
  trailType = 1;
  trailLength = 30;
  trailWidth = 0.3;
};

//=====================================================================//=== Fusion Cannon

ItemImageData WarpImage
{
	shapeFile = "mortargun";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	projectileType = WarpShell2;
	accuFire = true;
	reloadTime = 0.4;
	fireTime = 0.01;

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = SoundFusionFire;
	sfxActivate = SoundPickUpWeapon;
};

ItemData Warp
{
	description = "Fusion Cannon";
	className = "Weapon";
	shapeFile = "mortargun";
	hudIcon = "ammopack";
    heading = $InvHead[ihWel];
	shadowDetailMask = 4;
	imageType = WarpImage;
	price = 25;
	showWeaponBar = true;
};


function Warp::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Fusion Cannon<f1>\nA larger, deadlier version of the Eldar Fusion Gun.");
}


