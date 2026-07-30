function GEnergy::Init() {
	if($GEnergy:Loaded)
		return;
	$GEnergy:Loaded = true;
	
	HUD::New( "GEnergy::Container", 0, 0, 300, 200, GEnergy::Wake, GEnergy::Sleep );
	
	newObject("GEnergy::Texture", FearGuiFormattedText, 70, 0, 300, 200);
	
	HUD::Add("GEnergy::Container","GEnergy::Texture");
	
	Control::SetValue("GEnergy::Texture", "<B0,0:Modules/HeEnHUD/Ering.png>");

	newObject("GEnergy::txt", FearGuiFormattedText, 25, 25, 0, 0);
	
	HUD::Add("GEnergy::Container","GEnergy::txt");
	


	Control::SetValue("GEnergy::txt", "<B0,0:Modules/numHUD/white/0.png>");	
}

function GEnergy::Wake() { GEnergy::Update(); }
function GEnergy::Sleep() { Schedule::Cancel("GEnergy::Update();"); }

function GEnergy::Update() {
	// Emit an image ONLY for digits that exist. Cosmetic tidy-up, NOT a bug fix: energy was
	// never broken. Building all three unconditionally makes a value under 100 ask for
	// "Modules/numHUD/white/.png" in the empty places, and that lookup simply fails and is
	// skipped -- the digits that DID load still draw. This just avoids the pointless failed
	// resource lookup ten times a second.
	//
	// (An earlier comment here blamed that failed lookup for the small yellow readout. That
	// was wrong. The yellow text only ever affected SPEED, and its cause was main.cpp
	// redefining GSpeed::Update after Autoload -- fixed natively; see main.cpp step 4.)
	%d0 = String::GetSubStr($energy,0,1);
	%d1 = String::GetSubStr($energy,1,1);
	%d2 = String::GetSubStr($energy,2,1);
	%numbers = "";
	if (%d0 != "")
		%numbers = %numbers @ "<B0,0:Modules/numHUD/white/" @ %d0 @ ".png>";
	if (%d1 != "")
		%numbers = %numbers @ "<B0,0:Modules/numHUD/white/" @ %d1 @ ".png>";
	if (%d2 != "")
		%numbers = %numbers @ "<B0,0:Modules/numHUD/white/" @ %d2 @ ".png>";
	Control::SetValue("GEnergy::txt", "<jc>"~%numbers );
	Schedule::Add("GEnergy::Update();",0.1);
}

GEnergy::Init();