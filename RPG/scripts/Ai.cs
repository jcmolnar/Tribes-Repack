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


//
// AI support functions.
//

//
// This function creates an AI player using the supplied group of markers 
//    for locations.  The first marker in the group gives the starting location 
//    of the the AI, and the remaining markers specify the path to follow.  
//
// Example call:  
// 
//    createAI( guardNumberOne, "MissionGroup\\Teams\\team0\\guardPath", larmor );
//


//globals
//--------
// path type
// 0 = circular
// 1 = oneWay
// 2 = twoWay
$AI::defaultPathType = 2; //run twoWay paths

//armor types
//light = larmor
//medium = marmor
//heavy = harmor

$AIattackMode = 1;

//---------------------------------
//createAI()
//---------------------------------
function createAI(%aiName, %markerGroup, %name)
{
	dbecho($dbechoMode, "createAI(" @ %aiName @ ", " @ %markerGroup @ ", " @ %name @ ")");

	%group = nameToID( %markerGroup );
   
	if( %group == -1 || Group::objectCount(%group) == 0 )
	{
	      %spawnPos = %markerGroup;
	      %spawnRot = "0 0 0";
	}
	else
	{
		for(%i = 0; %i < Group::objectCount(%group); %i++)
		{
			%obj = Group::getObject(%group, %i);
			if(getObjectType(%obj) != "SimGroup")
				break;
		}
	      %spawnMarker = Group::getObject(%group, %i);
	      %spawnPos = GameBase::getPosition(%spawnMarker);
	      %spawnRot = GameBase::getRotation(%spawnMarker);
	}

	%guardtype = clipTrailingNumbers(%aiName);

	if($BotInfo[%aiName, RACE] != "")
		%armor = $RaceToArmorType[$BotInfo[%aiName, RACE]];		//bots in map will get this call
	else
		%armor = $RaceToArmorType[$NameForRace[%guardtype]];		//spawn bots will get this call

	if( AI::spawn( %aiName, %armor, %spawnPos, %spawnRot, %name, "male2" ) != "false" )
	{
		%AiId = AI::getId(%aiName);
		ClearVariables(%AiId);

		storeData(%AiId, "BotInfoAiName", %aiName);

		storeData(%AiId, "RACE", $ArmorTypeToRace[%armor]);

		storeData(%AiId, "LCKconsequence", "miss");
		storeData(%AiId, "RemortStep", 0);
		storeData(%AiId, "HasLoadedAndSpawned", True);
		storeData(%AiId, "botAttackMode", 1);
		storeData(%AiId, "tmpbotdata", "");

		storeData(%AiId, "HP", fetchData(%AiId, "MaxHP"));
		storeData(%AiId, "MANA", 1000);

		refreshHPREGEN(%AiId);
		refreshMANAREGEN(%AiId);

		storeData(%AiId, "LCK", $BotInfo[%aiName, LCK]);

		if(%group != -1)
		{
			// The order number is used for sorting waypoints, and other directives.  
			%orderNumber = 200;
         
			for(%i = 0; %i < Group::objectCount(%group); %i++)
			{
				%spawnMarker = Group::getObject(%group, %i);
				if(getObjectType(%spawnMarker) != "SimGroup")
				{
					%spawnPos = GameBase::getPosition(%spawnMarker);
           
					AI::DirectiveWaypoint( %aiName, %spawnPos, %orderNumber );
           
					%orderNumber++;
				}
			}

			AI::setAutomaticTargets(%aiName);
		}

		GameBase::startFadeIn(%AiId);
		PlaySound(SoundSpawn2, %spawnPos);
	}
	else
	{
      	dbecho(1, "Failure spawning bot:");
		dbecho(1, "%aiName: " @ %aiName);
		dbecho(1, "%armor: " @ %armor);
		dbecho(1, "%guardtype: " @ %guardtype);
		dbecho(1, "%spawnPos: " @ %spawnPos);
		dbecho(1, "%spawnRot: " @ %spawnRot);
		dbecho(1, "%name: " @ %name);
		return -1;
      }
}

//----------------------------------
// AI::setupAI()
//
// Called from Mission::init() which is defined in Objectives.cs (or Dm.cs for
//    deathmatch missions).  
//----------------------------------   
function AI::setupAI(%key, %team)
{
	dbecho($dbechoMode, "AI::setupAI(" @ %key @ ", " @ %team @ ")");

	//if there is no key then they don't exist yet
	if(%key == "")
	{
		%aiFound = 0;
		for( %T = 0; %T < 8; %T++ )
		{
			%groupId = nameToID("MissionGroup\\Teams\\team" @ %T @ "\\AI" );
			if( %groupId != -1 )
			{
				%teamItemCount = Group::objectCount(%groupId);
				if( %teamItemCount > 0 )
				{
					AI::initDrones(%T, %teamItemCount);
					%aiFound += %teamItemCount;
				}
			}
		}
		if( %aiFound == 0 )
			dbecho(1, "No drones exist...");
		else
			dbecho(1, %aiFound @ " drones installed..." );

		$numAi = %aiFound;
	}
	else     //respawning dead AI with original name and path
	{
		%group = nameToID("MissionGroup\\Teams\\team" @ %team @ "\\AI\\" @ %key);
		%num = Group::objectCount(%group);
		createAI(%key, %group, $BotInfo[%key, NAME]);
		%aiId = AI::getId(%key);

		GameBase::setTeam(%aiId, %team);
		AI::setVar(%key, pathType, $AI::defaultPathType);
		AI::setWeapons(%key);

		//**RPG (added because AI::onDroneKilled doesn't conserve the AI's team)
		storeData(%AiId, "botTeam", %team);
		//**
	}		
}

//------------------------------
// AI::setWeapons()
//------------------------------
function AI::setWeapons(%aiName, %loadout)
{
	dbecho($dbechoMode, "AI::setWeapons(" @ %aiName @ ")");

	%aiId = AI::getId(%aiName);

	if(%loadout == -1 || %loadout == "" || String::ICompare(%loadout, "default") == 0)
	{
		%items = $BotInfo[%aiName, ITEMS];
		if(%items == "")
			GiveThisStuff(%aiId, $BotEquipment[clipTrailingNumbers(%aiName)], False);
		else
			GiveThisStuff(%aiId, %items, False);
	}
	else
		GiveThisStuff(%aiId, $LoadOut[%loadout], False);

	HardcodeAIskills(%aiId);

	Game::refreshClientScore(%aiId);
  
	AI::SetVar(%aiName, triggerPct, 1.0 );
	AI::setVar(%aiName, iq, 100 );
	AI::setVar(%aiName, attackMode, $AIattackMode);
	AI::setAutomaticTargets( %aiName );

	ai::callbackPeriodic(%aiName, 5, AI::Periodic);

	AI::SelectBestWeapon(%aiId);	//this way the bot spawns and has a weapon in hand
}


function AI::Periodic(%aiName)
{
	dbecho($dbechoMode, "AI::Periodic(" @ %aiName @ ")");

	%aiId = AI::getId(%aiName);

	if(fetchData(%aiId, "dumbAIflag") || fetchData(%aiId, "frozen"))
		return;

	%aiTeam = GameBase::getTeam(%aiId);
	%aiPos = GameBase::getPosition(%aiId);

	//=================================================================
	//Everytime this function is called, the bot looks at all clients
	//(including bots) and sets a waypoint to the nearest one that is
	//NOT on the same team, and that it can see in its FOV (by
	//comparing Rotations).  These loops will undoubtedly cause alot
	//of CPU drain.
	//=================================================================

	if(fetchData(%aiId, "SpawnBotInfo") != "")
	{
		if($AIsmartFOVbots)
		{
			//find all clients that COULD be in the bot's FOV and that are not on the same team.
			%aiTeam = GameBase::getTeam(%aiId);
			%aiPos = GameBase::getPosition(%aiId);
			%aiRot = GetWord(GameBase::getRotation(%aiId), 2);

			%list = GetEveryoneIdList();
			for(%i = 0; GetWord(%list, %i) != -1; %i++)
			{
				%id = GetWord(%list, %i);
				if(GameBase::getTeam(%id) != %aiTeam && !fetchData(%id, "invisible"))
				{
					%vec = Vector::sub(GameBase::getPosition(%id), %aiPos);
					%vecRot = GetWord(Vector::getRotation(%vec), 2);
		
					if(%vecRot >= %aiRot - $AIFOVPan && %vecRot <= %aiRot + $AIFOVPan)
					{
						%idList[%c++] = %id;
					}
				}
			}
		}

		if(%idList[1] != "" && $AIsmartFOVbots)
		{
			%closest = 500000;
			for(%i = 1; %idList[%i] != ""; %i++)
			{
				//echo(%aiId @ ": AI spotted " @ %idList[%i]);
				%dist = Vector::getDistance(%aiPos, GameBase::getPosition(%idList[%i]));
				if(%dist < %closest)
				{
					%closest = %dist;
					%closestId = %idList[%i];
				}
			}
	
			if(%closest <= $AImaxRange)
			{
				//echo(%aiId @ ": targeting (moving towards) " @ %closestId @ ", " @ %closest @ " meters away");
				if(%closest <= 10)
					AI::newDirectiveWaypoint(%aiName, GameBase::getPosition(%closestId), 99);
				else
					AI::newDirectiveFollow(%aiName, %closestId, 0, 99);
			}
		}
		else
		{
			//==============================================================
			// I'm making it so bots can "smell" their target. Basically,
			// if you're close enough to them, they will lock onto you.
			// I doubt this loop will cause much lag... *fingers crossed*
			//==============================================================

			if(fetchData(%aiId, "SpellCastStep") != 1 && !fetchData(%aiId, "noBotSniff"))
			{
				%closest = 500000;

				%flag = "";
				%b = $AImaxRange * 2;
				%set = newObject("set", SimSet);
				%n = containerBoxFillSet(%set, $SimPlayerObjectType, %aiPos, %b, %b, %b, 0);
				for(%i = 0; %i < Group::objectCount(%set); %i++)
				{
					%id = Player::getClient(Group::getObject(%set, %i));
					if(GameBase::getTeam(%id) != %aiTeam && !fetchData(%id, "invisible"))
					{
						%dist = Vector::getDistance(%aiPos, GameBase::getPosition(%id));
						if(%dist < %closest)
						{
							%closest = %dist;
							%closestId = %id;
						}
					}
				}
				deleteObject(%set);

				if(%closest <= $AImaxRange)
				{
					if(%closest <= 10)
						AI::newDirectiveWaypoint(%aiName, GameBase::getPosition(%closestId), 99);
					else
						AI::newDirectiveFollow(%aiName, %closestId, 0, 99);

					PlaySound(RandomRaceSound(fetchData(%aiId, "RACE"), Acquired), GameBase::getPosition(%aiId));
				}
				else
					%flag = True;
			}
			
			if(%flag || fetchData(%aiId, "noBotSniff"))
			{
				AI::SelectMovement(%aiName);
			}
		}
	}

	//=================================================================
	// Event stuff
	//=================================================================
	%i = GetEventCommandIndex(%aiId, "onPosCloseEnough");
	if(%i != -1)
	{
		%x = GetWord($EventCommand[%aiId, %i], 2);
		%y = GetWord($EventCommand[%aiId, %i], 3);
		%z = GetWord($EventCommand[%aiId, %i], 4);
		%dpos = %x @ " " @ %y @ " " @ %z;

		if(Vector::getDistance(%dpos, GameBase::getPosition(%aiId)) <= 5)
		{
			%name = GetWord($EventCommand[%aiId, %i], 0);
			%type = GetWord($EventCommand[%aiId, %i], 1);
			%cl = NEWgetClientByName(%name);
			if(%cl == -1)
				%cl = 2048;

			%cmd = String::NEWgetSubStr($EventCommand[%aiId, %i], String::findSubStr($EventCommand[%aiId, %i], ">")+1, 99999);
			%pcmd = ParseBlockData(%cmd, %aiId, "");
			$EventCommand[%aiId, %i] = "";
			schedule("internalSay(" @ %cl @ ", 0, \"" @ %pcmd @ "\", \"" @ %name @ "\");", 1);
		}
	}
	%i = GetEventCommandIndex(%aiId, "onIdCloseEnough");
	if(%i != -1)
	{
		%id = GetWord($EventCommand[%aiId, %i], 2);
		%dpos = GameBase::getPosition(%id);

		if(Vector::getDistance(%dpos, %aiPos) <= 10)
		{
			%name = GetWord($EventCommand[%aiId, %i], 0);
			%type = GetWord($EventCommand[%aiId, %i], 1);
			%cl = NEWgetClientByName(%name);
			if(%cl == -1)
				%cl = 2048;

			%cmd = String::NEWgetSubStr($EventCommand[%aiId, %i], String::findSubStr($EventCommand[%aiId, %i], ">")+1, 99999);
			%pcmd = ParseBlockData(%cmd, %aiId, "");
			$EventCommand[%aiId, %i] = "";
			schedule("internalSay(" @ %cl @ ", 0, \"" @ %pcmd @ "\", \"" @ %name @ "\");", 1);
		}
	}

	//=================================================================
	//1 thru 4 = animation 10, 5 thru 10 = animation 11, else do nothing
	//=================================================================
	if(Item::getVelocity(%aiId) == "0 0 0")
	{
		%r = floor(getRandom() * 200)+1;
		if(%r >= 1 && %r <= 4)
			RemotePlayAnim(%aiId, 10);
		else if(%r >= 5 && %r <= 10)
			RemotePlayAnim(%aiId, 11);

		if(GameBase::getTeam(%aiId) > 1)
		{
			if(OddsAre(5))
				RemotePlayAnim(%aiId, 2);
		}
	}
	
	//=============================================
	//do other stuff...
	//=============================================
	//%curTarget = ai::getTarget( %aiName );

	if(OddsAre(4))
		AI::SelectBestWeapon(%aiId);
}
function AI::NextWeapon(%aiId)
{
	dbecho($dbechoMode, "AI::NextWeapon(" @ %aiId @ ")");

	%item = Player::getMountedItem(%aiId, $WeaponSlot);

	if(%item == -1 || $NextWeapon[%item] == "")
		selectValidWeapon(%aiId);
	else
	{
		for(%weapon = $NextWeapon[%item]; %weapon != %item; %weapon = $NextWeapon[%weapon])
		{
			if(isSelectableWeapon(%clientId, %weapon))
			{
				%x = "";
				if(GetAccessoryVar(%weapon, $AccessoryType) == $RangedAccessoryType)
				{
					%x = GetBestRangedProj(%aiId, %weapon);
					if(%x != -1)
						storeData(%aiId, "LoadedProjectile " @ %weapon, %x);
				}

				if(%x != -1)
				{
					Player::useItem(%aiId, %weapon);
					if(Player::getMountedItem(%clientId, $WeaponSlot) == %weapon || Player::getNextMountedItem(%aiId, $WeaponSlot) == %weapon)
						break;
				}
			}
		}
	}

	AI::SetSpotDist(%aiId);
}

function AI::SelectBestWeapon(%aiId)
{
	dbecho($dbechoMode, "AI::SelectBestWeapon(" @ %aiId @ ")");

	%weapon = GetBestWeapon(%aiId);
	if(%weapon != -1)
	{
		%x = "";
		if(GetAccessoryVar(%weapon, $AccessoryType) == $RangedAccessoryType)
		{
			%x = GetBestRangedProj(%aiId, %weapon);
			if(%x != -1)
				storeData(%aiId, "LoadedProjectile " @ %weapon, %x);
		}

		if(%x != -1)
		{
			Player::useItem(%aiId, %weapon);
			AI::SetSpotDist(%aiId);
		}
	}

	return -1;
}

function AI::SetSpotDist(%aiId)
{
	dbecho($dbechoMode, "AI::SetSpotDist(" @ %aiId @ ")");

	if(fetchData(%aiId, "frozen") || fetchData(%aiId, "dumbAIflag"))
		return;

	%item = Player::getMountedItem(%aiId, $WeaponSlot);

	AI::setVar(fetchData(%aiId, "BotInfoAiName"), SpotDist, GetRange(%item));
	AI::setVar(fetchData(%aiId, "BotInfoAiName"), triggerPct, 1.0);
}

function AI::activelyFollow(%aiName, %curTarget, %bypass)
{
	dbecho($dbechoMode, "AI::activelyFollow(" @ %aiName @ ", " @ %curTarget @ ", " @ %bypass @ ")");

	%aiId = ai::getId(%aiName);

	if(GameBase::getTeam(%aiId) != GameBase::getTeam(%curTarget) || %bypass)
	{
		//echo("Sending " @ %aiName @ " to actively follow and attack " @ %curTarget);
		AI::newDirectiveFollow(%aiName, %curTarget, 0, 99);
	}
}

function AI::moveToAttackMarker(%name, %method)
{
	dbecho($dbechoMode, "AI::moveToAttackMarker(" @ %name @ ", " @ %method @ ")");

	%AIid = AI::getId(%name);

	if(fetchData(%aiId, "dumbAIflag"))
		return;

      %tempSet = nameToID("MissionGroup\\Teams\\team1\\AIattackMarkers");

	if(%tempSet != -1)
	{
		%num = Group::objectCount(%tempSet);

		if(%method == 0)
		{
			//pick a random marker
			%r = floor(getRandom() * %num);
		}
		else if(%method == 1)
		{
			//pick nearest marker
			%dist = 1000000;
			for(%i=0; %i<=%num-1; %i++)
			{
				%m = Group::getObject(%tempSet, %i);
				%testdist = Vector::getDistance(GameBase::getPosition(%AIid), GameBase::getPosition(%m));
				if(%testdist < %dist)
				{
					%dist = %testdist;
					%r = %i;
				}
			}
		}
	      %marker = Group::getObject(%tempSet, %r);
	
		%worldLoc = GameBase::getPosition(%marker);

		AI::newDirectiveWaypoint(%name, %worldLoc, 99);
		storeData(%aiId, "AIattackMarker", %marker);

		//echo(%name @ " IS PROCEEDING TO LOCATION " @ %worldLoc);

		return True;
	}
	return False;
}

function AI::moveSomewhere(%aiName)
{
	dbecho($dbechoMode, "AI::moveSomewhere(" @ %aiName @ ")");

	%aiId = AI::getId(%aiName);

	if(fetchData(%aiId, "dumbAIflag"))
		return;

	//echo(%aiId @ " ======> Ok, i'm considering giving the bot a new place to go.");
	if(fetchData(%aiId, "AIattackMarker") != "")
	{
		%dist = Vector::getDistance(GameBase::getPosition(fetchData(%aiId, "AIattackMarker")), GameBase::getPosition(%aiId));
		//echo(%aiId @ " ======> It seems this bot has an attackMarker, and it's this far: " @ %dist);
		if(%dist <= $AIcloseEnoughMarkerDist)
		{
			//echo(%aiId @ " ======> The distance " @ %dist @ " seems to be close enough to cancel the attackMarker!");
			storeData(%aiId, "AIattackMarker", "");
		}
	}
	if(fetchData(%aiId, "AIattackMarker", ""))
	{
		//echo(%aiId @ " ======> The bot has no attack marker, so I'm attempting to give bot somewhere to go!");
		%aiPos = GameBase::getPosition(%aiId);
		%minrad = $AIminrad;
		%maxrad = $AImaxrad;

		%tempPos = RandomPositionXY(%minrad, %maxrad);

		%xPos = GetWord(%tempPos, 0) + GetWord(%aiPos, 0);
		%yPos = GetWord(%tempPos, 1) + GetWord(%aiPos, 1);
		%zPos = GetWord(%aiPos, 2); //doesn't matter; the bot can't go thru terrain
		
		%newPos = %xPos @ " " @ %yPos @ " " @ %zPos;

		storeData(%aiId, "AIattackMarker", "");
		AI::newDirectiveWaypoint(%aiName, %newPos, 99);

		//echo(%aiName @ " IS WANDERING TO LOCATION " @ %newPos);
	}
}

//experimental function, which makes bot look around himself at preset angles and, whichever is the furthest, go there.
//i'm hoping this will simulate a smarter bot.

//test results: seems to work great!  hopefully it doesn't cause too much lag, but up to now... looks fine
function AI::moveToFurthest(%aiName)
{
	dbecho($dbechoMode, "AI::moveToFurthest(" @ %aiName @ ")");

	%aiId = AI::getId(%aiName);

	if(fetchData(%aiId, "dumbAIflag"))
		return;

	if(fetchData(%aiId, "AIattackMarker") != "")
	{
		%dist = Vector::getDistance(GameBase::getPosition(fetchData(%aiId, "AIattackMarker")), GameBase::getPosition(%aiId));
		if(%dist <= $AIcloseEnoughMarkerDist)
		{
			storeData(%aiId, "AIattackMarker", "");
		}
	}
	if(fetchData(%aiId, "AIattackMarker") == "")
	{
		%aiPos = GameBase::getPosition(%aiId);
		%aiPlayer = Client::getOwnedObject(%aiId);

		%furthest = -1;
		for(%i = 0; %i <= 6.283; %i+= 0.52)
		{
			GameBase::getLOSinfo(%aiPlayer, 1000, "0 0 " @ %i);
			%dist = Vector::getDistance(%aiPos, $los::position);
			if(%dist > %furthest && $los::position != "0 0 0" && $los::position != "")
			{
				%furthest = %dist;
				%chosenPos = $los::position;
			}
		}
		if(%furthest == -1)
		{
			//it seems the bot only sees sky, so revert to AI::moveSomewhere
			AI::moveSomewhere(%aiName);
			return;
		}

		%finalPos = %chosenPos;

		AI::newDirectiveWaypoint(%aiName, %finalPos, 99);

		//echo(%aiName @ " FOUND THE FURTHEST POINT AT LOCATION " @ %chosenPos);
	}
}


//-----------------------------------
// AI::initDrones()
//-----------------------------------
function AI::initDrones(%team, %numAi)
{
	dbecho($dbechoMode, "AI::initDrones(" @ %team @ ", " @ %numAi @ ")");

	dbecho(1, "spawning team " @ %team @ " ai...");
	for(%guard = 0; %guard < %numAi; %guard++)
	{
		//check for internal data
		%tempSet = nameToID("MissionGroup\\Teams\\team" @ %team @ "\\AI");
		%tempItem = Group::getObject(%tempSet, %guard);
		%aiName = Object::getName(%tempItem);

		%set = nameToID("MissionGroup\\Teams\\team" @ %team @ "\\AI\\" @ %aiName);
		%numPts = Group::objectCount(%set);

		if(%numPts > 0)
		{
			GatherBotInfo(%set);

			createAI(%aiName, %set, $BotInfo[%aiName, NAME]);
			%aiId = AI::getId( %aiName );
			AI::setVar( %aiName, iq,  100 );
			AI::setVar( %aiName, attackMode, $AIattackMode);
			AI::setVar( %aiName, pathType, $AI::defaultPathType);
			AI::setWeapons(%aiName);

			UpdateTeam(%aiId);

			//**RPG (added because AI::onDroneKilled doesn't conserve the AI's team)
			storeData(%aiId, "botTeam", %team);
			//**
		}
		else
			dbecho(1, "no info to spawn ai...");
	}
}


//------------------------------------------------------------------
//functions to test and move AI players.
//
//------------------------------------------------------------------

$numAI = 0;
function AI::helper(%aiName, %displayName, %commandIssuer, %loadout)
{
	dbecho($dbechoMode, "AI::helper(" @ %aiName @ ", " @ %displayName @ ", " @ %commandIssuer @ ")");

	if(GetWord(%commandIssuer, 0) == "TempSpawn")
	{
		//the %commandIssuer is a data string
		%spawnPos = GetWord(%commandIssuer, 1) @ " " @ GetWord(%commandIssuer, 2) @ " " @ GetWord(%commandIssuer, 3);
	}
	else if(GetWord(%commandIssuer, 0) == "MarkerSpawn")
	{
		//the %commandIssuer is a marker
		%spawnPos = GameBase::getPosition(GetWord(%commandIssuer, 1));
	}
	else if(GetWord(%commandIssuer, 0) == "SpawnPoint")
	{
		//the %commandIssuer is a Spawn Point
		//we must now figure out a position around this Spawn Point

		%spawnpoint = GetWord(%commandIssuer, 1);

		%info = Object::getName(%spawnpoint);

		%minrad = GetWord(%info, 1);
		%maxrad = GetWord(%info, 2);

		%spawnPointPos = GameBase::getPosition(%spawnpoint);
		%tempPos = RandomPositionXY(%minrad, %maxrad);
		%xPos = GetWord(%tempPos, 0) + GetWord(%spawnPointPos, 0);
		%yPos = GetWord(%tempPos, 1) + GetWord(%spawnPointPos, 1);
		%zPos = GetWord(%spawnPointPos, 2);

		%spawnPos = %xPos @ " " @ %yPos @ " " @ %zPos;
	}

	%n = getAInumber();

	%newName = %aiName @ %n;
	if(%aiName == %displayName)
		%displayName = $NameForRace[%aiName] @ %newName;
	$numAI++;
	SpawnAI(%newName, %displayName, %spawnPos, %commandIssuer, %loadout);

	setAInumber(%newName, %n);

	return %newName;
}
function SpawnAI(%newName, %displayName, %aiSpawnPos, %commandIssuer, %loadout)
{
	dbecho($dbechoMode, "SpawnAI(" @ %newName @ ", " @ %displayName @ ", " @ %aiSpawnPos @ ", " @ %commandIssuer @ ")");

	%retval = createAI(%newName, %aiSpawnPos, %displayName);

	if(%retval != -1)
	{
		%aiId = AI::getId( %newName );
		AI::setVar( %newName,  iq,  100 );
		AI::setVar( %newName,  attackMode, $AIattackMode);
		AI::setVar( %newName,  pathType, $AI::defaultPathType);
		//AI::SetVar( %newName,  seekOff, 1);
		AI::setAutomaticTargets( %newName );

		if(GetWord(%commandIssuer, 0) == "TempSpawn")
		{
			//the %commandIssuer is a data string
			storeData(%aiId, "SpawnBotInfo", %commandIssuer);
			%team = GetWord(%commandIssuer, 4);
			GameBase::setTeam(%aiId, %team);

			AI::SetVar(%newName, spotDist, $AIspotDist);
		}
		else if(GetWord(%commandIssuer, 0) == "MarkerSpawn")
		{
			//the %commandIssuer is a marker
			storeData(%aiId, "SpawnBotInfo", %commandIssuer);
			%team = GameBase::getMapName(GetWord(%commandIssuer, 1));
			if(%team == "") %team = 0;
			GameBase::setTeam(%aiId, %team);

			AI::SetVar(%newName, spotDist, $AIspotDist);
		}
		else if(GetWord(%commandIssuer, 0) == "SpawnPoint")
		{
			//the %commandIssuer is a spawn crystal
			storeData(%aiId, "SpawnBotInfo", %commandIssuer);

			$numAIperSpawnPoint[GetWord(%commandIssuer, 1)]++;
			UpdateTeam(%aiId);

			AI::SetVar(%newName, spotDist, $AIspotDist);
		}

		AI::setWeapons(%newName, %loadout);

		return ( %newName );
	}
	else
	{
		return -1;
	}
}

//
//This function will move an AI player to the position of an object
//that the players LOS is hitting(terrain included). Must be within 50 units.
//
//
function AI::moveToLOS(%aiName, %commandIssuer) 
{
	dbecho($dbechoMode, "AI::moveToLos(" @ %aiName @ ", " @ %commandIssuer @ ")");

	%issuerRot = GameBase::getRotation(%commandIssuer);
	%playerObj = Client::getOwnedObject(%commandIssuer);
	%playerPos = GameBase::getPosition(%commandIssuer);
      
	//check within max dist
	if(GameBase::getLOSInfo(%playerObj, 100, %issuerRot))
	{ 
		%newIssuedVec = $LOS::position;
		%distance = Vector::getDistance(%playerPos, %newIssuedVec);
		dbecho(2, "Command accepted, AI player(s) moving....");
		dbecho(2, "distance to LOS: " @ %distance);
		AI::newDirectiveWaypoint( %aiName, %newIssuedVec, 99 );
	}
	else
		dbecho(2, "Distance too far.");

	dbecho(2, "LOS point: " @ $LOS::position);
}

//This function will move an AI player to a position directly in front of
//the player passed, at a distance that is specified.
function AI::moveAhead(%aiName, %commandIssuer, %distance) 
{
	dbecho($dbechoMode, "AI::moveAhead(" @ %aiName @ ", " @ %commandIssuer @ ", " @ %distance @ ")");

	%issuerRot = GameBase::getRotation(%commandIssuer);
	%commPos  = GameBase::getPosition(%commandIssuer);
	dbecho(2, "Commanders Position: " @ %commPos);

	//get commanders x and y positions
	%comm_x = getWord(%commPos, 0);
	%comm_y = getWord(%commPos, 1);

	//get offset x and y positions
	%offSetPos = Vector::getFromRot(%issuerRot, %distance);
	%off_x = getWord(%offSetPos, 0);
	%off_y = getWord(%offSetPos, 1);

	//calc new position
	%new_x = %comm_x + %off_x;
	%new_y = %comm_y + %off_y;
	%newPos = %new_x  @ " " @ %new_y @ " 0";

	//move AI player
	dbecho(2, "AI moving to " @ %newPos);
	AI::newDirectiveWaypoint(%aiName, %newPos, 99);
}  

//
// OK, this is the complete command callback - issued for any command sent
//    to an AI. 
//
function AI::onCommand ( %name, %commander, %command, %waypoint, %targetId, %cmdText, %cmdStatus, %cmdSequence)
{
	dbecho($dbechoMode, "AI::onCommand(" @ %name @ ", " @ %commander @ ", " @ %command @ ", " @ %waypoint @ ", " @ %targetId @ ", " @ %cmdText @ ", " @ %cmdStatus @ ", " @ %cmdSequence @ ")");

	%aiId = AI::getId( %name );
	%T = GameBase::getTeam( %aiId );
	%groupId = nameToID("MissionGroup\\Teams\\team" @ %T @ "\\AI\\" @ %name ); 
	%nodeCount = Group::objectCount( %groupId );
	dbecho(2, "checking drone information...." @ " number of nodes: " @ %nodeCount);
	dbecho(2, "AI id: " @ %aiId @ " groupId: " @ %groupId);
   
	if($SinglePlayer) // || %nodeCount == 1
	{
		if( %command == 2 || %command == 1 )
		{
			// must convert waypoint location into world location.  waypoint location
			//    is given in range [0-1023, 0-1023].  
			%worldLoc = WaypointToWorld ( %waypoint );
			AI::newDirectiveWaypoint( %name, %worldLoc, 99 );
			dbecho ( 2, %name @ " IS PROCEEDING TO LOCATION " @ %worldLoc );
		}
		dbecho( 2, "AI::OnCommand() issued to  " @ %name @ "  with parameters: " );
		dbecho( 3, "Cmdr:        " @ %commander );
		dbecho( 3, "Command:     " @ %command );
		dbecho( 3, "Waypoint:    " @ %waypoint );
		dbecho( 3, "TargetId:    " @ %targetId );
		dbecho( 3, "cmdText:     " @ %cmdText );
		dbecho( 3, "cmdStatus:   " @ %cmdStatus );
		dbecho( 3, "cmdSequence: " @ %cmdSequence );
	}
	else
		return;   
}

// Play the given wave file FROM %source to %DEST.  The wave name is JUST the basic wave
// name without voice base info (which it will grab for you from the source clientId).
// Basically does some string fiddling for you.
//
// Example:
//    Ai::soundHelper( 2051, 2049, cheer3 );
//
function Ai::soundHelper( %sourceId, %destId, %waveFileName )
{
	dbecho($dbechoMode, "Ai::soundHelper(" @ %sourceId @ ", " @ %destId @ ", " @ %waveFileName @ ")");

	%wName = strcat( "~w", Client::getVoiceBase( %sourceId ) );
	%wName = strcat( %wName, ".w" );
	%wName = strcat( %wName, %waveFileName );
	%wName = strcat( %wName, ".wav" );

	dbecho( 2, "Trying to play " @ %wName );

	Client::sendMessage( %destId, 0, %wName );
}


// Default periodic callback.  [Note by default it isn't called unless a frequency 
//    is set up using AI::CallbackPeriodic().  Type in that command to see how 
//    it works].  
function AI::onPeriodic( %aiName )
{
	dbecho($dbechoMode, "AI::onPeriodic(" @ %aiName @ ")");

	echo("onPeriodic() called with " @ %aiName);
}


function AI::onDroneKilled(%aiName)
{
	dbecho($dbechoMode, "AI::onDroneKilled(" @ %aiName @ ")");

	if(!$SinglePlayer )
	{
	      %aiId = AI::getId(%aiName);

      	%team = fetchData(%aiId, "botTeam");
		storeData(%aiId, "botTeam", "");
		$aiNumTable[$tmpbotn[%aiName]] = "";
		$tmpbotn[%aiName] = "";

		if(fetchData(%aiId, "SpawnBotInfo") != "")
		{
			if(GetWord(fetchData(%aiId, "SpawnBotInfo"), 0) == "SpawnPoint")
			{
				//this bot originally spawned from a crystal
				$numAIperSpawnPoint[GetWord(fetchData(%aiId, "SpawnBotInfo"), 1)]--;
			}
			storeData(%aiId, "SpawnBotInfo", "");
			storeData(%aiId, "AIattackMarker", "");

			//pet stuff
			$PetList = RemoveFromCommaList($PetList, %aiId);
			%petowner = fetchData(%aiId, "petowner");
			storeData(%petowner, "PersonalPetList", RemoveFromCommaList(fetchData(%petowner, "PersonalPetList"), %aiId));
			Client::sendMessage(%petowner, $MsgRed, Client::getName(%aiId) @ " was slain!");
			storeData(%aiId, "petowner", "");
			
			//botgroup stuff
			%b = AI::IsInWhichBotGroup(%aiId);
			if(%b != -1)
				AI::RemoveBotFromBotGroup(%aiId, %b);
		}
		else
		{
			schedule("AI::setupAI(" @ %aiName @ ", " @ %team @ ");", 60);
	      }
	}
	else
	{
		// just in case:
		dbecho( 2, "Non training callback called from Training" );
	}
}

//These will only be invoked if the target is REALLY close to the bot (since the SpotDist is only the range of the
//weapon).  This means that if the bot ever gets close enough to engage in battle, he will try his best to continue
//the fight by following the target.  Once the target is lost or dies, directive 99 will be cancelled and directive
//99 will take over (regular walking, formations etc)
function AI::onTargetLOSAcquired(%aiName, %idNum)
{
	dbecho($dbechoMode, "AI::onTargetLOSAcquired(" @ %aiName @ ", " @ %idNum @ ")");

	%aiId = AI::getId(%aiName);

	if(fetchData(%aiId, "SpawnBotInfo") != "" && !fetchData(%aiId, "dumbAIflag"))
		AI::newDirectiveFollow(%aiName, %idNum, 0, 99);
}

function AI::onTargetLOSLost(%aiName, %idNum)
{
	dbecho($dbechoMode, "AI::onTargetLOSLost(" @ %aiName @ ", " @ %idNum @ ")");

	%aiId = AI::getId(%aiName);

	if(fetchData(%aiId, "SpawnBotInfo") != "" && !fetchData(%aiId, "dumbAIflag"))
		AI::newDirectiveRemove(%aiName, 99);
}

function AI::onTargetLOSRegained(%aiName, %idNum)
{
	dbecho($dbechoMode, "AI::onTargetLOSRegained(" @ %aiName @ ", " @ %idNum @ ")");

	%aiId = AI::getId(%aiName);

	if(fetchData(%aiId, "SpawnBotInfo") != "" && !fetchData(%aiId, "dumbAIflag"))
		AI::newDirectiveFollow(%aiName, %idNum, 0, 99);
}

function AI::onTargetDied(%aiName, %idNum)
{
	dbecho($dbechoMode, "AI::onTargetDied(" @ %aiName @ ", " @ %idNum @ ")");

	%aiId = AI::getId(%aiName);

	if(fetchData(%aiId, "SpawnBotInfo") != "" && !fetchData(%aiId, "dumbAIflag"))
		AI::newDirectiveRemove(%aiName, 99);
}                                 

function AI::sayLater(%clientId, %guardId, %message, %look)
{
	dbecho($dbechoMode, "AI::sayLater(" @ %clientId @ ", " @ %guardId @ ", " @ %message @ ", " @ %look @ ")");

	%name = Client::getName(%clientId);

	Client::sendMessage(%clientId, $MsgBeige, $BotInfo[%guardId.name, NAME] @ " tells you, \"" @ %message @ "\"");

//	if(%look)
//		AI::lookAtPlayer(%clientId, %guardId);
}
function AI::lookAtPlayer(%clientId, %guardId)
{
	dbecho($dbechoMode, "AI::lookAtPlayer(" @ %clientId @ ", " @ %guardId @ ")");

	%clpos = GameBase::getPosition(%clientId);
	%gupos = GameBase::getPosition(%guardId);

	%v1 = Vector::sub(%clpos, %gupos);

	%norm = Vector::normalize(%v1);
	%rot = Vector::getRotation(%norm);

	GameBase::setRotation(%guardId, %rot);

	%gurot = GameBase::getRotation(%guardId);
	%temp = Vector::sub(%rot, %gurot);
	%temp2 = GetWord(%temp, 2);

	if(floor(%temp2) != 0)
		%rot = GetWord(%rot, 0) @ " " @ GetWord(%rot, 1) @ " " @ (GetWord(%rot, 2) + 3.141592654);

	RotateTownBot(%guardId, %rot);
}

function getAInumber()
{
	dbecho($dbechoMode, "getAInumber()");

	for(%i = 0; %i <= 5000; %i++)
	{
		if($aiNumTable[%i] == "")
		{
			return %i;
		}
	}
}
function setAInumber(%aiName, %n)
{
	dbecho($dbechoMode, "setAInumber(" @ %aiName @ ", " @ %n @ ")");

	$aiNumTable[%n] = True;
	$tmpbotn[%aiName] = %n;
}

//=================================================
function AI::SelectMovement(%aiName)
{
	dbecho($dbechoMode, "AI::SelectMovement(" @ %aiName @ ")");

	%aiId = AI::getId(%aiName);

	if(fetchData(%aiId, "botAttackMode") == 1)
	{
		//Regular walk

		if(IsInArenaDueler(%aiId) || IsInRoster(%aiId))
			%r = OddsAre(1);
		else
			%r = OddsAre($AImoveChance);
		
		if(%r && !fetchData(%aiId, "frozen"))
		{
			%s = RandomRaceSound(fetchData(%aiId, "RACE"), RandomWait);
			if(%s == "NoSound")
				PlaySound(SoundGrunt1, GameBase::getPosition(%aiId));
			else
				PlaySound(%s, GameBase::getPosition(%aiId));
			AI::moveToFurthest(%aiName);
		}
	}
	else if(fetchData(%aiId, "botAttackMode") == 2)
	{
		//Follow a specific player target

		%followId = fetchData(%aiId, "tmpbotdata");
		if(%followId != %aiId)
			AI::newDirectiveFollow(%aiName, %followId, 0, 99);
	}
	else if(fetchData(%aiId, "botAttackMode") == 3)
	{
		//Attack at certain position
		AI::newDirectiveWaypoint(%aiName, fetchData(%aiId, "tmpbotdata"), 99);
	}
	else if(fetchData(%aiId, "botAttackMode") == 4)
	{
		//BotGroup formation

		%a = AI::IsInWhichBotGroup(%aiId);
		%g = $tmpBotGroup[%a];

		//NOTE: can't make the bots follow a random bot in the group because at one point or another,
		//the bots will pick a follow combination which will NOT involve the team leader, leaving the
		//team leader alone.
		//This new method involves a North East oriented line.

		for(%i = 1; (%b = GetWord(%g, %i)) != -1; %i++)
		{
			if(%b == %aiId)
				%n = %i-1;
		}

		%followId = GetWord(%g, %n);

		if(!fetchData(%aiId, "frozen"))
		{
			if(%followId != %aiId)
				AI::newDirectiveFollow(%aiName, %followId, 0, 99);
			else
				AI::moveToFurthest(%aiName);					//team leader gets to move.
		}

	}
}

function HardcodeAIskills(%aiId)
{
	dbecho($dbechoMode, "HardcodeAIskills(" @ %aiId @ ")");

	SetAllSkills(%aiId, 0);

	%ns = getNumSkills();
	%a = $autoStartupSP + round($initSPcredits / %ns) + round(((fetchData(%aiId, "LVL")-1) * $SPgainedPerLevel) / %ns);
	for(%i = 1; %i <= %ns; %i++)
		AddSkillPoint(%aiId, %i, %a);

	//==== HARDCODED SKILLS TO ENSURE CHALLENGING BOTS ============
	$PlayerSkill[%aiId, $SkillSlashing] = (getRandom() * $SkillRangePerLevel) + ((fetchData(%aiId, "LVL")-1) * $SkillRangePerLevel);
	$PlayerSkill[%aiId, $SkillPiercing] = (getRandom() * $SkillRangePerLevel) + ((fetchData(%aiId, "LVL")-1) * $SkillRangePerLevel);
	$PlayerSkill[%aiId, $SkillBludgeoning] = (getRandom() * $SkillRangePerLevel) + ((fetchData(%aiId, "LVL")-1) * $SkillRangePerLevel);
	$PlayerSkill[%aiId, $SkillDodging] = (getRandom() * $SkillRangePerLevel) + ((fetchData(%aiId, "LVL")-1) * $SkillRangePerLevel);
	$PlayerSkill[%aiId, $SkillArchery] = (getRandom() * $SkillRangePerLevel) + ((fetchData(%aiId, "LVL")-1) * $SkillRangePerLevel);
	$PlayerSkill[%aiId, $SkillOffensiveCasting] = (getRandom() * $SkillRangePerLevel) + ((fetchData(%aiId, "LVL")-1) * $SkillRangePerLevel);
	$PlayerSkill[%aiId, $SkillDefensiveCasting] = (getRandom() * $SkillRangePerLevel) + ((fetchData(%aiId, "LVL")-1) * $SkillRangePerLevel);
	$PlayerSkill[%aiId, $SkillNeutralCasting] = (getRandom() * $SkillRangePerLevel) + ((fetchData(%aiId, "LVL")-1) * $SkillRangePerLevel);
	$PlayerSkill[%aiId, $SkillEnergy] = (getRandom() * $SkillRangePerLevel) + ((fetchData(%aiId, "LVL")-1) * $SkillRangePerLevel);
	$PlayerSkill[%aiId, $SkillSpeech] = $SkillCap;
	$PlayerSkill[%aiId, $SkillWeightCapacity] = (getRandom() * $SkillRangePerLevel) + ((fetchData(%aiId, "LVL")-1) * $SkillRangePerLevel);
	$PlayerSkill[%aiId, $SkillEndurance] = ( (getRandom() * $SkillRangePerLevel) + ((fetchData(%aiId, "LVL")-1) * $SkillRangePerLevel) ) / 2;
	$PlayerSkill[%aiId, $SkillSenseHeading] = (getRandom() * $SkillRangePerLevel) + ((fetchData(%aiId, "LVL")-1) * $SkillRangePerLevel);

	%a = (  (getRandom() * $SkillRangePerLevel) + ((fetchData(%aiId, "LVL")-1) * $SkillRangePerLevel)  ) / 2;
	%sr = round(%a * GetSkillMultiplier(%aiId, $SkillOffensiveCasting));
	$PlayerSkill[%aiId, $SkillSpellResistance] = %sr;
	//=============================================================
}

//------ BotGroup stuff ---------------------------------

function AI::IsInWhichBotGroup(%aiId)
{
	dbecho($dbechoMode, "AI::IsInWhichBotGroup(" @ %aiId @ ")");

	for(%i = 0; (%a = GetWord($BotGroups, %i)) != -1; %i++)
	{
		for(%j = 0; (%b = GetWord($tmpBotGroup[%a], %j)) != -1; %j++)
		{
			if(%b == %aiId)
				return %a;
		}
	}
	return -1;
}

function AI::CreateBotGroup(%group)
{
	dbecho($dbechoMode, "AI::CreateBotGroup(" @ %group @ ")");

	$BotGroups = $BotGroups @ %group @ " ";
	$tmpBotGroup[%group] = "";
}

function AI::DiscardBotGroup(%group)
{
	dbecho($dbechoMode, "AI::DiscardBotGroup(" @ %group @ ")");

	for(%i = 0; (%a = GetWord($tmpBotGroup[%group], %i)) != -1; %i++)
		storeData(%a, "botAttackMode", 1);

	$BotGroups = FixStuffString($BotGroups);
	$BotGroups = String::replace($BotGroups, " " @ %group @ " ", " ");
	$tmpBotGroup[%group] = "";
}

function AI::CountBotGroupMembers(%group)
{
	dbecho($dbechoMode, "AI::CountBotGroupMembers(" @ %group @ ")");

	for(%i = 0; (%a = GetWord($tmpBotGroup[%group], %i)) != -1; %i++){}
	return %i;
}

function AI::IsInBotGroup(%aiId, %group)
{
	dbecho($dbechoMode, "AI::IsInBotGroup(" @ %aiId @ ", " @ %group @ ")");

	for(%i = 0; (%a = GetWord($tmpBotGroup[%group], %i)) != -1; %i++)
	{
		if(%aiId == %a)
			return True;
	}
	return False;
}

function AI::BotGroupExists(%group)
{
	dbecho($dbechoMode, "AI::BotGroupExists(" @ %group @ ")");

	for(%i = 0; (%a = GetWord($BotGroups, %i)) != -1; %i++)
	{
		if(%a == %group)
			return True;
	}
	return False;
}

function AI::RemoveBotFromBotGroup(%aiId, %group)
{
	dbecho($dbechoMode, "AI::RemoveBotFromGroup(" @ %aiId @ ", " @ %group @ ")");

	$tmpBotGroup[%group] = String::Replace($tmpBotGroup[%group], %aiId @ " ", "");
	storeData(%aiId, "botAttackMode", 1);
}

function AI::AddBotToBotGroup(%aiId, %group)
{
	dbecho($dbechoMode, "AI::AddBotToBotGroup(" @ %aiId @ ", " @ %group @ ")");

	$tmpBotGroup[%group] = $tmpBotGroup[%group] @ %aiId @ " ";
	storeData(%aiId, "botAttackMode", 4);
}

//------ remastered directives ------------------------------

function AI::newDirectiveFollow(%aiName, %idNum, %rad, %directive)
{
	dbecho($dbechoMode, "AI::newDirectiveFollow(" @ %aiName @ ", " @ %idNum @ ", " @ %rad @ ", " @ %directive @ ")");

	%aiId = AI::getId(%aiName);
	AI::newDirectiveRemove(%aiName, %directive);

	$aidirectiveTable[%aiId, %directive] = "follow";
	AI::directiveFollow(%aiName, %idNum, %rad, %directive);
}

function AI::newDirectiveWaypoint(%aiName, %pos, %directive)
{
	dbecho($dbechoMode, "AI::newDirectiveWaypoint(" @ %aiName @ ", " @ %pos @ ", " @ %directive @ ")");

	%aiId = AI::getId(%aiName);
	AI::newDirectiveRemove(%aiName, %directive);

	$aidirectiveTable[%aiId, %directive] = "waypoint";
	AI::directiveWaypoint(%aiName, %pos, %directive);
}

function AI::newDirectiveRemove(%aiName, %directive)
{
	dbecho($dbechoMode, "AI::newDirectiveRemove(" @ %aiName @ ", " @ %directive @ ")");

	%aiId = AI::getId(%aiName);
	if($aidirectiveTable[%aiId, %directive] != "")
	{
		AI::directiveRemove(%aiName, %directive);
		$aidirectiveTable[%aiId, %directive] = "";
	}
}

//------- TownBot stuff ----------------------------------------

function InitTownBots()
{
	dbecho($dbechoMode, "InitTownBots()");

	$TownBotList = "";

	%group = nameToId("MissionGroup/TownBots");

	if(%group != -1)
	{
		%cnt = Group::objectCount(%group);
		for(%i = 0; %i <= %cnt - 1; %i++)
		{
			%object = Group::getObject(%group, %i);
			%name = Object::getName(%object);
			if(getObjectType(%object) == "SimGroup")
			{
				%marker = GatherBotInfo(%object);
			}

			%townbot = newObject("", "StaticShape", $BotInfo[%name, RACE] @ "TownBot", true);

			addToSet("MissionCleanup", %townbot);
			GameBase::setMapName(%townbot, $BotInfo[%name, NAME]);
			GameBase::setPosition(%townbot, GameBase::getPosition(%marker));
			GameBase::setRotation(%townbot, GameBase::getRotation(%marker));
			GameBase::setTeam(%townbot, $BotInfo[%name, TEAM]);
			GameBase::playSequence(%townbot, 0, "root");	//thanks Adger!!
			%townbot.name = %name;

			$TownBotList = $TownBotList @ %townbot @ " ";
		}
	}
}
function RotateTownBot(%id, %rot)
{
	dbecho($dbechoMode, "RotateTownBot(" @ %id @ ", " @ %rot @ ")");

	%pos = GameBase::getPosition(%id);
	%name = %id.name;

	//delete the bot
	$TownBotList = String::replace($TownBotList, %id @ " ", "");
	deleteObject(%id);

	//re-create the bot
	%townbot = newObject("", "Item", $BotInfo[%name, RACE] @ "TownBot", 1, false);
	GameBase::setRotation(%townbot, %rot);

	addToSet("MissionCleanup", %townbot);
	GameBase::setMapName(%townbot, $BotInfo[%name, NAME]);
	GameBase::setPosition(%townbot, %pos);
	GameBase::setTeam(%townbot, $BotInfo[%name, TEAM]);
	GameBase::playSequence(%townbot, 0, "root");	//thanks Adger!!
	%townbot.name = %name;

	$TownBotList = $TownBotList @ %townbot @ " ";
}
function newRotateTownBot(%client, %object, %clientPos, %botPos){	%dist = vector::getDistance(%clientPos, %botPos);	%rot = Vector::getRotation(Vector::normalize(Vector::sub(%clientPos, %botPos)));	%rot = "0 -0 "@GetWord(%rot, 2);	if(%dist < 1.57)	{//Prevent us from trapping the player		player::applyimpulse(%client, vector::getfromrot(%rot, 30, 12));		schedule("newRotateTownBot("@%client@", "@%object@", gamebase::getposition("@%client@"), gamebase::getposition("@%object@"));",0.3);		return;	}	GameBase::setRotation(%object, %rot);}

function GatherBotInfo(%group)
{
	dbecho($dbechoMode, "GatherBotInfo(" @ %group @ ")");

	%biggestn = 0;
	%aiName = Object::getName(%group);

	%count = Group::objectCount(%group);
	for(%i = 0; %i <= %count-1; %i++)
	{
		%object = Group::getObject(%group, %i);
		if(getObjectType(%object) == "SimGroup")
		{
			%system = Object::getName(%object);
			%type = GetWord(%system, 0);
			%info = String::getSubStr(%system, String::len(%type)+1, 9999);

			%type2 = clipTrailingNumbers(%type);
			%n = floor(String::getSubStr(%type, String::len(%type2), 9999));

			if(%type == "NAME")
				$BotInfo[%aiName, NAME] = %info;
			else if(%type == "LVL" || %type == "LEVEL")
				$BotInfo[%aiName, LVL] = %info;
			else if(%type == "RACE")
				$BotInfo[%aiName, RACE] = %info;
			else if(%type == "NEED")
				$BotInfo[%aiName, NEED] = %info;
			else if(%type == "TAKE")
				$BotInfo[%aiName, TAKE] = %info;
			else if(%type == "GIVE")
				$BotInfo[%aiName, GIVE] = %info;
			else if(%type == "SHOP")
				$BotInfo[%aiName, SHOP] = %info;
			else if(%type == "ITEMS")
				$BotInfo[%aiName, ITEMS] = %info;
			else if(%type == "CSAY")
				$BotInfo[%aiName, CSAY] = %info;
			else if(%type == "LSAY")
				$BotInfo[%aiName, LSAY] = %info;
			else if(%type == "LCK")
				$BotInfo[%aiName, LCK] = %info;
			else if(%type == "SIMGROUP")
				$BotInfo[%aiName, SIMGROUP] = %info;

			if(%type2 == "SAY")
				$BotInfo[%aiName, SAY, %n] = %info;
			else if(%type2 == "CUE")
				$BotInfo[%aiName, CUE, %n] = %info;
			else if(%type2 == "NSAY")
				$BotInfo[%aiName, NSAY, %n] = %info;
			else if(%type2 == "NCUE")
				$BotInfo[%aiName, NCUE, %n] = %info;

			if(%n > %biggestn)
				%biggestn = %n;
		}
		else
			%marker = %object;
	}
	$BotInfo[%aiName, SAY, %biggestn+1] = "";
	$BotInfo[%aiName, NSAY, %biggestn+1] = "";
	$BotInfo[%aiName, CUE, %biggestn+1] = "";
	$BotInfo[%aiName, NCUE, %biggestn+1] = "";

	//==============================================
	//The following is generally BotMaker-only code
	//==============================================
	if($BotInfo[%aiName, SIMGROUP] != "")
	{
		%g = nameToId("MissionGroup\\" @ $BotInfo[%aiName, SIMGROUP]);

		%count = Group::objectCount(%g);
		for(%i = 0; %i <= %count-1; %i++)
		{
			%object = Group::getObject(%g, %i);
			if(getObjectType(%object) == "SimGroup")
			{
				%system = Object::getName(%object);
				%type = GetWord(%system, 0);
				%info = String::getSubStr(%system, String::len(%type)+1, 9999);

				if(%type == "NAMES")
					$BotInfo[%aiName, NAMES] = %info;
				else if(%type == "DEFAULTS")
				{
					%class = GetWord(%info, 0);
					%stuff = String::getSubStr(%info, String::len(%class)+1, 9999);

					$BotInfo[%aiName, DEFAULTS, %class] = %stuff;
				}
			}
			else if(getObjectType(%object) == "Marker")
			{
				$BotInfo[%aiName, DESTSPAWN] = %object;
			}
		}
	}
	//==============================================

	return %marker;
}

function RPG::isAiControlled(%clientId)
{
	dbecho($dbechoMode, "RPG::isAiControlled(" @ %clientId @ ")");

	if(fetchData(%clientId, "BotInfoAiName") != "" || fetchData(%clientId, "SpawnBotInfo") != "")
		return True;
	else
		return False;
}

//These are for the pets
function Pet::BeforeTurnEvil(%clientId)
{
	dbecho($dbechoMode, "Pet::BeforeTurnEvil(" @ %clientId @ ")");

	internalSay(%clientId, 0, "#say I'm starting to get enough of this...");
}
function Pet::TurnEvil(%clientId)
{
	dbecho($dbechoMode, "Pet::TurnEvil(" @ %clientId @ ")");

	internalSay(%clientId, 0, "#shout To hell with you all! Die!");

	storeData(%aiId, "botAttackMode", 1);
	AI::newDirectiveRemove(fetchData(%clientId, "BotInfoAiName"), 99);
	storeData(%aiId, "tmpbotdata", "");

	GameBase::setTeam(%clientId, 1);
}