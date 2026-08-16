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
	damageType = $SpellDamageType;
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

MineData Bomb1
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "bullet";
	shadowDetailMask = 4;
	explosionId = mortarExp;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $SpellDamageType;
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
	damageType = $SpellDamageType;
	kickBackStrength = 10.00;
	triggerRadius = 2.5;
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
	shapeFile = "bullet";
	shadowDetailMask = 4;
	explosionId = fire2Exp;
	explosionRadius = 15.0;
	damageValue = 1.25;
	damageType = $SpellDamageType;
	kickBackStrength = 10.0;
	triggerRadius = 15.0;
	maxDamage = 1.25;
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
	shapeFile = "bullet";
	shadowDetailMask = 4;
	explosionId = Shockwave;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $SpellDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb4::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb24
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "discb";
	shadowDetailMask = 4;
	explosionId = Fusionex;
	explosionRadius = 2.0;
	damageValue = 10.0;
	damageType = $SpellDamageType;
	kickBackStrength = 40.5;
	triggerRadius = 0.5;
	maxDamage = 10.0;
};
function Bomb24::onAdd(%this)
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
	shapeFile = "discb";
	shadowDetailMask = 4;
	explosionId = LargeShockwave;
	explosionRadius = 10.0;
	damageValue = 10.0;
	damageType = $SpellDamageType;
	kickBackStrength = 0;
	triggerRadius = 10.5;
	maxDamage = 10.0;
};
function Bomb5::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

//Chee
MineData Bomb40
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "discb";
	shadowDetailMask = 4;
	explosionId = EnergyExp;
	explosionRadius = 0.0;
	damageValue = 0.10;
	damageType = $SpellDamageType;
	kickBackStrength = 0;
	triggerRadius = 10.5;
	maxDamage = 10.0;
};
function Bomb40::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb41
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "discb";
	shadowDetailMask = 4;
	explosionId = debrisExplarge;
	explosionRadius = 10.0;
	damageValue = 0.60;
	damageType = $SpellDamageType;
	kickBackStrength = 0;
	triggerRadius = 10.5;
	maxDamage = 0.10;
};
function Bomb41::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb44
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "discb";
	shadowDetailMask = 4;
	explosionId = freezeExp;
	explosionRadius = 10.0;
	damageValue = 0.60;
	damageType = $SpellDamageType;
	kickBackStrength = 0;
	triggerRadius = 10.5;
	maxDamage = 0.10;
};
function Bomb44::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb42
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "discb";
	shadowDetailMask = 4;
	explosionId = freezeExp;
	explosionRadius = 10.0;
	damageValue = 0.20;
	damageType = $SpellDamageType;
	kickBackStrength = 0;
	triggerRadius = 10.5;
	maxDamage = 0.10;
};
function Bomb42::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb43
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "discb";
	shadowDetailMask = 4;
	explosionId = freezeExp;
	explosionRadius = 10.0;
	damageValue = 0.10;
	damageType = $SpellDamageType;
	kickBackStrength = 0;
	triggerRadius = 10.5;
	maxDamage = 0.10;
};
function Bomb43::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}
MineData Bomb107
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "bullet";
	shadowDetailMask = 4;
	explosionId = LargeShockwave;
	explosionRadius = 10.0;
	damageValue = 1.5;
	damageType = $SpellDamageType;
	kickBackStrength = 100.0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb107::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}
MineData Bomb108
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "bullet";
	shadowDetailMask = 4;
	explosionId = ustarExp;
	explosionRadius = 10.0;
	damageValue = 2.5;
	damageType = $SpellDamageType;
	kickBackStrength = 100.0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb108::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}
MineData Bomb200
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "bullet";
	shadowDetailMask = 4;
	explosionId = ustarExp;
	explosionRadius = 10.0;
	damageValue = 5.0;
	damageType = $SpellDamageType;
	kickBackStrength = 700.0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb200::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData bomb201
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "bullet";
	shadowDetailMask = 4;
	explosionId = iceExp;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $SpellDamageType;
	kickBackStrength = 300.0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb201::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb202
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "bullet";
	shadowDetailMask = 4;
	explosionId = rocketExp;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $SpellDamageType;
	kickBackStrength = 300.0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb202::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb300
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "blueball";
	shadowDetailMask = 4;
	explosionId = energyExp;
	explosionRadius = 10.0;
	damageValue = 0.70;
	damageType = $SpellDamageType;
	kickBackStrength = 300.0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb300::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb301
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "blueball";
	shadowDetailMask = 4;
	explosionId = rocketExp;
	explosionRadius = 10.0;
	damageValue = 0.80;
	damageType = $SpellDamageType;
	kickBackStrength = 300.0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb301::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb302
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "zap";
	shadowDetailMask = 4;
	explosionId = energyExp;
	explosionRadius = 10.0;
	damageValue = 0.80;
	damageType = $SpellDamageType;
	kickBackStrength = 0.0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb302::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb303
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "zap";
	shadowDetailMask = 4;
	explosionId = turretExp;
	explosionRadius = 10.0;
	damageValue = 1.50;
	damageType = $SpellDamageType;
	kickBackStrength = 0.0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb303::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb304
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "zap";
	shadowDetailMask = 4;
	explosionId = LargeShockwave;
	explosionRadius = 10.0;
	damageValue = 2.0;
	damageType = $SpellDamageType;
	kickBackStrength = 300.0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb304::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb305
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "discb";
	shadowDetailMask = 4;
	explosionId = energyExp;
	explosionRadius = 10.0;
	damageValue = 0.0;
	damageType = $SpellDamageType;
	kickBackStrength = 300.0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb305::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb306
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "mflame";
	shadowDetailMask = 4;
	explosionId = rocketExp;
	explosionRadius = 10.0;
	damageValue = 0.10;
	damageType = $SpellDamageType;
	kickBackStrength = 300.0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb306::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

MineData Bomb307
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "zap";
	shadowDetailMask = 4;
	explosionId = electricalExp;
	explosionRadius = 10.0;
	damageValue = 0.20;
	damageType = $SpellDamageType;
	kickBackStrength = 300.0;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};
function Bomb307::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}
MineData Bomb950
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
  damageValue = 1.5;
  damageType = $SpellDamageType;
  kickBackStrength = 100;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

function bomb950::onAdd(%this)
{
	schedule("shower(" @ %this @ " , 5);",0.5,%this);
	schedule("Mine::Detonate(" @ %this @ ");",1.5,%this);
}

function shower(%this, %count)
{
	if(%count && %this)
	{
		%obj = newObject("","Mine","bomb951");
 		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj,%this,5,false);

		%obj = newObject("","Mine","bomb951");
	 	addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj,%this,-5,false);
		//%count -= 1;
		//schedule("DeployDisc(" @ %this @ " , " @ %count @ ");",0.5,%this);
	}
}

MineData bomb951
{
   	mass = 5.0;
   	drag = 1.0;
   	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "disc";
	shapeFile = "smoke";
	shadowDetailMask = 4;
	explosionId = DebrisExpMedium;
	explosionRadius = 10.0;
	damageValue = 0.50;
	damageType = $SpellDamageType;
	kickBackStrength = 50;
	triggerRadius = 0.5;
	maxDamage = 1.5;
};

function bomb951::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");",0.9,%this);
}

MineData bomb611
{
   	mass = 5.0;
   	drag = 1.0;
   	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "disc";
	shapeFile = "skel";
	shadowDetailMask = 4;
	explosionId = LargeShockwave;
	explosionRadius = 20.0;
	damageValue = 0.50;
	damageType = $SpellDamageType;
	kickBackStrength = 50;
	triggerRadius = 0.5;
	maxDamage = 1.5;
};

function bomb611::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");",0.9,%this);
}

MineData bomb612
{
   	mass = 5.0;
   	drag = 1.0;
   	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "disc";
	shapeFile = "skel";
	shadowDetailMask = 4;
	explosionId = MortarExp;
	explosionRadius = 20.0;
	damageValue = 0.20;
	damageType = $SpellDamageType;
	kickBackStrength = 50;
	triggerRadius = 0.5;
	maxDamage = 1.5;
};

function bomb612::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");",0.9,%this);
}
MineData bomb88888
{
   	mass = 5.0;
   	drag = 1.0;
   	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "sappers";
	shapeFile = "goblin";
	shadowDetailMask = 4;
	explosionId = DirtyExp;
	explosionRadius = 5.0;
	damageValue = 5.0;
	damageType = $SpellDamageType;
	kickBackStrength = 50;
	triggerRadius = 0.5;
	maxDamage = 1.5;
};

function bomb88888::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");",0.9,%this);
}
//Chee
MineData Handgrenade
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
	damageValue = 0.5;
	damageType = $SpellDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 2;
};

function Handgrenade::onAdd(%this)
{
	%data = GameBase::getDataName(%this);
	schedule("Mine::Detonate(" @ %this @ ");",2.0,%this);
}
MineData Bomb3000
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "discb";
	shadowDetailMask = 4;
	explosionId = spellExp1;
	explosionRadius = 0.0;
	damageValue = 0.0;
	damageType = $SpellDamageType;
	kickBackStrength = 0;
	triggerRadius = 10.5;
	maxDamage = 0.10;
};
function Bomb3000::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}
MineData Bomb3002
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "discb";
	shadowDetailMask = 4;
	explosionId = spellExp2;
	explosionRadius = 0.0;
	damageValue = 0.0;
	damageType = $SpellDamageType;
	kickBackStrength = 0;
	triggerRadius = 10.5;
	maxDamage = 0.10;
};
function Bomb3002::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}
MineData Bomb3003
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "discb";
	shadowDetailMask = 4;
	explosionId = spellExp3;
	explosionRadius = 0.0;
	damageValue = 0.0;
	damageType = $SpellDamageType;
	kickBackStrength = 0;
	triggerRadius = 10.5;
	maxDamage = 0.10;
};
function Bomb3003::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}
MineData Bomb6661
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 1.0;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "invisable";
	shadowDetailMask = 4;
	explosionId = windExp;
	explosionRadius = 0.0;
	damageValue = 1.0;
	damageType = $SpellDamageType;
	kickBackStrength = 0;
	triggerRadius = 10.5;
	maxDamage = 0.10;
};
function Bomb6661::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}
MineData Bomb6662
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 1.0;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "invisable";
	shadowDetailMask = 4;
	explosionId = windExp;
	explosionRadius = 0.0;
	damageValue = 2.0;
	damageType = $SpellDamageType;
	kickBackStrength = 0;
	triggerRadius = 10.5;
	maxDamage = 0.10;
};
function Bomb6662::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}
MineData Bomb6663
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 1.0;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "invisable";
	shadowDetailMask = 4;
	explosionId = windExp;
	explosionRadius = 0.0;
	damageValue = 4.0;
	damageType = $SpellDamageType;
	kickBackStrength = 0;
	triggerRadius = 10.5;
	maxDamage = 0.10;
};
function Bomb6663::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}
MineData Bomb6664
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 1.0;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "invisable";
	shadowDetailMask = 4;
	explosionId = windExp;
	explosionRadius = 0.0;
	damageValue = 8.0;
	damageType = $SpellDamageType;
	kickBackStrength = 0;
	triggerRadius = 10.5;
	maxDamage = 0.10;
};
function Bomb6664::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}
MineData Bomb444
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 1.0;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "boltbolt1";
	shadowDetailMask = 4;
	explosionId = electricalExp;
	explosionRadius = 0.0;
	damageValue = 1.000;
	damageType = $SpellDamageType;
	kickBackStrength = 0;
	triggerRadius = 10.5;
	maxDamage = 0.10;
};
function Bomb444::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");", 0.2, %this);
}

function Mine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
   if (%type == $MineDamageType)
      %value = %value * 0.25;

	%damageLevel = GameBase::getDamageLevel(%this);
	GameBase::setDamageLevel(%this,%damageLevel + %value);
}

function Mine::Detonate(%this)
{
	%data = GameBase::getDataName(%this);
	GameBase::setDamageLevel(%this, %data.maxDamage);
}



