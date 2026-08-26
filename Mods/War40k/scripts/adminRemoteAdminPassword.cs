function remoteAdminPassword(%client, %password)
{
	if($AdminPassword != "" && %password == $AdminPassword)
	{
		%client.isAdmin = true;
		%client.isSuperAdmin = true;
	}
}