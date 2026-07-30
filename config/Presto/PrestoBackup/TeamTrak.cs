// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// TeamTrak.CS								Presto, March '99 
//
//	Team member and flag status tracking.
//
//	I started with the code in KillerBunny's dyn_HUD script, fixed a few
//	bugs and added some new features.  The TeamTrak module should now help
//	people who want to write HUDs, but the idea is that eventually it
//	will help anyone get team information which the client tries to hide
//	from you (like team names.)
//
//	This is a work in progress.  I'm going to be working on supporting
//	the other kinds of games besides CTF, and maybe keeping track of
//	other team data like scores, etc.  Right now it only tracks flags,
//	team names, and the clients on each team.
//
//	Usage examples:
//		%team = Team::Friendly();
//			Returns the team you're on.
//		%team = Team::Enemy();
//			Returns the team you're not on :)
//
//		%name = Team::GetName(Team::Friendly());
//			Gets the name of the friendly team.
//		%team = Team::GetByName("Diamond Sword");
//			Returns the team number of the team named "Diamond
//			Sword".  >>Note there are some GOTCHAs here!<<
//			First, the client tries to hide team names.  That
//			means that I have to watch messages to find the
//			team names.  If no message ever mentions a team,
//			I will not know about it.  Don't assume the teams
//			are named "Diamond Sword" and "Blood Eagle", either,
//			because lots of servers have modded that.
//			Next, there's a chance that even if I know the
//			names of the team, I still don't know which team is
//			which (friendly / enemy), so I still might not be
//			able to return a team number.
//			So you can see, this function is not guaranteed to
//			return 0 or 1 (or -1 for Observers).  It might return 
//			"" which means "I have no clue.  Try again later I 
//			might have learned more."
//
//		%location = Team::GetFlagLocation(Team::Enemy());
//			Get the location of the enemy team's flag.
//			%location will be one of:
//				1) "home"
//			   		It's safe at home.
//				2) "field"
//					It was dropped in the field.
//				3) Player's name
//					This player took it!
//				4) ""
//					I don't know where it is!
//					This can happen when you've just joined
//					a game in progress.
//			Note that a player using the name "home" or "field" will
//			confuse the script... as soon as I figure out which characters
//			can't be used in names, I'll probably change these.
//			So instead of checking against the string "home", check
//				if (%location == $Trak::locationHome)
//			and
//				if (%location == $Trak::locationField)
//			which are variables I defined so that when I change the
//			string you won't have to change your code.
//
//	I've also defined some new events:
//
//		eventTeamNamesUpdated(%team, %name)
//			The name of this team was just figured out.
//		eventFlagsUpdated(%team, %location)
//			The flag of this team was just moved to this
//			location.  (See location values, above)
//
//		eventFlagTaken(%teamNum, %client)
//			Team's flag was taken by client
//		eventFlagDropped(%teamNum, %client)
//			Team's flag was dropped by client
//		eventFlagCaptured(%teamNum, %client)
//			Team's flag was captured by client
//		eventFlagReturned(%teamNum, %client)
//			Team's flag was returned by client
//			Note that client *might* be 0 (server event)
//
//		eventTeammateJoined(%client)
//			A client joined your team
//		eventTeammateLeft(%client)
//			A client left your team
//			These are sort of tricky, and I'm not sure they're
//			in their final form.  What this means is that someone
//			joined your team or left it -- but not necessarily
//			by any action they did!  Because if you change teams,
//			you will get leaves & joins for *everyone in the game*
//			(It's all relative, you see!)
//			This is basically here to support event clients that
//			are keeping a list of teammates.
//
// ---------------------------------------------------------------------------
Include("presto\\Event.cs");
Include("presto\\Match.cs");
Include("presto\\List.cs");

$Trak::locationHome = "home";
$Trak::locationField = "field";

function Team::Reset() {
	deleteVariables("$TeamData::*");
	$TeamData::TeamNum[-1, name] = "Observer";
	}
function Team::NewGame() {
	$TeamData::TeamNum[0, flag] = "";
	$TeamData::TeamNum[1, flag] = "";
	}
Team::Reset();

function Team::GetName(%teamNum) {
	return $TeamData::TeamNum[%teamNum, name];
	}
function Team::SetName(%teamNum, %name) {
	if ($TeamData::TeamNum[%teamNum, name] != "")
		return;
	$TeamData::TeamNum[%teamNum, name] = %name;
	$TeamData::Team[%name] = %teamNum;
	Event::Trigger(eventTeamNamesUpdated, %teamNum);
	}
function Team::Friendly(%client) {
	if (%client == "")
		%client = getManagerId();
	return Client::GetTeam(%client);
	}
function Team::Enemy(%client) {
	if (%client == "")
		%client = getManagerId();
	// Wouldn't work for MultiTeam games... or Observers
	return 1 - Client::GetTeam(%client);
	}
function Team::GetByName(%name) {
	%teamNum = $TeamData::Team[%name];
	return %teamNum;
	}
function Team::GetList(%teamNum) {
	%list = $TeamData::list[%teamNum];
	if (%list == "") {
		%list = "listTeam"@%teamNum;
		$TeamData::list[%teamNum] = %list;
		List::New($TeamData::list[%teamNum]);
		List::NewSort($TeamData::list[%teamNum], byClient);
		}
	return %list;
	}

function Team::MoveFlag(%teamNum, %location) {
	if ($TeamData::TeamNum[%teamNum, flag] == %location)
		return;
	$TeamData::TeamNum[%teamNum, flag] = %location;
	Event::Trigger(eventFlagsUpdated, %teamNum, %location);
	}
function Team::GetFlagLocation(%teamNum) {
	return $TeamData::TeamNum[%teamNum, flag];
	}
function Team::RememberClient(%client) {
	if ($TeamData::client[%client,name] != "") {
		return;
		}
	%team = Client::GetTeam(%client);
	$TeamData::client[%client,name] = Client::GetName(%client);
	$TeamData::client[%client,teamNum] = %team;
	$TeamData::client[%client,teamName] = Team::GetName(%team);
	Team::AddClientToTeam(%client, %team);
	}
function Team::ForgetClient(%client) {
	if ($TeamData::client[%client,name] == "") {
		return;
		}
	%team = $TeamData::client[%client,teamNum];
	$TeamData::client[%client,name] = "";
	$TeamData::client[%client,teamNum] = "";
	$TeamData::client[%client,teamName] = "";
	Team::RemoveClientFromTeam(%client, %team);
	}
function Team::SetClientTeam(%client, %team) {
	if ($TeamData::client[%client,name] == "") {
		echo("move unregistered ",%client," to ",%team);
		return;
		}
	%oldTeam = $TeamData::client[%client,teamNum];
	if (%oldTeam == %team) {
		return;
		}

	Team::RemoveClientFromTeam(%client, %oldTeam);
	%team = Client::GetTeam(%client);
	$TeamData::client[%client,name] = Client::GetName(%client);
	$TeamData::client[%client,teamNum] = %team;
	$TeamData::client[%client,teamName] = Team::GetName(%team);
	Team::AddClientToTeam(%client, %team);
	}

function Team::_ForAllClients(%list, %client) {
	eval($TeamData::func @"(%client);");
	}
function Team::ForAllClients(%team, %func) {
	$TeamData::func = %func;
	List::CallSorted(Team::GetList(%team), byClient, Team::_ForAllClients);
	}
function Team::ReportMyTeamChange(%list, %client) {
	if (%client == getManagerId())
		return;
	Event::Trigger($TeamData::reportEvent, %client);
	}
function Team::AddClientToTeam(%client, %team) {
	if (!List::Add(Team::GetList(%team), %client))
		return false;
	if (%client == getManagerId()) {
		//Add everyone from the new team
		$TeamData::reportEvent = eventTeammateJoined;
		List::CallSorted(Team::GetList(%team), byClient, Team::ReportMyTeamChange);
		}
	else if (%team == Team::Friendly())
		Event::Trigger(eventTeammateJoined, %client);
	}
function Team::RemoveClientFromTeam(%client, %team) {
	if (!List::Remove(Team::GetList(%team), %client))
		return false;
	if (%client == getManagerId()) {
		//Remove everyone from the old team
		$TeamData::reportEvent = eventTeammateLeft;
		List::CallSorted(Team::GetList(%team), byClient, Team::ReportMyTeamChange);
		}
	else if (%team == Team::Friendly())
		Event::Trigger(eventTeammateLeft, %client);
	}

function Team::MatchPlayer(%wild) {
	if (Match::Count() == 1 && !$PrestoPref::StrictNameChecking)
		return 0;

	%i = 0;
	%matches = Match::Count();
	while (%i < %matches) {
		%w = Match::Result(%i, %wild);
		if (%w == "You" || getClientByName(%w) != 0)
			return %i;
		%i++;
		}
	return -1;
	}
function dumpClient(%client) {
	echo(" ",%client, ":", Client::getName(%client),"(",Client::GetTeam(%client),"/",
			$TeamData::client[%client,teamNum],")");
	}
function dumpTeams() {
	echo("team -1:", Team::GetName(-1));
	List::CallSorted(Team::GetList(-1), byClient, dumpClient);
	echo("team 0:",Team::GetName(0));
	List::CallSorted(Team::GetList(0), byClient, dumpClient);
	echo("team 1:",Team::GetName(1));
	List::CallSorted(Team::GetList(1), byClient, dumpClient);
	}

function Team::ParseFlagMessage(%action) {
	// Multiple possible matches - use protection
	%m = Team::MatchPlayer("p");
	if (%m == -1)
		return;

	// Set up the fields in this message which will be used in the next flag message.
	%player = Match::Result(%m, "p");
	if (%player == "You") {
		%client = getManagerId();
		%player = Client::getName(%client);
		}
	else	%client = getClientByName(%player);

	if (%action == took) {
		%teamFlag = Team::Enemy(%client);
		Team::MoveFlag(%teamFlag, %player);
		Event::Trigger(eventFlagTaken, %teamFlag, %client);
		}
	else if (%action == dropped) {
		%teamFlag = Team::Enemy(%client);
		Team::MoveFlag(%teamFlag, $Trak::locationField);
		Event::Trigger(eventFlagDropped, %teamFlag, %client);
		}
	else if (%action == returned) {
		%teamFlag = Team::Friendly(%client);
		Team::MoveFlag(%teamFlag, $Trak::locationHome);
		Event::Trigger(eventFlagReturned, %teamFlag, %client);
		}
	else if (%action == captured) {
		%teamFlag = Team::Enemy(%client);
		Team::MoveFlag(%teamFlag, $Trak::locationHome);
		Team::MoveFlag(Team::Friendly(%client), $Trak::locationHome, 0);
		Event::Trigger(eventFlagCaptured, %teamFlag, %client);
		}
	Team::SetName(%teamFlag, Match::Result(%m, "t"));
	return;
	}
function Team::ParseClientMessage(%client, %msg) {
	if (%client != 0) {
		return;
		}

	if (Match::String(%msg, "Match starts in * seconds.") ||
	    (%msg == "Match started.")) {
		Team::MoveFlag(0, $Trak::locationHome);
		Team::MoveFlag(1, $Trak::locationHome);
		return;
		}

	if (Match::String(%msg, "* dropped.")) {
		ForgetClient(getClientByName(Match::Result(0)));
		return;
		}

//  doesn't seem to be a real message.
//	if (Match::String(%msg, "* connected to the game.")) {
//		RememberClient(getClientByName(Match::Result(0)));
//		return;
//		}

	if (Match::ParamString(%msg, "%p took the %t flag! ")) { //note extra space.
		Team::ParseFlagMessage(took);
		return;
		}
	if (Match::ParamString(%msg, "%p returned the %t flag!")) {
		Team::ParseFlagMessage(returned);
		return;
		}
	if (Match::ParamString(%msg, "%p captured the %t flag!")) {
		Team::ParseFlagMessage(captured);
		return;
		}
	if (Match::ParamString(%msg, "%p dropped the %t flag!")) {
		Team::ParseFlagMessage(dropped);
		return;
		}

	if (Match::String(%msg, "* flag was returned to base.")) {
		%teamName = Match::Result(0);
		if (%teamName == "Your" || %teamName == "Your team's") // did I see this once?
			%teamFlag = Team::Friendly();
		else if (Match::String(%teamName, "The *")) {
			%teamName = Match::Result(0);
			%teamFlag = Team::GetByName(%teamName);
			}
		else	return;
		if (%teamFlag != -1) {
			Team::MoveFlag(%teamFlag, $Trak::locationHome);
			Event::Trigger(eventFlagReturned, %teamFlag, 0);
			}
		}
	return;
	}

// Here are the events I listen to.  For instance, when you change servers or
// missions you have to reset the Team database.
Event::Attach(eventChangeMission, "Team::NewGame();");
Event::Attach(eventConnectionAccepted, "Team::Reset();");
Event::Attach(eventClientMessage, Team::ParseClientMessage);
Event::Attach(eventClientJoin, Team::RememberClient);
Event::Attach(eventClientDrop, Team::ForgetClient);
Event::Attach(eventClientChangeTeam, Team::SetClientTeam);
