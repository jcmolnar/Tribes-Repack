/*
 QuickVote 0.3 for 1.40+
 Featuring Smokeys VoteSounds
 Pops up a quick key vote menu instead of having to use the tab menu
 -|DF|- Link
 
 Press Y for yes or N for no on your keyboard
 Press 'Esc' to ignore the vote
 Go to your Options > Binds to set a key for QuickVote & sounds toggle (enable or disable)
 
 ** NOTE: QuickVote & Sounds enabled by default **
 
 Change the sound variables below to whatever sound file you would like (wav or ogg) 
*/

$QuickVote::voteYesSound = "C_BuySell";
$QuickVote::voteNoSound = "crash";
$QuickVote::voteInitiated = "vote_initiated.wav";
$QuickVote::votePassed = "vote_passes.wav";
$QuickVote::voteFailed = "vote_failed.wav";


// BEGIN. DO NOT TOUCH BELOW!
$QuickVote::Enabled = true;
$QuickVote::playSounds = true;
$QuickVote::isVoting = false;


function QuickVote::GameBinds::Init() after GameBinds::Init
{
	$GameBinds::CurrentMapHandle = GameBinds::GetActionMap2( "actionMap.sae");
	$GameBinds::CurrentMap = "actionMap.sae";
	GameBinds::addBindCommand( "Toggle QuickVote", "QuickVote::Toggle();");
	GameBinds::addBindCommand( "Toggle QuickVote Sounds", "QuickVote::ToggleSound();");
}

function QuickVote::Toggle()
{
	$QuickVote::Enabled = !$QuickVote::Enabled;
	remoteCP( 2048, "<JC><F1>QuickVote is now " @ ($QuickVote::Enabled ? "enabled" : "disabled") @ ".", 3 );	
}

function QuickVote::ToggleSound()
{
	$QuickVote::playSounds = !$QuickVote::playSounds;
	remoteCP( 2048, "<JC><F1>QuickVote sounds are now " @ ($QuickVote::playSounds ? "enabled" : "disabled") @ ".", 3 );	
}

function QuickVote::afterremoteBP( %manager, %msg, %timeout, %type ) after remoteBP
{
	if( (String::FindSubStr( %msg, "<f0>initiated a vote to <f1>") != -1 ) && $QuickVote::Enabled) {
		remoteBP( 2048, "" );
		return;
	}
}

function QuickVote::beforeremoteBP( %manager, %msg, %timeout, %type ) before remoteBP 
{	
	if(!$QuickVote::Enabled)
		return;
	
	%escape_msg = String::EscapeFormat(%msg);
	
	if( (String::FindSubStr( %escape_msg, "initiated a vote to " ) != -1 ) && !$QuickVote::isVoting)
	{
		if($QuickVote::playSounds)
			localSound( $QuickVote::voteInitiated );

		$QuickVote::isVoting = true;
				
		// Just don't ask...
		%index = String::getSubStr(%escape_msg, String::findSubStr(%escape_msg, "initiated a vote to "), 1000);
		%person = String::Trim( String::Replace(%escape_msg, %index, "") );		
		%str = String::Trim( String::Replace( String::Replace(%escape_msg, "initiated a vote to ", ""), %person, "") );
		
		// Turn off the score screen
		remoteEval( 2048, scoresOff );
	
		// Create & show the output
		QuickVote::Output( %person, %str );
		
		// Initialize the custom keybinds
		QuickVote::CreateBinds();
		
		// Check if remoteEP is visible repeatedly
		if($QuickVote::isVoting)
			QuickVote::remoteVisible();
	}
}

function QuickVote::Output( %person, %str )
{
	%header = String::AlignTextLeft("<f1>Voter: <f2>" @ %person @ "<f1>");
	%body = String::AlignTextLeft("Has initiated a vote to<f2> " @ %str @ "<f1>.");
	%end = String::Dup( "-", 35 );
	
	// Create the output
	%output = sprintf(
			"\n<JC><f2>QuickVote\n\n" @
			"<JL>%1\n\n" @
			"<JL>%2\n\n" @
			"<JC><f2>%3\n\n" @
			"<f1>Press <f2>[ Y ] <f1>or<f2> [ N ] <f1>now\n\n" @
			"<f1>You may also hit <f2>[ ESC ]<f1> to ignore voting.", %header, %body, %end
		);
	
	// Display the remoteEP
	remoteEP( %output, 18, true, 15, 20, 600 );
	
	// Move this remoteEP into the center of the screen
	%rEP = Control::getId("remoteEP");
	
	%FullScreenRes0 = getWord(playGui.extent, 0);
	%FullScreenRes1 = getWord(playGui.extent, 1);
	%rEP.w = getWord(%rEP.extent, 0);
	%rEP.h = getWord(%rEP.extent, 1);
	%rEP.x = floor( %FullScreenRes0 / 2 ) - ( %rEP.w / 2 );
	%rEP.y = floor( %FullScreenRes1 / 2 ) - ( %rEP.h / 2 );
	%rEP.position = %rEP.x @ " " @ %rEP.y;
}

function QuickVote::CreateBinds()
{  
	NewActionMap("QuickVoteBinds.sae");
	
	// Create binds for voting
	bindCommand(keyboard0, make, "y", TO, "QuickVote::vote(Yes);");
	bindCommand(keyboard0, break, "y", TO, "QuickVote::break();");
	
	bindCommand(keyboard0, make, "n", TO, "QuickVote::vote(No);");
	bindCommand(keyboard0, break, "n", TO, "QuickVote::break();");
	
	bindCommand(keyboard0, make, "escape", TO, "QuickVote::break();");
	bindCommand(keyboard0, break, "escape", TO, "");
	
	PushActionMap("QuickVoteBinds.sae");
}

function QuickVote::vote( %opt )
{
	if($QuickVote::playSounds)
		localSound( (%opt == "Yes" ? $QuickVote::voteYesSound : $QuickVote::voteNoSound) );
	
	if(%opt == "Yes")
		voteYes();
	else
		voteNo();
}

function QuickVote::break()
{
	Control::SetVisible("remoteEP", false);
	PopActionMap("QuickVoteBinds.sae");
}

function QuickVote::Display()
{
	remoteCP(2048, $QuickVote::output, 2.5);
	$QuickVote::output = "";
}

function QuickVote::GetMsg( %opt ) after QuickVote::vote
{
	%opt = String::toupper(%opt);
	$QuickVote::output = sprintf("<JC><f2>\nQuickVote\n\n<f1>You've cast a <f2>%1<f1> vote. Thanks for voting!\n\n", %opt);
	
	// Why do you have to do it like this?!
	Schedule::Add("QuickVote::Display();", 0.5);
}

function QuickVote::remoteVisible()
{
	if(!Control::GetVisible("remoteEP") && $QuickVote::isVoting) {
		Schedule::Cancel("RemoteVisible");
		$QuickVote::isVoting = false;
		PopActionMap("QuickVoteBinds.sae");
	}
	
	if($QuickVote::isVoting)
		Schedule::Add("QuickVote::remoteVisible();", 0.8, "RemoteVisible");
}

// Borrowed from Smokey's VoteSounds. Thanks dude!
function votechat(%cl, %msg, %type) before onClientMessage {
	if (%type != 0)
		return;
	
	if(!$QuickVote::playSounds)
		return;

	if (String::FindSubStr(%msg, "Vote to ") != -1) {
		if (String::FindSubStr(%msg, " passed: ") != -1) {
			localSound( $QuickVote::votePassed );
		} else if (String::FindSubStr(%msg, " did not pass: ") != -1) {
			localSound( $QuickVote::voteFailed );
		}
	}
}

// Custom String function
// Removes <f0> like formatting from entire string
// Feel free to use or make it better!
function String::EscapeFormat(%str)
{
	%formats = "<f0> <f1> <f2> <f3> <JC> <JL> <JR>";
	
	for(%i = 0; getWord(%formats, %i) != -1; %i++)
		%str = String::Replace( %str, getWord(%formats, %i), "");
	
	return %str;
}

// Custom String function
// Aligns each paragraph's line to the left (easier to read)
// Feel free to use or make it better!
function String::AlignTextLeft(%text)
{
	%textWidth = 49;
	
	for(%i=0; String::getSubStr(%text,%i,1) != ""; %i++)
		if(%i % %textWidth == 0)
			%str = %str @ "\t\t\t" @ String::Trim( String::getSubStr(%text, %i, %textWidth) ) @ "\n";
	
	return %str;
}
