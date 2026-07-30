$NMABD = 10;


ExplosionData NAfterExp
{
	shapeName = "plasmaex.dts";
	faceCamera = true;
	randomSpin = true;
	hasLight   = true;
	lightRange = 3.0;
	timeZero = 0.450;
	timeOne  = 0.750;
	colors[0]  = { 0.25, 0.25, 1.0 };
	colors[1]  = { 0.25, 0.25, 1.0 };
	colors[2]  = { 1.0,  1.0,  1.0 };
	radFactors = { 1.0, 1.0, 1.0 };
};

ExplosionData LargeShockwave2
{
   shapeName = "shockwave_large.dts";
   soundId   = shockExplosion;

   faceCamera = true;
   randomSpin = false;
   hasLight   = true;
   lightRange = 10.0;

   timeZero = 0.100;
   timeOne  = 0.300;

   colors[0]  = { 1.0, 1.0, 1.0 };
   colors[1]  = { 1.0, 1.0, 1.0 };
   colors[2]  = { 0.0, 0.0, 0.0 };
   radFactors = { 1.0, 0.5, 0.0 };
};

ExplosionData LargeShockwave3
{
   shapeName = "shockwave_large.dts";
   soundId   = shockExplosion;

   faceCamera = false;
   randomSpin = true;
   hasLight   = true;
   lightRange = 10.0;

   timeZero = 0;
   timeOne  = 500;

   colors[0]  = { 1.0, 1.0, 1.0 };
   colors[1]  = { 1.0, 1.0, 1.0 };
   colors[2]  = { 0.0, 0.0, 0.0 };
   radFactors = { 1.5, 1.5, 1.5 };
};

function NuclearExplosion2(%this,%pos)
{	
	echo("Nuclear: "@%this@", at "@%pos);
	%obj = newObject("","Mine","NRad");
	addToSet("MissionCleanup", %obj);//-Core
	GameBase::setPosition(%obj,%pos);

	%obj = newObject("","Mine","NRing12");
	addToSet("MissionCleanup", %obj);//-Horiz Ring
	GameBase::setPosition(%obj,%pos);

	%obj = newObject("","Mine","NRing22");
	addToSet("MissionCleanup", %obj);//-Vert Ring
	GameBase::setPosition(%obj,%pos);

	%obj = newObject("","Mine","NRing32");
	addToSet("MissionCleanup", %obj);//-Final Ring
	GameBase::setPosition(%obj,%pos);

	for(%i=0; %i < 6.28; %i += 1.256) {
		%NewPos1 = Vector::add(%pos, Vector::getFromRot(%i@" 0 0",$NMABD, 0.0));  // X = angle from center, Y = Horiz dist from center, Z = Vert dist from center
		%NewPos2 = Vector::add(%pos, Vector::getFromRot("0 "@%i@" 0",0.0,$NMABD)); // X = angle from center, Y = Horiz dist from center, Z = Vert dist from center
		%NewPos3 = Vector::add(%pos, Vector::getFromRot("0 0 "@%i,0.0,$NMABD));   // X = angle from center, Y = Horiz dist from center, Z = Vert dist from center
		%obj1 = newObject("","Mine","NAfter");
		%obj2 = newObject("","Mine","NAfter");
		%obj3 = newObject("","Mine","NAfter");
		addToSet("MissionCleanup",%obj1);
		addToSet("MissionCleanup",%obj2);
		addToSet("MissionCleanup",%obj3);
		GameBase::setPosition(%obj1,%NewPos1);
		GameBase::setPosition(%obj2,%NewPos2);
		GameBase::setPosition(%obj3,%NewPos3);
	}
}

//==================================================================================================== Nuclear Explosion

ItemData NukeMine
{				
	description = "";
	className = "Mine";
	showInventory = false;
};



MineData GRad
{
	className = "Mine";
        shapeFile = "force";
        shadowDetailMask = 4;
        explosionId = plasmaExp;
        explosionRadius = 20.0;
        damageValue = 0;
	damageType = $PlasmaDamageType;
        kickBackStrength = 0;
	triggerRadius = 0.01;
	maxDamage = 0.5;
	shadowDetailMask = 0;
	destroyDamage = 1.0;
	damageLevel = {1.0, 1.0};
};

MineData GRing1
{
	className = "Mine";
        shapeFile = "force";
        shadowDetailMask = 4;
        explosionId = LargeShockwave;
        explosionRadius = 15.0;
        damageValue = 0;
	damageType = $PlasmaDamageType;
        kickBackStrength = 0;
	triggerRadius = 0.01;
	maxDamage = 0.5;
	shadowDetailMask = 0;
	destroyDamage = 2.0;
	damageLevel = {1.0, 1.0};
};

MineData GRing2
{
	className = "Mine";
        shapeFile = "force";
        shadowDetailMask = 4;
        explosionId = LargeShockwave2;
        explosionRadius = 15.0;
        damageValue = 0;
	damageType = $PlasmaDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.01;
	maxDamage = 0.5;
	shadowDetailMask = 0;
	destroyDamage = 2.0;
	damageLevel = {1.0, 1.0};
};

MineData GRing3
{
	className = "Mine";
        shapeFile = "force";
        shadowDetailMask = 4;
        explosionId = LargeShockwave3;
        explosionRadius = 15.0;
        damageValue = 0;
	damageType = $PlasmaDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.01;
	maxDamage = 0.5;
	shadowDetailMask = 0;
	destroyDamage = 1.0;
	damageLevel = {1.0, 1.0};
};

MineData NRad
{
	className = "Mine";
        shapeFile = "force";
        shadowDetailMask = 4;
        explosionId = plasmaExp;
        explosionRadius = 25;
        damageValue = 1.0;
	damageType = $PlasmaDamageType;
        kickBackStrength = 100;
	triggerRadius = 0.01;
	maxDamage = 0.5;
	shadowDetailMask = 0;
	destroyDamage = 1.0;
	damageLevel = {1.0, 1.0};
};

MineData NRing12
{
	className = "Mine";
        shapeFile = "force";
        shadowDetailMask = 4;
        explosionId = LargeShockwave;
        explosionRadius = 50.0;
        damageValue = 1.0;
	damageType = $PlasmaDamageType;
        kickBackStrength = 450;
	triggerRadius = 0.01;
	maxDamage = 0.5;
	shadowDetailMask = 0;
	destroyDamage = 2.0;
	damageLevel = {1.0, 1.0};
};

MineData NRing22
{
	className = "Mine";
        shapeFile = "force";
        shadowDetailMask = 4;
        explosionId = LargeShockwave2;
        explosionRadius = 15.0;
        damageValue = 1.0;
	damageType = $PlasmaDamageType;
	kickBackStrength = 250;
	triggerRadius = 0.01;
	maxDamage = 0.5;
	shadowDetailMask = 0;
	destroyDamage = 2.0;
	damageLevel = {1.0, 1.0};
};

MineData NRing32
{
	className = "Mine";
        shapeFile = "force";
        shadowDetailMask = 4;
        explosionId = LargeShockwave3;
        explosionRadius = 15.0;
        damageValue = 0.65;
	damageType = $PlasmaDamageType;
	kickBackStrength = 150;
	triggerRadius = 0.01;
	maxDamage = 0.5;
	shadowDetailMask = 0;
	destroyDamage = 1.0;
	damageLevel = {1.0, 1.0};
};

MineData NAfter
{
	className = "Mine";
        shapeFile = "force";
        shadowDetailMask = 4;
        explosionId = NAfterExp;
        explosionRadius = 5.0;
        damageValue = 0.65;
	damageType = $PlasmaDamageType;
	kickBackStrength = 25;
	triggerRadius = 0.01;
	maxDamage = 0.5;
	shadowDetailMask = 0;
	destroyDamage = 1.0;
	damageLevel = {1.0, 1.0};
};

function NRad::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");",0.02,%this);
}
function NRing12::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");",0.02,%this);
}
function NRing22::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");",0.02,%this);
}
function NRing32::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");",0.02,%this);
}
function NAfter::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");",0.02,%this);
}