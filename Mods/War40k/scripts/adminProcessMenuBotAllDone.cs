function processMenuBotAllDone(%clientId, %opt)
{
	%teamnum = GameBase::getTeam(%clientId);
	AI::SpawnAdditionalBot(%opt, %teamNum, 1);
	return;
}