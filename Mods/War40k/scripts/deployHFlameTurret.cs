//-=-=-==-==-=-=-=--=-=-=-=-=-=-
//   Mortar Cannon
// Original creation Edgecrusher
//-=-=-===-==-=-=-=-=-=-=-=-=-=-=
$TeamItemMax[HFlameTurretPack] = 2;
$InvList[HFlameTurretPack] = 1;
$RemoteInvList[HFlameTurretPack] = 1;

$CanControl[DeployableHFlameTurret] = 1;
$EmbedController[DeployableHFlameTurret] = 1;
$CanAlwaysTeamDestroy[DeployableHFlameTurret] = 1;

function deployHFlameTurret::Initialize()
{
  $TeamItemCount[0 @ HFlameTurretPack] = 0;
  $TeamItemCount[1 @ HFlameTurretPack] = 0;
  $TeamItemCount[2 @ HFlameTurretPack] = 0;
  $TeamItemCount[3 @ HFlameTurretPack] = 0;
  $TeamItemCount[4 @ HFlameTurretPack] = 0;
  $TeamItemCount[5 @ HFlameTurretPack] = 0;
  $TeamItemCount[6 @ HFlameTurretPack] = 0;
  $TeamItemCount[7 @ HFlameTurretPack] = 0;
}


GrenadeData Napalm
{
  bulletShapeName = "mortar.dts";
  explosionTag = mortarExp;
  collideWithOwner = True;
  ownerGraceMS = 250;
  collisionRadius = 0.3;
  mass = 2.0;
  elasticity = 0.1;
  damageClass = 1;
  damageValue = 1.5;
  damageType = $MortarDamageType;
  explosionRadius = 30.0;
  kickBackStrength = 50.0;
  maxLevelFlightDist = 300;
  totalTime = 30.0;
  liveTime = 2.0;
  projSpecialTime = 0.01;
  inheritedVelocityScale = 0.5;
  smokeName = "smoke.dts";
}
;

ItemImageData HFlameTurretPackImage 
{
  shapeFile = "remoteturret";
  mountPoint = 2;
  mountOffset = { 0, -0.12, -0.1 };
  mountRotation = { 0, 0, 0 };
  mass = 2.5;
  firstPerson = false;
};

ItemData HFlameTurretPack 
{
  description = "Mortar Cannon";
  shapeFile = "remoteturret";
  className = "Backpack";
  heading = $InvHead[ihDWe];
  imageType = HFlameTurretPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 95;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function HFlameTurretPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function HFlameTurretPack::onDeploy(%player,%item,%pos) 
{
  if (Turret::deployShape(%player, "Mortar Cannon (" @ Client::getName(Player::getClient(%player)) @ ")", DeployableHFlameTurret, %item, $TurretLocGroundOnly))
    Player::decItemCount(%player,%item);
}

function HFlameTurretPack::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "Highly explosive bomblets are lobbed at the enemy, blasting them to bits.");
}
 //-=-=-=-

TurretData DeployableHFlameTurret
{
  className = "Turret";
  shapeFile = "hellfiregun";
  projectileType = Napalm;
  maxDamage = 1.5;
  maxEnergy = 125;
  minGunEnergy = 40;
  maxGunEnergy = 30;
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
  fireSound = SoundFirePlasma;
  activationSound = SoundPlasmaTurretOn;
  deactivateSound = SoundPlasmaTurretOff;
  whirSound = SoundPlasmaTurretTurn;
  explosionId = flashExpMedium;
  description = "Mortar Cannon";
  damageSkinData = "objectDamageSkins";
};

function DeployableHFlameTurret::onAdd(%this) 
{
  schedule("DeployableHFlameTurret::deploy(" @ %this @ ");",1,%this);
  GameBase::setRechargeRate(%this,5);
  %this.shieldStrength = 0.02;
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Mortar Cannon");
}

function DeployableHFlameTurret::deploy(%this) 
{
  GameBase::playSequence(%this,1,"deploy");
}

function DeployableHFlameTurret::onEndSequence(%this,%thread) 
{
  GameBase::setActive(%this,true);
}

function DeployableHFlameTurret::onDestroyed(%this) 
{
  StaticShape::objectiveDestroyed(%this);
  %this.shieldStrength = 0;
  GameBase::setRechargeRate(%this,0);
  Turret::onDeactivate(%this);
  Turret::objectiveDestroyed(%this);
  CalcRadiusDamage(%this,$DebrisDamageType,20,0.2,25,20,20,2.5,1.1,200,100);
  $TeamItemCount[GameBase::getTeam(%this) @ "HFlameTurretPack"]--;
}

function DeployableHFlameTurret::onPower(%this,%power,%generator) 
{
}

function DeployableHFlameTurret::onEnabled(%this) 
{
  GameBase::setRechargeRate(%this,8);
  GameBase::setActive(%this,true);
}

