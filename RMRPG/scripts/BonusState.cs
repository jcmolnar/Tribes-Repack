//======================================================================
// Bonus States are special bonuses for a certain player that last a
// certain amount of ticks.  A tick is decreased every 2 seconds by
// the zone check.
//======================================================================

$maxBonusStates = 20;

function DecreaseBonusStateTicks(%Client, %b)
{
	if(%b != "")
	{
		//Decrease specified tick for the player
		$BonusStateCnt[%Client, %b]--;

		if($BonusStateCnt[%Client, %b] <= 0)
		{
			if($ClientData[%Client, $BonusState[%Client, %b]] == "started")
				FailedQuest(%Client, $BonusState[%Client, %b]);
			if($BonusState[%Client, %b] == "Alvl")
				schedule("GameBase::setRotation(\""@%Client@"\", \"0 0 0\");", 1);
			$BonusStateCnt[%Client, %b] = "";
			$BonusState[%Client, %b] = "";
			playSound(BonusStateExpire, GameBase::getPosition(%Client));

			Schedule("Game::refreshClientScore("@%Client@");", 1); //Updates in case this player was marked as a PKer.
		}
	}
	else
	{
		%ibcnt = 0;

		//Decrease all ticks for that player
		for(%i = 1; %i <= $maxBonusStates; %i++)
		{
			if($BonusStateCnt[%Client, %i] > 0)
			{
				$BonusStateCnt[%Client, %i]--;

				if($BonusStateCnt[%Client, %i] <= 0)
				{
					if($BonusState[%Client, %i] == PKer)
						Client::sendMessage(%Client, 1, "You are no longer marked as a PKer");
					else if($ClientData[%Client, $BonusState[%Client, %i]] == "started")
						FailedQuest(%Client, $BonusState[%Client, %i]);
					else if($BonusState[%Client, %i] == "Alvl")
						schedule("GameBase::setRotation(\""@%Client@"\", \"0 0 0\");", 1);
					$BonusStateCnt[%Client, %i] = "";
					$BonusState[%Client, %i] = "";

					playSound(BonusStateExpire, GameBase::getPosition(%Client));

					Schedule("Game::refreshClientScore("@%Client@");", 1); //Updates in case this player was marked as a PKer.
				}
				else
					%ibcnt++;
			}
		}

		if(%ibcnt > 0)
			$isBonused[%Client] = true;
		else
			$isBonused[%Client] = "";
	}
}

function AddBonusStatePoints(%Client, %filter)
{
	%add = 0;
	for(%i = 1; %i <= $maxBonusStates; %i++)
	{
		if($BonusStateCnt[%Client, %i] > 0)
		{
			for(%z = 0; (%p1 = GetWord($BonusState[%Client, %i], %z)) != -1; %z+=2)
			{
				%p2 = GetWord($BonusState[%Client, %i], %z+1);
				if(String::ICompare(%p1, %filter) == 0)
				{
					//same filter
					%add += %p2;
				}
			}
		}
	}

	return %add;
}

function CheckBonus(%Client, %type, %type2, %type3) {

	for(%i = 1; %i <= $maxBonusStates; %i++) {

		if($BonusState[%Client, %i] == %type && %type != "")
			return True;
		else if($BonusState[%Client, %i] == %type2 && %type2 != "")
			return True;
		else if($BonusState[%Client, %i] == %type3 && %type3 != "")
			return True;
	}
	return False;
}

function UpdateBonusState(%Client, %type, %ticks, %opt) {

	$isBonused[%Client] = true;

	//look thru the current bonus states and attempt to update
	for(%i = 1; %i <= $maxBonusStates; %i++)
	{
		if($BonusStateCnt[%Client, %i] > 0)
		{
			if(String::ICompare($BonusState[%Client, %i], %type) == 0)
			{
				if(%opt == "")
					$BonusStateCnt[%Client, %i] = %ticks;
				else if(%opt == "add")
					$BonusStateCnt[%Client, %i] += %ticks;

				$BonusState[%Client, %i] = %type;

				return True;
			}
		}
	}

	//couldn't find a current entry to update, so make a new entry
	for(%i = 1; %i <= $maxBonusStates; %i++) {
		if($BonusState[%Client, %i] == "") {

			$BonusState[%Client, %i] = %type;
			$BonusStateCnt[%Client, %i] = %ticks;

			Schedule("Game::refreshClientScore("@%Client@");", 1); //Updates in case this player is marked as a PKer.

			return True;
		}
	}
}