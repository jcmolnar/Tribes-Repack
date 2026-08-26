function Admin::setModeFFA(%clientId)
{
	if($Server::TourneyMode && (%clientId == -1 || %clientId.isAdmin))
	{
		$Server::TeamDamageScale = 0;
		if(%clientId == -1) messageAll(0, "Server switched to Free-For-All Mode.");
		else messageAll(0, "Server switched to Free-For-All Mode by " @ Client::getName(%clientId) @ ".");
		$Server::TourneyMode = false;
		centerprintall(); // clear the messages
		if(!$matchStarted && !$countdownStarted)
		{
			if($Server::warmupTime) Server::Countdown($Server::warmupTime);
			else Game::startMatch();
		}
	}
}