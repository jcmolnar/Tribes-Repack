function SetupShop(%clientId, %id)
{
	dbecho($dbechoMode, "SetupShop(" @ %clientId @ ", " @ %id @ ")");

	ClearCurrentShopVars(%clientId);
	%clientId.currentShop = %id;

	%clientId.bulkNum = "";

	Client::clearItemShopping(%clientId);
	Client::clearItemBuying(%clientId);

	Client::setGuiMode(%clientId, 4);

	%txt = "<f1><jc>CREDITS: " @ fetchData(%clientId, "COINS");
	Client::setInventoryText(%clientId, %txt);

	%info = $BotInfo[%id.name, SHOP];	

	for(%i = 0; GetWord(%info, %i) != -1; %i++)
	{
		%a = GetWord(%info, %i);

		%max = getNumItems();		
		for(%z = 0; %z < %max; %z++)
		{
			%item = getItemData(%z);

			if($AccessoryVar[%item, $ShopIndex] == %a)
			{
				Client::setItemShopping(%clientId, %item);
				Client::setItemBuying(%clientId, %item);
			}
		}
	}
}

function SetupBank(%clientId, %id)
{
	dbecho($dbechoMode, "SetupBank(" @ %clientId @ ", " @ %id @ ")");

	ClearCurrentShopVars(%clientId);
	%clientId.currentBank = %id;

	%clientId.bulkNum = "";

	Client::clearItemShopping(%clientId);
	Client::clearItemBuying(%clientId);

	if(Client::getGuiMode(%clientId) != 4)
		Client::setGuiMode(%clientId, 4);

	%txt = "<f1><jc>CREDITS: " @ fetchData(%clientId, "COINS");
	Client::setInventoryText(%clientId, %txt);

	%info = fetchData(%clientId, "BankStorage");

	for(%i = 0; GetWord(%info, %i) != -1; %i+=2)
	{
		%item = GetWord(%info, %i);

		Client::setItemShopping(%clientId, %item);
		Client::setItemBuying(%clientId, %item);
	}
}

function SetupBlacksmith(%clientId, %id)
{
	dbecho($dbechoMode, "SetupBlacksmith(" @ %clientId @ ", " @ %id @ ")");

	%clientId.currentSmith = %id;

	%clientId.bulkNum = "";

	Client::clearItemShopping(%clientId);
	Client::clearItemBuying(%clientId);

	if(Client::getGuiMode(%clientId) != 4)
		Client::setGuiMode(%clientId, 4);

	%info = fetchData(%clientId, "TempSmith");
	for(%i = 0; GetWord(%info, %i) != -1; %i+=2)
	{
		%item = GetWord(%info, %i);

		Client::setItemShopping(%clientId, %item);
		Client::setItemBuying(%clientId, %item);
	}

	%txt = "<f1><jc>CREDITS: " @ fetchData(%clientId, "COINS");
	Client::setInventoryText(%clientId, %txt);
}

function SetupInvSteal(%clientId, %id)
{
	dbecho($dbechoMode, "SetupInvSteal(" @ %clientId @ ", " @ %id @ ")");

	ClearCurrentShopVars(%clientId);
	%clientId.currentInvSteal = %id;

	%clientId.bulkNum = "";

	Client::clearItemShopping(%clientId);
	Client::clearItemBuying(%clientId);

	if(Client::getGuiMode(%clientId) != 4)
		Client::setGuiMode(%clientId, 4);

	%txt = "<f1><jc>" @ Client::getName(%id) @ "'s inventory";
	Client::setInventoryText(%clientId, %txt);

	%max = getNumItems();
	for(%i = 0; %i < %max; %i++)
	{
		%item = getItemData(%i);
		%itemcount = Player::getItemCount(%id, %item);

		if(%itemcount > 0)
		{
			Client::setItemShopping(%clientId, %item);
			Client::setItemBuying(%clientId, %item);
		}
	}
}

function SetupCreatePack(%clientId)
{
	dbecho($dbechoMode, "SetupCreatePack(" @ %clientId @ ")");

	Client::clearItemShopping(%clientId);
	Client::clearItemBuying(%clientId);

	if(Client::getGuiMode(%clientId) != 4)
		Client::setGuiMode(%clientId, 4);

	%info = fetchData(%clientId, "TempPack");
	for(%i = 0; GetWord(%info, %i) != -1; %i+=2)
	{
		%item = GetWord(%info, %i);

		Client::setItemShopping(%clientId, %item);
		Client::setItemBuying(%clientId, %item);
	}
}

function ClearCurrentShopVars(%clientId)
{
	dbecho($dbechoMode, "ClearCurrentShopVars(" @ %clientId @ ")");

      %clientId.currentShop = "";
      %clientId.currentBank = "";
      %clientId.currentSmith = "";
	%clientId.currentInvSteal = "";

	storeData(%clientId, "TempPack", "");
	storeData(%clientId, "TempSmith", "");
}

$AccessoryVar[AdvisorRobe, $ShopIndex] = 129;
//$AccessoryVar[Ammo, $ShopIndex] = 1; // ======
$AccessoryVar[AntigravityBoots, $ShopIndex] = 9;
$AccessoryVar[ApprenticeRobe, $ShopIndex] = 125;
//$AccessoryVar[Backpack, $ShopIndex] = 2; // =====
$AccessoryVar[BactaCanister, $ShopIndex] = 5;
$AccessoryVar[BactaVial, $ShopIndex] = 4;
$AccessoryVar[BadgeOfFriendship, $ShopIndex] = 47;
$AccessoryVar[BadgeOfHonor, $ShopIndex] = 45;
$AccessoryVar[BadgeOfLoyalty, $ShopIndex] = 46;
$AccessoryVar[BadgeOfReverence, $ShopIndex] = 48;
$AccessoryVar[BaktoidE5, $ShopIndex] = 84;
$AccessoryVar[BastardSword, $ShopIndex] = 60;
$AccessoryVar[BlackStatue, $ShopIndex] = 37;
$AccessoryVar[BlasTechDL44, $ShopIndex] = 82;
$AccessoryVar[BlasTechDLT19, $ShopIndex] = 87;
$AccessoryVar[BlasTechE11, $ShopIndex] = 85;
$AccessoryVar[BlasTechT21, $ShopIndex] = 89;
$AccessoryVar[BlueLightsaber, $ShopIndex] = 54;
$AccessoryVar[BonadanHeavyArmor, $ShopIndex] = 120;
$AccessoryVar[Bowcaster, $ShopIndex] = 15;
$AccessoryVar[CastingBlade, $ShopIndex] = 91;
$AccessoryVar[CastingSaber, $ShopIndex] = 92;
$AccessoryVar[CheetaursPaws, $ShopIndex] = 8;
$AccessoryVar[CinnagarWarSuit, $ShopIndex] = 117;
$AccessoryVar[Claymore, $ShopIndex] = 63;
$AccessoryVar[CorellianPowersuit, $ShopIndex] = 116;
$AccessoryVar[Dagger, $ShopIndex] = 52;
$AccessoryVar[Diamond, $ShopIndex] = 35;
$AccessoryVar[DoubleRedLightsaber, $ShopIndex] = 57;
$AccessoryVar[DragonScale, $ShopIndex] = 40;
//$AccessoryVar[DroidTownBot, $ShopIndex] = 17; // =====
$AccessoryVar[DurasteelHeavyArmor, $ShopIndex] = 123;
$AccessoryVar[EchaniBattleArmor, $ShopIndex] = 118;
$AccessoryVar[EchaniFoil, $ShopIndex] = 64;
$AccessoryVar[EchaniHeavyArmor, $ShopIndex] = 124;
$AccessoryVar[EchaniLightArmor, $ShopIndex] = 114;
$AccessoryVar[EchaniShieldSuit, $ShopIndex] = 121;
$AccessoryVar[EchaniShield, $ShopIndex] = 14;
$AccessoryVar[EE3Carbine, $ShopIndex] = 86;
$AccessoryVar[Emerald, $ShopIndex] = 34;
$AccessoryVar[EnchantedStone, $ShopIndex] = 39;
$AccessoryVar[EnergyCells, $ShopIndex] = 94;
//$AccessoryVar[FemaleHumanTownBot, $ShopIndex] = 16; // =====
$AccessoryVar[GaffiiStick, $ShopIndex] = 59;
$AccessoryVar[GamorreanCleaver, $ShopIndex] = 65;
$AccessoryVar[GamorreanWaraxe, $ShopIndex] = 75;
$AccessoryVar[Gold, $ShopIndex] = 33;
$AccessoryVar[Granite, $ShopIndex] = 26;
$AccessoryVar[GreenLightsaber, $ShopIndex] = 55;
$AccessoryVar[GunganShield, $ShopIndex] = 11;
$AccessoryVar[HeavyCombatSuit, $ShopIndex] = 111;
$AccessoryVar[HeavyCrossbow, $ShopIndex] = 88;
$AccessoryVar[HeavyRepeater, $ShopIndex] = 90;
$AccessoryVar[HikenLightsaber, $ShopIndex] = 58;
$AccessoryVar[HoldoutBlaster, $ShopIndex] = 81;
$AccessoryVar[Holocron, $ShopIndex] = 42;
$AccessoryVar[IonBlasterCells, $ShopIndex] = 96;
$AccessoryVar[Jade, $ShopIndex] = 28;
$AccessoryVar[JediHolocron, $ShopIndex] = 43;
$AccessoryVar[JediKnightRobe, $ShopIndex] = 128;
$AccessoryVar[JediMasterRobe, $ShopIndex] = 132;
$AccessoryVar[JediRobe, $ShopIndex] = 127;
$AccessoryVar[JetPack, $ShopIndex] = 10;
$AccessoryVar[Keldrinite, $ShopIndex] = 36;
$AccessoryVar[KeldriniteLS, $ShopIndex] = 62;
$AccessoryVar[KelDromaRobe, $ShopIndex] = 130;
$AccessoryVar[Knife, $ShopIndex] = 51;
$AccessoryVar[KoltoCanister, $ShopIndex] = 7;
$AccessoryVar[KoltoVial, $ShopIndex] = 6;
$AccessoryVar[KrathDireSword, $ShopIndex] = 78;
$AccessoryVar[KrathHeavyArmor, $ShopIndex] = 137;
$AccessoryVar[KrathWarBlade, $ShopIndex] = 69;
$AccessoryVar[LightCombatSuit, $ShopIndex] = 110;
$AccessoryVar[LongStaff, $ShopIndex] = 77;
$AccessoryVar[LongSword, $ShopIndex] = 72;
//$AccessoryVar[Lootbag, $ShopIndex] = 22; // =====
$AccessoryVar[Mace, $ShopIndex] = 74;
$AccessoryVar[MandalorianAssaultArmor, $ShopIndex] = 119;
$AccessoryVar[MandalorianCombatSuit, $ShopIndex] = 115;
$AccessoryVar[MandalorianShield, $ShopIndex] = 12;
$AccessoryVar[NerinLightsaber, $ShopIndex] = 61;
$AccessoryVar[NoobianS5, $ShopIndex] = 83;
$AccessoryVar[NorrisRobe, $ShopIndex] = 131;
$AccessoryVar[Opal, $ShopIndex] = 27;
$AccessoryVar[OrbOfLuminance, $ShopIndex] = 49;
$AccessoryVar[OssusKeeperRobe, $ShopIndex] = 133;
$AccessoryVar[PadawanRobe, $ShopIndex] = 126;
$AccessoryVar[PaddedCombatSuit, $ShopIndex] = 109;
$AccessoryVar[Parchment, $ShopIndex] = 41;
$AccessoryVar[PDA, $ShopIndex] = 2;
$AccessoryVar[PickAxe, $ShopIndex] = 66;
$AccessoryVar[PoweredCombatSuit, $ShopIndex] = 113;
$AccessoryVar[QuarterStaff, $ShopIndex] = 76;
$AccessoryVar[Quartz, $ShopIndex] = 25;
$AccessoryVar[QuestMasterRobe, $ShopIndex] = 136;
$AccessoryVar[RakatanBattleAxe, $ShopIndex] = 67;
$AccessoryVar[RakatanVibroSword, $ShopIndex] = 71;
$AccessoryVar[RBaktoidE5, $ShopIndex] = 108;
$AccessoryVar[RBlasTechDL44, $ShopIndex] = 107;
$AccessoryVar[RBlasTechE11, $ShopIndex] = 20;
$AccessoryVar[RBlueLightsaber, $ShopIndex] = 100;
$AccessoryVar[RDagger, $ShopIndex] = 98;
$AccessoryVar[RDoubleRedLightsaber, $ShopIndex] = 103;
$AccessoryVar[RecallBeacon, $ShopIndex] = 24;
$AccessoryVar[RedLightsaber, $ShopIndex] = 56;
$AccessoryVar[RepairPatch, $ShopIndex] = 3;
$AccessoryVar[RGreenLightsaber, $ShopIndex] = 101;
$AccessoryVar[RKnife, $ShopIndex] = 97;
$AccessoryVar[RLongSword, $ShopIndex] = 105;
$AccessoryVar[RMace, $ShopIndex] = 106;
$AccessoryVar[RPickAxe, $ShopIndex] = 104;
$AccessoryVar[RRedLightsaber, $ShopIndex] = 102;
$AccessoryVar[RShortSword, $ShopIndex] = 99;
$AccessoryVar[Ruby, $ShopIndex] = 30;
$AccessoryVar[Sapphire, $ShopIndex] = 32;
$AccessoryVar[ShortSword, $ShopIndex] = 53;
$AccessoryVar[SithBattleArmor, $ShopIndex] = 122;
$AccessoryVar[SithHolocron, $ShopIndex] = 44;
$AccessoryVar[SithLordRobe, $ShopIndex] = 134;
$AccessoryVar[SkeletonBone, $ShopIndex] = 38;
$AccessoryVar[SmallRock, $ShopIndex] = 93;
$AccessoryVar[Spear, $ShopIndex] = 70;
$AccessoryVar[StarForgeRobe, $ShopIndex] = 135;
//$AccessoryVar[StormTrooperTownBot, $ShopIndex] = 19; // =====
$AccessoryVar[Tent, $ShopIndex] = 21;
$AccessoryVar[TibannaGasCells, $ShopIndex] = 95;
//$AccessoryVar[Tool, $ShopIndex] = 0; // =======
$AccessoryVar[Topaz, $ShopIndex] = 31;
$AccessoryVar[Turquoise, $ShopIndex] = 29;
//$AccessoryVar[TuskenTownBot, $ShopIndex] = 18; // =====
$AccessoryVar[VehicleBeacon, $ShopIndex] = 23;
$AccessoryVar[VerpineFiberMesh, $ShopIndex] = 112;
$AccessoryVar[VerpineShield, $ShopIndex] = 13;
$AccessoryVar[Vibroblade, $ShopIndex] = 73;
$AccessoryVar[Vibroshiv, $ShopIndex] = 79;
$AccessoryVar[Vibrosword, $ShopIndex] = 80;
$AccessoryVar[ZabrakVibroblade, $ShopIndex] = 68;

function setupIndexes()
{
	deleteVariables("AcessoryVar*");
	%max = getNumItems();
	for(%i = 0; %i < %max; %i++)
	{
		%item = getItemData(%i); if(String::findSubStr(%item @ "xx", "0xx") != -1) %e++;
		$AccessoryVar["[\""@ %item @ "\", $ShopIndex]"] = %i - %e;
		//export("AccessoryVar[\"" @ %item @ "\", " @ $ShopIndex @ "*", "temp\\aaashop.cs", True);
		//export("AccessoryVar[\"" @ %item @ "\", " @ $ShopIndex @ "]", "temp\\aaashop.cs", True);
	}
	export("AccessoryVar*", "temp\\aaashop.cs", False);
	echo(%i);
	echo(isFile("temp\\aaashop.cs"));
}