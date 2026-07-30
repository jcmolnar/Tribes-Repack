Event::Attach( eventClientJoin, StatHUD::playerActivityTimestamp );
Event::Attach( eventMatchStarted, StatHUD::playerTimestampReset );
Event::Attach( eventConnected, StatHUD::playerTimestampReset );
Event::Attach( eventClientDied, StatHud::onClientDied );
Event::Attach( eventClientKilled, StatHud::onClientKilled );
Event::Attach( eventClientTeamKilled, StatHud::onClientKilled );
Event::Attach( eventClientSuicided, StatHud::onClientSuicided );
Event::Attach( eventFlagUpdate, StatHUD::onFlagUpdate );

//=== 0 - Toggle Display
//=== 1 - Hold Button Display
$StatHUD::HoldDisplay = 1;

editActionMap("actionMap.sae");
bindCommand(keyboard0, make, l_alt, "s", TO, "StatHUD::Show();");
bindCommand(keyboard0, break, l_alt, "s", TO, "StatHUD::Hide();");

function StatHud::onClientDied( %victim ) {
	StatHUD::playerActivityTimestamp( %victim );

	if ( $StatHUD::StatSheetVisible )
		StatHUD::Show();
}

function StatHud::onClientKilled( %killer, %victim, %weapon ) {
	StatHUD::playerActivityTimestamp( %killer );
	StatHUD::playerActivityTimestamp( %victim );

	if ( $StatHUD::StatSheetVisible )
		StatHUD::Show();
}

function StatHud::onClientSuicided( %cl ) {
	StatHUD::playerActivityTimestamp( %cl );

	if ( $StatHUD::StatSheetVisible )
		StatHUD::Show();
}

function StatHUD::onFlagUpdate( %cl ) {
	StatHUD::playerActivityTimestamp( %cl );

	if ( $StatHUD::StatSheetVisible )
		StatHUD::Show();
}

function StatHUD::playerTimestampReset() {
	for ( %cl = Client::getFirst(); %cl != -1; %cl = Client::getNext( %cl ) )
		StatHUD::playerActivityTimestamp( %cl );
}

function StatHUD::playerActivityTimestamp( %cl ) {
	$StatHUD::PlayerTimeStamp[%cl] = getSimTime();
}

/////////////

function StatHUD::StopWatch( %time ) {
	if ( %time < 0 )
		%time = 0;
	%minutes = floor( %time / 60 );
	%seconds = floor( %time % 60 );
	return ( %minutes @ ":" @ String::lpad( %seconds, 2, "0" ) );
}


function StatHUD::LoadStats() {
	Stack::Freeze( "playerlist" );

	$StatHUD::PlayerCount[-1] = 0;
	$StatHUD::PlayerCount[0] = 0;
	$StatHUD::PlayerCount[1] = 0;

	Stack::Reset( "playerlist" );
	%count = Stack::Count( "playerlist" );
	for ( %i = 0; %i < %count; %i++ ) {
		%name = Stack::GetNext( "playerlist" );
		%cl = getClientByName( %name );
		%team = Client::getTeam( %cl );
		if ( !%cl )
			continue;

		$StatHUD::DisplayList[ %team, $StatHUD::PlayerCount[%team] ] = %name;
		$StatHUD::IdleTime[%name] = StatHUD::StopWatch( getSimTime() - $StatHUD::PlayerTimeStamp[%cl] );
		$StatHUD::PlayerCount[%team]++;
	}

	Stack::Unfreeze( "playerlist" );
}

// sort the players stats based on kills
function StatHUD::Sort() {
	for ( %team = -1; %team <= 1; %team++ ) {
		for ( %i = 0; %i < $StatHUD::PlayerCount[%team]-1; %i++ ) {
			for ( %j = %i+1; %j < $StatHUD::PlayerCount[%team]; %j++ ) {
				%namei = $StatHUD::DisplayList[ %team, %i ];
				%namej = $StatHUD::DisplayList[ %team, %j ];

				if ( $Collector::Score[ %namei ] < $Collector::Score[ %namej ] ) {
					%temp = $StatHUD::DisplayList[ %team, %i ];
					$StatHUD::DisplayList[ %team, %i ] = $StatHUD::DisplayList[ %team, %j ];
					$StatHUD::DisplayList[ %team, %j ] = %temp;
				}
			}
		}
	}
}

$StatHUD::CenterPrintFont = "sf_orange214_10.pft";
function StatHUD::Pixels( %str ) {
	return Font::getStringPixelWidth( $StatHUD::CenterPrintFont, %str );
}

// tab width in pixels
$StatHUD::Spacer = "\t";
$StatHUD::SpacerWidth = StatHud::Pixels( " " ) * 3; // how it's done in SimGui::TextFormat

// column widths in terms of our spacer
$StatHUD::headerPixelSize[0] = 10 * $StatHUD::SpacerWidth;
$StatHUD::headerPixelSize[1] = 4 * $StatHUD::SpacerWidth;
$StatHUD::headerPixelSize[2] = 4 * $StatHUD::SpacerWidth;
$StatHUD::headerPixelSize[3] = 4 * $StatHUD::SpacerWidth;
$StatHUD::headerPixelSize[4] = 4 * $StatHUD::SpacerWidth;
$StatHUD::headerPixelSize[5] = 4 * $StatHUD::SpacerWidth;
$StatHUD::headerPixelSize[6] = 4 * $StatHUD::SpacerWidth;
$StatHUD::headerPixelSize[7] = 4 * $StatHUD::SpacerWidth;
$StatHUD::headerPixelSize[8] = 4 * $StatHUD::SpacerWidth;
$StatHUD::headerPixelSize[9] = 4 * $StatHUD::SpacerWidth;
$StatHUD::headerPixelSize[10] = 4 * $StatHUD::SpacerWidth;
$StatHUD::headerPixelSize[11] = 5 * $StatHUD::SpacerWidth;
$StatHUD::headerPixelSize[12] = 5 * $StatHUD::SpacerWidth;
$StatHUD::headerPixelSize[13] = 4 * $StatHUD::SpacerWidth;
$StatHUD::headerPixelSize[14] = 4 * $StatHUD::SpacerWidth;
$StatHUD::headerPixelSize[15] = 4 * $StatHUD::SpacerWidth;


// handle blanks
function StatHud::Number( %n ) {
	return ( %n != "" ) ? %n : 0;
}

function StatHUD::TimeNumber( %duration ) {
	if ( %duration == "-" )
		return ( %duration );
	else if ( %duration == "" || %duration == "0" ) 
		return ( "0" );
	%hr = String::lpad( floor( %duration / 3600 ), 2, "0" );
	%duration = ( %duration % 3600 );
	%min = String::lpad( floor( %duration / 60 ), 2, "0" ); 
	%duration = ( %duration % 60 );
	%sec = String::lpad( floor( %duration ), 2, "0" );
	%time = %min @ ":" @ %sec;
	if ( %hr > 0 )
		%time = %hr @ ":" @ %time;
	return StatHud::Number( %time );
}


// kill/death formatting
function StatHUD::KillDeathPair( %kills, %deaths ) {
	return ( StatHud::Number( %kills ) @ "/" @ StatHud::Number( %deaths ) );
}

// tab an item over to fill it's column
function StatHUD::TabItem( %col, %text ) {
	%textpixels = StatHud::Pixels( %text );
	%columnpixels = $StatHUD::headerPixelSize[ %col ];

	// we are assuming the text width will never surpass the column
	%pixeldiff = ( %columnpixels - %textpixels );
	if ( %pixeldiff <= 0 ) echo( %pixeldiff @ " in column " @ %col );
	%tabs = floor( ( ( %pixeldiff + $StatHUD::SpacerWidth - 1 ) / $StatHUD::SpacerWidth ) );

	if ( %col == 0 )
		%text = String::escapeFormatting( %text ); // col 0 is the name, escape any < chars
	return ( %text @ String::Dup( $StatHUD::Spacer, %tabs ) );
}

// create a line of tabbed items
function StatHUD::TabList( %item0, %item1, %item2, %item3, %item4, %item5, %item6, %item7, %item8, %item9, %item10, %item11, %item12, %item13, %item14, %item15, %item16 ) {
	%list = "\t\t\t\t";

	// highlight our name!
	if ( %item[0] == Client::GetName(getManagerId()) )
		%list = %list @ "<f3>";
	else
		%list = %list @ "<f1>";

	for ( %i = 0; %i < 17; %i++ )
		%list = %list @ StatHUD::TabItem( %i, %item[%i] );
	return ( %list @ "\n" );
}


// display the player stats
function StatHUD::ShowStats() {
	//%topline = StatHUD::TabList("Name", "Rating", "K/D", "Disc", "Mortar", "Chain", "CKill", "TK", "Idle") @ "\n";
	%topline = StatHUD::TabList( "Name", "Rating", "K/D", "TKs", "Disc", "Nade", "Chain", "CKills", "Returns", "BBs", "Grabs", "Pickups", "Assists", "Caps", "MAs", "Dmg", "Flag" ) @ "\n";

	for ( %team = -1; %team <= 1; %team++ ) {
		for ( %i = 0; %i < $StatHUD::PlayerCount[%team] ; %i++ ) {
			%name = $StatHUD::DisplayList[ %team, %i ];
            %tempFlagTime0[%name] = TimeC::Duration( $Marker::Flag @ ( 0 ), %name );
            %tempFlagTime1[%name] = TimeC::Duration( $Marker::Flag @ ( 1 ), %name );
			%topline = %topline @ StatHUD::TabList(
				%name,
				$Collector::Score[%name],
				StatHUD::KillDeathPair( $Collector::Kills[%name], $Collector::Deaths[%name] ),
                StatHUD::KillDeathPair( $Collector::TeamKills[%name], $Collector::TeamDeaths[%name] ),
                StatHUD::KillDeathPair( $Collector::Kills[%name, "Disc"], $Collector::Deaths[%name, "Disc"] ),
                StatHUD::KillDeathPair( $Collector::Kills[%name, "Explosive"], $Collector::Deaths[%name, "Explosive"] ),
                StatHUD::KillDeathPair( $Collector::Kills[%name, "Chaingun"], $Collector::Deaths[%name, "Chaingun"] ),
                StatHUD::Number( $Collector::CarrierKills[%name] ),
                StatHUD::Number( ($Collector::Returns[ %name ] + $Collector::StandoffReturns[ %name ]) ),
                StatHUD::KillDeathPair( $Collector::BBGiven[%name], $Collector::BBTaken[%name] ),
                StatHUD::Number( $Collector::Grabs[%name] ),
                StatHUD::Number( $Collector::Pickups[%name] ),
                StatHUD::Number( $Collector::Assists[%name] ),
                StatHUD::Number( $Collector::Caps[%name] ),
                StatHUD::KillDeathPair( $Collector::MAGiven[%name], $Collector::MATaken[%name] ),
                StatHUD::KillDeathPair( floor(($Collector::DamageOut[%name]/10)), floor(($Collector::DamageIn[%name]/10)) ),
                StatHUD::TimeNumber( (%tempFlagTime0[%name] + %tempFlagTime1[%name]) )
                );
		}

		%topline = %topline @ "\n";
 	}

	remoteCP( 2048, %topline, 100 );
}

// show the player top level function
function StatHUD::Show() {
    
    if(!$StatHUD::ToggleHUD){
        
        if (!$StatHUD::HoldDisplay){
            $StatHUD::ToggleHUD = true;
        }
    
	$StatHUD::StatSheetVisible = true;
    
	StatHUD::LoadStats();
	StatHUD::Sort();
	StatHUD::ShowStats();
    
    }
    else {
        $StatHUD::ToggleHUD = false;
        $StatHUD::StatSheetVisible = false;
        remoteCP( 2048, '', 0 );
        
    }
}

// hide the players
function StatHUD::Hide() {
    
    if(!$StatHUD::HoldDisplay)
        return;
    
	$StatHUD::StatSheetVisible = false;
	remoteCP( 2048, '', 0 );
}