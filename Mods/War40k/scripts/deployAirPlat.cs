//Air Platform Pack: Original, but appears in other mods too.
$TeamItemMax[AirPlatformPack] = 20; 
$TeamItemMax[LargeAirPlatPack] = 10; 

$InvList[AirPlatformPack] = 1; 
$RemoteInvList[AirPlatformPack] = 1; 

$InvList[LargeAirPlatPack] = 1; 
$RemoteInvList[LargeAirPlatPack] = 1; 

$CanAlwaysTeamDestroy[AirPlatformPack] = 1;
$CanAlwaysTeamDestroy[LargeAirPlatPack] = 1;

 //-=-=-=-=-=-=-=- Initialize =-=-=-=-=-=-=-

function deployAirPlat::Initialize()
{
	$TeamItemCount[0 @ AirPlatformPack] = 0; 
	$TeamItemCount[1 @ AirPlatformPack] = 0; 
	$TeamItemCount[2 @ AirPlatformPack] = 0; 
	$TeamItemCount[3 @ AirPlatformPack] = 0; 
	$TeamItemCount[4 @ AirPlatformPack] = 0; 
	$TeamItemCount[5 @ AirPlatformPack] = 0; 
	$TeamItemCount[6 @ AirPlatformPack] = 0; 
	$TeamItemCount[7 @ AirPlatformPack] = 0; 

	$TeamItemCount[0 @ LargeAirPlatPack] = 0;
	$TeamItemCount[1 @ LargeAirPlatPack] = 0;
	$TeamItemCount[2 @ LargeAirPlatPack] = 0;
	$TeamItemCount[3 @ LargeAirPlatPack] = 0;
	$TeamItemCount[4 @ LargeAirPlatPack] = 0;
	$TeamItemCount[5 @ LargeAirPlatPack] = 0;
	$TeamItemCount[6 @ LargeAirPlatPack] = 0;
	$TeamItemCount[7 @ LargeAirPlatPack] = 0;
}





ItemImageData AirPlatformPackImage
{
	shapeFile = "ammopack";
	mountPoint = 2;
	mountOffset = { 0, 0, 0 };
	mountRotation = { 0, 0, 0 };
	firstPerson = false;
};

ItemData AirPlatformPack
{	
	description = "Grav Platform";
	shapefile = "ammopack";
	classname = "Backpack";
	heading = $InvHead[ihDOb];
	imageType = AirPlatformPackImage;
	shadowDetailMask = 4;
	mass = 1.0;
	elasticity = 0.1;
	price = 3;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function AirPlatformPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
	{
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else 
	{
		Player::deployItem(%player,%item);
	}
}

function AirPlatformPack::onDeploy(%player,%item,%pos)
{
	if (AirPlatformPack::deployShape(%player,%item)) 
	{
		Player::decItemCount(%player,%item);
	}
}

function AirPlatformPack::deployshape(%player,%item)
{
	GameBase::getLOSInfo(%player,6);
	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]) 
	{
		%playerPos = GameBase::getPosition(%player);
		%flag = $teamFlag[GameBase::getTeam(%player)];
		%flagpos = gamebase::getPosition(%flag);
		if(Vector::getDistance(%flagpos, %playerpos) > 10)
		{
		%camera = newObject("Air Platform","Staticshape",AirPlatform,true); 
	   	addToSet("MissionCleanup", %camera);				      
		GameBase::setTeam(%camera,GameBase::getTeam(%player));
		GameBase::setRotation(%camera,"0 0 0");// %rot
		GameBase::setPosition(%camera,GameBase::getPosition(%player));
		Gamebase::setMapName(%camera,"Air Platform#" @ $totalNumPlatforms++ @ " " @  Client::getName(%client));
		Client::sendMessage(%client,0,"Air Platform deployed");
		playSound(SoundPickupBackpack,$los::position);
		$TeamItemCount[GameBase::getTeam(%camera) @ "AirPlatformPack"]++;
		echo("MSG: ",%client," deployed an Air Platform ");
		return true;
		}
		else
			Client::sendMessage(%client,0,"You are too close to your flag, cant hide the flag in this mod.");
			return false;
		}
	else																						  
	 	Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
		return false;
}

// ****

ItemImageData LargeAirPlatPackImage
{
	shapeFile = "ammopack";
	mountPoint = 2;
	mountOffset = { 0, 0, 0 };
	mountRotation = { 0, 0, 0 };
	firstPerson = false;
};

ItemData LargeAirPlatPack
{	
	description = "Adv. Grav Platform";
	shapefile = "ammopack";
	classname = "Backpack";
	heading = $InvHead[ihDOb];
	imageType = LargeAirPlatPackImage;
	shadowDetailMask = 4;
	mass = 1.0;
	elasticity = 0.1;
	price = 100;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function LargeAirPlatPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
	{
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else 
	{
		Player::deployItem(%player,%item);
	}
}

function LargeAirPlatPack::onDeploy(%player,%item,%pos)
{
	if (LargeAirPlatPack::deployShape(%player,%item)) 
	{
		Player::decItemCount(%player,%item);
	}
}

function LargeAirPlatPack::deployshape(%player,%item)// here
{
	GameBase::getLOSInfo(%player,3);
	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]) 
	{
		// if player is within 30 yards of flag cannot deploy
		%playerPos = GameBase::getPosition(%player);
		%flag = $teamFlag[GameBase::getTeam(%player)];
		%flagpos = gamebase::getPosition(%flag);
		echo("%flagpos " @ %flagpos);
		echo("%playerpos " @ %playerpos);
		if(Vector::getDistance(%flagpos, %playerpos) > 10)
		{
		%camera = newObject("Large Air Platform","StaticShape",LargeAirPlatform,true); // name when targeted,cs the shape is in,
	   	addToSet("MissionCleanup", %camera);				      // shape name in cs,
		GameBase::setTeam(%camera,GameBase::getTeam(%player));
		GameBase::setRotation(%camera,"0 0 0");// %rot
		GameBase::setPosition(%camera,GameBase::getPosition(%player));
		Gamebase::setMapName(%camera,"Large Air Platform" @  Client::getName(%client));
		Client::sendMessage(%client,0,"Large Air Platform deployed");
		playSound(SoundPickupBackpack,$los::position);
		$TeamItemCount[GameBase::getTeam(%camera) @ "LargeAirPlatPack"]++;
		echo("MSG: ",%client," deployed a Large Air Platform ");
		return true;
		}
		else
			Client::sendMessage(%client,0,"You are too close to your flag, cant hide the flag in this mod.");
			return false;
	}

}

StaticShapeData AirPlatform
{
	shapeFile = "elevator_6x6_octagon"; 
	debrisId = defaultDebrisMedium;
	maxDamage = 2.0;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	explosionId = debrisExpLarge;
	visibleToSensor = true;
	mapFilter = 4;
	mapIcon = "S_Air Plat";
      description = "Air Platform";
};

function AirPlatform::onDestroyed(%this)
{
	
   StaticShape::objectiveDestroyed(%this);
   $TeamItemCount[GameBase::getTeam(%this) @ "AirPlatformPack"]--;
}

StaticShapeData LargeAirPlatform
{
	shapeFile = "elevator16x16_octo"; 
	debrisId = defaultDebrisLarge;
	maxDamage = 15.0;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	explosionId = debrisExpLarge;
	visibleToSensor = true;
	mapFilter = 4;
      description = "Large Air Platform";
};

function LargeAirPlatform::onDestroyed(%this)
{	
   StaticShape::objectiveDestroyed(%this);
   $TeamItemCount[GameBase::getTeam(%this) @ "LargeAirPlatPack"]--;
}
