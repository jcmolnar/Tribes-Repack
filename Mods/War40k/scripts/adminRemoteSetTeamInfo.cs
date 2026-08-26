function remoteSetTeamInfo(%client, %team, %teamName, %skinBase)
{
	if(%team >= 0 && %team < 8 && %client.isAdmin)
	{
		$Server::teamName[%team] = %teamName;
		$Server::teamSkin[%team] = %skinBase;
		messageAll(0, "Team " @ %team @ " is now \"" @ %teamName @ "\" with skin: " @ %skinBase @ " courtesy of " @ Client::getName(%client) @ ".  Changes will take effect next mission.");
	}
}