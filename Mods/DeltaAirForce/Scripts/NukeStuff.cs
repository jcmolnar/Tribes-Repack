RocketData NukeDomeRocket
{
   bulletShapeName  = "ammopack.dts";
   explosionTag     = rocketExp;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.0;
   damageType       = $MissileDamageType;

   explosionRadius  = 9.5;
   kickBackStrength = 250.0;
   muzzleVelocity   = 100.0;
   terminalVelocity = 100.0;
   acceleration     = 5.0;
   totalTime        = 20.0;
   liveTime         = 0.0;
   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };
   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "rsmoke.dts";
   smokeDist   = 10.5;

   soundId = SoundJetHeavy;
};

ExplosionData NukeDomeExp
{
   shapeName = "mortarex.dts";
   soundId   = SoundLFootLSoft;

   faceCamera = true;
   randomSpin = false;
//   faceCamera = true;
//   randomSpin = true;
   hasLight   = true;
   lightRange = 8.0;

   timeScale = 1.05;

   timeZero = 0.0;
   timeOne  = 0.500;

   colors[0]  = { 0.0, 0.0, 0.0 };
   colors[1]  = { 1.0, 1.0, 1.0 };
   colors[2]  = { 1.0, 1.0, 1.0 };
   radFactors = { 0.0, 1.0, 1.0 };
};

GrenadeData NukeDome
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = NukeDomeExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.2;
   mass               = 2.0;
   elasticity         = 0.0;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.0;
   damageType         = $MortarDamageType;

   explosionRadius    = 35.0;
   kickBackStrength   = 250.0;
   maxLevelFlightDist = 325;
   totalTime          = 0.0;
   liveTime           = 0.0;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 0.5;
   smokeName = "smoke.dts";
};

GrenadeData NukeDomeInit
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = NukeDomeExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.2;
   mass               = 2.0;
   elasticity         = 0.0;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.0;
   damageType         = $MortarDamageType;

   explosionRadius    = 35.0;
   kickBackStrength   = 250.0;
   maxLevelFlightDist = 325;
   totalTime          = 20.0;
   liveTime           = 0.0;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 0.5;
   smokeName = "smoke.dts";
};

//function NukeDomeInit::onAdd(%this)
//{
//	echo("nukedomeinit::onadd called");
//	//first
//	SpawnDome(%this);
//}

function SpawnDome(%this)
{	
	%player = -1;
//	for(%count1 = 0; %count1 <= 10; %count1= %count1+5){
//		for(%count2 = 0; %count2 <= 10; %count2= %count2+5){
//			for(%count3 = 0; %count3 <= 10; %count3= %count3+5){
//				%pos = getword(gameBase::getPosition(%this), 0) + count1 @ " " @ getword(gameBase::getPosition(%this), 1) +count2 @ " " @ getword(gameBase::getPosition(%this) + count3, 2);
//				%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
//				schedule ("Projectile::spawnProjectile(NukeDome, \"" @ %trans @ "\", \"" @ %player @ "\", \"0 0 0\");",0.03);
//
//				%pos = getword(gameBase::getPosition(%this), 0) - count1 @ " " @ getword(gameBase::getPosition(%this), 1) +count2 @ " " @ getword(gameBase::getPosition(%this) + count3, 2);
//				%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
//				schedule ("Projectile::spawnProjectile(NukeDome, \"" @ %trans @ "\", \"" @ %player @ "\", \"0 0 0\");",0.03);
//
//				%pos = getword(gameBase::getPosition(%this), 0) + count1 @ " " @ getword(gameBase::getPosition(%this), 1)-count2 @ " " @ getword(gameBase::getPosition(%this) + count3, 2);
//				%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
//				schedule ("Projectile::spawnProjectile(NukeDome, \"" @ %trans @ "\", \"" @ %player @ "\", \"0 0 0\");",0.03);
//				
//				%pos = getword(gameBase::getPosition(%this), 0) + count1 @ " " @ getword(gameBase::getPosition(%this), 1) +count2 @ " " @ getword(gameBase::getPosition(%this) - count3, 2);
//				%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
//				schedule ("Projectile::spawnProjectile(NukeDome, \"" @ %trans @ "\", \"" @ %player @ "\", \"0 0 0\");",0.03);
//
//				%pos = getword(gameBase::getPosition(%this), 0) - count1 @ " " @ getword(gameBase::getPosition(%this), 1) -count2 @ " " @ getword(gameBase::getPosition(%this) + count3, 2);
//				%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
//				schedule ("Projectile::spawnProjectile(NukeDome, \"" @ %trans @ "\", \"" @ %player @ "\", \"0 0 0\");",0.03);
//
//				%pos = getword(gameBase::getPosition(%this), 0) + count1 @ " " @ getword(gameBase::getPosition(%this), 1) -count2 @ " " @ getword(gameBase::getPosition(%this) - count3, 2);
//				%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
//				schedule ("Projectile::spawnProjectile(NukeDome, \"" @ %trans @ "\", \"" @ %player @ "\", \"0 0 0\");",0.03);
//
//
//				%pos = getword(gameBase::getPosition(%this), 0) - count1 @ " " @ getword(gameBase::getPosition(%this), 1) +count2 @ " " @ getword(gameBase::getPosition(%this) - count3, 2);
//				%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
//				schedule ("Projectile::spawnProjectile(NukeDome, \"" @ %trans @ "\", \"" @ %player @ "\", \"0 0 0\");",0.03);
//
//
//				%pos = getword(gameBase::getPosition(%this), 0) - count1 @ " " @ getword(gameBase::getPosition(%this), 1) -count2 @ " " @ getword(gameBase::getPosition(%this) - count3, 2);
//				%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
//				schedule ("Projectile::spawnProjectile(NukeDome, \"" @ %trans @ "\", \"" @ %player @ "\", \"0 0 0\");",0.03);
//			}
//		}
//	
//	}
//	
//
	%pos = getword(gameBase::getPosition(%this), 0) + 5 @ " " @ getword(gameBase::getPosition(%this), 1) @ " " @ getword(gameBase::getPosition(%this), 2);
	%trans = "0 0 0 0 0 0 1.57 0 0 " @ %pos;
	schedule ("Projectile::spawnProjectile(NukeDomeRocket, \"" @ %trans @ "\", \"" @ %player @ "\", \"0 0 0\");",0.03);
	
	%pos = getword(gameBase::getPosition(%this), 0) - 5 @ " " @ getword(gameBase::getPosition(%this), 1) @ " " @ getword(gameBase::getPosition(%this), 2);
	%trans = "0 0 0 0 0 0 0 1.57 0 " @ %pos;
	schedule ("Projectile::spawnProjectile(NukeDomeRocket, \"" @ %trans @ "\", \"" @ %player @ "\", \"0 0 0\");",0.03);
	
	%pos = getword(gameBase::getPosition(%this), 0) @ " " @ getword(gameBase::getPosition(%this), 1) +5 @ " " @ getword(gameBase::getPosition(%this), 2);
	%trans = "0 0 0 0 0 0 0 0 1.57 " @ %pos;
	schedule ("Projectile::spawnProjectile(NukeDomeRocket, \"" @ %trans @ "\", \"" @ %player @ "\", \"0 0 0\");",0.03);

	%pos = getword(gameBase::getPosition(%this), 0) @ " " @ getword(gameBase::getPosition(%this), 1) -5 @ " " @ getword(gameBase::getPosition(%this), 2);
	%trans = "0 0 0 0 0 0 -1.57 0 0 " @ %pos;
	schedule ("Projectile::spawnProjectile(NukeDomeRocket, \"" @ %trans @ "\", \"" @ %player @ "\", \"0 0 0\");",0.03);

	%pos = getword(gameBase::getPosition(%this), 0) @ " " @ getword(gameBase::getPosition(%this), 1) @ " " @ getword(gameBase::getPosition(%this), 2) +5;
	%trans = "0 0 0 0 0 0 0 -1.57 0 " @ %pos;
	schedule ("Projectile::spawnProjectile(NukeDomeRocket, \"" @ %trans @ "\", \"" @ %player @ "\", \"0 0 0\");",0.03);

	%pos = getword(gameBase::getPosition(%this), 0) @ " " @ getword(gameBase::getPosition(%this), 1) @ " " @ getword(gameBase::getPosition(%this), 2) -5;
	%trans = "0 0 0 0 0 0 0 0 -1.57 " @ %pos;
	schedule ("Projectile::spawnProjectile(NukeDomeRocket, \"" @ %trans @ "\", \"" @ %player @ "\", \"0 0 0\");",0.03);

	

}

function rocketTest( %player, %weapontype )
{
	Projectile::spawnProjectile(%weapontype, "0 0 0 0 0 0 1.57 0 0 " @ gamebase::getposition(%player), -1, "0 0 40");	

}

function NukeDomeRocket::onAdd(%this)
{
	RocketSmoke(%this);
}

function RocketSmoke(%this) 
{
	Projectile::spawnProjectile(NukeDome, "0 0 0 0 0 0 0 0 0 " @ gamebase::getposition(%this), -1, "0 0 0");
	schoedule ("RocketSmoke(\"" @ %this @ "\");",0.5);
}

function NukeDome::onAdd(%this)
{
	echo("nukedome::onadd called");

	
}

function shellTest( %player, %weapontype )
{
	echo(%weapontype @ " strike called on " @ %player );
	Projectile::spawnProjectile(%weapontype, "0 0 0 0 0 0 0 0 0 " @ getword(gamebase::getposition(%player),0) @ getword(gamebase::getposition(%player),1) @ getword(gamebase::getposition(%player),2)+30, -1, "-20 0 0");
}
