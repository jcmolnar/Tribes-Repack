//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Adv. Motion sensor
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$TeamItemMax[LRMotionSensorPack] = 3;
$InvList[LRMotionSensorPack] = 1;
$RemoteInvList[LRMotionSensorPack] = 1;

$CanAlwaysTeamDestroy[DeployableLRMotionSensor] = 1;

function deployLRMotionSensorPack::Initialize()
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

ItemImageData LRMotionSensorPackImage 
{
  shapeFile = "sensor_small";
  mountPoint = 2;
  mountOffset = { 0, 0, 0.1 };
  mountRotation = { 1.57, 0, 0 };
  firstPerson = false;
};

ItemData LRMotionSensorPack 
{
  description = "Adv. Motion Sensor";
  shapeFile = "sensor_small";
  className = "Backpack";
  heading = $InvHead[ihDSe];
  imageType = LRMotionSensorPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 250;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function LRMotionSensorPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function LRMotionSensorPack::onDeploy(%player,%item,%pos) 
{
  if (LRMotionSensorPack::deployShape(%player,%item)) 
  {
    Player::decItemCount(%player,%item);
    $TeamItemCount[GameBase::getTeam(%player) @ "LRMotionSensorPack"]++;
  }
}

function LRMotionSensorPack::deployShape(%player,%item) 
{
  %client = Player::getClient(%player);
  if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]) 
  {
    if (GameBase::getLOSInfo(%player,3)) 
    {
      %obj = getObjectType($los::object);
      if (%obj == "SimTerrain" || %obj == "InteriorShape" || %obj == "DeployablePlatform") 
      {
        %prot = GameBase::getRotation(%player);
        %zRot = getWord(%prot,2);
        if (Vector::dot($los::normal,"0 0 1") > 0.6) 
          %rot = "0 0 " @ %zRot;
        else 
        {
          if (Vector::dot($los::normal,"0 0 -1") > 0.6) 
            %rot = "3.14159 0 " @ %zRot;
          else 
            %rot = Vector::getRotation($los::normal);
        }
        if(checkDeployArea(%client,$los::position)) 
        {
          %mSensor = newObject("","Sensor",DeployableLRMotionSensor,true);
          addToSet("MissionCleanup", %mSensor);
          GameBase::setTeam(%mSensor,GameBase::getTeam(%player));
          GameBase::setRotation(%mSensor,%rot);
          GameBase::setPosition(%mSensor,$los::position);
          Gamebase::setMapName(%mSensor,"Adv. Motion Sensor");
          Client::sendMessage(%client,0,"Adv. Motion Sensor deployed");
          playSound(SoundPickupBackpack,$los::position);
//          reportDeploy(%mSensor, %client);
		echo("MSG: ",%client," deployed an Adv. Motion Sensor");
          return true;
        }
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

SensorData DeployableLRMotionSensor
{
   description = "Adv. Motion Sensor";
	className = "DeployableSensor";
	shapeFile = "sensor_small";
	shadowDetailMask = 16;
	visibleToSensor = true;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
//   explosionId = DebrisExp;
	damageLevel = {0.8, 1.0};
	maxDamage = 0.4;
	debrisId = defaultDebrisSmall;
	range = 250;
	dopplerVelocity = 1;
   castLOS = false;
   supression = false;
	supressable = false;
	pinger = false;
	mapFilter = 4;
	mapIcon = "M_motionSensor";
	damageSkinData = "objectDamageSkins";
};
