// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// Match.CS									Presto, March '99 
//
//	String pattern matching.
//
//	Most client-side mods involve attaching to the client message stream
//	and decoding information from there.  This set of functions was
//	designed to help people do that.
//
//	Usage examples:
//
//		%matched = Match::String(%message, "Presto joined the game.");
//			This will check to see if the %message string exactly 
//			matches the quoted string.  I don't know why you'd want
//			to do this when you have == :)
//			The function returns true if it matches.
//		%matched = Match::String(%message, "* joined the game.");
//			This will check to see if %message ends with " joined the
//			game."  The wildcard character * indicates that it will
//			match any text.  If the function returns true, you can
//			call:
//		%matchResult = Match::Result(0);
//			This gets the text from a previous Match::String call
//			which filled the wildcard.
//
//	More complicated usage examples:
//
//		if (Match::String(%message, "* joined the * team."))
//			Try a match against a string with two wildcards
//		{
//		%player = Match::Result(0);
//			Get the first wildcard.
//		%team = Match::Result(1);
//			Get the second wildcard.
//		}
//
//	How about something where you actually want *'s in the pattern?
//	Instead use
//		Match::String(%message, "*** Important: ^ ***", "^")
//	the end string indicates that you want the wildcard to be "^"
//	instead of "*".  Note that the wildcard can be any string so
//		Match::String(%message, "Presto says &&&.", "&&&")
//	is also legal.  Hey, you could even use "e" for all anyone cares.
//
//	>>>	Important!  The string must match exactly, end-to-end, the 
//	>>>	pattern string you pass in.  This is not a substring search
//	>>>	unless you write your pattern like "*substring*"
//	>>>	Also, "*" will match an empty string so "The*End" matches
//	>>>	"TheEnd" (Match::Result(0) would be "")
//
//	Then, there is another function.  This one is similar but more useful
//	for making the search string easier to understand.
//
//		%matched = Match::ParamString(%message, "%p joined the game.");
//
//	Match::ParamString is a paramterized wildcard search.  Instead of
//	returning the results in Match::Result(0) (1) (2) ... , it returns
//	in the letter following the wildcard.  The default wildcard is "%"
//	in parameterized strings rather than *, but again you can change it
//	if need be.
//
//	You would call Match::Result("p") to return the result of the above
//	search.  Remember parameters can only be *ONE CHARACTER*
//
//	One nice thing about the paramterized wildcard search is that if
//	you re-use a parameter in the search string, it must match the 
//	same text both times!  This means that 
//
//		%matched = Match::ParamString("word/word", "%p/%p");
//
//	will return 1 match, but
//
//		%matched = Match::ParamString("word&werd", "%p&%p");
//
//	will return 0.
//
//	Finally, there is one last feature which may be useful someday.
//	In some cases your search pattern is not enough to guarantee that
//	you have only one match.  For instance, the call
//
//		%matched = Match::String(%message, "*and*");
//	will have two possible results when %message = "random sandwich".
//	In this case, you can use the two-argument form of Match::Result
//	where the first argument is the match # and the second argument
//	is the wildcard number.
//		%matched = 2
//		Match::Result(0,0) = "r"
//		Match::Result(0,1) = "om sandwich"
//		Match::Result(1,0) = "random s"
//		Match::Result(1,1) = "wich"
//	Also, in case you don't save the return result, you can use the function
//		%matched = Match::Count();
//	which would again return 2.
//	This also works for parameterized searches where you can for instance
//	ask for Match::Result(0, "b");
//
//	In particular this feature is included so that when you write a 
//	script, it is possible to detect someone trying to fool that script.
//	If you are checking "* took the * flag!" someone could come in
//	named "[ took the ]" and really mess up the script.  The detection
//	of multiple matches does not magically fix the problem, but at
//	least you can detect it.
//
//	The solution is to always make your checking scripts as explicit
//	as possible!  I think it will be unecessary to use the more advanced
//	string matching features for now.  But I want the players out there
//	to know that if we scripters see people using weird names to break
//	scripts, it is possible for us to fix them...
//
//	Unfortunately it may never be possible to catch all of these, as
//	the text part of some patterns is necessarily so short that it can't
//	always tell.  For instance, if you're checking for "Repairing *.", 
//	a player named Repairing can screw with your script when he
//	gets killed by a turret ("Repairing dies.").  If the repair message 
//	"Repairing item #1." cannot possibly be told apart from the death 
//	message "Repairing dies.", there is no way to fix it. :(
//	
// ---------------------------------------------------------------------------

function Match::strLen(%str) {
	for (%i = 0; String::GetSubStr(%str, %i, 1) != ""; %i++)
		{}
	return %i;
	}

// Internal function, don't call this directly!
function Match::AddResult() {
	%num = $Match::params[$Match::results];
	$Match::params[$Match::results + 1] = %num;
	for (%i = 0; %i < %num; %i++) {
		$Match::param[$Match::results + 1, %i] = $Match::param[$Match::results, %i];
		}
	$Match::results++;
	}

function Match::Clear() {
	// Clear old results.
	%params = $Match::params[$Match::results];

	%i = 0;
	%param = String::GetSubStr(%params, %i,1);
	%first = %param;
	while (%param != "") {
		$Match::param[$Match::results, %param] = "";

		%i++;
		%param = String::GetSubStr(%params, %i,1);
		if (%param == %first)
			%param = %i;
		}

	$Match::params[$Match::results] = "";
	}
function Match::Success(%params) {
	// Set up for cleanup
	$Match::params[$Match::results] = %params;
	$Match::results++;
	Match::Clear($Match::results);

	%i = 0;
	%param = String::GetSubStr(%params, %i,1);
	%first = %param;
	while (%param != "") {
		$Match::param[$Match::results, %param] = $Match::param[$Match::results - 1, %param];

		%i++;
		%param = String::GetSubStr(%params, %i,1);
		if (%param == %first)
			%param = %i;
		}
	}
function Match::Add(%param, %str, %exists) {
	if (%exists) {
		if ($Match::param[$Match::results, %param] != %str)
			return false;
		}
	else	$Match::param[$Match::results, %param] = %str;
	return true;
	}
function Match::Remove(%param) {
	$Match::param[$Match::results, %param] = "";
	}

// Internal function, don't call this directly!
function Match::_ParamString(%str, %pat, %wc,%lenWc, %params, %numeric) {
	// Look for a wildcard in the pattern
	%lenText = String::FindSubStr(%pat, %wc);
	if (%lenText == -1) {
		// Match entire string
		if (%str == %pat)
			Match::Success(%params);
		return;
		}

	if (%lenText != 0) {
		// Match pattern text up to %lenText
		if (String::GetSubStr(%pat, 0, %lenText) != String::GetSubStr(%str, 0, %lenText))
			return; // no match.

		%str = String::GetSubStr(%str, %lenText, 10000);
		}

	if (%numeric != "") {
		%param = %numeric;
		%exists = false;
		%numeric++;
		%params = %params @ "0";	// trick ... 0000 = 4 numeric params.
		}
	else	{
		%param = String::GetSubStr(%pat, %lenText+%lenWC - 1, 1);
		if (%param == "")	// just in case they forgot.
			%param = 0;
		%exists = String::FindSubStr(%params, %param) != -1;
		if (!%exists)
			%params = %params @ %param;
		}

	// Chop off the wildcard
	%pat = String::GetSubStr(%pat, %lenText+%lenWc, 10000);
	if (%pat == "") {
		// No more pattern left after the wildcard, wildcard matches to end
		if (Match::Add(%param, %str, %exists)) {
			Match::Success(%params);
			Match::Remove(%param);
			}
		return;
		}

	// Look for the next wildcard in the pattern
	%lenText = String::FindSubStr(%pat, %wc);
	if (%lenText == 0)
		return;	// It's illegal to have two wildcards in a row.
	if (%lenText == -1) {
		// Special case:  match "*string".
		%idx = String::FindSubStr(%str@%wc, %pat@%wc);	// @%wc guarantees last match.
		if (%idx != -1 && String::GetSubStr(%str, %idx, 10000)==%pat) {
			if (Match::Add(%param, String::GetSubStr(%str, 0, %idx), %exists)) {
				Match::Success(%params);
				Match::Remove(%param);
				}
			return;
			}
		return;
		}

	// Chop off the text from the pattern
	%textPat = String::GetSubStr(%pat, 0, %lenText);
	%pat = String::GetSubStr(%pat, %lenText, 10000);

	%idx = String::FindSubStr(%str, %textPat);
	while (%idx != -1) {
		// Got one wildcard match

		// Make sure this is a valid match
		if (Match::Add(%param, String::GetSubStr(%str, 0, %idx), %exists)) {
			// Try a match given this wildcard match
			Match::_ParamString(String::GetSubStr(%str, %idx + %lenText, 10000),
						  %pat, %wc,%lenWc, %params, %numeric);
			Match::Remove(%param);
			}

		%newIdx = String::FindSubStr(String::GetSubStr(%str, %idx+1, 10000), %textPat);
		if (%newIdx == -1)
			%idx = -1;
		else	%idx = %newIdx + %idx + 1;
		}
	return;
	}

function Match::ParamString(%string, %pattern, %wildcard) {
	while ($Match::Results >= 0) {
		Match::Clear();
		$Match::Results--;
		}
	$Match::results = 0;

	if (%wildcard == "") {
		%wildcard = "%";
		%lenWC = 2;	// includes the next letter
		}
	else	%lenWC = Match::strLen(%wildcard) + 1;	// includes the next letter
	Match::_ParamString(%string, %pattern, %wildcard,%lenWC, "");
	return $Match::results;
	}
function Match::String(%string, %pattern, %wildcard) {
	while ($Match::Results >= 0) {
		Match::Clear();
		$Match::Results--;
		}
	$Match::results = 0;

	if (%wildcard == "") {
		%wildcard = "*";
		%lenWC = 1;
		}
	else	%lenWC = Match::strLen(%wildcard);
	Match::_ParamString(%string, %pattern, %wildcard,%lenWC,"",0);
	return $Match::results;
	}

function Match::Count(%num) {
	return $Match::results;
	}
function Match::Result(%num, %numWild) {
	if (%numWild == "") {
		%numWild = %num;
		%num = 0;
		}
	return $Match::param[%num, %numwild];
	}
