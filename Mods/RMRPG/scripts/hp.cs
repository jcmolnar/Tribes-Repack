
function getHP(%clientId) {

	%armor = Player::getArmor(%clientId);

	if(Client::GetName(%clientId)!="")
		%c = %armor.maxDamage - GameBase::getDamageLevel(Client::getOwnedObject(%clientId));
	else
		%c = %armor.maxDamage - GameBase::getDamageLevel(%clientId);
	%a = %c * $MaxHP[%clientId];
	%b = %a / %armor.maxDamage;

	return round(%b);
}

function setHP(%clientId, %val) {

	if(IsDead(%clientId) || !$HasLoadedAndSpawned[%clientId])
		return;
	%armor = Player::getArmor(%clientId);

	if(%val < 0)
		%val = 0;

	%a = %val * %armor.maxDamage;
	%b = %a / $MaxHP[%clientId];
	%c = %armor.maxDamage - %b;

	if(%c < 0)
		%c = 0;
	else if(%c > %armor.maxDamage)
		%c = %armor.maxDamage;

	if(%c == %armor.maxDamage) {
		if(Zone::getType($zone[%clientId]) != "FREEFORALL") {
			$LCK[%clientId]--;

			if(getFinalLCK(%clientId) >= 0) {

				Client::sendMessage(%clientId, $MsgRed, "You have lost an LCK point!");

				if($LCKconsequence[%clientId] == "miss") {

					%c = GameBase::getDamageLevel(Client::getOwnedObject(%clientId));

					%val = -1;
				}
			}
		}
	}


	GameBase::setDamageLevel(Client::getOwnedObject(%clientId), %c);

	return %val;
}
function refreshHP(%clientId, %value) {

	return setHP(%clientId, getHP(%clientId) - round(%value * $TribesDamageToNumericDamage));
}

function refreshHPREGEN(%clientId) {

	if(%clientId.sleepMode == 1)
		%a = 0.0250;
	else
		%a = 0.0000;

	%b = AddPoints(%clientId, 10) / 2000;

	%r = %a + %b;

	if(Client::GetName(%clientId)!="")
		GameBase::setAutoRepairRate(Client::getOwnedObject(%clientId), %r);
	else
		GameBase::setAutoRepairRate(%clientId, %r);
}

function AddHP(%clientId) {

	%a = round(getFinalCON(%clientId) * getFinalLVL(%clientId) / 10);
	%b = AddPoints(%ClientId, 13);
	if(!Player::isAIControlled(%clientId))
		%c = Cap(%a + 10 + %b, 1, 99999);
	else
		%c = Cap(%a + 5 + %b, 1, 9999999);
	$MaxHP[%clientId] = %c;

	return %c;
}
