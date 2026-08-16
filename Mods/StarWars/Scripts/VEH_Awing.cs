// ===================AWing===================================================
$TeamItemMax[AWingVehicle] = 3;
// ===========================================================================
ItemData AWingVehicle
{
	description = "A-Wing";
	className = "Vehicle";
   heading = "aRebel";
	price = 0;
	team=0;
};
// ===========================================================================
$TeamItemCount[0 @ AWingVehicle] = 0;
$TeamItemCount[1 @ AWingVehicle] = 0;
$TeamItemCount[2 @ AWingVehicle] = 0;
$TeamItemCount[3 @ AWingVehicle] = 0;
$TeamItemCount[4 @ AWingVehicle] = 0;
$TeamItemCount[5 @ AWingVehicle] = 0;
$TeamItemCount[6 @ AWingVehicle] = 0;
$TeamItemCount[7 @ AWingVehicle] = 0;

// ===========================================================================
$VehicleInvList[AWingVehicle] = 1;
$DataBlockName[AWingVehicle] = AWing;
$VehicleToItem[AWing] = AWingVehicle;


// ===========================================================================
FlierData AWING
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
    shapeFile = "awing";
   shieldShapeName = "shield_medium";
     
   mass = 9.0;
   drag = 1.0;
   density = 1.2;
   maxBank = 1.2;
   maxPitch = 0.9;
   maxSpeed = 100;
   minSpeed = 0;
	lift = 0.75;
	maxAlt = 900;
	maxVertical = 10;
	maxDamage = 2.00;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 1.0;

	groundDamageScale = 7.5;
	minGunEnergy = 75;
	maxGunEnergy = 6;

	projectileType = AWINGBlast;
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
	description = "A-Wing";
};
function AWing::onAdd(%this)
{
	%this.shieldStrength = 0.02;
	GameBase::setRechargeRate (%this, 4);
	GameBase::setMapName (%this, "AWing");
	
}

function AWing::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
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

function AWING::onCollision (%this, %object)
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
	Client::sendMessage(Player::getClient(%object),0,"A-Wing Shield Damaged, Please Wait For Shield To Recharge.~wError_Message.wav");
	}
}



// ===========================================================================

$DamageScale[AWing, $ImpactDamageType] = 5.0;
$DamageScale[AWing, $EMPDamageType] = 1.5;
$DamageScale[AWing, $PlasmaDamageType] = 2.0;
$DamageScale[AWing, $EnergyDamageType] = 1.0;
$DamageScale[AWing, $BulletDamageType] = 1.5;
$DamageScale[AWing, $ExplosionDamageType] = 2.0;
$DamageScale[AWing, $ShrapnelDamageType] = 2.0;
$DamageScale[AWing, $DebrisDamageType] = 3.0;
$DamageScale[AWing, $MissileDamageType] = 2.0;
$DamageScale[AWing, $LaserDamageType] = 2.0;
$DamageScale[AWing, $MortarDamageType] = 0.85;
$DamageScale[AWing, $BlasterDamageType] = 0.5;
$DamageScale[AWing, $ElectricityDamageType] = 1.0;
$DamageScale[AWing, $MineDamageType]        = 1.0;
$DamageScale[AWing, $AWingDamageType]           = 2.0;
$DamageScale[AWing, $XWingDamageType]           = 2.0;
$DamageScale[AWing, $SNOWSPEEDERDamageType]     = 2.0;
$DamageScale[AWing, $TIEDamageType]             = 2.0;
$DamageScale[AWing, $TIEINTERCEPTORDamageType]  = 2.0;
$DamageScale[AWing, $TIEBOMBDamageType]         = 2.0;
$DamageScale[AWing, $AWingDamageType]           = 2.0;
$DamageScale[AWing, $MSaberDamageType] 	      = 50.0;
$DamageScale[AWing, $BSaberDamageType] 	      = 50.0;
$DamageScale[AWing, $GSaberDamageType] 	     = 50.0;
$DamageScale[AWing, $RSaberDamageType] 	      = 50.0;

// ===========================================================================
// ===========================================================================






