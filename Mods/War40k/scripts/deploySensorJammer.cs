$TeamItemMax[DeployableSensorJammerPack] = 10;
$InvList[DeployableSensorJammerPack] = 1;
$RemoteInvList[DeployableSensorJammerPack] = 1;

$CanAlwaysTeamDestroy[DeployableSensorJammer] = 1;

function deploySensorJammer::Initialize()
{
  $TeamItemCount[0 @ DeployableSensorJammerPack] = 0;
  $TeamItemCount[1 @ DeployableSensorJammerPack] = 0;
  $TeamItemCount[2 @ DeployableSensorJammerPack] = 0;
  $TeamItemCount[3 @ DeployableSensorJammerPack] = 0;
  $TeamItemCount[4 @ DeployableSensorJammerPack] = 0;
  $TeamItemCount[5 @ DeployableSensorJammerPack] = 0;
  $TeamItemCount[6 @ DeployableSensorJammerPack] = 0;
  $TeamItemCount[7 @ DeployableSensorJammerPack] = 0;
}

ItemImageData DeployableSensorJamPackImage 
{
  shapeFile = "sensor_jammer";
  mountPoint = 2;
  mountOffset = { 0, 0.03, 0.1 };
  mountRotation = { 1.57, 0, 0 };
  firstPerson = false;
};

ItemData DeployableSensorJammerPack 
{
  description = "Sensor Jammer";
  shapeFile = "sensor_jammer";
  className = "Backpack";
  heading = $InvHead[ihDSe];
  imageType = DeployableSensorJamPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 20;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function DeployableSensorJammerPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function DeployableSensorJammerPack::onDeploy(%player,%item,%pos) 
{
  if (Item::deployShape(%player,"Sensor Jammer",DeployableSensorJammer,%item)) 
  {
    Player::decItemCount(%player,%item);
    $TeamItemCount[GameBase::getTeam(%player) @ "DeployableSensorJammerPack"]++;
  }
}

SensorData DeployableSensorJammer
{
	description = "Remote Sensor Jammer";
	className = "DeployableSensor";
	shapeFile = "sensor_jammer";
	shadowDetailMask = 4;
	visibleToSensor = true;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	damageLevel = {0.8, 1.0};
	maxDamage = 0.5;
//	explosionId = DebrisExp;
	debrisId = defaultDebrisSmall;
	range = 80;
	castLOS = true;
	supression = true;
	mapFilter = 4;
	mapIcon = "M_sensorJammer";
};
