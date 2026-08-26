//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Adv. Motion sensor
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$TeamItemMax[LRMotionSensorPack] = 2;
$InvList[LRMotionSensorPack] = 1;
$RemoteInvList[LRMotionSensorPack] = 1;

$CanAlwaysTeamDestroy[LRMotionSensorPack] = 1;

 //-=-=-=-

function deployLRMotion::Initialize()
{
  $TeamItemCount[0 @ LRMotionSensorPack] = 0;
  $TeamItemCount[1 @ LRMotionSensorPack] = 0;
  $TeamItemCount[2 @ LRMotionSensorPack] = 0;
  $TeamItemCount[3 @ LRMotionSensorPack] = 0;
  $TeamItemCount[4 @ LRMotionSensorPack] = 0;
  $TeamItemCount[5 @ LRMotionSensorPack] = 0;
  $TeamItemCount[6 @ LRMotionSensorPack] = 0;
  $TeamItemCount[7 @ LRMotionSensorPack] = 0;
}


// Long Range Motion Sensor
ItemImageData LRMotionSensorPackImage
{
	shapeFile = "sensorjampack";
	mountPoint = 2;
  	mountOffset = { 0, -0.07, 0 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData LRMotionSensorPack
{
	description = "Adv. Motion Sensor";
	shapeFile = "sensor_pulse_med";
	className = "Backpack";
	heading = $InvHead[ihDSe];
	shadowDetailMask = 4	;
	imageType = LRMotionSensorPackImage;
	mass = 2.5;
	elasticity = 0.2;
	price = 150;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};


function LRMotionSensorPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function LRMotionSensorPack::onDeploy(%player,%item,%pos)
{
	if (LRMotionSensorPack::deployShape(%player,%item)) {
		Player::decItemCount(%player,%item);
	}
}	

function LRMotionSensorPack::deployShape(%player,%item)
{
	%client = Player::getClient(%player);

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]) {
		if (GameBase::getLOSInfo(%player,3)) {
			%obj = getObjectType($los::object);
			if (%obj == "SimTerrain" || %obj == "InteriorShape") {
	    		if (Vector::dot($los::normal,"0 0 1") > 0.7) {
					if(checkDeployArea(%client,$los::position)) {
						%rot = GameBase::getRotation(%player); 
						%DTPad = newObject("","Sensor",LongRangeMotionSensor,true);
						addToSet("MissionCleanup", %DTPad);
						GameBase::setTeam(%DTPad,GameBase::getTeam(%player));
						GameBase::setPosition(%DTPad,Vector::add($los::position, "0 0 -0.05"));
						GameBase::setRotation(%DTPad, %rot);
						Gamebase::setMapName(%DTPad,"Adv. Motion Sensor");
						Client::sendMessage(%client,0,"Adv. Motion Sensor deployed");
						playSound(SoundPickupBackpack,$los::position);
						$TeamItemCount[GameBase::getTeam(%player) @ "DeployableLRMotionSensorPack"]++;
						echo("MSG: ",%client," deployed a Adv. Motion Sensor");
						Client::setOwnedObject(%client, %DTPad);
						Client::setOwnedObject(%client, %player);
						return true;
					}
				}
				else 
					Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
			}
			else 
				Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");
		}
		else 
			Client::sendMessage(%client,0,"Deploy position out of range");
	}
	else																						  
	 	Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");

	return false;
}
