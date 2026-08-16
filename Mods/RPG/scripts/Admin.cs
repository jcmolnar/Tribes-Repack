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



//rpg admin

$curVoteTopic = "";
$curVoteAction = "";
$curVoteOption = "";
$curVoteCount = 0;

function Admin::changeMissionMenu(%clientId)
{
}

function processMenuCMType(%clientId, %options)
{
}

function processMenuCMission(%clientId, %option)
{
}

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
}

function remoteSetTeamInfo(%clientId, %team, %teamName, %skinBase)
{
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
}

function Admin::setTeamDamageEnable(%admin, %enabled)
{
}

function Admin::DirectKick(%admin, %clientId, %msg)
{
	%ip = Client::getTransportAddress(%clientId);
	BanList::add(%ip, 30);
	%aname = rpg::getName(%admin);
	%name = rpg::getName(%clientId);
	//MessageAll(0, %name @ " was kicked by " @ %aname @ ".");
	newKick(%clientId, %msg, True);
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
			newKick(%clientId, "You were " @ %word @ " by  consensus.");
		}
		else
		{
			MessageAll(0, %name @ " was " @ %word @ " by " @ Client::getName(%admin) @ ".");
			newKick(%clientId, "You were " @ %word @ " by " @ Client::getName(%admin));
		}
	}
}


function Admin::setModeFFA(%clientId)
{
}

function Admin::setModeTourney(%clientId)
{
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
//      if($curVoteOption.voteTarget)
//         Admin::kick(-1, $curVoteOption);
   }
   else if($curVoteAction == "admin")
   {
      if($curVoteOption.voteTarget)
      {
//         $curVoteOption.adminLevel = 4;
         messageAll(0, Client::getName($curVoteOption) @ " has become an administrator.");
         if($curVoteOption.menuMode == "options")
            Game::menuRequest($curVoteOption);
      }
      $curVoteOption.voteTarget = false;
   }
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
      %clientId.vote = "no"; // yes
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
$menuChars = "1234567890-=abcghijklmnoqrtuvwxy .";
function Game::menuRequest(%clientId)
{
	if(%clientId.IsInvalid)
		return;

	if(%clientId.choosingGroup)
	{
		MenuGroup(%clientId);
		return;
	}
	else if(%clientId.choosingClass)
	{
		MenuClass(%clientId);
		return;
	}
	else if(%clientId.currentShop != "")
	{
		%clientId.beltShop = %clientId.currentShop;
		MenuBuyBeltItem(%clientid, 1);
		return;
	}
	else if(%clientId.currentBank != "")
	{
		//Store belt items
		MenuWithdrawBelt(%clientId);
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
			if(%clientid.adminlevel >= 1)
				Client::addMenuItem(%clientId, %curItem++ @ "Admin controls", "admincontrol " @ %sel);
		}
		else
		{
			%curItem = -1;
			if(!IsDead(%clientId))
				Client::addMenuItem(%clientId, string::getsubstr($menuChars,%curItem++,1) @ "View your stats" , "viewstats");
			Client::addMenuItem(%clientId, string::getsubstr($menuChars,%curItem++,1) @ "Settings..." , "settings");
			if(!IsDead(%clientId)){
				Client::addMenuItem(%clientId, string::getsubstr($menuChars,%curItem++,1) @ "Belt... (wt:"@Belt::GetWeight(%clientid)@")","viewbelt");
				Client::addMenuItem(%clientId, string::getsubstr($menuChars,%curItem++,1) @ "Skill points..." , "sp");
				Client::addMenuItem(%clientId, string::getsubstr($menuChars,%curItem++,1) @ "Compass" , "compass");
			}
			Client::addMenuItem(%clientId, string::getsubstr($menuChars,%curItem++,1) @ "Party options..." , "partyoptions");			if($sanctionedAdmin[rpg::getname(%clientId)] > 0)
				Client::addMenuItem(%clientId, string::getsubstr($menuChars,%curItem++,1) @ "Admin controls", "admincontrol");
			if(GetAccessoryList(%clientId, 9, -1) != "")
				Client::addMenuItem(%clientId, string::getsubstr($menuChars,%curItem++,1) @ "Ranged weapons..." , "rweapons");
		}
	}
}
function processMenuOptions(%clientId, %option)
{
	dbecho($dbechoMode, "processMenuOptions(" @ %clientId @ ", " @ %option @ ")");

	%opt = getWord(%option, 0);
	%cl = floor(getWord(%option, 1));
	


	//**RPG
	if(%opt == "settings")
	{
		Client::buildMenu(%clientId, "Settings", "settings", true);

		Client::addMenuItem(%clientId, %curItem++ @ "Set default talk..." , "defaulttalk");

		%lck = fetchData(%clientId, "LCK");
		if(fetchData(%clientId, "LCKconsequence") == "miss")
			Client::addMenuItem(%clientId, %curItem++ @ "Cur LCK mode:miss("@%lck@")" , "lckdeath");
		else if(fetchData(%clientId, "LCKconsequence") == "death")
			Client::addMenuItem(%clientId, %curItem++ @ "Cur LCK mode:death("@%lck@")" , "lckmiss");

		if(fetchData(%clientId, "ignoreGlobal"))
			Client::addMenuItem(%clientId, %curItem++ @ "Turn ignore global OFF" , "gignoreoff");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Turn ignore global ON" , "gignoreon");

		Client::addMenuItem(%clientId, %curItem++ @ "Battle message config..." , "battmsg");

	}
	else if(%opt == "compass")	{		Client::buildMenu(%clientId, "Sense heading: "@$playerSkill[%clientId,$SkillSenseHeading], "compass", true);		Client::addMenuItem(%clientId, %curItem++ @ "Recall (skill: 0)" , "recall");		Client::addMenuItem(%clientId, %curItem++ @ "Nearest town (skill: 3)" , "town");		Client::addMenuItem(%clientId, %curItem++ @ "Nearest dungeon (skill: 3)" , "dungeon");		Client::addMenuItem(%clientId, %curItem++ @ "Nearest zones list (skill: 20)" , "advcompass");		Client::addMenuItem(%clientId, %curItem++ @ "Track player (skill: 15)" , "track player");		Client::addMenuItem(%clientId, %curItem++ @ "Track AI (skill: 15)" , "track bot");		Client::addMenuItem(%clientId, %curItem++ @ "Track your packs (skill: 0)" , "trackpack own");		Client::addMenuItem(%clientId, %curItem++ @ "Track all packs (skill: 85)" , "trackpack all");		return;	}
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
		%a[%tmp++] = "MDEF: " @ fetchData(%clientId, "MDEF") @ "\n";
		%a[%tmp++] = "Hit Pts: " @ fetchData(%clientId, "HP") @ " / " @ fetchData(%clientId, "MaxHP") @ "\n";
		%a[%tmp++] = "LCK: " @ fetchData(%clientId, "LCK") @ "\n";

		if(fetchData(%clientId, "MyHouse") != "")
		{
			%a[%tmp++] = "Rank Pts: " @ fetchData(%clientId, "RankPoints") @ "\n";
			%a[%tmp++] = "House: " @ fetchData(%clientId, "MyHouse") @ "\n";
		}

		%a[%tmp++] = "Experience: " @ fetchData(%clientId, "EXP") @ "\n";
            %a[%tmp++] = "Exp needed: " @ (GetExp(GetLevel(fetchData(%clientId, "EXP"), %clientId)+1, %clientId) - fetchData(%clientId, "EXP") @ "\n\n");

		%a[%tmp++] = "Coins: " @ fetchData(%clientId, "COINS") @ " - Bank: " @ fetchData(%clientId, "BANK") @ "\n";
		%a[%tmp++] = "TOTAL $: " @ fetchData(%clientId, "COINS") + fetchData(%clientId, "BANK") @ "\n\n";
		
		%a[%tmp++] = "Weight: " @ fetchData(%clientId, "Weight") @ " / " @ fetchData(%clientId, "MaxWeight") @ "\n";
		%a[%tmp++] = "Mana: " @ fetchData(%clientId, "MANA") @ " / " @ fetchData(%clientId, "MaxMANA") @ "\n";

		for(%i = 1; %a[%i] != ""; %i++)
			%f = %f @ %a[%i];

		bottomprint(%clientId, %f, floor(String::len(%f) / 20));

		return;
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
	else if(%opt == "rweapons")
	{
		%list = GetAccessoryList(%clientId, 9, -1);

		Client::buildMenu(%clientId, "Ranged weapons:", "selectrweapon", true);
		for(%i = 0; GetWord(%list, %i) != -1; %i++)
		{
			%item = GetWord(%list, %i);

			Client::addMenuItem(%clientId, %curitem++ @ %item.description, %item);
		}
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
	else if(%opt == "viewbelt")
	{
		MenuViewBelt(%clientid, 1);
		return;
	}
	else if(%opt == "admincontrol"){
		if($sanctionedAdmin[rpg::getname(%clientId)] < 1)
			return;
		%title = "Admin controls";
		if(%cl != -1){
			%title = %title @ " for " @ rpg::getname(%cl);
		}
		Client::buildMenu(%clientId, %title, "admincontrol", true);
		%curItem = -1;
		if(%cl != -1){
			Client::addMenuItem(%clientId,string::getsubstr($menuChars,%curItem++,1) @ "Observe" , "eyes "@%cl);
			if(%clientId.adminlevel >= 2){
				Client::addMenuItem(%clientId,string::getsubstr($menuChars,%curItem++,1) @ "Teleport" , "teleport "@%cl);
				Client::addMenuItem(%clientId,string::getsubstr($menuChars,%curItem++,1) @ "Teleport to" , "teleportto "@%cl);
				Client::addMenuItem(%clientId,string::getsubstr($menuChars,%curItem++,1) @ "Force recall" , "forcerecall "@%cl);
				Client::addMenuItem(%clientId,string::getsubstr($menuChars,%curItem++,1) @ "Jail" , "jail "@%cl);
				Client::addMenuItem(%clientId,string::getsubstr($menuChars,%curItem++,1) @ "Kick" , "kick "@%cl);
			}
			if(%clientId.adminlevel >= 3)
				Client::addMenuItem(%clientId,string::getsubstr($menuChars,%curItem++,1) @ "Set race" , "setrace "@%cl);
			return;
		}
		Client::addMenuItem(%clientId, %curItem++ @ "Admin level" , "adminlevel");
		if(%clientId.adminlevel >= 3)
			Client::addMenuItem(%clientId,string::getsubstr($menuChars,%curItem++,1) @ "Set race" , "setrace "@%clientId);
	}
}

function admin::hasPermissionToMod(%client, %victim){
	if(%client == %victim)
		return True;
	if(%client.adminlevel > %victim.adminlevel)
		return True;
	return False;
}

function processMenuadmincontrol(%clientId, %option){
	%opt = getWord(%option, 0);
	if(%opt == "adminlevel"){
		%max = $sanctionedAdmin[rpg::getname(%clientId)];
		if(%max == "")
			return;
		Client::buildMenu(%clientId, "Your admin level (cur:"@%clientId.adminlevel@")", "adminlevel", true);
		%curItem = -1;
		for(%i = 0; %i <= %max; %i++){
			Client::addMenuItem(%clientId, string::getsubstr($menuChars,%curItem++,1) @ "Level "@%i , %i);
		}
	}
	if(%clientId.adminlevel < 1)
		return;
	%cl = getWord(%option, 1);
	if(%opt == "eyes"){
		if(admin::hasPermissionToMod(%clientId, %cl))
			rpg::eyes(%clientId, %cl);
		else
			Client::sendMessage(%clientId, 0, "Could not process command: Target admin clearance level too high.");
	}
	if(%clientId.adminlevel < 2)
		return;
	if(%opt == "teleport"){
		admin::teleport(%clientId, %cl);
	}
	if(%opt == "teleportto"){
		admin::teleportto(%clientId, %cl);
	}
	if(%opt == "forcerecall"){
		admin::forcerecall(%clientId, %cl);
	}
	if(%opt == "jail"){
		Client::buildMenu(%clientId, "Jail "@rpg::getname(%cl)@" for..", "jail", true);
		%curItem = -1;
		Client::addMenuItem(%clientId, string::getsubstr($menuChars,%curItem++,1) @ "30 seconds" , %cl@" 30");
		Client::addMenuItem(%clientId, string::getsubstr($menuChars,%curItem++,1) @ "60 seconds" , %cl@" 60");
		if(%clientId.adminlevel > 2){
			Client::addMenuItem(%clientId, string::getsubstr($menuChars,%curItem++,1) @ "120 seconds" , %cl@" 120");
			Client::addMenuItem(%clientId, string::getsubstr($menuChars,%curItem++,1) @ "300 seconds" , %cl@" 300");
			Client::addMenuItem(%clientId, string::getsubstr($menuChars,%curItem++,1) @ "600 seconds" , %cl@" 600");
		}
	}
	if(%opt == "kick"){		Client::buildMenu(%clientId, "Confirm kick?", "acconfirm", true);		Client::addMenuItem(%clientId, "1Confirm kick" , "confirm "@%opt@" "@%cl);		Client::addMenuItem(%clientId, "2Cancel" , "cancel");
	}
	if(%opt == "setrace"){
		Client::buildMenu(%clientId, "Set race for "@rpg::getname(%cl), "setrace", true);
		Client::addMenuItem(%clientId, "1Male Human" , "MaleHuman "@%cl);
		Client::addMenuItem(%clientId, "2Female Human" , "FemaleHuman "@%cl);
		Client::addMenuItem(%clientId, "3Death Knight" , "DeathKnight "@%cl);
	}
}
function processMenuacconfirm(%clientId, %option){	%c = getWord(%option,0);	%opt = getWord(%option,1);	%cl = getWord(%option,2);	if(%c == "confirm"){		if(%opt == "kick"){			if(admin::hasPermissionToMod(%clientId, %cl))				Admin::DirectKick(%clientId, %cl, "You have been kicked by the administrator.");			else				Client::sendMessage(%clientId, 0, "Could not process command: Target admin clearance level too high.");		}	}}

function processMenujail(%clientId, %option){
	if(%clientId.adminlevel < 2) return;
	%cl = getWord(%option, 0);
	if(!admin::hasPermissionToMod(%clientId, %cl)){
		Client::sendMessage(%clientId, 0, "Could not process command: Target admin clearance level too high.");
		return;
	}
	%time = getWord(%option,1);
	%c3 = GetRandomJailNumber();
	Jail(%cl, %time, %c3);
	Client::sendMessage(%clientId, 0, %cl @ " has been jailed for " @ %time @ " seconds in Jail #" @ %c3 @ ".");
}

function processMenusetrace(%clientId, %option){
	if(%clientId.adminlevel < 3) return;
	%cl = getWord(%option,1);
	if(!admin::hasPermissionToMod(%clientId, %cl)){
		Client::sendMessage(%clientId, 0, "Could not process command: Target admin clearance level too high.");
		return;
	}
	%race = getWord(%option,0);
	ChangeRace(%cl, %race);
}

function processMenuadminlevel(%clientId, %option){
	%name = rpg::getname(%clientId);
	%max = $sanctionedAdmin[%name];
	if(%option > %max)
		return;
	%clientId.adminlevel = %option;
	pecho("Setting "@%name@" admin level to "@%option);
	Client::sendMessage(%clientId, $msgWhite, "Your admin level is now "@%option);
}

function processMenusettings(%clientId, %option)
{

	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);

	if(%opt == "defaulttalk")
	{

		Client::buildMenu(%clientId, "Default Talk", "deftalk", true);
		%defaulttalks[1] = "#group";
		%defaulttalks[2] = "#global";
		%defaulttalks[3] = "#say";
		for(%i = 1; %defaulttalks[%i] != ""; %i++)
			Client::addMenuItem(%clientId, %i@%defaulttalks[%i], %defaulttalks[%i]);
	}
	else if(%opt == "gignoreon")
	{
		storeData(%clientId, "ignoreGlobal", True);
	}
	else if(%opt == "gignoreoff")
	{
		storeData(%clientId, "ignoreGlobal", "");
	}
	else if(%opt == "lckmiss")
	{
		storeData(%clientId, "LCKconsequence", "miss");
	}
	else if(%opt == "lckdeath")
	{
		storeData(%clientId, "LCKconsequence", "death");
	}
	else if(%opt == "battmsg")
	{
		Client::buildMenu(%clientId, "Battle message config:", "battmsgcfg", true);		%cur = fetchData(%clientId, "battlemsg");		if(%cur == "")			%cur = "auto";		%choices = "chathud topprint bottomprint auto";		%eng["chathud"] = "Chat HUD";		%eng["topprint"] = "Top Print";		%eng["bottomprint"] = "Bottom Print";		%eng["auto"] = "Automatic";		if(%clientId.repack >= 32){			%choices = "chathud topleft topprint bottomleft bottomprint auto";			%eng["topleft"] = "Top Left";			%eng["topprint"] = "Top Right";			%eng["bottomleft"] = "Bottom Left";			%eng["bottomprint"] = "Bottom Right";		}				for(%i = 0; getword(%choices, %i) != -1; %i++)		{			%w = getword(%choices, %i);			%eng = %eng[%w];			if(%cur == %w)				%eng = string::translate(%eng);			Client::addMenuItem(%clientId, %curitem++ @ %eng, %w);		}		if(%clientId.repack < 32)			Client::addMenuItem(%clientId, %curitem++ @ "Print # of lines..", "printlength");
		return;
	}
}
function processMenudeftalk(%clientId, %option)
{
	%defaulttalks[1] = "#group";
	%defaulttalks[2] = "#global";
	%defaulttalks[3] = "#say";
	for(%i = 1; %defaulttalks[%i] != ""; %i++){
		if(%option == %defaulttalks[%i])
			storeData(%clientId, "defaultTalk", %option);
	}
}
function processMenubattmsgcfg(%clientId, %option)
{
	dbecho($dbechoMode, "processMenuselectctrlk(" @ %clientId @ ", " @ %option @ ")");

	%opt = GetWord(%option, 0);
	%cl = GetWord(%option, 1);

	if(%opt == "printlength"){
		Client::buildMenu(%clientId, "Print # of lines...", "battmsglines", true);
		Client::addMenuItem(%clientId, %curItem++ @ "1" , "1");
		Client::addMenuItem(%clientId, %curItem++ @ "4" , "4");
		if(%clientId.repack >= 9){
			Client::addMenuItem(%clientId, %curItem++ @ "5" , "5");
			Client::addMenuItem(%clientId, %curItem++ @ "8" , "8");
			Client::addMenuItem(%clientId, %curItem++ @ "10" , "10");
			Client::addMenuItem(%clientId, %curItem++ @ "15" , "15");
			Client::addMenuItem(%clientId, %curItem++ @ "20" , "20");
		}
		return;
	}

	storeData(%clientId, "battlemsg", %opt);

	%eng["chathud"] = "Chat HUD";	%eng["topprint"] = "Top Print";	%eng["bottomprint"] = "Bottom Print";	%eng["auto"] = "Automatic";	if(%clientId.repack >= 32){		%eng["topleft"] = "Top Left";		%eng["topprint"] = "Top Right";		%eng["bottomleft"] = "Bottom Left";		%eng["bottomprint"] = "Bottom Right";	}

	if(%clientId.repack >= 32)		refreshBattleHudPos(%clientId);	processMenusettings(%clientId, "battmsg");

//	if(%opt == "chathud")
//		Client::sendMessage(%ClientId, 0, "Battle messages now appear in your chat hud.");
//	else if(%opt == "bottomprint")
//		Client::sendMessage(%ClientId, 0, "Battle messages now appear at the bottom of your screen.");
//	else if(%opt == "topprint")
//		Client::sendMessage(%ClientId, 0, "Battle messages now appear at the top of your screen.");
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

function processMenuselectspell(%clientId, %option)
{
	dbecho($dbechoMode, "processMenuselectspell(" @ %clientId @ ", " @ %option @ ")");

	%name = Client::getName(%clientId);

	$playerCurrentSpell[%clientId] = $spellShell[%option];
}
function processMenuselectrweapon(%clientId, %item)
{
	%list = GetAccessoryList(%clientId, 10, -1);

	Client::buildMenu(%clientId, "Projectiles:", "selectproj", true);
	for(%i = 0; GetWord(%list, %i) != -1; %i++)
	{
		%proj = GetWord(%list, %i);

		if(String::findSubStr($ProjRestrictions[%proj], "," @ %item @ ",") != -1)
			Client::addMenuItem(%clientId, %curitem++ @ $beltitem[%proj, "Name"], %item @ " " @ %proj);
	}
	return;
}
function processMenuselectproj(%clientId, %itemandproj)
{
	%item = GetWord(%itemandproj, 0);
	%proj = GetWord(%itemandproj, 1);

	storeData(%clientId, "LoadedProjectile " @ %item, %proj);
}

function processMenuOtheropt(%clientId, %option)
{
	dbecho($dbechoMode, "processMenuOtheropt(" @ %clientId @ ", " @ %option @ ")");

	%opt = GetWord(%option, 0);
	%cl = GetWord(%option, 1);
	if(%opt == "mute")
	      %clientId.muted[%cl] = true;
	else if(%opt == "unmute")
		%clientId.muted[%cl] = "";
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
      remoteEval(%clientId, "setInfoLine", 5, "URL: " @ $Client::info[%selId, 4]);
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
}


function admin::teleport(%clientId, %id){
	if(!admin::hasPermissionToMod(%clientId, %id)){
		Client::sendMessage(%clientId, 0, "Could not process command: Target admin clearance level too high.");
		return;
	}
	%player = Client::getOwnedObject(%clientId);
	if(GameBase::getLOSinfo(%player, 1000)){
		GameBase::setPosition(%id, $los::position);
		CheckAndBootFromArena(%id);
	}
	else if(GameBase::getLOSinfo(%player, 2000)){
		GameBase::setPosition(%id, $los::position);
		CheckAndBootFromArena(%id);
	}
	else if(GameBase::getLOSinfo(%player, 6000)){
		GameBase::setPosition(%id, $los::position);
		CheckAndBootFromArena(%id);
	}
	else{
		Client::sendMessage(%TrueClientId, 0, "Need to look at something.");
		return;
	}
	Client::sendMessage(%clientId, 0, "Teleporting " @ rpg::getname(%id) @ " to " @ $los::position @ ".");
}


function admin::teleportto(%clientId, %id){
	if(!admin::hasPermissionToMod(%clientId, %id))
		Client::sendMessage(%clientId, 0, "Could not process command: Target admin clearance level too high.");
	else
	{
		Client::sendMessage(%clientId, 0, "Teleporting " @ rpg::getname(%clientId) @ " (" @ %clientId @ ") to " @ rpg::getname(%id) @ " (" @ %id @ ").");
		GameBase::setPosition(%clientId, GameBase::getPosition(%id));
		CheckAndBootFromArena(%clientId);
	}
}
function admin::forcerecall(%clientId, %id){
	if(!admin::hasPermissionToMod(%clientId, %id)){
		Client::sendMessage(%clientId, 0, "Could not process command: Target admin clearance level too high.");
		return;
	}
	Client::sendMessage(%clientId, 0, "Teleporting " @ rpg::getname(%id) @ " to the recall point.");
	FellOffMap(%id);
}