function processMenuDMAffirm(%clientId, %opt)
{
	if(getWord(%opt, 0) == "yes")
	{
		if(%clientId.isSuperAdmin)
		{
			%cl = getWord(%opt, 1);
			%cl.muteAll = false;
			messageAll(0, Client::getName(%clientId) @ " removed " @ Client::getName(%cl) @ "'s mute to everyone.");
		}
	}
	Game::menuRequest(%clientId);
}