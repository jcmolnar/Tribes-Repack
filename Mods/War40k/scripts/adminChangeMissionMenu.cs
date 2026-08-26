function Admin::changeMissionMenu(%clientId)
{
	Client::buildMenu(%clientId, "Pick Mission Type", "cmtype", true);
	%index = 1;
	for(%type = 1; %type < $MLIST::TypeCount; %type++)
	if($MLIST::Type[%type] != "Training")
	{
		Client::addMenuItem(%clientId, %index @ $MLIST::Type[%type], %type @ " 0");
		%index++;
	}
}