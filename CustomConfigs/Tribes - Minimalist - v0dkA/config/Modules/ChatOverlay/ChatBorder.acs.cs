function ChatOverlay::Init() {
    if($ChatOverlay:Loaded)
        return;
    $ChatOverlay:Loaded = true;
    
    HUD::New( "ChatOverlay::Container", 0, 0, 664, 89, ChatOverlay::Wake, ChatOverlay::Sleep );
    newObject("ChatOverlay::Texture", FearGuiFormattedText, 0, 0, 664, 89);
    HUD::Add("ChatOverlay::Container","ChatOverlay::Texture");
    Control::SetValue("ChatOverlay::Texture", "<B0,0:Modules/ChatOverlay/ChatBG.png>");
}

function ChatOverlay::Wake() {
    // TAB can wake PlayGui again and the HUD manager can restore an old saved
    // overlay position.  The frame belongs to chatDisplayHud, so immediately
    // dock it again and keep following while K-dragging is active.
    Schedule::Cancel("ChatOverlay::Update();");
    ChatOverlay::Update();
}

function ChatOverlay::Sleep() {
    Schedule::Cancel("ChatOverlay::Update();");
}

function ChatOverlay::Update() {
    %pos = Control::getPosition("chatDisplayHud");
    if(%pos != "") {
        %x = getWord(%pos, 0) - 7;
        // ChatBG's 89px frame surrounds the xFont chat's four-line 78px
        // extent with five pixels above and six below.
        %y = getWord(%pos, 1) - 5;
        Control::setPosition("ChatOverlay::Container", %x, %y);
    }
    Schedule::Add("ChatOverlay::Update();", 0.1);
}

ChatOverlay::Init();
