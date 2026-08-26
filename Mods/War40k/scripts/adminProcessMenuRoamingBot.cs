function processMenuRoamingBot(%clientId, %opt)
{
	Client::buildMenu(%clientId, "Will Servitor Bot Be Roaming?:", "botalldone", true);
	Client::addMenuItem(%clientId, "1Yes ", %opt @ "_Roam");
	Client::addMenuItem(%clientId, "2No ", %opt);
	return;
}