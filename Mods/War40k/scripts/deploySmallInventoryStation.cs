
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Small Inventory Station (deployable)
//  By Dynamix
//  Cleaned up by Alazane (alazane@rkeng.com)
//
//  Installation:
//
//**Add the line 
//    exec(deploySmallInventoryStation);
//  in the file "server.cs" in the procedure 
//  "createServer" (just look for the other "exec"s--
//  must at least be before call to 
//  "preloadServerDataBlocks").
//  
//**Add the line 
//    deploySmallInventoryStation::Initialize();
//  also in the file "server.cs" in the procedure 
//  "Server::finishMissionLoad" right after
//  "Mission::reinitData".
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$TeamItemMax[DeployableInvPack] = 20; 
$InvList[DeployableInvPack] = 1; 
$RemoteInvList[DeployableInvPack] = 1;
$CanAlwaysTeamDestroy[DeployableInvStation] = 1;

 //-=-=-=-=-=-=-=- Initialize =-=-=-=-=-=-=-

function deploySmallInventoryStation::Initialize()
{
	$TeamItemCount[0 @ DeployableInvPack] = 0; 
	$TeamItemCount[1 @ DeployableInvPack] = 0; 
	$TeamItemCount[2 @ DeployableInvPack] = 0; 
	$TeamItemCount[3 @ DeployableInvPack] = 0; 
	$TeamItemCount[4 @ DeployableInvPack] = 0; 
	$TeamItemCount[5 @ DeployableInvPack] = 0; 
	$TeamItemCount[6 @ DeployableInvPack] = 0; 
	$TeamItemCount[7 @ DeployableInvPack] = 0; 
}

 //-=-=-=-=-=-=-=- Pack =-=-=-=-=-=-=-

ItemImageData DeployableInvPackImage 
{ 
	shapeFile = "invent_remote"; 
	mountPoint = 2; 
	mountOffset = { 0, -0.12, -0.3 }; 
	mountRotation = { 0, 0, 0 }; 
	mass = 2.5; 
	firstPerson = false; 
}; 

ItemData DeployableInvPack 
{ 
	description = "Inventory Station"; 
	shapeFile = "invent_remote"; 
	className = "Backpack"; 
	heading = $InvHead[ihDOb]; 
	shadowDetailMask = 4; 
	imageType = DeployableInvPackImage; 
	mass = 2.0; 
	elasticity = 0.2; 
	price = 125; 
	hudIcon = "deployable"; 
	showWeaponBar = true; 
	hiliteOnActive = true; 
}; 

function DeployableInvPack::onUse(%player,%item) 
{ 
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
		Player::mountItem(%player,%item,$BackpackSlot); 
	else 
		Player::deployItem(%player,%item);
} 

function DeployableInvPack::onDeploy(%player,%item,%pos) 
{ 
	if (DeployableInvPack::deployShape(%player,%item)) 
		Player::decItemCount(%player,%item);
} 

function DeployableInvPack::deployShape(%player,%item) 
{ 
	%client = Player::getClient(%player); 
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]) 
	{ Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s"); return false; }

	if (!GameBase::getLOSInfo(%player,3)) 
	{ Client::sendMessage(%client,0,"Deploy position out of range"); return false; }

	%obj = getObjectType($los::object); 
	if (%obj != "SimTerrain" && %obj != "InteriorShape" && %obj != "DeployablePlatform") 
	{ Client::sendMessage(%client,0,"Can only deploy on terrain or buildings"); return false; }

	if (Vector::dot($los::normal,"0 0 1") <= 0.7) 
	{ Client::sendMessage(%client,0,"Can only deploy on flat surfaces"); return false; }

	if (!checkDeployArea(%client,$los::position)) 
		return false;

	 //
	 // Passed validation, create the object
	 //
	%inv = newObject("ammounit_remote","StaticShape","DeployableInvStation",true); 
	addToSet("MissionCleanup", %inv); 
	%rot = GameBase::getRotation(%player); 
	GameBase::setTeam(%inv,GameBase::getTeam(%player)); 
	GameBase::setPosition(%inv,$los::position); 
	GameBase::setRotation(%inv,%rot); 
	Gamebase::setMapName(%inv,"Inventory Station"); 
	Client::sendMessage(%client,0,"Inventory Station deployed"); 
	playSound(SoundPickupBackpack,$los::position); 
	$TeamItemCount[GameBase::getTeam(%inv) @ "DeployableInvPack"]++; 
//	reportDeploy(%inv, %client);
	echo("MSG: ",%client," deployed a Remote Inventory Unit");
	return true; 
} 

 //-=-=-=-=-=-=-=- Object =-=-=-=-=-=-=-

StaticShapeData DeployableInvStation 
{ 
	description = "Inventory Unit"; 
	shapeFile = "invent_remote"; 
	className = "DeployableStation"; 
	maxDamage = 1.25; 
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor }; 
	sequenceSound[1] = { "use", SoundUseAmmoStation }; 
	sequenceSound[2] = { "power", SoundInventoryStationPower }; 
	visibleToSensor = true; 
	shadowDetailMask = 4; 
	castLOS = true; 
	supression = false; 
	supressable = false; 
	mapFilter = 4; 
	mapIcon = "M_station"; 
	debrisId = flashDebrisMedium; 
	damageSkinData = "objectDamageSkins"; 
	explosionId = flashExpSmall; 
}; 

function DeployableInvStation::onAdd(%this) 
{ 
	schedule("DeployableStation::deploy(" @ %this @ ");",1,%this); 
	if (GameBase::getMapName(%this) == "") 
		GameBase::setMapName (%this, "R-Inv Station"); 
	%this.Energy = $RemoteInvEnergy; 
} 

function DeployableInvStation::onActivate(%this) 
{ 
	if(%this.deployed == 1) 
	{ 
		GameBase::playSequence(%this,1,"use"); 
		InventoryStation::onResupply(%this,"RemoteInvList"); 
		%this.lastPlayer = Station::getTarget(%this);
	} 
	else 
		GameBase::setActive(%this,false); 
} 
