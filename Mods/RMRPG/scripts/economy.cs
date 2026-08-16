
$ArrowList = "Small_Rock,";

function getBuyCost(%aiName, %item) {
	%p = $ItemCost[%item];
	if($NewItemBuyCost[%aiName, %item] != "")
		%cost = $NewItemBuyCost[%aiName, %item];
	else
		%cost = %p;

	return %cost;
}
function getSellCost(%aiName, %item) {
	%p = $ItemCost[%item];
	if($NewItemSellCost[%aiName, %item] != "")
		%cost = $NewItemSellCost[%aiName, %item];
	else
		%cost = round(%p * ($resalePercentage/100));

	return %cost;
}


function BuySell(%player, %item, %delta, %buyORsell) {

	// IF - Cost positive selling    IF - Cost Negative buying

	%Client = Player::getClient(%player);
//	%station = %player.Station;
//	%stationName = GameBase::getDataName(%station);

	if(%Client.adminLevel < 4)
	{
		//Add entry to merchant counter.
		//Admin purchases do not count towards the economy.
		%aiName = $BotInfoAiName[%Client.currentShop];
		if(%buyORsell == BUY)
		{
			$MerchantCounterB[%aiName, %item] += %delta;
			%cost = getBuyCost(%aiName, %item) * %delta * -1;
		}
		else if(%buyORsell == SELL)
		{
			$MerchantCounterS[%aiName, %item] += %delta;
			%cost = getSellCost(%aiName, %item) * %delta;
		}

		GiveThisStuff(%Client, "COINS "@%cost, False);
	}

	%txt = "<f1>GIL: "@FixM($COINS[%Client]);
	Client::setInventoryText(%Client, %txt);
}


//============================================================================================
//=========================== DO NOT EDIT WITHOUT ASKING ME ===============================
//============================================================================================
function SetupBank(%Client, %id) {

	%Client.currentShop = "";
	%Client.currentBank = %id;
	%Client.currentLoot = "";
	%Client.currentSmith = "";

	%txt = "<f1>GIL: "@FixM($COINS[%Client]);
	Client::setInventoryText(%Client, %txt);

	%info = $BankStorage[%Client];

	%items = "";
	%ii = 0;
	for(%i = 0; (%item = GetWord(%info, %i)) != -1; %i += 2) {
		%items = %items@%item@" ";
		%ii++;
		if(%ii >= 10) {
			remoteEval(%Client, "GiveClientShoppingData", %items);
			%ii = 0;
			%items = "";
		}
	}
	remoteEval(%Client, "GiveClientShoppingData", %items, true);

	Client::setGuiMode(%Client, 4);
}

function SetupShop(%Client, %id, %i) {

	%Client.currentShop = %id;
	%Client.currentBank = "";
	%Client.currentLoot = "";
	%Client.currentSmith = "";

	%txt = "<f1>GIL: "@FixM($COINS[%Client]);
	Client::setInventoryText(%Client, %txt);

	%info = $TownBot[%id, SHOP];

	if(%i != "")
		%info = $TownBot[%id, SHOP, %i];

	%items = "";
	%ii = 0;
	for(%i = 0; (%item = GetWord(%info, %i)) != -1; %i++) {
		%items = %items@%item@" ";
		%ii++;
		if(%ii >= 10) {
			remoteEval(%Client, "GiveClientShoppingData", %items);
			%ii = 0;
			%items = "";
		}
	}
	remoteEval(%Client, "GiveClientShoppingData", %items, true);

	Client::setGuiMode(%Client, 4);
}

function SetUpLootShop(%Client, %id, %loot) { //LootShop


	%Client.currentShop = "";
	%Client.currentBank = "";
	%Client.currentLoot = %id;
	%Client.currentSmith = "";

	%items = "";
	%ii = 0;
	for(%i = 0; (%item = GetWord(%loot, %i)) != -1; %i += 2) {
		%items = %items@%item@" ";
		%ii++;
		if(%ii >= 10) {
			remoteEval(%Client, "GiveClientShoppingData", %items);
			%ii = 0;
			%items = "";
		}
	}

	remoteEval(%Client, "GiveClientShoppingData", %items, true);

//echo("LootShopItems:"@%items);
	if(getWord($loot[%id], 1) == -1) {
		Item::playPickupSound(%id);
		deleteObject(%id);
		$loot[%id] = "";
		$ClientData[$loottag[%id], OwnsLoot]--;
		$loottag[%id] = "";

		remotePlayMode(%Client);
		return;
	}

	Client::setGuiMode(%Client, 4);
	%txt = "<f1>GIL: "@FixM($COINS[%Client]);
	Client::setInventoryText(%Client, %txt);
}

function SetupBlackSmith(%Client, %id) {
	%Client.currentShop = "";
	%Client.currentBank = "";
	%Client.currentLoot = "";
	%Client.currentSmith = %id;

	%mat = getword($ClientData[%client, tmpSmith],0);
	if(%mat == -1 || $ClientData[%Client, SmithStage] == "canSmith")
		%txt = "<f1>GIL: "@FixM($COINS[%Client]);
	else	
		%txt = "<f1>Mat: "@%mat;
	Client::setInventoryText(%Client, %txt);

	%items = "";

	if($ClientData[%Client, SmithMode] == 1 && $ClientData[%Client, SmithStage] == "part") {
		%part = $ItemData[%mat, FixCaps];
		%smith = AddItemSpecificPoints(%part, "SMITH");
		%type = $ItemData[%mat, type];
	
		for(%i=1;%i<=100;%i+=10)
		{
			%s = %i + 9;
			for(%n=%i;$Smith::Stuff[%n]!=""&&%n<=%s;%n++)
			{
				%itemM = $Smith::Stuff[%n];
				%attach = $Smith::Mod::Attach[$Smith::Stuff[%n]];
				if(%attach != "")
					%itemM = $Smith::Stuff[$Smith::Stuff[%attach]]; //$Smith::Mod[%attach];

				%var = %smith;
				%var *= floor($Smith::Mod::Stuff[%itemM, $Smith::Stuff[%type]]/100);

				if(%var>0)
					%list[%i] = %list[%i] @ " Shape:_" @ $Smith::Stuff[%n];
			}
		}
		for(%i=1;%i<=100;%i+=10)
		{
			if(%list[%i] != "")
				remoteEval(%Client, "GiveClientShoppingData", %list[%i]);
		}
	}
	else if($ClientData[%Client, SmithMode] == 2) {
		%info = $ClientData[%Client, tmpSmith];

		%ii = 0;
		for(%i = 0; (%item = GetWord(%info, %i)) != -1; %i++) {

			%i++;
			%ii++;
			%items = %items@%item@" ";
			if(%ii >= 10) {
				remoteEval(%Client, "GiveClientShoppingData", %items);
				%ii = 0;
				%items = "";
			}
		}
	}


	remoteEval(%Client, "GiveClientShoppingData", %items, true);

	Client::setGuiMode(%Client, 4);
}

//============================================================================================
//============================================================================================
//============================================================================================


function Client::isShoppingOn(%Client, %ids, %idb, %idl, %idbs) {

	if(%ids != "")
		%id = %ids;
	else if(%idb != "")
		%id = %idb;
	else if(%idl != "")
		%id = %idl;
	else if(%idbs != "")
		%id = %idbs;

	if(%id != "") {
		if(Vector::getDistance(GameBase::getPosition(%Client), GameBase::getPosition(%id)) > 10) {
			remotePlayMode(%Client);
			return false;
		}
		return true;
	}
	return false;
}

function buyItem(%Client, %item, %bulk) {

	if(IsDead(%Client))
		return;

	%player = Client::getOwnedObject(%Client);

	if(Client::isShoppingOn(%Client, %Client.currentShop, %Client.currentBank, %Client.currentLoot, %Client.currentSmith)) {

		if(%bulk >= %Client.bulk)
			%Client.bulk = %bulk;


		%item = getCroppedItem($ItemData[%item, FixCaps]);

		if(%item == "")
			return;

		//Still thinking if equipped items should be included in this count.
		if(!Client::HasItem(%Client, %item)) {
			if(Client::getItemListCount(%Client, "ItemList") >= $Item::MaxItemListCount) {
				Client::sendMessage(%Client, 0, "You can't hold any more different types of items.");
				remotePlayMode(%Client);
				return 0;
			}
		}
		if(%Client.currentBank != "") {
			//============================================================
			//  Player is at a bank, removing from his/her bank storage
			//============================================================

			%itemcnt = 0;
			%id = -1;

			%pos = String::findSubStr($BankStorage[%Client], %item@" ");
			if(%pos != -1) {
				%data = String::NEWgetSubStr($BankStorage[%Client], %pos, 60);
				%id = getWord(%data, 0);
				%itemcnt = floor(GetWord(%data, 1));
			}

			%cnt = Client::getItemCount(%Client, %item);
			%Client.bulk = Cap(%Client.bulk, 0, %itemcnt); //Cap so you can get up to your limit
			%Client.bulk = Cap(%Client.bulk, 0, (99 - %cnt));

			if(%Client.bulk == 0 && %id != -1) {
		//		if(String::findSubStr($ArrowList, %item) != -1) { // check if player wants more arrows
		//			if(String::findSubStr($Quiver[%Client], "FreeSlot") != -1) { //Has an empty quiver
		//				$Quiver[%Client] = String::replace($Quiver[%Client], "FreeSlot", %item);
		//				Client::addItemCount(%Client, %item, 1); //Get a free arrow =p
		//				Client::sendMessage(%Client, 0, "You filled up a quiver and start filling up a new one");
		//			}
		//			else
		//				Client::sendMessage(%Client, $MsgWhite, %aiName@" tells you, \"You have all your quivers filled up, talk to a merchant to buy a new one.\"");
//
			//	}
			//	else
					Client::sendMessage(%Client, $MsgWhite, "You can't carry anymore "@$ItemData[%item, Name]@"s!");
			}
			else {
				if(%id != -1) {
					Client::addItemCount(%Client, %item, %Client.bulk);
					$BankStorage[%Client] = SetStuffString($BankStorage[%Client], %item, -%Client.bulk);
				}
			}
			%Client.bulk = 1;
			SetupBank(%Client, %Client.currentBank);	//refresh
			RefreshAll(%Client);
			return 0;
		}
		else if(%Client.currentShop != "") {
			//=========================================
			//  Player is at a regular shop
			//=========================================

			%aiName = $TownBot[%Client.currentShop, NAME];
			%list = $TownBot[%Client.currentShop, SHOP]@" ";
			if(String::findSubStr(%list, %item@" ") == -1) {
				Client::sendMessage(%Client, 0, %aiName@" tells you, \"I do not have that item.\"");
				return;
			}

			if($LastClickItemB[%Client, %item] != %item) {
				%cost = getBuyCost(%aiName, %item);
				Client::sendMessage(%Client, $MsgWhite, %aiName@" tells you, \"This "@$ItemData[%item, Name]@" will cost you "@FixM(%cost)@" gil.\"");
				$LastClickItemB[%Client, %item] = %item;
				schedule("$LastClickItemB[" @ %Client @ ", " @ %item @ "] = \"\";", 5);

				%Client.bulk = 1;
				return 0;
			}

			else {

				%cnt = Client::getItemCount(%Client, %item);
				%Client.bulk = Cap(%Client.bulk, 0, (99 - %cnt)); //Caps so you can buy up to your limit

				if(%Client.bulk == 0) { //Already has 99
				//	if(String::findSubStr($ArrowList, %item) != -1) { //So check if player wants more arrows
				//		if(String::findSubStr($Quiver[%Client], "FreeSlot") != -1) { //Has an empty quiver
				//			$Quiver[%Client] = String::replace($Quiver[%Client], "FreeSlot", %item);
				//			Client::addItemCount(%Client, %item, 1); //Get a free arrow =P
				//			Client::sendMessage(%Client, 0, "You filled up a quiver and start filling a new one.");
				//		}
				//		else {
				//			%cost = 10000 * $QuiverCnt[%Client];
				//			Client::sendMessage(%Client, $MsgWhite, %aiName@" tells you, \"You have all your quivers filled up, would you like to buy a new one for "@FixM(%cost)@"?\" [yes]");
				//			$state[%Client.currentShop, %Client] = 2;
				//		}
				//	}
				//	else {
						Client::sendMessage(%Client, $MsgWhite, "You can't carry anymore "@$ItemData[%item, Name]@"s!");
						return;
				//	}
				}
				if(checkResources(%player, %item, %Client.bulk) && !IsDead(%Client) && %Client.bulk > 0) {
					Client::addItemCount(%Client, %item, %Client.bulk);
					BuySell(%player, %item, %Client.bulk, BUY);
					playSound(SoundMoney1, GameBase::getPosition(%Client));
					RefreshAll(%Client);
					%Client.bulk = 1;
					return 1;
				}
			}
		}
		else if(%Client.currentLoot) {
			//============================================================
			//  Player is on a pack removing items from someones' loot
			//============================================================
			%this = $ClientData[%Client, Looting];
			%list = $loot[%this];
//echo("db: shop: lootbag: '"@$loot[%this]@"'");

			%itemcnt = 0;
			%id = -1;

			%pos = String::findSubStr(" "@%list@" ", " "@%item@" ");

			if(%pos != -1) {
				%data = String::NEWgetSubStr(%list, %pos, 60);
				%id = getWord(%data, 0);
				%itemcnt = floor(GetWord(%data, 1));
			}

			%cnt = Client::getItemCount(%Client, %item);
			%Client.bulk = Cap(%Client.bulk, 0, %itemcnt); //Cap so you can get up to your limit
			%Client.bulk = Cap(%Client.bulk, 0, (99 - %cnt));

			if(%Client.bulk == 0 && %id != -1) {
			//	if(String::findSubStr($ArrowList, %item) != -1) { // check if player wants more arrows
			//		if(String::findSubStr($Quiver[%Client], "FreeSlot") != -1) { //Has an empty quiver
			//			$Quiver[%Client] = String::replace($Quiver[%Client], "FreeSlot", %item);
			//			Client::addItemCount(%Client, %item, 1); //Get a free arrow =p
			//			Client::sendMessage(%Client, 0, "You filled up a quiver and start filling up a new one.");
			//		}
			//		else
			//			Client::sendMessage(%Client, $MsgWhite, "You don't have a empty quiver.");
			//	}
			//	else {
					Client::sendMessage(%Client, $MsgWhite, "You can't carry anymore "@$ItemData[%item, Name]@"s!");
					return;
			//	}
			}
			else {
				if(%id != -1) {
					Client::addItemCount(%Client, %item, %Client.bulk);
					$loot[%this] = SetStuffString(%list, %item, -%Client.bulk);
				}
			}

//echo("db: shop2: lootbag: '"@$loot[%this]@"'");
			%Client.bulk = 1;
			SetupLootShop(%Client, %Client.currentLoot, String::NEWgetSubStr($loot[%this], String::len(GetWord($loot[%this], 0))+1, 99999));	//refresh
			RefreshAll(%Client);
			return 1;
		}
		else if(%Client.currentSmith != "") {
			//=========================================
			//  Player is at a blacksmith
			//=========================================
			SmithClick(%Client, %item, -%Client.bulk);
		}
 	}
	return 0;
}

function sellItem(%Client, %item, %bulk) {

	if(IsDead(%Client))
		return;

	%player = Client::getOwnedObject(%Client);

	if(Client::isShoppingOn(%Client, %Client.currentShop, %Client.currentBank, %Client.currentLoot, %Client.currentSmith)) {

		if(%bulk >= %Client.bulk)
			%Client.bulk = %bulk;

		%item = getCroppedItem($ItemData[%item, FixCaps]);


		if(!Client::HasItem(%Client, %item) || %item == "") {
			return false;
		}

		if(%Client.currentBank != "") {

			//============================================================
			//  Player is at a bank, adding to his/her bank storage
			//============================================================

			if(String::findSubStr($BankStorage[%Client], %item@" ") == -1) {
				if(BankCountList(%Client) >= 40) {
					Client::sendMessage(%Client, 0, "You do not have any room left in your Storage.");
					return 0;
				}
			}

			if($ItemData[%item, className] != Equipped) {

				%ItemCnt = Client::getItemCount(%Client, %item);
				%Client.bulk = Cap(%Client.bulk, 0, %ItemCnt);

				Client::addItemCount(%Client, %item, -%Client.bulk);
				$BankStorage[%Client] = SetStuffString($BankStorage[%Client], %item, %Client.bulk);

				if(Client::getItemCount(%Client, %item) <= 0) {
				//	if(String::findSubStr($ArrowList, %item) != -1) { // check if player wants to store arrows
				//		if(String::findSubStr($Quiver[%Client], %item) != -1) { //Has an quiver filled with this item
//
				//			$Quiver[%Client] = String::replace($Quiver[%Client], %item, "FreeSlot");
				//			Client::addItemCount(%Client, %item, 99);
				//			Client::sendMessage(%Client, 0, "You get out an quiver filled with "@$ItemData[%item, Name]@".");
				//		}
				//	}
				}

				SetupBank(%Client, %Client.currentBank);	//refresh

				RefreshAll(%Client);
			}
			else
				Client::sendMessage(%Client, $MsgRed, "Unequip this item before storing it.~wC_BuySell.wav");

			%Client.bulk = 1;
			CheckMountWeapon(%Client, %item);

			return 1;
		}
		else if(%Client.currentShop != "") {

			//=========================================
			//  Player is at a regular shop
			//=========================================

			%aiName = $TownBot[%Client.currentShop, NAME];

			if($LastClickItemS[%Client, %item] != %item)
			{
			//	%aiName = $BotInfoAiName[%Client.currentShop];
				%cost = getSellCost(%aiName, %item);
				Client::sendMessage(%Client, $MsgWhite, %aiName@" tells you, I will give you "@FixM(%cost)@" gil for the "@$ItemData[%item, Name]@".");

				$LastClickItemS[%Client, %item] = %item;
				schedule("$LastClickItemS[" @ %Client @ ", " @ %item @ "] = \"\";", 5);
		//		%Client.bulk = 1;

				return 0;
			}
			else
			{
				%itemCnt = Client::getItemCount(%Client, %item);
				if($ItemData[%item, className] == Equipped) {
					Client::sendMessage(%Client, $MsgRed, "You cannot sell an equipped item.~wC_BuySell.wav");
				}
				else if(%itemCnt > 0) {

					if(%Client.bulk > %itemCnt)
						%Client.bulk = %itemCnt;

					Client::addItemCount(%Client, %item, -%Client.bulk);
					BuySell(%player, %item, %Client.bulk, SELL);

					if(Client::getItemCount(%Client, %item) <= 0) {
				//		if(String::findSubStr($ArrowList, %item) != -1) { // check if player wants to sell arrows
				//			if(String::findSubStr($Quiver[%Client], %item) != -1) { //Has an quiver filled with this item
//
				//				$Quiver[%Client] = String::replace($Quiver[%Client], %item, "FreeSlot");
				//				Client::addItemCount(%Client, %item, 99);
				//				Client::sendMessage(%Client, 0, "You get out an quiver filled with "@$ItemData[%item, Name]@"s.");
				//			}
				//		}
					}

					playSound(SoundMoney1, GameBase::getPosition(%Client));

					RefreshAll(%Client);

					%Client.bulk = 1;
					CheckMountWeapon(%Client, %item);

					return 1;
				}
			}
		}
		else if(%Client.currentSmith != "") {
			//=========================================
			//  Player is at a blacksmith
			//=========================================
			SmithClick(%Client, %item, %Client.bulk);
		}
	}
	return 0;
}

function remoteBuyItem(%Client, %type, %bulk) {


	%time =  getIntegerTime(true) >> 5;
	if((%time - %Client.lastBuySellTime) > 0.9) {
		%Client.lastBuySellTime = %time;

		if($ItemData[%type, DataName] != "") {

			%type = NEWgetItemType(%type);

			%bulk = Cap(floor(%bulk), 1, 99);

			buyItem(%Client, %type, %bulk);
		}
	}
}

function remoteSellItem(%Client, %type, %bulk) {

	%time =  getIntegerTime(true) >> 5;
	if((%time - %Client.lastBuySellTime) > 0.9) {
		%Client.lastBuySellTime = %time;

		if($ItemData[%type, DataName] != "") {

			%item = NEWgetItemType(%type);

			%bulk = Cap(floor(%bulk), 1, 99);

			sellItem(%Client, %item, %bulk);
		}
	}
}

function checkResources(%player, %item, %delta, %noMessage) {

	%Client = Player::getClient(%player);

	if($ItemCost[%item] == "")
		echo("Error: "@$ItemData[%item, Name]@" has no cost!");

	if($ItemCost[%item] * %delta > $COINS[%Client] && %Client.adminLevel < 4)
	{
		if(%noMessage == "")
			Client::sendMessage(%Client, $MsgRed, "You cannot afford the " @ $ItemData[%item, Name] @ ".~wC_BuySell.wav");
		return 0;
	}
	return 1;
}
