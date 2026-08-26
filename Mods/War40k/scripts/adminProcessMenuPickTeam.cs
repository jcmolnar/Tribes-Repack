function processMenuPickTeam(%clientId, %team, %adminClient)
{
	checkPlayerCash(%clientId);
	if(%team != -1 && %team == Client::getTeam(%clientId)) return;
	if(%clientId.observerMode == "justJoined")
	{
		%clientId.observerMode = "";
		centerprint(%clientId, "");
	}
	if((!$matchStarted || !$Server::TourneyMode || %adminClient) && %team == -2)
	{
		if(Observer::enterObserverMode(%clientId))
		{
			%clientId.notready = "";
			if(%adminClient == "") messageAll(0, Client::getName(%clientId) @ " became an observer.");
			else messageAll(0, Client::getName(%clientId) @ " was forced into observer mode by " @ Client::getName(%adminClient) @ ".");
			Game::resetScores(%clientId);	
			Game::refreshClientScore(%clientId);
		}
		return;
	}
	%player = Client::getOwnedObject(%clientId);
	if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player)) 
	{
		playNextAnim(%clientId);
		Player::kill(%clientId);
	}
	%clientId.observerMode = "";
	if(%adminClient == "") messageAll(0, Client::getName(%clientId) @ " changed teams.");
	else messageAll(0, Client::getName(%clientId) @ " was teamchanged by " @ Client::getName(%adminClient) @ ".");
	if(%team == -1)
	{
		Game::assignClientTeam(%clientId);
		%team = Client::getTeam(%clientId);
	}
	GameBase::setTeam(%clientId, %team);
	%clientId.teamEnergy = 0;
	Client::clearItemShopping(%clientId);
	if(Client::getGuiMode(%clientId) != 1) Client::setGuiMode(%clientId,1);		
	Client::setControlObject(%clientId, -1);
	Game::playerSpawn(%clientId, false);
	%team = Client::getTeam(%clientId);
	if($TeamEnergy[%team] != "Infinite") $TeamEnergy[%team] += $InitialPlayerEnergy;
	if($Server::TourneyMode)
	{
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) 
		{
			if(%cl.observerMode == "pickingTeam")
			{
				Game::initialMissionDrop(%cl);
				continue;
			}
		}
		if(!$CountdownStarted)
		{
			bottomprint(%clientId, "<f1><jc>Press FIRE when ready.", 0);
			%clientId.notready = true;
		}
	}
}