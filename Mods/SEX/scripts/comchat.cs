$MsgTypeSystem = 0;
$MsgTypeGame = 1;
$MsgTypeChat = 2;
$MsgTypeTeamChat = 3;
$MsgTypeCommand = 4;

function remoteSay(%clientId, %team, %message)
{
	%msg = %clientId @ " \"" @ escapeString(%message) @ "\"";
	for(%i = 0; (%word = getWord(%message, %i)) != -1; %i++)
	{
		if(String::findSubStr(%word, "god") == 0)
			%blasphemyg = true;
		if(String::findSubStr(%word, "damn") == 0)
			%blasphemyd = true;
	}
	if(%blasphemyg && %blasphemyd)
	{
		remoteKill(%clientId);
		%message = "I have been struck down for my sin of blasphemy!";
		//return;
	}
			
	if(%clientId.silenced)
	{
		Client::sendMessage(%clientId, 1, "You have been Silenced.~waccess_denied.wav");
		return;
	}

	// check for flooding if it's a broadcast OR if it's team in FFA
	if($Server::FloodProtectionEnabled && (!$Server::TourneyMode || !%team) && !%clientId.isSuperAdmin) // Allow Super Admins to Flood Messages
	{
		// we use getIntTime here because getSimTime gets reset.
		// time is measured in 32 ms chunks... so approx 32 to the sec
		%time = getIntegerTime(true) >> 5;
		if(%clientId.floodMute)
		{
			%delta = %clientId.muteDoneTime - %time;
			if(%delta > 0)
			{
				Client::sendMessage(%clientId, $MSGTypeGame, "FLOOD! You cannot talk for " @ %delta @ " seconds.");
				return;
			}
			%clientId.floodMute = "";
			%clientId.muteDoneTime = "";
		}
		%clientId.floodMessageCount++;
		// funky use of schedule here:
		schedule(%clientId @ ".floodMessageCount--;", 5, %clientId);
		if(%clientId.floodMessageCount > 4)
		{
			%clientId.floodMute = true;
			%clientId.muteDoneTime = %time + 10;
			Client::sendMessage(%clientId, $MSGTypeGame, "FLOOD! You cannot talk for 10 seconds.");
			return;
		}
	}
	
	if(string::getsubstr(%message, 0, 1) == "!" && %clientId.isAdmin) // Allow Admins to Bottom and Center print messages
	{
		if(string::getsubstr(%message, 1, 1) == "!" && %clientId.isAdmin)
		{
			%message = string::getsubstr(%message, 2, 999);
			centerprintall("<jc><f2>" @ %message, 8);
			return;
		}
		else
		{
			%message = string::getsubstr(%message, 1, 999);
			bottomprintall("<jc><f2>" @ %message, 8);
			return;
		}
	}
	if(string::getsubstr(%message, 0, 1) == "=" && string::getsubstr(%message, 2, 1) != "") // Whisper Talking
	{
		if(%clientId.whisper != "" && (Client::getTeam(%clientId) != -1 || %clientId.isSuperAdmin)) 
		{
			if(%clientId.isAdmin) 
			{
				if(string::getsubstr(%message, 1, 1) == "!" && %clientId.isSuperAdmin) 
				{
					%message = string::getsubstr(%message, 2, 999);
					centerprint(%clientId.whisper, "<jc><f0>" @ Client::getName(%clientID) @ " (private): <f1>" @ %message, 8);
				}
				Client::sendMessage(%clientId.whisper, $MsgTypeChat, Client::getName(%clientID) @ " (private): " @ %message);
				Client::sendMessage(%clientId, $MsgTypeChat, Client::getName(%clientID) @ " (to " @ Client::getName(%clientId.whisper) @ "): " @ %message);
			}
			else if((Client::getTeam(%clientId) != -1 || $Game::missionType == "Duel") && !(%clientId.whisper).muted[%clientId]) 
			{
				if(!(%clientId.whisper).isSuperAdmin && Client::getTeam(%clientId) != Client::getTeam(%clientId.whisper))
					cheatAdminMsg(Client::getName(%clientID)@" (to "@Client::getName(%clientId.whisper)@"): "@%message);
				if(!(%clientId.whisper).isSuperAdmin && $listen[%clientId] != "")
					Client::sendMessage($listen[%clientId], 1, Client::getName(%clientID)@" (to "@Client::getName(%clientId.whisper)@"): "@%message);
				Client::sendMessage(%clientId.whisper, $MsgTypeChat, Client::getName(%clientID)@" (private): "@%message);
				Client::sendMessage(%clientId, $MsgTypeChat, Client::getName(%clientID)@" (to "@Client::getName(%clientId.whisper)@"): "@%message);
			}
			%message = escapeString(%message);
			echo("SAY: " @ %clientId @ " \""@Client::getName(%clientID)@" (to "@Client::getName(%clientId.whisper)@"): "@%message@"\"");
			return;
		}
	}
	if(%team)
	{
		if($dedicated)
			echo("SAYTEAM: " @ %msg);
		%team = Client::getTeam(%clientId);
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			if(Client::getTeam(%cl) == %team && !%cl.muted[%clientId])
				Client::sendMessage(%cl, $MsgTypeTeamChat, %message, %clientId);
	}
	else
	{
		if($dedicated)
			echo("SAY: " @ %msg);
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			if(!%cl.muted[%clientId])
				Client::sendMessage(%cl, $MsgTypeChat, %message, %clientId);
	}
}

function remoteIssueCommand(%commander, %cmdIcon, %command, %wayX, %wayY,
      %dest1, %dest2, %dest3, %dest4, %dest5, %dest6, %dest7, %dest8, %dest9, %dest10, %dest11, %dest12, %dest13, %dest14)
{
   	if($dedicated)
      	echo("COMMANDISSUE: " @ %commander @ " \"" @ escapeString(%command) @ "\"");
   	// issueCommandI takes waypoint 0-1023 in x,y scaled mission area
   	// issueCommand takes float mission coords.
   	for(%i = 1; %dest[%i] != ""; %i = %i + 1)
      	if(!%dest[%i].muted[%commander])
      	{
      		issueCommandI(%commander, %dest[%i], %cmdIcon, %command, %wayX, %wayY);
        	//echo("Dest: " @ %dest[%i]);
      		%waypoint= %WayX @ " " @ %WayY;   
        	//Client::sendMessage(%dest[%i],1,"Dest: " @ %dest[%i] @ " " @ %i);
        	%commandee = %dest[%i];
        	$point[%commandee] = %waypoint;
        	//Client::sendMessage(%dest[%i],1,"Waypoint: " @ $point[%commandee]);
      	}
      	    
}

function remoteIssueTargCommand(%commander, %cmdIcon, %command, %targIdx, 
      %dest1, %dest2, %dest3, %dest4, %dest5, %dest6, %dest7, %dest8, %dest9, %dest10, %dest11, %dest12, %dest13, %dest14)
{
   if($dedicated)
      echo("COMMANDISSUE: " @ %commander @ " \"" @ escapeString(%command) @ "\"");
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
   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
      if(%cl != %except)
         Client::sendMessage(%cl, %mtype, %message);
}

