function getMANA(%Client) {

	%armor = Player::getArmor(%Client);

	%a = GameBase::getEnergy(Client::getOwnedObject(%Client)) * getMaxMANA(%Client);
	%b = %a / %armor.maxEnergy;

	return round(%b);
}

function setMANA(%Client, %val) {
	%armor = Player::getArmor(%Client);

	%a = %val * %armor.maxEnergy;
	%b = %a / getMaxMANA(%Client);

	if(%b < 0)
		%b = 0;
	else if(%b > %armor.maxEnergy)
		%b = %armor.maxEnergy;

	GameBase::setEnergy(Client::getOwnedObject(%Client), %b);
}

function refreshMANA(%Client, %value) {
	setMANA(%Client, (getMANA(%Client) - %value));
}

function getMaxMANA(%Client) {

	%a = round( getFinalINT(%Client) * 6 / 3); //5 / 3);
	%b = AddPoints(%Client, 14);
	%b = Cap(%a + %b, 1, 999);

	return %b;
}

function setMaxMANA(%Client) {

	%a = round( getFinalINT(%Client) * 6 / 3);
	%b = AddPoints(%Client, 14);
	%b = Cap(%a + %b, 1, 999);

	$MaxMana[%Client] = %b;
}

function refreshMANAREGEN(%Client) {

	if(%Client.sleepMode != "")
		%a = 1 + (getFinalINT(%Client) / 75); //50
	else
		%a = (getFinalINT(%Client) / 2000);

	%b = AddPoints(%Client, 11) / 950;
	%r = %a + %b;

	GameBase::setRechargeRate(Client::getOwnedObject(%Client), %r);
}
