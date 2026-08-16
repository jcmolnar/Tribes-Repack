// ===================TieBomb===================================================
$TeamItemMax[TieBombVehicle] = 2;
// ===========================================================================
ItemData TieBombVehicle
{
	description = "TIE Bomber";
	className = "Vehicle";
   heading = "bImperial";
	price = 0;
	team=1;
};
// ===========================================================================
$TeamItemCount[0 @ TieBombVehicle] = 0;
$TeamItemCount[1 @ TieBombVehicle] = 0;
$TeamItemCount[2 @ TieBombVehicle] = 0;
$TeamItemCount[3 @ TieBombVehicle] = 0;
$TeamItemCount[4 @ TieBombVehicle] = 0;
$TeamItemCount[5 @ TieBombVehicle] = 0;
$TeamItemCount[6 @ TieBombVehicle] = 0;
$TeamItemCount[7 @ TieBombVehicle] = 0;

// ===========================================================================
$VehicleInvList[TieBombVehicle] = 1;
$DataBlockName[TieBombVehicle] = TieBomb;
$VehicleToItem[TieBomb] = TieBombVehicle;


// ===========================================================================
FlierData TieBomb
{
		explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
    shapeFile = "TieBomber";
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
	maxDamage = 3.00;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 1.0;

	groundDamageScale = 1.0;
	minGunEnergy = 75;
	maxGunEnergy = 6;

	projectileType = Tiebombershell;

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
	description = "TIE Bomber";
};

function TieBomb::onAdd(%this)
{
	%this.shieldStrength = 0.02;
	GameBase::setRechargeRate (%this, 5);
	GameBase::setMapName (%this, "TieBomber");
}

function TIEBomb::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	if (%type == $EMPDamageType)	
	{
		
		GameBase::setEnergy(%this, 0);		
		Vehicle::dismount(%this, %mom);
	}
	
	else
	{
		%value *= $damageScale[GameBase::getDataName(%this), %type];
		StaticShape::shieldDamage(%this,%type,%value,%pos,%vec,%mom,%object);
	}
}

function TIEBomb::onCollision (%this, %object)
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
				if ((%armor == "bbarmor" || %armor == "charmor" || %armor == "tharmor" || %armor == "rebeltroop" || %armor == "rebelCommand" || %armor == "hsarmor" || %armor == "luarmor" || %armor == "dtarmor" || %armor == "larmor" || %armor == "harmor" || %armor == "marmor" || %armor == "sluarmor" || %armor == "rparmor" || %armor == "darmor" || %armor == "drarmor" || %armor == "dvarmor" || %armor == "xarmor" || %armor == "plarmor" || %armor == "htarmor" ) && Vehicle::canMount (%this, %object))
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
	Client::sendMessage(Player::getClient(%object),0,"TIE Bomber Shield Damaged, Please Wait For It To Recharge.~wError_Message.wav");
	}
}

// ===========================================================================

$DamageScale[TieBomb, $ImpactDamageType] = 5.0;
$DamageScale[TieBomb, $EMPDamageType] = 1.5;
$DamageScale[TieBomb, $PlasmaDamageType] = 1.0;
$DamageScale[TieBomb, $EnergyDamageType] = 1.0;
$DamageScale[TieBomb, $ExplosionDamageType] = 2.0;
$DamageScale[TieBomb, $ShrapnelDamageType] = 2.0;
$DamageScale[TieBomb, $BulletDamageType] = 1.5;
$DamageScale[TieBomb, $DebrisDamageType] = 3.5;
$DamageScale[TieBomb, $MissileDamageType] = 2.0;
$DamageScale[TieBomb, $LaserDamageType] = 2.0;
$DamageScale[TieBomb, $MortarDamageType] = 1.0;
$DamageScale[TieBomb, $BlasterDamageType] = 0.5;
$DamageScale[TieBomb, $ElectricityDamageType] = 1.0;
$DamageScale[TieBomb, $MineDamageType]        = 1.0;
$DamageScale[TieBomb, $AWingDamageType]           = 2.0;
$DamageScale[TieBomb, $XWingDamageType]           = 2.0;
$DamageScale[TieBomb, $SNOWSPEEDERDamageType]     = 2.0;
$DamageScale[TieBomb, $TIEDamageType]             = 2.0;
$DamageScale[TieBomb, $TIEINTERCEPTORDamageType]  = 2.0;
$DamageScale[TieBomb, $TIEBOMBDamageType]         = 2.0;
$DamageScale[TieBomb, $YWingDamageType]           = 2.0;
$DamageScale[TieBomb, $TieBombDamageType]           = 2.0;
$DamageScale[TieBomb, $MSaberDamageType] 	      = 50.0;
$DamageScale[TieBomb, $BSaberDamageType] 	      = 50.0;
$DamageScale[TieBomb, $GSaberDamageType] 	      = 50.0;
$DamageScale[TieBomb, $RSaberDamageType] 	     = 50.0;

// ===========================================================================
// ===========================================================================

