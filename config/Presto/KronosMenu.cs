//==============================================
// KronosMenu.cs - modern ScriptGL TAB menu for Kingdom of Kronos
//==============================================
// ARCHITECTURE
//
// Transport: 100% stock. remoteNewMenu/remoteAddMenuItem build the
// engine ChatMenu "CurServerMenu" (number/letter hotkeys + the
// menuSelect round-trip unchanged), and additionally record the items
// so a modern panel can be drawn via ScriptGL onPostDraw.
//
// Stock visuals: every control in the score-screen gui files
// (base\gui\Score.gui and lr_score.gui) is moved off-screen by a
// one-time binary patch (positions 8000,8000). The score DIALOG still
// opens on TAB - invisible - which keeps the engine cursor on. The
// live files are plain copies of the patched templates
// (KM_score_base.gui / KM_lr_base.gui).
//
// Mouse clicks: Hudbot mouse callbacks ($MouseEnable). The engine GUI
// is NOT used for input. Earlier versions baked SimGui::ActiveCtrl
// click zones into the gui files; that broke TAB-to-close, because
// ActiveCtrl::wantsTabListMembership() is unconditionally true, the
// canvas consumes any key event whenever a control holds keyboard
// focus (simGuiCanvas.cpp processEvent), and Canvas::onKeyDown turns
// TAB into tabNext() focus cycling - so the action map binding that
// sends scoresOff never fired. With zero ActiveCtrls in the dialog
// the tab list is empty, focus stays on the canvas, and TAB falls
// through to the stock close path. Hudbot reports cursor position
// and button state while the GUI cursor is active (exactly when the
// score dialog is up); rows are hit-tested in script against the
// same $KML layout the renderer uses.
//
// Player list: REQUEST-DRIVEN server push. On every NewMenu the
// client sends remoteEval(2048, KMGetPlayers); the server answers
// with KMPlayer rows + KMPlayerCount (KronosHUD_Server.cs). Vanilla
// clients never ask, never get pushed, and keep their stock engine
// scoreboard. Clicking a row sends the SAME message the stock
// scoreboard sent - remoteEval(2048, SelectClient, id) - so all the
// server-side selClient machinery (player-specific menu options,
// KronosMenu_SendPlayerInfo) works unchanged.
//
// Character info: the server feeds the stock bottom info box via
// remoteEval(client, "setInfoLine", n, text). remoteSetInfoLine is a
// base-script function (client.cs), overridden here to capture the
// text for our info panel. Vanilla clients keep the stock InfoCtrlBox.
//
// All panels are TOP-ANCHORED with fixed row slots so a row's hit
// rectangle is always at the same screen position no matter the
// item count.
//
// KronosMenu::probe() - run from console while TAB is open - reports
// menu/mouse state. KronosMenu::disable() stops the panel (debug only).
//==============================================

if($KM::enabled == "")
	$KM::enabled = true;

// UI scale knob (persists - it's a $pref). 1.0 = GUI sized for a 1080p
// reference; lower = smaller share of the screen at high resolutions.
// Tune live with KronosMenu::setScale(0.85) etc.
if($pref::Kronos::UiScale == "")
	$pref::Kronos::UiScale = 1.0;
if($pref::Kronos::UiRefH == "")
	$pref::Kronos::UiRefH = 1080;

// Movable panel positions (persist - fractions of the screen). Drag a
// panel by its title bar to move it; KronosMenu::resetLayout() restores
// these. infoX = "c" means the character-info box stays centered until
// it's dragged.
if($pref::Kronos::menuX == "")    $pref::Kronos::menuX = 0.08;
if($pref::Kronos::menuY == "")    $pref::Kronos::menuY = 0.16;
if($pref::Kronos::playersX == "") $pref::Kronos::playersX = 0.54;
if($pref::Kronos::playersY == "") $pref::Kronos::playersY = 0.16;
if($pref::Kronos::infoX == "")    $pref::Kronos::infoX = "c";
if($pref::Kronos::infoY == "")    $pref::Kronos::infoY = 0.75;
if($pref::Kronos::sliderX == "")  $pref::Kronos::sliderX = "c";   // UI-scale slider pos ("c" = centered)
if($pref::Kronos::sliderY == "")  $pref::Kronos::sliderY = 0.015;

// Hudbot: enables onMouseActive/onMouseMove/onMouseLMB callbacks
// (see Hudbot\Docs\prefs.html). Reported while the GUI cursor is up.
$MouseEnable = true;

$KM::MaxZones = 15;   // menu rows
$KM::MaxPRows = 16;   // player-list rows (matches $KronosMenu::MaxListRows server-side)

// ============================================
// Shared layout - single source of truth, in real screen pixels.
// Used by BOTH the panel renderer and the click hit-testing.
// ============================================

// UI scale factor (height-based). Maps the original proportional design
// DOWN toward a reference height so the GUI takes a smaller share of the
// screen as resolution climbs (more game world visible at 1440p/4K),
// instead of always filling the same percentage. $pref::Kronos::UiScale
// is the user knob (1.0 = sized for the reference height; lower = smaller).
// Capped at 1.0 (the original proportional size) so it can never overflow
// a small window like 809x597 - on screens at or below the reference the
// factor naturally falls back to full proportional.
function KronosMenu::uiScale(%sh)
{
	%scale = $pref::Kronos::UiScale;
	if(%scale == "" || %scale <= 0)
		%scale = 1.0;
	%ref = $pref::Kronos::UiRefH;
	if(%ref == "" || %ref < 100)
		%ref = 1080;

	%base = %ref / %sh;     // <1 on screens taller than the reference
	if(%base > 1.0)
		%base = 1.0;        // never larger than the original proportional size
	%k = %base * %scale;
	if(%k > 1.0)
		%k = 1.0;
	return %k;
}

function KronosMenu::computeLayout(%sw, %sh)
{
	// SIZES scale toward the reference (szW/szH); POSITIONS come from the
	// movable, persisted per-panel fractions (drag to reposition).
	%k = KronosMenu::uiScale(%sh);
	%szW = %sw * %k;
	%szH = %sh * %k;
	$KML::k = %k;

	$KML::pad    = floor(%szW * 0.012);
	$KML::w      = floor(%szW * 0.38);
	$KML::wMenu  = $KML::w; // menu panel may widen in render to fit long items
	$KML::wPlayers = $KML::w; // player panel may widen in render too
	$KML::titleH = floor(%szH * 0.05);
	$KML::rowH   = floor(%szH * 0.034);
	$KML::lineH  = floor(%szH * 0.026);

	// movable panel positions (persisted as fractions of the screen)
	$KML::mx     = floor($pref::Kronos::menuX * %sw);
	$KML::menuY  = floor($pref::Kronos::menuY * %sh);
	$KML::px     = floor($pref::Kronos::playersX * %sw);
	$KML::plY    = floor($pref::Kronos::playersY * %sh);
	$KML::iy     = floor($pref::Kronos::infoY * %sh);

	$KML::menuRowY0 = $KML::menuY + $KML::titleH + floor($KML::pad / 2);
	$KML::plRowY0   = $KML::plY   + $KML::titleH + floor($KML::pad / 2);

	// aliases for the menu panel (probe / legacy refs)
	$KML::y      = $KML::menuY;
	$KML::rowY0  = $KML::menuRowY0;
	$KML::ix     = floor((%sw - $KML::w) / 2);
}

// ============================================
// Authoritative screen size for ScriptGL layout
// ============================================
// The %dimensions ScriptGL passes to the draw hooks can be stale or
// wrong (notably in windowed OpenGL), which makes every panel render at
// a fixed pixel size in a screen corner instead of scaling with the
// resolution. ScriptGL always draws into the engine's live canvas
// ortho, whose size is the PlayGui extent - so that extent is the
// correct basis for both layout AND drawing. Fall back to the passed
// dims only if the extent isn't sane yet (mirrors the >100 guard in
// Presto::ScreenSize). If %dimensions was already correct the extent
// matches it, so this never changes a correctly-scaled GUI.
function KronosMenu::screenDim(%fallback)
{
	%ext = Control::getExtent(PlayGui);
	if(getWord(%ext, 0) > 100 && getWord(%ext, 1) > 100)
	{
		$KM::dimSrc = "extent";
		$KM::dim = %ext;
		$KM::dimSGL = %fallback;
		return %ext;
	}
	$KM::dimSrc = "scriptgl";
	$KM::dim = %fallback;
	$KM::dimSGL = %fallback;
	return %fallback;
}

// ============================================
// Menu transport overrides (base: scripts.vol menu.cs)
// ============================================

function remoteNewMenu(%server, %title)
{
	if(%server != 2048)
		return;

	if(isObject(CurServerMenu))
		deleteObject(CurServerMenu);

	newObject(CurServerMenu, ChatMenu, %title);
	if(isObject(PlayChatMenu))
		setCMMode(PlayChatMenu, 0);
	setCMMode(CurServerMenu, 1);

	$KM::title = %title;
	$KM::count = 0;
	$KM::active = true;
	$KM::measureDirty = true;

	// ask the server for the player list (vanilla-safe: only clients
	// running this script ever send the request). THROTTLED: menus rebuild on
	// every selection (each skill-point click = a new menu), and re-pulling
	// the whole roster each time wasted the reliable-stream budget the menu
	// rows need - the refresh felt sluggish. Rosters change slowly; 3s is fresh.
	// NB: sim time RESETS on join / mission change, so a stamp from a
	// previous session makes the delta negative - treat that as stale and
	// re-request (a plain "> 3.0" check suppressed the roster forever)
	%plDt = GetSimTime() - $KM::plReqTime;
	if($KM::plReqTime == "" || %plDt > 3.0 || %plDt < 0)
	{
		$KM::plReqTime = GetSimTime();
		remoteEval(2048, KMGetPlayers);
	}
}

function remoteAddMenuItem(%server, %title, %code)
{
	if(%server != 2048)
		return;

	addCMCommand(CurServerMenu, %title, clientMenuSelect, %code);

	// First character of the label is the engine hotkey
	%idx = $KM::count;
	$KM::key[%idx] = String::getSubStr(%title, 0, 1);
	$KM::label[%idx] = String::getSubStr(%title, 1, 999);
	$KM::code[%idx] = %code;
	$KM::count++;
	$KM::measureDirty = true;
}

function remoteCancelMenu(%server)
{
	if(%server != 2048)
		return;

	if(isObject(CurServerMenu))
		deleteObject(CurServerMenu);
	$KM::active = false;
	$KM::selId = "";
	for(%i = 1; %i <= 6; %i++)
		$KM::info[%i] = "";
}

// Called by the engine ChatMenu on hotkey press, and by clickOption
function clientMenuSelect(%code)
{
	if(isObject(CurServerMenu))
		deleteObject(CurServerMenu);
	$KM::active = false;

	// "Back" on the selected-player menu (server: Admin.cs "deselect")
	// - drop the player-list highlight along with the selection
	if(%code == "deselect")
		$KM::selId = "";

	remoteEval(2048, menuSelect, %code);
}

// ============================================
// Hudbot mouse input
// ============================================

function onMouseActive(%isActive)
{
	$KM::mouseOn = %isActive;
	if(!%isActive)
	{
		$KSlider::drag = false;    // cursor went away mid-drag
		$KSlider2::drag = false;
		$KSlider3::drag = false;
		KronosMenu::dragEnd();
		$KC::scroll = 0;           // chat jumps back to newest when cursor hides

		// drop any open text field (chat composer / bank amount / search) when
		// the cursor hides - it was opened via a click and has no cursor to
		// dismiss it (a key-bound beginSay manages its own blur on Enter/Esc)
		if(KronosInput::anyFocused())
			KronosInput::blur();

		// cursor gone (TAB / score closed) - dismiss an open NPC dialogue
		// and let the server know so it clears its side too
		if($KNPC::open != "")
		{
			remoteEval(2048, KNPCClose);
			$KNPC::open = "";
		}
	}
}

function onMouseMove(%x, %y)
{
	$KM::mouseX = %x;
	$KM::mouseY = %y;

	// live drag of the UI-scale / dmg-text / hue sliders, or a panel move
	if($KSlider::drag)
		KronosMenu::sliderSet(%x);
	else if($KSlider2::drag)
		KronosMenu::slider2Set(%x);
	else if($KSlider3::drag)
		KronosMenu::slider3Set(%x);
	else if($Drag::active)
		KronosMenu::dragMove(%x, %y);
}

function onMouseLMB(%isDown)
{
	$KM::lmbDown = %isDown;

	if(!%isDown)
	{
		$KSlider::drag = false;   // release ends any slider/panel drag
		$KSlider2::drag = false;
		$KSlider3::drag = false;
		KronosMenu::dragEnd();
		return;
	}

	// Scale widget expand/collapse toggle (checked before the slider
	// tracks so the [-] box isn't swallowed by a hit band)
	if($KM::mouseOn && $KM::enabled && KronosMenu::scaleToggleClick($KM::mouseX, $KM::mouseY))
		return;

	// UI-scale slider takes priority (it sits clear of the panels, at
	// top-center, and is only live while the cursor is up)
	if($KM::mouseOn && $KM::enabled && KronosMenu::sliderHit($KM::mouseX, $KM::mouseY))
	{
		$KSlider::drag = true;
		KronosMenu::sliderSet($KM::mouseX);
		return;
	}

	// Damage-text size slider (row 2 of the same widget)
	if($KM::mouseOn && $KM::enabled && KronosMenu::slider2Hit($KM::mouseX, $KM::mouseY))
	{
		$KSlider2::drag = true;
		KronosMenu::slider2Set($KM::mouseX);
		return;
	}

	// Theme preset chips + hue bar (row 3 of the same widget)
	if($KM::mouseOn && $KM::enabled && KronosMenu::themeClick($KM::mouseX, $KM::mouseY))
		return;
	if($KM::mouseOn && $KM::enabled && KronosMenu::slider3Hit($KM::mouseX, $KM::mouseY))
	{
		$KSlider3::drag = true;
		KronosMenu::slider3Set($KM::mouseX);
		return;
	}

	// Chat A-/A+ text-size buttons (click, not drag) - check before the
	// drag handles so the buttons aren't swallowed by the box move handle
	if($KM::mouseOn && $KM::enabled && KronosChat::handleClick($KM::mouseX, $KM::mouseY))
		return;

	// NPC dialogue option rows (click) - before the drag handles
	if($KM::mouseOn && $KM::enabled && KronosNPC::handleClick($KM::mouseX, $KM::mouseY))
		return;

	// session-stats panel chips (Reset / hide) - before its move handle
	if($KM::mouseOn && $KM::enabled && KronosStats::handleClick($KM::mouseX, $KM::mouseY))
		return;

	// Grab a panel by its title bar to move it (works in menu and shop)
	if($KM::mouseOn && $KM::enabled)
	{
		%grab = KronosMenu::dragHit($KM::mouseX, $KM::mouseY);
		if(%grab != "")
		{
			KronosMenu::dragStart(%grab, $KM::mouseX, $KM::mouseY);
			return;
		}
	}

	// Kronos shop/inventory screen takes priority (KronosShop.cs)
	if($KS::open != "" && $KS::open != false)
	{
		KronosShop::handleClick($KM::mouseX, $KM::mouseY);
		return;
	}

	if(!$KM::active || !$KM::enabled)
		return;
	KronosMenu::handleClick($KM::mouseX, $KM::mouseY);
}

// Hit-test a click against the fixed row slots. $KML is refreshed
// every frame by the renderer while the menu is open.
function KronosMenu::handleClick(%x, %y)
{
	if($KML::rowH < 1)
		return;

	// menu option rows (left panel - dynamic width, set by render)
	if(%x >= $KML::mx + $KML::pad && %x < $KML::mx + $KML::wMenu - $KML::pad
		&& %y >= $KML::menuRowY0)
	{
		%row = floor((%y - $KML::menuRowY0) / $KML::rowH);
		// $KM::MaxZones (15) used to gate this as well, and it was simply wrong: the
		// renderer draws, tints and HOVER-HIGHLIGHTS all $KM::count rows, so on a long
		// menu (a repack's 30+ mission types) rows 16+ lit up under the cursor and then
		// swallowed the click. $KM::count is the real bound and the loop above already
		// applies it. Rows drawn past the bottom of the screen are still unreachable --
		// that is a paging problem, fixed at the source in base\scripts\admin.cs, which
		// now sends the mission-type list 7 at a time like the mission list always has.
		if(%row < $KM::count)
			KronosMenu::clickOption(%row);
		return;
	}

	// player list rows (right panel - dynamic width, set by render)
	if(%x >= $KML::px + $KML::pad && %x < $KML::px + $KML::wPlayers - $KML::pad
		&& %y >= $KML::plRowY0)
	{
		%row = floor((%y - $KML::plRowY0) / $KML::rowH);
		if(%row < $KM::plCount && %row < $KM::MaxPRows)
		{
			// VOICE-CHAT: a click on the right-edge mute chip toggles that
			// player's voice mute instead of selecting the row.
			if($KM::muteUi && $KML::muteW > 0
				&& %x >= $KML::px + $KML::wPlayers - $KML::pad - $KML::muteW)
				KronosMenu::clickMute(%row);
			else
				KronosMenu::clickPlayer(%row);
		}
	}
}

function KronosMenu::clickOption(%idx)
{
	if(!$KM::active || !$KM::enabled)
		return;
	if(%idx < 0 || %idx >= $KM::count)
		return;
	if($KM::code[%idx] == "")
		return;

	clientMenuSelect($KM::code[%idx]);
}

// Same message the stock scoreboard click sent (FearGuiScoreList).
function KronosMenu::clickPlayer(%idx)
{
	if(!$KM::active || !$KM::enabled)
		return;
	if(%idx < 0 || %idx >= $KM::plCount)
		return;
	%id = $KM::plId[%idx];
	if(%id == "" || %id == -1)
		return;
	$KM::selId = %id;

	remoteEval(2048, SelectClient, %id);
}

// VOICE-CHAT: toggle voice mute for the row's player. Mutes persist by player
// name across sessions (voiceChatClient.cpp name-hash prefs); the chip in
// render reads the live state back through VoiceChat::isMuted.
function KronosMenu::clickMute(%idx)
{
	if(!$KM::active || !$KM::enabled)
		return;
	if($VoiceChat::api == "")
		return;
	if(%idx < 0 || %idx >= $KM::plCount)
		return;
	%id = $KM::plId[%idx];
	if(%id == "" || %id == -1)
		return;
	if($KM::plName[%idx] == $PCFG::Name)
		return;
	VoiceChat::mute(%id);
}

// ============================================
// Server data handlers
// ============================================

// Player list rows (KronosHUD_Server.cs remoteKMGetPlayers)
function remoteKMPlayer(%server, %idx, %id, %lvl, %remort, %class, %name, %zone)
{
	if(%server != 2048)
		return;
	$KM::plId[%idx] = %id;
	$KM::plLvl[%idx] = %lvl;
	$KM::plRL[%idx] = %remort;
	$KM::plClass[%idx] = %class;
	$KM::plName[%idx] = %name;
	$KM::plZone[%idx] = %zone;   // location column (older servers omit it -> "")
}

function remoteKMPlayerCount(%server, %sent, %total)
{
	if(%server != 2048)
		return;
	$KM::plCount = %sent;
	$KM::plTotal = %total;
	$KM::plDirty = true;
	// SCOREBOARD-REFRESH: stamp the Kronos push so scoreTick's base-server
	// fallback (cfgPushBaseScoreRows) never overwrites a live Kronos roster.
	$KM::kmFeedTime = GetSimTime();
}

// Character info lines. Stock base client.cs writes these into the
// (now hidden) InfoCtrlBox; we capture them for the info panel
// instead. The server sends them for own stats (Game::menuRequest)
// and for a selected player (remoteSelectClient).
function remoteSetInfoLine(%server, %lineNum, %text)
{
	if(%server != 2048)
		return;
	if(%lineNum < 1 || %lineNum > 6)
		return;
	$KM::info[%lineNum] = %text;
	$KM::infoDirty = true;
}

// ============================================
// ScriptGL rendering (panels draw UNDER the dialog, but every
// stock dialog control is off-screen, so nothing covers them)
// ============================================

function KronosMenu::render(%dimensions)
{
	if(!$KM::active || !$KM::enabled)
	{
		// nothing drawn -> clear drag rects so a stale rect can't be grabbed
		// (the shop, if open, re-stashes its own panes right after this)
		$Panel::menuW = 0;
		$Panel::plW = 0;
		$Panel::infoShown = false;
		return;
	}

	%sw = getword(%dimensions, 0);
	%sh = getword(%dimensions, 1);

	KronosMenu::computeLayout(%sw, %sh);
	%pad = $KML::pad;
	%w = $KML::w;
	%titleH = $KML::titleH;
	%rowH = $KML::rowH;
	%y = $KML::y;

	%chipW = floor(%rowH * 1.1);
	%fontTitle = floor(%titleH * 0.62);
	%fontItem = floor(%rowH * 0.62);

	// Auto-width: widen the menu panel (up to 45% of screen - the
	// player panel starts at 54%) to fit the longest item label;
	// beyond that, shrink the item font to fit. Label widths are
	// measured once per menu (and re-measured on resolution change).
	if($KM::measureDirty || $KM::measuredFont != %fontItem)
	{
		glSetFont("Verdana", %fontItem, $GLEX_SMOOTH, 0);
		%maxText = 0;
		for(%i = 0; %i < $KM::count; %i++)
		{
			%tw = getword(glGetStringDimensions($KM::label[%i]), 0);
			if(%tw > %maxText)
				%maxText = %tw;
		}
		$KM::menuTextW = %maxText;
		$KM::measuredFont = %fontItem;
		$KM::measureDirty = false;
	}

	%fixed = (%pad * 2) + %chipW + floor(%pad * 0.7);
	%needW = %fixed + $KM::menuTextW + %pad;
	%wMax = floor(%sw * 0.45);
	%wm = %w;
	if(%needW > %wm)
		%wm = %needW;
	if(%wm > %wMax)
		%wm = %wMax;
	$KML::wMenu = %wm;

	%fontItemM = %fontItem;
	if(%needW > %wMax && $KM::menuTextW > 0)
	{
		%avail = %wMax - %fixed - %pad;
		%fontItemM = floor(%fontItem * %avail / $KM::menuTextW);
		if(%fontItemM < 9)
			%fontItemM = 9;
	}

	// Player panel auto-width: measure the widest name / level / class /
	// location; columns are packed from these widths below. Capped on screen.
	if($KM::plDirty || $KM::plMeasuredFont != %fontItem)
	{
		glSetFont("Verdana", %fontItem, $GLEX_SMOOTH, 0);
		%nw = 0;
		%lw = 0;
		%cw = 0;
		%zw = 0;
		for(%i = 0; %i < $KM::plCount; %i++)
		{
			%t = getword(glGetStringDimensions($KM::plName[%i]), 0);
			if(%t > %nw)
				%nw = %t;
			%lvText = $KM::lvPrefix @ $KM::plLvl[%i];
			if($KM::plRL[%i] > 0)
				%lvText = %lvText @ " R" @ $KM::plRL[%i];
			%t = getword(glGetStringDimensions(%lvText), 0);
			if(%t > %lw)
				%lw = %t;
			%t = getword(glGetStringDimensions($KM::plClass[%i]), 0);
			if(%t > %cw)
				%cw = %t;
			%t = getword(glGetStringDimensions($KM::plZone[%i]), 0);
			if(%t > %zw)
				%zw = %t;
		}
		$KM::plNameW = %nw;
		$KM::plLvW = %lw;
		$KM::plClW = %cw;
		$KM::plZnW = %zw;
		$KM::plMeasuredFont = %fontItem;
		$KM::plDirty = false;
	}

	// content-packed columns: each column starts right after the widest text
	// of the previous one plus a fixed gap - no fraction slots, so long names
	// can't overlap the next column and short ones don't leave a huge hole
	%colGap = %pad * 2;
	// VOICE-CHAT (mute chips): a right-edge mute button per player row, only
	// when the running exe exports the voice API ($VoiceChat::api, set by
	// VoiceChat_consoleInit) - under an older exe nothing is drawn or clickable.
	$KM::muteUi = ($VoiceChat::api != "");
	$KML::muteW = 0;
	if($KM::muteUi)
		$KML::muteW = floor(%rowH * 2.4);
	%wp = %pad + $KM::plNameW + %colGap + $KM::plLvW + %colGap + $KM::plClW + %colGap + $KM::plZnW + %pad;
	if($KM::muteUi)
		%wp = %wp + %colGap + $KML::muteW;
	if(%wp < %w)
		%wp = %w;
	%wpMax = floor(%sw * 0.60);
	if(%wp > %wpMax)
		%wp = %wpMax;
	$KML::wPlayers = %wp;

	// stash panel rects for drag hit-testing (title bar = top titleH)
	$Panel::menuX = $KML::mx;   $Panel::menuY = %y;         $Panel::menuW = %wm;  $Panel::menuTH = %titleH;
	$Panel::plX   = $KML::px;   $Panel::plY   = $KML::plY;   $Panel::plW   = %wp;  $Panel::plTH   = %titleH;
	$Panel::infoShown = false;

	// hovered row (per-panel, same math as handleClick)
	%hovRow = -1;
	%hovPanel = "";
	if($KM::mouseOn && %rowH >= 1)
	{
		if($KM::mouseX >= $KML::mx + %pad && $KM::mouseX < $KML::mx + %wm - %pad
			&& $KM::mouseY >= $KML::menuRowY0)
		{
			%hovPanel = "menu";
			%hovRow = floor(($KM::mouseY - $KML::menuRowY0) / %rowH);
		}
		else if($KM::mouseX >= $KML::px + %pad && $KM::mouseX < $KML::px + %wp - %pad
			&& $KM::mouseY >= $KML::plRowY0)
		{
			%hovPanel = "players";
			%hovRow = floor(($KM::mouseY - $KML::plRowY0) / %rowH);
		}
	}

	// extra "+N more" row on the player panel when the list overflows
	%pRows = $KM::plCount;
	%overflow = 0;
	if($KM::plTotal > $KM::plCount)
		%overflow = 1;
	if(%pRows < 1)
		%pRows = 1;

	%mh = %titleH + (%rowH * $KM::count) + %pad;
	%ph = %titleH + (%rowH * (%pRows + %overflow)) + %pad;

	// ---- Pass 1: all rectangles (texture state stays off) ----
	glDisable($GL_TEXTURE_2D);
	glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);

	KronosMenu::drawPanelBody($KML::mx, %y, %wm, %mh, %pad, %titleH);
	KronosMenu::drawPanelBody($KML::px, $KML::plY, %wp, %ph, %pad, %titleH);

	// menu row tints + hover + hotkey chips
	%iy = $KML::rowY0;
	for(%i = 0; %i < $KM::count; %i++)
	{
		if(%hovPanel == "menu" && %i == %hovRow)
		{
			glColor4ub($KT::hvR, $KT::hvG, $KT::hvB, 55);
			glRectangle($KML::mx + 2, %iy, %wm - 4, %rowH);
		}
		else
		{
			%half = floor(%i / 2);
			if(%i - (%half * 2) == 1)
			{
				glColor4ub(255, 255, 255, 9);
				glRectangle($KML::mx + 2, %iy, %wm - 4, %rowH);
			}
		}
		glColor4ub($KT::chR, $KT::chG, $KT::chB, 150);
		glRectangle($KML::mx + %pad, %iy + 2, %chipW, %rowH - 4);
		%iy += %rowH;
	}

	// player row tints + hover + selection highlight
	%iy = $KML::plRowY0;
	for(%i = 0; %i < $KM::plCount; %i++)
	{
		if($KM::selId != "" && $KM::plId[%i] == $KM::selId)
		{
			glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, 70);
			glRectangle($KML::px + 2, %iy, %wp - 4, %rowH);
		}
		else if(%hovPanel == "players" && %i == %hovRow)
		{
			glColor4ub($KT::hvR, $KT::hvG, $KT::hvB, 55);
			glRectangle($KML::px + 2, %iy, %wp - 4, %rowH);
		}
		else
		{
			%half = floor(%i / 2);
			if(%i - (%half * 2) == 1)
			{
				glColor4ub(255, 255, 255, 9);
				glRectangle($KML::px + 2, %iy, %wp - 4, %rowH);
			}
		}
		// VOICE-CHAT: mute chip background. Red while muted; near-invisible
		// until hovered so the board stays clean. Own row gets no chip.
		if($KM::muteUi && $KM::plId[%i] != "" && $KM::plName[%i] != $PCFG::Name)
		{
			%mzX = $KML::px + %wp - %pad - $KML::muteW;
			%overChip = (%hovPanel == "players" && %i == %hovRow && $KM::mouseX >= %mzX);
			if(VoiceChat::isMuted($KM::plId[%i]))
			{
				if(%overChip)
					glColor4ub(200, 60, 60, 140);
				else
					glColor4ub(200, 60, 60, 95);
			}
			else if(%overChip)
				glColor4ub(255, 255, 255, 60);
			else
				glColor4ub(255, 255, 255, 18);
			glRectangle(%mzX, %iy + 2, $KML::muteW - 2, %rowH - 4);
		}
		%iy += %rowH;
	}

	// character info panel body (suppressed while the KronosHUD item
	// examine overlay occupies this spot - see onPostDraw below)
	%hasInfo = false;
	if($KM::info[1] != "")
		%hasInfo = true;
	if(kronos::simAge($KH::exTime) < 10.0)
		%hasInfo = false;
	if(%hasInfo)
	{
		%infoLines = 0;
		for(%i = 1; %i <= 6; %i++)
			if($KM::info[%i] != "")
				%infoLines++;

		// auto-width: fit the longest info line (panel stays centered)
		%fontInfo = floor($KML::lineH * 0.78);
		if($KM::infoDirty || $KM::infoMeasuredFont != %fontInfo)
		{
			glSetFont("Verdana", %fontInfo, $GLEX_SMOOTH, 0);
			%mwi = 0;
			for(%i = 1; %i <= 6; %i++)
			{
				if($KM::info[%i] == "")
					continue;
				%t = getword(glGetStringDimensions($KM::info[%i]), 0);
				if(%t > %mwi)
					%mwi = %t;
			}
			$KM::infoTextW = %mwi;
			$KM::infoMeasuredFont = %fontInfo;
			$KM::infoDirty = false;
		}
		%wi = $KM::infoTextW + (%pad * 2);
		if(%wi < %w)
			%wi = %w;
		%wiMax = floor(%sw * 0.6);
		if(%wi > %wiMax)
			%wi = %wiMax;
		if($pref::Kronos::infoX == "c" || $pref::Kronos::infoX == "")
			%ixA = floor((%sw - %wi) / 2);
		else
			%ixA = floor($pref::Kronos::infoX * %sw);

		%ih = ($KML::lineH * %infoLines) + (%pad * 2);
		KronosMenu::drawPanelBody(%ixA, $KML::iy, %wi, %ih, %pad, 0);

		// stash for drag (whole info box is the drag handle)
		$Panel::infoX = %ixA;
		$Panel::infoY = $KML::iy;
		$Panel::infoW = %wi;
		$Panel::infoH = %ih;
		$Panel::infoShown = true;
	}

	// ---- Pass 2: all text ----
	// menu title + items
	glColor4ub(235, 240, 255, 245);
	glSetFont("Verdana", %fontTitle, $GLEX_SMOOTH, 1);
	glDrawString($KML::mx + %pad, %y + floor(%titleH * 0.16), $KM::title);

	glSetFont("Verdana", %fontItemM, $GLEX_SMOOTH, 0);
	%iy = $KML::rowY0;
	for(%i = 0; %i < $KM::count; %i++)
	{
		%ty = %iy + floor((%rowH - %fontItemM) / 2) - 1;
		glColor4ub(255, 255, 255, 235);
		glDrawString($KML::mx + %pad + floor(%chipW * 0.32), %ty, $KM::key[%i]);
		glColor4ub(225, 230, 240, 225);
		glDrawString($KML::mx + %pad + %chipW + floor(%pad * 0.7), %ty, $KM::label[%i]);
		%iy += %rowH;
	}

	// player list title + rows
	glColor4ub(235, 240, 255, 245);
	glSetFont("Verdana", %fontTitle, $GLEX_SMOOTH, 1);
	glDrawString($KML::px + %pad, $KML::plY + floor(%titleH * 0.16), "Players (" @ $KM::plTotal @ ")");

	glSetFont("Verdana", %fontItem, $GLEX_SMOOTH, 0);
	%colGap = %pad * 2;
	%lvX = $KML::px + %pad + $KM::plNameW + %colGap;
	%clX = %lvX + $KM::plLvW + %colGap;
	%znX = %clX + $KM::plClW + %colGap;

	%iy = $KML::plRowY0;
	if($KM::plCount < 1)
	{
		glColor4ub(160, 170, 190, 180);
		glDrawString($KML::px + %pad, %iy + floor((%rowH - %fontItem) / 2) - 1, "(no players)");
	}
	for(%i = 0; %i < $KM::plCount; %i++)
	{
		%ty = %iy + floor((%rowH - %fontItem) / 2) - 1;
		glColor4ub(255, 255, 255, 235);
		glDrawString($KML::px + %pad, %ty, $KM::plName[%i]);

		%lvText = $KM::lvPrefix @ $KM::plLvl[%i];
		if($KM::plRL[%i] > 0)
			%lvText = %lvText @ " R" @ $KM::plRL[%i];
		glColor4ub($KT::txR, $KT::txG, $KT::txB, 220);
		glDrawString(%lvX, %ty, %lvText);

		glColor4ub(200, 210, 225, 210);
		glDrawString(%clX, %ty, $KM::plClass[%i]);

		glColor4ub(160, 210, 170, 210);
		glDrawString(%znX, %ty, $KM::plZone[%i]);
		%iy += %rowH;
	}
	// VOICE-CHAT: mute chip labels, own smaller font so the chips read as
	// buttons rather than another data column.
	if($KM::muteUi && $KM::plCount > 0)
	{
		%fontMute = floor(%fontItem * 0.78);
		if(%fontMute < 9)
			%fontMute = 9;
		glSetFont("Verdana", %fontMute, $GLEX_SMOOTH, 0);
		%mzX = $KML::px + %wp - %pad - $KML::muteW;
		%muteTxW = getword(glGetStringDimensions("mute"), 0);
		%mutedTxW = getword(glGetStringDimensions("muted"), 0);
		%iy = $KML::plRowY0;
		for(%i = 0; %i < $KM::plCount; %i++)
		{
			if($KM::plId[%i] != "" && $KM::plName[%i] != $PCFG::Name)
			{
				%ty = %iy + floor((%rowH - %fontMute) / 2) - 1;
				if(VoiceChat::isMuted($KM::plId[%i]))
				{
					glColor4ub(255, 235, 235, 235);
					glDrawString(%mzX + floor(($KML::muteW - %mutedTxW) / 2), %ty, "muted");
				}
				else
				{
					glColor4ub(210, 220, 235, 150);
					glDrawString(%mzX + floor(($KML::muteW - %muteTxW) / 2), %ty, "mute");
				}
			}
			%iy += %rowH;
		}
		glSetFont("Verdana", %fontItem, $GLEX_SMOOTH, 0);
	}
	if(%overflow)
	{
		glColor4ub(160, 170, 190, 180);
		glDrawString($KML::px + %pad, %iy + floor((%rowH - %fontItem) / 2) - 1, "+ " @ ($KM::plTotal - $KM::plCount) @ " more...");
	}

	// character info text
	if(%hasInfo)
	{
		%fontInfo = floor($KML::lineH * 0.78);
		%ty = $KML::iy + %pad;
		for(%i = 1; %i <= 6; %i++)
		{
			if($KM::info[%i] == "")
				continue;
			if(%i == 1)
			{
				glColor4ub($KT::txR, $KT::txG, $KT::txB, 245);
				glSetFont("Verdana", %fontInfo, $GLEX_SMOOTH, 1);
			}
			else
			{
				glColor4ub(225, 230, 240, 225);
				glSetFont("Verdana", %fontInfo, $GLEX_SMOOTH, 0);
			}
			glDrawString(%ixA + %pad, %ty, $KM::info[%i]);
			%ty += $KML::lineH;
		}
	}
}

// Panel chrome shared by all three panels (rect pass only).
// %titleH = 0 means no title underline.
function KronosMenu::drawPanelBody(%x, %y, %w, %h, %pad, %titleH)
{
	// body
	glColor4ub($KT::bgR, $KT::bgG, $KT::bgB, 238);
	glRectangle(%x, %y, %w, %h);

	// accent border: top bar + thin sides/bottom
	glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, 220);
	glRectangle(%x, %y, %w, 2);
	glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, 90);
	glRectangle(%x, %y + %h - 1, %w, 1);
	glRectangle(%x, %y, 1, %h);
	glRectangle(%x + %w - 1, %y, 1, %h);

	if(%titleH > 0)
	{
		glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, 140);
		glRectangle(%x + %pad, %y + %titleH - 2, %w - (%pad * 2), 1);
	}
}

// ============================================
// UI-scale slider (drag to resize the whole GUI)
// ============================================
// A small click-and-drag widget at top-center, shown whenever the GUI
// cursor is up (TAB menu or shop open). Dragging it sets
// $pref::Kronos::UiScale live, so the menu/shop/info panels resize as
// you slide. Geometry computed here is stashed for sliderHit/sliderSet.
function KronosMenu::renderSlider(%sw, %sh)
{
	if(!$KM::enabled || !$KM::mouseOn)
	{
		$Panel::uisShown = false;
		return;
	}

	// The slider widget is sized by resolution ONLY (the reference base,
	// without the UiScale knob) - so dragging it doesn't resize the slider
	// itself under the cursor, only the rest of the GUI.
	%ref = $pref::Kronos::UiRefH;
	if(%ref == "" || %ref < 100)
		%ref = 1080;
	%k = %ref / %sh;
	if(%k > 1.0)
		%k = 1.0;

	%w     = floor(%sw * 0.18 * %k);
	%lineH = floor(%sh * 0.030 * %k);
	%pad   = floor(%sw * 0.008 * %k);
	if(%pad < 4)
		%pad = 4;
	%h = (%lineH * 6) + (%pad * 2);   // 3 rows: UI Scale + Dmg Text + Theme
	// movable position: X defaults to centered ("c") until dragged
	if($pref::Kronos::sliderX == "c" || $pref::Kronos::sliderX == "")
		%x = floor((%sw - %w) / 2);
	else
		%x = floor($pref::Kronos::sliderX * %sw);
	%y = floor($pref::Kronos::sliderY * %sh);
	%font = floor(%lineH * 0.62);
	if(%font < 9)
		%font = 9;

	// ---- closed (default): draw nothing. The panel is opened from the
	// "UI" button in the chat window (KronosChat toggles $KM::scaleOpen).
	if(!$KM::scaleOpen)
	{
		$Panel::uisShown = false;
		$KScale::btnW = 0;   // no on-screen expand target while closed
		$KSlider::hitX0 = 0;   $KSlider::hitX1 = 0;
		$KSlider2::hitX0 = 0;  $KSlider2::hitX1 = 0;
		$KSlider3::hitX0 = 0;  $KSlider3::hitX1 = 0;
		$KTheB::w = 0;
		return;
	}

	// expanded: a small [-] box at the top-right collapses it again
	%clW = floor(%lineH * 0.8);
	$KScale::btnX = %x + %w - %clW - floor(%pad / 2);
	$KScale::btnY = %y + floor(%pad / 2);
	$KScale::btnW = %clW;
	$KScale::btnH = %clW;

	// stash the widget rect as a move handle (the track is grabbed first
	// for scale-adjust in onMouseLMB, so the rest of the box moves it)
	$Panel::uisX = %x;  $Panel::uisY = %y;  $Panel::uisW = %w;  $Panel::uisH = %h;
	$Panel::uisShown = true;

	// track + knob
	%trackX = %x + %pad;
	%trackW = %w - (%pad * 2);
	%trackH = floor(%lineH * 0.28);
	if(%trackH < 3)
		%trackH = 3;
	%trackY = %y + %pad + %lineH + floor((%lineH - %trackH) / 2);

	%min = $KSlider::min;
	%max = $KSlider::max;
	%val = $pref::Kronos::UiScale;
	if(%val == "")
		%val = 1.0;
	if(%val < %min)
		%val = %min;
	if(%val > %max)
		%val = %max;
	%frac = (%val - %min) / (%max - %min);

	%knobW = floor(%lineH * 0.45);
	if(%knobW < 6)
		%knobW = 6;
	%knobH = floor(%lineH * 0.95);
	%knobX = %trackX + floor((%trackW - %knobW) * %frac);
	%knobY = %trackY + floor(%trackH / 2) - floor(%knobH / 2);

	// stash hit geometry (generous band around the track for easy grab)
	$KSlider::trackX = %trackX;
	$KSlider::trackW = %trackW;
	$KSlider::hitX0  = %trackX - %knobW;
	$KSlider::hitX1  = %trackX + %trackW + %knobW;
	$KSlider::hitY0  = %knobY - %pad;
	$KSlider::hitY1  = %knobY + %knobH + %pad;

	// hot when dragging or hovering
	%hot = false;
	if($KSlider::drag)
		%hot = true;
	else if($KM::mouseX >= $KSlider::hitX0 && $KM::mouseX <= $KSlider::hitX1
		&& $KM::mouseY >= $KSlider::hitY0 && $KM::mouseY <= $KSlider::hitY1)
		%hot = true;

	// ---- rect pass ----
	glDisable($GL_TEXTURE_2D);
	glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);

	KronosMenu::drawPanelBody(%x, %y, %w, %h, %pad, 0);

	glColor4ub(0, 0, 0, 150);
	glRectangle(%trackX, %trackY, %trackW, %trackH);
	glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, 180);
	glRectangle(%trackX, %trackY, floor(%trackW * %frac), %trackH);

	if(%hot)
		glColor4ub($KT::hbR, $KT::hbG, $KT::hbB, 245);
	else
		glColor4ub($KT::hvR, $KT::hvG, $KT::hvB, 220);
	glRectangle(%knobX, %knobY, %knobW, %knobH);

	// ---- text pass ----
	glColor4ub(235, 240, 255, 240);
	glSetFont("Verdana", %font, $GLEX_SMOOTH, 1);
	glDrawString(%x + %pad, %y + floor(%pad * 0.5), "UI Scale  " @ floor((%val * 100) + 0.5) @ "%");

	// ---- damage-text size slider (row 2) ----
	// Same widget, second track: sets $pref::Kronos::dmgTextScale live (the
	// FloatStyle3 pop-damage size, independent of the UI scale above).
	%track2Y = %y + %pad + (%lineH * 3) + floor((%lineH - %trackH) / 2);

	%min2 = 0.5;
	%max2 = 2.5;
	%val2 = $pref::Kronos::dmgTextScale;
	if(%val2 == "")
		%val2 = 1.0;
	if(%val2 < %min2)
		%val2 = %min2;
	if(%val2 > %max2)
		%val2 = %max2;
	%frac2 = (%val2 - %min2) / (%max2 - %min2);

	%knob2X = %trackX + floor((%trackW - %knobW) * %frac2);
	%knob2Y = %track2Y + floor(%trackH / 2) - floor(%knobH / 2);

	$KSlider2::min = %min2;
	$KSlider2::max = %max2;
	$KSlider2::trackX = %trackX;
	$KSlider2::trackW = %trackW;
	$KSlider2::hitX0  = %trackX - %knobW;
	$KSlider2::hitX1  = %trackX + %trackW + %knobW;
	$KSlider2::hitY0  = %knob2Y - %pad;
	$KSlider2::hitY1  = %knob2Y + %knobH + %pad;

	%hot2 = false;
	if($KSlider2::drag)
		%hot2 = true;
	else if($KM::mouseX >= $KSlider2::hitX0 && $KM::mouseX <= $KSlider2::hitX1
		&& $KM::mouseY >= $KSlider2::hitY0 && $KM::mouseY <= $KSlider2::hitY1)
		%hot2 = true;

	// glDrawString above re-enabled textures - rects need them off again
	glDisable($GL_TEXTURE_2D);
	glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);

	glColor4ub(0, 0, 0, 150);
	glRectangle(%trackX, %track2Y, %trackW, %trackH);
	glColor4ub(200, 150, 60, 180);
	glRectangle(%trackX, %track2Y, floor(%trackW * %frac2), %trackH);

	if(%hot2)
		glColor4ub(255, 210, 110, 245);
	else
		glColor4ub(230, 180, 80, 220);
	glRectangle(%knob2X, %knob2Y, %knobW, %knobH);

	glColor4ub(235, 240, 255, 240);
	glSetFont("Verdana", %font, $GLEX_SMOOTH, 1);
	glDrawString(%x + %pad, %y + %pad + (%lineH * 2) + floor(%pad * 0.2), "Dmg Text  " @ floor((%val2 * 100) + 0.5) @ "%");

	// ---- theme row (row 3): preset chips + custom hue bar ----
	// Chips apply KTheme::set(blue/rpg/green); dragging the rainbow bar is
	// the "color wheel" - KTheme::hue(0-360) generates a full custom palette.
	%tLabY = %y + %pad + (%lineH * 4);
	%chipH = floor(%lineH * 0.7);
	%chipY = %tLabY + floor((%lineH - %chipH) / 2);
	%chipW = floor(%trackW * 0.14);
	%chipGap = floor(%pad * 0.8);
	%chipX0 = %x + %pad + floor(%trackW * 0.34);

	%hueY = %y + %pad + (%lineH * 5) + floor((%lineH - %trackH) / 2);
	%hueH = %trackH + 2;

	// stash hit geometry
	$KTheB::y = %chipY;  $KTheB::h = %chipH;  $KTheB::w = %chipW;
	$KTheB::x0 = %chipX0;  $KTheB::gap = %chipGap;
	$KSlider3::trackX = %trackX;
	$KSlider3::trackW = %trackW;
	$KSlider3::hitX0 = %trackX - %knobW;
	$KSlider3::hitX1 = %trackX + %trackW + %knobW;
	$KSlider3::hitY0 = %hueY - %pad;
	$KSlider3::hitY1 = %hueY + %hueH + %pad;

	glDisable($GL_TEXTURE_2D);
	glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);

	// preset chips (their own accent colors; white outline = active)
	%themes = "blue rpg green";
	%chipC[0] = "110 165 235";
	%chipC[1] = "200 165 110";
	%chipC[2] = "110 205 130";
	for(%ci = 0; %ci < 3; %ci++)
	{
		%cx = %chipX0 + (%ci * (%chipW + %chipGap));
		glColor4ub(getWord(%chipC[%ci], 0), getWord(%chipC[%ci], 1), getWord(%chipC[%ci], 2), 235);
		glRectangle(%cx, %chipY, %chipW, %chipH);
		if($pref::Kronos::theme == GetWord(%themes, %ci)
			|| ($pref::Kronos::theme == "" && %ci == 0))
		{
			glColor4ub(255, 255, 255, 245);
			glRectangle(%cx, %chipY, %chipW, 1);
			glRectangle(%cx, %chipY + %chipH - 1, %chipW, 1);
			glRectangle(%cx, %chipY, 1, %chipH);
			glRectangle(%cx + %chipW - 1, %chipY, 1, %chipH);
		}
	}

	// hue bar: 24 colored segments across 0-360
	%segW = %trackW / 24;
	for(%si = 0; %si < 24; %si++)
	{
		%rgb = KTheme::hsv(%si * 15, 0.65, 0.9);
		glColor4ub(getWord(%rgb, 0), getWord(%rgb, 1), getWord(%rgb, 2), 220);
		glRectangle(%trackX + floor(%si * %segW), %hueY, floor(%segW) + 1, %hueH);
	}
	// marker when a custom hue is active
	if(String::findSubStr($pref::Kronos::theme, "hue:") == 0)
	{
		%mh = String::getSubStr($pref::Kronos::theme, 4, 10);
		%mx = %trackX + floor(%trackW * (%mh / 360));
		glColor4ub(255, 255, 255, 245);
		glRectangle(%mx - 1, %hueY - 2, 3, %hueH + 4);
	}

	glColor4ub(235, 240, 255, 240);
	glSetFont("Verdana", %font, $GLEX_SMOOTH, 1);
	glDrawString(%x + %pad, %tLabY + floor(%pad * 0.2), "Theme");

	// collapse box [-] (top-right; geometry stashed above)
	glDisable($GL_TEXTURE_2D);
	glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);
	glColor4ub($KT::chR, $KT::chG, $KT::chB, 170);
	glRectangle($KScale::btnX, $KScale::btnY, $KScale::btnW, $KScale::btnH);
	glColor4ub(235, 240, 255, 235);
	glRectangle($KScale::btnX + 3, $KScale::btnY + floor($KScale::btnH / 2), $KScale::btnW - 6, 2);
}

// Collapsed pill click = expand; expanded [-] click = collapse.
function KronosMenu::scaleToggleClick(%x, %y)
{
	if($KScale::btnW < 1)
		return false;
	if(%x >= $KScale::btnX && %x < $KScale::btnX + $KScale::btnW
		&& %y >= $KScale::btnY && %y < $KScale::btnY + $KScale::btnH)
	{
		$KM::scaleOpen = !$KM::scaleOpen;
		return true;
	}
	return false;
}

// Theme preset chips: click applies. Returns true when consumed.
function KronosMenu::themeClick(%x, %y)
{
	if($KTheB::w < 1)
		return false;
	if(%y < $KTheB::y || %y >= $KTheB::y + $KTheB::h)
		return false;
	%themes = "blue rpg green";
	for(%ci = 0; %ci < 3; %ci++)
	{
		%cx = $KTheB::x0 + (%ci * ($KTheB::w + $KTheB::gap));
		if(%x >= %cx && %x < %cx + $KTheB::w)
		{
			KTheme::set(GetWord(%themes, %ci));
			return true;
		}
	}
	return false;
}

// Hue bar: map mouse x to 0-360 and apply the custom palette live.
function KronosMenu::slider3Hit(%x, %y)
{
	if($KSlider3::hitX1 <= $KSlider3::hitX0)
		return false;
	if(%x >= $KSlider3::hitX0 && %x <= $KSlider3::hitX1
		&& %y >= $KSlider3::hitY0 && %y <= $KSlider3::hitY1)
		return true;
	return false;
}

function KronosMenu::slider3Set(%x)
{
	if($KSlider3::trackW < 1)
		return;
	%frac = (%x - $KSlider3::trackX) / $KSlider3::trackW;
	if(%frac < 0)
		%frac = 0;
	if(%frac > 1)
		%frac = 1;
	KTheme::hue(%frac * 360);
}

// Is (x,y) on the damage-text slider? Geometry stashed by renderSlider.
function KronosMenu::slider2Hit(%x, %y)
{
	if($KSlider2::hitX1 <= $KSlider2::hitX0)
		return false;
	if(%x >= $KSlider2::hitX0 && %x <= $KSlider2::hitX1
		&& %y >= $KSlider2::hitY0 && %y <= $KSlider2::hitY1)
		return true;
	return false;
}

// Map a mouse x to a damage-text scale (snapped to 10% steps), apply live,
// and pop a sample float so the size can be judged while dragging.
function KronosMenu::slider2Set(%x)
{
	if($KSlider2::trackW < 1)
		return;
	%frac = (%x - $KSlider2::trackX) / $KSlider2::trackW;
	if(%frac < 0)
		%frac = 0;
	if(%frac > 1)
		%frac = 1;
	%val = $KSlider2::min + (%frac * ($KSlider2::max - $KSlider2::min));
	%val = floor((%val / 0.1) + 0.5) * 0.1;
	if(%val != $pref::Kronos::dmgTextScale)
	{
		$pref::Kronos::dmgTextScale = %val;
		KronosHUD::addFloat("123 DMG!", "attacker");   // live preview
	}
}

// Is (x,y) on the slider? Uses the geometry stashed by renderSlider.
function KronosMenu::sliderHit(%x, %y)
{
	if($KSlider::hitX1 <= $KSlider::hitX0)
		return false;
	if(%x >= $KSlider::hitX0 && %x <= $KSlider::hitX1
		&& %y >= $KSlider::hitY0 && %y <= $KSlider::hitY1)
		return true;
	return false;
}

// Map a mouse x to a scale value (snapped to 5% steps) and apply it live.
function KronosMenu::sliderSet(%x)
{
	if($KSlider::trackW < 1)
		return;
	%frac = (%x - $KSlider::trackX) / $KSlider::trackW;
	if(%frac < 0)
		%frac = 0;
	if(%frac > 1)
		%frac = 1;
	%val = $KSlider::min + (%frac * ($KSlider::max - $KSlider::min));
	%val = floor((%val / 0.05) + 0.5) * 0.05;   // snap to 0.05
	$pref::Kronos::UiScale = %val;
}

// ============================================
// Movable panels (drag a panel by its title bar)
// ============================================
// Panels are positioned from persisted screen fractions
// ($pref::Kronos::menuX/menuY, playersX/playersY, infoX/infoY) and store
// their on-screen rects each frame (in render). Grabbing a title bar
// starts a drag that rewrites those prefs live, so the move persists.

// Which panel's drag handle is under (x,y)?  "" if none.  Title bar =
// top titleH of the menu/players panels; the whole box for the info panel.
function KronosMenu::dragHit(%x, %y)
{
	// chat grip tab (explicit handle for the engine chat control)
	if($Panel::chatGripShown
		&& %x >= $Panel::chatGripX && %x < $Panel::chatGripX + $Panel::chatGripW
		&& %y >= $Panel::chatGripY && %y < $Panel::chatGripY + $Panel::chatGripH)
		return "chat";

	if($Panel::infoShown
		&& %x >= $Panel::infoX && %x < $Panel::infoX + $Panel::infoW
		&& %y >= $Panel::infoY && %y < $Panel::infoY + $Panel::infoH)
		return "info";

	if($Panel::menuW > 0
		&& %x >= $Panel::menuX && %x < $Panel::menuX + $Panel::menuW
		&& %y >= $Panel::menuY && %y < $Panel::menuY + $Panel::menuTH)
		return "menu";

	if($Panel::plW > 0
		&& %x >= $Panel::plX && %x < $Panel::plX + $Panel::plW
		&& %y >= $Panel::plY && %y < $Panel::plY + $Panel::plTH)
		return "players";

	// shop/inventory scrollbars (KronosShop.cs panes)
	if($Panel::sbInvShown
		&& %x >= $Panel::sbInvX && %x < $Panel::sbInvX + $Panel::sbInvW
		&& %y >= $Panel::sbInvY && %y < $Panel::sbInvY + $Panel::sbInvH)
		return "sbinv";
	if($Panel::sbStShown
		&& %x >= $Panel::sbStX && %x < $Panel::sbStX + $Panel::sbStW
		&& %y >= $Panel::sbStY && %y < $Panel::sbStY + $Panel::sbStH)
		return "sbst";

	// vhud HUD panels (HP/MP/XP, Lv/Gold, weapon bar) - whole panel is the
	// handle; rects come from the vhud render cache computed each onPreDraw
	for(%i = 0; %i < $Drag::hudN; %i++)
	{
		%nm = $Drag::hudName[%i];
		%rp = $vhud[%nm, render, pos];
		%rs = $vhud[%nm, render, size];
		%hx = getword(%rp, 0);
		%hy = getword(%rp, 1);
		%hw = getword(%rs, 0);
		%hh = getword(%rs, 1);
		if(%hw > 0 && %x >= %hx && %x < %hx + %hw && %y >= %hy && %y < %hy + %hh)
			return "hud" @ %i;
	}

	// chat overlay resize grip (bottom-right corner) - checked before the
	// body so the corner resizes and the rest of the box moves
	if($Panel::kchatSzShown
		&& %x >= $Panel::kchatSzX && %x < $Panel::kchatSzX + $Panel::kchatSzW
		&& %y >= $Panel::kchatSzY && %y < $Panel::kchatSzY + $Panel::kchatSzH)
		return "kchatsz";

	// chat scrollbar track (right gutter)
	if($Panel::kchatScrShown
		&& %x >= $Panel::kchatScrX && %x < $Panel::kchatScrX + $Panel::kchatScrW
		&& %y >= $Panel::kchatScrY && %y < $Panel::kchatScrY + $Panel::kchatScrTrackH)
		return "kchatscr";

	// custom chat overlay (KronosChat.cs) - whole box is the move handle
	if($Panel::kchatShown
		&& %x >= $Panel::kchatX && %x < $Panel::kchatX + $Panel::kchatW
		&& %y >= $Panel::kchatY && %y < $Panel::kchatY + $Panel::kchatH)
		return "kchat";

	// UI-scale slider widget - move handle (the track is grabbed earlier
	// in onMouseLMB for scale-adjust, so only the rest of it moves)
	if($Panel::uisShown
		&& %x >= $Panel::uisX && %x < $Panel::uisX + $Panel::uisW
		&& %y >= $Panel::uisY && %y < $Panel::uisY + $Panel::uisH)
		return "uislider";

	// NPC dialogue window - title bar moves it (option rows are clicked
	// earlier in onMouseLMB, so they aren't swallowed here)
	if($Panel::knpcShown
		&& %x >= $Panel::knpcX && %x < $Panel::knpcX + $Panel::knpcW
		&& %y >= $Panel::knpcY && %y < $Panel::knpcY + $Panel::knpcTH)
		return "knpcwin";

	// session-stats panel - whole panel is the move handle (its chips are
	// clicked earlier in onMouseLMB)
	if($Panel::kstatsShown
		&& %x >= $Panel::kstatsX && %x < $Panel::kstatsX + $Panel::kstatsW
		&& %y >= $Panel::kstatsY && %y < $Panel::kstatsY + $Panel::kstatsH)
		return "kstats";

	return "";
}

function KronosMenu::dragStart(%id, %x, %y)
{
	$Drag::id = %id;
	$Drag::active = true;
	if(%id == "menu")
	{
		$Drag::dx = %x - $Panel::menuX;
		$Drag::dy = %y - $Panel::menuY;
	}
	else if(%id == "players")
	{
		$Drag::dx = %x - $Panel::plX;
		$Drag::dy = %y - $Panel::plY;
	}
	else if(%id == "info")
	{
		$Drag::dx = %x - $Panel::infoX;
		$Drag::dy = %y - $Panel::infoY;
	}
	else if(String::getSubStr(%id, 0, 3) == "hud")
	{
		%i = String::getSubStr(%id, 3, 9);
		%rp = $vhud[$Drag::hudName[%i], render, pos];
		$Drag::dx = %x - getword(%rp, 0);
		$Drag::dy = %y - getword(%rp, 1);
	}
	else if(%id == "chat")
	{
		%cp = Control::getPosition("chatDisplayHud");
		$Drag::dx = %x - getword(%cp, 0);
		$Drag::dy = %y - getword(%cp, 1);
	}
	else if(%id == "kchat")
	{
		$Drag::dx = %x - $Panel::kchatX;
		$Drag::dy = %y - $Panel::kchatY;
	}
	else if(%id == "kchatsz")
	{
		// offset from the box's bottom-right corner, so it tracks the cursor
		$Drag::dx = %x - ($Panel::kchatX + $Panel::kchatW);
		$Drag::dy = %y - ($Panel::kchatY + $Panel::kchatH);
	}
	else if(%id == "uislider")
	{
		$Drag::dx = %x - $Panel::uisX;
		$Drag::dy = %y - $Panel::uisY;
	}
	else if(%id == "knpcwin")
	{
		$Drag::dx = %x - $Panel::knpcX;
		$Drag::dy = %y - $Panel::knpcY;
	}
	else if(%id == "kstats")
	{
		$Drag::dx = %x - $Panel::kstatsX;
		$Drag::dy = %y - $Panel::kstatsY;
	}
}

function KronosMenu::dragMove(%x, %y)
{
	if(!$Drag::active)
		return;
	%sw = getword($KM::dim, 0);
	%sh = getword($KM::dim, 1);
	if(%sw < 1 || %sh < 1)
		return;

	%fx = (%x - $Drag::dx) / %sw;
	%fy = (%y - $Drag::dy) / %sh;
	if(%fx < 0)    %fx = 0;
	if(%fx > 0.95) %fx = 0.95;
	if(%fy < 0)    %fy = 0;
	if(%fy > 0.96) %fy = 0.96;

	if($Drag::id == "menu")
	{
		$pref::Kronos::menuX = %fx;
		$pref::Kronos::menuY = %fy;
	}
	else if($Drag::id == "players")
	{
		$pref::Kronos::playersX = %fx;
		$pref::Kronos::playersY = %fy;
	}
	else if($Drag::id == "info")
	{
		$pref::Kronos::infoX = %fx;
		$pref::Kronos::infoY = %fy;
	}
	else if($Drag::id == "kchat")
	{
		$pref::Kronos::chatPosX = %fx;
		$pref::Kronos::chatPosY = %fy;
	}
	else if($Drag::id == "uislider")
	{
		$pref::Kronos::sliderX = %fx;
		$pref::Kronos::sliderY = %fy;
	}
	else if($Drag::id == "knpcwin")
	{
		$pref::Kronos::npcX = %fx;
		$pref::Kronos::npcY = %fy;
	}
	else if($Drag::id == "kstats")
	{
		$pref::Kronos::statsX = %fx;
		$pref::Kronos::statsY = %fy;
	}
	else if($Drag::id == "kchatsz")
	{
		// bottom-right corner: resize the WINDOW only - width from X,
		// height from Y (text size is separate, via the A-/A+ buttons).
		// The corner tracks the cursor.
		%right = %x - $Drag::dx;
		%bottom = %y - $Drag::dy;

		%wf = (%right - $Panel::kchatX) / %sw;
		if(%wf < 0.12) %wf = 0.12;
		if(%wf > 0.80) %wf = 0.80;
		$pref::Kronos::chatW = %wf;
		$KC::lastW = -1;        // width affects wrapping -> rewrap

		%hf = (%bottom - $Panel::kchatY) / %sh;
		if(%hf < 0.04) %hf = 0.04;
		if(%hf > 0.85) %hf = 0.85;
		$pref::Kronos::chatH = %hf;
	}
	else if($Drag::id == "kchatscr")
	{
		// scrollbar: map cursor Y on the track to a history scroll offset
		%vis = $Panel::kchatScrVisible;
		%tot = $Panel::kchatScrTotal;
		%max = %tot - %vis;
		if(%max > 0)
		{
			%th = $Panel::kchatScrTrackH;
			%thumbH = floor(%th * %vis / %tot);
			if(%thumbH < 14) %thumbH = 14;
			%travel = %th - %thumbH;
			if(%travel < 1) %travel = 1;
			%frac = (%y - floor(%thumbH / 2) - $Panel::kchatScrY) / %travel;
			if(%frac < 0) %frac = 0;
			if(%frac > 1) %frac = 1;
			$KC::scroll = floor(((1.0 - %frac) * %max) + 0.5);   // top = oldest
		}
	}
	else if($Drag::id == "sbinv" || $Drag::id == "sbst")
	{
		// shop/inventory scrollbar: cursor Y on the track -> list scroll
		// offset (top of track = top of list)
		if($Drag::id == "sbinv")
		{
			%trY = $Panel::sbInvY;  %trH = $Panel::sbInvH;
			%vis = $Panel::sbInvVis;  %tot = $Panel::sbInvTot;
		}
		else
		{
			%trY = $Panel::sbStY;  %trH = $Panel::sbStH;
			%vis = $Panel::sbStVis;  %tot = $Panel::sbStTot;
		}
		%max = %tot - %vis;
		if(%max > 0)
		{
			%thumbH = floor(%trH * %vis / %tot);
			if(%thumbH < 14) %thumbH = 14;
			%travel = %trH - %thumbH;
			if(%travel < 1) %travel = 1;
			%frac = (%y - floor(%thumbH / 2) - %trY) / %travel;
			if(%frac < 0) %frac = 0;
			if(%frac > 1) %frac = 1;
			%off = floor((%frac * %max) + 0.5);
			if($Drag::id == "sbinv")
				$KS::scroll[inv] = %off;
			else
				$KS::scroll[st] = %off;
		}
	}
	else if(String::getSubStr($Drag::id, 0, 3) == "hud")
	{
		// vhud panels store "x y" PERCENT and are recomputed by vhud, so
		// rewrite the pos + bust vhud's per-panel dimension cache
		%i = String::getSubStr($Drag::id, 3, 9);
		%nm = $Drag::hudName[%i];
		%posStr = (%fx * 100) @ " " @ (%fy * 100);
		$vhud[%nm, pos] = %posStr;
		$vhud[%nm, lastdimensions] = "";
		if(%nm == "kh_vitals")
			$pref::Kronos::vitalsPos = %posStr;
		else if(%nm == "kh_info")
			$pref::Kronos::infoHudPos = %posStr;
		else if(%nm == "kh_wbar")
			$pref::Kronos::wbarPos = %posStr;
	}
	else if($Drag::id == "chat")
	{
		// chat is an engine control - move it directly (pixels) and store
		// the position as fractions so it scales / persists
		%nx = %x - $Drag::dx;
		%ny = %y - $Drag::dy;
		if(%nx < 0) %nx = 0;
		if(%ny < 0) %ny = 0;
		if(%nx > %sw - 40) %nx = %sw - 40;
		if(%ny > %sh - 20) %ny = %sh - 20;
		Control::setPosition("chatDisplayHud", %nx, %ny);
		$pref::Kronos::chatX = %nx / %sw;
		$pref::Kronos::chatY = %ny / %sh;
	}
}

// Persist ALL prefs to disk NOW, instead of waiting for onExit() - which is skipped
// on a hard close / alt-F4 / crash, the usual reason HUD layout "doesn't save". Writes
// the same config\ClientPrefs.cs the boot loads, so panel positions, UI scale, chat
// layout, etc. survive across launches no matter how the client is closed. Called
// only on a real change (drag release / scale / chat set), so the small file write
// is unnoticeable. (Ported from the 1.40 KronosMenu.)
function KronosMenu::savePrefs()
{
	export("pref::*", "config\\ClientPrefs.cs", false);
}

function KronosMenu::dragEnd()
{
	%wasDragging = $Drag::active;   // true only if a panel/slider was actually moved
	$Drag::active = false;
	$Drag::id = "";
	if(%wasDragging)
		KronosMenu::savePrefs();    // a panel moved -> persist its new position now
}

// Small "Chat" grip tab at the chat window's top-left, shown while the
// cursor is up. Grabbing it drags the engine chat control (chatDisplayHud)
// - a dedicated handle so it never steals clicks from the chat or menus.
function KronosMenu::renderChatGrip(%sw, %sh)
{
	$Panel::chatGripShown = false;
	// the custom chat overlay (KronosChat.cs) handles chat + its own drag
	if($pref::Kronos::chatEnabled)
		return;
	if(!$KM::enabled || !$KM::mouseOn)
		return;

	%cp = Control::getPosition("chatDisplayHud");
	if(%cp == "")
		return;
	%cx = getword(%cp, 0);
	%cy = getword(%cp, 1);

	%k = KronosMenu::uiScale(%sh);
	%gw = floor(%sw * 0.05 * %k);
	if(%gw < 46)
		%gw = 46;
	%gh = floor(%sh * 0.026 * %k);
	if(%gh < 14)
		%gh = 14;

	$Panel::chatGripX = %cx;
	$Panel::chatGripY = %cy;
	$Panel::chatGripW = %gw;
	$Panel::chatGripH = %gh;
	$Panel::chatGripShown = true;

	glDisable($GL_TEXTURE_2D);
	glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);
	if($Drag::active && $Drag::id == "chat")
		glColor4ub($KT::hvR, $KT::hvG, $KT::hvB, 230);
	else
		glColor4ub($KT::chR, $KT::chG, $KT::chB, 200);
	glRectangle(%cx, %cy, %gw, %gh);
	glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, 235);
	glRectangle(%cx, %cy, %gw, 2);

	glColor4ub(235, 240, 255, 240);
	glSetFont("Verdana", floor(%gh * 0.6), $GLEX_SMOOTH, 0);
	glDrawString(%cx + floor(%gh * 0.3), %cy + floor(%gh * 0.14), "Chat");
}

// Re-apply the saved chat position (engine resets it to stock on gui load /
// resolution change). Captures the stock position once for resetLayout.
function KronosMenu::applyChatPos()
{
	%ext = Control::getExtent(PlayGui);
	%sw = getword(%ext, 0);
	%sh = getword(%ext, 1);
	if(%sw < 100 || %sh < 100)
		return;

	%cur = Control::getPosition("chatDisplayHud");
	if($pref::Kronos::chatStockX == "" && %cur != "")
	{
		$pref::Kronos::chatStockX = getword(%cur, 0) / %sw;
		$pref::Kronos::chatStockY = getword(%cur, 1) / %sh;
	}

	if($pref::Kronos::chatX == "")
		return;   // never moved - leave it at stock
	Control::setPosition("chatDisplayHud", floor($pref::Kronos::chatX * %sw), floor($pref::Kronos::chatY * %sh));
}

// SCOREBOARD-REFRESH (beta report: "Scoremenu does not refresh"): the roster used
// to be fed exactly ONCE, at TAB-open (dlgPlay.cpp pushes cfgPushBaseScoreRows at
// setScoresVisible; the Kronos request fired only when the server rebuilt a menu).
// So ping/players/teams froze while the board was up, and a server change showed
// the PREVIOUS server's rows. This tick runs every frame the play screen draws:
// while the scoreboard is visible it re-requests the Kronos roster and re-runs the
// native PlayerManager feed every 3s -- fed new data, show new data.
function KronosMenu::scoreTick()
{
	if($Config::ScoresVisible != "true")
		return;
	%t = GetSimTime();
	%dt = %t - $KM::scoreTickTime;
	if($KM::scoreTickTime != "" && %dt >= 0 && %dt < 3.0)
		return;
	$KM::scoreTickTime = %t;

	// Kronos roster (vanilla-safe: base servers just ignore the remoteEval)
	remoteEval(2048, KMGetPlayers);

	// Base-server fallback: live name/team/score/ping straight from the engine's
	// PlayerManager. Only when no Kronos push has landed recently -- a Kronos
	// server owns the rows and this must not overwrite them.
	%kdt = %t - $KM::kmFeedTime;
	if($KM::kmFeedTime == "" || %kdt > 6.0 || %kdt < 0)
		cfgPushBaseScoreRows();
}

// SCOREBOARD-REFRESH: a new connection or mission means the old roster is fiction.
// Clear everything the panels render from so the first frame on the new server is
// empty rather than stale; scoreTick repopulates within 3s of opening the board.
function KronosMenu::rosterReset()
{
	// ROSTER ONLY -- never touch $KM::active/$KM::count/$KM::title here. The server
	// MENU (incl. a base server's one-shot team-select push via remoteNewMenu, the
	// ONLY setter of $KM::active) can already be up when eventMissionInfo fires;
	// clearing it killed the base-server TAB board and the team-select screen for
	// the whole session, because base servers never resend the menu (beta report
	// 2026-07-31). Kronos survived only because it rebuilds menus constantly.
	$KM::plCount = 0;
	$KM::plTotal = 0;
	$KM::selId = "";
	$KM::plDirty = true;
	$KM::plReqTime = "";
	$KM::scoreTickTime = "";
	$KM::kmFeedTime = "";
	$KM::lvPrefix = "Lv ";
	for(%i = 0; %i < 4; %i++)
	{
		$KM::colTitle[%i] = "";
	}
}

function ScriptGL::playGui::onPostDraw(%dimensions)
{
	%dim = KronosMenu::screenDim(%dimensions);

	KronosMenu::scoreTick();

	KronosMenu::render(%dim);

	// Item examine overlay (KronosHUD.cs) - drawn here so it sits at
	// the menu's info-panel spot and shows whether or not the TAB
	// menu is currently open
	if(kronos::simAge($KH::exTime) < 10.0)
		kronos::examine_render(getword(%dim, 0), getword(%dim, 1));

	KronosChat::render(getword(%dim, 0), getword(%dim, 1));
	KronosMenu::renderSlider(getword(%dim, 0), getword(%dim, 1));
	KronosMenu::renderChatGrip(getword(%dim, 0), getword(%dim, 1));
}

// ============================================
// Console helpers
// ============================================

// Run from console WHILE the TAB menu is open.
function KronosMenu::probe()
{
	echo("--- KronosMenu::probe ---");
	echo("  $KM::active = " @ $KM::active @ "  count = " @ $KM::count @ "  players = " @ $KM::plCount @ "/" @ $KM::plTotal);
	echo("  mouseOn = " @ $KM::mouseOn @ "  mouse = " @ $KM::mouseX @ "," @ $KM::mouseY);
	echo("  rowY0 = " @ $KML::rowY0 @ "  rowH = " @ $KML::rowH @ "  menu x = " @ $KML::mx @ "  players x = " @ $KML::px);
	echo("  scale basis = " @ $KM::dimSrc @ "   used dims = " @ $KM::dim @ "   ScriptGL reported = " @ $KM::dimSGL);
	echo("  UiScale = " @ $pref::Kronos::UiScale @ "  refH = " @ $pref::Kronos::UiRefH @ "  -> factor k = " @ $KML::k);
	echo("  pos menu=" @ $pref::Kronos::menuX @ "," @ $pref::Kronos::menuY @ "  players=" @ $pref::Kronos::playersX @ "," @ $pref::Kronos::playersY @ "  info=" @ $pref::Kronos::infoX @ "," @ $pref::Kronos::infoY);
	echo("  info[1] = " @ $KM::info[1]);
	echo("-------------------------");
}

// Set the GUI scale live (and persist it). 1.0 = sized for the reference
// height; lower shrinks the whole GUI's screen-share, higher grows it back
// up to the original proportional size (capped there). Affects the TAB
// menu, the shop/inventory, and the item-examine overlay together.
function KronosMenu::setScale(%s)
{
	if(%s == "" || %s <= 0)
	{
		echo("usage: KronosMenu::setScale(0.85)  - current = " @ $pref::Kronos::UiScale);
		return;
	}
	$pref::Kronos::UiScale = %s;
	echo("KronosMenu: UI scale = " @ %s @ " (1.0 = sized for " @ $pref::Kronos::UiRefH @ "p; lower = smaller)");
	KronosMenu::savePrefs();
}

// Restore the default panel positions (and the centered info box). Leaves
// the UI scale alone - use KronosMenu::setScale to reset that.
function KronosMenu::resetLayout()
{
	$pref::Kronos::menuX = 0.08;
	$pref::Kronos::menuY = 0.16;
	$pref::Kronos::playersX = 0.54;
	$pref::Kronos::playersY = 0.16;
	$pref::Kronos::infoX = "c";
	$pref::Kronos::infoY = 0.75;

	// vhud HUD panels (push the value back into vhud + bust its cache)
	$pref::Kronos::vitalsPos = "1.5 84";
	$pref::Kronos::infoHudPos = "81.5 84";
	$pref::Kronos::wbarPos = "25 96.3";
	$vhud["kh_vitals", pos] = $pref::Kronos::vitalsPos;  $vhud["kh_vitals", lastdimensions] = "";
	$vhud["kh_info", pos]   = $pref::Kronos::infoHudPos; $vhud["kh_info", lastdimensions] = "";
	$vhud["kh_wbar", pos]   = $pref::Kronos::wbarPos;    $vhud["kh_wbar", lastdimensions] = "";

	// chat window back to its captured stock position (stock chat, if used)
	$pref::Kronos::chatX = $pref::Kronos::chatStockX;
	$pref::Kronos::chatY = $pref::Kronos::chatStockY;
	KronosMenu::applyChatPos();

	// custom chat overlay back to default spot
	$pref::Kronos::chatPosX = 0.015;
	$pref::Kronos::chatPosY = 0.60;

	// UI-scale slider back to centered-top
	$pref::Kronos::sliderX = "c";
	$pref::Kronos::sliderY = 0.015;

	KronosMenu::dragEnd();
	KronosMenu::savePrefs();
	echo("KronosMenu: panel positions reset to defaults");
}

function KronosMenu::disable()
{
	$KM::enabled = false;
	$KM::active = false;
	echo("KronosMenu: panel disabled. NOTE: the stock menu is moved");
	echo("  off-screen in the score gui files, so no menu will be visible.");
	echo("  Restore the .stockbak files for the stock menu back.");
}

function KronosMenu::enable()
{
	$KM::enabled = true;
	echo("KronosMenu: enabled");
}

// ============================================
// Initialize
// ============================================

// Second player column prefix. Kronos rows are levels ("Lv 42"); a base server's rows are
// whatever its score heading publishes (score, ping), so the feed clears this.
$KM::lvPrefix = "Lv ";

$KM::active = false;
$KM::count = 0;
$KM::measureDirty = false;
$KM::menuTextW = 0;
$KM::measuredFont = "";
$KM::plDirty = false;
$KM::plNameW = 0;
$KM::plLvW = 0;
$KM::plClW = 0;
$KM::plMeasuredFont = "";
$KM::infoDirty = false;
$KM::infoTextW = 0;
$KM::infoMeasuredFont = "";
$KM::plCount = 0;
$KM::plTotal = 0;
$KM::selId = "";
$KM::mouseOn = false;
$KM::mouseX = -1;
$KM::mouseY = -1;
$KM::lmbDown = false;

// UI-scale slider state
$KSlider::min = 0.5;     // slider left end  (50%)
$KSlider::max = 1.5;     // slider right end (150%)
$KSlider2::drag = false; // damage-text size slider (row 2; range set in renderSlider)
$KM::scaleOpen = false;  // scale/theme panel starts hidden; chat "UI" button opens it
$KScale::btnW = 0;
$KSlider3::drag = false; // theme hue bar (row 3)
$KSlider::drag = false;
$KSlider::trackX = 0;
$KSlider::trackW = 0;
$KSlider::hitX0 = 0;
$KSlider::hitX1 = 0;
$KSlider::hitY0 = 0;
$KSlider::hitY1 = 0;

// panel-drag state
$Drag::active = false;
$Drag::id = "";
$Panel::menuW = 0;
$Panel::plW = 0;
$Panel::infoShown = false;
$Panel::chatGripShown = false;
$Panel::kchatShown = false;
$Panel::uisShown = false;

// draggable vhud HUD panels (KronosHUD.cs) - hit-tested via their vhud
// render rects; positions persist in their own $pref vars
$Drag::hudN = 3;
$Drag::hudName[0] = "kh_vitals";
$Drag::hudName[1] = "kh_info";
$Drag::hudName[2] = "kh_wbar";

// if exec'd mid-game, PlayGui is already up - apply the saved chat position
// now (and capture the stock position for resetLayout)
if($Mode::PlayMode)
	KronosMenu::applyChatPos();

echo("KronosMenu: modern TAB menu loaded (Hudbot mouse input)");

// SCOREBOARD-REFRESH: purge the roster whenever the connection or mission changes.
Event::Attach(eventConnectionAccepted, "KronosMenu::rosterReset();", attachKMRosterReset);
Event::Attach(eventMissionInfo, "KronosMenu::rosterReset();", attachKMRosterReset2);
