ModernHUD::attach("eventConnectionAccepted", "Team::Reset");
ModernHUD::attach("eventChangeMission", "Team::Init");
ModernHUD::attach("eventMatchStarted", "Team::Init");

// Canonical Presto TeamTrak event names (TeamTrak.cs emits Taken/Dropped/
// Captured/Returned with args (teamFlag, client)). The previous aliases
// (eventFlagGrab/Pickup/Drop/Cap/Return) exist in no installed emitter, so the
// flag state never left "home".
ModernHUD::attach("eventFlagTaken", "Team::Flag::Taken");
ModernHUD::attach("eventFlagDropped", "Team::Flag::Dropped");
ModernHUD::attach("eventFlagCaptured", "Team::Flag::Captured");
ModernHUD::attach("eventFlagReturned", "Team::Flag::Returned");

ModernHUD::attach("eventTeamAdd", "Team::onTeamAdd");
ModernHUD::attach("eventClientJoin", "Team::onClientJoin");
ModernHUD::attach("eventClientDrop", "Team::onClientDrop");
ModernHUD::attach("eventClientChangeTeam", "Team::onClientChangeTeam");

function remoteTeamScore( %sv, %team, %score ) {
	$Team::Score[%team] = %score;
}

function Team::Reset() {
	DeleteVariables( "$Team::Client*" );
	$Team::Client::Count = 0; 
	
	for ( %i = 0; %i < 2; %i++ ) {
		$Team::Client::TeamSize[%i] = 0; 
		$Team::Client::TeamSize[%i] = 0;
	}
	
	Team::Init();
}

function Team::Init() {
	for ( %i = 0; %i < 2; %i++ ) {
		$Team::Score[%i] = 0;
		$Team::Flag::Location[%i] = "home";
		$Team::Flag::Timer[%i] = 0;
		$Team::Flag::TimerTag[%i] = 0;
	}
	
	if ( $ServerMissionType == "trabbit" )
		$Team::Flag::TimerStart = Timer::New(9, 0+1); //9.0 + 1 for the advance call
	else
		$Team::Flag::TimerStart = Timer::New(47, 5+1); //47.5 + 1 for the advance call
}

function Team::Friendly() {
	%team = Client::getTeam(getManagerId());
	return ( %team == -1 ) ? 0 : %team;
}

function Team::Enemy() {
	return ( Team::Friendly() ^ 1 );
}

function Team::onTeamAdd( %team, %name ) {
	$Team::Name[ %team - 1 ] = %name;
}

function Team::Flag::Location( %team ) {
	return $Team::Flag::Location[%team];
}

function Team::Flag::Timer( %team ) {
	return Timer::FormatSeconds($Team::Flag::Timer[%team]);
}

function Team::Score( %team ) {
	return $Team::Score[%team];
}


// Team Flag Events

// $pref::ModernHUD::FlagDiag = 1 traces every transition: event, team, client,
// resolved name, old -> new location.
function Team::Flag::diag( %evt, %team, %cl, %new ) {
	if ( $pref::ModernHUD::FlagDiag )
		echo( "[FLAG] " @ %evt @ " team=" @ %team @ " cl=" @ %cl
			@ " (" @ Client::GetName(%cl) @ ") "
			@ $Team::Flag::Location[%team] @ " -> " @ %new );
}

function Team::Flag::Dropped( %team, %cl ) {
	Team::Flag::diag( "Dropped", %team, %cl, "field" );
	$Team::Flag::Location[%team] = "field";
	$Team::Flag::Timer[%team] = $Team::Flag::TimerStart;
	$Team::Flag::TimerTag[%team]++;
	
	Team::Flag::DropTimer( %team, $Team::Flag::TimerTag[%team] );
}

function Team::Flag::Taken( %team, %cl ) {
	Team::Flag::diag( "Taken", %team, %cl, %cl );
	if ( $Team::Flag::Location[%team] == "field" )
		$Team::Flag::TimerTag[%team]++;
	
	$Team::Flag::Location[%team] = %cl;
}

function Team::Flag::Captured( %team, %cl ) {
	Team::Flag::diag( "Captured", %team, %cl, "home" );
	$Team::Flag::Location[0] = "home";
	$Team::Flag::Location[1] = "home";
	
	$Team::Score[ %team^1 ]++;
}

function Team::Flag::Returned( %team, %cl ) {
	Team::Flag::diag( "Returned", %team, %cl, "home" );
	$Team::Flag::TimerTag[%team]++;
	$Team::Flag::Location[%team] = "home";
}

// flag timer
function Team::Flag::DropTimer( %team, %tag ) {
	if ( ($Team::Flag::TimerTag[%team] != %tag) || ($Team::Flag::Location[%team] != "field") )
		return;

	$Team::Flag::Timer[%team] = Timer::Dec($Team::Flag::Timer[%team]);
	
	if( $Team::Flag::Timer[%team] <= 0 ) {
		$Team::Flag::Location[%team] = "home";
		$Team::Flag::TimerTag[%team]++;

		Event::Trigger( EventFlagReturned, %team, 0 );
		Event::Trigger( EventFlagUpdate );
	} else {
		Event::Trigger( EventFlagTimerUpdate, %team, Team::Flag::Timer(%team) );
		Schedule::Add( sprintf( "Team::Flag::DropTimer(%1, %2);", %team, $Team::Flag::TimerTag[%team] ), 0.1 );
	}
}


// team client listings
function Team::AddClient( %cl, %team ) {
	$Team::Client::Count++;
	$Team::Client::TeamSize[ %team ]++;
	
	// add client to the end of the list
	$Team::Client::List[ $Team::Client::Count ] = %cl;
	$Team::Client::Position[ %cl ] = $Team::Client::Count;
	
	//set the client info
	$Team::Client::Team[ %cl ] = %team;
	$Team::Client::Name[ %cl ] = Client::GetName( %cl );
}

function Team::DropClient( %cl, %team ) {
	// get some positions
	%lastcl			= $Team::Client::List[ $Team::Client::Count ];
	%dropclientpos	= $Team::Client::Position[ %cl ];
	
	// set the last client to the dropping clients position
	$Team::Client::List[ %dropclientpos ]	= %lastcl;
	$Team::Client::Position[ %lastcl ]		= %dropclientpos;
	
	// erase dropping client
	$Team::Client::Position[ %cl ] = "";
	$Team::Client::List[ $Team::Client::Num[%team] ] = "";
	$Team::Client::Team[ %cl ] = "";
	
	// dec
	$Team::Client::Count--;
	$Team::Client::TeamSize[ %team ]--;
}

function Team::onClientJoin( %cl ) {
	Team::AddClient( %cl, Client::GetTeam(%cl) );

	Event::Trigger(eventClientsUpdated);
}

function Team::onClientDrop( %cl ) {
	Team::DropClient( %cl, $Team::Client::Team[ %cl ] );

	Event::Trigger(eventClientsUpdated);
}

function Team::onClientChangeTeam( %cl, %team ) {
	Team::DropClient( %cl, $Team::Client::Team[ %cl ] );
	Team::AddClient( %cl, %team );

	Event::Trigger(eventClientsUpdated);
}

function Team::Size( %team ) {
	return $Team::Client::TeamSize[ %team ];
}

