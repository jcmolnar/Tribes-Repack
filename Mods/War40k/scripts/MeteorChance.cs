function MeteorChance()
{	
	%rnd = floor(getRandom() * 25);
	if (%rnd <= 5)
	{
		if (Client::getFirst() > 0) Meteor3();
		%time = 1800;
		schedule("MeteorChance();", %time);
	}
	else schedule("MeteorChance();", 600);
}

function Meteor1()
{
	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
	{
		client::sendMessage(%clientId,1,"Orbital debris cluster depleting");
	}
	%coordinate = waypointtoWorld("1024 0");
	%coordinate2 = waypointtoWorld("0 0");
	%coordinate=getword(%coordinate,0);
	%coordinate2=getword(%coordinate2,0);
	for(%i = 0; %i < 180; %i= %i++)
	{
		%time = %i * 60/180;
		%number=$sin[%i];
		%number=floor(%number*0.3+1);
		%test = schedule("MeteorStrike(" @ %number @ ");", %time);
	}
}

function Meteor2()
{
	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
	{
		client::sendMessage(%clientId,1,"Incoming heavy orbital debris!");
	}
	%coordinate = waypointtoWorld("1024 0");
	%coordinate2 = waypointtoWorld("0 0");
	%coordinate=getword(%coordinate,0);
	%coordinate2=getword(%coordinate2,0);
	schedule("Meteor1();", 120);
	for(%i = 0; %i < 180; %i= %i++)
	{
		%time = %i * 40/180;
		%number=$sin[%i];
		%number=floor(%number*0.9+1);
		%test = schedule("MeteorStrike(" @ %number @ ");", %time);
	}
}

function Meteor3()
{
	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
	{
		client::sendMessage(%clientId,1,"Orbital debris cluster entering atmosphere over your sector");
	}
	%coordinate = waypointtoWorld("1024 0");
	%coordinate2 = waypointtoWorld("0 0");
	%coordinate=getword(%coordinate,0);
	%coordinate2=getword(%coordinate2,0);
	schedule("Meteor2();", 120);
	for(%i = 0; %i < 180; %i= %i++)
	{
		%time = %i * 20/180;
		%number=$sin[%i];
		%number=floor(%number*0.3+1);
		%test = schedule("MeteorStrike(" @ %number @ ");", %time);
	}
}

function MeteorStrike(%number)
{
	for(%it = 0; %it < %number; %it++)
	{
		%clientId = Client::getFirst();
		%x = floor(getRandom() * 1024);
		%y = floor(getRandom() * 1024);
		%loc = %x @ " " @ %y;
		%loc = WaypointToWorld(%loc);
		%player = Client::getOwnedObject(%clientId);
		%player = "2048";
		%vel = "0 0 0";
		%vertical = "0 0 1000";
		%loc = vector::add(%loc,%vertical);
		%trans = "1.000000 0.000000 0.000000 0.000000 0.000345 -0.999999 0.000000 0.999999 0.000345";
		%trans = %trans @ " " @ %loc;
		%vel = Item::getVelocity(%player);
		Projectile::spawnProjectile("Meteor",%trans,%camera,%vel);
	}
}

ExplosionData orbDebrisExp
{
   shapeName = "fiery.dts";
   soundId   = bigExplosion3;

   faceCamera = true;
   randomSpin = true;
   hasLight   = true;
   lightRange = 33.33;

   timeScale = 1.5;

   timeZero = 0.150;
   timeOne  = 0.500;

   colors[0]  = { 0.0, 0.0,  0.0 };
   colors[1]  = { 1.0, 0.63, 0.0 };
   colors[2]  = { 1.0, 0.63, 0.0 };
   radFactors = { 0.0, 1.0, 0.9 };
};

RocketData Meteor
{
   bulletShapeName  = "breath.dts";
   explosionTag     = orbDebrisExp;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.8;
   damageType       = $DebrisDamageType;

   explosionRadius  = 13;
   kickBackStrength = 233;
   muzzleVelocity   = 100;
   terminalVelocity = 200;
   acceleration     = 100;
   totalTime        = 10;
   liveTime         = 11;
   lightRange       = 5.0;
   lightColor       = { 1, 0.2, 0.5 };
   inheritedVelocityScale = 0;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "plasmatrail.dts";
   smokeDist   = 65;

   soundId = SoundJetHeavy;
};