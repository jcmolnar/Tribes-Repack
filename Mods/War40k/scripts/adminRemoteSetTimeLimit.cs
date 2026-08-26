function remoteSetTimeLimit(%client, %time)
{
	%time = floor(%time);
	if(%time == $Server::timeLimit || (%time != 0 && %time < 1)) return;
	if(%client.isAdmin)
	{
		$Server::timeLimit = %time;
		if(%time) messageAll(0, Client::getName(%client) @ " changed the long time limit to " @ %time @ " minute(s).");
		else messageAll(0, Client::getName(%client) @ " disabled the time limit.");
	}
}