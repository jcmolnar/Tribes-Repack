
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Targetting Device
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

 // Adjust this to adjust the number a team can have
$TeamItemMax[CameraPack] = 20;
$InvList[CameraPack] = 1;
$RemoteInvList[CameraPack] = 1;

$CanAlwaysControl[CameraTurret] = 1;
$CanAlwaysTeamDestroy[CameraTurret] = 1;

 //-=-=-=-

function deployCamera::Initialize()
{
	$TeamItemCount[0 @ CameraPack] = 0; 
	$TeamItemCount[1 @ CameraPack] = 0; 
	$TeamItemCount[2 @ CameraPack] = 0; 
	$TeamItemCount[3 @ CameraPack] = 0; 
	$TeamItemCount[4 @ CameraPack] = 0; 
	$TeamItemCount[5 @ CameraPack] = 0; 
	$TeamItemCount[6 @ CameraPack] = 0; 
	$TeamItemCount[7 @ CameraPack] = 0; 
}

 //-=-=-=-

LightningData MarkerBeam
{
  bitmapName = "discglow1.bmp";
  damageType = $LaserDamageType;
  detachFromShooter = false;
  boltLength = 1000.0;
  coneAngle = 0.01;
  damagePerSec = 0;
  energyDrainPerSec = 0;
  segmentDivisions = 0;
  numSegments = 1;
  beamWidth = 0.65;
  updateTime = 5;
  skipPercent = 0.01;
  displaceBias = 0.01;
  lightRange = 5.0;
  lightColor = { 1, 0, 0 };
};

function MarkerBeam::damageTarget(%target, %timeSlice, %damPerSec, %enDrainPerSec, %pos, %vec, %mom, %shooterId) 
{
  if (%shooterId.LaunchAt != %target)
  {
    %shooterID.LaunchAt=%target;

     // Get preferred name
    if (getObjectType(%target) == "Player") %n = Client::getName(Player::getClient(%target));
    else %n = GameBase::getMapName(%target);
     // If better names are blank, grab the data name
    if (%n == "") %n = GameBase::getDataName(%target);
    Client::sendMessage(%shooterId, 0, "Marked " @ %n);
	%team = GameBase::getTeam(%shooterId);
	if(%team.orbitalCannon != -1)
	{
		GameBase::virtual(%team.orbitalCannon, "setTarget", %target, TRUE);
	}
  }
}


ItemImageData CameraPackImage 
{
  shapeFile = "camera";
  mountPoint = 2;
  mountOffset = { 0, -0.1, -0.06 };
  mountRotation = { 0, 0, 0 };
  firstPerson = false;
};

ItemData CameraPack 
{
  description = "Targetting Device";
  shapeFile = "camera";
  className = "Backpack";
  heading = $InvHead[ihDSe];
  imageType = CameraPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 2;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function CameraPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function CameraPack::onDeploy(%player,%item,%pos) 
{
  if (Turret::deployShape(%player, "Targetting Device (" @ Client::getName(Player::getClient(%player)) @ ")", CameraTurret, %item, $TurretLocAnywhere))
    Player::decItemCount(%player,%item);
}

function CameraPack::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "Targets incoming enemies, warning of their approach.");
}

 //-=-=-=-

TurretData CameraTurret 
{
  className = "Turret";
  shapeFile = "indoorgun";
  projectileType = MarkerBeam;
  maxDamage = 0.35;
  maxEnergy = 220;
  minGunEnergy = 0;
  maxGunEnergy = 0;
  sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
  speed = 15.0;
  speedModifier = 1.5;
  range = 125;
  visibleToSensor = true;
  shadowDetailMask = 4;
  dopplerVelocity = 0;
  castLOS = true;
  supression = false;
  mapFilter = 2;
  mapIcon = "M_turret";
  debrisId = flashDebrisMedium;
  shieldShapeName = "shield";
  activationSound = SoundRemoteTurretOn;
  deactivateSound = SoundRemoteTurretOff;
  explosionId = flashExpMedium;
  description = "Targetting Device";
  damageSkinData = "objectDamageSkins";
  isSustained = true;
  firingTimeMS = 750;
  energyRate = 30.0;
  reloadDelay = 0.1;
};


function CameraTurret::onAdd(%this)
{
  schedule("CameraTurret::deploy(" @ %this @ ");",1,%this);
  GameBase::setRechargeRate(%this,8);
  %this.shieldStrength = 0.02;
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Targetting Device");
}

function CameraTurret::deploy(%this) 
{
  GameBase::playSequence(%this,1,"deploy");
}

function CameraTurret::onEndSequence(%this,%thread) 
{
  GameBase::setActive(%this,true);
}

function CameraTurret::onDestroyed(%this) 
{
  Turret::onDestroyed(%this);
  $TeamItemCount[GameBase::getTeam(%this) @ "CameraPack"]--;
}

function CameraTurret::onPower(%this,%power,%generator) 
{
}

function CameraTurret::onEnabled(%this)
{
  GameBase::setRechargeRate(%this,8);
  GameBase::setActive(%this,true);
}

