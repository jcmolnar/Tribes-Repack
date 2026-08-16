// Thanks to Ari, Sinister, and whoever else who made/contributed to the linebreak, remoteeval and #cast hacking prevention.
// See: http://wastelandrpg.14.forumer.com/viewtopic.php?t=639
// I've only slightly altered it (Visually pleasing spacing, a variable to define the exploit log, and altered the warning text. Just minor things like that.

$MsgTypeSystem = 0;
$MsgTypeGame = 1;
$MsgTypeChat = 2;
$MsgTypeTeamChat = 3;
$MsgTypeCommand = 4;

$MsgWhite = 0;
$MsgRed = 1;
$MsgBeige = 2;
$MsgGreen = 3;

function remoteSay(%clientId, %team, %message, %senderName)
{
	dbecho($dbechoMode, "remoteSay(" @ %clientId @ ", " @ %team @ ", \"" @ %message @ "\", " @ %senderName @ ")");

	if(!Player::isAiControlled(%clientId))
		echo(client::getname(%clientId) @ ": " @ %message);

	if(%clientId.IsInvalid || %message == "" || %message == " " || (String::ICompare(%senderName, "Server") == 0 && %clientid != 2048))
		return;

	if (string::findsubstr(%message, "\x01") != -1) { %clientId.linebreaking = 01; }
	if (string::findsubstr(%message, "\x02") != -1) { %clientId.linebreaking = 02; }
	if (string::findsubstr(%message, "\x03") != -1) { %clientId.linebreaking = 03; }
	if (string::findsubstr(%message, "\x04") != -1) { %clientId.linebreaking = 04; }
	if (string::findsubstr(%message, "\x05") != -1) { %clientId.linebreaking = 05; }
	if (string::findsubstr(%message, "\x06") != -1) { %clientId.linebreaking = 06; }
	if (string::findsubstr(%message, "\x07") != -1) { %clientId.linebreaking = 07; }
	if (string::findsubstr(%message, "\x08") != -1) { %clientId.linebreaking = 08; }
	if (string::findsubstr(%message, "\x09") != -1) { %clientId.linebreaking = 09; }
	if (string::findsubstr(%message, "\x10") != -1) { %clientId.linebreaking = 10; }
	if (string::findsubstr(%message, "\x11") != -1) { %clientId.linebreaking = 11; }
	if (string::findsubstr(%message, "\x12") != -1) { %clientId.linebreaking = 12; }
	if (string::findsubstr(%message, "\x13") != -1) { %clientId.linebreaking = 13; }
	if (string::findsubstr(%message, "\x14") != -1) { %clientId.linebreaking = 14; }
	if (string::findsubstr(%message, "\x15") != -1) { %clientId.linebreaking = 15; }
	if (string::findsubstr(%message, "\x16") != -1) { %clientId.linebreaking = 16; }
	if (string::findsubstr(%message, "\x17") != -1) { %clientId.linebreaking = 17; }
	if (string::findsubstr(%message, "\x18") != -1) { %clientId.linebreaking = 18; }
	if (string::findsubstr(%message, "\x19") != -1) { %clientId.linebreaking = 19; }
	if (string::findsubstr(%message, "\n") != -1) { %clientId.linebreaking = "n"; }

	if (%clientId.linebreaking != "" && %clientId.linebreaking != "False")
	{
		if (%clientId.linebreakexploit != 1)
			messageall(1, client::getname(%clientId) @ " was naughty, and has been jailed. No Linebreaking! Repeated attempts at malicious action will get you a character wipe and/or ban.~wdrown1.wav");
		Jail(%clientId, 120, 1);
		%clientId.linebreakexploit = 1;
		%exploitip = Client::getTransportAddress(%clientId);
		$exploitlog::entry = "[Linebreak] " @ client::getname(%clientId) @ " (" @ %exploitip @ ") sent msg with linebreak " @ %clientId.linebreaking @ ": " @ %message;
		export("$exploitlog::*", $exploitlogfile, True);
		%clientId.linebreaking = "";
		return;
	} 
		
	//-------------------------//
	%TrueClientId = %clientId;
	if(%senderName != "")
	 {
		 if(%TrueClientId == 2048) {
				%clientToServerAdminLevel = $BlockOwnerAdminLevel[%senderName];
			 } else {
				%clientToServerAdminLevel = $BlockOwnerAdminLevel[Client::GetName(%TrueClientId)];
			 }
		  %clientId = 2048;
	 } 
	else
	{
		%senderName = Client::getName(%clientId);
		%clientToServerAdminLevel = floor(%clientId.adminLevel);
	}
	if(Player::isAiControlled(%clientId))
		%clientToServerAdminLevel = 3;

	if(%TrueClientId == 2048)
		%echoOff = True;
	else
		%echoOff = %TrueClientId.echoOff;

	if(%TrueClientId != 2048)
		%TCsenderName = Client::getName(%TrueClientId);
	else
		%TCsenderName = %senderName;

	//If %senderName is empty, the rest of this function will continue normally, as both %TrueClientId and %clientId
	//are identical.  However, if %senderName is NOT empty, messages that the server should hear will be under %clientId,
	//and messages that the client RUNNING the script needs to hear will be under %TrueClientId.
	//During %senderName being NOT empty, basic player command messages are sent to the server.  These commands shouldn't
	//normally be invoked anyway, unless the scripter forces it somehow.  Block management commands should use
	//%TrueClientId because they can only be run WHILE the client is in-game, so the messages should be sent to him.
	//The rest of the commands should use %clientId because those are the ones that the server will be calling.

	//An easy to way to distinguish the tasks between client and server is that the client runs the commands that
	//manage, while the server runs the commands that do actions.

	//- %TrueClientId should be assigned to things that require client access, and need to send a message
	//  (like a confirmation or error message) to someone.
	//- %clientId should be assigned to things that do actions.

	//%TrueClientId will only become 2048 if the client leaves the game.

	//Remember that if a client disconnects, the %TrueClientId will become 2048, the same as %clientId.  This means
	//that the server will then be receiving all these messages.

	//I had to write this little commentary because I was getting confused myself...

	//NEW:
	//- %TrueClientId should be assigned to things that involve the player at hand
	//- %clientId should be assigned to things that involve control
	//-------------------------//

	%time = getIntegerTime(true) >> 5;
	if(%time - %clientId.lastSayTime <= $sayDelay && !(%clientToServerAdminLevel >= 1))
		return;
	%clientId.lastSayTime = %time;

	%msg = %clientId @ " \"" @ escapeString(%message) @ "\"";

	// check for flooding if it's a broadcast OR if it's team in FFA
	if($Server::FloodProtectionEnabled && (!$Server::TourneyMode || !%team) && !(%clientToServerAdminLevel >= 1))
	{
		// we use getIntTime here because getSimTime gets reset.
		// time is measured in 32 ms chunks... so approx 32 to the sec
		%time = getIntegerTime(true) >> 5;
		if(%TrueClientId.floodMute)
		{
			%delta = %TrueClientId.muteDoneTime - %time;
			if(%delta > 0)
			{
				Client::sendMessage(%TrueClientId, $MSGTypeGame, "FLOOD! You cannot talk for " @ %delta @ " seconds.");
				return;
			}
			%TrueClientId.floodMute = "";
			%TrueClientId.muteDoneTime = "";
		}
		%TrueClientId.floodMessageCount++;
		// funky use of schedule here:
		schedule(%TrueClientId @ ".floodMessageCount--;", 5, %TrueClientId);
		if(%TrueClientId.floodMessageCount > 6)
		{
			%TrueClientId.floodMute = true;
			%TrueClientId.muteDoneTime = %time + 10;
			Client::sendMessage(%TrueClientId, $MSGTypeGame, "FLOOD! You cannot talk for 10 seconds.");
			return;
		}
	}

	//check for a bulknum-type of message
	if(%message == floor(%message))
	{
		if(%clientId.currentShop != "" || %clientId.currentBank != "")
		{
			if(%message < 1)
				%message = 1;
			if(%message > 200)
				%message = 200;
		}
		%TrueClientId.bulkNum = %message;
	}
	//check for chat-binding messages
	if(%TrueClientId.BindingNum > 0)
	{
		storeData(%clientId, "ChatBind" @ %TrueClientId.BindingNum, %message);
		client::sendMessage(%TrueClientId, 0, "Selected key bound to \"" @ %message @ "\"");
		%clientId.BindingNum = 0;
		return;
	}

	//parse message
	%botTalk = False;
	%isCommand = False;

	if(String::getSubStr(%message, 0, 1) == "!")
	{
		%w1 = getWord(%message, 0);
		if(%w1 == "!md" || %w1 == "!definition" || %w1 == "!say")
		{
			if($dedicated)
				echo("CMD: " @ %msg);
			for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
				if(!%cl.muted[%clientId] && floor(%cl.adminLevel) > 3)
					Client::sendMessage(%cl, $MsgTypeChat, %message, %clientId);
			return;
		}
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			if(!%cl.muted[%clientId] && floor(%clientId.adminLevel) > 2)
				Client::sendMessage(%cl, $MsgTypeChat, %message, %clientId);
	}
	if(String::getSubStr(%message, 0, 1) != "#")
	{
		if(%team)
			%message = fetchData(%TrueClientId, "alternateTalk") @ " " @ %message;
		else
			%message = fetchData(%TrueClientId, "defaultTalk") @ " " @ %message;

	}
	if(String::getSubStr(%message, 0, 1) == "#")
		%isCommand = True;

	//echo("SAY: " @ %msg);

	if($exportChat)
	{
		%ip = Client::getTransportAddress(%TrueClientId);
		if(%TrueClientId.doExport)
		{
			$log::msg["[\"" @ %TCsenderName @ "\"]"] = %message;
			export("log::msg[\"" @ %TCsenderName @ "\"*", "temp\\log$ @ " @ %TCsenderName @ ".cs", true);
		}
	}

	%w1 = GetWord(%message, 0);

	//========== Redirect block commands into memory =============================================
	if(fetchData(%TrueClientId, "BlockInputFlag") != "" && String::ICompare(%w1, "#endblock") != 0 && %w1 != -1 && %message != "")
	{
		//Entering block information into memory
		%tmpBlockCnt = fetchData(%TrueClientId, "tmpBlockCnt") + 1;
		storeData(%TrueClientId, "tmpBlockCnt", %tmpBlockCnt);
		$BlockData[%TCsenderName, fetchData(%TrueClientId, "BlockInputFlag"), %tmpBlockCnt] = %message;
		return 0;
	}
	//============================================================================================

	%cropped = String::NEWgetSubStr(%message, (String::len(%w1)+1), 99999);

	if(%isCommand)
	{
		if(%w1 == "#say" || %w1 == "#s")
		{
			if(SkillCanUse(%TrueClientId, "#say"))
			{
				%talkingPos = GameBase::getPosition(%TrueClientId);
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
				{
					%receivingPos = GameBase::getPosition(%cl);
					%distVec = Vector::getDistance(%talkingPos, %receivingPos);
					if(%distVec <= $maxSAYdistVec)
					{
						//%newmsg = FadeMsg(%cropped, %distVec, $maxSAYdistVec);
						%newmsg = %cropped;
	
						if(!%cl.muted[%TrueClientId] && %cl != %TrueClientId)
							Client::sendMessage(%cl, $MsgWhite, %TCsenderName @ " says, \"" @ %newmsg @ "\"");
					}
				}
				Client::sendMessage(%TrueClientId, $MsgWhite, "You say, \"" @ %cropped @ "\"");
				UseSkill(%TrueClientId, $SkillCharisma, True, True);
	
				%botTalk = True;
			}
			else
			{
				Client::sendMessage(%TrueClientId, $MsgWhite, "You lack the necessary skills to use this command.");
				UseSkill(%TrueClientId, $SkillCharisma, False, True);
			}
		}
	
		if(%w1 == "#shout")
		{
			if(SkillCanUse(%TrueClientId, "#shout"))
			{
				%talkingPos = GameBase::getPosition(%TrueClientId);
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
				{
					%receivingPos = GameBase::getPosition(%cl);
					%distVec = Vector::getDistance(%talkingPos, %receivingPos);
					if(%distVec <= $maxSHOUTdistVec)
					{
						//%newmsg = FadeMsg(%cropped, %distVec, $maxSHOUTdistVec);
						%newmsg = %cropped;
	
						if(!%cl.muted[%TrueClientId] && %cl != %TrueClientId)
							Client::sendMessage(%cl, $MsgWhite, %TCsenderName @ " shouts, \"" @ %newmsg @ "\"");
					}
				}
				Client::sendMessage(%TrueClientId, $MsgWhite, "You shouted, \"" @ %cropped @ "\"");
				UseSkill(%TrueClientId, $SkillCharisma, True, True);
	
				%botTalk = True;
			}
			else
			{
				Client::sendMessage(%TrueClientId, $MsgWhite, "You lack the necessary skills to use this command.");
				UseSkill(%TrueClientId, $SkillCharisma, False, True);
			}
		}

		if(%w1 == "#whisper")
		{
			if(SkillCanUse(%TrueClientId, "#whisper"))
			{
				%talkingPos = GameBase::getPosition(%TrueClientId);
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
				{
					%receivingPos = GameBase::getPosition(%cl);
					%distVec = Vector::getDistance(%talkingPos, %receivingPos);
					if(%distVec <= $maxWHISPERdistVec)
					{
						//%newmsg = FadeMsg(%cropped, %distVec, $maxSHOUTdistVec);
						%newmsg = %cropped;
	
						if(!%cl.muted[%TrueClientId] && %cl != %TrueClientId)
							Client::sendMessage(%cl, $MsgWhite, %TCsenderName @ " whispers, \"" @ %newmsg @ "\"");
					}
				}
				Client::sendMessage(%TrueClientId, $MsgWhite, "You whisper, \"" @ %cropped @ "\"");
				UseSkill(%TrueClientId, $SkillCharisma, True, True);
	
				%botTalk = True;
			}
			else
			{
				Client::sendMessage(%TrueClientId, $MsgWhite, "You lack the necessary skills to use this command.");
				UseSkill(%TrueClientId, $SkillCharisma, False, True);
			}
		}
	
		if(IsJailed(%TrueClientId))
			return;

		if(%w1 == "#tell" || %w1 == "#t" || %w1 == "#pm")
		{
			if(SkillCanUse(%TrueClientId, "#tell"))
			{
				if(%cropped == "")
				{
					Client::sendMessage(%TrueClientId, 0, "syntax: #tell whoever, message");
				}
				else
				{
					%pos1 = 0;
					%pos2 = String::findSubStr(%cropped, ",");
					%name = String::getSubStr(%cropped, %pos1, %pos2-%pos1);
					%final = String::getSubStr(%cropped, %pos2 + 2, String::len(%cropped)-%pos2-2);
					%cl = NEWgetClientByName(%name);
		
					if(%cl != -1)
					{
						%n = Client::getName(%cl);	//capitalize the name properly
						if(!%cl.muted[%TrueClientId])
						{
							Client::sendMessage(%cl, $MsgBeige, %TCsenderName @ " tells you, \"" @ %final @ "\"");
							if(%cl != %TrueClientId)
								Client::sendMessage(%TrueClientId, $MsgBeige, "You tell " @ %n @ ", \"" @ %final @ "\"");
							%cl.replyTo = %TCsenderName;
	
							UseSkill(%TrueClientId, $SkillCharisma, True, True);
						}
						else
							Client::sendMessage(%TrueClientId, $MsgRed, %n @ " has muted you.");
					}
					else
						Client::sendMessage(%TrueClientId, $MsgWhite, "Invalid player name.");
				}
		
				%botTalk = True;
			}
			else
			{
				Client::sendMessage(%TrueClientId, $MsgWhite, "You lack the necessary skills to use this command.");
				UseSkill(%TrueClientId, $SkillCharisma, False, True);
			}
		}
		if(%w1 == "#r" || %w1 == "##")
		{
			if(SkillCanUse(%TrueClientId, "#tell"))
			{
				if(%cropped == "")
					Client::sendMessage(%TrueClientId, 0, "syntax: #r message");
				else
				{
					%name = %TrueClientId.replyTo;
					if(%name != "")
					{
						%cl = NEWgetClientByName(%name);
			
						if(%cl != -1)
						{
							if(!%cl.muted[%TrueClientId])
							{
								Client::sendMessage(%cl, $MsgBeige, %TCsenderName @ " tells you, \"" @ %cropped @ "\"");
								if(%cl != %TrueClientId)
									Client::sendMessage(%TrueClientId, $MsgBeige, "You tell " @ %name @ ", \"" @ %cropped @ "\"");
								%cl.replyTo = %TCsenderName;
		
								UseSkill(%TrueClientId, $SkillCharisma, True, True);
							}
						}
						else
							Client::sendMessage(%TrueClientId, $MsgWhite, "Invalid player name.");
			
						%botTalk = True;
					}
					else
						Client::sendMessage(%TrueClientId, $MsgWhite, "You haven't received a #tell to reply to yet.");
				}
			}
			else
			{
				Client::sendMessage(%TrueClientId, $MsgWhite, "You lack the necessary skills to use this command.");
				UseSkill(%TrueClientId, $SkillCharisma, False, True);
			}
			return;
		}
		if(%w1 == "#global" || %w1 == "#g" || %w1 == "#glbl")
		{
			if(SkillCanUse(%TrueClientId, "#global"))
			{
				if(!fetchData(%TrueClientId, "ignoreGlobal"))
				{
			            for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			                  if(!%cl.muted[%TrueClientId] && %cl != %TrueClientId && !fetchData(%cl, "ignoreGlobal"))
			                        Client::sendMessage(%cl, $MsgGreen, "[GLBL] " @ %TCsenderName @ " \"" @ %cropped @ "\"");
			            Client::sendMessage(%TrueClientId, $MsgGreen, "[GLBL] \"" @ %cropped @ "\"");
	
					UseSkill(%TrueClientId, $SkillCharisma, True, True);
				}
				else
			            Client::sendMessage(%TrueClientId, $MsgRed, "You can't send a Global message when ignoring other Global messages.");
			}
			else
			{
				Client::sendMessage(%TrueClientId, $MsgWhite, "You lack the necessary skills to use this command.");
				UseSkill(%TrueClientId, $SkillCharisma, False, True);
			}
			return;
		}
		if(%w1 == "#zone" || %w1 == "#z")
		{
			if(SkillCanUse(%TrueClientId, "#zone"))
			{
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
					if(!%cl.muted[%TrueClientId] && %cl != %TrueClientId && fetchData(%cl, "zone") == fetchData(%TrueClientId, "zone"))
			      		Client::sendMessage(%cl, $MsgGreen, "[ZONE] " @ %TCsenderName @ " \"" @ %cropped @ "\"");
				Client::sendMessage(%TrueClientId, $MsgGreen, "[ZONE] \"" @ %cropped @ "\"");
	
				UseSkill(%TrueClientId, $SkillCharisma, True, True);
			}
			else
			{
				Client::sendMessage(%TrueClientId, $MsgWhite, "You lack the necessary skills to use this command.");
				UseSkill(%TrueClientId, $SkillCharisma, False, True);
			}
			return;
		}
		if(%w1 == "#group")
		{
			if(SkillCanUse(%TrueClientId, "#group"))
			{
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
				{
					if(!%cl.muted[%TrueClientId] && %cl != %TrueClientId && IsInCommaList(fetchData(%TrueClientId, "grouplist"), Client::getName(%cl)))
					{
						if(IsInCommaList(fetchData(%cl, "grouplist"), %TCsenderName))
							Client::sendMessage(%cl, $MsgBeige, "[GRP] " @ %TCsenderName @ " \"" @ %cropped @ "\"");
						else
							Client::sendMessage(%TrueClientId, $MsgRed, Client::getName(%cl) @ " does not have you on his/her group-list.");
					}
				}
	
				Client::sendMessage(%TrueClientId, $MsgBeige, "[GRP] \"" @ %cropped @ "\"");
				UseSkill(%TrueClientId, $SkillCharisma, True, True);
			}
			else
			{
				Client::sendMessage(%TrueClientId, $MsgWhite, "You lack the necessary skills to use this command.");
				UseSkill(%TrueClientId, $SkillCharisma, False, True);
			}
			return;
		}
		if(%w1 == "#party" || %w1 == "#p")
		{
			if(SkillCanUse(%TrueClientId, "#party"))
			{
				%list = GetPartyListIAmIn(%TrueClientId);
				for(%p = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999))
				{
					%cl = NEWgetClientByName(String::NEWgetSubStr(%list, 0, %p));
					if(!%cl.muted[%TrueClientId] && %cl != %TrueClientId)
						Client::sendMessage(%cl, $MsgBeige, "[PRTY] " @ %TCsenderName @ " \"" @ %cropped @ "\"");
				}
	
				Client::sendMessage(%TrueClientId, $MsgBeige, "[PRTY] \"" @ %cropped @ "\"");
				UseSkill(%TrueClientId, $SkillCharisma, True, True);
			}
			else
			{
				Client::sendMessage(%TrueClientId, $MsgWhite, "You lack the necessary skills to use this command.");
				UseSkill(%TrueClientId, $SkillCharisma, False, True);
			}
			return;
		}
		if(%w1 == "#grouplist")
		{
			for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			{
				%inmylist = %inmylist @ client::getname(%cl) @ ", ";
				if(%cl != %TrueClientId && IsInCommaList(fetchData(%TrueClientId, "grouplist"), Client::getName(%cl)))
				{
					if(IsInCommaList(fetchData(%cl, "grouplist"), %TCsenderName))
						%listingme = %listingme @ client::getname(%cl) @ ", ";
				}
			}
			%inmylist = String::NEWgetSubStr(%inmylist, String::len(%inmylist)-2, 99999);
			%listingme = String::NEWgetSubStr(%listingme, String::len(%listingme)-2, 99999);
			if(%inmylist == "")
				%inmylist = "No one is in your grouplist";
			if(%listingme == "")
				%listingme = "No one in your grouplist has you in their grouplist";
			bottomprint(%TrueClientId, "Your grouplist: " @ %inmylist @ ".\n\nMutually grouping you: " @ %listingme @ ".");
			return;
		}
		if(%w1 == "#partylist" || %w1 == "#plist")
		{
			if(SkillCanUse(%TrueClientId, "#party"))
			{
				%list = GetPartyListIAmIn(%TrueClientId);
				for(%p = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999))
				{
					%cl = NEWgetClientByName(String::NEWgetSubStr(%list, 0, %p));
					if(!%cl.muted[%TrueClientId] && %cl != %TrueClientId)
						Client::sendMessage(%cl, $MsgBeige, "[PRTY] " @ %TCsenderName @ " \"" @ %cropped @ "\"");
				}
	
				Client::sendMessage(%TrueClientId, $MsgBeige, "[PRTY] \"" @ %cropped @ "\"");
				UseSkill(%TrueClientId, $SkillCharisma, True, True);
			}
			else
			{
				Client::sendMessage(%TrueClientId, $MsgWhite, "You lack the necessary skills to use this command.");
				UseSkill(%TrueClientId, $SkillCharisma, False, True);
			}
			return;
		}

		if(IsDead(%TrueClientId) && %TrueClientId != 2048)
			return;

		//check for onHear events
		if(%botTalk)
		{
			%list = GetEveryoneIdList();
			for(%i = 0; GetWord(%list, %i) != -1; %i++)
			{
				%oid = GetWord(%list, %i);
	
				%time = getIntegerTime(true) >> 5;
				if(%time - fetchData(%oid, "nextOnHear") > 0.05)
				{
					storeData(%oid, "nextOnHear", %time);
	
					%oname = Client::getName(%oid);
	
					%index = GetEventCommandIndex(%oid, "onHear");
					if(%index != -1)
					{
						for(%i2 = 0; (%index2 = GetWord(%index, %i2)) != -1; %i2++)
						{
							%ec = $EventCommand[%oid, %index2];
		
							%hearName = GetWord(%ec, 2);
							%radius = GetWord(%ec, 3);
							if(Vector::getDistance(GameBase::getPosition(%oid), GameBase::getPosition(%TrueClientId)) <= %radius)
							{
								%targetname = GetWord(%ec, 5);
								if(String::ICompare(%targetname, "all") != 0)
									%targetId = NEWgetClientByName(%targetname);
		
								if(String::ICompare(%targetname, "all") == 0 || %targetId == %TrueClientId)
								{
									%sname = GetWord(%ec, 0);
									%type = GetWord(%ec, 1);
									%keep = GetWord(%ec, 4);
									%var = GetWord(%ec, 6);
									if(String::ICompare(%var, "var") == 0)
										%var = True;
									else
									{
										%div1 = String::findSubStr(%ec, "|");
										%div2 = String::ofindSubStr(%ec, "|", %div1+1);
										%text = String::NEWgetSubStr(%ec, %div1+1, %div2);
										%oec = String::NEWgetSubStr(%ec, %div1+%div2+2, 99999);
									}
		
									if(String::ICompare(%cropped, %text) == 0 || %var)
									{
										if((%cl = NEWgetClientByName(%sname)) == -1)
											%cl = 2048;

										%cmd = String::NEWgetSubStr($EventCommand[%oid, %index2], String::findSubStr($EventCommand[%oid, %index2], ">")+1, 99999);
										if(%var)
											%cmd = String::replace(%cmd, "^var", %cropped);
		
										%pcmd = ParseBlockData(%cmd, %TrueClientId, "");
										if(!%keep)
											$EventCommand[%oid, %index2] = "";
										remoteSay(%cl, 0, %pcmd, %sname);
									}
								}
							}
						}
					}
				}
			}
		}

		//=================================================
		// Beginning of commands
		// (player can't use any of these while dead)
		//=================================================

		if(%w1 == "#steal")
		{
			%time = getIntegerTime(true) >> 5;
			if(%time - %TrueClientId.lastStealTime > $stealDelay)
			{	
				if((%reason = AllowedToSteal(%TrueClientId)) == "True")
				{
					if(SkillCanUse(%TrueClientId, "#steal"))
					{
						if(GameBase::getLOSinfo(Client::getOwnedObject(%TrueClientId), 1))
						{
							%id = Player::getClient($los::object);

							if(getObjectType($los::object) == "Player")
							{
								if(!Player::isAiControlled(%id) && %time - %TrueClientId.lastStealTime > $stealInterval)
								{
									Client::sendMessage(%TrueClientId, $MsgRed, "You feel that it would be prudent to wait a little longer to try stealing again, you wouldn't want to get caught...");
									UseSkill(%TrueClientId, $SkillStealing, False, True);
									PostSteal(%TrueClientId, False, 0);
								}

								%victimName = Client::getName(%id);
								%stealerName = %TCsenderName;
								%victimCoins = fetchData(%id, "COINS");
								%fail = False;
								if(%victimCoins > 0)
								{
									%r1 = GetRoll("1d" @ ($PlayerSkill[%TrueClientId, $SkillStealing] * (4/5)));
									%r2 = GetRoll("1d" @ $PlayerSkill[%id, $SkillStealing]);
									%a = %r1 - %r2;
									if(%a > 0)
									{
										%amount = floor(%a * getRandom() * 1.2);
										if(%amount > %victimCoins)
											%amount = %victimCoins;
	
										if(%amount > 0)
										{
											storeData(%TrueClientId, "COINS", %amount, "inc");
											storeData(%id, "COINS", %amount, "dec");
											PerhapsPlayStealSound(%TrueClientId, 0);
	
											Client::sendMessage(%TrueClientId, $MsgTypeChat, "You successfully stole " @ %amount @ " credits from " @ %victimName @ "!");
			
											RefreshAll(%TrueClientId);
											RefreshAll(%id);
	
											UseSkill(%TrueClientId, $SkillStealing, True, True);
											PostSteal(%TrueClientId, True, 0, %amount);
										}
										else
											%fail = True;
									}
									else
										%fail = True;
	
									if(%fail)
									{
										Client::sendMessage(%TrueClientId, $MsgRed, "You failed to steal from " @ %victimName @ "!");
										Client::sendMessage(%id, $MsgRed, %stealerName @ " just failed to steal from you!");
	
										UseSkill(%TrueClientId, $SkillStealing, False, True);
										PostSteal(%TrueClientId, False, 0);
									}
								}
								else
			                                    		Client::sendMessage(%TrueClientId, $MsgRed, %victimName @ " doesn't appear to be carrying any credits...");
							}
						}
					}
					else
					{
						Client::sendMessage(%TrueClientId, $MsgWhite, "You can't steal because you lack the necessary skills.");
						UseSkill(%TrueClientId, $SkillStealing, False, True);
					}
				}
				else
					Client::sendMessage(%TrueClientId, $MsgRed, %reason);

				%TrueClientId.lastStealTime = %time;
			}
			return;
		}
		if(%w1 == "#savecharacter" || %w1 == "#savechar" || %w1 == "#save")
		{
	            if(%clientToServerAdminLevel >= 4)
	            {
	                  if(%cropped == "")
	                  {
	                        %r = SaveCharacter(%TrueClientId);
	                        Client::sendMessage(%TrueClientId, 0, "Saving self (" @ %TrueClientId @ "): success = " @ %r);
	                  }
	                  else
	                  {
	                        %id = NEWgetClientByName(%cropped);
	                        if(%id)
	                        {
	                              %r = SaveCharacter(%id);
	                              Client::sendMessage(%TrueClientId, 0, "Saving " @ Client::getName(%id) @ " (" @ %id @ "): success = " @ %r);
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	            }
	            else
	            {
				%time = getIntegerTime(true) >> 5;
				if(%time - %TrueClientId.lastSaveCharTime > 10)
				{
					%TrueClientId.lastSaveCharTime = %time;
	
		                  %r = SaveCharacter(%TrueClientId);
					Client::sendMessage(%TrueClientId, 0, "Saving self (" @ %TrueClientId @ "): success = " @ %r);
				}
	            }
			return;
		}
		if(%w1 == "#whatismyclientid")
		{
	            Client::sendMessage(%TrueClientId, 0, "Your clientId is " @ %TrueClientId);
			return;
		}
		if(%w1 == "#whatismyplayerid")
		{
	            Client::sendMessage(%TrueClientId, 0, "Your playerId is " @ Client::getOwnedObject(%TrueClientId));
			return;
		}
		if(%w1 == "#dropcoins" || $w == "#dropcredits")
		{
	            %cropped = GetWord(%cropped, 0);
	
	            if(%cropped == "all")
	                  %cropped = fetchData(%TrueClientId, "COINS");
	            else
	                  %cropped = floor(%cropped);
	
	            if(fetchData(%TrueClientId, "COINS") >= %cropped || %clientToServerAdminLevel >= 4)
	            {
	                  if(%cropped > 0)
	                  {
	                        if( !(%clientToServerAdminLevel >= 4) )
						storeData(%TrueClientId, "COINS", %cropped, "dec");
	
					%toss = GetTypicalTossStrength(%TrueClientId);
	
	                        TossLootbag(%TrueClientId, "COINS " @ %cropped, %toss, "*", 0);
					RefreshAll(%TrueClientId);
	
	                        Client::sendMessage(%TrueClientId, 0, "You dropped " @ %cropped @ " credits.");
	                        playSound(SoundMoney1, GameBase::getPosition(%TrueClientId));
	                  }
	            }
	            else
	            {
	                  Client::sendMessage(%TrueClientId, 0, "You don't even have that many credits!");
	            }
			return;
		}
		if(%w1 == "#compass")
		{
	            if(%cropped == "")
	                  Client::sendMessage(%TrueClientId, 0, "Use #compass town or #compass dungeon. (Do not specify which, simply write town or dungeon)");
	            else
	            {
				if(SkillCanUse(%TrueClientId, "#compass"))
				{
					%mpos = GetNearestZone(%TrueClientId, %cropped, 4);
	
					if(%mpos != False)
					{
						%d = GetNESW(GameBase::getPosition(%TrueClientId), %mpos);
						UseSkill(%TrueClientId, $SkillSenseHeading, True, True);
	
						Client::sendMessage(%TrueClientId, 0, "The nearest " @ %cropped @ " is " @ %d @ " of here.");
					}
					else
						Client::sendMessage(%TrueClientId, 1, "Error finding a zone!");
				}
				else
				{
					Client::sendMessage(%TrueClientId, $MsgWhite, "You can't use your compass because you lack the necessary skills.");
					UseSkill(%TrueClientId, $SkillSenseHeading, False, True);
				}
	            }
	     		return;
		}
		if(%w1 == "#getinfo")
		{
	            %cropped = GetWord(%cropped, 0);
	
	            if(%cropped == "")
	                  Client::sendMessage(%TrueClientId, 0, "Please specify a name.");
	            else
	            {
				%id = NEWgetClientByName(%cropped);
				if(%id != -1)
					DisplayGetInfo(%TrueClientId, %id, Client::getOwnedObject(%id));
				else
					Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
			}
			return;
		}
		if(%w1 == "#setinfo")
		{
	            if(%cropped == "")
	                  Client::sendMessage(%TrueClientId, 0, "Please specify text.");
	            else
	            {
				storeData(%TrueClientId, "PlayerInfo", %cropped);
	                  Client::sendMessage(%TrueClientId, 0, "Info set.  Use #getinfo [name] to retrieve this type of information.");
			}
			return;
		}
		if(%w1 == "#addinfo")
		{
	            if(%cropped == "")
	                  Client::sendMessage(%TrueClientId, 0, "Please specify text.");
	            else
	            {
				storeData(%TrueClientId, "PlayerInfo", %cropped, "strinc");
	                  Client::sendMessage(%TrueClientId, 0, "Info added to the end of previous info.");
			}
			return;
		}
		if(%w1 == "#w")
		{
	            %item = getCroppedItem(%cropped);
	
	            if(%item == "")
	                  Client::sendMessage(%TrueClientId, 0, "Please specify an item (ex: Black Statue = BlackStatue).");
	            else
	            {
				%msg = WhatIs(%item);
				bottomprint(%TrueClientId, %msg, floor(String::len(%msg) / 20));
			}
			return;
		}
//Original, vulnerable #cast/#spell:
//		if(%w1 == "#spell" || %w1 == "#cast" || %w1 == "#force")
//		{
//			if(fetchData(%TrueClientId, "SpellCastStep") == 1)
//				Client::sendMessage(%TrueClientId, 0, "You are already casting a spell!");
//			else if(fetchData(%TrueClientId, "SpellCastStep") == 2)
//				Client::sendMessage(%TrueClientId, 0, "You are still recovering from your last spell cast.");
//			else if(%TrueClientId.sleepMode != "" && %TrueClientId.sleepMode != False)
//				Client::sendMessage(%TrueClientId, $MsgRed, "You can not cast a spell while sleeping or meditating.");
//			else if(IsDead(%TrueClientId))
//				Client::sendMessage(%TrueClientId, $MsgRed, "You can not cast a spell when dead.");
//			else
//			{
//		            if(%cropped == "")
//					Client::sendMessage(%TrueClientId, 0, "Specify a spell.");
//		            else
//		            {
//					BeginCastSpell(%TrueClientId, %cropped);
//				}
//			}
//			return;
//		}
//Hack fixed #cast/#spell:
		if(%w1 == "#spell" || %w1 == "#cast" || %w1 == "#force")
		{
			%cropped = String::Replace(%cropped, "\"", "");
		////This section will actually make a log file of the person and ip who tries to hack your server////
			if (string::findsubstr(%cropped, "@") != -1)
			{
				if (%TrueClientId.castexploit != 1)
					messageall(1, client::getname(%TrueClientId) @ " attempted to exploit #cast/#spell/#force, and has been jailed. Repeated offenses get the ban!~wdrown1.wav");
				Jail(%TrueClientId, 120, 1);
				%TrueClientId.castexploit = 1;
				%exploitip = Client::getTransportAddress(%TrueClientId);
				$exploitlog::entry = "[#cast code execution] " @ client::getname(%TrueClientId) @ " (" @ %exploitip @ ") tried to cast: " @ %cropped;
				export("$exploitlog::*", $explotlogfile, True);
			}

			if(fetchData(%TrueClientId, "SpellCastStep") == 1)
				Client::sendMessage(%TrueClientId, 0, "You are already casting a spell!");
			else if(fetchData(%TrueClientId, "SpellCastStep") == 2)
				Client::sendMessage(%TrueClientId, 0, "You are still recovering from your last spell cast." @ fetchData(%TrueClientId, "zonedesc"));
			else if(%TrueClientId.sleepMode != "" && %TrueClientId.sleepMode != False)
				Client::sendMessage(%TrueClientId, $MsgRed, "You can not cast a spell while sleeping or meditating.");
			else
			{
				if(%cropped == "")
					Client::sendMessage(%TrueClientId, 0, "Specify a spell.");
				else
					BeginCastSpell(%TrueClientId, %cropped);
			}
			return;
		}
		if(%w1 == "#recall")
		{
			%zvel = floor(getWord(Item::getVelocity(%TrueClientId), 2));
			Client::sendMessage(%TrueClientId, $MsgRed, "ATTEMPTING RECALL");
			if(%zvel <= -350 || %zvel >= 350)
			{
				FellOffMap(%TrueClientId);
				CheckAndBootFromArena(%TrueClientId);
	
				%zv = "PASS";
			}
			else
				%zv = "FAIL";
			
			Client::sendMessage(%TrueClientId, $MsgBeige, "Z-Velocity check: " @ %zv);
	
			if(%zv != "PASS" && !fetchData(%TrueClientId, "tmprecall"))
			{
				%seconds = $recallDelay;
				storeData(%TrueClientId, "tmprecall", True);
				Client::sendMessage(%TrueClientId, $MsgBeige, "Stay at your current position for the next " @ %seconds @ " seconds to recall.");
	
				schedule("storeData(" @ %TrueClientId @ ", \"tmprecall\", \"\");if(Vector::getDistance(\"" @ GameBase::getPosition(%TrueClientId) @ "\", GameBase::getPosition(" @ %TrueClientId @ ")) <= 1){FellOffMap(" @ %TrueClientId @ ");CheckAndBootFromArena(" @ %TrueClientId @ ");}", %seconds);
			}
			return;
		}
		if(%w1 == "#track")
		{
	            %cropped = GetWord(%cropped, 0);
	
	            if(%cropped == "")
	                  Client::sendMessage(%TrueClientId, 0, "Please specify a name.");
	            else
	            {
				if(SkillCanUse(%TrueClientId, "#track"))
				{
					%id = NEWgetClientByName(%cropped);
					%cropped = Client::getName(%id);
					if(%id != -1)
					{
						%clientIdpos = GameBase::getPosition(%TrueClientId);
						%idpos = fetchData(%id, "lastScent");

						if(%idpos != "")
						{
							%dist = round(Vector::getDistance(%clientIdpos, %idpos));
	
							if(Cap($PlayerSkill[%TrueClientId, $SkillSenseHeading] * 7.5, 100, "inf") >= %dist)
							{
								%d = GetNESW(%clientIdpos, %idpos);
								Client::sendMessage(%TrueClientId, $MsgWhite, "You sense that " @ %cropped @ " is " @ %d @ " of here, " @ %dist @ " meters away.");
								UseSkill(%TrueClientId, $SkillSenseHeading, True, True);
							}
							else
							{
								Client::sendMessage(%TrueClientId, $MsgWhite, "You have no idea where " @ %cropped @ " could be.");
								UseSkill(%TrueClientId, $SkillSenseHeading, False, True);
							}
						}
						else
						{
							Client::sendMessage(%TrueClientId, $MsgWhite, "You have no idea where " @ %cropped @ " could be.");
							UseSkill(%TrueClientId, $SkillSenseHeading, False, True);
						}
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
				}
				else
				{
					Client::sendMessage(%TrueClientId, $MsgWhite, "You can't track because you lack the necessary skills.");
					UseSkill(%TrueClientId, $SkillSenseHeading, False, True);
				}
			}
			return;
		}
		if(%w1 == "#trackpack")
		{
	            %cropped = GetWord(%cropped, 0);
	
	            if(%cropped == "")
	                  Client::sendMessage(%TrueClientId, 0, "Please specify a name.");
	            else
	            {
				if(SkillCanUse(%TrueClientId, "#trackpack"))
				{
					%id = NEWgetClientByName(%cropped);
					if(%id != -1)
					{
						%cropped = Client::getName(%id);	//properly capitalize name
	
						%closest = 5000000;
						%closestId = -1;
						%clientIdpos = GameBase::getPosition(%TrueClientId);
						%list = fetchData(%id, "lootbaglist");
						for(%i = String::findSubStr(%list, ","); String::findSubStr(%list, ",") != -1; %list = String::NEWgetSubStr(%list, %i+1, 99999))
						{
							%id = String::NEWgetSubStr(%list, 0, %i);
							%idpos = GameBase::getPosition(%id);
							%dist = round(Vector::getDistance(%clientIdpos, %idpos));
							if(%dist < %closest)
							{
								%closest = %dist;
								%closestId = %id;
							}
						}
						if(%closestId != -1)
						{
							%idpos = GameBase::getPosition(%closestId);
	
							if(Cap($PlayerSkill[%TrueClientId, $SkillSenseHeading] * 15, 100, "inf") >= %closest)
							{
								%d = GetNESW(%clientIdpos, %idpos);
								Client::sendMessage(%TrueClientId, $MsgWhite, %cropped @ "'s nearest backpack is " @ %d @ " of here, " @ %closest @ " meters away.");
								UseSkill(%TrueClientId, $SkillSenseHeading, True, True);
							}
							else
							{
								Client::sendMessage(%TrueClientId, $MsgWhite, %cropped @ "'s nearest backpack is too far from you to track with your current sense heading skills.");
								UseSkill(%TrueClientId, $SkillSenseHeading, False, True);
							}
						}
						else
						{
							Client::sendMessage(%TrueClientId, $MsgWhite, %cropped @ " doesn't have any dropped backpacks.");
							UseSkill(%TrueClientId, $SkillSenseHeading, False, True);
						}
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
				}
				else
				{
					Client::sendMessage(%TrueClientId, $MsgWhite, "You can't track a backpack because you lack the necessary skills.");
					UseSkill(%TrueClientId, $SkillSenseHeading, False, True);
				}
			}
			return;
		}
		if(%w1 == "#sharepack")
		{
			%time = getIntegerTime(true) >> 5;
			if(%time - %TrueClientId.lastSharePackTime > 5)
			{
				%TrueClientId.lastSharePackTime = %time;
	
				%c1 = GetWord(%cropped, 0);
				%c2 = GetWord(%cropped, 1);
	
				if(%c1 != -1 && %c2 != -1)
				{
					%id = NEWgetClientByName(%c1);
		
					if(%id != -1 && Client::getName(%id) != %senderName)
					{
						%c1 = Client::getName(%id);	//properly capitalize name
						if(floor(%c2) != 0 || %c2 == "*")
						{
							%flag = "";
							%cnt = 0;
							%list = fetchData(%TrueClientId, "lootbaglist");
							for(%i = String::findSubStr(%list, ","); String::findSubStr(%list, ",") != -1; %list = String::NEWgetSubStr(%list, %i+1, 99999))
							{
								%cnt++;
								%bid = String::NEWgetSubStr(%list, 0, %i);
		
								if(%cnt == %c2 || %c2 == "*")
								{
									%flag++;
		
									%nl = GetWord($loot[%bid], 1);
									if(%nl != "*")
									{
										$loot[%bid] = String::Replace($loot[%bid], %nl, AddToCommaList(%nl, %c1));
										Client::sendMessage(%TrueClientId, $MsgBeige, "Adding " @ %c1 @ " to backpack #" @ %cnt @ " (" @ %bid @ ")'s share list.");
										Client::sendMessage(%id, $MsgBeige, %TCsenderName @ " is sharing his/her backpack #" @ %cnt @ " with you.");
									}
									else
										Client::sendMessage(%TrueClientId, 0, "Backpack #" @ %cnt @ " is already publicly available.");
								}
							}
							
							if(%flag == "")
								Client::sendMessage(%TrueClientId, 0, "Invalid backpack number.");
						}
						else
							Client::sendMessage(%TrueClientId, 0, "Please specify a backpack number (1, 2, 3, etc, or * for all)");
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Invalid player name or same player name.");
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
			}
			return;
		}
		if(%w1 == "#unsharepack")
		{
			%c1 = GetWord(%cropped, 0);
			%c2 = GetWord(%cropped, 1);
	
			if(%c1 != -1 && %c2 != -1)
			{
				%id = NEWgetClientByName(%c1);
	
				if(%id != -1 && Client::getName(%id) != %senderName)
				{
					%c1 = Client::getName(%id);	//properly capitalize name
					if(floor(%c2) != 0 || %c2 == "*")
					{
						%flag = "";
						%cnt = 0;
						%list = fetchData(%TrueClientId, "lootbaglist");
						for(%i = String::findSubStr(%list, ","); String::findSubStr(%list, ",") != -1; %list = String::NEWgetSubStr(%list, %i+1, 99999))
						{
							%cnt++;
							%bid = String::NEWgetSubStr(%list, 0, %i);
	
							if(%cnt == %c2 || %c2 == "*")
							{
								%flag++;
	
								%nl = GetWord($loot[%bid], 1);
								if(%nl != "*")
								{
									$loot[%bid] = String::Replace($loot[%bid], %nl, RemoveFromCommaList(%nl, %c1));
									Client::sendMessage(%TrueClientId, $MsgBeige, "Removing " @ %c1 @ " from backpack #" @ %cnt @ " (" @ %bid @ ")'s share list.");
									Client::sendMessage(%id, $MsgBeige, %TCsenderName @ " has removed you from his/her backpack #" @ %cnt @ " share list.");
								}
								else
									Client::sendMessage(%TrueClientId, 0, "Backpack #" @ %cnt @ " is already publicly available.  Its share list can not be changed.");
							}
						}
						
						if(%flag == "")
							Client::sendMessage(%TrueClientId, 0, "Invalid backpack number.");
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Please specify a backpack number (1, 2, 3, etc, or * for all)");
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Invalid player name or same player name.");
			}
			else
				Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	
			return;
		}
		if(%w1 == "#packsummary")
		{
			if(%clientToServerAdminLevel >= 1)
			{
				if(%cropped != "")
				{
					%id = NEWgetClientByName(%cropped);
					%cropped = Client::getName(%id);	//properly capitalize name
	
					%cnt = floor(CountObjInCommaList(fetchData(%id, "lootbaglist")));
					Client::sendMessage(%TrueClientId, 0, %cropped @ " has " @ %cnt @ " dropped backpacks.");
				}
				else
				{
					%list = GetPlayerIdList();
					for(%i = 0; (%id = GetWord(%list, %i)) != -1; %i++)
					{
						%cnt = CountObjInCommaList(fetchData(%id, "lootbaglist"));
						if(%cnt > 0)
							Client::sendMessage(%TrueClientId, 0, Client::getName(%id) @ " has " @ %cnt @ " dropped backpacks.");
					}
				}
			}
			else if(%cropped == "")
			{
				%cnt = floor(CountObjInCommaList(fetchData(%TrueClientId, "lootbaglist")));
				Client::sendMessage(%TrueClientId, 0, "You have a total of " @ %cnt @ " currently dropped backpacks.");
			}
			return;
		}
			
		if(%w1 == "#mypassword")
		{
		      %c1 = GetWord(%cropped, 0);
	
	            if(%c1 != -1)
			{
				storeData(%TrueClientId, "password", %c1);
				Client::sendMessage(%TrueClientId, 0, "Changed personal password to " @ fetchData(%TrueClientId, "password") @ ".");
			}
			else
				Client::sendMessage(%TrueClientId, 0, "Please specify a one-word password.");
	
			return;
		}
		if(%w1 == "#sleep")
		{
			if(fetchData(%TrueClientId, "InSleepZone") != "" && %TrueClientId.sleepMode == "" && !IsDead(%TrueClientId))
				%flag = True;
			if(String::ICompare(fetchData(%TrueClientId, "CLASS"), "Ranger") == 0 && fetchData(%TrueClientId, "zone") == "" && %TrueClientId.sleepMode == "" && !IsDead(%TrueClientId))
				%flag = True;
			
			if(%flag)
			{
				%TrueClientId.sleepMode = 1;
				Client::setControlObject(%TrueClientId, Client::getObserverCamera(%TrueClientId));
				Observer::setOrbitObject(%TrueClientId, Client::getOwnedObject(%TrueClientId), 30, 30, 30);
				refreshHPREGEN(%TrueClientId);
				refreshMANAREGEN(%TrueClientId);
	
				Client::sendMessage(%TrueClientId, $MsgWhite, "You fall asleep...  Use #wake to wake up.");
			}
			else
				Client::sendMessage(%TrueClientId, $MsgRed, "You can't seem to fall asleep here.");
	
			return;
		}
		if(%w1 == "#meditate")
		{
		    if(SkillCanUse(%TrueClientId, "#meditate"))
		    {
			if(%TrueClientId.sleepMode == "" && !IsDead(%TrueClientId) && $possessedBy[%TrueClientId].possessId != %TrueClientId)
			{
				%TrueClientId.sleepMode = 2;
				Client::setControlObject(%TrueClientId, Client::getObserverCamera(%TrueClientId));
				Observer::setOrbitObject(%TrueClientId, Client::getOwnedObject(%TrueClientId), 30, 30, 30);
				refreshHPREGEN(%TrueClientId);
				refreshMANAREGEN(%TrueClientId);
	
				Client::sendMessage(%TrueClientId, $MsgWhite, "You begin to meditate.  Use #wake to stop meditating.");
			}
			else
				Client::sendMessage(%TrueClientId, $MsgRed, "You can't seem to meditate.");
		    }
		    else
		    {
			Client::sendMessage(%TrueClientId, $MsgRed, "You don't know how to meditate. Usually it's a Jedi thing.");
		    }
	
			return;
		}
		if(%w1 == "#wake")
		{
			if(%TrueClientId.sleepMode != "")
			{
				%TrueClientId.sleepMode = "";
				Client::setControlObject(%TrueClientId, %TrueClientId);
				refreshHPREGEN(%TrueClientId);
				refreshMANAREGEN(%TrueClientId);
	
				Client::sendMessage(%TrueClientId, $MsgWhite, "You awake.");
			}
			else
				Client::sendMessage(%TrueClientId, $MsgRed, "You are not sleeping or meditating.");
	
			return;
		}
		if(%w1 == "#roll")
		{
		      %c1 = GetWord(%cropped, 0);
	
	            if(%c1 != -1)
				Client::sendMessage(%TrueClientId, 0, %c1 @ ": " @ GetRoll(%c1));
			else
				Client::sendMessage(%TrueClientId, 0, "Please specify a roll (example: 1d6)");
	
			return;
		}
		if(%w1 == "#hide")
		{
			if(SkillCanUse(%TrueClientId, "#hide"))
			{
				if(!fetchData(%TrueClientId, "invisible") && !fetchData(%TrueClientId, "blockHide"))
				{
					%closeEnoughToWall = Cap($PlayerSkill[%TrueClientId, $SkillHiding] / 125, 3.5, 8);
	
					%pos = GameBase::getPosition(%TrueClientId);
	
					%closest = 10000;
					for(%i = 0; %i <= 6.283; %i+= 0.52)
					{
						GameBase::getLOSinfo(Client::getOwnedObject(%TrueClientId), 25, "0 0 " @ %i);
						%dist = Vector::getDistance(%pos, $los::position);
						if(%dist < %closest && $los::position != "0 0 0" && $los::position != "")
							%closest = %dist;
					}
	
					if(%closest <= %closeEnoughToWall)
					{
						Client::sendMessage(%TrueClientId, $MsgBeige, "You are successful at Hide In Shadows.");
	
						GameBase::startFadeOut(%TrueClientId);
						storeData(%TrueClientId, "invisible", True);
	
						%grace = Cap($PlayerSkill[%TrueClientId, $SkillHiding] / 10, 5, 100);
						WalkSlowInvisLoop(%TrueClientId, 5, %grace);
	
						UseSkill(%TrueClientId, $SkillHiding, True, True);
					}
					else
					{
						Client::sendMessage(%TrueClientId, $MsgWhite, "You were unsuccessful at Hide In Shadows.");
						UseSkill(%TrueClientId, $SkillHiding, False, True);
					}
				}
			}
			else
			{
				Client::sendMessage(%TrueClientId, $MsgWhite, "You can't hide because you lack the necessary skills.");
				UseSkill(%TrueClientId, $SkillHiding, False, True);
			}
			return;
		}
		if(%w1 == "#bash")
		{
			if(!fetchData(%TrueClientId, "blockBash"))
			{
				if(SkillCanUse(%TrueClientId, "#bash"))
				{
					Client::sendMessage(%TrueClientId, $MsgBeige, "You are ready to bash!");
					storeData(%TrueClientId, "NextHitBash", True);
					storeData(%TrueClientId, "blockBash", True);
				}
				else
				{
					Client::sendMessage(%TrueClientId, $MsgWhite, "You can't bash because you lack the necessary skills.");
					UseSkill(%TrueClientId, $SkillBashing, False, True);
				}
			}
			return;
		}
		if(%w1 == "#shove")
		{
			%time = getIntegerTime(true) >> 5;
			if(%time - %TrueClientId.lastShoveTime > 1.5)
			{
				%TrueClientId.lastShoveTime = %time;
	
				if(SkillCanUse(%TrueClientId, "#shove"))
				{
					%player = Client::getOwnedObject(%TrueClientId);
					if(GameBase::getLOSinfo(%player, 2))
					{
						%id = Player::getClient($los::object);
		
						if(!(Player::isAiControlled(%id) && GameBase::getTeam(%id) == GameBase::getTeam(%TrueClientId)))
						{
							if(%TrueClientId.adminLevel > %id.adminLevel || %id.adminLevel < 1)
							{
								%b = GameBase::getRotation(%TrueClientId);
								%c1 = Cap(20 + fetchData(%TrueClientId, "LVL"), 0, 250);
								%c2 = %c1 / 4;
								%mom = Vector::getFromRot( %b, %c1, %c2 );
		
								Player::applyImpulse(%id, %mom);
								client::sendMessage(%id, 1, client::getName(%TrueClientId) @ " shoved you!");
							}
						}
					}
				}
				else
					Client::sendMessage(%TrueClientId, $MsgWhite, "You can't shove because you lack the necessary skills.");
			}
			return;
		}
		if(%w1 == "#defaulttalk")
		{
	            if(%cropped != "")
			{
				storeData(%TrueClientId, "defaultTalk", %cropped);
				Client::sendMessage(%TrueClientId, 0, "Changed Default Talk to " @ fetchData(%TrueClientId, "defaultTalk") @ ".");
			}
			else
				Client::sendMessage(%TrueClientId, 0, "Please specify what will be added to the beginning of each of your messages.");
	
			return;
		}
		if(%w1 == "#alternatetalk" || %w1 == "#secondarytalk")
		{
	            if(%cropped != "")
			{
				storeData(%TrueClientId, "alternateTalk", %cropped);
				Client::sendMessage(%TrueClientId, 0, "Changed Alternate Talk to " @ fetchData(%TrueClientId, "alternateTalk") @ ".");
			}
			else
				Client::sendMessage(%TrueClientId, 0, "Please specify what will be added to the beginning of each of your team-chat-button messages.");
	
			return;
		}
		if(%w1 == "#zonelist")
		{
			if(SkillCanUse(%TrueClientId, "#zonelist"))
			{
			      %c1 = GetWord(%cropped, 0);
	
		            if(%c1 != -1)
				{
					if(String::ICompare(%c1, "all") == 0)
						%t = 1;
					else if(String::ICompare(%c1, "players") == 0)
						%t = 2;
					else if(String::ICompare(%c1, "enemies") == 0)
						%t = 3;
	
					%list = Zone::getPlayerList(fetchData(%TrueClientId, "zone"), %t);
	
					if(%list != "")
					{
						for(%i = 0; (%id = GetWord(%list, %i)) != -1; %i++)
							Client::sendMessage(%TrueClientId, $MsgBeige, Client::getName(%id));
					}
					else
						Client::sendMessage(%TrueClientId, $MsgRed, "[none]");
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Please specify 'players', 'enemies', or 'all'");
			}
			else
			{
				Client::sendMessage(%TrueClientId, $MsgWhite, "You can't zonelist because you lack the necessary skills.");
				UseSkill(%TrueClientId, $SkillSenseHeading, False, True);
			}
			return;
		}
		if(%w1 == "#pickpocket")
		{
			%time = getIntegerTime(true) >> 5;
			if(%time - %TrueClientId.lastStealTime > $stealDelay)
			{
				%TrueClientId.lastStealTime = %time;
				if((%reason = AllowedToSteal(%TrueClientId)) == "True")
				{
					if(SkillCanUse(%TrueClientId, "#pickpocket"))
					{
						if(GameBase::getLOSinfo(Client::getOwnedObject(%TrueClientId), 1))
						{
							%id = Player::getClient($los::object);
							if(getObjectType($los::object) == "Player" && !Player::isAiControlled(%id) && %time - %TrueClientId.lastPickPocketTime > $pickPocketInterval)
							{
								%TrueClientId.stealType = 1;
								SetupInvSteal(%TrueClientId, %id);
							}
							else
							{
								Client::sendMessage(%TrueClientId, $MsgRed, "You feel that it would be prudent to wait a little longer to try pickpocketing again, you wouldn't want to get caught...");
								UseSkill(%TrueClientId, $SkillStealing, False, True);
							}
						}
					}
					else
					{
						Client::sendMessage(%TrueClientId, $MsgWhite, "You can't pickpocket because you lack the necessary skills.");
						UseSkill(%TrueClientId, $SkillStealing, False, True);
					}
				}
				else
					Client::sendMessage(%TrueClientId, $MsgRed, %reason);

				%TrueClientId.lastPickPocketTime = %time;
			}
			return;
		}
		if(%w1 == "#mug")
		{
			%time = getIntegerTime(true) >> 5;
			if(%time - %TrueClientId.lastStealTime > $stealDelay)
			{
				%TrueClientId.lastStealTime = %time;

				if((%reason = AllowedToSteal(%TrueClientId)) == "True")
				{
					if(SkillCanUse(%TrueClientId, "#mug"))
					{
						if(GameBase::getLOSinfo(Client::getOwnedObject(%TrueClientId), 1))
						{
							%id = Player::getClient($los::object);
							if(getObjectType($los::object) == "Player" && !Player::isAiControlled(%id)  && %time - %TrueClientId.lastMugTime > $mugInterval)
							{
								%TrueClientId.stealType = 2;
								SetupInvSteal(%TrueClientId, %id);
							}
							else
							{
								Client::sendMessage(%TrueClientId, $MsgRed, "You feel that it would be prudent to wait a little longer to try mugging again, you wouldn't want to get caught...");
								UseSkill(%TrueClientId, $SkillStealing, False, True);
							}
						}
					}
					else
					{
						Client::sendMessage(%TrueClientId, $MsgWhite, "You can't mug because you lack the necessary skills.");
						UseSkill(%TrueClientId, $SkillStealing, False, True);
					}
				}
				else
					Client::sendMessage(%TrueClientId, $MsgRed, %reason);
				%TrueClientId.lastMugTime = %time;
			}
			return;
		}
		if(%w1 == "#createpack")
		{
			if(fetchData(%TrueClientId, "TempPack") != "")
			{
				if(HasThisStuff(%TrueClientId, fetchData(%TrueClientId, "TempPack")))
				{
					TakeThisStuff(%TrueClientId, fetchData(%TrueClientId, "TempPack"));
					%namelist = %TCsenderName @ ",";
					TossLootbag(%TrueClientId, fetchData(%TrueClientId, "TempPack"), 5, %namelist, 0);
					RefreshAll(%TrueClientId);
	
					remotePlayMode(%TrueClientId);
				}
			}
			return;
		}
		if(%w1 == "#camp")
		{
			if(Player::getItemCount(%TrueClientId, Tent))
			{
				%camp = nameToId("MissionCleanup\\Camp" @ %TrueClientId);
				if(%camp == -1)
				{
					if(fetchData(%TrueClientId, "zone") == "")
					{
						Client::sendMessage(%TrueClientId, $MsgBeige, "Setting up camp...");
			
						%pos = GameBase::getPosition(%TrueClientId);
			
						Player::decItemCount(%TrueClientId, Tent);
						RefreshAll(%TrueClientId);
						%group = newObject("Camp" @ %TrueClientId, SimGroup);
						addToSet("MissionCleanup", %group);
		
						schedule("DoCampSetup(" @ %TrueClientId @ ", 1, \"" @ %pos @ "\");", 2, %group);
						schedule("DoCampSetup(" @ %TrueClientId @ ", 2, \"" @ %pos @ "\");", 10, %group);
						schedule("DoCampSetup(" @ %TrueClientId @ ", 3, \"" @ %pos @ "\");", 17, %group);
						schedule("DoCampSetup(" @ %TrueClientId @ ", 4, \"" @ %pos @ "\");", 20, %group);
					}
					else
						Client::sendMessage(%TrueClientId, $MsgRed, "You can't set up a camp here.");
				}
				else
					Client::sendMessage(%TrueClientId, $MsgRed, "You already have a camp setup somewhere.");
			}
			else
				Client::sendMessage(%TrueClientId, $MsgRed, "You aren't carrying a tent.");
	
			return;
		}
		if(%w1 == "#uncamp")
		{
			%camp = nameToId("MissionCleanup\\Camp" @ %TrueClientId);
			if(%camp != -1)
			{
				%obj = nameToId("MissionCleanup\\Camp" @ %TrueClientId @ "\\woodfire");
				if(Vector::getDistance(GameBase::getPosition(%TrueClientId), GameBase::getPosition(%obj)) <= 10)
				{
					DoCampSetup(%TrueClientId, 5);
					Client::sendMessage(%TrueClientId, $MsgBeige, "Camp has been packed up.");
				}
				else
					Client::sendMessage(%TrueClientId, $MsgRed, "You are too far from your camp.");
			}
			else
				Client::sendMessage(%TrueClientId, $MsgRed, "You don't have a camp.");
	
			return;
		}
		if(%w1 == "#advcompass" || %w1 == "#gps")
		{
	            if(%cropped == "")
	                  Client::sendMessage(%TrueClientId, 0, "Use #advcompass zone keyword");
	            else
	            {
				if(SkillCanUse(%TrueClientId, "#advcompass"))
				{
					%obj = GetZoneByKeywords(%TrueClientId, %cropped, 3);
	
					if(%obj != False)
					{
						%mpos = Zone::getMarker(%obj);
	
						%d = GetNESW(GameBase::getPosition(%TrueClientId), %mpos);
						UseSkill(%TrueClientId, $SkillSenseHeading, True, True);
	
						Client::sendMessage(%TrueClientId, 0, Zone::getDesc(%obj) @ " is " @ %d @ " of here.");
					}
					else
						Client::sendMessage(%TrueClientId, 1, "Couldn't fine a zone to match those keywords.");
				}
				else
				{
					Client::sendMessage(%TrueClientId, $MsgWhite, "You can't use #advcompass because you lack the necessary skills.");
					UseSkill(%TrueClientId, $SkillSenseHeading, False, True);
				}
	            }
			return;
		}
		if(%w1 == "#smith")
		{
			if(!%TrueClientId.IsSmithing)
			{
				%tempsmith = LTrim(fetchData(%TrueClientId, "TempSmith"));
				if((%sc = GetSmithCombo(%tempsmith)) != 0)
				{
					%amt = GetWord(%cropped, 0);
					if(%amt <= 0)
						%amt = 1;
					if(%amt > 100)
						%amt = 100;

					%cost = GetSmithComboCost(%TrueClientId, %sc) * %amt;

					if(HasThisStuff(%TrueClientId, %tempsmith, %amt) && !IsDead(%TrueClientId))
					{
						if(%cost <= fetchData(%TrueClientId, "COINS"))
						{
							AI::sayLater(%TrueClientId, %TrueClientId.currentSmith, "Let me see what I can do...", True);
	
							for(%i = 0; (%w = GetWord(%tempsmith, %i)) != -1; %i+=2)
							{
								%w2 = GetWord(%tempsmith, %i+1) * %amt;
								storeData(%TrueClientId, "BankStorage", SetStuffString(fetchData(%TrueClientId, "BankStorage"), %w, %w2));
								Player::decItemCount(%TrueClientId, %w, %w2);
							}
					
							playSound(SoundSmith, GameBase::getPosition(%TrueClientId));
							schedule("CompleteSmith(" @ %TrueClientId @ ", " @ %cost @ ", " @ %sc @ ", \"" @ %tempsmith @ "\", " @ %amt @ ");", 5.5, %TrueClientId);
							%TrueClientId.IsSmithing = True;
							
							return 1;
						}
						else
						{
							Client::sendMessage(%TrueClientId, $MsgRed, "You can't afford to smith this/these items.~wC_BuySell.wav");
							return 0;
						}
					}
				}
			}
		}
		if(%w1 == "#news" || %w1 == "#new" || %w1 == "#updates")
		{
			%c1 = GetWord(%cropped, 0); echo (%c1);
			%c2 = GetWord(%cropped, 1);

			if(%c2 < 1 || %c2 > 60)
				%c2 = 15;
			if(%c1 == -1)
			{
				for(%i = 1; $NewsT[%i] != "" && %i < 5; %i++)
					%nlist = %nlist @ "\n " @ %i @ ": " @ $NewsT[%i];
					centerprint(%TrueClientId, " Type $news X, where X is a number 1-5. For example: #news 3\n" @ %nlist, %c2);
			}
			else
				centerprint(%TrueClientId, "<jc>"@$newsT[%c1] @ "\n" @ $news[%c1], %c2);
		return;
		}

//============================
//ADMIN COMMANDS =============
//============================

		//if(%clientToServerAdminLevel >= 1)
		//{//search "is admin at all"
		if(%w1 == "#gm")
		{
			Client::sendMessage(%TrueClientId, $MsgWhite, "THIS COMMAND HAS BEEN DISCONTINUED, PLEASE USE #ANON");
			return;
			if(%clientToServerAdminLevel >= 4)
			{
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
					Client::sendMessage(%cl, $MsgRed, %cropped);
			}
			return;
		}
		if(%w1 == "#anon")
		{
			if(%clientToServerAdminLevel >= 3)
			{
				%aname = GetWord(%cropped, 0);
				%cn = floor(GetWord(%cropped, 1));
				if(%cn != -1 && %aname != -1)
				{
					%anonmsg = String::NEWgetSubStr(%cropped, String::findSubStr(%cropped, %cn)+String::len(%cn)+1, 99999);
					if(%aname == "all")
					{
						for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
						{
							if(floor(%cl.adminLevel) >= floor(%clientToServerAdminLevel))
								Client::sendMessage(%cl, %cn, "[ANON] " @ %TCsenderName @ ": " @ %anonmsg);
							else
								Client::sendMessage(%cl, %cn, %anonmsg);
						}
					}
					else
					{
						%id = NEWgetClientByName(%aname);
						if(%id != -1)
						{
							if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel))
								Client::sendMessage(%id, %cn, "[ANON] " @ %TCsenderName @ ": " @ %anonmsg);
							else
								Client::sendMessage(%id, %cn, %anonmsg);
						}
		                        else
		                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
					}
				}
				else
					Client::sendMessage(%TrueClientId, $MsgWhite, "Syntax: #anon name/all colorNumber message");
			}
			return;
		}
		if(%w1 == "#fw")
		{
			%c1 = GetWord(%cropped, 0);
	
			if(%c1 != -1)
			{
				%id = NEWgetClientByName(%c1);
	
				if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && %id != %TrueClientId && floor(%id.adminLevel) != 0)
					Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
				{
					if(%clientToServerAdminLevel >= 3)
					{
						%rest = String::getSubStr(%cropped, (String::len(%c1)+1), String::len(%cropped)-(String::len(%c1)+1));
						remoteSay(%id, 0, %rest, %senderName);
	
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Sent a forwarded message to " @ %id @ ".");
					}
				}
				else
					if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Invalid player name, or name is of a superAdmin.");
			}
			else
				Client::sendMessage(%TrueClientId, 0, "Please specify name, command and text.");
	
			return;
		}
		if(%w1 == "#forcespawn")
		{
	            if(%clientToServerAdminLevel >= 1)
			{
	                  if(%cropped == "")
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	                  else
	                  {
	                        %id = NEWgetClientByName(%cropped);
					
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
	                        else if(%id != -1)
	                        {
						if(IsDead(%id))
						{
							Game::playerSpawn(%id, True);
							if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Forced " @ %cropped @ " to spawn.");
						}
						else
							if(!%echoOff) Client::sendMessage(%TrueClientId, 0, %cropped @ " isn't dead.");
					}
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
			}
			return;
		}
		if(%w1 == "#attacklos")
		{
			if(%cropped == "")
				Client::sendMessage(%TrueClientId, 0, "Please specify a bot name.");
			else
			{
				%event = String::findSubStr(%cropped, ">");
				if(%event != -1)
				{
					%info = String::NEWgetSubStr(%cropped, 0, %event);
					%cmd = String::NEWgetSubStr(%cropped, %event, 99999);
				}
				else
					%info	= %cropped;
	
				%c1 = getWord(%info, 0);
				%ox = GetWord(%info, 1);
				%oy = GetWord(%info, 2);
				%oz = GetWord(%info, 3);
				%id = NEWgetClientByName(%c1);
	
				if(%id != -1)
				{
					if(IsInCommaList(fetchData(%TrueClientId, "PersonalPetList"), %id) || %clientToServerAdminLevel >= 1)
					{
						if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && %id != %TrueClientId && floor(%id.adminLevel) != 0)
							Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
						else if(Player::isAiControlled(%id))
						{
							%player = Client::getOwnedObject(%TrueClientId);
	
							if(%ox == -1 && %oy == -1 && %oz == -1)
							{
								GameBase::getLOSinfo(%player, 50000);
								%pos = $los::position;
							}
							else
								%pos = %ox @ " " @ %oy @ " " @ %oz;
	
							if(%event != -1)
								AddEventCommand(%id, %senderName, "onPosCloseEnough " @ %pos, %cmd);
	
							if(!%echoOff) Client::sendMessage(%TrueClientId, 0, %c1 @ " (" @ %id @ ") is attacking position " @ %pos @ ".");
							storeData(%id, "botAttackMode", 3);
							storeData(%id, "tmpbotdata", %pos);
						}
						else
							Client::sendMessage(%TrueClientId, 0, "Player must be a bot.");
					}
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	            }
			return;
		}
		if(%w1 == "#botnormal")
		{
			if(%cropped == "")
				Client::sendMessage(%TrueClientId, 0, "Please specify a bot name.");
			else
			{
				%id = NEWgetClientByName(%cropped);
	
				if(%id != -1)
				{
			            if(IsInCommaList(fetchData(%TrueClientId, "PersonalPetList"), %id) || %clientToServerAdminLevel >= 1)
			            {
						if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && %id != %TrueClientId && floor(%id.adminLevel) != 0)
							Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
						else if(Player::isAiControlled(%id))
		                        {
							if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Bot is now in normal attack mode.");
							storeData(%id, "botAttackMode", 1);
							AI::newDirectiveRemove(fetchData(%id, "BotInfoAiName"), 99);
							storeData(%id, "tmpbotdata", "");
	
							if(fetchData(%id, "petowner") != "")
							{
								storeData(%id, "botAttackMode", 2);
								storeData(%id, "tmpbotdata", fetchData(%id, "petowner"));
							}
						}
						else
							Client::sendMessage(%TrueClientId, 0, "Player must be a bot.");
					}
	                  }
				else
					Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	            }
			return;
		}
		if(%w1 == "#createbotgroup")
		{
			if(%clientToServerAdminLevel >= 1)
			{
				if(%cropped == "")
					Client::sendMessage(%TrueClientId, 0, "Please specify a one-word BotGroup name.");
				else
				{
					if(GetWord(%cropped, 1) == -1)
					{
						%g = GetWord(%cropped, 0);
						%n = AI::CountBotGroupMembers(%g);
						if(!AI::BotGroupExists(%g))
						{
							if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Created BotGroup '" @ %g @ "'.");
							AI::CreateBotGroup(%g);
						}
						else
							Client::sendMessage(%TrueClientId, 0, "BotGroup already exists and contains " @ %n @ " members.  Use #discardbotgroup to delete a BotGroup.");
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Please specify a ONE-WORD BotGroup name.");
				}
			}
			return;
		}
		if(%w1 == "#discardbotgroup")
		{
			if(%clientToServerAdminLevel >= 1)
			{
				if(%cropped == "")
					Client::sendMessage(%TrueClientId, 0, "Please specify a one-word BotGroup name.");
				else
				{
					if(GetWord(%cropped, 1) == -1)
					{
						%g = GetWord(%cropped, 0);
						if(AI::BotGroupExists(%g))
						{
							if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Discarded BotGroup '" @ %g @ "'.");
							AI::DiscardBotGroup(%g);
						}
						else
							Client::sendMessage(%TrueClientId, 0, "BotGroup does not exist.");
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Please specify a ONE-WORD BotGroup name.");
				}
			}
			return;
		}
		if(%w1 == "#getbotgroupleader")
		{
			if(%clientToServerAdminLevel >= 1)
			{
				if(%cropped == "")
					Client::sendMessage(%TrueClientId, 0, "Please specify a one-word BotGroup name.");
				else
				{
					if(GetWord(%cropped, 1) == -1)
					{
						%g = GetWord(%cropped, 0);
						if(AI::BotGroupExists(%g))
						{
							%tl = GetWord($tmpBotGroup[%g], 0);
							%tln = Client::getName(%tl);
							Client::sendMessage(%TrueClientId, 0, "BotGroup leader is " @ %tln @ " (" @ %tl @ ").");
						}
						else
							Client::sendMessage(%TrueClientId, 0, "BotGroup does not exist.");
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Please specify a ONE-WORD BotGroup name.");
				}
			}
			return;
		}
		if(%w1 == "#botgroup")
		{
			if(%clientToServerAdminLevel >= 1)
			{
				%c1 = GetWord(%cropped, 0);
				%c2 = GetWord(%cropped, 1);
	
				if(%c1 != -1 && %c2 != -1)
				{
					%id = NEWgetClientByName(%c1);
					if(%id != -1)
					{
						if(Player::isAiControlled(%id))
						{
							if(AI::BotGroupExists(%c2))
							{
								%b = AI::IsInWhichBotGroup(%id);
								if(%b == -1)
								{
									if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Adding minion " @ %c1 @ " (" @ %id @ ") to BotGroup '" @ %c2 @ "'.");
									AI::AddBotToBotGroup(%id, %c2);
								}
								else
									Client::sendMessage(%TrueClientId, 0, "This bot already belongs to the BotGroup '" @ %b @ "'.  Use #rbotgroup to remove a bot from a BotGroup.");
							}
							else
								Client::sendMessage(%TrueClientId, 0, "BotGroup '" @ %c2 @ "' does not exist.  Use #createbotgroup to create a BotGroup.");
						}
						else
							Client::sendMessage(%TrueClientId, 0, "Name must be a bot.");
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
			}
			return;
		}
		if(%w1 == "#rbotgroup")
		{
			if(%clientToServerAdminLevel >= 1)
			{
				%c1 = GetWord(%cropped, 0);
	
				if(%c1 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	                        if(%id != -1)
	                        {
	                              if(Player::isAiControlled(%id))
	                              {
							%b = AI::IsInWhichBotGroup(%id);
							if(%b != -1)
							{
								if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Removing minion " @ %c1 @ " (" @ %id @ ") from BotGroup '" @ %b @ "'.");
								AI::RemoveBotFromBotGroup(%id, %b);
							}
							else
								Client::sendMessage(%TrueClientId, 0, "This bot does not belong to a BotGroup.");
	                              }
	                              else
	                                    Client::sendMessage(%TrueClientId, 0, "Name must be a bot.");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#listbotgroups")
		{
			if(%clientToServerAdminLevel >= 1)
			{
				Client::sendMessage(%TrueClientId, 0, $BotGroups);
			}
			return;
		}
		if(%w1 == "#sandstorm")
		{
			if(%clientToServerAdminLevel >= 5)
			{
				$IsSandStorming = True;
				SandStorm(0);
			}
			return;
			if(%clientToServerAdminLevel >= 5)
			{
				%c1 = GetWord(%cropped, 0);
				%c2 = GetWord(%cropped, 1);
				setTerrainVisibility(8, %c1, %c2);
				client::sendMessage(%TrueClientId, 0, "\"setTerrainVisibility(8, 800, 900);\", setTerrainVisibility(8, " @ %c1 @ ", " @ %c2 @ "); Terrain default: 9? 800 825");
			}
			return;
		}
		if(%w1 == "#setupai")
		{
			if(%clientToServerAdminLevel >= 5)
			{
				for(%i = 0; (%id = GetWord($TownBotList, %i)) != -1; %i++)
					deleteObject(%id);
				InitTownBots();
			}
			return;
		}
		if(%w1 == "#getadmin")
		{
	            if(%clientToServerAdminLevel >= 1)
			{
	                  if(%cropped == "")
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	                  else
	                  {
	                        %id = NEWgetClientByName(%cropped);
					
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
	                        else if(%id != -1)
	                        {
						%a = floor(%id.adminLevel);
						Client::sendMessage(%TrueClientId, 0, %cropped @ "'s Admin Clearance Level: " @ %a);
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
			}
			return;
		}
		if(%w1 == "#setadmin")
		{
			if(%clientToServerAdminLevel >= 1)
			{
				%c1 = GetWord(%cropped, 0);
				%c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
	                        else if(%id != -1)
	                        {
						%a = floor(%c2);
						if(%a < 0)
							%a = 0;
						if(%a > %clientToServerAdminLevel)
							%a = %clientToServerAdminLevel;
	
						%id.adminLevel = %a;
						Game::refreshClientScore(%id);		//so the ping and PL are shown properly
	
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Changed " @ %c1 @ " (" @ %id @ ") Admin Clearance Level to " @ %id.adminLevel @ ".");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#eyes")
		{
			if(%cropped == "")
				Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
			else
			{
				%id = NEWgetClientByName(%cropped);
	
				if(IsInCommaList(fetchData(%TrueClientId, "PersonalPetList"), %id) || %clientToServerAdminLevel >= 1)
				{
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && %id != %TrueClientId && floor(%id.adminLevel) != 0)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
					{
						if(!IsDead(%id))
						{
							if(%clientToServerAdminLevel >= 1)
							{
								//revert
								Client::setControlObject(%TrueClientId.possessId, %TrueClientId.possessId);
								Client::setControlObject(%TrueClientId, %TrueClientId);
								storeData(%TrueClientId.possessId, "dumbAIflag", "");
								$possessedBy[%TrueClientId.possessId] = "";
			
								//eyes
								Client::setControlObject(%TrueClientId, Client::getObserverCamera(%TrueClientId));
								Observer::setOrbitObject(%TrueClientId, Client::getOwnedObject(%id), -3, -3, -3);
							}
						}
						else
							Client::sendMessage(%TrueClientId, 0, "Target client is dead.");
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
				}
			}
			return;
		}
		if(%w1 == "#possess")
		{
			if(%cropped == "")
				Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
			else
			{
				%id = NEWgetClientByName(%cropped);
	
				if(IsInCommaList(fetchData(%TrueClientId, "PersonalPetList"), %id) || %clientToServerAdminLevel >= 1)
				{
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && %id != %TrueClientId && floor(%id.adminLevel) != 0)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
					{
						if(!IsDead(%id))
						{
							if(%clientToServerAdminLevel >= 4)
							{
								//revert
								Client::setControlObject(%TrueClientId.possessId, %TrueClientId.possessId);
								Client::setControlObject(%TrueClientId, %TrueClientId);
								storeData(%TrueClientId.possessId, "dumbAIflag", "");
								$possessedBy[%TrueClientId.possessId] = "";
		
								//possess
								if(Player::isAiControlled(%id))
								{
									storeData(%id, "dumbAIflag", True);
									AI::setVar(fetchData(%id, "BotInfoAiName"), SpotDist, 0);
									AI::newDirectiveRemove(fetchData(%id, "BotInfoAiName"), 99);
								}
								%TrueClientId.possessId = %id;
								$possessedBy[%id] = %TrueClientId;
								Client::setControlObject(%id, -1);
								Client::setControlObject(%TrueClientId, %id);
							}
						}
						else
							Client::sendMessage(%TrueClientId, 0, "Target client is dead.");
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
				}
			}
			return;
		}
		if(%w1 == "#revert")
		{
			if(%TrueClientId.sleepMode == "")
			{
				Client::setControlObject(%TrueClientId.possessId, %TrueClientId.possessId);
				Client::setControlObject(%TrueClientId, %TrueClientId);
				storeData(%TrueClientId.possessId, "dumbAIflag", "");
				$possessedBy[%TrueClientId.possessId] = "";
			}
			return;
		}
		if(%w1 == "#fixspellflag")
		{
	            if(%clientToServerAdminLevel >= 4)
	            {
	                  if(%cropped == "")
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	                  else
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
	                        else if(%id != -1)
	                        {
						storeData(%id, "SpellCastStep", "");
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Spell flag reset.");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	            }
			return;
		}
		if(%w1 == "#fixbashflag")
		{
	            if(%clientToServerAdminLevel >= 4)
	            {
	                  if(%cropped == "")
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	                  else
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
	                        else if(%id != -1)
	                        {
						storeData(%id, "blockBash", "");
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Bash flag reset.");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	            }
			return;
		}
		if(%w1 == "#kick")
		{
	            if(%clientToServerAdminLevel >= 2)
	            {
	                  %c1 = GetWord(%cropped, 0);
	                  %c2 = GetWord(%cropped, 1);
	                  if(%c2 == -1)
	                        %c2 = False;
	
	                  if(%c1 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                              Admin::Kick(%TrueClientId, %id, %c2);
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
			}
			return;
		}
		if(%w1 == "#kickid")
		{
			if(%clientToServerAdminLevel >= 2)
			{
				%id = GetWord(%cropped, 0);
				%c2 = GetWord(%cropped, 1);
				if(%c2 == -1)
					%c2 = False;
	
				if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
					Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
					Admin::Kick(%TrueClientId, %id, %c2);
	                  else
					Client::sendMessage(%TrueClientId, 0, "Please specify clientId & data.");
			}
			return;
		}
		if(%w1 == "#admin")
		{
			%adlvl = $SanctionedAdmin[%senderName];
			if(%adlvl != "" && %adlvl > 0)
			{
				if(%cropped == "")
					%TrueClientId.adminLevel = %adlvl;
				else if(%cropped <= %adlvl)
					%TrueClientId.adminLevel = (%adlvl = %cropped);
				else
					return;

	                  if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "You have been granted Admin Clearance Level " @ %adlvl @ ".");
				return;
			}
			for(%i = 1; %i <= 8; %i++)
			{
				if(%cropped == $AdminPassword[%i] && $AdminPassword[%i] != "")
		           	{
					%TrueClientId.adminLevel = %i;
	
					if(%TrueClientId.adminLevel >= 4)
			                  ChangeRace(%TrueClientId, "DeathKnight");
					Game::refreshClientScore(%TrueClientId);		//so the ping and PL are shown properly
	
	                  	if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Password accepted for Admin Clearance Level " @ %TrueClientId.adminLevel @ ".");
	
					break;
				}
	            }
			return;
		}
		if(%w1 == "#human")
		{
	            if(%clientToServerAdminLevel >= 4)
	                  ChangeRace(%TrueClientId, "Human");
			return;
		}
		if(%w1 == "#wookiee")
		{
	            if(%clientToServerAdminLevel >= 4)
	                  ChangeRace(%TrueClientId, "Wookiee");
			return;
		}
		if(%w1 == "#dk")
		{
	            if(%clientToServerAdminLevel >= 4)
	                  ChangeRace(%TrueClientId, "DeathKnight");
			return;
		}
		if(%w1 == "#bd")
		{
	            if(%clientToServerAdminLevel >= 4)
	                  ChangeRace(%TrueClientId, "BattleDroid");
			return;
		}
		if(%w1 == "#st")
		{
	            if(%clientToServerAdminLevel >= 4)
	                  ChangeRace(%TrueClientId, "StormTrooper");
			return;
		}
		if(%w1 == "#loadworld")
		{
	            if(%clientToServerAdminLevel >= 4)
	            {
	                  if(%cropped == "")
	                        LoadWorld();
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Do not use parameters for this function call.");
	            }
			return;
		}
		if(%w1 == "#saveworld")
		{
	            if(%clientToServerAdminLevel >= 4)
	            {
	                  if(%cropped == "")
	                        SaveWorld();
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Do not use parameters for this function call.");
	            }
			return;
		}
		if(%w1 == "#loadcharacter" || %w1 == "#loadchar" || %w1 == "#load")
		{
	            if(%clientToServerAdminLevel >= 4)
	            {
	                  if(%cropped == "")
	                        Client::sendMessage(%TrueClientId, 0, "Please specify clientId.");
	                  else
	                        LoadCharacter(%cropped);
	            }
			return;
		}
		if(%w1 == "#exec")
		{
			if(%clientToServerAdminLevel >= 2)
				exec(%cropped);
		return;
		}
		if(%w1 == "#item")
		{
	            if(%clientToServerAdminLevel >= 2)
	            {
				%name = GetWord(%cropped, 0);
	
	                  %id = NEWgetClientByName(%name);
	
				if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
					Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
				{
					Player::setItemCount(%id, GetWord(%cropped, 1), GetWord(%cropped, 2));
					RefreshAll(%id);
					if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Set " @ %name @ " (" @ %id @ ") " @ GetWord(%cropped, 1) @ " count to " @ GetWord(%cropped, 2));
				}
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	            }
			return;
		}
		if(%w1 == "#getitemcount")
		{
	            if(%clientToServerAdminLevel >= 1)
	            {
	                  %id = NEWgetClientByName(GetWord(%cropped, 0));
	
				if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
					Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
				{
	                        %c = Player::getItemCount(%id, GetWord(%cropped, 1));
					Client::sendMessage(%TrueClientId, 0, "Item count for (" @ %id @ ") " @ GetWord(%cropped, 1) @ " is " @ %c);
				}
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	            }
			return;
		}
		if(%w1 == "#myitem")
		{
	            if(%clientToServerAdminLevel >= 2)
	            {
	                  Player::setItemCount(%TrueClientId, GetWord(%cropped, 0), GetWord(%cropped, 1));
				RefreshAll(%TrueClientId);
				if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Set " @ %TCsenderName @ " (" @ %TrueClientId @ ") " @ GetWord(%cropped, 0) @ " count to " @ GetWord(%cropped, 1));
	            }
			return;
		}
		if(%w1 == "#arenacutshort")
		{
	            if(%clientToServerAdminLevel >= 1)
	            {
	                  $IsABotMatch = True;
	                  $ArenaBotMatchTicker = $ArenaBotMatchLengthInTicks;
	            }
			return;
		}
		if(%w1 == "#tackling" || %w1 == "#tackle" || %w1 == "#collisions" || %w1 == "#collision" || %w1 == "#collide")
		{
			if(%clientToServerAdminLevel >= 3)
			{
				if($Tackling)
					$Tackling = 0;
				else
					$Tackling = 1;
			}
			client::sendMessage(%TrueClientId, 0, "Tackling set to: " @ $Tackling);
			return;
		}
		if(%w1 == "#teleport" || %w1 == "#tp")
		{
	            if(%clientToServerAdminLevel >= 2)
	            {
	                  if(%cropped == "")
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	                  else
	                  {
	                        %id = ClientFromShortName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
	                              %player = Client::getOwnedObject(%TrueClientId);
	                              GameBase::getLOSinfo(%player, 50000);
	                              GameBase::setPosition(%id, $los::position);
	
						CheckAndBootFromArena(%id);
	
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Teleporting " @ %cropped @ " (" @ %id @ ") to " @ $los::position @ ".");
					}
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	            }
			return;
		}
		if(%w1 == "#teleport2" || %w1 == "#tp2")
		{
	            if(%clientToServerAdminLevel >= 2)
	            {
	                  %c1 = GetFullName(GetWord(%cropped, 0));
	                  %c2 = GetFullName(GetWord(%cropped, 1));
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id1 = ClientFromName(%c1);
	                        %id2 = ClientFromName(%c2);
	
					if(floor(%id1.adminLevel) >= floor(%clientToServerAdminLevel) && %id1 != %TrueClientId)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
	                        else if(%id1 != -1 && %id2 != -1)
	                        {
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Teleporting " @ %c1 @ " (" @ %id1 @ ") to " @ %c2 @ " (" @ %id2 @ ").");
	                              GameBase::setPosition(%id1, GameBase::getPosition(%id2));
	
						CheckAndBootFromArena(%id1);
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name(s).");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#teleportc" || %w1 == "#tpc")
		{
	            if(%clientToServerAdminLevel >= 2)
	            {
	                  %c1 = GetWord(%cropped, 0);
	                  %c2 = GetWord(%cropped, 1);
	                  %c3 = GetWord(%cropped, 2);
	
	                  if(%c1 != -1 && %c2 != -1 && %c3 != -1)
	                  {
				gamebase::setPosition(client::getOwnedObject(%TrueClientId), %c1 @ " " @ %c2 @ " " @ %c3);
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please use three coordinate numbers.");
	            }
			return;
		}
		if(%w1 == "#follow")
		{
			if(%clientToServerAdminLevel >= 1)
			{
				%c1 = GetWord(%cropped, 0);
				%c2 = GetFullName(GetWord(%cropped, 1));
	
				if(%c1 != -1 && %c2 != -1)
				{
					%id1 = ClientFromShortName(%c1);
					%id2 = ClientFromName(%c2);
	                        if(%id1 != -1 && %id2 != -1)
	                        {
	                              if(Player::isAiControlled(%id1))
	                              {
	                                    if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Making " @ %c1 @ " (" @ %id1 @ ") follow " @ %c2 @ " (" @ %id2 @ ").");
	
							%event = String::findSubStr(%cropped, ">");
							if(%event != -1)
							{
								%cmd = String::NEWgetSubStr(%cropped, %event, 99999);
								AddEventCommand(%id1, %senderName, "onIdCloseEnough " @ %id2, %cmd);
							}
	                                    
							storeData(%id1, "tmpbotdata", %id2);
							storeData(%id1, "botAttackMode", 2);
	                              }
	                              else
	                                    Client::sendMessage(%TrueClientId, 0, "First name must be a bot.");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name(s).");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#cancelfollow")
		{
			if(%clientToServerAdminLevel >= 1)
			{
				if(%cropped != -1)
				{
					%id = NEWgetClientByName(%cropped);
					if(%id != -1)
					{
						if(Player::isAiControlled(%id))
						{
							AI::newDirectiveRemove(fetchData(%id, "BotInfoAiName"), 99);
							if(!%echoOff) Client::sendMessage(%TrueClientId, 0, %cropped @ " (" @ %id @ ") has stopped following its target.");
						}
						else
							Client::sendMessage(%TrueClientId, 0, "Player must be a bot.");
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
			}
			return;
		}
		if(%w1 == "#freeze")
		{
			if(%cropped != -1)
			{
				%id = NEWgetClientByName(%cropped);
				if(%id != -1)
				{
					if(IsInCommaList(fetchData(%TrueClientId, "PersonalPetList"), %id) || %clientToServerAdminLevel >= 1)
					{
						if(Player::isAiControlled(%id))
						{
							if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Freezing " @ %cropped @ " (" @ %id @ ").");
							storeData(%id, "frozen", True);
							AI::setVar(fetchData(%id, "BotInfoAiName"), SpotDist, 0);
							AI::newDirectiveRemove(fetchData(%id, "BotInfoAiName"), 99);
						}
						else
							Client::sendMessage(%TrueClientId, 0, "Name must be a bot.");
					}
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
			}
			else
				Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
			return;
		}
		if(%w1 == "#cancelfreeze" || %w1 == "#unfreeze")
		{
			if(%cropped != -1)
			{
				%id = NEWgetClientByName(%cropped);
				if(%id != -1)
				{
					if(IsInCommaList(fetchData(%TrueClientId, "PersonalPetList"), %id) || %clientToServerAdminLevel >= 1)
					{
						if(Player::isAiControlled(%id))
						{
							if(!%echoOff) Client::sendMessage(%TrueClientId, 0, %cropped @ " (" @ %id @ ") is no longer frozen.");
							storeData(%id, "frozen", "");
							AI::SetSpotDist(%id);
						}
						else
							Client::sendMessage(%TrueClientId, 0, "Player must be a bot.");
					}
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
			}
			else
				Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
			return;
		}
		if(%w1 == "#kill" || %w1 == "#smite")
		{
	            if(%clientToServerAdminLevel >= 2)
	            {
	                  if(%cropped != -1)
	                  {
	                        %id = NEWgetClientByName(%cropped);
		
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
						playNextAnim(%id);
	                              Player::Kill(%id);
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, %cropped @ " (" @ %id @ ") was executed.");
	                        }
	                        else
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
		if(%w1 == "#clearchar" || %w1 == "#clearcharacter")
		{
	            if(%clientToServerAdminLevel >= 5)
	            {
	                  if(%cropped != -1)
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
						playNextAnim(%id);
	                              Player::Kill(%id);
						ResetPlayer(%id);
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, %cropped @ " (" @ %id @ ") profile was RESET.");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			else
			{
				if(%TrueClientId.ClearMe == "")
				{
					Client::sendMessage(%TrueClientId, 1, "Are you sure that you want to clear your character? Remember that you can't undo this. If you really want to reset, type: #clearcharacter #yes");
					%TrueClientId.ClearMe = 5;
	                        }
				else if(%TrueClientId.ClearMe == 5 && %cropped == "#yes")
				{
					playNextAnim(%TrueClientId);
					Player::Kill(%TrueClientId);
					ResetPlayer(%TrueClientId);
					Client::sendMessage(%TrueClientId, 1, "You have cleared your character.");
				}
			}
			return;
		}
		if(%w1 == "#spawn")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
	                  if(%cropped == "")
	                        Client::sendMessage(%TrueClientId, 0, "syntax: #spawn botType displayName loadout [team] [x] [y] [z]");
	                  else
	                  {
					%event = String::findSubStr(%cropped, ">");
					if(%event != -1)
					{
						%info = String::NEWgetSubStr(%cropped, 0, %event);
						%cmd = String::NEWgetSubStr(%cropped, %event, 99999);
					}
					else
						%info	= %cropped;
	
	                        %c1 = GetWord(%info, 0);
	                        %c2 = GetWord(%info, 1);
					%loadout = GetWord(%info, 2);
					%team = GetWord(%info, 3);
					%ox = GetWord(%info, 4);
					%oy = GetWord(%info, 5);
					%oz = GetWord(%info, 6);
	
	                        if(%c1 != -1 && %c2 != -1 && %loadout != -1)
	                        {
						if(NEWgetClientByName(%c2) == -1)
						{
							if(%ox == -1 && %oy == -1 && %oz == -1)
							{
			                              %player = Client::getOwnedObject(%TrueClientId);
			                              GameBase::getLOSinfo(%player, 50000);
								%lospos = $los::position;
							}
							else
								%lospos = %ox @ " " @ %oy @ " " @ %oz;
		
							if(%team == -1) %team = 0;
		                              %n = AI::helper(%c1, %c2, "TempSpawn " @ %lospos @ " " @ %team, %loadout);
		                              %id = AI::getId(%n);
		
							if(%event != -1)
								AddEventCommand(%id, %senderName, "onkill", %cmd);
		
		                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Spawned " @ %n @ " (" @ %id @ ") at " @ %lospos @ ".");
		                        }
						else
		                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, %c2 @ " already exists.");
					}
					else
	                              Client::sendMessage(%TrueClientId, 0, "syntax: #spawn botType displayName loadout [team] [x] [y] [z]");
	                  }
	            }
			return;
		}
		if(%w1 == "#spawnvehicle" || %w1 == "#spawnveh" || %w1 == "#vehicle" || %w1 == "#veh" || %w1 == "#flyer" || %w1 == "#flier" || %w1 == "#scout")
		{
			if(%clientToServerAdminLevel >= 3)
			{
				%vehtype = GetWord(%cropped, 0);
				%team = GetWord(%cropped, 1);
				if(%vehtype == -1) return;
				%player = Client::getOwnedObject(%clientId);
				GameBase::getLOSInfo(%player,1000);
				%spawnRot = vector::add(GameBase::getRotation(%clientId), "0 0 1.57");
				%tc = getNumTeams();

				if(%team > %tc)
				{
					client::sendMessage(%clientId, 0, "Invalid team, please use 0-" @ %tc @ ".");
					return;
				}
				else if(%team == -1)
					%team = GameBase::GetTeam(%clientId);

				%vehicle = newObject("",flier,%vehtype,true);
				Gamebase::setMapName(%vehicle,%vehtype.description);
				addToSet("MissionCleanup", %vehicle);
				GameBase::setTeam(%vehicle,%team);
				GameBase::startFadeIn(%vehicle);
				GameBase::setPosition(%vehicle, $los::position);
				GameBase::setRotation(%vehicle, %spawnrot);

				client::sendMessage(%clientId, 0, "Spawned: " @ %vehtype @ "(team: " @ %team @ ") at " @ $los::position @ ", " @ %spawnrot @ "");
			}
		return;
		}
		if(%w1 == "#fell")
		{
	            if(%clientToServerAdminLevel >= 2)
	            {
	                  if(%cropped == "")
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	                  else
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Processing fell-off-map for " @ %cropped @ " (" @ %id @ ")");
	                              FellOffMap(%id);
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	            }
			return;
		}
		if(%w1 == "#getstorage")
		{
	            if(%clientToServerAdminLevel >= 1)
	            {
	                  if(%cropped == "")
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	                  else
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
	                              Client::sendMessage(%TrueClientId, 0, %id @ ": " @ fetchData(%id, "BankStorage"));
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	            }
			return;
		}
		if(%w1 == "#clearstorage")
		{
	            if(%clientToServerAdminLevel >= 4)
	            {
	                  if(%cropped == "")
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	                  else
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
						storeData(%id, "BankStorage", "");
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, %id @ " bank storage cleared.");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	            }
			return;
		}
		if(%w1 == "#setstorage")
		{
	            if(%clientToServerAdminLevel >= 4)
	            {
				%name = GetWord(%cropped, 0);
	
	                  %id = NEWgetClientByName(%name);
	
				if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
					Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
				{
	                        storeData(%id, "BankStorage", SetStuffString(fetchData(%id, "BankStorage"), GetWord(%cropped, 1), GetWord(%cropped, 2)));
	                        if(!%echoOff) Client::sendMessage(%TrueClientId, 0, %id @ " bank storage modified. Use #getstorage [name] to view.");
				}
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	            }
			return;
		}
	
		if(%w1 == "#addsp")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
	                  %c1 = GetWord(%cropped, 0);
	                  %c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
						storeData(%id, "SPcredits", %c2, "inc");
	                              RefreshAll(%id);
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Setting " @ %c1 @ " (" @ %id @ ") SP credits to " @ fetchData(%id, "SPcredits") @ ".");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#setsp")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
	                  %c1 = GetWord(%cropped, 0);
	                  %c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
						storeData(%id, "SPcredits", %c2);
	                              RefreshAll(%id);
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Setting " @ %c1 @ " (" @ %id @ ") SP credits to " @ fetchData(%id, "SPcredits") @ ".");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#addlck")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
	                  %c1 = GetWord(%cropped, 0);
	                  %c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
						storeData(%id, "LCK", %c2, "inc");
	                              RefreshAll(%id);
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Setting " @ %c1 @ " (" @ %id @ ") base LCK to " @ fetchData(%id, "LCK") @ ".");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#sethp")
		{
	            if(%clientToServerAdminLevel >= 2)
	            {
	                  %c1 = GetWord(%cropped, 0);
	                  %c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
						%max = fetchData(%id, "MaxHP");
						if(%c2 < 1)
							%c2 = 1;
						else if(%c2 > %max)
							%c2 = %max;
	
	                              setHP(%id, %c2);
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Setting " @ %c1 @ " (" @ %id @ ") HP to " @ fetchData(%id, "HP") @ ".");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#setmana")
		{
	            if(%clientToServerAdminLevel >= 2)
	            {
	                  %c1 = GetWord(%cropped, 0);
	                  %c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
	                              %max = fetchData(%id, "MaxMANA");
	                              if(%c2 < 0)
	                                    %c2 = 0;
	                              else if(%c2 > %max)
	                                    %c2 = %max;
	
	                              setMANA(%id, %c2);
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Setting " @ %c1 @ " (" @ %id @ ") MANA to " @ fetchData(%id, "MANA") @ ".");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#addexp")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
	                  %c1 = GetWord(%cropped, 0);
	                  %c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
						storeData(%id, "EXP", %c2, "inc");
						if(Player::isAiControlled(%id))
							HardcodeAIskills(%id);
						Game::refreshClientScore(%id);
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Setting " @ %c1 @ " (" @ %id @ ") EXP to " @ fetchData(%id, "EXP") @ ".");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#setexp")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
	                  %c1 = GetWord(%cropped, 0);
	                  %c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
						storeData(%id, "EXP", %c2);
	                              Game::refreshClientScore(%id);
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Setting " @ %c1 @ " (" @ %id @ ") EXP to " @ fetchData(%id, "EXP") @ ".");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#addcoins" || %w1 == "#addcredits")
		{
	            if(%clientToServerAdminLevel >= 2)
	            {
	                  %c1 = GetWord(%cropped, 0);
	                  %c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
						storeData(%id, "COINS", %c2, "inc");
	                              RefreshAll(%id);
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Setting " @ %c1 @ " (" @ %id @ ") COINS to " @ fetchData(%id, "COINS") @ ".");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#addbank")
		{
	            if(%clientToServerAdminLevel >= 2)
	            {
	                  %c1 = GetWord(%cropped, 0);
	                  %c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
						storeData(%id, "BANK", %c2, "inc");
	                              RefreshAll(%id);
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Setting " @ %c1 @ " (" @ %id @ ") BANK to " @ fetchData(%id, "BANK") @ ".");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#setteam")
		{
	            if(%clientToServerAdminLevel >= 2)
	            {
	                  %c1 = GetWord(%cropped, 0);
	                  %c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
	                              GameBase::setTeam(%id, %c2);
						RefreshAll(%id);
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Setting " @ %c1 @ " (" @ %id @ ") team to " @ GameBase::getTeam(%id) @ ".");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#setrace" || %w1 == "#changerace")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
	                  %c1 = GetWord(%cropped, 0);
	                  %c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
						if(%c2 == "DeathKnight" && %clientToServerAdminLevel >= 4 || %c2 != "DeathKnight")
		                              ChangeRace(%id, %c2, %clientToServerAdminLevel);
	
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Changed " @ %c1 @ " (" @ %id @ ") race to " @ fetchData(%id, "RACE") @ ".");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#changeskin" || %w1 == "#skin")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
	                  %c1 = GetWord(%cropped, 0);
	                  %c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id1 = NEWgetClientByName(%c1);
	                        %id2 = %c2;
	
					if(floor(%id1.adminLevel) >= floor(%clientToServerAdminLevel) && %id1 != %TrueClientId)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
	                        else if(%id1 != -1 && %id2 != -1)
	                        {
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Teleporting " @ %c1 @ " (" @ %id1 @ ") to " @ %c2 @ " (" @ %id2 @ ").");
				      Client::setSkin(%id1, %id2);
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name or skin.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & skin.");
	            }
			return;
		}
		if(%w1 == "#setpassword")
		{
	            if(%clientToServerAdminLevel >= 5)
	            {
	                  %c1 = GetWord(%cropped, 0);
	                  %c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
						storeData(%id, "password", %c2);
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Changed " @ %c1 @ " (" @ %id @ ") password to " @ fetchData(%id, "password") @ ".");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#setinvis")
		{
			if(%clientToServerAdminLevel >= 2)
			{
				%c1 = GetWord(%cropped, 0);
				%c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
	                              if(%c2 == 0)
	                              {
	                                    if(fetchData(%id, "invisible"))
								UnHide(%id);
	                              }
	                              else if(%c2 == 1)
	                              {
	                                    if(!fetchData(%id, "invisible"))
	                                          GameBase::startFadeOut(%id);
							storeData(%id, "invisible", True);
	                              }
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Changed " @ %c1 @ " (" @ %id @ ") invisible state to " @ %c2 @ ".");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#dumbai")
		{
			if(%clientToServerAdminLevel >= 2)
			{
				%c1 = GetWord(%cropped, 0);
				%c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
	                              if(%c2 == 0)
	                                    storeData(%id, "dumbAIflag", "");
	                              else if(%c2 == 1)
	                                    storeData(%id, "dumbAIflag", True);
	
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Changed " @ %c1 @ " (" @ %id @ ") dumb AI flag state to '" @ fetchData(%id, "dumbAIflag") @ "'.");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
	
		if(%w1 == "#getlck")
		{
	            if(%clientToServerAdminLevel >= 1)
	            {
	                  if(%cropped != -1)
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                              Client::sendMessage(%TrueClientId, 0, %cropped @ " (" @ %id @ ") base LCK is " @ fetchData(%id, "LCK") @ ".");
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
		if(%w1 == "#gethp")
		{
	            if(%clientToServerAdminLevel >= 1)
	            {
	                  if(%cropped != "")
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                              Client::sendMessage(%TrueClientId, 0, %cropped @ " (" @ %id @ ") HP is " @ fetchData(%id, "HP") @ ".");
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
		if(%w1 == "#getmana")
		{
	            if(%clientToServerAdminLevel >= 1)
	            {
	                  if(%cropped != "")
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                              Client::sendMessage(%TrueClientId, 0, %cropped @ " (" @ %id @ ") MANA is " @ fetchData(%id, "MANA") @ ".");
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
		if(%w1 == "#getmaxhp")
		{
	            if(%clientToServerAdminLevel >= 1)
	            {
	                  if(%cropped != "")
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                              Client::sendMessage(%TrueClientId, 0, %cropped @ " (" @ %id @ ") max HP is " @ fetchData(%id, "MaxHP") @ ".");
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
		if(%w1 == "#getmaxmana")
		{
	            if(%clientToServerAdminLevel >= 1)
	            {
	                  if(%cropped != "")
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                              Client::sendMessage(%TrueClientId, 0, %cropped @ " (" @ %id @ ") max MANA is " @ fetchData(%id, "MaxMANA") @ ".");
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
		if(%w1 == "#getexp")
		{
	            if(%clientToServerAdminLevel >= 1)
	            {
	                  if(%cropped != -1)
	                  {
	                        %cl = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                              Client::sendMessage(%TrueClientId, 0, %cropped @ " (" @ %cl @ ") EXP is " @ fetchData(%cl, "EXP") @ ".");
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
		if(%w1 == "#getcoins" || %w1 == "#getcredits")
		{
	            if(%clientToServerAdminLevel >= 1)
	            {
	                  if(%cropped != -1)
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                              Client::sendMessage(%TrueClientId, 0, %cropped @ " (" @ %id @ ") COINS is " @ fetchData(%id, "COINS") @ ".");
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
		if(%w1 == "#getbank")
		{
	            if(%clientToServerAdminLevel >= 1)
	            {
	                  if(%cropped != -1)
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                              Client::sendMessage(%TrueClientId, 0, %cropped @ " (" @ %id @ ") BANK is " @ fetchData(%id, "BANK") @ ".");
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
		if(%w1 == "#getteam")
		{
	            if(%clientToServerAdminLevel >= 1)
	            {
	                  if(%cropped != -1)
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                              Client::sendMessage(%TrueClientId, 0, %cropped @ " (" @ %id @ ") team is " @ GameBase::getTeam(%id) @ ".");
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
		if(%w1 == "#getclientid")
		{
	            if(%clientToServerAdminLevel >= 1)
	            {
	                  if(%cropped != -1)
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                              Client::sendMessage(%TrueClientId, 0, %cropped @ " clientId is " @ %id @ ".");
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
		if(%w1 == "#getplayerid")
		{
	            if(%clientToServerAdminLevel >= 1)
	            {
	                  if(%cropped != -1)
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                              Client::sendMessage(%TrueClientId, 0, %cropped @ " clientId is " @ Client::getOwnedObject(%id) @ ".");
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
		if(%w1 == "#getname")
		{
	            if(%clientToServerAdminLevel >= 1)
	            {
	                  if(%cropped != -1)
	                  {
	                        %n = Client::getName(Player::getClient(%cropped));
	
					if(%n != "")
	                              Client::sendMessage(%TrueClientId, 0, %cropped @ " name is " @ %n @ ".");
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid clientId.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify a clientId.");
	            }
			return;
		}
		if(%w1 == "#getpassword")
		{
	            if(%clientToServerAdminLevel >= 5)
	            {
	                  if(%cropped != -1)
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                              Client::sendMessage(%TrueClientId, 0, %cropped @ " password[" @ %id @ "] is " @ fetchData(%id, "password") @ ".");
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
		if(%w1 == "#getotherinfo")
		{
	            if(%clientToServerAdminLevel >= 5)
	            {
	                  if(%cropped != -1)
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                              Client::sendMessage(%TrueClientId, 0, %cropped @ " $Client::info[" @ %id @ ", 5] is " @ $Client::info[%id, 5] @ ".");
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
	
		if(%w1 == "#getlvl")
		{
	            if(%clientToServerAdminLevel >= 1)
	            {
	                  if(%cropped != -1)
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                              Client::sendMessage(%TrueClientId, 0, %cropped @ " (" @ %id @ ") LEVEL is " @ fetchData(%id, "LVL") @ ".");
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
	
		if(%w1 == "#getfinallck")
		{
	            if(%clientToServerAdminLevel >= 1)
	            {
	                  if(%cropped != -1)
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                              Client::sendMessage(%TrueClientId, 0, %cropped @ " (" @ %id @ ") final LCK is " @ fetchData(%id, "LCK") @ ".");
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
		if(%w1 == "#getfinaldef")
		{
	            if(%clientToServerAdminLevel >= 1)
	            {
	                  if(%cropped != -1)
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                              Client::sendMessage(%TrueClientId, 0, %cropped @ " (" @ %id @ ") max DEF roll is " @ fetchData(%id, "DEF") @ ".");
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
		if(%w1 == "#getfinalatk")
		{
	            if(%clientToServerAdminLevel >= 1)
	            {
	                  if(%cropped != -1)
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                              Client::sendMessage(%TrueClientId, 0, %cropped @ " (" @ %id @ ") max ATK roll is " @ fetchData(%id, "ATK") @ ".");
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
		if(%w1 == "#getalignment" || %w1 == "#getalign" || %w1 == "#geta")
		{
	            if(%clientToServerAdminLevel >= 4)
	            {
	                  if(%cropped != -1)
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                              ShowAlignment(%TrueClientId, %id);
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
		if(%w1 == "#exportchat")
		{
	            if(%clientToServerAdminLevel >= 5)
	            {
	                  if(%cropped != "")
				{
					if(%cropped == "0")
						$exportChat = False;
					else if(%cropped == "1")
						$exportChat = True;
	
	                        if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "exportChat set to " @ $exportChat @ ".");
				}
				else
	                        Client::sendMessage(%TrueClientId, 0, "Specify 1 or 0 (1 = True, 0 = False).");
			}
			return;
		}
		if(%w1 == "#doexport")
		{
			if(%clientToServerAdminLevel >= 2)
			{
				%c1 = GetWord(%cropped, 0);
				%c2 = GetWord(%cropped, 1);

	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
	                              if(%c2 == 0)
							%id.doExport = False;
	                              else if(%c2 == 1)
							%id.doExport = True;
	
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Changed " @ %c1 @ " (" @ %id @ ") doExport to " @ %id.doExport @ ".");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#getip")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
	                  if(%cropped != "")
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                              Client::sendMessage(%TrueClientId, 0, %cropped @ " (" @ %id @ ") IP is " @ Client::getTransportAddress(%id));
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
		if(%w1 == "#spawnpack")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
	                  if(%cropped != "")
	                  {
					%event = String::findSubStr(%cropped, ">");
					if(%event != -1)
					{
						%info = String::NEWgetSubStr(%cropped, 0, %event);
						%cmd = String::NEWgetSubStr(%cropped, %event, 99999);
					}
					else
						%info	= %cropped;
	
					%div = String::findSubStr(%info, "|");
	
					if(%div != -1)
					{
						%a = String::NEWgetSubStr(%info, 0, %div-1);
						%tag = GetWord(%a, 0);
						%ox = GetWord(%a, 1);
						%oy = GetWord(%a, 2);
						%oz = GetWord(%a, 3);
						if(%ox == -1 && %oy == -1 && %oz == -1)
						{
							//didn't enter coordinates.
							GameBase::getLOSinfo(Client::getOwnedObject(%TrueClientId), 50000);
							%pos = $los::position;
						}
						else
							%pos = %ox @ " " @ %oy @ " " @ %oz;
	
						if(!IsInCommaList($SpawnPackList, %tag))
						{
							%pack = String::NEWgetSubStr(%info, %div+1, 99999);
							%pid = DeployLootbag(%pos, "0 0 0", %pack);
							$SpawnPackList = AddToCommaList($SpawnPackList, %tag);
							$tagToObjectId[%tag] = %pid;
							%pid.tag = %tag;
		
							if(%event != -1)
								AddEventCommand(%pid, %senderName, "onpickup", %cmd);
		
		                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Spawned pack (" @ %pid @ ") at position " @ %pos @ ".");
						}
						else
							Client::sendMessage(%TrueClientId, 0, "Tagname " @ %tag @ " already exists.");
					}
		                  else
		                        Client::sendMessage(%TrueClientId, 0, "Divider not found. Type #spawnpack with no parameters to get a quick overview.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "#spawnpack tagname [x] [y] [z] | packstring. Use this command only if you know what you're doing.");
	            }
			return;
		}
		if(%w1 == "#delpack")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
				%tag = GetWord(%cropped, 0);
	
	                  if(%cropped != -1)
	                  {
					if($tagToObjectId[%tag] != "")
					{
						%object = $tagToObjectId[%tag];
						ClearEvents(%object);
						deleteObject(%object);
						$tagToObjectId[%tag] = "";
						$SpawnPackList = RemoveFromCommaList($SpawnPackList, %tag);
	
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Deleted " @ %tag @ " (" @ %object @ ")");
					}
					else
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Invalid tagname.");
				}
	                  else
					Client::sendMessage(%TrueClientId, 0, "#delpack tagname.");
	            }
			return;
		}
		if(%w1 == "#spawndis")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
	                  if(%cropped != "")
	                  {
					%f = GetWord(%cropped, 0);
					%tag = GetWord(%cropped, 1);
					%x = GetWord(%cropped, 2);
					%y = GetWord(%cropped, 3);
					%z = GetWord(%cropped, 4);
					%r1 = GetWord(%cropped, 5);
					%r2 = GetWord(%cropped, 6);
					%r3 = GetWord(%cropped, 7);
	
					if(%x == -1 && %y == -1 && %z == -1)
					{
						GameBase::getLOSinfo(Client::getOwnedObject(%TrueClientId), 50000);
						%pos = $los::position;
					}
					else
						%pos = %x @ " " @ %y @ " " @ %z;
	
					if(%r1 == -1 && %r2 == -1 && %r3 == -1)
						%rot = -1;
					else
						%rot = %r1 @ " " @ %r2 @ " " @ %r3;
	
					%fname = %f @ ".dis";
					%object = newObject(%tag, InteriorShape, %fname);
	
					if(%object != 0 && %tag != -1)
					{
						if(IsInCommaList($DISlist, %tag))
						{
							%o = $tagToObjectId[%tag];
							deleteObject(%o);
							$tagToObjectId[%tag] = "";
							%w = "Replaced";
						}
						else
						{
							$DISlist = AddToCommaList($DISlist, %tag);
							%w = "Spawned";
						}
	
						addToSet("MissionCleanup", %object);
						$tagToObjectId[%tag] = %object;
						%object.tag = %tag;
	
						GameBase::setPosition(%object, %pos);
						if(%rot != -1)
							GameBase::setRotation(%object, %rot);
	
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, %w @ " " @ %tag @ " (" @ %object @ ") at pos " @ %pos);
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Invalid DIS filename or tagname.");
				}
	                  else
					Client::sendMessage(%TrueClientId, 0, "#spawndis filename tagname [x] [y] [z] [r1] [r2] [r3]. Do not specify .dis, this will automatically be added.");
	            }
			return;
		}
		if(%w1 == "#deldis")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
				%tag = GetWord(%cropped, 0);
	
	                  if(%cropped != -1)
	                  {
					if($tagToObjectId[%tag] != "")
					{
						%object = $tagToObjectId[%tag];
						ClearEvents(%object);
						deleteObject(%object);
						$tagToObjectId[%tag] = "";
						$DISlist = RemoveFromCommaList($DISlist, %tag);
	
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Deleted " @ %tag @ " (" @ %object @ ")");
					}
					else
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Invalid tagname.");
				}
	                  else
					Client::sendMessage(%TrueClientId, 0, "#deldis tagname.");
	            }
			return;
		}
		if(%w1 == "#listdis")
		{
			if(%clientToServerAdminLevel >= 1)
			{
				Client::sendMessage(%TrueClientId, $MsgBeige, $DISlist);
			}
			return;
		}
		if(%w1 == "#listpacks")
		{
			if(%clientToServerAdminLevel >= 1)
			{
				Client::sendMessage(%TrueClientId, $MsgBeige, $SpawnPackList);
			}
			return;
		}
		if(%w1 == "#deleteobject")
		{
	            if(%clientToServerAdminLevel >= 5)
	            {
			  %c1 = GetWord(%cropped, 0);
	                  if(%c1 != -1)
	                  {
					if(%c1.tag != "")
					{
						$tagToObjectId[%c1.tag] = "";
						if(IsInCommaList($DISlist, %c1.tag))
							$DISlist = RemoveFromCommaList($DISlist, %c1.tag);
						else if(IsInCommaList($SpawnPackList, %c1.tag))
							$SpawnPackList = RemoveFromCommaList($SpawnPackList, %c1.tag);
					}
					deleteObject(%c1);
					ClearEvents(%c1);
	
					if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Attempted to deleteObject(" @ %c1 @ ")");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "#deleteobject [objectId].  Be careful with this command.");
	            }
			return;
		}
		if(%w1 == "#getposition")
		{
			if(%clientToServerAdminLevel >= 1)
			{
				%player = Client::getOwnedObject(%TrueClientId);
				GameBase::getLOSinfo(%player, 50000);
	
				Client::sendMessage(%TrueClientId, 0, "Position at LOS is " @ $los::position);
			}
			return;
		}
		if(%w1 == "#deathmsg")
		{
			if(%clientToServerAdminLevel >= 2)
			{
				%c1 = GetWord(%cropped, 0);
				%msg = String::NEWgetSubStr(%cropped, (String::len(%c1)+1), 99999);
	
				if(%c1 != -1)
				{
					%id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
					{
						storeData(%id, "deathmsg", %msg);
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Changed " @ %c1 @ " (" @ %id @ ") deathmsg to " @ fetchData(%id, "deathmsg"));
					}
					else
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
			}
			else
			{
				storeData(%TrueClientId, "deathmsg", %cropped);
				Client::sendMessage(%TrueClientId, 0, "Changed your death message to: " @ fetchData(%TrueClientId, "deathmsg"));
			}
			return;
		}
		if(%w1 == "#block")
		{
			if(%clientToServerAdminLevel >= 3)
			{
				%bname = GetWord(%cropped, 0);
				if(%bname != -1)
				{
					//Always clear the blockdata
					ClearBlockData(%senderName, %bname);
		
					if(!IsInCommaList($BlockList[%senderName], %bname))
						$BlockList[%senderName] = AddToCommaList($BlockList[%senderName], %bname);
		
					storeData(%TrueClientId, "BlockInputFlag", %bname);
					storeData(%TrueClientId, "tmpBlockCnt", "");
	
					ManageBlockOwnersList(%senderName);
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Incorrect syntax for #block [blockname]");
			}
			return;
		}
		if(%w1 == "#endblock")
		{
			if(%clientToServerAdminLevel >= 3)
			{
				if(fetchData(%TrueClientId, "BlockInputFlag") != "")
				{
					storeData(%TrueClientId, "BlockInputFlag", "");
					storeData(%TrueClientId, "tmpBlockCnt", "");
				}
				else
					Client::sendMessage(%TrueClientId, 0, "No block to end!");
			}
			return;
		}
		if(%w1 == "#delblock")
		{
			if(%clientToServerAdminLevel >= 3)
			{
				%bname = GetWord(%cropped, 0);
				if(%bname != -1)
				{
					if(IsInCommaList($BlockList[%senderName], %bname))
					{
						ClearBlockData(%senderName, %bname);
						$BlockList[%senderName] = RemoveFromCommaList($BlockList[%senderName], %bname);
	
						ManageBlockOwnersList(%senderName);
	
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Block " @ %bname @ " deleted.");
					}
					else
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Block does not exist!");
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Incorrect syntax for #delblock [blockname]");
			}
			return;
		}
		if(%w1 == "#clearblocks")
		{
			if(%clientToServerAdminLevel >= 5)
			{
				%targetName = GetWord(%cropped, 0);
				%id = NEWgetClientByName(%targetName);
			}
			else if(%clientToServerAdminLevel >= 3)
			{
				%targetName = %senderName;
				%id = %TrueClientId;
			}
	
			if(%id != -1)
			{
				if($BlockList[%targetName] != "")
				{
					%list = $BlockList[%targetName];
					$BlockList[%targetName] = "";
					for(%p = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999))
					{
						%w = String::NEWgetSubStr(%list, 0, %p);
						ClearBlockData(%targetName, %w);
					}
					ManageBlockOwnersList(%targetName);
	
					if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Deleted ALL of " @ %targetName @ "'s blocks.");
				}
			}
			else
				Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	
			return;
		}
		if(%w1 == "#clearallblocks")
		{
			if(%clientToServerAdminLevel >= 5)
			{
				%bname = GetWord(%cropped, 0);
				if(%bname == "confirm")
				{
					%blist = $BlockOwnersList;
					for(%bp = String::findSubStr(%blist, ","); (%bp = String::findSubStr(%blist, ",")) != -1; %blist = String::NEWgetSubStr(%blist, %bp+1, 99999))
					{
						%name = String::NEWgetSubStr(%blist, 0, %bp);
	
						if($BlockList[%name] != "")
						{
							%list = $BlockList[%name];
							$BlockList[%name] = "";
							for(%p = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999))
							{
								%w = String::NEWgetSubStr(%list, 0, %p);
								ClearBlockData(%name, %w);
							}
						}
						ManageBlockOwnersList(%name);
					}
					if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Deleted EVERYONE's blocks.");
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Type #clearallblocks confirm to clear EVERYONE's blocks.");
			}
			return;
		}
		if(%w1 == "#listblocks")
		{
			if(%clientToServerAdminLevel >= 5)
			{
				if(%cropped != "")
				{
					if(IsInCommaList($BlockOwnersList, %cropped))
						Client::sendMessage(%TrueClientId, 0, %cropped @ "'s BlockList: " @ $BlockList[%cropped]);
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
			}
			else if(%clientToServerAdminLevel >= 3)
			{
				Client::sendMessage(%TrueClientId, 0, "Your BlockList: " @ $BlockList[%senderName]);
			}
			return;
		}
		if(%w1 == "#echo")
		{
			if(String::ICompare(%cropped, "off") == 0)
				%TrueClientId.echoOff = True;
			else if(String::ICompare(%cropped, "on") == 0)
				%TrueClientId.echoOff = "";
			else
				Client::sendMessage(%TrueClientId, $MsgWhite, %cropped);
	
			return;
		}
		if(%w1 == "#call")
		{
			if(%clientToServerAdminLevel >= 3)
			{
				%bname = GetWord(%cropped, 0);
				
				if(%bname != -1)
				{
					%list = String::NEWgetSubStr(%cropped, (String::len(%bname)+1), 99999);
					for(%p = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999))
						%a[%c++] = String::NEWgetSubStr(%list, 0, %p);
	
					if(%c <= 8)
					{
						if(IsInCommaList($BlockList[%senderName], %bname))
						{
							%TrueClientId.echoOff = True;
		
							for(%i = 1; (%bd = $BlockData[%senderName, %bname, %i]) != ""; %i++)
							{
								if(%a[1] != "")
									%bd = nsprintf(%bd, %a[1], %a[2], %a[3], %a[4], %a[5], %a[6], %a[7], %a[8]);
	
								remoteSay(%clientId, 0, %bd, %senderName);
							}
		
							%TrueClientId.echoOff = "";
						}
						else
							Client::sendMessage(%TrueClientId, 0, "Block does not exist!");
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Too many parameters for #call (max of 8)");
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Incorrect syntax for #call [blockname]");
			}
			return;
		}
		if(%w1 == "#givethisstuff")
		{
			if(%clientToServerAdminLevel >= 2)
			{
				%c1 = GetWord(%cropped, 0);
				%stuff = String::NEWgetSubStr(%cropped, (String::len(%c1)+1), 99999);
	
				if(%c1 != -1)
				{
					%id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
					{
						GiveThisStuff(%id, %stuff, True);
						if(Player::isAiControlled(%id))
							HardcodeAIskills(%id);
							
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Gave " @ %c1 @ " (" @ %id @ "): " @ %stuff);
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
			}
			return;
		}
		if(%w1 == "#takethisstuff")
		{
			if(%clientToServerAdminLevel >= 2)
			{
				%c1 = GetWord(%cropped, 0);
				%stuff = String::NEWgetSubStr(%cropped, (String::len(%c1)+1), 99999);
	
				if(%c1 != -1)
				{
					%id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
					{
						if(HasThisStuff(%id, %stuff))
						{
							TakeThisStuff(%id, %stuff);
							if(Player::isAiControlled(%id))
								HardcodeAIskills(%id);
							RefreshAll(%id);
	
							if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Took " @ %c1 @ " (" @ %id @ "): " @ %stuff);
						}
						else
							if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Could not take stuff.");
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
			}
			return;
		}
		if(%w1 == "#refreshbotskills")
		{
	            if(%clientToServerAdminLevel >= 2)
	            {
	                  if(%cropped == "")
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	                  else
	                  {
	                        %id = NEWgetClientByName(%cropped);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
						HardcodeAIskills(%id);
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Refreshed skills for " @ %cropped @ " (" @ %id @ ").");
					}
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	            }
			return;
		}
		if(%w1 == "#listblockowners")
		{
			if(%clientToServerAdminLevel >= 5)
			{
				Client::sendMessage(%TrueClientId, $MsgBeige, $BlockOwnersList);
			}
			return;
		}
		if(%w1 == "#nodroppack")
		{
			if(%clientToServerAdminLevel >= 2)
			{
				%c1 = GetWord(%cropped, 0);
				%c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
	                              if(%c2 == 0)
	                                    storeData(%id, "noDropLootbagFlag", "");
	                              else if(%c2 == 1)
	                                    storeData(%id, "noDropLootbagFlag", True);
	
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Changed " @ %c1 @ " (" @ %id @ ") noDropLootbagFlag to '" @ fetchData(%id, "noDropLootbagFlag") @ "'.");
	                        }
	                        else
						Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#playsound")
		{
			if(%clientToServerAdminLevel >= 2)
			{
				%c1 = GetWord(%cropped, 0);
				%pos = String::NEWgetSubStr(%cropped, (String::len(%c1)+1), 99999);
	
				if(%c1 != -1)
				{
					if(GetWord(%pos, 0) == -1)
					{
						if(GameBase::getLOSinfo(Client::getOwnedObject(%TrueClientId), 50000))
							%pos = $los::position;
						else
							%pos = GameBase::getPosition(%TrueClientId);
					}
					playSound(%c1, %pos);
	
					if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Playing sound " @ %c1 @ " at pos " @ %pos);
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Please specify nsound & position.");
			}
			return;
		}
		if(%w1 == "#delbot")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
	                  if(%cropped != -1)
	                  {
	                        %id = NEWgetClientByName(%cropped);
		
					if(%id != -1)
	                        {
						if(Player::isAiControlled(%id))
						{
							storeData(%id, "noDropLootbagFlag", True);
							ClearEvents(%id);
							Player::Kill(%id);
							if(!%echoOff) Client::sendMessage(%TrueClientId, 0, %cropped @ " (" @ %id @ ") was deleted.");
						}
						else
	                              	Client::sendMessage(%TrueClientId, 0, "This command only works on bots.");
	                        }
	                        else
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
	            }
			return;
		}
		if(%w1 == "#clearbots" || %w1 == "#clearai")
		{
			if(%clientToServerAdminLevel >= 1)
			{
				%list = GetBotIdList();

				if(%list != "")
	                        {
					for(%i = 0; (%id = getword(%list, %i)) != -1; %i++)
					{
						storeData(%id, "noDropLootbagFlag", True);
						ClearEvents(%id);
						Player::Kill(%id);
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, client::getName(%id) @ " (" @ %id @ ") was deleted.");
					}
	                        }
	                        else
	                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "No bots to kill!");
			}
			return;
		}
		if(%w1 == "#clearvehicles" || %w1 == "#clearflyers")
		{
			if(%clientToServerAdminLevel >= 1)
			{
	%list = "";

	%tempSet = nameToID("MissionCleanup");
	if(%tempSet != -1)
	{
		%num = Group::objectCount(%tempSet);
		for(%i = 0; %i <= %num-1; %i++)
		{
			%tempItem = Group::getObject(%tempSet, %i);
			messageall(0, getObjectType(%tempItem));
			if(getObjectType(%tempItem) == "Flyer" || getObjectType(%tempItem) == "Vehicle")
				GameBase::setDamageLevel(%tempItem, GameBase::getDataName(%tempItem).maxDamage);
		}
	}
			}
			return;
		}
		if(%w1 == "#loadout")
		{
			if(%clientToServerAdminLevel >= 3)
			{
				%c1 = GetWord(%cropped, 0);
				%stuff = String::NEWgetSubStr(%cropped, (String::len(%c1)+1), 99999);
	
				if(%c1 != -1)
				{
					if(!IsInCommaList($LoadOutList, %c1))
					{
						$LoadOutList = AddToCommaList($LoadOutList, %c1);
						$LoadOut[%c1] = %stuff;
	
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Loadout " @ %c1 @ " defined.");
					}
					else
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Loadout tagname already exists.");
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Please specify tagname & data.");
			}
			return;
		}
		if(%w1 == "#delloadout")
		{
			if(%clientToServerAdminLevel >= 3)
			{
				%c1 = GetWord(%cropped, 0);
	
				if(%c1 != -1)
				{
					if(IsInCommaList($LoadOutList, %c1))
					{
						$LoadOutList = RemoveFromCommaList($LoadOutList, %c1);
						$LoadOut[%c1] = "";
	
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Loadout " @ %c1 @ " deleted.");
					}
					else
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Loadout tagname does not exist.");
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Please specify tagname.");
			}
			return;
		}
		if(%w1 == "#clearloadouts")
		{
			if(%clientToServerAdminLevel >= 4)
			{
				%list = $LoadOutList;
				$LoadOutList = "";
				for(%p = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999))
				{
					%w = String::NEWgetSubStr(%list, 0, %p);
					$LoadOut[%w] = "";
				}
	
				if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Deleted ALL loadouts.");
			}
			return;
		}
		if(%w1 == "#showloadout")
		{
			if(%clientToServerAdminLevel >= 3)
			{
				%c1 = GetWord(%cropped, 0);
	
				if(%c1 != -1)
				{
					if(IsInCommaList($LoadOutList, %c1))
						Client::sendMessage(%TrueClientId, 0, %c1 @ ": " @ $LoadOut[%c1]);
					else
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Loadout tagname does not exist.");
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Please specify tagname.");
			}
			return;
		}
		if(%w1 == "#listloadouts")
		{
			if(%clientToServerAdminLevel >= 3)
			{
				%list = $LoadOutList;
				for(%p = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999))
				{
					%w = String::NEWgetSubStr(%list, 0, %p);
					Client::sendMessage(%TrueClientId, 0, %w @ ": " @ $LoadOut[%w]);
				}
			}
			return;
		}
		if(%w1 == "#nobotsniff")
		{
			if(%clientToServerAdminLevel >= 3)
			{
				%c1 = GetWord(%cropped, 0);
				%c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
	                        else if(%id != -1)
	                        {
						if(%c2 == 0)
							storeData(%id, "noBotSniff", "");
						else if(%c2 == 1)
							storeData(%id, "noBotSniff", True);
	
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Changed " @ %c1 @ " (" @ %id @ ") noBotSniff flag to " @ %c2 @ ".");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#addrankpoints")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
	                  %c1 = GetWord(%cropped, 0);
	                  %c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
						storeData(%id, "RankPoints", %c2, "inc");
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Setting " @ %c1 @ " (" @ %id @ ") RankPoints to " @ fetchData(%id, "RankPoints") @ ".");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
	            }
			return;
		}
		if(%w1 == "#sethouse" || %w1 == "#setguild" || %w1 == "#setfaction")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
	                  %c1 = GetWord(%cropped, 0);
				%c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
						%hn = "";
						if(String::ICompare(%c2, "null") == 0)
							%hn = 0;
						else
						{
							for(%i = 1; $HouseName[%i] != ""; %i++)
							{
								if(String::findSubStr($HouseName[%i], %c2) != -1)
									%hn = %i;
							}
						}
	
						if(%hn != "")
						{
							%hname = $HouseName[%hn];
							storeData(%id, "MyHouse", %hname);
							if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Setting " @ %c1 @ " (" @ %id @ ") Faction to " @ fetchData(%id, "MyHouse") @ ".");
						}
						else
		                              Client::sendMessage(%TrueClientId, 0, "Invalid Faction.");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & house (to clear house, use: #sethouse name NULL).");
	            }
			return;
		}
		if(%w1 == "#leavehouse" || %w1 == "#leaveguild" || %w1 == "#leavefaction")
		{
			BootFromCurrentHouse(%clientId, True);
		}
		if(%w1 == "#setspawnmultiplier")
		{
			if(%clientToServerAdminLevel >= 5)
			{
				%c1 = GetWord(%cropped, 0);
	
				if(%c1 != -1)
				{
					$spawnMultiplier = Cap(%c1, 0, "inf");
					if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "spawnMultiplier set to " @ $spawnMultiplier @ ".");
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Please specify a number (normal should be 1. 0 will cease spawning.)");
			}
			return;
		}
		if(%w1 == "#jail")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
	                  %c1 = GetWord(%cropped, 0);
	                  %c2 = GetWord(%cropped, 1);
				%c3 = GetWord(%cropped, 2);
	
	                  if(%c1 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
	                        {
						%c1 = Client::getName(%id);
						if(%c2 == -1)
							%c2 = 300;
						if(%c3 == -1)
							%c3 = GetRandomJailNumber();
	
						%pos = GetPositionForJailNumber(%c3);
						if(%pos != -1)
						{
							Jail(%id, %c2, %c3);
							if(!%echoOff) Client::sendMessage(%TrueClientId, 0, %c1 @ " has been jailed for " @ %c2 @ " seconds in Jail #" @ %c3 @ ".");
						}
						else
		                              Client::sendMessage(%TrueClientId, 0, "Invalid jail number.");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name, time, and jail number.");
	            }
			return;
		}
		if(%w1 == "#beg")
		{
	            if(%clientToServerAdminLevel >= 2)
	            {
	                  %c1 = GetWord(%cropped, 0);
	                  %c2 = GetWord(%cropped, 1);
	                  if(%c2 == -1)
	                        %c2 = False;
	
	                  if(%c1 != -1)
	                  {
	                        %id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
					{
						%ip = Client::getTransportAddress(%id);
						BanList::add(%ip, 300);
	                              Net::kick(%id, "Do not beg from an admin! The next time you might be banned, so quit your begging.");
					}
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
			}
			return;
		}
		if(%w1 == "#onhear")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
	                  if(%cropped != "")
	                  {
					%event = String::findSubStr(%cropped, ">");
					if(%event != -1)
					{
						%info = String::NEWgetSubStr(%cropped, 0, %event);
						%cmd = String::NEWgetSubStr(%cropped, %event, 99999);
					}
					else
						%info	= %cropped;

					%var = GetWord(%info, 4);
					if(String::ICompare(%var, "var") == 0)
						%var = "var";
					else
					{
						%var = "";
						%quote1 = String::findSubStr(%info, "\"");
						%quote2 = String::ofindSubStr(%info, "\"", %quote1+1);
					}
					if(%quote1 != -1 && %quote2 != -1 || %var != "")
					{
						%pname = GetWord(%info, 0);
						%id = NEWgetClientByName(%pname);

						if(%id != -1)
						{
							%pname = Client::getName(%id);	//properly capitalize name
							%radius = GetWord(%info, 1);
							%keep = GetWord(%info, 2);

							if(%keep == "true" || %keep == "false")
							{
								%targetname = GetWord(%info, 3);
								%tid = NEWgetClientByName(%targetname);
								if(String::ICompare(%targetname, "all") == 0 || %tid != -1)
								{
									if(%var != "")
									{
										%vtxt = %var;
										%text = "var";
									}
									else
									{
										%text = String::NEWgetSubStr(%info, %quote1+1, %quote2);
										%vtxt = "|" @ %text @ "|";
									}

									if(%text != "")
									{
										if(%event != -1)
										{
											AddEventCommand(%id, %senderName, "onHear " @ %pname @ " " @ %radius @ " " @ %keep @ " " @ %targetname @ " " @ %vtxt, %cmd);
											if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "onHear event set for " @ %pname @ "(" @ %id @ ") with text: \"" @ %text @ "\"");
										}
										else
											Client::sendMessage(%TrueClientId, 0, "onHear event definition failed.");
									}
									else
										Client::sendMessage(%TrueClientId, 0, "Invalid text.");
								}
								else
									Client::sendMessage(%TrueClientId, 0, "Invalid name. Please specify 'all' or target's name.");
							}
							else
								Client::sendMessage(%TrueClientId, 0, "Specify 'true' or 'false'. 'true' means that the onHear event won't be deleted after use. 'false' is recommended to keep things clean.");
						}
						else
							Client::sendMessage(%TrueClientId, 0, "Invalid name.");
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Quotes for text not found.");
				}
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "#onhear name radius keep all/targetname \"text\"/var.");
	            }
			return;
		}
			if(%w1 == "#if")
			{
	            if(%clientToServerAdminLevel >= 7)
	            {
	                  if(%cropped != "")
	                  {
					%info	= %cropped;

					%para1 = String::findSubStr(%info, "{");
					%para2 = String::ofindSubStr(%info, "}", %para1+1);
					if(%para1 != -1 && %para2 != -1)
					{
						%expression = String::NEWgetSubStr(%info, %para1+1, %para2);
						if((%pw = CheckForProtectedWords(%expression)) == "")
						{
							%command = String::NEWgetSubStr(%info, %para1+%para2+3, 99999);
							%retval = eval("%x = (" @ %expression @ ");");

							if(%retval == 0)
								%r = false;
							else
								%r = true;
		                              if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "(" @ %expression @ ") = " @ %r);
	
							if(%retval && %command != "")
								remoteSay(%clientId, 0, %command, %senderName);
						}
						else
							Client::sendMessage(%TrueClientId, 0, "Protected word '" @ %pw @ "' can't be used in the #if statement.");
					}
					else
						Client::sendMessage(%TrueClientId, 0, "{ and } found.");
				}
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "#if {expression} command");
	            }
			return;
		}
		if(%w1 == "#addskill")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
				%name = GetWord(%cropped, 0);
	
	                  %id = NEWgetClientByName(%name);
	
				if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
					Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
				{
					%sid = GetWord(%cropped, 1);
					if($SkillDesc[%sid] != "")
					{
						%sn = floor(GetWord(%cropped, 2));
						if(%sn != 0)
						{
							$PlayerSkill[%id, %sid] += %sn;
							RefreshAll(%id);
							if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Set " @ %name @ " (" @ %id @ ") " @ $SkillDesc[%sid] @ " to " @ $PlayerSkill[%id, %sid]);
						}
					}
				}
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	            }
			return;
		}
		if(%w1 == "#setvelocity")
		{
	            if(%clientToServerAdminLevel >= 2)
	            {
				%name = GetWord(%cropped, 0);
	
	                  %id = NEWgetClientByName(%name);
	
				if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
					Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
				{
					%max = 5000;
					%x = Cap(floor(GetWord(%cropped, 1)), -%max, %max);
					%y = Cap(floor(GetWord(%cropped, 2)), -%max, %max);
					%z = Cap(floor(GetWord(%cropped, 3)), -%max, %max);

					%vel = %x @ " " @ %y @ " " @ %z;
					Item::setVelocity(%id, %vel);

					if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Set " @ %name @ " (" @ %id @ ") velocity to " @ %vel);
				}
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	            }
			return;
		}
		if(%w1 == "#getskill")
		{
	            if(%clientToServerAdminLevel >= 2)
	            {
				%name = GetWord(%cropped, 0);
	
	                  %id = NEWgetClientByName(%name);
	
				if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
					Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
				{
					%sid = GetWord(%cropped, 1);
					if($SkillDesc[%sid] != "")
						Client::sendMessage(%TrueClientId, 0, %name @ " (" @ %id @ ") " @ $SkillDesc[%sid] @ " is " @ $PlayerSkill[%id, %sid]);
				}
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	            }
			return;
		}
		if(%w1 == "#scheduleblock")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
				%bname = GetWord(%cropped, 0);
				if(%bname != -1)
				{
					if(IsInCommaList($BlockList[%senderName], %bname))
					{
						%delay = GetWord(%cropped, 1);
						if(%delay >= 0.05)
						{
							%repeat = floor(GetWord(%cropped, 2));
							if(%repeat >= 0)
							{
								%rp = (%repeat+1);

								%arglist = String::NEWgetSubStr(%cropped, (String::len(%bname @ %delay @ %repeat @ "  ")+1), 99999);
								if(GetWord(%arglist, 0) != -1)
									%txt = "#call " @ %bname @ " " @ %arglist;
								else
									%txt = "#call " @ %bname;

								for(%sbi = 1; %sbi <= %rp; %sbi++)
									schedule("remoteSay(" @ %clientId @ ", 0, \"" @ %txt @ "\", \"" @ %senderName @ "\");", %delay * %sbi);
								if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Block " @ %bname @ " scheduled for " @ %repeat @ " repeats at " @ %delay @ " second intervals.");
							}
							else
								Client::sendMessage(%TrueClientId, 0, "Schedule repeat too low, minimum is 0");
						}
						else
							Client::sendMessage(%TrueClientId, 0, "Schedule delay too low, minimum is 0.05");
					}
					else
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Block does not exist!");
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Incorrect syntax for #scheduleblock blockName delay numRepeat");
	            }
			return;
		}
		if(%w1 == "#listonhear")
		{
			if(%clientToServerAdminLevel >= 3)
			{
				if(%cropped != "")
				{
					%id = NEWgetClientByName(%cropped);

					if(%id != -1)
					{
						%index = GetEventCommandIndex(%id, "onHear");
						if(%index != -1)
						{
							for(%i2 = 0; (%index2 = GetWord(%index, %i2)) != -1; %i2++)
								Client::sendMessage(%TrueClientId, 0, Client::getName(%id) @ " onHear " @ %index2 @ ": " @ $EventCommand[%id, %index2]);
						}
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Please specify player name.");
			}
			return;
		}
		if(%w1 == "#clearonhear")
		{
			if(%clientToServerAdminLevel >= 3)
			{
				%name = GetWord(%cropped, 0);
				%oindex = GetWord(%cropped, 1);

				if(%name != -1)
				{
					%id = NEWgetClientByName(%name);

					if(%id != -1)
					{
						%index = GetEventCommandIndex(%id, "onHear");
						if(%index != -1)
						{
							for(%i2 = 0; (%index2 = GetWord(%index, %i2)) != -1; %i2++)
							{
								if(floor(%index2) == floor(%oindex) || %oindex == -1)
								{
									$EventCommand[%id, %index2] = "";
									if(!%echoOff) Client::sendMessage(%TrueClientId, 0, Client::getName(%id) @ " onHear " @ %index2 @ " cleared.");
								}
							}
						}
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Incorrect syntax for #clearonhear name [index]. If index is missing or -1, all onHears for name are cleared.");
			}
			return;
		}
		if(%w1 == "#getvelocity")
		{
	            if(%clientToServerAdminLevel >= 2)
	            {
				%name = GetWord(%cropped, 0);
	
	                  %id = NEWgetClientByName(%name);
	
				if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
					Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
				{
					%vel = Item::getVelocity(%id);
					Client::sendMessage(%TrueClientId, 0, %name @ " (" @ %id @ ") velocity: " @ %vel);
				}
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	            }
			return;
		}
		if(%w1 == "#onconsider")
		{
	            if(%clientToServerAdminLevel >= 3)
	            {
	                  if(%cropped != "")
	                  {
					%event = String::findSubStr(%cropped, ">");
					if(%event != -1)
					{
						%info = String::NEWgetSubStr(%cropped, 0, %event);
						%cmd = String::NEWgetSubStr(%cropped, %event, 99999);
					}
					else
						%info	= %cropped;

					%tag = GetWord(%info, 0);
					%object = $tagToObjectId[%tag];

					if(%object != "")
					{
						%radius = GetWord(%info, 1);
						%keep = GetWord(%info, 2);

						if(%keep == "true" || %keep == "false")
						{
							%targetname = GetWord(%info, 3);
							%tid = NEWgetClientByName(%targetname);
							if(String::ICompare(%targetname, "all") == 0 || %tid != -1)
							{
								if(%event != -1)
								{
									AddEventCommand(%tag, %senderName, "onConsider " @ %radius @ " " @ %keep @ " " @ %targetname, %cmd);
									if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "onConsider event set for tagname " @ %tag @ "(" @ %object @ ") for radius " @ %radius);
								}
								else
									Client::sendMessage(%TrueClientId, 0, "onConsider event definition failed.");
							}
							else
								Client::sendMessage(%TrueClientId, 0, "Invalid name. Please specify 'all' or target's name.");
						}
						else
							Client::sendMessage(%TrueClientId, 0, "Specify 'true' or 'false'. 'true' means that the onConsider event won't be deleted after use.");
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Invalid tagname.");
				}
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "#onconsider tagname radius keep all/targetname");
	            }
			return;
		}
		if(%w1 == "#listonconsider")
		{
			if(%clientToServerAdminLevel >= 3)
			{
				if(%cropped != "")
				{
					%tag = GetWord(%cropped, 0);
					%object = $tagToObjectId[%tag];

					if(%object != "")
					{
						%index = GetEventCommandIndex(%object, "onConsider");
						if(%index != -1)
						{
							for(%i2 = 0; (%index2 = GetWord(%index, %i2)) != -1; %i2++)
								Client::sendMessage(%TrueClientId, 0, %tag @ " (" @ %object @ ") onConsider " @ %index2 @ ": " @ $EventCommand[%object, %index2]);
						}
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Invalid tagname.");
				}
				else
				{
					%list = $DISlist;
					for(%p = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999))
					{
						%w = String::NEWgetSubStr(%list, 0, %p);
						%object = $tagToObjectId[%w];

						%index = GetEventCommandIndex(%object, "onConsider");
						if(%index != -1)
							Client::sendMessage(%TrueClientId, 0, %w @ ": " @ %index);
					}
				}
			}
			return;
		}
		if(%w1 == "#clearonconsider")
		{
			if(%clientToServerAdminLevel >= 3)
			{
				%tag = GetWord(%cropped, 0);
				%object = $tagToObjectId[%tag];

				if(%object != "")
				{
					%oindex = GetWord(%cropped, 1);

					%index = GetEventCommandIndex(%object, "onConsider");
					if(%index != -1)
					{
						for(%i2 = 0; (%index2 = GetWord(%index, %i2)) != -1; %i2++)
						{
							if(floor(%index2) == floor(%oindex) || %oindex == -1)
							{
								$EventCommand[%object.tag, %index2] = "";
								if(!%echoOff) Client::sendMessage(%TrueClientId, 0, %tag @ " (" @ %object @ ") onConsider " @ %index2 @ " cleared.");
							}
						}
					}
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Incorrect tagname for #clearonconsider tagname [index]. If index is missing or -1, all onConsiders for name are cleared.");
			}

			return;
		}
		if(%w1 == "#exp")
		{
			if(%clientToServerAdminLevel >= 3)
			{
				%c1 = GetWord(%cropped, 0);
				%c2 = GetWord(%cropped, 1);
	
				if(%c1 != -1 && %c2 != -1)
				{
					%id = NEWgetClientByName(%c1);
	
					if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel) && Client::getName(%id) != %senderName)
						Client::sendMessage(%TrueClientId, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1)
					{
						storeData(%id, "EXP", %c2, "inc");
						HardcodeAIskills(%id);
						Game::refreshClientScore(%id);
						if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Setting " @ %c1 @ " (" @ %id @ ") EXP to " @ fetchData(%id, "EXP") @ ".");
					}
					else
						Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
				}
				else
					Client::sendMessage(%TrueClientId, 0, "Please specify player name & data.");
			}
			return;
		}
	}
	//}//previous check to see if is admin at all.
	
	//========== BOT TALK ======================================================================================

	if(%botTalk)
	{
		//process TownBot talk

		%initTalk = "";
		for(%i = 0; (%w = GetWord("hail hello hi greetings yo hey sup salutations g'day goodday howdy boo hai hei bonjour salut heil", %i)) != -1; %i++)
			if(String::ICompare(%cropped, %w) == 0)
				%initTalk = True;

		%clientPos = GameBase::getPosition(%TrueClientId);
		%closest = 5000000;

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

		%aiName = %closestId.name;
		%displayName = $BotInfo[%aiName, NAME];
		
		if(%closest <= ($maxAIdistVec + ($PlayerSkill[%TrueClientId, $SkillCharisma] / 50)))// && Client::getTeam(%TrueClientId) == GameBase::getTeam(%closestId))
		{
			//%z = getword(vector::getrotation(vector::sub(gamebase::getposition(%TrueClientId), gamebase::getposition(%closestId))), 2);
			GameBase::SetRotation(%closestId, "0 0 " @ getword(vector::getrotation(vector::sub(gamebase::getposition(%TrueClientId), gamebase::getposition(%closestId))), 2)); //Thanks GetNESW function..
			//GameBase::playSequence(%closestId, 50, "wave");
			if(clipTrailingNumbers(%aiName) == "merchant")
			{
				//process merchant code
				%trigger[2] = "buy";
				if($state[%closestId, %TrueClientId] == "")
				{
					if(%initTalk)
					{
						AI::sayLater(%TrueClientId, %closestId, "Did you come to see what items you can BUY?", True);
						$state[%closestId, %TrueClientId] = 1;
					}
				}
				else if($state[%closestId, %TrueClientId] == 1)
				{
					if(String::findSubStr(%message, %trigger[2]) != -1)
					{
						SetupShop(%TrueClientId, %closestId);

						AI::sayLater(%TrueClientId, %closestId, "Take a look at what I have.", True);
						$state[%closestId, %TrueClientId] = "";
					}
				}
			}
			else if(clipTrailingNumbers(%aiName) == "banker")
			{
				//process banker code
				%trigger[2] = "deposit";
				%trigger[3] = "withdraw";
				%trigger[4] = "storage";
				if($state[%closestId, %TrueClientId] == "")
				{
					if(%initTalk)
					{
						AI::sayLater(%TrueClientId, %closestId, "I can keep your money from being stolen by thieves.  DEPOSIT, WITHDRAW or look at your STORAGE?  You are carrying " @ fetchData(%TrueClientId, "COINS") @ " credits and I have " @ fetchData(%TrueClientId, "BANK") @ " of yours.", True);
						$state[%closestId, %TrueClientId] = 1;
					}
				}
				if($state[%closestId, %TrueClientId] == 1)
				{
					if(String::findSubStr(%message, %trigger[2]) != -1)
					{
						//deposit question
						AI::sayLater(%TrueClientId, %closestId, "How much do you want me to hold?  You are carrying " @ fetchData(%TrueClientId, "COINS") @ " credits and I have " @ fetchData(%TrueClientId, "BANK") @ " of yours. (AMOUNT/ALL)", True);
						$state[%closestId, %TrueClientId] = 2;
					}
					if(String::findSubStr(%message, %trigger[3]) != -1)
					{
						//withdraw question
						AI::sayLater(%TrueClientId, %closestId, "How much do you want to take out?  You are carrying " @ fetchData(%TrueClientId, "COINS") @ " credits and I have " @ fetchData(%TrueClientId, "BANK") @ " of yours. (AMOUNT/ALL)", True);
						$state[%closestId, %TrueClientId] = 3;
					}
					if(String::findSubStr(%message, %trigger[4]) != -1)
					{
						//storage
						AI::sayLater(%TrueClientId, %closestId, "This is the equipment you have stored here.", True);

						SetupBank(%TrueClientId, %closestId);

						$state[%closestId, %TrueClientId] = "";
					}
				}
				else if($state[%closestId, %TrueClientId] == 2)
				{
					//deposit
					if(%cropped == "all")
						%cropped = fetchData(%TrueClientId, "COINS");
		
					%c = floor(%cropped);
					if(%c <= 0)
					{
						AI::sayLater(%TrueClientId, %closestId, "Invalid request.  Your transaction has been cancelled.~wError_Message.wav", True);
					}
					else if(%c <= fetchData(%TrueClientId, "COINS"))
					{
						storeData(%TrueClientId, "BANK", %c, "inc");
						storeData(%TrueClientId, "COINS", %c, "dec");
						RefreshAll(%TrueClientId);
						AI::sayLater(%TrueClientId, %closestId, "You have given me " @ %c @ " credits.  You are now carrying " @ fetchData(%TrueClientId, "COINS") @ " credits and I have " @ fetchData(%TrueClientId, "BANK") @ " of yours.  Have a nice day.", True);

						playSound(SoundMoney1, GameBase::getPosition(%closestId));
					}
					else
					{
						AI::sayLater(%TrueClientId, %closestId, "Sorry, you don't seem to have that many credits.  Your transaction has been cancelled.", True);
					}
					$state[%closestId, %TrueClientId] = "";
				}
				else if($state[%closestId, %TrueClientId] == 3)
				{
					//withdraw
					if(%cropped == "all")
						%cropped = fetchData(%TrueClientId, "BANK");

					%c = floor(%cropped);
					if(%c <= 0)
					{
						AI::sayLater(%TrueClientId, %closestId, "Invalid request.  Your transaction has been cancelled.~wError_Message.wav", True);
					}
					else if(%c <= fetchData(%TrueClientId, "BANK"))
					{
						storeData(%TrueClientId, "COINS", %c, "inc");
						storeData(%TrueClientId, "BANK", %c, "dec");
						RefreshAll(%TrueClientId);
						AI::sayLater(%TrueClientId, %closestId, "I have given you " @ %c @ " credits.  You are now carrying " @ fetchData(%TrueClientId, "COINS") @ " credits and I have " @ fetchData(%TrueClientId, "BANK") @ " of yours.  Have a nice day.", True);

						playSound(SoundMoney1, GameBase::getPosition(%TrueClientId));
					}
					else
					{
						AI::sayLater(%TrueClientId, %closestId, "I'm sorry but you don't have that many credits in my bank.  Your transaction has been cancelled.", True);
					}
					$state[%closestId, %TrueClientId] = "";
				}
			}
			else if(clipTrailingNumbers(%aiName) == "assassin")
			{
				//process assassin code
				%trigger[2] = "yes";
				%trigger[3] = "no";
				%trigger[4] = "buy";
				if($state[%closestId, %TrueClientId] == "")
				{
					if(%initTalk)
					{
						%highest = -1;
						%list = GetPlayerIdList();
						for(%i = 0; (%id = GetWord(%list, %i)) != -1; %i++)
						{
							if(fetchData(%id, "bounty") == "")
								storeData(%id, "bounty", 0);
							if(fetchData(%id, "bounty") > %highest)
							{
								%h = %id;
								%highest = fetchData(%id, "bounty");
							}
						}
						%n = Client::getName(%h);
						%c = fetchData(%h, "bounty");

						AI::sayLater(%TrueClientId, %closestId, "The highest bounty is currently on " @ %n @ " for $" @ %c @ ". Give me someone's name and I'll tell you their bounty, unless you want to BUY something." , True);

						$state[%closestId, %TrueClientId] = 1;
					}
				}
				else if($state[%closestId, %TrueClientId] == 1)
				{
					if(String::findSubStr(%message, %trigger[4]) != -1)
					{
						%cost = GetLCKcost(%TrueClientId);

						AI::sayLater(%TrueClientId, %closestId, "I will sell you one LCK point for $" @ %cost @ ". (YES/NO)", True);
						$state[%closestId, %TrueClientId] = 2;
					}
					else
					{
						%lowest = 99999;
						%h = "";
						%list = GetPlayerIdList();
						for(%i = 0; (%id = GetWord(%list, %i)) != -1; %i++)
						{
							%comp = String::ICompare(%cropped, Client::getName(%id));
							if(%comp < 0) %comp = -%comp;

							if(%comp < %lowest)
							{
								%h = %id;
								%lowest = %comp;
							}
						}
						if(%h != "")
						{
							%l = fetchData(%h, "LVL");
							%c = getFinalCLASS(%h);
							AI::sayLater(%TrueClientId, %closestId, "Are you talking about " @ Client::getName(%h) @ " the Level " @ %l @ " " @ %c @ "?", True);
							storeData(%TrueClientId, "tmpdata", %h);
							$state[%closestId, %TrueClientId] = 3;
						}
						else
						{
							AI::sayLater(%TrueClientId, %closestId, "I have no idea who you are talking about. Goodbye.", True);
							$state[%closestId, %TrueClientId] = "";
						}
					}
				}
				else if($state[%closestId, %TrueClientId] == 2)
				{
					if(String::findSubStr(%message, %trigger[2]) != -1)
					{
						%cost = GetLCKcost(%TrueClientId);

						if(fetchData(%TrueClientId, "COINS") >= %cost)
						{
							AI::sayLater(%TrueClientId, %closestId, "Here's your LCK point, thanks for your business.", True);
							GiveThisStuff(%TrueClientId, "LCK 1", True);
							storeData(%TrueClientId, "COINS", %cost, "dec");
							RefreshAll(%TrueClientId);
						}
						else
							AI::sayLater(%TrueClientId, %closestId, "You can't afford this.", True);

						$state[%closestId, %TrueClientId] = "";
					}
					else if(String::findSubStr(%message, %trigger[3]) != -1)
					{
						AI::sayLater(%TrueClientId, %closestId, "See ya.", True);
						$state[%closestId, %TrueClientId] = "";
					}
				}
				else if($state[%closestId, %TrueClientId] == 3)
				{
					if(String::findSubStr(%message, %trigger[2]) != -1)
					{
						%id = fetchData(%TrueClientId, "tmpdata");
						if(%id != %TrueClientId)
						{
							%n = Client::getName(%id);
							if(IsInCommaList(fetchData(%TrueClientId, "TempKillList"), %n))
							{
								storeData(%TrueClientId, "TempKillList", RemoveFromCommaList(fetchData(%TrueClientId, "TempKillList"), %n));
								AI::sayLater(%TrueClientId, %closestId, "I see you've killed " @ %n @ ". Here's your reward... " @ fetchData(%id, "bounty") @ " credits. Goodbye.", True);
								storeData(%TrueClientId, "COINS", fetchData(%id, "bounty"), "inc");
								storeData(%id, "bounty", 0);
	
								playSound(SoundMoney1, GameBase::getPosition(%TrueClientId));
								RefreshAll(%TrueClientId);
							}
							else
								AI::sayLater(%TrueClientId, %closestId, %n @ "'s bounty is currently at " @ fetchData(%id, "bounty") @ " credits. Goodbye.", True);
						}
						else
							AI::sayLater(%TrueClientId, %closestId, "You can't get a reward for killing yourself... idiot.", True);

						$state[%closestId, %TrueClientId] = "";
					}
					else if(String::findSubStr(%message, %trigger[3]) != -1)
					{
						AI::sayLater(%TrueClientId, %closestId, "Well then, I have no idea who you are talking about. Goodbye.", True);
						storeData(%TrueClientId, "tmpdata", "");
						$state[%closestId, %TrueClientId] = "";
					}
				}
			}
			else if(clipTrailingNumbers(%aiName) == "porter")
			{
				//process porter code
				%trigger[2] = "enter";
				if($state[%closestId, %TrueClientId] == "")
				{
					if(%initTalk)
					{
						if($arenaOn)
						{
							AI::sayLater(%TrueClientId, %closestId, "I am in charge of admitting fighters.  Do you want to ENTER for $" @ $teleportInArenaCost @ "?", True);
							$state[%closestId, %TrueClientId] = 1;
						}
						else
						{
							AI::sayLater(%TrueClientId, %closestId, "I'm sorry but the arena was disabled by your server admin.", True);
							$state[%closestId, %TrueClientId] = "";
						}
					}
				}
				else if($state[%closestId, %TrueClientId] == 1)
				{
					if(String::findSubStr(%message, %trigger[2]) != -1)
					{
						if(fetchData(%TrueClientId, "COINS") >= $teleportInArenaCost)
						{
							%retval = TeleportToMarker(%TrueClientId, "TheArena\\TeleportEntranceMarkers", 1, 0);
							if(%retval != False)
							{
								storeData(%TrueClientId, "COINS", $teleportInArenaCost, "dec");
								storeData(%TrueClientId, "inArena", True);
								RefreshArenaTextBox(%TrueClientId);
								RefreshAll(%TrueClientId);

								$state[%closestId, %TrueClientId] = "";
							}
							else
							{
								AI::sayLater(%TrueClientId, %closestId, "Hmmm... I guess there are people standing in the way of the teleport destinations.  Try again later.", True);
								$state[%closestId, %TrueClientId] = "";
							}
						}
						else
						{
							AI::sayLater(%TrueClientId, %closestId, "You don't even have that many credits.  Sorry, you can't get in.", True);
							$state[%closestId, %TrueClientId] = "";
						}
					}
				}
			}
			else if(clipTrailingNumbers(%aiName) == "quest" || clipTrailingNumbers(%aiName) == "tutorial")
			{
				//process quest code
				%trigger[2] = $BotInfo[%aiName, CUE, 1];
				%trigger[3] = $BotInfo[%aiName, NCUE, 1];
				%trigger[4] = "buy";

				if(%initTalk || $state[%closestId, %TrueClientId] != "")
				{
					%hasTheStuff = HasThisStuff(%TrueClientId, $BotInfo[%aiName, NEED]);

					if($BotInfo[%aiName, CSAY] == "" && %hasTheStuff == 666)
						%hasTheStuff = False;
					if($BotInfo[%aiName, LSAY] == "" && %hasTheStuff == 667)
						%hasTheStuff = False;

					if(%hasTheStuff == 666 && $state[%closestId, %TrueClientId] == "")
					{
						if(%initTalk)
						{
							AI::sayLater(%TrueClientId, %closestId, $BotInfo[%aiName, CSAY], True);
							$state[%closestId, %TrueClientId] = -5;
						}
					}
					else if(%hasTheStuff == 667 && $state[%closestId, %TrueClientId] == "")
					{
						if(%initTalk)
						{
							AI::sayLater(%TrueClientId, %closestId, $BotInfo[%aiName, LSAY], True);
							$state[%closestId, %TrueClientId] = -5;
						}
					}
					else if(%hasTheStuff == False)
					{
						if($state[%closestId, %TrueClientId] == "")
						{
							if(%initTalk)
							{
								AI::sayLater(%TrueClientId, %closestId, $BotInfo[%aiName, SAY, 1] @ " [" @ %trigger[2] @ "]", True);
								$state[%closestId, %TrueClientId] = 1;
							}
						}
						else if($state[%closestId, %TrueClientId] == 1)
						{
							if(String::findSubStr(%message, %trigger[2]) != -1)
							{
								AI::sayLater(%TrueClientId, %closestId, $BotInfo[%aiName, SAY, 2], True);
								$state[%closestId, %TrueClientId] = "";
							}
						}
					}
					else if(%hasTheStuff == True)
					{
						if($state[%closestId, %TrueClientId] == "")
						{
							if(%initTalk)
							{
								AI::sayLater(%TrueClientId, %closestId, $BotInfo[%aiName, NSAY, 1] @ " [" @ %trigger[3] @ "]", True);
								$state[%closestId, %TrueClientId] = 1;
							}
						}
						else if($state[%closestId, %TrueClientId] == 1)
						{
							if(String::findSubStr(%message, %trigger[3]) != -1)
							{
								if(HasThisStuff(%TrueClientId, $BotInfo[%aiName, NEED]))
								{
									if($BotInfo[%aiName, TAKE] != "")
										TakeThisStuff(%TrueClientId, $BotInfo[%aiName, TAKE], True);
									if($BotInfo[%aiName, GIVE] != "")
										GiveThisStuff(%TrueClientId, $BotInfo[%aiName, GIVE], True);
	
									AI::sayLater(%TrueClientId, %closestId, $BotInfo[%aiName, NSAY, 2], True);
								}
								else
									AI::sayLater(%TrueClientId, %closestId, "Nice try, I'm keeping what I managed to get from you.", True);
	
								$state[%closestId, %TrueClientId] = "";
								Game::refreshClientScore(%TrueClientId);
							}
						}
					}

					%t4 = String::findSubStr(%message, %trigger[4]);
					if(%t4 != -1 || ($state[%closestId, %TrueClientId] == -5 && %t4 != -1))
					{
						if($BotInfo[%aiName, SHOP] != "")
						{
							SetupShop(%TrueClientId, %closestId);
							AI::sayLater(%TrueClientId, %closestId, "Take a look at what I have.", True);
						}
						else
							AI::sayLater(%TrueClientId, %closestId, "I have nothing to sell.", True);

						$state[%closestId, %TrueClientId] = "";
					}
				}
			}
			else if(clipTrailingNumbers(%aiName) == "manager")
			{
				//process manager code
				%trigger[2] = "fight";
				%trigger[3] = "leave";
				if($state[%closestId, %TrueClientId] == "")
				{
					if(%initTalk)
					{
						AI::sayLater(%TrueClientId, %closestId, "Hail. Do you wish to FIGHT or LEAVE?", True);
						$state[%closestId, %TrueClientId] = 1;
					}
				}
				else if($state[%closestId, %TrueClientId] == 1)
				{
					if(String::findSubStr(%message, %trigger[2]) != -1)
					{
						//FIGHT
						%x = AddToRoster(%TrueClientId);
						if(%x != -1)
						{
							TeleportToMarker(%TrueClientId, "TheArena\\WaitingRoomMarkers", 0, 1);

							$state[%closestId, %TrueClientId] = "";
						}
						else
						{
							//arena is full
							AI::sayLater(%TrueClientId, %closestId, "Sorry, the arena roster is full right now.", True);
							$state[%closestId, %TrueClientId] = "";
						}
					}
					else if(String::findSubStr(%message, %trigger[3]) != -1)
					{
						//LEAVE
						%retval = TeleportToMarker(%TrueClientId, "TheArena\\TeleportExitMarkers", 1, 0);

						if(%retval != False)
						{
							storeData(%TrueClientId, "inArena", "");
							CloseArenaTextBox(%TrueClientId);

							$state[%closestId, %TrueClientId] = "";
						}
						else
						{
							AI::sayLater(%TrueClientId, %closestId, "Hmmm... I guess there are people standing in the way of the teleport destinations.  Try again later.", True);
							$state[%closestId, %TrueClientId] = "";
						}
					}
				}
			}
			if(clipTrailingNumbers(%aiName) == "botmaker")
			{
				//process botmaker code
				%trigger[2] = "yes";
				%trigger[3] = "no";
				if($state[%closestId, %TrueClientId] == "")
				{
					if(%initTalk)
					{
						if(CountObjInCommaList($PetList) >= $maxPets)
						{
							AI::sayLater(%TrueClientId, %closestId, "I'm sorry but all my mercenaries are already on duty.", True);
							$state[%closestId, %TrueClientId] = "";
						}
						else if(CountObjInCommaList(fetchData(%TrueClientId, "PersonalPetList")) >= $maxPetsPerPlayer)
						{
							AI::sayLater(%TrueClientId, %closestId, "I'm sorry but you have too many mercenaries currently at your disposal.", True);
							$state[%closestId, %TrueClientId] = "";
						}
						else
						{
							AI::sayLater(%TrueClientId, %closestId, "I have all sorts of mercenaries at my disposal. Tell me which class you are interested in.", True);
							$state[%closestId, %TrueClientId] = 1;
						}
					}
				}
				else if($state[%closestId, %TrueClientId] == 1)
				{
					%class = GetWord(%cropped, 0);
					%gender = GetWord(%cropped, 1);
					%defaults = "LVL 5 LCK 1 HoldoutBlaster 1 ApprenticeRobe0 1";//$BotInfo[%aiName, DEFAULTS, %class];
					if(%gender == -1)
						%gender = "Male";

					if(String::ICompare(%gender, "male") == 0)
					{
						%gender = "Male";
						%gflag = True;
					}
					else if(String::ICompare(%gender, "female") == 0)
					{
						%gender = "Female";
						%gflag = True;
					}

					if(String::ICompare(%class, "jedi") == 0)
						%class = "Jedi Apprentice";
					else if(String::ICompare(%class, "gray") == 0)
						%class = "Gray Apprentice";
					else if(String::ICompare(%class, "sith") == 0)
						%class = "Sith Apprentice";
					else if(String::ICompare(%class, "soldier") == 0)
						%class = "private";
					else if(String::ICompare(%class, "pilot") == 0)
						%class = "navalman recruit";
					else if(String::ICompare(%class, "mercenary") == 0)
						%class = "mercenary";
					else if(String::ICompare(%class, "smuggler") == 0)
						%class = "smuggler";

					if(%defaults != "")
					{
						if(%gflag)
						{
							%lvl = GetStuffStringCount(%defaults, "LVL");
							%nc = pow(%lvl, 2) * 3;
							$tmpdata[%TrueClientId, 1] = %class;
							$tmpdata[%TrueClientId, 2] = %gender;
							$tmpdata[%TrueClientId, 3] = %nc;	//just so the equation is only in one place.

							AI::sayLater(%TrueClientId, %closestId, "My " @ %class @ "s are Level " @ %lvl @ ", and will cost you " @ %nc @ " credits. [yes/no]", True);
							$state[%closestId, %TrueClientId] = 2;
						}
						else
						{
							AI::sayLater(%TrueClientId, %closestId, "Invalid gender. Use 'male' or 'female'.", True);
							$state[%closestId, %TrueClientId] = "";
						}
					}
					else
					{
						AI::sayLater(%TrueClientId, %closestId, "Invalid class. Use any of the following: jedi gray sith soldier pilot mercenary smuggler.", True);
						$state[%closestId, %TrueClientId] = "";
					}
				}
				else if($state[%closestId, %TrueClientId] == 2)
				{
					if(String::findSubStr(%message, %trigger[2]) != -1)
					{
						%nc = $tmpdata[%TrueClientId, 3];

						if(%nc <= 0)
						{
							AI::sayLater(%TrueClientId, %closestId, "Invalid request.  Your transaction has been cancelled.~wError_Message.wav", True);
							$state[%closestId, %TrueClientId] = "";
						}
						else if(%nc <= fetchData(%TrueClientId, "COINS"))
						{
							%class = $tmpdata[%TrueClientId, 1];
							%gender = $tmpdata[%TrueClientId, 2];
							%defaults = $BotInfo[%aiName, DEFAULTS, %class];
							%lvl = GetStuffStringCount(%defaults, "LVL");
	
							storeData(%TrueClientId, "COINS", %nc, "dec");
							playSound(SoundMoney1, GameBase::getPosition(%closestId));
							RefreshAll(%TrueClientId);
	
							%n = "";
							for(%i = 0; (%a = GetWord($BotInfo[%aiName, NAMES], %i)) != -1; %i++)
							{
								if(NEWgetClientByName(%a) == -1)
								{
									%n = %a;
									break;
								}
							}
							if(%n == "")
								%n = "generic";

							$BotEquipment[generic] = "CLASS " @ %class @ " " @ %defaults;
							%an = AI::helper("generic", %n, "TempSpawn " @ GameBase::getPosition($BotInfo[%aiName, DESTSPAWN]) @ " " @ GameBase::getTeam(%TrueClientId));
							%id = AI::getId(%an);
							ChangeRace(%id, %gender @ "Human");
							storeData(%id, "tmpbotdata", %TrueClientId);
							storeData(%id, "botAttackMode", 2);

							schedule("Pet::BeforeTurnEvil(" @ %id @ ");", 55*60, Client::getOwnedObject(%id));
							schedule("Pet::TurnEvil(" @ %id @ ");", 60*60, Client::getOwnedObject(%id));

							$PetList = AddToCommaList($PetList, %id);
							storeData(%TrueClientId, "PersonalPetList", AddToCommaList(fetchData(%TrueClientId, "PersonalPetList"), %id));
							storeData(%id, "petowner", %TrueClientId);
						
							AI::sayLater(%TrueClientId, %closestId, "This is " @ %n @ ", a Level " @ %lvl @ " " @ %class @ "! He is at your disposal. He will follow you around and fight for you for the next hour.", True);
							$state[%closestId, %TrueClientId] = "";
						}
						else
						{
							AI::sayLater(%TrueClientId, %closestId, "You don't have enough credits. Goodbye.", True);
							$state[%closestId, %TrueClientId] = "";
						}

					}
					else if(String::findSubStr(%message, %trigger[3]) != -1)
					{
						AI::sayLater(%TrueClientId, %closestId, "As you wish. Goodbye.", True);
						$state[%closestId, %TrueClientId] = "";
					}
				}
			}
			if(clipTrailingNumbers(%aiName) == "blacksmith")
			{
				//process botmaker code
				%trigger[2] = "buy";
				%trigger[3] = "smith";
				if($state[%closestId, %TrueClientId] == "")
				{
					if(%initTalk)
					{
						AI::sayLater(%TrueClientId, %closestId, "Hail friend, are you here to have me SMITH an old weapon?", True);
						$state[%closestId, %TrueClientId] = 1;
					}
				}
				else if($state[%closestId, %TrueClientId] == 1)
				{
					if(String::findSubStr(%message, %trigger[2]) != -1)
					{
						if($BotInfo[%aiName, SHOP] != "")
						{
							SetupShop(%TrueClientId, %closestId);
							AI::sayLater(%TrueClientId, %closestId, "Take a look at what I have.", True);
						}
						else
							AI::sayLater(%TrueClientId, %closestId, "I have nothing to sell.", True);

						$state[%closestId, %TrueClientId] = "";
					}
					if(String::findSubStr(%message, %trigger[3]) != -1)
					{
						AI::sayLater(%TrueClientId, %closestId, "Click Use on an item and I will tell you how much it will cost to smith. Click Use on this item again and I will get to work.", True);
						SetupBlacksmith(%TrueClientId, %closestId);

						$state[%closestId, %TrueClientId] = "";
					}
				}
			}
			else if(clipTrailingNumbers(%aiName) == "guildmaster")
			{
				//process guildmaster code
				%trigger[2] = "join";
				if($state[%closestId, %TrueClientId] == "")
				{
					if(%initTalk)
					{
						if(fetchData(%TrueClientId, "LVL") >= 25)
						{
							%h = fetchData(%TrueClientId, "MyHouse");
							if(%h == "")
							{
								AI::sayLater(%TrueClientId, %closestId, "Hello. Are you interested in joining a faction for " @ $joinHouseCost @ " credits? [join]", True);
								$state[%closestId, %TrueClientId] = 1;
							}
							else
							{
								AI::sayLater(%TrueClientId, %closestId, "Members of " @ %h @ " are welcome here. Are you interested in joining a new faction for " @ $changeHouseCost @ " credits? [join]", True);
								$state[%closestId, %TrueClientId] = 2;
							}
						}
						else
						{
							AI::sayLater(%TrueClientId, %closestId, "Come back when you are at least level 25. Goodbye.", True);
							$state[%closestId, %TrueClientId] = "";
						}
					}
				}
				if($state[%closestId, %TrueClientId] == 1 || $state[%closestId, %TrueClientId] == 2)
				{
					if(String::findSubStr(%message, %trigger[2]) != -1)
					{
						%ch = fetchData(%TrueClientId, "MyHouse");
						%hlist = "";
						for(%i = 1; $HouseName[%i] != ""; %i++)
						{
							if($HouseName[%i] != %ch)
								%hlist = %hlist @ $HouseName[%i] @ ", ";
						}
						%fhlist = String::NEWgetSubStr(%hlist, 0, String::len(%hlist)-2) @ ".";

						if($state[%closestId, %TrueClientId] == 1)
						{
							//join new house
							AI::sayLater(%TrueClientId, %closestId, "Which faction would you like to join? [" @ %fhlist @ "]", True);
							$state[%closestId, %TrueClientId] = 3;
						}
						else if($state[%closestId, %TrueClientId] == 2)
						{
							//change house
							AI::sayLater(%TrueClientId, %closestId, "Which faction would you like to change to? [" @ %fhlist @ "]", True);
							$state[%closestId, %TrueClientId] = 4;
						}
					}
				}
				else if($state[%closestId, %TrueClientId] == 3 || $state[%closestId, %TrueClientId] == 4)
				{
					%houseNum = "";
					for(%i = 1; $HouseName[%i] != ""; %i++)
					{
						if(String::ICompare(%cropped, $HouseName[%i]) == 0)
							%houseNum = %i;
					}

					if(%houseNum != "")
					{
						if($state[%closestId, %TrueClientId] == 3)
							%cost = $joinHouseCost;
						else if($state[%closestId, %TrueClientId] == 4)
							%cost = $changeHouseCost;

						%c = floor(fetchData(%TrueClientId, "COINS"));
						if(%c >= %cost)
						{
							storeData(%TrueClientId, "COINS", %cost, "dec");
							BootFromCurrentHouse(%TrueClientId, True);
							JoinHouse(%TrueClientId, %houseNum, True);
							GiveHouseStuff(%TrueClientId, %houseNum);
							//GiveThisStuff(%TrueClientId, $HouseStartUpEq[%houseNum]);

							RefreshAll(%TrueClientId);
							AI::sayLater(%TrueClientId, %closestId, "Welcome to " @ $HouseName[%houseNum] @ "! Here is your start up equipment. Good luck on your adventures!", True);

							playSound(SoundMoney1, GameBase::getPosition(%TrueClientId));
						}
						else
						{
							AI::sayLater(%TrueClientId, %closestId, "I'm sorry but you do not have enough credits. Goodbye.", True);
						}
						$state[%closestId, %TrueClientId] = "";
					}
					else
					{
						AI::sayLater(%TrueClientId, %closestId, "That faction does not exist. Goodbye.", True);
						$state[%closestId, %TrueClientId] = "";
					}
				}
			}
		}
		else
		{
			//This condition occurs when you are talking from too far of any TownBot.  All states are cleared here.
			//This means that potentially, you could initiate a conversation with the banker, travel for an hour
			//WITHOUT saying a word, come back and continue the conversation.  As soon as you speak in a way that
			//townbots hear you (#say, #shout, #tell) and are too far from them, all conversations are reset.

			for(%i = 0; (%id = GetWord($TownBotList, %i)) != -1; %i++)
				$state[%id, %TrueClientId] = "";
		}
	}
}

function remoteIssueCommand(%commander, %cmdIcon, %command, %wayX, %wayY, %dest1, %dest2, %dest3, %dest4, %dest5, %dest6, %dest7, %dest8, %dest9, %dest10, %dest11, %dest12, %dest13, %dest14)
{
	// issueCommandI takes waypoint 0-1023 in x,y scaled mission area
	// issueCommand takes float mission coords.
	for(%i = 1; %dest[%i] != ""; %i = %i + 1)
		if(!%dest[%i].muted[%commander])
			issueCommandI(%commander, %dest[%i], %cmdIcon, %command, %wayX, %wayY);
}

function remoteIssueTargCommand(%commander, %cmdIcon, %command, %targIdx, %dest1, %dest2, %dest3, %dest4, %dest5, %dest6, %dest7, %dest8, %dest9, %dest10, %dest11, %dest12, %dest13, %dest14)
{
	for(%i = 1; %dest[%i] != ""; %i = %i + 1)
		if(!%dest[%i].muted[%commander])
			issueTargCommand(%commander, %dest[%i], %cmdIcon, %command, %targIdx);
}

function remoteCStatus(%clientId, %status, %message)
{
	// setCommandStatus returns false if no status was changed.
	// in this case these should just be team says.
	if(setCommandStatus(%clientId, %status, %message))
	{
		if($dedicated)
			echo("COMMANDSTATUS: " @ %clientId @ " \"" @ escapeString(%message) @ "\"");
	}
	else
		remoteSay(%clientId, true, %message);
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
	dbecho($dbechoMode, "messageAll(" @ %mtype @ ", " @ %message @ ", " @ %filter @ ")");

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
	dbecho($dbechoMode, "messageAllExcept(" @ %except @ ", " @ %mtype @ ", " @ %message @ ")");

	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl != %except)
			Client::sendMessage(%cl, %mtype, %message);
	}
}

function radiusAllExcept(%except1, %except2, %message)
{
	dbecho($dbechoMode, "radiusAllExcept(" @ %except1 @ ", " @ %except2 @ ", " @ %message @ ")");

	%epos1 = GameBase::getPosition(%except1);
	%epos2 = GameBase::getPosition(%except2);
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		%clpos = GameBase::getPosition(%cl);
		%dist1 = Vector::getDistance(%clpos, %epos1);
		%dist2 = Vector::getDistance(%clpos, %epos2);
		if(%cl != %except1 && %cl != %except2 && !IsDead(%cl))
		{
			if(%dist1 <= $maxSAYdistVec || %dist2 <= $maxSAYdistVec)
				Client::sendMessage(%cl, $MsgBeige, %message);
		}
	}
}

function FadeMsg(%txt, %dist, %max)
{
	dbecho($dbechoMode, "FadeMsg(" @ %txt @ ", " @ %dist @ ", " @ %max @ ")");

	if(%dist <= %max)
		return %txt;
	else
	{
		for(%i = 0; (%z = GetWord(%txt, %i)) != -1; %i++)
			%ntxt = %ntxt @ %z;
		%lntxt = String::len(%ntxt);

		%x = %dist - %max;
		%amt = round((%x / %max) * %lntxt);

		%txt = BuildDotString(%txt, %amt);
		
		return %txt;
	}
}

function BuildDotString(%txt, %n)
{
	dbecho($dbechoMode, "BuildDotString(" @ %txt @ ", " @ %n @ ")");

	%len = String::len(%txt);

	//i currently dont really know any other way to put a certain amount of characters in a string in a random fashion
	//other than to "sprinkle" them on until the count is correct.  Maybe someday someone will decide to rework this
	//function and make it more CPU friendly.  Right now this method sucks.

	%retry = 0;
	for(%i = %n; %i > 0; %i)
	{
		%p = floor(getRandom() * %len);
		%a = String::getSubStr(%txt, %p, 1);
		if(%a != " " && %a != ".")
		{
			%txt = String::getSubStr(%txt, 0, %p) @ "." @ String::getSubStr(%txt, %p+1, 99999);
			%i--;
			%retry = 0;
		}
		else
			%retry++;

		if(%retry > 10)
			break;
	}
	return %txt;
}

function ClearBlockData(%name, %block)
{
	dbecho($dbechoMode, "ClearBlockData(" @ %name @ ", " @ %block @ ")");

	for(%i = 1; $BlockData[%name, %block, %i] != ""; %i++)
		$BlockData[%name, %block, %i] = "";
}

function ManageBlockOwnersList(%name)
{
	dbecho($dbechoMode, "ManageBlockOwnersList(" @ %name @ ")");

	%clientId = NEWgetClientByName(%name);

	if(CountObjInCommaList($BlockList[%name]) > 0)
	{
		if(!IsInCommaList($BlockOwnersList, %name))
		{
			$BlockOwnersList = AddToCommaList($BlockOwnersList, %name);
			if(%name != "Server")
				$BlockOwnerAdminLevel[%name] = floor(%clientId.adminLevel);
		}
	}
	else
	{
		$BlockOwnersList = RemoveFromCommaList($BlockOwnersList, %name);
		if(%name != "Server")
			$BlockOwnerAdminLevel[%name] = "";
	}

	return $BlockOwnersList;
}

function ParseBlockData(%bd, %victimId, %killerId)
{
	dbecho($dbechoMode, "ParseBlockData(" @ %bd @ ", " @ %victimId @ ", " @ %killerId @ ")");

	//the passed variables MUST BE IN COMMALIST FORMAT!

	%vtype[1] = "^victimName";
	%vtype[2] = "^victimId";
	%vtype[3] = "^victimPos";
	%vtype[4] = "^victimRot";
	%vtype[5] = "^victimZoneId";
	%vtype[6] = "^victimZoneType";
	%vtype[7] = "^victimZoneDesc";
	%vtype[8] = "^victimClass";
	%vtype[9] = "^victimLevel";
	%vtype[10] = "^victimX";
	%vtype[11] = "^victimY";
	%vtype[12] = "^victimZ";
	%vtype[13] = "^victimR1";
	%vtype[14] = "^victimR2";
	%vtype[15] = "^victimR3";
	%vtype[16] = "^victimCoins";
	%vtype[17] = "^victimBank";
	%vtype[18] = "^victimVelX";
	%vtype[19] = "^victimVelY";
	%vtype[20] = "^victimVelZ";

	%vtype[21] = "^killerName";
	%vtype[22] = "^killerId";
	%vtype[23] = "^killerPos";
	%vtype[24] = "^killerRot";
	%vtype[25] = "^killerZoneId";
	%vtype[26] = "^killerZoneType";
	%vtype[27] = "^killerZoneDesc";
	%vtype[28] = "^killerClass";
	%vtype[29] = "^killerLevel";
	%vtype[30] = "^killerX";
	%vtype[31] = "^killerY";
	%vtype[32] = "^killerZ";
	%vtype[33] = "^killerR1";
	%vtype[34] = "^killerR2";
	%vtype[35] = "^killerR3";
	%vtype[36] = "^killerCoins";
	%vtype[37] = "^killerBank";
	%vtype[38] = "^killerVelX";
	%vtype[39] = "^killerVelY";
	%vtype[40] = "^killerVelZ";

	if(%victimId != "")
	{
		%vpos = GameBase::getPosition(%victimId);
		%vrot = GameBase::getRotation(%victimId);
		%vvel = Item::getVelocity(%victimId);

		%var[1] = Client::getName(%victimId);
		%var[2] = %victimId;
		%var[3] = %vpos;
		%var[4] = %vrot;
		%var[5] = fetchData(%victimId, "zone");
		%var[6] = Zone::getType(fetchData(%victimId, "zone"));
		%var[7] = Zone::getDesc(fetchData(%victimId, "zone"));
		%var[8] = fetchData(%victimId, "CLASS");
		%var[9] = fetchData(%victimId, "LVL");
		%var[10] = GetWord(%vpos, 0);
		%var[11] = GetWord(%vpos, 1);
		%var[12] = GetWord(%vpos, 2);
		%var[13] = GetWord(%vrot, 0);
		%var[14] = GetWord(%vrot, 1);
		%var[15] = GetWord(%vrot, 2);
		%var[16] = fetchData(%victimId, "COINS");
		%var[17] = fetchData(%victimId, "BANK");
		%var[18] = GetWord(%vvel, 0);
		%var[19] = GetWord(%vvel, 1);
		%var[20] = GetWord(%vvel, 2);
	}
	if(%killerId != "")
	{
		%kpos = GameBase::getPosition(%killerId);
		%krot = GameBase::getRotation(%killerId);
		%kvel = Item::getVelocity(%killerId);

		%var[21] = Client::getName(%killerId);
		%var[22] = %killerId;
		%var[23] = %kpos;
		%var[24] = %krot;
		%var[25] = fetchData(%killerId, "zone");
		%var[26] = Zone::getType(fetchData(%killerId, "zone"));
		%var[27] = Zone::getDesc(fetchData(%killerId, "zone"));
		%var[28] = fetchData(%killerId, "CLASS");
		%var[29] = fetchData(%killerId, "LVL");
		%var[30] = GetWord(%kpos, 0);
		%var[31] = GetWord(%kpos, 1);
		%var[32] = GetWord(%kpos, 2);
		%var[33] = GetWord(%krot, 0);
		%var[34] = GetWord(%krot, 1);
		%var[35] = GetWord(%krot, 2);
		%var[36] = fetchData(%killerId, "COINS");
		%var[37] = fetchData(%killerId, "BANK");
		%var[38] = GetWord(%kvel, 0);
		%var[39] = GetWord(%kvel, 1);
		%var[40] = GetWord(%kvel, 2);
	}

	for(%i = 1; %vtype[%i] != ""; %i++)
		%bd = String::replace(%bd, %vtype[%i], %var[%i], True);

	return %bd;
}