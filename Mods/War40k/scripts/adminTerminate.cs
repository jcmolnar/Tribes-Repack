function ADMIN::Terminate(%cl)
{
	Player::setArmor(%cl,larmor);
	armorChange(%cl);
	Player::blowUp(%cl);
	remoteKill(%cl);
	messageAll(0, Client::getName(%cl) @ " was obliterated by Chaos.");
}