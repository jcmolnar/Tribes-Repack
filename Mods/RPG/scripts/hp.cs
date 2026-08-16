function setHP(%clientId, %val)
{
	dbecho($dbechoMode, "setHP(" @ %clientId @ ", " @ %val @ ")");

	%armor = Player::getArmor(%clientId);

	if(%val < 0)
		%val = 0;
	if(%val == "")
		%val = fetchData(%clientId, "MaxHP");

	%a = %val * %armor.maxDamage;
	%b = %a / fetchData(%clientId, "MaxHP");
	%c = %armor.maxDamage - %b;

	if(%c < 0)
		%c = 0;
	else if(%c > %armor.maxDamage)
		%c = %armor.maxDamage;

	if(%c == %armor.maxDamage && !IsStillArenaFighting(%clientId))
	{
		storeData(%clientId, "LCK", 1, "dec");

		if(fetchData(%clientId, "LCK") >= 0)
		{
			Client::sendMessage(%clientId, $MsgRed, "You have permanently lost an LCK point!");

			if(fetchData(%clientId, "LCKconsequence") == "miss")
			{
				%c = GameBase::getDamageLevel(Client::getOwnedObject(%clientId));
				%val = -1;
			}
		}
	}

	GameBase::setDamageLevel(Client::getOwnedObject(%clientId), %c);

	return %val;
}
function refreshHP(%clientId, %value)
{
	dbecho($dbechoMode, "refreshHP(" @ %clientId @ ", " @ %value @ ")");

	return setHP(%clientId, fetchData(%clientId, "HP") - round(%value * $TribesDamageToNumericDamage));
}
function refreshHPREGEN(%clientId)
{
	dbecho($dbechoMode, "refreshHPREGEN(" @ %clientId @ ")");

	%a = $PlayerSkill[%clientId, $SkillHealing] / 250000;
	if(%clientId.sleepMode == 1)
		%b = %a + 0.0200;
	else if(%clientId.sleepMode == 2)
		%b = %a;
	else
		%b = %a;

	%c = AddPoints(%clientId, 10) / 2000;

	%r = %b + %c;

	GameBase::setAutoRepairRate(Client::getOwnedObject(%clientId), %r);
}
