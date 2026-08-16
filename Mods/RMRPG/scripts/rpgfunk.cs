//rewrote String::len from scratch, which is now approximately 6.5 times faster than the previous one i had from PSS.
function String::len(%string) {

	%chunk = 10;
	%length = 0;

	for(%i = 0; String::NEWgetSubStr(%string, %i, 1) != ""; %i += %chunk)
		%length += %chunk;
	%length -= %chunk;

	%checkstr = String::NEWgetSubStr(%string, %length, 99999);
	for(%k = 0; String::NEWgetSubStr(%checkstr, %k, 1) != ""; %k++)
		%length++;

	if(%length == -%chunk)
		%length = 0;

	return %length;
}

function String::replace(%string, %search, %replace)
{
	%loc = String::findSubStr(%string, %search);

	if(%loc != -1)
	{
		%ls = String::len(%search);

		%part1 = String::NEWgetSubStr(%string, 0, %loc);
		%part2 = String::NEWgetSubStr(%string, %loc + %ls, 99999);

		%string = %part1 @ %replace @ %part2;
	}

	return %string;
}

function String::ofindSubStr(%s, %f, %o)
{
	dbecho($dbechoMode, "String::ofindSubStr(" @ %s @ ", " @ %f @ ", " @ %o @ ")");

	%ns = String::NEWgetSubStr(%s, %o, 99999);
	return String::findSubStr(%ns, %f);
}
//--------------
function viewGroupList(%name) {

	%Client = getClientByName(%name);
	centerPrint(%Client, $grouplist["\""@%name@"\""], 8);
}

//function updateSpawnBuyList(%Client) {
//echo("updateSpawnBuyList called");
//}

function isInGroupList(%id1, %id2) {

	//check if %sname (includes delimiter) is in %name's grouplist
	if(String::findSubStr($grouplist[Client::getName(%id1)], Client::getName(%id2)@",") != -1)
		return True;
	else
		return False;
}

function StartRecord(%Client) {

	//clear variables
	$recording[%Client] = "";
	for(%t=1; $rec::type[%t] != ""; %t++)
		$rec::type[%t] = "";
	$recCount[%Client]=0;

	$recording[%Client] = 1;
}

function StopRecord(%Client, %f) {

	//%f = String::replace(%f, "\", "\\");
	File::delete(%f);
	export("rec::*", "temp\\"@%f, false);

	//clear variables
	$recording[%Client] = "";
	for(%t=1; $rec::type[%t] != ""; %t++)
		$rec::type[%t] = "";
	$recCount[%Client]=0;
}

function AddObjectToRec(%Client, %a, %pos, %rot) {

	//%pos: deploy position
	//%rot: player's rotation

	$recCount[%Client]++;

	if($recCount[%Client] == 1)
	{
		//this is the first object placed, so use it as a reference object
		$recRefpos[%Client] = %pos;
		$recRefrot[%Client] = %rot;
	}
	$rec::type[$recCount[%Client]] = %a;

	$rec::pos[$recCount[%Client]] = Vector::sub(%pos, $recRefpos[%Client]);

	$rec::rot[$recCount[%Client]] = %rot;
}

function DeployBase(%Client, %f, %refPos, %refRot) {

	//%refPos: deploy position
	//%refRot: player's rotation

	for(%t=1; $rec::type[%t] != ""; %t++)
		$rec::type[%t] = "";

	$ConsoleWorld::DefaultSearchPath = $ConsoleWorld::DefaultSearchPath;	//thanks Presto
	exec(%f);

	$baseIndex++;
	for(%i = 1; $rec::type[%i] != ""; %i++)
	{
		if(%i == 1)
		{
			%newpos = %refPos;
			%newrot = $rec::rot[%i];
		}
		else
		{
			%a = Vector::add(%refPos, $rec::pos[%i]);

			%newpos = %a;
			%newrot = $rec::rot[%i];
		}

		if($rec::type[%i] == 1)
		{
			%a = DepPlatSmallHorz;
		}
		else if($rec::type[%i] == 2)
		{
			%a = DepPlatMediumHorz;
		}
		else if($rec::type[%i] == 3)
		{
			%a = DepPlatLargeHorz;
		}
		else if($rec::type[%i] == 4)
		{
			%a = DepPlatSmallVert;
			%newrot = "0 1.5708 "@GetWord(%newrot, 2) + "1.5708";
			%newpos = GetWord(%newpos, 0)@" "@GetWord(%newpos, 1)@" "@(GetWord(%newpos, 2) + 2);
		}
		else if($rec::type[%i] == 5)
		{
			%a = DepPlatMediumVert;
			%newrot = "0 1.5708 "@GetWord(%newrot, 2) + "1.5708";
			%newpos = GetWord(%newpos, 0)@" "@GetWord(%newpos, 1)@" "@(GetWord(%newpos, 2) + 3);
		}
		else if($rec::type[%i] == 6)
		{
			%a = DepPlatLargeVert;
			%newrot = "0 1.5708 "@GetWord(%newrot, 2) + "1.5708";
			%newpos = GetWord(%newpos, 0)@" "@GetWord(%newpos, 1)@" "@(GetWord(%newpos, 2) + 4.5);
		}
		else if($rec::type[%i] == 7)
		{
			%a = StaticDoorForceField;
		}

		%depbase = newObject("","StaticShape",%a,true);
		addToSet("MissionCleanup", %depbase);
		GameBase::setTeam(%depbase, GameBase::getTeam(%Client));
		GameBase::setPosition(%depbase, %newpos);
		GameBase::setRotation(%depbase, %newrot);
		GameBase::startFadeIn(%depbase);

		$owner[%depbase] = Client::getName(%Client);
	}

	Client::sendMessage(%Client,0,"Base deployed");
}

function DoCamp(%Client, %savecharTry) {

	if(%savecharTry)
	{
		%vel = Item::getVelocity(%Client);
		if(getWord(%vel, 2) > -500)
		{
			if(!IsDead(%Client))
				$campPos[%Client] = GameBase::getPosition(%Client);
			return True;
		}
	}
	else
	{
		if(GameBase::isAtRest(%Client))
		{
			$campPos[%Client] = GameBase::getPosition(%Client);
			return True;
		}
	}
	return False;
}

function SaveCharacter(%Client, %docamp) {

	//first pass check
	if(%Client.isInvalid || !$HasLoadedAndSpawned[%Client]) //|| IsInRoster(%Client) || IsInArenaDueler(%Client))
		return FALSE;

	//second pass check, will cause 4 line flood if the client is invalid
	//only do this as a "last resort" test.  if the player is detected to be dead, then there shouldn't be a problem
	if(!IsDead(%Client))
	{
		Player::incItemCount(%Client, Tool);
		%x = Player::getItemCount(%Client, Tool);
		Player::decItemCount(%Client, Tool);
		%y = Player::getItemCount(%Client, Tool);
		if(%x == %y)
			return FALSE;
	}

	%name = Client::getName(%Client);

	if(%docamp)
		DoCamp(%Client, True);

	ClearFunkVar(%name);

	if(%name == "" || %name == False)
		return false;

	//the first identifier in the array is the player's name.
	//this is needed because we are using a global array ($SaveData), so if another player
	//attempts to save at the same time, then there won't be $SaveData's being overwritten

	//the second identifier in the array is either 0, 1, or 2
	//0: regular player variable
	//1: weapon/item
	//2: quest counters

	//the third identifier is simply for identifying what we're saving.

	$SaveData["[\""@%name@"\", 0, 1]"] = $RACE[%Client];
	$SaveData["[\""@%name@"\", 0, \"2Exp1\"]"] = $EXP1[%Client];
	$SaveData["[\""@%name@"\", 0, \"2Exp2\"]"] = $EXP2[%Client];
	$SaveData["[\""@%name@"\", 0, \"2Exp3\"]"] = $EXP3[%Client];
	$SaveData["[\""@%name@"\", 0, 3]"] = $campPos[%Client];
	$SaveData["[\""@%name@"\", 0, 4]"] = $COINS[%Client];
	$SaveData["[\""@%name@"\", 0, 5]"] = $ClientData[%Client, APPoints]; //$ORE[%Client];

	$SaveData["[\""@%name@"\", 0, 6]"] = $BANK[%Client];
	$SaveData["[\""@%name@"\", 0, 8]"] = $grouplist[%name];
	$SaveData["[\""@%name@"\", 0, 9]"] = $defaultTalk[%Client];
	$SaveData["[\""@%name@"\", 0, 10]"] = $password[%Client];

	$SaveData["[\""@%name@"\", 0, 11]"] = $stealskill[%Client];
	$SaveData["[\""@%name@"\", 0, 12]"] = $ClientData[%Client, Skill_BlackSmith]; //$inArena[%Client];
	$SaveData["[\""@%name@"\", 0, 13]"] = $PlayerInfo[%Client];
	$SaveData["[\""@%name@"\", 0, 14]"] = $playerCurrentSpell[%Client];
	$SaveData["[\""@%name@"\", 0, 15]"] = $ClientData[%Client, AP_Check]; //$spellList[%name];

//	$SaveData["[\""@%name@"\", 0, 16]"] = $BankStorage[%Client];
	FixDataString::Save(%Client, %name);	 // Does BankStorage and ItemList

	$SaveData["[\""@%name@"\", 0, 17]"] = $AP[%Client, 1];
	$SaveData["[\""@%name@"\", 0, 18]"] = $AP[%Client, 2];
	$SaveData["[\""@%name@"\", 0, 19]"] = $AP[%Client, 3];
	$SaveData["[\""@%name@"\", 0, 20]"] = $AP[%Client, 4];

	$SaveData["[\""@%name@"\", 0, 21]"] = $AP[%Client, 5];
	$SaveData["[\""@%name@"\", 0, 22]"] = $LCK[%Client];
	$SaveData["[\""@%name@"\", 0, 26]"] = $GROUP[%Client];
	$SaveData["[\""@%name@"\", 0, 27]"] = $CLASS[%Client];
	$SaveData["[\""@%name@"\", 0, 28]"] = $MaxHP[%Client];
	$SaveData["[\""@%name@"\", 0, 29]"] = $PKflag[%Client];

	$SaveData["[\""@%name@"\", 0, 31]"] = $BurnedClientTagId[%Client];
	$SaveData["[\""@%name@"\", 0, 32]"] = $isBlessed[%Client];
//	echo(%Client@" Client's privilege level is "@$PL[%Client]);
	$SaveData["[\""@%name@"\", 0, 33]"] = $PL[%Client];

	$SaveData["[\""@%name@"\", 0, 34, 0]"] = $SkillCounter[%Client, $SlashingDamageType];
	$SaveData["[\""@%name@"\", 0, 35, 0]"] = $SkillCounter[%Client, $BludgeoningDamageType];
	$SaveData["[\""@%name@"\", 0, 36, 0]"] = $SkillCounter[%Client, $PiercingDamageType];
	$SaveData["[\""@%name@"\", 0, 37, 0]"] = $SkillCounter[%Client, $ProjectileDamageType];

	$SaveData["[\""@%name@"\", 0, 34, 1]"] = $AttackBonus[$SlashingDamageType, %Client];
	$SaveData["[\""@%name@"\", 0, 35, 1]"] = $AttackBonus[$BludgeoningDamageType, %Client];
	$SaveData["[\""@%name@"\", 0, 36, 1]"] = $AttackBonus[$PiercingDamageType, %Client];
	$SaveData["[\""@%name@"\", 0, 37, 1]"] = $AttackBonus[$ProjectileDamageType, %Client];

	$SaveData["[\""@%name@"\", 0, 38]"] = $AP[%Client, 6];

	$SaveData["[\""@%name@"\", 0, 39]"] = $ClientData[%Client, "isMimic"];

	//
	if(IsDead(%Client)) {
		$SaveData["[\""@%name@"\", 0, 40]"] = $MaxHP[%Client];
		$SaveData["[\""@%name@"\", 0, 41]"] = $MaxMANA[%Client];
		$SaveData["[\""@%name@"\", 0, 42]"] = $MaxSTA[%Client];
	}
	else {
		$SaveData["[\""@%name@"\", 0, 40]"] = getHP(%Client);
		$SaveData["[\""@%name@"\", 0, 41]"] = getMANA(%Client);
		$SaveData["[\""@%name@"\", 0, 42]"] = getSTA(%Client);
	}

	$SaveData["[\""@%name@"\", 0, 43]"] = $Quiver[%Client];

	//IP dump, for server admin look-up purposes
	$SaveData["[\""@%name@"\", 0, 666]"] = Client::getTransportAddress(%Client);


//	$SaveData["[\""@%name@"\", 0, 45]"] = $ClientData[%Client, "ItemList"];
	$SaveData["[\""@%name@"\", 0, 46]"] = $ClientData[%Client, "EquipList"];
	$SaveData["[\""@%name@"\", 0, 47]"] = $ClientData[%Client, "QuestList"];
	$SaveData["[\""@%name@"\", 30, 1]"] = $showexp[%Client];

	%cnt = 0;
	%list = $TownBotList;
	for(%i = 0; (%id = GetWord(%list, %i)) != -1; %i++) {

		%aiName = $TownBot[%id, NAME]; //$BotInfoAiName[%id];

		if($QuestCounter[%name, %aiName] != "") {
			%cnt++;
			$SaveData["[\""@%name@"\", 2, "@%cnt@"]"] = %aiName;
			$SaveData["[\""@%name@"\", 3, "@%cnt@"]"] = $QuestCounter[%name, %aiName];
		}
	}
	for(%i = 0; $QuestData::[%i] != ""; %i++) {
		if($ClientData[%Client, $QuestData::[%i]] != "")
			$SaveData["[\""@%name@"\", 4, "@%i@"]"] = $ClientData[%Client, $QuestData::[%i]];
	}


	//==================================
	//Save Chocobo(s) if there are any
	%cnt = 0;
	//if($Chocobo[%Client] >= 1) {
		$SaveData["[\""@%name@"\", 5, 1, 10, 10]"] = "-Chocobo save Start-";
		$SaveData["[\""@%name@"\", 5, 1, 10, 11]"] = $Chocobo[%Client];
		for(%i = 1; $Chocobo[%Client, %i] != ""; %i++) {
			%cnt++;
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 15]"] = $ChocoboName[%Client, %cnt];
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 16]"] = $ChocoboColor[%Client, %cnt];
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 17]"] = $ChocoboSex[%Client, %cnt];
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 18]"] = $ChocoboYAge[%Client, %cnt];
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 19]"] = $ChocoboDAge[%Client, %cnt];
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 20]"] = $ChocoboTempAge[%Client, %cnt];
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 21]"] = $Chocobo[%Client, %cnt];
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 22]"] = $ChocoboTakeCare[%Client, %cnt];

			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 25]"] = $ChocoboSTR[%Client, %cnt];
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 26]"] = $ChocoboDEX[%Client, %cnt];
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 27]"] = $ChocoboCON[%Client, %cnt];
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 28]"] = $ChocoboINT[%Client, %cnt];
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 29]"] = $ChocoboWIS[%Client, %cnt];
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 30]"] = $ChocoboEXP[%Client, %cnt];
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 31]"] = $ChocoboHealth[%Client, %cnt];
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 32]"] = $ChocoboHungry[%Client, %cnt];
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 33]"] = $ChocoboWorth[%Client, %cnt];
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 34]"] = $ChocoboMeats[%Client, %cnt];
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 35]"] = $ChocoboFruits[%Client, %cnt];
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 36]"] = $ChocoboVits[%Client, %cnt];
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 37]"] = $ChocoboSeeds[%Client, %cnt];
			$SaveData["[\""@%name@"\", 5, "@%cnt@", 10, 38]"] = $ChocoboCandies[%Client, %cnt];
		}
		$SaveData["[\""@%name@"\", 5, "@$MaxChocobo@", 10, 50]"] = "-Chocobo save End-";
	//}

	$SaveData["[\""@%name@"\", 6, 1]"] = $KeyList[%Client];

	//============= RUNES =========
	for(%i = 0; $RuneColorList[%i] != ""; %i++) {
		if($ClientData[%Client, "Rune", "State", $RuneColorList[%i]] > 0) {
			$SaveData["[\""@%name@"\", 6, 2, "@%i@"]"] = $ClientData[%Client, "Rune", "State", $RuneColorList[%i]];
			$SaveData["[\""@%name@"\", 6, 3, "@%i@"]"] = $ClientData[%Client, "Rune", "POS", $RuneColorList[%i]];
		}
	}


	//=========== BONUS STATE =======
	for(%i = 1; %i <= $maxBonusStates; %i++)
	{
		$SaveData["[\""@%name@"\", 10, "@%i@"]"] = $BonusState[%Client, %i];
		$SaveData["[\""@%name@"\", 11, "@%i@"]"] = $BonusStateCnt[%Client, %i];
	}

	//============= STATUS ============
	for(%i  = 0; $StatusLookUpList[%i] != ""; %i++) {
		if($ClientData[%Client, $StatusLookUpList[%i]] > 0)
			$SaveData["[\""@%name@"\", 15, "@%i@"]"] = $ClientData[%Client, $StatusLookUpList[%i]];
	}

	ClearFileName(%name);

	if(%Client.adminlevel <= 0)
		%Client.adminlevel = 0;

	export("SaveData[\""@%name@"\",*", "temp\\[RM]"@%name@"["@%Client.adminlevel@"].cs", false);
	ClearFunkVar(%name);
	echo("SAVED: "@%name@" [CLIENT "@%Client@"] [ADMIN "@%Client.adminlevel@" | PRIV "@$PL[%Client]@"]");



	return "True";
}

function LoadCharacter(%Client) {

	ClearVariables(%Client);
	remoteEval(%Client, "ClearItemLists");
	%Client.bulk = 1;

	%name = Client::getName(%Client);
	%filename = GetFileName(%name);

	$ConsoleWorld::DefaultSearchPath = $ConsoleWorld::DefaultSearchPath;	//thanks Presto

	if(%filename != false)
	{
		//load character
		echo(">> Loading character "@%name@" [CLIENT "@%Client@" | FILE \"temp\\"@%filename@"\"]");

		for(%retry = 1; %retry <= 10; %retry++)		//This might not be necessary, but it's to ensure that the
		{													//exec doesn't get flakey when there's lag.
			exec(%filename);
			if($SaveData[%name, 0, 1] != "")
				break;
		}

		$RACE[%Client] = $SaveData[%name, 0, 1];
		$EXP1[%Client] = $SaveData[%name, 0, "2Exp1"];
		$EXP2[%Client] = $SaveData[%name, 0, "2Exp2"];
		$EXP3[%Client] = $SaveData[%name, 0, "2Exp3"];
		$campPos[%Client] = $SaveData[%name, 0, 3];
		$COINS[%Client] = $SaveData[%name, 0, 4];
		$ClientData[%Client, APPoints] = $SaveData[%name, 0, 5]; //$ORE[%Client]

		$BANK[%Client] = $SaveData[%name, 0, 6];
		$grouplist[%name] = $SaveData[%name, 0, 8];
		$defaultTalk[%Client] = $SaveData[%name, 0, 9];
		$password[%Client] = $SaveData[%name, 0, 10];

		$stealskill[%Client] = $SaveData[%name, 0, 11];
		$ClientData[%Client, Skill_BlackSmith] = $SaveData[%name, 0, 12];	//$inArena[%Client]
		$PlayerInfo[%Client] = $SaveData[%name, 0, 13];
		$playerCurrentSpell[%Client] = $SaveData[%name, 0, 14];
		$ClientData[%Client, AP_Check] = $SaveData[%name, 0, 15]; //$spawnStuff[%Client]

//		$BankStorage[%Client] = $SaveData[%name, 0, 16];  see line 490 (or near it)

		$AP[%Client, 1] = $SaveData[%name, 0, 17];
		$AP[%Client, 2] = $SaveData[%name, 0, 18];
		$AP[%Client, 3] = $SaveData[%name, 0, 19];
		$AP[%Client, 4] = $SaveData[%name, 0, 20];
		$AP[%Client, 5] = $SaveData[%name, 0, 21];
		$LCK[%Client] = $SaveData[%name, 0, 22];
		$GROUP[%Client] = $SaveData[%name, 0, 26];
		$CLASS[%Client] = $SaveData[%name, 0, 27];
		$MaxHP[%Client] = $SaveData[%name, 0, 28];
		$PKflag[%Client] = $SaveData[%name, 0, 29];

		$BurnedClientTagId[%Client] = $SaveData[%name, 0, 31];
		$isBlessed[%Client] = $SaveData[%name, 0, 32];
		$PL[%Client] =  $SaveData[%name, 0, 33];

		$showexp[%Client] = $SaveData[%name, 30, 1];

		$SkillCounter[%Client, $SlashingDamageType] = $SaveData[%name, 0, 34, 0];
		$SkillCounter[%Client, $BludgeoningDamageType] = $SaveData[%name, 0, 35, 0];
		$SkillCounter[%Client, $PiercingDamageType] = $SaveData[%name, 0, 36, 0];
		$SkillCounter[%Client, $ProjectileDamageType] = $SaveData[%name, 0, 37, 0];

		$AttackBonus[$SlashingDamageType, %Client] =  $SaveData[%name, 0, 34, 1];
		$AttackBonus[$BludgeoningDamageType, %Client] =  $SaveData[%name, 0, 35, 1];
		$AttackBonus[$PiercingDamageType, %Client] =  $SaveData[%name, 0, 36, 1];
		$AttackBonus[$ProjectileDamageType, %Client] =  $SaveData[%name, 0, 37, 1];

		$AP[%Client, 6] = $SaveData[%name, 0, 38];

		$ClientData[%Client, "isMimic"] = $SaveData[%name, 0, 39];

		$Client::tmp[%Client, hp] = $SaveData[%name, 0, 40];
		$Client::tmp[%Client, mp] = $SaveData[%name, 0, 41];
		$Client::tmp[%Client, sta] = $SaveData[%name, 0, 42];

		//
		$Quiver[%Client] = $SaveData[%name, 0, 43];

		$ClientData[%Client, "ItemList"] = " "; $ClientData[%Client, "EquipList"] = " "; $ClientData[%Client, "QuestList"] = " ";

//		$Client::tmp[%Client, "ItemList"] = $SaveData[%name, 0, 45];

		FixDataString::Load(%Client, %name); //this loads ItemList and BankStorage

		$Client::tmp[%Client, "EquipList"] = $SaveData[%name, 0, 46];
		$Client::tmp[%Client, "QuestList"] = $SaveData[%name, 0, 47];

		$Weight[%Client] = "0";

		$ClientData[%Client, JustJoined] = true;

		for(%i = 1; $SaveData[%name, 3, %i] != ""; %i++)
		{
			$QuestCounter[%name, $SaveData[%name, 2, %i]] = $SaveData[%name, 3, %i];
		}
		for(%i = 0; $QuestData::[%i] != ""; %i++) {
			if($SaveData[%name, 4, %i] != "")
				$ClientData[%Client, $QuestData::[%i]] = $SaveData[%name, 4, %i];
		}
		//Load Chocobo(s) if there are any
		Chocobo::GetSaveSlot(%Client);
		//if($Chocobo[%Client] >= 1) {

			$Chocobo[%Client] = $SaveData[%name, 5, 1, 10, 11];
			//for(%i = 1; $Chocobo[%Client, %i] != ""; %i++) {
			for(%i = 1; %i <= $MaxChocobo; %i++) {
				$ChocoboName[%Client, %i] = $SaveData[%name, 5, %i, 10, 15];
				$ChocoboColor[%Client, %i] = $SaveData[%name, 5, %i, 10, 16];
				$ChocoboSex[%Client, %i] = $SaveData[%name, 5, %i, 10, 17];
				$ChocoboYAge[%Client, %i] = $SaveData[%name, 5, %i, 10, 18];
				$ChocoboDAge[%Client, %i] = $SaveData[%name, 5, %i, 10, 19];
				$ChocoboTempAge[%Client, %i] = $SaveData[%name, 5, %i, 10, 20];
				$Chocobo[%Client, %i] = $SaveData[%name, 5, %i, 10, 21];
				$ChocoboTakeCare[%Client, %i] = $SaveData[%name, 5, %i, 10, 22];

				$ChocoboSTR[%Client, %i] = $SaveData[%name, 5, %i, 10, 25];
				$ChocoboDEX[%Client, %i] = $SaveData[%name, 5, %i, 10, 26];
				$ChocoboCON[%Client, %i] = $SaveData[%name, 5, %i, 10, 27];
				$ChocoboINT[%Client, %i] = $SaveData[%name, 5, %i, 10, 28];
				$ChocoboWIS[%Client, %i] = $SaveData[%name, 5, %i, 10, 29];
				$ChocoboEXP[%Client, %i] = $SaveData[%name, 5, %i, 10, 30];
				$ChocoboHealth[%Client, %i] = $SaveData[%name, 5, %i, 10, 31];
				$ChocoboHungry[%Client, %i] = $SaveData[%name, 5, %i, 10, 32];
				$ChocoboWorth[%Client, %i] = $SaveData[%name, 5, %i, 10, 33];
				$ChocoboMeats[%Client, %i] = $SaveData[%name, 5, %i, 10, 34];
				$ChocoboFruits[%Client, %i] = $SaveData[%name, 5, %i, 10, 35];
				$ChocoboVits[%Client, %i] = $SaveData[%name, 5, %i, 10, 36];
				$ChocoboSeeds[%Client, %i] = $SaveData[%name, 5, %i, 10, 37];
				$ChocoboCandies[%Client, %i] = $SaveData[%name, 5, %i, 10, 38];
			}
		//}

		//============== RUNES ==============
		for(%i = 0; $RuneColorList[%i] != ""; %i++) {
			if($SaveData[%name, 6, 2, %i] != "") {
				$ClientData[%Client, "Rune", "State", $RuneColorList[%i]] = $SaveData[%name, 6, 2, %i];
				$ClientData[%Client, "Rune", "POS", $RuneColorList[%i]] = $SaveData[%name, 6, 3, %i];
			}
		}

		//============== BONUS =============
		for(%i = 1; %i <= $maxBonusStates; %i++)
		{
			$BonusState[%Client, %i] = $SaveData[%name, 10, %i];
			$BonusStateCnt[%Client, %i] = $SaveData[%name, 11, %i];
		}

		//============== STATUS =============
		for(%i  = 0; $StatusLookUpList[%i] != ""; %i++) {
			if($SaveData[%name, 15, %i] != "") {
				$ClientData[%Client, $StatusLookUpList[%i]] = $SaveData[%name, 15, %i];
				UpdateStatusList(%Client, $StatusLookUpList[%i], "add");
			}
		}

		$KeyList[%Client] = $SaveData[%name, 6, 1];
		$templvl[%Client] = getFinalLVL(%Client);
		%Client.adminlevel = 0;

		$ClientData[%Client, UsingWeapon] = "-1";

		echo(">> Load complete.");
	}
	else {
		echo(">> NEW PLAYER: "@%name@" ["@%Client@"]");
		$RACE[%Client] = Client::getGender(%Client)@"Human";
		$Exp1[%Client] = 0;
		$Exp2[%Client] = 0;
		$Exp3[%Client] = 0;
		$campPos[%Client] = "";
		$BANK[%Client] = $initbankcoins;
		//$ORE[%Client] = 0;
		$grouplist[%name] = "";
		$spellList[%name] = "";
		$defaultTalk[%Client] = "#say";
		$password[%Client] = $Client::info[%Client, 5];
		$stealskill[%Client] = $initstealskill;
		$LCK[%Client] = $initLCK;
		$PlayerInfo[%Client] = "";
		$ignoreGlobal[%Client] = "";
		$LCKconsequence[%Client] = "death";

		$showexp[%Client] = false;	// EXP BAR

		$ClientData[%Client, AP_Check] = 10;

		$PL[%Client] = 0;
		$AP[%Client, 6] = 0;

		$AttackBonus[$SkillSlashing, %Client] = 0;
		$AttackBonus[$SkillBludgeoning, %Client] = 0;
		$AttackBonus[$SkillPiercing, %Client] = 0;
		$AttackBonus[$SkillArchery, %Client] = 0;

		$Quiver[%Client] = "0 ";

		$MaxCOINS[%Client] = "99";
		$Weight[%Client] = "0";

		//Do not add items here
		$ClientData[%Client, "ItemList"] = " "; $ClientData[%Client, "EquipList"] = " "; $ClientData[%Client, "QuestList"] = " ";


		//$var removed after client spawns and has a GiveThisStuff called.
		$ClientData[%Client, "NEWItemList"] = "";
		$ClientData[%Client, "NEWEquipList"] = "";
		$ClientData[%Client, "NEWQuestList"] = "newcharpass 1 Tiny_Bag 1 ";

		$ClientData[%Client, UsingWeapon] = "-1";

		$templvl[%Client] = 1;
		%Client.adminlevel = 0;
		%Client.choosingGroup = True;
	}

	$isZombie[%Client] = "";
	ClearFunkVar(%name);
	%Client.bulk = 1;
}

function ResetPlayer(%Client) {

	%name = Client::getName(%Client);
	ClearFileName(%name);
	LoadCharacter(%Client);
	StartStatSelection(%Client);
}

function SaveWorld() {

	echo(">> Saving world '[SaveWorld_"@$missionName@"]'...");
	messageAll(2, "[SaveWorld]");
	%i = 0;
	%ii = 0;
	%othercnt = 0;
	deletevariables("$world::*");
	if($saveworldsearch == "")
		$saveworldsearch = 200;
	%eomID = nametoid("MissionGroup\\EndOfMap");
	if(%eomID < 1){
		echo("Saveworld failed, EndOfMap not found");
		return;
	}
	while(%othercnt < $saveworldsearch)//while(%othercnt < 15)
	{
		%i++;
		%ID = %eomID + %i; //%ID = 8361 + %i;
		%obj = GameBase::getDataName(%ID);
	//	if(String::findSubStr($WorldSaveList, "|"@%obj@"|") != -1)
		if(%obj == LootBag) {
			%ii++;
			//echo("Saving object #"@%ii@" : "@%obj);
			$world::object[%ii] = %obj;
			$world::owner[%ii] = $owner[%ID];
			$world::pos[%ii] = GameBase::getPosition(%ID);
			$world::rot[%ii] = GameBase::getRotation(%ID);
			$world::team[%ii] = GameBase::getTeam(%ID);
			$world::special[%ii] = "";
			//modify special depending on the item
			if(%obj == "LootBag") // datablock name is "LootBag" (capital B); Tribes == is case-sensitive, so "Lootbag" never matched -> loot contents were dropped
			{
				%loot = $loot[%ID];
				%w1 = getWord(%loot, 0);
				if(%w1 != "*")
					%loot = "* "@String::NEWgetSubStr(%loot, String::len(%w1), 9999);
				$world::special[%ii] = %loot;
			}
		}

		if(%obj == "")
			%othercnt++;
		else
			%othercnt = 0;

	}
	File::delete("temp\\[SaveWorld_"@$missionName@"].cs");

	export("world::*", "temp\\[SaveWorld_"@$missionName@"].cs", false);
	echo(">> Save complete.");
	//messageAll(2, "[SaveWorld] complete.");
}
function LoadWorld() {

	%filename = "[SaveWorld_"@$missionName@"].cs";

	if(isFile("temp\\"@%filename))
	{
		//load world
		echo(">> Loading world '"@$missionName@"'...");
		messageAll(2, "LoadWorld in progress...");

		$ConsoleWorld::DefaultSearchPath = $ConsoleWorld::DefaultSearchPath;	//thanks Presto
		exec(%filename);

		for(%i = 1; $world::object[%i] != ""; %i++)
		{
			echo("Loading object #"@%i@" : "@$world::object[%i]);
			if($world::object[%i] == "DepPlatSmallHorz" || $world::object[%i] == "DepPlatMediumHorz" || $world::object[%i] == "DepPlatSmallVert" || $world::object[%i] == "DepPlatMediumVert")
			{
				DeployPlatform($world::owner[%i], $world::team[%i], $world::pos[%i], $world::rot[%i], $world::object[%i]);
			}
			else if($world::object[%i] == "StaticDoorForceField")
			{
				DeployForceField($world::owner[%i], $world::team[%i], $world::pos[%i], $world::rot[%i]);
			}
			else if($world::object[%i] == "DeployableTree")
			{
				DeployTree($world::owner[%i], $world::team[%i], $world::pos[%i], $world::rot[%i]);
			}
			else if($world::object[%i] == "LootBag") // was "Lootbag"; SaveWorld stores the datablock name "LootBag", and Tribes == is case-sensitive, so loot bags never rehydrated
			{
				DeployLootbag($world::owner[%i], $world::pos[%i], $world::rot[%i], $world::special[%i]);
			}
		}
		echo(">> Load complete.");
		messageAll(2, "LoadWorld complete.");
	}
	else
	{
		// Not an error, just no persisted world yet; the file is written on the first
		// SaveWorld once dropped loot or a deployable exists to save.
		echo(">> No saved world yet for this map (created on first save).");
	}
}
function DeployPlatform(%name, %team, %pos, %rot, %plattype) {

	%platform = newObject("", "StaticShape", %plattype, true);

	$owner[%platform] = %name;

	if($recording[getClientByName(%name)] == 1)
		AddObjectToRec(getClientByName(%name), 1, %pos, %rot);

	addToSet("MissionCleanup", %platform);
	GameBase::setTeam(%platform, %team);
	GameBase::setPosition(%platform, %pos);
	GameBase::setRotation(%platform, %rot);
	Gamebase::setMapName(%platform, %plattype);
	GameBase::startFadeIn(%platform);
	playSound(SoundPickupBackpack, %pos);
	playSound(ForceFieldOpen, %pos);
}

function DeployLootbag(%name, %pos, %rot, %special) {

	if(%special != "") {
		%lootbag = newObject("", "Item", "Lootbag", 1, false);

		$loot[%lootbag] = %special;
		$owner[%lootbag] = %name;

		schedule("RMItem::Pop("@%lootbag@");", $LootbagPopTime, %lootbag);

		addToSet("MissionCleanup", %lootbag);

		GameBase::setPosition(%lootbag, %pos);
		GameBase::setRotation(%lootbag, %rot);
		GameBase::setMapName(%lootbag, "Backpack");
		GameBase::startFadeIn(%lootbag);
	}
}

function WhatIs(%Client, %item, %opt) {

	//--------- GATHER INFO ------------------
	%desc = $ItemData[%item, Name];

	%t = $ItemData[%item, type];
	%w = $ItemData[%item, Weight];
	%c = $Itemcost[%item];
	%bonus = WhatSpecialVars(%item);
	%s = $ItemData[%item, ToUseSkill];

	if($ItemData[%item, Delay] > 0)
		%sd = $ItemData[%item, Delay];
	else
		%sd = "";

	if($ItemData[%item, Range] > 0)
		%sra = $ItemData[%item, Range];
	else
		%sra = "";

	if($LocationDesc[%t] != "")
		%loc = " - Location: " @ $LocationDesc[%t];
	else
		%loc = "";

	if($ItemData[%item, info] != "NULL")
		%nfo = $ItemData[%item, info];
	else
		%nfo = "There is no further information available.";

	%si = $Spell::index[%item];
	if((%si != "" && %desc == "") || (%si != "" && %opt))
	{
		%loc = ""; %w = ""; %c = ""; %t = ""; %bonus = "";
		%item = %si;
		%desc = $Spell::name[%si];
		%nfo = $Spell::description[%si];
		%atkinfo = $Spell::damageValue[%si];
		%sd = $Spell::delay[%si];
		%sr = $Spell::recoveryTime[%si];
		%sm = $Spell::manaCost[%si];

		//%ml = "LVL: "@$Spell::minLevel[%si];
		//%cr = $Spell::classRestrictions[%si];
		%s = $Spell::ToUseSkill[%si];

		%type = $Spell::Type[%si];
	}

	//--------- BUILD MSG --------------------
	%msg = "";
	if(%desc == "") {
		remoteEval(%Client, "SetPrint::Title", %item);
		%msg = "<f2> Error:\n<f0> not a valid item. \n              \nThere is no further information available.      ";
		remoteEval(%Client, "SetPrint::Info", %msg);
		return %msg;
	}

	%msg = %Rmsg = %desc @ %loc;
	remoteEval(%Client, "SetPrint::Title", %msg);

	%msg = "";

	%size = 0;

	if(%bonus != "") {
		%msg = %msg @ "\nBonuses: "@%bonus;
		%size++;
	}
	if(%atkinfo != "") {
		%msg = %msg@"\nDamage Value: "@%atkinfo;
		%size++;
	}
	if(%s != "") {
		%msg = %msg@"\nNeeded Skills: "@%s;
		%size++;
	}
	if(%cr != "") {
		%msg = %msg@"\nClass: "@%cr;
		%size++;
	}
	if($GroupRestrictions[%item] != "") {
		%msg = %msg @ "\nRestrictions: "@$GroupRestrictions[%item];
		%size++;
	}
	if(%w != "") {
		%msg = %msg @ "\nWeight: "@%w;
		%size++;
	}
	if(%c != "") {
		%msg = %msg @ "\nPrice: $"@FixM(%c);
		%size++;
	}
	if(%sd != "") {
		%msg = %msg @ "\nDelay: "@%sd@" sec";
		%size++;
	}
	if(%sra != "") {
		%msg = %msg@"\nRange: "@%sra;
		%size++;
	}
	if(%sr != "") {
		%msg = %msg @ "\nRecovery: "@%sr@" sec";
		%size++;
	}
	if(%sm != "") {
		%msg = %msg @ "\nMana: "@%sm;
		%size++;
	}
	if(%type != "") {
		%msg = %msg@"\nType: "@%type;
		%size++;
	}

	%msg = %msg @ "\n\n<f0>"@%nfo@getRuneInfo(%item);
	%size += 2;

	remoteEval(%Client, "SetPrint::Size", %size);
	remoteEval(%Client, "SetPrint::Info", String::NEWgetSubStr(%msg, 1, 99999));

	return %Rmsg@%msg;
}

function getRuneInfo(%rune) {
	//TODO
	return;
}

function NEWgetClientByName(%name) {

	%list = GetEveryoneIdList();
	for(%i = 0; (%id = GetWord(%list, %i)) != -1; %i++)
	{
		%displayName = Client::getName(%id);
		if(String::ICompare(%name, %displayName) == 0)
			return %id;
	}
	return -1;
}

function clipTrailingNumbers(%str) {

	for(%i=0; %i <= String::len(%str); %i++)
	{
		%a = String::getSubStr(%str, %i, 1);
		%b = (%a+1-1);

		if(String::ICompare(%b, %a) == 0)
			break;
	}
	%pos = %i;

	return String::getSubStr(%str, 0, %pos);
}

function GetArmorSkin(%Client) {
	%list = $ClientData[%Client, "EquipList"];
	if(%list == "")
		return -1;
	for(%i = 0; (%w = GetWord(%list, %i)) != -1; %i+=2) {
		if($ItemData[%w, type] == $BodyAccessoryType)
			return $ItemData[%w, Equip]@" "@$ItemData[%w, shape];
	}
	return -1;
}

function UpdateSkin(%Client) {

	%s = Client::getSkinBase(%Client);
	%ae = GameBase::getEnergy(Client::getOwnedObject(%Client));

	%flag = true;

	%c = $RACE[%Client];
	if(%c == "MaleHuman" || %c == "FemaleHuman") {
		%armor = GetArmorSkin(%Client);
		%s = "rpghuman0";
		if(%armor != -1)
			%s = getWord(%armor, 0);
	}
	else if(%c == "DeathKnight")
		%s = "cphoenix";
	else if(%c == "Zombie Beast")
		%s = "undead";

//echo("SKIN "@%s@" RACE "@%c@" ARMOR "@%armor);

	if(Client::getSkinBase(%Client) != %s)
		Client::setSkin(%Client, %s);

	%a = $RaceToArmorType[$RACE[%Client]];

	if(%s == "undead") {
		Player::setArmor(%Client, %a);
		$ClientData[%Client, Robed] = "";
		refreshAll(%Client);
	}
	else if($ClientData[%Client, "isMimic"]) {
		Player::setArmor(%Client, %a);
		$ClientData[%Client, Robed] = "";
		refreshAll(%Client);
	}
	else if((%a = getWord(%armor, 1)) == Robed) {
		if(Player::setArmor(%Client, $RACE[%Client]@"RobedArmor7")) {
			$ClientData[%Client, Robed] = true;
			refreshAll(%Client);
			%flag = "IsRobed";
		}
	}
	else {
		Player::setArmor(%Client, $RACE[%Client]@"Armor7");
		$ClientData[%Client, Robed] = "";
		refreshAll(%Client);
	}

	GameBase::setEnergy(Client::getOwnedObject(%Client), %ae);

	return %flag;
}

function UpdateTeam(%Client) {

	%t = $TeamForRace[$RACE[%Client]];
	GameBase::setTeam(%Client, %t);
}

function ChangeRace(%Client, %race) {

	if(%race == "DeathKnight")
		$RACE[%Client] = "DeathKnight";
	else if(%race == "Human")
		$RACE[%Client] = Client::getGender(%Client)@"Human";
	else if(%race == "MaleHuman")
		$RACE[%Client] = "MaleHuman";
	else if(%race == "FemaleHuman")
		$RACE[%Client] = "FemaleHuman";
	else
		$RACE[%Client] = %race;

	%a = $RaceToArmorType[$RACE[%Client]];

	if(UpdateSkin(%Client) != "IsRobed") {
		if(!Player::setArmor(%Client, %a))
			Player::setArmor(%Client, $RaceToArmorType[Client::getGender(%Client)@"Human"]);
	}

	setHP(%Client, $MaxHP[%Client]);
	setMANA(%Client, getMaxMANA(%Client));
	setSTAMINA(%Client, getSTAMINA(%Client));

	refreshAll(%Client);
}

function ClearVariables(%Client) {

	%name = Client::getName(%Client);

	//clear variables
	ClearFunkVar(%name);

	$RACE[%Client] = "";
	$GROUP[%Client] = "";
	$CLASS[%Client] = "";
	$EXP1[%Client] = "";
	$EXP2[%Client] = "";
	$EXP3[%Client] = "";
	$COINS[%Client] = "";
	$MaxCOINS[%Client] = "";
	$BANK[%Client] = "";
	$grouplist[%name] = "";
	$defaultTalk[%Client] = "";
	$password[%Client] = "";
	$stealskill[%Client] = "";
	$playerCurrentSpell[%Client] = "";
	$spellList[%name] = "";
	$inArena[%Client] = "";
	$AP[%Client, 1] = "";
	$AP[%Client, 2] = "";
	$AP[%Client, 3] = "";
	$AP[%Client, 4] = "";
	$AP[%Client, 5] = "";
	$AP[%Client, 6] = "";
	$AP[%Client, 7] = "";
	$BurnedClientTagId[%Client] = "";
	$PL[%Client] = "";
	$LCK[%Client] = "";
	$zone[%Client] = "";
	$lastzone[%Client] = "";
	$BonusAttack[%Client] = "";
	$templvl[%Client] = "";	//used to determine WHEN someone levels up (in Game::refreshClientScore)
	$campPos[%Client] = "";
	$ignoreGlobal[%Client] = "";
	$LCKconsequence[%Client] = "";
	$HasLoadedAndSpawned[%Client] = "";
	$PlayerInfo[%Client] = "";
	$BankStorage[%Client] = "";
	$SpellCastStep[%Client] = "";
	$MaxHP[%Client] = "";
	$dead[%Client] = "";
	$PKflag[%Client] = "";
	$tmprecall[%Client] = "";
	$invisible[%Client] = "";
	$lastPos[%Client] = "";
	$blockHide[%Client] = "";
	$NextHitBash[%Client] = "";
	$blockBash[%Client] = "";

	$isBonused[%Client] = "";
	$showexp[%Client] = "";
	$Quiver[%Client] = "0 ";

	$Weight[%Client] = "";

	deleteVariables("ClientData"@%Client@"*");
	deleteVariables("Client::tmp"@%Client@"*");

	deleteVariables("*GotHitBy"@%Client);

	deleteVariables("BonusState"@%Client@"*");
	deleteVariables("BonusStateCnt"@%Client@"*");

	for(%c = 1; %c <= $MaxChocobo; %c++)
		Chocobo::Clear(%Client, %c);

	//this is only for bots
	$BotFollowDirective[$BotInfoAiName[%Client]] = "";
	$BotInfoAiName[%Client] = "";
	$SpawnBotInfo[%Client] = "";
	$noDropLootbagFlag[%Client] = "";		//used currently only for when bots fall off the edge of the map
	$dumbAIflag[%Client] = "";
	$botAttackMode[%Client] = "";
	$tmpbotdata[%Client] = "";
	$frozen[%Client] = "";
	//clear directives
	$aidirectiveTable[%Client, 99] = "";
	$aidirectiveTable[%Client, 127] = "";

	%Client.IsInvalid = "";
	%Client.currentShop = "";
	%Client.currentBank = "";
	%Client.adminLevel = "";
	%Client.lastWaitActionTime = "";
	%Client.lastActionTime = "";
	%Client.lastTossTime = "";
	%Client.lastMissMessage = "";
	%Client.choosingGroup = "";
	%Client.choosingClass = "";
	%Client.possessId = "";
	%Client.sleeping = "";
	%Client.lastMountTime = "";
	%clientId.replyTo = "";


	deleteVariables("damagedBy"@%name@"*");

	deleteVariables("state"@%Client@"*");
	deleteVariables("QuestCounter"@%name@"*");

	deleteVariables("ClientData"@%Client@"*");


	//clear skill variables
	$SkillCounter[%Client, $SlashingDamageType] = "";
	$SkillCounter[%Client, $PiercingDamageType] = "";
	$SkillCounter[%Client, $ProjectileDamageType] = "";
	$SkillCounter[%Client, $BludgeoningDamageType] = "";

	$AttackBonus[$SkillSlashing, %Client] = "";
	$AttackBonus[$SkillBludgeoning, %Client] = "";
	$AttackBonus[$SkillPiercing, %Client] = "";
	$AttackBonus[$SkillArchery, %Client] = "";
}

function ClearFunkVar(%name) {

		deleteVariables("SaveData[\""@%name@"\"*");
		deleteVariables("SaveData"@%name@"*");
}

function Down(%t) {

	%tinsec = %t * 60;
	for(%i = %t; %i > 1; %i--)
	{
		%a = (%tinsec - (60 * %i));
		schedule("dmsg(" @ %i @ ", \"minutes\");", %a);
		//schedule("messageAll(1, \"Server going down in \"@"@%i@"@\" minutes, please disconnect to save your character.\");", %a);
	}

	if(%tinsec > 60)
		%startfrom = 60;
	else
		%startfrom = %tinsec;

	for(%i = %startfrom; %i >= 1; %i -= 10)
	{
		%a = (%tinsec - %i);
		schedule("dmsg(" @ %i @ ", \"seconds\");", %a);
		//schedule("messageAll(1, \"Server going down in \"@"@%i@"@\" seconds, please disconnect to save your character.\");", %a);
	}
	%list = GetPlayerIdList();
	for(%i = 0; GetWord(%list, %i) != -1; %i++)
	{
		%id = GetWord(%list, %i);
		schedule("SaveCharacter(" @ %id @ ");", (%tinsec2=%tinsec-3), %id);
	}
	schedule("saveworld();", (%tinsec2=%tinsec-3));

	//schedule("focusserver();quit();", %tinsec);
	schedule("downomgwtfbbq();", %tinsec);
	$server::password="down";
}
function dmsg(%i, %w)
{
	if(!$canceldown){
		$down="reset:"@%i@%w@" ";
		echo("========= SERVER RESTARTING IN " @ %i @ " " @ %w @ " =========");
		messageAll(1, "Server restarting in " @ %i @ " " @ %w @ ", please disconnect to save your character.");
	}
}
function downomgwtfbbq()
{
	if($canceldown)
	{
		pecho("Reset cancelled");
		messageAll(1, "Server reset cancelled");
		$server::password="";
		$down="";
		return;
	}
	focusserver();
	quit();
}

function GetEveryoneIdList() {

	%list = "";
	%list = %list@GetPlayerIdList();
	%list = %list@GetBotIdList();
	return %list;

	//%i = $InfoListId++;
	//if($InfoListId > 5) {
	//	$InfoListId = 0;
	//	%i = 0;
	//}
	//$InfoList[%i] = "";

	//GetClientInfoList(5, %i);

	//return $InfoList[%i];
}

function GetEveryoneNameList() {

	%list = "";
	%list = %list@GetPlayerNameList();
	%list = %list@GetBotNameList();
	return %list;

	//%i = $InfoListId++;
	//if($InfoListId > 5) {
	//	$InfoListId = 0;
	//	%i = 0;
	//}
	//$InfoList[%i] = "";

	//GetClientInfoList(6, %i);

	//return $InfoList[%i];
}

function GetBotIdList() {

	%list = "";
	%tempSet = nameToID("MissionCleanup");
	if(%tempSet != -1)
	{
		%num = Group::objectCount(%tempSet);
		for(%i = 0; %i <= %num-1; %i++)
		{
		%tempItem = Group::getObject(%tempSet, %i);
			if(getObjectType(%tempItem) == "Player")
			{
				%Client = Player::getClient(%tempItem);
				if(Player::isAiControlled(%Client))
				{
					%list = %list @ %Client@" ";
				}
			}
		}
	}

	return %list;

	//%i = $InfoListId++;
	//if($InfoListId > 5) {
	//	$InfoListId = 0;
	//	%i = 0;
	//}
	//$InfoList[%i] = "";

	//GetClientInfoList(3, %i);

	//return $InfoList[%i];
}

function GetBotNameList() {

	%list = "";
	%tempSet = nameToID("MissionCleanup");
	if(%tempSet != -1)
	{
		%num = Group::objectCount(%tempSet);
		for(%i = 0; %i <= %num-1; %i++)
		{
			%tempItem = Group::getObject(%tempSet, %i);
			if(getObjectType(%tempItem) == "Player")
			{
				%Client = Player::getClient(%tempItem);
				if(Player::isAiControlled(%Client))
				{
					//%list = %list@Client::getName(%Client)@" ";
					%list = %list@$BotInfoAiName[%Client]@" ";
				}
			}
		}
	}
	return %list;

	//%i = $InfoListId++;
	//if($InfoListId > 5) {
	//	$InfoListId = 0;
	//	%i = 0;
	//}
	//$InfoList[%i] = "";

	//GetClientInfoList(4, %i);

	//return $InfoList[%i];
}

function GetPlayerIdList() {

	%list = "";
	for(%c = Client::getFirst(); %c != -1; %c = Client::getNext(%c))
		%list = %list @ %c@" ";

	return %list;
}

function GetPlayerNameList() {

	%list = "";
	for(%c = Client::getFirst(); %c != -1; %c = Client::getNext(%c))
		%list = %list @ Client::getName(%c)@" ";

	return %list;
}

//%type's
//1: Player Id
//2: Player Names
//3: Bot Id
//4: Bot Names
//5: All Id
//6: All Names
function GetClientInfoList(%type, %i) {

	%mset = newObject("set", SimSet);
	%n = containerBoxFillSet(%mset, $SimPlayerObjectType, "0 0 0", 99999, 99999, 99999, 0);
	Group::iterateRecursive(%mset, getPlayerInfoForList, %type, %i);
	deleteObject(%mset);
}

function getPlayerInfoForList(%object, %type, %i) {
	%Client = Player::getClient(%object);
	if(%type == 1) {
		if(!Player::isAiControlled(%Client))
			$InfoList[%i] = $InfoList[%i]@%Client@" ";
	}
	else if(%type == 2) {
		if(!Player::isAiControlled(%Client))
			$InfoList[%i] = $InfoList[%i]@Client::getName(%Client)@" ";
	}
	else if(%type == 3) {
		if(Player::isAiControlled(%Client))
			$InfoList[%i] = $InfoList[%i]@%Client@" ";
	}
	else if(%type == 4) {
		if(Player::isAiControlled(%Client))
			$InfoList[%i] = $InfoList[%i]@$BotInfoAiName[%Client]@" ";
	}
	else if(%type == 5) { //PlayerId
			$InfoList[%i] = $InfoList[%i]@%Client@" ";
	}
	else if(%type == 6) {
		if(!Player::isAiControlled(%Client))
			$InfoList[%i] = $InfoList[%i]@Client::getName(%Client)@" ";
		else
			$InfoList[%i] = $InfoList[%i]@$BotInfoAiName[%Client]@" ";
	}
}

function ChangeWeather() {

	//credits go to LabRat for the original code for this... Thanks Lab!
	if(OddsAre(1))
	{
		%intensity = getRandom();

		%x = -1 + (getRandom() * 2);
		%y = -1 + (getRandom() * 2);
		%z = -300 + (floor(getRandom() * 40));
		%vec = %x@" "@%y@" "@%z;

		%t = floor(getRandom() * 50);
		if(%t >= 0 && %t < 20)
			%type = 1;			//rain
		else
			%type = -1;			//stop any weather

		if(isObject("weather"))
			deleteObject("weather");

		if(%type == 1)
			%weather = newObject("weather", Snowfall, %intensity, %vec, 0, %type);
	}
}

function FindInvalidChar(%name) {

	//looks for invalid characters in player's name
	for(%a = 1; %a <= String::len($invalidChars); %a++)
	{
		%b = String::getSubStr($invalidChars, %a-1, 1);
		if(String::findSubStr(%name, %b) != -1)
		{
			return %a-1;
		}
	}
	return "";
}
function CheckForReservedWords(%name) {

	%w[%c++] = "ArenaGladiator";
	%w[%c++] = "Traveller";
	%w[%c++] = "Goblin";
	%w[%c++] = "Gnoll";
	%w[%c++] = "Orc";
	%w[%c++] = "Ogre";
	%w[%c++] = "Elf";
	%w[%c++] = "Undead";
	%w[%c++] = "Minotaur";
	%w[%c++] = "Uber";

	%we[%d++] = "[RM]";
	%we[%d++] = "[Save";

	for(%i = 1; %we[%i] != ""; %i++)
	{
		if(String::findSubStr(%name, %we[%i]) != -1)
			return %we[%i];
	}

	for(%i = 1; %w[%i] != ""; %i++)
	{
		if(String::ICompare(%name, %w[%i]) == 0)
			return %w[%i];
	}

	%list = GetBotNameList();
	for(%i = 0; (%b = GetWord(%list, %i)) != -1; %i++)
	{
		if(String::findSubStr(%name, %b) != -1)
			return %b;
	}
	return "";
}

function RandomPositionXY(%minrad, %maxrad) {

	%diff = %maxrad - %minrad;

	%tmpX = floor(getRandom() * (%diff*2)) - %diff;
	if(%tmpX < 0)
		%tmpX -= %minrad;
	else
		%tmpX += %minrad;

	%tmpY = floor(getRandom() * (%diff*2)) - %diff;
	if(%tmpY < 0)
		%tmpY -= %minrad;
	else
		%tmpY += %minrad;

	return %tmpX@" "@%tmpY@" ";
}

function OddsAre(%n) {

	%a = floor(getRandom() * %n);
	if(%a == %n-1)
		return True;
	else
		return False;
}

function TeleportToMarker(%Client, %markergroup, %testpos, %random) {

	%group = nameToID("MissionGroup\\"@%markergroup);

	if(%group != -1)
	{
		%num = Group::objectCount(%group);

		if(%random)
		{
			%r = floor(getRandom() * %num);
		      %marker = Group::getObject(%group, %r);

			%worldLoc = GameBase::getPosition(%marker);
			%worldRot = GameBase::getRotation(%marker);

			if(%testpos)
			{
				%set = newObject("tempset", SimSet);
				%n = containerBoxFillSet(%set, $SimPlayerObjectType, %worldLoc, 1.0, 1.0, 1.5, getWord(%worldLoc, 2));
				deleteObject(%set);

				if(%n > 0)
				{
					GameBase::setPosition(%Client, %worldLoc);
					GameBase::setRotation(%Client, %worldRot);
					return %worldLoc;
				}
			}
			else
			{
				GameBase::setPosition(%Client, %worldLoc);
				GameBase::setRotation(%Client, %worldRot);
				return %worldLoc;
			}
		}
		else
		{
			for(%i = 0; %i <= %num-1; %i++)
			{
			      %marker = Group::getObject(%group, %i);

				%worldLoc = GameBase::getPosition(%marker);
				%worldRot = GameBase::getRotation(%marker);

				if(%testpos)
				{
					//this is part of the method SF uses for their teleporters.  thanks Hosed
					%set = newObject("tempset", SimSet);
					%n = containerBoxFillSet(%set, $SimPlayerObjectType, %worldLoc, 1.0, 1.0, 1.5, getWord(%worldLoc, 2));
					deleteObject(%set);

					if(%n == 0)
					{
						GameBase::setPosition(%Client, %worldLoc);
						GameBase::setRotation(%Client, %worldRot);
						return %worldLoc;
					}
				}
				else
				{
					GameBase::setPosition(%Client, %worldLoc);
					GameBase::setRotation(%Client, %worldRot);
					return %worldLoc;
				}
			}
		}
	}

	return False;
}

function TossLootbag(%Client, %loot, %vel, %private, %t, %forcedrop) {

	if(getWord(%loot, 1) == -1)
		return;

	%player = Client::getOwnedObject(%Client);
	%name = Client::getName(%Client);

	if($ClientData[%name, OwnsLoot] <= $MaxDroppedPacksPerPlayer || Player::isAiControlled(%Client) || %forcedrop) {

		%lootbag = newObject("", "Item", "Lootbag", 1, false);

		if(%private) {
			schedule("$loot["@%lootbag@"] = \"* "@%loot@"\";", getFinalLVL(%Client) * %t, %lootbag);
			%loot = Client::getName(%Client)@" "@%loot;
		}
		else if(%private == Drop) {
			%loot = "* "@%loot;
			schedule(""@%lootbag@".Item::Pop();", 30, %lootbag);
		}
		else {
			%loot = "* "@%loot;
			if($LootbagPopTime != -1)
			  	schedule(""@%lootbag@".Item::Pop();", $LootbagPopTime, %lootbag);
		}

		$ClientData[%name, OwnsLoot]++;
		$loot[%lootbag] = %loot@" ";
		$loottag[%lootbag] = %name;

		$loot[%lootbag, LTT] = getIntegerTime(true) >> 5;

		addToSet("MissionCleanup", %lootbag);
		GameBase::setMapName(%lootbag, "Backpack");
		GameBase::throw(%lootbag, %player, %vel, false);
	}
	else {
		if(!$ClientData[%Client, WantsToDropOverLimit]) Client::sendMessage(%Client, 0, "You can only have "@$MaxDroppedPacksPerPlayer@" packs dropped at one time.(type #nolimit true if you want to drop more items, but NO pack will be dropped)");
	}
}

function dot_op_Item::Pop(%this) {
	if(getWord($loot[%this], 1) != -1) {
		RMItem::Pop(%this);
	}
}

function RMItem::Pop(%this) {

	%time =  getIntegerTime(true) >> 5;
	if(%time-(60 * 15) >= $loot[%this, LTT]) {

		$loot[%this, LTT] = "";
		$ClientData[$loottag[%this], OwnsLoot]--;
		$loot[%this] = "";
		$loottag[%this] = "";
		$loot[%this, LTT] = "";
		Item::Pop(%this);
	}
	else
		schedule("RMItem::Pop("@%this@");", (60 * 2));
}

function ChangeSky(%sky) {

	%group = nameToId("MissionGroup\\LandScape");
	if(%group != -1)
	{
		%count = Group::objectCount(%group);
		for(%i = 0; %i <= %count-1; %i++)
		{
			%object = Group::getObject(%group, %i);
			if(getObjectType(%object) == "Sky")
			{
				deleteobject(%object);
			}
		}
	}

	%newsky = newObject(Sky, Sky, 0, 0, 0, %sky, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
	addToSet("MissionGroup\\LandScape", %newsky);
}


function round(%n) {

	if(%n < 0)
	{
		%t = -1;
		%n = -%n;
	}
	else if(%n >= 0)
		%t = 1;

	%f = floor(%n);
	%a = %n - %f;
	if(%a < 0.5)
		%b = 0;
	else if(%a >= 0.5)
		%b = 1;

	return (%f + %b) * %t;
}

function FixDecimals(%c) {
	%d = round(%c * 10);
	%m = (%d / 10) * 1.000001;
	return %m;
}

function RefreshAll(%Client) {

	if(%Client == -1 || %Client == "" || !$HasLoadedAndSpawned[%Client])
		return;

//	setMaxMANA(%Client);
	refreshMANAREGEN(%Client);
	AddHP(%Client, getFinalLVL(%Client));
//	refreshHP(%Client); //, %value)
	refreshHPREGEN(%Client);
	UpdateQuestStats(%Client);

	if(!Player::isAiControlled(%Client)) { //Bots don't need these
		RefreshWeight(%Client);
		getSTAMINA(%Client);
		refreshSTAREGEN(%Client);
		UpdateClassName(%Client);
		remoteEval(%Client,"RefreshHPMPEXP", Fix(getHP(%Client), %Client, HP), Fix(getMANA(%Client), %Client, MP), Fix(getSTA(%Client), %Client, STA), Fix(getTNL(%Client, Strip), %Client, EXP), $ShowEXP[%Client]);
	}
}

function FixStuffString(%stuff) {
	%nstuff = " ";
	for(%i = 0; (%w = GetWord(%stuff, %i)) != -1; %i++)
		%nstuff = %nstuff @ %w @ " ";

	return %nstuff;
}

function GatherBotInfo(%group) {

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
			else if(%type == "STR")
				$BotInfo[%aiName, STR] = %info;
			else if(%type == "DEX")
				$BotInfo[%aiName, DEX] = %info;
			else if(%type == "CON")
				$BotInfo[%aiName, CON] = %info;
			else if(%type == "INT")
				$BotInfo[%aiName, INT] = %info;
			else if(%type == "LCK")
				$BotInfo[%aiName, LCK] = %info;

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
	}
	$BotInfo[%aiName, SAY, %biggestn+1] = "";
	$BotInfo[%aiName, NSAY, %biggestn+1] = "";
	$BotInfo[%aiName, CUE, %biggestn+1] = "";
	$BotInfo[%aiName, NCUE, %biggestn+1] = "";
}

function HasThisStuff(%Client, %list) {

	%name = Client::getName(%Client);

	//--------
	// PASS 1
	//--------
	%flag = False;

	for(%i = 0; GetWord(%list, %i) != -1; %i+=2)
	{
		%w = GetWord(%list, %i);
		%w2 = GetWord(%list, %i+1);

		if(%w == "LVLG")
		{
			if(getFinalLVL(%Client) > %w2)
				%flag = True;
			else
				%flag = 667;
		}
		else if(%w == "LVLS")
		{
			if(getFinalLVL(%Client) < %w2)
				%flag = True;
			else
				%flag = 667;
		}
		else if(%w == "LVLE")
		{
			if(getFinalLVL(%Client) == %w2)
				%flag = True;
			else
				%flag = 667;
		}
	}

	if(%flag == 667)
		return %flag;


	//--------
	// PASS 2
	//--------
	%cntindex = 0;
	%flag = False;

	for(%i = 0; GetWord(%list, %i) != -1; %i+=2)
	{
		%w = GetWord(%list, %i);
		%w2 = GetWord(%list, %i+1);
		if(%w == "CNT")
		{
			%cntindex++;
			%tmpcnt[%cntindex] = %w2;
		}
		else if(%w == "CNTAFFECTS")
		{
			%tmpcntaffects[%cntindex] = %w2;
		}
	}

	//Process the counter data, if any
	for(%i = 1; %tmpcnt[%i] != ""; %i++)
	{
		if(%tmpcnt[%i] != "" && %tmpcntaffects[%i] != "")
		{
			%firstchar = String::getSubStr(%tmpcnt[%i], 0, 1);
			%n = floor(String::getSubStr(%tmpcnt[%i], 1, 9999));
			if(%firstchar == "<")
			{
				if($QuestCounter[%name, %tmpcntaffects[%i]] < %n)
					%flag = True;
				else
					%flag = 666;
			}
			else if(%firstchar == ">")
			{
				if($QuestCounter[%name, %tmpcntaffects[%i]] > %n)
					%flag = True;
				else
					%flag = 666;
			}
			else if(%firstchar == "=")
			{
				if($QuestCounter[%name, %tmpcntaffects[%i]] == %n)
					%flag = True;
				else
					%flag = 666;
			}
		}
		if(%flag == 666)
			return %flag;
	}


	//--------
	// PASS 3
	//--------
	%flag = False;

	for(%i = 0; GetWord(%list, %i) != -1; %i+=2)
	{
		%w = GetWord(%list, %i);
		%w2 = GetWord(%list, %i+1);
		if(%w == "COINS")
		{
			if($COINS[%Client] >= %w2)
				%flag = True;
			else
				return False;
		}
		else if(%w != "COINS" && %w != "LVLG" && %w != "LVLS" && %w != "LVLE" && %w != "CNT" && %w != "CNTAFFECTS")
		{

			if(String::findSubStr(%w, "Shape:_") == 0)
				%flag = True;
			else {

				if($ItemData[%w, header] == $Headers::Z)
					%ilist = "QuestList";
				else
					%ilist = "ItemList";
				if(Client::getItemCount(%Client, %w, %ilist) >= %w2)
					%flag = True;
				else
					return False;
			}
		}
	}

	return %flag;
}

function TakeThisStuff(%Client, %list) {

	for(%i = 0; GetWord(%list, %i) != -1; %i+=2)
	{
		%w = GetWord(%list, %i);
		%w2 = GetWord(%list, %i+1);
		if(%w == "COINS")
		{
			if($COINS[%Client] >= %w2)
				$COINS[%Client] -= %w2;
			else
				return False;
		}
		else if(%w == "CNT" || %w == "CNTAFFECTS" || %w == "LVLG" || %w == "LVLS" || %w == "LVLE")
		{
			//ignore
		}
		else
		{
			if(String::findSubStr(%w, "Shape:_") == -1) {

				if($ItemData[%w, header] == $Headers::Z)
					%ilist = "QuestList";
				else
					%ilist = "ItemList";

				%amount = Client::getItemCount(%Client, %w, %ilist);
				if(%amount >= %w2)
					Client::addItemCount(%Client, %w, -%w2, %ilist);
				else
					return False;
			}
		}
	}

	return True;
}

$APINFO[STR] = 1;
$APINFO[DEX] = 2;
$APINFO[CON] = 3;
$APINFO[INT] = 4;
$APINFO[WIS] = 5;
$APINFO[STA] = 6;

function GiveThisStuff(%Client, %list, %echo) {

	%name = Client::getName(%Client);

	%cntindex = 0;

	for(%i = 0; (%w =  GetWord(%list, %i)) != -1; %i+=2) {
		%w2 = GetWord(%list, %i+1);

		//if there is a / in %w2, then what trails after the / is the minimum random number between 0 and 100 which
		//is applied as a percentage to the starting number of %w2
		%spos = String::findSubStr(%w2, "/");
		if(%spos > 0) {
			%original = String::getSubStr(%w2, 0, %spos);
			%perc = String::NEWgetSubStr(%w2, %spos+1, 99999);

			%r = floor(getRandom() * (100-%perc))+%perc+1;
			if(%r > 100) %r = 100;

			%w2 = round(%original * (%r/100));
			if(%w2 < 0) %w2 = 0;

			%skip = true;
		}

		//if there is a - in %w2, then it randomly picks a number between these numbers. No lower, no higher.
		//Ex: "LVL 1-5"
		//Will only pick a number randomly between 1-5
		%spos = String::findSubStr(%w2, "-");
		if(%spos > 0 && !%skip) {
			%l = String::NEWgetSubStr(%w2, 0, %spos);
			%h = String::NEWgetSubStr(%w2, %spos+1, 99999);
			%perc = floor(getRandom() * 100);

			if(%l == 0 || %l == "") {} // is it just a -num ?
			else {
				%lh = %h - %l;

				%w2 = Cap(round((%lh * (%perc/100)) + %l), %l, %h);
			}
		}
		//if there is a d in %w2 AND it has a number on either side, then it's a dice roll
		%dpos = String::findSubStr(%w2, "d");
		if(%dpos > 0) {
			%l1 = String::getSubStr(%w2, %dpos-1, 1);
			%l2 = floor(%l1);
			%r1 = String::getSubStr(%w2, %dpos+1, 1);
			%r2 = floor(%r1);
			if(%dpos > 0 && String::ICompare(%l1, %l2) == 0 && String::ICompare(%r1, %r2) == 0)
			{
				%w2 = GetRoll(%w2);
				if(%w2 < 1) %w2 = 1;
			}
		}
		if(%w == "COINS") {

			if(Player::isAiControlled(%Client)) {
				$COINS[%Client] += %w2;
			}
			else {
				$COINS[%Client] = $COINS[%Client]+%w2;
				%tmpC = $MaxCOINS[%Client] - $COINS[%Client];
				if(%tmpC < 0) {
					%DropCoins = True;
					%tmpC = -%tmpC;//make it +
					$COINS[%Client] -= %tmpC;
					%w2 -= %tmpC;
				}
				if(%w2 > 0) {
					if(%echo) Client::sendMessage(%Client, 0, "You received "@%w2@" gil.");
				}
				else
					if(%echo) Client::sendMessage(%Client, 0, "You can't hold anymore gil! "@$COINS[%Client]);
				if(%DropCoins) {
					%time =  getIntegerTime(true) >> 5;
					if((%time - %Client.lastTossTime) > 2) {
						%Client.lastTossTime = %time;
						TossLootbag(%Client, "COINS "@%tmpC, 10, False, 0);
					}
				}
			}
		}
		else if(%w == "EXP") {
			if(GetLevel(getFixedExp(%Client)) < 999)
			{
				GiveExp(%Client, FixM(%w2, True));
				if(%echo) Client::sendMessage(%Client, 0, "You received "@FixM(%w2)@" experience.");
			}
		}
		else if(String::findSubStr("STR DEX CON INT WIS STA ", %w@" ") != -1) {
			$AP[%Client, $APINFO[%w]] += %w2;
			if(%echo) Client::sendMessage(%Client, 0, "You received "@%w2@" "@%w@".");
		}
		else if(%w == "LCK") {
			$LCK[%Client] += %w2;
			if(%echo) Client::sendMessage(%Client, 0, "You received "@%w2@" LCK.");
		}
		else if(%w == "CLASS") {
			$CLASS[%Client] = %w2;
			$GROUP[%Client] = $ClassGroup[$CLASS[%Client]];
		}
		else if(%w == "LVL") {
			//note: the class MUST be specified in %stuff prior to this call

			GiveExp(%Client, $EXPtable[%w2], force);
			GiveExp(%Client, "0 0 5");

		}
		else if(%w == "HP") {
			//note: the class AND level MUST be specified in %stuff prior to this call
			$MaxHP[%Client] = %w2;
		}
		else if(%w == "CNT") {
			%cntindex++;
			%tmpcnt[%cntindex] = %w2;
		}
		else if(%w == "CNTAFFECTS") {
			%tmpcntaffects[%cntindex] = %w2;
		}
		else if(%w == "RandMat" ) {
			material::drop(%w,%w2,%client);
		}
		else {
			if(%w2 == 0)
				return;//No need to keep going if %w2 == 0...
			Item::giveItem(%Client, %w, %w2, %echo);
		}
	}
	RefreshAll(%Client);
	//Process the counter data, if any
	for(%i = 1; %tmpcnt[%i] != ""; %i++)
	{
		if(%tmpcnt[%i] != "" && %tmpcntaffects[%i] != "")
		{
			%first = String::getSubStr(%tmpcnt[%i], 0, 1);
			if(%first == "+" || %first == "-")
				$QuestCounter[%name, %tmpcntaffects[%i]] += floor(%tmpcnt[%i]);
			else
				$QuestCounter[%name, %tmpcntaffects[%i]] = floor(%tmpcnt[%i]);
		}
	}
}

function getSpawnIndex(%aiName) {

	for(%i = 1; $spawnIndex[%i] != ""; %i++)
	{
		if($spawnIndex[%i] == %aiName)
			return %i;
	}
	return -1;
}

function FellOffMap(%id) {

	if(Player::isAiControlled(%id))
	{
		$noDropLootbagFlag[%id] = True;
		playNextAnim(%id);
		Player::Kill(%id);
	}
	else
	{
		Item::setVelocity(%id, "0 0 0");
		%spawnMarker = Game::pickPlayerSpawn(%id, "", True);
		if(%spawnMarker) {
			GameBase::setPosition(%id, GameBase::getPosition(%spawnMarker));
			GameBase::setRotation(%id, GameBase::getRotation(%spawnMarker));
			Client::sendMessage(%id, $MsgGreen, "You have been returned to Rin Vale.");
		}
		else
			Schedule("FellOffMap("@%id@");", 2);

		//TeleportToMarker(%id, "TheArena\\TeleportExitMarkers", 0, 0);
	}
}

function Game::refreshClientScore(%Client) {

	//echo("$templvl["@%Client@"]: "@$templvl[%Client]);
	//echo("GetLevel("@$EXP[%Client]@", "@$CLASS[%Client]@"): "@GetLevel($EXP[%Client], $CLASS[%Client]));
//	if(GetLevel(getFixedExp(%Client)) != $templvl[%Client] && $HasLoadedAndSpawned[%Client] && $templvl[%Client] != "") {

	if($HasLoadedAndSpawned[%Client]) {
		%lvl = GetLevel(getFixedExp(%Client));

		if(%lvl != $templvl[%Client]) {

			//client has leveled up/down
			%lvls = (%lvl - $templvl[%Client]);

			$templvl[%Client] = %lvl;

			if($templvl[%Client] == 5 && %lvls > 0)
				Client::sendMessage(%Client, 0, "One of the guards [near starting place in Rin] would like to help you.");
			if($templvl[%Client] == 10 && %lvls > 0)
				Client::sendMessage(%Client, 0, "You have gained "@floor(21-$CLASSTOTALAPGAIN[$CLASS[%Client]])@" AP points, add them wisely. (You gain "@(21-$CLASSTOTALAPGAIN[$CLASS[%Client]])@" every 10 levels)");
			if($templvl[%Client] == 15 && %lvls > 0)
				Client::sendMessage(%Client, 0, "Guile [at the Inn in Rin] said he would like to talk to you.");

			if($templvl[%Client] >= $ClientData[%Client, AP_Check] && %lvls > 0) {
				$ClientData[%Client, AP_Check] += 10;
				$ClientData[%Client, APPoints] += floor(21-$CLASSTOTALAPGAIN[$CLASS[%Client]]);
			}

			//===NEW AddSkills===
			if(!Player::isAiControlled(%Client)) {
				if($isBlessed[%Client] == true)
					%BlessedSoul = 1 * %lvls;
				else
					%BlessedSoul = 0;

				$AP[%Client, 1] += $AddAPSTRClass[$CLASS[%Client]] * %lvls + %BlessedSoul;
				$AP[%Client, 2] += $AddAPDEXClass[$CLASS[%Client]] * %lvls + %BlessedSoul;
				$AP[%Client, 3] += $AddAPCONClass[$CLASS[%Client]] * %lvls + %BlessedSoul;
				$AP[%Client, 4] += $AddAPINTClass[$CLASS[%Client]] * %lvls + %BlessedSoul;
				$AP[%Client, 5] += $AddAPWISClass[$CLASS[%Client]] * %lvls + %BlessedSoul;
				$AP[%Client, 6] += $AddAPSTAClass[$CLASS[%Client]] * %lvls + %BlessedSoul;

				if(%lvls > 0) {
					PlaySound(SoundLevelUp, GameBase::getPosition(%Client));
					Schedule("ShowLVLup("@%Client@","@%lvls@");", 1.5);
					Client::sendMessage(%Client,0,"LEVEL: "@getFinalLVL(%Client));
				}
				else if(%lvls < 0) {
					Schedule("ShowLVLup("@%Client@","@%lvls@");", 1.5);
					Client::sendMessage(%Client,0,"LEVEL: "@getFinalLVL(%Client));
				}
			}

			AddHP(%Client, %lvls);

			RefreshAll(%Client);
		}
	}

	if(CheckBonus(%Client, "PKer")) {
		Client::setScore(%Client, "%n the PKer", getFinalLVL(%Client));
		return;
	}
	Client::setScore(%Client, "%n", getFinalLVL(%Client));
}

function ShowLVLup(%Client,%lvls) {

	if(%lvls > 0) {
		if(%lvls == 1)
			remoteEval(%Client,"ATKText", "<jc><f1>LEVEL UP!", true);
		else
			remoteEval(%Client,"ATKText", "<jc><f1>LEVELS UP!", true);
	}
	if(%lvls < 0) {
		if(%lvls == -1)
			remoteEval(%Client,"ATKText", "<jc><f1>LEVEL DOWN...", true);
		else
			remoteEval(%Client,"ATKText", "<jc><f1>LEVELS DOWN...", true);
	}
}

function SetStuffString(%stuff, %item, %amount) {

	//replaces both Add and Remove stuff string functions by enabling negative values for %amount
	%stuff = FixStuffString(%stuff);
	%pos = String::findSubStr(%stuff, " "@%item@" ");
	if(%pos != -1)
	{
		%a = String::NEWgetSubStr(%stuff, %pos+1, 99999);
		%amt = GetWord(%a, 1);	//getword 0 would be the item, so getword 1 is the amount (which follows the item)

		%part1 = String::NEWgetSubStr(%stuff, 0, %pos+1);
		%part2 = String::NEWgetSubStr(%stuff, %pos+String::len(%item)+String::len(%amt)+3, 99999);

		%b = %amt + %amount;
		if(%b <= 0)
			%part3 = "";
		else
			%part3 = %item@" "@%b@" ";

		%final = %part1 @ %part2 @ %part3;
	}
	else
		%final = %stuff @ %item@" "@%amount@" ";
	return %final;
}

function GetStuffStringCount(%stuff, %item)
{

	%stuff = FixStuffString(%stuff);

	%pos = String::findSubStr(%stuff, " " @ %item @ " ");

	if(%pos != -1)
	{
		%a = String::NEWgetSubStr(%stuff, %pos+1, 99999);
		%amt = GetWord(%a, 1);

		return %amt;
	}

	return 0;
}

function FixStuffString(%stuff) {
	%nstuff = " ";
	for(%i = 0; GetWord(%stuff, %i) != -1; %i++)
	{
		%w = GetWord(%stuff, %i);
		%nstuff = %nstuff @ %w@" ";
	}

	return %nstuff;
}

//$tst = "I am typing a whole bunch of bullshit on this screen so I can fix this stupid bug concerning the storage. The string::getsubstr function can only get two-hundred fifty five (255) characters from a string, so whenever this function was performed on the storage stuff string, alot of info would get lost. Players were actually capable of spawning items that aren't normally supposed to be spawned, like the Deployable Base for example. This is a big problem, but with this NEWgetSubStr function that I wrote, which splits up strings into chunks of 255 in order to get the string portion properly, the storage bug should go away and there should be a hell of a lot less cheating.";
function String::NEWgetSubStr(%s, %x, %y) {

	%len = %y;
	%chunks = floor(%len / 255) + 1;

	%q = %len;
	%nx = %x;
	%final = "";

	for(%i = 1; %i <= %chunks; %i++)
	{
		%q = %q - 255;
		if(%q <= 0)
			%chunkLen = %q+255;
		else
			%chunkLen = 255;

		%final = %final @ String::getSubStr(%s, %nx, %chunkLen);
		%nx = %nx + %chunkLen;
	}

	return %final;
}

function AddToCommaList(%list, %item) {
	%list = %list @ %item @ $sepchar;

	return %list;
}
function RemoveFromCommaList(%list, %item) {
	%a = $sepchar @ %list;
	%a = String::replace(%a, $sepchar @ %item @ $sepchar, ",");
	%list = String::NEWgetSubStr(%a, 1, 99999);

	return %list;
}
function IsInCommaList(%list, %item) {

	%a = $sepchar @ %list;
	if(String::findSubStr(%a, "," @ %item @ ",") != -1)
		return True;
	else
		return False;
}
function CountObjInCommaList(%list) {
	for(%i = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999))
		%cnt++;
	return %cnt;
}

function CountObjInList(%list) {
	for(%i = 0; GetWord(%list, %i) != -1; %i++){}

	return %i;
}

function GetRoll(%roll, %optionalMinMax) {

	//this function accepts the following syntax, where N is any positive number NOT containing a +:
	//NdN
	//NdN+N
	//NdN-N
	//NdNxN
	//NdN+NxN
	//NdN-NxN

	%d = String::findSubStr(%roll, "d");
	%p = String::findSubStr(%roll, "+");
	if(%p == -1)
		%m = String::findSubStr(%roll, "-");
	%x = String::findSubStr(%roll, "x");

	if(%d == -1)
		return %roll;

	if(%x == -1)
		%x = String::len(%roll);

	%numDice = floor(String::getSubStr(%roll, 0, %d));
	if(%p != -1)
	{
		%diceFaces = String::getSubStr(%roll, %d+1, %p-%d-1);
		%bonus = String::getSubStr(%roll, %p+1, %x-1);
	}
	else if(%p == -1 && %m != -1)
	{
		%diceFaces = String::getSubStr(%roll, %d+1, %m-%d-1);
		%bonus = -String::getSubStr(%roll, %m+1, %x-1);
	}
	else
		%diceFaces = String::getSubStr(%roll, %d+1, 99999);

	%total = 0;
	for(%i = 1; %i <= %numDice; %i++)
	{
		if(%optionalMinMax == "min")
			%r = 1;
		else if(%optionalMinMax == "max")
			%r = %diceFaces;
		else
			%r = floor(getRandom() * %diceFaces)+1;

		%total += %r;
	}

	if(%bonus != "")
		%total += %bonus;

	if(%x != String::len(%roll))
		%total *= String::getSubStr(%roll, %x+1, 99999);

	return %total;
}

function GetCombo(%n) {

	//--- This is used so ComboTables don't get overwritten by simultaneous calls ---
	$w++;
	if($w > 20) $w = 1;
	//-------------------------------------------------------------------------------

	for(%i = 1; $ComboTable[$w, %i] != ""; %i++)
		$ComboTable[$w, %i] = "";

	%cnt = 0;

	while(%i != -1)
	{
		for(%i = 0; pow(2, %i) <= %n; %i++){}
		%i--;

		if(%i >= 0)
		{
			$ComboTable[$w, %cnt++] = pow(2, %i);
			%n -= pow(2, %i);
		}
	}

	return $w;
}

function IsPartOfCombo(%combo, %n) {
	%w = GetCombo(%combo);

	%flag = false;

	for(%i = 1; $ComboTable[%w, %i] != ""; %i++)
	{
		if(%n == $ComboTable[%w, %i])
			%flag = true;

		//It's a good idea to clean up after oneself, especially with all the ComboTables that would be floating around
		$ComboTable[%w, %i] = "";
	}

	return %flag;
}

function DefaultRangedProj(%Client, %item) {
	//this function equips a %Client's ranged weapon with any projectile available to it.  it DOES NOT pick the "best"
	//type of projectile for the weapon, it simply picks the next one available.

	//This function was written for BOTS.  It WILL work for players, but it will defeat the purpose of manually
	//equipping projectiles for each ranged weapon.

	%list = GetAccessoryList(%Client, 10, -1);

	for(%i = 0; GetWord(%list, %i) != -1; %i++)
	{
		%proj = GetWord(%list, %i);

		if(String::findSubStr($ProjRestrictions[%proj], ","@%item@",") != -1)
		{
			$LoadedProjectile[%Client, %item] = %proj;
			return true;
		}
	}
	return false;
}

function IsDead(%id) {
	%Client = Player::getClient(%id);
	%player = Client::getOwnedObject(%Client);

	if(%player == -1)
		return True;
	else
		return False;
}


function Cap(%n, %lb, %ub) {

	if(%lb != "inf") {
		if(%n < %lb)
			%n = %lb;
	}
	if(%ub != "inf") {
		if(%n > %ub)
			%n = %ub;
	}
	return %n;
}

function GetNESW(%pos1, %pos2) {
	%v1 = Vector::sub(%pos1, %pos2);
	%v2 = Vector::getRotation(%v1);
	%a = GetWord(%v2, 2);

	if(%a >= 2.7475 && %a <= 3.15 || %a >= -3.15 && %a <= -2.7475)
		%d = "North";
	else if(%a >= 1.9625 && %a <= 2.7475)
		%d = "North East";
	else if(%a >= 1.1775 && %a <= 1.9625)
		%d = "East";
	else if(%a >= 0.3925 && %a <= 1.1775)
		%d = "South East";
	else if(%a >= -0.3925 && %a <= 0.3925)
		%d = "South";
	else if(%a >= -1.1775 && %a <= -0.3925)
		%d = "South West";
	else if(%a >= -1.9625 && %a <= -1.1775)
		%d = "West";
	else if(%a >= -2.7475 && %a <= -1.9625)
		%d = "North West";

	return %d;
}

function InitSoundPoints() {

	%group = nameToID("MissionGroup\\SoundPoints");

	if(%group != -1)
	{
		for(%i = 0; %i <= Group::objectCount(%group)-1; %i++)
		{
		      %this = Group::getObject(%group, %i);
			%info = Object::getName(%this);

			if(%info != "")
			{
				GameBase::playSound(%this, %info, 0);
				//echo("Playing sound "@%info@" for object "@%this);
			}
		}
	}
}

function SetOnGround(%Client, %extraZ) {
	%maxdist = 5000;

	%origpos = GameBase::getPosition(%Client);

	%x = GetWord(%origpos, 0);
	%y = GetWord(%origpos, 1);
	%z = GetWord(%origpos, 2);

	%finalpos = %x@" "@%y@" "@%z + %extraZ;

	GameBase::setPosition(%Client, %finalpos);

	%index = 0;
	//for(%i = 0; %i >= -3.15; %i -= 1.57)
	for(%i = 0; %i >= -4.725; %i -= 0.785)
	{
		if(GameBase::getLOSinfo(Client::getOwnedObject(%Client), %maxdist, %i@" 0 0"))
		{
			%index++;
			%pos[%index] = $los::position;
		}
	}

	%closest = %maxdist+1;
	for(%j = 1; %j <= %index; %j++)
	{
		%dist = Vector::getDistance(%pos[%j], %finalpos);
		if(%dist < %closest)
		{
			%closest = %dist;
			%closestIndex = %j;
		}
	}

	if(%pos[%closestIndex] != "")
		GameBase::setPosition(%Client, %pos[%closestIndex]);
	else
		GameBase::setPosition(%Client, %origpos);

	return %pos[%closestIndex];
}

function WalkSlowInvisLoop(%Client, %delay, %grace) {
	%pos = GameBase::getPosition(%Client);
	if($lastPos[%Client] == "")
		$lastPos[%Client] = %pos;

	if(Vector::getDistance(%pos, $lastPos[%Client]) <= %grace && $invisible[%Client])
	{
		$lastPos[%Client] = GameBase::getPosition(%Client);
		schedule("WalkSlowInvisLoop("@%Client@", "@%delay@", "@%grace@");", %delay, %Client);
	}
	else
	{
		if($invisible[%Client])
		{
			GameBase::startFadeIn(%Client);
			$invisible[%Client] = "";
		}

		Client::sendMessage(%Client, $MsgRed, "You are no longer Hiding In Shadows.");

		$lastPos[%Client] = "";
		$blockHide[%Client] = True;
		schedule("$blockHide["@%Client@"] = \"\";", 10);
	}
}

function RecursiveWorld(%seconds) {

	//This function is a substitute for a few recursive schedule calls.  By having all schedule calls replaced by
	//this huge one, there should be less cause for lag.  As a standard, the RecursiveWorld should be called every
	//5 seconds.

	//(note, spawn crystal loop is not in this function, because I judge it causes less lag when used separately)

	$ticker[1] = floor($ticker[1]+1);
	$ticker[2] = floor($ticker[2]+1);
	$ticker[3] = floor($ticker[3]+1);
//	$ticker[4] = floor($ticker[4]+1); Arena

//	$ticker[5] = floor($ticker[5]+1);

	$ticker[6] = floor($ticker[6]+1); //Chocobo $ticker

	if($ticker[1] >= ($SaveWorldFreq / %seconds))
	{
		//Save World call
		SaveWorld();

		%list = GetPlayerIdList();
		for(%i = 0; GetWord(%list, %i) != -1; %i++)
		{
			%id = GetWord(%list, %i);

			if(SaveCharacter(%id, True)) {
				if($Chocobo[%id] >= 1)
					%ChocoboSaved = " Saved "@$Chocobo[%id]@" Chocobos.";
				Client::sendMessage(%id, $MsgBeige, Client::getName(%id)@" saved."@%ChocoboSaved);
			}
		}

		//check velocity of all the bots and kill off the bots that are falling too fast (ie, ran off the map)
		%list = GetEveryoneIdList();
		for(%i = 0; GetWord(%list, %i) != -1; %i++)
		{
			%id = GetWord(%list, %i);
			%vel = Item::getVelocity(%id);
			if(getWord(%vel, 2) <= -500)
			{
				FellOffMap(%id);
			}
		}

		$ticker[1] = 0;
	}
	if($ticker[2] >= ($ChangeWeatherFreq / %seconds))
	{
		//change weather call
		ChangeWeather();

		$ticker[2] = 0;
	}

	if($ticker[3] >= 1 && $nightDayCycle)
	{
		%a = (($initHaze * 2) / $fullCycleTime) * %seconds;

		$currentHaze -= %a;

		if($currentHaze < 0)
			%h = -$currentHaze;
		else
			%h = $currentHaze;

		if($currentHaze < -$initHaze)
			$currentHaze = $initHaze;

		setTerrainVisibility(8, 800, %h);

		//-------

		for(%i = 1; %i <= 5; %i++)
		{
			if($currentHaze >= $dayCycleHaze[%i] && $currentHaze <= $dayCycleHaze[%i-1])
			{
				if($currentSky != $dayCycleSky[%i])
				{
					$currentSky = $dayCycleSky[%i];
					ChangeSky($currentSky);
					break;
				}
			}
		}

		$ticker[3] = 0;
	}

	if($ticker[5] >= ($RecalcEconomyDelay) / %seconds)
	{
		//re-evaluate economy
		%list = GetBotIdList();
		for(%i = 0; GetWord(%list, %i) != -1; %i++)
		{
			%id = GetWord(%list, %i);
			%aiName = $BotInfoAiName[%id];

			if($BotInfo[%aiName, SHOP] != "")
			{
				%max = getNumItems();
				for(%z = 0; %z < %max; %z++)
				{
					%checkItem = getItemData(%z);

					%p = $ItemCost[%checkItem];
					%q = $ItemCost[%checkItem] * ($resalePercentage/100);

					%b = $MerchantCounterB[%aiName, %checkItem];
					%s = $MerchantCounterS[%aiName, %checkItem];

					%constantB = 100;
					%constantS = 75;

					%x = round( %p - (%p * (%b/%constantB)) );
					%y = round( %q - (%q * (%s/%constantS)) );

					if(%x < 1) %x = 1;
					if(%y >= %p) %y = %p-1;

					$NewItemBuyCost[%aiName, %checkItem] = %x;
					$NewItemSellCost[%aiName, %checkItem] = %y;

					//reset counter
					$MerchantCounterB[%aiName, %checkItem] = "";
					$MerchantCounterS[%aiName, %checkItem] = "";
				}
			}
		}
	//	messageAll($MsgBeige, "The merchants have revised their prices.");

		$ticker[5] = 0;
	}
	if($ticker[6] >= 12) { //12 ticks = 1 min CCC
		%list = GetPlayerIdList();
		for(%i = 0; GetWord(%list, %i) != -1; %i++) {

			%id = GetWord(%list, %i);
			if($Chocobo[%id] >= 1) {
				for(%Choco = 1; %Choco <= $MaxChocobo; %Choco++) {
				if($Chocobo[%id, %Choco] != "" && $Chocobo[%id, %Choco] != false)
					Chocobo::Timer(%id, %Choco);
				}
			}
		}
		$ticker[6] = 0;
	}

	//Call itself again, %seconds later.
	schedule("RecursiveWorld("@%seconds@");", %seconds);
}

function CheckForProtectedWords(%string) {

	%i = 0;
	//this function checks for words that shouldn't be used in the #if statement due to its extremely powerful nature
	%w[%i++] = "Admin";
	%w[%i++] = "ResetPlayer";
	%w[%i++] = "exec";
	%w[%i++] = "down";
	%w[%i++] = "quit";
	%w[%i++] = "eval";
	%w[%i++] = "function";

	for(%i = 1; %w[%i] != ""; %i++)
	{
		if(String::findSubStr(%string, %w[%i]) != -1)
			return %w[%i];
	}
	return "";
}

function AddBounty(%Client, %amt) {

	Cap($bounty[%Client] += %amt, 0, 65000);

	return $bounty[%Client];
}

function PostSteal(%Client, %success, %type) {

	if(%type == 0) {
		//regular steal
		if(%success) {
			%delay = 5;
			AddBounty(%Client, 10);
		}
		else {
			%delay = 30;
			AddBounty(%Client, 100);
		}
	}

	UpdateBonusState(%clientId, Thief, %delay, "add"); //Bonus updates every 2 secs (so delay * 2)

}

function AllowedToSteal(%Client) {

	if($InSleepZone[%Client] != "")
		return "You can't steal inside a sleeping area.";

	return "True";
}

function PerhapsPlayStealSound(%Client, %Id, %type) {

	if(%type == 0)
		%snd = SoundMoney1;

	%r = getRandom() * 1000;
	%n = 1000 - ( round( (getFinalDEX(%Client) + getFinalLVL(%Client)) * getRandom() ) - (getFinalDEX(%Id) + getFinalLVL(%Id)) );
	if(%r <= %n)
	{
		playSound(%snd, GameBase::getPosition(%Client));
		return True;
	}
	else
		return False;
}

function SimDrunk(%Client, %alvl) {
	%a = %alvl * 0.01;
	if(%alvl >= 20 && getRandom() < %a) {
		Item::setVelocity(%Client, "0 0 5"); //%x@" "@%y@" 6");
		%rot = GameBase::getRotation(%Client);
		if(getRandom() > 0.5)
			%xx = 0.4;
		else
			%xx = -0.4;
		%x =  getWord(%rot,0)-%xx;
		%y =  getWord(%rot,1)-getRandom();
		%z =  getWord(%rot,2)-floor(%alvl/getRandom());
		GameBase::setRotation(%Client, %x@" "@%y@" "@%z);
	}
}

function IsInCommaList(%list, %item)
{
	dbecho($dbechoMode, "IsInCommaList(" @ %list @ ", " @ %item @ ")");

	%a = $sepchar @ %list;
	if(String::findSubStr(%a, "," @ %item @ ",") != -1)
		return True;
	else
		return False;
}
