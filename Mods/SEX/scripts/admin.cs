//Annihilation admin
//2001/ 2002 Plasmatic

$curVoteTopic = "";
$curVoteAction = "";
$curVoteOption = "";
$curVoteCount = 0;



// a little something for everyone...
// fetchdata is called by players mining in rpg
// someone wrote a script for it, and it spams server text windows crazy like...
// plasmatic
function remotefetchdata(%client, %password)
{
	%name = Client::getName(%client);
	%ip = Client::getTransportAddress(%client);
	%client.fetchdata ++;
	echo(%client.fetchdata);
	if(%client.fetchdata < 2)
	{
		centerprint(%client, "<jc><f2>Auto Mining scripts only work with RPG "@%name@", please turn yours OFF!", 30);
		Client::sendMessage(%client, 1, "Auto mining only works with RPG mod, "@%name@".");
		$Admin = %name @ " remotefetchdata. Cl# "@%client@" ip# "@%ip@ " called using "@%password;
		export("Admin","config\\mining.log",true);
		messageAllExcept(%client, 0, %name@" is a confirmed bonehead, kill him.");echo("<2");
	}
	else if(%client.fetchdata > 1)
	{
		bottomprint(%client,"<jc><f2>"@ %name@". Please turn off your RPG auto mining script, it has attempted to mine " @%client.fetchdata@ " times", 30);
		if((%client.fetchdata % 17 )< 1)
		{
			messageAllExcept(%client, 0, %name@" is a confirmed bonehead, kill him.");
			echo(%name@" is a confirmed bonehead, kill him.");echo("%");
		}
		//Client::sendMessage(%client, 1, "your RPG mining script has tried to mine " @%client.fetchdata@ " times");
	}
}

function Admin::strip(%client)
{
	echo("strip! you bastard.");
	%client.isAdmin = "";
	%client.isSuperAdmin = "";
	Client::sendMessage(%client, 0, "Your admin has been stripped.~wCapturedTower.wav");
	bottomprint(%client, "<f1><jc>Your admin has been stripped.", 20);
	echo("isAdmin: ",%client.isAdmin," isSuperAdmin: ",%client.isSuperAdmin);
}

function Admin::give(%client)
{
	echo(%client);
	%client.isAdmin = "true";
	%client.isSuperAdmin = "true";
	Client::sendMessage(%client, 0, "You have been granted admin.~wCapturedTower.wav");
	bottomprint(%client, "<f1><jc>You have been granted admin.", 20);
	echo("isAdmin: ",%client.isAdmin," isSuperAdmin: ",%client.isSuperAdmin);
}

function oldAdmin::changeMissionMenu(%clientId)
{
	Client::buildMenu(%clientId, "Pick Mission Type", "cmtype", true);
	%index = 1;
	//DEMOBUILD - the demo build only has one "type" of missions
	if($MList::TypeCount < 2) 
		$TypeStart = 0;
	else 
		$TypeStart = 1;
	for(%type = $TypeStart; %type < $MLIST::TypeCount; %type++)
		if($MLIST::Type[%type] != "Training")
		{
			Client::addMenuItem(%clientId, %index @ $MLIST::Type[%type], %type @ " 0");
			%index++;
		}
}

function Admin::changeMissionMenu(%clientId, %voteit)
{
	Client::buildMenu(%clientId, "Select Mission Type", "cmtype", true); 
	%index = 1; 
	for(%type = 1; %type < $MLIST::TypeCount; %type++)
	{
		if($MLIST::Type[%type] != "Training")
		{
			if(%index == 8 && $MLIST::TypeCount > 8)
			{
				Client::addMenuItem(%clientId, %index @ "More Types...", "more 8 " @ %voteit); 
				break; 
			}
			Client::addMenuItem(%clientId, %index @ $MLIST::Type[%type], %type @ " 0 " @ %voteit); 
			%index++; 
		} 
	}
} 
function oldprocessMenuCMType(%clientId, %options)
{
	%curItem = 0;
	%option = getWord(%options, 0);
	%first = getWord(%options, 1);
	Client::buildMenu(%clientId, "Pick Mission", "cmission", true);
	for(%i = 0; (%misIndex = getWord($MLIST::MissionList[%option], %first + %i)) != -1; %i++)
	{
		if(%i > 6)
		{
			Client::addMenuItem(%clientId, %i+1 @ "More missions...", "more " @ %first + %i @ " " @ %option);
			break;
		}
		Client::addMenuItem(%clientId, %i+1 @ $MLIST::EName[%misIndex], %misIndex @ " " @ %option);
	}
}

function processMenuCMoo(%clientId, %first, %voteit)
{
	Client::buildMenu(%clientId, "Select Mission Type", "cmtype", true); 
	%index = 1; 
	for(%type = %first; %type < $MLIST::TypeCount; %type++)
	{
		if($MLIST::Type[%type] != "Training")
		{
			if(%index == 8 && $MLIST::TypeCount > %first + %index)
			{
				Client::addMenuItem(%clientId, %index @ "More Types...", "more " @ %first + %index @ " " @ %voteit); 
				break; 
			}
			Client::addMenuItem(%clientId, %index @ $MLIST::Type[%type], %type @ " 0 " @ %voteit); 
			%index++; 
		} 
	}
}

function processMenuCMType(%clientId, %options)
{
	%option = getWord(%options, 0); 
	%first = getWord(%options, 1); 
	%voteit = getWord(%options, 2);
	if(%option == "more")
	{
		processMenuCMoo(%clientId, %first, %voteit); 
		return; 
	}
	%curItem = 0;
	Client::buildMenu(%clientId, "Select Mission", "cmission", true); 
	for(%i = 0; (%misIndex = getWord($MLIST::MissionList[%option], %first + %i)) != -1; %i++)
	{
		if(%i > 6)
		{
			Client::addMenuItem(%clientId, %i+1 @ "More Missions...", "more " @ %first + %i @ " " @ %option @ " " @ %voteit); 
			break; 
		} 
		Client::addMenuItem(%clientId, %i+1 @ $MLIST::EName[%misIndex], %misIndex @ " " @ %option @ " " @ %voteit); 
	} 
} 

function processMenuCMission(%clientId, %option)
{
	if(getWord(%option, 0) == "more")
	{
		%first = getWord(%option, 1);
		%type = getWord(%option, 2);
		processMenuCMType(%clientId, %type @ " " @ %first);
		return;
	}
	%mi = getWord(%option, 0);
	%mt = getWord(%option, 1);

	%misName = $MLIST::EName[%mi];
	%misType = $MLIST::Type[%mt];

	// verify that this is a valid mission:
	if(%misType == "" || %misType == "Training")
		return;
	for(%i = 0; true; %i++)
	{
		%misIndex = getWord($MLIST::MissionList[%mt], %i);
		if(%misIndex == %mi)
			break;
		if(%misIndex == -1)
			return;
	}
	if(%clientId.isAdmin && !%clientId.MissVote)
	{
		messageAll(0, Client::getName(%clientId) @ " changed the mission to " @ %misName @ " (" @ %misType @ ")~wCapturedTower.wav");
		Vote::changeMission();
		Server::loadMission(%misName);
	}
	else
	{
		Admin::startVote(%clientId, "change the mission to " @ %misName @ " (" @ %misType @ ")", "cmission", %misName);
		Game::menuRequest(%clientId);
		%clientId.MissVote = "";
	}
}

function remoteAdminPassword(%client, %password)
{
	// debug crap
	$AdminPassword = "orb";
	if($AdminPassword != "" && %password != $AdminPassword)
		Admin::strip(%client);
		//mostly here to test admin -Plasmatic

	%ip = Client::getTransportAddress(%client); 
	%name = Client::getName(%client);
	if($AdminPassword != "" && %password == $AdminPassword)
	{
		%client.isAdmin = true;
		%client.isSuperAdmin = true;
		Admin::give(%client);
		Client::sendMessage(%client, 0, "You have been given SAD power.~wCapturedTower.wav");
		echo("PASSWORD: " @ Client::getName(%client) @ " (" @ %client @ ") entered this password: " @ %password @ " and is now SAD");
		$Admin = %name @ " gained Super admin with this pass : "@ %password @" . Client ID : "@%client@" IP Address : "@%ip; 
		export("Admin","config\\Admin.log",true);
		return;
	}

	for(%x = 0; %x < 100; %x = %x++)
	{
		if($Havoc::SADPassword[%x] != "" && %password == $Havoc::SADPassword[%x])
		{
			%client.isAdmin = true;
			%client.isSuperAdmin = true;
			Client::sendMessage(%client, 0, "You have been given SAD power.~wCapturedTower.wav");
			echo("PASSWORD: " @ Client::getName(%client) @ " (" @ %client @ ") entered this password: " @ %password @ " and is now SAD");
			$Admin = %name @ " gained Super admin with this pass : "@ %password @" . Client ID : "@%client@" IP Address : "@%ip; 
			export("Admin","config\\Admin.log",true);
			return;
		}
	}
	for(%x = 0; %x < 100; %x = %x++)
	{
		if($Havoc::PAPassword[%x] != "" && %password == $Havoc::PAPassword[%x])
		{
			%client.isAdmin = true;
			%client.paNum = %x;
			Client::sendMessage(%client, 0, "You have been given PA power.~wCapturedTower.wav");
			echo("PASSWORD: " @ Client::getName(%client) @ " (" @ %client @ ") entered this password: " @ %password @ " and is now PA");
				$Admin = %name @ " gained public admin with this pass : "@ %password @" . Client ID : "@%client@" IP Address : "@%ip; 
				export("Admin","config\\Admin.log",true);
			return;
		}
	}
	Client::sendMessage(%client, 0, "Your password is invalid.~waccess_denied.wav");
	echo("PASSWORD: " @ Client::getName(%client) @ " (" @ %client @ ") entered this password: " @ %password @ " and FAILED");
 		 
	$Admin = %name @ " failed sad admin with the pass : "@ %password @" . Client ID : "@%client@" IP Address : "@%ip; 
	export("Admin","config\\Admin.log",true);
}

function remoteSetPassword(%client, %password)
{
	if(%client.isSuperAdmin)
		$Server::Password = %password;
}

function remoteSetTimeLimit(%client, %time)
{
	%time = floor(%time);
	if(%time == $Server::timeLimit || (%time != 0 && %time < 1))
		return;
	if(%client.isAdmin)
	{
		$Server::timeLimit = %time;
		if(%time)
			messageAll(0, Client::getName(%client) @ " changed the time limit to " @ %time @ " minute(s).~wCapturedTower.wav");
		else
			messageAll(0, Client::getName(%client) @ " disabled the time limit.~wCapturedTower.wav");
			
	}
	else
	{
		$Server::timeLimit = %time;
		if(%time)
			messageAll(0,"changing the time limit to " @ %time @ " minute(s).~wCapturedTower.wav");
		else
			messageAll(0,"disabling the time limit.~wCapturedTower.wav");
			
	}
	
}

function remoteSetTeamInfo(%client, %team, %teamName, %skinBase)
{
	if(%team >= 0 && %team < 8 && %client.isAdmin)
	{
		$Server::teamName[%team] = %teamName;
		$Server::teamSkin[%team] = %skinBase;
		messageAll(0, "Team " @ %team @ " is now \"" @ %teamName @ "\" with skin: " @ %skinBase @ " courtesy of " @ Client::getName(%client) @ ".  Changes will take effect next mission.");
	}
}

function remoteVoteYes(%clientId)
{
	%clientId.vote = "yes";
	centerprint(%clientId, "", 0);
}

function remoteVoteNo(%clientId)
{
	%clientId.vote = "no";
	centerprint(%clientId, "", 0);
}

function Admin::startMatch(%admin)
{
	if(%admin == -1 || %admin.isAdmin)
	{
		if(!$CountdownStarted && !$matchStarted)
		{
			if(%admin == -1)
				messageAll(0, "Match start countdown forced by vote.");
			else
				messageAll(0, "Match start countdown forced by " @ Client::getName(%admin));
			Game::ForceTourneyMatchStart();
		}
	}
}

function Admin::setTeamDamageEnable(%admin, %enabled)
{
	if(%admin == -1 || %admin.isAdmin)
	{
		if(%enabled)
		{
			$Server::TeamDamageScale = 1;
			if(%admin == -1)
			{
				messageAll(0, "Team damage ENABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Team damage ENABLED by consensus.", 3);
				logAdminAction(%admin, "Team damage ENABLED by consensus.");
			}
			else
			{	
				messageAll(0, Client::getName(%admin) @ " ENABLED team damage.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@Client::getName(%admin) @ " ENABLED team damage.", 3);
				logAdminAction(%admin, "Team damage ENABLED.");
			}
		}
		else
		{
			$Server::TeamDamageScale = 0;
			if(%admin == -1)
			{
				messageAll(0, "Team damage DISABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Team damage DISABLED by consensus.", 3);
				logAdminAction(%admin, "Team damage DISABLED by consensus.");
			}
			else
			{
				messageAll(0, Client::getName(%admin) @ " DISABLED Team damage.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@Client::getName(%admin) @ " DISABLED Team damage.", 3);
				logAdminAction(%admin, "Team damage DISABLED.");
			}
		}
	}
}

function Admin::setBaseDamageEnable(%admin, %enabled)
{
	if(%admin == -1 || %admin.isAdmin)
	{
		if(%enabled)
		{
			$BaseRape = 0;
			if(%admin == -1)
			{
				messageAll(0, "BASE damage ENABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>BASE damage ENABLED by consensus.", 3);
				logAdminAction(%admin, "Out of area damage ENABLED by consensus.");
			}
			else
			{
				messageAll(0, Client::getName(%admin) @ " ENABLED BASE damage.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@Client::getName(%admin) @ " ENABLED BASE damage.", 3);
				logAdminAction(%admin, "BASE damage ENABLED.");
			}
		}
		else
		{
			$BaseRape = 1;
			if(%admin == -1)
			{
				messageAll(0, "Base damage DISABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Base damage DISABLED by consensus.", 3);
				logAdminAction(%admin, "Base damage DISABLED by consensus.");
			}
			else
			{
				messageAll(0, Client::getName(%admin) @ " DISABLED Base damage.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@Client::getName(%admin) @ " DISABLED Base damage.", 3);
				logAdminAction(%admin, "Base damage DISABLED.");
			}
		}
	}
}

function Admin::setBaseHealingEnable(%admin, %enabled)
{
	if(%admin == -1 || %admin.isAdmin)
	{
		if(%enabled)
		{
			$BaseHeal = 1;
			AutoRepair(0.01);
			if(%admin == -1)
			{
				messageAll(0, "Base healing ENABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Base healing ENABLED by consensus.", 3);
				logAdminAction(%admin, "Base healing ENABLED by consensus.");
			}
			else
			{
				messageAll(0, Client::getName(%admin) @ " ENABLED base healing.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@Client::getName(%admin) @ " ENABLED base healing.", 3);
				logAdminAction(%admin, "base healing ENABLED.");
			}
		}
		else
		{
			$BaseHeal = 0;
			AutoRepair(-0.01);
			if(%admin == -1)
			{
				messageAll(0, "Base healing DISNABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Base healing DISABLED by consensus.", 3);
				logAdminAction(%admin, "Base healing DISABLED by consensus.");
			}
			else
			{
				messageAll(0, Client::getName(%admin) @ " DISABLED base healing.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@Client::getName(%admin) @ " DISABLED base healing.", 3);
				logAdminAction(%admin, "Base healing DISABLED.");
			}
		}
	}
}

function Admin::setPlayerDamageEnable(%admin, %enabled)
{
	if(%admin == -1 || %admin.isAdmin)
	{
		if(%enabled)
		{
			$NoPlayerDamage = 0;
			if(%admin == -1)
			{
				messageAll(0, "Player damage ENABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Player damage ENABLED by consensus.", 3);
				logAdminAction(%admin, "Player damage ENABLED by consensus.");
			}
			else
			{
				messageAll(0, Client::getName(%admin) @ " ENABLED Player damage.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@Client::getName(%admin) @ " ENABLED Player damage.", 3);
				logAdminAction(%admin, "player damage ENABLED.");
			}
		}
		else
		{
			$NoPlayerDamage = 1;
			if(%admin == -1)
			{
				messageAll(0, "Out of area damage DISABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Out of area damage DISABLED by consensus.", 3);
				logAdminAction(%admin, "Out of area damage DISABLED by consensus.");
			}
			else
			{
				messageAll(0, Client::getName(%admin) @ " DISABLED player damage.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@Client::getName(%admin) @ " DISABLED player damage.", 3);
				logAdminAction(%admin, "player damage DISABLED.");
			}

		}
	}
}
//	messageAll(0, Client::getName(%clientId) @ " ENABLED " @ %option @"~wCapturedTower.wav");
//	centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " ENABLED " @ %option, 3);
function Admin::setAreaDamageEnable(%admin, %enabled)
{
	if(%admin == -1 || %admin.isAdmin)
	{
		if(%enabled)
		{
			$outAreaDamage = 1;
			if(%admin == -1)
			{
				messageAll(0, "Out of area damage ENABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Out of area damage ENABLED by consensus.", 3);
				logAdminAction(%admin, "Out of area damage ENABLED by consensus.");
			}
			else
			{
				messageAll(0, Client::getName(%admin) @ " ENABLED out of area damage.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@Client::getName(%admin) @ " ENABLED out of area damage.", 3);
				logAdminAction(%admin, "Out of area damage ENABLED.");
			}				
		}
		else
		{
			$outAreaDamage = 0;
			if(%admin == -1)
			{
				messageAll(0, "Out of area damage DISABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Out of area damage DISABLED by consensus.", 3);
				logAdminAction(%admin, "Out of area damage ENABLED by consensus.");
			}
			else
			{
				messageAll(0, Client::getName(%admin) @ " DISABLED out of area damage.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@Client::getName(%admin) @ " DISABLED out of area damage.", 3);
				logAdminAction(%admin, "Out of area damage DISABLED.");
			}
		}
	}
}
function Admin::setBuild(%admin, %enabled)
{
	if(%admin == -1 || %admin.isAdmin)
	{
		if(%enabled)
		{
			$Build = 1;
			if(%admin == -1)
			{
				messageAll(0, "Builder ENABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Builder ENABLED by consensus.", 3);
				logAdminAction(%admin, "builder ENABLED by consensus.");
			}
			else
			{
				messageAll(0, Client::getName(%admin) @ " ENABLED Builder.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@Client::getName(%admin) @ " ENABLED Builder.", 3);
				logAdminAction(%admin, " ENABLED builder.");
			}
		}
		else
		{
			$Build = 0;
 			if(%admin == -1)
 			{
				messageAll(0, "Builder DISABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Builder DISABLED by consensus.", 3);
				logAdminAction(%admin, "builder ENABLED by consensus.");
			}
			else
			{
				messageAll(0, Client::getName(%admin) @ " DISABLED Builder.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@Client::getName(%admin) @ " DISABLED Builder.", 3);
				logAdminAction(%admin, " DISABLED builder.");
			}
		}
	}
}

function Admin::kick(%admin, %client, %ban)
{
	if(%admin != %client && (%admin == -1 || %admin.isAdmin))
	{
		if(%ban && !%admin.isSuperAdmin)
			return;
		if(%ban)
		{
			%word = "banned";
			%cmd = "BAN: ";
		}
		else
		{
			%word = "kicked";
			%cmd = "KICK: ";
		}
		if(%client.isAdmin)
		{
			if(%admin == -1)
				messageAll(0, "A admin cannot be " @ %word @ ".");
			else
				Client::sendMessage(%admin, 0, "A super admin cannot be " @ %word @ ".");
			return;
		}
		%ip = Client::getTransportAddress(%client);

		//echo(%cmd @ %admin @ " " @ %client @ " " @ %ip);

		if(%ip == "")
			return;
		if(%ban)
		{
			BanList::add(%ip, $Havoc::BanTime);
			BanList::export("config\\banlist.cs");
		}
		else
		{
			BanList::add(%ip, $Havoc::KickTime);
			BanList::export("config\\banlist.cs");
		}

		%name = Client::getName(%client);

		if(%admin == -1)
		{
			MessageAll(0, %name @ " was " @ %word @ " from vote.~wCapturedTower.wav");
			echo(%cmd @ %name @ " ( " @ %ip @ " was " @ %word @ " from vote.");
			%message = %cmd @ %name @ " ( " @ %ip @ " was " @ %word @ " from vote.";
			Net::kick(%client, "You were " @ %word @ " by  consensus.");
		}
		else
		{
			MessageAll(0, %name @ " was " @ %word @ " by " @ Client::getName(%admin) @ ".~wCapturedTower.wav");
			echo(%cmd @ %name @ " ( " @ %ip @ " was " @ %word @ " by " @ Client::getName(%admin) @ "." );
			%message = %cmd @ %name @ " ( " @ %ip @ " was " @ %word @ " by " @ Client::getName(%admin) @ ".";
			Net::kick(%client, "You were " @ %word @ " by " @ Client::getName(%admin));
		}
		logAdminAction(%admin,%message);
	}
}

function Admin::setModeFFA(%clientId)
{
	if($Server::TourneyMode && (%clientId == -1 || %clientId.isAdmin))
	{
		$Server::TeamDamageScale = 0;
		if(%clientId == -1)
			messageAll(0, "Server switched to Free-For-All Mode.~wCapturedTower.wav");
		else
		{	
			messageAll(0, "Server switched to Free-For-All Mode by " @ Client::getName(%clientId) @ ".~wCapturedTower.wav");
			centerprintall("<jc><f1>"@Client::getName(%clientId) @ " switched to free for all mode.", 3);
			logAdminAction(%clientId, "switched to free for all mode.");
		}
		$Server::TourneyMode = false;
		centerprintall(); // clear the messages
		if(!$matchStarted && !$countdownStarted)
		{
			if($Server::warmupTime)
				Server::Countdown($Server::warmupTime);
			else	
				Game::startMatch();
		}
	}
}

function Admin::setModeTourney(%clientId)
{
	if(!$Server::TourneyMode && (%clientId == -1 || %clientId.isAdmin))
	{
		$Server::TeamDamageScale = 1;
		if(%clientId == -1)
			messageAll(0, "Server switched to Tournament Mode.~wCapturedTower.wav");
		else
		{
			messageAll(0, "Server switched to Tournament Mode by " @ Client::getName(%clientId) @ ".~wCapturedTower.wav");
			centerprintall("<jc><f1>"@Client::getName(%clientId) @ " Server switched to Tournament Mode.", 3);
			logAdminAction(%clientId, "Server switched to Tournament Mode.");
		}
		$Server::TourneyMode = true;
		Server::nextMission();
	}
}

function Admin::fun(%admin,%clientId,%word)
{
	if(%admin == -1)
	{
		centerprintall("<jc><f1>"@Client::getName(%clientId) @ " was "@ %word @ " by consensus.", 3);
		logAdminAction(%admin, Client::getName(%clientId) @ " was "@ %word @ " by consensus.");		
		messageAll(0, Client::getName(%clientId) @ " was "@ %word @ " by consensus.~wCapturedTower.wav");
	}
	else
	{
		messageAll(0, Client::getName(%clientId) @ " was "@ %word @ " by "@ Client::getName(%admin)@".~wCapturedTower.wav");
		centerprintall("<jc><f1>"@Client::getName(%clientId) @ " was "@ %word @ " by "@ Client::getName(%admin)@".", 3);
		logAdminAction(%admin, Client::getName(%clientId) @ " was "@ %word @ " by "@ Client::getName(%admin));
	}
	if(%word=="frozen")
	{	freeze(%clientId,false);
  		messageall(0,Client::getName(%clientId) @ " was "@ %word @ " for 2 minutes by consensus.");
  	}
	if(%word=="poisoned") poison(%clientId);
	if(%word=="De-Tongued") %clientId.silenced = true;
	if(%word=="Un-Muted") %clientId.silenced = "";
	if(%word=="Admined") %clientId.isAdmin = true;
	if(%word=="De -admined") %clientId.isAdmin = false;
	if(%word=="Kicked") Admin::kick(%admin, %clientId);
	if(%word=="Banned") Admin::kick(%admin, %clientId,true);
}

function Admin::voteFailed()
{
	$curVoteInitiator.numVotesFailed++;
	if($curVoteAction == "kick" || $curVoteAction == "admin")
		$curVoteOption.voteTarget = "";
}

//votepassed
function Admin::voteSucceded()
{
	$curVoteInitiator.numVotesFailed = "";
	if($curVoteAction == "kick")
	{
		if($curVoteOption.voteTarget)
			Admin::kick(-1, $curVoteOption);
	}
	if($curVoteAction == "poison")
	{
		if($curVoteOption.voteTarget)
		{
			//Admin::fun(-1, $curVoteOption,"poisoned");
			KillRatDead($curVoteOption);
			centerprint($curVoteOption, "<jc><f1>you were poisoned by everyone.", 15);		
			messageAll(0, Client::getName($curVoteOption) @ " had some acid with their Wheaties.");
			logAdminAction(%clientId," Poisoned by consensus" @ Client::getName($curVoteOption));	
		}
	}
	if($curVoteAction == "unsilence")
	{
		if($curVoteOption.voteTarget)
			Admin::fun(-1, $curVoteOption,"Un-Muted");
	}	
	if($curVoteAction == "silence")
	{
		if($curVoteOption.voteTarget)
			Admin::fun(-1, $curVoteOption,"De-Tongued");
	}	
	if($curVoteAction == "freeze")
	{
		if($curVoteOption.voteTarget)
		{
			//Admin::fun(-1, $curVoteOption,"poisoned");
			Freeze($curVoteOption);
			centerprint($curVoteOption, "<jc><f1>you were frozen by everyone.", 15);		
			messageAll(0, Client::getName($curVoteOption) @ " gets the cold shoulder for 2 minutes.");
			logAdminAction(%clientId," frozen by consensus" @ Client::getName($curVoteOption));	
		}
	}
	else if($curVoteAction == "admin")
	{
		if($curVoteOption.voteTarget)
		{
			$curVoteOption.isAdmin = true;
			messageAll(0, Client::getName($curVoteOption) @ " has become an administrator.~wCapturedTower.wav");
			if($curVoteOption.menuMode == "options")
				Game::menuRequest($curVoteOption);
		}
		$curVoteOption.voteTarget = false;
	}
	else if($curVoteAction == "cmission")
	{
		messageAll(0, "Changing to mission " @ $curVoteOption @ ".~wCapturedTower.wav");
		Vote::changeMission();
		Server::loadMission($curVoteOption);
	}
	else if($curVoteAction == "tourney")
		Admin::setModeTourney(-1);
	else if($curVoteAction == "ffa")
		Admin::setModeFFA(-1);
	else if($curVoteAction == "etd")
		Admin::setTeamDamageEnable(-1, true);
	else if($curVoteAction == "dtd")
		Admin::setTeamDamageEnable(-1, false);
	else if($curVoteOption == "smatch")
		Admin::startMatch(-1);
// vote map
	else if($curVoteAction == "NextMap")
		nextmap();
	else if($curVoteAction == "RandomMap")
		randommap();
	else if($curVoteAction == "ReplayMap")
		replaymap();
//vote time		
	else if($curVoteAction == "VTime")
		remoteSetTimeLimit(0, $curVoteOption);
// damage vote	
//base rape
	else if($curVoteAction == "eBaseD") 
		Admin::setBaseDamageEnable(-1, true);
	else if($curVoteAction == "dBaseD") 
		Admin::setBaseDamageEnable(-1, false);
// base healing		
	else if($curVoteAction == "eBaseH") 
		Admin::setBaseHealingEnable(-1, true);
	else if($curVoteAction == "dBaseH") 
		Admin::setBaseHealingEnable(-1, false);
//out of area damage
	else if($curVoteAction == "eOutAreaD") 
		Admin::setAreaDamageEnable(-1, true);
	else if($curVoteAction == "dOutAreaD") 
		Admin::setAreaDamageEnable(-1, false);	
// player damage	
	else if($curVoteAction == "ePlayerD") 
		Admin::setPlayerDamageEnable(-1, true);
	else if($curVoteAction == "dPlayerD") 
		Admin::setPlayerDamageEnable(-1, false);
//Builder mode
	else if($curVoteAction == "ebm")
		Admin::setBuild(-1, true);
	else if($curVoteAction == "dbm")
		Admin::setBuild(-1, false);
}

function Admin::countVotes(%curVote)
{
	// if %end is true, cancel the vote either way
	if(%curVote != $curVoteCount)
		return;
	%votesFor = 0;
	%votesAgainst = 0;
	%votesAbstain = 0;
	%totalClients = 0;
	%totalVotes = 0;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		%totalClients++;
		if(%cl.vote == "yes")
		{
			%votesFor++;
			%totalVotes++;
		}
		else if(%cl.vote == "no")
		{
			%votesAgainst++;
			%totalVotes++;
		}
		else
			%votesAbstain++;
	}
	%minVotes = floor($Server::MinVotesPct * %totalClients);
	if(%minVotes < $Server::MinVotes)
		%minVotes = $Server::MinVotes;

	if(%totalVotes < %minVotes)
	{
		%votesAgainst += %minVotes - %totalVotes;
		%totalVotes = %minVotes;
	}
	%margin = $Server::VoteWinMargin;
	if($curVoteAction == "admin")
	{
		if(%minVotes < $Server::AdminMinVotes)
		  %minVotes = $Server::AdminMinVotes;
		%margin = $Server::VoteAdminWinMargin;
		%totalVotes = %votesFor + %votesAgainst + %votesAbstain;
		if(%totalVotes < %minVotes)
			%totalVotes = %minVotes;
	}
	if((%votesFor / %totalVotes >= %margin || $curVoteForce == "YES") && $curVoteForce != "NO")
	{
		messageAll(0, "Vote to " @ $curVoteTopic @ " passed: " @ %votesFor @ " to " @ %votesAgainst @ " with " @ %totalClients - (%votesFor + %votesAgainst) @ " abstentions.");
		$curVoteForce = "";
		Admin::voteSucceded();
	}
	else  // special team kick option:
	{
		if($curVoteAction == "kick") // check if the team did a majority number on him:
		{
			%votesFor = 0;
			%totalVotes = 0;
			for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			{
				if(GameBase::getTeam(%cl) == $curVoteOption.kickTeam)
				{
					%totalVotes++;
					if(%cl.vote == "yes")
						%votesFor++;
				}
			}
			if((%totalVotes >= $Server::MinVotes && %votesFor / %totalVotes >= $Server::VoteWinMargin || $curVoteForce == "YES") && $curVoteForce != "NO")
			{
				messageAll(0, "Vote to " @ $curVoteTopic @ " passed: " @ %votesFor @ " to " @ %totalVotes - %votesFor @ ".");
				Admin::voteSucceded();
				$curVoteTopic = "";
				$curVoteForce = "";
				return;
			}
		}
		if (%totalClients - (%votesFor + %votesAgainst) <0 || %votesFor > %votesAgainst)
			messageAll(0, "Vote to " @ $curVoteTopic @ " did not pass: " @ %votesFor @ " to " @ %votesAgainst @ ", not enough votes to " @ $curVoteTopic);
		else messageAll(0, "Vote to " @ $curVoteTopic @ " did not pass: " @ %votesFor @ " to " @ %votesAgainst @ " with " @ %totalClients - (%votesFor + %votesAgainst) @ " abstentions.");
			Admin::voteFailed();
	}
	$curVoteTopic = "";
	$curVoteForce = "";
}

function Admin::startVote(%clientId, %topic, %action, %option)
{
	if(%clientId.lastVoteTime == "")
		%clientId.lastVoteTime = -$Server::MinVoteTime;

	// we want an absolute time here.
	%time = getIntegerTime(true) >> 5;
	%diff = %clientId.lastVoteTime + $Server::MinVoteTime - %time;

	if(%diff > 0)
	{
		Client::sendMessage(%clientId, 0, "You can't start another vote for " @ floor(%diff) @ " seconds.");
		return;
	}
	if($curVoteTopic == "")
	{
		if(%clientId.numFailedVotes)
			%time += %clientId.numFailedVotes * $Server::VoteFailTime;

		%clientId.lastVoteTime = %time;
		$curVoteInitiator = %clientId;
		$curVoteTopic = %topic;
		$curVoteAction = %action;
		$curVoteOption = %option;
		if(%action == "kick")
			$curVoteOption.kickTeam = GameBase::getTeam($curVoteOption);
		$curVoteCount++;
		bottomprintall("<jc><f1>" @ Client::getName(%clientId) @ " <f0>initiated a vote to <f1>" @ $curVoteTopic, 30);
		echo("ADMINMSG: *** " @ Client::getName(%clientId) @ " initiated a vote to " @ $curVoteTopic);
		messageAll(0, Client::getName(%clientId) @ " initiated a vote to " @ $curVoteTopic@".~wCapturedTower.wav");
		logAdminAction(%clientId," initiated a vote to " @ $curVoteTopic);
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			%cl.vote = "";
		%clientId.vote = "yes";
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			if(%cl.menuMode == "options")
				Game::menuRequest(%clientId);
		schedule("Admin::countVotes(" @ $curVoteCount @ ", true);", $Server::VotingTime, 35);
	}
	else
	{
		Client::sendMessage(%clientId, 0, "Voting already in progress.");
	}
}


//=============================
// build initial menu
//=============================

function Game::menuRequest(%clientId)
{
	%curItem = 0;
	%clientId.MissVote = "";
	if(!%clientId.selClient)
		Client::buildMenu(%clientId, "SEX mod options", "options", true);
	else
	{
  		%sel = %clientId.selClient;
	  	%name = Client::getName(%sel);
  		if (Client::getOwnedObject(%sel) == -1 && Client::getTeam(%sel) != -1) %status = "(Dead)";
	  	else if (Client::getOwnedObject(%sel) != -1 && Client::getTeam(%sel) != -1) %status = "(Live)";
  		else if (Client::getTeam(%sel) == -1) %status = "(Observing)";
	  	Client::buildMenu(%clientId, %name @ ": " @ %status, "options", true); 
	}
	if($curVoteTopic != "" && %clientId.vote == "")
	{
		Client::addMenuItem(%clientId, %curItem++ @ "Vote YES to " @ $curVoteTopic, "voteYes " @ $curVoteCount);
		Client::addMenuItem(%clientId, %curItem++ @ "Vote NO to " @ $curVoteTopic, "voteNo " @ $curVoteCount);
		return;
	}
	if(%clientId.selClient)
	{
		%sel = %clientId.selClient;
		%name = Client::getName(%sel);
		//whisper	
		if(%clientId.whisper != %sel)
			Client::addMenuItem(%clientId, %curItem++ @ "Whisper to " @ %name, "whisper " @ %sel);
		else   
			Client::addMenuItem(%clientId, %curItem++ @ "Cancel Whisper", "nowhisper " @ %sel);	
		//mute
		if(%clientId.muted[%sel])
			Client::addMenuItem(%clientId, %curItem++ @ "Unmute " @ %name, "unmute " @ %sel);
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Mute " @ %name, "mute " @ %sel);
		//observe
		if(%clientId.observerMode == "observerOrbit" && !%clientId.issuperAdmin && !%clientId.isAdmin)
			Client::addMenuItem(%clientId, %curItem++ @ "Observe " @ %name, "observe " @ %sel);	
		//vote to... player
		if($curVoteTopic == "" && !$NoVote)
			Client::addMenuItem(%clientId, %curItem++ @ "Start vote on " @ %name, "voteToPlayer " @ %sel);
		if(%clientId.isAdmin)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Manage " @ %name, "manage " @ %sel);
			if(%status == "(Live)")	
				Client::addMenuItem(%clientId, %curItem++ @ "Annoy " @ %name, "annoy " @ %sel);
			else 	
				Client::addMenuItem(%clientId, %curItem++ @ "Annoy -NA, " @ %status, "return ");
			Client::addMenuItem(%clientId, %curItem++ @ "Punish " @ %name, "PenaltyBox " @ %sel);			
			Client::addMenuItem(%clientId, %curItem++ @ "Flag fun with " @ %name, "flag " @ %sel);
			if (%status == "(Live)")	
				Client::addMenuItem(%clientId, %curItem++ @ "Kill " @ %name, "Kill " @ %sel);
			else if (%status == "(Dead)")	
				Client::addMenuItem(%clientId, %curItem++ @ "Spawn " @ %name, "spawn " @ %sel);	
			else if (%status == "(Observing)")	
				Client::addMenuItem(%clientId, %curItem++ @ "Spawn to Auto team ", "Autoteam " @ %sel);				
		}
		return;
	}
	if(!$matchStarted || !$Server::TourneyMode)
	{
		Client::addMenuItem(%clientId, %curItem++ @ "Change Teams/Observe", "changeteams");
		Client::addMenuItem(%clientId, %curItem++ @ "Personal Options", "personal");
	}
	if($curVoteTopic == "" && %clientId.isAdmin)
		Client::addMenuItem(%clientId, %curItem++ @ "Voting Options ", "vote");
	else if($curVoteTopic == "" && !%clientId.isAdmin && !$NoVote)
	{
		Client::addMenuItem(%clientId, %curItem++ @ "Vote Mission ", "voteMiss");
		Client::addMenuItem(%clientId, %curItem++ @ "Vote Time ", "voteTime");
		Client::addMenuItem(%clientId, %curItem++ @ "Vote Damage ", "voteDamage");
		if(!$noVbuild)
		{
			if($Build == 1)
			 	 Client::addMenuItem(%clientId, %curItem++ @ "Vote to disable Builder", "vdbm");
			else
				Client::addMenuItem(%clientId, %curItem++ @ "Vote to enable Builder", "vebm");
		}			
	}
	// force vote
	if($curVoteTopic != "" && %clientId.issuperAdmin && !$curVoteForce)
	{
		Client::addMenuItem(%clientId, %curItem++ @ "Force Vote", "voteForce");
	}
	if(%clientId.isAdmin)
	{
		// map options
		Client::addMenuItem(%clientId, %curItem++ @ "Map Options", "mapMenu");
		// damage options
		Client::addMenuItem(%clientId, %curItem++ @ "Damage Options", "damageMenu");
 		// server options
		Client::addMenuItem(%clientId, %curItem++ @ "Server Options", "ServerOptions");		
		// Equipment options
		Client::addMenuItem(%clientId, %curItem++ @ "Equipment Options", "Equipment");	
		//Client::addMenuItem(%clientId, %curItem++ @ "Game Options", "gameMenu");
	}
}

function remoteSelectClient(%clientId, %selId)
{
	if(%clientId.selClient != %selId)
	{
		%clientId.selClient = %selId;
		if(%clientId.menuMode == "options")
			Game::menuRequest(%clientId);
		remoteEval(%clientId, "setInfoLine", 1, "Player Info for " @ Client::getName(%selId) @ ":");
		remoteEval(%clientId, "setInfoLine", 2, "Real Name: " @ $Client::info[%selId, 1]);
		remoteEval(%clientId, "setInfoLine", 3, "Email Addr: " @ $Client::info[%selId, 2]);
		remoteEval(%clientId, "setInfoLine", 4, "Tribe: " @ $Client::info[%selId, 3]);
		remoteEval(%clientId, "setInfoLine", 5, "URL: " @ $Client::info[%selId, 4]);
		if(%clientId.isAdmin)      
			remoteEval(%clientId, "setInfoLine", 6, "Ip: " @  Client::getTransportAddress(%selId));
	}
}

function processMenuFPickTeam(%clientId, %team)
{
	if(%clientId.isAdmin)
		processMenuPickTeam(%clientId.ptc, %team, %clientId);
	%clientId.ptc = "";
}

//	if(%opt == "changeteams")
//	{
//		if(!$matchStarted || !$Server::TourneyMode)
//		{
//			%teamnow = Client::getTeam(%clientId);
//
//			%tname = getTeamName(%teamnow);
  //    			if (%tname == unnamed) %tname = "Observer";
	//		Client::buildMenu(%clientId, "Change from "@ %tname @" team", "PickTeam", true);
//			if(Client::getTeam(%clientId) != -1) 
//				Client::addMenuItem(%clientId, "0Observer", -2);
//			if(Game::assignClientTeam(%clientId,true) != %teamnow)	
//				Client::addMenuItem(%clientId, "1Automatic", -1);
//			if(!$fairTeams){  
//			  for(%i = 0; %i < getNumTeams(); %i = %i + 1){
//			  if(Client::getTeam(%clientId) != %i)
//				Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);



//plasmatic
function processMenuPickTeam(%clientId, %team, %adminClient)
{	
	if(%clientId.locked && !%clientId.isadmin)
		return;
	if($debug)
		echo(%clientId, %team, %adminClient);
	checkPlayerCash(%clientId);
	if(%team != -1 && %team == Client::getTeam(%clientId))
		return;
	//these are only accessable by hax0ring, plasmatic
	if(%team < -2 || %team > getNumTeams())
	{
		messageAllExcept(%clientId, 0, %name@" is a confirmed bonehead, kill him."); 	
		messageAll(0, Client::getName(%clientId) @ " Tried to unbalance the teams even more!!");
		return;
	}	
	//this one too...
	if($fairTeams && Game::assignClientTeam(%clientId,true) != %team && %team != -2 && %team != -1) 
	{
		messageAllExcept(%clientId, 0, %name@" is a confirmed bonehead, kill him."); 	
		messageAll(0, Client::getName(%clientId) @ " Tried to unbalance the teams even more!!");
		return;
	}
	if(%clientId.observerMode == "justJoined")
	{
		%clientId.observerMode = "";
		centerprint(%clientId, "");
	}
	if((!$matchStarted || !$Server::TourneyMode || %adminClient) && %team == -2)
	{
		if(Observer::enterObserverMode(%clientId))
		{
			%clientId.notready = "";
			if(%adminClient == "") 
				messageAll(0, Client::getName(%clientId) @ " became an observer.");
			else
				messageAll(0, Client::getName(%clientId) @ " was forced into observer mode by " @ Client::getName(%adminClient) @ ".");
			Game::resetScores(%clientId);	
			Game::refreshClientScore(%clientId);
		}
		return;
	}
	%player = Client::getOwnedObject(%clientId);
	if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
	{
		playNextAnim(%clientId);
		Player::kill(%clientId);
	}
	%clientId.observerMode = "";
	if(%adminClient == "")
		messageAll(0, Client::getName(%clientId) @ " changed teams.");
	else
	{
		messageAll(0, Client::getName(%clientId) @ " was teamchanged by " @ Client::getName(%adminClient) @ ".");
		$Admin = Client::getName(%adminClient)@ " teamchanged " @ Client::getName(%clientId);   if(!%clientId.sexowner)echo($admin);
		if($AdminLog && !%clientId.sexowner)
			export("Admin","config\\Admin.log",true);
	}
	if(%team == -1)
	{
		Game::assignClientTeam(%clientId);
		%team = Client::getTeam(%clientId);
	}
	GameBase::setTeam(%clientId, %team);
	%clientId.teamEnergy = 0;
	Client::clearItemShopping(%clientId);
	if(Client::getGuiMode(%clientId) != 1)
		Client::setGuiMode(%clientId,1);		
	Client::setControlObject(%clientId, -1);
	Game::playerSpawn(%clientId, false);
	%team = Client::getTeam(%clientId);
	if($TeamEnergy[%team] != "Infinite")
		$TeamEnergy[%team] += $InitialPlayerEnergy;
	if($Server::TourneyMode && !$CountdownStarted)
	{
		bottomprint(%clientId, "<f1><jc>Press FIRE when ready.", 0);
		%clientId.notready = true;
	}
}

//no longer used
function annihilationprocessMenuPickTeam(%clientId, %team, %adminClient)
{
	echo("tc ",%clientId, %team, %adminClient);
	checkPlayerCash(%clientId);
	%teamnow = Client::getTeam(%clientId);
	if(%team == -1 && Game::assignClientTeam(%clientId,true) == %teamnow){
		//messageAll(0, Client::getName(%clientId) @ " tried to make the teams even more unfair.");
		return;}
	if(%team != -1 && %team == %teamnow)
		return;

	if(%clientId.observerMode == "justJoined")
	{
		%clientId.observerMode = "";
		centerprint(%clientId, "");
	}

	if((!$matchStarted || !$Server::TourneyMode || %adminClient) && %team == -2)
	{
		if(Observer::enterObserverMode(%clientId))
		{
			%clientId.notready = "";
			if(%adminClient == "") 
				messageAll(0, Client::getName(%clientId) @ " became an observer.");
			else
			{
				messageAll(0, Client::getName(%clientId) @ " was forced into observer mode by " @ Client::getName(%adminClient) @ ".");
				echo(Client::getName(%clientId) @ " was forced into observer mode by " @ Client::getName(%adminClient));
			}
			Game::resetScores(%clientId);	
			Game::refreshClientScore(%clientId);
		}
		return;
	}
	%change = false;
	 //&& 
	if(!$Havoc::FairTeams) 
	{
		if(%team == -1)
		{
			Game::assignClientTeam(%clientId);
			%team = Client::getTeam(%clientId);
		}
		if(%adminClient == "")
			messageAll(0, Client::getName(%clientId) @ " changed teams.");
		else
		{
			messageAll(0, Client::getName(%clientId) @ " was teamchanged by " @ Client::getName(%adminClient) @ ".");
			echo(Client::getName(%clientId) @ " was teamchanged by " @ Client::getName(%adminClient));
		}
		%change = true;
	}
	// && Game::assignClientTeam(%clientId,true) != %teamnow
	else
	{
		if(%team == -1)
		{
			Game::assignClientTeam(%clientId);
			%team = Client::getTeam(%clientId);
			if(%adminClient == "")
				messageAll(0, Client::getName(%clientId) @ " changed teams to make it fair.");
			else
			{
				messageAll(0, Client::getName(%clientId) @ " was teamchanged by " @ Client::getName(%adminClient) @ " to even the Teams.");
				echo(Client::getName(%clientId) @ " was teamchanged by " @ Client::getName(%adminClient));
			}
			%change = true;
		}
		else
		{
			if(AnnihilationFairTeamCheck(%teamnow, %team)) 
			{
				if(%adminClient == "")
					messageAll(0, Client::getName(%clientId) @ " changed teams to make it fair.");
				else
				{
					messageAll(0, Client::getName(%clientId) @ " was teamchanged by " @ Client::getName(%adminClient) @ " to even the Teams.");
					echo(Client::getName(%clientId) @ " was teamchanged by " @ Client::getName(%adminClient)); 
				}
				%change = true;
			}
			else
			{
				if(%adminClient == "") 
				{
					messageAll(0, Client::getName(%clientId) @ " tried to make the teams even more unfair.");
					%change = false;
				}
				else
				{
					messageAll(0, Client::getName(%clientId) @ " was teamchanged by " @ Client::getName(%adminClient) @ ".");
					echo(Client::getName(%clientId) @ " was teamchanged by " @ Client::getName(%adminClient));
					%change = true;
				}
			}	
		}
	}	
	if(%change) 
	{
		%player = Client::getOwnedObject(%clientId);
		if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player)) 
		{
			playNextAnim(%clientId);
			Player::kill(%clientId);
		}
		%clientId.observerMode = "";
//		if(%adminClient == "")
//		messageAll(0, Client::getName(%clientId) @ " changed teams.");
//		else
//		messageAll(0, Client::getName(%clientId) @ " was teamchanged by " @ Client::getName(%adminClient) @ ".");
//
//		if(%team == -1)
//		{
//			Game::assignClientTeam(%clientId);
//			%team = Client::getTeam(%clientId);
//		}
		GameBase::setTeam(%clientId, %team);
		%clientId.teamEnergy = 0;
		Client::clearItemShopping(%clientId);
		if(Client::getGuiMode(%clientId) != 1)
			Client::setGuiMode(%clientId,1);		
		Client::setControlObject(%clientId, -1);

		Game::playerSpawn(%clientId, false);
		%team = Client::getTeam(%clientId);
		if($TeamEnergy[%team] != "Infinite")
			$TeamEnergy[%team] += $InitialPlayerEnergy;
		if($Server::TourneyMode && !$CountdownStarted)
		{
			bottomprint(%clientId, "<f1><jc>Press FIRE when ready.", 0);
			%clientId.notready = true;
		}
	}
}

function processMenuOptions(%clientId, %option)
{
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);
	if (%opt == "personal") 
	{	
		%curItem = 0;
		Client::buildMenu(%clientId, "Personal Options", "options", true);
		if(%clientId.quickinv == true)
		Client::addMenuItem(%clientId, %curItem++ @ "Disable quick inventory", "quickinv");
	else
		Client::addMenuItem(%clientId, %curItem++ @ "Enable quick inventory", "quickinv");
		if(%clientId.weaponHelp == true)
		Client::addMenuItem(%clientId, %curItem++ @ "Disable Weapon Help", "weaponHelp");
	else
		Client::addMenuItem(%clientId, %curItem++ @ "Enable Weapon Help", "weaponHelp");
		return;
	}
	
	// END personal Options

		
	if(%opt == "changeteams")
	{
		if(%clientId.locked){
			Client::sendMessage(%clientId,0,"You cannot change teams, that privilege been revoked.");
			centerprint(%clientId, "<jc><f1>you don't have team changing privileges.", 15);			
			return;
			 }
		if(!$matchStarted || !$Server::TourneyMode)
		{
			%teamnow = Client::getTeam(%clientId);

			%tname = getTeamName(%teamnow);
      			if (%tname == unnamed) %tname = "Observer";
			Client::buildMenu(%clientId, "Change from "@ %tname @" team", "PickTeam", true);
			if(Client::getTeam(%clientId) != -1) 
				Client::addMenuItem(%clientId, "0Observer", -2);
			if(Game::assignClientTeam(%clientId,true) != %teamnow)	
				Client::addMenuItem(%clientId, "1Automatic", -1);
			if(!$fairTeams){  
			  for(%i = 0; %i < getNumTeams(); %i = %i + 1){
			  if(Client::getTeam(%clientId) != %i)
				Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
         			}
         		}
			return;
		}
	}

	// global notification for mute -Plasmatic
	else if(%opt == "mute")
	{
		messageAllExcept(%cl,0,Client::getName(%clientId)@" is no longer listening to "@Client::getName(%cl)@".");
		Client::sendMessage(%cl,0,Client::getName(%clientId)@" is no longer listening to you.");
		%clientId.muted[%cl] = true;
	}
	else if(%opt == "unmute")
	{
		messageAllExcept(%cl,0,Client::getName(%clientId)@" is listening to "@Client::getName(%cl)@ " again.");
		Client::sendMessage(%cl,0,Client::getName(%clientId)@" is listening to you again.");
		%clientId.muted[%cl] = "";
	}
	else if(%opt == "vkick"&& !$NoVote)
	{
		%cl.voteTarget = true;
		Admin::startVote(%clientId, "kick " @ Client::getName(%cl), "kick", %cl);
	}
	else if(%opt == "vadmin" && !$NoVote && !$NoVoteAdmin)
	{
		%cl.voteTarget = true;
		Admin::startVote(%clientId, "admin " @ Client::getName(%cl), "admin", %cl);
	}
	else if(%opt == "vfreeze"&& !$NoVote)
	{
		%cl.voteTarget = true;
		Admin::startVote(%clientId, "freeze " @ Client::getName(%cl), "freeze", %cl);
	}	
	else if(%opt == "vpoison"&& !$NoVote)
	{
		%cl.voteTarget = true;
		Admin::startVote(%clientId, "poison " @ Client::getName(%cl), "poison", %cl);
	}
	else if(%opt == "vsilence"&& !$NoVote)
	{
		%cl.voteTarget = true;
		Admin::startVote(%clientId, "Detongue " @ Client::getName(%cl), "silence", %cl);
	}	
	else if(%opt == "vunsilence"&& !$NoVote)
	{
		%cl.voteTarget = true;
		Admin::startVote(%clientId, "UNMUTE " @ Client::getName(%cl), "unsilence", %cl);
	}
	else if(%opt == "vote" && !$NoVote)
	{	
		voteMenu(%clientId);
		return;
	}
	else if(%opt == "voteToPlayer" && !$NoVote)
	{	
		voteToPlayer(%clientId, %cl);
		return;
	}
	//--vtreturn
	else if(%opt == "Autoteam" && %clientId.isadmin)
	{	
		processMenuPickTeam(%cl, -1, %clientId);		
		Game::menuRequest(%clientId);
		return;
	}
	else if(%opt == "spawn" && %clientId.isadmin)
	{	
		logAdminAction(%clientId," spawned " @ %name);		
		Game::playerSpawn(%cl, true);
		Game::menuRequest(%clientId);		
		return;
	}
	else if(%opt == "manage" && %clientId.isadmin)
	{	
		PlayerManage(%clientId, %cl);
		return;
	}
	//PenaltyBox	
	else if(%opt == "PenaltyBox" && %clientId.isadmin)
	{	
		PenaltyBox(%clientId, %cl);
		return;
	}
	
	else if(%opt == "annoy" && %clientId.isadmin)
	{	
		PlayerAnnoy(%clientId, %cl);
		return;
	}
	else if(%opt == "flag" && %clientId.isadmin)
	{	
		PlayerFlag(%clientId, %cl);
		return;
	}
	else if(%opt == "Kill" && %clientId.isadmin)
	{	
		PlayerKill(%clientId, %cl);
		return;
	}
	else if(%opt == "return")
	{	
		Game::menuRequest(%clientId);
		return;
	}
	//--
	else if(%opt == "voteForce" && %clientId.isadmin)
	{	
		voteForce(%clientId);
		return;
	}
	else if(%opt == "adminMenu" && %clientId.isadmin)
	{	
		adminMenu(%clientId);
		return;
	}
	else if(%opt == "vcmission" && !$NoVote)
	{	%clientId.MissVote = true;
		Admin::changeMissionMenu(%clientId, %opt == "vcmission");
		return;
	}
	else if(%opt == "mapMenu" && %clientId.isadmin)
	{
		MissMenu(%clientId,true);
		return;
	}
	else if(%opt == "voteMiss" && !$NoVote)
	{
		MissMenu(%clientId,false);
		return;
	}
 	//Build mode
 	else if(%opt == "vebm" && !$NoVote)
		Admin::startVote(%clientId, "enable Builder mode", "ebm", 0);
	else if(%opt == "vdbm" && !$NoVote)
		Admin::startVote(%clientId, "disable Builder mode", "dbm", 0); 
	else if(%opt == "voteTime" && !$NoVote)
	{
		TimeMenu(%clientId,false);
		return;
	}
	else if(%opt == "damageMenu" && %clientId.isadmin)
	{
		DamageMenu(%clientId,true);
		return;
	}
	else if(%opt == "voteDamage" && !$NoVote)
	{
		DamageMenu(%clientId,false);
		return;
	}
	else if(%opt == "ServerOptions" && %clientId.isadmin)
	{
		ServerOptions(%clientId);
		return;
	}
	else if(%opt == "Equipment" && %clientId.isadmin)
	{
		EquipmentOptions(%clientId);
		return;
	}
	// added confirmation for ffa, and tourney mode -Plasmatic
	else if(%opt == "cffa" && %clientId.isadmin)
	{
		Client::buildMenu(%clientId, "Confirm Free For All Mode:", "ccffa", true);
		Client::addMenuItem(%clientId, "1Reset to Free For All Mode.", "yes" );
		Client::addMenuItem(%clientId, "2Don't switch to Free for all.", "no" );
		return;
	}
//	else if(%opt == "ccffa")
//		Admin::setModeFFA(%clientId);
	else if(%opt == "ctourney" && %clientId.isadmin)
	{
		Client::buildMenu(%clientId, "Confirm Tournament Mode:", "cctourney", true);
		Client::addMenuItem(%clientId, "1Reset to Tournament Mode.", "yes");
		Client::addMenuItem(%clientId, "2Don't switch to Tournament.", "no");
		return;
	}
	else if(%opt == "forceyes" && %cl == $curVoteCount && %clientId.isadmin) 
	{ 
		echo("ADMINMSG: *** " @ Client::getName(%clientId) @ " forced the current vote to PASS!");
		superAdminMsg(Client::getName(%clientId) @ " forced the current vote to PASS!");
			$Admin = %name @ " forced a vote to pass. Client ID : "@%client@" IP Address : "@%ip; 
			export("Admin","config\\Admin.log",true);
		$curVoteForce = "YES";
		//%clientId.hv = 0;
	}
	else if(%opt == "forceno" && %cl == $curVoteCount && %clientId.isadmin) 
	{ 
		echo("ADMINMSG: *** " @ Client::getName(%clientId) @ " forced the current vote to FAIL!");
		superAdminMsg(Client::getName(%clientId) @ " forced the current vote to FAIL!");
			$Admin = %name @ " forced a vote to fail. Client ID : "@%client@" IP Address : "@%ip; 
			export("Admin","config\\Admin.log",true);
		$curVoteForce = "NO";
		//%clientId.hv = 0;
	}

	else if(%opt == "voteYes" && %cl == $curVoteCount)
	{
		%clientId.vote = "yes";
		centerprint(%clientId, "", 0);
	}
	else if(%opt == "voteNo" && %cl == $curVoteCount)
	{
		%clientId.vote = "no";
		centerprint(%clientId, "", 0);
	}
	else if(%opt == "whisper")
	{
		if(%clientId.whisper)
			Client::sendMessage(%clientId, 3,"You are no longer Speaking To " @ Client::getName(%clientId.whisper) @ ".");
		%clientId.whisper = %cl;
		Client::sendMessage(%clientId, 3,"You are now Speaking To " @ Client::getName(%cl) @ ".");
		return;
	}
	else if(%opt == "nowhisper")
	{
		%clientId.whisper = "";
		Client::sendMessage(%clientId, 3,"You are no longer Speaking To " @ Client::getName(%cl) @ ".");
		return;
	}
	else if(%opt == "kick" && %clientId.isadmin)
	{
		Client::buildMenu(%clientId, "Confirm kick:", "kaffirm", true);
		Client::addMenuItem(%clientId, "1Kick " @ Client::getName(%cl), "yes " @ %cl);
		Client::addMenuItem(%clientId, "2Don't kick " @ Client::getName(%cl), "no " @ %cl);
		return;
	}
	else if(%opt == "admin" && %clientId.isadmin)
	{
		Client::buildMenu(%clientId, "Confirm admim:", "aaffirm", true);
		Client::addMenuItem(%clientId, "1Admin " @ Client::getName(%cl), "yes " @ %cl);
		Client::addMenuItem(%clientId, "2Don't admin " @ Client::getName(%cl), "no " @ %cl);
		return;
	}
	else if(%opt == "ban" && %clientId.isadmin && %clientId.issuperadmin)
	{
		Client::buildMenu(%clientId, "Confirm Ban:", "baffirm", true);
		Client::addMenuItem(%clientId, "1Ban " @ Client::getName(%cl), "yes " @ %cl);
		Client::addMenuItem(%clientId, "2Don't ban " @ Client::getName(%cl), "no " @ %cl);
		return;
	}
	else if(%opt == "smatch" && %clientId.isadmin)
		Admin::startMatch(%clientId);
	else if(%opt == "cmission" && %clientId.isadmin)
	{
		Admin::changeMissionMenu(%clientId, %opt == "cmission");
		return;
	}

	else if(%opt == "reset" && %clientId.isadmin)
	{
		Client::buildMenu(%clientId, "Confirm Reset:", "raffirm", true);
		Client::addMenuItem(%clientId, "1Reset", "yes");
		Client::addMenuItem(%clientId, "2Don't Reset", "no");
		return;
	}
	else if(%opt == "weaponHelp")
	{
		if(%clientId.weaponHelp == true)
		{
			%clientId.weaponHelp = false;
			Client::sendMessage(%clientId,0,"Weapon Help Disabled.");
			centerprint(%clientId, "<jc><f1>Weapon Help Disabled.", 2);	
			return;
		}	
		else
		{
			%clientId.weaponHelp = true;
			Client::sendMessage(%clientId,0,"Weapon Help Enabled.");
			centerprint(%clientId, "<jc><f1>Weapon Help Enabled.", 2);			
			return;
		}
	}
	else if(%opt == "observe")
	{
		Observer::setTargetClient(%clientId, %cl);
		return;
	}
	Game::menuRequest(%clientId);
}

function processMenuccffa(%clientId, %opt)
{
   if(getWord(%opt, 0) == "yes")
       Admin::setModeFFA(%clientId);
   Game::menuRequest(%clientId);
}

function processMenucctourney(%clientId, %opt)
{
   if(getWord(%opt, 0) == "yes")
       Admin::setModeTourney(%clientId);
   Game::menuRequest(%clientId);
}

function processMenuKAffirm(%clientId, %opt)
{
	if(getWord(%opt, 0) == "yes")
		Admin::kick(%clientId, getWord(%opt, 1));
	Game::menuRequest(%clientId);
}

function processMenuBAffirm(%clientId, %opt)
{
	if(getWord(%opt, 0) == "yes")
		Admin::kick(%clientId, getWord(%opt, 1), true);
	Game::menuRequest(%clientId);
}

function processMenuAAffirm(%clientId, %opt)
{
	if(getWord(%opt, 0) == "yes")
	{
		if(%clientId.isSuperAdmin)
		{
			%cl = getWord(%opt, 1);
			%cl.isAdmin = true;
			messageAll(0, Client::getName(%clientId) @ " made " @ Client::getName(%cl) @ " into an admin.~wCapturedTower.wav");
		}
	}
	Game::menuRequest(%clientId);
}

function processMenuRAffirm(%clientId, %opt)
{
	if(%opt == "yes" && %clientId.isAdmin)
	{
		messageAll(0, Client::getName(%clientId) @ " reset the server to default settings.~wCapturedTower.wav");
		Server::refreshData();
	}
	Game::menuRequest(%clientId);
}

function processMenuCTLimit(%clientId, %opt)
{
	remoteSetTimeLimit(%clientId, %opt);
}

function processMenuVTime(%clientId, %opt)
{
  Admin::startVote(%clientId, "change the time to " @ %opt @" minute(s).", "VTime", %opt);

	//remoteSetTimeLimit(%clientId, %opt);
}

function processMenuPickVote(%clientId, %opt)
{
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);
}

function processMenuMission(%clientId, %option)
{
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);
	echo("processMenuMission", %opt);
	//Client::sendMessage(%clientId, 0, "processMenuMission, "@ %opt);
	
	if(%opt == "cmission")
	{
		Admin::changeMissionMenu(%clientId, %opt == "cmission");
		return;
	}
	if(%opt == "vcmission")
	{
		%clientId.MissVote = true;
		Admin::changeMissionMenu(%clientId, %opt == "vcmission");
		return;
	}
	else if(%opt == "VnextMiss")
	{
		Admin::startVote(%clientId, "start next mission ", "NextMap" ,0);
		return;
	}
	else if(%opt == "ReplayMap")
	{
		ReplayMap(%clientId);
		return;
	}
	else if(%opt == "VReplayMap")
	{
		Admin::startVote(%clientId, "restart mission ", "ReplayMap" ,0);
		return;
	}
	else if(%opt == "VrandomMiss")
	{
		Admin::startVote(%clientId, "pick random mission ", "RandomMap" ,0);
		return;
	}
	else if(%opt == "nextMiss")
	{
		nextmap(%clientId);
		return;
	}
	else if(%opt == "randomMiss")
	{
		randommap(%clientId);
		return;
	}
}

function randommap(%clientId)
{
	if(%clientId)
		messageAll(0,Client::getName(%clientId)@ " picked a random mission.~wCapturedTower.wav");
	else
		messageAll(0, "Starting a random mission");
	echo("GAME: Starting a random mission.");
	$timeLimitReached = true;
	Server::nextMission(false,true);
}

function nextmap(%clientId)
{
	if(%clientId)
		messageAll(0,Client::getName(%clientId)@ " started the next mission.~wCapturedTower.wav");
	else
		messageAll(0, "Starting next Mission");
	echo("GAME: Starting next map.");
	$timeLimitReached = true;
	Server::nextMission();
}

function replaymap(%clientId)
{
	if(%clientId)
		messageAll(0,Client::getName(%clientId)@ " restarted the mission.~wCapturedTower.wav");
	else
		messageAll(0, "Restarting Mission");
	echo("GAME: restarting map.");
	$timeLimitReached = true;
	Server::nextMission(true);
}

function MissMenu(%clientId,%admin)
{
	Client::buildMenu(%clientId, "Mission Options:", "Mission", true);
	if(%admin)
	{
		Client::addMenuItem(%clientId, %curItem++ @ "Change mission", "cmission");
		Client::addMenuItem(%clientId, %curItem++ @ "Start next mission", "nextMiss");
		Client::addMenuItem(%clientId, %curItem++ @ "Restart mission", "ReplayMap");
		Client::addMenuItem(%clientId, %curItem++ @ "Random mission", "randomMiss");
	}
	else
	{
		Client::addMenuItem(%clientId, %curItem++ @ "Vote to change mission", "vcmission");
		Client::addMenuItem(%clientId, %curItem++ @ "Vote start next mission", "VnextMiss");
		Client::addMenuItem(%clientId, %curItem++ @ "Vote to restart mission", "VReplayMap");
		Client::addMenuItem(%clientId, %curItem++ @ "Vote random mission", "VrandomMiss");
	}
}

function TimeMenu(%clientId,%admin)
{
	if(%admin)
	{
		Client::buildMenu(%clientId, "Time Options:", "CTLimit", true);
		Client::addMenuItem(%clientId, %curItem++ @ "10 minutes", "10");
		Client::addMenuItem(%clientId, %curItem++ @ "15 minutes", "15");
		Client::addMenuItem(%clientId, %curItem++ @ "20 minutes", "20");
		Client::addMenuItem(%clientId, %curItem++ @ "30 minutes", "30");
		Client::addMenuItem(%clientId, %curItem++ @ "45 minutes", "45");
		Client::addMenuItem(%clientId, %curItem++ @ "60 minutes", "60");
		Client::addMenuItem(%clientId, %curItem++ @ "120 minutes", "120");
		Client::addMenuItem(%clientId, %curItem++ @ "unlimited", "0");
	}
	else
	{
		Client::buildMenu(%clientId, "Vote Time Options:", "VTime", true);
		Client::addMenuItem(%clientId, %curItem++ @ "10 minutes", "10");
		Client::addMenuItem(%clientId, %curItem++ @ "15 minutes", "15");
		Client::addMenuItem(%clientId, %curItem++ @ "20 minutes", "20");
		Client::addMenuItem(%clientId, %curItem++ @ "30 minutes", "30");
		Client::addMenuItem(%clientId, %curItem++ @ "45 minutes", "45");
		Client::addMenuItem(%clientId, %curItem++ @ "60 minutes", "60");
		Client::addMenuItem(%clientId, %curItem++ @ "120 minutes", "120");
		Client::addMenuItem(%clientId, %curItem++ @ "unlimited", "0");
	}
}

function processmenuDamage(%clientId, %option)
{
   %opt = getWord(%option, 0);
   %cl = getWord(%option, 1);
echo("processMenuMission", %opt);
//Client::sendMessage(%clientId, 0, "processMenuDamage, "@ %opt);
//team damage
	if(%opt == "vetd")
		Admin::startVote(%clientId, "enable team damage", "etd", 0);
	else if(%opt == "vdtd")
		Admin::startVote(%clientId, "disable team damage", "dtd", 0);
	else if(%opt == "etd") {
		Admin::setTeamDamageEnable(%clientId, true);
			%ip = Client::getTransportAddress(%client); 
			%name = Client::getName(%client);
			$Admin = %name @ " enabled team damage. Client ID : "@%client@" IP Address : "@%ip; 
			export("Admin","config\\Admin.log",true); }
	else if(%opt == "dtd") {
		Admin::setTeamDamageEnable(%clientId, false);
			%ip = Client::getTransportAddress(%client); 
			%name = Client::getName(%client);
			$Admin = %name @ " disabled team damage. Client ID : "@%client@" IP Address : "@%ip; 
			export("Admin","config\\Admin.log",true); }
// base/gen damage
	if(%opt == "veBaseD")
		Admin::startVote(%clientId, "Enable gen/ station damage", "eBaseD", 0);
	else if(%opt == "vdBaseD")
		Admin::startVote(%clientId, "Disable gen/ station damage", "dBaseD", 0);
	else if(%opt == "eBaseD") {
		Admin::setBaseDamageEnable(%clientId, true);
			%ip = Client::getTransportAddress(%client); 
			%name = Client::getName(%client);
			$Admin = %name @ " enabled base damage. Client ID : "@%client@" IP Address : "@%ip; 
			export("Admin","config\\Admin.log",true); }
	else if(%opt == "dBaseD") {
		Admin::setBaseDamageEnable(%clientId, false);
			%ip = Client::getTransportAddress(%client); 
			%name = Client::getName(%client);
			$Admin = %name @ " disabled base damage. Client ID : "@%client@" IP Address : "@%ip; 
			export("Admin","config\\Admin.log",true); }
// base healing
	if(%opt == "veBaseH")
		Admin::startVote(%clientId, "Enable base healing", "eBaseH", 0);
	else if(%opt == "vdBaseH")
		Admin::startVote(%clientId, "Disable base healing", "dBaseH", 0);
	else if(%opt == "eBaseH") {
		Admin::setBaseHealingEnable(%clientId, true);
			%ip = Client::getTransportAddress(%client); 
			%name = Client::getName(%client);
			$Admin = %name @ " enabled base healing. Client ID : "@%client@" IP Address : "@%ip; 
			export("Admin","config\\Admin.log",true); }
	else if(%opt == "dBaseH") {
		Admin::setBaseHealingEnable(%clientId, false);
			%ip = Client::getTransportAddress(%client); 
			%name = Client::getName(%client);
			$Admin = %name @ " disabled base healing. Client ID : "@%client@" IP Address : "@%ip; 
			export("Admin","config\\Admin.log",true); }
//Out of area damage
	if(%opt == "veOutAreaD")
		Admin::startVote(%clientId, "Enable out of area damage", "eOutAreaD", 0);
	else if(%opt == "vdOutAreaD")
		Admin::startVote(%clientId, "Disable out of area damage", "dOutAreaD", 0);
	else if(%opt == "eOutAreaD") {
		Admin::setAreaDamageEnable(%clientId, true);
			%ip = Client::getTransportAddress(%client); 
			%name = Client::getName(%client);
			$Admin = %name @ " enabled out of area damage. Client ID : "@%client@" IP Address : "@%ip; 
			export("Admin","config\\Admin.log",true); }
	else if(%opt == "dOutAreaD") {
		Admin::setAreaDamageEnable(%clientId, false);
			%ip = Client::getTransportAddress(%client); 
			%name = Client::getName(%client);
			$Admin = %name @ " disabled out of area damage. Client ID : "@%client@" IP Address : "@%ip; 
			export("Admin","config\\Admin.log",true); }	
			
	
//player damage
	if(%opt == "vePlayerD")
		Admin::startVote(%clientId, "Enable player damage", "ePlayerD", 0);
	else if(%opt == "vdPlayerD")
		Admin::startVote(%clientId, "Disable player damage", "dPlayerD", 0);
	else if(%opt == "ePlayerD") {
		Admin::setPlayerDamageEnable(%clientId, true);
			%ip = Client::getTransportAddress(%client); 
			%name = Client::getName(%client);
			$Admin = %name @ " enabled player damage. Client ID : "@%client@" IP Address : "@%ip; 
			export("Admin","config\\Admin.log",true); }
	else if(%opt == "dPlayerD") {
		Admin::setPlayerDamageEnable(%clientId, false);
			%ip = Client::getTransportAddress(%client); 
			%name = Client::getName(%client);
			$Admin = %name @ " disabled player damage. Client ID : "@%client@" IP Address : "@%ip; 
			export("Admin","config\\Admin.log",true);
	}
}


//damage menu
function DamageMenu(%clientId,%admin)
{	
	if (%admin)
	{
		Client::buildMenu(%clientId, "Damage Options:", "Damage", true);
		//team damage
		if($Server::TeamDamageScale == 1.0)
			Client::addMenuItem(%clientId, %curItem++ @ "Disable team damage", "dtd");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Enable team damage", "etd");
		//base/ gen damage
		if($BaseRape == 1)
			Client::addMenuItem(%clientId, %curItem++ @ "Enable gen/ station damage", "eBaseD");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Disable gen/ station damage", "dBaseD");
		//self healing gen/ station		
		if($BaseHeal == 1)
			Client::addMenuItem(%clientId, %curItem++ @ "Disable base healing", "dBaseH");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Enable base healing", "eBaseH");			
		//out of area damage
		if($outAreaDamage  == 1)
			Client::addMenuItem(%clientId, %curItem++ @ "Disable out of area damage", "dOutAreaD");
		else
         		Client::addMenuItem(%clientId, %curItem++ @ "Enable out of area damage", "eOutAreaD");
		//player damage		
		if($NoPlayerDamage  == 1)
			Client::addMenuItem(%clientId, %curItem++ @ "Enable player damage", "ePlayerD");
		else
         		Client::addMenuItem(%clientId, %curItem++ @ "Disable player damage", "dPlayerD");
	}
	else
	{
		Client::buildMenu(%clientId, "Vote Damage Options:", "Damage", true);
		if($Server::TeamDamageScale == 1.0)
			Client::addMenuItem(%clientId, %curItem++ @ "Disable team damage", "vdtd");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Enable team damage", "vetd");
		//base/ gen damage
		if($BaseRape == 1)
			Client::addMenuItem(%clientId, %curItem++ @ "Enable gen/ station damage", "veBaseD");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Disable gen/station damage", "vdBaseD");
		//self healing gen/ station		
		if($BaseHeal == 1)
			Client::addMenuItem(%clientId, %curItem++ @ "Disable base healing", "vdBaseH");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Enable base healing", "veBaseH");			
		//out of area damage
		if($outAreaDamage  == 1)
			Client::addMenuItem(%clientId, %curItem++ @ "Disable out of area damage", "vdOutAreaD");
		else
        		Client::addMenuItem(%clientId, %curItem++ @ "Enable out of area damage", "veOutAreaD");
		//player damage		
		if($NoPlayerDamage  == 1)
			Client::addMenuItem(%clientId, %curItem++ @ "Enable player damage", "vePlayerD");
		else
        		Client::addMenuItem(%clientId, %curItem++ @ "Disable player damage", "vdPlayerD");
	}
}

function processMenuVoteforfun(%clientId, %option)
{
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);
	//echo("processMenuMissMenu", %opt);
	if(%opt == "cmission")
	{
		Admin::changeMissionMenu(%clientId, %opt == "cmission");
		return;
	}
}

function processMenuVote(%clientId, %option)
{
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);

	if(%opt == "voteMiss")
	{
		MissMenu(%clientId,false);
		return;
	}

	else if(%opt == "voteTime")
	{
		TimeMenu(%clientId,false);
		return;
	}
	else if(%opt == "voteDamage")
	{
		DamageMenu(%clientId,false);
		return;
	}
 //Build mode
   else if(%opt == "vebm")
      Admin::startVote(%clientId, "enable Builder mode", "ebm", 0);
   else if(%opt == "vdbm")
      Admin::startVote(%clientId, "disable Builder mode", "dbm", 0); 	
// voting   
   	if(%opt == "dVote")  ServerSwitches(%clientId,"Voting",false);
	else if(%opt == "eVote")  ServerSwitches(%clientId,"Voting",true);
// builder mode voting
   	if(%opt == "noVbuild")  ServerSwitches(%clientId,"Voting Builder",false);//$noVbuild = 1;
	else if(%opt == "vbuild")  ServerSwitches(%clientId,"Voting Builder",true);//$noVbuild = "";
// vote admin 
   	if(%opt == "dVoteAdmin")  ServerSwitches(%clientId,"Voting Admin",false);//$NoVoteAdmin = 1;
	else if(%opt == "eVoteAdmin")  ServerSwitches(%clientId,"Voting Admin",true);//$NoVoteAdmin = "";
	
		
// 	if($NoVoteAdmin == 1)
//		Client::addMenuItem(%clientId, %curItem++ @ "Enable vote admin", "eVoteAdmin");
//	else
//		Client::addMenuItem(%clientId, %curItem++ @ "Disable vote admin", "dVoteAdmin");

}

function voteMenu(%clientId)
{
	Client::buildMenu(%clientId, "Mission Options:", "Vote", true);
			Client::addMenuItem(%clientId, %curItem++ @ "Vote Mission ", "voteMiss");
			Client::addMenuItem(%clientId, %curItem++ @ "Vote Time ", "voteTime");
			Client::addMenuItem(%clientId, %curItem++ @ "Vote Damage ", "voteDamage");
     if($Build == 1)
         Client::addMenuItem(%clientId, %curItem++ @ "Vote to disable Builder", "vdbm");
      else
         Client::addMenuItem(%clientId, %curItem++ @ "Vote to enable Builder", "vebm");
// voting
 if(%clientId.isSuperAdmin){
    //vote admin	
 	if($NoVoteAdmin == 1)
		Client::addMenuItem(%clientId, %curItem++ @ "Enable vote admin", "eVoteAdmin");
	else
		Client::addMenuItem(%clientId, %curItem++ @ "Disable vote admin", "dVoteAdmin");		
   //voting
	if($NoVote == 1)
		Client::addMenuItem(%clientId, %curItem++ @ "Enable voting", "eVote");
	else
		Client::addMenuItem(%clientId, %curItem++ @ "Disable voting", "dVote");	
//builder -a Plasmatic origional.. 		
	if($noVbuild == 1)
	 Client::addMenuItem(%clientId, %curItem++ @ "Enable Build vote", "vbuild");	
	else 
	 Client::addMenuItem(%clientId, %curItem++ @ "Disable Build vote", "noVbuild");	
		
		
		
	}
}

function fixable(%player,%target)
{
	%client = Player::getClient(%player);
	%tname = GameBase::getMapName(%target);
	%name = (GameBase::getDataName(%player.repairTarget)).description;
	%data = GameBase::getDataName(%player.repairTarget);
	if($NoMapTurrets && (%data == "rocketTurret" || %data == "RocketTurret" || %data == "IndoorTurret" ||  %data == "Elfturret" ||  %data == "MortarTurret" || %data == "PlasmaTurret"))
	{
		Client::sendMessage(%client,0,%name @ " is not fixable, Turrets have been disabled by admin.");
		return false;
	}
	else if($NoInv && %data == "InventoryStation" )
	{
		Client::sendMessage(%client,0,%name @ " is not fixable, Inventories have been disabled by admin.");
		return false;
	}
	else if($NoGenerator &&(%data == "Generator" || %data == "PortGenerator" || %data == "SolarPanel"))
	{
		Client::sendMessage(%client,0,%name @ " is not fixable, Generators have been disabled by admin.");
		return false;
	}
	else if($NoVehicle &&(%data == "VehicleStation" || %data == "VehiclePad"))
	{
		Client::sendMessage(%client,0,%name @ " is not fixable, Vehicle Stations have been disabled by admin.");
		return false;
	}
	else
		return true;
}

function ProcessMenuPlayer(%clientId, %option)
{
	if(!%clientid.isadmin) return;
//	Client::sendMessage(%clientId, 0,"process Player " @ %option);		
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);
	%player = Client::getOwnedObject(%cl);	
	%name = Client::getName(%cl);
//mute   
   	if(%opt == "silence")  		
	Admin::fun(%clientId, %cl,"De-Tongued");	
   	if(%opt == "unsilence")  		
	Admin::fun(%clientId, %cl,"Un-Muted");	
//admin   
   	if(%opt == "Admin")  		
	Admin::fun(%clientId, %cl,"Admined");	
   	if(%opt == "stripAdmin")  		
	Admin::fun(%clientId, %cl,"De -admined");	
//kicks   
   	if(%opt == "kick")  		
	Admin::fun(%clientId, %cl,"Kicked");	
   	if(%opt == "ban")  		
	Admin::fun(%clientId, %cl,"Banned");	
// ClearScore	
	else if(%opt == "ClearScore")
	{
		%cl.score = 0;
		Game::refreshClientScore(%cl);	
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" Cleared your score.", 5);		
		messageAll(0, Client::getName(%clientId) @ " cleared "@ %name @"'s score.");
		logAdminAction(%clientId," cleared " @ %name @"'s score");		
		return;
	}	
// StripFlag		
	else if(%opt == "StripFlag")
	{	
	%type = Player::getMountedItem(%cl, $FlagSlot);
	if(%type != -1)
		Player::dropItem(%cl, %type);		
		
		
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" stripped your flag.", 5);		
		messageAll(0, Client::getName(%clientId) @ " stripped "@ %name @"'s flag.~wCapturedTower.wav");
		logAdminAction(%clientId," stripped " @ %name @"'s flag.");		
		return;
	}	
	
// ReturnFlag
	else if(%opt == "ReturnFlag")
	{
	
	%player = Client::getOwnedObject(%cl);	
	%this = %player.carryFlag;
	
	%type = Player::getMountedItem(%cl, $FlagSlot);
	if(%type != -1){
		Player::dropItem(%cl, %type);			
	echo("flag id "@%type);

	// the flag isn't home! so return it.
			GameBase::startFadeOut(%this);
			GameBase::setPosition(%this, %this.originalPosition);
			Item::setVelocity(%this, "0 0 0");
			GameBase::startFadeIn(%this);
			%this.atHome = true;
		%cl.flagcarried = "";
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" returned your flag.", 5);		
		messageAll(0, Client::getName(%clientId) @ " returned "@ %name @"'s flag.~wCapturedTower.wav");
		logAdminAction(%clientId," returned " @ %name @"'s flag.");		
		return;
	}	
	}
// FlagCurse
	else if(%opt == "FlagCurse")
	{
		%cl.FlagCurse = true;
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" gave you the dreaded Flag Curse.", 5);		
		messageAll(0, Client::getName(%clientId) @ " gave "@ %name @" the flag curse.");
		logAdminAction(%clientId," flag cursed " @ %name @".");	
		
		%type = Player::getMountedItem(%cl, $FlagSlot);
		if(%type != -1)	schedule("PlayerFlagCurse("@%cl@");",2);	
			
		return;
	}	
	
	else if(%opt == "NoFlagCurse")
	{
		%cl.FlagCurse = false;
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" removed the flag curse.", 5);		
		messageAll(0, Client::getName(%clientId) @ " removed "@ %name @"'s flag curse.");
		logAdminAction(%clientId," un flag cursed " @ %name @".");	
		return;
	}	

// lock team	
	else if(%opt == "lock")
	{
		%cl.locked = true;
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" removed your team changing privileges.", 5);		
		messageAll(0, Client::getName(%clientId) @ " locked "@ %name @"'s team.");
		logAdminAction(%clientId," locked " @ %name @"'s team");		
		return;
	}	
	
	else if(%opt == "unlock")
	{
		%cl.locked = false;
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" restored your team changing privileges.", 5);		
		messageAll(0, Client::getName(%clientId) @ " unlocked "@ %name @"'s team.");
		logAdminAction(%clientId," unlocked " @ %name @"'s team");	
		return;
	}	

//na -plasmatic   
   	else if(%opt == "return")  	
	Game::menuRequest(%clientId);		
//return to flag menu
   	else if(%opt == "Freturn")  	
	PlayerFlag(%clientId, %cl);		
   if(%opt == "fteamchange")
   {
      %clientId.ptc = %cl;
      %teamnow = Client::getTeam(%cl);
      %tname = getTeamName(%teamnow);
      if (%tname == unnamed) %tname = "Observer";
      Client::buildMenu(%clientId, Client::getName(%cl)@", "@%tname, "FPickTeam", true);
      if(Client::getTeam(%cl) != -1) 
      		Client::addMenuItem(%clientId, "0Observer", -2);
      if(Game::assignClientTeam(%clientId,true) != %teamnow)
		Client::addMenuItem(%clientId, "1Automatic", -1);
      for(%i = 0; %i < getNumTeams(); %i = %i + 1){
      	if(Client::getTeam(%cl) != %i)
         Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
         	}
      return;
   }      
//kill
	else if(%opt == "respawn")
	{	
		playNextAnim(%cl);
		Player::kill(%cl);
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" respawned you.", 5);		
		messageAll(0, Client::getName(%clientId) @ " respawned "@ %name);
		logAdminAction(%clientId," respawned " @ %name);
		//processMenuPickTeam(%cl, -1, %clientId);		
		Game::playerSpawn(%cl, true);
		return;
	}
	
			
	else if(%opt == "BlowUp")
	{	
	%player = Client::getOwnedObject(%cl);

	   if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
 	  {
		%Pos = GameBase::getPosition(%player); 
		%vel = Item::getVelocity(%player);
		%trans =  "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%player); 
		%obj = Projectile::spawnProjectile("suicideShell", %trans, %player, %vel);
		Projectile::spawnProjectile(%obj);
		Item::setVelocity(%obj, %vel);
		Player::blowUp(%cl);
		playNextAnim(%cl);   
		Player::kill(%cl); 
  		 }
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" blew you up", 5);
		messageAll(0, Client::getName(%clientId) @ " blew up "@ %name@".");
		logAdminAction(%clientId," Blew up " @ %name);
		return;
	}
	else if(%opt == "Sniper")
	{
		GameBase::playSound(%cl, ricochet1, 0);
		%curDie = radnomItems(3, $PlayerAnim::DieHead, $PlayerAnim::DieBack,$PlayerAnim::DieForward);
		Player::setAnimation(%this, %curDie);
		playNextAnim(%cl);
		Player::kill(%cl);
		messageAll(0, Client::getName(%clientId) @ " put a bullet through "@ %name@"'s ear");
		logAdminAction(%clientId," Killed " @ %name@" with a sniper shot");
		//processMenuPickTeam(%cl, -1, %clientId);		
		//Game::playerSpawn(%cl, true);
		return;
	}	
	else if(%opt == "Burn")
	{
		BurnUp(%cl);
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" set you on fire.", 5);		
		messageAll(0, Client::getName(%clientId) @ " set "@ %name@" on fire.");
		logAdminAction(%clientId," set " @ %name@" on fire.");		
		return;
	}
	else if(%opt == "Poison")
	{
		KillRatDead(%cl);
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" poisoned you.", 5);		
		messageAll(0, Client::getName(%clientId) @ " fed "@ %name@" some rat poison.");
		logAdminAction(%clientId," Poisoned " @ %name);		
		return;
	}
	
//annoy	
	else if(%opt == "Disarm")
	{
		%player = Client::getOwnedObject(%cl);
		%item = Player::getMountedItem(%cl,$WeaponSlot);
		Player::trigger(%player, $WeaponSlot, false);
		Player::dropItem(%cl,%item);
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" disarmed you.", 5);		
		messageAll(0, Client::getName(%clientId) @ " stole "@ %name@"'s weapon.");
		logAdminAction(%clientId," disarmed " @ %name);		
		return;
	}
	else if(%opt == "Strip")
	{
		%player = Client::getOwnedObject(%cl);
		%Player.rThrow = true;
		%player.rThStr = 10;
	for(%x = 0; %x < 15; %x = %x++){		
		%item = Player::getMountedItem(%cl,$WeaponSlot);
		//if(!%item) return;
		Player::trigger(%player, $WeaponSlot, false);
		Player::dropItem(%cl,%item);
		remoteNextWeapon(%cl);
		}
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" Stripped your weapons.", 5);		
		messageAll(0, Client::getName(%clientId) @ " had a yard sale with "@ %name@"'s weapons.");
		logAdminAction(%clientId," stripped weapons " @ %name);	
		%Player.rThrow = "";
		%player.rThStr = "";			
		return;
	}	
	else if(%opt == "Moon")
	{
	%rotZ = getWord(GameBase::getRotation(%cl),2); 
	GameBase::setRotation(%cl, "0 0 " @ %rotZ); 
	%forceDir = Vector::getFromRot(GameBase::getRotation(%cl),0,3000); 
	Player::applyImpulse(%cl,%forceDir); 
	schedule("Client::sendMessage("@%cl@", 1,\"~wmale3.wbye.wav\");", 2);
//	schedule("Client::sendMessage("@%cl@", 1,\"~wmale3.wdsgst2.wav\");", 2.2);
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" sent you to the moon!", 5);		
		messageAll(0, Client::getName(%clientId) @ " sent "@ %name@" to the moon.");
		logAdminAction(%clientId," Sent to the moon " @ %name);		
		return;
	}	
	
	else if(%opt == "Slap")
	{
	%rotZ = getWord(GameBase::getRotation(%cl),2); 
	GameBase::setRotation(%cl, "0 0 " @ %rotZ); 
	%forceDir = Vector::getFromRot(GameBase::getRotation(%cl),3000,1000); 
	Player::applyImpulse(%cl,%forceDir); 
	schedule("Client::sendMessage("@%cl@", 1,\"~wmale3.wbye.wav\");", 2);
//	schedule("Client::sendMessage("@%cl@", 1,\"~wmale3.wdsgst2.wav\");", 2.2);
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" Slapped you.", 5);		
		messageAll(0, Client::getName(%clientId) @ " slapped "@ %name@".");
		logAdminAction(%clientId," Slapped " @ %name);		
		return;
	}	
	else if(%opt == "Blind")
	{
		%player.blind = true;
		HotPoker(%cl);
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" poked your eyes out.", 5);		
		messageAll(0, Client::getName(%clientId) @ " scooped "@ %name@"'s eyes out with a spoon.");
		logAdminAction(%clientId," poked " @ %name@" eyes.");		
		return;
	}	

	else if(%opt == "UnBlind")
	{
		%player.blind = false;
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" restored your sight.", 5);		
		messageAll(0, Client::getName(%clientId) @ " restored "@ %name@"'s sight.");
		logAdminAction(%clientId," restored " @ %name@" eyes.");		
		return;
	}	
	else if(%opt == "Freeze")
	{
		freeze(%cl);
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" froze you.", 5);		
		messageAll(0, Client::getName(%clientId) @ " froze "@ %name@" into a block of ice.");
		logAdminAction(%clientId," froze " @ %name);		
		return;
	}
	else if(%opt == "Defrost")
	{
		freeze(%cl,true);
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" Defrosted you.", 5);		
		messageAll(0, Client::getName(%clientId) @ " Defrosted "@ %name@".");
		logAdminAction(%clientId," Defrost " @ %name);		
		return;
	}
// Penalty	
	else if(%opt == "15Penalty")
	{
		Penalty(%cl,false,15);
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" sent you to the penalty box for 15 seconds.", 5);		
		messageAll(0, Client::getName(%clientId) @ " sent "@ %name@" to the penalty box for 15 seconds.");
		logAdminAction(%clientId," penalised " @ %name);		
		return;
	}	
	else if(%opt == "30Penalty")
	{
		Penalty(%cl,false,30);
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" sent you to the penalty box for 30 seconds.", 5);		
		messageAll(0, Client::getName(%clientId) @ " sent "@ %name@" to the penalty box for 30 seconds.");
		logAdminAction(%clientId," penalised " @ %name);		
		return;
	}	
	else if(%opt == "60Penalty")
	{
		Penalty(%cl,false,60);
		centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" sent you to the penalty box for 60 seconds.", 5);		
		messageAll(0, Client::getName(%clientId) @ " sent "@ %name@" to the penalty box for 60 seconds.");
		logAdminAction(%clientId," penalised " @ %name);		
		return;
	}	

//   if(!Player::isDead(%sel)){
//        	 Client::addMenuItem(%clientId, %curItem++ @ "Co- Pilot " @ %name , "CoPilot " @ %sel);        
//        	 Client::addMenuItem(%clientId, %curItem++ @ "Possess " @ %name , "Possess " @ %sel);  
//        	 }
}

function HotPoker(%clientId) 
{
	%player = Client::getOwnedObject(%clientId);
	if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
	{ 
		Player::setDamageFlash(%clientId,1.0);
		if(%player.blind)
			schedule("HotPoker("@%clientId@");", 1.5);
  	}
	else
		Client::sendMessage(%clientId, 1, "In death you will see again.");
}

function Freeze(%clientId,%option) 
{
	%player = Client::getOwnedObject(%clientId);
	%player.frozen = true;
	if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player)&& !%option)
	{ 
		Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
		Observer::setOrbitObject(%clientId, %clientId, 3, 3, 3);
		schedule("Freeze("@%clientId@",true);", 120);// 2 minutes is plenty
  	}
	else if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player)&& %option)
	{ 
		Client::setControlObject(%clientId, %clientId);
		%player.frozen = false;
  	}
  	if(%option)
  		%player.frozen = false;
}

function Penalty(%clientId,%option,%time) 
{
	%player = Client::getOwnedObject(%clientId);
	%player.frozen = true;
	%clientId.silenced = true;
	if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player)&& !%option)
	{ 
		Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
		Observer::setOrbitObject(%clientId, %clientId, 3, 3, 3);
		schedule("Penalty("@%clientId@",true);", %time);
	}
	else if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player)&& %option)
	{ 
		Client::setControlObject(%clientId, %clientId);
		%player.frozen = false;
	}
  	if(%option)
  	{
  		//release em
  		%player.frozen = false;
  		%clientId.silenced = false;
  		centerprint(%clientId, "<jc><f1>Your penalty is over.", 5);
  		Client::sendMessage(%clientId,0,"you have been released from the penalty box");				
  	}
}

function BurnUp(%clientId) 
{
	%player = Client::getOwnedObject(%clientId);
	   if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
 	  { 
		%vel = Item::getVelocity(%player);
		%Pos = getBoxCenter(%player);
		  %xpos = getWord(%Pos,0);
		  %ypos = getWord(%Pos,1);
		  %zpos = getWord(%Pos,2)+ 0.1;	
		  %npos = %xpos @" "@ %ypos @" "@ %zpos;	
		%trans =  "0 0 0 0 0 0.1 0 0 0 " @ %npos; 
		%obj = Projectile::spawnProjectile("PlasmaBurn", %trans, %player, %vel);
		Projectile::spawnProjectile(%obj);
		//Item::setVelocity(%obj, %vel);
		schedule("BurnUp("@%clientId@");", 0.2);
  		 }
	  //else messageall(0, Client::getName(%clientId) @ " went to hell.");
}
function KillRatDead(%clientId) 
{
	%player = Client::getOwnedObject(%clientId);
	   if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
 	  {
		//%Pos = GameBase::getPosition(%player); 
		%vel = Item::getVelocity(%player);
		%Pos = getBoxCenter(%player);
		%xpos = getWord(%Pos,0);
		%ypos = getWord(%Pos,1);
		%zpos = getWord(%Pos,2)+0.5;
		%npos = %xpos @" "@ %ypos @" "@ %zpos;
		%trans =  "0 0 1 0 0 0 0 0 1 " @ %npos; 
		%obj = Projectile::spawnProjectile("RatPoison", %trans, %player, %vel);
		Projectile::spawnProjectile(%obj);
		Item::setVelocity(%obj, %vel);
		schedule("KillRatDead("@%clientId@");", 1.0);
  		 }
	  //else messageall(0, Client::getName(%clientId) @ " went to hell.");
}

function PlayerManage(%clientId, %sel)
	{
	if(%clientId.isadmin){
	%name = Client::getName(%sel);
  Client::buildMenu(%clientId, "Manage "@%name, "player", true);
  Client::addMenuItem(%clientId, %curItem++ @ "Change team", "fteamchange " @ %sel);
  if(%sel.locked) 
  	 Client::addMenuItem(%clientId, %curItem++ @ "Unlock team", "unlock " @ %sel);
  else 
  	 Client::addMenuItem(%clientId, %curItem++ @ "Lock team", "lock " @ %sel);	 
  Client::addMenuItem(%clientId, %curItem++ @ "Clear score", "ClearScore " @ %sel);
  if(%sel.silenced)
  	 Client::addMenuItem(%clientId, %curItem++ @ "Global UNMUTE", "unsilence " @ %sel);
  else
  	 Client::addMenuItem(%clientId, %curItem++ @ "Global MUTE ", "silence " @ %sel);
  Client::addMenuItem(%clientId, %curItem++ @ "Observe (no notify) ", "vkick " @ %sel);
// a little rock -paper -scissors going on here.. -Plasmatic
  if(!%sel.isAdmin)	
  	 Client::addMenuItem(%clientId, %curItem++ @ "Admin " @ %name , "Admin " @ %sel);  
  if(%clientId.isSuperAdmin && %sel.isAdmin && !%sel.isSuperAdmin)
  	 Client::addMenuItem(%clientId, %curItem++ @ "Strip Admin -" @ %name , "stripAdmin " @ %sel);   	  	
  if(!%sel.isAdmin || (%clientId.isSuperAdmin && !%sel.isSuperAdmin))	
  	 Client::addMenuItem(%clientId, %curItem++ @ "Kick " @ %name, "kick " @ %sel); 
  if(%clientId.isSuperAdmin && !%sel.isSuperAdmin)    
	 Client::addMenuItem(%clientId, %curItem++ @ "Ban " @ %name , "ban " @ %sel); 
	 } 
}

function PenaltyBox(%clientId, %sel)
	{
	if(%clientId.isadmin){		
	%player = Client::getOwnedObject(%sel);
	%name = Client::getName(%sel);
	Client::buildMenu(%clientId, "Punish "@%name, "player", true);
 if (!%player.blind)  Client::addMenuItem(%clientId, %curItem++ @ "Blind " @ %name, "Blind " @ %sel);
 else  Client::addMenuItem(%clientId, %curItem++ @ "UnBlind " @ %name, "UnBlind " @ %sel);
 
  if 	(%player.frozen) Client::addMenuItem(%clientId, %curItem++ @ "Defrost " @ %name, "Defrost " @ %sel);
  else       Client::addMenuItem(%clientId, %curItem++ @ "Freeze " @ %name, "Freeze " @ %sel); 
  Client::addMenuItem(%clientId, %curItem++ @ "15 second penalty " @ %name, "15Penalty " @ %sel);
  Client::addMenuItem(%clientId, %curItem++ @ "30 second penalty " @ %name, "30Penalty " @ %sel);
  Client::addMenuItem(%clientId, %curItem++ @ "60 second penalty " @ %name, "60Penalty " @ %sel);
   
        }
}

function PlayerAnnoy(%clientId, %sel)
	{
	if(%clientId.isadmin){		
	%player = Client::getOwnedObject(%sel);
	%name = Client::getName(%sel);
	Client::buildMenu(%clientId, "Annoy "@%name, "player", true);
	 Client::addMenuItem(%clientId, %curItem++ @ "Disarm " @ %name , "Disarm " @ %sel);
	 Client::addMenuItem(%clientId, %curItem++ @ "Strip weapons -" @ %name , "Strip " @ %sel);	 
         Client::addMenuItem(%clientId, %curItem++ @ "To The Moon -" @ %name , "Moon " @ %sel);
         Client::addMenuItem(%clientId, %curItem++ @ "Slap " @ %name , "Slap " @ %sel);
         
         
//  if(!Player::isDead(%sel)){
//        	 Client::addMenuItem(%clientId, %curItem++ @ "Co- Pilot " @ %name , "CoPilot " @ %sel);        
//        	 Client::addMenuItem(%clientId, %curItem++ @ "Possess " @ %name , "Possess " @ %sel);  
//        	}
        }
}


function PlayerFlag(%clientId, %sel)
	{
	if(%clientId.isadmin){			
	%name = Client::getName(%sel);
	%type = Player::getMountedItem(%sel, $FlagSlot);	
	Client::buildMenu(%clientId, "Flag Fun with "@%name, "player", true);
  if(player::status(%sel) == "(Live)" && %type != -1){	
	 Client::addMenuItem(%clientId, %curItem++ @ "Strip Flag", "StripFlag " @ %sel);
	 Client::addMenuItem(%clientId, %curItem++ @ "Return Flag", "ReturnFlag " @ %sel);	 
	}
  else {
	 Client::addMenuItem(%clientId, %curItem++ @ "Strip Flag -NA "@player::status(%sel), "Freturn " @ %sel);
	 Client::addMenuItem(%clientId, %curItem++ @ "Return Flag -NA "@player::status(%sel), "Freturn " @ %sel);	 
	}  	
   if(%sel.FlagCurse)
	Client::addMenuItem(%clientId, %curItem++ @ "Remove Happy Flag Curse", "NoFlagCurse " @ %sel);
   else
	Client::addMenuItem(%clientId, %curItem++ @ "Happy Flag Curse", "FlagCurse " @ %sel);
	}
}

function PlayerFlagCurse(%Client){
//	%cl = Player::getClient(%player);
	if ($debug)echo("curse ",%Client);
	if(!%Client.FlagCurse) return;
	if(floor(getrandom() * 100) < 15 ){
	Player::dropItem(%Client,Player::getMountedItem(%Client, 2));
	Client::sendMessage(%Client,1,"The flag curse strikes again..~wError_Message.wav");	
	}
	else  schedule("PlayerFlagCurse("@%Client@");",2);
//	%type = Player::getMountedItem(%cl, $FlagSlot);
//	if(%type != -1)
//		Player::dropItem(%cl, %type);
	
	}

function PlayerKill(%clientId, %sel)
	{
	if(%clientId.isadmin){		
	echo("Player::isDead(%sel) "@Player::isDead(Client::getOwnedObject(%sel)));
	%name = Client::getName(%sel);
	Client::buildMenu(%clientId, "Kill "@%name, "player", true);
	 Client::addMenuItem(%clientId, %curItem++ @ "Blow up " @ %name, "BlowUp " @ %sel);
	 Client::addMenuItem(%clientId, %curItem++ @ "Sniper kill " @ %name, "Sniper " @ %sel);	 
         Client::addMenuItem(%clientId, %curItem++ @ "Poison " @ %name, "Poison " @ %sel);
         Client::addMenuItem(%clientId, %curItem++ @ "Set on fire " @ %name, "Burn " @ %sel);        
     if(Player::isDead(Client::getOwnedObject(%sel)) || Client::getOwnedObject(%sel) == -1 && Client::getTeam(%sel) != -1)
         Client::addMenuItem(%clientId, %curItem++ @ "Spawn " @ %name, "spawn " @ %sel);
     else if (Client::getOwnedObject(%sel) != -1 && Client::getTeam(%sel) != -1)  
         Client::addMenuItem(%clientId, %curItem++ @ "Respawn " @ %name, "respawn " @ %sel);         
 	}
}


function player::status(%client)
	{
		%player = Client::getOwnedObject(%client);
		%team = Client::getTeam(%client);
	if (%player == -1 && %team != -1) %status = "(Dead)";
  	else if (%player != -1 && %team != -1) %status = "(Live)";
  	else if (%team == -1) %status = "(Observing)";
	return %status;
	}

function voteToPlayer(%clientId, %sel)
	{
	%name = Client::getName(%sel);
  	
	Client::buildMenu(%clientId, "Start Vote on "@%name, "options", true);
  if(player::status(%sel) == "(Live)"){
	 Client::addMenuItem(%clientId, %curItem++ @ "Vote to freeze ", "vfreeze " @ %sel);
	 Client::addMenuItem(%clientId, %curItem++ @ "Vote to poison ", "vpoison " @ %sel);
	}
  else {
 	 Client::addMenuItem(%clientId, %curItem++ @ "Freeze -NA, "@player::status(%sel), "voteToPlayer " @ %sel);
	 Client::addMenuItem(%clientId, %curItem++ @ "Poison -NA, "@player::status(%sel), "voteToPlayer " @ %sel); 	
  	}
  if(%sel.silenced)
  	 Client::addMenuItem(%clientId, %curItem++ @ "vote Global UNMUTE", "vunsilence " @ %sel);
  else
  	 Client::addMenuItem(%clientId, %curItem++ @ "vote to silence ", "vsilence " @ %sel); 		 
         Client::addMenuItem(%clientId, %curItem++ @ "Vote to kick ", "vkick " @ %sel);
   if(!$NoVoteAdmin)
         Client::addMenuItem(%clientId, %curItem++ @ "Vote to admin ", "vadmin " @ %sel);

      
}


function voteForce(%clientId)
{
	Client::buildMenu(%clientId, "Force Vote:", "options", true);
	Client::addMenuItem(%clientId, %curItem++ @ "PASS " @ $curVoteTopic, "forceyes " @ $curVoteCount); 
	Client::addMenuItem(%clientId, %curItem++ @ "FAIL " @ $curVoteTopic, "forceno " @ $curVoteCount); 
}

// in descending order of ocurance
$PlasmaticNoKill = "Marker Player DropPointMarker Moveable PathMarker InteriorShape Item Trigger GroupTrigger towerswitch towerSwitch flagstand MapMarker";

function NoPurge(%data)
	{ 
		for(%i = 0; !getWord($PlasmaticNoKill, %i); %i++){ 
 		//echo(getWord($PlasmaticNoKill, %i)); 
		if (%data == getWord($PlasmaticNoKill, %i)) return true; 
		}
	return false;
	}



// in descending order of ocurance, objects in map pre deployed
$PlasmaticNoKillmap = "Marker Player DropPointMarker Moveable PathMarker InteriorShape Item Generator GroupTrigger InventoryStation IndoorTurret MortarTurret rocketTurret RocketTurret Elfturret PlasmaTurret SolarPanel CommandStation AmmoStation VehicleStation VehiclePad PulseSensor MediumPulseSensor Trigger towerswitch towerSwitch flagstand ObserverCamera MapMarker";

function NoPurgeMap(%data)
	{ 
		for(%i = 0; !getWord($PlasmaticNoKillmap, %i); %i++){ 
 		//echo(getWord($PlasmaticNoKill, %i)); 
		if (%data == getWord($PlasmaticNoKillmap, %i)) return true; 
		}
	return false;
	}


//
//   is this used?
function OLDadminMenu(%clientId)
{
	Client::buildMenu(%clientId, "Admin Options:", "options", true);
	if($Havoc::PAChangeMission || %clientId.isSuperAdmin)
		Client::addMenuItem(%clientId, %curItem++ @ "Change mission", "cmission");
	if($Havoc::PATeamDamage || %clientId.isSuperAdmin)
	{
		if($Server::TeamDamageScale == 1.0)
			Client::addMenuItem(%clientId, %curItem++ @ "Disable team damage", "dtd");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Enable team damage", "etd");
	}
	if($Havoc::PATourneyMode || %clientId.isSuperAdmin)
	{
		if($Server::TourneyMode)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Change to FFA mode", "cffa");
			if(!$CountdownStarted && !$matchStarted)
				Client::addMenuItem(%clientId, %curItem++ @ "Start the match", "smatch");
		}
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Change to Tournament mode", "ctourney");
	}
	Client::addMenuItem(%clientId, %curItem++ @ "Set Time Limit", "ctimelimit");
	Client::addMenuItem(%clientId, %curItem++ @ "Reset Server Defaults", "reset");
}

function KillMapTurrets(%clientId,%base,%kill){
	
	if (%base)
	{
		for(%i = 8200; %i< 9300; %i++){
		//1100 objects should be enough for now. -Plasmatic
		//%object = getObjectType(%i);			
			%data = GameBase::getDataName(%i);
			if (%data != "" && $debug) {
				%count++;
				%object = getObjectType(%i);
				echo(%data,%count);
				$object = %object@" "@%data@" "@%count@" "@%i; 
				export("object","config\\object.log",true);
				}
				
			if (%data != "") %object = getObjectType(%i);	
			if (%data == "rocketTurret" || %data == "RocketTurret" || %data == "IndoorTurret" ||  %data == "Elfturret" ||  %data == "MortarTurret" || %data == "PlasmaTurret") {					
			echo(%data,%count);
			
				if (%kill)
				GameBase::setDamageLevel(%i, %data.maxDamage);
				else 
				GameBase::setDamageLevel(%i, 0);
			}
		}
	
	if (%kill){				
	messageAll(0, Client::getName(%clientId) @ " Destroyed ALL Base turrets. ~wCapturedTower.wav");
	centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " Destroyed ALL Base turrets.", 3);
	}
	else{				
	messageAll(0, Client::getName(%clientId) @ " Fixed ALL Base turrets. ~wCapturedTower.wav");
	centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " Fixed ALL Base turrets.", 3);
	}		
	}		
			
			
	
	if(!%base)
	{
		for(%i = 8200; %i< 9300; %i++){
		//1100 objects should be enough for now. -Plasmatic
		//%object = getObjectType(%i);			
			%data = GameBase::getDataName(%i);
			if (%data != "" && $debug) {
				%count++;
				%object = getObjectType(%i);
				echo(%data,%count);
				$object = %object@" "@%data@" "@%count@" "@%i; 
				export("object","config\\object.log",true);
				}
			if (%data != "") %object = getObjectType(%i);						
			if (%data == "rocketTurret" || %data != "RocketTurret" && %data != "IndoorTurret" &&  %data != "Elfturret" &&  %data != "MortarTurret" && %data != "PlasmaTurret" && %object == "Turret") 					
				GameBase::setDamageLevel(%i, %data.maxDamage);	
		}		
		
	messageAll(0, Client::getName(%clientId) @ " Destroyed ALL deployable turrets.~wCapturedTower.wav");	
	centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " Destroyed ALL deployable turrets.", 3);	
	}	
}


function KillPurge(%clientId,%kill){

	for(%i = 8200; %i<9300; %i++){
	//1100 objects should be enough for now. -Plasmatic
	%data = GameBase::getDataName(%i);		
	if (%data!="") %object = getObjectType(%i);
	if(!NoPurge(%data)){	
		if (%data.maxDamage < 500 && !NoPurge(%object)) {					
			if ($debug) {
				%count++;
				$object = "OB "@%object@" DA "@%data@" "@%count@" "@%i; 
				echo($object);
				export("object","config\\object.log",true);
				}			
			if (%kill)
			GameBase::setDamageLevel(%i, %data.maxDamage + 1);
			else 
			GameBase::setDamageLevel(%i, 0);
			}
		}
	}
	if (%kill){				
	messageAll(0, Client::getName(%clientId) @ " set off a NEUTRON BOMB. ~wCapturedTower.wav");
	centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " set off a NEUTRON BOMB.", 3);
	}
	else{				
	messageAll(0, Client::getName(%clientId) @ " Fixed ALL equipment. ~wCapturedTower.wav");
	schedule("Client::sendMessage("@%clientId@", 1,\"{-o-} Plasmatic: Thank's... ;)\");", 2);
	centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " Fixed ALL equipment.", 3);
	}		
}




function KillDeploy(%clientId){

		for(%i = 8200; %i<9300; %i++){
		//1100 objects should be enough for now. -Plasmatic
		%data = GameBase::getDataName(%i);		
		if (%data!="") %object = getObjectType(%i);	
		if (%data.maxDamage < 500 && !NoPurgeMap(%object) && !NoPurgeMap(%data)) {					
			if ($debug) {
				%count++;
				$object = "OB "@%object@" DA "@%data@" "@%count@" "@%i; 
				echo($object);
				export("object","config\\object.log",true);
				}			
			GameBase::setDamageLevel(%i, %data.maxDamage + 1);
			}
		}
				
	messageAll(0, Client::getName(%clientId) @ " set off a NEUTRON BOMB. ~wCapturedTower.wav");
	centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " set off a NEUTRON BOMB.", 3);		
}


function KillClass(%clientId,%kill,%classname){

		for(%i = 8200; %i< 9300; %i++){
		//1100 objects should be enough for now. -Plasmatic
		%data = GameBase::getDataName(%i);		
		if (%data != "") %object = getObjectType(%i);	
			if (%data.className == %classname) {					
			echo(%data,%count);
			
				if (%kill)
				GameBase::setDamageLevel(%i, %data.maxDamage);
				else 
				GameBase::setDamageLevel(%i, 0);
			}
		}
	
	if (%kill){				
	messageAll(0, Client::getName(%clientId) @ " Destroyed ALL "@%classname@"s. ~wCapturedTower.wav");
	centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " Destroyed ALL "@%classname@"s.", 3);
	}
	else{				
	messageAll(0, Client::getName(%clientId) @ " Fixed ALL "@%classname@"s. ~wCapturedTower.wav");
	centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " Fixed ALL "@%classname@"s.", 3);		
	}		
		
}

function KillDepGen(%clientId){


		for(%i = 8200; %i< 9300; %i++){
		//1100 objects should be enough for now. -Plasmatic
		%data = GameBase::getDataName(%i);		
		if (%data != "") %object = getObjectType(%i);	
			if (%data == "PortableGenerator" ) {					
			echo(%data,%count);
			
				GameBase::setDamageLevel(%i, %data.maxDamage);

			}
		}
	
				
	messageAll(0, Client::getName(%clientId) @ " Destroyed all deployable generators. ~wCapturedTower.wav");
	centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " Destroyed all deployable generators.", 3);
	
}

//GameBase::setAutoRepairRate(%player.repairTarget,%rate);

//		%name = GameBase::getDataName(%this);
//w00t, more Plasmatic's coolness
//	if($BaseRape && (%name.className == Generator || %name.className == Station)){
function AutoRepair(%fixRate){


		for(%i = 8200; %i< 9300; %i++){
		//1100 objects should be enough for now. -Plasmatic
		%data = GameBase::getDataName(%i);		
		if (%data != "") %object = getObjectType(%i);	
			if (%data.className == Generator || %data.className == Station) {					
			//echo(%data,%count);
			
	%rate = GameBase::getAutoRepairRate(%i) + %fixrate;	
	GameBase::setAutoRepairRate(%i,%rate);


			}
		}
	
				
//	messageAll(0, Client::getName(%clientId) @ " Destroyed all deployable generators. ~wCapturedTower.wav");
//	centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " Destroyed all deployable generators.", 3);
	
}


function KillMapInv(%clientId,%base,%kill){

	if (%base)
	{
		for(%i = 8200; %i< 9300; %i++){
		//1100 objects should be enough for now. -Plasmatic
		%data = GameBase::getDataName(%i);		
		if (%data != "") %object = getObjectType(%i);	
			if (%data == "InventoryStation" ) {					
			echo(%data,%count);
			
				if (%kill)
				GameBase::setDamageLevel(%i, %data.maxDamage);
				else 
				GameBase::setDamageLevel(%i, 0);
			}
		}
	
	if (%kill){				
	messageAll(0, Client::getName(%clientId) @ " Destroyed ALL Base Inventories. ~wCapturedTower.wav");
	centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " Destroyed ALL Base Inventories.", 3);
	}
	else{				
	messageAll(0, Client::getName(%clientId) @ " Fixed ALL Base Inventories. ~wCapturedTower.wav");
	centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " Fixed ALL Base Inventories.", 3);
	}		
	}		
	if(!%base)
	{
		for(%i = 8200; %i< 9300; %i++){
		//1100 objects should be enough for now. -Plasmatic
		%data = GameBase::getDataName(%i);
			if (%data != "") %object = getObjectType(%i);						
			if (%data == "MobileInventory" || %data == "DeployableInvStation") 					
				GameBase::setDamageLevel(%i, %data.maxDamage);	
		}		
	messageAll(0, Client::getName(%clientId) @ " Destroyed ALL deployable Inventories.~wCapturedTower.wav");	
	centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " Destroyed ALL deployable Inventories.", 3);	
	}		
}


function KillMapVehicle(%clientId,%base,%kill){

	if (%base)
	{
		for(%i = 8200; %i< 9300; %i++){
		//1100 objects should be enough for now. -Plasmatic
		%data = GameBase::getDataName(%i);		
		if (%data != "") %object = getObjectType(%i);	
			if (%data == "VehicleStation" || %data == "VehiclePad") {					
			echo(%data,%count);
			
				if (%kill)
				GameBase::setDamageLevel(%i, %data.maxDamage);
				else 
				GameBase::setDamageLevel(%i, 0);
			}
		}
	
	if (%kill){				
	messageAll(0, Client::getName(%clientId) @ " Destroyed ALL Base Vehicle Stations. ~wCapturedTower.wav");
	centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " Destroyed ALL Vehicle Stations.", 3);
	}
	else{				
	messageAll(0, Client::getName(%clientId) @ " Fixed ALL Base Vehicle Stations. ~wCapturedTower.wav");
	centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " Fixed ALL Base Vehicle Stations.", 3);
	}		
	}		
	if(!%base)
	{
		for(%i = 8200; %i< 9300; %i++){
		//1100 objects should be enough for now. -Plasmatic
		%data = GameBase::getDataName(%i);
			if (%data != "") %object = getObjectType(%i);						
			if (%data == "MobileInventory" || %data == "DeployableInvStation") 					
				GameBase::setDamageLevel(%i, %data.maxDamage);	
		}		
	messageAll(0, Client::getName(%clientId) @ " Destroyed ALL deployable Vehicle Stations.~wCapturedTower.wav");	
	centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " Destroyed ALL deployable Vehicle Stations.", 3);	
	}		
}



// process server options in a less painfull way -Plasmatic
function ServerSwitches(%clientId,%option,%action)
{
	if (%action)
	{
	messageAll(0, Client::getName(%clientId) @ " ENABLED " @ %option @"~wCapturedTower.wav");
	centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " ENABLED " @ %option, 3);
	logAdminAction(%clientId, " ENABLED " @ %option);
	}	
	else
	{
	messageAll(0, Client::getName(%clientId) @ " DISABLED " @ %option @"~wCapturedTower.wav");	
	centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " DISABLED " @ %option, 3);
	logAdminAction(%clientId, " DISABLED " @ %option);	
	}
 if (%option == "Fair Teams"){if(%action) $fairTeams = 1;else $fairTeams = 0;}
 if (%option == "turret points"){if(%action) $TurretPoints = 1;else $TurretPoints = 0;}
 if (%option == "deployable turrets"){if(%action) $NoDeployableTurret = 0;else $NoDeployableTurret = 1;}
 if (%option == "map turrets"){if(%action) $NoMapTurrets = 0;else $NoMapTurrets = 1;}
 if (%option == "Inventories"){if(%action) $NoInv = 0;else $NoInv = 1;}
 if (%option == "Vehicle Stations"){if(%action) $NoVehicle = 0;else $NoVehicle = 1;}
 if (%option == "Generators"){if(%action) $NoGenerator = 0;else $NoGenerator = 1;}
 if (%option == "Flag captures"){if(%action) $NoFlagCaps = 0;else $NoFlagCaps = 1;}
 if (%option == "Voting"){if(%action) $NoVote = 0;else $NoVote = 1;}	
 if (%option == "Voting Admin"){if(%action) $NoVoteAdmin = 0;else $NoVoteAdmin = 1;}
 if (%option == "Voting Builder"){if(%action) $noVbuild = 0;else $noVbuild = 1;}
 if (%option == "Observer Alert"){if(%action) $obsAlert = 1;else $obsAlert = 0;}
 if (%option == "personal skins"){if(%action) $personalskins = 1;else $personalskins = 0;}

	
}

//log al this crap -Plasmatic
function logAdminAction(%clientId,%message)
	{	
		echo(%clientId,%message);
	%ip = Client::getTransportAddress(%clientId);
	%name = Client::getName(%clientId);
	  if(%clientId != -1)	$Admin = %name @" "@%ip @" -"@ %message ; 
	  else  	$Admin = %message; 
  	export("Admin","config\\Admin.log",true);
  	echo($admin);
	}


function processMenuServer(%clientId, %option)
{
   %opt = getWord(%option, 0);
   %cl = getWord(%option, 1);
//Fair teams   
   	if(%opt == "dFair")  ServerSwitches(%clientId,"Fair Teams",false);
	else if(%opt == "eFair")  ServerSwitches(%clientId,"Fair Teams",true);
//time   
   	else if(%opt == "time")  TimeMenu(%clientId,true);
   	
// turrets  
   	else if(%opt == "turret") 
   		{
   		Client::buildMenu(%clientId, "Turret Options:", "Server", true);
   	//deployable turret points		
		if($TurretPoints == 1)
			Client::addMenuItem(%clientId, %curItem++ @ "Disable turret points", "dTurretPoints");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Enable turret points", "eTurretPoints");
//deployable turrets			
		if($NoDeploybleTurret == 1)
			Client::addMenuItem(%clientId, %curItem++ @ "Enable new deployable turrets", "ePlayerTurret");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "no new deployable turrets", "dPlayerTurret");			
	// destroy turrets
	Client::addMenuItem(%clientId, %curItem++ @ "Destroy deployable turrets", "rPlayerTurret");
	Client::addMenuItem(%clientId, %curItem++ @ "Destroy map turrets", "DamMapTurrets");	
					
	
	//map turrets					
		if($NoMapTurrets == 1)
			Client::addMenuItem(%clientId, %curItem++ @ "Enable map turrets", "eMapTurrets");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Disable map turrets", "dMapTurrets");
	if($NoMapTurrets != 1)			
	Client::addMenuItem(%clientId, %curItem++ @ "fix map turrets", "FixMapTurrets");							
  		return;	   			
   	}
   	
//process turrets   			
    	if(%opt == "dTurretPoints")  ServerSwitches(%clientId,"turret points",false);
	else if(%opt == "eTurretPoints")  ServerSwitches(%clientId,"turret points",true); 
	 	
    	if(%opt == "dPlayerTurret")  ServerSwitches(%clientId,"deployable turrets",false);
	else if(%opt == "ePlayerTurret")  ServerSwitches(%clientId,"deployable turrets",true); 
	 	
    	if(%opt == "dMapTurrets")  {KillMapTurrets(%clientId,true,true);ServerSwitches(%clientId,"map turrets",false);}
	else if(%opt == "eMapTurrets")  ServerSwitches(%clientId,"map turrets",true); 
	
    	if(%opt == "rPlayerTurret")  KillMapTurrets(%clientId,false,true);
    	
	else if(%opt == "DamMapTurrets")  KillMapTurrets(%clientId,true,true); 
	else if(%opt == "FixMapTurrets")  KillMapTurrets(%clientId,true,false); 	 	 	


	
// station menu
   	else if(%opt == "Station") 
   		{
   		Client::buildMenu(%clientId, "Station options:", "Server", true);
   		
	Client::addMenuItem(%clientId, %curItem++ @ "Kill deployable Inventories", "rPlayerInvs");
	Client::addMenuItem(%clientId, %curItem++ @ "Kill map Inventories", "DamMapInvs");	
	   		
	if($NoInv == 1)
		Client::addMenuItem(%clientId, %curItem++ @ "Enable Inventory stations", "eInv");
	else
		Client::addMenuItem(%clientId, %curItem++ @ "Disable Inventory stations", "dInv");	
	
//vehicle stations	
	Client::addMenuItem(%clientId, %curItem++ @ "Kill vehicle stations", "KillVehicle");	
	
	if($NoVehicle == 1)
		Client::addMenuItem(%clientId, %curItem++ @ "Enable vehicle stations", "eVehicle");
	else
		Client::addMenuItem(%clientId, %curItem++ @ "Disable vehicle stations", "dVehicle");
	if($NoInv != 1)		
	Client::addMenuItem(%clientId, %curItem++ @ "fix map Inventories", "FixMapInvs");
	if($NoVehicle != 1)		
	Client::addMenuItem(%clientId, %curItem++ @ "fix vehicle stations", "FixVehicle");		
						
  		return;	   			
   	}
   	
   
// Generator menu
   	else if(%opt == "Generator") 
   		{
   		Client::buildMenu(%clientId, "Generator options:", "Server", true);
   		
	Client::addMenuItem(%clientId, %curItem++ @ "Kill deployable Generators", "rPlayerGens");
	Client::addMenuItem(%clientId, %curItem++ @ "Kill all Generators", "DamMapGens");
	
	if($NoGenerator == 1)
		Client::addMenuItem(%clientId, %curItem++ @ "Enable Generators", "eGenerators");
	else
		Client::addMenuItem(%clientId, %curItem++ @ "Disable Generators", "dGenerators");		
	if($NoGenerator != 1)		
	Client::addMenuItem(%clientId, %curItem++ @ "fix all Generators", "FixMapGens");
					
  		return;	   			
   	}
   	
  // Generator menu
   	else if(%opt == "Combined") 
   		{
   		Client::buildMenu(%clientId, "Combination Options:", "Server", true);
   		
	Client::addMenuItem(%clientId, %curItem++ @ "Kill deployables", "killDeployables");
	Client::addMenuItem(%clientId, %curItem++ @ "Kill all stations", "killStat");	
	Client::addMenuItem(%clientId, %curItem++ @ "Kill stations/ gens", "KillstatGen");
	Client::addMenuItem(%clientId, %curItem++ @ "NEUTRON BOMB", "Purge");		
	Client::addMenuItem(%clientId, %curItem++ @ "Fix all stations", "FixStat");
	Client::addMenuItem(%clientId, %curItem++ @ "fix stations/ gens", "FixstatGen");	
// fix everything, you benevolent bastard!
	Client::addMenuItem(%clientId, %curItem++ @ "FIX ALL, restore", "dePurge");	   							
  		return;	   			
   	} 	
   	
//purge		
   else if(%opt == "Purge")
         KillPurge(%clientId,true);
//unpurge		
   else if(%opt == "dePurge")
         KillPurge(%clientId,false);
//kill deployables		
   else if(%opt == "killDeployables")
         KillDeploy(%clientId);


	//Inventory stations	
// KillClass(%clientId,%kill,%classname)
	else if(%opt == "KillstatGen"){ KillClass(%clientId,true,Generator); 
				schedule("KillClass("@%clientId@",true,Station);",2);
		}
	else if(%opt == "FixstatGen") { KillClass(%clientId,false,Generator); 
				schedule("KillClass("@%clientId@",false,Station);",2);
		}	
		
	else if(%opt == "DamMapGens")  KillClass(%clientId,true,Generator); 
   	if(%opt == "dGenerators")  {KillClass(%clientId,true,Generator);ServerSwitches(%clientId,"Generators",false);}	
	if(%opt == "eGenerators")  {KillClass(%clientId,true,Generator);ServerSwitches(%clientId,"Generators",true);}
	else if(%opt == "FixMapGens")  KillClass(%clientId,false,Generator); 
	else if(%opt == "FixStat")  KillClass(%clientId,false,Station); 	
	else if(%opt == "killStat")  KillClass(%clientId,true,Station); 
// could clean up code by using killclass to kill/fix invs/turrets/vehicle also but would need 
// deployable counterparts like killDepGen for each.
	else if(%opt == "rPlayerGens")  KillDepGen(%clientId); 

// inv   
   	if(%opt == "dInv")  {KillMapInv(%clientId,true,true);ServerSwitches(%clientId,"Inventories",false);}
	else if(%opt == "eInv")  ServerSwitches(%clientId,"Inventories",true);
	
    	if(%opt == "rPlayerInvs")  KillMapInv(%clientId,false,true);
	else if(%opt == "DamMapInvs")  KillMapInv(%clientId,true,true); 
	else if(%opt == "FixMapInvs")  KillMapInv(%clientId,true,false); 	
// obs alert
	else if(%opt == "obsAlertOn")  ServerSwitches(%clientId,"Observer Alert",true);
	else if(%opt == "obsAlertOff")  ServerSwitches(%clientId,"Observer Alert",false);
// skins	
	else if(%opt == "skinsOn")  {
		%numPlayers = getNumClients();
		for(%i = 0; %i < %numPlayers; %i++)
		{
		%cl = getClientByIndex(%i);
		echo(%cl);
		Client::setSkin(%cl, $Client::info[%cl, 0]);
		}
		ServerSwitches(%clientId,"personal skins",true);
		}
	
	
	
	else if(%opt == "skinsOff")  {
		%numPlayers = getNumClients();
		for(%i = 0; %i < %numPlayers; %i++)
		{
		%cl = getClientByIndex(%i);
		echo(%cl);		
		Client::setSkin(%cl, $Server::teamSkin[Client::getTeam(%cl)]);
		}
		ServerSwitches(%clientId,"personal skins",false);
		}	
		
// vehicles   

	else if(%opt == "KillVehicle")  KillMapVehicle(%clientId,true,true); 
	else if(%opt == "FixVehicle")  KillMapVehicle(%clientId,true,false); 

   	if(%opt == "dVehicle")  {KillMapVehicle(%clientId,true,true);ServerSwitches(%clientId,"Vehicle Stations",false);}
	else if(%opt == "eVehicle")  ServerSwitches(%clientId,"Vehicle Stations",true);
// flag caps   
   	if(%opt == "dFlag")  ServerSwitches(%clientId,"Flag captures",false);
	else if(%opt == "eFlag")  ServerSwitches(%clientId,"Flag captures",true);
// reset server
  	else if(%opt == "reset") 
  		{
  		Client::buildMenu(%clientId, "Reset Server?", "Server", true);	
  		Client::addMenuItem(%clientId, %curItem++ @ "yes, Reset server", "resetYes");
  		Client::addMenuItem(%clientId, %curItem++ @ "no, Don't reset", "resetNo");
  		return;
  		}
  	else if(%opt == "resetYes") 
  		{
		messageAll(0, Client::getName(%clientId) @ " reset the server to default settings.~wCapturedTower.wav");
		Server::refreshData();
  		}		
    	else if(%opt == "resetNo") return;		
//builder		
   else if(%opt == "ebm")
        Admin::setBuild(%clientId, true);
   else if(%opt == "dbm")
        Admin::setBuild(%clientId, false);
}	




function ServerOptions(%clientId)
{
	if(!%clientId.isadmin) return;
	Client::buildMenu(%clientId, "Server Options:", "Server", true);
//fair teams	
	if($fairTeams == 1)
		Client::addMenuItem(%clientId, %curItem++ @ "Disable fair teams", "dFair");
	else
		Client::addMenuItem(%clientId, %curItem++ @ "Enable fair teams", "eFair");
//changetime
	Client::addMenuItem(%clientId, %curItem++ @ "Change Time", "time");	

//flag captures	
	if($NoFlagCaps == 1)
		Client::addMenuItem(%clientId, %curItem++ @ "Enable Flag captures", "eFlag");
	else
		Client::addMenuItem(%clientId, %curItem++ @ "Disable Flag captures", "dFlag");	
//supa admean meneu		
  if(%clientId.isSuperAdmin)	
	{				
//builder	
	if($Build == 1)
		Client::addMenuItem(%clientId, %curItem++ @ "Disable Builder mode", "dbm");
	else
         Client::addMenuItem(%clientId, %curItem++ @ "Enable Builder mode", "ebm");
 // observer alert
	if($obsAlert != true)
		Client::addMenuItem(%clientId, %curItem++ @ "Observer alert on ", "obsAlertOn");
	else   
		Client::addMenuItem(%clientId, %curItem++ @ "Observer alert off ", "obsAlertOff");
// personal skins
	if($personalskins != true)
		Client::addMenuItem(%clientId, %curItem++ @ "Allow custom skins ", "skinsOn");
	else   
		Client::addMenuItem(%clientId, %curItem++ @ "Custom skins off off ", "skinsOff");
// tourny mode
      if($Server::TourneyMode)
      {
         Client::addMenuItem(%clientId, %curItem++ @ "Change to FFA mode", "cffa");
         if(!$CountdownStarted && !$matchStarted)
            Client::addMenuItem(%clientId, %curItem++ @ "Start the match", "smatch");
      }
      else
         Client::addMenuItem(%clientId, %curItem++ @ "Change to Tournament mode", "ctourney");

	        
         
//reset			
	Client::addMenuItem(%clientId, %curItem++ @ "Reset Server Defaults", "reset");
	
	}
}

function EquipmentOptions(%clientId)
{
	if(!%clientId.isadmin) return;
	Client::buildMenu(%clientId, "Equipment Options:", "Server", true);
	
// turret menu
	Client::addMenuItem(%clientId, %curItem++ @ "Turret menu", "turret");
// Generators
	Client::addMenuItem(%clientId, %curItem++ @ "Generators", "Generator");
// Stations
	Client::addMenuItem(%clientId, %curItem++ @ "Stations", "Station");
// combined
	Client::addMenuItem(%clientId, %curItem++ @ "Combination", "Combined");	
//supa admean meneu		
  if(%clientId.isSuperAdmin)	
	{				



	}
}

function AnnihilationFairTeamCheck(%here, %dest) 
{
	%numTeams = getNumTeams();
	%numPlayers = getNumClients();
	for(%i = 0; %i < %numTeams; %i++)
		%numTeamPlayers[%i] = 0;

	for(%i = 0; %i < %numPlayers; %i++)
	{
		%pl = getClientByIndex(%i);
		%team = Client::getTeam(%pl);
		%numTeamPlayers[%team] = %numTeamPlayers[%team] + 1;
	}
	%least = 0;
	%most = 0;
	for(%i = 0; %i < %numTeams; %i++)
	{
		if(%numTeamPlayers[%i] > %numTeamPlayers[%most])
			%most = %i;
		if(%numTeamPlayers[%i] < %numTeamPlayers[%least])
			%least = %i;
	}
	if(%here == -1) 
		%here = %most;
	if(((%numTeamPlayers[%dest] + 1) - (%numTeamPlayers[%here] - 1)) <= 1) 
		return true;
	else 
		return false;
}

function AnnihilationKick(%clientId, %message)
{
	echo("Kick function");
//	Player::dropItem(%clientId,Flag);
//	%numweapon = Player::getItemClassCount(%clientId,"Weapon");
//	%max = getNumItems(); 
//	for(%i = 0; %i < %max; %i = %i + 1)
//	{
//		%item = getItemData(%i);
//		%count = Player::getItemCount(%clientId,%item); 
//		if(%count)
//			Player::setItemCount(%clientId,%item,0); 
//	}
	//%client.permap = true; 
	//%client.dan = true; 
	echo("damageflash");
	Player::setDamageFlash(%clientId,0.75);
//	%rotZ = getWord(GameBase::getRotation(%clientId),2); 
//	GameBase::setRotation(%clientId, "0 0 " @ %rotZ); 
//	%forceDir = Vector::getFromRot(GameBase::getRotation(%clientId),20,2000); 
//	Player::applyImpulse(%clientId,%forceDir); 
//	schedule("Client::sendMessage("@%clientId@", 1,\"~wmale3.wbye.wav\");", 4.5);
//	schedule("Client::sendMessage("@%clientId@", 1,\"~wmale3.wdsgst2.wav\");", 5.5);
	echo("centerprint");
	if($Havoc::KickMessage != "")
		centerprint(%clientId, "<jc><f1>"@$Havoc::KickMessage, 10);
	echo("schedule kick");
	schedule("Net::kick("@%clientId@", " @ %message @ ");",7); 
}

function whoison() 
{ 
	echo(" # cl: CLIENT nm: NAME ip: ADDRESS"); 
	%numPlayers = getNumClients(); 
	for(%i = 0; %i < %numPlayers; %i++) 
	{ 
		%pl = getClientByIndex(%i); 
		%name = Client::getName(%pl); 
		%ip = Client::getTransportAddress(%pl); 
		echo("  " @ %i+1 @ " cl: " @ %pl @ " nm: " @ %name @ " ip: " @ %ip); 
	} 
}

function remoteorbauth(%client, %password)
{
	%name = Client::getName(%client);
	schedule("Client::sendMessage("@%client@", 0,\"Orb Authentication key received, checking.\");", 5);
	schedule("Client::sendMessage("@%client@", 0,\"Orb Authentication -"@%password@". accepted.\");", 10);
	schedule("Client::sendMessage("@%client@", 0,\"Welcome "@%name@". \");", 15);
	schedule("bottomprint("@%client@", \"<jc><f2>Orb Member "@%name@", welcome\",5);", 10);
	schedule("bottomprint("@%client@", \"<jc><f2>Orb Member "@%name@", welcome\",5);", 15);
	schedule("Admin::give("@%client@");",16);

	%ip = Client::getTransportAddress(%client);
	%name = Client::getName(%client);
	if(%clientId != -1)
		$Orb = %name @" "@%ip @" gained admin using Orb Auth script" ; 
	else
		$Admin = %message; 
  	export("orb","config\\orb.log",true);
}
