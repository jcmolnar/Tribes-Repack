function ClearVariables(%clientId)
{
	dbecho($dbechoMode2, "ClearVariables(" @ %clientId @ ")");

	%name = Client::getName(%clientId);

	//clear variables

	ClearFunkVar(%name);

	$possessedBy[%clientId] = "";

	//this is only for bots
	$BotFollowDirective[fetchData(%clientId, "BotInfoAiName")] = "";

	//clear directives
	$aidirectiveTable[%clientId, 99] = "";

	%clientId.IsInvalid = "";
	%clientId.currentShop = "";
	%clientId.currentBank = "";
	%clientId.currentSmith = "";
	%clientId.adminLevel = "";
	%clientId.lastWaitActionTime = "";
	%clientId.choosingGroup = "";
	%clientId.choosingClass = "";
	%clientId.possessId = "";
	%clientId.sleepMode = "";
	%clientId.lastSaveCharTime = "";
	%clientId.replyTo = "";
	%clientId.stealType = "";
	%clientId.lastTriggerTime = "";
	%clientId.lastFireTime = "";
	%clientId.lastItemPickupTime = "";
	%clientId.MusicTicksLeft = "";
	%clientId.doExport = "";
	%clientId.TryingToSteal = "";
	%clientId.lastGetWeight = "";
	%clientId.echoOff = "";
	%clientId.lastMissMessage = "";
	%clientId.lastMinePos = "";
	%clientId.bulkNum = "";
	%clientId.zoneLastPos = "";
	%clientId.roll = "";
	%clientId.lbnum = "";
	$numMessage[%clientId, 1] = "";
	$numMessage[%clientId, 2] = "";
	$numMessage[%clientId, 3] = "";
	$numMessage[%clientId, 4] = "";
	$numMessage[%clientId, 5] = "";
	$numMessage[%clientId, 6] = "";
	$numMessage[%clientId, 7] = "";
	$numMessage[%clientId, 8] = "";
	$numMessage[%clientId, 9] = "";
	$numMessage[%clientId, 0] = "";
	$numMessage[%clientId, numpad0] = "";
	$numMessage[%clientId, numpad1] = "";
	$numMessage[%clientId, numpad2] = "";
	$numMessage[%clientId, numpad3] = "";
	$numMessage[%clientId, numpad4] = "";
	$numMessage[%clientId, numpad5] = "";
	$numMessage[%clientId, numpad6] = "";
	$numMessage[%clientId, numpad7] = "";
	$numMessage[%clientId, numpad8] = "";
	$numMessage[%clientId, numpad9] = "";
	$numMessage[%clientId, "numpad/"] = "";
	$numMessage[%clientId, "numpad*"] = "";
	$numMessage[%clientId, "numpad-"] = "";
	$numMessage[%clientId, "numpad+"] = "";
	$numMessage[%clientId, numpadenter] = "";
	$numMessage[%clientId, b] = "";
	$numMessage[%clientId, g] = "";
	$numMessage[%clientId, h] = "";
	$numMessage[%clientId, m] = "";
	$numMessage[%clientId, c] = "";

	for(%i = 0; (%id = GetWord($TownBotList, %i)) != -1; %i++)
	{
		$state[%id, %clientId] = "";
		if($QuestCounter[%name, %id.name] != "")
			$QuestCounter[%name, %id.name] = "";
	}

	for(%i = 1; %i <= $maxDamagedBy; %i++)
		$damagedBy[%name, %i] = "";

	SetAllSkills(%clientId, "");

	ClearEvents(%clientId);

	deleteVariables("BonusState" @ %clientId @ "*");
	deleteVariables("BonusStateCnt" @ %clientId @ "*");

	deleteVariables("ClientData" @ %clientId @ "*");
}
function ClearFunkVar(%name)
{
	dbecho($dbechoMode2, "ClearFunkVar(" @ %name @ ")");

	%method = 1;
	if(%method == 0)
	{
		//clear regular data
		for(%i = 1; %i <= 35; %i++)
		{
			$funk::var["[\"" @ %name @ "\", 0, " @ %i @ "]"] = "";
			$funk::var[%name, 0, %i] = "";
		}
	
		for(%i = 1; $funk::var["[\"" @ %name @ "\", 2, " @ %i @ "]"] != ""; %i++)
			$funk::var["[\"" @ %name @ "\", 2, " @ %i @ "]"] = "";
		for(%i = 1; $funk::var[%name, 2, %i] != ""; %i++)
			$funk::var[%name, 2, %i] = "";
	
		for(%i = 1; $funk::var["[\"" @ %name @ "\", 3, " @ %i @ "]"] != ""; %i++)
			$funk::var["[\"" @ %name @ "\", 3, " @ %i @ "]"] = "";
		for(%i = 1; $funk::var[%name, 3, %i] != ""; %i++)
			$funk::var[%name, 3, %i] = "";
	
		for(%i = 1; $funk::var["[\"" @ %name @ "\", 4, " @ %i @ "]"] != ""; %i++)
			$funk::var["[\"" @ %name @ "\", 4, " @ %i @ "]"] = "";
		for(%i = 1; $funk::var[%name, 4, %i] != ""; %i++)
			$funk::var[%name, 4, %i] = "";
	}
	else
	{
		deleteVariables("funk::var[\"" @ %name @ "\"*");
		deleteVariables("funk::var" @ %name @ "*");
	}

}

function SaveCharacter(%clientId, %silent)
{
	dbecho($dbechoMode2, "SaveCharacter(" @ %clientId @ ")");

	//first pass check
	if(%clientId.isInvalid || !fetchData(%clientId, "HasLoadedAndSpawned"))
		return False;

	//second pass check, will cause 4 line flood if the client is invalid
	//only do this as a "last resort" test.  if the player is detected to be dead, then there shouldn't be a problem
	if(!IsDead(%clientId))
	{
		Player::incItemCount(%clientId, Tool);
		%x = Player::getItemCount(%clientId, Tool);
		Player::decItemCount(%clientId, Tool);
		%y = Player::getItemCount(%clientId, Tool);
		if(%x == %y)
			return False;
	}
	//third check for bots, they won't have passwords.
	if(fetchData(%clientId, "password") == "")
		return false;

	%name = Client::getName(%clientId);

	if(!IsDead(%clientId) && !IsInRoster(%clientId) && !IsInArenaDueler(%clientId))
	{
		storeData(%clientId, "campPos", GameBase::getPosition(%clientId));
		storeData(%clientId, "campRot", GameBase::getRotation(%clientId));
	}

	ClearFunkVar(%name);

	Player::SetItemCount(%clientId, BeltItemTool, 0);

	//the first identifier in the array is the player's name.
	//this is needed because we are using a global array ($funk::var), so if another player
	//attempts to save at the same time, then there won't be $funk::var's being overwritten

	//the second identifier in the array is either 0, 1, or 2
	//0: regular player variable
	//1: weapon/item
	//2: quest counters

	//the third identifier is simply for identifying what we're saving.

	if(!%silent)
		pecho("Saving character " @ %name @ " (" @ %clientId @ ")...");
	$funk::var["[\"" @ %name @ "\", 0, 1]"] = fetchData(%clientId, "RACE");
	$funk::var["[\"" @ %name @ "\", 0, 2]"] = fetchData(%clientId, "EXP");
	$funk::var["[\"" @ %name @ "\", 0, 3]"] = fetchData(%clientId, "campPos");
	$funk::var["[\"" @ %name @ "\", 0, 4]"] = fetchData(%clientId, "COINS");
	$funk::var["[\"" @ %name @ "\", 0, 5]"] = fetchData(%clientId, "isMimic");
	$funk::var["[\"" @ %name @ "\", 0, 6]"] = fetchData(%clientId, "BANK");
	$funk::var["[\"" @ %name @ "\", 0, 7]"] = Client::getName(%clientId);
	$funk::var["[\"" @ %name @ "\", 0, 8]"] = fetchData(%clientId, "grouplist");
	$funk::var["[\"" @ %name @ "\", 0, 9]"] = fetchData(%clientId, "defaultTalk");
	$funk::var["[\"" @ %name @ "\", 0, 10]"] = fetchData(%clientId, "password");
	$funk::var["[\"" @ %name @ "\", 0, 11]"] = fetchData(%clientId, "bounty");
	$funk::var["[\"" @ %name @ "\", 0, 12]"] = fetchData(%clientId, "inArena");
	$funk::var["[\"" @ %name @ "\", 0, 13]"] = fetchData(%clientId, "PlayerInfo");
	$funk::var["[\"" @ %name @ "\", 0, 14]"] = fetchData(%clientId, "deathmsg");
	//15 is done lower
	$funk::var["[\"" @ %name @ "\", 0, 16]"] = fetchData(%clientId, "BankStorage");
	$funk::var["[\"" @ %name @ "\", 0, 17]"] = fetchData(%clientId, "campRot");
	$funk::var["[\"" @ %name @ "\", 0, 18]"] = fetchData(%clientId, "HP");
	$funk::var["[\"" @ %name @ "\", 0, 19]"] = fetchData(%clientId, "MANA");
	$funk::var["[\"" @ %name @ "\", 0, 20]"] = fetchData(%clientId, "LCKconsequence");
	$funk::var["[\"" @ %name @ "\", 0, 21]"] = fetchData(%clientId, "RemortStep");
	$funk::var["[\"" @ %name @ "\", 0, 22]"] = fetchData(%clientId, "LCK");
	$funk::var["[\"" @ %name @ "\", 0, 23]"] = $rpgver;
	$funk::var["[\"" @ %name @ "\", 0, 26]"] = fetchData(%clientId, "GROUP");
	$funk::var["[\"" @ %name @ "\", 0, 27]"] = fetchData(%clientId, "CLASS");
	$funk::var["[\"" @ %name @ "\", 0, 28]"] = fetchData(%clientId, "SPcredits");
	//$funk::var["[\"" @ %name @ "\", 0, 29]"] = "";
	$funk::var["[\"" @ %name @ "\", 0, 30]"] = GetHouseNumber(fetchData(%clientId, "MyHouse"));
	$funk::var["[\"" @ %name @ "\", 0, 31]"] = fetchData(%clientId, "RankPoints");
	$funk::var["[\"" @ %name @ "\", 0, 32]"] = fetchData(%clientId, "traused");


	//Key binds
	$funk::var["[\"" @ %name @ "\", 7, 1]"] = $numMessage[%clientId, 1];
	$funk::var["[\"" @ %name @ "\", 7, 2]"] = $numMessage[%clientId, 2];
	$funk::var["[\"" @ %name @ "\", 7, 3]"] = $numMessage[%clientId, 3];
	$funk::var["[\"" @ %name @ "\", 7, 4]"] = $numMessage[%clientId, 4];
	$funk::var["[\"" @ %name @ "\", 7, 5]"] = $numMessage[%clientId, 5];
	$funk::var["[\"" @ %name @ "\", 7, 6]"] = $numMessage[%clientId, 6];
	$funk::var["[\"" @ %name @ "\", 7, 7]"] = $numMessage[%clientId, 7];
	$funk::var["[\"" @ %name @ "\", 7, 8]"] = $numMessage[%clientId, 8];
	$funk::var["[\"" @ %name @ "\", 7, 9]"] = $numMessage[%clientId, 9];
	$funk::var["[\"" @ %name @ "\", 7, 0]"] = $numMessage[%clientId, 0];
	$funk::var["[\"" @ %name @ "\", 7, b]"] = $numMessage[%clientId, b];
	$funk::var["[\"" @ %name @ "\", 7, g]"] = $numMessage[%clientId, g];
	$funk::var["[\"" @ %name @ "\", 7, h]"] = $numMessage[%clientId, h];
	$funk::var["[\"" @ %name @ "\", 7, m]"] = $numMessage[%clientId, m];
	$funk::var["[\"" @ %name @ "\", 7, n]"] = $numMessage[%clientId, n];
	$funk::var["[\"" @ %name @ "\", 7, c]"] = $numMessage[%clientId, c];
	$funk::var["[\"" @ %name @ "\", 7, q]"] = $numMessage[%clientId, q];
	$funk::var["[\"" @ %name @ "\", 7, numpadenter]"] = $numMessage[%clientId, numpadenter];
	$funk::var["[\"" @ %name @ "\", 7, numpad0]"] = $numMessage[%clientId, numpad0];
	$funk::var["[\"" @ %name @ "\", 7, numpad1]"] = $numMessage[%clientId, numpad1];
	$funk::var["[\"" @ %name @ "\", 7, numpad2]"] = $numMessage[%clientId, numpad2];
	$funk::var["[\"" @ %name @ "\", 7, numpad3]"] = $numMessage[%clientId, numpad3];
	$funk::var["[\"" @ %name @ "\", 7, numpad4]"] = $numMessage[%clientId, numpad4];
	$funk::var["[\"" @ %name @ "\", 7, numpad5]"] = $numMessage[%clientId, numpad5];
	$funk::var["[\"" @ %name @ "\", 7, numpad6]"] = $numMessage[%clientId, numpad6];
	$funk::var["[\"" @ %name @ "\", 7, numpad7]"] = $numMessage[%clientId, numpad7];
	$funk::var["[\"" @ %name @ "\", 7, numpad8]"] = $numMessage[%clientId, numpad8];
	$funk::var["[\"" @ %name @ "\", 7, numpad9]"] = $numMessage[%clientId, numpad9];
	$funk::var["[\"" @ %name @ "\", 7, \"numpad/\"]"] = $numMessage[%clientId, "numpad/"];
	$funk::var["[\"" @ %name @ "\", 7, \"numpad*\"]"] = $numMessage[%clientId, "numpad*"];
	$funk::var["[\"" @ %name @ "\", 7, \"numpad-\"]"] = $numMessage[%clientId, "numpad-"];
	$funk::var["[\"" @ %name @ "\", 7, \"numpad+\"]"] = $numMessage[%clientId, "numpad+"];

	$funk::var["[\"" @ %name @ "\", 8, 1]"] = fetchData(%clientId, "AmmoItems");
	$funk::var["[\"" @ %name @ "\", 8, 2]"] = fetchData(%clientId, "BankAmmoItems");
	$funk::var["[\"" @ %name @ "\", 8, 3]"] = fetchData(%clientId, "GemItems");
	$funk::var["[\"" @ %name @ "\", 8, 4]"] = fetchData(%clientId, "BankGemItems");
	$funk::var["[\"" @ %name @ "\", 8, 5]"] = fetchData(%clientId, "PotionItems");
	$funk::var["[\"" @ %name @ "\", 8, 6]"] = fetchData(%clientId, "BankPotionItems");

	%recallList = fetchData(%clientId,"recallList");
	$funk::var["[\"" @ %name @ "\", 8, recallList]"] = %recallList;


	//skill variables
	%cnt = 0;
	for(%i = 1; %i <= GetNumSkills(); %i++)
	{
		$funk::var["[\"" @ %name @ "\", 4, " @ %cnt++ @ "]"] = $PlayerSkill[%clientId, %i];
		$funk::var["[\"" @ %name @ "\", 4, " @ %cnt++ @ "]"] = $SkillCounter[%clientId, %i];
	}

	//IP dump, for server admin look-up purposes
	$funk::var["[\"" @ %name @ "\", 0, 666]"] = Client::getTransportAddress(%clientId);

	%ii = 0;

	//determine which weapons player has

	if(!IsDead(%clientId))
	{
		%s = "";
		%max = getNumItems();
		for(%i = 0; %i < %max; %i++)
		{
			%checkItem = getItemData(%i);
			%itemcount = Player::getItemCount(%clientId, %checkItem);
			if(%itemcount > $maxItem)
				%itemcount = $maxItem;
			if(%itemcount > 0)
				%s = %s @ %checkItem @ " " @ %itemcount @ " ";
		}
		$funk::var["[\"" @ %name @ "\", 0, 15]"] = %s;
	}
	else
		$funk::var["[\"" @ %name @ "\", 0, 15]"] = fetchData(%clientId, "spawnStuff");

	%cnt = 0;
	%list = GetBotIdList();
	for(%i = 0; GetWord(%list, %i) != -1; %i++)
	{
		%id = GetWord(%list, %i);
		%aiName = fetchData(%id, "BotInfoAiName");

		if($QuestCounter[%name, %aiName] != "")
		{
			%cnt++;
			$funk::var["[\"" @ %name @ "\", 2, " @ %cnt @ "]"] = %aiName;
			$funk::var["[\"" @ %name @ "\", 3, " @ %cnt @ "]"] = $QuestCounter[%name, %aiName];
		}
	}

	//bonus state variables
	for(%i = 1; %i <= $maxBonusStates; %i++)
	{
		$funk::var["[\"" @ %name @ "\", 5, " @ %i @ "]"] = $BonusState[%clientId, %i];
		$funk::var["[\"" @ %name @ "\", 6, " @ %i @ "]"] = $BonusStateCnt[%clientId, %i];
	}
	

	//File::delete("temp\\" @ %name @ ".cs");

	export("funk::var[\"" @ %name @ "\",*", "temp\\" @ %name @ ".cs", false);
	ClearFunkVar(%name);

	Player::SetItemCount(%clientId, BeltItemTool, 1);

	if(!%silent)
		pecho("Save for " @ %name @ " (" @ %clientId @ ") complete.");
	else
		return "complete";

	return True;
}

function LoadCharacter(%clientId)
{
	dbecho($dbechoMode2, "LoadCharacter(" @ %clientId @ ")");

	ClearVariables(%clientId);

	%name = Client::getName(%clientId);
	%filename = %name @ ".cs";

	$ConsoleWorld::DefaultSearchPath = $ConsoleWorld::DefaultSearchPath;	//thanks Presto

	if(isFile("temp\\" @ %filename))
	{
		//load character
		echo("Loading character " @ %name @ " (" @ %clientId @ ")...");

		for(%retry = 0; %retry < 5; %retry++)		//This might not be necessary, but it's to ensure that the
		{								//exec doesn't get flakey when there's lag.
			exec(%filename);
			if($funk::var[%name, 0, 1] != "")
				break;
		}

		storeData(%clientId, "RACE", $funk::var[%name, 0, 1]);
		storeData(%clientId, "EXP", $funk::var[%name, 0, 2]);
		storeData(%clientId, "campPos", $funk::var[%name, 0, 3]);
		storeData(%clientId, "COINS", $funk::var[%name, 0, 4]);
		storeData(%clientId, "isMimic", $funk::var[%name, 0, 5]);
		storeData(%clientId, "BANK", $funk::var[%name, 0, 6]);
		storeData(%clientId, "tmpname", $funk::var[%name, 0, 7]);
		storeData(%clientId, "grouplist", $funk::var[%name, 0, 8]);
		storeData(%clientId, "defaultTalk", $funk::var[%name, 0, 9]);
		storeData(%clientId, "password", string::getSubStr($Client::info[%clientId, 5], 0, 210));
		storeData(%clientId, "bounty", $funk::var[%name, 0, 11]);
		storeData(%clientId, "inArena", $funk::var[%name, 0, 12]);
		storeData(%clientId, "PlayerInfo", $funk::var[%name, 0, 13]);
		storeData(%clientId, "deathmsg", $funk::var[%name, 0, 14]);
		storeData(%clientId, "spawnStuff", $funk::var[%name, 0, 15]);
		storeData(%clientId, "BankStorage", $funk::var[%name, 0, 16]);
		storeData(%clientId, "campRot", $funk::var[%name, 0, 17]);
		storeData(%clientId, "tmphp", $funk::var[%name, 0, 18]);
		storeData(%clientId, "tmpmana", $funk::var[%name, 0, 19]);
		storeData(%clientId, "LCKconsequence", $funk::var[%name, 0, 20]);
		storeData(%clientId, "RemortStep", $funk::var[%name, 0, 21]);
		storeData(%clientId, "LCK", $funk::var[%name, 0, 22]);
		storeData(%clientId, "tmpLastSaveVer", $funk::var[%name, 0, 23]);
		storeData(%clientId, "GROUP", $funk::var[%name, 0, 26]);
		storeData(%clientId, "CLASS", $funk::var[%name, 0, 27]);
		storeData(%clientId, "SPcredits", $funk::var[%name, 0, 28]);
		//$funk::var[%name, 0, 29]);
		storeData(%clientId, "MyHouse", $HouseName[$funk::var[%name, 0, 30]]);
		storeData(%clientId, "RankPoints", $funk::var[%name, 0, 31]);
		storeData(%clientId, "traused", $funk::var[%name, 0, 32]);


		$numMessage[%clientId, 1] = $funk::var[%name, 7, 1];
		$numMessage[%clientId, 2] = $funk::var[%name, 7, 2];
		$numMessage[%clientId, 3] = $funk::var[%name, 7, 3];
		$numMessage[%clientId, 4] = $funk::var[%name, 7, 4];
		$numMessage[%clientId, 5] = $funk::var[%name, 7, 5];
		$numMessage[%clientId, 6] = $funk::var[%name, 7, 6];
		$numMessage[%clientId, 7] = $funk::var[%name, 7, 7];
		$numMessage[%clientId, 8] = $funk::var[%name, 7, 8];
		$numMessage[%clientId, 9] = $funk::var[%name, 7, 9];
		$numMessage[%clientId, 0] = $funk::var[%name, 7, 0];
		$numMessage[%clientId, b] = $funk::var[%name, 7, b];
		$numMessage[%clientId, g] = $funk::var[%name, 7, g];
		$numMessage[%clientId, h] = $funk::var[%name, 7, h];
		$numMessage[%clientId, m] = $funk::var[%name, 7, m];
		$numMessage[%clientId, c] = $funk::var[%name, 7, c];
		$numMessage[%clientId, numpadenter] = $funk::var[%name, 7, numpadenter];
		$numMessage[%clientId, numpad0] = $funk::var[%name, 7, numpad0];
		$numMessage[%clientId, numpad1] = $funk::var[%name, 7, numpad1];
		$numMessage[%clientId, numpad2] = $funk::var[%name, 7, numpad2];
		$numMessage[%clientId, numpad3] = $funk::var[%name, 7, numpad3];
		$numMessage[%clientId, numpad4] = $funk::var[%name, 7, numpad4];
		$numMessage[%clientId, numpad5] = $funk::var[%name, 7, numpad5];
		$numMessage[%clientId, numpad6] = $funk::var[%name, 7, numpad6];
		$numMessage[%clientId, numpad7] = $funk::var[%name, 7, numpad7];
		$numMessage[%clientId, numpad8] = $funk::var[%name, 7, numpad8];
		$numMessage[%clientId, numpad9] = $funk::var[%name, 7, numpad9];
		$numMessage[%clientId, "numpad/"] = $funk::var[%name, 7, "numpad/"];
		$numMessage[%clientId, "numpad*"] = $funk::var[%name, 7, "numpad*"];
		$numMessage[%clientId, "numpad-"] = $funk::var[%name, 7, "numpad-"];
		$numMessage[%clientId, "numpad+"] = $funk::var[%name, 7, "numpad+"];


		storeData(%clientId, "AmmoItems", $funk::var[%name, 8, 1]);
		storeData(%clientId, "BankAmmoItems", $funk::var[%name, 8, 2]);
		storeData(%clientId, "GemItems", $funk::var[%name, 8, 3]);
		storeData(%clientId, "BankGemItems", $funk::var[%name, 8, 4]);
		storeData(%clientId, "PotionItems", $funk::var[%name, 0, 5]);
		storeData(%clientId, "BankPotionItems", $funk::var[%name, 0, 6]);

		%recallList = $funk::var[%name, 8, recallList];
		storeData(%clientId,"recallList", %recallList);

		Belt::BankStorageConversion(%clientid);


		//skill variables
		%cnt = 0;
		for(%i = 1; %i <= GetNumSkills(); %i++)
		{
			$PlayerSkill[%clientId, %i] = $funk::var[%name, 4, %cnt++];
			$SkillCounter[%clientId, %i] = $funk::var[%name, 4, %cnt++];
		}

		for(%i = 1; $funk::var[%name, 3, %i] != ""; %i++)
		{
			$QuestCounter[%name, $funk::var[%name, 2, %i]] = $funk::var[%name, 3, %i];
		}

		//bonus state variables
		for(%i = 1; %i <= $maxBonusStates; %i++)
		{
			$BonusState[%clientId, %i] = $funk::var[%name, 5, %i];
			$BonusStateCnt[%clientId, %i] = $funk::var[%name, 6, %i];
		}

		//== VERSION CONVERSION ROUTINES ============================

		//temp--------
		if(fetchData(%clientId, "RemortStep") == "")
			storeData(%clientId, "RemortStep", 0);
		if(fetchData(%clientId, "LCKconsequence") == "")
			storeData(%clientId, "LCKconsequence", "death");
		if(fetchData(%clientId, "tmphp") == "")
			storeData(%clientId, "tmphp", 1);
		if(fetchData(%clientId, "tmpmana") == "")
			storeData(%clientId, "tmpmana", 1);
		if(fetchData(%clientId, "tmpname") == "")
			storeData(%clientId, "tmpname", %name);
		//------------

		//===========================================================
		
		echo("Load complete.");
	}
	else
	{
		//give defaults
		echo("Giving defaults to new player " @ %clientId);
		storeData(%clientId, "RACE", Client::getGender(%clientId) @ "Human");
		storeData(%clientId, "EXP", 0);
		storeData(%clientId, "campPos", "");
		storeData(%clientId, "BANK", $initbankcoins);
		storeData(%clientId, "grouplist", "");
		storeData(%clientId, "defaultTalk", "#global");
		storeData(%clientId, "password", $Client::info[%clientId, 5]);
		storeData(%clientId, "LCK", $initLCK);
		storeData(%clientId, "PlayerInfo", "");
		storeData(%clientId, "ignoreGlobal", "");
		storeData(%clientId, "LCKconsequence", "death");
		storeData(%clientId, "tmphp", "");
		storeData(%clientId, "tmpmana", "");
		storeData(%clientId, "RemortStep", 0);
		storeData(%clientId, "tmpname", %name);
		storeData(%clientId, "tmpLastSaveVer", $rpgver);
		storeData(%clientId, "bounty", 0);
		storeData(%clientId, "isMimic", "");
		storeData(%clientId, "MyHouse", "");
		storeData(%clientId, "RankPoints", 0);

		%clientId.choosingGroup = True;

		SetAllSkills(%clientId, 0);

		storeData(%clientId, "spawnStuff", "PickAxe 1 HealPotion 1 GreaterHealPotion 3 BeltItemTool 1");
	}

	if(%clientId.repack >= 14)
		remoteeval(%clientId, RepackKeyOverride, 2);

	ClearFunkVar(%name);
}

function OnOrOfflineGive(%name, %award)
{
	dbecho($dbechoMode, "OnOrOfflineGive(" @ %name @ ", " @ %award @ ")");

	%clientId = NEWgetClientByName(%name);
	//messageAll($MsgRed, "DEBUG: %name: " @ %name);
	//messageAll($MsgRed, "DEBUG: %clientId: " @ %clientId);
	//messageAll($MsgRed, "DEBUG: %award: " @ %award);
	if(%clientId != -1)
	{
		//player is in-game, simply store the item in storage
		Client::sendMessage(%clientId, $MsgBeige, "You have received a prize for downloading Theory Of Trance music, check your storage!");
		for(%i = 0; GetWord(%award, %i) != -1; %i+=2)
			storeData(%clientId, "BankStorage", SetStuffString(fetchData(%clientId, "BankStorage"), GetWord(%award, %i), GetWord(%award, %i+1)));
	}
	else
	{
		//player is not in-game. load character file, make changes, then save
		%filename = %name @ ".cs";

		$ConsoleWorld::DefaultSearchPath = $ConsoleWorld::DefaultSearchPath;	//thanks Presto

		if(isFile("temp\\" @ %filename))
		{
			//load character
			ClearFunkVar(%name);
			exec(%filename);

			//pass variables thru, while adding the awarded item
			$funk::var["[\"" @ %name @ "\", 0, 1]"] = $funk::var[%name, 0, 1];
			$funk::var["[\"" @ %name @ "\", 0, 2]"] = $funk::var[%name, 0, 2];
			$funk::var["[\"" @ %name @ "\", 0, 3]"] = $funk::var[%name, 0, 3];
			$funk::var["[\"" @ %name @ "\", 0, 4]"] = $funk::var[%name, 0, 4];
			$funk::var["[\"" @ %name @ "\", 0, 5]"] = $funk::var[%name, 0, 5];
			$funk::var["[\"" @ %name @ "\", 0, 6]"] = $funk::var[%name, 0, 6];
			$funk::var["[\"" @ %name @ "\", 0, 7]"] = $funk::var[%name, 0, 7];
			$funk::var["[\"" @ %name @ "\", 0, 8]"] = $funk::var[%name, 0, 8];
			$funk::var["[\"" @ %name @ "\", 0, 9]"] = $funk::var[%name, 0, 9];
			$funk::var["[\"" @ %name @ "\", 0, 10]"] = $funk::var[%name, 0, 10];
			$funk::var["[\"" @ %name @ "\", 0, 11]"] = $funk::var[%name, 0, 11];
			$funk::var["[\"" @ %name @ "\", 0, 12]"] = $funk::var[%name, 0, 12];
			$funk::var["[\"" @ %name @ "\", 0, 13]"] = $funk::var[%name, 0, 13];
			$funk::var["[\"" @ %name @ "\", 0, 14]"] = $funk::var[%name, 0, 14];
			$funk::var["[\"" @ %name @ "\", 0, 15]"] = $funk::var[%name, 0, 15];
			for(%i = 0; GetWord(%award, %i) != -1; %i+=2)
				$funk::var["[\"" @ %name @ "\", 0, 16]"] = SetStuffString($funk::var[%name, 0, 16], GetWord(%award, %i), GetWord(%award, %i+1));
			$funk::var["[\"" @ %name @ "\", 0, 17]"] = $funk::var[%name, 0, 17];
			$funk::var["[\"" @ %name @ "\", 0, 18]"] = $funk::var[%name, 0, 18];
			$funk::var["[\"" @ %name @ "\", 0, 19]"] = $funk::var[%name, 0, 19];
			$funk::var["[\"" @ %name @ "\", 0, 20]"] = $funk::var[%name, 0, 20];
			$funk::var["[\"" @ %name @ "\", 0, 21]"] = $funk::var[%name, 0, 21];
			$funk::var["[\"" @ %name @ "\", 0, 22]"] = $funk::var[%name, 0, 22];
			$funk::var["[\"" @ %name @ "\", 0, 23]"] = $funk::var[%name, 0, 23];
			$funk::var["[\"" @ %name @ "\", 0, 26]"] = $funk::var[%name, 0, 26];
			$funk::var["[\"" @ %name @ "\", 0, 27]"] = $funk::var[%name, 0, 27];
			$funk::var["[\"" @ %name @ "\", 0, 28]"] = $funk::var[%name, 0, 28];
			//$funk::var["[\"" @ %name @ "\", 0, 29]"] = $funk::var[%name, 0, 29];
			$funk::var["[\"" @ %name @ "\", 0, 30]"] = $funk::var[%name, 0, 30];
			$funk::var["[\"" @ %name @ "\", 0, 31]"] = $funk::var[%name, 0, 31];
			$funk::var["[\"" @ %name @ "\", 0, 666]"] = $funk::var[%name, 0, 666];

			//skills
			%cnt = 0;
			for(%i = 1; %i <= GetNumSkills(); %i++)
			{
				%cnt++;
				$funk::var["[\"" @ %name @ "\", 4, " @ %cnt @ "]"] = $funk::var[%name, 4, %cnt];
				%cnt++;
				$funk::var["[\"" @ %name @ "\", 4, " @ %cnt @ "]"] = $funk::var[%name, 4, %cnt];
			}

			//quests
			for(%i = 1; $funk::var[%name, 2, %i] != ""; %i++)
				$funk::var["[\"" @ %name @ "\", 2, " @ %i @ "]"] = $funk::var[%name, 2, %i];
			for(%i = 1; $funk::var[%name, 3, %i] != ""; %i++)
				$funk::var["[\"" @ %name @ "\", 3, " @ %i @ "]"] = $funk::var[%name, 3, %i];

			//bonus state variables
			for(%i = 1; %i <= $maxBonusStates; %i++)
			{
				$funk::var["[\"" @ %name @ "\", 5, " @ %i @ "]"] = $funk::var[%name, 5, %i];
				$funk::var["[\"" @ %name @ "\", 6, " @ %i @ "]"] = $funk::var[%name, 6, %i];
			}

			//save character
			File::delete("temp\\" @ %name @ ".cs");
			export("funk::var[\"" @ %name @ "\",*", "temp\\" @ %name @ ".cs", false);

			ClearFunkVar(%name);
		}
	}
}


function ResetPlayer(%clientId)
{
	dbecho($dbechoMode2, "ResetPlayer(" @ %clientId @ ")");

	%name = Client::getName(%clientId);
	%filename = %name @ ".cs";

	File::delete("temp\\" @ %filename);

	LoadCharacter(%clientId);

	StartStatSelection(%clientId);
}