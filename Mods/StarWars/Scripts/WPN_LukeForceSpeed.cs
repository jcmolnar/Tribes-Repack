// ===================Force Speed=============================================
// ===========================================================================
$AutoUse[ForceSpeed] = False;
// ===========================================================================
$WeaponAmmo[ForceSpeed] = "";

ItemImageData ForceSpeedImage
{
	shapeFile = "force";
   	mountPoint = 0;
	mountOffset = { -0.04, -0.06, -0.19 };
	mountRotation = { 0, 0, 0 };
	weaponType = 0;  // single
	minEnergy = 0;
	maxEnergy = 0;  // Energy used/sec for sustained weapons
	reloadTime = 1;
	fireTime = 1;
};
// ===========================================================================
ItemData ForceSpeed
{
	description = "Force Speed";
	shapeFile = "force";
	hudIcon = "energyRifle";
	className = "Weapon";
	heading = "cRebel Weapons";
	shadowDetailMask = 4;
	imageType = ForceSpeedImage;
	showWeaponBar = true;
	price = 0;
};

function ForceSpeed::onMount(%player,%item) 
{
	Bottomprint(Player::getClient(%player), "Force Speed: Uses The Force To Allow Sudden Attacks, And Retreats.");
}

function ForceSpeedImage::onFire(%player, %slot) 
{
	%energy=GameBase::getEnergy(%player);
	if($force_delay[%player]<=0&&Player::getArmor(%player)==luarmor&&%energy>=30)//enough e
	{
		GameBase::setEnergy(%player,%energy-30);
		schedule("set_speedluke(" @ %player @ ");",1,%player);
		Bottomprint(Player::getClient(%player), "Force Speed: On.");
		schedule("eCheck(" @ %player @ ");",2,%player);
		$force_delay[%player]=50;
	}
	else if($force_delay[%player]<=0&&Player::getArmor(%player)==sluarmor)
	{
		schedule("set_luke(" @ %player @ ");",1,%player);
		Bottomprint(Player::getClient(%player), "Force Speed: Off.");
		$force_delay[%player]=100;
	}
	$force_delay[%player]--;
}

function eCheck(%player)
{
	$force_delay[%player]-=10;
	%energy=GameBase::getEnergy(%player);
	if(Player::getArmor(%player)==sluarmor&&%energy>15)//enough e
	{
		GameBase::setEnergy(%player,%energy-15);
		schedule("eCheck(" @ %player @ ");",1,%player);
	}
	else
	{
		schedule("set_luke(" @ %player @ ");",1,%player);
		Bottomprint(Player::getClient(%player), "Force Speed: Off.");
		$force_delay[%player]=100;
	}
}

function set_luke(%player)
{
	$force_delay[%player]=100;
	schedule("$force_delay[" @ %player @ "]=0;",5,%player);
	%energy=GameBase::getEnergy(%player);
	Player::setArmor(%player,luarmor); 
	GameBase::setEnergy(%player,%energy);
	Player::trigger(%player,$WeaponSlot,false);
}
function set_speedluke(%player)
{
	$force_delay[%player]=50;
	schedule("$force_delay[" @ %player @ "]=0;",5,%player);
	%energy=GameBase::getEnergy(%player);
	Player::setArmor(%player,sluarmor); 
	GameBase::setEnergy(%player,%energy);
	Player::trigger(%player,$WeaponSlot,false);
}