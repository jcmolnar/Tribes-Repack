function processMenuHelpers(%clientId, %opt)
{
	if (%opt == "flag") 
	{
		if (%flag == "-1")
		{
			schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>You are not carrying a flag! .\", 10);", 0);		
		}   	
		if (%flag == "flag")
		{
			schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>You are carrying a flag, you need to take the flag to your flag stand!.\", 10);", 0);		
		}
	}
	if (%opt == "helparmor")
	{
		Client::buildMenu(%clientId, "Armor Help", "helpers", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Check Armor And Find Out Info", "firstarmor");
		return;
	}
	if (%opt == "firstarmor")
	{
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>This is still being made, try again in the next version.\", 10);", 0); 			
		return;
	}
	if (%opt == "locate")
	{
		Client::buildMenu(%clientId, "Locate Flags", "helpers", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Enemy Flag Location", "nmeflag");
		Client::addMenuItem(%clientId, %curItem++ @ "Friendly Flag Location", "frdflag");
		return;
	}
	if (%opt == "nmeflag")
	{
		%playerteam = GameBase::getTeam(%clientId);
		%playerpos = GameBase::getPosition(%clientId);
		if (%playerteam == 0)
		{
			%pos = GameBase::getPosition($teamFlag[1]);
			%posX = getWord(%pos,0);
			%posY = getWord(%pos,1);
			%distance = Vector::getDistance(%pos, %playerpos);
		}
		else if (%playerteam == 1)
		{
			%pos = GameBase::getPosition($teamFlag[0]);
			%posX = getWord(%pos,0);
			%posY = getWord(%pos,1);
			%distance = Vector::getDistance(%pos, %playerpos);
		}
		else if (%playerteam > 1)
		{
			schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Locate does not work in multi team.\", 3);", 0);
			return;
		}
		issueCommand(%clientId, %clientId, 0,"Waypoint set to enemy flag. ", %posX, %posY);
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>The enemy flag is " @ %distance @ " meters away.\", 3);", 0);
		return;
	}
	if (%opt == "frdflag")
	{
		%playerteam = GameBase::getTeam(%clientId);
		%playerpos = GameBase::getPosition(%clientId);
		%pos = GameBase::getPosition($teamFlag[%playerteam]);
		%posX = getWord(%pos,0);
		%posY = getWord(%pos,1);
		%distance = Vector::getDistance(%pos, %playerpos);
		issueCommand(%clientId, %clientId, 0,"Way point set to your flag. Your flag is " @ %distance @ "meters away.", %posX, %posY);
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Your flag is " @ %distance @ " meters away.\", 3);", 0);
		return;
	}
}