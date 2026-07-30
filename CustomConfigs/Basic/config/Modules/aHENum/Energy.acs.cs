function GEnergy::Init() {
	if($GEnergy:Loaded)
		return;
	$GEnergy:Loaded = true;
	
	HUD::New( "GEnergy::Container", 0, 0, 100, 20, GEnergy::Wake, GEnergy::Sleep );

	newObject("GEnergy::txt", FearGuiFormattedText, 0, 0, 80, 20);

	HUD::Add("GEnergy::Container","GEnergy::txt");
}

function GEnergy::Wake() { GEnergy::Update(); }
function GEnergy::Sleep() { Schedule::Cancel("GEnergy::Update();"); }

function GEnergy::Update() {
		if ( $energy < 35 ) {
			Control::SetValue("GEnergy::txt", "<jc><f:small-black-stroke.pft:ff0000ff:000000ff:1,1>" @ $energy );
		} else {
			Control::SetValue("GEnergy::txt", "<jc><f:small-black-stroke.pft:ffffffff:000000ff:1,1>" @ $energy );
		}

	//Control::SetValue("GEnergy::txt", "<jc><f:white2.pft:FFFFFF>" @ $energy );
	Schedule::Add("GEnergy::Update();",0.1);
}

GEnergy::Init();