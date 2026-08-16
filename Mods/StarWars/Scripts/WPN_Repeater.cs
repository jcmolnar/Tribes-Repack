// ===================Trooper Repeater========================================
// ===========================================================================
$AutoUse[Repeater] = True;
// ===========================================================================
$WeaponAmmo[Repeater] = RepeaterAmmo;
// ===========================================================================
$SellAmmo[RepeaterAmmo] = 50;
// ===========================================================================
//--------------------------------------
BulletData RepeaterBullet
{
   bulletShapeName    = "blastred.dts";
   validateShape      = true;
   explosionTag       = redblast;
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
   isVisible          = False;

   tracerPercentage   = 1.0;
   tracerLength       = 60;
};


//----------------------------------------------------------------------------

ItemData RepeaterAmmo
{
	description = "Repeater Ammo";
	className = "Ammo";
	shapeFile = "ammo1";
   heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 0;
	team=-1;
};

ItemImageData RepeaterImage
{
	shapeFile = "repeater";
	mountPoint = 0;

	weaponType = 1; // Spinning
	reloadTime = 0;
	spinUpTime = 1.0;
	spinDownTime = 3;
	fireTime = 0.1;

	ammoType = RepeaterAmmo;
	projectileType = RepeaterBullet;
	accuFire = true;

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1 };

	sfxFire = SoundFireRepeater;
	sfxActivate = SoundPickUpWeapon;
	sfxSpinUp = SoundSpinUp;
	sfxSpinDown = SoundSpinDown;
};

ItemData Repeater
{
	description = "Repeater";
	className = "Weapon";
	shapeFile = "repeater";
   validateShape = false;
	hudIcon = "repeater";
   heading = "cWeapons";
	shadowDetailMask = 4;
	imageType = RepeaterImage;
	price = 0;
	showWeaponBar = true;
	team=1;
};


// ===========================================================================
function Repeater::onMount(%player,%item) 
{
%client = Player::getClient(%player);
Bottomprint(%client, "Repeater: A Storm Troopers Best Friend When it comes to Defense.  Medium Spread Makes this A Deadly Weapon.");
}
// ===========================================================================