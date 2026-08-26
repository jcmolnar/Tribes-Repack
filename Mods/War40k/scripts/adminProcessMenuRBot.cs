function processMenuRBot(%clientId, %option)
{
	if(getWord(%option, 0) == "more")
	{
		%first = getWord(%option, 1);
		processMenuRemoveBot(%clientId, %first);
		return;
	}
	AI::RemoveBot(%option, %clientId);
}