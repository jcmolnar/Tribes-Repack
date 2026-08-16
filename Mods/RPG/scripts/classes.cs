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


//There are FOUR hard-coded groups:
//-Priest
//-Rogue
//-Warrior
//-Wizard

//Each of these has classes.  They are specified in here.
//Anything that does NOT have to do with visuals when it comes to classes should ALWAYS use the 0 offset in $ClassName.

$initcoins[Priest] = "3d6x10";
$initcoins[Rogue] = "2d6x10";
$initcoins[Warrior] = "5d4x10";
$initcoins[Wizard] = "1d4+1x10";

$MinHP[MaleHuman] = 12;
$MinHP[FemaleHuman] = 12;
$MinHP[Traveller] = 12;
$MinHP[Goblin] = 0;
$MinHP[Orc] = 7;
$MinHP[Ogre] = 10;
$MinHP[Gnoll] = 3;
$MinHP[Undead] = 11;
$MinHP[Elf] = 10;
$MinHP[Minotaur] = 15;
$MinHP[Uber] = 25;
$MinHP[DeathKnight] = 5000;
$MinHP[DeathKnight] = 12;

$ClassName[1, 0] = "Cleric";
$ClassName[2, 0] = "Druid";
$ClassName[3, 0] = "Thief";
$ClassName[4, 0] = "Bard";
$ClassName[5, 0] = "Fighter";
$ClassName[6, 0] = "Paladin";
$ClassName[7, 0] = "Ranger";
$ClassName[8, 0] = "Mage";

$ClassGroup[Cleric] = "Priest";
$ClassGroup[Druid] = "Priest";
$ClassGroup[Thief] = "Rogue";
$ClassGroup[Bard] = "Rogue";
$ClassGroup[Fighter] = "Warrior";
$ClassGroup[Paladin] = "Warrior";
$ClassGroup[Ranger] = "Warrior";
$ClassGroup[Mage] = "Wizard";

//===================================
// REMORTS
//===================================
$ClassName[1, 1] = "Bishop";
$ClassName[2, 1] = "Archdruid";
$ClassName[3, 1] = "Bandit";
$ClassName[4, 1] = "Lyricist";
$ClassName[5, 1] = "Berzerker";
$ClassName[6, 1] = "Avenger";
$ClassName[7, 1] = "Woodsman";
$ClassName[8, 1] = "Archmage";

$ClassName[1, 2] = "Saint";
$ClassName[2, 2] = "Mystic";
$ClassName[3, 2] = "Outlaw";
$ClassName[4, 2] = "Musician";
$ClassName[5, 2] = "Knight";
$ClassName[6, 2] = "Centurian";
$ClassName[7, 2] = "Marksman";
$ClassName[8, 2] = "Sorcerer";

$ClassName[1, 3] = "Pope";
$ClassName[2, 3] = "Head Mystic";
$ClassName[3, 3] = "Klepto";
$ClassName[4, 3] = "Producer";
$ClassName[5, 3] = "Guardian";
$ClassName[6, 3] = "Crusader";
$ClassName[7, 3] = "Archer";
$ClassName[8, 3] = "Wizard";

$ClassName[1, 4]  = "PopeIV";
$ClassName[2, 4]  = "Head MysticIV";
$ClassName[3, 4]  = "KleptoIV";
$ClassName[4, 4]  = "ArtisteIV";
$ClassName[5, 4]  = "GuardianIV";
$ClassName[6, 4]  = "CrusaderIV";
$ClassName[7, 4]  = "ArcherIV";
$ClassName[8, 4]  = "WizardIV";

$ClassName[1, 5]  = "PopeV";
$ClassName[2, 5]  = "Head MysticV";
$ClassName[3, 5]  = "KleptoV";
$ClassName[4, 5]  = "ArtisteV";
$ClassName[5, 5]  = "GuardianV";
$ClassName[6, 5]  = "CrusaderV";
$ClassName[7, 5]  = "ArcherV";
$ClassName[8, 5]  = "WizardV";

$ClassName[1, 6]  = "PopeVI";
$ClassName[2, 6]  = "Head MysticVI";
$ClassName[3, 6]  = "KleptoVI";
$ClassName[4, 6]  = "ArtisteVI";
$ClassName[5, 6]  = "GuardianVI";
$ClassName[6, 6]  = "CrusaderVI";
$ClassName[7, 6]  = "ArcherVI";
$ClassName[8, 6]  = "WizardVI";

$ClassName[1, 7]  = "PopeVII";
$ClassName[2, 7]  = "Head MysticVII";
$ClassName[3, 7]  = "KleptoVII";
$ClassName[4, 7]  = "ArtisteVII";
$ClassName[5, 7]  = "GuardianVII";
$ClassName[6, 7]  = "CrusaderVII";
$ClassName[7, 7]  = "ArcherVII";
$ClassName[8, 7]  = "WizardVII";

$ClassName[1, 8]  = "PopeVIII";
$ClassName[2, 8]  = "Head MysticVIII";
$ClassName[3, 8]  = "KleptoVIII";
$ClassName[4, 8]  = "ArtisteVIII";
$ClassName[5, 8]  = "GuardianVIII";
$ClassName[6, 8]  = "CrusaderVIII";
$ClassName[7, 8]  = "ArcherVIII";
$ClassName[8, 8]  = "WizardVIII";

$ClassName[1, 9]  = "PopeIX";
$ClassName[2, 9]  = "Head MysticIX";
$ClassName[3, 9]  = "KleptoIX";
$ClassName[4, 9]  = "ArtisteIX";
$ClassName[5, 9]  = "GuardianIX";
$ClassName[6, 9]  = "CrusaderIX";
$ClassName[7, 9]  = "ArcherIX";
$ClassName[8, 9]  = "WizardIX";

$ClassName[1, 10]  = "PopeX";
$ClassName[2, 10]  = "Head MysticX";
$ClassName[3, 10]  = "KleptoX";
$ClassName[4, 10]  = "ArtisteX";
$ClassName[5, 10]  = "GuardianX";
$ClassName[6, 10]  = "CrusaderX";
$ClassName[7, 10]  = "ArcherX";
$ClassName[8, 10]  = "WizardX";

$ClassName[1, 11]  = "PopeXI";
$ClassName[2, 11]  = "Head MysticXI";
$ClassName[3, 11]  = "KleptoXI";
$ClassName[4, 11]  = "ArtisteXI";
$ClassName[5, 11]  = "GuardianXI";
$ClassName[6, 11]  = "CrusaderXI";
$ClassName[7, 11]  = "ArcherXI";
$ClassName[8, 11]  = "WizardXI";

$ClassName[1, 12]  = "PopeXII";
$ClassName[2, 12]  = "Head MysticXII";
$ClassName[3, 12]  = "KleptoXII";
$ClassName[4, 12]  = "ArtisteXII";
$ClassName[5, 12]  = "GuardianXII";
$ClassName[6, 12]  = "CrusaderXII";
$ClassName[7, 12]  = "ArcherXII";
$ClassName[8, 12]  = "WizardXII";

$ClassName[1, 13]  = "PopeXIII";
$ClassName[2, 13]  = "Head MysticXIII";
$ClassName[3, 13]  = "KleptoXIII";
$ClassName[4, 13]  = "ArtisteXIII";
$ClassName[5, 13]  = "GuardianXIII";
$ClassName[6, 13]  = "CrusaderXIII";
$ClassName[7, 13]  = "ArcherXIII";
$ClassName[8, 13]  = "WizardXIII";

$ClassName[1, 14]  = "PopeXIV";
$ClassName[2, 14]  = "Head MysticXIV";
$ClassName[3, 14]  = "KleptoXIV";
$ClassName[4, 14]  = "ArtisteXIV";
$ClassName[5, 14]  = "GuardianXIV";
$ClassName[6, 14]  = "CrusaderXIV";
$ClassName[7, 14]  = "ArcherXIV";
$ClassName[8, 14]  = "WizardXIV";

$ClassName[1, 15]  = "PopeXV";
$ClassName[2, 15]  = "Head MysticXV";
$ClassName[3, 15]  = "KleptoXV";
$ClassName[4, 15]  = "ArtisteXV";
$ClassName[5, 15]  = "GuardianXV";
$ClassName[6, 15]  = "CrusaderXV";
$ClassName[7, 15]  = "ArcherXV";
$ClassName[8, 15]  = "WizardXV";

$ClassName[1, 16]  = "PopeXVI";
$ClassName[2, 16]  = "Head MysticXVI";
$ClassName[3, 16]  = "KleptoXVI";
$ClassName[4, 16]  = "ArtisteXVI";
$ClassName[5, 16]  = "GuardianXVI";
$ClassName[6, 16]  = "CrusaderXVI";
$ClassName[7, 16]  = "ArcherXVI";
$ClassName[8, 16]  = "WizardXVI";

$ClassName[1, 17]  = "PopeXVII";
$ClassName[2, 17]  = "Head MysticXVII";
$ClassName[3, 17]  = "KleptoXVII";
$ClassName[4, 17]  = "ArtisteXVII";
$ClassName[5, 17]  = "GuardianXVII";
$ClassName[6, 17]  = "CrusaderXVII";
$ClassName[7, 17]  = "ArcherXVII";
$ClassName[8, 17]  = "WizardXVII";

$ClassName[1, 18]  = "PopeXVIII";
$ClassName[2, 18]  = "Head MysticXVIII";
$ClassName[3, 18]  = "KleptoXVIII";
$ClassName[4, 18]  = "ArtisteXVIII";
$ClassName[5, 18]  = "GuardianXVIII";
$ClassName[6, 18]  = "CrusaderXVIII";
$ClassName[7, 18]  = "ArcherXVIII";
$ClassName[8, 18]  = "WizardXVIII";

$ClassName[1, 19]  = "PopeXIX";
$ClassName[2, 19]  = "Head MysticXIX";
$ClassName[3, 19]  = "KleptoXIX";
$ClassName[4, 19]  = "ArtisteXIX";
$ClassName[5, 19]  = "GuardianXIX";
$ClassName[6, 19]  = "CrusaderXIX";
$ClassName[7, 19]  = "ArcherXIX";
$ClassName[8, 19]  = "WizardXIX";

$ClassName[1, 20]  = "PopeXX";
$ClassName[2, 20]  = "Head MysticXX";
$ClassName[3, 20]  = "KleptoXX";
$ClassName[4, 20]  = "ArtisteXX";
$ClassName[5, 20]  = "GuardianXX";
$ClassName[6, 20]  = "CrusaderXX";
$ClassName[7, 20]  = "ArcherXX";
$ClassName[8, 20]  = "WizardXX";

$ClassName[1, 21]  = "PopeXXI";
$ClassName[2, 21]  = "Head MysticXXI";
$ClassName[3, 21]  = "KleptoXXI";
$ClassName[4, 21]  = "ArtisteXXI";
$ClassName[5, 21]  = "GuardianXXI";
$ClassName[6, 21]  = "CrusaderXXI";
$ClassName[7, 21]  = "ArcherXXI";
$ClassName[8, 21]  = "WizardXXI";

$ClassName[1, 22]  = "PopeXXII";
$ClassName[2, 22]  = "Head MysticXXII";
$ClassName[3, 22]  = "KleptoXXII";
$ClassName[4, 22]  = "ArtisteXXII";
$ClassName[5, 22]  = "GuardianXXII";
$ClassName[6, 22]  = "CrusaderXXII";
$ClassName[7, 22]  = "ArcherXXII";
$ClassName[8, 22]  = "WizardXXII";

$ClassName[1, 23]  = "PopeXXIII";
$ClassName[2, 23]  = "Head MysticXXIII";
$ClassName[3, 23]  = "KleptoXXIII";
$ClassName[4, 23]  = "ArtisteXXIII";
$ClassName[5, 23]  = "GuardianXXIII";
$ClassName[6, 23]  = "CrusaderXXIII";
$ClassName[7, 23]  = "ArcherXXIII";
$ClassName[8, 23]  = "WizardXXIII";

$ClassName[1, 24]  = "PopeXXIV";
$ClassName[2, 24]  = "Head MysticXXIV";
$ClassName[3, 24]  = "KleptoXXIV";
$ClassName[4, 24]  = "ArtisteXXIV";
$ClassName[5, 24]  = "GuardianXXIV";
$ClassName[6, 24]  = "CrusaderXXIV";
$ClassName[7, 24]  = "ArcherXXIV";
$ClassName[8, 24]  = "WizardXXIV";

$ClassName[1, 25]  = "PopeXXV";
$ClassName[2, 25]  = "Head MysticXXV";
$ClassName[3, 25]  = "KleptoXXV";
$ClassName[4, 25]  = "ArtisteXXV";
$ClassName[5, 25]  = "GuardianXXV";
$ClassName[6, 25]  = "CrusaderXXV";
$ClassName[7, 25]  = "ArcherXXV";
$ClassName[8, 25]  = "WizardXXV";

$ClassName[1, 26]  = "PopeXXVI";
$ClassName[2, 26]  = "Head MysticXXVI";
$ClassName[3, 26]  = "KleptoXXVI";
$ClassName[4, 26]  = "ArtisteXXVI";
$ClassName[5, 26]  = "GuardianXXVI";
$ClassName[6, 26]  = "CrusaderXXVI";
$ClassName[7, 26]  = "ArcherXXVI";
$ClassName[8, 26]  = "WizardXXVI";

$ClassName[1, 27]  = "PopeXXVII";
$ClassName[2, 27]  = "Head MysticXXVII";
$ClassName[3, 27]  = "KleptoXXVII";
$ClassName[4, 27]  = "ArtisteXXVII";
$ClassName[5, 27]  = "GuardianXXVII";
$ClassName[6, 27]  = "CrusaderXXVII";
$ClassName[7, 27]  = "ArcherXXVII";
$ClassName[8, 27]  = "WizardXXVII";

$ClassName[1, 28]  = "PopeXXVIII";
$ClassName[2, 28]  = "Head MysticXXVIII";
$ClassName[3, 28]  = "KleptoXXVIII";
$ClassName[4, 28]  = "ArtisteXXVIII";
$ClassName[5, 28]  = "GuardianXXVIII";
$ClassName[6, 28]  = "CrusaderXXVIII";
$ClassName[7, 28]  = "ArcherXXVIII";
$ClassName[8, 28]  = "WizardXXVIII";

$ClassName[1, 29]  = "PopeXXIX";
$ClassName[2, 29]  = "Head MysticXXIX";
$ClassName[3, 29]  = "KleptoXXIX";
$ClassName[4, 29]  = "ArtisteXXIX";
$ClassName[5, 29]  = "GuardianXXIX";
$ClassName[6, 29]  = "CrusaderXXIX";
$ClassName[7, 29]  = "ArcherXXIX";
$ClassName[8, 29]  = "WizardXXIX";

$ClassName[1, 30]  = "PopeXXX";
$ClassName[2, 30]  = "Head MysticXXX";
$ClassName[3, 30]  = "KleptoXXX";
$ClassName[4, 30]  = "ArtisteXXX";
$ClassName[5, 30]  = "GuardianXXX";
$ClassName[6, 30]  = "CrusaderXXX";
$ClassName[7, 30]  = "ArcherXXX";
$ClassName[8, 30]  = "WizardXXX";

$ClassName[1, 31]  = "PopeXXXI";
$ClassName[2, 31]  = "Head MysticXXXI";
$ClassName[3, 31]  = "KleptoXXXI";
$ClassName[4, 31]  = "ArtisteXXXI";
$ClassName[5, 31]  = "GuardianXXXI";
$ClassName[6, 31]  = "CrusaderXXXI";
$ClassName[7, 31]  = "ArcherXXXI";
$ClassName[8, 31]  = "WizardXXXI";

$ClassName[1, 32]  = "PopeXXXII";
$ClassName[2, 32]  = "Head MysticXXXII";
$ClassName[3, 32]  = "KleptoXXXII";
$ClassName[4, 32]  = "ArtisteXXXII";
$ClassName[5, 32]  = "GuardianXXXII";
$ClassName[6, 32]  = "CrusaderXXXII";
$ClassName[7, 32]  = "ArcherXXXII";
$ClassName[8, 32]  = "WizardXXXII";

$ClassName[1, 33]  = "PopeXXXIII";
$ClassName[2, 33]  = "Head MysticXXXIII";
$ClassName[3, 33]  = "KleptoXXXIII";
$ClassName[4, 33]  = "ArtisteXXXIII";
$ClassName[5, 33]  = "GuardianXXXIII";
$ClassName[6, 33]  = "CrusaderXXXIII";
$ClassName[7, 33]  = "ArcherXXXIII";
$ClassName[8, 33]  = "WizardXXXIII";

$ClassName[1, 34]  = "PopeXXXIV";
$ClassName[2, 34]  = "Head MysticXXXIV";
$ClassName[3, 34]  = "KleptoXXXIV";
$ClassName[4, 34]  = "ArtisteXXXIV";
$ClassName[5, 34]  = "GuardianXXXIV";
$ClassName[6, 34]  = "CrusaderXXXIV";
$ClassName[7, 34]  = "ArcherXXXIV";
$ClassName[8, 34]  = "WizardXXXIV";

$ClassName[1, 35]  = "PopeXXXV";
$ClassName[2, 35]  = "Head MysticXXXV";
$ClassName[3, 35]  = "KleptoXXXV";
$ClassName[4, 35]  = "ArtisteXXXV";
$ClassName[5, 35]  = "GuardianXXXV";
$ClassName[6, 35]  = "CrusaderXXXV";
$ClassName[7, 35]  = "ArcherXXXV";
$ClassName[8, 35]  = "WizardXXXV";

$ClassName[1, 36]  = "PopeXXXVI";
$ClassName[2, 36]  = "Head MysticXXXVI";
$ClassName[3, 36]  = "KleptoXXXVI";
$ClassName[4, 36]  = "ArtisteXXXVI";
$ClassName[5, 36]  = "GuardianXXXVI";
$ClassName[6, 36]  = "CrusaderXXXVI";
$ClassName[7, 36]  = "ArcherXXXVI";
$ClassName[8, 36]  = "WizardXXXVI";

$ClassName[1, 37]  = "PopeXXXVII";
$ClassName[2, 37]  = "Head MysticXXXVII";
$ClassName[3, 37]  = "KleptoXXXVII";
$ClassName[4, 37]  = "ArtisteXXXVII";
$ClassName[5, 37]  = "GuardianXXXVII";
$ClassName[6, 37]  = "CrusaderXXXVII";
$ClassName[7, 37]  = "ArcherXXXVII";
$ClassName[8, 37]  = "WizardXXXVII";

$ClassName[1, 38]  = "PopeXXXVIII";
$ClassName[2, 38]  = "Head MysticXXXVIII";
$ClassName[3, 38]  = "KleptoXXXVIII";
$ClassName[4, 38]  = "ArtisteXXXVIII";
$ClassName[5, 38]  = "GuardianXXXVIII";
$ClassName[6, 38]  = "CrusaderXXXVIII";
$ClassName[7, 38]  = "ArcherXXXVIII";
$ClassName[8, 38]  = "WizardXXXVIII";

$ClassName[1, 39]  = "PopeXXXIX";
$ClassName[2, 39]  = "Head MysticXXXIX";
$ClassName[3, 39]  = "KleptoXXXIX";
$ClassName[4, 39]  = "ArtisteXXXIX";
$ClassName[5, 39]  = "GuardianXXXIX";
$ClassName[6, 39]  = "CrusaderXXXIX";
$ClassName[7, 39]  = "ArcherXXXIX";
$ClassName[8, 39]  = "WizardXXXIX";

$ClassName[1, 40]  = "PopeXL";
$ClassName[2, 40]  = "Head MysticXL";
$ClassName[3, 40]  = "KleptoXL";
$ClassName[4, 40]  = "ArtisteXL";
$ClassName[5, 40]  = "GuardianXL";
$ClassName[6, 40]  = "CrusaderXL";
$ClassName[7, 40]  = "ArcherXL";
$ClassName[8, 40]  = "WizardXL";

$ClassName[1, 41]  = "PopeXLI";
$ClassName[2, 41]  = "Head MysticXLI";
$ClassName[3, 41]  = "KleptoXLI";
$ClassName[4, 41]  = "ArtisteXLI";
$ClassName[5, 41]  = "GuardianXLI";
$ClassName[6, 41]  = "CrusaderXLI";
$ClassName[7, 41]  = "ArcherXLI";
$ClassName[8, 41]  = "WizardXLI";

$ClassName[1, 42]  = "PopeXLII";
$ClassName[2, 42]  = "Head MysticXLII";
$ClassName[3, 42]  = "KleptoXLII";
$ClassName[4, 42]  = "ArtisteXLII";
$ClassName[5, 42]  = "GuardianXLII";
$ClassName[6, 42]  = "CrusaderXLII";
$ClassName[7, 42]  = "ArcherXLII";
$ClassName[8, 42]  = "WizardXLII";

$ClassName[1, 43]  = "PopeXLIII";
$ClassName[2, 43]  = "Head MysticXLIII";
$ClassName[3, 43]  = "KleptoXLIII";
$ClassName[4, 43]  = "ArtisteXLIII";
$ClassName[5, 43]  = "GuardianXLIII";
$ClassName[6, 43]  = "CrusaderXLIII";
$ClassName[7, 43]  = "ArcherXLIII";
$ClassName[8, 43]  = "WizardXLIII";

$ClassName[1, 44]  = "PopeXLIV";
$ClassName[2, 44]  = "Head MysticXLIV";
$ClassName[3, 44]  = "KleptoXLIV";
$ClassName[4, 44]  = "ArtisteXLIV";
$ClassName[5, 44]  = "GuardianXLIV";
$ClassName[6, 44]  = "CrusaderXLIV";
$ClassName[7, 44]  = "ArcherXLIV";
$ClassName[8, 44]  = "WizardXLIV";

$ClassName[1, 45]  = "PopeXLV";
$ClassName[2, 45]  = "Head MysticXLV";
$ClassName[3, 45]  = "KleptoXLV";
$ClassName[4, 45]  = "ArtisteXLV";
$ClassName[5, 45]  = "GuardianXLV";
$ClassName[6, 45]  = "CrusaderXLV";
$ClassName[7, 45]  = "ArcherXLV";
$ClassName[8, 45]  = "WizardXLV";

$ClassName[1, 46]  = "PopeXLVI";
$ClassName[2, 46]  = "Head MysticXLVI";
$ClassName[3, 46]  = "KleptoXLVI";
$ClassName[4, 46]  = "ArtisteXLVI";
$ClassName[5, 46]  = "GuardianXLVI";
$ClassName[6, 46]  = "CrusaderXLVI";
$ClassName[7, 46]  = "ArcherXLVI";
$ClassName[8, 46]  = "WizardXLVI";

$ClassName[1, 47]  = "PopeXLVII";
$ClassName[2, 47]  = "Head MysticXLVII";
$ClassName[3, 47]  = "KleptoXLVII";
$ClassName[4, 47]  = "ArtisteXLVII";
$ClassName[5, 47]  = "GuardianXLVII";
$ClassName[6, 47]  = "CrusaderXLVII";
$ClassName[7, 47]  = "ArcherXLVII";
$ClassName[8, 47]  = "WizardXLVII";

$ClassName[1, 48]  = "PopeXLVIII";
$ClassName[2, 48]  = "Head MysticXLVIII";
$ClassName[3, 48]  = "KleptoXLVIII";
$ClassName[4, 48]  = "ArtisteXLVIII";
$ClassName[5, 48]  = "GuardianXLVIII";
$ClassName[6, 48]  = "CrusaderXLVIII";
$ClassName[7, 48]  = "ArcherXLVIII";
$ClassName[8, 48]  = "WizardXLVIII";

$ClassName[1, 49]  = "PopeXLIX";
$ClassName[2, 49]  = "Head MysticXLIX";
$ClassName[3, 49]  = "KleptoXLIX";
$ClassName[4, 49]  = "ArtisteXLIX";
$ClassName[5, 49]  = "GuardianXLIX";
$ClassName[6, 49]  = "CrusaderXLIX";
$ClassName[7, 49]  = "ArcherXLIX";
$ClassName[8, 49]  = "WizardXLIX";

$ClassName[1, 50]  = "PopeL";
$ClassName[2, 50]  = "Head MysticL";
$ClassName[3, 50]  = "KleptoL";
$ClassName[4, 50]  = "ArtisteL";
$ClassName[5, 50]  = "GuardianL";
$ClassName[6, 50]  = "CrusaderL";
$ClassName[7, 50]  = "ArcherL";
$ClassName[8, 50]  = "WizardL";

$ClassName[1, 51]  = "PopeLI";
$ClassName[2, 51]  = "Head MysticLI";
$ClassName[3, 51]  = "KleptoLI";
$ClassName[4, 51]  = "ArtisteLI";
$ClassName[5, 51]  = "GuardianLI";
$ClassName[6, 51]  = "CrusaderLI";
$ClassName[7, 51]  = "ArcherLI";
$ClassName[8, 51]  = "WizardLI";

$ClassName[1, 52]  = "PopeLII";
$ClassName[2, 52]  = "Head MysticLII";
$ClassName[3, 52]  = "KleptoLII";
$ClassName[4, 52]  = "ArtisteLII";
$ClassName[5, 52]  = "GuardianLII";
$ClassName[6, 52]  = "CrusaderLII";
$ClassName[7, 52]  = "ArcherLII";
$ClassName[8, 52]  = "WizardLII";

$ClassName[1, 53]  = "PopeLIII";
$ClassName[2, 53]  = "Head MysticLIII";
$ClassName[3, 53]  = "KleptoLIII";
$ClassName[4, 53]  = "ArtisteLIII";
$ClassName[5, 53]  = "GuardianLIII";
$ClassName[6, 53]  = "CrusaderLIII";
$ClassName[7, 53]  = "ArcherLIII";
$ClassName[8, 53]  = "WizardLIII";

$ClassName[1, 54]  = "PopeLIV";
$ClassName[2, 54]  = "Head MysticLIV";
$ClassName[3, 54]  = "KleptoLIV";
$ClassName[4, 54]  = "ArtisteLIV";
$ClassName[5, 54]  = "GuardianLIV";
$ClassName[6, 54]  = "CrusaderLIV";
$ClassName[7, 54]  = "ArcherLIV";
$ClassName[8, 54]  = "WizardLIV";

$ClassName[1, 55]  = "PopeLV";
$ClassName[2, 55]  = "Head MysticLV";
$ClassName[3, 55]  = "KleptoLV";
$ClassName[4, 55]  = "ArtisteLV";
$ClassName[5, 55]  = "GuardianLV";
$ClassName[6, 55]  = "CrusaderLV";
$ClassName[7, 55]  = "ArcherLV";
$ClassName[8, 55]  = "WizardLV";

$ClassName[1, 56]  = "PopeLVI";
$ClassName[2, 56]  = "Head MysticLVI";
$ClassName[3, 56]  = "KleptoLVI";
$ClassName[4, 56]  = "ArtisteLVI";
$ClassName[5, 56]  = "GuardianLVI";
$ClassName[6, 56]  = "CrusaderLVI";
$ClassName[7, 56]  = "ArcherLVI";
$ClassName[8, 56]  = "WizardLVI";

$ClassName[1, 57]  = "PopeLVII";
$ClassName[2, 57]  = "Head MysticLVII";
$ClassName[3, 57]  = "KleptoLVII";
$ClassName[4, 57]  = "ArtisteLVII";
$ClassName[5, 57]  = "GuardianLVII";
$ClassName[6, 57]  = "CrusaderLVII";
$ClassName[7, 57]  = "ArcherLVII";
$ClassName[8, 57]  = "WizardLVII";

$ClassName[1, 58]  = "PopeLVIII";
$ClassName[2, 58]  = "Head MysticLVIII";
$ClassName[3, 58]  = "KleptoLVIII";
$ClassName[4, 58]  = "ArtisteLVIII";
$ClassName[5, 58]  = "GuardianLVIII";
$ClassName[6, 58]  = "CrusaderLVIII";
$ClassName[7, 58]  = "ArcherLVIII";
$ClassName[8, 58]  = "WizardLVIII";

$ClassName[1, 59]  = "PopeLIX";
$ClassName[2, 59]  = "Head MysticLIX";
$ClassName[3, 59]  = "KleptoLIX";
$ClassName[4, 59]  = "ArtisteLIX";
$ClassName[5, 59]  = "GuardianLIX";
$ClassName[6, 59]  = "CrusaderLIX";
$ClassName[7, 59]  = "ArcherLIX";
$ClassName[8, 59]  = "WizardLIX";

$ClassName[1, 60]  = "PopeLX";
$ClassName[2, 60]  = "Head MysticLX";
$ClassName[3, 60]  = "KleptoLX";
$ClassName[4, 60]  = "ArtisteLX";
$ClassName[5, 60]  = "GuardianLX";
$ClassName[6, 60]  = "CrusaderLX";
$ClassName[7, 60]  = "ArcherLX";
$ClassName[8, 60]  = "WizardLX";

$ClassName[1, 61]  = "PopeLXI";
$ClassName[2, 61]  = "Head MysticLXI";
$ClassName[3, 61]  = "KleptoLXI";
$ClassName[4, 61]  = "ArtisteLXI";
$ClassName[5, 61]  = "GuardianLXI";
$ClassName[6, 61]  = "CrusaderLXI";
$ClassName[7, 61]  = "ArcherLXI";
$ClassName[8, 61]  = "WizardLXI";

$ClassName[1, 62]  = "PopeLXII";
$ClassName[2, 62]  = "Head MysticLXII";
$ClassName[3, 62]  = "KleptoLXII";
$ClassName[4, 62]  = "ArtisteLXII";
$ClassName[5, 62]  = "GuardianLXII";
$ClassName[6, 62]  = "CrusaderLXII";
$ClassName[7, 62]  = "ArcherLXII";
$ClassName[8, 62]  = "WizardLXII";

$ClassName[1, 63]  = "PopeLXIII";
$ClassName[2, 63]  = "Head MysticLXIII";
$ClassName[3, 63]  = "KleptoLXIII";
$ClassName[4, 63]  = "ArtisteLXIII";
$ClassName[5, 63]  = "GuardianLXIII";
$ClassName[6, 63]  = "CrusaderLXIII";
$ClassName[7, 63]  = "ArcherLXIII";
$ClassName[8, 63]  = "WizardLXIII";

$ClassName[1, 64]  = "PopeLXIV";
$ClassName[2, 64]  = "Head MysticLXIV";
$ClassName[3, 64]  = "KleptoLXIV";
$ClassName[4, 64]  = "ArtisteLXIV";
$ClassName[5, 64]  = "GuardianLXIV";
$ClassName[6, 64]  = "CrusaderLXIV";
$ClassName[7, 64]  = "ArcherLXIV";
$ClassName[8, 64]  = "WizardLXIV";

$ClassName[1, 65]  = "PopeLXV";
$ClassName[2, 65]  = "Head MysticLXV";
$ClassName[3, 65]  = "KleptoLXV";
$ClassName[4, 65]  = "ArtisteLXV";
$ClassName[5, 65]  = "GuardianLXV";
$ClassName[6, 65]  = "CrusaderLXV";
$ClassName[7, 65]  = "ArcherLXV";
$ClassName[8, 65]  = "WizardLXV";

$ClassName[1, 66]  = "PopeLXVI";
$ClassName[2, 66]  = "Head MysticLXVI";
$ClassName[3, 66]  = "KleptoLXVI";
$ClassName[4, 66]  = "ArtisteLXVI";
$ClassName[5, 66]  = "GuardianLXVI";
$ClassName[6, 66]  = "CrusaderLXVI";
$ClassName[7, 66]  = "ArcherLXVI";
$ClassName[8, 66]  = "WizardLXVI";

$ClassName[1, 67]  = "PopeLXVII";
$ClassName[2, 67]  = "Head MysticLXVII";
$ClassName[3, 67]  = "KleptoLXVII";
$ClassName[4, 67]  = "ArtisteLXVII";
$ClassName[5, 67]  = "GuardianLXVII";
$ClassName[6, 67]  = "CrusaderLXVII";
$ClassName[7, 67]  = "ArcherLXVII";
$ClassName[8, 67]  = "WizardLXVII";

$ClassName[1, 68]  = "PopeLXVIII";
$ClassName[2, 68]  = "Head MysticLXVIII";
$ClassName[3, 68]  = "KleptoLXVIII";
$ClassName[4, 68]  = "ArtisteLXVIII";
$ClassName[5, 68]  = "GuardianLXVIII";
$ClassName[6, 68]  = "CrusaderLXVIII";
$ClassName[7, 68]  = "ArcherLXVIII";
$ClassName[8, 68]  = "WizardLXVIII";

$ClassName[1, 69]  = "PopeLXIX";
$ClassName[2, 69]  = "Head MysticLXIX";
$ClassName[3, 69]  = "KleptoLXIX";
$ClassName[4, 69]  = "ArtisteLXIX";
$ClassName[5, 69]  = "GuardianLXIX";
$ClassName[6, 69]  = "CrusaderLXIX";
$ClassName[7, 69]  = "ArcherLXIX";
$ClassName[8, 69]  = "WizardLXIX";

$ClassName[1, 70]  = "PopeLXX";
$ClassName[2, 70]  = "Head MysticLXX";
$ClassName[3, 70]  = "KleptoLXX";
$ClassName[4, 70]  = "ArtisteLXX";
$ClassName[5, 70]  = "GuardianLXX";
$ClassName[6, 70]  = "CrusaderLXX";
$ClassName[7, 70]  = "ArcherLXX";
$ClassName[8, 70]  = "WizardLXX";
	
$ClassName[1, 71]  = "PopeLXXI";
$ClassName[2, 71]  = "Head MysticLXXI";
$ClassName[3, 71]  = "KleptoLXXI";
$ClassName[4, 71]  = "ArtisteLXXI";
$ClassName[5, 71]  = "GuardianLXXI";
$ClassName[6, 71]  = "CrusaderLXXI";
$ClassName[7, 71]  = "ArcherLXXI";
$ClassName[8, 71]  = "WizardLXXI";
	
$ClassName[1, 72]  = "PopeLXXII";
$ClassName[2, 72]  = "Head MysticLXXII";
$ClassName[3, 72]  = "KleptoLXXII";
$ClassName[4, 72]  = "ArtisteLXXII";
$ClassName[5, 72]  = "GuardianLXXII";
$ClassName[6, 72]  = "CrusaderLXXII";
$ClassName[7, 72]  = "ArcherLXXII";
$ClassName[8, 72]  = "WizardLXXII";
	
$ClassName[1, 73]  = "PopeLXXIII";
$ClassName[2, 73]  = "Head MysticLXXIII";
$ClassName[3, 73]  = "KleptoLXXIII";
$ClassName[4, 73]  = "ArtisteLXXIII";
$ClassName[5, 73]  = "GuardianLXXIII";
$ClassName[6, 73]  = "CrusaderLXXIII";
$ClassName[7, 73]  = "ArcherLXXIII";
$ClassName[8, 73]  = "WizardLXXIII";
	
$ClassName[1, 74]  = "PopeLXXIV";
$ClassName[2, 74]  = "Head MysticLXXIV";
$ClassName[3, 74]  = "KleptoLXXIV";
$ClassName[4, 74]  = "ArtisteLXXIV";
$ClassName[5, 74]  = "GuardianLXXIV";
$ClassName[6, 74]  = "CrusaderLXXIV";
$ClassName[7, 74]  = "ArcherLXXIV";
$ClassName[8, 74]  = "WizardLXXIV";
	
$ClassName[1, 75]  = "PopeLXXV";
$ClassName[2, 75]  = "Head MysticLXXV";
$ClassName[3, 75]  = "KleptoLXXV";
$ClassName[4, 75]  = "ArtisteLXXV";
$ClassName[5, 75]  = "GuardianLXXV";
$ClassName[6, 75]  = "CrusaderLXXV";
$ClassName[7, 75]  = "ArcherLXXV";
$ClassName[8, 75]  = "WizardLXXV";
	
$ClassName[1, 76]  = "PopeLXXVI";
$ClassName[2, 76]  = "Head MysticLXXVI";
$ClassName[3, 76]  = "KleptoLXXVI";
$ClassName[4, 76]  = "ArtisteLXXVI";
$ClassName[5, 76]  = "GuardianLXXVI";
$ClassName[6, 76]  = "CrusaderLXXVI";
$ClassName[7, 76]  = "ArcherLXXVI";
$ClassName[8, 76]  = "WizardLXXVI";
	
$ClassName[1, 77]  = "PopeLXXVII";
$ClassName[2, 77]  = "Head MysticLXXVII";
$ClassName[3, 77]  = "KleptoLXXVII";
$ClassName[4, 77]  = "ArtisteLXXVII";
$ClassName[5, 77]  = "GuardianLXXVII";
$ClassName[6, 77]  = "CrusaderLXXVII";
$ClassName[7, 77]  = "ArcherLXXVII";
$ClassName[8, 77]  = "WizardLXXVII";
	
$ClassName[1, 78]  = "PopeLXXVIII";
$ClassName[2, 78]  = "Head MysticLXXVIII";
$ClassName[3, 78]  = "KleptoLXXVIII";
$ClassName[4, 78]  = "ArtisteLXXVIII";
$ClassName[5, 78]  = "GuardianLXXVIII";
$ClassName[6, 78]  = "CrusaderLXXVIII";
$ClassName[7, 78]  = "ArcherLXXVIII";
$ClassName[8, 78]  = "WizardLXXVIII";
	
$ClassName[1, 79]  = "PopeLXXIX";
$ClassName[2, 79]  = "Head MysticLXXIX";
$ClassName[3, 79]  = "KleptoLXXIX";
$ClassName[4, 79]  = "ArtisteLXXIX";
$ClassName[5, 79]  = "GuardianLXXIX";
$ClassName[6, 79]  = "CrusaderLXXIX";
$ClassName[7, 79]  = "ArcherLXXIX";
$ClassName[8, 79]  = "WizardLXXIX";
	
$ClassName[1, 80]  = "PopeLXXX";
$ClassName[2, 80]  = "Head MysticLXXX";
$ClassName[3, 80]  = "KleptoLXXX";
$ClassName[4, 80]  = "ArtisteLXXX";
$ClassName[5, 80]  = "GuardianLXXX";
$ClassName[6, 80]  = "CrusaderLXXX";
$ClassName[7, 80]  = "ArcherLXXX";
$ClassName[8, 80]  = "WizardLXXX";
	
$ClassName[1, 81]  = "PopeLXXXI";
$ClassName[2, 81]  = "Head MysticLXXXI";
$ClassName[3, 81]  = "KleptoLXXXI";
$ClassName[4, 81]  = "ArtisteLXXXI";
$ClassName[5, 81]  = "GuardianLXXXI";
$ClassName[6, 81]  = "CrusaderLXXXI";
$ClassName[7, 81]  = "ArcherLXXXI";
$ClassName[8, 81]  = "WizardLXXXI";
	
$ClassName[1, 82]  = "PopeLXXXII";
$ClassName[2, 82]  = "Head MysticLXXXII";
$ClassName[3, 82]  = "KleptoLXXXII";
$ClassName[4, 82]  = "ArtisteLXXXII";
$ClassName[5, 82]  = "GuardianLXXXII";
$ClassName[6, 82]  = "CrusaderLXXXII";
$ClassName[7, 82]  = "ArcherLXXXII";
$ClassName[8, 82]  = "WizardLXXXII";
	
$ClassName[1, 83]  = "PopeLXXXIII";
$ClassName[2, 83]  = "Head MysticLXXXIII";
$ClassName[3, 83]  = "KleptoLXXXIII";
$ClassName[4, 83]  = "ArtisteLXXXIII";
$ClassName[5, 83]  = "GuardianLXXXIII";
$ClassName[6, 83]  = "CrusaderLXXXIII";
$ClassName[7, 83]  = "ArcherLXXXIII";
$ClassName[8, 83]  = "WizardLXXXIII";
	
$ClassName[1, 84]  = "PopeLXXXIV";
$ClassName[2, 84]  = "Head MysticLXXXIV";
$ClassName[3, 84]  = "KleptoLXXXIV";
$ClassName[4, 84]  = "ArtisteLXXXIV";
$ClassName[5, 84]  = "GuardianLXXXIV";
$ClassName[6, 84]  = "CrusaderLXXXIV";
$ClassName[7, 84]  = "ArcherLXXXIV";
$ClassName[8, 84]  = "WizardLXXXIV";
	
$ClassName[1, 85]  = "PopeLXXXV";
$ClassName[2, 85]  = "Head MysticLXXXV";
$ClassName[3, 85]  = "KleptoLXXXV";
$ClassName[4, 85]  = "ArtisteLXXXV";
$ClassName[5, 85]  = "GuardianLXXXV";
$ClassName[6, 85]  = "CrusaderLXXXV";
$ClassName[7, 85]  = "ArcherLXXXV";
$ClassName[8, 85]  = "WizardLXXXV";
	
$ClassName[1, 86]  = "PopeLXXXVI";
$ClassName[2, 86]  = "Head MysticLXXXVI";
$ClassName[3, 86]  = "KleptoLXXXVI";
$ClassName[4, 86]  = "ArtisteLXXXVI";
$ClassName[5, 86]  = "GuardianLXXXVI";
$ClassName[6, 86]  = "CrusaderLXXXVI";
$ClassName[7, 86]  = "ArcherLXXXVI";
$ClassName[8, 86]  = "WizardLXXXVI";
	
$ClassName[1, 87]  = "PopeLXXXVII";
$ClassName[2, 87]  = "Head MysticLXXXVII";
$ClassName[3, 87]  = "KleptoLXXXVII";
$ClassName[4, 87]  = "ArtisteLXXXVII";
$ClassName[5, 87]  = "GuardianLXXXVII";
$ClassName[6, 87]  = "CrusaderLXXXVII";
$ClassName[7, 87]  = "ArcherLXXXVII";
$ClassName[8, 87]  = "WizardLXXXVII";
	
$ClassName[1, 88]  = "PopeLXXXVIII";
$ClassName[2, 88]  = "Head MysticLXXXVIII";
$ClassName[3, 88]  = "KleptoLXXXVIII";
$ClassName[4, 88]  = "ArtisteLXXXVIII";
$ClassName[5, 88]  = "GuardianLXXXVIII";
$ClassName[6, 88]  = "CrusaderLXXXVIII";
$ClassName[7, 88]  = "ArcherLXXXVIII";
$ClassName[8, 88]  = "WizardLXXXVIII";
	
$ClassName[1, 89]  = "PopeLXXXIX";
$ClassName[2, 89]  = "Head MysticLXXXIX";
$ClassName[3, 89]  = "KleptoLXXXIX";
$ClassName[4, 89]  = "ArtisteLXXXIX";
$ClassName[5, 89]  = "GuardianLXXXIX";
$ClassName[6, 89]  = "CrusaderLXXXIX";
$ClassName[7, 89]  = "ArcherLXXXIX";
$ClassName[8, 89]  = "WizardLXXXIX";
	
$ClassName[1, 90]  = "PopeXC";
$ClassName[2, 90]  = "Head MysticXC";
$ClassName[3, 90]  = "KleptoXC";
$ClassName[4, 90]  = "ArtisteXC";
$ClassName[5, 90]  = "GuardianXC";
$ClassName[6, 90]  = "CrusaderXC";
$ClassName[7, 90]  = "ArcherXC";
$ClassName[8, 90]  = "WizardXC";
	
$ClassName[1, 91]  = "PopeXCI";
$ClassName[2, 91]  = "Head MysticXCI";
$ClassName[3, 91]  = "KleptoXCI";
$ClassName[4, 91]  = "ArtisteXCI";
$ClassName[5, 91]  = "GuardianXCI";
$ClassName[6, 91]  = "CrusaderXCI";
$ClassName[7, 91]  = "ArcherXCI";
$ClassName[8, 91]  = "WizardXCI";
	
$ClassName[1, 92]  = "PopeXCII";
$ClassName[2, 92]  = "Head MysticXCII";
$ClassName[3, 92]  = "KleptoXCII";
$ClassName[4, 92]  = "ArtisteXCII";
$ClassName[5, 92]  = "GuardianXCII";
$ClassName[6, 92]  = "CrusaderXCII";
$ClassName[7, 92]  = "ArcherXCII";
$ClassName[8, 92]  = "WizardXCII";
	
$ClassName[1, 93]  = "PopeXCIII";
$ClassName[2, 93]  = "Head MysticXCIII";
$ClassName[3, 93]  = "KleptoXCIII";
$ClassName[4, 93]  = "ArtisteXCIII";
$ClassName[5, 93]  = "GuardianXCIII";
$ClassName[6, 93]  = "CrusaderXCIII";
$ClassName[7, 93]  = "ArcherXCIII";
$ClassName[8, 93]  = "WizardXCIII";
	
$ClassName[1, 94]  = "PopeXCIV";
$ClassName[2, 94]  = "Head MysticXCIV";
$ClassName[3, 94]  = "KleptoXCIV";
$ClassName[4, 94]  = "ArtisteXCIV";
$ClassName[5, 94]  = "GuardianXCIV";
$ClassName[6, 94]  = "CrusaderXCIV";
$ClassName[7, 94]  = "ArcherXCIV";
$ClassName[8, 94]  = "WizardXCIV";
	
$ClassName[1, 95]  = "PopeXCV";
$ClassName[2, 95]  = "Head MysticXCV";
$ClassName[3, 95]  = "KleptoXCV";
$ClassName[4, 95]  = "ArtisteXCV";
$ClassName[5, 95]  = "GuardianXCV";
$ClassName[6, 95]  = "CrusaderXCV";
$ClassName[7, 95]  = "ArcherXCV";
$ClassName[8, 95]  = "WizardXCV";
	
$ClassName[1, 96]  = "PopeXCVI";
$ClassName[2, 96]  = "Head MysticXCVI";
$ClassName[3, 96]  = "KleptoXCVI";
$ClassName[4, 96]  = "ArtisteXCVI";
$ClassName[5, 96]  = "GuardianXCVI";
$ClassName[6, 96]  = "CrusaderXCVI";
$ClassName[7, 96]  = "ArcherXCVI";
$ClassName[8, 96]  = "WizardXCVI";
	
$ClassName[1, 97]  = "PopeXCVII";
$ClassName[2, 97]  = "Head MysticXCVII";
$ClassName[3, 97]  = "KleptoXCVII";
$ClassName[4, 97]  = "ArtisteXCVII";
$ClassName[5, 97]  = "GuardianXCVII";
$ClassName[6, 97]  = "CrusaderXCVII";
$ClassName[7, 97]  = "ArcherXCVII";
$ClassName[8, 97]  = "WizardXCVII";
	
$ClassName[1, 98]  = "PopeXCVIII";
$ClassName[2, 98]  = "Head MysticXCVIII";
$ClassName[3, 98]  = "KleptoXCVIII";
$ClassName[4, 98]  = "ArtisteXCVIII";
$ClassName[5, 98]  = "GuardianXCVIII";
$ClassName[6, 98]  = "CrusaderXCVIII";
$ClassName[7, 98]  = "ArcherXCVIII";
$ClassName[8, 98]  = "WizardXCVIII";
	
$ClassName[1, 99]  = "PopeXCIX";
$ClassName[2, 99]  = "Head MysticXCIX";
$ClassName[3, 99]  = "KleptoXCIX";
$ClassName[4, 99]  = "ArtisteXCIX";
$ClassName[5, 99]  = "GuardianXCIX";
$ClassName[6, 99]  = "CrusaderXCIX";
$ClassName[7, 99]  = "ArcherXCIX";
$ClassName[8, 99]  = "WizardXCIX";
	
$ClassName[1, 100]  = "God";
$ClassName[2, 100]  = "God";
$ClassName[3, 100]  = "God";
$ClassName[4, 100]  = "God";
$ClassName[5, 100]  = "God";
$ClassName[6, 100]  = "God";
$ClassName[7, 100]  = "God";
$ClassName[8, 100]  = "God";

function getFinalCLASS(%clientId)
{
	dbecho($dbechoMode, "getFinalCLASS(" @ %clientId @ ")");

	for(%i = 1; $ClassName[%i, 0] != ""; %i++)
	{
		if(String::ICompare($ClassName[%i, 0], fetchData(%clientId, "CLASS")) == 0)
			return $ClassName[%i, fetchData(%clientId, "RemortStep")];
	}
	return -1;
}

function IsAClass(%class)
{
	dbecho($dbechoMode, "IsAClass(" @ %class @ ")");

	for(%i = 1; $ClassName[%i, 0] != ""; %i++)
	{
		if(String::ICompare(%class, $ClassName[%i, 0]) == 0)
			return True;
	}

	return False;
}