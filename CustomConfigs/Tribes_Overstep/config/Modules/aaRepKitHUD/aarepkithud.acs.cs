function aaRepKitHUD::Init() {
	if ($aaRepKitHUD::Loaded)
		return;
	$aaRepKitHUD::Loaded = true;

	$aaRepKitHUD::Awake = false;
	$aaRepKitHUD::Kits = 0;
	
	HUD::New( "aaRepKitHUD::Container", 200, 200, 130, 40, aaRepKitHUD::Wake, aaRepKitHUD::Sleep );
	newObject( "aaRepKitHUD::Text", FearGuiFormattedText, 0, 0, 130, 40 );
	HUD::Add( "aaRepKitHUD::Container", "aaRepKitHUD::Text" );
}

function aaRepKitHUD::Wake() {
	$aaRepKitHUD::Awake = true;
	aaRepKitHUD::Update();
}

function aaRepKitHUD::Sleep() {
	Schedule::Cancel("aaRepKitHUD::Update();");
	$aaRepKitHUD::Awake = false;
}

function aaRepKitHUD::Update() {
	if ( !$aaRepKitHUD::Awake )
		return;

	Schedule::Add("aaRepKitHUD::Update();", 1);
	
	%text = "";
	%kits = getItemCount("Repair Kit");

	//dont bother updating if count hasnt changed
	if ( %kits == $aaRepKitHUD::Kits ) 
		return;

	$aaRepKitHUD::Kits = %kits;
 
	%kits = ( %kits > 0 ) ? "rkit.png" : "blankdot.png";

	%text = "<B0,0:modules/aaRepKitHUD/" @ %kits @ ">";
	Control::SetValue( "aaRepKitHUD::Text", %text );
}

aaRepKitHUD::Init();

Event::Attach(eventItemReceived, "Schedule::Add(\"aaRepKitHUD::Update();\", 0);");
Event::Attach(eventItemDropped, "Schedule::Add(\"aaRepKitHUD::Update();\", 0);");
Event::Attach(eventItemUsed, "Schedule::Add(\"aaRepKitHUD::Update();\", 0);");
