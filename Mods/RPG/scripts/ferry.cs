//This file is part of Tribes RPG.
//Tribes RPG server side scripts
//Written by Jason "phantom" Daley,  Matthiew "JeremyIrons" Bouchard, and more (yet undetermined)

//	Copyright (C) 2014  Jason Daley

//	This program is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.

//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.

//	You should have received a copy of the GNU General Public License
//	along with this program.  If not, see <http://www.gnu.org/licenses/>.

//You may contact the author at beatme101@gmail.com or www.tribesrpg.org/contact.php

//This GPL does not apply to Starsiege: Tribes or any non-RPG related files included.
//Starsiege: Tribes, including the engine, retains a proprietary license forbidding resale.



$FerryfolderNameForSystem = "System";
$FerryfolderNameForPath = "Path";

$FerryStationWait = 0.05;

if($ferryObject == "")
	$ferryObject = "raft_b";

MoveableData PlatformFerry
{
	shapeFile = $ferryObject;
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
