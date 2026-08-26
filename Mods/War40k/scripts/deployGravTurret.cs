
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//  Grav Turret
//  original creation Edgecrusher
//-=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
$TeamItemMax[GravTurretPack] = 4;
$InvList[GravTurretPack] = 1;
$RemoteInvList[GravTurretPack] = 1;

$CanAlwaysTeamDestroy[DeployableGravTurret] = 1;

 //-=-=-=-

function deployGravTurret::Initialize()
{
  $TeamItemCount[0 @ GravTurretPack] = 0;
  $TeamItemCount[1 @ GravTurretPack] = 0;
  $TeamItemCount[2 @ GravTurretPack] = 0;
  $TeamItemCount[3 @ GravTurretPack] = 0;
  $TeamItemCount[4 @ GravTurretPack] = 0;
  $TeamItemCount[5 @ GravTurretPack] = 0;
  $TeamItemCount[6 @ GravTurretPack] = 0;
  $TeamItemCount[7 @ GravTurretPack] = 0;
}

 //-=-=-=-

ItemImageData GravTurretPackImage
{
  shapeFile = "remoteturret";
  mountPoint = 2;
  mountOffset = { 0, -0.12, -0.1 };
  mountRotation = { 0, 0, 0 };
  mass = 2.5;
  firstPerson = false;
};

ItemData GravTurretPack
{
  description = "Grav Turret";
  shapeFile = "remoteturret";
  className = "Backpack";
  heading = $InvHead[ihDWe];
  imageType = GravTurretPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 14;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function GravTurretPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function GravTurretPack::onDeploy(%player,%item,%pos) 
{
  if (Turret::deployShape(%player, "Grav Turret (" @ Client::getName(Player::getClient(%player)) @ ")", DeployableGravTurret, %item, $TurretLocAnywhere))
    Player::decItemCount(%player,%item);
}

function GravTurretPack::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "The Grav turret pulls targets into its proximity. Does no direct damage, but can be used constructively.");
}
 //-=-=-=-

TurretData DeployableGravTurret
{
  maxDamage = 1.0;
  maxEnergy = 130;
  minGunEnergy = 12;
  maxGunEnergy = 2;
  range = 60;
  visibleToSensor = true;
  dopplerVelocity = 0;
  castLOS = true;
  supression = false;
  mapFilter = 2;
  mapIcon = "M_turret";
  debrisId = defaultDebrisMedium;
  className = "Turret";
  shapeFile = "chainturret";
  shieldShapeName = "shield";
  speed = 5.0;
  speedModifier = 1.5;
  projectileType = TractorBeam;
  reloadDelay = 0.3;
  explosionId = LargeShockwave;
  description = "Grav Turret";
  fireSound = SoundGeneratorPower;
  activationSound = SoundChainTurretOn;
  deactivateSound = SoundChainTurretOff;
  damageSkinData = "objectDamageSkins";
  shadowDetailMask = 8;
  isSustained = true;
  firingTimeMS = 750;
  energyRate = 30.0;
};

function DeployableGravTurret::onAdd(%this)
{
  schedule("DeployableGravTurret::deploy(" @ %this @ ");",1,%this);
  GameBase::setRechargeRate(%this,5);
  %this.shieldStrength = 0.005;
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Grav Turret");
}

function DeployableGravTurret::deploy(%this) 
{
  GameBase::playSequence(%this,1,"deploy");
}

function DeployableGravTurret::onEndSequence(%this,%thread) 
{
  GameBase::setActive(%this,true);
}

function DeployableGravTurret::onDestroyed(%this) 
{
  Turret::onDestroyed(%this);
  $TeamItemCount[GameBase::getTeam(%this) @ "GravTurretPack"]--;
}

function DeployableGravTurret::onPower(%this,%power,%generator) 
{
}

function DeployableGravTurret::onEnabled(%this)
{
  GameBase::setRechargeRate(%this,5);
  GameBase::setActive(%this,true);
}

