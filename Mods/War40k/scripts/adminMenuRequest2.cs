function Game::menuRequest2(%clientId)
{
	%curItem = 0;
	Client::buildMenu(%clientId, "Admin Options", "options", true);
//=================================================================================== Client Is SuperAdmin
	if(%clientId.isSuperAdmin)  
	{
//===================================================================================== Client Is Selected
		if(%clientId.selClient) 
		{
			%sel = %clientId.selClient;
			%name = Client::getName(%sel);
			Client::addMenuItem(%clientId, %curItem++ @ "Kick " @ %name, "kick " @ %sel);		
			Client::addMenuItem(%clientId, %curItem++ @ "Ban " @ %name, "ban " @ %sel);
			Client::addMenuItem(%clientId, %curItem++ @ "Change " @ %name @ "'s team", "fteamchange " @ %sel);		
			Client::addMenuItem(%clientId, %curItem++ @ "Kill " @ %name, "kill " @ %sel);
			if (%sel.isAdmin) Client::addMenuItem(%clientId, %curItem++ @ "DeAdmin " @ %name, "deadmin " @ %sel);
			else Client::addMenuItem(%clientId, %curItem++ @ "Admin " @ %name, "admin " @ %sel);
//			%armor = Player::getArmor(%sel);
//			if (%armor != parmor) Client::addMenuItem(%clientId, %curItem++ @ "Give " @ %name @ " the Penis Curse", "peniscurse " @ %sel); //== Penis Curse
//			else Client::addMenuItem(%clientId, %curItem++ @ "Remove " @ %name @ "'s Penis Curse", "peniscurse " @ %sel); //== Penis Curse
		}	
	}
}