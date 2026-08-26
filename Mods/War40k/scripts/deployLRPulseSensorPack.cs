//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Adv. Pulse Sensor
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$TeamItemMax[LRPulseSensorPack] = 3;
$InvList[LRPulseSensorPack] = 1;
$RemoteInvList[LRPulseSensorPack] = 1;

$CanAlwaysTeamDestroy[DeployableLRPulseSensor] = 1;

function deployLRPulseSensorPack::Initialize()
{
  $TeamItemCount[0 @ LRPulseSensorPack] = 0;
  $TeamItemCount[1 @ LRPulseSensorPack] = 0;
  $TeamItemCount[2 @ LRPulseSensorPack] = 0;
  $TeamItemCount[3 @ LRPulseSensorPack] = 0;
  $TeamItemCount[4 @ LRPulseSensorPack] = 0;
  $TeamItemCount[5 @ LRPulseSensorPack] = 0;
  $TeamItemCount[6 @ LRPulseSensorPack] = 0;
  $TeamItemCount[7 @ LRPulseSensorPack] = 0;
}

ItemImageData LRPulseSensorPackImage 
{
  shapeFile = "radar_small";
  mountPoint = 2;
  mountOffset = { 0, 0, 0.1 };
  mountRotation = { 1.57, 0, 0 };
  firstPerson = false;
};

ItemData LRPulseSensorPack 
{
  description = "Adv. Pulse Sensor";
  shapeFile = "radar_small";
  className = "Backpack";
  heading = $InvHead[ihDSe];
  imageType = LRPulseSensorPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 250;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function LRPulseSensorPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function LRPulseSensorPack::onDeploy(%player,%item,%pos) 
{
  if (Item::deployShape(%player,"LRPulse Sensor",DeployableLRPulseSensor,%item)) 
  {
    Player::decItemCount(%player,%item);
    $TeamItemCount[GameBase::getTeam(%player) @ "LRPulseSensorPack"]++;
  }
}

SensorData DeployableLRPulseSensor
{
	description = "Adv. Pulse Sensor";
	className = "DeployableSensor";
	shapeFile = "radar_small";
	shadowDetailMask = 4;
	visibleToSensor = true;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	damageLevel = {0.8, 1.0};
	maxDamage = 1.0;
//   explosionId = DebrisExp;
	debrisId = flashDebrisSmall;
	range = 250;
	castLOS = true;
	supression = false;
	mapFilter = 4;
	mapIcon = "M_Radar";
};
