//Began:  11:11 PM, Sunday August 5th, 2007.
// To my knowledge, a brand new feature to the world of Tribes RPG. The idea mainly comes from City of Heroes..
// ..Yes, I do steal some of my ideas, just as anyone else. ;p

// Kay, so here's the jist of it. $Badge[x] = "Name Of Badge,Badge Description";
// If you look over the functions below, you'll find that BadgeName() will return what is before the comma
// And BadgeDesc()/BadgeDescription() will return what is after it.

// Note: It only looks for the FIRST comma, so you could have a comma in the description if you wish, but not one in the name.
// $Badge[347] = "Cookie Eater,The possessor of this badge eats cookies, you shall all bow before %2.";

// Just like for base death messages, you can also get the client's gender in the badge's name or description
// Use %1 for his/her, or %2 for him/her, %3 for er/ess, %4 for nothing/ess,
// %5 for nothing/dess, %6 for the name of the player.

// For example:
// $Badge[468] = "Sorcer%3 of Shazbot,The mighty Sorcer%3 of Shazbot is not to be looked upon; otherwise %1 gaze will destroy you.";
// Note: (%4 on, say, Wizard, would give you "Wizard" if male, and "Wizardess" if female. Hence "Nothing")

function CreateBadgeMenu(%clientId)
{
	Client::buildMenu(%clientId, "Select category:", "BadgeList", true);
	Client::addMenuItem(%clientId, "1Exploration...", "1 1");
	Client::addMenuItem(%clientId, "2Quest...", "2 1");
	Client::addMenuItem(%clientId, "4Accomplishments...", "3 1");
	Client::addMenuItem(%clientId, "3Accolades...", "4 1");
	Client::addMenuItem(%clientId, "xBack to belt...", "back");
//	return;
}

function processMenuBadgeList(%clientId, %page)
{
	dbecho($dbechoMode, "processMenuBadgeList(" @ %clientId @ ", " @ %page @ ")");

	if(%page == "back")
	{
		processMenuOptions(%clientId, belt);
		return;
	}
	%type = getWord(%page, 0);
	%page = getWord(%page, 1);

	%clientId.bulkNum = "";

	%l = 6;
	%ns = CountObjInCommaList($ClientData[%clientId, "Badges" @ %type]) - 1; echo("count:",%ns);
	%np = floor(%ns / %l);
	
	%lb = (%page * %l) - (%l-1);
	%ub = %lb + (%l-1);
	if(%ub > %ns)
		%ub = %ns;


	for(%j = 0; $Badge[%type, %j] != ""; %j++)
		if(hasBadge(%client, %type, %j))
			%tempbadgelist = %tempbadgelist @ " " @ %badge;

	if(%tempbadgelist == "")
	{
		CreateBadgeMenu(%clientId);
		client::sendMessage(%clientId, 1, "You have no badges of that type!");
		return;
	}
	else
		Client::buildMenu(%clientId, "Select a badge: (Page " @ %page @ ")", "BadgeDesc", true);

	%cnt = 0;
	for(%i = %lb; %i <= %ub; %i++)
		if((%badge = getword(%tempbadgelist, %i)) != -1)
			Client::addMenuItem(%clientId, %cnt++ @ BadgeName(%clientId, %type, %badge), %badge @ " " @ %page @ " " @ %type);

	if(%ns <= %l)
	{
		Client::addMenuItem(%clientId, "xBack to badge types...", "done");
	}
	else if(%page == 1)
	{
		Client::addMenuItem(%clientId, "nNext >>", "page " @ %page+1 @ " " @ %type);
		Client::addMenuItem(%clientId, "xBack to badge types...", "done");
	}
	else if(%page == %np + 1)
	{
		Client::addMenuItem(%clientId, "p<< Prev", "page " @ %page-1 @ " " @ %type);
		Client::addMenuItem(%clientId, "xBack to badge types...", "done");
	}
	else
	{
		Client::addMenuItem(%clientId, "nNext >>", "page " @ %page+1 @ " " @ %type);
		Client::addMenuItem(%clientId, "p<< Prev", "page " @ %page-1 @ " " @ %type);
	}

	return;
}
function processMenuBadgeDesc(%clientId, %opt)
{
	dbecho($dbechoMode, "processMenuBadgeDesc(" @ %clientId @ ", " @ %opt @ ")");

	%o = GetWord(%opt, 0);
	%p = GetWord(%opt, 1);
	%t = GetWord(%opt, 2);
	echo(%t);
	if(%o == "done")
	{
		CreateBadgeMenu(%clientId);
		return;
	}
	else if(%o != "page")
	{
		%msg = "<jc><f1>" @ BadgeName(%clientId, %type, %o) @ "\n<f2>" @ BadgeDesc(%clientId, %type, %o);
		bottomprint(%clientId, %msg, floor(String::len(%msg) / 20));
	}
	ProcessMenuBadgeList(%clientId, %t @ " " @ %p);
}

//AddToCommaList(%list, %item)
//RemoveFromCommaList(%list, %item)
//IsInCommaList(%list, %item)
//CountObjInCommaList(%list)

function GiveBadge(%client, %type, %number)
{
	if($Badge[%type, %number] == "" || HasBadge(%client, %type, %number) == "True") return;
	client::sendMessage(%client, 0, "You have earned the \"" @ BadgeName(%type, %number) @ "\" badge!");
	AddToCommaList($ClientData[%client, "Badges" @ %type], %number);
}
function AddBadge(%client, %type, %number)
{
	GiveBadge(%client, %type, %number);
}

function TakeBadge(%client, %type, %number)
{
	client::sendMessage(%client, 0, "You have lost a badge!");
	RemoveFromCommaList($ClientData[%client, "Badges" @ %type], %number);
}
function RemoveBadge(%client, %type, %number)
{
	TakeBadge(%client, %type, %number);
}

function HasBadge(%client, %type, %number)
{
	if(IsInCommaList($ClientData[%client, "Badges" @ %type], %number))
		return True;
	else
		return False;
}

function FilterBadge(%client, %string)
{
	if(Client::getGender(%client) == "Female")
	{
		%arg1 = "her";
		%arg2 = "her";
		%arg3 = "ess";
		%arg4 = "ess";
		%arg5 = "dess";
	}
	else
	{
		%arg1 = "his";
		%arg2 = "him";
		%arg3 = "er";
		%arg4 = "";
		%arg5 = "";
	}
	%arg6 = client::getName(%client);
	return sprintf(%string, %arg1, %arg2, %arg3, %arg4, %arg5, %arg6); 
}

function BadgeName(%client, %type, %number)
{
	return FilterBadge(%client, String::NEWgetSubStr($Badge[%type, %number], 0, string::findSubStr($Badge[%type, %number], ",")));
}

function BadgeDesc(%client, %type, %number)
{
	return FilterBadge(%client, String::NEWgetSubStr($Badge[%type, %number], string::findSubStr($Badge[%type, %number], ",") + 1, 999));
}
function BadgeDescription(%number)
{
	return BadgeDesc(%number);
}

function CountBadgeType(%list1, %type)
{
	dbecho($dbechoMode, "CountObjInCommaList(" @ %list @ ")");
	%list2 = %list1;
	for(%i = String::findSubStr(%list2, ","); (%p = String::findSubStr(%list2, ",")) != -1; %list2 = String::NEWgetSubStr(%list, %p+1, 99999))
		if(string::getSubStr(string::replace(%list1, %list2, ""), 0, 1) == ","@%type)
			%cnt++;
	return %cnt;
}

			// ALWAYS UPDATE THIS WHEN ADDING OR REMOVING A BADGE!
$BadgeCount = 32;	// ALWAYS UPDATE THIS WHEN ADDING OR REMOVING A BADGE!
			// ALWAYS UPDATE THIS WHEN ADDING OR REMOVING A BADGE!

//Exploration Badges, awarded when you visit certain places.
$Badge[1, 0] = "Explorer,This is the Explorer test badge!";
$Badge[1, 1] = "Solace,This is another test badge.";
$Badge[1, 2] = "Omg spaces in the name,This is another test badge.";

//Quest(Story) Badges, awarded for completing certain quests/quest-related tasks.
$Badge[2, 0] = "Pathfinder,You're sexy.";

//Achievement Badges, for acquiring remort levels and other things.
$Badge[3, 0] = "Diligent,You have reached remort level 10.";
$Badge[3, 1] = "Vigilent,You have reached remort level 25.";
$Badge[3, 2] = "Unwavering,You have reached remort level 50.";
$Badge[3, 3] = "Unyielding,You have reached remort level 100.";
$Badge[3, 4] = "Immortal One,You exist beyond time and space, you have reached remort level 500. You are a God%4.";
$Badge[3, 5] = "Padawan,You have completed the first trials of the Jedi.";
$Badge[3, 6] = "Jedi Knight,You are now a Jedi Knight.";
$Badge[3, 7] = "Jedi Master,You have become a Jedi Master.";
$Badge[3, 8] = "Jedi Council,You have become a member of the Jedi Council.";
$Badge[3, 9] = "Non-Commisioned Officer,You are now an NCO.";
$Badge[3, 10] = "Officer,You have been promoted to the Officer level.";
$Badge[3, 11] = "Admiralty,You have joined the ranks of the Admiralty.";
$Badge[3, 12] = "General,You are now in control. Bwahahahahaha.";
$Badge[3, 13] = "Bounty Hunter,You have collected many bounties.";
$Badge[3, 14] = "Cutpurse,You have stolen [over?] 10,000 coins.";
$Badge[3, 15] = "Stoic,Damage > 10,000? 50,000?";
$Badge[3, 16] = "Staunch,Damage > 20,000? 100,000?";
$Badge[3, 16] = "Stalwart,Damage > 150,000? 200,000?";

//Accolade Badges(Medals, Ribbons, etc.), awarded for things like somany kills, or somany spell-healed HP, etc.
$Badge[4, 0] = "Badge Of Friendship,Your faithfulness to your friends has earned you this badge.";
$Badge[4, 1] = "Badge Of Loyalty,Your loyalty has earned you this badge.";
$Badge[4, 2] = "Badge Of Honor,Your honerable actions have earned you this badge.";
$Badge[4, 3] = "Badge Of Reverence,Your reverence has earned you this badge.";
$Badge[4, 4] = "Alpha,Everyone who helped with alpha version testing of SWRPG received this badge.";
$Badge[4, 5] = "Beta,Everyone who helped with beta version testing of SWRPG received this badge.";
$Badge[4, 6] = "Exterminator,This badge is given to someone who has found and reported a bug/glitch/exploit in SWRPG.";
$Badge[4, 7] = "Fly Swatter,This badge is given to someone who has found and reported a bug/glitch/exploit in SWRPG.";
$Badge[4, 8] = "Medal of Honor,You have done honorable deeds.";
$Badge[4, 9] = "Distinguished Service Ribbon,Your notable acts have earned you this ribbon.";

// in the log in function, for removing a badge from everyone, add a check if hasbadge(x); removefromcommalist(",x,");
// you can check if they have the requirements for it also, whatever variable it is.
// IsInCommaList(%list, %item)