// ===================Heavy Guard Rifle=====================================
// ===========================================================================
$AutoUse[GuardGun] = True;
// ===========================================================================
$WeaponAmmo[GuardGun] = GuardGunAmmo;
// ===========================================================================
$SellAmmo[GuardGunAmmo] = 5;

// ===========================================================================
ItemData GuardGunAmmo
{
	description = "Heavy Blaster Cells";
	className = "Ammo";
	shapeFile = "discammo";
   heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 0;
	team=-1;
};

// ===========================================================================
ItemImageData GuardGunImage
{
   shapeFile  = "GuardGun"; 
	mountPoint = 0;
    accuFire = true;
	weaponType = 0; // Single Shot
    reloadTime = 0.25;
	fireTime = 0.0;
	projectileType = GuardGunBullet;

    ammotype = GuardGunAmmo;
	
   lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	  lightColor        = { 0.25, 1.0, 0.25 };	


	sfxFire = SoundFireBlaster;
	sfxActivate = SoundPickUpWeapon;
};
// ===========================================================================
ItemData GuardGun
{
   heading = "cWeapons";
	description = "Coruscant Guard Rifle";
	className = "Weapon";
   shapeFile  = "GuardGun";
	hudIcon = "blaster";
	shadowDetailMask = 4;
	imageType = GuardGunImage;
	price = 0;
	showWeaponBar = true;
	team=0;
};
// ===========================================================================
function GuardGun::onMount(%player,%item) 
{
%client = Player::getClient(%player);
Bottomprint(%client, "Coruscant Guard Rifle: Based on the Standard E-11 Blaster Rifle Technology, This Weapon Has a High Rate Of Fire, And Does Very High Damage.");
}
// ===========================================================================