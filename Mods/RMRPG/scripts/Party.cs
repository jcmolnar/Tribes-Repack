$maxpartymembers = 4;

function CreateParty(%Client) {

	if($ClientData[%Client, partyOwned]) {
		DisbandParty(%Client);
	}

	Client::sendMessage(%Client, $MsgBeige, "You have created a new party.");
	$ClientData[%Client, partyOwned] = True;

	AddToParty(%Client, Client::getName(%Client));
}

function DisbandParty(%Client) {


	$ClientData[%Client, partyOwned] = "";
	%list = $ClientData[%Client, partylist];
	for(%p = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999)) {
		%w = String::NEWgetSubStr(%list, 0, %p);
		RemoveFromParty(%Client, %w, True);
	}

	Client::sendMessage(%Client, $MsgBeige, "Your party has been disbanded.");
}

function RemoveFromParty(%Client, %name, %optional) {

	%id = NEWgetClientByName(%name);
	if(%id != -1) {
		if(%Client != %id)
			Client::sendMessage(%id, $MsgBeige, "You are no longer in "@Client::getName(%Client)@"'s party.");
		else
			Client::sendMessage(%id, $MsgBeige, "You have left your party.");
	}

	$ClientData[%Client, partylist] = RemoveFromCommaList($ClientData[%Client, partylist], %name);

	%list = $ClientData[%Client, partylist];
	for(%p = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999)) {
		%w = String::NEWgetSubStr(%list, 0, %p);
		%cl = NEWgetClientByName(%w);
		if(%id != %cl && %id != %Client)
			Client::sendMessage(%cl, $MsgBeige, %name@" is no longer in your party.");
	}

	if(!%optional) {
		if(CountObjInCommaList($ClientData[%Client, partylist]) <= 0)
			DisbandParty(%Client);
	}
}

function AddToParty(%Client, %name) {
	%id = NEWgetClientByName(%name);

	if(%id != -1) {
		if(%Client != %id)
			Client::sendMessage(%id, $MsgBeige, "You are now in "@Client::getName(%Client)@"'s party.");
		else
			Client::sendMessage(%id, $MsgBeige, "You have joined your party.");
	}

	$ClientData[%Client, partylist] = AddToCommaList($ClientData[%Client, partylist], %name);

	%Client.invitee[%id] = "";
	%list = $ClientData[%Client, partylist];
	for(%p = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999)) {
		%w = String::NEWgetSubStr(%list, 0, %p);
		%cl = NEWgetClientByName(%w);
		if(%id != %cl && %id != %Client)
			Client::sendMessage(%cl, $MsgBeige, %name@" has joined your party.");
	}
}

function IsInWhichParty(%name) {

	%Client = NEWgetClientByName(%name);
	for(%id = Client::getFirst(); %id != -1; %id = Client::getNext(%id)) {
		if($ClientData[%id, partyOwned]) {
			if(IsInCommaList($ClientData[%id, partylist], %name))
				return %id;
			else {
				if(%id.invitee[%Client])
					return %id@" i";
			}
		}
	}
	return -1;
}

function GetPartyListIAmIn(%Client){

	%name = Client::getName(%Client);

	%p = IsInWhichParty(%name);
	%id = GetWord(%p, 0);
	%inv = GetWord(%p, 1);

	if(%inv == -1)
		return $ClientData[%id, partylist];
}