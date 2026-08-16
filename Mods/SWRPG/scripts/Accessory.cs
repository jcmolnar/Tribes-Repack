//When adding a new accessory, follow these steps:
//-(if it has a new accessory type, fill in the stuff here)
//-add the actual itemdata here

//current item method involves having two ItemData's for each item, where one differs from the
//other by category.  One is Accessory, the other is Equipped.

//=========================
//  $SpecialVar list:
//=========================
//1:
//2:
//3: MDEF
//4: HP
//5: Mana
//6: ATK
//7: DEF
//8: Internal armor switching variable
//9: Max Weight increase
//10: HP regen
//11: Mana regen	

$SpecialVarDesc[1] = "";
$SpecialVarDesc[2] = "";
$SpecialVarDesc[3] = "FDEF (Force)";
$SpecialVarDesc[4] = "HP";
$SpecialVarDesc[5] = "Energy";
$SpecialVarDesc[6] = "ATK";
$SpecialVarDesc[7] = "DEF";
$SpecialVarDesc[8] = "[Internal]";
$SpecialVarDesc[9] = "Additional Weight Capacity"; //Not sure this works yet.
$SpecialVarDesc[10] = "HP regen";
$SpecialVarDesc[11] = "Energy regen";

$RingAccessoryType = 1;
$BodyAccessoryType = 2;
$BootsAccessoryType = 3;
$BackAccessoryType = 4;
$ShieldAccessoryType = 5;
$TalismanAccessoryType = 6;
$SwordAccessoryType = 7;
$AxeAccessoryType = 8;
$PolearmAccessoryType = 9;
$BludgeonAccessoryType = 10;
$RangedAccessoryType = 11;
$ProjectileAccessoryType = 12;
$GrenadeAccessoryType = 13;
$PotionAccessoryType = 14;

$LocationDesc[$RingAccessoryType] = "Ring";
$LocationDesc[$BodyAccessoryType] = "Body";
$LocationDesc[$BootsAccessoryType] = "Feet";
$LocationDesc[$BackAccessoryType] = "Back";
$LocationDesc[$ShieldAccessoryType] = "Shield";
$LocationDesc[$TalismanAccessoryType] = "Talisman";
$LocationDesc[$SwordAccessoryType] = "Sword";
$LocationDesc[$AxeAccessoryType] = "Axe";
$LocationDesc[$PolearmAccessoryType] = "Polearm";
$LocationDesc[$BludgeonAccessoryType] = "Bludgeon";
$LocationDesc[$RangedAccessoryType] = "Ranged";
$LocationDesc[$ProjectileAccessoryType] = "Projectile";
$LocationDesc[$GrenadeAccessoryType] = "Thrown";

$maxAccessory[$RingAccessoryType] = 2;
$maxAccessory[$BodyAccessoryType] = 1;
$maxAccessory[$BootsAccessoryType] = 1;
$maxAccessory[$BackAccessoryType] = 1;
$maxAccessory[$ShieldAccessoryType] = 1;
$maxAccessory[$TalismanAccessoryType] = 1;

//these are used for $AccessoryVar
$AccessoryType = 1;			//(used in item.cs)
$SpecialVar = 2;				//(used in player.cs)
$Weight = 3;				//(used in rpgfunk.cs)
$ShopIndex = 4;
$MiscInfo = 5;

$HardcodedItemCost[BactaVial] = 15;
$HardcodedItemCost[BactaCanister] = 100;
$HardcodedItemCost[KoltoVial] = 15;
$HardcodedItemCost[KoltoCanister] = 100;

$HardcodedItemCost[Tent] = 4000;
$HardcodedItemCost[VehicleBeacon] = 1000000;
$HardcodedItemCost[RecallBeacon] = 1;
$HardcodedItemCost[PDA] = 500;
$HardcodedItemCost[Glowrod] = 10;
$HardcodedItemCost[BreathingDevice] = 1000;
$HardcodedItemCost[CheetaursPaws] = 3500;
$HardcodedItemCost[AntigravityBoots] = 8000;
$HardcodedItemCost[JetPack] = 45000;

$HardcodedItemCost[BlackStatue] = 180;
$HardcodedItemCost[EnchantedStone] = 2450;
$HardcodedItemCost[SkeletonBone] = 5860;
$HardcodedItemCost[Parchment] = 1000000;
$HardcodedItemCost[Holocron] = 1000000;
$HardcodedItemCost[JediHolocron] = 1000000;
$HardcodedItemCost[SithHolocron] = 1000000;
$HardcodedItemCost[DragonScale] = 245310;
$HardcodedItemCost[BadgeOfFriendship] = 1;
$HardcodedItemCost[BadgeOfLoyalty] = 1;
$HardcodedItemCost[BadgeOfHonor] = 1;
$HardcodedItemCost[BadgeOfReverence] = 1;

function GenerateAllShieldCosts()
{
	dbecho($dbechoMode, "GenerateAllShieldCosts()");

	$ItemCost[GunganShield] = GenerateItemCost(GunganShield);
	$ItemCost[MandalorianShield] = GenerateItemCost(MandalorianShield);
	$ItemCost[VerpineShield] = GenerateItemCost(VerpineShield);
	$ItemCost[EchaniShield] = GenerateItemCost(EchaniShield);
}

//=====================
// ACCESSORY FUNCTIONS
//=====================

function GetAccessoryVar(%item, %type)
{
	dbecho($dbechoMode, "GetAccessoryVar(" @ %item @ ", " @ %type @ ")");

	%nitem = getCroppedItem(%item);

	return $AccessoryVar[%nitem, %type];
}

function getCroppedItem2(%item)
{
	if(%item.className == "Equipped")
		%x = 1;
	return String::getSubStr(%item, 0, String::len(%b)-%x);
}

function getCroppedItem(%item)
{
	dbecho($dbechoMode, "getCroppedItem(" @ %item @ ")");

	%zitem = %item @ "xx";
	%p = String::findSubStr(%zitem, "0xx");
	if(%p != -1)
		%nitem = String::getSubStr(%item, 0, %p);
	else
		%nitem = %item;

	return %nitem;
}

function GetAccessoryList(%clientId, %type, %filter)
{
	dbecho($dbechoMode, "GetAccessoryList(" @ %clientId @ ", " @ %type @ ", " @ %filter @ ")");

	if(IsDead(%clientId) || !fetchData(%clientId, "HasLoadedAndSpawned") || %clientId.IsInvalid || %clientId.choosingGroup || %clientId.choosingRace || %clientId.choosingClass)
		return "";

	%list = "";
	%max = getNumItems();
	for(%i = 0; %i < %max; %i++)
	{
		%count = Player::getItemCount(%clientId, %i);

		if(%count)
		{
			%item = getItemData(%i);

			%flag = "";
			if(%type == 1)
			{
				if(%item.className == "Accessory")
					%flag = True;
			}
			else if(%type == 2)
			{
				if(%item.className == "Equipped")
					%flag = True;
			}
			else if(%type == 3)
			{
				if(%item.className == "Accessory" || %item.className == "Equipped")
					%flag = True;
			}
			else if(%type == 4)
			{
				if(%item.className == "Equipped" || %item.className == "Weapon" || %item.className == "Backpack")
				{
					if(%item.className == "Weapon")
					{
						if(Player::getMountedItem(%clientId, $WeaponSlot) == %item)
							%flag = True;
					}
					else if(%item.className == "Backpack")
					{
						if(Player::getMountedItem(%clientId, $BackpackSlot) == %item)
							%flag = True;
					}
					else
						%flag = True;
				}
			}
			else if(%type == 5)
			{
				if($AccessoryVar[%item, $AccessoryType] == $SwordAccessoryType)
					%flag = True;
			}
			else if(%type == 6)
			{
				if($AccessoryVar[%item, $AccessoryType] == $AxeAccessoryType)
					%flag = True;
			}
			else if(%type == 7)
			{
				if($AccessoryVar[%item, $AccessoryType] == $PolearmAccessoryType)
					%flag = True;
			}
			else if(%type == 8)
			{
				if($AccessoryVar[%item, $AccessoryType] == $BludgeonAccessoryType)
					%flag = True;
			}
			else if(%type == 9)
			{
				if($AccessoryVar[%item, $AccessoryType] == $RangedAccessoryType)
					%flag = True;
			}
			else if(%type == 10)
			{
				if($AccessoryVar[%item, $AccessoryType] == $ProjectileAccessoryType)
					%flag = True;
			}
			else if(%type == 11)
			{
				if($AccessoryVar[%item, $AccessoryType] == $GrenadeAccessoryType)
					%flag = True;
			}
			else if(%type == 12)
			{
				if($AccessoryVar[%item, $AccessoryType] == $PotionAccessoryType)
					%flag = True;
			}
			else if(%type == -1)
				%flag = True;

			if(%flag)
			{
				if(%filter != -1)
				{
					%flag2 = "";
					%av = GetAccessoryVar(%item, $SpecialVar);
					for(%j = 0; GetWord(%av, %j) != -1; %j+=2)
					{
						%w = GetWord(%av, %j);
						if(String::findSubStr(%filter, %w) != -1)
							%flag2 = True;
					}
				}
				if(%filter == -1 || %flag2)
					%list = %list @ %item @ " ";
			}
		}
	}
	return %list;
}

function AddPoints(%clientId, %char)
{
	dbecho($dbechoMode, "AddPoints(" @ %clientId @ ", " @ %char @ ")");

	%add = 0;
	%list = GetAccessoryList(%clientId, 4, %char);
	for(%i = 0; GetWord(%list, %i) != -1; %i++)
	{
		%w = GetWord(%list, %i);

		%slot = "";
		if(%w.className == Weapon)
			%slot = $WeaponSlot;
		else if(%w.className == Backpack)
			%slot = $BackpackSlot;

		if(%slot != "")
		{
			if(Player::getMountedItem(%clientId, %slot) == %w)
				%count = 1;
			else
				%count = 0;
		}
		else
			%count = Player::getItemCount(%clientId, %w);

		%tmp = GetAccessoryVar(%w, $SpecialVar);

		for(%j = 0; GetWord(%tmp, %j) != -1; %j+=2)
		{
			%e = GetWord(%tmp, %j);
			if(String::findSubStr(%char, %e) != -1)
				%add += GetWord(%tmp, %j+1) * %count;
		}
	}
	return %add;
}

function AddItemSpecificPoints(%item, %char)
{
	dbecho($dbechoMode, "AddItemSpecificPoints(" @ %item @ ", " @ %char @ ")");

	%tmp = GetAccessoryVar(%item, $SpecialVar);

	for(%j = 0; GetWord(%tmp, %j) != -1; %j+=2)
	{
		%e = GetWord(%tmp, %j);
		if(%e == %char)
		{
			%info = GetWord(%tmp, %j+1);
			break;
		}
	}

	return %info;
}

function WhatSpecialVars(%thing)
{
	dbecho($dbechoMode, "WhatSpecialVars(" @ %thing @ ")");

	%tmp = GetAccessoryVar(%thing, $SpecialVar);

	%t = "";
	for(%i = 0; GetWord(%tmp, %i) != -1; %i+=2)
	{
		%s = GetWord(%tmp, %i);
		%n = GetWord(%tmp, %i+1);

		%t = %t @ $SpecialVarDesc[%s] @ ": " @ %n @ ", ";
	}
	if(%t == "")
		%t = "None";
	else
		%t = String::getSubStr(%t, 0, String::len(%t)-2);
	
	return %t;
}

function NullItemList(%clientId, %type, %msgcolor, %msg)
{
	dbecho($dbechoMode, "NullItemList(" @ %clientId @ ", " @ %type @ ", " @ %msgcolor @ ", " @ %msg @ ")");

	for(%z = 1; $ItemList[%type, %z] != ""; %z++)
	{
		%item = $ItemList[%type, %z];
		if(Player::getItemCount(%clientId, %item))
		{
			Player::setItemCount(%clientId, %item, 0);

			%newmsg = nsprintf(%msg, %item.description);
			Client::sendMessage(%clientId, %msgcolor, %newmsg);
		}
	}
}

function GetCurrentlyWearingArmor(%clientId)
{
	dbecho($dbechoMode, "GetCurrentlyWearingArmor(" @ %clientId @ ")");

	//the $ArmorList is present only for this function so far, in order to speed things up and not have to cycle thru
	//each and every item in the game
	for(%i = 1; $ArmorList[%i] != ""; %i++)
	{
		if(Player::getItemCount(%clientId, $ArmorList[%i] @ "0"))
			return $ArmorList[%i];
	}
	return "";
}

//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//   POTIONS
//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$AccessoryVar[BactaVial, $AccessoryType] = $PotionAccessoryType;
$AccessoryVar[BactaVial, $Weight] = 3;
$AccessoryVar[BactaVial, $MiscInfo] = "A a small vial of Bacta that heals 15 HP";
ItemData BactaVial
{
	description = "Bacta Vial";
	shapeFile = "armorPatch";
	heading = "eMiscellany";
	className = "Accessory";
	shadowDetailMask = 4;
	price = 0;
};
function BactaVial::onUse(%player,%item)
{
	%clientId = Player::getClient(%player);

	Player::decItemCount(%player,%item);
	%hp = fetchData(%clientId, "HP");
	refreshHP(%clientId, -0.15);
	refreshAll(%clientId);

	if(fetchData(%clientId, "HP") != %hp)
		UseSkill(%clientId, $SkillHealing, True, True);
}

$AccessoryVar[BactaCanister, $AccessoryType] = $PotionAccessoryType;
$AccessoryVar[BactaCanister, $Weight] = 6;
$AccessoryVar[BactaCanister, $MiscInfo] = "A personal size bacta canister that heals 60 HP";
ItemData BactaCanister
{
	description = "Bacta Canister";
	shapeFile = "armorKit";
	heading = "eMiscellany";
	className = "Accessory";
	shadowDetailMask = 4;
	price = 0;
};
function BactaCanister::onUse(%player,%item)
{
	%clientId = Player::getClient(%player);

	Player::decItemCount(%player,%item);
	%hp = fetchData(%clientId, "HP");
	refreshHP(%clientId, -0.6);
	refreshAll(%clientId);

	if(fetchData(%clientId, "HP") != %hp)
		UseSkill(%clientId, $SkillHealing, True, True);
}

$AccessoryVar[KoltoVial, $AccessoryType] = $PotionAccessoryType;
$AccessoryVar[KoltoVial, $Weight] = 2;
$AccessoryVar[KoltoVial, $MiscInfo] = "An vial of kolto that provides 16 MP";
ItemData KoltoVial
{
	description = "Kolto Vial";
	shapeFile = "armorPatch";
	heading = "eMiscellany";
	className = "Accessory";
	shadowDetailMask = 4;
	price = 0;
};
function KoltoVial::onUse(%player,%item)
{
	%clientId = Player::getClient(%player);

	Player::decItemCount(%player,%item);
	//refreshMANApct(%clientId, 10);
	refreshMANA(%clientId, -16);
	refreshAll(%clientId);
}

$AccessoryVar[KoltoCanister, $AccessoryType] = $PotionAccessoryType;
$AccessoryVar[KoltoCanister, $Weight] = 5;
$AccessoryVar[KoltoCanister, $MiscInfo] = "A kolto canister that provides 50 MP";
ItemData KoltoCanister
{
	description = "Kolto Canister";
	shapeFile = "armorKit";
	heading = "eMiscellany";
	className = "Accessory";
	shadowDetailMask = 4;
	price = 0;
};
function KoltoCanister::onUse(%player,%item)
{
	%clientId = Player::getClient(%player);

	Player::decItemCount(%player,%item);
	//refreshMANApct(%clientId, 20);
	refreshMANA(%clientId, -50);
	refreshAll(%clientId);
}

//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//   RINGS
//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$RingWeight = 1;

//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//   ARMOR MODIFYING ACCESSORIES
//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$AccessoryVar[CheetaursPaws, $AccessoryType] = $BootsAccessoryType;
$AccessoryVar[CheetaursPaws, $SpecialVar] = "8 1";
$AccessoryVar[CheetaursPaws, $Weight] = 3;
$AccessoryVar[CheetaursPaws, $MiscInfo] = "Cheetaur's Paws increase speed and jump power";

ItemData CheetaursPaws
{
	description = "Cheetaur's Paws";
	className = "Accessory";
	shapeFile = "discammo";

	heading = "eMiscellany";
	price = 0;
};
ItemData CheetaursPaws0
{
	description = "Cheetaur's Paws";
	className = "Equipped";
	shapeFile = "discammo";

	heading = "aArmor";
};

$AccessoryVar[AntigravityBoots, $AccessoryType] = $BootsAccessoryType;
$AccessoryVar[AntigravityBoots, $SpecialVar] = "8 2";
$AccessoryVar[AntigravityBoots, $Weight] = 3;
$AccessoryVar[AntigravityBoots, $MiscInfo] = "Antigravity boots let you float";

ItemData AntigravityBoots
{
	description = "Antigravity Boots";
	className = "Accessory";
	shapeFile = "discammo";

	heading = "eMiscellany";
	price = 0;
};
ItemData AntigravityBoots0
{
	description = "Antigravity Boots";
	className = "Equipped";
	shapeFile = "discammo";

	heading = "aArmor";
};

$AccessoryVar[JetPack, $AccessoryType] = $BootsAccessoryType;
$AccessoryVar[JetPack, $SpecialVar] = "8 3";
$AccessoryVar[JetPack, $Weight] = 3;
$AccessoryVar[JetPack, $MiscInfo] = "Jet Packs let you fly!";

ItemImageData JetPackImage
{
	shapeFile = "mortarpack";
	mountPoint = 2;
	//mountOffset = {-0.2, 0.4, 0.1};
	//mountRotation = {0, 0, 0.4};
};

ItemData JetPack
{
	description = "Jet Pack";
	className = "Accessory";
	shapeFile = "discammo";
	imageType = JetPackImage;

	heading = "eMiscellany";
	price = 0;
};
ItemData JetPack0
{
	description = "Jet Pack";
	className = "Equipped";
	shapeFile = "discammo";

	heading = "aArmor";
};

//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//   BACK PACKS
//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$AccessoryVar[CloakingDevice, $AccessoryType] = $BackAccessoryType;
$AccessoryVar[CloakingDevice, $SpecialVar] = "7 10";
$AccessoryVar[CloakingDevice, $Weight] = 3;
$AccessoryVar[CloakingDevice, $MiscInfo] = "A personal cloaking device.!";

ItemImageData CloakingDeviceImage
{
	shapeFile = "shield_medium";
	mountPoint = 2;
	mountOffset = {-0.2, 0.4, 0.1};
	mountRotation = {0, 0, 0.4};
};

ItemData CloakingDevice
{
	description = "Cloaking Device";
	className = "Accessory";
	shapeFile = "discammo";
	imageType = CloakingDeviceImage;

	heading = "eMiscellany";
	price = 0;
};
ItemData CloakingDevice0
{
	description = "Cloaking Device";
	className = "Equipped";
	shapeFile = "discammo";

	heading = "aArmor";
};

 // See itemevents.cs, in Item::onUse for where these two functions are called.
function CloakingDevice::onEquip(%player, %item)
{
	Client::sendMessage(Player::getClient(%player),0,"Cloaking enabled");
	gameBase::startFadeOut(%player);
}

// NOTE: In *::onUnequip, %item is the xxx0 item, the className = Equipped one.
// It's not the classname = Accessory one. I may change this in the future, if
// necessary, to return the item name without 0.
function CloakingDevice::onUnequip(%player, %item)
{
	Client::sendMessage(Player::getClient(%player),0,"Cloaking disabled");
	gameBase::startFadeIn(%player);
}

//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//   PERSONAL SHIELDS
//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$AccessoryVar[GunganShield, $AccessoryType] = $ShieldAccessoryType;
$AccessoryVar[GunganShield, $SpecialVar] = "7 250";
$AccessoryVar[GunganShield, $Weight] = 16;
$AccessoryVar[GunganShield, $MiscInfo] = "The Gungan Shield is a unique item that provides great defense.";

ItemImageData GunganShieldImage
{
	shapeFile = "shield";
	mountPoint = 2;
	mountOffset = {-0.2, 0.4, 0.1};
	mountRotation = {0, 0, 0.4};
	firstPerson = false;
};
ItemData GunganShield
{
	description = "Gungan Shield";
	className = "Accessory";
	shapeFile = "shield";
	imageType = GunganShieldImage;

	heading = "eMiscellany";
	price = 0;
};
ItemData GunganShield0
{
	description = "Gungan Shield";
	className = "Equipped";
	shapeFile = "shield";

	heading = "aArmor";
};

$AccessoryVar[MandalorianShield, $AccessoryType] = $ShieldAccessoryType;
$AccessoryVar[MandalorianShield, $SpecialVar] = "7 315 3 635";
$AccessoryVar[MandalorianShield, $Weight] = 14;
$AccessoryVar[MandalorianShield, $MiscInfo] = "Mandalorians don't fear melee combat, but anything that absorbs physical damage brings them a step closer to victory, and these forearm shields are a favorite.";

ItemImageData MandalorianShieldImage
{
	shapeFile = "shield_medium";
	mountPoint = 2;
	mountOffset = {-0.2, 0.4, 0.1};
	mountRotation = {0, 0, 0.4};
};
ItemData MandalorianShield
{
	description = "Mandalorian Shield";
	className = "Accessory";
	shapeFile = "shield_medium";
	imageType = MandalorianShieldImage;

	heading = "eMiscellany";
	price = 0;
};
ItemData MandalorianShield0
{
	description = "Mandalorian Shield";
	className = "Equipped";
	shapeFile = "shield_medium";

	heading = "aArmor";
};

$AccessoryVar[VerpineShield, $AccessoryType] = $ShieldAccessoryType;
$AccessoryVar[VerpineShield, $SpecialVar] = "7 315 3 635";
$AccessoryVar[VerpineShield, $Weight] = 14;
$AccessoryVar[VerpineShield, $MiscInfo] = "Though manufactured by the Verpine, these forearm shields are based on highly modified Arkanian designs.";

ItemImageData VerpineShieldImage
{
	shapeFile = "shield_medium";
	mountPoint = 2;
	mountOffset = {-0.2, 0.4, 0.1};
	mountRotation = {0, 0, 0.4};
};
ItemData VerpineShield
{
	description = "Verpine Shield";
	className = "Accessory";
	shapeFile = "shield_medium";
	imageType = VerpineShieldImage;

	heading = "eMiscellany";
	price = 0;
};
ItemData VerpineShield0
{
	description = "Verpine Shield";
	className = "Equipped";
	shapeFile = "shield_medium";

	heading = "aArmor";
};

$AccessoryVar[EchaniShield, $AccessoryType] = $ShieldAccessoryType;
$AccessoryVar[EchaniShield, $SpecialVar] = "7 540 4 210";
$AccessoryVar[EchaniShield, $Weight] = 15;
$AccessoryVar[EchaniShield, $MiscInfo] = "The Echani put much effort into developing a forearm shield that, once activated, would allow a mercenary to close on a blaster-wielding enemy relatively unscathed.";

ItemImageData EchaniShieldImage
{
	shapeFile = "shield";
	mountPoint = 2;
	mountOffset = {-0.2, 0.4, 0.1};
	mountRotation = {0, 0, 0.4};
	firstPerson = false;
};
ItemData EchaniShield
{
	description = "Echani Shield";
	className = "Accessory";
	shapeFile = "shield_large";
	imageType = EchaniShieldImage;

	heading = "eMiscellany";
	price = 0;
};
ItemData EchaniShield0
{
	description = "Echani Shield";
	className = "Equipped";
	shapeFile = "shield_large";

	heading = "aArmor";
};

//============================================================================
// NPCS / Townbots

StaticShapeData MaleHumanTownBot
{
	description = "Male Town Bot";
	className = "TownBot";
	shapeFile = "rpgmalehuman";

	debrisId = defaultDebrisSmall;
	maxDamage = 10000.0;
	visibleToSensor = true;	//thanks Adger!!
	mapFilter = 1;		//thanks Adger!!
};
StaticShapeData FemaleHumanTownBot
{
	description = "Female Town Bot";
	className = "TownBot";
	shapeFile = "lfemalehuman";

	debrisId = defaultDebrisSmall;
	maxDamage = 10000.0;
	visibleToSensor = true;	//thanks Adger!!
	mapFilter = 1;		//thanks Adger!!
};
StaticShapeData MaleRobedTownBot
{
	description = "Male Robed Town Bot";
	className = "TownBot";
	shapeFile = "magemale";
	isTranslucent = true;

	debrisId = defaultDebrisSmall;
	maxDamage = 10000.0;
	visibleToSensor = true;
	mapFilter = 1;
};
StaticShapeData FemaleRobedTownBot
{
	description = "Female Robed Town Bot";
	className = "TownBot";
	shapeFile = "femalemage";
	isTranslucent = true;

	debrisId = defaultDebrisSmall;
	maxDamage = 10000.0;
	visibleToSensor = true;
	mapFilter = 1;
};
StaticShapeData DroidTownBot
{
	description = "Droid Town Bot";
	className = "TownBot";
	shapeFile = "droid";

	debrisId = defaultDebrisSmall;
	maxDamage = 10000.0;
	visibleToSensor = true;
	mapFilter = 1;
};
StaticShapeData TuskenTownBot
{
	description = "Tusken Town Bot";
	className = "TownBot";
	shapeFile = "tuskanraider";

	debrisId = defaultDebrisSmall;
	maxDamage = 10000.0;
	visibleToSensor = true;
	mapFilter = 1;
};
StaticShapeData StormTrooperTownBot
{
	description = "Storm Trooper Town Bot";
	className = "TownBot";
	shapeFile = "StormTrooper";

	debrisId = defaultDebrisSmall;
	maxDamage = 10000.0;
	visibleToSensor = true;
	mapFilter = 1;
};
StaticShapeData WookieeTownBot
{
	description = "Wookiee Town Bot";
	className = "TownBot";
	shapeFile = "chewy";

	debrisId = defaultDebrisSmall;
	maxDamage = 10000.0;
	visibleToSensor = true;
	mapFilter = 1;
};
StaticShapeData RebelTrooperTownBot
{
	description = "Rebel Trooper Town Bot";
	className = "TownBot";
	shapeFile = "Rebeltroop";
	isTranslucent = true;

	debrisId = defaultDebrisSmall;
	maxDamage = 10000.0;
	visibleToSensor = true;
	mapFilter = 1;
};
StaticShapeData RebelPilotTownBot
{
	description = "Rebel Pilot Town Bot";
	className = "TownBot";
	shapeFile = "RebelPilot";
	isTranslucent = true;

	debrisId = defaultDebrisSmall;
	maxDamage = 10000.0;
	visibleToSensor = true;
	mapFilter = 1;
};

StaticShapeData MandalorianTownBot
{
	description = "Mandalorian Town Bot";
	className = "TownBot";
	shapeFile = "bobafett";
	isTranslucent = true;

	debrisId = defaultDebrisSmall;
	maxDamage = 10000.0;
	visibleToSensor = true;
	mapFilter = 1;
};

StaticShapeData RebelPilotTownBot
{
	description = "Rebel Pilot Town Bot";
	className = "TownBot";
	shapeFile = "RebelPilot";
	isTranslucent = true;

	debrisId = defaultDebrisSmall;
	maxDamage = 10000.0;
	visibleToSensor = true;
	mapFilter = 1;
};

StaticShapeData GnollTownBot
{
	description = "Gnoll Alien Town Bot";
	className = "TownBot";
	shapeFile = "marmorgnoll";
	isTranslucent = true;

	debrisId = defaultDebrisSmall;
	maxDamage = 10000.0;
	visibleToSensor = true;
	mapFilter = 1;
};

//===== MISC STUFF ===============================================================

//------------------------
$AccessoryVar[Tent, $Weight] = 40;
$AccessoryVar[Tent, $MiscInfo] = "A tent. Use #camp to set it up, and #uncamp to disassemble it.";

ItemData Tent
{
	description = "Tent";
	shapeFile = "armorKit";
	heading = "eMiscellany";
	className = "Accessory";
	shadowDetailMask = 4;
	price = 0;
};

ItemData Lootbag
{
	description = "Backpack";
	className = "Lootbag";
	shapeFile = "ammo2";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

$AccessoryVar[VehicleBeacon, $Weight] = 4; //When activated, allows you to spawn a vehicle that you have. :D
$AccessoryVar[VehicleBeacon, $MiscInfo] = "When used, allows you to select from the list of vehicles you own and call for one.";
$StealProtectedItem[VehicleBeacon] = True;
ItemData VehicleBeacon
{
	description = "Vehicle Beacon";
	className = "Accessory";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};
$woot = 1;
function VehicleBeacon::onUse(%player,%item)
{
		%clientId = player::getClient(%player);
		if($woot == 1) { storeData(%clientId, "OwnedVehicles", "Xwing 1 Banshee 3 Ywing 2 TieInterceptor 1"); $woot++;}

		%vehicles = fetchData(%clientId, "OwnedVehicles");

		Client::buildMenu(%clientId, "Select a vehicle", "vehbeacon", true);
		Client::addMenuItem(%clientId, "iInformation", "info");
		for(%i = 0; (%word = GetWord(%vehicles, %i)) != -1; %i++)
			if((%count = GetWord(%vehicles, %i++)) > 0)
				Client::addMenuItem(%clientId, %i / 2 + 1 @ %word.description @ " (" @ %count @ ")", %word);
}

function processMenuvehbeacon(%clientId, %opt)
{
	dbecho($dbechoMode, "processMenuvehbeacon(" @ %clientId @ ", " @ %option @ ")");

	//%opt = getWord(%option, 0);
	//%cl = getWord(%option, 1);

	if(%opt == "Awing")
	{
//yourox
	}
	else if(%opt == "Xwing")
	{
//yourox
	}
	else if(%opt == "Ywing")
	{
//yourox
	}
	else if(%opt == "SnowSpeeder")
	{
//yourox
	}
	else if(%opt == "TieBomber")
	{
//yourox
	}
	else if(%opt == "TieFighter")
	{
//yourox
	}
	else if(%opt == "TieInterceptor")
	{
//yourox
	}
	else if(%opt == "Banshee")
	{
//yourox
	}

	if(GameBase::getLOSInfo(client::getOwnedObject(%clientId),10))
	{
		%vehicle = newObject("",flier,%opt,true);
		Gamebase::setMapName(%vehicle,%opt.description);
		addToSet("MissionCleanup", %vehicle);
		GameBase::setTeam(%vehicle, GameBase::GetTeam(%clientId));
		GameBase::startFadeIn(%vehicle);
		GameBase::setPosition(%vehicle, $los::position);
		GameBase::setRotation(%vehicle, vector::add(GameBase::getRotation(%clientId), "0 0 1.57"));
		
		echo(fetchData(%clientId, "OwnedVehicles"));
		
		%vehicles = fetchData(%clientId, "OwnedVehicles");
		for(%i = 0; (%word = GetWord(%vehicles, %i)) != -1; %i++)
		{
			if(%word = %opt)
			{
				if(GetWord(%vehicles, %i++) > 1)
					%string = String::replace(fetchData(%clientId, "OwnedVehicles"), %opt @ " " @ %num, %opt @ " " @ %num - 1);
				storeData(%clientId, "OwnedVehicles", %string);
				break;
			}
		}
		echo(fetchData(%clientId, "OwnedVehicles"));
	}
	return;
}

$AccessoryVar[RecallBeacon, $Weight] = 5;
$AccessoryVar[RecallBeacon, $MiscInfo] = "When used, teleports you to the nearest safe zone (city, town, space port, etc.)";
$StealProtectedItem[RecallBeacon] = True;
ItemData RecallBeacon
{
	description = "Recall Beacon";
	className = "Accessory";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};
function RecallBeacon::onUse(%player,%item)
{
	%clientId = player::getClient(%player);
	%zoneId = GetNearestZone(%clientId, Town, 2);

	if(%zoneId != False)
	{
		Client::sendMessage(%clientId, $MsgBeige, "Teleporting near " @ Zone::getDesc(%zoneId));

		//teleport
		//%mpos = Zone::getMarker(%zoneId);
		TeleportToMarker(%clientId, "Zones\\" @ %zoneid @ "\\DropPoints", False, True);
		if(!fetchData(%clientId, "invisible"))
			GameBase::startFadeIn(%clientId);

		GameBase::setPosition(%clientId, %mpos);
		CheckAndBootFromArena(%clientId);
		//NullItemList(%clientId, Lore, $MsgRed, "You lost all %1s you were carrying when you teleported.");

		Player::setDamageFlash(%clientId, 0.7);
		playSound(ActivateCH, %castPos);
		%pos = vector::add(Gamebase::getposition(%player), "0 0 1.7");
		%trans = "0 0 -1 0 0 0 0 0 -1 " @ %pos;
	      	%vel = Item::getVelocity(%player);
		Projectile::spawnProjectile(FusionBolt, %trans, %player, %vel);

			//%castPos = SetOnGround(%clientId, 500);
	}
	else
		Client::sendMessage(%clientId, $MsgBeige, "Teleportation failed. ..That's weird, it shouldn't have. I wonder why it did? Tell Hazor, please, so that he can try to fix it. Thanks! ^_^");
}

$AccessoryVar[PDA, $Weight] = 5;
$AccessoryVar[PDA, $MiscInfo] = "The Hitchhiker's Guide to the Galaxy.";
$StealProtectedItem[PDA] = False; //Chance this in the future?
ItemData PDA
{
	description = "PDA";
	className = "Accessory";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};
function PDA::onUse(%player,%item)
{
	Computer::Initialize(player::getClient(%player), 0);	//Leave the second arguement as 0! This way the computer system functions know
}								//to treat it as a portable computer, and not a map-item terminal.
								//If it's a number above 0, it'll think it's a static terminal, not a portable.
//===================
//  Mining stuff
//===================
$AccessoryVar[Quartz, $Weight] = 0.2;
$AccessoryVar[Granite, $Weight] = 0.2;
$AccessoryVar[Opal, $Weight] = 0.2;
$AccessoryVar[Jade, $Weight] = 0.25;
$AccessoryVar[Turquoise, $Weight] = 0.3;
$AccessoryVar[Ruby, $Weight] = 0.3;
$AccessoryVar[Topaz, $Weight] = 0.3;
$AccessoryVar[Sapphire, $Weight] = 0.3;
$AccessoryVar[Gold, $Weight] = 3.5;
$AccessoryVar[Emerald, $Weight] = 0.2;
$AccessoryVar[Diamond, $Weight] = 0.1;
$AccessoryVar[Keldrinite, $Weight] = 5.0;

$AccessoryVar[Quartz, $MiscInfo] = "Quartz";
$AccessoryVar[Granite, $MiscInfo] = "Granite";
$AccessoryVar[Opal, $MiscInfo] = "Opal";
$AccessoryVar[Jade, $MiscInfo] = "Jade";
$AccessoryVar[Turquoise, $MiscInfo] = "Turquoise";
$AccessoryVar[Ruby, $MiscInfo] = "Ruby";
$AccessoryVar[Topaz, $MiscInfo] = "Topaz";
$AccessoryVar[Sapphire, $MiscInfo] = "Sapphire";
$AccessoryVar[Gold, $MiscInfo] = "Gold";
$AccessoryVar[Emerald, $MiscInfo] = "Emerald";
$AccessoryVar[Diamond, $MiscInfo] = "Diamond";
$AccessoryVar[Keldrinite, $MiscInfo] = "Keldrinite is a very rare magical gem that, when in the hands of a skilled blacksmith, can give items magical properties.";

$HardcodedItemCost[SmallRock] = 13;
$HardcodedItemCost[Quartz] = 100;
$HardcodedItemCost[Granite] = 180;
$HardcodedItemCost[Opal] = 300;
$HardcodedItemCost[Jade] = 550;
$HardcodedItemCost[Turquoise] = 850;
$HardcodedItemCost[Ruby] = 1200;
$HardcodedItemCost[Topaz] = 1604;
$HardcodedItemCost[Sapphire] = 2930;
$HardcodedItemCost[Gold] = 4680;
$HardcodedItemCost[Emerald] = 9702;
$HardcodedItemCost[Diamond] = 16575;
$HardcodedItemCost[Keldrinite] = 125200;

%f = 43;
$ItemList[Mining, 1] = "SmallRock " @ round($HardcodedItemCost[SmallRock] / %f)+2;
$ItemList[Mining, 2] = "Quartz " @ round($HardcodedItemCost[Quartz] / %f)+2;
$ItemList[Mining, 3] = "Granite " @ round($HardcodedItemCost[Granite] / %f)+2;
$ItemList[Mining, 4] = "Opal " @ round($HardcodedItemCost[Opal] / %f)+2;
$ItemList[Mining, 5] = "Jade " @ round($HardcodedItemCost[Jade] / %f)+2;
$ItemList[Mining, 6] = "Turquoise " @ round($HardcodedItemCost[Turquoise] / %f)+2;
$ItemList[Mining, 7] = "Ruby " @ round($HardcodedItemCost[Ruby] / %f)+2;
$ItemList[Mining, 8] = "Topaz " @ round($HardcodedItemCost[Topaz] / %f)+2;
$ItemList[Mining, 9] = "Sapphire " @ round($HardcodedItemCost[Sapphire] / %f)+2;
$ItemList[Mining, 10] = "Gold " @ round($HardcodedItemCost[Gold] / %f)+2;
$ItemList[Mining, 11] = "Emerald " @ round($HardcodedItemCost[Emerald] / %f)+2;
$ItemList[Mining, 12] = "Diamond " @ round($HardcodedItemCost[Diamond] / %f)+2;
$ItemList[Mining, 13] = "Keldrinite " @ round($HardcodedItemCost[Keldrinite] / %f)+2;

ItemData Quartz
{
	description = "Quartz";
	className = "Accessory";
	shapeFile = "quartz";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};
ItemData Granite
{
	description = "Granite";
	className = "Accessory";
	shapeFile = "granite";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};
ItemData Opal
{
	description = "Opal";
	className = "Accessory";
	shapeFile = "opal";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};
ItemData Jade
{
	description = "Jade";
	className = "Accessory";
	shapeFile = "jade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};
ItemData Turquoise
{
	description = "Turquoise";
	className = "Accessory";
	shapeFile = "turquoise";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};
ItemData Ruby
{
	description = "Ruby";
	className = "Accessory";
	shapeFile = "ruby";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};
ItemData Topaz
{
	description = "Topaz";
	className = "Accessory";
	shapeFile = "topaz";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};
ItemData Sapphire
{
	description = "Sapphire";
	className = "Accessory";
	shapeFile = "saphire";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};
ItemData Gold
{
	description = "Gold";
	className = "Accessory";
	shapeFile = "gold";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};
ItemData Emerald
{
	description = "Emerald";
	className = "Accessory";
	shapeFile = "Emerald";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};
ItemData Diamond
{
	description = "Diamond";
	className = "Accessory";
	shapeFile = "diamond";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};
ItemData Keldrinite
{
	description = "Keldrinite";
	className = "Accessory";
	shapeFile = "keldrinite";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

$AccessoryVar[BlackStatue, $Weight] = 1;
$AccessoryVar[BlackStatue, $MiscInfo] = "A black statue";

ItemData BlackStatue
{
	description = "Black Statue";
	className = "Accessory";
	shapeFile = "mineammo";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

$AccessoryVar[SkeletonBone, $Weight] = 1;
$AccessoryVar[SkeletonBone, $MiscInfo] = "A skeleton bone";

ItemData SkeletonBone
{
	description = "Skeleton Bone";
	className = "Accessory";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

$AccessoryVar[EnchantedStone, $Weight] = 5;
$AccessoryVar[EnchantedStone, $MiscInfo] = "An enchanted stone";

ItemData EnchantedStone
{
	description = "Enchanted Stone";
	className = "Accessory";
	shapeFile = "granite";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

$AccessoryVar[DragonScale, $Weight] = 8;
$AccessoryVar[DragonScale, $MiscInfo] = "A dragon scale";

ItemData DragonScale
{
	description = "Dragon Scale";
	className = "Accessory";
	shapeFile = "granite";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

//===================
//  LORE ITEMS
//===================
$ItemList[Lore, 1] = "Parchment";
$ItemList[Lore, 2] = "Holocron";
$ItemList[Lore, 3] = "JediHolocron";
$ItemList[Lore, 4] = "SithHolocron";

$AccessoryVar[Parchment, $Weight] = 0.2;
$AccessoryVar[Parchment, $MiscInfo] = "A parchment";
$LoreItem[Parchment] = True;

ItemData Parchment
{
	description = "Parchment";
	className = "Accessory";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

$AccessoryVar[Holocron, $Weight] = 0.2;
$AccessoryVar[Holocron, $MiscInfo] = "A device used for storing phenomenal quantities of data. In the era of the Rebellion, the technology to make them is nearly lost.";
$LoreItem[Holocron] = True;

ItemData Holocron
{
	description = "Holocron";
	className = "Accessory";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};
function Holocron::onUse(%player, %item)
{
	Player::decItemCount(%player, %item);

	%list = GetEveryoneIdList();
	for(%i = 0; (%id = GetWord(%list, %i)) != -1; %i++)
	{
		%pl = Client::getOwnedObject(%Id);
		if(Vector::getDistance(GameBase::getPosition(%player), GameBase::getPosition(%pl)) <= 20)
			Player::applyImpulse(%pl, "0 0 500");
	}
}

$AccessoryVar[JediHolocron, $Weight] = 0.2;
$AccessoryVar[JediHolocron, $MiscInfo] = "Jedi Holocrons are a sub-class of holocrons which are made by Jedi, and contain valuable information about jedi techniques, training methods, lightsaber creation, and other useful knowledge. Usually, only Force-attuned users can access a Jedi Holocron.";
$LoreItem[JediHolocron] = True;

ItemData JediHolocron
{
	description = "Jedi Holocron";
	className = "Accessory";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};
function JediHolocron::onUse(%player, %item)
{
	%clientId = player::getClient(%player);
	%clientId.hologram = newObject("", "StaticShape", Client::getGender(%clientId) @ "RobedTownBot", 1, false);

	%playerPos = GameBase::getPosition(%player);
	%playerRot = GameBase::getRotation(%player);
	//GameBase::setPosition(%clientId.hologram, GameBase::getMuzzlePosition(%player));
	GameBase::setPosition(%clientId.hologram, vector::getfromrot(%playerRot, 2));
	GameBase::SetRotation(%clientId.hologram, vector::getrotation(vector::sub(%playerpos, gamebase::getposition(%clientId.hologram))));

	schedule("deleteObject(" @ %client.hologram @ ");", 10);
}
//The Holocron may refuse to divulge information to non-force-sensitives.

$AccessoryVar[SithHolocron, $Weight] = 0.2;
$AccessoryVar[SithHolocron, $MiscInfo] = "A device used for storing phenomenal quantities of data. In the era of the Rebellion, the technology to make them is nearly lost.";
$LoreItem[SithHolocron] = True;

ItemData SithHolocron
{
	description = "Sith Holocron";
	className = "Accessory";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};
function SithHolocron::onUse(%player, %item)
{
	Player::decItemCount(%player, %item);

	%list = GetEveryoneIdList();
	for(%i = 0; (%id = GetWord(%list, %i)) != -1; %i++)
	{
		%pl = Client::getOwnedObject(%clientId);
		if(Vector::getDistance(GameBase::getPosition(%player), GameBase::getPosition(%pl)) <= 20)
			Player::applyImpulse(%pl, "0 0 500");
	}
}

//===================
// Badges
//===================
$ItemList[Badge, 1] = "BadgeOfFriendship";
$ItemList[Badge, 2] = "BadgeOfLoyalty";
$ItemList[Badge, 3] = "BadgeOfHonor";
$ItemList[Badge, 4] = "BadgeOfReverence";

$AccessoryVar[BadgeOfHonor, $Weight] = 1;
$AccessoryVar[BadgeOfHonor, $MiscInfo] = "Badge Of Honor. A chance in 220 every two seconds that an LCK point will be awarded.";
$BonusItem[BadgeOfHonor] = "LCK 1 220";	//a chance in 220 every ZoneCheck that 1 LCK will be awarded
$StealProtectedItem[BadgeOfHonor] = True;

ItemData BadgeOfHonor
{
	description = "Badge Of Honor";
	className = "Accessory";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

$AccessoryVar[BadgeOfLoyalty, $Weight] = 1;
$AccessoryVar[BadgeOfLoyalty, $MiscInfo] = "Badge Of Loyalty. A chance in 120 every two seconds that 3 EXP will be awarded.";
$BonusItem[BadgeOfLoyalty] = "EXP 3 120";
$StealProtectedItem[BadgeOfLoyalty] = True;

ItemData BadgeOfLoyalty
{
	description = "Badge Of Loyalty";
	className = "Accessory";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

$AccessoryVar[BadgeOfFriendship, $Weight] = 1;
$AccessoryVar[BadgeOfFriendship, $MiscInfo] = "Badge Of Friendship. A chance in 80 every two seconds that 50 credits will be awarded.";
$BonusItem[BadgeOfFriendship] = "COINS 50 80";
$StealProtectedItem[BadgeOfFriendship] = True;

ItemData BadgeOfFriendship
{
	description = "Badge Of Friendship";
	className = "Accessory";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

$AccessoryVar[BadgeOfReverence, $Weight] = 1;
$AccessoryVar[BadgeOfReverence, $MiscInfo] = "Badge Of Reverence. A chance in 180 every two seconds that an SP credit will be awarded.";
$BonusItem[BadgeOfReverence] = "SP 1 180";
$StealProtectedItem[BadgeOfReverence] = True;

ItemData BadgeOfReverence
{
	description = "Badge Of Reverence";
	className = "Accessory";
	shapeFile = "grenade";
	heading = "eMiscellany";
	shadowDetailMask = 4;
	price = 0;
};

//========= ORBS ===================================================

//i suggest putting orbs that protect from water at the top of the list.
$ItemList[Orb, 1] = "BreathingDevice";
$ItemList[Orb, 2] = "Glowrod";

//Orb of Luminance
$AccessoryVar[Glowrod, $AccessoryType] = $ShieldAccessoryType;
$AccessoryVar[Glowrod, $Weight] = 1.0;
$AccessoryVar[Glowrod, $MiscInfo] = "The glow rod provides you with temporary illumination.";
$OverrideMountPoint[Glowrod] = 2;
$BurnOut[Glowrod] = 150;
$BurnOutInRain[Glowrod] = 5;
$ProtectFromWater[Glowrod] = "1";

ItemImageData GlowrodImage
{
	shapeFile = "orb";
	mountPoint = $OverrideMountPoint[Glowrod];
	mountOffset = {0.0, 0.0, 1.8};
	mountRotation = {5, 3, 3};

	lightType = 2;
	lightRadius = 13;
	lightTime = 9999;
	lightColor = { 0.95, 0.85, 0.55 };
};
ItemData Glowrod
{
	description = "Glowrod";
	className = "Accessory";
	shapeFile = "orb";
	imageType = GlowrodImage;

	heading = "eMiscellany";
	price = 0;
};
ItemData Glowrod0
{
	description = "Lit Glowrod";
	className = "Equipped";
	shapeFile = "orb";
	imageType = GlowrodImage;

	heading = "aArmor";
};

//Orb of Breath
$AccessoryVar[BreathingDevice, $AccessoryType] = $ShieldAccessoryType;
$AccessoryVar[BreathingDevice, $Weight] = 0.8;
$AccessoryVar[BreathingDevice, $MiscInfo] = "This oxygen mask allows you to breath while under water.";
$OverrideMountPoint[BreathingDevice] = 2;
$BurnOut[BreathingDevice] = 300;
$BurnOutInRain[BreathingDevice] = 0;
$ProtectFromWater[BreathingDevice] = True;

ItemImageData BreathingDeviceImage
{
	shapeFile = "orb";
	mountPoint = $OverrideMountPoint[BreathingDevice];
	mountOffset = {0.0, 0.0, 0.4};
	mountRotation = {5, 3, 3};
};
ItemData BreathingDevice
{
	description = "Breathing Device";
	className = "Accessory";
	shapeFile = "orb";
	imageType = BreathingDeviceImage;

	heading = "eMiscellany";
	price = 0;
};
ItemData BreathingDevice0
{
	description = "Breathing Device in use";
	className = "Equipped";
	shapeFile = "orb";
	imageType = BreathingDeviceImage;

	heading = "aArmor";
};
