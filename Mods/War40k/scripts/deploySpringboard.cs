
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Springboard
//  By Alazane & Mjolnir
//  alazane@rkeng.com
//
//  Installation:
//
//**Add the line 
//    exec(deploySpringboard);
//  in the file "server.cs" in the procedure 
//  "createServer" (just look for the other "exec"s--
//  must at least be before call to 
//  "preloadServerDataBlocks").
//  
//**Add the line 
//    deploySpringboard::Initialize();
//  also in the file "server.cs" in the procedure 
//  "Server::finishMissionLoad" right after
//  "Mission::reinitData".
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

 // Adjust this to adjust the number a team can have
$TeamItemMax[Springboard] = 3;
$InvList[SpringPack] = 3;
$RemoteInvList[SpringPack] = 3;

$CanAlwaysTeamDestroy[Springboard] = 1;

 //-=-=-=-=-=-=-=- Initialize =-=-=-=-=-=-=-

function deploySpringboard::Initialize()
{
	$TeamItemCount[0 @ "Springboard"] = 0;
	$TeamItemCount[1 @ "Springboard"] = 0;
	$TeamItemCount[2 @ "Springboard"] = 0;
	$TeamItemCount[3 @ "Springboard"] = 0;
	$TeamItemCount[4 @ "Springboard"] = 0;
	$TeamItemCount[5 @ "Springboard"] = 0;
	$TeamItemCount[6 @ "Springboard"] = 0;
	$TeamItemCount[7 @ "Springboard"] = 0;
}

 //-=-=-=-=-=-=-=- Pack =-=-=-=-=-=-=-

ItemImageData SpringPackImage
{
	shapeFile = "ammopack";
	mountPoint = 2;
	mountOffset = { 0, 0, 0.1 };
	mountRotation = { 1.57, 0, 0 };
	firstPerson = false;
};

ItemData SpringPack
{
	description = "Launch Pad";
	shapeFile = "ammopack";
	className = "Backpack";
	heading = $InvHead[ihDOb];
	imageType = SpringPackImage;
	shadowDetailMask = 4;
	mass = 1.5;
	elasticity = 0.2;
	price = 5;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function SpringPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function SpringPack::onDeploy(%player,%item,%pos)
{
	if (SpringPack::deployShape(%player,"Launch Pad (" @ Client::getName(Player::getClient(%player)) @ ")", SpringBoard, %item, $TurretLocAnywhere))
Player::decItemCount(%player,%item);
}

function SpringPack::deployShape(%player,%item)
{
	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ "Springboard"] >= $TeamItemMax[Springboard]) 
	{ Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s"); return false; }

	if (!GameBase::getLOSInfo(%player,3)) 
	{ Client::sendMessage(%client,0,"Deploy position out of range"); return false; }

	 //
	 // Passed validation, create the object
	 //
	%rot = GameBase::getRotation(%player);
	%objSpringboard = newObject("Springboard", "StaticShape", Springboard, true);
	addToSet("MissionCleanup", %objSpringboard);
	GameBase::setTeam(%objSpringboard, GameBase::getTeam(%player));
	GameBase::setPosition(%objSpringboard, $los::position);
	GameBase::setRotation(%objSpringboard, %rot);
	Gamebase::setMapName(%objSpringboard, "Launch Pad");
	Client::sendMessage(%client,0,"Launch Pad Deployed");
	GameBase::startFadeIn(%objSpringboard);
	playSound(SoundPickupBackpack, $los::position);
	$TeamItemCount[GameBase::getTeam(%player) @ "Springboard"]++;
//	reportDeploy(%objSpringboard, %client);
		echo("MSG: ",%client," deployed a Launch Pad");
	return true;
}

 //-=-=-=-=-=-=-=- Object =-=-=-=-=-=-=-

StaticShapeData Springboard
{
	shapeFile = "flagstand";
	debrisId = defaultDebrisSmall;
	maxDamage = 2.00;
	isTranslucent = true;
   	description = "Deployable Spring";
	visibleToSensor = true;
};

function Springboard::onDestroyed(%this)
{
	StaticShape::objectiveDestroyed(%this);
	$TeamItemCount[GameBase::getTeam(%this) @ "Springboard"]--;
}

function Springboard::onCollision(%this,%obj)
{
	%c = Player::getClient(%obj);
	%vecVelocity = Item::getVelocity(%obj);
	%rnd = floor(getRandom() * 55);

	 // Check misfires
	if (%rnd == 1)
	{
		GameBase::playSound(%this, debrisLargeExplosion, 0);
		Client::SendMessage(%c, 0, "Launching...");		
		%HMult = 2;
		%ZMax = 50;

		%rnd = floor(getRandom() * 3); 
		if (%rnd == 0) 
		{ MessageAll(0,strcat(Client::getName(%c), " suffers a Launch malfunction.")); } 
		else if (%rnd == 1) 
		{ MessageAll(0,strcat(Client::getName(%c), " hits orbit.")); } 
		else if (%rnd == 2) 
		{ MessageAll(0,strcat(Client::getName(%c), " falls off the edge of the planet.")); }
	}
	else if (%rnd > 45)  // 46-54
	{
		GameBase::playSound(%this, debrisLargeExplosion, 0);
		Client::SendMessage(%c, 0, "Launching...");
		%HMult = 2;
		%ZMax = 45;
	}
	else
	{
		GameBase::playSound(%this, SoundFireMortar, 0);
		Client::SendMessage(%c, 0, "Launching...");
		%HMult = 2;
		%ZMax = 45;
	}
	%vecNewVelocity = GetWord(%vecVelocity, 0) * %HMult @ " " @ 
	                  GetWord(%vecVelocity, 1) * %HMult @ " " @
	                  %ZMax;
	Item::setVelocity(%obj, %vecNewVelocity);
}

