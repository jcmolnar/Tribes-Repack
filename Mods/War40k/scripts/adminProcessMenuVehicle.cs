function processMenuVehicle(%clientId, %opt)
{
	if (%opt == "vehicle_vyp")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Vyper Weapons", "vehicle", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Shuriken Catapult", "veh_vy_vulcan");
		Client::addMenuItem(%clientId, %curItem++ @ "Plasma Missile", "veh_vy_plasma");
		Client::addMenuItem(%clientId, %curItem++ @ "Fusion Charge", "veh_vy_fusion");
		return;
	}
	if (%opt == "vehicle_land")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Landspeeder Weapons", "vehicle", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Assault Cannon", "veh_land_chain");
		Client::addMenuItem(%clientId, %curItem++ @ "Heavy Flamer", "veh_land_flame");
		Client::addMenuItem(%clientId, %curItem++ @ "Heavy Melta", "veh_land_melta");
		return;
	}
	if (%opt == "veh_vy_vulcan")
	{
		%clientId.vvOpt = "0";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Vyper will now come with Vulcan Cannon.\", 3);", 0);
		return;
	}
	if (%opt == "veh_vy_plasma")
	{
		%clientId.vvOpt = "1";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Vyper will now come with Plasma Missile.\", 3);", 0);
		return;
	}
	if (%opt == "veh_vy_fusion")
	{
		%clientId.vvOpt = "2";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Vyper will now come with Fusion Charge.\", 3);", 0);
		return;
	}
	if (%opt == "veh_land_chain")
	{
		%clientId.vlOpt = "0";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Landspeeder will now come with Assault Cannon.\", 3);", 0);
		return;
	}
	if (%opt == "veh_land_flame")
	{
		%clientId.vlOpt = "1";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Landspeeder will now come with Heavy Flamer.\", 3);", 0);
		return;
	}
	if (%opt == "veh_land_melta")
	{
		%clientId.vlOpt = "2";
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Landspeeder will now come with Heavy Melta.\", 3);", 0);
		return;
	}
}