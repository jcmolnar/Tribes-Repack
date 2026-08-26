function Admin::setModeTourney(%clientId)
{
	if(!$Server::TourneyMode && (%clientId == -1 || %clientId.isAdmin))
	{
		$Server::TeamDamageScale = 1;
		if(%clientId == -1) messageAll(0, "Server switched to Total War Mode.");
		else messageAll(0, "Server switched to Total War Mode by " @ Client::getName(%clientId) @ ".");
		$Server::TourneyMode = true;
		Server::nextMission();
	}
}