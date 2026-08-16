//This file is part of Tribes RPG.
//Tribes RPG server side scripts
//Written by Jason "phantom" Daley,  Matthiew "JeremyIrons" Bouchard, and more (yet undetermined)

//	Copyright (C) 2016  Jason Daley

//	This program is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.

//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.

//	You should have received a copy of the GNU General Public License
//	along with this program.  If not, see <http://www.gnu.org/licenses/>.

//You may contact the author at beatme101@gmail.com or www.tribesrpg.org/contact.php

//This GPL does not apply to Starsiege: Tribes or any non-RPG related files included.
//Starsiege: Tribes, including the engine, retains a proprietary license forbidding resale.

function remoteTrue()
{
	//for some reason this function gets called from key binds, so i created it so the console doesn't get flooded with
	//remoteTrue: unknown commands.
	return;
}

//This fixes consol spam caused by idiots using poorly coded scripts :)
//This may be too simple.. Meh. Provided by tribesrpg.org
function remotePlayFakeDeath(){
}
function remoteSetclient(){
}
function remotegiveall(%client, %arg){
}
function remotebwadmin::isCompatible(%client, %arg){
}


function remotePlayMode(%clientId)
{
	Client::clearItemShopping(%clientId);
	Client::clearItemBuying(%clientId);
	ClearCurrentShopVars(%clientId);

	if(!%clientId.guiLock)
	{
		remoteSCOM(%clientId, -1);
		Client::setGuiMode(%clientId, $GuiModePlay);
	}
	if(%clientId.menuMode != ""){
		remoteScoresOff(%clientId);
	}
}

function remoteCommandMode(%clientId)
{
	//RPG players don't need commander mode.
	remoteRawKey(%clientId, "c");	return;

	//if(!(%clientId.adminLevel >= 1))
	//{
	//	//RPG players don't need commander mode.
	//	return;
	//}

	Client::clearItemShopping(%clientId);
	Client::clearItemBuying(%clientId);
	ClearCurrentShopVars(%clientId);

	// can't switch to command mode while a server menu is up
	if(!%clientId.guiLock)
	{
		remoteSCOM(%clientId, -1);  // force the bandwidth to be full command

		Client::setGuiMode(%clientId, $GuiModeCommand);
	}
}

function remoteInventoryMode(%clientId)
{
	if(!%clientId.guiLock && !Observer::isObserver(%clientId))
	{
		remoteSCOM(%clientId, -1);
		Client::setGuiMode(%clientId, $GuiModeInventory);

		Client::clearItemShopping(%clientId);
		Client::clearItemBuying(%clientId);

		%txt = "<f1><jc>COINS: " @ fetchData(%clientId, "COINS");
		Client::setInventoryText(%clientId, %txt);
	}
}

function remoteObjectivesMode(%clientId)
{
	Client::clearItemShopping(%clientId);
	Client::clearItemBuying(%clientId);
	ClearCurrentShopVars(%clientId);

	if(!%clientId.guiLock)
	{
		remoteSCOM(%clientId, -1);
		Client::setGuiMode(%clientId, $GuiModeObjectives);
	}
}

function remoteScoresOn(%clientId)
{
	if(!%clientId.menuMode)
		Game::menuRequest(%clientId);
}

function remoteScoresOff(%clientId)
{
	Client::cancelMenu(%clientId);
}

function remoteToggleCommandMode(%clientId)
{
	if(Client::getGuiMode(%clientId) != $GuiModeCommand)
		remoteCommandMode(%clientId);
	else
		remotePlayMode(%clientId);
}

function remoteToggleInventoryMode(%clientId)
{
	Client::clearItemShopping(%clientId);
	Client::clearItemBuying(%clientId);
	ClearCurrentShopVars(%clientId);

	if(Client::getGuiMode(%clientId) != $GuiModeInventory)
		remoteInventoryMode(%clientId);
	else
		remotePlayMode(%clientId);
}

function remoteToggleObjectivesMode(%clientId)
{
	if(Client::getGuiMode(%clientId) != $GuiModeObjectives)
		remoteObjectivesMode(%clientId);
	else
		remotePlayMode(%clientId);
}

function remoteUseItem(%clientId, %type)
{
	dbecho($dbechoMode, "remoteUseItem(" @ %clientId @ ", " @ %type @ ")");

	%trueClientId = player::getclient(%clientId);	if(%trueClientId == "" || %trueClientId == -1)		%trueClientId = %clientId;

	%time = getIntegerTime(true) >> 5;
	if(%time - %trueClientId.lastWaitActionTime > $waitActionDelay)
	{
		%trueClientId.lastWaitActionTime = %time;

		%trueClientId.throwStrength = 1;

		%item = getItemData(%type);

		if(%item == "BeltItemTool")		{			MenuViewBelt(%trueClientId, 1);			return;		}

		%pressedKey = -1;
		if(%item == Backpack) 
		{
			%item = -1;
			remoteConsider(%trueClientId);
			return;
		}
		%weaponNameToKey["Blaster"] = 1;		%weaponNameToKey["PlasmaGun"] = 2;		%weaponNameToKey["Chaingun"] = 3;		%weaponNameToKey["DiscLauncher"] = 4;		%weaponNameToKey["GrenadeLauncher"] = 5;		%weaponNameToKey["LaserRifle"] = 6;		%weaponNameToKey["ElfGun"] = 7;		%weaponNameToKey["Mortar"] = 8;		%weaponNameToKey["TargetingLaser"] = 9;		%weaponNameToKey["RepairKit"] = "h";		%weaponNameToKey["Beacon"] = "b";
		if(%weaponNameToKey[%item] != ""){			%pressedKey = %weaponNameToKey[%item];
			if(%trueClientId.overrideKeybinds){				remoteRawOverride(%trueClientId, %pressedKey);				return;			}			if($numMessage[%trueClientId, %pressedKey] == "")				client::sendmessage(%trueClientId, 0, "#set "@%pressedKey@" [message]");			else				internalSay(%trueClientId,0,$numMessage[%trueClientId, %pressedKey]);		}
		else
		{
			if (%item == Weapon) 
				%item = Player::getMountedItem(%clientId,$WeaponSlot);
	
			if(%item != -1)
			{
				Player::useItem(%clientId, %item);
			}
		}
	}
}

function remoteThrowItem(%clientId,%type,%strength)
{
	%trueClientId = player::getclient(%clientId);
	%item = getItemData(%type);	%pressedKey = -1;	if(%item == Grenade)		%pressedKey = "g";	else if(%item == MineAmmo)		%pressedKey = "m";	if(%pressedKey != -1){		remoteRawKey(%clientId, %pressedKey);		return;	}
	return;

	//echo("Throw item: " @ %type @ " " @ %strength);
	%item = getItemData(%type);
	if (%item == Grenade || %item == MineAmmo) {
		if (%strength < 0)
			%strength = 0;
		else
			if (%strength > 100)
				%strength = 100;
		%clientId.throwStrength = 0.3 + 0.7 * (%strength / 100);
		Player::useItem(%clientId,%item);
	}
}

function remoteDropItem(%clientId,%type)
{
	dbecho($dbechoMode, "remoteDropItem(" @ %clientId @ ", " @ %item @ ")");

	%time = getIntegerTime(true) >> 5;
	if(%time - %clientId.lastWaitActionTime > $waitActionDelay)
	{
		%clientId.lastWaitActionTime = %time;
	
		if($droppingAllowed == 1)
		{
			if((Client::getOwnedObject(%clientId)).driver != 1) {
				//echo("Drop item: ",%type);
				%clientId.throwStrength = 1;
	
				%item = getItemData(%type);				if(%item == "BeltItemTool"){					return;				}
				if(%item == Weapon)
				{
					%item = Player::getMountedItem(%clientId,$WeaponSlot);
					Player::dropItem(%clientId,%item);
				}
				else if(%item == Ammo)
				{
					%item = Player::getMountedItem(%clientId,$WeaponSlot);
					if(%item.className == Weapon)
					{
						%item = %item.imageType.ammoType;
						Player::dropItem(%clientId,%item);
					}
				}
				else if (%item.className == Equipped)
				{
					Client::sendMessage(%clientId, $MsgRed, "You can't drop an equipped item!~wC_BuySell.wav");
				}
				else if ($LoreItem[%item])
				{
					Client::sendMessage(%clientId, $MsgRed, "You can't drop a lore item!~wC_BuySell.wav");
				}
				else 
					Player::dropItem(%clientId,%item);
			}
		}
	}
}
function remoteDeployItem(%clientId,%type)
{
	//echo("Deploy item: ",%type);
	%item = getItemData(%type);
	Player::deployItem(%clientId,%item);
}

function remoteConsider(%clientId)
{
	dbecho($dbechoMode, "remoteConsider(" @ %clientId @ ")");


	%object = fetchData(%clientId, "InEnterBox");		if(IsJailed(%clientId))			return;	if(%object != ""){		enterEnterBox(%clientId,%object);		return;	}
	%inSleepZone = fetchData(%clientId, "InSleepZone");	if(%inSleepZone != ""){		if(IsDead(%clientId))			return;		if(%clientId.sleepMode == ""){			%clientId.sleepMode = 1;			Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));			Observer::setOrbitObject(%clientId, Client::getOwnedObject(%clientId), 30, 30, 30);			RefreshAll(%clientId);			centerprint(%clientId, "<jc>You are asleep.\n<f2>Press T to awaken.", 10);		}		else {			%clientId.sleepMode = "";			Client::setControlObject(%clientId, %clientId);			RefreshAll(%clientId);			centerprint(%clientId, "<jc>You wake up.\nThis spot feels relaxing enough to sleep.\n<f2>Press T to sleep.", 10);		}		return;	} else {		if(%clientId.sleepMode != "")		{			%clientId.sleepMode = "";			Client::setControlObject(%clientId, %clientId);			RefreshAll(%clientId);			centerprint(%clientId, "<jc>You wake up.", 3);			return;		}	}

	%msgText[7] = "Easy prey!";
	%msgText[6] = "Shouldn't be a problem at all.";
	%msgText[5] = "You should win.";
	%msgText[4] = "Looks like an even fight.";
	%msgText[3] = "You might get killed...";
	%msgText[2] = "Looks VERY risky...";
	%msgText[1] = "You will NOT survive!";

	%msgColor[7] = $MsgGreen;
	%msgColor[6] = $MsgBeige;
	%msgColor[5] = $MsgBeige;
	%msgColor[4] = $MsgWhite;
	%msgColor[3] = $MsgRed;
	%msgColor[2] = $MsgRed;
	%msgColor[1] = $MsgRed;

	%maxMsg = 7;
	%midMsg = 4;
	%minMsg = 1;

	%nothingMsg = "You see nothing of interest.";
	%length = 500;
	%sawsomething = "";

	%player = Client::getOwnedObject(%clientId);
	%clientname = Client::getName(%clientId);
	%clientpos = GameBase::getPosition(%clientId);

	$los::object = "";
	$los::position = "";
	if(GameBase::getLOSinfo(%player, %length))
	{
		%object = $los::object;
		%objpos = $los::position;
		%obj = getObjectType(%object);
		%cl = Player::getClient(%object);

		%index = GetEventCommandIndex(%object.tag, "onConsider");

		if(%obj == "Player")
		{
			DisplayGetInfo(%clientId, %cl, %object);
			%sawsomething = True;
		}
		else if($BotInfo[%object.name, NAME] != "")		{			%clientPos = GameBase::getPosition(%clientId);			%botPos = GameBase::getPosition(%object);			%closest = Vector::getDistance(%clientPos, %botPos);			%botz = getWord(%botPos, 2);			%clz = getWord(%clientPos, 2);			newRotateTownBot(%clientId, %object, %clientPos, %botPos);			if(Client::getTeam(%clientId) != 0)				AI::sayLater(%clientId, %object, "Eek!", True);			else if(%closest <= ($maxAIdistVec + ($PlayerSkill[%clientId, $SkillSpeech] / 50))){				if(%clientId.tmpbottalk != "")					%clientId.tmpbottalk = "";				endBotTalkChoice(%clientId);				eval("bottalk::"@clipTrailingNumbers(%object.name)@"("@%clientId@","@%object@",True,\"#say considerhi\");");			}			else				AI::sayLater(%clientId, %object, "I can't hear you all the way over there!", True);			%sawsomething = True;		}
		else if(%obj == "InteriorShape" && %object.tag != "" && %clientId.adminLevel >= 1)
		{
			Client::sendMessage(%clientId, $MsgWhite, %object @ "'s tag name: " @ %object.tag);
			%sawsomething = True;
		}
		else if(%clientId.adminLevel >= 3)
		{
			Client::sendMessage(%clientId, $MsgWhite, "Position at LOS is " @ %objpos);
			%sawsomething = True;
		}

		if(%index != -1)
		{
			%closest = 999999;
			%cindex = "";

			//pick the event with the closest radius, matching criteria of event
			for(%i2 = 0; (%index2 = GetWord(%index, %i2)) != -1; %i2++)
			{
				%ec = $EventCommand[%object.tag, %index2];

				%targetname = GetWord(%ec, 4);
				if(String::ICompare(%targetname, %clientname) == 0 || String::ICompare(%targetname, "all") == 0)
				{
					%radius = GetWord(%ec, 2);
					if(Vector::getDistance(%objpos, %clientpos) <= %radius)
					{
						if(%radius < %closest)
						{
							%closest = %radius;
							%cindex = %index2;
						}
					}
				}
			}

			if(%cindex != "")
			{
				%ec = $EventCommand[%object.tag, %cindex];

				%name = GetWord(%ec, 0);
				if((%cl = NEWgetClientByName(%name)) == -1)
					%cl = 2048;
				%keep = GetWord(%ec, 3);

				%cmd = String::NEWgetSubStr(%ec, String::findSubStr(%ec, ">")+1, 99999);
				%pcmd = ParseBlockData(%cmd, %clientId, "");
				if(!%keep)
					$EventCommand[%object.tag, %cindex] = "";
				internalSay(%cl, 0, %pcmd, %name);

				%sawsomething = True;
			}
		}
	}

	if(!%sawsomething)
		Client::sendMessage(%clientId, $MsgWhite, %nothingMsg);
}


function disableOverrides(%client){	schedule::Cancel("NewBotMessage"@%client);
	schedule::cancel("transportmenu"@%client);	if(%client.overrideKeybinds == 2){		%index = floor(%client.castingmenuindex);		EndCast(%client,False,0,%index,gamebase::getposition(%client),False);	}	%client.keyOverride = "";	%client.overrideKeybinds = "";}

function remoteRawOverride(%client, %key, %mod){	if(string::getsubstr(%key, 0, 6) == "numpad" && string::len(%key) == 7)		%key = string::getsubstr(%key, 6, 1);	if(string::getsubstr(%key, 0, 1) == "f" && string::len(%key) < 4)		%key = string::getsubstr(%key, 1, 2);	if(%mod == "control")		%key += 9;	if(String::Compare(floor(%key), %key) != 0){		client::sendmessage(%client, 0, "Please press a number corresponding to your choice.");		return;	}	if(%client.keyOverride != ""){		%evalstring = %client.keyOverride@"("@%client@",\""@%key@"\");";		eval(%evalstring);	}}

$redirectKey["t"] = "remoteConsider";
//By phantom: tribesrpg.org
function remoteRawKey(%client, %key, %mod){	if(%mod == "") {		if($redirectKey[%key] != ""){			eval($redirectKey[%key]@"("@%client@");");			return;		}	}
	if(%mod != "" && %mod != "control" && %mod != "alt" && %mod != "shift"){
		Client::sendMessage(%client, 0, "Erroneous modifier key.");
		return;
	}
	if(string::len(%key) > 15){
		Client::sendMessage(%client, 0, "Key name too long.");
		return;
	}
	if(String::findSubStr(%key, "\"") != -1 || String::findSubStr(%key, " ") != -1){
		client::sendmessage(%client, 0, "Invalid key.");
		return;
	}
	if(%client.isinvalid){
		client::sendmessage(%client, 0, "You have no character loaded yet!");
		return;
	}
	if(%client.overrideKeybinds){		remoteRawOverride(%client, %key, %mod);		return;	}
	if(string::getsubstr(%key, 0, 1) == "f")
	{
		Client::sendMessage(%client, 0, "This key ("@%key@") is not yet supported.");
		return;
	}
	if($numMessage[%client, %key] == ""){
		Client::sendMessage(%client, 0, "#set "@%key@" [message]");
	}
	else{
		internalSay(%client,0,$numMessage[%client, %key]);
	}




	//client::sendmessage(%client, 0, "This server does not support the use of extra keybinds.");

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
function refreshBattleHudPos(%clientId){	%cur = fetchData(%clientId, "battlemsg");	if(%cur == "chathud"){		remoteEval(%clientId, DisableRPGMsghud, False);		return;	}	%coords["topleft"] = "0 0";	%coords["topprint"] = "1 0";	%coords["bottomleft"] = "0 1";	%coords["bottomprint"] = "1 1";	%pos = %coords[%cur];	if(%pos == "")		%pos = "1 1";
	remoteEval(%clientId, RPGMsgHudPos, %pos);}