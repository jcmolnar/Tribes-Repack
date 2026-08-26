
//-=-=-==-==-=-=-=--=-=-=-=-=-=-
//  Force Fields
// Original creation Alazane
//-=-=-===-==-=-=-=-=-=-=-=-=-=-=

 // Adjust this to adjust the number a team can have
$TeamItemMax[ForceFieldPack] = 20; 
$TeamItemMax[LargeForceFieldPack] = 4; 

$InvList[ForceFieldPack] = 1; 
$RemoteInvList[ForceFieldPack] = 1; 

$InvList[LargeForceFieldPack] = 1; 
$RemoteInvList[LargeForceFieldPack] = 1; 

$CanAlwaysTeamDestroy[ForceField] = 1;
$CanAlwaysTeamDestroy[LargeForceFieldPack] = 1;

 //-=-=-=-=-=-=-=- Initialize =-=-=-=-=-=-=-

function deployForceField::Initialize()
{
	$TeamItemCount[0 @ ForceFieldPack] = 0; 
	$TeamItemCount[1 @ ForceFieldPack] = 0; 
	$TeamItemCount[2 @ ForceFieldPack] = 0; 
	$TeamItemCount[3 @ ForceFieldPack] = 0; 
	$TeamItemCount[4 @ ForceFieldPack] = 0; 
	$TeamItemCount[5 @ ForceFieldPack] = 0; 
	$TeamItemCount[6 @ ForceFieldPack] = 0; 
	$TeamItemCount[7 @ ForceFieldPack] = 0; 

	$TeamItemCount[0 @ LargeForceFieldPack] = 0;
	$TeamItemCount[1 @ LargeForceFieldPack] = 0;
	$TeamItemCount[2 @ LargeForceFieldPack] = 0;
	$TeamItemCount[3 @ LargeForceFieldPack] = 0;
	$TeamItemCount[4 @ LargeForceFieldPack] = 0;
	$TeamItemCount[5 @ LargeForceFieldPack] = 0;
	$TeamItemCount[6 @ LargeForceFieldPack] = 0;
	$TeamItemCount[7 @ LargeForceFieldPack] = 0;
}

 //-=-=-=-=-=-=-=- Packs =-=-=-=-=-=-=-

ItemImageData ForceFieldPackImage 
{ 
	shapeFile = "AmmoPack"; 
	mountPoint = 2; 
	mountOffset = { 0, -0.03, 0 }; 
	mass = 2.5; 
	firstPerson = false; 
}; 

ItemData ForceFieldPack 
{ 
	description = "Force Field"; 
	shapeFile = "AmmoPack"; 
	className = "Backpack"; 
	heading = $InvHead[ihDrs]; 
	imageType = ForceFieldPackImage; 
	shadowDetailMask = 4; 
	mass = 1.5; 
	elasticity = 0.2; 
	price = 30; 
	hudIcon = "deployable"; 
	showWeaponBar = true; 
	hiliteOnActive = true; 
}; 

function ForceFieldPack::onUse(%player,%item) 
{ 
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
		Player::mountItem(%player,%item,$BackpackSlot); 
	else 
		Player::deployItem(%player,%item); 
} 

function ForceFieldPack::onDeploy(%player,%item,%pos) 
{ 
	if (ForceFieldPack::deployShape(%player,%item)) 
		Player::decItemCount(%player,%item);

} 

function ForceFieldPack::deployShape(%player,%item) 
{ 
	%client = Player::getClient(%player); 
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]) 
	{ Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s"); return false; }

	if (!GameBase::getLOSInfo(%player,3)) 
	{ Client::sendMessage(%client,0,"Deploy position out of range"); return false; }

	if (Vector::dot($los::normal,"0 0 1") <= 0.7) 
	{ Client::sendMessage(%client,0,"Can only deploy on flat surfaces"); return false; }

	 //
	 // Passed validation, create the object
	 //
	%rot = GameBase::getRotation(%player); 
	%objForceField = newObject("","StaticShape",DeployableForceField,true); 
	addToSet("MissionCleanup", %objForceField); 
	GameBase::setTeam(%objForceField,GameBase::getTeam(%player)); 
	GameBase::setPosition(%objForceField,$los::position); 
	GameBase::setRotation(%objForceField,%rot); 
	Gamebase::setMapName(%objForceField,"Force Field"); 
	Client::sendMessage(%client,0,"Force Field Deployed"); 
	GameBase::startFadeIn(%objForceField); 
	playSound(SoundPickupBackpack,$los::position); 
	playSound(ForceFieldOpen,$los::position); 
	$TeamItemCount[GameBase::getTeam(%player) @ "ForceFieldPack"]++; 
//        reportDeploy(%objForceField, %client);
	echo("MSG: ",%client," deployed a Force Field");
	return true; 
} 

ItemImageData LargeForceFieldPackImage 
{ 
	shapeFile = "AmmoPack"; 
	mountPoint = 2; 
	mountOffset = { 0, -0.03, 0 }; 
	mass = 2.5; 
	firstPerson = false; 
}; 

ItemData LargeForceFieldPack 
{ 
	description = "Large Force Field"; 
	shapeFile = "AmmoPack"; 
	className = "Backpack"; 
	heading = $InvHead[ihDrs]; 
	imageType = LargeForceFieldPackImage; 
	shadowDetailMask = 4; 
	mass = 1.5; 
	elasticity = 0.2; 
	price = 75; 
	hudIcon = "deployable"; 
	showWeaponBar = true; 
	hiliteOnActive = true; 
}; 

function LargeForceFieldPack::onUse(%player,%item) 
{ 
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
		Player::mountItem(%player,%item,$BackpackSlot); 
	else 
		Player::deployItem(%player,%item);
} 

function LargeForceFieldPack::onDeploy(%player,%item,%pos) 
{ 
	if (LargeForceFieldPack::deployShape(%player,%item)) 
		Player::decItemCount(%player,%item);  
} 

function LargeForceFieldPack::deployShape(%player,%item) 
{ 
	%client = Player::getClient(%player); 
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]) 
	{ Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s"); return false; }

	if (!GameBase::getLOSInfo(%player,3)) 
	{ Client::sendMessage(%client,0,"Deploy position out of range"); return false; }

	// %obj = getObjectType($los::object); 
	// %set = newObject("set",SimSet); 
	// %num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$ForceFieldBoxMinLength,$ForceFieldBoxMinWidth,$ForceFieldBoxMinHeight,0); 
	// %num = CountObjects(%set,"DeployableForceField",%num); 
	// deleteObject(%set); 

	if (Vector::dot($los::normal,"0 0 1") <= 0.7) 
	{ Client::sendMessage(%client,0,"Can only deploy on flat surfaces"); return false; }

	 //
	 // Passed validation, create the object
	 //
	%rot = GameBase::getRotation(%player); 
	%objForceField = newObject("","StaticShape",LargeForceField,true); 
	addToSet("MissionCleanup", %objForceField); 
	GameBase::setTeam(%objForceField,GameBase::getTeam(%player)); 
	GameBase::setPosition(%objForceField,$los::position); 
	GameBase::setRotation(%objForceField,%rot); 
	Gamebase::setMapName(%objForceField,"Force Field"); 
	Client::sendMessage(%client,0,"Force Field Deployed"); 
	GameBase::startFadeIn(%objForceField); 
	playSound(SoundPickupBackpack,$los::position); 
	playSound(ForceFieldOpen,$los::position); 
	$TeamItemCount[GameBase::getTeam(%player) @ "LargeForceFieldPack"]++; 
//        reportDeploy(%objForceField, %client);
	echo("MSG: ",%client," deployed a Force Field");
	return true; 
} 

 //-=-=-=-=-=-=-=- Objects =-=-=-=-=-=-=-

StaticShapeData DeployableForceField 
{ 
	shapeFile = "forcefield_5x5"; 
	debrisId = defaultDebrisSmall; 
	maxDamage = 10.50; 
	visibleToSensor = true; 
	isTranslucent = true; 
	description = "Force Field"; 
}; 

function DeployableForceField::onDestroyed(%this) 
{ 
	StaticShape::objectiveDestroyed(%this);
	$TeamItemCount[GameBase::getTeam(%this) @ "ForceFieldPack"]--; 
} 

StaticShapeData LargeForceField 
{ 
	shapeFile = "forcefield"; 
	debrisId = defaultDebrisLarge; 
	maxDamage = 20.00; 
	visibleToSensor = true; 
	isTranslucent = true; 
	description = "Force Field"; 
}; 

function LargeForceField::onDestroyed(%this) 
{ 
	StaticShape::objectiveDestroyed(%this);
	$TeamItemCount[GameBase::getTeam(%this) @ "LargeForceFieldPack"]--; 
} 
