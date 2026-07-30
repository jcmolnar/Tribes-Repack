function clock::Init() {
	
	HUD::New("clock::Container", 0, 0, 180, 20, clock::Wake, clock::Sleep);
	newObject("clock::Text", FearGuiFormattedText, 0, 0, 180, 20); 
	HUD::Add("clock::Container", "clock::Text");
		
	clock::Reset();
}

function clock::Wake() { clock::Update(); }
function clock::Sleep() { }
function clock::Reset() { clock::Update(); }

function clock::Update() { }


// mission clock
function clock::Reset() {
	$clock::Hour = 0;
	$clock::Min = 0;
	$clock::Sec = 0;
	$clock::CountingDown = false;
	Schedule::Add("clock::Iterate();", 1);
}

function clock::Update() {
	Control::setValue("clock::Text", "<jc><f1>" @ ( ( $clock::Hour < 10 ) ? "0" @ $clock::Hour : $clock::Hour ) @ ":" @ ( ( $clock::Min < 10 ) ? "0" @ $clock::Min : $clock::Min ) @ ":" @ ( ( $clock::Sec < 10 ) ? "0" @ $clock::Sec : $clock::Sec ) );
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