//==============================================
// KronosHUD.cs - ScriptGL RPG HUD for Kingdom of Kronos
//==============================================
// Modern RPG HUD using Hudbot ScriptGL. Renders HP/Mana bars,
// XP strip, gold/level/class readout, and a combat target frame.
// Uses the vhud resolution-independent layout framework.
//
//----------------------------------------------
// HOOKS / INTEGRATIONS:
//----------------------------------------------
// autoexec.cs         - exec("Presto\\KronosHUD.cs"); loads this script
// scriptgl2.cs        - vhud framework calls our onrender callbacks via
//                       ScriptGL::playGui::onPreDraw -> vhud::render
// KronosHUD_Server.cs - Server pushes stats via remoteEval("KronosHUD",...)
//==============================================

// Network rates: the 1998 defaults ($pref::PacketRate=10, PacketSize=200)
// pace the reliable stream for dial-up, which makes every server menu
// round-trip (skill points, shop pushes, roster) feel sluggish on a LAN /
// broadband. 30 pkt/s + 450B is the stock protocol maximum - both ends
// clamp, so this is safe against any server.
$pref::PacketRate = 30;
$pref::PacketSize = 450;

// Fullscreen must present through OpenGL. The stock 1.3 default fullscreen
// driver is "Software" (windowed is OpenGL), so toggling fullscreen swapped
// the renderer out from under ScriptGL - every HUD gl* call then ran with no
// GL context and CRASHED the client. Forcing the fullscreen driver to OpenGL
// (the renderer the game is already presenting with) fixes it; the pref is
// saved back to ClientPrefs.cs so it sticks.
$pref::VideoFullScreenDriver = "OpenGL";

// The HUD draws through OpenGL, so the WINDOWED driver must be OpenGL too
// (stock default is Software - the HUD would simply be invisible).
if($pref::VideoWindowedDriver == "" || $pref::VideoWindowedDriver == "Software")
	$pref::VideoWindowedDriver = "OpenGL";

// Boot-renderer fix: stock 1.3 applies the video prefs (OptionsVideo::apply)
// immediately after creating the window - too early for the OpenGL device to
// take - so the game can boot into the SOFTWARE renderer even though
// ClientPrefs saves $pref::VideoWindowedDriver = "OpenGL". A GL-drawn HUD is
// invisible in software mode. Hook OptionsVideo::validate (called right
// before that startup apply, when the scheduler exists) and arm a one-shot
// delayed re-apply. Body mirrors the stock Options.cs validate exactly.
function OptionsVideo::validate()
{
	if($pref::VideoFullScreen == "") $pref::VideoFullScreen = "FALSE";
	if($pref::VideoFullScreenDriver == "")
	{
		if(isGfxDriver(MainWindow, "Glide")) $pref::VideoFullScreenDriver = "Glide";
		else $pref::VideoFullScreenDriver = "Software";
	}
	if($pref::VideoWindowedDriver == "") $pref::VideoWindowedDriver = "Software";
	if($pref::VideoFullScreenRes == "") $pref::VideoFullScreenRes = "640x480";
	if($pref::VideoGamma == "") $pref::VideoGamma = "0.5";

	if(!$KV::reapplyArmed)
	{
		$KV::reapplyArmed = true;
		schedule("KronosVideo::reapply();", 3);
	}
}

function KronosVideo::reapply()
{
	// only re-assert in windowed mode, and never downgrade to Software
	if(String::ICompare($pref::VideoFullScreen, "TRUE") == 0)
		return;
	if($pref::VideoWindowedDriver == "" || $pref::VideoWindowedDriver == "Software")
		return;
	echo("KronosVideo: re-applying windowed driver '" @ $pref::VideoWindowedDriver @ "'");
	setWindowedDevice(MainWindow, $pref::VideoWindowedDriver);
}

// ============================================
// HUD color theme
// ============================================
// Every Kronos panel draws its chrome (borders, hovers, chips, tinted text,
// dark backdrops) from the $KT::* role variables set here, so one theme
// switch recolors the whole suite. Functional colors (HP green/red, mana
// blue, XP gold, enemy red, damage floats) are deliberately NOT themed.
//   Presets:  KTheme::set(blue);  KTheme::set(rpg);  KTheme::set(green);
//   Custom :  KTheme::hue(215);   (any hue 0-360 - the widget's rainbow bar)
// Persisted in $pref::Kronos::theme. Picker UI lives in the scale widget.

// h 0-360, s/v 0-1 -> "r g b" bytes
function KTheme::hsv(%h, %s, %v)
{
	%h = %h - (floor(%h / 360) * 360);
	if(%h < 0)
		%h = %h + 360;
	%c = %v * %s;
	%hp = %h / 60;
	%m2 = %hp - (floor(%hp / 2) * 2);
	%d = %m2 - 1;
	if(%d < 0)
		%d = -%d;
	%x = %c * (1 - %d);
	%sec = floor(%hp);
	if(%sec == 0)      { %r = %c; %g = %x; %b = 0;  }
	else if(%sec == 1) { %r = %x; %g = %c; %b = 0;  }
	else if(%sec == 2) { %r = 0;  %g = %c; %b = %x; }
	else if(%sec == 3) { %r = 0;  %g = %x; %b = %c; }
	else if(%sec == 4) { %r = %x; %g = 0;  %b = %c; }
	else               { %r = %c; %g = 0;  %b = %x; }
	%m = %v - %c;
	return floor((%r + %m) * 255) @ " " @ floor((%g + %m) * 255) @ " " @ floor((%b + %m) * 255);
}

function KTheme::roles(%ac, %dm, %hv, %hb, %ch, %tx, %bg, %b2)
{
	$KT::acR = getWord(%ac, 0);  $KT::acG = getWord(%ac, 1);  $KT::acB = getWord(%ac, 2);
	$KT::dmR = getWord(%dm, 0);  $KT::dmG = getWord(%dm, 1);  $KT::dmB = getWord(%dm, 2);
	$KT::hvR = getWord(%hv, 0);  $KT::hvG = getWord(%hv, 1);  $KT::hvB = getWord(%hv, 2);
	$KT::hbR = getWord(%hb, 0);  $KT::hbG = getWord(%hb, 1);  $KT::hbB = getWord(%hb, 2);
	$KT::chR = getWord(%ch, 0);  $KT::chG = getWord(%ch, 1);  $KT::chB = getWord(%ch, 2);
	$KT::txR = getWord(%tx, 0);  $KT::txG = getWord(%tx, 1);  $KT::txB = getWord(%tx, 2);
	$KT::bgR = getWord(%bg, 0);  $KT::bgG = getWord(%bg, 1);  $KT::bgB = getWord(%bg, 2);
	$KT::b2R = getWord(%b2, 0);  $KT::b2G = getWord(%b2, 1);  $KT::b2B = getWord(%b2, 2);
}

function KTheme::apply()
{
	%t = $pref::Kronos::theme;
	if(%t == "")
		%t = "blue";
	if(%t == "rpg")
		KTheme::roles("200 165 110", "165 130 85", "215 180 120", "235 205 150",
			"140 110 70", "225 205 170", "22 17 11", "36 28 18");
	else if(%t == "green")
		KTheme::roles("110 205 130", "85 165 105", "125 215 145", "155 240 175",
			"70 150 90", "175 225 185", "10 18 12", "18 32 22");
	else if(String::findSubStr(%t, "hue:") == 0)
	{
		%h = String::getSubStr(%t, 4, 10);
		KTheme::roles(KTheme::hsv(%h, 0.53, 0.92), KTheme::hsv(%h, 0.57, 0.82),
			KTheme::hsv(%h, 0.49, 0.94), KTheme::hsv(%h, 0.38, 0.98),
			KTheme::hsv(%h, 0.61, 0.71), KTheme::hsv(%h, 0.28, 0.94),
			KTheme::hsv(%h, 0.45, 0.09), KTheme::hsv(%h, 0.50, 0.16));
	}
	else // "blue" - the original palette, byte-for-byte
		KTheme::roles("110 165 235", "85 140 210", "120 170 235", "150 195 245",
			"70 115 180", "170 200 240", "10 12 20", "20 28 45");
}

function KTheme::set(%name)
{
	$pref::Kronos::theme = %name;
	KTheme::apply();
	echo("KTheme: theme = " @ %name);
}

function KTheme::hue(%h)
{
	$pref::Kronos::theme = "hue:" @ floor(%h);
	KTheme::apply();
}

KTheme::apply();   // colors must exist before any panel renders

// ============================================
// Sim-time age helper
// ============================================
// GetSimTime() RESETS on join / mission change, so a stamp taken before the
// reset makes (now - stamp) NEGATIVE - and every "show while age < N" overlay
// check passes forever (stuck examine text, immortal target frames...).
// Treat empty stamps and negative deltas as VERY OLD.
function kronos::simAge(%t)
{
	if(%t == "" || %t == 0)
		return 99999;
	%d = GetSimTime() - %t;
	if(%d < 0)
		return 99999;
	return %d;
}

// ============================================
// Remote handlers - receive data from server
// ============================================

// Vitals push (all numeric values - safe for remoteEval)
function remoteKronosHUD(%server, %hp, %maxHp, %mana, %maxMana, %exp, %xpCur, %xpNext, %gold, %lvl, %remort)
{
	if(%server != 2048) return;

	$KH::hp = %hp;
	$KH::maxHp = %maxHp;
	$KH::mana = %mana;
	$KH::maxMana = %maxMana;
	$KH::exp = %exp;
	$KH::xpCur = %xpCur;
	$KH::xpNext = %xpNext;
	$KH::gold = %gold;
	$KH::lvl = %lvl;
	$KH::remort = %remort;
	$KH::hasData = true;

	// Handshake: tell the server this client runs KronosHUD, so it can
	// send HUD popups instead of stock bottomprints. Re-affirmed every
	// 30s to survive server restarts / reconnects within one session.
	// GetSimTime() RESETS on reconnect / mission reload, so the clock
	// can run backwards relative to lastHandshake - treat that as a
	// reconnect and re-handshake immediately, otherwise the throttle
	// would never fire again (negative elapsed) and the inventory /
	// shop HUD would stay broken until a full client restart.
	%now = GetSimTime();
	if($KH::lastHandshake == "" || %now < $KH::lastHandshake || %now - $KH::lastHandshake > 30)
	{
		$KH::lastHandshake = %now;
		remoteEval(2048, KHudOn);
	}
}

// Metadata push (class/zone - may contain spaces, sent separately)
function remoteKronosHUD2(%server, %class, %zone)
{
	if(%server != 2048) return;

	$KH::class = %class;
	$KH::zone = %zone;
}

// Target frame push (LOS scan and damage hits)
// %dmg is empty for LOS-scan pushes; a number (or "LCK") for hits
function remoteKronosTarget(%server, %targetName, %targetHpPct, %dmg)
{
	if(%server != 2048) return;

	// New target - reset the combat readout slots
	if(%targetName != $KH::targetName)
	{
		$KH::tMissTime = 0;
		$KH::tLckN = 0;   $KH::tLckTime = 0;
		$KH::tDmgSum = 0; $KH::tDmgTime = 0;
	}

	$KH::targetName = %targetName;
	$KH::targetHp = %targetHpPct;
	$KH::targetTime = GetSimTime();

	// Damage hit: LCK and numeric damage feed SEPARATE plate slots
	// (MISS comes via the nameplate ATKText path); rapid hits accumulate
	if(%dmg != "" && %dmg != -1)
	{
		%now = GetSimTime();
		if(%dmg == "LCK")
		{
			if(kronos::simAge($KH::tLckTime) >= 1.5)
				$KH::tLckN = 1;
			else
				$KH::tLckN++;
			$KH::tLckTime = %now;
		}
		else
		{
			if(kronos::simAge($KH::tDmgTime) >= 1.5)
				$KH::tDmgSum = %dmg;
			else
				$KH::tDmgSum = $KH::tDmgSum + %dmg;
			$KH::tDmgTime = %now;
		}
	}
}

// Weapon info popup push - replaces the stock weapon-switch
// bottomprint for HUD clients (server: KronosWeaponInfo). %text is
// the original bottomprint string: rows separated by runs of 4+
// spaces, colored with <f0>/<f1>/<f2>/<f3> tags.
function remoteKronosWeapon(%server, %text)
{
	if(%server != 2048) return;

	%rows = 0;
	%rest = %text;
	while(%rest != "" && %rows < 8)
	{
		%idx = String::findSubStr(%rest, "    ");
		if(%idx == -1)
		{
			%chunk = %rest;
			%rest = "";
		}
		else
		{
			%chunk = String::getSubStr(%rest, 0, %idx);
			%rest = String::getSubStr(%rest, %idx + 4, 9999);
			while(String::getSubStr(%rest, 0, 1) == " ")
				%rest = String::getSubStr(%rest, 1, 9999);
		}
		if(%chunk != "")
		{
			$KH::wpnRowRaw[%rows] = %chunk;
			%rows++;
		}
	}
	$KH::wpnRows = %rows;
	$KH::wpnCachedA = -1; // force re-colorize on next frame
	$KH::wpnTime = GetSimTime();

	// Equipped-weapon bar: the weapon name is row 0 between the
	// leading <f1> tag and the first colon
	if(%rows > 0)
	{
		%t = $KH::wpnRowRaw[0];
		if(String::findSubStr(%t, "<f1>") == 0)
			%t = String::getSubStr(%t, 4, 9999);
		%ci = String::findSubStr(%t, ":");
		if(%ci != -1)
			%t = String::getSubStr(%t, 0, %ci);
		if(%t != "")
			$KH::curWeapon = %t;
	}
}

// 0-255 -> two lowercase hex digits (for inline glDrawString colors)
function KronosHUD_Hex2(%n)
{
	%digits = "0123456789abcdef";
	if(%n < 0) %n = 0;
	if(%n > 255) %n = 255;
	%hi = floor(%n / 16);
	return String::getSubStr(%digits, %hi, 1) @ String::getSubStr(%digits, %n - (%hi * 16), 1);
}

// Convert the server's <fN> tags to inline <RRGGBBAA> color codes at
// the given alpha. Cached per alpha value - only re-runs while fading.
function KronosHUD_ColorizeWeaponRows(%alpha)
{
	%aa = KronosHUD_Hex2(%alpha);
	%cLabel = "<aab4c8" @ %aa @ ">";   // f0 - labels
	%cTitle = "<ffffff" @ %aa @ ">";   // f1 - weapon name
	%cValue = "<ffd278" @ %aa @ ">";   // f2 - values
	%cSpec  = "<ff9682" @ %aa @ ">";   // f3 - SPECIAL effects

	for(%i = 0; %i < $KH::wpnRows; %i++)
	{
		%rest = $KH::wpnRowRaw[%i];
		%out = %cLabel;
		%idx = String::findSubStr(%rest, "<f");
		while(%idx != -1)
		{
			%out = %out @ String::getSubStr(%rest, 0, %idx);
			%d = String::getSubStr(%rest, %idx + 2, 1);
			if(%d == 1)
				%out = %out @ %cTitle;
			else if(%d == 2)
				%out = %out @ %cValue;
			else if(%d == 3)
				%out = %out @ %cSpec;
			else
				%out = %out @ %cLabel;
			%rest = String::getSubStr(%rest, %idx + 4, 9999);
			%idx = String::findSubStr(%rest, "<f");
		}
		$KH::wpnRowTxt[%i] = %out @ %rest;
	}
	$KH::wpnCachedA = %alpha;
}

// Item examine push - replaces the stock WhatIs bottomprint for HUD
// clients (server: KronosExamineInfo). %text is the original multi-
// line bottomprint string (\n-separated, <jc>/<f0>/<f1> tags). Shown
// as an overlay on the bottom-right info panel for ~10s.
function remoteKronosExamine(%server, %text)
{
	if(%server != 2048) return;

	// Split into rows on newlines, rendered line-by-line (the engine's
	// newline handling in glGetStringDimensions is unreliable, so we
	// never measure or draw the block as one multiline string).
	%nl = "\n";
	%nlLen = String::len(%nl);

	%rows = 0;
	%more = true;
	%rest = %text;
	while(%more && %rows < 16)
	{
		%idx = String::findSubStr(%rest, %nl);
		if(%idx == -1)
		{
			%chunk = %rest;
			%more = false;
		}
		else
		{
			%chunk = String::getSubStr(%rest, 0, %idx);
			%rest = String::getSubStr(%rest, %idx + %nlLen, 99999);
		}
		$KH::exRowRaw[%rows] = %chunk;
		%rows++;
	}
	// drop trailing blank rows
	while(%rows > 0 && $KH::exRowRaw[%rows - 1] == "")
		%rows--;

	$KH::exRows = %rows;
	$KH::exMeasured = false;
	$KH::exDispA = -1;
	$KH::exTime = GetSimTime();
}

// Colorize examine rows at the given alpha. Bottomprint color tags
// persist across newlines, so the last color of each row carries
// forward as the next row's starting color.
function KronosHUD_ColorizeExamineRows(%alpha)
{
	%aa = KronosHUD_Hex2(%alpha);
	%cur = "e6e6f0"; // default body color

	for(%i = 0; %i < $KH::exRows; %i++)
	{
		%raw = String::replace($KH::exRowRaw[%i], "<jc>", "");

		// find the LAST color tag in the row - it becomes the carry
		%next = %cur;
		%best = -1;
		%p = String::findSubStr(%raw, "<f0>");
		if(%p > %best) { %best = %p; %next = "aab4c8"; }
		%p = String::findSubStr(%raw, "<f1>");
		if(%p > %best) { %best = %p; %next = "ffffff"; }
		%p = String::findSubStr(%raw, "<f2>");
		if(%p > %best) { %best = %p; %next = "ffd278"; }
		%p = String::findSubStr(%raw, "<f3>");
		if(%p > %best) { %best = %p; %next = "ff9682"; }

		%d = String::replace(%raw, "<f0>", "<aab4c8" @ %aa @ ">");
		%d = String::replace(%d, "<f1>", "<ffffff" @ %aa @ ">");
		%d = String::replace(%d, "<f2>", "<ffd278" @ %aa @ ">");
		%d = String::replace(%d, "<f3>", "<ff9682" @ %aa @ ">");

		$KH::exRowTxt[%i] = "<" @ %cur @ %aa @ ">" @ %d;
		%cur = %next;
	}
	$KH::exDispA = %alpha;
}

// Cast bar push - a spell cast just started
// %fireDelay = seconds until the spell fires, %recovTime = seconds
// until "ready to cast" (full bar duration). Countdown runs locally.
function remoteKronosCast(%server, %spellName, %fireDelay, %recovTime)
{
	if(%server != 2048) return;

	%now = GetSimTime();
	$KH::castName = %spellName;
	$KH::castStart = %now;
	$KH::castFire = %now + %fireDelay;
	$KH::castEnd = %now + %recovTime;
	$KH::castInterrupted = 0;
}

// Cast interrupted - kill the bar and flash a notice
function remoteKronosCastStop(%server)
{
	if(%server != 2048) return;
	if($KH::castName == "") return;

	$KH::castEnd = 0;
	$KH::castInterrupted = GetSimTime();
}

// ============================================
// HUD Layout Definition (all percentage-based)
// ============================================

function kronos::create()
{
	// Movable HUD panel positions (persist - "x y" percentages). Drag them
	// while the cursor is up (TAB); KronosMenu's drag system rewrites these
	// and KronosMenu::resetLayout() restores the defaults below.
	if($pref::Kronos::vitalsPos == "")  $pref::Kronos::vitalsPos  = "1.5 84";
	if($pref::Kronos::infoHudPos == "") $pref::Kronos::infoHudPos = "81.5 84";
	if($pref::Kronos::wbarPos == "")    $pref::Kronos::wbarPos    = "25 96.3";

	// -------------------------------------------
	// Left panel: Vitals (HP, Mana, XP)
	// 18% wide x 14% tall, anchored bottom-left
	// -------------------------------------------
	vhud::create("kh_vitals", "22% 14%", $pref::Kronos::vitalsPos, "kronos::vitals_onrender");

	// NOTE: vhud::scale adds the parent panel's screen position to every
	// two-word item, so sizes can't be stored directly - bars are stored
	// as top-left + bottom-right corner positions and width/height are
	// computed by subtraction (same idiom as vhud.halflife2.cs).

	// HP bar
	vhud::add_item("hp_bar_pos", "5% 6%");
	vhud::add_item("hp_bar_br", "95% 30%");
	vhud::add_item("hp_text_pos", "8% 8%");
	vhud::add_item("hp_font_size", "17%");

	// Mana bar
	vhud::add_item("mana_bar_pos", "5% 37%");
	vhud::add_item("mana_bar_br", "95% 61%");
	vhud::add_item("mana_text_pos", "8% 39%");
	vhud::add_item("mana_font_size", "17%");

	// XP bar (slightly thinner than HP/MP)
	vhud::add_item("xp_bar_pos", "5% 68%");
	vhud::add_item("xp_bar_br", "95% 88%");
	vhud::add_item("xp_text_pos", "8% 69%");
	vhud::add_item("xp_font_size", "13%");

	// Shared label size
	vhud::add_item("label_font_size", "10%");

	// -------------------------------------------
	// Right panel: Info (Level, Class, Gold, Zone)
	// 17% wide x 14% tall, anchored bottom-right
	// -------------------------------------------
	vhud::create("kh_info", "17% 14%", $pref::Kronos::infoHudPos, "kronos::info_onrender");
	vhud::add_item("line1_pos", "8% 10%");
	vhud::add_item("line2_pos", "8% 33%");
	vhud::add_item("line3_pos", "8% 56%");
	vhud::add_item("line4_pos", "8% 78%");
	vhud::add_item("info_font_size", "17%");
	vhud::add_item("info_label_size", "14%");

	// -------------------------------------------
	// Target frame: above crosshair area
	// 20% wide x 7% tall, centered horizontally
	// -------------------------------------------
	// -------------------------------------------
	// Cast bar: centered, below the crosshair
	// 24% wide x 4% tall
	// -------------------------------------------
	vhud::create("kh_cast", "24% 4%", "38% 64%", "kronos::cast_onrender");
	vhud::add_item("cast_name_pos", "4% 6%");
	vhud::add_item("cast_time_pos", "82% 6%");
	vhud::add_item("cast_bar_pos", "4% 54%");
	vhud::add_item("cast_bar_br", "96% 86%");
	vhud::add_item("cast_font", "30%");

	// -------------------------------------------
	// Weapon info popup: bottom-center, shows ~3s on weapon switch
	// 38% wide x 13% tall (height shrinks to fit the row count)
	// -------------------------------------------
	vhud::create("kh_weapon", "38% 13%", "31% 70%", "kronos::weapon_onrender");

	// -------------------------------------------
	// Equipped-weapon bar: long thin strip between the vitals
	// panel (ends 23.5%) and the info panel (starts 81.5%),
	// vertically aligned with the XP strip (~93.5% screen height)
	// -------------------------------------------
	vhud::create("kh_wbar", "55% 2.9%", $pref::Kronos::wbarPos, "kronos::wbar_onrender");

	vhud::create("kh_target", "20% 7%", "40% 34%", "kronos::target_onrender");
	vhud::add_item("tgt_name_pos", "5% 8%");
	vhud::add_item("tgt_dmg_pos", "58% 8%");
	vhud::add_item("tgt_bar_pos", "5% 52%");
	vhud::add_item("tgt_bar_br", "77% 88%");
	vhud::add_item("tgt_hp_pos", "80% 52%");
	vhud::add_item("tgt_name_font", "32%");
	vhud::add_item("tgt_hp_font", "28%");
}

// ============================================
// Shared rendering helpers
// ============================================

// Semi-transparent dark panel background with subtle border
function kronos::backdrop()
{
	glDisable($GL_TEXTURE_2D);
	glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);

	// Dark fill
	glColor4ub($KT::bgR, $KT::bgG, $KT::bgB, 155);
	vhud::render_box("pos", "size");

	// Subtle border (1px lines)
	%pos = vhud::render_value("pos");
	%size = vhud::render_value("size");
	%x = getword(%pos, 0);
	%y = getword(%pos, 1);
	%w = getword(%size, 0);
	%h = getword(%size, 1);

	glColor4ub($KT::chR, $KT::chG, $KT::chB, 70);
	glRectangle(%x, %y, %w, 1);
	glRectangle(%x, %y + %h - 1, %w, 1);
	glRectangle(%x, %y, 1, %h);
	glRectangle(%x + %w - 1, %y, 1, %h);
}

// Draw a filled bar with background, highlight, and empty-portion tint.
// %pos_label = top-left corner item, %br_label = bottom-right corner item
// (both are positions; width/height computed by subtraction)
// %pct = 0.0 - 1.0  |  %r,%g,%b = fill color (0-255)
function kronos::draw_bar(%pos_label, %br_label, %pct, %r, %g, %b)
{
	%pos = vhud::render_value(%pos_label);
	%br = vhud::render_value(%br_label);

	%x = getword(%pos, 0);
	%y = getword(%pos, 1);
	%w = getword(%br, 0) - %x;
	%h = getword(%br, 1) - %y;

	// Clamp percentage
	if(%pct > 1)
		%pct = 1;
	if(%pct < 0)
		%pct = 0;

	%fillW = floor(%w * %pct);

	glDisable($GL_TEXTURE_2D);
	glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);

	// Bar track (dark background)
	glColor4ub(0, 0, 0, 120);
	glRectangle(%x, %y, %w, %h);

	// Filled portion
	if(%fillW > 0)
	{
		glColor4ub(%r, %g, %b, 195);
		glRectangle(%x, %y, %fillW, %h);

		// 1px top highlight for depth
		glColor4ub(255, 255, 255, 40);
		glRectangle(%x, %y, %fillW, 1);
	}

	// Empty portion (subtle tint)
	if(%fillW < %w)
	{
		%emptyX = %x + %fillW;
		glColor4ub(50, 50, 65, 80);
		glRectangle(%emptyX, %y, %w - %fillW, %h);
	}
}

// ============================================
// Vitals panel render (HP, Mana, XP)
// ============================================

// Draw text fitted inside a bar's rect: vertically centered, and the font
// shrunk if the string would overflow the bar width (so big RPG numbers
// like "598985 / 598985" stay inside the bar). %posLabel/%brLabel are the
// bar's top-left / bottom-right vhud corner items.
function kronos::drawBarText(%posLabel, %brLabel, %text, %r, %g, %b, %a)
{
	%pos = vhud::render_value(%posLabel);
	%br  = vhud::render_value(%brLabel);
	%bx = getword(%pos, 0);
	%by = getword(%pos, 1);
	%bw = getword(%br, 0) - %bx;
	%bh = getword(%br, 1) - %by;

	%margin = floor(%bh * 0.22);
	if(%margin < 2)
		%margin = 2;
	%maxW = %bw - (%margin * 2);
	if(%maxW < 8)
		%maxW = 8;

	%font = floor(%bh * 0.72);
	if(%font < 7)
		%font = 7;
	glSetFont("Verdana", %font, $GLEX_SMOOTH, 9);
	%tw = getword(glGetStringDimensions(%text), 0);
	if(%tw > %maxW && %tw > 0)
	{
		%font = floor(%font * %maxW / %tw);
		if(%font < 6)
			%font = 6;
		glSetFont("Verdana", %font, $GLEX_SMOOTH, 9);
	}

	%ty = %by + floor((%bh - %font) / 2) - 1;
	glColor4ub(%r, %g, %b, %a);
	glDrawString(%bx + %margin, %ty, %text);
}

function kronos::vitals_onrender()
{
	// no panel backdrop - each bar draws its own dark track for contrast

	// --- HP Bar ---
	// $Health is 0-100, set by the engine every frame (free, no server push)
	%hpPct = $Health / 100;
	if(%hpPct < 0)
		%hpPct = 0;

	// HP color: green -> yellow -> red
	if(%hpPct > 0.6)
	{
		%hr = 45;
		%hg = 200;
		%hb = 95;
	}
	else if(%hpPct > 0.3)
	{
		%hr = 225;
		%hg = 195;
		%hb = 45;
	}
	else
	{
		%hr = 215;
		%hg = 50;
		%hb = 50;
	}

	kronos::draw_bar("hp_bar_pos", "hp_bar_br", %hpPct, %hr, %hg, %hb);

	// HP text overlay (fit + centered inside the bar)
	if($KH::hasData)
		%hpText = "HP  " @ $KH::hp @ " / " @ $KH::maxHp;
	else
		%hpText = "HP  " @ floor($Health) @ "%";
	kronos::drawBarText("hp_bar_pos", "hp_bar_br", %hpText, 255, 255, 255, 235);

	// --- Mana Bar ---
	// $Energy is 0-100, set by the engine (free)
	%manaPct = $Energy / 100;
	if(%manaPct < 0)
		%manaPct = 0;

	kronos::draw_bar("mana_bar_pos", "mana_bar_br", %manaPct, 55, 130, 215);

	// Mana text (fit + centered inside the bar)
	if($KH::hasData)
		%manaText = "MP  " @ $KH::mana @ " / " @ $KH::maxMana;
	else
		%manaText = "MP  " @ floor($Energy) @ "%";
	kronos::drawBarText("mana_bar_pos", "mana_bar_br", %manaText, 235, 245, 255, 235);

	// --- XP Bar ---
	%xpPct = 0;
	if($KH::hasData)
	{
		%xpRange = $KH::xpNext - $KH::xpCur;
		if(%xpRange > 0)
			%xpPct = ($KH::exp - $KH::xpCur) / %xpRange;
	}
	if(%xpPct < 0)
		%xpPct = 0;
	if(%xpPct > 1)
		%xpPct = 1;

	kronos::draw_bar("xp_bar_pos", "xp_bar_br", %xpPct, 195, 165, 45);

	// XP text - near-black so it reads against the gold bar
	if($KH::hasData)
		%xpText = "XP  " @ floor(%xpPct * 100) @ "%";
	else
		%xpText = "XP";
	kronos::drawBarText("xp_bar_pos", "xp_bar_br", %xpText, 20, 18, 8, 245);

	// --- OWN combat readout (nameplate mode) ---
	// Incoming MISS(dodged) | LCK | -DAMAGE in the same slot row as the
	// target plate, drawn just above our own "plate" (the HP bar)
	%oPos = vhud::render_value("hp_bar_pos");
	%oBr = vhud::render_value("hp_bar_br");
	// hp_bar_br is the bar's bottom-right CORNER, not a size - the bar
	// height is (brY - posY). Using brY directly made MISS half the screen.
	%oFont = floor((getword(%oBr, 1) - getword(%oPos, 1)) * 0.85);
	if(%oFont < 9)
		%oFont = 9;
	KronosHUD::renderPlateRow(getword(%oPos, 0), getword(%oPos, 1) - %oFont - 4, %oFont,
		$KH::oMissTime, $KH::oLckN, $KH::oLckTime, $KH::oDmgSum, $KH::oDmgTime,
		255, 90, 80);
}

// Own plate feed (nameplate mode): parse an incoming defender message into
// the MISS / LCK / DMG slots. Unknown formats fall back to a sinking float.
function KronosHUD::ownHit(%plain)
{
	%now = GetSimTime();
	if(String::findSubStr(%plain, "Dodged") != -1 || String::findSubStr(%plain, "Miss") != -1)
	{
		$KH::oMissTime = %now;
		return;
	}
	if(String::findSubStr(%plain, "LCK") != -1)
	{
		if(kronos::simAge($KH::oLckTime) >= 1.5)
			$KH::oLckN = 1;
		else
			$KH::oLckN++;
		$KH::oLckTime = %now;
		return;
	}
	%dash = String::findSubStr(%plain, "-");
	if(%dash != -1)
	{
		%n = String::getSubStr(%plain, %dash + 1, 99999) + 0;
		if(%n > 0)
		{
			if(kronos::simAge($KH::oDmgTime) >= 1.5)
				$KH::oDmgSum = %n;
			else
				$KH::oDmgSum = $KH::oDmgSum + %n;
			$KH::oDmgTime = %now;
			return;
		}
	}
	KronosHUD::addFloat(%plain, "defender", -1);
}

// ============================================
// Info panel render (Level, Class, Gold, Zone)
// ============================================

function kronos::info_onrender()
{
	// Don't render until server has sent data
	if(!$KH::hasData)
		return;

	kronos::backdrop();

	// Line 1: Level + Remort
	glColor4ub(255, 255, 255, 230);
	%lvlText = "Lv " @ $KH::lvl;
	if($KH::remort > 0)
		%lvlText = %lvlText @ "  RL" @ $KH::remort;
	vhud::render_text("line1_pos", "Verdana", "info_font_size", $GLEX_SMOOTH, %lvlText);

	// Line 2: Class
	glColor4ub(175, 195, 255, 210);
	if($KH::class != "" && $KH::class != -1)
		vhud::render_text("line2_pos", "Verdana", "info_label_size", $GLEX_SMOOTH, $KH::class);

	// Line 3: Gold
	glColor4ub(255, 210, 75, 225);
	vhud::render_text("line3_pos", "Verdana", "info_label_size", $GLEX_SMOOTH, "Gold " @ KronosShop::commafy($KH::gold));

	// Line 4: Zone
	glColor4ub(145, 175, 145, 180);
	if($KH::zone != "" && $KH::zone != -1)
		vhud::render_text("line4_pos", "Verdana", "info_label_size", $GLEX_SMOOTH, $KH::zone);
}

// ============================================
// Item examine overlay render (~10s) - replaces the TAB menu's
// bottom-center character-info panel. Auto-sizes to the text:
// centered horizontally, top anchored at the info panel's y
// (75% of screen height). Called from KronosMenu's onPostDraw so
// it layers with (and substitutes for) the menu's info panel.
// Whole block drawn with one glDrawString (handles \n + colors).
// ============================================

function kronos::examine_render(%sw, %sh)
{
	if($KH::exRows < 1)
		return;

	// Hold solid for 9s, fade out over the last 1s
	%elapsed = kronos::simAge($KH::exTime);
	%alpha = 230;
	if(%elapsed > 9.0)
	{
		%fade = (%elapsed - 9.0) / 1.0;
		%alpha = floor(230 - (230 * %fade));
		if(%alpha < 0)
			%alpha = 0;
	}

	// Mirror the TAB menu character-info panel exactly: same width, same
	// position (which the player can drag), same top y. SIZES scale toward
	// the reference; POSITION follows the persisted info-panel prefs (x is
	// centered until dragged, same as KronosMenu::render).
	%k = KronosMenu::uiScale(%sh);
	%pad = floor(%sw * %k * 0.012);
	%w = floor(%sw * %k * 0.38);
	if($pref::Kronos::infoX == "c" || $pref::Kronos::infoX == "")
		%x0 = floor((%sw - %w) / 2);
	else
		%x0 = floor($pref::Kronos::infoX * %sw);
	%y0 = floor($pref::Kronos::infoY * %sh);

	%lineH = floor(%sh * %k * 0.026);
	%maxH = floor(%sh * 0.97) - %y0 - (%pad * 2);
	if(%lineH * $KH::exRows > %maxH && $KH::exRows > 0)
		%lineH = floor(%maxH / $KH::exRows);
	%font = floor(%lineH * 0.78);
	if(%font < 9)
		%font = 9;

	// Measure the widest row once (re-measured if the font basis
	// changes, e.g. resolution change)
	if(!$KH::exMeasured || $KH::exBaseFont != %font)
	{
		glSetFont("Verdana", %font, $GLEX_SMOOTH, 0);
		%mw = 0;
		for(%i = 0; %i < $KH::exRows; %i++)
		{
			%p = String::replace($KH::exRowRaw[%i], "<jc>", "");
			%p = String::replace(%p, "<f0>", "");
			%p = String::replace(%p, "<f1>", "");
			%p = String::replace(%p, "<f2>", "");
			%p = String::replace(%p, "<f3>", "");
			%t = getword(glGetStringDimensions(%p), 0);
			if(%t > %mw)
				%mw = %t;
		}
		$KH::exTextW = %mw;
		$KH::exBaseFont = %font;
		$KH::exMeasured = true;
	}

	// Panel width is FIXED - shrink the font if the widest row
	// wouldn't fit
	%maxTW = %w - (%pad * 2);
	%fontUse = %font;
	if($KH::exTextW > %maxTW && $KH::exTextW > 0)
	{
		%fontUse = floor(%font * %maxTW / $KH::exTextW);
		if(%fontUse < 9)
			%fontUse = 9;
	}

	// Re-colorize when the alpha changes (only during the fade)
	if($KH::exDispA != %alpha)
		KronosHUD_ColorizeExamineRows(%alpha);

	%boxH = ($KH::exRows * %lineH) + (%pad * 2);

	// backdrop
	glDisable($GL_TEXTURE_2D);
	glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);

	glColor4ub($KT::bgR, $KT::bgG, $KT::bgB, floor(%alpha * 0.8));
	glRectangle(%x0, %y0, %w, %boxH);

	glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, floor(%alpha * 0.85));
	glRectangle(%x0, %y0, %w, 2);
	glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, floor(%alpha * 0.35));
	glRectangle(%x0, %y0 + %boxH - 1, %w, 1);
	glRectangle(%x0, %y0, 1, %boxH);
	glRectangle(%x0 + %w - 1, %y0, 1, %boxH);

	// rows
	glSetFont("Verdana", %fontUse, $GLEX_SMOOTH, 0);
	%ty = %y0 + %pad;
	for(%i = 0; %i < $KH::exRows; %i++)
	{
		if($KH::exRowTxt[%i] != "")
			glDrawString(%x0 + %pad, %ty, $KH::exRowTxt[%i]);
		%ty += %lineH;
	}
}

// ============================================
// Cast bar render (spell name + countdown bar)
// ============================================

function kronos::cast_onrender()
{
	%now = GetSimTime();

	// Brief "Interrupted!" flash after a cast stop
	if($KH::castInterrupted > 0)
	{
		%age = kronos::simAge($KH::castInterrupted);
		if(%age < 0.8)
		{
			glDisable($GL_TEXTURE_2D);
			glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);
			glColor4ub(30, 8, 8, 150);
			vhud::render_box("pos", "size");
			glColor4ub(255, 90, 90, 235);
			vhud::render_text("cast_name_pos", "Verdana", "cast_font", $GLEX_SMOOTH, "Interrupted!");
		}
		else
		{
			$KH::castInterrupted = 0;
			$KH::castName = "";
		}
		return;
	}

	// No active cast
	if($KH::castName == "" || %now >= $KH::castEnd)
		return;

	kronos::backdrop();

	// Bar drains from full to empty over the recovery time
	%total = $KH::castEnd - $KH::castStart;
	%remaining = $KH::castEnd - %now;
	%pct = 0;
	if(%total > 0)
		%pct = %remaining / %total;

	// Violet while the spell is still casting, dim blue-grey once it
	// has fired and we're just recovering
	if(%now < $KH::castFire)
		kronos::draw_bar("cast_bar_pos", "cast_bar_br", %pct, 150, 90, 220);
	else
		kronos::draw_bar("cast_bar_pos", "cast_bar_br", %pct, 95, 105, 150);

	// Spell name (left) and remaining seconds (right)
	glColor4ub(235, 225, 255, 235);
	vhud::render_text("cast_name_pos", "Verdana", "cast_font", $GLEX_SMOOTH, $KH::castName);

	%dispTime = floor(%remaining * 10) / 10;
	glColor4ub(255, 255, 255, 220);
	vhud::render_text("cast_time_pos", "Verdana", "cast_font", $GLEX_SMOOTH, %dispTime @ "s");
}

// ============================================
// Equipped-weapon bar render (persistent strip, bottom-center)
// ============================================

function kronos::wbar_onrender()
{
	if($KH::curWeapon == "" || !$KH::hasData)
		return;

	kronos::backdrop();

	%pos = vhud::render_value("pos");
	%size = vhud::render_value("size");
	%bx = getword(%pos, 0);
	%by = getword(%pos, 1);
	%bw = getword(%size, 0);
	%bh = getword(%size, 1);

	// centered weapon name
	glSetFont("Verdana", floor(%bh * 0.58), $GLEX_SMOOTH, 0);
	%dim = glGetStringDimensions($KH::curWeapon);
	%tx = %bx + floor((%bw - getword(%dim, 0)) / 2);
	%ty = %by + floor((%bh - getword(%dim, 1)) / 2);
	glColor4ub(235, 240, 255, 235);
	glDrawString(%tx, %ty, $KH::curWeapon);
}

// ============================================
// Weapon info popup render (~3s on weapon switch)
// ============================================

function kronos::weapon_onrender()
{
	if($KH::wpnRows == "" || $KH::wpnRows < 1 || $KH::wpnTime == "")
		return;

	// Hold solid for 2.4s, fade out over the last 0.6s
	%elapsed = kronos::simAge($KH::wpnTime);
	if(%elapsed > 3.0)
		return;

	%alpha = 230;
	if(%elapsed > 2.4)
	{
		%fade = (%elapsed - 2.4) / 0.6;
		%alpha = floor(230 - (230 * %fade));
		if(%alpha < 0)
			%alpha = 0;
	}

	if($KH::wpnCachedA != %alpha)
		KronosHUD_ColorizeWeaponRows(%alpha);

	%pos = vhud::render_value("pos");
	%size = vhud::render_value("size");
	%bx = getword(%pos, 0);
	%by = getword(%pos, 1);
	%bw = getword(%size, 0);
	%bh = getword(%size, 1);

	// fixed row slots sized for a full 6-row print; backdrop shrinks
	// to the actual row count so short prints don't float in space
	%rowH = floor(%bh / 6);
	%useH = %rowH * ($KH::wpnRows + 1);

	// --- backdrop ---
	glDisable($GL_TEXTURE_2D);
	glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);

	glColor4ub($KT::bgR, $KT::bgG, $KT::bgB, floor(%alpha * 0.72));
	glRectangle(%bx, %by, %bw, %useH);

	glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, floor(%alpha * 0.85));
	glRectangle(%bx, %by, %bw, 2);
	glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, floor(%alpha * 0.35));
	glRectangle(%bx, %by + %useH - 1, %bw, 1);
	glRectangle(%bx, %by, 1, %useH);
	glRectangle(%bx + %bw - 1, %by, 1, %useH);

	// --- rows (inline color tags carry the alpha) ---
	%pad = floor(%bw * 0.03);
	%titleFont = floor(%rowH * 0.82);
	%rowFont = floor(%rowH * 0.66);

	%ty = %by + floor(%rowH * 0.45);
	glSetFont("Verdana", %titleFont, $GLEX_SMOOTH, 4);
	glDrawString(%bx + %pad, %ty, $KH::wpnRowTxt[0]);

	%ty += %rowH + floor(%rowH * 0.2);
	glSetFont("Verdana", %rowFont, $GLEX_SMOOTH, 0);
	for(%i = 1; %i < $KH::wpnRows; %i++)
	{
		glDrawString(%bx + %pad, %ty, $KH::wpnRowTxt[%i]);
		%ty += %rowH;
	}
}

// ============================================
// Target frame render (enemy name + HP bar)
// ============================================

function kronos::target_onrender()
{
	// Only show if we have a recent target
	if($KH::targetName == "" || $KH::targetName == -1)
		return;

	// Server LOS scan refreshes targetTime every 0.5s while we're
	// looking at the target, so the frame stays solid while aiming
	// and fades out quickly once we look away.
	%elapsed = kronos::simAge($KH::targetTime);
	if(%elapsed > 2.2)
	{
		// Target expired
		$KH::targetName = "";
		return;
	}

	// Hold solid for 1.2s, then fade out over the last 1.0s
	%alpha = 210;
	if(%elapsed > 1.2)
	{
		%fade = (%elapsed - 1.2) / 1.0;
		%alpha = floor(210 - (210 * %fade));
		if(%alpha < 0)
			%alpha = 0;
	}

	// Town bots / NPCs: server sends "NPC" in the HP slot - friendly
	// colors, name only, no HP bar or damage number
	%isNPC = ($KH::targetHp == "NPC");

	// --- Backdrop with fade ---
	glDisable($GL_TEXTURE_2D);
	glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);

	%bgAlpha = floor(%alpha * 0.65);
	glColor4ub($KT::bgR, $KT::bgG, $KT::bgB, %bgAlpha);
	vhud::render_box("pos", "size");

	// Subtle border (red for enemies, green for NPCs)
	%pos = vhud::render_value("pos");
	%size = vhud::render_value("size");
	%bx = getword(%pos, 0);
	%by = getword(%pos, 1);
	%bw = getword(%size, 0);
	%bh = getword(%size, 1);
	%borderA = floor(%alpha * 0.3);
	if(%isNPC)
		glColor4ub(80, 165, 100, %borderA);
	else
		glColor4ub(180, 60, 60, %borderA);
	glRectangle(%bx, %by, %bw, 1);
	glRectangle(%bx, %by + %bh - 1, %bw, 1);
	glRectangle(%bx, %by, 1, %bh);
	glRectangle(%bx + %bw - 1, %by, 1, %bh);

	// --- Target name (centered in the bubble) ---
	// Was drawn at the fixed tgt_name_pos anchor, which sat off-center in the
	// frame. Center horizontally on the frame width; vertically center in the
	// whole bubble for NPCs (name-only) or in the space above the HP bar.
	%nameFont = vhud::render_value("tgt_name_font");
	glSetFont("Verdana", %nameFont, $GLEX_SMOOTH, 5);
	%ntw = getword(glGetStringDimensions($KH::targetName), 0);
	%nx = %bx + floor((%bw - %ntw) / 2);
	if(%isNPC)
		%nameAreaH = %bh;
	else
		%nameAreaH = getword(vhud::render_value("tgt_bar_pos"), 1) - %by;
	if(%nameAreaH < %nameFont)
		%nameAreaH = %nameFont;
	%ny = %by + floor((%nameAreaH - %nameFont) / 2);
	if(%isNPC)
		glColor4ub(195, 235, 205, %alpha);
	else
		glColor4ub(255, 195, 195, %alpha);
	glDrawString(%nx, %ny, $KH::targetName);

	// NPC frame is just the nameplate
	if(%isNPC)
		return;

	// --- Target HP bar ---
	%tgtPct = $KH::targetHp / 100;
	if(%tgtPct > 1)
		%tgtPct = 1;
	if(%tgtPct < 0)
		%tgtPct = 0;

	// Enemy HP color: red shades
	if(%tgtPct > 0.6)
	{
		%tr = 175;
		%tg = 45;
		%tb = 45;
	}
	else if(%tgtPct > 0.3)
	{
		%tr = 195;
		%tg = 110;
		%tb = 45;
	}
	else
	{
		%tr = 210;
		%tg = 35;
		%tb = 35;
	}

	// Bar
	// glDrawString (used for the name above) re-enables GL_TEXTURE_2D;
	// textures must be off again or the bar rectangles render with the
	// font texture bound and come out invisible
	glDisable($GL_TEXTURE_2D);
	glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);

	%barPos = vhud::render_value("tgt_bar_pos");
	%barBr = vhud::render_value("tgt_bar_br");
	%barX = getword(%barPos, 0);
	%barY = getword(%barPos, 1);
	%barW = getword(%barBr, 0) - %barX;
	%barH = getword(%barBr, 1) - %barY;
	%barFill = floor(%barW * %tgtPct);
	%barBgA = floor(%alpha * 0.45);
	%barFillA = floor(%alpha * 0.85);

	glColor4ub(0, 0, 0, %barBgA);
	glRectangle(%barX, %barY, %barW, %barH);

	if(%barFill > 0)
	{
		glColor4ub(%tr, %tg, %tb, %barFillA);
		glRectangle(%barX, %barY, %barFill, %barH);
	}

	// --- HP percentage text ---
	glColor4ub(255, 255, 255, %alpha);
	%hpPos = vhud::render_value("tgt_hp_pos");
	glSetFont("Verdana", vhud::render_value("tgt_hp_font"), $GLEX_SMOOTH, 5);
	glDrawString(getword(%hpPos, 0), getword(%hpPos, 1), floor($KH::targetHp) @ "%");

	// --- Combat readout row (top-left, above the bubble) ---
	// Fixed slots so nothing overlaps: MISS | LCK xN | -DAMAGE (the damage
	// slot is last so a huge number can trail off to the right un-clipped)
	%dmgFont = vhud::render_value("tgt_name_font");
	KronosHUD::renderPlateRow(%bx, %by - %dmgFont - 2, %dmgFont,
		$KH::tMissTime, $KH::tLckN, $KH::tLckTime, $KH::tDmgSum, $KH::tDmgTime,
		255, 170, 60);
}

// Shared MISS | LCK xN | -DMG slot row (target plate + own plate).
// Each slot shows for 1.5s after its stamp, fading over the last 0.5s.
function KronosHUD::plateAlpha(%t)
{
	if(%t == "" || %t == 0)
		return 0;
	%age = kronos::simAge(%t);
	if(%age >= 1.5)
		return 0;
	if(%age <= 1.0)
		return 235;
	%a = floor(235 * (1 - ((%age - 1.0) / 0.5)));
	if(%a < 0)
		%a = 0;
	return %a;
}

function KronosHUD::renderPlateRow(%x, %y, %font, %missT, %lckN, %lckT, %dmgSum, %dmgT, %dr, %dg, %db)
{
	%aMiss = KronosHUD::plateAlpha(%missT);
	%aLck = KronosHUD::plateAlpha(%lckT);
	%aDmg = KronosHUD::plateAlpha(%dmgT);
	if(%aMiss <= 0 && %aLck <= 0 && %aDmg <= 0)
		return;

	glSetFont("Verdana", %font, $GLEX_SMOOTH, 5);
	// fixed columns: MISS slot, LCK slot, then damage trailing right
	%missW = getword(glGetStringDimensions("MISS"), 0);
	%lckW = getword(glGetStringDimensions("LCK x9"), 0);
	%gap = floor(%font * 0.6);
	%c2 = %x + %missW + %gap;
	%c3 = %c2 + %lckW + %gap;

	if(%aMiss > 0)
	{
		glColor4ub(210, 215, 225, %aMiss);
		glDrawString(%x, %y, "MISS");
	}
	if(%aLck > 0)
	{
		if(%lckN > 1)
			%lckText = "LCK x" @ %lckN;
		else
			%lckText = "LCK";
		glColor4ub(255, 235, 120, %aLck);
		glDrawString(%c2, %y, %lckText);
	}
	if(%aDmg > 0 && %dmgSum != 0)
	{
		glColor4ub(%dr, %dg, %db, %aDmg);
		glDrawString(%c3, %y, "-" @ %dmgSum);
	}
}

// ============================================
// Stock HUD hiding
// ============================================
// The KronosHUD bars replace the stock health/energy bars, so hide
// them. ScriptGL renders in any hardware OpenGL mode (fullscreen or
// windowed), so no fallback is needed - use kronos::showStockHuds()
// to manually restore the stock bars if ever needed.

// User-configurable: set to false (before exec) to keep a stock HUD.
// weaponHud is hidden by default too - the repack normally hides it
// via GraphicPlugin, this just makes sure it stays hidden.
if($KH::hideStockHealth == "")
	$KH::hideStockHealth = true;
if($KH::hideStockEnergy == "")
	$KH::hideStockEnergy = true;
if($KH::hideStockWeapon == "")
	$KH::hideStockWeapon = true;

function kronos::applyStockHudVisibility()
{
	// master kill switch (khOff/khOn): never hide the stock bars while off
	if($pref::Kronos::hudOff)
		return;
	// Only replace the stock HUDs once a Kronos server is actually pushing data.
	// This function is called EVERY frame from onPreDraw, so gating on $KH::hasData
	// makes the HUD self-disable on non-Kronos servers: the stock health/energy/weapon
	// bars stay visible instead of being hidden with empty Kronos bars over them. As
	// soon as a Kronos server sends remoteKronosHUD ($KH::hasData=true) the next frame
	// hides them. showStockHuds() also clears hasData so a manual restore sticks.
	if(!$KH::hasData)
		return;
	if($KH::hideStockHealth)
		Control::SetVisible(healthHud, false);
	if($KH::hideStockEnergy)
		Control::SetVisible(jetPackHud, false);
	if($KH::hideStockWeapon)
		Control::SetVisible(weaponHud, false);
}

// Manual restore (console helper)
function kronos::showStockHuds()
{
	Control::SetVisible(healthHud, true);
	Control::SetVisible(jetPackHud, true);
	Control::SetVisible(weaponHud, true);
}

// Hook PlayGui open (the stock HUD objects exist once PlayGui is up).
// Tagged attaches are safe against re-exec of this file.
Include("presto\\Event.cs");
Event::Attach(eventGuiOpen_PlayGui, "kronos::applyStockHudVisibility();", attachKronosHud);
Event::Attach(eventScreenModeChanged, "kronos::applyStockHudVisibility();", attachKronosHud);

// Re-apply the saved chat-window position when PlayGui opens or the
// resolution changes (the engine resets chatDisplayHud to stock then).
// KronosMenu::applyChatPos lives in KronosMenu.cs (loaded alongside).
Event::Attach(eventGuiOpen_PlayGui, "KronosMenu::applyChatPos();", attachKronosChat);
Event::Attach(eventScreenModeChanged, "KronosMenu::applyChatPos();", attachKronosChat);

// ============================================
// ScriptGL pre-draw hook - feed vhud the real canvas size
// ============================================
// scriptgl2.cs defines onPreDraw -> vhud::render(%dimensions). The raw
// ScriptGL %dimensions can be stale/wrong in windowed OpenGL, which
// pins the vhud HUD to a fixed size in a screen corner. Override it
// (this file loads after scriptgl2.cs) to feed vhud the engine's live
// canvas extent - same authoritative basis the TAB menu/shop use.
// KronosMenu::screenDim lives in KronosMenu.cs (loaded alongside);
// guard against it being absent so a HUD-only setup still works.
function ScriptGL::playGui::onPreDraw(%dimensions)
{
	// master kill switch (khOff/khOn): skip ALL HUD overlay drawing -
	// vitals/target/cast/info panels and the damage floats. The TAB
	// menu/shop/NPC panels (onPostDraw) stay: the stock score gui is
	// binary-patched off-screen, so they remain the only menu UI.
	if($pref::Kronos::hudOff)
		return;

	// Re-assert stock-HUD + stock-chat hiding EVERY frame (ported from 1.40):
	// the engine WAKES the stock bars when the player SPAWNS, which happens
	// AFTER eventGuiOpen, so the one-shot hide gets undone on spawn/respawn.
	// SetVisible(false) on an already-hidden control is a cheap no-op.
	kronos::applyStockHudVisibility();
	KronosChat::applyVisibility();

	if($KH::fpsProbe)
		KronosHUD::fpsSample();
	if($KM::enabled != "")
		vhud::render( KronosMenu::screenDim(%dimensions) );
	else
		vhud::render(%dimensions);

	// FloatStyle3 damage text draws LAST so it sits on top of the target
	// frame / nameplates (the old engine-control path drew under everything)
	%fd = %dimensions;
	if($KM::enabled != "")
		%fd = KronosMenu::screenDim(%dimensions);
	KronosHUD::renderFloats(getword(%fd, 0), getword(%fd, 1));
}

// ============================================
// FloatStyle3 ("pop") damage text - ScriptGL floater
// ============================================
// ATKText's pop style drew through engine FearGuiFormattedText controls,
// which render UNDER every ScriptGL overlay (so damage numbers hid behind
// the target nameplate) and have no scalable font. Route the "pop" style
// to this ScriptGL floater instead: draws on top, rises + fades, and the
// size follows $pref::Kronos::dmgTextScale (persisted; tune live with
// KronosHUD::setDmgTextScale(1.4) etc. - separate from the UI scale).
// All other ATKText styles keep the stock path.

if($pref::Kronos::dmgTextScale == "")
	$pref::Kronos::dmgTextScale = 1.0;

// probe once at load: does the DLL provide the millisecond clock?
// (old kronos_textinput.dll -> unknown command -> "" -> sim-clock fallback)
$KH::hasTicks = (glTicks() != "");

// HUD UPDATE RATE - the root cause of "jittery" HUD animation: Hudbot only
// calls the ScriptGL onDraw hooks every $ScriptGL::Latency MILLISECONDS
// (default 100 = ~10fps), replaying the last frame's GL calls in between
// ("carbon copied" per the Hudbot docs). khFps measured exactly that. 15ms
// is the documented floor (~60+/sec); we default there and persist a knob:
//   KronosHUD::setHudFps(30);   // budget machines - halves script cost
if($pref::Kronos::hudLatency == "")
	$pref::Kronos::hudLatency = 15;
$ScriptGL::Latency = $pref::Kronos::hudLatency;

function KronosHUD::setHudFps(%fps)
{
	if(%fps < 1 || %fps == "")
		%fps = 60;
	%lat = floor(1000 / %fps);
	if(%lat < 15)
		%lat = 15;   // Hudbot's documented minimum useful interval
	$pref::Kronos::hudLatency = %lat;
	$ScriptGL::Latency = %lat;
	echo("KronosHUD: ScriptGL latency " @ %lat @ " ms (~" @ floor(1000 / %lat) @ " fps)");
}

$KHF::Max = 8;      // concurrent floats
$KHF::life = 1.4;   // seconds on screen

function KronosHUD::setDmgTextScale(%s)
{
	if(%s <= 0 || %s == "")
		%s = 1.0;
	$pref::Kronos::dmgTextScale = %s;
	echo("KronosHUD: damage text scale = " @ %s);
}

// Animation clock for the floats. glTicks() (kronos_textinput.dll) is a real
// millisecond wall clock - perfectly smooth per rendered frame. Older DLLs
// without it fall back to the sim clock low-passed in renderFloats (smoother
// than raw GetSimTime, but velocity still wobbles at the ~30Hz tick rate).
function KronosHUD::floatNow()
{
	if($KH::hasTicks)
		return glTicks() / 1000;
	return $KHF::clock;
}

// Remove ALL <...> tags including the brackets. String::removeBetween can
// leave the empty "<>" shells behind (the "<> in Miss floats" bug) and
// unknown server formats arrive with their tags intact - cut them by hand.
function KronosHUD::stripTags(%text)
{
	%guard = 0;
	while(%guard < 24)
	{
		%guard++;
		%lt = String::findSubStr(%text, "<");
		if(%lt == -1)
			return %text;
		%rest = String::getSubStr(%text, %lt + 1, 99999);
		%gt = String::findSubStr(%rest, ">");
		if(%gt == -1)
			return %text;   // unmatched "<" - leave as-is
		%text = String::getSubStr(%text, 0, %lt) @ String::getSubStr(%rest, %gt + 1, 99999);
	}
	return %text;
}

function KronosHUD::addFloat(%text, %viewType, %dir)
{
	// strip <jc>/<fN>/... tags and newlines - color comes from viewType
	%text = KronosHUD::stripTags(%text);
	%text = String::replace(%text, "\n", "  ");
	if(%text == "")
		return;
	if(%dir == "" || %dir == 0)
		%dir = 1;   // 1 = rise (default), -1 = sink (nameplate defender)
	%now = KronosHUD::floatNow();

	// ANTI-OVERLAP CHAIN: if the previous float in this direction hasn't
	// moved a full text-height away from the spawn line yet, start this one
	// a text-height further back so rapid hits never overprint. Offsets are
	// fractions of screen height; rise rate is 0.11 sh/sec (matches render).
	%off = 0;
	if($KHF::lastT0[%dir] != "")
	{
		%gap = 0.048 * $pref::Kronos::dmgTextScale;   // ~popped text height
		%prev = $KHF::lastOff[%dir] - ((%now - $KHF::lastT0[%dir]) * 0.11);
		if(%prev + %gap > %off)
			%off = %prev + %gap;
		if(%off > 0.30)
			%off = 0.30;   // cap a machine-gun stream on screen
	}
	$KHF::lastT0[%dir] = %now;
	$KHF::lastOff[%dir] = %off;

	%i = $KHF::next + 0;
	$KHF::next = %i + 1;
	if($KHF::next >= $KHF::Max)
		$KHF::next = 0;
	$KHF::text[%i] = %text;
	$KHF::view[%i] = %viewType;
	$KHF::t0[%i] = %now;
	$KHF::off[%i] = %off;
	$KHF::dir[%i] = %dir;
}

function KronosHUD::renderFloats(%sw, %sh)
{
	// SMOOTH CLOCK: prefer the DLL's real millisecond clock (glTicks);
	// otherwise low-pass the coarse ~30Hz sim clock so positions advance
	// every rendered frame instead of stepping per tick. Hard resync on any
	// big jump (mission change / getSimTime rebase).
	if($KH::hasTicks)
		%now = glTicks() / 1000;
	else
	{
		%sim = GetSimTime();
		if($KHF::clock == "" || %sim - $KHF::clock > 1.0 || %sim - $KHF::clock < -1.0)
			$KHF::clock = %sim;
		else
			$KHF::clock = $KHF::clock + ((%sim - $KHF::clock) * 0.2);
		%now = $KHF::clock;
	}
	%base = floor(%sh * 0.032 * $pref::Kronos::dmgTextScale);
	if(%base < 10)
		%base = 10;
	%drawn = 0;
	for(%i = 0; %i < $KHF::Max; %i++)
	{
		if($KHF::text[%i] == "")
			continue;
		%age = %now - $KHF::t0[%i];
		if(%age > $KHF::life)
		{
			$KHF::text[%i] = "";
			continue;
		}
		%alpha = 240;
		if(%age > $KHF::life - 0.5)
		{
			%alpha = floor(240 * ($KHF::life - %age) / 0.5);
			if(%alpha < 0)
				%alpha = 0;
		}
		// the "pop": oversized for the first 0.12s, settles to base
		%font = %base;
		if(%age < 0.12)
			%font = floor(%base * (1.4 - (%age / 0.12) * 0.4));
		glSetFont("Verdana", %font, $GLEX_SMOOTH, 5);
		%tw = getword(glGetStringDimensions($KHF::text[%i]), 0);
		%x = (%sw - %tw) / 2;
		// travel from just below screen center; the spawn offset chain set
		// in addFloat keeps rapid hits from overprinting; dir -1 = sink.
		// Coordinates stay FRACTIONAL (no floor) - ScriptGL hands them to GL
		// as floats, so the glide is sub-pixel smooth instead of stepping
		// whole pixels at uneven intervals.
		%y = (%sh * (0.44 + ($KHF::off[%i] * $KHF::dir[%i]))) - ((%age * %sh * 0.11) * $KHF::dir[%i]);
		if($KHF::view[%i] == "attacker")
			glColor4ub(255, 205, 80, %alpha);       // damage you deal - gold
		else if($KHF::view[%i] == "defender")
			glColor4ub(255, 95, 80, %alpha);        // damage you take - red
		else
			glColor4ub(225, 230, 240, %alpha);      // spectator - white
		glDrawString(%x, %y, $KHF::text[%i]);
		%drawn++;
	}
}

// Route "pop" (FloatStyle3) to the ScriptGL floater; every other style
// falls through to the stock ATKText engine-control path unchanged.
// (This override loads AFTER Presto's ATKText.cs - autoexec order.)
function remoteATKText(%server, %text, %animationStyle, %viewType)
{
	if(%server != 2048)
		return;
	if(%viewType == "" || %viewType == -1)
		%viewType = "defender";
	// khOff(): route our custom styles back to the stock engine-control path
	if($pref::Kronos::hudOff
		&& (%animationStyle == "pop" || %animationStyle == "nameplate"))
	{
		%animationStyle = $animationStyle;
		if(%animationStyle == "pop" || %animationStyle == "nameplate")
			%animationStyle = "float";   // stock default is also custom -> stock float
	}
	if(%animationStyle == "pop")
	{
		KronosHUD::addFloat(%text, %viewType);
		return;
	}
	if(%animationStyle == "nameplate")
	{
		// nameplate mode: everything renders on the PLATES, not as floats.
		// Attacker messages reaching here are only "Miss!" (dealt damage
		// goes to the target plate via KronosTarget) -> target MISS slot.
		// Defender messages feed the OWN plate above the vitals bars.
		%plain = KronosHUD::stripTags(%text);
		if(%viewType == "attacker")
		{
			$KH::tMissTime = GetSimTime();
			return;
		}
		if(%viewType == "defender")
		{
			KronosHUD::ownHit(%plain);
			return;
		}
		// spectators shouldn't reach here (server converts to pop) - float it
		KronosHUD::addFloat(%text, %viewType);
		return;
	}
	// stock validation: unknown/legacy style values fall back to the default
	if(%animationStyle != "float" && %animationStyle != "redmoon" && %animationStyle != "wow"
		&& %animationStyle != "test")
		%animationStyle = $animationStyle;
	atktext::set(%text, %animationStyle, %viewType);
}

// ============================================
// Initialize
// ============================================

$KH::hasData = false;
$KH::targetName = "";
$KH::targetTime = 0;
$KH::castName = "";
$KH::castEnd = 0;
$KH::castInterrupted = 0;
$KH::wpnRows = 0;
$KH::wpnTime = "";
$KH::wpnCachedA = -1;
$KH::curWeapon = "";
$KH::exTime = "";
$KH::exRows = 0;
$KH::exMeasured = false;
$KH::exDispA = -1;
$KH::lastHandshake = "";

// Build all HUD elements (registers with vhud linked list)
kronos::create();

// If we're exec'd mid-game, PlayGui is already open - apply now
if($Mode::PlayMode)
	kronos::applyStockHudVisibility();

// ============================================
// Render-cadence probe: khFps(); from the console, then watch chat/console.
// Samples 120 consecutive ScriptGL frames with the real clock and reports
// average fps + the worst single frame gap. If the average is ~30, the
// CLIENT is rendering at 30 - no animation can be smoother than that.
// ============================================
function khFps()
{
	if(!$KH::hasTicks)
	{
		echo("khFps: glTicks not available (old DLL)");
		return;
	}
	$KH::fpsN = 0;
	$KH::fpsLast = "";
	$KH::fpsSum = 0;
	$KH::fpsMax = 0;
	$KH::fpsProbe = true;
	echo("khFps: sampling 120 frames...");
}

function KronosHUD::fpsSample()
{
	%t = glTicks();
	if($KH::fpsLast != "")
	{
		%dt = %t - $KH::fpsLast;
		$KH::fpsSum += %dt;
		if(%dt > $KH::fpsMax)
			$KH::fpsMax = %dt;
		$KH::fpsN++;
		if($KH::fpsN >= 120)
		{
			$KH::fpsProbe = false;
			%avg = $KH::fpsSum / $KH::fpsN;
			echo("khFps: avg " @ floor((1000 / %avg) + 0.5) @ " fps (frame " @ floor(%avg * 10) / 10
				@ " ms, worst gap " @ floor($KH::fpsMax * 10) / 10 @ " ms)");
		}
	}
	$KH::fpsLast = %t;
}

// Console convenience: reload the whole Kronos suite in load order
// (all scripts are re-exec safe - vhud::create won't double-register and
// the Event attaches are tagged). Usage from the console:  khReload();
function khReload()
{
	exec("Presto\\KronosHUD.cs");
	exec("Presto\\ChatFilter.cs");
	exec("Presto\\KronosInput.cs");
	exec("Presto\\KronosMenu.cs");
	exec("Presto\\KronosShop.cs");
	exec("Presto\\KronosChat.cs");
	exec("Presto\\KronosNPC.cs");
	exec("Presto\\KronosCM.cs");
	exec("Presto\\KronosStats.cs");
	echo("khReload: Kronos suite re-exec'd");
}

// ============================================
// Master kill switch: khOff(); / khOn(); / khToggle();
// ============================================
// khOff() returns to the stock GUI: all HUD overlays stop drawing
// (vitals/target/cast/info/damage floats), the stock health/energy/
// weapon bars and stock chat come back, Y opens stock chat, and pop/
// nameplate damage text falls back to the stock float style. Persists
// ($pref::Kronos::hudOff). EXCEPTION: the TAB menu / shop / NPC panels
// stay Kronos-styled - the stock score gui is binary-patched off-screen
// (base\gui .stockbak files must be restored manually for that).
function khOff()
{
	$pref::Kronos::hudOff = true;
	kronos::showStockHuds();          // applyStockHudVisibility is now inert
	KronosChat::applyVisibility();    // stock chat back
	KronosChat::bindTalkKey();        // Y -> stock chat
	KronosMenu::savePrefs();
	echo("KronosHUD: OFF - stock HUD/chat restored (TAB menu stays Kronos-");
	echo("  styled; the stock score gui is patched off-screen). khOn(); to re-enable.");
}

function khOn()
{
	$pref::Kronos::hudOff = false;
	kronos::applyStockHudVisibility();
	KronosChat::applyVisibility();
	KronosChat::bindTalkKey();
	KronosMenu::savePrefs();
	echo("KronosHUD: ON");
}

function khToggle()
{
	if($pref::Kronos::hudOff)
		khOn();
	else
		khOff();
}

echo("KronosHUD: ScriptGL RPG HUD loaded");
