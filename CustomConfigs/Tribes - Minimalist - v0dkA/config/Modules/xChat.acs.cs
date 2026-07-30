// xChat.dll Config

// Restart Tribes for changes to take effect

//=======================================================================================================
// Configuration variables
//=======================================================================================================

$xChat::HiderEnabled = "True"; // (Default: True; Will display messages for a specific period of time)
$xChat::HiderTimeout = "10"; // (Default: 10; Seconds to display each message)
$xChat::ScrollTimeout = "5"; // (Default: 5; Seconds that hider turns off when you scroll message history with PgUp/PgDn)
$xChat::ChatOnly = "False"; // (Default: False; Only display Global or Team chat messages. Scroll (PgUp/PgDn) to see all messages)
$xChat::HideCmdMsg = "True"; // (Default: True; Hide command messages at bottom of Chat Hud in playGui)
$xChat::TransChat = "False"; // (Default: True; Make the Chat Hud transparent)
$xChat::TransInput = "True"; // (Default: True; Make the Chat Input box transparent)
$xChat::IconsEnabled = "False"; // (Default: False; Display custom icons next to each message based on message type)

//=======================================================================================================
// Custom Fonts
//
// Place custom fonts in the [Tribes/base/fonts] folder and use the .pft filenames below
//=======================================================================================================

// Default Chat Fonts (msgType: 0-4)             msgType     Default
$xChat::SystemFont = "if_w_10b.pft";        //       0        if_w_10b.pft (White)
$xChat::GameFont = "sf_red_10b.pft";        //       1        sf_red_10b.pft (Red)
$xChat::MsgFont = "sf_yellow_10b.pft";      //       2        sf_yellow_10b.pft (Yellow)
$xChat::TeamMsgFont = "if_g_10b.pft";       //       3        if_g_10b.pft (Green)
$xChat::CommandFont = "sf_red_10b.pft";     //       4        sf_red_10b.pft (Red)

// Voice Chat Font
$xChat::VChatFont = "if_g_10b.pft";         //       -        if_g_10b.pft (Green)

//=======================================================================================================
// Chat Icons
//
// Place custom chat icons in [Tribes/base/huds] folder and use the .png filenames below
//
// Enable these by changing $xChat::IconsEnabled preference above
//=======================================================================================================

$xChat::SystemFont::Icon = "chat_white.png";
$xChat::GameFont::Icon = "chat_red.png";
$xChat::MsgFont::Icon = "chat_yellow.png";
$xChat::TeamMsgFont::Icon = "chat_green.png";
$xChat::CommandFont::Icon = "chat_red.png";

$xChat::Icon::xOffset = "1"; // (Default: 8; x offset from left of message text)
$xChat::Icon::yOffset = "5"; // (Default: 3; y offset from top of message text)
