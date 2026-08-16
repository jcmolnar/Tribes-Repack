function InitZones()
{
	dbecho($dbechoMode, "InitZones()");

	$numZones = 0;
	%zcnt = 0;

	%group = nameToId("MissionGroup\\Zones");

	if(%group != -1)
	{
		%count = Group::objectCount(%group);
		for(%i = 0; %i <= %count-1; %i++)
		{
			%object = Group::getObject(%group, %i);
			%system = Object::getName(%object);
		      %type = GetWord(%system, 0);
		      %desc = String::getSubStr(%system, String::len(%type)+1, 9999);

			//---------------------------------------------------------------
			//THIS PART GATHERS SOUNDS FOR THE GENERIC UNKNOWN ZONE
			// there is no EXIT sound for the unknown zone.
			//---------------------------------------------------------------
			if(GetWord(%system, 0) == "ENTERSOUND")
			{
				$Zone::EnterSound[0] = GetWord(%system, 1);
			}
			else if(GetWord(%system, 0) == "AMBIENTSOUND")
			{
				$Zone::AmbientSound[0] = GetWord(%system, 1);
				$Zone::AmbientSoundPerc[0] = GetWord(%system, 2);
			}
			//---------------------------------------------------------------
			else
			{
				%zcnt++;

				%tmpgroup = nameToId("MissionGroup\\Zones\\" @ %system);
				%tmpcount = Group::objectCount(%tmpgroup);
				%marker = "";

				for(%z = 0; %z <= %tmpcount-1; %z++)
				{
					%tmpobject = Group::getObject(%tmpgroup, %z);

					if(getObjectType(%tmpobject) == "Marker")
					{
						if(%marker == "")
						{
							%marker = %tmpobject;
							$numZones++;
						}
					}
					else if(getObjectType(%tmpobject) == "SimGroup")
					{
						%n = Object::getName(%tmpobject);

						if(GetWord(%n, 0) == "ENTERSOUND")
						{
							$Zone::EnterSound[%zcnt] = GetWord(%n, 1);
						}
						else if(GetWord(%n, 0) == "AMBIENTSOUND")
						{
							$Zone::AmbientSound[%zcnt] = GetWord(%n, 1);
							$Zone::AmbientSoundPerc[%zcnt] = GetWord(%n, 2);
						}
						else if(GetWord(%n, 0) == "EXITSOUND")
						{
							$Zone::ExitSound[%zcnt] = GetWord(%n, 1);
						}
						else if(GetWord(%n, 0) == "MUSIC")
						{
							$Zone::Music[%zcnt] = GetWord(%n, 1);
							$Zone::MusicTicks[%zcnt] = GetWord(%n, 2);
						}
					}
				}

				%mname = Object::getName(%marker);
				$Zone::Marker[%zcnt] = GameBase::getPosition(%marker);
				$Zone::Length[%zcnt] = GetWord(%mname, 0);
				$Zone::Width[%zcnt] = GetWord(%mname, 1);
				$Zone::Height[%zcnt] = GetWord(%mname, 2);
				$Zone::SHeight[%zcnt] = GetWord(%mname, 3);
				$Zone::Type[%zcnt] = %type;
				$Zone::Desc[%zcnt] = %desc;
				$Zone::FolderID[%zcnt] = %tmpgroup;
			}
		}
		echo($numZones @ " zones initialized.");
	}
}

function RecursiveZone(%delay)
{

	//increment by 1 every $zoneCheckDelay seconds
	$zoneTicker[1]++;
//	$zoneTicker[2]++;

	if($zoneTicker[1] >= 1)		//check zone every 2 seconds for players
	{
		DoZoneCheck(2, %delay);
		$zoneTicker[1] = "";
	}
//	if($zoneTicker[2] >= 15)	//check zone every 30 seconds for bots
//	{
//		DoZoneCheck(1, %delay);
//		$zoneTicker[2] = "";
//	}

	schedule("RecursiveZone(" @ %delay @ ");", %delay);
}

function DoZoneCheck(%w, %d)
{

	//Massive zone check for entire world
	%mset = newObject("set", SimSet);
	%b = 999999;
	%n = containerBoxFillSet(%mset, $SimPlayerObjectType, "0 0 0", %b, %b, %b, 0);

	for(%z = 1; %z <= $numZones; %z++)
	{
		%set = newObject("set", SimSet);
		%n = containerBoxFillSet(%set, $SimPlayerObjectType, $Zone::Marker[%z], $Zone::Length[%z], $Zone::Width[%z], $Zone::Height[%z], $Zone::SHeight[%z]);
		Group::iterateRecursive(%set, setzoneflags, %z);
		deleteObject(%set);
	}

	Group::iterateRecursive(%mset, UpdateZone);
	deleteObject(%mset);
}
function setzoneflags(%object, %z) {

	%Client = Player::getClient(%object);
	$tmpzone[%Client] = %z;
}

function UpdateZone(%object)
{

	%Client = Player::getClient(%object);

	%zoneflag = $tmpzone[%Client];

	//check if the player was found inside a zone
	if(%zoneflag != "")
	{
		//the player is inside a zone!

		//check if the player's current zone matches the one he's detected in
		if($zone[%Client] != $Zone::FolderID[%zoneflag])
		{
			//the client's current zone does not match the one he really is in, so boot the player out of his
			//current zone (if any)
			if($zone[%Client] != "")
				Zone::DoExit(Zone::getIndex($zone[%Client]), %Client);
			//throw the player inside this new zone
			Zone::DoEnter(%zoneflag, %Client);
		}
		else
		{
			//the client is in the same zone as he was since the last zonecheck
		//	if($Zone::AmbientSound[%zoneflag] != "" && !Player::isAiControlled(%Client))
		//	{
		//		%m = $Zone::AmbientSoundPerc[%zoneflag];
		//		if(%m == "") %m = 100;
//
		//		%r = floor(getRandom() * 100)+1;
		//		if(%r <= %m)
		//			Client::sendMessage(%Client, 0, "~w" @ $Zone::AmbientSound[%zoneflag]);
		//	}
		//	if($Zone::Music[%zoneflag] != "" && !Player::isAiControlled(%Client))
		//	{
		//		if(%Client.MusicTicksLeft < 1)
		//		{
		//			Client::sendMessage(%Client, 0, "~w" @ $Zone::Music[%zoneflag]);
		//			%Client.MusicTicksLeft = $Zone::MusicTicks[%zoneflag]+2;
		//		}
		//	}
			if($Zone::Type[%zoneflag] == "WATER")
			{
				if(!IsDead(%Client))
				{
					%noDrown = "";
					for(%i = 1; (%orb = $ItemList[Orb, %i]) != ""; %i++)
					{
						if($ProtectFromWater[%orb])
						{
							//if(Player::getItemCount(%Client, %orb @ "0"))
							if(Client::getItemCount(%Client, %orb, "EquipList"))
							{

								$drownCounter[%Client] = 0;
								%noDrown = True;
								break;
							}
						}
					}

					if(!%noDrown)
					{
						%dn = 10;

						$drownCounter[%Client]++;

						if((%dc = $drownCounter[%Client]) > %dn)
						{
							%dmg = Cap(floor(pow((%dc - %dn) / 1.2, 2)), 1.0, 1000) * "0.01";
							GameBase::virtual(%Client, "onDamage", 0, %dmg, "0 0 0", "0 0 0", "0 0 0", "torso", "front_right", %Client);
							%snd = radnomItems(3, SoundDrown1, SoundDrown2, SoundDrown3);
							playSound(%snd, GameBase::getPosition(%Client));
						}
					}
				}
			}
		}

		//this simulates underwater
		//if($Zone::Type[%zoneflag] == "WATER")
		//	if($underwaterEffects)
		//		gravWorkaround(%Client, 1);
	}
	else
	{
		//the player is not inside any zone.
		//if the player has a current zone, then we need to kick him out of it
			if($zone[%Client] != "")
			Zone::DoExit(Zone::getIndex($zone[%Client]), %Client);

		//start playing the ambient sound for the unknown zone
		if($Zone::AmbientSound[0] != "" && !Player::isAiControlled(%Client))
		{
			%m = $Zone::AmbientSoundPerc[0];
			if(%m == "") %m = 100;

			%r = floor(getRandom() * 100)+1;
			if(%r <= %m)
		//		Client::sendMessage(%Client, 0, "~w" @ $Zone::AmbientSound[0]);
				remoteEval(%Client, SFXPLAY, 1, $Zone::AmbientSound[0], $Zone::MusicTicks[%z]+1); //
		}

		//play the enter sound for the unknown zone
		if($Zone::EnterSound[0] != "" && !Player::isAiControlled(%Client))
			Client::sendMessage(%Client, 0, "~w" @ $Zone::EnterSound[0]);
	}

	//-----------------------------------------------------------
	// Decrease music ticks
	//-----------------------------------------------------------
//	if(%Client.MusicTicksLeft > 0)
//		%Client.MusicTicksLeft--;
	
	
//	%eqlist = $ClientData[%Client, "EquipList"];
//	for(%eqi = 0; (%eqw = GetWord(%eqlist, %eqi)) != -1; %eqi+=2) {
//		if(String::findsubstr($instrumentList, ","@%eqw@",") != -1)
//		{
		//	echo("area effect "@%eqw);
		//	%radius = 50;
		//	%b = %radius * 2;
		//	%set = newObject("set", SimSet);
		//	%n = containerBoxFillSet(%set, $SimPlayerObjectType, GameBase::getPosition(%Client), %b, %b, %b, 0);
		//	
		//	Group::iterateRecursive(%set, DoInstrumentBoxFunction, %Client, %eqw);
		//	deleteObject(%set);
//		}
//	}
	

	//-----------------------------------------------------------
	// Decrease bonus state ticks
	//-----------------------------------------------------------
	DecreaseBonusStateTicks(%Client);

	//----
	if(!Player::isAiControlled(%Client)) {
		refreshSTAMINA(%Client, -$STAREGEN[%Client]);
		if(CheckBonus(%Client, "Alvl"))
			SimDrunk(%Client, $ClientData[%Client, Alvl]--);
		else
			$ClientData[%Client, Alvl] = "";
	}
	//----

	//-----------------------------------------------------------
	// Check if the player has moved since last ZoneCheck
	//-----------------------------------------------------------
	%pos = GameBase::getPosition(%Client);
	if(%pos != %Client.zoneLastPos && !IsDead(%Client))
	{
		//cycle thru orbs
		for(%i = 1; (%orb = $ItemList[Orb, %i]) != ""; %i++)
		{
			if(OddsAre($BurnOut[%orb]))
			{
				//if(Player::getItemCount(%Client, %orb @ "0"))
				if(Client::getItemCount(%Client, %orb, "EquipList"))
				{
					Client::sendMessage(%Client, $MsgRed, "Your " @ $ItemData[%orb, Name] @ " has burned out.");
					Client::addItemCount(%Client, %orb, -1);
					RefreshAll(%Client);
				}
			}
			if($BurnOutInRain[%orb] > 0)
			{

				if($zone[%Client] == "" && $isRaining)
				{
					if(OddsAre($BurnOutInRain[%orb]))
					{
						if(Client::getItemCount(%Client, %orb, "EquipList"))
						{
							Client::sendMessage(%Client, $MsgRed, "The rain has burned out your " @ $ItemData[%orb, Name] @ ".");
							Client::addItemCount(%Client, %orb, -1);
							RefreshAll(%Client);
						}
					}
				}
			}
		}

		//hard-coded list to save on CPU
		for(%z = 1; $ItemList[Badge, %z] != ""; %z++)
		{
			//if(Player::getItemCount(%Client, $ItemList[Badge, %z]))
			if(Client::getItemCount(%Client, $ItemList[Badge, %z]))
			{
				%a = GetWord($BonusItem[$ItemList[Badge, %z]], 0);
				%b = GetWord($BonusItem[$ItemList[Badge, %z]], 1);
				%c = GetWord($BonusItem[$ItemList[Badge, %z]], 2);

				if(OddsAre(%c))
					GiveThisStuff(%Client, %a @ " " @ %b, True);
			}
		}
	}
	%Client.zoneLastPos = %pos;


	$tmpzone[%Client] = "";
}

function gravWorkaround(%Client, %method)
{

	if(%method == 1)
	{
		%rzdelay = 2;
		%steps = 24;

		for(%i = 0; %i < %steps; %i++)
		{
			%d = %i / (%steps / %rzdelay);
			schedule("Player::applyImpulse(" @ %Client @ ", \"0 0 13\");", %d, %Client);
		}
	}
	else if(%method == 2)
	{
		if($xyvel == "") $xyvel = 0.8;
		if($nzvel == "") $nzvel = 0.2;
		if($pzvel == "") $pzvel = 1.0;
		if($impulse == "") $impulse = 4;

		Player::applyImpulse(%Client, "0 0 " @ $impulse);

		%vel = Item::getVelocity(%Client);

		%xvel = GetWord(%vel, 0) * $xyvel;
		%yvel = GetWord(%vel, 1) * $xyvel;
		%zvel = GetWord(%vel, 2);

		if(%zvel < 0)
			%zvel *= $nzvel;
		else
			%zvel *= $pzvel;

		%nvel = %xvel @ " " @ %yvel @ " " @ %zvel;

		Item::setVelocity(%Client, %nvel);
	}
}

function Zone::DoEnter(%z, %Client)
{

	%oldZone = $zone[%Client];
	%newZone = $Zone::FolderID[%z];

	$zone[%Client] = $Zone::FolderID[%z];

	if($Zone::Type[%z] == "PROTECTED")
	{
		%msg = "You have entered " @ $Zone::Desc[%z] @ ". This is protected territory.";
		%color = $MsgBeige;

		if($ClientData[%Client, "isMimic"]) {
			$ClientData[%Client, "isMimic"] = "";
			ChangeRace(%Client, "Human");
		}

	}
	else if($Zone::Type[%z] == "DUNGEON")
	{
		%msg = "You have entered " @ $Zone::Desc[%z] @ ". Beware of enemies!";
		%color = $MsgRed;
	}
	else if($Zone::Type[%z] == "WATER")
	{
		%msg = "";
	}
	else if($Zone::Type[%z] == "FREEFORALL")
	{
		%msg = "You have entered " @ $Zone::Desc[%z] @ ".";
		%color = $MsgRed;
	}

	remoteEval(%Client, SFXPLAY, 1, $Zone::Music[%z], $Zone::MusicTicks[%z]+1); //

	if($Zone::EnterSound[%z] != "")
		Client::sendMessage(%Client, 0, "~w" @ $Zone::EnterSound[%z]);

	if(%msg != "" && !Player::isAiControlled(%Client)) {
		%msg = FixSpaces(%msg);
		remoteEval(%Client, "ZONEText", %msg);
		//Game::refreshClientScore(%Client);	//this is so players can see which zone this client is in
	}
	Zone::onEnter(%Client, %oldZone, %newZone);
}

function Zone::DoExit(%z, %Client)
{

	%zoneLeft = $zone[%Client];

	$zone[%Client] = "";

	if($Zone::Type[%z] == "PROTECTED")
	{
		%msg = "You have left " @ $Zone::Desc[%z] @ ".";
		%color = $MsgRed;
	}
	else if($Zone::Type[%z] == "DUNGEON")
	{
		%msg = "You have left " @ $Zone::Desc[%z] @ ".";
		%color = $MsgBeige;
	}
	else if($Zone::Type[%z] == "WATER")
	{
		%msg = "";
	}
	else if($Zone::Type[%z] == "FREEFORALL")
	{
		%msg = "You have left " @ $Zone::Desc[%z] @ ".";
		%color = $MsgBeige;
	}

	remoteEval(%Client, SFXSTOP); //

	if($Zone::ExitSound[%z] != "")
		%msg = %msg @ "~w" @ $Zone::ExitSound[%z];

	//if(%msg != "")
	//      Client::sendMessage(%Client, %color, %msg);

	//if(!Player::isAiControlled(%Client))
	//	Game::refreshClientScore(%Client);	//this is so players can see which zone this client is in

	Zone::onExit(%Client, %zoneLeft);
}

function IsInBetween(%x, %r1, %r2)
{

	if(%r1 > %r2)
	{
		%tmp = %r1;
		%r1 = %r2;
		%r2 = %tmp;
	}
	if(%x >= %r1 && %x <= %r2)
		return True;
	else
		return False;
}

function Zone::getIndex(%z)
{

	if(%z != "")
	{
		for(%i = 1; %i <= $numZones; %i++)
		{
			if($Zone::FolderID[%i] == %z)
			{
				return %i;
			}
		}
	}
	return -1;
}
function Zone::getMarker(%z)
{

	if(%z != "")
	{
		for(%i = 1; %i <= $numZones; %i++)
		{
			if($Zone::FolderID[%i] == %z)
			{
				return $Zone::Marker[%i];
			}
		}
	}
	return -1;
}
function Zone::getType(%z)
{
	if(%z != "")
	{
		for(%i = 1; %i <= $numZones; %i++)
		{
			if($Zone::FolderID[%i] == %z)
			{
				return $Zone::Type[%i];
			}
		}
	}
	return -1;
}
function Zone::getDesc(%z)
{

	if(%z != "")
	{
		for(%i = 1; %i <= $numZones; %i++)
		{
			if($Zone::FolderID[%i] == %z)
			{
				return $Zone::Desc[%i];
			}
		}
	}
	return -1;
}
function Zone::getEnterSound(%z)
{

	if(%z != "")
	{
		for(%i = 1; %i <= $numZones; %i++)
		{
			if($Zone::FolderID[%i] == %z)
			{
				return $Zone::EnterSound[%i];
			}
		}
	}
	return -1;
}
function Zone::getAmbientSound(%z)
{

	if(%z != "")
	{
		for(%i = 1; %i <= $numZones; %i++)
		{
			if($Zone::FolderID[%i] == %z)
			{
				return $Zone::AmbientSound[%i];
			}
		}
	}
	return -1;
}
function Zone::getAmbientSoundPerc(%z)
{

	if(%z != "")
	{
		for(%i = 1; %i <= $numZones; %i++)
		{
			if($Zone::FolderID[%i] == %z)
			{
				return $Zone::AmbientSoundPerc[%i];
			}
		}
	}
	return -1;
}
function Zone::getExitSound(%z)
{

	if(%z != "")
	{
		for(%i = 1; %i <= $numZones; %i++)
		{
			if($Zone::FolderID[%i] == %z)
			{
				return $Zone::ExitSound[%i];
			}
		}
	}
	return -1;
}

function Zone::onEnter(%Client, %oldZone, %newZone)
{

	refreshHPREGEN(%Client);	//this is because you regen faster or slower depending on the zone you are in
	refreshSTAREGEN(%Client);

	if(Zone::getType(%newZone) == "WATER") {
		//Client::sendMessage(%Client, $MsgBeige, "You have entered water!");
		$drownCounter[%Client] = "";
	}
	if(Zone::getType(%newZone) == "PROTECTED") {

		if($ClientData[%Client, "isMimic"]) {
			$ClientData[%Client, "isMimic"] = "";
			ChangeRace(%Client, "Human");
			UpdateTeam(%Client);
			RefreshAll(%Client);

			playSound(AbsorbABS, GameBase::getPosition(%Client));
		}
	}
}

function Zone::onExit(%Client, %zoneLeft)
{

	refreshHPREGEN(%Client);	//this is because you regen faster or slower depending on the zone you are in

	if(Zone::getType(%zoneLeft) == "WATER")
	{
		//Client::sendMessage(%Client, $MsgBeige, "You have left water!");
		$drownCounter[%Client] = "";
	}

	Zone::CheckZone(%zoneLeft);
}

function GetNearestZone(%Client, %zonetype, %returnType)
{

	//%zonetype can be "town", "dungeon" or "freeforall"

	%closestDist = 500000;
	%closestZone = "";
	%mpos = "";
	%clpos = GameBase::getPosition(%Client);

	for(%i = 1; %i <= $numZones; %i++)
	{
		%type = $Zone::Type[%i];
		if(%type == "PROTECTED" && String::ICompare(%zonetype, "town") == 0 || %type == "DUNGEON" && String::ICompare(%zonetype, "dungeon") == 0 || %type == "FREEFORALL" && String::ICompare(%zonetype, "freeforall") == 0 || %zonetype == -1)
		{
			%finalpos = $Zone::Marker[%i];

			%dist = Vector::getDistance(%finalpos, %clpos);
			if(%dist < %closestDist)
			{
				%closestDist = %dist;
				%closestZoneDesc = $Zone::Desc[%i];
				%closestZone = $Zone::FolderID[%i];
				%mpos = %finalpos;
			}
		}
	}

	if(%mpos == "")		//no zones were found (this means there are NO zones in the map...)
		return False;

	//%returnType:
	//1 = returns the distance from the client to the nearest zone
	//2 = returns the description of the zone nearest to the client
	//3 = returns the zone id of the zone nearest to the client
	//4 = returns the position of the middle of the zone nearest to the client

	if(%returnType == 1)
		return %closestDist;
	else if(%returnType == 2)
		return %closestZoneDesc;
	else if(%returnType == 3)
		return %closestZone;
	else if(%returnType == 4)
		return %mpos;
}

function _GetNearestZone(%Client) {

	%closestDist = 500000;
	%closestZone = "";
	%mpos = "";
	%clpos = GameBase::getPosition(%Client);

	for(%i = 1; %i <= $numZones; %i++) {
		%type = $Zone::Type[%i];
		%finalpos = $Zone::Marker[%i];

		%dist = Vector::getDistance(%finalpos, %clpos);
		if(%dist < %closestDist) {
			%closestDist = %dist;
			%closestZoneDesc = $Zone::Desc[%i];
			%closestZone = $Zone::FolderID[%i];
			%mpos = %finalpos;
		}
	}

	if(%mpos == "")		//no zones were found (this means there are NO zones in the map...)
		return False;

	return %closestZoneDesc;

}

function GetZoneByKeywords(%Client, %keywords, %returnType)
{

	%mpos = "";

	%group = nameToId("MissionGroup\\Zones");

	if(%group != -1)
	{
		//IMPORTANT: zone markers must be objects 0 and 1 in the zone's folder

		%count = Group::objectCount(%group);
		for(%i = 0; %i <= %count-1; %i++)
		{
			%object = Group::getObject(%group, %i);
			%system = Object::getName(%object);
			%type = GetWord(%system, 0);
			%desc = String::getSubStr(%system, String::len(%type)+1, 9999);
			if(%type == "PROTECTED" || %type == "DUNGEON" || %type == "FREEFORALL")
			{
				if(String::findSubStr(%desc, %keywords) != -1)
				{
					//get the two markers
					%tmpgroup = nameToId("MissionGroup\\Zones\\" @ %system);

					%m1pos = GameBase::getPosition(Group::getObject(%tmpgroup, 0));
					%m2pos = GameBase::getPosition(Group::getObject(%tmpgroup, 1));

					%mx = (((GetWord(%m2pos, 0) - GetWord(%m1pos, 0)) / 2) + GetWord(%m1pos, 0));
					%my = (((GetWord(%m2pos, 1) - GetWord(%m1pos, 1)) / 2) + GetWord(%m1pos, 1));
					%mz = (((GetWord(%m2pos, 2) - GetWord(%m1pos, 2)) / 2) + GetWord(%m1pos, 2));

					%mpos = %mx @ " " @ %my @ " " @ %mz;
					%dist = Vector::getDistance(%mpos, GameBase::getPosition(%Client));

					//%returnType:
					//1 = returns the distance from the client to the zone
					//2 = returns the description of the zone
					//3 = returns the zone id
					//4 = returns the position of the middle of the zone

					if(%returnType == 1)
						return %dist;
					else if(%returnType == 2)
						return %desc;
					else if(%returnType == 3)
						return %object;
					else if(%returnType == 4)
						return %mpos;
				}
			}
		}
		return False;
	}
	else
		return False;
}

function Zone::getNumPlayers(%z, %all)
{

	if(%all)
		%list = GetEveryoneIdList();
	else
		%list = GetPlayerIdList();

	%n = 0;
	for(%i = 0; (%id = GetWord(%list, %i)) != -1; %i++) {

		if($zone[%id] == %z)
			%n++;
	}

	return %n;
}

function ObjectInWhichZone(%object)
{

	//not perfect but good enough

	%fid = "";
	%closest = 99999;
	%objpos = GameBase::getPosition(%object);
	for(%z = 1; %z <= $numZones; %z++)
	{
		%rad = ($Zone::Length[%z] + $Zone::Width[%z] + $Zone::Height[%z]) / 3;
		%dist = Vector::getDistance(%objpos, $Zone::Marker[%z]);
		if(%dist <= %rad)
		{
			if(%dist < %closest)
			{
				%closest = %dist;
				%fid = $Zone::FolderID[%z];
				$ZoneSpawnId = $Zone::Desc[%z];
			}
		}
	}
	return %fid;
}

function Zone::getPlayerList(%z, %type)
{

	if(%type == 1)
		%list = GetEveryoneIdList();
	else if(%type == 2)
		%list = GetPlayerIdList();
	else if(%type == 3)
		%list = GetBotIdList();

	%n = 0;
	%aa = "";
	for(%i = 0; (%id = GetWord(%list, %i)) != -1; %i++) {
		if($zone[%id] == %z)
			%aa = %aa @ %id @ " ";
	}

	return %aa;
}

function Zone::CheckZone(%zone) {

	%list = Zone::getPlayerList(%zone, 2);

	if(%list == "") { // No players found!

		%bots = Zone::getPlayerList(%zone, 3);
		if(%bots != "") { // Let the killing begin!

			echo("Zone "@Zone::getDesc(%zone)@" empty, killing "@%bots);

			for(%i = 0; (%id = GetWord(%bots, %i)) != -1; %i++) {
				Schedule("ScheduleKillBot("@%id@");", %i);
			}
		}
	}
}

function ScheduleKillBot(%id) {

	%aiName = clipTrailingNumbers($BotInfoAiName[%id]); //echo("CheckZone: "@%aiName);
	$tmpBotCnt = gettmpBotCnt(%aiName);

	%class = "CLASS "@$CLASS[%id]@" ";
	%lvl = "LVL "@getFinalLVL(%id)@" ";
	%coins = "COINS "@floor($COINS[%id])@" ";
	%lck = "LCK "@floor(getFinalLCK(%id))@" ";

//	%string = "";
//	if($AP[%id, 1] > 0)
//		%string = %string@"STR "@floor($AP[%id, 1])@" ";
//	if($AP[%id, 2] > 0)
//		%string = %string@"DEX "@floor($AP[%id, 2])@" ";
//	if($AP[%id, 3] > 0)
//		%string = %string@"CON "@floor($AP[%id, 3])@" ";
//	if($AP[%id, 4] > 0)
//		%string = %string@"INT "@floor($AP[%id, 4])@" ";
//	if($AP[%id, 5] > 0)
//		%string = %string@"WIS "@floor($AP[%id, 5])@" ";

	%string = %class@%lvl@%coins@%lck@%string;

	$tmpBotStuff[%aiName, $tmpBotCnt] = %string@$ClientData[%id, ItemList];
	$tmpBotStuffEquip[%aiName, $tmpBotCnt] = $ClientData[%id, EquipList];

	$noDropLootbagFlag[%id] = True;
	Player::Kill(%id);
}

function gettmpBotCnt(%aiName) {
	for(%i = 0; %i <= 5000; %i++) {
		if($tmpBotStuff[%aiName, %i] == "") {
			if(%i >= $tmpBotStuffMAX)
				$tmpBotStuffMAX = %i;
			return %i;
		}
	}
}