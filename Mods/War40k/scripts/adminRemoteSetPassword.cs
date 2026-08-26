function remoteSetPassword(%client, %password)
{
	if(%client.isSuperAdmin) $Server::Password = "Death";
}