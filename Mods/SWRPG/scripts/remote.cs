function remotebwadmin::isCompatible()
{
	return;//to stop the unknown command error when someone using it logs in.
}

function remoteTrue()
{
	//for some reason this function gets called from key binds, so i created it so the console doesn't get flooded with
	//remoteTrue: unknown commands.
	return;
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
}

function remoteCommandMode(%clientId)
{
	if(Player::getItemCount(%clientId, "PDA") > 0)
	{
		if(%clientid.computer =="")
			Player::useItem(%clientId, "PDA"); //PDA, see the item in Accessory.cs, it's the portable computer terminal.
		else
			Client::cancelMenu(%clientId);
	}
else {
	if(!(%clientId.adminLevel >= 1))
	{
		//RPG players don't need commander mode.
		return;
	}

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
}

function remoteInventoryMode(%clientId)
{
	if(!%clientId.guiLock && !Observer::isObserver(%clientId))
	{
		remoteSCOM(%clientId, -1);
		Client::setGuiMode(%clientId, $GuiModeInventory);

		Client::clearItemShopping(%clientId);
		Client::clearItemBuying(%clientId);

		%txt = "<f1><jc>CREDITS: " @ fetchData(%clientId, "COINS");
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

function remoteUseItem(%clientId, %type) //clientId is actually a player id? wth?
{
	dbecho($dbechoMode, "remoteUseItem(" @ %clientId @ ", " @ %type @ ")");

	%time = getIntegerTime(true) >> 5;
	if(%time - %clientId.lastWaitActionTime > $waitActionDelay)
	{
		%clientId.lastWaitActionTime = %time;

		%clientId.throwStrength = 1;

		if(%type == Backpack) 
			remoteConsider(%clientId);
		else if(%type == Beacon) 
		{
			//ToggleBook();
			if($ClientData[%clientId, "SelectedSpell"] == "")
				processMenuFP(%clientId, 1);
				//processMenuPowerMenu(%clientId, 1);
			else
				remoteSay(%clientId, false, "#cast " @ $ClientData[%clientId, "SelectedSpell"]);
			
		}
		else if(%type == "Repair Kit")
		{
			Player::useItem(%clientId, $ClientData[%clientId, "SelectedPotion"]);
			client::sendMessage(%clientId, 0, "You used a " @ $ClientData[%clientId, "SelectedPotion"].description);
		}
		else if(%type == "Blaster")
			CycleFPUp(%clientId);
		else if(%type == "Plasma Gun")
			CycleFPDown(%clientId);
		else if(%type == "Chaingun")
			processMenuFP(%clientId, 1);
		else if(%type == "Disc Launcher")
			%x = "4";
		else if(%type == "Grenade Launcher")
			%x = "5";
		else if(%type == "Laser Rifle")
			%x = "6";
		else if(%type == "ELF gun")
			%x = "7";
		else if(%type == "Mortar")
			%x = "8";
		else if(%type == "Targeting Laser")
			%x = "9";
		else
		{
			%item = getItemData(%type);

			if (%item == Weapon) 
				%item = Player::getMountedItem(%clientId,$WeaponSlot);
			else
				Player::useItem(%clientId, %item);
		}

		if(%x != "" && (%dat = fetchData(%clientId, "ChatBind" @ %x)) != "")
			remoteSay(%clientId, false, %dat, client::getName(%clientId));
	}
}

function remoteChatBinds(%clientId, %d)
{
	dbecho($dbechoMode, "remoteUseItem(" @ %clientId @ ", " @ %d @ ")");

	%time = getIntegerTime(true) >> 5;
	if(%time - %clientId.lastWaitActionTime > $waitActionDelay)
	{
		%clientId.lastWaitActionTime = %time;

		if(%d == "Disc Launcher")
			%x = "4";
		else if(%d == "Grenade Launcher")
			%x = "5";
		else if(%d == "Laser Rifle")
			%x = "6";
		else if(%d == "ELF gun")
			%x = "7";
		else if(%d == "Mortar")
			%x = "8";
		else if(%d == "Targeting Laser")
			%x = "9";

		if((%dat = fetchData(%clientId, "ChatBind" @ %x)) != "")
			remoteSay(%clientId, false, %dat, client::getName(%clientId));

	}
}

function remoteThrowItem(%clientId,%type,%strength)
{
	if($ClientData[%clientId, "SelectedGrenade"] == "" || !(Player::getItemCount(%clientId, $ClientData[%clientId, "SelectedGrenade"]) > 0))
		return;

	//echo("Throw item: " @ %type @ " " @ %strength);
	%item = getItemData(%type);
	if (%item == Grenade)
	{
		if (%strength < 0)
			%strength = 0;
		else
			if (%strength > 100)
				%strength = 100;
		%clientId.throwStrength = 0.3 + 0.7 * (%strength / 100);
		//Player::useItem(%clientId, $ClientData[%clientId, "SelectedGrenade"]);
		Grenade::Throw(client::getOwnedObject(%clientId), $ClientData[%clientId, "SelectedGrenade"]);
	}
	else if (%item == MineAmmo)
	{
		//Do cycling stuff
		//$ClientData[%clientId, "SelectedGrenade"] = %blah;
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
	
				%item = getItemData(%type);
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
				else if (%item.className == Equipped) //Hazor: Consider making it dec-1	 the xitemx0, inc+1 the xitemx, and tossing the xitemx
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
		%obj = getObjectType(%object); echo("Object: " @ %object.description @ ", " @ %obj);
		%cl = Player::getClient(%object);

		%index = GetEventCommandIndex(%object.tag, "onConsider");

		if(%obj == "Player")
		{
			DisplayGetInfo(%clientId, %cl, %object);
			%sawsomething = True;
		}
		else if(GameBase::getDataName(%object).className == "Station" && vector::getDistance(gamebase::getposition(%player), %objpos) < 2)
		{
			Computer::Initialize(%clientId, %object);
			%sawsomething = True;
		}
		else if(%obj == "Backpack") //Doesn't actually work. x.x It can't see them, or something. I wonder what'd happen if I turned 'em into static shapes.. of course, then it wouldn't fall or anything. Maybe if it spawns the itemdata, then upon reaching velocity "0 0 0" turn it into a static? :o
        	{
			%msg = "";

			%ownerName = GetWord($loot[%object], 0);
			%namelist = GetWord($loot[%object], 1);
			if($loot[%object] == "")
				%msg = "You found an empty backpack.";
			else
			{
				if(IsInCommaList(%namelist, Client::getName(%clientId)) || %namelist == "*")
				{
					if(String::ICompare(%ownerName, Client::getName(%clientId)) == 0)
						%msg = "You found one of your backpacks.";
					else if(%ownerName == "*")
						%msg = "You found a backpack.";
					else
						%msg = "You found one of " @ %ownerName @ "'s backpacks.";
				}
			}

			if(%msg != "")
			{
				%newloot = String::getSubStr($loot[%object], String::len(%ownerName)+String::len(%namelist)+2, 99999);

				Client::sendMessage(%clientId, 0, %msg);

				GiveThisStuff(%clientId, %newloot, True);

				if(%object.tag != "")
				{
					$tagToObjectId[%object.tag] = "";
					$SpawnPackList = RemoveFromCommaList($SpawnPackList, %object.tag);
				}
				Item::playPickupSound(%object);
				$loot[%object] = "";

				if(%ownerName != "*")
				{
					%ownerId = NEWgetClientByName(%ownerName);
					storeData(%ownerId, "lootbaglist", RemoveFromCommaList(fetchData(%ownerId, "lootbaglist"), %object));
				}

				//event stuff
				%i = GetEventCommandIndex(%object, "onpickup");
				if(%i != -1)
				{
					%name = GetWord($EventCommand[%object, %i], 0);
					%type = GetWord($EventCommand[%object, %i], 1);
					%cl = NEWgetClientByName(%name);
					if(%cl == -1)
						%cl = 2048;

					%cmd = String::NEWgetSubStr($EventCommand[%object, %i], String::findSubStr($EventCommand[%object, %i], ">")+1, 99999);
					%pcmd = ParseBlockData(%cmd, %clientId, "");
					$EventCommand[%object, %i] = "";
					remoteSay(%cl, 0, %pcmd, %name);
				}

				deleteObject(%object);
				ClearEvents(%object);
			}
			else
			{
				if(%ownerName == "*")
					Client::sendMessage(%clientId, $MsgRed, "You do not have the right to take this backpack.");
				else
					Client::sendMessage(%clientId, $MsgRed, "You do not have the right to take " @ %ownerName @ "'s backpack.");
			}
			%sawsomething = True;
		}
		else if(%clientId.adminLevel >= 1)
		{
			if(%obj == "InteriorShape" && %object.tag != "")
				Client::sendMessage(%clientId, $MsgWhite, %object @ "'s tag name: " @ %object.tag);
			else if(%obj == "Vehicle" || %obj == "Flyer")
				Client::sendMessage(%clientId, $MsgWhite, "Object at LOS is " @ %obj);
			%sawsomething = True;
		}
		else if(%obj.description != "")
		{
			Client::sendMessage(%clientId, $MsgWhite, "You see: " @ %obj.description);
			%sawsomething = True;
		}

		if(%clientId.adminLevel >= 3)
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
				remoteSay(%cl, 0, %pcmd, %name);

				%sawsomething = True;
			}
		}
	}

	if(!%sawsomething)
		Client::sendMessage(%clientId, $MsgWhite, %nothingMsg);
}
