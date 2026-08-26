
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Small Ammo Station (deployable)
//  By Dynamix
//  Cleaned up by Alazane (alazane@rkeng.com)
//
//  Installation:
//
//**Add the line 
//    exec(deploySmallAmmoStation);
//  in the file "server.cs" in the procedure 
//  "createServer" (just look for the other "exec"s--
//  must at least be before call to 
//  "preloadServerDataBlocks").
//  
//**Add the line 
//    deploySmallAmmoStation::Initialize();
//  also in the file "server.cs" in the procedure 
//  "Server::finishMissionLoad" right after
//  "Mission::reinitData".
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$TeamItemMax[DeployableAmmoPack] = 10; 
$InvList[DeployableAmmoPack] = 1; 

$CanAlwaysTeamDestroy[DeployableAmmoStation] = 1;

 //-=-=-=-=-=-=-=- Initialize =-=-=-=-=-=-=-

function deploySmallAmmoStation::Initialize()
{
	$TeamItemCount[0 @ DeployableAmmoPack] = 0; 
	$TeamItemCount[1 @ DeployableAmmoPack] = 0; 
	$TeamItemCount[2 @ DeployableAmmoPack] = 0; 
	$TeamItemCount[3 @ DeployableAmmoPack] = 0; 
	$TeamItemCount[4 @ DeployableAmmoPack] = 0; 
	$TeamItemCount[5 @ DeployableAmmoPack] = 0; 
	$TeamItemCount[6 @ DeployableAmmoPack] = 0; 
	$TeamItemCount[7 @ DeployableAmmoPack] = 0; 
}

 //-=-=-=-=-=-=-=- Pack =-=-=-=-=-=-=-
 
ItemImageData DeployableAmmoPackImage 
{ 
	shapeFile = "ammounit_remote"; 
	mountPoint = 2; 
	mountOffset = { 0, -0.1, -0.3 }; 
	mountRotation = { 0, 0, 0 }; 
	mass = 1.0; 
	firstPerson = false; 
}; 

ItemData DeployableAmmoPack 
{ 
	description = "Ammo Station"; 
	shapeFile = "ammounit_remote"; 
	className = "Backpack"; 
	heading = $InvHead[ihDOb]; 
	shadowDetailMask = 4; 
	imageType = DeployableAmmoPackImage; 
	mass = 2.0; 
	elasticity = 0.2; 
	price = $RemoteAmmoEnergy; 
	hudIcon = "deployable"; 
	showWeaponBar = true; 
	hiliteOnActive = true; 
}; 

function DeployableAmmoPack::onUse(%player,%item) 
{ 
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
		Player::mountItem(%player,%item,$BackpackSlot); 
	else 
		Player::deployItem(%player,%item);
}

function DeployableAmmoPack::onDeploy(%player,%item,%pos) 
{ 
	if (DeployableAmmoPack::deployShape(%player,%item)) 
		Player::decItemCount(%player,%item);
} 

function DeployableAmmoPack::deployShape(%player,%item) 
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
	%inv = newObject("ammounit_remote","StaticShape","DeployableAmmoStation",true); 
	addToSet("MissionCleanup", %inv); 
	%rot = GameBase::getRotation(%player); 
	GameBase::setTeam(%inv,GameBase::getTeam(%player)); 
	GameBase::setPosition(%inv,$los::position); 
	GameBase::setRotation(%inv,%rot); 
	Gamebase::setMapName(%inv,"Ammo Station"); 
	Client::sendMessage(%client,0,"Ammo Station deployed"); 
	playSound(SoundPickupBackpack,$los::position); 
	$TeamItemCount[GameBase::getTeam(%inv) @ "DeployableAmmoPack"]++; 
//	reportDeploy(%inv, %client);
		echo("MSG: ",%client," deployed a Remote Ammo Unit");
	return true; 
} 

 //-=-=-=-=-=-=-=- Object =-=-=-=-=-=-=-

StaticShapeData DeployableAmmoStation 
{ 
	description = "Remote Ammo Unit"; 
	shapeFile = "ammounit_remote"; 
	className = "DeployableStation"; 
	maxDamage = 0.25; 
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor }; 
	sequenceSound[1] = { "use", SoundUseAmmoStation }; 
	sequenceSound[2] = { "power", SoundAmmoStationPower }; 
	visibleToSensor = true; 
	shadowDetailMask = 4; 
	castLOS = true; 
	supression = false; 
	supressable = false; 
	mapFilter = 4; 
	mapIcon = "M_station"; 
	debrisId = flashDebrisSmall; 
	damageSkinData = "objectDamageSkins"; 
	explosionId = flashExpMedium; 
}; 

function DeployableAmmoStation::onAdd(%this) 
{ 
	schedule("DeployableStation::deploy(" @ %this @ ");",1,%this); 
	if (GameBase::getMapName(%this) == "") 
		GameBase::setMapName (%this, "R-Ammo Station"); 
	%this.Energy = $RemoteAmmoEnergy; 
} 

function DeployableAmmoStation::onActivate(%this) 
{ 
	if(%this.deployed == 1) 
	{ 
		GameBase::playSequence(%this,1,"use"); 
		schedule("AmmoStation::onResupply(" @ %this @ ");",0.5,%this); 
		%this.lastPlayer = Station::getTarget(%this);
	} 
	else 
		GameBase::setActive(%this,false); 
} 
