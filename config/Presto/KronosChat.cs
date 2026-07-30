//==============================================
// KronosChat.cs - custom ScriptGL chat overlay for Kingdom of Kronos
//==============================================
// Replaces the stock engine chat (chatDisplayHud / FearGuiChatDisplay,
// whose bitmap .pft fonts are fixed-size and not script-adjustable) with
// a ScriptGL overlay rendered at an ADJUSTABLE font size + visible line
// count, word-wrapped, draggable, and scalable like the rest of the
// Kronos GUI.
//
// CAPTURE: incoming chat reaches the client via onClientMessage, which
// Presto's events.cs turns into eventClientMessage(%client, %text,
// %repeated). We attach to that (same pattern as ChatFilter) - no need to
// override onClientMessage. %client==0 => server/system line, else a
// player line. (Note: a few engine-LOCAL messages go straight to the C++
// chat control and never hit onClientMessage; those won't appear here.)
//
// STOCK CHAT: hidden via Control::SetVisible(chatDisplayHud, false) while
// this overlay is enabled (re-applied on PlayGui open / resolution change
// in KronosHUD.cs's event block is NOT used - we hook our own below).
//
// Loaded last (autoexec.cs, after KronosShop.cs). The drag + scale
// framework lives in KronosMenu.cs; this overlay registers as drag id
// "kchat" and is moved via $pref::Kronos::chatPosX/chatPosY.
//==============================================

Include("presto\\Event.cs");

// ---- prefs (persist in ClientPrefs.cs) ----
if($pref::Kronos::chatEnabled == "") $pref::Kronos::chatEnabled = true;
if($pref::Kronos::chatFontH == "")   $pref::Kronos::chatFontH = 0.011;    // TEXT SIZE: font height, fraction of screen height (independent of window size)
if($pref::Kronos::chatW == "")       $pref::Kronos::chatW = 0.34;         // WINDOW width, fraction of screen
if($pref::Kronos::chatH == "")       $pref::Kronos::chatH = 0.16;         // WINDOW height, fraction of screen (how many lines fit depends on text size)
if($pref::Kronos::chatPosX == "")    $pref::Kronos::chatPosX = 0.015;     // top-left, fractions
if($pref::Kronos::chatPosY == "")    $pref::Kronos::chatPosY = 0.58;
if($pref::Kronos::chatBg == "")      $pref::Kronos::chatBg = true;        // dim backdrop behind text
// $pref::Kronos::chatHide = space-list of filtered categories (persists). "" = none hidden.

$KC::MaxRaw = 120;   // stored source messages (for re-wrap on resize)
$KC::MaxDL  = 240;   // stored wrapped display lines
$KC::muteThis = false;   // per-message filter flag (set by onTag, consumed by onMsg)
$KC::diag0 = "";  $KC::diag1 = "";  $KC::diag2 = "";   // last-3-messages ring for KronosChat::lastMsg()
$KC::fbN = 0;            // filter buttons shown this frame

// short labels for the filter toggle buttons (falls back to the category name)
$KC::fLabel["adv"]    = "ADS";
$KC::fLabel["spellc"] = "CAST";
$KC::fLabel["loot"]   = "LOOT";
$KC::fLabel["house"]  = "HOUSE";
$KC::fLabel["stats"]  = "STATS";

// ============================================
// Capture
// ============================================
// A tag (~adv/~spellc/~loot/...) for the message about to arrive. If that category
// is filtered, flag the message so we ALSO skip it in the overlay - ChatFilter only
// mutes the STOCK chat; eventClientMessage still fires for muted lines. Fires before
// onMsg (events.cs triggers tag messages before eventClientMessage).
function KronosChat::onTag(%client, %tag, %value, %repeated)
{
	if($ChatFilter::Hide[%tag])
		$KC::muteThis = true;
}
Event::Attach(eventClientTagMessage, KronosChat::onTag, attachKronosChatTag);

function KronosChat::onMsg(%client, %msg, %repeated)
{
	%muted = $KC::muteThis;
	$KC::muteThis = false;        // consume the per-message filter flag
	if(!$pref::Kronos::chatEnabled)
		return;
	if(%muted)
		return;                   // category is filtered - don't show in overlay
	if(%msg == "")
		return;

	// Kronos relays ALL chat as server messages (%client is 0 even for player chat),
	// so we can't use the sender id. Player chat looks like  [GLBL] "text"  /
	// [TEAM] "text"  - it carries a channel tag AND the spoken text is in DOUBLE
	// quotes; system lines ("You have entered...", "Jobo has joined") have neither.
	// (Use the \" escaped double quote - standard in this codebase - NOT a single
	// quote, which Tribes' parser treats as a tagged-string delimiter.)
	%type = "srv";
	if(String::findSubStr(%msg, "\"") != -1
		|| String::findSubStr(%msg, "[GLBL]") != -1
		|| String::findSubStr(%msg, "[TEAM]") != -1)
		%type = "ply";

	// DIAG (on-demand via KronosChat::lastMsg()): show the COMPUTED type + the text,
	// so we can see whether player chat is being classified "ply" and what it looks
	// like. Stored only, never echoed (echoing every line during connect hung it).
	$KC::diag2 = $KC::diag1;
	$KC::diag1 = $KC::diag0;
	$KC::diag0 = "[" @ %type @ "] " @ String::getSubStr(%msg, 0, 45);

	KronosChat::pushRaw(%msg, %type);

	// wrap incrementally using the last render metrics; if we haven't
	// rendered yet, render()'s first pass will rewrap everything
	if($KC::lastW > 0 && $KC::lastFont > 0)
		KronosChat::wrapPush(%msg, %type, $KC::lastW, $KC::lastFont);
}

Event::Attach(eventClientMessage, KronosChat::onMsg, attachKronosChat);

// ============================================
// Message / display-line ring buffers (linear, drop oldest 25% when full)
// ============================================
function KronosChat::pushRaw(%text, %type)
{
	if($KC::rawN >= $KC::MaxRaw)
	{
		%drop = floor($KC::MaxRaw / 4);
		for(%i = 0; %i < $KC::rawN - %drop; %i++)
		{
			$KC::raw[%i] = $KC::raw[%i + %drop];
			$KC::rawType[%i] = $KC::rawType[%i + %drop];
		}
		$KC::rawN -= %drop;
	}
	$KC::raw[$KC::rawN] = %text;
	$KC::rawType[$KC::rawN] = %type;
	$KC::rawN++;
}

function KronosChat::pushDL(%text, %type)
{
	if($KC::dlN >= $KC::MaxDL)
	{
		%drop = floor($KC::MaxDL / 4);
		for(%i = 0; %i < $KC::dlN - %drop; %i++)
		{
			$KC::dl[%i] = $KC::dl[%i + %drop];
			$KC::dlType[%i] = $KC::dlType[%i + %drop];
		}
		$KC::dlN -= %drop;
	}
	$KC::dl[$KC::dlN] = %text;
	$KC::dlType[$KC::dlN] = %type;
	$KC::dlN++;

	// if the user has scrolled up, keep the view anchored on the same old
	// lines as new ones arrive (don't yank them to the bottom)
	if(!$KC::rewrapping && $KC::scroll > 0)
		$KC::scroll++;
}

// Word-wrap %text to %w pixels at %font, appending each line to the DL buffer
function KronosChat::wrapPush(%text, %type, %w, %font)
{
	glSetFont("Verdana", %font, $GLEX_SMOOTH, 0);
	if(%text == "" || getWord(glGetStringDimensions(%text), 0) <= %w)
	{
		KronosChat::pushDL(%text, %type);
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
			KronosChat::pushDL(%line, %type);
			%line = %word;
		}
		else
			%line = %try;
	}
	if(%line != "")
		KronosChat::pushDL(%line, %type);
}

// Rebuild all wrapped lines from the raw messages (on width / font change)
function KronosChat::rewrap(%w, %font)
{
	$KC::rewrapping = true;   // suppress scroll anchoring during a full rebuild
	$KC::dlN = 0;
	for(%i = 0; %i < $KC::rawN; %i++)
		KronosChat::wrapPush($KC::raw[%i], $KC::rawType[%i], %w, %font);
	$KC::rewrapping = false;
}

// ============================================
// Render (called every frame from the onPostDraw hook)
// ============================================
// $Config::ClassicOwnsHud: while a classic CustomConfig owns PlayGui, that config draws
// its own chat through the engine's chatDisplayHud -- KronosChat::applyVisibility (main.cpp)
// deliberately UN-hides it in that case. This renderer had no ownership test, so both chats
// drew at once. bindTalkKey already stands down (glSetTalkKey(0)); the renderer did not.
// Falling into the existing early-out also clears the $Panel::kchat* shown-flags, so the
// size/scroll widgets go with it.
function KronosChat::render(%sw, %sh)
{
	if(!$pref::Kronos::chatEnabled || $pref::Kronos::hudOff || $Config::ClassicOwnsHud)
	{
		$Panel::kchatShown = false;
		$Panel::kchatSzShown = false;
		$Panel::kchatScrShown = false;
		$KC::btnShown = false;
		$KC::composerShown = false;
		if(KronosInput::isFocused("kchat"))
			KronosInput::blur();
		return;
	}

	%up = $KM::mouseOn;

	// drain captured keystrokes from the native plugin into the focused field
	KronosInput::pump();

	// talk hotkey (Y): the plugin swallowed it + flagged us - open the composer
	if(String::len(glPollHotkey()) > 0)
		KronosChat::talkGlobal();

	// --- text size (independent of window size) ---
	%font = floor(%sh * $pref::Kronos::chatFontH);
	if(%font < 7)
		%font = 7;
	%pad = floor(%font * 0.4);
	if(%pad < 2)
		%pad = 2;
	%lineH = %font + floor(%font * 0.3);
	if(%lineH < 1)
		%lineH = 1;

	// --- window size (independent of text size) ---
	%w = floor(%sw * $pref::Kronos::chatW);
	%boxH = floor(%sh * $pref::Kronos::chatH);
	%x = floor(%sw * $pref::Kronos::chatPosX);
	%y = floor(%sh * $pref::Kronos::chatPosY);

	// scrollbar gutter is ALWAYS reserved (so text width - and wrapping -
	// don't change when the cursor toggles); the bar only draws when up
	%scrW = floor(%font * 0.7);
	if(%scrW < 6)
		%scrW = 6;
	%textW = %w - (%pad * 2) - %scrW;
	if(%textW < 20)
		%textW = 20;

	// how many wrapped lines fit in the current window height
	%visible = floor((%boxH - (%pad * 2)) / %lineH);
	if(%visible < 1)
		%visible = 1;

	// rewrap to the text width / font if either changed
	if($KC::lastW != %textW || $KC::lastFont != %font)
	{
		KronosChat::rewrap(%textW, %font);
		$KC::lastW = %textW;
		$KC::lastFont = %font;
	}

	// clamp scroll (0 = newest at bottom; up to dlN-visible into history)
	%maxScroll = $KC::dlN - %visible;
	if(%maxScroll < 0)
		%maxScroll = 0;
	if($KC::scroll > %maxScroll)
		$KC::scroll = %maxScroll;
	if($KC::scroll < 0)
		$KC::scroll = 0;

	// stash rects for the drag system
	$Panel::kchatX = %x;   $Panel::kchatY = %y;   $Panel::kchatW = %w;   $Panel::kchatH = %boxH;
	$Panel::kchatShown = true;

	// ---- backdrop ----
	if(($pref::Kronos::chatBg && $KC::dlN > 0) || %up)
	{
		glDisable($GL_TEXTURE_2D);
		glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);
		%bgA = 70;
		if(%up)
			%bgA = 130;
		glColor4ub($KT::bgR, $KT::bgG, $KT::bgB, %bgA);
		glRectangle(%x, %y, %w, %boxH);
		if(%up)
		{
			glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, 200);
			glRectangle(%x, %y, %w, 2);
			glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, 90);
			glRectangle(%x, %y + %boxH - 1, %w, 1);
			glRectangle(%x, %y, 1, %boxH);
			glRectangle(%x + %w - 1, %y, 1, %boxH);
		}
	}

	// ---- chat text ----
	%end = $KC::dlN - $KC::scroll;     // exclusive; newest shown = end-1
	if(%end > $KC::dlN)
		%end = $KC::dlN;
	%start = %end - %visible;
	if(%start < 0)
		%start = 0;
	%nshown = %end - %start;

	%ty = %y + %boxH - %pad - (%nshown * %lineH);
	if(%ty < %y + %pad)
		%ty = %y + %pad;

	glSetFont("Verdana", %font, $GLEX_SMOOTH, 0);
	for(%i = %start; %i < %end; %i++)
	{
		// NOTE: must use String::Compare, NOT == ("ply"=="srv" is TRUE because ==
		// coerces non-numeric strings to 0, which colored EVERY line as player chat).
		if(String::Compare($KC::dlType[%i], "ply") == 0)
			glColor4ub(100, 230, 120, 245);   // player chat - GREEN (stands out from ads/system)
		else
			glColor4ub(205, 215, 235, 235);   // server / system - cool white
		glDrawString(%x + %pad, %ty, $KC::dl[%i]);
		%ty += %lineH;
	}

	if($KC::dlN < 1 && %up)
	{
		glColor4ub(160, 170, 190, 170);
		glSetFont("Verdana", %font, $GLEX_SMOOTH, 0);
		glDrawString(%x + %pad, %y + %pad, "(chat)");
	}

	// ---- chat composer (text input row, just below the chat box) ----
	// Shown while the cursor is up OR while actively typing (so a key-bound
	// KronosChat::beginSay works during gameplay with the cursor down).
	KronosChat::renderComposer(%x, %y + %boxH + 2, %w, %font, %lineH, %pad);

	// ---- cursor-up controls: scrollbar, resize grip, A-/A+ text size ----
	$Panel::kchatSzShown = false;
	$Panel::kchatScrShown = false;
	$KC::btnShown = false;
	$KC::fbN = 0;
	if(!%up)
		return;

	glDisable($GL_TEXTURE_2D);
	glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);

	%gz = floor(%font * 1.2);
	if(%gz < 13)
		%gz = 13;

	// scrollbar (right gutter, above the resize grip) - high transparency
	%trackX = %x + %w - %scrW;
	%trackY = %y;
	%trackH = %boxH - %gz;
	if(%trackH < %lineH)
		%trackH = %lineH;
	$Panel::kchatScrX = %trackX;  $Panel::kchatScrY = %trackY;
	$Panel::kchatScrW = %scrW;    $Panel::kchatScrTrackH = %trackH;
	$Panel::kchatScrVisible = %visible;
	$Panel::kchatScrTotal = $KC::dlN;
	$Panel::kchatScrShown = true;

	// faint track
	glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, 28);
	glRectangle(%trackX, %trackY, %scrW, %trackH);

	if($KC::dlN > %visible)
	{
		%thumbH = floor(%trackH * %visible / $KC::dlN);
		if(%thumbH < %gz)
			%thumbH = %gz;
		%travel = %trackH - %thumbH;
		%frac = 0;
		if(%maxScroll > 0)
			%frac = $KC::scroll / %maxScroll;     // 0 newest .. 1 oldest
		%thumbY = %trackY + floor((1.0 - %frac) * %travel);
		%thumbA = 70;
		if($Drag::active && $Drag::id == "kchatscr")
			%thumbA = 150;
		glColor4ub($KT::txR, $KT::txG, $KT::txB, %thumbA);
		glRectangle(%trackX + 1, %thumbY, %scrW - 2, %thumbH);
	}

	// resize grip (bottom-right) - WINDOW size only (width + height)
	%gx = %x + %w - %gz;
	%gy = %y + %boxH - %gz;
	$Panel::kchatSzX = %gx;  $Panel::kchatSzY = %gy;
	$Panel::kchatSzW = %gz;  $Panel::kchatSzH = %gz;
	$Panel::kchatSzShown = true;
	if($Drag::active && $Drag::id == "kchatsz")
		glColor4ub($KT::hbR, $KT::hbG, $KT::hbB, 245);
	else
		glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, 210);
	glRectangle(%gx, %gy, %gz, %gz);
	glColor4ub(235, 240, 255, 220);
	glRectangle(%gx + 3, %gy + %gz - 4, %gz - 6, 1);
	glRectangle(%gx + floor(%gz / 2), %gy + 3, 1, %gz - 6);

	// A- / A+ text-size buttons (top-right, left of the scrollbar)
	%bw = floor(%font * 1.4);
	if(%bw < 16)
		%bw = 16;
	%bh = floor(%font * 1.3);
	if(%bh < 14)
		%bh = 14;
	%by = %y + 2;
	%apX = %trackX - %bw - 2;
	%amX = %apX - %bw - 2;
	$KC::amX = %amX;  $KC::amY = %by;  $KC::apX = %apX;  $KC::apY = %by;
	$KC::bW = %bw;    $KC::bH = %bh;
	$KC::btnShown = true;

	glColor4ub($KT::chR, $KT::chG, $KT::chB, 170);
	glRectangle(%amX, %by, %bw, %bh);
	glRectangle(%apX, %by, %bw, %bh);
	glColor4ub(235, 240, 255, 240);
	glSetFont("Verdana", floor(%bh * 0.62), $GLEX_SMOOTH, 0);
	glDrawString(%amX + floor(%bw * 0.28), %by + floor(%bh * 0.12), "A-");
	glDrawString(%apX + floor(%bw * 0.28), %by + floor(%bh * 0.12), "A+");

	// chat-filter toggle buttons (left of A-): white label = category SHOWN,
	// red = HIDDEN. Click toggles + persists ($pref::Kronos::chatHide).
	%fbw = floor(%font * 2.6);
	if(%fbw < 28)
		%fbw = 28;
	%fx = %amX;
	$KC::fbN = 0;
	$KC::fbY = %by;  $KC::fbW = %fbw;  $KC::fbH = %bh;
	glSetFont("Verdana", floor(%bh * 0.52), $GLEX_SMOOTH, 0);
	for(%fi = 0; (%cat = GetWord($ChatFilter::Categories, %fi)) != -1; %fi++)
	{
		%fx = %fx - %fbw - 2;
		%lbl = $KC::fLabel[%cat];
		if(String::len(%lbl) < 1)
			%lbl = %cat;
		glColor4ub(45, 55, 75, 180);
		glRectangle(%fx, %by, %fbw, %bh);
		if($ChatFilter::Hide[%cat])
			glColor4ub(225, 80, 70, 250);     // red = hidden
		else
			glColor4ub(235, 240, 255, 245);   // white = shown
		glDrawString(%fx + 3, %by + floor(%bh * 0.16), %lbl);
		$KC::fbX[$KC::fbN] = %fx;  $KC::fbCat[$KC::fbN] = %cat;
		$KC::fbN++;
	}

	// "UI" button (leftmost): pops the Scale / Theme panel open/closed
	// (the panel itself is KronosMenu's slider widget, drawn outside chat)
	%fx = %fx - %fbw - 2;
	$KC::uiX = %fx;  $KC::uiY = %by;  $KC::uiW = %fbw;  $KC::uiH = %bh;
	if($KM::scaleOpen)
		glColor4ub($KT::hbR, $KT::hbG, $KT::hbB, 210);   // lit while open
	else
		glColor4ub(45, 55, 75, 180);
	glRectangle(%fx, %by, %fbw, %bh);
	glColor4ub(235, 240, 255, 245);
	glDrawString(%fx + 3, %by + floor(%bh * 0.16), "UI");
}

// ============================================
// Composer (chat text input)
// ============================================
// A one-line input row drawn just below the chat box. Type a message and press
// Enter to send it (global or team), Esc to cancel, Up/Down to recall history.
// Uses the shared KronosInput field (needs the native glTextInput seam).
function KronosChat::renderComposer(%x, %yTop, %w, %font, %lineH, %pad)
{
	$KC::composerShown = false;
	%foc = KronosInput::isFocused("kchat");
	// only show when the cursor is up (clickable) or we're actively typing
	if(!$KM::mouseOn && !%foc)
		return;

	%rowH = %lineH + (%pad * 2);
	%cy = %yTop;

	glDisable($GL_TEXTURE_2D);
	glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);

	// backdrop (brighter while focused)
	if(%foc)
		glColor4ub($KT::b2R, $KT::b2G, $KT::b2B, 205);
	else
		glColor4ub(12, 16, 26, 135);
	glRectangle(%x, %cy, %w, %rowH);
	if(%foc)
	{
		glColor4ub($KT::acR, $KT::acG, $KT::acB, 225);
		glRectangle(%x, %cy, %w, 2);
		glRectangle(%x, %cy + %rowH - 1, %w, 1);
		glRectangle(%x, %cy, 1, %rowH);
		glRectangle(%x + %w - 1, %cy, 1, %rowH);
	}

	// All/Team toggle chip on the left
	glSetFont("Verdana", %font, $GLEX_SMOOTH, 0);
	%lbl = "All";
	if($KC::team == 1)
		%lbl = "Team";
	%chipW = getWord(glGetStringDimensions("Team"), 0) + (%pad * 2);
	if($KC::team == 1)
		glColor4ub(210, 90, 80, 215);    // team = red
	else
		glColor4ub($KT::chR, $KT::chG, $KT::chB, 205);   // all  = blue
	glRectangle(%x + %pad, %cy + %pad, %chipW, %lineH);
	glColor4ub(240, 245, 255, 248);
	glDrawString(%x + %pad + %pad, %cy + %pad, %lbl);

	// text area
	%tx = %x + %pad + %chipW + (%pad * 2);
	%tw = (%x + %w - %pad) - %tx;
	if(%tw < 12)
		%tw = 12;
	if(%foc)
	{
		KronosInput::drawText(%tx, %cy + %pad, %tw, %font);
	}
	else
	{
		glSetFont("Verdana", %font, $GLEX_SMOOTH, 0);
		glColor4ub(150, 160, 185, 165);
		glDrawString(%tx, %cy + %pad, "Click here to chat");
	}

	// stash rects for the click handler
	$KC::composerX = %x;            $KC::composerY = %cy;
	$KC::composerW = %w;            $KC::composerH = %rowH;
	$KC::teamChipX = %x + %pad;     $KC::teamChipY = %cy + %pad;
	$KC::teamChipW = %chipW;        $KC::teamChipH = %lineH;
	$KC::composerShown = true;
}

// Begin composing (bind a key to this for talk-key-style quick chat, e.g.
//   bindCommand(keyboard0, make, "u", TO, "KronosChat::beginSay();");
// works even with the cursor down - the key seam is global).
function KronosChat::beginSay()
{
	// Same ownership test as render(): with a classic config owning the HUD the composer
	// must not open either, or a stray bind still pops a Kronos input line over its chat.
	if(!$pref::Kronos::chatEnabled || $Config::ClassicOwnsHud)
		return;
	$KC::histPos = $KC::histN;
	KronosInput::focus("kchat", "", "KronosChat::submit", "", "KronosChat::nav", 120);
}

// Enter: send the line (global or team) and return control to the game.
function KronosChat::submit()
{
	%t = KronosInput::text();
	if(%t != "")
	{
		// client -> server chat; senderName MUST stay empty (a non-empty one
		// trips remoteSay's exploit guard). %team: 0 = all, 1 = team.
		remoteEval(2048, Say, $KC::team, %t);
		// history
		$KC::hist[$KC::histN] = %t;
		$KC::histN++;
		$KC::histPos = $KC::histN;
	}
	KronosInput::blur();
}

// Up/Down: recall sent-message history.
function KronosChat::nav(%dik)
{
	if($KC::histN < 1)
		return;
	if(%dik == $DIK::Up)
	{
		$KC::histPos = $KC::histPos - 1;
		if($KC::histPos < 0)
			$KC::histPos = 0;
		KronosInput::setText($KC::hist[$KC::histPos]);
	}
	else
	{
		$KC::histPos = $KC::histPos + 1;
		if($KC::histPos >= $KC::histN)
		{
			$KC::histPos = $KC::histN;
			KronosInput::setText("");
		}
		else
			KronosInput::setText($KC::hist[$KC::histPos]);
	}
}

function KronosChat::toggleTeam()
{
	if($KC::team == 1)
		$KC::team = 0;
	else
		$KC::team = 1;
}

// Click handler for the composer + the on-screen A-/A+ text-size buttons.
// Called from KronosMenu's onMouseLMB BEFORE the panel-drag check (so the
// buttons/composer win over the box's move handle). Returns true if hit.
function KronosChat::handleClick(%x, %y)
{
	if(!$pref::Kronos::chatEnabled || $pref::Kronos::hudOff)
		return false;

	// composer row (chat input)
	if($KC::composerShown
		&& %x >= $KC::composerX && %x < $KC::composerX + $KC::composerW
		&& %y >= $KC::composerY && %y < $KC::composerY + $KC::composerH)
	{
		// All/Team toggle chip
		if(%x >= $KC::teamChipX && %x < $KC::teamChipX + $KC::teamChipW
			&& %y >= $KC::teamChipY && %y < $KC::teamChipY + $KC::teamChipH)
		{
			KronosChat::toggleTeam();
			return true;
		}
		// focus the text field
		if(!KronosInput::isFocused("kchat"))
			KronosChat::beginSay();
		return true;
	}

	if(!$KC::btnShown)
		return false;

	if(%x >= $KC::amX && %x < $KC::amX + $KC::bW
		&& %y >= $KC::amY && %y < $KC::amY + $KC::bH)
	{
		KronosChat::setFont($pref::Kronos::chatFontH - 0.0015);
		return true;
	}
	if(%x >= $KC::apX && %x < $KC::apX + $KC::bW
		&& %y >= $KC::apY && %y < $KC::apY + $KC::bH)
	{
		KronosChat::setFont($pref::Kronos::chatFontH + 0.0015);
		return true;
	}
	// "UI" button - toggle the Scale / Theme panel
	if(%x >= $KC::uiX && %x < $KC::uiX + $KC::uiW
		&& %y >= $KC::uiY && %y < $KC::uiY + $KC::uiH)
	{
		$KM::scaleOpen = !$KM::scaleOpen;
		return true;
	}
	// chat-filter toggle buttons (left of A-)
	for(%fi = 0; %fi < $KC::fbN; %fi++)
	{
		if(%x >= $KC::fbX[%fi] && %x < $KC::fbX[%fi] + $KC::fbW
			&& %y >= $KC::fbY && %y < $KC::fbY + $KC::fbH)
		{
			KronosChat::toggleFilter($KC::fbCat[%fi]);
			return true;
		}
	}
	return false;
}

// ============================================
// Chat category filters (ads / casting / loot) - toggle + persist
// ============================================
// Hide a category in BOTH the stock chat (ChatFilter mutes it) and our overlay
// (onMsg skips it via the onTag flag). State persists in $pref::Kronos::chatHide
// (a space-list of hidden categories) so it survives restarts.
function KronosChat::toggleFilter(%cat)
{
	if($ChatFilter::Hide[%cat])
		$ChatFilter::Hide[%cat] = "";
	else
		$ChatFilter::Hide[%cat] = true;
	KronosChat::saveFilterPrefs();
}

function KronosChat::saveFilterPrefs()
{
	%list = "";
	for(%fi = 0; (%cat = GetWord($ChatFilter::Categories, %fi)) != -1; %fi++)
	{
		if($ChatFilter::Hide[%cat])
		{
			if(String::len(%list) < 1)
				%list = %cat;
			else
				%list = %list @ " " @ %cat;
		}
	}
	$pref::Kronos::chatHide = %list;
	KronosMenu::savePrefs();
}

function KronosChat::applyFilterPrefs()
{
	for(%fi = 0; (%cat = GetWord($pref::Kronos::chatHide, %fi)) != -1; %fi++)
		$ChatFilter::Hide[%cat] = true;
}

// DIAG: after a PLAYER says something (or an ad appears), run KronosChat::lastMsg()
// in the console to see the last 3 messages' sender ids. Tells us whether player
// chat arrives with a non-zero %client (so we can color it green) or as client=0.
function KronosChat::lastMsg()
{
	echo("[KC] recent messages (newest first):");
	echo("  1) " @ $KC::diag0);
	echo("  2) " @ $KC::diag1);
	echo("  3) " @ $KC::diag2);
}

// ============================================
// Stock chat visibility
// ============================================
function KronosChat::applyVisibility()
{
	if($pref::Kronos::chatEnabled && !$pref::Kronos::hudOff)
		Control::SetVisible(chatDisplayHud, false);
	else
		Control::SetVisible(chatDisplayHud, true);
}

Event::Attach(eventGuiOpen_PlayGui, "KronosChat::applyVisibility();", attachKronosChatVis);
Event::Attach(eventScreenModeChanged, "KronosChat::applyVisibility();", attachKronosChatVis);
// Re-assert the talk hotkey on play-mode entry (harmless/idempotent; the plugin
// holds the hotkey itself, so no timing race like the old bindCommand approach).
Event::Attach(eventGuiOpen_PlayGui, "KronosChat::bindTalkKey();", attachKronosChatBind);

// ============================================
// Console helpers
// ============================================
// Set the WINDOW height to fit %n lines at the current text size (the
// bottom-right corner grip does this by dragging; this is the console form)
function KronosChat::setLines(%n)
{
	if(%n == "" || %n < 1)
	{
		echo("usage: KronosChat::setLines(12)  - sets window height to fit n lines");
		return;
	}
	%sh = getWord(Control::getExtent(PlayGui), 1);
	if(%sh < 100)
		%sh = 1080;
	%font = floor(%sh * $pref::Kronos::chatFontH);
	if(%font < 7)
		%font = 7;
	%lineH = %font + floor(%font * 0.3);
	%pad = floor(%font * 0.4);
	$pref::Kronos::chatH = ((%n * %lineH) + (%pad * 2)) / %sh;
	KronosMenu::savePrefs();
	echo("KronosChat: window sized to fit " @ %n @ " lines");
}

// TEXT SIZE - font height as a fraction of screen height (independent of
// the window size). e.g. 0.009 = small, 0.014 = large. Clamped to sane limits.
function KronosChat::setFont(%frac)
{
	if(%frac == "" || %frac <= 0)
	{
		echo("usage: KronosChat::setFont(0.010)  - fraction of screen height; current = " @ $pref::Kronos::chatFontH);
		return;
	}
	if(%frac < 0.006)
		%frac = 0.006;
	if(%frac > 0.040)
		%frac = 0.040;
	$pref::Kronos::chatFontH = %frac;
	$KC::lastFont = -1;   // force a rewrap
	KronosMenu::savePrefs();
	echo("KronosChat: text size = " @ %frac @ " of screen height");
}

function KronosChat::smaller()
{
	%f = $pref::Kronos::chatFontH - 0.001;
	if(%f < 0.006)
		%f = 0.006;
	KronosChat::setFont(%f);
}

function KronosChat::bigger()
{
	KronosChat::setFont($pref::Kronos::chatFontH + 0.001);
}

function KronosChat::setWidth(%frac)
{
	if(%frac == "" || %frac <= 0)
	{
		echo("usage: KronosChat::setWidth(0.30)  - fraction of screen width; current = " @ $pref::Kronos::chatW);
		return;
	}
	$pref::Kronos::chatW = %frac;
	$KC::lastW = -1;   // force a rewrap
	KronosMenu::savePrefs();
	echo("KronosChat: width = " @ %frac @ " of screen");
}

function KronosChat::enable()
{
	$pref::Kronos::chatEnabled = true;
	KronosChat::applyVisibility();
	KronosChat::bindTalkKey();    // Y -> composer
	KronosMenu::savePrefs();
	echo("KronosChat: custom chat overlay ON (stock chat hidden)");
}

function KronosChat::disable()
{
	$pref::Kronos::chatEnabled = false;
	KronosChat::applyVisibility();
	KronosChat::bindTalkKey();    // Y -> stock chat
	KronosMenu::savePrefs();
	echo("KronosChat: custom chat overlay OFF (stock chat restored)");
}

function KronosChat::clear()
{
	$KC::rawN = 0;
	$KC::dlN = 0;
}

// preview without a server: inject sample lines
function KronosChat::test()
{
	KronosChat::onMsg(0, "Server: Welcome to Kingdom of Kronos!", false);
	KronosChat::onMsg(2050, "Yuliple: anyone selling a broadsword?", false);
	KronosChat::onMsg(0, "This is a longer system line that should wrap across multiple rows to show the word-wrapping in the custom chat overlay.", false);
	KronosChat::onMsg(2051, "Murch: gl hf everyone", false);
}

// ============================================
// Talk key (Y) -> open the composer for global chat
// ============================================
// Override the stock global-chat key so it opens the Kronos composer instead of
// the engine message HUD. config.cs binds Y -> IDACTION_CHAT; this runs later (via
// autoexec, after config.cs) and replaces that binding while the overlay is on.
function KronosChat::talkGlobal()
{
	$KC::team = 0;            // global channel
	KronosChat::beginSay();
}

// Point Y at our composer when the overlay is enabled; restore the stock chat
// action when it's disabled (so toggling KronosChat::disable() doesn't dead-key Y).
function KronosChat::bindTalkKey()
{
	// Y is hardwired to the engine's IDACTION_CHAT, which script binds can't override.
	// So the native plugin SWALLOWS Y (DIK 0x15 = 21) before the engine's action map
	// and flags us; we poll glPollHotkey() in render() and open the composer. Setting
	// the hotkey to 0 lets Y fall back to stock chat (when the overlay is disabled).
	if($pref::Kronos::chatEnabled && !$pref::Kronos::hudOff)
		glSetTalkKey(21);
	else
		glSetTalkKey(0);
}

// ============================================
// Initialize
// ============================================
$KC::rawN = 0;
$KC::dlN = 0;
$KC::scroll = 0;
$KC::rewrapping = false;
$KC::btnShown = false;
$KC::lastW = -1;
$KC::lastFont = -1;
// composer (chat input)
$KC::team = 0;            // 0 = all, 1 = team
$KC::histN = 0;          // sent-message history count
$KC::histPos = 0;        // history cursor (== histN means "new line")
$KC::composerShown = false;
$Panel::kchatShown = false;
$Panel::kchatSzShown = false;
$Panel::kchatScrShown = false;

if($Mode::PlayMode)
	KronosChat::applyVisibility();

KronosChat::bindTalkKey();    // Y opens the composer (runs after config.cs's Y->IDACTION_CHAT)
KronosChat::applyFilterPrefs();   // restore persisted ad/cast/loot filter state

echo("KronosChat: custom chat overlay loaded (KronosChat::setFont/setLines/disable)");
