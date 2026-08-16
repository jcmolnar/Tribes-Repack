// ===================Force Throw=============================================
// ===========================================================================
$AutoUse[ForceThrow] = False;
// ===========================================================================
$WeaponAmmo[ForceThrow] = "";
// ===========================================================================
ItemImageData ForceThrowImage
{
	shapeFile = "force";
   	mountPoint = 0;
	mountOffset = { -0.04, -0.06, -0.19 };
	mountRotation = { 0, 0, 0 };

   weaponType = 2;  // Sustained
	projectileType = throw;
   minEnergy = 50;
   maxEnergy = 100;  // Energy used/sec for sustained weapons
	reloadTime = 0.2;
                        
   lightType = 3;  // Weapon Fire
   lightRadius = 2;
   lightTime = 1;
   lightColor = { 0.25, 0.25, 0.85 };
};
// ===========================================================================
ItemData ForceThrow
{
   description = "Force Throw";
	shapeFile = "force";
	hudIcon = "energyRifle";
   className = "Weapon";
   heading = "cWeapons";
   shadowDetailMask = 4;
   imageType = ForceThrowImage;
	showWeaponBar = true;
   price = 0;
	team=1;
};
// ===========================================================================
function ForceThrow::onMount(%player,%item) 
{
%client = Player::getClient(%player);
Bottomprint(%client, "Force Throw: Deals Low Damage But Allows You To Toss Any Enemy Around Like A Toy");
}
// ===========================================================================
