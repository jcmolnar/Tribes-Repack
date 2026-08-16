# Damage Types
#
$ImpactDamageType		  = -1;
$LandingDamageType	  =  0;
$BulletDamageType      =  1;
$EnergyDamageType      =  2;
$PlasmaDamageType      =  3;
$ExplosionDamageType   =  4;
$ShrapnelDamageType    =  5;
$LaserDamageType       =  6;
$MortarDamageType      =  7;
$BlasterDamageType     =  8;
$ElectricityDamageType =  9;
$CrushDamageType       = 10;
$DebrisDamageType      = 11;
$MissileDamageType     = 12;
$MineDamageType        = 13;
// DELTA FORCE
$ChargeDamageType      = 14;
$AirstrikeDamageType   = 15; 
$BleedDamageType       = 16;
$GrenadeDamageType     = 17;
$PoisonDamageType      = 18;
$SmokeDamageType       = 19;
$StabDamageType       = 20;

// END DELTA FORCE

// DELTA FORCE PROJECTILES

function bombTarget( %player, %weapontype)
{
	echo(%weapontype @ " strike called on " @ %player );
	Projectile::spawnProjectile(%weapontype, "0 0 0 0 0 0 0 0 0 " @ getword(gamebase::getposition(%player), 0) @ " " @ getword(gamebase::getposition(%player), 1) @ " 500", 2048, "0 0 0");
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//   Adding all missiles/rockets that CAN lock, or are locked, into two global tables: Radar lock, 
//     and infared lock. They will be removed and added accodrningly on birth and death.

function AddToILM(%this)
{
	
	%Done = false;
	//Assume less then 50 missiles total.
	for(%x = 0; !%Done; %x++)
	{
		if($InfaredLockers[%x] == "")
		{
			$InfaredLockers[%x] = %this;
			%Done = true;
		}
	}
}

function RemoveFromILM(%this)
{

	%Done = false;
	//Assume less then 50 missiles total.
	for(%x = 0; %x < 50 && !%Done; %x++)
	{
		if($InfaredLockers[%x] == %this)
		{
			$InfaredLockers[%x] = "";
			%Done = true;
		}
	}		

}

function AddToRLM(%this)
{
	
	%Done = false;
	//Assume less then 50 missiles total.
	for(%x = 0; !%Done; %x++)
	{
		if($RadarLockers[%x] == "")
		{
			$RadarLockers[%x] = %this;
			%Done = true;
		}
	}
}

function RemoveFromRLM(%this)
{

	%Done = false;
	//Assume less then 50 missiles total.
	for(%x = 0; %x < 50 && !%Done; %x++)
	{
		if($RadarLockers[%x] == %this)
		{
			$RadarLockers[%x] = "";
			%Done = true;
		}
	}
}

GrenadeData FlareBurst
{
   bulletShapeName    = "shotgunbolt.dts";
   explosionTag       = blasterExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 1.0;
   mass               = 1.0;
   elasticity         = 0.45;

   aimDeflection      = 0.001;
   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.1;
   damageType         = $PlasmaDamageType;

   explosionRadius    = 3;
   kickBackStrength   = 10.0;
   maxLevelFlightDist = -5;
   totalTime          = 10.0;    // special meaning for grenades...
   liveTime           = 0.5;
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 0.4;

   smokeName              = "shotgunex.dts";
};

function FlareBurst::onAdd(%this)
{
	%obj = newObject("","Item","GhostTarget",10,true);
	addToSet("MissionCleanup", %obj);
	GameBase::setPosition(%obj, gamebase::getposition(%this));
	
	$GTForP[%this] = %obj;
	$PForGT[%obj] = %this;
	
	
	schedule("StickGhost(" @ %this @ ", 0);",0.1,%this);
	
	schedule("FlareCheck(" @ %this @ ");",0.1,%this);
}

function FlareBurst::onRemove(%this)
{
	deleteObject($GTForP[%this]);

}



//Attract only one missile. Not realistic persay but.... game balance must be maintained.
function FlareCheck(%this)
{
	%position = GameBase::getPosition(%this);  //GameBase::getLOSInfo(%player,3); dont test where the shot is aimed, test from the position of the bullet
	%target = "";
	%closest = "";
	for(%i = 0; %i < 50; %i++) {
		if($InfaredLockers[%i] != "") {
			if(Vector::getDistance(%position,gamebase::Getposition($InfaredLockers[%i])) < Vector::getDistance(%position,gamebase::Getposition(%closest)) || %closest == "")
				%closest = $InfaredLockers[%i];
		}
	}
	if(Vector::getDistance(%position,gamebase::Getposition(%closest)) < 100)
		%target = %closest;
	// Don't always work. That wouldn't be to correct.
	if(%target == "" || floor(getRandom()*100) > 5 )
		schedule("FlareCheck(" @ %this @ ");",0.1,%this);
	else {	
		%player = 2048;
	
		%rotation = GameBase::getRotation(%target); // get rotation of object...
		%velocity = Item::getVelocity(%target); // get velocity of object...
		%transform = "0 0 0 0 0 0 "@ %rotation @" "@ gamebase::getposition(%target); // make the transform for the new object...
			
		Projectile::spawnProjectile(DeadMissile, %transform, %player, %velocity, $GTForP[%this]); // spawn new seeking projectile
		deleteObject(%target); // and delete original projectile
	}
}

GrenadeData Chaff
{
   bulletShapeName    = "force.dts";
   explosionTag       = bulletExp0;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 1.0;
   mass               = 1.0;
   elasticity         = 0.45;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.1;
   damageType         = $BulletDamageType;

   explosionRadius    = 1;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = -5;
   totalTime          = 10.0;    // special meaning for grenades...
   liveTime           = 0.5;
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 0.2;

   smokeName              = "breath.dts";
};

GrenadeData ChaffBurst
{
   bulletShapeName    = "force.dts";
   explosionTag       = bulletExp0;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 1.0;
   mass               = 1.0;
   elasticity         = 0.45;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.1;
   damageType         = $BulletDamageType;

   explosionRadius    = 0;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = -5;
   totalTime          = 10.0;    // special meaning for grenades...
   liveTime           = 0.5;
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 0.2;

   smokeName              = "breath.dts";
};

function ChaffBurst::onAdd(%this)
{
	%obj = newObject("","Item","GhostTarget",10,true);
	addToSet("MissionCleanup", %obj);
	GameBase::setPosition(%obj, gamebase::getposition(%this));
	
	$GTForP[%this] = %obj;
	$PForGT[%obj] = %this;
	
	
	schedule("StickGhost(" @ %this @ ", 0);",0.1,%this);
	
	schedule("ChaffCheck(" @ %this @ ");",0.1,%this);
}

function ChaffBurst::onRemove(%this)
{
	deleteObject($GTForP[%this]);

}


//Attract only one missile. Not realistic persay but.... game balance must be maintained.
function ChaffCheck(%this)
{
	%position = GameBase::getPosition(%this);  //GameBase::getLOSInfo(%player,3); dont test where the shot is aimed, test from the position of the bullet
	%target ="";
	%closest = "";
	for(%i = 0; %i < 50; %i++) {
		if($RadarLockers[%i] != "") {
			if(Vector::getDistance(%position,gamebase::Getposition($RadarLockers[%i])) < Vector::getDistance(%position,gamebase::Getposition(%closest)) || %closest == "")
				%closest = $RadarLockers[%i];
		}
	}
	if(Vector::getDistance(%position,gamebase::Getposition(%closest)) < 100)
		%target = %closest;
	if(%target == "" || floor(getRandom()*100) > 5)
		schedule("ChaffCheck(" @ %this @ ");",0.1,%this);
	else {	
		%player = 2048;
	
		%rotation = GameBase::getRotation(%target); // get rotation of object...
		%velocity = Item::getVelocity(%target); // get velocity of object...
		%transform = "0 0 0 0 0 0 "@ %rotation @" "@ gamebase::getposition(%target); // make the transform for the new object...
			
		Projectile::spawnProjectile(DeadMissile, %transform, %player, %velocity, $GTForP[%this]); // spawn new seeking projectile
		deleteObject(%target); // and delete original projectile
	}
}

function StickGhost(%this, %done)
{
	if(%done < 100){
		%done++;
		%obj = $GTForP[%this];
		gamebase::setposition(%obj, gamebase::getposition(%this));
		schedule("StickGhost(" @ %this @ ", " @ %done @ ");",0.1,%this);
	}

}


GrenadeData FlareBurst3
{
   bulletShapeName    = "shotgunex.dts";
   explosionTag       = blasterExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.3;
   mass               = 5.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.001;
   damageType         = $PlasmaDamageType;

   explosionRadius    = 2;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 12.5;
   totalTime          = 2.0;
   liveTime           = 2.0;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 1.0;

   smokeName              = "enex.dts";
};

GrenadeData FlareBurst2
{
   bulletShapeName    = "shotgunbolt.dts";
   explosionTag       = blasterExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.2;
   mass               = 1.0;
   elasticity         = 0.45;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.4;
   damageType         = $ShrapnelDamageType;

   explosionRadius    = 15;
   kickBackStrength   = 150.0;
   maxLevelFlightDist = 150;
   totalTime          = 30.0;    // special meaning for grenades...
   liveTime           = 1.0;
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 0.5;

   smokeName              = "shotgunbolt.dts";

};



SeekingMissileData DeadMissile
{
   bulletShapeName = "rocket.dts";
   explosionTag    = rocketExp;
   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 0;       // 0 impact, 1, radius
   damageValue      = 2.4;
   damageType       = $MissileDamageType;
   explosionRadius  = 3.5;
   kickBackStrength = 175.0;

   muzzleVelocity    = 90.0;
   totalTime         = 8;
   liveTime          = 8;
   seekingTurningRadius    = 5;
   nonSeekingTurningRadius = 60.0;
   proximityDist     = 1.5;
   smokeDist         = 1.75;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

   soundId = SoundJetHeavy;
};


//==================================TEST PROJ

SeekingMissileData TestMissile
{
   bulletShapeName = "rocket.dts";
   explosionTag    = rocketExp;
   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 0;       // 0 impact, 1, radius
   damageValue      = 2.4;
   damageType       = $MissileDamageType;
   explosionRadius  = 3.5;
   kickBackStrength = 175.0;

   muzzleVelocity    = 90.0;
   totalTime         = 8;
   liveTime          = 8;
   seekingTurningRadius    = 10;
   nonSeekingTurningRadius = 75.0;
   proximityDist     = 1.5;
   smokeDist         = 1.75;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

   soundId = SoundJetHeavy;
};


RocketData TestRocket
{
   bulletShapeName  = "rocket.dts";
   explosionTag     = rocketExp;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 3.0;
   damageType       = $MissileDamageType;

   explosionRadius  = 9.5;
   kickBackStrength = 250.0;
   muzzleVelocity   = 80.0;
   terminalVelocity = 150.0;
   acceleration     = 5.0;
   totalTime        = 60.0;
   liveTime         = 0.0;
   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };
   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "rsmoke.dts";
   smokeDist   = 1.8;

   soundId = SoundJetHeavy;
};

function TestRocket::onAdd(%this)
{
	schedule("TestBoxCheck(" @ %this @ ");",0.5,%this);
}

function TestBoxCheck(%this)
{
	%position = GameBase::getPosition(%this);  //GameBase::getLOSInfo(%player,3); dont test where the shot is aimed, test from the position of the bullet
	%set = newObject("set",SimSet); // make new box for testing.
	if(containerBoxFillSet(%set,$SimPlayerObjectType|$VehicleObjectType,%position,100,100,100,0)){ // if we find a player or vehicle in the box,
		%player = 2048;
		%target = Group::getObject(%set,0); // get the objNum of the found object.
		// GameBase::getDataName(
		%rotation = GameBase::getRotation(%this); // get rotation of object...
		%velocity = Item::getVelocity(%this); // get velocity of object...
		%transform = "0 0 0 0 0 0 "@ %rotation @" "@ %position; // make the transform for the new object...
		
		Projectile::spawnProjectile(TestMissile, %transform, %player, %velocity, %target); // spawn new seeking projectile
		warningMessage(GameBase::getControlClient(%target), %this); // send warning to target
		deleteObject(%this); // and delete original projectile
	
	} else { // else if nothing significant is inside the box
		schedule("TestBoxCheck(" @ %this @ ");",0.1,%this); // then run this bad boy again.
	}
	deleteObject(%set); // then delete current box no matter what.
				
	
}


//==================================TEST PROJ

// -------------------------------------
BulletData KnifeStab
{
   bulletShapeName    = "bullet.dts";
   explosionTag       = bulletExp0;
   mass               = 0.0;
   
   damageClass        = 0;
   damageValue        = 0.0;
   damageType         = $StabDamageType;

   muzzleVelocity     = 0.0;
   totalTime          = 0.0;
   
   lightRange         = 0.0;
   lightColor         = { 1.0, 0, 0 };
   inheritedVelocityScale = 0.0;
   isVisible          = false;

};

BulletData PSG1Bullet
{
   bulletShapeName    = "bullet.dts";
   explosionTag       = bulletExp0;
   expRandCycle       = 3;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.5;
   damageType         = $LaserDamageType;

   aimDeflection      = 0.0009;
   muzzleVelocity     = 650.0;
   totalTime          = 1.5;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 1.0;
   tracerLength       = 5;
};

// -------------------------------------------------

BulletData FiftyCalBullet
{
   bulletShapeName    = "bullet.dts";
   explosionTag       = bulletExp0;
   expRandCycle       = 3;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.85;
   damageType         = $LaserDamageType;

   aimDeflection      = 0.0005;
   muzzleVelocity     = 750.0;
   totalTime          = 1.5;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 1.0;
   tracerLength       = 5;
};

// ---------------------------------------

BulletData NATOBullet
{
   bulletShapeName    = "bullet.dts";
   explosionTag       = bulletExp0;
   expRandCycle       = 3;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.3;
   damageType         = $BulletDamageType;

   aimDeflection      = 0.005;
   muzzleVelocity     = 425.0;
   totalTime          = 1.5;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 50.0;
   tracerLength       = 50;
};

// ---------------------------------------

BulletData TwentyFivemmBullet
{
   bulletShapeName    = "bullet.dts";
   explosionTag       = bulletExp0;
   expRandCycle       = 3;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.4;
   damageType         = $BulletDamageType;

   aimDeflection      = 0.005;
   muzzleVelocity     = 425.0;
   totalTime          = 1.5;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 50.0;
   tracerLength       = 50;
};

// ---------------------------------------

BulletData SAWBullet
{
   bulletShapeName    = "bullet.dts";
   explosionTag       = bulletExp0;
   expRandCycle       = 3;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.3;
   damageType         = $BulletDamageType;

   aimDeflection      = 0.008;
   muzzleVelocity     = 500.0;
   totalTime          = 1.5;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 100.0;
   tracerLength       = 50;
};

// ---------------------------------------

BulletData MP5Bullet
{
   bulletShapeName    = "bullet.dts";
   explosionTag       = bulletExp0;
   expRandCycle       = 3;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.2;
   damageType         = $BulletDamageType;

   aimDeflection      = 0.02;
   muzzleVelocity     = 500.0;
   totalTime          = 1.5;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 100.0;
   tracerLength       = 50;
};

// ---------------------------------------------

GrenadeData AntiAirShell
{
   bulletShapeName    = "grenade.dts";
   explosionTag       = grenadeExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 1.0;
   mass               = 1.0;
   elasticity         = 0.45;

   aimDeflection      = 0.08;
   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.5;
   damageType         = $ShrapnelDamageType;

   explosionRadius    = 20;
   kickBackStrength   = 150.0;
   maxLevelFlightDist = 1200;
   totalTime          = 1.3;    // special meaning for grenades...
   liveTime           = 0.0;
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 0.5;

   smokeName              = "smoke.dts";
};

GrenadeData AntiAirShell2
{
   bulletShapeName    = "grenade.dts";
   explosionTag       = grenadeExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 1.0;
   mass               = 1.0;
   elasticity         = 0.45;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.5;
   damageType         = $ShrapnelDamageType;

   explosionRadius    = 20;
   kickBackStrength   = 150.0;
   maxLevelFlightDist = 1200;
   totalTime          = 0.0;    // special meaning for grenades...
   liveTime           = 0.3;
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 1.0;

   smokeName              = "smoke.dts";
};

function AntiAirShell::onAdd(%this)
{
	schedule("AASBoxCheck(" @ %this @ ");",0.2,%this);
}

function AASBoxCheck(%this)
{
	%position = GameBase::getPosition(%this);  //GameBase::getLOSInfo(%player,3); dont test where the shot is aimed, test from the position of the bullet
	%set = newObject("set",SimSet); // make new box for testing.
	if(containerBoxFillSet(%set,$VehicleObjectType,%position,20,20,20,0)){ // if we find a player or vehicle in the box,

		%num = containerBoxFillSet(%set,$VehicleObjectType,%position,20,20,20,0);
		%num = CheckForAircraft(%set, %position, %num);
		if(%num != -1){ // as long as we found a flying vehicle
			%player = 2048; // have to make the attacker flag on the projectile a null or else things will crash.
			// GameBase::getDataName(
			%rotation = GameBase::getRotation(%this); // get rotation of object...
			%velocity = Item::getVelocity(%this); // get velocity of object...
			%transform = "0 0 0 0 0 0 "@ %rotation @" "@ %position; // make the transform for the new object...
		
			Projectile::spawnProjectile(AntiAirShell2, %transform, %player, %velocity); // spawn new projectile
			deleteObject(%this); // and delete original projectile
		} else { // if we didnt find a flying vehicle,
			schedule("AASBoxCheck(" @ %this @ ");",0.01,%this); // then run this bad boy again.
		}
	} else { // else if nothing significant is inside the box
		schedule("AASBoxCheck(" @ %this @ ");",0.01,%this); // then run this bad boy again.
	}
	deleteObject(%set); // then delete current box no matter what.
				
	
}

// ---------------------------------------------

GrenadeData AutoGrenShell
{
   bulletShapeName    = "grenade.dts";
   explosionTag       = grenadeExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.2;
   mass               = 1.0;
   elasticity         = 0.45;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.4;
   damageType         = $ShrapnelDamageType;

   explosionRadius    = 15;
   kickBackStrength   = 150.0;
   maxLevelFlightDist = 150;
   totalTime          = 30.0;    // special meaning for grenades...
   liveTime           = 0.8;
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 0.5;

   smokeName              = "smoke.dts";
};

// ---------------------------------------------

GrenadeData BouncingMineCharge
{
   bulletShapeName    = "grenade.dts";
   explosionTag       = NewGrenExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.2;
   mass               = 1.0;
   elasticity         = 0.45;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.7;
   damageType         = $ShrapnelDamageType;

   explosionRadius    = 5;
   kickBackStrength   = 100.0;
   maxLevelFlightDist = 150;
   totalTime          = 0.1;    // special meaning for grenades...
   liveTime           = 0.05;
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 0.5;

   smokeName              = "breath.dts";
};

// -----------------------------------------

BulletData AutoShotgunFlechette
{
   bulletShapeName    = "bullet.dts";
   explosionTag       = bulletExp0;
   expRandCycle       = 3;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.1;
   damageType         = $BulletDamageType;

   aimDeflection      = 0.03;
   muzzleVelocity     = 500.0;
   totalTime          = 1.0;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 100.0;
   tracerLength       = 5;
};

BulletData AutoShotgunSlug
{
   bulletShapeName    = "bullet.dts";
   explosionTag       = bulletExp0;
   expRandCycle       = 3;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.7;
   damageType         = $BulletDamageType;

   aimDeflection      = 0.01;
   muzzleVelocity     = 500.0;
   totalTime          = 1.5;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 100.0;
   tracerLength       = 50;
};

// -----------------------------------------
GrenadeData AbramsShell
{
   bulletShapeName    = "grenade.dts";
   explosionTag       = rocketExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.1;
   mass               = 5.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 5.0;
   damageType         = $MortarDamageType;

   explosionRadius    = 5.5;
   kickBackStrength   = 250.0;
   maxLevelFlightDist = 450;
   totalTime          = 30.0;
   liveTime           = 0.0;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 0.5;
   smokeName = "smoke.dts";
};

// --------------------------------------------
GrenadeData MLRSRocket
{
   bulletShapeName    = "ammopack.dts";
   explosionTag       = MineExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.1;
   mass               = 5.0;
   elasticity         = 0.0;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 7.0;
   damageType         = $MissileDamageType;

   explosionRadius    = 13.0;
   kickBackStrength   = 400.0;
   maxLevelFlightDist = 2000;
   totalTime          = 60.0;
   liveTime           = 0.0;
   projSpecialTime    = 0.01;

   soundId = SoundJetHeavy;
   inheritedVelocityScale = 0.5;
   smokeName = "smoke.dts";
};

function MLRSRocket::onAdd(%this)
{
	AddtoRLM(%this);
	schedule("MLRSBoxCheck(" @ %this @ ");",0.5,%this);
}

function MLRSRocket::onRemove(%this)
{
	RemoveFromRLM(%this);
}


SeekingMissileData MLRSMissile
{
   bulletShapeName = "rocket.dts";
   explosionTag    = mineExp;
   collisionRadius = 0.1;
   mass            = 5.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 7.0;
   damageType       = $MissileDamageType;
   explosionRadius  = 13;
   kickBackStrength = 400.0;

   muzzleVelocity    = 70.0;
   totalTime         = 60;
   liveTime          = 60;
   seekingTurningRadius    = 10;
   nonSeekingTurningRadius = 75.0;
   proximityDist     = 1.5;
   smokeDist         = 10.0;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

   soundId = SoundJetHeavy;

};

function MLRSMissile::onAdd(%this)
{
	AddtoRLM(%this);
}

function MLRSMissile::onRemove(%this)
{
	RemoveFromRLM(%this);
}

function MLRSBoxCheck(%this)
{
	%position = GameBase::getPosition(%this);  //GameBase::getLOSInfo(%player,3); dont test where the shot is aimed, test from the position of the bullet
	%set = newObject("set",SimSet); // make new box for testing.
	if(containerBoxFillSet(%set,$VehicleObjectType,%position,30,30,30,0)){ // if we find a player or vehicle in the box,

		%num = containerBoxFillSet(%set,$VehicleObjectType,%position,30,30,30,0);
		%num = CheckForAircraft(%set, %position, %num);
		if(%num != -1){ // as long as we found a flying vehicle
			%player = %this.owner; // have to make the attacker flag on the projectile a null or else things will crash.
			%target = Group::getObject(%set,%num); // get the objNum of the found object.
			// GameBase::getDataName(
			%rotation = GameBase::getRotation(%this); // get rotation of object...
			%velocity = Item::getVelocity(%this); // get velocity of object...
			%transform = "0 0 0 0 0 0 "@ %rotation @" "@ %position; // make the transform for the new object...
		
			Projectile::spawnProjectile(MLRSMissile, %transform, %player, %velocity, %target); // spawn new seeking projectile
			warningMessage(GameBase::getControlClient(%target), %this); // send warning to target
			deleteObject(%this); // and delete original projectile
		} else { // if we didnt find a flying vehicle,
			schedule("MLRSBoxCheck(" @ %this @ ");",0.1,%this); // then run this bad boy again.
		}
	} else { // else if nothing significant is inside the box
		schedule("MLRSBoxCheck(" @ %this @ ");",0.1,%this); // then run this bad boy again.
	}
	deleteObject(%set); // then delete current box no matter what.
				
	
}

// --------------------------------------------

RocketData AirstrikeShot
{
   bulletShapeName = "discb.dts";
   explosionTag    = mortarExp;

   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 20.0;
   damageType       = $AirstrikeDamageType;

   explosionRadius  = 38.0;
   kickBackStrength = 5.0;

   muzzleVelocity   = 4000.0;
   terminalVelocity = 4500.0;
   acceleration     = 5.0;

   totalTime        = 6.5;
   liveTime         = 8.0;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 1;
   trailLength = 0;
   trailWidth  = 0.0;

   soundId = SoundDiscSpin;
};

//GrenadeData AirstrikeShell
//{
//   bulletShapeName    = "magcargo.dts";
//   explosionTag       = mortarExp;
//   collideWithOwner   = True;
//   ownerGraceMS       = 0;
//   collisionRadius    = 0.3;
//   mass               = 2.0;
//   elasticity         = 0.0;

//   damageClass        = 1;       // 0 impact, 1, radius
//   damageValue        = 5.0;
//   damageType         = $AirstrikeDamageType;

//   explosionRadius    = 38.0;
//   kickBackStrength   = 5.0;
//   maxLevelFlightDist = 0;
//   totalTime          = 30.0;
//   liveTime           = 0.0;
//   projSpecialTime    = 0.01;
//
//   inheritedVelocityScale = 0.5;
//   smokeName = "rsmoke.dts";
//};

GrenadeData AirstrikeShell
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = mortarExp;
   collideWithOwner   = True;
   ownerGraceMS       = 0;
   collisionRadius    = 1.0;
   mass               = 5.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 20.0;
   damageType         = $AirstrikeDamageType;

   explosionRadius    = 38.0;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 50.0;
   totalTime          = 100.0;
   liveTime           = 0.5;
   projSpecialTime    = 0.005;

   inheritedVelocityScale = 0.5;
   smokeName              = "smoke.dts";
};

GrenadeData BombShell
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = mortarExp;
   collideWithOwner   = True;
   ownerGraceMS       = 0;
   collisionRadius    = 0.3;
   mass               = 1000.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 11.0;
   damageType         = $MortarDamageType;

   explosionRadius    = 35.0;
   kickBackStrength   = 400.0;
   maxLevelFlightDist = 0;
   totalTime          = 30.0;
   liveTime           = 0.5;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 1.0;
   smokeName              = "smoke.dts";
};


// ---------------------------------------------
// ---------------------------------------------------------------------------------------------------------------------------
// Start stinger-lock code
// ---------------------------------------------------------------------------------------------------------------------------

RocketData StingerRocket
{
   bulletShapeName  = "rocket.dts";
   explosionTag     = rocketExp;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.5;
   damageType       = $MissileDamageType;

   explosionRadius  = 3.5;
   kickBackStrength = 175.0;
   muzzleVelocity   = 70.0;
   terminalVelocity = 90.0;
   acceleration     = 5.0;
   totalTime        = 8.0;
   liveTime         = 0.0;
   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };
   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "rsmoke.dts";
   smokeDist   = 1.8;

   soundId = SoundJetHeavy;
};

function StingerRocket::onAdd(%this)
{
	AddtoILM(%this);
	schedule("StingerBoxCheck(" @ %this @ ");",0.5,%this);

}

function StingerRocket::onRemove(%this)
{
	RemoveFromILM(%this);
}

SeekingMissileData StingerMissile
{
   bulletShapeName = "rocket.dts";
   explosionTag    = rocketExp;
   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 0;       // 0 impact, 1, radius
   damageValue      = 2.4;
   damageType       = $MissileDamageType;
   explosionRadius  = 3.5;
   kickBackStrength = 175.0;

   muzzleVelocity    = 90.0;
   totalTime         = 8;
   liveTime          = 8;
   seekingTurningRadius    = 10;
   nonSeekingTurningRadius = 75.0;
   proximityDist     = 1.5;
   smokeDist         = 1.75;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

   soundId = SoundJetHeavy;
};

function StingerMissile::onAdd(%this)
{
	AddtoILM(%this);
}

function StingerMissile::onRemove(%this)
{
	RemoveFromILM(%this);
}

SeekingMissileData Stinger2Missile
{
   bulletShapeName = "rocket.dts";
   explosionTag    = rocketExp;
   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 0;       // 0 impact, 1, radius
   damageValue      = 2.5;
   damageType       = $MissileDamageType;
   explosionRadius  = 3.5;
   kickBackStrength = 175.0;

   muzzleVelocity    = 90.0;
   totalTime         = 8;
   liveTime          = 8;
   seekingTurningRadius    = 5;
   nonSeekingTurningRadius = 60.0;
   proximityDist     = 1.5;
   smokeDist         = 1.75;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

   soundId = SoundJetHeavy;
};

function Stinger2Missile::onAdd(%this)
{
	AddtoILM(%this);
}

function Stinger2Missile::onRemove(%this)
{
	RemoveFromILM(%this);
}

function StingerBoxCheck(%this)
{
	%position = GameBase::getPosition(%this);  //GameBase::getLOSInfo(%player,3); dont test where the shot is aimed, test from the position of the bullet
	%set = newObject("set",SimSet); // make new box for testing.
	if(containerBoxFillSet(%set,$VehicleObjectType,%position,40,40,40,0)){ // if we find a player or vehicle in the box,

		%num = containerBoxFillSet(%set,$VehicleObjectType,%position,40,40,40,0);
		%num = CheckForAircraft(%set, %position, %num);
		if(%num != -1){ // as long as we found a flying vehicle
			%player = %this.owner; // have to make the attacker flag on the projectile a null or else things will crash.
			%target = Group::getObject(%set,%num); // get the objNum of the found object.
			// GameBase::getDataName(
			%rotation = GameBase::getRotation(%this); // get rotation of object...
			%velocity = Item::getVelocity(%this); // get velocity of object...
			%transform = "0 0 0 0 0 0 "@ %rotation @" "@ %position; // make the transform for the new object...
		
			Projectile::spawnProjectile(Stinger2Missile, %transform, %player, %velocity, %target); // spawn new seeking projectile
			warningMessage(GameBase::getControlClient(%target), %this); // send warning to target
			deleteObject(%this); // and delete original projectile
		} else { // if we didnt find a flying vehicle,
			schedule("StingerBoxCheck(" @ %this @ ");",0.1,%this); // then run this bad boy again.
		}
	} else { // else if nothing significant is inside the box
		schedule("StingerBoxCheck(" @ %this @ ");",0.1,%this); // then run this bad boy again.
	}
	deleteObject(%set); // then delete current box no matter what.
				
	
}
// ---------------------------------------------------------------------------------------------------------------------------
// End stinger-lock code
// ---------------------------------------------------------------------------------------------------------------------------
RocketData SidewinderRocket
{
   bulletShapeName  = "rocket.dts";
   explosionTag     = rocketExp;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.5;
   damageType       = $MissileDamageType;

   explosionRadius  = 3.5;
   kickBackStrength = 175.0;
   muzzleVelocity   = 70.0;
   terminalVelocity = 90.0;
   acceleration     = 5.0;
   totalTime        = 8.0;
   liveTime         = 0.0;
   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };
   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "rsmoke.dts";
   smokeDist   = 1.8;

   soundId = SoundJetHeavy;
};

function SidewinderRocket::onAdd(%this)
{
	AddtoILM(%this);
	schedule("SidewinderBoxCheck(" @ %this @ ");",1.3,%this);
}

function SidewinderRocket::onRemove(%this)
{
	RemoveFromILM(%this);
}

SeekingMissileData SidewinderMissile
{
   bulletShapeName = "rocket.dts";
   explosionTag    = rocketExp;
   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 0;       // 0 impact, 1, radius
   damageValue      = 2.4;
   damageType       = $MissileDamageType;
   explosionRadius  = 3.5;
   kickBackStrength = 175.0;

   muzzleVelocity    = 90.0;
   totalTime         = 8;
   liveTime          = 8;
   seekingTurningRadius    = 5;
   nonSeekingTurningRadius = 60.0;
   proximityDist     = 1.5;
   smokeDist         = 1.75;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

   soundId = SoundJetHeavy;
};
function SidewinderMissile::onAdd(%this)
{
	AddtoILM(%this);
}

function SidewinderMissile::onRemove(%this)
{
	RemoveFromILM(%this);
}

function SidewinderBoxCheck(%this)
{
	%position = GameBase::getPosition(%this);  //GameBase::getLOSInfo(%player,3); dont test where the shot is aimed, test from the position of the bullet
	%set = newObject("set",SimSet); // make new box for testing.
	if(containerBoxFillSet(%set,$VehicleObjectType,%position,40,40,40,0)){ // if we find a player or vehicle in the box,

		%num = containerBoxFillSet(%set,$VehicleObjectType,%position,40,40,40,0);
		%num = CheckForAircraft(%set, %position, %num);
		if(%num != -1){ // as long as we found a flying vehicle
			%player = %this.owner; // have to make the attacker flag on the projectile a null or else things will crash.
			%target = Group::getObject(%set,%num); // get the objNum of the found object.
			// GameBase::getDataName(
			%rotation = GameBase::getRotation(%this); // get rotation of object...
			%velocity = Item::getVelocity(%this); // get velocity of object...
			%transform = "0 0 0 0 0 0 "@ %rotation @" "@ %position; // make the transform for the new object...
		
			Projectile::spawnProjectile(SidewinderMissile, %transform, %player, %velocity, %target); // spawn new seeking projectile
			warningMessage(GameBase::getControlClient(%target), %this); // send warning to target
			deleteObject(%this); // and delete original projectile
		} else { // if we didnt find a flying vehicle,
			schedule("SidewinderBoxCheck(" @ %this @ ");",0.1,%this); // then run this bad boy again.
		}
	} else { // else if nothing significant is inside the box
		schedule("SidewinderBoxCheck(" @ %this @ ");",0.1,%this); // then run this bad boy again.
	}
	deleteObject(%set); // then delete current box no matter what.
				
	
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


RocketData LAWRocket
{
   bulletShapeName  = "rocket.dts";
   explosionTag     = rocketExp;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 3.0;
   damageType       = $MissileDamageType;

   explosionRadius  = 9.5;
   kickBackStrength = 250.0;
   muzzleVelocity   = 80.0;
   terminalVelocity = 100.0;
   acceleration     = 5.0;
   totalTime        = 5.0;
   liveTime         = 0.0;
   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };
   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "rsmoke.dts";
   smokeDist   = 1.8;

   soundId = SoundJetHeavy;
};

// --------------------------------------------

GrenadeData Flame
{
   bulletShapeName    = "plasmabolt.dts";
   explosionTag       = plasmaExp;
   collideWithOwner   = True;
   ownerGraceMS       = 400;
   collisionRadius    = 3.0;
   mass               = 5.0;
   elasticity         = 0.0;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.3; // 0.3
   damageType         = $PlasmaDamageType;

   explosionRadius    = 9.0;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 300.0;
   totalTime          = 0.2;
   liveTime           = 0.0;
   projSpecialTime    = 0.005;

   inheritedVelocityScale = 0.5;
   smokeName              = "plasmabolt.dts";
};

GrenadeData AirFlame
{
   bulletShapeName    = "plasmabolt.dts";
   explosionTag       = plasmaExp;
   collideWithOwner   = True;
   ownerGraceMS       = 400;
   collisionRadius    = 1.0;
   mass               = 5.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.5;
   damageType         = $PlasmaDamageType;

   explosionRadius    = 15.0;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 50.0;
   totalTime          = 100.0;
   liveTime           = 0.5;
   projSpecialTime    = 0.005;

   inheritedVelocityScale = 0.5;
   smokeName              = "plasmabolt.dts";
};

RocketData FlameExplosion
{
   bulletShapeName  = "rocket.dts";
   explosionTag     = mortarExp;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 4.0;
   damageType       = $PlasmaDamageType;

   explosionRadius  = 15.0;
   kickBackStrength = 250.0;
   muzzleVelocity   = 0.0001;
   terminalVelocity = 0.0001;
   acceleration     = 0.0001;
   totalTime        = 0.0001;
   liveTime         = 0.0001;
   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };
   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "rsmoke.dts";
   smokeDist   = 1.8;

   soundId = SoundJetHeavy;
};

GrenadeData FlameExplosionTwo
{
   bulletShapeName    = "plasmabolt.dts";
   explosionTag       = plasmaExp;
   collideWithOwner   = True;
   ownerGraceMS       = 400;
   collisionRadius    = 1.0;
   mass               = 5.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 1.0;
   damageType         = $PlasmaDamageType;

   explosionRadius    = 7.0;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 0.0;
   totalTime          = 0.0;
   liveTime           = 0.0;
   projSpecialTime    = 0.005;

   inheritedVelocityScale = 0.5;
   smokeName              = "plasmabolt.dts";
};

GrenadeData FlameExplosionThree
{
   bulletShapeName    = "plasmabolt.dts";
   explosionTag       = mineExp;
   collideWithOwner   = True;
   ownerGraceMS       = 400;
   collisionRadius    = 1.0;
   mass               = 5.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 1.0;
   damageType         = $ShrapnelDamageType;

   explosionRadius    = 15.0;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 0.0;
   totalTime          = 0.0;
   liveTime           = 0.0;
   projSpecialTime    = 0.005;

   inheritedVelocityScale = 0.5;
   smokeName              = "plasmabolt.dts";
};


// --------------------------------------------

GrenadeData HowitzerShell
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = mortarExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.2;
   mass               = 2.0;
   elasticity         = 0.0;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 5.5;
   damageType         = $MortarDamageType;

   explosionRadius    = 35.0;
   kickBackStrength   = 250.0;
   maxLevelFlightDist = 325;
   totalTime          = 30.0;
   liveTime           = 0.0;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 0.5;
   smokeName = "smoke.dts";
};

GrenadeData BigBomb
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = mortarExp;
   collideWithOwner   = True;
   ownerGraceMS       = 0;
   collisionRadius    = 0.0;
   mass               = 2.0;
   elasticity         = 0.0;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 10.0;
   damageType         = $AirstrikeDamageType;

   explosionRadius    = 30.0;
   kickBackStrength   = 250.0;
   maxLevelFlightDist = 0;
   totalTime          = 30.0;
   liveTime           = 0.5;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 1.0;
   smokeName = "smoke.dts";
};


// ---------------------------------------------

TargetLaserData AirstrikeLaserTemp
{
   laserBitmapName   = "laserPulse.bmp";

   damageConversion  = 0.0;
   baseDamageType    = 0;

   lightRange        = 2.0;
   lightColor        = { 0.25, 1.0, 0.25 };

   detachFromShooter = false;
};
//TODO: Finish this

// ------------------------------------------

RocketData ApacheRocket
{
   bulletShapeName  = "rocket.dts";
   explosionTag     = rocketExp;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 1.0;
   damageType       = $MissileDamageType;

   explosionRadius  = 6.5;
   kickBackStrength = 250.0;
   muzzleVelocity   = 90.0;
   terminalVelocity = 100.0;
   acceleration     = 5.0;
   totalTime        = 8.0;
   liveTime         = 11.0;
   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };
   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "rsmoke.dts";
   smokeDist   = 1.8;

   soundId = SoundJetHeavy;
};

// ------------------------------------------

RocketData HydraRocket
{
   bulletShapeName  = "grenade.dts";
   explosionTag     = NewGrenExp;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.5;
   damageType       = $MissileDamageType;

   explosionRadius  = 3.5;
   kickBackStrength = 100.0;
   muzzleVelocity   = 90.0;
   terminalVelocity = 110.0;
   acceleration     = 5.0;
   totalTime        = 8.0;
   liveTime         = 11.0;
   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };
   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "breath.dts";
   smokeDist   = 1.8;

   soundId = SoundJetHeavy;
};
// ------------------------------------------

RocketData PhoenixRocket
{
   bulletShapeName  = "rocket.dts";
   explosionTag     = rocketExp;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 2.5;
   damageType       = $MissileDamageType;

   explosionRadius  = 4.5;
   kickBackStrength = 250.0;
   muzzleVelocity   = 95.0;
   terminalVelocity = 150.0;
   acceleration     = 10.0;
   totalTime        = 8.0;
   liveTime         = 11.0;
   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };
   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "rsmoke.dts";
   smokeDist   = 1.8;

   soundId = SoundJetHeavy;
};

function PhoenixRocket::onAdd(%this)
{
	AddtoRLM(%this);
	if($LockingMode[%clientId] == 1){
		schedule("PhoenixBoxCheck(" @ %this @ ");",1.0,%this);
	}
}

function PhoenixRocket::onRemove(%this)
{
	RemoveFromRLM(%this);
}


SeekingMissileData PhoenixMissile
{
   bulletShapeName = "rocket.dts";
   explosionTag    = rocketExp;
   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 0;       // 0 impact, 1, radius
   damageValue      = 2.5;
   damageType       = $MissileDamageType;
   explosionRadius  = 4.5;
   kickBackStrength = 250.0;

   muzzleVelocity    = 95.0;
   totalTime         = 8;
   liveTime          = 8;
   seekingTurningRadius    = 5;
   nonSeekingTurningRadius = 60.0;
   proximityDist     = 1.5;
   smokeDist         = 1.8;

   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };

   inheritedVelocityScale = 0.5;

   soundId = SoundJetHeavy;
};

function PhoenixMissile::onAdd(%this)
{
	AddtoRLM(%this);
}

function PhoenixMissile::onRemove(%this)
{
	RemoveFromRLM(%this);
}


function PhoenixBoxCheck(%this)
{
	%position = GameBase::getPosition(%this);  //GameBase::getLOSInfo(%player,3); dont test where the shot is aimed, test from the position of the bullet
	%set = newObject("set",SimSet); // make new box for testing.
	if(containerBoxFillSet(%set,$VehicleObjectType,%position,20,20,20,0)){ // if we find a player or vehicle in the box,

		%num = containerBoxFillSet(%set,$VehicleObjectType,%position,20,20,20,0);
		%num = CheckForAircraft(%set, %position, %num);
		if(%num != -1){ // as long as we found a flying vehicle
			%player = %this.owner; // have to make the attacker flag on the projectile a null or else things will crash.
			%target = Group::getObject(%set,%num); // get the objNum of the found object.
			// GameBase::getDataName(
			%rotation = GameBase::getRotation(%this); // get rotation of object...
			%velocity = Item::getVelocity(%this); // get velocity of object...
			%transform = "0 0 0 0 0 0 "@ %rotation @" "@ %position; // make the transform for the new object...
		
			Projectile::spawnProjectile(PhoenixMissile, %transform, %player, %velocity, %target); // spawn new seeking projectile
			warningMessage(GameBase::getControlClient(%target), %this); // send warning to target
			deleteObject(%this); // and delete original projectile
		} else { // if we didnt find a flying vehicle,
			schedule("PhoenixBoxCheck(" @ %this @ ");",0.1,%this); // then run this bad boy again.
		}
	} else { // else if nothing significant is inside the box
		schedule("PhoenixBoxCheck(" @ %this @ ");",0.1,%this); // then run this bad boy again.
	}
	deleteObject(%set); // then delete current box no matter what.
				
	
}
// ------------------------------------------

RocketData MavMiss
{
   bulletShapeName  = "rocket.dts";
   explosionTag     = mineExp;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 5.0;
   damageType       = $MortarDamageType;

   explosionRadius  = 15.0;
   kickBackStrength = 300.0;
   muzzleVelocity   = 60.0;
   terminalVelocity = 90.0;
   acceleration     = 3.0;
   totalTime        = 10.0;
   liveTime         = 15.0;
   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };
   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "rsmoke.dts";
   smokeDist   = 1.8;

   soundId = SoundJetHeavy;
};

// --------------------------------------------

BulletData WarthogBullet
{
   bulletShapeName    = "bullet.dts";
   explosionTag       = debrisExpSmall;
   expRandCycle       = 0;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.5;
   damageType         = $BulletDamageType;

   aimDeflection      = 0.01;
   muzzleVelocity     = 650.0;
   totalTime          = 1.5;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 50.0;
   tracerLength       = 10;
};

// --------------------------------------------

BulletData FortymmBullet
{
   bulletShapeName    = "bullet.dts";
   explosionTag       = debrisExpSmall;
   expRandCycle       = 0;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.7;
   damageType         = $BulletDamageType;

   aimDeflection      = 0.001;
   muzzleVelocity     = 650.0;
   totalTime          = 1.5;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 80.0;
   tracerLength       = 10;
};

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~=====VEHICLE DAMAGE PROJECTILES
GrenadeData VehicleSmokeExpulsion
{
   bulletShapeName    = "bullet.dts";
   explosionTag       = rocketTwoExp;
   collideWithOwner   = True;
   ownerGraceMS       = 400;
   collisionRadius    = 0.0;
   mass               = -2.0;
   elasticity         = 0.0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.0; 
   damageType         = $PlasmaDamageType;

   explosionRadius    = 0.0;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 0.0;
   totalTime          = 0.1;
   liveTime           = 0.1;
   projSpecialTime    = 0.005;

   inheritedVelocityScale = 1.0;
   smokeName              = "rsmoke.dts";
};

GrenadeData VehicleFlameExpulsion
{
   bulletShapeName    = "plasmabolt.dts";
   explosionTag       = plasmaTwoExp;
   collideWithOwner   = True;
   ownerGraceMS       = 400;
   collisionRadius    = 0.0;
   mass               = -2.0;
   elasticity         = 0.0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.0; 
   damageType         = $PlasmaDamageType;

   explosionRadius    = 0.0;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 0.0;
   totalTime          = 0.005;
   liveTime           = 0.005;
   projSpecialTime    = 0.005;

   inheritedVelocityScale = 1.0;
   smokeName              = "rsmoke.dts";
};

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~=====END VEHICLE DAMAGE PROJECTILES

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~=====NEW VEHICLE WEAPONRY

RocketData HARMRocket
{
   bulletShapeName  = "rocket.dts";
   explosionTag     = MineExp;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.5;
   damageType       = $MissileDamageType;

   explosionRadius  = 1.5;
   kickBackStrength = 50.0;
   muzzleVelocity   = 50.0;
   terminalVelocity = 70.0;
   acceleration     = 5.0;
   totalTime        = 30.0;
   liveTime         = 30.0;
   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };
   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "rsmoke.dts";
   smokeDist   = 1.8;

   soundId = SoundJetHeavy;
};

function HARMRocket::onAdd(%this)
{
	AddtoRLM(%this);
	schedule("HARMBoxCheck(" @ %this @ ");",0.5,%this);
}

function HARMRocket::onRemove(%this)
{
	RemoveFromRLM(%this);
}

SeekingMissileData HARMMissile
{
   bulletShapeName = "rocket.dts";
   explosionTag    = MineExp;
   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 5.0;
   damageType       = $MissileDamageType;
   explosionRadius  = 10.0;
   kickBackStrength = 200.0;

   muzzleVelocity    = 70.0;
   totalTime         = 30;
   liveTime          = 30;
   seekingTurningRadius    = 10;
   nonSeekingTurningRadius = 75.0;
   proximityDist     = 1.5;
   smokeDist         = 1.75;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

   soundId = SoundJetHeavy;
};

function HARMMissile::onAdd(%this)
{
	AddtoRLM(%this);
}

function HARMMissile::onRemove(%this)
{
	RemoveFromRLM(%this);
}


function HARMBoxCheck(%this)
{
	%position = GameBase::getPosition(%this);  //GameBase::getLOSInfo(%player,3); dont test where the shot is aimed, test from the position of the bullet
	%set = newObject("set",SimSet); // make new box for testing.
	if(containerBoxFillSet(%set,$VehicleObjectType,%position,60,60,60,0)){ // if we find a player or vehicle in the box,

		%num = containerBoxFillSet(%set,$StaticObjectType,%position,60,60,60,0);
		%num = CheckForSensor(%set, %position, %num);
		if(%num != -1){ // as long as we found a flying vehicle
			%player = %this.owner; // have to make the attacker flag on the projectile a null or else things will crash.
			%target = Group::getObject(%set,%num); // get the objNum of the found object.
			// GameBase::getDataName(
			%rotation = GameBase::getRotation(%this); // get rotation of object...
			%velocity = Item::getVelocity(%this); // get velocity of object...
			%transform = "0 0 0 0 0 0 "@ %rotation @" "@ %position; // make the transform for the new object...
		
			Projectile::spawnProjectile(HARMMissile, %transform, %player, %velocity, %target); // spawn new seeking projectile
			warningMessage(GameBase::getControlClient(%target), %this); // send warning to target
			deleteObject(%this); // and delete original projectile
		} else { // if we didnt find a flying vehicle,
			schedule("HARMBoxCheck(" @ %this @ ");",0.1,%this); // then run this bad boy again.
		}
	} else { // else if nothing significant is inside the box
		schedule("HARMBoxCheck(" @ %this @ ");",0.1,%this); // then run this bad boy again.
	}
	deleteObject(%set); // then delete current box no matter what.
				
	
}
GrenadeData APClusterBomb
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = debrisExpSmall;
   collideWithOwner   = True;
   ownerGraceMS       = 0;
   collisionRadius    = 0.0;
   mass               = 2.0;
   elasticity         = 0.3;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 1.0;
   damageType         = $DebrisDamageType;

   explosionRadius    = 3.0;
   kickBackStrength   = 10.0;
   maxLevelFlightDist = 0;
   totalTime          = 30.0;
   liveTime           = 0.5;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 1.0;
   smokeName = "smoke.dts";
};

function APClusterBomb::onAdd(%this)
{
	schedule("APCheckGround(" @ %this @ ");",0.5,%this);
}

BulletData APBullet
{
   bulletShapeName    = "bullet.dts";
   explosionTag       = APbulletExp0;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.3;
   damageType         = $BulletDamageType;

   aimDeflection      = 0.008;
   muzzleVelocity     = 500.0;
   totalTime          = 0.4;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 100.0;
   tracerLength       = 50;
};

GrenadeData MineClusterBomb
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = debrisExpSmall;
   collideWithOwner   = True;
   ownerGraceMS       = 0;
   collisionRadius    = 0.0;
   mass               = 2.0;
   elasticity         = 0.3;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 1.0;
   damageType         = $DebrisDamageType;

   explosionRadius    = 3.0;
   kickBackStrength   = 10.0;
   maxLevelFlightDist = 0;
   totalTime          = 30.0;
   liveTime           = 0.5;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 1.0;
   smokeName = "smoke.dts";
};

function MineClusterBomb::onAdd(%this)
{
	schedule("MineCheckGround(" @ %this @ ");",0.5,%this);
}

GrenadeData BombletClusterBomb
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = debrisExpSmall;
   collideWithOwner   = True;
   ownerGraceMS       = 0;
   collisionRadius    = 0.0;
   mass               = 2.0;
   elasticity         = 0.3;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 1.0;
   damageType         = $DebrisDamageType;

   explosionRadius    = 3.0;
   kickBackStrength   = 10.0;
   maxLevelFlightDist = 0;
   totalTime          = 30.0;
   liveTime           = 0.5;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 1.0;
   smokeName = "smoke.dts";
};

function BombletClusterBomb::onAdd(%this)
{
	schedule("ClusterCheckGround(" @ %this @ ");",0.5,%this);
}

GrenadeData Bomblet
{
   bulletShapeName    = "grenade.dts";
   explosionTag       = grenadeExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.2;
   mass               = 1.0;
   elasticity         = 0.45;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.5;
   damageType         = $GrenadeDamageType;

   explosionRadius    = 15;
   kickBackStrength   = 150.0;
   maxLevelFlightDist = 150;
   totalTime          = 2.0;    // special meaning for grenades...
   liveTime           = 0.01;
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 0.5;

   smokeName              = "smoke.dts";
};

GrenadeData Tank
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = debrisExpSmall;
   collideWithOwner   = True;
   ownerGraceMS       = 0;
   collisionRadius    = 1.0;
   mass               = 2.0;
   elasticity         = 0.4;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 1.0;
   damageType         = $DebrisDamageType;

   explosionRadius    = 2.0;
   kickBackStrength   = 10.0;
   maxLevelFlightDist = 0;
   totalTime          = 30.0;
   liveTime           = 3.0;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 1.0;
   smokeName = "smoke.dts";
};

GrenadeData TankSmoke
{
   bulletShapeName    = "bullet.dts";
   explosionTag       = smokeCloudExp;
   collideWithOwner   = False;
   ownerGraceMS       = 400;
   collisionRadius    = 0.0;
   mass               = -2.0;
   elasticity         = 0.0;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.01; 
   damageType         = $SmokeDamageType;

   explosionRadius    = 17.0;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 0.0;
   totalTime          = 0.1;
   liveTime           = 0.1;
   projSpecialTime    = 0.005;

   inheritedVelocityScale = 1.0;
   smokeName              = "rsmoke.dts";
};

GrenadeData TankNerveGas
{
   bulletShapeName    = "bullet.dts";
   explosionTag       = gasCloudExp;
   collideWithOwner   = False;
   ownerGraceMS       = 400;
   collisionRadius    = 0.0;
   mass               = -2.0;
   elasticity         = 0.0;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.1; 
   damageType         = $PoisonDamageType;

   explosionRadius    = 17.0;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 0.0;
   totalTime          = 0.1;
   liveTime           = 0.1;
   projSpecialTime    = 0.005;

   inheritedVelocityScale = 1.0;
   smokeName              = "rsmoke.dts";
};

GrenadeData NapalmBomb
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = debrisExpSmall;
   collideWithOwner   = True;
   ownerGraceMS       = 0;
   collisionRadius    = 0.0;
   mass               = 2.0;
   elasticity         = 0.3;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 1.0;
   damageType         = $DebrisDamageType;

   explosionRadius    = 3.0;
   kickBackStrength   = 10.0;
   maxLevelFlightDist = 0;
   totalTime          = 30.0;
   liveTime           = 0.5;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 1.0;
   smokeName = "smoke.dts";
};

function NapalmBomb::onAdd(%this)
{
	%pos = GameBase::getPosition(%this);
	%vel = Item::getVelocity(%this); 
	%xvel = getword(%vel, 0);
	%yvel = getword(%vel, 1);
	%zvel = getword(%vel, 2);
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	schedule("NapalmCheckGround(" @ %this @ ");",0.01,%this);
	if(!$NapeSpawned || $NapeSpawned == 0){
		$NapeSpawned = 1;
		%vel = %xvel+ floor(getRandom() * ($NapeSpawned+1)) - floor(getRandom() * ($NapeSpawned+1))@ " " @%yvel + floor(getRandom() * ($NapeSpawned+1)) - floor(getRandom() * ($NapeSpawned+1))	@ " "  @%zvel + floor(getRandom() * ($NapeSpawned+1)) - floor(getRandom() * ($NapeSpawned+1));
		Projectile::spawnProjectile(NapalmBomb, %trans, %this.owner, %vel);
		//%fired = (Projectile::spawnProjectile(NapalmBomb, %trans, $ProjectileOwner[%this], %vel));
		//$ProjectileOwner[%fired] = $ProjectileOwner[%this];
	} else if( $NapeSpawned <= 3){
		$NapeSpawned++;
		%vel = %xvel + floor(getRandom() * ($NapeSpawned+1)) - floor(getRandom() * ($NapeSpawned+1))@ " " @%yvel + floor(getRandom() * ($NapeSpawned+1)) - floor(getRandom() * ($NapeSpawned+1))@ " "  @%zvel + floor(getRandom() * ($NapeSpawned+1)) - floor(getRandom() * ($NapeSpawned+1));
		Projectile::spawnProjectile(NapalmBomb, %trans, %this.owner, %vel);
		//%fired = (Projectile::spawnProjectile(NapalmBomb, %trans, $ProjectileOwner[%this], %vel));
		//$ProjectileOwner[%fired] = $ProjectileOwner[%this];
	} else {
		$NapeSpawned = 0;
	}
}

//function NapalmBomb::onRemove(%this)
//{
//	NapeStrike(%this);
//	schedule("NapeStrike("@%this@");",0.2, %this);
//}


GrenadeData NapFlame
{
   bulletShapeName    = "plasmabolt.dts";
   explosionTag       = plasmaNapExp;
   collideWithOwner   = True;
   ownerGraceMS       = 400;
   collisionRadius    = 1.0;
   mass               = 5.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 1.0; // 0.3
   damageType         = $PlasmaDamageType;

   explosionRadius    = 9.0;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 50.0;
   totalTime          = 100.0;
   liveTime           = 0.5;
   projSpecialTime    = 0.005;

   inheritedVelocityScale = 0.5;
   smokeName              = "plasmabolt.dts";
};


RocketData NukeShell2
{
   bulletShapeName  = "cactus1.dts";
   explosionTag     = NukeExp;
   collisionRadius  = 0.0;
   mass             = 9.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 1000.0;
   damageType       = $PlasmaDamageType;

   explosionRadius  = 100.0;
   kickBackStrength = 250.0;
   muzzleVelocity   = 5.0;
   terminalVelocity = 25.0;
   acceleration     = 10.0;
   totalTime        = 1000.0;
   liveTime         = 0.5;
   lightRange       = 0.01;
   lightColor       = { 1.0, 0.7, 0.5 };
   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "smoke.dts";
   smokeDist   = 0.5;

   soundId = SoundJetHeavy;
};

GrenadeData NukeShell
{
   bulletShapeName    = "cactus1.dts";
   explosionTag       = NukeExp;
   collideWithOwner   = True;
   ownerGraceMS       = 0;
   collisionRadius    = 0.0;
   mass               = 9.0;
   elasticity         = 0.3;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 1000.0;
   damageType         = $PlasmaDamageType;

   explosionRadius    = 100.0;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 0;
   totalTime          = 1000.0;
   liveTime           = 0.5;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 0.0;
   smokeName = "smoke.dts";
};

function NukeShell::onAdd(%this)
{
	//Player::applyImpulse(%this, "0 0 75");
	schedule("NukeCheckGround(" @ %this @ ");",0.5,%this);
}

GrenadeData NukeMainBlast
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = FlashExp;
   collideWithOwner   = True;
   ownerGraceMS       = 0;
   collisionRadius    = 0.0;
   mass               = 2.0;
   elasticity         = 0.3;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 1000.0;
   damageType         = $PlasmaDamageType;

   explosionRadius    = 1000.0;
   kickBackStrength   = 1000.0;
   maxLevelFlightDist = 0;
   totalTime          = 0.02;
   liveTime           = 0.01;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 1.0;
   smokeName = "smoke.dts";
};


GrenadeData EMCContainer
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = debrisExpSmall;
   collideWithOwner   = True;
   ownerGraceMS       = 0;
   collisionRadius    = 1.0;
   mass               = 2.0;
   elasticity         = 0.4;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 1.0;
   damageType         = $DebrisDamageType;

   explosionRadius    = 2.0;
   kickBackStrength   = 10.0;
   maxLevelFlightDist = 0;
   totalTime          = 30.0;
   liveTime           = 3.0;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 1.0;
   smokeName = "smoke.dts";
};

GrenadeData BunkerBusterBomb
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = debrisExpSmall;
   collideWithOwner   = True;
   ownerGraceMS       = 0;
   collisionRadius    = 1.0;
   mass               = 2.0;
   elasticity         = 0.4;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 3.0;
   damageType         = $AirstrikeDamageType;

   explosionRadius    = 2.0;
   kickBackStrength   = 10.0;
   maxLevelFlightDist = 0;
   totalTime          = 30.0;
   liveTime           = 0.5;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 1.0;
   smokeName = "smoke.dts";
};

function BunkerBuster::onadd(%this)
{
	BunkerBuster::updatePos(%this);
}

function BunkerBuster::updatePos(%this)
{
	$ProjPosition[%this] = gamebase::getposition(%this);
	schedule("BunkerBuster::updatePos(" @ %this @ ");",0.01,%this);
}

function BunkerBuster::onRemove(%this)
{
	%Pos = $ProjPosition[%this];
	BunkerBusterExplosion(%Pos);
}


GrenadeData DaisyShell
{
   bulletShapeName    = "cactus1.dts";
   explosionTag       = DaisyExp;
   collideWithOwner   = True;
   ownerGraceMS       = 0;
   collisionRadius    = 0.3;
   mass               = 15000.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 7.0;
   damageType         = $MortarDamageType;

   explosionRadius    = 100.0;
   kickBackStrength   = 1000.0;
   maxLevelFlightDist = 0;
   totalTime          = 120.0;
   liveTime           = 0.5;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 0.0;
   smokeName              = "smoke.dts";
};
function DaisyShell::onAdd(%this)
{
	%trans = "0 0 0 0 0 0 0 0 0 " @ gamebase::getposition(%this);
	%vel = item::getvelocity(%this);
	%player = %this.owner;
	Projectile::spawnProjectile(DaisySubShell, %transform, %player, %vel);

	//schedule ("Projectile::spawnProjectile(DaisySubShell, \"" @ %trans @ "\", \"" @ %player @ "\", \"" @ %vel @ "\");",0.5);
	
}	

GrenadeData DaisySubShell
{
   bulletShapeName    = "grenade.dts";
   explosionTag       = DaisySubExp;
   collideWithOwner   = True;
   ownerGraceMS       = 0;
   collisionRadius    = 0.0;
   mass               = 1.0;
   elasticity         = 0.1;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.0;
   damageType         = $MortarDamageType;

   explosionRadius    = 0.0;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 0;
   totalTime          = 120.0;
   liveTime           = 0.5;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 0.0;
   smokeName              = "smoke.dts";
};

GrenadeData SupplyDrop
{
   bulletShapeName    = "magcargo.dts";
   explosionTag       = debrisExpSmall;
   collideWithOwner   = True;
   ownerGraceMS       = 0;
   collisionRadius    = 1.0;
   mass               = 2.0;
   elasticity         = 0.4;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 1.0;
   damageType         = $DebrisDamageType;

   explosionRadius    = 2.0;
   kickBackStrength   = 10.0;
   maxLevelFlightDist = 0;
   totalTime          = 30.0;
   liveTime           = 3.0;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 1.0;
   smokeName = "smoke.dts";
};

function SupplyDrop::onadd(%this)
{
	SupplyDrop::updatePos(%this);
}

function SupplyDrop::updatePos(%this)
{
	$ProjPosition[%this] = gamebase::getposition(%this);
	schedule("SupplyDrop::updatePos(" @ %this @ ");",0.01,%this);
}

function SupplyDrop::onRemove(%this)
{
	%Pos = $ProjPosition[%this];
	SupplyDropDeploy(%Pos);
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~=====END NEW VEHICLE WEAPONRY
//==================================================================================================== Nuclear Explosion
GrenadeData NBase
{
	explosionTag       = grenadeExp;
	collideWithOwner   = True;
	ownerGraceMS       = 250;
	collisionRadius    = 0;
	mass               = 5.0;
	elasticity         = 0.0;

	damageClass        = 1;
	damageValue        = 3.5;
	damageType         = $PlasmaDamageType;

	explosionRadius    = 35;
	kickBackStrength   = 1.0;
	maxLevelFlightDist = 375;
	totalTime          = 0.1;
	liveTime           = 0.1;
	projSpecialTime    = 0.01;

	inheritedVelocityScale = 0.0;
};

GrenadeData NRing
{
	explosionTag       = LargeShockwave;
    	explosionId        = LargeShockwave;
	
	collideWithOwner   = True;
	ownerGraceMS       = 250;
	collisionRadius    = 0;
	mass               = 5.0;
	elasticity         = 0.0;

	damageClass        = 1;
	damageValue        = 2.5;
	damageType         = $PlasmaDamageType;

	explosionRadius    = 35;
	kickBackStrength   = 1.0;
	maxLevelFlightDist = 375;
	totalTime          = 1.1;
	liveTime           = 0.1;
	projSpecialTime    = 1.9;

	inheritedVelocityScale = 0.0;
};

MineData NRing1
{
	mass = 1.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0;
	friction = 10.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "grenade";
	shadowDetailMask = 4;
	explosionId = LargeShockwave;
	explosionRadius = 0.0;
	damageValue = 0.0;
	damageType = $PlasmaDamageType;
	kickBackStrength = 50;
	triggerRadius = 0.5;
	maxDamage = 10000.0;
};

function NRing1::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");",0.2,%this);
}


GrenadeData NBlast
{
	explosionTag       = grenadeExp;
	collideWithOwner   = True;
	ownerGraceMS       = 250;
	collisionRadius    = 0;
	mass               = 5.0;
	elasticity         = 0.0;
	
	damageClass        = 1;
	damageValue        = 1.0;
	damageType         = $PlasmaDamageType;

	explosionRadius    = 25;
	kickBackStrength   = 1.0;
	maxLevelFlightDist = 375;
	totalTime          = 0.1;
	liveTime           = 0.1;
	projSpecialTime    = 0.01;

	inheritedVelocityScale = 0.0;
};

GrenadeData NCloud
{
	explosionTag       = mortarExp;
	collideWithOwner   = True;
	ownerGraceMS       = 250;
	collisionRadius    = 0;
	mass               = 5.0;
	elasticity         = 0.0;

	damageClass        = 1;
	damageValue        = 1.0;
	damageType         = $PlasmaDamageType;

	explosionRadius    = 25;
	kickBackStrength   = 1.0;
	maxLevelFlightDist = 375;
	totalTime          = 0.1;
	liveTime           = 0.1;
	projSpecialTime    = 0.01;

	inheritedVelocityScale = 0.0;
};

//GrenadeData NBase
//{
//   bulletShapeName    = "bullet.dts";
//   explosionTag       = grenadeExp;
//   collideWithOwner   = True;
//   ownerGraceMS       = 250;
//   collisionRadius    = 0.0;
//   mass               = 5.0;
//   elasticity         = 0.0;

//   damageClass        = 1;
//   damageValue        = 100;
//   damageType         = $PlasmaDamageType;
//
//   explosionRadius    = 35.0;
//   kickBackStrength   = 1.0;
//   maxLevelFlightDist = 375;
//   totalTime          = 0.1;
//   liveTime           = 0.1;
//   projSpecialTime    = 0.01;
   
//   inheritedVelocityScale = 0.0;
//   smokeName              = "rsmoke.dts";
//};

//GrenadeData NRing
//{
//   bulletShapeName    = "bullet.dts";
//   explosionTag       = LargeShockWave;
//   collideWithOwner   = True;
//   ownerGraceMS       = 250;
//   collisionRadius    = 0.0;
//   mass               = 5.0;
//   elasticity         = 0.0;

//   damageClass        = 1;
//   damageValue        = 100;
//   damageType         = $PlasmaDamageType;

//   explosionRadius    = 35.0;
//   kickBackStrength   = 1.0;
//   maxLevelFlightDist = 375;
//   totalTime          = 1.1;
//   liveTime           = 0.1;
//   projSpecialTime    = 1.9;
//   
//   inheritedVelocityScale = 0.0;
//   smokeName              = "rsmoke.dts";
//};

//GrenadeData NBlast
//{
//   bulletShapeName    = "bullet.dts";
//   explosionTag       = grenadeExp;
//   collideWithOwner   = True;
//   ownerGraceMS       = 250;
//   collisionRadius    = 0.0;
//   mass               = 5.0;
//   elasticity         = 0.0;
	
//   damageClass        = 1;
//   damageValue        = 100;
//   damageType         = $PlasmaDamageType;

//   explosionRadius    = 25.0;
//   kickBackStrength   = 1.0;
//   maxLevelFlightDist = 375;
//   totalTime          = 0.1;
//   liveTime           = 0.1;
//   projSpecialTime    = 0.01;

//   inheritedVelocityScale = 0.0;
//   smokeName              = "rsmoke.dts";
//};

//GrenadeData NCloud
//{
//   bulletShapeName    = "bullet.dts";
//   explosionTag       = mortarExp;
//   collideWithOwner   = True;
//   ownerGraceMS       = 250;
//   collisionRadius    = 0.0;
//   mass               = 5.0;
//   elasticity         = 0.0;

//   damageClass        = 1;
//   damageValue        = 100;
//   damageType         = $PlasmaDamageType;
   
//   explosionRadius    = 25.0;
//   kickBackStrength   = 1.0;
//   maxLevelFlightDist = 375;
//   totalTime          = 0.1;
//   liveTime           = 0.1;
//   projSpecialTime    = 0.01;

//   inheritedVelocityScale = 0.0;
//   smokeName              = "rsmoke.dts";
//};

//GrenadeData NRing1
//{
//   bulletShapeName    = "bullet.dts";
//   explosionTag       = LargeShockWave;
//   collideWithOwner   = True;
//   ownerGraceMS       = 250;
//   collisionRadius    = 0.0;
//   mass               = 1.3;
//   elasticity         = 0.0;

//   damageClass        = 1;
//   damageValue        = 2.0;
//   damageType         = $PlasmaDamageType;
   
//   explosionRadius    = 0.0;
//   kickBackStrength   = 50.0;
//   maxLevelFlightDist = 375;
//   totalTime          = 0.1;
//   liveTime           = 0.1;
//   projSpecialTime    = 0.01;

//   inheritedVelocityScale = 0.0;
//   smokeName              = "rsmoke.dts";
//};


// ---------------------------------------------
// END DELTA FORCE PROJECTILES

//--------------------------------------
BulletData ChaingunBullet
{
   bulletShapeName    = "bullet.dts";
   explosionTag       = bulletExp0;
   expRandCycle       = 3;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.11;
   damageType         = $BulletDamageType;

   aimDeflection      = 0.005;
   muzzleVelocity     = 425.0;
   totalTime          = 1.5;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 1.0;
   tracerLength       = 30;
};

//--------------------------------------
BulletData FusionBolt
{
   bulletShapeName    = "fusionbolt.dts";
   explosionTag       = turretExp;
   mass               = 0.05;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.25;
   damageType         = $EnergyDamageType;

   muzzleVelocity     = 50.0;
   totalTime          = 6.0;
   liveTime           = 4.0;
   isVisible          = True;

   rotationPeriod = 1.5;
};

//--------------------------------------
BulletData MiniFusionBolt
{
   bulletShapeName    = "enbolt.dts";
   explosionTag       = energyExp;

   damageClass        = 0;
   damageValue        = 0.1;
   damageType         = $EnergyDamageType;

   muzzleVelocity     = 80.0;
   totalTime          = 4.0;
   liveTime           = 2.0;

   lightRange         = 3.0;
   lightColor         = { 0.25, 0.25, 1.0 };
   inheritedVelocityScale = 0.5;
   isVisible          = True;

   rotationPeriod = 1;
};

//--------------------------------------
BulletData BlasterBolt
{
   bulletShapeName    = "shotgunbolt.dts";
   explosionTag       = blasterExp;

   damageClass        = 0;
   damageValue        = 0.125;
   damageType         = $BlasterDamageType;

   muzzleVelocity     = 200.0;
   totalTime          = 2.0;
   liveTime           = 1.125;

   lightRange         = 3.0;
   lightColor         = { 1.0, 0.25, 0.25 };
   inheritedVelocityScale = 0.5;
   isVisible          = True;

   rotationPeriod = 1;
};

//--------------------------------------
BulletData PlasmaBolt
{
   bulletShapeName    = "plasmabolt.dts";
   explosionTag       = plasmaExp;

   damageClass        = 1;
   damageValue        = 0.45;
   damageType         = $PlasmaDamageType;
   explosionRadius    = 4.0;

   muzzleVelocity     = 55.0;
   totalTime          = 3.0;
   liveTime           = 2.0;
   lightRange         = 3.0;
   lightColor         = { 1, 1, 0 };
   inheritedVelocityScale = 0.3;
   isVisible          = True;

   soundId = SoundJetLight;
};

//--------------------------------------
RocketData DiscShell
{
   bulletShapeName = "discb.dts";
   explosionTag    = rocketExp;

   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.5;
   damageType       = $ExplosionDamageType;

   explosionRadius  = 7.5;
   kickBackStrength = 150.0;

   muzzleVelocity   = 65.0;
   terminalVelocity = 80.0;
   acceleration     = 5.0;

   totalTime        = 6.5;
   liveTime         = 8.0;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 1;
   trailLength = 15;
   trailWidth  = 0.3;

   soundId = SoundDiscSpin;
};

//--------------------------------------
GrenadeData GrenadeShell
{
   bulletShapeName    = "grenade.dts";
   explosionTag       = grenadeExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.2;
   mass               = 1.0;
   elasticity         = 0.45;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.4;
   damageType         = $ShrapnelDamageType;

   explosionRadius    = 15;
   kickBackStrength   = 150.0;
   maxLevelFlightDist = 150;
   totalTime          = 30.0;    // special meaning for grenades...
   liveTime           = 1.0;
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 0.5;

   smokeName              = "smoke.dts";
};

//--------------------------------------
GrenadeData MortarShell
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = mortarExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.3;
   mass               = 5.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 1.0;
   damageType         = $MortarDamageType;

   explosionRadius    = 20.0;
   kickBackStrength   = 250.0;
   maxLevelFlightDist = 275;
   totalTime          = 30.0;
   liveTime           = 2.0;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 0.5;
   smokeName              = "mortartrail.dts";
};

//--------------------------------------
GrenadeData MortarTurretShell
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = mortarExp;
   collideWithOwner   = True;
   ownerGraceMS       = 400;
   collisionRadius    = 1.0;
   mass               = 5.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 1.32;
   damageType         = $MortarDamageType;

   explosionRadius    = 30.0;
   kickBackStrength   = 250.0;
   maxLevelFlightDist = 400;
   totalTime          = 1000.0;
   liveTime           = 2.0;
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 0.5;
   smokeName              = "mortartrail.dts";
};

//--------------------------------------
RocketData FlierRocket
{
   bulletShapeName  = "rocket.dts";
   explosionTag     = rocketExp;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.5;
   damageType       = $MissileDamageType;

   explosionRadius  = 9.5;
   kickBackStrength = 250.0;
   muzzleVelocity   = 65.0;
   terminalVelocity = 87.0;
   acceleration     = 5.0;
   totalTime        = 10.0;
   liveTime         = 11.0;
   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };
   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "rsmoke.dts";
   smokeDist   = 1.8;

   soundId = SoundJetHeavy;
};

//--------------------------------------
SeekingMissileData TurretMissile
{
   bulletShapeName = "rocket.dts";
   explosionTag    = rocketExp;
   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 2.4;
   damageType       = $MissileDamageType;
   explosionRadius  = 9.5;
   kickBackStrength = 175.0;

   muzzleVelocity    = 80.0;
   totalTime         = 10;
   liveTime          = 10;
   seekingTurningRadius    = 9;
   nonSeekingTurningRadius = 75.0;
   proximityDist     = 1.5;
   smokeDist         = 1.75;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

   soundId = SoundJetHeavy;
};

function SeekingMissile::updateTargetPercentage(%target)
{
   return GameBase::virtual(%target, "getHeatFactor");
}

function TurretMissile::onAdd(%this)
{
	AddtoILM(%this);
	warningMessage($SamTarget, %this);

}

function TurretMissile::onRemove(%this)
{
	RemoveFromILM(%this);
}


//-------------------------------------- 
// These are kinda oddball dat's
// the lasers really don't fit into
// the typical projectile catagories...
//--------------------------------------
LaserData sniperLaser
{
   laserBitmapName   = "laserPulse.bmp";
   hitName           = "laserhit.dts";

   damageConversion  = 0.0;
   baseDamageType    = $LaserDamageType;

   beamTime          = 0.5;

   lightRange        = 2.0;
   lightColor        = { 1.0, 0.25, 0.25 };

   detachFromShooter = false;
   hitSoundId        = SoundLaserHit;
};

TargetLaserData targetLaser
{
   laserBitmapName   = "laserpulse.bmp";

   damageConversion  = 0.0;
   baseDamageType    = 0;

   lightRange        = 1.0;
   lightColor        = { 1, 0, 0 };

   detachFromShooter = false;
};

// DELTA FORCE

LaserData Grapple
{
	laserBitmapName = "paintPulse.bmp";

	damageConversion = 0.0;
	baseDamageType = 0;

	beamTime          = 0.2;

	lightRange = 0.0;
	lightColor = { 0, 0, 0 };

	detachFromShooter = false;
};

LightningData AirstrikeLaser
{
   bitmapName       = "paintPulse.bmp";

   damageType       = $ElectricityDamageType;
   boltLength       = 150.0;
   coneAngle        = 10.0;
   damagePerSec      = 0.0;
   energyDrainPerSec = 0.0;
   segmentDivisions = 1;
   numSegments      = 1;
   beamWidth        = 1.0;

   updateTime   = 10;
   skipPercent  = 0.0;
   displaceBias = 0.005;

   lightRange = 3.0;
   lightColor = { 0.0, 1.0, 0.0 };

   soundId = SoundELFFire;
};

// END DELTA FORCE

LightningData lightningCharge
{
   bitmapName       = "lightningNew.bmp";

   damageType       = $ElectricityDamageType;
   boltLength       = 10.0;
   coneAngle        = 35.0;
   damagePerSec      = 0.0;
   energyDrainPerSec = 0.0;
   segmentDivisions = 4;
   numSegments      = 5;
   beamWidth        = 0.125;//075;

   updateTime   = 120;
   skipPercent  = 0.5;
   displaceBias = 0.15;

   lightRange = 3.0;
   lightColor = { 0.25, 0.25, 0.85 };

   soundId = SoundELFFire;
};

LightningData turretCharge
{
   bitmapName       = "lightningNew.bmp";

   damageType       = $ElectricityDamageType;
   boltLength       = 40.0;
   coneAngle        = 35.0;
   damagePerSec      = 0.06;
   energyDrainPerSec = 60.0;
   segmentDivisions = 4;
   numSegments      = 5;
   beamWidth        = 0.125;

   updateTime   = 120;
   skipPercent  = 0.5;
   displaceBias = 0.15;

   lightRange = 3.0;
   lightColor = { 0.25, 0.25, 0.85 };

   soundId = SoundELFFire;
};

function Vector::scale(%vec, %scalar)
{
	%x = getWord(%vec, 0);
	%y = getWord(%vec, 1);
	%z = getWord(%vec, 2);

	%x *= %scalar;
	%y *= %scalar;
	%z *= %scalar;

	%result = %x @ " " @ %y @ " " @ %z;

	return %result;
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//Projectile for randomlightning

GrenadeData NaturalLightning2
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = lightningExp;
   collideWithOwner   = True;
   ownerGraceMS       = 400;
   collisionRadius    = 0.0;
   mass               = -2.0;
   elasticity         = 0.0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.0; 
   damageType         = $PlasmaDamageType;

   explosionRadius    = 0.0;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 0.0;
   totalTime          = 0.005;
   liveTime           = 0.005;
   projSpecialTime    = 0.005;

   inheritedVelocityScale = 1.0;
   smokeName              = "rsmoke.dts";
};

function NaturalLightning::damageTarget(%target, %timeSlice, %damPerSec, %enDrainPerSec, %pos, %vec, %mom, %shooterId)
{
   %damVal = %timeSlice * %damPerSec;
   %enVal  = %timeSlice * %enDrainPerSec;

   GameBase::applyDamage(%target, $ElectricityDamageType, %damVal, %pos, %vec, %mom, %shooterId);

   %energy = GameBase::getEnergy(%target);
   %energy = %energy - %enVal;
   if (%energy < 0) {
      %energy = 0;
   }
   GameBase::setEnergy(%target, %energy);
}


function LightningStrike(%misX, %misY, %misW, %misH)
{
	%X = floor(getRandom() *(%misX)); 
	%W = floor(getRandom() *(%misW)); 					
	%Y = floor(getRandom() *(%misY)); 
	%H = floor(getRandom() *(%misH)); 					
	echo("Lightning called");
	//GameBase::getLOSInfo(%player, 5000);
	%tx = %X + %W; 
	%ty = %Y + %H; 
	%tz = 100;
	//echo("Transform: " @ %transform);
	%transform = "0 0 0 0 0 0 90 0 0 "@%tx@" "@%ty@" "@%tz@"";
	Projectile::spawnProjectile("NaturalLightning", %transform, "", "0 0 0");
	Projectile::spawnProjectile("NaturalLightning2", %transform, "", "0 0 0");
	Projectile::spawnProjectile("NaturalLightning3", %transform, "", "0 0 50");
	%rnd = floor(getRandom() * 10);	
	schedule("LightningStrike(" @%misX@ ", " @%misY@ ", " @%misW@ ", " @%misH@ ");", %rnd);
	
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Lightning damage is now the reloader pack code
function lightningCharge::damageTarget(%target, %timeSlice, %damPerSec, %enDrainPerSec, %pos, %vec, %mom, %shooterId)
{
	%client = Player::getClient(%shooterId);
	if($ReloaderPackLeft[%client] <= 0)
	{
		Bottomprint(%shooterId, "<jc>Reloader Pack Empty", 2);
		Player::trigger(%player, $WeaponSlot, false);	
		return;
	}

	// DELTAFORCE
	%targetType = GameBase::getDataName(%target);
	if (%targetType == PlasmaTurret || %targetType == DeployablePlasma || %targetType == MortarTurret || %targetType == DeployableMortar || %targetType == DeployableAA) {
		if(GameBase::getEnergy(%target) < %targetType.maxEnergy) {
			GameBase::setEnergy(%target, GameBase::getEnergy(%target) + ((%targetType.maxEnergy / 4) * %timeSlice));
			if(GameBase::getEnergy(%target) >= %targetType.maxEnergy) {
				GameBase::setEnergy(%target, %targetType.maxEnergy); 
				%reloadDone[0] = 1;
				%reloadDone[1] = 1;
			}
		} else {
			%reloadDone[0] = 1;
			%reloadDone[1] = 1;
		}
	} else if (%targetType == Abrams || %targetType == Blackhawk || %targetType == Humvee || %targetType == LineBacker  || %targetType == Bradley || %targetType == MLRS) {
		if($VehicleCounterMeasuresCur[%target] < $VehicleCounterMeasures[%targetType] )
			 $VehicleCounterMeasuresCur[%target] = $VehicleCounterMeasures[%targetType];
		for(%i = 2; %i < 4; %i++) {
			if($PassengerSlot[%target, %i] != 0) {
				Client::sendMessage(%shooterId,0,"Vehicle Must be empty to reload");
				//%item = Player::getItemCount($PassengerSlot[%target, %i], $VehicleAmmoType[%targetType, %i]);
				//if (%item != $VehicleAmmoMax[ %targetType, $VehicleAmmoType[%targetType, %i] ]) {
				//	Player::incItemCount($PassengerSlot[%target, %i], $VehicleAmmoType[%targetType, %i], %timeSlice * $VehicleReloadRate[ $VehicleAmmoType[%targetType, %i] ]);
				//	if (Player::getItemCount($PassengerSlot[%target, %i], $VehicleAmmoType[%targetType, %i]) > $VehicleAmmoMax[ $VehicleAmmoType[%targetType, %i] ]) {
				//		Player::setItemCount($PassengerSlot[%target, %i], $VehicleAmmoType[%targetType, %i], $VehicleAmmoMax[ %targetType, $VehicleAmmoType[%targetType, %i] ]);
				//		%reloadDone[%i-2] = 1;
				//	}
				//} else %reloadDone[%i-2] = 1;
			} else {
				if ($VehicleAmmo[%target, $AmmoWeapon[ $VehicleAmmoType[%targetType, %i] ] ] != $VehicleAmmoMax[ %targetType, $VehicleAmmoType[%targetType, %i] ]) {
					$VehicleAmmo[%target, $AmmoWeapon[ $VehicleAmmoType[%targetType, %i] ] ] += %timeSlice * $VehicleReloadRate[ $VehicleAmmoType[%targetType, %i] ];
					if($VehicleAmmo[%target, $AmmoWeapon[ $VehicleAmmoType[%targetType, %i] ] ] > $VehicleAmmoMax[ %targetType, $VehicleAmmoType[%targetType, %i] ]) {
						$VehicleAmmo[%target, $AmmoWeapon[ $VehicleAmmoType[%targetType, %i] ] ] = $VehicleAmmoMax[ %targetType, $VehicleAmmoType[%targetType, %i] ];
						%reloadDone[%i-2] = 1;
					}
				} else %reloadDone[%i-2] = 1;
			}
		}
	} else if (%targetType == Warthog || %targetType == MoveWarthog || %targetType == Apache || %targetType == Fighter || %targetType == Fighter2 || %targetType == DiveBomber || %targetType == Bomber || %targetType == Hind || %targetType == MineLayer) {
		if($VehicleCounterMeasuresCur[%target] < $VehicleCounterMeasures[%targetType] )
			 $VehicleCounterMeasuresCur[%target] = $VehicleCounterMeasures[%targetType];
		
		for(%i = 0; %i < $VehiclePrimaryGuns[%targetType]; %i++)
		{
			%weapName = $VehicleWeapon[%targetType, %i];
			if($VehicleWeaponAmmo[%target, %weapName] < $VehicleWeaponAmmoMax[%targetType, %weapName])
			{
				$amountReloaded[%i] = %timeSlice * $WeaponReloadRate[%weapName];
				$VehicleWeaponAmmo[%target, %weapName] += $amountReloaded[%i];
				if($VehicleWeaponAmmo[%target, %weapName] >= $VehicleWeaponAmmoMax[%targetType, %weapName])
				{
					$VehicleWeaponAmmo[%target, %weapName] = $VehicleWeaponAmmoMax[%targetType, %weapName];
					$amountReloaded[%i] = 0;
				}
			}
		}

		%tempAdd = 0;
		for(%i = 0; %i < $VehiclePrimaryGuns[%targetType]; %i++)
		{
			%tempAdd += $amountReloaded[%i];
			$amountReloaded[%i] = 0;
		}

		if(%tempAdd == 0)
		{
			%reloadDone[0] = 1;
			%reloadDone[1] = 1;
		}
	} else if (%targetType == Gunship) {
		if($VehicleCounterMeasuresCur[%target] < $VehicleCounterMeasures[%targetType] )
			 $VehicleCounterMeasuresCur[%target] = $VehicleCounterMeasures[%targetType];
		for(%i = 2; %i < 6; %i++) {
			if($PassengerSlot[%target, %i] != 0) {
				Client::sendMessage(%shooterId,0,"Vehicle Must be empty to reload");
				//%item = Player::getItemCount($PassengerSlot[%target, %i], $VehicleAmmoType[%targetType, %i]);
				//if (%item != $VehicleAmmoMax[ %targetType, $VehicleAmmoType[%targetType, %i] ]) {
				//	Player::incItemCount($PassengerSlot[%target, %i], $VehicleAmmoType[%targetType, %i], %timeSlice * $VehicleReloadRate[ $VehicleAmmoType[%targetType, %i] ]);
				//	if (Player::getItemCount($PassengerSlot[%target, %i], $VehicleAmmoType[%targetType, %i]) > $VehicleAmmoMax[ $VehicleAmmoType[%targetType, %i] ]) {
				//		Player::setItemCount($PassengerSlot[%target, %i], $VehicleAmmoType[%targetType, %i], $VehicleAmmoMax[ %targetType, $VehicleAmmoType[%targetType, %i] ]);
				//		%reloadDone[%i-2] = 1;
				//	}
				//} else %reloadDone[%i-2] = 1;
			} else {
				if ($VehicleAmmo[%target, $AmmoWeapon[ $VehicleAmmoType[%targetType, %i] ] ] != $VehicleAmmoMax[ %targetType, $VehicleAmmoType[%targetType, %i] ]) {
					$VehicleAmmo[%target, $AmmoWeapon[ $VehicleAmmoType[%targetType, %i] ] ] += %timeSlice * $VehicleReloadRate[ $VehicleAmmoType[%targetType, %i] ];
					if($VehicleAmmo[%target, $AmmoWeapon[ $VehicleAmmoType[%targetType, %i] ] ] > $VehicleAmmoMax[ %targetType, $VehicleAmmoType[%targetType, %i] ]) {
						$VehicleAmmo[%target, $AmmoWeapon[ $VehicleAmmoType[%targetType, %i] ] ] = $VehicleAmmoMax[ %targetType, $VehicleAmmoType[%targetType, %i] ];
						%reloadDone[%i-2] = 1;
					}
				} else %reloadDone[%i-2] = 1;
			}
		}
	} else {
		if ($MessagePause[%shooterId] == 0) {
			Client::sendMessage(%shooterId,0,"Invalid target for reloading");
			$MessagePause[%shooterId] = 1;
			schedule("$MessagePause["@%shooterId@"] = 0;", 2);
			Player::trigger(%player, $WeaponSlot, false);	
		}
		return;
	}

	if (%reloadDone[0] == 1 && %reloadDone[1] == 1) {
		if ($MessagePause[%shooterId] == 0) {
			Client::sendMessage(%shooterId,0,"Reload completed");
			$MessagePause[%shooterId] = 1;
			schedule("$MessagePause["@%shooterId@"] = 0;", 2);
			Player::trigger(%player, $WeaponSlot, false);	
		}
		return;
	}

	$ReloaderPackLeft[%client] -= %timeSlice * 8;
	Bottomprint(%shooterId, "<jc>" @ $ReloaderPackLeft[%client] @ "% Remaining in Reloader Pack", 2);

	// END DELTAFORCE
}

RepairEffectData RepairBolt
{
   bitmapName       = "repairadd.bmp";
   boltLength       = 5.0;
   segmentDivisions = 4;
   beamWidth        = 0.125;

   updateTime   = 450;
   skipPercent  = 0.6;
   displaceBias = 0.15;

   lightRange = 3.0;
   lightColor = { 0.85, 0.25, 0.25 };
};

RepairEffectData MedicBolt
{
   bitmapName       = "repairadd.bmp";
   boltLength       = 5.0;
   segmentDivisions = 4;
   beamWidth        = 0.125;

   updateTime   = 450;
   skipPercent  = 0.6;
   displaceBias = 0.15;

   lightRange = 3.0;
   lightColor = { 0.0, 0.0, 1.0 };
};

function MedicBolt::onAcquire(%this, %player, %target)
{
	%client = Player::getClient(%player);

	if (%target == %player) {
		%player.repairTarget = -1;
		$woundedWeaponArm[Player::getClient(%player)] = 0;
		$woundedLegs[Player::getClient(%player)] = 0;
		if(Player::getArmor(%player) == "marmor2" || Player::getArmor(%player) == "marmor3")
			Player::setArmor(%player,marmor);
	   	else if(Player::getArmor(%player) == "sarmor2" || Player::getArmor(%player) == "sarmor3")
			Player::setArmor(%player,sarmor);
		else if(Player::getArmor(%player) == "iarmor2" || Player::getArmor(%player) == "iarmor3")
			Player::setArmor(%player,iarmor);
		else if(Player::getArmor(%player) == "garmor2" || Player::getArmor(%player) == "garmor3")
			Player::setArmor(%player,garmor);
		else if(Player::getArmor(%player) == "carmor2" || Player::getArmor(%player) == "carmor3")
			Player::setArmor(%player,carmor);
		else if(Player::getArmor(%player) == "earmor2" || Player::getArmor(%player) == "earmor3")
			Player::setArmor(%player,earmor);
		else if(Player::getArmor(%player) == "larmor2" || Player::getArmor(%player) == "larmor3")
			Player::setArmor(%player,larmor);
		else if(Player::getArmor(%player) == "aarmor2" || Player::getArmor(%player) == "aarmor3")
			Player::setArmor(%player,aarmor);
		else if(Player::getArmor(%player) == "mfemale2" || Player::getArmor(%player) == "mfemale3")
			Player::setArmor(%player,mfemale);
		else if(Player::getArmor(%player) == "sfemale2" || Player::getArmor(%player) == "sfemale3")
			Player::setArmor(%player,sfemale);
		else if(Player::getArmor(%player) == "ifemale2" || Player::getArmor(%player) == "ifemale3")
			Player::setArmor(%player,ifemale);
		else if(Player::getArmor(%player) == "gfemale2" || Player::getArmor(%player) == "gfemale3")
			Player::setArmor(%player,gfemale);
		else if(Player::getArmor(%player) == "cfemale2" || Player::getArmor(%player) == "cfemale3")
			Player::setArmor(%player,cfemale);
		else if(Player::getArmor(%player) == "efemale2" || Player::getArmor(%player) == "efemale3")
			Player::setArmor(%player,efemale);
		else if(Player::getArmor(%player) == "lfemale2" || Player::getArmor(%player) == "lfemale3")
			Player::setArmor(%player,lfemale);
		else if(Player::getArmor(%player) == "afemale2" || Player::getArmor(%player) == "afemale3")
			Player::setArmor(%player,afemale);
		armorChange2(%player);
		$PlayerBleeding[Player::getClient(%player)] = 0;
		if (GameBase::getDamageLevel(%player) != 0) {
			%player.repairRate = 0.05;
			%player.repairTarget = %player;
			GameBase::setAutoRepairRate(%player.repairTarget, %player.repairRate);
			Client::sendMessage(%client, 0, "AutoHeal On");
		}
		else {
			Client::sendMessage(%client,0,"Nothing in range");
			Player::trigger(%player, $WeaponSlot, false);
			return;
		}
	}
	else {
		%player.repairTarget = %target;
		%player.repairRate   = 0.1;
		
		if (getObjectType(%player.repairTarget) == "Player") {
			%rclient = Player::getClient(%player.repairTarget);
			%name = Client::getName(%rclient);
			$woundedWeaponArm[%rclient] = 0;
			$woundedLegs[%rclient] = 0;
			if(Player::getArmor(%rclient) == "marmor2" || Player::getArmor(%rclient) == "marmor3")
				Player::setArmor(%rclient,marmor);
		   	else if(Player::getArmor(%rclient) == "sarmor2" || Player::getArmor(%rclient) == "sarmor3")
				Player::setArmor(%rclient,sarmor);
			else if(Player::getArmor(%rclient) == "iarmor2" || Player::getArmor(%rclient) == "iarmor3")
				Player::setArmor(%rclient,iarmor);
			else if(Player::getArmor(%rclient) == "garmor2" || Player::getArmor(%rclient) == "garmor3")
				Player::setArmor(%rclient,garmor);
			else if(Player::getArmor(%rclient) == "carmor2" || Player::getArmor(%rclient) == "carmor3")
				Player::setArmor(%rclient,carmor);
			else if(Player::getArmor(%rclient) == "earmor2" || Player::getArmor(%rclient) == "earmor3")
				Player::setArmor(%rclient,earmor);
			else if(Player::getArmor(%rclient) == "larmor2" || Player::getArmor(%rclient) == "larmor3")
				Player::setArmor(%rclient,larmor);
			else if(Player::getArmor(%rclient) == "aarmor2" || Player::getArmor(%rclient) == "aarmor3")
				Player::setArmor(%rclient,aarmor);
			else if(Player::getArmor(%rclient) == "mfemale2" || Player::getArmor(%rclient) == "mfemale3")
				Player::setArmor(%rclient,mfemale);
			else if(Player::getArmor(%rclient) == "sfemale2" || Player::getArmor(%rclient) == "sfemale3")
				Player::setArmor(%rclient,sfemale);
			else if(Player::getArmor(%rclient) == "ifemale2" || Player::getArmor(%rclient) == "ifemale3")
				Player::setArmor(%rclient,ifemale);
			else if(Player::getArmor(%rclient) == "gfemale2" || Player::getArmor(%rclient) == "gfemale3")
				Player::setArmor(%rclient,gfemale);
			else if(Player::getArmor(%rclient) == "cfemale2" || Player::getArmor(%rclient) == "cfemale3")
				Player::setArmor(%rclient,cfemale);
			else if(Player::getArmor(%rclient) == "efemale2" || Player::getArmor(%rclient) == "efemale3")
				Player::setArmor(%rclient,efemale);
			else if(Player::getArmor(%rclient) == "lfemale2" || Player::getArmor(%rclient) == "lfemale3")
				Player::setArmor(%rclient,lfemale);
			else if(Player::getArmor(%rclient) == "afemale2" || Player::getArmor(%rclient) == "afemale3")
				Player::setArmor(%rclient,afemale);
			armorChange2(%rclient);
			$PlayerBleeding[Player::getClient(%rclient)] = 0;
			if (GameBase::getDamageLevel(%player.repairTarget) == 0) {
				Client::sendMessage(%client,0,%name @ " is not wounded");
				Player::trigger(%player,$WeaponSlot,false);
				%player.repairTarget = -1;
				return;
			}
			Client::sendMessage(%rclient,0,"Being healed by " @ Client::getName(%client));
			Client::sendMessage(%client,0,"Healing " @ %name);
			%rate = GameBase::getAutoRepairRate(%player.repairTarget) + %player.repairRate;
			GameBase::setAutoRepairRate(%player.repairTarget,%rate);
		} else {
			Client::sendMessage(%client,0,"You can't heal an inanimate object...");
			Player::trigger(%player,$WeaponSlot,false);
			%player.repairTarget = -1;
			return;
		}
	}
}

function RepairBolt::onAcquire(%this, %player, %target)
{
	
	%client = Player::getClient(%player);
	
	if(%player.driver == 1 || %client.inVehicle == true) {
		Client::sendMessage(%client,0,"You must get out to to repair the machinery");
	   	%player.repairTarget = -1;
		Player::trigger(%player, $WeaponSlot, false);
		return;
	}
			
	if (%target == %player) {
	   %player.repairTarget = -1;
		Client::sendMessage(%client,0,"Nothing in range");
		Player::trigger(%player, $WeaponSlot, false);
		return;
	} else {
		%player.repairTarget = %target;
		%player.repairRate   = 0.1;
		
		if (getObjectType(%player.repairTarget) == "Player") {
			Client::sendMessage(%client,0,"You can't repair a person...");
			Player::trigger(%player,$WeaponSlot,false);
			%player.repairTarget = -1;
			return;
		} else {
			%name = GameBase::getMapName(%target);
			if(%name == "") {
				%name = (GameBase::getDataName(%player.repairTarget)).description;
			}
			if( getObjectType(%player.repairTarget) == Flier){
				if(GameBase::getDataName(%player.repairTarget) == Apache || GameBase::getDataName(%player.repairTarget) == Fighter || GameBase::getDataName(%player.repairTarget) == Fighter2 || GameBase::getDataName(%player.repairTarget) == DiveBomber || GameBase::getDataName(%player.repairTarget) == Humvee || GameBase::getDataName(%player.repairTarget) == MineLayer)
					%player.repairRate = 0.1;				
				if(GameBase::getDataName(%player.repairTarget) == Bomber || GameBase::getDataName(%player.repairTarget) == Hercules || GameBase::getDataName(%player.repairTarget) == Gunship)
					%player.repairRate = 1.0;				
				else
					%player.repairRate = 0.6;	
			}
			if (GameBase::getDamageLevel(%player.repairTarget) == 0) {
				Client::sendMessage(%client,0,%name @ " is not damaged");
				Player::trigger(%player,$WeaponSlot,false);
				%player.repairTarget = -1;
				return;
			}
			Client::sendMessage(%client,0,"Repairing " @ %name);
			
			
			%rate = GameBase::getAutoRepairRate(%player.repairTarget) + %player.repairRate;
			GameBase::setAutoRepairRate(%player.repairTarget,%rate);	
		}
	}
}

function MedicBolt::onRelease(%this, %player)
{
	%object = %player.repairTarget;
	if (%object != -1) {
		%client = Player::getClient(%player);
		if (%object == %player) {
			Client::sendMessage(%client,0,"AutoHeal Off");
		}
		else {
			if (GameBase::getDamageLevel(%object) == 0) {
				Client::sendMessage(%client,0,"Healing Complete");
			}
			else {
				Client::sendMessage(%client,0,"Healing Stopped");
			}
		}
		%rate = GameBase::getAutoRepairRate(%object) - %player.repairRate;
      if (%rate < 0)
         %rate = 0;
      
		GameBase::setAutoRepairRate(%object,%rate);
	}
}

function RepairBolt::onRelease(%this, %player)
{
	%object = %player.repairTarget;
	if (%object != -1) {
		if (GameBase::getDamageLevel(%object) == 0) {
			Client::sendMessage(%client,0,"Repair Done");
		} else {
			Client::sendMessage(%client,0,"Repair Stopped");
		}
		%rate = GameBase::getAutoRepairRate(%object) - %player.repairRate;
      if (%rate < 0)
         %rate = 0;
      
		GameBase::setAutoRepairRate(%object,%rate);
	}
}

function MedicBolt::checkDone(%this, %player)
{
	%mountItem = Player::getMountedItem(%player,$WeaponSlot);
	if (Player::isTriggered(%player,$WeaponSlot) && 
	%mountItem == MedicGun && %player.repairTarget != -1) {
		%object = %player.repairTarget;
		if (%object == %player) {
			if (GameBase::getDamageLevel(%player) == 0) {
				Player::trigger(%player,$WeaponSlot,false);
				return;
			}
		}
		else {
			if (GameBase::getDamageLevel(%object) == 0) {
				Player::trigger(%player,$WeaponSlot,false);
				return;
			}
		}
	}
}

function RepairBolt::checkDone(%this, %player)
{
	%mountItem = Player::getMountedItem(%player,$WeaponSlot);
	if (Player::isTriggered(%player,$WeaponSlot) && 
       %mountItem == RepairGun && %player.repairTarget != -1) {
		%object = %player.repairTarget;
		if (%object == %player) {
			if (GameBase::getDamageLevel(%player) == 0) {
				Player::trigger(%player,$WeaponSlot,false);
				return;
			}
		}
		else {
			if (GameBase::getDamageLevel(%object) == 0) {
				Player::trigger(%player,$WeaponSlot,false);
				return;
			}
		}
	}
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Projectile special functions

//Random bombing for urbancommando
function BombStrike(%misX, %misY, %misW, %misH)
{
	%X = floor(getRandom() *(%misX)); 
	%W = floor(getRandom() *(%misW)); 					
	%Y = floor(getRandom() *(%misY)); 
	%H = floor(getRandom() *(%misH)); 					
	//echo("Bomb dropped");
	//GameBase::getLOSInfo(%player, 5000);
	%tx = %X + %W; 
	%ty = %Y + %H; 
	%tz = 1000;
	//echo("Transform: " @ %transform);
	%transform = "0 0 0 0 0 0 90 0 0 "@%tx@" "@%ty@" "@%tz@"";
	%randVel = floor(getRandom() * 2);
	if(%randVel == 0) { 
		Projectile::spawnProjectile("Bombshell", %transform, 2048, "0 0 0");
		Projectile::spawnProjectile("Bombshell", %transform, 2048, "5 0 5");
		Projectile::spawnProjectile("Bombshell", %transform, 2048, "10 0 10");
		Projectile::spawnProjectile("Bombshell", %transform, 2048, "15 0 15");
		Projectile::spawnProjectile("Bombshell", %transform, 2048, "20 0 20");
		Projectile::spawnProjectile("Bombshell", %transform, 2048, "25 0 25");
	} else if(%randVel == 1) { 
		Projectile::spawnProjectile("Bombshell", %transform, 2048, "0 0 0");
		Projectile::spawnProjectile("Bombshell", %transform, 2048, "0 5 5");
		Projectile::spawnProjectile("Bombshell", %transform, 2048, "0 10 10");
		Projectile::spawnProjectile("Bombshell", %transform, 2048, "0 15 15");
		Projectile::spawnProjectile("Bombshell", %transform, 2048, "0 20 20");
		Projectile::spawnProjectile("Bombshell", %transform, 2048, "0 25 25");
	} else {
		Projectile::spawnProjectile("Bombshell", %transform, 2048, "0 0 0");
		Projectile::spawnProjectile("Bombshell", %transform, 2048, "5 5 5");
		Projectile::spawnProjectile("Bombshell", %transform, 2048, "10 10 10");
		Projectile::spawnProjectile("Bombshell", %transform, 2048, "15 15 15");
		Projectile::spawnProjectile("Bombshell", %transform, 2048, "20 20 20");
		Projectile::spawnProjectile("Bombshell", %transform, 2048, "25 25 25");
	}
	%rnd = floor(getRandom() * 5);	
	schedule("BombStrike(" @%misX@ ", " @%misY@ ", " @%misW@ ", " @%misH@ ");", %rnd);
	
}


//=====for locking weapons
function warningMessage(%target, %projectile)
{
//	Client::sendMessage(%target,0,"** WARNING ** -Enemy Missile Lock detected!~waccess_denied.wav");
//	Client::sendMessage(GameBase::getControlClient(%target),0,"** WARNING ** -Enemy Missile Lock detected!");
	Client::sendMessage(%target,0,"** WARNING ** -Enemy Missile Lock detected!");
//	schedule("Client::sendMessage(" @ GameBase::getControlClient(%target) @ ",0,\"~waccess_denied.wav\");",0.5);
//	schedule("Client::sendMessage(" @ GameBase::getControlClient(%target) @ ",0,\"~waccess_denied.wav\");",1.0);
//	schedule("Client::sendMessage(" @ GameBase::getControlClient(%target) @ ",0,\"~waccess_denied.wav\");",1.5);
	schedule("Client::sendMessage(" @ %target @ ",0,\"~waccess_denied.wav\");",0);
	//Client::sendMessage(GameBase::getControlClient(%target),0,"~waccess_denied.wav");
	warningclangon(%target, %projectile);	
}


function warningclangon(%target, %projectile)
{
	if($missiles[%target] == %projectile || $missiles[%target] == "") {
		$missiles[%target] = %projectile; 
		Client::sendMessage(%target,0,"~waccess_denied.wav");
	}
	//%time = gamebase::Getposition(%target) - gamebase::Getposition(%projectile); 
	%time = Vector::getDistance(gamebase::Getposition(%target), gamebase::Getposition(%projectile));
	%time = sqrt(%time*%time);

	if(gamebase::getposition(%projectile) != "0 0 0" && %projectile != 0 && %projectile != "" && %projectile != -1 && %projectile != false){
		%time = %time/300;
		schedule("warningclangon(" @ %target @ ", " @ %projectile @ ");",%time);
	}
	else
		$missiles[%target] = "";
}



function CheckForSensor(%set,%position, %num)
{
	for(%i=0;%num>%i;%i++){ // a loop to make sure our target is a flying vehicle, not a ground based.
		if(GameBase::getClassName(Group::getObject(%set,%i)) == Sensor && GameBase::isActive(Group::getObject(%set,%i))){ // if our object is sensor, and its working
			%num = %i; // then do everything
			return %num;
		} // do nothing if it isnt, and check loop again.
	}
	return -1; // when loop comes back empty, return a -1 
		
}

function CheckForAircraft(%set,%position, %num)
{
	for(%i=0;%num>%i;%i++){ // a loop to make sure our target is a flying vehicle, not a ground based.
		if(GameBase::getDataName(Group::getObject(%set,%i)) == Fighter || GameBase::getDataName(Group::getObject(%set,%i)) == DiveBomber || GameBase::getDataName(Group::getObject(%set,%i)) == Fighter2 || GameBase::getDataName(Group::getObject(%set,%i)) == Apache || GameBase::getDataName(Group::getObject(%set,%i)) == Blackhawk || GameBase::getDataName(Group::getObject(%set,%i)) == Warthog || GameBase::getDataName(Group::getObject(%set,%i)) == MoveWarthog || GameBase::getDataName(Group::getObject(%set,%i)) == Hind || GameBase::getDataName(Group::getObject(%set,%i)) == Bomber || GameBase::getDataName(Group::getObject(%set,%i)) == Hercules || GameBase::getDataName(Group::getObject(%set,%i)) == Gunship){ // if our object is an aircraft,
			%num = %i; // then do everything
			return %num;
		} // do nothing if it isnt, and check loop again.
	}
	return -1; // when loop comes back empty, return a -1 
		
}
//=====end locking


function MLRSBalistics(%this)
{
	%projRot = GameBase::getRotation(%this);
	%projRX = getword(%projRot, 0);
	%projRY = getword(%projRot, 1);
	%projRZ = getword(%projRot, 2);
	%vel = Item::getVelocity(%this);
	%pos = GameBase::getPosition(%this);
	echo(%projRZ);
	if(%projRZ > -1.07){
		%projRZ = %projRZ - 0.01;
		%NewRot = %projRX @ " " @ %projRY @ " " @ %projRZ;
		%vela = "0 0 1.0";
		%trans = "0 0 0 0 0 0 " @ %NewRot @ %pos;
		%vel = Vector::add(%vel, %vela);
		Projectile::spawnProjectile(MLRSRocket, %trans, $ProjectileOwner[%this], %vel);
		deleteObject(%this);
	}
	schedule("MLRSBalistics(" @ %this @ ");",0.01,%this);
}

function APcheckGround(%this){
	
	%refHeight = "-1.575 0 0";
	
	GameBase::getLOSInfo(%this,20,%refHeight);
	
	%groundPos = $los::position;
	%projPos = GameBase::getPosition(%this);
	%projZ = getword(%projPos, 2);
	%groundz = getword(%groundPos, 2);
	%height = %projZ - %groundZ;
	
	
	if(%height <= 5){
		APClusterExplosion(%this);
		schedule("APClusterExplosion("@%this@");",0.1, %this);
		schedule("APClusterExplosion("@%this@");",0.2, %this);
		schedule("APClusterExplosion("@%this@");",0.3, %this);
		schedule("APClusterExplosion("@%this@");",0.4, %this);
		schedule("APClusterExplosion("@%this@");",0.5, %this);
		


	}else
		schedule("APcheckGround("@%this@");",0.01, %this);
	
}

function MinecheckGround(%this){
	
	%refHeight = "-1.575 0 0";
	
	GameBase::getLOSInfo(%this,20,%refHeight);
	
	%groundPos = $los::position;
	%projPos = GameBase::getPosition(%this);
	%projZ = getword(%projPos, 2);
	%groundz = getword(%groundPos, 2);
	%height = %projZ - %groundZ;

	if(%height <= 30)
		MineClusterExplosion(%this);
	else
		schedule("MinecheckGround("@%this@");",0.01, %this);
}

function ClustercheckGround(%this){
	
	%refHeight = "-1.575 0 0";
	
	GameBase::getLOSInfo(%this,20,%refHeight);
	
	%groundPos = $los::position;
	%projPos = GameBase::getPosition(%this);
	%projZ = getword(%projPos, 2);
	%groundz = getword(%groundPos, 2);
	%height = %projZ - %groundZ;
	
	
	if(%height <= 10){
		BombletClusterExplosion(%this);
		schedule("BombletClusterExplosion("@%this@");",0.1, %this);
		schedule("BombletClusterExplosion("@%this@");",0.2, %this);
		schedule("BombletClusterExplosion("@%this@");",0.3, %this);
	
	
	}else
		schedule("ClustercheckGround("@%this@");",0.01, %this);
	
}

function NukecheckGround(%this){
	%refHeight = "-1.575 0 0";
	
	GameBase::getLOSInfo(%this,20,%refHeight);
	
	%groundPos = $los::position;
	%projPos = GameBase::getPosition(%this);
	%projZ = getword(%projPos, 2);
	%groundz = getword(%groundPos, 2);
	%height = %projZ - %groundZ;
	
	if(%height <= 50){
		NuclearExplosion(%this);
	}else
		schedule("NukecheckGround("@%this@");",0.01, %this);
}

function NapalmcheckGround(%this){
	
	%refHeight = "-1.575 0 0";
	
	GameBase::getLOSInfo(%this,20,%refHeight);
	
	%groundPos = $los::position;
	%projPos = GameBase::getPosition(%this);
	%projZ = getword(%projPos, 2);
	%groundz = getword(%groundPos, 2);
	%height = %projZ - %groundZ;
	
	if(%height <= 5){
		NapeStrike(%this);
		schedule("NapeStrike("@%this@");",0.2, %this);
		
	}else
		schedule("NapalmcheckGround("@%this@");",0.01, %this);
		
	
}

function BunkercheckGround(%this){
	
	%refHeight = "-1.575 0 0";
	
	GameBase::getLOSInfo(%this,20,%refHeight);
	
	%groundPos = $los::position;
	%projPos = GameBase::getPosition(%this);
	%projZ = getword(%projPos, 2);
	%groundz = getword(%groundPos, 2);
	%height = %projZ - %groundZ;
	
	if(%height <= 5)
		BunkerBusterExplosion(%this);
	else
		schedule("BunkercheckGround("@%this@");",0.005, %this);
}
function MineClusterExplosion(%this){
	%pos = GameBase::getPosition(%this);
	%X = getword(%pos, 0);
	%Y = getword(%pos, 1);
	%Z = getword(%pos, 2);
	for(%i=0;%i<30;%i++){
		%newpos = floor(getRandom() * 100) - floor(getRandom() * 100) +%X @ " " @ floor(getRandom() * 100) - floor(getRandom() * 100) +%Y @ " " @ %Z;
		%obj = newObject("","Mine","AntipersonelMinegator");
		addToSet("MissionCleanup", %obj);
		GameBase::setPosition(%obj, %newpos);	
	}			
}

function VolcanicEruption(%this, %player)
{
	%pos = GameBase::getPosition(%this);
	%X = getword(%pos, 0);
	%Y = getword(%pos, 1);
	%Z = getword(%pos, 2) + 5;
	%amount = 0;
	if(%player.minetype == 3){
		%amount = 3;
		%type = AntiArmorMine;
	} else if(%player.minetype == 2){
		%amount = 4;
		%type = AntipersonelMineBouncing;
	} else {
		%amount = 5;
		%type = AntipersonelMine;
	}
	%t = 1;
	for(%i=0;%i<%amount;%i++){
		%newpos = (floor(getRandom() * 30)*%t +%X) @ " " @ (floor(getRandom() * 30)*%t +%Y) @ " " @ %Z;
		%obj = newObject("","Mine",%type);
		addToSet("MissionCleanup", %obj);
		GameBase::setPosition(%obj, %newpos);
		%obj.thrower = %player;
		%t = %t*-1;
	}

}


function test(%client)
{
	%obj = newObject("","Item","SOCOMAmmo",1,true);
	addToSet("MissionCleanup", %obj);
	GameBase::setPosition(%obj, vector::add(gamebase::getposition(%client), "0 0 3"));
	schedule("Item::Pop(" @ %obj @ ");", 10, %obj);	
}


function SupplyDropDeploy(%pos)
{
	//%pos = GameBase::getPosition(%this);
	%X = getword(%pos, 0);
	%Y = getword(%pos, 1);
	%Z = getword(%pos, 2);
	
	%newpos = vector::add(%pos, "0 0 -0.5");
	%obj = newObject("CargoCrate","StaticShape","CargoCrate",true);
	addToSet("MissionCleanup", %obj);
	GameBase::setPosition(%obj, %newpos);
	schedule("Item::Pop(" @ %obj @ ");", 120, %obj);	
		
	for(%i=0;%i<4;%i++){
		%newpos = floor(getRandom() * 50) - floor(getRandom() * 50) +%X @ " " @ floor(getRandom() * 50) - floor(getRandom() * 50) +%Y @ " " @ %Z+10;
		%obj = newObject("","Item","SOCOMAmmo",10,true);
		addToSet("MissionCleanup", %obj);
		GameBase::setPosition(%obj, %newpos);
		schedule("Item::Pop(" @ %obj @ ");", 120, %obj);	
		
		%newpos = floor(getRandom() * 50) - floor(getRandom() * 50) +%X @ " " @ floor(getRandom() * 50) - floor(getRandom() * 50) +%Y @ " " @ %Z+10;
		%obj = newObject("","Item","OICWAmmo",25,true);
		addToSet("MissionCleanup", %obj);
		GameBase::setPosition(%obj, %newpos);
		schedule("Item::Pop(" @ %obj @ ");", 120, %obj);	
		
		%newpos = floor(getRandom() * 50) - floor(getRandom() * 50) +%X @ " " @ floor(getRandom() * 50) - floor(getRandom() * 50) +%Y @ " " @ %Z+10;
		%obj = newObject("","Item","SAWAmmo",50,true);
		addToSet("MissionCleanup", %obj);
		GameBase::setPosition(%obj, %newpos);
		schedule("Item::Pop(" @ %obj @ ");", 120, %obj);	
		
		%newpos = floor(getRandom() * 50) - floor(getRandom() * 50) +%X @ " " @ floor(getRandom() * 50) - floor(getRandom() * 50) +%Y @ " " @ %Z+10;
		%obj = newObject("","Item","MP5Ammo",25,true);
		addToSet("MissionCleanup", %obj);
		GameBase::setPosition(%obj, %newpos);
		schedule("Item::Pop(" @ %obj @ ");", 120, %obj);	
		
		%newpos = floor(getRandom() * 50) - floor(getRandom() * 50) +%X @ " " @ floor(getRandom() * 50) - floor(getRandom() * 50) +%Y @ " " @ %Z+10;
		%obj = newObject("","Item","PSG1Ammo",10,true);
		addToSet("MissionCleanup", %obj);
		GameBase::setPosition(%obj, %newpos);
		schedule("Item::Pop(" @ %obj @ ");", 120, %obj);	
		
		%newpos = floor(getRandom() * 50) - floor(getRandom() * 50) +%X @ " " @ floor(getRandom() * 50) - floor(getRandom() * 50) +%Y @ " " @ %Z+10;
		%obj = newObject("","Item","FiftyCalAmmo",10,true);
		addToSet("MissionCleanup", %obj);
		GameBase::setPosition(%obj, %newpos);
		schedule("Item::Pop(" @ %obj @ ");", 120, %obj);	
		
		%newpos = floor(getRandom() * 50) - floor(getRandom() * 50) +%X @ " " @ floor(getRandom() * 50) - floor(getRandom() * 50) +%Y @ " " @ %Z+10;
		%obj = newObject("","Item","LAWAmmo",1,true);
		addToSet("MissionCleanup", %obj);
		GameBase::setPosition(%obj, %newpos);
		schedule("Item::Pop(" @ %obj @ ");", 120, %obj);	
		
		%newpos = floor(getRandom() * 50) - floor(getRandom() * 50) +%X @ " " @ floor(getRandom() * 50) - floor(getRandom() * 50) +%Y @ " " @ %Z+10;
		%obj = newObject("","Item","AutoShotgunAmmo",25,true);
		addToSet("MissionCleanup", %obj);
		GameBase::setPosition(%obj, %newpos);
		schedule("Item::Pop(" @ %obj @ ");", 120, %obj);	
		
		%newpos = floor(getRandom() * 50) - floor(getRandom() * 50) +%X @ " " @ floor(getRandom() * 50) - floor(getRandom() * 50) +%Y @ " " @ %Z+10;
		%obj = newObject("","Item","StingerAmmo",1,true);
		addToSet("MissionCleanup", %obj);
		GameBase::setPosition(%obj, %newpos);
		schedule("Item::Pop(" @ %obj @ ");", 120, %obj);	
		
		%newpos = floor(getRandom() * 50) - floor(getRandom() * 50) +%X @ " " @ floor(getRandom() * 50) - floor(getRandom() * 50) +%Y @ " " @ %Z+10;
		%obj = newObject("","Item","FlameAmmo",50,true);
		addToSet("MissionCleanup", %obj);
		GameBase::setPosition(%obj, %newpos);
		schedule("Item::Pop(" @ %obj @ ");", 120, %obj);	
		
		%newpos = floor(getRandom() * 50) - floor(getRandom() * 50) +%X @ " " @ floor(getRandom() * 50) - floor(getRandom() * 50) +%Y @ " " @ %Z+10;
		%obj = newObject("","Item","RepairKit",1,true);
		addToSet("MissionCleanup", %obj);
		GameBase::setPosition(%obj, %newpos);
		schedule("Item::Pop(" @ %obj @ ");", 120, %obj);	
		
		%newpos = floor(getRandom() * 50) - floor(getRandom() * 50) +%X @ " " @ floor(getRandom() * 50) - floor(getRandom() * 50) +%Y @ " " @ %Z+10;
		%obj = newObject("","Item","MineAmmo",1,true);
		addToSet("MissionCleanup", %obj);
		GameBase::setPosition(%obj, %newpos);
		schedule("Item::Pop(" @ %obj @ ");", 120, %obj);	
		
		%newpos = floor(getRandom() * 50) - floor(getRandom() * 50) +%X @ " " @ floor(getRandom() * 50) - floor(getRandom() * 50) +%Y @ " " @ %Z+10;
		%obj = newObject("","Item","Grenade",1,true);
		addToSet("MissionCleanup", %obj);
		GameBase::setPosition(%obj, %newpos);
		schedule("Item::Pop(" @ %obj @ ");", 120, %obj);	
		
		%newpos = floor(getRandom() * 50) - floor(getRandom() * 50) +%X @ " " @ floor(getRandom() * 50) - floor(getRandom() * 50) +%Y @ " " @ %Z+10;
		%obj = newObject("","Item","Beacon",1,true);
		addToSet("MissionCleanup", %obj);
		GameBase::setPosition(%obj, %newpos);
		schedule("Item::Pop(" @ %obj @ ");", 120, %obj);	
		
		%newpos = floor(getRandom() * 50) - floor(getRandom() * 50) +%X @ " " @ floor(getRandom() * 50) - floor(getRandom() * 50) +%Y @ " " @ %Z+10;
		%obj = newObject("","Item","RepairPatch",1,true);
		addToSet("MissionCleanup", %obj);
		GameBase::setPosition(%obj, %newpos);
		schedule("Item::Pop(" @ %obj @ ");", 120, %obj);	
		
		
	}
}


function APClusterExplosion(%this){
	%pos = GameBase::getPosition(%this);
	%vel = "0 0 0";
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	for(%i=0;%i<50;%i++){ //200
		%vel = floor(getRandom() * 500) - floor(getRandom() * 500) @ " " @ floor(getRandom() * 500) - floor(getRandom() * 500) @ " " @ floor(getRandom() * 500) - floor(getRandom() * 500);
		Projectile::spawnProjectile(APBullet, %trans, %this.owner, %vel);
	}
}

function BombletClusterExplosion(%this){
	%pos = GameBase::getPosition(%this);
	%vel = "0 0 0";
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	for(%i=0;%i<50;%i++){
		%vel = floor(getRandom() * 50) - floor(getRandom() * 50) @ " " @ floor(getRandom() * 50) - floor(getRandom() * 50) @ " " @ floor(getRandom() * 50) - floor(getRandom() * 50);
		Projectile::spawnProjectile(Bomblet, %trans, %this.owner, %vel);
		
	}
}

function NapeStrike(%this){
	%pos = GameBase::getPosition(%this);
	%vel = "0 0 0";
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	for(%i=0;%i<15;%i++){
		%vel = floor(getRandom() * 50) - floor(getRandom() * 50) @ " " @ floor(getRandom() * 50) - floor(getRandom() * 50) @ " " @ floor(getRandom() * 50) - floor(getRandom() * 50);
		Projectile::spawnProjectile(NapFlame, %trans, %this.owner, %vel);
	}
}

function BunkerBusterExplosion(%pos){
	//%pos = GameBase::getPosition(%this);
	%X = getword(%pos, 0);
	%Y = getword(%pos, 1);
	%Z = getword(%pos, 2);
	%vel = "0 0 0";
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	for(%i=0;%i<40;%i++){
		%trans = "0 0 0 0 0 0 0 0 0 " @ floor(getRandom() * 8) - floor(getRandom() * 8) + %X @ " " @ floor(getRandom() * 8) - floor(getRandom() * 8) + %Y @ " " @ %Z - floor(getRandom() * 20);
		Projectile::spawnProjectile(Bomblet, %trans, %this.owner, %vel);
	}
}


function NukeBoxKill(%this, %position)
{
	%set = newObject("set",SimSet); // make new box for killing.
	if(containerBoxFillSet(%set,$StaticObjectType | $VehicleObjectType | $SimPlayerObjectType,%position,700,700,700,0)){ // if we find a player or staticshape in the box,
		%num = containerBoxFillSet(%set,$StaticObjectType | $VehicleObjectType | $SimPlayerObjectType,%position,700,700,700,0);
		for(%i=0;%num>%i;%i++){ // To walk through the objects in the box.
			%pos = gamebase::getposition(Group::getObject(%set,%i));
			if(gamebase::getdataname(Group::getObject(%set,%i)) != SataliteUplinkDish && gamebase::getdataname(Group::getObject(%set,%i)) != Satalite){
			
			Staticshape::ondamage(Group::getobject(%set,%i), $plasmadamagetype, 1000, %pos);
			GameBase::applyRadiusDamage($PlasmaDamageType, %pos, 30, 1000, 30, %this.owner);
			}
		}
	}	
	deleteObject(%set);	
}

//============================================================================================OBTAINED FROM SHIFTER'S "Tatical Nuke" along with the individal 
//============================================================================================blasts used for it

function NuclearExplosion(%this)
{
	echo("NukeStrikeCalled");
	//%cl = $ProjectileOwner[%this];
	%player = %this.owner;
	%vel = "0 0 0";
	
	//if (!%player)
	//	return;
	
	%pos1 = gamebase::getposition(%this);
//	%rot = (gamebase::getrotation(%this)); //Causes bugs and is not needed! probably was supposed to work with a mine projectile, not a bullet proj.
//	%dir = (Vector::getfromrot(%rot));	
//	%trans1 = (%rot @ " " @ %dir @ " " @ %rot);
	
	if($NukeThroughPrefabs){
		%trans = "0 0 0 0 0 0 0 0 0 " @ %pos1;
		schedule ("NukeBoxKill(\"" @ %this @ "\", \"" @ %pos1 @ "\");",0.5);
	}
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos1;
	schedule ("Projectile::spawnProjectile(NukeMainBlast, \"" @ %trans @ "\", \"" @ %player @ "\", \"" @ %vel @ "\");",1.0);

	%padd = "0 0 2.0";
	%pos = Vector::add(%pos1, %padd);
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	schedule ("Projectile::spawnProjectile(NBase, \"" @ %trans @ "\", \"" @ %player @ "\", \"" @ %vel @ "\");",0.1);

	%padd = "0 0 3.0";
	%pos = Vector::add(%pos1, %padd);
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	schedule ("Projectile::spawnProjectile(NRing, \"" @ %trans @ "\", \"" @ %player @ "\", \"0 0 10\");",1.1);

	%padd = "0 0 4.0";
	%pos = Vector::add(%pos1, %padd);
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	schedule ("Projectile::spawnProjectile(NRing, \"" @ %trans @ "\", \"" @ %player @ "\", \"0 0 10\");",0.2);

	%padd = "0 0 8.0";
	%pos = Vector::add(%pos1, %padd);
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	schedule ("Projectile::spawnProjectile(NRing, \"" @ %trans @ "\", \"" @ %player @ "\", \"0 0 10\");",0.2);
	
	//schedule ("Projectile::spawnProjectile(NRing1, \"" @ %trans @ "\", \"" @ %player @ "\", \"0 0 0\");",0.2);
	%obj = newObject("","Mine","NRing1"); 
	addToSet("MissionCleanup", %obj);
	GameBase::throw(%obj,%player,0,false);
	gamebase::setposition(%obj, %pos);
	
	%padd = "0 0 10.0";
	%pos = Vector::add(%pos1, %padd);
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	schedule ("Projectile::spawnProjectile(NBlast, \"" @ %trans @ "\", \"" @ %player @ "\", \"" @ %vel @ "\");",0.3);
	
	//schedule ("Projectile::spawnProjectile(NRing1, \"" @ %trans @ "\", \"" @ %player @ "\", \"0 0 0\");",0.2);
	%obj = newObject("","Mine","NRing1"); 
	addToSet("MissionCleanup", %obj);
	GameBase::throw(%obj,%player,0,false);
	gamebase::setposition(%obj, %pos);
	
	%padd = "0 0 25.0";
	%pos = Vector::add(%pos1, %padd);
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	schedule ("Projectile::spawnProjectile(NBlast, \"" @ %trans @ "\", \"" @ %player @ "\", \"" @ %vel @ "\");",0.1);

	%padd = "0 0 35.0";
	%pos = Vector::add(%pos1, %padd);
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	schedule ("Projectile::spawnProjectile(NBlast, \"" @ %trans @ "\", \"" @ %player @ "\", \"" @ %vel @ "\");",0.1);

	%padd = "0 0 45.0";
	%pos = Vector::add(%pos1, %padd);
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	schedule ("Projectile::spawnProjectile(NBlast, \"" @ %trans @ "\", \"" @ %player @ "\", \"" @ %vel @ "\");",0.1);

	%padd = "15.0 0 60.0";
	%pos = Vector::add(%pos1, %padd);
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	schedule ("Projectile::spawnProjectile(NCloud, \"" @ %trans @ "\", \"" @ %player @ "\", \"" @ %vel @ "\");",0.1);

	%padd = "-15.0 0 60.0";
	%pos = Vector::add(%pos1, %padd);
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	schedule ("Projectile::spawnProjectile(NCloud, \"" @ %trans @ "\", \"" @ %player @ "\", \"" @ %vel @ "\");",0.1);

	%padd = "0 15.0 60.0";
	%pos = Vector::add(%pos1, %padd);
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	schedule ("Projectile::spawnProjectile(NCloud, \"" @ %trans @ "\", \"" @ %player @ "\", \"" @ %vel @ "\");",0.1);

	%padd = "0 -15.0 60.0";
	%pos = Vector::add(%pos1, %padd);
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	schedule ("Projectile::spawnProjectile(NCloud, \"" @ %trans @ "\", \"" @ %player @ "\", \"" @ %vel @ "\");",0.1);

	%padd = "0 0 75.0";
	%pos = Vector::add(%pos1, %padd);
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	schedule ("Projectile::spawnProjectile(NCloud, \"" @ %trans @ "\", \"" @ %player @ "\", \"" @ %vel @ "\");",0.1);

	%padd = "0 0 65";
	%pos = Vector::add(%pos1, %padd);
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	schedule ("Projectile::spawnProjectile(NRing, \"" @ %trans @ "\", \"" @ %player @ "\", \"" @ %vel @ "\");",0.2);
	
	//schedule ("Projectile::spawnProjectile(NRing1, \"" @ %trans @ "\", \"" @ %player @ "\", \"0 0 0\");",0.2);
	%obj = newObject("","Mine","NRing1"); 
	addToSet("MissionCleanup", %obj);
	GameBase::throw(%obj,%player,0,false);
	gamebase::setposition(%obj, %pos);
	
}

