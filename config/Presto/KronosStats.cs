//==============================================
// KronosStats.cs - session stats panel (XP/hr, gold/hr) for Kingdom of Kronos
//==============================================
// Pure client-side: samples the vitals the server already pushes for the HUD
// ($KH::exp / $KH::gold / $KH::xpNext, remoteKronosHUD in KronosHUD.cs) and
// shows session time, XP gained + per hour, net gold + per hour, and an ETA
// to the next level. No server traffic at all.
//
// - XP counts POSITIVE deltas only, so a remort/death XP reset doesn't wipe
//   the session's earn rate.
// - Gold is the NET change in carried gold (spending and bank deposits count
//   against it - it answers "am I up or down this session?").
// - Session time accumulates real played seconds and is immune to the
//   GetSimTime() reset on reconnect (per-frame deltas, outliers dropped).
//
// Draggable anywhere (whole panel is the handle, position persists). While
// the cursor is up it shows [R]eset and [x]hide chips. Console:
//   KronosStats::toggle();   KronosStats::reset();
// Rendered from the shared onPostDraw chain (KronosShop.cs); drag + click
// wiring lives in KronosMenu.cs ("kstats").
//==============================================

if($pref::Kronos::statsOn == "")  $pref::Kronos::statsOn = true;
if($pref::Kronos::statsX == "")   $pref::Kronos::statsX = 0.845;
if($pref::Kronos::statsY == "")   $pref::Kronos::statsY = 0.30;

// ---- session accumulators ----
$KST::elapsed = 0;      // played seconds this session
$KST::lastT = "";       // last sample sim-time ("" = not started)
$KST::xpGain = 0;       // sum of positive XP deltas
$KST::goldGain = 0;     // net carried-gold change
$KST::xpPrev = "";
$KST::goldPrev = "";
$Panel::kstatsShown = false;
$KST::btnShown = false;

function KronosStats::reset()
{
	$KST::elapsed = 0;
	$KST::lastT = "";
	$KST::xpGain = 0;
	$KST::goldGain = 0;
	$KST::xpPrev = "";
	$KST::goldPrev = "";
	echo("KronosStats: session counters reset");
}

function KronosStats::toggle()
{
	if($pref::Kronos::statsOn)
		$pref::Kronos::statsOn = "";
	else
		$pref::Kronos::statsOn = true;
	KronosMenu::savePrefs();
}

// "75m" under 2h, else "3h 24m"
function KronosStats::fmtTime(%sec)
{
	%m = floor(%sec / 60);
	if(%m < 120)
		return %m @ "m";
	%h = floor(%m / 60);
	return %h @ "h " @ (%m - (%h * 60)) @ "m";
}

function KronosStats::render(%sw, %sh)
{
	$Panel::kstatsShown = false;
	$KST::btnShown = false;
	if(!$pref::Kronos::statsOn || !$KM::enabled)
		return;
	if(!$KH::hasData)
		return;

	// ---- sample (per frame; deltas keep it correct across clock resets) ----
	%now = GetSimTime();
	if($KST::lastT == "")
	{
		$KST::lastT = %now;
		$KST::xpPrev = $KH::exp;
		$KST::goldPrev = $KH::gold;
	}
	%dt = %now - $KST::lastT;
	if(%dt > 0 && %dt < 5)
		$KST::elapsed = $KST::elapsed + %dt;
	$KST::lastT = %now;

	%dx = $KH::exp - $KST::xpPrev;
	if(%dx > 0)
		$KST::xpGain = $KST::xpGain + %dx;
	$KST::xpPrev = $KH::exp;
	%dg = $KH::gold - $KST::goldPrev;
	$KST::goldGain = $KST::goldGain + %dg;
	$KST::goldPrev = $KH::gold;

	// ---- rates ----
	%xpHr = 0;
	%goldHr = 0;
	if($KST::elapsed >= 60)   // rates are noise for the first minute
	{
		%xpHr = floor($KST::xpGain * 3600 / $KST::elapsed);
		%goldHr = floor($KST::goldGain * 3600 / $KST::elapsed);
	}

	// ETA to next level from the session XP rate
	%eta = "";
	if(%xpHr > 0 && $KH::xpNext > $KH::exp)
	{
		%etaMin = floor(($KH::xpNext - $KH::exp) * 60 / %xpHr);
		if(%etaMin < 1)
			%etaMin = 1;
		%eta = KronosStats::fmtTime(%etaMin * 60);
	}

	// ---- layout ----
	%k = KronosMenu::uiScale(%sh);
	%lineH = floor(%sh * %k * 0.024);
	if(%lineH < 11)
		%lineH = 11;
	%font = floor(%lineH * 0.74);
	%pad = floor(%lineH * 0.4);
	if(%pad < 3)
		%pad = 3;

	%l1 = "Session  " @ KronosStats::fmtTime($KST::elapsed);
	%l2 = "XP    +" @ KronosShop::commafy($KST::xpGain) @ "  (" @ KronosShop::commafy(%xpHr) @ "/hr)";
	%g = $KST::goldGain;
	%gs = "+";
	if(%g < 0)
	{
		%gs = "-";
		%g = 0 - %g;
	}
	%l3 = "Gold  " @ %gs @ KronosShop::commafy(%g) @ "  (" @ KronosShop::commafy(%goldHr) @ "/hr)";
	%l4 = "";
	if(%eta != "")
		%l4 = "Next level  ~" @ %eta;

	%lines = 3;
	if(%l4 != "")
		%lines = 4;

	glSetFont("Verdana", %font, $GLEX_SMOOTH, 0);
	%w = getword(glGetStringDimensions(%l2), 0);
	%t = getword(glGetStringDimensions(%l3), 0);
	if(%t > %w)
		%w = %t;
	%t = getword(glGetStringDimensions(%l1), 0) + (%lineH * 3);   // room for the R / x chips
	if(%t > %w)
		%w = %t;
	if(%l4 != "")
	{
		%t = getword(glGetStringDimensions(%l4), 0);
		if(%t > %w)
			%w = %t;
	}
	%w = %w + (%pad * 2);
	%h = (%lines * %lineH) + (%pad * 2);

	%x = floor($pref::Kronos::statsX * %sw);
	%y = floor($pref::Kronos::statsY * %sh);
	if(%x + %w > %sw - 2)
		%x = %sw - 2 - %w;

	// drag handle (whole panel)
	$Panel::kstatsX = %x;  $Panel::kstatsY = %y;
	$Panel::kstatsW = %w;  $Panel::kstatsH = %h;
	$Panel::kstatsShown = true;

	// ---- rects ----
	glDisable($GL_TEXTURE_2D);
	glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);
	glColor4ub($KT::bgR, $KT::bgG, $KT::bgB, 150);
	glRectangle(%x, %y, %w, %h);
	glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, 130);
	glRectangle(%x, %y, %w, 1);
	glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, 55);
	glRectangle(%x, %y + %h - 1, %w, 1);
	glRectangle(%x, %y, 1, %h);
	glRectangle(%x + %w - 1, %y, 1, %h);

	// cursor-up chips: [R]eset + [x] hide, top-right
	if($KM::mouseOn)
	{
		%cz = %lineH - 2;
		%cxX = %x + %w - %pad - %cz;
		%cxR = %cxX - %cz - 2;
		%cy = %y + %pad;
		glColor4ub($KT::chR, $KT::chG, $KT::chB, 190);
		glRectangle(%cxR, %cy, %cz, %cz);
		glColor4ub(150, 70, 70, 190);
		glRectangle(%cxX, %cy, %cz, %cz);
		$KST::rX = %cxR;  $KST::xX = %cxX;  $KST::bY = %cy;  $KST::bS = %cz;
		$KST::btnShown = true;
	}

	// ---- text ----
	%tx = %x + %pad;
	%ty = %y + %pad;
	glColor4ub($KT::txR, $KT::txG, $KT::txB, 230);
	glSetFont("Verdana", %font, $GLEX_SMOOTH, 0);
	glDrawString(%tx, %ty, %l1);
	if($KST::btnShown)
	{
		%cf = floor($KST::bS * 0.72);
		glSetFont("Verdana", %cf, $GLEX_SMOOTH, 0);
		glColor4ub(235, 240, 255, 240);
		glDrawString($KST::rX + floor($KST::bS * 0.22), $KST::bY + floor($KST::bS * 0.1), "R");
		glDrawString($KST::xX + floor($KST::bS * 0.26), $KST::bY + floor($KST::bS * 0.1), "x");
		glSetFont("Verdana", %font, $GLEX_SMOOTH, 0);
	}
	%ty += %lineH;
	glColor4ub(220, 200, 120, 235);
	glDrawString(%tx, %ty, %l2);
	%ty += %lineH;
	if($KST::goldGain < 0)
		glColor4ub(230, 130, 110, 235);
	else
		glColor4ub(120, 220, 140, 235);
	glDrawString(%tx, %ty, %l3);
	%ty += %lineH;
	if(%l4 != "")
	{
		glColor4ub(200, 210, 230, 215);
		glDrawString(%tx, %ty, %l4);
	}
}

// Click the [R] / [x] chips (dispatched from KronosMenu onMouseLMB, before the
// drag handles so the chips win over the panel's move handle).
function KronosStats::handleClick(%x, %y)
{
	if(!$KST::btnShown)
		return false;
	if(%y < $KST::bY || %y >= $KST::bY + $KST::bS)
		return false;
	if(%x >= $KST::rX && %x < $KST::rX + $KST::bS)
	{
		KronosStats::reset();
		return true;
	}
	if(%x >= $KST::xX && %x < $KST::xX + $KST::bS)
	{
		KronosStats::toggle();
		return true;
	}
	return false;
}

echo("KronosStats: session stats panel loaded (KronosStats::toggle/reset)");
