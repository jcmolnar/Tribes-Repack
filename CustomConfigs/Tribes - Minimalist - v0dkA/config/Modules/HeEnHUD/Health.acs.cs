function GHealth::Init() {
    if($GHealth:Loaded)
        return;
    $GHealth:Loaded = true;
    
    HUD::New( "GHealth::Container", 0, 0, 156, 28, GHealth::Wake, GHealth::Sleep );
    
    newObject("GHealth::Frame", FearGuiFormattedText, 0, 0, 156, 28);
    newObject("GHealth::HBar", FearGuiFormattedText, 11, 11, 135, 7);
    
    HUD::Add("GHealth::Container","GHealth::Frame");
    HUD::Add("GHealth::Container","GHealth::HBar");
    
    Control::SetValue("GHealth::Frame", "<B0,0:Modules/HeEnHUD/frame.png>");
    Control::SetValue("GHealth::HBar", "<B0,0:Modules/HeEnHUD/healthbar.png>");
}

function GHealth::Wake() { GHealth::Update(); }
function GHealth::Sleep() { Schedule::Cancel("GHealth::Update();"); }

function GHealth::Update() {
    Control::SetExtent("GHealth::HBar", $health*1.35, 7 );
    Schedule::Add("GHealth::Update();",0.1);
}

GHealth::Init();