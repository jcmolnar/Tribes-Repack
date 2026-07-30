function GHealth::Init() {
	if($GHealth:Loaded)
		return;
	$GHealth:Loaded = true;
	
	HUD::New( "GHealth::Container", 0, 0, 100, 20, GHealth::Wake, GHealth::Sleep );

	newObject("GHealth::txt", FearGuiFormattedText, 0, 0, 80, 20);

	HUD::Add("GHealth::Container","GHealth::txt");
}

function GHealth::Wake() { GHealth::Update(); }
function GHealth::Sleep() { Schedule::Cancel("GHealth::Update();"); }

function GHealth::Update() {
		if ( $health > 66 ) {
			Control::SetValue("GHealth::txt", "<jc><f:small-black-stroke.pft:ffffffff:000000ff:1,1>" @ $health );
		} else if ( $health > 32 ) {
			Control::SetValue("GHealth::txt", "<jc><f:small-black-stroke.pft:fff000ff:000000ff:1,1>" @ $health );
		} else {
			Control::SetValue("GHealth::txt", "<jc><f:small-black-stroke.pft:ff0000:000000ff:1,1>" @ $health );
		}

	//Control::SetValue("GHealth::txt", "<jc><f:white2.pft:FFFFFF>" @ $health );
	Schedule::Add("GHealth::Update();",0.1);
}

GHealth::Init();