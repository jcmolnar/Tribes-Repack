function processMenuSelBotAction(%clientId, %opt)
{
	if (%opt == "spawnbot") 
	{
		Client::buildMenu(%clientId, "Select Servitor Bot type:", "selbotgender", true);
		Client::addMenuItem(%clientId, "1Guard", "Guard");
		Client::addMenuItem(%clientId, "2Demo", "Demo");
		Client::addMenuItem(%clientId, "3Painter", "Painter");
		Client::addMenuItem(%clientId, "4Sniper", "Sniper");
		Client::addMenuItem(%clientId, "5Medic", "Medic");
//		Client::addMenuItem(%clientId, "6Miner", "Miner");   //======= Removed Miner Bot for now to limit bot amounts
		return;
	}
	else if (%opt == "removebot")
	{
		%opt = 0;
		processMenuRemoveBot(%clientId, %opt);
		return;
	}
	else if (%opt == "kbaffirm")
	{
		ADMIN::KillAllBots();
		return;
	}
}