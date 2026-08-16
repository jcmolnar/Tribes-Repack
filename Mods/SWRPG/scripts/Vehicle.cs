//----------------------------------------------------------------------------
// Space Vehicles
//     -Rebel
//     -Imperial
//     -Neutral
// Land vehicles
//     -Podracers
//     -Land speeders

$AccessoryVar[vehicles, $MiscInfo] = "Awing, Xwing, Ywing, SnowSpeeder, TieBomber, TieFighter, TieInterceptor, Scout, Banshiee, PodRacer1, SpeederBike";

//Rebel
$Type[Awing] = "Starfighter/Interceptor";
$Desc[Awing] = "The RZ-1 A-wing starfighter";
$Fac[Awing] = "Rebel Alliance";
$Type[Xwing] = "Starfighter";
$Desc[Xwing] = "The Incom T-65 X-wing starfighter";
$Fac[Xwing] = "Rebel Alliance";
$Type[Ywing] = "Starfighter/Bomber";
$Desc[Ywing] = "The BTL-S3 Y-wing Starfighter";
$Fac[Ywing] = "Rebel Alliance";
$Type[SnowSpeeder] = "Assault speeder";
$Desc[SnowSpeeder] = "The Incom T-47 Snowspeeder";
$Fac[SnowSpeeder] = "Rebel Alliance";

//Imperial
$Type[TieBomber] = "Starfighter/Bomber";
$Desc[TieBomber] = "The Sienar Fleet Systems TIE bomber";
$Fac[TieBomber] = "Galactic Empire";
$Type[TieFighter] = "Starfighter";
$Desc[TieFighter] = "The Sienar Fleet Systems TIE fighter";
$Fac[TieFighter] = "Galactic Empire";
$Type[TieInterceptor] = "Starfighter/Interceptor";
$Desc[TieInterceptor] = "The Sienar Fleet Systems TIE interceptor";
$Fac[TieInterceptor] = "Galactic Empire";

//Neutral
$Type[Scout] = "Air speeder";
$Desc[Scout] = "";
$Fac[Scout] = "Tribal";
$Type[Banshee] = "Air speeder";
$Desc[Banshee] = "";
$Fac[Banshee] = "Covenant";

//Podracers
$Type[PodRacer1] = "Podracer";
$Desc[PodRacer1] = "";
$Fac[PodRacer1] = "Neutral";

//Land speeders
$Type[SpeederBike] = "Scout speeder";
$Desc[SpeederBike] = "The 74-Z speeder bike";
$Fac[SpeederBike] = "Galactic Empire";

//****************************************************************************************************
//   Space Vehicles
//****************************************************************************************************

//*******************
//   Rebel
//*******************

FlierData Awing
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
	shapeFile = "Awing";
	shieldShapeName = "shield_medium";
	mass = 9.0;
	drag = 1.0;
	density = 1.2;
	maxBank = 157;
	maxPitch = 157;
	maxSpeed = 90;
	minSpeed = -50;
	lift = 0.5;
	maxAlt = 25;
	maxVertical = 10;
	maxDamage = 0.6;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 0.4;

	groundDamageScale = 1.0;

	projectileType = XwingBlast;
	reloadDelay = 0.2;
	repairRate = 0;
	fireSound = SoundFlyerFireLaser;
	damageSound = SoundFlierCrash;
	ramDamage = 1.5;
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	visibleToSensor = true;
	shadowDetailMask = 2;

	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundFlyerIdle;
	moveSound = SoundFlyerActive;

	visibleDriver = false;
	driverPose = 22;
	description = "A-wing";
};

FlierData Xwing
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
	shapeFile = "Xwing";
	shieldShapeName = "shield_medium";
	mass = 9.0;
	drag = 1.0;
	density = 1.2;
	maxBank = 0.5;
	maxPitch = 0.5;
	maxSpeed = 75;
	minSpeed = -2;
	lift = 0.5;
	maxAlt = 25;
	maxVertical = 10;
	maxDamage = 0.6;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 0.4;

	groundDamageScale = 1.0;

	projectileType = XwingBlast;
	reloadDelay = 0.2;
	repairRate = 0;
	fireSound = SoundFlyerFireLaser;
	damageSound = SoundFlierCrash;
	ramDamage = 1.5;
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	visibleToSensor = true;
	shadowDetailMask = 2;

	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundFlyerIdle;
	moveSound = SoundFlyerActive;

	visibleDriver = false;
	driverPose = 22;
	description = "X-wing";
};

FlierData Ywing
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
	shapeFile = "Ywing";
	shieldShapeName = "shield_medium";
	mass = 9.0;
	drag = 1.0;
	density = 1.2;
	maxBank = 0.5;
	maxPitch = 0.5;
	maxSpeed = 65;
	minSpeed = -2;
	lift = 0.5;
	maxAlt = 25;
	maxVertical = 10;
	maxDamage = 0.6;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 0.4;

	groundDamageScale = 1.0;

	projectileType = TIEBomberShell;
	reloadDelay = 0.7;
	repairRate = 0;
	fireSound = SoundFlyerFireLaser;
	damageSound = SoundFlierCrash;
	ramDamage = 1.5;
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	visibleToSensor = true;
	shadowDetailMask = 2;

	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundFlyerIdle;
	moveSound = SoundFlyerActive;

	visibleDriver = false;
	driverPose = 22;
	description = "Y-wing";
};

FlierData SnowSpeeder
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
	shapeFile = "snowspeeder";
	shieldShapeName = "shield_medium";
	mass = 30.0;
	drag = 1.0;
	density = 1.2;
	maxBank = 0.5;
	maxPitch = 0.5;
	maxSpeed = 75;
	minSpeed = -2;
	lift = 0.5;
	maxAlt = 25;
	maxVertical = 10;
	maxDamage = 0.6;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 0.4;

	groundDamageScale = 1.0;

	projectileType = XwingBlast;
	reloadDelay = 0.2;
	repairRate = 0;
	fireSound = SoundFlyerFireLaser;
	damageSound = SoundFlierCrash;
	ramDamage = 1.5;
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	visibleToSensor = true;
	shadowDetailMask = 2;

	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundFlyerIdle;
	moveSound = SoundFlyerActive;

	visibleDriver = false;
	driverPose = 22;
	description = "Snow Speeder";
};

//*******************
//   Imperial
//*******************

FlierData TieBomber
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
	shapeFile = "tiebomber";
	shieldShapeName = "shield_medium";
	mass = 9.0;
	drag = 1.0;
	density = 1.2;
	maxBank = 0.5;
	maxPitch = 0.5;
	maxSpeed = 65;
	minSpeed = -2;
	lift = 0.5;
	maxAlt = 25;
	maxVertical = 10;
	maxDamage = 0.6;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 0.4;

	groundDamageScale = 1.0;

	projectileType = TIEBomberShell;
	reloadDelay = 0.7;
	repairRate = 0;
	fireSound = SoundFlyerFireLaser;
	damageSound = SoundFlierCrash;
	ramDamage = 1.5;
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	visibleToSensor = true;
	shadowDetailMask = 2;

	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundFlyerIdle2;
	moveSound = SoundFlyerActive;

	visibleDriver = false;
	driverPose = 22;
	description = "Tie Bomber";
};

FlierData TieFighter
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
	shapeFile = "tie";
	shieldShapeName = "shield_medium";
	mass = 9.0;
	drag = 1.0;
	density = 1.2;
	maxBank = 0.5;
	maxPitch = 0.5;
	maxSpeed = 75;
	minSpeed = -2;
	lift = 0.5;
	maxAlt = 25;
	maxVertical = 10;
	maxDamage = 0.6;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 0.4;

	groundDamageScale = 1.0;

	projectileType = TIEBlast;
	reloadDelay = 0.2;
	repairRate = 0;
	fireSound = SoundFlyerFireLaser;
	damageSound = SoundFlierCrash;
	ramDamage = 1.5;
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	visibleToSensor = true;
	shadowDetailMask = 2;

	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundFlyerIdle2;
	moveSound = SoundFlyerActive;

	visibleDriver = false;
	driverPose = 22;
	description = "Tie Fighter";
};

FlierData TieInterceptor
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
	shapeFile = "interceptor";
	shieldShapeName = "shield_medium";
	mass = 9.0;
	drag = 1.0;
	density = 1.2;
	maxBank = 0.5;
	maxPitch = 0.5;
	maxSpeed = 90;
	minSpeed = -2;
	lift = 0.5;
	maxAlt = 25;
	maxVertical = 10;
	maxDamage = 0.6;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 0.4;

	groundDamageScale = 1.0;

	projectileType = TIEBlast;
	reloadDelay = 0.15;
	repairRate = 0;
	fireSound = SoundFlyerFireLaser;
	damageSound = SoundFlierCrash;
	ramDamage = 1.5;
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	visibleToSensor = true;
	shadowDetailMask = 2;

	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundFlyerIdle2;
	moveSound = SoundFlyerActive;

	visibleDriver = false;
	driverPose = 22;
	description = "Tie Interceptor";
};

//*******************
//   Neutral/alien
//*******************

FlierData Scout
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
	shapeFile = "flyer";
	shieldShapeName = "shield_medium";
	mass = 9.0;
	drag = 1.0;
	density = 1.2;
	maxBank = 0.5;
	maxPitch = 0.5;
	maxSpeed = 80;
	minSpeed = -2;
	lift = 0.5;
	maxAlt = 25;
	maxVertical = 10;
	maxDamage = 0.6;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 0.4;

	groundDamageScale = 1.0;

	projectileType = FlierRocket;
	reloadDelay = 2.0;
	repairRate = 0;
	fireSound = SoundFireFlierRocket;
	damageSound = SoundFlierCrash;
	ramDamage = 1.5;
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	visibleToSensor = true;
	shadowDetailMask = 2;

	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundFlyerIdle;
	moveSound = SoundFlyerActive;

	visibleDriver = true;
	driverPose = 22;
	description = "Scout";
};

FlierData Banshee
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
	shapeFile = "banshee";
	shieldShapeName = "shield_medium";
	mass = 9.0;
	drag = 1.0;
	density = 1.2;
	maxBank = 0.5;
	maxPitch = 0.5;
	maxSpeed = 80;
	minSpeed = -2;
	lift = 0.5;
	maxAlt = 25;
	maxVertical = 10;
	maxDamage = 0.6;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 0.4;

	groundDamageScale = 1.0;

	projectileType = MiniFusionBolt;
	reloadDelay = 0.15;
	repairRate = 0;
	fireSound = SoundFirePlasma;
	damageSound = SoundFlierCrash;
	ramDamage = 1.5;
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	visibleToSensor = true;
	shadowDetailMask = 2;

	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundFlyerIdle2;
	moveSound = SoundFlyerActive;

	visibleDriver = false;
	driverPose = 22;
	description = "Banshee";
};

FlierData Raft
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
	shapeFile = "raft_b";
	shieldShapeName = "shield_medium";
	mass = 9.0;
	drag = 1.0;
	density = 1.2;
	maxBank = 157;
	maxPitch = 157;
	maxSpeed = 90;
	minSpeed = -50;
	lift = 0.5;
	maxAlt = 25;
	maxVertical = 10;
	maxDamage = 0.6;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 0.4;

	groundDamageScale = 1.0;

	projectileType = XwingBlast;
	reloadDelay = 0.2;
	repairRate = 0;
	fireSound = SoundFlyerFireLaser;
	damageSound = SoundFlierCrash;
	ramDamage = 1.5;
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	visibleToSensor = true;
	shadowDetailMask = 2;

	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundFlyerIdle;
	moveSound = SoundFlyerActive;

	visibleDriver = false;
	driverPose = 22;
	description = "A-wing";
};

FlierData Longship
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
	shapeFile = "kl_longship";
	shieldShapeName = "shield_medium";
	mass = 9.0;
	drag = 1.0;
	density = 1.2;
	maxBank = 157;
	maxPitch = 157;
	maxSpeed = 90;
	minSpeed = -50;
	lift = 0.5;
	maxAlt = 25;
	maxVertical = 10;
	maxDamage = 0.6;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 0.4;

	groundDamageScale = 1.0;

	projectileType = XwingBlast;
	reloadDelay = 0.2;
	repairRate = 0;
	fireSound = SoundFlyerFireLaser;
	damageSound = SoundFlierCrash;
	ramDamage = 1.5;
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	visibleToSensor = true;
	shadowDetailMask = 2;

	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundFlyerIdle;
	moveSound = SoundFlyerActive;

	visibleDriver = false;
	driverPose = 22;
	description = "A-wing";
};

FlierData Longship2
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
	shapeFile = "longship";
	shieldShapeName = "shield_medium";
	mass = 9.0;
	drag = 1.0;
	density = 1.2;
	maxBank = 157;
	maxPitch = 157;
	maxSpeed = 90;
	minSpeed = -50;
	lift = 0.5;
	maxAlt = 25;
	maxVertical = 10;
	maxDamage = 0.6;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 0.4;

	groundDamageScale = 1.0;

	projectileType = XwingBlast;
	reloadDelay = 0.2;
	repairRate = 0;
	fireSound = SoundFlyerFireLaser;
	damageSound = SoundFlierCrash;
	ramDamage = 1.5;
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	visibleToSensor = true;
	shadowDetailMask = 2;

	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundFlyerIdle;
	moveSound = SoundFlyerActive;

	visibleDriver = false;
	driverPose = 22;
	description = "A-wing";
};

//****************************************************************************************************
//   Land Vehicles
//****************************************************************************************************

//*******************
//   Pod racers
//*******************
FlierData PodRacer1
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
	shapeFile = "flyer";
	shieldShapeName = "shield_medium";
	mass = 2.0;
	drag = 1.0;
	density = 1.2;
	maxBank = 1.57;
	maxPitch = 0.05;
	maxSpeed = 50;
	minSpeed = -2;
	lift = 0.1;
	maxAlt = 25;
	maxVertical = 0;
	maxDamage = 0.6;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 0.4;

	groundDamageScale = 1.0;

	//projectileType = MiniFusionBolt;
	//reloadDelay = 0.15;
	repairRate = 0;
	fireSound = SoundFirePlasma;
	damageSound = SoundFlierCrash;
	ramDamage = 1.5;
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	visibleToSensor = true;
	shadowDetailMask = 2;

	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundFlyerIdle2;
	moveSound = SoundFlyerActive;

	visibleDriver = false;
	driverPose = 22;
	description = "Pod Racer 1";
};

//*******************
//   Speeder bikes
//*******************

FlierData SpeederBike
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
	shapeFile = "SPDR";
	shieldShapeName = "shield_medium";
	mass = 9.0;
	drag = 1.0;
	density = 1.2;
	maxBank = 1;
	maxPitch = 0.5;
	maxSpeed = 50;
	minSpeed = -2;
	lift = 0.5;
	maxAlt = 20;
	maxVertical = 10;
	maxDamage = 0.6;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 0.4;

	groundDamageScale = 1.0;

	projectileType = XwingBlast;
	reloadDelay = 0.5;
	repairRate = 0.1;
	fireSound = SoundFlyerFireLaser;
	damageSound = SoundFlierCrash;
	ramDamage = 1.5;
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	visibleToSensor = true;
	shadowDetailMask = 2;

	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundFlyerIdle;
	moveSound = SoundFlyerActive;

	visibleDriver = true;
	driverPose = 22;
	description = "Speeder Bike";
};

//----------------------------------------------------------------------------

//Determine vehicle HP setup. Perhaps have it based on a player's stat, endurance or sommat, maybe piloting skill also,
//then use a local variable in the flierdata as a modifier, like hpmod = 1.5 for a bomber, or 0.8 for like an Awing/interceptor, 0.3 for speederbike, etc.

function Vehicle::onAdd(%this)
{
	%this.shieldStrength = 0.0;
	GameBase::setRechargeRate (%this, 10);
	GameBase::setMapName (%this, GameBase::getDataName(%this).description);
	%this.hp = * %this.hpmod;
}

function Vehicle::onCollision (%this, %object)
{

	if(%this.pilot != "")
		return;

	if(GameBase::getDamageLevel(%this) < (GameBase::getDataName(%this)).maxDamage)
	{
		echo("Vehicle::onCollision(" @ %this @ ", " @ %object @ ")");
		if (getObjectType (%object) == "Player" && (getSimTime() > %object.newMountTime || %object.lastMount != %this) && %this.fading == "")
		{
		      if( Player::isAiControlled(%object) )
				return;

			%shipId = %this;
			%armor = Player::getArmor(%object);
			%clientId = Player::getClient(%object);

			%name = Client::getName(%clientId);
			//%owner = $owner[%shipId];
			echo(%shipId);
			if(%you == "")//if(isInCommaList($grouplist[%owner], %name) || %name == %owner)
			{
				%dn = GameBase::getDataName(%this);
				if (Vehicle::canMount (%this, %object))	//(%armor == "larmor" || %armor == "lfemale") && 
				{
					%weapon = Player::getMountedItem(%object,$WeaponSlot);
					if(%weapon != -1)
					{
						%object.lastWeapon = %weapon;
						Player::unMountItem(%object,$WeaponSlot);
					}
					Player::setMountObject(%object, %this, 1);
					Client::setControlObject(%clientId, %this);
					playSound (GameBase::getDataName(%this).mountSound, GameBase::getPosition(%this));
					%object.driver = 1;
					%object.vehicle = %this;
					%this.clLastMount = %clientId;
					%this.pilot = %clientId;
				}
				else if(%dn != Awing && %dn != Xwing && %dn != Ywing && %dn != SnowSpeeder && %dn != TieBomber && %dn != TieFighter && %dn != TieInterceptor && %dn != SpeederBike && %dn != Scout && %dn != Banshee && %dn != PodRacer1) 
				{
				 	%mountSlot= Vehicle::findEmptySeat(%this,%clientId); 
					if(%mountSlot) 
					{
						%object.vehicleSlot = %mountSlot;
						%object.vehicle = %this;
						Player::setMountObject(%object, %this, %mountSlot);
						playSound (GameBase::getDataName(%this).mountSound, GameBase::getPosition(%this));
					}
				}
			}
			else
				Client::sendMessage(%clientId,0,"You are not allowed to operate nor ride this vehicle.~wError_Message.wav");
		}
	}
}

function Vehicle::findEmptySeat(%this,%clientId)
{
	if(GameBase::getDataName(%this) == HAPC)
		%numSlots = 4;
	else
		%numSlots = 2;
	%count=0;
	for(%i=0;%i<%numSlots;%i++)  
		if(%this.Seat[%i] == "") {
			%slotPos[%count] = Vehicle::getMountPoint(%this,%i+2);
			%slotVal[%count] = %i+2;
			%lastEmpty = %i+2;
			%count++;
		}
	if(%count == 1) {
		%this.Seat[%lastEmpty-2] = %clientId;
		return %lastEmpty;
	}
	else if (%count > 1)	{
		%freeSlot = %slotVal[getClosestPosition(%count,GameBase::getPosition(%clientId),%slotPos[0],%slotPos[1],%slotPos[2],%slotPos[3])];
		%this.Seat[%freeSlot-2] = %clientId;
		return %freeSlot;
	}
	else
		return "False";
}

function getClosestPosition(%num,%playerPos,%slotPos0,%slotPos1,%slotPos2,%slotPos3)
{
	%playerX = getWord(%playerPos,0);
	%playerY = getWord(%playerPos,1);
	for(%i = 0 ;%i<%num;%i++) {
		%x = (getWord(%slotPos[%i],0)) - %playerX;
		%y = (getWord(%slotPos[%i],1)) - %playerY;
		if(%x < 0)
			%x *= -1;
		if(%y < 0)
			%y *= -1;
		%newDistance = sqrt((%x*%x)+(%y*%y));
		if(%newDistance < %distance || %distance == "") {
	  		%distance = %newDistance;			
			%closePos = %i;	
		}
	}		
	return %closePos;
}

function Vehicle::passengerJump(%this,%passenger,%mom)
{
	%armor = Player::getArmor(%passenger);
//	if($ArmorPosInv[%armor] == 1) {
//		%height = 2;
//		%velocity = 70;
//		%zVec = 70;
//	}
//	else if($ArmorPosInv[%armor] == 2) {
//		%height = 2;
//		%velocity = 100;
//		%zVec = 100;
//	}
//	else if($ArmorPosInv[%armor] == 3) {
//		%height = 2;
//		%velocity = 140;
//		%zVec = 110;
//	}

	%height = 2;
	%velocity = 140;
	%zVec = 110;

	%pos = GameBase::getPosition(%passenger);
	%posX = getWord(%pos,0);
	%posY	= getWord(%pos,1);
	%posZ	= getWord(%pos,2);

	if(GameBase::testPosition(%passenger,%posX @ " " @ %posY @ " " @ (%posZ + %height))) {	
		%clientId = Player::getClient(%passenger);
		%this.Seat[%passenger.vehicleSlot-2] = "";
		%passenger.vehicleSlot = "";
	   %passenger.vehicle= "";
		Player::setMountObject(%passenger, -1, 0);
		%rotZ = getWord(GameBase::getRotation(%passenger),2);
		GameBase::setRotation(%passenger, "0 0 " @ %rotZ);
		GameBase::setPosition(%passenger,%posX @ " " @ %posY @ " " @ (%posZ + %height));
		%jumpDir = Vector::getFromRot(GameBase::getRotation(%passenger),%velocity,%zVec);
		Player::applyImpulse(%passenger,%jumpDir);
	}
	else
		Client::sendMessage(Player::getClient(%passanger),0,"Can not dismount - Obstacle in the way.~wError_Message.wav");
}

function Vehicle::jump(%this,%mom)
{
   Vehicle::dismount(%this,%mom);
}

function Vehicle::dismount(%this,%mom)
{
	%cl = GameBase::getControlClient(%this);
	if(%cl != -1)
	{
		%pl = Client::getOwnedObject(%cl);
		if(getObjectType(%pl) == "Player")
		{
		// dismount the player	  
			if(GameBase::testPosition(%pl, Vehicle::getMountPoint(%this,0)) || (%type = GameBase::getDataName(%this)) == Banshee || %type == Raft || %type == Longship || %type == Longship2)
			{ //make it just dismount and setPosition away.
				%pl.lastMount = %this;
				%pl.newMountTime = getSimTime() + 3.0;
				Player::setMountObject(%pl, %this, 0);
        	 			Player::setMountObject(%pl, -1, 0);
				if(%type == Banshee)
					gamebase::setPosition(%pl, vector::add(gameBase::getPosition(%this), "0 5 5"));
				%rot = GameBase::getRotation(%this);
				%rotZ = getWord(%rot,2);
				GameBase::setRotation(%pl, "0 0 " @ %rotZ);
				Player::applyImpulse(%pl,%mom);
        	 			Client::setControlObject(%cl, %pl);
				playSound (GameBase::getDataName(%this).dismountSound, GameBase::getPosition(%this));
				if(%pl.lastWeapon != "")
				{
					Player::useItem(%pl,%pl.lastWeapon);		 	
					%pl.lastWeapon = "";
      				}
				if(%pl.driver)
					%this.pilot = "";
				%pl.driver = "";
				%pl.vehicle = "";
			}
			else
				Client::sendMessage(%cl,0,"Can not dismount - Obstacle in the way.~wError_Message.wav");
		}
	}
}

function Vehicle::onDestroyed (%this,%mom)
{
//	if($testcheats || $servercheats)
	$TeamItemCount[GameBase::getTeam(%this) @ $VehicleToItem[GameBase::getDataName(%this)]]--;
	%cl = GameBase::getControlClient(%this);
	%pl = Client::getOwnedObject(%cl);

	//**RPG
	$owner[%this] = "";
	//**

	if(%pl != -1) {
	   Player::setMountObject(%pl, -1, 0);
   	Client::setControlObject(%cl, %pl);
		if(%pl.lastWeapon != "") {
			Player::useItem(%pl,%pl.lastWeapon);		 	
			%pl.lastWeapon = "";
		}
		%pl.driver = "";
	}
	for(%i = 0 ; %i < 4 ; %i++)
		if(%this.Seat[%i] != "") {
			%pl = Client::getOwnedObject(%this.Seat[%i]);
		   Player::setMountObject(%pl, -1, 0);
	  	 	Client::setControlObject(%this.Seat[%i], %pl);
		}
	calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 2, 0.55, 
		0.1, 225, 100); 
}

function Vehicle::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	decho("this:" @ %this @ " type:" @ %type @ " value:" @ %value @ " pos:" @ %pos @ " vec:" @ %vec @ " momentum:" @ %mom @ " obj:" @ %object);

	%time = getSimTime();
	if(%type == -1 && !(%time > %this.time + 1))
		return;

	//StaticShape::onDamage(%this,%type,%value,%pos,%vec,%mom,%object);
	%damageLevel = GameBase::getDamageLevel(%this);
	%dValue = %damageLevel + %value;
   %this.lastDamageObject = %object;
   %this.lastDamageTeam = GameBase::getTeam(%object);
	if(GameBase::getTeam(%this) == GameBase::getTeam(%object)) {
		%name = GameBase::getDataName(%this);
		if(%name.className == Generator || %name.className == Station) { 
			%TDS = $Server::TeamDamageScale;
			%dValue = %damageLevel + %value * %TDS;
			%disable = GameBase::getDisabledDamage(%this);
			if(!$Server::TourneyMode && %dValue > %disable - 0.05) {
            if(%damageLevel > %disable - 0.05)
               return;
            else
               %dValue = %disable - 0.05;
			}
		}
	}
	else
	{
		GameBase::setDamageLevel(%this,%dValue);
	}
}

function Vehicle::getHeatFactor(%this)
{
	// Not getting called right now because turrets don't track
	// vehicles.  A hack has been placed in Player::getHeatFactor.
   return 1.0;
}
