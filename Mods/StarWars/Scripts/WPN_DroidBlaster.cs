// ===================Droid Blaster===========================================
// ===========================================================================
$AutoUse[Blaster] = True;

// ===========================================================================
ItemImageData BlasterImage
{
   shapeFile  = "droidgun"; 
	mountPoint = 0;
    accuFire = true;
	weaponType = 0; // Single Shot
    reloadTime = 0.2;
	fireTime = 0;
	projectileType = DroidBlast;

    ammotype = BlasterAmmo;
	
    lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 1.0, 0.25, 0.25 };	

	sfxFire = SoundFireBlaster;
	sfxActivate = SoundPickUpWeapon;
};
// ===========================================================================
ItemData Blaster
{
   heading = "cImperial Weapons";
	description = "Battle Droid Blaster";
	className = "Weapon";
   shapeFile  = "droidgun";
	hudIcon = "blast";
	shadowDetailMask = 4;
	imageType = BlasterImage;
	price = 0;
	showWeaponBar = true;
	team=1;
};
// ===========================================================================
function Blaster::onMount(%player,%item) 
{
%client = Player::getClient(%player);
Bottomprint(%client, "Battle Droid Blaster: Only Suitable For A Battle Droids Hands.  High Rate Of Fire Enables This Weapon To Chew Up Most Armors Within A Few Seconds.");
}
// ===========================================================================