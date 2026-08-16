//This file is part of Tribes RPG.
//Tribes RPG server side scripts
//Written by Jason "phantom" Daley,  Matthiew "JeremyIrons" Bouchard, and more (yet undetermined)

//	Copyright (C) 2019  Jason Daley

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

// putting a global variable in the argument list means:
// if an argument is passed for that parameter it gets
// assigned to the global scope, not the scope of the function


function processMenurecall(%clientId, %option)
{
	if(%option == "addrecall"){
		%zoneId = fetchData(%clientId, "zone");
		if(zone::getType(%zoneId) != "PROTECTED"){
			Client::sendMessage(%clientId, $MsgRed, "You must be in a protected zone. You can only add a zone you are standing in.");
			return;
		}
		else if(fetchData(%clientId, "RankPoints") < 5)
		{
			Client::sendMessage(%clientId, $MsgRed, "For 1 Rank Point you could add a town to your recall list. Need at least 5.");
			return;
		}
		else {
			%zone = $Zone::Desc[%zoneId];
			Client::sendMessage(%clientId, $MsgBeige, "For 1 Rank Point you can add a town to your recall list.");
			Client::buildMenu(%clientId, "Add this town?", "perkaddrecall", true);
			Client::addMenuItem(%clientId, %curitem++ @ "Add "@%zone, %zone);
			Client::addMenuItem(%clientId, %curitem++ @ "Cancel", "c");
			return;
		}
		return;
	}
	if(fetchData(%clientId, "tmprecall"))
		return;

	%list = fetchData(%clientId, "recallList");
	%isInList = IsInCommaList(%list, %option);
	if(!%isInList && %option != "Safe Location")
		return;
	if($recallTo[%option] != "")
		%clientId.recallingTo = $recallTo[%option];
	else
	{
		%zoneId = zone::descToId(%option);
		%clientId.recallingTo = $Zone::droppoint[%zoneId, 1];
	}

	%clientId.recallingToName = %option;
	if(%clientId.perks & 1){
		FellOffMap(%clientId);
		return;
	}
	%pos = GameBase::getPosition(%clientId);
	if(GetWord(%pos, 0) < -8000)
		%offworld = True;
	if(GetWord(%pos, 1) < -3072)
		%offworld = True;
	if(%offworld)
		FellOffMap(%clientId);
	%zvel = floor(getWord(Item::getVelocity(%clientId), 2));
	if(%zvel <= -250 || %zvel >= 350)
	{
		FellOffMap(%clientId);
		%zv = "Falling; recalling instantly.";
		Client::sendMessage(%clientId, $MsgBeige, %zv);
		return;
	}
//	if(client::isHorse(%clientId)){
//		Client::sendMessage(%clientId, $MsgBeige, "Can't recall while on a horse.");
//		return;
//	}
	%zv = "Not falling; recalling normally.";
	Client::sendMessage(%clientId, $MsgBeige, %zv);
	%seconds = $recallDelay;
	if(Zone::getType(fetchData(%clientId, "zone")) == "PROTECTED")
		%seconds = %seconds/10;
	storeData(%TrueClientId, "tmprecall", True);
	Client::sendMessage(%clientId, $MsgBeige, "Stay at your current position for the next " @ %seconds @ " seconds to recall.");
	recallTimer(%clientId, %seconds+1, GameBase::getPosition(%clientId));
}

function processMenutrack(%clientId, %option)
{
	%clientId.tabMenuSpam++;
	if(%clientId.tabMenuSpam > 30){
		exploitBan(%clientId, "Menu spam", 1);
		return;
	}
	%w1 = getWord(%option,0);
	%cropped = String::NEWgetSubStr(%option, (String::len(%w1)+1), 99999);
	if(%w1 == "refresh"){
		processMenucompass(%clientId, %cropped);
		return;
	}
	%targetId = %option;
	%clientPos = gamebase::getposition(%clientId);
	%zonePos = gamebase::getposition(%targetId);
	arrowTowards(%clientPos, %zonePos, rpg::getname(%option));
}

function processMenutrackpack(%clientId, %option)
{
	%clientId.tabMenuSpam++;
	if(%clientId.tabMenuSpam > 30){
		exploitBan(%clientId, "Menu spam", 1);
		return;
	}
	%w1 = getWord(%option,0);
	%cropped = String::NEWgetSubStr(%option, (String::len(%w1)+1), 99999);
	if(%w1 == "refresh"){
		processMenucompass(%clientId, %cropped);
		return;

	}
	%targetId = %option;
	%clientPos = gamebase::getposition(%clientId);
	%zonePos = gamebase::getposition(%targetId);
	arrowTowards(%clientPos, %zonePos, getWord($loot[%option],0)@"'s pack");
}
function processMenuadvcompass(%clientId, %option)
{
	%clientId.tabMenuSpam++;
	if(%clientId.tabMenuSpam > 30){
		exploitBan(%clientId, "Menu spam", 1);
		return;
	}
	%zoneId = %option;
	if(%zoneId == ""){
		pecho("processMenuadvcompass"@" no zone: "@%clientId@" "@%option);
		return;
	}
	%clientPos = gamebase::getposition(%clientId);
	%zonePos = findZoneFrom(%zoneId, %clientPos);
	%zone = zone::getdesc(%zoneId);
	arrowTowards(%clientPos, %zonePos, %zone);
}
function processMenucompass(%clientId, %option)
{
	%opt = getWord(%option,0);
	if(%opt == "recall")
	{
		if(isJailed(%clientId))
			return;
		Client::buildMenu(%clientId, "Recall to:", "recall", true);
		%curItem=-1;
		Client::addMenuItem(%clientId, string::getsubstr($menuChars,%curItem++,1) @ "Safe Location", "Safe Location");
		//if(!hasthisstuff(%clientId, "ACHIEV 0:256"))return;

		%list = fetchData(%clientId, "recallList");

		%cnt = 0;
		for(%i = String::findSubStr(%list, ","); (%i = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %i+1, 99999))
		{
			%town = String::NEWgetSubStr(%list, 0, %i);
			Client::addMenuItem(%clientId, string::getsubstr($menuChars,%curItem++,1) @ %town, %town);
		}
		Client::addMenuItem(%clientId, string::getsubstr($menuChars,%curItem++,1) @ "Add current zone to list (1 RP)", "addrecall");
		return;
	}
	if(!SkillCanUse(%clientId, "#compass")){
		Client::sendMessage(%clientId, $MsgBeige, "You do not have enough Sense Heading skill.");
		return;
	}
	%pos = GameBase::getPosition(%clientId);
	%skill = $playerSkill[%clientId,$SkillSenseHeading];
	if(%opt == "trackpack")
	{
		%type = getWord(%option,1);
		if(%type != "own" && !SkillCanUse(%clientId, "#trackpack")){
			Client::sendMessage(%clientId, $MsgBeige, "You do not have enough Sense Heading skill.");
			return;
		}
		%range = %skill * 15;
		if(%range > 2000)
			%range = 2000;
		Client::buildMenu(%clientId, %skill@" skill = "@%range@" range", "trackpack", true);
		Client::addMenuItem(%clientId, "rRefresh", "refresh "@%option);
		if(%type == "own"){
			%list = fetchData(%clientId, "lootbaglist");
			%count = -1;
			for(%i = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999))
			{
				%id = floor(%list);
				%idpos = gamebase::getposition(%id);
				if(%idpos != "0 0 0"){
					%dist = round(Vector::getDistance(%pos, %idpos));
					if(%dist < %range){
						$thinglist[%count++] = "Pack ("@getNESWa(%pos, %idpos)@" "@%dist@" m)";
						$thingdist[%count] = %dist;
						$thingID[%count] = %id;
						//if(%count > 30) break;
					}
				}
			}
		}
		else {
			%ii = 0;
			%count = -1;
			for(%i = 1; %i <= $swpacknum; %i++)
			{
				%id = $sw::packid[%i];
				%idpos = gamebase::getposition(%id);
				if(%idpos != "0 0 0" && $loot[%id] != ""){
					%dist = round(Vector::getDistance(%pos, %idpos));
					if(%dist < %range){
						$thinglist[%count++] = $sw::packowner[%i]@"'s Pack ("@getNESWa(%pos, %idpos)@" "@%dist@" m)";
						$thingdist[%count] = %dist;
						$thingID[%count] = %id;
						//if(%count > 30) break;
					}
				}
			}
		}
		sendThingList(%clientId);
		return;
	}
	if(%opt == "track")
	{
		%type = getWord(%option,1);
		if(!SkillCanUse(%clientId, "#track")){
			Client::sendMessage(%clientId, $MsgBeige, "You do not have enough Sense Heading skill.");
			return;
		}
		%range = %skill * 7.5;
		if(%range > 2000)
			%range = 2000;
		Client::buildMenu(%clientId, %skill@" skill = "@%range@" range", "track", true);
		Client::addMenuItem(%clientId, "rRefresh", "refresh "@%option);
		if(%type == "player")
			%list = GetPlayerIdList();
		else
			%list = GetBotIdList();
		%count = -1;
		for(%i = 0; (%id = GetWord(%list, %i)) != -1; %i++)
		{
			%idpos = fetchData(%id, "lastScent");
			if(%id == %clientId)
				%idpos = "";
			if(%idpos != ""){
				%dist = round(Vector::getDistance(%pos, %idpos));
				if(%dist < %range){
					%name = rpg::getname(%id);
					$thinglist[%count++] = %name@" ("@getNESWa(%pos, %idpos)@" "@%dist@"m)";
					$thingdist[%count] = %dist;
					$thingID[%count] = %id;
					//if(%count > 30) break;
				}
			}
		}
		sendThingList(%clientId);
		return;
	}
	if(%option == "advcompass")
	{
		if(!SkillCanUse(%clientId, "#advcompass")){
			Client::sendMessage(%clientId, $MsgBeige, "You do not have enough Sense Heading skill.");
			return;
		}
		%range = %skill * 15;
		if(%range > 2000)
			%range = 2000;
		Client::buildMenu(%clientId, %skill@" skill = "@%range@" range", "advcompass", true);
		%list = getNearestZones(%pos, %range);
		%count = -1;
		for(%i = 0; (%zone = GetWord(%list, %i)) != -1; %i+=3)
		{
			%a = GetWord(%list, %i+1);
			%dist = GetWord(%list, %i+2);
			%a = %a @ " " @ %dist @ "m";
			$thinglist[%count++] = Zone::GetDesc($zone::folderid[%zone])@" ("@%a@")";
			$thingdist[%count] = %dist;
			$thingID[%count] = %zone;
					//if(%count > 30) break;
			//Client::addMenuItem(%clientId, %curItem++ @ %desc, %zone);
		}
		sendThingList(%clientId);
		return;
	}
	%zone = getZoneAt(%pos);
	%zonetype = zone::getType(%zone);
	if(%zonetype == "PROTECTED")
		%zonetype = "town";
	if(String::findSubStr(%zonetype, %option) != -1)
	{
		Client::sendMessage(%clientId, 1, "You're um.. You're already in a "@%option);
		return;
	}
	%mpos = GetNearestZone(%clientId, %option, 4);
	if(%mpos != False)
	{
		%d = GetNESW(%pos, %mpos);
		arrowTowards(%pos, %mpos, %option);
		//UseSkill(%clientId, $SkillSenseHeading, True, True);
		Client::sendMessage(%clientId, 0, "The nearest " @ %option @ " is " @ %d @ " of your location.");
	}
	else
		Client::sendMessage(%clientId, 1, "Zone not found or an error occurred!");
		
}

function processMenuperkaddrecall(%clientId,%option)
{
	pecho("perkaddrecall: " @ rpg::getName(%clientId)@" - "@%option);
	if(%option == "c")
		return;
	if(string::len(fetchData(%clientId, "recallList")) > 230){
		Client::sendMessage(%clientId, $MsgRed, "Max recall list reached.");
		return;
	}
	%zoneId = zone::descToId(%option);
	if(%zoneId == -1){
		Client::sendMessage(%clientId, $MsgRed, "Something went wrong. Zone doesn't seem to exist.");
		pecho(rpg::getName(%clientId)@" tried to add recall zone "@%option);
		return;
	}
	if(zone::getType(%zoneId) != "PROTECTED"){
		Client::sendMessage(%clientId, $MsgRed, "You must be in a protected zone. You can only add a zone you are standing in.");
		return;
	}
	else if(fetchData(%clientId, "RankPoints") < 5)
	{
		Client::sendMessage(%clientId, $MsgRed, "For 1 Rank Point you could add a town to your recall list. You must have at least 5 total.");
		return;
	}
	if($Zone::droppoint[%zoneId, 1] == "")
	{
		Client::sendMessage(%clientId, $MsgRed, "Something went wrong. Zone doesn't seem to have a droppoint.");
		pecho("No dropppoint for "@%option);
		return;
	}
	%list = fetchData(%clientId, "recallList");
	%isInList = IsInCommaList(%list, %option);
	if(%isInList){
		Client::sendMessage(%clientId, $MsgRed, %option@" is already in your recall list!");
		return;
	}
	storeData(%clientId, "RankPoints", 1, "dec");
	Client::sendMessage(%clientId, $MsgBeige, %option@" added to your recall list.");
	storeData(%clientId, "recallList", AddToCommaList(%list, %option));
	
}

function sendThingList(%clientId)
{
	//I know, bubble sort bad, but it is fine here.
	//Don't need anything more elaborate for such a low frequency list.
	%swapped = True;
	while(%swapped == True){
		%swapped = False;
		for(%i = 0;$thingdist[%i+1] != "";%i++)
		{
			if($thingdist[%i] > $thingdist[%i+1]){
				%temp = $thingdist[%i+1];
				$thingdist[%i+1] = $thingdist[%i];
				$thingdist[%i] = %temp;
				%temp = $thinglist[%i+1];
				$thinglist[%i+1] = $thinglist[%i];
				$thinglist[%i] = %temp;
				%temp = $thingID[%i+1];
				$thingID[%i+1] = $thingID[%i];
				$thingID[%i] = %temp;
				%swapped = True;
			}
		}
	}
	%curItem=-1;
	for(%i = 0;$thinglist[%i] != "";%i++)
	{
		if(%curItem < 32)
			Client::addMenuItem(%clientId, string::getsubstr($menuChars,%curItem++,1) @ $thinglist[%i], $thingID[%i]);
		$thinglist[%i] = "";
		$thingdist[%i] = "";
		$thingID[%i] = "";
	}
}