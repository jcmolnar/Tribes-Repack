function processMenuRaces(%clientId, %opt)
{
	if (%opt == "reg")
	{
		$Server::RaceOption = 0;
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Server set to Marine vs. Eldar\", 3);", 0);
		return;
	}
	if (%opt == "marine")
	{
		$Server::RaceOption = 1;
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Server set to Marine vs. Marine\", 3);", 0);
		return;
	}
	if (%opt == "eldar")
	{
		$Server::RaceOption = 2;
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Server set to Eldar vs. Eldar\", 3);", 0);
		return;
	}
	if (%opt == "allarmor")
	{
		$Server::RaceOption = 3;
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Server set to All Available\", 3);", 0);
		return;
	}
}