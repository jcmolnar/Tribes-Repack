// ===================Torplauncher+===========================================
// ===========================================================================
$AutoUse[Torplauncher] = True;
// ===========================================================================
$WeaponAmmo[TorpLauncher] = PTorpedoAmmo;
// ===========================================================================
$SellAmmo[PTorpedoAmmo] = 5;

// ===========================================================================
ItemData PTorpedoAmmo
{
	description = "P. Torpedos";
	className = "Ammo";
	shapeFile = "rocket";
   heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 0;

};
// ===========================================================================
SeekingMissileData PTorpedo
{
   bulletShapeName = "rocket.dts";
   explosionTag    = rocketExp;
   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 1.5;
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
ItemImageData TorplauncherImage
{
   shapeFile  = "Torplauncher"; 
	mountPoint = 0;
	mountOffset = { -0.1, 0.0, 0.1 };
	mountRotation = { 0, 0.0, 0 };
    accuFire = true;
	weaponType = 0; // Single Shot
    reloadTime = 1.0;
	fireTime = 0;

    ammotype = PTorpedoammo;
	
    lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 1.0, 0.25, 0.25 };	

	sfxFire = SoundFireBlaster;
	sfxActivate = SoundPickUpWeapon;
};
// ===========================================================================
ItemData Torplauncher
{
   heading = "cWeapons";
	description = "Torpedo Launcher";
	className = "Weapon";
   shapeFile  = "Torplauncher";
	hudIcon = "blaster";
	shadowDetailMask = 4;
	imageType = TorpLauncherImage;
	price = 0;
	showWeaponBar = true;
	team=1;
};
// ===========================================================================
function Torplauncher::onMount(%player,%item) 
{
%client = Player::getClient(%player);
Bottomprint(%client, "Portable Proton Torpedo Launcher: Locks onto vehicles, and fires normal when there is no lock, does high damage.");
}
// ===========================================================================
function TorpLauncherImage::onFire(%player, %slot) 
{ 
%trans = GameBase::getMuzzleTransform(%player); 
%vel = Item::getVelocity(%player); 
if(GameBase::getLOSInfo(%player, 3000.0) && (getObjectType($los::object) == "Flier")) 
{
Projectile::spawnProjectile(PTorpedo,%trans,%player,%veloc, $los::object); 
%client = Player::getClient(%player);
Bottomprint(%client, "Lock Detected, Firing Seeking Torpedo");
}
else 
{
Projectile::spawnProjectile(PTorpedo,%trans,%player,%veloc); 
%client = Player::getClient(%player);
Bottomprint(%client, "No Lock Detected, Firing Normal Torpedo");
}
Player::decItemCount(%player,PTorpedoAmmo,1); 
} 
// ===========================================================================