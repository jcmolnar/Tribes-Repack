function checkTeams()
{
	%numTeams = getNumTeams();
	%numPlayers = getNumClients();
	for(%i=0; %i < %numTeams; %i++) %numTeamPlayers[%i] = 0;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		%team = Client::getTeam(%cl);
		if(%team != -1)  %numTeamPlayers[%team]++;
	}
	%lowPlayer = %numTeamPlayers[0];
	%lowTeam = 0;
	%tieteams = 0;	
	for(%i=1; %i < %numTeams; %i++)
	{
		if(%numTeamPlayers[%i] == %lowPlayer)
		{
			%tieteams++;
		}
		if(%numTeamPlayers[%i] < %lowPlayer)
		{
			%lowTeam = %i;
			%lowPlayer = %numTeamPlayers;
		}
	}
	if (%tieteams == %numTeams - 1) return -1;
	return %lowTeam;
} 