// simple ammo hud for 1.40 - only shows the current mounted weapons ammo

//Event::Attach(eventConnected, GAmmo::Init);

function GAmmo::Init() {
    if($GAmmo::Loaded)
        return;
    $GAmmo::Loaded = true;

    HUD::New( "GAmmo::Container", 0, 0, 100, 50, GAmmo::Wake, GAmmo::Sleep );

    newObject("GAmmo::Count", FearGuiFormattedText, 0, 0, 100, 50);
    
    HUD::Add("GAmmo::Container","GAmmo::Count");

}

function GAmmo::Wake() { GAmmo::Update(); }
function GAmmo::Sleep() { Schedule::Cancel("GAmmo::Update();"); }

function GAmmo::Update() {
    if($Weapon::Ammo < 1) {
        %display = " ";
    } else {
        %display = $Weapon::Ammo;
    }
        
    if($health == 0 || $playingdemo || Client::GetTeam(getManagerId()) == -1) {
        %display = "";
    }

    //%ammoCount = ($Weapon::Ammo < 0 ? "~" : %display);
        
    Control::SetValue("GAmmo::Count", "<jc><f:white-default.pft:00cfffff:006880c8:2,2>" @ %display);
    Schedule::Add("GAmmo::Update();",0.1);
}

GAmmo::Init();