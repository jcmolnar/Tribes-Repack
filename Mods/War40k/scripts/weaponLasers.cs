//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Brightlance
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Brightlance] = 1;
$RemoteInvList[Brightlance] = 1;
$AutoUse[Brightlance] = False;
$WeaponAmmo[Brightlance] = "";

addWeapon(Brightlance);

$Needs[Brightlance] = EnergyPack;

LaserData BrightlanceBolt
{
   laserBitmapName   = "warp.bmp";
   hitName           = "laserhit.dts";

   damageConversion  = 0.025;
   baseDamageType    = $LaserDamageType;

   beamTime          = 1.0;

   lightRange        = 1.0;
   lightColor        = { 0.0, 1.25, 1.25 };

   detachFromShooter = false;
   hitSoundId        = SoundLaserHit;
};


ItemImageData BrightlanceImage
{
   shapeFile  = "Brightlance";
	mountPoint = 0;

      weaponType = 0; // Single Shot
	projectileType = BrightlanceBolt;
	reloadTime = 2.0;
	fireTime = 0.01;
	minEnergy = 25;
	maxEnergy = 25;

	accuFire = true;

	sfxFire = SoundFireLaser;
	sfxActivate = SoundPickUpWeapon;
};

ItemData Brightlance
{
   heading = $InvHead[ihWel];
	description = "Bright Lance";
	className = "Weapon";
   shapeFile  = "Brightlance";
	hudIcon = "blaster";
	shadowDetailMask = 4;
	imageType = BrightlanceImage;
	price = 16;
	showWeaponBar = true;
};

function Brightlance::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Brightlance<f1>\nA brutal laser weapon, far surpassing the Imperiums Las technology. Requires an Energy Pack");
}

function Brightlance::onUse(%player,%item)
{
	if(Player::getMountedItem(%player,$BackpackSlot) == EnergyPack || Player::getMountedItem(%player,$BackpackSlot) == LaserPack)
		Weapon::onUse(%player,%item);
        
	else
		Client::sendMessage(Player::getClient(%player),0,
			"Must have an Energy Pack to use a Bright Lance."); }

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Las Blaster
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[LasBlaster] = 1;
$RemoteInvList[LasBlaster] = 1;
$AutoUse[LasBlaster] = False;
$WeaponAmmo[LasBlaster] = "";
AddWeapon(LasBlaster);

LaserData LasBlasterBeam
{
   laserBitmapName   = "laserpulse.bmp";
   hitName           = "laserhit.dts";

   damageConversion  = 0.066;
   baseDamageType    = $LaserDamageType;

   beamTime          = 0.5;

   lightRange        = 1.0;
   lightColor        = { 1.0, 0.0, 0.0 };

   detachFromShooter = false;
   hitSoundId        = SoundLaserHit;
};

ItemImageData LasBlasterImage 
{
  shapeFile = "lasblaster";
  mountPoint = 0;
  weaponType = 0;
  minEnergy = 1;
  maxEnergy = 2;
  projectileType = LasBlasterBeam;
  accuFire = true;
  reloadTime = 0.03;
  lightType = 3;
  lightRadius = 1;
  lightTime = 1;
  lightColor = { 1, 0, 0 };
  sfxFire = SoundFireLaser;
  sfxActivate = SoundPickUpWeapon;
};

ItemData LasBlaster 
{
  description = "Las Blaster";
  className = "Weapon";
  shapeFile = "lasblaster";
  hudIcon = "sniper";
  heading = $InvHead[ihWel];
  shadowDetailMask = 4;
  imageType = LasBlasterImage;
  price = 4;
  showWeaponBar = true;
};

function LasBlaster::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Las Blaster<f1>\nFar surpassing the Imperium's Las Gun, this rapid fire laser is the pride of Swooping Hawks.");
}


//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Las Cannon
//  By <[DC]>Paladin
//
//
//  For installation information, see Install.txt
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$InvList[LasCannon] = 1;
$RemoteInvList[LasCannon] = 1;
$AutoUse[LasCannon] = False;
$WeaponAmmo[LasCannon] = "";

addWeapon(LasCannon);

$Needs[LasCannon] = LaserPack;

LaserData LasCannonBlast
{
   laserBitmapName   = "discglow1.bmp";
   hitName           = "laserhit.dts";

   damageConversion  = 0.055;
   baseDamageType    = $LaserDamageType;

   beamTime          = 2.0;

   lightRange        = 1.0;
   lightColor        = { 2.0, 0.0, 0.0 };

   detachFromShooter = false;
   hitSoundId        = SoundLaserHit;
};

ItemImageData LasCannonImage
{
	shapeFile = "lascannon";
	mountPoint = 0;
        weaponType = 0; // Single Shot
	projectileType = LasCannonBlast;
	accuFire = True;
	reloadTime = 4.0;
	fireTime = 0.05;
	minEnergy = 40;
	maxEnergy = 55;

	lightType = 3;  // Weapon Fire
	lightRadius = 5;
	lightTime = 1;
	lightColor = { 0.0, 0.0, 2.0 };

	sfxFire = SoundFireLas;
	sfxActivate = SoundPickUpWeapon;
};

ItemData LasCannon
{
	description = "Las Cannon";
	className = "Weapon";
	shapeFile = "lascannon";
	heading = $InvHead[ihWma];
	hudIcon = "sniper";
	shadowDetailMask = 4;
	imageType = LasCannonImage;
	price = 30;
	showWeaponBar = true;
};

function LasCannon::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Las Cannon<f1>\nThe most powerful laser weapon used by ground forces. With good aim it can kill a Terminator with a single shot. Requires an Adv. Energy Pack.");
}

function LasCannon::onUse(%player,%item)
{
	if(Player::getMountedItem(%player,$BackpackSlot) == LaserPack)
		Weapon::onUse(%player,%item);
	else
		Client::sendMessage(Player::getClient(%player),0,
			"Must have an Adv. Energy Pack to use a Las Cannon."); }


//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Scatter Laser
//  By <[DC]>Paladin
//
//
//  For installation information, see Install.txt
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$InvList[ScatterLas] = 1;
$RemoteInvList[ScatterLas] = 1;
$AutoUse[ScatterLas] = False;
$WeaponAmmo[ScatterLas] = "";

addWeapon(ScatterLas);

$Needs[ScatterLas] = LaserPack;

LaserData ScatterLasCharge
{
   laserBitmapName   = "laserpulse.bmp";
   hitName           = "laserhit.dts";

   damageConversion  = 0.02;
   baseDamageType    = $LaserDamageType;

   beamTime          = 0.2;

   lightRange        = 1.0;
   lightColor        = { 0.0, 2.25, 0.0 };

   detachFromShooter = false;
   hitSoundId        = SoundLaserHit;
};

ItemImageData ScatterLasImage
{
	shapeFile = "mortargun";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	projectileType = ScatterLasCharge;
	accuFire = True;
	reloadTime = 0.05;
	fireTime = 0.0;
	minEnergy = 3;
	maxEnergy = 5;

	lightType = 3;  // Weapon Fire
	lightRadius = 2;
	lightTime = 1;
	lightColor = { 2.5, 2.0, 2.0 };

	sfxFire = SoundFireLaser;
	sfxActivate = SoundPickUpWeapon;
};

ItemData ScatterLas
{
	description = "Scatter Laser";
	className = "Weapon";
	shapeFile = "mortargun";
	heading = $InvHead[ihWel];
	hudIcon = "sniper";
	shadowDetailMask = 4;
	imageType = ScatterLasImage;
	price = 25;
	showWeaponBar = true;
};

function ScatterLas::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Scatter Laser<f1>\nA monstrous rapid-fire laser, built for the bulk of a Wraithlord.");
}



