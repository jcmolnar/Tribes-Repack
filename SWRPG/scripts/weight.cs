function GetWeight(%clientId)
{
	dbecho($dbechoMode, "GetWeight(" @ %clientId @ ")");

	if(IsDead(%clientId) || !fetchData(%clientId, "HasLoadedAndSpawned") || %clientId.IsInvalid)
		return 0;

	//== HELPS REDUCE LAG WHEN THERE ARE SIMULTANEOUS CALLS ======
	%time = getIntegerTime(true);
	if(%time - %clientId.lastGetWeight <= 1 && fetchData(%clientId, "tmpWeight") != "")
		return fetchData(%clientId, "tmpWeight");
	%clientId.lastGetWeight = %time;
	//============================================================

	$GetWeight::ArmorMod = "";
	%total = 0;

	//add up items
	%max = getNumItems();
	for(%i = 0; %i < %max; %i++)
	{
		%checkItem = getItemData(%i);
		%itemcount = Player::getItemCount(%clientId, %checkItem);

		if(%itemcount)
		{
			%weight = GetAccessoryVar(%checkItem, $Weight);
			if(%weight != "" && %weight != False)
				%total += %weight * %itemcount;

			//Replaces the laggy AddPoints(%clientId, 8) in RefreshWeight (the real lag comes from GetAccessoryList however)
			%specialvar = GetAccessoryVar(%checkItem, $SpecialVar);
			if(GetWord(%specialvar, 0) == 8 && %checkItem.className == Equipped)
				$GetWeight::ArmorMod = GetWord(%specialvar, 1);
		}
	}

	//add up coins
	%total += fetchData(%clientId, "COINS") * $coinweight;

	storeData(%clientId, "tmpWeight", %total);
	return %total;
}

function RefreshWeight(%clientId)
{
	//decho("refresh weight called");
	dbecho($dbechoMode2, "RefreshWeight(" @ %clientId @ ")");

	%player = Client::getOwnedObject(%clientId);

	if(!fetchData(%clientId, "SlowdownHitFlag"))
	{
		%weight = fetchData(%clientId, "Weight");
		
		%changeweightstep = 5;

		//determine the new armor to use
		%newarmor = $ArmorForSpeed[fetchData(%clientId, "RACE"), 0];
		%spill = %weight - fetchData(%clientId, "MaxWeight");

		%num = floor(%spill / %changeweightstep);

		if(%num > 0)
		{
			//overweight, select appropriate armor
			for(%i = -1; %i >= -%num; %i--)
			{
				if($ArmorForSpeed[fetchData(%clientId, "RACE"), %i] != "")
					%newarmor = $ArmorForSpeed[fetchData(%clientId, "RACE"), %i];
				else
					break;
			}
		}
		else
		{
			//when not overweight, the special armor-modifying items come in
			%x = $GetWeight::ArmorMod;
			if(%x > 0)
				%newarmor = $ArmorForSpeed[fetchData(%clientId, "RACE"), %x];
		}
	}
	else
	{
		%newarmor = $ArmorForSpeed[fetchData(%clientId, "RACE"), -5];
	}

	%a = Player::getArmor(%clientId);
	%ae = GameBase::getEnergy(%player);

	if(%a != %newarmor && %newarmor != "")
	{

//		decho("Set to: " @ %newarmor);
		decho("Old: " @ Player::getArmor(%clientId));
		Player::setArmor(%clientId, %newarmor);
		decho("New: " @ Player::getArmor(%clientId));
		schedule("echo(Player::getArmor(" @ %clientId @ "));", 0.1);
//		schedule("Player::setArmor(" @ %clientId @ ", " @ %newarmor @ ");", 0.1); //..@#!@#! WHY DOES IT ONLY SOMETIMES WORK IF I DON'T DELAY IT?!?!
		schedule("echo(Player::getArmor(" @ %clientId @ "));", 0.3);		//Good god. So It will change the armor properly when it's on
											//an overweight one, but otherwise not? Or something? What the
											//freaking heck?! So frustrating. -.-

		GameBase::setEnergy(%player, %ae);

		//UseSkill(%clientId, $SkillWeightCapacity, True, True, 25);
	}

	//save the %num in a global variable for use on stats (in order to give penalties to other stats for being overweight)
	storeData(%clientId, "OverweightStep", %num);
}
