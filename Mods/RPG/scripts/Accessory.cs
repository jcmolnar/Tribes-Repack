//This file is part of Tribes RPG.
//Tribes RPG server side scripts
//General RPG scripts written by Jason "phantom" Daley,  Matthiew "JeremyIrons" Bouchard, and more (yet undetermined)

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


//When adding a new accessory, follow these steps:
//-(if it has a new accessory type, fill in the stuff here)
//-add the actual itemdata here

//current item method involves having two ItemData's for each item, where one differs from the
//other by category.  One is Accessory, the other is Equipped.

//=========================
//  $SpecialVar list:
//=========================
//1:
//2:
//3: MDEF
//4: HP
//5: Mana
//6: ATK
//7: DEF
//8: Internal armor switching variable
//9:
//10: HP regen
//11: Mana regen	

$SpecialVarDesc[1] = "";
$SpecialVarDesc[2] = "";
$SpecialVarDesc[3] = "MDEF (Magical)";
$SpecialVarDesc[4] = "HP";
$SpecialVarDesc[5] = "Mana";
$SpecialVarDesc[6] = "ATK";
$SpecialVarDesc[7] = "DEF";
$SpecialVarDesc[8] = "[Internal]";
$SpecialVarDesc[9] = "";
$SpecialVarDesc[10] = "HP regen";
$SpecialVarDesc[11] = "Mana regen";

$RingAccessoryType = 1;
$BodyAccessoryType = 2;
$BootsAccessoryType = 3;
$BackAccessoryType = 4;
$ShieldAccessoryType = 5;
$TalismanAccessoryType = 6;
$SwordAccessoryType = 7;
$AxeAccessoryType = 8;
$PolearmAccessoryType = 9;
$BludgeonAccessoryType = 10;
$RangedAccessoryType = 11;
$ProjectileAccessoryType = 12;

$LocationDesc[$RingAccessoryType] = "Ring";
$LocationDesc[$BodyAccessoryType] = "Body";
$LocationDesc[$BootsAccessoryType] = "Feet";
$LocationDesc[$BackAccessoryType] = "Back";
$LocationDesc[$ShieldAccessoryType] = "Shield";
$LocationDesc[$TalismanAccessoryType] = "Talisman";
$LocationDesc[$SwordAccessoryType] = "Sword";
$LocationDesc[$AxeAccessoryType] = "Axe";
$LocationDesc[$PolearmAccessoryType] = "Polearm";
$LocationDesc[$BludgeonAccessoryType] = "Bludgeon";
$LocationDesc[$RangedAccessoryType] = "Ranged";
$LocationDesc[$ProjectileAccessoryType] = "Projectile";

$maxAccessory[$RingAccessoryType] = 2;
$maxAccessory[$BodyAccessoryType] = 1;
$maxAccessory[$BootsAccessoryType] = 1;
$maxAccessory[$BackAccessoryType] = 1;
$maxAccessory[$ShieldAccessoryType] = 1;
$maxAccessory[$TalismanAccessoryType] = 1;

//these are used for $AccessoryVar
$AccessoryType = 1;			//(used in item.cs)
$SpecialVar = 2;				//(used in player.cs)
$Weight = 3;				//(used in rpgfunk.cs)
$ShopIndex = 4;
$MiscInfo = 5;

//replaced with other potions in belt.cs
//$HardcodedItemCost[BluePotion] = 15;
//$HardcodedItemCost[CrystalBluePotion] = 100;
//$HardcodedItemCost[EnergyVial] = 15;
//$HardcodedItemCost[CrystalEnergyVial] = 100;

$HardcodedItemCost[Tent] = 4000;
$HardcodedItemCost[Trancephyte] = 120000;
$HardcodedItemCost[OrbOfLuminance] = 25;
$HardcodedItemCost[OrbOfBreath] = 1000;
$HardcodedItemCost[CheetaursPaws] = 3500;
$HardcodedItemCost[BootsOfGliding] = 8000;
$HardcodedItemCost[WindWalkers] = 45000;

$HardcodedItemCost[BlackStatue] = 180;
$HardcodedItemCost[EnchantedStone] = 2450;
$HardcodedItemCost[SkeletonBone] = 5860;
$HardcodedItemCost[Parchment] = 1000000;
$HardcodedItemCost[MagicDust] = 1000000;
$HardcodedItemCost[DragonScale] = 245310;
$HardcodedItemCost[BadgeOfFriendship] = 1;
$HardcodedItemCost[BadgeOfLoyalty] = 1;
$HardcodedItemCost[BadgeOfHonor] = 1;
$HardcodedItemCost[BadgeOfReverence] = 1;

function GenerateAllShieldCosts()
{
	dbecho($dbechoMode, "GenerateAllShieldCosts()");

	$ItemCost[KnightShield] = GenerateItemCost(KnightShield);
	$ItemCost[HeavenlyShield] = GenerateItemCost(HeavenlyShield);
	$ItemCost[DragonShield] = GenerateItemCost(DragonShield);
}

//=====================
// ACCESSORY FUNCTIONS
//=====================

function GetAccessoryVar(%item, %type)
{
	dbecho($dbechoMode, "GetAccessoryVar(" @ %item @ ", " @ %type @ ")");

	%nitem = getCroppedItem(%item);

	return $AccessoryVar[%nitem, %type];
}

function getCroppedItem(%item)
{
	dbecho($dbechoMode, "getCroppedItem(" @ %item @ ")");

	%zitem = %item @ "xx";
	%p = String::findSubStr(%zitem, "0xx");
	if(%p != -1)
		%nitem = String::getSubStr(%item, 0, %p);
	else
		%nitem = %item;

	return %nitem;
}

function GetAccessoryList(%clientId, %type, %filter)
{
	dbecho($dbechoMode, "GetAccessoryList(" @ %clientId @ ", " @ %type @ ", " @ %filter @ ")");

	if(IsDead(%clientId) || !fetchData(%clientId, "HasLoadedAndSpawned") || %clientId.IsInvalid || %clientId.choosingGroup || %clientId.choosingClass)
		return "";

	%list = "";	if(%type == 10)	{		%max = $count[AmmoItems];		for(%i = 0; %i < %max; %i++)		{			%item = $beltitem[%i+1, "Num", "AmmoItems"];			%count = belt::hasthisstuff(%clientId, %item);			if(%count)			{				if(%filter != -1)				{					%flag2 = False;					%av = GetAccessoryVar(%item, $SpecialVar);					for(%j = 0; GetWord(%av, %j) != -1; %j+=2)					{						%w = GetWord(%av, %j);						if(String::findSubStr(%filter, %w) != -1)							%flag2 = True;					}				}				if(%filter == -1 || %flag2)					%list = %list @ %item @ " ";			}		}		return %list;	}
	%max = getNumItems();
	for(%i = 0; %i < %max; %i++)
	{
		%count = Player::getItemCount(%clientId, %i);

		if(%count)
		{
			%item = getItemData(%i);

			%flag = "";
			if(%type == 1)
			{
				if(%item.className == "Accessory")
					%flag = True;
			}
			else if(%type == 2)
			{
				if(%item.className == "Equipped")
					%flag = True;
			}
			else if(%type == 3)
			{
				if(%item.className == "Accessory" || %item.className == "Equipped")
					%flag = True;
			}
			else if(%type == 4)
			{
				if(%item.className == "Equipped" || %item.className == "Weapon" || %item.className == "Backpack")
				{
					if(%item.className == "Weapon")
					{
						if(Player::getMountedItem(%clientId, $WeaponSlot) == %item)
							%flag = True;
					}
					else if(%item.className == "Backpack")
					{
						if(Player::getMountedItem(%clientId, $BackpackSlot) == %item)
							%flag = True;
					}
					else
						%flag = True;
				}
			}
			else if(%type == 5)
			{
				if($AccessoryVar[%item, $AccessoryType] == $SwordAccessoryType)
					%flag = True;
			}
			else if(%type == 6)
			{
				if($AccessoryVar[%item, $AccessoryType] == $AxeAccessoryType)
					%flag = True;
			}
			else if(%type == 7)
			{
				if($AccessoryVar[%item, $AccessoryType] == $PolearmAccessoryType)
					%flag = True;
			}
			else if(%type == 8)
			{
				if($AccessoryVar[%item, $AccessoryType] == $BludgeonAccessoryType)
					%flag = True;
			}
			else if(%type == 9)
			{
				if($AccessoryVar[%item, $AccessoryType] == $RangedAccessoryType)
					%flag = True;
			}
			else if(%type == -1)
				%flag = True;

			if(%flag)
			{
				if(%filter != -1)
				{
					%flag2 = False;
					%av = GetAccessoryVar(%item, $SpecialVar);
					for(%j = 0; GetWord(%av, %j) != -1; %j+=2)
					{
						%w = GetWord(%av, %j);
						if(String::findSubStr(%filter, %w) != -1)
							%flag2 = True;
					}
				}
				if(%filter == -1 || %flag2)
					%list = %list @ %item @ " ";
			}
		}
	}
	return %list;
}

function AddPoints(%clientId, %char)
{
	dbecho($dbechoMode, "AddPoints(" @ %clientId @ ", " @ %char @ ")");

	%add = 0;
	%list = GetAccessoryList(%clientId, 4, %char);
	for(%i = 0; GetWord(%list, %i) != -1; %i++)
	{
		%w = GetWord(%list, %i);

		%slot = "";
		if(%w.className == Weapon)
			%slot = $WeaponSlot;
		else if(%w.className == Backpack)
			%slot = $BackpackSlot;

		if(%slot != "")
		{
			if(Player::getMountedItem(%clientId, %slot) == %w)
				%count = 1;
			else
				%count = 0;
		}
		else
			%count = Player::getItemCount(%clientId, %w);

		%tmp = GetAccessoryVar(%w, $SpecialVar);

		for(%j = 0; GetWord(%tmp, %j) != -1; %j+=2)
		{
			%e = GetWord(%tmp, %j);
			if(String::findSubStr(%char, %e) != -1)
				%add += GetWord(%tmp, %j+1) * %count;
		}
	}

	return %add;
}

function AddItemSpecificPoints(%item, %char)
{
	dbecho($dbechoMode, "AddItemSpecificPoints(" @ %item @ ", " @ %char @ ")");

	%tmp = GetAccessoryVar(%item, $SpecialVar);

	for(%j = 0; GetWord(%tmp, %j) != -1; %j+=2)
	{
		%e = GetWord(%tmp, %j);
		if(%e == %char)
		{
			%info = GetWord(%tmp, %j+1);
			break;
		}
	}

	return %info;
}

function WhatSpecialVars(%thing)
{
	dbecho($dbechoMode, "WhatSpecialVars(" @ %thing @ ")");

	%tmp = GetAccessoryVar(%thing, $SpecialVar);

	%t = "";
	for(%i = 0; GetWord(%tmp, %i) != -1; %i+=2)
	{
		%s = GetWord(%tmp, %i);
		%n = GetWord(%tmp, %i+1);

		%t = %t @ $SpecialVarDesc[%s] @ ": " @ %n @ ", ";
	}
	if(%t == "")
		%t = "None";
	else
		%t = String::getSubStr(%t, 0, String::len(%t)-2);
	
	return %t;
}

function NullItemList(%clientId, %type, %msgcolor, %msg)
{
	dbecho($dbechoMode, "NullItemList(" @ %clientId @ ", " @ %type @ ", " @ %msgcolor @ ", " @ %msg @ ")");

	for(%z = 1; $ItemList[%type, %z] != ""; %z++)
	{
		%item = $ItemList[%type, %z];
		if(isBeltItem(%item))		{			%amnt = Belt::HasThisStuff(%clientid,%item);			if(%amnt > 0)			{				%item = $beltitem[%item, "Item"];				%name = $beltitem[%item, "Name"];				%newmsg = nsprintf(%msg, %name);				Client::sendMessage(%clientId, %msgcolor, %newmsg);				Belt::TakeThisStuff(%clientid,%item,%amnt);			}		}
		else if(Player::getItemCount(%clientId, %item))
		{
			Player::setItemCount(%clientId, %item, 0);

			%newmsg = nsprintf(%msg, %item.description);
			Client::sendMessage(%clientId, %msgcolor, %newmsg);
		}
	}
}

function GetCurrentlyWearingArmor(%clientId)
{
	dbecho($dbechoMode, "GetCurrentlyWearingArmor(" @ %clientId @ ")");

	//the $ArmorList is present only for this function so far, in order to speed things up and not have to cycle thru
	//each and every item in the game
	for(%i = 1; $ArmorList[%i] != ""; %i++)
	{
		if(Player::getItemCount(%clientId, $ArmorList[%i] @ "0"))
			return $ArmorList[%i];
	}
	return "";
}

//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//   POTIONS
//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

//$AccessoryVar[BluePotion, $Weight] = 4;
//ItemData BluePotion
//{
//	description = "Blue Potion";
//	shapeFile = "armorKit";
//	heading = "eMiscellany";
//	className = "Accessory";
//	shadowDetailMask = 4;
//	price = 0;
//};
//function BluePotion::onUse(%player,%item)
//{
//	%clientId = Player::getClient(%player);
//
//	Player::decItemCount(%player,%item);
//	%hp = fetchData(%clientId, "HP");
//	refreshHP(%clientId, -0.15);
//	refreshAll(%clientId);
//
//	if(fetchData(%clientId, "HP") != %hp)
//		UseSkill(%clientId, $SkillHealing, True, True);
//}

//$AccessoryVar[CrystalBluePotion, $Weight] = 10;
//ItemData CrystalBluePotion
//{
//	description = "Crystal Blue Potion";
//	shapeFile = "armorKit";
//	heading = "eMiscellany";
//	className = "Accessory";
//	shadowDetailMask = 4;
//	price = 0;
//};
//function CrystalBluePotion::onUse(%player,%item)
//{
//	%clientId = Player::getClient(%player);
//
//	Player::decItemCount(%player,%item);
//	%hp = fetchData(%clientId, "HP");
//	refreshHP(%clientId, -0.6);
//	refreshAll(%clientId);
//
//	if(fetchData(%clientId, "HP") != %hp)
//		UseSkill(%clientId, $SkillHealing, True, True);
//}

//$AccessoryVar[EnergyVial, $Weight] = 2;
//ItemData EnergyVial
//{
//	description = "Energy Vial";
//	shapeFile = "armorKit";
//	heading = "eMiscellany";
//	className = "Accessory";
//	shadowDetailMask = 4;
//	price = 0;
//};
//function EnergyVial::onUse(%player,%item)
//{
//	%clientId = Player::getClient(%player);
//
//	Player::decItemCount(%player,%item);
//	refreshMANA(%clientId, -16);
//	refreshAll(%clientId);
//}

//$AccessoryVar[CrystalEnergyVial, $Weight] = 5;
//ItemData CrystalEnergyVial
//{
//	description = "Crystal Energy Vial";
//	shapeFile = "armorKit";
//	heading = "eMiscellany";
//	className = "Accessory";
//	shadowDetailMask = 4;
//	price = 0;
//};
//function CrystalEnergyVial::onUse(%player,%item)
//{
//	%clientId = Player::getClient(%player);
//
//	Player::decItemCount(%player,%item);
//	refreshMANA(%clientId, -50);
//	refreshAll(%clientId);
//}

//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//   RINGS
//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$RingWeight = 1;

//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//   ARMOR MODIFYING ACCESSORIES
//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$AccessoryVar[CheetaursPaws, $AccessoryType] = $BootsAccessoryType;
$AccessoryVar[CheetaursPaws, $SpecialVar] = "8 1";
$AccessoryVar[CheetaursPaws, $Weight] = 3;
$AccessoryVar[CheetaursPaws, $MiscInfo] = "Cheetaur's Paws increase speed and jump power";

ItemData CheetaursPaws
{
	description = "Cheetaur's Paws";
	className = "Accessory";
	shapeFile = "discammo";

	heading = "eMiscellany";
	price = 0;
};
ItemData CheetaursPaws0
{
	description = "Cheetaur's Paws";
	className = "Equipped";
	shapeFile = "discammo";

	heading = "aArmor";
};

$AccessoryVar[BootsOfGliding, $AccessoryType] = $BootsAccessoryType;
$AccessoryVar[BootsOfGliding, $SpecialVar] = "8 2";
$AccessoryVar[BootsOfGliding, $Weight] = 3;
$AccessoryVar[BootsOfGliding, $MiscInfo] = "Boots Of Gliding let you glide";

ItemData BootsOfGliding
{
	description = "Boots Of Gliding";
	className = "Accessory";
	shapeFile = "discammo";

	heading = "eMiscellany";
	price = 0;
};
ItemData BootsOfGliding0
{
	description = "Boots Of Gliding";
	className = "Equipped";
	shapeFile = "discammo";

	heading = "aArmor";
};

$AccessoryVar[WindWalkers, $AccessoryType] = $BootsAccessoryType;
$AccessoryVar[WindWalkers, $SpecialVar] = "8 3";
$AccessoryVar[WindWalkers, $Weight] = 3;
$AccessoryVar[WindWalkers, $MiscInfo] = "Wind Walkers let you fly!";

ItemData WindWalkers
{
	description = "Wind Walkers";
	className = "Accessory";
	shapeFile = "discammo";

	heading = "eMiscellany";
	price = 0;
};
ItemData WindWalkers0
{
	description = "Wind Walkers";
	className = "Equipped";
	shapeFile = "discammo";

	heading = "aArmor";
};

//============================================================================

$AccessoryVar[KnightShield, $AccessoryType] = $ShieldAccessoryType;
$AccessoryVar[KnightShield, $SpecialVar] = "7 250";
$AccessoryVar[KnightShield, $Weight] = 16;
$AccessoryVar[KnightShield, $MiscInfo] = "The Knight's Shield is a unique item that provides great defense.";

ItemImageData KnightShieldImage
{
	shapeFile = "shield3";
	mountPoint = 2;
	mountOffset = {0.18, -0.1, -0.2};
	mountRotation = {0, 0, 0.5};
};
ItemData KnightShield
{
	description = "Knight's Shield";
	className = "Accessory";
	shapeFile = "shield3";
	imageType = KnightShieldImage;

	heading = "eMiscellany";
	price = 0;
};
ItemData KnightShield0
{
	description = "Knight's Shield";
	className = "Equipped";
	shapeFile = "shield3";

	heading = "aArmor";
};

$AccessoryVar[HeavenlyShield, $AccessoryType] = $ShieldAccessoryType;
$AccessoryVar[HeavenlyShield, $SpecialVar] = "7 315 3 635";
$AccessoryVar[HeavenlyShield, $Weight] = 14;
$AccessoryVar[HeavenlyShield, $MiscInfo] = "The Heavenly Shield is a unique item that provides Excellent Magical and Physical defense.";

ItemImageData HeavenlyShieldImage
{
	shapeFile = "shield3";
	mountPoint = 2;
	mountOffset = {0.18, -0.1, -0.2};
	mountRotation = {0, 0, 0.5};
};
ItemData HeavenlyShield
{
	description = "Heavenly Shield";
	className = "Accessory";
	shapeFile = "shield3";
	imageType = HeavenlyShieldImage;

	heading = "eMiscellany";
	price = 0;
};
ItemData HeavenlyShield0
{
	description = "Heavenly Shield";
	className = "Equipped";
	shapeFile = "shield3";

	heading = "aArmor";
};

$AccessoryVar[DragonShield, $AccessoryType] = $ShieldAccessoryType;
$AccessoryVar[DragonShield, $SpecialVar] = "7 540 4 210";
$AccessoryVar[DragonShield, $Weight] = 15;
$AccessoryVar[DragonShield, $MiscInfo] = "A shield made from the scales of a dragon";

ItemImageData DragonShieldImage
{
	shapeFile = "shield3";
	mountPoint = 2;
	mountOffset = {0.18, -0.1, -0.2};
	mountRotation = {0, 0, 0.5};
};
ItemData DragonShield
{
	description = "Dragon Shield";
	className = "Accessory";
	shapeFile = "shield3";
	imageType = DragonShieldImage;

	heading = "eMiscellany";
	price = 0;
};
ItemData DragonShield0
{
	description = "Dragon Shield";
	className = "Equipped";
	shapeFile = "shield3";

	heading = "aArmor";
};

//============================================================================

//------------------------
$AccessoryVar[Tent, $Weight] = 40;
$AccessoryVar[Tent, $MiscInfo] = "A tent. Use #camp to set it up, and #uncamp to disassemble it.";

ItemData Tent
{
	description = "Tent";
	shapeFile = "armorKit";
	heading = "eMiscellany";
	className = "Accessory";
	shadowDetailMask = 4;
	price = 0;
};
//------------------------

//===== MISC STUFF ===============================================================

ItemData Lootbag
{
	description = "Backpack";
	className = "Lootbag";
	shapeFile = "ammo2";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

//===================
//  Mining stuff
//===================
$AccessoryVar[Quartz, $Weight] = 0.2;
$AccessoryVar[Granite, $Weight] = 0.2;
$AccessoryVar[Opal, $Weight] = 0.2;
$AccessoryVar[Jade, $Weight] = 0.25;
$AccessoryVar[Turquoise, $Weight] = 0.3;
$AccessoryVar[Ruby, $Weight] = 0.3;
$AccessoryVar[Topaz, $Weight] = 0.3;
$AccessoryVar[Sapphire, $Weight] = 0.3;
$AccessoryVar[Gold, $Weight] = 3.5;
$AccessoryVar[Emerald, $Weight] = 0.2;
$AccessoryVar[Diamond, $Weight] = 0.1;
$AccessoryVar[Keldrinite, $Weight] = 5.0;

$AccessoryVar[Quartz, $MiscInfo] = "Quartz";
$AccessoryVar[Granite, $MiscInfo] = "Granite";
$AccessoryVar[Opal, $MiscInfo] = "Opal";
$AccessoryVar[Jade, $MiscInfo] = "Jade";
$AccessoryVar[Turquoise, $MiscInfo] = "Turquoise";
$AccessoryVar[Ruby, $MiscInfo] = "Ruby";
$AccessoryVar[Topaz, $MiscInfo] = "Topaz";
$AccessoryVar[Sapphire, $MiscInfo] = "Sapphire";
$AccessoryVar[Gold, $MiscInfo] = "Gold";
$AccessoryVar[Emerald, $MiscInfo] = "Emerald";
$AccessoryVar[Diamond, $MiscInfo] = "Diamond";
$AccessoryVar[Keldrinite, $MiscInfo] = "Keldrinite is a very rare magical gem that, when in the hands of a skilled blacksmith, can give items magical properties.";

$HardcodedItemCost[SmallRock] = 13;
$HardcodedItemCost[Quartz] = 100;
$HardcodedItemCost[Granite] = 180;
$HardcodedItemCost[Opal] = 300;
$HardcodedItemCost[Jade] = 550;
$HardcodedItemCost[Turquoise] = 850;
$HardcodedItemCost[Ruby] = 1200;
$HardcodedItemCost[Topaz] = 1604;
$HardcodedItemCost[Sapphire] = 2930;
$HardcodedItemCost[Gold] = 4680;
$HardcodedItemCost[Emerald] = 9702;
$HardcodedItemCost[Diamond] = 16575;
$HardcodedItemCost[Keldrinite] = 125200;

%f = 43;
$ItemList[Mining, 1] = "SmallRock " @ round($HardcodedItemCost[SmallRock] / %f)+2;
$ItemList[Mining, 2] = "Quartz " @ round($HardcodedItemCost[Quartz] / %f)+2;
$ItemList[Mining, 3] = "Granite " @ round($HardcodedItemCost[Granite] / %f)+2;
$ItemList[Mining, 4] = "Opal " @ round($HardcodedItemCost[Opal] / %f)+2;
$ItemList[Mining, 5] = "Jade " @ round($HardcodedItemCost[Jade] / %f)+2;
$ItemList[Mining, 6] = "Turquoise " @ round($HardcodedItemCost[Turquoise] / %f)+2;
$ItemList[Mining, 7] = "Ruby " @ round($HardcodedItemCost[Ruby] / %f)+2;
$ItemList[Mining, 8] = "Topaz " @ round($HardcodedItemCost[Topaz] / %f)+2;
$ItemList[Mining, 9] = "Sapphire " @ round($HardcodedItemCost[Sapphire] / %f)+2;
$ItemList[Mining, 10] = "Gold " @ round($HardcodedItemCost[Gold] / %f)+2;
$ItemList[Mining, 11] = "Emerald " @ round($HardcodedItemCost[Emerald] / %f)+2;
$ItemList[Mining, 12] = "Diamond " @ round($HardcodedItemCost[Diamond] / %f)+2;
$ItemList[Mining, 13] = "Keldrinite " @ round($HardcodedItemCost[Keldrinite] / %f)+2;


$AccessoryVar[BlackStatue, $Weight] = 1;
$AccessoryVar[BlackStatue, $MiscInfo] = "A black statue";

ItemData BlackStatue
{
	description = "Black Statue";
	className = "Accessory";
	shapeFile = "mineammo";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

$AccessoryVar[SkeletonBone, $Weight] = 1;
$AccessoryVar[SkeletonBone, $MiscInfo] = "A skeleton bone";

ItemData SkeletonBone
{
	description = "Skeleton Bone";
	className = "Accessory";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

$AccessoryVar[EnchantedStone, $Weight] = 5;
$AccessoryVar[EnchantedStone, $MiscInfo] = "An enchanted stone";

ItemData EnchantedStone
{
	description = "Enchanted Stone";
	className = "Accessory";
	shapeFile = "granite";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

$AccessoryVar[DragonScale, $Weight] = 8;
$AccessoryVar[DragonScale, $MiscInfo] = "A dragon scale";

ItemData DragonScale
{
	description = "Dragon Scale";
	className = "Accessory";
	shapeFile = "granite";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

//===================
//  LORE ITEMS
//===================
$ItemList[Lore, 1] = "Parchment";
$ItemList[Lore, 2] = "MagicDust";

$AccessoryVar[Parchment, $Weight] = 0.2;
$AccessoryVar[Parchment, $MiscInfo] = "A parchment";
$LoreItem[Parchment] = True;

ItemData Parchment
{
	description = "Parchment";
	className = "Accessory";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

$AccessoryVar[MagicDust, $Weight] = 0.2;
$AccessoryVar[MagicDust, $MiscInfo] = "A small bag containing magic dust";
$LoreItem[MagicDust] = True;

ItemData MagicDust
{
	description = "Magic Dust";
	className = "Accessory";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};
function MagicDust::onUse(%player, %item)
{
	Player::decItemCount(%player, %item);

	%list = GetEveryoneIdList();
	for(%i = 0; (%id = GetWord(%list, %i)) != -1; %i++)
	{
		%pl = Client::getOwnedObject(%clientId);
		if(Vector::getDistance(GameBase::getPosition(%player), GameBase::getPosition(%pl)) <= 20)
			Player::applyImpulse(%pl, "0 0 500");
	}
}

//===================
// Badges
//===================
$ItemList[Badge, 1] = "BadgeOfFriendship";
$ItemList[Badge, 2] = "BadgeOfLoyalty";
$ItemList[Badge, 3] = "BadgeOfHonor";
$ItemList[Badge, 4] = "BadgeOfReverence";

$AccessoryVar[BadgeOfHonor, $Weight] = 1;
$AccessoryVar[BadgeOfHonor, $MiscInfo] = "Badge Of Honor. A chance in 220 every two seconds that an LCK point will be awarded.";
$BonusItem[BadgeOfHonor] = "LCK 1 220";	//a chance in 220 every ZoneCheck that 1 LCK will be awarded
$StealProtectedItem[BadgeOfHonor] = True;

ItemData BadgeOfHonor
{
	description = "Badge Of Honor";
	className = "Accessory";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

$AccessoryVar[BadgeOfLoyalty, $Weight] = 1;
$AccessoryVar[BadgeOfLoyalty, $MiscInfo] = "Badge Of Loyalty. A chance in 120 every two seconds that 3 EXP will be awarded.";
$BonusItem[BadgeOfLoyalty] = "EXP 3 120";
$StealProtectedItem[BadgeOfLoyalty] = True;

ItemData BadgeOfLoyalty
{
	description = "Badge Of Loyalty";
	className = "Accessory";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

$AccessoryVar[BadgeOfFriendship, $Weight] = 1;
$AccessoryVar[BadgeOfFriendship, $MiscInfo] = "Badge Of Friendship. A chance in 80 every two seconds that 50 coins will be awarded.";
$BonusItem[BadgeOfFriendship] = "COINS 50 80";
$StealProtectedItem[BadgeOfFriendship] = True;

ItemData BadgeOfFriendship
{
	description = "Badge Of Friendship";
	className = "Accessory";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

$AccessoryVar[BadgeOfReverence, $Weight] = 1;
$AccessoryVar[BadgeOfReverence, $MiscInfo] = "Badge Of Reverence. A chance in 180 every two seconds that an SP credit will be awarded.";
$BonusItem[BadgeOfReverence] = "SP 1 180";
$StealProtectedItem[BadgeOfReverence] = True;

ItemData BadgeOfReverence
{
	description = "Badge Of Reverence";
	className = "Accessory";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

$AccessoryVar[Trancephyte, $Weight] = 1;
$StealProtectedItem[Trancephyte] = True;
ItemData Trancephyte
{
	description = "Trancephyte";
	className = "Accessory";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

//========= ORBS ===================================================

//i suggest putting orbs that protect from water at the top of the list.
$ItemList[Orb, 1] = "OrbOfBreath";
$ItemList[Orb, 2] = "OrbOfLuminance";

//Orb of Luminance
$AccessoryVar[OrbOfLuminance, $AccessoryType] = $ShieldAccessoryType;
$AccessoryVar[OrbOfLuminance, $Weight] = 1.0;
$AccessoryVar[OrbOfLuminance, $MiscInfo] = "The Orb Of Luminance provides you with temporary illumination.";
$OverrideMountPoint[OrbOfLuminance] = 2;
$BurnOut[OrbOfLuminance] = 150;
$BurnOutInRain[OrbOfLuminance] = 5;
$ProtectFromWater[OrbOfLuminance] = "";

ItemImageData OrbOfLuminanceImage
{
	shapeFile = "orb";
	mountPoint = $OverrideMountPoint[OrbOfLuminance];
	mountOffset = {0.0, 0.0, 1.8};
	mountRotation = {5, 3, 3};

	lightType = 2;
	lightRadius = 13;
	lightTime = 9999;
	lightColor = { 0.95, 0.85, 0.55 };
};
ItemData OrbOfLuminance
{
	description = "Orb Of Luminance";
	className = "Accessory";
	shapeFile = "orb";
	imageType = OrbOfLuminanceImage;

	heading = "eMiscellany";
	price = 0;
};
ItemData OrbOfLuminance0
{
	description = "Lit Orb Of Luminance";
	className = "Equipped";
	shapeFile = "orb";
	imageType = OrbOfLuminanceImage;

	heading = "aArmor";
};

//Orb of Breath
$AccessoryVar[OrbOfBreath, $AccessoryType] = $ShieldAccessoryType;
$AccessoryVar[OrbOfBreath, $Weight] = 0.8;
$AccessoryVar[OrbOfBreath, $MiscInfo] = "The Orb Of Breath provides you with a temporary ability to breathe underwater.";
$OverrideMountPoint[OrbOfBreath] = 2;
$BurnOut[OrbOfBreath] = 300;
$BurnOutInRain[OrbOfBreath] = 0;
$ProtectFromWater[OrbOfBreath] = True;

ItemImageData OrbOfBreathImage
{
	shapeFile = "orb";
	mountPoint = $OverrideMountPoint[OrbOfBreath];
	mountOffset = {0.0, 0.0, 1.4};
	mountRotation = {5, 3, 3};
};
ItemData OrbOfBreath
{
	description = "Orb Of Breath";
	className = "Accessory";
	shapeFile = "orb";
	imageType = OrbOfBreathImage;

	heading = "eMiscellany";
	price = 0;
};
ItemData OrbOfBreath0
{
	description = "Orb Of Breath in use";
	className = "Equipped";
	shapeFile = "orb";
	imageType = OrbOfBreathImage;

	heading = "aArmor";
};

ItemData Grenade
{
	description = "Grenade";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 5;
	className = "HandAmmo";
};
ItemData MineAmmo
{
	description = "Mine";
	shapeFile = "mineammo";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 10;
	className = "HandAmmo";
};
ItemData RepairKit
{
   description = "Repair Kit";
   shapeFile = "armorKit";
   heading = "eMiscellany";
   shadowDetailMask = 4;
   price = 35;
};


//used for iceTurretData HappyStand {		className = "Turret";	shapeFile = "camera";	maxDamage = 2500;	maxEnergy = 10;	speed = 20;	speedModifier = 1.0;	range = 0.0;	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };	visibleToSensor = true;	shadowDetailMask = 4;	castLOS = true;	supression = false;	supressable = false;	mapFilter = 2;	mapIcon = "M_camera";	debrisId = defaultDebrisSmall;	FOV = 0.707;	pinger = false;	explosionId = debrisExpMedium;	description = "HappyStand";};
function HappyStand::onDamage(%this,%type,%value,%pos,%vec,%mom,%object){	//do nothing, no damage..}

ItemData BeltItemTool
{
	description = "Belt Items...";
	shapeFile = "longbow";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
	className = "Accessory";
};