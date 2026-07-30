// Lemon's JumpJet for 1.40+
// requires unhappyjump.dll

$Lem::JumpDelay = 0.02;

function LemJumpJet::addBindsToMenu() after GameBinds::Init {
	
	$GameBinds::CurrentMapHandle = GameBinds::GetActionMap2( "playMap.sae");
	$GameBinds::CurrentMap = "playMap.sae";
	GameBinds::addBindCommand( "Lemon's JumpJet", "Lem::JumpJet(1);", "Lem::JumpJet(0);" );
	GameBinds::addBindCommand( "Lemon's Ski", "Lem::Ski(1);", "Lem::Ski(0);" );
}

function Lem::JumpJet( %val ) {
	
	switch ( %val ) {
		case "0": { postAction( 2048,IDACTION_JET, 0 ); break; }
		case "1": { postAction( 2048, IDACTION_MOVEUP, 1 ); postAction( 2048, IDACTION_JET, 1 ); }
	}
}

function Lem::Ski( %val ) {
	
	switch ( %val ) {
		case "0": { postAction( 2048, IDACTION_MOVEUP, 0 ); schedule::cancel( "lemski" ); break; }
		case "1": { postAction( 2048, IDACTION_MOVEUP, 1 ); schedule::add( "Lem::Ski( 1 );", $Lem::JumpDelay, "lemski" ); }
	}
}