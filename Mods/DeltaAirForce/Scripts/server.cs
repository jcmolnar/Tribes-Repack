// putting a global variable in the argument list means:
// if an argument is passed for that parameter it gets
// assigned to the global scope, not the scope of the function

// DELTAFORCE
$MODInfo = "<jc><f0>||| DeltaAirForce ||| \n <f1>The offical DAF website is up at <f2>http://jmods.bravepages.com\n <f1>Download mappacks, releases, and any updates there.\nDownload Clientpack at above site.\nIf you DLed DAFCP before 10/23, then DL again for bug fix.";
$VIPText = "The VIP has escaped!";

$Server::SmokePuffs = 8;
$Server::SniperLimit = 0.2;

$hostageGame = 0;
$prevVal = $Server::AutoAssignTeams;
$HuntedQueue = 0;
$Hunted = 0;

function getTeamNumPlayers(%team) {
	%num = 0;
	for (%i = Client::getFirst(); %i != -1; %i = Client::getNext(%i)) {
		if (Client::getTeam(%i) == %team) {
			%num++;
		}
	}
	return %num;
}
// END DELTAFORCE

function clearscreen()
{
	for(%i = 0; %i< 80; %i++)
		echo();
}

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
		remoteEval(%clientId,onClientLeaving);

		ClearComm(%clientId, true);
	ClearFromComm(%clientId, true);

	// DELTAFORCE
	if($Game::missionType == "Assassination") {
	   if($Hunted == %clientId) {
			$Hunted = 0;
			if ($HuntedQueue == 0) {
				for (%h = Client::getFirst(); %h != -1; %h = Client::getNext(%h)) 
				{
					if((GameBase::getTeam(%h) == 0) && (%h != %clientId))
						break;
				}
				if(%h != -1)
				{
					$Hunted = %h;
					Player::kill(%h);
					schedule("centerprint("@%h@", \"<jc><f0>You are now the VIP!\", 4);", 0.5);
				}
			} else {
				%h = $HuntedList[0];
				for (%i = 0; %i < $HuntedQueue-1; %i++) {
					$HuntedList[%i] = $HuntedList[%i+1];
					if($HuntedList[%i])
						schedule("bottomprint("@$HuntedList[%i]@", \"<jc><f0>You are #"@(%i+1)@" in line to be the VIP.\", 4);", 0.5);
				}
				$HuntedQueue--;
				$Hunted = %h;
				Player::kill(%h);
				GameBase::setTeam(%h, 0);
				schedule("centerprint("@%h@", \"<jc><f0>You are now the VIP!\", 4);", 0.5);
			}
   	   } else       // Take a disconnecting player out of the VIP queue
	   {
			for(%i = 0; %i < $HuntedQueue; %i++)
			{
				if($HuntedList[%i] == %clientId)
				{
					for(%c = %i; %c < $HuntedQueue-1; %c++)
						$HuntedList[%c] = $HuntedList[%c+1];
					
					$HuntedQueue--;
				}
			}
	   }
   } else if($hostageGame || $Game::missionType == "Hostage") {
		%team = GameBase::getTeam(%clientId);
		for(%x = 0; %x < 3; %x++) {
			if($Hostages[%clientId, %x] > 0) {
				GameBase::setTeam($Hostages[%clientId, %x], getNumTeams - 1);
				AI::DirectiveFollow($Hostages[%clientId, %x], -1, 0, 1024);
				$HostageOwner[$Hostages[%clientId, %x]] = -1;
				$Hostages[%clientId, %x] = 0;
				$deltaTeamScore[%team] -= 5;
				$TakenHostages[%clientId]--;
			}
		}
   }
   // END DELTAFORCE

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
   if(getNumClients() == 1) // this is the last client.
      Server::refreshData();
}

function KickDaJackal(%clientId)
{
   Net::kick(%clientId, "The FBI has been notified.  You better buy a legit copy before they get to your house.");
}

function KickTheFucker(%clientId)
{
   %ip = Client::getTransportAddress(%clientId);
   BanList::add(%ip, 1800);
   BanList::export("config\\banlist.cs");
   Net::kick(%clientId, "You Have been banned. Go away.");
}

function KickTheBigot(%clientId)
{
   %ip = Client::getTransportAddress(%clientId);
   BanList::add(%ip, 1800);
   BanList::export("config\\banlist.cs");
   Net::kick(%clientId, "You have been banned for the use of derogatory language. Email Deathknight@hehe.com to beg for your rights to play again, and/or more details.");
}

for(%x=0; %x < 50; %x++)
{
	$AddressFound[%x] = "0";
}
for(%x=0; %x < 25; %x++)
{
	$AddressFoundTwo[%x] = "0";
}
for(%x=0; %x < 10; %x++)
{
	$AddressFoundThree[%x] = "0";
}

function getNextAdNumber()
{
	for(%x=0; %x < 50; %x++){
		if($AddressFound[%x] == "0" )
			return %x;
	}
	
}
function getNextAdNumberTwo()
{
	for(%x=0; %x < 25; %x++){
		if($AddressFoundTwo[%x] == "0" )
			return %x;
	}
	
}
function getNextAdNumberThree()
{
	for(%x=0; %x < 10; %x++){
		if($AddressFoundThree[%x] == "0" )
			return %x;
	}
	
}

function displayAddress()
{
	echo();
	for(%x = 0; %x < 50; %x++){
		
		if($AddressFound[%x] !=	"0" ){
			%found = 1;	
			echo($AddressFound[%x]);
		}
	}
	echo();
	for(%y = 0; %y < 25; %y++){
		
		if($AddressFoundTwo[%y] !=	"0" ){
			%found = 1;	
			echo($AddressFoundTwo[%y]);
		}
	}
	echo();
	for(%z = 0; %z < 10; %z++){
		
		if($AddressFoundThree[%z] !=	"0" ){
			%found = 1;	
			echo($AddressFoundThree[%z]);
		}
	}

	if(%found != 1)
		echo("None found");
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


//   if( Client::getName(%clientId) == "=SoK=AgentOrange" || (String::findSubStr(Client::getTransportAddress(%clientId),"IP:12.251.76.254") == 0) 
//	||  Client::getName(%clientId) == "Sarah" || (String::findSubStr(Client::getTransportAddress(%clientId),"IP:205.251.180.73") == 0)
//	||  Client::getName(%clientId) == "|ATS|SilentScope" || (String::findSubStr(Client::getTransportAddress(%clientId),"IP:216.119") == 0)
//	||  Client::getName(%clientId) == "bloodychaos" || (String::findSubStr(Client::getTransportAddress(%clientId),"IP:206.148.120.66") == 0)
//	||  Client::getName(%clientId) == "Mercenary" || (String::findSubStr(Client::getTransportAddress(%clientId),"IP:207.41.77.89") == 0)
//	||  Client::getName(%clientId) == "Outlaw" || (String::findSubStr(Client::getTransportAddress(%clientId),"IP:24.79.135.212") == 0)
//	||  Client::getName(%clientId) == "Zero" || Client::getName(%clientId) == "Z e r o" ||  Client::getName(%clientId) == "Bob the Corn Cob" ||  Client::getName(%clientId) == "DaTacoDatFlies" ||  Client::getName(%clientId) == "ZeroFuryBones" || (String::findSubStr(Client::getTransportAddress(%clientId),"IP:24.117") == 0)  || (String::findSubStr(Client::getTransportAddress(%clientId),"IP:65.145") == 0) 
//	||  Client::getName(%clientId) == "[101st] Folin" ||  Client::getName(%clientId) == "Soulus Raven" ||  Client::getName(%clientId) == "Raven"  
//	||  Client::getName(%clientId) == "Ralinea" || (String::findSubStr(Client::getTransportAddress(%clientId),"IP:68.102.45.169") == 0)  
//	||  Client::getName(%clientId) == "moose" || (String::findSubStr(Client::getTransportAddress(%clientId),"IP:209.92.218.64") == 0)  )
//	Schedule("KickTheBigot(" @ %clientId @ ");", 5, %clientId);	



   %clientId.noghost = true;
   %clientId.messageFilter = -1; // all messages
   Game::InitializeObjectives(%clientID,$ObjectiveList, $Game::missionType);
   remoteEval(%clientId, SVInfo, version(), $Server::Hostname, $modList, $Server::Info, $ItemFavoritesKey);
   remoteEval(%clientId, MODInfo, $MODInfo);
   remoteEval(%clientId, FileURL, $Server::FileURL);
   
  
//   if(Client::getName(%clientId) == "Jesus" && String::findSubStr(Client::getTransportAddress(%clientId),"IP:192.168.1.") == 0){
//      schedule("bottomprint(" @ %clientId @ ", \"<jc><f1><jc><f2>Welcome Jesus!.\", 5);", 35);
//      %clientId.isAdmin = true;
//      %clientId.isSuperAdmin = true;   
//   } else if(String::findSubStr(Client::getName(%clientId), "Jesus") != -1)
//	net::Kick(%clientId, "Riiight");
//   if(String::findSubStr(Client::getTransportAddress(%clientId),"IP:144.80.105.106") == 0){
//      schedule("bottomprint(" @ %clientId @ ", \"<jc><f1><jc><f2>Welcome Administrator!.\", 5);", 35);
//      %clientId.isAdmin = true; 
//   }
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
// DELTA FORCE
//	if (version() < 1.8) {
//		echo("Foolish mortal! You are attempting to run the DeltaForce mod without having upgraded to TRIBES version 1.8 or later!");
//		echo("                       You are currently only running version ", version(), ".");
//		echo("      Upgrade immediately by going to http://www.tribesplayers.com and downloading the latest patch.");
//		quit();
//	}
// END DELTA FORCE
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
	exec("HAPPYNESS.cs");
   IncreaseHappyness(10);
   
   Server::storeData();

   // NOTE!! You must have declared all data blocks BEFORE you call
   // preloadServerDataBlocks.

   preloadServerDataBlocks();

   Server::loadMission( ($missionName = %mission), true );

   // DELTAFORCE
   if($Game::missionType == "Hostage") {
	$hostageGame = 1;
	$prevVal = $Server::AutoAssignTeams;
   	$Server::AutoAssignTeams = "False";
   } else if($Game::missionType == "Assassination") {
    $prevVal = $Server::AutoAssignTeams;
   	$Server::AutoAssignTeams = "False";
	} else {
	$hostageGame = 0;
	$Server::AutoAssignTeams = $prevVal;
   }
   // END DELTAFORCE

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
   // DELTAFORCE
   exec(misrestart);
   // END DELTAFORCE  
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

function remoteCheckDAF( %client )
{
	remoteEval(%client, "DAFAccepted");

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

function AdminTalk(%msg)
{
	echo(%msg);
	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
		remoteEval(%clientId, "BP", "Admin: " @ %msg, 6);
}


function Listplayers()
{
	%num = 0;
	for(%i = Client::getFirst(); %i != -1; %i = Client::getNext(%i))
	{
		%num++;
		%display = %i @ ": " @ Client::getname(%i) @ " \tat " @ Client::getTransportAddress(%i);
		%display = %display @ " \tTeam: " @ Client::getTeam(%i) @ " TKS: " @ $TKCount[%i, Client::getName(%i)] @ " SC: " @ %i.score;
		Echo(%display);

	}
	echo("Happyness at: " @ $HAPPYNESS @  "%" );
	echo(%Num  @ " Players");
}

function DecHappyness()
{

	DecreaseHappyness(5);
	Export( "HAPPYNESS*", "config\\HAPPYNESS.cs", False );
	
	schedule("DecHappyness();", 900);
}

function DecreaseHappyness(%amt)
{
	if(!%amt)
		$Happyness--;
	else
		$Happyness = $Happyness - %amt;
	
	if($HAPPYNESS < 0)
		$HAPPYNESS = 0;
	Export( "HAPPYNESS*", "config\\HAPPYNESS.cs", False );
	
}

function IncreaseHappyness(%amt)
{
	if(!%amt)
		$Happyness++;
	else
		$Happyness = $Happyness + %amt;
	
	if($HAPPYNESS > 100)
		$HAPPYNESS = 100;
	Export( "HAPPYNESS*", "config\\HAPPYNESS.cs", False );
	
}

function Ban(%client, %message) 
{
	%ip = Client::getTransportAddress(%clientId);
	BanList::add(%ip, 1800);
	BanList::export("config\\banlist.cs");
	net::kick(%client, %message);
}
