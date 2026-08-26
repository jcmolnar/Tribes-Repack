$InvList[DeadWeight] = 0;
$RemoteInvList[DeadWeight] = 0;

ItemImageData DeadWeightImage
{
	shapeFile = "breath";
	mountPoint = 4;
	mass = 200.0;
};

ItemData DeadWeight
{
	description = "Deadweight";
	className = "Tool";
	shapeFile = "grenammo";
	heading = "eDeployables";
	shadowDetailMask = 4;
	imageType = DeadWeightImage;
	price = 0;
	showWeaponBar = false;
	mass = 200.0;
	showInventory = false;
};

function DeadWeight::onDrop(%player, %item)
{
}
