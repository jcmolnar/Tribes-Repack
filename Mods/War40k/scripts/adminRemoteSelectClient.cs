function remoteSelectClient(%clientId, %selId)
{
	if(%clientId.selClient != %selId)
	{
		%clientId.selClient = %selId;
		if(%clientId.menuMode == "options") Game::menuRequest(%clientId);
		remoteEval(%clientId, "setInfoLine", 1, "Game Stats");
		remoteEval(%clientId, "setInfoLine", 2, "Last TKer : " @ $War40k::LastTKer);
		remoteEval(%clientId, "setInfoLine", 3, "Last TKed : " @ $War40k::LastTKed);
		remoteEval(%clientId, "setInfoLine", 4, "TK Count  : " @ $War40k::TKCount);
		remoteEval(%clientId, "setInfoLine", 5, "Last TKed by:  " @ %clientId.LastTker);
		remoteEval(%clientId, "setInfoLine", 6, "You Last TKed: " @ %clientId.LastTKed);
		remoteEval(%clientId, "setInfoLine", 7, "Your TK Count: " @ %clientId.TKCount);
		remoteEval(%clientId, "setInfoLine", 8, "..............");
		remoteEval(%clientId, "setInfoLine", 9, "..............");
		remoteEval(%clientId, "setInfoLine", 10, "..............");
	}
}
