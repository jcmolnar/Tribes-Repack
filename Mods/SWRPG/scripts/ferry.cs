$FerryfolderNameForSystem = "System";
$FerryfolderNameForPath = "Path";

$FerryStationWait = 0.05;

MoveableData PlatformFerry
{
	shapeFile = "raft_b";
	className = "Ferry";
	destroyable = false;
	maxDamage = 100;
	triggerRadius = 4;
	isPerspective = true;
	displace = true;

	explosionId = debrisExpLarge;
	debrisId = defaultDebrisLarge;

	sfxStart = NoSound;
	sfxStop = NoSound;
      sfxRun = SoundBoat;
	sfxBlocked = NoSound;

	speed = 25;
};

MoveableData PlatformFerry2
{
	shapeFile = "kl_longship";
	className = "Ferry";
	destroyable = false;
	maxDamage = 100;
	triggerRadius = 4;
	isPerspective = true;
	displace = true;

	explosionId = debrisExpLarge;
	debrisId = defaultDebrisLarge;

	sfxStart = NoSound;
	sfxStop = NoSound;
	sfxRun = SoundBoat;
	sfxBlocked = NoSound;

	speed = 25;
};

function Ferry::onNewPath(%this)
{
	NextWaypoint(%this);
}
function Ferry::onWaypoint(%this)
{
	NextWaypoint(%this);
}
function ResetFerry(%this)
{
	dbecho($dbechoMode, "ResetFerry(" @ %this @ ")");

	Moveable::setWaypoint(%this, 0);
}
function NextWaypoint(%this)
{
	dbecho($dbechoMode, "NextWaypoint(" @ %this @ ")");

	//This function makes %this go to the next waypoint on its stack.
	//Once the current waypoint is the last waypoint, switch the ferry to waypoint 0 and wait $FerryStationWait amount
	//of seconds before moving on to next waypoint.

	%which = round(Moveable::getPosition(%this));

	//echo("NextWaypoint: " @ %this);
	//echo("waypointCount = " @ Moveable::getWaypointCount(%this));
	//echo("%which: " @ %which);
	//echo("-------------------------------");

	if(%which < (Moveable::getWaypointCount(%this)-1))
	{
		schedule("Moveable::moveToWaypoint(" @ %this @ ", " @ %which+1 @ ");", 0.05+$Ferry::PauseTime[%this, %which]);
	}
	else if(%which == (Moveable::getWaypointCount(%this)-1))
	{
		ResetFerry(%this);
		schedule("NextWaypoint(" @ %this @ ");", $FerryStationWait, %this);
	}
}

function Ferry::onCollision(%this, %object)
{
	return;

	%clientId = Player::getClient(%object);

	for(%i = 1; $personalFerryArray[%clientId, %i] != ""; %i++){}
	$personalFerryArray[%clientId, %i] = "1 " @ %this;
	schedule("$personalFerryArray[" @ %clientId @ ", " @ %i @ "] = \"\";", 5);
}

function IsOnFerry(%clientId)
{
	dbecho($dbechoMode, "IsOnFerry(" @ %clientId @ ")");

	%wferry = "";

	for(%i = 1; %i <= 20; %i++)
	{
		%z = floor(GetWord($personalFerryArray[%clientId, %i], 0));
		if(%z == -1) %z = 0;

		%cnt += %z;
		if(GetWord($personalFerryArray[%clientId, %i], 1) != -1 && %wferry == "")
			%wferry = GetWord($personalFerryArray[%clientId, %i], 1);
	}

	if(%cnt > 3)
		return %wferry;
	else
		return -1;
}

function InitFerry()
{
	dbecho($dbechoMode, "InitFerry()");

	%group = nameToId("MissionGroup\\Ferry");

	if(%group != -1)
	{
		%count = Group::objectCount(%group);
		for(%i = 0; %i <= %count-1; %i++)
		{
			%object = Group::getObject(%group, %i);
			%system = Object::getName(%object);
			%wferry = String::getSubStr(%system, String::len($FerryfolderNameForSystem), String::len(%system)-String::len($FerryfolderNameForSystem));

			//find %ferry id
			%g = nameToId("MissionGroup\\Ferry\\" @ %system);
			%c = Group::objectCount(%g);
			for(%k = 0; %k <= %c-1; %k++)
			{
				%o = Group::getObject(%g, %k);
				if(getObjectType(%o) == "Moveable")
					%ferry = %o;
			}

			$Ferry::FolderName[%ferry] = "MissionGroup\\Ferry\\" @ %system;

			//go thru all the markers / droppoints and perhaps do something?
			%groupForPath = nameToId("MissionGroup\\Ferry\\" @ %system @ "\\" @ $FerryfolderNameForPath);
			%countForPath = Group::objectCount(%groupForPath);
			for(%j = 0; %j <= %countForPath-1; %j++)
			{
				%o1 = Group::getObject(%groupForPath, %j);
				$Ferry::MarkerPos[%ferry, %j] = GameBase::getPosition(%o1);
				$Ferry::PauseTime[%ferry, %j] = floor((Object::getName(%o1)) * 100) / 100;
			}
		}
	}
}
