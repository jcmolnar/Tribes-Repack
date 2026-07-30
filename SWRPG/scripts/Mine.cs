//----------------------------------------------------------------------------
// MINE DYNAMIC DATA

MineData AntipersonelMine
{
	className = "Mine";
	description = "Antipersonel Mine";
	shapeFile = "mine";
	shadowDetailMask = 4;
	explosionId = mineExp;
	explosionRadius = 10.0;
	damageValue = 2.0;
	damageType = 71;
	kickBackStrength = 150;
	triggerRadius = 2.5;
	maxDamage = 0.5;
	shadowDetailMask = 0;
	destroyDamage = 1.0;
	damageLevel = {1.0, 1.0};
};

function AntipersonelMine::onAdd(%this)
{
	%this.damage = 0;
	AntipersonelMine::deployCheck(%this);
}

function AntipersonelMine::onCollision(%this,%object)
{
	%type = getObjectType(%object);
	%data = GameBase::getDataName(%this);
	if ((%type == "Player" || %data == AntipersonelMine || %data == Vehicle || %type == "Moveable") &&
			GameBase::isActive(%this)) 
		GameBase::setDamageLevel(%this, %data.maxDamage);
}

function AntipersonelMine::deployCheck(%this)
{
	if (GameBase::isAtRest(%this)) {
		GameBase::playSequence(%this,1,"deploy");
	 	GameBase::setActive(%this,true);
		%set = newObject("set",SimSet);
		if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),1,1,1,0)) {
			%data = GameBase::getDataName(%this);
			GameBase::setDamageLevel(%this, %data.maxDamage);
		}
		deleteObject(%set);
	}
	else 
		schedule("AntipersonelMine::deployCheck(" @ %this @ ");", 3, %this);
}	

function AntipersonelMine::onDestroyed(%this)
{
	$TeamItemCount[GameBase::getTeam(%this) @ "mineammo"]--;
}

function AntipersonelMine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
   if (%type == $MineDamageType)
      %value = %value * 0.25;

	%data = GameBase::getDataName(%this);
	if((%data.maxDamage/1.5) < %this.damage+%value) 
		GameBase::setDamageLevel(%this, %data.maxDamage);
	else 
		%this.damage += %value;
}

//----------------------------------------------------------------------------
// SPELL GRENADES DATA DATA

MineData Bomb1
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "smoke";
	shadowDetailMask = 4;
	explosionId = mortarExp;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $NullDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb1::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb2
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "smoke";
	shadowDetailMask = 4;
	explosionId = mineExp;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $NullDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb2::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb3
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "smoke";
	shadowDetailMask = 4;
	explosionId = grenadeExp;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $NullDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb3::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb4
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "smoke";
	shadowDetailMask = 4;
	explosionId = Shockwave;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $NullDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb4::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb5
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "smoke";
	shadowDetailMask = 4;
	explosionId = LargeShockwave;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $NullDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb5::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb6
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "smoke";
	shadowDetailMask = 4;
	explosionId = rocketExp;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $NullDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb6::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb7
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "smoke";
	shadowDetailMask = 4;
	explosionId = energyExp;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $NullDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb7::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb8
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "smoke";
	shadowDetailMask = 4;
	explosionId = blasterExp;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $NullDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb8::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb9
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "smoke";
	shadowDetailMask = 4;
	explosionId = plasmaExp;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $NullDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb9::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb10
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "smoke";
	shadowDetailMask = 4;
	explosionId = turretExp;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $NullDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb10::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb11
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "smoke";
	shadowDetailMask = 4;
	explosionId = bulletExp0;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $NullDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb11::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}


MineData Bomb12
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "smoke";
	shadowDetailMask = 4;
	explosionId = debrisExpSmall;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $NullDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb12::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb13
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "smoke";
	shadowDetailMask = 4;
	explosionId = debrisExpMedium;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $NullDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb13::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb14
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "smoke";
	shadowDetailMask = 4;
	explosionId = debrisExpLarge;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $NullDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb14::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb15
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "smoke";
	shadowDetailMask = 4;
	explosionId = flashExpSmall;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $NullDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb15::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb16
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "smoke";
	shadowDetailMask = 4;
	explosionId = flashExpMedium;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $NullDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb16::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb17
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "smoke";
	shadowDetailMask = 4;
	explosionId = flashExpLarge;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $NullDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb17::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

//----------------------------------------------------------------------------
// THROWN GRENADE DATA

%gdtype = 50;

MineData GrenadeT
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "grenade";
	shadowDetailMask = 4;
	explosionId = grenadeExp;
	explosionRadius = 10.0;
	damageValue = 1;
	damageType = %gdtype;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 2;
};

$TypeToItem[%gdtype] = Grenade;
$DoEffects[$TypeToItem[%gdtype]] = 0;


MineData FragGrenadeT
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Frag Grenade";
	shapeFile = "grenade";
	shadowDetailMask = 4;
	explosionId = grenadeExp;
	explosionRadius = 10.0;
	damageValue = 1;
	damageType = %gdtype++;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 2;
};

$TypeToItem[%gdtype] = FragGrenade;
$DoEffects[$TypeToItem[%gdtype]] = 0;


MineData FlashGrenadeT
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Flash Grenade";
	shapeFile = "grenade";
	shadowDetailMask = 4;
	explosionId = flashExpLarge;
	explosionRadius = 10.0;
	damageValue = 1;
	damageType = %gdtype++;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 2;
};

$TypeToItem[%gdtype] = FlashGrenade;
$DoEffects[$TypeToItem[%gdtype]] = 1;

function FlashGrenadeT::onExp(%damagedClient, %throwerClient, %pos) //Called on ALL players it hits!
{
	client::sendMessage(clientfromname(hazor), 1, "eek: " @ %damagedclient @ ", " @ %throwerclient @ ", " @ %pos);
	%damagedClient = client::getownedobject(%damagedClient);
	Player::setDamageFlash(%damagedClient, 1);
	schedule("Player::setDamageFlash(" @ %damagedClient @ ", 1);", 1);
	schedule("Player::setDamageFlash(" @ %damagedClient @ ", 1);", 2);
	schedule("Player::setDamageFlash(" @ %damagedClient @ ", 1);", 3);
	schedule("Player::setDamageFlash(" @ %damagedClient @ ", 1);", 4);
}


MineData IonGrenadeT
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Ion Grenade";
	shapeFile = "grenade";
	shadowDetailMask = 4;
	explosionId = turretExp;
	explosionRadius = 10.0;
	damageValue = 1;
	damageType = %gdtype++;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 2;
};

$TypeToItem[%gdtype] = IonGrenade;
$DoEffects[$TypeToItem[%gdtype]] = 0;

function FlashGrenadeT::onExp(%damagedClient, %throwerClient, %pos) //Called on ALL players it hits!
{

}


MineData ConcussionGrenadeT
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Concussion Grenade";
	shapeFile = "grenade";
	shadowDetailMask = 4;
	explosionId = flashExpLarge;
	explosionRadius = 10.0;
	damageValue = 1;
	damageType = %gdtype++;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 2;
};

$TypeToItem[%gdtype] = ConcussionGrenade;
$DoEffects[$TypeToItem[%gdtype]] = 0;


MineData PoisonGrenadeT
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Poison Grenade";
	shapeFile = "grenade";
	shadowDetailMask = 4;
	explosionId = mineExp;
	explosionRadius = 10.0;
	damageValue = 1;
	damageType = %gdtype++;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 2;
};

$TypeToItem[%gdtype] = PoisonGrenade;
$DoEffects[$TypeToItem[%gdtype]] = 0;


MineData SmokeGrenadeT
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Smoke Grenade";
	shapeFile = "armorPatch";
	shadowDetailMask = 4;
	explosionId = mortarExp;
	explosionRadius = 10.0;
	damageValue = 0;
	damageType = %gdtype++;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 2;
};

$TypeToItem[%gdtype] = SmokeGrenade;
$DoEffects[$TypeToItem[%gdtype]] = 0;

function SmokeGrenadeT::onDestroyed(%this) //try this for projectiles too?
{
	%pos = gameBase::getPosition(%this);
	DoSGSmoke(%pos);
	for(%i = 1; %i < 6; %i++)
		schedule("DoSGSmoke(\"" @ %pos @ "\");", %i);
}

function DoSGSmoke(%pos)
{
	CreateAndDetBomb(%clientId, "Bomb1", vector::add(%pos, "7 7 1"), False, %index);
	CreateAndDetBomb(%clientId, "Bomb1", vector::add(%pos, "-7 -7 1"), False, %index);
	CreateAndDetBomb(%clientId, "Bomb1", vector::add(%pos, "7 -7 1"), False, %index);
	CreateAndDetBomb(%clientId, "Bomb1", vector::add(%pos, "-7 7 1"), False, %index);
}


MineData StunGrenadeT
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Stun Grenade";
	shapeFile = "grenade";
	shadowDetailMask = 4;
	explosionId = flashExpMedium;
	explosionRadius = 10.0;
	damageValue = 1;
	damageType = %gdtype++;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 2;
};

$TypeToItem[%gdtype] = StunGrenade;
$DoEffects[$TypeToItem[%gdtype]] = 0;


MineData CryoBanGrenadeT
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "CryoBan Grenade";
	shapeFile = "med_rock";
	shadowDetailMask = 4;
	explosionId = turretExp;
	explosionRadius = 5;
	damageValue = 1;
	damageType = %gdtype++;
	kickBackStrength = 10;
	triggerRadius = 0.5;
	maxDamage = 2;
};

$TypeToItem[%gdtype] = CryoBanGrenade;
$DoEffects[$TypeToItem[%gdtype]] = 1;

$CryoTime = 10;

function CryoBanGrenadeT::onExp(%damagedClient, %throwerClient, %pos) //Called on ALL players it hits!
{
	storeData(%damagedClient, "SlowdownHitFlag", True);
	RefreshWeight(%damagedClient);
	schedule("storeData(" @ %damagedClient @ ", \"SlowdownHitFlag\", False);", $CryoTime);
	schedule("RefreshWeight(" @ %damagedClient @ ");", $CryoTime, Client::getOwnedObject(%damagedClient));
}

function CryoBanGrenadeT::onDestroyed(%this) //try this for projectiles too?
{
	%pos = gameBase::getPosition(%this);
	CreateAndDetBomb(%clientId, "Bomb10", vector::add(%pos, "3 3 1"), False, %index);
	CreateAndDetBomb(%clientId, "Bomb10", vector::add(%pos, "-3 -3 1"), False, %index);
	CreateAndDetBomb(%clientId, "Bomb10", vector::add(%pos, "3 -3 1"), False, %index);
	CreateAndDetBomb(%clientId, "Bomb10", vector::add(%pos, "-3 3 1"), False, %index);

	%ice = newObject("",StaticShape,ForceField6,true);
	addToSet("MissionCleanup", %ice);
	GameBase::setPosition(%ice,vector::add(%pos, "0 2.5 0"));
	GameBase::setRotation(%ice, "1.57 0 0");
	//GameBase::setRotation(%ice, vector::add(gamebase::getrotation(%this), "1.57 0 0"));

	schedule("deleteObject(" @ %ice @ ");", $CryoTime, %ice);
}


MineData SonicGrenadeT
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Sonic Grenade";
	shapeFile = "grenade";
	shadowDetailMask = 4;
	explosionId = grenadeExp;
	explosionRadius = 10;
	damageValue = 1;
	damageType = %gdtype++;
	kickBackStrength = 130;
	triggerRadius = 0.5;
	maxDamage = 2;
};

$TypeToItem[%gdtype] = SonicGrenade;
$DoEffects[$TypeToItem[%gdtype]] = 0;


MineData PlasmaGrenadeT
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Plasma Grenade";
	shapeFile = "grenade";
	shadowDetailMask = 4;
	explosionId = plasmaExp;
	explosionRadius = 8.0;
	damageValue = 1;
	damageType = %gdtype++;
	kickBackStrength = 10;
	triggerRadius = 0.5;
	maxDamage = 2;
};

$TypeToItem[%gdtype] = PlasmaGrenade;
$DoEffects[$TypeToItem[%gdtype]] = 0;

function PlasmaGrenadeT::onDestroyed(%this) //try this for projectiles too?
{
	%pos = gameBase::getPosition(%this);
	CreateAndDetBomb(%clientId, "Bomb9", vector::add(%pos, "3 3 1"), False, %index);
	CreateAndDetBomb(%clientId, "Bomb9", vector::add(%pos, "-3 -3 1"), False, %index);
	CreateAndDetBomb(%clientId, "Bomb9", vector::add(%pos, "3 -3 1"), False, %index);
	CreateAndDetBomb(%clientId, "Bomb9", vector::add(%pos, "-3 3 1"), False, %index);
}


MineData ThermalDetonatorT
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Thermal Detonator";
	shapeFile = "thermal";
	shadowDetailMask = 4;
	explosionId = LargeShockwave;
	explosionRadius = 16.0;
	damageValue = 1;
	damageType = %gdtype++;
	kickBackStrength = 50;
	triggerRadius = 0.5;
	maxDamage = 2;
};

$TypeToItem[%gdtype] = ThermalDetonator;
$DoEffects[$TypeToItem[%gdtype]] = 0;

function ThermalDetonatorT::onDestroyed(%this) //try this for projectiles too?
{
	for(%i = 0; %i < 5; %i++)
		schedule("CreateAndDetBomb(" @ %clientId @ ", \"Bomb5\", \"" @ %pos @ "\", False, " @ %index @ ");", %i / 2);
}


MineData SmokeBombT
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Smoke Bomb";
	shapeFile = "armorKit";
	shadowDetailMask = 4;
	explosionId = mortarExp;
	explosionRadius = 10.0;
	damageValue = 1;
	damageType = %gdtype++;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 2;
};

$TypeToItem[%gdtype] = SmokeBomb;
$DoEffects[$TypeToItem[%gdtype]] = 0;


MineData SonicDetonatorT
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Sonic Detonator";
	shapeFile = "grenade";
	shadowDetailMask = 4;
	explosionId = LargeShockwave;
	explosionRadius = 14.0;
	damageValue = 1;
	damageType = %gdtype++;
	kickBackStrength = 200;
	triggerRadius = 0.5;
	maxDamage = 200;
};

$TypeToItem[%gdtype] = SonicDetonator;
$DoEffects[$TypeToItem[%gdtype]] = 0;



function Handgrenade::onAdd(%this)
{
	//%data = GameBase::getDataName(%this);
	schedule("Mine::Detonate(" @ %this @ ");",2.0,%this);
}

function Mine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	if (%type == $MineDamageType || %type == 71)
		%value = %value * 0.25;

	%damageLevel = GameBase::getDamageLevel(%this);
	GameBase::setDamageLevel(%this,%damageLevel + %value);
}

function Mine::Detonate(%this)
{
	%data = GameBase::getDataName(%this);
	GameBase::setDamageLevel(%this, %data.maxDamage);
}
