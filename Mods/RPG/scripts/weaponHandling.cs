//This file is part of Tribes RPG.
//Tribes RPG server side scripts
//Written by Jason "phantom" Daley,  Matthiew "JeremyIrons" Bouchard, and more (yet undetermined)

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



function CreateWeaponCyclingTables()
{
	dbecho($dbechoMode, "CreateWeaponCyclingTables()");

	%n = getNumItems();
	%counter = 0;
	for (%i = 0; %i < %n; %i++)
	{
		%item = getItemData(%i);
		if(%item.className == Weapon)
		{
			if(%counter > 0)
				$NextWeapon[%lastitem] = %item;
			else
				%firstitem = %item;

			%lastitem = %item;
			%counter++;
		}
	}
	$NextWeapon[%lastitem] = %firstitem;

	for(%i = Dagger; $NextWeapon[%i] != Dagger; %i = $NextWeapon[%i])
	      $PrevWeapon[$NextWeapon[%i]] = %i;
	$PrevWeapon[$NextWeapon[%i]] = %i;
}

function RPGmountItem(%player, %item, %slot)
{
	dbecho($dbechoMode, "RPGmountItem(" @ %player @ ", " @ %item @ ", " @ %slot @ ")");

	%clientId = Player::getClient(%player);

	if(SkillCanUse(%clientId, %item))
	{
		Player::mountItem(%player, %item, %slot);
		return True;
	}
	else
	{
		Client::sendMessage(%clientId, $MsgRed, "You can't equip this item because you lack the necessary skills.~wC_BuySell.wav");
		return False;
	}
}

function remoteNextWeapon(%clientId)
{
	dbecho($dbechoMode, "remoteNextWeapon(" @ %clientId @ ")");

	%item = Player::getMountedItem(%clientId,$WeaponSlot);

	if(%item == -1 || $NextWeapon[%item] == "")
		selectValidWeapon(%clientId);
	else
	{
		for(%weapon = $NextWeapon[%item]; %weapon != %item; %weapon = $NextWeapon[%weapon])
		{
			if(isSelectableWeapon(%clientId, %weapon))
			{
				Player::useItem(%clientId,%weapon);
				// Make sure it mounted (laser may not), or at least
				// next in line to be mounted.
				if (Player::getMountedItem(%clientId,$WeaponSlot) == %weapon || Player::getNextMountedItem(%clientId,$WeaponSlot) == %weapon)
					break;
			}
		}
	}
}

function remotePrevWeapon(%clientId)
{
	dbecho($dbechoMode, "remotePrevWeapon(" @ %clientId @ ")");

	%item = Player::getMountedItem(%clientId,$WeaponSlot);
	if (%item == -1 || $PrevWeapon[%item] == "")
		selectValidWeapon(%clientId);
	else {
		for (%weapon = $PrevWeapon[%item]; %weapon != %item;
				%weapon = $PrevWeapon[%weapon]) {
			if (isSelectableWeapon(%clientId,%weapon)) {
				Player::useItem(%clientId,%weapon);
				// Make sure it mounted (laser may not), or at least
				// next in line to be mounted.
				if (Player::getMountedItem(%clientId,$WeaponSlot) == %weapon || Player::getNextMountedItem(%clientId,$WeaponSlot) == %weapon)
					break;
			}
		}
	}
}

function selectValidWeapon(%clientId)
{
	dbecho($dbechoMode, "selectValidWeapon(" @ %clientId @ ")");

	%item = Dagger;	//any weapon, it doesn't matter

	if($NextWeapon[%item] == "")	//the $NextWeapon table has NOT yet been created.
		return;

	%weapon = %item;
	while(%item == Dagger)
	{
		if(isSelectableWeapon(%clientId, %weapon))
		{
			Player::useItem(%clientId, %weapon);
			break;
		}

		%weapon = $NextWeapon[%weapon];

		if(%weapon == %item)
			break;
	}
}

function isSelectableWeapon(%clientId,%weapon)
{
	dbecho($dbechoMode, "isSelectableWeapon(" @ %clientId @ ", " @ %weapon @ ")");

	if(IsDead(%clientId) || !fetchData(%clientId, "HasLoadedAndSpawned") || %clientId.IsInvalid)
		return 0;

	if(!SkillCanUse(%clientId, %weapon))
		return false;

	if(Player::getItemCount(%clientId, %weapon))
	{
		//%ammo = $WeaponAmmo[%weapon];
		//if (%ammo == "" || Player::getItemCount(%clientId,%ammo) > 0)
			return true;
	}
	return false;
}

ItemData Weapon
{
	description = "Weapon";
	showInventory = false;
};

function Weapon::onUse(%player,%item)
{
	dbecho($dbechoMode, "Weapon::onUse(" @ %player @ ", " @ %item @ ")");

	%clientId = Player::getClient(%player);

	if(IsDead(%clientId) || !fetchData(%clientId, "HasLoadedAndSpawned") || %clientId.IsInvalid)
		return 0;

	%ammo = %item.imageType.ammoType;
	if (%ammo == "") {
		// Energy weapons dont have ammo types
		RPGmountItem(%player,%item,$WeaponSlot);
	}
	else {
		if (Player::getItemCount(%player,%ammo) > 0) 
			RPGmountItem(%player,%item,$WeaponSlot);
		else {
			Client::sendMessage(Player::getClient(%player),0,
			strcat(%item.description," has no ammo"));
		}
	}
}

function Weapon::onFire(%player, %slot)
{
}

function GetBestRangedProj(%clientId, %item)
{
	dbecho($dbechoMode, "GetBestRangedProj(" @ %clientId @ ", " @ %item @ ")");

	//this function returns the best projectile for a %clientId's ranged weapon

	//This function was written for BOTS.  It WILL work for players, but it will defeat the purpose of manually
	//equipping projectiles for each ranged weapon.

	%list = GetAccessoryList(%clientId, 10, -1);

	%highest = -1;
	%bestProj = -1;
	for(%i = 0; GetWord(%list, %i) != -1; %i++)
	{
		%proj = GetWord(%list, %i);

		if(String::findSubStr($ProjRestrictions[%proj], "," @ %item @ ",") != -1 && belt::hasthisstuff(%clientId, %proj) > 0)
		{
			%v = AddItemSpecificPoints(%proj, 6);
			if(%v > %highest)
			{
				%bestProj = %proj;
				%highest = %v;
			}
		}
	}
	return %bestProj;
}

function GetBestWeapon(%clientId)
{
	dbecho($dbechoMode, "GetBestWeapon(" @ %clientId @ ")");

	%highest = -1;
	%bestWeapon = -1;

	%item = Knife;
	for(%weapon = $NextWeapon[%item]; %weapon != %item; %weapon = $NextWeapon[%weapon])
	{
		if(isSelectableWeapon(%clientId, %weapon))
		{
			%x = "";
			%add = 0;
			if(GetAccessoryVar(%weapon, $AccessoryType) == $RangedAccessoryType)
			{
				%x = GetBestRangedProj(%clientId, %weapon);
				if(%x != -1)
					%add += AddItemSpecificPoints(%x, 6);
				else
					%add = -99999;
			}

			if(%weapon == CastingBlade)
				%add += 200;

			if(%x != -1)
			{
				%atk = AddItemSpecificPoints(%weapon, 6) + %add;
				%delay = GetDelay(%weapon);

				%v = %atk / %delay;

				if(%v > %highest)
				{
					%bestWeapon = %weapon;
					%highest = %v;
				}
			}
		}
	}

	return %bestWeapon;
}