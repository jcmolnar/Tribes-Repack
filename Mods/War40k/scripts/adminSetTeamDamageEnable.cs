function Admin::setTeamDamageEnable(%admin, %enabled)
{
	if(%admin == -1 || %admin.isAdmin)
	{
		if(%enabled)
		{
			$Server::TeamDamageScale = 1;
			if(%admin == -1) messageAll(0, "Team damage set to ENABLED by consensus.");
			else messageAll(0, Client::getName(%admin) @ " ENABLED team damage.");
		}
		else
		{
			$Server::TeamDamageScale = 0;
			if(%admin == -1) messageAll(0, "Team damage set to DISABLED by consensus.");
			else messageAll(0, Client::getName(%admin) @ " DISABLED team damage.");
		}
	}
}