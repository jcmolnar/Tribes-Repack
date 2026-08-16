$SensorNetworkEnabled = true;

$GuiModePlay = 1;
$GuiModeCommand = 2;
$GuiModeVictory = 3;
$GuiModeInventory = 4;
$GuiModeObjectives = 5;
$GuiModeLobby = 6;


//  Global Variables

//---------------------------------------------------------------------------------
// Energy each team is given at beginning of game
//---------------------------------------------------------------------------------
$DefaultTeamEnergy = "Infinite";

//---------------------------------------------------------------------------------
// Team Energy variables
//---------------------------------------------------------------------------------
$TeamEnergy[-1] = $DefaultTeamEnergy; 
$TeamEnergy[0]  = $DefaultTeamEnergy; 
$TeamEnergy[1]  = $DefaultTeamEnergy; 
$TeamEnergy[2]  = $DefaultTeamEnergy; 
$TeamEnergy[3]  = $DefaultTeamEnergy; 
$TeamEnergy[4]  = $DefaultTeamEnergy; 
$TeamEnergy[5]  = $DefaultTeamEnergy; 
$TeamEnergy[6]  = $DefaultTeamEnergy; 				
$TeamEnergy[7]  = $DefaultTeamEnergy; 

//---------------------------------------------------------------------------------
// If 1 then Team Spending Ignored -- Team Energy is set to $MaxTeamEnergy every
// 	$secTeamEnergy.
//---------------------------------------------------------------------------------
$TeamEnergyCheat = 0;

//---------------------------------------------------------------------------------
// MAX amount team energy can reach
//---------------------------------------------------------------------------------
$MaxTeamEnergy = 700000;

//---------------------------------------------------------------------------------
// Amount to inc team energy every ($secTeamEnergy) seconds
//---------------------------------------------------------------------------------
$incTeamEnergy = 0;

//---------------------------------------------------------------------------------
// (Rate is sec's) Set how often TeamEnergy is incremented
//---------------------------------------------------------------------------------
$secTeamEnergy = 30;

//---------------------------------------------------------------------------------
// (Rate is sec's) Items respwan
//---------------------------------------------------------------------------------
$ItemRespawnTime = 30;

//---------------------------------------------------------------------------------
//Amount of Energy remote stations start out with
//---------------------------------------------------------------------------------
$RemoteAmmoEnergy = 100000000;
$RemoteInvEnergy = 100000000;

//---------------------------------------------------------------------------------
// TEAM ENERGY -  Warn team when teammate has spent x amount - Warn team that 
//				  energy level is low when it reaches x amount 
//---------------------------------------------------------------------------------
$TeammateSpending = 0; 		 //Set = to 0 if don't want the warning message
$WarnEnergyLow = 0;		 //Set = to 0 if don't want the warning message

//---------------------------------------------------------------------------------
// Amount added to TeamEnergy when a player joins a team
//---------------------------------------------------------------------------------
$InitialPlayerEnergy = "Infinite";

//---------------------------------------------------------------------------------
// REMOTE TURRET
//---------------------------------------------------------------------------------
$MaxNumTurretsInBox = 20;    	//Number of remote turrets allowed in the area
$TurretBoxMaxLength = 50;    	//Define Max Length of the area
$TurretBoxMaxWidth =  50;    	//Define Max Width of the area
$TurretBoxMaxHeight = 25;    	//Define Max Height of the area

$TurretBoxMinLength = 2;	//Define Min Length from another turret
$TurretBoxMinWidth =  2;	//Define Min Width from another turret
$TurretBoxMinHeight = 2;    	//Define Min Height from another turret

//---------------------------------------------------------------------------------
//	Object Types	
//---------------------------------------------------------------------------------
$SimTerrainObjectType    = 1 << 1;
$SimInteriorObjectType   = 1 << 2;
$SimPlayerObjectType     = 1 << 7;

$MineObjectType		    = 1 << 26;	
$MoveableObjectType	    = 1 << 22;
$VehicleObjectType	 	 = 1 << 29;  
$StaticObjectType			 = 1 << 23;	   
$ItemObjectType			 = 1 << 21;	  

//---------------------------------------------------------------------------------
// CHEATS
//---------------------------------------------------------------------------------
$ServerCheats = 0;
$TestCheats = 0;

//---------------------------------------------------------------------------------
//Respawn automatically after X sec's -  If 0..no respawn
//---------------------------------------------------------------------------------
$AutoRespawn = 0;

//---------------------------------------------------------------------------------

function Time::getMinutes(%simTime)
{
	dbecho($dbechoMode, "Time::getMinutes(" @ %simTime @ ")");

	return floor(%simTime / 60);
}

function Time::getSeconds(%simTime)
{
	dbecho($dbechoMode, "Time::getSeconds(" @ %simTime @ ")");

	return %simTime % 60;
}


function UpdateClientTimes(%time)
{
	dbecho($dbechoMode, "UpdateClientTimes(" @ %time @ ")");

	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		remoteEval(%cl, "setTime", -%time);
}

function Game::notifyMatchStart(%time)
{
	dbecho($dbechoMode, "Game::notifyMatchStart(" @ %time @ ")");

	messageAll(0, "Match starts in " @ %time @ " seconds.");
	UpdateClientTimes(%time);
}



function onServerGhostAlwaysDone()
{
}

function GameBase::getHeatFactor(%this)
{
	return 0.0;
}

// Need to add this to banker response to player
// remoteEval(%clientId, "setCMMode", "BankerChatMenu", 2);
// This should reside inside game.cs so it gets transferred
// to the player. In the future it could simply be part of
// the Client DL.  - *IX*Savage1

newObject(BankerChatMenu, ChatMenu, "Banker Menu:");

function addBankerChat(%text, %msg, %sound)
{
   if(%sound != "")
   {
      %msg = %msg @ "~w" @ %sound;
   }
   if($curPlayChatMenu != "")
   {
      %text = $curPlayChatMenu @ "\\" @ %text;
   }
   addCMCommand(BankerChatMenu, %text, say, 0, %msg);
}

addBankerChat("wWithdraw", "withdraw");
addBankerChat("dDeposit", "deposit");
