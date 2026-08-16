//This file is part of Tribes RPG.
//Tribes RPG server side scripts
//Written by Jason "phantom" Daley,  Matthiew "JeremyIrons" Bouchard, and more (yet undetermined)

//	Copyright (C) 2016  Jason Daley

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



function Game::pickObserverSpawn(%clientId)
{
	dbecho($dbechoMode2, "Game::pickObserverSpawn(" @ %clientId @ ")");

	%group = nameToID("MissionGroup\\ObserverDropPoints");
	%count = Group::objectCount(%group);

	if(%group == -1 || !%count)
		%group = nameToID("MissionGroup\\Teams\\team0\\DropPoints");
	%count = Group::objectCount(%group);
	if(%group == -1 || !%count)
		%group = nameToID("MissionGroup\\Teams\\team1\\DropPoints");
	%count = Group::objectCount(%group);
	if(%group == -1 || !%count)
		return -1;
	%spawnIdx = %clientId.lastObserverSpawn + 1;
	if(%spawnIdx >= %count)
		%spawnIdx = 0;
	%clientId.lastObserverSpawn = %spawnIdx;

	return Group::getObject(%group, %spawnIdx);
}

function Game::pickPlayerSpawn(%clientId, %respawn)
{
	dbecho($dbechoMode2, "Game::pickPlayerSpawn(" @ %clientId @ ", " @ %respawn @ ")");

	if(fetchData(%clientId, "lastzone") == "")
		%group = nameToID("MissionGroup/Teams/team0/DropPoints");
	else
		%group = nameToID("MissionGroup/Zones/" @ Object::getName(fetchData(%clientId, "lastzone")) @ "/DropPoints");

	%count = Group::objectCount(%group);
	if(!%count)
		return -1;
	%spawnIdx = floor(getRandom() * (%count - 0.1));
	%value = %count;

	for(%i = %spawnIdx; %i < %value; %i++)
	{
		%set = newObject("set",SimSet);
		%obj = Group::getObject(%group, %i);
		if(containerBoxFillSet(%set,$SimPlayerObjectType|$VehicleObjectType,GameBase::getPosition(%obj),2,2,4,0) == 0)
		{
			deleteObject(%set);
			return %obj;		
		}
		if(%i == %count - 1)
		{
			%i = -1;
			%value = %spawnIdx;
		}
		deleteObject(%set);
	}
	return -1;
}

function Game::playerSpawn(%clientId, %respawn)
{
	dbecho($dbechoMode2, "Game::playerSpawn(" @ %clientId @ ", " @ %respawn @ ")");

	if(!$ghosting)
		return false;

	Client::clearItemShopping(%clientId);
	Client::clearItemBuying(%clientId);

	if(fetchData(%clientId, "isMimic"))
	{
		storeData(%clientId, "RACE", Client::getGender(%clientId) @ "Human");
		storeData(%clientId, "isMimic", "");
	}

	if(%clientId.RespawnMeInArena)
	{
		%group = nameToID("MissionGroup\\TheArena\\TeleportEntranceMarkers");

		if(%group != -1)
		{
			%num = Group::objectCount(%group);

			%r = floor(getRandom() * %num);
			%spawnMarker = Group::getObject(%group, %r);
		}
		else
		{
			%spawnMarker = Game::pickPlayerSpawn(%clientId, %respawn);
		}

		RefreshArenaTextBox(%clientId);
	}
	else
	{
		%spawnMarker = Game::pickPlayerSpawn(%clientId, %respawn);

		//the player is spawning normally, ie. not in the arena
		storeData(%clientId, "inArena", "");
		CloseArenaTextBox(%clientId);
	}

	//if(%spawnMarker)
	//{
		%clientId.guiLock = "";
		%clientId.dead = "";
		if(fetchData(%clientId, "campPos") != "" && !%respawn)
		{
			//if the player HAS a campPos and it is his FIRST TIME SPAWNING, then spawn him at this campPos
			%spawnPos = fetchData(%clientId, "campPos");
			%spawnRot = fetchData(%clientId, "campRot");
		}
		else if(%spawnMarker == -1)
		{
			%spawnPos = "-2428.75 -267.75 77.5942";
			%spawnRot = "0 0 0";
		}
		else
		{
			%spawnPos = GameBase::getPosition(%spawnMarker);
			%spawnRot = GameBase::getRotation(%spawnMarker);
		}

		%armor = $RaceToArmorType[fetchData(%clientId, "RACE")];		if(%armor.mass == False)			%armor = "MaleHumanArmor0";

		%pl = spawnPlayer(%armor, %spawnPos, %spawnRot);
		PlaySound(SoundSpawn2, %spawnPos);
		GameBase::startFadeIn(Client::getOwnedObject(%clientId));

		echo("SPAWN: cl:" @ %clientId @ " pl:" @ %pl @ " marker:" @ %spawnMarker @ " position: " @ %spawnPos @ " armor:" @ %armor);

		if(%pl != -1)
		{
			UpdateTeam(%clientId);
			GameBase::setTeam(%pl, Client::getTeam(%clientId));
			Client::setOwnedObject(%clientId, %pl);
			Client::setControlObject(%clientId, %pl);
			Game::playerSpawned(%pl, %clientId, %armor, %respawn);
			%spawnhp = 1;			%spawnmana = 1;
			if(%respawn)	      
			{				%spawnhp = fetchData(%clientId, "MaxHP");				%spawnmana = fetchData(%clientId, "MaxMANA");
			}
			else
			{
				%spawnhp = fetchData(%clientId, "tmphp");				%spawnmana = fetchData(%clientId, "tmpmana");
				storeData(%clientId, "tmphp", "");
				storeData(%clientId, "tmpmana", "");
			}			if(%spawnhp < 1){ %spawnhp = 1; echo("Error: spawn hp was less than 1"); }			if(%spawnmana < 1){ %spawnmana = 1; echo("Error: spawn mp was less than 1"); }			setHP(%clientId, %spawnhp);			setMANA(%clientId, %spawnmana);
			storeData(%clientId.possessId, "dumbAIflag", "");
			storeData(%clientId, "isDead", False);
		}
		schedule("afterspawnstuff("@%clientId@");",0.1);
		return true;
	//}
	//else
	//{
	//	Client::sendMessage(%clientId,0,"Sorry No Respawn Positions Are Empty - Try again later ");
	//	return false;
	//}
}
function afterspawnstuff(%clientId){	if(isDead(%clientId))		return;
	if(%clientId.hasSpawned)		return;	%clientId.hasSpawned = True;
	%name = rpg::getname(%clientId);	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))	{		if(%cl.repack >= 24){			Client::sendMessage(%cl, $MsgWhite, %name@" has entered the world.");		}	}
	schedule("repackAlert("@%clientId@");",1.0);	if(%clientId.repack > 29)		initEscText(%clientId);
}

function Game::playerSpawned(%pl, %clientId, %armor)
{
	dbecho($dbechoMode2, "Game::playerSpawned(" @ %pl @ ", " @ %clientId @ ", " @ %armor @ ")");

	storeData(%clientId, "HasLoadedAndSpawned", True);

	if(%clientId.RespawnMeInArena)
	{
		//give him his equipment back
		RestorePreviousEquipment(%clientId);

		%clientId.RespawnMeInArena = "";
	}
	else
	{
		GiveThisStuff(%clientId, fetchData(%clientId, "spawnStuff"), False);
	}

	if(fetchData(%clientId, "LCK") < 0)
		storeData(%clientId, "LCK", 0);
	player::setitemcount(%clientId,BeltItemTool, 1);

	RefreshAll(%clientId);
} 

function Game::autoRespawn(%clientId)
{
	dbecho($dbechoMode2, "Game::autoRespawn(" @ %clientId @ ")");

	if(%clientId.dead == 1)
		Game::playerSpawn(%clientId, True);
}