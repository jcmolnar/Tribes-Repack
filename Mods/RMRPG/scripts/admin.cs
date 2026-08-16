

$max_ppl = 24;
$injail = 0;

for(%i = 0; %i <= 6; %i++) {
	$jail[%i] = 0;
	$jailttl[%i] = 0;
}

function jail_them() {
	$injail=0;
	for(%mynum=1;%mynum <= $max_ppl; %mynum++)
	{
		if($jailttl[%mynum]>0&&$jail[%mynum]!=0)
		{
			$injail=1;
			set_position($jail[%mynum],"-84.8372 -1410.26 127.158");//jailed
			$jailttl[%mynum]--;
		}
		if($jailttl[%mynum]<=0&&$jail[%mynum]!=0)
		{
			set_position($jail[%mynum],"-11.8967 -1715.55 75");//escape
			$jail[%mynum]=0;
			$jailttl[%mynum]=0;
		}
	}
	if($injail==1)
	{
		schedule("jail_them();",1);
	}
}
//new//

$curVoteTopic = "";
$curVoteAction = "";
$curVoteOption = "";
$curVoteCount = 0;


function Admin::changeMissionMenu() {}
function processMenuCMType() {}
function processMenuCMission() {}

function remoteAdminPassword(%Client, %password) {

	dbecho($dbechoMode, "remoteAdminPassword("@%Client@", "@%password@")");

	if($AdminPassword != "" && %password == $AdminPassword[4]) {
		%Client.adminLevel = 4;
	}
}

function remoteSetPassword(%Client, %password)
{
	dbecho($dbechoMode, "remoteSetPassword("@%Client@", "@%password@")");

	if(%Client.adminLevel >= 5)
		$Server::Password = %password;
}

function remoteSetTimeLimit() {}
function remoteSetTeamInfo() {}

function remoteVoteYes(%Client) {
	%Client.notready = "yes";
	centerprint(%Client, "", 0);
}

function remoteVoteNo(%Client) {
	%Client.notready = "no";
	centerprint(%Client, "", 0);
}

function Admin::startMatch() {}
function Admin::setTeamDamageEnable() {}

function Admin::kick(%admin, %Client, %ban) {
	dbecho($dbechoMode, "Admin::kick("@%admin@", "@%Client@", "@%ban@")");

   if(%admin == -1 || %admin.adminLevel >= 4) {
      if(%ban && %admin.adminLevel < 5)
         return;

      if(%ban) {
         %word = "banned";
         %cmd = "BAN: ";
      }
      else {
         %word = "kicked";
         %cmd = "KICK: ";
      }
      if(%Client.adminLevel >= 5) {
         if(%admin == -1)
            messageAll(0, "A super admin cannot be "@%word@".");
         else
            Client::sendMessage(%admin, 0, "A super admin cannot be "@%word@".");
         return;
      }
      %ip = Client::getTransportAddress(%Client);

      echo(%cmd @ %admin@" "@%Client@" "@%ip);

      if(%ip == "")
         return;
      if(%ban)
         BanList::add(%ip, 1800);
      else
         BanList::add(%ip, 180);

      %name = Client::getName(%Client);

      if(%admin == -1) {
         MessageAll(0, %name@" was "@%word@" from vote.");
         Net::kick(%Client, "You were "@%word@" by  consensus.");
      }
      else {
         MessageAll(0, %name@" was "@%word@" by "@Client::getName(%admin)@".");
         Net::kick(%Client, "You were "@%word@" by "@Client::getName(%admin));
      }
   }
}

function Admin::setModeFFA() {}

function Admin::setModeTourney() {}

function Admin::voteFailed() {
	dbecho($dbechoMode, "Admin::voteFailed()");

   $curVoteInitiator.numVotesFailed++;

   if($curVoteAction == "kick" || $curVoteAction == "admin")
      $curVoteOption.voteTarget = "";
}

function Admin::voteSucceded() {

	dbecho($dbechoMode, "Admin::voteSucceded()");

	$curVoteInitiator.numVotesFailed = "";
	%jailTime=GetWord($curVoteAction,1);
	$curVoteAction=GetWord($curVoteAction,0);
	if(%jailTime==""||%jailTime==-1) {
		%jailTime=2;
	}
	if($curVoteAction == "jail") {
		messageAll(0, "Vote to "@$curVoteAction@" "@Client::getName($curVoteOption)@" for "@%jailTime@" minutes passed.");
			if($curVoteOption.adminLevel>0&&$curVoteOption.adminLevel<5) {
				$curVoteOption.adminLevel=-10;
			}
			else {
				$curVoteOption.adminLevel=0;
			}
			%temp_loc=0;
			for(%mynum=1;%mynum<=$max_ppl;%mynum++) {
				if($jail[%mynum]==$curVoteOption)
				{
					%temp_loc=1;
					break;
				}
			}
			if(%temp_loc==0) {
				for(%mynum=1;%mynum<=$max_ppl;%mynum++) {
					if($jail[%mynum]==0) {
						$jail[%mynum]=$curVoteOption;
						$jailttl[%mynum]=%jailTime*60;
						if($injail==0) {
							$injail=1;
							for(%fnum=1;%fnum<=$rentmax;%fnum++) {
								if($rentedbotowner[%fnum]==$curVoteOption) {
									felloffmap($rentedbotid[%fnum]);
									//$rentedbot[%fnum]=0;
									$rentedbotid[%fnum]=0;
									$rentedbotowner[%fnum]=0;
								}
							}
							schedule("jail_them();",1);
						}
						break;
					}
				}
			}
			messageAll(0, Client::getName($curVoteOption)@" has been jailed.");
			if($curVoteOption.menuMode == "options")
				Game::menuRequest($curVoteOption);
		//}
		//$curVoteOption.voteTarget = false;
	}
	else if($curVoteAction == "kick") {
		//if($curVoteOption.voteTarget)
			Admin::kick(-1, $curVoteOption);
	}
	else if($curVoteAction == "admin") {
		//if($curVoteOption.voteTarget)
		//{
			$curVoteOption.adminLevel = 4;
			messageAll(0, Client::getName($curVoteOption)@" has become an administrator.");
			if($curVoteOption.menuMode == "options")
				Game::menuRequest($curVoteOption);
		//}
		//$curVoteOption.voteTarget = false;
	}
}

function Admin::countVotes(%curVote) {
	dbecho($dbechoMode, "Admin::countVotes("@%cureVote@")");

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
		if(%cl.notready == "yes")
		{
			%votesFor++;
			%totalVotes++;
		}
		else if(%cl.notready == "no")
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
	%margin = 1;
	if($curVoteAction == "admin")
	{
		%margin = $Server::VoteAdminWinMargin;
		%totalVotes = %votesFor + %votesAgainst + %votesAbstain;
		if(%totalVotes < %minVotes)
			%totalVotes = %minVotes;
	}
	if(%votesFor / %totalVotes >= %margin)
	{
		messageAll(0, "Vote to "@$curVoteTopic@" passed: "@%votesFor@" to "@%votesAgainst@" with "@%totalClients - (%votesFor + %votesAgainst)@" abstentions.");
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
					if(%cl.notready == "yes")
						%votesFor++;
				}
			}
			if(%totalVotes >= $Server::MinVotes && %votesFor / %totalVotes >= $Server::VoteWinMargin)
			{
				messageAll(0, "Vote to "@$curVoteTopic@" passed: "@%votesFor@" to "@%totalVotes - %votesFor@".");
				Admin::voteSucceded();
				$curVoteTopic = "";
				return;
			}
		}
		messageAll(0, "Vote to "@$curVoteTopic@" did not pass: "@%votesFor@" to "@%votesAgainst@" with "@%totalClients - (%votesFor + %votesAgainst)@" abstentions.");
		Admin::voteFailed();
	}
	$curVoteTopic = "";
}

function Admin::startVote(%Client, %topic, %action, %option) {
	dbecho($dbechoMode, "Admin::startVote("@%Client@", "@%topic@", "@%action@", "@%option@")");

	if(%Client.lastVoteTime == "")
		%Client.lastVoteTime = -$Server::MinVoteTime;

	// we want an absolute time here.
	%time = getIntegerTime(true) >> 5;
	%diff = %Client.lastVoteTime + $Server::MinVoteTime - %time;

	if(%diff > 0)
	{
		Client::sendMessage(%Client, 0, "You can't start another vote for "@floor(%diff)@" seconds.");
		return;
	}
	if($curVoteTopic == "")
	{
		if(%Client.numFailedVotes)
			%time += %Client.numFailedVotes * $Server::VoteFailTime;

		%Client.lastVoteTime = %time;
		$curVoteInitiator = %Client;
		$curVoteTopic = %topic;
		$curVoteAction = %action;
		$curVoteOption = %option;
		if(%action == "kick")
			$curVoteOption.kickTeam = GameBase::getTeam($curVoteOption);
		$curVoteCount++;
		bottomprintall("<jc><f1>"@Client::getName(%Client)@" <f0>initiated a vote to <f1>"@$curVoteTopic, 10);
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			%cl.notready = "";
		%Client.notready = "yes";
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			if(%cl.menuMode == "options")
				Game::menuRequest(%cl);
				//Game::menuRequest(%Client);
		schedule("Admin::countVotes("@$curVoteCount@", true);", $Server::VotingTime, 35);
	}
	else
	{
		Client::sendMessage(%Client, 0, "Voting already in progress.");
	}
}

function Game::menuRequest(%Client) {

	if(%Client.IsInvalid)
		return;
	else if(%Client.choosingGroup) {
		MenuGroup(%Client);
		return;
	}
	else if(%Client.choosingClass) {
		MenuClass(%Client);
		return;
	}

	%Client.bulk = 1;

	%curItem = 0;
	Client::buildMenu(%Client, "Options", "options", true);
	if($curVoteTopic != "" && %Client.notready == "")
	{
		Client::addMenuItem(%Client, %curItem++@"Vote YES to "@$curVoteTopic, "vyes");
		Client::addMenuItem(%Client, %curItem++@"Vote NO to "@$curVoteTopic, "vno");
	}
	else
	{
		if(%Client.selClient && %Client.selClient != %Client) {
			%sel = %Client.selClient;
			%selname = Client::getName(%sel);

			if(%Client != %sel) {
				if(isInGroupList(%Client, %sel))
					Client::addMenuItem(%Client, %curItem++@"Remove from group-list", "remgroup "@%sel);
				else
					Client::addMenuItem(%Client, %curItem++@"Add to group-list", "addgroup "@%sel);

				if($ClientData[%Client, partyOwned]) {
					if(IsInCommaList($ClientData[%Client, partylist], %selname))
						Client::addMenuItem(%Client, %curItem++ @ "Remove from your party", "remparty " @ %sel);
					else {
						if(CountObjInCommaList($ClientData[%Client, partylist]) < $maxpartymembers) {
							%p = IsInWhichParty(%selname);
							if(%p == -1)
								Client::addMenuItem(%Client, %curItem++ @ "Invite to your party", "addparty " @ %sel);
							else if(GetWord(%p, 1) == "i")
								Client::addMenuItem(%Client, %curItem++ @ "Cancel invitation", "cancelinv " @ %sel);
							else
								Client::addMenuItem(%Client, %curItem++ @ "(Can't invite, already in a party)", "");
						}
						else
							Client::addMenuItem(%Client, %curItem++ @ "(Can't invite, too many members)", "");
					}
				}
			}
		}
		else {
			if(!IsDead(%Client))
				Client::addMenuItem(%Client, %curItem++@"View your stats" , "viewstats");

			if($defaultTalk[%Client] == "#say")
				Client::addMenuItem(%Client, %curItem++@"Set default talk: #group" , "defgroup");
			else if($defaultTalk[%Client] == "#group")
				Client::addMenuItem(%Client, %curItem++@"Set default talk: #say" , "defsay");

		//	if(GetAccessoryList(%Client, 9, -1) != "") FIX!
		//		Client::addMenuItem(%Client, %curItem++@"Ranged weapons..." , "rweapons");

			if(!IsDead(%Client))
				Client::addMenuItem(%Client, %curItem++@"Ability points..." , "ap");

		//	if($LCKconsequence[%Client] == "miss")
		//		Client::addMenuItem(%Client, %curItem++@"Set LCK mode = death" , "lckdeath");
		//	else if($LCKconsequence[%Client] == "death")
		//		Client::addMenuItem(%Client, %curItem++@"Set LCK mode = miss" , "lckmiss");
		//
			if($Chocobo[%Client] >= 1)
				Client::addMenuItem(%Client, %curItem++@"Chocobo...", "Chocobo");

			Client::addMenuItem(%Client, %curItem++ @ "Party options..." , "partyoptions");
			Client::addMenuItem(%Client, %curItem++@"Next>>", "Other");
		}
	}
}
function processMenuOptions(%Client, %option) {

	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);

	if(%opt == "Other")
	{
		%sel = %Client.selClient;
		if(%sel == "") %sel = %Client;
		%name = Client::getName(%sel);

		Client::buildMenu(%Client, "Other options", "Otheropt", true);

	//	if($curVoteTopic == "")
	//	{
		//	if($PKflag[%Client]&&String::ICompare($GROUP[%Client], "Rogue") == 0) {}
		//	else {
		//		Client::addMenuItem(%Client, %curItem++@"Vote to jail "@%name, "vjail "@%sel);
		//	}
	//	}
		if(%Client.adminLevel >= 4) {
			Client::addMenuItem(%Client, %curItem++@"Kick "@%name, "kick "@%sel);
			if(%Client.adminLevel >= 5) {
				Client::addMenuItem(%Client, %curItem++@"Ban "@%name, "ban "@%sel);
			}
		}
		if(%Client.muted[%sel])
			Client::addMenuItem(%Client, %curItem++@"Unmute "@%name, "unmute "@%sel);
		else
			Client::addMenuItem(%Client, %curItem++@"Mute "@%name, "mute "@%sel);

		if($ignoreGlobal[%Client])
			Client::addMenuItem(%Client, %curItem++@"Turn ignore global OFF" , "gignoreoff");
		else
			Client::addMenuItem(%Client, %curItem++@"Turn ignore global ON" , "gignoreon");

		if($showexp[%Client])
			Client::addMenuItem(%Client, %curItem++@"Turn off Exp Bar" , "baroff");
		else
			Client::addMenuItem(%Client, %curItem++@"Turn on Exp Bar" , "baron");

		if(!$PKflag[%Client])
			Client::addMenuItem(%Client, %curItem++@"Enable PvP..." , "pk");
		else
			Client::addMenuItem(%Client, %curItem++@"Disable PvP...", "pk");
	//	Client::addMenuItem(%Client, %curItem++@"Next>>" , "MoreOptions");

		return;
	}
	//RPG
	else if(%opt == "viewstats") {
		if(%Client.adminlevel > 0)
			%extra = " <f2>-<f2>- <f0>Admin level <f1>"@%Client.adminlevel@" <f2>-<f2>-";
		%text1 = "<f1>"@Client::getName(%Client)@", LVL "@getFinalLVL(%Client)@" "@$RACE[%Client]@" "@$CLASS[%Client]@%extra;

		%text2 = "<f0>HP: <f1>"@getHP(%Client)@" <f0>/ <f1>"@$MaxHP[%Client]@"\n";
		%text2 = %text2@"<f0>MP: <f1>"@getMANA(%Client)@" <f0>/ <f1>"@getMaxMANA(%Client)@"\n";
		%text2 = %text2@"<f0>STA: <f1>"@round(getSTA(%Client))@" <f0>/ <f1>"@getSTAMINA(%Client);

		%text3 = "\n\n\n<f0>DEF: <f1>"@getFinalDEF(%Client)@"\n";
		%text3 = %text3@"<f0>MDEF: <f1>"@getFinalMDEF(%Client)@"\n\n";

		%text4 = "\n\n\n\n\n<f0>LCK: <f1>"@getFinalLCK(%Client)@"\n";
		%text4 = %text4@"<f0>EXP C: <f1>"@ShowExp(%Client)@"\n";
		%text4 = %text4@"<f0>EXP N: <f1>"@getTNL(%Client, True)@"\n\n";

		%text4 = %text4@"<f0>GIL: <f1>"@FixM($COINS[%Client])@" <f2>-- <f0>Bank: <f1>"@FixM($BANK[%Client]);
		%text5 = "\n\n\n\n\n\n\n\n\n<f0>Total: <f1>"@FixM($COINS[%Client] + $BANK[%Client])@"\n\n";

		%text5 = %text5@"<f0>Weight: <f1>"@round(GetWeight(%Client))@" <f0>/ <f1>"@GetMaxWeight(%Client)@"\n\n";
//		%text5 = %text5@"<f0>"@$PlayerInfo[%Client];

		remoteEval(%Client, "StatsText", %text1, %text2, %text3, %text4, %text5);
		remoteEval(%Client, "RefreshHPMPEXP", Fix(getHP(%Client), %Client, HP), Fix(getMANA(%Client), %Client, MP), Fix(getSTA(%Client), %Client, STA), Fix(getTNL(%Client, Strip), %Client, EXP), $ShowEXP[%Client]);

		return;
	}
	else if(%opt == "defgroup")
	{
		$defaultTalk[%Client] = "#group";
	}
	else if(%opt == "defsay")
	{
		$defaultTalk[%Client] = "#say";
	}
	else if(%opt == "addgroup")
	{
		if(countObjInCommaList($grouplist[Client::getName(%Client)]) <= 30) {
			%name = Client::getName(%cl);
			%name2 = Client::getName(%Client);
			$grouplist[%name2] = $grouplist[%name2] @ %name @ $sepchar;
			Client::sendMessage(%cl, $MsgBeige, %name2@" has added you to his/her group-list.");
			Client::sendMessage(%Client, $MsgBeige, %name@" is now on your group-list.");
		}
		else
			Client::sendMessage(%Client, $MsgRed, "You have too many people on your group-list.");
	}
	else if(%opt == "remgroup")
	{
		%name = Client::getName(%cl);
		%name2 = Client::getName(%Client);
		$grouplist[%name2] = String::replace($grouplist[%name2], %name @ $sepchar, "");
		Client::sendMessage(%cl, $MsgBeige, %name2@" has removed you from his/her group-list.");
		Client::sendMessage(%Client, $MsgBeige, %name@" is no longer on your group-list.");
	}
//	else if(%opt == "lckdeath") {
//		$LCKconsequence[%Client] = "death";
//	}
	else if(%opt == "addparty")
	{
		%Client.invitee[%cl] = True;
		Client::sendMessage(%cl, $MsgBeige, Client::getName(%Client)@" has invited you to join his/her party.");
		Client::sendMessage(%Client, $MsgBeige, "You have invited "@Client::getName(%cl)@" to join your party.");
	}
	else if(%opt == "remparty")
	{
		%name = Client::getName(%cl);
		RemoveFromParty(%Client, %name);
	}
	else if(%opt == "cancelinv")
	{
		%Client.invitee[%cl] = "";
		Client::sendMessage(%cl, $MsgRed, Client::getName(%Client)@" has cancelled his invitation.");
		Client::sendMessage(%Client, $MsgBeige, "You cancelled your invitation to "@Client::getName(%cl)@".");
	}
	else if(%opt == "ap")
	{
		MenuAP(%Client);
		return;
	}
	else if(%opt == "partyoptions")
	{
		Client::buildMenu(%Client, "Party options", "partyopt", true);

		if($ClientData[%Client, partyOwned])
			Client::addMenuItem(%Client, "xDisband party", "disbandparty");
		else
			Client::addMenuItem(%Client, "cCreate party", "createparty");

		%name = Client::getName(%Client);
		if( (%p = IsInWhichParty(%name)) != -1)
		{
			%id = GetWord(%p, 0);
			%inv = GetWord(%p, 1);
			if(%inv == -1)
			{
				//this player is in the party
				Client::addMenuItem(%Client, "pLeave current party", "leaveparty "@%id);
			}
			else if(%inv == "i")
			{
				//this player is being invited
				Client::addMenuItem(%Client, "pAccept "@Client::getName(%id)@"'s party invitation", "acceptinv " @ %id);
			}
		}

		%list = $ClientData[%Client, partylist];
		for(%p = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999))
		{
			%w = String::NEWgetSubStr(%list, 0, %p);
			Client::addMenuItem(%Client, %curitem++ @ "Remove " @ %w, "remparty " @ %w);
		}
	}
	else if(%opt == "vyes"||%option == "vyes")// && %cl == $curVoteCount)
	{
		%Client.notready = "yes";
	 	centerprint(%Client, "", 0);
	}
	else if(%opt == "vno"||%option == "vno")// && %cl == $curVoteCount)
	{
		%Client.notready = "no";
	      centerprint(%Client, "", 0);
	}
	else if(%opt == "Chocobo")
		MenuChocobo(%Client);

}

function processMenupartyopt(%Client, %option) {

	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);

	if(%opt == "disbandparty")
		DisbandParty(%Client);

	else if(%opt == "createparty")
		CreateParty(%Client);

	else if(%opt == "remparty")
		RemoveFromParty(%Client, %cl);

	else if(%opt == "acceptinv") {
		%name = Client::getName(%Client);
		if( (%p = IsInWhichParty(%name)) != -1) {
			%id = GetWord(%p, 0);
			%inv = GetWord(%p, 1);
			if(%inv == "i")
				AddToParty(%id, %name);
		}
	}
	else if(%opt == "leaveparty")
		RemoveFromParty(%cl, Client::getName(%Client));

}

function processMenuPvP(%Client) {

	if(!$PKflag[%Client]) {
		Client::sendMessage(%Client, $MsgBeige, "PvP Enabled.");
		$PKflag[%Client] = True;
	}
	else {
		Client::sendMessage(%Client, $MsgBeige, "PvP Disabled.");
		$PKflag[%Client] = "";
	}
}
function processMenuOtheropt(%Client, %option) {

	%opt = GetWord(%option, 0);
	%cl = GetWord(%option, 1);

	if(%opt == "mute")
	      %Client.muted[%cl] = true;
	else if(%opt == "unmute")
		%Client.muted[%cl] = "";
	else if(%opt == "vkick")
	{
	      %cl.notreadyTarget = true;
	      Admin::startVote(%Client, "kick "@Client::getName(%cl), "kick", %cl);
	}
	else if(%opt == "vadmin")
	{
	      %cl.notreadyTarget = true;
	      Admin::startVote(%Client, "admin "@Client::getName(%cl), "admin", %cl);
	}
	else if(%opt == "kick")
	{
	      Client::buildMenu(%Client, "Confirm kick:", "kaffirm", true);
	      Client::addMenuItem(%Client, "1Kick "@Client::getName(%cl), "yes "@%cl);
	      Client::addMenuItem(%Client, "2Don't kick "@Client::getName(%cl), "no "@%cl);
	      return;
	}
	else if(%opt == "admin")
	{
	      Client::buildMenu(%Client, "Confirm admim:", "aaffirm", true);
	      Client::addMenuItem(%Client, "1Admin "@Client::getName(%cl), "yes "@%cl);
	      Client::addMenuItem(%Client, "2Don't admin "@Client::getName(%cl), "no "@%cl);
	      return;
	}
	else if(%opt == "ban")
	{
	      Client::buildMenu(%Client, "Confirm Ban:", "baffirm", true);
	      Client::addMenuItem(%Client, "1Ban "@Client::getName(%cl), "yes "@%cl);
	      Client::addMenuItem(%Client, "2Don't ban "@Client::getName(%cl), "no "@%cl);
		return;
	}
	else if(%opt == "vjail")
	{
		//add time limit
	      %cl.notreadyTarget = true;
		MenuJAILlist(%Client,%cl);
		return;
		//Admin::startVote(%Client, "jail "@Client::getName(%cl), "jail", %cl);
	}
	else if(%opt == "vyes"||%option == "vyes")// && %cl == $curVoteCount)
	{
		%Client.notready = "yes";
	 	centerprint(%Client, "", 0);
	}
	else if(%opt == "vno"||%option == "vno")// && %cl == $curVoteCount)
	{
		%Client.notready = "no";
	      centerprint(%Client, "", 0);
	}
	else if(%opt == "gignoreon")
		$ignoreGlobal[%Client] = True;
	else if(%opt == "gignoreoff")
		$ignoreGlobal[%Client] = "";
	else if(%opt == "baron") {
		$showexp[%Client] = True;
		remoteEval(%Client,"RefreshHPMPEXP", Fix(getHP(%Client), %Client, HP), Fix(getMANA(%Client), %Client, MP), Fix(getSTA(%Client), %Client, STA), Fix(getTNL(%Client, Strip), %Client, EXP), $ShowEXP[%Client]);
	}
	else if(%opt == "baroff") {
		$showexp[%Client] = "";
		remoteEval(%Client,"RefreshHPMPEXP", Fix(getHP(%Client), %Client, HP), Fix(getMANA(%Client), %Client, MP), Fix(getSTA(%Client), %Client, STA), Fix(getTNL(%Client, Strip), %Client, EXP), $ShowEXP[%Client]);
	}
	else if(%opt == "pk")
		processMenuPvP(%Client);

	//Game::menuRequest(%Client);
}

function remoteSelectClient(%Client, %selId)
{
	if(%Client.selClient != %selId)
	{
		%Client.selClient = %selId;
		if(%Client.menuMode == "options")
			Game::menuRequest(%Client);
		//remoteEval(%Client, "setInfoLine", 1, "Player Info for "@Client::getName(%selId)@":");
		//remoteEval(%Client, "setInfoLine", 2, "Real Name: "@$Client::info[%selId, 1]);
		//remoteEval(%Client, "setInfoLine", 3, "Email Addr: "@$Client::info[%selId, 2]);
		//remoteEval(%Client, "setInfoLine", 4, "Tribe: "@$Client::info[%selId, 3]);
		//remoteEval(%Client, "setInfoLine", 5, "URL: "@$Client::info[%selId, 4]);
		remoteEval(%Client, "setInfoLine", 5, "Real Name: "@$Client::info[%selId, 1]);
	}
}


function processMenuPickTeam(%Client, %team, %adminClient) { }
//NEW//
//for voting
function MenuJAILlist(%Client,%toJail)
{
	Client::buildMenu(%Client, "Jail Time","JAILlist",true);
	//Client::addMenuItem(%Client, "1OneMinute", %toJail@" "@1);
	Client::addMenuItem(%Client, "2TwoMinutes", %toJail@" "@2);
	Client::addMenuItem(%Client, "5FiveMinutes", %toJail@" "@5);
	Client::addMenuItem(%Client, "0TenMinutes", %toJail@" "@10);
	Client::addMenuItem(%Client, "3ThirtyMinutes", %toJail@" "@30);
	//Client::addMenuItem(%Client, "6OneHour", %toJail@" "@60);
	Client::addMenuItem(%Client, "kKick", %toJail@" "@kick);
	Client::addMenuItem(%Client, "xBack", back);
	return;
}
function processMenuJAILlist(%Client, %opt)
{
	%cl=getWord(%opt,0);
	%opt=getWord(%opt,1);
	if(%opt != -1&&%opt != "back" &&%opt != "kick")
	      Admin::startVote(%Client, "jail "@Client::getName(%cl)@" for "@%opt@" minutes", "jail "@%opt, %cl);

	else if(%opt == "kick")
	      Admin::startVote(%Client, "kick "@Client::getName(%cl), "kick", %cl);

	else
		return;
}
