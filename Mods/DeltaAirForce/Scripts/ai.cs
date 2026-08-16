//
// AI support functions.
//

//
// This function creates an AI player using the supplied group of markers 
//    for locations.  The first marker in the group gives the starting location 
//    of the the AI, and the remaining markers specify the path to follow.  
//
// Example call:  
// 
//    createAI( guardNumberOne, "MissionGroup\\Teams\\team0\\guardPath", larmor );
//

//globals
//--------
// path type
// 0 = circular
// 1 = oneWay
// 2 = twoWay
$AI::defaultPathType = 2; //run twoWay paths

//armor types
//light = larmor
//medium = marmor
//heavy = harmor
$AI::defaultArmorType = "larmor";



//---------------------------------
//createAI()
//---------------------------------
function createAI( %aiName, %markerGroup, %armorType, %name )
{
   %group = nameToID( %markerGroup );
   
   if( %group == -1 || Group::objectCount(%group) == 0 )
   {
      dbecho(1, %aiName @ "Couldn't create AI: " @ %markerGroup @ " empty or not found." );
      return -1;
   }
   else
   {
      %spawnMarker = Group::getObject(%group, 0);
      %spawnPos = GameBase::getPosition(%spawnMarker);
      %spawnRot = GameBase::getRotation(%spawnMarker);

      if( AI::spawn( %aiName, %armorType, %spawnPos, %spawnRot, %name, "male2" ) != "false" )
      {
         // The order number is used for sorting waypoints, and other directives.  
         %orderNumber = 100;
         
         for(%i = 1; %i < Group::objectCount(%group); %i = %i + 1)
         {
             
            %spawnMarker = Group::getObject(%group, %i);
            %spawnPos = GameBase::getPosition(%spawnMarker);
            
            AI::DirectiveWaypoint( %aiName, %spawnPos, %orderNumber );

			$SquadWaypoint[%team, %squad, %i-1] = %spawnPos;
            
            %orderNumber += 100;
         }
			
		 $SquadWaypointNum[%team, %squad] = Group::objectCount(%group) - 1;
		 //$AISpawn[AI::getId(%aiName)] = %spawnPos;
      }
      else{
         dbecho( 1, "Failure spawning: " @ %aiName );
      }
   }
}

//----------------------------------
// AI::setupAI()
//
// Called from Mission::init() which is defined in Objectives.cs (or Dm.cs for
//    deathmatch missions).  
//----------------------------------   
function AI::setupAI(%key, %team)
{
   //if there is no key then they don't exist yet
   if(%key == "")
   {
      %aiFound = 0;
      for( %T = 0; %T < 8; %T++ )
      {
         %groupId = nameToID("MissionGroup\\Teams\\team" @ %T @ "\\AI" );
         if( %groupId != -1 )
         {
            %teamItemCount = Group::objectCount(%groupId);
            if( %teamItemCount > 0 )
            {
               AI::initDrones(%T, %teamItemCount);
               %aiFound += %teamItemCount;
            }
         }
      }
      //check for Graph information
      if(nameToId("MissionGroup\\AIGraph") != -1)
      	AI::buildGraph();
      else
         echo("No Graph info to build AI Graph!");
      
      if( %aiFound == 0 )
         dbecho(1, "No drones exist...");
      else
         dbecho(1, %aiFound @ " drones installed..." );
   }
   else     //respawning dead AI with original name and path
   {
      %group = nameToID("MissionGroup\\Teams\\team" @ %team @ "\\AI\\" @ %key);
      %num = Group::objectCount(%group);
      createAI(%key, %group, $AI::defaultArmorType, %key);
      %aiId = AI::getId(%key);
      GameBase::setTeam(%aiId, %team);
      AI::setVar(%key, pathType, $AI::defaultPathType);
      AI::setWeapons(%key);
      
   }		
}

//------------------------------
// AI::setWeapons()
//------------------------------
function AI::setWeapons(%aiName)
{
	%aiId = AI::getId(%aiName);
	
   if(Game::missionType == "DM")
   {	
      dbecho(2, "giving DM weapon select...");
      //Player::setItemCount(%aiId, SOCOM, 1);
   }
   // DELTAFORCE
   else if(Game::missionType == "Hostage" || $Game::missionType == "Jail") {
	dbecho(2, "Hostages are unarmed.");
   }
   // END DELTAFORCE
   else
   {
      //dbecho(2, "giving normal weapon select...");
      //Player::setItemCount(%aiId, SOCOM, 1);
	  // Player::setItemCount(%aiId, OICW, 1);
	  // Player::setItemCount(%aiId, OICWAmmo, 200);
   }

   // Player::mountItem(%aiId, SOCOM, 0);
   AI::SetVar(%aiName, triggerPct, 0.03 );
   AI::setVar(%aiName, iq, 70 );
   AI::setVar(%aiName, attackMode, 1);
   AI::setAutomaticTargets( %aiName );
   //ai::callbackPeriodic(%aiName, 5, ai::periodicWeaponChange);
}


//-----------------------------------
// AI::periodicWeaponChange()
//-----------------------------------
function ai::periodicWeaponChange(%aiName)
{
	if($Game::missionType == "Hostage" || $hostageGame || $Game::missionType == "Jail")
		return;

	dbecho(2, %aiName @ " thinking....");
   %aiId = AI::getId(%aiName);
   %curTarget = ai::getTarget( %aiName );
   
   if(%curTarget == -1)
   {
   	return;
   }
      
   dbecho(1, %aiName @ " target: " @ %curTarget);	
   
   %targLoc = GameBase::getPosition(Client::getOwnedObject(%curTarget));
   %aiLoc = GameBase::getPosition(Client::getOwnedObject(%aiId));
   %targetDist = Vector::getDistance(%aiLoc, %targLoc);
   dbecho(2, "distance to target: " @ %targetDist @ 
                  " targetPosition: " @ targLoc @ " aiLocation: " @ %aiLoc);
                  
   if(%targetDist > 100)
   {
	  if($SquadIndex[%aiName] % 2 != 0)
	  { 
		Player::mountItem(%aiId, SAW, 0);
	  } else {
		Player::mountItem(%aiId, OICW, 0);
	  }
      AI::SetVar(%aiName, triggerPct, 0.8 );
   }   
   else
   {
      dbecho(2, "checking for target jet...");
      dbecho(2, "jetting? " @ Player::isJetting(%curTarget));
      if(Player::isJetting(%curTarget))
      {	
         if(Player::getItemCount(%aiId, SAW) > 0)
		 { 
		    Player::mountItem(%aiId, SAW, 0);
	     } else {
		   Player::mountItem(%aiId, OICW, 0);
		 }
         AI::SetVar(%aiName, triggerPct, 0.9 );
      }
      else
      {
         if($SquadIndex[%aiName] % 2 != 0)
		 { 
			Player::mountItem(%aiId, SAW, 0);
		  } else {
			Player::mountItem(%aiId, OICW, 0);
		  }
         AI::SetVar(%aiName, triggerPct, 0.9 );
      }   
   }
}

$BotSquads[0] = 0;
$BotSquads[1] = 0;
$BotSquads[2] = 0;
$BotSquads[3] = 0;
$BotSquads[4] = 0;
$BotSquads[5] = 0;
$BotSquads[6] = 0;
$BotSquads[7] = 0;

function setRifleWeapons(%aiName)
{
	%aiId = AI::getId(%aiName);

    dbecho(2, "giving normal weapon select...");
    Player::setItemCount(%aiId, SOCOM, 1);
	Player::setItemCount(%aiId, SOCOMAmmo, 50);

	if($SquadIndex[%aiName] == 2)
	{
		Player::setItemCount(%aiId, SAW, 1);
		Player::setItemCount(%aiId, SAWAmmo, 400);
		Player::mountItem(%aiId, SAW, 0);
	} else {
		Player::setItemCount(%aiId, OICW, 1);
		Player::setItemCount(%aiId, OICWAmmo, 200);
		Player::mountItem(%aiId, OICW, 0);
	}

   AI::SetVar(%aiName, triggerPct, 0.9 );
   AI::setVar(%aiName, attackMode, 1);
   AI::setAutomaticTargets( %aiName );
   //ai::callbackPeriodic(%aiName, 5, ai::periodicWeaponChange);
}

function setSniperWeapons(%aiName)
{
   %aiId = AI::getId(%aiName);

   dbecho(2, "giving normal weapon select...");
   Player::setItemCount(%aiId, FiftyCal, 1);
   Player::setItemCount(%aiId, FiftyCalAmmo, 50);
   Player::mountItem(%aiId, FiftyCal, 0);

   AI::SetVar(%aiName, triggerPct, 0.9 );
   AI::setVar(%aiName, attackMode, 1);
   AI::setAutomaticTargets( %aiName );
   //ai::callbackPeriodic(%aiName, 5, ai::periodicWeaponChange);
}

function setGunnerWeapons(%aiName)
{
   // No weapons

   AI::SetVar(%aiName, triggerPct, 0.9 );
   AI::setVar(%aiName, attackMode, 1);
   AI::setAutomaticTargets( %aiName );
   $originalRot[%aiName] = GameBase::getRotation(AI::getId(%aiName));
   ai::callbackPeriodic(%aiName, 1);
}

function spawnSquadmate(%aiName, %armorType, %commandIssuer)
{
   %spawnMarker = GameBase::getPosition(%commandIssuer);
   %xPos = getWord(%spawnMarker, 0);
   %yPos = getword(%spawnMarker, 1) + 5;
   %zPos = getWord(%spawnMarker, 2) + 2;
   %rPos = GameBase::getRotation(%commandIssuer);
   
   dbecho(2, "Spawning AI helper at position " @ %xPos @ " " @ %yPos @ " " @ %zPos);
   dbecho(2, "Current Issuer rotation: " @ %rPos);
      
   %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;
   %newName = %aiName @ $numAI;
   $numAI++;
   Ai::spawn(%newName, %armorType, %aiSpawnPos, %rPos);
   GameBase::setTeam(AI::getId(%newName), GameBase::getTeam(%commandIssuer));
   return (%newName);
}

function spawnRifle(%team, %set, %orders, %oldSquad)
{
	%aiName = "Rifle" @ $numAI;
	$numAI++;
	createAI(%aiName, %set, "iarmor", %aiName);	
	%aiId = AI::getId(%aiName);
	GameBase::setTeam(%aiId, %team);
	AI::setVar( %aiName,  iq,  80 );
	AI::setVar( %aiName,  attackMode, 1);
	
	if(%orders == 0)
		AI::setVar( %aiName,  pathType, $AI::defaultPathType);
	else
		AI::setVar( %aiName,  pathType, 1);
		
	schedule("setRifleWeapons(" @ %aiName @ ");", 1);
	$SquadIndex[%aiName] = 0;

	if(%oldSquad == -1)
	{
		$SquadClass[%team, $BotSquads[%team]] = "Rifle";
		$SquadDir[%team, $BotSquads[%team]] = %orders;

		$SquadSet[%team, $BotSquads[%team]] = %set;
		$MemberOf[%aiName] = $BotSquads[%team];
		$Squad[%team, $BotSquads[%team], 0] = %aiName;
		
		// Spawn the squad
		for(%i = 0; %i < 3; %i++)
		{		
			%newAi = spawnSquadmate("RS", "iarmor", %aiId);
			AI::setVar( %newAi,  iq,  80 );
			AI::setVar( %newAi,  attackMode, 1);
			if(%orders == 0) { // Patrol mode
				AI::setVar( %newAi,  pathType, $AI::defaultPathType);
			} else {   // Assault mode
				AI::setVar( %newAi,  pathType, 1);
			}
			schedule("setRifleWeapons(" @ %newAi @ ");", 1);
			//AI::DirectiveFollow(%newAi, %aiId, 0, 1024);
			AI::DirectiveFollow(%newAi, %aiId, 0, 2);
			%aiId = AI::getId(%newAi);
			$SquadIndex[%newAi] = %i+1;
			$MemberOf[%newAi] = $BotSquads[%team];
			$Squad[%team, $BotSquads[%team], %i+1] = %newAi;
		}
		$BotSquads[%team]++;
	} else
	{
		$MemberOf[%aiName] = %oldSquad;
		$Squad[%team, %oldSquad, 0] = %aiName;

		// Spawn the squad
		for(%i = 0; %i < 3; %i++)
		{		
			%newAi = spawnSquadmate("RS", "iarmor", %aiId);
			AI::setVar( %newAi,  iq,  70 );
			AI::setVar( %newAi,  attackMode, 1);
			if(%orders == 0) { // Patrol mode
				AI::setVar( %newAi,  pathType, $AI::defaultPathType);
			} else {   // Assault mode
				AI::setVar( %newAi,  pathType, 1);
			}
			schedule("setRifleWeapons(" @ %newAi @ ");", 1);
			//AI::DirectiveFollow(%newAi, %aiId, 0, 1024);
			AI::DirectiveFollow(%newAi, %aiId, 0, 2);
			%aiId = AI::getId(%newAi);
			$SquadIndex[%newAi] = %i+1;
			$MemberOf[%newAi] = %oldSquad;
			$Squad[%team, %oldSquad, %i+1] = %newAi;
		}
	}
}

function spawnSniper(%team, %set, %oldSquad)
{
	%aiName = "Sniper" @ $numAI;
	$numAI++;
	createAI(%aiName, %set, "sarmor", %aiName);
	%aiId = AI::getId(%aiName);
	GameBase::setTeam(%aiId, %team);
	AI::setVar( %aiName,  iq,  150 );
	AI::setVar( %aiName,  attackMode, 1);
	AI::setVar( %aiName,  pathType, 1);
	schedule("setSniperWeapons(" @ %aiName @ ");", 1);
	$SquadIndex[%aiName] = 0;

	if(%oldSquad == -1)
	{
		$SquadSet[%team, $BotSquads[%team]] = %set;
		$SquadClass[%team, $BotSquads[%team]] = "Sniper";
		$MemberOf[%aiName] = $BotSquads[%team];
		$Squad[%team, $BotSquads[%team], 0] = %aiName;
		$Squad[%team, $BotSquads[%team], 1] = -1;
		$BotSquads[%team]++;
	} else
	{
		$MemberOf[%aiName] = %oldSquad;
		$Squad[%team, %oldSquad, 0] = %aiName;
		$Squad[%team, %oldSquad, 1] = -1;
	}
}

function spawnGunner(%team, %set, %oldSquad)
{
	%aiName = "Gunner" @ $numAI;
	$numAI++;
	createAI(%aiName, %set, "iarmor", %aiName);
	%aiId = AI::getId(%aiName);
	GameBase::setTeam(%aiId, %team);
	AI::setVar( %aiName,  iq,  100 );
	AI::setVar( %aiName,  attackMode, 1);
	AI::setVar( %aiName,  pathType, 1);
	schedule("setGunnerWeapons(" @ %aiName @ ");", 1);
	$SquadIndex[%aiName] = 0;

	if(%oldSquad == -1)
	{
		$SquadSet[%team, $BotSquads[%team]] = %set;
		$SquadClass[%team, $BotSquads[%team]] = "Gunner";
		$MemberOf[%aiName] = $BotSquads[%team];
		$Squad[%team, $BotSquads[%team], 0] = %aiName;
		$Squad[%team, $BotSquads[%team], 1] = -1;
		$BotSquads[%team]++;
	} else
	{
		$MemberOf[%aiName] = %oldSquad;
		$Squad[%team, %oldSquad, 0] = %aiName;
		$Squad[%team, %oldSquad, 1] = -1;
	}
}

//-----------------------------------
// AI::initDrones()
//-----------------------------------
function AI::initDrones(%team, %numAi)
{
	dbecho(1, "spawning team " @ %team @ " ai...");
   for(%guard = 0; %guard < %numAi; %guard++)
   {
      //check for internal data
      %tempSet = 	nameToID("MissionGroup\\Teams\\team" @ %team @ "\\AI");
      %tempItem = Group::getObject(%tempSet, %guard);
      %aiName = Object::getName(%tempItem);
      
      %set = nameToID("MissionGroup\\Teams\\team" @ %team @ "\\AI\\" @ %aiName);
      %numPts = Group::objectCount(%set);
	
	  // DELTAFORCE
	  if(%numPts > 0)
	  {
		if(($Game::missionType != "Hostage" || $hostageGame == 0 )&& $Game::missionType != "Jail")
		{
			if(String::findSubStr(%aiName, "Rifle") >= 0)
			{
				if(String::findSubStr(%aiName, "Patrol") >= 0) // Patrol
				{
					spawnRifle(%team, %set, 0, -1);
				} else {  // Assault
					spawnRifle(%team, %set, 1, -1);
				}
			} else if(String::findSubStr(%aiName, "Sniper") >= 0)
			{
				spawnSniper(%team, %set, -1);
			} else if(String::findSubStr(%aiName, "Gunner") >= 0)
			{
				spawnGunner(%team, %set, -1);
			}
		} else if($Game::missionType == "Hostage" || $hostageGame == 1 || $Game::missionType == "Jail") {
			 createAI(%aiName, %set, $AI::defaultArmorType, %aiName);
			%aiId = AI::getId( %aiName );
			GameBase::setTeam(%aiId, %team);
			$hostageTeam[%aiId] = -1;
			$HostageOwner[%aiId] = -1;
	// END DELTAFORCE
			AI::setVar( %aiName,  iq,  60 );
			AI::setVar( %aiName,  attackMode, 1);
			AI::setVar( %aiName,  pathType, $AI::defaultPathType);
      		schedule("AI::setWeapons(" @ %aiName @ ");", 1);
		} else {
			 dbecho(1, "no info to spawn ai...");
	    }
	  }
   }
}


//------------------------------------------------------------------
//functions to test and move AI players.
//
//------------------------------------------------------------------

//
//This function will spawn an AI player about 5 units away from the 
//player that is passed to the function(%commandIssuer).
//
//
$numAI = 0;
function AI::helper(%aiName, %armorType, %commandIssuer)
{
   %spawnMarker = GameBase::getPosition(%commandIssuer);
   %xPos = getWord(%spawnMarker, 0) + floor(getRandom() * 15);
   %yPos = getword(%spawnMarker, 1) + floor(getRandom() * 10);
   %zPos = getWord(%spawnMarker, 2) + 2;
   %rPos = GameBase::getRotation(%commandIssuer);
   
   dbecho(2, "Spawning AI helper at position " @ %xPos @ " " @ %yPos @ " " @ %zPos);
   dbecho(2, "Current Issuer rotation: " @ %rPos);
      
   %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;
   %newName = %aiName @ $numAI;
   $numAI++;
   Ai::spawn(%newName, %armorType, %aiSpawnPos, %rPos);
   return ( %newName );
}

//
//This function will move an AI player to the position of an object
//that the players LOS is hitting(terrain included). Must be `	within 50 units.
//
//
function AI::moveToLOS(%aiName, %commandIssuer) 
{
   %issuerRot = GameBase::getRotation(%commandIssuer);
   %playerObj = Client::getOwnedObject(%commandIssuer);
   %playerPos = GameBase::getPosition(%commandIssuer);
      
   //check within max dist
   if(GameBase::getLOSInfo(%playerObj, 100, %issuerRot))
   { 
      %newIssuedVec = $LOS::position;
	  %distance = Vector::getDistance(%playerPos, %newIssuedVec);
	  dbecho(2, "Command accepted, AI player(s) moving....");
	  dbecho(2, "distance to LOS: " @ %distance);
	  AI::DirectiveWaypoint( %aiName, %newIssuedVec, 1 );
   }
   else
      dbecho(2, "Distance to far.");
      
   dbecho(2, "LOS point: " @ $LOS::position);
}

//This function will move an AI player to a position directly in front of
//the player passed, at a distance that is specified.
function  AI::moveAhead(%aiName, %commandIssuer, %distance) 
{
   
   %issuerRot = GameBase::getRotation(%commandIssuer);
   %commPos  = GameBase::getPosition(%commandIssuer);
   dbecho(2, "Commanders Position: " @ %commPos);
   
   //get commanders x and y positions
   %comm_x = getWord(%commPos, 0);
   %comm_y = getWord(%commPos, 1);
   
   //get offset x and y positions
   %offSetPos = Vector::getFromRot(%issuerRot, %distance);
   %off_x = getWord(%offSetPos, 0);
   %off_y = getWord(%offSetPos, 1);
   
   //calc new position
   %new_x = %comm_x + %off_x;
   %new_y = %comm_y + %off_y;
   %newPos = %new_x  @ " " @ %new_y @ " 0";
  
   //move AI player
   dbecho(2, "AI moving to " @ %newPos);
   AI::DirectiveWaypoint(%aiName, %newPos, 1);
}  

//
// OK, this is the complete command callback - issued for any command sent
//    to an AI. 
//
function AI::onCommand ( %name, %commander, %command, %waypoint, %targetId, %cmdText, 
         %cmdStatus, %cmdSequence )
{
   %aiId = AI::getId( %name );
   %T = GameBase::getTeam( %aiId );
   %groupId = nameToID("MissionGroup\\Teams\\team" @ %T @ "\\AI\\" @ %name ); 
  	%nodeCount = Group::objectCount( %groupId );
   dbecho(2, "checking drone information...." @ " number of nodes: " @ %nodeCount);
   dbecho(2, "AI id: " @ %aiId @ " groupId: " @ %groupId);

	//if($SinglePlayer || %nodeCount == 1)
   if(($Game::missionType != "Hostage" || $hostageGame != 1) && $Game::missionType != "Jail")   
   {
	   if( %command == 2 || %command == 1 )
	   {
		  %com = $Squad[%T, $MemberOf[%name], 0];
		  // must convert waypoint location into world location.  waypoint location
		  //    is given in range [0-1023, 0-1023].  
		  %worldLoc = WaypointToWorld ( %waypoint );
		  AI::DirectiveWaypoint( %com, %worldLoc, 2 );
		  dbecho ( 2, %name @ " IS PROCEEDING TO LOCATION " @ %worldLoc );
	   }
	   dbecho( 2, "AI::OnCommand() issued to  " @ %name @ "  with parameters: " );
	   dbecho( 3, "Cmdr:        " @ %commander );
	   dbecho( 3, "Command:     " @ %command );
	   dbecho( 3, "Waypoint:    " @ %waypoint );
	   dbecho( 3, "TargetId:    " @ %targetId );
	   dbecho( 3, "cmdText:     " @ %cmdText );
	   dbecho( 3, "cmdStatus:   " @ %cmdStatus );
	   dbecho( 3, "cmdSequence: " @ %cmdSequence );
   }
   else
   	return;   
}


// Play the given wave file FROM %source to %DEST.  The wave name is JUST the basic wave
// name without voice base info (which it will grab for you from the source client Id).  
// Basically does some string fiddling for you.  
//
// Example:
//    Ai::soundHelper( 2051, 2049, cheer3 );
//
function Ai::soundHelper( %sourceId, %destId, %waveFileName )
{
   %wName = strcat( "~w", Client::getVoiceBase( %sourceId ) );
   %wName = strcat( %wName, ".w" );
   %wName = strcat( %wName, %waveFileName );
   %wName = strcat( %wName, ".wav" );
   
   dbecho( 2, "Trying to play " @ %wName );
   
   Client::sendMessage( %destId, 0, %wName );
}


// Default periodic callback.  [Note by default it isn't called unless a frequency 
//    is set up using AI::CallbackPeriodic().  Type in that command to see how 
//    it works].  
function AI::onPeriodic( %aiName )
{
   dbecho(2, "onPeriodic() called with " @ %aiName );
   %client = AI::getId(%aiName);
   %pl = Client::getControlObject(%client);

   //if(%pl.vehicle)
   //{
		%targ = AI::getTarget(%aiName);
		if(%targ != -1)
		{
			%curRot = GameBase::getRotation(%pl);
			%targPos = GameBase::getPosition(%targ);
			%myPos = GameBase::getPositon(%pl);
			%cZ = getWord(%curRot, 2);
			%oZ = getWord($originalRot[%aiName], 2);
			
			%newZ = %cZ - %oZ;

			%targVec = Vector::sub(%targPos, %myPos);

			// Rotate the target point
			//%tX = getWord(%targVec, 0) * cos(%newZ) - getWord(%targVec, 1) * sin(%newZ);
			//%tY = getWord(%targVec, 0) * sin(%newZ) + getWord(%targVec, 1) * cos(%newZ);
			%tZ = getWord(%targVec, 2);

			%tX += getWord(%myPos, 0);
			%tY += getWord(%myPos, 1);
			%tZ += getWord(%myPos, 2);

			AI::DirectiveTargetPoint(%aiName, %tX @ " " @ %tY @ " " @ %tZ, 0, 2);
			echo("Point selected!");
		}
   //}		
}


function AI::onDroneKilled(%aiName)
{
   if( ! $SinglePlayer )
   {
      %aiId = AI::getId(%aiName);
	  if($Game::missionType == "Hostage" || $hostageGame == 1 || $Game::missionType == "Jail")
		GameBase::setTeam(%aiId, getNumTeams() - 1);
      
	  %team = GameBase::getTeam(%aiId);
      dbecho(2, "AI Id: " @ %aiId);
     
	  // DELTAFORCE
	  if($Game::missionType == "Hostage" || $hostageGame == 1 || $Game::missionType == "Jail")
	  {
		// Trying a little longer delay -
		schedule("AI::setupAI(" @ %aiName @ ", " @ %team @ ");", 8 );
			if($hostageTeam[%aiId] != -1 && $hostageTeam[%aiId] != getNumTeams() - 1) {
				%oldTeam = $hostageTeam[%aiId];
				if($HostageOwner[%aiId] != -1) {
					if($Game::missionType != "Jail")
						$deltaTeamScore[%oldTeam] -= 5;
					for(%x = 0; %x < 3; %x++) {
						if( $Hostages[ $HostageOwner[%aiId], %x ] == %aiId )
							$Hostages[ $HostageOwner[%aiId], %x ] = 0;
					}
					$TakenHostages[$HostageOwner[%aiId]]--;
				}
				$hostageTeam[%aiId] = -1;
				$HostageOwner[%aiId] = -1;
			}
			for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) {
				Client::sendMessage(%cl, 1, %aiName @ " has been executed!");
			}
		} else 
		{
			if($SquadIndex[%aiName] != 0)
			{
				// Make the squad stick together
				%sub = $Squad[%team, $MemberOf[%aiName], ($SquadIndex[%aiName]+1)];
				%com = $Squad[%team, $MemberOf[%aiName], ($SquadIndex[%aiName]-1)];
				// Follow the guy in front of you
				//AI::DirectiveFollow(%sub, AI::getId(%com), 0, 1024);
				AI::DirectiveFollow(%sub, AI::getId(%com), 0, 2);
			} else
			{
				if($Squad[%team, $MemberOf[%aiName], 1] == -1)
				{
					// The entire squad's dead!
					if(!$TrainingGame) // Only respawn in multiplayer games
					{
						if($SquadClass[%team, $MemberOf[%aiName]] == "Rifle")
						{
							schedule("spawnRifle("@%team@", "@$SquadSet[%team, $MemberOf[%aiName]]@", "@$SquadDir[%team, $MemberOf[%aiName]]@", "@$MemberOf[%aiName]@");", 8);
							return; 
						} else if($SquadClass[%team, $MemberOf[%aiName]] == "Sniper")
						{
							schedule("spawnSniper("@%team@", "@$SquadSet[%team, $MemberOf[%aiName]]@", "@$MemberOf[%aiName]@");", 8);
							return;
						} else if($SquadClass[%team, $MemberOf[%aiName]] == "Gunner")
						{
							schedule("spawnGunner("@%team@", "@$SquadSet[%team, $MemberOf[%aiName]]@", "@$MemberOf[%aiName]@");", 8);
							return;
						}
					}
				}
			}

			for(%i = $SquadIndex[%aiName]; %i < 3; %i++)
			{
				$Squad[%team, $MemberOf[%aiName], %i] = $Squad[%team, $MemberOf[%aiName], %i+1];
				$SquadIndex[$Squad[%team, $MemberOf[%aiName], %i]]--;

				// If he's the new squad leader, remove his follow directive
				// and give him the squad waypoints
				if($SquadIndex[$Squad[%team, $MemberOf[%aiName], %i]] == 0)
				{
					%pos = GameBase::getPosition(%aiId);

					AI::DirectiveRemove($Squad[%team, $MemberOf[%aiName], %i], 2);
					
					// Find the closest waypoint
					if($SquadWaypointNum[%team, $MemberOf[%aiName]] > 0)
					{
						%dist = 0;
						%selWpt = 0;
						for(%i = 0; %i < $SquadWaypointNum[%team, $MemberOf[%aiName]]; %i++)
						{
							%tD = Vector::getDistance(%pos, $SquadWaypoint[%team, $MemberOf[%aiName], %i]);

							if(%tD > %dist)
							{
								%dist = %tD;
								%selWpt = %i;
							}
						}
					
						// Follow the waypoints in order
						AI::DirectiveWaypoint(%aiName, $SquadWaypoint[%team, $MemberOf[%aiName], %selWpt], 100);

						%i = %selWpt + 1;
						%orderNum = 200;
						while(%i != %selWpt)
						{
							if(%i >= $SquadWaypointNum[%team, $MemberOf[%aiName]])
							{
								%i = 0;
								if(%selWpt == %i)
									break;
							}						
						
							AI::DirectiveWaypoint(%aiName, $SquadWaypoint[%team, $MemberOf[%aiName], %i], %orderNum);
						
							%orderNum += 100;
							%i++;
						}
					}
				}
				
				$Squad[%team, $MemberOf[%aiName], %i+1] = -1;
			}
		}
		// END DELTAFORCE
   }
   else
   {
      // just in case:
      dbecho( 2, "Non training callback called from Training" );
   }
}


function AI::buildGraph(%argv)
{
	
   if(!%argv)
   {
	   echo("building AI Graph...");
	   $nodeGroup = nameToID("MissionGroup\\AIGraph");
	   %numNodes  = Group::ObjectCount($nodeGroup);
	   echo("nodeGroup: " @ %nodeGroup @ " number of Nodes: " @ %numNodes); 
   }
   else
   {
	   echo("rebuilding AI Graph...");
	   %numNodes  = Group::ObjectCount($nodeGroup);
   }
   
   for(%i = 0; %i < %numNodes; %i++)
   {
   	%node = Group::getObject($nodeGroup, %i);
      %nodePos = GameBase::getPosition(%node);
      %type = GameBase::getDataName(%node);
      %name = "Node " @ %i;
      Graph::AddNode(%nodePos, %name);
   }
   if(%numNodes > 0)	
   {   
      Graph::buildGraph();
      echo("Graph build complete.");
   }
   else
   	echo("No nodes to build graph.");
}


function AI::rebuildGraph()
{
	Graph::reset();
   AI::graphOff();
   focusServer();
   AI::buildGraph("true");
   focusClient();
   AI::GraphOn();
}

//these AI function callbacks can be very useful!
function AI::onTargetLOSAcquired(%aiName, %idNum)
{
	if(($Game::missionType != "Hostage" || $hostageGame == 0 ) && $Game::missionType != "Jail") {
		// Make sure you don't shoot your teammate in the back...
		GameBase::getLOSInfo(AI::getId(%aiName), 5, GameBase::getRotation(AI::getId(%aiName)));
		if(GameBase::getTeam($los::object) == GameBase::getTeam(AI::getId(%aiName)))
		{
			// Disable firing
			//AI::SetVar(%aiName, triggerPct, 0.0 );
		}
	}
}

function AI::onTargetDied(%aiName, %idNum)
{
	if(($Game::missionType != "Hostage" || $hostageGame == 0) && $Game::missionType != "Jail") {
		%team = GameBase::getTeam(AI::getId(%aiName));
		// Return to formation...
		if($SquadIndex[%aiName] != 0) {
			%com = $Squad[%team, $MemberOf[%aiName], ($SquadIndex[%aiName]-1)];
			//AI::DirectiveFollow(%aiName, AI::getId(%com), 0, 1024);
			AI::DirectiveFollow(%aiName, AI::getId(%com), 0, 2);
		} else {
			AI::DirectiveRemove(%aiName, 2);
		}
	}
}                                 

function AI::onTargetLOSLost(%aiName, %idNum)
{
	if(($Game::missionType != "Hostage" || $hostageGame == 0) && $Game::missionType != "Jail") {
		%team = GameBase::getTeam(AI::getId(%aiName));
		if($SquadClass[%team, $MemberOf[%aiName]] == "Rifle") {
			// Search and destroy!
			//AI::DirectiveFollow(%aiName, %idNum, 0, 1024);
			if($SquadIndex[%aiName] == 0) {
				//AI::DirectiveWaypoint( %aiName, GameBase::getPosition(%idNum), 0);
				AI::DirectiveWaypoint( %aiName, GameBase::getPosition(%idNum), 2);
			}
		}
	}
}

function AI::onTargetLOSRegained(%aiName, %idNum)
{
}

//Render lines to show AI Graph
$AI::testGraph = 0;
function AI::GraphOn(){
   if( $AI::testGraph == 0 ){
   	$AI::testGraph = newObject( "graphRender", AI::GraphPathRender );
      echo("Graph render on");
   }
   else{
      echo("Graph render ALREADY on");
   }
}
function AI::GraphOff(){
   if( $AI::testGraph != 0 ){
   	deleteObject( $AI::testGraph );
      $AI::testGraph = 0;
   	echo("Graph render off");
   }
   else{
      echo("Graph render ALREADY off");
   }
}


function AI::testingCommander()
{
   %clientId = "2049";
   AI::helper("jettkiller", harmor, %clientId);
   %aiId = "2050";
   GameBase::setTeam(%aiId, 0);
   AI::callWithId(%aiId, Player::setItemCount, Mortar, 1);
	AI::callWithId(%aiId, Player::setItemCount, MortarAmmo, 10000);
   AI::callWithId(%aiId, Player::mountItem, mortar, 0);
   AI::DirectiveTargetLaser( %aiId, %clientId );
}