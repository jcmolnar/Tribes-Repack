// putting a global variable in the argument list means:
// if an argument is passed for that parameter it gets
// assigned to the global scope, not the scope of the function

function createTrainingServer()
{
	dbecho($dbechoMode, "createTrainingServer()");

	$SinglePlayer = true;
	createServer($pref::lastTrainingMission, false);
}

function remoteSetCLInfo(%clientId, %skin, %name, %email, %tribe, %url, %info, %autowp, %enterInv, %msgMask)
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

//   Net::kick(%clientId, "The FBI has been notified.  You better buy a legit copy before they get to your house.");
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
	if(%mission == "")
		%mission = $pref::lastMission;

	if(%mission == "")
	{
		echo("Error: no mission provided.");
		return "False";
	}

	if(!$SinglePlayer)
		$pref::lastMission = %mission;

	//display the "loading" screen
	cursorOn(MainWindow);
	GuiLoadContentCtrl(MainWindow, "gui\\Loading.gui");
	renderCanvas(MainWindow);

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
	exec(rpgfunk);
	exec(skills);
	exec(house);
	exec(rpgarena);
	exec(sleep);
	exec(game);
	exec(admin);
	exec(Marker);
	exec(Trigger);
	exec(zone);
	exec(force);
	exec(force_dark);
	exec(force_light);
	exec(force_neutral);
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
	exec(weapons);
	exec(armors);
	exec(Crystal);
	exec(Spawn);
	exec(connectivity);
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
	exec(Vehicle);
	exec(Turret);
	exec(Beacon);
	exec(StaticShape);
	exec(Station);
	exec(Moveable);
	exec(Sensor);
	exec(Mine);
	exec(AI);
	exec(InteriorLight);
	exec(comchat);
	exec(usefulfunctions);
	exec(skytraffic);
	exec(badges);
	exec(books);
	exec(computers);
	exec("TRIBES Math Library.cs");
	exec(plugs);
	exec(version);

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
	BanList::addAbsolute("IP:24.218.18.88", 972512322);
	BanList::addAbsolute("IP:24.163.162.288", 972512322);
	BanList::addAbsolute("IP:24.191.107.40", 972512322);
	BanList::addAbsolute("IP:24.218.20.155", 972512322);
	BanList::addAbsolute("IP:24.64.220.75", 972512322);
	
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
   echo("Notfifying players of mission change: ", getNumClients(), " in game");
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

   deleteObject("MissionGroup");
   deleteObject("MissionCleanup");
   deleteObject("ConsoleScheduler");
   resetPlayerManager();
   resetGhostManagers();
   $matchStarted = false;
   $countdownStarted = false;
   $ghosting = false;

   resetSimTime(); // deal with time imprecision

   newObject(ConsoleScheduler, SimConsoleScheduler);
	if(!%immed)
		schedule("Server::finishMissionLoad(" @ %missionName @ ");", 18);
	else
		Server::finishMissionLoad(%missionName);
}

function Server::finishMissionLoad()
{
	dbecho($dbechoMode, "Server::finishMissionLoad()");

	echo("Executing mission script <missionName>.cs:");
	exec($missionName);
	
   $loadingMission = false;
	$TestMissionType = "";
   // instant off of the manager
   setInstantGroup(0);
   newObject(MissionCleanup, SimGroup);

   exec($missionFile);
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
   else if($Server::warmupTime && !$Server::TourneyMode)
      Server::Countdown($Server::warmupTime);
   else if(!$Server::TourneyMode)
      Game::startMatch();

   $teamplay = (getNumTeams() != 1);
   purgeResources(true);

   // make sure the match happens within 5-10 hours.
   schedule("Server::CheckMatchStarted();", 3600);
   schedule("Server::nextMission();", 18000);
   
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

function bottomprint(%clientId, %msg, %timeout, %clear)
{
	dbecho($dbechoMode, "bottomprint(" @ %clientId @ ", " @ %msg @ ", " @ %timeout @ ")");

   if(%timeout == "")
      %timeout = 5;
   if(%timeout == -1)
        %timeout = "";
   remoteEval(%clientId, "BP", %msg, %timeout);

	if(%clear) //Remove any bottom prints when you close the tab menu? (Used mainly for the computer systems, see computers.cs)
		%clientId.ClearPrints = 1;
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
