function remoteVoteNo(%clientId)
{
	%clientId.vote = "no";
	centerprint(%clientId, "", 0);
}