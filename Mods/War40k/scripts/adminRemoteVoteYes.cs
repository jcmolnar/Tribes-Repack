function remoteVoteYes(%clientId)
{
	%clientId.vote = "yes";
	centerprint(%clientId, "", 0);
}