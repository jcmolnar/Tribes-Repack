function processMenuBotSelect(%clientId, %opt)
{
	messageAll(0, Client::getName(%clientId) @ " Spawns a Servitor Bot...");
	echo("BOT: Spawned " @ %clientId);
	AI::SpawnAdditionalBot(%opt, %clientId);
}