//This file is part of Tribes RPG.
//Tribes RPG server side scripts
//Written by Jason "phantom" Daley,  Matthiew "JeremyIrons" Bouchard, and more (yet undetermined)

//	Copyright (C) 2014  Jason Daley

//	This program is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.

//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.

//	You should have received a copy of the GNU General Public License
//	along with this program.  If not, see <http://www.gnu.org/licenses/>.

//You may contact the author at beatme101@gmail.com or www.tribesrpg.org/contact.php

//This GPL does not apply to Starsiege: Tribes or any non-RPG related files included.
//Starsiege: Tribes, including the engine, retains a proprietary license forbidding resale.


$fireTimeDelay = 0.1;

$RustyDamageAmp = 0.7;
$RustyWeightAmp = 1.5;
$RustyCostAmp = 0.3;

$RangeTable[$AxeAccessoryType] = 3;
$RangeTable[$SwordAccessoryType] = 3;
$RangeTable[$PolearmAccessoryType] = 4;
$RangeTable[$BludgeonAccessoryType] = 3;

$DelayFactorTable[$RingAccessoryType] = "0.0";
$DelayFactorTable[$BodyAccessoryType] = "0.0";
$DelayFactorTable[$BootsAccessoryType] = "0.0";
$DelayFactorTable[$BackAccessoryType] = "0.0";
$DelayFactorTable[$ShieldAccessoryType] = "0.0";
$DelayFactorTable[$TalismanAccessoryType] = "0.0";
$DelayFactorTable[$AxeAccessoryType] = "1.0";
$DelayFactorTable[$SwordAccessoryType] = "1.0";
$DelayFactorTable[$PolearmAccessoryType] = "1.0";
$DelayFactorTable[$BludgeonAccessoryType] = "1.0";
$DelayFactorTable[$RangedAccessoryType] = "1.0";
$DelayFactorTable[$ProjectileAccessoryType] = "1.0";

$CostFactorTable[$RingAccessoryType] = "1.0";
$CostFactorTable[$BodyAccessoryType] = "1.0";
$CostFactorTable[$BootsAccessoryType] = "1.0";
$CostFactorTable[$BackAccessoryType] = "1.0";
$CostFactorTable[$ShieldAccessoryType] = "1.0";
$CostFactorTable[$TalismanAccessoryType] = "1.0";
$CostFactorTable[$SwordAccessoryType] = "1.0";
$CostFactorTable[$AxeAccessoryType] = "1.0";
$CostFactorTable[$PolearmAccessoryType] = "1.0";
$CostFactorTable[$BludgeonAccessoryType] = "1.0";
$CostFactorTable[$RangedAccessoryType] = "1.0";
$CostFactorTable[$ProjectileAccessoryType] = "0.01";

//****************************************************************************************************

$AccessoryVar[Hatchet, $AccessoryType] = $AxeAccessoryType;
$AccessoryVar[BroadSword, $AccessoryType] = $SwordAccessoryType;
$AccessoryVar[WarAxe, $AccessoryType] = $AxeAccessoryType;
$AccessoryVar[LongSword, $AccessoryType] = $SwordAccessoryType;
$AccessoryVar[BattleAxe, $AccessoryType] = $AxeAccessoryType;
$AccessoryVar[BastardSword, $AccessoryType] = $SwordAccessoryType;
$AccessoryVar[Halberd, $AccessoryType] = $AxeAccessoryType;
$AccessoryVar[Claymore, $AccessoryType] = $SwordAccessoryType;
$AccessoryVar[Club, $AccessoryType] = $BludgeonAccessoryType;
$AccessoryVar[SpikedClub, $AccessoryType] = $BludgeonAccessoryType;
$AccessoryVar[Mace, $AccessoryType] = $BludgeonAccessoryType;
$AccessoryVar[HammerPick, $AccessoryType] = $BludgeonAccessoryType;
$AccessoryVar[WarHammer, $AccessoryType] = $BludgeonAccessoryType;
$AccessoryVar[WarMaul, $AccessoryType] = $BludgeonAccessoryType;
$AccessoryVar[QuarterStaff, $AccessoryType] = $BludgeonAccessoryType;
$AccessoryVar[LongStaff, $AccessoryType] = $BludgeonAccessoryType;
$AccessoryVar[JusticeStaff, $AccessoryType] = $BludgeonAccessoryType;
$AccessoryVar[Knife, $AccessoryType] = $SwordAccessoryType;
$AccessoryVar[Dagger, $AccessoryType] = $SwordAccessoryType;
$AccessoryVar[ShortSword, $AccessoryType] = $SwordAccessoryType;
$AccessoryVar[Spear, $AccessoryType] = $PolearmAccessoryType;
$AccessoryVar[Gladius, $AccessoryType] = $SwordAccessoryType;
$AccessoryVar[Trident, $AccessoryType] = $PolearmAccessoryType;
$AccessoryVar[Rapier, $AccessoryType] = $SwordAccessoryType;
$AccessoryVar[AwlPike, $AccessoryType] = $PolearmAccessoryType;
$AccessoryVar[PickAxe, $AccessoryType] = $AxeAccessoryType;
$AccessoryVar[Sling, $AccessoryType] = $RangedAccessoryType;
$AccessoryVar[ShortBow, $AccessoryType] = $RangedAccessoryType;
$AccessoryVar[LongBow, $AccessoryType] = $RangedAccessoryType;
$AccessoryVar[ElvenBow, $AccessoryType] = $RangedAccessoryType;
$AccessoryVar[CompositeBow, $AccessoryType] = $RangedAccessoryType;
$AccessoryVar[LightCrossbow, $AccessoryType] = $RangedAccessoryType;
$AccessoryVar[HeavyCrossbow, $AccessoryType] = $RangedAccessoryType;
$AccessoryVar[RepeatingCrossbow, $AccessoryType] = $RangedAccessoryType;
$AccessoryVar[SmallRock, $AccessoryType] = $ProjectileAccessoryType;
$AccessoryVar[BasicArrow, $AccessoryType] = $ProjectileAccessoryType;
$AccessoryVar[SheafArrow, $AccessoryType] = $ProjectileAccessoryType;
$AccessoryVar[BladedArrow, $AccessoryType] = $ProjectileAccessoryType;
$AccessoryVar[LightQuarrel, $AccessoryType] = $ProjectileAccessoryType;
$AccessoryVar[HeavyQuarrel, $AccessoryType] = $ProjectileAccessoryType;
$AccessoryVar[ShortQuarrel, $AccessoryType] = $ProjectileAccessoryType;
$AccessoryVar[CastingBlade, $AccessoryType] = $SwordAccessoryType;
$AccessoryVar[KeldriniteLS, $AccessoryType] = $SwordAccessoryType;
$AccessoryVar[AeolusWing, $AccessoryType] = $RangedAccessoryType;
$AccessoryVar[StoneFeather, $AccessoryType] = $ProjectileAccessoryType;
$AccessoryVar[MetalFeather, $AccessoryType] = $ProjectileAccessoryType;
$AccessoryVar[Talon, $AccessoryType] = $ProjectileAccessoryType;
$AccessoryVar[CeraphumsFeather, $AccessoryType] = $ProjectileAccessoryType;
$AccessoryVar[BoneClub, $AccessoryType] = $BludgeonAccessoryType;
$AccessoryVar[SpikedBoneClub, $AccessoryType] = $BludgeonAccessoryType;

$AccessoryVar[Hatchet, $SpecialVar] = "6 20";			//12 (5)
$AccessoryVar[BroadSword, $SpecialVar] = "6 35";		//21 (5)
$AccessoryVar[WarAxe, $SpecialVar] = "6 70";			//30 (7)
$AccessoryVar[LongSword, $SpecialVar] = "6 65";			//39 (5)
$AccessoryVar[BattleAxe, $SpecialVar] = "6 144";		//48 (9)
$AccessoryVar[BastardSword, $SpecialVar] = "6 133";		//57 (7)
$AccessoryVar[Halberd, $SpecialVar] = "6 176";			//66 (8)
$AccessoryVar[Claymore, $SpecialVar] = "6 188";			//75.2 (7.5)
$AccessoryVar[KeldriniteLS, $SpecialVar] = "6 90";		//90 (0.5)
//.................................................................................
$AccessoryVar[Club, $SpecialVar] = "6 12";			//12 (3)
$AccessoryVar[QuarterStaff, $SpecialVar] = "6 35";		//21 (5)
$AccessoryVar[BoneClub, $SpecialVar] = "6 34";			//26 (4)
$AccessoryVar[SpikedClub, $SpecialVar] = "6 30";		//30 (3)
$AccessoryVar[Mace, $SpecialVar] = "6 78";			//39 (6)
$AccessoryVar[HammerPick, $SpecialVar] = "6 80";		//48 (5)
$AccessoryVar[SpikedBoneClub, $SpecialVar] = "6 70";		//52.5 (4)
$AccessoryVar[LongStaff, $SpecialVar] = "6 114";		//57 (6)
$AccessoryVar[WarHammer, $SpecialVar] = "6 176";		//66 (8)
$AccessoryVar[JusticeStaff, $SpecialVar] = "6 118";		//70.8 (5)
$AccessoryVar[WarMaul, $SpecialVar] = "6 175";			//75 (7)
//.................................................................................
$AccessoryVar[PickAxe, $SpecialVar] = "6 16";			//12 (4)
$AccessoryVar[Knife, $SpecialVar] = "6 18";			//18 (1)
$AccessoryVar[Dagger, $SpecialVar] = "6 23";			//23 (3)
$AccessoryVar[ShortSword, $SpecialVar] = "6 50";		//30 (5)
$AccessoryVar[Spear, $SpecialVar] = "6 78";			//39 (6)
$AccessoryVar[Gladius, $SpecialVar] = "6 80";			//48 (5)
$AccessoryVar[Trident, $SpecialVar] = "6 114";			//57 (6)
$AccessoryVar[Rapier, $SpecialVar] = "6 110";			//66 (5)
$AccessoryVar[AwlPike, $SpecialVar] = "6 200";			//75 (8)
//.................................................................................
$AccessoryVar[CastingBlade, $SpecialVar] = "6 18";
//.................................................................................
$AccessoryVar[Sling, $SpecialVar] = "6 11";			//11 (2)
$AccessoryVar[ShortBow, $SpecialVar] = "6 23";			//23 (3)
$AccessoryVar[LightCrossbow, $SpecialVar] = "6 72";		//36 (6)
$AccessoryVar[LongBow, $SpecialVar] = "6 86";			//51.6 (5)
$AccessoryVar[CompositeBow, $SpecialVar] = "6 85";		//63.75 (4)
$AccessoryVar[RepeatingCrossbow, $SpecialVar] = "6 75";	//75 (3)
$AccessoryVar[ElvenBow, $SpecialVar] = "6 89";			//89 (3)
$AccessoryVar[AeolusWing, $SpecialVar] = "6 101";		//101 (2)
$AccessoryVar[HeavyCrossbow, $SpecialVar] = "6 300";		//112.5 (8)
//.................................................................................
$AccessoryVar[SmallRock, $SpecialVar] = "6 10";
$AccessoryVar[BasicArrow, $SpecialVar] = "6 12";
$AccessoryVar[ShortQuarrel, $SpecialVar] = "6 14";
$AccessoryVar[LightQuarrel, $SpecialVar] = "6 16";
$AccessoryVar[SheafArrow, $SpecialVar] = "6 30";
$AccessoryVar[StoneFeather, $SpecialVar] = "6 40";
$AccessoryVar[BladedArrow, $SpecialVar] = "6 42";
$AccessoryVar[HeavyQuarrel, $SpecialVar] = "6 44";
$AccessoryVar[MetalFeather, $SpecialVar] = "6 60";
$AccessoryVar[Talon, $SpecialVar] = "6 80";
$AccessoryVar[CeraphumsFeather, $SpecialVar] = "6 105";
//.................................................................................

$AccessoryVar[Hatchet, $Weight] = 5;
$AccessoryVar[BroadSword, $Weight] = 5;
$AccessoryVar[WarAxe, $Weight] = 7;
$AccessoryVar[LongSword, $Weight] = 5;
$AccessoryVar[BattleAxe, $Weight] = 9;
$AccessoryVar[BastardSword, $Weight] = 7;
$AccessoryVar[Halberd, $Weight] = 8;
$AccessoryVar[Claymore, $Weight] = "7.5";
$AccessoryVar[KeldriniteLS, $Weight] = "0.5";
//.................................................................................
$AccessoryVar[Club, $Weight] = 3;
$AccessoryVar[QuarterStaff, $Weight] = 5;
$AccessoryVar[BoneClub, $Weight] = 4;
$AccessoryVar[SpikedClub, $Weight] = 3;
$AccessoryVar[Mace, $Weight] = 6;
$AccessoryVar[HammerPick, $Weight] = 5;
$AccessoryVar[SpikedBoneClub, $Weight] = 4;
$AccessoryVar[LongStaff, $Weight] = 6;
$AccessoryVar[WarHammer, $Weight] = 8;
$AccessoryVar[JusticeStaff, $Weight] = 5;
$AccessoryVar[WarMaul, $Weight] = 7;
//.................................................................................
$AccessoryVar[PickAxe, $Weight] = 4;
$AccessoryVar[Knife, $Weight] = 1;
$AccessoryVar[Dagger, $Weight] = 3;
$AccessoryVar[ShortSword, $Weight] = 5;
$AccessoryVar[Spear, $Weight] = 6;
$AccessoryVar[Gladius, $Weight] = 5;
$AccessoryVar[Trident, $Weight] = 6;
$AccessoryVar[Rapier, $Weight] = 5;
$AccessoryVar[AwlPike, $Weight] = 8;
//.................................................................................
$AccessoryVar[Sling, $Weight] = 2;
$AccessoryVar[ShortBow, $Weight] = 3;
$AccessoryVar[LightCrossbow, $Weight] = 6;
$AccessoryVar[LongBow, $Weight] = 5;
$AccessoryVar[CompositeBow, $Weight] = 4;
$AccessoryVar[RepeatingCrossbow, $Weight] = 3;
$AccessoryVar[ElvenBow, $Weight] = 3;
$AccessoryVar[AeolusWing, $Weight] = 2;
$AccessoryVar[HeavyCrossbow, $Weight] = 8;
//.................................................................................
$AccessoryVar[SmallRock, $Weight] = "0.2";
$AccessoryVar[BasicArrow, $Weight] = "0.1";
$AccessoryVar[SheafArrow, $Weight] = "0.1";
$AccessoryVar[BladedArrow, $Weight] = "0.1";
$AccessoryVar[LightQuarrel, $Weight] = "0.1";
$AccessoryVar[HeavyQuarrel, $Weight] = "0.2";
$AccessoryVar[ShortQuarrel, $Weight] = "0.1";
$AccessoryVar[CastingBlade, $Weight] = "0.5";
$AccessoryVar[StoneFeather, $Weight] = "0.1";
$AccessoryVar[MetalFeather, $Weight] = "0.1";
$AccessoryVar[Talon, $Weight] = "0.2";
$AccessoryVar[CeraphumsFeather, $Weight] = "0.08";

$AccessoryVar[Hatchet, $MiscInfo] = "A hatchet";
$AccessoryVar[BroadSword, $MiscInfo] = "A broad sword";
$AccessoryVar[WarAxe, $MiscInfo] = "A war axe";
$AccessoryVar[LongSword, $MiscInfo] = "A long sword";
$AccessoryVar[BattleAxe, $MiscInfo] = "A battle axe";
$AccessoryVar[BastardSword, $MiscInfo] = "A bastard sword";
$AccessoryVar[Halberd, $MiscInfo] = "A halberd";
$AccessoryVar[Claymore, $MiscInfo] = "A claymore";
$AccessoryVar[Club, $MiscInfo] = "A club";
$AccessoryVar[SpikedClub, $MiscInfo] = "A spiked club";
$AccessoryVar[Mace, $MiscInfo] = "A mace";
$AccessoryVar[HammerPick, $MiscInfo] = "A hammer pick";
$AccessoryVar[WarHammer, $MiscInfo] = "A war hammer";
$AccessoryVar[WarMaul, $MiscInfo] = "A war maul";
$AccessoryVar[QuarterStaff, $MiscInfo] = "A quarter staff";
$AccessoryVar[LongStaff, $MiscInfo] = "A long staff";
$AccessoryVar[JusticeStaff, $MiscInfo] = "A Justice long staff";
$AccessoryVar[Knife, $MiscInfo] = "A knife";
$AccessoryVar[Dagger, $MiscInfo] = "A dagger";
$AccessoryVar[ShortSword, $MiscInfo] = "A short sword";
$AccessoryVar[Spear, $MiscInfo] = "A spear";
$AccessoryVar[Gladius, $MiscInfo] = "A gladius";
$AccessoryVar[Trident, $MiscInfo] = "A trident";
$AccessoryVar[Rapier, $MiscInfo] = "A rapier";
$AccessoryVar[AwlPike, $MiscInfo] = "An awl pike";
$AccessoryVar[PickAxe, $MiscInfo] = "A pick axe";
$AccessoryVar[Sling, $MiscInfo] = "A sling";
$AccessoryVar[ShortBow, $MiscInfo] = "A short bow";
$AccessoryVar[LongBow, $MiscInfo] = "A long bow";
$AccessoryVar[ElvenBow, $MiscInfo] = "An elven bow";
$AccessoryVar[CompositeBow, $MiscInfo] = "A composite bow";
$AccessoryVar[LightCrossbow, $MiscInfo] = "A light crossbow";
$AccessoryVar[HeavyCrossbow, $MiscInfo] = "A heavy crossbow";
$AccessoryVar[RepeatingCrossbow, $MiscInfo] = "A repeating crossbow";
$AccessoryVar[SmallRock, $MiscInfo] = "A small rock";
$AccessoryVar[BasicArrow, $MiscInfo] = "A basic arrow";
$AccessoryVar[SheafArrow, $MiscInfo] = "A sheaf arrow";
$AccessoryVar[BladedArrow, $MiscInfo] = "A bladed arrow";
$AccessoryVar[LightQuarrel, $MiscInfo] = "A light quarrel";
$AccessoryVar[HeavyQuarrel, $MiscInfo] = "A heavy quarrel";
$AccessoryVar[ShortQuarrel, $MiscInfo] = "A heavy quarrel";
$AccessoryVar[CastingBlade, $MiscInfo] = "Selects the best spell and casts it.  Used only for bots.";
$AccessoryVar[KeldriniteLS, $MiscInfo] = "The Keldrinite LongSword is one of the rarest and most powerful weapons in the world of Tribes RPG.";
$AccessoryVar[AeolusWing, $MiscInfo] = "Aeolus's wing is a mystical bow with the power of wind";
$AccessoryVar[StoneFeather, $MiscInfo] = "A feather made of stone";
$AccessoryVar[MetalFeather, $MiscInfo] = "A Sharp metal feather. Beautifully crafted";
$AccessoryVar[Talon, $MiscInfo] = "A gemmed talon. It is terribly sharp";
$AccessoryVar[CeraphumsFeather, $MiscInfo] = "Said to have come from the wing of a ceraphum. But we all knew that it came from the forge";
$AccessoryVar[BoneClub, $MiscInfo] = "A club made made of skeleton bones";
$AccessoryVar[SpikedBoneClub, $MiscInfo] = "A spiked club made of skeleton bones";

//NOTE: See shopping.cs for the shopIndexes

$SkillType[Hatchet] = $SkillSlashing;
$SkillType[BroadSword] = $SkillSlashing;
$SkillType[WarAxe] = $SkillSlashing;
$SkillType[LongSword] = $SkillSlashing;
$SkillType[BattleAxe] = $SkillSlashing;
$SkillType[BastardSword] = $SkillSlashing;
$SkillType[Halberd] = $SkillSlashing;
$SkillType[Claymore] = $SkillSlashing;
$SkillType[Club] = $SkillBludgeoning;
$SkillType[SpikedClub] = $SkillBludgeoning;
$SkillType[Mace] = $SkillBludgeoning;
$SkillType[HammerPick] = $SkillBludgeoning;
$SkillType[WarHammer] = $SkillBludgeoning;
$SkillType[WarMaul] = $SkillBludgeoning;
$SkillType[QuarterStaff] = $SkillBludgeoning;
$SkillType[LongStaff] = $SkillBludgeoning;
$SkillType[JusticeStaff] = $SkillBludgeoning;
$SkillType[Knife] = $SkillPiercing;
$SkillType[Dagger] = $SkillPiercing;
$SkillType[ShortSword] = $SkillPiercing;
$SkillType[Spear] = $SkillPiercing;
$SkillType[Gladius] = $SkillPiercing;
$SkillType[Trident] = $SkillPiercing;
$SkillType[Rapier] = $SkillPiercing;
$SkillType[AwlPike] = $SkillPiercing;
$SkillType[PickAxe] = $SkillPiercing;
$SkillType[Sling] = $SkillArchery;
$SkillType[ShortBow] = $SkillArchery;
$SkillType[LongBow] = $SkillArchery;
$SkillType[ElvenBow] = $SkillArchery;
$SkillType[CompositeBow] = $SkillArchery;
$SkillType[LightCrossbow] = $SkillArchery;
$SkillType[HeavyCrossbow] = $SkillArchery;
$SkillType[RepeatingCrossbow] = $SkillArchery;
$SkillType[SmallRock] = $SkillArchery;
$SkillType[BasicArrow] = $SkillArchery;
$SkillType[SheafArrow] = $SkillArchery;
$SkillType[BladedArrow] = $SkillArchery;
$SkillType[LightQuarrel] = $SkillArchery;
$SkillType[HeavyQuarrel] = $SkillArchery;
$SkillType[ShortQuarrel] = $SkillArchery;
$SkillType[CastingBlade] = $SkillPiercing;
$SkillType[KeldriniteLS] = $SkillSlashing;
$SkillType[AeolusWing] = $SkillArchery;
$SkillType[StoneFeather] = $SkillArchery;
$SkillType[MetalFeather] = $SkillArchery;
$SkillType[Talon] = $SkillArchery;
$SkillType[CeraphumsFeather] = $SkillArchery;
$SkillType[BoneClub] = $SkillBludgeoning;
$SkillType[SpikedBoneClub] = $SkillBludgeoning;

$WeaponRange[Sling] = 35;
$WeaponRange[ShortBow] = 120;
$WeaponRange[LongBow] = 200;
$WeaponRange[ElvenBow] = 260;
$WeaponRange[CompositeBow] = 360;
$WeaponRange[LightCrossbow] = 300;
$WeaponRange[AeolusWing] = 400;
$WeaponRange[HeavyCrossbow] = 500;
$WeaponRange[RepeatingCrossbow] = 280;
$WeaponRange[CastingBlade] = 1000;	//will swing from anywhere...BUT will be able to snipe with beam

$WeaponDelay[Sling] = 1.5;

$ProjRestrictions[SmallRock] = ",Sling,";
$ProjRestrictions[BasicArrow] = ",ShortBow,LongBow,ElvenBow,CompositeBow,RShortBow,";
$ProjRestrictions[SheafArrow] = ",ShortBow,LongBow,ElvenBow,CompositeBow,RShortBow,";
$ProjRestrictions[BladedArrow] = ",ShortBow,LongBow,ElvenBow,CompositeBow,RShortBow,";
$ProjRestrictions[LightQuarrel] = ",LightCrossbow,HeavyCrossbow,RLightCrossbow,";
$ProjRestrictions[HeavyQuarrel] = ",LightCrossbow,HeavyCrossbow,RLightCrossbow,";
$ProjRestrictions[ShortQuarrel] = ",RepeatingCrossbow,";
$ProjRestrictions[StoneFeather] = ",AeolusWing,";
$ProjRestrictions[MetalFeather] = ",AeolusWing,";
$ProjRestrictions[Talon] = ",AeolusWing,";
$ProjRestrictions[CeraphumsFeather] = ",AeolusWing,";

function GenerateAllWeaponCosts()
{
	dbecho($dbechoMode, "GenerateAllWeaponCosts()");

	//All item costs that need to be Generated must be in a function, later called after all files have been exec'd.
	//This function, among other similar ones, is run once only in server.cs.

	$ItemCost[Hatchet] = GenerateItemCost(Hatchet);
	$ItemCost[BroadSword] = GenerateItemCost(BroadSword);
	$ItemCost[WarAxe] = GenerateItemCost(WarAxe);
	$ItemCost[LongSword] = GenerateItemCost(LongSword);
	$ItemCost[BattleAxe] = GenerateItemCost(BattleAxe);
	$ItemCost[BastardSword] = GenerateItemCost(BastardSword);
	$ItemCost[Halberd] = GenerateItemCost(Halberd);
	$ItemCost[Claymore] = GenerateItemCost(Claymore);
	$ItemCost[Club] = GenerateItemCost(Club);
	$ItemCost[SpikedClub] = GenerateItemCost(SpikedClub);
	$ItemCost[Mace] = GenerateItemCost(Mace);
	$ItemCost[HammerPick] = GenerateItemCost(HammerPick);
	$ItemCost[WarHammer] = GenerateItemCost(WarHammer);
	$ItemCost[WarMaul] = GenerateItemCost(WarMaul);
	$ItemCost[QuarterStaff] = GenerateItemCost(QuarterStaff);
	$ItemCost[LongStaff] = GenerateItemCost(LongStaff);
	$ItemCost[JusticeStaff] = GenerateItemCost(JusticeStaff);
	$ItemCost[Knife] = GenerateItemCost(Knife);
	$ItemCost[Dagger] = GenerateItemCost(Dagger);
	$ItemCost[ShortSword] = GenerateItemCost(ShortSword);
	$ItemCost[Spear] = GenerateItemCost(Spear);
	$ItemCost[Gladius] = GenerateItemCost(Gladius);
	$ItemCost[Trident] = GenerateItemCost(Trident);
	$ItemCost[Rapier] = GenerateItemCost(Rapier);
	$ItemCost[AwlPike] = GenerateItemCost(AwlPike);
	$ItemCost[PickAxe] = GenerateItemCost(PickAxe);
	$ItemCost[Sling] = GenerateItemCost(Sling);
	$ItemCost[ShortBow] = GenerateItemCost(ShortBow);
	$ItemCost[LongBow] = GenerateItemCost(LongBow);
	$ItemCost[ElvenBow] = GenerateItemCost(ElvenBow);
	$ItemCost[CompositeBow] = GenerateItemCost(CompositeBow);
	$ItemCost[LightCrossbow] = GenerateItemCost(LightCrossbow);
	$ItemCost[HeavyCrossbow] = GenerateItemCost(HeavyCrossbow);
	$ItemCost[RepeatingCrossbow] = GenerateItemCost(RepeatingCrossbow);
	$ItemCost[BasicArrow] = GenerateItemCost(BasicArrow);
	$ItemCost[SheafArrow] = GenerateItemCost(SheafArrow);
	$ItemCost[BladedArrow] = GenerateItemCost(BladedArrow);
	$ItemCost[LightQuarrel] = GenerateItemCost(LightQuarrel);
	$ItemCost[HeavyQuarrel] = GenerateItemCost(HeavyQuarrel);
	$ItemCost[ShortQuarrel] = GenerateItemCost(ShortQuarrel);
	$ItemCost[CastingBlade] = 0;
	$ItemCost[KeldriniteLS] = GenerateItemCost(KeldriniteLS);
	$ItemCost[AeolusWing] = GenerateItemCost(AeolusWing);
	$ItemCost[StoneFeather] = GenerateItemCost(StoneFeather);
	$ItemCost[MetalFeather] = GenerateItemCost(MetalFeather);
	$ItemCost[Talon] = GenerateItemCost(Talon);
	$ItemCost[CeraphumsFeather] = GenerateItemCost(CeraphumsFeather);
	$ItemCost[BoneClub] = GenerateItemCost(BoneClub);
	$ItemCost[SpikedBoneClub] = GenerateItemCost(SpikedBoneClub);

	$ItemCost[RHatchet] = round($ItemCost[Hatchet] * $RustyCostAmp);
	$ItemCost[RBroadSword] = round($ItemCost[BroadSword] * $RustyCostAmp);
	$ItemCost[RLongSword] = round($ItemCost[LongSword] * $RustyCostAmp);
	$ItemCost[RClub] = round($ItemCost[Club] * $RustyCostAmp);
	$ItemCost[RSpikedClub] = round($ItemCost[SpikedClub] * $RustyCostAmp);
	$ItemCost[RKnife] = round($ItemCost[Knife] * $RustyCostAmp);
	$ItemCost[RDagger] = round($ItemCost[Dagger] * $RustyCostAmp);
	$ItemCost[RShortSword] = round($ItemCost[ShortSword] * $RustyCostAmp);
	$ItemCost[RPickAxe] = round($ItemCost[PickAxe] * $RustyCostAmp);
	$ItemCost[RShortBow] = round($ItemCost[ShortBow] * $RustyCostAmp);
	$ItemCost[RLightCrossbow] = round($ItemCost[LightCrossbow] * $RustyCostAmp);
	$ItemCost[RWarAxe] = round($ItemCost[WarAxe] * $RustyCostAmp);
}

//****************************************************************************************************

function MeleeAttack(%player, %length, %weapon)
{
	dbecho($dbechoMode, "MeleeAttack(" @ %player @ ", " @ %length @ ")");

	%clientId = Player::getClient(%player);
	if(%clientId == "")
		%clientId = 0;
	if(%clientId.sleepMode > 0)		return;

	//==== ANTI-SPAM CHECK, CAUSE FOR SPAM UNKNOWN ==========
	//%time = getIntegerTime(true) >> 5;
	//if(%time - %clientId.lastFireTime <= $fireTimeDelay)
	//	return;
	//%clientId.lastFireTime = %time;
	//=======================================================
	if($WeaponDelay[%weapon] != ""){		if($justmeleed[%clientId])			return;	}	else		$WeaponDelay[%weapon] = GetDelay(%weapon);	$justmeleed[%clientId] = True;	schedule("$justmeleed["@%clientId@"]=\"\";",$WeaponDelay[%weapon]-0.11);

	$los::object = "";
	if(GameBase::getLOSinfo(%player, %length))
	{
		%target = $los::object;
		%obj = getObjectType(%target);
		if(%obj == "Player")
		{
			GameBase::virtual($los::object, "onDamage", "", 1.0, "0 0 0", "0 0 0", "0 0 0", "torso", "front_right", %clientId, %weapon);
		}
		else if(%target.isIce){
			ice::hit(%clientId,%target);
		}
	}

	PostAttack(%clientId, %weapon);
}

function ProjectileAttack(%clientId, %weapon, %vel)
{
	dbecho($dbechoMode, "ProjectileAttack(" @ %clientId @ ", " @ %weapon @ ", " @ %vel @ ")");

	//==== ANTI-SPAM CHECK, CAUSE FOR SPAM UNKNOWN ==========
	//%time = getIntegerTime(true) >> 5;
	//if(%time - %clientId.lastFireTime <= $fireTimeDelay)
	//	return;
	//%clientId.lastFireTime = %time;
	//=======================================================
	if(%clientId.sleepMode > 0)		return;	if($WeaponDelay[%weapon] != ""){		if($justRanged[%clientId])			return;	}	else		$WeaponDelay[%weapon] = GetDelay(%weapon);	$justRanged[%clientId] = True;	schedule("$justRanged["@%clientId@"]=\"\";",$WeaponDelay[%weapon]-0.11);


	%loadedProjectile = fetchData(%clientId, "LoadedProjectile " @ %weapon);
	if(%loadedProjectile == ""){		if(!Player::isAiControlled(%clientId)){			processMenuselectrweapon(%clientId, %weapon);		}		return;
	}
	if(belt::hasthisstuff(%clientId, %loadedProjectile) <= 0)		return;

	%zoffset = 0.44;

	%arrow = newObject("", "Item", %loadedProjectile, 1, false);
	%arrow.owner = %clientId;
	%arrow.delta = 1;
	%arrow.weapon = %weapon;

	addToSet("MissionCleanup", %arrow);
  	schedule("Item::Pop(" @ %arrow @ ");", 30, %arrow);

	//double-check stuff
	$ProjectileDoubleCheck[%arrow] = True;
	schedule("$ProjectileDoubleCheck[" @ %arrow @ "] = \"\";", 1.5, %arrow);

	%rot = GameBase::getRotation(%clientId);
	%newrot = (GetWord(%rot, 0) - %zoffset) @ " " @ GetWord(%rot, 1) @ " " @ GetWord(%rot, 2);

	GameBase::setRotation(%clientId, %newrot);
	GameBase::throw(%arrow, Client::getOwnedObject(%clientId), %vel, false);
	GameBase::setRotation(%arrow, %rot);
	GameBase::setRotation(%clientId, %rot);

	belt::takethisstuff(%clientId, %loadedProjectile, 1);

	PostAttack(%clientId, %weapon);
}

function PickAxeSwing(%player, %length, %weapon)
{
	dbecho($dbechoMode, "PickAxeSwing(" @ %player @ ", " @ %length @ ")");

	%clientId = Player::getClient(%player);
	if(%clientId == "")
		%clientId = 0;

	//==== ANTI-SPAM CHECK, CAUSE FOR SPAM UNKNOWN ==========
	//%time = getIntegerTime(true) >> 5;
	//if(%time - %clientId.lastFireTime <= $fireTimeDelay)
	//	return;
	//%clientId.lastFireTime = %time;
	//=======================================================
	if($WeaponDelay[%weapon] != ""){		if($justmeleed[%clientId])			return;	}	else		$WeaponDelay[%weapon] = GetDelay(%weapon);	$justmeleed[%clientId] = True;	schedule("$justmeleed["@%clientId@"]=\"\";",$WeaponDelay[%weapon]-0.11);

	$los::object = "";
	if(GameBase::getLOSinfo(%player, %length))
	{
		%target = $los::object;
		%obj = getObjectType(%target);
		%type = GameBase::getDataName(%target);

		if(%type == "Crystal")
		{
			%brflag = String::findSubStr(fetchData(%clientId, "RACE"), "Human");	//must be human to mine
			if(Vector::getDistance(%clientId.lastMinePos, GameBase::getPosition(%clientId)) > 1.0 && %brflag != -1)
			{
				playSound(SoundHitore, GameBase::getPosition(%target));	//vectrex, modified by JI

				%score = DoRandomMining(%clientId, %target);
				if(%score != "")
				{
					givethisstuff(%clientId, %score@" 1");
					RefreshAll(%clientId);
					%desc = %score.description;
					if(%desc == False){
						%desc = $beltitem[%score, "Name"];
						%beltTag = " ["@getDisp($beltitem[%score, "Type"])@", have "@belt::hasthisstuff(%clientId, %score)@"]";
					}
					Client::sendMessage(%clientId, 0, "You found " @ %desc @ "." @ %beltTag);

					if( floor(getRandom() * 10) == 5)
						%clientId.lastMinePos = GameBase::getPosition(%clientId);
				}
				UseSkill(%clientId, $SkillMining, True, True);
			}
			else
				playSound(SoundHitore2, GameBase::getPosition(%target));
		}
		else if(%target.isIce){
			ice::hit(%clientId,%target);
		}
		else if(%obj == "Player")
			GameBase::virtual(%target, "onDamage", "", 1.0, "0 0 0", "0 0 0", "0 0 0", "torso", "front_right", %clientId, %weapon);
	}

	PostAttack(%clientId, %weapon);
}function ice::hit(%client, %target){	if(%target < 1)		return;	deleteobject(%target);}

function PostAttack(%clientId, %weapon)
{
	dbecho($dbechoMode, "PostAttack(" @ %clientId @ ", " @ %weapon @ ")");

	if($postAttackGraphBar)
	{
		%t = GetDelay(%weapon);
		%ticks = 30;
		%chunks = 10;

		%chunklen = floor(%ticks / %chunks);
		%d = %t / %chunks;

		for(%i = 0; %i <= %chunks; %i++)
			schedule("bottomprint(" @ %clientId @ ", \" \" @ String::create(\"*\", " @ %ticks @ " - (" @ %chunklen @ " * " @ %i @ ")) @ \"\", " @ %d @ " + 0.25);", %d * %i);
	}

	if(%weapon == CastingBlade)
	{
		%x = floor(%clientId.castingBladeBeat);
		if(%x != 0)
		{
			if(%x == 1)
				playSound(MClip5, GameBase::getPosition(%clientId));
			else if(%x == 2)
				playSound(MClip6, GameBase::getPosition(%clientId));
		}

		%x++;
		if(%x > 2) %x = 1;

		%clientId.castingBladeBeat = %x;
	}
}

function DoRandomMining(%clientId, %crystal)
{
	dbecho($dbechoMode, "DoRandomMining(" @ %clientId @ ", " @ %crystal @ ")");

	%lastscore = "";
	for(%i = 1; $ItemList[Mining, %i] != ""; %i++)
	{
		%w1 = GetWord($ItemList[Mining, %i], 1) - %crystal.bonus[%i];
		%n = Cap( (%w1 * getRandom()) + (%w1 / 2), 0, %w1);
		%r = 1 + ($PlayerSkill[%clientId, $SkillMining] * (1/10)) * getRandom();

		if(%n > %r)
			return %lastscore;

		%lastscore = GetWord($ItemList[Mining, %i], 0);
	}
	return %lastscore;
}

function GetRange(%weapon)
{
	dbecho($dbechoMode, "GetRange(" @ %weapon @ ")");

	%minRange = 2.0;
	if($WeaponRange[%weapon] != "")
		return %minRange + $WeaponRange[%weapon];
	else
		return %minRange + $RangeTable[$AccessoryVar[%weapon, $AccessoryType]];
}
function GetDelay(%weapon)
{
	dbecho($dbechoMode, "GetDelay(" @ %weapon @ ")");

	if($WeaponDelay[%weapon] != "")
		return $WeaponDelay[%weapon];
	else
	{
		%a = 3.0;
		%b = Cap($AccessoryVar[%weapon, $Weight] / %a, 1.0, "inf");
		%c = %b * $DelayFactorTable[$AccessoryVar[%weapon, $AccessoryType]];
		return %c;
	}
}

function GenerateItemCost(%item)
{
	dbecho($dbechoMode, "GenerateItemCost(" @ %item @ ")");

	if($HardcodedItemCost[%item] != "")
		return $HardcodedItemCost[%item];

	%cft = $CostFactorTable[$AccessoryVar[%item, $AccessoryType]];

	%a = GetDelay(%item);
	if(floor(%a) == 0)
		%a = 1.0;

	%b6 = AddItemSpecificPoints(%item, "6") * 1.2;	//ATK
	%b7 = AddItemSpecificPoints(%item, "7") / 6;	//DEF
	%b3 = AddItemSpecificPoints(%item, "3") / 6;	//MDEF

	%extracost = 0;
	for(%i = 1; $SmithCombo[%i] != ""; %i++)
	{
		for(%j = 0; (%w = GetWord($SmithComboResult[%i], %j)) != -1; %j+=2)
		{
			if(String::ICompare(%item, %w) == 0)
			{
				%n = GetWord($SmithComboResult[%i], %j+1);
				for(%k = 0; (%w2 = GetWord($SmithCombo[%i], %k)) != -1; %k+=2)
				{
					%n2 = GetWord($SmithCombo[%i], %k+1);
					%extracost += (GenerateItemCost(%w2) * %n2);
				}
				%extracost *= %n;
				break;
			}
		}
		if(%extracost > 0)
			break;
	}
	%extracost = %extracost * ($ResalePercentage / 100);
	
	%c = (%b6 + %b7 + %b3) / %a;
	%d = Cap(0.01 * pow(%c, 3.7), 0, "inf");
	%e = Cap(%d * %cft, 1, "inf");
	%f = floor(%e + %extracost);

	return %f;
}

//****************************************************************************************************
//   KNIFE
//****************************************************************************************************

ItemImageData KnifeImage
{
	shapeFile  = "dagger";
	mountPoint = 0;

	weaponType = 0;
	reloadTime = 0;
	fireTime = GetDelay(Knife);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing1;
	sfxActivate = AxeSlash2;
};
ItemData Knife
{
	heading = "bWeapons";
	description = "Knife";
	className = "Weapon";
	shapeFile  = "dagger";
	hudIcon = "dagger";
	shadowDetailMask = 4;
	imageType = KnifeImage;
	price = 0;
	showWeaponBar = true;
};
function KnifeImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(Knife), Knife);
}

//****************************************************************************************************
//   DAGGER
//****************************************************************************************************

ItemImageData DaggerImage
{
	shapeFile  = "dagger";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(Dagger);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing1;
	sfxActivate = AxeSlash2;
};
ItemData Dagger
{
	heading = "bWeapons";
	description = "Dagger";
	className = "Weapon";
	shapeFile  = "dagger";
	hudIcon = "dagger";
	shadowDetailMask = 4;
	imageType = DaggerImage;
	price = 0;
	showWeaponBar = true;
};
function DaggerImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(Dagger), Dagger);
}

//****************************************************************************************************
//   GLADIUS
//****************************************************************************************************

ItemImageData GladiusImage
{
	shapeFile  = "sword";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(Gladius);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing6;
	sfxActivate = AxeSlash2;
};
ItemData Gladius
{
	heading = "bWeapons";
	description = "Gladius";
	className = "Weapon";
	shapeFile  = "sword";
	hudIcon = "blaster";
	shadowDetailMask = 4;
	imageType = GladiusImage;
	price = 0;
	showWeaponBar = true;
};
function GladiusImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(Gladius), Gladius);
}

//****************************************************************************************************
//   SHORT SWORD
//****************************************************************************************************

ItemImageData ShortswordImage
{
	shapeFile  = "short_sword";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(Shortsword);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing2;
	sfxActivate = AxeSlash2;
};
ItemData Shortsword
{
	heading = "bWeapons";
	description = "Short Sword";
	className = "Weapon";
	shapeFile  = "short_sword";
	hudIcon = "blaster";
	shadowDetailMask = 4;
	imageType = ShortswordImage;
	price = 0;
	showWeaponBar = true;
};
function ShortswordImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(Shortsword), Shortsword);
}

//****************************************************************************************************
//   BROAD SWORD
//****************************************************************************************************

ItemImageData BroadswordImage
{
	shapeFile  = "sword";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(Broadsword);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing5;
	sfxActivate = AxeSlash2;
};
ItemData Broadsword
{
	heading = "bWeapons";
	description = "Broad Sword";
	className = "Weapon";
	shapeFile  = "sword";
	hudIcon = "blaster";
	shadowDetailMask = 4;
	imageType = BroadswordImage;
	price = 0;
	showWeaponBar = true;
};
function BroadswordImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(Broadsword), Broadsword);
}

//****************************************************************************************************
//   LONG SWORD
//****************************************************************************************************

ItemImageData LongswordImage
{
	shapeFile  = "long_sword";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(Longsword);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing6;
	sfxActivate = AxeSlash2;
};
ItemData Longsword
{
	heading = "bWeapons";
	description = "Long Sword";
	className = "Weapon";
	shapeFile  = "long_sword";
	hudIcon = "blaster";
	shadowDetailMask = 4;
	imageType = LongswordImage;
	price = 0;
	showWeaponBar = true;
};
function LongswordImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(Longsword), Longsword);
}

//****************************************************************************************************
//   KELDRINITE LONG SWORD
//****************************************************************************************************

ItemImageData KeldriniteLSImage
{
	shapeFile  = "elfinblade";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(KeldriniteLS);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing2;
	sfxActivate = ActivateAS;
};
ItemData KeldriniteLS
{
	heading = "bWeapons";
	description = "Keldrinite Long Sword";
	className = "Weapon";
	shapeFile  = "elfinblade";
	hudIcon = "blaster";
	shadowDetailMask = 4;
	imageType = KeldriniteLSImage;
	price = 0;
	showWeaponBar = true;
};
function KeldriniteLSImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(KeldriniteLS), KeldriniteLS);
}

//****************************************************************************************************
//   BASTARD SWORD
//****************************************************************************************************

ItemImageData BastardswordImage
{
	shapeFile  = "sword";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(Bastardsword);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing6;
	sfxActivate = AxeSlash2;
};
ItemData Bastardsword
{
	heading = "bWeapons";
	description = "Bastard Sword";
	className = "Weapon";
	shapeFile  = "sword";
	hudIcon = "blaster";
	shadowDetailMask = 4;
	imageType = BastardswordImage;
	price = 0;
	showWeaponBar = true;
};
function BastardswordImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(Bastardsword), Bastardsword);
}

//****************************************************************************************************
//   RAPIER
//****************************************************************************************************

ItemImageData RapierImage
{
	shapeFile  = "katana";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(Rapier);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing3;
	sfxActivate = AxeSlash2;
};
ItemData Rapier
{
	heading = "bWeapons";
	description = "Rapier";
	className = "Weapon";
	shapeFile  = "katana";
	hudIcon = "katana";
	shadowDetailMask = 4;
	imageType = RapierImage;
	price = 0;
	showWeaponBar = true;
};
function RapierImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(Rapier), Rapier);
}

//****************************************************************************************************
//   CLAYMORE
//****************************************************************************************************

ItemImageData ClaymoreImage
{
	shapeFile  = "sword";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(Claymore);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing6;
	sfxActivate = AxeSlash2;
};
ItemData Claymore
{
	heading = "bWeapons";
	description = "Claymore";
	className = "Weapon";
	shapeFile  = "sword";
	hudIcon = "katana";
	shadowDetailMask = 4;
	imageType = ClaymoreImage;
	price = 0;
	showWeaponBar = true;
};
function ClaymoreImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(Claymore), Claymore);
}

//****************************************************************************************************
//   HATCHET
//****************************************************************************************************

ItemImageData HatchetImage
{
	shapeFile  = "hatchet";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(Hatchet);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing1;
	sfxActivate = AxeSlash2;
};
ItemData Hatchet
{
	heading = "bWeapons";
	description = "Hatchet";
	className = "Weapon";
	shapeFile  = "hatchet";
	hudIcon = "axe";
	shadowDetailMask = 4;
	imageType = HatchetImage;
	price = 0;
	showWeaponBar = true;
};
function HatchetImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(Hatchet), Hatchet);
}

//****************************************************************************************************
//   WAR AXE
//****************************************************************************************************

ItemImageData WarAxeImage
{
	shapeFile  = "axe";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(WarAxe);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing3;
	sfxActivate = AxeSlash2;
};
ItemData WarAxe
{
	heading = "bWeapons";
	description = "War Axe";
	className = "Weapon";
	shapeFile  = "axe";
	hudIcon = "axe";
	shadowDetailMask = 4;
	imageType = WarAxeImage;
	price = 0;
	showWeaponBar = true;
};
function WarAxeImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(WarAxe), WarAxe);
}

//****************************************************************************************************
//   PICK AXE
//****************************************************************************************************

ItemImageData PickAxeImage
{
	shapeFile = "Pick";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(PickAxe);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing1;
	sfxActivate = CrossbowSwitch1;
};
ItemData PickAxe
{
	heading = "bWeapons";
	description = "Pick Axe";
	className = "Weapon";
	shapeFile = "Pick";
	hudIcon = "pick";
	shadowDetailMask = 4;
	imageType = PickAxeImage;
	price = 0;
	showWeaponBar = true;
};
function PickAxeImage::onFire(%player, %slot)
{
	PickAxeSwing(%player, GetRange(PickAxe), PickAxe);
}

//****************************************************************************************************
//   HAMMER PICK
//****************************************************************************************************

ItemImageData HammerPickImage
{
	shapeFile = "Pick";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(HammerPick);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing1;
	sfxActivate = CrossbowSwitch1;
};
ItemData HammerPick
{
	heading = "bWeapons";
	description = "Hammer Pick";
	className = "Weapon";
	shapeFile = "Pick";
	hudIcon = "pick";
	shadowDetailMask = 4;
	imageType = HammerPickImage;
	price = 0;
	showWeaponBar = true;
};
function HammerPickImage::onFire(%player, %slot)
{
	PickAxeSwing(%player, GetRange(HammerPick), HammerPick);
}

//****************************************************************************************************
//   BATTLE AXE
//****************************************************************************************************

ItemImageData BattleAxeImage
{
	shapeFile  = "BattleAxe";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(BattleAxe);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing7;
	sfxActivate = AxeSlash2;
};
ItemData BattleAxe
{
	heading = "bWeapons";
	description = "Battle Axe";
	className = "Weapon";
	shapeFile  = "BattleAxe";
	hudIcon = "axe";
	shadowDetailMask = 4;
	imageType = BattleAxeImage;
	price = 0;
	showWeaponBar = true;
};
function BattleAxeImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(BattleAxe), BattleAxe);
}

//****************************************************************************************************
//   HALBERD
//****************************************************************************************************

ItemImageData HalberdImage
{
	shapeFile  = "sword";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(Halberd);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing7;
	sfxActivate = AxeSlash2;
};
ItemData Halberd
{
	heading = "bWeapons";
	description = "Halberd";
	className = "Weapon";
	shapeFile  = "sword";
	hudIcon = "spear";
	shadowDetailMask = 4;
	imageType = HalberdImage;
	price = 0;
	showWeaponBar = true;
};
function HalberdImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(Halberd), Halberd);
}

//****************************************************************************************************
//   SPEAR
//****************************************************************************************************

ItemImageData SpearImage
{
	shapeFile  = "spear";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(Spear);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing3;
	sfxActivate = AxeSlash2;
};
ItemData Spear
{
	heading = "bWeapons";
	description = "Spear";
	className = "Weapon";
	shapeFile  = "spear";
	hudIcon = "spear";
	shadowDetailMask = 4;
	imageType = SpearImage;
	price = 0;
	showWeaponBar = true;
};
function SpearImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(Spear), Spear);
}

//****************************************************************************************************
//   AWL PIKE
//****************************************************************************************************

ItemImageData AwlPikeImage
{
	shapeFile  = "spear";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(AwlPike);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing6;
	sfxActivate = AxeSlash2;
};
ItemData AwlPike
{
	heading = "bWeapons";
	description = "Awl Pike";
	className = "Weapon";
	shapeFile  = "spear";
	hudIcon = "trident";
	shadowDetailMask = 4;
	imageType = AwlPikeImage;
	price = 0;
	showWeaponBar = true;
};
function AwlPikeImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(AwlPike), AwlPike);
}

//****************************************************************************************************
//   TRIDENT
//****************************************************************************************************

ItemImageData TridentImage
{
	shapeFile  = "trident";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(Trident);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing3;
	sfxActivate = AxeSlash2;
};
ItemData Trident
{
	heading = "bWeapons";
	description = "Trident";
	className = "Weapon";
	shapeFile  = "trident";
	hudIcon = "trident";
	shadowDetailMask = 4;
	imageType = TridentImage;
	price = 0;
	showWeaponBar = true;
};
function TridentImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(Trident), Trident);
}

//****************************************************************************************************
//   CLUB
//****************************************************************************************************

ItemImageData ClubImage
{
	shapeFile  = "mace";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(Club);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing5;
	sfxActivate = AxeSlash2;
};
ItemData Club
{
	heading = "bWeapons";
	description = "Club";
	className = "Weapon";
	shapeFile  = "mace";
	hudIcon = "club";
	shadowDetailMask = 4;
	imageType = ClubImage;
	price = 0;
	showWeaponBar = true;
};
function ClubImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(Club), Club);
}

//****************************************************************************************************
//   SPIKED CLUB
//****************************************************************************************************

ItemImageData SpikedClubImage
{
	shapeFile  = "mace";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(SpikedClub);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing5;
	sfxActivate = AxeSlash2;
};
ItemData SpikedClub
{
	heading = "bWeapons";
	description = "Spiked Club";
	className = "Weapon";
	shapeFile  = "mace";
	hudIcon = "sclub";
	shadowDetailMask = 4;
	imageType = SpikedClubImage;
	price = 0;
	showWeaponBar = true;
};
function SpikedClubImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(SpikedClub), SpikedClub);
}

//****************************************************************************************************
//   MACE
//****************************************************************************************************

ItemImageData MaceImage
{
	shapeFile  = "mace";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(Mace);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing6;
	sfxActivate = AxeSlash2;
};
ItemData Mace
{
	heading = "bWeapons";
	description = "Mace";
	className = "Weapon";
	shapeFile  = "mace";
	hudIcon = "mace";
	shadowDetailMask = 4;
	imageType = MaceImage;
	price = 0;
	showWeaponBar = true;
};
function MaceImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(Mace), Mace);
}

//****************************************************************************************************
//   WAR HAMMER
//****************************************************************************************************

ItemImageData WarHammerImage
{
	shapeFile  = "hammer";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(WarHammer);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing6;
	sfxActivate = AxeSlash2;
};
ItemData WarHammer
{
	heading = "bWeapons";
	description = "War Hammer";
	className = "Weapon";
	shapeFile  = "hammer";
	hudIcon = "hammer";
	shadowDetailMask = 4;
	imageType = WarHammerImage;
	price = 0;
	showWeaponBar = true;
};
function WarHammerImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(WarHammer), WarHammer);
}

//****************************************************************************************************
//   WAR MAUL
//****************************************************************************************************

ItemImageData WarMaulImage
{
	shapeFile  = "hammer";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(WarMaul);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing7;
	sfxActivate = AxeSlash2;
};
ItemData WarMaul
{
	heading = "bWeapons";
	description = "War Maul";
	className = "Weapon";
	shapeFile  = "hammer";
	hudIcon = "hammer";
	shadowDetailMask = 4;
	imageType = WarMaulImage;
	price = 0;
	showWeaponBar = true;
};
function WarMaulImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(WarMaul), WarMaul);
}

//****************************************************************************************************
//   QUARTER STAFF
//****************************************************************************************************

ItemImageData QuarterStaffImage
{
	shapeFile  = "quarterstaff";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(QuarterStaff);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing3;
	sfxActivate = AxeSlash2;
};
ItemData QuarterStaff
{
	heading = "bWeapons";
	description = "Quarter Staff";
	className = "Weapon";
	shapeFile  = "quarterstaff";
	hudIcon = "spear";
	shadowDetailMask = 4;
	imageType = QuarterStaffImage;
	price = 0;
	showWeaponBar = true;
};
function QuarterStaffImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(QuarterStaff), QuarterStaff);
}

//****************************************************************************************************
//   LONG STAFF
//****************************************************************************************************

ItemImageData LongStaffImage
{
	shapeFile  = "longstaff";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(LongStaff);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing3;
	sfxActivate = AxeSlash2;
};
ItemData LongStaff
{
	heading = "bWeapons";
	description = "Long Staff";
	className = "Weapon";
	shapeFile  = "longstaff";
	hudIcon = "spear";
	shadowDetailMask = 4;
	imageType = LongStaffImage;
	price = 0;
	showWeaponBar = true;
};
function LongStaffImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(LongStaff), LongStaff);
}

//****************************************************************************************************
//   JUSTICE STAFF
//****************************************************************************************************

ItemImageData JusticeStaffImage
{
	shapeFile  = "longstaff";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(JusticeStaff);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing4;
	sfxActivate = AxeSlash2;
};
ItemData JusticeStaff
{
	heading = "bWeapons";
	description = "Justice Staff";
	className = "Weapon";
	shapeFile  = "quarterstaff";
	hudIcon = "spear";
	shadowDetailMask = 4;
	imageType = JusticeStaffImage;
	price = 0;
	showWeaponBar = true;
};
function JusticeStaffImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(JusticeStaff), JusticeStaff);
}

//****************************************************************************************************
//   BONE CLUB
//****************************************************************************************************

ItemImageData BoneClubImage
{
	shapeFile  = "mace";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(BoneClub);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing5;
	sfxActivate = AxeSlash2;
};
ItemData BoneClub
{
	heading = "bWeapons";
	description = "Bone Club";
	className = "Weapon";
	shapeFile  = "mace";
	hudIcon = "club";
	shadowDetailMask = 4;
	imageType = BoneClubImage;
	price = 0;
	showWeaponBar = true;
};
function BoneClubImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(BoneClub), BoneClub);
}

//****************************************************************************************************
//   SPIKED BONE CLUB
//****************************************************************************************************

ItemImageData SpikedBoneClubImage
{
	shapeFile  = "mace";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(SpikedBoneClub);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing5;
	sfxActivate = AxeSlash2;
};
ItemData SpikedBoneClub
{
	heading = "bWeapons";
	description = "Spiked Bone Club";
	className = "Weapon";
	shapeFile  = "mace";
	hudIcon = "sclub";
	shadowDetailMask = 4;
	imageType = SpikedBoneClubImage;
	price = 0;
	showWeaponBar = true;
};
function SpikedBoneClubImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(SpikedBoneClub), SpikedBoneClub);
}

//****************************************************************************************************
//   SLING
//****************************************************************************************************

ItemImageData SlingImage
{
	shapeFile = "crossbow";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	ammoType = "";
	projectileType = NoProjectile;
	accuFire = false;
	reloadTime = 0;
	fireTime = GetDelay(Sling);

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = CrossbowShoot1;
	sfxActivate = CrossbowSwitch1;
	sfxReload = NoSound;
};
ItemData Sling
{
	description = "Sling";
	className = "Weapon";
	shapeFile = "crossbow";
	hudIcon = "grenade";
	heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = SlingImage;
	price = 0;
	showWeaponBar = true;
};
function SlingImage::onFire(%player, %slot)
{
	%clientId = Player::getClient(%player);

	%vel = 60;
	ProjectileAttack(%clientId, Sling, %vel);
}

//****************************************************************************************************
//   SHORT BOW
//****************************************************************************************************

ItemImageData ShortBowImage
{
	shapeFile = "longbow";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	ammoType = "";
	projectileType = NoProjectile;
	accuFire = false;
	reloadTime = 0;
	fireTime = GetDelay(ShortBow);

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = CrossbowShoot1;
	sfxActivate = CrossbowSwitch1;
	sfxReload = NoSound;
};
ItemData ShortBow
{
	description = "Short Bow";
	className = "Weapon";
	shapeFile = "longbow";
	hudIcon = "bow";
	heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = ShortBowImage;
	price = 0;
	showWeaponBar = true;
};
function ShortBowImage::onFire(%player, %slot)
{
	%clientId = Player::getClient(%player);

	%vel = 100;
	ProjectileAttack(%clientId, ShortBow, %vel);
}

//****************************************************************************************************
//   LONG BOW
//****************************************************************************************************

ItemImageData LongBowImage
{
	shapeFile = "longbow";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	ammoType = "";
	projectileType = NoProjectile;
	accuFire = false;
	reloadTime = 0;
	fireTime = GetDelay(LongBow);

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = CrossbowShoot1;
	sfxActivate = CrossbowSwitch1;
	sfxReload = NoSound;
};
ItemData LongBow
{
	description = "Long Bow";
	className = "Weapon";
	shapeFile = "longbow";
	hudIcon = "bow";
	heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = LongBowImage;
	price = 0;
	showWeaponBar = true;
};
function LongBowImage::onFire(%player, %slot)
{
	%clientId = Player::getClient(%player);

	%vel = 100;
	ProjectileAttack(%clientId, LongBow, %vel);
}

//****************************************************************************************************
//   ELVEN BOW
//****************************************************************************************************

ItemImageData ElvenBowImage
{
	shapeFile = "longbow";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	ammoType = "";
	projectileType = NoProjectile;
	accuFire = false;
	reloadTime = 0;
	fireTime = GetDelay(ElvenBow);

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = CrossbowShoot1;
	sfxActivate = CrossbowSwitch1;
	sfxReload = NoSound;
};
ItemData ElvenBow
{
	description = "Elven Bow";
	className = "Weapon";
	shapeFile = "longbow";
	hudIcon = "bow";
	heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = ElvenBowImage;
	price = 0;
	showWeaponBar = true;
};
function ElvenBowImage::onFire(%player, %slot)
{
	%clientId = Player::getClient(%player);

	%vel = 100;
	ProjectileAttack(%clientId, ElvenBow, %vel);
}

//****************************************************************************************************
//   COMPOSITE BOW
//****************************************************************************************************

ItemImageData CompositeBowImage
{
	shapeFile = "comp_bow";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	ammoType = "";
	projectileType = NoProjectile;
	accuFire = false;
	reloadTime = 0;
	fireTime = GetDelay(CompositeBow);

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = CrossbowShoot1;
	sfxActivate = CrossbowSwitch1;
	sfxReload = NoSound;
};
ItemData CompositeBow
{
	description = "Composite Bow";
	className = "Weapon";
	shapeFile = "comp_bow";
	hudIcon = "bow";
	heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = CompositeBowImage;
	price = 0;
	showWeaponBar = true;
};
function CompositeBowImage::onFire(%player, %slot)
{
	%clientId = Player::getClient(%player);

	%vel = 100;
	ProjectileAttack(%clientId, CompositeBow, %vel);
}

//****************************************************************************************************
//   AEOLUS'S WING
//****************************************************************************************************

ItemImageData AeolusWingImage
{
	shapeFile = "comp_bow";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	ammoType = "";
	projectileType = NoProjectile;
	accuFire = false;
	reloadTime = 0;
	fireTime = GetDelay(AeolusWing);

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = CrossbowShoot1;
	sfxActivate = CrossbowSwitch1;
	sfxReload = NoSound;
};
ItemData AeolusWing
{
	description = "Aeolus's Wing";
	className = "Weapon";
	shapeFile = "comp_bow";
	hudIcon = "bow";
	heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = AeolusWingImage;
	price = 0;
	showWeaponBar = true;
};
function AeolusWingImage::onFire(%player, %slot)
{
	%clientId = Player::getClient(%player);

	%vel = 100;
	ProjectileAttack(%clientId, AeolusWing, %vel);
}

//****************************************************************************************************
//   LIGHT CROSSBOW
//****************************************************************************************************

ItemImageData LightCrossbowImage
{
	shapeFile = "crossbow";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	ammoType = "";
	projectileType = NoProjectile;
	accuFire = false;
	reloadTime = 0;
	fireTime = GetDelay(LightCrossbow);

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = CrossbowShoot1;
	sfxActivate = CrossbowSwitch1;
	sfxReload = NoSound;
};
ItemData LightCrossbow
{
	description = "Light Crossbow";
	className = "Weapon";
	shapeFile = "crossbow";
	hudIcon = "grenade";
	heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = LightCrossbowImage;
	price = 0;
	showWeaponBar = true;
};
function LightCrossbowImage::onFire(%player, %slot)
{
	%clientId = Player::getClient(%player);

	%vel = 100;
	ProjectileAttack(%clientId, LightCrossbow, %vel);
}

//****************************************************************************************************
//   HEAVY CROSSBOW
//****************************************************************************************************

ItemImageData HeavyCrossbowImage
{
	shapeFile = "crossbow";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	ammoType = "";
	projectileType = NoProjectile;
	accuFire = false;
	reloadTime = 0;
	fireTime = GetDelay(HeavyCrossbow);

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = CrossbowShoot1;
	sfxActivate = CrossbowSwitch1;
	sfxReload = NoSound;
};
ItemData HeavyCrossbow
{
	description = "Heavy Crossbow";
	className = "Weapon";
	shapeFile = "crossbow";
	hudIcon = "grenade";
	heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = HeavyCrossbowImage;
	price = 0;
	showWeaponBar = true;
};
function HeavyCrossbowImage::onFire(%player, %slot)
{
	%clientId = Player::getClient(%player);

	%vel = 100;
	ProjectileAttack(%clientId, HeavyCrossbow, %vel);
}

//****************************************************************************************************
//   REPEATING CROSSBOW
//****************************************************************************************************

ItemImageData RepeatingCrossbowImage
{
	shapeFile = "crossbow";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	ammoType = "";
	projectileType = NoProjectile;
	accuFire = false;
	reloadTime = 0;
	fireTime = GetDelay(RepeatingCrossbow);

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = CrossbowShoot1;
	sfxActivate = CrossbowSwitch1;
	sfxReload = NoSound;
};
ItemData RepeatingCrossbow
{
	description = "Repeating Crossbow";
	className = "Weapon";
	shapeFile = "crossbow";
	hudIcon = "grenade";
	heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = RepeatingCrossbowImage;
	price = 0;
	showWeaponBar = true;
};
function RepeatingCrossbowImage::onFire(%player, %slot)
{
	%clientId = Player::getClient(%player);

	%vel = 100;
	ProjectileAttack(%clientId, RepeatingCrossbow, %vel);
}

//****************************************************************************************************
//   CASTING BLADE
//****************************************************************************************************

ItemImageData CastingBladeImage
{
	shapeFile  = "dagger";
	mountPoint = 0;

	weaponType = 0;
	reloadTime = 0;
	fireTime = GetDelay(CastingBlade);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = NoSound;
	sfxActivate = NoSound;
};
ItemData CastingBlade
{
	heading = "bWeapons";
	description = "Casting Blade";
	className = "Weapon";
	shapeFile  = "dagger";
	hudIcon = "dagger";
	shadowDetailMask = 4;
	imageType = CastingBladeImage;
	price = 0;
	showWeaponBar = true;
};
function CastingBladeImage::onFire(%player, %slot)
{
	%clientId = Player::getClient(%player);
	if(%clientId == "")
		%clientId = 0;

//	if(Player::isAIcontrolled(%clientId))
//	{
//		if(fetchData(%clientId, "HP") <= (fetchData(%clientId, "MaxHP")/3))
//		{
//			if( floor(getRandom() * 10) > 7 )
//				%doHealSpell = True;
//		}
//	}
//	if(%doHealSpell)
//		%index = GetBestSpell(%clientId, -1, True);
//	else

	%index = GetBestSpell(%clientId, 1, True);

	%length = $Spell::LOSrange[%index]-1;
		
	$los::object = "";
	if(GameBase::getLOSinfo(%player, %length) && %index != -1)
	{
		%obj = getObjectType($los::object);
		if(%obj == "Player")
		{
			if(Player::isAiControlled(%clientId))
			{
				AI::newDirectiveRemove(fetchData(%clientId, "BotInfoAiName"), 99);
			}
			internalSay(%clientId, 0, "#cast " @ $Spell::keyword[%index]);
			%hasCast = True;
		}
	}
	if(!%hasCast)
	{
		if(OddsAre(3))
			MeleeAttack(%player, GetRange(Hatchet), CastingBlade);	//mimic the hatchet range
	}
	%hasCast = "";
}

//====== "Projectiles" ======================================================

ItemData SmallRock
{
	description = "Small Rock";
	className = "Projectile";
	shapeFile = "little_rock";
	heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 0;
};
ItemData BasicArrow
{
	description = "Basic Arrow";
	className = "Projectile";
	shapeFile = "tracer";
	heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 0;
};
ItemData SheafArrow
{
	description = "Sheaf Arrow";
	className = "Projectile";
	shapeFile = "tracer";
	heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 0;
};
ItemData BladedArrow
{
	description = "Bladed Arrow";
	className = "Projectile";
	shapeFile = "tracer";
	heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 0;
};
ItemData LightQuarrel
{
	description = "Light Quarrel";
	className = "Projectile";
	shapeFile = "bullet";
	heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 0;
};
ItemData HeavyQuarrel
{
	description = "Heavy Quarrel";
	className = "Projectile";
	shapeFile = "bullet";
	heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 0;
};
ItemData ShortQuarrel
{
	description = "Short Quarrel";
	className = "Projectile";
	shapeFile = "bullet";
	heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 0;
};
ItemData StoneFeather
{
	description = "Stone Feather";
	className = "Projectile";
	shapeFile = "tracer";
	heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 0;
};
ItemData MetalFeather
{
	description = "Metal Feather";
	className = "Projectile";
	shapeFile = "tracer";
	heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 0;
};
ItemData Talon
{
	description = "Talon";
	className = "Projectile";
	shapeFile = "tracer";
	heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 0;
};
ItemData CeraphumsFeather
{
	description = "Ceraphum's Feather";
	className = "Projectile";
	shapeFile = "tracer";
	heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 0;
};

//===========================================================================================
//===========================================================================================
//===========================================================================================
//====================================             ==========================================
//====================================   RUSTIES   ==========================================
//====================================             ==========================================
//===========================================================================================
//===========================================================================================
//===========================================================================================

//---------------------------------

$AccessoryVar[RHatchet, $AccessoryType] = $AxeAccessoryType;
$AccessoryVar[RBroadSword, $AccessoryType] = $SwordAccessoryType;
$AccessoryVar[RLongSword, $AccessoryType] = $SwordAccessoryType;
$AccessoryVar[RClub, $AccessoryType] = $BludgeonAccessoryType;
$AccessoryVar[RSpikedClub, $AccessoryType] = $BludgeonAccessoryType;
$AccessoryVar[RKnife, $AccessoryType] = $SwordAccessoryType;
$AccessoryVar[RDagger, $AccessoryType] = $SwordAccessoryType;
$AccessoryVar[RShortSword, $AccessoryType] = $SwordAccessoryType;
$AccessoryVar[RPickAxe, $AccessoryType] = $AxeAccessoryType;
$AccessoryVar[RShortBow, $AccessoryType] = $RangedAccessoryType;
$AccessoryVar[RLightCrossbow, $AccessoryType] = $RangedAccessoryType;
$AccessoryVar[RWarAxe, $AccessoryType] = $AxeAccessoryType;

$AccessoryVar[RHatchet, $SpecialVar] = "6 " @ round(GetWord($AccessoryVar[Hatchet, $SpecialVar], 1) * $RustyDamageAmp);
$AccessoryVar[RBroadSword, $SpecialVar] = "6 " @ round(GetWord($AccessoryVar[BroadSword, $SpecialVar], 1) * $RustyDamageAmp);
$AccessoryVar[RLongSword, $SpecialVar] = "6 " @ round(GetWord($AccessoryVar[LongSword, $SpecialVar], 1) * $RustyDamageAmp);
$AccessoryVar[RClub, $SpecialVar] = "6 " @ round(GetWord($AccessoryVar[Club, $SpecialVar], 1) * $RustyDamageAmp);
$AccessoryVar[RSpikedClub, $SpecialVar] = "6 " @ round(GetWord($AccessoryVar[SpikedClub, $SpecialVar], 1) * $RustyDamageAmp);
$AccessoryVar[RKnife, $SpecialVar] = "6 " @ round(GetWord($AccessoryVar[Knife, $SpecialVar], 1) * $RustyDamageAmp);
$AccessoryVar[RDagger, $SpecialVar] = "6 " @ round(GetWord($AccessoryVar[Dagger, $SpecialVar], 1) * $RustyDamageAmp);
$AccessoryVar[RShortSword, $SpecialVar] = "6 " @ round(GetWord($AccessoryVar[ShortSword, $SpecialVar], 1) * $RustyDamageAmp);
$AccessoryVar[RPickAxe, $SpecialVar] = "6 " @ round(GetWord($AccessoryVar[PickAxe, $SpecialVar], 1) * $RustyDamageAmp);
$AccessoryVar[RShortBow, $SpecialVar] = "6 " @ round(GetWord($AccessoryVar[ShortBow, $SpecialVar], 1) * $RustyDamageAmp);
$AccessoryVar[RLightCrossbow, $SpecialVar] = "6 " @ round(GetWord($AccessoryVar[LightCrossbow, $SpecialVar], 1) * $RustyDamageAmp);
$AccessoryVar[RWarAxe, $SpecialVar] = "6 " @ round(GetWord($AccessoryVar[WarAxe, $SpecialVar], 1) * $RustyDamageAmp);

$AccessoryVar[RHatchet, $Weight] = $AccessoryVar[Hatchet, $Weight] * $RustyWeightAmp;
$AccessoryVar[RBroadSword, $Weight] = $AccessoryVar[BroadSword, $Weight] * $RustyWeightAmp;
$AccessoryVar[RLongSword, $Weight] = $AccessoryVar[LongSword, $Weight] * $RustyWeightAmp;
$AccessoryVar[RClub, $Weight] = $AccessoryVar[Club, $Weight] * $RustyWeightAmp;
$AccessoryVar[RSpikedClub, $Weight] = $AccessoryVar[SpikedClub, $Weight] * $RustyWeightAmp;
$AccessoryVar[RKnife, $Weight] = $AccessoryVar[Knife, $Weight] * $RustyWeightAmp;
$AccessoryVar[RDagger, $Weight] = $AccessoryVar[Dagger, $Weight] * $RustyWeightAmp;
$AccessoryVar[RShortSword, $Weight] = $AccessoryVar[ShortSword, $Weight] * $RustyWeightAmp;
$AccessoryVar[RPickAxe, $Weight] = $AccessoryVar[PickAxe, $Weight] * $RustyWeightAmp;
$AccessoryVar[RShortBow, $Weight] = $AccessoryVar[ShortBow, $Weight] * $RustyWeightAmp;
$AccessoryVar[RLightCrossbow, $Weight] = $AccessoryVar[LightCrossbow, $Weight] * $RustyWeightAmp;
$AccessoryVar[RWarAxe, $Weight] = $AccessoryVar[WarAxe, $Weight] * $RustyWeightAmp;

$AccessoryVar[RHatchet, $MiscInfo] = "A rusty hatchet";
$AccessoryVar[RBroadSword, $MiscInfo] = "A rusty broad sword";
$AccessoryVar[RLongSword, $MiscInfo] = "A rusty long sword";
$AccessoryVar[RClub, $MiscInfo] = "A cracked club";
$AccessoryVar[RSpikedClub, $MiscInfo] = "A cracked spiked club";
$AccessoryVar[RKnife, $MiscInfo] = "A rusty knife";
$AccessoryVar[RDagger, $MiscInfo] = "A rusty dagger";
$AccessoryVar[RShortSword, $MiscInfo] = "A rusty short sword";
$AccessoryVar[RPickAxe, $MiscInfo] = "A rusty pick axe";
$AccessoryVar[RShortBow, $MiscInfo] = "A cracked short bow";
$AccessoryVar[RLightCrossbow, $MiscInfo] = "A cracked light crossbow";
$AccessoryVar[RWarAxe, $MiscInfo] = "A rusty war axe";

$SkillType[RHatchet] = $SkillSlashing;
$SkillType[RBroadSword] = $SkillSlashing;
$SkillType[RLongSword] = $SkillSlashing;
$SkillType[RClub] = $SkillBludgeoning;
$SkillType[RSpikedClub] = $SkillBludgeoning;
$SkillType[RKnife] = $SkillPiercing;
$SkillType[RDagger] = $SkillPiercing;
$SkillType[RShortSword] = $SkillPiercing;
$SkillType[RPickAxe] = $SkillPiercing;
$SkillType[RShortBow] = $SkillArchery;
$SkillType[RLightCrossbow] = $SkillArchery;
$SkillType[RWarAxe] = $SkillSlashing;

//****************************************************************************************************
// RUSTY KNIFE
//****************************************************************************************************

ItemImageData RKnifeImage
{
	shapeFile = "dagger";
	mountPoint = 0;

	weaponType = 0;
	reloadTime = 0;
	fireTime = GetDelay(RKnife);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing1;
	sfxActivate = AxeSlash2;
};
ItemData RKnife
{ 
	heading = "bWeapons"; 
	description = "Rusty Knife"; 
	className = "Weapon"; 
	shapeFile = "dagger"; 
	hudIcon = "dagger";
	shadowDetailMask = 4; 
	imageType = RKnifeImage; 
	price = 0; 
	showWeaponBar = true; 
}; 
function RKnifeImage::onfire(%player, %slot) 
{ 
	MeleeAttack(%player, GetRange(RKnife), RKnife); 
} 

//**************************************************************************************************** 
// RUSTY DAGGER 
//**************************************************************************************************** 

ItemImageData RDaggerImage 
{ 
	shapeFile = "dagger"; 
	mountPoint = 0; 

	weaponType = 0; // Single Shot 
	reloadTime = 0; 
	fireTime = GetDelay(RDagger); 
	minEnergy = 0; 
	maxEnergy = 0; 

	accuFire = true; 

	sfxFire = SoundSwing1; 
	sfxActivate = AxeSlash2; 
}; 
ItemData RDagger 
{ 
	heading = "bWeapons"; 
	description = "Rusty Dagger"; 
	className = "Weapon"; 
	shapeFile = "dagger"; 
	hudIcon = "dagger";
	shadowDetailMask = 4; 
	imageType = RDaggerImage; 
	price = 0; 
	showWeaponBar = true; 
}; 
function RDaggerImage::onfire(%player, %slot) 
{ 
	MeleeAttack(%player, GetRange(RDagger), RDagger); 
} 

//**************************************************************************************************** 
// RUSTY SHORTSWORD 
//**************************************************************************************************** 

ItemImageData RShortswordImage 
{ 
	shapeFile = "short_sword"; 
	mountPoint = 0; 

	weaponType = 0; // Single Shot 
	reloadTime = 0; 
	fireTime = GetDelay(RShortsword);
	minEnergy = 0; 
	maxEnergy = 0; 

	accuFire = true; 

	sfxFire = SoundSwing2; 
	sfxActivate = AxeSlash2; 
}; 
ItemData RShortsword 
{ 
	heading = "bWeapons"; 
	description = "Rusty Short Sword"; 
	className = "Weapon"; 
	shapeFile = "short_sword"; 
	hudIcon = "blaster";
	shadowDetailMask = 4; 
	imageType = RShortswordImage; 
	price = 0; 
	showWeaponBar = true; 
}; 
function RShortswordImage::onfire(%player, %slot) 
{ 
	MeleeAttack(%player, GetRange(RShortsword), RShortsword); 
} 

//**************************************************************************************************** 
// RUSTY BROADSWORD 
//**************************************************************************************************** 

ItemImageData RBroadswordImage 
{ 
	shapeFile = "sword"; 
	mountPoint = 0; 

	weaponType = 0; // Single Shot 
	reloadTime = 0; 
	fireTime = GetDelay(RBroadsword);
	minEnergy = 0; 
	maxEnergy = 0; 

	accuFire = true; 

	sfxFire = SoundSwing5; 
	sfxActivate = AxeSlash2; 
}; 
ItemData RBroadsword 
{ 
	heading = "bWeapons"; 
	description = "Rusty Broad Sword"; 
	className = "Weapon"; 
	shapeFile = "sword"; 
	hudIcon = "blaster";
	shadowDetailMask = 4; 
	imageType = RBroadswordImage; 
	price = 0; 
	showWeaponBar = true; 
}; 
function RBroadswordImage::onfire(%player, %slot) 
{ 
	MeleeAttack(%player, GetRange(RBroadsword), RBroadsword); 
} 

//**************************************************************************************************** 
// RUSTY LONGSWORD 
//**************************************************************************************************** 

ItemImageData RLongswordImage 
{ 
	shapeFile = "long_sword"; 
	mountPoint = 0; 

	weaponType = 0; // Single Shot 
	reloadTime = 0; 
	fireTime = GetDelay(RLongsword);
	minEnergy = 0; 
	maxEnergy = 0; 

	accuFire = true; 

	sfxFire = SoundSwing6; 
	sfxActivate = AxeSlash2; 
}; 
ItemData RLongsword 
{ 
	heading = "bWeapons"; 
	description = "Rusty Long Sword"; 
	className = "Weapon"; 
	shapeFile = "long_sword"; 
	hudIcon = "blaster";
	shadowDetailMask = 4; 
	imageType = RLongswordImage; 
	price = 0; 
	showWeaponBar = true; 
}; 
function RLongswordImage::onfire(%player, %slot) 
{ 
	MeleeAttack(%player, GetRange(RLongsword), RLongsword); 
} 

//**************************************************************************************************** 
// RUSTY HATCHET 
//**************************************************************************************************** 

ItemImageData RHatchetImage 
{ 
	shapeFile = "axe"; 
	mountPoint = 0; 

	weaponType = 0; // Single Shot 
	reloadTime = 0; 
	fireTime = GetDelay(RHatchet);
	minEnergy = 0; 
	maxEnergy = 0; 

	accuFire = true; 

	sfxFire = SoundSwing1; 
	sfxActivate = AxeSlash2; 
}; 
ItemData RHatchet 
{ 
	heading = "bWeapons"; 
	description = "Rusty Hatchet"; 
	className = "Weapon"; 
	shapeFile = "axe"; 
	hudIcon = "axe";
	shadowDetailMask = 4; 
	imageType = RHatchetImage; 
	price = 0; 
	showWeaponBar = true; 
}; 
function RHatchetImage::onfire(%player, %slot) 
{ 
	MeleeAttack(%player, GetRange(RHatchet), RHatchet); 
} 

//**************************************************************************************************** 
// RUSTY PICKAXE 
//**************************************************************************************************** 

ItemImageData RPickAxeImage 
{ 
	shapeFile = "Pick"; 
	mountPoint = 0; 

	weaponType = 0; // Single Shot 
	reloadTime = 0; 
	fireTime = GetDelay(RPickAxe);
	minEnergy = 0; 
	maxEnergy = 0; 

	accuFire = true; 

	sfxFire = SoundSwing1; 
	sfxActivate = CrossbowSwitch1; 
}; 
ItemData RPickAxe 
{ 
	heading = "bWeapons"; 
	description = "Rusty Pick Axe"; 
	className = "Weapon"; 
	shapeFile = "Pick"; 
	hudIcon = "pick";
	shadowDetailMask = 4; 
	imageType = RPickAxeImage; 
	price = 0; 
	showWeaponBar = true; 
}; 
function RPickAxeImage::onfire(%player, %slot) 
{ 
	PickAxeSwing(%player, GetRange(RPickAxe), RPickAxe); 
} 

//**************************************************************************************************** 
// CRACKED CLUB 
//**************************************************************************************************** 

ItemImageData RClubImage 
{ 
	shapeFile = "mace"; 
	mountPoint = 0; 

	weaponType = 0; // Single Shot 
	reloadTime = 0; 
	fireTime = GetDelay(RClub);
	minEnergy = 0; 
	maxEnergy = 0; 

	accuFire = true; 

	sfxFire = SoundSwing5; 
	sfxActivate = AxeSlash2; 
}; 
ItemData RClub 
{ 
	heading = "bWeapons"; 
	description = "Cracked Club"; 
	className = "Weapon"; 
	shapeFile = "mace"; 
	hudIcon = "club";
	shadowDetailMask = 4; 
	imageType = RClubImage; 
	price = 0; 
	showWeaponBar = true; 
}; 
function RClubImage::onfire(%player, %slot) 
{ 
	MeleeAttack(%player, GetRange(RClub), RClub); 
} 

//**************************************************************************************************** 
// CRACKED SPIKED CLUB 
//**************************************************************************************************** 

ItemImageData RSpikedClubImage 
{ 
	shapeFile = "mace"; 
	mountPoint = 0; 

	weaponType = 0; // Single Shot 
	reloadTime = 0; 
	fireTime = GetDelay(RSpikedClub);
	minEnergy = 0; 
	maxEnergy = 0; 

	accuFire = true; 

	sfxFire = SoundSwing5; 
	sfxActivate = AxeSlash2; 
}; 
ItemData RSpikedClub 
{ 
	heading = "bWeapons"; 
	description = "Cracked Spiked Club"; 
	className = "Weapon"; 
	shapeFile = "mace"; 
	hudIcon = "sclub";
	shadowDetailMask = 4; 
	imageType = RSpikedClubImage; 
	price = 0; 
	showWeaponBar = true; 
}; 
function RSpikedClubImage::onfire(%player, %slot) 
{ 
	MeleeAttack(%player, GetRange(RSpikedClub), RSpikedClub); 
} 

//**************************************************************************************************** 
// CRACKED SHORT BOW 
//**************************************************************************************************** 

ItemImageData RShortBowImage 
{ 
	shapeFile = "longbow"; 
	mountPoint = 0; 

	weaponType = 0; // Single Shot 
	ammoType = ""; 
	projectileType = NoProjectile; 
	accuFire = false; 
	reloadTime = 0; 
	fireTime = GetDelay(RShortBow);

	lightType = 3; // Weapon Fire 
	lightRadius = 3; 
	lightTime = 1; 
	lightColor = { 0.6, 1, 1.0 }; 

	sfxFire = CrossbowShoot1; 
	sfxActivate = CrossbowSwitch1; 
	sfxReload = NoSound; 
}; 
ItemData RShortBow 
{ 
	description = "Cracked Short Bow"; 
	className = "Weapon"; 
	shapeFile = "longbow"; 
	hudIcon = "bow";
	heading = "bWeapons"; 
	shadowDetailMask = 4; 
	imageType = RShortBowImage; 
	price = 0; 
	showWeaponBar = true; 
}; 
function RShortBowImage::onfire(%player, %slot) 
{ 
	%clientId = Player::getClient(%player); 

	%vel = 100; 
	ProjectileAttack(%clientId, RShortBow, %vel); 
} 

//**************************************************************************************************** 
// CRACKED LIGHT CROSSBOW 
//**************************************************************************************************** 

ItemImageData RLightCrossbowImage 
{ 
	shapeFile = "crossbow"; 
	mountPoint = 0; 

	weaponType = 0; // Single Shot 
	ammoType = ""; 
	projectileType = NoProjectile; 
	accuFire = false; 
	reloadTime = 0; 
	fireTime = GetDelay(RLightCrossbow);

	lightType = 3; // Weapon Fire 
	lightRadius = 3; 
	lightTime = 1; 
	lightColor = { 0.6, 1, 1.0 }; 

	sfxFire = CrossbowShoot1; 
	sfxActivate = CrossbowSwitch1; 
	sfxReload = NoSound; 
}; 
ItemData RLightCrossbow 
{ 
	description = "Cracked Light Crossbow"; 
	className = "Weapon"; 
	shapeFile = "crossbow"; 
	hudIcon = "grenade";
	heading = "bWeapons"; 
	shadowDetailMask = 4; 
	imageType = RLightCrossbowImage; 
	price = 0; 
	showWeaponBar = true; 
}; 
function RLightCrossbowImage::onfire(%player, %slot) 
{ 
	%clientId = Player::getClient(%player); 

	%vel = 100; 
	ProjectileAttack(%clientId, RLightCrossbow, %vel); 
}

//****************************************************************************************************
// RUSTY WARAXE
//****************************************************************************************************

ItemImageData RWarAxeImage
{
	shapeFile = "axe";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = GetDelay(RWarAxe);
	minEnergy = 0;
	maxEnergy = 0;

	accuFire = true;

	sfxFire = SoundSwing3;
	sfxActivate = CrossbowSwitch1;
};
ItemData RWarAxe
{
	heading = "bWeapons";
	description = "Rusty War Axe";
	className = "Weapon";
	shapeFile = "axe";
	hudIcon = "axe";
	shadowDetailMask = 4;
	imageType = RWarAxeImage;
	price = 0;
	showWeaponBar = true;
};
function RWarAxeImage::onFire(%player, %slot)
{
	MeleeAttack(%player, GetRange(RWarAxe), RWarAxe);
}


ItemData Blaster
{
	heading = "zOmg";
	description = "Blaster";
	className = "Weapon";
	shapeFile  = "energygun";
	hudIcon = "blaster";
};


ItemData PlasmaGun
{
	heading = "zOmg";
	description = "Plasma Gun";
	className = "Weapon";
	shapeFile  = "energygun";
	hudIcon = "blaster";
};


ItemData Chaingun
{
	heading = "zOmg";
	description = "Chaingun";
	className = "Weapon";
	shapeFile  = "energygun";
	hudIcon = "blaster";
};


ItemData DiscLauncher
{
	heading = "zOmg";
	description = "Disc Launcher";
	className = "Weapon";
	shapeFile  = "energygun";
	hudIcon = "blaster";
};


ItemData GrenadeLauncher
{
	heading = "zOmg";
	description = "Grenade Launcher";
	className = "Weapon";
	shapeFile  = "energygun";
	hudIcon = "blaster";
};


ItemData LaserRifle
{
	heading = "zOmg";
	description = "Laser Rifle";
	className = "Weapon";
	shapeFile  = "energygun";
	hudIcon = "blaster";
};


ItemData ElfGun
{
	heading = "zOmg";
	description = "Elf Gun";
	className = "Weapon";
	shapeFile  = "energygun";
	hudIcon = "blaster";
};


ItemData Mortar
{
	heading = "zOmg";
	description = "Mortar";
	className = "Weapon";
	shapeFile  = "energygun";
	hudIcon = "blaster";
};


ItemData TargetingLaser
{
	heading = "zOmg";
	description = "Targeting Laser";
	className = "Weapon";
	shapeFile  = "energygun";
	hudIcon = "blaster";
};