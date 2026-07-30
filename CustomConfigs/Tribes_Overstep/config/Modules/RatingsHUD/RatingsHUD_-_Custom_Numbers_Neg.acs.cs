Event::Attach( eventConnectionAccepted, RatingsHUD::Update );
Event::Attach( eventClientScoreAdd, RatingsHUD::Update );
Event::Attach( eventMatchStarted, RatingsHUD::Clear );

//==SET VARIABLES HERE==

$RatingsHUD::maxScore = 3000;

$RatingsHUD::xbar = 160;
$RatingsHUD::ybar = 14;

//==DO NOT TOUCH BELOW==

$RatingsHUD::textWidth = 45;
$RatingsHUD::xbbar = ($RatingsHUD::xbar + 2);
$RatingsHUD::ybbar = ($RatingsHUD::ybar + 2);
$RatingsHUD::Mult = ($RatingsHUD::xbar/$RatingsHUD::maxScore);

//======================

function RatingsHUD::Init() {
	if($RatingsHUD:Loaded)
		return;
	$RatingsHUD:Loaded = true;
    
	HUD::New( "RatingsHUD::Container", 0, 0, 300, 90, RatingsHUD::Wake, RatingsHUD::Sleep );
    newObject("RatingsHUD::Border", FearGuiFormattedText, 13, 10, 300, 40);
    HUD::Add("RatingsHUD::Container","RatingsHUD::Border");
	newObject("RatingsHUD::ET", FearGuiFormattedText, 10, 30, 25, 32);
	HUD::Add("RatingsHUD::Container","RatingsHUD::ET");
    Control::SetValue("RatingsHUD::Border", "<B0,0:Modules/RatingsHUD/borderbar.png>");
    Control::SetValue("RatingsHUD::ET", "<B0,0:Modules/numHUD/0.png>");
    
    $RatingsHUD::SoundCount = 0;
}

function RatingsHUD::Wake() { return; }
function RatingsHUD::Sleep() { return; }

function RatingsHUD::Update( %cl, %scoreAdd ) {
    
    if (getManagerId() != %cl) { return; }
    
    %name = Client::GetName( %cl );
    
    if ($Collector::Score[%name] == "") { $Collector::Score[%name] = 0; }

    if ($Collector::Score[%name] < 0) { %neg_prefix = "<B0,0:Modules/numHUD/score/NSign.png>"; }

	%numbers = %neg_prefix @ "<B0,0:Modules/numHUD/score/"~String::GetSubStr($Collector::Score[%name],0,1)~".png><B0,0:Modules/numHUD/score/"~String::GetSubStr($Collector::Score[%name],1,1)~".png><B0,0:Modules/numHUD/score/"~String::GetSubStr($Collector::Score[%name],2,1)~".png><B0,0:Modules/numHUD/score/"~String::GetSubStr($Collector::Score[%name],3,1)~".png>";
	Control::SetValue("RatingsHUD::ET",  %numbers );
    
if ($Collector::Score[%name] >= 500 && !$RatingsHUD::SoundCount) {
        $RatingsHUD::SoundCount++;
        localSound(jamz500);
    }
    else if ( ($Collector::Score[%name] >= 1000) && ($RatingsHUD::SoundCount == 1) ) {
        $RatingsHUD::SoundCount++;
        localSound(jamz1000);
    }
    else if ( ($Collector::Score[%name] >= 1500) && ($RatingsHUD::SoundCount == 2) ) {
        $RatingsHUD::SoundCount++;
        localSound(jamz1500);
    }
    else if ( ($Collector::Score[%name] >= 2000) && ($RatingsHUD::SoundCount == 3) ) {
        $RatingsHUD::SoundCount++;
        localSound(jamz2000);
    }
    else if ( ($Collector::Score[%name] >= 3000) && ($RatingsHUD::SoundCount == 4) ) {
        $RatingsHUD::SoundCount++;
        localSound(jamz3000);
    }
    else { }
}

function RatingsHUD::Clear() { $RatingsHUD::SoundCount = 0; }

RatingsHUD::Init();