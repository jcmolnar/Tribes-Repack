function remoteTrue()
{
	//for some reason this function gets called from key binds, so i created it so the console doesn't get flooded with
	//remoteTrue: unknown commands.
	return;
}

function remoteFalse() { return; }

function remoteQuestChat(%Client, %bool) {

	if(!$ClientData[%Client, BotTalking])
		return;

	$ClientData[%Client, BotTalking] = "";
	%Client.guilock = "";

	if($QuestEvalStr[%Client] != "") {
		Eval($QuestEvalStr[%Client]);
		$QuestEvalStr[%Client] = "";
	}
	if(%bool == true) {
		Quest(%Client, %type, false);
	}
	else if(String::findSubStr($RM::KeyWords, %bool@" ") != -1) {
		BotChatStuff(%Client, $ClientData[%Client, BotId], "#say "@%bool, %bool);
	}
	else if(%bool == "NULL")
		%Client.guiLock = "";
}

function remotePlayMode(%Client) {

	%Client.currentShop = "";
	%Client.currentBank = "";
	%Client.currentLoot = "";
	%Client.currentSmith = "";

	$ClientData[%Client, Looting] = "";
	$ClientData[%Client, SmithStage] = "";

	//if(!%Client.guiLock) {
		remoteSCOM(%Client, -1);
		Client::setGuiMode(%Client, $GuiModePlay);
	//}
}

function remoteCommandMode(%Client) { return; }

function remoteToggleInventoryMode(%Client) {

	%Client.currentShop = "";
	%Client.currentBank = "";
	%Client.currentLoot = "";
	%Client.currentSmith = "";

	Client::cancelMenu(%Client);

	if(Client::getGuiMode(%Client) != $GuiModeInventory)
		schedule("remoteInventoryMode("@%Client@");", 0.1);
	else
		remotePlayMode(%Client);
}

function remoteInventoryMode(%Client) {
//	if(!%Client.guiLock && -- client shouldn't be locked in inventory.. for RPG
	if(!Observer::isObserver(%Client)) {
		remoteSCOM(%Client, -1);
		Client::setGuiMode(%Client, $GuiModeInventory);
		%txt = "<f1>GIL: "@$COINS[%Client];
		Client::setInventoryText(%Client, %txt);
	}
}

function Client::setInventoryText(%Client, %txt) {
	remoteEval(%Client, "ITXT", %txt);
}

function remoteObjectivesMode(%Client) {

	%Client.currentShop = "";
	%Client.currentBank = "";
	%Client.currentLoot = "";
	$ClientData[%Client, Looting] = "";

	//remoteSCOM(%Client, -1);
	//Client::setGuiMode(%client, $GuiModeObjectives);

}

function remoteScoresOn(%Client) {
	if(!%Client.menuMode) {
		Game::menuRequest(%Client);
		remotePlayMode(%Client);
	}
}

function remoteScoresOff(%Client) {
	Client::cancelMenu(%Client);
	remotePlayMode(%Client);
}

function CheckMountWeapon(%Client, %item) {

	if(%item == $ClientData[%Client, UsingWeapon]) {
		if(Client::getItemCount(%Client, %item) <= 0) {
			Player::unmountItem(%Client, $WeaponSlot);
			$ClientData[%Client, UsingWeapon] = "-1";
			return 0;
		}
		return 1;
	}
}

function CheckUsingWeapon(%Client) {

	%item = $ClientData[%Client, UsingWeapon];

	if(Client::getItemCount(%Client, %item) <= 0 || %item == -1) {
		Player::unmountItem(%Client, $WeaponSlot);
		$ClientData[%Client, UsingWeapon] = "-1";
		return 0;
	}
	return 1;
}

function remoteToggleCommandMode(%Client) {
		remotePlayMode(%Client);
}

function remoteToggleObjectivesMode(%Client) {

	if(Client::getGuiMode(%Client) != $GuiModeObjectives)
		remoteObjectivesMode(%Client);
	else
		remotePlayMode(%Client);
}

function remoteUseItem(%Client, %type, %equip) {

	%time = getIntegerTime(true) >> 5;
	if(%time - %Client.lastWaitActionTime > $waitActionDelay) {

		%Client.lastWaitActionTime = %time;
		%Client.throwStrength = 1;

		%item = $ItemData[%type, DataName];
		%class = $ItemData[%item, className];

		if(getWord(%item, 0) == -1)
			return;

		if(%equip)
			%item = %item@$EquipTag;

//echo("UseItem item:"@%item@" - CLASS "@%class);
		if(%class == "")
			return;
		if(%class == "BackPack") {
			remoteConsider(%Client);
			return;
		}
		else if(%class == "toggleShield") {
			remotetoggleShield(%Client);
			return;
		}
		else {

			if(%equip)
				%list = "EquipList";
			else
				%list = "ItemList";

//echo("list:"@%list@" HasItem?:"@Client::HasItem(%Client, %item, %list));
			if(!Client::HasItem(%Client, %item, %list))
				return;
			else if($ItemData[%item, header] == $Headers::Z)
				return false;
			if(%item != -1) {
				%pl = Client::getOwnedObject(%Client);
				Item::Use(%pl, %item);
			}
		}
	}
}

function remotetoggleShield(%Client) {

	%time = getIntegerTime(true) >> 5;
	if(%time - %Client.lastWaitActionTime > $waitActionDelay)
		%Client.lastWaitActionTime = %time;
	else
		return;

	$ClientData[%Client, toggleShield] = (!$ClientData[%Client, toggleShield] |= $ClientData[%Client, toggleShield]);

	%item = Client::getItemType(%Client, $ShieldAccessoryType, "EquipList");
	if(%item != -1) {
		playSound(AxeSlash2, GameBase::getPosition(%Client));
		if($ClientData[%Client, toggleShield])
			Player::mountItem(%Client, $ItemData[%item, shape], 2);
		else
			Player::mountItem(%Client, $ItemData[%item, shape]@"OnBack", 2);
	}
	else
		$ClientData[%Client, toggleShield] = "";
}

function remoteThrowItem(%Client,%type,%strength) { return false; }

function remoteDropItem(%Client,%type, %equip, %delta) {

	%time = getIntegerTime(true) >> 5;
	if(%time - %Client.lastWaitActionTime > $waitActionDelay) {
		%Client.lastWaitActionTime = %time;

		if($droppingAllowed == 1) {

			%item = $ItemData[%type, DataName];
			%class = $ItemData[%item, className];

			if(getWord(%item, 0) == -1)
				return;

			if(%equip)
				%item = %item@$EquipTag;

//echo(%item@"|"@%class);

			%player = Client::getOwnedObject(%Client);
			if(%player.driver != 1) {
				//echo("Drop item: ",%type);
				%Client.throwStrength = 1;

				if(%class == Weapon) {
					if(%item == "Weapon")
						%item = $ClientData[%Client, UsingWeapon];
					Item::Drop(%player, %item);
					return;
				}

				if(%class == Equipped)
					Client::sendMessage(%Client, $MsgRed, "You can't drop an equipped item!~wC_BuySell.wav");
				else if(!Client::HasItem(%Client, %item, "ItemList") || Client::getItemCount(%Client, %item) < %delta)
					return;
				else if($LoreItem[%item] == True)
					Client::sendMessage(%Client, $MsgRed, "You can't drop a lore item!~wC_BuySell.wav");
				else if($ItemData[%item, header] == $Headers::Z)
					return;
				else {
					Item::Drop(%player, %item, %delta);
				}
			}
		}
	}
}

function remoteDeployItem(%Client,%type) {}

function remoteConsider(%Client) {

	%time =  getIntegerTime(true) >> 5;
	if((%time - %Client.lastRemoteActionTime) > 5) {

		%Client.lastRemoteActionTime = %time;

		%length = 500;

		%player = Client::getOwnedObject(%Client);

		$los::object = "";
		if(GameBase::getLOSinfo(%player, %length))
		{
			%obj = getObjectType($los::object);
			%cl = Player::getClient($los::object);

			%object = $los::object;
			%objpos = $los::position;

			if(%obj == "Player")
			{
				DisplayGetInfo(%Client, %cl, %obj);
				if(client::getname(%cl) == -1 || client::getname(%cl) == "")
				{
					listspawn(%cl,1);
					item::pop($los::object);
				}
				%sawsomething = True;
			}
			else if(%obj == "StaticShape" && $TownBot[$los::object, NAME] != "") {
				Client::sendMessage(%Client, $MsgWhite, "TownBot ("@$los::object@") Type:"@$TownBot[$los::object, TYPE]@".");
				%sawsomething = True;
			}
			else if(%obj == "StaticShape" && Object::getName($los::object) == "MagicWall") {
				Client::sendMessage(%Client, $MsgWhite, $los::object.owner@"'s magic wall has "@$los::object.hp@" hp.");
				%sawsomething = True;
			}
			else if(%obj == "InteriorShape" && %object.tag != "" && %Client.adminLevel >= 1) {
				Client::sendMessage(%Client, $MsgWhite, %object @ "'s tag name: " @ %object.tag);
				%sawsomething = True;
			}
			else if(%Client.adminLevel >= 1) {
				Client::sendMessage(%Client, $MsgWhite, "Position at LOS is " @ %objpos);
				%sawsomething = True;
			}
		}

		if(!%sawsomething)
			Client::sendMessage(%Client, $MsgWhite, "You see nothing to consider.");
	}
}

function DisplayGetInfo(%Client, %id, %obj) {

	if(%Client.adminLevel >= 1) {
		%showid = %id@" ("@Client::getOwnedObject(%id)@")";
		%lvl = "LVL "@getFinalLVL(%id)@" ";
	}
	else {
		%lvl = getLVLFromWIS(getFinalWIS(%Client), getFinalLVL(%id))@" ";
	}

	%msg = "<f1>"@Client::getName(%id)@", "@%lvl@$RACE[%id]@" "@$CLASSN[%id]@"<f0>  "@%showid@"\n\n"@$PlayerInfo[%id];
	if($PlayerInfo[%id] == "")
		%msg = %msg @ "This player has not setup his/her information.  Use #setinfo to set this type of information.";

	bottomprint(%Client, %msg, floor(String::len(%msg) / 20));
}

function getLVLFromWIS(%WIS, %LeveltoCal) {

	%a = Cap(floor( (%LeveltoCal * ((getRandom()*50)+10) / %wis) ), 0, 76);

	if(%a == 0)
		return "LVL "@%LevelToCal;
	else if(%a > 75)
		return "LVL ???";

	else {

		if(getRandom() > 0.5)
			%lvl = %LeveltoCal - %a + floor(25*getRandom()*getRandom());
		else
			%lvl = %LeveltoCal + %a - floor(25*getRandom()*getRandom());

		return "LVL? "@Cap(%lvl, 1, 999);
	}
}

//This function is a placeholder+prevents possible console spam.
//By phantom: beatme101.com, tribesrpg.org
function remoteRawKey(%client, %key, %mod){
	client::sendmessage(%client, 0, "This server does not support the use of extra keybinds.");

	//Under normal conditions, %key will be one of the following:
	//Repack 4 and up:
	//"numpad0" - "numpad9", "numpadenter", "numpad+", "numpad-", "numpad*", "numpad/", "0"
	//Repack 6 adds:
	//"1" - "9" (only with %mod "alt" or "control"), "f1" - "f12" (only with %mod "")

	//Under normal conditions, %mod will be one of the following:
	//"", "control", "alt", "shift"

	//You shouldn't see "alt" and "numpadenter" together because that
	//toggles fullscreen, and thus isn't bound to this on the Tribes Repack.
	//If you decide to code something here, ensure that it can handle
	//anything a client might send to try to mess with the system.
	//See the current repack version's extra-controls.cs for a full list of
	//acceptable input, and note that this could be updated in the future.

}