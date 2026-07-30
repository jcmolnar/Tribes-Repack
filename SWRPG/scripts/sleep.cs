function GroupTrigger::onTrigEnter(%object, %this)
{
	dbecho($dbechoMode, "GroupTrigger::onTrigEnter(" @ %object @ ", " @ %this @ ")");

      %clientId = Player::getClient(%this);

	%flag = "";
	%g = Object::getName(getGroup(%object));
	if(String::ICompare(%g, "sleep") == 0)
		%flag = True;
	else if(String::findSubStr(%g, "Camp") != -1)
	{
		%id = String::getSubStr(%g, 4, 99999);
		if(%clientId == %id || IsInCommaList(fetchData(%id, "grouplist"), Client::getName(%clientId)))
			%flag = True;
	}

	if(%flag)
	{
		if(fetchData(%clientId, "InSleepZone") == "")
		{
			storeData(%clientId, "InSleepZone", %object);

			%time = getIntegerTime(true) >> 5;
			if(%time - %clientId.lastTriggerTime > $triggerDelay)
			{
				%clientId.lastTriggerTime = %time;
			      Client::sendMessage(%clientId, $MsgBeige, "You feel that this area would make a suitable place to #sleep");
			}
		}
	}
	else if(String::ICompare(Object::getName(getGroup(getGroup(getGroup(%object)))), "TeleportBoxes") == 0)
	{
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
			}
		}

		%h = HasThisStuff(%clientId, %need);
		if(%h != 667 && %h != 666 && %h != False)
		{
			TakeThisStuff(%clientId, %take);

			%pos = TeleportToMarker(%clientId, "TeleportBoxes\\" @ Object::getName(%group) @ "\\Output", False, True);
			Player::setDamageFlash(%clientId, 0.9);
			if(!fetchData(%clientId, "invisible"))
				GameBase::startFadeIn(%clientId);

			RefreshAll(%clientId);

			schedule("Client::sendMessage(" @ %clientId @ ", $MsgBeige, \"" @ %gsay @ "\");", 0.22);
		}
		else
			Client::sendMessage(%clientId, $MsgRed, %nsay);
	}
}
function GroupTrigger::onTrigLeave(%object, %this)
{
	dbecho($dbechoMode, "GroupTrigger::onTrigLeave(" @ %object @ ", " @ %this @ ")");

      %clientId = Player::getClient(%this);

	if(fetchData(%clientId, "InSleepZone") != "")
	{
		storeData(%clientId, "InSleepZone", "");

		%time = getIntegerTime(true) >> 5;
		if(%time - %clientId.lastTriggerTime > $triggerDelay)
		{
			%clientId.lastTriggerTime = %time;
		      Client::sendMessage(%clientId, $MsgBeige, "You have left the sleeping area.");
		}
	}
	
	if(String::ICompare(Object::getName(getGroup(getGroup(getGroup(%object)))), "TeleportBoxes") == 0)
	{
		//echo("left teleporter box");
	}
}

function DoCampSetup(%clientId, %step, %pos)
{
	dbecho($dbechoMode, "DoCampSetup(" @ %clientId @ ", " @ %step @ ", " @ %pos @ ")");

	if(%pos != "")
	{
		if(Vector::getDistance(GameBase::getPosition(%clientId), %pos) > 10)
		{
			if(GameBase::getPosition(Group::getObject("MissionCleanup/Camp" @ %clientId, 0)) != "0 0 0")
			{
				Client::sendMessage(%clientId, $MsgRed, "You have wandered too far from your camp while setting it up.");
				%step = 5;
			}
			else
				return;
		}
	}

	if(%step == 1)
	{
		%object = newObject("wood", InteriorShape, "wood.dis");
		addToSet("MissionCleanup\\Camp" @ %clientId, %object);

		%x = GetWord(%pos, 0);
		%y = GetWord(%pos, 1);
		%z = GetWord(%pos, 2);
		%npos = (%x + 1) @ " " @ (%y + 0) @ " " @ (%z + 0);
		GameBase::setPosition(%object, %npos);
	}
	else if(%step == 2)
	{
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
	else if(%step == 3)
	{
		%object = newObject("tent", InteriorShape, "tent.dis");
		addToSet("MissionCleanup\\Camp" @ %clientId, %object);

		%x = GetWord(%pos, 0);
		%y = GetWord(%pos, 1);
		%z = GetWord(%pos, 2);
		%npos = (%x + 5) @ " " @ (%y + 0) @ " " @ (%z + 0);
		GameBase::setPosition(%object, %npos);
	}
	else if(%step == 4)
	{
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

		Player::incItemCount(%clientId, Tent);
		RefreshAll(%clientId);

		//so the players in the grouptrigger get kicked out first.
		Group::iterateRecursive(%g, GameBase::setPosition, "0 0 0");

		%gg = nameToId(%g);
		schedule("deleteObject(" @ %gg @ ");", 5);
	}
}