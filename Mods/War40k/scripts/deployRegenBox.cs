//Nano-Generator
// In development, by Draconus

$TeamItemMax[RegenBoxPack] = 1;
$InvList[RegenBoxPack] = 1;
$RemoteInvList[RegenBoxPack] = 1;
$CanAlwaysTeamDestroy = 1;

function deployRegenBox::Initialize()
{
		$TeamItemCount[1 @ RegenBoxPack] = 0;
		$TeamItemCount[2 @ RegenBoxPack] = 0;
		$TeamItemCount[3 @ RegenBoxPack] = 0;
		$TeamItemCount[4 @ RegenBoxPack] = 0;
		$TeamItemCount[5 @ RegenBoxPack] = 0;
		$TeamItemCount[6 @ RegenBoxPack] = 0;
		$TeamItemCount[7 @ RegenBoxPack] = 0;
}


ItemImageData RegenBoxPackImage
{
	shapeFile = "generator";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData RegenBoxPack
{
	description = "ReGenerator";
	shapeFile = "generator";
	className = "Backpack";
   heading = $InvHead[ihDOb];
	imageType = RegenBoxPackImage;
	shadowDetailMask = 4;
	mass = 2.5;
	elasticity = 0.2;
	price = 140;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};


function RegenBoxPack::onUse(%player,%item)
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

function RegenBoxPack::onDeploy(%player,%item,%pos)
{
	if (RegenBoxPack::deployShape(%player,%item))
	{
		Player::decItemCount(%player,%item);
	}
}

function RegenBoxPack::deployShape(%player,%item)
{
	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item])
	{
		if (GameBase::getLOSInfo(%player,3))
		{
			%obj = getObjectType($los::object);
			if (%obj == "InteriorShape")
			{
				if (Vector::dot($los::normal,"0 0 1") > 0.7)
				{
					if(checkDeployArea(%client,Vector::add($los::position, "0.2 0.2 0")))
					{
						%rot = GameBase::getRotation(%player); 
						%turret = newObject("RegenBox","Turret",RegenBox,true);
                 				addToSet("MissionCleanup", %turret);
						GameBase::setTeam(%turret,GameBase::getTeam(%player));
						GameBase::setPosition(%turret,$los::position);
						GameBase::setRotation(%turret,%rot);
						Gamebase::setMapName(%turret,"ReGenerator " @ Client::getName(%client));
						Client::sendMessage(%client,0,"ReGenerator deployed");
						playSound(SoundPickupBackpack,$los::position);
						$TeamItemCount[GameBase::getTeam(%player) @ "RegenBoxPack"]++;

   				echo("MSG: ",%client," deployed an ReGenerator
						//	Remote turrets - kill points to player that deploy them
						// Client::setOwnedObject(%client, %turret); 
						// Client::setOwnedObject(%client, %player);
						if(Player::getMountedItem(%player, $BackpackSlot) == SCVPack)
							GameBase::setDamageLevel(%turret, 0.7 * RegenBox.maxDamage);

						return true;
					}
				}
				else 
					Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
			}
			else 
				Client::sendMessage(%client,0,"Can only deploy in buildings");
		}
		else 
			Client::sendMessage(%client,0,"Deploy position out of range");
	}
	else																						  
	 	Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "es");

	return false;
}

		

TurretData RegenBox
{
	className = "Turret";
	shapeFile = "generator";
//	projectileType = none;
	maxDamage = 2;
	maxEnergy = 0;
//	minGunEnergy = 6;
//	maxGunEnergy = 5;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
//	reloadDelay = 0.4;
//	speed = 4.0;
//	speedModifier = 1.5;
//	range = 10;
	visibleToSensor = true;
	shadowDetailMask = 4;
	supressable = true;
	pinger = false;
	dopplerVelocity = 0;
	castLOS = true;
	supression = true;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = flashDebrisMedium;
	shieldShapeName = "shield";
//	fireSound = SoundFireMortar;
	activationSound = SoundRemoteTurretOn;
	deactivateSound = SoundRemoteTurretOff;
	explosionId = flashExpMedium;
	description = "ReGenerator TM";
	damageSkinData = "objectDamageSkins";
};

function RegenBox::onAdd(%this)
{
	schedule("RegenBox::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,5);
	if (GameBase::getMapName(%this) == "") {
		GameBase::setMapName (%this, "ReGenerator");
	}
}

function RegenBox::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function RegenBox::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function RegenBox::onDisabled(%this)
{
	Turret::onDisabled(%this);

	%num = Group::objectCount(%this.set);
	for(%i=%num-1; %i >= 0; %i--)
	{
		%obj = Group::getObject(%this.set, %i);
		%obj. repairRate = 0;
	}
	deleteObject(%this.set);
}
function Regenbox::onDestroyed(%this)
{
	Turret::onDestroyed(%this);
  	$TeamItemCount[GameBase::getTeam(%this) @ "RegenBoxPack"]--;
}

// Override base class just in case.
function RegenBox::onPower(%this,%power,%generator) {}
function REgenBox::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);

	%Set = newObject("set",SimSet); 
	%Pos = GameBase::getPosition(%this); 
	%Mask = $SimPlayerObjectType|$StaticObjectType|$VehicleObjectType|$MineObjectType|$SimInteriorObjectType; // people, thiings, vehicles, mines, and the base itself
	containerBoxFillSet(%Set, %Mask, %Pos, 100, 100, 25,0);
	%num = Group::objectCount(%Set);
	for(%i; %i < %num; %i++)
	{
		%obj = Group::getObject(%Set, %i);
		if(GameBase::getTeam(%obj) != GameBase::getTeam(%this) || %obj == %this)
		{
			//don't heal enemies or the box itself
		}
		else
		{
					%obj.repairRate = 0.05;
		}
	}

	%this.set = %Set;

	schedule("REgenBox::checkRegenBox(" @ %this @ ");", 0.1, %this);

}	

function RegenBox::checkRegenBox(%this)
{

	if(GameBase::getDamageState(%this) != "Enabled")
		return;

	%this.evenodd = !%this.evenodd; //switches from 1 to 0... tells every other check... used to check if in both new & old sets

	%Set = newObject("set",SimSet); 
	%Pos = GameBase::getPosition(%this); 
	%Mask = $SimPlayerObjectType|$StaticObjectType|$VehicleObjectType|$MineObjectType|$SimInteriorObjectType; //heals people, thiings, vehicles, mines, and the base itself
	containerBoxFillSet(%Set, %Mask, %Pos, 100, 100, 25,0);
	%num = Group::objectCount(%Set);
for(%i; %i < %num; %i++)
	{
		%obj = Group::getObject(%Set, %i);
		if(GameBase::getTeam(%obj) != GameBase::getTeam(%this) || %obj == %this)
		{
			//don't heal enemies or the box itself
		}
		else
		{
			%obj.repairRate = 0.05 + %this.evenodd; //1 half the time & 2 other half... used to check if in this set while searching the old set
		}
	}


	%num = Group::objectCount(%this.set);

	for(%j; %j < %num; %j++)
	{
		%obj = Group::getObject(%this.set, %j);
		if(%obj == %this || GameBase::getTeam(%obj) != GameBase::getTeam(%this))
		{
			//don't bother checking the other team or the box itself; they're not cloaked
		}
		else if(%obj.repairRate != (%this.evenodd + 1)) //if different then new set
		{
			%obj. repairRate = 0;
		}
	}

	deleteObject(%this.set); //delete the old set
	%this.set = %Set; //and replace with new set

	schedule("RegenBox::checkRegenBox(" @ %this @ ");", 6.0, %this); //then recheck in 10 seconds
}
