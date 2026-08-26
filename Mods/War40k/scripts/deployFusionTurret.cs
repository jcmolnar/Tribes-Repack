
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//  Fusion Turret
//  original creation Edgecrusher
//-=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
$TeamItemMax[FusionTurretPack] = 6;
$InvList[FusionTurretPack] = 1;
$RemoteInvList[FusionTurretPack] = 1;

$CanAlwaysTeamDestroy[DeployableFusionTurret] = 1;

 //-=-=-=-

function deployFusionTurret::Initialize()
{
  $TeamItemCount[0 @ FusionTurretPack] = 0;
  $TeamItemCount[1 @ FusionTurretPack] = 0;
  $TeamItemCount[2 @ FusionTurretPack] = 0;
  $TeamItemCount[3 @ FusionTurretPack] = 0;
  $TeamItemCount[4 @ FusionTurretPack] = 0;
  $TeamItemCount[5 @ FusionTurretPack] = 0;
  $TeamItemCount[6 @ FusionTurretPack] = 0;
  $TeamItemCount[7 @ FusionTurretPack] = 0;
}

//RocketData FusionBlast
//{
  //bulletShapeName = "plasmaex.dts";
  //explosionTag = plasmaExp;
  //collisionRadius = 0.0;
  //mass = 2.0;
  //damageClass = 1;
  //damageValue = 0.15;
  //damageType = $DeathDamageType;
  //explosionRadius = 6;
  //kickBackStrength = 0.0;
  //muzzleVelocity = 200.0;
  //terminalVelocity = 200.0;
  //acceleration = 5.0;
  //totalTime = 0.3;
  //liveTime = 0.3;
  //lightRange = 5.0;
  //lightColor = { 1.0, 0.7, 0.5 };
  //inheritedVelocityScale = 0.5;
  //trailType = 1;
  //trailLength = 30;
 // trailWidth = 0.3;
//};
 //-=-=-=-

ItemImageData FusionTurretPackImage
{
  shapeFile = "remoteturret";
  mountPoint = 2;
  mountOffset = { 0, -0.12, -0.1 };
  mountRotation = { 0, 0, 0 };
  mass = 2.5;
  firstPerson = false;
};

ItemData FusionTurretPack
{
  description = "Fusion Turret";
  shapeFile = "remoteturret";
  className = "Backpack";
  heading = $InvHead[ihDWe];
  imageType = FusionTurretPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 14;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function FusionTurretPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function FusionTurretPack::onDeploy(%player,%item,%pos) 
{
  if (Turret::deployShape(%player, "Fusion Turret (" @ Client::getName(Player::getClient(%player)) @ ")", DeployableFusionTurret, %item, $TurretLocAnywhere))
    Player::decItemCount(%player,%item);
}

function FusionTurretPack::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "Bursts of energy rip targets to shreds.");
}
 //-=-=-=-

TurretData DeployableFusionTurret
{
  className = "Turret";
  shapeFile = "remoteturret";
  projectileType = FusionBoltx;
  maxDamage = 1.4;
  maxEnergy = 160;
  minGunEnergy = 10;
  maxGunEnergy = 5;
  sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
  reloadDelay = 1.0;
  speed = 4.0;
  speedModifier = 1.5;
  range = 75;
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
  description = "Fusion Turret";
  damageSkinData = "objectDamageSkins";
};

function DeployableFusionTurret::onAdd(%this)
{
  schedule("DeployableFusionTurret::deploy(" @ %this @ ");",1,%this);
  GameBase::setRechargeRate(%this,5);
  %this.shieldStrength = 0.02;
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Fusion Turret");
}

function DeployableFusionTurret::deploy(%this) 
{
  GameBase::playSequence(%this,1,"deploy");
}

function DeployableFusionTurret::onEndSequence(%this,%thread) 
{
  GameBase::setActive(%this,true);
}

function DeployableFusionTurret::onDestroyed(%this) 
{
  Turret::onDestroyed(%this);
  $TeamItemCount[GameBase::getTeam(%this) @ "FusionTurretPack"]--;
}

function DeployableFusionTurret::onPower(%this,%power,%generator) 
{
}

function DeployableFusionTurret::onEnabled(%this)
{
  GameBase::setRechargeRate(%this,5);
  GameBase::setActive(%this,true);
}

