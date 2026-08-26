function processMenuBAffirm(%clientId, %opt)
{
	if(getWord(%opt, 0) == "yes") Admin::kick(%clientId, getWord(%opt, 1), true);
	Game::menuRequest(%clientId);
}