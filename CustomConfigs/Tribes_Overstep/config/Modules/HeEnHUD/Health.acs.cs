function GHealth::Init() {
	if($GHealth:Loaded)
		return;
	$GHealth:Loaded = true;
	
	HUD::New( "GHealth::Container", 0, 0, 300, 200, GHealth::Wake, GHealth::Sleep );
	
	newObject("GHealth::Texture", FearGuiFormattedText, 70, 0, 300, 200);
	HUD::Add("GHealth::Container","GHealth::Texture");
	Control::SetValue("GHealth::Texture", "<B0,0:Modules/HeEnHUD/Hring.png>");

	newObject("GHealth::txt", FearGuiFormattedText, 25, 25, 0, 0);
	
	HUD::Add("GHealth::Container","GHealth::txt");

	Control::SetValue("GHealth::txt", "<B0,0:Modules/numHUD/white/0.png>");
}

function GHealth::Wake() { GHealth::Update(); }
function GHealth::Sleep() { Schedule::Cancel("GHealth::Update();"); }

function GHealth::Update() {
	// Emit an image ONLY for digits that exist. Cosmetic tidy-up, NOT a bug fix: health was
	// never broken. Building all three unconditionally makes a value under 100 ask for
	// "Modules/numHUD/white/.png" in the empty places, and that lookup simply fails and is
	// skipped -- the digits that DID load still draw. This just avoids the pointless failed
	// resource lookup ten times a second.
	//
	// (An earlier comment here blamed that failed lookup for the small yellow readout. That
	// was wrong. The yellow text only ever affected SPEED, and its cause was main.cpp
	// redefining GSpeed::Update after Autoload -- fixed natively; see main.cpp step 4.)
	//
	// The three health thresholds above were byte-identical (all "white/"), so they collapse
	// to one builder. If differently coloured digit sets are ever wanted, the numHUD folder
	// already carries Black/ and Score/ alongside White/ -- switch the folder per branch.
	%d0 = String::GetSubStr($health,0,1);
	%d1 = String::GetSubStr($health,1,1);
	%d2 = String::GetSubStr($health,2,1);
	%numbers = "";
	if (%d0 != "")
		%numbers = %numbers @ "<B0,0:Modules/numHUD/white/" @ %d0 @ ".png>";
	if (%d1 != "")
		%numbers = %numbers @ "<B0,0:Modules/numHUD/white/" @ %d1 @ ".png>";
	if (%d2 != "")
		%numbers = %numbers @ "<B0,0:Modules/numHUD/white/" @ %d2 @ ".png>";
	Control::SetValue("GHealth::txt", "<jc>"~%numbers );
	Schedule::Add("GHealth::Update();",0.1);
}

GHealth::Init();