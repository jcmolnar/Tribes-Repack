// ===================TIE===================================================
$TeamItemMax[TieVehicle] = 3;
// ===========================================================================
ItemData TieVehicle
{
	description = "TIE Fighter";
	className = "Vehicle";
   heading = "bImperial";
	price = 0;
	team=1;
};
// ===========================================================================
$TeamItemCount[0 @ TIEVehicle] = 0;
$TeamItemCount[1 @ TIEVehicle] = 0;
$TeamItemCount[2 @ TIEVehicle] = 0;
$TeamItemCount[3 @ TIEVehicle] = 0;
$TeamItemCount[4 @ TIEVehicle] = 0;
$TeamItemCount[5 @ TIEVehicle] = 0;
$TeamItemCount[6 @ TIEVehicle] = 0;
$TeamItemCount[7 @ TIEVehicle] = 0;

// ===========================================================================
$VehicleInvList[TIEVehicle] = 1;
$DataBlockName[TIEVehicle] = TIE;
$VehicleToItem[TIE] = TIEVehicle;


// ===========================================================================
FlierData Tie
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
   shapeFile = "tie";
   shieldShapeName = "shield_medium";
   mass = 9.0;
   drag = 1.0;
   density = 1.2;
   maxBank = 0.9;
   maxPitch = 1.2;
   maxSpeed = 85;
   minSpeed = 0;
	lift = 0.75;
	maxAlt = 900;
	maxVertical = 10;
	maxDamage = 2.50;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 1.0;

	groundDamageScale = 1.0;

	projectileType = TIEBlast;
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
	description = "Tie Fighter";
};
function TIE::onAdd(%this)
{
	%this.shieldStrength = 0.02;
	GameBase::setRechargeRate (%this, 4);
	GameBase::setMapName (%this, "TIE");
}

function TIE::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
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

function TIE::onCollision (%this, %object)
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
	Client::sendMessage(Player::getClient(%object),0,"TIE Shield Damaged, Please Wait For It To Recharge.~wError_Message.wav");
	}
}


// ===========================================================================

$DamageScale[TIE, $ImpactDamageType] = 5.0;
$DamageScale[TIE, $EMPDamageType] = 1.5;
$DamageScale[TIE, $PlasmaDamageType] = 1.0;
$DamageScale[TIE, $EnergyDamageType] = 1.0;
$DamageScale[TIE, $ExplosionDamageType] = 2.0;
$DamageScale[TIE, $ShrapnelDamageType] = 2.0;
$DamageScale[TIE, $DebrisDamageType] = 3.0;
$DamageScale[TIE, $MissileDamageType] = 2.0;
$DamageScale[TIE, $BulletDamageType] = 1.5;
$DamageScale[TIE, $LaserDamageType] = 2.0;
$DamageScale[TIE, $MortarDamageType] = 1.0;
$DamageScale[TIE, $BlasterDamageType] = 0.5;
$DamageScale[TIE, $ElectricityDamageType] = 1.0;
$DamageScale[TIE, $MineDamageType]        = 1.0;
$DamageScale[TIE, $AWingDamageType]           = 2.0;
$DamageScale[TIE, $XWingDamageType]           = 2.0;
$DamageScale[TIE, $SNOWSPEEDERDamageType]     = 2.0;
$DamageScale[TIE, $TIEDamageType]             = 2.0;
$DamageScale[TIE, $TIEINTERCEPTORDamageType]  = 2.0;
$DamageScale[TIE, $TIEBOMBDamageType]         = 2.0;
$DamageScale[TIE, $YWINGDamageType]           = 2.0;
$DamageScale[TIE, $MSaberDamageType] 	     = 50.0;
$DamageScale[TIE, $BSaberDamageType] 	    = 50.0;
$DamageScale[TIE, $GSaberDamageType] 	    = 50.0;
$DamageScale[TIE, $RSaberDamageType] 	   = 50.0;

// ===========================================================================
// ===========================================================================



