$SensorNetworkEnabled = true;

$GuiModePlay = 1;
$GuiModeCommand = 2;
$GuiModeVictory = 3;
$GuiModeInventory = 4;
$GuiModeObjectives = 5;
$GuiModeLobby = 6;


//  Global Variables

//---------------------------------------------------------------------------------
// Energy each team is given at beginning of game
//---------------------------------------------------------------------------------
$DefaultTeamEnergy = "Infinite";

//---------------------------------------------------------------------------------
// Team Energy variables
//---------------------------------------------------------------------------------
$TeamEnergy[-1] = $DefaultTeamEnergy;
$TeamEnergy[0]  = $DefaultTeamEnergy;
$TeamEnergy[1]  = $DefaultTeamEnergy;
$TeamEnergy[2]  = $DefaultTeamEnergy;
$TeamEnergy[3]  = $DefaultTeamEnergy;
$TeamEnergy[4]  = $DefaultTeamEnergy;
$TeamEnergy[5]  = $DefaultTeamEnergy;
$TeamEnergy[6]  = $DefaultTeamEnergy;
$TeamEnergy[7]  = $DefaultTeamEnergy;

//---------------------------------------------------------------------------------
// If 1 then Team Spending Ignored -- Team Energy is set to $MaxTeamEnergy every
// 	$secTeamEnergy.
//---------------------------------------------------------------------------------
$TeamEnergyCheat = 0;

//---------------------------------------------------------------------------------
// MAX amount team energy can reach
//---------------------------------------------------------------------------------
$MaxTeamEnergy = 700000;

//---------------------------------------------------------------------------------
// Amount to inc team energy every ($secTeamEnergy) seconds
//---------------------------------------------------------------------------------
$incTeamEnergy = 0;

//---------------------------------------------------------------------------------
// (Rate is sec's) Set how often TeamEnergy is incremented
//---------------------------------------------------------------------------------
$secTeamEnergy = 30;

//---------------------------------------------------------------------------------
// (Rate is sec's) Items respwan
//---------------------------------------------------------------------------------
$ItemRespawnTime = 30;

//---------------------------------------------------------------------------------
//Amount of Energy remote stations start out with
//---------------------------------------------------------------------------------
$RemoteAmmoEnergy = 100000000;
$RemoteInvEnergy = 100000000;

//---------------------------------------------------------------------------------
// TEAM ENERGY -  Warn team when teammate has spent x amount - Warn team that
//				  energy level is low when it reaches x amount
//---------------------------------------------------------------------------------
$TeammateSpending = 0; 		 //Set = to 0 if don't want the warning message
$WarnEnergyLow = 0;		 //Set = to 0 if don't want the warning message

//---------------------------------------------------------------------------------
// Amount added to TeamEnergy when a player joins a team
//---------------------------------------------------------------------------------
$InitialPlayerEnergy = "Infinite";

//---------------------------------------------------------------------------------
// REMOTE TURRET
//---------------------------------------------------------------------------------
$MaxNumTurretsInBox = 20;    	//Number of remote turrets allowed in the area
$TurretBoxMaxLength = 50;    	//Define Max Length of the area
$TurretBoxMaxWidth =  50;    	//Define Max Width of the area
$TurretBoxMaxHeight = 25;    	//Define Max Height of the area

$TurretBoxMinLength = 2;	//Define Min Length from another turret
$TurretBoxMinWidth =  2;	//Define Min Width from another turret
$TurretBoxMinHeight = 2;    	//Define Min Height from another turret

//---------------------------------------------------------------------------------
//	Object Types
//---------------------------------------------------------------------------------
$SimTerrainObjectType    = 1 << 1;
$SimInteriorObjectType   = 1 << 2;
$SimPlayerObjectType     = 1 << 7;

$MineObjectType		    = 1 << 26;
$MoveableObjectType	    = 1 << 22;
$VehicleObjectType	 	 = 1 << 29;
$StaticObjectType			 = 1 << 23;
$ItemObjectType			 = 1 << 21;

//---------------------------------------------------------------------------------
// CHEATS
//---------------------------------------------------------------------------------
$ServerCheats = 0;
$TestCheats = 0;

//---------------------------------------------------------------------------------
//Respawn automatically after X sec's -  If 0..no respawn
//---------------------------------------------------------------------------------
$AutoRespawn = 0;

//---------------------------------------------------------------------------------

function Time::getMinutes(%simTime) {
	dbecho($dbechoMode, "Time::getMinutes(" @ %simTime @ ")");

	return floor(%simTime / 60);
}

function Time::getSeconds(%simTime) {
	dbecho($dbechoMode, "Time::getSeconds(" @ %simTime @ ")");

	return %simTime % 60;
}


function UpdateClientTimes(%time) {
	dbecho($dbechoMode, "UpdateClientTimes(" @ %time @ ")");

	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		remoteEval(%cl, "setTime", -%time);
}

function Game::notifyMatchStart(%time) {
	dbecho($dbechoMode, "Game::notifyMatchStart(" @ %time @ ")");

	messageAll(0, "Match starts in " @ %time @ " seconds.");
	UpdateClientTimes(%time);
}


//newc//
function RandomEvent() {
	//%totalweight = 0;
	//if ($Meteor=="True") %totalweight = $MeteorWeight;
	//if ($Lightning=="True") %totalweight = %totalweight + $LightningWeight;
	//if ($SolarFlare=="True") %totalweight = %totalweight + $SolarFlareWeight;
	//%lightweight = %totalweight - $SolarFlareWeight;
	%totalweight = $MeteorWeight + $LightningWeight + $SolarFlareWeight;
	%lightweight = $MeteorWeight + $LightningWeight;
	%rnd = floor(getRandom() * %totalweight);
	if (%rnd <= $MeteorWeight)
	{
		if ($Meteor=="True")
		{
			if (Client::getFirst() > 0) Meteor();
			echo("Meteors");
			if ($RandomTimeVariance ==0) %variance = 0;
			else %variance = (getRandom() * $RandomTimeVariance)-$RandomTimeVariance/2;
			%time = $RandomEventTime+($RandomEventTime * %variance) + $MeteorDuration;
			echo ("Meteors " @ %variance @ " " @ %time);
			schedule("RandomEvent();", %time);
		}
		else schedule("RandomEvent();", 0.2);
	}
	else if (%rnd <= %lightweight)
	{
		if ($Lightning=="True")
		{
			if (Client::getFirst() > 0) Lightning();
			echo("Lightning");
			if ($RandomTimeVariance ==0) %variance = 0;
			else %variance = (getRandom() * $RandomTimeVariance)-$RandomTimeVariance/2;
			%time = $RandomEventTime+($RandomEventTime * %variance) + $LightningDuration;
			schedule("RandomEvent();", %time);
		}
		else schedule("RandomEvent();", 0.2);
	}
	else if (%rnd <= (%totalweight + 1))
	{
		if ($SolarFlare=="True")
		{
			if (Client::getFirst() > 0) SolarFlare();
			echo("SolarFlare");
			if ($RandomTimeVariance ==0) %variance = 0;
			else %variance = (getRandom() * $RandomTimeVariance)-$RandomTimeVariance/2;
			%time = $RandomEventTime+($RandomEventTime * %variance) + $SolarFlareDuration;
			schedule("RandomEvent();", %time);
		}
		else schedule("RandomEvent();", 0.2);

	}
}

function Meteor()
{
	%coordinate = waypointtoWorld("1024 512");
	%sunspot = vector::add(%coordinate,"0 0 300");
	echo("sunspot: " @ %sunspot);
	%camera = newObject("Camera","Turret",cameraturret,true);
	GameBase::setPosition(%camera,%sunspot);
	GameBase::setTeam(%camera,-1);
	addToSet("MissionCleanup", %camera);
	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
	{
		client::sendMessage(%clientId,2,"Orbital debris entering atmosphere over your sector.");
		Client::sendMessage(%clientId,0,"~wC_BuySell.wav");
		schedule ("Client::sendMessage(" @ %clientId @ ",0,\"~wC_BuySell.wav\");",0.5);
		schedule ("Client::sendMessage(" @ %clientId @ ",0,\"~wC_BuySell.wav\");",1.0);
	}
	%coordinate = waypointtoWorld("1024 0");
	%coordinate2 = waypointtoWorld("0 0");
	%coordinate=getword(%coordinate,0);
	%coordinate2=getword(%coordinate2,0);
	%density = %coordinate - %coordinate2;
	%density=%density/850;
	%density=pow(%density,2)-1;
	for(%i = 0; %i < 180; %i++)
	{
		%time = %i * $MeteorDuration/180;
		%number=$sin[%i];
		%number=floor(%number*$MeteorDensity+(%density*$MeteorAreaVariance))+1;
		%test = schedule("MeteorStrike(" @ %number @ " ," @ %camera @ ");", %time);
	}
	schedule ("DeleteObject(" @ %camera @ ");",$MeteorDuration+1);
}

function MeteorStrike(%number,%camera)
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
		%vertical = "0 0 200";
		%loc = vector::add(%loc,%vertical);
		%trans = "1.000000 0.000000 0.000000 0.000000 0.000345 -0.999999 0.000000 0.999999 0.000345";
		//%trans = "0.707 0 0.707 0 1 0 -0.707 0 0.707";
		%trans = %trans @ " " @ %loc;
		%vel = Item::getVelocity(%player);
		Projectile::spawnProjectile("Meteor",%trans,%camera,%vel);
	}
}

function Lightning()
{
	%coordinate = waypointtoWorld("0 0");
	%sunspot = vector::add(%coordinate,"0 0 300");
	echo("sunspot: " @ %sunspot);
	%camera = newObject("Camera","Turret",cameraturret,true);
	GameBase::setPosition(%camera,%sunspot);
	GameBase::setTeam(%camera,-1);
	addToSet("MissionCleanup", %camera);
	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
	{
		client::sendMessage(%clientId,2,"Electrical storm approaching sector.");
		Client::sendMessage(%clientId,0,"~wC_BuySell.wav");
		schedule ("Client::sendMessage(" @ %clientId @ ",0,\"~wC_BuySell.wav\");",0.5);
		schedule ("Client::sendMessage(" @ %clientId @ ",0,\"~wC_BuySell.wav\");",1.0);
	}
	%coordinate = waypointtoWorld("1024 0");
	%coordinate2 = waypointtoWorld("0 0");
	%coordinate=getword(%coordinate,0);
	%coordinate2=getword(%coordinate2,0);
	%density = %coordinate - %coordinate2;
	%density=%density/850;
	%density=pow(%density,2)-1;
	for(%i = 0; %i < 180; %i++)
	{
		%time = %i * $LightningDuration/180;
		%number=$sin[%i];
		%number=floor(%number*$LightningDensity+(%density*$LightingAreaVariance))+1;
		%test = schedule("LightningStrike(" @ %number @ " ," @ %camera @ ");", %time);
	}
	schedule ("DeleteObject(" @ %camera @ ");",$LightningDuration+1);
}

function LightningStrike(%number,%camera)
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
		%vertical = "0 0 200";
		%loc = vector::add(%loc,%vertical);
		%trans = "1.000000 0.000000 0.000000 0.000000 0.000345 -0.999999 0.000000 0.999999 0.000345";
		//%trans = "0.707 0 0.707 0 1 0 -0.707 0 0.707";
		%trans = %trans @ " " @ %loc;
		//%vel = Item::getVelocity(%player);
		Projectile::spawnProjectile("LightningBlast",%trans,%camera,"0 0 -900");
		Projectile::spawnProjectile("LightningBlast2",%trans,%camera,"0 0 -900");
		Projectile::spawnProjectile("LightningBeam",%trans,%camera,%vel);
	}
}

function SolarFlare()
{
	%coordinate = waypointtoWorld("1024 512");
	echo("coordinate: " @ %coordinate);
	%sunspot = vector::add(%coordinate,"0 0 300");
	echo("sunspot: " @ %sunspot);
	%camera = newObject("Camera","Turret",cameraturret,true);
	addToSet("MissionCleanup", %camera);
	//%damagetype = 1;
	//%damageradius = 3000;
	//%damagevalue = 1.0;
	//%force = 0.0;
	GameBase::setPosition(%camera,%sunspot);
	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
	{
		client::sendMessage(%clientId,2,"Solar radiation burst detected.");
		Client::sendMessage(%clientId,0,"~wC_BuySell.wav");
		schedule ("Client::sendMessage(" @ %clientId @ ",0,\"~wC_BuySell.wav\");",0.5);
		schedule ("Client::sendMessage(" @ %clientId @ ",0,\"~wC_BuySell.wav\");",1.0);
	}
	for(%i = 0; %i < 60; %i++)
	{
		%time = %i * $SolarFlareDuration/60;
		%test = schedule("SolarFlareBurst(" @ %camera @ ");", %time);
	}
	schedule ("DeleteObject(" @ %camera @ ");",$SolarFlareDuration+1);
}

function SolarFlareBurst(%obj)
{
	%pos = GameBase::getPosition(%obj);
	GameBase::ApplyRadiusDamage(26,%pos,3000,0.00201,0.001,%obj);
}
//newc//

function onServerGhostAlwaysDone()
{
}

function GameBase::getHeatFactor(%this)
{
	return 0.0;
}
