//============================================================================
// CustomChatHUD.cs - Custom Chat Display with Single-Message Scrolling
// Replaces the default chatDisplayHud with a fully script-controlled HUD
// Requires PrestoPack (same as rpghud.cs)
//============================================================================

// Configuration
$ChatHUD::Enabled = true;       // Set to true to use custom HUD
$ChatHUD::MaxLines = 15;        // Number of visible lines
$ChatHUD::BufferSize = 100;     // Message history size
$ChatHUD::ScrollOffset = 0;     // 0 = bottom (newest messages)
$ChatHUD::RefreshRate = 0.1;    // HUD refresh interval in seconds

// Initialize the message buffer
for(%i = 0; %i < $ChatHUD::BufferSize; %i++)
    $ChatHUD::Line[%i] = "";

//----------------------------------------------------------------------------
// HUD Update Function - Called periodically by the HUD system
//----------------------------------------------------------------------------
function UpdateChatHUD(%hud)
{
    // Calculate which lines to display based on scroll offset
    // ScrollOffset 0 = show newest messages (bottom of buffer)
    // ScrollOffset N = show N messages older
    
    for(%i = 0; %i < $ChatHUD::MaxLines; %i++)
    {
        %lineIdx = $ChatHUD::ScrollOffset + ($ChatHUD::MaxLines - 1 - %i);
        %text = $ChatHUD::Line[%lineIdx];
        
        if(%text != "")
            HUD::AddTextLine(%hud, %text);
        else
            HUD::AddTextLine(%hud, "");
    }
    
    return $ChatHUD::RefreshRate;
}

//----------------------------------------------------------------------------
// Add a new message to the buffer
//----------------------------------------------------------------------------
function ChatHUD::AddLine(%text)
{
    // Shift all messages down (oldest falls off the end)
    for(%i = $ChatHUD::BufferSize - 1; %i > 0; %i--)
        $ChatHUD::Line[%i] = $ChatHUD::Line[%i - 1];
    
    // Insert new message at position 0 (newest)
    $ChatHUD::Line[0] = %text;
    
    // Reset scroll to bottom when new message arrives
    $ChatHUD::ScrollOffset = 0;
}

//----------------------------------------------------------------------------
// Scroll Functions - Move 1 message at a time
//----------------------------------------------------------------------------
function ChatHUD::ScrollUp()
{
    // Calculate max scroll offset (don't scroll past oldest message)
    %maxOffset = $ChatHUD::BufferSize - $ChatHUD::MaxLines;
    
    if($ChatHUD::ScrollOffset < %maxOffset)
        $ChatHUD::ScrollOffset++;
}

function ChatHUD::ScrollDown()
{
    if($ChatHUD::ScrollOffset > 0)
        $ChatHUD::ScrollOffset--;
}

//----------------------------------------------------------------------------
// Toggle visibility of custom chat HUD
//----------------------------------------------------------------------------
function ChatHUD::Toggle()
{
    HUD::ToggleDisplay(ChatHUD);
}

//----------------------------------------------------------------------------
// Remote handler for server messages
// Server calls: remoteEval(%cl, "ChatLine", %text);
//----------------------------------------------------------------------------
function remoteChatLine(%server, %text)
{
    // Only accept messages from the server (2048)
    if(%server != 2048)
        return;
    
    ChatHUD::AddLine(%text);
}

//----------------------------------------------------------------------------
// Create and display the HUD
// Position: bottom-left area (similar to default chat)
// Parameters: HUD::New(name, callback, x%, y%, width, height)
//----------------------------------------------------------------------------
HUD::New(ChatHUD, UpdateChatHUD, "1%", "55%", 400, 200);
HUD::Display(ChatHUD);

// Hide the default chat display
Control::setVisible("chatDisplayHud", false);

//----------------------------------------------------------------------------
// Keybinds for scrolling (override default PageUp/PageDown)
//----------------------------------------------------------------------------
EditActionMap("playMap.sae");
bindCommand(keyboard0, make, "prior", TO, "ChatHUD::ScrollUp();");
bindCommand(keyboard0, make, "next", TO, "ChatHUD::ScrollDown();");

echo("CustomChatHUD loaded - Single-message scrolling enabled");
