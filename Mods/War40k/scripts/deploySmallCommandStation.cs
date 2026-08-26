
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Small Command Station (deployable)
//  By Unknown
//  Cleaned up by Alazane (alazane@rkeng.com)
//
//  Installation:
//
//**Add the line 
//    exec(deploySmallCommandStation);
//  in the file "server.cs" in the procedure 
//  "createServer" (just look for the other "exec"s--
//  must at least be before call to 
//  "preloadServerDataBlocks").
//  
//**Add the line 
//    deploySmallCommandStation::Initialize();
//  also in the file "server.cs" in the procedure 
//  "Server::finishMissionLoad" right after
//  "Mission::reinitData".
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$TeamItemMax[DeployableComPack] = 5; 
$InvList[DeployableComPack] = 1; 
$RemoteInvList[DeployableComPack] = 1; 

$CanAlwaysTeamDestroy[DeployableComStation] = 1;

 //-=-=-=-=-=-=-=- Initialize =-=-=-=-=-=-=-

function deploySmallCommandStation::Initialize()
{
	$TeamItemCount[0 @ DeployableComPack] = 0; 
	$TeamItemCount[1 @ DeployableComPack] = 0; 
	$TeamItemCount[2 @ DeployableComPack] = 0; 
	$TeamItemCount[3 @ DeployableComPack] = 0; 
	$TeamItemCount[4 @ DeployableComPack] = 0; 
	$TeamItemCount[5 @ DeployableComPack] = 0; 
	$TeamItemCount[6 @ DeployableComPack] = 0; 
	$TeamItemCount[7 @ DeployableComPack] = 0; 
}

 //-=-=-=-=-=-=-=- Pack =-=-=-=-=-=-=-

ItemImageData DeployableComPackImage 
{ 
	shapeFile = "ammounit_remote"; 
	mountPoint = 2; 
	mountOffset = { 0, -0.12, -0.3 }; 
	mountRotation = { 0, 0, 0 }; 
	mass = 4.5; 
	firstPerson = false; 
}; 

ItemData DeployableComPack 
{ 
	description = "Command Station"; 
	shapeFile = "ammounit_remote"; 
	className = "Backpack"; 
	heading = $InvHead[ihDOb]; 
	shadowDetailMask = 4; 
	imageType = DeployableComPackImage; 
	mass = 4.0; 
	elasticity = 0.2; 
	price = $RemoteComEnergy + 200; 
	hudIcon = "deployable"; 
	showWeaponBar = true; 
	hiliteOnActive = true; 
}; 

function DeployableComPack::onUse(%player,%item) 
{ 
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
		Player::mountItem(%player,%item,$BackpackSlot); 
	else 
		Player::deployItem(%player,%item); 
}

function DeployableComPack::onDeploy(%player,%item,%pos) 
{ 
	if (DeployableComPack::deployShape(%player,%item)) 
		Player::decItemCount(%player,%item); 
}

function DeployableComPack::deployShape(%player,%item) 
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
	%inv = newObject("comunit_remote","StaticShape","DeployableComStation",true); 
	addToSet("MissionCleanup", %inv); 
	%rot = GameBase::getRotation(%player); 
	GameBase::setTeam(%inv,GameBase::getTeam(%player)); 
	GameBase::setPosition(%inv,$los::position); 
	GameBase::setRotation(%inv,%rot); 
	Gamebase::setMapName(%inv,"Command Station"); 
	Client::sendMessage(%client,0,"Command Station deployed"); 
	playSound(SoundPickupBackpack,$los::position); 
	$TeamItemCount[GameBase::getTeam(%inv) @ "DeployableComPack"]++; 
//	reportDeploy(%inv, %client);
	echo("MSG: ",%client," deployed a Remote Command Station");
	return true; 
} 

 //-=-=-=-=-=-=-=- Object =-=-=-=-=-=-=-

StaticShapeData DeployableComStation 
{ 
	description = "Remote Command Station"; 
	shapeFile = "cmdpnl"; 
	className = "DeployableStation"; 
	visibleToSensor = true; 
	sequenceSound[0] = { "activate", SoundActivateCommandStation }; 
	sequenceSound[1] = { "power", SoundCommandStationPower }; 
	sequenceSound[2] = { "use", SoundUseCommandStation }; 
	maxDamage = 1.0; 
	debrisId = flashDebrisMedium; 
	mapFilter = 4; 
	mapIcon = "M_station"; 
	damageSkinData = "objectDamageSkins"; 
	shadowDetailMask = 16; 
	triggerRadius = 1.5; 
	castLOS = true; 
	supression = false; 
	supressable = false; 
	explosionId = flashExpLarge; 
}; 

function DeployableComStation::onAdd(%this) 
{ 
	schedule("DeployableStation::deploy(" @ %this @ ");",1,%this); 
	if (GameBase::getMapName(%this) == "") 
		GameBase::setMapName (%this, "R-Com Station"); 
	%this.Energy = 3000; 
} 

function DeployableComStation::OnActivate(%this) 
{ 
	if (GameBase::isActive(%this)) 
	{ 
		%player = Station::getTarget(%this); 
		if (%player != -1 && %this.lastPlayer = %player) 
		{ 
			%client = Player::getClient(%player); 
			if (%this.target != %client) 
			{ 
				%this.target = %client; 
				%player.CommandTag = 1; 
				Client::setGuiMode(%client,2); 
				Client::sendMessage(%client,0,"Command Access On"); 
			} 
			schedule("DeployableComStation::onActivate(" @ %this @ ");",0.5,%this); 
			return; 
		} 
		GameBase::setActive(%this,false); 
	} 

	if (%this.target) 
	{ 
		Client::sendMessage(%this.target,0,"Command Access Off"); 
		%this.target.CommandTag = 0; 
	} 

	(Client::getOwnedObject(%this.target)).Station = ""; 
	%this.target = ""; 
} 
