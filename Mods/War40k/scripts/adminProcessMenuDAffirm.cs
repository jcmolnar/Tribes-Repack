function processMenuDAffirm(%clientId, %opt)
{
	if(getWord(%opt, 0) == "yes")
	{
		if(%clientId.isSuperAdmin)
		{
			%cl = getWord(%opt, 1);
			%cl.isAdmin = false;
			messageAll(0, Client::getName(%clientId) @ " revoked " @ Client::getName(%cl) @ "'s admin ability.");
		}
	}
	Game::menuRequest(%clientId);
}