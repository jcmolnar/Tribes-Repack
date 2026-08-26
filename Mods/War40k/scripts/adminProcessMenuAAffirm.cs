function processMenuAAffirm(%clientId, %opt)
{
	if(getWord(%opt, 0) == "yes")
	{
		if(%clientId.isSuperAdmin)
		{
			%cl = getWord(%opt, 1);
			%cl.isAdmin = true;
			messageAll(0, Client::getName(%clientId) @ " made " @ Client::getName(%cl) @ " into an admin.");
		}
	}
	Game::menuRequest(%clientId);
}