function GSpeed::Init() {
	if($GSpeed:Loaded)
		return;
	$GSpeed:Loaded = true;
	
	HUD::New("GSpeed::Container", 0, 0, 50, 20, GSpeed::Wake, GSpeed::Sleep );
	
	newObject("GSpeed::txt", FearGuiFormattedText, 0, 0, 50, 20);
	
	HUD::Add("GSpeed::Container","GSpeed::txt");
}

function GSpeed::Wake() { GSpeed::Update(); }
function GSpeed::Sleep() { Schedule::Cancel("GSpeed::Update();"); }

function GSpeed::Update() {
	Control::SetValue("GSpeed::txt", "<f8>" @ $speed );
	Schedule::Add("GSpeed::Update();",0.1);
}

GSpeed::Init();