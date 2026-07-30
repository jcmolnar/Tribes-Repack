    $CTFTimerDrop = xFont::NewTimer("CTFTimerDrop", 0, 255, 25, 0.05, 0);
    xFont::StartTimer("CTFTimerDrop");

function CTFHUD::Init() {
    if ( $CTFHUD::Loaded )
        return;
    $CTFHUD::Loaded = true;
    
    HUD::New("CTFHUD::Container", 0, 0, 500, 72, CTFHUD::Wake, CTFHUD::Sleep);

    newObject("CTFHUD::Image0", FearGuiFormattedText, 0, 1, 24, 24); 
    newObject("CTFHUD::Image1", FearGuiFormattedText, 0, 25, 24, 24);

    newObject("CTFHUD::MainStatus0", FearGuiFormattedText, 55, 8, 340, 20); 
    newObject("CTFHUD::Status0BG1", FearGuiFormattedText, 55, 8, 340, 20); 

    newObject("CTFHUD::MainStatus1", FearGuiFormattedText, 55, 32, 340, 20);
    newObject("CTFHUD::Status1BG1", FearGuiFormattedText, 55, 32, 340, 20);

    newObject("CTFHUD::Score0", FearGuiFormattedText, 33, 8, 35, 30);

    newObject("CTFHUD::Score1", FearGuiFormattedText, 33, 32, 35, 30);

    Hud::Add("CTFHUD::Container", "CTFHUD::Image0");
    Hud::Add("CTFHUD::Container", "CTFHUD::Image1");
    
    Hud::Add("CTFHUD::Container", "CTFHUD::MainStatus0");
    Hud::Add("CTFHUD::Container", "CTFHUD::Status0BG1");

    Hud::Add("CTFHUD::Container", "CTFHUD::MainStatus1");
    Hud::Add("CTFHUD::Container", "CTFHUD::Status1BG1");

    Hud::Add("CTFHUD::Container", "CTFHUD::Score0");

    Hud::Add("CTFHUD::Container", "CTFHUD::Score1");

    $FlagDropFont = "<f:small-black-stroke.pft:FFFFFFFF:000000ff:1,1>";

    CTFHUD::Reset();
}

function CTFHUD::Wake() {
    CTFHUD::Update();
    FlagFlash(%team);
}
function CTFHUD::Sleep() { }

function CTFHUD::Reset() {
    Control::SetValue("CTFHUD::Image0", "<b3,3:Modules/CTFHud/friendly.home.png>");
    Control::SetValue("CTFHUD::Image1", "<b3,4:Modules/CTFHud/enemy.home.png>");

    CTFHUD::Update();
}

function CTFHUD::Update() {
        %score0 = Team::Score(Team::Friendly());
        %score1 = Team::Score(Team::Enemy());
    CTFHUD::FriendlyTeamValue(%team);
    CTFHUD::EnemyTeamValue(%team);
    Control::SetValue( "CTFHUD::Score0", "<f:small-black-stroke.pft:228b01FF:000000ff:1,1>" ~%score0);

    Control::SetValue( "CTFHUD::Score1", "<f:small-black-stroke.pft:ff0000FF:000000ff:1,1>" ~%score1);

}


function CTFHUD::FriendlyTeamValue(%team, %score0) {
        %team = Team::Friendly();
    %loc = Team::Flag::Location(Team::Friendly());

    FlagFlash(%team);
    
    switch ( %loc ) {
        case "home":
            %loc = "<f:small-black-stroke.pft:FFFFFFFF:000000ff:1,1>Home";
            %bmp = "flag-icon.png:00ff02ff";
            break;
        case "field":
            %loc = $FlagDropFont ~ Team::Flag::Timer(%team);
            %bmp = $FlagDropIcon0;
            break;
        default:
            %loc = String::escapeFormatting(Client::GetName(%loc));
            %bmp = "flag-icon.png:fdff00ff";
            break;
    }
    Control::SetValue( "CTFHUD::MainStatus0", "<f:font-nostroke.pft:807b00ff:807b00ff:1,1>" ~ %loc );
    Control::SetValue( "CTFHUD::Status0BG1", "<f:font-stroke.pft:fff500ff>" ~ %loc );

    Control::SetValue( "CTFHUD::Image0", "<b3,3:Modules/CTFHud/"~%bmp~">" );

}

function CTFHUD::EnemyTeamValue(%team, %score1) {
        %team = Team::Enemy();
    %loc = Team::Flag::Location(Team::Enemy());

    FlagFlash(%team);
    
    switch ( %loc ) {
        case "home":
            %loc = "<f:small-black-stroke.pft:FFFFFFFF:000000ff:1,1>Home";
            %bmp = "flag-icon.png:ff0000ff";
            break;
        case "field":
            %loc = $FlagDropFont ~ Team::Flag::Timer(%team);
            %bmp = $FlagDropIcon1;
            break;
        default:
            %loc = String::escapeFormatting(Client::GetName(%loc));
            %bmp = "flag-icon.png:00d2ffff";
            break;
    }
    Control::SetValue( "CTFHUD::MainStatus1", "<f:font-nostroke.pft:006880ff:006880ff:1,1>" ~ %loc );
    Control::SetValue( "CTFHUD::Status1BG1", "<f:font-stroke.pft:00cfff>" ~ %loc );
    
    Control::SetValue( "CTFHUD::Image1", "<b3,3:Modules/CTFHud/"~%bmp~">" );

}

function FlagFlash(%team) {
if (Team::Flag::Timer(%team) < 10) {
    $FlagDropIcon0 = "flag-icon.png:00ff02"~$CTFTimerDrop~"";
    $FlagDropIcon1 = "flag-icon.png:ff0000"~$CTFTimerDrop~"";
  } else {
    $FlagDropIcon0 = "flag-icon.png:ffffff";
    $FlagDropIcon1 = "flag-icon.png:ffffff";
  }
}



// if we change teams, the sides may need to be updated
function CTFHUD::SelfUpdate( %client, %team ) {
    if ( %client == getManagerId() )
        CTFHUD::Update();
}

Event::Attach( EventFlagUpdate, CTFHUD::Update );
Event::Attach( EventFlagTimerUpdate, CTFHUD::Update );
Event::Attach( EventClientChangeTeam, CTFHUD::SelfUpdate );

CTFHUD::Init();