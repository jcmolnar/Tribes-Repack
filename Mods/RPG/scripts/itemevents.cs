function Item::giveItem(%player, %item, %delta, %showmsg)
{
	dbecho($dbechoMode, "Item::giveItem(" @ %player @ ", " @ %item @ ", " @ %delta @ ", " @ %showmsg @ ")");

	%clientId = Player::getClient(%player);

	//i used to restrict what you could pick up here, but that sucks, so i made
	//it so you can pick up anything, but you can't EQUIP anything. (see Item::onUse)

	//also, the only reason you'd be getting a giveItem of an Equipped type is
	//by giving the client an item and pre-equipping it.

	if(%showmsg)
		Client::sendMessage(%clientId, 0, "You received " @ %delta @ " " @ %item.description @ ".");

	if(isbeltitem(%item))		belt::givethisstuff(%clientId, %item, %delta);	else
		Player::incItemCount(%clientId, %item, %delta);

	return %delta;
}

function Item::onCollision(%this,%object)
{
	dbecho($dbechoMode, "Item::onCollision(" @ %this @ ", " @ %object @ ")");

	%clientId = Player::getClient(%object);
	%armor = Player::getArmor(%clientId);

	if(getObjectType(%object) == "Player" && !IsDead(%clientId))
      {
		%time = getIntegerTime(true) >> 5;
		if(%time - %clientId.lastItemPickupTime <= 0.1)
			return 0;

		%clientId.lastItemPickupTime = %time;

		%item = Item::getItemData(%this);

            if(%item == "Lootbag")
            {
			%msg = "";

			%ownerName = GetWord($loot[%this], 0);
			%namelist = GetWord($loot[%this], 1);
			if($loot[%this] == "")
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
				%newloot = String::getSubStr($loot[%this], String::len(%ownerName)+String::len(%namelist)+2, 99999);

				Client::sendMessage(%clientId, 0, %msg);

				GiveThisStuff(%clientId, %newloot, True);

				if(%this.tag != "")
				{
					$tagToObjectId[%this.tag] = "";
					$SpawnPackList = RemoveFromCommaList($SpawnPackList, %this.tag);
				}
				Item::playPickupSound(%this);
				$loot[%this] = "";

				if(%ownerName != "*")
				{
					%ownerId = NEWgetClientByName(%ownerName);
					storeData(%ownerId, "lootbaglist", RemoveFromCommaList(fetchData(%ownerId, "lootbaglist"), %this));
				}

				//event stuff
				%i = GetEventCommandIndex(%this, "onpickup");
				if(%i != -1)
				{
					%name = GetWord($EventCommand[%this, %i], 0);
					%type = GetWord($EventCommand[%this, %i], 1);
					%cl = NEWgetClientByName(%name);
					if(%cl == -1)
						%cl = 2048;

					%cmd = String::NEWgetSubStr($EventCommand[%this, %i], String::findSubStr($EventCommand[%this, %i], ">")+1, 99999);
					%pcmd = ParseBlockData(%cmd, %clientId, "");
					$EventCommand[%this, %i] = "";
					internalSay(%cl, 0, %pcmd, %name);
				}

				deleteObject(%this);
				ClearEvents(%this);
			}
			else
			{
				if(%ownerName == "*")
					Client::sendMessage(%clientId, $MsgRed, "You do not have the right to take this backpack.");
				else
					Client::sendMessage(%clientId, $MsgRed, "You do not have the right to take " @ %ownerName @ "'s backpack.");
			}
            }
            else if(%item.className == "Projectile")
            {
			%damagedClient = %clientId;
			%shooterClient = %this.owner;
			if(%shooterClient != "")
			{
				%vec = Vector::getDistance("0 0 0", Item::getVelocity(%this));
				if(%vec == 0 && $ProjectileDoubleCheck[%this])
					%vec = 3.0;
			}
			else
				%vec = 0;	//don't let thrown projectiles damage!

			$ProjectileDoubleCheck[%this] = "";

			if(%vec >= 2.5)
			{
				GameBase::virtual(%object, "onDamage", $DamageType[%item], 1.0, "0 0 0", "0 0 0", "0 0 0", "torso", %this.weapon, %shooterClient, %item);
			}
			else
			{
				if(Item::giveItem(%clientId, %item, %this.delta, True))
				{
					Item::playPickupSound(%this);
					RefreshAll(%clientId);
				}
			}

			deleteObject(%this);
		}
            else if(%item.className == "Accessory" || $LoreItem[%item] == True)
            {
			if(Item::giveItem(%clientId, %item, 1, True))
			{
				Item::playPickupSound(%this);
				RefreshAll(%clientId);
				deleteObject(%this);
			}
		}
		else if(%item.className == "TownBot")
		{
			//do nothing.
		}
            else
            {
            	//%count = Player::getItemCount(%object,%item);
            	if(Item::giveItem(%object, %item, %this.delta, True))
                  {
                  	Item::playPickupSound(%this);
				RefreshAll(%clientId);
                        Item::respawn(%this);
			}
		}
	}
}

function Item::onMount(%player,%item)
{
}

function Item::onUnmount(%player,%item)
{
}

function Item::onUse(%player,%item)
{
	dbecho($dbechoMode, "Item::onUse(" @ %player @ ", " @ %item @ ")");

	%clientId = Player::getClient(%player);

	if(!IsDead(%clientId))
	{
		//this is how you toggle back and forth from equipped to carrying.
		if(%item.className == Accessory)
		{
			%cnt = 0;
			%max = getNumItems();
			for(%i = 0; %i < %max; %i++)
			{
				%checkItem = getItemData(%i);
				if(%checkItem.className == Equipped && GetAccessoryVar(%checkItem, $AccessoryType) == GetAccessoryVar(%item, $AccessoryType))
					%cnt += Player::getItemCount(%player, %checkItem);
			}

			if(SkillCanUse(%clientId, %item))
			{
				if(%cnt < $maxAccessory[GetAccessoryVar(%item, $AccessoryType)])
				{
					Client::sendMessage(%clientId, $MsgBeige, "You equipped " @ %item.description @ ".");
					Player::setItemCount(%player, %item, Player::getItemCount(%player, %item)-1);
					Player::setItemCount(%player, %item @ "0", Player::getItemCount(%player, %item @ "0")+1);
				}
				else
					Client::sendMessage(%clientId, $MsgRed, "You can't equip this item because you have too many already equipped.~wC_BuySell.wav");
			}
			else
				Client::sendMessage(%clientId, $MsgRed, "You can't equip this item because you lack the necessary skills.~wC_BuySell.wav");

			if($OverrideMountPoint[%item] == "")
				Player::mountItem(%player, %item @ "0", 1, 0);
		}
		else if(%item.className == Equipped)
		{
			%o = String::getSubStr(%item, 0, String::len(%item)-1);	//remove the 0
			Client::sendMessage(%clientId, $MsgBeige, "You unequipped " @ %item.description @ ".");
			Player::setItemCount(%player, %item, Player::getItemCount(%player, %item)-1);
			Player::setItemCount(%player, %o, Player::getItemCount(%player, %o)+1);

			if($OverrideMountPoint[%item] == "")
				Player::unMountItem(%player, 1);
		}
		else
		{
			RPGmountItem(%player, %item, $DefaultSlot);
		}

		refreshHP(%clientId, 0);
		refreshMANA(%clientId, 0);
		RefreshAll(%clientId);
	}
}

function Item::onDrop(%player,%item)
{
	dbecho($dbechoMode, "Item::onDrop(" @ %player @ ", " @ %item @ ")");

	if($matchStarted)
	{
		if(%item.className != Armor)
		{
			if(%item.className == Projectile)
				%delta = 20;
			else
				%delta = 1;

			if(Player::getItemCount(%player, %item) < %delta)
				%delta = Player::getItemCount(%player, %item);

			if(%delta > 0)
			{
				%obj = newObject("","Item",%item,1,false);
				%obj.delta = %delta;
	 	 	  	schedule("Item::Pop(" @ %obj @ ");", $ItemPopTime, %obj);
	 	 	 	addToSet("MissionCleanup", %obj);

				if(IsDead(%player)) 
					GameBase::throw(%obj, %player, 10, true);
				else {
					GameBase::throw(%obj, %player, 15, false);
					Item::playPickupSound(%obj);
				}

				Player::decItemCount(%player,%item,%delta);
				RefreshAll(Player::getClient(%player));

				return %obj;
			}
		}
	}
}

function Ammo::onDrop(%player,%item)
{
	dbecho($dbechoMode, "Ammo::onDrop(" @ %player @ ", " @ %item @ ")");

	if($matchStarted)
	{
		if(%item.className == Ammo)
			%delta = 20;
		else
			%delta = 1;

		if(Player::getItemCount(%player, %item) < %delta)
			%delta = Player::getItemCount(%player, %item);

		if(%delta > 0)
		{
			%obj = newObject("","Item",%item,%delta,false);
			%obj.delta = %delta;
	      	schedule("Item::Pop(" @ %obj @ ");", $ItemPopTime, %obj);

      		addToSet("MissionCleanup", %obj);
			GameBase::throw(%obj,%player,20,false);
			Item::playPickupSound(%obj);
			Player::decItemCount(%player,%item,%delta);

			RefreshAll(Player::getClient(%player));
		}
	}
}	

function Item::onDeploy(%player,%item,%pos)
{
}
