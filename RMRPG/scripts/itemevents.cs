$Item::MaxItemListCount = 50; // was 30

function Item::giveItem(%player, %item, %delta, %showmsg) {
	//echo("Item::giveItem(" @ %player @ ", " @ %item @ ", " @ %delta @ ", " @ %showmsg @ ");");

	if(%delta == 0)
		return;

	%Client = Player::getClient(%player);

	%header = $ItemData[%item, header];
	if(%header != $Headers::Z) {

		if(%header == $Headers::A) //Equipped Item
			%list = "EquipList";
		else
			%list = "ItemList";

		if(!Player::isAiControlled(%Client)) {
			if(Client::getItemListCount(%Client, "ItemList") >= $Item::MaxItemListCount) {
				Client::sendMessage(%Client, 0, "You can't hold any more different types of items.");
				%time =  getIntegerTime(true) >> 5;
				if((%time - %Client.lastTossTime) > 5) {
					%Client.lastTossTime = %time;
					TossLootbag(%Client, %item@" "@%delta@" ", 5, Drop, 1);
				}
				return %delta;
			}
		}

		%cnt = Client::getItemCount(%Client, %item, %list);

		%Ndelta = Cap(%delta, 0, (99 - %cnt));

		if(%Ndelta == 0) {

			if(%showmsg)
				Client::sendMessage(%Client, $MsgWhite, "You can't carry anymore "@$ItemData[%item, Name]@"s.");

			%time =  getIntegerTime(true) >> 5;
			if((%time - %Client.lastTossTime) > 4) { // =P
				%Client.lastTossTime = %time;
				if($ClientData[Client::getName(%Client), OwnsLoot] <= $MaxDroppedPacksPerPlayer)
					TossLootbag(%Client, %item@" "@%delta@" ", 5, Drop, 1);
			}
			return %delta;
		}

		%citem = getCroppedItem(%item);
		Client::addItemCount(%Client, %citem, %Ndelta, "ItemList");

		if(%list == "EquipList")
			Client::EquipArmor(%Client, %citem, 1, 1);

		if(%showmsg)
			Client::sendMessage(%Client, 0, "You received "@%Ndelta@" "@$ItemData[%item, Name]@".");

		if($ClientData[%Client, AI::SetUpWeaps]) {//AI Stuff (only does this on spawn)
			if($ItemData[%item, className] == "Weapon")
				$ClientData[%Client, RegWeapons] = $ClientData[%Client, RegWeapons]@%item@" ";
			else if($ItemData[%item, type] == $ShieldAccessoryType)
				Client::EquipArmor(%Client, %item, true);//Equip the shield!
		}

		return %Ndelta;
	}
	else {

		%Ndelta = Cap(%delta, 0, 1);

		Client::addItemCount(%Client, %item, %Ndelta, "QuestList");
		if(%Ndelta == 1) {

			if(%showmsg) {
				Client::sendMessage(%Client, 0, "You received a "@$ItemData[%item, Name]@".");
			}
		}
	}
}

function Item::onCollision(%this,%object) {
	%Client = Player::getClient(%object);
	%armor = Player::getArmor(%Client);

	if(getObjectType(%object) == "Player" && !IsDead(%Client)) {

		%time = getIntegerTime(true) >> 5;
		if(%time - $ClientData[%Client, lastItemPickupTime] <= 0.1)
		  	return 0;

		$ClientData[%Client, lastItemPickupTime] = %time;

		%item = Item::getItemData(%this);

		if(%item == Lootbag) {

			$loot[%this, LTT] = %time;

			if($ClientData[%Client, Looting] == %this)
				return;

//echo("db onCol: lootbag: '"@$loot[%this]@"'");
			%ownerName = GetWord($loot[%this], 0);

			if($loottag[%this] == Client::getName(%Client))
				%msg = "You found your backpack!";
			else if($loottag[%this] != "" && %ownerName == "*")
				%msg = "You found one of " @ $loottag[%this] @ "'s backpacks.";
			else if($loot[%this] == "")
				%msg = "You found an empty backpack.";
			else
				%msg = "";
			if(%msg != "") {

				echo(Client::getName(%client)@" Collected pack: "@$loot[%this]);
				%newloot = String::NEWgetSubStr($loot[%this], String::len(GetWord($loot[%this], 0))+1, 9999);

				Client::sendMessage(%Client, 0, %msg);
				if(GetWord(%newloot, 0) == COINS) {
					%coins = GetWord(%newloot, 1);
					GiveThisStuff(%Client, "COINS "@%coins, True);

					$loot[%this] = SetStuffString($loot[%this], "COINS", -%coins); //String::replace(" "@$loot[%this], " COINS "@%coins, " ");

					%newloot = String::NEWgetSubStr($loot[%this], String::len(GetWord($loot[%this], 0))+1, 9999);
				}
				Item::playPickupSound(%this);
				if(getWord(%newloot, 1) == -1 || getWord($loot[%this], 1) == LootBag) {
					deleteObject(%this);
					$loot[%this] = "";
					$ClientData[$loottag[%this], OwnsLoot]--;
					$loottag[%this] = "";
					$loot[%this, LTT] = "";
					$ClientData[%Client, Looting] = "";
				}
				else {

					%newloot = " "@%newloot;

//echo("db onCol: %newloot: '"@%newloot@"'");
					if(!Player::isAiControlled(%Client)) {
						$ClientData[%Client, Looting] = %this;
						SetUpLootShop(%Client, %this, %newloot);
					}
					else {
						if((%w = GetWord(%newloot, 0)) != -1) {
							%cnt = GetWord(%newloot, 1);
//echo("db: onCol: Bot loots "@%w@" "@%cnt);
							GiveThisStuff(%Client, %w@" "@%cnt, False);
							$loot[%this] = SetStuffString($loot[%this], %w, -%cnt); //String::replace(" "@$loot[%this]@" ", " "@%w@" "@%cnt, " ");
//echo("db: onCol: new lootbag: '"@$loot[%this]@"'");
						}
					}
				}
			}
			else {
				Client::sendMessage(%Client, $MsgRed, "You do not have the right to take " @ $loottag[%this] @ "'s backpack.");
			}
		}

		else if(%item.className == "Projectile") {	//Projectiles have a class added (this.RealItem)
			%Ritem = %this.RealItem;				//Only way for me to get info in the dropped Projectile
			%this.RealItem = "";
			%damagedClient = %Client;
			%shooterClient = %this.owner;
			if(%shooterClient != "") {
				%vec = Vector::getDistance("0 0 0", Item::getVelocity(%this));
				if(%vec == 0 && $ProjectileDoubleCheck[%this])
					%vec = 3.0;
			}
			else
				%vec = 0;	// don't let thrown projectiles damage!

			$ProjectileDoubleCheck[%this] = "";

			if(%vec >= 2.5)
				GameBase::virtual(%object, "onDamage", $ItemData[%Ritem, DamageType], 1.0, "0 0 0", "0 0 0", "0 0 0", "torso", "front_right", %shooterClient, %Ritem);

			else {
				if(Item::giveItem(%Client, %Ritem, %this.delta, True)) {
					Item::playPickupSound(%this);
					RefreshAll(%Client);
				}
			}
			deleteObject(%this);
		}
		else if(%item.classname == "Tele")
		{
			firedoortele(%this,%object);
		}
	}
}

function Item::onMount(%player,%item) {}
function Item::onUnmount(%player,%item) {}

function Client::reloadInv(%Client) {

	%time = getIntegerTime(true) >> 5;
	if(%time - %Client.lastreloadTime <= 60)
		return -1;
	%Client.lastreloadTime = %time;

	Client::sendMessage(%Client, 1, "Reloading Inventory...");

	remoteEval(%Client, "ClearItemLists");

	%ItemList = $ClientData[%Client, "ItemList"];
	%EquipList = $ClientData[%Client, "EquipList"];
	%QuestList = $ClientData[%Client, "QuestList"];

	%coins = $COINS[%Client];

	$Weight[%Client] = 0;
	$ClientData[%Client, "ItemList"] = " ";
	$ClientData[%Client, "EquipList"] = " ";
	$ClientData[%Client, "QuestList"] = " ";
	$ClientData[%Client, UsingWeapon] = "-1";

	CheckUsingWeapon(%Client);

	GiveThisStuff(%Client, %QuestList, false);
	GiveThisStuff(%Client, %ItemList, false);
	GiveThisStuff(%Client, %EquipList, false);

	$COINS[%Client] = %coins;
}

function Client::getItemCount(%Client, %item, %list) {

	if(%list == "")
		%list = "ItemList";

	if(%list != "ItemList" && %list != "EquipList" && %list != "QuestList")
		return false;

	%item = $ItemData[%item, FixCaps];

	%pos = String::findSubStr($ClientData[%Client, %list], " "@%item@" ");

	if(%pos != -1) { //Has item
		if(%list != "QuestList") {
			%data = String::NEWgetSubStr($ClientData[%Client, %list], %pos, 60);
			return floor(GetWord(%data, 1));
		}
		else
			return 1;
 	}
 	return 0;
}

function Client::getItemListCount(%Client, %list) {
	if(%list == "")
		%list = "ItemList";

	if(%list != "ItemList" && %list != "EquipList" && %list != "QuestList")
		return false;

	for(%i = 0; getWord($ClientData[%Client, %list], %i) != -1; %i+=2) {
		%cnt++;
	}
	return %cnt;
}

function BankCountList(%Client) {

	for(%i = 0; getWord($BankStorage[%Client], %i) != -1; %i+=2) {
		%cnt++;
	}
	return %cnt;
}

function Client::getItemTypeCount(%Client, %type, %list) {

	if(%list == "")
		%list = "ItemList";

	if(%list != "ItemList" && %list != "EquipList" && %list != "QuestList")
		return false;

	%HasType = 0;
	for(%i = 0; (%w = getWord($ClientData[%Client, %list], %i)) != -1; %i++) {
		%i++;
		if($ItemData[%w, type] == %type)
			%HasType += getWord($ClientData[%Client, %list], %i);
	}

	return %HasType;
}

function Client::getItemType(%Client, %type, %list) {

	if(%list == "")
		%list = "ItemList";

	if(%list != "ItemList" && %list != "EquipList" && %list != "QuestList")
		return false;

	for(%i = 0; (%w = getWord($ClientData[%Client, %list], %i)) != -1; %i+=2) {
		if($ItemData[%w, type] == %type)
			return %w;
	}
	return -1;
}

function Client::getItemListByClass(%Client, %cn, %list) {

	if(%list == "")
		%list = "ItemList";

	if(%list != "ItemList" && %list != "EquipList" && %list != "QuestList")
		return false;

	%clist = "";
	for(%i = 0; (%w = getWord($ClientData[%Client, %list], %i)) != -1; %i+=2) {
		if($ItemData[%w, className] == %cn)
			%clist = %clist@%w@" ";
	}
	return %clist;
}

function Client::HasItem(%Client, %item, %list) {

	if(%item == "")
		return false;

	if(%list != "") {
		if(String::findSubStr($ClientData[%Client, %list], " "@%item@" ") != -1)
			{ return true; }//Has item
	}
	else {
		if(String::findSubStr($ClientData[%Client, "ItemList"], " "@%item@" ") != -1)
			{ return true; }//Has item
	}
	return false;
}

function Client::addItemCount(%Client, %item, %amount) {

	if($ItemData[%item, Name] != "" && $ItemData[%item, Name] != "This will fix some stuff.") {

		%item = $ItemData[%item, FixCaps];

		//There! Now items will go where they should go!
		%header = $ItemData[%item, header];
		if(%header != $Headers::Z) {

			if(%header == $Headers::A) // Equipped Item
				%list = "EquipList";
			else
				%list = "ItemList";
		}
		else
			%list = "QuestList";
		//====================

		%pos = String::findSubStr($ClientData[%Client, %list], " "@%item@" ");//FIXED!
		if(%pos != -1) { //Already has this item
			%data = String::getSubStr($ClientData[%Client, %list], %pos, 60);
			%nitem = GetWord(%data, 0);
			%cnt = floor(GetWord(%data, 1));
			%ncnt = floor(%cnt+%amount);

			if(%list != "QuestList") {
				if(%ncnt > 0) { //echo("REPLACE");
					$ClientData[%Client, %list] = String::replace($ClientData[%Client, %list], " "@%item@" "@%cnt@" ", " "@%nitem@" "@%ncnt@" ");
					remoteEval(%Client, "Client::addItemCount", %item, %amount, %list, $ItemData[%item, header]);
				}
				else { //echo("REMOVE");
					$ClientData[%Client, %list] = String::replace($ClientData[%Client, %list], " "@%item@" "@%cnt@" ", " ");
					remoteEval(%Client, "Client::addItemCount", %item, %amount, %list, $ItemData[%item, header]);
				}
			}
			else {
				if(%amount <= 0 || %cnt <= 0) { //echo("REMOVE");
					$ClientData[%Client, %list] = String::replace($ClientData[%Client, %list], " "@%item@" 1 ", " ");
					remoteEval(%Client, "Client::addItemCount", %item, -1, %list, $ItemData[%item, header]);
					UpdateQuestStats(%Client);
				}
				else {
					echo(" -- ["@%Client@" - "@Client::getName(%Client)@"] -- This Client already has this Quest Item! ("@%item@") --");
					return false;
				}
			}
			$Weight[%Client] += ($ItemData[%item, weight] * %amount);//Only time when $Weight[Client] is changed
																				//addItemCount neg num will take weight away
			return true;
		}
		else {

			if(%amount < 0)  {
				echo("Error: "@%Client@" does not have item "@%item@" and is getting "@%amount@" added!");
				return false;
			}

			if(%list != "QuestList") { //echo("ADD");
				$ClientData[%Client, %list] = $ClientData[%Client, %list]@%item@" "@%amount@" ";
				remoteEval(%Client, "Client::addItemCount", %item, %amount, %list, $ItemData[%item, header]);
			}
			else { //echo("ADD");
				$ClientData[%Client, %list] = $ClientData[%Client, %list]@%item@" 1 ";
				remoteEval(%Client, "Client::addItemCount", %item, 1, %list, $ItemData[%item, header]);
				UpdateQuestStats(%Client);
			}
		}

		$Weight[%Client] += ($ItemData[%item, weight] * %amount);
		return true;
	}

	echo("Client::addItemCount(); incorrect item type: "@%item);
	return false; // Not an Item
}


function UpdateQuestStats(%Client) {

	if(!Player::isAiControlled(%Client))
		$MaxCOINS[%Client] = AddPoints(%Client, "MaxCOINS", "QuestList");

}

function Client::EquipArmor(%Client, %item, %equip, %bool) {

	if(%equip) { //Equipping Item does not have a & at the end

		Client::addItemCount(%Client, %item@$EquipTag, 1, "EquipList");
		Client::addItemCount(%Client, %item, -1, "ItemList");

		if(!%bool)
			Client::sendMessage(%Client, $MsgBeige, "You equipped "@$ItemData[%item, Name]@".");

		if($ItemData[%item, type] == $ShieldAccessoryType) {
			Player::mountItem(%Client, $ItemData[%item, shape], 2);
			$ClientData[%Client, toggleShield] = true; // = on
		}
		else if($ItemData[%item, shape] == "Orb") {
			Player::mountItem(%Client, "Orb", 3);
		}
	}
	else { //Un-equiping Item has a & at the end

		Client::addItemCount(%Client, %item, -1, "EquipList");
		Item::giveItem(Client::getOwnedObject(%Client), getCroppedItem(%item), 1, false);
		//Client::addItemCount(%Client, getCroppedItem(%item), 1, "ItemList");//if they had 99 already they'd get 100

		if(!%bool)
			Client::sendMessage(%Client, $MsgBeige, "You unequipped "@$ItemData[%item, Name]@".");

		if($ItemData[%item, type] == $ShieldAccessoryType) {
			Player::unmountItem(%Client, 2);
			$ClientData[%Client, toggleShield] = "";
		}
		else if($ItemData[%item, shape] == "Orb") {
			Player::unmountItem(%Client, 3);
		}
	}

	UpdateSkin(%Client);
}

function Item::Use(%player, %item, %forced) {

	%Client = Player::getClient(%player);

	if($ClientData[%Client, Petrify] != "") {
		Client::sendMessage(%Client, 0, "You are Petrified! You cannot use items!");
		return;
	}
	if($ItemData[%item, header] == "Equipped")
		%list = "EquipList";
	else
		%list = "ItemList";

	if(!Client::HasItem(%Client, %item, %list))
		return false;
	
	if($ItemDataOnUseFunc[%item])
		Eval("Item::"@%item@"OnUse(%player, %Client, %item, %forced);");
	else
		Item::UseItem(%player, %Client, %item, %forced);
}

function Item::UseItem(%player, %Client, %item, %forced) {
	//echo("Item::UseItem("@%player@", "@%Client@", "@%item@", "@%forced@")");

	if(!IsDead(%Client)) {

		%class = $ItemData[%item, className]; //echo("UseItem: CLASS: '"@%class@"'");

		//this is how you toggle back and forth from equipped to carrying.
		if(%class == Accessory) {
			%Ecnt = Client::getItemTypeCount(%Client, $ItemData[%item, type], "EquipList");
			if(!Client::HasItem(%Client, %item, "ItemList"))
				return false;
			if($ItemData[%item, Name] == "Zombie Ring") {
				if(%Ecnt > 0) {
					Client::sendMessage(%Client, 0, "The Zombie Ring refuses to be worn with other rings...");
					return;
				}
				else {
					Client::EquipArmor(%Client, %item, true);
					ZombieRingOn(%Client, %item);
				}
			}
			else if($isZombie[%Client] && $ItemData[%item, type] == $RingAccessoryType) {
				Client::sendMessage(%Client, 0, "The Zombie Ring refuses to be worn with other rings...");
				return;
			}
			else if(%Ecnt < $maxAccessory[$ItemData[%item, type]]) {
				if(SkillCanUse(%Client, %item, true))
					Client::EquipArmor(%Client, %item, true);
				//else
				//	Client::SendMessage(%Client, $MsgRed, "You can't equip this item because of your class.~wC_BuySell.wav");
			}
			else
				Client::SendMessage(%Client, $MsgRed, "You can't equip this item because you have too many already equipped.~wC_BuySell.wav");
		}
		else if(%class == Equipped) {
			if($isZombie[%Client])
				ZombieRingOff(%Client);
			Client::EquipArmor(%Client, %item, false);
		}
		else if(%class == Potion) {
			%svar = $ItemData[%item, svar];
			if((%pos = String::findSubStr(%svar, "HEAL")) != -1) {
				%data = String::NEWgetSubStr(%svar, %pos, 25);
				%res = (floor(GetWord(%data, 1)) * 0.01);
				Client::addItemCount(%Client, %item, -1);
				refreshHP(%Client, -%res);
			}
			if((%pos = String::findSubStr(%svar, "MP")) != -1) {
				%data = String::NEWgetSubStr(%svar, %pos, 25);
				%res = floor(GetWord(%data, 1));// * 0.01);
				Client::addItemCount(%Client, %item, -1);
				refreshMANA(%Client, -%res);

			}
			if((%pos = String::findSubStr(%svar, "STA")) != -1) { //super potions ;p
				%data = String::NEWgetSubStr(%svar, %pos, 25);
				%res = floor(GetWord(%data, 1));
				Client::addItemCount(%Client, %item, -1);
				refreshSTAMINA(%Client, -%res);
			}
		}
		else if(%class == Food) {
			if((%pos = String::findSubStr($ItemData[%item, svar], "STA")) != -1) {
				%data = String::NEWgetSubStr($ItemData[%item, svar], %pos, 25);
				%res = floor(GetWord(%data, 1));
				Client::addItemCount(%Client, %item, -1);
				refreshSTAMINA(%Client, -%res);
			}
		}
		else if(%class == "Cure Potion") {
			CurePotionStuff(%Client, %item, 0);
		}
		else if(%class == "Status Potion") {
			StatusPotionStuff(%Client, %item);
		}
		else if(%class == "Projectile") {
			if($ClientData[%Client, UsingWeapon] != -1)
				CheckCanUseProjectile(%Client, $ItemData[%item, FixCaps], $ClientData[%Client, UsingWeapon]);
		}
		else if(%class == "Alcohol") {
			if((%pos = String::findSubStr($ItemData[%item, svar], "Alvl")) != -1) {
				%data = String::NEWgetSubStr($ItemData[%item, svar], %pos, 25);
				%res = floor(GetWord(%data, 1));
				Client::addItemCount(%Client, %item, -1);
				%time = Cap(round(pow(%res, 1.2)), 1, 1.5) * 30;
				UpdateBonusState(%Client, "Alvl", %time, "add");
				$ClientData[%Client, Alvl] += %res;
			}
		}
		else if(%item == "Burning_Door") {
			firedoor(%client);
		}
		else if(%class == Weapon) {
			RPGmountItem(%player, %item, $WeaponSlot);
		}
		RefreshAll(%Client);

		return true;
	}
	return false;
}

function CheckCanUseProjectile(%Client, %arrow, %weapon) {
	//if(String::findSubStr($ItemData[%weapon, Ammo], ","@%arrow@",") != -1) {
	if((String::findSubStr(%weapon,"CrossBow") != -1 && String::findSubStr(%arrow,"Quarrel") != -1) || (String::findSubStr(%weapon,"CrossBow") == -1 && String::findSubStr(%weapon,"Bow") != -1 && String::findSubStr(%arrow,"Arrow") != -1)) 
	{
			$LoadedProjectile[%Client, %weapon] = %arrow;
			Client::sendMessage(%Client, 0, "Loaded "@$ItemData[%weapon, Name]@" with "@$ItemData[%arrow, Name]@".");
			return True;
	}
	else
		Client::sendMessage(%Client, 0, "Cannot load "@$ItemData[%arrow, Name]@" into "@$ItemData[%weapon, Name]@".");

	return False;
}

function CurePotionStuff(%Client, %item, %pos) {

	%svar = $ItemData[%item, svar];

	for(%i = 0; (%a = GetWord(%svar, %i)) != -1; %i++) {
		%i++;
		%res = GetWord(%svar, %i);

		if(%res != "Petrify" && %res != "Poison" && %res != "Blind" && %res != "Mute")
			continue;

		if(%a != "21")
			continue;

		if(%res != "Petrify") {
			$ClientData[%Client, %res] = 1;
			Client::addItemCount(%Client, %item, -1);
		}
		else {
			if((%id = GetClosestClient(%Client, 10)) != "") {
				if($ClientData[%id, Petrify] > 0) {
					$ClientData[%id, Petrify] = 1;
					Client::addItemCount(%Client, %item, -1);
					Client::sendMessage(%id, 0, Client::getName(%Client)@" uses "@$ItemData[%item, Name]@" on you.");
					Client::sendMessage(%Client, 0, "You use "@$ItemData[%item, Name]@" on "@Client::getName(%id)@".");
				}
			}
		}
	}
}

function StatusPotionStuff(%Client, %item) {

	%flag = false;
	%area = "";
	%svar = $ItemData[%item, svar];
	for(%i = 0; (%res = GetWord(%svar, %i)) != -1; %i++) { //echo("res:"@%res);
		%i++;
		if(%res == "AREA") {
			%area = getWord(%svar, %i); %i++; //echo("area "@%area);
			%res = getWord(%svar, %i); %i++; //echo("res "@%res);
			%pow = getWord(%svar, %i); //echo("pow "@%pow);
		}
		if(%area == "") {
			if(GameBase::getLOSinfo(Client::getOwnedObject(%Client), 4)) {
				%obj = getObjectType($los::object);
				if(%obj == "Player") {
					%id = Player::getClient($los::object);
					%pow = getWord(%svar, %i);
					Eval("Status::"@$StatusLookUpList[%res-30]@"("@%id@", "@%Client@", "@%pow@");");
					%flag = true;
				}
			}
		}
		else
			%pow = getWord(%svar, %i);

		if(%area != "") {
			StatusRadiusDamage(%Client, GameBase::getPosition(%Client), %area, %pow, $StatusLookUpList[%res-30]);
			%flag = true;
		}
	}
	if(%flag)
		Client::addItemCount(%Client, %item, -1);
}

function StatusWeaponStuff(%Client, %item) {

	%svar = $ItemData[%item, svar];
	for(%i = 0; (%res = GetWord(%svar, %i)) != -1; %i++) { //echo("res:"@%res);
		%i++;
		if(GameBase::getLOSinfo(Client::getOwnedObject(%Client), 4)) {
			%obj = getObjectType($los::object);
			if(%obj == "Player") {
				%id = Player::getClient($los::object);
				%pow = getWord(%svar, %i);
				Eval("Status::"@$StatusLookUpList[%res-30]@"("@%id@", "@%Client@", "@%pow@");");
			}
		}
	}
}

function StatusRadiusDamage(%Client, %pos, %area, %pow, %status) {

	%b = %area;
	%set = newObject("set", SimSet);
	%n = containerBoxFillSet(%set, $SimPlayerObjectType, %pos, %b, %b, %b, 0);
	Group::iterateRecursive(%set, DoStatusEffect, %Client, %pos, %area, %pow, %status);
	deleteObject(%set);
}

function DoStatusEffect(%object, %Client, %pos, %area, %pow, %status) {

	%id = Player::getClient(%object);
	if(%id != %Client || %status == "Poison") {//Poison hits even the person that drops it.
		%percMin = 5;
		%percMax = 100;
		%dist = Vector::getDistance(%pos, GameBase::getPosition(%id));
		if(%dist <= %area) {
			%newDamage = CalcRadiusDamage(%dist, %area, %pow, %percMin, %percMax);
			Eval("Status::"@%status@"("@%id@", "@%Client@", "@%newDamage@");");
		}
	}
}

function CalcRadiusDamage(%dist, %radius, %dmg, %percMin, %percMax) {

	%newdmg = %dmg - (%dist * (%dmg / %radius));
	%p = (%newdmg * 100) / %dmg;
	if(%p < %percMin)
		%p = %percMin;
	else if(%p > %percMax)
		%p = %percMax;
	%newdmg = (%p * %dmg) / 100;

	return %newdmg;
}

function ZombieRingOn(%Client) {
	$isZombie[%Client] = True;
	$ClientData[%Client, tmpRACE] = $RACE[%Client];
	$RACE[%Client] = "Zombie Beast";
	UpdateSkin(%Client);
}

function ZombieRingOff(%Client) {
	$isZombie[%Client] = "";
	$RACE[%Client] = $ClientData[%Client, tmpRACE];
	UpdateSkin(%Client);
}

function Item::Drop(%player, %item, %delta) {

	%Client = Player::getClient(%player);

	if($ClientData[%Client, Petrify] != "") {
		Client::sendMessage(%Client, 0, "You are Petrified! You cannot drop items!");
		return;
	}
	Item::DropItem(%player, %Client, %item, %delta);
}

function Item::DropItem(%player, %Client, %item, %delta) {

	if($ClientData[Client::getName(%Client), OwnsLoot] <= $MaxDroppedPacksPerPlayer || $ClientData[%Client, WantsToDropOverLimit]) {
		if($ItemData[%item, className] == Projectile) {
			if(%delta == "")
				%delta = 20;
		}
		else {
			if(%delta == "")
				%delta = 1;
		}

		%cnt = Client::getItemCount(%Client, %item);
		if(%cnt < %delta)
			%delta = %cnt;

		if(%delta > 0) {
			if(%delta == 1 && $ItemData[%item, className] == "Weapon")
				CheckMountWeapon(%Client, %item);

	 	 	TossLootbag(%Client, %item@" "@%delta@" ", 5, Drop, 1);
			Client::addItemCount(%Client, %item, -%delta);
			RefreshAll(%Client);
			return true;
		}
		else {
			if(Client::HasItem(%Client, %item@$EquipTag, "EquipList")) {
				Client::sendMessage(%Client, $MsgRed, "You cannot drop an equipped item!~wC_BuySell.wav");
				return false;
			}
		}
		return true;
	}
	if(!$ClientData[%Client, WantsToDropOverLimit]) Client::sendMessage(%Client, 0, "You can only have "@$MaxDroppedPacksPerPlayer@" packs dropped at one time.(type #nolimit true if you want to drop more items, but NO pack will be dropped)");
	return false;
}


function Ammo::onDrop(%player,%item)  {
return;
	if($matchStarted)
	{
		if(%item.className == Ammo)
			%delta = 20;
		else
			%delta = 1;

		%Client = Player::getClient(%player);
		%cnt = Client::getItemCount(%Client, %item);
		if(%cnt < %delta)
			%delta = %cnt;

		if(%delta > 0)
		{
			TossLootbag(%Client, %item@" "@%delta@" ", 5, Drop, 5);

			Client::addItemCount(%Client, %item, -%delta);

			RefreshAll(Player::getClient(%player));
		}
	}
}

function Item::onDeploy(%player,%item,%pos) {}
