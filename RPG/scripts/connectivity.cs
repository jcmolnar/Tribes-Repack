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



function Game::initialMissionDrop(%clientId)
{
	dbecho($dbechoMode2, "Game::initialMissionDrop(" @ %clientId @ ")");

	Client::setGuiMode(%clientId, $GuiModePlay);
	%clientId.observerMode = "";

	centerprint(%clientId, "", 0);

	//===================================================
	// Look for invalid characters in the player's name.
	// If none are found, LoadCharacter
	//===================================================

	%name = Client::getName(%clientId);

	%retval = FindInvalidChar(%name);
	if(%retval != "")
	{
		%kickMsg = "You are using invalid characters in your name.  Use a simpler name.  Suggested clan tag characters are dashes and underscores.";
		%clientId.IsInvalid = True;
	}
	else
	{
		%rw = CheckForReservedWords(%name);
		if(%rw != "")
		{
			%kickMsg = "You are using a reserved word in your name (" @ %rw @ ").";
			%clientId.IsInvalid = True;
		}
		else
		{
			//==================================================
			// Check for duplicate names with players currently
			// on server. Also check for duplicate IP's
			//==================================================
			%flag = False;
			%list = GetPlayerIdList();
			%pip = Client::getTransportAddress(%clientId);
			for(%i = 0; (%id = GetWord(%list, %i)) != -1; %i++)
			{
				%n = Client::getName(%id);
				if(String::ICompare(%n, %name) == 0 && %id != %clientId)
				{
					%kickMsg = "This character name is currently in use.";
					%clientId.IsInvalid = True;
					%flag = True;
					break;
				}

				if(!$allowDuplicateIPs)
				{
					%ip = Client::getTransportAddress(%id);
					if(String::ICompare(TrimIP(%ip), TrimIP(%pip)) == 0 && %id != %clientId)
					{
						%kickMsg = "You are not allowed to run two clients on the same server.";
						%clientId.IsInvalid = True;
						%flag = True;
						break;
					}
				}
			}

			if(!%flag)
			{
				LoadCharacter(%clientId);

				if(String::Compare(fetchData(%clientId, "tmpname"), Client::getName(%clientId)) != 0)
				{
					%kickMsg = "This character name already exists. Please choose another.";
					%clientId.IsInvalid = True;
				}

				//==================================================
				// Now that the profile is loaded, we can verify
				// the password.
				//==================================================
	
				if($Client::info[%clientId, 5] == "")
				{
					%kickMsg = "You have not entered a password to protect your character. Select a password in the \"Other info\" field in your profile.";
					%clientId.IsInvalid = True;
				}
				else if(string::getSubStr($Client::info[%clientId, 5], 0, 210) != $Client::info[%clientId, 5])
				{
					%kickMsg = "Your password is too long. Max password length is 210 characters.";
					%clientId.IsInvalid = True;
				}
				else if(String::findSubStr($Client::info[%clientId, 5], "\"") != -1)
				{
					%kickMsg = "Your password contains invalid characters, please change it in \"Other info\" in your profile.";
					%clientId.IsInvalid = True;
				}
				else if(string::compare(fetchData(%clientId, "password"), $Client::info[%clientId, 5]) != 0 && string::compare(fetchData(%clientId, "password"), "") != 0)
				{
					%kickMsg = "This character name has already been selected by someone else on this server, or you are using an incorrect profile password. Change your password in \"Other info\" in your profile.";
					%clientId.IsInvalid = True;
				}
			}
		}
	}

	//==================================================
	// If there was invalid characters in the player's
	// name or the password was incorrect, then stick
	// the player in observer mode so he can be kicked
	// out soon after.
	//==================================================

	if(%clientId.IsInvalid)
	{
		//schedule("Net::kick(" @ %clientId @ ", \"" @ %kickMsg @ "\");", 20);
		centerprint(%clientId, %kickMsg, 0);// @ " You will automatically be kicked within 20 seconds.  If not, please disconnect manually.", 0);

		Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
		%camSpawn = Game::pickObserverSpawn(%clientId);
		Observer::setFlyMode(%clientId, GameBase::getPosition(%camSpawn), GameBase::getRotation(%camSpawn), false, false);
	}
	else
	{
		//==================================================
		// Everything went fine, spawn the player (or make
		// him/her choose stats if creating a new char)
		//==================================================

		if(%clientId.choosingGroup)
			StartStatSelection(%clientId);
		else
			Game::playerSpawn(%clientId, false);
	}
}

function Server::onClientDisconnect(%clientId)
{
	dbecho($dbechoMode2, "Server::onClientDisconnect(" @ %clientId @ ")");
	$players--;
	setWindowTitle($players@"/"@$server::maxplayers@" Tribes RPG server");

	Client::setControlObject(%clientId, -1);

	if(!%clientId.IsInvalid && fetchData(%clientId, "HasLoadedAndSpawned"))
	{
		//Arena stuff
		if(IsInRoster(%clientId))
		{
			RestorePreviousEquipment(%clientId);
			RemoveFromRoster(%clientId);
		}
		if(IsInArenaDueler(%clientId))
		{
			RestorePreviousEquipment(%clientId);
			RemoveFromArenaDueler(%clientId);
		}

		//Pet stuff
		%list = fetchData(%clientId, "PersonalPetList");
		for(%p = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999))
		{
			%w = String::NEWgetSubStr(%list, 0, %p);
			FellOffMap(%w);
		}

		//Camp stuff
		%camp = nameToId("MissionCleanup\\Camp" @ %clientId);
		if(%camp != -1)
			DoCampSetup(%clientId, 5);

		SaveCharacter(%clientId);

		ClearEvents(%clientId);
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))		{			if(%cl.repack >= 24){				Client::sendMessage(%cl, $MsgWhite, rpg::getname(%clientId)@" has left the world.");			}		}
	}

	for(%i = 0; %i < 10; %i++)
		$Client::info[%clientId, %i] = "";

	echo("GAME: clientdrop " @ %clientId);

	%set = nameToID("MissionCleanup/ObjectivesSet");
	for(%i = 0; (%obj = Group::getObject(%set, %i)) != -1; %i++)
	GameBase::virtual(%obj, "clientDropped", %clientId);
}

function Server::onClientConnect(%clientId)
{
	dbecho($dbechoMode2, "Server::onClientConnect(" @ %clientId @ ")");
	$players++;
	setWindowTitle($players@"/"@$server::maxplayers@" Tribes RPG server");

	%hisip = Client::getTransportAddress(%clientId);

	%bannedFlag = False;
	for(%i = 1; $bannedip[%i] != ""; %i++)
	{
		if(String::findSubStr(%hisip,$bannedip[%i]) != -1){
			newKick(%clientId,%msg);
			echo(%hisip @ " (banned)");
			%clientId.IsInvalid = True;
			BanList::add(%hisip, 9999);
			%bannedFlag = true;
		}
	}
	if(!%bannedFlag){		//Connection spam protection, 5 second delay on each connection, per IP
		BanList::add(%hisip, 5);
	}


	if(!String::NCompare(Client::getTransportAddress(%clientId), "LOOPBACK", 8))
	{
		// force admin the loopback dude
		%clientId.adminLevel = 5;
	}
	echo("CONNECT: " @ %clientId @ " \"" @ escapeString(Client::getName(%clientId)) @ "\" " @ Client::getTransportAddress(%clientId));

	%clientId.noghost = true;
	%clientId.messageFilter = -1; // all messages

	remoteEval(%clientId, SVInfo, version(), $Server::Hostname, $modList, $Server::Info, $ItemFavoritesKey);
	remoteEval(%clientId, MODInfo, $MODInfo);
	remoteEval(%clientId, FileURL, $Server::FileURL);

	//This checks if connecting client has the repack.
	//Search rpgfunk.cs for function remoteRepackConfirm
	//for the other half of this check.
	remoteeval(%clientid, RepackIdent, true);

//-------------------------------------------------------------

	ClearVariables(%clientId);			//this needs to be done so the profile is as clean as possible...
	Game::refreshClientScore(%clientId);	//so the player appears in the score list right away
}

function Game::onPlayerConnected(%playerId)
{
}

function Client::leaveGame(%clientId)
{
}

function newKick(%client, %msg, %force){	if(!isObject(%client))		return false;	if($newKicked[%client])		return false;	if(%client.blockAllKicks && !%force)		return false;	$newKicked[%client] = True;
	if(%msg == "")
		%msg = " ";//So clients won't assume the "server went down".	%msg = escapestring(%msg);	pecho("Kick "@rpg::getname(%client)@": "@%msg);	schedule("net::kick("@%client@",\""@%msg@"\");$newKicked["@%client@"]=\"\";",0.05);
	//Using a schedule prevents us from crashing because then it doesn't
	//happen while doing other operations on the client.
	return true;}

// Part of phantom's hack block thingies
function delayedban(%id)
{
	%hisip = Client::getTransportAddress(%id);
	newKick(%id, "2 hour ban for bad behaviour.");
	BanList::add(%hisip, 7200);
}
function exploitBan(%id, %type, %period){	%hisip = Client::getTransportAddress(%id);	%banmsg = "You have been temporarily auto-banned for "@%period@" minutes. Reason: "@%type;
	%ret = newKick(%id,%banmsg, True);
	if(%ret){
		%period = %period * 60;
		BanList::add(%hisip, %period);
		%msg = %type@" detected from: " @ rpg::getname(%id) @ ", kicking.";
		messageall(0,%msg);
		pecho(%msg);
	}
	return %ret;}
//For use from the server console. Coded functions should use newKick.//You can use a name or an ID on this.//Message is optional.//Examples:////kick("phantom", "testing and stuff");//kick("phantom");//kick(2049);//function kick(%target, %msg){	if(%target == "")	{		pecho("kick(\"Name/id\", \"kick message\");");		pecho("ex: kick(2049,\"Oh behave!\");");		pecho("Kicked players can return immediately.");		return false;	}	%client = %target;	if(client::getname(%target) == "")	{//looks like we were given a name		%client = NEWgetClientByName(%target);		if(%client == -1){			pecho("Couldn't kick "@%target@", name is invalid.");			return false;		}	}	if(%client < 2049){		pecho("Couldn't kick "@%client@", ID is invalid.");		return false;	}	if(!isObject(%client)){		pecho("Couldn't kick "@%client@", ID is invalid.");		return false;	}	newKick(%client, %msg, True);}function ban(%target, %period, %msg){	if(%target == "")	{		pecho("ban(\"Name/id\", minutes, \"kick message\");");		pecho("ex: ban(2049,10,\"Time for a time out!\");");		pecho("Banned players can't return until their ban runs out.");		return false;	}	%client = %target;	if(client::getname(%target) == "")	{//looks like we were given a name		%client = NEWgetClientByName(%target);		if(%client == -1){			pecho("Couldn't ban "@%target@", name is invalid.");			return false;		}	}	if(%client < 2049){		pecho("Couldn't ban "@%client@", ID is invalid.");		return false;	}	if(!isObject(%client)){		pecho("Couldn't ban "@%client@", ID is invalid.");		return false;	}	%hisip = Client::getTransportAddress(%client);	%period = %period * 60;	BanList::add(%hisip, %period);	newKick(%client, %msg);}