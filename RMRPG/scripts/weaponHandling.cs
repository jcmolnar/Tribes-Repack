
function CreateWeaponCyclingTables() {

	return; // not used anymore

	%n = NEWgetNumItems();
	%counter = 0;
	for (%i = 0; %i < %n; %i++)
	{
		%item = NEWgetItemName(%i);
		if($ItemData[%item, className] == "Weapon")
		{
			if(%counter > 0)
				$NextWeapon[%lastitem] = %item;
			else
				%firstitem = %item;

			%lastitem = %item;
			%counter++;
		}
	}
	$NextWeapon[%lastitem] = %firstitem;

	for(%i = Dummy; $NextWeapon[%i] != Dummy; %i = $NextWeapon[%i])
	      $PrevWeapon[$NextWeapon[%i]] = %i;
	$PrevWeapon[$NextWeapon[%i]] = %i;

	echo("CreateWeaponCyclingTables Completed!");
}

function RPGmountItem(%player, %item, %slot) {

	%Client = Player::getClient(%player);

	%time =  getIntegerTime(true) >> 5;
	if((%time - %Client.lastRPGMountTime) > 0.2)
		%Client.lastRPGMountTime = %time;
	else
		return;

	if(SkillCanUse(%Client, %item, true)) {

		%weapon = $ItemData[%item, shape];

		if(%weapon == "NULL") {
			echo("Error: RPGmountItem(); %item "@%item@" shape == NULL --  This should not happen!");
			return false;
		}

		$ClientData[%Client, UsingWeapon] = %item;

		if(!playSound($ItemData[%item, ASound], GameBase::getPosition(%Client)))
			echo("SoundData "@$ItemData[%item, ASound]@" undefined (RPGmountItem)");

		%text = "<jc><f0>Using a<f1> "@$ItemData[%item, Name]@".";
		%atk = GetWord($ItemData[%item, svar], 1);
		if(%atk > 0)
			%text = %text@" <f0>ATK: <f1>"@%atk;

		bottomprint(%Client, %text, 2);
		Player::setItemCount(%player, %weapon, 1);

		Player::mountItem(%Client, %weapon, %slot);

		return true;
	}
	else
		return false;
}

function remoteNextWeapon(%Client) {
	remoteChangeWeapon(%Client, "Next");
}

function remotePrevWeapon(%Client) {
	remoteChangeWeapon(%Client, "Prev");
}

function remoteChangeWeapon(%Client, %NextOrPrev) {

	if(IsDead(%Client))
		return;

	%time =  getIntegerTime(true) >> 5;
	if((%time - %Client.lastMountTime) > 1)
		%Client.lastMountTime = %time;
	else
		return;

	%item = $ClientData[%Client, UsingWeapon];
	%shape = $ItemData[%item, shape];

	%wlist = Client::getItemListByClass(%Client, "Weapon", "ItemList");

	if(%wlist == "")
		return;

	%wpos = FindWord(%wlist, %item);
	%wc = GetWordCount(%wlist);

	if(%NextOrPrev == "Prev")
		%d = -1;
	else
		%d = 1;

	%n = %wpos + %d;

	if(%n == -1)
		%n = %wc;
	else if(%wpos > %wc)
		%n = 0;

	%weapon = getWord(%wlist, %n);	//echo("changeWeapon["@%wlist@"]: d:"@%d@" "@%NextOrPrev@" n:"@%n@" wpos:"@%wpos@" wc:"@%wc@" weapon:"@%weapon@" ");

	for(%i = 0; %i <= %wc+1; %i++) {

		if(isSelectableWeapon(%Client, %weapon)) {

			Item::use(%Client, %weapon);
			%shape = $ItemData[%weapon, shape];

			if(Player::getMountedItem(%Client,$WeaponSlot) == %shape)
				break;
		}
		else {

			%n += %d;

			if(%n == -1)
				%n = %wc;
			else if(%n > %wc)
				%n = 0;

			%weapon = getWord(%wlist, %n);
		}
	}

}

function selectValidWeapon(%Client) { echo("selectValidWeapon called!"); }

function isSelectableWeapon(%Client, %weapon) {

	if(IsDead(%Client) || !$HasLoadedAndSpawned[%Client] || %Client.IsInvalid || !SkillCanUse(%Client, %weapon, false))
		return false;

	if(Client::HasItem(%Client, %weapon)) {
		//%ammo = $ItemData[%weapon, Ammo];
		//if(%ammo == "" || HasAmmoForWeapon(%Client, $ItemData[%weapon, Ammo])) //Client::getItemCount(%Client, %ammo) > 0)
			return true;
	}
	return false;
}

ItemData Weapon
{
	description = "Weapon";
	showInventory = false;
};

function Weapon::onUse(%player, %item) {}

function Weapon::onFire(%player, %slot) {}
