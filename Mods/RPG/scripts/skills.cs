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


//######################################################################################
// Skills
//######################################################################################

$SkillSlashing = 1;
$SkillPiercing = 2;
$SkillBludgeoning = 3;
$SkillDodging = 4;
$SkillWeightCapacity = 5;
$SkillBashing = 6;
$SkillStealing = 7;
$SkillHiding = 8;
$SkillBackstabbing = 9;
$SkillOffensiveCasting = 10;
$SkillDefensiveCasting = 11;
$SkillSpellResistance = 12;
$SkillHealing = 13;
$SkillArchery = 14;
$SkillEndurance = 15;
$SkillMining = 17;
$SkillSpeech = 18;
$SkillSenseHeading = 19;
$SkillEnergy = 20;
$SkillHaggling = 21;
$SkillNeutralCasting = 22;
$MinLevel = "L";
$MinGroup = "G";
$MinClass = "C";
$MinRemort = "R";
$MinAdmin = "A";
$MinHouse = "H";

$SkillDesc[1] = "Slashing";
$SkillDesc[2] = "Piercing";
$SkillDesc[3] = "Bludgeoning";
$SkillDesc[4] = "Dodging";
$SkillDesc[5] = "Weight Capacity";
$SkillDesc[6] = "Bashing";
$SkillDesc[7] = "Stealing";
$SkillDesc[8] = "Hiding";
$SkillDesc[9] = "Backstabbing";
$SkillDesc[10] = "Offensive Casting";
$SkillDesc[11] = "Defensive Casting";
$SkillDesc[12] = "Spell Resistance";
$SkillDesc[13] = "Healing";
$SkillDesc[14] = "Archery";
$SkillDesc[15] = "Endurance";
$SkillDesc[16] = "(no longer used)";
$SkillDesc[17] = "Mining";
$SkillDesc[18] = "Speech";
$SkillDesc[19] = "Sense Heading";
$SkillDesc[20] = "Energy";
$SkillDesc[21] = "Haggling";
$SkillDesc[22] = "Neutral Casting";
$SkillDesc[L] = "Level";
$SkillDesc[G] = "Group";
$SkillDesc[C] = "Class";
$SkillDesc[R] = "Remort";
$SkillDesc[A] = "Admin Level";
$SkillDesc[H] = "House";

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
// Cleric
//--------------
// Clerics are good with Bludgeoning weapons but VERY good at healing spells.  They also
// know the basics behind offensive spells.

//Primary Skill: Defensive Casting
//Secondary Skills: Healing, Energy, Bludgeoning

$SkillMultiplier[Cleric, $SkillSlashing] = 0.6;
$SkillMultiplier[Cleric, $SkillPiercing] = 0.7;
$SkillMultiplier[Cleric, $SkillBludgeoning] = 1.5;
$SkillMultiplier[Cleric, $SkillDodging] = 0.7;
$SkillMultiplier[Cleric, $SkillWeightCapacity] = 1.0;
$SkillMultiplier[Cleric, $SkillBashing] = 0.5;
$SkillMultiplier[Cleric, $SkillStealing] = 0.2;
$SkillMultiplier[Cleric, $SkillHiding] = 0.2;
$SkillMultiplier[Cleric, $SkillBackstabbing] = 0.2;
$SkillMultiplier[Cleric, $SkillOffensiveCasting] = 0.9;
$SkillMultiplier[Cleric, $SkillDefensiveCasting] = 2.0;
$SkillMultiplier[Cleric, $SkillNeutralCasting] = 1.2;
$SkillMultiplier[Cleric, $SkillSpellResistance] = 1.5;
$SkillMultiplier[Cleric, $SkillHealing] = 2.0;
$SkillMultiplier[Cleric, $SkillArchery] = 0.5;
$SkillMultiplier[Cleric, $SkillEndurance] = 1.1;
$SkillMultiplier[Cleric, $SkillMining] = 1.0;
$SkillMultiplier[Cleric, $SkillSpeech] = 1.0;
$SkillMultiplier[Cleric, $SkillSenseHeading] = 1.0;
$SkillMultiplier[Cleric, $SkillEnergy] = 1.5;
$SkillMultiplier[Cleric, $SkillHaggling] = 1.0;
$EXPmultiplier[Cleric] = 0.85;

//--------------
// Druid
//--------------
// Druids are good with Bludgeoning weapons and are somewhat familiar with spells.  They specialize in Neutral casting.
// However they are also able to easily hide.

//Primary Skill: Neutral Casting
//Secondary Skill: Hiding, Slashing, Spell Resistance

$SkillMultiplier[Druid, $SkillSlashing] = 1.5;
$SkillMultiplier[Druid, $SkillPiercing] = 0.7;
$SkillMultiplier[Druid, $SkillBludgeoning] = 0.6;
$SkillMultiplier[Druid, $SkillDodging] = 2.0;
$SkillMultiplier[Druid, $SkillWeightCapacity] = 2.0;
$SkillMultiplier[Druid, $SkillBashing] = 0.5;
$SkillMultiplier[Druid, $SkillStealing] = 0.2;
$SkillMultiplier[Druid, $SkillHiding] = 2.0;
$SkillMultiplier[Druid, $SkillBackstabbing] = 0.5;
$SkillMultiplier[Druid, $SkillOffensiveCasting] = 0.7;
$SkillMultiplier[Druid, $SkillDefensiveCasting] = 0.7;
$SkillMultiplier[Druid, $SkillNeutralCasting] = 2.0;
$SkillMultiplier[Druid, $SkillSpellResistance] = 1.0;
$SkillMultiplier[Druid, $SkillHealing] = 1.3;
$SkillMultiplier[Druid, $SkillArchery] = 0.7;
$SkillMultiplier[Druid, $SkillEndurance] = 0.8;
$SkillMultiplier[Druid, $SkillMining] = 2.0;
$SkillMultiplier[Druid, $SkillSpeech] = 1.0;
$SkillMultiplier[Druid, $SkillSenseHeading] = 1.7;
$SkillMultiplier[Druid, $SkillEnergy] = 1.2;
$SkillMultiplier[Druid, $SkillHaggling] = 1.3;
$EXPmultiplier[Druid] = 0.8;

//--------------
// Thief
//--------------
//Thieves handle piercing weapons well enough, and are very good at hiding and backstabbing.
//And of course, they are great at stealing.

//Primary Skill: Stealing
//Secondary Skill: Hiding, Backstabbing, Piercing, Archery

$SkillMultiplier[Thief, $SkillSlashing] = 0.6;
$SkillMultiplier[Thief, $SkillPiercing] = 1.8;
$SkillMultiplier[Thief, $SkillBludgeoning] = 0.5;
$SkillMultiplier[Thief, $SkillDodging] = 1.1;
$SkillMultiplier[Thief, $SkillWeightCapacity] = 0.7;
$SkillMultiplier[Thief, $SkillBashing] = 0.2;
$SkillMultiplier[Thief, $SkillStealing] = 2.0;
$SkillMultiplier[Thief, $SkillHiding] = 2.0;
$SkillMultiplier[Thief, $SkillBackstabbing] = 2.0;
$SkillMultiplier[Thief, $SkillOffensiveCasting] = 0.2;
$SkillMultiplier[Thief, $SkillDefensiveCasting] = 0.2;
$SkillMultiplier[Thief, $SkillNeutralCasting] = 0.2;
$SkillMultiplier[Thief, $SkillSpellResistance] = 0.3;
$SkillMultiplier[Thief, $SkillHealing] = 0.5;
$SkillMultiplier[Thief, $SkillArchery] = 1.6;
$SkillMultiplier[Thief, $SkillEndurance] = 1.0;
$SkillMultiplier[Thief, $SkillMining] = 1.0;
$SkillMultiplier[Thief, $SkillSpeech] = 1.0;
$SkillMultiplier[Thief, $SkillSenseHeading] = 1.0;
$SkillMultiplier[Thief, $SkillEnergy] = 0.5;
$SkillMultiplier[Thief, $SkillHaggling] = 1.5;
$EXPmultiplier[Thief] = 0.8;

//--------------
// Bard
//--------------
//Bards are much like thieves, except that they are a bit more evenly balanced.

//Primary Skill: Stealing
//Secondary Skill: Archery

$SkillMultiplier[Bard, $SkillSlashing] = 1.3;
$SkillMultiplier[Bard, $SkillPiercing] = 1.5;
$SkillMultiplier[Bard, $SkillBludgeoning] = 1.3;
$SkillMultiplier[Bard, $SkillDodging] = 2.0;
$SkillMultiplier[Bard, $SkillWeightCapacity] = 0.8;
$SkillMultiplier[Bard, $SkillBashing] = 0.2;
$SkillMultiplier[Bard, $SkillStealing] = 2.0;
$SkillMultiplier[Bard, $SkillHiding] = 1.8;
$SkillMultiplier[Bard, $SkillBackstabbing] = 1.8;
$SkillMultiplier[Bard, $SkillOffensiveCasting] = 0.3;
$SkillMultiplier[Bard, $SkillDefensiveCasting] = 0.3;
$SkillMultiplier[Bard, $SkillNeutralCasting] = 0.5;
$SkillMultiplier[Bard, $SkillSpellResistance] = 0.5;
$SkillMultiplier[Bard, $SkillHealing] = 2.0;
$SkillMultiplier[Bard, $SkillArchery] = 1.4;
$SkillMultiplier[Bard, $SkillEndurance] = 2.0;
$SkillMultiplier[Bard, $SkillMining] = 2.0;
$SkillMultiplier[Bard, $SkillSpeech] = 1.0;
$SkillMultiplier[Bard, $SkillSenseHeading] = 1.5;
$SkillMultiplier[Bard, $SkillEnergy] = 0.6;
$SkillMultiplier[Bard, $SkillHaggling] = 2.0;
$EXPmultiplier[Bard] = 0.8;

//--------------
// Fighter
//--------------
// Fighters are great with swords, namely slashing weapons.  They are strong, but dumb.
// They know nothing when it comes to spells.  However they can easily wear armor and
// wield all kinds of weapons.

//Primary Skill: Slashing
//Secondary Skill: Bludgeoning

$SkillMultiplier[Fighter, $SkillSlashing] = 2.0;
$SkillMultiplier[Fighter, $SkillPiercing] = 2.0;
$SkillMultiplier[Fighter, $SkillBludgeoning] = 2.0;
$SkillMultiplier[Fighter, $SkillDodging] = 1.5;
$SkillMultiplier[Fighter, $SkillWeightCapacity] = 1.5;
$SkillMultiplier[Fighter, $SkillBashing] = 1.6;
$SkillMultiplier[Fighter, $SkillStealing] = 0.2;
$SkillMultiplier[Fighter, $SkillHiding] = 0.2;
$SkillMultiplier[Fighter, $SkillBackstabbing] = 0.2;
$SkillMultiplier[Fighter, $SkillOffensiveCasting] = 0.1;
$SkillMultiplier[Fighter, $SkillDefensiveCasting] = 0.1;
$SkillMultiplier[Fighter, $SkillNeutralCasting] = 0.1;
$SkillMultiplier[Fighter, $SkillSpellResistance] = 0.2;
$SkillMultiplier[Fighter, $SkillHealing] = 1.2;
$SkillMultiplier[Fighter, $SkillArchery] = 1.6;
$SkillMultiplier[Fighter, $SkillEndurance] = 1.6;
$SkillMultiplier[Fighter, $SkillMining] = 1.0;
$SkillMultiplier[Fighter, $SkillSpeech] = 0.8;
$SkillMultiplier[Fighter, $SkillSenseHeading] = 0.4;
$SkillMultiplier[Fighter, $SkillEnergy] = 0.2;
$SkillMultiplier[Fighter, $SkillHaggling] = 1.0;
$EXPmultiplier[Fighter] = 1.0;

//--------------
// Paladin
//--------------
//Paladins are much like Fighters, except that they are a bit more evenly balanced.

//Primary Skill: Bludgeoning
//Secondary Skill: Healing

$SkillMultiplier[Paladin, $SkillSlashing] = 1.5;
$SkillMultiplier[Paladin, $SkillPiercing] = 1.5;
$SkillMultiplier[Paladin, $SkillBludgeoning] = 1.2;
$SkillMultiplier[Paladin, $SkillDodging] = 1.5;
$SkillMultiplier[Paladin, $SkillWeightCapacity] = 1.5;
$SkillMultiplier[Paladin, $SkillBashing] = 1.5;
$SkillMultiplier[Paladin, $SkillStealing] = 0.3;
$SkillMultiplier[Paladin, $SkillHiding] = 0.3;
$SkillMultiplier[Paladin, $SkillBackstabbing] = 0.3;
$SkillMultiplier[Paladin, $SkillOffensiveCasting] = 0.2;
$SkillMultiplier[Paladin, $SkillDefensiveCasting] = 1.2;
$SkillMultiplier[Paladin, $SkillNeutralCasting] = 0.3;
$SkillMultiplier[Paladin, $SkillSpellResistance] = 0.9;
$SkillMultiplier[Paladin, $SkillHealing] = 1.3;
$SkillMultiplier[Paladin, $SkillArchery] = 1.2;
$SkillMultiplier[Paladin, $SkillEndurance] = 1.5;
$SkillMultiplier[Paladin, $SkillMining] = 1.0;
$SkillMultiplier[Paladin, $SkillSpeech] = 0.8;
$SkillMultiplier[Paladin, $SkillSenseHeading] = 0.5;
$SkillMultiplier[Paladin, $SkillEnergy] = 0.9;
$SkillMultiplier[Paladin, $SkillHaggling] = 1.3;
$EXPmultiplier[Paladin] = 1.0;

//--------------
// Ranger
//--------------
// Rangers specialize in ranged weaponry.  They are also good at finding their way when lost.
// They can also wear armors and wield weapons easily enough.

//Primary Skill: Archery
//Secondary Skills: Slashing, Sense Heading

$SkillMultiplier[Ranger, $SkillSlashing] = 1.2;
$SkillMultiplier[Ranger, $SkillPiercing] = 1.1;
$SkillMultiplier[Ranger, $SkillBludgeoning] = 1.2;
$SkillMultiplier[Ranger, $SkillDodging] = 1.8;
$SkillMultiplier[Ranger, $SkillWeightCapacity] = 1.0;
$SkillMultiplier[Ranger, $SkillBashing] = 0.9;
$SkillMultiplier[Ranger, $SkillStealing] = 0.5;
$SkillMultiplier[Ranger, $SkillHiding] = 1.0;
$SkillMultiplier[Ranger, $SkillBackstabbing] = 0.4;
$SkillMultiplier[Ranger, $SkillOffensiveCasting] = 0.2;
$SkillMultiplier[Ranger, $SkillDefensiveCasting] = 0.4;
$SkillMultiplier[Ranger, $SkillNeutralCasting] = 0.3;
$SkillMultiplier[Ranger, $SkillSpellResistance] = 0.2;
$SkillMultiplier[Ranger, $SkillHealing] = 0.8;
$SkillMultiplier[Ranger, $SkillArchery] = 2.0;
$SkillMultiplier[Ranger, $SkillEndurance] = 1.2;
$SkillMultiplier[Ranger, $SkillMining] = 1.0;
$SkillMultiplier[Ranger, $SkillSpeech] = 1.0;
$SkillMultiplier[Ranger, $SkillSenseHeading] = 2.0;
$SkillMultiplier[Ranger, $SkillEnergy] = 0.7;
$SkillMultiplier[Ranger, $SkillHaggling] = 0.7;
$EXPmultiplier[Ranger] = 0.95;

//--------------
// Mage
//--------------
// Mages are horrible with weapons and armor, but excel in anything that
// relates to spells.

//Primary Skill: Offensive Casting
//Secondary Skills: Energy

$SkillMultiplier[Mage, $SkillSlashing] = 0.3;
$SkillMultiplier[Mage, $SkillPiercing] = 0.8;
$SkillMultiplier[Mage, $SkillBludgeoning] = 0.3;
$SkillMultiplier[Mage, $SkillDodging] = 1.2;
$SkillMultiplier[Mage, $SkillWeightCapacity] = 0.6;
$SkillMultiplier[Mage, $SkillBashing] = 0.2;
$SkillMultiplier[Mage, $SkillStealing] = 0.2;
$SkillMultiplier[Mage, $SkillHiding] = 0.2;
$SkillMultiplier[Mage, $SkillBackstabbing] = 0.2;
$SkillMultiplier[Mage, $SkillOffensiveCasting] = 2.0;
$SkillMultiplier[Mage, $SkillDefensiveCasting] = 1.0;
$SkillMultiplier[Mage, $SkillNeutralCasting] = 1.8;
$SkillMultiplier[Mage, $SkillSpellResistance] = 1.5;
$SkillMultiplier[Mage, $SkillHealing] = 0.7;
$SkillMultiplier[Mage, $SkillArchery] = 0.8;
$SkillMultiplier[Mage, $SkillEndurance] = 0.4;
$SkillMultiplier[Mage, $SkillMining] = 1.0;
$SkillMultiplier[Mage, $SkillSpeech] = 1.2;
$SkillMultiplier[Mage, $SkillSenseHeading] = 0.7;
$SkillMultiplier[Mage, $SkillEnergy] = 2.0;
$SkillMultiplier[Mage, $SkillHaggling] = 1.0;
$EXPmultiplier[Mage] = 1.0;

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

$SkillRestriction[BluePotion] = $SkillHealing @ " 0";
$SkillRestriction[CrystalBluePotion] = $SkillHealing @ " 0";
$SkillRestriction[ApprenticeRobe] = $SkillEndurance @ " 0 " @ $SkillEnergy @ " 8";
$SkillRestriction[LightRobe] = $SkillEndurance @ " 3 " @ $SkillEnergy @ " 80";
$SkillRestriction[FineRobe] = $SkillEndurance @ " 9 " @ $SkillEnergy @ " 175";
$SkillRestriction[BloodRobe] = $SkillEndurance @ " 8 " @ $SkillEnergy @ " 300";
$SkillRestriction[AdvisorRobe] = $SkillEndurance @ " 10 " @ $SkillEnergy @ " 450";
$SkillRestriction[ElvenRobe] = $SkillEndurance @ " 12 " @ $SkillEnergy @ " 620";
$SkillRestriction[RobeOfVenjance] = $SkillEndurance @ " 18 " @ $SkillEnergy @ " 800";
$SkillRestriction[PhensRobe] = $SkillEndurance @ " 20 " @ $SkillEnergy @ " 980";
$SkillRestriction[QuestMasterRobe] = $MinAdmin @ " 3";

$SkillRestriction[PaddedArmor] = $SkillEndurance @ " 5";
$SkillRestriction[LeatherArmor] = $SkillEndurance @ " 40";
$SkillRestriction[StuddedLeather] = $SkillEndurance @ " 95";
$SkillRestriction[SpikedLeather] = $SkillEndurance @ " 135";
$SkillRestriction[HideArmor] = $SkillEndurance @ " 180";
$SkillRestriction[ScaleMail] = $SkillEndurance @ " 240";
$SkillRestriction[BrigandineArmor] = $SkillEndurance @ " 300";
$SkillRestriction[ChainMail] = $SkillEndurance @ " 350";
$SkillRestriction[RingMail] = $SkillEndurance @ " 410";
$SkillRestriction[BandedMail] = $SkillEndurance @ " 490";
$SkillRestriction[SplintMail] = $SkillEndurance @ " 580";
$SkillRestriction[BronzePlateMail] = $SkillEndurance @ " 660";
$SkillRestriction[PlateMail] = $SkillEndurance @ " 775";
$SkillRestriction[FieldPlateArmor] = $SkillEndurance @ " 840";
$SkillRestriction[DragonMail] = $SkillEndurance @ " 950";
$SkillRestriction[FullPlateArmor] = $SkillEndurance @ " 1065";
$SkillRestriction[CheetaursPaws] = $MinLevel @ " 8";
$SkillRestriction[BootsOfGliding] = $MinLevel @ " 25";
$SkillRestriction[WindWalkers] = $MinLevel @ " 60";
$SkillRestriction[KeldrinArmor] = $SkillEndurance @ " 1305";

$SkillRestriction[KnightShield] = $SkillEndurance @ " 140";
$SkillRestriction[HeavenlyShield] = $SkillEndurance @ " 540 " @ $SkillEnergy @ " 850";
$SkillRestriction[DragonShield] = $SkillEndurance @ " 715";

$SkillRestriction[Hatchet] = $SkillSlashing @ " 0";
$SkillRestriction[BroadSword] = $SkillSlashing @ " 20";
$SkillRestriction[WarAxe] = $SkillSlashing @ " 60";
$SkillRestriction[LongSword] = $SkillSlashing @ " 140";
$SkillRestriction[BattleAxe] = $SkillSlashing @ " 300";
$SkillRestriction[BastardSword] = $SkillSlashing @ " 620";
$SkillRestriction[Halberd] = $SkillSlashing @ " 768";
$SkillRestriction[Claymore] = $SkillSlashing @ " 900";
$SkillRestriction[KeldriniteLS] = $SkillSlashing @ " 1120 " @ $MinRemort @ " 1";
//.................................................................................
$SkillRestriction[Club] = $SkillBludgeoning @ " 0";
$SkillRestriction[QuarterStaff] = $SkillBludgeoning @ " 20";
$SkillRestriction[BoneClub] = $SkillBludgeoning @ " 45";
$SkillRestriction[SpikedClub] = $SkillBludgeoning @ " 60";
$SkillRestriction[Mace] = $SkillBludgeoning @ " 140";
$SkillRestriction[HammerPick] = $SkillBludgeoning @ " 300";
$SkillRestriction[SpikedBoneClub] = $SkillBludgeoning @ " 450";
$SkillRestriction[LongStaff] = $SkillBludgeoning @ " 620";
$SkillRestriction[WarHammer] = $SkillBludgeoning @ " 768";
$SkillRestriction[JusticeStaff] = $SkillBludgeoning @ " 834";
$SkillRestriction[WarMaul] = $SkillBludgeoning @ " 900";
//.................................................................................
$SkillRestriction[PickAxe] = $SkillPiercing @ " 0";
$SkillRestriction[Knife] = $SkillPiercing @ " 0";
$SkillRestriction[Dagger] = $SkillPiercing @ " 60";
$SkillRestriction[ShortSword] = $SkillPiercing @ " 140";
$SkillRestriction[Spear] = $SkillPiercing @ " 280";
$SkillRestriction[Gladius] = $SkillPiercing @ " 450";
$SkillRestriction[Trident] = $SkillPiercing @ " 620";
$SkillRestriction[Rapier] = $SkillPiercing @ " 768";
$SkillRestriction[AwlPike] = $SkillPiercing @ " 900";
//.................................................................................
$SkillRestriction[Sling] = $SkillArchery @ " 0";
$SkillRestriction[ShortBow] = $SkillArchery @ " 25";
$SkillRestriction[LightCrossbow] = $SkillArchery @ " 160";
$SkillRestriction[LongBow] = $SkillArchery @ " 318";
$SkillRestriction[CompositeBow] = $SkillArchery @ " 438";
$SkillRestriction[RepeatingCrossbow] = $SkillArchery @ " 550";
$SkillRestriction[ElvenBow] = $SkillArchery @ " 685";
$SkillRestriction[AeolusWing] = $SkillArchery @ " 805";
$SkillRestriction[HeavyCrossbow] = $SkillArchery @ " 925";
//.................................................................................
$SkillRestriction[SmallRock] = $SkillArchery @ " 0";
$SkillRestriction[BasicArrow] = $SkillArchery @ " 0";
$SkillRestriction[ShortQuarrel] = $SkillArchery @ " 0";
$SkillRestriction[LightQuarrel] = $SkillArchery @ " 0";
$SkillRestriction[SheafArrow] = $SkillArchery @ " 0";
$SkillRestriction[StoneFeather] = $SkillArchery @ " 0";
$SkillRestriction[BladedArrow] = $SkillArchery @ " 0";
$SkillRestriction[HeavyQuarrel] = $SkillArchery @ " 0";
$SkillRestriction[MetalFeather] = $SkillArchery @ " 0";
$SkillRestriction[Talon] = $SkillArchery @ " 0";
$SkillRestriction[CeraphumsFeather] = $SkillArchery @ " 0";


$SkillRestriction[RHatchet] = $SkillRestriction[Hatchet];
$SkillRestriction[RBroadSword] = $SkillRestriction[BroadSword];
$SkillRestriction[RWarAxe] = $SkillRestriction[WarAxe];
$SkillRestriction[RLongSword] = $SkillRestriction[LongSword];
$SkillRestriction[RClub] = $SkillRestriction[Club];
$SkillRestriction[RSpikedClub] = $SkillRestriction[SpikedClub];
$SkillRestriction[RPickAxe] = $SkillRestriction[PickAxe];
$SkillRestriction[RKnife] = $SkillRestriction[Knife];
$SkillRestriction[RDagger] = $SkillRestriction[Dagger];
$SkillRestriction[RShortSword] = $SkillRestriction[ShortSword];
$SkillRestriction[RShortBow] = $SkillRestriction[ShortBow];
$SkillRestriction[RLightCrossbow] = $SkillRestriction[LightCrossbow];

// Chat functions
$SkillRestriction["#say"] = $SkillSpeech @ " 0";
$SkillRestriction["#shout"] = $SkillSpeech @ " 3";
$SkillRestriction["#whisper"] = $SkillSpeech @ " 0";
$SkillRestriction["#tell"] = $SkillSpeech @ " 0";
$SkillRestriction["#global"] = $SkillSpeech @ " 5";
$SkillRestriction["#zone"] = $SkillSpeech @ " 0";
$SkillRestriction["#group"] = $SkillSpeech @ " 6";
$SkillRestriction["#party"] = $SkillSpeech @ " 0";
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
$SkillRestriction[thorn] = $SkillOffensiveCasting @ " 15";
$SkillRestriction[fireball] = $SkillOffensiveCasting @ " 20";
$SkillRestriction[firebomb] = $SkillOffensiveCasting @ " 35";
$SkillRestriction[icespike] = $SkillOffensiveCasting @ " 45";
$SkillRestriction[icestorm] = $SkillOffensiveCasting @ " 85";
$SkillRestriction[ironfist] = $SkillOffensiveCasting @ " 110";
$SkillRestriction[cloud] = $SkillOffensiveCasting @ " 145";
$SkillRestriction[melt] = $SkillOffensiveCasting @ " 220";
$SkillRestriction[powercloud] = $SkillOffensiveCasting @ " 340";
$SkillRestriction[hellstorm] = $SkillOffensiveCasting @ " 420";
$SkillRestriction[beam] = $SkillOffensiveCasting @ " 520";
$SkillRestriction[dimensionrift] = $SkillOffensiveCasting @ " 750";

$SkillRestriction[teleport] = $SkillNeutralCasting @ " 60";
$SkillRestriction[transport] = $SkillNeutralCasting @ " 200";
$SkillRestriction[advtransport] = $SkillNeutralCasting @ " 350";
$SkillRestriction[remort] = $SkillNeutralCasting @ " 0 " @ $MinLevel @ " 101";
$SkillRestriction[mimic] = $SkillNeutralCasting @ " 145 " @ $MinRemort @ " 2";
$SkillRestriction[masstransport] = $SkillNeutralCasting @ " 650 " @ $MinRemort @ " 1";

$SkillRestriction[heal] = $SkillDefensiveCasting @ " 10";
$SkillRestriction[advheal1] = $SkillDefensiveCasting @ " 80";
$SkillRestriction[advheal2] = $SkillDefensiveCasting @ " 110";
$SkillRestriction[advheal3] = $SkillDefensiveCasting @ " 200";
$SkillRestriction[advheal4] = $SkillDefensiveCasting @ " 320";
$SkillRestriction[advheal5] = $SkillDefensiveCasting @ " 400";
$SkillRestriction[advheal6] = $SkillDefensiveCasting @ " 500";
$SkillRestriction[godlyheal] = $SkillDefensiveCasting @ " 600";
$SkillRestriction[fullheal] = $SkillDefensiveCasting @ " 750";
$SkillRestriction[massheal] = $SkillDefensiveCasting @ " 850 " @ $MinRemort @ " 2";
$SkillRestriction[massfullheal] = $SkillDefensiveCasting @ " 950 " @ $MinRemort @ " 3";
$SkillRestriction[shield] = $SkillDefensiveCasting @ " 20";
$SkillRestriction[advshield1] = $SkillDefensiveCasting @ " 60";
$SkillRestriction[advshield2] = $SkillDefensiveCasting @ " 140";
$SkillRestriction[advshield3] = $SkillDefensiveCasting @ " 290";
$SkillRestriction[advshield4] = $SkillDefensiveCasting @ " 420";
$SkillRestriction[advshield5] = $SkillDefensiveCasting @ " 635";
$SkillRestriction[massshield] = $SkillDefensiveCasting @ " 680";

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

	////////temporary/////////////////////////
	if(%skill == 16)	//weapon handling
		return False;
	//////////////////////////////////////////

	//==== CAPS ================
	//if($PlayerSkill[%clientId, %skill] >= $SkillCap)
	//	return False;

	%ub = ($skillRangePerLevel * fetchData(%clientId, "LVL")) + 20 + fetchData(%clientId, "RemortStep");
	if($PlayerSkill[%clientId, %skill] >= %ub)
		return False;
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