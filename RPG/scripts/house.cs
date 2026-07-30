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


$HouseName[1] = "House Antiva";
$HouseName[2] = "House Fenyar";
$HouseName[3] = "House Temmin";
$HouseName[4] = "House Venk";

//$HouseStartUpEq[1] = "AntivaRobe 1";
//$HouseStartUpEq[2] = "FenyarRobe 1";
//$HouseStartUpEq[3] = "TemminRobe 1";
//$HouseStartUpEq[4] = "VenkRobe 1";
$HouseStartUpEq[1] = "";
$HouseStartUpEq[2] = "";
$HouseStartUpEq[3] = "";
$HouseStartUpEq[4] = "";

function GetHouseNumber(%n)
{
	dbecho($dbechoMode, "GetHouseNumber(" @ %n @ ")");

	for(%i = 1; $HouseName[%i] != ""; %i++)
	{
		//if($HouseName[%i] == %n)
		if(String::ICompare($HouseName[%i], %n) == 0)
			return %i;
	}
	return "";
}

function BootFromCurrentHouse(%clientId, %echo)
{
	dbecho($dbechoMode, "BootFromCurrentHouse(" @ %clientId @ ", " @ %echo @ ")");

	%h = fetchData(%clientId, "MyHouse");

	if(%h != "")
	{
		UnequipMountedStuff(%clientId);

		%hn = GetHouseNumber(%h);
		if(%echo) Client::sendMessage(%clientId, $MsgRed, "You have been booted from " @ $HouseName[%hn] @ " and have lost all rank points.");

		storeData(%clientId, "MyHouse", "");
		storeData(%clientId, "RankPoints", 0);

		return %hn;
	}
	else
		return -1;
}

function JoinHouse(%clientId, %hn, %echo)
{
	dbecho($dbechoMode, "JoinHouse(" @ %clientId @ ", " @ %hn @ ", " @ %echo @ ")");

	storeData(%clientId, "MyHouse", $HouseName[%hn]);
	storeData(%clientId, "RankPoints", $joinHouseRankPoints);

	if(%echo) Client::sendMessage(%clientId, $MsgBeige, "You have joined " @ $HouseName[%hn] @ " and have been awarded " @ $joinHouseRankPoints @ " rank points.");
}