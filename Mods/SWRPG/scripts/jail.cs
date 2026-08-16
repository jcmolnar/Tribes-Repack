function GetRandomJailNumber()
{
	dbecho($dbechoMode, "GetRandomJailNumber()");

	%group = nameToID("MissionGroup\\Jail");

	if(%group != -1)
	{
		%cnt = Group::objectCount(%group);
		%n = floor(getRandom() * %cnt) + 1;

		return %n;
	}
	return -1;
}

function GetPositionForJailNumber(%jn)
{
	dbecho($dbechoMode, "GetPositionForJailNumber(" @ %jn @ ")");

	%group = nameToID("MissionGroup\\Jail");

	if(%group != -1)
	{
		%set = nameToId("MissionGroup\\Jail\\" @ %jn);
		if(%set != -1)
			return GameBase::getPosition(%set);
	}
	return -1;
}

function Jail(%clientId, %time, %jn)
{
	dbecho($dbechoMode, "Jail(" @ %clientId @ ", " @ %time @ ", " @ %jn @ ")");

	%pos = GetPositionForJailNumber(%jn);

	UpdateBonusState(%clientId, "Jail 1", floor(%time / 2));

	Item::setVelocity(%clientId, "0 0 0");
	GameBase::setPosition(%clientId, %pos);
	schedule("FellOffMap(" @ %clientId @ ");", %time, %clientId);

	Client::sendMessage(%clientId, $MsgRed, "You have been jailed for " @ %time @ " seconds.");
}

function IsJailed(%clientId)
{
	dbecho($dbechoMode, "IsJailed(" @ %clientId @ ")");

	%b = AddBonusStatePoints(%clientId, "Jail");

	if(%b >= 1)
		return True;
	else
		return False;
}