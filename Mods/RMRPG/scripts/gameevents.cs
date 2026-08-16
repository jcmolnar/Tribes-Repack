$hidelvls = true;
function Mission::init() {

	//if($displayPingAndPL)
	//	setClientScoreHeading("Name\t\x50Zone\t\xBFLVL\t\xDFPing\t\xFFPL");
	//else
	//	setClientScoreHeading("Name\t\x50Zone\t\xB2LVL\t\xD2Class\t\xFFPvP");

	//if($hidelvls)
	//	setClientScoreHeading("Warrior Name\t\x72Zone\t\xD0Class");
			setClientScoreHeading("Warrior Name"); //  \t\x79Zone");  //  \t\xD0Class");
	//else
	//	setClientScoreHeading("Warrior Name\t\x72Zone\t\xD0Level\t\xF6Class");

	if(!$NoSpawn)
		AI::setupAI();


	echo(".--==< RecursiveWorld STARTED >==--.");
      RecursiveWorld(5);
      RecursiveZone(2);
      RefreshStatus(5);
	exec("[RM]ServerStats.cs");
	if($OtherPrefs::BurnedClientId == "")
		$OtherPrefs::BurnedClientId = 1000;
	//exec("[RM]FireFloodLog.cs");
}

function Game::startMatch() {
	dbecho($dbechoMode, "Game::startMatch()");

	$matchStarted = true;
	$missionStartTime = getSimTime();
}

function Game::initialMissionDrop(%clientId) {

	Client::setGuiMode(%clientId, $GuiModePlay);
	%clientId.observerMode = "";

	//===================================================
	// Look for invalid characters in the player's name.
	// If none are found, LoadCharacter
	//===================================================

	%password = 0;
	%name = Client::getName(%clientId);
	%retval = FindInvalidChar(%name);
	%clientId.IsInvalid = False;

	for(%id = Client::getFirst(); %id != -1; %id = Client::getNext(%id)) {
		if(%clientId != %id && String::ICompare(%name, Client::getName(%id)) == 0) {
			%retval = "kick";
			break;
		}
	}

	if(%retval != "") {
		Observer::setFlyMode(%clientId, GameBase::getPosition(%observerMarker), GameBase::getRotation(%observerMarker), false, true);
		%kickMsg = "You are using invalid characters in your name.  Use a simpler name.  Suggested clan tag characters are dashes and underscores.";
		%clientId.IsInvalid = True;
	}
	%rw = CheckForReservedWords(%name);
	if(%rw != "") {
		Observer::setFlyMode(%clientId, GameBase::getPosition(%observerMarker), GameBase::getRotation(%observerMarker), false, true);
		%kickMsg = "You are using a reserved word in your name ("@%rw@").";
		%clientId.IsInvalid = True;
	}
	else {
		LoadCharacter(%clientId);
		
		//==================================================
		// Now that the profile is loaded, we can verify
		// the password.
		//==================================================

		%pkc = pk::bancheck(%clientid);	// Lets see if this character has pked to many people.

		if($password[%clientId] != $Client::info[%clientId, 5] && $password[%clientId] != "") {
			%clientId.IsInvalid = True;
			%password = 1;
		}
	}

	//==================================================
	// If there was invalid characters in the player's
	// name or the password was incorrect, then stick
	// the player in observer mode so he can be kicked
	// out soon after.
	//==================================================

	if(%clientId.IsInvalid) {
		if(%password == 1) {

			centerprint(%clientId, "This is a password protected character... You will have to enter a password by typing \"#logon mypassword\" or you will automatically be kicked within 20 seconds.  If this is not your character, please disconnect manually.", 0);
			schedule("check_password(" @ %clientId @ ",15);",5);
		}
		else {

			schedule("Net::kick(" @ %clientId @ ", \"" @ %kickMsg @ "\");", 20);
			centerprint(%clientId, %kickMsg @ "  You will automatically be kicked within 20 seconds.  If not, please disconnect manually.", 0);
		}
		RMSetObserver(%clientId);
	}
	else if(%pkc == "True")
	{
		schedule("Net::kick(" @ %clientId @ ", \"" @ %kickMsg @ "\");", 20);
		centerprint(%clientId, %kickMsg @ "  This character is banned for excessive PKing.", 0);
		RMSetObserver(%clientId);
	}
	else {
		//==================================================
		// Everything went fine, spawn the player (or make
		// him/her choose stats if creating a new char)
		//==================================================

		if(%clientId.choosingGroup) {
			StartStatSelection(%clientId);
			CheckClientFiles(%ClientId);
		}
		else {

			$ClientWaiting[%clientId] = "True 1";
			Schedule("MenuJustJoined("@%clientId@");", 2);
			Schedule("ScheduleKick("@%clientId@");", 60+30); //1min 30 sec warning... and then 60 more to kick...
		}
	}
}

function MenuJustJoined(%clientId, %t) {

	if($ClientWaiting[%clientId] == "")	//hehe oops
		return;

	if(%t == "")
		%msg = "Welcome!";
	else
		%msg = "Welcome! Join in "@%t@" seconds.";
	Client::buildMenu(%clientId, %msg, "justjoined", false);

	Client::addMenuItem(%clientId, "1Join this world.", "join");
	//Client::addMenuItem(%clientId, "--:-:-:-:-:-:- :-", "");
	//Client::addMenuItem(%clientId, "9Quit.", "quit");
}

function ScheduleKick(%clientId) {

	if(GetWord($ClientWaiting[%clientId], 0) != True)
		return;

	%w = GetWord($ClientWaiting[%clientId], 1);
	%cnt = 0;
	if(%w == 1) {
		for(%i = 60; %i > 0; %i--) {
			%cnt++;
			Schedule("MenuJustJoined("@%clientId@", "@%i@");", %cnt);
		}
		Schedule("ScheduleKick("@%clientId@");", 60);
		$ClientWaiting[%clientId] = "True 2";
	}
	else if(%w == 2) {
		Net::kick(%clientId, "You have been kicked because you waited to long on the login screen.\n\nYou have NOT been banned.");
		$ClientWaiting[%clientId] = "";
	 }
}

function processMenujustjoined(%clientId, %opt) {

	if(%opt == "join") {
		$ClientWaiting[%clientId] = "";
		ClientJustJoinedTeam(%ClientId);
	}
	else if(%opt == "quit") {
		$ClientWaiting[%clientId] = "";
		remoteEval(%ClientId, dropclient);
	}
	else
		MenuJustJoined(%ClientId);
}

function ClientJustJoinedTeam(%ClientId) {

	$ClientWaiting[%clientId] = "";
	Game::playerSpawn(%ClientId, false);
	RefreshAll(%ClientId);
	Schedule("remoteEval("@%ClientId@",\"ATKText\", \"<jc>Welcome to Red Moon RPG.\", wait);", 2);
	Schedule("CheckIsBlessed("@%clientId@");", 3);

	CheckClientFiles(%ClientId);
}

function DoServerStatus(%clientId) {
	exec("[RM]ServerStats.cs");
	$BurnedClientTagId[%clientId] = $OtherPrefs::BurnedClientId;
	echo(">> New $BurnedClientTagId["@%clientId@"] = "@$OtherPrefs::BurnedClientId);
	$OtherPrefs::BurnedClientId++;
	export("$OtherPrefs::*", "Temp\\[RM]ServerStats.cs");

}

function CheckIsBlessed(%clientId) {

	if($isBlessed[%ClientId] == true)
		Client::sendmessage(%ClientId, 0, "Your Soul was blessed by the Gods of the Life Stream!");
	else {
		Client::sendmessage(%ClientId, 0, "Your Soul isn't blessed.");
		$isBlessed[%ClientId] = false;
	}
}
function RMSetObserver(%Client) {
	%group = nameToId("MissionGroup\\ObserverDropPoints");
	%observerMarker = Group::getObject(%group, 0);

	Client::setControlObject(%Client, Client::getObserverCamera(%Client));
	Observer::setFlyMode(%Client, GameBase::getPosition(%observerMarker), GameBase::getRotation(%observerMarker), false, true);
}

function Player::enterMissionArea(%player) { }	// Maybe we could use these somehow...
function Player::leaveMissionArea(%player) { }

function check_password(%clientId,%times_to_go)
{
	if($password[%clientId]!=$mypassword[%clientId])
	{
		if(%times_to_go>0)
		{
			%times_to_go--;
			schedule("check_password(" @ %clientId @ "," @ %times_to_go @ ");",1);
		}
		else
		{
			%kickMsg="You did not logon with the correct password. Change your password in \"Other info\" in your profile, or type \"#logon mypassword\".";
			Net::kick(%clientId,%kickMsg);
		}
	}
	else
	{
		%clientId.IsInvalid = false;
		centerprint(%clientId,"Password accepted.", 0);
		if(%clientId.choosingStats)
                  StartStatSelection(%clientId);
		else
			Game::playerSpawn(%clientId, false);
	}
}