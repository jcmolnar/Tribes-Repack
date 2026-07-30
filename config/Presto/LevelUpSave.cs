// ---------------------------------------------------------------------------
// LevelUpSave.cs
// Auto-save character on level up
//
// This script hooks into Presto's event system to detect when a player
// levels up and automatically triggers a character save via the #savecharacter
// command. It also handles AutoRemort functionality.
// ---------------------------------------------------------------------------

include("presto\\Event.cs");
// Try to include Events.cs (case may vary depending on system)
if(isFile("presto\\Events.cs"))
	include("presto\\Events.cs"); // Ensure Events.cs is loaded for eventServerMessage
else if(isFile("presto\\events.cs"))
	include("presto\\events.cs"); // Fallback to lowercase filename

// Client-side String::toLower function (if not already defined)
// This prevents "Unknown command" errors when String::toLower is called from client-side scripts
if(!isFunction("String::toLower"))
{
	function String::toLower(%string)
	{
		// Convert string to lowercase
		%len = String::len(%string);
		%result = "";
		
		// Character mapping for A-Z to a-z
		%upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		%lower = "abcdefghijklmnopqrstuvwxyz";
		
		for(%i = 0; %i < %len; %i++)
		{
			%char = String::getSubStr(%string, %i, 1);
			%pos = String::findSubStr(%upper, %char);
			if(%pos != -1)
				%char = String::getSubStr(%lower, %pos, 1);
			%result = %result @ %char;
		}
		
		return %result;
	}
	
	// Console command wrapper for String::toLower
	function toLower(%string)
	{
		return String::toLower(%string);
	}
}

// Initialize AutoRemort preference if not already set
if($PrestoPref::AutoRemort == "")
	$PrestoPref::AutoRemort = false;
// Initialize RemortStep if not already set
if($PrestoPref::RemortStep == "")
	$PrestoPref::RemortStep = 0;
// Cache last known level (from server messages) to allow re-checks
if($PrestoPref::LastKnownLevel == "")
	$PrestoPref::LastKnownLevel = 0;

// --- Helpers --------------------------------------------------------------

function PrestoLevelUpSave::ParseFirstNumber(%text)
{
	// Returns the first numeric word in %text, or "" if none
	for(%i = 0; (%w = String::getWord(%text, %i)) != -1; %i++)
	{
		%num = %w + 0;
		// Accept if the cast keeps it identical (basic numeric check)
		if(%num != "" && %num == %w + 0)
			return %num;
	}
	return "";
}

function PrestoLevelUpSave::MaybeUpdateRemortStep(%msg)
{
	// Try to pick up remort step from any server message that contains "Remort"
	if(String::findSubStr(String::toLower(%msg), "remort") == -1)
		return;

	%num = PrestoLevelUpSave::ParseFirstNumber(%msg);
	if(%num != "")
	{
		$PrestoPref::RemortStep = %num;
		echo("Presto: RemortStep updated (generic parse) to " @ $PrestoPref::RemortStep @ " from msg: " @ %msg);
	}
}

function PrestoLevelUpSave::RequiredRemortLevel()
{
	// Use 101 as first remort, then +4 per remort step (matches menu text)
	%step = $PrestoPref::RemortStep;
	if(%step == "" || %step < 0)
		%step = 0;
	return 101 + (%step * 4);
}

function PrestoLevelUpSave::CheckAutoRemort(%currentLevel)
{
	if(!$PrestoPref::AutoRemort)
		return;

	if(%currentLevel == "" || %currentLevel < 1)
		return;

	$PrestoPref::LastKnownLevel = %currentLevel;

	%remortLevel = PrestoLevelUpSave::RequiredRemortLevel();

	echo("Presto: AutoRemort check - Current level: " @ %currentLevel @ ", Required remort level: " @ %remortLevel @ " (RemortStep: " @ $PrestoPref::RemortStep @ ")");

	if(%currentLevel >= %remortLevel)
	{
		echo("Presto: AutoRemort triggered! Current level " @ %currentLevel @ " >= required " @ %remortLevel);
		say(0, "#cast remort");
		Client::centerPrint("<jc><f0>AutoRemort: <f1>Attempting to remort...", 1);
		Schedule("Client::centerPrint(\"\", 1);", 3);
	}
}

function PrestoLevelUpSave::RequestSelfInfo()
{
	// Issue a one-time #getinfo for the local player to seed level/remort caches
	if($PrestoPref::RequestedSelfInfo)
		return;

	%myId = getManagerId();
	%myName = Client::getName(%myId);
	if(%myName == "" || %myName == -1)
		return;

	$PrestoPref::RequestedSelfInfo = true;
	echo("Presto: AutoRemort requesting #getinfo for " @ %myName);
	say(0, "#getinfo " @ %myName);
}

// Function to detect level-up messages and trigger save/remort
// Using eventClientMessage like DeusRPGPack (when %client is 0, it's a server message)
function PrestoLevelUpSave::OnClientMessage(%client, %msg) {
	// Debug: Check for server messages (level-up detection)
	if(!%client && String::findSubStr(String::toLower(%msg), "level") != -1) {
		echo("PrestoLevelUpSave: Server message detected: [" @ %msg @ "]");
	}

	// Parse compact server hint from #getinfo (added for Presto)
	// Format: [PRESTOINFO] lvl=<lvl> remort=<remortStep>
	if(String::findSubStr(%msg, "[PRESTOINFO]") != -1)
	{
		%lvlPos = String::findSubStr(%msg, "lvl=");
		%remortPos = String::findSubStr(%msg, "remort=");
		if(%lvlPos != -1)
		{
			%lvlStr = String::getSubStr(%msg, %lvlPos + 4, String::len(%msg));
			%lvl = PrestoLevelUpSave::ParseFirstNumber(%lvlStr);
			if(%lvl != "")
				$PrestoPref::LastKnownLevel = %lvl + 0;
		}
		if(%remortPos != -1)
		{
			%remortStr = String::getSubStr(%msg, %remortPos + 7, String::len(%msg));
			%rem = PrestoLevelUpSave::ParseFirstNumber(%remortStr);
			if(%rem != "")
				$PrestoPref::RemortStep = %rem + 0;
		}
		// Run remort check if we have level
		if($PrestoPref::LastKnownLevel != "" && $PrestoPref::LastKnownLevel > 0)
			PrestoLevelUpSave::CheckAutoRemort($PrestoPref::LastKnownLevel);
		return true;
	}
	
	// If %client exists, it's from another player (ignore)
	if(%client)
		return true;
	
	// Server message (level-up detection / remort tracking)
	// Opportunistically parse remort step from any message containing "Remort"
	PrestoLevelUpSave::MaybeUpdateRemortStep(%msg);
	
	// Check if this is a remort message to store RemortStep
	// Remort message: "Welcome to Remort Level X! Your stats have all increased!!"
	if(String::findSubStr(%msg, "Welcome to Remort Level") != -1) {
		// Parse RemortStep from the message
		// Format: "Welcome to Remort Level X! Your stats have all increased!!"
		%remortPos = String::findSubStr(%msg, "Welcome to Remort Level");
		if(%remortPos != -1) {
			%afterRemort = String::getSubStr(%msg, %remortPos + String::len("Welcome to Remort Level"), String::len(%msg));
			%remortStep = String::getWord(%afterRemort, " ", 0); // Use space as separator
			// Remove any trailing punctuation
			if(String::len(%remortStep) > 0 && String::getSubStr(%remortStep, String::len(%remortStep) - 1, 1) == "!")
				%remortStep = String::getSubStr(%remortStep, 0, String::len(%remortStep) - 1);
			
			if(%remortStep != "") {
				$PrestoPref::RemortStep = %remortStep;
				echo("Presto: RemortStep updated to " @ $PrestoPref::RemortStep);
			}
		}
		return true; // Don't process this as a level-up
	}
	
	// Check if this is a level-up message
	// Level-up message: "Welcome to level X" (case-insensitive on 'level')
	%lowerMsg = String::toLower(%msg);
	if(String::findSubStr(%lowerMsg, "welcome to level ") != -1) {
		// Level-up detected! Trigger save
		echo("Presto: Level-up detected from message: [" @ %msg @ "]");
		say(0, "#savecharacter");
		echo("Presto: Level-up detected, character saved!");
		
		// Parse level from the message
		%levelPos = String::findSubStr(%lowerMsg, "welcome to level ");
		%afterLevel = String::getSubStr(%msg, %levelPos + String::len("welcome to level "), String::len(%msg));
		%currentLevelStr = String::getWord(%afterLevel, " ", 0); // Use space as separator
		%currentLevel = %currentLevelStr + 0; // Force numeric conversion

		// Run the auto-remort check (if enabled) using the new helper
		PrestoLevelUpSave::CheckAutoRemort(%currentLevel);
	}
	
	return true;
}

// Hook into client messages to detect level-up (using eventClientMessage like DeusRPGPack)
// When %client is 0, it's a server message (which includes level-up messages)
// This script is loaded via Install.cs which ensures Events.cs is already loaded
Event::Attach(eventClientMessage, PrestoLevelUpSave::OnClientMessage, prestoLevelUpSave);
echo("PrestoLevelUpSave: Event attached to eventClientMessage");

// Kick off an initial self-info request shortly after load to seed level/remort
schedule("PrestoLevelUpSave::RequestSelfInfo();", 5);

