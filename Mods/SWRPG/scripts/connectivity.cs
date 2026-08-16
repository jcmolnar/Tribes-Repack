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
				if(fetchData(%clientId, "password") != $Client::info[%clientId, 5] && fetchData(%clientId, "password") != "")
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
		schedule("Net::kick(" @ %clientId @ ", \"" @ %kickMsg @ "\");", 20);
		centerprint(%clientId, %kickMsg @ " You will automatically be kicked within 20 seconds.  If not, please disconnect manually.", 0);

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

		if(%clientId.choosingRace)
			StartStatSelection(%clientId);
		else
			Game::playerSpawn(%clientId, false);
		//%clientId.LoginTime = getSimTime();
	}
}

function Server::onClientDisconnect(%clientId)
{
	dbecho($dbechoMode2, "Server::onClientDisconnect(" @ %clientId @ ")");

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
		ClearVariables(%clientId); //Is this needed?
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

	if(!String::NCompare(Client::getTransportAddress(%clientId), "LOOPBACK", 8))
	{
		// force admin the loopback dude
		%clientId.adminLevel = 5;
	}
	echo("CONNECT: " @ %clientId @ " \"" @ escapeString((%name = Client::getName(%clientId))) @ "\" " @ Client::getTransportAddress(%clientId));

	%clientId.noghost = true;
	%clientId.messageFilter = -1; // all messages

	remoteEval(%clientId, SVInfo, version(), $Server::Hostname, $modList, $Server::Info, $ItemFavoritesKey);
	remoteEval(%clientId, MODInfo, $MODInfo);
	remoteEval(%clientId, FileURL, $Server::FileURL);

//-------------------------------------------------------------

	ClearVariables(%clientId);	//this needs to be done so the profile is as clean as possible...
	Game::refreshClientScore(%clientId);	//so the player appears in the score list right away

	if($LogConnections)
	{
		//$cn[$Server::NumConnections++] = escapeString(Client::getName(%clientId)) @ " " @ Client::getTransportAddress(%clientId) @ ", at " @ getsimtime();
		//export("cn*", "temp\\logs_connections.log", true);
		echo("Number of connections: " @ $NumConnections++ @ ". as of " @ getsimtime());
		$CNL[%CNLs++] = %name;
		//deleteVariables("cn*");
	}
}

function Game::onPlayerConnected(%playerId)
{
}

function Client::leaveGame(%clientId)
{
}

