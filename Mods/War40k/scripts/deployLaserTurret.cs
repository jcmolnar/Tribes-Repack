//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Las Turret
//
//  For installation information, see Install.txt
//  Created by ???
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$TeamItemMax[LaserTurretPack] = 8;
$InvList[LaserTurretPack] = 1;
$RemoteInvList[LaserTurretPack] = 1;

$CanAlwaysTeamDestroy[DeployableLaserTurret] = 1;

function deployLaserTurret::Initialize()
{
  $TeamItemCount[0 @ LaserTurretPack] = 0;
  $TeamItemCount[1 @ LaserTurretPack] = 0;
  $TeamItemCount[2 @ LaserTurretPack] = 0;
  $TeamItemCount[3 @ LaserTurretPack] = 0;
  $TeamItemCount[4 @ LaserTurretPack] = 0;
  $TeamItemCount[5 @ LaserTurretPack] = 0;
  $TeamItemCount[6 @ LaserTurretPack] = 0;
  $TeamItemCount[7 @ LaserTurretPack] = 0;
}

LaserData SnipeLaser
{
   laserBitmapName   = "laserPulse.bmp";
   hitName           = "laserhit.dts";

   damageConversion  = 0.01;
   baseDamageType    = $LaserDamageType;

   beamTime          = 1.0;

   lightRange        = 1.0;
   lightColor        = { 1.0, 0.25, 0.25 };

   detachFromShooter = false;
   hitSoundId        = SoundLaserHit;
};
ItemImageData LaserTurretPackImage 
{
  shapeFile = "remoteturret";
  mountPoint = 2;
  mountOffset = { 0, -0.1, -0.06 };
  mountRotation = { 0, 0, 0 };
  firstPerson = false;
};

ItemData LaserTurretPack 
{
  description = "Las Turret";
  shapeFile = "indoorgun";
  className = "Backpack";
  heading = $InvHead[ihDWe];
  imageType = LaserTurretPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 50;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function LaserTurretPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function LaserTurretPack::onDeploy(%player,%item,%pos) 
{
  if (Turret::deployShape(%player, "Las Turret (" @ Client::getName(Player::getClient(%player)) @ ")", DeployableLaserTurret, %item, $TurretLocAnywhere))
    Player::decItemCount(%player,%item);
}

function LaserTurretPack::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "Decent fire rate and excellent reliability make this a defense favorite.");
}
TurretData DeployableLaserTurret 
{
  className = "Turret";
  shapeFile = "indoorgun";
  projectileType = SnipeLaser;
  maxDamage = 1.65;
  maxEnergy = 150;
  minGunEnergy = 8;
  maxGunEnergy = 25;
  sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
  reloadDelay = 0.7;
  speed = 4.0;
  speedModifier = 1.5;
  range = 55;
  visibleToSensor = true;
  shadowDetailMask = 4;
  dopplerVelocity = 0;
  castLOS = true;
  supression = false;
  mapFilter = 2;
  mapIcon = "M_turret";
  debrisId = flashDebrisMedium;
  shieldShapeName = "shield";
  fireSound = SoundFireLaser;
  activationSound = SoundRemoteTurretOn;
  deactivateSound = SoundRemoteTurretOff;
  explosionId = flashExpMedium;
  description = "Las Turret";
  damageSkinData = "objectDamageSkins";
};

function DeployableLaserTurret::onAdd(%this) 
{
  schedule("DeployableLaserTurret::deploy(" @ %this @ ");",1,%this);
  GameBase::setRechargeRate(%this,5);
  %this.shieldStrength = 0.02;
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Las Turret");
}

function DeployableLaserTurret::deploy(%this) 
{
  GameBase::playSequence(%this,1,"deploy");
}

function DeployableLaserTurret::onEndSequence(%this,%thread) 
{
  GameBase::setActive(%this,true);
}

function DeployableLaserTurret::onDestroyed(%this) 
{
  Turret::onDestroyed(%this);
  $TeamItemCount[GameBase::getTeam(%this) @ "LaserTurretPack"]--;
}

function DeployableLaserTurret::onPower(%this,%power,%generator) 
{
}

function DeployableLaserTurret::onEnabled(%this) 
{
  GameBase::setRechargeRate(%this,10);
  GameBase::setActive(%this,true);
}

