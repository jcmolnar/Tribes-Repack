$TeamItemMax[SatchelPack] = 25;
// Satchel charges cannot be purchased at an inventory station

$CanAlwaysControl[DeployableSatchel] = 1;
$CanAlwaysTeamDestroy[DeployableSatchel] = 1;

GrenadeData SatchelShell 
{
  bulletShapeName = "fusionbolt.dts";
  explosionTag = LargeShockwave;
  collideWithOwner = True;
  ownerGraceMS = 250;
  collisionRadius = 0.2;
  mass = 1.0;
  elasticity = 0.45;
  damageClass = 1;
  damageValue = 1.4;
  damageType = $DeathDamageType;
  explosionRadius = 75;
  kickBackStrength = 350.0;
  maxLevelFlightDist = 1;
  totalTime = 30.0;
  liveTime = 0.01;
  projSpecialTime = 0.01;
  inheritedVelocityScale = 0.5;
  smokeName = "smoke.dts";
};

function deploySatchelCharge::Initialize()
{  
  $TeamItemCount[0 @ SatchelPack] = 0;
  $TeamItemCount[1 @ SatchelPack] = 0;
  $TeamItemCount[2 @ SatchelPack] = 0;
  $TeamItemCount[3 @ SatchelPack] = 0;
  $TeamItemCount[4 @ SatchelPack] = 0;
  $TeamItemCount[5 @ SatchelPack] = 0;
  $TeamItemCount[6 @ SatchelPack] = 0;
  $TeamItemCount[7 @ SatchelPack] = 0;
}

function DeploySatchel( %clientId, %player, %bec) 
{
  %item = "SatchelPack";
  %client = Player::getClient(%player);
  if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]) 
  {
    if (GameBase::getLOSInfo(%player,3)) 
    {
      %obj = getObjectType($los::object);
      %prot = GameBase::getRotation(%player);
      %zRot = getWord(%prot,2);
      if (Vector::dot($los::normal,"0 0 1") > 0.6) 
        %rot = "0 0 " @ %zRot;
      else 
      {
        if (Vector::dot($los::normal,"0 0 -1") > 0.6) 
          %rot = "3.14159 0 " @ %zRot;
        else 
          %rot = Vector::getRotation($los::normal);
      }
      if(checkDeployArea(%client,$los::position)) 
      {
        %camera = newObject("Camera","Turret",DeployableSatchel,true);
        addToSet("MissionCleanup", %camera);
        GameBase::setTeam(%camera,GameBase::getTeam(%player));
        GameBase::setRotation(%camera,%rot);
        GameBase::setPosition(%camera,$los::position);
        Gamebase::setMapName(%camera,"Fusion Explosive#"@ $totalNumCameras++ @ " " @ Client::getName(%client));
        Client::sendMessage(%client,0,"Fusion Explosive#"@ $totalNumCameras @ " deployed. Set it off from within the Commander Screen.");
        playSound(SoundPickupBackpack,$los::position);
        $TeamItemCount[GameBase::getTeam(%camera) @ "SatchelPack"]++;
//        reportDeploy(%camera, %client);
		echo("MSG: ",%client," deployed a Fusion Explosive");
        Player::decItemCount(%player,%bec);
        return true;
      }
    }
    else 
      Client::sendMessage(%client,0,"Deploy position out of range");
  }
  else 
    Client::sendMessage(%client,0,"Deployable Item limit reached for Satchel Charges");
  return false;
}

 //-=-=-=- 

TurretData DeployableSatchel 
{
  className = "Turret";
  shapeFile = "camera";
  projectileType = SatchelShell;
  maxDamage = 0.4;
  maxEnergy = 75;
  minGunEnergy = 10;
  maxGunEnergy = 60;
  sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
  reloadDelay = 10.0;
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
  fireSound = SoundFireLaser;
  activationSound = SoundRemoteTurretOn;
  deactivateSound = SoundRemoteTurretOff;
  explosionId = rocketExp;
  description = "Fusion Explosive";
  damageSkinData = "objectDamageSkins";
};

function DeployableSatchel::onAdd(%this) 
{
GameBase::startFadeout(%this);
  schedule("DeployableSatchel::deploy(" @ %this @ ");",1,%this);
  GameBase::setRechargeRate(%this,5);
  %this.shieldStrength = 0;
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Fusion Explosive");
}

function DeployableSatchel::deploy(%this) 
{
  GameBase::playSequence(%this,1,"deploy");
}

function DeployableSatchel::onEndSequence(%this,%thread) 
{
  GameBase::setActive(%this,true);
}

function DeployableSatchel::onDestroyed(%this) 
{
  StaticShape::objectiveDestroyed(%this);
  %this.shieldStrength = 0;
  GameBase::setRechargeRate(%this,0);
  Turret::onDeactivate(%this);
  Turret::objectiveDestroyed(%this);
  calcRadiusDamage(%this, $DebrisDamageType, 20,0.2,25,20,20,1.0,1.1,200,100);
  Projectile::spawnProjectile("SatchelShell",%trans,%player,%vel);
  $TeamItemCount[GameBase::getTeam(%this) @ "SatchelPack"]--;
}

function DeployableSatchel::onPower(%this,%power,%generator) 
{
}

function DeployableSatchel::onEnabled(%this) 
{
  GameBase::setRechargeRate(%this,5);
  GameBase::setActive(%this,true);
}
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
// Point Defense Mine
//-=-==-=-=-=-=-=-==-=-=-=-=-=-=-=
$TeamItemMax[PointdefPack] = 25;
// Point Def Mines cannot be purchased at an inventory station

$CanAlwaysControl[DeployablePointdef] = 1;
$CanAlwaysTeamDestroy[DeployablePointdef] = 1;

LaserData PointdefBlast
{
   laserBitmapName   = "paintpulse.bmp";
   hitName           = "laserhit.dts";

   damageConversion  = 0.009;
   baseDamageType    = $LaserDamageType;

   beamTime          = 1.0;

   lightRange        = 1.0;
   lightColor        = { 0.0, 1.25, 0.25 };

   detachFromShooter = false;
   hitSoundId        = SoundLaserHit;
};

function deployPointDef::Initialize()
{  
  $TeamItemCount[0 @ PointdefPack] = 0;
  $TeamItemCount[1 @ PointdefPack] = 0;
  $TeamItemCount[2 @ PointdefPack] = 0;
  $TeamItemCount[3 @ PointdefPack] = 0;
  $TeamItemCount[4 @ PointdefPack] = 0;
  $TeamItemCount[5 @ PointdefPack] = 0;
  $TeamItemCount[6 @ PointdefPack] = 0;
  $TeamItemCount[7 @ PointdefPack] = 0;
}

function DeployPointDef( %clientId, %player, %bec) 
{
  %item = "PointdefPack";
  %client = Player::getClient(%player);
  if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]) 
  {
    if (GameBase::getLOSInfo(%player,3)) 
    {
      %obj = getObjectType($los::object);
      %prot = GameBase::getRotation(%player);
      %zRot = getWord(%prot,2);
      if (Vector::dot($los::normal,"0 0 1") > 0.6) 
        %rot = "0 0 " @ %zRot;
      else 
      {
        if (Vector::dot($los::normal,"0 0 -1") > 0.6) 
          %rot = "3.14159 0 " @ %zRot;
        else 
          %rot = Vector::getRotation($los::normal);
      }
      if(checkDeployArea(%client,$los::position)) 
      {
        %camera = newObject("Camera","Turret",DeployablePointdef,true);
        addToSet("MissionCleanup", %camera);
        GameBase::setTeam(%camera,GameBase::getTeam(%player));
        GameBase::setRotation(%camera,%rot);
        GameBase::setPosition(%camera,$los::position);
        Gamebase::setMapName(%camera,"Point Defense#"@ $totalNumCameras++ @ " " @ Client::getName(%client));
        Client::sendMessage(%client,0,"Point Defense#"@ $totalNumCameras @ " deployed. Can be controlled via the command screen.");
        playSound(SoundPickupBackpack,$los::position);
        $TeamItemCount[GameBase::getTeam(%camera) @ "DeployablePointdef"]++;
//        reportDeploy(%camera, %client);
		echo("MSG: ",%client," deployed a Point Defense Unit");
        Player::decItemCount(%player,%bec);
        return true;
      }
    }
    else 
      Client::sendMessage(%client,0,"Deploy position out of range");
  }
  else 
    Client::sendMessage(%client,0,"Deployable Item limit reached for Point Defense Units");
  return false;
}

 //-=-=-=- 

TurretData DeployablePointDef 
{
  className = "Turret";
  shapeFile = "camera";
  projectileType = PointDefBlast;
  maxDamage = 0.4;
  maxEnergy = 75;
  minGunEnergy = 10;
  maxGunEnergy = 20;
  sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
  reloadDelay = 4.0;
  speed = 4.0;
  speedModifier = 1.5;
  range = 20;
  visibleToSensor = true;
  shadowDetailMask = 4;
  dopplerVelocity = 0;
  castLOS = true;
  supression = false;
  mapFilter = 2;
  mapIcon = "M_turret";
  debrisId = flashDebrisMedium;
  shieldShapeName = "shield";
  fireSound = SoundFireLaser;
  activationSound = SoundRemoteTurretOn;
  deactivateSound = SoundRemoteTurretOff;
  explosionId = rocketExp;
  description = "Point Defense Unit";
  damageSkinData = "objectDamageSkins";
};

function DeployablePointDef::onAdd(%this) 
{
  schedule("DeployablePointDef::deploy(" @ %this @ ");",1,%this);
  GameBase::setRechargeRate(%this,5);
  %this.shieldStrength = 0;
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Point Defense Unit");
}

function DeployablePointDef::deploy(%this) 
{
  GameBase::playSequence(%this,1,"deploy");
}

function DeployablePointDef::onEndSequence(%this,%thread) 
{
  GameBase::setActive(%this,true);
}

function DeployablePointDef::onDestroyed(%this) 
{
  StaticShape::objectiveDestroyed(%this);
  %this.shieldStrength = 0;
  GameBase::setRechargeRate(%this,0);
  Turret::onDeactivate(%this);
  Turret::objectiveDestroyed(%this);
  calcRadiusDamage(%this, $DebrisDamageType, 20,0.2,25,20,20,0.1,0.1,200,100);
  Projectile::spawnProjectile("PointDefBlast",%trans,%player,%vel);
  $TeamItemCount[GameBase::getTeam(%this) @ "PointdefPack"]--;
}

function DeployablePointDef::onPower(%this,%power,%generator) 
{
}

function DeployablePointDef::onEnabled(%this) 
{
  GameBase::setRechargeRate(%this,5);
  GameBase::setActive(%this,true);
}
