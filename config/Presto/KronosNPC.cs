//==============================================
// KronosNPC.cs - modern window over the EXISTING NPC dialogue
//==============================================
// Does NOT invent dialogue. The server (KronosNPC_Server.cs + the
// AI::sayLater hook) mirrors the bot's real spoken lines here via KNPCLine,
// and we present them in a window with the bracketed [keyword] options the
// NPC offers turned into buttons. Clicking a button just sends "#say
// keyword" (remoteEval 2048 say) - so the entire existing dialogue logic
// (quest hand-ins, enemy spawns, teleporters, ...) runs unchanged, exactly
// as if the player typed it. Opened only for HUD clients.
//
// Drawn through the shared onPostDraw hook (KronosShop); draggable by its
// title bar via KronosMenu's drag system (id "knpcwin").
//==============================================

if($pref::Kronos::npcX == "")  $pref::Kronos::npcX = "c";    // "c" = centered until dragged
if($pref::Kronos::npcY == "")  $pref::Kronos::npcY = 0.28;
if($pref::Kronos::npcW == "")  $pref::Kronos::npcW = 0.44;   // width, fraction of (scaled) screen

$KNPC::MaxOpt = 8;
$KNPC::MaxLines = 8;     // recent NPC lines kept

// ============================================
// Server pushes
// ============================================

// Open: clear the window and greet through the real #say path so the bot's
// actual greeting/state machine fires.
function remoteKNPCBegin(%server, %name)
{
	if(%server != 2048)
		return;
	$KNPC::name = %name;
	$KNPC::open = true;
	$KNPC::lineN = 0;
	$KNPC::optCount = 0;
	$KNPC::wlDirty = true;
	remoteEval(2048, say, 0, "#say hi");
}

// One spoken line from the NPC (forwarded by the AI::sayLater hook)
function remoteKNPCLine(%server, %text)
{
	if(%server != 2048)
		return;
	if($KNPC::open == "")
		return;
	KronosNPC::pushLine(%text);
	$KNPC::wlDirty = true;
}

// Clickable options, parsed + lowercased server-side (KronosNPC_Server.cs).
// Space-separated single-word keywords; replaces the current button set.
function remoteKNPCOpts(%server, %list)
{
	if(%server != 2048)
		return;
	if($KNPC::open == "")
		return;
	%n = 0;
	for(%i = 0; (%kw = getWord(%list, %i)) != -1; %i++)
	{
		if(%n >= $KNPC::MaxOpt)
			break;
		$KNPC::opt[%n] = %kw;
		%n++;
	}
	$KNPC::optCount = %n;
}

function remoteKNPCClose(%server)
{
	if(%server != 2048)
		return;
	$KNPC::open = "";
	if($KNPC::keyCap)   // drop our Esc/Tab key capture if the server closes us
	{
		glTextInput(0);
		$KNPC::keyCap = false;
	}
}

// Close the dialogue (Goodbye / Esc / Tab) - tells the server and drops key capture.
function KronosNPC::close()
{
	$KNPC::open = "";
	remoteEval(2048, KNPCClose);
	if($KNPC::keyCap)
	{
		glTextInput(0);
		$KNPC::keyCap = false;
	}
}

// Per-frame key pump: while the dialogue is open AND the cursor is up (so the player
// is interacting), capture keys via the glTextInput seam (kronos_textinput.dll) so
// Esc / Tab close the window like the other Kronos UI menus. We only grab keys once
// the cursor is up, so we never eat the TAB used to raise the cursor in the first
// place. Called every frame from the onPostDraw chain.
function KronosNPC::pump()
{
	if($KNPC::open == "")
	{
		if($KNPC::keyCap)
		{
			glTextInput(0);
			$KNPC::keyCap = false;
		}
		return;
	}
	// A focused text field (chat composer / bank amount) owns the key seam -
	// defer to it instead of eating its keystrokes. KronosInput::focus already
	// turned the seam on; when the field blurs (Enter/Esc) the seam drops and
	// the next frame re-grabs it for Esc/Tab close. Without this, typing in
	// the chat composer was dead while an NPC dialogue was open (this pump
	// drained the queue first and discarded the characters).
	if(KronosInput::anyFocused())
	{
		$KNPC::keyCap = false;
		return;
	}
	if($KM::mouseOn && !$KNPC::keyCap)
	{
		glTextInput(1);
		$KNPC::keyCap = true;
	}
	if(!$KM::mouseOn && $KNPC::keyCap)
	{
		glTextInput(0);
		$KNPC::keyCap = false;
		return;
	}
	if(!$KNPC::keyCap)
		return;
	%guard = 0;
	while(%guard < 100)
	{
		%guard++;
		%ev = glTextPoll();
		if(String::len(%ev) < 1)
			return;
		%kind = String::getSubStr(%ev, 0, 1);
		%val  = String::getSubStr(%ev, 1, 99999);
		if(String::Compare(%kind, "c") != 0)   // special key (Esc=1, Tab=15)
		{
			if(%val == 1 || %val == 15)
			{
				KronosNPC::close();
				return;
			}
		}
		else
		{
			// number keys pick the numbered options (the rows render as
			// "1. keyword"); anything else is ignored while the window is up
			if(%val >= 1 && %val <= $KNPC::optCount
				&& String::findSubStr("123456789", %val) != -1)
			{
				remoteEval(2048, say, 0, "#say " @ $KNPC::opt[%val - 1]);
				$KNPC::optCount = 0;   // same advance as a click (KNPCOpts repopulates)
				return;
			}
		}
	}
}

// ============================================
// Line buffer (options come pre-parsed from the server via KNPCOpts)
// ============================================
function KronosNPC::pushLine(%text)
{
	if($KNPC::lineN >= $KNPC::MaxLines)
	{
		for(%i = 0; %i < $KNPC::MaxLines - 1; %i++)
			$KNPC::line[%i] = $KNPC::line[%i + 1];
		$KNPC::lineN = $KNPC::MaxLines - 1;
	}
	$KNPC::line[$KNPC::lineN] = %text;
	$KNPC::lineN++;
}

// ============================================
// Word-wrap the accumulated lines (cached by width/font)
// ============================================
function KronosNPC::rewrap(%w, %font)
{
	$KNPC::wlN = 0;
	for(%i = 0; %i < $KNPC::lineN; %i++)
		KronosNPC::wrapLine($KNPC::line[%i], %w, %font);
}

function KronosNPC::wrapLine(%text, %w, %font)
{
	glSetFont("Verdana", %font, $GLEX_SMOOTH, 0);
	if(%text == "" || getWord(glGetStringDimensions(%text), 0) <= %w)
	{
		$KNPC::wl[$KNPC::wlN] = %text;
		$KNPC::wlN++;
		return;
	}
	%line = "";
	for(%i = 0; (%word = getWord(%text, %i)) != -1; %i++)
	{
		if(%line == "")
			%try = %word;
		else
			%try = %line @ " " @ %word;
		if(getWord(glGetStringDimensions(%try), 0) > %w && %line != "")
		{
			$KNPC::wl[$KNPC::wlN] = %line;
			$KNPC::wlN++;
			%line = %word;
		}
		else
			%line = %try;
	}
	if(%line != "")
	{
		$KNPC::wl[$KNPC::wlN] = %line;
		$KNPC::wlN++;
	}
}

// ============================================
// Render
// ============================================
function KronosNPC::render(%sw, %sh)
{
	if($KNPC::open == "" || !$KM::enabled)
	{
		$Panel::knpcShown = false;
		$KNPC::btnShown = false;
		return;
	}

	%k = KronosMenu::uiScale(%sh);
	%szW = %sw * %k;
	%szH = %sh * %k;

	%w = floor(%sw * $pref::Kronos::npcW * %k);
	%pad = floor(%szW * 0.012);
	if(%pad < 4)
		%pad = 4;
	%titleH = floor(%szH * 0.05);
	%lineH = floor(%szH * 0.032);
	if(%lineH < 1)
		%lineH = 1;
	%optH = floor(%szH * 0.038);
	%fontTitle = floor(%titleH * 0.6);
	%fontText = floor(%lineH * 0.6);
	if(%fontText < 9)
		%fontText = 9;
	%fontOpt = floor(%optH * 0.5);
	if(%fontOpt < 9)
		%fontOpt = 9;

	%textW = %w - (%pad * 2);
	if($KNPC::wlDirty || $KNPC::wrapW != %textW || $KNPC::wrapFont != %fontText)
	{
		KronosNPC::rewrap(%textW, %fontText);
		$KNPC::wrapW = %textW;
		$KNPC::wrapFont = %fontText;
		$KNPC::wlDirty = false;
	}

	%nOpt = $KNPC::optCount + 1;   // + Goodbye
	%textBlockH = ($KNPC::wlN * %lineH);
	%h = %titleH + %pad + %textBlockH + %pad + (%nOpt * %optH) + %pad;

	if($pref::Kronos::npcX == "c" || $pref::Kronos::npcX == "")
		%x = floor((%sw - %w) / 2);
	else
		%x = floor($pref::Kronos::npcX * %sw);
	%y = floor($pref::Kronos::npcY * %sh);

	// move handle (title bar) for the drag system
	$Panel::knpcX = %x;  $Panel::knpcY = %y;  $Panel::knpcW = %w;  $Panel::knpcTH = %titleH;
	$Panel::knpcShown = true;

	// option geometry for click hit-testing
	%optY0 = %y + %titleH + %pad + %textBlockH + %pad;
	$KNPC::optX = %x + %pad;
	$KNPC::optW = %w - (%pad * 2);
	$KNPC::optY0 = %optY0;
	$KNPC::optRowH = %optH;
	$KNPC::btnShown = true;

	%hov = -1;
	if($KM::mouseOn && $KM::mouseX >= $KNPC::optX && $KM::mouseX < $KNPC::optX + $KNPC::optW
		&& $KM::mouseY >= %optY0)
		%hov = floor(($KM::mouseY - %optY0) / %optH);

	// ---- rects ----
	glDisable($GL_TEXTURE_2D);
	glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);
	KronosMenu::drawPanelBody(%x, %y, %w, %h, %pad, %titleH);

	%iy = %optY0;
	for(%i = 0; %i < %nOpt; %i++)
	{
		if(%i == %hov)
			glColor4ub($KT::hvR, $KT::hvG, $KT::hvB, 80);
		else if(%i == $KNPC::optCount)
			glColor4ub(150, 70, 70, 55);   // Goodbye row tint
		else
			glColor4ub($KT::chR, $KT::chG, $KT::chB, 55);
		glRectangle($KNPC::optX, %iy + 1, $KNPC::optW, %optH - 2);
		%iy += %optH;
	}

	// ---- text ----
	glColor4ub(235, 240, 255, 245);
	glSetFont("Verdana", %fontTitle, $GLEX_SMOOTH, 1);   // glow 1: match the other panel headers
	glDrawString(%x + %pad, %y + floor(%titleH * 0.16), $KNPC::name);

	glSetFont("Verdana", %fontText, $GLEX_SMOOTH, 0);
	glColor4ub(225, 230, 240, 235);
	%ty = %y + %titleH + %pad;
	for(%i = 0; %i < $KNPC::wlN; %i++)
	{
		glDrawString(%x + %pad, %ty, $KNPC::wl[%i]);
		%ty += %lineH;
	}

	glSetFont("Verdana", %fontOpt, $GLEX_SMOOTH, 0);
	%iy = %optY0;
	for(%i = 0; %i < $KNPC::optCount; %i++)
	{
		glColor4ub(255, 255, 255, 235);
		glDrawString($KNPC::optX + %pad, %iy + floor((%optH - %fontOpt) / 2) - 1, ((%i + 1) @ ".  ") @ $KNPC::opt[%i]);
		%iy += %optH;
	}
	glColor4ub(255, 220, 220, 235);
	glDrawString($KNPC::optX + %pad, %iy + floor((%optH - %fontOpt) / 2) - 1, "Goodbye");
}

// ============================================
// Click handling (dispatched from KronosMenu onMouseLMB)
// ============================================
function KronosNPC::handleClick(%x, %y)
{
	if($KNPC::open == "" || !$KNPC::btnShown)
		return false;
	if($KNPC::optRowH < 1)
		return false;
	if(%x < $KNPC::optX || %x >= $KNPC::optX + $KNPC::optW)
		return false;
	if(%y < $KNPC::optY0)
		return false;
	%i = floor((%y - $KNPC::optY0) / $KNPC::optRowH);
	if(%i < 0 || %i > $KNPC::optCount)
		return false;

	if(%i < $KNPC::optCount)
	{
		// run the real dialogue path - identical to typing "#say keyword"
		remoteEval(2048, say, 0, "#say " @ $KNPC::opt[%i]);
		// advance: drop the current choices immediately so they don't
		// linger; the bot's response repopulates them via KNPCOpts (if it
		// offers any - a terminal reply just leaves Goodbye)
		$KNPC::optCount = 0;
	}
	else
	{
		KronosNPC::close();   // Goodbye
	}
	return true;
}

// ============================================
// Console helpers
// ============================================
function KronosNPC::resetPos()
{
	$pref::Kronos::npcX = "c";
	$pref::Kronos::npcY = 0.28;
	echo("KronosNPC: dialogue window position reset");
}

// preview the window layout without a server (fake lines + options)
function KronosNPC::test()
{
	$KNPC::name = "Town Guard";
	$KNPC::open = true;
	$KNPC::lineN = 0;
	$KNPC::optCount = 0;
	KronosNPC::pushLine("Greetings. I can transport you to areas isolated from standard magic. Where to? [loop],[demise],[enigma],[echos],[yuliple]");
	remoteKNPCOpts(2048, "loop demise enigma echos yuliple");
	$KNPC::wlDirty = true;
}

// ============================================
// Initialize
// ============================================
$KNPC::open = "";
$KNPC::keyCap = false;   // true while we hold the glTextInput seam for Esc/Tab close
$KNPC::name = "";
$KNPC::lineN = 0;
$KNPC::optCount = 0;
$KNPC::wlN = 0;
$KNPC::wlDirty = true;
$KNPC::wrapW = -1;
$KNPC::wrapFont = -1;
$KNPC::btnShown = false;
$Panel::knpcShown = false;

echo("KronosNPC: NPC dialogue window loaded");
