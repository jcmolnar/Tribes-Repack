$TeamItemMax[PulseSensorPack] = 15;
$InvList[PulseSensorPack] = 1;
$RemoteInvList[PulseSensorPack] = 1;

$CanAlwaysTeamDestroy[DeployablePulseSensor] = 1;

function deployPulseSensor::Initialize()
{
  $TeamItemCount[0 @ PulseSensorPack] = 0;
  $TeamItemCount[1 @ PulseSensorPack] = 0;
  $TeamItemCount[2 @ PulseSensorPack] = 0;
  $TeamItemCount[3 @ PulseSensorPack] = 0;
  $TeamItemCount[4 @ PulseSensorPack] = 0;
  $TeamItemCount[5 @ PulseSensorPack] = 0;
  $TeamItemCount[6 @ PulseSensorPack] = 0;
  $TeamItemCount[7 @ PulseSensorPack] = 0;
}

ItemImageData PulseSensorPackImage 
{
  shapeFile = "radar_small";
  mountPoint = 2;
  mountOffset = { 0, 0, 0.1 };
  mountRotation = { 1.57, 0, 0 };
  firstPerson = false;
};

ItemData PulseSensorPack 
{
  description = "Pulse Sensor";
  shapeFile = "radar_small";
  className = "Backpack";
  heading = $InvHead[ihDSe];
  imageType = PulseSensorPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 12;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function PulseSensorPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function PulseSensorPack::onDeploy(%player,%item,%pos) 
{
  if (Item::deployShape(%player,"Pulse Sensor",DeployablePulseSensor,%item)) 
  {
    Player::decItemCount(%player,%item);
    $TeamItemCount[GameBase::getTeam(%player) @ "PulseSensorPack"]++;
  }
}

SensorData DeployablePulseSensor
{
	description = "Remote Pulse Sensor";
	className = "DeployableSensor";
	shapeFile = "radar_small";
	shadowDetailMask = 4;
	visibleToSensor = true;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	damageLevel = {0.8, 1.0};
	maxDamage = 1.0;
//   explosionId = DebrisExp;
	debrisId = flashDebrisSmall;
	range = 200;
	castLOS = true;
	supression = false;
	mapFilter = 4;
	mapIcon = "M_Radar";
};
