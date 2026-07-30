function CTFHUD::Init() {
    if ( $CTFHUD::Loaded )
        return;
    $CTFHUD::Loaded = true;
    
    HUD::New("CTFHUD::Container", 0, 0, 200, 100, CTFHUD::Wake, CTFHUD::Sleep);

    newObject("CTFHUD::BG", FearGuiFormattedText, 0, 0, 173, 75);

    newObject("CTFHUD::Score0", FearGuiFormattedText, 64, 5, 18, 6);
    newObject("CTFHUD::Score1", FearGuiFormattedText, 64, 43, 18, 6);
    newObject("CTFHUD::ScoreTxt0", FearGuiFormattedText, 14, 5, 40, 6);
    newObject("CTFHUD::ScoreTxt1", FearGuiFormattedText, 14, 43, 40, 6);

    newObject("CTFHUD::Flag0", FearGuiFormattedText, 16, 18, 89, 6);
    newObject("CTFHUD::Flag1", FearGuiFormattedText, 16, 56, 89, 6);
    newObject("CTFHUD::FlagTxt0", FearGuiFormattedText, 14, 18, 28, 9);
    newObject("CTFHUD::FlagTxt1", FearGuiFormattedText, 14, 56, 28, 9);

    Hud::Add("CTFHUD::Container", "CTFHUD::BG");

    Hud::Add("CTFHUD::Container", "CTFHUD::Score0");
    Hud::Add("CTFHUD::Container", "CTFHUD::Score1");
    Hud::Add("CTFHUD::Container", "CTFHUD::ScoreTxt0");
    Hud::Add("CTFHUD::Container", "CTFHUD::ScoreTxt1");

    Hud::Add("CTFHUD::Container", "CTFHUD::Flag0");
    Hud::Add("CTFHUD::Container", "CTFHUD::Flag1");
    Hud::Add("CTFHUD::Container", "CTFHUD::FlagTxt0");
    Hud::Add("CTFHUD::Container", "CTFHUD::FlagTxt1");
    
    // xFont bitmap tint alpha: retain the authored colors while making the
    // near-opaque black score backdrop properly translucent over the world.
    Control::SetValue( "CTFHUD::BG", "<b3,3:Modules/CTFHud/CTFBG.png:ffffff80>");
    Control::SetValue( "CTFHUD::ScoreTxt0", "<f1>Score:");
    Control::SetValue( "CTFHUD::ScoreTxt1", "<f1>Score:");
    Control::SetValue( "CTFHUD::FlagTxt0", "<f1>Flag:");
    Control::SetValue( "CTFHUD::FlagTxt1", "<f1>Flag:");

    $CTFTimerDropID = xFont::NewTimer("CTFTimerDrop", 0, 255, 25, 0.05, 0);
    xFont::StartTimer("CTFTimerDrop");

    CTFHUD::Reset();
}

function CTFHUD::Wake() {
    CTFHUD::Update();
    FlagFlash(%team);
}
function CTFHUD::Sleep() { }

function CTFHUD::Reset() {
    CTFHUD::Update();
}

function CTFHUD::Update() {
        %score0 = Team::Score(Team::Friendly());
        %score1 = Team::Score(Team::Enemy());
    CTFHUD::FriendlyTeamValue(%team);
    CTFHUD::EnemyTeamValue(%team);
    Control::SetValue( "CTFHUD::Score0", "<f1>" ~%score0);
    Control::SetValue( "CTFHUD::Score1", "<f1>" ~%score1);

}


function CTFHUD::FriendlyTeamValue(%team, %score0) {
        %team = Team::Friendly();
    %loc = Team::Flag::Location(Team::Friendly());
    FlagFlash(%team);
    
    switch ( %loc ) {
        case "home":
            %loc = "<f:CTFHud-Font.pft>:::::: Home ::::::";
            break;
        case "field":
            %loc = $FlagDropFontF ~ Team::Flag::Timer(%team);
            break;
        default:
            %loc = "<f:CTFHud-Font.pft:d20808>" ~ String::escapeFormatting(Client::GetName(%loc));
            break;
    }

    Control::SetValue( "CTFHUD::flag0", "<jc><f1>" ~ %loc );

}

function CTFHUD::EnemyTeamValue(%team, %score1) {
        %team = Team::Enemy();
    %loc = Team::Flag::Location(Team::Enemy());
    FlagFlash(%team);
    
    switch ( %loc ) {
        case "home":
            %loc = "<f:CTFHud-Font.pft>:::::: Home ::::::";
            break;
        case "field":
            %loc = $FlagDropFontE ~ Team::Flag::Timer(%team);
            break;
        default:
            %loc = "<f:CTFHud-Font.pft:3ffd04>" ~ String::escapeFormatting(Client::GetName(%loc));
            break;
    }

    Control::SetValue( "CTFHUD::flag1", "<jc>" ~ %loc );

}

function FlagFlash(%team) {
if (Team::Flag::Timer(%team) < 10) {
    $FlagDropFontE = "<f:CTFHud-Font.pft:ff0000"~$CTFTimerDropID~":ffffff>";
    $FlagDropFontF = "<f:CTFHud-Font.pft:3ffd04"~$CTFTimerDropID~":ffffff>";
  } else {
    $FlagDropFontE = "<f:CTFHud-Font.pft:FFFFFF>";
    $FlagDropFontF = "<f:CTFHud-Font.pft:FFFFFF>";
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
