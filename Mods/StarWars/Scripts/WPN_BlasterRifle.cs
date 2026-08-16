// ===================Heavy Blaster Rifle=====================================
// ===========================================================================
$AutoUse[BlasterRifle] = True;
// ===========================================================================
$WeaponAmmo[BlasterRifle] = BlasterRifleAmmo;
// ===========================================================================
$SellAmmo[BlasterRifleAmmo] = 5;

// ===========================================================================
ItemData BlasterRifleAmmo
{
	description = "Heavy Blaster Cells";
	className = "Ammo";
	shapeFile = "discammo";
   heading = "xAmmunitionImp";
	shadowDetailMask = 4;
	price = 0;
	team=-1;
};

// ===========================================================================
ItemImageData BlasterRifleImage
{
   shapeFile  = "bstrrfle"; 
	mountPoint = 0;
    accuFire = true;
	weaponType = 0; // Single Shot
    reloadTime = 0.25;
	fireTime = 0.0;
	projectileType = BlasterRifleBullet;

    ammotype = BlasterRifleAmmo;
	
    lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 1.0, 0.25, 0.25 };	

	sfxFire = SoundFireBlaster;
	sfxActivate = SoundPickUpWeapon;
};
// ===========================================================================
ItemData BlasterRifle
{
   heading = "cWeaponsImp";
	description = "BlasTech E-11 Rifle";
	className = "Weapon";
   shapeFile  = "bstrrfle";
	hudIcon = "blast";
	shadowDetailMask = 4;
	imageType = BlasterRifleImage;
	price = 0;
	showWeaponBar = true;
	team=1;
};
// ===========================================================================
function BlasterRifle::onMount(%player,%item) 
{
%client = Player::getClient(%player);
Bottomprint(%client, "Heavy Blaster Rifle: Standard E-11, Folding Stock Fitted With Internal Energy Cells Fire, Very High Damage.");
}
// ===========================================================================