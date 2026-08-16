
function CheckChatState(%Client, %closestId, %state) {
	if($state[%Client, %closestId] == %state) {
		Client::cancelMenu(%Client);
		AI::sayLater(%Client, $TownBot[%closestId, NAME], $TownBot[%closestId, SayBye], "NULL");
		$state[%Client, %closestId] = "";
		$ClientData[%Client, BotId] = "";
	}
	%Client.guiLock = "";
}

function BotChatStuff(%Client, %closestId, %message, %cropped, %initTalk) {

	%botNAME = $TownBot[%closestId, NAME];
	%botTYPE = $TownBot[%closestId, TYPE];

//echo("BotChatStuff("@%Client@", "@%closestId@", "@%message@", "@%cropped@", "@%initTalk@"); - "@%botNAME@" - "@%botTYPE@" - state("@$state[%Client, %closestId]@")");

	$ClientData[%Client, BotTalking] = true;
	$ClientData[%Client, BotId] = "";

	Client::cancelMenu(%Client);
	remotePlayMode(%Client);

	%Client.guiLock = true;

	if(%botTYPE == "merchant") {
		//process merchant code
		%trigger[2] = "buy";
		%trigger[3] = "yes";
		%trigger[4] = "no";
		remoteEval(%Client, "SetUpKeys", "b buy n no y yes");

		if($state[%Client, %closestId] == "") {
			if(%initTalk) {
				%p1 = "Did you come to see what items you can buy?";
				%p2 = "\n\n  <f1>B<f0>uy.\n  <f1>N<f0>o. Changed my mind.";
				AI::sayLater(%Client, %closestId, %p1, %p2);
				$state[%Client, %closestId] = 1;
				schedule("CheckChatState("@%Client@", "@%closestId@", 1);", 5);
			}
			$ClientData[%Client, BotId] = %closestId;
		}
		else if($state[%Client, %closestId] == 1) {
			if(String::findSubStr(%message, %trigger[2]) != -1) {

				schedule("SetupShop("@%Client@", "@%closestId@");", 2.1);

				AI::sayLater(%Client, %closestId, "Take a look at what I have.", "NULL");//use NULL to clear out of this msg madness
				$state[%Client, %closestId] = "";
			}
			else if(String::findSubStr(%message, %trigger[4]) != -1) {
				AI::sayLater(%Client, %closestId, $TownBot[%closestId, SayBye], "NULL");
				$state[%Client, %closestId] = "";
			}
		}
		else if($state[%closestId, %Client] == 2) {
			if(String::findSubStr(%message, %trigger[3]) != -1) {
				%w = floor( getWord($Quiver[%Client], 0) );
				$Quiver[%Client] = String::replace($Quiver[%Client], %w, %w+1);
				$Quiver[%Client] = $Quiver[%Client]@" FreeSlot";
				AI::sayLater(%Client, %closestId, "Here you go. Have a nice day!", "NULL");
				$state[%Client, %closestId] = "";
			}
		}
		%Client.guiLock = "";
	}
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	else if(%botTYPE == "banker") {
		//process banker code
		%trigger[2] = "deposit";
		%trigger[3] = "withdraw";
		%trigger[4] = "storage";
		remoteEval(%Client, "SetUpKeys", "d deposit w withdraw s storage");
		if($state[%Client, %closestId] == "") {
			if(%initTalk) {
				%p1 = "<f0>I can keep your money from being stolen by thieves. You are carrying <f1>"@$COINS[%Client]@"<f0> gil and I have <f1>"@$BANK[%Client]@"<f0> of yours.";
				%p2 = "\n\n  <f1>D<f0>eposit <f1>W<f0>ithdraw <f1>S<f0>torage";
				AI::sayLater(%Client, %closestId, %p1, %p2);
				$state[%Client, %closestId] = 1;
				schedule("CheckChatState("@%Client@", "@%closestId@", 1);", $AIwait[banker]);
			}
			$ClientData[%Client, BotId] = %closestId;
		}
		if($state[%Client, %closestId] == 1) {
			if(String::findSubStr(%message, %trigger[2]) != -1) {
				//deposit question
				AI::sayLater(%Client, %closestId, "How much to you want me to hold? You are carrying <f1>"@$COINS[%Client]@"<f0> gil and I have <f1>"@$BANK[%Client]@"<f0> of yours.\n(<f1>AMOUNT<f0>/<f1>ALL<f0>)", "NULL");
				$state[%Client, %closestId] = 2;
			}
			if(String::findSubStr(%message, %trigger[3]) != -1) {
				//withdraw question
				AI::sayLater(%Client, %closestId, "How much do you want to take out? You are carrying <f1>"@$COINS[%Client]@"<f0> gil and I have <f1>"@$BANK[%Client]@"<f0> of yours.\n(<f1>AMOUNT<f0>/<f1>ALL<f0>)", "NULL");
				$state[%Client, %closestId] = 3;
			}
			if(String::findSubStr(%message, %trigger[4]) != -1) {
				//storage
				AI::sayLater(%Client, %closestId, "This is the equipment you have stored here.", "NULL");
				schedule("SetupBank("@%Client@", "@%closestId@");", 2.1);
				$state[%Client, %closestId] = "";
			}
			%Client.guiLock = "";
		}
		else if($state[%Client, %closestId] == 2) {
			//deposit
			if(%cropped == "all")
				%cropped = $COINS[%Client];
			%c = floor(%cropped);
			if(%c <= 0)
				AI::sayLater(%Client, %closestId, "Invalid request.  Your transaction has been cancelled.", "NULL");
			else if(%c <= $COINS[%Client]) {
				$BANK[%Client] += %c;
				$COINS[%Client] -= %c;
				AI::sayLater(%Client, %closestId, "You have given me <f1>"@%c@"<f0> gil. You are now carrying <f1>"@$COINS[%Client]@"<f0> gil and I have <f1>"@$BANK[%Client]@"<f0> of yours. Have a nice day.", "NULL");
				playSound(SoundMoney1, GameBase::getPosition(%closestId));
			}
			else
				AI::sayLater(%Client, %closestId, "I'm sorry, you don't have that much gil.  Your transaction has been cancelled.", "NULL");
			$state[%Client, %closestId] = "";
			%Client.guiLock = "";
		}
		else if($state[%Client, %closestId] == 3) {
			//withdraw
			if(%cropped == "all")
				%cropped = $BANK[%Client];
			%c = floor(%cropped);
			if(%c <= 0)
				AI::sayLater(%Client, %closestId, "Invalid request.  Your transaction has been cancelled.", "NULL");
			else if(%c+$COINS[%Client] > $MaxCOINS[%Client]) {
				AI::sayLater(%Client, %closestId, "You cannot carry over "@$MaxCOINS[%Client]@" gil.", "NULL");
			}
			else if(%c <= $BANK[%Client]) {
				$COINS[%Client] += %c;
				$BANK[%Client] -= %c;
				AI::sayLater(%Client, %closestId, "I have given you <f1>"@%c@"<f0> gil. You are now carrying <f1>"@$COINS[%Client]@"<f0> gil and I have <f1>"@$BANK[%Client]@"<f0> of yours. Have a nice day.", "NULL");
				playSound(SoundMoney1, GameBase::getPosition(%Client));
			}
			else {
				AI::sayLater(%Client, %closestId, "I'm sorry but you don't have that much gil in my bank.  Your transaction has been cancelled.", "NULL");
			}
			$state[%Client, %closestId] = "";
		}
	}
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	else if(%botTYPE == "assassin") {
		//process assassin code
		%trigger[2] = "yes";
		%trigger[3] = "no";
		%trigger[4] = "buy";
		remoteEval(%Client, "SetUpKeys", "y yes n no b buy");
		if($state[%Client, %closestId] == "") {
			if(%initTalk) {
				if(GetWord($bounty[%Client], 1) == "!Q@W#E$R%T^Y&U*I(O)P") {
					%reward = floor($coinsrewardperlevel * (GetWord($bounty[%Client], 0) / getFinalLVL(%Client)));
					AI::sayLater(%Client, %closestId, "Hey thanks a lot, here's <f1>"@FixM(%reward)@"<f0> gil for your troubles. Come back anytime, I hate a lot of people and I have a lot of coins.", True);

					GiveThisStuff(%Client, "COINS "@%reward, True);
					playSound(SoundMoney1, GameBase::getPosition(%Client));
					$bounty[%Client] = "";
					$state[%Client, %closestId] = "";
					%Client.guiLock = "";
				}
				else {
					%i = 0;
					%n = floor(getRandom() * GetNumClients());
					for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) {
						if(%i == %n && %cl != %Client)
							break;
						%i++;
					}
					if(%cl == %Client || %cl == -1) {
						//he's the only player in the game
						%p1 = "\n\n  <f1>B<f0>uy something...\n  <f1>N<f0>evermind.";
						AI::sayLater(%Client, %closestId, "Go away unless you want to buy something from me.", %p1);
						$state[%Client, %closestId] = 1;
						schedule("CheckChatState("@%Client@", "@%closestId@", 1);", $AIwait[assassin]);
						$ClientData[%Client, BotId] = %closestId;
					}
					else {

						%reward = floor($coinsrewardperlevel * (getFinalLVL(%cl) / getFinalLVL(%Client)));

						$bounty[%Client] = Client::getName(%cl);
						%p1 = "\n\n  <f1>Y<f0>es!\n  <f1>N<f0>o way!";
						AI::sayLater(%Client, %closestId, "So you want to help me, eh?  Alright, I want you to kill <f1>"@FixM($bounty[%Client])@"<f0> for <f1>"@FixM(%reward)@"<f0> gil. Up for it? I also have something else you might want to buy.", %p1);
						$state[%Client, %closestId] = 1;
						$ClientData[%Client, BotId] = %closestId;
						schedule("if($state["@%closestId@", "@%Client@"] == \"1\"){AI::sayLater("@%Client@", "@%closestId@", \"You don't want to answer?  Fine, I'll kill him myself.\", NULL);$bounty["@%Client@"] = \"\";$state["@%closestId@", "@%Client@"] = \"\";}", $AIwait[assassin]);
					}
				}
			}
		}
		else if($state[%Client, %closestId] == 1) {
			if(String::findSubStr(%message, %trigger[2]) != -1 && $bounty[%Client] != "") {
				AI::sayLater(%Client, %closestId, "Ok good luck. Remember to come and see me once he's dead. I'll give you the gil then.", "NULL");
				$state[%Client, %closestId] = "";
				%Client.guiLock = "";
			}
			if(String::findSubStr(%message, %trigger[3]) != -1 && $bounty[%Client] != "") {
				if(String::ICompare(Client::getGender($bounty[%Client]), "Male") == 0)
					%gender = "him";
				else if(String::ICompare(Client::getGender($bounty[%Client]), "Female") == 0)
					%gender = "her";
				AI::sayLater(%Client, %closestId, "You go to hell you son of a bitch! I'll kill "@%gender@" myself!", "NULL");
				$bounty[%Client] = "";
				$state[%Client, %closestId] = "";
				%Client.guiLock = "";
			}
			if(String::findSubStr(%message, %trigger[4]) != -1) {
				%cost = ( pow(2, getFinalLCK(%Client)) * 5 );
				%p1 = "\n\n  <f1>Y<f0>es, good deal.\n  <f1>N<f0>o way!";
				AI::sayLater(%Client, %closestId, "I will sell you one LCK point for $<f1>"@%cost@"<f0>.", %p1);
				$bounty[%Client] = "";
				$state[%Client, %closestId] = 2;
				$ClientData[%Client, BotId] = %closestId;
			}
		}
		else if($state[%Client, %closestId] == 2) {
			if(String::findSubStr(%message, %trigger[2]) != -1) {
				%cost = ( pow(2, getFinalLCK(%Client)) * 5 );
				if($COINS[%Client] >= %cost) {
					AI::sayLater(%Client, %closestId, "Here's your LCK point, thanks for your business.", "NULL");
					GiveThisStuff(%Client, "LCK 1");
					$COINS[%Client] -= %cost;
				}
				else
					AI::sayLater(%Client, %closestId, "You can't afford this.", "NULL");
				$state[%Client, %closestId] = "";
			}
			if(String::findSubStr(%message, %trigger[3]) != -1) {
				AI::sayLater(%Client, %closestId, "Good bye.", "NULL");
				$state[%Client, %closestId] = "";
			}
			%Client.guiLock = "";
		}
	}
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	else if(%botTYPE == "quester") {
		//process quest code
		%trigger[2] = "yes";
		%trigger[3] = "yes"; //HasThisStuff

		if($TownBot[%closestId, SHOP] != "") {
			%buy = " b buy";
			%trigger[4] = "buy";
		}
		else
			%trigger[4] ="NULL";

		%trigger[5] = "no";
		%trigger[6] = "no"; //HasThisStuff

		%list = $TownBot[%closestId, CUEKEY, 1];
		%key1 = getWord(%list, 0)@" yes ";
		%key2 = getWord(%list, 1)@" no";
		remoteEval(%Client, "SetUpKeys", %key1@%key2@%buy);
		if(%initTalk || $state[%Client, %closestId] != "") {
			if($TownBot[%closestId, NQUESTISON, 1] != "") {
				if($ClientData[%Client, $TownBot[%closestId, NQUESTISON, 1]] == "started")
					%NQUEST = True;
				else
					%NQUEST = False;
			}
			else
				%NQUEST = True;

			%hasTheStuff = HasThisStuff(%Client, $TownBot[%closestId, NEED]);
			if($TownBot[%closestId, CSAY] == "" && %hasTheStuff == 666)
				%hasTheStuff = False;
			if($TownBot[%closestId, LSAY] == "" && %hasTheStuff == 667)
				%hasTheStuff = False;

			if(%hasTheStuff) {//HasStuff
				if(!%NQUEST)//But NQUEST is off
					%hasTheStuff = False;
			}
	//echo("hasTheStuff"@%hasTheStuff@" NQUEST"@%NQUEST);
			if(%hasTheStuff == 666 && $state[%Client, %closestId] == "") {
				if(%initTalk) {
					AI::sayLater(%Client, %closestId, $TownBot[%closestId, CSAY], "NULL");
					$state[%Client, %closestId] = -5;
					schedule("CheckChatState("@%Client@", "@%closestId@", \"-5\");", $AIwait[quest]);
				}
			}
			else if(%hasTheStuff == 667 && $state[%Client, %closestId] == "") {
				if(%initTalk) {
					AI::sayLater(%Client, %closestId, $TownBot[%closestId, LSAY], "NULL");
					$state[%Client, %closestId] = -5;
					schedule("CheckChatState("@%Client@", "@%closestId@", \"-5\");", $AIwait[quest]);
				}
			}
			else if(!%hasTheStuff) { //|| (%hasTheStuff && !%NQUEST)) {
	//echo(" !hasTheStuff");
				if($state[%Client, %closestId] == "") {
					if(%initTalk) {
						AI::sayLater(%Client, %closestId, $TownBot[%closestId, SAY, 1], $TownBot[%closestId, CUE, 1]);
						$state[%Client, %closestId] = 1;
						schedule("CheckChatState("@%Client@", "@%closestId@", 1);", $AIwait[quest]);
						$ClientData[%Client, BotId] = %closestId;
					}
				}
				else if($state[%Client, %closestId] == 1) {
					if(String::findSubStr(%message, %trigger[2]) != -1) {
	//echo("say 2");
						AI::sayLater(%Client, %closestId, $TownBot[%closestId, SAY, 2], "NULL");
						$state[%Client, %closestId] = "";
						%Client.guiLock = "";
					}
					else if(String::findSubStr(%message, %trigger[5]) != -1) {
	//echo("SayBye");
						AI::sayLater(%Client, %closestId, $TownBot[%closestId, SayBye], "NULL");
						$state[%Client, %closestId] = "";
						%Client.guiLock = "";
					}
				}
			}
			else if(%hasTheStuff) {
	//echo(" hasTheStuff");
				%list = $TownBot[%closestId, NCUEKEY, 1];
				%key1 = getWord(%list, 0)@" yes ";
				%key2 = getWord(%list, 1)@" no";
				remoteEval(%Client, "SetUpKeys", %key1@%key2@%buy);    //	remoteEval(%Client, "SetUpKeys", $TownBot[%closestId, NCUEKEY, 1]@%buy);
				if($state[%Client, %closestId] == "") {
					if(%initTalk) {
						AI::sayLater(%Client, %closestId, $TownBot[%closestId, NSAY, 1], $TownBot[%closestId, NCUE, 1]);
						$state[%Client, %closestId] = 1;
						schedule("CheckChatState("@%Client@", "@%closestId@", 1);", $AIwait[quest]);
						$ClientData[%Client, BotId] = %closestId;
					}
				}
				else if($state[%Client, %closestId] == 1) {
					if(String::findSubStr(%message, %trigger[3]) != -1) {
						if(TakeThisStuff(%Client, $TownBot[%closestId, NEED])) {
							GiveThisStuff(%Client, $TownBot[%closestId, GIVE]);
							AI::sayLater(%Client, %closestId, $TownBot[%closestId, NSAY, 2], "NULL");
							if($TownBot[%closestId, NQUEST, 1] != "")
								$QuestEvalStr[%Client] = "NewQuest("@%Client@", $TownBot["@%closestId@", NQUEST, 1]);";
							if($TownBot[%closestId, NQUESTDONE, 1] != "")
								$QuestEvalStr[%Client] = "EndQuest("@%Client@", $TownBot["@%closestId@", NQUESTDONE, 1]);";
						}
						else
							AI::sayLater(%Client, %closestId, "Nice try, I'm keeping what I managed to get from you.", "NULL");
						Game::refreshClientScore(%Client);
					}
					else if(String::findSubStr(%message, %trigger[6]) != -1)
						AI::sayLater(%Client, %closestId, $TownBot[%closestId, NSAYNO, 1], "NULL");

					$state[%Client, %closestId] = "";
					%Client.guiLock = "";
				}
			}
			%t4 = String::findSubStr(%message, %trigger[4]);
			if(%t4 != -1 || ($state[%Client, %closestId] == -5 && %t4 != -1)) {
				if($TownBot[%closestId, SHOP] != "") {
					schedule("SetupShop("@%Client@", "@%closestId@");", 2);
					AI::sayLater(%Client, %closestId, "Take a look at what I have.", "NULL");
				}
				else
					AI::sayLater(%Client, %closestId, "I have nothing to sell.", "NULL");
				$state[%Client, %closestId] = "";
				%Client.guiLock = "";
			}
		}
	}
	else if(%botTYPE == "extended") {//quester++
		for(%i=1;$TownBot[%closestId,NEED,%i]!="";%i++) {
			if(HasThisStuff(%Client,$TownBot[%closestId,NEED,%i])==false)
					break;
		}
		if($TownBot[%closestId,NEED,%i]=="")
			%i=1;

		if(HasThisStuff(%Client,$TownBot[%closestId,ALWAYSGIVE,%i]) == false)//player dont have stuff -- needed to complete quest...
			GiveThisStuff(%Client, $TownBot[%closestId,ALWAYSGIVE,%i]);
		//process quest code
		%trigger[2] = "yes"; //$TownBot[%closestId, CUE, %i, 1];
		%trigger[3] = "no"; //$TownBot[%closestId, NCUE, %i, 1];
	//	%trigger[4] = "buy";

		if($TownBot[%closestId, SHOP] != "") {
			%buy = " b buy";
			%trigger[4] = "buy";
		}
		else
			%trigger[4] ="NULL";

		%list = $TownBot[%closestId, CUEKEY, %i, 1];
		%key1 = getWord(%list, 0)@" yes ";
		%key2 = getWord(%list, 1)@" no";
		remoteEval(%Client, "SetUpKeys", %key1@%key2@%buy);
	//	remoteEval(%Client, "SetUpKeys", $TownBot[%closestId, CUEKEY, %i, 1]@" "@$TownBot[%closestId, NCUEKEY, %i, 1]@" b buy");
		if(%initTalk || $state[%Client, %closestId] != "") {
			%hasTheStuff = HasThisStuff(%Client, $TownBot[%closestId, NEED, %i]);
			if($TownBot[%closestId, CSAY, %i] == "" && %hasTheStuff == 666)
				%hasTheStuff = False;
			if($TownBot[%closestId, LSAY, %i] == "" && %hasTheStuff == 667)
				%hasTheStuff = False;
			if(%hasTheStuff == 666 && $state[%Client, %closestId] == "") {
				if(%initTalk) {
					AI::sayLater(%Client, %closestId, $TownBot[%closestId, CSAY, %i], "NULL");
					$state[%Client, %closestId] = -5;
					schedule("CheckChatState("@%Client@", "@%closestId@", \"-5\");", $AIwait[quest]);
					%Client.guiLock = "";
				}
			}
			else if(%hasTheStuff == 667 && $state[%Client, %closestId] == "") {
				if(%initTalk) {
					AI::sayLater(%Client, %closestId, $TownBot[%closestId, LSAY, %i], "NULL");
					$state[%Client, %closestId] = -5;
					schedule("CheckChatState("@%Client@", "@%closestId@", \"-5\");", $AIwait[quest]);
					%Client.guiLock = "";
				}
			}
			else if(HasThisStuff(%Client,$TownBot[%closestId,NEEDTOEND,%i])==false) {
				if($state[%Client, %closestId] == "") {
					if(%initTalk) {
						AI::sayLater(%Client, %closestId, $TownBot[%closestId, SAY, %i, 1], %trigger[2]);
						$state[%Client, %closestId] = 1;
						schedule("CheckChatState("@%Client@", "@%closestId@", 1);", $AIwait[quest]);
						$ClientData[%Client, BotId] = %closestId;
					}
				}
				else if($state[%Client, %closestId] == 1) {
					if(String::findSubStr(%message, %trigger[2]) != -1) {
						if($TownBot[%closestId,SPAWN,%i]!=""&&HasThisStuff(%Client,$TownBot[%closestId,NEEDTOEND,%i])==false) {
							%to_run=String::replace($TownBot[%closestId,SPAWN,%i],"clientId",%Client);
							%to_run=String::replace(%to_run,"playerId",Client::getOwnedObject(%Client));
							%to_run=String::replace(%to_run,"townbot",%closestId);
							schedule(%to_run,1);
						}
						AI::sayLater(%Client, %closestId, $TownBot[%closestId, SAY, %i, 2], "NULL");
						$state[%Client, %closestId] = "";
						%Client.guiLock = "";
					}
				}
			}
			else if(HasThisStuff(%Client,$TownBot[%closestId,NEEDTOEND,%i])==true) {
				if($state[%Client, %closestId] == "") {
					if(%initTalk) {
						AI::sayLater(%Client, %closestId, $TownBot[%closestId, NSAY, %i, 1], %trigger[3]);
						$state[%Client, %closestId] = 1;
						schedule("CheckChatState("@%Client@", "@%closestId@", 1);", $AIwait[quest]);
						$ClientData[%Client, BotId] = %closestId;
					}
				}
				else if($state[%Client, %closestId] == 1) {
					if(String::findSubStr(%message, %trigger[3]) != -1) {
						if(TakeThisStuff(%Client, $TownBot[%closestId, NEEDTOEND, %i])) {
							GiveThisStuff(%Client, $TownBot[%closestId, GIVE, %i]);
							AI::sayLater(%Client, %closestId, $TownBot[%closestId, NSAY, %i, 2], "NULL");
						}
						else
							AI::sayLater(%Client, %closestId, "Nice try, I'm keeping what I managed to get from you.", "NULL");
						$state[%Client, %closestId] = "";
						Game::refreshClientScore(%Client);
					}
					%Client.guiLock = "";
				}
			}
			%t4 = String::findSubStr(%message, %trigger[4]);
			if(%t4 != -1 || ($state[%Client, %closestId] == -5 && %t4 != -1)) {
				if($TownBot[%closestId, SHOP, %i] != "") {
					schedule("SetupShop("@%Client@", "@%closestId@", "@%i@");", 2.1);
					AI::sayLater(%Client, %closestId, "Take a look at what I have.", "NULL");
				}
				else
					AI::sayLater(%Client, %closestId, "I have nothing to sell.", "NULL");
				$state[%Client, %closestId] = "";
				%Client.guiLock = "";
			}
		}
	}
	if(%botTYPE == "blacksmith") {

		%trigger[2] = "buy";
		%trigger[3] = "smith";
		%trigger[4] = "no";
		remoteEval(%Client, "SetUpKeys", "b buy s smith n no");

		if($state[%Client, %closestId] == "")
		{
			if(%initTalk)
			{
				%p1 = "\n\n  <f1>S<f0>mith.\n  <f1>N<f0>o.";
				AI::sayLater(%Client, %closestId, "Hail friend, are you here to have me smith an old item?", %p1);
				$state[%Client, %closestId] = 1;
				$ClientData[%Client, BotId] = %closestId;
			}
		}
		else if($state[%Client, %closestId] == 1)
		{
			//echo("$state == 1 "@%message@" ");
			if(String::findSubStr(%message, %trigger[2]) != -1)
			{
				if($BotInfo[%aiName, SHOP] != "")
				{
					schedule("SetupShop("@%Client@", "@%closestId@");", 2.1);
					AI::sayLater(%Client, %closestId, "Take a look at what I have.", "NULL");
				}
				else
					AI::sayLater(%Client, %closestId, "I have nothing to sell.", "NULL");

				$state[%Client, %closestId] = "";
			}
			if(String::findSubStr(%message, %trigger[3]) != -1)
			{
			//	AI::sayLater(%Client, %closestId, "Click Use on an item and I will tell you how much it\nwill cost to smith. Click Use on this item again and I\nwill get to work.", "NULL");
				AI::sayLater(%Client, %closestId, "Lets do this.", "NULL");
				Client::sendMessage(%Client, 0, "Click Sell on an item and I will keep a list on what to smith. Click Buy to take back an item.");
				$ClientData[%Client, tmpSmith] = "";
				$ClientData[%Client, SmithStage] = "stuff";
				$ClientData[%Client, SmithMode] = 1;
				SetupBlacksmith(%Client, %closestId);

				$state[%Client, %closestId] = "";
			}
		}
	}
	else if(%botTYPE == "militia") {
		//process guard code
		if($state[%Client, %closestId] == "") {
			if(%initTalk) {
				AI::sayLater(%Client, %closestId, "Ugh! Me tough!", "NULL");
				$state[%Client, %closestId] = "";
				%Client.guiLock = "";
			}
		}
	}
}


function remoteIssueCommand(%commander, %cmdIcon, %command, %wayX, %wayY, %dest1, %dest2, %dest3, %dest4, %dest5, %dest6, %dest7, %dest8, %dest9, %dest10, %dest11, %dest12, %dest13, %dest14)
{
	if($dedicated)
		echo("COMMANDISSUE: "@%commander@" \""@escapeString(%command)@"\"");
	// issueCommandI takes waypoint 0-1023 in x,y scaled mission area
	// issueCommand takes float mission coords.
	for(%i = 1; %dest[%i] != ""; %i = %i + 1)
		if(!%dest[%i].muted[%commander])
			issueCommandI(%commander, %dest[%i], %cmdIcon, %command, %wayX, %wayY);
}

function remoteIssueTargCommand(%commander, %cmdIcon, %command, %targIdx, %dest1, %dest2, %dest3, %dest4, %dest5, %dest6, %dest7, %dest8, %dest9, %dest10, %dest11, %dest12, %dest13, %dest14)
{
	if($dedicated)
		echo("COMMANDISSUE: "@%commander@" \""@escapeString(%command)@"\"");
	for(%i = 1; %dest[%i] != ""; %i = %i + 1)
		if(!%dest[%i].muted[%commander])
			issueTargCommand(%commander, %dest[%i], %cmdIcon, %command, %targIdx);
}

function remoteCStatus(%Client, %status, %message)
{
	// setCommandStatus returns false if no status was changed.
	// in this case these should just be team says.
	if(setCommandStatus(%Client, %status, %message))
	{
		if($dedicated)
			echo("COMMANDSTATUS: "@%Client@" \""@escapeString(%message)@"\"");
	}
	else
		remoteSay(%Client, true, %message);
}

function teamMessages(%mtype, %team1, %message1, %team2, %message2, %message3)
{
	%numPlayers = getNumClients();
	for(%i = 0; %i < %numPlayers; %i = %i + 1)
	{
		%id = getClientByIndex(%i);
		if(Client::getTeam(%id) == %team1)
		{
			Client::sendMessage(%id, %mtype, %message1);
		}
		else if(%message2 != "" && Client::getTeam(%id) == %team2)
		{
			Client::sendMessage(%id, %mtype, %message2);
		}
		else if(%message3 != "")
		{
			Client::sendMessage(%id, %mtype, %message3);
		}
	}
}

function messageAll(%mtype, %message, %filter)
{
	dbecho($dbechoMode, "messageAll("@%mtype@", "@%message@", "@%filter@")");

	if(%filter == "")
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			Client::sendMessage(%cl, %mtype, %message);
	else
	{
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			if(%cl.messageFilter & %filter)
			Client::sendMessage(%cl, %mtype, %message);
		}
	}
}

function messageAllExcept(%except, %mtype, %message)
{
	dbecho($dbechoMode, "messageAllExcept("@%except@", "@%mtype@", "@%message@")");

	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl != %except)
			Client::sendMessage(%cl, %mtype, %message);
	}
}

function radiusAllExcept(%except1, %except2, %message)
{
	dbecho($dbechoMode, "radiusAllExcept("@%except1@", "@%except2@", "@%message@")");

	%epos1 = GameBase::getPosition(%except1);
	%epos2 = GameBase::getPosition(%except2);
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		%clpos = GameBase::getPosition(%cl);
		%dist1 = Vector::getDistance(%clpos, %epos1);
		%dist2 = Vector::getDistance(%clpos, %epos2);
		if(%cl != %except1 && %cl != %except2)
		{
			if(%dist1 <= $maxSAYdistVec || %dist2 <= $maxSAYdistVec)
				Client::sendMessage(%cl, $MsgBeige, %message);
		}
	}
}
function getRandomName(%Client, %ii) {
	%w = Fun::getName(%Client);
	if(%ii > 5)
		return "Goblinrunt0";
	%name = Client::getName(%Client);
	if(%w != "" && %w != %name)
		return %w;

	%list = GetEveryoneNameList();
	for(%i = 0; (%w[%i] = getWord(%list, %i)) != -1; %i++){}
	%r = Cap(floor(getRandom() * %i), 0, %i);
	if(%w[%r] != %name)
		return %w[%r];
	else
		return getRandomName(%Client, %ii+1);

}
function Fun::getName(%Client) {
	%closest = 500000;
	%Pos = GameBase::getPosition(%Client);
	%b = 250;
	%set = newObject("set", SimSet);
	%n = containerBoxFillSet(%set, $SimPlayerObjectType, %Pos, %b, %b, %b, 0);
	for(%i = 0; %i < Group::objectCount(%set); %i++) {
		%id = Player::getClient(Group::getObject(%set, %i));
		%dist = Vector::getDistance(%Pos, GameBase::getPosition(%id));
		if(%dist < %closest) {
			%closest = %dist;
			%closestId = %id;
		}
	}
	deleteObject(%set);
	if(%closestId != "")
		return Client::getName(%closestId);
	else
		return "";
}

//echo("           --'"@DrunkenBastard(0, "The white spotted dog crossed the street.")@"'");
function DrunkenBastard(%Client, %msg) {

	%a = $ClientData[%Client, Alvl];

	%sflag = False;
	for(%i = 0; (%w = getWord(%msg, %i)) != -1; %i++) {

		if(floor(getRandom()*30) < %a) {//Thanks to FENIX for some of these fun lines =]
			if(%w == 'i' || %w == 'I') { if(floor(getRandom()*50) < %a && !%sflag) { %w = "I.. I.. LOVE YOU "@getRandomName(%Client)@"!! and I"; %sflag = True; } }
			else if(%w == 'My' || %w == 'my') { if(floor(getRandom()*50) < %a && !%sflag) { %w = "My my your looking mighty fine "@getRandomName(%Client)@". My"; %sflag = True; } }
			else if(%w == 'Hey' || %w == 'hey') { if(floor(getRandom()*50) < %a && !%sflag) { %w = "Hey you sexay thang,"; %sflag = True;} }
			else if(%w == 'Hi' || %w == 'hi') { if(floor(getRandom()*50) < %a && !%sflag) { %w = "Hi, my name is "@Client::getName(%Client)@", how do you like me so far?"; %sflag = True;} }
			else if(%w == 'Do' || %w == 'do') { if(floor(getRandom()*50) < %a && !%sflag) { %w = "Doo you love me ass mmuch as I llooove you "@getRandomName(%Client)@"!? Do"; %sflag = True;} }
			else if(%w == 'Can' || %w == 'can') { if(floor(getRandom()*50) < %a && !%sflag) { %w = "Can I get yourr numberr "@getRandomName(%Client)@"?? and can"; %sflag = True;} }
			else if(%w == 'You' || %w == 'you') { if(floor(getRandom()*50) < %a && !%sflag) { %w = "Yoo turrn me oon "@getRandomName(%Client)@"! and you"; %sflag = True;} }
			else if(%w == 'No' || %w == 'no') { if(floor(getRandom()*50) < %a) { %w = "Yes"; } }
			else if(%w == 'Yes' || %w == 'yes') { if(floor(getRandom()*50) < %a) { %w = "No"; } }
			else if(%w == 'don\'t' || %w == 'dont') { if(floor(getRandom()*50) < %a) { %w = "dooo"; } }

			if(%w == s) %w = String::replace(%w, "s", "ss");
			if(%w == r) %w = String::replace(%w, "r", "rr");
			if(%w == a) %w = String::replace(%w, "a", "aa");
			if(%w == u) %w = String::replace(%w, "u", "ooo");
		}

		if(floor(getRandom()*150) < %a) {
			%r = floor(getRandom()*100);
			if(%r <= 65 && %r >= 45)
				%add = " *hic*";
			else if(%r >= 90)
				%add = " durrr";
			else if(%r <= 10)
				%add = " uuh";
			else if(%r >= 11 && %r <= 20)
				%add = " *fart*";
		}
		%Nmsg = %Nmsg@%w@%add@" ";

		if(%i > 100) {
			echo("Break");
			break;
		}
	}

	%Nmsg = String::NEWgetSubStr(%Nmsg, 0, String::len(%Nmsg)-1);

	return %Nmsg;
}

function move_to_position(%object,%Client)
{		// move_to_position() by Adger
		//Edited a bit by Deus_ex_Machina
		//Added $NEWtpos
	if(%Client.cmd==%object&&IsObject(%object))
	{
		%pos=GameBase::getposition(Client::getObserverCamera(%Client));
		%pos=GetWord(%pos,0)-$NEWXtpos[%Client]@" "@GetWord(%pos,1)-$NEWYtpos[%Client]@" "@GetWord(%pos,2)-$NEWZtpos[%Client]; //"@GetWord(%pos,2)-20;
		GameBase::setposition(%object,%pos);
		GameBase::setrotation(%object,GameBase::getrotation(Client::getObserverCamera(%Client)));
		schedule("move_to_position("@%object@","@%Client@");",1/10,%Client);
	}
}
