$InvList[EyePack] = 1;
$RemoteInvList[EyePack] = 1;

GrenadeData GodEye
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

function GodEye::onAdd(%this)
{
	%rot = GameBase::getRotation(%this);
	%pos = GameBase::getPosition(%this);
	%spawnPos = Vector::getFromRot(%rot, 2.0);
	%pos = Vector::add(%pos, %spawnPos);

	%client = $FlyClientID;
	$FlyClientID = 0;

	//Spawn Flying Bug Camera
	%obj = newObject("","Flier","FlyingEye",true);
	addToSet("MissionCleanup", %obj);
	$FlyingEyeClient[%obj] = %client;
	GameBase::setTeam(%obj, GameBase::getTeam(%client));
	//echo("$FlyingEyeClient[%obj] - Proj onAdd ",$FlyingEyeClient[%obj]);
	//Position
	GameBase::setPosition(%obj, %pos);
	GameBase::setRotation(%obj, %rot);
	//Set Controls
	Client::setControlObject(%client,%obj);
}

ItemImageData EyePackImage
{
	shapeFile = "shieldpack";
	mountPoint = 2;
	mountOffset = { 0, -0.1, -0.06 };
	mountRotation = { 0, 0, 0 };
	firstPerson = false;
};

ItemData EyePack
{
	description = "Eye of God";
	shapeFile = "shieldpack";
	className = "Backpack";
   heading = $InvHead[ihBac];
	imageType = EyePackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 100;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function EyePack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function EyePack::onDeploy(%player,%item, %pos)
{
	%client = Player::getClient(%player);
	%item = "CameraPack";

	if( $TeamItemCount[GameBase::getTeam(%client) @ %item] < $TeamItemMax[%item] )
	{
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);
		$FlyClientID = %client;
		Projectile::spawnProjectile("GodEye",%trans,%player,%vel);

		Client::sendMessage(%client,0,"The Eye of God opens...");
		Player::trigger(%player,$BackpackSlot,false);

		%item = "EyePack";
		Player::decItemCount(%player,%item);
	}
	else
	 	Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
}

function EyePack::onUnmount(%player,%item)
{
	if (Player::getMountedItem(%player,$WeaponSlot) == EyePackLauncher) {
		Player::unmountItem(%player,$WeaponSlot);
	}
}

function EyePack::onDrop(%player,%item)
{
	if($matchStarted) {
		%mounted = Player::getMountedItem(%player,$WeaponSlot);
		if (%mounted == EyePackLauncher) {
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

function EyePackImage::onActivate(%player,%imageSlot)
{
	%client = Player::getClient(%player);
	%item = "CameraPack";

	if( $TeamItemCount[GameBase::getTeam(%client) @ %item] < $TeamItemMax[%item] )
	{
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);
		$FlyClientID = %client;
		Projectile::spawnProjectile("GodEye",%trans,%player,%vel);

		Client::sendMessage(%client,0,"The eye of God opens...");
		Player::trigger(%player,$BackpackSlot,false);

		%item = "EyePack";
		Player::decItemCount(%player,%item);
	}
	else
	 	Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
}

$VehicleSlots[FlyingEye] = 2;

$DamageScale[FlyingEye, $ImpactDamageType] = 1.0;
$DamageScale[FlyingEye, $BulletDamageType] = 1.0;
$DamageScale[FlyingEye, $PlasmaDamageType] = 1.0;
$DamageScale[FlyingEye, $EnergyDamageType] = 1.0;
$DamageScale[FlyingEye, $ExplosionDamageType] = 1.0;
$DamageScale[FlyingEye, $ShrapnelDamageType] = 1.0;
$DamageScale[FlyingEye, $DebrisDamageType] = 1.0;
$DamageScale[FlyingEye, $MissileDamageType] = 1.0;
$DamageScale[FlyingEye, $LaserDamageType] = 1.0;
$DamageScale[FlyingEye, $MortarDamageType] = 1.0;
$DamageScale[FlyingEye, $BlasterDamageType] = 1.0;
$DamageScale[FlyingEye, $ElectricityDamageType] = 1.0;
$DamageScale[FlyingEye, $MineDamageType]        = 1.0;
$DamageScale[FlyingEye, $SniperDamageType]        = 1.0;
$DamageScale[FlyingEye, $PsiDamageType] = 1.0;
$DamageScale[FlyingEye, $ChemDamageType] = 1.0;
$DamageScale[FlyingEye, $KrakenDamageType] = 1.0;
$DamageScale[FlyingEye, $MeltaDamageType] = 1.0;
$DamageScale[FlyingEye, $DeathDamageType] = 1.0;
$DamageScale[FlyingEye, $DDamageType] = 1.0;
$DamageScale[FlyingEye, $FlamerDamageType] = 1.0;
$DamageScale[FlyingEye, $ShellDamageType] = 1.0;
$DamageScale[FlyingEye, $ShurikenDamageType] = 1.0;
$DamageScale[FlyingEye, $ReaperDamageType] = 1.0;

FlierData FlyingEye
{
	explosionId = debrisExpMedium;
	debrisId = defaultDebrisSmall;
	className = "Vehicle";
   shapeFile = "shieldpack";
   shieldShapeName = "shield";
   mass = 100.0;
   drag = 2.0;
   density = 4.2;
   maxBank = 7.5;
   maxPitch = 7.5;
   maxSpeed = 20;
   minSpeed = -12;
	lift = 0.05;
	maxAlt = 12;
	maxVertical = 12;
	maxDamage = 0.3;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 1.0;

	groundDamageScale = 0.0;

	projectileType = DarkRocket;

	reloadDelay = 0.5;
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
function FlyingEye::onAdd(%this)
{
	schedule("checkOperator("@%this@");",5.0,%this);
}

function FlyingEye::onFire(%this, %slot)
{
	%item = "CameraPack";
 	%client = GameBase::getControlClient(%this);

	if($TeamItemCount[GameBase::getTeam(%client) @ %item] < $TeamItemMax[%item]) {
		if (GameBase::getLOSInfo(%this,3)) {
			%obj = getObjectType($los::object);
			if (%obj == "SimTerrain" || %obj == "InteriorShape") {
				%prot = GameBase::getRotation(%this);
				%zRot = getWord(%prot,2);
				if (Vector::dot($los::normal,"0 0 1") > 0.6) {
					%rot = "0 0 " @ %zRot;
				}
				else {
					if (Vector::dot($los::normal,"0 0 -1") > 0.6) {
						%rot = "3.14159 0 " @ %zRot;
					}
					else {
						%rot = Vector::getRotation($los::normal);
					}
				}
				if(checkDeployArea(%client,$los::position)) {

					%camera = newObject("Camera","Turret",CameraTurret,true);
	   	      		addToSet("MissionCleanup", %camera);

					GameBase::setTeam(%camera,GameBase::getTeam(%client));
					GameBase::setRotation(%camera,%rot);
					GameBase::setPosition(%camera,$los::position);
					Gamebase::setMapName(%camera,"Camera#"@ $totalNumCameras++ @ " " @ Client::getName(%client));
					Client::sendMessage(%client,0,"Camera deployed");
					playSound(SoundPickupBackpack,$los::position);
					$TeamItemCount[GameBase::getTeam(%camera) @ "CameraPack"]++;
					echo("MSG: ",%client," deployed a Camera");

					//Exit and delete the Flier model
					%player = Client::getOwnedObject(%client);
					Client::setControlObject(%client, %player);
					
					schedule("deleteObject("@%this@");",0.05, %this);

					return true;
				}
			}
			else {
				Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");
			}
		}
		else {
			Client::sendMessage(%client,0,"Deploy position out of range");		
		}
	}
	else																						  
	 	Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
	
	return false;

}


