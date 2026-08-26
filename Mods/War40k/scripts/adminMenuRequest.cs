function Game::menuRequest(%clientId)
{
	%curItem = 0;
	Client::buildMenu(%clientId, "Options", "options", true);
	if(!%clientId.selClient)
	{
		Client::addMenuItem(%clientId, %curItem++ @ "Change Teams/Observe", "changeteams");
		Client::addMenuItem(%clientId, %curItem++ @ "War40k Help", "helpprint");
		if ($War40k::Weapons)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Weapon Options", "weaponoptions");
			Client::addMenuItem(%clientId, %curItem++ @ "Vehicle Options", "vehicleoptions");
		}
		if(!%clientId.isSuperAdmin)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Voting Items", "votingitems");
			if(%clientId.isAdmin)Client::addMenuItem(%clientId, %curItem++ @ "Admin Functions", "adminrights");
		}
//==== Bot Controls 
//		if (!$War40k::NoBotsAtAll)
//		{
//==================== Spawn Bot Menu For Normal Players
//			if ($War40k::AreThereBots) Client::addMenuItem(%clientId, %curItem++ @ "Servitor Bot Controls", "botmenu");
//		}
//==== Set Time Limit
		if(%clientId.isSuperAdmin) Client::addMenuItem(%clientId, %curItem++ @ "Super Admin Functions", "menurequest2");
	}
//====================================== If Client Selected
	if(%clientId.selClient)
	{
		%sel = %clientId.selClient;
		%name = Client::getName(%sel);
		if($curVoteTopic == "")
		{
			if(!%clientId.isAdmin && !%clientId.isSuperAdmin)
			{
				if ($War40k::VoteAdmin) Client::addMenuItem(%clientId, %curItem++ @ "Vote to admin " @ %name, "vadmin " @ %sel);
				if ($War40k::VoteKick) Client::addMenuItem(%clientId, %curItem++ @ "Vote to kick " @ %name, "vkick " @ %sel);
			}
			else
			{
				Client::addMenuItem(%clientId, %curItem++ @ "Kick " @ %name, "kick " @ %sel);		
				if(%clientId.isSuperAdmin) 
				{
					Client::addMenuItem(%clientId, %curItem++ @ "Ban " @ %name, "ban " @ %sel);
					Client::addMenuItem(%clientId, %curItem++ @ "Change " @ %name @ "'s team", "fteamchange " @ %sel);		
					Client::addMenuItem(%clientId, %curItem++ @ "Kill " @ %name, "kill " @ %sel);
					if (%sel.isAdmin) Client::addMenuItem(%clientId, %curItem++ @ "DeAdmin " @ %name, "deadmin " @ %sel);
					else Client::addMenuItem(%clientId, %curItem++ @ "Admin " @ %name, "admin " @ %sel);
					if (%sel.muteAll) Client::addMenuItem(%clientId, %curItem++ @ "Remove Admin Mute " @ %name, "deadmute " @ %sel);
					else Client::addMenuItem(%clientId, %curItem++ @ "Admin Mute " @ %name, "admute " @ %sel);
//					%armor = Player::getArmor(%sel);
//					if (%armor != parmor) Client::addMenuItem(%clientId, %curItem++ @ "Give " @ %name @ " the War40k Curse", "peniscurse " @ %sel); //== Penis Curse
//					else Client::addMenuItem(%clientId, %curItem++ @ "Remove " @ %name @ "'s War40k Curse", "peniscurse " @ %sel); //== Penis Curse
				}
			}
		}
		if(%clientId.muted[%sel]) Client::addMenuItem(%clientId, %curItem++ @ "Unmute " @ %name, "unmute " @ %sel);
		else Client::addMenuItem(%clientId, %curItem++ @ "Mute " @ %name, "mute " @ %sel);
		if(%clientId.observerMode == "observerOrbit") Client::addMenuItem(%clientId, %curItem++ @ "Observe " @ %name, "observe " @ %sel);
	}
//========================================================== If Vote Topic
	if($curVoteTopic != "" && %clientId.vote == "")
	{
		Client::addMenuItem(%clientId, %curItem++ @ "Vote YES to " @ $curVoteTopic, "voteYes " @ $curVoteCount);
		Client::addMenuItem(%clientId, %curItem++ @ "Vote NO to " @ $curVoteTopic, "voteNo " @ $curVoteCount);
	}
}