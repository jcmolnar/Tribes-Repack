// ===================Interceptor===================================================
$TeamItemMax[InterceptorVehicle] = 3;
// ===========================================================================
ItemData InterceptorVehicle
{
	description = "TIE Interceptor";
	className = "Vehicle";
   heading = "bImperial";
	price = 0;
	team=0;
};
// ===========================================================================
$TeamItemCount[0 @ InterceptorVehicle] = 0;
$TeamItemCount[1 @ InterceptorVehicle] = 0;
$TeamItemCount[2 @ InterceptorVehicle] = 0;
$TeamItemCount[3 @ InterceptorVehicle] = 0;
$TeamItemCount[4 @ InterceptorVehicle] = 0;
$TeamItemCount[5 @ InterceptorVehicle] = 0;
$TeamItemCount[6 @ InterceptorVehicle] = 0;
$TeamItemCount[7 @ InterceptorVehicle] = 0;

// ===========================================================================
$VehicleInvList[InterceptorVehicle] = 1;
$DataBlockName[InterceptorVehicle] = Interceptor;
$VehicleToItem[Interceptor] = InterceptorVehicle;


// ===========================================================================
FlierData Interceptor
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
   shapeFile = "interceptor";
   shieldShapeName = "shield_medium";
   mass = 9.0;
   drag = 1.0;
   density = 1.2;
   maxBank = 1.5;
   maxPitch = 0.9;
   maxSpeed = 100;
   minSpeed = 0;
	lift = 0.75;
	maxAlt = 900;
	maxVertical = 10;
	maxDamage = 3.00;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 1.0;

	groundDamageScale = 1.0;

	projectileType = TIEINTERCEPTORBlast;
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
	idleSound = SoundFlyerIdle;
	moveSound = SoundFlyerActive;

	visibleDriver = false;
	driverPose = 22;
	description = "Tie Interceptor";
};

function Interceptor::onAdd(%this)
{
	%this.shieldStrength = 0.02;
	GameBase::setRechargeRate (%this, 4);
	GameBase::setMapName (%this, "Interceptor");
}

function Interceptor::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
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

function Interceptor::onCollision (%this, %object)
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
	Client::sendMessage(Player::getClient(%object),0,"Interceptor Shield Damaged, Please Wait For It To Recharge.~wError_Message.wav");
	}
}

// ===========================================================================

$DamageScale[Interceptor, $ImpactDamageType] = 5.0;
$DamageScale[Interceptor, $EMPDamageType] = 1.5;
$DamageScale[Interceptor, $PlasmaDamageType] = 1.0;
$DamageScale[Interceptor, $EnergyDamageType] = 1.0;
$DamageScale[Interceptor, $ExplosionDamageType] = 2.0;
$DamageScale[Interceptor, $ShrapnelDamageType] = 2.0;
$DamageScale[Interceptor, $DebrisDamageType] = 3.0;
$DamageScale[Interceptor, $MissileDamageType] = 2.0;
$DamageScale[Interceptor, $LaserDamageType] = 2.0;
$DamageScale[Interceptor, $MortarDamageType] = 1.0;
$DamageScale[Interceptor, $BulletDamageType] = 1.5;
$DamageScale[Interceptor, $BlasterDamageType] = 0.5;
$DamageScale[Interceptor, $ElectricityDamageType] = 1.0;
$DamageScale[Interceptor, $MineDamageType]        = 1.0;
$DamageScale[Interceptor, $AWINGDamageType]           = 2.0;
$DamageScale[Interceptor, $XWINGDamageType]           = 2.0;
$DamageScale[Interceptor, $SNOWSPEEDERDamageType]     = 2.0;
$DamageScale[Interceptor, $TIEDamageType]             = 2.0;
$DamageScale[Interceptor, $TIEINTERCEPTORDamageType]  = 2.0;
$DamageScale[Interceptor, $TIEBOMBDamageType]         = 2.0;
$DamageScale[Interceptor, $YWINGDamageType]           = 2.0;
$DamageScale[Interceptor, $MSaberDamageType] 		= 50.0;
$DamageScale[Interceptor, $BSaberDamageType] 		= 50.0;
$DamageScale[Interceptor, $GSaberDamageType] 		= 50.0;
$DamageScale[Interceptor, $RSaberDamageType] 		= 50.0;
// ===========================================================================
// ===========================================================================


