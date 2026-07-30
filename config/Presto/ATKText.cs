//----------------------------------------------------------------------------
// ATKText - Floating damage display for Presto pack
// Ported from repackmsghud.cs by phantom (tribesrpg.org)
//----------------------------------------------------------------------------

// Animation style: "float", "redmoon", "wow", "test", or "pop"
$animationStyle = "redmoon";
$maxfloatmsg = 10;

// Initialize ATKTextInUse array
for(%i = 1; %i < $maxfloatmsg; %i++)
	$ATKTextInUse[%i] = "";

// Character width table for text width calculation
$charWidth[32] = 5; $charWidth[33] = 2; $charWidth[34] = 3; $charWidth[35] = 9; $charWidth[36] = 7;
$charWidth[37] = 12; $charWidth[38] = 9; $charWidth[39] = 1; $charWidth[40] = 4; $charWidth[41] = 4;
$charWidth[42] = 5; $charWidth[43] = 7; $charWidth[44] = 3; $charWidth[45] = 3; $charWidth[46] = 2;
$charWidth[47] = 4; $charWidth[48] = 6; $charWidth[49] = 2; $charWidth[50] = 6; $charWidth[51] = 6;
$charWidth[52] = 6; $charWidth[53] = 6; $charWidth[54] = 6; $charWidth[55] = 6; $charWidth[56] = 6;
$charWidth[57] = 6; $charWidth[58] = 2; $charWidth[59] = 3; $charWidth[60] = 7; $charWidth[61] = 8;
$charWidth[62] = 7; $charWidth[63] = 4; $charWidth[64] = 12; $charWidth[65] = 10; $charWidth[66] = 7;
$charWidth[67] = 8; $charWidth[68] = 9; $charWidth[69] = 6; $charWidth[70] = 6; $charWidth[71] = 9;
$charWidth[72] = 8; $charWidth[73] = 2; $charWidth[74] = 4; $charWidth[75] = 9; $charWidth[76] = 6;
$charWidth[77] = 10; $charWidth[78] = 9; $charWidth[79] = 10; $charWidth[80] = 7; $charWidth[81] = 10;
$charWidth[82] = 8; $charWidth[83] = 7; $charWidth[84] = 8; $charWidth[85] = 8; $charWidth[86] = 9;
$charWidth[87] = 13; $charWidth[88] = 10; $charWidth[89] = 10; $charWidth[90] = 9; $charWidth[91] = 4;
$charWidth[92] = 4; $charWidth[93] = 4; $charWidth[94] = 8; $charWidth[95] = 7; $charWidth[96] = 3;
$charWidth[97] = 6; $charWidth[98] = 6; $charWidth[99] = 5; $charWidth[100] = 6; $charWidth[101] = 6;
$charWidth[102] = 5; $charWidth[103] = 7; $charWidth[104] = 6; $charWidth[105] = 2; $charWidth[106] = 3;
$charWidth[107] = 7; $charWidth[108] = 2; $charWidth[109] = 10; $charWidth[110] = 6; $charWidth[111] = 7;
$charWidth[112] = 6; $charWidth[113] = 6; $charWidth[114] = 5; $charWidth[115] = 5; $charWidth[116] = 5;
$charWidth[117] = 7; $charWidth[118] = 7; $charWidth[119] = 10; $charWidth[120] = 7; $charWidth[121] = 7;
$charWidth[122] = 6; $charWidth[123] = 5; $charWidth[124] = 1; $charWidth[125] = 5; $charWidth[126] = 8;

// Helper function to get character width
function atktext::charwidth(%v)
{
	%c = charCode(%v);
	if($charWidth[%c] != "")
		return $charWidth[%c] + 1;
	return 5; // Default fallback
}

// Helper function to get character code
function charCode(%char)
{
	for(%i = 32; %i < 127; %i++)
	{
		if(String::Compare(%char, $char[%i]) == 0)
			return %i;
	}
	return 124; // Default fallback
}

// Helper function to remove text between two delimiters (e.g., remove HTML tags)
function String::removeBetween(%string, %startDelim, %endDelim)
{
	%startPos = String::findSubStr(%string, %startDelim);
	if(%startPos == -1)
		return %string; // Start delimiter not found, return original
	
	%endPos = String::findSubStr(%string, %endDelim, %startPos + 1);
	if(%endPos == -1)
		return %string; // End delimiter not found, return original
	
	// Get text before start delimiter
	%before = String::getSubStr(%string, 0, %startPos);
	// Get text after end delimiter
	%after = String::getSubStr(%string, %endPos + 1, 99999);
	
	return %before @ %after;
}

// Random offset table for test animation
$randOffset[1] = 50;
$randOffset[2] = 25;
$randOffset[3] = 0;
$randOffset[4] = -25;
$randOffset[5] = -50;
$randOffset[6] = -75;
$randOffset[7] = -100;
$offsetId = 7; // Start from 7, go to 1

//----------------------------------------------------------------------------
// Main remote function - called from server via remoteEval
//----------------------------------------------------------------------------
// remoteEval(%clientId, "ATKText", %text, %animationStyle, %viewType);
// %viewType: "attacker" = player dealing damage (red), "defender" = player taking damage (current color), "spectator" = other players' damage (different position)
function remoteATKText(%server, %text, %animationStyle, %viewType) {
	if(%server != 2048) return;
	if(%text == "") return;

	// Handle backward compatibility: if %animationStyle is not a valid style, it might be the old %hit parameter
	if(%animationStyle != "float" && %animationStyle != "redmoon" && %animationStyle != "wow" && %animationStyle != "test" && %animationStyle != "pop" && %animationStyle != "nameplate")
	{
		// Old format - %animationStyle might be a boolean or missing
		if(%animationStyle == "" || %animationStyle == -1)
		{
			%animationStyle = $animationStyle; // Use default
			%viewType = "defender"; // Default to defender view for old format
		}
		else if(%animationStyle == true || %animationStyle == 1 || %animationStyle == "1")
		{
			%animationStyle = $animationStyle;
			%viewType = "attacker";
		}
		else
		{
			%animationStyle = $animationStyle;
			%viewType = "defender";
		}
	}
	
	// Default view type if not provided
	if(%viewType == "" || %viewType == -1)
		%viewType = "defender";

	atktext::set(%text, %animationStyle, %viewType);
}

//----------------------------------------------------------------------------
// Set up and display ATKText
//----------------------------------------------------------------------------
function atktext::set(%text, %animationStyle, %viewType) {
	if(%text == "") return;
	
	// Default view type if not provided
	if(%viewType == "" || %viewType == -1)
		%viewType = "defender";
	
	// Get screen resolution - try Presto's ScreenSize, fallback to default
	// Note: In TorqueScript, calling a non-existent function typically returns empty string or -1
	%res = Presto::ScreenSize();
	if(%res == "" || %res == -1)
		%res = "640 480"; // Fallback to default resolution
	%x = getWord(%res, 0);
	%y = getWord(%res, 1);
	
	// Position based on view type
	if(%viewType == "spectator")
	{
		// Spectator view (other players' damage) - position to the right side
		%centery = %y * 0.5;
		%centery -= 20; // Position slightly above center
		%centerx = %x * 0.75; // Position on right side of screen (75% from left)
	}
	else
	{
		// Attacker or defender view (player's own damage) - center of screen
		%centery = %y * 0.5;
		%centery -= 20; // Position slightly above center
		%centerx = %x * 0.5; // Always center horizontally (no random offset)
	}
	
	// Change color based on view type
	// Remove justify tags but preserve any existing font color tags from critical hits
	%textWithoutJustify = %text;
	%textWithoutJustify = String::replace(%textWithoutJustify, "<jc>", "");
	%textWithoutJustify = String::replace(%textWithoutJustify, "<jr>", "");
	%textWithoutJustify = String::replace(%textWithoutJustify, "<jl>", "");
	
	if(%viewType == "attacker")
	{
		// Player dealing damage - use orangeish yellow/beige color (since no red is available)
		// <f2> = orangeish yellow/beige (used for critical hits and values)
		// <f0> = white (default)
		// <f1> = beige (used for weapon names)
		// For critical hits: "<f2>Critical hit! <f0>123 DMG!" -> "<f2>Critical hit! <f2>123 DMG!" (all beige)
		// For normal hits: "123 DMG!" -> "<f2>123 DMG!" (beige)
		%text = %textWithoutJustify;
		
		// First, handle critical hits specifically
		if(String::findSubStr(%text, "<f2>Critical hit! <f0>") != -1)
		{
			// Critical hit - replace <f0> after "Critical hit!" with <f2> for beige damage
			%text = String::replace(%text, "<f2>Critical hit! <f0>", "<f2>Critical hit! <f2>");
		}
		// Then handle any remaining <f0> tags (for non-critical hits or if pattern didn't match)
		if(String::findSubStr(%text, "<f0>") != -1)
		{
			// Replace all <f0> with <f2> for beige/orange
			%text = String::replace(%text, "<f0>", "<f2>");
		}
		// If no color tag at all, add <f2> at the start for beige/orange
		if(String::findSubStr(%text, "<f") == -1)
		{
			%text = "<f2>" @ %text;
		}
	}
	else if(%viewType == "defender")
	{
		// Player taking damage - keep default color (white/yellow) - use <f0>
		// Defender keeps original colors from server (critical hits already have <f1>)
		%text = %textWithoutJustify;
		// If no color tag at all, add <f0>
		if(String::findSubStr(%text, "<f") == -1)
			%text = "<f0>" @ %text;
	}
	else if(%viewType == "spectator")
	{
		// Other players' damage - use white color (same as defender)
		%text = %textWithoutJustify;
		if(String::findSubStr(%text, "<f") == -1)
			%text = "<f0>" @ %text;
	}
	else
	{
		// Default to current color if view type is unknown
		%text = %textWithoutJustify;
		if(String::findSubStr(%text, "<f") == -1)
			%text = "<f0>" @ %text;
	}
	%bgheight = 18;
	%trimmed = %text;
	%ind1 = String::findSubStr(%text, "\n");

	if(%ind1 > 0) {
		%trimmed = String::getSubStr(%text, 0, %ind1);
		%bgheight *= 2;
	}

	// Remove font tags for width calculation
	%xtr = String::removeBetween(%trimmed, "<", ">");
	while(%xtr != %trimmed) {
		%trimmed = %xtr;
		%xtr = String::removeBetween(%trimmed, "<", ">");
	}
	
	// Calculate text width
	%textw = 0;
	for(%i = 0; (%v = String::getSubStr(%trimmed, %i, 1)) != ""; %i++)
		%textw += atktext::charwidth(%v);

	%bgwidth = %textw + 4;
	%posy = %centery;
	%bgposx = %centerx - (%bgwidth * 0.5) - 1;
	%titleposx = %centerx - (%textw * 0.5);

	// Find available slot (protection from too many pop-ups)
	for(%num = 1; %num < $maxfloatmsg; %num++) {
		if($ATKTextInUse[%num] == "") {
			$ATKTextInUse[%num] = true;
			break;
		}
	}

	// If all slots are in use, return
	if($ATKTextInUse[%num] == "")
		return;

	// Create GUI object
	%hudname = "atktexthud_" @ %num;
	%object = newObject(%hudname, FearGuiFormattedText, %titleposx, %posy, 15, 100);
	addToSet(PlayGui, %object);
	Control::setValue(%hudname, %text);

	// Run animation based on style
	if(%animationStyle == "float")
		floatAnimation(%num, %hudname, %titleposx, %posy);
	else if(%animationStyle == "wow")
		wowAnimation(%num, %hudname, %titleposx, %posy);
	else if(%animationStyle == "redmoon")
		redMoonAnimation(%num, %hudname, %centerx, %posy);
	else if(%animationStyle == "test")
		testAnimation(%text, %num, %hudname, %centerx, %posy);
	else if(%animationStyle == "pop" || %animationStyle == "nameplate")
		popAnimation(%text, %num, %hudname, %titleposx, %posy, %viewType);
	else
		redMoonAnimation(%num, %hudname, %centerx, %posy); // Default to redmoon
}

//----------------------------------------------------------------------------
// Animation Styles
//----------------------------------------------------------------------------

// Float animation - simple upward float
function floatAnimation(%num, %hudname, %posx, %posy) {
	for(%i = 1; %i <= 60; %i++) {
		%newY = %posy + (5 * -%i);
		Schedule::Add("Control::setPosition(\"" @ %hudname @ "\", " @ %posx @ ", " @ %newY @ ");", %i / 30);
	}
	Schedule::Add("ClearATKText(" @ %num @ ");", 2, %hudname);
}

// Original RedMoon animation style - bounce up and down
function redMoonAnimation(%num, %hudname, %posx, %posy) {
	Control::setPosition(%hudname, %posx, %posy);
	%x = %posx;
	%y = %posy;
	for(%i = 1; %i <= 10; %i++) {
		%newY = %y + (5 * -%i);
		schedule("Control::setPosition(\"" @ %hudname @ "\", " @ %x @ ", " @ %newY @ ");", %i / 30);
	}
	%cnt = 0;
	for(%i = 9; %i >= 1; %i--) {
		%newY = %y + (5 * -%i);
		schedule("Control::setPosition(\"" @ %hudname @ "\", " @ %x @ ", " @ %newY @ ");", (%cnt++ / 20) + 0.5);
	}
	schedule("Control::setPosition(\"" @ %hudname @ "\", " @ %posx @ ", " @ %posy @ ");", (%cnt++ / 20) + 0.5);
	Schedule::Add("ClearATKText(" @ %num @ ");", 2.5, %hudname);
}

// WoW-like animation - smooth upward float
function wowAnimation(%num, %hudname, %posx, %posy) {
	for(%i = 1; %i <= 30; %i++) {
		%newY = %posy + (2 * -%i);
		Schedule::Add("Control::setPosition(\"" @ %hudname @ "\", " @ %posx @ ", " @ %newY @ ");", %i / 30);
	}
	Schedule::Add("ClearATKText(" @ %num @ ");", 1, %hudname);
}

// Test animation - improved WoW-like with better positioning
function testAnimation(%text, %num, %hudname, %posx, %posy) {
	%randX = floor((getRandom() * 3) + 1);
	%randY = floor((getRandom() * 3) + 1);
	%offsetX = floor((getRandom() * 6) - 2) * 50;
	%offsetY = $randOffset[$offsetId];
	
	%newX = %posx + %offsetX;
	%newY = %posy + %offsetY;
	Control::setPosition(%hudname, %newX, %newY);
	
	if($offsetId > 1) {
		$offsetId = $offsetId - 1;
	}
	
	for(%i = 1; %i <= 60; %i++) {
		%newY = %posy + %offsetY + (2 * -%i);
		Schedule::Add("Control::setPosition(\"" @ %hudname @ "\", " @ %posx @ ", " @ %newY @ ");", %i / 30);
	}

	Schedule::Add("ClearATKText(" @ %num @ ");", 2, %hudname);
}

// Pop animation keyframes - fast rise with overshoot, then settle (ease-out punch)
$popRise[1] = -9;  $popRise[2] = -17; $popRise[3] = -24; $popRise[4] = -29;
$popRise[5] = -33; $popRise[6] = -36; $popRise[7] = -35; $popRise[8] = -32;
$popRise[9] = -30;

// Pop animation - punch in with overshoot, hold, then accelerate away while dimming.
// Vertical zones keep the streams separate: damage you deal pops above the
// crosshair and rises; damage you take pops below the crosshair and falls.
// Each active message slot also gets its own lane (alternating left/right,
// stepping away from center) so simultaneous hits fan out instead of stacking.
function popAnimation(%text, %num, %hudname, %posx, %posy, %viewType) {
	%vdir = 1; // 1 = travels up, -1 = travels down
	if(%viewType == "defender") {
		%vdir = -1;
		%posy += 70;
	}
	else if(%viewType == "attacker")
		%posy -= 30;

	// Lane offsets: slot 1 = center, 2 = left, 3 = right, 4 = far left, ...
	%lane = floor(%num / 2);
	if(%num - (%lane * 2) == 0)
		%dir = -1;
	else
		%dir = 1;
	%baseX = %posx + (%dir * %lane * 28);
	%baseY = %posy - (%vdir * %lane * 10);

	Control::setPosition(%hudname, %baseX, %baseY);

	// Phase 1: punch in with overshoot (~0.3s)
	for(%i = 1; %i <= 9; %i++) {
		%newY = %baseY + (%vdir * $popRise[%i]);
		Schedule::Add("Control::setPosition(\"" @ %hudname @ "\", " @ %baseX @ ", " @ %newY @ ");", %i / 30);
	}

	// Phase 2: hold at the settle point until 0.8s, then drift away with ease-in (~0.8s)
	for(%i = 1; %i <= 16; %i++) {
		%p = %i / 16;
		%newY = %baseY + (%vdir * (-30 - floor(55 * %p * %p)));
		%newX = %baseX + floor(%dir * 20 * %p * %p);
		Schedule::Add("Control::setPosition(\"" @ %hudname @ "\", " @ %newX @ ", " @ %newY @ ");", 0.8 + (%i * 0.05));
	}

	// Dim the text partway through the drift to fake a fade-out
	%dimText = %text;
	%dimText = String::replace(%dimText, "<f0>", "<f1>");
	%dimText = String::replace(%dimText, "<f2>", "<f1>");
	if(%dimText != %text && String::findSubStr(%text, "\n") == -1)
		Schedule::Add("Control::setValue(\"" @ %hudname @ "\", \"" @ %dimText @ "\");", 1.2);

	Schedule::Add("ClearATKText(" @ %num @ ");", 1.7, %hudname);
}

//----------------------------------------------------------------------------
// Cleanup Functions
//----------------------------------------------------------------------------

// Clear specific ATKText by number
function ClearATKText(%num) {
	$ATKTextInUse[%num] = "";
	atktext::clear("atktexthud_" @ %num);
	if($offsetId < 7) {
		$offsetId = $offsetId + 1;
	}
}

// Clear all ATKText HUDs (for cleanup/reset)
function atktext::reset() {
	%ngs = nameToId(NamedGuiSet);
	%len = Group::objectCount(%ngs);
	%list = "";
	for(%i = 0; %i < %len; %i++) {
		%obj = Group::getObject(%ngs, %i);
		%objectName = Object::getName(%obj);
		if(String::FindSubStr(%objectName, "atktexthud_") != -1)
			%list = %list @ %obj @ " ";
	}
	for(%i = 0; (%g = GetWord(%list, %i)) != -1; %i++)
		deleteObject(%g);
	
	// Reset in-use flags
	for(%i = 1; %i < $maxfloatmsg; %i++)
		$ATKTextInUse[%i] = "";
	$offsetId = 7;
}

// Clear specific ATKText HUD by name
function atktext::clear(%name) {
	%ngs = nameToId(NamedGuiSet);
	%len = Group::objectCount(%ngs);
	%list = "";
	for(%i = 0; %i < %len; %i++) {
		%obj = Group::getObject(%ngs, %i);
		%objectName = Object::getName(%obj);
		if(String::FindSubStr(%objectName, %name) != -1)
			%list = %list @ %obj @ " ";
	}
	for(%i = 0; (%g = GetWord(%list, %i)) != -1; %i++)
		deleteObject(%g);
}

