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



//Data for spells are defined in the SPELL DEFINITIONS section.  Unfortunately, not everything can be designed there
//(ie, special effects etc) so for the great majority, the spells are coded in the DoCastSpell function.  This method
//isn't the ideal one... maybe one day i'll fix that if the need comes along.

//-- SPELL TYPES (not in use due to hardcoding of spells...) -----------------------------------------------------

$SelfType = 1;				//casts only to self
$LOSType = 2;				//casts only at LOS

$SelfRadiusType = 3;			//casts to self and around self
$LOSRadiusType = 4;			//casts at LOS and around LOS

$SelfRadiusLOSType = 5;			//casts to self and around self and at LOS
$LOSRadiusSelfType = 6;			//casts at LOS and around LOS and to self

$SelfRadiusLOSRadiusType = 7;		//casts to self and around self and to LOS and around LOS

//-- SPELL DEFINITIONS -------------------------------------------------------------------------------------------

$Spell::keyword[1] = "firebomb";
$Spell::index[firebomb] = 1;
$Spell::name[1] = "Fire Bomb From Hell";
$Spell::description[1] = "Casts an explosive.";
$Spell::delay[1] = 1.5;
$Spell::recoveryTime[1] = 2.625;
$Spell::radius[1] = 10;
$Spell::damageValue[1] = "55";
$Spell::LOSrange[1] = 80;
$Spell::manaCost[1] = 5;
$Spell::startSound[1] = ActivateBF;
$Spell::endSound[1] = ExplodeLM;
$Spell::groupListCheck[1] = False;
$Spell::refVal[1] = 55;
$Spell::graceDistance[1] = 2;
$SkillType[firebomb] = $SkillOffensiveCasting;

$Spell::keyword[2] = "teleport";
$Spell::index[teleport] = 2;
$Spell::name[2] = "Teleport close to nearest zone";
$Spell::description[2] = "Teleports you near a zone";
$Spell::delay[2] = 3.5;
$Spell::recoveryTime[2] = 16.5;
$Spell::manaCost[2] = 8;
$Spell::startSound[2] = Portal11;
$Spell::endSound[2] = ActivateCH;
$Spell::groupListCheck[2] = False;
$Spell::refVal[2] = 0;
$Spell::graceDistance[2] = 2;
$SkillType[teleport] = $SkillNeutralCasting;
$spell::menu[2] = True;

$Spell::keyword[3] = "transport";
$Spell::index[transport] = 3;
$Spell::name[3] = "Transport to zone";
$Spell::description[3] = "Transports to a specific zone";
$Spell::delay[3] = 4.0;
$Spell::recoveryTime[3] = 23;
$Spell::manaCost[3] = 12;
$Spell::startSound[3] = RespawnB;
$Spell::endSound[3] = ActivateCH;
$Spell::groupListCheck[3] = False;
$Spell::refVal[3] = 0;
$Spell::graceDistance[3] = 2;
$SkillType[transport] = $SkillNeutralCasting;

$Spell::keyword[4] = "advtransport";
$Spell::index[advtransport] = 4;
$Spell::name[4] = "Advanced Transport to zone";
$Spell::description[4] = "Transports self OR person in line-of-sight to a specific zone";
$Spell::delay[4] = 4.0;
$Spell::recoveryTime[4] = 27;
$Spell::LOSrange[4] = 500;
$Spell::manaCost[4] = 16;
$Spell::startSound[4] = RespawnB;
$Spell::endSound[4] = ActivateCH;
$Spell::groupListCheck[4] = True;
$Spell::refVal[4] = 0;
$Spell::graceDistance[4] = 2;
$SkillType[advtransport] = $SkillNeutralCasting;

$Spell::keyword[5] = "cloud";
$Spell::index[cloud] = 5;
$Spell::name[5] = "Cloud Attack";
$Spell::description[5] = "Casts an explosive.";
$Spell::delay[5] = 1.5;
$Spell::recoveryTime[5] = 2.625;
$Spell::radius[5] = 10;
$Spell::damageValue[5] = "85";
$Spell::LOSrange[5] = 80;
$Spell::manaCost[5] = 10;
$Spell::startSound[5] = ActivateBF;
$Spell::endSound[5] = ExplodeLM;
$Spell::groupListCheck[5] = False;
$Spell::refVal[5] = 85;
$Spell::graceDistance[5] = 2;
$SkillType[cloud] = $SkillOffensiveCasting;

$Spell::keyword[6] = "melt";
$Spell::index[melt] = 6;
$Spell::name[6] = "Melt Bomb Attack";
$Spell::description[6] = "Casts an explosive.";
$Spell::delay[6] = 1.5;
$Spell::recoveryTime[6] = 2.625;
$Spell::radius[6] = 10;
$Spell::damageValue[6] = "140";
$Spell::LOSrange[6] = 80;
$Spell::manaCost[6] = 15;
$Spell::startSound[6] = ActivateBF;
$Spell::endSound[6] = ExplodeLM;
$Spell::groupListCheck[6] = False;
$Spell::refVal[6] = 140;
$Spell::graceDistance[6] = 2;
$SkillType[melt] = $SkillOffensiveCasting;

$Spell::keyword[7] = "powercloud";
$Spell::index[powercloud] = 7;
$Spell::name[7] = "Power Cloud Attack";
$Spell::description[7] = "Casts three explosives.";
$Spell::delay[7] = 1.5;
$Spell::recoveryTime[7] = 7.5;
$Spell::radius[7] = 10;
$Spell::damageValue[7] = "70";
$Spell::LOSrange[7] = 80;
$Spell::manaCost[7] = 20;
$Spell::startSound[7] = ActivateBF;
$Spell::endSound[7] = ExplodeLM;
$Spell::groupListCheck[7] = False;
$Spell::refVal[7] = 210;
$Spell::graceDistance[7] = 2;
$SkillType[powercloud] = $SkillOffensiveCasting;

$Spell::keyword[8] = "heal";
$Spell::index[heal] = 8;
$Spell::name[8] = "Heal Self";
$Spell::description[8] = "Heals the caster.";
$Spell::delay[8] = 1.5;
$Spell::recoveryTime[8] = 2.25;
$Spell::damageValue[8] = -6;
$Spell::manaCost[8] = 2;
$Spell::startSound[8] = DeActivateWA;
$Spell::endSound[8] = ActivateAR;
$Spell::groupListCheck[8] = False;
$Spell::refVal[8] = -6;
$Spell::graceDistance[8] = 2;
$SkillType[heal] = $SkillDefensiveCasting;

$Spell::keyword[9] = "advheal1";
$Spell::index[advheal1] = 9;
$Spell::name[9] = "Heal Self or Other (1st)";
$Spell::description[9] = "Heals the caster or someone in the LOS.";
$Spell::delay[9] = 1.5;
$Spell::recoveryTime[9] = 3.25;
$Spell::damageValue[9] = -10;
$Spell::LOSrange[9] = 80;
$Spell::manaCost[9] = 3;
$Spell::startSound[9] = DeActivateWA;
$Spell::endSound[9] = ActivateAR;
$Spell::groupListCheck[9] = False;
$Spell::refVal[9] = -10;
$Spell::graceDistance[9] = 2;
$SkillType[advheal1] = $SkillDefensiveCasting;

$Spell::keyword[10] = "advheal2";
$Spell::index[advheal2] = 10;
$Spell::name[10] = "Heal Self Or Other (2nd)";
$Spell::description[10] = "Heals the caster or someone in the LOS.";
$Spell::delay[10] = 1.5;
$Spell::recoveryTime[10] = 4.0;
$Spell::damageValue[10] = -15;
$Spell::LOSrange[10] = 80;
$Spell::manaCost[10] = 4;
$Spell::startSound[10] = DeActivateWA;
$Spell::endSound[10] = ActivateAR;
$Spell::groupListCheck[10] = False;
$Spell::refVal[10] = -15;
$Spell::graceDistance[10] = 2;
$SkillType[advheal2] = $SkillDefensiveCasting;

$Spell::keyword[11] = "godlyheal";
$Spell::index[godlyheal] = 11;
$Spell::name[11] = "Godly Heal Self Or Other";
$Spell::description[11] = "Heals the caster or someone in the LOS.";
$Spell::delay[11] = 1.5;
$Spell::recoveryTime[11] = 6;
$Spell::damageValue[11] = -80;
$Spell::LOSrange[11] = 80;
$Spell::manaCost[11] = 15;
$Spell::startSound[11] = DeActivateWA;
$Spell::endSound[11] = ActivateAR;
$Spell::groupListCheck[11] = False;
$Spell::refVal[11] = -80;
$Spell::graceDistance[11] = 2;
$SkillType[godlyheal] = $SkillDefensiveCasting;

$Spell::keyword[12] = "beam";
$Spell::index[beam] = 12;
$Spell::name[12] = "Beam";
$Spell::description[12] = "Light gathers into a concentrated beam and causes intense damage to the target.";
$Spell::delay[12] = 0.0;
$Spell::recoveryTime[12] = 15;
$Spell::damageValue[12] = "180";
$Spell::LOSrange[12] = 1000;
$Spell::manaCost[12] = 30;
$Spell::startSound[12] = HitLevelDT;
$Spell::endSound[12] = HitBF;
$Spell::groupListCheck[12] = False;
$Spell::refVal[12] = 180;
$Spell::graceDistance[12] = 5;
$SkillType[beam] = $SkillOffensiveCasting;

$Spell::keyword[13] = "thorn";
$Spell::index[thorn] = 13;
$Spell::name[13] = "Thorn";
$Spell::description[13] = "Casts thorn.";
$Spell::delay[13] = 0.1;
$Spell::recoveryTime[13] = 1.5;
$Spell::radius[13] = 6;
$Spell::damageValue[13] = "20";
$Spell::LOSrange[13] = 80;
$Spell::manaCost[13] = 1;
$Spell::startSound[13] = ActivateFK;
$Spell::endSound[13] = DeflectAS;
$Spell::groupListCheck[13] = False;
$Spell::refVal[13] = 20;
$Spell::graceDistance[13] = 5;
$SkillType[thorn] = $SkillOffensiveCasting;

$Spell::keyword[14] = "fireball";
$Spell::index[fireball] = 14;
$Spell::name[14] = "Fireball";
$Spell::description[14] = "Casts a fireball.";
$Spell::delay[14] = 1;
$Spell::recoveryTime[14] = 1.5;
$Spell::radius[14] = 8;
$Spell::damageValue[14] = "35";
$Spell::LOSrange[14] = 80;
$Spell::manaCost[14] = 3;
$Spell::startSound[14] = ActivateAB;
$Spell::endSound[14] = LaunchFB;
$Spell::groupListCheck[14] = False;
$Spell::refVal[14] = 35;
$Spell::graceDistance[14] = 2;
$SkillType[fireball] = $SkillOffensiveCasting;

$Spell::keyword[15] = "icespike";
$Spell::index[icespike] = 15;
$Spell::name[15] = "Icespike";
$Spell::description[15] = "Casts icespike.";
$Spell::delay[15] = 0.1;
$Spell::recoveryTime[15] = 1.5;
$Spell::radius[15] = 6;
$Spell::damageValue[15] = "28";
$Spell::LOSrange[15] = 80;
$Spell::manaCost[15] = 3;
$Spell::startSound[15] = ActivateFK;
$Spell::endSound[15] = HitPawnDT;
$Spell::groupListCheck[15] = False;
$Spell::refVal[15] = 28;
$Spell::graceDistance[15] = 5;
$SkillType[icespike] = $SkillOffensiveCasting;

$Spell::keyword[16] = "icestorm";
$Spell::index[icestorm] = 16;
$Spell::name[16] = "Icestorm";
$Spell::description[16] = "Casts icestorm.";
$Spell::delay[16] = 1;
$Spell::recoveryTime[16] = 2.25;
$Spell::radius[16] = 11;
$Spell::damageValue[16] = "45";
$Spell::LOSrange[16] = 80;
$Spell::manaCost[16] = 4;
$Spell::startSound[16] = ImpactTR;
$Spell::endSound[16] = Reflected;
$Spell::groupListCheck[16] = False;
$Spell::refVal[16] = 45;
$Spell::graceDistance[16] = 2;
$SkillType[icestorm] = $SkillOffensiveCasting;

$Spell::keyword[17] = "ironfist";
$Spell::index[ironfist] = 17;
$Spell::name[17] = "Ironfist";
$Spell::description[17] = "Casts ironfist.";
$Spell::delay[17] = 0.1;
$Spell::recoveryTime[17] = 13.5;
$Spell::radius[17] = 7;
$Spell::damageValue[17] = "128";
$Spell::LOSrange[17] = 80;
$Spell::manaCost[17] = 12;
$Spell::startSound[17] = UnravelAM;
$Spell::endSound[17] = NoSound;
$Spell::groupListCheck[17] = False;
$Spell::refVal[17] = 128;
$Spell::graceDistance[17] = 3;
$SkillType[ironfist] = $SkillOffensiveCasting;

$Spell::keyword[18] = "hellstorm";
$Spell::index[hellstorm] = 18;
$Spell::name[18] = "Hellstorm";
$Spell::description[18] = "Casts hellstorm.";
$Spell::delay[18] = 6;
$Spell::recoveryTime[18] = 10.5;
$Spell::radius[18] = 20;
$Spell::damageValue[18] = "265";
$Spell::LOSrange[18] = 80;
$Spell::manaCost[18] = 20;
$Spell::startSound[18] = LoopLS;
$Spell::endSound[18] = LaunchET;
$Spell::groupListCheck[18] = False;
$Spell::refVal[18] = 265;
$Spell::graceDistance[18] = 2;
$SkillType[hellstorm] = $SkillOffensiveCasting;

$Spell::keyword[19] = "dimensionrift";
$Spell::index[dimensionrift] = 19;
$Spell::name[19] = "Dimension Rift";
$Spell::description[19] = "Casts Dimension Rift.";
$Spell::delay[19] = 9.5;
$Spell::recoveryTime[19] = 11.25;
$Spell::radius[19] = 30;
$Spell::damageValue[19] = "320";
$Spell::LOSrange[19] = 80;
$Spell::manaCost[19] = 40;
$Spell::startSound[19] = LaunchLS;
$Spell::endSound[19] = Explode3FW;
$Spell::groupListCheck[19] = False;
$Spell::refVal[19] = 320;
$Spell::graceDistance[19] = 2;
$SkillType[dimensionrift] = $SkillOffensiveCasting;

$Spell::keyword[20] = "advheal3";
$Spell::index[advheal3] = 20;
$Spell::name[20] = "Heal Self Or Other (3rd)";
$Spell::description[20] = "Heals the caster or someone in the LOS.";
$Spell::delay[20] = 1.5;
$Spell::recoveryTime[20] = 4.75;
$Spell::damageValue[20] = -25;
$Spell::LOSrange[20] = 80;
$Spell::manaCost[20] = 5;
$Spell::startSound[20] = DeActivateWA;
$Spell::endSound[20] = ActivateAR;
$Spell::groupListCheck[20] = False;
$Spell::refVal[20] = -25;
$Spell::graceDistance[20] = 2;
$SkillType[advheal3] = $SkillDefensiveCasting;

$Spell::keyword[21] = "remort";
$Spell::index[remort] = 21;
$Spell::name[21] = "Remort";
$Spell::description[21] = "Remorts a level 101 character to level 1, with bonuses.";
$Spell::delay[21] = 3.0;
$Spell::recoveryTime[21] = 1;
$Spell::damageValue[21] = 0;
$Spell::manaCost[21] = 1;
$Spell::startSound[21] = RespawnA;
$Spell::endSound[21] = RespawnC;
$Spell::groupListCheck[21] = False;
$Spell::refVal[21] = 0;
$Spell::graceDistance[21] = 2;
$SkillType[remort] = $SkillNeutralCasting;

$Spell::keyword[22] = "fullheal";
$Spell::index[fullheal] = 22;
$Spell::name[22] = "Full Heal Self";
$Spell::description[22] = "Fully heals the caster.";
$Spell::delay[22] = 1.5;
$Spell::recoveryTime[22] = 60;
$Spell::damageValue[22] = 0;
$Spell::manaCost[22] = 2;
$Spell::startSound[22] = DeActivateWA;
$Spell::endSound[22] = PlaceSeal;
$Spell::groupListCheck[22] = False;
$Spell::refVal[22] = -9998;
$Spell::graceDistance[22] = 2;
$SkillType[fullheal] = $SkillDefensiveCasting;

$Spell::keyword[23] = "massheal";
$Spell::index[massheal] = 23;
$Spell::name[23] = "Mass Heal";
$Spell::description[23] = "Heals caster and friendlies 10 meters around.";
$Spell::delay[23] = 1.5;
$Spell::recoveryTime[23] = 10;
$Spell::radius[23] = 10;
$Spell::damageValue[23] = -30;
$Spell::manaCost[23] = 12;
$Spell::startSound[23] = DeActivateWA;
$Spell::endSound[23] = ActivateAR;
$Spell::groupListCheck[23] = False;
$Spell::refVal[23] = -30;
$Spell::graceDistance[23] = 2;
$SkillType[massheal] = $SkillDefensiveCasting;

$Spell::keyword[24] = "massfullheal";
$Spell::index[massfullheal] = 24;
$Spell::name[24] = "Mass Full Heal";
$Spell::description[24] = "Fully Heals caster and friendlies 12 meters around.";
$Spell::delay[24] = 1.5;
$Spell::recoveryTime[24] = 300;
$Spell::radius[24] = 12;
$Spell::damageValue[24] = 0;
$Spell::manaCost[24] = 200;
$Spell::startSound[24] = DeActivateWA;
$Spell::endSound[24] = PlaceSeal;
$Spell::groupListCheck[24] = False;
$Spell::refVal[24] = -9999;
$Spell::graceDistance[24] = 2;
$SkillType[massfullheal] = $SkillDefensiveCasting;

$Spell::keyword[25] = "shield";
$Spell::index[shield] = 25;
$Spell::name[25] = "Shield Self";
$Spell::description[25] = "A magical shield adds 50 DEF to the caster.";
$Spell::delay[25] = 2.0;
$Spell::recoveryTime[25] = 8;
$Spell::damageValue[25] = "DEF 50";
$Spell::ticks[25] = 150;	//5 minutes
$Spell::manaCost[25] = 5;
$Spell::startSound[25] = ActivateTR;
$Spell::endSound[25] = ActivateTD;
$Spell::groupListCheck[25] = False;
$Spell::refVal[25] = -10;
$Spell::graceDistance[25] = 2;
$SkillType[shield] = $SkillDefensiveCasting;

$Spell::keyword[26] = "advshield1";
$Spell::index[advshield1] = 26;
$Spell::name[26] = "Shield Self Or Other (1st)";
$Spell::description[26] = "A magical shield that adds 80 DEF to the caster or target in LOS.";
$Spell::delay[26] = 2.0;
$Spell::recoveryTime[26] = 10;
$Spell::damageValue[26] = "DEF 80";
$Spell::ticks[26] = 165;	//5:30 minutes
$Spell::LOSrange[26] = 80;
$Spell::manaCost[26] = 8;
$Spell::startSound[26] = ActivateTR;
$Spell::endSound[26] = ActivateTD;
$Spell::groupListCheck[26] = False;
$Spell::refVal[26] = -11;
$Spell::graceDistance[26] = 2;
$SkillType[advshield1] = $SkillDefensiveCasting;

$Spell::keyword[27] = "advshield2";
$Spell::index[advshield2] = 27;
$Spell::name[27] = "Shield Self Or Other (2nd)";
$Spell::description[27] = "A magical shield that adds 70 DEF and 50 MDEF to the caster or target in LOS.";
$Spell::delay[27] = 2.0;
$Spell::recoveryTime[27] = 12;
$Spell::damageValue[27] = "DEF 70 MDEF 50";
$Spell::ticks[27] = 190;	//6:20 minutes
$Spell::LOSrange[27] = 80;
$Spell::manaCost[27] = 15;
$Spell::startSound[27] = ActivateTR;
$Spell::endSound[27] = ActivateTD;
$Spell::groupListCheck[27] = False;
$Spell::refVal[27] = -12;
$Spell::graceDistance[27] = 2;
$SkillType[advshield2] = $SkillDefensiveCasting;

$Spell::keyword[28] = "advshield3";
$Spell::index[advshield3] = 28;
$Spell::name[28] = "Shield Self Or Other (3rd)";
$Spell::description[28] = "A magical shield that adds 120 DEF and 80 MDEF to the caster or target in LOS.";
$Spell::delay[28] = 2.0;
$Spell::recoveryTime[28] = 14;
$Spell::damageValue[28] = "DEF 120 MDEF 80";
$Spell::ticks[28] = 218;	//7:16 minutes
$Spell::LOSrange[28] = 80;
$Spell::manaCost[28] = 18;
$Spell::startSound[28] = ActivateTR;
$Spell::endSound[28] = ActivateTD;
$Spell::groupListCheck[28] = False;
$Spell::refVal[28] = -13;
$Spell::graceDistance[28] = 2;
$SkillType[advshield3] = $SkillDefensiveCasting;

$Spell::keyword[29] = "advshield4";
$Spell::index[advshield4] = 29;
$Spell::name[29] = "Shield Self Or Other (4th)";
$Spell::description[29] = "A magical shield that adds 170 MDEF to the caster or target in LOS.";
$Spell::delay[29] = 2.0;
$Spell::recoveryTime[29] = 16;
$Spell::damageValue[29] = "MDEF 170";
$Spell::ticks[29] = 255;	//8:30 minutes
$Spell::LOSrange[29] = 80;
$Spell::manaCost[29] = 22;
$Spell::startSound[29] = ActivateTR;
$Spell::endSound[29] = ActivateTD;
$Spell::groupListCheck[29] = False;
$Spell::refVal[29] = -14;
$Spell::graceDistance[29] = 2;
$SkillType[advshield4] = $SkillDefensiveCasting;

$Spell::keyword[30] = "advshield5";
$Spell::index[advshield5] = 30;
$Spell::name[30] = "Shield Self Or Other (5th)";
$Spell::description[30] = "A magical shield that adds 150 DEF and 210 MDEF to the caster or target in LOS.";
$Spell::delay[30] = 2.0;
$Spell::recoveryTime[30] = 20;
$Spell::damageValue[30] = "DEF 150 MDEF 210";
$Spell::ticks[30] = 300;	//10 minutes
$Spell::LOSrange[30] = 80;
$Spell::manaCost[30] = 25;
$Spell::startSound[30] = ActivateTR;
$Spell::endSound[30] = ActivateTD;
$Spell::groupListCheck[30] = False;
$Spell::refVal[30] = -15;
$Spell::graceDistance[30] = 2;
$SkillType[advshield5] = $SkillDefensiveCasting;

$Spell::keyword[31] = "massshield";
$Spell::index[massshield] = 31;
$Spell::name[31] = "Mass Shield";
$Spell::description[31] = "A magical shield that adds 115 DEF and 105 MDEF to all friendlies within a 10 meter radius.";
$Spell::delay[31] = 2.0;
$Spell::recoveryTime[31] = 30;
$Spell::radius[31] = 10;
$Spell::damageValue[31] = "DEF 115 MDEF 105";
$Spell::ticks[31] = 270;	//9 minutes
$Spell::manaCost[31] = 20;
$Spell::startSound[31] = ActivateTR;
$Spell::endSound[31] = ActivateTD;
$Spell::groupListCheck[31] = False;
$Spell::refVal[31] = -16;
$Spell::graceDistance[31] = 2;
$SkillType[massshield] = $SkillDefensiveCasting;

$Spell::keyword[32] = "mimic";
$Spell::index[mimic] = 32;
$Spell::name[32] = "Mimic";
$Spell::description[32] = "A very dangerous spell that transforms the caster into the creature in his/her LOS.";
$Spell::delay[32] = 4.0;
$Spell::recoveryTime[32] = 60;
$Spell::LOSrange[32] = 80;
$Spell::damageValue[32] = 0;
$Spell::manaCost[32] = 80;
$Spell::startSound[32] = LoopSP;
$Spell::endSound[32] = AbsorbABS;
$Spell::groupListCheck[32] = False;
$Spell::refVal[32] = 1;
$Spell::graceDistance[32] = 2;
$SkillType[mimic] = $SkillNeutralCasting;

$Spell::keyword[33] = "masstransport";
$Spell::index[masstransport] = 33;
$Spell::name[33] = "Mass Transport";
$Spell::description[33] = "Transports self and all friendlies within a 6 meter radius to a specific zone.";
$Spell::delay[33] = 4.0;
$Spell::recoveryTime[33] = 45;
$Spell::radius[33] = 6;
$Spell::manaCost[33] = 50;
$Spell::startSound[33] = RespawnB;
$Spell::endSound[33] = ActivateCH;
$Spell::groupListCheck[33] = False;
$Spell::refVal[33] = 0;
$Spell::graceDistance[33] = 2;
$SkillType[masstransport] = $SkillNeutralCasting;

$Spell::keyword[34] = "advheal4";
$Spell::index[advheal4] = 34;
$Spell::name[34] = "Heal Self Or Other (4th)";
$Spell::description[34] = "Heals the caster or someone in the LOS.";
$Spell::delay[34] = 1.5;
$Spell::recoveryTime[34] = 5.0;
$Spell::damageValue[34] = -35;
$Spell::LOSrange[34] = 80;
$Spell::manaCost[34] = 6;
$Spell::startSound[34] = DeActivateWA;
$Spell::endSound[34] = ActivateAR;
$Spell::groupListCheck[34] = False;
$Spell::refVal[34] = -35;
$Spell::graceDistance[34] = 2;
$SkillType[advheal4] = $SkillDefensiveCasting;

$Spell::keyword[35] = "advheal5";
$Spell::index[advheal5] = 35;
$Spell::name[35] = "Heal Self Or Other (5th)";
$Spell::description[35] = "Heals the caster or someone in the LOS.";
$Spell::delay[35] = 1.5;
$Spell::recoveryTime[35] = 5.5;
$Spell::damageValue[35] = -50;
$Spell::LOSrange[35] = 80;
$Spell::manaCost[35] = 7;
$Spell::startSound[35] = DeActivateWA;
$Spell::endSound[35] = ActivateAR;
$Spell::groupListCheck[35] = False;
$Spell::refVal[35] = -50;
$Spell::graceDistance[35] = 2;
$SkillType[advheal5] = $SkillDefensiveCasting;

$Spell::keyword[36] = "advheal6";
$Spell::index[advheal6] = 36;
$Spell::name[36] = "Heal Self Or Other (6th)";
$Spell::description[36] = "Heals the caster or someone in the LOS.";
$Spell::delay[36] = 1.5;
$Spell::recoveryTime[36] = 6.0;
$Spell::damageValue[36] = -60;
$Spell::LOSrange[36] = 80;
$Spell::manaCost[36] = 8;
$Spell::startSound[36] = DeActivateWA;
$Spell::endSound[36] = ActivateAR;
$Spell::groupListCheck[36] = False;
$Spell::refVal[36] = -60;
$Spell::graceDistance[36] = 2;
$SkillType[advheal6] = $SkillDefensiveCasting;

//----------------------------------------------------------------------------------------------------------------

function BeginCastSpell(%clientId, %keyword)
{
	dbecho($dbechoMode, "BeginCastSpell(" @ %clientId @ ", " @ %keyword @ ")");

	%w1 = GetWord(%keyword, 0);
	%w2 = String::getSubStr(%keyword, String::len(%w1)+1, 99999);

	for(%i = 1; $Spell::keyword[%i] != ""; %i++)
	{
		if(String::ICompare($Spell::keyword[%i], %w1) == 0)
		{
			if(SkillCanUse(%clientId, $Spell::keyword[%i]))
			{
				if(fetchData(%clientId, "MANA") >= $Spell::manaCost[%i])
				{
					Client::sendMessage(%clientId, $MsgBeige, "Casting " @ $Spell::name[%i] @ ".");

					%player = Client::getOwnedObject(%clientId);
					if(GameBase::getLOSinfo(%player, $Spell::LOSrange[%i]))
					{
						//%lospos = $los::position;
						%losobj = $los::object;
					}
					else
					{
						//%lospos = "";
						%losobj = 0;
					}
	
					storeData(%clientId, "SpellCastStep", 1);
	
					%tempManaCost = floor($Spell::manaCost[%i] / 2);
					refreshMANA(%clientId, %tempManaCost);
					if($Spell::startSound[%i] != "")
						playSound($Spell::startSound[%i], GameBase::getPosition(%clientId));

					%skt = $SkillType[$Spell::keyword[%i]];
					%sk1 = $PlayerSkill[%clientId, %skt];
					%gsa = GetSkillAmount($Spell::keyword[%i], %skt);
					%sk2 = %sk1 - %gsa;
					%sk = Cap(%sk2, 0, "inf");
					%rt = $Spell::recoveryTime[%i];
					%a = %rt / 2;
					%b = (1000 - %sk) / 1000;
					%c = %b * %a;
					//recovery time is never smaller than half of the original and never bigger than the original.
					%recovTime = Cap(%a + %c, %a, %rt);
					storeData(%clientId, "SpellRecovTime", %recovTime);
					if($spell::menu[%i]){						eval("casting::"@$spell::keyword[%i]@"(" @ %clientId @ ", " @ %i @ ", \"" @ GameBase::getPosition(%clientId) @ "\", \"" @ %losobj @ "\", \"" @ %w2 @ "\");");					}					else if($Spell::delay[%i] > 0){						if($Spell::Indicator[%i]){							spellIndicatorLoop(%clientId, $Spell::LOSrange[%i]);						}						Schedule::Add("DoCastSpell(" @ %clientId @ ", " @ %i @ ", \"" @ GameBase::getPosition(%clientId) @ "\", \"" @ %losobj @ "\", \"" @ %w2 @ "\");", $Spell::delay[%i], "spell"@%clientId);					}					else						DoCastSpell(%clientId, %i, GameBase::getPosition(%clientId), %losobj, %w2);
					//schedule("DoCastSpell(" @ %clientId @ ", " @ %i @ ", \"" @ GameBase::getPosition(%clientId) @ "\", \"" @ %lospos @ "\", \"" @ %losobj @ "\", \"" @ %w2 @ "\");", $Spell::delay[%i]);
					//schedule("%retval=DoCastSpell(" @ %clientId @ ", " @ %i @ ", \"" @ GameBase::getPosition(%clientId) @ "\", \"" @ %lospos @ "\", \"" @ %losobj @ "\", \"" @ %w2 @ "\"); if(%retval){refreshMANA(" @ %clientId @ ", " @ %tempManaCost @ ");}", $Spell::delay[%i]);
		
					return True;
				}
				else
					Client::sendMessage(%clientId, $MsgWhite, "Insufficient mana to cast this spell.");
			}
			else
				Client::sendMessage(%clientId, $MsgWhite, "You can't cast this spell because you lack the necessary skills.");

			return False;
		}
	}
	Client::sendMessage(%clientId, $MsgWhite, "This spell seems unfamiliar to you.");

	return False;
}

//function DoCastSpell(%clientId, %index, %oldpos, %castPos, %castObj, %w2)
function DoCastSpell(%clientId, %index, %oldpos, %castObj, %w2)
{
	dbecho($dbechoMode, "DoCastSpell(" @ %clientId @ ", " @ %index @ ", " @ %oldpos @ ", " @ %castPos @ ", " @ %castObj @ ", " @ %w2 @ ")");

	%player = Client::getOwnedObject(%clientId);
	if($Spell::graceDistance[%index] == "")		$Spell::graceDistance[%index] = 0.25;	$los::position = "";	if(GameBase::getLOSinfo(%player, $Spell::LOSrange[%index])){		%castPos = $los::position;		if(%castObj < 1)			%castObj = $los::object;	}
	if(Vector::getDistance(%oldpos, GameBase::getPosition(%clientId)) > $Spell::graceDistance[%index])
	{
		Client::sendMessage(%clientId, $MsgBeige, "Your casting was interrupted.");
		//storeData(%clientId, "SpellCastStep", 2);

		%returnflag = False;
		return EndCast(%clientid,%overrideEndSound,%extradelay,%index,%castpos,%returnflag);
		//return False;
	}

	//group-list check
	if($Spell::groupListCheck[%index])
	{
		%cl = Player::getClient(%castObj);
		if( !(IsInCommaList(fetchData(%clientId, "grouplist"), Client::getName(%cl)) && IsInCommaList(fetchData(%cl, "grouplist"), Client::getName(%clientId))) && %cl != %clientId && %cl != -1)
		{
			Client::sendMessage(%clientId, $MsgBeige, "You are not part of the target's group.");
			//storeData(%clientId, "SpellCastStep", 2);

			%returnflag = False;
			return EndCast(%clientid,%overrideEndSound,%extradelay,%index,%castpos,%returnflag);
			//return False;
		}
	}

	//==================================================================

	//unfortunately hard-coded part -- although that is the original purpose of Tribes scripting
	if(%index == 1)
	{
		//firebomb spell, casts to LOS with radius damage

		if(%castPos != "")
		{
			CreateAndDetBomb(%clientId, "Bomb1", %castPos, True, %index);

			%overrideEndSound = True;
			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Could not find a target.");

			%returnFlag = False;
		}
	}

	if(%index == 2)
	{
		//teleport zone spell

		%zoneId = GetNearestZone(%clientId, %w2, 3);

		if(%zoneId != False)
		{
			Client::sendMessage(%clientId, $MsgBeige, "Teleporting near " @ Zone::getDesc(%zoneId));

			//teleport
			%originPos = gamebase::getposition(%clientId);

			if(!fetchData(%clientId, "invisible"))
				GameBase::startFadeIn(%clientId);
			CheckAndBootFromArena(%clientId);
			Player::setDamageFlash(%clientId, 0.7);

			//%mpos = Zone::getMarker(%zoneId);
			//GameBase::setPosition(%clientId, %mpos);
			//NullItemList(%clientId, Lore, $MsgRed, "You lost all %1s you were carrying when you teleported.");//never worked anyway

			%extraDelay = 0.22;	//sometimes the endSound doesn't get played unless there is sufficient delay

			//%castPos = SetOnGround(%clientId, 500);

			%returnFlag = True;			if($Zone::ForceTeleport[%zoneId]){				%castPos = TeleportToZone(%clientId, %desc, False, True);				if(%castPos == False){					Client::sendMessage(%id, $MsgBeige, "The zone's there, but you can't reach it.");					%returnFlag = False;					%castPos = %originPos;				}			}			else {				%mpos = Zone::getMarker(%zoneId);
				%zid = $Zone::zoneID[%zoneId];				%sizex = $Zone::Length[%zid];				%sizey = $Zone::Width[%zid];				%castPos = findGroundPos(%mpos, %sizex*0.6, %sizey*0.6);				if(%castPos != False)					GameBase::setPosition(%clientId, %castPos);				else{					Client::sendMessage(%id, $MsgBeige, "The zone's there, but you can't reach it.");					%returnFlag = False;					%castPos = %originPos;				}			}
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Teleportation failed.");
			%returnFlag = False;
		}
	}

	if(%index == 3)
	{
		//Transport zone spell

		%zoneId = GetZoneByKeywords(%clientId, %w2, 3);

		if(%zoneId != False)
		{
			Client::sendMessage(%clientId, $MsgBeige, "Transporting to " @ Zone::getDesc(%zoneId));

			//teleport

			%system = Object::getName(%zoneId);
			%type = GetWord(%system, 0);
			%desc = String::getSubStr(%system, String::len(%type)+1, 9999);

			%castPos = TeleportToMarker(%clientId, "Zones\\" @ %system @ "\\DropPoints", False, True);
			CheckAndBootFromArena(%clientId);
			NullItemList(%clientId, Lore, $MsgRed, "You lost all %1s you were carrying when you teleported.");

			if(!fetchData(%clientId, "invisible"))
				GameBase::startFadeIn(%clientId);

			Player::setDamageFlash(%clientId, 0.7);
			%extraDelay = 0.22;	//sometimes the endSound doesn't get played unless there is sufficient delay

			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Transportation failed.");
			%returnFlag = False;
		}
	}

	if(%index == 4)
	{
		//Advanced Transport zone spell

		%zoneId = GetZoneByKeywords(%clientId, %w2, 3);

		if(%zoneId != False)
		{
			if(getObjectType(%castObj) == "Player")
				%id = Player::getClient(%castObj);
			else
				%id = %clientId;

			Client::sendMessage(%clientId, $MsgBeige, "Transporting to " @ Zone::getDesc(%zoneId));
			if(%clientId != %id)
				Client::sendMessage(%id, $MsgBeige, "You are being transported to " @ Zone::getDesc(%zoneId));

			//teleport

			%system = Object::getName(%zoneId);
			%type = GetWord(%system, 0);
			%desc = String::getSubStr(%system, String::len(%type)+1, 9999);

			%castPos = TeleportToMarker(%id, "Zones\\" @ %system @ "\\DropPoints", False, True);
			CheckAndBootFromArena(%id);
			NullItemList(%clientId, Lore, $MsgRed, "You lost all %1s you were carrying when you teleported.");

			if(!fetchData(%id, "invisible"))
				GameBase::startFadeIn(%id);

			Player::setDamageFlash(%id, 0.7);
			%extraDelay = 0.22;	//sometimes the endSound doesn't get played unless there is sufficient delay

			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Transportation failed.");
			%returnFlag = False;
		}
	}
	if(%index == 5)
	{
		//cloud spell, casts to LOS with radius damage

		if(%castPos != "")
		{
			CreateAndDetBomb(%clientId, "Bomb2", %castPos, True, %index);

			%overrideEndSound = True;
			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Could not find a target.");

			%returnFlag = False;
		}
	}
	if(%index == 6)
	{
		//melt spell, casts to LOS with radius damage

		if(%castPos != "")
		{
			CreateAndDetBomb(%clientId, "Bomb3", %castPos, True, %index);

			%overrideEndSound = True;
			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Could not find a target.");

			%returnFlag = False;
		}
	}
	if(%index == 7)
	{
		//power cloud spell, casts to LOS with radius damage

		if(%castPos != "")
		{
			for(%i = 0; %i <= 2; %i++)
			{
				schedule("CreateAndDetBomb(" @ %clientId @ ", \"Bomb2\", \"" @ %castPos @ "\", True, " @ %index @ ");", %i / 2, %player);
			}

			%overrideEndSound = True;
			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Could not find a target.");

			%returnFlag = False;
		}
	}
	if(%index == 8)
	{
		//heal self spell

		Client::sendMessage(%clientId, $MsgBeige, "Healing self");

		%r = $Spell::damageValue[%index] / $TribesDamageToNumericDamage;
		refreshHP(%clientId, %r);

		%castPos = GameBase::getPosition(%clientId);

		%returnFlag = True;
	}
	if(%index == 9 || %index == 10 || %index == 11 || %index == 20 || %index == 34 || %index == 35 || %index == 36)
	{
		//heal self or other (LOS) 1st, 2nd, 3rd, 4th, 5th, 6th, godly

		if(getObjectType(%castObj) == "Player" && !Player::isAiControlled(%clientId))
			%id = Player::getClient(%castObj);
		else
			%id = %clientId;

		Client::sendMessage(%clientId, $MsgBeige, "Healing " @ Client::getName(%id));
		if(%clientId != %id)
			Client::sendMessage(%id, $MsgBeige, Client::getName(%clientId) @ " is casting " @ $Spell::name[%index] @ " on you.");

		%r = $Spell::damageValue[%index] / $TribesDamageToNumericDamage;

		refreshHP(%id, %r);

		%castPos = GameBase::getPosition(%id);

		%returnFlag = True;
	}
	if(%index == 12)
	{
		//laser beam

		if(getObjectType(%castObj) == "Player")
			%id = Player::getClient(%castObj);

		%trans = GameBase::getMuzzleTransform(%clientId);
		%p = Projectile::spawnProjectile("sniperLaser", %trans, %player, "0 0 0", 1.0);

		%mom1 = Vector::getFromRot( GameBase::getRotation(%clientId), -60, 1 );
		Player::applyImpulse(%clientId, %mom1);

		%r = $Spell::damageValue[%index];
	
		if(%id != "")
		{
			//%miss = CalcSpellMiss(%clientId, %id, %index);

			SpellDamage(%clientId, %id, %r, %index);
			%mom2 = Vector::getFromRot( GameBase::getRotation(%clientId), 50, 1 );
			Player::applyImpulse(%id, %mom2);
		}

		%castPos = GameBase::getPosition(%clientId);

		%returnFlag = True;
	}

	if(%index == 13)
	{
		if(%castPos != "")
		{
			CreateAndDetBomb(%clientId, "Bomb11", %castPos, True, %index);

			%overrideEndSound = True;
			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Could not find a target.");
			%returnFlag = False;
		}
	}

	if(%index == 14)
	{
		if(%castPos != "")
		{
			CreateAndDetBomb(%clientId, "Bomb9", %castPos, True, %index);

			%overrideEndSound = True;
			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Could not find a target.");
			%returnFlag = False;
		}
	}

	if(%index == 15)
	{
		if(%castPos != "")
		{
			CreateAndDetBomb(%clientId, "Bomb7", %castPos, True, %index);

			%overrideEndSound = True;
			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Could not find a target.");
			%returnFlag = False;
		}
	}

	if(%index == 16)
	{
		if(%castPos != "")
		{
			%minrad = 0;
			%maxrad = $Spell::radius[%index] / 2;
			for(%i = 0; %i <= 8; %i++)
			{
				%tempPos = RandomPositionXY(%minrad, %maxrad);

				%xPos = GetWord(%tempPos, 0) + GetWord(%castPos, 0);
				%yPos = GetWord(%tempPos, 1) + GetWord(%castPos, 1);
				%zPos = GetWord(%castPos, 2);
		
				%newPos = %xPos @ " " @ %yPos @ " " @ %zPos;

				schedule("CreateAndDetBomb(" @ %clientId @ ", \"Bomb10\", \"" @ %newPos @ "\", False, " @ %index @ ");", %i / 7, %player);
			}
			CreateAndDetBomb(%clientId, "Bomb10", %castPos, True, %index);

			%overrideEndSound = True;
			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Could not find a target.");
			%returnFlag = False;
		}
	}

	if(%index == 17)
	{
		if(%castPos != "")
		{
			%minrad = 0;
			%maxrad = $Spell::radius[%index];
			for(%i = 0; %i <= 8; %i++)
			{
				%tempPos = RandomPositionXY(%minrad, %maxrad);

				%xPos = GetWord(%tempPos, 0) + GetWord(%castPos, 0);
				%yPos = GetWord(%tempPos, 1) + GetWord(%castPos, 1);
				%zPos = GetWord(%castPos, 2) + (%i / 3);
		
				%newPos = %xPos @ " " @ %yPos @ " " @ %zPos;

				schedule("CreateAndDetBomb(" @ %clientId @ ", \"Bomb12\", \"" @ %newPos @ "\", False, " @ %index @ ");", %i / 24, %player);
			}
			CreateAndDetBomb(%clientId, "Bomb12", %castPos, True, %index);

			%overrideEndSound = True;
			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Could not find a target.");
			%returnFlag = False;
		}
	}

	if(%index == 18)
	{
		if(%castPos != "")
		{
			%minrad = 0;
			%maxrad = 5;
			for(%i = 0; %i <= 24; %i++)
			{
				%tempPos = RandomPositionXY(%minrad, %maxrad);

				%xPos = GetWord(%tempPos, 0) + GetWord(%castPos, 0);
				%yPos = GetWord(%tempPos, 1) + GetWord(%castPos, 1);
				%zPos = GetWord(%castPos, 2) + 72 - (%i * 3);
		
				%newPos = %xPos @ " " @ %yPos @ " " @ %zPos;

				schedule("CreateAndDetBomb(" @ %clientId @ ", \"Bomb9\", \"" @ %newPos @ "\", False, " @ %index @ ");", %i / 16, %player);
			}
			schedule("CreateAndDetBomb(" @ %clientId @ ", \"Bomb1\", \"" @ %castPos @ "\", True, " @ %index @ ");", %i / 16, %player);

			%overrideEndSound = True;
			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Could not find a target.");
			%returnFlag = False;
		}
	}

	if(%index == 19)
	{
		if(%castPos != "")
		{
			%minrad = 0;
			%maxrad = 4;
			for(%i = 0; %i <= 10; %i++)
			{
				%tempPos = RandomPositionXY(%minrad, %maxrad);

				%xPos = GetWord(%tempPos, 0) + GetWord(%castPos, 0);
				%yPos = GetWord(%tempPos, 1) + GetWord(%castPos, 1);
				%zPos = GetWord(%castPos, 2) + (%i / 4);
		
				%newPos = %xPos @ " " @ %yPos @ " " @ %zPos;

				schedule("CreateAndDetBomb(" @ %clientId @ ", \"Bomb7\", \"" @ %newPos @ "\", False, " @ %index @ ");", %i / 20, %player);
			}
			for(%i = 0; %i <= 10; %i++)
			{
				%tempPos = RandomPositionXY(%minrad, %maxrad);

				%xPos = GetWord(%tempPos, 0) + GetWord(%castPos, 0);
				%yPos = GetWord(%tempPos, 1) + GetWord(%castPos, 1);
				%zPos = GetWord(%castPos, 2) + (%i / 4);
		
				%newPos = %xPos @ " " @ %yPos @ " " @ %zPos;

				schedule("CreateAndDetBomb(" @ %clientId @ ", \"Bomb8\", \"" @ %newPos @ "\", False, " @ %index @ ");", %i / 20, %player);
			}

			schedule("CreateAndDetBomb(" @ %clientId @ ", \"Bomb5\", \"" @ %castPos @ "\", False, " @ %index @ ");", 1.0, %player);
			schedule("CreateAndDetBomb(" @ %clientId @ ", \"Bomb6\", \"" @ %castPos @ "\", False, " @ %index @ ");", 1.05, %player);
			schedule("CreateAndDetBomb(" @ %clientId @ ", \"Bomb14\", \"" @ %castPos @ "\", True, " @ %index @ ");", 1.1, %player);

			%overrideEndSound = True;
			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Could not find a target.");
			%returnFlag = False;
		}
	}
	if(%index == 21)
	{
		if(!fetchData(%clientId, "currentlyRemorting"))
		{
			%castPos = DoRemort(%clientId);		

			%extraDelay = 0.22;
			%returnFlag = True;
		}
		else
			%returnFlag = False;
	}
	if(%index == 22)
	{
		//full heal self spell

		Client::sendMessage(%clientId, $MsgBeige, "Fully healing self");

		setHP(%clientId, fetchData(%clientId, "MaxHP"));

		%castPos = GameBase::getPosition(%clientId);

		%returnFlag = True;
	}
	if(%index == 23 || %index == 24 || %index == 31)
	{
		//23 = mass heal spell
		//24 = mass full heal spell
		//31 = mass shield spell

		%b = $Spell::radius[%index] * 2;
		%set = newObject("set", SimSet);
		%n = containerBoxFillSet(%set, $SimPlayerObjectType, GameBase::getPosition(%clientId), %b, %b, %b, 0);

		Group::iterateRecursive(%set, DoBoxFunction, %clientId, %index, %w2);
		deleteObject(%set);

		%overrideEndSound = True;

		%returnFlag = True;
	}
	if(%index == 25)
	{
		//shield self spell

		Client::sendMessage(%clientId, $MsgBeige, "Shielding self");

		UpdateBonusState(%clientId, $Spell::damageValue[%index], $Spell::ticks[%index]);

		%castPos = GameBase::getPosition(%clientId);

		%returnFlag = True;
	}
	if(%index == 26 || %index == 27 || %index == 28 || %index == 29 || %index == 30)
	{
		//shield self or other (LOS) 1st, 2nd, 3rd, 4th, 5th

		if(getObjectType(%castObj) == "Player" && !Player::isAiControlled(%clientId))
			%id = Player::getClient(%castObj);
		else
			%id = %clientId;

		Client::sendMessage(%clientId, $MsgBeige, "Shielding " @ Client::getName(%id));
		if(%clientId != %id)
			Client::sendMessage(%id, $MsgBeige, Client::getName(%clientId) @ " is casting " @ $Spell::name[%index] @ " on you.");

		UpdateBonusState(%id, $Spell::damageValue[%index], $Spell::ticks[%index]);

		%castPos = GameBase::getPosition(%id);

		%returnFlag = True;
	}
	if(%index == 32)
	{
		//mimic spell
		if(Zone::getType(fetchData(%clientId, "zone")) == "PROTECTED")
		{
			Client::sendMessage(%clientId, $MsgRed, "You can't cast mimic in protected territory.");
			%overrideEndsound = True;
			%returnFlag = False;
		}
		else
		{
			%id = Player::getClient(%castObj);
			if(getObjectType(%castObj) == "Player")
			{
				%skilltype = $SkillType[$Spell::keyword[%index]];
				%troll = fetchData(%id, "LVL") + floor(getRandom() * ($PlayerSkill[%id, %skilltype] + ($PlayerSkill[%id, $SkillSpellResistance] * (1/2)) ));
				%yroll = fetchData(%clientId, "LVL") + floor(getRandom() * $PlayerSkill[%clientId, %skilltype]);

				if(%yroll > %troll)
				{
// ** this code used to put all your items into storage upon mimic.
//					%max = getNumItems();
//					for(%i = 0; %i < %max; %i++)
//					{
//						%checkItem = getItemData(%i);
//						%checkItemCount = Player::getItemCount(%clientId, %checkItem);
//						if(%checkItemCount)
//						{
//							%b = %checkItem;
//							if(%b.className == "Equipped")
//								%b = String::getSubStr(%b, 0, String::len(%b)-1);
//			
//							storeData(%clientId, "BankStorage", SetStuffString(fetchData(%clientId, "BankStorage"), %b, %checkItemCount));
//							Player::setItemCount(%clientId, %checkItem, 0);
//						}
//					}
					storeData(%clientId, "RACE", fetchData(%id, "RACE"));
					storeData(%clientId, "isMimic", True);
				
					UpdateTeam(%clientId);
					RefreshAll(%clientId);
				
					%castPos = GameBase::getPosition(%clientId);
					%returnFlag = True;
				}
				else
				{
					Client::sendMessage(%clientId, $MsgBeige, "Mimic failed.");
					%overrideEndsound = True;
					%returnFlag = False;
				}
			}
			else
			{
				Client::sendMessage(%clientId, $MsgBeige, "Could not find a target.");
				%overrideEndsound = True;
				%returnFlag = False;
			}
		}
	}
	if(%index == 33)
	{
		//mass transport spell

		%zoneId = GetZoneByKeywords(%clientId, %w2, 3);

		if(%zoneId != False)
		{
			%b = $Spell::radius[%index] * 2;
			%set = newObject("set", SimSet);
			%n = containerBoxFillSet(%set, $SimPlayerObjectType, GameBase::getPosition(%clientId), %b, %b, %b, 0);

			Group::iterateRecursive(%set, DoBoxFunction, %clientId, %index, %zoneId);
			deleteObject(%set);

			%overrideEndSound = True;

			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Mass Transportation failed.");
			%returnFlag = False;
		}
	}
	return EndCast(%clientid,%overrideEndSound,%extradelay,%index,%castpos,%returnflag);

//	Player::setAnimation(%clientId, 39);
//
//	if(!%overrideEndSound)
//	{
//		if(%extraDelay == "")
//			playSound($Spell::endSound[%index], %castPos);
//		else
//			schedule("playSound(" @ $Spell::endSound[%index] @ ", \"" @ %castPos @ "\");", %extraDelay);
//	}
//
//	//==================================================================
//
//	%skilltype = $SkillType[$Spell::keyword[%index]];
//	if(%returnFlag == True)
//	{
//		storeData(%clientId, "SpellCastStep", 2);
//
//		if(%skilltype == $SkillNeutralCasting || %skilltype == $SkillDefensiveCasting)
//			UseSkill(%clientId, %skilltype, True, True);
//		UseSkill(%clientId, $SkillEnergy, True, True);
//
//		return True;
//	}
//	else if(%returnFlag == False)
//	{
//		storeData(%clientId, "SpellCastStep", 2);
//
//		UseSkill(%clientId, %skilltype, False, True);
//		UseSkill(%clientId, $SkillEnergy, False, True);
//
//		return False;
//	}
}

function EndCast(%clientid,%overrideEndSound,%extradelay,%index,%castpos,%returnflag){	Player::setAnimation(%clientId, 39);	if(!%overrideEndSound)	{		if(%extraDelay == "")
			playSound($Spell::endSound[%index], %castPos);		else			schedule("playSound(" @ $Spell::endSound[%index] @ ", \"" @ %castPos @ "\");", %extraDelay);	}		storeData(%clientId, "SpellCastStep", 2);	//==================================================================	%recovTime = fetchData(%clientId, "SpellRecovTime");	%skilltype = $SkillType[$Spell::keyword[%index]];	if(%returnFlag == True)	{		if(%skilltype == $SkillNeutralCasting || %skilltype == $SkillDefensiveCasting)			UseSkill(%clientId, %skilltype, True, True);		UseSkill(%clientId, $SkillEnergy, True, True);		%tempManaCost2 = $Spell::manaCost[%index] / 2;		%tempManaCost = floor($Spell::manaCost[%index] / 2);		//pecho(%tempManaCost2 @ " " @%tempManaCost);		if(%tempManaCost2 != %tempManaCost)			%tempManaCost += 1;		refreshMANA(%clientId, %tempManaCost);	}	else if(%returnFlag == False)	{		UseSkill(%clientId, %skilltype, False, True);		UseSkill(%clientId, $SkillEnergy, False, True);		%recovTime = %recovTime * 0.5;	}	if(%clientId.repack > 32){		remoteEval(%clientId, "rpgbarhud", %recovTime, 4, 2, "||");		schedule("storeData(" @ %clientId @ ", \"SpellCastStep\", \"\");", %recovTime, %clientId);	}	else		schedule("storeData(" @ %clientId @ ", \"SpellCastStep\", \"\");sendDoneRecovMsg(" @ %clientId @ ");", %recovTime);	return %returnFlag;}

function sendDoneRecovMsg(%clientId)
{
	//this function is here just to make the schedule command where this is called easier to read
	Client::sendMessage(%clientId, $MsgBeige, "You are ready to cast.");
}

function CreateAndDetBomb(%clientId, %b, %castPos, %doDamage, %index)
{
	dbecho($dbechoMode, "CreateAndDetBomb(" @ %clientId @ ", " @ %b @ ", " @ %castPos @ ", " @ %index @ ")");

	%player = Client::getOwnedObject(%clientId);

	%bomb = newObject("", "Mine", %b);

	addToSet("MissionCleanup", %bomb);

	GameBase::Throw(%bomb, %player, 0, false);
	GameBase::setPosition(%bomb, %castPos);
	
	if(%doDamage)
		SpellRadiusDamage(%clientId, %castPos, %index);

	playSound($Spell::endSound[%index], %castPos);
}

function SpellDamage(%clientId, %targetId, %damageValue, %index)
{
	dbecho($dbechoMode, "SpellDamage(" @ %clientId @ ", " @ %targetId @ ", " @ %damageValue @ ", " @ %index @ ")");

	GameBase::virtual(%targetId, "onDamage", $SpellDamageType, %damageValue, "0 0 0", "0 0 0", "0 0 0", "torso", "front_right", %clientId, $Spell::keyword[%index]);
}

function SpellRadiusDamage(%clientId, %pos, %index)
{
	dbecho($dbechoMode, "SpellRadiusDamage(" @ %clientId @ ", " @ %pos @ ", " @ %index @ ")");

	%b = $Spell::radius[%index] * 2;
	%set = newObject("set", SimSet);
	%n = containerBoxFillSet(%set, $SimPlayerObjectType, %pos, %b, %b, %b, 0);

	Group::iterateRecursive(%set, DoSpellDamage, %clientId, %pos, %index);
	deleteObject(%set);
}
function DoSpellDamage(%object, %clientId, %pos, %index)
{
	dbecho($dbechoMode, "DoSpellDamage(" @ %object @ ", " @ %clientId @ ", " @ %pos @ ", " @ %index @ ")");

	%id = Player::getClient(%object);

	%percMin = 5;
	%percMax = 100;

	%dist = Vector::getDistance(%pos, GameBase::getPosition(%id));

	if(%dist <= $Spell::radius[%index])
	{
		%newDamage = SpellCalcRadiusDamage(%dist, $Spell::radius[%index], $Spell::damageValue[%index], %percMin, %percMax);
		SpellDamage(%clientId, %id, %newDamage, %index);
	}
}

function SpellCalcRadiusDamage(%dist, %radius, %dmg, %percMin, %percMax)
{
	dbecho($dbechoMode, "SpellCalcRadiusDamage(" @ %dist @ ", " @ %radius @ ", " @ %dmg @ ", " @ %percMin @ ", " @ %percMax @ ")");

	%newdmg = %dmg - (%dist * (%dmg / %radius));

	%p = (%newdmg * 100) / %dmg;

	if(%p < %percMin)
		%p = %percMin;
	else if(%p > %percMax)
		%p = %percMax;

	%newdmg = (%p * %dmg) / 100;

	return %newdmg;
}

function GetBestSpell(%clientId, %type, %semiRandomSpell)
{
	dbecho($dbechoMode, "GetBestSpell(" @ %clientId @ ", " @ %type @ ", " @ %semiRandomSpell @ ")");

	%wdelay = 10;	//weights
	%wrecov = 0.5;

	%bestSpell = -1;
	%backupSpell = "";
	%highest = 0.1;

	for(%i = 1; $Spell::keyword[%i] != ""; %i++)
	{
		if(SkillCanUse(%clientId, $Spell::keyword[%i]))
		{
			if(fetchData(%clientId, "MANA") >= $Spell::manaCost[%i])
			{
				%d = ( ($Spell::delay[%i] / %wdelay) + ($Spell::recoveryTime[%i] / %wrecov) );
				%x = (100 / %d) * $Spell::refVal[%i];
				%v =  %x * %type;

				if(%semiRandomSpell)
				{
					%r = getRandom() * 100;
					%rr = getRandom() * 100;
				}
				else
				{
					%r = 1;
					%rr = 0;
				}

				if(%v > %highest)
				{
					if(%r > %rr)
					{
						%bestSpell = %i;
						%highest = %v;
					}
					else
						%backupSpell = %i;
				}
			}
		}
	}
	if(%bestSpell == -1 && %backupSpell != "")
		%bestSpell = %backupSpell;

	return %bestSpell;
}

function CalcSpellMiss(%clientId, %targetId, %index)
{
	dbecho($dbechoMode, "CalcSpellMiss(" @ %clientId @ ", " @ %targetId @ ", " @ %index @ ")");

	%range = $Spell::LOSrange[%index];
	%dist = Vector::getDistance(GameBase::getPosition(%clientId), GameBase::getPosition(%targetId));

	%m = floor((getRandom() * %range)) + (%range / 6);

	//echo(%dist @ " / " @ %range @ " : --> " @ %m);
	if(%m > %dist)
		return False;
	else
		return True;
}

function DoBoxFunction(%object, %clientId, %index, %extra)
{
	dbecho($dbechoMode, "DoBoxFunction(" @ %object @ ", " @ %clientId @ ", " @ %index @ ", " @ %extra @ ")");

	%id = Player::getClient(%object);

	if(%index == 23)
	{
		if(GameBase::getTeam(%clientId) == GameBase::getTeam(%id))
		{
			Client::sendMessage(%clientId, $MsgBeige, "Mass Healing " @ Client::getName(%id));
			if(%clientId != %id)
				Client::sendMessage(%id, $MsgBeige, "You are being Mass Healed by " @ Client::getName(%clientId));

			%r = $Spell::damageValue[%index] / $TribesDamageToNumericDamage;
			refreshHP(%id, %r);

			%castPos = GameBase::getPosition(%id);

			CreateAndDetBomb(%clientId, "Bomb10", %castPos, False, %index);
			playSound($Spell::endSound[%index], %castPos);
		}
	}
	if(%index == 24)
	{
		if(GameBase::getTeam(%clientId) == GameBase::getTeam(%id))
		{
			Client::sendMessage(%clientId, $MsgBeige, "Mass Fully Healing " @ Client::getName(%id));
			if(%clientId != %id)
				Client::sendMessage(%id, $MsgBeige, "You are being Mass Fully Healed by " @ Client::getName(%clientId));

			setHP(%id, fetchData(%id, "MaxHP"));

			%castPos = GameBase::getPosition(%id);

			CreateAndDetBomb(%clientId, "Bomb10", %castPos, False, %index);
			playSound($Spell::endSound[%index], %castPos);
		}
	}
	if(%index == 31)
	{
		if(GameBase::getTeam(%clientId) == GameBase::getTeam(%id))
		{
			Client::sendMessage(%clientId, $MsgBeige, "Shielding " @ Client::getName(%id));
			if(%clientId != %id)
				Client::sendMessage(%id, $MsgBeige, Client::getName(%clientId) @ " is casting " @ $Spell::name[%index] @ " on you.");

			UpdateBonusState(%id, $Spell::damageValue[%index], $Spell::ticks[%index]);

			%castPos = GameBase::getPosition(%id);

			CreateAndDetBomb(%clientId, "Bomb10", %castPos, False, %index);
			playSound($Spell::endSound[%index], %castPos);
		}
	}
	if(%index == 33)
	{
		if(IsInCommaList(fetchData(%clientId, "grouplist"), Client::getName(%id)) && IsInCommaList(fetchData(%id, "grouplist"), Client::getName(%clientId)) || %clientId == %id)
		{
			Client::sendMessage(%clientId, $MsgBeige, "Transporting " @ Client::getName(%id) @ " to " @ Zone::getDesc(%extra));
			if(%clientId != %id)
				Client::sendMessage(%id, $MsgBeige, Client::getName(%clientId) @ " is transporting you to " @ Zone::getDesc(%extra));

			//teleport

			%system = Object::getName(%extra);
			%type = GetWord(%system, 0);
			%desc = String::getSubStr(%system, String::len(%type)+1, 9999);

			%castPos = TeleportToMarker(%id, "Zones\\" @ %system @ "\\DropPoints", False, True);
			CheckAndBootFromArena(%id);
			NullItemList(%clientId, Lore, $MsgRed, "You lost all %1s you were carrying when you teleported.");

			if(!fetchData(%id, "invisible"))
				GameBase::startFadeIn(%id);

			Player::setDamageFlash(%id, 0.7);

			%extraDelay = 0.22;
			schedule("playSound(" @ $Spell::endSound[%index] @ ", \"" @ %castPos @ "\");", %extraDelay);
		}
	}
}

function SpellCanCast(%clientId, %keyword)
{
	dbecho($dbechoMode, "SpellCanCast(" @ %clientId @ ", " @ %keyword @ ")");

	for(%i = 1; $Spell::keyword[%i] != ""; %i++)
	{
		if(String::ICompare($Spell::keyword[%i], %keyword) == 0)
		{
			if(SkillCanUse(%clientId, $Spell::keyword[%i]))
			{
				if(fetchData(%clientId, "MaxMANA") >= $Spell::manaCost[%i])
					return True;
			}
		}
	}
	return False;
}
function SpellCanCastNow(%clientId, %keyword)
{
	dbecho($dbechoMode, "SpellCanCastNow(" @ %clientId @ ", " @ %keyword @ ")");

	for(%i = 1; $Spell::keyword[%i] != ""; %i++)
	{
		if(String::ICompare($Spell::keyword[%i], %keyword) == 0)
		{
			if(SkillCanUse(%clientId, $Spell::keyword[%i]))
			{
				if(fetchData(%clientId, "MANA") >= $Spell::manaCost[%i])
					return True;
			}
		}
	}
	return False;
}
function casting::teleport(%clientId, %index, %oldpos, %castObj, %w2){	%zoneId = GetNearestZone(%clientId, %w2, 3);	if(%zoneId != False)	{		Schedule::Add("DoCastSpell(" @ %clientId @ ", " @ %index @ ", \"" @ %oldpos @ "\", \"" @ %castObj @ "\", \"" @ %w2 @ "\");", $Spell::delay[%index], "spell"@%clientId);	}	else	{		disableOverrides(%clientId);		%clientId.overrideKeybinds = 2;		%clientId.keyOverride = "casting::teleportmenu_input";		%clientId.tpmenutype = "";		%clientId.castingmenuindex = %index;		casting::teleportmenu(%clientid);	}}
function casting::teleportmenu(%clientId){	%msg = "<jc>Teleport to nearest:";	%msg = %msg @ "\n\n<f2>1:<f0> Town\n<f2>2:<f0> Dungeon";	%msg = %msg @ "\n\n<f1>0:<f0> Cancel";	rpg::longPrint(%clientId,%msg,1,0.6);	schedule::add("casting::teleportmenu("@%clientId@");",0.3,"transportmenu"@%clientId, %clientId);//should override other transport menu so you don't get both}function casting::teleportmenu_input(%clientId, %key){	%index = floor(%clientId.castingmenuindex);	if(%key == 0){		disableOverrides(%clientId);		return;	}	else{		if(%key == 1){			%clientId.tpmenutype = "town";			%chosen = True;		}		else if(%key == 2){			%clientId.tpmenutype = "dungeon";			%chosen = True;		}		if(%chosen){			Schedule::Add("DoCastSpell(" @ %clientId @ ", " @ %index @ ", \"" @ GameBase::getPosition(%clientId) @ "\", 0, \"" @ %clientId.tpmenutype @ "\");", $Spell::delay[%index], "spell"@%clientId);			%clientId.overrideKeybinds = 1;			disableOverrides(%clientId);			%chosen = True;		}	}	if(!%chosen)		client::sendmessage(%clientId, $msgRed,"That isn't on the menu.");}