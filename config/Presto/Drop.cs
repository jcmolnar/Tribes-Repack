// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// Drop.CS								Presto, March '99 
//
//	A new menu for choosing what you want to drop.
//
//	DROP.CS is an example of using my generic Menu code, and I haven't
//	spent much time polishing it.  It could be cooler with a little more
//	work.
//
// >>>>>>>	A key is bound to "Menu::Display(menuDrop);" by PrestoPrefs.cs
//
//	I was kind of sick of switching weapons around and remembering a whole
//	bunch of different ctrlkeys to drop different items.  So I wrote this
//	menu to aid in the process.  Let's say you bind the menu to the D
//	key.  Then you can just hit
//		D, M		to drop the mortar
//		D, A, M	to drop mortar ammo
//		D, B		to drop the blaster
//		D, F		to drop the flag
//		D, K		to drop your pack
//					(P conflicted with plasma gun)
//		etc.
//
//	It also demonstrates Menu enable states:  you won't get menu choices for
//	weapons you don't have.  (I haven't done this checking for packs or flags
//	yet, though)
//
//	I'm thinking that the other mod which would be cool to do would be to let
//	you request particular kinds of ammo.  For instance,
//
//		Menu::AddChoice(menuRequestAmmo, "mMortar", "Say::Team(needAmmo, 
//			\"Does anyone have any mortar ammo?\"");
//
//	etc.
//
// ---------------------------------------------------------------------------
Include("presto\\Menu.cs");
Include("presto\\Inventory.cs");

function DropCount(%item, %count) {
	%item = $Inv::[%item];
	if (%item == "")
		return;

	if (%count <= 0)
		%count = 1;
	if (%count > GetItemCount($Inv::Name[%item]))
		%count = GetItemCount($Inv::Name[%item]);

	for (%i = 0; %i < %count; %i++)
		drop($Inv::Name[%item]);
	}
function DropAmmoCount(%weapon, %count) {
	%weapon = $Inv::[%weapon];
	if (%weapon == "")
		return;

	%ammo = $Inv::Ammo[%weapon];
	if (%ammo == "")
		return;

	%ammo = $Inv::[%ammo];
	if (%ammo == "")
		return;

	DropCount(%ammo, %count);
	}
function HasItem(%item) {
	%item = $Inv::[%item];
	if (%item == "")
		return;

	return GetItemCount($Inv::Name[%item]) != 0;
	}
function HasAmmo(%weapon) {
	%weapon = $Inv::[%weapon];
	if (%weapon == "")
		return false;

	%ammo = $Inv::Ammo[%weapon];
	if (%ammo == "")
		return false;

	%ammo = $Inv::[%ammo];
	if (%ammo == "")
		return false;

	return GetItemCount($Inv::Name[%ammo]) != 0;
	}

function InitDropMenu() {
	Menu::SetEnabled(menuDrop, 2, getMountedItem(2) != -1);
	Menu::SetEnabled(menuDrop, 3, getMountedItem(1) != -1);
	Menu::SetEnabled(menuDrop, 4, getMountedItem(0) != -1);
	Menu::SetEnabled(menuDrop, 5, HasItem(Blaster));
	Menu::SetEnabled(menuDrop, 6, HasItem(Chaingun));
	Menu::SetEnabled(menuDrop, 7, HasItem(Disc_Launcher));
	Menu::SetEnabled(menuDrop, 8, HasItem(Energy_Rifle));
	Menu::SetEnabled(menuDrop, 9, HasItem(Grenade_Launcher));
	Menu::SetEnabled(menuDrop, 10, HasItem(Laser_Rifle));
	Menu::SetEnabled(menuDrop, 11, HasItem(Mortar));
	Menu::SetEnabled(menuDrop, 12, HasItem(Plasma_Gun));
	Menu::SetEnabled(menuDrop, 13, HasItem(Targeting_Laser));

	$DropCount = 0;
	}
Menu::New(menuDrop, "Drop", "InitDropMenu();");
 Menu::AddChoice(menuDrop, "aAmmo...", "Menu::Display(menuDropAmmo);");
 Menu::AddChoice(menuDrop, "iItem...", "Menu::Display(menuDropItem);");
 Menu::AddChoice(menuDrop, "fFlag", "drop(Flag);");
 Menu::AddChoice(menuDrop, "kCurrent pack", "drop(BackPack);");
 Menu::AddChoice(menuDrop, "wCurrent weapon", "drop(Weapon);");
 Menu::AddChoice(menuDrop, "bBlaster", "DropCount(Blaster);");
 Menu::AddChoice(menuDrop, "cChaingun", "DropCount(Chaingun);");
 Menu::AddChoice(menuDrop, "dDisc Launcher", "DropCount(Disc_Launcher);");
 Menu::AddChoice(menuDrop, "eELF Gun", "DropCount(ELF_Gun);");
 Menu::AddChoice(menuDrop, "gGrenade Launcher", "DropCount(Grenade_Launcher);");
 Menu::AddChoice(menuDrop, "lLaser Rifle", "DropCount(Laser_Rifle);");
 Menu::AddChoice(menuDrop, "mMortar", "DropCount(Mortar);");
 Menu::AddChoice(menuDrop, "pPlasma Gun", "DropCount(Plasma_Gun);");
 Menu::AddChoice(menuDrop, "tTargeting Laser", "DropCount(Targeting_Laser);");

function InitDropAmmoMenu() {
	Menu::SetEnabled(menuDropAmmo, 1, HasAmmo(Chaingun));
	Menu::SetEnabled(menuDropAmmo, 2, HasAmmo(Disc_Launcher));
	Menu::SetEnabled(menuDropAmmo, 3, HasAmmo(Grenade_Launcher));
	Menu::SetEnabled(menuDropAmmo, 4, HasAmmo(Mortar));
	Menu::SetEnabled(menuDropAmmo, 5, HasAmmo(Plasma_Gun));

	$DropCount++;
	}
Menu::New(menuDropAmmo, "Drop ammo for","InitDropAmmoMenu();");
 Menu::AddChoice(menuDropAmmo, "wCurrent weapon", "Inv::Drop(Ammo,$DropCount);");
 Menu::AddChoice(menuDropAmmo, "cChaingun", "DropAmmoCount(Chaingun,$DropCount);");
 Menu::AddChoice(menuDropAmmo, "dDisc Launcher", "DropAmmoCount(Disc_Launcher,$DropCount);");
 Menu::AddChoice(menuDropAmmo, "gGrenade Launcher", "DropAmmoCount(Grenade_Launcher,$DropCount);");
 Menu::AddChoice(menuDropAmmo, "mMortar", "DropAmmoCount(Mortar,$DropCount);");
 Menu::AddChoice(menuDropAmmo, "pPlasmaGun", "DropAmmoCount(Plasma_Gun,$DropCount);");
 Menu::AddChoice(menuDropAmmo, "a", "Menu::Display(menuDropAmmo);");

function InitDropItemMenu() {
	Menu::SetEnabled(menuDropItem, 0, HasItem(Beacon));
	Menu::SetEnabled(menuDropItem, 1, getMountedItem(2) != -1);
	Menu::SetEnabled(menuDropItem, 2, HasItem(Grenade));
	Menu::SetEnabled(menuDropItem, 3, getMountedItem(1) != -1);
	Menu::SetEnabled(menuDropItem, 4, HasItem(Mine));
	Menu::SetEnabled(menuDropItem, 5, HasItem(Repair_Kit));

	$DropCount++;
	}
Menu::New(menuDropItem, "Drop item","InitDropItemMenu();");
 Menu::AddChoice(menuDropItem, "bBeacons", "DropCount(Beacon,$DropCount);");
 Menu::AddChoice(menuDropItem, "fFlag", "drop(Flag);");
 Menu::AddChoice(menuDropItem, "gGrenades", "DropCount(Grenade,$DropCount);");
 Menu::AddChoice(menuDropItem, "kCurrent pack", "drop(BackPack);");
 Menu::AddChoice(menuDropItem, "mMines", "DropCount(Mine,$DropCount);");
 Menu::AddChoice(menuDropItem, "rRepair Kit", "DropCount(Repair_Kit,$DropCount);");
 Menu::AddChoice(menuDropItem, "i", "Menu::Display(menuDropItem);");
// ---------------------------------------------------------------------------