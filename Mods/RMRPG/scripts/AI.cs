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

$AIattackMode = 1;

$botinitmana = 9999;
//---------------------------------
//createAI()
//---------------------------------
function createAI(%aiName, %markerGroup, %name)
{
	dbecho($dbechoMode, "createAI(" @ %aiName @ ", " @ %markerGroup @ ", " @ %name @ ")");

	%group = nameToID( %markerGroup );

	if( %group == -1 || Group::objectCount(%group) == 0 )
	{
	      %spawnPos = %markerGroup;
	      %spawnRot = "0 0 0";
	}
	else
	{
		for(%i = 0; %i < Group::objectCount(%group); %i++)
		{
			%obj = Group::getObject(%group, %i);
			if(getObjectType(%obj) != "SimGroup")
				break;
		}
	      %spawnMarker = Group::getObject(%group, %i);
	      %spawnPos = GameBase::getPosition(%spawnMarker);
	      %spawnRot = GameBase::getRotation(%spawnMarker);
	}

	%guardtype = clipTrailingNumbers(%aiName);

	if($BotInfo[%guardtype, RACE] != "")
		%armor = $RaceToArmorType[$BotInfo[%guardtype, RACE]];	//bots in map will get this call
	else
		%armor = $RaceToArmorType[$NameForRace[%guardtype]];		//spawn bots will get this call

	if( AI::spawn( %aiName, %armor, %spawnPos, %spawnRot, %name, "male2" ) != "false" )
	{
		%AiId = AI::getId(%aiName);
		ClearVariables(%AiId);

		$BotInfoAiName[%AiId] = %aiName;

		$RACE[%AiId] = $ArmorTypeToRace[%armor];

		$LCKconsequence[%AiId] = "miss";
		$HasLoadedAndSpawned[%AiId] = True;
		$botAttackMode[%AiId] = 1;
		$tmpbotdata[%AiId] = "";
		$templvl[%AiId] = "";
		$ClientData[%AiId, UsingWeapon] = "-1";
		$ClientData[%AiId, AI::SetUpWeaps] = true; //lets RegWeapons build
		$ClientData[%AiId, RegWeapons] = ""; //will build on GiveThisStuff
		$ClientData[%AiId, MaxCOINS] = 99999999;


		setMANA(%AiId, $botinitmana);
		refreshMANAREGEN(%AiId);

		if(%group != -1) {
			// The order number is used for sorting waypoints, and other directives.
			%orderNumber = 200;

			for(%i = 0; %i < Group::objectCount(%group); %i++) {
				%spawnMarker = Group::getObject(%group, %i);
				if(getObjectType(%spawnMarker) != "SimGroup") {
					%spawnPos = GameBase::getPosition(%spawnMarker);

					AI::DirectiveWaypoint( %aiName, %spawnPos, %orderNumber );

					%orderNumber++;
				}
			}

			AI::setAutomaticTargets(%aiName);
		}

		GameBase::startFadeIn(%AiId);
		PlaySound(SoundSpawn2, %spawnPos);
	}
	else {
      	dbecho(1, "Failure spawning bot:");
		dbecho(1, "%aiName: " @ %aiName);
		dbecho(1, "%armor: " @ %armor);
		dbecho(1, "%guardtype: " @ %guardtype);
		dbecho(1, "%spawnPos: " @ %spawnPos);
		dbecho(1, "%spawnRot: " @ %spawnRot);
		dbecho(1, "%name: " @ %name);
		return -1;
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
//	dbecho($dbechoMode, "AI::setupAI(" @ %key @ ", " @ %team @ ")");

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
		if( %aiFound == 0 )
			dbecho(1, "No drones exist...");
		else
			dbecho(1, %aiFound @ " drones installed..." );

		$numAi = %aiFound;
	}
	else     //respawning dead AI with original name and path
	{
		%group = nameToID("MissionGroup\\Teams\\team" @ %team @ "\\AI\\" @ %key);
		%num = Group::objectCount(%group);
		createAI(%key, %group, $BotInfo[%key, NAME]);
		%aiId = AI::getId(%key);

		GameBase::setTeam(%aiId, %team);
		AI::setVar(%key, pathType, $AI::defaultPathType);
		AI::setWeapons(%key);

		//**RPG (added because AI::onDroneKilled doesn't conserve the AI's team)
		$botTeam[%aiId] = %team;
		//**
	}
}

//------------------------------
// AI::setWeapons()
//------------------------------
function AI::setWeapons(%aiName, %loadout, %IsMaster) {

	%aiId = AI::getId(%aiName);

	$Weight[%aiId] = 0;
	$ClientData[%aiId, "ItemList"] = " ";
	$ClientData[%aiId, "EquipList"] = " ";
	$ClientData[%aiId, "QuestList"] = " ";
	$ClientData[%aiId, UsingWeapon] = "-1";

	%Cname = clipTrailingNumbers(%aiName); //echo("AI::setWeapons: "@%CName);

	//----
	//Allows us to kill off bots in emtpy zones, and re-spawn them with the old loot (in-case a player's loot was taken by a bot)
	%hasStuff = false;
	for(%i = 0; %i <= $tmpBotStuffMAX; %i++) {
		if($tmpBotStuff[%CName, %i] != "") {

			GiveThisStuff(%aiId, $tmpBotStuff[%CName, %i]);
			$tmpBotStuff[%CName, %i] = "";

			%hasStuff = true;

			if($tmpBotStuffEquip[%CName, %i] != "") {
				GiveThisStuff(%aiId, $tmpBotStuffEquip[%CName, %i]);
				$tmpBotStuffEquip[%CName, %i] = "";
			}
			break;
		}
	}
	//----

	if(!%hasStuff) {

		%items = $BotInfo[%aiName, ITEMS];

		if(%items == "")
			GiveThisStuff(%aiId, $BotEquipment[%Cname]);
		else
			GiveThisStuff(%aiId, %items);
	}

	AI::HardCodedSkills(%aiId, $CLASS[%aiId], %IsMaster);

	setHP(%aiId, AddHP(%AiId));
	refreshHPREGEN(%aiId);


	$ClientData[%AiId, AI::SetUpWeaps] = ""; //No more weapons to give. This will
													//keep players from droping crappy
	Game::refreshClientScore(%aiId);			//weaps on big bots and not having
													//to do a huge loop to find the *best*
													//weap to use. Use AI::NextWeapon
	AI::SetVar(%aiName, triggerPct, 1.0 );
	AI::setVar(%aiName, iq, 100 );
	AI::setVar(%aiName, attackMode, $AIattackMode);
	AI::setAutomaticTargets( %aiName );

	AI::callbackPeriodic(%aiName, 5, AI::Periodic);

	if(%IsMaster) {
		schedule("DoRandomBotEvents(\""@%CName@"\", \""@%aiId@"\");", 1);
	}

	AI::NextWeapon(%aiId);	//this way the bot spawns and has a weapon in hand
}

function AI::HardCodedSkills(%aiId, %class, %IsMaster) {

	%lvl = getFinalLVL(%aiId);
	if(%lvl <= 0)
		return False;

	%skill = Cap(floor(%lvl / 45), 1, 2) * %lvl;

	$AP[%aiId, 1] += ($AddAPSTRClass[%class] * 6) + %skill;//STR
	$AP[%aiId, 2] += ($AddAPDEXClass[%class] * 6) + %skill;
	$AP[%aiId, 3] += ($AddAPCONClass[%class] * 8) + Cap(floor(%lvl / 40), 2, 3) * %lvl; // CON
	$AP[%aiId, 4] += ($AddAPINTClass[%class] * 6) + %skill;
	$AP[%aiId, 5] += ($AddAPWISClass[%class] * 6) + %skill;
	//$AP[%aiId, 6] = Cap(%lvl / 45, 1, 3); // Bots don't need STA =P

	if(%IsMaster) {
		%skill = floor(%skill / 2)+1;
		$AP[%aiId, 1] += %skill;
		$AP[%aiId, 2] += %skill;
		$AP[%aiId, 3] += %skill;
		$AP[%aiId, 4] += %skill;
		$AP[%aiId, 5] += %skill;
	}

	if(%lvl > 30) {
		%skill = Cap(floor(%lvl / 50), 0, 20);
		$AttackBonus[$SkillSlashing, %AiId] = %skill;
		$AttackBonus[$SkillBludgeoning, %AiId] = %skill;
		$AttackBonus[$SkillPiercing, %AiId] = %skill;
		$AttackBonus[$SkillArchery, %AiId] = %skill;
	}

	$ClientData[%AiId, MaxCOINS] = 9999999;

	$COINS[%AiId] += %lvl * floor(getRandom()*100)+1;

	if(%lvl <= 2)
		$AP[%aiId, 1] -= 10;

	return True;
}

function AI::Periodic(%aiName) {

	%aiId = AI::getId(%aiName);

	if($dumbAIflag[%aiId] || $frozen[%aiId])
		return;

	%aiTeam = GameBase::getTeam(%aiId);
	%aiPos = GameBase::getPosition(%aiId);

	//=================================================================
	//Everytime this function is called, the bot looks at all clients
	//(including bots) and sets a waypoint to the nearest one that is
	//NOT on the same team, and that it can see in its FOV (by
	//comparing Rotations).  These loops will undoubtedly cause alot
	//of CPU drain.
	//=================================================================

	if($SpawnBotInfo[%aiId] != "" && !$ClientData[%aiId, Blind] > 0 && $SpellCastStep[%aiId] != 1)
	{
		if($AIsmartFOVbots)
		{
			//find all clients that COULD be in the bot's FOV and that are not on the same team.
			%aiTeam = GameBase::getTeam(%aiId);
			%aiPos = GameBase::getPosition(%aiId);
			%aiRot = GetWord(GameBase::getRotation(%aiId), 2);

		//	%list = GetEveryoneIdList();
			%list = GetPlayerIdList();
			for(%i = 0; GetWord(%list, %i) != -1; %i++)
			{
				%id = GetWord(%list, %i);
				if(GameBase::getTeam(%id) != %aiTeam && !$invisible[%id])
				{
					%vec = Vector::sub(GameBase::getPosition(%id), %aiPos);
					%vecRot = GetWord(Vector::getRotation(%vec), 2);

					if(%vecRot >= %aiRot - $AIFOVPan && %vecRot <= %aiRot + $AIFOVPan)
					{
						%dist = Vector::getDistance(%aiPos, GameBase::getPosition(%id));
						if(%dist < %closest) {
							%closest = %dist;
							%closestId = %id;
						}
					}
				}
			}
			if(%closestId != "" && %closest <= $AImaxRange)
			{
				//echo(%aiId @ ": targeting (moving towards) " @ %closestId @ ", " @ %closest @ " meters away");
				if(%closest <= 10)
					AI::newDirectiveWaypoint(%aiName, GameBase::getPosition(%closestId), 99);
				else
					AI::newDirectiveFollow(%aiName, %closestId, 0, 99);
			}
		}
		else
		{
			//==============================================================
			// I'm making it so bots can "smell" their target. Basically,
			// if you're close enough to them, they will lock onto you.
			// I doubt this loop will cause much lag... *fingers crossed*
			//==============================================================

			if(!$noBotSniff[%aiId])
			{
				%closest = 500000;

				%flag = "";
				%b = $AImaxRange * 2;
				%set = newObject("set", SimSet);
				%n = containerBoxFillSet(%set, $SimPlayerObjectType, %aiPos, %b, %b, 10, 0); //Made it not check Z to much.. (keeps them from going crazy when your on a higher floor level)
				for(%i = 0; %i < Group::objectCount(%set); %i++)
				{
					%id = Player::getClient(Group::getObject(%set, %i));
					if(GameBase::getTeam(%id) != %aiTeam && !$invisible[%id])
					{
						%dist = Vector::getDistance(%aiPos, GameBase::getPosition(%id));
						if(%dist < %closest)
						{
							%closest = %dist;
							%closestId = %id;
						}
					}
				}
				deleteObject(%set);

				if(%closest <= $AImaxRange)
				{
					if(%closest <= 10)
						AI::newDirectiveWaypoint(%aiName, GameBase::getPosition(%closestId), 99);
					else
						AI::newDirectiveFollow(%aiName, %closestId, 0, 99);

					PlaySound(RandomRaceSound($RACE[%aiId], Acquired), GameBase::getPosition(%aiId));
				}
				else
					%flag = True;
			}

			if(%flag || $noBotSniff[%aiId])
			{
				AI::SelectMovement(%aiName);
			}
		}
	}
	//=================================================================
	//1 thru 4 = animation 10, 5 thru 10 = animation 11, else do nothing
	//=================================================================
	%vel = Item::getVelocity(%aiId);
	if(%vel == "0 0 0") {
		%r = floor(getRandom() * 200)+1;
		if(%r >= 1 && %r <= 4)
			RemotePlayAnim(%aiId, 10);
		else if(%r >= 5 && %r <= 10)
			RemotePlayAnim(%aiId, 11);

		if(GameBase::getTeam(%aiId) > 1)
		{
			if(OddsAre(5))
				RemotePlayAnim(%aiId, 2);
		}

		if(OddsAre(2)) {

			// is AI looking at a wall?
			//echo("checking for wall... "@%player@"...");
			if(GameBase::getLOSinfo(%aiId, 3)) {

				%obj = getObjectType($los::object);
				if(%obj == "InteriorShape") {

					//echo("looking at wall...");

					%furthest = -1;
					for(%i = 0; %i <= 6.283; %i += 1.04) { // 0.52

						GameBase::getLOSinfo(%aiId, 25, "0 0 "@%i);
						%dist = Vector::getDistance(%aiPos, $los::position);

						if($los::position != "0 0 0" && $los::position != "") {
							if(%dist > %furthest) {
								%furthest = %dist;
								%chosenPos = $los::position;
							}
							else {
								if(%dist == %furthest) {
									if(OddsAre(3)) {
										%furthest = %dist;
										%chosenPos = $los::position;
									}
								}
							}

						}
					}
					if(%furthest == -1) {
						//echo("moveSomewhere...");
						AI::moveSomewhere(%aiName);
					}
					else {
						//echo("moving to "@%chosenPos@".. ");
						AI::newDirectiveWaypoint(%aiName, %chosenPos, 99);
					}
				}
			}
		}
	}
	//else {
		if(OddsAre(4)) {
			%jumpDir = Vector::getFromRot(GameBase::getRotation(%aiId),%vel, $JumpVelForRace[$RACE[%aiId]]);
			Schedule("Player::applyImpulse(\""@%aiId@"\", \""@%jumpDir@"\");", 1);
		}
//	}
	//=============================================
	//do other stuff...
	//=============================================
	//%curTarget = ai::getTarget(%aiName);

	if(OddsAre(4))
		AI::NextWeapon(%aiId);

	if(OddsAre(2))
		remotetoggleShield(%aiId);
}

function AI::NextWeapon(%aiId) {

	%weap = AI::getRandomItemFromList($ClientData[%AiId, RegWeapons]);
	if(%weap != -1)
		AI::mountItem(%AiId, %weap, $WeaponSlot);

	AI::SetSpotDist(%aiId);
}

function AI::getRandomItemFromList(%list) {

	for(%i = 0; (%w[%i] = getWord(%list, %i)) != -1; %i++){}
	%r = (floor(getRandom() * %i));
	return %w[%r];
}


function AI::mountItem(%Client, %item, %slot) {

	%weapon = $ItemData[%item, shape];

	if(%weapon == "NULL") {
		echo("Error: RPGmountItem(); %item "@%item@" shape == NULL --  This should not happen!");
		return false;
	}

	if(IsDead(%Client))
		return "dead";

	$ClientData[%Client, UsingWeapon] = %item;

	if(!playSound($ItemData[%item, ASound], GameBase::getPosition(%Client)))
		echo("SoundData "@$ItemData[%item, ASound]@" undefined. Item "@%item@" called this. (AI::mountItem)");

	Player::setItemCount(%Client, %weapon, 1);
	Player::mountItem(%Client, %weapon, %slot);

	return true;
}

function AI::SetSpotDist(%aiId)
{
	if($dumbAIflag[%aiId])
		return;

	%item = $ClientData[%AiId, UsingWeapon]; //Player::getMountedItem(%aiId, $WeaponSlot);
	if($ItemData[%item, type] == $RangedAccessoryType)
	{
		DefaultRangedProj(%aiId, %item);
		AI::setVar($BotInfoAiName[%aiId], SpotDist, 60);
	}
	else
		AI::setVar($BotInfoAiName[%aiId], SpotDist, $ItemData[%item, Range]);
}

function AI::activelyFollow(%aiName, %curTarget, %bypass)
{
//	dbecho($dbechoMode, "AI::activelyFollow(" @ %aiName @ ", " @ %curTarget @ ", " @ %bypass @ ")");

	%aiId = ai::getId(%aiName);

	if(GameBase::getTeam(%aiId) != GameBase::getTeam(%curTarget) || %bypass)
	{
		//echo("Sending " @ %aiName @ " to actively follow and attack " @ %curTarget);
		AI::newDirectiveFollow(%aiName, %curTarget, 0, 99);
	}
}

function AI::moveToAttackMarker(%name, %method)
{
//	dbecho($dbechoMode, "AI::moveToAttackMarker(" @ %name @ ", " @ %method @ ")");

	%AIid = AI::getId(%name);

	if($dumbAIflag[%aiId])
		return;

      %tempSet = nameToID("MissionGroup\\Teams\\team1\\AIattackMarkers");

	if(%tempSet != -1)
	{
		%num = Group::objectCount(%tempSet);

		if(%method == 0)
		{
			//pick a random marker
			%r = floor(getRandom() * %num);
		}
		else if(%method == 1)
		{
			//pick nearest marker
			%dist = 1000000;
			for(%i=0; %i<=%num-1; %i++)
			{
				%m = Group::getObject(%tempSet, %i);
				%testdist = Vector::getDistance(GameBase::getPosition(%AIid), GameBase::getPosition(%m));
				if(%testdist < %dist)
				{
					%dist = %testdist;
					%r = %i;
				}
			}
		}
	      %marker = Group::getObject(%tempSet, %r);

		%worldLoc = GameBase::getPosition(%marker);

		AI::newDirectiveWaypoint(%name, %worldLoc, 99);
		$AIattackMarker[%AIid] = %marker;

		//echo(%name @ " IS PROCEEDING TO LOCATION " @ %worldLoc);

		return True;
	}
	return False;
}

function AI::moveSomewhere(%aiName)
{
//	dbecho($dbechoMode, "AI::moveSomewhere(" @ %aiName @ ")");

	%aiId = AI::getId(%aiName);

	if($dumbAIflag[%aiId])
		return;

	//echo(%aiId @ " ======> Ok, i'm considering giving the bot a new place to go.");
	if($AIattackMarker[%aiId] != "")
	{
		%dist = Vector::getDistance(GameBase::getPosition($AIattackMarker[%aiId]), GameBase::getPosition(%aiId));
		//echo(%aiId @ " ======> It seems this bot has an attackMarker, and it's this far: " @ %dist);
		if(%dist <= $AIcloseEnoughMarkerDist)
		{
			//echo(%aiId @ " ======> The distance " @ %dist @ " seems to be close enough to cancel the attackMarker!");
			$AIattackMarker[%aiId] = "";
		}
	}
	if($AIattackMarker[%aiId] == "")
	{
		//echo(%aiId @ " ======> The bot has no attack marker, so I'm attempting to give bot somewhere to go!");
		%aiPos = GameBase::getPosition(%aiId);
		%minrad = $AIminrad;
		%maxrad = $AImaxrad;

		%tempPos = RandomPositionXY(%minrad, %maxrad);

		%xPos = GetWord(%tempPos, 0) + GetWord(%aiPos, 0);
		%yPos = GetWord(%tempPos, 1) + GetWord(%aiPos, 1);
		%zPos = GetWord(%aiPos, 2); //doesn't matter; the bot can't go thru terrain

		%newPos = %xPos @ " " @ %yPos @ " " @ %zPos;

		$AIattackMarker[%aiId] = "";
		AI::newDirectiveWaypoint(%aiName, %newPos, 99);

		//echo(%aiName @ " IS WANDERING TO LOCATION " @ %newPos);
	}
}

//experimental function, which makes bot look around himself at preset angles and, whichever is the furthest, go there.
//i'm hoping this will simulate a smarter bot.

//test results: seems to work great!  hopefully it doesn't cause too much lag, but up to now... looks fine
function AI::moveToFurthest(%aiName)
{
//	dbecho($dbechoMode, "AI::moveToFurthest(" @ %aiName @ ")");

	%aiId = AI::getId(%aiName);

	if($dumbAIflag[%aiId])
		return;

	if($AIattackMarker[%aiId] != "")
	{
		%dist = Vector::getDistance(GameBase::getPosition($AIattackMarker[%aiId]), GameBase::getPosition(%aiId));
		if(%dist <= $AIcloseEnoughMarkerDist)
		{
			$AIattackMarker[%aiId] = "";
		}
	}
	if($AIattackMarker[%aiId] == "")
	{
		%aiPos = GameBase::getPosition(%aiId);
		%aiPlayer = Client::getOwnedObject(%aiId);

		%furthest = -1;
		for(%i = 0; %i <= 6.283; %i+= 0.52)
		{
			GameBase::getLOSinfo(%aiPlayer, 1000, "0 0 " @ %i);
			%dist = Vector::getDistance(%aiPos, $los::position);
			if(%dist > %furthest && $los::position != "0 0 0" && $los::position != "")
			{
				%furthest = %dist;
				%chosenPos = $los::position;
			}
		}
		if(%furthest == -1)
		{
			//it seems the bot only sees sky, so revert to AI::moveSomewhere
			AI::moveSomewhere(%aiName);
			return;
		}

		%finalPos = %chosenPos;

		AI::newDirectiveWaypoint(%aiName, %finalPos, 99);

		//echo(%aiName @ " FOUND THE FURTHEST POINT AT LOCATION " @ %chosenPos);
	}
}


//-----------------------------------
// AI::initDrones()
//-----------------------------------
function AI::initDrones(%team, %numAi)
{
	dbecho($dbechoMode, "AI::initDrones(" @ %team @ ", " @ %numAi @ ")");

	dbecho(1, "spawning team " @ %team @ " ai...");
	for(%guard = 0; %guard < %numAi; %guard++)
	{
		//check for internal data
		%tempSet = nameToID("MissionGroup\\Teams\\team" @ %team @ "\\AI");
		%tempItem = Group::getObject(%tempSet, %guard);
		%aiName = Object::getName(%tempItem);

		%set = nameToID("MissionGroup\\Teams\\team" @ %team @ "\\AI\\" @ %aiName);
		%numPts = Group::objectCount(%set);

		if(%numPts > 0)
		{
			GatherBotInfo(%set);

			createAI(%aiName, %set, $BotInfo[%aiName, NAME]);
			%aiId = AI::getId( %aiName );
			AI::setVar( %aiName, iq,  100 );
			AI::setVar( %aiName, attackMode, $AIattackMode);
			AI::setVar( %aiName, pathType, $AI::defaultPathType);
			AI::setWeapons(%aiName);

			UpdateTeam(%aiId);

			//**RPG (added because AI::onDroneKilled doesn't conserve the AI's team)
			$botTeam[%aiId] = %team;
			//**
		}
		else
			dbecho(1, "no info to spawn ai...");
	}
}


//------------------------------------------------------------------
//functions to test and move AI players.
//
//------------------------------------------------------------------

$numAI = 0;
function AI::helper(%aiName, %displayName, %commandIssuer, %loadout)
{
//	dbecho($dbechoMode, "AI::helper(" @ %aiName @ ", " @ %displayName @ ", " @ %commandIssuer @ ")");

	if(GetWord(%commandIssuer, 0) == "TempSpawn")
	{
		//the %commandIssuer is a data string
		%spawnPos = GetWord(%commandIssuer, 1) @ " " @ GetWord(%commandIssuer, 2) @ " " @ GetWord(%commandIssuer, 3);
	}
	else if(GetWord(%commandIssuer, 0) == "MarkerSpawn")
	{
		//the %commandIssuer is a marker
		%spawnPos = GameBase::getPosition(GetWord(%commandIssuer, 1));
	}
	else if(GetWord(%commandIssuer, 0) == "SpawnPoint")
	{
		//the %commandIssuer is a Spawn Point
		//we must now figure out a position around this Spawn Point

		%spawnpoint = GetWord(%commandIssuer, 1);

		//%info = $BotSpawnInfo::Data[%spawnpoint, info]; //Object::getName(%spawnpoint);

		%minrad = $BotSpawnInfo::Data[%spawnpoint, minrad]; //GetWord(%info, 1);
		%maxrad = $BotSpawnInfo::Data[%spawnpoint, maxrad]; //GetWord(%info, 2);

		%spawnPointPos = GameBase::getPosition(%spawnpoint);
		%tempPos = RandomPositionXY(%minrad, %maxrad);
		%xPos = GetWord(%tempPos, 0) + GetWord(%spawnPointPos, 0);
		%yPos = GetWord(%tempPos, 1) + GetWord(%spawnPointPos, 1);
		%zPos = GetWord(%spawnPointPos, 2);

		%spawnPos = %xPos @ " " @ %yPos @ " " @ %zPos;
	}

	%n = getAInumber();

	%IsMaster = false;
	%newName = %aiName @ %n;
//echo(%aiName@" - "@%displayName@" - "@%newName);
	if(%aiName == %displayName) {
		if($MasterBot[%aiName] != "") {
			if((%IsMaster = SetRandomBotEvents(%aiName)) == true || $testMaster) {
				if($testMaster) %IsMaster = $testMaster;
				%displayName = "Master"@$NameForRace[%aiName]@%newName;
			}
			else
				%displayName = $NameForRace[%aiName] @ %newName;
		}
		else
			%displayName = $NameForRace[%aiName] @ %newName;
	}
	$numAI++;
//echo("SpawnAI(" @ %newName @ ", " @ %displayName @ ", " @ %aiSpawnPos @ ", " @ %commandIssuer @ ");");

	if(SpawnAI(%newName, %displayName, %spawnPos, %commandIssuer, %loadout, %IsMaster) == -1) {
		$AI::FailedToSpawnBot++;
		echo("FAILED TO SPAWN BOT -- SpawnAI(" @ %newName @ ", " @ %displayName @ ", " @ %aiSpawnPos @ ", " @ %commandIssuer @ ") ");
		return "ShazBot$"@ $AI::FailedToSpawnBot;
	}

	setAInumber(%newName, %n);

	return %newName;
}
function SpawnAI(%newName, %displayName, %aiSpawnPos, %commandIssuer, %loadout, %IsMaster)
{
	dbecho($dbechoMode, "SpawnAI(" @ %newName @ ", " @ %displayName @ ", " @ %aiSpawnPos @ ", " @ %commandIssuer @ ")");

	%retval = createAI(%newName, %aiSpawnPos, %displayName);

	if(%retval != -1)
	{
		%aiId = AI::getId( %newName );
		AI::setVar( %newName,  iq,  100 );
		AI::setVar( %newName,  attackMode, $AIattackMode);
		AI::setVar( %newName,  pathType, $AI::defaultPathType);
		//AI::SetVar( %newName,  seekOff, 1);
		AI::setAutomaticTargets( %newName );

		if(GetWord(%commandIssuer, 0) == "TempSpawn")
		{
			//the %commandIssuer is a data string
			$SpawnBotInfo[%aiId] = %commandIssuer;
			%team = GetWord(%commandIssuer, 4);
			GameBase::setTeam(%aiId, %team);

			AI::SetVar(%newName, spotDist, $AIspotDist);
		}
		else if(GetWord(%commandIssuer, 0) == "MarkerSpawn")
		{
			//the %commandIssuer is a marker
			$SpawnBotInfo[%aiId] = %commandIssuer;
			%team = GameBase::getMapName(GetWord(%commandIssuer, 1));
			if(%team == "") %team = 0;
			GameBase::setTeam(%aiId, %team);

			AI::SetVar(%newName, spotDist, $AIspotDist);
		}
		else if(GetWord(%commandIssuer, 0) == "SpawnPoint")
		{
			//the %commandIssuer is a spawn crystal
			$SpawnBotInfo[%aiId] = %commandIssuer;

			$numAIperSpawnPoint[GetWord(%commandIssuer, 1)]++;
			UpdateTeam(%aiId);

			AI::SetVar(%newName, spotDist, $AIspotDist);
		}

		AI::setWeapons(%newName, %loadout, %IsMaster);

		return ( %newName );
	}
	else
	{
		return -1;
	}
}

//
//This function will move an AI player to the position of an object
//that the players LOS is hitting(terrain included). Must be within 50 units.
//
//
function AI::moveToLOS(%aiName, %commandIssuer)
{
//	dbecho($dbechoMode, "AI::moveToLos(" @ %aiName @ ", " @ %commandIssuer @ ")");

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
		AI::newDirectiveWaypoint( %aiName, %newIssuedVec, 99 );
	}
	else
		dbecho(2, "Distance too far.");

	dbecho(2, "LOS point: " @ $LOS::position);
}

//This function will move an AI player to a position directly in front of
//the player passed, at a distance that is specified.
function AI::moveAhead(%aiName, %commandIssuer, %distance)
{
//	dbecho($dbechoMode, "AI::moveAhead(" @ %aiName @ ", " @ %commandIssuer @ ", " @ %distance @ ")");

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
	AI::newDirectiveWaypoint(%aiName, %newPos, 99);
}

//
// OK, this is the complete command callback - issued for any command sent
//    to an AI.
//
function AI::onCommand ( %name, %commander, %command, %waypoint, %targetId, %cmdText, %cmdStatus, %cmdSequence)
{
//	dbecho($dbechoMode, "AI::onCommand(" @ %name @ ", " @ %commander @ ", " @ %command @ ", " @ %waypoint @ ", " @ %targetId @ ", " @ %cmdText @ ", " @ %cmdStatus @ ", " @ %cmdSequence @ ")");

	%aiId = AI::getId( %name );
	%T = GameBase::getTeam( %aiId );
	%groupId = nameToID("MissionGroup\\Teams\\team" @ %T @ "\\AI\\" @ %name );
	%nodeCount = Group::objectCount( %groupId );
	dbecho(2, "checking drone information...." @ " number of nodes: " @ %nodeCount);
	dbecho(2, "AI id: " @ %aiId @ " groupId: " @ %groupId);

	if($SinglePlayer) // || %nodeCount == 1
	{
		if( %command == 2 || %command == 1 )
		{
			// must convert waypoint location into world location.  waypoint location
			//    is given in range [0-1023, 0-1023].
			%worldLoc = WaypointToWorld ( %waypoint );
			AI::newDirectiveWaypoint( %name, %worldLoc, 99 );
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
// name without voice base info (which it will grab for you from the source clientId).
// Basically does some string fiddling for you.
//
// Example:
//    Ai::soundHelper( 2051, 2049, cheer3 );
//
function Ai::soundHelper( %sourceId, %destId, %waveFileName )
{
//	dbecho($dbechoMode, "Ai::soundHelper(" @ %sourceId @ ", " @ %destId @ ", " @ %waveFileName @ ")");

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
//	dbecho($dbechoMode, "AI::onPeriodic(" @ %aiName @ ")");

	echo("onPeriodic() called with " @ %aiName);
}


function AI::onDroneKilled(%aiName)
{
	dbecho($dbechoMode, "AI::onDroneKilled(" @ %aiName @ ")");

	if(!$SinglePlayer )
	{
	      %aiId = AI::getId(%aiName);

      	%team = $botTeam[%aiId];
		$botTeam[%aiId] = "";
		$aiNumTable[$tmpbotn[%aiName]] = "";
		$tmpbotn[%aiName] = "";

		if($SpawnBotInfo[%aiId] != "")
		{
			if(GetWord($SpawnBotInfo[%aiId], 0) == "SpawnPoint")
			{
				//this bot originally spawned from a crystal
				$numAIperSpawnPoint[GetWord($SpawnBotInfo[%aiId], 1)]--;
			}
			$SpawnBotInfo[%aiId] = "";
			$AIattackMarker[%aiId] = "";

			//botgroup stuff
			%b = AI::IsInWhichBotGroup(%aiId);
			if(%b != -1)
				AI::RemoveBotFromBotGroup(%aiId, %b);
		}
		else
		{
			schedule("AI::setupAI(" @ %aiName @ ", " @ %team @ ");", 60);
	      }
	}
	else
	{
		// just in case:
		dbecho( 2, "Non training callback called from Training" );
	}
}

//These will only be invoked if the target is REALLY close to the bot (since the SpotDist is only the range of the
//weapon).  This means that if the bot ever gets close enough to engage in battle, he will try his best to continue
//the fight by following the target.  Once the target is lost or dies, directive 99 will be cancelled and directive
//99 will take over (regular walking, formations etc)
function AI::onTargetLOSAcquired(%aiName, %idNum)
{
//	dbecho($dbechoMode, "AI::onTargetLOSAcquired(" @ %aiName @ ", " @ %idNum @ ")");

	%aiId = AI::getId(%aiName);

	if($SpawnBotInfo[%aiId] != "" && !$dumbAIflag[%aiId] && !$invisible[%idNum])
		AI::newDirectiveFollow(%aiName, %idNum, 0, 99);
}

function AI::onTargetLOSLost(%aiName, %idNum)
{
//	dbecho($dbechoMode, "AI::onTargetLOSLost(" @ %aiName @ ", " @ %idNum @ ")");

	%aiId = AI::getId(%aiName);

	if($SpawnBotInfo[%aiId] != "" && !$dumbAIflag[%aiId])
		AI::newDirectiveRemove(%aiName, 99);
}

function AI::onTargetLOSRegained(%aiName, %idNum)
{
//	dbecho($dbechoMode, "AI::onTargetLOSRegained(" @ %aiName @ ", " @ %idNum @ ")");

	%aiId = AI::getId(%aiName);

	if($SpawnBotInfo[%aiId] != "" && !$dumbAIflag[%aiId] && !$invisible[%idNum])
		AI::newDirectiveFollow(%aiName, %idNum, 0, 99);
}

function AI::onTargetDied(%aiName, %idNum)
{
//	dbecho($dbechoMode, "AI::onTargetDied(" @ %aiName @ ", " @ %idNum @ ")");

	%aiId = AI::getId(%aiName);

	if($SpawnBotInfo[%aiId] != "" && !$dumbAIflag[%aiId])
		AI::newDirectiveRemove(%aiName, 99);
}

function AI::sayLater(%clientId, %id, %msg, %msg2)
{
	//dbecho($dbechoMode, "AI::sayLater(" @ %clientId @ ", " @ %guardId @ ", " @ %message @ ", " @ %look @ ")");

//	Client::sendMessage(%clientId, $MsgBeige, %Name@" tells you, \""@%message@"\"");
	remoteEval(%ClientId, "SetUpQuest", %msg, "", "", %msg2, $TownBot[%id, PIC]);

}

function AI::SayZone(%AiId, %msg) {

	%name = Client::getName(%AiId);
//echo("AI::SayZone("@%AiId@", "@%msg@"); zone="@$zone[%AiId]);
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) {
		if(%cl != %Client && $zone[%cl] == $zone[%AiId])
			Client::sendMessage(%cl, $MsgGreen, "[ZONE] "@%name@" \""@%msg@"\"");
	}
}

function AI::lookAtPlayer(%clientId, %guardId) {

	%clpos = GameBase::getPosition(%clientId);
	%gupos = GameBase::getPosition(%guardId);

	%v1 = Vector::sub(%clpos, %gupos);

	%norm = Vector::normalize(%v1);
	%rot = Vector::getRotation(%norm);

	GameBase::setRotation(%guardId, %rot);

	%gurot = GameBase::getRotation(%guardId);
	%temp = Vector::sub(%rot, %gurot);
	%temp2 = GetWord(%temp, 2);

	if(floor(%temp2) != 0)
		%rot = GetWord(%rot, 0) @ " " @ GetWord(%rot, 1) @ " " @ (GetWord(%rot, 2) + 3.141592654);

	GameBase::setRotation(%guardId, %rot);
	//RotateTownBot(%guardId, %rot);
}


function getAInumber()
{
//	dbecho($dbechoMode, "getAInumber()");

	for(%i = 0; %i <= 5000; %i++)
		if($aiNumTable[%i] == "")
			return %i;

}

function SetAInumber(%aiName, %n)
{
//	dbecho($dbechoMode, "setAInumber(" @ %aiName @ ", " @ %n @ ")");

	$aiNumTable[%n] = True;
	$tmpbotn[%aiName] = %n;
}

//=================================================
function AI::SelectMovement(%aiName)
{
//	dbecho($dbechoMode, "AI::SelectMovement(" @ %aiName @ ")");

	%aiId = AI::getId(%aiName);

	if($botAttackMode[%aiId] == 1)
	{
		//Regular walk

		%r = OddsAre($AImoveChance);

		if(%r && !$frozen[%aiId])
		{
			%s = RandomRaceSound($RACE[%aiId], RandomWait);
			if(%s == "NoSound")
				PlaySound(SoundGrunt1, GameBase::getPosition(%aiId));
			else
				PlaySound(%s, GameBase::getPosition(%aiId));

			AI::moveToFurthest(%aiName);
		}
	}
	else if($botAttackMode[%aiId] == 2)
	{
		//Follow a specific player target

		%followId = $tmpbotdata[%aiId];
		if(%followId != %aiId)
			AI::newDirectiveFollow(%aiName, %followId, 0, 99);
	}
	else if($botAttackMode[%aiId] == 3)
	{
		//Attack at certain position
		AI::newDirectiveWaypoint(%aiName, $tmpbotdata[%aiId], 99);
	}
	else if($botAttackMode[%aiId] == 4)
	{
		//BotGroup formation

		%a = AI::IsInWhichBotGroup(%aiId);
		%g = $tmpBotGroup[%a];

		//NOTE: can't make the bots follow a random bot in the group because at one point or another,
		//the bots will pick a follow combination which will NOT involve the team leader, leaving the
		//team leader alone.
		//This new method involves a North East oriented line.

		for(%i = 1; (%b = GetWord(%g, %i)) != -1; %i++)
		{
			if(%b == %aiId)
				%n = %i-1;
		}

		%followId = GetWord(%g, %n);

		if(!$frozen[%aiId])
		{
			if(%followId != %aiId)
				AI::newDirectiveFollow(%aiName, %followId, 0, 99);
			else
				AI::moveToFurthest(%aiName);					//team leader gets to move.
		}

	}
}

//------ BotGroup stuff ---------------------------------

function AI::IsInWhichBotGroup(%aiId)
{
//	dbecho($dbechoMode, "AI::IsInWhichBotGroup(" @ %aiId @ ")");

	for(%i = 0; (%a = GetWord($BotGroups, %i)) != -1; %i++)
	{
		for(%j = 0; (%b = GetWord($tmpBotGroup[%a], %j)) != -1; %j++)
		{
			if(%b == %aiId)
				return %a;
		}
	}
	return -1;
}

function AI::CreateBotGroup(%group)
{
//	dbecho($dbechoMode, "AI::CreateBotGroup(" @ %group @ ")");

	$BotGroups = $BotGroups @ %group @ " ";
	$tmpBotGroup[%group] = "";
}

function AI::DiscardBotGroup(%group)
{
//	dbecho($dbechoMode, "AI::DiscardBotGroup(" @ %group @ ")");

	for(%i = 0; (%a = GetWord($tmpBotGroup[%group], %i)) != -1; %i++)
		$botAttackMode[%a] = 1;

	$BotGroups = FixStuffString($BotGroups);
	$BotGroups = String::replace($BotGroups, " " @ %group @ " ", " ");
	$tmpBotGroup[%group] = "";
}

function AI::CountBotGroupMembers(%group)
{
//	dbecho($dbechoMode, "AI::CountBotGroupMembers(" @ %group @ ")");

	for(%i = 0; (%a = GetWord($tmpBotGroup[%group], %i)) != -1; %i++){}
	return %i;
}

function AI::IsInBotGroup(%aiId, %group)
{
//	dbecho($dbechoMode, "AI::IsInBotGroup(" @ %aiId @ ", " @ %group @ ")");

	for(%i = 0; (%a = GetWord($tmpBotGroup[%group], %i)) != -1; %i++)
	{
		if(%aiId == %a)
			return True;
	}
	return False;
}

function AI::BotGroupExists(%group)
{
//	dbecho($dbechoMode, "AI::BotGroupExists(" @ %group @ ")");

	for(%i = 0; (%a = GetWord($BotGroups, %i)) != -1; %i++)
	{
		if(%a == %group)
			return True;
	}
	return False;
}

function AI::RemoveBotFromBotGroup(%aiId, %group)
{
//	dbecho($dbechoMode, "AI::RemoveBotFromGroup(" @ %aiId @ ", " @ %group @ ")");

	$tmpBotGroup[%group] = String::Replace($tmpBotGroup[%group], %aiId @ " ", "");
	$botAttackMode[%aiId] = 1;
}

function AI::AddBotToBotGroup(%aiId, %group)
{
//	dbecho($dbechoMode, "AI::AddBotToBotGroup(" @ %aiId @ ", " @ %group @ ")");

	$tmpBotGroup[%group] = $tmpBotGroup[%group] @ %aiId @ " ";
	$botAttackMode[%aiId] = 4;
}

//------ remastered directives ------------------------------

function AI::newDirectiveFollow(%aiName, %idNum, %rad, %directive)
{
//	dbecho($dbechoMode, "AI::newDirectiveFollow(" @ %aiName @ ", " @ %idNum @ ", " @ %rad @ ", " @ %directive @ ")");

	%aiId = AI::getId(%aiName);
	AI::newDirectiveRemove(%aiName, %directive);

	$aidirectiveTable[%aiId, %directive] = "follow";
	AI::directiveFollow(%aiName, %idNum, %rad, %directive);
}

function AI::newDirectiveWaypoint(%aiName, %pos, %directive)
{
//	dbecho($dbechoMode, "AI::newDirectiveWaypoint(" @ %aiName @ ", " @ %pos @ ", " @ %directive @ ")");

	%aiId = AI::getId(%aiName);
	AI::newDirectiveRemove(%aiName, %directive);

	$aidirectiveTable[%aiId, %directive] = "waypoint";
	AI::directiveWaypoint(%aiName, %pos, %directive);
}

function AI::newDirectiveRemove(%aiName, %directive)
{
//	dbecho($dbechoMode, "AI::newDirectiveRemove(" @ %aiName @ ", " @ %directive @ ")");

	%aiId = AI::getId(%aiName);
	if($aidirectiveTable[%aiId, %directive] != "")
	{
		AI::directiveRemove(%aiName, %directive);
		$aidirectiveTable[%aiId, %directive] = "";
	}
}

//------- TownBot stuff ----------------------------------------

function InitTownBots()
{
//	dbecho($dbechoMode, "InitTownBots()");
return;
	$TownBotList = "";

	%group = nameToId("MissionGroup/TownBots");

	if(%group != -1)
	{
		%cnt = Group::objectCount(%group);
		for(%i = 0; %i <= %cnt - 1; %i++)
		{
			%object = Group::getObject(%group, %i);
			%name = Object::getName(%object);
			if(getObjectType(%object) == "SimGroup")
			{
				%marker = GatherBotInfo(%object);
			}

			%townbot = newObject("", "Item", $BotInfo[%name, RACE] @ "TownBot", 1, false);

			addToSet("MissionCleanup", %townbot);
			GameBase::setMapName(%townbot, $BotInfo[%name, NAME]);
			GameBase::setPosition(%townbot, GameBase::getPosition(%marker));
			GameBase::setRotation(%townbot, GameBase::getRotation(%marker));
			GameBase::setTeam(%townbot, $BotInfo[%name, TEAM]);
			GameBase::playSequence(%townbot, 0, "root");	//thanks Adger!!
			%townbot.name = %name;

			$TownBotList = $TownBotList @ %townbot @ " ";
		}
	}
}
function RotateTownBot(%id, %rot)
{
//	dbecho($dbechoMode, "RotateTownBot(" @ %id @ ", " @ %rot @ ")");

	%pos = GameBase::getPosition(%id);
	%name = %id.name;

	//delete the bot
	$TownBotList = String::replace($TownBotList, %id @ " ", "");
	deleteObject(%id);

	//re-create the bot
	%townbot = newObject("", "Item", $BotInfo[%name, RACE] @ "TownBot", 1, false);
	GameBase::setRotation(%townbot, %rot);

	addToSet("MissionCleanup", %townbot);
	GameBase::setMapName(%townbot, $BotInfo[%name, NAME]);
	GameBase::setPosition(%townbot, %pos);
	GameBase::setTeam(%townbot, $BotInfo[%name, TEAM]);
	GameBase::playSequence(%townbot, 0, "root");	//thanks Adger!!
	%townbot.name = %name;

	$TownBotList = $TownBotList @ %townbot @ " ";
}

function GatherBotInfo(%group)
{
//	dbecho($dbechoMode, "GatherBotInfo(" @ %group @ ")");

	%biggestn = 0;
	%aiName = Object::getName(%group);

	%count = Group::objectCount(%group);
	for(%i = 0; %i <= %count-1; %i++)
	{
		%object = Group::getObject(%group, %i);
		if(getObjectType(%object) == "SimGroup")
		{
			%system = Object::getName(%object);
			%type = GetWord(%system, 0);
			%info = String::getSubStr(%system, String::len(%type)+1, 9999);

			%type2 = clipTrailingNumbers(%type);
			%n = floor(String::getSubStr(%type, String::len(%type2), 9999));

			if(%type == "NAME")
				$BotInfo[%aiName, NAME] = %info;
			else if(%type == "LVL" || %type == "LEVEL")
				$BotInfo[%aiName, LVL] = %info;
			else if(%type == "RACE")
				$BotInfo[%aiName, RACE] = %info;
			else if(%type == "NEED")
				$BotInfo[%aiName, NEED] = %info;
			else if(%type == "TAKE")
				$BotInfo[%aiName, TAKE] = %info;
			else if(%type == "GIVE")
				$BotInfo[%aiName, GIVE] = %info;
			else if(%type == "SHOP")
				$BotInfo[%aiName, SHOP] = %info;
			else if(%type == "ITEMS")
				$BotInfo[%aiName, ITEMS] = %info;
			else if(%type == "CSAY")
				$BotInfo[%aiName, CSAY] = %info;
			else if(%type == "LSAY")
				$BotInfo[%aiName, LSAY] = %info;
			else if(%type == "LCK")
				$BotInfo[%aiName, LCK] = %info;
			else if(%type == "SIMGROUP")
				$BotInfo[%aiName, SIMGROUP] = %info;

			if(%type2 == "SAY")
				$BotInfo[%aiName, SAY, %n] = %info;
			else if(%type2 == "CUE")
				$BotInfo[%aiName, CUE, %n] = %info;
			else if(%type2 == "NSAY")
				$BotInfo[%aiName, NSAY, %n] = %info;
			else if(%type2 == "NCUE")
				$BotInfo[%aiName, NCUE, %n] = %info;

			if(%n > %biggestn)
				%biggestn = %n;
		}
		else
			%marker = %object;
	}
	$BotInfo[%aiName, SAY, %biggestn+1] = "";
	$BotInfo[%aiName, NSAY, %biggestn+1] = "";
	$BotInfo[%aiName, CUE, %biggestn+1] = "";
	$BotInfo[%aiName, NCUE, %biggestn+1] = "";

	//==============================================
	//The following is generally BotMaker-only code
	//==============================================
	if($BotInfo[%aiName, SIMGROUP] != "")
	{
		%g = nameToId("MissionGroup\\" @ $BotInfo[%aiName, SIMGROUP]);

		%count = Group::objectCount(%g);
		for(%i = 0; %i <= %count-1; %i++)
		{
			%object = Group::getObject(%g, %i);
			if(getObjectType(%object) == "SimGroup")
			{
				%system = Object::getName(%object);
				%type = GetWord(%system, 0);
				%info = String::getSubStr(%system, String::len(%type)+1, 9999);

				if(%type == "NAMES")
					$BotInfo[%aiName, NAMES] = %info;
				else if(%type == "DEFAULTS")
				{
					%class = GetWord(%info, 0);
					%stuff = String::getSubStr(%info, String::len(%class)+1, 9999);

					$BotInfo[%aiName, DEFAULTS, %class] = %stuff;
				}
			}
			else if(getObjectType(%object) == "Marker")
			{
				$BotInfo[%aiName, DESTSPAWN] = %object;
			}
		}
	}
	//==============================================

	return %marker;
}

function RPG::isAiControlled(%clientId)
{
//	dbecho($dbechoMode, "RPG::isAiControlled(" @ %clientId @ ")");

	if($BotInfoAiName[%clientId] != "" || $SpawnBotInfo[%clientId] != "")
		return True;
	else
		return False;
}
