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
	$Client::info[%clientId, 5] = %info;
	if(%autowp) %clientId.autoWaypoint = true;
	if(%enterInv) %clientId.noEnterInventory = true;
	if(%msgMask != "") %clientId.messageFilter = %msgMask;
}

function Server::storeData()
{
	$ServerDataFile = "serverTempData" @ $Server::Port @ ".cs";
	export("Server::*", "temp\\" @ $ServerDataFile, False);
	if (!$RandomMissions) export("pref::lastMission", "temp\\" @ $ServerDataFile, true);
	EvalSearchPath();
}

function Server::refreshData()
{
// reload prefs.
	exec($ServerDataFile);
	checkMasterTranslation();
	if ($RandomMissions) Server::loadMission(MissionList::nextRandom(), false);
	else Server::loadMission($pref::lastMission, false);
}

function Server::onClientDisconnect(%clientId)
{
	Client::setControlObject(%clientId, -1);
	Client::leaveGame(%clientId);
	Game::CheckTourneyMatchStart();
// this is the last client.
	if(getNumClients() == 1) Server::refreshData();
}

function AutoKick(%clientId)
{
	Net::kick(%clientId, "You have been banned from this server. Contact the admin if you don't know why.");
}

function KickDaJackal(%clientId)
{
	Net::kick(%clientId, "The FBI has been notified.You better buy a legit copy before they get to your house.");
}

function Server::onClientConnect(%clientId)
{
// force admin the loopback dude
	if(!String::NCompare(Client::getTransportAddress(%clientId), "LOOPBACK", 8))
	{
		%clientId.isAdmin = true;
		%clientId.isSuperAdmin = true;
	}
	if(Client::getName(%clientId) == "DaJackal") schedule("KickDaJackal(" @ %clientId @ ");", 20, %clientId);
// AutoAdmin and AutoKick stuff
	CheckTransportAddress(%clientId);
	CheckAutoAdmins(%clientId);
	CheckAutoBan(%clientId);
	CheckAutoMute(%clientId);
	echo("CONNECT: " @ %clientId @ " \"" @ escapeString(Client::getName(%clientId)) @ "\" " @ Client::getTransportAddress(%clientId));
	$Stats::ClientsConnected++;
	%clientId.num = $Stats::ClientsConnected;
	export("Stats::*", "config\\stats.cs", false);
	%clientId.noghost = true;
// all messages
	%clientId.messageFilter = -1;
	remoteEval(%clientId, SVInfo, version(), $Server::Hostname, $modList, $Server::Info, $ItemFavoritesKey);
// clear out any client info:
	for(%i = 0; %i < 10; %i++) $Client::info[%clientId, %i] = "";
	Game::onPlayerConnected(%clientId);
}

function createServer(%mission, %dedicated)
{
// Randomize
	$RandomSeed = 0;
	exec("random.cs");
	for (%i = 0; %i < $RandomSeed; %i++) %j = getRandom();
	$RandomSeed = floor(getRandom() * 1000);
	export("RandomSeed", "config\\random.cs", False);
	exec("stats.cs");
	$Stats::ResetCount++;
	export("Stats::*", "config\\stats.cs", false);
	$loadingMission = false;
	if(%mission == "")
	{
		if ($RandomMissions) %mission = MissionList::nextRandom();
		else %mission = $pref::lastMission;
	}
	if(%mission == "")
	{
		echo("Error: no mission provided.");
		return "False";
	}
	if(!$SinglePlayer) $pref::lastMission = %mission;
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
	if($SinglePlayer) newObject(serverDelegate, FearCSDelegate, true, "LOOPBACK", $Server::Port);
	else newObject(serverDelegate, FearCSDelegate, true, "IP", $Server::Port, "IPX", $Server::Port, "LOOPBACK", $Server::Port);
	exec(admin);
	exec(Marker);
	exec(Trigger);
	exec(NSound);
	exec(BaseExpData);
	exec(BaseDebrisData);
//	exec(Logging);
	exec(BaseProjData);
	exec(InventoryHeaders);
	exec(Item);
	exec(BaseAmmo);
	exec(Player);
	exec(StaticShape);
	exec(Station);// <<
	exec(Moveable);
	exec(Sensor);// <<
	exec(AI);
	exec(InteriorLight); 
	exec(Mission);
	exec(ArmorData);
	exec(Turret);
	exec(serverLink);
	exec(adminChangeMissionMenu);
	exec(adminCheckTeams);
	exec(adminCountVotes);
	exec(adminDistanceToTarget);
	exec(adminKick);
	exec(adminKillAllBots);
	exec(adminMenuRequest);
//	exec(adminMenuRequest2);
	exec(adminProcessMenuAAffirm);
	exec(adminProcessMenuAMAffirm);
	exec(adminProcessMenuBAffirm);
	exec(adminProcessMenuCMission);
	exec(adminProcessMenuCMType);
	exec(adminProcessMenuCTLimit);
	exec(adminProcessMenuDAffirm);
	exec(adminProcessMenuDMAffirm);
	exec(adminProcessMenuFPickTeam);
	exec(adminProcessMenuKAffirm);
	exec(adminProcessMenuHelpers);
	exec(adminProcessMenuOptions);
	exec(adminProcessMenuPickTeam);
	exec(adminProcessMenuRaces);
	exec(adminProcessMenuRAffirm);
	exec(adminProcessMenuSelBotAction);
	exec(adminProcessMenuSelBotGender);
	exec(adminProcessMenuRoamingBot);
	exec(adminProcessMenuBotSelect);
	exec(adminProcessMenuRemoveBot);
	exec(adminProcessMenuRBot);
	exec(adminProcessMenuBotAllDone);
	exec(adminProcessMenuVehicle);
	exec(adminProcessMenuWeapons);
	exec(adminRemoteAdminPassword);
	exec(adminRemoteSelectClient);
	exec(adminRemoteSetPassword);
	exec(adminRemoteSetTeamInfo);
	exec(adminRemoteSetTimeLimit);
	exec(adminRemoteVoteNo);
	exec(adminRemoteVoteYes);
	exec(adminSetModeFFA);
	exec(adminSetModeTourney);
	exec(adminSetTeamDamageEnable);
	exec(adminStartMatch);
	exec(adminStartVote);
	exec(adminTerminate);
	exec(adminVoteFailed);
	exec(adminVoteSucceded);
	ServerLink::Start();
	echo('>> Mission Items');
	exec(missionSpringPad);
	echo('>> Loads complete');
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
	if(%replay || $Server::TourneyMode) %nextMission = $missionName;
	else if ($RandomMissions) %nextMission = MissionList::nextRandom();
	else %nextMission = $nextMission[$missionName];
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
	if(%clientId.dataFinished) return;
	%clientId.dataFinished = true;
	Client::setDataFinished(%clientId);
// clear the data flag
	%clientId.svNoGhost = "";
	if($ghosting)
	{
// allow a CGA done from this dude
		%clientId.ghostDoneFlag = true;
// let the ghosting begin!
		startGhosting(%clientId);
	}
}

function remoteCGADone(%playerId)
{
	if(!%playerId.ghostDoneFlag || !$ghosting) return;
	%playerId.ghostDoneFlag = "";
	Game::initialMissionDrop(%playerid);
	if ($cdTrack != "") remoteEval (%playerId, setMusic, $cdTrack, $cdPlayMode);
	remoteEval(%playerId, MInfo, $missionName);
}

function Server::loadMission(%missionName, %immed)
{
	if($loadingMission) return;
	%missionFile = "missions\\" $+ %missionName $+ ".mis";
	if(File::FindFirst(%missionFile) == "")
	{
		if ($RandomMissions) %missionName = MissionList::nextRandom();
		else %missionName = $firstMission;
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
// deal with time imprecision
	resetSimTime();
	newObject(ConsoleScheduler, SimConsoleScheduler);
	if(!%immed) schedule("Server::finishMissionLoad();", 18);
	else Server::finishMissionLoad();
}

function Server::finishMissionLoad()
{
	$loadingMission = false;
	$TestMissionType = "";
// instant off of the manager
	setInstantGroup(0);
	newObject(MissionCleanup, SimGroup);
	exec($missionFile);
	Mission::init();
	Mission::reinitData();
	serverLink::InitializeMission();
	if($prevNumTeams != getNumTeams())
	{
// loop thru clients and setTeam to -1;
		messageAll(0, "New teamcount - resetting teams.");
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) GameBase::setTeam(%cl, -1);
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
	if($SinglePlayer) Game::startMatch();
	else if($Server::warmupTime && !$Server::TourneyMode) Server::Countdown($Server::warmupTime);
	else if(!$Server::TourneyMode) Game::startMatch();
	$teamplay = (getNumTeams() != 1);
	purgeResources(true);
// make sure the match happens within 5-10 hours.
	schedule("Server::CheckMatchStarted();", 3600);
	schedule("Server::nextMission();", 18000);
	return "True";
}

function Server::CheckMatchStarted()
{
// if the match hasn't started yet, just reset the map
// timing issue.
	if(!$matchStarted) Server::nextMission(true);
}

function Server::Countdown(%time)
{
	$countdownStarted = true;
	schedule("Game::startMatch();", %time);
	Game::notifyMatchStart(%time);
	if(%time > 30) schedule("Game::notifyMatchStart(30);", %time - 30);
	if(%time > 15) schedule("Game::notifyMatchStart(15);", %time - 15);
	if(%time > 10) schedule("Game::notifyMatchStart(10);", %time - 10);
	if(%time > 5) schedule("Game::notifyMatchStart(5);", %time - 5);
}

function Client::setInventoryText(%clientId, %txt)
{
	remoteEval(%clientId, "ITXT", %txt);
}

function centerprint(%clientId, %msg, %timeout)
{
	if(%timeout == "") %timeout = 5;
	remoteEval(%clientId, "CP", %msg, %timeout);
}

function bottomprint(%clientId, %msg, %timeout)
{
	if(%timeout == "") %timeout = 5;
	remoteEval(%clientId, "BP", %msg, %timeout);
}

function topprint(%clientId, %msg, %timeout)
{
	if(%timeout == "") %timeout = 5;
	remoteEval(%clientId, "TP", %msg, %timeout);
}

function centerprintall(%msg, %timeout)
{
	if(%timeout == "") %timeout = 5;
	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId)) remoteEval(%clientId, "CP", %msg, %timeout);
}

function bottomprintall(%msg, %timeout)
{
	if(%timeout == "") %timeout = 5;
	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId)) remoteEval(%clientId, "BP", %msg, %timeout);
}

function topprintall(%msg, %timeout)
{
	if(%timeout == "") %timeout = 5;
	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId)) remoteEval(%clientId, "TP", %msg, %timeout);
}

function CheckAutoAdmins(%clientid)
{
	%addr = Client::getTransportAddress(%clientId);
	for(%i=0; $Server::AutoAdmin[%i] != "" || $Server::AutoAdminAddr[%i] != "" ;%i++)
	{
		if(($Server::AutoAdmin[%i] == "" || $Server::AutoAdmin[%i] == Client::getName(%clientId)) && (String::findSubStr(%addr,$Server::AutoAdminAddr[%i]) == 0))
		{
			schedule("SayAutoAdmin(" @ %clientId @ ");", 30, %clientId);
			%clientId.isAdmin = true;
			if($Server::IsSuperAdmin[%i]) %clientId.isSuperAdmin = true;
		}
	}
}

function SayAutoAdmin(%clientid)
{
	TopPrint(%clientid,"<F1><jc>You have been Auto Adminned", 5);
}

function CheckAutoBan(%clientid)
{
	%addr = Client::getTransportAddress(%clientId);
	for(%i=0; $Server::AutoBan[%i] != "" && $Server::AutoBanAddr[%i] != "" ;%i++)
	{
		if($Server::AutoBan[%i] == Client::getName(%clientId))
		{
			schedule("KickClient(" @ %clientid @ ");",20,%clientid);
			return;
		}
		else if(String::findSubStr(%addr, $Server::AutoBanAddr[%i]) == 0)
		{
			schedule("KickClient(" @ %clientid @ ");",20,%clientid);
			return;
		}

	}
}

function CheckAutoMute(%clientid)
{
	%addr = Client::getTransportAddress(%clientId);
	for(%i=0; $Server::AutoMute[%i] != "" && $Server::AutoMuteAddr[%i] != "" ;%i++)
	{
		if($Server::AutoMute[%i] == Client::getName(%clientId))
		{
			%clientId.muteAll = True;
			return;
		}
		else if(String::findSubStr(%addr, $Server::AutoMuteAddr[%i]) == 0)
		{
			%clientId.muteAll = True;
			return;
		}
	}
}

function CheckTransportAddress(%clientid)
{
	%addr = Client::getTransportAddress(%clientId);
	if(String::getSubStr(%addr, 0, 8) == "LOOPBACK") return;
	if(String::getSubStr(%addr, 0, 3) != "IP:" && String::getSubStr(%addr, 0, 4) != "IPX:" )
	{
		schedule("KickClient(" @ %clientid @ ");",20,%clientid);
		return;
	}
}

function KickClient(%clientid)
{
	Net::kick(%clientid,"You are banned from this server");
}