editActionMap("playMap.sae");

//entire hud visbility on/off
//bindCommand(keyboard0, make, "numpad-", TO, "EnemyHUD::Toggle(0);");
//bindCommand(keyboard0, break, "numpad-", TO, "");

//Disable/Enable EnemyHUD Entirely
$EnemyHUD::Active = true;

//2 second delay from when you die to when you can actually spawn in
$respawnDelay = 2;

//only display names after threat time
$toggleNonThreat = true;
$threatTime = 10;

//leave these values as is
$EnemyHUD::ToggleHUD = true;
$EnemyHUD::positionChange = false;
$EnemyHUD::GameStarted = false;

function EnemyHUD::Init() {
    
    if(!$EnemyHUD::Active)
        return;
	
	Hud::New( "Enemy_Hud", 0, 200, 200, 300, EnemyHUD::Wake, EnemyHUD::Sleep );

	// row 1
	newObject("EnemyHUD::Slot1", FearGuiFormattedText, 55, 19, 140, 16);
	newObject("EnemyHUD::Slot2", FearGuiFormattedText, 55, 64, 140, 16);
	newObject("EnemyHUD::Slot3", FearGuiFormattedText, 55, 109, 140, 16);
	newObject("EnemyHUD::Slot4", FearGuiFormattedText, 55, 154, 140, 16);
	newObject("EnemyHUD::Slot5", FearGuiFormattedText, 55, 199, 140, 16);

	// row 2
	newObject("EnemyHUD::Slot6", FearGuiFormattedText, 5, 10, 140, 16);
	newObject("EnemyHUD::Slot7", FearGuiFormattedText, 5, 55, 140, 16);
	newObject("EnemyHUD::Slot8", FearGuiFormattedText, 5, 100, 140, 16);
	newObject("EnemyHUD::Slot9", FearGuiFormattedText, 5, 145, 140, 16);
	newObject("EnemyHUD::Slot10", FearGuiFormattedText, 5, 190, 140, 16);

	
	Hud::Add( "Enemy_Hud", "EnemyHUD::Slot6" );
	Hud::Add( "Enemy_Hud", "EnemyHUD::Slot7" );
	Hud::Add( "Enemy_Hud", "EnemyHUD::Slot8" );
	Hud::Add( "Enemy_Hud", "EnemyHUD::Slot9" );
	Hud::Add( "Enemy_Hud", "EnemyHUD::Slot10" );
	Hud::Add( "Enemy_Hud", "EnemyHUD::Slot1" );
	Hud::Add( "Enemy_Hud", "EnemyHUD::Slot2" );
	Hud::Add( "Enemy_Hud", "EnemyHUD::Slot3" );
	Hud::Add( "Enemy_Hud", "EnemyHUD::Slot4" );
	Hud::Add( "Enemy_Hud", "EnemyHUD::Slot5" );


}

function EnemyHUD::Wake() {
	EnemyHUD::Update();
}

function EnemyHUD::Sleep() {
	
}

function EnemyHUD::ClearPositions()
{
	
	for( %slot = 1; %slot <= 5; %slot++ ) {

		$EnemyTeam::Position[ %slot ] = "";

	}
	
	EnemyHUD::Update();
}

function EnemyHUD::SelectPosition(%slot)
{

	if ($EnemyTeam::Position[ %slot ] == "") {

		$EnemyTeam::Position[ %slot ] = "CAP";
			
			
	}
	else if ($EnemyTeam::Position[ %slot ] == "CAP") {

		$EnemyTeam::Position[ %slot ] = "LO";

	}
	else if ($EnemyTeam::Position[ %slot ] == "LO") {

		$EnemyTeam::Position[ %slot ] = "CH";

	}
	else if ($EnemyTeam::Position[ %slot ] == "CH") {

		$EnemyTeam::Position[ %slot ] = "H";

	}
	else if ($EnemyTeam::Position[ %slot ] == "H") {

		$EnemyTeam::Position[ %slot ] = "";

	}
	else {
		//do nothing
	}
}

function EnemyHUD::Toggle(%slot) {
    
    if(!$EnemyHUD::Active)
        return;

    if (%slot == 11) {
		if ($toggleNonThreat) {

			$toggleNonThreat = false;
			EnemyHUD::Update();
		}
		else {
			$toggleNonThreat = true;
			EnemyHUD::Update();
		}
	}
    
    if (%slot == 0) {
        if($EnemyHUD::ToggleHUD) {
            
            for (%slot = 1; %slot <= 5; %slot++) {
                Control::SetVisible("EnemyHUD::Slot" @ %slot, false);
                Control::SetVisible("EnemyHUD::Slot" @ %slot + 5, false);
            }
            $EnemyHUD::ToggleHUD = false;
            
        }
        else {
            
            for (%slot = 1; %slot <= 5; %slot++) {
                Control::SetVisible("EnemyHUD::Slot" @ %slot, true);
                Control::SetVisible("EnemyHUD::Slot" @ %slot + 5, true);
            }
            $EnemyHUD::ToggleHUD = true;
        }  
    }

    //if ((%slot >= 6 && %slot <= 10)) {
	if (%slot == 10) {
		
		if (!$EnemyHUD::positionChange) {
			
			$EnemyHUD::positionChange = true;
			EnemyHUD::Update();
		}
		else {
		
			$EnemyHUD::positionChange = false;
			EnemyHUD::Update();
		}
		
		Schedule::Cancel("$EnemyHUD::positionChange = false;");
		Schedule::Cancel("EnemyHUD::Update();");
			
		Schedule::Add("$EnemyHUD::positionChange = false;", 3);
		Schedule::Add("EnemyHUD::Update();", 3.15);
    }
	
	if (%slot == 9) { EnemyHUD::ClearPositions(); }
	
	if (%slot >= 1 && %slot <= 5) {
	
		if(!$EnemyHUD::positionChange) {
			if(Control::GetVisible("EnemyHUD::Slot" @ %slot)) {
					Control::SetVisible("EnemyHUD::Slot" @ %slot, false);
					Control::SetVisible("EnemyHUD::Slot" @ %slot + 5, false);
			}
			else {
					Control::SetVisible("EnemyHUD::Slot" @ %slot, true);
					Control::SetVisible("EnemyHUD::Slot" @ %slot + 5, true);
			}
		}
		else {
			
			Schedule::Cancel("$EnemyHUD::positionChange = false;");
			Schedule::Cancel("EnemyHUD::Update();");
			
			Schedule::Add("$EnemyHUD::positionChange = false;", 3);
			Schedule::Add("EnemyHUD::Update();", 3.15);

			EnemyHUD::SelectPosition(%slot);
			EnemyHUD::Update();
		}
	}
}

function EnemyHUD::UpdateTeams() {
    
    if(!$EnemyHUD::Active)
        return;

	$EnemyHUD::EnemyTeam = Team::Enemy();
	
	%myid = getManagerId();
	
	$EnemyHUD::Enemies = 0;

	$NewCount = 0;
	
	for( %l = 1; %l <= $Team::Client::Count; %l++ ) {
		
		%team = $Team::Client::Team[ $Team::Client::List[ %l ] ];
				
		if( %team == $EnemyHUD::EnemyTeam && $EnemyHUD::Enemies < 5) {
		
			$EnemyTeam::Name[ $EnemyHUD::Enemies++ ] = $Team::Client::Name[ $Team::Client::List[ %l ] ];
			$EnemyTeam::PlayerNumber[ $EnemyHUD::Enemies ] = %l;

		}
	}
	EnemyHUD::Update();
}

function EnemyHUD::Update() {
    
    if(!$EnemyHUD::Active)
        return;

	for( %slot = 1; %slot <= $EnemyHUD::Enemies; %slot++ ) {
        
           %slotTimer = (%slot + 5);
           
           if ($EnemyHUD::Timer[$EnemyTeam::PlayerNumber[ %slot ]] < 3) {
               
               %theDot = "<B0,0:modules\\enemyhud\\gray0.png>";
           }
           else if (($EnemyHUD::Timer[$EnemyTeam::PlayerNumber[ %slot ]] >= 3) && ($EnemyHUD::Timer[$EnemyTeam::PlayerNumber[ %slot ]] <= 9)) {
               
               %theDot = "<B0,0:modules\\enemyhud\\green1.png>";
           }
           else if (($EnemyHUD::Timer[$EnemyTeam::PlayerNumber[ %slot ]] >= 10) && ($EnemyHUD::Timer[$EnemyTeam::PlayerNumber[ %slot ]] <= 14)) {
               
               %theDot = "<B0,0:modules\\enemyhud\\yellow2.png>";  
           }
           else if (($EnemyHUD::Timer[$EnemyTeam::PlayerNumber[ %slot ]] >= 15) && ($EnemyHUD::Timer[$EnemyTeam::PlayerNumber[ %slot ]] <= 19)) {
               
               %theDot = "<B0,0:modules\\enemyhud\\orange3.png>";
           }
           else if (($EnemyHUD::Timer[$EnemyTeam::PlayerNumber[ %slot ]] >= 20)) {
            
                %theDot = "<B0,0:modules\\enemyhud\\red4.png>";
           }
           else {
            
                //do nothing
           }
		   
		   //to let us know we are in position change mode
		   if ($EnemyHUD::positionChange) {
			   %theDot = "<f2>*";
		   }

        %enemyName = String::escapeFormatting( $EnemyTeam::Name[ %slot ] );
        %theFont = "<f2>";   
           
        if ($toggleNonThreat && $EnemyHUD::Timer[$EnemyTeam::PlayerNumber[ %slot ]] < $threatTime) {
            
            control::setValue( "EnemyHUD::Slot" @ %slot, "" );
            control::setValue( "EnemyHUD::Slot" @ %slotTimer, "" );
        }
        else {
            control::setValue( "EnemyHUD::Slot" @ %slot, %theFont @ %enemyName @ "<f2> " @  $EnemyTeam::Position[ %slot ]);
            control::setValue( "EnemyHUD::Slot" @ %slotTimer, %theDot );
        }
        
    }


	//when people drop clear out etc
	for( %slot = $EnemyHUD::Enemies + 1; %slot <= 5; %slot++ )
		control::setValue( "EnemyHUD::Slot" @ %slot, "" );

	for( %slot = $EnemyHUD::Enemies + 6; %slot <= 10; %slot++ )
		control::setValue( "EnemyHUD::Slot" @ %slot, "" );

}

function EnemyHUD::UpdateTimer() {
    
    if(!$EnemyHUD::Active)
        return;
	
	for( %slot = 1; %slot <= $EnemyHUD::Enemies; %slot++ ) {
        %slotTimer = (%slot + 5);
        
		if(($EnemyHUD::Timer[ $EnemyTeam::PlayerNumber[ %slot ] ] == 0) && ($respawnSwitch[ %slot ] < $respawnDelay)) {
            
			$EnemyHUD::Timer[ $EnemyTeam::PlayerNumber[ %slot ] ] = 0;
			$respawnSwitch[ %slot ]++;
            %theDot = "<B0,0:modules\\enemyhud\\gray0.png>";
            
		}
		else {
            
			$EnemyHUD::Timer[ $EnemyTeam::PlayerNumber[ %slot ] ]++;
			$respawnSwitch[ %slot ] = 0;
            %theDot = "<B0,0:modules\\enemyhud\\green1.png>";

		}
		
        if (($EnemyHUD::Timer[$EnemyTeam::PlayerNumber[ %slot ]] >= 10) && ($EnemyHUD::Timer[$EnemyTeam::PlayerNumber[ %slot ]] <= 14)) {
            
			%theDot = "<B0,0:modules\\enemyhud\\yellow2.png>";
		}
        else if (($EnemyHUD::Timer[$EnemyTeam::PlayerNumber[ %slot ]] >= 15) && ($EnemyHUD::Timer[$EnemyTeam::PlayerNumber[ %slot ]] <= 19)) {
            
			%theDot = "<B0,0:modules\\enemyhud\\orange3.png>";
		}
        else if (($EnemyHUD::Timer[$EnemyTeam::PlayerNumber[ %slot ]] >= 20)) {
            
			%theDot = "<B0,0:modules\\enemyhud\\red4.png>";
		}
		else {
            
			//do nothing
		}
		
		if ($EnemyHUD::positionChange) {
			   %theDot = "<f2>*";
		}

        %enemyName = String::escapeFormatting( $EnemyTeam::Name[ %slot ] );
        %theFont = "<f2>";

        if ($toggleNonThreat && $EnemyHUD::Timer[$EnemyTeam::PlayerNumber[ %slot ]] < $threatTime) {
            
            control::setValue( "EnemyHUD::Slot" @ %slot, "" );
            control::setValue( "EnemyHUD::Slot" @ %slotTimer, "" );
        }
        else {
            control::setValue( "EnemyHUD::Slot" @ %slot, %theFont @ %enemyName @ "<f2> " @  $EnemyTeam::Position[ %slot ]);
            control::setValue( "EnemyHUD::Slot" @ %slotTimer, %theDot );
        }
        
	}
	
	Schedule::Add("EnemyHUD::UpdateTimer();", 1);
	
}

function EnemyHUD::Reset() {
    
    if(!$EnemyHUD::Active)
        return;

    //%theDot = "<B0,0:modules\\enemyhud\\gray0.png>";
	
    for( %slot = 1; %slot <= $EnemyHUD::Enemies; %slot++ ) {
        
        //%slotTimer = (%slot + 5);
		
		if ($EnemyHUD::GameStarted) {
			
			$EnemyHUD::Timer[ $EnemyTeam::PlayerNumber[ %slot] ] = 1;
			
		}
		else {
			
			$EnemyHUD::Timer[ $EnemyTeam::PlayerNumber[ %slot] ] = 0;
			$respawnSwitch[ %slot ] = 0;
		}
		
        
        //if ($toggleNonThreat) {
            
            	//control::setValue( "EnemyHUD::Slot" @ %slotTimer, "" );
        //}
        //else {

            	//control::setValue( "EnemyHUD::Slot" @ %slotTimer, %theDot );
        //}
        
    }
	
	$EnemyHUD::GameStarted = false;
    EnemyHUD::UpdateTimer();

}

// suicides
function EnemyHUD::SetS_Status( %v, %w ) {
	if( Client::GetTeam( %v ) != $EnemyHUD::EnemyTeam )
		return;
	$EnemyHUD::Timer[ $Team::Client::Position[ %v ] ] = 0;
	EnemyHUD::Update();
}

// deaths and team kills
function EnemyHUD::SetStatus( %k, %v, %w ) {
	if( Client::GetTeam( %v ) != $EnemyHUD::EnemyTeam )
		return;
	$EnemyHUD::Timer[ $Team::Client::Position[ %v ] ] = 0;
	EnemyHUD::Update();
}

function EnemyHUD::TeamChange(%cl, %newteam) {
	
	$EnemyHUD::Timer[ $Team::Client::Position[ %cl ] ] = 1;
	
	if (getManagerId() == %cl) {

		//clear all positions
		EnemyHUD::Toggle(9);
		
		//turn threat toggle off when switching teams
		//this is for when half time switch occurs
		if ($toggleNonThreat) {
			EnemyHUD::Toggle(11);
		}
	}
}

function EnemyHUD::MatchStarted( %cl, %msg, %type )
	after onClientMessage {
  
	if(%type != 1 || !$EnemyHUD::Active)
		return;
	
	if ( String::FindSubStr(%msg, "Match started.") != -1) {
		//reset if match started
		$EnemyHUD::GameStarted = true;
		EnemyHUD::Reset();
	}
	else if ( String::FindSubStr(%msg, "First half has started. ") != -1) {
		//reset if match started
		$EnemyHUD::GameStarted = true;
		EnemyHUD::Reset();
	}
	else if ( String::FindSubStr(%msg, "Second half has started!") != -1) {
		//reset if match started
		$EnemyHUD::GameStarted = true;
		EnemyHUD::Reset();
	}
	else { }

}

EnemyHUD::Init();

Event::Attach( eventClientKilled, EnemyHUD::SetStatus );
Event::Attach( eventClientTeamKilled, EnemyHUD::SetStatus );
Event::Attach( eventClientSuicided, EnemyHUD::SetS_Status );
Event::Attach( eventClientsUpdated, EnemyHUD::UpdateTeams );
Event::Attach( eventClientChangeTeam, EnemyHud::TeamChange );
Event::Attach( eventConnected, EnemyHUD::Reset );