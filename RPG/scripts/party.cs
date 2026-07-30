$maxpartymembers = 4;

function CreateParty(%clientId)
{
	dbecho($dbechoMode, "CreateParty(" @ %clientId @ ")");

	if(fetchData(%clientId, "partyOwned"))
	{
		DisbandParty(%clientId);
	}

	Client::sendMessage(%clientId, $MsgBeige, "You have created a new party.");
	storeData(%clientId, "partyOwned", True);

	AddToParty(%clientId, Client::getName(%clientId));
}

function DisbandParty(%clientId)
{
	dbecho($dbechoMode, "DisbandParty(" @ %clientId @ ")");

	storeData(%clientId, "partyOwned", "");

	%list = fetchData(%clientId, "partylist");
	for(%p = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999))
	{
		%w = String::NEWgetSubStr(%list, 0, %p);
		
		RemoveFromParty(%clientId, %w, True);
	}

	Client::sendMessage(%clientId, $MsgBeige, "Your party has been disbanded.");
}

function RemoveFromParty(%clientId, %name, %optional)
{
	dbecho($dbechoMode, "RemoveFromParty(" @ %clientId @ ", " @ %name @ ", " @ %optional @ ")");

	%id = NEWgetClientByName(%name);

	if(%id != -1)
	{
		if(%clientId != %id)
			Client::sendMessage(%id, $MsgBeige, "You are no longer in " @ Client::getName(%clientId) @ "'s party.");
		else
			Client::sendMessage(%id, $MsgBeige, "You have left your party.");
	}

	storeData(%clientId, "partylist", RemoveFromCommaList(fetchData(%clientId, "partylist"), %name));

	%list = fetchData(%clientId, "partylist");
	for(%p = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999))
	{
		%w = String::NEWgetSubStr(%list, 0, %p);
		%cl = NEWgetClientByName(%w);
		if(%id != %cl && %id != %clientId)
			Client::sendMessage(%cl, $MsgBeige, %name @ " is no longer in your party.");
	}

	if(!%optional)
	{
		if(CountObjInCommaList(fetchData(%clientId, "partylist")) <= 0)
			DisbandParty(%clientId);
	}
}

function AddToParty(%clientId, %name)
{
	dbecho($dbechoMode, "AddToParty(" @ %clientId @ ", " @ %name @ ")");

	%id = NEWgetClientByName(%name);

	if(%id != -1)
	{
		if(%clientId != %id)
			Client::sendMessage(%id, $MsgBeige, "You are now in " @ Client::getName(%clientId) @ "'s party.");
		else
			Client::sendMessage(%id, $MsgBeige, "You have joined your party.");
	}

	storeData(%clientId, "partylist", AddToCommaList(fetchData(%clientId, "partylist"), %name));

	%clientId.invitee[%id] = "";

	%list = fetchData(%clientId, "partylist");
	for(%p = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999))
	{
		%w = String::NEWgetSubStr(%list, 0, %p);
		%cl = NEWgetClientByName(%w);
		if(%id != %cl && %id != %clientId)
			Client::sendMessage(%cl, $MsgBeige, %name @ " has joined your party.");
	}
}

function IsInWhichParty(%name)
{
	dbecho($dbechoMode, "IsInWhichParty(" @ %name @ ", " @ %name @ ")");

	%clientId = NEWgetClientByName(%name);

	for(%id = Client::getFirst(); %id != -1; %id = Client::getNext(%id))
	{
		if(fetchData(%id, "partyOwned"))
		{
			if(IsInCommaList(fetchData(%id, "partylist"), %name))
				return %id;
			else
			{
				if(%id.invitee[%clientId])
					return %id @ " i";
			}
		}
	}

	return -1;
}

function GetPartyListIAmIn(%clientId)
{
	dbecho($dbechoMode, "GetPartyListIAmIn(" @ %clientId @ ")");

	%name = Client::getName(%clientId);

	%p = IsInWhichParty(%name);
	%id = GetWord(%p, 0);
	%inv = GetWord(%p, 1);

	if(%inv == -1)
		return fetchData(%id, "partylist");
}