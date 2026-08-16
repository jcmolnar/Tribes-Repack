StaticShapeData DefaultBeacon
{
	className = "Beacon";
	damageSkinData = "objectDamageSkins";

	shapeFile = "sensor_small";
	maxDamage = 0.1;
	maxEnergy = 200;

   castLOS = true;
   supression = false;
	mapFilter = 2;
	//mapIcon = "M_marker";
	visibleToSensor = true;
   explosionId = flashExpSmall;
	debrisId = flashDebrisSmall;
};
																						 
function Beacon::onEnabled(%this)
{
   GameBase::setIsTarget(%this,true);
}

function Beacon::onDisabled(%this)
{
   GameBase::setIsTarget(%this,false);
}

function Beacon::onDestroyed(%this)
{
   GameBase::setIsTarget(%this,false);
	$TeamItemCount[GameBase::getTeam(%this) @ "Beacon"]--;
}

// --------------------------------------------------
// DELTA FORCE

StaticShapeData DefaultCharge
{
	className = "Beacon";
	damageSkinData = "objectDamageSkins";

	shapeFile = "sensor_small";
	maxDamage = 0.1;
	maxEnergy = 200;

   castLOS = true;
   supression = false;
	mapFilter = 2;
	//mapIcon = "M_marker";
	visibleToSensor = true;
   explosionId = MineEXP;
	debrisId = flashDebrisSmall;
};
																						 
function Charge::onEnabled(%this)
{
	Gamebase::setIsTarget(%this, false);
	GameBase::setMapName(%this, "Charge");
	startCount(%this);
}

function Charge::onDisabled(%this) {}

function Charge::onDestroyed(%this)
{
	$TeamItemCount[GameBase::getTeam(%this) @ "Charge"]--;
}

function startCount(%object) 
{
	schedule("ChargeDetonate("@%object@");", 15, %object);
}

function ChargeDetonate(%object)
{
	CalcRadiusDamage(%object,$ChargeDamageType,20,0.2,25,20,20,20.5,10.1,200,100); 
	GameBase::applyRadiusDamage($ChargeDamageType, GameBase::getPosition(%object), 20,6,250,%object);
	Gamebase::setDamageLevel(%object, 0.1);
	$TeamItemCount[GameBase::getTeam(%this) @ "Charge"]--;
	echo("MSG: Charge has exploded");
}

// END DELTA FORCE
// --------------------------------------------------