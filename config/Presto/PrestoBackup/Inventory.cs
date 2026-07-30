// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// Inventory.CS								Presto, March '99 
//
//	A few inventory functions
//
//	Well, don't pay too much attention to this script, because most of it
//	will be going away.  Both XP-Lux and Vis.DS-Writer have made better
//	solutions than I, so I'll be working with them!
//
//	Note that I introduce two new events:
//
//		eventEnterStation
//		eventExitStation
//
//	which trigger when you, well, enter or exit a station! :)
//
// --------------------------------------------------------------------------

function Inventory::onClientMessage(%client, %message) {
	if (%client != 0)
		return;

	if (%message == "Station Access On") {
		Event::Trigger(eventEnterStation);
		return;
		}
	if (%message == "Station Access Off") {
		Event::Trigger(eventExitStation);
		return;
		}
	}
Event::Attach(eventClientMessage, Inventory::onClientMessage, inventory);

// ****** DON'T USE FUNCTIONS BELOW THIS POINT! ********
// I recommend using writer's script.  If you don't already have it, a version
// is also included here in my pack.  This script is by Vis-DS.Writer and you
// can find his home page (and awesome scripts) at
//		http://www.videon.wave.ca/~writer/tribes/index.html
//
if (Include("writer\\inventory_table.cs") == error) {
	echo("Ignore the invalid script error above.");
	Include("presto\\writer\\inventory_table.cs");
	}
// ****** DON'T USE FUNCTIONS BELOW THIS POINT! ********

function Inv::SetName(%item, %name) {
	$InvList::itemName::[%item] = %name;
	}
function Inv::SetModName(%item, %mod, %name) {
	$InvList::modName::[%item, %mod] = %name;
	}
function Inv::GetName(%item) {
	return $InvList::itemName::[%item];
	}
function Inv::GetModName(%item, %mod) {
	if (%mod == "")
		%mod = $Mod::current;
	return $InvList::modName::[%item, %mod];
	}
function Inv::GetModNum(%item, %mod) {
	if (%mod == "")
		%mod = $Mod::current;
	return $InvList::numInMod::[%item, %mod];
	}
function Inv::GetCount(%item) {
	return getItemCount(Inv::GetModName(%item));
	}
function Inv::GetAmmoItem(%item) {
	return $InvList::ammo::[%item];
	}

function Inv::New(%item, %name, %type) {	
	if (%type == "")
		%type = "misc";
	Inv::SetName(%item, %name);
	$InvList::type::[%item] = %type;
	}
function Mod::AddItem(%mod, %num, %item) {
	$InvList::numInMod::[%item, %mod] = %num;
	$Mod::item::[%mod, %num] = %item;
	Inv::SetModName(%item, %mod, Inv::GetName(%item));
	}

function Inv::NewWeapon(%item, %name) {
	Inv::New(%item, %name, "weapon");
	}
function Inv::NewAmmo(%item, %name, %itemWeapon) {
	Inv::New(%item, %name, "ammo");
	$InvList::ammo::[%itemWeapon] = %item;
	$InvList::weapon::[%item] = %itemWeapon;
	}
function Inv::NewPack(%item, %name) {
	Inv::New(%item, %name, "pack");
	}
function Inv::NewArmor(%item, %name) {
	Inv::New(%item, %name, "armor");
	}
function Inv::NewDeployable(%item, %name) {
	Inv::New(%item, %name, "deployable");
	}
function Inv::NewMisc(%item, %name) {
	Inv::New(%item, %name, "misc");
	}
function Inv::NewVehicle(%item, %name) {
	Inv::New(%item, %name, "vehicle");
	}

function Mod::New(%mod, %modBase) {
	$Mod::base::[%mod] = %modBase;
	}
function Mod::GetFreeItems(%mod, %freeStr) {
	if (%mod == "")
		%mod = $Mod::current;
	return $Mod::free::[%mod];
	}
function Mod::SetFreeItems(%mod, %freeStr) {
	$Mod::free::[%mod] = %freeStr;
	}

function Inv::PerformByNum(%action, %weapon) {
	%num = Inv::GetModNum(%weapon);
	if (%num != "")
		remoteEval(2048, %action, %num);
	}
function Inv::Drop(%weapon) {
	Inv::PerformByNum(dropItem, %weapon);
	}
function Inv::Use(%weapon) {
	Inv::PerformByNum(useItem, %weapon);
	}
function Inv::Sell(%weapon) {
	Inv::PerformByNum(sellItem, %weapon);
	}
function Inv::DropAmmo(%weapon) {
	Inv::Drop(Inv::GetAmmoItem(%weapon));
	}




Mod::New(base, "");

// ---------------------------------------------------------------
// ARMOR
Inv::NewArmor(armorLight, "Light Armor");
Mod::AddItem(base, 2, armorLight);

Inv::NewArmor(armorMedium, "Medium Armor");
Mod::AddItem(base, 3, armorMedium);

Inv::NewArmor(armorHeavy, "Heavy Armor");
Mod::AddItem(base, 4, armorHeavy);

// ---------------------------------------------------------------
// WEAPON
Inv::NewWeapon(wpnBlaster, "Blaster");
Mod::AddItem(base, 11, wpnBlaster);

Inv::NewWeapon(wpnChaingun, "Chaingun");
Inv::NewAmmo(ammoChaingun, "Bullet", wpnChaingun);
Mod::AddItem(base, 12, ammoChaingun);
Mod::AddItem(base, 13, wpnChaingun);

Inv::NewWeapon(wpnPlasmaGun, "Plasma Gun");
Inv::NewAmmo(ammoPlasmaGun, "Plasma Bolt", wpnPlasmaGun);
Mod::AddItem(base, 14, ammoPlasmaGun);
Mod::AddItem(base, 15, wpnPlasmaGun);

Inv::NewWeapon(wpnGrenadeLauncher, "Grenade Launcher");
Inv::NewAmmo(ammoGrenadeLauncher, "Grenade Ammo", wpnGrenadeLauncher);
Mod::AddItem(base, 16, ammoGrenadeLauncher);
Mod::AddItem(base, 17, wpnGrenadeLauncher);

Inv::NewWeapon(wpnMortar, "Mortar");
Inv::NewAmmo(ammoMortar, "Mortar Ammo", wpnMortar);
Mod::AddItem(base, 18, ammoMortar);
Mod::AddItem(base, 19, wpnMortar);

Inv::NewWeapon(wpnDiscLauncher, "Disc Launcher");
Inv::NewAmmo(ammoDiscLauncher, "Disc", wpnDiscLauncher);
Mod::AddItem(base, 20, ammoDiscLauncher);
Mod::AddItem(base, 21, wpnDiscLauncher);

Inv::NewWeapon(wpnLaserRifle, "Laser Rifle");
Mod::AddItem(base, 22, wpnLaserRifle);

Inv::NewWeapon(wpnTargetingLaser, "Targeting Laser");
Mod::AddItem(base, 23, wpnTargetingLaser);

Inv::NewWeapon(wpnEnergyRifle, "ELF Gun");
Mod::AddItem(base, 24, wpnEnergyRifle);

Inv::NewWeapon(wpnRepairGun, "Repair Gun");
Mod::AddItem(base, 25, wpnRepairGun);

// ---------------------------------------------------------------
// PACK
Inv::NewPack(packEnergy, "Energy Pack");
Mod::AddItem(base, 29, packEnergy);

Inv::NewPack(packRepair, "Repair Pack");
Mod::AddItem(base, 30, packRepair);

Inv::NewPack(packShield, "Shield Pack");
Mod::AddItem(base, 31, packShield);

Inv::NewPack(packSensorJammer, "Sensor Jammer Pack");
Mod::AddItem(base, 32, packSensorJammer);

Inv::NewPack(packAmmo, "Ammo Pack");
Mod::AddItem(base, 34, packAmmo);

// ---------------------------------------------------------------
// DEPLOYABLE
Inv::NewDeployable(depInvStation, "Inventory Station");
Mod::AddItem(base, 27, depInvStation);

Inv::NewDeployable(depAmmoStation, "Ammo Station");
Mod::AddItem(base, 28, depAmmoStation);

Inv::NewDeployable(depMotionSensor, "Motion Sensor");
Mod::AddItem(base, 33, depMotionSensor);

Inv::NewDeployable(depPulseSensor, "Pulse Sensor");
Mod::AddItem(base, 35, depPulseSensor);

Inv::NewDeployable(depSensorJammer, "Sensor Jammer");
Mod::AddItem(base, 36, depSensorJammer);

Inv::NewDeployable(depCamera, "Camera");
Mod::AddItem(base, 37, depCamera);

Inv::NewDeployable(depTurret, "Turret");
Mod::AddItem(base, 38, depTurret);

// ---------------------------------------------------------------
// MISC
Inv::NewMisc(itemRepairKit, "Repair Kit");
Mod::AddItem(base, 39, itemRepairKit);

Inv::NewMisc(itemMine, "Mine");
Mod::AddItem(base, 40, itemMine);

Inv::NewMisc(itemGrenade, "Grenade");
Mod::AddItem(base, 41, itemGrenade);

Inv::NewMisc(itemBeacon, "Beacon");
Mod::AddItem(base, 42, itemBeacon);

Inv::NewMisc(miscFlag, "Flag");
Mod::AddItem(base, 0, miscFlag);

Inv::NewMisc(miscCurrentWeapon, "Weapon");
Mod::AddItem(base, 8, miscCurrentWeapon);
Inv::NewMisc(miscCurrentAmmo, "Ammo");
Mod::AddItem(base, 10, miscCurrentAmmo);
Inv::NewMisc(miscCurrentBackpack, "Backpack");
Mod::AddItem(base, 26, miscCurrentBackpack);

// ---------------------------------------------------------------
// VEHICLE
Inv::NewVehicle(vehScout, "Scout");
Mod::AddItem(base, 5, vehScout);
Inv::NewVehicle(vehLPC, "LPC");
Mod::AddItem(base, 6, vehLPC);
Inv::NewVehicle(vehHPC, "HPC");
Mod::AddItem(base, 7, vehHPC);

Mod::SetFreeItems(base, " wpnTargetingLaser ammoChaingun ammoMortar ammoGrenadeLauncher ammoDiscLauncher ammoPlasmaGun itemMine itemRepairKit itemGrenade itemBeacon");


function Inventory::onJoinServer() {
	// Fix the base mod
	if ($ServerVersion == "1.0") {
		Inv::SetModName(ammoGrenadeLauncher, base, "GrenadeAmmo");
		Inv::SetModName(ammoMortar, base, "MortarAmmo");
		}
	 else {
		Inv::SetModName(ammoGrenadeLauncher, base, "Grenade Ammo");
		Inv::SetModName(ammoMortar, base, "Mortar Ammo");
		}

	// Figure out the mod
	$Mod::current = $ServerFavoritesKey;	// works for now
	if ($Mod::current == "")
		$Mod::current = "base";
	Event::Trigger(eventDetectMod);
	}
// commented out when I added Writer's version.
Event::Attach(eventConnected, Inventory::onJoinServer, inventory);

