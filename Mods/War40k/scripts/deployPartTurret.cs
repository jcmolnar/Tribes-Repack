//-=-=-==-==-=-=-=--=-=-=-=-=-=-
//   Particle Blast Turret
// Original creation Edgecrusher
//-=-=-===-==-=-=-=-=-=-=-=-=-=-=

$TeamItemMax[PartTurretPack] = 6;
$InvList[PartTurretPack] = 1;
$RemoteInvList[PartTurretPack] = 1;

$CanAlwaysTeamDestroy[DeployablePartTurret] = 1;

 //-=-=-=-

function deployPartTurret::Initialize()
{
  $TeamItemCount[0 @ PartTurretPack] = 0;
  $TeamItemCount[1 @ PartTurretPack] = 0;
  $TeamItemCount[2 @ PartTurretPack] = 0;
  $TeamItemCount[3 @ PartTurretPack] = 0;
  $TeamItemCount[4 @ PartTurretPack] = 0;
  $TeamItemCount[5 @ PartTurretPack] = 0;
  $TeamItemCount[6 @ PartTurretPack] = 0;
  $TeamItemCount[7 @ PartTurretPack] = 0;
}


BulletData PartBlast 
{
        bulletShapeName = "paint.dts";
	explosionTag = turretExp;
	collisionRadius = 0.0;
	mass = 2.0;
	damageClass = 1;
	damageValue = 0.27;
	damageType = $BlasterDamageType;
	explosionRadius = 5.0;
	kickBackStrength = 175.0;
	muzzleVelocity = 250.0;
	totalTime = 10;
	liveTime = 10;
	seekingTurningRadius = 6.97;
	nonSeekingTurningRadius = 45.0;
	proximityDist = 1.5;
	smokeDist = 0.0;
	lightRange = 5.0;
	lightColor = { 0.4, 0.4, 1.0 };
	inheritedVelocityScale = 0.5;
	soundId = SoundJetHeavy;
};

function PartBlast::updateTargetPercentage(%target) 
{
	return GameBase::virtual(%target, "getHeatFactor");
}

 //-=-=-=-

ItemImageData PartTurretPackImage
{
  shapeFile = "indoorgun";
  mountPoint = 2;
  mountOffset = { 0, -0.12, -0.1 };
  mountRotation = { 0, 0, 0 };
  mass = 2.5;
  firstPerson = false;
};

ItemData PartTurretPack
{
  description = "Particle Flak Cannon";
  shapeFile = "indoorgun";
  className = "Backpack";
  heading = $InvHead[ihDWe];
  imageType = PartTurretPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 25;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function PartTurretPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function PartTurretPack::onDeploy(%player,%item,%pos) 
{
  if (Turret::deployShape(%player, "Particle Flak Cannon (" @ Client::getName(Player::getClient(%player)) @ ")", DeployablePartTurret, %item, $TurretLocAnywhere))
    Player::decItemCount(%player,%item);
}

function PartTurretPack::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "The Particle Flak Cannon fires charged bolts of deadly energy at airbound targets.");
}

 //-=-=-=-

TurretData DeployablePartTurret
{
  className = "Turret";
  shapeFile = "hellfiregun";
  projectileType = PartBlast;
  maxDamage = 1.75;
  maxEnergy = 175;
  minGunEnergy = 5;
  maxGunEnergy = 2;
  sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
  reloadDelay = 0.33;
  speed = 50.0;
  speedModifier = 21.5;
  range = 180;
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
  explosionId = LargeShockwave;
  description = "Particle Flak Cannon";
  damageSkinData = "objectDamageSkins";
};

function DeployablePartTurret::verifyTarget(%this,%target) 
{
  if (GameBase::virtual(%target, "getHeatFactor") >= 0.5) return "True";
  else return "False";
}

function DeployablePartTurret::onAdd(%this)
{
  schedule("DeployablePartTurret::deploy(" @ %this @ ");",1,%this);
  GameBase::setRechargeRate(%this,5);
  %this.shieldStrength = 0.02;
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Particle Flak Cannon");
}

function DeployablePartTurret::deploy(%this) 
{
  GameBase::playSequence(%this,1,"deploy");
}

function DeployablePartTurret::onEndSequence(%this,%thread) 
{
  GameBase::setActive(%this,true);
}

function DeployablePartTurret::onDestroyed(%this) 
{
  Turret::onDestroyed(%this);
  $TeamItemCount[GameBase::getTeam(%this) @ "PartTurretPack"]--;
}

function DeployablePartTurret::onPower(%this,%power,%generator) 
{
//Turret::onPower(%this,%power,%generator)
}

function DeployablePartTurret::onEnabled(%this)
{
  GameBase::setRechargeRate(%this,5);
  GameBase::setActive(%this,true);
}


