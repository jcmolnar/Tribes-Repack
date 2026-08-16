// ===================Flak cannon=============================================
// ===========================================================================
$AutoUse[FlakCannon] = True;

// ===========================================================================
ItemImageData FlakCannonImage
{
   shapeFile  = "flakcannon"; 
	mountPoint = 0;
    accuFire = true;
	weaponType = 0; // Single Shot
    reloadTime = 0.5;
	fireTime = 0;
	projectileType = noprojectile;

    ammotype = FlakAmmo;
	
    lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 1.0, 0.25, 0.25 };	

	sfxFire = mineExplosion;

	sfxActivate = SoundPickUpWeapon;
};
// ===========================================================================
ItemData FlakCannon
{
   heading = "cWeapons";
	description = "Flak Cannon";
	className = "Weapon";
   shapeFile  = "flakcannon";
	hudIcon = "blast";
	shadowDetailMask = 4;
	imageType = FlakCannonImage;
	price = 0;
	showWeaponBar = true;
	team=1;
};
// ===========================================================================
function FlakCannon::onMount(%player,%item) 
{
%client = Player::getClient(%player);
Bottomprint(%client, "Flak Cannon: Only Suitable for Stronger Armors, and highly trained armies.  Shoots groups of Twisted schards of metal.");
}
// ===========================================================================
function FlakcannonImage::onFire(%player, %slot) 
{
	Player::decItemCount(%player, FlakAmmo, 1);
	%trans = GameBase::getMuzzleTransform(%player); 
	%vel = Item::getVelocity(%player);
	
	for(%i=0; %i<12; %i++)
		%proj = Projectile::spawnProjectile("Flak",%trans,%player,%vel);
		
}
