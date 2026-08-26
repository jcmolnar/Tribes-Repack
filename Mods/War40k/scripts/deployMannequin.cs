
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Deployable Mannequin
//  By Unknown (xxx@xxx.com)
//  Cleaned up by Alazane (alazane@rkeng.com)
//
//  Installation:
//
//**Add the line 
//    exec(deployMannequin);
//  in the file "server.cs" in the procedure 
//  "createServer" (just look for the other "exec"s--
//  must at least be before call to 
//  "preloadServerDataBlocks").
//  
//**Add the line 
//    deployMannequin::Initialize();
//  also in the file "server.cs" in the procedure 
//  "Server::finishMissionLoad" right after
//  "Mission::reinitData".
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$TeamItemMax[Mannequin] = 15;
$InvList[MannequinPack] = 1; 
$RemoteInvList[MannequinPack] = 1; 

$CanAlwaysTeamDestroy[Mannequin1] = 1;
$CanAlwaysTeamDestroy[Mannequin2] = 1;
$CanAlwaysTeamDestroy[Mannequin3] = 1;

 //-=-=-=-=-=-=-=- Initialize -=-=-=-=-=-=-=-

function deployMannequin::Initialize()
{
	$TeamItemCount[0 @ Mannequin] = 0;
	$TeamItemCount[1 @ Mannequin] = 0;
	$TeamItemCount[2 @ Mannequin] = 0;
	$TeamItemCount[3 @ Mannequin] = 0;
	$TeamItemCount[4 @ Mannequin] = 0;
	$TeamItemCount[5 @ Mannequin] = 0;
	$TeamItemCount[6 @ Mannequin] = 0;
	$TeamItemCount[7 @ Mannequin] = 0;
}

 //-=-=-=-=-=-=-=- Pack -=-=-=-=-=-=-=-

ItemImageData MannequinPackImage 
{
	shapeFile = "ShieldPack";
	mountPoint = 2;
	mountOffset = { 0, -0.03, 0 };
	mass = 1.0;
	firstPerson = false;  
};

ItemData MannequinPack 
{
	description = "Hologram";
	shapeFile = "larmor";
	className = "Backpack";
	heading = $InvHead[ihDOb];
	imageType = MannequinPackImage;
	shadowDetailMask = 4;
	mass = 1.5;
	elasticity = 0.2;
	price = 900;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function MannequinPack::onUse(%player,%item) 
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
		Player::mountItem(%player,%item,$BackpackSlot);
	else 
		Player::deployItem(%player,%item);
}

function MannequinPack::onDeploy(%player,%item,%pos) 
{
	if (MannequinPack::deployShape(%player,%item)) 
		Player::decItemCount(%player,%item);
}

function MannequinPack::deployShape(%player,%item) 
{
	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ "Mannequin"] >= $TeamItemMax["Mannequin"]) 
	{ Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s"); return false; }

	if (!GameBase::getLOSInfo(%player,3)) 
	{ Client::sendMessage(%client,0,"Deploy position out of range"); return false; }

	if (Vector::dot($los::normal,"0 0 1") <= 0.7) 
	{ Client::sendMessage(%client,0,"Can only deploy on flat surfaces"); return false; }

	if (!checkDeployArea(%client,$los::position)) 
		return false;

	 //
	 // Passed validation, create the object
	 //
	%rot = GameBase::getRotation(%player);

	%rnd = floor(getRandom() * 10);
	if (%rnd > 6) 
		%objMannequin = newObject("","StaticShape",Mannequin1,true);
	else if ((%rnd > 2) && (%rnd < 7)) 
		%objMannequin = newObject("","StaticShape",Mannequin2,true);
	else 
		%objMannequin = newObject("","StaticShape",Mannequin3,true);

	addToSet("MissionCleanup", %objMannequin);
	GameBase::setTeam(%objMannequin,GameBase::getTeam(%player));
	GameBase::setPosition(%objMannequin,$los::position);
	GameBase::setRotation(%objMannequin,%rot);
        Gamebase::setMapName(%objMannequin, "Mannequin");
	Client::sendMessage(%client,0,"Mannequin Deployed");
	GameBase::startFadeIn(%objMannequin);
	playSound(ForceFieldOpen,$los::position);
	$TeamItemCount[GameBase::getTeam(%player) @ "Mannequin"]++;
	//schedule("Mannequin::onMove(" @ %objMannequin @ ");", 5, %objMannequin); 
//        reportDeploy(%objMannequin, %client);
	echo("MSG: ",%client," deployed a Hologram");
	return true;
}

 //-=-=-=-=-=-=-=- Object -=-=-=-=-=-=-=-

function Mannequin::onDestroyed(%this)
{
	StaticShape::objectiveDestroyed(%this);
	$TeamItemCount[GameBase::getTeam(%this) @ "Mannequin"]--; 
}

function Mannequin::onMove(%this)
{
  // THIS DOESN'T WORK.  That's why it's not scheduled above.
messageAll(1, "Mannequin");
	GameBase::playSequence(%this, 0, "celebration 1");
	schedule("Mannequin::onMove(" @ %this @ ");", 5, %this); 
}

StaticShapeData Mannequin1 
{ 
	description = "Mannequin"; 
	shapeFile = "larmor"; 
	className = "Mannequin"; 
	debrisId = defaultDebrisSmall; 
	maxDamage = 0.75; 
}; 

StaticShapeData Mannequin2 
{ 
	description = "Mannequin"; 
	shapeFile = "marmor"; 
	className = "Mannequin"; 
	debrisId = defaultDebrisSmall; 
	maxDamage = 1.10; 
}; 

StaticShapeData Mannequin3 
{ 
	description = "Mannequin"; 
	shapeFile = "harmor"; 
	className = "Mannequin"; 
	debrisId = defaultDebrisSmall; 
	maxDamage = 1.5; 
}; 

