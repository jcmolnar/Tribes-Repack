// ===================Force Lightning=========================================
// ===========================================================================
$AutoUse[ForceLightning] = False;
// ===========================================================================
$WeaponAmmo[ForceLightning] = "";
// ===========================================================================
ItemImageData ForceLightningImage
{
	shapeFile = "force";
   mountPoint = 0; //   x  y  z ???
	mountOffset = { -0.04, -0.06, -0.19 };
	mountRotation = { 0, 0, 0 };



   weaponType = 2;  // Sustained
	projectileType = FLightning;
   minEnergy = 3;
   maxEnergy = 10;  // Energy used/sec for sustained weapons
	reloadTime = 0.1;
                        
   lightType = 3;  // Weapon Fire
   lightRadius = 2;
   lightTime = 1;
   lightColor = { 0.25, 0.25, 0.85 };
};
// ===========================================================================
ItemData ForceLightning
{
   description = "Force Lightning";
	shapeFile = "force";
	hudIcon = "energyRifle";
   className = "Weapon";
   heading = "cWeapons";
   shadowDetailMask = 4;
   imageType = ForceLightningImage;
	showWeaponBar = true;
   price = 0;
};
// ===========================================================================
function ForceLightning::onMount(%player,%item) 
{
%client = Player::getClient(%player);
Bottomprint(%client, "Force Lightning: Deals Medium Damage.  Allows You To Kill Players Within Seconds.");
}
// ===========================================================================