function RadarOverlay::Init() {
    if($RadarOverlay:Loaded)
        return;
    $RadarOverlay:Loaded = true;
    
    HUD::New( "RadarOverlay::Container", 0, 0, 308, 308, RadarOverlay::WakeSleep, RadarOverlay::WakeSleep );
    newObject("RadarOverlay::Texture", FearGuiFormattedText, 0, 0, 308, 308);
    HUD::Add("RadarOverlay::Container","RadarOverlay::Texture");
    // This legacy frame encoded its unused corners as opaque black.  Apply
    // native near-black keying only to this tagged bitmap; gray frame art and
    // ordinary black PNG content elsewhere remain intact.
    Control::SetValue("RadarOverlay::Texture", "<B0,0:Modules/minimap/radar.png:keyblack>");
}

function RadarOverlay::WakeSleep() { 
    // normally you'd use this to run things on wake or sleep but this overlay doesn't need it
 }

RadarOverlay::Init();
