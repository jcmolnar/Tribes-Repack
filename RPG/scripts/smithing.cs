//This file is part of Tribes RPG.
//Tribes RPG smithing scripts
//Written by Jason "phantom" Daley, and Matthiew "JeremyIrons" Bouchard

//	Copyright (C) 2014  Jason Daley

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

function Smith::addItem(%item,%combo,%result,%num)
{
	if(floor(%result) == %result){
		pecho("Smith error "@%item);
		return;
	}
	$SmithNum[%num] = %item;
	$SmithVar[%item] = %num;
	$SmithCombo[%num] = %combo;
	$SmithComboResult[%num] = %result;
}


//Notes on smithed items and rusties:
//-To determine the cost of a final combined item, add up all the costs of the materials
// involved and divide by $RustyCostAmp.

//---------------------------------
%num = 0;
Smith::addItem("RHatchet","RHatchet 1","Hatchet 1",%num++);
Smith::addItem("RBroadSword","RBroadSword 1","BroadSword 1",%num++);
Smith::addItem("RLongSword","RLongSword 1","LongSword 1",%num++);
Smith::addItem("RClub","RClub 1","Club 1",%num++);
Smith::addItem("RSpikedClub","RSpikedClub 1","SpikedClub 1",%num++);
Smith::addItem("RKnife","RKnife 1","BroadSword 1",%num++);
Smith::addItem("RDagger","RDagger 1","Dagger 1",%num++);
Smith::addItem("RShortSword","RShortSword 1","ShortSword 1",%num++);
Smith::addItem("RPickAxe","RPickAxe 1","PickAxe 1",%num++);
Smith::addItem("RShortBow","RShortBow 1","ShortBow 1",%num++);
Smith::addItem("RLightCrossbow","RLightCrossbow 1","LightCrossbow 1",%num++);
Smith::addItem("RWarAxe","RWarAxe 1","WarAxe 1",%num++);
Smith::addItem("KeldriniteLS","Keldrinite 1 LongSword 1","KeldriniteLS 1",%num++);
Smith::addItem("AeolusWing","ElvenBow 1 CompositeBow 1 Quartz 3","AeolusWing 1",%num++);
Smith::addItem("StoneFeather","SmallRock 1 Quartz 1","StoneFeather 1",%num++);
Smith::addItem("MetalFeather","Knife 1 Quartz 1","MetalFeather 1",%num++);
Smith::addItem("Talon","Dagger 1 Quartz 1 Granite 2","Talon 1",%num++);
Smith::addItem("CeraphumsFeather","Dagger 2 Jade 2 Quartz 4","CeraphumsFeather 1",%num++);
Smith::addItem("BoneClub","Club 1 SkeletonBone 1 Granite 3","BoneClub 1",%num++);
Smith::addItem("SpikedBoneClub","SpikedClub 1 SkeletonBone 2 Granite 5","SpikedBoneClub 1",%num++);
Smith::addItem("FineRobe","LightRobe 1 ApprenticeRobe 1 EnchantedStone 5","FineRobe 1",%num++);
Smith::addItem("KeldrinArmor","Keldrinite 2 FullPlateArmor 1 Gold 5 Emerald 5 Diamond 5 EnchantedStone 5","KeldrinArmor 1",%num++);
Smith::addItem("DragonMail","DragonScale 5 Diamond 5 Ruby 3","DragonMail 1",%num++);
Smith::addItem("DragonShield","DragonScale 3 Ruby 2","DragonShield 1",%num++);
Smith::addItem("ElvenRobe","AdvisorRobe 1 Topaz 2 EnchantedStone 4","ElvenRobe 1",%num++);
Smith::addItem("JusticeStaff","LongStaff 1 Granite 4 Turquoise 2","JusticeStaff 1",%num++);


//The code beyond here isn't used anymore.


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

	%txt = "<f1><jc>COINS: " @ fetchData(%clientId, "COINS");
	Client::setInventoryText(%clientId, %txt);
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