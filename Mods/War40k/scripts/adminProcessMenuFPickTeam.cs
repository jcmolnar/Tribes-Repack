function processMenuFPickTeam(%clientId, %team)
{
	if(%clientId.isAdmin) processMenuPickTeam(%clientId.ptc, %team, %clientId);
	%clientId.ptc = "";
}