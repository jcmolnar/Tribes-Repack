$InvList[StarCannonPack] = 1;
$RemoteInvList[StarCannonPack] = 1;

RocketData StarBlast
{
  bulletShapeName = "paint.dts";
  explosionTag = LargeShockwave;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.72;
  damageType = $ReaperDamageType;
  explosionRadius = 30.5;
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
  trailString = "paint.dts";
  smokeDist = 1.0;
  soundId = SoundJetHeavy;
};

GrenadeData StarDep
{
   bulletShapeName    = "breath.dts";
   explosionTag       = bulletExp0;
   collideWithOwner   = True;
   ownerGraceMS       = 150;
   collisionRadius    = 0.2;
   mass               = 1.0;
   elasticity         = 0.0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.0;
   damageType         = $mortarDamageType;

   explosionRadius    = 0;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 1.0;
   totalTime          = 0.001;    // special meaning for grenades...
   liveTime           = 0.01;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 0.125;

   smokeName          = "mortartrail.dts";
};

function StarDep::onAdd(%this)
{
	%rot = GameBase::getRotation(%this);
	%pos = GameBase::getPosition(%this);
	%spawnPos = Vector::getFromRot(%rot, 2.0);
	%pos = Vector::add(%pos, %spawnPos);

	%client = $FlyClientID;
	$FlyClientID = 0;

	//Spawn Star Cannon
	%obj = newObject("","Flier","FlyingStar",true);
	addToSet("MissionCleanup", %obj);
	$FlyingStarClient[%obj] = %client;
	GameBase::setTeam(%obj, GameBase::getTeam(%client));
	//echo("$FlyingStarClient[%obj] - Proj onAdd ",$FlyingStarClient[%obj]);
	//Position
	GameBase::setPosition(%obj, %pos);
	GameBase::setRotation(%obj, %rot);
	//Set Controls
	Client::setControlObject(%client,%obj);
}

ItemImageData StarCannonPackImage
{
	shapeFile = "shieldpack";
	mountPoint = 2;
	mountOffset = { 0, -0.1, -0.06 };
	mountRotation = { 0, 0, 0 };
	firstPerson = false;
};

ItemData StarCannonPack
{
	description = "Star Cannon";
	shapeFile = "shieldpack";
	className = "Backpack";
   heading = $InvHead[ihDwe];
	imageType = StarCannonPackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 100;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function StarCannonPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function StarCannonPack::onDeploy(%player,%item, %pos)
{
	%client = Player::getClient(%player);
	%item = "CameraPack";

	if( $TeamItemCount[GameBase::getTeam(%client) @ %item] < $TeamItemMax[%item] )
	{
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);
		$FlyClientID = %client;
		Projectile::spawnProjectile("StarDep",%trans,%player,%vel);

		Client::sendMessage(%client,0,"Star Cannon Deployed.");
		Player::trigger(%player,$BackpackSlot,false);

		%item = "StarCannonPack";
		Player::decItemCount(%player,%item);
	}
	else
	 	Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
}

function StarCannonPack::onUnmount(%player,%item)
{
	if (Player::getMountedItem(%player,$WeaponSlot) == StarCannonPackLauncher) {
		Player::unmountItem(%player,$WeaponSlot);
	}
}

function StarCannonPack::onDrop(%player,%item)
{
	if($matchStarted) {
		%mounted = Player::getMountedItem(%player,$WeaponSlot);
		if (%mounted == StarCannonPackLauncher) {
			Player::unmountItem(%player,$WeaponSlot);
		}
		else {
			// Remount the existing weapon to make sure the RepairGun
			// is not on the delayed mount "stack".
			Player::mountItem(%player,%mounted,$WeaponSlot);
		}
		Item::onDrop(%player,%item);
	}
}

function StarCannonPackImage::onActivate(%player,%imageSlot)
{
	%client = Player::getClient(%player);
	%item = "CameraPack";

	if( $TeamItemCount[GameBase::getTeam(%client) @ %item] < $TeamItemMax[%item] )
	{
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);
		$FlyClientID = %client;
		Projectile::spawnProjectile("StarDep",%trans,%player,%vel);

		Client::sendMessage(%client,0,"Star Cannon Deployed.");
		Player::trigger(%player,$BackpackSlot,false);

		%item = "StarCannonPack";
		Player::decItemCount(%player,%item);
	}
	else
	 	Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
}

$VehicleSlots[FlyingStar] = 0;

$DamageScale[FlyingStar, $ImpactDamageType] = 1.0;
$DamageScale[FlyingStar, $BulletDamageType] = 1.0;
$DamageScale[FlyingStar, $PlasmaDamageType] = 1.0;
$DamageScale[FlyingStar, $EnergyDamageType] = 1.0;
$DamageScale[FlyingStar, $ExplosionDamageType] = 1.0;
$DamageScale[FlyingStar, $ShrapnelDamageType] = 1.0;
$DamageScale[FlyingStar, $DebrisDamageType] = 1.0;
$DamageScale[FlyingStar, $MissileDamageType] = 1.0;
$DamageScale[FlyingStar, $LaserDamageType] = 1.0;
$DamageScale[FlyingStar, $MortarDamageType] = 1.0;
$DamageScale[FlyingStar, $BlasterDamageType] = 1.0;
$DamageScale[FlyingStar, $ElectricityDamageType] = 1.0;
$DamageScale[FlyingStar, $MineDamageType]        = 1.0;
$DamageScale[FlyingStar, $SniperDamageType]        = 1.0;
$DamageScale[FlyingStar, $PsiDamageType] = 1.0;
$DamageScale[FlyingStar, $ChemDamageType] = 1.0;
$DamageScale[FlyingStar, $KrakenDamageType] = 1.0;
$DamageScale[FlyingStar, $MeltaDamageType] = 1.0;
$DamageScale[FlyingStar, $DeathDamageType] = 1.0;
$DamageScale[FlyingStar, $DDamageType] = 1.0;
$DamageScale[FlyingStar, $FlamerDamageType] = 1.0;
$DamageScale[FlyingStar, $ShellDamageType] = 1.0;
$DamageScale[FlyingStar, $ShurikenDamageType] = 1.0;
$DamageScale[FlyingStar, $ReaperDamageType] = 1.0;

FlierData FlyingStar
{
	explosionId = debrisExpMedium;
	debrisId = defaultDebrisSmall;
	className = "Vehicle";
   shapeFile = "hellfiregun";
   shieldShapeName = "shield";
   mass = 100.0;
   drag = 2.0;
   density = 4.2;
   maxBank = 7.5;
   maxPitch = 7.5;
   maxSpeed = 5;
   minSpeed = 0;
	lift = 0.05;
	maxAlt = 4;
	maxVertical = 4;
	maxDamage = 5.84;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 1.0;

	groundDamageScale = 0.0;

	projectileType = StarBlast;

	reloadDelay = 2.0;
	repairRate = 0;
	fireSound = SoundFusionFire;
	damageSound = SoundFlierCrash;

	ramDamage = 0.25;
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	visibleToSensor = true;
	shadowDetailMask = 2;

	//mountSound = NoSound;
	//dismountSound = NoSound;
	idleSound = SoundElevatorStart;
	moveSound = SoundElevatorStart;

	visibleDriver = true;
	driverPose = 23;
	description = "Star Cannon";
};

function FlyingStar::onAdd(%this)
{
	schedule("checkOperator("@%this@");",5.0,%this);
}



//-=-=--=-=--=-=-=-=-
