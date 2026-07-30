function GEnergy::Init() {
	if($GEnergy:Loaded)
		return;
	$GEnergy:Loaded = true;
	
	HUD::New( "GEnergy::Container", 0, 0, 156, 28, GEnergy::Wake, GEnergy::Sleep );
	
	newObject("GEnergy::Frame", FearGuiFormattedText, 0, 0, 156, 28);
	newObject("GEnergy::EBar", FearGuiFormattedText, 11, 11, 135, 7);
	
	HUD::Add("GEnergy::Container","GEnergy::Frame");
	HUD::Add("GEnergy::Container","GEnergy::EBar");
	
	Control::SetValue("GEnergy::Frame", "<B0,0:Modules/HeEnHUD/frame.png>");
	Control::SetValue("GEnergy::EBar", "<B0,0:Modules/HeEnHUD/energybar.png>");
}

function GEnergy::Wake() { GEnergy::Update(); }
function GEnergy::Sleep() { Schedule::Cancel("GEnergy::Update();"); }

function GEnergy::Update() {
	Control::SetExtent("GEnergy::EBar", $energy*1.35, 7 );
	Schedule::Add("GEnergy::Update();",0.1);
}

GEnergy::Init();