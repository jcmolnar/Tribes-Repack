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
			//Trigger delay isn't needed anymore since centerprint spam doesn't matter
			if(%clientId.sleepMode == "")				centerprint(%clientId, "<jc>This spot feels relaxing enough to sleep.\n <f2>Press T to sleep.", 10);			else				centerprint(%clientId, "<jc>You are asleep.\n<f2>Press T to awaken.", 10);
		}
	}
	else if(String::ICompare(Object::getName(getGroup(getGroup(%object))), "EnterBoxes") == 0)
	{
		%need = %object.need;		%esay = %object.esay;		%nsay = %object.nsay;			storeData(%clientId, "InEnterBox", %object);			%h = HasThisStuff(%clientId, %need);			if(%h != 667 && %h != 666 && %h != False)			{				centerprint(%clientId, "<jc><f1>"@%esay, 8);			}			else			{				for(%i = 0; GetWord(%need, %i) != -1; %i+=2)				{					%w = GetWord(%need, %i);					%w2 = GetWord(%need, %i+1);					if(%w == "LVLM"){						%lvlm = %w2;						break;					}				}				%lvlm++;				%lvlm = %lvlm - (fetchData(%clientId,"RemortStep") * 2);				%nsay = String::replace(%nsay, "<lvlm>", %lvlm);				centerprint(%clientId, "<jc><f0>"@%nsay, 8);			}	}
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
		storeData(%clientId, "InSleepZone", "");		if(%clientId.sleepMode == "")			centerprint(%clientId, "<jc>You have left the sleeping area.", 4);		else			centerprint(%clientId, "<jc>You have left the sleeping area.\n<f2>Press T to awaken.", 10);
	}	if(fetchData(%clientId, "InEnterBox") != ""){		storeData(%clientId, "InEnterBox", "");		centerprint(%clientId, "", 1);	}
	
//	if(String::ICompare(Object::getName(getGroup(getGroup(getGroup(%object)))), "TeleportBoxes") == 0)
//	{
//		//echo("left teleporter box");
//	}
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

function enterEnterBox(%clientId,%object){	%need = %object.need;	%take = %object.take;	%nsay = %object.nsay;	%gsay = %object.gsay;	%h = HasThisStuff(%clientId, %need);	if(%h != 667 && %h != 666 && %h != False)	{
		TakeThisStuff(%clientId, %take);		%landingPos = %object.landingPos;		Item::setVelocity(%clientId, "0 0 0");		GameBase::setPosition(%clientId, %landingPos);		if(!fetchData(%clientId, "invisible"))			GameBase::startFadeIn(%clientId);				RefreshAll(%clientId);				schedule("Client::sendMessage(" @ %clientId @ ", $MsgBeige, \"" @ %gsay @ "\");", 0.22);	}	else		Client::sendMessage(%clientId, $MsgRed, %nsay);}