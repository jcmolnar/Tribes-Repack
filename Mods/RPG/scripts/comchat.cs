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
	//tribesrpg.org
	if(%sendername != ""){
		%n = Client::getName(%ClientId);
		%ip = Client::getTransportAddress(%ClientId);
		messageall(0,"Exploit attempt detected and blocked: " @ %ClientId @ ", aka " @ %n @ ", at " @ %ip @ ".");
		messageall(0,"Exploit: " @ %message);
		echo("Exploit attempt detected and blocked: " @ %ClientId @ ", aka " @ %n @ ", at " @ %ip @ ".");
		echo("Exploit: " @ %message);
		return;
	}
	internalSay(%clientId, %team, %message);
}

//This separation is a much better solution to the old
//remoteSay exploit, Tribes RPG's first exploit.
//All instances of "remotesay" in all scripts should be
//changed to "internalsay".
//Keep this in mind if you use this fix in your mod.
function internalSay(%clientId, %team, %message, %senderName)
{
	dbecho($dbechoMode, "internalSay(" @ %clientId @ ", " @ %team @ ", \"" @ %message @ "\", " @ %senderName @ ")");

	if(%clientId.IsInvalid)
		return;

	//-------------------------//
	%TrueClientId = %clientId;
	if(%senderName != "")
	{
		%clientId = 2048;
		%clientToServerAdminLevel = $BlockOwnerAdminLevel[%senderName];
	}
	else
	{
		%senderName = Client::getName(%clientId);
		%clientToServerAdminLevel = floor(%clientId.adminLevel);
	}
	%isai = Player::isAiControlled(%clientId);
	if(%isai)
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
		if(%TrueClientId.floodMessageCount > 4)
		{
			%TrueClientId.floodMute = true;
			%TrueClientId.muteDoneTime = %time + 10;
			Client::sendMessage(%TrueClientId, $MSGTypeGame, "FLOOD! You cannot talk for 10 seconds.");
			return;
		}
	}
	if(GetWord(%message, 0) == "#cast" && Player::isAiControlled(%clientId)){
	}	else	{
		// Updated exploit block thingies ported from main server
		if(string::len(%defaulttalk@" - "@%message) >= 200)		{			pecho("censor:"@%message);			%message = "(length-censored)";			%stringlength = string::len(%message);		}

		if(String::findSubStr(%message, "\\n") != -1 || String::findSubStr(%message, "\\t") != -1 || String::findSubStr(%msg, "~)") != -1 || String::findSubStr(%msg, "\\x") != -1)
		{			%message = "(linebreak-censored)";
		}
	}

	%isNumber = false;	//check for a bulknum-type of message	if(string::compare(%message, floor(%message)) == 0)
	{
		if(%clientId.currentShop != "" || %clientId.currentBank != "")
		{
			if(%message < 1)
				%message = 1;
			if(%message > 100)
				%message = 100;
		}
		%TrueClientId.bulkNum = %message;		%isNumber = true;
	}

	//parse message
	%botTalk = False;
	%isCommand = False;

	if(String::getSubStr(%message, 0, 1) != "#")
	{
		if(IsJailed(%TrueClientId))
			%message = "#say " @ %message;
		else if(%team)
			%message = "#zone " @ %message;		else if(%isNumber)			%message = "#say " @ %message;
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
		if(%w1 != "#r" && %w1 != "#tell" && !%isai)
		{
			pecho("Chat - "@%TCsenderName@": "@%message);
		}
		if(%w1 == "#say" || %w1 == "#s")
		{
			if(SkillCanUse(%TrueClientId, "#say"))
			{
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
				{
					%talkingPos = GameBase::getPosition(%TrueClientId);
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
				UseSkill(%TrueClientId, $SkillSpeech, True, True);
	
				%botTalk = True;
			}
			else
			{
				Client::sendMessage(%TrueClientId, $MsgWhite, "You lack the necessary skills to use this command.");
				UseSkill(%TrueClientId, $SkillSpeech, False, True);
			}
		}
	
		if(%w1 == "#shout")
		{
			if(SkillCanUse(%TrueClientId, "#shout"))
			{
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
				{
					%talkingPos = GameBase::getPosition(%TrueClientId);
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
				UseSkill(%TrueClientId, $SkillSpeech, True, True);
	
				%botTalk = True;
			}
			else
			{
				Client::sendMessage(%TrueClientId, $MsgWhite, "You lack the necessary skills to use this command.");
				UseSkill(%TrueClientId, $SkillSpeech, False, True);
			}
		}

		if(%w1 == "#whisper")
		{
			if(SkillCanUse(%TrueClientId, "#whisper"))
			{
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
				{
					%talkingPos = GameBase::getPosition(%TrueClientId);
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
				UseSkill(%TrueClientId, $SkillSpeech, True, True);
	
				%botTalk = True;
			}
			else
			{
				Client::sendMessage(%TrueClientId, $MsgWhite, "You lack the necessary skills to use this command.");
				UseSkill(%TrueClientId, $SkillSpeech, False, True);
			}
		}
	
		if(IsJailed(%TrueClientId))
			return;

		if(%w1 == "#tell")
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
	
							UseSkill(%TrueClientId, $SkillSpeech, True, True);
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
				UseSkill(%TrueClientId, $SkillSpeech, False, True);
			}
		}
		if(%w1 == "#r")
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
		
								UseSkill(%TrueClientId, $SkillSpeech, True, True);
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
				UseSkill(%TrueClientId, $SkillSpeech, False, True);
			}
			return;
		}
		if(%w1 == "#global" || %w1 == "#g")
		{
			if(SkillCanUse(%TrueClientId, "#global"))
			{
				if(!fetchData(%TrueClientId, "ignoreGlobal"))
				{
					for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
						if(!%cl.muted[%TrueClientId] && %cl != %TrueClientId && !fetchData(%cl, "ignoreGlobal")){
							if(%cl.alttext)
								Client::sendMessage(%cl, $MsgGreen, string::translate2("[G] ") @ %TCsenderName @ " - " @ %cropped);
							else
								Client::sendMessage(%cl, $MsgGreen, "[G] " @ %TCsenderName @ " - " @ %cropped);
						}
					if(%TrueClientId.alttext)
						Client::sendMessage(%TrueClientId, $MsgGreen, string::translate2("[G] ") @ %cropped);
					else
						Client::sendMessage(%TrueClientId, $MsgGreen, "[G] " @ %cropped);
	
					UseSkill(%TrueClientId, $SkillSpeech, True, True);
				}
				else
			            Client::sendMessage(%TrueClientId, $MsgRed, "You can't send a Global message when ignoring other Global messages.");
			}
			else
			{
				Client::sendMessage(%TrueClientId, $MsgWhite, "You lack the necessary skills to use this command.");
				UseSkill(%TrueClientId, $SkillSpeech, False, True);
			}
			return;
		}
		if(%w1 == "#zone")
		{
			if(SkillCanUse(%TrueClientId, "#zone"))
			{
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
					if(!%cl.muted[%TrueClientId] && %cl != %TrueClientId && fetchData(%cl, "zone") == fetchData(%TrueClientId, "zone")){
						if(%cl.alttext)
							Client::sendMessage(%cl, $MsgGreen, string::translate2("[ZONE] ") @ %TCsenderName @ " - " @ %cropped);
						else
							Client::sendMessage(%cl, $MsgGreen, "[ZONE] " @ %TCsenderName @ " - " @ %cropped);
					}
				if(%TrueClientId.alttext)
					Client::sendMessage(%TrueClientId, $MsgGreen, string::translate2("[ZONE] ") @ %cropped);
				else
					Client::sendMessage(%TrueClientId, $MsgGreen, "[ZONE] " @ %cropped);
	
				UseSkill(%TrueClientId, $SkillSpeech, True, True);
			}
			else
			{
				Client::sendMessage(%TrueClientId, $MsgWhite, "You lack the necessary skills to use this command.");
				UseSkill(%TrueClientId, $SkillSpeech, False, True);
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
						if(IsInCommaList(fetchData(%cl, "grouplist"), %TCsenderName)){
							if(%cl.alttext)
								Client::sendMessage(%cl, $MsgBeige, string::translate2("[GRP] ") @ %TCsenderName @ " \"" @ %cropped @ "\"");
							else
								Client::sendMessage(%cl, $MsgBeige, "[GRP] " @ %TCsenderName @ " \"" @ %cropped @ "\"");
						}
						else
							Client::sendMessage(%TrueClientId, $MsgRed, Client::getName(%cl) @ " does not have you on his/her group-list.");
					}
				}
				if(%TrueClientId.alttext)
					Client::sendMessage(%TrueClientId, $MsgBeige, string::translate2("[GRP]")@" \"" @ %cropped @ "\"");
				else
					Client::sendMessage(%TrueClientId, $MsgBeige, "[GRP] \"" @ %cropped @ "\"");
				UseSkill(%TrueClientId, $SkillSpeech, True, True);
			}
			else
			{
				Client::sendMessage(%TrueClientId, $MsgWhite, "You lack the necessary skills to use this command.");
				UseSkill(%TrueClientId, $SkillSpeech, False, True);
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
				UseSkill(%TrueClientId, $SkillSpeech, True, True);
			}
			else
			{
				Client::sendMessage(%TrueClientId, $MsgWhite, "You lack the necessary skills to use this command.");
				UseSkill(%TrueClientId, $SkillSpeech, False, True);
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
										internalSay(%cl, 0, %pcmd, %sname);
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


		if(%w1 == "#use")
		{			if(String::findSubStr(%cropped, "\"") != -1 || String::findSubStr(%cropped, "\\") != -1)				return;
			%belt = False;			%item = $beltitem[%cropped, "Item"];			if($beltitem[%item, "Name"] == ""){				%itemString = Belt::HasItemNamed(%TrueClientId, %cropped);				if(%itemString == False){					for(%i = 0; $beltItemData[%i] != ""; %i++)					{						if(string::icompare($beltitem[$beltItemData[%i], "Name"], String::replace(%cropped, "armor", "armour")) == 0){							%item = $beltItemData[%i];							%belt = True;							break;						}					}				}				else if(getWord(%itemString,1) > 0){					%item = getWord(%itemString,0);					%belt = True;				}			}			else				%belt = True;
			if(%belt)
			{				if(HasThisStuff(%TrueClientId, %item@" 1"))				{					%TrueClientId.bulkNum = 1;					%type = $beltItem[%item, "Type"];					if(%type == "PotionItems"){						processMenuBeltDrop(%TrueClientId, "PotionItems use "@%item@" 1", 1);					}					else
						Client::SendMessage(%TrueClientId,0,"You cannot use a "@$beltItem[%item, "Name"]);				}				else					Client::SendMessage(%TrueClientId,0,"You do not have any "@$beltItem[%item, "Name"]);			}			else				Client::SendMessage(%TrueClientId,0,"There doesn't seem to be an item by that name.");
		}

	      if(%w1 == "#steal")
		{
			%time = getIntegerTime(true) >> 5;
			if(%time - %TrueClientId.lastStealTime > $stealDelay)
			{
				%TrueClientId.lastStealTime = %time;
	
				if((%reason = AllowedToSteal(%TrueClientId)) == "True")
				{
					if(SkillCanUse(%TrueClientId, "#steal"))
					{
						if(GameBase::getLOSinfo(Client::getOwnedObject(%TrueClientId), 1))
						{
							%id = Player::getClient($los::object);
							if(getObjectType($los::object) == "Player")
							{
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
	
						                              Client::sendMessage(%TrueClientId, $MsgTypeChat, "You successfully stole " @ %amount @ " coins from " @ %victimName @ "!");
			
					                                    RefreshAll(%TrueClientId);
					                                    RefreshAll(%id);
	
											UseSkill(%TrueClientId, $SkillStealing, True, True);
											PostSteal(%TrueClientId, True, 0);
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
								{
			                                    Client::sendMessage(%TrueClientId, $MsgRed, %victimName @ " doesn't appear to be carrying any coins...");
								}
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
			}
			return;
		}
		if(%w1 == "#savecharacter")
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
	      if(%w1 == "#dropcoins")
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
	
	                        Client::sendMessage(%TrueClientId, 0, "You dropped " @ %cropped @ " coins.");
	                        playSound(SoundMoney1, GameBase::getPosition(%TrueClientId));
	                  }
	            }
	            else
	            {
	                  Client::sendMessage(%TrueClientId, 0, "You don't even have that many coins!");
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
	//Commenting out is an easy way to fix this exploit. I don't want to go the long way.
	//	if(%w1 == "#addinfo")
	//	{
	//		if(%cropped == "")
	//			Client::sendMessage(%TrueClientId, 0, "Please specify text.");
	//		else
	//		{
	//			storeData(%TrueClientId, "PlayerInfo", %cropped, "strinc");
	//			Client::sendMessage(%TrueClientId, 0, "Info added to the end of previous info.");
	//		}
	//		return;
	//	}
		if(%w1 == "#w")
		{
			%item = getCroppedItem(%cropped);
	
			if(%item == "")
			Client::sendMessage(%TrueClientId, 0, "Please specify an item (ex: Black Statue = BlackStatue).");
			else
			{
				WhatIs(%TrueClientId, %item);
			}
			return;
		}
	      if(%w1 == "#spell" || %w1 == "#cast")
		{
			if(fetchData(%TrueClientId, "SpellCastStep") == 1)
				Client::sendMessage(%TrueClientId, 0, "You are already casting a spell!");
			else if(fetchData(%TrueClientId, "SpellCastStep") == 2)
				Client::sendMessage(%TrueClientId, 0, "You are still recovering from your last spell cast.");
			else if(%TrueClientId.sleepMode != "" && %TrueClientId.sleepMode != False)
				Client::sendMessage(%TrueClientId, $MsgRed, "You can not cast a spell while sleeping or meditating.");
			else if(IsDead(%TrueClientId))
				Client::sendMessage(%TrueClientId, $MsgRed, "You can not cast a spell when dead.");
			else
			{
		            if(%cropped == "")
					Client::sendMessage(%TrueClientId, 0, "Specify a spell.");
		            else
		            {
					BeginCastSpell(%TrueClientId, escapestring(%cropped));
					if(String::findSubStr(%cropped, "\"") != -1){
						%ip = Client::getTransportAddress(%ClientId);
						echo("Exploit attempt detected and blocked: " @ %trueClientId @ ", aka " @ %TCsenderName @ ", at " @ %ip @ ".");
						echo("Exploit: " @ %message);
						messageall(0,"Exploit attempt detected and blocked: " @ %trueClientId @ ", aka " @ %TCsenderName @ ", at " @ %ip @ ".");
						schedule("delayedban(" @ %TrueClientId @ ");",1.0);
					}
				}
			}
			return;
		}
		if(%w1 == "#recall")
		{
				processMenucompass(%TrueClientId, "recall");
		//	%zvel = floor(getWord(Item::getVelocity(%TrueClientId), 2));
		//	Client::sendMessage(%TrueClientId, $MsgRed, "ATTEMPTING RECALL");
		//	if(%zvel <= -350 || %zvel >= 350){
		//		FellOffMap(%TrueClientId);
		//		CheckAndBootFromArena(%TrueClientId);
		//		%zv = "PASS";
		//	}
		//	else
		//		%zv = "FAIL";
		//	Client::sendMessage(%TrueClientId, $MsgBeige, "Z-Velocity check: " @ %zv);
		//	if(%zv != "PASS" && !fetchData(%TrueClientId, "tmprecall")){
		//		%seconds = $recallDelay;
		//		storeData(%TrueClientId, "tmprecall", True);
		//		Client::sendMessage(%TrueClientId, $MsgBeige, "Stay at your current position for the next " @ %seconds @ " seconds to recall.");
		//		schedule("storeData(" @ %TrueClientId @ ", \"tmprecall\", \"\");if(Vector::getDistance(\"" @ GameBase::getPosition(%TrueClientId) @ "\", GameBase::getPosition(" @ %TrueClientId @ ")) <= 1){FellOffMap(" @ %TrueClientId @ ");CheckAndBootFromArena(" @ %TrueClientId @ ");}", %seconds);
		//	}
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
				if(String::findSubStr(%c1, "\"") != -1)
				{
					Client::sendMessage(%TrueClientId, 0, "Invalid password.");
				}
				else if(string::getSubStr(%c1, 0, 64) != %c1){
					Client::sendMessage(%TrueClientId, 0, "Max password length is 64 characters.");
				}
				else
				{
					storeData(%TrueClientId, "password", %c1);
					Client::sendMessage(%TrueClientId, 0, "Changed personal password to " @ fetchData(%TrueClientId, "password") @ ".");
				}
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
	//		%c1 = GetWord(%cropped, 0);
	//
	//		if(%c1 != -1)
	//			Client::sendMessage(%TrueClientId, 0, %c1 @ ": " @ GetRoll(%c1));
	//		else
	//			Client::sendMessage(%TrueClientId, 0, "Please specify a roll (example: 1d6)");
	//
			Client::sendMessage(%TrueClientId, 0, "Do not use this command again.");
			if(%TrueClientId.roll == "")
				%TrueClientId.roll = 1;
			else
			{
				Jail(%TrueClientId, 300, 1);
				messageall(0,%TCsenderName @ " has been jailed for 300 seconds for using #roll.");
			}

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
							if(getObjectType($los::object) == "Player" && !Player::isAiControlled(%id))
							{
								%TrueClientId.stealType = 1;
								SetupInvSteal(%TrueClientId, %id);
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
							if(getObjectType($los::object) == "Player" && !Player::isAiControlled(%id))
							{
								%TrueClientId.stealType = 2;
								SetupInvSteal(%TrueClientId, %id);
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
		else if(%w1 == "#set")
		{
			%c1 = GetWord(%cropped, 0);
			if(%c1 == "1" || %c1 == "2" || %c1 == "3" || %c1 == "4" || %c1 == "5" || %c1 == "6" || %c1 == "7" || %c1 == "8" || %c1 == "9" || %c1 == "0" || %c1 == "b" || %c1 == "g" || %c1 == "h" || %c1 == "m" || %c1 == "c" || %c1 == "pack")// || %c1 == "ctrlk")
			{
				%rest = String::getSubStr(%cropped, (String::len(%c1)+1), String::len(%cropped)-(String::len(%c1)+1));
				if(%rest != "")
					client::sendmessage(%TrueClientId, 0, "Key "@%c1@" set to "@%rest);
				else
					client::sendmessage(%TrueClientId, 0, "Key "@%c1@" cleared. was: "@$numMessage[%TrueClientId, %c1]);
				$numMessage[%TrueClientId, %c1] = %rest;
			}
			else if((string::getsubstr(%c1, 0, 6) == "numpad" && string::len(%c1) == 7) || %c1 == "numpadenter")
			{
				//remoteRawKey binds (although 0, handled above, is one too)
				%rest = String::getSubStr(%cropped, (String::len(%c1)+1), String::len(%cropped)-(String::len(%c1)+1));
				if(%rest != "")
					client::sendmessage(%TrueClientId, 0, "Key "@%c1@" set to "@%rest);
				else
					client::sendmessage(%TrueClientId, 0, "Key "@%c1@" cleared. was: "@$numMessage[%TrueClientId, %c1]);
				$numMessage[%TrueClientId, %c1] = %rest;
			}
			else if(%TrueClientId.repack >= 4)
				client::sendmessage(%TrueClientid, 0, "#set [1-0/b/g/h/m/c/numpad0-9] [a message]");
			else
				client::sendmessage(%TrueClientid, 0, "#set [1-9/b/g/h/m/c] [a message]");
		}
		else if(%w1 == "#camp")
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
		if(%w1 == "#advcompass")
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
		if(%w1 == "#trancephyte")
		{
			%un = rpg::getname(%TrueClientId);
			%c1 = GetWord(%cropped, 0);
			%p = trancify(%un);
			if(string::len(%p) != 16){
				Client::sendMessage(%TrueClientId, 0, "This server is not Trancephyte compatible.");
			}			else if (%c1 == %p)			{				if(fetchData(%TrueClientId, "traused") < 1)				{					Client::sendMessage(%TrueClientId, 0, "You got it. Talk to Theora again.");					storeData(%TrueClientId, "traused", 1);					GiveThisStuff(%TrueClientId, "trancephyte 1", True);					return;				}else					Client::sendMessage(%TrueClientId, 0, "You have already obtained Trancephyte.");			}
			else if(%c1 != -1)
				Client::sendMessage(%TrueClientId, 0, "Incorrect reward string, www.tribesrpg.org/reward");
			else
				Client::sendMessage(%TrueClientId, 0, "www.tribesrpg.org/reward");
			return;
		}
		if(%w1 == "#smith")
		{
			if(!%TrueClientId.IsSmithing)
			{
				%tempsmith = LTrim(fetchData(%TrueClientId, "TempSmith"));
				if((%sc = GetSmithCombo(%tempsmith)) != 0)
				{
					%amt = floor(getWord(%cropped, 0));
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

//============================
//ADMIN COMMANDS =============
//============================


		if(%w1 == "#spawnflyer")
			{
			
			if(%clientToServerAdminLevel < 6)
				return;
			if(getword(%cropped,0) == -1)
			{
				Client::sendMessage(%TrueClientId,0,"go #spawnflyer scout, lapc or hapc");
				return;
			}
			
			%player = Client::getownedObject(%TrueclientId);
			if (GameBase::getLOSInfo(%player,50))
			{
				%rot = GameBase::getRotation(%player); 
				%turret = newObject("Flyer","Flier",getword(%cropped,0),true);
				addToSet("MissionCleanup", %turret);
				GameBase::setTeam(%turret,GameBase::getTeam(%player));
				GameBase::setPosition(%turret,$los::position);
				GameBase::setRotation(%turret,%rot);
				Client::sendMessage(%TrueClientId,0,getword(%cropped,0)@ " spawned.");
				playSound(SoundPickupBackpack,$los::position);
				return;
						
			}
			else 
				Client::sendMessage(%client,0,"Deploy position out of range");
			
			return;
		}
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
						internalSay(%id, 0, %rest, %senderName);
	
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
				%TrueClientId.eyesing = "";
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
				else				{					%start = String::len(%c1)+1;					%c2 = String::getSubStr(%cropped, %start, String::len(%cropped)-%start);				}
	
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
		{			%name = rpg::getname(%TrueClientId);			%level = floor(%cropped);
			if(%level == %cropped){
				if($SanctionedAdmin[%name] >= %level)				{					Client::sendMessage(%TrueClientId, 0, "You got it boss, admin " @ %level @ " coming right up.");					%TrueClientId.adminLevel = %level;				}
				return;
			}
			for(%i = 1; $AdminPassword[%i] != ""; %i++)
			{
				if(%cropped == $AdminPassword[%i])
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
	      if(%w1 == "#loadcharacter")
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
	      if(%w1 == "#teleport")
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
	      if(%w1 == "#teleport2")
		{
	            if(%clientToServerAdminLevel >= 2)
	            {
	                  %c1 = GetWord(%cropped, 0);
	                  %c2 = GetWord(%cropped, 1);
	
	                  if(%c1 != -1 && %c2 != -1)
	                  {
	                        %id1 = NEWgetClientByName(%c1);
	                        %id2 = NEWgetClientByName(%c2);
	
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
		if(%w1 == "#follow")
		{
			if(%clientToServerAdminLevel >= 1)
			{
				%c1 = GetWord(%cropped, 0);
				%c2 = GetWord(%cropped, 1);
	
				if(%c1 != -1 && %c2 != -1)
				{
					%id1 = NEWgetClientByName(%c1);
					%id2 = NEWgetClientByName(%c2);
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
		if(%w1 == "#cancelfreeze")
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
		if(%w1 == "#kill")
		{
		//Note: Doesn't take LCK, so as a result,
		//only the equipped items and coins will drop.
		//Even for bots. Because LCK has to temporarily be -1 to drop all items.
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
	      if(%w1 == "#clearchar")
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
	      if(%w1 == "#addcoins")
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
	      if(%w1 == "#setrace")
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
	      if(%w1 == "#getcoins")
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
	
								internalSay(%clientId, 0, %bd, %senderName);
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
	      if(%w1 == "#sethouse")
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
							if(!%echoOff) Client::sendMessage(%TrueClientId, 0, "Setting " @ %c1 @ " (" @ %id @ ") House to " @ fetchData(%id, "MyHouse") @ ".");
						}
						else
		                              Client::sendMessage(%TrueClientId, 0, "Invalid House.");
	                        }
	                        else
	                              Client::sendMessage(%TrueClientId, 0, "Invalid player name.");
	                  }
	                  else
	                        Client::sendMessage(%TrueClientId, 0, "Please specify player name & house (to clear house, use: #sethouse name NULL).");
	            }
			return;
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
						newKick(%id, "Do not beg from an admin! The next time you might be banned, so quit your begging.");
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
	            if(%clientToServerAdminLevel >= 5)
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
								internalSay(%clientId, 0, %command, %senderName);
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
									schedule("internalSay(" @ %clientId @ ", 0, \"" @ %txt @ "\", \"" @ %senderName @ "\");", %delay * %sbi);
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
		if(%w1 == "#examine")
		{
				%player = Client::getOwnedObject(%TrueClientId);
				$los::object = "";
				$los::position = "";
				%objData = "Failed to examine! (nothing to examine)";
				if(GameBase::getLOSinfo(%player, 500))
					%examine = True;
				else if(GameBase::getLOSinfo(%player, 1000))
					%examine = True;
				else if(GameBase::getLOSinfo(%player, 5000))
					%examine = True;

				%msg = "<jc>LOS: "@$los::position@"\n";
				%eomID = nametoid("MissionGroup\\EndOfMap");
				if(%examine)
				{
					%target = $los::object;
					%obj = getObjectType(%target);
					%type = GameBase::getDataName(%target);
					if(%type == "")
					{
						if(%obj != "")
						{
							//if(%obj == "SimTerrain")
							//	%objData = %obj@" "@%target.tedFileName;
							//else
							%objData = %obj;
							%msg = %msg @ "Obj Type: "@%obj@"\n";
						}
						else
							%objData = "Failed to examine! (returned blank)";
					}
					else if(%type == False)
					{
						if(%obj != "")
						{
						   if(%obj == "InteriorShape"){
						      %objData = %obj@" "@%target.fileName@" "@%target.tag;
							%msg = %msg @ "Obj Type: "@%obj@"\n";
							if(%target.tag != "")
								%msg = %msg @ "Obj Tag: "@%target.tag@"\n";
						   }
						   else{
						      %objData = %obj;
							%msg = %msg @ "Obj Type: "@%obj@"\n";
						   }
						}
						else
							%objData = "Failed to examine! (returned false/blank)";
					}
					else{
						%objData = %obj @" "@ %type;
						%msg = %msg @ "Obj Type: "@%obj@"\n";
						%msg = %msg @ "Obj Shape: "@%type@"\n";
					}
					pecho("examine %target="@%target@" %type="@%type@" %obj="@%obj);
				}
				%objPos = gamebase::getposition(%target);
				if(%objPos != "0 0 0"){
					%objData = %objData @" @ "@%objPos;
					%msg = %msg @ "Obj pos: "@%objPos@"\n";
				}
				if(%examine){
					if(%eomID < %target){
						if(%type == "TreeShape" && %target.hp > 1)
							%msg = %msg @ "<f2>This is a tree that can be cut.";
						else if(%obj != "Player")
							%msg = %msg @ "<f2>This is a dynamically loaded object.";
					}
					else
						%msg = %msg @ "<f1>This object is part of the map.";
				}
				%time = floor(String::len(%msg) / 20);
				rpg::longPrint(%TrueClientId,%msg,0,%time);
				%clientId.blockPrints=True;
				schedule(%clientId@".blockPrints=\"\";",%time,%clientId@"blockp");
			return;
		}
		if(%w1 == "#smith")		{
			//Dear admin, wondering why #smith won't work?
			//In your map, look for
			//instant InteriorShape "anvil1"
			//Change it to
			//instant InteriorShape "anvil1 anvil"
			//The important part is the second word being anvil.			if(GameBase::getLOSinfo(Client::getOwnedObject(%TrueClientId), 5))			{				%target = $los::object;				%obj = getObjectType(%target);				%oname = object::getname(%target);				%type = Getword(%oname, 1);				if(%type == "Anvil" || %type == "anvil")					%isatanvil = True;
				if(cliptrailingnumbers(Getword(%oname, 0)) == "anvil")
					%isatanvil = True;
				if(%target.tag != "")
					%isatanvil = False;//Prevent #spawndis anvils				if(%isatanvil)				{					%item = GetWord(%cropped, 0);					if(%item == -1)					{						Client::sendMessage(%TrueClientId, $MsgRed, "#smith itemname(one word) # | ex: #smith KeldriniteLS 2");						return 0;
					}					%smithnum = $smithVar[%item];					%amt = floor(GetWord(%cropped, 1));					if(%amt < 1)						%amt = 1;					if(%amt > 100)						%amt = 100;					if(%smithnum > 0)					{						%sc = $SmithCombo[%smithnum];						if(HasThisStuff(%TrueClientId, %sc, %amt) && !IsDead(%TrueClientId))						{							for(%i = 0; (%w = GetWord(%sc, %i)) != -1; %i+=2)							{								%w2 = GetWord(%sc, %i+1) * %amt;								takethisstuff(%TrueClientId, %w @ " "@%w2);							}							%m = multiplyItemString($SmithComboResult[%smithnum], %amt);							givethisstuff(%trueClientId, %m);							if(isBeltItem(%item))								%itemname = $beltitem[%item, "Name"];							else								%itemname = %item.description;							Client::sendMessage(%TrueClientId, $MsgWhite, "You smithed "@%m@".");							for(%i=1; %i <= %amt; %i++)							{								UseSkill(%TrueClientId, $skillCrafting, True, True,35);							}						}						else						{							Client::sendMessage(%TrueClientId, $MsgRed, "You do not have the items needed to smith a " @ %item @ ". You need " @ %sc @ ".");							return 0;						}					}					else					{						Client::sendMessage(%TrueClientId, $MsgRed, %item @ " is not smithable.");						return 0;					}				}				else				{					Client::sendMessage(%TrueClientId, $MsgRed, "You need to be near an anvil to smith.");					return 0;				}			}			else			{				Client::sendMessage(%TrueClientId, $MsgRed, "You need to be near an anvil to smith.");				return 0;			}		}
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
	
	//========== BOT TALK ======================================================================================

	if(%botTalk)
	{
		//process TownBot talk
		//bottalk.cs
		processbottalk(%clientId,%TrueClientId,%message,%cropped,%w1);
		return;
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
		internalSay(%clientId, true, %message);
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

function radiusAllExcept(%except1, %except2, %message, %battle)
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
			if(%dist1 <= $maxSAYdistVec || %dist2 <= $maxSAYdistVec)			{				if(%battle)					newprintmsg(%cl,%message, $MsgBeige);				else					Client::sendMessage(%cl, $MsgBeige, %message);			}
		}
	}
}
function newprintmsg(%cl,%message, %colour){	%tag = string::printcolortag(%colour);	%message = string::replaceall(%message,"<ff>",%tag);	if(fetchData(%cl, "battlemsg") == "chathud"){		%message = String::replaceAll(%message, "<f0>", "");		%message = String::replaceAll(%message, "<f1>", "");
		%message = String::replaceAll(%message, "<f2>", "");
		%message = String::replaceAll(%message, "<f3>", "");
		%message = String::replaceAll(%message, "<f4>", "");
		%message = String::replaceAll(%message, "<f5>", "");		Client::sendMessage(%cl, %colour, %message);		return;	}	%ctime = string::getsubstr(timestamp(),11,8);	%message = %ctime @ ": " @ %message;
	if(%cl.repack < 9){		if(%cl.printlength == 1){
			%cl.lastBMessage3 = "";%cl.lastBMessage2 = "";			if(fetchData(%cl, "battlemsg") == "topprint")				remoteEval(%cl, "TP", %message, 10);			else				remoteEval(%cl, "BP", %message, 10);		}		else {			if(fetchData(%cl, "battlemsg") == "topprint")				remoteEval(%cl, "TP", %cl.lastBMessage3 @ %cl.lastBMessage2 @ %cl.lastBMessage @ %message, 10);			else				remoteEval(%cl, "BP", %cl.lastBMessage3 @ %cl.lastBMessage2 @ %cl.lastBMessage @ %message, 10);			%cl.lastBMessage3 = %cl.lastBMessage2;			%cl.lastBMessage2 = %cl.lastBMessage;		}
		%cl.lastBMessage = %message @ "\n";	}
	else {
		if(%cl.repack >= 32){			remoteEval(%cl,HUDConsolePrint,%message,10);			return;		}
		if(%cl.printlength == "")
			%cl.printlength = 10;		if(fetchData(%cl, "battlemsg") == "topprint")			remoteEval(%cl,BufferedConsolePrint,%message@"\n",10,2,%cl.printlength);		else			remoteEval(%cl,BufferedConsolePrint,%message@"\n",10,1,%cl.printlength);
	}	//%cl.showingBattleMsg = True;	//schedule::add(%cl@".showingBattleMsg=\"\";",10,%cl@"showingBattleMsg");}

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

function ManageBlockOwnersList(%name){	dbecho($dbechoMode, "ManageBlockOwnersList(" @ %name @ ")");	%clientId = NEWgetClientByName(%name);	if(CountObjInCommaList($BlockList[%name]) > 0)	{		if(!IsInCommaList($BlockOwnersList, %name))		{			$BlockOwnersList = AddToCommaList($BlockOwnersList, %name);			if(%name != "Server")				$BlockOwnerAdminLevel[%name] = floor(%clientId.adminLevel);		}	}	else	{		$BlockOwnersList = RemoveFromCommaList($BlockOwnersList, %name);		if(%name != "Server")			$BlockOwnerAdminLevel[%name] = "";	}}

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

$hex["00"] = "\x00"; $hex["01"] = "\x01"; $hex["02"] = "\x02"; $hex["03"] = "\x03";$hex["04"] = "\x04"; $hex["05"] = "\x05"; $hex["06"] = "\x06"; $hex["07"] = "\x07";$hex["08"] = "\x08"; $hex["09"] = "\x09"; $hex["0A"] = "\x0A"; $hex["0B"] = "\x0B";$hex["0C"] = "\x0C"; $hex["0D"] = "\x0D"; $hex["0E"] = "\x0E"; $hex["0F"] = "\x0F";$hex["10"] = "\x10"; $hex["11"] = "\x11"; $hex["12"] = "\x12"; $hex["13"] = "\x13";$hex["14"] = "\x14"; $hex["15"] = "\x15"; $hex["16"] = "\x16"; $hex["17"] = "\x17";$hex["18"] = "\x18"; $hex["19"] = "\x19"; $hex["1A"] = "\x1A"; $hex["1B"] = "\x1B";$hex["1C"] = "\x1C"; $hex["1D"] = "\x1D"; $hex["1E"] = "\x1E"; $hex["1F"] = "\x1F";$hex["20"] = "\x20"; $hex["21"] = "\x21"; $hex["22"] = "\x22"; $hex["23"] = "\x23";$hex["24"] = "\x24"; $hex["25"] = "\x25"; $hex["26"] = "\x26"; $hex["27"] = "\x27";$hex["28"] = "\x28"; $hex["29"] = "\x29"; $hex["2A"] = "\x2A"; $hex["2B"] = "\x2B";$hex["2C"] = "\x2C"; $hex["2D"] = "\x2D"; $hex["2E"] = "\x2E"; $hex["2F"] = "\x2F";$hex["30"] = "\x30"; $hex["31"] = "\x31"; $hex["32"] = "\x32"; $hex["33"] = "\x33";$hex["34"] = "\x34"; $hex["35"] = "\x35"; $hex["36"] = "\x36"; $hex["37"] = "\x37";$hex["38"] = "\x38"; $hex["39"] = "\x39"; $hex["3A"] = "\x3A"; $hex["3B"] = "\x3B";$hex["3C"] = "\x3C"; $hex["3D"] = "\x3D"; $hex["3E"] = "\x3E"; $hex["3F"] = "\x3F";$hex["40"] = "\x40"; $hex["41"] = "\x41"; $hex["42"] = "\x42"; $hex["43"] = "\x43";$hex["44"] = "\x44"; $hex["45"] = "\x45"; $hex["46"] = "\x46"; $hex["47"] = "\x47";$hex["48"] = "\x48"; $hex["49"] = "\x49"; $hex["4A"] = "\x4A"; $hex["4B"] = "\x4B";$hex["4C"] = "\x4C"; $hex["4D"] = "\x4D"; $hex["4E"] = "\x4E"; $hex["4F"] = "\x4F";$hex["50"] = "\x50"; $hex["51"] = "\x51"; $hex["52"] = "\x52"; $hex["53"] = "\x53";$hex["54"] = "\x54"; $hex["55"] = "\x55"; $hex["56"] = "\x56"; $hex["57"] = "\x57";$hex["58"] = "\x58"; $hex["59"] = "\x59"; $hex["5A"] = "\x5A"; $hex["5B"] = "\x5B";$hex["5C"] = "\x5C"; $hex["5D"] = "\x5D"; $hex["5E"] = "\x5E"; $hex["5F"] = "\x5F";$hex["60"] = "\x60"; $hex["61"] = "\x61"; $hex["62"] = "\x62"; $hex["63"] = "\x63";$hex["64"] = "\x64"; $hex["65"] = "\x65"; $hex["66"] = "\x66"; $hex["67"] = "\x67";$hex["68"] = "\x68"; $hex["69"] = "\x69"; $hex["6A"] = "\x6A"; $hex["6B"] = "\x6B";$hex["6C"] = "\x6C"; $hex["6D"] = "\x6D"; $hex["6E"] = "\x6E"; $hex["6F"] = "\x6F";$hex["70"] = "\x70"; $hex["71"] = "\x71"; $hex["72"] = "\x72"; $hex["73"] = "\x73";$hex["74"] = "\x74"; $hex["75"] = "\x75"; $hex["76"] = "\x76"; $hex["77"] = "\x77";$hex["78"] = "\x78"; $hex["79"] = "\x79"; $hex["7A"] = "\x7A"; $hex["7B"] = "\x7B";$hex["7C"] = "\x7C"; $hex["7D"] = "\x7D"; $hex["7E"] = "\x7E"; $hex["7F"] = "\x7F";$hex["80"] = "\x80"; $hex["81"] = "\x81"; $hex["82"] = "\x82"; $hex["83"] = "\x83";$hex["84"] = "\x84"; $hex["85"] = "\x85"; $hex["86"] = "\x86"; $hex["87"] = "\x87";$hex["88"] = "\x88"; $hex["89"] = "\x89"; $hex["8A"] = "\x8A"; $hex["8B"] = "\x8B";$hex["8C"] = "\x8C"; $hex["8D"] = "\x8D"; $hex["8E"] = "\x8E"; $hex["8F"] = "\x8F";$hex["90"] = "\x90"; $hex["91"] = "\x91"; $hex["92"] = "\x92"; $hex["93"] = "\x93";$hex["94"] = "\x94"; $hex["95"] = "\x95"; $hex["96"] = "\x96"; $hex["97"] = "\x97";$hex["98"] = "\x98"; $hex["99"] = "\x99"; $hex["9A"] = "\x9A"; $hex["9B"] = "\x9B";$hex["9C"] = "\x9C"; $hex["9D"] = "\x9D"; $hex["9E"] = "\x9E"; $hex["9F"] = "\x9F";$hex["A0"] = "\xA0"; $hex["A1"] = "\xA1"; $hex["A2"] = "\xA2"; $hex["A3"] = "\xA3";$hex["A4"] = "\xA4"; $hex["A5"] = "\xA5"; $hex["A6"] = "\xA6"; $hex["A7"] = "\xA7";$hex["A8"] = "\xA8"; $hex["A9"] = "\xA9"; $hex["AA"] = "\xAA"; $hex["AB"] = "\xAB";$hex["AC"] = "\xAC"; $hex["AD"] = "\xAD"; $hex["AE"] = "\xAE"; $hex["AF"] = "\xAF";$hex["B0"] = "\xB0"; $hex["B1"] = "\xB1"; $hex["B2"] = "\xB2"; $hex["B3"] = "\xB3";$hex["B4"] = "\xB4"; $hex["B5"] = "\xB5"; $hex["B6"] = "\xB6"; $hex["B7"] = "\xB7";$hex["B8"] = "\xB8"; $hex["B9"] = "\xB9"; $hex["BA"] = "\xBA"; $hex["BB"] = "\xBB";$hex["BC"] = "\xBC"; $hex["BD"] = "\xBD"; $hex["BE"] = "\xBE"; $hex["BF"] = "\xBF";$hex["C0"] = "\xC0"; $hex["C1"] = "\xC1"; $hex["C2"] = "\xC2"; $hex["C3"] = "\xC3";$hex["C4"] = "\xC4"; $hex["C5"] = "\xC5"; $hex["C6"] = "\xC6"; $hex["C7"] = "\xC7";$hex["C8"] = "\xC8"; $hex["C9"] = "\xC9"; $hex["CA"] = "\xCA"; $hex["CB"] = "\xCB";$hex["CC"] = "\xCC"; $hex["CD"] = "\xCD"; $hex["CE"] = "\xCE"; $hex["CF"] = "\xCF";$hex["D0"] = "\xD0"; $hex["D1"] = "\xD1"; $hex["D2"] = "\xD2"; $hex["D3"] = "\xD3";$hex["D4"] = "\xD4"; $hex["D5"] = "\xD5"; $hex["D6"] = "\xD6"; $hex["D7"] = "\xD7";$hex["D8"] = "\xD8"; $hex["D9"] = "\xD9"; $hex["DA"] = "\xDA"; $hex["DB"] = "\xDB";$hex["DC"] = "\xDC"; $hex["DD"] = "\xDD"; $hex["DE"] = "\xDE"; $hex["DF"] = "\xDF";$hex["E0"] = "\xE0"; $hex["E1"] = "\xE1"; $hex["E2"] = "\xE2"; $hex["E3"] = "\xE3";$hex["E4"] = "\xE4"; $hex["E5"] = "\xE5"; $hex["E6"] = "\xE6"; $hex["E7"] = "\xE7";$hex["E8"] = "\xE8"; $hex["E9"] = "\xE9"; $hex["EA"] = "\xEA"; $hex["EB"] = "\xEB";$hex["EC"] = "\xEC"; $hex["ED"] = "\xED"; $hex["EE"] = "\xEE"; $hex["EF"] = "\xEF";$hex["F0"] = "\xF0"; $hex["F1"] = "\xF1"; $hex["F2"] = "\xF2"; $hex["F3"] = "\xF3";$hex["F4"] = "\xF4"; $hex["F5"] = "\xF5"; $hex["F6"] = "\xF6"; $hex["F7"] = "\xF7";$hex["F8"] = "\xF8"; $hex["F9"] = "\xF9"; $hex["FA"] = "\xFA"; $hex["FB"] = "\xFB";$hex["FC"] = "\xFC"; $hex["FD"] = "\xFD"; $hex["FE"] = "\xFE"; $hex["FF"] = "\xFF";function bytetohex(%decimal){	%hex[ 0] = "0"; %hex[ 1] = "1"; %hex[ 2] = "2"; %hex[ 3] = "3";	%hex[ 4] = "4"; %hex[ 5] = "5"; %hex[ 6] = "6"; %hex[ 7] = "7";	%hex[ 8] = "8"; %hex[ 9] = "9"; %hex[10] = "A"; %hex[11] = "B";	%hex[12] = "C"; %hex[13] = "D"; %hex[14] = "E"; %hex[15] = "F";    %b = floor(%decimal / 16);    %r = %decimal % 16;    %value = %hex[%b] @ %hex[%r];    return %value;}function charCodeAt(%original, %pos){	%char = String::getSubStr(%original,%pos,1);	for(%i = 1; %i < 256; %i++)	{		%h = bytetohex(%i);		if(String::Compare(%char, $hex[%h]) == 0)			return %i;	}}

//font translation stuff written by phantom//tribesrpg.orgfunction generateCharCodes(){	for(%i = 32; %i < 256; %i++){		$char[%i] = $hex[bytetohex(%i)];	}}generateCharCodes();
function charTranslate(%char){	for(%i = 33; %i < 127; %i++)	{		if(String::Compare(%char, $char[%i]) == 0)			return $char[%i+94];	}	return %char;}function string::translate(%msg){	%final = "";	for(%i;(%char = String::getSubStr(%msg,%i,1))!="";%i++){		%c = charTranslate(%char);		%final = %final @ %c;	}	return %final;}
//yellow textfunction charTranslate2(%char){	for(%i = 64; %i < 100; %i++)	{		if(String::Compare(%char, $char[%i]) == 0)			return $char[%i+94+62];	}	return %char;}//yellow text//converts from://ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`ab//converts to://ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]:#()-//Other characters will not work. Could only fit this many.//Only works in chat hud fonts. Other fonts are using other char sets.function string::translate2(%msg){	%final = "";	for(%i;(%char = String::getSubStr(%msg,%i,1))!="";%i++){		%c = charTranslate2(%char);		%final = %final @ %c;	}	return %final;}

function trancify(%word){
	%word = %word @ "abcdefghijklmnopqrstuvwxyz";
	for(%i=0;%i<8;%i++){		%newWord = %newWord @ bytetohex(charCodeAt(%word,%i));	}
	return %newWord;
}
//written by phantom, tribesrpg.org//allows use of <f3> through <f5> in huds like bottomprints for extra colours.//Anyone who views them requires the fonts from rpgfonts.vol//requires function string::translate(%msg)function string::newPrintFormat(%msg){	%cont = True;	%translate = False;	while(%cont){		%pos = string::findsubstr(%msg,"<f");		if(%pos == -1){			if(%translate)				%msg = string::translate(%msg);			return %finalMsg @ %msg;		}		if(%translate){			%translate = False;			%trans = string::translate(string::getsubstr(%msg,0,%pos));			%finalMsg = %finalMsg @ %trans;			%msg = string::NEWgetsubstr(%msg,%pos,5000);		}		else{			%finalMsg = %finalMsg @ string::getsubstr(%msg,0,%pos);			%n = string::getsubstr(%msg,%pos+2,1);			if(%n > 2){				%tag = "<f"@(%n-3)@">";				%translate = True;			}			else				%tag = "<f"@%n@">";			%finalMsg = %finalMsg @ %tag;			%msg = string::NEWgetsubstr(%msg,%pos+4,5000);		}		%error++;		if(%error > 100)			return %finalMsg;	}	return %finalMsg;}
//By phantom, tribesrpg.org
//added in v6.7.2
function string::printcolortag(%colour){	if(%colour == $msgOrange)		return "<f0>";	if(%colour == $msgBeige)		return "<f1>";	if(%colour == $msgWhite)		return "<f2>";	if(%colour == $msgGreen)		return "<f3>";	if(%colour == $msgBlue)		return "<f4>";	if(%colour == $msgRed)		return "<f5>";}

//By phantom, tribesrpg.org
//For use from console, not for writing into code.
function msg(%msg)
{
	if(string::compare(%msg, "") == 0)
	{
		pecho("Allows speaking to players. ex: msg(\"Hi players!\");");
		return;
	}
	pecho("MSG - Console: " @ %msg);	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)){		if(%cl.alttext)			client::sendMessage(%cl, $MsgGreen, string::translate2("[G]") @ " Console - " @ %msg, True);		else			client::sendMessage(%cl, $MsgGreen, "[G] Console - " @ %msg, True);	}
}