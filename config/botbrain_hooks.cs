//=============================================================================================
// BotBrain objective hooks -- BASE mod only (BotBrain refuses other mods by construction).
// Derived from base\scripts\objectives.cs via SpoonBot's generated copy, with EVERY
// SpoonBot reference removed. The ONLY functional change vs stock: the
// Player::isAIControlled early-out in Flag::onCollision stays deleted, or no bot can ever
// pick up a flag. Re-exec'd from C++ (botWorld.cpp) at EVERY mission init, because
// objectives.cs is re-exec'd per mission and clobbers these bodies.
//=============================================================================================
function Flag::onCollision(%this, %object)
{
   //echo("Flag collision ", %object);
   if(getObjectType(%object) != "Player")
      return;

   if(%this.carrier != -1)
      return; // spurious collision
      
   // NATIVE-PORT (SpoonBot): stock early-out DELETED -- without this a bot
   // cannot pick up a flag at all; every AI collision returned here.
   //   if(Player::isAIControlled(%object))
   //      return;
   %name = Item::getItemData(%this);
   %playerTeam = GameBase::getTeam(%object);
   %flagTeam = GameBase::getTeam(%this);
   %playerClient = Player::getClient(%object);
   %touchClientName = Client::getName(%playerClient);
							 

   if(%flagTeam == %playerTeam)
   {
      // player is touching his own flag...
      if(!%this.atHome)
      {
         // the flag isn't home! so return it.
			GameBase::startFadeOut(%this);
			GameBase::setPosition(%this, %this.originalPosition);
         Item::setVelocity(%this, "0 0 0");
			GameBase::startFadeIn(%this);
         %this.atHome = true;
         MessageAllExcept(%playerClient, 0, %touchClientName @ " returned the " @ getTeamName(%playerTeam) @ " flag!~wflagreturn.wav");
         Client::sendMessage(%playerClient, 0, "You returned the " @ getTeamName(%playerTeam) @ " flag!~wflagreturn.wav");
         teamMessages(1, %playerTeam, "Your flag was returned to base.", -2, "", "The " @ getTeamName(%playerTeam) @ " flag was returned to base.");
         %this.pickupSequence++;
         ObjectiveMission::ObjectiveChanged(%this);
      }
      else
      {
         // it's at home - see if we have an enemy flag!
         if(%object.carryFlag != "")
         {
            // can't cap the neutral flags, duh
           	%enemyTeam = GameBase::getTeam(%object.carryFlag);
			   if(%enemyTeam != -1)
            {
               MessageAllExcept(%playerClient, 0, %touchClientName @ " captured the " @ getTeamName(%enemyTeam) @ " flag!~wflagcapture.wav");
               Client::sendMessage(%playerClient, 0, "You captured the " @ getTeamName(%enemyTeam) @ " flag!~wflagcapture.wav");
               TeamMessages(1, %playerTeam, "Your team captured the flag.", %enemyTeam, "Your team's flag was captured.");
            
               %flag = %object.carryFlag;
               %flag.atHome = true;
               %flag.carrier = -1;
               %flag.caps[%playerTeam]++;
               %flag.enemyCaps++;
               
               
               Item::hide(%flag, false);
               $flagAtHome[1] = true;
               GameBase::setPosition(%flag, %flag.originalPosition);
               Item::setVelocity(%flag, "0 0 0");

               %flag.trainingObjectiveComplete = true;
               ObjectiveMission::ObjectiveChanged(%flag);

               Player::setItemCount(%object, Flag, 0);
               %object.carryFlag = "";
               Flag::clearWaypoint(%playerClient, true);

               $teamScore[%playerTeam] += %flag.scoreValue;
               ObjectiveMission::checkScoreLimit();

               //flag carrier gets 5 points for caputure
               %playerClient.score += 5;
               Game::refreshClientScore(%playerClient);
               messageAll(0, Client::getName(%playerClient) @ " receives 5 point capture bonus.");
               %aiId = Player::getClient(%object);
               // BOTBRAIN 0A: the AUTHORITATIVE capture event. This block is the single
               // funnel point for every capture (physical collisions and the C++ touch
               // assist both route through Flag::onCollision); C++ joins it to the active
               // offensive attempt. The [BBFLAG] CARRIED->HOME machine is the cross-check.
               BotBrain::CaptureEvent(%object, %flag);
            }
         }
      }
   }
   else
   {
      // it's an enemy's flag! woohoo!
      if(%object.carryFlag == "")
      {
			if(%object.outArea == "") {
				// don't pick up our flags
        		if(%this.holdingTeam == %playerTeam)
        		   return;

        		Player::setItemCount(%object, Flag, 1);
        		Player::mountItem(%object, Flag, $FlagSlot, %flagTeam);
        		Item::hide(%this, true);
        		$flagAtHome[1] = false;
        		%this.atHome = false;
        		%this.carrier = %object;
        		%this.pickupSequence++;
        		%object.carryFlag = %this;
	 			Flag::setWaypoint(%playerClient, %this);

				// NATIVE-PORT (SpoonBot): DEFERRED. BotTree::GetMeToPos raycasts every treepoint;
				// running it inside a collision callback is the wrong shape. See hooks_common.cs.
	 			if(%this.fadeOut) {
					GameBase::startFadeIn(%this);
	 				%this.fadeOut= "";
				}
        		
        		if((%this.lastTeam == "" || %this.lastTeam != %playerTeam) && %flagTeam == -1) {
			 		%this.currentFlagStand="";
			 		%this.changeTeamCount++;
					%this.lastTeam = %playerTeam;
     		      %this.timerOn = 1;
     		      if($flagToStandTime >= 30) {
	     		      %timeToStand = $flagToStandTime - 30;
						%timeLeft = 30;
	     		      if($flagToStandTime > 30)
	     		      	Client::sendMessage(%playerClient, 0, "You have " @ $flagToStandTime @ " sec to put the flag in a stand.");
					}
					else {	
						if($flagToStandTime >= 10)
							%remain = $flagToStandTime % 10;
						else 
							%remain = $flagToStandTime % 5;
						
						if(%remain > 0 && %remain != $flagToStandTime) {
							%timeToStand = %remain;
							%timeLeft = $flagToStandTime - %remain;
		     		      Client::sendMessage(%playerClient, 0, "You have " @ $flagToStandTime @ " sec to put the flag in a stand.");
						}
						else {
							%timeToStand = 0;
							%timeLeft = $flagToStandTime;
						}
					}

     		      schedule("Flag::checkFlagsTime(" @ %this @"," @ %timeLeft @ "," @ %this.changeTeamCount @ ");",%timeToStand);
				}
        		
        		if(%flagTeam != -1)
        		{
	     		   MessageAllExcept(%playerClient, 0, %touchClientName @ " took the " @ getTeamName(%flagTeam) @ " flag! ~wflag1.wav");
        		   Client::sendMessage(%playerClient, 0, "You took the " @ getTeamName(%flagTeam) @ " flag! ~wflag1.wav");
        		   TeamMessages(1, %playerTeam, "Your team has the " @ getTeamName(%flagTeam) @ " flag.", %flagTeam, "Your team's flag has been taken.");
        		}
        		else
        		{
        		   %hteam = %this.holdingTeam;
	     		   if(%hteam != -1)
        		   {
        		      $teamScore[%hteam] -= %this.scoreValue;
        		      $deltaTeamScore[%hteam] -= %this.deltaTeamScore;

	     		      MessageAllExcept(%playerClient, 0, %touchClientName @ " took " @ %this.objectiveName @ " from the " @ getTeamName(%hteam) @ " team.~wflag1.wav");
        		      Client::sendMessage(%playerClient, 0, "You took " @ %this.objectiveName @ " from the " @ getTeamName(%hteam) @ " team.~wflag1.wav");
        		      TeamMessages(1, %playerTeam, "Your team has " @ %this.objectiveName @ ".", %hteam, "Your team lost " @ %this.objectiveName @ ".", "The " @ getTeamName(%playerTeam) @ " team has taken " @ %this.objectiveName @ " from the " @ getTeamName(%hteam) @ " team.");
        		      %this.holdingTeam = -1;
        		      %this.holder.flag = "";
        		   }
        		   else
        		   {
	     		      MessageAllExcept(%playerClient, 0, %touchClientName @ " took " @ %this.objectiveName @ ".~wflag1.wav");
        		      Client::sendMessage(%playerClient, 0, "You took " @ %this.objectiveName @ ".~wflag1.wav");
        		      TeamMessages(1, %playerTeam, "Your team has " @ %this.objectiveName @ ".", -2, "", "The " @ getTeamName(%playerTeam) @ " team has taken " @ %this.objectiveName @ ".");
        		   }
        		}
        		%this.trainingObjectiveComplete = true;
        		ObjectiveMission::ObjectiveChanged(%this);
			}
			else
  		      Client::sendMessage(%playerClient, 1, "Flag not in mission area.");
		}
   }
}

// ---- Flag::onDrop ----

function Flag::onDrop(%player, %type)
{
   %playerTeam = GameBase::getTeam(%player);
   %flag = %player.carryFlag;
   %flagTeam = GameBase::getTeam(%flag);
   %playerClient = Player::getClient(%player);
   %dropClientName = Client::getName(%playerClient);

   if(%flagTeam == -1)
   {
      MessageAllExcept(%playerClient, 1, %dropClientName @ " dropped " @ %flag.objectiveName @ "!");
      Client::sendMessage(%playerClient, 1, "You dropped "  @ %flag.objectiveName @ "!");
   }
   else
   {
      MessageAllExcept(%playerClient, 0, %dropClientName @ " dropped the " @ getTeamName(%flagTeam) @ " flag!");
      Client::sendMessage(%playerClient, 0, "You dropped the " @ getTeamName(%flagTeam) @ " flag!");
      TeamMessages(1, %flagTeam, "Your flag was dropped in the field.", -2, "", "The " @ getTeamName(%flagTeam) @ " flag was dropped in the field.");
   }
   GameBase::throw(%flag, %player, 10, false);
   Item::hide(%flag, false);
   Player::setItemCount(%player, "Flag", 0);
   %flag.carrier = -1;
   %player.carryFlag = "";
   Flag::clearWaypoint(%playerClient, false);

   schedule("Flag::checkReturn(" @ %flag @ ", " @ %flag.pickupSequence @ ");", $flagReturnTime);
	%flag.dropFade = 1;
   ObjectiveMission::ObjectiveChanged(%flag);

   // upstream had no guard and leaked directives until the sim stalled.
}

// ---- Flag::checkReturn ----

function Flag::checkReturn(%flag, %sequenceNum)
{
   //echo("checking for flag return: ", %flag, ", ", %sequenceNum);
   if(%flag.pickupSequence == %sequenceNum && %flag.timerOn == "")
   {
		if(%flag.dropFade) { 
			GameBase::startFadeOut(%flag);
		  	%flag.dropFade= "";
			%flag.fadeOut= 1;
		   schedule("Flag::checkReturn(" @ %flag @ ", " @ %sequenceNum @ ");", 2.5);
		}
		else {
   	   %flagTeam = GameBase::getTeam(%flag);
   	   if(%flagTeam == -1)
   	   {
   	      if(%flag.flagStand == "" || %flag.flagStand.flag != "") {
					MessageAll(0, %flag.objectiveName @ " was returned to its initial position.");
				   GameBase::setPosition(%flag, %flag.originalPosition);
               Item::setVelocity(%flag, "0 0 0");
				   %flag.flagStand = "";
   			}
   			else
   			{
   	         %holdTeam = GameBase::getTeam(%flag.flagStand);
					TeamMessages(0, %holdTeam, "Your flag was returned to base.~wflagreturn.wav", -2, "", "The " @ getTeamName(GameBase::getTeam(%flag.flagStand)) @ " flag was returned to base.~wflagreturn.wav");
				   GameBase::setPosition(%flag, GameBase::getPosition(%flag.flagStand));
               %flag.flagStand.flag = %flag;
				   %flag.holdingTeam = %holdTeam;
				   %flag.carrier = -1;
				   $teamScore[%holdTeam] += %flag.scoreValue;
				   $deltaTeamScore[%holdTeam] += %flag.deltaTeamScore;
				   %flag.holder = %flag.flagStand;
				   TeamMessages(0,%holdTeam, "Your team holds " @ %flag.objectiveName @ ".~wflagcapture.wav", -2, "", "The " @ getTeamName(%playerTeam) @ " team holds " @ %flag.objectiveName @ ".");
				   ObjectiveMission::checkScoreLimit();
			   }
			}
         else
         {
   	      TeamMessages(0, %flagTeam, "Your flag was returned to base.~wflagreturn.wav", -2, "", "The " @ getTeamName(%flagTeam) @ " flag was returned to base.~wflagreturn.wav");
				GameBase::setPosition(%flag, %flag.originalPosition);
            Item::setVelocity(%flag, "0 0 0");
         }
         %flag.atHome = true;
			GameBase::startFadeIn(%flag);
   	   %flag.fadeOut= "";
			ObjectiveMission::ObjectiveChanged(%flag);
		}
   }
}

