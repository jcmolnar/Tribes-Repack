

function SetTeamDBCount(%team, %count)
{
	$CommandTents[%team] = %count;
	$Concretes[%team] = %count * 2;
	$Bunkers[%team] = %count * 2;
	$WatchTowers[%team] = %count;
}

function GiveAirUnits(%team, %amount)
{
	$Airstrips[%team] = 1;
	$AirUnits[%team] = %amount;
}

function GiveChopperUnits(%team, %amount)
{
	$ChopperPads[%team] = 1;
	$ChopperUnits[%team] = %amount;
}

function GiveTankUnits(%team, %amount)
{
	$TankYards[%team] = 1;
	$TankUnits[%team] = %amount;
		
}

function GiveAllUnits(%team, %amount)
{
	SetTeamDBCount(%team, %amount);
	$TankYards[%team] = %amount;
	$TankUnits[%team] = %amount;
	$ChopperPads[%team] = %amount;
	$ChopperUnits[%team] = %amount;
	$Airstrips[%team] = %amount;
	$AirUnits[%team] = %amount;
	$RadarUpLinks[%team] = %amount;
}

function GiveSatilites(%team, %amount)
{
	$RadarUpLinks[%team] = %amount;
}

function StartDropbase()
{
	$IsTourny = true;
	for(%i = 0; %i < getNumTeams(); %i++) {
		%Group1 = newObject("DropBase", SimGroup,"Group");
		%Group2 = newObject("team"@%i, SimGroup,"Group");
		
		addToSet("MissionCleanup",%Group2);
		addToSet("MissionCleanup/team"@%i,%Group1);
		
		$CommandTents[%i] = 0;
		$Concretes[%i] = 0;
		$Bunkers[%i] = 0;
		$WatchTowers[%i] = 0;
		$Airstrips[%i] = 0;
		$ChopperPads[%i] = 0;
		$TankYards[%i] = 0;
		$RadarUplinks[%i] = 0;
		$CTLayed[%i] = false;	
	}	
}
	
function CommandTent::place( %player )
{
	%team = GameBase::getTeam(%player);
	%position = Gamebase::getposition(%player);
	
	%rot = Gamebase::getrotation(%player);	
	//%rot = NewRotation(%rot,"left");
	%position = GetOffSetRot("2 2 3.189",%rot,%position);
	
	%posTn1 = GetOffSetRot("7.725 0 1",%rot,%position);
	%posTn2 = GetOffSetRot("0 0 1",%rot,%position);
	%posTn3 = GetOffSetRot("11.5 0 -7",%rot,%position);
	%posTn4 = GetOffSetRot("-3.75 0 -7",%rot,%position);
	%posGen = GetOffSetRot("0.054 6.064 -2",%rot,%position);
	%posRep = GetOffSetRot("-5.025 -1.892 -2",%rot,%position);
	%posSt1 = GetOffSetRot("7.183 6.209 -2",%rot,%position);
	%posSt2 = GetOffSetRot("7.256 11.637 -2",%rot,%position);
	%posCmd = GetOffSetRot("-0.216 10.557 -2",%rot,%position);

	%rotTn1 = Vector::add(%rot,"0 -2.44334 0");
	%rotTn2 = Vector::add(%rot,"0 2.44334 0");
	%rotTn3 = Vector::add(%rot,"0 -1.57 0");
	%rotTn4 = Vector::add(%rot,"0 1.57 0");
	%rotSt1 = Vector::add(%rot,"0 -0 -1.59988");
	%rotSt2 = Vector::add(%rot,"0 -0 -1.59988");
	%rotCmd = Vector::add(%rot,"0 -0 1.59988");

	%Tent1 = newObject("Bunker", InteriorShape,"flatbrdg512.dis");
	%Tent2 = newObject("Bunker", InteriorShape,"flatbrdg512.dis");
	%Tent3 = newObject("Bunker", InteriorShape,"flatbrdg512.dis");
	%Tent4 = newObject("Bunker", InteriorShape,"flatbrdg512.dis");
	%gen = newObject("Bunker", "StaticShape", "PortGenerator", false);
	%reppack = newObject("Bunker", "Item", "RepairPack", 10, true, true);
	%station1 = newObject("Bunker", "StaticShape", "InventoryStation", false);
	%station2 = newObject("Bunker", "StaticShape", "InventoryStation", false);
	%cmdStation = newObject("Bunker", "StaticShape", "CommandStation", false);
	
	%Group2 = newObject("CommandTent", SimGroup,"Group");
	addToSet("MissionCleanup/team"@%team@"/DropBase",%Group2);
	
	addToSet("MissionCleanup/team"@%team@"/DropBase/CommandTent",%Tent1);
	addToSet("MissionCleanup/team"@%team@"/DropBase/CommandTent",%Tent2);
	addToSet("MissionCleanup/team"@%team@"/DropBase/CommandTent",%Tent3);
	addToSet("MissionCleanup/team"@%team@"/DropBase/CommandTent",%Tent4);
	addToSet("MissionCleanup/team"@%team@"/DropBase/CommandTent",%gen);
	addToSet("MissionCleanup/team"@%team@"/DropBase/CommandTent",%reppack);
	addToSet("MissionCleanup/team"@%team@"/DropBase/CommandTent",%station1);
	addToSet("MissionCleanup/team"@%team@"/DropBase/CommandTent",%station2);
	addToSet("MissionCleanup/team"@%team@"/DropBase/CommandTent",%cmdstation);
	
	//GameBase::setPosition(%gen,Vector::add(%position, "0.054 6.064 -2"));
	GameBase::setRotation(%gen,%rot);
	GameBase::setPosition(%gen,%posGen);

	if(GameBase::getLOSInfo(%gen,10,NewRotation(%rot,"down")))
		GameBase::setPosition(%gen,$los::position);

	//GameBase::setPosition(%reppack,Vector::add(%position, "-5.025 -1.892 -2"));
	GameBase::setRotation(%reppack,%rot);
	GameBase::setPosition(%reppack,%posRep);

	if(GameBase::getLOSInfo(%reppack,10,NewRotation(%rot,"down")))
		GameBase::setPosition(%reppack,$los::position);

	//GameBase::setRotation(%station1,"0 -0 -1.59988");
	GameBase::setRotation(%station1,%rotSt1);
	//GameBase::setPosition(%station1,Vector::add(%position, "7.183 6.209 -2"));
	GameBase::setPosition(%station1,%posSt1);

	if(GameBase::getLOSInfo(%station1,10,NewRotation(%rot,"down")))
		GameBase::setPosition(%station1,$los::position);

	//GameBase::setRotation(%station2,"0 -0 -1.57988");
	GameBase::setRotation(%station2,%rotSt2);
	//GameBase::setPosition(%station2,Vector::add(%position, "7.256 11.637 -2"));
	GameBase::setPosition(%station2,%posSt2);

	if(GameBase::getLOSInfo(%station2,10,NewRotation(%rot,"down")))
		GameBase::setPosition(%station2,$los::position);
	
	//GameBase::setRotation(%cmdstation, "0 -0 1.59988");
	GameBase::setRotation(%cmdstation,%rotCmd);
	//GameBase::setPosition(%cmdstation,Vector::add(%position, "-0.216 10.557 -2"));
	GameBase::setPosition(%cmdstation,%posCmd);
	
	if(GameBase::getLOSInfo(%cmdstation,10,NewRotation(%rot,"down")))
		GameBase::setPosition(%cmdstation,$los::position);

	//GameBase::setRotation(%tent1,"0 -2.44334 0");
	GameBase::setRotation(%tent1,%rotTn1);
	//GameBase::setPosition(%tent1,Vector::add(%position, "7.725 0 1"));
	GameBase::setPosition(%tent1,%posTn1);

	//GameBase::setRotation(%tent2,"0 2.44334 0");
	GameBase::setRotation(%tent2,%rotTn2);
	//GameBase::setPosition(%tent2,Vector::add(%position, "0 0 1"));
	GameBase::setPosition(%tent2,%posTn2);

	//GameBase::setRotation(%tent3,"0 -1.57 0");
	GameBase::setRotation(%tent3,%rotTn3);
	//GameBase::setPosition(%tent3,Vector::add(%position, "11.5 0 -7"));
	GameBase::setPosition(%tent3,%posTn3);

	//GameBase::setRotation(%tent4,"0 1.57 0");
	GameBase::setRotation(%tent4,%rotTn4);
	//GameBase::setPosition(%tent4,Vector::add(%position, "-3.75 0 -7"));
	GameBase::setPosition(%tent4,%posTn4);
	
	%posDP1 = GetOffSetRot("0 -5 3",%rot,%position);
	%posDP2 = GetOffSetRot("12 12 3",%rot,%position);
	%posDP3 = GetOffSetRot("-4 -5 3",%rot,%position);
	%posDP4 = GetOffSetRot("-5 12 3",%rot,%position);
	%posDP5 = GetOffSetRot("-6 -5 3",%rot,%position);
	%posDP6 = GetOffSetRot("13 12 3",%rot,%position);
	%posDP7 = GetOffSetRot("14 -5 3",%rot,%position);
	%posDP8 = GetOffSetRot("15 12 3",%rot,%position);	

	%DP1 = newObject("Drop Point", Marker,"DropPointMarker");
	%DP2 = newObject("Drop Point", Marker,"DropPointMarker");
	%DP3 = newObject("Drop Point", Marker,"DropPointMarker");
	%DP4 = newObject("Drop Point", Marker,"DropPointMarker");
	%DP5 = newObject("Drop Point", Marker,"DropPointMarker");
	%DP6 = newObject("Drop Point", Marker,"DropPointMarker");
	%DP7 = newObject("Drop Point", Marker,"DropPointMarker");
	%DP8 = newObject("Drop Point", Marker,"DropPointMarker");
	
	addToSet("MissionGroup/Teams/team"@%team@"/DropPoints/Random",%DP1);
	addToSet("MissionGroup/Teams/team"@%team@"/DropPoints/Random",%DP2);
	addToSet("MissionGroup/Teams/team"@%team@"/DropPoints/Random",%DP3);
	addToSet("MissionGroup/Teams/team"@%team@"/DropPoints/Random",%DP4);
	addToSet("MissionGroup/Teams/team"@%team@"/DropPoints/Random",%DP5);
	addToSet("MissionGroup/Teams/team"@%team@"/DropPoints/Random",%DP6);
	addToSet("MissionGroup/Teams/team"@%team@"/DropPoints/Random",%DP7);
	addToSet("MissionGroup/Teams/team"@%team@"/DropPoints/Random",%DP8);

	gamebase::setteam(%gen, %team);
	gamebase::setteam(%station1, %team);
	gamebase::setteam(%station2, %team);
	gamebase::setteam(%cmdstation, %team);

	gamebase::setteam(%DP1, %team);
	gamebase::setteam(%DP2, %team);
	gamebase::setteam(%DP3, %team);
	gamebase::setteam(%DP4, %team);
	gamebase::setteam(%DP5, %team);
	gamebase::setteam(%DP6, %team);
	gamebase::setteam(%DP7, %team);
	gamebase::setteam(%DP8, %team);

	GameBase::setRotation(%DP1,%rot);
	//GameBase::setPosition(%DP1,Vector::add(%position, "0 -5 3"));
	GameBase::setPosition(%DP1,%posDP1);
	if(GameBase::getLOSInfo(%DP1,100,"-1.570796327 0 0")){
		%Newpos = $los::position;
		GameBase::setPosition(%DP1,vector::add(%Newpos, "0 0 0.1"));
	}
	GameBase::setRotation(%DP2,%rot);
	//GameBase::setPosition(%DP2,Vector::add(%position, "12 12 3"));
	GameBase::setPosition(%DP2,%posDP2);
	if(GameBase::getLOSInfo(%DP2,100,"-1.570796327 0 0")){
		%Newpos = $los::position;
		GameBase::setPosition(%DP2,vector::add(%Newpos, "0 0 0.1"));
	}
	GameBase::setRotation(%DP3,%rot);
	//GameBase::setPosition(%DP3,Vector::add(%position, "-4 -5 3"));
	GameBase::setPosition(%DP3,%posDP3);
	if(GameBase::getLOSInfo(%DP3,100,"-1.570796327 0 0")){
		%Newpos = $los::position;
		GameBase::setPosition(%DP3,vector::add(%Newpos, "0 0 0.1"));
	}
	GameBase::setRotation(%DP4,%rot);
	//GameBase::setPosition(%DP4,Vector::add(%position, "-5 12 3"));
	GameBase::setPosition(%DP4,%posDP4);
	if(GameBase::getLOSInfo(%DP4,100,"-1.570796327 0 0")){
		%Newpos = $los::position;
		GameBase::setPosition(%DP4,vector::add(%Newpos, "0 0 0.1"));
	}
	GameBase::setRotation(%DP5,%rot);
	//GameBase::setPosition(%DP5,Vector::add(%position, "-6 -5 3"));
	GameBase::setPosition(%DP5,%posDP5);
	if(GameBase::getLOSInfo(%DP5,100,"-1.570796327 0 0")){
		%Newpos = $los::position;
		GameBase::setPosition(%DP5,vector::add(%Newpos, "0 0 0.1"));
	}
	GameBase::setRotation(%DP6,%rot);
	//GameBase::setPosition(%DP6,Vector::add(%position, "13 12 3"));
	GameBase::setPosition(%DP6,%posDP6);
	if(GameBase::getLOSInfo(%DP6,100,"-1.570796327 0 0")){
		%Newpos = $los::position;
		GameBase::setPosition(%DP6,vector::add(%Newpos, "0 0 0.1"));
	}
	GameBase::setRotation(%DP7,%rot);
	//GameBase::setPosition(%DP7,Vector::add(%position, "14 -5 3"));
	GameBase::setPosition(%DP7,%posDP7);
	if(GameBase::getLOSInfo(%DP7,100,"-1.570796327 0 0")){
		%Newpos = $los::position;
		GameBase::setPosition(%DP7,vector::add(%Newpos, "0 0 0.1"));
	}
	GameBase::setRotation(%DP8,%rot);
	//GameBase::setPosition(%DP8,Vector::add(%position, "15 12 3"));
	GameBase::setPosition(%DP8,%posDP8);
	if(GameBase::getLOSInfo(%DP8,100,"-1.570796327 0 0")){
		%Newpos = $los::position;
		GameBase::setPosition(%DP8,vector::add(%Newpos, "0 0 0.1"));
	}
	$CommandTents[%team]--;
	$CTLayed[%team] = true;			
	exec(game);
}

function Concrete::place( %player )
{
	%team = GameBase::getTeam(%player);
	%block = newObject("Bunker", InteriorShape,"iblock.dis");

	addToSet("MissionCleanup/team"@%team@"/DropBase",%block);
	%rot1 = GameBase::getRotation(%player);

	%pos = Vector::Add(gamebase::getposition(%player),Vector::getFromRot(%rot1,34));
				
	GameBase::setPosition(%block,vector::add(%pos, "0 0 -4"));
	GameBase::setRotation(%block,%rot1);	
	$Concretes[%team]--;
	
}

function Bunker::place( %player )
{
	%team = GameBase::getTeam(%player);
	%position = Gamebase::getposition(%player);
	%rot = Gamebase::getrotation(%player);
	%position = GetOffSetRot("2 0 -1.9",%rot,%position);

	%rotBnk = %rot;
	%rotGen = %rot;
	%rotSt1 = Vector::Add(%rot,"0 0 -0.7853981635");
	%rotSt2	= Vector::Add(%rot,"0 0 0.7853981635");
	%rotTur = NewRotation(%rot,"back");
	
	%posBnk = %position;
	%posGen = GetOffSetRot("4 -2.5 1.9",%rot,%position);
	%posSt1 = GetOffSetRot("3 3 1.7",%rot,%position);
	%posSt2 = GetOffSetRot("-3.5 3.5 1.7",%rot,%position);
	%posTur = GetOffSetRot("0 3 8",%rot,%position);

	%bunker = newObject("Bunker", InteriorShape,"ebunker.dis");
	%gen = newObject("Bunker", "StaticShape", "PortGenerator", false);
	%station1 = newObject("Bunker", "StaticShape", "InventoryStation", false);
	%station2 = newObject("Bunker", "StaticShape", "AmmoStation", false);
	%turret = newObject("Bunker", "Turret", "PlasmaTurret", false);

	%Group2 = newObject("Bunker", SimGroup,"Group");
	addToSet("MissionCleanup/team"@%team@"/DropBase",%Group2);
	
	addToSet("MissionCleanup/team"@%team@"/DropBase/Bunker",%bunker);
	addToSet("MissionCleanup/team"@%team@"/DropBase/Bunker",%gen);
	addToSet("MissionCleanup/team"@%team@"/DropBase/Bunker",%station1);
	addToSet("MissionCleanup/team"@%team@"/DropBase/Bunker",%station2);
	addToSet("MissionCleanup/team"@%team@"/DropBase/Bunker",%turret);
	
	gamebase::setteam(%gen, %team);
	gamebase::setteam(%station1, %team);
	gamebase::setteam(%station2, %team);
	gamebase::setteam(%turret, %team);
	
	GameBase::setPosition(%bunker,%posBnk);
	GameBase::setRotation(%bunker,%rotBnk);
	
	GameBase::setPosition(%gen,%posGen);
	GameBase::setRotation(%gen,%rotGen);
	
	//GameBase::setPosition(%station1,Vector::add(%position, "3 3 1.7"));
	GameBase::setPosition(%station1,%posSt1);
	GameBase::setRotation(%station1,%rotSt1);
	//GameBase::setRotation(%station1,"0 0 -0.7853981634");
	
	//GameBase::setPosition(%station2,Vector::add(%position, "-3.5 3.5 1.7"));
	GameBase::setPosition(%station2,%posSt2);
	GameBase::setRotation(%station2,%rotSt2);
	//GameBase::setRotation(%station2,"0 0 0.7853981634");
	
	GameBase::setPosition(%turret,%posTur);
	GameBase::setRotation(%turret,%rotTur);

	$Bunkers[%team]--;
}

function Airstrip::place( %player )
{
	%team = GameBase::getTeam(%player);
	%position = Gamebase::getposition(%player);
	%position = vector::add(%position, "0 0 1");
	
	%rot = Gamebase::getrotation(%player);
	%rot = NewRotation(%rot,"back");
	
	%bunker = newObject("Bunker", InteriorShape,"bunker.dis");
	%Strip2 = newObject("Bunker", InteriorShape,"expbridge.dis");
	%gen = newObject("Bunker", "StaticShape", "SolarPanel", false);
	%station1 = newObject("Bunker", "StaticShape", "JetStation", false);
	%station2 = newObject("Bunker", "StaticShape", "JetPad", false);
	
	%rotBnk = %rot;
	%rotStr = Vector::Add(%rot,"3.141592654 0 0");
	%rotGen = %rot;
	%rotSt1 = %rot;
	%rotSt2 = NewRotation(%rot,"back");

	%posBnk = GetOffSetRot("5 10 -1.5",%rot,%position);
	%posStr = GetOffSetRot("0 -80 0",%rot,%position);
	%posGen = GetOffSetRot("0 18 3.3",%rot,%position);
	%posSt1 = GetOffSetRot("0 21 -1",%rot,%position);
	%posSt2 = GetOffSetRot("0 -20 -0.8",%rot,%position);

	%Group2 = newObject("Airstrip", SimGroup,"Group");
	addToSet("MissionCleanup/team"@%team@"/DropBase",%Group2);
		
	addToSet("MissionCleanup/team"@%team@"/DropBase/Airstrip",%bunker);
	addToSet("MissionCleanup/team"@%team@"/DropBase/Airstrip",%strip2);
	addToSet("MissionCleanup/team"@%team@"/DropBase/Airstrip",%gen);
	addToSet("MissionCleanup/team"@%team@"/DropBase/Airstrip",%station1);
	addToSet("MissionCleanup/team"@%team@"/DropBase/Airstrip",%station2);
	
	gamebase::setteam(%gen, %team);
	gamebase::setteam(%station1, %team);
	gamebase::setteam(%station2, %team);
	
	//GameBase::setPosition(%strip2,Vector::add(%position, "0 -80 0"));
	GameBase::setPosition(%strip2,%posStr);
	//GameBase::setRotation(%strip2,"3.141592654 0 0");
	GameBase::setRotation(%strip2,%rotStr);
	
	//GameBase::setPosition(%bunker,Vector::add(%position, "5 10 -1.5"));
	GameBase::setPosition(%bunker,%posBnk);
	GameBase::setRotation(%bunker,%rotBnk);
	
	//GameBase::setPosition(%gen,Vector::add(%position, "0 18 3.3"));
	GameBase::setPosition(%gen,%posGen);
	GameBase::setRotation(%gen,%rotGen);

	//GameBase::setPosition(%station1,Vector::add(%position, "0 21 -1"));
	GameBase::setPosition(%station1,%posSt1);
	GameBase::setRotation(%station1,%rotSt1);

	//GameBase::setPosition(%station2,Vector::add(%position, "0 -20 -0.8"));
	GameBase::setPosition(%station2,%posSt2);
	//GameBase::setRotation(%station2,"0 0 3.141592654");
	GameBase::setRotation(%station2,%rotSt2);
	
	$Airstrips[%team]--;
}

function WatchTower::place( %player )
{
	
	%team = GameBase::getTeam(%player);
	%position = Gamebase::getposition(%player);
	%position = vector::add(%position, "0 0 -3");
	
	%rot = Gamebase::getrotation(%player);
	%rot = NewRotation(%rot,"left");
	
	%posTwr = GetOffSetRot("15 0 0",%rot,%position);
	%posGen = GetOffSetRot("6 -3 10",%rot,%position);
	%posSr1 = GetOffSetRot("4.1 -7.3 40",%rot,%position);
	%posSr2 = GetOffSetRot("25.88 6.7 40",%rot,%position);

	%rotTwr = NewRotation(%rot,"left");
	%rotGen = %rot;
	%rotSr1 = NewRotation(%rot,"");
	%rotSr2 = NewRotation(%rot,"back");

	%tower = newObject("Bunker", InteriorShape,"tank14.dis");
	%gen = newObject("Bunker", "StaticShape", "PortGenerator", false);
	%sensor1 = newObject("Bunker", "Sensor", "MediumPulseSensor", false);
	%sensor2 = newObject("Bunker", "Sensor", "MediumPulseSensor", false);
	
	%Group2 = newObject("WatchTower", SimGroup,"Group");
	addToSet("MissionCleanup/team"@%team@"/DropBase",%Group2);
		
	addToSet("MissionCleanup/team"@%team@"/DropBase/WatchTower",%tower);
	addToSet("MissionCleanup/team"@%team@"/DropBase/WatchTower",%gen);
	addToSet("MissionCleanup/team"@%team@"/DropBase/WatchTower",%sensor1);
	addToSet("MissionCleanup/team"@%team@"/DropBase/WatchTower",%sensor2);
	
	gamebase::setteam(%gen, %team);
	gamebase::setteam(%sensor1, %team);
	gamebase::setteam(%sensor2, %team);
		
	//GameBase::setPosition(%tower,Vector::add(%position, "15 0 0"));
	GameBase::setPosition(%tower,%posTwr);
	//GameBase::setRotation(%tower,"0 0 1.570796327");
	GameBase::setRotation(%tower,%rotTwr);
	
	//GameBase::setPosition(%gen,Vector::add(%position, "6 -3 10"));
	GameBase::setPosition(%gen,%posGen);
	//GameBase::setRotation(%gen,"0 0 0");
	GameBase::setRotation(%gen,%rotGen);
	
	if(GameBase::getLOSInfo(%gen,10,NewRotation(%rot,"down")))
		GameBase::setPosition(%gen,$los::position);

	//GameBase::setPosition(%sensor1,Vector::add(%position, "4.1 -7.3 40"));
	GameBase::setPosition(%sensor1,%posSr1);
	//GameBase::setRotation(%sensor1,"0 0 0");
	GameBase::setRotation(%sensor1,%rotSr1);
	
	//GameBase::setPosition(%sensor2,Vector::add(%position, "25.88 6.7 40"));
	GameBase::setPosition(%sensor2,%posSr2);
	//GameBase::setRotation(%sensor2,"0 0 0");
	GameBase::setRotation(%sensor2,%rotSr2);

	%posTw1 = GetOffSetRot("15 -7 3.5",%rot,%position);
	%posTw2 = GetOffSetRot("4 -8.5 13.5",%rot,%position);
	%posTw3 = GetOffSetRot("1.2 -11 14.5",%rot,%position);

	%rotTw1 = Vector::add(%rot,"-0.8 0 1.570796327");
	%rotTw2 = Vector::add(%rot,"-0.8 0 0");
	%rotTw3 = Vector::add(%rot,"-1.570796327 0 0");
	
	%towerwalk1 = newObject("Bunker", InteriorShape,"logo1.dis");
	%towerwalk2 = newObject("Bunker", InteriorShape,"logo1.dis");
	%towerwalk3 = newObject("Bunker", InteriorShape,"logo3.dis");
		
	addToSet("MissionCleanup/team"@%team@"/DropBase/WatchTower",%towerwalk1);
	addToSet("MissionCleanup/team"@%team@"/DropBase/WatchTower",%towerwalk2);
	addToSet("MissionCleanup/team"@%team@"/DropBase/WatchTower",%towerwalk3);

	//GameBase::setPosition(%towerwalk1,Vector::add(%position, "15 -7 3.5"));
	GameBase::setPosition(%towerwalk1,%posTw1);
	//GameBase::setRotation(%towerwalk1,"-0.8 0 1.570796327");
	GameBase::setRotation(%towerwalk1,%rotTw1);
	
	//GameBase::setPosition(%towerwalk2,Vector::add(%position, "4 -8.5 13.5"));
	GameBase::setPosition(%towerwalk2,%posTw2);
	//GameBase::setRotation(%towerwalk2,"-0.8 0 0");
	GameBase::setRotation(%towerwalk2,%rotTw2);
	
	//GameBase::setPosition(%towerwalk3,Vector::add(%position, "1.2 -11 14.5"));
	GameBase::setPosition(%towerwalk3,%posTw3);
	//GameBase::setRotation(%towerwalk3,"-1.570796327 0 0");
	GameBase::setRotation(%towerwalk3,%rotTw3);
	
	$WatchTowers[%team]--;
}

function ChopperPad::place( %player )
{
	%team = GameBase::getTeam(%player);
	%position = Gamebase::getposition(%player);
	%position = vector::add(%position, "0 0 1");
	
	%rot = Gamebase::getrotation(%player);
	%rot = NewRotation(%rot,"back");
	
	%bunker = newObject("Bunker", InteriorShape,"bunker.dis");
	%pad = newObject("Bunker", InteriorShape,"etower.dis");
	%gen = newObject("Bunker", "StaticShape", "SolarPanel", false);
	%station1 = newObject("Bunker", "StaticShape", "ChopperStation", false);
	%station2 = newObject("Bunker", "StaticShape", "chopperPad", false);
	
	%posBnk = GetOffSetRot("5 10 -1.5",%rot,%position);
	%posPad = GetOffSetRot("0 -20 -36.2",%rot,%position);
	%posGen = GetOffSetRot("0 18 3.3",%rot,%position);
	%posSt1 = GetOffSetRot("0 21 -1",%rot,%position);
	%posSt2 = GetOffSetRot("0 -20 -0.8",%rot,%position);

	%rotBnk = %rot;
	%rotPad = %rot;
	%rotGen = %rot;
	%rotSt1 = %rot;
	%rotSt2 = NewRotation(%rot,"back");

	%Group2 = newObject("ChopperPad", SimGroup,"Group");
	addToSet("MissionCleanup/team"@%team@"/DropBase",%Group2);
		
	addToSet("MissionCleanup/team"@%team@"/DropBase/ChopperPad",%bunker);
	addToSet("MissionCleanup/team"@%team@"/DropBase/ChopperPad",%pad);
	addToSet("MissionCleanup/team"@%team@"/DropBase/ChopperPad",%gen);
	addToSet("MissionCleanup/team"@%team@"/DropBase/ChopperPad",%station1);
	addToSet("MissionCleanup/team"@%team@"/DropBase/ChopperPad",%station2);
	
	gamebase::setteam(%gen, %team);
	gamebase::setteam(%station1, %team);
	gamebase::setteam(%station2, %team);
	
	//GameBase::setPosition(%bunker,Vector::add(%position, "5 10 -1.5"));
	GameBase::setPosition(%bunker,%posBnk);
	GameBase::setRotation(%bunker,%rotBnk);

	//GameBase::setPosition(%pad,Vector::add(%position, "0 -20 -36.2"));
	GameBase::setPosition(%pad,%posPad);
	GameBase::setRotation(%pad,%rotPad);
		
	//GameBase::setPosition(%gen,Vector::add(%position, "0 18 3.3"));
	GameBase::setPosition(%gen,%posGen);
	GameBase::setRotation(%gen,%rotGen);

	//GameBase::setPosition(%station1,Vector::add(%position, "0 21 -1"));
	GameBase::setPosition(%station1,%posSt1);
	GameBase::setRotation(%station1,%rotSt1);

	//GameBase::setPosition(%station2,Vector::add(%position, "0 -20 -0.8"));
	GameBase::setPosition(%station2,%posSt2);
	GameBase::setRotation(%station2,%rotSt2);
	//GameBase::setRotation(%station2,"0 0 3.141592654");

	$ChopperPads[%team]--;
	
}

function TankYard::place( %player )
{
	%team = GameBase::getTeam(%player);
	%position = Gamebase::getposition(%player);
	%position = vector::add(%position, "0 0 1");
	
	%rot = Gamebase::getrotation(%player);
	%rot = NewRotation(%rot,"back");
	
	%bunker = newObject("Bunker", InteriorShape,"bunker.dis");
	%pad = newObject("Bunker", InteriorShape,"COPMfloatingpad.dis");
	%gen = newObject("Bunker", "StaticShape", "SolarPanel", false);
	%station1 = newObject("Bunker", "StaticShape", "TankStation", false);
	%station2 = newObject("Bunker", "StaticShape", "TankPad", false);

	%rotBnk = %rot;
	%rotPad = %rot;
	%rotGen = %rot;
	%rotSt1 = %rot;
	%rotSt2 = NewRotation(%rot,"back");

	%posBnk = GetOffSetRot("5 10 -1.5",%rot,%position);
	%posPad = GetOffSetRot("0 -15 -16.5",%rot,%position);
	%posGen = GetOffSetRot("0 18 3.3",%rot,%position);
	%posSt1 = GetOffSetRot("0 21 -1",%rot,%position);
	%posSt2 = GetOffSetRot("0 -15 -0.8",%rot,%position);
	//%posSt2 = GetOffSetRot("0 -20 -0.8",%rot,%position);

	%Group2 = newObject("TankPad", SimGroup,"Group");
	addToSet("MissionCleanup/team"@%team@"/DropBase",%Group2);
		
	addToSet("MissionCleanup/team"@%team@"/DropBase/TankPad",%bunker);
	addToSet("MissionCleanup/team"@%team@"/DropBase/TankPad",%pad);
	addToSet("MissionCleanup/team"@%team@"/DropBase/TankPad",%gen);
	addToSet("MissionCleanup/team"@%team@"/DropBase/TankPad",%station1);
	addToSet("MissionCleanup/team"@%team@"/DropBase/TankPad",%station2);
	
	gamebase::setteam(%gen, %team);
	gamebase::setteam(%station1, %team);
	gamebase::setteam(%station2, %team);
	
	//GameBase::setPosition(%pad,Vector::add(%position, "0 -15 -16.5"));
	GameBase::setPosition(%pad,%posPad);
	GameBase::setRotation(%pad,%rotPad);
	
	//GameBase::setPosition(%bunker,Vector::add(%position, "5 10 -1.5"));
	GameBase::setPosition(%bunker,%posBnk);
	GameBase::setRotation(%bunker,%rotBnk);
	
	//GameBase::setPosition(%gen,Vector::add(%position, "0 18 3.3"));
	GameBase::setPosition(%gen,%posGen);
	GameBase::setRotation(%gen,%rotGen);

	//GameBase::setPosition(%station1,Vector::add(%position, "0 21 -1"));
	GameBase::setPosition(%station1,%posSt1);
	GameBase::setRotation(%station1,%rotSt1);

	//GameBase::setPosition(%station2,Vector::add(%position, "0 -15 -0.8"));
	GameBase::setPosition(%station2,%posSt2);
	GameBase::setRotation(%station2,%rotSt2);
	//GameBase::setRotation(%station2,"0 0 3.141592654");
	
	$TankYards[%team]--;
}


function RadarUplink::place( %player )
{
	%team = GameBase::getTeam(%player);
	%position = Gamebase::getposition(%player);
	
	%rot = Gamebase::getrotation(%player);
	
	%rotcmd = NewRotation(%rot,"right");
	%rotsat = %rot;
	%rotGen = %rot;
	%rotbun = %rot;
	
	%poscmd = GetOffSetRot("5 20 1",%rot,%position);
	%possat = GetOffSetRot("0 20 12",%rot,%position);
	%posGen = GetOffSetRot("-5 20 1",%rot,%position);
	%posbun = GetOffSetRot("0 20 -7",%rot,%position);
	
	%gen = newObject("Bunker", "StaticShape", "portgenerator", false);
	%cmdstation = newObject("Bunker", "StaticShape", "Commandstation", false);
	%satuplink = newObject("Bunker", "Sensor", "Sataliteuplinkdish", false);
	%bunker = newObject("Bunker", InteriorShape,"bunker6.dis");

	%Group = newObject("RdrUplnk", SimGroup,"Group");
	addToSet("MissionCleanup/team"@%team@"/DropBase",%Group);
		
	addToSet("MissionCleanup/team"@%team@"/DropBase/RdrUplnk",%bunker);
	addToSet("MissionCleanup/team"@%team@"/DropBase/RdrUplnk",%cmdstation);
	addToSet("MissionCleanup/team"@%team@"/DropBase/RdrUplnk",%gen);
	addToSet("MissionCleanup/team"@%team@"/DropBase/RdrUplnk",%satuplink);
	
	gamebase::setteam(%gen, %team);
	gamebase::setteam(%cmdstation, %team);
	gamebase::setteam(%satuplink, %team);
	
	GameBase::setPosition(%satuplink,%possat);
	GameBase::setRotation(%satuplink,%rotsat);
	
	GameBase::setPosition(%bunker,%posbun);
	GameBase::setRotation(%bunker,%rotbun);
	
	GameBase::setPosition(%gen,%posGen);
	GameBase::setRotation(%gen,%rotGen);

	GameBase::setPosition(%cmdstation,%poscmd);
	GameBase::setRotation(%cmdstation,%rotcmd);
	$UplinkCmd[%satuplink] = %cmdstation;

	Satalite::Deploy(%satuplink, %team);

	$RadarUplinks[%team]--;
}


$SataliteInc = 3.0;
$SataliteUpdate = 0.5; 
$SataliteSpawn = 300;

function Satalite::Deploy(%uplink, %team)
{
    %pos = GameBase::getPosition(%uplink);
    %pos = Vector::add(%pos,getRandom()*$SataliteSpawn@" "@getRandom()*$SataliteSpawn@" 501");
    %sat = newObject("GPS Satalite", "Sensor", "Satalite", false);
    addToSet("MissionCleanup/team"@%team@"/DropBase/RdrUplnk",%sat);
    GameBase::setteam(%sat,%team); 
    GameBase::setPosition(%sat,%pos);
    GameBase::setRotation(%sat,"-1.570796327 0 0");
    $Uplink[%sat] = %uplink;
    $Satalite[%uplink] = %sat;
    $TeamSat[%team] = %sat;
    GameBase::startFadeOut(%sat);
}

function Satalite::updatePos(%this)
{
    if(GameBase::getDamageState($Uplink[%this]) == "Enabled") {
        GameBase::setDamageLevel(%this,0);
        %pos = GameBase::getPosition(%this);
        %dest = getWord($SatDest[%this],0)@" "@getWord($SatDest[%this],1)@" "@getWord(%pos,2);
        %dist = Vector::getDistance(%pos,%dest);
        %aim = Vector::getRotAim(%pos,$SatDest[%this]);
        %posInc = Vector::add(%pos,Vector::getFromRot(Vector::getRotAim(%pos,%dest),$SataliteInc)); 
    
        GameBase::setPosition(%this,%posInc);
        GameBase::setRotation(%this,%aim);
        $SatOnRoute[%this] = %dist;
        if(%dist > 5.2 && $SatDest[%this] != ""){

            schedule("Satalite::updatePos("@%this@");",$SataliteUpdate);
	} else {
            $SatOnRoute[%this] = "";
	}
    }
    else
        Satalite::Hault(%this);
}

function Satalite::Hault(%sat)
{
    GameBase::setDamageLevel(%sat,0.99);
}


function NewRotation(%rot,%dir)
{
	%rot1  = %rot;						   //- rotation aim forward
	%rot2  = Vector::Add(%rot,"0 0 3.141592654");   	   //- rotation aim back
	%rot3  = Vector::Add(%rot,"0 0 -1.570796327");  	   //- rotation aim right
	%rot4  = Vector::Add(%rot,"0 0 1.570796327");   	   //- rotation aim left
	%rot5  = Vector::Add(%rot,"1.570796327 0 0");   	   //- rotation aim up
	%rot6  = Vector::Add(%rot,"-1.570796327 0 0");  	   //- rotation aim down
	%rot7  = Vector::Add(%rot,"-0.7853981635 0 0");		   //- rotation aim down/forward
	%rot8  = Vector::Add(%rot,"0.7853981635 0 0");  	   //- rotation aim up/forward
	%rot9  = Vector::Add(%rot,"-3.9269908175 0 0");            //- rotation aim down/back
	%rot10 = Vector::Add(%rot,"3.9269908175 0 0");             //- rotation aim up/back
	%rot11 = Vector::Add(%rot,"0 0 -0.7853981635");            //- rotation aim left/forward
	%rot12 = Vector::Add(%rot,"0 0 0.7853981635");             //- rotation aim right/forward
	%rot13 = Vector::Add(%rot,"0 0 -3.9269908175");            //- rotation aim left/back
	%rot14 = Vector::Add(%rot,"0 0 3.9269908175");             //- rotation aim right/back
	%rot15 = Vector::Add(%rot,"1.570796327 0.7853981635 0");   //- rotation aim up/right
	%rot16 = Vector::Add(%rot,"-1.570796327 0.7853981635 0");  //- rotation aim down/right
	%rot17 = Vector::Add(%rot,"1.570796327 -0.7853981635 0");  //- rotation aim up/left
	%rot18 = Vector::Add(%rot,"-1.570796327 -0.7853981635 0"); //- rotation aim down/left
	if(%dir == "back")
		return %rot2;
	else if(%dir == "right")
		return %rot3;
	else if(%dir == "left")
		return %rot4;
	else if(%dir == "up")
		return %rot5;
	else if(%dir == "down")
		return %rot6;
	else
		return %rot;
}

function GetOffSetRot(%offset,%rot,%pos)
{
	%x = getWord(%offset,0);
	%y = getWord(%offset,1);
	%z = getWord(%offset,2);
	%posA = Vector::add(%pos,Vector::getFromRot(NewRotation(%rot,"right"),%x));
	%posB = Vector::add(%posA,Vector::getFromRot(%rot,%y));
	%posC = Vector::add(%posB,Vector::getFromRot(NewRotation(%rot,"up"),%z));
}

function Vector::getRotAim(%pos1,%pos2,%neg)
{
	%vec = Vector::normalize(Vector::neg(Vector::sub(%pos1,%pos2)));
	if(%neg)
		%vec = Vector::normalize(Vector::sub(%pos1,%pos2));
	%rot = Vector::add(Vector::getRotation(%vec),"1.570796327 0 0");
	return %rot;
}
