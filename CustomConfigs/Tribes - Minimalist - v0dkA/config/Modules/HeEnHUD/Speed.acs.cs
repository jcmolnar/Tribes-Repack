function GSpeed::Init() {
	if($GSpeed:Loaded)
		return;
	$GSpeed:Loaded = true;
	
	HUD::New("GSpeed::Container", 0, 0, 80, 30, GSpeed::Wake, GSpeed::Sleep );
	
	newObject("GSpeed::txt", FearGuiFormattedText, 0, 5, 80, 30);
	
	HUD::Add("GSpeed::Container","GSpeed::txt");
}

function GSpeed::Wake() { GSpeed::Update(); }
function GSpeed::Sleep() { Schedule::Cancel("GSpeed::Update();"); }

function GSpeed::Update() {
	Control::SetValue("GSpeed::txt", "<jc><f:Speed-Font.pft:ffc600FF>" @ $speed );
	Schedule::Add("GSpeed::Update();",0.1);
}

GSpeed::Init();