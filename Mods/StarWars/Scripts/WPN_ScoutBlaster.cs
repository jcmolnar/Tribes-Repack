// ===================Scout Gun===============================================
// ===========================================================================
$AutoUse[ScoutGun] = True;

// ===========================================================================
ItemImageData ScoutGunImage
{
   shapeFile  = "ScoutGun"; 
	mountPoint = 0;
    accuFire = true;
	weaponType = 0; // Single Shot
    reloadTime = 0.2;
	fireTime = 0;
	projectileType = ScoutBlast;

    ammotype = BlasterAmmo;
	
   lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	  lightColor        = { 0.25, 1.0, 0.25 };	


	sfxFire = SoundFireBlaster;
	sfxActivate = SoundPickUpWeapon;
};
// ===========================================================================
ItemData ScoutGun
{
   heading = "cRebel Weapons";
	description = "Scout Blaster";
	className = "Weapon";
   shapeFile  = "ScoutGun";
	hudIcon = "blast";
	shadowDetailMask = 4;
	imageType = ScoutGunImage;
	price = 0;
	showWeaponBar = true;
	team=1;
};
// ===========================================================================
function ScoutGun::onMount(%player,%item) 
{
%client = Player::getClient(%player);
Bottomprint(%client, "Scout Blaster: Small, But With A High Rate Of Fire.  This Weapon Is Not To Be Underestimated.");
}
// ===========================================================================