// ===================yWing===================================================
$TeamItemMax[yWingVehicle] = 2;
// ===========================================================================
ItemData yWingVehicle
{
	description = "Y-Wing";
	className = "Vehicle";
   heading = "aRebel";
	price = 0;
	team=0;
};
// ===========================================================================
$TeamItemCount[0 @ yWingVehicle] = 0;
$TeamItemCount[1 @ yWingVehicle] = 0;
$TeamItemCount[2 @ yWingVehicle] = 0;
$TeamItemCount[3 @ yWingVehicle] = 0;
$TeamItemCount[4 @ yWingVehicle] = 0;
$TeamItemCount[5 @ yWingVehicle] = 0;
$TeamItemCount[6 @ yWingVehicle] = 0;
$TeamItemCount[7 @ yWingVehicle] = 0;

// ===========================================================================
$VehicleInvList[yWingVehicle] = 1;
$DataBlockName[yWingVehicle] = yWing;
$VehicleToItem[yWing] = yWingVehicle;


// ===========================================================================
FlierData yWing
{
		explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
    shapeFile = "yWing";
  shieldShapeName = "shield_medium";
   mass = 9.0;
   drag = 1.0;
   density = 1.2;
   maxBank = 0.5;
   maxPitch = 0.5;
   maxSpeed = 45;
   minSpeed = 0;
	lift = 0.45;
	maxAlt = 900;
	maxVertical = 10;
	maxDamage = 3.50;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 1.0;

	groundDamageScale = 8.0;
	minGunEnergy = 75;
	maxGunEnergy = 6;

	projectileType = yWingbombershell;
	reloadDelay = 0.7;
	repairRate = 0;
	fireSound = mineExplosion;
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
	description = "Y-Wing";
};

function yWing::onAdd(%this)
{
	%this.shieldStrength = 0.03;
	GameBase::setRechargeRate (%this, 5);
	GameBase::setMapName (%this, "yWing");
}

function yWing::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
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

function yWING::onCollision (%this, %object)
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
	Client::sendMessage(Player::getClient(%object),0,"Y-Wing Shield Damaged, Please Wait For Shield To Recharge.~wError_Message.wav");
	}
}




// ===========================================================================

$DamageScale[yWing, $ImpactDamageType] = 5.0;
$DamageScale[yWing, $EMPDamageType] = 1.5;
$DamageScale[yWing, $PlasmaDamageType] = 1.0;
$DamageScale[yWing, $EnergyDamageType] = 1.0;
$DamageScale[yWing, $ExplosionDamageType] = 2.0;
$DamageScale[yWing, $ShrapnelDamageType] = 2.0;
$DamageScale[yWing, $DebrisDamageType] = 3.5;
$DamageScale[yWing, $MissileDamageType] = 2.0;
$DamageScale[yWing, $LaserDamageType] = 2.0;
$DamageScale[yWing, $MortarDamageType] = 0.85;
$DamageScale[yWing, $BlasterDamageType] = 0.5;
$DamageScale[yWing, $BulletDamageType] = 1.5;
$DamageScale[yWing, $ElectricityDamageType] = 1.0;
$DamageScale[yWing, $MineDamageType]        = 1.0;
$DamageScale[yWing, $AWingDamageType]           = 2.0;
$DamageScale[yWing, $XWingDamageType]           = 2.0;
$DamageScale[yWing, $SNOWSPEEDERDamageType]     = 2.0;
$DamageScale[yWing, $TIEDamageType]             = 2.0;
$DamageScale[yWing, $TIEINTERCEPTORDamageType]  = 2.0;
$DamageScale[yWing, $TIEBOMBDamageType]         = 2.0;
$DamageScale[yWing, $yWingDamageType]           = 2.0;
$DamageScale[yWing, $MSaberDamageType] 	      = 50.0;
$DamageScale[yWing, $BSaberDamageType] 	      = 50.0;
$DamageScale[yWing, $GSaberDamageType] 	      = 50.0;
$DamageScale[yWing, $RSaberDamageType] 	      = 50.0;

// ===========================================================================
// ===========================================================================


