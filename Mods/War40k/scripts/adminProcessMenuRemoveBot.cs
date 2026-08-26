function processMenuRemoveBot(%clientId, %options)
{
	%curItem = 0;
	%first = getWord(%options, 0);
	Client::buildMenu(%clientId, "Pick Servitor Bot to remove", "rbot", true);
	%i = 0;
	%menunum = 0;
	%startCl = 2049;
	%endCl = %startCl + 50;
	for(%cl = %startCl; %cl < %endCl; %cl = %cl + 1)
	if (Player::isAIControlled(%cl)) //Is this a bot?
	{
		%aiName = Client::getName(%cl);
		%i = %i + 1;
		if (%i > %first)  // Skip some bots if we selected "more Servitor Bot" previously
		{
			%menunum = %menunum + 1;
			if(%menunum > 6)
			{
				Client::addMenuItem(%clientId, %menunum @ "More Servitor Bot...", "more " @ %first + %menunum - 1);
				break;
			}
			Client::addMenuItem(%clientId, %menunum @ %aiName, %aiName);
		}
	}
	return;
}