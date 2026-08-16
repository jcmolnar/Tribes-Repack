// ===================Torplauncher+===========================================
// ===========================================================================
$AutoUse[Conclauncher] = True;
// ===========================================================================
$WeaponAmmo[ConcLauncher] = ConcAmmo;
// ===========================================================================
$SellAmmo[ConcAmmo] = 5;

// ===========================================================================
ItemData ConcAmmo
{
	description = "C. Torpedos";
	className = "Ammo";
	shapeFile = "mortarammo";
   heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 0;

};
// ===========================================================================
SeekingMissileData CTorpedo
{
   bulletShapeName = "rocket.dts";
   explosionTag    = rocketExp;
   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.5;
   damageType       = $MissileDamageType;
   explosionRadius  = 5.5;
   kickBackStrength = 175.0;

   muzzleVelocity    = 110.0;
   totalTime         = 10;
   liveTime          = 10;
   seekingTurningRadius    = 9;
   nonSeekingTurningRadius = 75.0;
   proximityDist     = 1.5;
   smokeDist         = 1.75;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

   soundId = SoundJetHeavy;
};



// ===========================================================================
ItemImageData ConclauncherImage
{
   shapeFile  = "Torplauncher"; 
	mountPoint = 0;
	mountOffset = { -0.1, 0.0, 0.1 };
	mountRotation = { 0, 0.0, 0 };
    accuFire = true;
	weaponType = 0; // Single Shot
    reloadTime = 1.0;
	fireTime = 0;

    ammotype = Concammo;
	
    lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 1.0, 0.25, 0.25 };	

	sfxFire = SoundFireBlaster;
	sfxActivate = SoundPickUpWeapon;
};
// ===========================================================================
ItemData Conclauncher
{
   heading = "cRebel Weapons";
	description = "Conc. Torpedo Launcher";
	className = "Weapon";
   shapeFile  = "Torplauncher";
	hudIcon = "blaster";
	shadowDetailMask = 4;
	imageType = ConcLauncherImage;
	price = 0;
	showWeaponBar = true;
	team=1;
};
// ===========================================================================
function Conclauncher::onMount(%player,%item) 
{
%client = Player::getClient(%player);
Bottomprint(%client, "Portable Concussion Torpedo Launcher: Dumb fire mode only, does not lock onto anything, and does high damage.");
}
// ===========================================================================
function ConcLauncherImage::onFire(%player, %slot) 
{ 
%trans = GameBase::getMuzzleTransform(%player); 
%vel = Item::getVelocity(%player); 
Projectile::spawnProjectile(ctorpedo,%trans,%player,%veloc); 
%client = Player::getClient(%player);
Bottomprint(%client, "Firing Concussion Torpedo");

Player::decItemCount(%player,ConcAmmo,1); 
}

// ===========================================================================