function processMenuCMType(%clientId, %options)
{
	%curItem = 0;
	%option = getWord(%options, 0);
	%first = getWord(%options, 1);
	Client::buildMenu(%clientId, "Pick Mission", "cmission", true);
	for(%i = 0; (%misIndex = getWord($MLIST::MissionList[%option], %first + %i)) != -1; %i++)
	{
		if(%i > 6)
		{
			Client::addMenuItem(%clientId, %i+1 @ "More missions...", "more " @ %first + %i @ " " @ %option);
			break;
		}
		Client::addMenuItem(%clientId, %i+1 @ $MLIST::EName[%misIndex], %misIndex @ " " @ %option);
	}
}