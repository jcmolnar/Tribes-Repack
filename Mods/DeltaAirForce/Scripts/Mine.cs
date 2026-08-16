//----------------------------------------------------------------------------
// MINE DYNAMIC DATA

MineData AntipersonelMine
{
	className = "Mine";
   description = "M14 APMine";
   shapeFile = "mine";
   shadowDetailMask = 4;
   explosionId = NewGrenExp;
	explosionRadius = 5.0;
	damageValue = 1.0;
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 2.5;
	maxDamage = 0.1;
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
	%objdata = GameBase::getDataName(%object);
	//if ((%type == "Player" || %data == AntipersonelMine || %data == AntipersonelMineGator || %data == AntiarmorMine || %data == AntipersonelMineBouncing || %data == Vehicle || %type == "Moveable") && GameBase::isActive(%this)) {
	if ((%type == "Player" || %objdata == AntipersonelMine || %objdata == Vehicle || %type == "Moveable") && GameBase::isActive(%this)) {
		// DELTA FORCE 
		//if(Gamebase::getTeam(%this) == Gamebase::getTeam(%object)) return;
		// END DELTA FORCE
		GameBase::setDamageLevel(%this, %data.maxDamage + 1);
	}
}

function AntipersonelMine::deployCheck(%this)
{
	if (GameBase::isAtRest(%this)) {
		GameBase::playSequence(%this,1,"deploy");
	 	GameBase::setActive(%this,true);
		%set = newObject("set",SimSet);
		if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),2.5,2.5,2.5,0)) {
			%data = GameBase::getDataName(%this);
			GameBase::setDamageLevel(%this, %data.maxDamage + 1);
		} else if(GameBase::getLOSInfo(%this, 1, "-1.571 0 0"))
		{
			if(getObjectType($los::object) == "SimTerrain")
				GameBase::startFadeOut(%this); // Invisible mines!
		}
		deleteObject(%set);
	}
	else 
		schedule("AntipersonelMine::deployCheck(" @ %this @ ");", 3, %this);
}	

function AntipersonelMine::onDestroyed(%this)
{
	$TeamItemCount[GameBase::getTeam(%this) @ "APMinePack"]--;
}

function AntipersonelMine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
   if (%type == $MineDamageType)
      %value = %value * 0.25;

	%data = GameBase::getDataName(%this);
	if((%data.maxDamage/1.5) < %this.damage+%value) 
		GameBase::setDamageLevel(%this, %data.maxDamage + 1);
	else 
		%this.damage += %value;
}

//----------------------------------------------------------------------------
	 
MineData AntipersonelMineBouncing
{
	className = "Mine";
   description = "M16 APMine";
   shapeFile = "sensor_small";
   shadowDetailMask = 4;
   explosionId = mineExpTwo;
	explosionRadius = 5.0;
	damageValue = 0.001;
	damageType = $MineDamageType;
	kickBackStrength = 0;
	triggerRadius = 3.0;
	maxDamage = 0.1;
	shadowDetailMask = 0;
	destroyDamage = 1.0;
	damageLevel = {1.0, 1.0};
};

function AntipersonelMineBouncing::onAdd(%this)
{
	%this.damage = 0;
	AntipersonelMineBouncing::deployCheck(%this);
	
}

function AntipersonelMineBouncing::onCollision(%this,%object)
{
	%type = getObjectType(%object);
	%data = GameBase::getDataName(%this);
	%objdata = GameBase::getDataName(%object);
	if ((%type == "Player" || %objdata == AntipersonelMineBouncing || %objdata == Vehicle || %objtype == "Moveable") && GameBase::isActive(%this)) {
		// DELTA FORCE 
		//if(Gamebase::getTeam(%this) == Gamebase::getTeam(%object)) return;
		// END DELTA FORCE
		GameBase::setDamageLevel(%this, %data.maxDamage + 1);
	}
}

function AntipersonelMineBouncing::deployCheck(%this)
{
	if (GameBase::isAtRest(%this)) {
		GameBase::playSequence(%this,1,"deploy");
	 	GameBase::setActive(%this,true);
		$APBMinePos[%this] = gamebase::getposition(%this); 
		%set = newObject("set",SimSet);
		if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),3.0,3.0,3.0,0)) {
			%data = GameBase::getDataName(%this);
			GameBase::setDamageLevel(%this, %data.maxDamage + 1);
		} else if(GameBase::getLOSInfo(%this, 1, "-1.571 0 0"))
		{
			if(getObjectType($los::object) == "SimTerrain")
				GameBase::startFadeOut(%this); // Invisible mines!
		}
		deleteObject(%set);
	}
	else 
		schedule("AntipersonelMineBouncing::deployCheck(" @ %this @ ");", 3, %this);
}	

function AntipersonelMineBouncing::onDestroyed(%this)
{
	$TeamItemCount[GameBase::getTeam(%this) @ "BouncingMinePack"]--;
	
	//Projectile::spawnProjectile(BouncingMineCharge, "0 0 0 0 0 0 0 0 0 " @ vector::add($APBMinePos[%this], "0 0 0.1"), %this.thrower, "0 0 50");
	Projectile::spawnProjectile(BouncingMineCharge, "0 0 0 0 0 0 0 0 0 " @ vector::add(gamebase::getposition(%this), "0 0 0.1"), 2048, "0 0 50");
}

function AntipersonelMineBouncing::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
   if (%type == $MineDamageType)
      %value = %value * 0.25;

	%data = GameBase::getDataName(%this);
	if((%data.maxDamage/1.5) < %this.damage+%value) 
		GameBase::setDamageLevel(%this, %data.maxDamage + 1);
	else 
		%this.damage += %value;
}

//----------------------------------------------------------------------------

MineData AntiarmorMine
{
	className = "Mine";
   description = "M15 AAMine";
   shapeFile = "discammo";
   shadowDetailMask = 4;
   explosionId = mineExp;
	explosionRadius = 12.0;
	damageValue = 4.0;
	damageType = $MineDamageType;
	kickBackStrength = 200;
	triggerRadius = 5.0;
	maxDamage = 0.5;
	shadowDetailMask = 0;
	destroyDamage = 1.0;
	damageLevel = {1.0, 1.0};
};

function AntiarmorMine::onAdd(%this)
{
	%this.damage = 0;
	AntiarmorMine::deployCheck(%this);
}

function AntiarmorMine::onCollision(%this,%object)
{
	%type = getObjectType(%object);
	%data = GameBase::getDataName(%this);
	%objdata = GameBase::getDataName(%object);
	if ((%objdata == AntiarmorMine || %objdata == Vehicle || %type == "Moveable") && GameBase::isActive(%this)) {
		// DELTA FORCE 
		//if(Gamebase::getTeam(%this) == Gamebase::getTeam(%object)) return;
		// END DELTA FORCE
		GameBase::setDamageLevel(%this, %data.maxDamage + 1);
	}
}

function AntiarmorMine::deployCheck(%this)
{
	if (GameBase::isAtRest(%this)) {
		GameBase::playSequence(%this,1,"deploy");
	 	GameBase::setActive(%this,true);
		%set = newObject("set",SimSet);
		if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),5,5,5,0)) {
			%data = GameBase::getDataName(%this);
			GameBase::setDamageLevel(%this, %data.maxDamage + 1);
		} else if(GameBase::getLOSInfo(%this, 1, "-1.571 0 0"))
		{
			if(getObjectType($los::object) == "SimTerrain")
				GameBase::startFadeOut(%this); // Invisible mines!
		}
		deleteObject(%set);
	}
	else 
		schedule("AntiarmorMine::deployCheck(" @ %this @ ");", 3, %this);
}	

function AntiarmorMine::onDestroyed(%this)
{
	$TeamItemCount[GameBase::getTeam(%this) @ "AAMinePack"]--;
	
}

function AntiarmorMine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
   if (%type == $MineDamageType)
      %value = %value * 0.25;

	%data = GameBase::getDataName(%this);
	if((%data.maxDamage/1.5) < %this.damage+%value) 
		GameBase::setDamageLevel(%this, %data.maxDamage + 1);
	else 
		%this.damage += %value;
}

//----------------------------------------------------------------------------

MineData AntipersonelMineGator
{
	className = "Mine";
   description = "BLU-92/B APMine";
   shapeFile = "force";
   shadowDetailMask = 4;
   explosionId = mineExpTwo;
	explosionRadius = 3.0;
	damageValue = 0.5;
	damageType = $ShrapnelDamageType;
	kickBackStrength = 30;
	triggerRadius = 2.0;
	maxDamage = 0.1;
	shadowDetailMask = 0;
	destroyDamage = 1.0;
	damageLevel = {1.0, 1.0};
};

function AntipersonelMineGator::onAdd(%this)
{
	%this.damage = 0;
	AntipersonelMineGator::deployCheck(%this);
}

function AntipersonelMineGator::onCollision(%this,%object)
{
	%type = getObjectType(%object);
	%data = GameBase::getDataName(%this);
	%objdata = GameBase::getDataName(%object);
	if ((%type == "Player" || %objdata == AntipersonelMineGator || %objdata == Vehicle || %type == "Moveable") && GameBase::isActive(%this)) {
		// DELTA FORCE 
		//if(Gamebase::getTeam(%this) == Gamebase::getTeam(%object)) return;
		// END DELTA FORCE
		gamebase::setposition(%this, vector::add(gamebase::Getposition(%this), "0 0 0.1"));
		GameBase::setDamageLevel(%this, %data.maxDamage + 1);
	}
}

function AntipersonelMineGator::deployCheck(%this)
{
	if (GameBase::isAtRest(%this)) {
		GameBase::playSequence(%this,1,"deploy");
	 	GameBase::setActive(%this,true);
		%set = newObject("set",SimSet);
		if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),0.5,0.5,0.5,0)) {
			%data = GameBase::getDataName(%this);
			gamebase::setposition(%this, vector::add(gamebase::Getposition(%this), "0 0 0.1"));
			GameBase::setDamageLevel(%this, %data.maxDamage + 1);

		}// else if(GameBase::getLOSInfo(%this, 1, "-1.571 0 0"))
		//{

		//	if(getObjectType($los::object) == "SimTerrain")
		//		GameBase::startFadeOut(%this); // Invisible mines!
		//}
		deleteObject(%set);
		//gamebase::setposition(%this, vector::add(gamebase::Getposition(%this), "0 0 -0.15"));
	}
	else 
		schedule("AntipersonelMineGator::deployCheck(" @ %this @ ");", 3, %this);
}	

function AntipersonelMineGator::onDestroyed(%this)
{
	gamebase::setposition(%this, vector::add(gamebase::Getposition(%this), "0 0 0.1"));
}

function AntipersonelMineGator::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
   if (%type == $MineDamageType)
      %value = %value * 0.25;

	%data = GameBase::getDataName(%this);
	if((%data.maxDamage/1.5) < %this.damage+%value){
		gamebase::setposition(%this, vector::add(gamebase::Getposition(%this), "0 0 0.1"));
		GameBase::setDamageLevel(%this, %data.maxDamage + 1);
	} else 
		%this.damage += %value;
}

//----------------------------------------------------------------------------



MineData Smokegrenade
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
   explosionId = smokeCloudExp;
	explosionRadius = 10.0;
	damageValue = 0.0;
	damageType = $SmokeDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 2;
};

function Smokegrenade::onAdd(%this)
{
	schedule("smokeDetonate("@%this@");", 1, %this);
}

function smokeDetonate(%this)
{
	if($SmokeTime[%this] > 0)
	{
		%gren = newObject("", "Mine", "Smokegrenade");
		$GrenOwner[%gren] = $GrenOwner[%this];
		$SmokeTime[%gren] = $SmokeTime[%this] - 1;
		GameBase::setPosition(%gren, GameBase::getPosition(%this));
		GameBase::setRotation(%gren, GameBase::getRotation(%this));
	}
	GameBase::setDamageLevel(%this, 2);

	// Blind anyone actually inside the smoke cloud...
	//%set = newObject("set",SimSet);
	//%num = containerBoxFillSet(%set,$SimPlayerObjectType,GameBase::getPosition(%this),20,20,20,0);
	//if(%num > 0)
	//{
	//	for(%i = 0; %i < %num; %i++)
	//	{
	//		%player = Group::getObject(%set, %i);
	//		Player::setDamageFlash(%player, 5.0);
	//		schedule("Player::setDamageFlash("@%player@",0.0);", 2.0, %player);
	//	}
	//}
	//deleteObject(%set);
}

BulletData FragGrenadeShrapnelBullet
{
	bulletShapeName = "bullet.dts";
	explosionTag = BulletExp0;
	expRandCycle = 3;
	mass = 0.05;
	bulletHoleIndex = 0;

	damageClass = 0;       // 0 impact, 1, radius
	damageValue = 0.2;
	damageType = $BulletDamageType;

	aimDeflection = 1.0;
	muzzleVelocity = 1000.0;
	totalTime = 1.5;
	inheritedVelocityScale = 0.25;
	isVisible = true;

	tracerPercentage = 1.0;
	tracerLength = 30;
};

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
   explosionId = NewGrenExp;
	explosionRadius = 17.0;
	damageValue = 0.7;
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 2;
};

function grenadeFindOwner(%this)
{
	return $Thrower[%this];
}

function checkSmoke(%this)
{
	%owner = grenadeFindOwner(%this);
	if($STHROW)
		echo(%owner);
	$GrenOwner[%this] = %owner;
	if($GrenMode[%owner] == 1) 
	{ // Smoke
		$SmokeGren[%this] = true;
		$SmokeTime[%this] = $Server::SmokePuffs; // # of puffs of smoke
	} else $SmokeGren[%this] = false;
}

function Handgrenade::onAdd(%this)
{
	%data = GameBase::getDataName(%this);
	
	// DELTA FORCE
	schedule("checkSmoke("@%this@");", 0.05, %this);
	//checkSmoke(%this);
	// END DELTA FORCE
	
	schedule("Mine::Detonate(" @ %this @ ");",2.0,%this);
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
	if($GrenOwner[%this] == "-1" || !$GrenOwner[%this])
		$GrenOwner[%this] = $Thrower[%this];
	if(!$SmokeGren[%this] && %data != "Handgrenade") // Not a smoke grenade, and not a Hand grenade
	{
		GameBase::setDamageLevel(%this, %data.maxDamage);
	} else if($SmokeGren[%this])
	{
		%gren = newObject("", "Mine", "Smokegrenade");
		$GrenOwner[%gren] = $GrenOwner[%this];
		$SmokeTime[%gren] = $SmokeTime[%this];
		GameBase::setPosition(%gren, GameBase::getPosition(%this));
		GameBase::setRotation(%gren, GameBase::getRotation(%this));
		Smokegrenade::onAdd(%gren);
		deleteObject(%this);
	} else {
		GameBase::SetDamageLevel(%this, %data.maxDamage);
		%pos = GameBase::getPosition(%this);
		if($FragProjectile) {
			%rot = GameBase::getRotation(%this);
			%vec = Vector::getfromrot(%rot);
			%trans = %rot@" "@%vec@" "@%rot@" "@%pos;
			%vel = Item::getVelocity(%this);
			%proj0 = Projectile::spawnProjectile("FragGrenadeShrapnelBullet",%trans,$GrenOwner[%this],%vel);
			%proj1 = Projectile::spawnProjectile("FragGrenadeShrapnelBullet",%trans,$GrenOwner[%this],%vel);
			%proj2 = Projectile::spawnProjectile("FragGrenadeShrapnelBullet",%trans,$GrenOwner[%this],%vel);
			%proj3 = Projectile::spawnProjectile("FragGrenadeShrapnelBullet",%trans,$GrenOwner[%this],%vel);
			%proj4 = Projectile::spawnProjectile("FragGrenadeShrapnelBullet",%trans,$GrenOwner[%this],%vel);
			%proj5 = Projectile::spawnProjectile("FragGrenadeShrapnelBullet",%trans,$GrenOwner[%this],%vel);
			%proj6 = Projectile::spawnProjectile("FragGrenadeShrapnelBullet",%trans,$GrenOwner[%this],%vel);
			%proj7 = Projectile::spawnProjectile("FragGrenadeShrapnelBullet",%trans,$GrenOwner[%this],%vel);
			%proj8 = Projectile::spawnProjectile("FragGrenadeShrapnelBullet",%trans,$GrenOwner[%this],%vel);
			%proj9 = Projectile::spawnProjectile("FragGrenadeShrapnelBullet",%trans,$GrenOwner[%this],%vel);
		}
		GameBase::applyRadiusDamage(17,%pos, 7, 1.4, 30, $GrenOwner[%this]);
	}
}


//==================================FLARE

MineData Flare
{
	className = "Mine";
   description = "Flare";
   shapeFile = "shotgunbolt";
   shadowDetailMask = 4;
   explosionId = blasterexp;
	explosionRadius = 1.0;
	damageValue = 0.1;
	damageType = $PlasmaDamageType;
	kickBackStrength = 1;
	triggerRadius = 5.0;
	maxDamage = 0.01;
	shadowDetailMask = 0;
	destroyDamage = 0.1;
	damageLevel = {1.0, 1.0};
};

function Flare::onAdd(%this)
{
	%this.damage = 0;

	schedule("Flare::light(" @ %this @ ");", 0.5, %this);
	schedule("Flare::Timer(" @ %this @ ");", 20, %this);
}

function Flare::Timer(%this)
{
	GameBase::setDamageLevel(%this, GameBase::getDataName(%this).maxDamage + 1);
}

function Flare::light(%this)
{
	if(gamebase::getposition(%this) != "0 0 0"){
		
	Projectile::spawnProjectile("FlareBurst", "0 0 0 0 0 0 0 0 0 " @ gamebase::getposition(%this), 2048, "0 0 0");


	schedule("Flare::light(" @ %this @ ");", 0.5, %this);
	
	}
}

