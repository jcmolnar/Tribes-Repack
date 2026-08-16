$numArenaPlayers = 2;

$ArenaSpawnIndexes = "44";

$teleportInArenaCost = 5;

$maxroster = 2;
$ArenaPickDuelersTime = 20;
$ArenaStartTime = 30;
$DoCheckMatchWin = False;

$ArenaBotMatchLengthInTicks = 36;
$ArenaBotMatchTicker = 0;

$ArenaEquipment = "Hatchet 1 Knife 1 Club 1 ";

//first part = chance, second part = prize
$tmp="";
$ArenaPrize[$tmp++] = "50000 PaddedArmor 1";
$ArenaPrize[$tmp++] = "20000 COINS 250";
$ArenaPrize[$tmp++] = "5000 COINS 500";
$ArenaPrize[$tmp++] = "500 COINS 1000";
$ArenaPrize[$tmp++] = "250 CheetaursPaws 1";
$ArenaPrize[$tmp++] = "50 COINS 5000";

function InitArena()
{
	dbecho($dbechoMode, "InitArena()");

	ClearRoster();
	ClearArenaDueler();

	//search for an arena

	%group = nameToID("MissionGroup\\TheArena");

	if(%group != -1)
	{
		ScheduleArenaMatch();

		StringArenaTextBox("The RPG Arena has started.  Welcome.");
	}
}

function ScheduleArenaMatch()
{
	dbecho($dbechoMode, "ScheduleArenaMatch()");

	if(HumansInArena())
	{
		FillWaitingRoom($ArenaSpawnIndexes);
		schedule("CreateArenaDueler(" @ $numArenaPlayers @ ");", $ArenaPickDuelersTime);
		schedule("StartArenaMatch();", $ArenaStartTime);
	}
	else
		schedule("ScheduleArenaMatch();", $ArenaStartTime);
}
function StartArenaMatch()
{
	dbecho($dbechoMode, "StartArenaMatch()");

	StringArenaTextBox("The duel has started!");

	%z = 1;
	for(%i = 1; %i <= $maxroster; %i++)
	{
		if($ArenaDueler[%i] != "")
		{
			if(%z == 1)
				%team = 0;
			else if(%z == -1)
				%team = 1;
			%z = -%z;

			%id = GetWord($ArenaDueler[%i], 0);

			GameBase::setTeam(%id, %team);
			GiveArenaEquipment(%id);

			//this way the fighter starts with a weapon in-hand
			if(Player::isAiControlled(%id))
				AI::SelectBestWeapon(%id);
			else
				RemoteNextWeapon(%id);

			TeleportToMarker(%id, "TheArena\\ArenaSpawnMarkers", 1, 0);
			Client::sendMessage(%id, 1, "The duel has started!");
			CloseArenaTextBox(%id);
		}
	}
	$DoCheckMatchWin = True;
}
function GiveArenaEquipment(%clientId)
{
	dbecho($dbechoMode, "GiveArenaEquipment(" @ %clientId @ ")");

	//give the client the arena equipment
	GiveThisStuff(%clientId, $ArenaEquipment, False);

	//for(%a = 1; $ArenaEquipment[%a] != ""; %a++)
	//	Player::setItemCount(%clientId, GetWord($ArenaEquipment[%a], 0), GetWord($ArenaEquipment[%a], 1));
	//RefreshAll(%clientId);
}

function CreateArenaDueler(%num)
{
	dbecho($dbechoMode, "CreateArenaDueler(" @ %num @ ")");

	//pick %num players, with priority to the client's at the beginning of the list
	%pick = 0;
	%playerFlag = False;
	while(%picked < %num)
	{
		%pick++;
		if(%pick > $maxroster)
		{
			//not enough players in roster to satisfy %num
			return -1;
		}
		if($ArenaRoster[%pick] != "")
		{
			%picked++;
			$ArenaDueler[%picked] = $ArenaRoster[%pick] @ " ALIVE";
			$ArenaRoster[%pick] = "";

			if(!Player::isAiControlled(GetWord($ArenaDueler[%picked], 0)))
			{
				//there is a human player, so don't start the counter for a limited match time.
				%playerFlag = True;
			}
		}
	}

	//fix bot's levels to match the average of the human players
	%cnt = 0;
	for(%i = 1; $ArenaDueler[%i] != ""; %i++)
	{
		%id = GetWord($ArenaDueler[%i], 0);
		if(!Player::isAiControlled(%id))
		{
			%cnt++;
			%l += fetchData(%id, "LVL");
		}
	}
	if(%cnt > 0)
	{
		%nlvl = round(%l / %cnt);
		%rr = round(%nlvl / 3);
		%nrr = round(getRandom() * (%rr * 2.5));
		%avg = Cap(%nlvl + (getRandom() * %rr) - %nrr, 1, "inf");
		for(%i = 1; $ArenaDueler[%i] != ""; %i++)
		{
			%id = GetWord($ArenaDueler[%i], 0);
			if(Player::isAiControlled(%id))
			{
				storeData(%id, "EXP", GetExp(%avg, %id));
				Game::refreshClientScore(%id);
				HardcodeAIskills(%id);

				setHP(%id, fetchData(%id, "MaxHP"));
			}
		}
	}

	if(!%playerFlag)
		$IsABotMatch = True;
	else
		$IsABotMatch = False;

	//rearrange the $ArenaRoster array (i don't like the current method i'm using btw...)
	%parseagain = 1;
	while(%parseagain == 1)
	{
		%q = 0;
		%parseagain = "";
		for(%i = $maxroster; %i >= 1; %i--)
		{
			if($ArenaRoster[%i] != "" && %q == 0)
				%q = 1;
			if($ArenaRoster[%i] == "" && %q == 1)
				%parseagain = 1;
		}

		if(%parseagain == 1)
		{
			for(%i = 1; %i <= $maxroster-1; %i++)
			{
				if($ArenaRoster[%i] == "")
				{
					$ArenaRoster[%i] = $ArenaRoster[%i+1];
					$ArenaRoster[%i+1] = "";
				}
			}
		}
	}
	//notify duelers that they will be fighting soon
	for(%i = 1; $ArenaDueler[%i] != ""; %i++)
	{
		Client::sendMessage(GetWord($ArenaDueler[%i], 0), 1, "You have been selected to compete in a duel.  Get ready!");
		StringArenaTextBox(Client::getName(GetWord($ArenaDueler[%i], 0)) @ " has been selected to compete.");
	}
}


//roster functions for arena
function AddToRoster(%clientId)
{
	dbecho($dbechoMode, "AddToRoster(" @ %clientId @ ")");

	for(%i = 1; %i <= $maxroster; %i++)
	{
		if(Player::isAiControlled($ArenaRoster[%i]) && !Player::isAiControlled(%clientId))
		{
			//if there's a bot in the roster, make way for a player trying to enter
			storeData($ArenaRoster[%i], "noDropLootbagFlag", True);
			playNextAnim($ArenaRoster[%i]);
			Player::Kill($ArenaRoster[%i]);

			$ArenaRoster[%i] = "";
		}
		if($ArenaRoster[%i] == "")
		{
			$ArenaRoster[%i] = %clientId;

			//set to the common team 0 (all players in roster go to team 0)
			GameBase::setTeam(%clientId, 0);
			setHP(%clientId, fetchData(%clientId, "MaxHP"));
			setMANA(%clientId, fetchData(%clientId, "MaxMANA"));

			//StringArenaTextBox(Client::getName(%clientId) @ " was added to the arena roster.");

			CreateArenaStorage(%clientId);

			return %i;
		}
	}
	return -1;
}
function CreateArenaStorage(%clientId)
{
	dbecho($dbechoMode, "CreateArenaStorage(" @ %clientId @ ")");

	//remember this client's equipment.
	//all he can keep is his armor
	//and take all the rest out
	//-- NEW METHOD: dump all the player's stuff into his bank storage

	for(%i = 1; $ArenaStorage[%clientId, %i] != ""; %i++)
		$ArenaStorage[%clientId, %i] = "";
	%max = getNumItems();
	for(%i = 0; %i < %max; %i++)
	{
		%checkItem = getItemData(%i);
		%checkItemCount = Player::getItemCount(%clientId, %checkItem);
		if(%checkItemCount && %checkItem.className != Armor)
		{
			%ii++;

			%b = %checkItem;
			if(%b.className == "Equipped")
				%b = String::getSubStr(%b, 0, String::len(%b)-1);
			
			storeData(%clientId, "BankStorage", SetStuffString(fetchData(%clientId, "BankStorage"), %b, %checkItemCount));
			$ArenaStorage[%clientId, %ii] = %b @ " " @ %checkItemCount;
			Player::setItemCount(%clientId, %checkItem, 0);
		}
	}

	RefreshAll(%clientId);
}
function RemoveFromRoster(%clientId)
{
	dbecho($dbechoMode, "RemoveFromRoster(" @ %clientId @ ")");

	for(%i = 1; %i <= $maxroster; %i++)
	{
		if($ArenaRoster[%i] == %clientId)
		{
			$ArenaRoster[%i] = "";

			return %i;
		}
	}
	return -1;
}
function IsInRoster(%clientId)
{
	dbecho($dbechoMode, "IsInRoster(" @ %clientId @ ")");

	for(%i = 1; %i <= $maxroster; %i++)
	{
		if($ArenaRoster[%i] == %clientId)
		{
			return True;
		}
	}
	return False;
}

function ClearRoster()
{
	dbecho($dbechoMode, "ClearRoster()");

	for(%i = 1; %i <= $maxroster; %i++)
		$ArenaRoster[%i] = "";
}
function ClearArenaDueler()
{
	dbecho($dbechoMode, "ClearArenaDueler");

	for(%i = 1; %i <= $maxroster; %i++)
		$ArenaDueler[%i] = "";
}
function GetArenaDuelerIndex(%clientId)
{
	dbecho($dbechoMode, "GetArenaDuelerIndex(" @ %clientId @ ")");

	for(%i = 1; %i <= $maxroster; %i++)
	{
		if(GetWord($ArenaDueler[%i], 0) == %clientId)
		{
			return %i;
		}
	}
	return False;
}
function IsInArenaDueler(%clientId)
{
	dbecho($dbechoMode, "IsInArenaDueler(" @ %clientId @ ")");

	for(%i=1; %i <= $maxroster; %i++)
	{
		if(GetWord($ArenaDueler[%i], 0) == %clientId)
		{
			return True;
		}
	}
	return False;
}
function IsStillArenaFighting(%clientId)
{
	dbecho($dbechoMode, "IsStillArenaFighting(" @ %clientId @ ")");

	for(%i = 1; %i <= $maxroster; %i++)
	{
		%c = GetWord($ArenaDueler[%i], 0);
		%s = GetWord($ArenaDueler[%i], 1);
		if(%s == "ALIVE" && %c == %clientId)
			return True;
	}
	return False;
}
function RemoveFromArenaDueler(%clientId)
{
	dbecho($dbechoMode, "RemoveFromArenaDueler(" @ %clientId @ ")");

	for(%i=1; %i <= $maxroster; %i++)
	{
		if(GetWord($ArenaDueler[%i], 0) == %clientId)
		{
			$ArenaDueler[%i] = "";
			return %i;
		}
	}
	return -1;
}
function RemoveArenaEquipment(%clientId)
{
	dbecho($dbechoMode, "RemoveArenaEquipment(" @ %clientId @ ")");

	TakeThisStuff(%clientId, $ArenaEquipment);
	//remove the arenaequipment that the player was given
	//for(%i = 1; $ArenaEquipment[%i] != ""; %i++)
	//{
	//	if(Player::getItemCount(%clientId, GetWord($ArenaEquipment[%i], 0)))
	//		Player::setItemCount(%clientId, GetWord($ArenaEquipment[%i], 0), 0);
	//}

	//RefreshAll(%clientId);
}
function RestoreArenaStorage(%clientId)
{
	dbecho($dbechoMode, "RestoreArenaStorage(" @ %clientId @ ")");

	//restore this client's old equipment
	for(%i = 1; $ArenaStorage[%clientId, %i] != ""; %i++)
	{
		%item = GetWord($ArenaStorage[%clientId, %i], 0);
		%n = GetWord($ArenaStorage[%clientId, %i], 1);

		Item::giveItem(%clientId, %item, %n, False);
		storeData(%clientId, "BankStorage", SetStuffString(fetchData(%clientId, "BankStorage"), %item, -%n));
	}

	RefreshAll(%clientId);

	for(%i = 1; $ArenaStorage[%clientId, %i] != ""; %i++)
		$ArenaStorage[%clientId, %i] = "";
}
function CheckMatchWin()
{
	dbecho($dbechoMode, "CheckMatchWin()");

	%totalLiveTeams = 0;
	for(%i = 0; %i <= getNumTeams(); %i++)
		%teamCount[%i] = 0;	//just so it's not blank

	for(%i = 1; %i <= $maxroster; %i++)
	{
		%c = GetWord($ArenaDueler[%i], 0);
		%s = GetWord($ArenaDueler[%i], 1);
		if(%s == "ALIVE")
			%teamCount[GameBase::getTeam(%c)]++;
	}
	for(%i = 0; %teamCount[%i] != ""; %i++)
	{
		if(%teamCount[%i] > 0) %totalLiveTeams++;
	}
	//echo("%teamCount[0]: " @ %teamCount[0]);
	//echo("%teamCount[1]: " @ %teamCount[1]);
	//echo("Found " @ %totalLiveTeams @ " total live teams.");
	if(%totalLiveTeams == 1)
	{
		//determine prize.  prize is given to everyone on the team, no splitting.
		%prize = DetermineArenaPrize();

		//match is won
		for(%i = 0; %teamCount[%i] != ""; %i++)
		{
			if(%teamCount[%i] > 0)
			{
				//winners are on team %i
				for(%ii = 1; %ii <= $maxroster; %ii++)
				{
					%c = GetWord($ArenaDueler[%ii], 0);
					%s = GetWord($ArenaDueler[%ii], 1);

					if(%c != "")
					{
						if(GameBase::getTeam(%c) == %i && !IsDead(%c) && %s == "ALIVE")
						{
							//these are the winning players (%c)
							StringArenaTextBox(Client::getName(%c) @ " is victorious!");
							//echo(Client::getName(%c) @ " is victorious!");

							Client::sendMessage(%c, $MsgBeige, "You won a prize!");

							//award prize to player
							GiveThisStuff(%c, %prize, True);
							RefreshAll(%c);
						}
						//remove all remaining players from ArenaDueler and
						//send them back to the arena lobby
						if(%s == "ALIVE")
						{
							RestorePreviousEquipment(%c);
							ReturnToArenaLobby(%c);
						}
					}
				}
			}
		}
		return True;
	}
	else if(%totalLiveTeams == 0)
	{
		//match is a tie
		//simply schedule the next match, since the arena is clear.
		StringArenaTextBox("The match was a tie.");
		//echo("The match was a tie.");

		return True;
	}
	return False;
}
function ReturnToArenaLobby(%c)
{
	dbecho($dbechoMode, "ReturnToArenaLobby(" @ %c @ ")");

	if(Player::isAiControlled(%c))
	{
		storeData(%c, "noDropLootbagFlag", True);
		playNextAnim(%c);
		Player::Kill(%c);
	}
	else
	{
		UpdateTeam(%c);
		RefreshAll(%c);
		TeleportToMarker(%c, "TheArena\\TeleportEntranceMarkers", 0, 1);
		RefreshArenaTextBox(%c);
	}
}
function FillWaitingRoom(%indexes)
{
	dbecho($dbechoMode, "FillWaitingRoom(" @ %indexes @ ")");

	%group = nameToID("MissionGroup\\TheArena\\WaitingRoomMarkers");

	if(%group != -1)
	{
		for(%i = 1; %i <= $maxroster; %i++)
		{
			if($ArenaRoster[%i] == "")
			{
				//the WaitingRoomMarkers must contain at least as many markers as $maxroster
			      %marker = Group::getObject(%group, (%i-1));

				for(%z = 0; GetWord(%indexes, %z) != -1; %z++){}
				%r = floor(getRandom() * %z);

				//extra precautions
				if(%r < 0) %r = 0;
				if(%r > (%z-1)) %r = (%z-1);

				%index = GetWord(%indexes, %r);

				//schedule("SpawnArenaBot(" @ %index @ ", " @ %i @ ", " @ %marker @ ");", %i * 2);
				%AIname = AI::helper($spawnIndex[%index], "ArenaGladiator" @ %i, "MarkerSpawn " @ %marker);
				%aiId = AI::getId(%AIname);

				AddToRoster(%aiId);
			}
		}
		return 0;
	}
	return -1;
}
function SpawnArenaBot(%index, %i, %marker)
{
	dbecho($dbechoMode, "SpawnArenaBot(" @ %index @ ", " @ %i @ ", " @ %clientId @ ")");

	%AIname = AI::helper($spawnIndex[%index], "ArenaGladiator" @ %i, "MarkerSpawn " @ %marker);
	%aiId = AI::getId(%AIname);

	AddToRoster(%aiId);
}

function DetermineArenaPrize()
{
	dbecho($dbechoMode, "DetermineArenaPrize()");

	//1st step, count the total chances
	for(%i = 1; $ArenaPrize[%i] != ""; %i++)
		%total += GetWord($ArenaPrize[%i], 0);

	//2nd step, pick a random number between 1 and %total
	%num = $PrizeSeed;
	if($PrizeSeed == "")
		%num = floor(getRandom() * %total)+1;

	//3rd step
	%cnt=0;
	for(%i = 1; $ArenaPrize[%i] != ""; %i++)
	{
		%cnt += GetWord($ArenaPrize[%i], 0);

		if(%cnt > %num)
			return String::getSubStr($ArenaPrize[%i], String::len(GetWord($ArenaPrize[%i], 0)) + 1, 99999);
	}
}

function RestorePreviousEquipment(%c)
{
	dbecho($dbechoMode, "RestorePreviousEquipment(" @ %c @ ")");

	if(!Player::isAiControlled(%c) && !IsDead(%c))
	{
		RemoveArenaEquipment(%c);
		RestoreArenaStorage(%c);
	}
}

$ArenaTextBoxLines = 6;
function RefreshArenaTextBox(%clientId)
{
	dbecho($dbechoMode, "RefreshArenaTextBox(" @ %clientId @ ")");

	%final = "";
	for(%i = 1; %i <= $ArenaTextBoxLines; %i++)
	{
		%final = %final @ $ArenaTextBox[%i] @ "\n";
	}
	bottomprint(%clientId, %final, -1);
}
function StringArenaTextBox(%text)
{
	dbecho($dbechoMode, "StringArenaTextBox(" @ %text @ ")");

	for(%i = 2; %i <= $ArenaTextBoxLines; %i++)
	{
		$ArenaTextBox[%i-1] = $ArenaTextBox[%i];
	}
	$ArenaTextBox[$ArenaTextBoxLines] = %text;

	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(fetchData(%cl, "inArena"))
			RefreshArenaTextBox(%cl);
	}
}
function CloseArenaTextBox(%clientId)
{
	dbecho($dbechoMode, "CloseArenaTextBox(" @ %clientId @ ")");

	bottomprint(%clientId, "", -1);
}

function CheckAndBootFromArena(%clientId)
{
	dbecho($dbechoMode, "CheckAndBootFromArena(" @ %clientId @ ")");

	storeData(%clientId, "inArena", "");
	CloseArenaTextBox(%clientId);

	if(IsInRoster(%clientId))
	{
		RestorePreviousEquipment(%clientId);
           	RemoveFromRoster(%clientId);
	}
	if(IsInArenaDueler(%clientId))
	{
		RestorePreviousEquipment(%clientId);
            RemoveFromArenaDueler(%clientId);
	}

	if(!Player::isAiControlled(%clientId) && GameBase::getTeam(%clientId) == 1)
		GameBase::setTeam(%clientId, 0);
}

function HumansInArena()
{
	%list = GetPlayerIdList();
	for(%i = 0; GetWord(%list, %i) != -1; %i++)
	{
		%id = GetWord(%list, %i);
		if(fetchData(%id, "inArena"))
			return True;
	}
	return False;
}

function sr()
{
	echo("-----------------------------------------");
	for(%i = 1; %i <= $maxroster; %i++)
		echo("$ArenaRoster[" @ %i @ "]: " @ $ArenaRoster[%i]);
	echo("-----------------------------------------");
}
function sd()
{
	echo("-----------------------------------------");
	for(%i = 1; %i <= $maxroster; %i++)
		echo("$ArenaDueler[" @ %i @ "]: " @ $ArenaDueler[%i]);
	echo("-----------------------------------------");
}