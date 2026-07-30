// 1.40 stuff hud
// Legendz FPS Hud

Event::Attach(eventGuiOpen, TimeFPS::Wake);

function LegendzFPSHUD::Init() {
	
	HUD::New("LegendzFPSHUD::Container", 0, 0, 106, 26, LegendzFPSHUD::Wake, LegendzFPSHUD::Sleep);

	newObject("FPS", FearGuiFormattedText, 0, 1, 100, 20);

	HUD::Add("LegendzFPSHUD::Container", "FPS");
		
	LegendzFPSHUD::Reset();
}

function LegendzFPSHUD::Wake() { LegendzFPSHUD::Update(); }
function LegendzFPSHUD::Sleep() { }

function LegendzFPSHUD::Reset() {
	LegendzFPSHUD::Update();
}

function LegendzFPSHUD::Update() { }

function TimeFPS::Wake() {
	TimeFPS::Update();
}

function TimeFPS::Sleep() {
	Schedule::Cancel("TimeFPS::Update();");
}

function TimeFPS::Update() {
	%fps ="<f2> FPS:<f3> " @ floor($ConsoleWorld::FrameRate)@"";
	Control::SetValue("FPS", %fps);
	Schedule::Add("TimeFPS::Update();", 1);
}

LegendzFPSHUD::Init();