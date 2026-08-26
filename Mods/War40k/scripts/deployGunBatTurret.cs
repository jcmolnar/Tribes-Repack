//Gun Battery: Original creation Edgecrusher
$TeamItemMax[GunBatPack] = 4;
$InvList[GunBatPack] = 1;
$RemoteInvList[GunBatPack] = 1;

$CanControl[DeployableGunBat] = 1;
$EmbedController[DeployableGunBat] = 1;
$CanAlwaysTeamDestroy[DeployableGunBat] = 1;

function deployGunBatTurret::Initialize()
{
  $TeamItemCount[0 @ GunBatPack] = 0;
  $TeamItemCount[1 @ GunBatPack] = 0;
  $TeamItemCount[2 @ GunBatPack] = 0;
  $TeamItemCount[3 @ GunBatPack] = 0;
  $TeamItemCount[4 @ GunBatPack] = 0;
  $TeamItemCount[5 @ GunBatPack] = 0;
  $TeamItemCount[6 @ GunBatPack] = 0;
  $TeamItemCount[7 @ GunBatPack] = 0;
}

RocketData AAGunBolt
{
  bulletShapeName = "mortar.dts";
  explosionTag = mortarExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.0;
  damageType = $MortarDamageType;
  explosionRadius = 20.0;
  kickBackStrength = 50.0;
  muzzleVelocity = 265.0;
  terminalVelocity = 465.0;
  acceleration = 5.0;
  totalTime = 10.0;
  liveTime = 1.0;
  lightRange = 2.0;
  lightColor = { 1.0, 0.7, 0.5 };
  inheritedVelocityScale = 0.5;
  trailType = 2;
  trailString = "plasmatrail.dts";
  smokeDist = 1.6;
  soundId = SoundJetHeavy;
};
function AAGunBolt::onAdd(%this)
{
	schedule("DeployThudds(" @ %this @ " , 1);",0.1,%this);//5++++++
}

function DeployThudds(%this, %count) 
{
	if(%count && %this)
	{
		%obj = newObject("","Mine","Thudd");
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj,%this, 0.3,false);//0++++	
		%obj = newObject("","Mine","Thudd");
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj,%this, 1.0,false);
		%obj = newObject("","Mine","Thudd");
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj,%this, 2.0,false);
		%obj = newObject("","Mine","Thudd1");
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj,%this, 3.0,false);
		%obj = newObject("","Mine","Thudd1");
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj,%this, 4.0,false);
		%obj = newObject("","Mine","Thudd2");
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj,%this, 7.0,false);
		%count -= 1;
		schedule("DeployThudds(" @ %this @ " , " @ %count @ ");",0.1,%this);//0.5++++++++
	}
}

MineData Thudd
{
  mass = 1.0;
  drag = 1.0;
  density = 1.0;
  elasticity = 0.1;
  friction = 0.2;
  className = "Handgrenade";
  description = "Handgrenade";
  shapeFile = "fusionbolt";
  shadowDetailMask = 4;
  explosionId = grenadeExp;
  explosionRadius = 20.0;
  damageValue = 0.35;
  damageType = $MortarDamageType;
  kickBackStrength = 250;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

function Thudd::onAdd(%this) 
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",1.0,%this);
}

MineData Thudd1
{
  mass = 0.5;
  drag = 0.45;
  density = 0.0;
  elasticity = 0.5;
  friction = 0.2;
  className = "Handgrenade";
  description = "Handgrenade";
  shapeFile = "fusionbolt";
  shadowDetailMask = 4;
  explosionId = mortarExp;
  explosionRadius = 15.0;
  damageValue = 0.55;
  damageType = $MortarDamageType;
  kickBackStrength = 250;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

function Thudd1::onAdd(%this) 
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",2.2,%this);
}

MineData Thudd2
{
  mass = 2.5;
  drag = 1.0;
  density = 1.5;
  elasticity = 0.1;
  friction = 0.0;
  className = "Handgrenade";
  description = "Handgrenade";
  shapeFile = "fusionbolt";
  shadowDetailMask = 4;
  explosionId = LargeShockwave;
  explosionRadius = 40.0;
  damageValue = 0.6;
  damageType = $MortarDamageType;
  kickBackStrength = 250;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

function Thudd2::onAdd(%this) 
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",1.4,%this);
}

ItemImageData GunBatPackImage 
{
  shapeFile = "remoteturret";
  mountPoint = 2;
  mountOffset = { 0, -0.12, -0.1 };
  mountRotation = { 0, 0, 0 };
  mass = 3.0;
  firstPerson = false;
};

ItemData GunBatPack 
{
  description = "Thudd Gun";
  shapeFile = "remoteturret";
  className = "Backpack";
  heading = $InvHead[ihDWe];
  imageType = GunBatPackImage;
  shadowDetailMask = 4;
  mass = 3.0;
  elasticity = 0.2;
  price = 65;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function GunBatPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function GunBatPack::onDeploy(%player,%item,%pos) 
{
  if (Turret::deployShape(%player, "Gun Battery (" @ Client::getName(Player::getClient(%player)) @ ")", DeployableGunBat, %item, $TurretLocAnywhere))
    Player::decItemCount(%player,%item);
}
function GunBatPack::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "Thudd Gun: Firing barrages of brutal fire into the enemy, nothing is more likely to decimate a squad.");
}

 //-=-=-=-

TurretData DeployableGunBat 
{
  className = "Turret";
  shapeFile = "hellfiregun";
  projectileType = AAGunBolt;
  maxDamage = 2.65;
  maxEnergy = 100;
  minGunEnergy = 40;
  maxGunEnergy = 30;
  sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
  reloadDelay = 3.5;
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
  activationSound = SoundRemoteTurretOn;
  deactivateSound = SoundRemoteTurretOff;
  explosionId = flashExpMedium;
  description = "Thudd Gun";
  damageSkinData = "objectDamageSkins";
};

function DeployableGunBat::onAdd(%this) 
{
  schedule("DeployableGunBat::deploy(" @ %this @ ");",1,%this);
  GameBase::setRechargeRate(%this,5);
  %this.shieldStrength = 0.02;
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Thudd Gun");
}

function DeployableGunBat::deploy(%this) 
{
  GameBase::playSequence(%this,1,"deploy");
}

function DeployableGunBat::onEndSequence(%this,%thread) 
{
  GameBase::setActive(%this,true);
}

function DeployableGunBat::onDestroyed(%this) 
{
  StaticShape::objectiveDestroyed(%this);
  %this.shieldStrength = 0;
  GameBase::setRechargeRate(%this,0);
  Turret::onDeactivate(%this);
  Turret::objectiveDestroyed(%this);
  CalcRadiusDamage(%this,$DebrisDamageType,20,0.2,25,20,20,1.0,1.1,200,100);
  $TeamItemCount[GameBase::getTeam(%this) @ "GunBatPack"]--;
}

function DeployableGunBat::onPower(%this,%power,%generator) 
{
}

function DeployableGunBat::onEnabled(%this) 
{
  GameBase::setRechargeRate(%this,20);
  GameBase::setActive(%this,true);
}

