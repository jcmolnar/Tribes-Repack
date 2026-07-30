//==============================================
// KronosInput.cs - reusable ScriptGL text-input field
//==============================================
// ScriptGL is draw-only; it has no way to type text. The native build adds a
// keyboard seam (engine/SimGui/code/scriptGL.cpp + engine/Sim/code/simGame.cpp):
//   glTextInput(1)  -> the engine forwards every keyboard MAKE to script and
//                      SWALLOWS it, so action-map binds (move/weapon/...) don't
//                      fire while you type. glTextInput(0) stops capture.
//   ScriptGL::onChar(%ch)  -> a printable character was typed (the literal
//                             character, not its ascii code - TorqueScript can't
//                             convert a code back to a char, so the engine sends
//                             the char itself).
//   ScriptGL::onKey(%dik)  -> a non-printable key (Enter/Backspace/arrows/...),
//                             given as its DirectInput scancode ($DIK::*).
//
// This file owns the ONE focused field at a time: its text buffer + caret, the
// onChar/onKey handlers, and small draw/helper functions. Consumers (the chat
// composer, a bank-amount box, a search field) just:
//   - call KronosInput::focus(id, initial, submitFn, cancelFn, navFn, maxLen)
//     when their field is clicked / a hotkey is pressed,
//   - draw their own box and call KronosInput::drawText(...) for the focused one,
//   - implement submitFn (Enter), optional cancelFn (Esc), optional navFn (Up/Down).
//
// REQUIRES THE NATIVE BUILD with the glTextInput seam. On a client without it,
// glTextInput is an unknown command (a one-line echo) and nothing captures keys
// - the rest of the GUI is unaffected.
//==============================================

// DirectInput scancodes we care about (DIK_*), passed to ScriptGL::onKey.
$DIK::Escape      = 1;
$DIK::Back        = 14;   // backspace
$DIK::Tab         = 15;
$DIK::Return      = 28;
$DIK::NumpadEnter = 156;
$DIK::Up          = 200;
$DIK::Left        = 203;
$DIK::Right       = 205;
$DIK::End         = 207;
$DIK::Down        = 208;
$DIK::Home        = 199;
$DIK::Delete      = 211;

// ---- focus state (one field at a time) ----
$KIN::focus    = "";   // id of the focused field ("" = none)
$KIN::text     = "";   // current buffer
$KIN::caret    = 0;    // caret index (0..len)
$KIN::maxLen   = 120;
$KIN::submitFn = "";   // eval'd on Enter
$KIN::cancelFn = "";   // eval'd on Esc (after blur)
$KIN::navFn    = "";   // eval'd on Up/Down as navFn(dik)
$KIN::numeric  = false; // when true, onChar accepts digits only (amount fields)

// ============================================
// Focus / blur / query
// ============================================
function KronosInput::focus(%id, %initial, %submitFn, %cancelFn, %navFn, %maxLen)
{
	$KIN::focus    = %id;
	$KIN::text     = %initial;
	$KIN::caret    = String::len(%initial);
	$KIN::submitFn = %submitFn;
	$KIN::cancelFn = %cancelFn;
	$KIN::navFn    = %navFn;
	if(%maxLen == "" || %maxLen < 1)
		%maxLen = 120;
	$KIN::maxLen = %maxLen;
	$KIN::numeric = false;   // default text field; callers opt into digits-only
	glTextInput(1);   // engine: start forwarding keys to ScriptGL::onChar/onKey
}

// Restrict the focused field to digits (amount entry). Call right after focus().
function KronosInput::setNumeric(%b)
{
	$KIN::numeric = %b;
}

function KronosInput::blur()
{
	if($KIN::focus == "")
		return;
	$KIN::focus = "";
	glTextInput(0);   // engine: stop capture, binds resume
}

// String::Compare is case-sensitive (== numerically coerces letters), so use it
function KronosInput::isFocused(%id)
{
	if($KIN::focus == "")
		return false;
	return String::Compare($KIN::focus, %id) == 0;
}

function KronosInput::anyFocused()
{
	return $KIN::focus != "";
}

function KronosInput::text()
{
	return $KIN::text;
}

function KronosInput::setText(%t)
{
	$KIN::text  = %t;
	$KIN::caret = String::len(%t);
}

// ============================================
// Engine callbacks (the native key seam calls these)
// ============================================
function ScriptGL::onChar(%ch)
{
	if($KIN::focus == "")
		return;
	if(String::len($KIN::text) >= $KIN::maxLen)
		return;
	// digits-only field: reject any non-digit (findSubStr avoids the == numeric-
	// coercion pitfall - a non-digit char would otherwise coerce to 0 and pass)
	if($KIN::numeric && String::findSubStr("0123456789", %ch) == -1)
		return;
	%c = $KIN::caret;
	%left  = String::getSubStr($KIN::text, 0, %c);
	%right = String::getSubStr($KIN::text, %c, 99999);
	$KIN::text  = %left @ %ch @ %right;
	$KIN::caret = %c + 1;
}

function ScriptGL::onKey(%dik)
{
	if($KIN::focus == "")
		return;

	if(%dik == $DIK::Back)
	{
		if($KIN::caret > 0)
		{
			%left  = String::getSubStr($KIN::text, 0, $KIN::caret - 1);
			%right = String::getSubStr($KIN::text, $KIN::caret, 99999);
			$KIN::text  = %left @ %right;
			$KIN::caret = $KIN::caret - 1;
		}
		return;
	}
	if(%dik == $DIK::Delete)
	{
		%left  = String::getSubStr($KIN::text, 0, $KIN::caret);
		%right = String::getSubStr($KIN::text, $KIN::caret + 1, 99999);
		$KIN::text = %left @ %right;
		return;
	}
	if(%dik == $DIK::Left)
	{
		if($KIN::caret > 0)
			$KIN::caret = $KIN::caret - 1;
		return;
	}
	if(%dik == $DIK::Right)
	{
		if($KIN::caret < String::len($KIN::text))
			$KIN::caret = $KIN::caret + 1;
		return;
	}
	if(%dik == $DIK::Home)
	{
		$KIN::caret = 0;
		return;
	}
	if(%dik == $DIK::End)
	{
		$KIN::caret = String::len($KIN::text);
		return;
	}
	if(%dik == $DIK::Return || %dik == $DIK::NumpadEnter)
	{
		// submit BEFORE the consumer can re-focus; capture the fn first
		%fn = $KIN::submitFn;
		if(%fn != "")
			eval(%fn @ "();");
		return;
	}
	if(%dik == $DIK::Escape)
	{
		%fn = $KIN::cancelFn;
		KronosInput::blur();
		if(%fn != "")
			eval(%fn @ "();");
		return;
	}
	if(%dik == $DIK::Up || %dik == $DIK::Down)
	{
		%fn = $KIN::navFn;
		if(%fn != "")
			eval(%fn @ "(" @ %dik @ ");");
		return;
	}
}

// ============================================
// Pump: drain the native key queue and apply each key. Call EVERY FRAME while a
// field is focused (KronosChat::render does). The native plugin (kronos_textinput.dll)
// captures keystrokes into a C queue from inside the input hook - it does NOT call
// back into the engine there (that crashed). We drain it here, in safe script
// context, via glTextPoll(): "c<char>" = a literal typed character, "k<dik>" = a
// special key by scancode. Uses String::len / String::Compare (NOT ==, which
// numerically coerces single chars - "" and "c" both coerce to 0).
function KronosInput::pump()
{
	if($KIN::focus == "")
		return;
	%guard = 0;
	while(%guard < 200)
	{
		%guard++;
		%ev = glTextPoll();
		if(String::len(%ev) < 1)
			return;                       // queue empty
		%kind = String::getSubStr(%ev, 0, 1);
		%val  = String::getSubStr(%ev, 1, 99999);
		if(String::Compare(%kind, "c") == 0)
			ScriptGL::onChar(%val);       // literal typed character
		else
			ScriptGL::onKey(%val);        // special key, %val = DIK scancode
	}
}

// ============================================
// Draw the focused field's text + caret inside a %w-wide area.
// Call ONLY for the field that is currently focused (KronosInput::isFocused).
// Scrolls from the left so the end of what you're typing stays visible.
// ============================================
function KronosInput::drawText(%x, %y, %w, %fontH)
{
	glSetFont("Verdana", %fontH, $GLEX_SMOOTH, 0);

	%vis = $KIN::text;
	%guard = 0;
	while(%guard < 400 && String::len(%vis) > 1
		&& getWord(glGetStringDimensions(%vis), 0) > %w)
	{
		%vis = String::getSubStr(%vis, 1, 99999);
		%guard++;
	}

	glColor4ub(238, 242, 255, 248);
	glDrawString(%x, %y, %vis);

	// caret at the end of the visible text (solid block; no blink)
	%cw = getWord(glGetStringDimensions(%vis), 0);
	glDisable($GL_TEXTURE_2D);
	glColor4ub(255, 235, 150, 235);
	glRectangle(%x + %cw + 1, %y, 2, %fontH);
}

echo("KronosInput: ScriptGL text-input field loaded (needs native glTextInput seam)");
