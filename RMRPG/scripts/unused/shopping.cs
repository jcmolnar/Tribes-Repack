function SetupShop(%clientId, %aiId)
{
	dbecho($dbechoMode, "SetupShop(" @ %clientId @ ", " @ %aiId @ ")");

	%clientId.currentShop = %aiId;
	%clientId.currentBank = "";

	Client::clearItemShopping(%clientId);
	Client::clearItemBuying(%clientId);

	Client::setGuiMode(%clientId, 4);

	%txt = "<f1><jc>GIL: " @ $COINS[%clientId];
	Client::setInventoryText(%clientId, %txt);

	%info = $BotInfo[$BotInfoAiName[%aiId], SHOP];

	for(%i = 0; GetWord(%info, %i) != -1; %i++)
	{
		%a = GetWord(%info, %i);

		%max = getNumItems();
		for(%z = 0; %z < %max; %z++)
		{
			%item = getItemData(%z);

			if($AccessoryVar[%item, $ShopIndex] == %a)
			{
				Client::setItemShopping(%clientId, %item);
				Client::setItemBuying(%clientId, %item);
			}
		}
	}
}

function SetupBank(%clientId, %aiId)
{
	dbecho($dbechoMode, "SetupBank(" @ %clientId @ ", " @ %aiId @ ")");

	%clientId.currentShop = "";
	%clientId.currentBank = %aiId;

	Client::clearItemShopping(%clientId);
	Client::clearItemBuying(%clientId);

	if(Client::getGuiMode(%clientId) != 4)
		Client::setGuiMode(%clientId, 4);

	%txt = "<f1><jc>GIL: " @ $COINS[%clientId];
	Client::setInventoryText(%clientId, %txt);

	%info = $BankStorage[%clientId];

	for(%i = 0; GetWord(%info, %i) != -1; %i+=2)
	{
		%item = GetWord(%info, %i);

		Client::setItemShopping(%clientId, %item);
		Client::setItemBuying(%clientId, %item);
	}
}

function SetupBlacksmith(%clientId, %id)
{
	dbecho($dbechoMode, "SetupBlacksmith(" @ %clientId @ ", " @ %id @ ")");

	%clientId.currentSmith = %id;

	%clientId.bulkNum = "";

	Client::clearItemShopping(%clientId);
	Client::clearItemBuying(%clientId);

	if(Client::getGuiMode(%clientId) != 4)
		Client::setGuiMode(%clientId, 4);

	%info = fetchData(%clientId, "TempSmith");

	for(%i = 0; GetWord(%info, %i) != -1; %i+=2)
	{
		%item = GetWord(%info, %i);

		Client::setItemShopping(%clientId, %item);
		Client::setItemBuying(%clientId, %item);
	}

	%txt = "<f1><jc>GIL: " @ fetchData(%clientId, "COINS");
	Client::setInventoryText(%clientId, %txt);
}
