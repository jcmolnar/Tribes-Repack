function HUDOverlay::Init() {
	if($HUDOverlay:Loaded)
		return;
	$HUDOverlay:Loaded = true;
	
	HUD::New( "HUDOverlay::Container", 0, 0, 380, 380, HUDOverlay::WakeSleep, HUDOverlay::WakeSleep );
	newObject("HUDOverlay::Texture", FearGuiFormattedText, 0, 0, 380, 380);
	HUD::Add("HUDOverlay::Container","HUDOverlay::Texture");
	Control::SetValue("HUDOverlay::Texture", "<B0,0:Modules/minimap/R1.png>");
	//Control::SetValue("HUDOverlay::Texture", "<B0,0:Modules/minimap/R2.png>");
}

function HUDOverlay::WakeSleep() { 
	// normally you'd use this to run things on wake or sleep but this overlay doesn't need it
 }

HUDOverlay::Init();