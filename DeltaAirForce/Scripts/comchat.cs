$MsgTypeSystem = 0;
$MsgTypeGame = 1;
$MsgTypeChat = 2;
$MsgTypeTeamChat = 3;
$MsgTypeCommand = 4;

//function TypoCheck(%word)
//{
//	if(%word == "nigger" || %word == "NIGGER" || %word == "Nigger" || %word == "nigga" || %word == "NIGGA" || %word == "Nigga" || %word == "Nigger!" || %word == "NIGGER!" || %word == "nigger!" || %word == "Nigga!" || %word == "NIGGA!" || %word == "nigga!" || %word == "NiGga" || %word == "niggre!" || %word == "NIGGER?" || %word == "Nigga?" || %word == "NIGGA?" || %word == "Nigger?" || %word == "nigger?" || %word == "nigga?")
//		return nigger;
//	else if(%word == "whitey" || %word == "WHITEY" || %word == "Whitey" || %word == "whitee" || %word == "yt" || %word == "whytee" || %word == "YTee!" || %word == "YT!" || %word == "WHITEY!" || %word == "Whitey!" || %word == "whitey!")
//		return whitey;
//	else if(%word == "fag" || %word == "f@g" || %word == "F@g" || %word == "F@G" || %word == "faggot" || %word == "fagot" || %word == "Fag" || %word == "Faggot" || %word == "Fagot" || %word == "FAG" || %word == "FAGGOT" || %word == "FAGOT" || %word == "FAG!" || %word == "FAGGOT!" || %word == "FAGOT!" || %word == "FAG." || %word == "FAGGOT." || %word == "FAGOT." || %word == "Fag!" || %word == "Faggot!" || %word == "fag." || %word == "faggot!" || %word == "Fagot!")
//		return fag;
//	else if(%word == "gay" || %word == "g@y" || %word == "G@y" || %word == "G@Y" || %word == "G@Y!" || %word == "GAY!" || %word == "Gay!" || %word == "gay!" || %word == "Ghay" || %word == "GHAY" || %word == "ghay" || %word == "Ghay!" || %word == "ghay!" ||  || %word == "Ghey" || %word == "GHEY" || %word == "ghey" || %word == "Ghey!" || %word == "ghey!")
//		return gay;
//	else
//		return %word;
//}

function Kickdabitch(%client)
{
	net::kick(%client, "Hrm nope, looks like it didnt work");
}

function chat(%message)
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
        	Client::sendMessage(%cl,0,%message);
	echo(">>> " @%message);

}

function remoteSay(%clientId, %team, %message)
{
   %msg = %clientId @ " \"" @ escapeString(%message) @ "\"";
 	

   if(String::findSubStr(%message,"nigguh") != -1 || String::findSubStr(%message,"nigger") != -1 || String::findSubStr(%message, "NIGGER") != -1 || String::findSubStr(%message, "Nigger") != -1 || String::findSubStr(%message, "nigga") != -1 || String::findSubStr(%message, "NIGGA") != -1 || String::findSubStr(%message, "Nigga") != -1 || String::findSubStr(%message, "Nigger!") != -1 || String::findSubStr(%message, "NIGGER!") != -1 || String::findSubStr(%message, "nigger!") != -1 || String::findSubStr(%message, "Nigga!") != -1 || String::findSubStr(%message, "NIGGA!") != -1 || String::findSubStr(%message, "nigga!") != -1 || String::findSubStr(%message, "NiGga") != -1 || String::findSubStr(%message, "niggre!") != -1 || String::findSubStr(%message, "NIGGER?") != -1 || String::findSubStr(%message, "Nigga?") != -1 || String::findSubStr(%message, "NIGGA?") != -1 || String::findSubStr(%message, "Nigger?") != -1 || String::findSubStr(%message, "nigger?") != -1 || String::findSubStr(%message, "nigga?") != -1
	|| String::findSubStr(%message, "whitey") != -1 || String::findSubStr(%message, "WHITEY") != -1 || String::findSubStr(%message, "Whitey") != -1 || String::findSubStr(%message, "whitee") != -1 || String::findSubStr(%message, "whytee") != -1 || String::findSubStr(%message, "YTee!") != -1 || String::findSubStr(%message, "YT!") != -1 || String::findSubStr(%message, "WHITEY!") != -1 || String::findSubStr(%message, "Whitey!") != -1 || String::findSubStr(%message, "whitey!") != -1
	|| String::findSubStr(%message, "fag") != -1 || String::findSubStr(%message, "f@g") != -1 || String::findSubStr(%message, "F@g") != -1 || String::findSubStr(%message, "F@G") != -1 || String::findSubStr(%message, "faggot") != -1 || String::findSubStr(%message, "fagot") != -1 || String::findSubStr(%message, "Fag") != -1 || String::findSubStr(%message, "Faggot") != -1 || String::findSubStr(%message, "Fagot") != -1 || String::findSubStr(%message, "FAG") != -1 || String::findSubStr(%message, "FAGGOT") != -1 || String::findSubStr(%message, "FAGOT") != -1 || String::findSubStr(%message, "FAG!") != -1 || String::findSubStr(%message, "FAGGOT!") != -1 || String::findSubStr(%message, "FAGOT!") != -1 || String::findSubStr(%message, "FAG.") != -1 || String::findSubStr(%message, "FAGGOT.") != -1 || String::findSubStr(%message, "FAGOT.") != -1 || String::findSubStr(%message, "Fag!") != -1 || String::findSubStr(%message, "Faggot!") != -1 
	|| String::findSubStr(%message, "fag.") != -1 || String::findSubStr(%message, "faggot!") != -1 || String::findSubStr(%message, "Fagot!" ) != -1
	|| String::findSubStr(%message, "gay") != -1 || String::findSubStr(%message, "g@y") != -1 || String::findSubStr(%message, "G@y") != -1 || String::findSubStr(%message, "G@Y") != -1 || String::findSubStr(%message, "G@Y!") != -1 || String::findSubStr(%message, "GAY!") != -1 || String::findSubStr(%message, "Gay!") != -1 || String::findSubStr(%message, "gay!") != -1 || String::findSubStr(%message, "Ghay") != -1 || String::findSubStr(%message, "GHAY") != -1 || String::findSubStr(%message, "ghay") != -1 || String::findSubStr(%message, "Ghay!") != -1 || String::findSubStr(%message, "ghay!") != -1 || String::findSubStr(%message, "Ghey") != -1 || String::findSubStr(%message, "GHEY") != -1 || String::findSubStr(%message, "ghey") != -1 || String::findSubStr(%message, "Ghey!") != -1 || String::findSubStr(%message, "ghey!") != -1
	)
   {
	exec("RacistBastards.cs");
	%ip = Client::getTransportAddress(%clientId);
	%name = Client::getname(%clientId);
	$ExportNum++;
	$ExportThis::[$ExportNum] = %name @ " at " @ %ip @ " says***: " @ %message;
	Export( "Export*", "config\\RacistBastards.cs", False );
	DecreaseHappyness(1);
	echo("LOGGING MESSAGE");
 
   }



	if(%clientId.notalk == true)
		return;

		


  // if(String::findSubStr(%message, "condom") == 0 || String::findSubStr(%message, "an someone") == 0)
   //{
//	echo("Passed");
	//schedule("KickdaBitch(" @ %clientId @ ");", 0.1, %clientId);   
   //} 

   // check for flooding if it's a broadcast OR if it's team in FFA
   if($Server::FloodProtectionEnabled && (!$Server::TourneyMode || !%team))
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
      schedule(%clientId @ ".floodMessageCount--;", 10, %clientId);
      if(%clientId.floodMessageCount > 4)
      {
         %clientId.floodMute = true;
         %clientId.muteDoneTime = %time + 10;
         Client::sendMessage(%clientId, $MSGTypeGame, "FLOOD! You cannot talk for 10 seconds.");
         return;
      }
   }
   
   if(%team)
   {
      if($dedicated)
         if($STEAMTALK)
		echo("SAYTEAM: " @ %msg);
      %team = Client::getTeam(%clientId);
      for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
         if(Client::getTeam(%cl) == %team && !%cl.muted[%clientId])
            Client::sendMessage(%cl, $MsgTypeTeamChat, %message, %clientId);
   }
   else if(!$NoTalk)
   {
	if(String::findSubStr(%message,"~w") != -1)
	{
		Client::sendMessage(%clientId, $MSGTypeGame, "Thats too noisy for global chat.");
		return;
	}
	if($LastMessage[%clientId] == %msg) 
	   {
		 Client::sendMessage(%clientId, $MSGTypeGame, "I think we heard you the last time Cowboy.");
	         return;
	   } else
		$LastMessage[%clientId] = %msg;


      if($dedicated)
	if($STALK)
        	echo("SAY: " @ %msg);
      for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
         if(!%cl.muted[%clientId])
            Client::sendMessage(%cl, $MsgTypeChat, %message, %clientId);
   } else if($NoTalk) {
	Client::sendMessage(%clientId, $MSGTypeGame, "Global chat has been turned off to encourage teamwork.");
	         
   }
}

function remoteIssueCommand(%commander, %cmdIcon, %command, %wayX, %wayY, %dest1, %dest2, %dest3, %dest4, %dest5, %dest6, %dest7, %dest8, %dest9, %dest10, %dest11, %dest12, %dest13, %dest14)
{
	if($dedicated) {
		if($STEAMTALK) {
			echo("COMMANDISSUE: " @ %commander @ " \"" @ escapeString(%command) @ "\"");
		}
	}
	// issueCommandI takes waypoint 0-1023 in x,y scaled mission area
	// issueCommand takes float mission coords.
	for(%i = 1; %dest[%i] != ""; %i = %i + 1) {
		if(!%dest[%i].muted[%commander]) {
			issueCommandI(%commander, %dest[%i], %cmdIcon, %command, %wayX, %wayY);
			$LastPlayerWayPoint[%commander] = WaypointToWorld(%wayX@" "@%wayY);
			echo($LastPlayerWayPoint[%commander]);
		}
	}
}

function remoteIssueTargCommand(%commander, %cmdIcon, %command, %targIdx, 
      %dest1, %dest2, %dest3, %dest4, %dest5, %dest6, %dest7, %dest8, %dest9, %dest10, %dest11, %dest12, %dest13, %dest14)
{
   if($dedicated)
	if($STEAMTALK)
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
	if($STEAMTALK)
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

