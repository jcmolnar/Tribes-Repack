//ORIGINAL CREATION 
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Shuriken Turret
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$TeamItemMax[ShurTurretPack] = 4;
$InvList[ShurTurretPack] = 1;
$RemoteInvList[ShurTurretPack] = 1;

$CanControl[DeployableShurTurret] = 0;
$EmbedController[DeployableShurTurret] = 0;
$CanAlwaysTeamDestroy[DeployableShurTurret] = 1;

function deployShurTurret::Initialize()
{
  $TeamItemCount[0 @ ShurTurretPack] = 0;
  $TeamItemCount[1 @ ShurTurretPack] = 0;
  $TeamItemCount[2 @ ShurTurretPack] = 0;
  $TeamItemCount[3 @ ShurTurretPack] = 0;
  $TeamItemCount[4 @ ShurTurretPack] = 0;
  $TeamItemCount[5 @ ShurTurretPack] = 0;
  $TeamItemCount[6 @ ShurTurretPack] = 0;
  $TeamItemCount[7 @ ShurTurretPack] = 0;
}


RocketData ShurBlast 
{
  bulletShapeName = "discb.dts";
  explosionTag = bulletExp0;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 0;
  damageValue = 0.13;
  damageType = $ShurikenDamageType;
  kickBackStrength = 0.0;
  muzzleVelocity = 285.0;
  terminalVelocity = 285.0;
  acceleration = 5.0;
  totalTime = 4.5;
  liveTime = 4.5;
  lightRange = 5.0;
  lightColor = { 0.4, 0.4, 1.0 };
  inheritedVelocityScale = 1.0;
  trailType = 1;
  trailLength = 15;
  trailWidth = 0.3;
  soundId = SoundDiscSpin;
};

ItemImageData ShurTurretPackImage 
{
  shapeFile = "remoteturret";
  mountPoint = 2;
  mountOffset = { 0, -0.12, -0.1 };
  mountRotation = { 0, 0, 0 };
  mass = 2.5;
  firstPerson = false;
};

ItemData ShurTurretPack 
{
  description = "Shuriken Turret";
  shapeFile = "remoteturret";
  className = "Backpack";
  heading = $InvHead[ihDWe];
  imageType = ShurTurretPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 20;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function ShurTurretPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function ShurTurretPack::onDeploy(%player,%item,%pos) 
{
  if (Turret::deployShape(%player, "Shuriken Turret (" @ Client::getName(Player::getClient(%player)) @ ")", DeployableShurTurret, %item, $TurretLocAnywhere))
    Player::decItemCount(%player,%item);
}

function ShurTurretPack::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "The shuriken turret fires non-explosive, rapid fire shuriken.Excellent for all purposes.");
}
 //-=-=-=-

TurretData DeployableShurTurret
{
  className = "Turret";
  shapeFile = "remoteturret";
  projectileType = ShurBlast;
  maxDamage = 1.5;
  maxEnergy = 180;
  minGunEnergy = 20;
  maxGunEnergy = 16;
  sequenceSound[0] = {"deploy", SoundActivateMotionSensor };
  reloadDelay = 0.25;
  speed = 4.0;
  speedModifier = 1.5;
  range = 100;
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
  activationSound = SoundPlasmaTurretOn;
  deactivateSound = SoundPlasmaTurretOff;
  whirSound = SoundPlasmaTurretTurn;
  explosionId = flashExpMedium;
  description = "Shuriken Turret";
  damageSkinData = "objectDamageSkins";
};

function DeployableShurTurret::onAdd(%this) 
{
  schedule("DeployableShurTurret::deploy(" @ %this @ ");",1,%this);
  GameBase::setRechargeRate(%this,5);
  %this.shieldStrength = 0.02;
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Shuriken Turret");
}

function DeployableShurTurret::deploy(%this) 
{
  GameBase::playSequence(%this,1,"deploy");
}

function DeployableShurTurret::onEndSequence(%this,%thread) 
{
  GameBase::setActive(%this,true);
}

function DeployableShurTurret::onDestroyed(%this) 
{
  StaticShape::objectiveDestroyed(%this);
  %this.shieldStrength = 0;
  GameBase::setRechargeRate(%this,0);
  Turret::onDeactivate(%this);
  Turret::objectiveDestroyed(%this);
  CalcRadiusDamage(%this,$DebrisDamageType,20,0.2,25,20,20,0.5,0.7,200,100);
  $TeamItemCount[GameBase::getTeam(%this) @ "ShurTurretPack"]--;
}

function DeployableShurTurret::onPower(%this,%power,%generator) 
{
}

function DeployableShurTurret::onEnabled(%this) 
{
  GameBase::setRechargeRate(%this,8);
  GameBase::setActive(%this,true);
}

