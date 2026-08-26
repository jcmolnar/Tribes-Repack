// Siege mission type by Plasmatic for Annihilation Mod
// www.annihilation.info

// to use, this needs to go into the mods folder
// and be executed in the .mis file right after objectives at the end.
// example:

//	//--- export object end ---//
//	$teamScoreLimit = 8;
//	exec(objectives);
//	exec(siege);
//	$Game::missionType = "SIEGE";
//	$cdTrack = 8;
//	$cdPlayMode = 1;

// In the maps .dsc file change mission type to look like this..

//	$MDESC::Type = "Siege";

// Only works for 2 team maps, switche may be placed in team folders or alone
// if placed in a team dir. place in own 'group' otherwise will control drop points, inventories, etc.
// no need to assign a team at maps start.
// ONE SWITCH per map.


$switchPos = "";
$switchFirstTeam = "";


function TowerSwitch::onAdd(%this)
{
	%this.numSwitchTeams = 0;
	if (GameBase::getTeam(%this) != -1) $switchFirstTeam = GameBase::getTeam(%this);
}
function TowerSwitch::objectiveInit(%this)
{
	return %this.scoreValue || %this.deltaTeamScore;
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
		else 
		{
			if(%forTeam != -1)
				return "<Btower_enemycontrol.bmp>\nThe " @ getTeamName(%thisTeam) @ " team finished the mission in control of " @ %this.objectiveName @ ".";
			else
				return "<Btower_teamcontrol.bmp>\nThe " @ getTeamName(%thisTeam) @ " team finished the mission in control of " @ %this.objectiveName @ ".";
		}
	}
	else
	{
		if(%forTeam != -1)
		{
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
		else 
		{
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

	%this.trainingObjectiveComplete = true;
	switch::setWaypoints(%this);	
	%playerClient = Player::getClient(%object);
	%touchClientName = Client::getName(%playerClient);
	%group = GetGroup(%this);
	Group::iterateRecursive(%group, GameBase::setTeam, %playerTeam);

	%dropPoints = nameToID(%group @ "/DropPoints");
	%oldDropSet = nameToID("MissionCleanup/TeamDrops" @ %oldTeam);
	%newDropSet = nameToID("MissionCleanup/TeamDrops" @ %playerTeam);

	$deltaTeamScore[%oldTeam] -= %this.deltaTeamScore;
	$deltaTeamScore[%playerTeam] += %this.deltaTeamScore;
	$teamScore[%oldTeam] -= %this.scoreValue;
	$teamScore[%playerTeam] += %this.scoreValue;

	if(%dropPoints != -1)
	{
		for(%i = 0; (%dropPoint = Group::getObject(%dropPoints, %i)) != -1; %i++)
		{
			if(%oldDropSet != -1)
				removeFromSet(%oldDropSet, %dropPoint);
			addToSet(%newDropSet, %dropPoint);
		}
	}
	if($switchFirstTeam  == -1 && %oldTeam != -1) $switchFirstTeam  = %oldTeam;
	
	if(%oldTeam == -1)
	{
		MessageAllExcept(%playerClient, 0, %touchClientName @ " claimed " @ %this.objectiveName @ " for the " @ getTeamName(%playerTeam) @ " team!");
		Client::sendMessage(%playerClient, 0, "You claimed " @ %this.objectiveName @ " for the " @ getTeamName(%playerTeam) @ " team!");
		$switchFirstTeam = %playerTeam;
		TeamMessages(1, %playerTeam, %touchClientName @" has claimed " @ %this.objectiveName @ ". Your team needs to hold it to win the mission.~wCapturedTower.wav");
		%playerClient.score+=10;
		switch::setWaypoints(%this);
		Game::refreshClientScore(%playerClient);	
		return;
 	}
	else
	{
		if(%this.objectiveLine)
		{
			MessageAllExcept(%playerClient, 0, %touchClientName @ " captured " @ %this.objectiveName @ " from the " @ getTeamName(%oldTeam) @ " team!");
			Client::sendMessage(%playerClient, 0, "You captured " @ %this.objectiveName @ " from the " @ getTeamName(%oldTeam) @ " team!");
			%playerClient.score+=5;
			Game::refreshClientScore(%playerClient);
			%this.numSwitchTeams++;
			schedule("TowerSwitch::timeLimitCheckPoints(" @ %this @ "," @ %playerClient @ "," @ %this.numSwitchTeams @ ");",60);
		}
	}
	if($switchFirstTeam == %playerTeam)
	{
	TeamMessages(1, %playerTeam, %touchClientName @" has reclaimed your base.~wCapturedTower.wav");		
	TeamMessages(1, %oldTeam, %touchClientName @" has reclaimed thier base, preventing your team from winning.~wCapturedTower.wav");		
	}
	if ($switchFirstTeam != %playerTeam)
	{
	TeamMessages(1, %playerTeam, "Your team has taken the enemy's base, Hold it for 60 seconds and your team will win.~wCapturedTower.wav");
	TeamMessages(1, %oldTeam, %touchClientName @" has taken your base, They will win in 60 seconds if your team fails to reclaim it.~wCapturedTower.wav");		
		
	}

}

function TowerSwitch::timeLimitCheckPoints(%this,%client,%numChange)
{
	//give player 5 points for capturing tower!
	%team = GameBase::getTeam(%client);
	%switchteam = GameBase::getTeam(%client);
	if(%this.numSwitchTeams == %numChange && %switchteam != $switchFirstTeam)
	{
		%client.score+=50;
		Game::refreshClientScore(%client);
		Client::sendMessage(%client, 0, "You receive 50 points for holding your captured tower!");
		messageall(1,getTeamName(%switchteam)@" held the defenders switch to complete the mission");
		$switchFirstTeam = "";
		ObjectiveMission::missionComplete();
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
function switch::setWaypoints(%this)
{
	if($switchFirstTeam != -1)
		%switchTeam = $switchFirstTeam;
	else 
		%switchTeam = GameBase::getTeam(%this);
//	if (%switchTeam == -1) return;
	$switchPos = GameBase::getPosition(%this);
//	if(%this.objectiveName) %name = %this.objectiveName;
//	else %name = "";
	%numPlayers = getNumClients();
	%posX = getWord($switchPos,0);
	%posY = getWord($switchPos,1);
	if (%switchTeam != -1)
	{
		for(%i = 0; %i < %numPlayers; %i = %i + 1)
		{
			%client = getClientByIndex(%i);
			%team = Client::getTeam(%client);	
			if(%team == %switchTeam)
			{
			issueCommand(%client, %client, 0,"Defend your teams switch.~wdefobj", %posX, %posY);
			}
			else
			{
			issueCommand(%client, %client, 0,"Attack enemy teams switch.~wcapobj", %posX, %posY);
			}	
		}
	}
//--------------------------	
	if (%switchTeam == -1)
	{	for(%i = 0; %i < %numPlayers; %i = %i + 1)
		{
		%client = getClientByIndex(%i);
		%team = Client::getTeam(%client);	
			if(%team == %switchTeam)
			{
			issueCommand(%client, %client, 0,"Defend your teams switch.~wdefobj", %posX, %posY);
			}
			else
			{
			issueCommand(%client, %client, 0,"Attack enemy teams switch.~wcapobj", %posX, %posY);
			}
		}	
	}	
	
//--------------
}

function Siege::setwaypoints()
{
	for(%i = 8200; %i< 9300; %i++){
		//1100 objects should be enough for now. -Plasmatic
		%data = GameBase::getDataName(%i);		
		if (%data == "towerSwitch") 
		{	
		switch::setWaypoints(%i);
		$switchFirstTeam = GameBase::getTeam(%i);
		}
	}
}

schedule("messageall(1,\"Welcome to Siege! Capture or hold the switch to complete the mission.\");",5);
schedule("Siege::setwaypoints();",10);




//	%numPlayers = getNumClients();
//	for(%i = 0; %i < %numPlayers; %i = %i + 1)
//	{
//		%id = getClientByIndex(%i);
//		if(Client::getTeam(%id) == %team1)
//		{
//			Client::sendMessage(%id, %mtype, %message1);
//		}
//		else if(%message2 != "" && Client::getTeam(%id) == %team2)
//		{
//			Client::sendMessage(%id, %mtype, %message2);
//		}
//		else if(%message3 != "")
//		{
//			Client::sendMessage(%id, %mtype, %message3);
//		}
//	}