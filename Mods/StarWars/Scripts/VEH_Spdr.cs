// ===================SnowSpeeder===================================================
$TeamItemMax[SnowSpeederVehicle] = 3;
// ===========================================================================
ItemData SnowSpeederVehicle
{
	description = "Snow Speeder";
	className = "Vehicle";
   heading = "aRebel";
	price = 0;
	team=0;
};
// ===========================================================================
$TeamItemCount[0 @ SnowSpeederVehicle] = 0;
$TeamItemCount[1 @ SnowSpeederVehicle] = 0;
$TeamItemCount[2 @ SnowSpeederVehicle] = 0;
$TeamItemCount[3 @ SnowSpeederVehicle] = 0;
$TeamItemCount[4 @ SnowSpeederVehicle] = 0;
$TeamItemCount[5 @ SnowSpeederVehicle] = 0;
$TeamItemCount[6 @ SnowSpeederVehicle] = 0;
$TeamItemCount[7 @ SnowSpeederVehicle] = 0;

// ===========================================================================
$VehicleInvList[SnowSpeederVehicle] = 1;
$DataBlockName[SnowSpeederVehicle] = SnowSpeeder;
$VehicleToItem[SnowSpeeder] = SnowSpeederVehicle;


// ===========================================================================
FlierData SnowSpeeder
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
   shapeFile = "snowspeeder";
   shieldShapeName = "shield_medium";
   mass = 9.0;
   drag = 1.0;
   density = 1.6;
   maxBank = 1.7;
   maxPitch = 1.3;
   maxSpeed = 75;
   minSpeed = 0;
	lift = 0.75;
	maxAlt = 900;
	maxVertical = 10;
	maxDamage = 2.60;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 1.0;

	groundDamageScale = 1.0;

	projectileType = SnowspeederBlast;
	reloadDelay = 0.2;
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
	idleSound = SoundFlyerIdle;
	moveSound = SoundFlyerActive;

	visibleDriver = false;
	driverPose = 22;
	description = "Snow Speeder";
};

function SnowSpeeder::onAdd(%this)
{
	%this.shieldStrength = 0.01;
	GameBase::setRechargeRate (%this, 4);
	GameBase::setMapName (%this, "SnowSpeeder");
}

function SnowSpeeder::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
%energy = GameBase::getEnergy(%this);
%value *= $damageScale[GameBase::getDataName(%this), %type];
StaticShape::shieldDamage(%this,%type,%value,%pos,%vec,%mom,%object);

	if (%type == $EMPDamageType)
		{
		if (%energy < 15)
		{
			GameBase::setEnergy(%this, 0);		
			Vehicle::dismount(%this, %mom);
		}
		}
	
	
	
}

function SnowSpeeder::onCollision (%this, %object)
{
%energy = GameBase::getEnergy(%this);
if (%energy == 100)
	{
	if(GameBase::getDamageLevel(%this) < (GameBase::getDataName(%this)).maxDamage) {
		if (getObjectType (%object) == "Player" && %object.vehicle == "" && (getSimTime() > %object.newMountTime || %object.lastMount != %this) && %this.fading == "")
			{
           if( Player::isAiControlled(%object) )
               return;
               
				%armor = Player::getArmor(%object);
		      %client = Player::getClient(%object);
				if ((%armor == "bbarmor" || %armor == "charmor" || %armor == "tharmor" || %armor == "rebeltroop" || %armor == "rebelCommand" || %armor == "hsarmor" || %armor == "luarmor" || %armor == "dtarmor" || %armor == "larmor" || %armor == "harmor" || %armor == "marmor" || %armor == "sluarmor" || %armor == "rparmor" || %armor == "darmor" ||%armor == "drarmor" || %armor == "dvarmor" || %armor == "xarmor" || %armor == "plarmor" || %armor == "htarmor" ) && Vehicle::canMount (%this, %object))
					{
						%weapon = Player::getMountedItem(%object,$WeaponSlot);
						if(%weapon != -1) {
							%object.lastWeapon = %weapon;
							Player::unMountItem(%object,$WeaponSlot);
						}
						Player::setMountObject(%object, %this, 1);
				    		Client::setControlObject(%client, %this);
						playSound (GameBase::getDataName(%this).mountSound, GameBase::getPosition(%this));
						%object.driver= 1;
		            %object.vehicle = %this;
						%this.clLastMount = %client;
					}
				else if (GameBase::getControlClient(%this) == -1)
					Client::sendMessage(Player::getClient(%object),0,"You must be in Light Armor to pilot the vehicles.~wError_Message.wav");
			}
		}
	}
else
	{
	Client::sendMessage(Player::getClient(%object),0,"Speeder Shield Damaged, Please Wait For Shield To Recharge.~wError_Message.wav");
	}
}




// ===========================================================================

$DamageScale[SnowSpeeder, $ImpactDamageType] = 5.0;
$DamageScale[SnowSpeeder, $EMPDamageType] = 1.5;
$DamageScale[SnowSpeeder, $PlasmaDamageType] = 1.0;
$DamageScale[SnowSpeeder, $EnergyDamageType] = 1.0;
$DamageScale[SnowSpeeder, $ExplosionDamageType] = 2.0;
$DamageScale[SnowSpeeder, $ShrapnelDamageType] = 2.0;
$DamageScale[SnowSpeeder, $DebrisDamageType] = 3.0;
$DamageScale[SnowSpeeder, $MissileDamageType] = 2.0;
$DamageScale[SnowSpeeder, $BulletDamageType] = 1.5;
$DamageScale[SnowSpeeder, $LaserDamageType] = 2.0;
$DamageScale[SnowSpeeder, $MortarDamageType] = 0.85;
$DamageScale[SnowSpeeder, $BlasterDamageType] = 0.5;
$DamageScale[SnowSpeeder, $ElectricityDamageType] = 1.0;
$DamageScale[SnowSpeeder, $MineDamageType]        = 1.0;
$DamageScale[SnowSpeeder, $SnowSpeederDamageType] = 2.0;
$DamageScale[SnowSpeeder, $AWingDamageType]     = 2.0;
$DamageScale[SnowSpeeder, $XWingDamageType]     = 2.0;
$DamageScale[SnowSpeeder, $TIEDamageType]             = 2.0;
$DamageScale[SnowSpeeder, $TIEINTERCEPTORDamageType]  = 2.0;
$DamageScale[SnowSpeeder, $TIEBOMBDamageType]         = 2.0;
$DamageScale[SnowSpeeder, $YWINGDamageType]           = 2.0;
$DamageScale[SnowSpeeder, $MSaberDamageType] 	     = 50.0;
$DamageScale[SnowSpeeder, $BSaberDamageType] 	     = 50.0;
$DamageScale[SnowSpeeder, $GSaberDamageType] 	    = 50.0;
$DamageScale[SnowSpeeder, $RSaberDamageType] 	    = 50.0;

// ===========================================================================
// ===========================================================================



