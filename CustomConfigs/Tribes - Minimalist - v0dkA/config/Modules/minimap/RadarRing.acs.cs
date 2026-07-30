function RadarRing::Init() {
    if($RadarRing:Loaded)
        return;
    $RadarRing:Loaded = true;
    
    HUD::New( "RadarRing::Container", 0, 0, 300, 300, RadarRing::WakeSleep, RadarRing::WakeSleep );
    newObject("RadarRing::Texture", FearGuiFormattedText, 0, 0, 300, 300);
    HUD::Add("RadarRing::Container","RadarRing::Texture");
    Control::SetValue("RadarRing::Texture", "<B0,0:Modules/minimap/ring.png>");
}

function RadarRing::WakeSleep() { 
    // normally you'd use this to run things on wake or sleep but this overlay doesn't need it
 }

RadarRing::Init();