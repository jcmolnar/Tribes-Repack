//-=-=-==-==-=-=-=--=-=-=-=-=-=-
//   Advanced Force Field
// Original creation Edgecrusher
//-=-=-===-==-=-=-=-=-=-=-=-=-=-=

 // Adjust this to adjust the number a team can have
$TeamItemMax[BigFieldPack] = 4; 
$InvList[BigFieldPack] = 1; 
$RemoteInvList[BigFieldPack] = 1; 

$CanAlwaysTeamDestroy[BigFieldPack] = 1;


 //-=-=-=-=-=-=-=- Initialize =-=-=-=-=-=-=-

function deployBigField::Initialize()
{
	$TeamItemCount[0 @ BigFieldPack] = 0; 
	$TeamItemCount[1 @ BigFieldPack] = 0; 
	$TeamItemCount[2 @ BigFieldPack] = 0; 
	$TeamItemCount[3 @ BigFieldPack] = 0; 
	$TeamItemCount[4 @ BigFieldPack] = 0; 
	$TeamItemCount[5 @ BigFieldPack] = 0; 
	$TeamItemCount[6 @ BigFieldPack] = 0; 
	$TeamItemCount[7 @ BigFieldPack] = 0; 
}

 //-=-=-=-=-=-=-=- Packs =-=-=-=-=-=-=-

ItemImageData BigFieldPackImage 
{ 
	shapeFile = "AmmoPack"; 
	mountPoint = 2; 
	mountOffset = { 0, -0.03, 0 }; 
	mass = 2.5; 
	firstPerson = false; 
}; 

ItemData BigFieldPack 
{ 
	description = "Adv. Force Field"; 
	shapeFile = "AmmoPack"; 
	className = "Backpack"; 
	heading = $InvHead[ihDrs]; 
	imageType = BigFieldPackImage; 
	shadowDetailMask = 4; 
	mass = 3.5; 
	elasticity = 0.2; 
	price = 100; 
	hudIcon = "deployable"; 
	showWeaponBar = true; 
	hiliteOnActive = true; 
}; 

function BigFieldPack::onUse(%player,%item) 
{ 
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
		Player::mountItem(%player,%item,$BackpackSlot); 
	else 
		Player::deployItem(%player,%item); 
} 

function BigFieldPack::onDeploy(%player,%item,%pos) 
{ 
	if (BigFieldPack::deployShape(%player,%item)) 
		Player::decItemCount(%player,%item); 
} 

function BigFieldPack::deployShape(%player,%item) 
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
	%objForceField = newObject("","StaticShape",DeployableBigField,true); 
	addToSet("MissionCleanup", %objForceField); 
	GameBase::setTeam(%objForceField,GameBase::getTeam(%player)); 
	GameBase::setPosition(%objForceField,$los::position); 
	GameBase::setRotation(%objForceField,%rot); 
	Gamebase::setMapName(%objForceField,"Adv. Force Field"); 
	Client::sendMessage(%client,0,"Adv. Force Field Deployed"); 
	GameBase::startFadeIn(%objForceField); 
	playSound(SoundPickupBackpack,$los::position); 
	playSound(ForceFieldOpen,$los::position); 
	$TeamItemCount[GameBase::getTeam(%player) @ "BigFieldPack"]++; 
//        reportDeploy(%objForceField, %client);
	echo("MSG: ",%client," deployed an Adv. Force Field");
	return true; 
} 


 //-=-=-=-=-=-=-=- Objects =-=-=-=-=-=-=-

StaticShapeData DeployableBigField 
{ 
	shapeFile = "forcefield_4x17"; 
	debrisId = defaultDebrisSmall; 
	maxDamage = 30.50; 
	visibleToSensor = true; 
	isTranslucent = true; 
	description = "Force Field"; 
}; 

function DeployableBigField::onDestroyed(%this) 
{ 
	StaticShape::objectiveDestroyed(%this);
	$TeamItemCount[GameBase::getTeam(%this) @ "ForceFieldPack"]--; 
} 


