//Deployable building-script pack
ItemImageData DepBasePackImage
{
	shapeFile = "shieldPack";
	mountPoint = 2;
	mountOffset = { 0, -0, 0 };
	mass = 0.0;
	firstPerson = false;
};
ItemData DepBasePack
{
	description = "Deployable Base Pack";
	shapeFile = "shieldPack";
	className = "Backpack";
	heading = "dDeployables";
	imageType = DepBasePackImage;
	shadowDetailMask = 4;
	mass = 0.0;
	elasticity = 0.2;
        price = 40000;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};
function DepBasePack::onUse(%player,%item)
{
	dbecho($dbechoMode, "DepBasePack::onUse(" @ %player @ ", " @ %item @ ")");

	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}
function DepBasePack::onDeploy(%player,%item,%pos)
{
	dbecho($dbechoMode, "DepBasePack::onDeploy(" @ %player @ ", " @ %item @ ", " @ %pos @ ")");

	if (DepBasePack::deployShape(%player,%item))
	{
		if(Player::getClient(%player).adminLevel < 4)
			Player::decItemCount(%player,%item);
	}
}
function DepBasePack::deployShape(%player,%item)
{
	dbecho($dbechoMode, "DepBasePack::deployShape(" @ %player @ ", " @ %item @ ")");

	%clientId = Player::getClient(%player);
	if (GameBase::getLOSInfo(%player,3))
	{
		if (Vector::dot($los::normal,"0 0 1") > 0.7)
		{
			if(Zone::getType(fetchData(%clientId, "zone")) != "PROTECTED" || %clientId.adminLevel >= 4)
			{
				%rot = GameBase::getRotation(%player);
				DeployBase(%clientId, "base1.cs", $los::position, %rot);
			}
			else 
			{
				Client::sendMessage(%clientId,0,"You are not allowed to deploy bases inside protected territory.");
				return false;
			}
		}
	}
	else 
	{
		Client::sendMessage(%clientId,0,"Deploy position out of range");
		return false;
	}
}