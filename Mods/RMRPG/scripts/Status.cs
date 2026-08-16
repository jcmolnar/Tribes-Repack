//Deus_ex_Machina
//
//	Status effects

$MaxStatusTime = 60 * 10; //10mins

$StatusLookUpList[0] = "Poison";//Same name in $ClientData[%Client, Poison]
$StatusLookUpList[1] = "Blind";
$StatusLookUpList[2] = "Mute";
$StatusLookUpList[3] = "Petrify";


function Status::Poison(%Client, %SClient, %poisonlvl) {

	%stop = FindStatusPoints(%Client, "Poison", "EquipList");
	if(!%stop) {
		%time = Cap((getFinalLVL(%SClient) + %poisonlvl) - (getFinalLVL(%Client) + floor(getRandom()*25)), 0, $MaxStatusTime);
		if(%time > 0) {
			$ClientData[%Client, Poison] = Cap($ClientData[%Client, Poison]+%time, 0, $MaxStatusTime);
			if(%Client != %SClient) {
				Client::sendMessage(%Client, $MsgRed, Client::getName(%SClient)@" just Poisoned you!");
				Client::sendMessage(%SClient, $MsgRed, "You Poisoned "@Client::getName(%Client)@"!");
			}
			else
				Client::sendMessage(%Client, $MsgRed, "You Poisoned yourself!");
			Player::setDamageFlash(%Client, 0.50);
			UpdateStatusList(%Client, "Poison", "add");

			return true;
		}
	}
	if(%Client != %SClient) {
		Client::sendMessage(%Client, $MsgRed, Client::getName(%SClient)@" just failed to Poison you!");
		Client::sendMessage(%SClient, $MsgRed, "You failed to Poison "@Client::getName(%Client)@"!");
	}
	return false;
}
function Status::Blind(%Client, %SClient, %blindlvl) {

	%stop = FindStatusPoints(%Client, "Blind", "EquipList");
	if(!%stop) {
		%time = Cap((getFinalLVL(%SClient) + %blindlvl) - (getFinalLVL(%Client) + floor(getRandom()*25)), 0, $MaxStatusTime);
		if(%time > 0) {
			$ClientData[%Client, Blind] = Cap($ClientData[%Client, Blind]+%time, 0, $MaxStatusTime);
			if(%Client != %SClient) {
				Client::sendMessage(%Client, $MsgRed, Client::getName(%SClient)@" just Blinded you!");
				Client::sendMessage(%SClient, $MsgRed, "You Blinded "@Client::getName(%Client)@"!");
			}
			else
				Client::sendMessage(%Client, $MsgRed, "You Blinded yourself!");
		//	if(Player::isAiControlled(%Client))
		//		$dumbAIflag[%Client] = True;
			Player::setDamageFlash(%Client, 2);
			UpdateStatusList(%Client, "Blind", "add");
			return true;
		}
	}
	if(%Client != %SClient) {
		Client::sendMessage(%Client, $MsgRed, Client::getName(%SClient)@" just failed to Blind you!");
		Client::sendMessage(%SClient, $MsgRed, "You failed to Blind "@Client::getName(%Client)@"!");
	}
	return false;
}
function Status::Mute(%Client, %SClient, %mutelvl) {

	%stop = FindStatusPoints(%Client, "Mute", "EquipList");
	if(!%stop) {
		%time = Cap((getFinalLVL(%SClient) + %mutelvl) - (getFinalLVL(%Client) + floor(getRandom()*50)), 0, $MaxStatusTime);
		if(%time > 0) {
			$ClientData[%Client, Mute] = Cap($ClientData[%Client, Mute]+%time, 0, $MaxStatusTime);
			if(%Client != %SClient) {
				Client::sendMessage(%Client, $MsgRed, Client::getName(%SClient)@" just Muted you!");
				Client::sendMessage(%SClient, $MsgRed, "You Muted "@Client::getName(%Client)@"!");
			}
			else
				Client::sendMessage(%Client, $MsgRed, "You Muted yourself!");
			Player::setDamageFlash(%Client, 5);
			UpdateStatusList(%Client, "Mute", "add");
			return true;
		}
	}
	if(%Client != %SClient) {
		Client::sendMessage(%Client, $MsgRed, Client::getName(%SClient)@" just failed to Mute you!");
		Client::sendMessage(%SClient, $MsgRed, "You failed to Mute "@Client::getName(%Client)@"!");
	}
	return false;
}

function Status::Petrify(%Client, %SClient, %petrifylvl) {

	%stop = FindStatusPoints(%Client, "Petrify", "EquipList");
	if(!%stop) {
		%time = Cap((getFinalLVL(%SClient) + %petrifylvl) - (getFinalLVL(%Client) + floor(getRandom()*100)), 0, $MaxStatusTime);
		if(%time > 0) {
			$ClientData[%Client, Petrify] = Cap($ClientData[%Client, Petrify]+%time, 0, $MaxStatusTime);
			if(%Client != %SClient) {
				Client::sendMessage(%Client, $MsgRed, Client::getName(%SClient)@" just Petrified you!");
				Client::sendMessage(%SClient, $MsgRed, "You Petrified "@Client::getName(%Client)@"!");
			}
			else
				Client::sendMessage(%Client, $MsgRed, "You Petrified yourself!");

			if(Player::isAiControlled(%Client)) {
				$frozen[%Client] = True;
				AI::setVar($BotInfoAiName[%Client], SpotDist, 0);
				AI::newDirectiveRemove($BotInfoAiName[%Client], 99);
				AI::newDirectiveRemove($BotInfoAiName[%Client], 127);
			}
			else {
				remotePlayMode(%Client);
				Player::setDamageFlash(%Client, 5);
				Client::setControlObject(%Client, -1);
				%Client.guilock = true;
			}
			UpdateStatusList(%Client, "Petrify", "add");
			return true;
		}
	}
	if(%Client != %SClient) {
		Client::sendMessage(%Client, $MsgRed, Client::getName(%SClient)@" just failed to Petrify you!");
		Client::sendMessage(%SClient, $MsgRed, "You failed to Petrify "@Client::getName(%Client)@"!");
	}
	return false;
}

function UpdateStatusList(%Client, %list, %opt) {
	if(%opt == "add")
		$StatusList[%list] = $StatusList[%list]@%Client@" ";
	else if(%opt == "remove") {
		$StatusList[%list] = String::replace($StatusList[%list], %Client@" ", "");
	}
}

function RefreshStatus(%time) { //Update all status victims(every 5 sec)

	for(%i = 0; (%Client = getWord($StatusList[Poison], %i)) != -1; %i++) {
		$ClientData[%Client, Poison] -= 5;
		if($ClientData[%Client, Poison] <= 0) {
			if($ClientData[%Client, Poison] >= -666)
				Client::sendMessage(%Client, 0, "You are no longer Poisoned.");
			$ClientData[%Client, Poison] = "";
			UpdateStatusList(%Client, "Poison", "remove");
		}
		else {
			refreshHP(%Client, Cap(round($ClientData[%Client, Poison]/2), 3, 100));
			Player::setDamageFlash(%Client, 0.1);
		}
	}
	for(%i = 0; (%Client = getWord($StatusList[Blind], %i)) != -1; %i++) {
		$ClientData[%Client, Blind] -= 5;
		if($ClientData[%Client, Blind] <= 0) {
			if($ClientData[%Client, Blind] >= -666)
				Client::sendMessage(%Client, 0, "You are no longer Blinded.");
			$ClientData[%Client, Blind] = "";
			//if(Player::isAiControlled(%Client))
			//	$dumbAIflag[%Client] = "";
			UpdateStatusList(%Client, "Blind", "remove");
		}
		else {
			if(!Player::isAiControlled(%Client)) {
				Player::setDamageFlash(%Client, 5);
				schedule("Player::setDamageFlash("@%Client@", 5);", 2.5);
			}
		}
	}
	for(%i = 0; (%Client = getWord($StatusList[Mute], %i)) != -1; %i++) {
		$ClientData[%Client, Mute] -= 5;
		if($ClientData[%Client, Mute] <= 0) {
			if($ClientData[%Client, Mute] >= -666)
				Client::sendMessage(%Client, 0, "You are no longer Muted.");
			$ClientData[%Client, Mute] = "";
			UpdateStatusList(%Client, "Mute", "remove");
		}
		else
			$ClientData[%Client, Mute] = true;
	}
	for(%i = 0; (%Client = getWord($StatusList[Petrify], %i)) != -1; %i++) {
		$ClientData[%Client, Petrify] -= 5;
		if($ClientData[%Client, Petrify] <= 0) {
			if($ClientData[%Client, Petrify] >= -666)
				Client::sendMessage(%Client, 0, "You are no longer Petrified.");
			$ClientData[%Client, Petrify] = "";
			if(Player::isAiControlled(%Client)) {
				$frozen[%Client] = "";
				AI::SetSpotDist(%Client);
			}
			else {
				%Client.guiLock = "";
				Client::setControlObject(%Client, %Client);
			}
			UpdateStatusList(%Client, "Petrify", "remove");
		}
		else {
			Client::setControlObject(%Client, -1);
			%Client.guilock = true;
		}
	}
	schedule("RefreshStatus("@%time@");", %time);
}
