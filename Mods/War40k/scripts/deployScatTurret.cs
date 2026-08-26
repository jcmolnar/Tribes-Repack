//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Scatter Las Turret
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$TeamItemMax[ScatTurretPack] = 1;
$InvList[ScatTurretPack] = 1;
$RemoteInvList[ScatTurretPack] = 1;

$CanControl[DeployableScatTurret] = 1;
$EmbedController[DeployableScatTurret] = 1;
$CanAlwaysTeamDestroy[DeployableScatTurret] = 1;

function deployScatTurret::Initialize()
{
  $TeamItemCount[0 @ ScatTurretPack] = 0;
  $TeamItemCount[1 @ ScatTurretPack] = 0;
  $TeamItemCount[2 @ ScatTurretPack] = 0;
  $TeamItemCount[3 @ ScatTurretPack] = 0;
  $TeamItemCount[4 @ ScatTurretPack] = 0;
  $TeamItemCount[5 @ ScatTurretPack] = 0;
  $TeamItemCount[6 @ ScatTurretPack] = 0;
  $TeamItemCount[7 @ ScatTurretPack] = 0;
}

LaserData ScatterLasBeam
{
   laserBitmapName   = "laserPulse.bmp";
   hitName           = "laserhit.dts";

   damageConversion  = 0.035;
   baseDamageType    = $LaserDamageType;

   beamTime          = 0.5;

   lightRange        = 1.0;
   lightColor        = { 0.25, 1.25, 0.25 };

   detachFromShooter = false;
   hitSoundId        = SoundLaserHit;
};
 //-=-=-=-

ItemImageData ScatTurretPackImage
{
  shapeFile = "remoteturret";
  mountPoint = 2;
  mountOffset = { 0, -0.12, -0.1 };
  mountRotation = { 0, 0, 0 };
  mass = 5.5;
  firstPerson = false;
};

ItemData ScatTurretPack
{
  description = "Scatter Las Turret";
  shapeFile = "remoteturret";
  className = "Backpack";
  heading = $InvHead[ihDWe];
  imageType = ScatTurretPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 140;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function ScatTurretPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function ScatTurretPack::onDeploy(%player,%item,%pos) 
{
  if (Turret::deployShape(%player, "Scatter Las Turret (" @ Client::getName(Player::getClient(%player)) @ ")", DeployableScatTurret, %item, $TurretLocAnywhere))
    Player::decItemCount(%player,%item);
}

function ScatTurretPack::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "The Scatter Las Turret is a display of the superior technology the Eldar possess.");
}
 //-=-=-=-

TurretData DeployableScatTurret
{
  className = "Turret";
  shapeFile = "hellfiregun";
  projectileType = ScatterLasBeam;
  maxDamage = 1.5;
  maxEnergy = 180;
  minGunEnergy = 1;
  maxGunEnergy = 5;
  sequenceSound[0] = {"deploy", SoundActivateMotionSensor };
  reloadDelay = 0.05;
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
  fireSound = SoundFireLas;
  activationSound = SoundPlasmaTurretOn;
  deactivateSound = SoundPlasmaTurretOff;
  whirSound = SoundPlasmaTurretTurn;
  explosionId = flashExpMedium;
  description = "Scatter Las Turret";
  damageSkinData = "objectDamageSkins";
};

function DeployableScatTurret::onAdd(%this)
{
  schedule("DeployableScatTurret::deploy(" @ %this @ ");",1,%this);
  GameBase::setRechargeRate(%this,5);
  %this.shieldStrength = 0.005;
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Scatter Las Turret");
}

function DeployableScatTurret::deploy(%this) 
{
  GameBase::playSequence(%this,1,"deploy");
}

function DeployableScatTurret::onEndSequence(%this,%thread) 
{
  GameBase::setActive(%this,true);
}

function DeployableScatTurret::onDestroyed(%this) 
{
  Turret::onDestroyed(%this);
  $TeamItemCount[GameBase::getTeam(%this) @ "ScatTurretPack"]--;
}

function DeployableScatTurret::onPower(%this,%power,%generator) 
{
}

function DeployableScatTurret::onEnabled(%this)
{
  GameBase::setRechargeRate(%this,25);
  GameBase::setActive(%this,true);
}

