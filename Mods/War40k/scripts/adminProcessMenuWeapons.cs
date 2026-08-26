function processMenuWeapons(%clientId, %opt)
{
	if (%opt == "weapon_gl")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Grenade Options", "weapons", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Frag", "weapon_gl_reg");
		Client::addMenuItem(%clientId, %curItem++ @ "Haywire", "weapon_gl_haywire");
		Client::addMenuItem(%clientId, %curItem++ @ "MIRV", "weapon_gl_hellfire");
		Client::addMenuItem(%clientId, %curItem++ @ "Plasma", "weapon_gl_plasma");
		Client::addMenuItem(%clientId, %curItem++ @ "Krak", "weapon_gl_krak");
		Client::addMenuItem(%clientId, %curItem++ @ "Inferno", "weapon_gl_inferno");
		return;
	}
	if (%opt == "weapon_mortar")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Bolt Pistol Options", "weapons", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Standard", "weapon_mortar_reg");
		//Client::addMenuItem(%clientId, %curItem++ @ "Hellfire", "weapon_mortar_napalm");
		Client::addMenuItem(%clientId, %curItem++ @ "Kraken", "weapon_mortar_haywire");
		return;
	}
	if (%opt == "weapon_magnum")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Bolter Options", "weapons", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Standard", "weapon_magnum_reg");
		//Client::addMenuItem(%clientId, %curItem++ @ "Inferno", "weapon_magnum_inferno");
		//Client::addMenuItem(%clientId, %curItem++ @ "Particle", "weapon_magnum_slug");
		//Client::addMenuItem(%clientId, %curItem++ @ "Hellfire", "weapon_magnum_hellfire");
		Client::addMenuItem(%clientId, %curItem++ @ "Kraken", "weapon_magnum_kraken");
		return;
	}
	if (%opt == "weapon_rl")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Isolanth Options", "weapons", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Standard", "weapon_rl_reg");
		Client::addMenuItem(%clientId, %curItem++ @ "Charge", "weapon_rl_charge");
		Client::addMenuItem(%clientId, %curItem++ @ "Spread", "weapon_rl_spread");
		Client::addMenuItem(%clientId, %curItem++ @ "Force", "weapon_rl_force");
		Client::addMenuItem(%clientId, %curItem++ @ "Lash", "weapon_rl_lash");
		Client::addMenuItem(%clientId, %curItem++ @ "Terror", "weapon_rl_terror");
		Client::addMenuItem(%clientId, %curItem++ @ "Scorch", "weapon_rl_scorch");
		return;
	}
	if (%opt == "weapon_erl")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Eldar Rocket Launcher Options", "weapons", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Plasma", "weapon_erl_plasma");
		Client::addMenuItem(%clientId, %curItem++ @ "Plague", "weapon_erl_plague");
		Client::addMenuItem(%clientId, %curItem++ @ "Krak", "weapon_erl_krak");
		return;
	}
	if (%opt == "weapon_vulcan")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "StormBolter Options", "weapons", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Standard", "weapon_vulcan_reg");
		//Client::addMenuItem(%clientId, %curItem++ @ "Inferno", "weapon_vulcan_inferno");
		//Client::addMenuItem(%clientId, %curItem++ @ "Particle", "weapon_vulcan_slug");
		//Client::addMenuItem(%clientId, %curItem++ @ "Hellfire", "weapon_vulcan_hellfire");
		Client::addMenuItem(%clientId, %curItem++ @ "Kraken", "weapon_vulcan_kraken");
		return;
	}
	if (%opt == "weapon_rail")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Heavy Bolter Options", "weapons", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Standard", "weapon_rail_reg");
		Client::addMenuItem(%clientId, %curItem++ @ "Inferno", "weapon_rail_inferno");
		//Client::addMenuItem(%clientId, %curItem++ @ "Particle", "weapon_rail_slug");
		Client::addMenuItem(%clientId, %curItem++ @ "Hellfire", "weapon_rail_hellfire");
		Client::addMenuItem(%clientId, %curItem++ @ "Kraken", "weapon_rail_kraken");
		return;
	}
//======================================== 
	if (%opt == "weapon_rail_reg")
	{
		%clientId.HBOpt = "0";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Heavy Bolter set to Standard.\", 3);", 0);
		return;
	}
	if (%opt == "weapon_rail_inferno")
	{
		%clientId.HBOpt = "1";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Heavy Bolter set to Inferno.\", 3);", 0);
		return;
	}
	//if (%opt == "weapon_rail_slug")
	//{
	//	%clientId.HBOpt = "2";
	//	schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Heavy Bolter set to Particle.\", 3);", 0);
	//	return;
	//}
	if (%opt == "weapon_rail_hellfire")
	{
		%clientId.HBOpt = "3";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Heavy Bolter set to Hellfire.\", 3);", 0);
		return;
	}
	if (%opt == "weapon_rail_kraken")
	{
		%clientId.HBOpt = "4";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Heavy Bolter set to Kraken.\", 3);", 0);
		return;
	}
//======================================== 
	if (%opt == "weapon_vulcan_reg")
	{
		%clientId.SBOpt = "0";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Storm Bolter set to Standard.\", 3);", 0);
		return;
	}
	//if (%opt == "weapon_vulcan_inferno")
	//{
	//	%clientId.SBOpt = "1";
	//	schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Storm Bolter set to Inferno.\", 3);", 0);
	//	return;
	//}
	//if (%opt == "weapon_vulcan_slug")
	//{
	//	%clientId.SBOpt = "2";
	//	schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Storm Bolter set to Particle.\", 3);", 0);
	//	return;
	//}
	//if (%opt == "weapon_vulcan_hellfire")
	//{
	//	%clientId.SBOpt = "3";
	//	schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Storm Bolter set to Hellfire.\", 3);", 0);
	//	return;
	//}
	if (%opt == "weapon_vulcan_kraken")
	{
		%clientId.SBOpt = "4";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Storm Bolter set to Kraken.\", 3);", 0);
		return;
	}
//======================================== 
	if (%opt == "weapon_erl_plasma")
	{
		%clientId.ERLOpt = "0";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Eldar Missile Launcher set to Plasma.\", 3);", 0);
		return;
	}
	if (%opt == "weapon_erl_plague")
	{
		%clientId.ERLOpt = "1";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Eldar Missile Launcher set to Plague.\", 3);", 0);
		return;
	}
	if (%opt == "weapon_erl_krak")
	{
		%clientId.ERLOpt = "2";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Eldar Missile Launcher set to Krak.\", 3);", 0);
		return;
	}
//======================================== 
	if (%opt == "weapon_rl_reg")
	{
		%clientId.IsoOpt = "0";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Isolanth set to Standard.\", 3);", 0);
		return;
	}
	if (%opt == "weapon_rl_charge")
	{
		%clientId.IsoOpt = "1";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Isolanth set to Charge.\", 3);", 0);
		return;
	}
	if (%opt == "weapon_rl_spread")
	{
		%clientId.IsoOpt = "2";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Isolanth set to Spread.\", 3);", 0);
		return;
	}
	if (%opt == "weapon_rl_force")
	{
		%clientId.IsoOpt = "3";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Isolanth set to Force.\", 3);", 0);
		return;
	}
	if (%opt == "weapon_rl_lash")
	{
		%clientId.IsoOpt = "4";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Isolanth set to Lash.\", 3);", 0);
		return;
	}
	if (%opt == "weapon_rl_terror")
	{
		%clientId.IsoOpt = "5";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Isolanth set to Terror.\", 3);", 0);
		return;
	}
	if (%opt == "weapon_rl_scorch")
	{
		%clientId.IsoOpt = "6";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Isolanth set to Scorch.\", 3);", 0);
		return;
	}
//======================================== 
	if (%opt == "weapon_magnum_reg")
	{
		%clientId.BOpt = "0";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Bolter set to Standard.\", 3);", 0);
		return;
	}
	//if (%opt == "weapon_magnum_inferno")
	//{
	//	%clientId.BOpt = "1";
	//	schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Bolter set to Inferno.\", 3);", 0);
	//	return;
	//}
	//if (%opt == "weapon_magnum_slug")
	//{
	//	%clientId.BOpt = "2";
	//	schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Bolter set to Particle.\", 3);", 0);
	//	return;
	//}
	//if (%opt == "weapon_magnum_hellfire")
	//{
	//	%clientId.BOpt = "3";
	//	schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Bolter set to Hellfire.\", 3);", 0);
	//	return;
	//}
	if (%opt == "weapon_magnum_kraken")
	{
		%clientId.BOpt = "4";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Bolter set to Kraken.\", 3);", 0);
		return;
	}
//======================================== 
	if (%opt == "weapon_mortar_reg")
	{
		%clientId.MOOpt = "0";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Bolt Pistol set the Standard.\", 3);", 0);
		return;
	}
	//if (%opt == "weapon_mortar_napalm")
	//{
	//	%clientId.MOOpt = "1";
	//	schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Bolt Pistol set to Hellfire.\", 3);", 0);
	//	return;
	//}
	if (%opt == "weapon_mortar_haywire")
	{
		%clientId.MOOpt = "2";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Bolt Pistol set to Kraken.\", 3);", 0);
		return;
	}
//======================================== 
	if (%opt == "weapon_gl_reg")
	{
		%clientId.GLOpt = "0";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Grenade Launcher set to Frag.\", 3);", 0);
		return;
	}
	if (%opt == "weapon_gl_haywire")
	{
		%clientId.GLOpt = "1";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Grenade Launcher set to Haywire.\", 3);", 0);
		return;
	}
	if (%opt == "weapon_gl_hellfire")
	{
		%clientId.GLOpt = "2";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Grenade Launcher set to MIRV.\", 3);", 0);
		return;
	}
	if (%opt == "weapon_gl_plasma")
	{
		%clientId.GLOpt = "3";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Grenade Launcher set to Plasma.\", 3);", 0);
		return;
	}
	if (%opt == "weapon_gl_krak")
	{
		%clientId.GLOpt = "4";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Grenade Launcher set to Krak.\", 3);", 0);
		return;
	}
	if (%opt == "weapon_gl_inferno")
	{
		%clientId.GLOpt = "5";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Grenade Launcher set to Inferno.\", 3);", 0);
		return;
	}
}