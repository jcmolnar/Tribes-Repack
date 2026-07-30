//==============================================
// ChatFilter.cs - category-based chat line filtering
//==============================================
// The server tags filterable chat lines with ~category suffixes:
//   ~adv    = server advertisements (advertisements.cs)
//   ~spellc = "X is casting Y on you." messages (spells.cs)
//   ~loot   = loot/coin pickup messages (rpgfunk, Belt, backpack, ...)
//   ~house  = house payout/loyalty messages (gameevents, housebonus)
//   ~stats  = skill-gain updates + [Auto-Skill] spend broadcasts (skills, AutoSkill)
//
// The engine strips everything from "~" before displaying a chat line,
// so tagged messages look identical on clients without this script.
// Presto's events.cs parses the tags into eventClientTagMessage, and a
// handler returning "mute" suppresses the line entirely.
//
// Usage (console):
//   ChatFilter::toggle(adv);     - toggle advertisements
//   ChatFilter::toggle(spellc);  - toggle spell casting messages
//   ChatFilter::toggle(loot);    - toggle loot pickup messages
//   ChatFilter::status();        - show all filter states
//
// To persist a filter across sessions, set it in your autoexec.cs
// BEFORE this file is exec'd, e.g.: $ChatFilter::Hide["adv"] = true;
//
// New categories need no client changes - tag the server message with
// ~yourtag and toggle it with ChatFilter::toggle(yourtag).
//==============================================

Include("presto\\Event.cs");

// Known categories (for status display and toggle feedback)
$ChatFilter::Categories = "adv spellc loot house stats";
$ChatFilter::Desc["adv"] = "Server advertisements";
$ChatFilter::Desc["spellc"] = "Spell casting messages";
$ChatFilter::Desc["loot"] = "Loot pickup messages";
$ChatFilter::Desc["house"] = "House payout messages";
$ChatFilter::Desc["stats"] = "Skill/AutoSkill updates";

// ============================================
// The filter itself
// ============================================

function ChatFilter::onTagMessage(%client, %tag, %value, %repeated)
{
	if($ChatFilter::Hide[%tag])
		return "mute";
}

// Tagged attach: safe against re-exec of this file
Event::Attach(eventClientTagMessage, ChatFilter::onTagMessage, attachChatFilter);

// ============================================
// Console helpers
// ============================================

function ChatFilter::toggle(%category)
{
	%desc = $ChatFilter::Desc[%category];
	if(%desc == "")
		%desc = "'" @ %category @ "' messages";

	if($ChatFilter::Hide[%category])
	{
		$ChatFilter::Hide[%category] = "";
		%state = "SHOWN";
	}
	else
	{
		$ChatFilter::Hide[%category] = true;
		%state = "HIDDEN";
	}

	echo("ChatFilter: " @ %desc @ " -> " @ %state);
	Client::centerPrint("<jc><f0>" @ %desc @ ": <f1>" @ %state, 1);
	Schedule("Client::centerPrint(\"\", 1);", 2);
}

function ChatFilter::status()
{
	echo("ChatFilter states:");
	for(%i = 0; (%cat = GetWord($ChatFilter::Categories, %i)) != -1; %i++)
	{
		if($ChatFilter::Hide[%cat])
			%state = "HIDDEN";
		else
			%state = "shown";
		echo("  " @ %cat @ " (" @ $ChatFilter::Desc[%cat] @ "): " @ %state);
	}
}

echo("ChatFilter: chat category filtering loaded (ChatFilter::status() for states)");
