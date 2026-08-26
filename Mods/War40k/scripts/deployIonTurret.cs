
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//  Fusion Turret
//  original creation ???
//-=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
$TeamItemMax[IonTurretPack] = 4;
$InvList[IonTurretPack] = 1;
$RemoteInvList[IonTurretPack] = 1;

$CanAlwaysTeamDestroy[DeployableIonTurret] = 1;

 //-=-=-=-

function deployIonTurret::Initialize()
{
  $TeamItemCount[0 @ IonTurretPack] = 0;
  $TeamItemCount[1 @ IonTurretPack] = 0;
  $TeamItemCount[2 @ IonTurretPack] = 0;
  $TeamItemCount[3 @ IonTurretPack] = 0;
  $TeamItemCount[4 @ IonTurretPack] = 0;
  $TeamItemCount[5 @ IonTurretPack] = 0;
  $TeamItemCount[6 @ IonTurretPack] = 0;
  $TeamItemCount[7 @ IonTurretPack] = 0;
}

 //-=-=-=-

ItemImageData IonTurretPackImage
{
  shapeFile = "remoteturret";
  mountPoint = 2;
  mountOffset = { 0, -0.12, -0.1 };
  mountRotation = { 0, 0, 0 };
  mass = 2.5;
  firstPerson = false;
};

ItemData IonTurretPack
{
  description = "Fusion Turret";
  shapeFile = "remoteturret";
  className = "Backpack";
  heading = $InvHead[ihDWe];
  imageType = IonTurretPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 14;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function IonTurretPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function IonTurretPack::onDeploy(%player,%item,%pos) 
{
  if (Turret::deployShape(%player, "Ion Turret (" @ Client::getName(Player::getClient(%player)) @ ")", DeployableIonTurret, %item, $TurretLocGroundOnly))
    Player::decItemCount(%player,%item);
}

function IonTurretPack::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "The moment you don't respect this turret, it kills you. Often thought to be weak and useless.");
}
 //-=-=-=-

TurretData DeployableIonTurret
{
  className = "Turret";
  shapeFile = "remoteturret";
  projectileType = IonBolt;
  maxDamage = 0.65;
  maxEnergy = 60;
  minGunEnergy = 20;
  maxGunEnergy = 5;
  sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
  reloadDelay = 0.5;
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
  fireSound = SoundRemoteTurretFire;
  activationSound = SoundRemoteTurretOn;
  deactivateSound = SoundRemoteTurretOff;
  explosionId = flashExpMedium;
  description = "Fusion Turret";
  damageSkinData = "objectDamageSkins";
};

function DeployableIonTurret::onAdd(%this)
{
  schedule("DeployableIonTurret::deploy(" @ %this @ ");",1,%this);
  GameBase::setRechargeRate(%this,5);
  %this.shieldStrength = 0.02;
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Fusion Turret");
}

function DeployableIonTurret::deploy(%this) 
{
  GameBase::playSequence(%this,1,"deploy");
}

function DeployableIonTurret::onEndSequence(%this,%thread) 
{
  GameBase::setActive(%this,true);
}

function DeployableIonTurret::onDestroyed(%this) 
{
  Turret::onDestroyed(%this);
  $TeamItemCount[GameBase::getTeam(%this) @ "IonTurretPack"]--;
}

function DeployableIonTurret::onPower(%this,%power,%generator) 
{
}

function DeployableIonTurret::onEnabled(%this)
{
  GameBase::setRechargeRate(%this,5);
  GameBase::setActive(%this,true);
}

