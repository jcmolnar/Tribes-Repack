function Server::onClientDisconnect(%clientId) {

	$ClientsConnected--;
	setWindowTitle(MainWindow, "Red Moon Server: "@$Server::HostName@" ["@$ClientsConnected@" / "@$Server::MaxPlayers@"]");

	Client::setControlObject(%clientId, -1);
	$ClientData[client::getname(%clientId), OwnsLoot] = 0;

	if(!%clientId.IsInvalid && $HasLoadedAndSpawned[%clientId]) {

		%docamp = True;

		remoteEval(%clientId, "ClearHeadersData");

		//Camp stuff
		%camp = nameToId("MissionCleanup\\Camp" @ %clientId);
		if(%camp != -1)
			DoCampSetup(%clientId, 5);

		if($ClientData[%clientId, "isMimic"])
			 ChangeRace(%clientId, "Human");

		SaveCharacter(%clientId, %docamp);
	}
	else
	{
		%ip = Client::getTransportAddress(%clientid);
		BanList::add(%ip, 20);
	}


	for(%i = 0; %i < 10; %i++)
		$Client::info[%clientId, %i] = "";

	echo("GAME: clientdrop " @ %clientId);

	%set = nameToID("MissionCleanup/ObjectivesSet");
	for(%i = 0; (%obj = Group::getObject(%set, %i)) != -1; %i++)
      GameBase::virtual(%obj, "clientDropped", %clientId);
}

function Server::onClientConnect(%clientId) {

	$ClientsConnected++;
	setWindowTitle(MainWindow, "Red Moon Server: "@$Server::HostName@" ["@$ClientsConnected@" / "@$Server::MaxPlayers@"]");

	remoteEval(%clientId, "ClearHeadersData");
	if(!String::NCompare(Client::getTransportAddress(%clientId), "LOOPBACK", 8))
	{
		// force admin the loopback dude
		%clientId.adminLevel = 5;
	}


	echo("CONNECT: " @ %clientId @ " \"" @ escapeString(Client::getName(%clientId)) @ "\" " @ Client::getTransportAddress(%clientId));

	%clientId.noghost = true;
	%clientId.messageFilter = -1; // all messages

	$tmpClient = %clientId;

	remoteEval(%clientId, SVInfo, version(), $Server::Hostname, $modList, $Server::Info, $ItemFavoritesKey);
	remoteEval(%clientId, MODInfo, $MODInfo);
	remoteEval(%clientId, FileURL, $Server::FileURL);


//-------------------------------------------------------------

	GiveClientHeadersData(%clientId);
	ClearVariables(%clientId);			//this needs to be done so the profile is as clean as possible...
	//Game::refreshClientScore(%clientId);	//so the player appears in the score list right away
	Client::setScore(%clientId, "%n is joining...", 0);
}

function onConnectionError(%clientId, %manager, %errorString)
{
	dbecho($dbechoMode, "onConnectionError(" @ %clientId @ ", " @ %manager @ ", " @ %errorString @ ")");

	//This is client-side stuff

	if(%manager == 2048)
	{
	}
	else
	{
		Quickstart();
		GuiPushDialog(MainWindow, "gui\\MessageDialog.gui");
		$errorString = "Connection to server error:\n" @ %errorString;
		schedule("Control::setValue(MessageDialogTextFormat, $errorString);", 0);
	}
}

function onConnection(%message)
{
	dbecho($dbechoMode, "onConnection(" @ %message @ ")");

	//This is client-side stuff

	echo("Connection ", %message);
	$dataFinished = false;
	if(%message == "Accepted")
	{

		if(!$RM::ClientHosting)
			resetSimTime();

		//execute the custom script
		if($PCFG::Script != "")
		{
			exec($PCFG::Script);
		}

		resetPlayDelegate();
		remoteEval(2048, "SetCLInfo", $PCFG::SkinBase, $PCFG::RealName, $PCFG::EMail, $PCFG::Tribe, $PCFG::URL, $PCFG::Info, $pref::autoWaypoint, $pref::noEnterInvStation, $pref::messageMask);

		if($Pref::PlayGameMode == "JOIN")
		{
			cursorOn(MainWindow);
			GuiLoadContentCtrl(MainWindow, "gui\\Loading.gui");
			renderCanvas(MainWindow);
		}

	}
	else if(%message == "Rejected")
	{
		Quickstart();
		$errorString = "Connection to server rejected:\n" @ $errorString;
		GuiPushDialog(MainWindow, "gui\\MessageDialog.gui");
		schedule("Control::setValue(MessageDialogTextFormat, $errorString);", 0);
	}
	else
	{
		//startMainMenuScreen();
		Quickstart();

		if(%message == "Dropped")
		{
			if($errorString == "")
				$errorString = "Connection to server lost:\nServer went down.";
			else
				$errorString = "Connection to server lost:\n" @ $errorString;

			GuiPushDialog(MainWindow, "gui\\MessageDialog.gui");
			schedule("Control::setValue(MessageDialogTextFormat, $errorString);", 0);
		}
		else if(%message == "TimedOut")
		{
			$errorString = "Connection to server timed out.";
			GuiPushDialog(MainWindow, "gui\\MessageDialog.gui");
			schedule("Control::setValue(MessageDialogTextFormat, $errorString);", 0);
		}
	}
}

function Game::onPlayerConnected(%playerId)
{
}

function Client::leaveGame(%clientId)
{
}

