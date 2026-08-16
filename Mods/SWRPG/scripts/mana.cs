function setMANA(%clientId, %val)
{
	dbecho($dbechoMode, "setMANA(" @ %clientId @ ", " @ %val @ ")");

	%armor = Player::getArmor(%clientId);

	if(%val == "")
		%val = fetchData(%clientId, "MaxMANA");

	%a = %val * %armor.maxEnergy;
	%b = %a / fetchData(%clientId, "MaxMANA");

	if(%b < 0)
		%b = 0;
	else if(%b > %armor.maxEnergy)
		%b = %armor.maxEnergy;

	GameBase::setEnergy(Client::getOwnedObject(%clientId), %b);
}
function refreshMANA(%clientId, %value)
{
	dbecho($dbechoMode, "refreshMANA(" @ %clientId @ ", " @ %value @ ")");

	setMANA(%clientId, (fetchData(%clientId, "MANA") - %value));
}
function refreshMANApct(%clientId, %percent)
{
	dbecho($dbechoMode, "refreshMANA(" @ %clientId @ ", " @ %value @ ")");

	%hasmana = fetchData(%clientId, "MANA");
	%newmana = (%hasmana / 100) * %percent;
	setMANA(%clientId, %newmana);
}
function refreshMANAREGEN(%clientId)
{
	dbecho($dbechoMode, "refreshMANAREGEN(" @ %clientId @ ")");

	%a = ($PlayerSkill[%clientId, $SkillEnergy] / 3250);
	if(%clientId.sleepMode == 1)
		%b = 1.0 + %a;
	else if(%clientId.sleepMode == 2)
		%b = 2.25 + %a;
	else
		%b = %a;

	%c = AddPoints(%clientId, 11) / 800;

	%r = %b + %c;

	GameBase::setRechargeRate(Client::getOwnedObject(%clientId), %r);
}
