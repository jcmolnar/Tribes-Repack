function processMenuSelBotGender(%clientId, %opt)
{
	Client::buildMenu(%clientId, "Select Servitor Bot gender:", "roamingbot", true);
	Client::addMenuItem(%clientId, "1Male " @ %opt, %opt @ "_Male");
	Client::addMenuItem(%clientId, "2Female " @ %opt, %opt @ "_Female");
	return;
}