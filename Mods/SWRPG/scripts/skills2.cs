//######################################################################################
// Skills
//######################################################################################

$SkillArchery = 1;
$SkillMelee = 2;
$SkillLightsabers = 3;
$SkillBashing = 4;
$SkillDodging = 5;
$SkillEndurance = 6;
$SkillHealing = 7;
$SkillWeightCapacity = 8;
$SkillStealing = 9;
$SkillHiding = 10;
$SkillBackstabbing = 11;
$SkillCharisma = 12;
$SkillMining = 13;
$SkillSenseHeading = 14;
$SkillDefensiveCasting = 15;
$SkillOffensiveCasting = 16;
$SkillNeutralCasting = 17;
$SkillEnergy = 18;
$SkillSpellResistance = 19;
$MinLevel = "L";
$MinGroup = "G";
$MinClass = "C";
$MinRemort = "R";
$MinAdmin = "A";
$MinHouse = "H";
$MinFaction = "H";
$MinAlignment = "FMin";	// You must have MORE THAN x amount alignment. For example: $MinAlignment @ " 15"
$MaxAlignment = "FMax";	// You must have LESS THAN the amount.
			// Force alignment is on a scale of -50 to +50, light side is > 0, dark side is < 0. (Should I make the scale -100 to +100? hmm..)

$SkillDesc[1] = "Blasters";
$SkillDesc[2] = "Melee weapons";
$SkillDesc[3] = "Lightsabers";
$SkillDesc[4] = "Tactics";
$SkillDesc[5] = "Dodging";
$SkillDesc[6] = "Vitality";
$SkillDesc[7] = "Healing";
$SkillDesc[8] = "Weight Capacity";
$SkillDesc[9] = "Stealing";
$SkillDesc[10] = "Hiding";
$SkillDesc[11] = "Backstabbing";
$SkillDesc[12] = "Charisma";
$SkillDesc[13] = "Mining";
$SkillDesc[14] = "Sense Heading";
$SkillDesc[15] = "Light Side Affinity";
$SkillDesc[16] = "Dark Side Affinity";
$SkillDesc[17] = "Force Attunement";
$SkillDesc[18] = "Energy";
$SkillDesc[19] = "Force Resistance";
$SkillDesc[L] = "Level";
$SkillDesc[G] = "Group";
$SkillDesc[C] = "Class";
$SkillDesc[R] = "Remort";
$SkillDesc[A] = "Admin Level";
$SkillDesc[H] = "Faction";
$SkillDesc[FMin] = "Alignment(at least)";
$SkillDesc[FMax] = "Alignment(at most)";

//######################################################################################
// Class multipliers
//######################################################################################

//***********************************
// GENERAL RULES FOR MULTIPLIERS:
//***********************************
//- Maximum multiplier should be 2.0
//- Minimum multiplier should be 0.1
//- A 0.1 should be VERY rare.  The normal minimum is 0.2.  If a class should not even
//  be near a certain skill, that's when the 0.1 comes in.

//******** SUMMARY ******************
//- Primary skills use a 2.0 multiplier
//- Secondary skills use a 1.5 multiplier
//- Normal skills use a ~1.0 multiplier
//- Weak skills use a ~0.5 multiplier
//- VERY weak skills use a 0.2
//- Unsuitable skills for a specific class use a 0.1

//--------------
// Jedi
//--------------

$SkillMultiplier["Jedi Apprentice", $SkillArchery] = 0.8;
$SkillMultiplier["Jedi Apprentice", $SkillMelee] = 1.0;
$SkillMultiplier["Jedi Apprentice", $SkillLightsabers] = 1.8;
$SkillMultiplier["Jedi Apprentice", $SkillBashing] = 1.0;
$SkillMultiplier["Jedi Apprentice", $SkillDodging] = 1.5;
$SkillMultiplier["Jedi Apprentice", $SkillEndurance] = 0.9;
$SkillMultiplier["Jedi Apprentice", $SkillHealing] = 1.0;
$SkillMultiplier["Jedi Apprentice", $SkillWeightCapacity] = 0.9;
$SkillMultiplier["Jedi Apprentice", $SkillStealing] = 0.2;
$SkillMultiplier["Jedi Apprentice", $SkillHiding] = 1.0;
$SkillMultiplier["Jedi Apprentice", $SkillBackstabbing] = 0.5;
$SkillMultiplier["Jedi Apprentice", $SkillCharisma] = 1.2;
$SkillMultiplier["Jedi Apprentice", $SkillMining] = 0.8;
$SkillMultiplier["Jedi Apprentice", $SkillSenseHeading] = 1.5;
$SkillMultiplier["Jedi Apprentice", $SkillDefensiveCasting] = 2.0;
$SkillMultiplier["Jedi Apprentice", $SkillOffensiveCasting] = 0.1;
$SkillMultiplier["Jedi Apprentice", $SkillNeutralCasting] = 1.5;
$SkillMultiplier["Jedi Apprentice", $SkillEnergy] = 1.5;
$SkillMultiplier["Jedi Apprentice", $SkillSpellResistance] = 1.5;
$EXPmultiplier["Jedi Apprentice"] = 0.85;

//--------------
// Gray Jedi
//--------------

$SkillMultiplier["Gray Apprentice", $SkillArchery] = 0.8;
$SkillMultiplier["Gray Apprentice", $SkillMelee] = 1.0;
$SkillMultiplier["Gray Apprentice", $SkillLightsabers] = 1.5;
$SkillMultiplier["Gray Apprentice", $SkillBashing] = 1.0;
$SkillMultiplier["Gray Apprentice", $SkillDodging] = 1.5;
$SkillMultiplier["Gray Apprentice", $SkillEndurance] = 1.0;
$SkillMultiplier["Gray Apprentice", $SkillHealing] = 0.8;
$SkillMultiplier["Gray Apprentice", $SkillWeightCapacity] = 0.9;
$SkillMultiplier["Gray Apprentice", $SkillStealing] = 0.5;
$SkillMultiplier["Gray Apprentice", $SkillHiding] = 1.0;
$SkillMultiplier["Gray Apprentice", $SkillBackstabbing] = 0.5;
$SkillMultiplier["Gray Apprentice", $SkillCharisma] = 1.0;
$SkillMultiplier["Gray Apprentice", $SkillMining] = 0.8;
$SkillMultiplier["Gray Apprentice", $SkillSenseHeading] = 1.5;
$SkillMultiplier["Gray Apprentice", $SkillDefensiveCasting] = 0.9;
$SkillMultiplier["Gray Apprentice", $SkillOffensiveCasting] = 0.8;
$SkillMultiplier["Gray Apprentice", $SkillNeutralCasting] = 2.0;
$SkillMultiplier["Gray Apprentice", $SkillEnergy] = 1.5;
$SkillMultiplier["Gray Apprentice", $SkillSpellResistance] = 1.6;
$EXPmultiplier["Gray Apprentice"] = 0.85;

//--------------
// Dark  Jedi
//--------------

$SkillMultiplier["Sith Apprentice", $SkillArchery] = 0.8;
$SkillMultiplier["Sith Apprentice", $SkillMelee] = 1.0;
$SkillMultiplier["Sith Apprentice", $SkillLightsabers] = 1.8;
$SkillMultiplier["Sith Apprentice", $SkillBashing] = 1.0;
$SkillMultiplier["Sith Apprentice", $SkillDodging] = 1.5;
$SkillMultiplier["Sith Apprentice", $SkillEndurance] = 0.8;
$SkillMultiplier["Sith Apprentice", $SkillHealing] = 0.1;
$SkillMultiplier["Sith Apprentice", $SkillWeightCapacity] = 0.8;
$SkillMultiplier["Sith Apprentice", $SkillStealing] = 0.5;
$SkillMultiplier["Sith Apprentice", $SkillHiding] = 1.0;
$SkillMultiplier["Sith Apprentice", $SkillBackstabbing] = 1.5;
$SkillMultiplier["Sith Apprentice", $SkillCharisma] = 0.9;
$SkillMultiplier["Sith Apprentice", $SkillMining] = 0.8;
$SkillMultiplier["Sith Apprentice", $SkillSenseHeading] = 1.5;
$SkillMultiplier["Sith Apprentice", $SkillDefensiveCasting] = 0.1;
$SkillMultiplier["Sith Apprentice", $SkillOffensiveCasting] = 2.0;
$SkillMultiplier["Sith Apprentice", $SkillNeutralCasting] = 1.5;
$SkillMultiplier["Sith Apprentice", $SkillEnergy] = 1.5;
$SkillMultiplier["Sith Apprentice", $SkillSpellResistance] = 1.5;
$EXPmultiplier["Sith Apprentice"] = 0.85;

//--------------
// Soldier
//--------------

$SkillMultiplier[Private, $SkillArchery] = 2.0;
$SkillMultiplier[Private, $SkillMelee] = 1.5;
$SkillMultiplier[Private, $SkillLightsabers] = 0.2;
$SkillMultiplier[Private, $SkillBashing] = 1.5;
$SkillMultiplier[Private, $SkillDodging] = 1.0;
$SkillMultiplier[Private, $SkillEndurance] = 1.8;
$SkillMultiplier[Private, $SkillHealing] = 1.0;
$SkillMultiplier[Private, $SkillWeightCapacity] = 1.5;
$SkillMultiplier[Private, $SkillStealing] = 0.2;
$SkillMultiplier[Private, $SkillHiding] = 0.4;
$SkillMultiplier[Private, $SkillBackstabbing] = 1.0;
$SkillMultiplier[Private, $SkillCharisma] = 0.7;
$SkillMultiplier[Private, $SkillMining] = 1.0;
$SkillMultiplier[Private, $SkillSenseHeading] = 1.5;
$SkillMultiplier[Private, $SkillDefensiveCasting] = 0.1;
$SkillMultiplier[Private, $SkillOffensiveCasting] = 0.1;
$SkillMultiplier[Private, $SkillNeutralCasting] = 0.1;
$SkillMultiplier[Private, $SkillEnergy] = 0.1;
$SkillMultiplier[Private, $SkillSpellResistance] = 0.2;
$EXPmultiplier[Private] = 0.95;

//--------------
// Pilot
//--------------

$SkillMultiplier["Navalman Recruit", $SkillArchery] = 1.8;
$SkillMultiplier["Navalman Recruit", $SkillMelee] = 1.0;
$SkillMultiplier["Navalman Recruit", $SkillLightsabers] = 0.1;
$SkillMultiplier["Navalman Recruit", $SkillBashing] = 1.0;
$SkillMultiplier["Navalman Recruit", $SkillDodging] = 1.1;
$SkillMultiplier["Navalman Recruit", $SkillEndurance] = 1.0;
$SkillMultiplier["Navalman Recruit", $SkillHealing] = 0.8;
$SkillMultiplier["Navalman Recruit", $SkillWeightCapacity] = 1.5;
$SkillMultiplier["Navalman Recruit", $SkillStealing] = 0.3;
$SkillMultiplier["Navalman Recruit", $SkillHiding] = 0.5;
$SkillMultiplier["Navalman Recruit", $SkillBackstabbing] = 1.0;
$SkillMultiplier["Navalman Recruit", $SkillCharisma] = 1.2;
$SkillMultiplier["Navalman Recruit", $SkillMining] = 0.9;
$SkillMultiplier["Navalman Recruit", $SkillSenseHeading] = 1.8;
$SkillMultiplier["Navalman Recruit", $SkillDefensiveCasting] = 0.1;
$SkillMultiplier["Navalman Recruit", $SkillOffensiveCasting] = 0.1;
$SkillMultiplier["Navalman Recruit", $SkillNeutralCasting] = 0.1;
$SkillMultiplier["Navalman Recruit", $SkillEnergy] = 0.1;
$SkillMultiplier["Navalman Recruit", $SkillSpellResistance] = 0.1;
$EXPmultiplier["Navalman Recruit"] = 0.95;

//--------------
// Bounty Hunter
//--------------

$SkillMultiplier[Mercenary, $SkillArchery] = 1.5;
$SkillMultiplier[Mercenary, $SkillMelee] = 1.5;
$SkillMultiplier[Mercenary, $SkillLightsabers] = 0.1;
$SkillMultiplier[Mercenary, $SkillBashing] = 0.8;
$SkillMultiplier[Mercenary, $SkillDodging] = 1.4;
$SkillMultiplier[Mercenary, $SkillEndurance] = 1.0;
$SkillMultiplier[Mercenary, $SkillHealing] = 0.5;
$SkillMultiplier[Mercenary, $SkillWeightCapacity] = 1.0;
$SkillMultiplier[Mercenary, $SkillStealing] = 1.0;
$SkillMultiplier[Mercenary, $SkillHiding] = 2.0;
$SkillMultiplier[Mercenary, $SkillBackstabbing] = 2.0;
$SkillMultiplier[Mercenary, $SkillCharisma] = 1.5;
$SkillMultiplier[Mercenary, $SkillMining] = 1.5;
$SkillMultiplier[Mercenary, $SkillSenseHeading] = 2.0;
$SkillMultiplier[Mercenary, $SkillDefensiveCasting] = 0.1;
$SkillMultiplier[Mercenary, $SkillOffensiveCasting] = 0.1;
$SkillMultiplier[Mercenary, $SkillNeutralCasting] = 0.1;
$SkillMultiplier[Mercenary, $SkillEnergy] = 0.1;
$SkillMultiplier[Mercenary, $SkillSpellResistance] = 0.3;
$EXPmultiplier[Mercenary] = 1.0;

//--------------
// Smuggler
//--------------

$SkillMultiplier[Smuggler, $SkillArchery] = 1.6;
$SkillMultiplier[Smuggler, $SkillMelee] = 0.9;
$SkillMultiplier[Smuggler, $SkillLightsabers] = 0.1;
$SkillMultiplier[Smuggler, $SkillBashing] = 1.0;
$SkillMultiplier[Smuggler, $SkillDodging] = 1.3;
$SkillMultiplier[Smuggler, $SkillEndurance] = 1.0;
$SkillMultiplier[Smuggler, $SkillHealing] = 0.8;
$SkillMultiplier[Smuggler, $SkillWeightCapacity] = 2.0;
$SkillMultiplier[Smuggler, $SkillStealing] = 2.0;
$SkillMultiplier[Smuggler, $SkillHiding] = 1.8;
$SkillMultiplier[Smuggler, $SkillBackstabbing] = 1.0;
$SkillMultiplier[Smuggler, $SkillCharisma] = 2.0;
$SkillMultiplier[Smuggler, $SkillMining] = 1.2;
$SkillMultiplier[Smuggler, $SkillSenseHeading] = 1.5;
$SkillMultiplier[Smuggler, $SkillDefensiveCasting] = 0.1;
$SkillMultiplier[Smuggler, $SkillOffensiveCasting] = 0.1;
$SkillMultiplier[Smuggler, $SkillNeutralCasting] = 0.1;
$SkillMultiplier[Smuggler, $SkillEnergy] = 0.1;
$SkillMultiplier[Smuggler, $SkillSpellResistance] = 0.2;
$EXPmultiplier[Smuggler] = 1.0;

//--------------
// Fighter. Generic bot class.
//--------------

$SkillMultiplier[Fighter, $SkillArchery] = 1.0;
$SkillMultiplier[Fighter, $SkillMelee] = 1.0;
$SkillMultiplier[Fighter, $SkillLightsabers] = 0.3;
$SkillMultiplier[Fighter, $SkillBashing] = 1.0;
$SkillMultiplier[Fighter, $SkillDodging] = 1.0;
$SkillMultiplier[Fighter, $SkillEndurance] = 1.0;
$SkillMultiplier[Fighter, $SkillHealing] = 0.7;
$SkillMultiplier[Fighter, $SkillWeightCapacity] = 1.0;
$SkillMultiplier[Fighter, $SkillStealing] = 1.0;
$SkillMultiplier[Fighter, $SkillHiding] = 1.0;
$SkillMultiplier[Fighter, $SkillBackstabbing] = 1.0;
$SkillMultiplier[Fighter, $SkillCharisma] = 1.0;
$SkillMultiplier[Fighter, $SkillMining] = 1.0;
$SkillMultiplier[Fighter, $SkillSenseHeading] = 1.0;
$SkillMultiplier[Fighter, $SkillDefensiveCasting] = 0.1;
$SkillMultiplier[Fighter, $SkillOffensiveCasting] = 0.1;
$SkillMultiplier[Fighter, $SkillNeutralCasting] = 0.1;
$SkillMultiplier[Fighter, $SkillEnergy] = 0.1;
$SkillMultiplier[Fighter, $SkillSpellResistance] = 0.5;
$EXPmultiplier[Fighter] = 1.0;

//######################################################################################
// Skill Restriction tables
//######################################################################################

//To determine skill restrictions, do the following:
//
//-Determine the following variables first:
//	(weapon):
//	a = ATK * 1.1 (archery is 0.75)
//	b = Delay = Cap((Weight / 3), 1, "inf")
//
//	(armor):
//	a = (DEF + MDEF) / 6
//	b = 1.0
//
//-To find out what the skill restriction number is, follow this formula, where s is the final skill restriction:
//	s = Cap((a / b) - 20), 0, "inf") * 10.0;
//

function GetSpecialVar(%item, %type)
{
	if(%type == ATK || %type == 6)
		%n = 6;
	else if(%type == DEF || %type == 7)
		%n = 7;
	else if(%type == MDEF || %type == FDEF || %type == 3)
		%n = 3;
	else if(%type == HP || %type == 4)
		%n = 4;
	else if(%type == Mana || %type == Energy || %type == 5)
		%n = 5;
	else if(%type == HPregen || %type == 10)
		%n = 10;
	else if(%type == Manaregen || %type == Energyregen || %type == 11)
		%n = 11;
	else if(%type == Internal || %type == 8)
		%n = 8;

	for(%i = 0; (%w = getWord($AccessoryVar[%item, 2], %i)) != -1; %i += 2)
		if(%w == %n)
			return getWord($AccessoryVar[%item, 2], %i + 1);
}

function GetRequirements(%item)
{
	%type = $AccessoryVar[%item, $AccessoryType];
	//echo(%type);
	if(%type >= 7 && %type <= 10) //Sword, Axe, Polearm, Bludgeon(respective)
	{
		%a = GetSpecialVar(%item, ATK) * 1.1;
		%b = GetDelay(%item);
	}
	else if(%type == $RangedAccessoryType)//$ = 11
	{
		%a = GetSpecialVar(%item, ATK) * 0.75;
		%b = GetDelay(%item);
	}
	else if(%type == $BodyAccessoryType)//$ = 2
	{
		%a = (GetSpecialVar(%item, DEF) + GetSpecialVar(%item, MDEF)) / 6;
		%b = 1;
	}
	else
		return False;

	return Cap(((%a / %b) - 20), 0, "inf") * 10.0;
}

//function GenerateAllSkillRequirements()
//{
$SkillRestriction[BactaVial] = $SkillHealing @ " 0";
$SkillRestriction[BactaCanister] = $SkillHealing @ " 0";
$SkillRestriction[KoltoVial] = $SkillEnergy @ " 0";
$SkillRestriction[KoltoCanister] = $SkillEnergy @ " 0";

$SkillRestriction[ApprenticeRobe] = $SkillEndurance @ " 0 " @ $SkillEnergy @ " 8 " @ $MinGroup @ " Jedi";
$SkillRestriction[PadawanRobe] = $SkillEndurance @ " 3 " @ $SkillEnergy @ " 80 " @ $MinGroup @ " Jedi";
$SkillRestriction[JediRobe] = $SkillEndurance @ " 9 " @ $SkillEnergy @ " 175 " @ $MinGroup @ " Jedi";
$SkillRestriction[JediKnightRobe] = $SkillEndurance @ " 8 " @ $SkillEnergy @ " 300 " @ $MinGroup @ " Jedi";
$SkillRestriction[AdvisorRobe] = $SkillEndurance @ " 10 " @ $SkillEnergy @ " 450 " @ $MinGroup @ " Jedi";
$SkillRestriction[KelDromaRobe] = $SkillEndurance @ " 12 " @ $SkillEnergy @ " 620 " @ $MinGroup @ " Jedi";
$SkillRestriction[NorrisRobe] = $SkillEndurance @ " 18 " @ $SkillEnergy @ " 800 " @ $MinGroup @ " Jedi";
$SkillRestriction[JediMasterRobe] = $SkillEndurance @ " 20 " @ $SkillEnergy @ " 980 " @ $MinRemort @ " 1 " @ $MinGroup @ " Jedi";
$SkillRestriction[OssusKeeperRobe] = $SkillEndurance @ " 20 " @ $SkillEnergy @ " 980 " @ $MinRemort @ " 1 " @ $MinGroup @ " Jedi";
$SkillRestriction[SithLordRobe] = $SkillEndurance @ " 20 " @ $SkillEnergy @ " 980 " @ $MinRemort @ " 1 " @ $MinGroup @ " Jedi";
$SkillRestriction[StarForgeRobe] = $SkillEndurance @ " 20 " @ $SkillEnergy @ " 1060 " @ $MinRemort @ " 2 " @ $MinGroup @ " Jedi";
$SkillRestriction[QuestMasterRobe] = $MinAdmin @ " 3";

$SkillRestriction[PaddedCombatSuit] = $SkillEndurance @ " " @ $SkillRestriction[PaddedCombatSuit] = $SkillEndurance @ " 5";
$SkillRestriction[LightCombatSuit] = $SkillEndurance @ " 40";
$SkillRestriction[HeavyCombatSuit] = $SkillEndurance @ " 95";
$SkillRestriction[VerpineFiberMesh] = $SkillEndurance @ " 135";
$SkillRestriction[PoweredCombatSuit] = $SkillEndurance @ " 180";
$SkillRestriction[EchaniLightArmor] = $SkillEndurance @ " 240";
$SkillRestriction[MandalorianCombatSuit] = $SkillEndurance @ " 300";
$SkillRestriction[CorellianPowersuit] = $SkillEndurance @ " 350";
$SkillRestriction[CinnagarWarSuit] = $SkillEndurance @ " 410";
$SkillRestriction[EchaniBattleArmor] = $SkillEndurance @ " 490";
$SkillRestriction[MandalorianAssaultArmor] = $SkillEndurance @ " 580";
$SkillRestriction[BonadanHeavyArmor] = $SkillEndurance @ " 660";
$SkillRestriction[KrathHeavyArmor] = $SkillEndurance @ " 775";
$SkillRestriction[EchaniShieldSuit] = $SkillEndurance @ " 840";
$SkillRestriction[SithBattleArmor] = $SkillEndurance @ " 950";
$SkillRestriction[DurasteelHeavyArmor] = $SkillEndurance @ " 1065";
$SkillRestriction[EchaniHeavyArmor] = $SkillEndurance @ " 1305";

$SkillRestriction[CheetaursPaws] = $MinLevel @ " 8";
$SkillRestriction[AntigravityBoots] = $MinLevel @ " 25";
$SkillRestriction[JetPack] = $MinLevel @ " 60";

$SkillRestriction[GunganShield] = $SkillEndurance @ " 140";
$SkillRestriction[MandalorianShield] = $SkillEndurance @ " 540 " @ $SkillEnergy @ " 850";
$SkillRestriction[VerpineShield] = $SkillEndurance @ " 540 " @ $SkillEnergy @ " 850";
$SkillRestriction[EchaniShield] = $SkillEndurance @ " 715";

$SkillRestriction[Lightsaber] = $SkillLightsabers @ " 1";
$SkillRestriction[TrainingLightsaber] = $SkillLightsabers @ " 1";
$SkillRestriction[BlueLightsaber] = $SkillLightsabers @ " 1";
$SkillRestriction[GreenLightsaber] = $SkillLightsabers @ " 1";
$SkillRestriction[RedLightsaber] = $SkillLightsabers @ " 1";
$SkillRestriction[DoubleRedLightsaber] = $SkillLightsabers @ " 500";
$SkillRestriction[HikenLightsaber] = $SkillLightsabers @ " 300 " @ $MinAlignment @ " 30";
$SkillRestriction[BastardSword] = $SkillLightsabers @ " 620";
$SkillRestriction[NerinLightsaber] = $SkillLightsabers @ " 300 " @ $MaxAlignment @ " -30";
$SkillRestriction[Claymore] = $SkillLightsabers @ " 900";
$SkillRestriction[KeldriniteLS] = $SkillLightsabers @ " 1120 " @ $MinRemort @ " 1";
//.................................................................................
$SkillRestriction[PickAxe] = $SkillMelee @ " 0";
$SkillRestriction[Knife] = $SkillMelee @ " 0";
$SkillRestriction[GaffiiStick] = $SkillMelee @ " 0";
$SkillRestriction[Dagger] = $SkillMelee @ " 20";
$SkillRestriction[Vibroshiv] = $SkillMelee @ " 45";
$SkillRestriction[ShortSword] = $SkillMelee  @ " 60";
$SkillRestriction[QuarterStaff] = $SkillMelee @ " 100";
$SkillRestriction[LongSword] = $SkillMelee @ " 150";
$SkillRestriction[Mace] = $SkillMelee @ " 210";
$SkillRestriction[Vibroblade] = $SkillMelee @ " 260";
$SkillRestriction[GamorreanCleaver] = $SkillMelee @ " 320";
$SkillRestriction[ZabrakVibroblade] = $SkillMelee @ " 310 " @ $MinRemort @ " 1";
$SkillRestriction[LongStaff] = $SkillMelee @ " 400";
$SkillRestriction[Spear] = $SkillMelee @ " 480";
$SkillRestriction[Vibrosword] = $SkillMelee @ " 570";
$SkillRestriction[RakatanBattleAxe] = $SkillMelee @ " 540 " @ $MinRemort @ " 2";
$SkillRestriction[KrathWarBlade] = $SkillMelee @ " 768";
$SkillRestriction[GamorreanWaraxe] = $SkillMelee @ " 660 " @ $MinRemort @ " 3";
$SkillRestriction[EchaniVibrosword] = $SkillMelee @ " 720 " @ $MinRemort @ " 2";
$SkillRestriction[KrathDireSword] = $SkillMelee @ " 886 " @ $MinRemort @ " 5";
$SkillRestriction[EchaniFoil] = $SkillMelee @ " 910 " @ $MinRemort @ " 4";
$SkillRestriction[RakatanVibroSword] = $SkillMelee @ " 940 " @ $MinRemort @ " 8";
//.................................................................................
$SkillRestriction[HoldoutBlaster] = $SkillArchery @ " 0";
$SkillRestriction[BlasTechDL44] = $SkillArchery @ " 25";
$SkillRestriction[NoobianS5] = $SkillArchery @ " 160";
$SkillRestriction[BaktoidE5] = $SkillArchery @ " 318";
$SkillRestriction[BlasTechE11] = $SkillArchery @ " 438";
$SkillRestriction[EE3Carbine] = $SkillArchery @ " 550";
$SkillRestriction[BlasTechDLT19] = $SkillArchery @ " 685";
$SkillRestriction[HeavyCrossbow] = $SkillArchery @ " 805";
$SkillRestriction[BlasTechT21] = $SkillArchery @ " 925";
$SkillRestriction[HeavyRepeater] = $SkillArchery @ " 1000";
//.................................................................................
$SkillRestriction[SmallRock] = $SkillArchery @ " 0";
$SkillRestriction[EnergyCells] = $SkillArchery @ " 0";
$SkillRestriction[TibannaGasCells] = $SkillArchery @ " 10";
$SkillRestriction[IonBlasterCells] = $SkillArchery @ " 10";
//.................................................................................
$SkillRestriction[FragGrenade] = $SkillBashing @ " 1";
$SkillRestriction[IonGrenade] = $SkillBashing @ " 1";
$SkillRestriction[FlashGrenade] = $SkillBashing @ " 40";
$SkillRestriction[ConcussionGrenade] = $SkillBashing @ " 60";
$SkillRestriction[PoisonGrenade] = $SkillBashing @ " 140";
$SkillRestriction[SmokeGrenade] = $SkillBashing @ " 200";
$SkillRestriction[StunGrenade] = $SkillBashing @ " 220";
$SkillRestriction[CryoBanGrenade] = $SkillBashing @ " 310";
$SkillRestriction[SonicGrenade] = $SkillBashing @ " 450";
$SkillRestriction[PlasmaGrenade] = $SkillBashing @ " 520";
$SkillRestriction[ThermalDetonator] = $SkillBashing @ " 630";
$SkillRestriction[SmokeBomb] = $SkillBashing @ " 775";
$SkillRestriction[SonicDetonator] = $SkillBashing @ " 870";
$SkillRestriction[TripMine] = $SkillBashing @ " 200";


$SkillRestriction[RBlueLightsaber] = $SkillRestriction[BlueLightsaber];
$SkillRestriction[RGreenLightsaber] = $SkillRestriction[GreenLightsaber];
$SkillRestriction[RRedLightsaber] = $SkillRestriction[RedLightsaber];
$SkillRestriction[RDoubleRedLightsaber] = $SkillRestriction[DoubleRedLightsaber];
$SkillRestriction[RLongSword] = $SkillRestriction[LongSword];
$SkillRestriction[RMace] = $SkillRestriction[Mace];
$SkillRestriction[RPickAxe] = $SkillRestriction[PickAxe];
$SkillRestriction[RKnife] = $SkillRestriction[Knife];
$SkillRestriction[RDagger] = $SkillRestriction[Dagger];
$SkillRestriction[RShortSword] = $SkillRestriction[ShortSword];
$SkillRestriction[RBlasTechDL44] = $SkillRestriction[BlasTechDL44];
$SkillRestriction[RBaktoidE5] = $SkillArchery @ " 120";
$SkillRestriction[RBlasTechE11] = $SkillArchery @ " 300";

// Chat functions
$SkillRestriction["#say"] = $SkillCharisma @ " 0";
$SkillRestriction["#shout"] = $SkillCharisma @ " 3";
$SkillRestriction["#whisper"] = $SkillCharisma @ " 1";
$SkillRestriction["#tell"] = $SkillCharisma @ " 0";
$SkillRestriction["#global"] = $SkillCharisma @ " 0";
$SkillRestriction["#zone"] = $SkillCharisma @ " 0";
$SkillRestriction["#group"] = $SkillCharisma @ " 0";
$SkillRestriction["#party"] = $SkillCharisma @ " 0";

$SkillRestriction["#steal"] = $SkillStealing @ " 15";
$SkillRestriction["#pickpocket"] = $SkillStealing @ " 270";
$SkillRestriction["#mug"] = $SkillStealing @ " 620";
$SkillRestriction["#compass"] = $SkillSenseHeading @ " 3";
$SkillRestriction["#track"] = $SkillSenseHeading @ " 15";
$SkillRestriction["#trackpack"] = $SkillSenseHeading @ " 85";
$SkillRestriction["#hide"] = $SkillHiding @ " 15";
$SkillRestriction["#bash"] = $SkillBashing @ " 15";
$SkillRestriction["#shove"] = $SkillBashing @ " 5";
$SkillRestriction["#zonelist"] = $SkillSenseHeading @ " 45";
$SkillRestriction["#advcompass"] = $SkillSenseHeading @ " 20";

// Spells
$SkillRestriction[fear] = $SkillOffensiveCasting @ " 15";
$SkillRestriction[plague] = $SkillOffensiveCasting @ " 20";
$SkillRestriction[choke] = $SkillOffensiveCasting @ " 35";
$SkillRestriction[blind] = $SkillOffensiveCasting @ " 45";
$SkillRestriction[lightning] = $SkillOffensiveCasting @ " 85";
$SkillRestriction[drain] = $SkillOffensiveCasting @ " 110";
$SkillRestriction[rage] = $SkillOffensiveCasting @ " 145";
//$SkillRestriction[melt] = $SkillOffensiveCasting @ " 220";
//$SkillRestriction[powercloud] = $SkillOffensiveCasting @ " 340";
//$SkillRestriction[hellstorm] = $SkillOffensiveCasting @ " 420";
//$SkillRestriction[beam] = $SkillOffensiveCasting @ " 520";
//$SkillRestriction[dimensionrift] = $SkillOffensiveCasting @ " 750";

$SkillRestriction[teleport] = $SkillEnergy @ " 60";
$SkillRestriction[advteleport] = $SkillEnergy @ " 130";
$SkillRestriction[transport] = $SkillEnergy @ " 200";
$SkillRestriction[advtransport] = $SkillEnergy @ " 350";
$SkillRestriction[transportfriend] = $SkillEnergy @ " 430";
$SkillRestriction[masstransportfriend] = $SkillEnergy @ " 500 " @ $MinRemort @ " 1";
$SkillRestriction[masstransport] = $SkillEnergy @ " 650 " @ $MinRemort @ " 1";
$SkillRestriction[remort] = $SkillEnergy @ " 0 " @ $MinLevel @ " 101";
$SkillRestriction[mimic] = $SkillEnergy @ " 145 " @ $MinRemort @ " 2";
$SkillRestriction[push] = $SkillEnergy @ " 5";
$SkillRestriction[pull] = $SkillEnergy @ " 10";

$SkillRestriction[heal] = $SkillDefensiveCasting @ " 10";
$SkillRestriction[fullheal] = $SkillDefensiveCasting @ " 750";
$SkillRestriction[massheal] = $SkillDefensiveCasting @ " 850 " @ $MinRemort @ " 2";
$SkillRestriction[massfullheal] = $SkillDefensiveCasting @ " 950 " @ $MinRemort @ " 3";
$SkillRestriction[protect] = $SkillDefensiveCasting @ " 20";
//$SkillRestriction[massprotect] = $SkillDefensiveCasting @ " 680 " @ $MinRemort @ " 2";
$SkillRestriction[absorb] = $SkillDefensiveCasting @ " 20";

//$SkillRestriction[massabsorb] = $SkillDefensiveCasting @ " 680 " @ $MinRemort @ " 2";
//}

//######################################################################################
// Skill functions
//######################################################################################

function GetNumSkills()
{
	dbecho($dbechoMode, "GetNumSkills()");

	for(%i = 1; $SkillDesc[%i] != ""; %i++){}
	return %i-1;
}

function AddSkillPoint(%clientId, %skill, %delta)
{
	dbecho($dbechoMode, "AddSkillPoint(" @ %clientId @ ", " @ %skill @ ", " @ %delta @ ")");

	if(%delta == "")
		%delta = 1;


	//==== CAPS ================
	//if($PlayerSkill[%clientId, %skill] >= $SkillCap)
	//	return False;

	%ub = ($skillRangePerLevel * fetchData(%clientId, "LVL")) + 20 + fetchData(%clientId, "RemortStep");
	if($PlayerSkill[%clientId, %skill] >= %ub)
	{
		client::sendMessage(%clientId, 1, "Your skill has reached its current limit.");
		return False;
	} //Thanks to Jobo for this idea. It used to say nothing when you tried to raise a skill past its limit, potentially causing confusion.
	//==========================

	%a = GetSkillMultiplier(%clientId, %skill) * %delta;
	%b = $PlayerSkill[%clientId, %skill];
	%c = %a + %b;
	%d = round(%c * 10);
	%e = (%d / 10) * 1.000001;

	$PlayerSkill[%clientId, %skill] = %e;

	return True;
}

function GetPlayerSkill(%clientId, %skill)
{
	return $PlayerSkill[%clientId, %skill];
}
function GetSkillMultiplier(%clientId, %skill)
{
	dbecho($dbechoMode, "GetSkillMultiplier(" @ %clientId @ ", " @ %skill @ ")");

	%a = $SkillMultiplier[fetchData(%clientId, "CLASS"), %skill];
	%b = fetchData(%clientId, "RemortStep") * 0.1;

	%c = Cap(%a + %b, "inf", 30.0);

	return FixDecimals(%c);
}
function GetEXPmultiplier(%clientId)
{
	dbecho($dbechoMode, "GetEXPmultiplier(" @ %clientId @ ")");

	%a = $EXPmultiplier[fetchData(%clientId, "CLASS")];
	%b = fetchData(%clientId, "RemortStep") * 0.5;

	%c = %a + %b;

	return FixDecimals(%c);
}

function SetAllSkills(%clientId, %n)
{
	dbecho($dbechoMode, "SetAllSkills(" @ %clientId @ ", " @ %n @ ")");

	for(%i = 1; $SkillDesc[%i] != ""; %i++)
		$PlayerSkill[%clientId, %i] = %n;
}

function SkillCanUse(%clientId, %thing)
{
	dbecho($dbechoMode, "SkillCanUse(" @ %clientId @ ", " @ %thing @ ")");

	if(%clientId.adminLevel >= 5)
		return True;

	%flag = 0;
	%gc = 0;
	%gcflag = 0;
	for(%i = 0; GetWord($SkillRestriction[%thing], %i) != -1; %i+=2)
	{
		%s = GetWord($SkillRestriction[%thing], %i);
		%n = GetWord($SkillRestriction[%thing], %i+1);

		if(%s == "L")
		{
			if(fetchData(%clientId, "LVL") < %n)
				%flag = 1;
		}
		else if(%s == "R")
		{
			if(fetchData(%clientId, "RemortStep") < %n)
				%flag = 1;
		}
		else if(%s == "A")
		{
			if(%clientId.adminLevel < %n)
				%flag = 1;
		}
		else if(%s == "G")
		{
			%gcflag++;
			if(String::ICompare(fetchData(%clientId, "GROUP"), %n) == 0)
				%gc = 1;
		}
		else if(%s == "C")
		{
			%gcflag++;
			if(String::ICompare(fetchData(%clientId, "CLASS"), %n) == 0)
				%gc = 1;
		}
		else if(%s == "H")
		{
			%hflag++;
			if(String::ICompare(fetchData(%clientId, "MyHouse"), %n) == 0)
				%hh = 1;
		}
		else if(%s == "FMin")
		{
			%hflag++;
			if(fetchData(%clientId, "Alignment") >= %n)
				%flag = 1;
		}
		else if(%s == "FMax")
		{
			%hflag++;
			if(fetchData(%clientId, "Alignment") <= %n)
				%flag = 1;
		}
		else
		{
			if($PlayerSkill[%clientId, %s] < %n)
				%flag = 1;
		}
	}

	//First, if there are any class/group restrictions, house restrictions, check these first.
	if(%gcflag > 0)
	{
		if(%gc == 0)
			%flag = 1;
	}
	if(%hflag > 0)
	{
		if(%hh == 0)
			%flag = 1;
	}

	
	if(%flag != 1)
		return True;
	else
		return False;
}

function UseSkill(%clientId, %skilltype, %successful, %showmsg, %base, %refreshall)
{
	dbecho($dbechoMode, "UseSkill(" @ %clientId @ ", " @ %skilltype @ ", " @ %successful @ ", " @ %showmsg @ ", " @ %base @ ", " @ %refreshall @ ")");

	if(%base == "") %base = 35;

	%ub = ($skillRangePerLevel * fetchData(%clientId, "LVL")) + 20 + fetchData(%clientId, "RemortStep");
	if($PlayerSkill[%clientId, %skilltype] < %ub)
	{
		if(%successful)
			$SkillCounter[%clientId, %skilltype] += 1;
		else
			$SkillCounter[%clientId, %skilltype] += 0.05;

		%p = 1 - ($PlayerSkill[%clientId, %skilltype] / 1150);
		%e = round( (%base / GetSkillMultiplier(%clientId, %skilltype)) * %p );

		if($SkillCounter[%clientId, %skilltype] >= %e)
		{
			$SkillCounter[%clientId, %skilltype] = 0;
			%retval = AddSkillPoint(%clientId, %skilltype, 1);

			if(%retval)
			{
				if(%showmsg)
					Client::sendMessage(%clientId, $MsgBeige, "You have increased your skill in " @ $SkillDesc[%skilltype] @ " (" @ $PlayerSkill[%clientId, %skilltype] @ ")");
				if(%refreshall)
					RefreshAll(%clientId);
			}
		}
	}
}

function WhatSkills(%thing)
{
	dbecho($dbechoMode, "WhatSkills(" @ %thing @ ")");

	%t = "";
	for(%i = 0; GetWord($SkillRestriction[%thing], %i) != -1; %i+=2)
	{
		%s = GetWord($SkillRestriction[%thing], %i);
		%n = GetWord($SkillRestriction[%thing], %i+1);

		%t = %t @ $SkillDesc[%s] @ ": " @ %n @ ", ";
	}
	if(%t == "")
		%t = "None";
	else
		%t = String::getSubStr(%t, 0, String::len(%t)-2);
	
	return %t;
}

function GetSkillAmount(%thing, %skill)
{
	dbecho($dbechoMode, "GetSkillAmount(" @ %thing @ ", " @ %skill @ ")");

	for(%i = 0; GetWord($SkillRestriction[%thing], %i) != -1; %i+=2)
	{
		%s = GetWord($SkillRestriction[%thing], %i);

		if(%s == %skill)
			return GetWord($SkillRestriction[%thing], %i+1);
	}
	return 0;
}

//######################################################################################
// Command and Skill #w infos. Not many more suitable locations for these, so I dropped 'em here.
//######################################################################################
//$MiscInfo = 5, $MiscoInfo hasn't been declared yet. Accessory.cs(Where it's defined) is executed after this.
//So I replaced $MiscoInfo ($AV[%item, $MiscInfo]) with 5, which is what $MiscInfo will ==
$AccessoryVar[blasters, 5] = "HoldoutBlaster, BlasTechDL44, NoobianS5, BaktoidE5, BlasTechE11, EE3Carbine, BlasTechDLT19, HeavyCrossbow, BlasTechT21, HeavyRepeater.";
$AccessoryVar[meleeweapons, 5] = "omghi2u";
$AccessoryVar[melee, 5] = $AccessoryVar[meleeweapons, 5];
$AccessoryVar[lightsabers, 5] = "TrainingLightsaber, BlueLightsaber, GreenLightsaber, RedLightsaber, DoubleRedLightsaber, HikenLightsaber.";
$AccessoryVar[tactics, 5] = "See: #w grenades. Also, this contains #bash, and likely will have backstabbing combined with it.";
$AccessoryVar[grenade, 5] = "FragGrenade, FlashGrenade, IonGrenade, ConcussionGrenade, PoisonGrenade, SmokeGrenade, StunGrenade, CryoBanGrenade, SonicGrenade, PlasmaGrenade, ThermalDetonator, SmokeBomb, SonicDetonator, TripMine";
$AccessoryVar[grenades, 5] = $AccessoryVar[grenade, 5];
$AccessoryVar[dodging, 5] = "Your ability to dodge attacks.";
$AccessoryVar[vitality, 5] = "Your hit-points, and ability to wear armor. (Same as Endurance in base RPG.)";
$AccessoryVar[healing, 5] = "Your natural ability to heal. (Same as in base RPG.)";
$AccessoryVar[weightcapacity, 5] = "Your physical strength. (Same as in base RPG.)";
$AccessoryVar[stealing, 5] = "Your skill with stealing. Commands: #steal, #pickpocket, #mug.";
$AccessoryVar[hiding, 5] = "Your ability to conceal yourself. Commands: #hide.";
$AccessoryVar[backstabbing, 5] = "(Same as in base RPG.)";
$AccessoryVar[charisma, 5] = "Your linguistic abilities, and haggling skills.";
$AccessoryVar[mining, 5] = "(Same as in base RPG.)";
$AccessoryVar[senseheading, 5] = "(Same as in base RPG.) Commands: #compass, #advcompass, #track, #trackpack, #zonelist.";
$AccessoryVar[lightsideaffinity, 5] = "Your affinity with the light side of the force. This measures how capable you are with the use of light-side force powers.";
$AccessoryVar[lightaffinity, 5] = $AccessoryVar[lightsideaffinity, 5];
$AccessoryVar[lightside, 5] = $AccessoryVar[lightsideaffinity, 5];
$AccessoryVar[darksideaffinity, 5] = "Your affinity with the dark side of the force. This measures how capable you are with the use of dark-side force powers.";
$AccessoryVar[darkaffinity, 5] = $AccessoryVar[darksideaffinity, 5];
$AccessoryVar[darkside, 5] = $AccessoryVar[darksideaffinity, 5];
$AccessoryVar["forceattunement(energy)", 5] = "Your attunement to the force. This measures how much you can make use of the force, and your ability to use neutral force powers.";
$AccessoryVar[forceattunementenergy, 5] = $AccessoryVar["forceattunement(energy)", 5];
$AccessoryVar[forceattunement, 5] = $AccessoryVar["forceattunement(energy)", 5];
$AccessoryVar[energy, 5] = $AccessoryVar["forceattunement(energy)", 5];
$AccessoryVar[forceresistance, 5] = "Your ability to resist the power of the force.";
$AccessoryVar[forceresist, 5] = $AccessoryVar[forceresistance, 5];

$AccessoryVar[#say, 5] = "This will send a message to everyone near you.";
$AccessoryVar[#shout, 5] = "This will send a message to everyone near you.";
$AccessoryVar[#whisper, 5] = "This will send a message to anyone who is very close to you.";
$AccessoryVar[#t, 5] = "This will send a message to a specific person.";
$AccessoryVar[#telll, 5] = $AccessoryVar[#t, 5];
$AccessoryVar[#pm, 5] = $AccessoryVar[#t, 5];
$AccessoryVar[#g, 5] = "This will send a message to everyone in the server.";
$AccessoryVar[#glbl, 5] = $AccessoryVar[#g, 5];
$AccessoryVar[#global, 5] = $AccessoryVar[#g, 5];
$AccessoryVar[#z, 5] = "This will send a message to everyone who is in the same zone as you.";
$AccessoryVar[#zone, 5] = $AccessoryVar[#z, 5];
$AccessoryVar[#group, 5] = "This will send a message to everyone in your group.";
$AccessoryVar[#p, 5] = "This will send a message to everyone in your party.";
$AccessoryVar[#party, 5] = $AccessoryVar[#p, 5];