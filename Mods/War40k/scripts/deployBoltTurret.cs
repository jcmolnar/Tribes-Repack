
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//  Auto Turret
//  original creation Edgecrusher
//-=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
$TeamItemMax[BoltTurretPack] = 6;
$InvList[BoltTurretPack] = 1;
$RemoteInvList[BoltTurretPack] = 1;

$CanAlwaysTeamDestroy[DeployableBoltTurret] = 1;

 //-=-=-=-

function deployBoltTurret::Initialize()
{
  $TeamItemCount[0 @ BoltTurretPack] = 0;
  $TeamItemCount[1 @ BoltTurretPack] = 0;
  $TeamItemCount[2 @ BoltTurretPack] = 0;
  $TeamItemCount[3 @ BoltTurretPack] = 0;
  $TeamItemCount[4 @ BoltTurretPack] = 0;
  $TeamItemCount[5 @ BoltTurretPack] = 0;
  $TeamItemCount[6 @ BoltTurretPack] = 0;
  $TeamItemCount[7 @ BoltTurretPack] = 0;
}

 //-=-=-=-

BulletData BoltBullet 
{
  bulletShapeName = "rocket.dts";
  explosionTag = debrisExpSmall;
  expRandCycle = 3;
  mass = 0.05;
  bulletHoleIndex = 0;
  damageClass = 1;
  damageValue = 0.15;
  damageType = $MissileDamageType;
  explosionRadius = 4.0;
  aimDeflection = 0.004;
  muzzleVelocity = 625.0;
  inheritedVelocityScale = 1.0;
  isVisible = True;
  totalTime = 1.5;
  liveTime = 1.5;
};

ItemImageData BoltTurretPackImage
{
  shapeFile = "remoteturret";
  mountPoint = 2;
  mountOffset = { 0, -0.12, -0.1 };
  mountRotation = { 0, 0, 0 };
  mass = 2.5;
  firstPerson = false;
};

ItemData BoltTurretPack
{
  description = "Auto Turret";
  shapeFile = "remoteturret";
  className = "Backpack";
  heading = $InvHead[ihDWe];
  imageType = BoltTurretPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 14;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function BoltTurretPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function BoltTurretPack::onDeploy(%player,%item,%pos) 
{
  if (Turret::deployShape(%player, "Auto Turret (" @ Client::getName(Player::getClient(%player)) @ ")", DeployableBoltTurret, %item, $TurretLocAnywhere))
    Player::decItemCount(%player,%item);
}

function BoltTurretPack::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "The auto turret fires armor piercing rounds, ripping apart targets quite swiftly.");
}
 //-=-=-=-

TurretData DeployableBoltTurret
{
  className = "Turret";
  shapeFile = "chainturret";
  projectileType = BoltBullet;
  maxDamage = 2.2;
  maxEnergy = 180;
  minGunEnergy = 5;
  maxGunEnergy = 1;
  sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
  reloadDelay = 0.35;
  speed = 4.0;
  speedModifier = 1.5;
  range = 65;
  visibleToSensor = true;
  shadowDetailMask = 4;
  dopplerVelocity = 0;
  castLOS = true;
  supression = false;
  mapFilter = 2;
  mapIcon = "M_turret";
  debrisId = flashDebrisMedium;
  shieldShapeName = "shield";
  fireSound = SoundRemoteTurretFire;
  activationSound = SoundRemoteTurretOn;
  deactivateSound = SoundRemoteTurretOff;
  explosionId = flashExpMedium;
  description = "Auto Turret";
  damageSkinData = "objectDamageSkins";
  shieldStrength = 0.1;
};

function DeployableBoltTurret::onAdd(%this)
{
  schedule("DeployableBoltTurret::deploy(" @ %this @ ");",1,%this);
  GameBase::setRechargeRate(%this,5);
  %this.shieldStrength = 0.02;
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Auto Turret");
}

function DeployableBoltTurret::deploy(%this) 
{
  GameBase::playSequence(%this,1,"deploy");
}

function DeployableBoltTurret::onEndSequence(%this,%thread) 
{
  GameBase::setActive(%this,true);
}

function DeployableBoltTurret::onDestroyed(%this) 
{
  Turret::onDestroyed(%this);
  $TeamItemCount[GameBase::getTeam(%this) @ "BoltTurretPack"]--;
}

function DeployableBoltTurret::onPower(%this,%power,%generator) 
{
}

function DeployableBoltTurret::onEnabled(%this)
{
  GameBase::setRechargeRate(%this,5);
  GameBase::setActive(%this,true);
}

