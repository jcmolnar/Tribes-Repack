function GroupTrigger::onTrigEnter(%object, %this) {

	%clientId = Player::getClient(%this);
	if(Client::getTeam(%clientId) != 0)
		return;

	%flag = "";
	%g = Object::getName(getGroup(%object));
	if(String::ICompare(%g, "sleep") == 0)
		%flag = True;
	else if(String::findSubStr(%g, "Camp") != -1) {
		%id = String::getSubStr(%g, 4, 99999);
		if(%clientId == %id || IsInCommaList($grouplist[%id], Client::getName(%clientId)))
			%flag = True;
	}
	else if(String::getSubStr(%g, 0, 5) == "sleep")
		%flag = True;

	if(%flag) {
		if($InSleepZone[%clientId] == "") {
			$InSleepZone[%clientId] = %object;

			%time = getIntegerTime(true) >> 5;
			if(%time - %clientId.lastTriggerTime > $triggerDelay) {
				%clientId.lastTriggerTime = %time;
				Client::sendMessage(%clientId, $MsgBeige, "You feel that this area would make a suitable place to #sleep");
			}
		}
	}
	else if(String::ICompare(Object::getName(getGroup(getGroup(getGroup(%object)))), "TeleportBoxes") == 0) {
		//echo("entered teleporter box");

		%group = getGroup(getGroup(%object));
		%count = Group::objectCount(%group);
		for(%i = 0; %i <= %count-1; %i++)
		{
			%object = Group::getObject(%group, %i);
			if(getObjectType(%object) == "SimGroup")
			{
				%system = Object::getName(%object);
				%type = GetWord(%system, 0);
				%info = String::getSubStr(%system, String::len(%type)+1, 9999);

				if(%type == "NEED")
					%need = %info;
				else if(%type == "TAKE")
					%take = %info;
				else if(%type == "NSAY")
					%nsay = %info;
				else if(%type == "GSAY")
					%gsay = %info;
				else if(%type == "ASAY") {
					%color = getWord(%system, 1);
					%asay = String::getSubStr(%system, (String::len(%type)+String::len(%color)+2), 9999);
					schedule("Client::sendMessage(" @ %clientId @ ", "@%color@", \"" @ %asay @ "\");", 0.22);
				}
			}
		}
		%h = HasThisStuff(%clientId, %need);
		if(%h != 667 && %h != 666 && %h != False) {
			TakeThisStuff(%clientId, %take);

			%pos = TeleportToMarker(%clientId, "TeleportBoxes\\" @ Object::getName(%group) @ "\\Output", False, True);

			if(%asay == "")
				Player::setDamageFlash(%clientId, 0.9);

			if(!$invisible[%clientId])
				GameBase::startFadeIn(%clientId);

			RefreshAll(%clientId);

			if(getWord(%gsay, 1) != -1)
				schedule("Client::sendMessage(" @ %clientId @ ", $MsgBeige, \"" @ %gsay @ "\");", 0.22);
		}
		else
			Client::sendMessage(%clientId, $MsgRed, %nsay);
	}
}
//$RM::trigRot[CataCombsToUpper] = "0 -0 -1.56514"; //from CataCombs to upper
//$RM::trigRot[UpperToCataCombs] = "0 -0 -3.13063"; //from upper to catacombs

//$RM::trigRot[CataCombsToRojo] = "0 -0 1.73507"; //from CataCombs to Rojo
//$RM::trigRot[RojoToCataCombs] = "0 -0 -0.0164564";  //from Rojo to Catacombs

//$RM::trigRot[RojoToBloodan] = "0 -0 -1.86384"; //from Rojo to bloodan
//$RM::trigRot[BloodanToRojo] = "0 -0 -1.44679";//from bloodan to Rojo

//$RM::trigRot[UpperGoobaToHowlingEarth] = "0 -0 -1.19202"; //from upper gooba to howling earth
//$RM::trigRot[] = "0 -0 0.0501071"; //to upper gooba from

//$RM::trigRot[LowerGoobaToUpperGooba] = "0 -0 -1.53319"; //from lower gooba to upper gooba
//$RM::trigRot[UpperGoobaToLowerGooba] = "0 -0 -2.06812"; //from upper gooba to lower gooba

function GroupTrigger::onTrigLeave(%object, %this) {

	%clientId = Player::getClient(%this);

	if(Client::getTeam(%clientId) != 0)
		return;

	if($InSleepZone[%clientId] != "")
	{
		$InSleepZone[%clientId] = "";

		%time = getIntegerTime(true) >> 5;
		if(%time - %clientId.lastTriggerTime > $triggerDelay)
		{
			%clientId.lastTriggerTime = %time;
//		      Client::sendMessage(%clientId, $MsgGreen, "Teleportation was succesful.");
		}
	}

	//if(String::ICompare(Object::getName(getGroup(getGroup(getGroup(%object)))), "TeleportBoxes") == 0)
	//	echo("left teleporter box");
}

function DoCampSetup(%clientId, %step, %pos) {
	if(%pos != "") {
		if(Vector::getDistance(GameBase::getPosition(%clientId), %pos) > 10) {
			if(GameBase::getPosition(Group::getObject("MissionCleanup/Camp" @ %clientId, 0)) != "0 0 0") {
				Client::sendMessage(%clientId, $MsgRed, "You have wandered too far from your camp while setting it up.");
				%step = 5;
			}
			else
				return;
		}
	}
	if(%step == 1) {
		%object = newObject("wood", InteriorShape, "wood.dis");
		addToSet("MissionCleanup\\Camp" @ %clientId, %object);

		%x = GetWord(%pos, 0);
		%y = GetWord(%pos, 1);
		%z = GetWord(%pos, 2);
		%npos = (%x + 1) @ " " @ (%y + 0) @ " " @ (%z + 0);
		GameBase::setPosition(%object, %npos);
	}
	else if(%step == 2) {
		%old = nameToId("MissionCleanup\\Camp" @ %clientId @ "\\wood");
		deleteObject(%old);

		%object = newObject("woodfire", InteriorShape, "woodfire.dis");
		addToSet("MissionCleanup\\Camp" @ %clientId, %object);

		%x = GetWord(%pos, 0);
		%y = GetWord(%pos, 1);
		%z = GetWord(%pos, 2);
		%npos = (%x + 1) @ " " @ (%y + 0) @ " " @ (%z + 0);
		GameBase::setPosition(%object, %npos);
	}
	else if(%step == 3) {
		%object = newObject("tent", InteriorShape, "tent.dis");
		addToSet("MissionCleanup\\Camp" @ %clientId, %object);

		%x = GetWord(%pos, 0);
		%y = GetWord(%pos, 1);
		%z = GetWord(%pos, 2);
		%npos = (%x + 5) @ " " @ (%y + 0) @ " " @ (%z + 0);
		GameBase::setPosition(%object, %npos);
	}
	else if(%step == 4) {
		%object = newObject("sleepzone", Trigger, GroupTrigger);
		addToSet("MissionCleanup\\Camp" @ %clientId, %object);

		%x = GetWord(%pos, 0);
		%y = GetWord(%pos, 1);
		%z = GetWord(%pos, 2);
		%npos = (%x + 8) @ " " @ (%y + 0) @ " " @ (%z + 2);
		GameBase::setPosition(%object, %npos);

		Client::sendMessage(%clientId, $MsgBeige, "Finished setting up camp. Use #uncamp to pack up.");
	}
	else if(%step == 5)
	{
		%g = "MissionCleanup/Camp" @ %clientId;

		Client::addItemCount(%clientId, Tent, 1);
		RefreshAll(%clientId);

		//so the players in the grouptrigger get kicked out first.
		Group::iterateRecursive(%g, GameBase::setPosition, "0 0 0");

		%gg = nameToId(%g);
		schedule("deleteObject(" @ %gg @ ");", 5);
	}
}