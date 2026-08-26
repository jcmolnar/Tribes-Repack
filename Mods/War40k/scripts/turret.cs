//---------------------------------------------------------------------------------
// REMOTE TURRET
//---------------------------------------------------------------------------------
$MaxNumTurretsInBox = 2;     //Number of remote turrets allowed in the area
$TurretBoxMaxLength = 25;    //Define Max Length of the area
$TurretBoxMaxWidth =  25;    //Define Max Width of the area
$TurretBoxMaxHeight = 25;    //Define Max Height of the area

$TurretBoxMinLength = 15;	  //Define Min Length from another turret
$TurretBoxMinWidth =  15;	  //Define Min Width from another turret
$TurretBoxMinHeight = 15;    //Define Min Height from another turret

RocketData TurretBlast 
{
  bulletShapeName = "mortar.dts";
  explosionTag = debrisExpLarge;
  expRandCycle = 1;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.4;
  damageType = $DDamageType;
  explosionRadius = 9.5;
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
  trailString = "lflame.dts";
  smokeDist = 1.8;
  soundId = SoundJetHeavy;
};

LightningData turretCharge 
{
  bitmapName = "lightningNew.bmp";
  damageType = $ElectricityDamageType;
  boltLength = 40.0;
  coneAngle = 35.0;
  damagePerSec = 0.06;
  energyDrainPerSec = 60.0;
  segmentDivisions = 4;
  numSegments = 5;
  beamWidth = 0.125;
  updateTime = 120;
  skipPercent = 0.5;
  displaceBias = 0.15;
  lightRange = 3.0;
  lightColor = { 0.25, 0.25, 0.85 };
  soundId = SoundELFFire;
};

function Lightning::damageTarget(%target, %timeSlice, %damPerSec, %enDrainPerSec, %pos, %vec, %mom, %shooterId) 
{
  %damVal = %timeSlice * %damPerSec;
  %enVal = %timeSlice * %enDrainPerSec;
  GameBase::applyDamage(%target, $ElectricityDamageType, %damVal, %pos, %vec, %mom, %shooterId);
  %energy = GameBase::getEnergy(%target);
  %energy = %energy - %enVal;
  if (%energy < 0) 
    %energy = 0;
  GameBase::setEnergy(%target, %energy);
}

RocketData MortarTurretShell 
{
  bulletShapeName = "shotgunbolt.dts";
  explosionTag = blasterExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.1;
  damageType = $DeathDamageType;
  explosionRadius = 6;
  kickBackStrength = 0.0;
  muzzleVelocity = 200.0;
  terminalVelocity = 200.0;
  acceleration = 5.0;
  totalTime = 1.6;
  liveTime = 1.6;
  lightRange = 5.0;
  lightColor = { 1.0, 0.7, 0.5 };
  inheritedVelocityScale = 0.5;
  trailType = 1;
  trailLength = 50;
  trailWidth = 0.3;
  soundId = SoundJetHeavy;
};

TurretData PlasmaTurret 
{
  maxDamage = 1.0;
  maxEnergy = 250;
  minGunEnergy = 12;
  maxGunEnergy = 15;
  reloadDelay = 1.324;
  fireSound = SoundPlasmaTurretFire;
  activationSound = SoundPlasmaTurretOn;
  deactivateSound = SoundPlasmaTurretOff;
  whirSound = SoundPlasmaTurretTurn;
  range = 100;
  dopplerVelocity = 0;
  castLOS = true;
  supression = false;
  mapFilter = 2;
  mapIcon = "M_turret";
  visibleToSensor = true;
  debrisId = defaultDebrisMedium;
  className = "Turret";
  shapeFile = "hellfiregun";
  shieldShapeName = "shield_medium";
  speed = 5.0;
  speedModifier = 2.0;
  projectileType = TurretBlast;
  damageSkinData = "objectDamageSkins";
  shadowDetailMask = 8;
  explosionId = LargeShockwave;
  description = "Shock Cannon";
};

TurretData ELFTurret 
{
  maxDamage = 1.0;
  maxEnergy = 130;
  minGunEnergy = 50;
  maxGunEnergy = 5;
  range = 60;
  visibleToSensor = true;
  dopplerVelocity = 0;
  castLOS = true;
  supression = false;
  mapFilter = 2;
  mapIcon = "M_turret";
  debrisId = defaultDebrisMedium;
  className = "Turret";
  shapeFile = "chainturret";
  shieldShapeName = "shield";
  speed = 5.0;
  speedModifier = 1.5;
  projectileType = turretCharge;
  reloadDelay = 0.3;
  explosionId = LargeShockwave;
  description = "ELF Turret";
  fireSound = SoundGeneratorPower;
  activationSound = SoundRemoteTurretOn;
  deactivateSound = SoundRemoteTurretOff;
  damageSkinData = "objectDamageSkins";
  shadowDetailMask = 8;
  isSustained = true;
  firingTimeMS = 750;
  energyRate = 30.0;
};

TurretData RocketTurret 
{
  maxDamage = 1.05;
  maxEnergy = 250;
  minGunEnergy = 20;
  maxGunEnergy = 25;
  reloadDelay = 2.0;
  range = 200;
  gunRange = 300;
  visibleToSensor = true;
  dopplerVelocity = 0;
  castLOS = true;
  supression = false;
  mapFilter = 2;
  mapIcon = "M_turret";
  debrisId = defaultDebrisLarge;
  className = "Turret";
  shapeFile = "missileturret";
  shieldShapeName = "shield_medium";
  speed = 2.0;
  speedModifier = 2.0;
  projectileType = TurretMissile;
  fireSound = SoundMissileTurretFire;
  activationSound = SoundMissileTurretOn;
  deactivateSound = SoundMissileTurretOff;
  damageSkinData = "objectDamageSkins";
  shadowDetailMask = 8;
  targetableFovRatio = 0.5;
  explosionId = LargeShockwave;
  description = "Rocket Turret";
};

function RocketTurret::onPower(%this,%power,%generator) 
{
  if (%power) 
  {
    %this.shieldStrength = 0.03;
    GameBase::setRechargeRate(%this,14);
  }
  else 
  {
    %this.shieldStrength = 0;
    GameBase::setRechargeRate(%this,0);
    Turret::disengageOperator(%this);
  }
  GameBase::setActive(%this,%power);
}

function RocketTurret::verifyTarget(%this,%target) 
{
  if (GameBase::virtual(%target, "getHeatFactor") >= 0.5) return "True";
  else return "False";
}

TurretData MortarTurret 
{
  maxDamage = 1.0;
  maxEnergy = 120;
  minGunEnergy = 4;
  maxGunEnergy = 8;
  reloadDelay = 0.5;
  fireSound = SoundMortarTurretFire;
  activationSound = SoundMortarTurretOn;
  deactivateSound = SoundMortarTurretOff;
  whirSound = SoundMortarTurretTurn;
  range = 150;
  dopplerVelocity = 0;
  castLOS = true;
  supression = false;
  mapFilter = 2;
  mapIcon = "M_turret";
  visibleToSensor = true;
  debrisId = defaultDebrisMedium;
  className = "Turret";
  shapeFile = "mortar_turret";
  shieldShapeName = "shield_medium";
  speed = 2.0;
  speedModifier = 2.0;
  projectileType = MortarTurretShell;
  damageSkinData = "objectDamageSkins";
  shadowDetailMask = 8;
  explosionId = LargeShockwave;
  description = "Ion Turret";
};

TurretData IndoorTurret 
{
  className = "Turret";
  shapeFile = "indoorgun";
  projectileType = MiniFusionBolt;
  maxDamage = 2.5;
  maxEnergy = 100;
  minGunEnergy = 8;
  maxGunEnergy = 4;
  reloadDelay = 0.4;
  speed = 5.0;
  speedModifier = 1.0;
  range = 25;
  visibleToSensor = true;
  dopplerVelocity = 2;
  castLOS = true;
  supression = false;
  supressable = false;
  pinger = false;
  mapFilter = 2;
  mapIcon = "M_turret";
  debrisId = defaultDebrisMedium;
  shieldShapeName = "shield";
  fireSound = SoundEnergyTurretFire;
  activationSound = SoundEnergyTurretOn;
  deactivateSound = SoundEnergyTurretOff;
  damageSkinData = "objectDamageSkins";
  shadowDetailMask = 8;
  explosionId = debrisExpMedium;
  description = "Fusion Turret";
};

 //-=-=-=-

$TurretStatusIdle = 0;
$TurretStatusRecalibrating = 1;
$TurretStatusAlert = 2;

function Turret::onAdd(%this) 
{
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Turret");
}

function Turret::onActivate(%this) 
{

  GameBase::playSequence(%this,0,power);
  %this.Status = $TurretStatusIdle;
}

function Turret::onDeactivate(%this) 
{
  GameBase::stopSequence(%this,0);
  Turret::disengageOperator(%this);
}

function Turret::onSetTeam(%this,%oldTeam) 
{
   // If team change was not controlled by the turret logic 
   // then assume objective was taken an force OrgTeam over.
  if (!%this.ControlledTeamChange)
    %this.OrgTeam = GameBase::getTeam(%this);
  else
    %this.ControlledTeamChange = False;

   // If the turret's team and the controllers team do not match
   // then spit the controller out.
  if(GameBase::getTeam(%this) != Client::getTeam(GameBase::getControlClient(%this))) 
    Turret::disengageOperator(%this);
}

function Turret::disengageOperator(%this) 
{
   // Basically spit any controllers out of the turret
  %cl = GameBase::getControlClient(%this); // get client from turret
  if (%cl != -1) 
  {
    %player = Client::getOwnedObject(%cl); // get player from client
    if (%player.ManualCommandTag)
    {
      %player.ManualCommandTag = False;
      %player.CommandTag = "";
    }
    Player::setMountObject(%player, -1,0); // Unmount the player (obj=-1)
    Client::setControlObject(%cl, %player); // Specify that the client is controlling the player body
  }
//  Client::setGuiMode(%cl,2); -- Go to the command screen
}

function Turret::onPower(%this,%power,%generator) 
{
  if (%power) 
  {
    %this.shieldStrength = 0.03;
    GameBase::setRechargeRate(%this,10);
  }
  else 
  {
    %this.shieldStrength = 0;
    GameBase::setRechargeRate(%this,0);
    Turret::disengageOperator(%this);
  }
  GameBase::setActive(%this,%power);
}

function Turret::onEnabled(%this) 
{
  if (GameBase::isPowered(%this)) 
  {
    %this.shieldStrength = 0.03;
    GameBase::setRechargeRate(%this,10);
    GameBase::setActive(%this,true);
  }
}

function Turret::onDisabled(%this) 
{
  %this.shieldStrength = 0;
  GameBase::setRechargeRate(%this,0);
  Turret::onDeactivate(%this);
}

function Turret::onDestroyed(%this) 
{
  StaticShape::objectiveDestroyed(%this);
  %this.shieldStrength = 0;
  GameBase::setRechargeRate(%this,0);
  Turret::onDeactivate(%this);
  Turret::objectiveDestroyed(%this);
  calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 9, 3, 0.40, 0.1, 200, 100);
}

function Turret::objectiveDestroyed(%this)
{
  //
}

function Turret::onDamage(%this,%type,%value,%pos,%vec,%mom,%object) 
{
  if(%this.objectiveLine) %this.lastDamageTeam = GameBase::getTeam(%object);
  %TDS= 1;
  if(GameBase::getTeam(%this) == GameBase::getTeam(%object)) 
  {
    %name = GameBase::getDataName(%this);
    if (!$CanAlwaysTeamDestroy[%name])
      %TDS = $Server::TeamDamageScale;
  }
  StaticShape::shieldDamage(%this,%type,%value * %TDS,%pos,%vec,%mom,%object);
}

function Turret::onManualControl(%this, %player) 
{
   // When a player jumps in
Client::sendMessage(Player::getClient(%player),1, "You manually control the Cannon.");
  %client = Player::getClient(%player);

  %player.ManualCommandTag = True;
  %player.CommandTag = 1;
  Client::takeControl(%client, %this);

   // Move the player onto the turret if appropriate
  %name = GameBase::getDataName(%this);
  if ($EmbedController[%name])
    GameBase::SetPosition(%player, GameBase::GetPosition(%this));
}

function Turret::onDismount (%this, %object) 
{
   // When a player ceases to control the turret (either jump or stop control)
Client::sendMessage(Player::getClient(%player),1, "You cease control of the Cannon.");
  %client = %object;
  Turret::disengageOperator(%this);
  %this.Status = $TurretStatusRecalibrating;
  schedule("Turret::onInternalDiagnostics(" @ %this @ ", " @ %object @ ");", 5, %this);
}

function Turret::Jump(%this, %mom) 
{
  %cl = GameBase::getControlClient(%this);
  Turret::onDismount(%this, %cl);
}

function Turret::onInternalDiagnostics(%this, %object)
{
   // Verify that we're on our original team
  if (GameBase::getTeam(%this) != %this.OrgTeam)
  {
    %this.Status = $TurretStatusAlert;
    GameBase::setTeam(%this, %this.OrgTeam);  
     // Spend additional time in alert if we had to correct our team
    schedule("Turret::onReturnToIdle(" @ %this @ ");", 10, %this);
  }
  else 
    Turret::onReturnToIdle(%this);
}

function Turret::onReturnToIdle(%this)
{
  %this.Status = $TurretStatusIdle;
}

function Turret::onCollision (%this, %object)
{
  %client = Player::getClient(%object);
  if (%this.CommandTag) return;

  %name = GameBase::getDataName(%this);
  if (getObjectType(%object) != "Player" || !$CanControl[%name]) return;

   // Still standing in the turret?
  if (GameBase::GetPosition(%this) == GameBase::GetPosition(%object)) return;

   // Verify the status of the turret
  if (Player::getMountedItem(%object,$BackpackSlot) == Laptop)
  {
    if (%this.Status == $TurretStatusRecalibrating)
    {
       Laptop::Error(%client, "Turret is recalibrating.");
       return;
    }
    else if (%this.Status == $TurretStatusAlert)
    {
       Laptop::Error(%client, "Turret is on alert.");
       return;
    }
  }
  else
  {
    if (%this.Status != $TurretStatusIdle)
    {
       Client::sendMessage(%client,0,"Turret is recharging.");
       return;
    }
  }

  if (GameBase::getTeam(%object) != GameBase::getTeam(%this))
  {
     // If the player has a laptop, then they can control anything
    if (Player::getMountedItem(%object,$BackpackSlot) != Laptop)
    {
      Client::sendMessage(%client,0,"--ACCESS DENIED-- Wrong Team.~waccess_denied.wav");
      return;
    }

     // Team change the turret (internal diagnostics will change it back once dismounted)
    %this.OrgTeam = GameBase::getTeam(%this);
    %this.ControlledTeamchange = True;
    GameBase::setTeam(%this, GameBase::getTeam(%client));

    Turret::onManualControl(%this, %object);
    Laptop::Output(%client, "Successful patch into enemy turret.");
  }
  else // Normal non-laptop connection to team turret
  {
    Turret::onManualControl(%this, %object);
    Client::sendMessage(%client, 0, "Manually controlling turret");
  }
}

function Turret::onAttemptControl(%this, %client)
{
   // If you're controlling via a Command station, this is the first entry into
   // Turret.  If you're running through collision, this is called above.
  %player = Client::getOwnedObject(%client);
  %name = GameBase::getDataName(%this);

   // Run some final checks
  if (!%player.CommandTag && !$CanAlwaysControl[%name])
  {
    if (Player::getMountedItem(%player, $BackpackSlot) != Laptop) 
    { 
      Client::SendMessage(%client, 0, "Need laptop or Command Station.");
      return;
    }
  }

   // Actually control the turret
  Client::setControlObject(%client, %this);
  Client::setGuiMode(%client, $GuiModePlay);
}

$TurretLocGroundOnly = 0;
$TurretLocAnywhere = 1;

function CountTurrets(%set,%num) 
{
  %count = 0;
  for(%i=0; %i < %num; %i++)
  {
    %obj=GameBase::getDataName(Group::getObject(%set,%i));
    if (%obj.ClassName == Turret && %obj.Range > 0) %count++;
  }
  return %count;
}

function Turret::deployShape(%player, %name, %shape, %item, %validloc)
{
  %client = Player::getClient(%player);

  if ($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]) 
  { Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s"); return; }

  if (!GameBase::getLOSInfo(%player, 3))
  { Client::sendMessage(%client,0,"Deploy position out of range"); return; }

  %obj = getObjectType($los::object);
  if (%obj != "SimTerrain" && %obj != "InteriorShape" && %obj == "DeployablePlatform")   
  { Client::sendMessage(%client,0,"Can only deploy on terrain or buildings"); return; }

  if (!GameBase::testPosition(%player, vector::add($los::position, "0 0 1")) && %validloc == $TurretLocGroundOnly)
  { Client::sendMessage(%client,0,"Turret does not fit there"); return; }

   // Check long range count (not applicable to manual turrets)
  if (%shape.Range > 0)
  {
    %set = newObject("set",SimSet);
    %num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMaxLength,$TurretBoxMaxWidth,$TurretBoxMaxHeight,0);
    %num = CountTurrets(%set, %num);
    deleteObject(%set);
    if (%num > $MaxNumTurretsInBox) 
    { Client::sendMessage(%client,0,"Interference from other remote turrets in the area"); return; }
  }

   // Check short range count
  %set = newObject("set",SimSet);
  %num = containerBoxFillSet(%set, $StaticObjectType, $los::position, $TurretBoxMinLength,$TurretBoxMinWidth,$TurretBoxMinHeight,0);
  %num = CountObjects(%set, %shape, %num);
  deleteObject(%set);
  if (%num) 
  { Client::sendMessage(%client,0,"Frequency Overload - Too close to other remote turrets"); return; }

   // Check slope of the floor
  if (%validloc == $TurretLocGroundOnly)
  {
    if (Vector::dot($los::normal,"0 0 1") <= 0.7) 
    { Client::sendMessage(%client,0,"Can only deploy on flat surfaces"); return; }
    %rot = GameBase::getRotation(%player);
  }
  else if (%validloc == $TurretLocAnywhere)
  {
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
  }
  else
    return;

   // Make sure this isn't colliding with other objects
  if (!checkDeployArea(%client,$los::position)) 
  { return; }

   // Passed validations, deploy
  %obj = newObject("hellfiregun", "Turret", %shape, true);
  addToSet("MissionCleanup", %obj);
  GameBase::setTeam(%obj, GameBase::getTeam(%player));
  GameBase::setPosition(%obj, $los::position);
  GameBase::setRotation(%obj, %rot);
  Gamebase::setMapName(%obj, %name);
  Client::sendMessage(%client, 0, %item.description @ " deployed");
  playSound(SoundPickupBackpack,$los::position);
  $TeamItemCount[GameBase::getTeam(%player) @ %item]++;
//  reportDeploy(%obj, %client);
	echo("MSG: ",%client," deployed a " @ %item.description);

 if ($TurretScoring) 
{      
Client::SetOwnedObject(%client,%obj);     Client::SetOwnedObject(%client,%player);  
}
return true;
}
