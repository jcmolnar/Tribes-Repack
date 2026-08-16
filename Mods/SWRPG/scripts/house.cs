$HouseName[1] = "Rebel Alliance";
$HouseName[2] = "Galactic Empire";
$HouseName[3] = "Mercenary's Guild";
//$HouseName[4] = "House Venk";

$HouseStartUpEq[1] = "";
$HouseStartUpEq[2] = "";
$HouseStartUpEq[3] = "";
//$HouseStartUpEq[4] = "";

function GiveHouseStuff(%TrueClientId, %houseNum)
{
//if person is jedi
//give lightsaber based on faction?

//else if person is this
//do that

//else if person is..wookiee?
//do that

//else
//do that for the rest
}

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