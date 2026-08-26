exec("game.cs");
$MaxNumKills = 15;
//Deathmatch mission script -Aug. 25 1998

//calc scores upon fraging
function Game::clientKilled(%playerId, %killerId)
{
   if($teamplay)
   {
      if(%killerId == -1 || %playerId == -1)
      {
        return;
      }

      if (%killerId == 0)
      {
        %pteam = Client::getTeam(%playerId);

        for ( %i = 0; %i < $numTeams; %i = %i + 1 )
        {
          if (%i == %pteam)
          {
            // Do Nothing
          }
          else
          {
            $teamScore[%i] = $teamScore[%i] + 1;
          }
        }
        return;
      }

      %kteam = Client::getTeam(%killerId);
      %pteam = Client::getTeam(%playerId);
      
      if(%kteam == %pteam)
      {
         $teamScore[%kteam] = $teamScore[%kteam] - 1;
         MessageAll(0, Client::getName(%killerId) @ " should know better!~wLeftMissionArea.wav");
         MessageAll(0, "Enemies get 1 bonus kill!");

         for ( %i = 0; %i < $numTeams; %i = %i + 1 )
         {
           if (%i == %kteam)
           {
             // Do Nothing
           }
           else
           {
             $teamScore[%i] = $teamScore[%i] + 1;
           }
         }
      }
      else
         $teamScore[%kteam] = $teamScore[%kteam] + 1;
      
      DMTEAM::checkMissionObjectives();
   }
   else
      DM::missionObjectives();
   
}

function Game::playerSpawned(%pl, %clientId, %armor)
{	  
   // use this client's skin preference
   Client::setSkin(%clientId, $Client::info[%clientId, 0]);

  %clientId.spawn= 1;
  %max = $TotalItems;
  if ($Server::RaceOption == "") $Server::RaceOption = 0; 
  %team = (client::getteam(%clientId)+1)%2; //Team Specific Races
  if ($Server::RaceOption == 1) %team = 1; // Space Marines Only
  if ($Server::RaceOption == 2) %team = 0; // Eldar Only
  if ($Server::RaceOption == 3) %team = 2; // All available

  for(%i = 0; (%item = $spawnBuyList[%team, %i]) != ""; %i++) 
  {
    buyItem(%clientId,%item);
    if(%item.className == Weapon) %clientId.spawnWeapon = %item;
  }
  %clientId.spawn= "";
  if(%clientId.spawnWeapon != "") 
  {
    Player::useItem(%pl,%clientId.spawnWeapon);
    %clientId.spawnWeapon="";
  }
  Player::decItemCount(%clientId,RepairPack,1);
  Player::setItemCount(%clientId,DeployableInvPack,1);

   if($teamplay)
   {
      DMTEAM::checkMissionObjectives();
      DMTEAM::echoScores();
   }
}

//Player has a total of 10 seconds per life allowed outside designated mission area.
//After a player expends this 10 sec, the player is remotely killed.
//-lesson to be learned= stay in the mission area!
function Player::leaveMissionArea(%player)
{
   %cl = Player::getClient(%player);
	Client::sendMessage(%cl,1,"You have left the mission area.");
	%player.outArea=1;
	alertPlayer(%player, 10);
}

//checking for timeout of dieSeqCount
function Player::checkLMATimeout(%player, %seqCount)
{
   echo("checking player timeout " @ %player @ " " @ %seqCount);
   if(%player.dieSeqCount == %seqCount)
      remoteKill(Player::getClient(%player));
}

//called if player leaves mission area
function Player::enterMissionArea(%player)
{
   %player.outArea="";
   %player.dieSeqCount = 0;
   %player.timeLeft = %player.timeLeft - (getSimTime() - %player.leaveTime);
}
  
function alertPlayer(%player, %count)
{
	if(%player.outArea == 1) {
		%clientId = Player::getClient(%player);
	  	Client::sendMessage(%clientId,1,"~wLeftMissionArea.wav");
		if(%count > 1)
		   schedule("alertPlayer(" @ %player @ ", " @ %count - 1 @ ");",1.5,%clientId);
		else 
	   	schedule("leaveMissionAreaDamage(" @ %clientId @ ");",1,%clientId);
	}
}

function leaveMissionAreaDamage(%client)
{
  %player = Client::getOwnedObject(%client);
  if(%player.outArea == 1)
    {
    if(!Player::isDead(%player))
    {
      Player::setDamageFlash(%client,0.1);
      GameBase::setDamageLevel(%player,GameBase::getDamageLevel(%player) + 0.05);
      schedule("leaveMissionAreaDamage(" @ %client @ ");",1);
    }
    else
    {
      MessageAll(0, Client::getName(%client) @ " should know better not to leave the mission area.~wLeftMissionArea.wav");

      %pteam = Client::getTeam(%client);

      for ( %i = 0; %i < $numTeams; %i = %i + 1 )
      {
        if (%i == %pteam)
        {
          // Do Nothing
        }
        else
        {
          $teamScore[%i] = $teamScore[%i] + 1;
        }
      }
      playNextAnim(%client);
    }
  }
}

function Game::checkTimeLimit()
{
   // if no timeLimit set or timeLimit set to 0,
   // just reschedule the check for a minute hence
   $timeLimitReached = false;
   $timeReached = "";

   if(!$Server::timeLimit)
   {
      schedule("Game::checkTimeLimit();", 60);
      return;
   }
   %curTimeLeft = ($Server::timeLimit * 60) + $missionStartTime - getSimTime();
   if(%curTimeLeft <= 0)
   {
echo("TIMES UP");
      $timeLimitReached = true;
      $timeReached = 1;

      if ($teamplay) DMTEAM::checkMissionObjectives();
      else DM::checkMissionObjectives();

//      Server::nextMission();
   }
   else
   {
      schedule("Game::checkTimeLimit();", 20);
      UpdateClientTimes(%curTimeLeft);
   }
}

function Vote::changeMission()
{
echo("CHANGING MISSION!");
  $timeLimitReached = true;
  $timeReached = 1;
echo("Set fake time limit reached");
  if ($teamplay) 
  {
echo("Going to call checkMission Objective");
    for(%p = 0; %p < $numTeams; %p = %p + 1) 
    {
      if (DMTEAM::teamMissionObjectives(%p)) echo("Confirmed");
    }
  $timeLimitReached = false;
  $timeReached = "";
  }
  else DM::checkMissionObjectives();
}
  
//---------------------------------------------------------------------------------------
//
//Free for all Deathmatch function definitions
//
//---------------------------------------------------------------------------------------
function DM::checkMissionObjectives(%playerId) 
{
   if(DM::missionObjectives()) 
      schedule("nextMission();", 0);
	if($DMScoreLimit > 0)
		if((Player::getClient(%playerId)).scoreKills >= $DMScoreLimit) {
	      $timeLimitReached = true;
   	   $timeReached = 1;
			DM::missionObjectives();
			Server::nextMission();
		}
}

function DM::missionObjectives()
{
	%numClients = getNumClients();
	for(%i = 0 ; %i < %numClients ; %i++) 
		%clientList[%i] = getClientByIndex(%i);
	%doIt = 1;
	while(%doIt == 1) {
		%doIt = "";
		for(%i= 0 ; %i < %numClients; %i++) {
			if((%clientList[%i]).ratio < (%clientList[%i+1]).ratio) {
				%hold = %clientList[%i];
				%clientList[%i] = %clientList[%i+1];
				%clientList[%i+1]	= %hold;
				%doIt=1;
			}
		}
	}
   if(!$Server::timeLimit)
      %str = "<f1>   - No time limit on the game.";
   else if($timeLimitReached)
      %str = "<f1>   - Time limit reached.";
   else
      %str = "<f1>   - Time remaining: " @ floor($Server::timeLimit - (getSimTime() - $missionStartTime) / 60) @ " minutes.";

	for(%l = -1; %l < 1 ; %l++) {		
		%lineNum = 0;
		if($timeReached == "") {
	 	  	Team::setObjective(%l, %lineNum, "<jc><B0,0:deathmatch1.bmp><B0,0:deathmatch2.bmp>");
	  		Team::setObjective(%l, %lineNum++, "<f5>Mission Information:");
			Team::setObjective(%l, %lineNum++, "<f1>   - Mission Name: " @ $missionName); 
	      Team::setObjective(%l, %lineNum++, %str);
	      Team::setObjective(%l, %lineNum++, " ");
	 	  	Team::setObjective(%l, %lineNum++, "<f5>Mission Objectives:");
	 	  	Team::setObjective(%l, %lineNum++, "<f1>   -Kill all other players!");
	 	  	Team::setObjective(%l, %lineNum++, "<f1>   -Stay alive!");
	 	  	Team::setObjective(%l, %lineNum++, "<f1>   -To have the highest Efficiency!");
	 	  	Team::setObjective(%l, %lineNum++, "<f1>   	 -Efficiency is calculated once (Kills + Deaths) is greater than 4");
	 	  	Team::setObjective(%l, %lineNum++, " ");
	 	  	Team::setObjective(%l, %lineNum++, "<f1>Remember to stay within the mission area, which is defined by the extents of your commander screen map."	@ 
	 	                                 " If you go outside of the mission area you will have 3 seconds to get back into the mission area, or you'll start taking damage!");
	 	  	Team::setObjective(%l, %lineNum++, " ");
	 	  	Team::setObjective(%l, %lineNum++, " ");
		  	Team::setObjective(%l, %lineNum++, "<f5>TOP PLAYERS ARE: " );
	 	  	Team::setObjective(%l, %lineNum++, " ");
	 	  	Team::setObjective(%l, %lineNum++, "<f1>Player Name<L30>Kills<L50>Deaths<L70>Efficiency");
	   }
	   else {
			Team::setObjective(%l, %lineNum++, "<f5>Mission Summary:");
	 	  	Team::setObjective(%l, %lineNum++, " " );
	 	  	Team::setObjective(%l, %lineNum++, "<f1>     - The Best Player(s): " );
			%i=0;
			%TopRatio="";
			while(%i < %numClients && %clientList[%i].ratio != 0 && (%TopRatio == "" || (%TopRatio ==  (%clientList[%i+1]).ratio && %TopRatio != 0) )) {
	 	  		Team::setObjective(%l, %lineNum++, "<L14><f5><Bskull_big.bmp>\n" @ Client::getName(%clientList[%i]) @ "<f1> with a ratio of <f5>" @ (%clientList[%i]).ratio @ ".0%");
				%TopRatio = (%clientList[%i]).ratio;
				%i++;
			}
			if(%i == 0)
	 	  		Team::setObjective(%l, %lineOffset++, "<L14><f1>NONE with a ratio greater than 0.0%");
	 	  	Team::setObjective(%l, %lineNum++, " ");
	 	  	Team::setObjective(%l, %lineNum++, " ");
			Team::setObjective(%l, %lineNum++, "<f5>TOP PLAYERS ARE: " );
	 	  	Team::setObjective(%l, %lineNum++, " ");
			Team::setObjective(%l, %lineNum++, "<f1>Player Name<L30>Kills<L50>Deaths<L70>Efficiency");
		}
	   //print out top 5 scores
		%index = 0;
		while(%index < %numClients && %clientList[%index].ratio != 0 && (%index < 5 || (%clientList[%index].ratio == %lastRatio && %lastRatio != 0))) {
	  		%client = getClientByIndex(%count);
	  	   Team::setObjective(%l, %lineNum++,"<Bskull_small.bmp>" @ Client::getName(%clientList[%index]) @ " <L31>" @ (%clientList[%index]).scoreKills @ "<L53>" @ (%clientList[%index]).scoreDeaths @ "<L72>" @ (%clientList[%index]).ratio @ ".0%");
			%lastRatio = (%clientList[%index]).ratio;
			%index++;
		}  
		for(%s = %lineNum+1; %s < 30 ;%s++)
			Team::setObjective(%l, %s, " ");
	}
	if ($timeLimitReached) return "true";
	else return "false";

	$timeReached="";
}

//-----------------------------------------------------------------
//
//Team Deathmatch Function definitions
// 
//-----------------------------------------------------------------
function DMTEAM::echoScores()
{   
	 %score = getTeamName(0) @ ": " @ $teamScore[0];
	 
	 for(%i = 1; %i < $numTeams; %i = %i + 1)
        %score = %score @ ", " @ getTeamName(%i) @ ": " @ $teamScore[%i];
     
     MessageAll(0, %score);
     schedule("DMTEAM::echoScores();", 600);
}

function DMTEAM::checkMissionObjectives()
{
  for(%p = 0; %p < $numTeams; %p = %p + 1) 
  {
    if(DMTEAM::teamMissionObjectives(%p))
    {
      // For some reasons, this was not cleared when changing missions
      $timeLimitReached = false;
      $timeReached = "";
      schedule("Server::nextMission();", 0);
    }
  }
}

function DMTEAM::teamMissionObjectives(%teamId)
{
  %numHighs = 1;
  %teamName = getTeamName(%teamId);
  %teamScore = $teamScore[%teamId];
  %highScore = 0;

  for(%t = 0; %t < $numTeams; %t = %t + 1)
  {
//    if(%teamScore > $teamScore[%t]) 
//    {
//      %highScore = %teamScore;
//      %numHighs = %numHighs + 1;
//    }
//    else if($teamScore[%t] > %highScore)

    if ($teamScore[%t] > %highScore)
      %highScore = $teamScore[%t];
    else if ($teamScore[%t] == %highScore)
      %numHighs = %numHighs + 1;
  }

  if(%highScore == $ScoreLimit || $timeReached == 1)
  {
//      if((%teamScore == %highScore) && (%numHighs > 1))
//        Team::setObjective(%teamId, 2, "<f5>Your team ended up tied for the lead!");
//      else if(%teamScore == %highScore)
//        Team::setObjective(%teamId, 2, "<f5>Your team is victorious!");
//      else
//        Team::setObjective(%teamId, 2, "<f5>Your team lost!"); 

    if((%teamScore == %highScore) && (%highScore == 0))
      Team::setObjective(%teamId, 2, "<f5>  All teams ended up tied at 0! Weird.");
    else if((%teamScore == %highScore) && (%numHighs > 1)) 
      Team::setObjective(%teamId, 2, "<f5>  Your team ended up tied for the lead!");
    else if((%teamScore == %highScore) && (%numHighs == 1))
      Team::setObjective(%teamId, 2, "<f5>  Your team is victorious!");
    else
      Team::setObjective(%teamId, 2, "<f5>  Your team lost!");

    Team::setObjective(%teamId, 3, "\n");

    //print out all team scores
    %lnum = 4;
    for(%q = 0; %q < $numTeams; %q = %q + 1) 
    {
      Team::setObjective(%teamId, %lnum, "<f5>" @ getTeamName(%q) @ ": " @ $teamScore[%q]);
      %lnum = %lnum + 1;
    }

    // Cleanup left over lines from old screen
    for (%xxx = %lnum; %xxx < 14 + $numTeams; %xxx = %xxx + 1)
      Team::setObjective(%teamId, %xxx, "");

    return "True"; //change mission
  }
  
  Team::setObjective(%teamId, 3, "\n");
  Team::setObjective(%teamId, 4, "<f5>Mission Status:"); 													    
   
   
  if((%teamScore == %highScore) && (%highScore == 0))
    Team::setObjective(%teamId, 5, "<f1> - All teams tied at 0.");
  else if((%teamScore == %highScore) && (%numHighs > 1)) 
    Team::setObjective(%teamId, 5, "<f1> - Your team is Tied for the lead!");
  else if((%teamScore == %highScore) && (%numHighs == 1))
    Team::setObjective(%teamId, 5, "<f1> - Your team is winning.");
  else
    Team::setObjective(%teamId, 5, "<f1> - Your team is losing.");

  Team::setObjective(%teamId, 6, "\n");
  Team::setObjective(%teamId, 7, "<f5>You must:");
  Team::setObjective(%teamId, 8, "<f1> - Kill all players on all other teams.");
  Team::setObjective(%teamId, 9, "<f1> - Stay alive!");
  Team::setObjective(%teamId, 10, "\n");
  Team::setObjective(%teamId, 11, "<f1>Remember to stay within the mission area, which is defined by the extents of your commander screen map."	@
                                  " <f1>If you go outside of the mission area you will have 10 seconds to get back into the mission area, or you will be killed!");
  Team::setObjective(%teamId, 12, "\n");

  //print out team scores
  %lnum = 14;
  Team::setObjective(%teamId, 13, "<f5>Team Scores");
  for(%g = 0; %g < $numTeams; %g = %g + 1) 
  {
    Team::setObjective(%teamId, %lnum, "<f1>" @ getTeamName(%g) @ ": " @ $teamScore[%g]);
    %lnum = %lnum + 1;
  }

  return "False";
}												

function TowerSwitch::objectiveInit(%this)
{
   return %this.scoreValue || %this.deltaTeamScore;
}

function TowerSwitch::onAdd(%this)
{
	%this.numSwitchTeams = 0;	
}

function TowerSwitch::onDamage()
{
   // tower switches can't take damage
}

function TowerSwitch::getObjectiveString(%this, %forTeam)
{
   %thisTeam = GameBase::getTeam(%this);
   
   if($missionComplete)
   {
      if(%thisTeam == -1)
         return "<Btowers_neutral.bmp>\nNo team claimed " @ %this.objectiveName @ ".";
      else if(%thisTeam == %forTeam)
         return "<Btower_teamcontrol.bmp>\nYour team finished the mission in control of " @ %this.objectiveName @ ".";
      else {
       	if(%forTeam != -1)
		   	return "<Btower_enemycontrol.bmp>\nThe " @ getTeamName(%thisTeam) @ " team finished the mission in control of " @ %this.objectiveName @ ".";
   		else
		   	return "<Btower_teamcontrol.bmp>\nThe " @ getTeamName(%thisTeam) @ " team finished the mission in control of " @ %this.objectiveName @ ".";
		}
	}
   else
   {
		if(%forTeam != -1) {
     		if(%this.deltaTeamScore)
     		{																	  
				if(%thisTeam == -1)
 			   	return "<Btowers_neutral.bmp>\nClaim " @ %this.objectiveName @ " to gain " @ %this.deltaTeamScore @ " points per minute."; 
 			   else if(%thisTeam == %forTeam)
 			      return "<Btower_teamcontrol.bmp>\nDefend " @ %this.objectiveName @ " to retain " @ %this.deltaTeamScore @ " points per minute.";
 			   else
 			      return "<Btower_enemycontrol.bmp>\nCapture " @ %this.objectiveName @ " from the " @ getTeamName(%thisTeam) @ " team to gain " @ %this.deltaTeamScore @ " points per minute.";
			}
     		else if(%this.scoreValue)
     		{
     			if(%thisTeam == -1)
     		      return "<Btowers_neutral.bmp>\nClaim and defend " @ %this.objectiveName @ " to gain " @ %this.scoreValue @ " points.";
     		   else if(%thisTeam == %forTeam)
     		      return "<Btower_teamcontrol.bmp>\nDefend " @ %this.objectiveName @ " to retain " @ %this.scoreValue @ " points.";
     		   else
     		      return "<Btower_enemycontrol.bmp>\nCapture " @ %this.objectiveName @ " from the " @ getTeamName(%thisTeam) @ " team to gain " @ %this.deltaTeamScore @ " points.";
     		 }
		}
		else {
 			if(%thisTeam == -1)
 			  	return "<Btowers_neutral.bmp>\n" @ %this.objectiveName @ " has not been claimed."; 
 			else
 			   return "<Btower_teamcontrol.bmp>\nThe " @ getTeamName(%thisTeam) @ " team is in control of the " @ %this.objectiveName @ ".";
	  	}
   }
}

function TowerSwitch::onCollision(%this, %object)
{
   //echo("switch collision ", %object);
   if(getObjectType(%object) != "Player")
      return;

   if(Player::isDead(%object))
      return;

   %playerTeam = GameBase::getTeam(%object);
   %oldTeam = GameBase::getTeam(%this);
   if(%oldTeam == %playerTeam)
      return;

//   %this.trainingObjectiveComplete = true;
   
   %playerClient = Player::getClient(%object);
   %touchClientName = Client::getName(%playerClient);
   %group = GetGroup(%this);
   echo("Group :", %group);
   echo("Player Team ", %playerTeam);
   Group::iterateRecursive(%group, GameBase::setTeam, %playerTeam);

   %dropPoints = nameToID(%group @ "/DropPoints");
   %oldDropSet = nameToID("MissionCleanup/TeamDrops" @ %oldTeam);
   %newDropSet = nameToID("MissionCleanup/TeamDrops" @ %playerTeam);

   if(%dropPoints != -1)
   {
      for(%i = 0; (%dropPoint = Group::getObject(%dropPoints, %i)) != -1; %i++)
      {
         if(%oldDropSet != -1)
            removeFromSet(%oldDropSet, %dropPoint);
         addToSet(%newDropSet, %dropPoint);
      }
   }

   if(%oldTeam == -1)
   {
      MessageAllExcept(%playerClient, 1, %touchClientName @ " claimed " @ %this.objectiveName @ " for the " @ getTeamName(%playerTeam) @ " team!~wCapturedTower.wav");
      Client::sendMessage(%playerClient, 1, "You claimed " @ %this.objectiveName @ " for the " @ getTeamName(%playerTeam) @ " team!~wCapturedTower.wav");
 	}
   else
   {
        MessageAllExcept(%playerClient, 1, %touchClientName @ " captured " @ %this.objectiveName @ " from the " @ getTeamName(%oldTeam) @ " team!~wCapturedTower.wav");
        Client::sendMessage(%playerClient, 1, "You captured " @ %this.objectiveName @ " from the " @ getTeamName(%oldTeam) @ " team!~wCapturedTower.wav");
	%this.numSwitchTeams++;	
	schedule("TowerSwitch::timeLimitCheckPoints(" @ %this @ "," @ %playerClient @ "," @ %this.numSwitchTeams @ ");",60);
   }
}

function TowerSwitch::timeLimitCheckPoints(%this,%client,%numChange)
{
   //give player 5 points for capturing tower!
	if(%this.numSwitchTeams == %numChange) {
	   %client.score+=5;
		Game::refreshClientScore(%client);
	   Client::sendMessage(%client, 0, "You receive 5 points for holding your captured tower!");
	}
}

function TowerSwitch::clientKilled(%this, %playerId, %killerId)
{      
   if(!%this.objectiveLine)
      return;

   %killerTeam = Client::getTeam(%killerId);
   %playerTeam = Client::getTeam(%playerId);
   %killerPos = GameBase::getPosition(%killerId);
      
   if(%killerId && (%playerTeam != %killerTeam))
   {   
      %dist = Vector::getDistance(%killerPos, GameBase::getPosition(%this));
      //echo(%dist);
      if(%dist <= 80)
      {
         //echo("distance to objective" @ %this @ " : " @ %dist);
         if(GameBase::getTeam(%this) == Client::getTeam(%killerId) && getObjectType(%killerId) == "Player")
         {
            %killerId.score++;
            Game::refreshClientScore(%killerId);
            messageAll(0, strcat(Client::getName(%killerId), " receives a bonus for defending " @ %this.objectiveName @ "."));
         }
      }
   }
}


function getEfficiencyRatio(%clientId)
{
	if((%clientId.scoreKills + %clientId.scoreDeaths) > 4) {
		%ratio = floor((%clientId.scoreKills/(%clientId.scoreKills + %clientId.scoreDeaths))*100);		
		return %ratio;
	}
	return "0";
}

function Game::refreshClientScore(%clientId)
{
	%clientId.ratio = getEfficiencyRatio(%clientId);
   if($teamplay)
   {
      Client::setScore(%clientId, "%n\t%t\t" @ %clientId.score  @ "\t%p\t%l", %clientId.score);
      DMTEAM::checkMissionObjectives();
   }
   else
   {
      Client::setScore(%clientId, "%n\t " @ %clientId.scoreKills @ "\t  " @ %clientId.scoreDeaths @ "\t  " @ %clientId.ratio @ ".0%\t%p\t %l", %clientId.ratio);
      DM::missionObjectives();
   }
}

function ObjectiveMission::refreshTeamScores()
{
   %nt = getNumTeams();
   Team::setScore(-1, "%t\t  0", 0);
   for(%i = 0; %i < %nt; %i++)
   {
      Team::setScore(%i, "%t\t  " @ $teamScore[%i], $teamScore[%i]);
   }
}

function Mission::init()
{
   echo("Starting mission!");
   setClientScoreHeading("Player Name\t\x78Team\t\xC8Score");

   $numTeams = getNumTeams();
   for(%i = 0; %i < $numTeams; %i++)
   {
      $teamScore[%i] = 0;
      newObject("TeamDrops" @ %i, SimSet);
      addToSet(MissionCleanup, "TeamDrops" @ %i);
      %dropSet = nameToID("MissionGroup/Teams/Team" @ %i @ "/DropPoints/Random");
      for(%j = 0; (%dropPoint = Group::getObject(%dropSet, %j)) != -1; %j++)
         addToSet("MissionCleanup/TeamDrops" @ %i, %dropPoint);
   }

   if($teamplay = !($numTeams == 1))
   {
//      setTeamScoreHeading("Team Name\t\xD6Score");
      setTeamScoreHEading("");
//      setClientScoreHeading("Player Name\t\x78Team\t\xC8Score");
      setClientScoreHeading("Player Name\t\x6FTeam\t\xA6Score\t\xCFPing\t\xEFPL");
      DMTEAM::checkMissionObjectives();
   }
   else
   {
      $SensorNetworkEnabled = false;
      setTeamScoreHeading("");
      setClientScoreHeading("Player Name\t\x55Kills\t\x75Deaths\t\xA5Efficiency\t\xE3Ping\t\xFFPL");
      DM::missionObjectives();
   }
   $dieSeqCount = 0;
}

function Game::pickRandomSpawn(%team)
{
   %spawnSet = nameToID("MissionCleanup/TeamDrops" @ %team);
   %spawnCount = Group::objectCount(%spawnSet);
   if(!%spawnCount)
      return -1;
  	%spawnIdx = floor(getRandom() * (%spawnCount - 0.1));
  	%value = %spawnCount;
	for(%i = %spawnIdx; %i < %value; %i++) {
		%set = newObject("set",SimSet);
		%obj = Group::getObject(%spawnSet, %i);
		if(containerBoxFillSet(%set,$SimPlayerObjectType|$VehicleObjectType,GameBase::getPosition(%obj),2,2,4,0) == 0) {
			deleteObject(%set);
			return %obj;		
		}
		if(%i == %spawnCount - 1) {
			%i = -1;
			%value = %spawnIdx;
		}
		deleteObject(%set);
	}
   return false;
}
