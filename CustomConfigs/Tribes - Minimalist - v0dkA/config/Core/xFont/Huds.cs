// xFont.dll Config
// Restart Tribes for changes to take effect

//=======================================================================================================
// Inline Font Tags <f:>
//=======================================================================================================
//
// Use inline font tag <f:> directly in your control text for SimGui::TextFormat based controls (incl. FearGuiFormattedText and CenterPrint)
//
//      Inline <f:> font tag allows you to:
//          1) Define in your script the font file to render
//          2) Change the color of the font and its transparency
//          3) Add a drop shadow at a custom color/transparency and offset
//
//      Refer to the Readme.txt for more information
//

//=======================================================================================================
// HUD Fonts
//=======================================================================================================

// FearGuiFormattedText Font Tags (<F0> - <F2>)
$xFont::FearGuiFormattedText::F0 = "sf_orange214_10.pft"; // (Default: sf_orange214_10.pft)
$xFont::FearGuiFormattedText::F1 = "sf_orange255_10.pft"; // (Default: sf_orange255_10.pft)
$xFont::FearGuiFormattedText::F2 = "sf_white_10.pft"; // (Default: sf_white_10.pft)

// CenterPrint Font Tags (<F0> - <F2>)
$xFont::CenterPrint::F0 = "sf_orange214_10.pft"; // (Default: sf_orange214_10.pft)
$xFont::CenterPrint::F1 = "sf_orange255_10.pft"; // (Default: sf_orange255_10.pft)
$xFont::CenterPrint::F2 = "sf_white_9b.pft"; // Default: sf_white_9b.pft)

//=======================================================================================================
// Miscellaneous
//=======================================================================================================

// Fix text alignment based on text control width rather than parent width
// Credit to Altimor for this fix
$xFont::FixTextAlignment = "False"; // (Default: False)
