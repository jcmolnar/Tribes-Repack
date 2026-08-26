function ADMIN::KillAllBots(%clientId, %options)
{
	%startCl = 2049;
	%endCl = %startCl + 50;
	%startkill = "0.5";
	for(%cl = %startCl; %cl < %endCl; %cl = %cl + 1)
	{
		if (Player::isAIControlled(%cl)) //Is this a bot?
		{
			%startkill = %startkill + 0.5;	      
			%aiName = Client::getName(%cl);
			echo("Bot " @ %aiName @ " has been terminated.");
			schedule ("AI::RemoveBot(" @ %aiName @ "," @ %cl @ ");",%startkill);
		}
	}
}