// bookmark cam settings
$DemoCam::UpdateTime = 1; 
$DemoCam::DemoLead = 4; //open bookmark window
$DemoCam::DemoWindow = $DemoCam::DemoLead * 2; // bookamrk dwell time
$DemoCam::DemoSpeedUp = 4; // speed between bookmarks 
$DemoCam::DemoScale = 1;
$DemoCam::CurrentMark = 100;	
$happymod::permiff = true;
$happymod::MinimapTerrain = false;

// move to bookmark time
function DemoCam::MoveToBookmark( %num, %val ) {
	echoc( 2, "DemoCam::MoveToBookmark " ~ %num ~ " " ~ %val );
	if( %val < 0.1 )
		%val = 0.1;
	$DemoCam::CurrentMark = %num;
	$DemoCam::DemoScale = %val;
	
	%bookevent = getword( $Demo::bookmark[ %num ], 0 );
	%booktime = getword( $Demo::bookmark[ %num ], 1 );
		
	%nowtime = floor( getsimtime() );
	%settime = ( %booktime - $DemoCam::DemoLead );
			
	if( %nowtime < %settime ) {
		$SimGame::TimeScale = $DemoCam::DemoSpeedUp;
		schedule::add( "DemoCam::MoveToBookmark(" ~ %num ~ "," ~ %val ~ ");", $DemoCam::UpdateTime, dcmtb );
	}
	else {
		%num++;
		if( $DemoCam::DemoScale > 0.1 )
			schedule::add( "DemoCam::MoveToBookmark(" ~ %num ~ "," ~ %val ~ ");", $DemoCam::DemoWindow, dcmtb ); 
		else
			schedule::cancel( dcmtb );
				
		$SimGame::TimeScale = $DemoCam::DemoScale;
	}
}

// set binds
function DemoBookmark::BindInit() after GameBinds::Init {
  
	$GameBinds::CurrentMapHandle = GameBinds::GetActionMap2( "playmap.sae");
	$GameBinds::CurrentMap = "playmap.sae";
	GameBinds::addBindCommand( "Set Bookmark", "Demo::Bookmarker();" );
	GameBinds::addBindCommand( "First BookMark", "dcmtb(101,1);" );
	GameBinds::addBindCommand( "Next BookMark", "dcmtn(1);" );
	GameBinds::addBindCommand( "Stop BookMark", "schedule::cancel(dcmtb);" );
}

// load bookmarks
function DemoCam::LoadBookmarks( %filename ) after RecordingsGui::PlayDemo {
	
	if( $Demo::BookExport )
		return;
	
	%tempname = string::replace( %filename, ".rec", ".cs" );
	%tempname2 = string::replace( %tempname, "recordings/", "modules/DemoBookmarks/" );
	exec( %tempname2 );
	echoc( 2, "DemoCam::LoadBookmarks() " ~ %filename );
}

function dcmtb(%num, %val) {
	
	DemoCam::MoveToBookmark(%num, %val);
}

function dcmtn(%val) {
	
	if( %val < 0.1 )
		%val = 1;
	%num = $DemoCam::CurrentMark++;
	DemoCam::MoveToBookmark(%num, %val);
}
