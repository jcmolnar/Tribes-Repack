
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//  Haywire Turret
//  original creation Edgecrusher
//-=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
$TeamItemMax[HaywireTurretPack] = 4;
$InvList[HaywireTurretPack] = 1;
$RemoteInvList[HaywireTurretPack] = 1;

$CanAlwaysTeamDestroy[DeployableHaywireTurret] = 1;

 //-=-=-=-

function deployHaywireTurret::Initialize()
{
  $TeamItemCount[0 @ HaywireTurretPack] = 0;
  $TeamItemCount[1 @ HaywireTurretPack] = 0;
  $TeamItemCount[2 @ HaywireTurretPack] = 0;
  $TeamItemCount[3 @ HaywireTurretPack] = 0;
  $TeamItemCount[4 @ HaywireTurretPack] = 0;
  $TeamItemCount[5 @ HaywireTurretPack] = 0;
  $TeamItemCount[6 @ HaywireTurretPack] = 0;
  $TeamItemCount[7 @ HaywireTurretPack] = 0;
}

 //-=-=-=-

BulletData HaywireBullet 
{
  bulletShapeName = "mortar.dts";
  explosionTag = debrisExpSmall;
  expRandCycle = 3;
  mass = 0.05;
  bulletHoleIndex = 0;
  damageClass = 1;
  damageValue = 0.12;
  damageType = $FlashDamageType;
  explosionRadius = 4.0;
  aimDeflection = 0.004;
  muzzleVelocity = 625.0;
  inheritedVelocityScale = 1.0;
  isVisible = True;
  totalTime = 1.5;
  liveTime = 1.5;
};

ItemImageData HaywireTurretPackImage
{
  shapeFile = "remoteturret";
  mountPoint = 2;
  mountOffset = { 0, -0.12, -0.1 };
  mountRotation = { 0, 0, 0 };
  mass = 2.5;
  firstPerson = false;
};

ItemData HaywireTurretPack
{
  description = "Haywire Turret";
  shapeFile = "remoteturret";
  className = "Backpack";
  heading = $InvHead[ihDWe];
  imageType = HaywireTurretPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 14;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function HaywireTurretPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function HaywireTurretPack::onDeploy(%player,%item,%pos) 
{
  if (Turret::deployShape(%player, "Haywire Turret (" @ Client::getName(Player::getClient(%player)) @ ")", DeployableHaywireTurret, %item, $TurretLocAnywhere))
    Player::decItemCount(%player,%item);
}

function HaywireTurretPack::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "The haywire turret fires bursts of EMP energy which eliminate targets energy source temporarily.");
}
 //-=-=-=-

TurretData DeployableHaywireTurret
{
  className = "Turret";
  shapeFile = "chainturret";
  projectileType = HaywireBullet;
  maxDamage = 1.2;
  maxEnergy = 80;
  minGunEnergy = 5;
  maxGunEnergy = 1;
  sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
  reloadDelay = 1.15;
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
  description = "Haywire Turret";
  damageSkinData = "objectDamageSkins";
};

function DeployableHaywireTurret::onAdd(%this)
{
  schedule("DeployableHaywireTurret::deploy(" @ %this @ ");",1,%this);
  GameBase::setRechargeRate(%this,5);
  %this.shieldStrength = 0.005;
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Haywire Turret");
}

function DeployableHaywireTurret::deploy(%this) 
{
  GameBase::playSequence(%this,1,"deploy");
}

function DeployableHaywireTurret::onEndSequence(%this,%thread) 
{
  GameBase::setActive(%this,true);
}

function DeployableHaywireTurret::onDestroyed(%this) 
{
  Turret::onDestroyed(%this);
  $TeamItemCount[GameBase::getTeam(%this) @ "HaywireTurretPack"]--;
}

function DeployableHaywireTurret::onPower(%this,%power,%generator) 
{
}

function DeployableHaywireTurret::onEnabled(%this)
{
  GameBase::setRechargeRate(%this,5);
  GameBase::setActive(%this,true);
}

