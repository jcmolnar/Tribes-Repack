//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Bolter Cannon
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$TeamItemMax[BoltCanTurretPack] = 2;
$InvList[BoltCanTurretPack] = 1;
$RemoteInvList[BoltCanTurretPack] = 1;

$CanControl[DeployableBoltCanTurret] = 1;
$EmbedController[DeployableBoltCanTurret] = 1;
$CanAlwaysTeamDestroy[DeployableBoltCanTurret] = 1;

function deployBoltCanTurret::Initialize()
{
  $TeamItemCount[0 @ BoltCanTurretPack] = 0;
  $TeamItemCount[1 @ BoltCanTurretPack] = 0;
  $TeamItemCount[2 @ BoltCanTurretPack] = 0;
  $TeamItemCount[3 @ BoltCanTurretPack] = 0;
  $TeamItemCount[4 @ BoltCanTurretPack] = 0;
  $TeamItemCount[5 @ BoltCanTurretPack] = 0;
  $TeamItemCount[6 @ BoltCanTurretPack] = 0;
  $TeamItemCount[7 @ BoltCanTurretPack] = 0;
}

RocketData BoltCanRound 
{
  bulletShapeName = "rocket.dts";
  explosionTag = debrisExpSmall;
  expRandCycle = 1;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.15;
  damageType = $MissileDamageType;
  explosionRadius = 7.5;
  kickBackStrength = 0.0;
  muzzleVelocity = 165.0;
  terminalVelocity = 2000.0;
  aimDeflection = 0.003;
  acceleration = 100.0;
  totalTime = 3.5;
  liveTime = 3.5;
  lightRange = 5.0;
  lightColor = { 0.0, 0.0, 0.0 };
  inheritedVelocityScale = 0.5;
  trailType = 2;
  trailString = "rsmoke.dts";
  smokeDist = 0.0;
  soundId = SoundJetHeavy;
};

ItemImageData BoltCanTurretPackImage 
{
  shapeFile = "remoteturret";
  mountPoint = 2;
  mountOffset = { 0, -0.12, -0.1 };
  mountRotation = { 0, 0, 0 };
  mass = 2.5;
  firstPerson = false;
};

ItemData BoltCanTurretPack 
{
  description = "Bolter Cannon";
  shapeFile = "remoteturret";
  className = "Backpack";
  heading = $InvHead[ihDWe];
  imageType = BoltCanTurretPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 130;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function BoltCanTurretPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function BoltCanTurretPack::onDeploy(%player,%item,%pos) 
{
  if (Turret::deployShape(%player, "Bolter Cannon (" @ Client::getName(Player::getClient(%player)) @ ")", DeployableBoltCanTurret, %item, $TurretLocGroundOnly))
    Player::decItemCount(%player,%item);
}

function BoltCanTurretPack::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "The Bolter Cannon is lethal, but must be controlled manually.");
}

 //-=-=-=-

TurretData DeployableBoltCanTurret 
{
  className = "Turret";
  shapeFile = "hellfiregun";
  projectileType = BoltCanRound;
  maxDamage = 1.0;
  maxEnergy = 115;
  minGunEnergy = 2;
  maxGunEnergy = 6;
  sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
  reloadDelay = 0.112;
  speed = 4.0;
  speedModifier = 1.5;
  range = 0;
  visibleToSensor = true;
  shadowDetailMask = 4;
  dopplerVelocity = 0;
  castLOS = true;
  supression = false;
  mapFilter = 2;
  mapIcon = "M_turret";
  debrisId = flashDebrisMedium;
  shieldShapeName = "shield";
  fireSound = SoundMissileTurretFire;
  activationSound = SoundPlasmaTurretOn;
  deactivateSound = SoundPlasmaTurretOff;
  whirSound = SoundPlasmaTurretTurn;
  explosionId = flashExpMedium;
  description = "Bolter Cannon";
  damageSkinData = "objectDamageSkins";
};

function DeployableBoltCanTurret::onAdd(%this) 
{
  schedule("DeployableBoltCanTurret::deploy(" @ %this @ ");",1,%this);
  GameBase::setRechargeRate(%this,4);
  %this.shieldStrength = 0.02;
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Bolter Cannon");
}

function DeployableBoltCanTurret::deploy(%this) 
{
  GameBase::playSequence(%this,1,"deploy");
}

function DeployableBoltCanTurret::onEndSequence(%this,%thread) 
{
  GameBase::setActive(%this,true);
}

function DeployableBoltCanTurret::onDestroyed(%this) 
{
  StaticShape::objectiveDestroyed(%this);
  %this.shieldStrength = 0;
  GameBase::setRechargeRate(%this,0);
  Turret::onDeactivate(%this);
  Turret::objectiveDestroyed(%this);
  CalcRadiusDamage(%this,$DebrisDamageType,20,0.2,25,20,20,2.5,1.1,200,100);
  $TeamItemCount[GameBase::getTeam(%this) @ "BoltCanTurretPack"]--;
}

function DeployableBoltCanTurret::onPower(%this,%power,%generator) 
{
}

function DeployableBoltCanTurret::onEnabled(%this) 
{
  GameBase::setRechargeRate(%this,15);
  GameBase::setActive(%this,true);
}
