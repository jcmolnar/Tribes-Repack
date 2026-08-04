$CtfHUD::Image[0, home] = "friendly.home.png";
$CtfHUD::Image[0, player] = "friendly.player.png";
$CtfHUD::Image[0, field] = "friendly.empty.png";

$CtfHUD::Image[1, home] = "enemy.home.png";
$CtfHUD::Image[1, player] = "enemy.player.png";
$CtfHUD::Image[1, field] = "enemy.empty.png";

function CtfHUD::Init() {
	if ( $CtfHUD::Loaded )
		return;
	$CtfHUD::Loaded = true;
	
	HUD::New("CtfHUD::Container", 272, 40, 835, 108, CtfHUD::Wake, CtfHUD::Sleep);
	
	newObject("CtfHUD::BG", FearGuiFormattedText, 260, 0, 300, 70); 
	
	
	newObject("CtfHUD::Image0", FearGuiFormattedText, 243, 70, 300, 70); 
	newObject("CtfHUD::Image1", FearGuiFormattedText, 392, 70, 300, 70);
	
	newObject("CtfHUD::Score0", FearGuiFormattedText, 279, 17, 150, 20);
	newObject("CtfHUD::Score1", FearGuiFormattedText, 497, 17, 150, 20);

	newObject("CtfHUD::Status0", FearGuiFormattedText, -92, 74, 220, 20); 
	newObject("CtfHUD::Status1", FearGuiFormattedText, 55, 74, 220, 20);
	

	HUD::Add("CtfHUD::Container", "CtfHUD::BG");

	HUD::Add("CtfHUD::Container", "CtfHUD::Image0");
	HUD::Add("CtfHUD::Container", "CtfHUD::Image1");
	
	HUD::Add("CtfHUD::Container", "CtfHUD::Score0");
	HUD::Add("CtfHUD::Container", "CtfHUD::Score1");
	
	HUD::Add("CtfHUD::Container", "CtfHUD::Status0");
	HUD::Add("CtfHUD::Container", "CtfHUD::Status1");

		// can delete these defaults later i think
	Control::SetValue( "CtfHUD::Score0", "<B0,0:Modules/numHUD/8.png>");
	Control::SetValue( "CtfHUD::Score1", "<B0,0:Modules/numHUD/8.png>");
	
	Control::SetValue( "CtfHUD::BG", "<B0,0:modules/numHUD/ctfHUD/bg.png>");

	CtfHUD::Reset();
}

function CtfHUD::Wake() { CtfHUD::Update(); }
function CtfHUD::Sleep() { }

function CtfHUD::Reset() {
	Control::SetValue("CtfHUD::Image0", "<b3,3:Modules/numHUD/CTFHud/friendly.home.png>");
	Control::SetValue("CtfHUD::Image1", "<b3,4:Modules/numHUD/CTFHud/enemy.home.png>");

	CtfHUD::Update();
}

function CtfHUD::Update() {
	//friendly team goes in slot 0
	CtfHUD::SetTeamValue( 0, Team::Friendly() );
	//enemy team goes in slot 0
	CtfHUD::SetTeamValue( 1, Team::Enemy() );
}


function CtfHUD::SetTeamValue( %slot, %team ) {
	%score = Team::Score(%team);
	%loc = Team::Flag::Location(%team);
	
	switch ( %loc ) {
		case "home":
			%loc = "<f2>";
			%bmp = $CtfHUD::Image[%slot, "home"];
			break;
		case "field":
			%loc = "<f2>" ~ Team::Flag::Timer(%team);
			%bmp = $CtfHUD::Image[%slot, "field"];
			break;
		default:
			%loc = "<f2>" ~ String::escapeFormatting(Client::GetName(%loc));
			%bmp = $CtfHUD::Image[%slot, "player"];
			break;
	}
	
	Control::SetValue( "CtfHUD::Image"~%slot, "<b3,3:Modules/numHUD/CTFHud/"~%bmp~">" );
	Control::SetValue( "CtfHUD::Status"~%slot, "<jc>"~%loc );
	Control::SetValue( "CtfHUD::Score"~%slot, "<B0,0:Modules/numHUD/black/"~ %score ~".png>" );
	//Control::SetValue( "CtfHUD::txt", "<jc>"~%numbers );

}

// if we change teams, the sides may need to be updated
function CtfHUD::SelfUpdate( %client, %team ) {
	if ( %client == getManagerId() )
		CtfHUD::Update();
}

Event::Attach( EventFlagUpdate, CtfHUD::Update );
Event::Attach( EventFlagTimerUpdate, CtfHUD::Update );
Event::Attach( EventClientChangeTeam, CtfHUD::SelfUpdate );

CtfHUD::Init();