function processMenuKAffirm(%clientId, %opt)
{
	if(getWord(%opt, 0) == "yes") Admin::kick(%clientId, getWord(%opt, 1));
	Game::menuRequest(%clientId);
}