
$MsgTypeSystem = 0;
$MsgTypeGame = 1;
$MsgTypeChat = 2;
$MsgTypeTeamChat = 3;
$MsgTypeCommand = 4;

$MsgWhite = 0;
$MsgRed = 1;
$MsgBeige = 2;
$MsgGreen = 3;

function remoteSay(%Client, %team, %message) {

	if(%message == "" || %Client == -1)
		return;

	if(Player::isAiControlled(%Client))
		%Client.adminlevel = 3;

	%time = getIntegerTime(true) >> 5;
	if(%time - %Client.lastSayTime <= $sayDelay && !%Client.adminlevel >= 1)
		return;
	%Client.lastSayTime = %time;

	if(%Client.IsInvalid) {
		if(GetWord(%message,0) == "#logon") { // logon with your password
			if(GetWord(%message,1) != "")
				$mypassword[%Client] = GetWord(%message, 1);
			else
				Client::sendMessage(%Client, 0, "Put in your password.");
		}
		return;
	}

	if($ClientData[%Client, Petrify] > 0 && getWord(%message,0) != "#savecharacter" && getWord(%message,0) != "#save") {
		Client::sendMessage(%Client, 0, "You are Petrified!");
		return;
	}

	%sendername = Client::getName(%Client);

//	if(String::findSubStr(%message, "\n\n\n\n\n\n") != -1) {
//
//		%ip = Client::getTransportAddress(%Client);
//		BanList::add(%ip, 60 * 5);
//		Net::Kick(%Client, "You have been banned for 5mins.");
//		echo(%sendername@" ("@%Client@") tried to crash clients and has been banned for 5mins.");
//		return;
//	}

	if(linebreak::check(%message,GetWord(%message,0)))
	{
		MessageAll(0,"Possible LineBreak blocked from "@client::getname(%Client)@".");
		return;
	}

     if($dedicated) {
		 %msg = client::getname(%Client)@" \""@escapeString(%message)@"\"";
		echo("SAY: "@%msg);
	}

	if($exportChat) {
		%ip = Client::getTransportAddress(%Client);
		$log::msg["[\""@%senderName@"\"]"] = RMgetTime(MDY)@" "@RMgetTime()@" | IP: "@%ip@" MSG: "@%message;
		export("log::msg[\""@%senderName@"\"*", "temp\\log$ @ "@%senderName@".cs", true);
	}

	// check for flooding if it's a broadcast OR if it's team in FFA
	if($Server::FloodProtectionEnabled && (!$Server::TourneyMode || !%team)) {
		// we use getIntTime here because getSimTime gets reset.
		// time is measured in 32 ms chunks... so approx 32 to the sec
		%time = getIntegerTime(true) >> 5;
		if(%Client.floodMute)
		{
			%delta = %Client.muteDoneTime - %time;
			if(%delta > 0)
			{
				Client::sendMessage(%Client, $MSGTypeGame, "FLOOD! You cannot talk for "@%delta@" seconds.");
				return;
			}
			%Client.floodMute = "";
			%Client.muteDoneTime = "";
		}
		%Client.floodMessageCount++;
		// funky use of schedule here:
		schedule(%Client@".floodMessageCount--;", 5, %Client);
		if(%Client.floodMessageCount > 4)
		{
			%Client.floodMute = true;
			%Client.muteDoneTime = %time + 10;
			Client::sendMessage(%Client, $MSGTypeGame, "FLOOD! You cannot talk for 10 seconds.");
			return;
		}
	}

	if($ClientData[%Client, Alvl] > 0)
		%message = DrunkenBastard(%Client, %message);

	//check for shopping
	%Client.bulk = 1;
//	if(%Client.currentShop != "" || %Client.currentBank != "") {
		if(%message == floor(%message)) {
			%Client.bulk = %message;
		}
//	}

	//parse message
	%botTalk = False;

	if(String::NEWgetSubStr(%message, 0, 1) != "#")
	{
		if(%team)
			%message = "#zone "@%message;
		else
			%message = $defaultTalk[%Client]@" "@%message;
	}

	%w1 = GetWord(%message, 0);

	%cropped = String::NEWgetSubStr(%message, (String::len(%w1)+1), String::len(%message)-(String::len(%w1)+1));

	//if(%Client.adminLevel >= 3)
	//{
	//	%cropped=String::replace(%cropped,"mystring",$my_string[%Client]);
	//}

//============================================================================================================
	if($ClientData[%Client, Mute] > 0) {
		if(%w1 == "#cast" || %w1 == "#say" || %w1 == "#shout" || %w1 == "#tell" || %w1 == "#global" || %w1 == "#zone" || %w1 == "#group") {
			Client::sendMessage(%Client, 0, "You are Mute.");
			return;
		}
	}
	else {
		if(%w1 == "#say") {
			if(getSTA(%Client) < 0.1) {
				Client::sendMessage(%Client, $MsgWhite, "You are far to weak to even say a word...");
				return;
			}
			for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))  {
				%talkingPos = GameBase::getPosition(%Client);
				%receivingPos = GameBase::getPosition(%cl);
				%distVec = Vector::getDistance(%talkingPos, %receivingPos);
				if(%distVec <= $maxSAYdistVec) {
					if(!%cl.muted[%Client] && %cl != %Client)
						Client::sendMessage(%cl, $MsgWhite, %sendername@" says, \""@%cropped@"\"");
				}
			}
			Client::sendMessage(%Client, $MsgWhite, "You say, \""@%cropped@"\"");
			refreshSTAMINA(%Client, 0.1);
			%botTalk = True;
		}
		if(%w1 == "#shout") {
			if(getSTA(%Client) < 0.3) {
					Client::sendMessage(%Client, $MsgWhite, "You don't have enough Stamina!");
					return;
			}
			for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) {
				%talkingPos = GameBase::getPosition(%Client);
				%receivingPos = GameBase::getPosition(%cl);
				%distVec = Vector::getDistance(%talkingPos, %receivingPos);
				if(%distVec <= $maxSHOUTdistVec) {
					if(!%cl.muted[%Client] && %cl != %Client)
						Client::sendMessage(%cl, $MsgWhite, %sendername@" shouts, \""@%cropped@"\"");
				}
			}
			Client::sendMessage(%Client, $MsgWhite, "You shouted, \""@%cropped@"\"");
			refreshSTAMINA(%Client, 0.3);

			%botTalk = True;
		}

		if(%client.jailed == "true")
			return;

		if(%w1 == "#tell") {
			if(%cropped == "") {
				Client::sendMessage(%Client, 0, "syntax: #tell whoever, message");
			}
			else {
				if(getSTA(%Client) < 1) {
					Client::sendMessage(%Client, $MsgWhite, "You don't have enough Stamina!");
					return;
				}
				%pos1 = 0;
				%pos2 = String::findSubStr(%cropped, ",");
				%name = String::NEWgetSubStr(%cropped, %pos1, %pos2-%pos1);
				%final = String::NEWgetSubStr(%cropped, %pos2 + 2, String::len(%cropped)-%pos2-2);
				%cl = NEWgetClientByName(%name);
				if(%cl != -1) {
					%n = Client::getName(%cl);
					if(!%cl.muted[%Client]) {

						Client::sendMessage(%cl, $MsgBeige, %sendername@" tells you, \""@%final@"\"");
						if(%cl != %Client) Client::sendMessage(%Client, $MsgBeige, "You tell "@%n@", \""@%final@"\"");
						%cl.replyTo = %senderName;
						refreshSTAMINA(%Client, 1);
					}
					else
						Client::sendMessage(%Client, 0, %n@" has muted you.");
				}
				else
					Client::sendMessage(%Client, 0, "Invalid playerd name.");
			}
			%botTalk = True;
		}
		if(%w1 == "#r") {
			if(%cropped == "")
				Client::sendMessage(%Client, 0, "syntax: #r message");
			else {
				if(getSTA(%Client) < 1) {
					Client::sendMessage(%Client, $MsgWhite, "You don't have enough Stamina!");
					return;
				}
				%name = %Client.replyTo;
				if(%name != "")
				{
					%cl = NEWgetClientByName(%name);

					if(%cl != -1)
					{
						if(!%cl.muted[%eClient])
						{
							Client::sendMessage(%cl, $MsgBeige, %senderName@" tells you, \""@%cropped@"\"");
							if(%cl != %Client)
								Client::sendMessage(%Client, $MsgBeige, "You tell " @ %name @ ", \""@%cropped@"\"");
							%cl.replyTo = %senderName;
							refreshSTAMINA(%Client, 1);
						}
					}
					else
						Client::sendMessage(%Client, $MsgWhite, "Invalid player name.");

					%botTalk = True;
				}
				else
					Client::sendMessage(%Client, $MsgWhite, "You haven't received a #tell to reply to yet.");
			}
		}
		if(%w1 == "#global") {
			if(getSTA(%Client) < 5) {
				Client::sendMessage(%Client, $MsgWhite, "You don't have enough Stamina!");
				return;
			}
			if(!$ignoreGlobal[%Client]) {
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) {
					 if(!%cl.muted[%Client] && %cl != %Client && !$ignoreGlobal[%cl])
						Client::sendMessage(%cl, $MsgGreen, "[GLBL] "@%sendername@" \""@%cropped@"\"");
				}
				Client::sendMessage(%Client, $MsgGreen, "[GLBL] \""@%cropped@"\"");
				refreshSTAMINA(%Client, 5);
			}
			else
				Client::sendMessage(%Client, $MsgRed, "You can't send a Global message when ignoring other Global messages.");
			return;
		}
		if(%w1 == "#zone") {
			if(getSTA(%Client) < 0.5) {
				Client::sendMessage(%Client, $MsgWhite, "You don't have enough Stamina!");
				return;
			}
			for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) {
				if(!%cl.muted[%Client] && %cl != %Client && $zone[%cl] == $zone[%Client])
					Client::sendMessage(%cl, $MsgGreen, "[ZONE] "@%senderName@" \""@%cropped@"\"");
			}
			Client::sendMessage(%Client, $MsgGreen, "[ZONE] \""@%cropped@"\"");
			refreshSTAMINA(%Client, 0.5);
			return;
		}
		if(%w1 == "#group") {
			if(getSTA(%Client) < 1) {
				Client::sendMessage(%Client, $MsgWhite, "You don't have enough Stamina!");
				return;
			}
			for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) {
				if(!%cl.muted[%Client] && %cl != %Client && isInGroupList(%Client, %cl)) {
					if(isInGroupList(%cl, %Client))
						Client::sendMessage(%cl, $MsgBeige, "[GRP] "@%sendername@" \""@%cropped@"\"");
					else {
						if($HasLoadedAndSpawned[%cl]) // stop spamming this error if the client hasn't spawned yet!
							Client::sendMessage(%Client, $MsgRed, Client::getName(%cl)@" does not have you on his/her group-list.");
					}
				}
			}
			Client::sendMessage(%Client, $MsgBeige, "[GRP] \""@%cropped@"\"");
			refreshSTAMINA(%Client, 1);
			return;
		}
	}
	if(%w1 == "#party" || %w1 == "#p") {
		if(getSTA(%Client) < 1) {
			Client::sendMessage(%Client, $MsgWhite, "You don't have enough Stamina!");
			return;
		}
		%list = GetPartyListIAmIn(%Client);
		for(%p = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999)) {
			%cl = NEWgetClientByName(String::NEWgetSubStr(%list, 0, %p));
			if(!%cl.muted[%Client] && %cl != %Client)
				Client::sendMessage(%cl, $MsgBeige, "[PRTY] "@%senderName@" \""@%cropped@"\"");
		}
		Client::sendMessage(%Client, $MsgBeige, "[PRTY] \""@%cropped@"\"");
		refreshSTAMINA(%Client, 1);

		return;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////
	if(IsDead(%Client) && %Client != 2048)
		return;

	if(%w1 == "#smith") {

		if(!%Client.IsSmithing && $ClientData[%Client, SmithStage] == "canSmith") {

			%tempsmith = $ClientData[%Client, tmpSmith];
			%cost = $ClientData[%Client, SmithCost];

			$ClientData[%Client, tmpSmith] = $ClientData[%Client, SmithStage] = "";
			$ClientData[%Client, SmithCost] = $ClientData[%Client, SmithMode] = "";

			remotePlayMode(%Client);

			if((%sc = GetSmithCombo(%Client, %tempsmith)) != 0) {

				%amt = GetWord(%cropped, 0);

				if(%amt <= 0)
					%amt = 1;
				if(%amt >= 100)
					%amt = 99;

				%cost *= %amt;

				if(HasThisStuff(%Client, getWord(%tempsmith, 0), %amt)) {
					if(%cost <= $COINS[%Client]) {

						playSound(SoundSmith, GameBase::getPosition(%Client));

						AI::sayLater(%Client, %Client.currentSmith, "Let me see what I can do...", "NULL");

						%item = $ClientData[%Client, GiveSmithItem];

						schedule("CompleteSmith(\""@%Client@"\", \""@%Client.currentSmith@"\", \""@%cost@"\", \""@%item@"\", \""@%tempsmith@"\", \""@%amt@"\");", 5.5, %Client);
						%Client.IsSmithing = True;

						return 1;
					}
					else {
						Client::sendMessage(%Client, $MsgRed, "You can't afford to smith this/these items.~wC_BuySell.wav");
						return 0;
					}
				}
				else {
					Client::sendMessage(%Client, $MsgRed, "You don't have the needed items.");
					return;
				}
			}
		}
	}
	//================================================================================================================
      if(%w1 == "#name") {
		if($CanName[%Client] == true && $OldName[%Client] == "") {
			if(%cropped != "") {
				%len = Chocobo::Getlen(%cropped);
				if(%len > 16) {
					Client::sendMessage(%Client, 0, "Your Chocobo name can't be more then 16 chars long.");
					Client::sendMessage(%Client, 0, "Enter a name for your Chocobo again.");
					$CanName[%Client] = true;
					return;
				}
			}
			if(%cropped != "") {
				%Check = Chocobo::InvalidChar(%cropped);
				if(%Check == false) {
					%Check = CheckForReservedWords(%cropped);
					if(%Check == "") {
						$ChocoboName[%Client, $ChocoboTempName[%Client]] = %cropped;
						Client::sendMessage(%Client, 0, "Your "@$ChocoboColor[%Client, $ChocoboTempName[%Client]]@" Chocobo's name is now "@$ChocoboName[%Client, $ChocoboTempName[%Client]]@".");
						$CanName[%Client] = false;
					}
					else {
						Client::sendMessage(%Client, 0, "You can't use a reserved word in your Chocobo name ("@%Check@").");
						Client::sendMessage(%Client, 0, "Enter a name for your Chocobo again.");
						$CanName[%Client] = true;
					}
				}
				else {
					Client::sendMessage(%Client, 0, "Your Chocob name can't have any of these chars  "@$Chocobo::invalidChars);
					Client::sendMessage(%Client, 0, "Enter a name for your Chocobo again.");
					$CanName[%Client] = true;
				}
			}
			else
				Chocobo::NewName(%Client, TryAgain, $ChocoboTempName[%Client]);
		}
		else if($CanName[%Client] == true && $OldName[%Client] == true) {
			if(%cropped != "") {
 				%Check = Chocobo::InvalidChar(%cropped);
				if(%Check == false) {
					%Check = CheckForReservedWords(%cropped);
					if(%Check == "") {
						$ChocoboName[%Client, $ChocoboTempName[%Client]] = %cropped;
						Client::sendMessage(%Client, 0, "You changed your "@$ChocoboColor[%Client, $ChocoboTempName[%Client]]@" Chocobo's name to "@$ChocoboName[%Client, $ChocoboTempName[%Client]]@".");
						$CanName[%Client] = false;
						$OldName[%Client] = false;
					}
					else {
						Client::sendMessage(%Client, 0, "You can't use a reserved word in your Chocobo name ("@%Check@").");
						Client::sendMessage(%Client, 0, "Enter a name for your Chocobo again.");
						$CanName[%Client] = true;
					}
				}
				else {
					Client::sendMessage(%Client, 0, "Your Chocob name can't have any of these chars  "@$Chocobo::invalidChars);
					Client::sendMessage(%Client, 0, "Enter a name for your Chocobo again.");
					$CanName[%Client] = true;
				}
			}
			else {
				Client::sendMessage(%Client, 0, "You didn't change your Chocobo's name.");
				$CanName[%Client] = false;
				$OldName[%Client] = false;
			}
		}
		return;
      }
	//===
	if(%w1 == "#trade") {
		if($CanTrade[%Client] == false) {
			if(%cropped != "") {
				if(%cropped == yes) {
					Client::sendMessage(%Client, 0, "You traded "@$ChocoboName[%Client, $MenuTradeChocoboId::Buyer[%Client]]@" for "@$ChocoboName[$MenuTradeHostId[%Client], $MenuTradeChocoboId::Host[$MenuTradeHostId[%Client]]]);
					Client::sendMessage($MenuTradeHostId[%Client], 0, "You traded "@$ChocoboName[$MenuTradeHostId[%Client], $MenuTradeChocoboId::Host[$MenuTradeHostId[%Client]]]@" for "@$ChocoboName[%Client, $MenuTradeChocoboId::Buyer[%Client]]);

					Chocobo::Trade($MenuTradeHostId[%Client], %Client, Traded);
				}
				else
					Chocobo::Trade($MenuTradeHostId[%Client], %Client, failedBuyer);
			}
			else
				Client::sendMessage(%Client, 0, "Type '#trade yes' if you want to make this trade! Or '#trade no' if you don't.");
		}
		else if($CanTrade[%Client] == true) {
			if(%cropped != "") {
				if(%cropped == no)
					Chocobo::Trade(%Client, $MenuTradeBuyerId[%Client], failedHost);
			}
			else
				Client::sendMessage(%Client, 0, "Type '#trade no' to cancel the trade!");
		}
		return;
	}
	if(%w1 == "#breed") {
		if($CanBreed[%Client] == false) {
			if(%cropped != "") {
				if(%cropped == yes) {
					Client::sendMessage(%Client, 0, "You breed "@$ChocoboName[%Client, $MenuBreedChocoboId::Buyer[%Client]]@" with "@$ChocoboName[$MenuBreedHostId[%Client], $MenuBreedChocoboId::Host[$MenuTradeHostId[%Client]]]);
					Client::sendMessage($MenuTradeHostId[%Client], 0, "You breed "@$ChocoboName[$MenuBreedHostId[%Client], $MenuBreedChocoboId::Host[$MenuBreedHostId[%Client]]]@" with "@$ChocoboName[%Client, $MenuBreedChocoboId::Buyer[%Client]]);

					Chocobo::Breed($MenuBreedHostId[%Client], %Client, Breeding);
				}
				else
					Chocobo::Breed($MenuBreedHostId[%Client], %Client, failedBuyer);
			}
			else
				Client::sendMessage(%Client, 0, "Type '#breed yes' if you want to breed these Chocobos! Or '#breed no' if you don't.");
		}
		else if($CanBreed[%Client] == true) {
			if(%cropped != "") {
				if(%cropped == no)
					Chocobo::Breed(%Client, $MenuTradeBuyerId[%Client], failedHost);
			}
			else
				Client::sendMessage(%Client, 0, "Type '#breed no' to cancel!");
		}
		return;
	}
	//if(%w1 == "#sell") {
	//	return; //===
	//}
	//if(%w1 == "#hunt") {
	//	return; //===
	//}
	if(%w1 == "#steal") {
			%time = getIntegerTime(true) >> 5;
			if(%time - %Client.lastStealTime > $stealDelay)
			{
				%Client.lastStealTime = %time;

				if((%reason = AllowedToSteal(%Client)) == "True")
				{
			//		if(String::ICompare($GROUP[%Client], "Rogue") == 0)
			//		{
						if(GameBase::getLOSinfo(Client::getOwnedObject(%Client), 1))
						{
							%id = Player::getClient($los::object);
							if(getObjectType($los::object) == "Player")
							{
								%victimName = Client::getName(%id);
								%stealerName = %senderName;
								%victimCoins = $COINS[%id];
								%fail = False;
								if(%victimCoins > 0)
								{
									if(String::ICompare($GROUP[%Client], "Rogue") == 0)
										%a = round( ( (getRandom()*getFinalDEX(%Client)) - (getRandom()*getFinalDEX(%id)) ) + ( getFinalLVL(%Client) - getFinalLVL(%id) ) );
									else
										%a = round( ( (getRandom()*(getFinalDEX(%Client)-$LimitSteal[$GROUP[%Client]])) - (getRandom()*getFinalDEX(%id)) ) + ( -getFinalLVL(%id) ) );
									if(%a > 0)
									{
										%amount = floor(%a * getRandom() * 50); // 1.2 100
										if(%amount > %victimCoins)
											%amount = %victimCoins;

										if(%amount > 0) {
											$COINS[%Client] += %amount;
											$COINS[%id] -= %amount;
											PerhapsPlayStealSound(%Client, %id, 0);

											Client::sendMessage(%Client, $MsgTypeChat, "You successfully stole "@%amount@" gil from "@%victimName@"!");

											RefreshAll(%Client);
					     					RefreshAll(%id);

											PostSteal(%Client, True, 0);
					  					}
										else
											%fail = True;
									}
									else
										%fail = True;

									if(%fail) {
				             			Client::sendMessage(%Client, $MsgRed, "You failed to steal from "@%victimName@"!");
				             			Client::sendMessage(%id, $MsgRed, %stealerName@" just failed to steal from you!");

										//UseSkill(%Client, $SkillStealing, False, True);
										PostSteal(%Client, False, 0);
									}
			            		}
								else
			               			Client::sendMessage(%Client, $MsgRed, %victimName@" doesn't appear to be carrying any gil...");
							}
						}
			//		}
			//		else {
			//			Client::sendMessage(%Client, $MsgWhite, "Only Rogues can steal.");
			//		}
				}
				else
					Client::sendMessage(%Client, $MsgRed, %reason);
		}
		return;
	}
	if(%w1 == "#game") {
		%name = getWord(%cropped, 0);
		%game = getWord(%cropped, 1);
		if(!String::Empty(%game) && !String::Empty(%name)) {
			if(String::findSubStr($GameList, %game@" ") != -1) {
				if(%name != AI) {
					%id = NEWgetClientByName(%name);
					%name = Client::getName(%id);
				}
				else {
					if(%game == "TicTacToe") {
						Client::sendMessage(%Client, 0, "Game accepted. Loading...");
						Games::SetUpClients(%Client, "AI", %game);
						return;
					}
					Client::sendMessage(%Client, 0, "Games AI supported TicTacToe.");
					return;
				}
				if(%id != -1 && !Player::isAiControlled(%id)) {
					if(Vector::getDistance(GameBase::getPosition(%Client), GameBase::getPosition(%id)) < 25) {
						$ClientData[%id, Invited]  = %Client;
						$ClientData[%id, _Game] = %game;
						Client::sendMessage(%Client, 0, "Sending invite to "@%name@" to play "@%game@".");
						Client::sendMessage(%id, 0, %senderName@" has invited you to play a game of "@%game@". Type '#playgame' to play.");

					}
					else
						Client::sendMessage(%Client, 0, "You are to far away to play a game with "@%name@".");
				}
				else
					Client::sendMessage(%Client, 0, "Invalid Name.");
			}
			else
				Client::sendMessage(%Client, 0, "Avaible games "@String::NEWgetSubStr($GameList, 0, String::len($GameList)-1)@".");
		}
		else
			Client::sendMessage(%Client, 0, "#game [PlayerName] [Game]");
		return;
	}
	if(%w1 == "#playgame") {
		if(!String::empty($ClientData[%Client, Invited])) {
			if(Vector::getDistance(GameBase::getPosition(%Client), GameBase::getPosition($ClientData[%Client, Invited])) < 25) {
				Client::sendMessage(%Client, 0, "Game accepted. Loading...");
				Client::sendMessage($ClientData[%Client, Invited], 0, "Game accepted. Loading...");
				Games::SetUpClients($ClientData[%Client, Invited], %Client, $ClientData[%Client, _Game]);
				$ClientData[%Client, Invited] = "";
				$ClientData[%Client, _Game] = "";
			}
			else {
				Client::sendMessage(%Client, 0, "You are to far away to play. Game Cancelled.");
				$ClientData[%Client, Invited] = "";
			}
		}
		return;
	}

	if(%w1 == "#savecharacter" || %w1 == "#save")
	{
            if(%Client.adminLevel >= 4)
            {
                  if(%cropped == "")
                  {
                        %r = SaveCharacter(%Client, True);
                        Client::sendMessage(%Client, 0, "Saving self ["@%Client@"] "@%r);
                  }
                  else
                  {
                        %id = NEWgetClientByName(%cropped);
                        if(%id)
                        {
                              %r = SaveCharacter(%id, True);
                              Client::sendMessage(%Client, 0, "Saving "@Client::getName(%id)@" ["@%id@"] "@%r);
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
            }
            else
            {
				%r = SaveCharacter(%Client, True);
				Client::sendMessage(%Client, 0, "Saving self ["@%Client@"] "@%r);
		}
		return;
	}
	if(%w1 == "#reload") {
		Client::reloadInv(%Client);
	}
	if(%w1 == "#whatismyclientid")
	{
		Client::sendMessage(%Client, 0, "Your clientId is "@%Client);
		return;
	}
	if(%w1 == "#whatismyplayerid")
	{
		Client::sendMessage(%Client, 0, "Your playerId is "@Client::getOwnedObject(%Client));
		return;
	}
	if(%w1 == "#dropgil")
	{
		%cropped = GetWord(%cropped, 0);

		if(%cropped == "all")
			%cropped = $COINS[%Client];
		else
			%cropped = floor(%cropped);

		if(%cropped <= 0)
			return;

		if($COINS[%Client] >= %cropped)// || %Client.adminLevel >= 3)
		{

			$COINS[%Client] -= %cropped;

			TossLootbag(%Client, "COINS "@%cropped, 10, False, 0);
			RefreshAll(%Client);

			Client::sendMessage(%Client, 0, "You dropped "@%cropped@" gil.");
			playSound(SoundMoney1, GameBase::getPosition(%Client));
		}
		else
			Client::sendMessage(%Client, 0, "You don't even have that much gil!");

		return;
	}
	if(%w1 == "#compass") {
		if(Client::HasItem(%Client, "Compass", "QuestList")) {
			if(%cropped == "")
                  Client::sendMessage(%Client, 0, "Use #compass town or #compass dungeon. (Do not specify which, simply write town or dungeon)");
            else {
				%mpos = GetNearestZone(%Client, %cropped, 4);

				if(%mpos != False) {
					%d = GetNESW(GameBase::getPosition(%Client), %mpos);

					Client::sendMessage(%Client, 0, "The nearest "@%cropped@" is "@%d@" of here.");
				}
				else
					Client::sendMessage(%Client, 1, "Error finding a zone!");
			}
		}
		else
			Client::sendMessage(%Client, 0, "You do not have a Compass.");

		return;
	}
	if(%w1 == "#advcompass") {
		if(%cropped == "")
			Client::sendMessage(%Client, 0, "Use #advcompass zone keyword");
		else {
			if(Client::HasItem(%Client, "Adv_Compass", "QuestItem")) {
				%obj = GetZoneByKeywords(%Client, %cropped, 3);

				if(%obj != False) {
					%mpos = Zone::getMarker(%obj);

					%d = GetNESW(GameBase::getPosition(%Client), %mpos);

					Client::sendMessage(%Client, 0, Zone::getDesc(%obj)@" is "@%d@" of here.");
				}
				else
					Client::sendMessage(%Client, 1, "Couldn't fine a zone to match those keywords.");
			}
			else {
				Client::sendMessage(%Client, $MsgWhite, "You do not have a Adv Compass.");
			}
		}
		return;
	}
	if(%w1 == "#defaulttalk") {
		if(%cropped != "") {
			$defaultTalk[%Client] = %cropped;
			Client::sendMessage(%Client, 0, "Changed Default Talk to "@$defaultTalk[%Client]@".");
		}
		else
			Client::sendMessage(%Client, 0, "Please specify what will be added to the beginning of each of your messages.");

		return;
	}
	if(%w1 == "#getinfo")
	{
            %cropped = GetWord(%cropped, 0);

            if(%cropped == "")
                  Client::sendMessage(%Client, 0, "Please specify a name.");
            else
            {
			%id = NEWgetClientByName(%cropped);
			if(%id != -1)
				DisplayGetInfo(%Client, %id, Client::getOwnedObject(%id));
			else
				Client::sendMessage(%Client, 0, "Invalid player name.");
		}
		return;
	}
	if(%w1 == "#setinfo")
	{
            if(%cropped == "")
                  Client::sendMessage(%Client, 0, "Please specify text.");
            else
            {
			$PlayerInfo[%Client] = %cropped;
                  Client::sendMessage(%Client, 0, "Info set.  Use #getinfo [name] to retrieve this type of information.");
		}
		return;
	}
	if(%w1 == "#addinfo")
	{
		if(%cropped == "")
			Client::sendMessage(%Client, 0, "Please specify text.");
		else if(String::len($PlayerInfo[%Client]) >= 60)
			Client::sendMessage(%Client, 0, "Can't add any more info because your current info is to long.");
		else {
			$PlayerInfo[%Client] = $PlayerInfo[%Client] @ %cropped;
			Client::sendMessage(%Client, 0, "Info added to the end of previous info.");
		}
		return;
	}
	 if(%w1 == "#w")
	{
		 %item = getCroppedItem(%cropped);

		  if(%item == "")
				 Client::sendMessage(%Client, 0, "Please specify an item (ex: Leather Armor = Leather_Armor).");
		 else  {
			%msg = WhatIs(%Client, %item);
			remoteEval(%Client, "SetPrint::TimeOut", floor(String::len(%msg) /15)); //bottomprint(%Client, %msg, floor(String::len(%msg) / 15));
		}
		return;
	}
	if(%w1 == "#ws" || %w1 == "#wspell")
	{
		%spell = %cropped;
		  if(%spell == "")
				 Client::sendMessage(%Client, 0, "Please specify a spell.");
		 else  {
			%msg = WhatIs(%Client, %spell, true);
			remoteEval(%Client, "SetPrint::TimeOut", floor(String::len(%msg) /15)); //bottomprint(%Client, %msg, floor(String::len(%msg) / 20));
		}
		return;
	}
	if(%w1 == "#spell" || %w1 == "#cast")
	{
		if($ClientData[%Client, Mute] > 0) {
			Client::sendMessage(%Client, 0, "You are Mute.");
			return;
		}
		if($SpellCastStep[%Client] == 1 && %Client.adminLevel < 5)
			Client::sendMessage(%Client, 0, "You are already casting a spell!");
		else if($SpellCastStep[%Client] == 2 && %Client.adminLevel < 5)
			Client::sendMessage(%Client, 0, "You are still recovering from your last spell cast.");

		else {
			if(%cropped == "")
				Client::sendMessage(%Client, 0, "Specify a spell.");

			else if(String::findSubStr(%cropped, "\"") == -1){
				BeginCastSpell(%Client, escapestring(%cropped),1);
			}
		}
		return;
	}
	if(%w1 == "#recall")
	{
		%zvel = floor(getWord(Item::getVelocity(%Client), 2));
		Client::sendMessage(%Client, $MsgRed, "ATTEMPTING RECALL");
		if(%zvel <= -100)
		{
			FellOffMap(%Client);

			%zv = "PASS";
		}
		else
			%zv = "FAIL";

		Client::sendMessage(%Client, $MsgBeige, "Z-Velocity check: "@%zv);

		if(%zv != "PASS" && !$tmprecall[%Client])
		{
			$tmprecall[%Client] = True;
			if(getFinalLVL(%Client) <= 20)
				%seconds = 15;
			else
				%seconds = 120;

			Client::sendMessage(%Client, $MsgBeige, "Stay at your current position for the next "@%seconds@" seconds to recall.");

			schedule("$tmprecall["@%Client@"] = \"\";if(Vector::getDistance(\""@GameBase::getPosition(%Client)@"\", GameBase::getPosition("@%Client@")) <= 1){FellOffMap("@%Client@");}", %seconds);

		}
		return;
	}
	if(%w1 == "#nolimit") {
		%cropped = getWord(%cropped, 0);
		if(%cropped == "true" || %cropped == "false") {

			$ClientData[%Client, WantsToDropOverLimit] = %cropped;
			Client::sendMessage(%Client, 0, "Setting #nolimit to "@%cropped@".");
		}
		else
			Client::sendMessage(%Client, 0, "#nolimit true or #nolimit false.");

		return;
	}
	if(%w1 == "#track")
	{
            %cropped = GetWord(%cropped, 0);

            if(%cropped == "")
                  Client::sendMessage(%Client, 0, "Please specify a name.");
            else
            {
			if(String::ICompare($CLASS[%Client], "Ranger") == 0 || %Client.adminLevel >= 5)
			{
				%id = NEWgetClientByName(%cropped);
				if(%id != -1)
				{
					%Clientpos = GameBase::getPosition(%Client);
					%idpos = GameBase::getPosition(%id);

					%dist = round(Vector::getDistance(%Clientpos, %idpos));
					%d = GetNESW(%Clientpos, %idpos);

					Client::sendMessage(%Client, $MsgWhite, %cropped@" is "@%d@" of here, "@%dist@" meters away.");
				}
				else
					Client::sendMessage(%Client, 0, "Invalid player name.");
			}
			else
				Client::sendMessage(%Client, $MsgWhite, "Only rangers can track.");
		}
		return;
	}
	if(%w1 == "#mypassword")
	{
	      %c1 = GetWord(%cropped, 0);

            if(%c1 != -1)
		{
			$password[%Client] = %c1;
			Client::sendMessage(%Client, 0, "Changed personal password to "@$password[%Client]@".");
		}
		else
			Client::sendMessage(%Client, 0, "Please specify a one-word password.");
		return;
	}
	if(%w1 == "#sleep")
	{
		if(( ($InSleepZone[%Client] != "" && %Client.sleepMode == "" && !IsDead(%Client)) || (String::ICompare($CLASS[%Client], "Ranger") == 0 && %Client.sleepMode == "" && !IsDead(%Client) && $zone[%Client] == "") ))
		{
			%Client.sleepMode = 1;
			Client::setControlObject(%Client, Client::getObserverCamera(%Client));
			Observer::setOrbitObject(%Client, Client::getOwnedObject(%Client), 30, 30, 30);
			refreshHPREGEN(%Client);
			refreshMANAREGEN(%Client);

			Client::sendMessage(%Client, $MsgWhite, "You fall asleep...  Use #wake to wake up.");
		}
		else
			Client::sendMessage(%Client, $MsgRed, "You can't seem to fall asleep here.");
		return;
	}
	if(%w1 == "#meditate")
	{
			if(%Client.sleepMode == "" && !IsDead(%Client) && $possessedBy[%Client].possessId != %Client)
			{
				%Client.sleepMode = 2;
				Client::setControlObject(%Client, Client::getObserverCamera(%Client));
				Observer::setOrbitObject(%Client, Client::getOwnedObject(%Client), 30, 30, 30);
				refreshHPREGEN(%Client);
				refreshMANAREGEN(%Client);

				Client::sendMessage(%Client, $MsgWhite, "You begin to meditate.  Use #wake to stop meditating.");
			}
			else
				Client::sendMessage(%Client, $MsgRed, "You can't seem to meditate.");

		return;
	}
	if(%w1 == "#wake") {
		if(%Client.sleepMode != "") {
			%Client.sleepMode = "";
			Client::setControlObject(%Client, %Client);
			refreshHPREGEN(%Client);
			refreshMANAREGEN(%Client);

			Client::sendMessage(%Client, $MsgWhite, "You awake.");
		}
		else
			Client::sendMessage(%Client, $MsgRed, "You aren't sleeping.");
		return;
	}
	if(%w1 == "#roll")
	{
	      %c1 = GetWord(%cropped, 0);

            if(%c1 != -1)
			Client::sendMessage(%Client, 0, %c1@": "@GetRoll(%c1));
		else
			Client::sendMessage(%Client, 0, "Please specify a roll (example: 1d6)");
		return;
	}
	if(%w1 == "#hide") {
		if(String::ICompare($GROUP[%Client], "Rogue") == 0 || %Client.adminLevel >= 5) {
			if(!$invisible[%Client] && !$blockHide[%Client]) {
				%closeEnoughToWall = Cap(GetFinalLVL(%Client)/15, 3.5, 8);

				%pos = GameBase::getPosition(%Client);

				%closest = 10000;
				for(%i = 0; %i <= 6.283; %i+= 0.52) {
					GameBase::getLOSinfo(Client::getOwnedObject(%Client), 25, "0 0 "@%i);
					%dist = Vector::getDistance(%pos, $los::position);
					if(%dist < %closest && $los::position != "0 0 0" && $los::position != "")
						%closest = %dist;
				}
				if(%closest <= %closeEnoughToWall)
				{
					Client::sendMessage(%Client, $MsgBeige, "You are successful at Hide In Shadows.");

					GameBase::startFadeOut(%Client);
					$invisible[%Client] = True;

					%grace = Cap(5 + getFinalLVL(%Client), 5, 100);
					WalkSlowInvisLoop(%Client, 5, %grace);
				}
				else
					Client::sendMessage(%Client, $MsgWhite, "You were unsuccessful at Hide In Shadows.");
			}
		}
		else
			Client::sendMessage(%Client, $MsgWhite, "Only Rogues can Hide In Shadows.");
		return;
	}
	if(%w1 == "#bash")
	{
		if(!$blockBash[%Client]) {
			if(String::ICompare($CLASS[%Client], "Fighter") == 0 || String::ICompare($CLASS[%Client], "Paladin") == 0 || %Client.adminLevel >= 5)
			{
				Client::sendMessage(%Client, $MsgBeige, "You are ready to bash!");
				$NextHitBash[%Client] = True;
				$blockBash[%Client] = True;
			}
			else
				Client::sendMessage(%Client, $MsgWhite, "Only Fighters and Paladins can bash.");
		}
		return;
	}
	if(%w1 == "#shove")
	{
		%time = getIntegerTime(true) >> 5;
		if(%time - %Client.lastShoveTime > 1.5)
		{
			%Client.lastShoveTime = %time;

			%player = Client::getOwnedObject(%Client);
			if(GameBase::getLOSinfo(%player, 2))
			{
				%id = Player::getClient($los::object);

				if(%Client.adminLevel > %id.adminLevel || %id.adminLevel < 1)
				{
					%b = GameBase::getRotation(%Client);
					%c1 = Cap(20 + getFinalSTR(%Client), 0, 250); // 350
					%c2 = %c1 / 4;
					%mom = Vector::getFromRot( %b, %c1, %c2 );
					Player::applyImpulse(%id, %mom);
				}
			}
		}
		return;
	}
	if(%w1 == "#camp") {
		if(Client::HasItem(%Client, Tent)) {
			%camp = nameToId("MissionCleanup\\Camp"@%Client);
			if(%camp == -1) {
				if($zone[%Client] == "") {

					%passed = checkArea(%Client, 25);

					if(!%passed) {
						Client::sendMessage(%Client, $MsgBeige, "You are to close to an object. (wall or tree, etc)");
					}
					else {
						Client::sendMessage(%Client, $MsgBeige, "Setting up camp...");

						%pos = GameBase::getPosition(%Client);
						Client::addItemCount(%Client, Tent, -1);
						RefreshAll(%Client);
						%group = newObject("Camp"@%Client, SimGroup);
						addToSet("MissionCleanup", %group);
						schedule("DoCampSetup("@%Client@", 1, \""@%pos@"\");", 2, %group);
						schedule("DoCampSetup("@%Client@", 2, \""@%pos@"\");", 10, %group);
						schedule("DoCampSetup("@%Client@", 3, \""@%pos@"\");", 17, %group);
						schedule("DoCampSetup("@%Client@", 4, \""@%pos@"\");", 20, %group);
					}
				}
				else
					Client::sendMessage(%Client, $MsgRed, "You can't set up a camp here.");
			}
			else
				Client::sendMessage(%Client, $MsgRed, "You already have a camp setup somewhere.");
		}
		else
			Client::sendMessage(%Client, $MsgRed, "You aren't carrying a tent.");

		return;
	}
	if(%w1 == "#uncamp") {
		%camp = nameToId("MissionCleanup\\Camp"@%Client);
		if(%camp != -1) {
			%obj = nameToId("MissionCleanup\\Camp"@%Client@"\\woodfire");
			if(Vector::getDistance(GameBase::getPosition(%Client), GameBase::getPosition(%obj)) <= 10) {
				DoCampSetup(%Client, 5);
				Client::sendMessage(%Client, $MsgBeige, "Camp has been packed up.");
			}
			else
				Client::sendMessage(%Client, $MsgRed, "You are too far from your camp.");
		}
		else
			Client::sendMessage(%Client, $MsgRed, "You don't have a camp.");

		return;
	}
	if(%w1 == "#getstorage") {
		if(%Client.adminLevel >= 1) {
			if(%cropped == "")
				Client::sendMessage(%Client, 0, "Please specify player name.");
			else {
				%id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1) {
					Client::sendMessage(%Client, 0, %id@": "@$BankStorage[%id]);
                }
                else
					Client::sendMessage(%Client, 0, "Invalid player name.");
			}
		}
		else
			Client::sendMessage(%Client, 0, %Client@": "@$BankStorage[%Client]);
		return;
	}
	if(%w1 == "#clearstorage") {
		if(%Client.adminLevel >= 3) {
			if(%cropped == "")
				Client::sendMessage(%Client, 0, "Please specify player name.");
			else {
				%id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1) {
					$BankStorage[%id] = "";
					Client::sendMessage(%Client, 0, %id@" bank storage cleared.");
				}
				else
					Client::sendMessage(%Client, 0, "Invalid player name.");
			}
		}
		else
		{
			if(%cropped == "")
				Client::sendMessage(%Client, 0, "Please specify your name. THIS CANNOT BE UNDO.");
			else {
				%id = NEWgetClientByName(%cropped);

				if(%id == %Client) {
					$BankStorage[%id] = "";
					Client::sendMessage(%Client, 0, %id@" bank storage cleared. THIS CANNOT BE UNDO.");
				}
				else
					Client::sendMessage(%Client, 0, "You can only clear your storage.");
			}
		}
		return;
	}
	if(%w1 == "#admin") {
		if(%Client.adminLevel == -10)
			Client::sendMessage(%Client, 0, "Admins should know better than to be jailed.");
		else {
			%name = client::getname(%client);

				cage(%client,900);
				messageall(1,%name@" has been caged for 900 seconds!");
				echo(%name@" has been caged for 900 seconds for trying to #admin!");
				return;

			for(%i = 1; $adminpassword[%i] != ""; %i++) {
				if(%cropped == $AdminPassword[%i]) {

					%Client.adminLevel = %i;

					if(%Client.adminLevel >= 4)
						ChangeRace(%Client, "DeathKnight");

					Game::refreshClientScore(%Client);
      	            Client::sendMessage(%Client, 0, "Password accepted for Admin Clearance Level "@%Client.adminLevel@".");
					%name = Client::getName(%Client);

					$admin::var["[\""@%name@"\", 0, 1]"] = %cropped;
					$admin::var["[\""@%name@"\", 0, 666]"] = Client::getTransportAddress(%Client);
					File::delete("temp\\admin-"@%name@".cs");
					export("admin::var[\""@%name@"\",*", "temp\\Admin_"@%name@".cs", false);
					break;
				}
			}
		}
		return;
	}

	if(%Client.adminlevel > 0 || $PL[%Client] > 0) {
		ComChat_Admin(%Client, %message, %w1, %cropped);
	}

	//========== BOT TALK ======================================================================================

	if(%botTalk)
	{
		//process TownBot talk
		%clientPos = GameBase::getPosition(%Client);
		%closest = 5000000;
		%closestPos = "";

		for(%i = 0; (%id = GetWord($TownBotList, %i)) != -1; %i++)
		{
			%botPos = GameBase::getPosition(%id);
			%dist = Vector::getDistance(%clientPos, %botPos);

			if(%dist < %closest)
			{
				%closest = %dist;
				%closestId = %id;
				%closestPos = %botPos;
			}
		}
		%initTalk = "";
		%botNAME = $TownBot[%closestId, NAME];
		%botTYPE = $TownBot[%closestId, TYPE];

		if(String::ICompare(%cropped, %botNAME) == 0) { //possibly player punches in BOTs name....
			%initTalk = True;
		}
		else {
			if(String::findSubStr(" hail hello hi greetings yo hey sup salutations g'day howdy wazup ", " "@getWord(%cropped, 0)@" ") != -1)
				%initTalk = True;
		}
		if(%closest <= $maxAIdistVec && Client::getTeam(%Client) == GameBase::getTeam(%closestId) && %closestId != %Client) {
			if(%initTalk) {
				//Rotate Bot to look at player
				%rot = Vector::getRotation(Vector::normalize(Vector::sub(%clientPos, %closestPos)));
				%rot = "0 -0 "@GetWord(%rot, 2);

				GameBase::setRotation(%closestId, %rot);
				%r = floor(getRandom()*100); if(%r < 50) GameBase::playSequence(%closestId, 0, "root"); else if(%r < 80) GameBase::playSequence(%closestId, 0, "crouch root"); else GameBase::playSequence(%closestId, 0, "wave"); //"jet");
			}

			BotChatStuff(%Client, %closestId, %message, %cropped, %initTalk);
		}
		else
		{
			//This condition occurs when you are talking from too far of any TownBot.  All states are cleared here.
			//This means that potentially, you could initiate a conversation with the banker, travel for an hour
			//WITHOUT saying a word, come back and continue the conversation.  As soon as you speak in a way that
			//townbots hear you (#say, #shout, #tell) and are too far from them, all conversations are reset.

			deleteVariables("state"@%Client@"*");
		}
	}
}

exec(carling1);