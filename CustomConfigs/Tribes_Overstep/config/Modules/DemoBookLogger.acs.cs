// tribes demo bookmark logger

$Demo::BookExport = true; // set true to log demo bookmarks
$Demo::Screenshot = true;

function DemoBookmark::BindInit() after GameBinds::Init {
  
	$GameBinds::CurrentMapHandle = GameBinds::GetActionMap2( "playmap.sae");
	$GameBinds::CurrentMap = "playmap.sae";
	GameBinds::addBindCommand( "Set Bookmark", "Demo::Bookmarker();" );
}

// manual bookmark and screenshot
function Demo::Bookmarker() {
	
    $Demo::bookmark[ $Demo::Index++ ] = sprintf( "%1 %2 %3", "CamBookmark", floor( getsimtime() ), $cam[$camcurrent] );
	if($Demo::Screenshot)
		screenShot(MainWindow);
    localsound( "camera_click.ogg" );
}

// setup bookmarks file
function Demo::SetupBookmarks( %filename ) after RecordingsGui::PlayDemo {

	$Demo::Index = 99;
	$bookmarkFileName = string::replace( %filename, ".rec", ".cs" );
	$Demo::bookmark[ $Demo::Index++ ] = sprintf( "%1 %2 %3", "Bookmark File for:", %filename, timestamp::format() );
}

// export bookmarks
function Demo::ExportBookmarks() {
	
	if( $Demo::BookExport )
		export( "$Demo::Bookmark*",  $bookmarkFileName, 0 );
	Demo::ClearBookmarks();
}

// clear bookmarks
function Demo::ClearBookmarks() {
	
	deleteVariables( "$Demo::*" );
	$bookmarkFileName = "";
}

function Demo::FlagCap( %team, %cl ) {
	
	$Demo::bookmark[ $Demo::Index++ ] = sprintf( "%1 %2 ~%3~ %4", "Cap", floor( getsimtime() ), Client::GetName( %cl ), %team );
}

function Demo::FlagGrab( %team, %cl ) {
	
	$Demo::bookmark[ $Demo::Index++ ] = sprintf( "%1 %2 ~%3~ %4", "Grab", floor( getsimtime() ), Client::GetName( %cl ), %team );
}

function Demo::FlagCarrierKill( %cl ) {
	
	$Demo::bookmark[ $Demo::Index++ ] = sprintf( "%1 %2 ~%3~", "CK", floor( getsimtime() ), Client::GetName( %cl ) );
}

function DemoCam::AttachCreate() after DemoCam::Create {
	
	$Demo::bookmark[ $Demo::Index++ ] = sprintf( "%1 %2 %3", "CamCreate", floor( getsimtime() ), $cam[$camcurrent] );
}

function DemoBookmark::LoadRecordings() {
	
	$DemoRecCount = 0;
	Bootstrap::evalSearchPath();
	%rec = File::FindFirst("*.rec");
	$DemoRecList[$DemoRecCount++] = %rec;
	echoc( 2, %rec );
	while(%rec != "") {
	   %rec = File::FindNext("*.rec");
	   $DemoRecList[$DemoRecCount++] = %rec;
	   echoc( 2, %rec );
	}
	export( "$DemoRecList*", "reclist.cs", 0 );
}

//exec(reclist);
function dbpd() { DemoBookmark::ProcessDemos(); }
function dblr(){ DemoBookmark::LoadRecordings(); }

Event::Attach( eventPlaybackOver, Demo::ExportBookmarks );
Event::Attach( eventFlagCap, Demo::FlagCap );
Event::Attach( eventFlagGrab, Demo::FlagGrab );
Event::Attach( eventFlagCarrierKill, Demo::FlagCarrierKill );