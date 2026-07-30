// Demo Cam Controls For Tribes 1.4+ || lemon version for demo bookmarker
// credits runar freaky? cowboy dynamix andrew plasmatic

$DemoCam::FlipMouse = true;
function Demo::onConnected() {
	if ( !$playingDemo )
		return;
	
	Demo::SpeedControl( "normal" );
	pushActionMap( "demoMap.sae" );
}

function Demo::onLeaveServer() {
	if ( !$playingDemo )
		return;
	
	Demo::SpeedControl( "normal" );
	popActionMap( "demoMap.sae" );
}

function Demo::SpeedControl( %fn ) {
	if ( !$playingDemo )
		return;

	switch ( %fn ) {
		case "pause": $Demo::CurrentScale = 0; break;
		case "sd": $Demo::CurrentScale--; break;
		case "ff": $Demo::CurrentScale++; break;
		case "normal":
		default: $Demo::CurrentScale = $Demo::NormalSpeed; break;
	}

	$Demo::CurrentScale = clamp( $Demo::CurrentScale, 0, $Demo::Count );
	$SimGame::TimeScale = $Demo::Scale[ $Demo::CurrentScale ];
}

function DemoCam::Create() {
	
	$cam[0] = Group::getObject(9, 0); //remember the first cams id# (player perspective) -plasmatic.
	$camnum++;
	%CameraID = newobject(DemoCam, EditCamera, "move.sae"); // create cam
	addtoset(9, %CameraID); // SimCameraSet
	addtoset(6, %CameraID); // Simset
	postAction(PlayGui, Attach, DemoCam); // Attach it
	focus(DemoCam);	
	DemoCam::Move(12); // Default move speed
	$cam[$camnum] = %CameraID;// Add the id of the current cam to list.. -plasmatic
	$camcurrent = $camnum;
}

function DemoCam::Move( %speed, %posRot )
{
	if( %speed == "" )
		$MoveSpeed = 2;
	else
		$MoveSpeed = %speed;

	if( %posRot == "" )
		$PosRotation = 0.2;
	else
		$PosRotation = %posRot;
	%NegRotation = strcat( "-", $PosRotation );
	
	editActionMap("move.sae");
	bindAction( keyboard0, make, a, TO, IDACTION_MOVELEFT, $MoveSpeed );
	bindAction( keyboard0, break, a, TO, IDACTION_MOVELEFT, 0 );
	bindAction( keyboard0, make, d, TO, IDACTION_MOVERIGHT, $MoveSpeed );
	bindAction( keyboard0, break, d, TO, IDACTION_MOVERIGHT, 0 );
	bindAction( keyboard0, make, s, TO, IDACTION_MOVEBACK, $MoveSpeed );
	bindAction( keyboard0, break, s, TO, IDACTION_MOVEBACK, 0 );
	bindAction( keyboard0, make, w, TO, IDACTION_MOVEFORWARD, $MoveSpeed );
	bindAction( keyboard0, break, w, TO, IDACTION_MOVEFORWARD, 0 );
	bindAction( keyboard0, make, e, TO, IDACTION_MOVEUP, $MoveSpeed );
	bindAction( keyboard0, break, e, TO, IDACTION_MOVEUP, 0 );
	bindAction( keyboard0, make, c, TO, IDACTION_MOVEDOWN, $MoveSpeed );
	bindAction( keyboard0, break, c, TO, IDACTION_MOVEDOWN, 0 );
	pushActionMap("move.sae");
	
}

$Demo::CurrentScale = 9;

function DemoCam::switch()
{
	$camcurrent++;
	if($camcurrent > $camnum)
		$camcurrent = 0;
	echo("switching to cam # " @ $camcurrent @ " id# " @ $cam[$camcurrent]);
	postAction(PlayGui, Attach, $cam[$camcurrent]); // 
	focus($cam[$camcurrent]);	
} 

// push new binds
function DemoCam::PushBinds() after RecordingsGui::PlayDemo {
	
	$SimGame::TimeScale = 36.0;	//Start up demo quickly. -Plasmatic
	newActionMap("move.sae");	//created memory problems where it was before. -Plasmatic
	editActionMap("move.sae");
	if( $DemoCam::FlipMouse ) {
	bindAction(mouse0, xaxis0, TO, IDACTION_YAW, Scale, 0.001);
	bindAction(mouse0, yaxis0, TO, IDACTION_PITCH, Scale, 0.001);
	}
	else {
	bindAction(mouse0, xaxis0, TO, IDACTION_YAW, Flip, Scale, 0.001);
	bindAction(mouse0, yaxis0, TO, IDACTION_PITCH, Scale, 0.001);
	}
	bindCommand(keyboard0, make, "space", TO, "DemoCam::switch();"); // Modify space to switch cams -plasmatic
	bindCommand(keyboard0, break, "space", TO, "$Null = '';");
	bindCommand(keyboard0, make, "f12", TO, "if($playingDemo)DemoCam::Create();"); // Moved here to eliminate binding conflicts -Plasmatic
	bindCommand( keyboard0, make, 1, to, "DemoCam::Move(0.2);");
	bindCommand( keyboard0, make, 2, to, "DemoCam::Move(0.4);");
	bindCommand( keyboard0, make, 3, to, "DemoCam::Move(0.6);");
	bindCommand( keyboard0, make, 4, to, "DemoCam::Move(0.8);");
	bindCommand( keyboard0, make, 5, to, "DemoCam::Move(1);");
	bindCommand( keyboard0, make, 6, to, "DemoCam::Move(2);");
	bindCommand( keyboard0, make, 7, to, "DemoCam::Move(3);");
	bindCommand( keyboard0, make, 8, to, "DemoCam::Move(5);");
	bindCommand( keyboard0, make, 9, to, "DemoCam::Move(8);");
	bindCommand( keyboard0, make, 0, to, "DemoCam::Move(11);"); // it goes to 11
	pushActionMap("move.sae");
}

Event::Attach( eventConnected, Demo::onConnected );
Event::Attach( eventLeaveServer, Demo::onLeaveServer );

$Demo::Count = -1;
$Demo::Scale[$Demo::Count++] = 0.000000000;
$Demo::Scale[$Demo::Count++] = 0.00390625;
$Demo::Scale[$Demo::Count++] = 0.0078125;
$Demo::Scale[$Demo::Count++] = 0.015625;
$Demo::Scale[$Demo::Count++] = 0.03125;
$Demo::Scale[$Demo::Count++] = 0.0625;
$Demo::Scale[$Demo::Count++] = 0.125;
$Demo::Scale[$Demo::Count++] = 0.25;
$Demo::Scale[$Demo::Count++] = 0.5;
$Demo::Scale[$Demo::Count++] = 1;
$Demo::NormalSpeed = $Demo::Count;
$Demo::Scale[$Demo::Count++] = 2;
$Demo::Scale[$Demo::Count++] = 3;
$Demo::Scale[$Demo::Count++] = 4;
$Demo::Scale[$Demo::Count++] = 5;
$Demo::Scale[$Demo::Count++] = 6;
$Demo::Scale[$Demo::Count++] = 8;
$Demo::Scale[$Demo::Count++] = 10;
$Demo::Scale[$Demo::Count++] = 12;
$Demo::Scale[$Demo::Count++] = 14;
$Demo::Scale[$Demo::Count++] = 16;
$Demo::Scale[$Demo::Count++] = 20;
$Demo::Scale[$Demo::Count++] = 24;
$Demo::Scale[$Demo::Count++] = 28;
$Demo::Scale[$Demo::Count++] = 32;
$Demo::Scale[$Demo::Count++] = 36;


function DemoCam::SetSpeed() after RecordingsGui::onOpen {
	
	popActionMap("move.sae");
	$SimGame::TimeScale = 1.0;	// Reset speed -Plasmatic
}

function DemoCam::SetMove( %sv, %missionName ) after remoteMInfo {
	
	if(%server == 2048) {
		$SimGame::TimeScale = 1.0;
		DemoCam::Move();
	}
}
