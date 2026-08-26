function Admin::startMatch(%admin)
{
	if(%admin == -1 || %admin.isAdmin)
	{
		if(!$CountdownStarted && !$matchStarted)
		{
			if(%admin == -1) messageAll(0, "Match start countdown forced by vote.");
			else messageAll(0, "Match start countdown forced by " @ Client::getName(%admin));
			Game::ForceTourneyMatchStart();
		}
	}
}