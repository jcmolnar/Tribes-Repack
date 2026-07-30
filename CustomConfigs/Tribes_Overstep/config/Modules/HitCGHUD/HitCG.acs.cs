editActionMap("playMap.sae");

bindCommand(keyboard0, make, "n", TO, "HitCGHUD::Toggle();");
bindCommand(keyboard0, break, "n", TO, "");

//Disable/Enable HitCG Entirely
$HitCGHUD::Active = true;

//on/off toggle switch for a bind
$HitCGHUD::Toggle = 1;

function HitCGHUD::Init() {
    
    if(!$HitCGHUD::Active)
        return;
	
    //set your resolution here
    %w = 2560;
    %h = 1440;
    
    //dimension of hitmarker image file is 30x30
    %hm = 60;
    
    %x = ((%w/2) - (%hm/2));
    %y = ((%h/2) - (%hm/2));
    
	Hud::New( "HitCG1_Hud", %x, %y, %hm, %hm, HitCGHUD::Wake, HitCGHUD::Sleep );
	newObject("HitCGHUD::Marker100", FearGuiFormattedText, 0, 0, %hm, %hm);
    newObject("HitCGHUD::Marker75", FearGuiFormattedText, 0, 0, %hm, %hm);
    newObject("HitCGHUD::Marker50", FearGuiFormattedText, 0, 0, %hm, %hm);
    newObject("HitCGHUD::Marker25", FearGuiFormattedText, 0, 0, %hm, %hm);
    
	Hud::Add( "HitCG1_Hud", "HitCGHUD::Marker100" );
    Hud::Add( "HitCG1_Hud", "HitCGHUD::Marker75" );
    Hud::Add( "HitCG1_Hud", "HitCGHUD::Marker50" );
    Hud::Add( "HitCG1_Hud", "HitCGHUD::Marker25" );
    
    %marker100 = "<B0,0:Modules\\HitCGHUD\\hitmark100.png>";
    %marker75 = "<B0,0:Modules\\HitCGHUD\\hitmark75.png>";
    %marker50 = "<B0,0:Modules\\HitCGHUD\\hitmark50.png>";
    %marker25 = "<B0,0:Modules\\HitCGHUD\\hitmark25.png>";
    
    control::setValue( "HitCGHUD::Marker100", %marker100 );
    control::setValue( "HitCGHUD::Marker75", %marker75 );
    control::setValue( "HitCGHUD::Marker50", %marker50 );
    control::setValue( "HitCGHUD::Marker25", %marker25 );
    
    Control::SetVisible("HitCGHUD::Marker100", false);
    Control::SetVisible("HitCGHUD::Marker75", false);
    Control::SetVisible("HitCGHUD::Marker50", false);
    Control::SetVisible("HitCGHUD::Marker25", false);
}

function HitCGHUD::Wake() {
	//HitCGHUD::Update();
}

function HitCGHUD::Sleep() {
	
}

function HitCGHUD::Toggle() {
    
    if(!$HitCGHUD::Active)
        return;
    
    if($HitCGHUD::Toggle == 2) {
        
        $HitCGHUD::Toggle = 0;
        remoteEP("<f2>HitCG HUD: <f1>OFF", 3, 2, 2, 10, 300);
        HitCGHUD::ClearAll();
        
    }
    else if ($HitCGHUD::Toggle == 0) {
        
        $HitCGHUD::Toggle = 1;
        remoteEP("<f2>HitCG HUD: <f1>ON", 3, 2, 2, 10, 300);
        HitCGHUD::ClearAll();
        
    }
    else {
        
        $HitCGHUD::Toggle = 2;
        remoteEP("<f2>HitCG HUD: <f1>ADJUST", 3, 2, 2, 10, 300);
        Control::SetVisible("HitCGHUD::Marker100", true);
        
    }
}


function HitCGHUD::Update(%cl) {
    
    if(!$HitCGHUD::Active || $HitCGHUD::Toggle == 0 || $HitCGHUD::Toggle == 2)
        return;
    
    %myid = getManagerId();
    if(%cl != %myid) { return; }
   

    //when cg event occurs set visible
    Control::SetVisible("HitCGHUD::Marker100", true);
    schedule("HitCGHUD::GoInvisible(100);", 0.05);

}

function HitCGHUD::GoInvisible(%x)
{
    if (%x == 100) {
        Control::SetVisible("HitCGHUD::Marker100", false);
        Control::SetVisible("HitCGHUD::Marker75", true);
        schedule("HitCGHUD::GoInvisible(75);", 0.05);
    }
    else if (%x == 75) {
        Control::SetVisible("HitCGHUD::Marker75", false);
        Control::SetVisible("HitCGHUD::Marker50", true);
        schedule("HitCGHUD::GoInvisible(50);", 0.05);
    }
    else if (%x == 50) {
        Control::SetVisible("HitCGHUD::Marker50", false);
        Control::SetVisible("HitCGHUD::Marker25", true);
        schedule("HitCGHUD::GoInvisible(25);", 0.05);
        
    }
    else if (%x == 25) {
        Control::SetVisible("HitCGHUD::Marker25", false);
    }
    else { }
    
}

function HitCGHUD::ClearAll()
{
    
    Control::SetVisible("HitCGHUD::Marker100", false);
    Control::SetVisible("HitCGHUD::Marker75", false);
    Control::SetVisible("HitCGHUD::Marker50", false);
    Control::SetVisible("HitCGHUD::Marker25", false);

}


// suicides - clientsuicided
function HitCGHUD::clientSuicide( %v, %w ) 
{
    
    HitCGHUD::ClearAll();

}

// deaths and team kills - clientkilled - clientteamkilled
function HitCGHUD::clientKilled( %k, %v, %w )
{

    %myid = getManagerId();
    if (%v == %myid) { HitCGHUD::ClearAll(); }

}

HitCGHUD::Init();

Event::Attach( eventHitCG, HitCGHUD::Update );
Event::Attach( eventClientKilled, HitCGHUD::clientKilled );
Event::Attach( eventClientTeamKilled, HitCGHUD::clientKilled );
Event::Attach( eventClientSuicided, HitCGHUD::clientSuicide );