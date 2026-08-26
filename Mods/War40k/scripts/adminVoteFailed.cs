function Admin::voteFailed()
{
	$curVoteInitiator.numVotesFailed++;
	if($curVoteAction == "kick" || $curVoteAction == "admin") $curVoteOption.voteTarget = "";
}