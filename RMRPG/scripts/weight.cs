
function GetMaxWeight(%Client) {

	%a = WeightAllow(%Client);
//	%b = AddPoints(%Client, 9);

	return %a;
}

function GetWeight(%Client) {

	if(IsDead(%Client) || !$HasLoadedAndSpawned[%Client] || %Client.IsInvalid)
		return 0;

	if($Weight[%Client] < 0)
		$Weight[%Client] = 0;

	return $Weight[%Client]; //$Weight gets updated everytime Client::addItemCount is called (That is the only func that handles item count + or -)
}

function RefreshWeight(%Client) {

	%weight = GetWeight(%Client);

	//echo("Weight carried by " @ %Client @ " is " @ %weight);
	%changeweightstep = 5;

	//determine the new armor to use
	if($ClientData[%Client, Robed]) {
		%race = $RACE[%Client]@"Robed";
		%newarmor = $ArmorForSpeed[%race, 0];
	}
	else {
		%race = $RACE[%Client];
		%newarmor = $ArmorForSpeed[%race, 0];
	}

	%spill = %weight - GetMaxWeight(%Client);

	%num = floor(%spill / %changeweightstep);

	if(%num > 0) {
		//overweight, select appropriate armor
		for(%i = -1; %i >= -%num; %i--) {
			if($ArmorForSpeed[%race, %i] != "")
				%newarmor = $ArmorForSpeed[%race, %i];
		}
	}
	else {
		//when not overweight, the special armor-modifying items come in
		%x = AddPoints(%Client, 8);
		if(%x > 0)
			%newarmor = $ArmorForSpeed[%race, %x];
	}

	%a = Player::getArmor(%Client);
	%ae = GameBase::getEnergy(Client::getOwnedObject(%Client));

	if(%a != %newarmor && %newarmor != "")
	{
		//set the new armor
		Player::setArmor(%Client, %newarmor);
		GameBase::setEnergy(Client::getOwnedObject(%Client), %ae);
	}
}
