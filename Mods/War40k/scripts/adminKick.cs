function Admin::kick(%admin, %client, %ban)
{
	if(%admin != %client && (%admin == -1 || %admin.isAdmin))
	{
		if(%ban && !%admin.isSuperAdmin) return;
		if(%ban)
		{
			%word = "banned";
			%cmd = "BAN: ";
		}
		else
		{
			%word = "kicked";
			%cmd = "KICK: ";
		}
		if(%client.isSuperAdmin)
		{
			if(%admin == -1) messageAll(0, "A super admin cannot be " @ %word @ ".");
			else Client::sendMessage(%admin, 0, "A super admin cannot be " @ %word @ ".");
			return;
		}
		%ip = Client::getTransportAddress(%client);
		echo(%cmd @ %admin @ " " @ %client @ " " @ %ip);
		if(%ip == "") return;
		if(%ban) BanList::add(%ip, 1800);
		else BanList::add(%ip, 180);
		%name = Client::getName(%client);
		if(%admin == -1)
		{
			MessageAll(0, %name @ " was " @ %word @ " from vote.");
			Net::kick(%client, "You were " @ %word @ " by  consensus.");
		}
		else
		{
			MessageAll(0, %name @ " was " @ %word @ " by " @ Client::getName(%admin) @ ".");
			Net::kick(%client, "You were " @ %word @ " by " @ Client::getName(%admin));
		}
	}
}