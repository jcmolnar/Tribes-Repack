$low=1;
$up=1;
function LHHud::Init() {
	if($LHHud:Loaded)
		return;
	$LHHud:Loaded = true;
	HUD::New( "LHHud::Container", 0, 0, 72, 72, LHHud::Wake, LHHud::Sleep );
	newObject("LHHud::HM", FearGuiFormattedText, 0, 0, 56, 0);
	newObject("LHHud::JT", FearGuiFormattedText, 230, 45, 100, 100);
	HUD::Add("LHHud::Container","LHHud::HM");

	LHHud::Reset();
	
}

function LHHud::Wake() { Schedule::Add("LHHud::Update();", 3); }
function LHHud::Sleep() { }

function LHHud::Reset(){
	 Schedule::Add("LHHud::Update();", 3);
}
function LHHud::Update() {

 	if($health < 50)
 	{
 		Control::SetValue("LHHud::HM", "<B0,0:Modules/LowHealth/low" ~ $low ~ ".png>");
 		if($up)
 		{
 			$low++;
 			if($low>6){
 				$low=6;
 				$up=0;
 			}
 		}
 		else
 		{
 			$low--;
 			if($low<1){
			 	$low=1;
			 	$up=1;
			 }
 			
 		}
 	}
 	else if($health == 0){
 		Control::SetValue("LHHud::HM", "<B0,0:Modules/LowHealth/low6.png>");
 	}
 	else{
 		Control::SetValue("LHHud::HM", "<B0,0:Modules/LowHealth/h1.png>");
	}
 	Control::SetExtent("LHHud::HM", 72, 125-$health/100*142 );
	Schedule::Add("LHHud::Update();",0.1);
}

LHHud::Init();

