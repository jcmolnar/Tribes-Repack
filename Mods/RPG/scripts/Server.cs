//This file is part of Tribes RPG.
//Tribes RPG server side scripts
//Written by Jason "phantom" Daley,  Matthiew "JeremyIrons" Bouchard, and more (yet undetermined)

//	Copyright (C) 2019  Jason Daley

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

// putting a global variable in the argument list means:
// if an argument is passed for that parameter it gets
// assigned to the global scope, not the scope of the function

function dbecho(){}
function pecho(%m){	echo(String::getSubStr(%m, 0, 250));}

function rp_include(%file){
	if(!$fileLoaded[%file])
		exec(%file);
	$fileLoaded[%file] = True;
}

function createTrainingServer()
{
	dbecho($dbechoMode, "createTrainingServer()");

	$SinglePlayer = true;
	createServer($pref::lastTrainingMission, false);
}

function remoteSetCLInfo(%clientId, %skin, %name, %email, %tribe, %url, %info, %autowp, %enterInv, %msgMask, %rpv)
{
	dbecho($dbechoMode, "remoteSetCLInfo(" @ %clientId @ ", " @ %skin @ ", " @ %name @ ", " @ %email @ ", " @ %tribe @ ", " @ %url @ ", " @ %info @ ", " @ %autowp @ ", " @ %enterInv @ ", " @ %msgMask @ ")");

   $Client::info[%clientId, 0] = %skin;
   $Client::info[%clientId, 1] = %name;
   $Client::info[%clientId, 2] = %email;
   $Client::info[%clientId, 3] = %tribe;
   $Client::info[%clientId, 4] = %url;
   $Client::info[%clientId, 5] = %info;
   if(%autowp)
      %clientId.autoWaypoint = true;
   if(%enterInv)
      %clientId.noEnterInventory = true;
   if(%msgMask != "")
      %clientId.messageFilter = %msgMask;

	if(%rpv == ""){		if(%info == ""){//Reasonably certain they don't have it.			newKick(%clientId, "You must download RPG mod to play here. Download site: www.TribesRPG.org");		}		else{			remoteEval(%clientId, MODInfo, "Your copy of RPG may be out of date. Please visit www.TribesRPG.org to update.");		}	}	%flag = False;	%list = GetPlayerIdList();	for(%i = 0; (%id = GetWord(%list, %i)) != -1; %i++){		%n = rpg::getName(%id);		if(String::Compare(%n, %clname) == 0 && %id != %clientId){			%eflag = True;			if(string::compare(fetchData(%id,"Password"), %info) == 0){				%flag = True;				break;			}		}	}	if(%flag){
		newKick(%id);
		newKick(%clientId, "Ghosted player dropped, please reconnect.");	}	else if(%eflag)	{		//This only happens if the player connecting a duplicate name doesn't		//share a password with the existing player.		//Makes sense to give them a little time out for bad behaviour.		%hisip = Client::getTransportAddress(%clientId);		BanList::add(%hisip, 30);	}
}

function Server::storeData()
{
	dbecho($dbechoMode, "Server::storeData()");

   $ServerDataFile = "serverTempData" @ $Server::Port @ ".cs";

   export("Server::*", "temp\\" @ $ServerDataFile, False);
   export("pref::lastMission", "temp\\" @ $ServerDataFile, true);
   EvalSearchPath();
}

function Server::refreshData()
{
	dbecho($dbechoMode, "Server::refreshData()");

   exec($ServerDataFile);  // reload prefs.
   checkMasterTranslation();
   Server::loadMission($pref::lastMission, false);
}

function KickDaJackal(%clientId)
{
	dbecho($dbechoMode, "KickDaJackal(" @ %clientId @ ")");

//	newKick(%clientId, "The FBI has been notified.  You better buy a legit copy before they get to your house.");
}

function createServer(%mission, %dedicated)
{
	dbecho($dbechoMode2, "createServer(" @ %mission @ ", " @ %dedicated @ ")");

	deleteVariables("tmpBotGroup*");
	deleteVariables("aidirectiveTable*");
	deleteVariables("aiNumTable*");
	deleteVariables("tmpbotn*");
	deleteVariables("funk*");
	deleteVariables("Skill*");
	deleteVariables("world*");
	deleteVariables("Quest*");
	deleteVariables("loot*");
	deleteVariables("BotInfo*");
	deleteVariables("Merchant*");
	deleteVariables("NameForRace*");
	deleteVariables("BlockData*");
	deleteVariables("EventCommand*");
	deleteVariables("LoadOut*");
	$PetList = "";
	$DISlist = "";
	$SpawnPackList = "";
	$LoadOutList = "";
	$isRaining = "";

	$loadingMission = false;
	$ME::Loaded = false;
	exec(rpgserv);	if(%mission == "")		%mission = $pref::lastMission;	if(%mission == "")	{		%printlevel = $console::printlevel;		$console::printlevel = 1;		echo("Error: no mission provided.");		$console::printlevel = %printlevel;		return "False";	}

	%ms = String::GetSubStr(timestamp(), 20, 03);
	%ms = %ms * 4;	for(%i=0; %i <= %ms; %i++)	{		getrandom();	}	if(!$SinglePlayer)		$pref::lastMission = %mission;

	$MODInfo = "www.TribesRPG.org\n";
	if(!$dedicated){
		//display the "loading" screen
		cursorOn(MainWindow);
		GuiLoadContentCtrl(MainWindow, "gui\\Loading.gui");
		renderCanvas(MainWindow);
	}

	if(!%dedicated)
	{
		deleteServer();
		purgeResources();
		newServer();
		focusServer();
	}
	if($SinglePlayer)
		newObject(serverDelegate, FearCSDelegate, true, "LOOPBACK", $Server::Port);
	else
		newObject(serverDelegate, FearCSDelegate, true, "IP", $Server::Port, "IPX", 	$Server::Port, "LOOPBACK", $Server::Port);
   


	exec(globals);
	rp_include(strings);
	exec(rpgfunk);
	exec(charfunk);
	exec(connectivity);

	setWindowTitle("0/"@$server::maxplayers@" Tribes RPG server");

	exec(skills);
	exec(house);
	exec(rpgarena);
	exec(sleep);
	exec(game);
	exec(admin);
	exec(Marker);
	exec(Trigger);
	exec(zone);
	exec(spells);
	exec(classes);
	exec(party);
	exec(jail);
	exec(NSound);
	exec(BaseExpData);
	exec(BaseDebrisData);
	exec(BaseProjData);
	exec(ArmorData);
	exec(Mission);
	exec(Item);
	exec(Accessory);
	exec(smithing);
	exec(weapons);
	exec(armors);
	exec(Crystal);
	exec(Spawn);
	exec(gameevents);
	exec(shopping);
	exec(weight);
	exec(mana);
	exec(hp);
	exec(rpgstats);
	exec(playerdamage);
	exec(playerspawn);
	exec(itemevents);
	exec(economy);
	exec(remote);
	exec(weaponHandling);
	exec(BonusState);
	//exec(depbase);
	exec(ferry);
	exec(Player);
	//exec(Vehicle);
	//exec(Turret);
	exec(Beacon);
	exec(rpgStaticShape);

	%oldrpgmap["rpgmap1"] = True;
	%oldrpgmap["rpgmap5"] = True;
	if(!%oldrpgmap[%mission])
		exec(rpgStaticShapeVI);

	//exec(Station);
	//exec(Moveable);
	//exec(Sensor);
	exec(Mine);
	exec(AI);
	exec(InteriorLight);
	exec(comchat);
	//exec(plugs);
	exec(version);
	exec(bottalk);
	exec(belt);
	exec(compass);
	// NATIVE-PORT: test Car vehicle datablock (CarData). Must be exec'd here — after all other
	// datablock defs and BEFORE preloadServerDataBlocks() below — so it registers server-side and
	// ghosts to clients on connect (defining it after preload = "Invalid Packet" on join).
	exec("test_car.cs");
	$Server::Info = "Running RPG Mod 6.9 - www.tribesrpg.org\n" @ $extrainfo;
	$server::modinfo = $Server::Info;

	Server::storeData();

	// NOTE!! You must have declared all data blocks BEFORE you call
	// preloadServerDataBlocks.

	preloadServerDataBlocks();

	Server::loadMission( ($missionName = %mission), true );

	//**RPG

	CreateWeaponCyclingTables();

	LoadWorld();
	InitCrystals();
	InitZones();
	InitFerry();
	InitTownBots();
	if(!$NoSpawn)
		InitSpawnPoints();

	if($arenaOn)
	{
		if(!$NoSpawn)
			InitArena();
	}

	GenerateAllWeaponCosts();
	GenerateAllShieldCosts();
	GenerateAllArmorCosts();

	InitObjectives();

	//permanent banlist
	//BanList::addAbsolute("IP:24.218.18.88", 972512322);
	//BanList::addAbsolute("IP:24.163.162.288", 972512322);
	//BanList::addAbsolute("IP:24.191.107.40", 972512322);
	//BanList::addAbsolute("IP:24.218.20.155", 972512322);
	//BanList::addAbsolute("IP:24.64.220.75", 972512322);
	//BanList::addAbsolute("IP:24.141.193.213", 972512322);
	//BanList::addAbsolute("172.*", 972512322);
	BanList::addAbsolute("70.70.*", 972512322); // Surfer666, DOS attacks
	
	//**

	if(!%dedicated)
	{
		focusClient();

		if($IRC::DisconnectInSim == "")
		{
			$IRC::DisconnectInSim = true;
		}
		if($IRC::DisconnectInSim == true)
		{
			ircDisconnect();
			$IRCConnected = FALSE;
			$IRCJoinedRoom = FALSE;
		}
		// join up to the server
		$Server::Address = "LOOPBACK:" @ $Server::Port;
		$Server::JoinPassword = $Server::Password;
		connect($Server::Address);
	}

	schedule("help(True);",1);

	return "True";
}

function Server::nextMission(%replay)
{
	dbecho($dbechoMode, "Server::nextMission(" @ %replay @ ")");

//THERE! now it won't change mission!!!

//   if(%replay || $Server::TourneyMode)
//      %nextMission = $missionName;
//   else
//      %nextMission = $nextMission[$missionName];
//   echo("Changing to mission ", %nextMission, ".");
//   // give the clients enough time to load up the victory screen
//   Server::loadMission(%nextMission);
}

function remoteCycleMission(%clientId)
{
	dbecho($dbechoMode, "remoteCycleMission(" @ %clientId @ ")");

   if(%clientId.adminLevel >= 4)
   {
      messageAll(0, Client::getName(%playerId) @ " cycled the mission.");
      Server::nextMission();
   }
}

function remoteDataFinished(%clientId)
{
	dbecho($dbechoMode, "remoteDataFinished(" @ %clientId @ ")");

   if(%clientId.dataFinished)
      return;
   %clientId.dataFinished = true;
   Client::setDataFinished(%clientId);
   %clientId.svNoGhost = ""; // clear the data flag
   if($ghosting)
   {
      %clientId.ghostDoneFlag = true; // allow a CGA done from this dude
      startGhosting(%clientId);  // let the ghosting begin!
   }
}

function remoteCGADone(%playerId)
{
	dbecho($dbechoMode, "remoteCGADone(" @ %playerId @ ")");

   if(!%playerId.ghostDoneFlag || !$ghosting)
      return;
   %playerId.ghostDoneFlag = "";

   Game::initialMissionDrop(%playerid);

	if ($cdTrack != "")
		remoteEval (%playerId, setMusic, $cdTrack, $cdPlayMode);
   remoteEval(%playerId, MInfo, $missionName);
}

function Server::loadMission(%missionName, %immed)
{
	dbecho($dbechoMode, "Server::loadMission(" @ %missionName @ ", " @ %immed @ ")");

   if($loadingMission)
      return;

   %missionFile = "missions\\" $+ %missionName $+ ".mis";
   if(File::FindFirst(%missionFile) == "")
   {
      %missionName = $firstMission;
      %missionFile = "missions\\" $+ %missionName $+ ".mis";
      if(File::FindFirst(%missionFile) == "")
      {
         echo("invalid nextMission and firstMission...");
         echo("aborting mission load.");
         return;
      }
   }
   //echo("Notfifying players of mission change: ", getNumClients(), " in game");
   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
   {
      Client::setGuiMode(%cl, $GuiModeVictory);
      %cl.guiLock = true;
      %cl.nospawn = true;
      remoteEval(%cl, missionChangeNotify, %missionName);
   }

   $loadingMission = true;
   $missionName = %missionName;
   $missionFile = %missionFile;
   $prevNumTeams = getNumTeams();

	if(isObject("MissionGroup"))
		deleteObject("MissionGroup");
	if(isObject("MissionCleanup"))
		deleteObject("MissionCleanup");
	if(isObject("ConsoleScheduler"))
		deleteObject("ConsoleScheduler");
   resetPlayerManager();
   resetGhostManagers();
   $matchStarted = false;
   $countdownStarted = false;
   $ghosting = false;

   resetSimTime(); // deal with time imprecision

   newObject(ConsoleScheduler, SimConsoleScheduler);
   if(!%immed)
      schedule("Server::finishMissionLoad();", 18);
   else
      Server::finishMissionLoad();      
}

function Server::finishMissionLoad()
{
	dbecho($dbechoMode, "Server::finishMissionLoad()");

	$loadingMission = false;
	$TestMissionType = "";
	// instant off of the manager
	setInstantGroup(0);
	newObject(MissionCleanup, SimGroup);

	exec($missionFile);


	$END_OF_MAP = newObject("EndOfMap", SimGroup);
	//Don't modify $END_OF_MAP in your scripts.
	//it is used by saveworld and should never change after this point.


	if($END_OF_MAP > 1)
		Mission::init();
   if($prevNumTeams != getNumTeams())
   {
      // loop thru clients and setTeam to -1;
      messageAll(0, "New teamcount - resetting teams.");
      for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
         GameBase::setTeam(%cl, -1);
   }

   $ghosting = true;
   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
   {
      if(!%cl.svNoGhost)
      {
         %cl.ghostDoneFlag = true;
         startGhosting(%cl);
      }
   }
   if($SinglePlayer)
      Game::startMatch();
   //else if($Server::warmupTime && !$Server::TourneyMode)
   //   Server::Countdown($Server::warmupTime);
   //else if(!$Server::TourneyMode)
      Game::startMatch();

   $teamplay = (getNumTeams() != 1);
   purgeResources(true);

   // make sure the match happens within 5-10 hours.
   //schedule("Server::CheckMatchStarted();", 3600);
   //schedule("Server::nextMission();", 18000);
   
   return "True";
}

function Server::CheckMatchStarted()
{
	dbecho($dbechoMode, "Server::CheckMatchStarted()");

   // if the match hasn't started yet, just reset the map
   // timing issue.
   if(!$matchStarted)
      Server::nextMission(true);
}

function Server::Countdown(%time)
{
	dbecho($dbechoMode, "Server::Countdown(" @ %time @ ")");

   $countdownStarted = true;
   schedule("Game::startMatch();", %time);
   Game::notifyMatchStart(%time);
   if(%time > 30)
      schedule("Game::notifyMatchStart(30);", %time - 30);
   if(%time > 15)
      schedule("Game::notifyMatchStart(15);", %time - 15);
   if(%time > 10)
      schedule("Game::notifyMatchStart(10);", %time - 10);
   if(%time > 5)
      schedule("Game::notifyMatchStart(5);", %time - 5);
}

function Client::setInventoryText(%clientId, %txt)
{
	dbecho($dbechoMode, "Client::setInventoryText(" @ %clientId @ ", " @ %txt @ ")");

	remoteEval(%clientId, "ITXT", %txt);
}

function centerprint(%clientId, %msg, %timeout)
{
	dbecho($dbechoMode, "centerprint(" @ %clientId @ ", " @ %msg @ ", " @ %timeout @ ")");

   if(%timeout == "")
      %timeout = 5;
   if(%timeout == -1)
        %timeout = "";
   remoteEval(%clientId, "CP", %msg, %timeout);
}

function bottomprint(%clientId, %msg, %timeout)
{
	dbecho($dbechoMode, "bottomprint(" @ %clientId @ ", " @ %msg @ ", " @ %timeout @ ")");

   if(%timeout == "")
      %timeout = 5;
   if(%timeout == -1)
        %timeout = "";
   remoteEval(%clientId, "BP", %msg, %timeout);
}

function topprint(%clientId, %msg, %timeout)
{
	dbecho($dbechoMode, "topprint(" @ %clientId @ ", " @ %msg @ ", " @ %timeout @ ")");

   if(%timeout == "")
      %timeout = 5;
   if(%timeout == -1)
        %timeout = "";
   remoteEval(%clientId, "TP", %msg, %timeout);
}

function centerprintall(%msg, %timeout)
{
	dbecho($dbechoMode, "centerprintall(" @ %msg @ ", " @ %timeout @ ")");

   if(%timeout == "")
      %timeout = 5;
   if(%timeout == -1)
        %timeout = "";
   for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
      remoteEval(%clientId, "CP", %msg, %timeout);
}

function bottomprintall(%msg, %timeout)
{
	dbecho($dbechoMode, "bottomprintall(" @ %msg @ ", " @ %timeout @ ")");

   if(%timeout == "")
      %timeout = 5;
   if(%timeout == -1)
        %timeout = "";
   for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
      remoteEval(%clientId, "BP", %msg, %timeout);
}

function topprintall(%msg, %timeout)
{
	dbecho($dbechoMode, "topprintall(" @ %msg @ ", " @ %timeout @ ")");

   if(%timeout == "")
      %timeout = 5;
   if(%timeout == -1)
        %timeout = "";
   for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
      remoteEval(%clientId, "TP", %msg, %timeout);
}


function Schedule::Add( %eval, %time, %tag ) {	if ( %tag == "" )		%tag = %eval;	if(String::findSubStr(%tag, "\"") != -1 || String::findSubStr(%tag, "\\") != -1){		pecho("%tag malformed: "@%tag);		return;	}	$Schedule::id[%tag]++;	$Schedule::eval[%tag] = %eval;		schedule( "Schedule::Exec(\""@%tag@"\", "@$Schedule::ID[%tag]@");", %time );}function Schedule::Exec( %tag, %id ) {	if ( $Schedule::ID[%tag] != %id )		return;	%eval = $Schedule::eval[%tag];	Schedule::Cancel(%tag);	eval(%eval);}function Schedule::Cancel( %tag ) {	if($Schedule::ID[%tag] > 900000)		$Schedule::ID[%tag] = 0;	else		$Schedule::ID[%tag]++;	$Schedule::eval[%tag] = "";}function Schedule::Check( %tag ) {	if( $Schedule::eval[%tag] != "" )		return true;	else		return false;}

$round2plugin = False;
if(round2(2) != False) $round2plugin = True;

function FixDecimals(%c)
{
	dbecho($dbechoMode, "FixDecimals(" @ %c @ ")");
	if($round2plugin){//Are we running on the math plugins?
		%val = round2(%c*10);
		if(%val < 10)
			return "0." @ %val;
		return String::getSubStr(%val, 0, String::len(%val)-1) @ "." @ %val%10;
	}


	%d = round(%c * 10);
	%m = (%d / 10) * 1.000001;

	return %m;
}

function safePosition(%obj, %pos, %caller){	if(string::findsubstr(%pos, "N") > -1){		pecho(%caller@" just tried to set pos "@%pos);		%x = floor(getWord(%pos, 0));		%y = floor(getWord(%pos, 1));		%z = floor(getWord(%pos, 2));		%pos = %x@" "@%y@" "@%z;	}	gamebase::setposition(%obj, %pos);}
function safeDelete(%id, %caller, %instant){	if(%id < 700){		pecho("Failure 1 to safeDelete object "@%id@", called by "@%caller);		return false;	}	if(!isObject(%id)){		pecho("Failure 2 to safeDelete object "@%id@", called by "@%caller);		return false;	}	if(%instant)		deleteObject(%id);	else		schedule("finalDelete("@%id@",\"escapeString(%caller)\");",0.01,%id);	return true;}
function finalDelete(%id, %caller){	if(!isObject(%id)){		pecho("Failure 2 to finalDelete object "@%id@", called by "@%caller);		return false;	}	deleteObject(%id);	return true;}
// for player rotations only (around z axis) -plasmatic function rotateVector(%vec,%rot){	%pi = 3.1416;	%rot3= getWord(%rot,2);	for(%i = 0; %rot3 >= %pi*2; %i++) %rot3 = %rot3 - %pi*2;	if (%rot3 > %pi) %rot3 = %rot3 - %pi*2;	%vec1= getWord(%vec,0);	%vec2= getWord(%vec,1);	%vc = %vec2;	%vec3= getWord(%vec,2); 	%ray = %vec1;		%vec1 = %ray*cos(%rot3);	%vec2 = %ray*sin(%rot3);	%vec = %vec1 @" "@ %vec2 @" "@ %vec3;	%vec = Vector::add(%vec,Vector::getFromRot(%rot,%vc,0));	return %vec;}