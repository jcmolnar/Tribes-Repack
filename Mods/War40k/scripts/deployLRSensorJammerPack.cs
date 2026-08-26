//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Adv. jammer
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$TeamItemMax[DeployableLRSensorJammerPack] = 4;
$InvList[DeployableLRSensorJammerPack] = 1;
$RemoteInvList[DeployableLRSensorJammerPack] = 1;

$CanAlwaysTeamDestroy[DeployableLRSensorJammer] = 1;

function deployLRSensorJammerPack::Initialize()
{
  $TeamItemCount[0 @ DeployableLRSensorJammerPack] = 0;
  $TeamItemCount[1 @ DeployableLRSensorJammerPack] = 0;
  $TeamItemCount[2 @ DeployableLRSensorJammerPack] = 0;
  $TeamItemCount[3 @ DeployableLRSensorJammerPack] = 0;
  $TeamItemCount[4 @ DeployableLRSensorJammerPack] = 0;
  $TeamItemCount[5 @ DeployableLRSensorJammerPack] = 0;
  $TeamItemCount[6 @ DeployableLRSensorJammerPack] = 0;
  $TeamItemCount[7 @ DeployableLRSensorJammerPack] = 0;
}

ItemImageData DeployableLRSensorJamPackImage 
{
  shapeFile = "sensor_jammer";
  mountPoint = 2;
  mountOffset = { 0, 0.03, 0.1 };
  mountRotation = { 1.57, 0, 0 };
  firstPerson = false;
};

ItemData DeployableLRSensorJammerPack 
{
  description = "Adv. Sensor Jammer";
  shapeFile = "sensor_jammer";
  className = "Backpack";
  heading = $InvHead[ihDSe];
  imageType = DeployableLRSensorJamPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 250;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function DeployableLRSensorJammerPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function DeployableLRSensorJammerPack::onDeploy(%player,%item,%pos) 
{
  if (Item::deployShape(%player,"Sensor Jammer",DeployableLRSensorJammer,%item)) 
  {
    Player::decItemCount(%player,%item);
    $TeamItemCount[GameBase::getTeam(%player) @ "DeployableLRSensorJammerPack"]++;
  }
}

SensorData DeployableLRSensorJammer
{
	description = "Adv. Sensor Jammer";
	className = "DeployableSensor";
	shapeFile = "sensor_jammer";
	shadowDetailMask = 4;
	visibleToSensor = true;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	damageLevel = {0.8, 1.0};
	maxDamage = 0.5;
//	explosionId = DebrisExp;
	debrisId = defaultDebrisSmall;
	range = 250;
	castLOS = true;
	supression = true;
	mapFilter = 4;
	mapIcon = "M_sensorJammer";
};
