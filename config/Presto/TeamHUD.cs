// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// TeamHUD.CS								Presto, March '99 
//
//	A cool new way of keeping track of your teammates.
//
//	The TeamHUD displays at the bottom of the screen, and is an easy
//	way to track what's going on in the game.  For instance, when someone
//	is killed, a little death icon appears next to their name.  And when
//	someone speaks, their name will flash (right now, not much use.  But
//	later, you could use it to find their number and waypoint them,
//	reply personally, etc.)
//
//	It will also, more importantly, keep track of the jobs everyone
//	is doing.  That means that if someone declares that they are going
//	on the offensive, you will see a red icon next to their name.
//	Similarly defense players get green icons.  Players using my
//	CHORES.CS job chat menu will get very specific icons next to their
//	names!  For instance repair icons identify what they are repairing.
//
//	This is a work in progress!  I'm always open to suggestions.
//	
//	Let me try to explain a few of my design goals with the TeamHUD
//	in case people are curious why, for instance, there are blanks left
//	in the HUD when people leave.
//
//	I'm hoping that scripts will use the client number maintained in
//	TeamHUD to allow things like waypoint-to-player, or maybe even (if
//	we can figure out enough GUI items), messaging with a player's
//	name prepended ("Presto:  Player>> Meet me at the base!").  In thse
//	cases the number will be more important than the name because it's
//	easier to type.
//
//	Towards this end, I keep the list sorted in numerical order rather
//	than sorted by name.  But because you can't easily scan a list of names
//	for one in particular unless they're sorted alphabetically, I try to
//	avoid moving a name once it's in a certain position.  That way you
//	always know where to look when you're trying to remember someone's
//	number - once you've found them the first time.
//
//	Eventually I will offer a toggle to let the user determine the sort
//	order himself.  In particular I will offer at least the following
//	sorts:
//		* Name order
//		* Number order as is done now
//		* Job groupings (offense, defense, repair, etc)
//
// ---------------------------------------------------------------------------
Include("presto\\HUD.cs");
Include("presto\\TeamTrak.cs");
Include("presto\\JobTrak.cs");
Include("presto\\KillTrak.cs");

if ($PrestoPref::TeamHUDColumns <= 1)
	$PrestoPref::TeamHUDColumns = 1;

function TeamHUD::Reset() {
	deleteVariables("$TeamHudData::*");

	HUD::NewFrame(hudTeam, "", getWord($PrestoPref::TeamHudPosition, 0),
					   getWord($PrestoPref::TeamHudPosition, 1),
					   getWord($PrestoPref::TeamHudPosition, 2),
					   0);
	HUD::Display(hudTeam, $PrestoPref::TeamHudDisplayOnEntry);
	TeamHUD::addClient(getManagerId());
	Team::ForAllClients(Team::Friendly(), TeamHUD::AddClient);
	}

function TeamHUD::Resize(%rows) {
	if (%rows == $TeamHudData::Rows)
		return;
	$TeamHudData::Rows = %rows;

	$TeamHudData::perRow = getWord($PrestoPref::TeamHUDPosition, 3);
	HUD::SetCoord(hudTeam, height, %rows * $TeamHudData::perRow);
	}
function TeamHUD::AddCell(%num) {
	%cells = HUD::GetGuiObjectCount(hudTeam);
	if (%cells > %num)
		return;

	%columns = $PrestoPref::TeamHUDColumns;
	%perRow = $TeamHudData::perRow;
	%perCol = HUD::Width(hudTeam) / %columns;
 
	%row = floor(%cells / %columns);
	%col = %cells - (%row * %columns);
	while (%cells <= %num) {
		Hud::AddObject(hudTeam, FearGuiFormattedText, %col * %perCol + 4, %row * %perRow - 2, %perCol-4, %perRow);
		TeamHUD::UpdatePlayer(%cells);

		%col++;
		if (%col == %columns) {
			%col = 0;
			%row++;
			}
		%cells++;
		}
	}

function TeamHUD::UpdatePlayer(%p) {
	%cell = HUD::GetGuiObject(hudTeam, %p);
	if (%cell == -1)
		return;
	%cell = Object::GetName(%cell);

	%client = $TeamHudData::client[%p];
	if (%client == "")
		Control::SetValue(%cell , %client);
	else	{
		%status = "";

		%status = %status @ JobTrak::GetIcon(%client);
		if ($TeamHudData::death[%client])
			%status = %status @ "<B1,3:skull_small.bmp>";

		if ($TeamHudData::blink[%client] == 1)
			%color = "<f2>";
		else	%color = "<f1>";

		Control::SetValue(%cell, "<f1>"@%p@".<L3>" @ %status @ %color @ Client::GetName(%client));
		}
	}

function TeamHUD::FindEmptySlot(%client) {
	%slot = 0;
	while ($TeamHudData::client[%slot] != "")
		%slot++;
	return %slot;
	}
function TeamHUD::AddClient(%client) {
	%slot = $TeamHudData::slot[%client];
	if (%slot != "")
		return;

	%slot = TeamHUD::FindEmptySlot(%client);
	$TeamHudData::client[%slot] = %client;
	$TeamHudData::slot[%client] = %slot;

	if (%slot >= $TeamHudData::maxClient)
		$TeamHudData::maxClient = %slot;

	%columns = $PrestoPref::TeamHUDColumns;
	TeamHud::Resize(floor($TeamHudData::maxClient / %columns) + 1);
	TeamHud::AddCell(%slot);
	TeamHud::UpdatePlayer(%slot);
	return %slot;
	}
function TeamHUD::DeleteClient(%client) {
	%slot = $TeamHudData::slot[%client];
	if (%slot == "")
		return;

	$TeamHudData::client[%slot] = "";
	$TeamHudData::slot[%client] = "";
	while ($TeamHudData::client[$TeamHudData::maxClient] == "") {
		if ($TeamHudData::maxClient <= 0)
			break;
		$TeamHudData::maxClient--;
		}

	%columns = $PrestoPref::TeamHUDColumns;
	TeamHud::Resize(floor($TeamHudData::maxClient / %columns) + 1);
	TeamHud::UpdatePlayer(%slot);
	}

function TeamHUD::Death(%client, %show) {
	%slot = $TeamHudData::slot[%client];
	if (%slot == "")
		return;

	if (!%show)
		$TeamHudData::death[%client] = "";
	else	{
		$TeamHudData::death[%client] = true;
		schedule("TeamHUD::Death("@%client@", false);", 5);
		}

	TeamHUD::UpdatePlayer(%slot);
	}
function TeamHUD::Blink(%client, %mode) {
	%slot = $TeamHudData::slot[%client];
	if (%slot == "")
		return;

	if (%mode == 8)
		$TeamHudData::blink[%client] = "";
	else	{
		$TeamHudData::blink[%client] = !$TeamHudData::blink[%client];
		schedule("TeamHUD::Blink("@%client@", "@ (%mode + 1) @");", 0.3);
		}

	TeamHUD::UpdatePlayer(%slot);
	}

function TeamHUD::OnClientMessage(%client, %message) {
	if (%client == 0)
		return;

	if ($TeamHudData::blink[%client] == "")
		TeamHUD::Blink(%client, 0);

	return;
	}

function TeamHUD::OnKill(%killer, %victim, %weapon) {
	if (!$TeamHudData::death[%victim])
		TeamHUD::Death(%victim, true);
	}

function TeamHUD::ToggleDisplay() {
	if (HUD::GetDisplayed(hudTeam))
		HUD::Display(hudTeam, false);
	else	{
		TeamHUD::Reset();
		HUD::Display(hudTeam, true);
		}
	}

function TeamHUD::OnResize(%hud) {
	if (%hud != hudTeam)
		return;

	%width = HUD::GetCoordValue(hudTeam, 2);
	if (%width != %TeamHUDData::lastWidth)
		TeamHUD::Reset();
	%TeamHUDData::lastWidth = %width;
	}

function TeamHUD::Init() {
	Event::Attach(eventClientMessage, TeamHUD::OnClientMessage);
	Event::Attach(eventKillTrak, TeamHUD::OnKill);
	Event::Attach(eventTeammateJoined, TeamHUD::AddClient);
	Event::Attach(eventTeammateLeft, TeamHUD::DeleteClient);
	Event::Attach(eventConnected, TeamHUD::Reset);
	Event::Attach(eventJobUpdated, TeamHUD::UpdatePlayer);
	Event::Attach(eventHUDResized, TeamHUD::OnResize);
	}

if (Presto::Enabled(TeamHUD))
	TeamHUD::Init();