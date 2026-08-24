// putting a global variable in the argument list means:
// if an argument is passed for that parameter it gets
// assigned to the global scope, not the scope of the function

function createTrainingServer()
{
   $SinglePlayer = true;
   createServer($pref::lastTrainingMission, false);
}

function remoteSetCLInfo(%clientId, %skin, %name, %email, %tribe, %url, %info, %autowp, %enterInv, %msgMask)
{
   $Client::info[%clientId, 0] = %skin;
   $Client::info[%clientId, 1] = %name;
   $Client::info[%clientId, 2] = %email;
   $Client::info[%clientId, 3] = %tribe;
   $Client::info[%clientId, 4] = %url;
   //$Client::info[%clientId, 5] = %info;
   if(%autowp)
      %clientId.autoWaypoint = true;
   if(%enterInv)
      %clientId.noEnterInventory = true;
   if(%msgMask != "")
      %clientId.messageFilter = %msgMask;
}

function Server::storeData()
{
   $ServerDataFile = "serverTempData" @ $Server::Port @ ".cs";

   export("Server::*", "temp\\" @ $ServerDataFile, False);
   export("pref::lastMission", "temp\\" @ $ServerDataFile, true);
   EvalSearchPath();
}

function Server::refreshData()
{
   exec($ServerDataFile);  // reload prefs.
   checkMasterTranslation();
   Server::nextMission(false);
}

function Server::onClientDisconnect(%clientId)
{
	// Need to kill the player off here to make everything
	// is cleaned up properly.
   %player = Client::getOwnedObject(%clientId);
   if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player)) {
		playNextAnim(%player);
	   Player::kill(%player);
	}

   Client::setControlObject(%clientId, -1);
   Client::leaveGame(%clientId);
   Game::CheckTourneyMatchStart();
   // NATIVE-EDITOR: never cycle the map out from under an editing session.
   // refreshData() ends in Server::nextMission(false), which loads
   // $nextMission[$missionName] -- the next map in ROTATION. ME::ReloadMission
   // begins with disconnect(), so the editor's own reload dropped the last
   // client, tripped this, and was overridden: relight Broadside, land in
   // BullsEye. Rotating an empty server makes sense in play; in the editor the
   // reload is already bringing the same map back.
   if(getNumClients() == 1 && !$EditingMission) // this is the last client.
      Server::refreshData();
}

function KickDaJackal(%clientId)
{
   Net::kick(%clientId, "The FBI has been notified.  You better buy a legit copy before they get to your house.");
}

function Server::onClientConnect(%clientId)
{
   if(!String::NCompare(Client::getTransportAddress(%clientId), "LOOPBACK", 8))
   {
      // force admin the loopback dude
      %clientId.isAdmin = true;
      %clientId.isSuperAdmin = true;
   }
   echo("CONNECT: " @ %clientId @ " \"" @ 
      escapeString(Client::getName(%clientId)) @ 
      "\" " @ Client::getTransportAddress(%clientId));

   if(Client::getName(%clientId) == "DaJackal")
      schedule("KickDaJackal(" @ %clientId @ ");", 20, %clientId);

   %clientId.noghost = true;
   %clientId.messageFilter = -1; // all messages
   remoteEval(%clientId, SVInfo, version(), $Server::Hostname, $modList, $Server::Info, $ItemFavoritesKey);
   remoteEval(%clientId, MODInfo, $MODInfo);
   remoteEval(%clientId, FileURL, $Server::FileURL);

   // clear out any client info:
   for(%i = 0; %i < 10; %i++)
      $Client::info[%clientId, %i] = "";

   Game::onPlayerConnected(%clientId);
}

function createServer(%mission, %dedicated)
{
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
      newObject(serverDelegate, FearCSDelegate, true, "IP", $Server::Port, "IPX", $Server::Port, "LOOPBACK", $Server::Port);
   
   exec(admin);
   exec(Marker);
   exec(Trigger);
   exec(NSound);
   exec(BaseExpData);
   exec(BaseDebrisData);
	exec(BaseProjData);
   exec(ArmorData);
   exec(Mission);
	exec(Item);
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

   // NATIVE-PORT (SpoonBot phase 1): this placement is load-bearing in BOTH directions --
   // AFTER exec(AI) above, so the payload's AI::* definitions win the last-wins addFunction
   // race (consoleInternal.cpp:695); and BEFORE preloadServerDataBlocks() below, because the
   // payload declares 5 datablocks (SoundBotRepairItem + 4x TreePoint*). Inert at 0.
   // NATIVE-EDITOR: no AI of any kind in an editing session ($EditingMission).
   if($Server::SpoonBots == 1 && $Server::BotBrain != 1 && !$EditingMission)
      exec("spoonbot\\spoonbot_load.cs");

   // NATIVE-EDITOR (thunderstorms): weather datablocks + storm loop. Placement
   // matters like the SpoonBot gate above: BEFORE preloadServerDataBlocks so
   // StormBolt/StormFlash/StormThunder register. Inert unless a mission places
   // a Storm marker (see weather.cs / registerUserObjects.cs).
   exec("weather.cs");

   // MECH-MAYHEM (generic mod hook): a mod's boot script may point
   // $Mod::ServerDataBlocks at a script declaring extra datablocks (weapons,
   // projectiles, armor twins). Placement is load-bearing both ways, like the
   // SpoonBot gate above: AFTER the stock datablock execs so mod overrides win
   // the last-wins race, and BEFORE preloadServerDataBlocks() below so the
   // declared datablocks actually register. exec() resolves through the mod
   // search path (isFile() would not). Inert when the variable is unset.
   if($Mod::ServerDataBlocks != "")
      exec($Mod::ServerDataBlocks);

   Server::storeData();

   // NOTE!! You must have declared all data blocks BEFORE you call
   // preloadServerDataBlocks.

   preloadServerDataBlocks();

   Server::loadMission( ($missionName = %mission), true );

   if(!%dedicated)
   {
      focusClient();

		if ($IRC::DisconnectInSim == "")
		{
			$IRC::DisconnectInSim = true;
		}
		if ($IRC::DisconnectInSim == true)
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
   if(%replay || $Server::TourneyMode)
      %nextMission = $missionName;
   else
      %nextMission = $nextMission[$missionName];
   echo("Changing to mission ", %nextMission, ".");
   // give the clients enough time to load up the victory screen
   Server::loadMission(%nextMission);
}

function remoteCycleMission(%clientId)
{
   if(%clientId.isAdmin)
   {
      messageAll(0, Client::getName(%playerId) @ " cycled the mission.");
      Server::nextMission();
   }
}

function remoteDataFinished(%clientId)
{
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
      schedule("Server::finishMissionLoad();", 18);
   else
      Server::finishMissionLoad();      
}

function Server::finishMissionLoad()
{
   $loadingMission = false;
	$TestMissionType = "";
   // instant off of the manager
   setInstantGroup(0);
   newObject(MissionCleanup, SimGroup);

   // NATIVE-EDITOR: per-mission datablocks generated by the editor's model
   // browser live in missions\<name>_shapes.cs. They MUST be declared before the
   // .mis is exec'd, because the .mis names them -- a StaticShape whose datablock
   // does not exist yet fails processArguments and the object is dropped.
   // captureMission() bundles this file with the map.
   %meShapes = "missions\\" @ $missionName @ "_shapes.cs";
   if(File::findFirst(%meShapes) != "")
      exec(%meShapes);

   exec($missionFile);
   Mission::init();
	Mission::reinitData();
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

   // NATIVE-PORT (apocalypse auto-start): the ONLY safe place to kick the bot harness from.
   // A +exec file runs before createServer, and Server::loadMission deletes and re-creates
   // ConsoleScheduler (:259/:268) which silently discards anything scheduled earlier -- so
   // scheduling from boot does not survive a mission change. This point is after the new
   // scheduler exists and after the mission is fully loaded, so it fires correctly on the first
   // mission AND on every change. Inert unless $Apoc::autoRun is set (config\apocalypse.cs).
   if($Apoc::autoRun == 1)
      schedule("Apoc::autoStart();", 5);

   // NATIVE-PORT (SpoonBot phase 2): same rationale as the Apoc hook above -- this is the only
   // point that survives a mission CHANGE, because Server::loadMission deletes and re-creates
   // ConsoleScheduler (:259/:268) and discards anything scheduled earlier. Phase 1 only DEFINED
   // the payload; this initialises it per mission. MissionStart self-gates on $matchStarted.
   if($Server::SpoonBots == 1 && $Server::BotBrain != 1 && !$EditingMission)
      schedule("SpoonBot::MissionStart();", 5);

   // NATIVE-EDITOR (thunderstorms): find placed Storm markers and start their
   // strike loops. Same placement rationale as the SpoonBot phase-2 hook: this
   // spot survives a mission change (ConsoleScheduler is recreated by
   // Server::loadMission, discarding anything scheduled earlier).
   schedule("Storm::MissionStart();", 6);

   return "True";
}

function Server::CheckMatchStarted()
{
   // if the match hasn't started yet, just reset the map
   // timing issue.
   if(!$matchStarted)
      Server::nextMission(true);
}

function Server::Countdown(%time)
{
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
   remoteEval(%clientId, "ITXT", %txt);
}

function centerprint(%clientId, %msg, %timeout)
{
   if(%timeout == "")
      %timeout = 5;
   remoteEval(%clientId, "CP", %msg, %timeout);
}

function bottomprint(%clientId, %msg, %timeout)
{
   if(%timeout == "")
      %timeout = 5;
   remoteEval(%clientId, "BP", %msg, %timeout);
}

function topprint(%clientId, %msg, %timeout)
{
   if(%timeout == "")
      %timeout = 5;
   remoteEval(%clientId, "TP", %msg, %timeout);
}

function centerprintall(%msg, %timeout)
{
   if(%timeout == "")
      %timeout = 5;
   for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
      remoteEval(%clientId, "CP", %msg, %timeout);
}

function bottomprintall(%msg, %timeout)
{
   if(%timeout == "")
      %timeout = 5;
   for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
      remoteEval(%clientId, "BP", %msg, %timeout);
}

function topprintall(%msg, %timeout)
{
   if(%timeout == "")
      %timeout = 5;
   for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
      remoteEval(%clientId, "TP", %msg, %timeout);
}


//This function is a placeholder+prevents possible console spam.
//By phantom: beatme101.com, tribesrpg.org
function remoteRawKey(%client, %key, %mod){
	client::sendmessage(%client, 0, "This server does not support the use of extra keybinds.");

	//Under normal conditions, %key will be one of the following:
	//Repack 4 and up:
	//"numpad0" - "numpad9", "numpadenter", "numpad+", "numpad-", "numpad*", "numpad/", "0"
	//Repack 6 adds:
	//"1" - "9" (only with %mod "alt" or "control"), "f1" - "f12" (only with %mod "")

	//Under normal conditions, %mod will be one of the following:
	//"", "control", "alt", "shift"

	//You shouldn't see "alt" and "numpadenter" together because that
	//toggles fullscreen, and thus isn't bound to this on the Tribes Repack.
	//If you decide to code something here, ensure that it can handle
	//anything a client might send to try to mess with the system.
	//See the current repack version's extra-controls.cs for a full list of
	//acceptable input, and note that this could be updated in the future.

}