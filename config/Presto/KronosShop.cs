//==============================================
// KronosShop.cs - modern shop / inventory screen for Kingdom of Kronos
//==============================================
// Replaces the stock CmdInventory gui mode ("I" key + merchant shops)
// for HUD clients. The server (KronosHUD_Server.cs) pushes the item
// rows; actions send the SAME remoteEval messages the stock gui
// buttons sent (buyItem/sellItem/useItem/dropItem + index), so all
// server economy logic is unchanged. Vanilla clients never see any of
// this - the server gates on the KHudOn handshake.
//
// Rendering: ScriptGL panels in playGui onPostDraw (the score dialog
// is opened server-side for the mouse cursor; its stock controls are
// off-screen). Mouse input arrives via the Hudbot callbacks owned by
// KronosMenu.cs, which dispatches here while the shop is open.
//
// Layout mirrors the KronosMenu panels: left pane = your items
// (Sell/Use/Drop), right pane = merchant stock (Buy). Long lists scroll
// with a right-side scrollbar (drag the thumb, or click the track),
// wired through KronosMenu's drag system as "sbinv" / "sbst".
//==============================================

$KS::MaxRows = 14;   // max item rows shown before the pane scrolls

// ============================================
// Server pushes
// ============================================

function remoteKShopOpen(%server, %mode, %shopName)
{
	if(%server != 2048)
		return;

	$KS::open = %mode;
	$KS::shopName = %shopName;
	$KS::invN = 0;
	$KS::stN = 0;
	$KS::scroll[inv] = 0;
	$KS::scroll[st] = 0;
	$KS::sel[inv] = -1;
	$KS::sel[st] = -1;
	$KS::selItem[inv] = "";
	$KS::selItem[st] = "";
	$KS::filter[inv] = "";
	$KS::filter[st] = "";
	$KS::builtFilter[inv] = "";
	$KS::builtFilter[st] = "";
	$KS::hovId = "";
	$KS::tipFor = "";
	$KS::tipReq = "";
	$KS::tipRows = 0;
}

function remoteKShopClose(%server)
{
	if(%server != 2048)
		return;
	$KS::open = "";
	// close while typing -> drop whichever shop-owned field was focused
	if(KronosInput::isFocused("bankqty")
		|| KronosInput::isFocused("ksinv") || KronosInput::isFocused("ksst"))
		KronosInput::blur();
}

// Coin balances for bank mode pane headers (carried / banked)
function remoteKBankCoins(%server, %coins, %bank)
{
	if(%server != 2048)
		return;
	$KS::coins = %coins;
	$KS::bank = %bank;
}

// Own inventory row. %kind: "d" = ItemData (%ref = index, stock
// buyItem/sellItem protocol), "b" = belt item (%ref = belt item name,
// KShopBelt* protocol)
function remoteKShopInv(%server, %i, %kind, %ref, %cnt, %heading, %desc)
{
	if(%server != 2048)
		return;
	$KS::invKind[%i] = %kind;
	$KS::invRef[%i] = %ref;
	$KS::invCnt[%i] = %cnt;
	$KS::invHead[%i] = %heading;
	$KS::invName[%i] = %desc;
}

function remoteKShopInvCount(%server, %n)
{
	if(%server != 2048)
		return;
	$KS::invN = %n;
	KronosShop::buildDisp("inv");
}

// Merchant stock row (same kind/ref scheme as inventory rows)
function remoteKShopStock(%server, %i, %kind, %ref, %price, %heading, %desc)
{
	if(%server != 2048)
		return;
	$KS::stKind[%i] = %kind;
	$KS::stRef[%i] = %ref;
	$KS::stPrice[%i] = %price;
	$KS::stHead[%i] = %heading;
	$KS::stName[%i] = %desc;
}

function remoteKShopStockCount(%server, %n)
{
	if(%server != 2048)
		return;
	$KS::stN = %n;
	KronosShop::buildDisp("st");
}

// ============================================
// Display list - sorts rows by heading then name, inserts heading
// rows, restores the selection (by ItemData index) after a re-push
// ============================================

function KronosShop::rowKey(%p, %i)
{
	if(%p == "inv")
		return $KS::invHead[%i] @ "|" @ $KS::invName[%i];
	return $KS::stHead[%i] @ "|" @ $KS::stName[%i];
}

function KronosShop::buildDisp(%p)
{
	if(%p == "inv")
		%n = $KS::invN;
	else
		%n = $KS::stN;

	// search filter: keep only rows whose name contains the pane's filter text
	// (String::findSubStr is case-insensitive, which is what we want for search)
	%f = $KS::filter[%p];
	$KS::builtFilter[%p] = %f;
	%m = 0;
	for(%i = 0; %i < %n; %i++)
	{
		if(%f != "")
		{
			if(%p == "inv")
				%nm = $KS::invName[%i];
			else
				%nm = $KS::stName[%i];
			if(String::findSubStr(%nm, %f) == -1)
				continue;
		}
		%ord[%m] = %i;
		%m++;
	}
	%n = %m;

	// insertion sort row indices by heading|name
	for(%i = 1; %i < %n; %i++)
	{
		%v = %ord[%i];
		%vk = KronosShop::rowKey(%p, %v);
		%j = %i - 1;
		%moving = true;
		while(%moving)
		{
			if(%j < 0)
				%moving = false;
			else if(String::ICompare(KronosShop::rowKey(%p, %ord[%j]), %vk) > 0)
			{
				%ord[%j + 1] = %ord[%j];
				%j--;
			}
			else
				%moving = false;
		}
		%ord[%j + 1] = %v;
	}

	// build display rows, inserting a heading row on category change.
	// heading strings are "bWeapons" style - first char is the stock
	// sort key, the rest is the label
	%d = 0;
	%lastHead = "<none>";
	for(%i = 0; %i < %n; %i++)
	{
		%r = %ord[%i];
		if(%p == "inv")
			%head = $KS::invHead[%r];
		else
			%head = $KS::stHead[%r];
		if(%head != %lastHead)
		{
			$KS::dispType[%p, %d] = "head";
			$KS::dispRef[%p, %d] = String::getSubStr(%head, 1, 99);
			%d++;
			%lastHead = %head;
		}
		$KS::dispType[%p, %d] = "item";
		$KS::dispRef[%p, %d] = %r;
		%d++;
	}
	$KS::dispN[%p] = %d;

	// clamp scroll to the new list length (render re-clamps to the visible
	// window too), restore selection by ItemData index
	if($KS::scroll[%p] >= %d)
		$KS::scroll[%p] = %d - 1;
	if($KS::scroll[%p] < 0)
		$KS::scroll[%p] = 0;

	// restore selection - but only from the DISPLAYED rows, so a search filter
	// can never leave an invisible item selected (Deposit/Sell etc. act on the
	// selection, and acting on something you can't see is how items get lost)
	$KS::sel[%p] = -1;
	if($KS::selItem[%p] != "")
	{
		for(%i = 0; %i < %d; %i++)
		{
			if($KS::dispType[%p, %i] != "item")
				continue;
			%r = $KS::dispRef[%p, %i];
			if(%p == "inv")
				%id = $KS::invKind[%r] @ "|" @ $KS::invRef[%r];
			else
				%id = $KS::stKind[%r] @ "|" @ $KS::stRef[%r];
			if(%id == $KS::selItem[%p])
				$KS::sel[%p] = %r;
		}
		if($KS::sel[%p] == -1)
			$KS::selItem[%p] = "";
	}
}

// ============================================
// Layout (mirrors the KronosMenu panels)
// ============================================

function KronosShop::computeLayout(%sw, %sh)
{
	// SIZES scale toward the reference (szW/szH); POSITIONS anchor to the
	// real screen - mirrors KronosMenu::computeLayout so the shop matches
	// the TAB menu panels at every resolution / UI scale.
	%k = KronosMenu::uiScale(%sh);
	%szW = %sw * %k;
	%szH = %sh * %k;

	$KSL::pad    = floor(%szW * 0.012);
	$KSL::w      = floor(%szW * 0.38);
	$KSL::titleH = floor(%szH * 0.05);
	$KSL::rowH   = floor(%szH * 0.034);

	// shared movable positions: inv pane follows the menu panel, stock pane
	// follows the players panel (so dragging either screen moves both).
	$KSL::lx     = floor($pref::Kronos::menuX * %sw);
	$KSL::ly     = floor($pref::Kronos::menuY * %sh);
	$KSL::rx     = floor($pref::Kronos::playersX * %sw);
	$KSL::ry     = floor($pref::Kronos::playersY * %sh);
	// item rows start below the title AND the search (filter) row
	$KSL::invRowY0 = $KSL::ly + $KSL::titleH + $KSL::rowH + floor($KSL::pad / 2);
	$KSL::stRowY0  = $KSL::ry + $KSL::titleH + $KSL::rowH + floor($KSL::pad / 2);
}

// ============================================
// Rendering
// ============================================

function KronosShop::render(%dimensions)
{
	// drain the native key queue into the focused field (the bank amount box)
	// every frame, independent of whether chat is enabled. KronosChat::render
	// also pumps, but only when chat is on; a second pump here is a no-op.
	KronosInput::pump();

	if($KS::open == "" || $KS::open == false)
	{
		$Panel::sbInvShown = false;
		$Panel::sbStShown = false;
		return;
	}

	%sw = getword(%dimensions, 0);
	%sh = getword(%dimensions, 1);
	KronosShop::computeLayout(%sw, %sh);

	$KS::btnN = 0;
	$KS::hovNow = "";   // re-stashed by renderPane when an item row is hovered

	// reset drag rects (info box is a menu-only thing; stock pane only
	// exists in a merchant shop) so dragHit doesn't match stale panels
	$Panel::infoShown = false;
	$Panel::plW = 0;
	$Panel::sbStShown = false;
	$KS::findW[st] = 0;   // re-stashed by renderPane if the stock pane draws

	if($KS::open == "bank")
	{
		KronosShop::renderPane("inv", $KSL::lx, $KSL::ly, "Your Items   " @ $KS::cGold @ "Coins: " @ $KS::cGreen @ "$" @ KronosShop::commafy($KS::coins));
		KronosShop::renderPane("st", $KSL::rx, $KSL::ry, "Bank Storage   " @ $KS::cGold @ "Bank: " @ $KS::cGreen @ "$" @ KronosShop::commafy($KS::bank));
	}
	else
	{
		KronosShop::renderPane("inv", $KSL::lx, $KSL::ly, "Your Items   " @ $KS::cGold @ "Gold: " @ $KS::cGreen @ "$" @ KronosShop::commafy($KH::gold));
		if($KS::open == "shop")
		{
			%title = $KS::shopName;
			if(%title == "" || %title == -1)
				%title = "Merchant";
			KronosShop::renderPane("st", $KSL::rx, $KSL::ry, %title);
		}
	}

	// ---- item tooltip: request after a short hover, draw when the text is in ----
	// (suppressed while the amount modal is up or a drag is active)
	if(KronosInput::isFocused("bankqty") || $Drag::active)
		$KS::hovNow = "";
	if(String::Compare($KS::hovNow, $KS::hovId) != 0)
	{
		$KS::hovId = $KS::hovNow;
		$KS::hovT = GetSimTime();
	}
	if($KS::hovId != "" && kronos::simAge($KS::hovT) > 0.35
		&& String::Compare($KS::tipFor, $KS::hovId) != 0
		&& String::Compare($KS::tipReq, $KS::hovId) != 0)
	{
		// ask the server for this item's examine text, once per hovered item
		$KS::tipReq = $KS::hovId;
		remoteEval(2048, KShopTip, $KS::hovKind, $KS::hovRef);
	}
	if($KS::hovId != "" && $KS::tipRows > 0 && String::Compare($KS::tipFor, $KS::hovId) == 0)
		KronosShop::renderTip(%sw, %sh);

	// amount-entry modal for custom deposit/withdraw of items & coins
	if(KronosInput::isFocused("bankqty"))
		KronosShop::renderBankAmt(%sw, %sh);
}

// ============================================
// Item tooltip (server: remoteKShopTip in KronosHUD_Server.cs)
// ============================================
// The reply is the WhatIs examine text: \n-separated rows with <jc>/<fN> color
// tags. Rows are split + colorized ONCE here (fixed alpha - tooltips don't
// fade); glGetStringDimensions skips inline <RRGGBBAA> tags, so the stored
// colored rows measure correctly at render time.
// Chunked delivery: one remoteEval string arg is capped at 255 bytes on the
// wire, so the server splits long examine texts into KShopTipPart pieces and
// we reassemble, then hand the whole thing to the parser below.
function remoteKShopTipBegin(%server)
{
	if(%server != 2048)
		return;
	$KS::tipBuf = "";
}

function remoteKShopTipPart(%server, %part)
{
	if(%server != 2048)
		return;
	$KS::tipBuf = $KS::tipBuf @ %part;
}

function remoteKShopTipDone(%server)
{
	if(%server != 2048)
		return;
	remoteKShopTipText(2048, $KS::tipBuf);
	$KS::tipBuf = "";
}

function remoteKShopTipText(%server, %text)
{
	if(%server != 2048)
		return;

	%nl = "\n";
	%nlLen = String::len(%nl);
	%cur = "e6e6f0";   // default body color; <fN> tags carry across rows
	%rows = 0;
	%more = true;
	%rest = %text;
	while(%more && %rows < 32)
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
		%raw = String::replace(%chunk, "<jc>", "");

		// the LAST color tag in a row becomes the next row's starting color
		%next = %cur;
		%best = -1;
		%q = String::findSubStr(%raw, "<f0>");
		if(%q > %best) { %best = %q; %next = "aab4c8"; }
		%q = String::findSubStr(%raw, "<f1>");
		if(%q > %best) { %best = %q; %next = "ffffff"; }
		%q = String::findSubStr(%raw, "<f2>");
		if(%q > %best) { %best = %q; %next = "ffd278"; }
		%q = String::findSubStr(%raw, "<f3>");
		if(%q > %best) { %best = %q; %next = "ff9682"; }

		%d = String::replace(%raw, "<f0>", "<aab4c8e6>");
		%d = String::replace(%d, "<f1>", "<ffffffe6>");
		%d = String::replace(%d, "<f2>", "<ffd278e6>");
		%d = String::replace(%d, "<f3>", "<ff9682e6>");
		$KS::tipRow[%rows] = "<" @ %cur @ "e6>" @ %d;
		%cur = %next;
		%rows++;
	}
	// drop trailing blank rows
	while(%rows > 0 && String::len($KS::tipRow[%rows - 1]) <= 10)
		%rows--;

	$KS::tipRows = %rows;
	$KS::tipFor = $KS::tipReq;   // the hover this reply answers
	$KS::tipMeasuredFont = -1;   // re-measure at the current font
}

// Draw the tooltip near the cursor, clamped to the screen.
function KronosShop::renderTip(%sw, %sh)
{
	%font = floor($KSL::rowH * 0.56);
	if(%font < 9)
		%font = 9;
	%lineH = %font + floor(%font * 0.35);
	%pad = floor(%font * 0.6);

	// wrap + measure the rows (cached until the font or text changes): long
	// description rows are word-wrapped to the tooltip's max width instead of
	// overflowing past the box. Each stored row starts with a 10-char
	// <rrggbbaa> color tag; continuation lines re-prefix it so the color holds.
	if($KS::tipMeasuredFont != %font)
	{
		glSetFont("Verdana", %font, $GLEX_SMOOTH, 0);
		%wrapW = floor(%sw * 0.34) - (%pad * 2);
		%mw = 0;
		%n = 0;
		for(%i = 0; %i < $KS::tipRows && %n < 60; %i++)
		{
			%row = $KS::tipRow[%i];
			%t = getword(glGetStringDimensions(%row), 0);
			if(%t <= %wrapW)
			{
				$KS::tipWL[%n] = %row;
				if(%t > %mw)
					%mw = %t;
				%n++;
				continue;
			}
			%clr = String::getSubStr(%row, 0, 10);
			%body = String::getSubStr(%row, 10, 99999);
			%line = "";
			for(%wi = 0; (%word = getWord(%body, %wi)) != -1 && %n < 60; %wi++)
			{
				if(%line == "")
					%try = %word;
				else
					%try = %line @ " " @ %word;
				if(getword(glGetStringDimensions(%clr @ %try), 0) > %wrapW && %line != "")
				{
					$KS::tipWL[%n] = %clr @ %line;
					%t = getword(glGetStringDimensions(%clr @ %line), 0);
					if(%t > %mw)
						%mw = %t;
					%n++;
					%line = %word;
				}
				else
					%line = %try;
			}
			if(%line != "" && %n < 60)
			{
				$KS::tipWL[%n] = %clr @ %line;
				%t = getword(glGetStringDimensions(%clr @ %line), 0);
				if(%t > %mw)
					%mw = %t;
				%n++;
			}
		}
		$KS::tipWN = %n;
		$KS::tipTextW = %mw;
		$KS::tipMeasuredFont = %font;
	}

	%w = $KS::tipTextW + (%pad * 2);
	%wMax = floor(%sw * 0.34);
	if(%w > %wMax)
		%w = %wMax;
	%h = ($KS::tipWN * %lineH) + (%pad * 2);

	%x = $KM::mouseX + 18;
	%y = $KM::mouseY + 14;
	if(%x + %w > %sw - 4)
		%x = $KM::mouseX - %w - 10;   // flip to the left of the cursor
	if(%x < 2)
		%x = 2;
	if(%y + %h > %sh - 4)
		%y = %sh - 4 - %h;
	if(%y < 2)
		%y = 2;

	glDisable($GL_TEXTURE_2D);
	glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);
	glColor4ub($KT::bgR, $KT::bgG, $KT::bgB, 244);
	glRectangle(%x, %y, %w, %h);
	glColor4ub($KT::acR, $KT::acG, $KT::acB, 220);
	glRectangle(%x, %y, %w, 1);
	glColor4ub($KT::acR, $KT::acG, $KT::acB, 100);
	glRectangle(%x, %y + %h - 1, %w, 1);
	glRectangle(%x, %y, 1, %h);
	glRectangle(%x + %w - 1, %y, 1, %h);

	glSetFont("Verdana", %font, $GLEX_SMOOTH, 0);
	%ty = %y + %pad;
	for(%i = 0; %i < $KS::tipWN; %i++)
	{
		glDrawString(%x + %pad, %ty, $KS::tipWL[%i]);
		%ty += %lineH;
	}
}

// Centered modal showing the digits-only amount field. Driven entirely by the
// keyboard (Enter = confirm via KronosShop::bankAmtSubmit, Esc = cancel); clicks
// are swallowed by handleClick while it is up. $KS::amtTitle / $KS::amtMax are set
// by KronosShop::beginBankAmt.
function KronosShop::renderBankAmt(%sw, %sh)
{
	%font = floor(%sh * 0.024);
	if(%font < 12)
		%font = 12;
	%pad = floor(%font * 0.6);
	%w = floor(%sw * 0.30);
	if(%w < 280)
		%w = 280;
	%lineH = %font + floor(%pad * 0.5);
	%fh = %font + %pad;                       // input box height
	// top pad + title + gap + field + gap + max line + controls line + bottom pad
	%h = (%pad * 2) + (%lineH * 3) + %fh + %pad;
	%x = floor((%sw - %w) / 2);
	%y = floor(%sh * 0.38);
	%tx = %x + %pad;
	%avail = %w - (%pad * 2);

	glDisable($GL_TEXTURE_2D);
	glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);

	// backdrop + border
	glColor4ub(15, 20, 34, 242);
	glRectangle(%x, %y, %w, %h);
	glColor4ub($KT::acR, $KT::acG, $KT::acB, 235);
	glRectangle(%x, %y, %w, 2);
	glColor4ub($KT::acR, $KT::acG, $KT::acB, 110);
	glRectangle(%x, %y + %h - 1, %w, 1);
	glRectangle(%x, %y, 1, %h);
	glRectangle(%x + %w - 1, %y, 1, %h);

	%cy = %y + %pad;

	// title (what we're depositing/withdrawing) - shrink to fit
	glColor4ub(235, 240, 255, 245);
	KronosShop::fitText($KS::amtTitle, floor(%font * 0.92), %avail, 8, 0);
	glDrawString(%tx, %cy, $KS::amtTitle);
	%cy += %lineH + floor(%pad * 0.5);

	// input box + the digits (KronosInput::drawText scrolls long values itself)
	glColor4ub($KT::bgR, $KT::bgG, $KT::bgB, 235);
	glRectangle(%tx, %cy, %avail, %fh);
	glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, 150);
	glRectangle(%tx, %cy, %avail, 1);
	glRectangle(%tx, %cy + %fh - 1, %avail, 1);
	KronosInput::drawText(%tx + 5, %cy + floor(%pad * 0.4), %avail - 10, %font);
	%cy += %fh + floor(%pad * 0.5);

	// max on its OWN line (so a big balance can't collide with the controls) - shrink
	// to fit. Coin ops show a green $amount with commas; item ops show a plain count.
	if($KS::amtOp == "depcoin" || $KS::amtOp == "wdcoin")
		%maxStr = "Max: " @ $KS::cGreen @ "$" @ KronosShop::commafy($KS::amtMax);
	else
		%maxStr = "Max: " @ KronosShop::commafy($KS::amtMax);
	glColor4ub(205, 212, 228, 225);
	KronosShop::fitText(%maxStr, floor(%font * 0.72), %avail, 7, 0);
	glDrawString(%tx, %cy, %maxStr);
	%cy += %lineH;

	// controls
	glColor4ub(150, 160, 185, 195);
	KronosShop::fitText("Enter = OK     Esc = cancel", floor(%font * 0.70), %avail, 7, 0);
	glDrawString(%tx, %cy, "Enter = OK     Esc = cancel");
}

function KronosShop::renderPane(%p, %px, %py, %title)
{
	$KS::curPane = %p;   // buttons drawn below belong to THIS pane (see KronosShop::button)
	%pad = $KSL::pad;
	%w = $KSL::w;
	%rowH = $KSL::rowH;
	%titleH = $KSL::titleH;
	%y = %py;
	%rowY0 = %py + %titleH + %rowH + floor(%pad / 2);   // title, then search row, then items
	%fontTitle = floor(%titleH * 0.62);
	%fontItem = floor(%rowH * 0.62);

	// live search: while the pane's find box is focused, mirror its text into the
	// filter; rebuild the display list whenever the effective filter changed
	// (String::Compare - a filter like "10" must not numeric-compare)
	if(%p == "inv")
		%fid = "ksinv";
	else
		%fid = "ksst";
	if(KronosInput::isFocused(%fid))
		$KS::filter[%p] = KronosInput::text();
	if(String::Compare($KS::filter[%p], $KS::builtFilter[%p]) != 0)
		KronosShop::buildDisp(%p);

	// stash this pane's rect for drag hit-testing (title bar = top titleH).
	// inv pane shares the menu panel's stored position, stock pane the
	// players panel's - so KronosMenu::dragHit/dragMove move both screens.
	if(%p == "inv")
	{
		$Panel::menuX = %px;  $Panel::menuY = %py;  $Panel::menuW = %w;  $Panel::menuTH = %titleH;
	}
	else
	{
		$Panel::plX = %px;    $Panel::plY = %py;    $Panel::plW = %w;    $Panel::plTH = %titleH;
	}

	// visible window into the (possibly longer) display list
	%total = $KS::dispN[%p];
	%visible = %total;
	if(%visible > $KS::MaxRows)
		%visible = $KS::MaxRows;
	if(%visible < 1)
		%visible = 1;
	%maxScroll = %total - %visible;
	if(%maxScroll < 0)
		%maxScroll = 0;
	if($KS::scroll[%p] > %maxScroll)
		$KS::scroll[%p] = %maxScroll;
	if($KS::scroll[%p] < 0)
		$KS::scroll[%p] = 0;
	%first = $KS::scroll[%p];
	$KS::visible[%p] = %visible;   // for handleClick

	%scrW = floor(%rowH * 0.45);
	if(%scrW < 6)
		%scrW = 6;
	%hasScroll = (%total > %visible);

	// pane height: title + search row + item rows + one button row + pad
	%ph = %titleH + %rowH + (%rowH * (%visible + 1)) + %pad;

	// hovered slot (exclude the scrollbar gutter)
	%hovSlot = -1;
	if($KM::mouseOn && $KM::mouseY >= %rowY0 && $KM::mouseY < %rowY0 + (%visible * %rowH)
		&& $KM::mouseX >= %px + %pad && $KM::mouseX < %px + %w - %pad - %scrW)
		%hovSlot = floor(($KM::mouseY - %rowY0) / %rowH);

	// hovered ITEM identity for the tooltip (render() compares it across
	// frames and requests the examine text after a short hover delay)
	if(%hovSlot != -1)
	{
		%hd = %first + %hovSlot;
		if(%hd < %total && $KS::dispType[%p, %hd] == "item")
		{
			%hr = $KS::dispRef[%p, %hd];
			if(%p == "inv")
			{
				%hk = $KS::invKind[%hr];
				%hrf = $KS::invRef[%hr];
			}
			else
			{
				%hk = $KS::stKind[%hr];
				%hrf = $KS::stRef[%hr];
			}
			$KS::hovNow = %p @ "|" @ %hk @ "|" @ %hrf;
			$KS::hovKind = %hk;
			$KS::hovRef = %hrf;
		}
	}

	// ---- rectangles ----
	glDisable($GL_TEXTURE_2D);
	glBlendFunc($GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA);

	glColor4ub($KT::bgR, $KT::bgG, $KT::bgB, 238);
	glRectangle(%px, %y, %w, %ph);
	glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, 220);
	glRectangle(%px, %y, %w, 2);
	glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, 90);
	glRectangle(%px, %y + %ph - 1, %w, 1);
	glRectangle(%px, %y, 1, %ph);
	glRectangle(%px + %w - 1, %y, 1, %ph);
	glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, 140);
	glRectangle(%px + %pad, %y + %titleH - 2, %w - (%pad * 2), 1);

	// search (filter) row box - between the title and the item rows
	%foc = KronosInput::isFocused(%fid);
	%fx = %px + %pad;
	%fy = %y + %titleH + 1;
	%fbH = %rowH - 3;
	%fw = %w - (%pad * 2);
	if(%foc)
		glColor4ub($KT::b2R, $KT::b2G, $KT::b2B, 215);
	else
		glColor4ub($KT::bgR, $KT::bgG, $KT::bgB, 170);
	glRectangle(%fx, %fy, %fw, %fbH);
	if(%foc)
	{
		glColor4ub($KT::acR, $KT::acG, $KT::acB, 220);
		glRectangle(%fx, %fy, %fw, 1);
		glRectangle(%fx, %fy + %fbH - 1, %fw, 1);
	}
	else
	{
		glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, 70);
		glRectangle(%fx, %fy, %fw, 1);
		glRectangle(%fx, %fy + %fbH - 1, %fw, 1);
	}
	// clear chip ("x") at the right end while a filter is active
	%fcW = 0;
	if($KS::filter[%p] != "")
	{
		%fcW = %fbH;
		glColor4ub(150, 70, 70, 200);
		glRectangle(%fx + %fw - %fcW, %fy, %fcW, %fbH);
	}
	// stash rects for handleClick
	$KS::findX[%p] = %fx;   $KS::findY[%p] = %fy;
	$KS::findW[%p] = %fw;   $KS::findH[%p] = %fbH;
	$KS::findCW[%p] = %fcW;

	// row tints
	%iy = %rowY0;
	for(%s = 0; %s < %visible; %s++)
	{
		%d = %first + %s;
		if(%d >= %total)
			break;
		if($KS::dispType[%p, %d] == "item")
		{
			%r = $KS::dispRef[%p, %d];
			if(%r == $KS::sel[%p] && $KS::sel[%p] != -1)
			{
				glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, 80);
				glRectangle(%px + 2, %iy, %w - 4, %rowH);
			}
			else if(%s == %hovSlot)
			{
				glColor4ub($KT::hvR, $KT::hvG, $KT::hvB, 45);
				glRectangle(%px + 2, %iy, %w - 4, %rowH);
			}
		}
		else
		{
			glColor4ub($KT::chR, $KT::chG, $KT::chB, 60);
			glRectangle(%px + 2, %iy, %w - 4, %rowH);
		}
		%iy += %rowH;
	}

	// button row (slot %visible)
	%btnY = %rowY0 + (%visible * %rowH);
	if(%p == "inv")
	{
		if($KS::open == "bank")
		{
			// flow left-to-right by measured width so the longer labels never overlap
			%bx = %px + %pad;
			%bx += KronosShop::button(%bx, %btnY, "Deposit", "deposit", %rowH, %fontItem) + %pad;
			%bx += KronosShop::button(%bx, %btnY, "Deposit " @ $KS::cGold @ "Coins", "depcoins", %rowH, %fontItem) + %pad;
			KronosShop::button(%bx, %btnY, "Close", "close", %rowH, %fontItem);
		}
		else
		{
			if($KS::open == "shop")
				KronosShop::button(%px + %pad, %btnY, "Sell", "sell", %rowH, %fontItem);
			KronosShop::button(%px + %pad + floor(%w * 0.24), %btnY, "Use", "use", %rowH, %fontItem);
			KronosShop::button(%px + %pad + floor(%w * 0.48), %btnY, "Drop", "drop", %rowH, %fontItem);
			KronosShop::button(%px + %pad + floor(%w * 0.72), %btnY, "Close", "close", %rowH, %fontItem);
		}
	}
	else
	{
		if($KS::open == "bank")
		{
			%bx = %px + %pad;
			%bx += KronosShop::button(%bx, %btnY, "Withdraw", "withdraw", %rowH, %fontItem) + %pad;
			KronosShop::button(%bx, %btnY, "Withdraw " @ $KS::cGold @ "Coins", "wdcoins", %rowH, %fontItem);
		}
		else
			KronosShop::button(%px + %pad, %btnY, "Buy", "buy", %rowH, %fontItem);
	}

	// scrollbar (right gutter over the row area) - high transparency
	if(%hasScroll)
	{
		%trackX = %px + %w - %scrW - 2;
		%trackY = %rowY0;
		%trackH = %visible * %rowH;

		if(%p == "inv")
		{
			$Panel::sbInvX = %trackX;  $Panel::sbInvY = %trackY;
			$Panel::sbInvW = %scrW;    $Panel::sbInvH = %trackH;
			$Panel::sbInvVis = %visible;  $Panel::sbInvTot = %total;
			$Panel::sbInvShown = true;
			%dragging = ($Drag::active && $Drag::id == "sbinv");
		}
		else
		{
			$Panel::sbStX = %trackX;  $Panel::sbStY = %trackY;
			$Panel::sbStW = %scrW;    $Panel::sbStH = %trackH;
			$Panel::sbStVis = %visible;  $Panel::sbStTot = %total;
			$Panel::sbStShown = true;
			%dragging = ($Drag::active && $Drag::id == "sbst");
		}

		glColor4ub($KT::dmR, $KT::dmG, $KT::dmB, 30);
		glRectangle(%trackX, %trackY, %scrW, %trackH);

		%thumbH = floor(%trackH * %visible / %total);
		if(%thumbH < %rowH)
			%thumbH = %rowH;
		%travel = %trackH - %thumbH;
		if(%travel < 1)
			%travel = 1;
		%frac = 0;
		if(%maxScroll > 0)
			%frac = %first / %maxScroll;     // 0 = top of list, 1 = bottom
		%thumbY = %trackY + floor(%frac * %travel);
		%thumbA = 75;
		if(%dragging)
			%thumbA = 155;
		glColor4ub($KT::txR, $KT::txG, $KT::txB, %thumbA);
		glRectangle(%trackX + 1, %thumbY, %scrW - 2, %thumbH);
	}
	else if(%p == "inv")
		$Panel::sbInvShown = false;
	// (sbStShown was reset in render() before the panes)

	// ---- text ----
	// shrink the title font if needed so a large coin/gold balance can't overflow
	// the pane's right edge (bank headers carry "(Coins: $123456789)")
	glColor4ub(235, 240, 255, 245);
	KronosShop::fitText(%title, %fontTitle, %w - (%pad * 2), 8, 1);
	glDrawString(%px + %pad, %y + floor(%titleH * 0.16), %title);

	// search row text: the live field while focused, else the set filter (with
	// its clear "x"), else a grey placeholder
	%fFont = floor(%fbH * 0.62);
	if(%fFont < 8)
		%fFont = 8;
	%fty = %fy + floor((%fbH - %fFont) / 2);
	if(%foc)
		KronosInput::drawText(%fx + 4, %fty, %fw - %fcW - 8, %fFont);
	else
	{
		glSetFont("Verdana", %fFont, $GLEX_SMOOTH, 0);
		if($KS::filter[%p] != "")
		{
			glColor4ub(255, 235, 150, 240);
			glDrawString(%fx + 4, %fty, $KS::filter[%p]);
		}
		else
		{
			glColor4ub(140, 150, 175, 150);
			glDrawString(%fx + 4, %fty, "Find...");
		}
	}
	if(%fcW > 0)
	{
		glSetFont("Verdana", %fFont, $GLEX_SMOOTH, 0);
		glColor4ub(255, 230, 230, 240);
		glDrawString(%fx + %fw - %fcW + floor(%fcW * 0.3), %fty, "x");
	}

	glSetFont("Verdana", %fontItem, $GLEX_SMOOTH, 0);
	%numX = %px + floor(%w * 0.80);
	%iy = %rowY0;
	for(%s = 0; %s < %visible; %s++)
	{
		%d = %first + %s;
		if(%d >= %total)
			break;
		%ty = %iy + floor((%rowH - %fontItem) / 2) - 1;
		if($KS::dispType[%p, %d] == "head")
		{
			glColor4ub($KT::txR, $KT::txG, $KT::txB, 240);
			glDrawString(%px + %pad, %ty, $KS::dispRef[%p, %d]);
		}
		else
		{
			%r = $KS::dispRef[%p, %d];
			glColor4ub(225, 230, 240, 230);
			if(%p == "inv")
			{
				glDrawString(%px + %pad + floor(%pad * 0.8), %ty, $KS::invName[%r]);
				glColor4ub(255, 255, 255, 210);
				glDrawString(%numX, %ty, $KS::invCnt[%r]);
			}
			else
			{
				glDrawString(%px + %pad + floor(%pad * 0.8), %ty, $KS::stName[%r]);
				if($KS::open == "bank")
				{
					// bank mode: stPrice carries the STORED COUNT, not a price
					glColor4ub(255, 255, 255, 210);
					glDrawString(%px + floor(%w * 0.80), %ty, $KS::stPrice[%r]);
				}
				else
				{
					glColor4ub(255, 210, 75, 220);
					glDrawString(%px + floor(%w * 0.70), %ty, "$" @ $KS::stPrice[%r]);
				}
			}
		}
		%iy += %rowH;
	}

	// button labels (rects were drawn by KronosShop::button)
	for(%i = 0; %i < $KS::btnN; %i++)
	{
		if($KS::btnPane[%i] != %p)
			continue;
		glColor4ub(235, 240, 255, 240);
		glDrawString($KS::btnX[%i] + floor(%pad * 0.6),
			$KS::btnY[%i] + floor(($KS::btnH[%i] - %fontItem) / 2) - 1, $KS::btnLabel[%i]);
	}
}

// Draws the button chip and records its rect for click hit-testing
function KronosShop::button(%x, %y, %label, %act, %rowH, %font)
{
	glSetFont("Verdana", %font, $GLEX_SMOOTH, 0);
	%tw = getword(glGetStringDimensions(%label), 0);
	%bw = %tw + floor($KSL::pad * 1.2);
	%bh = %rowH - 4;

	glColor4ub($KT::chR, $KT::chG, $KT::chB, 160);
	glRectangle(%x, %y + 2, %bw, %bh);

	%i = $KS::btnN;
	$KS::btnX[%i] = %x;
	$KS::btnY[%i] = %y + 2;
	$KS::btnW[%i] = %bw;
	$KS::btnH[%i] = %bh;
	$KS::btnLabel[%i] = %label;
	$KS::btnAct[%i] = %act;
	// pane ownership = the pane being rendered (set in renderPane). The old
	// geometric guess (%x < $KSL::rx) misfiled inventory-mode buttons that sit
	// past the right-pane x as "st"; that pane never renders in inventory mode,
	// so their labels were skipped (rect drawn, text missing).
	$KS::btnPane[%i] = $KS::curPane;
	$KS::btnN++;
	return %bw;   // so callers can flow buttons left-to-right by measured width
}

// Group an integer with thousands separators: 1234567 -> "1,234,567". Avoids the
// %-modulo operator (which collides with TorqueScript's %local prefix) by counting
// digits in groups of three from the right.
function KronosShop::commafy(%n)
{
	%s = %n;
	%len = String::len(%s);
	if(%len <= 3)
		return %s;
	%out = "";
	%grp = 0;
	for(%i = %len - 1; %i >= 0; %i--)
	{
		if(%grp == 3)
		{
			%out = "," @ %out;
			%grp = 0;
		}
		%out = String::getSubStr(%s, %i, 1) @ %out;
		%grp++;
	}
	return %out;
}

// Set "Verdana" at the largest size (down to %minFont) whose rendering of %text
// fits within %avail px, then leave that font selected for the caller's draw.
// Used to keep long titles / big coin balances from overflowing their box.
function KronosShop::fitText(%text, %font, %avail, %minFont, %glow)
{
	glSetFont("Verdana", %font, $GLEX_SMOOTH, %glow);
	%tw = getword(glGetStringDimensions(%text), 0);
	%guard = 0;
	while(%tw > %avail && %font > %minFont && %guard < 40)
	{
		%font--;
		glSetFont("Verdana", %font, $GLEX_SMOOTH, %glow);
		%tw = getword(glGetStringDimensions(%text), 0);
		%guard++;
	}
	return %font;
}

// ============================================
// Mouse input (dispatched from KronosMenu.cs onMouseLMB)
// ============================================

function KronosShop::handleClick(%x, %y)
{
	// amount modal up: it's keyboard-driven (Enter/Esc) - swallow stray clicks so
	// they don't select a row behind it
	if(KronosInput::isFocused("bankqty"))
		return;

	// search (find) boxes: the clear "x" first, then the box itself to focus it.
	// A click anywhere else while a find box is focused just blurs it (the typed
	// filter stays applied) and the click falls through to rows/buttons as usual.
	for(%fi = 0; %fi < 2; %fi++)
	{
		if(%fi == 0)
			%p = "inv";
		else
			%p = "st";
		if($KS::findW[%p] < 1)
			continue;
		if(%x >= $KS::findX[%p] && %x < $KS::findX[%p] + $KS::findW[%p]
			&& %y >= $KS::findY[%p] && %y < $KS::findY[%p] + $KS::findH[%p])
		{
			if($KS::findCW[%p] > 0 && %x >= $KS::findX[%p] + $KS::findW[%p] - $KS::findCW[%p])
			{
				// clear chip: drop the filter (and the field, if focused)
				$KS::filter[%p] = "";
				if(KronosInput::isFocused("ksinv") || KronosInput::isFocused("ksst"))
					KronosInput::blur();
				return;
			}
			KronosShop::beginFind(%p);
			return;
		}
	}
	if(KronosInput::isFocused("ksinv") || KronosInput::isFocused("ksst"))
		KronosInput::blur();

	// buttons first (their rects overlap the row grid)
	for(%i = 0; %i < $KS::btnN; %i++)
	{
		if(%x >= $KS::btnX[%i] && %x < $KS::btnX[%i] + $KS::btnW[%i]
			&& %y >= $KS::btnY[%i] && %y < $KS::btnY[%i] + $KS::btnH[%i])
		{
			KronosShop::doAction($KS::btnAct[%i]);
			return;
		}
	}

	// item rows - pick the pane by x, then use that pane's row origin
	if($KSL::rowH < 1)
		return;

	%p = "";
	%rowY0 = 0;
	if(%x >= $KSL::lx + $KSL::pad && %x < $KSL::lx + $KSL::w - $KSL::pad)
	{
		%p = "inv";
		%rowY0 = $KSL::invRowY0;
	}
	else if(($KS::open == "shop" || $KS::open == "bank") && %x >= $KSL::rx + $KSL::pad && %x < $KSL::rx + $KSL::w - $KSL::pad)
	{
		%p = "st";
		%rowY0 = $KSL::stRowY0;
	}
	if(%p == "")
		return;
	if(%y < %rowY0)
		return;
	%slot = floor((%y - %rowY0) / $KSL::rowH);
	if(%slot >= $KS::visible[%p])
		return;

	%d = $KS::scroll[%p] + %slot;
	if(%d >= $KS::dispN[%p])
		return;
	if($KS::dispType[%p, %d] != "item")
		return;

	%r = $KS::dispRef[%p, %d];
	$KS::sel[%p] = %r;
	if(%p == "inv")
		$KS::selItem[%p] = $KS::invKind[%r] @ "|" @ $KS::invRef[%r];
	else
		$KS::selItem[%p] = $KS::stKind[%r] @ "|" @ $KS::stRef[%r];
}

function KronosShop::doAction(%act)
{
	%verb = getword(%act, 0);

	if(%verb == "close")
	{
		remoteEval(2048, KShopClose);
		return;
	}

	// Timegate the item actions (0.3s) - rapid-fire drops spawn
	// interpenetrating thrown items that can clip through the floor
	// and vanish, and the server gates at 0.25s anyway, so pace the
	// clicks instead of wasting them. GetSimTime() RESETS on reconnect /
	// mission reload, so a negative elapsed means the clock restarted -
	// treat it as "gate open", else every action stays blocked until the
	// new clock catches up to the old timestamp (same trap the KronosHUD
	// handshake throttle guards against).
	%now = GetSimTime();
	if($KS::lastAct != "" && %now >= $KS::lastAct && (%now - $KS::lastAct) < 0.3)
		return;
	$KS::lastAct = %now;

	// ---- bank actions: open the amount-entry modal (pre-filled with the max,
	//      so Enter = "all of it", edit = custom). KronosShop::bankAmtSubmit
	//      sends the chosen amount to the server. ----
	if(%verb == "depcoins")
	{
		KronosShop::beginBankAmt("depcoin", "", $KS::coins, "Deposit " @ $KS::cGold @ "Coins");
		return;
	}
	if(%verb == "wdcoins")
	{
		KronosShop::beginBankAmt("wdcoin", "", $KS::bank, "Withdraw " @ $KS::cGold @ "Coins");
		return;
	}
	if(%verb == "withdraw")
	{
		%r = $KS::sel[st];
		if(%r == -1 || %r == "")
			return;
		// banked belt items (kind "b") withdraw through the belt-aware handler
		if($KS::stKind[%r] == "b")
		{
			KronosShop::beginBankAmt("wdbelt", $KS::stRef[%r], $KS::stPrice[%r], "Withdraw: " @ $KS::stName[%r]);
			return;
		}
		KronosShop::beginBankAmt("wditem", $KS::stRef[%r], $KS::stPrice[%r], "Withdraw: " @ $KS::stName[%r]);
		return;
	}
	if(%verb == "deposit")
	{
		%r = $KS::sel[inv];
		if(%r == -1 || %r == "")
			return;
		// belt/backpack items (kind "b") go through the belt-aware bank handler
		// (the server resolves the registered item + category); ItemData equipment
		// uses the stock KBankDeposit path.
		if($KS::invKind[%r] == "b")
		{
			KronosShop::beginBankAmt("depbelt", $KS::invRef[%r], $KS::invCnt[%r], "Deposit: " @ $KS::invName[%r]);
			return;
		}
		KronosShop::beginBankAmt("depitem", $KS::invRef[%r], $KS::invCnt[%r], "Deposit: " @ $KS::invName[%r]);
		return;
	}

	// item actions - ItemData rows send exactly the messages the stock
	// gui buttons sent; belt rows use the KShopBelt* server handlers
	if(%verb == "buy")
	{
		%r = $KS::sel[st];
		if(%r == -1 || %r == "")
			return;
		if($KS::stKind[%r] == "b")
			remoteEval(2048, KShopBeltBuy, $KS::stRef[%r]);
		else
			remoteEval(2048, buyItem, $KS::stRef[%r]);
	}
	else
	{
		%r = $KS::sel[inv];
		if(%r == -1 || %r == "")
			return;
		if($KS::invKind[%r] == "b")
		{
			if(%verb == "sell")
				remoteEval(2048, KShopBeltSell, $KS::invRef[%r]);
			else if(%verb == "use")
				remoteEval(2048, KShopBeltUse, $KS::invRef[%r]);
			else if(%verb == "drop")
				remoteEval(2048, KShopBeltDrop, $KS::invRef[%r]);
		}
		else
		{
			if(%verb == "sell")
				remoteEval(2048, sellItem, $KS::invRef[%r]);
			else if(%verb == "use")
				remoteEval(2048, useItem, $KS::invRef[%r]);
			else if(%verb == "drop")
				remoteEval(2048, dropItem, $KS::invRef[%r]);
		}
	}

	// counts changed - ask the server for fresh rows
	schedule("remoteEval(2048, KShopSync);", 0.5);
}

// ============================================
// Search (find) boxes - one per pane, filters the item list as you type
// ============================================
function KronosShop::beginFind(%p)
{
	if(%p == "inv")
		%fid = "ksinv";
	else
		%fid = "ksst";
	$KS::findPane = %p;
	// Enter keeps the filter (just blurs); Esc clears it
	KronosInput::focus(%fid, $KS::filter[%p], "KronosShop::findSubmit", "KronosShop::findCancel", "", 24);
}

function KronosShop::findSubmit()
{
	KronosInput::blur();
}

function KronosShop::findCancel()
{
	$KS::filter[$KS::findPane] = "";
}

// ============================================
// Mouse wheel (glPollWheel seam in the native DLL)
// ============================================
// Drained EVERY frame from the onPostDraw chain. The DLL reports whole wheel
// notches since the last poll ("+n" = wheel up, "-n" = down, "" = none); we
// route them to whatever the GUI cursor is over: a shop/bank pane scrolls its
// item list, the chat window scrolls its history. Ticks are DISCARDED when the
// cursor is down, so wheel spins during gameplay (weapon switch) can't dump a
// stale scroll into the next menu. Without the DLL seam glPollWheel is an
// unknown command (returns "") and this is a no-op.
function KronosShop::pumpWheel()
{
	%t = glPollWheel();
	if(%t == "" || %t == 0)
		return;
	if(!$KM::mouseOn || !$KM::enabled)
		return;
	%x = $KM::mouseX;
	%y = $KM::mouseY;

	// shop/bank panes: scroll the hovered pane (wheel up = toward the top of
	// the list, 2 rows per notch; render() clamps to the list length)
	if($KS::open != "" && $KS::open != false && $KSL::rowH >= 1)
	{
		%p = "";
		if(%x >= $KSL::lx && %x < $KSL::lx + $KSL::w
			&& %y >= $KSL::ly && %y < $KSL::invRowY0 + (($KS::visible[inv] + 1) * $KSL::rowH))
			%p = "inv";
		else if(($KS::open == "shop" || $KS::open == "bank")
			&& %x >= $KSL::rx && %x < $KSL::rx + $KSL::w
			&& %y >= $KSL::ry && %y < $KSL::stRowY0 + (($KS::visible[st] + 1) * $KSL::rowH))
			%p = "st";
		if(%p != "")
		{
			$KS::scroll[%p] = $KS::scroll[%p] - (%t * 2);
			if($KS::scroll[%p] < 0)
				$KS::scroll[%p] = 0;
			return;
		}
	}

	// chat window: wheel up = back into history (3 lines per notch; the chat
	// render clamps to the history length and re-anchors on new lines)
	if($Panel::kchatShown
		&& %x >= $Panel::kchatX && %x < $Panel::kchatX + $Panel::kchatW
		&& %y >= $Panel::kchatY && %y < $Panel::kchatY + $Panel::kchatH)
	{
		$KC::scroll = $KC::scroll + (%t * 3);
		if($KC::scroll < 0)
			$KC::scroll = 0;
	}
}

// ============================================
// Mouse wheel weapon-switch gate
// ============================================
// The physical wheel drives TWO independent input paths: the DirectInput
// zaxis binding (nextWeapon/prevWeapon, bound in autoexec.cs/config.cs) AND
// the Win32 WM_MOUSEWHEEL -> glPollWheel seam that pumpWheel drains above.
// While the HUD cursor is up we want the wheel to scroll the shop/bank/chat
// pane WITHOUT also cycling weapons, so we re-bind the zaxis to this gated
// wrapper: it's a no-op when the GUI cursor is active, and a plain weapon
// switch otherwise. This rebind runs AFTER autoexec.cs's binds (KronosShop
// is exec'd later in autoexec), so it wins.
function Kronos::wheelWeapon(%dir)
{
	if($KM::mouseOn && $KM::enabled)
		return;
	if(%dir > 0)
		nextWeapon();
	else
		prevWeapon();
}

// Make sure we edit the same map autoexec bound to, then override the wheel.
editActionMap("playMap.sae");
bindCommand(mouse0, zaxis0, TO, "Kronos::wheelWeapon(1);");  // wheel forward
bindCommand(mouse0, zaxis1, TO, "Kronos::wheelWeapon(-1);"); // wheel backward

// ============================================
// Right-click: quick bank transfer (full stack, no amount modal)
// ============================================
// The DLL's glMouseRMB reports the hardware button state; we edge-detect a
// press each frame. In BANK mode, right-clicking a row moves the item's WHOLE
// stack immediately - deposit from the left pane, withdraw from the right -
// using the same server messages as the modal's "Enter = all" path (which
// clamps server-side too). Reversible by right-clicking it back, so a misclick
// costs nothing. Without the DLL seam glMouseRMB is unknown -> "" -> no-op.

// Which item row is under (x,y)? Returns "pane rowindex" or "".
function KronosShop::rowAt(%x, %y)
{
	if($KSL::rowH < 1)
		return "";
	%p = "";
	%rowY0 = 0;
	if(%x >= $KSL::lx + $KSL::pad && %x < $KSL::lx + $KSL::w - $KSL::pad)
	{
		%p = "inv";
		%rowY0 = $KSL::invRowY0;
	}
	else if(($KS::open == "shop" || $KS::open == "bank") && %x >= $KSL::rx + $KSL::pad && %x < $KSL::rx + $KSL::w - $KSL::pad)
	{
		%p = "st";
		%rowY0 = $KSL::stRowY0;
	}
	if(%p == "" || %y < %rowY0)
		return "";
	%slot = floor((%y - %rowY0) / $KSL::rowH);
	if(%slot >= $KS::visible[%p])
		return "";
	%d = $KS::scroll[%p] + %slot;
	if(%d >= $KS::dispN[%p])
		return "";
	if($KS::dispType[%p, %d] != "item")
		return "";
	return %p @ " " @ $KS::dispRef[%p, %d];
}

function KronosShop::pumpRMB()
{
	%dn = 0;
	if(glMouseRMB() != "")
		%dn = 1;
	if(%dn == $KS::rmbWas)
		return;
	$KS::rmbWas = %dn;
	if(%dn != 1)
		return;                     // act on the press edge only
	if(!$KM::mouseOn || !$KM::enabled)
		return;
	if($KS::open != "bank")         // quick transfer is a bank-only shortcut
		return;
	if(KronosInput::isFocused("bankqty"))
		return;

	%hit = KronosShop::rowAt($KM::mouseX, $KM::mouseY);
	if(%hit == "")
		return;
	%p = getword(%hit, 0);
	%r = getword(%hit, 1);

	// same anti-spam pacing as the buttons (and the same clock-reset guard)
	%now = GetSimTime();
	if($KS::lastAct != "" && %now >= $KS::lastAct && (%now - $KS::lastAct) < 0.3)
		return;
	$KS::lastAct = %now;

	if(%p == "inv")
	{
		// deposit the whole carried stack
		if($KS::invKind[%r] == "b")
			remoteEval(2048, KBankBeltDeposit, $KS::invRef[%r], $KS::invCnt[%r]);
		else
			remoteEval(2048, KBankDeposit, $KS::invRef[%r], $KS::invCnt[%r]);
	}
	else
	{
		// withdraw the whole stored stack (stPrice = stored count in bank mode)
		if($KS::stKind[%r] == "b")
			remoteEval(2048, KBankBeltWithdraw, $KS::stRef[%r], $KS::stPrice[%r]);
		else
			remoteEval(2048, KBankWithdraw, $KS::stRef[%r], $KS::stPrice[%r]);
	}
	schedule("remoteEval(2048, KShopSync);", 0.5);
}

// ============================================
// Bank amount entry (custom deposit/withdraw counts via KronosInput)
// ============================================
// %op: depitem / wditem / depcoin / wdcoin. %ref: ItemData ref (items only).
// %max: the available amount (carried count / stored count / carried or banked
// coins) - used to pre-fill the field (so Enter = "all") and clamp on submit.
function KronosShop::beginBankAmt(%op, %ref, %max, %title)
{
	$KS::amtOp = %op;
	$KS::amtRef = %ref;
	$KS::amtMax = %max;
	$KS::amtTitle = %title;

	%init = %max;
	if(%init == "" || %init < 1)
		%init = "";                   // nothing available -> empty field
	// digits-only field; Enter -> bankAmtSubmit, Esc -> cancel (no-op fn)
	KronosInput::focus("bankqty", %init, "KronosShop::bankAmtSubmit", "", "", 9);
	KronosInput::setNumeric(true);
}

// Enter in the amount field: send the chosen (clamped) amount to the server. The
// server clamps too and treats a missing/0 amount as "all/1", so this is safe even
// against a server that hasn't been updated (it just ignores the extra arg).
function KronosShop::bankAmtSubmit()
{
	%amt = KronosInput::text();
	%op  = $KS::amtOp;
	%ref = $KS::amtRef;
	%max = $KS::amtMax;
	KronosInput::blur();
	$KS::amtOp = "";

	if(%amt == "" || %amt < 1)
		return;                       // nothing entered -> cancel
	if(%max > 0 && %amt > %max)
		%amt = %max;                  // clamp to what's available

	if(%op == "depitem")
		remoteEval(2048, KBankDeposit, %ref, %amt);
	else if(%op == "wditem")
		remoteEval(2048, KBankWithdraw, %ref, %amt);
	else if(%op == "depbelt")
		remoteEval(2048, KBankBeltDeposit, %ref, %amt);
	else if(%op == "wdbelt")
		remoteEval(2048, KBankBeltWithdraw, %ref, %amt);
	else if(%op == "depcoin")
		remoteEval(2048, KBankCoinsDeposit, %amt);
	else if(%op == "wdcoin")
		remoteEval(2048, KBankCoinsWithdraw, %amt);

	schedule("remoteEval(2048, KShopSync);", 0.5);
}

// ============================================
// Draw hook - replaces KronosMenu.cs's definition (this file loads
// last); calls the menu render, the examine overlay, then the shop
// ============================================

function ScriptGL::playGui::onPostDraw(%dimensions)
{
	%dim = KronosMenu::screenDim(%dimensions);

	KronosCM::pump();      // drain keys for the V quick-command menu (no-op unless open)
	KronosNPC::pump();     // Esc/Tab close for the NPC dialogue (no-op unless open)
	KronosShop::pumpWheel(); // mouse wheel -> shop/bank pane or chat scroll (no-op without cursor)
	KronosShop::pumpRMB();   // right-click -> quick full-stack bank transfer (no-op outside bank)

	KronosMenu::render(%dim);

	if(kronos::simAge($KH::exTime) < 10.0)
		kronos::examine_render(getword(%dim, 0), getword(%dim, 1));

	KronosShop::render(%dim);

	KronosChat::render(getword(%dim, 0), getword(%dim, 1));
	KronosNPC::render(getword(%dim, 0), getword(%dim, 1));
	KronosMenu::renderSlider(getword(%dim, 0), getword(%dim, 1));
	KronosMenu::renderChatGrip(getword(%dim, 0), getword(%dim, 1));
	KronosStats::render(getword(%dim, 0), getword(%dim, 1));  // session XP/gold rates
	KronosCM::render(getword(%dim, 0), getword(%dim, 1));   // V quick menu, on top
}

// ============================================
// Initialize
// ============================================

$KS::open = "";
$KS::curPane = "";
$KS::lastAct = "";
$KS::invN = 0;
$KS::stN = 0;
$KS::btnN = 0;
$KS::dispN[inv] = 0;
$KS::dispN[st] = 0;
$KS::sel[inv] = -1;
$KS::sel[st] = -1;
$KS::scroll[inv] = 0;
$KS::scroll[st] = 0;
$KS::visible[inv] = 0;
$KS::visible[st] = 0;
$KS::coins = 0;
$KS::bank = 0;
$KS::amtOp = "";
$KS::amtRef = "";
$KS::amtMax = 0;
$KS::amtTitle = "";
// search boxes
$KS::filter[inv] = "";
$KS::filter[st] = "";
$KS::builtFilter[inv] = "";
$KS::builtFilter[st] = "";
$KS::findW[inv] = 0;
$KS::findW[st] = 0;
$KS::findPane = "inv";
// hover tooltip
$KS::hovNow = "";
$KS::hovId = "";
$KS::hovT = 0;
$KS::hovKind = "";
$KS::hovRef = "";
$KS::tipFor = "";
$KS::tipReq = "";
$KS::tipRows = 0;
$KS::tipTextW = 0;
$KS::tipMeasuredFont = -1;
// right-click edge detector
$KS::rmbWas = 0;
// inline glDrawString color tags (the DLL parses <RRGGBBAA>; dimensions skip them)
$KS::cGold  = "<ffd24bff>";   // gold  - currency labels ("Coins:" / "Bank:" / "Gold:")
$KS::cGreen = "<64e678ff>";   // green - the $ amount
$Panel::sbInvShown = false;
$Panel::sbStShown = false;

echo("KronosShop: modern shop/inventory screen loaded");
