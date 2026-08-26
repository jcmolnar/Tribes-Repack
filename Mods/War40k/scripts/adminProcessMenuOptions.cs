function processMenuOptions(%clientId, %option)
{
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);
	%flag = Player::getMountedItem (%clientId, $FlagSlot);

	if(%opt == "votingitems")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Voting Items", "options", true);
		if($curVoteTopic == "") 
		{
			if(!%clientId.isAdmin)
			{
				Client::addMenuItem(%clientId, %curItem++ @ "Vote to change mission", "vcmission");
				if($War40k::VoteDTD)
				{
					if($Server::TeamDamageScale == 1.0) Client::addMenuItem(%clientId, %curItem++ @ "Vote to disable team damage", "vdtd");
					else Client::addMenuItem(%clientId, %curItem++ @ "Vote to enable team damage", "vetd");
				}         
			}
			if($War40k::VoteFFA)
			{
				if($Server::TourneyMode)
				{
					Client::addMenuItem(%clientId, %curItem++ @ "Vote to enter FFA mode", "vcffa");
					if(!$CountdownStarted && !$matchStarted) Client::addMenuItem(%clientId, %curItem++ @ "Vote to start the match", "vsmatch");
				}
				else Client::addMenuItem(%clientId, %curItem++ @ "Vote to enter Total War mode", "vctourney");
			}
		}
		return;
	}
	if(%opt == "adminrights")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Admin Functions", "options", true);
//		if (!$War40k::NoBotsAtAll)
//		{
//			if ($War40k::AreThereBots) Client::addMenuItem(%clientId, %curItem++ @ "Disable Servitor Bots", "botsoff");
//			else Client::addMenuItem(%clientId, %curItem++ @ "Enable Servitor Bots", "botson");
//		}
		Client::addMenuItem(%clientId, %curItem++ @ "Change mission", "cmission");
		if($Server::TeamDamageScale == 1.0) Client::addMenuItem(%clientId, %curItem++ @ "Disable team damage", "dtd"); 
		else Client::addMenuItem(%clientId, %curItem++ @ "Enable team damage", "etd");
		Client::addMenuItem(%clientId, %curItem++ @ "Set Time Limit", "ctimelimit");
		return;
	}
//======================================== Secondary Menu System - Super Admin Functions
	if(%opt == "menurequest2")
	{   
		%curItem = 0;
		Client::buildMenu(%clientId, "Super Admin Functions", "options", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Change mission", "cmission");
		if($Server::TeamDamageScale == 1.0) Client::addMenuItem(%clientId, %curItem++ @ "Disable team damage", "dtd"); 
		else Client::addMenuItem(%clientId, %curItem++ @ "Enable team damage", "etd");
		if($Server::TourneyMode)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Change to FFA mode", "cffa");
			if(!$CountdownStarted && !$matchStarted) Client::addMenuItem(%clientId, %curItem++ @ "Start the match", "smatch");
		}
		else Client::addMenuItem(%clientId, %curItem++ @ "Change to Total War mode", "ctourney");
		Client::addMenuItem(%clientId, %curItem++ @ "Set Time Limit", "ctimelimit");
		Client::addMenuItem(%clientId, %curItem++ @ "Change Race Option", "raceopt");
		Client::addMenuItem(%clientId, %curItem++ @ "Reset Server Defaults", "reset");		
		return;
	}
//======================================== Print Help Screens.
	if (%opt == "helpprint") 
	{ 	
		%curItem = 0;
		Client::buildMenu(%clientId, "War40K GameHelpers", "helpers", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Flag Help", "flag");
		Client::addMenuItem(%clientId, %curItem++ @ "Flag Locate", "locate");
		Client::addMenuItem(%clientId, %curItem++ @ "Armor Info", "helparmor");
		return;
	} 	
//======================================== Weapon Options
	if (%opt == "weaponoptions") 
	{ 	
		%curItem = 0;
		Client::buildMenu(%clientId, "Weapon Options", "weapons", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Grenade Launcher", "weapon_gl");
		Client::addMenuItem(%clientId, %curItem++ @ "Bolt Pistol", "weapon_mortar");
		Client::addMenuItem(%clientId, %curItem++ @ "Bolter", "weapon_magnum");
		Client::addMenuItem(%clientId, %curItem++ @ "Isolanth", "weapon_rl");
		Client::addMenuItem(%clientId, %curItem++ @ "Eldar Rocket Launcher", "weapon_erl");
		Client::addMenuItem(%clientId, %curItem++ @ "Stormbolter", "weapon_vulcan");
		Client::addMenuItem(%clientId, %curItem++ @ "Hvy Bolter", "weapon_rail");
		return;
	}
//======================================== Vehicle Options
	if (%opt == "vehicleoptions") 
	{ 	
		%curItem = 0;
		Client::buildMenu(%clientId, "Vehicle Options", "vehicle", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Vyper", "vehicle_vyp");
		Client::addMenuItem(%clientId, %curItem++ @ "Landspeeder", "vehicle_land");
		return;
	}
//======================================== 
	if(%opt == "fteamchange")
	{
		echo("fteamchange");
		%clientId.ptc = %cl;
		Client::buildMenu(%clientId, "Pick a team:", "FPickTeam", true);
		Client::addMenuItem(%clientId, "0Observer", -2);
//---Allow admin to alter palyers team in Total War mode
//if($matchStarted && $Server::TourneyMode) return;
		Client::addMenuItem(%clientId, "1Automatic", -1);
		for(%i = 0; %i < getNumTeams(); %i++) Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
		return;
	}      
	if (%opt == "changeteams")
	{
		if(!$matchStarted || !$Server::TourneyMode)
		{
			Client::buildMenu(%clientId, "Pick a team:", "PickTeam", true);
			Client::addMenuItem(%clientId, "0Observer", -2);
			Client::addMenuItem(%clientId, "1Automatic", -1);
		}
		if($War40k::KeepBalanced)
		{
			%i = checkTeams();
			if(%i != -1) Client::addMenuItem(%clientId, (2) @ getTeamName(%i), %i);
			else
			{
				if(Client::getTeam(%clientId) == -1)
				{
					for(%i = 0; %i < getNumTeams(); %i++)
					Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
				} 
			}
		}
		else
		{
			for(%i = 0; %i < getNumTeams(); %i++)
			Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
		}
		return;
	}
	if (%opt == "mute") %clientId.muted[%cl] = true;
	if (%opt == "unmute") %clientId.muted[%cl] = "";
	if (%opt == "vkick")
	{
		%cl.voteTarget = true;
		Admin::startVote(%clientId, "kick " @ Client::getName(%cl), "kick", %cl);
	}
	if (%opt == "vadmin")
	{
		%cl.voteTarget = true;
		Admin::startVote(%clientId, "admin " @ Client::getName(%cl), "admin", %cl);
	}
	if (%opt == "vsmatch") Admin::startVote(%clientId, "start the match", "smatch", 0);
	if (%opt == "vetd") Admin::startVote(%clientId, "enable team damage", "etd", 0);
	if (%opt == "vdtd") Admin::startVote(%clientId, "disable team damage", "dtd", 0);
	if (%opt == "etd") Admin::setTeamDamageEnable(%clientId, true);
	if (%opt == "dtd") Admin::setTeamDamageEnable(%clientId, false);
	if (%opt == "vcffa") Admin::startVote(%clientId, "change to Free For All mode", "ffa", 0);
	if (%opt == "vctourney") Admin::startVote(%clientId, "change to Total War mode", "tourney", 0);
	if (%opt == "cffa") Admin::setModeFFA(%clientId);
	if (%opt == "ctourney") Admin::setModeTourney(%clientId);
//======================================== Yes
	if (%opt == "voteYes" && %cl == $curVoteCount) 
	{
		%clientId.vote = "yes";
		centerprint(%clientId, "", 0);
	}
//======================================== No
	if (%opt == "voteNo" && %cl == $curVoteCount) 
	{
		%clientId.vote = "no";
		centerprint(%clientId, "", 0);
	}
//======================================== Kick Player
	if (%opt == "kick") 
	{
		Client::buildMenu(%clientId, "Confirm kick:", "kaffirm", true);
		Client::addMenuItem(%clientId, "1Kick " @ Client::getName(%cl), "yes " @ %cl);
		Client::addMenuItem(%clientId, "2Don't kick " @ Client::getName(%cl), "no " @ %cl);
		return;
	}
//======================================== Admin
	if (%opt == "admin") 
	{
		Client::buildMenu(%clientId, "Confirm admim:", "aaffirm", true);
		Client::addMenuItem(%clientId, "1Admin " @ Client::getName(%cl), "yes " @ %cl);
		Client::addMenuItem(%clientId, "2Don't admin " @ Client::getName(%cl), "no " @ %cl);
		return;
	}
//======================================== DeAdmin Conf
	if (%opt == "deadmin") 
	{
		Client::buildMenu(%clientId, "Confirm deadmim:", "daffirm", true);
		Client::addMenuItem(%clientId, "1DeAdmin " @ Client::getName(%cl), "yes " @ %cl);
		Client::addMenuItem(%clientId, "2Don't DeAdmin " @ Client::getName(%cl), "no " @ %cl);
		return;
	}
//======================================== Admin Mute
	if (%opt == "admute") 
	{
		Client::buildMenu(%clientId, "Confirm Admim Mute:", "amaffirm", true);
		Client::addMenuItem(%clientId, "1Admin Mute" @ Client::getName(%cl), "yes " @ %cl);
		Client::addMenuItem(%clientId, "2Don't Admin Mute" @ Client::getName(%cl), "no " @ %cl);
		return;
	}
//======================================== Remove Admin Mute
	if (%opt == "deadmute") 
	{
		Client::buildMenu(%clientId, "Confirm Remove Admim Mute:", "dmaffirm", true);
		Client::addMenuItem(%clientId, "1Remove Admin Mute " @ Client::getName(%cl), "yes " @ %cl);
		Client::addMenuItem(%clientId, "2Don't Remove Admin Mute " @ Client::getName(%cl), "no " @ %cl);
		return;
	}
//======================================== Bot Menu
//	if (%opt == "botmenu") 
//	{
//		Client::buildMenu(%clientId, "Servitor Bot Menu:", "selbotaction", true); 
//		Client::addMenuItem(%clientId, "1Spawn A Servitor Bot", "spawnbot");
//		Client::addMenuItem(%clientId, "2Remove Servitor Bot", "removebot");
//		if(%clientId.isSuperAdmin) Client::addMenuItem(%clientId, "3Kill'em All", "kbaffirm");
//		return;
//	}
//======================================== Ban Player
	if (%opt == "ban") 
	{
		Client::buildMenu(%clientId, "Confirm Ban:", "baffirm", true);
		Client::addMenuItem(%clientId, "1Ban " @ Client::getName(%cl), "yes " @ %cl);
		Client::addMenuItem(%clientId, "2Don't ban " @ Client::getName(%cl), "no " @ %cl);
		return;
	}
//======================================== Admin Kill Player
	if (%opt == "kill") 
	{
		Player::setArmor(%cl,larmor);
		armorChange(%cl);
		Player::blowUp(%cl);
		remoteKill(%cl);
		messageAll(0, Client::getName(%cl) @ " was torn asunder by the hand of god.");
		return; 
	}	
//======================================== Penis Curse
//	if (%opt == "peniscurse") 
//	{
//		%armor = Player::getArmor(%cl);
//		if (%armor != parmor) 
//		{
//			Player::setArmor(%cl,parmor);
//			checkMaxDrop(%cl,parmor);
//			armorChange(%cl);
//			Player::setItemCount(%cl, $ArmorName[%armor], 0);
//			messageAll(0, Client::getName(%cl) @ " was given ONE FOR BEING ONE by " @ Client::getName(%clientId) @ ".");
//			Player::setItemCount(%cl, Penis, 1);
//			Player::mountItem(%cl, Penis, $BackPackSlot);
//			if(Player::getMountedItem(%cl,$FlagSlot) != -1) Player::dropItem(%cl,Player::getMountedItem(%cl,$FlagSlot));
//		}
//		else
//		{
//			Client::sendMessage(%clientId,0,"Removing The Curse...");
//			messageAll(0, " The curse has been lifted from " @ Client::getName(%cl) @ ", the price is death...");
//			Player::setArmor(%cl,aarmor);
//			schedule ("Player::setArmor(" @ %cl @ ",marmor);", 0.4);
//			schedule ("Player::setArmor(" @ %cl @ ",larmor);", 0.8);
//			schedule ("Player::setArmor(" @ %cl @ ",harmor);", 1.1);
//			schedule ("Player::setArmor(" @ %cl @ ",earmor);", 1.4);
//			schedule ("Player::setArmor(" @ %cl @ ",spyarmor);", 1.7);
//			Vehicle::passengerJump(0,%cl,0);	
//			Player::dropItem(%cl,Penis);
//			Player::blowUp(%cl);
//			schedule ("Player::Kill(" @ %cl @ ");", 2.0);
//			schedule ("playSound(ShockExplosion,GameBase::getPosition(" @ %cl @ "));",2.0);
//			%obj = newObject("","Mine","PenisBlast");
//			addToSet("MissionCleanup", %obj);
//			%pos = GameBase::getPosition(%cl);
//			GameBase::setPosition(%obj, %pos);
//		}
//	}
//======================================== End Penis Curse
	if (%opt == "smatch") Admin::startMatch(%clientId);
	if (%opt == "vcmission" || %opt == "cmission")
	{
		Admin::changeMissionMenu(%clientId, %opt == "cmission");
		return;
	}
//======================================== Race Options
	if (%opt == "raceopt") 
	{ 	
		%curItem = 0;
		Client::buildMenu(%clientId, "Change Race Options", "races", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Marine vs. Eldar", "reg");
		Client::addMenuItem(%clientId, %curItem++ @ "Marine vs. Marine", "marine");
		Client::addMenuItem(%clientId, %curItem++ @ "Eldar vs. Eldar", "eldar");
		Client::addMenuItem(%clientId, %curItem++ @ "All Available", "allarmor");
		return;
	}
	if (%opt == "ctimelimit")
	{
		Client::buildMenu(%clientId, "Change Time Limit:", "ctlimit", true);
		Client::addMenuItem(%clientId, "120 Minutes", 20);
		Client::addMenuItem(%clientId, "230 Minutes", 30);
		Client::addMenuItem(%clientId, "360 Minutes", 60);
		Client::addMenuItem(%clientId, "480 Minutes", 80);
		Client::addMenuItem(%clientId, "5120 Minutes", 120);
		Client::addMenuItem(%clientId, "6160 Minutes", 160);
		Client::addMenuItem(%clientId, "7180 Minutes", 180);
		Client::addMenuItem(%clientId, "8No Time Limit", 0);
		return;
	}
	if (%opt == "reset")
	{
		Client::buildMenu(%clientId, "Confirm Reset:", "raffirm", true);
		Client::addMenuItem(%clientId, "1Reset", "yes");
		Client::addMenuItem(%clientId, "2Don't Reset", "no");
		return;
	}
	if (%opt == "observe")
	{
		Observer::setTargetClient(%clientId, %cl);
		return;
	}
//======================================== Admin - Disable Bots
//	if (%opt == "botsoff") 
//	{
//		$War40k::AreThereBots = False;
//		$Spoonbot::AutoSpawn = False;
//		$sbots = False;
//		messageAll(0, Client::getName(%clientId) @ " Turns Servitor Bots Off.");
//		echo(Client::getName(%clientId) @ " Turns Servitor Bots Off.");
//	}
//======================================== Admin - Enable Bots
//	if (%opt == "botson") 
//	{
//		$War40k::AreThereBots = True;
//		$Spoonbot::AutoSpawn = True;
//		%sbots = True;
//		messageAll(0, Client::getName(%clientId) @ " Turns Servitor Bots On.");
//		echo (Client::getName(%clientId) @ " Turns Servitor Bots On.");
//	}
	Game::menuRequest(%clientId);
}