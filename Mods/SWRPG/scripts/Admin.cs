//rpg admin

$curVoteTopic = "";
$curVoteAction = "";
$curVoteOption = "";
$curVoteCount = 0;

function remoteAdminPassword(%clientId, %password)
{
	if($AdminPassword != "" && %password == $AdminPassword[4])
	{
		%clientId.adminLevel = 4;
	}
}

function remoteSetPassword(%clientId, %password)
{
	if(%clientId.adminLevel >= 5)
		$Server::Password = %password;
}

function remoteSetTimeLimit(%clientId, %time)
{
   %time = floor(%time);
   if(%time == $Server::timeLimit || (%time != 0 && %time < 1))
      return;
   if(%clientId.adminLevel >= 4)
   {
      $Server::timeLimit = %time;
      if(%time)
         messageAll(0, Client::getName(%clientId) @ " changed the time limit to " @ %time @ " minute(s).");
      else
         messageAll(0, Client::getName(%clientId) @ " disabled the time limit.");
         
   }
}

function remoteSetTeamInfo(%clientId, %team, %teamName, %skinBase)
{
   if(%team >= 0 && %team < 8 && %clientId.adminLevel >= 4)
   {
      $Server::teamName[%team] = %teamName;
      $Server::teamSkin[%team] = %skinBase;
      messageAll(0, "Team " @ %team @ " is now \"" @ %teamName @ "\" with skin: " 
         @ %skinBase @ " courtesy of " @ Client::getName(%clientId) @ ".  Changes will take effect next mission.");
   }
}

function Admin::kick(%admin, %clientId, %ban)
{
   if(%admin == -1 || %admin.adminLevel >= 4)
   {
      if(%ban && %admin.adminLevel < 5)
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
      if(%clientId.adminLevel >= 5)
      {
         if(%admin == -1)
            messageAll(0, "A super admin cannot be " @ %word @ ".");
         else
            Client::sendMessage(%admin, 0, "A super admin cannot be " @ %word @ ".");
         return;
      }
      %ip = Client::getTransportAddress(%clientId);

      echo(%cmd @ %admin @ " " @ %clientId @ " " @ %ip);

      if(%ip == "")
         return;
      if(%ban)
         BanList::add(%ip, 1800);
      else
         BanList::add(%ip, 180);

      %name = Client::getName(%clientId);

      if(%admin == -1)
      {
         MessageAll(0, %name @ " was " @ %word @ " from vote.");
         Net::kick(%clientId, "You were " @ %word @ " by  consensus.");
      }
      else
      {
         MessageAll(0, %name @ " was " @ %word @ " by " @ Client::getName(%admin) @ ".");
         Net::kick(%clientId, "You were " @ %word @ " by " @ Client::getName(%admin));
      }
   }
}

function Admin::voteFailed()
{
   $curVoteInitiator.numVotesFailed++;

   if($curVoteAction == "kick" || $curVoteAction == "admin")
      $curVoteOption.voteTarget = "";
}

function Admin::voteSucceded()
{
   $curVoteInitiator.numVotesFailed = "";
   if($curVoteAction == "kick")
   {
      if($curVoteOption.voteTarget)
         Admin::kick(-1, $curVoteOption);
   }
   else if($curVoteAction == "admin")
   {
      if($curVoteOption.voteTarget)
      {
         $curVoteOption.adminLevel = 4;
         messageAll(0, Client::getName($curVoteOption) @ " has become an administrator.");
         if($curVoteOption.menuMode == "options")
            Game::menuRequest($curVoteOption);
      }
      $curVoteOption.voteTarget = false;
   }
   else if($curVoteAction == "cmission")
   {
      messageAll(0, "Changing to mission " @ $curVoteOption @ ".");
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
      %margin = $Server::VoteAdminWinMargin;
      %totalVotes = %votesFor + %votesAgainst + %votesAbstain;
      if(%totalVotes < %minVotes)
         %totalVotes = %minVotes;
   }
   if(%votesFor / %totalVotes >= %margin)
   {
      messageAll(0, "Vote to " @ $curVoteTopic @ " passed: " @ %votesFor @ " to " @ %votesAgainst @ " with " @ %totalClients - (%votesFor + %votesAgainst) @ " abstentions.");
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
         if(%totalVotes >= $Server::MinVotes && %votesFor / %totalVotes >= $Server::VoteWinMargin)
         {
            messageAll(0, "Vote to " @ $curVoteTopic @ " passed: " @ %votesFor @ " to " @ %totalVotes - %votesFor @ ".");
            Admin::voteSucceded();
            $curVoteTopic = "";
            return;
         }
      }
      messageAll(0, "Vote to " @ $curVoteTopic @ " did not pass: " @ %votesFor @ " to " @ %votesAgainst @ " with " @ %totalClients - (%votesFor + %votesAgainst) @ " abstentions.");
      Admin::voteFailed();
   }
   $curVoteTopic = "";
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
      bottomprintall("<jc><f1>" @ Client::getName(%clientId) @ " <f0>initiated a vote to <f1>" @ $curVoteTopic, 10);
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

function Game::menuRequest(%clientId)
{
	if(%clientId.IsInvalid)
		return;

	if(%clientId.choosingRace)
	{
		MenuRace(%clientId);
		return;
	}
	else if(%clientId.choosingClothes)
	{
		MenuClothes(%clientId);
		return;
	}
	else if(%clientId.choosingGroup)
	{
		MenuGroup(%clientId);
		return;
	}
	else if(%clientId.choosingClass)
	{
		MenuClass(%clientId);
		return;
	}

	%curItem = 0;
	Client::buildMenu(%clientId, "Options", "options", true);
	if($curVoteTopic != "" && %clientId.vote == "")
	{
		Client::addMenuItem(%clientId, %curItem++ @ "Vote YES to " @ $curVoteTopic, "voteYes " @ $curVoteCount);
		Client::addMenuItem(%clientId, %curItem++ @ "Vote NO to " @ $curVoteTopic, "voteNo " @ $curVoteCount);
	}
	else
	{
		if(%clientId.selClient)
		{
			%sel = %clientId.selClient;
			%selname = Client::getName(%sel);
	
			if(%clientId != %sel && fetchData(%sel, "HasLoadedAndSpawned"))
			{
                        if(IsInCommaList(fetchData(%clientId, "grouplist"), %selname))
					Client::addMenuItem(%clientId, %curItem++ @ "Remove from group-list", "remgroup " @ %sel);
				else
					Client::addMenuItem(%clientId, %curItem++ @ "Add to group-list", "addgroup " @ %sel);

                        if(IsInCommaList(fetchData(%clientId, "targetlist"), %selname))
					Client::addMenuItem(%clientId, %curItem++ @ "Remove from target-list", "remtarget " @ %sel);
				else
					Client::addMenuItem(%clientId, %curItem++ @ "Add to target-list", "addtarget " @ %sel);

				if(fetchData(%clientId, "partyOwned"))
				{
					if(IsInCommaList(fetchData(%clientId, "partylist"), %selname))
						Client::addMenuItem(%clientId, %curItem++ @ "Remove from your party", "remparty " @ %sel);
					else
					{
						if(CountObjInCommaList(fetchData(%clientId, "partylist")) < $maxpartymembers)
						{
							%p = IsInWhichParty(Client::getName(%sel));
							if(%p == -1)
								Client::addMenuItem(%clientId, %curItem++ @ "Invite to your party", "addparty " @ %sel);
							else if(GetWord(%p, 1) == "i")
								Client::addMenuItem(%clientId, %curItem++ @ "Cancel invitation", "cancelinv " @ %sel);
							else
								Client::addMenuItem(%clientId, %curItem++ @ "(Can't invite, already in a party)", "");
						}
						else
							Client::addMenuItem(%clientId, %curItem++ @ "(Can't invite, too many members)", "");
					}
				}

				if(%clientId.muted[%sel])
					Client::addMenuItem(%clientId, %curItem++ @ "Unmute", "unmute " @ %sel);
				else
					Client::addMenuItem(%clientId, %curItem++ @ "Mute", "mute " @ %sel);

			}
			else
				if(!IsDead(%clientId))
					Client::addMenuItem(%clientId, %curItem++ @ "View your stats" , "viewstats");
		}
		else
		{
			if(!IsDead(%clientId))
				Client::addMenuItem(%clientId, %curItem++ @ "View your stats" , "viewstats");
	
			Client::addMenuItem(%clientId, %curItem++ @ "User options..." , "useropts");

			if(!IsDead(%clientId))
				Client::addMenuItem(%clientId, %curItem++ @ "Skill points..." , "sp");

			Client::addMenuItem(%clientId, %curItem++ @ "Party options..." , "partyoptions");

			Client::addMenuItem(%clientId, %curItem++ @ "Inventory..." , "belt");
		}

		//Client::addMenuItem(%clientId, %curItem++ @ "other...", "Other");
	}
}
function processMenuOptions(%clientId, %option)
{
	dbecho($dbechoMode, "processMenuOptions(" @ %clientId @ ", " @ %option @ ")");

	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);
	
	if(%opt == "Other")
	{
		%sel = %clientId.selClient;
		if(%sel == "") %sel = %clientId;
		%name = Client::getName(%sel);

		Client::buildMenu(%clientId, "Other options", "Otheropt", true);

		if($curVoteTopic == "" && %clientId.adminLevel < 4)
		{
			//Client::addMenuItem(%clientId, %curItem++ @ "Vote to change mission", "vcmission");
			if($Server::TeamDamageScale == 1.0)
				Client::addMenuItem(%clientId, %curItem++ @ "Vote to disable team damage", "vdtd");
			else
				Client::addMenuItem(%clientId, %curItem++ @ "Vote to enable team damage", "vetd");
	               
			if($Server::TourneyMode)
			{
				//Client::addMenuItem(%clientId, %curItem++ @ "Vote to enter FFA mode", "vcffa");
				if(!$CountdownStarted && !$matchStarted)
					Client::addMenuItem(%clientId, %curItem++ @ "Vote to start the match", "vsmatch");
			}
			else
			{
				//Client::addMenuItem(%clientId, %curItem++ @ "Vote to enter Tournament mode", "vctourney");
			}
		}
		else if(%clientId.adminLevel >= 4)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Change mission", "cmission");
			if($Server::TeamDamageScale == 1.0)
				Client::addMenuItem(%clientId, %curItem++ @ "Disable team damage", "dtd");
			else
				Client::addMenuItem(%clientId, %curItem++ @ "Enable team damage", "etd");
		}
		if($curVoteTopic == "" && %clientId.adminLevel < 4)
		{
			//Client::addMenuItem(%clientId, %curItem++ @ "Vote to admin " @ %name, "vadmin " @ %sel);
			//Client::addMenuItem(%clientId, %curItem++ @ "Vote to kick " @ %name, "vkick " @ %sel);
		}
		if(%clientId.adminLevel >= 4)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Kick " @ %name, "kick " @ %sel);
			if(%clientId.adminLevel >= 5)
			{
				Client::addMenuItem(%clientId, %curItem++ @ "Ban " @ %name, "ban " @ %sel);
				Client::addMenuItem(%clientId, %curItem++ @ "Admin " @ %name, "admin " @ %sel);
			}
			Client::addMenuItem(%clientId, %curItem++ @ "Change " @ %name @ "'s team", "fteamchange " @ %sel);
		}
		if(%clientId.muted[%sel])
			Client::addMenuItem(%clientId, %curItem++ @ "Unmute " @ %name, "unmute " @ %sel);
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Mute " @ %name, "mute " @ %sel);
		if(%clientId.observerMode == "observerOrbit")
			Client::addMenuItem(%clientId, %curItem++ @ "Observe " @ %name, "observe " @ %sel);
	
		if($Server::TourneyMode)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Change to FFA mode", "cffa");
			if(!$CountdownStarted && !$matchStarted)
				Client::addMenuItem(%clientId, %curItem++ @ "Start the match", "smatch");
		}
		else
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Change to Tournament mode", "ctourney");
			Client::addMenuItem(%clientId, %curItem++ @ "Set Time Limit", "ctimelimit");
			Client::addMenuItem(%clientId, %curItem++ @ "Reset Server Defaults", "reset");
		}
		return;
	}
	//**RPG
	else if(%opt == "selspell")
	{
		Client::buildMenu(%clientId, "Select a spell", "selectspell", true);
		%curitem=1;
		%name = Client::getName(%clientId);

		for(%i=1; $spellShell[%i] != ""; %i++)
		{
			if(isInSpellList(%name, $spellShell[%i]) == 1)
			{
				Client::addMenuItem(%clientId, %curitem @ $spellName[%i], %i);
				%curitem++;
			}
		}

		return;
	}
	else if(%opt == "viewstats")
	{
		%a[%tmp++] = "<f1>" @ Client::getName(%clientId) @ ", LEVEL " @ fetchData(%clientId, "LVL") @ " " @ fetchData(%clientId, "RACE") @ " " @ fetchData(%clientId, "CLASS") @ "<f0>\n\n";

		%a[%tmp++] = "ATK: " @ fetchData(%clientId, "ATK") @ "\n";
		%a[%tmp++] = "DEF: " @ fetchData(%clientId, "DEF") @ "\n";
		%a[%tmp++] = "FDEF: " @ fetchData(%clientId, "MDEF") @ "\n";
		%a[%tmp++] = "Hit Pts: " @ fetchData(%clientId, "HP") @ " / " @ fetchData(%clientId, "MaxHP") @ "\n";
		%a[%tmp++] = "LCK: " @ fetchData(%clientId, "LCK") @ "\n";

		if(fetchData(%clientId, "MyHouse") != "")
		{
			%a[%tmp++] = "Rank Pts: " @ fetchData(%clientId, "RankPoints") @ "\n";
			%a[%tmp++] = "Faction: " @ fetchData(%clientId, "MyHouse") @ "\n";
		}

		%a[%tmp++] = "Experience: " @ fetchData(%clientId, "EXP") @ "\n";
            %a[%tmp++] = "Exp needed: " @ (GetExp(GetLevel(fetchData(%clientId, "EXP"), %clientId)+1, %clientId) - fetchData(%clientId, "EXP") @ "\n\n");

		%a[%tmp++] = "Credits: " @ fetchData(%clientId, "COINS") @ " - Bank: " @ fetchData(%clientId, "BANK") @ "\n";
		%a[%tmp++] = "TOTAL $: " @ fetchData(%clientId, "COINS") + fetchData(%clientId, "BANK") @ "\n\n";
		
		%a[%tmp++] = "Weight: " @ fetchData(%clientId, "Weight") @ " / " @ fetchData(%clientId, "MaxWeight") @ "\n";
		%a[%tmp++] = "Mana: " @ fetchData(%clientId, "MANA") @ " / " @ fetchData(%clientId, "MaxMANA") @ "\n";
		%a[%tmp++] = "Alignment: " @ fetchData(%clientId, "Alignment") @ "\n";

		for(%i = 1; %a[%i] != ""; %i++)
			%f = %f @ %a[%i];

		bottomprint(%clientId, %f, floor(String::len(%f) / 20));

		return;
	}
	else if(%opt == "useropts")
	{
		Client::buildMenu(%clientId, "User settings:", "UserOptions", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Set primary talk key..." , "deftalk");
		Client::addMenuItem(%clientId, %curItem++ @ "Set secondary talk key..." , "alttalk");

		if(fetchData(%clientId, "ignoreGlobal"))
			Client::addMenuItem(%clientId, %curItem++ @ "Turn ignore global OFF" , "gignoreoff");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Turn ignore global ON" , "gignoreon");

		if(fetchData(%clientId, "LCKconsequence") == "miss")
			Client::addMenuItem(%clientId, %curItem++ @ "Set LCK mode = death" , "lckdeath");
		else if(fetchData(%clientId, "LCKconsequence") == "death")
			Client::addMenuItem(%clientId, %curItem++ @ "Set LCK mode = miss" , "lckmiss");
	}
	else if(%opt == "addgroup")
	{
		if(countObjInCommaList(fetchData(%clientId, "grouplist")) <= 30)
		{
			%name = Client::getName(%cl);
			storeData(%clientId, "grouplist", AddToCommaList(fetchData(%clientId, "grouplist"), %name));

			Client::sendMessage(%cl, $MsgBeige, Client::getName(%clientId) @ " has added you to his/her group-list.");
			Client::sendMessage(%clientId, $MsgBeige, %name @ " is now on your group-list.");
		}
		else
			Client::sendMessage(%clientId, $MsgRed, "You have too many people on your group-list.");
	}
	else if(%opt == "remgroup")
	{
		%name = Client::getName(%cl);
		storeData(%clientId, "grouplist", RemoveFromCommaList(fetchData(%clientId, "grouplist"), %name));

		Client::sendMessage(%cl, $MsgBeige, Client::getName(%clientId) @ " has removed you from his/her group-list.");
		Client::sendMessage(%clientId, $MsgBeige, %name @ " is no longer on your group-list.");
	}
	else if(%opt == "addtarget")
	{
		if(countObjInCommaList(fetchData(%clientId, "targetlist")) <= 30)
		{
			%delay = 20;
			%name = Client::getName(%cl);
			Client::sendMessage(%clientId, $MsgRed, %name @ " will be added to your target-list in " @ %delay @ " seconds.");
			Client::sendMessage(%cl, $MsgRed, Client::getName(%clientId) @ " is thinking about killing you.");

			schedule("AddToTargetList(" @ %clientId @ ", " @ %cl @ ");", %delay, %cl);
		}
		else
			Client::sendMessage(%clientId, $MsgRed, "You have too many people on your target-list.");
	}
	else if(%opt == "remtarget")
	{
		%name = Client::getName(%cl);
		storeData(%clientId, "targetlist", RemoveFromCommaList(fetchData(%clientId, "targetlist"), %name));

		Client::sendMessage(%cl, $MsgBeige, Client::getName(%clientId) @ " has declared a truce.");
		Client::sendMessage(%clientId, $MsgBeige, %name @ " is no longer on your target-list.");
	}
	else if(%opt == "addparty")
	{
		%clientId.invitee[%cl] = True;
		Client::sendMessage(%cl, $MsgBeige, Client::getName(%clientId) @ " has invited you to join his/her party.");
		Client::sendMessage(%clientId, $MsgBeige, "You have invited " @ Client::getName(%cl) @ " to join your party.");
	}
	else if(%opt == "remparty")
	{
		%name = Client::getName(%cl);
		RemoveFromParty(%clientId, %name);
	}
	else if(%opt == "cancelinv")
	{
		%clientId.invitee[%cl] = "";
		Client::sendMessage(%cl, $MsgRed, Client::getName(%clientId) @ " has cancelled his invitation.");
		Client::sendMessage(%clientId, $MsgBeige, "You cancelled your invitation to " @ Client::getName(%cl) @ ".");
	}
	else if(%opt == "mute")
	      %clientId.muted[%cl] = True;
	else if(%opt == "unmute")
		%clientId.muted[%cl] = "";
	else if(%opt == "sp")
	{
		MenuSP(%clientId, 1);
		return;
	}
	else if(%opt == "partyoptions")
	{
		Client::buildMenu(%clientId, "Party options", "partyopt", true);

		if(fetchData(%clientId, "partyOwned"))
			Client::addMenuItem(%clientId, "xDisband party", "disbandparty");
		else
			Client::addMenuItem(%clientId, "cCreate party", "createparty");

		%name = Client::getName(%clientId);
		if( (%p = IsInWhichParty(%name)) != -1)
		{
			%id = GetWord(%p, 0);
			%inv = GetWord(%p, 1);
			if(%inv == -1)
			{
				//this player is in the party
				Client::addMenuItem(%clientId, "pLeave current party", "leaveparty " @ %id);
			}
			else if(%inv == "i")
			{
				//this player is being invited
				Client::addMenuItem(%clientId, "pAccept " @ Client::getName(%id) @ "'s party invitation", "acceptinv " @ %id);
			}
		}

		%list = fetchData(%clientId, "partylist");
		for(%p = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999))
		{
			%w = String::NEWgetSubStr(%list, 0, %p);
			Client::addMenuItem(%clientId, %curitem++ @ "Remove " @ %w, "remparty " @ %w);
		}
	}
	else if(%opt == "belt")
	{
		Client::buildMenu(%clientId, "Inventory options:", "selectbelt", true);
		if(%clientId.adminLevel >= 5)
		{
		Client::addMenuItem(%clientId, %curItem++ @ "Books...", "books");
		Client::addMenuItem(%clientId, %curItem++ @ "Badges..." , "badges");
		}
		Client::addMenuItem(%clientId, %curItem++ @ "Hotkey options..." , "bindoptions");
		if(GetAccessoryList(%clientId, 9, -1) != "")
			Client::addMenuItem(%clientId, %curItem++ @ "Ranged weapons...", "rweapons");
		if(GetAccessoryList(%clientId, 11, -1) != "")
			Client::addMenuItem(%clientId, %curItem++ @ "Thrown weapons...", "tweapons");
		if(GetAccessoryList(%clientId, 12, -1) != "")
			Client::addMenuItem(%clientId, %curItem++ @ "Repair/Energy key...", "healthkey");
		Client::addMenuItem(%clientId, "xBack to main menu..." , "back");
//		if(%clientId.adminLevel >= 5)
//			Client::addMenuItem(%clientId, "Generate shopping indexes" , "indexes");
	}
	//**
}

function processMenuselectbelt(%clientId, %option)
{
	dbecho($dbechoMode, "processMenuselectbelt(" @ %clientId @ ", " @ %option @ ")");

	if(%option == "books")
	{
		Client::buildMenu(%clientId, "Books you have:", "selectbook", true);
		Client::addMenuItem(%clientId, "1Smithing book...", "smith");
		Client::addMenuItem(%clientId, "2Text command book...", "commands");
		Client::addMenuItem(%clientId, "3Force: Light powers book...", $SkillDefensiveCasting);
		Client::addMenuItem(%clientId, "4Force: Dark powers book...", $SkillOffensiveCasting);
		Client::addMenuItem(%clientId, "5Force: Neutral powers book...", $SkillEnergy);
		Client::addMenuItem(%clientId, "6My books", "mybooks");
		Client::addMenuItem(%clientId, "xBack to belt...", "back");	
	}
	else if(%option == "badges")
		CreateBadgeMenu(%clientId);
	else if(%option == "bindoptions")
		processMenubindopt(%clientId, page1);
	else if(%option == "rweapons")
	{
		%list = GetAccessoryList(%clientId, 9, -1);

		Client::buildMenu(%clientId, "Ranged weapons:", "selectrweapon", true);
		for(%i = 0; GetWord(%list, %i) != -1; %i++)
		{
			%item = GetWord(%list, %i);
			Client::addMenuItem(%clientId, %curitem++ @ %item.description, %item);
		}
		Client::addMenuItem(%clientId, "xBack to inventory menu..." , "back");
	}
	else if(%option == "tweapons")
	{
		%list = GetAccessoryList(%clientId, 11, -1);

		Client::buildMenu(%clientId, "Select grenade to throw:", "selecttweapon", true);
		for(%i = 0; GetWord(%list, %i) != -1; %i++)
		{
			%item = GetWord(%list, %i);
			Client::addMenuItem(%clientId, %curitem++ @ %item.description, %item);
			if(%i == 7)// && GetWord(%list, %i) != -1)
			{
				Client::addMenuItem(%clientId, "nNext >>", "more " @ %list);
				break;
			}
		}
		Client::addMenuItem(%clientId, "xBack to inventory menu..." , "back");
	}
	else if(%option == "healthkey")
	{
		%list = GetAccessoryList(%clientId, 12, -1);

		Client::buildMenu(%clientId, "Select item:", "selecthealthkey", true);
		for(%i = 0; GetWord(%list, %i) != -1; %i++)
		{
			%item = GetWord(%list, %i);

			Client::addMenuItem(%clientId, %curitem++ @ %item.description, %item);
		}
		Client::addMenuItem(%clientId, "xBack to inventory menu..." , "back");
	}
	else if(%option == "back")
		Game::menuRequest(%clientId);
//	else if(%option == "indexes")
//		setupindexes();
	return;
}

function processMenuselectbook(%clientId, %option)
{
	dbecho($dbechoMode, "processMenuselectbook(" @ %clientId @ ", " @ %option @ ")");

	//%opt = getWord(%option, 0);
	//%cl = getWord(%option, 1);

	if(%option == "back")
		processMenuOptions(%clientId, belt);
	else if(%option == "smith")
		MenuSmithlist(%clientId, 1);
	else if(%option == "commands")
		MenuSmithlist(%clientId, 2);
	else if(%option == "mybooks")
		CreateBookMenu(%clientId);
	else// if (%option == a spell skill)
		MenuSpellList(%clientId, %option);
	return;
}

function processMenubindopt(%clientId, %option)
{
	dbecho($dbechoMode, "processMenubindopt(" @ %clientId @ ", " @ %option @ ")");

	if(%option == "back")
		processMenuOptions(%clientId, belt);
	else if(%option == "page1")
	{
		Client::buildMenu(%clientId, "Chat binding options", "bindopt", true);
		Client::addMenuItem(%clientId, "iBinding Info", "info");
		//Client::addMenuItem(%clientId, "1Blaster key", "1");
		//Client::addMenuItem(%clientId, "2Plasma Gun key", "2");
		//Client::addMenuItem(%clientId, "3Chaingun key", "3");
		Client::addMenuItem(%clientId, "4Disc Launcher key", "4");
		Client::addMenuItem(%clientId, "5Gren. Launcher key", "5");
		Client::addMenuItem(%clientId, "6Laser Rifle key", "6");
		Client::addMenuItem(%clientId, "7ELF gun key", "7");
		Client::addMenuItem(%clientId, "8Mortar key", "8");
		Client::addMenuItem(%clientId, "9Targeting Las. key", "9");
//		Client::addMenuItem(%clientId, "nNext page...", "page2");
		Client::addMenuItem(%clientId, "xBack to belt...", "back");
	}
	else if(%option == "page2") //nolonger used
	{
		Client::buildMenu(%clientId, "Chat binding options2", "bindopt", true);
		Client::addMenuItem(%clientId, "iBinding Info", "info");
		Client::addMenuItem(%clientId, "6Laser Rifle key", "6");
		Client::addMenuItem(%clientId, "7ELF gun key", "7");
		Client::addMenuItem(%clientId, "8Mortar key", "8");
		Client::addMenuItem(%clientId, "9Targeting Las. key", "9");
		Client::addMenuItem(%clientId, "pPrevious page...", "page1");
		Client::addMenuItem(%clientId, "xBack to belt...", "back");
	}
	else if(%option == "info")
	{
		bottomPrint(%clientId, " <f1>These are chat binds. Created for use with force powers, but can be made to say anything you want.\n <f2>Select one in the menu, and the next thing you say will be said each time you press the key for it. (See Options, under <f0>Inventory<f2>)", 15);
		processMenuOptions(%clientId, bindoptions);
	}
	else
	{
		%clientId.BindingNum = %option;
		client::sendMessage(%clientId, 0, "The next thing you say will be bound to the key you selected.");
	}

	return;
}

function processMenuUserOptions(%clientId, %opt)
{
	if(%opt == "deftalk")
	{
		Client::buildMenu(%clientId, "Select default talk:", "DefTalk", true);
		Client::addMenuItem(%clientId, "1Set default talk: #global", "#g");
		Client::addMenuItem(%clientId, "2Set default talk: #group", "#group");
		Client::addMenuItem(%clientId, "3Set default talk: #party", "#p");
		Client::addMenuItem(%clientId, "4Set default talk: #r", "r");
		Client::addMenuItem(%clientId, "5Set default talk: #say", "#say");
		Client::addMenuItem(%clientId, "6Set default talk: #shout", "#shout");
		Client::addMenuItem(%clientId, "7Set default talk: #whisper", "#whisper");
		Client::addMenuItem(%clientId, "8Set default talk: #zone", "#z");
	}
	else if(%opt == "alttalk")
	{
		Client::buildMenu(%clientId, "Select alternate talk:", "AltTalk", true);
		Client::addMenuItem(%clientId, "1Set alternate talk: #global", "#g");
		Client::addMenuItem(%clientId, "2Set alternate talk: #group", "#group");
		Client::addMenuItem(%clientId, "3Set alternate talk: #party", "#p");
		Client::addMenuItem(%clientId, "4Set alternate talk: #r", "r");
		Client::addMenuItem(%clientId, "5Set alternate talk: #say", "#say");
		Client::addMenuItem(%clientId, "6Set alternate talk: #shout", "#shout");
		Client::addMenuItem(%clientId, "7Set alternate talk: #whisper", "#whisper");
		Client::addMenuItem(%clientId, "8Set alternate talk: #zone", "#z");
	}
	else if(%opt == "gignoreon")
	{
		storeData(%clientId, "ignoreGlobal", True);
		client::sendMessage(%clientId, 0, "Ignore global chat: ON");
	}
	else if(%opt == "gignoreoff")
	{
		storeData(%clientId, "ignoreGlobal", "");
		client::sendMessage(%clientId, 0, "Ignore global chat: OFF");
	}
	else if(%opt == "lckmiss")
	{
		storeData(%clientId, "LCKconsequence", "miss");
	}
	else if(%opt == "lckdeath")
	{
		storeData(%clientId, "LCKconsequence", "death");
	}
}

function processMenuDefTalk(%clientId, %opt)
{
	storeData(%clientId, "defaultTalk", %opt);
	return;

	if(%opt == "glbl")
		storeData(%clientId, "defaultTalk", "#g");
	else if(%opt == "grp")
		storeData(%clientId, "defaultTalk", "#group");
	else if(%opt == "prty")
		storeData(%clientId, "defaultTalk", "#party");
	else if(%opt == "r")
		storeData(%clientId, "defaultTalk", "#r");
	else if(%opt == "say")
		storeData(%clientId, "defaultTalk", "#say");
	else if(%opt == "sht")
		storeData(%clientId, "defaultTalk", "#shout");
	else if(%opt == "say")
		storeData(%clientId, "defaultTalk", "#say");
	else if(%opt == "zone")
		storeData(%clientId, "defaultTalk", "#z");
}

function processMenuAltTalk(%clientId, %opt)
{
	storeData(%clientId, "alternateTalk", %opt);
	return;

	if(%opt == "glbl")
		storeData(%clientId, "alternateTalk", "#g");
	else if(%opt == "grp")
		storeData(%clientId, "alternateTalk", "#group");
	else if(%opt == "prty")
		storeData(%clientId, "alternateTalk", "#party");
	else if(%opt == "r")
		storeData(%clientId, "alternateTalk", "#r");
	else if(%opt == "say")
		storeData(%clientId, "alternateTalk", "#say");
	else if(%opt == "sht")
		storeData(%clientId, "alternateTalk", "#shout");
	else if(%opt == "say")
		storeData(%clientId, "alternateTalk", "#say");
	else if(%opt == "zone")
		storeData(%clientId, "alternateTalk", "#z");
}

function processMenupartyopt(%clientId, %option)
{
	dbecho($dbechoMode, "processMenupartyopt(" @ %clientId @ ", " @ %option @ ")");

	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);

	if(%opt == "disbandparty")
	{
		DisbandParty(%clientId);
	}
	else if(%opt == "createparty")
	{
		CreateParty(%clientId);
	}
	else if(%opt == "remparty")
	{
		RemoveFromParty(%clientId, %cl);
	}
	else if(%opt == "acceptinv")
	{
		%name = Client::getName(%clientId);
		if( (%p = IsInWhichParty(%name)) != -1)
		{
			%id = GetWord(%p, 0);
			%inv = GetWord(%p, 1);
			if(%inv == "i")
				AddToParty(%id, %name);
		}
	}
	else if(%opt == "leaveparty")
	{
		RemoveFromParty(%cl, Client::getName(%clientId));
	}

	return;
}

function processMenuselectrweapon(%clientId, %item)
{
	if(%item == "back")
	{
		processMenuOptions(%clientId, "belt " @ %clientId);
		return;
	}
	%list = GetAccessoryList(%clientId, 10, -1);

	Client::buildMenu(%clientId, "Projectiles:", "selectproj", true);
	for(%i = 0; GetWord(%list, %i) != -1; %i++)
	{
		%proj = GetWord(%list, %i);

		if(String::findSubStr($ProjRestrictions[%proj], "," @ %item @ ",") != -1)
			Client::addMenuItem(%clientId, %curitem++ @ %proj.description, %item @ " " @ %proj);
	}
	Client::addMenuItem(%clientId, "xBack ranged weapons menu..." , "back");
	return;
}
function processMenuselectproj(%clientId, %itemandproj)
{
	%item = GetWord(%itemandproj, 0);
	%proj = GetWord(%itemandproj, 1);
	if(%item != "back")
	{
		storeData(%clientId, "LoadedProjectile " @ %item, %proj);
		client::sendMessage(%clientId, 0, "Your " @ %item.description @ " has been loaded with " @ %proj.description);
	}
	processMenuselectbelt(%clientId, rweapons);
}

function processMenuselecttweapon(%clientId, %item)
{
	if((%m = getWord(%item, 0)) == more)
	{
		Client::buildMenu(%clientId, "Select grenade to throw:", "selecttweapon", true);
		%list = String::NEWgetSubStr(%item, (String::len(%m)+1), 99999);
		for(%i = 7; GetWord(%list, %i) != -1; %i++)
		{
			%item = GetWord(%list, %i);
			Client::addMenuItem(%clientId, %i - 6 @ %item.description, %item);
		}
		Client::addMenuItem(%clientId, "p<< Prev", "prev");
		Client::addMenuItem(%clientId, "xBack to inventory menu..." , "back");
		return;
	}
	if(%item == prev)
	{
		processMenuselectbelt(%clientId, tweapons);
		return;
	}
	if(%item != back)
	{
		storeData(%clientId, "SelectedGrenade", %item);
		client::sendMessage(%clientId, 0, "Your grenade key will now throw: " @ %item.description);
	}
	processMenuOptions(%clientId, "belt " @ %clientId);
	return;
}

function processMenuselecthealthkey(%clientId, %item)
{
	if(%item != back)
	{
		storeData(%clientId, "SelectedPotion", %item);
		client::sendMessage(%clientId, 0, "Your health kit key will now use: " @ %item.description);
	}
	processMenuOptions(%clientId, "belt " @ %clientId);
	return;
}

function processMenuOtheropt(%clientId, %option)
{
	dbecho($dbechoMode, "processMenuOtheropt(" @ %clientId @ ", " @ %option @ ")");

	%opt = GetWord(%option, 0);
	%cl = GetWord(%option, 1);
	if(%opt == "fteamchange")
	{
		%clientId.ptc = %cl;
		Client::buildMenu(%clientId, "Pick a team:", "FPickTeam", true);
	      Client::addMenuItem(%clientId, "0Observer", -2);
	      Client::addMenuItem(%clientId, "1Automatic", -1);
	      for(%i = 0; %i < getNumTeams(); %i = %i + 1)
		      Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
		return;
	}      
	else if(%opt == "changeteams")
	{
	      if(!$matchStarted || !$Server::TourneyMode)
	      {
			Client::buildMenu(%clientId, "Pick a team:", "PickTeam", true);
			Client::addMenuItem(%clientId, "0Observer", -2);
			Client::addMenuItem(%clientId, "1Automatic", -1);
			for(%i = 0; %i < getNumTeams(); %i = %i + 1)
				Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
			return;
		}
	}
	else if(%opt == "mute")
	      %clientId.muted[%cl] = true;
	else if(%opt == "unmute")
		%clientId.muted[%cl] = "";
	else if(%opt == "vkick")
	{
	      %cl.voteTarget = true;
	      Admin::startVote(%clientId, "kick " @ Client::getName(%cl), "kick", %cl);
	}
	else if(%opt == "vadmin")
	{
	      %cl.voteTarget = true;
	      Admin::startVote(%clientId, "admin " @ Client::getName(%cl), "admin", %cl);
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
	else if(%opt == "kick")
	{
	      Client::buildMenu(%clientId, "Confirm kick:", "kaffirm", true);
	      Client::addMenuItem(%clientId, "1Kick " @ Client::getName(%cl), "yes " @ %cl);
	      Client::addMenuItem(%clientId, "2Don't kick " @ Client::getName(%cl), "no " @ %cl);
	      return;
	}
	else if(%opt == "admin")
	{
	      Client::buildMenu(%clientId, "Confirm admim:", "aaffirm", true);
	      Client::addMenuItem(%clientId, "1Admin " @ Client::getName(%cl), "yes " @ %cl);
	      Client::addMenuItem(%clientId, "2Don't admin " @ Client::getName(%cl), "no " @ %cl);
	      return;
	}
	else if(%opt == "ban")
	{
	      Client::buildMenu(%clientId, "Confirm Ban:", "baffirm", true);
	      Client::addMenuItem(%clientId, "1Ban " @ Client::getName(%cl), "yes " @ %cl);
	      Client::addMenuItem(%clientId, "2Don't ban " @ Client::getName(%cl), "no " @ %cl);
		return;
	}
	else if(%opt == "smatch")
		Admin::startMatch(%clientId);
	else if(%opt == "vcmission" || %opt == "cmission")
	{
		Admin::changeMissionMenu(%clientId, %opt == "cmission");
		return;
	}
	else if(%opt == "ctimelimit")
	{
	      Client::buildMenu(%clientId, "Change Time Limit:", "ctlimit", true);
	      Client::addMenuItem(%clientId, "110 Minutes", 10);
	      Client::addMenuItem(%clientId, "215 Minutes", 15);
	      Client::addMenuItem(%clientId, "320 Minutes", 20);
	      Client::addMenuItem(%clientId, "425 Minutes", 25);
	      Client::addMenuItem(%clientId, "530 Minutes", 30);
	      Client::addMenuItem(%clientId, "645 Minutes", 45);
	      Client::addMenuItem(%clientId, "760 Minutes", 60);
	      Client::addMenuItem(%clientId, "8No Time Limit", 0);
	      return;
	}
	else if(%opt == "reset")
	{
	      Client::buildMenu(%clientId, "Confirm Reset:", "raffirm", true);
	      Client::addMenuItem(%clientId, "1Reset", "yes");
		Client::addMenuItem(%clientId, "2Don't Reset", "no");
		return;
	}
	else if(%opt == "observe")
	{
		Observer::setTargetClient(%clientId, %cl);
		return;
	}
	Game::menuRequest(%clientId);
}

function remoteSelectClient(%clientId, %selId)
{
	dbecho($dbechoMode, "remoteSelectClient(" @ %clientId @ ", " @ %selId @ ")");

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
		remoteEval(%clientId, "setInfoLine", 6, "Remort level: " @ $ClientData[%selId, "RemortStep"]);
	}
}


function processMenuPickTeam(%clientId, %team, %adminClient)
{
	dbecho($dbechoMode, "processMenuPickTeam(" @ %clientId @ ", " @ %team @ ", " @ %adminClient @ ")");

   if(%team != -1 && %team == Client::getTeam(%clientId))
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
            messageAll(0, Client::getName(%clientId) @ " was forced into observer mode by " @ Client::getName(%adminClient) @ ".");
		   Game::refreshClientScore(%clientId);
		}
      return;
   }

   %player = Client::getOwnedObject(%clientId);
   %clientId.observerMode = "";

   if(%team == -1)
   {
      UpdateTeam(%clientId);
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
