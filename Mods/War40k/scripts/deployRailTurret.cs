//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Hvy Las Turret
//
//  For installation information, see Install.txt
//  Created by Edgecrusher
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$TeamItemMax[RailTurretPack] = 2;
$InvList[RailTurretPack] = 1;
$RemoteInvList[RailTurretPack] = 1;

$CanControl[DeployableRailTurret] = 1;
$EmbedController[DeployableRailTurret] = 1;
$CanAlwaysTeamDestroy[DeployableRailTurret] = 1;

function deployRailTurret::Initialize()
{
  $TeamItemCount[0 @ RailTurretPack] = 0;
  $TeamItemCount[1 @ RailTurretPack] = 0;
  $TeamItemCount[2 @ RailTurretPack] = 0;
  $TeamItemCount[3 @ RailTurretPack] = 0;
  $TeamItemCount[4 @ RailTurretPack] = 0;
  $TeamItemCount[5 @ RailTurretPack] = 0;
  $TeamItemCount[6 @ RailTurretPack] = 0;
  $TeamItemCount[7 @ RailTurretPack] = 0;
}

ItemImageData RailTurretPackImage 
{
  shapeFile = "remoteturret";
  mountPoint = 2;
  mountOffset = { 0, -0.12, -0.1 };
  mountRotation = { 0, 0, 0 };
  mass = 2.5;
  firstPerson = false;
};

ItemData RailTurretPack 
{
  description = "Star Cannon";
  shapeFile = "remoteturret";
  className = "Backpack";
  heading = $InvHead[ihDWe];
  imageType = RailTurretPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 70;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function RailTurretPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function RailTurretPack::onDeploy(%player,%item,%pos) 
{
  if (Turret::deployShape(%player, "Star Cannon (" @ Client::getName(Player::getClient(%player)) @ ")", DeployableRailTurret, %item, $TurretLocGroundOnly))
    Player::decItemCount(%player,%item);
}

function RailTurretPack::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "The Heavy Las Turret has no automatic sensors. A teammate must control it manually.");
}

 //-=-=-=-

RocketData LasCannonCharge
{
  bulletShapeName = "shockwave.dts";
  explosionTag = mortarExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.62;
  damageType = $ReaperDamageType;
  explosionRadius = 25.5;
  kickBackStrength = 80.0;
  muzzleVelocity = 85.0;
  terminalVelocity = 180.0;
  acceleration = 5.0;
  totalTime = 10.0;
  liveTime = 10.0;
  lightRange = 2.0;
  lightColor = { 1.20, 1.7, 1.5 };
  inheritedVelocityScale = 0.0;
  trailType = 2;
  trailString = "shockwave.dts";
  smokeDist = 1.0;
  soundId = SoundJetHeavy;
};

TurretData DeployableRailTurret 
{
  className = "Turret";
  shapeFile = "hellfiregun";
  projectileType = LasCannonCharge;
  maxDamage = 2.21;
  maxEnergy = 140;
  minGunEnergy = 35;
  maxGunEnergy = 25;
  sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
  reloadDelay = 1.0;
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
  description = "Star Cannon";
  damageSkinData = "objectDamageSkins";
};

function DeployableRailTurret::onAdd(%this) 
{
  schedule("DeployableRailTurret::deploy(" @ %this @ ");",1,%this);
  GameBase::setRechargeRate(%this,7);
  %this.shieldStrength = 0.02;
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Star Cannon");
}

function DeployableRailTurret::deploy(%this) 
{
  GameBase::playSequence(%this,1,"deploy");
}

function DeployableRailTurret::onEndSequence(%this,%thread) 
{
  GameBase::setActive(%this,true);
}

function DeployableRailTurret::onDestroyed(%this) 
{
  StaticShape::objectiveDestroyed(%this);
  %this.shieldStrength = 0;
  GameBase::setRechargeRate(%this,0);
  Turret::onDeactivate(%this);
  Turret::objectiveDestroyed(%this);
  CalcRadiusDamage(%this,$DebrisDamageType,20,0.2,25,20,20,2.5,1.1,200,100);
  $TeamItemCount[GameBase::getTeam(%this) @ "RailTurretPack"]--;
}

function DeployableRailTurret::onPower(%this,%power,%generator) 
{
}

function DeployableRailTurret::onEnabled(%this) 
{
  GameBase::setRechargeRate(%this,15);
  GameBase::setActive(%this,true);
}
