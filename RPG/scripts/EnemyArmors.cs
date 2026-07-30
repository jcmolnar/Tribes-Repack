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



$Server::teamName[0] = "Citizen";
$Server::teamSkin[0] = "rpgbase";
$Server::teamName[1] = "Enemy";
$Server::teamSkin[1] = "rpgbase";
$Server::teamName[2] = "Greenskins";
$Server::teamSkin[2] = "rpgorc";
$Server::teamName[3] = "Gnoll";
$Server::teamSkin[3] = "rpggnoll";
$Server::teamName[4] = "Undead";
$Server::teamSkin[4] = "undead";
$Server::teamName[5] = "Elf";
$Server::teamSkin[5] = "rpgelf";
$Server::teamName[6] = "Minotaur";
$Server::teamSkin[6] = "min";
$Server::teamName[7] = "Uber";
$Server::teamSkin[7] = "fedmonster";

//------------------------------

$NameForRace[generic] = "MaleHuman";
$NameForRace[merchant] = "MaleHuman";
$NameForRace[banker] = "MaleHuman";
$NameForRace[assassin] = "MaleHuman";
$NameForRace[mage] = "MaleHuman";
$NameForRace[porter] = "FemaleHuman";
$NameForRace[manager] = "MaleHuman";

$NameForRace[Thug] = "Traveller";
$NameForRace[Monk] = "Traveller";
$NameForRace[Miner] = "Traveller";
$NameForRace[Commoner] = "Traveller";
$NameForRace[Mercenary] = "Traveller";
$NameForRace[Militia] = "Traveller";
$NameForRace[Brigand] = "Traveller";
$NameForRace[Marauder] = "Traveller";
$NameForRace[Knight] = "Traveller";
$NameForRace[Paladin] = "Traveller";

$NameForRace[Runt] = "Goblin";
$NameForRace[Thief] = "Goblin";
$NameForRace[Wizard] = "Goblin";
$NameForRace[Raider] = "Goblin";

$NameForRace[Pup] = "Gnoll";
$NameForRace[Shaman] = "Gnoll";
$NameForRace[Scavenger] = "Gnoll";
$NameForRace[Hunter] = "Gnoll";

$NameForRace[Warlock] = "Orc";
$NameForRace[Berserker] = "Orc";
$NameForRace[Ravager] = "Orc";
$NameForRace[Slayer] = "Orc";

$NameForRace[Ruffian] = "Ogre";
$NameForRace[Destroyer] = "Ogre";
$NameForRace[Halberdier] = "Ogre";
$NameForRace[Dreadnought] = "Ogre";
$NameForRace[Magi] = "Ogre";

$NameForRace[Mauler] = "Zombie";
$NameForRace[Thrasher] = "Zombie";
$NameForRace[Skeleton] = "Undead";
$NameForRace[Necromancer] = "Undead";
$NameForRace[Spawn] = "Demon";

$NameForRace[Protector] = "MaleElf";
$NameForRace[Peacekeeper] = "MaleElf";
$NameForRace[Lord] = "MaleElf";
$NameForRace[Champion] = "MaleElf";
$NameForRace[Conjurer] = "FemaleElf";

$NameForRace[Civilian] = "MaleHuman";
$NameForRace[Gladiator] = "MaleHuman";

$NameForRace[Goliath] = "Minotaur";
$NameForRace[Reaper] = "Minotaur";

$NameForRace[Sloth] = "Uber";
$NameForRace[Gohort] = "Uber";

//------------------------------

$ArmorTypeToRace[TravellerArmor] = "Traveller";
$ArmorTypeToRace[GoblinArmor] = "Goblin";
$ArmorTypeToRace[GnollArmor] = "Gnoll";
$ArmorTypeToRace[OrcArmor] = "Orc";
$ArmorTypeToRace[OgreArmor] = "Ogre";
$ArmorTypeToRace[UndeadArmor] = "Undead";
$ArmorTypeToRace[MaleElfArmor] = "MaleElf";
$ArmorTypeToRace[FemaleElfArmor] = "FemaleElf";
$ArmorTypeToRace[MinotaurArmor] = "Minotaur";
$ArmorTypeToRace[UberArmor] = "Uber";
$ArmorTypeToRace[ZombieArmor] = "Zombie";
$ArmorTypeToRace[DemonArmor] = "Demon";

$RaceToArmorType[Goblin] = "GoblinArmor";
$RaceToArmorType[Gnoll] = "GnollArmor";
$RaceToArmorType[Orc] = "OrcArmor";
$RaceToArmorType[Ogre] = "OgreArmor";
$RaceToArmorType[Undead] = "UndeadArmor";
$RaceToArmorType[Traveller] = "TravellerArmor";
$RaceToArmorType[MaleElf] = "MaleElfArmor";
$RaceToArmorType[FemaleElf] = "FemaleElfArmor";
$RaceToArmorType[Minotaur] = "MinotaurArmor";
$RaceToArmorType[Uber] = "UberArmor";
$RaceToArmorType[Zombie] = "ZombieArmor";
$RaceToArmorType[Demon] = "DemonArmor";

//------------------------------

$spawnIndex[1] = "merchant";
$spawnIndex[2] = "banker";
$spawnIndex[3] = "assassin";
$spawnIndex[4] = "mage";
$spawnIndex[5] = "engineer";
$spawnIndex[6] = "elder";
$spawnIndex[8] = "porter";
$spawnIndex[10] = "manager";

$spawnIndex[11] = "Thug";
$spawnIndex[12] = "Monk";
$spawnIndex[13] = "Miner";
$spawnIndex[14] = "Commoner";

$spawnIndex[15] = "Runt";
$spawnIndex[16] = "Thief";
$spawnIndex[17] = "Wizard";
$spawnIndex[18] = "Raider";

$spawnIndex[19] = "Pup";
$spawnIndex[20] = "Shaman";
$spawnIndex[21] = "Scavenger";
$spawnIndex[22] = "Hunter";

$spawnIndex[23] = "Warlock";
$spawnIndex[24] = "Berserker";
$spawnIndex[25] = "Ravager";
$spawnIndex[26] = "Slayer";

$spawnIndex[27] = "Ruffian";
$spawnIndex[28] = "Destroyer";
$spawnIndex[29] = "Halberdier";
$spawnIndex[30] = "Dreadnought";

$spawnIndex[31] = "Mauler";
$spawnIndex[32] = "Thrasher";
$spawnIndex[33] = "Skeleton";
$spawnIndex[34] = "Necromancer";

$spawnIndex[35] = "Protector";
$spawnIndex[36] = "Peacekeeper";
$spawnIndex[37] = "Lord";
$spawnIndex[38] = "Champion";

$spawnIndex[39] = "Brigand";
$spawnIndex[40] = "Marauder";
$spawnIndex[41] = "Knight";
$spawnIndex[42] = "Paladin";

$spawnIndex[43] = "Civilian";
$spawnIndex[44] = "Gladiator";
$spawnIndex[45] = "Mercenary";
$spawnIndex[46] = "Militia";

$spawnIndex[47] = "Goliath";
$spawnIndex[48] = "Reaper";

$spawnIndex[49] = "Sloth";
$spawnIndex[50] = "Gohort";

$spawnIndex[51] = "Conjurer";
$spawnIndex[52] = "Magi";
$spawnIndex[53] = "Spawn";

//------------------------------

$BotEquipment[Runt] = 		"CLASS Fighter LVL 1 COINS 1/50 LCK 0 RKnife 1 Quartz 4/-300";
$BotEquipment[Thief] = 		"CLASS Fighter LVL 5/50 COINS 3/50 LCK 0 RKnife 1 Sling 1 SmallRock 20/50 BlackStatue 1/-100";
$BotEquipment[Wizard] = 	"CLASS Mage LVL 9/50 COINS 5/50 LCK 0 CastingBlade 1 Turquoise 1/-500";
$BotEquipment[Raider] = 	"CLASS Fighter LVL 11/50 COINS 4/50 LCK 0 RPickAxe 1 BlackStatue 1/-150 Jade 1/-300";

$BotEquipment[Pup] = 		"CLASS Fighter LVL 10/50 COINS 6/50 LCK 0 RClub 1 CrystalBluePotion 1 Ruby 1/-2000";
$BotEquipment[Shaman] = 	"CLASS Mage LVL 12/50 COINS 7/50 LCK 0 CastingBlade 1 RClub 1";
$BotEquipment[Scavenger] = 	"CLASS Fighter LVL 15/50 COINS 8/50 LCK 0 RClub 1 Sapphire 2/-5000";
$BotEquipment[Hunter] = 	"CLASS Ranger LVL 17/50 COINS 9/50 LCK 0 RWarAxe 1 Sling 1 SmallRock 20/50 Topaz 3/-3000";

$BotEquipment[Warlock] = 	"CLASS Mage LVL 16/50 COINS 10/50 LCK 0 CastingBlade 1 Sling 1 SmallRock 20/50 EnchantedStone 1/-100";
$BotEquipment[Berserker] = 	"CLASS Fighter LVL 20/50 COINS 13/50 LCK 0 RBroadSword 1 Topaz 4/-500";
$BotEquipment[Ravager] = 	"CLASS Fighter LVL 24/50 COINS 16/50 LCK 0 RBroadSword 1 BluePotion 3/30 Opal 4/-300";
$BotEquipment[Slayer] = 	"CLASS Fighter LVL 28/50 COINS 19/50 LCK 0 RBroadSword 1 RShortBow 1 BasicArrow 20/50 Opal 5/-250";

$BotEquipment[Ruffian] = 	"CLASS Fighter LVL 22/50 COINS 20/50 LCK 0 RBroadSword 1 Quartz 8/-200";
$BotEquipment[Destroyer] = 	"CLASS Fighter LVL 27/50 COINS 23/50 LCK 0 RSpikedClub 1";
$BotEquipment[Halberdier] = 	"CLASS Fighter LVL 31/50 COINS 26/50 LCK 0 RBroadSword 1 BluePotion 3/30";
$BotEquipment[Dreadnought] = 	"CLASS Fighter LVL 36/50 COINS 29/50 LCK 1 RWarAxe 1 RShortBow 1 BasicArrow 15/75";
$BotEquipment[Magi] =		"CLASS Mage LVL 42/50 COINS 50/50 LCK 1 CastingBlade 1 Emerald 1/-6000 Quartz 10/-200";

$BotEquipment[Mauler] = 	"CLASS Fighter LVL 45/50 COINS 20/50 LCK 0 RSpikedClub 1 Granite 10/-300";
$BotEquipment[Thrasher] =	"CLASS Fighter LVL 49/50 COINS 23/50 LCK 0 RLongSword 1 Opal 3/-300";
$BotEquipment[Skeleton] = 	"CLASS Fighter LVL 54/50 COINS 26/50 LCK 0 RWarAxe 1 SkeletonBone 1/-250 Turquoise 4/-300";
$BotEquipment[Necromancer] = 	"CLASS Mage LVL 61/50 COINS 29/50 LCK 1 CastingBlade 1 Sling 1 SmallRock 20/50 Diamond 1/-3000";
$BotEquipment[Spawn] = 		"CLASS Paladin LVL 180/90 COINS 500/50 LCK 2 Gladius 1 Diamond 1/-1000 Emerald 1/-700";

$BotEquipment[Protector] = 	"CLASS Ranger LVL 50/50 COINS 25/50 LCK 0 RShortSword 1 Ruby 2/-500";
$BotEquipment[Peacekeeper] =	"CLASS Ranger LVL 54/50 COINS 28/50 LCK 0 RLongSword 1 RShortBow 1 SheafArrow 40/50 Jade 5/-500";
$BotEquipment[Lord] = 		"CLASS Ranger LVL 59/50 COINS 31/50 LCK 1 RSpikedClub 1 RLightCrossbow 1 LightQuarrel 25/75 Emerald 1/-2800";
$BotEquipment[Champion] = 	"CLASS Ranger LVL 63/50 COINS 34/50 LCK 1 RLongSword 1 RLightCrossbow 1 HeavyQuarrel 25/75 Sapphire 3/-1000";
$BotEquipment[Conjurer] =	"CLASS Mage LVL 70/50 COINS 32/50 LCK 0 CastingBlade 1 Topaz 2/-300";

$BotEquipment[Brigand] = 	"CLASS Fighter LVL 75/50 COINS 30/50 LCK 0 RLongSword 1 Sapphire 2/-3000";
$BotEquipment[Marauder] =	"CLASS Fighter LVL 79/50 COINS 33/50 LCK 0 RLongSword 1 Opal 4/-300 Turquoise 1/-800";
$BotEquipment[Knight] = 	"CLASS Fighter LVL 83/50 COINS 36/50 LCK 0 RBroadSword 1 RShortBow 1 SheafArrow 40/50 Jade 2/-600";
$BotEquipment[Paladin] = 	"CLASS Mage LVL 87/50 COINS 39/50 LCK 1 CastingBlade 1 Topaz 1/-300";

$BotEquipment[Civilian] = 	"CLASS Fighter LVL 1 COINS 5/50 LCK 0 RBroadSword 1";
$BotEquipment[Gladiator] =	"CLASS Fighter LVL 1 LCK 0";
$BotEquipment[Mercenary] = 	"CLASS Ranger LVL 65/50 COINS 32/50 LCK 0 RBroadSword 1";
$BotEquipment[Militia] = 	"CLASS Paladin LVL 75/50 COINS 35/50 LCK 1 RLongSword 1";

$BotEquipment[Thug] = 		"CLASS Thief LVL 65/50 COINS 32/50 LCK 1 RBroadSword 1 Jade 5/-500";
$BotEquipment[Miner] = 		"CLASS Paladin LVL 29/50 COINS 35/50 LCK 0 RPickAxe 1 Parchment 1/-16000 Quartz 10/50 Opal 5/50 Turquoise 2/-50 Emerald 1/-1000";

$BotEquipment[Goliath] = 	"CLASS Fighter LVL 107/50 COINS 70/50 LCK 1 RBroadSword 1";
$BotEquipment[Reaper] = 	"CLASS Mage LVL 174/50 COINS 105/50 LCK 2 CastingBlade 1 Turquoise 5/-500";

$BotEquipment[Sloth] = 		"CLASS Paladin LVL 317/50 COINS 115/50 LCK 3 RLongSword 1 DragonScale 1/-3000 Gold 1/-1000";
$BotEquipment[Gohort] = 	"CLASS Mage LVL 527/50 COINS 135/50 LCK 4 CastingBlade 1 DragonScale 1/-300 Emerald 1/-1000";

//------------------------------

$TeamForRace[Traveller] = 1;
$TeamForRace[Goblin] = 2;
$TeamForRace[Gnoll] = 3;
$TeamForRace[Orc] = 2;
$TeamForRace[Ogre] = 2;
$TeamForRace[Undead] = 4;
$TeamForRace[MaleElf] = 5;
$TeamForRace[FemaleElf] = 5;
$TeamForRace[Minotaur] = 6;
$TeamForRace[Uber] = 7;
$TeamForRace[Zombie] = 4;
$TeamForRace[Demon] = 4;

//------------------------------

$RaceSound[Ogre, Death, 1] = SoundOgreDeath1;
$RaceSound[Ogre, Acquired, 1] = SoundOgreAcquired1;
$RaceSound[Ogre, Acquired, 2] = SoundOgreAcquired2;
$RaceSound[Ogre, Hit, 1] = SoundOgreHit1;
$RaceSound[Ogre, Hit, 2] = SoundOgreHit2;
$RaceSound[Ogre, Taunt, 1] = SoundOgreTaunt1;
$RaceSound[Ogre, Taunt, 2] = SoundOgreTaunt2;
$RaceSound[Ogre, RandomWait, 1] = SoundOgreRandom1;
$RaceSound[Ogre, RandomWait, 2] = SoundOgreRandom2;

$RaceSound[Undead, Death, 1] = SoundUndeadDeath1;
$RaceSound[Undead, Acquired, 1] = SoundUndeadAcquired1;
$RaceSound[Undead, Hit, 1] = SoundUndeadHit1;
$RaceSound[Undead, Hit, 2] = SoundUndeadHit2;
$RaceSound[Undead, Taunt, 1] = SoundUndeadTaunt1;
$RaceSound[Undead, RandomWait, 1] = SoundUndeadRandom1;

$RaceSound[MaleElf, Death, 1] = SoundTravellerDeath1;
$RaceSound[MaleElf, Acquired, 1] = SoundTravellerAcquired1;
$RaceSound[MaleElf, Acquired, 2] = SoundTravellerAcquired2;
$RaceSound[MaleElf, Acquired, 3] = SoundTravellerAcquired3;
$RaceSound[MaleElf, Hit, 1] = SoundTravellerHit1;
$RaceSound[MaleElf, Hit, 2] = SoundTravellerHit2;
$RaceSound[MaleElf, Hit, 3] = SoundTravellerHit3;

$RaceSound[Traveller, Death, 1] = SoundTravellerDeath1;
$RaceSound[Traveller, Acquired, 1] = SoundTravellerAcquired1;
$RaceSound[Traveller, Acquired, 2] = SoundTravellerAcquired2;
$RaceSound[Traveller, Acquired, 3] = SoundTravellerAcquired3;
$RaceSound[Traveller, Hit, 1] = SoundTravellerHit1;
$RaceSound[Traveller, Hit, 2] = SoundTravellerHit2;
$RaceSound[Traveller, Hit, 3] = SoundTravellerHit3;

$RaceSound[Minotaur, Death, 1] = SoundMinotaurDeath1;
$RaceSound[Minotaur, Acquired, 1] = SoundMinotaurAcquired1;
$RaceSound[Minotaur, Acquired, 2] = SoundMinotaurAcquired2;
$RaceSound[Minotaur, Hit, 1] = SoundMinotaurHit1;

$RaceSound[Uber, Death, 1] = SoundUberDeath1;
$RaceSound[Uber, Acquired, 1] = SoundUberAcquired1;
$RaceSound[Uber, Acquired, 2] = SoundUberAcquired2;
$RaceSound[Uber, RandomWait, 1] = SoundUberRandom1;
$RaceSound[Uber, Hit, 1] = SoundUberHit1;

$RaceSound[Goblin, Death, 1] = SoundGoblinDeath1;
$RaceSound[Goblin, Death, 2] = SoundGoblinDeath2;
$RaceSound[Goblin, Acquired, 1] = SoundGoblinAcquired1;
$RaceSound[Goblin, Acquired, 2] = SoundGoblinAcquired2;
$RaceSound[Goblin, Acquired, 3] = SoundGoblinAcquired3;
$RaceSound[Goblin, Taunt, 1] = SoundGoblinTaunt1;
$RaceSound[Goblin, RandomWait, 1] = SoundGoblinRandom1;
$RaceSound[Goblin, Hit, 1] = SoundGoblinHit1;
$RaceSound[Goblin, Hit, 2] = SoundGoblinHit2;
$RaceSound[Goblin, Hit, 3] = SoundGoblinHit3;
$RaceSound[Goblin, Hit, 4] = SoundGoblinHit4;
$RaceSound[Goblin, Hit, 5] = SoundGoblinHit5;
$RaceSound[Goblin, Hit, 6] = SoundGoblinHit6;
$RaceSound[Goblin, Hit, 7] = SoundGoblinHit7;
$RaceSound[Goblin, Hit, 8] = SoundGoblinHit8;

$RaceSound[Gnoll, Death, 1] = SoundGnollDeath1;
$RaceSound[Gnoll, Death, 2] = SoundGnollDeath2;
$RaceSound[Gnoll, Acquired, 1] = SoundGnollAcquired1;
$RaceSound[Gnoll, Taunt, 1] = SoundGnollTaunt1;
$RaceSound[Gnoll, RandomWait, 1] = SoundGnollRandom1;
$RaceSound[Gnoll, RandomWait, 2] = SoundGnollRandom2;
$RaceSound[Gnoll, Hit, 1] = SoundGnollHit1;
$RaceSound[Gnoll, Hit, 2] = SoundGnollHit2;

$RaceSound[Orc, Death, 1] = SoundUberDeath1;
$RaceSound[Orc, Acquired, 1] = SoundMinotaurAcquired1;
$RaceSound[Orc, Acquired, 2] = SoundMinotaurAcquired2;
$RaceSound[Orc, Acquired, 3] = SoundUberAcquired1;
$RaceSound[Orc, Acquired, 4] = SoundUberAcquired2;
$RaceSound[Orc, Hit, 1] = SoundMinotaurHit1;

$RaceSound[Zombie, Death, 1] = SoundUndeadDeath1;
$RaceSound[Zombie, Acquired, 1] = SoundUndeadRandom1;
$RaceSound[Zombie, Hit, 1] = SoundGnollRandom1;
$RaceSound[Zombie, Hit, 2] = SoundGnollRandom2;
$RaceSound[Zombie, Taunt, 1] = SoundUndeadTaunt1;
$RaceSound[Zombie, RandomWait, 1] = SoundUberAcquired2;

$RaceSound[Demon, Death, 1] = SoundUndeadDeath1;
$RaceSound[Demon, Acquired, 1] = SoundGnollAcquired1;
$RaceSound[Demon, Hit, 1] = SoundUndeadHit1;
$RaceSound[Demon, Hit, 2] = SoundUndeadHit2;
$RaceSound[Demon, Taunt, 1] = SoundUndeadTaunt1;
$RaceSound[Demon, RandomWait, 1] = SoundUndeadRandom1;

//------------------------------------------------------------------
// Traveller armor data:	(light)
//------------------------------------------------------------------

PlayerData TravellerArmor
{
	className = "Armor";
	shapeFile = "rpgmalehuman";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	flameShapeName = "lflame";
	shieldShapeName = "shield";
	shadowDetailMask = 1;

	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
	canCrouch = false;

	maxJetSideForceFactor = 1;
	maxJetForwardVelocity = 1.0;
	minJetEnergy = 60;
	jetForce = 1;
	jetEnergyDrain = 0.0;

	maxDamage = 1.0;
	maxForwardSpeed = $spdmed;
	maxBackwardSpeed = $spdmed * 0.8;
	maxSideSpeed = $spdmed * 0.75;

	groundForce = 75 * 9.0;
	mass = 9.0;
	groundTraction = 3.0;
	maxEnergy = 60;
	drag = 1.0;
	density = 1.2;

	minDamageSpeed = 16;
	damageScale = $damageScale;

	jumpImpulse = 75;
	jumpSurfaceMinDot = $jumpSurfaceMinDot;

	// animation data:
	// animation name, one shot, direction, firstPerson, chaseCam, thirdPerson, signalThread
	// movement animations:
	animData[0]  = { "root", none, 1, true, true, true, false, 0 };
	animData[1]  = { "run", none, 1, true, false, true, false, 3 };
	animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
	animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
	animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
	animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
	animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
	animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
	animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
	animData[19] = { "jet", none, 1, true, true, true, false, 3 };

	// misc. animations:
	animData[20] = { "die back", none, 1, true, false, false, false, 0 };
	animData[21] = { "throw", none, 1, true, false, false, false, 3 };
	animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
	animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
	animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
	// death animations:
	animData[25] = { "crouch die", none, 1, false, false, false, false, 4 };
	animData[26] = { "die chest", none, 1, false, false, false, false, 4 };
	animData[27] = { "die head", none, 1, false, false, false, false, 4 };
	animData[28] = { "die grab back", none, 1, false, false, false, false, 4 };
	animData[29] = { "die right side", none, 1, false, false, false, false, 4 };
	animData[30] = { "die left side", none, 1, false, false, false, false, 4 };
	animData[31] = { "die leg left", none, 1, false, false, false, false, 4 };
	animData[32] = { "die leg right", none, 1, false, false, false, false, 4 };
	animData[33] = { "die blown back", none, 1, false, false, false, false, 4 };
	animData[34] = { "die spin", none, 1, false, false, false, false, 4 };
	animData[35] = { "die forward", none, 1, false, false, false, false, 4 };
	animData[36] = { "die forward kneel", none, 1, false, false, false, false, 4 };
	animData[37] = { "die back", none, 1, false, false, false, false, 4 };

	// signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
	animData[39] = { "sign point", none, 1, true, false, false, false, 1 };
	animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
	animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
	animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

	// celebration animations:
	animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
	// taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
	// poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
	animData[50] = { "wave", none, 1, true, false, false, true, 1 };

	jetSound = NoSound;
	rFootSounds = 
	{
		SoundLFootRSoft,
		SoundLFootRHard,
		SoundLFootRSoft,
		SoundLFootRHard,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRHard,
		SoundLFootRSnow,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft
	}; 
	lFootSounds =
	{
		SoundLFootLSoft,
		SoundLFootLHard,
		SoundLFootLSoft,
		SoundLFootLHard,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLHard,
		SoundLFootLSnow,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft
	};

	footPrints = { 0, 1 };

	boxWidth = 0.5;
	boxDepth = 0.5;
	boxNormalHeight = 2.3;
	boxCrouchHeight = 1.8;

	boxNormalHeadPercentage  = 0.83;
	boxNormalTorsoPercentage = 0.53;
	boxCrouchHeadPercentage  = 0.6666;
	boxCrouchTorsoPercentage = 0.3333;

	boxHeadLeftPercentage  = 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage  = 0;
	boxHeadFrontPercentage = 1;
};

//------------------------------------------------------------------
// Goblin armor data:	(light)
//------------------------------------------------------------------

PlayerData GoblinArmor
{
	className = "Armor";
	shapeFile = "goblin";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	flameShapeName = "lflame";
	shieldShapeName = "shield";
	shadowDetailMask = 1;

	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
	canCrouch = false;

	maxJetSideForceFactor = 1;
	maxJetForwardVelocity = 1.0;
	minJetEnergy = 60;
	jetForce = 1;
	jetEnergyDrain = 0.0;

	maxDamage = 1.0;
	maxForwardSpeed = $spdlowmed;
	maxBackwardSpeed = $spdlowmed * 0.8;
	maxSideSpeed = $spdlowmed * 0.75;

	groundForce = 75 * 9.0;
	mass = 9.0;
	groundTraction = 3.0;
	maxEnergy = 60;
	drag = 1.0;
	density = 1.2;

	minDamageSpeed = 16;
	damageScale = $damageScale;

	jumpImpulse = 75;
	jumpSurfaceMinDot = $jumpSurfaceMinDot;

	// animation data:
	// animation name, one shot, direction, firstPerson, chaseCam, thirdPerson, signalThread
	// movement animations:
	animData[0]  = { "root", none, 1, true, true, true, false, 0 };
	animData[1]  = { "run", none, 1, true, false, true, false, 3 };
	animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
	animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
	animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
	animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
	animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
	animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
	animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
	animData[19] = { "jet", none, 1, true, true, true, false, 3 };

	// misc. animations:
	animData[20] = { "die back", none, 1, true, false, false, false, 0 };
	animData[21] = { "throw", none, 1, true, false, false, false, 3 };
	animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
	animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
	animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
	// death animations:
	animData[25] = { "crouch die", none, 1, false, false, false, false, 4 };
	animData[26] = { "die chest", none, 1, false, false, false, false, 4 };
	animData[27] = { "die head", none, 1, false, false, false, false, 4 };
	animData[28] = { "die grab back", none, 1, false, false, false, false, 4 };
	animData[29] = { "die right side", none, 1, false, false, false, false, 4 };
	animData[30] = { "die left side", none, 1, false, false, false, false, 4 };
	animData[31] = { "die leg left", none, 1, false, false, false, false, 4 };
	animData[32] = { "die leg right", none, 1, false, false, false, false, 4 };
	animData[33] = { "die blown back", none, 1, false, false, false, false, 4 };
	animData[34] = { "die spin", none, 1, false, false, false, false, 4 };
	animData[35] = { "die forward", none, 1, false, false, false, false, 4 };
	animData[36] = { "die forward kneel", none, 1, false, false, false, false, 4 };
	animData[37] = { "die back", none, 1, false, false, false, false, 4 };

	// signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
	animData[39] = { "sign point", none, 1, true, false, false, false, 1 };
	animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
	animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
	animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

	// celebration animations:
	animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
	// taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
	// poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
	animData[50] = { "wave", none, 1, true, false, false, true, 1 };

	jetSound = NoSound;
	rFootSounds = 
	{
		SoundLFootRSoft,
		SoundLFootRHard,
		SoundLFootRSoft,
		SoundLFootRHard,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRHard,
		SoundLFootRSnow,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft
	}; 
	lFootSounds =
	{
		SoundLFootLSoft,
		SoundLFootLHard,
		SoundLFootLSoft,
		SoundLFootLHard,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLHard,
		SoundLFootLSnow,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft
	};

	footPrints = { 0, 1 };

	boxWidth = 0.5;
	boxDepth = 0.5;
	boxNormalHeight = 2.3;
	boxCrouchHeight = 1.8;

	boxNormalHeadPercentage  = 0.83;
	boxNormalTorsoPercentage = 0.53;
	boxCrouchHeadPercentage  = 0.6666;
	boxCrouchTorsoPercentage = 0.3333;

	boxHeadLeftPercentage  = 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage  = 0;
	boxHeadFrontPercentage = 1;
};

//------------------------------------------------------------------
// Gnoll Armor data:	(medium)
//------------------------------------------------------------------

PlayerData GnollArmor
{
	className = "Armor";
	shapeFile = "marmorgnoll";
	flameShapeName = "mflame";
	shieldShapeName = "shield";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	shadowDetailMask = 1;

	canCrouch = false;
	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

	maxJetSideForceFactor = 1;
	maxJetForwardVelocity = 1.0;
	minJetEnergy = 60;
	jetForce = 1;
	jetEnergyDrain = 0.0;

	maxDamage = 1.0;
	maxForwardSpeed = $spdmed;
	maxBackwardSpeed = $spdmed * 0.8;
	maxSideSpeed = $spdmed * 0.75;
	groundForce = 75 * 9.0;
	mass = 9.0;
	groundTraction = 3.0;
	maxEnergy = 80;
	drag = 1.0;
	density = 1.2;

	minDamageSpeed = 16;
	damageScale = $damageScale;

	jumpImpulse = 75;
	jumpSurfaceMinDot = $jumpSurfaceMinDot;

	// animation data:
	// animation name, one shot, exclude, direction, firstPerson, chaseCam, thirdPerson, signalThread

	// movement animations:
	animData[0]  = { "root", none, 1, true, true, true, false, 0 };
	animData[1]  = { "run", none, 1, true, false, true, false, 3 };
	animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
	animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
	animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
	animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
	animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
	animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
	animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
	animData[19] = { "jet", none, 1, true, true, true, false, 3 };

	// misc. animations:
	animData[20] = { "die back", none, 1, true, false, false, false, 0 };
	animData[21] = { "throw", none, 1, true, false, false, false, 3 };
	animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
	animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
	animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
	// death animations:
	animData[25] = { "crouch die", none, 1, false, false, false, false, 4 };
	animData[26] = { "die chest", none, 1, false, false, false, false, 4 };
	animData[27] = { "die head", none, 1, false, false, false, false, 4 };
	animData[28] = { "die grab back", none, 1, false, false, false, false, 4 };
	animData[29] = { "die right side", none, 1, false, false, false, false, 4 };
	animData[30] = { "die left side", none, 1, false, false, false, false, 4 };
	animData[31] = { "die leg left", none, 1, false, false, false, false, 4 };
	animData[32] = { "die leg right", none, 1, false, false, false, false, 4 };
	animData[33] = { "die blown back", none, 1, false, false, false, false, 4 };
	animData[34] = { "die spin", none, 1, false, false, false, false, 4 };
	animData[35] = { "die forward", none, 1, false, false, false, false, 4 };
	animData[36] = { "die forward kneel", none, 1, false, false, false, false, 4 };
	animData[37] = { "die back", none, 1, false, false, false, false, 4 };

	// signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
	animData[39] = { "sign point", none, 1, true, false, false, false, 1 };
	animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
	animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
	animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

	// celebraton animations:
	animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

	// taunt anmations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

	// poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
	animData[50] = { "wave", none, 1, true, false, false, true, 1 };

	jetSound = NoSound;

	rFootSounds = 
	{
		SoundMFootRSoft,
		SoundMFootRHard,
		SoundMFootRSoft,
		SoundMFootRHard,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRHard,
		SoundMFootRSnow,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRSoft
	}; 
	lFootSounds =
	{
		SoundMFootLSoft,
		SoundMFootLHard,
		SoundMFootLSoft,
		SoundMFootLHard,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLHard,
		SoundMFootLSnow,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLSoft
	};

	footPrints = { 2, 3 };

	boxWidth = 0.7;
	boxDepth = 0.7;
	boxNormalHeight = 2.4;

	boxNormalHeadPercentage  = 0.83;
	boxNormalTorsoPercentage = 0.49;

	boxHeadLeftPercentage  = 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage  = 0;
	boxHeadFrontPercentage = 1;
};

//------------------------------------------------------------------
// Orc Armor data:	(medium)
//------------------------------------------------------------------

PlayerData OrcArmor
{
	className = "Armor";
	shapeFile = "marmor";
	flameShapeName = "mflame";
	shieldShapeName = "shield";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	shadowDetailMask = 1;

	canCrouch = false;
	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

	maxJetSideForceFactor = 1;
	maxJetForwardVelocity = 1.0;
	minJetEnergy = 60;
	jetForce = 1;
	jetEnergyDrain = 0.0;

	maxDamage = 1.0;
	maxForwardSpeed = $spdmed;
	maxBackwardSpeed = $spdmed * 0.8;
	maxSideSpeed = $spdmed * 0.75;
	groundForce = 75 * 9.0;
	mass = 9.0;
	groundTraction = 3.0;
	maxEnergy = 80;
	drag = 1.0;
	density = 1.2;

	minDamageSpeed = 16;
	damageScale = $damageScale;

	jumpImpulse = 75;
	jumpSurfaceMinDot = $jumpSurfaceMinDot;

	// animation data:
	// animation name, one shot, exclude, direction, firstPerson, chaseCam, thirdPerson, signalThread

	// movement animations:
	animData[0]  = { "root", none, 1, true, true, true, false, 0 };
	animData[1]  = { "run", none, 1, true, false, true, false, 3 };
	animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
	animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
	animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
	animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
	animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
	animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
	animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
	animData[19] = { "jet", none, 1, true, true, true, false, 3 };

	// misc. animations:
	animData[20] = { "die back", none, 1, true, false, false, false, 0 };
	animData[21] = { "throw", none, 1, true, false, false, false, 3 };
	animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
	animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
	animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
	// death animations:
	animData[25] = { "crouch die", none, 1, false, false, false, false, 4 };
	animData[26] = { "die chest", none, 1, false, false, false, false, 4 };
	animData[27] = { "die head", none, 1, false, false, false, false, 4 };
	animData[28] = { "die grab back", none, 1, false, false, false, false, 4 };
	animData[29] = { "die right side", none, 1, false, false, false, false, 4 };
	animData[30] = { "die left side", none, 1, false, false, false, false, 4 };
	animData[31] = { "die leg left", none, 1, false, false, false, false, 4 };
	animData[32] = { "die leg right", none, 1, false, false, false, false, 4 };
	animData[33] = { "die blown back", none, 1, false, false, false, false, 4 };
	animData[34] = { "die spin", none, 1, false, false, false, false, 4 };
	animData[35] = { "die forward", none, 1, false, false, false, false, 4 };
	animData[36] = { "die forward kneel", none, 1, false, false, false, false, 4 };
	animData[37] = { "die back", none, 1, false, false, false, false, 4 };

	// signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
	animData[39] = { "sign point", none, 1, true, false, false, false, 1 };
	animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
	animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
	animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

	// celebraton animations:
	animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

	// taunt anmations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

	// poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
	animData[50] = { "wave", none, 1, true, false, false, true, 1 };

	jetSound = NoSound;

	rFootSounds = 
	{
		SoundMFootRSoft,
		SoundMFootRHard,
		SoundMFootRSoft,
		SoundMFootRHard,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRHard,
		SoundMFootRSnow,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRSoft
	}; 
	lFootSounds =
	{
		SoundMFootLSoft,
		SoundMFootLHard,
		SoundMFootLSoft,
		SoundMFootLHard,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLHard,
		SoundMFootLSnow,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLSoft
	};

	footPrints = { 2, 3 };

	boxWidth = 0.7;
	boxDepth = 0.7;
	boxNormalHeight = 2.4;

	boxNormalHeadPercentage  = 0.83;
	boxNormalTorsoPercentage = 0.49;

	boxHeadLeftPercentage  = 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage  = 0;
	boxHeadFrontPercentage = 1;
};

//------------------------------------------------------------------
// Ogre Armor data:		(heavy)
//------------------------------------------------------------------

PlayerData OgreArmor
{
	className = "Armor";
	shapeFile = "harmor";
	flameShapeName = "hflame";
	shieldShapeName = "shield";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	shadowDetailMask = 1;

	canCrouch = false;
	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

	maxJetSideForceFactor = 1;
	maxJetForwardVelocity = 1.0;
	minJetEnergy = 60;
	jetForce = 1;
	jetEnergyDrain = 0.0;

	maxDamage = 1.0;
	maxForwardSpeed = $spdlow;
	maxBackwardSpeed = $spdlow * 0.8;
	maxSideSpeed = $spdlow * 0.75;
	groundForce = 75 * 9.0;
	mass = 9.0;
	groundTraction = 3.0;
	maxEnergy = 110;
	drag = 1.0;
	density = 1.2;

	minDamageSpeed = 16;
	damageScale = 0.006;

	jumpImpulse = 75;
	jumpSurfaceMinDot = $jumpSurfaceMinDot;
  
	// animation data:
	// animation name, one shot, exclude, direction, firstPerson, chaseCam, thirdPerson, signalThread

	// movement animations:
	animData[0]  = { "root", none, 1, true, true, true, false, 0 };
	animData[1]  = { "run", none, 1, true, false, true, false, 3 };
	animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
	animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
	animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
	animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
	animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
	animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
	animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
	animData[19] = { "jet", none, 1, true, true, true, false, 3 };

	// misc. animations:
	animData[20] = { "die back", none, 1, true, false, false, false, 0 };
	animData[21] = { "throw", none, 1, true, false, false, false, 3 };
	animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
	animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
	animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
	// death animations:
	animData[25] = { "crouch die", none, 1, false, false, false, false, 4 };
	animData[26] = { "die chest", none, 1, false, false, false, false, 4 };
	animData[27] = { "die head", none, 1, false, false, false, false, 4 };
	animData[28] = { "die grab back", none, 1, false, false, false, false, 4 };
	animData[29] = { "die right side", none, 1, false, false, false, false, 4 };
	animData[30] = { "die left side", none, 1, false, false, false, false, 4 };
	animData[31] = { "die leg left", none, 1, false, false, false, false, 4 };
	animData[32] = { "die leg right", none, 1, false, false, false, false, 4 };
	animData[33] = { "die blown back", none, 1, false, false, false, false, 4 };
	animData[34] = { "die spin", none, 1, false, false, false, false, 4 };
	animData[35] = { "die forward", none, 1, false, false, false, false, 4 };
	animData[36] = { "die forward kneel", none, 1, false, false, false, false, 4 };
	animData[37] = { "die back", none, 1, false, false, false, false, 4 };

	// signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
	animData[39] = { "sign point", none, 1, true, false, false, false, 1 };
	animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
	animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
	animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

	// celebraton animations:
	animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

	// taunt anmations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

	// poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
	animData[50] = { "wave", none, 1, true, false, false, true, 1 };

	jetSound = NoSound;

	rFootSounds = 
	{
		SoundHFootRSoft,
		SoundHFootRHard,
		SoundHFootRSoft,
		SoundHFootRHard,
		SoundHFootRSoft,
		SoundHFootRSoft,
		SoundHFootRSoft,
		SoundHFootRHard,
		SoundHFootRSnow,
		SoundHFootRSoft,
		SoundHFootRSoft,
		SoundHFootRSoft,
		SoundHFootRSoft,
		SoundHFootRSoft,
		SoundHFootRSoft
	}; 
	lFootSounds =
	{
		SoundHFootLSoft,
		SoundHFootLHard,
		SoundHFootLSoft,
		SoundHFootLHard,
		SoundHFootLSoft,
		SoundHFootLSoft,
		SoundHFootLSoft,
		SoundHFootLHard,
		SoundHFootLSnow,
		SoundHFootLSoft,
		SoundHFootLSoft,
		SoundHFootLSoft,
		SoundHFootLSoft,
		SoundHFootLSoft,
		SoundHFootLSoft
	};

	footPrints = { 4, 5 };

	boxWidth = 0.8;
	boxDepth = 0.8;
	boxNormalHeight = 2.6;

	boxNormalHeadPercentage  = 0.70;
	boxNormalTorsoPercentage = 0.45;

	boxHeadLeftPercentage  = 0.48;
	boxHeadRightPercentage = 0.70;
	boxHeadBackPercentage  = 0.48;
	boxHeadFrontPercentage = 0.60;
};

//------------------------------------------------------------------
// MaleElf armor data:	(light)
//------------------------------------------------------------------

PlayerData MaleElfArmor
{
	className = "Armor";
	shapeFile = "rpgmalehuman";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	flameShapeName = "lflame";
	shieldShapeName = "shield";
	shadowDetailMask = 1;

	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
	canCrouch = false;

	maxJetSideForceFactor = 1;
	maxJetForwardVelocity = 1.0;
	minJetEnergy = 60;
	jetForce = 1;
	jetEnergyDrain = 0.0;

	maxDamage = 1.0;
	maxForwardSpeed = $spdmed;
	maxBackwardSpeed = $spdmed * 0.8;
	maxSideSpeed = $spdmed * 0.75;

	groundForce = 75 * 9.0;
	mass = 9.0;
	groundTraction = 3.0;
	maxEnergy = 60;
	drag = 1.0;
	density = 1.2;

	minDamageSpeed = 16;
	damageScale = $damageScale;

	jumpImpulse = 75;
	jumpSurfaceMinDot = $jumpSurfaceMinDot;

	// animation data:
	// animation name, one shot, direction, firstPerson, chaseCam, thirdPerson, signalThread
	// movement animations:
	animData[0]  = { "root", none, 1, true, true, true, false, 0 };
	animData[1]  = { "run", none, 1, true, false, true, false, 3 };
	animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
	animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
	animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
	animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
	animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
	animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
	animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
	animData[19] = { "jet", none, 1, true, true, true, false, 3 };

	// misc. animations:
	animData[20] = { "die back", none, 1, true, false, false, false, 0 };
	animData[21] = { "throw", none, 1, true, false, false, false, 3 };
	animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
	animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
	animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
	// death animations:
	animData[25] = { "crouch die", none, 1, false, false, false, false, 4 };
	animData[26] = { "die chest", none, 1, false, false, false, false, 4 };
	animData[27] = { "die head", none, 1, false, false, false, false, 4 };
	animData[28] = { "die grab back", none, 1, false, false, false, false, 4 };
	animData[29] = { "die right side", none, 1, false, false, false, false, 4 };
	animData[30] = { "die left side", none, 1, false, false, false, false, 4 };
	animData[31] = { "die leg left", none, 1, false, false, false, false, 4 };
	animData[32] = { "die leg right", none, 1, false, false, false, false, 4 };
	animData[33] = { "die blown back", none, 1, false, false, false, false, 4 };
	animData[34] = { "die spin", none, 1, false, false, false, false, 4 };
	animData[35] = { "die forward", none, 1, false, false, false, false, 4 };
	animData[36] = { "die forward kneel", none, 1, false, false, false, false, 4 };
	animData[37] = { "die back", none, 1, false, false, false, false, 4 };

	// signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
	animData[39] = { "sign point", none, 1, true, false, false, false, 1 };
	animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
	animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
	animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

	// celebration animations:
	animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
	// taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
	// poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
	animData[50] = { "wave", none, 1, true, false, false, true, 1 };

	jetSound = NoSound;
	rFootSounds = 
	{
		SoundLFootRSoft,
		SoundLFootRHard,
		SoundLFootRSoft,
		SoundLFootRHard,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRHard,
		SoundLFootRSnow,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft
	}; 
	lFootSounds =
	{
		SoundLFootLSoft,
		SoundLFootLHard,
		SoundLFootLSoft,
		SoundLFootLHard,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLHard,
		SoundLFootLSnow,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft
	};

	footPrints = { 0, 1 };

	boxWidth = 0.5;
	boxDepth = 0.5;
	boxNormalHeight = 2.3;
	boxCrouchHeight = 1.8;

	boxNormalHeadPercentage  = 0.83;
	boxNormalTorsoPercentage = 0.53;
	boxCrouchHeadPercentage  = 0.6666;
	boxCrouchTorsoPercentage = 0.3333;

	boxHeadLeftPercentage  = 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage  = 0;
	boxHeadFrontPercentage = 1;
};

//------------------------------------------------------------------
// FemaleElf armor data:	(light)
//------------------------------------------------------------------

PlayerData FemaleElfArmor
{
	className = "Armor";
	shapeFile = "lfemalehuman";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	flameShapeName = "lflame";
	shieldShapeName = "shield";
	shadowDetailMask = 1;

	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
	canCrouch = false;

	maxJetSideForceFactor = 1;
	maxJetForwardVelocity = 1.0;
	minJetEnergy = 60;
	jetForce = 1;
	jetEnergyDrain = 0.0;

	maxDamage = 1.0;
	maxForwardSpeed = $spdmed;
	maxBackwardSpeed = $spdmed * 0.8;
	maxSideSpeed = $spdmed * 0.75;

	groundForce = 75 * 9.0;
	mass = 9.0;
	groundTraction = 3.0;
	maxEnergy = 60;
	drag = 1.0;
	density = 1.2;

	minDamageSpeed = 16;
	damageScale = $damageScale;

	jumpImpulse = 75;
	jumpSurfaceMinDot = $jumpSurfaceMinDot;

	// animation data:
	// animation name, one shot, direction, firstPerson, chaseCam, thirdPerson, signalThread
	// movement animations:
	animData[0]  = { "root", none, 1, true, true, true, false, 0 };
	animData[1]  = { "run", none, 1, true, false, true, false, 3 };
	animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
	animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
	animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
	animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
	animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
	animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
	animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
	animData[19] = { "jet", none, 1, true, true, true, false, 3 };

	// misc. animations:
	animData[20] = { "die back", none, 1, true, false, false, false, 0 };
	animData[21] = { "throw", none, 1, true, false, false, false, 3 };
	animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
	animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
	animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
	// death animations:
	animData[25] = { "crouch die", none, 1, false, false, false, false, 4 };
	animData[26] = { "die chest", none, 1, false, false, false, false, 4 };
	animData[27] = { "die head", none, 1, false, false, false, false, 4 };
	animData[28] = { "die grab back", none, 1, false, false, false, false, 4 };
	animData[29] = { "die right side", none, 1, false, false, false, false, 4 };
	animData[30] = { "die left side", none, 1, false, false, false, false, 4 };
	animData[31] = { "die leg left", none, 1, false, false, false, false, 4 };
	animData[32] = { "die leg right", none, 1, false, false, false, false, 4 };
	animData[33] = { "die blown back", none, 1, false, false, false, false, 4 };
	animData[34] = { "die spin", none, 1, false, false, false, false, 4 };
	animData[35] = { "die forward", none, 1, false, false, false, false, 4 };
	animData[36] = { "die forward kneel", none, 1, false, false, false, false, 4 };
	animData[37] = { "die back", none, 1, false, false, false, false, 4 };

	// signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
	animData[39] = { "sign point", none, 1, true, false, false, false, 1 };
	animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
	animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
	animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

	// celebration animations:
	animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
	// taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
	// poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
	animData[50] = { "wave", none, 1, true, false, false, true, 1 };

	jetSound = NoSound;
	rFootSounds = 
	{
		SoundLFootRSoft,
		SoundLFootRHard,
		SoundLFootRSoft,
		SoundLFootRHard,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRHard,
		SoundLFootRSnow,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft
	}; 
	lFootSounds =
	{
		SoundLFootLSoft,
		SoundLFootLHard,
		SoundLFootLSoft,
		SoundLFootLHard,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLHard,
		SoundLFootLSnow,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft
	};

	footPrints = { 0, 1 };

	boxWidth = 0.5;
	boxDepth = 0.5;
	boxNormalHeight = 2.3;
	boxCrouchHeight = 1.8;

	boxNormalHeadPercentage  = 0.83;
	boxNormalTorsoPercentage = 0.53;
	boxCrouchHeadPercentage  = 0.6666;
	boxCrouchTorsoPercentage = 0.3333;

	boxHeadLeftPercentage  = 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage  = 0;
	boxHeadFrontPercentage = 1;
};

//------------------------------------------------------------------
// Minotaur Armor data:	(minotaur player model)
//------------------------------------------------------------------

PlayerData MinotaurArmor
{
	className = "Armor";
	shapeFile = "min";
	flameShapeName = "mflame";
	shieldShapeName = "shield";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	shadowDetailMask = 1;

	canCrouch = false;
	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

	maxJetSideForceFactor = 1;
	maxJetForwardVelocity = 1.0;
	minJetEnergy = 60;
	jetForce = 1;
	jetEnergyDrain = 0.0;

	maxDamage = 1.0;
	maxForwardSpeed = $spdlowmed;
	maxBackwardSpeed = $spdlowmed * 0.8;
	maxSideSpeed = $spdlowmed * 0.75;
	groundForce = 75 * 9.0;
	mass = 9.0;
	groundTraction = 3.0;
	maxEnergy = 80;
	drag = 1.0;
	density = 1.2;

	minDamageSpeed = 16;
	damageScale = $damageScale;

	jumpImpulse = 75;
	jumpSurfaceMinDot = $jumpSurfaceMinDot;

	// animation data:
	// animation name, one shot, exclude, direction, firstPerson, chaseCam, thirdPerson, signalThread

	// movement animations:
	animData[0]  = { "root", none, 1, true, true, true, false, 0 };
	animData[1]  = { "run", none, 1, true, false, true, false, 3 };
	animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
	animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
	animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
	animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
	animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
	animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
	animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
	animData[19] = { "jet", none, 1, true, true, true, false, 3 };

	// misc. animations:
	animData[20] = { "die back", none, 1, true, false, false, false, 0 };
	animData[21] = { "throw", none, 1, true, false, false, false, 3 };
	animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
	animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
	animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
	// death animations:
	animData[25] = { "crouch die", none, 1, false, false, false, false, 4 };
	animData[26] = { "die chest", none, 1, false, false, false, false, 4 };
	animData[27] = { "die head", none, 1, false, false, false, false, 4 };
	animData[28] = { "die grab back", none, 1, false, false, false, false, 4 };
	animData[29] = { "die right side", none, 1, false, false, false, false, 4 };
	animData[30] = { "die left side", none, 1, false, false, false, false, 4 };
	animData[31] = { "die leg left", none, 1, false, false, false, false, 4 };
	animData[32] = { "die leg right", none, 1, false, false, false, false, 4 };
	animData[33] = { "die blown back", none, 1, false, false, false, false, 4 };
	animData[34] = { "die spin", none, 1, false, false, false, false, 4 };
	animData[35] = { "die forward", none, 1, false, false, false, false, 4 };
	animData[36] = { "die forward kneel", none, 1, false, false, false, false, 4 };
	animData[37] = { "die back", none, 1, false, false, false, false, 4 };

	// signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
	animData[39] = { "sign point", none, 1, true, false, false, false, 1 };
	animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
	animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
	animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

	// celebraton animations:
	animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

	// taunt anmations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

	// poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
	animData[50] = { "wave", none, 1, true, false, false, true, 1 };

	jetSound = NoSound;

	rFootSounds = 
	{
		SoundMFootRSoft,
		SoundMFootRHard,
		SoundMFootRSoft,
		SoundMFootRHard,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRHard,
		SoundMFootRSnow,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRSoft
	}; 
	lFootSounds =
	{
		SoundMFootLSoft,
		SoundMFootLHard,
		SoundMFootLSoft,
		SoundMFootLHard,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLHard,
		SoundMFootLSnow,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLSoft
	};

	footPrints = { 2, 3 };

	boxWidth = 0.7;
	boxDepth = 0.7;
	boxNormalHeight = 2.4;

	boxNormalHeadPercentage  = 0.83;
	boxNormalTorsoPercentage = 0.49;

	boxHeadLeftPercentage  = 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage  = 0;
	boxHeadFrontPercentage = 1;
};

//------------------------------------------------------------------
// Uber Armor data:		(fedmonster player model)
//------------------------------------------------------------------

PlayerData UberArmor
{
	className = "Armor";
	shapeFile = "fedmonster";
	flameShapeName = "hflame";
	shieldShapeName = "shield";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	shadowDetailMask = 1;

	canCrouch = false;
	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

	maxJetSideForceFactor = 1;
	maxJetForwardVelocity = 1.0;
	minJetEnergy = 60;
	jetForce = 1;
	jetEnergyDrain = 0.0;

	maxDamage = 1.0;
	maxForwardSpeed = $spdfast;
	maxBackwardSpeed = $spdfast * 0.8;
	maxSideSpeed = $spdfast * 0.75;
	groundForce = 75 * 9.0;
	mass = 9.0;
	groundTraction = 3.0;
	maxEnergy = 110;
	drag = 1.0;
	density = 1.2;

	minDamageSpeed = 16;
	damageScale = 0.006;

	jumpImpulse = 75;
	jumpSurfaceMinDot = $jumpSurfaceMinDot;
  
	// animation data:
	// animation name, one shot, exclude, direction, firstPerson, chaseCam, thirdPerson, signalThread

	// movement animations:
	animData[0]  = { "root", none, 1, true, true, true, false, 0 };
	animData[1]  = { "run", none, 1, true, false, true, false, 3 };
	animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
	animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
	animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
	animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
	animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
	animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
	animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
	animData[19] = { "jet", none, 1, true, true, true, false, 3 };

	// misc. animations:
	animData[20] = { "die back", none, 1, true, false, false, false, 0 };
	animData[21] = { "throw", none, 1, true, false, false, false, 3 };
	animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
	animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
	animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
	// death animations:
	animData[25] = { "crouch die", none, 1, false, false, false, false, 4 };
	animData[26] = { "die chest", none, 1, false, false, false, false, 4 };
	animData[27] = { "die head", none, 1, false, false, false, false, 4 };
	animData[28] = { "die grab back", none, 1, false, false, false, false, 4 };
	animData[29] = { "die right side", none, 1, false, false, false, false, 4 };
	animData[30] = { "die left side", none, 1, false, false, false, false, 4 };
	animData[31] = { "die leg left", none, 1, false, false, false, false, 4 };
	animData[32] = { "die leg right", none, 1, false, false, false, false, 4 };
	animData[33] = { "die blown back", none, 1, false, false, false, false, 4 };
	animData[34] = { "die spin", none, 1, false, false, false, false, 4 };
	animData[35] = { "die forward", none, 1, false, false, false, false, 4 };
	animData[36] = { "die forward kneel", none, 1, false, false, false, false, 4 };
	animData[37] = { "die back", none, 1, false, false, false, false, 4 };

	// signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
	animData[39] = { "sign point", none, 1, true, false, false, false, 1 };
	animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
	animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
	animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

	// celebraton animations:
	animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

	// taunt anmations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

	// poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
	animData[50] = { "wave", none, 1, true, false, false, true, 1 };

	jetSound = NoSound;

	rFootSounds = 
	{
		SoundHFootRSoft,
		SoundHFootRHard,
		SoundHFootRSoft,
		SoundHFootRHard,
		SoundHFootRSoft,
		SoundHFootRSoft,
		SoundHFootRSoft,
		SoundHFootRHard,
		SoundHFootRSnow,
		SoundHFootRSoft,
		SoundHFootRSoft,
		SoundHFootRSoft,
		SoundHFootRSoft,
		SoundHFootRSoft,
		SoundHFootRSoft
	}; 
	lFootSounds =
	{
		SoundHFootLSoft,
		SoundHFootLHard,
		SoundHFootLSoft,
		SoundHFootLHard,
		SoundHFootLSoft,
		SoundHFootLSoft,
		SoundHFootLSoft,
		SoundHFootLHard,
		SoundHFootLSnow,
		SoundHFootLSoft,
		SoundHFootLSoft,
		SoundHFootLSoft,
		SoundHFootLSoft,
		SoundHFootLSoft,
		SoundHFootLSoft
	};

	footPrints = { 4, 5 };

	boxWidth = 0.8;
	boxDepth = 0.8;
	boxNormalHeight = 2.6;

	boxNormalHeadPercentage  = 0.70;
	boxNormalTorsoPercentage = 0.45;

	boxHeadLeftPercentage  = 0.48;
	boxHeadRightPercentage = 0.70;
	boxHeadBackPercentage  = 0.48;
	boxHeadFrontPercentage = 0.60;
};

//------------------------------------------------------------------
// Zombie armor data:	(light)
//------------------------------------------------------------------

PlayerData ZombieArmor
{
	className = "Armor";
	shapeFile = "zombie";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	flameShapeName = "lflame";
	shieldShapeName = "shield";
	shadowDetailMask = 1;

	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
	canCrouch = false;

	maxJetSideForceFactor = 1;
	maxJetForwardVelocity = 1.0;
	minJetEnergy = 60;
	jetForce = 1;
	jetEnergyDrain = 0.0;

	maxDamage = 1.0;
	maxForwardSpeed = $spdlowmed;
	maxBackwardSpeed = $spdlowmed * 0.8;
	maxSideSpeed = $spdlowmed * 0.75;

	groundForce = 75 * 9.0;
	mass = 9.0;
	groundTraction = 3.0;
	maxEnergy = 60;
	drag = 1.0;
	density = 1.2;

	minDamageSpeed = 16;
	damageScale = $damageScale;

	jumpImpulse = 75;
	jumpSurfaceMinDot = $jumpSurfaceMinDot;

	// animation data:
	// animation name, one shot, direction, firstPerson, chaseCam, thirdPerson, signalThread
	// movement animations:
	animData[0]  = { "root", none, 1, true, true, true, false, 0 };
	animData[1]  = { "run", none, 1, true, false, true, false, 3 };
	animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
	animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
	animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
	animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
	animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
	animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
	animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
	animData[19] = { "jet", none, 1, true, true, true, false, 3 };

	// misc. animations:
	animData[20] = { "die back", none, 1, true, false, false, false, 0 };
	animData[21] = { "throw", none, 1, true, false, false, false, 3 };
	animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
	animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
	animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
	// death animations:
	animData[25] = { "crouch die", none, 1, false, false, false, false, 4 };
	animData[26] = { "die chest", none, 1, false, false, false, false, 4 };
	animData[27] = { "die head", none, 1, false, false, false, false, 4 };
	animData[28] = { "die grab back", none, 1, false, false, false, false, 4 };
	animData[29] = { "die right side", none, 1, false, false, false, false, 4 };
	animData[30] = { "die left side", none, 1, false, false, false, false, 4 };
	animData[31] = { "die leg left", none, 1, false, false, false, false, 4 };
	animData[32] = { "die leg right", none, 1, false, false, false, false, 4 };
	animData[33] = { "die blown back", none, 1, false, false, false, false, 4 };
	animData[34] = { "die spin", none, 1, false, false, false, false, 4 };
	animData[35] = { "die forward", none, 1, false, false, false, false, 4 };
	animData[36] = { "die forward kneel", none, 1, false, false, false, false, 4 };
	animData[37] = { "die back", none, 1, false, false, false, false, 4 };

	// signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
	animData[39] = { "sign point", none, 1, true, false, false, false, 1 };
	animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
	animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
	animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

	// celebration animations:
	animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
	// taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
	// poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
	animData[50] = { "wave", none, 1, true, false, false, true, 1 };

	jetSound = NoSound;
	rFootSounds = 
	{
		SoundLFootRSoft,
		SoundLFootRHard,
		SoundLFootRSoft,
		SoundLFootRHard,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRHard,
		SoundLFootRSnow,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft
	}; 
	lFootSounds =
	{
		SoundLFootLSoft,
		SoundLFootLHard,
		SoundLFootLSoft,
		SoundLFootLHard,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLHard,
		SoundLFootLSnow,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft
	};

	footPrints = { 0, 1 };

	boxWidth = 0.5;
	boxDepth = 0.5;
	boxNormalHeight = 2.3;
	boxCrouchHeight = 1.8;

	boxNormalHeadPercentage  = 0.83;
	boxNormalTorsoPercentage = 0.53;
	boxCrouchHeadPercentage  = 0.6666;
	boxCrouchTorsoPercentage = 0.3333;

	boxHeadLeftPercentage  = 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage  = 0;
	boxHeadFrontPercentage = 1;
};

//------------------------------------------------------------------
// Undead armor data:	(light)
//------------------------------------------------------------------

PlayerData UndeadArmor
{
	className = "Armor";
	shapeFile = "skel";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	flameShapeName = "lflame";
	shieldShapeName = "shield";
	shadowDetailMask = 1;

	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
	canCrouch = false;

	maxJetSideForceFactor = 1;
	maxJetForwardVelocity = 1.0;
	minJetEnergy = 60;
	jetForce = 1;
	jetEnergyDrain = 0.0;

	maxDamage = 1.0;
	maxForwardSpeed = $spdmed;
	maxBackwardSpeed = $spdmed * 0.8;
	maxSideSpeed = $spdmed * 0.75;

	groundForce = 75 * 9.0;
	mass = 9.0;
	groundTraction = 3.0;
	maxEnergy = 60;
	drag = 1.0;
	density = 1.2;

	minDamageSpeed = 16;
	damageScale = $damageScale;

	jumpImpulse = 75;
	jumpSurfaceMinDot = $jumpSurfaceMinDot;

	// animation data:
	// animation name, one shot, direction, firstPerson, chaseCam, thirdPerson, signalThread
	// movement animations:
	animData[0]  = { "root", none, 1, true, true, true, false, 0 };
	animData[1]  = { "run", none, 1, true, false, true, false, 3 };
	animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
	animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
	animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
	animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
	animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
	animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
	animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
	animData[19] = { "jet", none, 1, true, true, true, false, 3 };

	// misc. animations:
	animData[20] = { "die back", none, 1, true, false, false, false, 0 };
	animData[21] = { "throw", none, 1, true, false, false, false, 3 };
	animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
	animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
	animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
	// death animations:
	animData[25] = { "crouch die", none, 1, false, false, false, false, 4 };
	animData[26] = { "die chest", none, 1, false, false, false, false, 4 };
	animData[27] = { "die head", none, 1, false, false, false, false, 4 };
	animData[28] = { "die grab back", none, 1, false, false, false, false, 4 };
	animData[29] = { "die right side", none, 1, false, false, false, false, 4 };
	animData[30] = { "die left side", none, 1, false, false, false, false, 4 };
	animData[31] = { "die leg left", none, 1, false, false, false, false, 4 };
	animData[32] = { "die leg right", none, 1, false, false, false, false, 4 };
	animData[33] = { "die blown back", none, 1, false, false, false, false, 4 };
	animData[34] = { "die spin", none, 1, false, false, false, false, 4 };
	animData[35] = { "die forward", none, 1, false, false, false, false, 4 };
	animData[36] = { "die forward kneel", none, 1, false, false, false, false, 4 };
	animData[37] = { "die back", none, 1, false, false, false, false, 4 };

	// signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
	animData[39] = { "sign point", none, 1, true, false, false, false, 1 };
	animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
	animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
	animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

	// celebration animations:
	animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
	// taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
	// poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
	animData[50] = { "wave", none, 1, true, false, false, true, 1 };

	jetSound = NoSound;
	rFootSounds = 
	{
		SoundLFootRSoft,
		SoundLFootRHard,
		SoundLFootRSoft,
		SoundLFootRHard,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRHard,
		SoundLFootRSnow,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft,
		SoundLFootRSoft
	}; 
	lFootSounds =
	{
		SoundLFootLSoft,
		SoundLFootLHard,
		SoundLFootLSoft,
		SoundLFootLHard,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLHard,
		SoundLFootLSnow,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft,
		SoundLFootLSoft
	};

	footPrints = { 0, 1 };

	boxWidth = 0.5;
	boxDepth = 0.5;
	boxNormalHeight = 2.3;
	boxCrouchHeight = 1.8;

	boxNormalHeadPercentage  = 0.83;
	boxNormalTorsoPercentage = 0.53;
	boxCrouchHeadPercentage  = 0.6666;
	boxCrouchTorsoPercentage = 0.3333;

	boxHeadLeftPercentage  = 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage  = 0;
	boxHeadFrontPercentage = 1;
};

//------------------------------------------------------------------
// Demon Armor data:	(minotaur player model)
//------------------------------------------------------------------

PlayerData DemonArmor
{
	className = "Armor";
	shapeFile = "min";
	flameShapeName = "mflame";
	shieldShapeName = "shield";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	shadowDetailMask = 1;

	canCrouch = false;
	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

	maxJetSideForceFactor = 1;
	maxJetForwardVelocity = 1.0;
	minJetEnergy = 60;
	jetForce = 1;
	jetEnergyDrain = 0.0;

	maxDamage = 1.0;
	maxForwardSpeed = $spdmed;
	maxBackwardSpeed = $spdmed * 0.8;
	maxSideSpeed = $spdmed * 0.75;
	groundForce = 75 * 9.0;
	mass = 9.0;
	groundTraction = 3.0;
	maxEnergy = 80;
	drag = 1.0;
	density = 1.2;

	minDamageSpeed = 16;
	damageScale = $damageScale;

	jumpImpulse = 75;
	jumpSurfaceMinDot = $jumpSurfaceMinDot;

	// animation data:
	// animation name, one shot, exclude, direction, firstPerson, chaseCam, thirdPerson, signalThread

	// movement animations:
	animData[0]  = { "root", none, 1, true, true, true, false, 0 };
	animData[1]  = { "run", none, 1, true, false, true, false, 3 };
	animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
	animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
	animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
	animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
	animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
	animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
	animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
	animData[19] = { "jet", none, 1, true, true, true, false, 3 };

	// misc. animations:
	animData[20] = { "die back", none, 1, true, false, false, false, 0 };
	animData[21] = { "throw", none, 1, true, false, false, false, 3 };
	animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
	animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
	animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
	// death animations:
	animData[25] = { "crouch die", none, 1, false, false, false, false, 4 };
	animData[26] = { "die chest", none, 1, false, false, false, false, 4 };
	animData[27] = { "die head", none, 1, false, false, false, false, 4 };
	animData[28] = { "die grab back", none, 1, false, false, false, false, 4 };
	animData[29] = { "die right side", none, 1, false, false, false, false, 4 };
	animData[30] = { "die left side", none, 1, false, false, false, false, 4 };
	animData[31] = { "die leg left", none, 1, false, false, false, false, 4 };
	animData[32] = { "die leg right", none, 1, false, false, false, false, 4 };
	animData[33] = { "die blown back", none, 1, false, false, false, false, 4 };
	animData[34] = { "die spin", none, 1, false, false, false, false, 4 };
	animData[35] = { "die forward", none, 1, false, false, false, false, 4 };
	animData[36] = { "die forward kneel", none, 1, false, false, false, false, 4 };
	animData[37] = { "die back", none, 1, false, false, false, false, 4 };

	// signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
	animData[39] = { "sign point", none, 1, true, false, false, false, 1 };
	animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
	animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
	animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

	// celebraton animations:
	animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

	// taunt anmations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

	// poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
	animData[50] = { "wave", none, 1, true, false, false, true, 1 };

	jetSound = NoSound;

	rFootSounds = 
	{
		SoundMFootRSoft,
		SoundMFootRHard,
		SoundMFootRSoft,
		SoundMFootRHard,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRHard,
		SoundMFootRSnow,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRSoft,
		SoundMFootRSoft
	}; 
	lFootSounds =
	{
		SoundMFootLSoft,
		SoundMFootLHard,
		SoundMFootLSoft,
		SoundMFootLHard,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLHard,
		SoundMFootLSnow,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLSoft,
		SoundMFootLSoft
	};

	footPrints = { 2, 3 };

	boxWidth = 0.7;
	boxDepth = 0.7;
	boxNormalHeight = 2.4;

	boxNormalHeadPercentage  = 0.83;
	boxNormalTorsoPercentage = 0.49;

	boxHeadLeftPercentage  = 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage  = 0;
	boxHeadFrontPercentage = 1;
};
