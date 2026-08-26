function processMenuAMAffirm(%clientId, %opt)
{
	if(getWord(%opt, 0) == "yes")
	{
		if(%clientId.isSuperAdmin)
		{
			%cl = getWord(%opt, 1);
			%cl.muteAll = true;
			messageAll(0, Client::getName(%clientId) @ " muted " @ Client::getName(%cl) @ " to everyone.");
		}
	}
	Game::menuRequest(%clientId);
}