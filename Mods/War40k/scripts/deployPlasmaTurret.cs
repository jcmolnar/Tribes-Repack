//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Distort Cannon
//
//  For installation information, see Install.txt
//  Created by Edgecrusher
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$TeamItemMax[PlasmaTurretPack] = 4;
$InvList[PlasmaTurretPack] = 1;
$RemoteInvList[PlasmaTurretPack] = 1;

$CanControl[DeployablePlasmaTurret] = 1;
$EmbedController[DeployablePlasmaTurret] = 1;
$CanAlwaysTeamDestroy[DeployablePlasmaTurret] = 1;

function deployPlasmaTurret::Initialize()
{
  $TeamItemCount[0 @ PlasmaTurretPack] = 0;
  $TeamItemCount[1 @ PlasmaTurretPack] = 0;
  $TeamItemCount[2 @ PlasmaTurretPack] = 0;
  $TeamItemCount[3 @ PlasmaTurretPack] = 0;
  $TeamItemCount[4 @ PlasmaTurretPack] = 0;
  $TeamItemCount[5 @ PlasmaTurretPack] = 0;
  $TeamItemCount[6 @ PlasmaTurretPack] = 0;
  $TeamItemCount[7 @ PlasmaTurretPack] = 0;
}

ItemImageData PlasmaTurretPackImage 
{
  shapeFile = "remoteturret";
  mountPoint = 2;
  mountOffset = { 0, -0.12, -0.1 };
  mountRotation = { 0, 0, 0 };
  mass = 2.5;
  firstPerson = false;
};

ItemData PlasmaTurretPack 
{
  description = "Distort Cannon";
  shapeFile = "remoteturret";
  className = "Backpack";
  heading = $InvHead[ihDWe];
  imageType = PlasmaTurretPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 100;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function PlasmaTurretPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function PlasmaTurretPack::onDeploy(%player,%item,%pos) 
{
  if (Turret::deployShape(%player, "Distort Cannon (" @ Client::getName(Player::getClient(%player)) @ ")", DeployablePlasmaTurret, %item, $TurretLocGroundOnly))
    Player::decItemCount(%player,%item);
}

function PlasmaTurretPack::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "Supercharged plasma bolts are fired, searing targets.");
}
 //-=-=-=-

RocketData Shock 
{
  bulletShapeName = "mortar.dts";
  explosionTag = LargeShockwave;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 1.5;
  damageType = $DDamageType;
  explosionRadius = 30.5;
  kickBackStrength = 0.0;
  muzzleVelocity = 85.0;
  terminalVelocity = 130.0;
  acceleration = 5.0;
  totalTime = 4.5;
  liveTime = 4.5;
  lightRange = 2.0;
  lightColor = { 1.20, 1.7, 1.5 };
  inheritedVelocityScale = 0.0;
  trailType = 2;
  trailString = "plasmawall.dts";
  smokeDist = 1.8;
};

TurretData DeployablePlasmaTurret
{
  className = "Turret";
  shapeFile = "hellfiregun";
  projectileType = Shock;
  maxDamage = 1.5;
  maxEnergy = 120;
  minGunEnergy = 60;
  maxGunEnergy = 40;
  sequenceSound[0] = {"deploy", SoundActivateMotionSensor };
  reloadDelay = 3.0;
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
  fireSound = SoundPlasmaTurretFire;
  activationSound = SoundPlasmaTurretOn;
  deactivateSound = SoundPlasmaTurretOff;
  whirSound = SoundPlasmaTurretTurn;
  explosionId = flashExpMedium;
  description = "Distort Cannon";
  damageSkinData = "objectDamageSkins";
};

function DeployablePlasmaTurret::onAdd(%this) 
{
  schedule("DeployablePlasmaTurret::deploy(" @ %this @ ");",1,%this);
  GameBase::setRechargeRate(%this,5);
  %this.shieldStrength = 0.02;
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Distort Cannon");
}

function DeployablePlasmaTurret::deploy(%this) 
{
  GameBase::playSequence(%this,1,"deploy");
}

function DeployablePlasmaTurret::onEndSequence(%this,%thread) 
{
  GameBase::setActive(%this,true);
}

function DeployablePlasmaTurret::onDestroyed(%this) 
{
  StaticShape::objectiveDestroyed(%this);
  %this.shieldStrength = 0;
  GameBase::setRechargeRate(%this,0);
  Turret::onDeactivate(%this);
  Turret::objectiveDestroyed(%this);
  CalcRadiusDamage(%this,$DebrisDamageType,20,0.2,25,20,20,2.5,1.1,200,100);
  $TeamItemCount[GameBase::getTeam(%this) @ "PlasmaTurretPack"]--;
}

function DeployablePlasmaTurret::onPower(%this,%power,%generator) 
{
}

function DeployablePlasmaTurret::onEnabled(%this) 
{
  GameBase::setRechargeRate(%this,5);
  GameBase::setActive(%this,true);
}

