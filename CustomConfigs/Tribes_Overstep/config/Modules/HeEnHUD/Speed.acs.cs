function GSpeed::Init() {
	if($GSpeed:Loaded)
		return;
	$GSpeed:Loaded = true;
	
	HUD::New( "GSpeed::Container", 0, 0, 300, 200, GSpeed::Wake, GSpeed::Sleep );
	
	newObject("GSpeed::Texture", FearGuiFormattedText, 70, 0, 300, 200);
	
	HUD::Add("GSpeed::Container","GSpeed::Texture");
	
	Control::SetValue("GSpeed::Texture", "<B0,0:Modules/HeEnHUD/Sring.png>");

	// Identical to Health and Energy on purpose. The "buried / too far left" look was NOT
	// this offset: main.cpp was redefining GSpeed::Update after Autoload, replacing this
	// module's digit images with small left-aligned yellow text that no edit here could
	// undo. Fixed natively (main.cpp step 4 now gates that reroute to Basic's <f8> module),
	// so this offset matches the other two, which is what it should do.
	newObject("GSpeed::txt", FearGuiFormattedText, 25, 25, 0, 0);
	
	HUD::Add("GSpeed::Container","GSpeed::txt");
	


	Control::SetValue("GSpeed::txt", "<B0,0:Modules/numHUD/white/0.png>");	
}

function GSpeed::Wake() { GSpeed::Update(); }
function GSpeed::Sleep() { Schedule::Cancel("GSpeed::Update();"); }

function GSpeed::Update() {
	// Emit an image ONLY for digits that exist. Cosmetic tidy-up, NOT a bug fix -- see the
	// note above for the real cause of the yellow text. A missing "white/.png" lookup just
	// fails and is skipped; the digits that DID load still draw. This only avoids the
	// pointless failed resource lookup ten times a second.
	%d0 = String::GetSubStr($Speed,0,1);
	%d1 = String::GetSubStr($Speed,1,1);
	%d2 = String::GetSubStr($Speed,2,1);
	%numbers = "";
	if (%d0 != "")
		%numbers = %numbers @ "<B0,0:Modules/numHUD/white/" @ %d0 @ ".png>";
	if (%d1 != "")
		%numbers = %numbers @ "<B0,0:Modules/numHUD/white/" @ %d1 @ ".png>";
	if (%d2 != "")
		%numbers = %numbers @ "<B0,0:Modules/numHUD/white/" @ %d2 @ ".png>";
	Control::SetValue("GSpeed::txt", "<jc>"~%numbers );
	Schedule::Add("GSpeed::Update();",0.1);
}

GSpeed::Init();