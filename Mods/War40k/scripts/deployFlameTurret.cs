
//-=-=-==-==-=-=-=--=-=-=-=-=-=-
//   Flame Turret
// Original creation Edgecrusher
//-=-=-===-==-=-=-=-=-=-=-=-=-=-=

$TeamItemMax[FlameTurretPack] = 4;
$InvList[FlameTurretPack] = 1;
$RemoteInvList[FlameTurretPack] = 1;

$CanAlwaysTeamDestroy[DeployableFlameTurret] = 1;

//-=-=-=-

function deployFlameTurret::Initialize()
{
  $TeamItemCount[0 @ FlameTurretPack] = 0;
  $TeamItemCount[1 @ FlameTurretPack] = 0;
  $TeamItemCount[2 @ FlameTurretPack] = 0;
  $TeamItemCount[3 @ FlameTurretPack] = 0;
  $TeamItemCount[4 @ FlameTurretPack] = 0;
  $TeamItemCount[5 @ FlameTurretPack] = 0;
  $TeamItemCount[6 @ FlameTurretPack] = 0;
  $TeamItemCount[7 @ FlameTurretPack] = 0;
}


BulletData TFlamerBolt 
{
  bulletShapeName = "tumult_small.dts";
  explosionTag = plasmaExp;
  damageClass = 1;
  damageValue = 0.06;
  damageType = $PlasmaDamageType;
  explosionRadius = 4.0;
  muzzleVelocity = 30.0;
  totalTime = 0.7;
  liveTime = 0.7;
  lightRange = 3.0;
  lightColor = { 1, 1, 0 };
  inheritedVelocityScale = 0.3;
  isVisible = True;
  soundId = SoundFirePlasma;
};
 //-=-=-=-

ItemImageData FlameTurretPackImage
{
  shapeFile = "remoteturret";
  mountPoint = 2;
  mountOffset = { 0, -0.12, -0.1 };
  mountRotation = { 0, 0, 0 };
  mass = 2.5;
  firstPerson = false;
};

ItemData FlameTurretPack
{
  description = "Flame Turret";
  shapeFile = "remoteturret";
  className = "Backpack";
  heading = $InvHead[ihDWe];
  imageType = FlameTurretPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 30;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function FlameTurretPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function FlameTurretPack::onDeploy(%player,%item,%pos) 
{
  if (Turret::deployShape(%player, "Flame Turret (" @ Client::getName(Player::getClient(%player)) @ ")", DeployableFlameTurret, %item, $TurretLocAnywhere))
    Player::decItemCount(%player,%item);
}

function FlameTurretPack::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "Brutal on the enemy. Ignites them, burning them to death in a short time.");
}
 //-=-=-=-

TurretData DeployableFlameTurret
{
  className = "Turret";
  shapeFile = "camera";
  projectileType = TFlamerBolt;
  maxDamage = 1.0;
  maxEnergy = 120;
  minGunEnergy = 20;
  maxGunEnergy = 5;
  sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
  reloadDelay = 0.1;
  speed = 4.0;
  speedModifier = 1.5;
  range = 35;
  visibleToSensor = true;
  shadowDetailMask = 4;
  dopplerVelocity = 0;
  castLOS = true;
  supression = false;
  mapFilter = 2;
  mapIcon = "M_turret";
  debrisId = flashDebrisMedium;
  shieldShapeName = "shield";
  fireSound = SoundFirePlasma;
  activationSound = SoundRemoteTurretOn;
  deactivateSound = SoundRemoteTurretOff;
  explosionId = flashExpMedium;
  description = "Flame Turret";
  damageSkinData = "objectDamageSkins";
};

function DeployableFlameTurret::onAdd(%this)
{
  schedule("DeployableFlameTurret::deploy(" @ %this @ ");",1,%this);
  GameBase::setRechargeRate(%this,5);
  %this.shieldStrength = 0.02;
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Flame Turret");
}

function DeployableFlameTurret::deploy(%this) 
{
  GameBase::playSequence(%this,1,"deploy");
}

function DeployableFlameTurret::onEndSequence(%this,%thread) 
{
  GameBase::setActive(%this,true);
}

function DeployableFlameTurret::onDestroyed(%this) 
{
  Turret::onDestroyed(%this);
  $TeamItemCount[GameBase::getTeam(%this) @ "FlameTurretPack"]--;
}

function DeployableFlameTurret::onPower(%this,%power,%generator) 
{
}

function DeployableFlameTurret::onEnabled(%this)
{
  GameBase::setRechargeRate(%this,5);
  GameBase::setActive(%this,true);
}

