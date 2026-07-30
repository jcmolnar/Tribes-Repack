function getBuyCost(%clientId, %item)
{
	dbecho($dbechoMode, "getBuyCost(" @ %clientId @ ", " @ %item @ ")");

	%aiName = fetchData(%clientId.currentShop, "BotInfoAiName");

	%p = GetItemCost(%item);
	if($NewItemBuyCost[%aiName, %item] != "")
		%cost = $NewItemBuyCost[%aiName, %item];
	else
		%cost = %p;

	%p = round($PlayerSkill[%clientId, $SkillCharisma] / 20) / 100;
	%x = round(%cost * Cap(%p, 0.0, 0.5) );
	%cost -= %x;

	return %cost;
}
function getSellCost(%clientId, %item)
{
	dbecho($dbechoMode, "getSellCost(" @ %clientId @ ", " @ %item @ ")");

	%aiName = fetchData(%clientId.currentShop, "BotInfoAiName");

	%p = GetItemCost(%item);
	if($NewItemSellCost[%aiName, %item] != "")
		%cost = $NewItemSellCost[%aiName, %item];
	else
		%cost = round(%p * ($resalePercentage/100));

	%p = round($PlayerSkill[%clientId, $SkillCharisma] / 11) / 100;
	%x = round(%cost * Cap(%p, 0.0, 1.0) );
	%cost += %x;

	return %cost;
}
function GetItemCost(%item)
{
	dbecho($dbechoMode, "GetItemCost(" @ %item @ ")");

	if($HardcodedItemCost[%item] != "")
		return $HardcodedItemCost[%item];

	return $ItemCost[%item];		
}

function BuySell(%player, %item, %delta, %buyORsell)
{
	dbecho($dbechoMode, "BuySell(" @ %player @ ", " @ %item @ ", " @ %delta @ ", " @ %buyORsell @ ")");

	// IF - Cost positive selling    IF - Cost Negative buying 

	%clientId = Player::getClient(%player);
	%station = %player.Station;
	%stationName = GameBase::getDataName(%station); 

	if(%clientId.adminLevel < 4)
	{
		//Add entry to merchant counter.
		//Admin purchases do not count towards the economy.
		%aiName = fetchData(%clientId.currentShop, "BotInfoAiName");
		if(%buyORsell == BUY)
		{
			$MerchantCounterB[%aiName, %item] += %delta;
			%cost = getBuyCost(%clientId, %item) * %delta * -1;
		}
		else if(%buyORsell == SELL)
		{
			$MerchantCounterS[%aiName, %item] += %delta;
			%cost = getSellCost(%clientId, %item) * %delta;
		}
		UseSkill(%clientId, $SkillCharisma, True, True);

		storeData(%clientId, "COINS", %cost, "inc");
	}

	%txt = "<f1><jc>CREDITS: " @ fetchData(%clientId, "COINS");
	Client::setInventoryText(%clientId, %txt);
}

function buyItem(%clientId, %item)
{
	dbecho($dbechoMode, "buyItem(" @ %clientId @ ", " @ %item @ ")");

	if(IsDead(%clientId))
		return;

	%player = Client::getOwnedObject(%clientId);

	if(Client::isItemShoppingOn(%clientId, %item))
	{
		if(%clientId.currentBank != "")
		{
			//============================================================
			//  Player is at a bank, removing from his/her bank storage
			//============================================================
			if(%clientId.bulkNum != "")
				%n = %clientId.bulkNum;
			else
				%n = 1;
			%cnt = GetStuffStringCount(fetchData(%clientId, "BankStorage"), %item);
			if(%cnt >= %n)
			{
				Player::incItemCount(%clientId, %item, %n);
				storeData(%clientId, "BankStorage", SetStuffString(fetchData(%clientId, "BankStorage"), %item, -%n));
	
				SetupBank(%clientId, %clientId.currentBank);	//refresh

				RefreshAll(%clientId);
			}
			else
				Client::sendMessage(%clientId, $MsgRed, "You only have " @ %cnt @ " of this item.~wC_BuySell.wav");
		}
		else if(%clientId.currentShop != "")
		{
			//=========================================
			//  Player is at a regular shop
			//=========================================

			%cost = getBuyCost(%clientId, %item);
			if($LastClickItemB[%clientId, %item] != %item)
			{
				Client::sendMessage(%clientId, $MsgWhite, "The " @ %item.description @ " will cost you " @ %cost @ " coins.");

				$LastClickItemB[%clientId, %item] = %item;
				schedule("$LastClickItemB[" @ %clientId @ ", " @ %item @ "] = \"\";", 5);
				%clientId.bulkNum = 1;

				return 0;
			}
			else
			{
				if(checkResources(%player,%item,%cost,%clientId.bulkNum) && !IsDead(%clientId))
				{
					Player::incItemCount(%clientId, %item, %clientId.bulkNum);
					BuySell(%player, %item, %clientId.bulkNum, BUY);
		
					RefreshAll(%clientId);
					%clientId.bulkNum = 1;

					return 1;
				}
			}
		}
		else if(%clientId.currentInvSteal != "")
		{
			//=========================================
			//  Player is trying to pickpocket/mug
			//=========================================
			%cl = %clientId.currentInvSteal;

			%itemweight = GetAccessoryVar(%item, $Weight);

			if(Vector::getDistance(GameBase::getPosition(%clientId), GameBase::getPosition(%cl)) > 2)
			{
				remotePlayMode(%clientId);
				Client::sendMessage(%clientId, $MsgWhite, "Your target has wandered off...");
			}
			else if(%clientId.TryingToSteal)
			{
				Client::sendMessage(%clientId, $MsgRed, "You are already trying to steal an item...");
			}
			else
			{
				if(%clientId.stealType == 1)
				{
					%max = 10;
					%w = "pickpocket";
					%canStealEquipped = False;
					%weightToTimeFactor = 0.6;
				}
				else if(%clientId.stealType == 2)
				{
					%max = 99999;
					%w = "mug";
					%canStealEquipped = True;
					%weightToTimeFactor = 0.55;
				}

				%eflag = "";
				%fitem = %item;
				if(%item.className == Equipped)
				{
					%fitem = String::getSubStr(%item, 0, String::len(%item)-1);
					if(%canStealEquipped)
						%eflag = True;
				}
				else
					%eflag = True;

				if(%eflag)
				{
					if(%itemweight <= %max)
					{
						if(!$StealProtectedItem[%item])
						{
							%icnt = Player::getItemCount(%cl, %item);
							if(%icnt)
							{
								Client::sendMessage(%clientId, $MsgBeige, "Attempting to " @ %w @ "...");
	
								%a = %itemweight * %weightToTimeFactor;
								%b = (1000 - $PlayerSkill[%clientId, $SkillStealing]) / 50;
								%c = Cap(%b, 0, "inf");
								%d = %c * %weightToTimeFactor;
	
								%time = %a + %d;
	
								%clientId.TryingToSteal = True;
								schedule("DoSteal(" @ %clientId @ ", " @ %cl @ ", " @ %icnt @ ", " @ %item @ ", " @ %fitem @ ", \"" @ %w @ "\");", %time);
							}
						}
						else
							Client::sendMessage(%clientId, $MsgWhite, "This item is magically protected from your thieving hands...");
					}
					else
						Client::sendMessage(%clientId, $MsgWhite, "This item is too heavy to " @ %w @ "...");
				}
				else
					Client::sendMessage(%clientId, $MsgWhite, "You can't " @ %w @ " an equipped item...");
			}
		}
		else if(%clientId.currentSmith != "")
		{
			//=================================================
			//  Player is at a blacksmith unselecting an item
			//=================================================
			BlackSmithClick(%clientId, %item, -1);
		}
		else
		{
			storeData(%clientId, "TempPack", SetStuffString(fetchData(%clientId, "TempPack"), %item, -1));
			SetupCreatePack(%clientId);
		}
 	}

	return 0;
}
function DoSteal(%clientId, %cl, %itemcount, %item, %fitem, %w)
{
	dbecho($dbechoMode, "DoSteal(" @ %clientId @ ", " @ %cl @ ", " @ %itemcount @ ", " @ %item @ ", " @ %fitem @ ", " @ %w @ ")");

	%victimName = Client::getName(%cl);
	%stealerName = Client::getName(%clientId);

	%icnt = Player::getItemCount(%cl, %item);

	%clientId.TryingToSteal = "";

	//weights
	%itemweight = GetAccessoryVar(%item, $Weight);
	%wweight = 10;
	if(Player::getMountedItem(%clientId, $WeaponSlot) == %item || %item.className == Equipped)
		%handweight = 5.0;
	else
		%handweight = 1.0;
	%FailWeight = (%itemweight * %wweight) * %handweight;

	if(Vector::getDistance(GameBase::getPosition(%clientId), GameBase::getPosition(%cl)) > 2)
		Client::sendMessage(%clientId, $MsgWhite, "Your target has wandered off...");
	else
	{
		%r1 = GetRoll("1d" @ $PlayerSkill[%clientId, $SkillStealing]);
		%r2 = GetRoll("1d" @ ($PlayerSkill[%cl, $SkillStealing] + $PlayerSkill[%cl, $SkillDodging]));
		%b = %r1 - %r2;
		%a = %b - %FailWeight;
		if(%a > 0 && %itemcount == %icnt)
		{
			Client::sendMessage(%clientId, $MsgBeige, "You successfully stole a " @ %fitem.description @ " from " @ %victimName @ "!");
			Player::decItemCount(%cl, %item, 1);
			Player::incItemCount(%clientId, %fitem, 1);
			PerhapsPlayStealSound(%clientId, %clientId.stealType);
		
			SetupInvSteal(%clientId, %cl);
					
			RefreshAll(%clientId);
			RefreshAll(%cl);
				
			UseSkill(%clientId, $SkillStealing, True, True);
			PostSteal(%clientId, True, %clientId.stealType);

			return True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgRed, "You failed to " @ %w @ " " @ %victimName @ "!");
			Client::sendMessage(%cl, $MsgRed, %stealerName @ " just failed to " @ %w @ " you!");
	
			UseSkill(%clientId, $SkillStealing, False, True);
			PostSteal(%clientId, False, %clientId.stealType);
		}
	}

	//force kick from Inv Screen
	remotePlayMode(%clientId);

	return False;
}

function HasTheftFlag(%clientId)
{
	%b = AddBonusStatePoints(%clientId, "Theft");

	if(%b >= 1)
		return True;
	else
		return False;
}

function remoteBuyItem(%clientId, %type)
{
	dbecho($dbechoMode, "remoteBuyItem(" @ %clientId @ ", " @ %type @ ")");

	%time = getIntegerTime(true) >> 5;
	if(%time - %clientId.lastWaitActionTime > $waitActionDelay)
	{
		%clientId.lastWaitActionTime = %time;

		%item = getItemData(%type);
		buyItem(%clientId, %item);
	}
}

function sellItem(%clientId, %item)
{
	dbecho($dbechoMode, "sellItem(" @ %clientId @ ", " @ %item @ ")");

	if(IsDead(%clientId))
		return;

	%player = Client::getOwnedObject(%clientId);

	if(%clientId.currentShop != "" || %clientId.currentBank != "" || %clientId.currentSmith != "")
	{
		if(%clientId.currentBank != "")
		{
			//============================================================
			//  Player is at a bank, adding to his/her bank storage
			//============================================================
			if(CountObjInList(fetchData(%clientId, "BankStorage")) / 2 < 25)
			{
				if(%clientId.bulkNum != "")
					%n = %clientId.bulkNum;
				else
					%n = 1;
				%cnt = Player::getItemCount(%clientId, %item);
				if(%cnt >= %n)
				{
					if(%item.className != Equipped)
					{
						Player::decItemCount(%clientId, %item, %n);
						storeData(%clientId, "BankStorage", SetStuffString(fetchData(%clientId, "BankStorage"), %item, %n));
		
						SetupBank(%clientId, %clientId.currentBank);	//refresh
		
						RefreshAll(%clientId);
					}
					else
						Client::sendMessage(%clientId, $MsgRed, "Unequip this item before storing it.~wC_BuySell.wav");
				}
				else
					Client::sendMessage(%clientId, $MsgRed, "You only have " @ %cnt @ " of this item.~wC_BuySell.wav");
	
				return 1;
			}
			else
				Client::sendMessage(%clientId, $MsgRed, "You can only store 25 different types of items.~wC_BuySell.wav");
		}
		else if(%clientId.currentShop != "")
		{
			%nitem = getCroppedItem(%item);

			//=========================================
			//  Player is at a regular shop
			//=========================================
			if($LastClickItemS[%clientId, %item] != %item)
			{
				%cost = getSellCost(%clientId, %item);
				Client::sendMessage(%clientId, $MsgWhite, "This merchant will give you " @ %cost @ " coins for the " @ %nitem.description @ ".");

				$LastClickItemS[%clientId, %nitem] = %nitem;
				schedule("$LastClickItemS[" @ %clientId @ ", " @ %nitem @ "] = \"\";", 5);
				%clientId.bulkNum = 1;

				return 0;
			}
			else
			{
				%itemCnt = Player::getItemCount(%clientId, %item);
				if(%item.className == Equipped)
				{
					Client::sendMessage(%clientId, $MsgRed, "You cannot sell an equipped item.~wC_BuySell.wav");
				}
				else if(%itemCnt && !IsDead(%clientId))
				{
					if(%clientId.bulkNum > %itemCnt)
						%clientId.bulkNum = %itemCnt;

					if($LoreItem[%item])
						Client::sendMessage(%clientId, $MsgRed, "(You have sold a lore item)");
	
					%count = Player::getItemCount(%clientId, %item);
					%numsell = %clientId.bulkNum;
	
					BuySell(%player, %item, %clientId.bulkNum, SELL);
					Player::setItemCount(%player, %item, (%count-%numsell));
					Client::SendMessage(%clientId, $MsgWhite, "~wbuysellsound.wav");
	
					RefreshAll(%clientId);
					%clientId.bulkNum = 1;

					return 1;
				}
			}
		}
		else if(%clientId.currentSmith != "")
		{
			//=========================================
			//  Player is at a blacksmith
			//=========================================
			BlackSmithClick(%clientId, %item, 1);
		}
	}
	else
	{
		if(%item.className != Equipped && !$LoreItem[%item])
		{
			storeData(%clientId, "TempPack", SetStuffString(fetchData(%clientId, "TempPack"), %item, 1));
			SetupCreatePack(%clientId);
		}
		else
			Client::sendMessage(%clientId, $MsgRed, "You can't select this item.~wC_BuySell.wav");
	}

	return 0;
}
function remoteSellItem(%clientId, %type)
{
	dbecho($dbechoMode, "remoteSellItem(" @ %clientId @ ", " @ %type @ ")");

	%time = getIntegerTime(true) >> 5;
	if(%time - %clientId.lastWaitActionTime > $waitActionDelay)
	{
		%clientId.lastWaitActionTime = %time;

		%item = getItemData(%type);
		sellItem(%clientId, %item);
	}
}


function checkResources(%player, %item, %cost, %delta, %noMessage)
{
	dbecho($dbechoMode, "checkResources(" @ %player @ ", " @ %item @ ", " @ %cost @ ", " @ %delta @ ", " @ %noMessage @ ")");

	%clientId = Player::getClient(%player);

	if(%cost * %delta > fetchData(%clientId, "COINS") && %clientId.adminLevel < 4)
	{
		if(%noMessage == "")
			Client::sendMessage(%clientId, $MsgRed, "You cannot afford the " @ %item.description @ ".~wC_BuySell.wav");
		return 0;
	}
	return 1;
}

function CompleteSmith(%clientId, %cost, %sc, %tempsmith, %multiplier)
{
	dbecho($dbechoMode, "CompleteSmith(" @ %clientId @ ", " @ %cost @ ", " @ %sc @ ", " @ %tempsmith @ ", " @ %multiplier @ ")");

	%clientId.IsSmithing = "";

	if(fetchData(%clientId, "COINS") < %cost)
		return;

	storeData(%clientId, "COINS", %cost, "dec");
	playSound(SoundMoney1, GameBase::getPosition(%clientId));
	GiveThisStuff(%clientId, $SmithComboResult[%sc], True, %multiplier);

	for(%i = 0; (%w = GetWord(%tempsmith, %i)) != -1; %i+=2)
	{
		%w2 = GetWord(%tempsmith, %i+1) * %multiplier;
		storeData(%clientId, "BankStorage", SetStuffString(fetchData(%clientId, "BankStorage"), %w, -%w2));
	}

	AI::sayLater(%clientId, %clientId.currentSmith, "Here you go.", True);
}

function BlackSmithClick(%clientId, %item, %delta)
{
	dbecho($dbechoMode, "BlackSmithClick(" @ %clientId @ ", " @ %item @ ", " @ %delta @ ")");

	if(%clientId.IsSmithing)
	{
		Client::sendMessage(%clientId, $MsgRed, "The blacksmith is busy...");
	}
	else
	{
		if(%item.className != Equipped)
		{
			storeData(%clientId, "TempSmith", SetStuffString(fetchData(%clientId, "TempSmith"), %item, %delta));
			SetupBlacksmith(%clientId, %clientId.currentSmith);

			%tempsmith = LTrim(fetchData(%clientId, "TempSmith"));
			if((%sc = GetSmithCombo(%tempsmith)) != 0)
			{
				%cost = GetSmithComboCost(%clientId, %sc);

				Client::sendMessage(%clientId, $MsgWhite, "It will cost you " @ %cost @ " coins to smith these items.~wcanSmith.wav");
				Client::sendMessage(%clientId, $MsgBeige, "(type #smith to accept the cost and start smithing)");

				return 0;
			}
			else
			{
				%cnt = GetStuffStringCount(fetchData(%clientId, "TempSmith"), %item);
				Client::sendMessage(%clientId, $MsgRed, "Invalid combination. (" @ %cnt @ " " @ %item.description @ ")");
			}
		}
	}
}

function GetSmithCombo(%tempsmith)
{
	dbecho($dbechoMode, "GetSmithCombo(" @ %tempsmith @ ")");

	for(%i = 1; $SmithCombo[%i] != ""; %i++)
	{
		if(IsStuffStringEquiv(%tempsmith, $SmithCombo[%i], True))
			return %i;
	}
	return 0;
}

function GetSmithComboCost(%clientId, %sc)
{
	dbecho($dbechoMode, "GetSmithComboCost(" @ %clientId @ ", " @ %sc @ ")");

	%c1 = GetStuffStringCost(%clientId, $SmithCombo[%sc]);
	%c2 = GetStuffStringCost(%clientId, $SmithComboResult[%sc]);

	return Cap( round(%c2 - (%c1 * 1.35)), 1, "inf");
}

function GetStuffStringCost(%clientId, %itemlist)
{
	dbecho($dbechoMode, "GetStuffStringCost(" @ %clientId @ ", " @ %itemlist @ ")");

	%cost = 0;
	for(%i = 0; (%w = GetWord(%itemlist, %i)) != -1; %i++)
	{
		%w2 = GetWord(%itemlist, %i+1);
		%c = getBuyCost(%clientId, %w) * %w2;
		%cost += %c;
	}

	return %cost;
}