function Admin::voteSucceded()
{
	$curVoteInitiator.numVotesFailed = "";
	if($curVoteAction == "kick")
	{
		if($curVoteOption.voteTarget) Admin::kick(-1, $curVoteOption);
	}
	else if($curVoteAction == "admin")
	{
		if($curVoteOption.voteTarget)
		{
			$curVoteOption.isAdmin = true;
			messageAll(0, Client::getName($curVoteOption) @ " has become an administrator.");
			if($curVoteOption.menuMode == "options") Game::menuRequest($curVoteOption);
		}
		$curVoteOption.voteTarget = false;
	}
	else if($curVoteAction == "cmission")
	{
		messageAll(0, "Changing to mission " @ $curVoteOption @ ".");
		Vote::changeMission();
		Server::loadMission($curVoteOption);
	}
	else if($curVoteAction == "tourney") Admin::setModeTourney(-1);
	else if($curVoteAction == "ffa") Admin::setModeFFA(-1);
	else if($curVoteAction == "etd") Admin::setTeamDamageEnable(-1, true);
	else if($curVoteAction == "dtd") Admin::setTeamDamageEnable(-1, false);
	else if($curVoteOption == "smatch") Admin::startMatch(-1);
}