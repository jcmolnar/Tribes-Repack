//======================================================================
// Bonus States are special bonuses for a certain player that last a
// certain amount of ticks.  A tick is decreased every 2 seconds by
// the zone check.
//======================================================================

$maxBonusStates = 10;

function DecreaseBonusStateTicks(%clientId, %b)
{
	if(%b != "")
	{
		//Decrease specified tick for the player
		$BonusStateCnt[%clientId, %b]--;

		if($BonusStateCnt[%clientId, %b] <= 0)
		{
			$BonusStateCnt[%clientId, %b] = "";
			$BonusState[%clientId, %b] = "";
			playSound(BonusStateExpire, GameBase::getPosition(%clientId));
		}
	}
	else
	{
		%totalbcnt = 0;
		%truebcnt = 0;

		//Decrease all ticks for that player
		for(%i = 1; %i <= $maxBonusStates; %i++)
		{
			if($BonusStateCnt[%clientId, %i] > 0)
			{
				$BonusStateCnt[%clientId, %i]--;

				if($BonusStateCnt[%clientId, %i] <= 0)
				{
					$BonusStateCnt[%clientId, %i] = "";
					$BonusState[%clientId, %i] = "";
					playSound(BonusStateExpire, GameBase::getPosition(%clientId));
				}
				else
				{
					%totalbcnt++;
					if($BonusState[%clientId, %i] != "Jail" && $BonusState[%clientId, %i] != "Theft")
						%truebcnt++;
				}
			}
		}

		if(%truebcnt > 0)
			storeData(%clientId, "isBonused", True);
		else
			storeData(%clientId, "isBonused", "");
			
	}
}

function AddBonusStatePoints(%clientId, %filter)
{
	%add = 0;
	for(%i = 1; %i <= $maxBonusStates; %i++)
	{
		if($BonusStateCnt[%clientId, %i] > 0)
		{
			for(%z = 0; (%p1 = GetWord($BonusState[%clientId, %i], %z)) != -1; %z+=2)
			{
				%p2 = GetWord($BonusState[%clientId, %i], %z+1);
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

function UpdateBonusState(%clientId, %type, %ticks)
{
	//look thru the current bonus states and attempt to update
	%flag = False;
	for(%i = 1; %i <= $maxBonusStates; %i++)
	{
		if($BonusStateCnt[%clientId, %i] > 0)
		{
			if(String::ICompare($BonusState[%clientId, %i], %type) == 0)
			{
				$BonusStateCnt[%clientId, %i] = %ticks;
				%flag = True;
			}
		}
	}

	if(!%flag)
	{
		//couldn't find a current entry to update, so make a new entry
		for(%i = 1; %i <= $maxBonusStates; %i++)
		{
			if( !($BonusStateCnt[%clientId, %i] > 0) )
			{
				$BonusState[%clientId, %i] = %type;
				$BonusStateCnt[%clientId, %i] = %ticks;

				return True;
			}
		}
	}

	return %flag;
}