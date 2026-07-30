$lc_numspath = "<B0,0:Modules/numHUD/clock/";
$lc_numscol = "<B0,0:Modules/numHUD/clock/colon.png>";

function clock::Init() {
	
	HUD::New("clock::Container", 0, 0, 160, 50, clock::Wake, clock::Sleep);
	newObject("clock::Min", FearGuiFormattedText, 10, 5, 70, 50);
	newObject("clock::Sep", FearGuiFormattedText, 68, 5, 10, 50);
	newObject("clock::Sec", FearGuiFormattedText, 78, 5, 70, 50);
	HUD::Add("clock::Container", "clock::Min");
	HUD::Add("clock::Container", "clock::Sep");
	HUD::Add("clock::Container", "clock::Sec");
	Control::setValue( "clock::Sep", $lc_numscol );
}

function clock::Wake() { clock::Update(); }
function clock::Sleep() { }

// mission clock
function clock::Reset() {
	$clock::Hour = 0;
	$clock::Min = 0;
	$clock::Sec = 0;
	$clock::CountingDown = false;
	Schedule::Add("clock::Iterate();", 1);
}

function clock::Update() {
	
	Control::setValue( "clock::Min", ($clock::Min < 10) ?
	$lc_numspath ~ "0.png>" ~ $lc_numspath ~ $clock::Min ~ ".png>" :
	$lc_numspath ~ String::GetSubStr($clock::Min, 0, 1) ~ ".png>" ~ $lc_numspath ~ String::GetSubStr($clock::Min, 1, 1) ~ ".png>" );
	
	Control::setValue( "clock::Sec", ($clock::Sec < 10) ?
	$lc_numspath ~ "0.png>" ~ $lc_numspath ~ $clock::Sec ~ ".png>" :
	$lc_numspath ~ String::GetSubStr($clock::Sec, 0, 1) ~ ".png>" ~ $lc_numspath ~ String::GetSubStr($clock::Sec, 1, 1) ~ ".png>" );
}

function clock::UpdateTime(%min, %sec) {
	$clock::Hour = floor(%min / 60);
	$clock::Min = %min % 60;
	$clock::Sec = %sec;
	$clock::CountingDown = true;
	Schedule::Add("clock::Iterate();", 1);
}

function clock::SetReverse() {
	$clock::Hour = $clock::Min = $clock::Sec = 0;
	$clock::CountingDown = false;
	
	Schedule::Add("clock::Iterate();", 1);
	clock::Update();
}

function clock::Iterate() {
	if ($clock::CountingDown)
	{
		if ($clock::Sec > 0)
			$clock::Sec--;
		else
		{
			$clock::Sec = 59;
		
			if ($clock::Min > 0)
				$clock::Min--;
			else
			{
				$clock::Min = 59;
				$clock::Hour--;
			}
		}
	}
	else
	{
		if ($clock::Sec < 59)
			$clock::Sec++;
		else
		{
			$clock::Sec = 0;
		
			if ($clock::Min < 59)
				$clock::Min++;
			else
			{
				$clock::Min = 0;
				$clock::Hour++;
			}
		}
	}
	clock::Update();
	Schedule::Add("clock::Iterate();", 1);
}

Event::Attach(eventConnected, clock::Reset);
Event::Attach(eventUpdateTime, clock::UpdateTime);
Event::Attach(eventMatchStarted, clock::SetReverse);

clock::Init();