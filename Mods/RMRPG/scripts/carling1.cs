function material::drop(%w, %w2, %client) 
{
	%s = String::findSubStr(%w2, "|");
	%l = String::NEWgetSubStr(%w2, 0, %s);

	if((getrandom() * 100) >= 99 || %l == 2)
	{
		%h = String::NEWgetSubStr(%w2, %s+1, 99999);

		if(%h == "underling")		%list = "Brass Copper Pine White_Pine Ivory Coral Leather_Fabric Pink_Fabric";
		else if(%h == "raider")		%list = "Copper White_Pine Coral Bronze Cyprus_Pine Poplar Cider Quartz Crystal Lapis Purple_Fabric Hard_Leather Red_Fabric";
		else if(%h == "runt")		%list = "Brass Ivory";
		else if(%h == "chief")		%list = "Silver Gold Oak RedWood DogWood Pearl Amber Black_Fabric Blue_Fabric Green_Fabric";
		else if(%h == "pup")		%list = "Crystal Cider Silver DogWood";
		else if(%h == "shaman")		%list = "DogWood Green_Fabric";
		else if(%h == "warrior")	%list = "Silver Gold Dark_Fabric Cherry_Wood Amber Ruby";
		else if(%h == "hunter")		%list = "Silver Gold Dark_Fabric Pearl Cherry_Wood Amber Ruby Emerald";
		else if(%h == "gloom")		%list = "Brass Ivory";
		else if(%h == "alchemist")	%list = "Silver Gold Pearl Ruby Amber";
		else if(%h == "berserker")	%list = "DogWood Ivory Coral Bronze Quartz Ruby";
		else if(%h == "scourge")	%list = "Silver Gold Pearl Ruby Amber Quartz Coral";
		else if(%h == "soldier")	%list = "Iron Steel Gold Silver Bronze Copper Brass Pine White_Pine";
		else if(%h == "leader")		%list = "Pine White_Pine Cyprus_Pine Poplar Cider Oak Redwood Dogwood Maple Cherry_wood Mahongany Light_Fabric";
		else if(%h == "fury")		%list = "Leather_Fabric Pink_Fabric Purple_Fabric Hard_Leather_Fabric Red_Fabric Hide_Fabric Black_Fabric Blue_Fabric Brass Copper Bronze Silver Gold Iron Steel Platinum";
		else if(%h == "matyr")		%list = "Ivory Coral Quartz Crystal Lapis Pearl Amber Ruby Emerald Diamond Valorite Brass Copper Bronze Silver Gold Iron Steel Platinum Pine White_Pine Cyprus_Pine Poplar Cider Oak Redwood Dogwood Maple Cherry_wood Green_Fabric Dark_Fabric Light_Fabric";
		else if(%h == "giant")		%list = "Iron Steel Platinum Mythril Titanium Cider Oak Redwood Dogwood Maple Cherry_wood Mahongany Amber Ruby Emerald Diamond Valorite Hard_Leather_Fabric Red_Fabric Hide_Fabric Black_Fabric Blue_Fabric Green_Fabric Dark_Fabric Light_Fabric Elven_Fabric";
		else if(%h == "terror")		%list = "Steel Platinum Mythril Titanium Cider Oak Redwood Dogwood Maple Cherry_wood Mahongany Amber Ruby Emerald Diamond Valorite Dark_Fabric Light_Fabric Elven_Fabric";
		else if(%h == "rocko")		%list = "Iron Steel Platinum Mythril Titanium Cider Oak Redwood Dogwood Maple Cherry_wood Mahongany Amber Ruby Emerald Diamond Valorite Hard_Leather_Fabric Red_Fabric Hide_Fabric Black_Fabric Blue_Fabric Green_Fabric Dark_Fabric Light_Fabric Elven_Fabric";
		else messageall(1,"Error: 001 "@client::getname(%client));

		//Brass Copper Bronze Silver Gold Iron Steel Platinum Mythril Titanium
		//Pine White_Pine Cyprus_Pine Poplar Cider Oak Redwood Dogwood Maple Cherry_wood Mahongany
		//Ivory Coral Quartz Crystal Lapis Pearl Amber Ruby Emerald Diamond Valorite Red_Moon
		//Leather_Fabric Pink_Fabric Purple_Fabric Hard_Leather_Fabric Red_Fabric Hide_Fabric Black_Fabric Blue_Fabric
		//Green_Fabric Dark_Fabric Light_Fabric Elven_Fabric

		%perc = floor(getRandom() * 100);
		for(%i=0;getword(%list,%i) != -1;%i++)
			%z++;
		%z--;
		%lh = %z - 0;
		%w2 = Cap(round((%lh * (%perc/100)) + 0), 0, %z);

		if(getword(%list,%w2) != -1)
			Item::giveItem(%Client, getword(%list,%w2), 1, true);
		else
			messageall(1,"Error 002: "@client::getname(%client)@" has "@ getword(%list,%w2) @" material to drop ("@%lh@" "@%z@" "@%w2@")");

		echo("--- "@client::getname(%client)@" has "@ getword(%list,%w2)@"!!! ---");

		rareitemdrop(%client);
	}
}

function rareitemdrop(%client)
{
	%time = getIntegerTime(true) >> 5;
	if(%time > ($LastRareDropTime + 3600))
	{
		%var = floor(getrandom() * 8);
		echo("Rare Item Drop - "@%var);
		if(%var == 3)
		{
			$LastRareDropTime = %time;
			echo("--- "@client::getname(%client)@" has Burning_Key!!! ---");
			Item::giveItem(%Client, "Burning_Key", 1, true);
		}
	}
}

function linebreak::check(%cropped,%w1)
{
	if(%w1 != "#setinfo" && %w1 != "#addinfo")
	{
	 	if(String::findSubStr(%cropped, "\t") != -1)			%block = 1;
		else if(String::findSubStr(%cropped, "\n") != -1)		%block = 1;
		else if(String::findSubStr(%cropped, "\x01") != -1)		%block = 1;
		else if(String::findSubStr(%cropped, "\x02") != -1)		%block = 1;
		else if(String::findSubStr(%cropped, "\x03") != -1)		%block = 1;
		else if(String::findSubStr(%cropped, "\x04") != -1)		%block = 1;
		else if(String::findSubStr(%cropped, "\x05") != -1)		%block = 1;
		else if(String::findSubStr(%cropped, "\x06") != -1)		%block = 1;
		else if(String::findSubStr(%cropped, "\x07") != -1)		%block = 1;
		else if(String::findSubStr(%cropped, "\x08") != -1)		%block = 1;
		else if(String::findSubStr(%cropped, "\x09") != -1)		%block = 1;
		else if(String::findSubStr(%cropped, "\x10") != -1)		%block = 1;
		else if(String::findSubStr(%cropped, "\x11") != -1)		%block = 1;
		else if(String::findSubStr(%cropped, "\x12") != -1)		%block = 1;
		else if(String::findSubStr(%cropped, "\x13") != -1)		%block = 1;
		else if(String::findSubStr(%cropped, "\x14") != -1)		%block = 1;
		else if(String::findSubStr(%cropped, "\x15") != -1)		%block = 1;
		else if(String::findSubStr(%cropped, "\x16") != -1)		%block = 1;
		else if(String::findSubStr(%cropped, "\x17") != -1)		%block = 1;
		else if(String::findSubStr(%cropped, "\x18") != -1)		%block = 1;
		else if(String::findSubStr(%cropped, "\x19") != -1)		%block = 1;
	}
	else if(String::findSubStr(%cropped, "loding.bmp>") != -1)	%block = 1;

	if(%block == 1)
		return true;
	else
		return false;
}


function cage(%Client,%time,%spell)
{
	if(%time <= 0)
		return;

	Item::setVelocity(%client,"0 0 0");

	if(!%spell)
	{
		$last::jail++;
		if($last::jail > 6)		$last::jail = 1;
		if($last::jail == 1)		%pos = "-192 15 60";
		if(%time == "")			%time = 60;
		else if($last::jail == 2)	%pos = "-192 25 60";
		else if($last::jail == 3)	%pos = "-192 35 60";
		else if($last::jail == 4)	%pos = "-182 15 60";
		else if($last::jail == 5)	%pos = "-182 25 60";
		else if($last::jail == 6)	%pos = "-182 35 60";
	}
	else
	{
		%pos = GameBase::getposition(%client);
		%sop = %pos;
		%xxx = getword(%pos,0);
		%yyy = getword(%pos,1);
		%zzz = getword(%pos,2);
		%pos = (%xxx-1000)@" "@(%yyy-3000)@" "@(%zzz-3000);
	}

	%wall = newObject("", "StaticShape", bluebluegreen,true);
	if(%wall != 0) {
		addToSet("MissionCleanup", %wall);
		schedule("Item::Pop("@%wall@");", %time, %wall);
		GameBase::setPosition(%wall, GetWord(%pos,0)@" "@GetWord(%pos,1)@" "@GetWord(%pos,2)+3.5);
		GameBase::setRotation(%wall, "0 0 1.57");
	}
	%wall = newObject("", "StaticShape", bluebluegreen,true);
	if(%wall != 0) {
		addToSet("MissionCleanup", %wall);
		schedule("Item::Pop("@%wall@");", %time, %wall);
		GameBase::setPosition(%wall, GetWord(%pos,0)@" "@GetWord(%pos,1)+1.99@" "@GetWord(%pos,2)+1.5);
		GameBase::setRotation(%wall, "1.57 0 0");
	}
	%wall = newObject("", "StaticShape", bluebluegreen,true);
	if(%wall != 0) {
		addToSet("MissionCleanup", %wall);
		schedule("Item::Pop("@%wall@");", %time, %wall);
		GameBase::setPosition(%wall, GetWord(%pos,0)@" "@GetWord(%pos,1)-1.81@" "@GetWord(%pos,2)+1.5);
		GameBase::setRotation(%wall, "1.56 0 0");
	}
	%wall = newObject("", "StaticShape", bluebluegreen,true);
	if(%wall != 0) {
		addToSet("MissionCleanup", %wall);
		schedule("Item::Pop("@%wall@");", %time, %wall);
		GameBase::setPosition(%wall, GetWord(%pos,0)+1.81@" "@GetWord(%pos,1)@" "@GetWord(%pos,2)+1.5);
		GameBase::setRotation(%wall, "1.57 0 1.57");
	}
	%wall = newObject("", "StaticShape", bluebluegreen,true);
	if(%wall != 0) {
		addToSet("MissionCleanup", %wall);
		schedule("Item::Pop("@%wall@");", %time, %wall);
		GameBase::setPosition(%wall, GetWord(%pos,0)-1.99@" "@GetWord(%pos,1)@" "@GetWord(%pos,2)+1.5);
		GameBase::setRotation(%wall, "1.56 0 1.57");
	}
	%wall = newObject("", "StaticShape", bluebluegreen,true);
	if(%wall != 0) {
		addToSet("MissionCleanup", %wall);
		schedule("Item::Pop("@%wall@");", %time, %wall);
		GameBase::setPosition(%wall, GetWord(%pos,0)@" "@GetWord(%pos,1)@" "@GetWord(%pos,2)-0.5);
		GameBase::setRotation(%wall, "0 0 1.57");
	}
	gamebase::setposition(%client,%pos);

	if(!%spell)
	{
		%client.jailed = true;
		schedule("cage::unjail("@%client@");",%time);
		Client::sendmessage(%client,0,"You have been jailed for "@%time@" seconds!");
	}
	else
	{
		$ClientData[%Client, Petrify] = Cap($ClientData[%Client, Petrify]+%time, 0, $MaxStatusTime);
		if(Player::isAiControlled(%Client)) {
			$frozen[%Client] = True;
			AI::setVar($BotInfoAiName[%Client], SpotDist, 0);
			AI::newDirectiveRemove($BotInfoAiName[%Client], 99);
			AI::newDirectiveRemove($BotInfoAiName[%Client], 127);
		}
		else {
			remotePlayMode(%Client);
			Player::setDamageFlash(%Client, 5);
			Client::setControlObject(%Client, -1);
			%Client.guilock = true;
		}
		UpdateStatusList(%Client, "Petrify", "add");
		Schedule("cage::uncage(\""@%client@"\", \""@%sop@"\");", %time);
	}
}

function cage::unjail(%client)
{
	%client.jailed = false;
	felloffmap(%client);
}
function cage::uncage(%client,%pos)
{
	GameBase::setPosition(%client, %pos);
}

function listspawn(%clientid,%opt)
{
	%group = nameToID("MissionGroup\\SpawnPoints");
	%clientPos = GameBase::getPosition(%clientid);
	%closest = 5000000;
	%closestPos = "";

	for(%i = 0; %i <= Group::objectCount(%group)-1; %i++)
	{
		%this = Group::getObject(%group, %i);
		%botPos = GameBase::getPosition(%this);
		%dist = Vector::getDistance(%clientPos, %botPos);

		if(%dist < %closest)
		{
			%closest = %dist;
			%closestId = %this;
			%closestPos = %botPos;
			%index = GetWord($BotSpawnInfo::Data[%this, indexes], floor(getRandom() * ($BotSpawnInfo::Data[%this, indexesCnt])));
			%race = $NameForRace[$spawnindex[%index]];

			if(%opt == 1)
			{
				$numAIperSpawnPoint[%closestid] = 0;
			}	
			if(%opt == 2)
			{

			}
		}
	}


	echo("---");
	echo(%closest);
	echo(%closestid);
	echo(%closestpos);
	echo(%race@$spawnindex[%index]);
	echo($numAIperSpawnPoint[%closestid] @ "/" @ $BotSpawnInfo::Data[%closestid, maxspawn]);
	echo("---");
}

function clearall()
{
	dbecho($dbechoMode, "GetBotIdList()");

	%list = "";

	%tempSet = nameToID("MissionCleanup");
	if(%tempSet != -1)
	{
		%num = Group::objectCount(%tempSet);
		for(%i = 0; %i <= %num-1; %i++)
		{
			%tempItem = Group::getObject(%tempSet, %i);

			if(getObjectType(%tempItem) == "Player")
			{
				%clientId = Player::getClient(%tempItem);
				if(Player::isAiControlled(%clientId))
				{
	echo(client::getname(%clientid));
				felloffmap(%clientid);
			}
			}
		}
	}

	return %list;
}

function spawnthing(%pos,%rot,%item,%int,%smoke)
{
	if(%item == "" || %item == -1)
		%item = bluebluegreen;
	if(%pos == "" || %pos == -1)
		%pos = "365.907 -534.971 -2305";
	if(%rot == "" || %rot == -1)
		%rot = "0 0 0";
	if(%int == 1)
		%wall = newObject(%tag, InteriorShape, %item@".dis");
	else
		%wall = newObject("", "StaticShape",%item,true);

	addToSet("MissionCleanup", %wall);
	GameBase::setPosition(%wall, %pos);
	GameBase::setRotation(%wall, %rot);

	if(%wall)
	{
		schedule("Item::Pop("@%wall@");", 10, %wall);
		if(%smoke)
		{
			for(%i=0;%i<=10;%i+=0.5)
				schedule("smokeitem("@%wall@");",%i);
		}
	}
}

function smokeitem(%this)
{
	%this.ArmorSetSmoke();
}

function firedoor(%client)
{
	if(Client::HasItem(%client, "Burning_Key", "ItemList"))
	{
		if(checkArea(%client,1))
		{
			%time = 1800;
			%pos2 = gamebase::getposition(%client);
			%rot = "0 0 0";

			%item = "nicefire";
			%pos = Vector::add(%pos2, "0 1.5 -1.4092");
			firedoor::loop(%pos,%rot,%item,%time);
	
			%item = "bluethingy";
			%pos = Vector::add(%pos2, "0.4 1.5 -0.4092");
			%rot = Vector::add(%rot, "0 0 0.8");
			firedoor::loop(%pos,%rot,%item,%time,1);

			Client::addItemCount(%client, "Burning_Key", -1);
			Client::addItemCount(%client, "Burning_Door", -1);
		}
		else
			Client::sendmessage(%client,0,"The burning door needs more space to open.");
	}
	else
		Client::sendmessage(%client,0,"The burning door requires a burning key.");
}

function firedoor::loop(%pos,%rot,%item,%time,%smoke)
{
	%wall = newObject("", "StaticShape",%item,true);
	addToSet("MissionCleanup", %wall);
	GameBase::setPosition(%wall, %pos);
	GameBase::setRotation(%wall, %rot);

	if(%wall)
	{
		schedule("Item::Pop("@%wall@");", %time, %wall);
		if(%smoke)
		{
			%tele = newObject("", "Item", "Tele", 1, false);
			%pos = Vector::add(%pos, "0 0 0.5");
			GameBase::setPosition(%tele, %pos);
			if(%tele)
				schedule("Item::Pop("@%tele@");", %time, %tele);

			for(%i=0;%i<=%time;%i+=0.5)
				schedule("smokeitem("@%wall@");",%i);
		}
	}
}

ItemData Tele
{
	description = "Tele";
	className = "Tele";
	shapeFile = "invisable";
	disableCollision = false;
	visibleToSensor = true;
	mapFilter = 1;
};

function firedoortele(%this,%object)
{
	%client = Player::getClient(%object);
	Gamebase::setposition(%client,"-198.767 44.0474 40.0342");
}

function a(%msg) 
{
	messageall(2,"Carling: "@%msg);
}

exec(carling2);
