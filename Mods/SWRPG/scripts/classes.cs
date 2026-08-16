//There are THREE hard-coded groups:
//-Jedi
//-Military
//-Rogue

//Each of these has classes.  They are specified in here.
//Anything that does NOT have to do with visuals when it comes to classes should ALWAYS use the 0 offset in $ClassName.

$initcoins[Jedi] = "3d6x10";
$initcoins[Military] = "5d4x10";
$initcoins[Rogue] = "1d4+1x10";

$MinHP[MaleHuman] = 12;
$MinHP[FemaleHuman] = 12;
$MinHP[MaleWookiee] = 12;
$MinHP[FemaleWookiee] = 12;
$MinHP[Traveller] = 12;
$MinHP[Goblin] = 0;
$MinHP[Tusken] = 2;
$MinHP[Orc] = 7;
$MinHP[Ogre] = 10;
$MinHP[Gnoll] = 3;
$MinHP[Undead] = 11;
$MinHP[Elf] = 10;
$MinHP[Minotaur] = 15;
$MinHP[Uber] = 25;
$MinHP[DeathKnight] = 5000;

$ClassName[1, 0] = "Jedi Apprentice";
$ClassName[2, 0] = "Gray Apprentice";
$ClassName[3, 0] = "Sith Apprentice";
$ClassName[4, 0] = "Private";
$ClassName[5, 0] = "Navalman Recruit"; $ClassNameF[5, 0] = "Navalwoman Recruit";
$ClassName[6, 0] = "Mercenary";
$ClassName[7, 0] = "Smuggler";
//bot classes
$ClassName[8, 0] = "Fighter";
$ClassName[9, 0] = "Shaman";

$ClassGroup["Jedi Apprentice"] = "Jedi";
$ClassGroup["Gray Apprentice"] = "Jedi";
$ClassGroup["Sith Apprentice"] = "Jedi";
$ClassGroup[Private] = "Military";
$ClassGroup["Navalman Recruit"] = "Military";
$ClassGroup[Mercenary] = "Rogue";
$ClassGroup[Smuggler] = "Rogue";
//Smuggler
//Outlaw
//Pirate
//Scoundrel
//Bootlegger

//This is what shows up when selecting your class in the tab menu, and in the SWRPG version of rpghud.cs
$ClassTitle[1] = "Light Jedi";
$ClassTitle[2] = "Gray Jedi";
$ClassTitle[3] = "Dark Jedi";
$ClassTitle[4] = "Soldier";
$ClassTitle[5] = "Pilot";
//$ClassTitle[6, 0] = "Mercenary";
//$ClassTitle[7, 0] = "Smuggler";

$ClassAbvName[JediApprentice] = "Jedi Apprentice";
$ClassAbvName[GrayApprentice] = "Gray Apprentice";
$ClassAbvName[SithApprentice] = "Sith Apprentice";
$ClassAbvName[NavalmanRecruit] = "Navalman Recruit";
//So that multi-word class names can be used for
// #givethisstuff, packs, etc. For example:
//to make someone a "Sith Apprentice", use "SithApprentice"
//i.e. #givethisstuff hazor CLASS SithApprentice
//Giving male class names even if the player is female.

$SpawnStuff[Human, "Jedi Apprentice"] = "TrainingLightsaber 1 KoltoVial 3 ApprenticeRobe 1";
$SpawnStuff[Human, "Gray Apprentice"] = "TrainingLightsaber 1 KoltoVial 3 ApprenticeRobe 1";
$SpawnStuff[Human, "Sith Apprentice"] = "TrainingLightsaber 1 KoltoVial 3 ApprenticeRobe 1";
$SpawnStuff[Human, Private] = "HoldoutBlaster 1 EnergyCells 100 PaddedCombatSuit 1";
$SpawnStuff[Human, "Navalman Recruit"] = "HoldoutBlaster 1 EnergyCells 100 PaddedCombatSuit 1";
$SpawnStuff[Human, Mercenary] = "HoldoutBlaster 1 EnergyCells 100 PaddedCombatSuit 1";
$SpawnStuff[Human, Smuggler] = "HoldoutBlaster 1 EnergyCells 100 PaddedCombatSuit 1";

$SpawnStuff[Wookiee, "Jedi Apprentice"] = "TrainingLightsaber 1 KoltoVial 3";
$SpawnStuff[Wookiee, "Gray Apprentice"] = "TrainingLightsaber 1 KoltoVial 3";
$SpawnStuff[Wookiee, "Sith Apprentice"] = "TrainingLightsaber 1 KoltoVial 3";
$SpawnStuff[Wookiee, Private] = "HoldoutBlaster 1 EnergyCells 100 ";
$SpawnStuff[Wookiee, "Navalman Recruit"] = "HoldoutBlaster 1 EnergyCells 100 ";
$SpawnStuff[Wookiee, Mercenary] = "HoldoutBlaster 1 EnergyCells 100 ";
$SpawnStuff[Wookiee, Smuggler] = "HoldoutBlaster 1 EnergyCells 100 ";

function ClassInitialSpawnStuff(%clientId, %class, %race) //see processMenupickclass() in rpgstats.cs
{
	storeData(%clientId, "spawnStuff", %stuff @ " Pickaxe 1 BactaVial 3 RecallBeacon 1");
}

//===================================
// REMORTS
//===================================
$ClassName[1, 1] = "Jedi Apprentice I";
$ClassName[2, 1] = "Gray Apprentice I";
$ClassName[3, 1] = "Sith Apprentice I";
$ClassName[4, 1] = "Private First Class";
$ClassName[5, 1] = "Navalman"; $ClassNameF[5, 1] = "Navalwoman";
$ClassName[6, 1] = "Hired Gun";
$ClassName[7, 1] = "Bootlegger";
$ClassName[8, 1] = "Archmage";

$ClassName[1, 2] = "Jedi Padawan";
$ClassName[2, 2] = "Gray Padawan";
$ClassName[3, 2] = "Dark Padawan";
$ClassName[4, 2] = "Private First Class II";
$ClassName[5, 2] = "Navalman II"; $ClassNameF[5, 2] = "Navalwoman II";
$ClassName[6, 2] = "Hit man";
$ClassName[7, 2] = "Scoundrel";
$ClassName[8, 2] = "Sorcerer";

$ClassName[1, 3] = "Jedi Padawan III";
$ClassName[2, 3] = "Gray Padawan III";
$ClassName[3, 3] = "Dark Padawan III";
$ClassName[4, 3] = "Lance Corporal III";
$ClassName[5, 3] = "Able Navalman III"; $ClassNameF[5, 3] = "Able Navalwoman III";
$ClassName[6, 3] = "Bounty Hunter";
$ClassName[7, 3] = "Pirate";
$ClassName[8, 3] = "Wizard";

$ClassName[1, 4]  = "Jedi Padawan IV";
$ClassName[2, 4]  = "Gray Padawan IV";
$ClassName[3, 4]  = "Dark Padawan IV";
$ClassName[4, 4]  = "Lance Corporal IV";
$ClassName[5, 4]  = "Able Navalman IV"; $ClassNameF[5, 4]  = "Able Navalwoman IV";
$ClassName[6, 4]  = "Assassin";
$ClassName[7, 4]  = "Pirate";
$ClassName[8, 4]  = "WizardIV";

$ClassName[1, 5]  = "Jedi";
$ClassName[2, 5]  = "Gray Jedi";
$ClassName[3, 5]  = "Dark Jedi";
$ClassName[4, 5]  = "Corporal V";
$ClassName[5, 5]  = "Leading Navalman V"; $ClassNameF[5, 5]  = "Leading Navalwoman V";
$ClassName[6, 5]  = "CrusaderV";
$ClassName[7, 5]  = "ArcherV";
$ClassName[8, 5]  = "WizardV";

$ClassName[1, 6]  = "Jedi VI";
$ClassName[2, 6]  = "Gray Jedi VI";
$ClassName[3, 6]  = "Dark Jedi VI";
$ClassName[4, 6]  = "Corporal VI";
$ClassName[5, 6]  = "Leading Navalman VI"; $ClassNameF[5, 6]  = "Leading Navalwoman VI";
$ClassName[6, 6]  = "CrusaderVI";
$ClassName[7, 6]  = "ArcherVI";
$ClassName[8, 6]  = "WizardVI";

$ClassName[1, 7]  = "Jedi VII";
$ClassName[2, 7]  = "Gray Jedi VII";
$ClassName[3, 7]  = "Dark Jedi VII";
$ClassName[4, 7]  = "Sergeant VII";
$ClassName[5, 7]  = "Senior Navalman VII"; $ClassNameF[5, 7]  = "Senior Navalwoman VII";
$ClassName[6, 7]  = "CrusaderVII";
$ClassName[7, 7]  = "ArcherVII";
$ClassName[8, 7]  = "WizardVII";

$ClassName[1, 8]  = "Jedi VIII";
$ClassName[2, 8]  = "Gray Jedi VIII";
$ClassName[3, 8]  = "Dark Jedi VIII";
$ClassName[4, 8]  = "Sergeant VIII";
$ClassName[5, 8]  = "Senior Navalman VIII"; $ClassNameF[5, 8]  = "Senior Navalwoman VIII";
$ClassName[6, 8]  = "CrusaderVIII";
$ClassName[7, 8]  = "ArcherVIII";
$ClassName[8, 8]  = "WizardVIII";

$ClassName[1, 9]  = "Jedi IX";
$ClassName[2, 9]  = "Gray Jedi IX";
$ClassName[3, 9]  = "Dark Jedi IX";
$ClassName[4, 9]  = "Staff Sergeant IX";
$ClassName[5, 9]  = "Petty Officer 3rd class IX";
$ClassName[6, 9]  = "CrusaderIX";
$ClassName[7, 9]  = "ArcherIX";
$ClassName[8, 9]  = "WizardIX";

$ClassName[1, 10]  = "Jedi Weapon Master";
$ClassName[2, 10]  = "Gray Jedi Deliberator";
$ClassName[3, 10]  = "Sith Weapon Master";
$ClassName[4, 10]  = "Staff Sergeant X";
$ClassName[5, 10]  = "Petty Officer 3rd class X";
$ClassName[6, 10]  = "CrusaderX";
$ClassName[7, 10]  = "ArcherX";
$ClassName[8, 10]  = "WizardX";

$ClassName[1, 11]  = "Jedi Weapon Master XI";
$ClassName[2, 11]  = "Gray Jedi Deliberator XI";
$ClassName[3, 11]  = "Sith Weapon Master XI";
$ClassName[4, 11]  = "Staff Sergeant XI";
$ClassName[5, 11]  = "Petty Officer 3rd class XI";
$ClassName[6, 11]  = "CrusaderXI";
$ClassName[7, 11]  = "ArcherXI";
$ClassName[8, 11]  = "WizardXI";

$ClassName[1, 12]  = "Jedi Weapon Master XII";
$ClassName[2, 12]  = "Gray Jedi Deliberator XII";
$ClassName[3, 12]  = "Sith Weapon Master XII";
$ClassName[4, 12]  = "Gunnery Sergeant XII";
$ClassName[5, 12]  = "Petty Officer 2nd class XII";
$ClassName[6, 12]  = "CrusaderXII";
$ClassName[7, 12]  = "ArcherXII";
$ClassName[8, 12]  = "WizardXII";

$ClassName[1, 13]  = "Jedi Weapon Master XIII";
$ClassName[2, 13]  = "Gray Jedi Deliberator XIII";
$ClassName[3, 13]  = "Sith Weapon Master XIII";
$ClassName[4, 13]  = "Gunnery Sergeant XIII";
$ClassName[5, 13]  = "Petty Officer 2nd class XIII";
$ClassName[6, 13]  = "CrusaderXIII";
$ClassName[7, 13]  = "ArcherXIII";
$ClassName[8, 13]  = "WizardXIII";

$ClassName[1, 14]  = "Jedi Weapon Master XIV";
$ClassName[2, 14]  = "Gray Jedi Deliberator XIV";
$ClassName[3, 14]  = "Sith Weapon Master XIV";
$ClassName[4, 14]  = "Gunnery Sergeant XIV";
$ClassName[5, 14]  = "Petty Officer 2nd class XIV";
$ClassName[6, 14]  = "CrusaderXIV";
$ClassName[7, 14]  = "ArcherXIV";
$ClassName[8, 14]  = "WizardXIV";

$ClassName[1, 15]  = "Jedi Sentinel";
$ClassName[2, 15]  = "Gray Jedi Sentinel";
$ClassName[3, 15]  = "Dark Jedi Sentinel";
$ClassName[4, 15]  = "Master Sergeant XV";
$ClassName[5, 15]  = "Petty Officer 1st class XV";
$ClassName[6, 15]  = "CrusaderXV";
$ClassName[7, 15]  = "ArcherXV";
$ClassName[8, 15]  = "WizardXV";

$ClassName[1, 16]  = "Jedi Sentinel XVI";
$ClassName[2, 16]  = "Gray Jedi Sentinel XVI";
$ClassName[3, 16]  = "Dark Jedi Sentinel XVI";
$ClassName[4, 16]  = "Master Sergeant XVI";
$ClassName[5, 16]  = "Petty Officer 1st class XVI";
$ClassName[6, 16]  = "CrusaderXVI";
$ClassName[7, 16]  = "ArcherXVI";
$ClassName[8, 16]  = "WizardXVI";

$ClassName[1, 17]  = "Jedi Sentinel XVII";
$ClassName[2, 17]  = "Gray Jedi Sentinel XVII";
$ClassName[3, 17]  = "Dark Jedi Sentinel XVII";
$ClassName[4, 17]  = "Master Sergeant XVII";
$ClassName[5, 17]  = "Petty Officer 1st class XVII";
$ClassName[6, 17]  = "CrusaderXVII";
$ClassName[7, 17]  = "ArcherXVII";
$ClassName[8, 17]  = "WizardXVII";

$ClassName[1, 18]  = "Jedi Sentinel XVIII";
$ClassName[2, 18]  = "Gray Jedi Sentinel XVIII";
$ClassName[3, 18]  = "Dark Jedi Sentinel XVIII";
$ClassName[4, 18]  = "First Sergeant XVIII";
$ClassName[5, 18]  = "Chief Petty Officer XVIII";
$ClassName[6, 18]  = "CrusaderXVIII";
$ClassName[7, 18]  = "ArcherXVIII";
$ClassName[8, 18]  = "WizardXVIII";

$ClassName[1, 19]  = "Jedi Sentinel XIX";
$ClassName[2, 19]  = "Gray Jedi Sentinel XIX";
$ClassName[3, 19]  = "Dark Jedi Sentinel XIX";
$ClassName[4, 19]  = "First Sergeant XIX";
$ClassName[5, 19]  = "Chief Petty Officer XIX";
$ClassName[6, 19]  = "CrusaderXIX";
$ClassName[7, 19]  = "ArcherXIX";
$ClassName[8, 19]  = "WizardXIX";

$ClassName[1, 20]  = "Jedi Knight";
$ClassName[2, 20]  = "Gray Jedi Knight";
$ClassName[3, 20]  = "Dark Jedi Knight";
$ClassName[4, 20]  = "First Sergeant XX";
$ClassName[5, 20]  = "Chief Petty Officer XX";
$ClassName[6, 20]  = "CrusaderXX";
$ClassName[7, 20]  = "ArcherXX";
$ClassName[8, 20]  = "WizardXX";

$ClassName[1, 21]  = "Jedi Knight XXI";
$ClassName[2, 21]  = "Gray Jedi Knight XXI";
$ClassName[3, 21]  = "Dark Jedi Knight XXI";
$ClassName[4, 21]  = "Master Gunnery Sergeant XXI";
$ClassName[5, 21]  = "Senior Chief Petty Officer XXI";
$ClassName[6, 21]  = "CrusaderXXI";
$ClassName[7, 21]  = "ArcherXXI";
$ClassName[8, 21]  = "WizardXXI";

$ClassName[1, 22]  = "Jedi Knight XXII";
$ClassName[2, 22]  = "Gray Jedi Knight XXII";
$ClassName[3, 22]  = "Dark Jedi Knight XXII";
$ClassName[4, 22]  = "Master Gunnery Sergeant XXII";
$ClassName[5, 22]  = "Senior Chief Petty Officer XXII";
$ClassName[6, 22]  = "CrusaderXXII";
$ClassName[7, 22]  = "ArcherXXII";
$ClassName[8, 22]  = "WizardXXII";

$ClassName[1, 23]  = "Jedi Knight XXIII";
$ClassName[2, 23]  = "Gray Jedi Knight XXIII";
$ClassName[3, 23]  = "Dark Jedi Knight XXIII";
$ClassName[4, 23]  = "Master Gunnery Sergeant XXIII";
$ClassName[5, 23]  = "Senior Chief Petty Officer XXIII";
$ClassName[6, 23]  = "CrusaderXXIII";
$ClassName[7, 23]  = "ArcherXXIII";
$ClassName[8, 23]  = "WizardXXIII";

$ClassName[1, 24]  = "Jedi Knight XXIV";
$ClassName[2, 24]  = "Gray Jedi Knight XXIV";
$ClassName[3, 24]  = "Dark Jedi Knight XXIV";
$ClassName[4, 24]  = "Senior Master Sergeant XXIV";
$ClassName[5, 24]  = "Master Chief Petty Officer XXIV";
$ClassName[6, 24]  = "CrusaderXXIV";
$ClassName[7, 24]  = "ArcherXXIV";
$ClassName[8, 24]  = "WizardXXIV";

$ClassName[1, 25]  = "Jedi Guardian";
$ClassName[2, 25]  = "Gray Jedi Atoner";
$ClassName[3, 25]  = "Sith Hunter";
$ClassName[4, 25]  = "Senior Master Sergeant XXV";
$ClassName[5, 25]  = "Master Chief Petty Officer XXV";
$ClassName[6, 25]  = "CrusaderXXV";
$ClassName[7, 25]  = "ArcherXXV";
$ClassName[8, 25]  = "WizardXXV";

$ClassName[1, 26]  = "Jedi Guardian XXVI";
$ClassName[2, 26]  = "Gray Jedi Atoner XXVI";
$ClassName[3, 26]  = "Sith Hunter XXVI";
$ClassName[4, 26]  = "Senior Master Sergeant XXVI";
$ClassName[5, 26]  = "Master Chief Petty Officer XXVI";
$ClassName[6, 26]  = "CrusaderXXVI";
$ClassName[7, 26]  = "ArcherXXVI";
$ClassName[8, 26]  = "WizardXXVI";

$ClassName[1, 27]  = "Jedi Guardian XXVII";
$ClassName[2, 27]  = "Gray Jedi Atoner XXVII";
$ClassName[3, 27]  = "Sith Hunter XXVII";
$ClassName[4, 27]  = "Chief Master Sergeant XXVII";
$ClassName[5, 27]  = "Warrant Officer 2nd class XXVII";
$ClassName[6, 27]  = "CrusaderXXVII";
$ClassName[7, 27]  = "ArcherXXVII";
$ClassName[8, 27]  = "WizardXXVII";

$ClassName[1, 28]  = "Jedi Guardian XXVIII";
$ClassName[2, 28]  = "Gray Jedi Atoner XXVIII";
$ClassName[3, 28]  = "Sith Hunter XXVIII";
$ClassName[4, 28]  = "Chief Master Sergeant XXVIII";
$ClassName[5, 28]  = "Warrant Officer 2nd class XXVIII";
$ClassName[6, 28]  = "CrusaderXXVIII";
$ClassName[7, 28]  = "ArcherXXVIII";
$ClassName[8, 28]  = "WizardXXVIII";

$ClassName[1, 29]  = "Jedi Guardian XXIX";
$ClassName[2, 29]  = "Gray Jedi Atoner XXIX";
$ClassName[3, 29]  = "Sith Hunter XXIX";
$ClassName[4, 29]  = "Chief Master Sergeant XXIX";
$ClassName[5, 29]  = "Warrant Officer 2nd class XXIX";
$ClassName[6, 29]  = "CrusaderXXIX";
$ClassName[7, 29]  = "ArcherXXIX";
$ClassName[8, 29]  = "WizardXXIX";

$ClassName[1, 30]  = "Jedi Consulor";
$ClassName[2, 30]  = "Gray Jedi Judicator";
$ClassName[3, 30]  = "Sith Marauder";
$ClassName[4, 30]  = "Chief Master Sergeant XXX";
$ClassName[5, 30]  = "Warrant Officer 2nd class XXX";
$ClassName[6, 30]  = "CrusaderXXX";
$ClassName[7, 30]  = "ArcherXXX";
$ClassName[8, 30]  = "WizardXXX";

$ClassName[1, 31]  = "Jedi Consulor XXXI";
$ClassName[2, 31]  = "Gray Jedi Judicator XXXI";
$ClassName[3, 31]  = "Sith Marauder XXXI";
$ClassName[4, 31]  = "Sergeant Major XXXI";
$ClassName[5, 31]  = "Warrant Officer 1st class XXXI";
$ClassName[6, 31]  = "CrusaderXXXI";
$ClassName[7, 31]  = "ArcherXXXI";
$ClassName[8, 31]  = "WizardXXXI";

$ClassName[1, 32]  = "Jedi Consulor XXXII";
$ClassName[2, 32]  = "Gray Jedi Judicator XXXII";
$ClassName[3, 32]  = "Sith Marauder XXXII";
$ClassName[4, 32]  = "Sergeant Major XXXII";
$ClassName[5, 32]  = "Warrant Officer 1st class XXXII";
$ClassName[6, 32]  = "CrusaderXXXII";
$ClassName[7, 32]  = "ArcherXXXII";
$ClassName[8, 32]  = "WizardXXXII";

$ClassName[1, 33]  = "Jedi Consulor XXXIII";
$ClassName[2, 33]  = "Gray Jedi Judicator XXXIII";
$ClassName[3, 33]  = "Sith Marauder XXXIII";
$ClassName[4, 33]  = "Sergeant Major XXXIII";
$ClassName[5, 33]  = "Warrant Officer 1st class XXXIII";
$ClassName[6, 33]  = "CrusaderXXXIII";
$ClassName[7, 33]  = "ArcherXXXIII";
$ClassName[8, 33]  = "WizardXXXIII";

$ClassName[1, 34]  = "Jedi Consulor XXXIV";
$ClassName[2, 34]  = "Gray Jedi Judicator XXXIV";
$ClassName[3, 34]  = "Sith Marauder XXXIV";
$ClassName[4, 34]  = "Sergeant Major XXXIV";
$ClassName[5, 34]  = "Warrant Officer 1st class XXXIV";
$ClassName[6, 34]  = "CrusaderXXXIV";
$ClassName[7, 34]  = "ArcherXXXIV";
$ClassName[8, 34]  = "WizardXXXIV";

$ClassName[1, 35]  = "Jedi Watchman";
$ClassName[2, 35]  = "Gray Jedi Judicator";
$ClassName[3, 35]  = "Sith Assassin";
$ClassName[4, 35]  = "Officer Cadet XXXV";
$ClassName[5, 35]  = "Midshipman XXXV";
$ClassName[6, 35]  = "CrusaderXXXV";
$ClassName[7, 35]  = "ArcherXXXV";
$ClassName[8, 35]  = "WizardXXXV";

$ClassName[1, 36]  = "Jedi Watchman XXXVI";
$ClassName[2, 36]  = "Gray Jedi Judicator XXXVI";
$ClassName[3, 36]  = "Sith Assassin XXXVI";
$ClassName[4, 36]  = "Officer Cadet XXXVI";
$ClassName[5, 36]  = "Midshipman XXXVI";
$ClassName[6, 36]  = "CrusaderXXXVI";
$ClassName[7, 36]  = "ArcherXXXVI";
$ClassName[8, 36]  = "WizardXXXVI";

$ClassName[1, 37]  = "Jedi Watchman XXXVII";
$ClassName[2, 37]  = "Gray Jedi Judicator XXXVII";
$ClassName[3, 37]  = "Sith Assassin XXXVII";
$ClassName[4, 37]  = "Officer Cadet XXXVII";
$ClassName[5, 37]  = "Midshipman XXXVII";
$ClassName[6, 37]  = "CrusaderXXXVII";
$ClassName[7, 37]  = "ArcherXXXVII";
$ClassName[8, 37]  = "WizardXXXVII";

$ClassName[1, 38]  = "Jedi Watchman XXXVIII";
$ClassName[2, 38]  = "Gray Jedi Judicator XXXVIII";
$ClassName[3, 38]  = "Sith Assassin XXXVIII";
$ClassName[4, 38]  = "Officer Cadet XXXVIII";
$ClassName[5, 38]  = "Midshipman XXXVIII";
$ClassName[6, 38]  = "CrusaderXXXVIII";
$ClassName[7, 38]  = "ArcherXXXVIII";
$ClassName[8, 38]  = "WizardXXXVIII";

$ClassName[1, 39]  = "Jedi Watchman XXXIX";
$ClassName[2, 39]  = "Gray Jedi Judicator XXXIX";
$ClassName[3, 39]  = "Sith Assassin XXXIX";
$ClassName[4, 39]  = "Second Lieutenant XXXIX";
$ClassName[5, 39]  = "Ensign XXXIX";
$ClassName[6, 39]  = "CrusaderXXXIX";
$ClassName[7, 39]  = "ArcherXXXIX";
$ClassName[8, 39]  = "WizardXXXIX";

$ClassName[1, 40]  = "Jedi Master";
$ClassName[2, 40]  = "Head MysticXL";
$ClassName[3, 40]  = "Sith Lord";
$ClassName[4, 40]  = "Second Lieutenant XL";
$ClassName[5, 40]  = "Ensign XL";
$ClassName[6, 40]  = "CrusaderXL";
$ClassName[7, 40]  = "ArcherXL";
$ClassName[8, 40]  = "WizardXL";

$ClassName[1, 41]  = "Jedi Master XLI";
$ClassName[2, 41]  = "Head MysticXLI";
$ClassName[3, 41]  = "Sith Lord XLI";
$ClassName[4, 41]  = "Second Lieutenant XLI";
$ClassName[5, 41]  = "Ensign XLI";
$ClassName[6, 41]  = "CrusaderXLI";
$ClassName[7, 41]  = "ArcherXLI";
$ClassName[8, 41]  = "WizardXLI";

$ClassName[1, 42]  = "Jedi Master XLII";
$ClassName[2, 42]  = "Head MysticXLII";
$ClassName[3, 42]  = "Sith Lord XLII";
$ClassName[4, 42]  = "Second Lieutenant XLII";
$ClassName[5, 42]  = "Ensign XLII";
$ClassName[6, 42]  = "CrusaderXLII";
$ClassName[7, 42]  = "ArcherXLII";
$ClassName[8, 42]  = "WizardXLII";

$ClassName[1, 43]  = "Jedi Master XLIII";
$ClassName[2, 43]  = "Head MysticXLIII";
$ClassName[3, 43]  = "Sith Lord XLIII";
$ClassName[4, 43]  = "First Lieutenant XLIII";
$ClassName[5, 43]  = "Sub-Lieutenant XLIII";
$ClassName[6, 43]  = "CrusaderXLIII";
$ClassName[7, 43]  = "ArcherXLIII";
$ClassName[8, 43]  = "WizardXLIII";

$ClassName[1, 44]  = "Jedi Master XLIV";
$ClassName[2, 44]  = "Head MysticXLIV";
$ClassName[3, 44]  = "Sith Lord XLIV";
$ClassName[4, 44]  = "First Lieutenant XLIV";
$ClassName[5, 44]  = "Sub-Lieutenant XLIV";
$ClassName[6, 44]  = "CrusaderXLIV";
$ClassName[7, 44]  = "ArcherXLIV";
$ClassName[8, 44]  = "WizardXLIV";

$ClassName[1, 45]  = "PopeXLV";
$ClassName[2, 45]  = "Head MysticXLV";
$ClassName[3, 45]  = "KleptoXLV";
$ClassName[4, 45]  = "First Lieutenant XLV";
$ClassName[5, 45]  = "Sub-Lieutenant XLV";
$ClassName[6, 45]  = "CrusaderXLV";
$ClassName[7, 45]  = "ArcherXLV";
$ClassName[8, 45]  = "WizardXLV";

$ClassName[1, 46]  = "PopeXLVI";
$ClassName[2, 46]  = "Head MysticXLVI";
$ClassName[3, 46]  = "KleptoXLVI";
$ClassName[4, 46]  = "First Lieutenant XLVI";
$ClassName[5, 46]  = "Sub-Lieutenant XLVI";
$ClassName[6, 46]  = "CrusaderXLVI";
$ClassName[7, 46]  = "ArcherXLVI";
$ClassName[8, 46]  = "WizardXLVI";

$ClassName[1, 47]  = "PopeXLVII";
$ClassName[2, 47]  = "Head MysticXLVII";
$ClassName[3, 47]  = "KleptoXLVII";
$ClassName[4, 47]  = "Captain XLVII";
$ClassName[5, 47]  = "Lieutenant XLVII";
$ClassName[6, 47]  = "CrusaderXLVII";
$ClassName[7, 47]  = "ArcherXLVII";
$ClassName[8, 47]  = "WizardXLVII";

$ClassName[1, 48]  = "PopeXLVIII";
$ClassName[2, 48]  = "Head MysticXLVIII";
$ClassName[3, 48]  = "KleptoXLVIII";
$ClassName[4, 48]  = "Captain XLVIII";
$ClassName[5, 48]  = "Lieutenant XLVIII";
$ClassName[6, 48]  = "CrusaderXLVIII";
$ClassName[7, 48]  = "ArcherXLVIII";
$ClassName[8, 48]  = "WizardXLVIII";

$ClassName[1, 49]  = "PopeXLIX";
$ClassName[2, 49]  = "Head MysticXLIX";
$ClassName[3, 49]  = "KleptoXLIX";
$ClassName[4, 49]  = "Captain XLIX";
$ClassName[5, 49]  = "Lieutenant XLIX";
$ClassName[6, 49]  = "CrusaderXLIX";
$ClassName[7, 49]  = "ArcherXLIX";
$ClassName[8, 49]  = "WizardXLIX";

$ClassName[1, 50]  = "PopeL";
$ClassName[2, 50]  = "Head MysticL";
$ClassName[3, 50]  = "KleptoL";
$ClassName[4, 50]  = "Captain L";
$ClassName[5, 50]  = "Lieutenant L";
$ClassName[6, 50]  = "CrusaderL";
$ClassName[7, 50]  = "ArcherL";
$ClassName[8, 50]  = "WizardL";

$ClassName[1, 51]  = "PopeLI";
$ClassName[2, 51]  = "Head MysticLI";
$ClassName[3, 51]  = "KleptoLI";
$ClassName[4, 51]  = "Major LI";
$ClassName[5, 51]  = "Lieutenant Commander LI";
$ClassName[6, 51]  = "CrusaderLI";
$ClassName[7, 51]  = "ArcherLI";
$ClassName[8, 51]  = "WizardLI";

$ClassName[1, 52]  = "PopeLII";
$ClassName[2, 52]  = "Head MysticLII";
$ClassName[3, 52]  = "KleptoLII";
$ClassName[4, 52]  = "Major LII";
$ClassName[5, 52]  = "Lieutenant Commander LII";
$ClassName[6, 52]  = "CrusaderLII";
$ClassName[7, 52]  = "ArcherLII";
$ClassName[8, 52]  = "WizardLII";

$ClassName[1, 53]  = "PopeLIII";
$ClassName[2, 53]  = "Head MysticLIII";
$ClassName[3, 53]  = "KleptoLIII";
$ClassName[4, 53]  = "Major LIII";
$ClassName[5, 53]  = "Lieutenant Commander LIII";
$ClassName[6, 53]  = "CrusaderLIII";
$ClassName[7, 53]  = "ArcherLIII";
$ClassName[8, 53]  = "WizardLIII";

$ClassName[1, 54]  = "PopeLIV";
$ClassName[2, 54]  = "Head MysticLIV";
$ClassName[3, 54]  = "KleptoLIV";
$ClassName[4, 54]  = "Major LIV";
$ClassName[5, 54]  = "Lieutenant Commander LIV";
$ClassName[6, 54]  = "CrusaderLIV";
$ClassName[7, 54]  = "ArcherLIV";
$ClassName[8, 54]  = "WizardLIV";

$ClassName[1, 55]  = "PopeLV";
$ClassName[2, 55]  = "Head MysticLV";
$ClassName[3, 55]  = "KleptoLV";
$ClassName[4, 55]  = "Lieutenant Colonel LV";
$ClassName[5, 55]  = "Commander LV";
$ClassName[6, 55]  = "CrusaderLV";
$ClassName[7, 55]  = "ArcherLV";
$ClassName[8, 55]  = "WizardLV";

$ClassName[1, 56]  = "PopeLVI";
$ClassName[2, 56]  = "Head MysticLVI";
$ClassName[3, 56]  = "KleptoLVI";
$ClassName[4, 56]  = "Lieutenant Colonel LVI";
$ClassName[5, 56]  = "Commander LVI";
$ClassName[6, 56]  = "CrusaderLVI";
$ClassName[7, 56]  = "ArcherLVI";
$ClassName[8, 56]  = "WizardLVI";

$ClassName[1, 57]  = "PopeLVII";
$ClassName[2, 57]  = "Head MysticLVII";
$ClassName[3, 57]  = "KleptoLVII";
$ClassName[4, 57]  = "Lieutenant Colonel LVII";
$ClassName[5, 57]  = "Commander LVII";
$ClassName[6, 57]  = "CrusaderLVII";
$ClassName[7, 57]  = "ArcherLVII";
$ClassName[8, 57]  = "WizardLVII";

$ClassName[1, 58]  = "PopeLVIII";
$ClassName[2, 58]  = "Head MysticLVIII";
$ClassName[3, 58]  = "KleptoLVIII";
$ClassName[4, 58]  = "Lieutenant Colonel LVIII";
$ClassName[5, 58]  = "Commander LVIII";
$ClassName[6, 58]  = "CrusaderLVIII";
$ClassName[7, 58]  = "ArcherLVIII";
$ClassName[8, 58]  = "WizardLVIII";

$ClassName[1, 59]  = "PopeLIX";
$ClassName[2, 59]  = "Head MysticLIX";
$ClassName[3, 59]  = "KleptoLIX";
$ClassName[4, 59]  = "Colonel LIX";
$ClassName[5, 59]  = "Captain LIX";
$ClassName[6, 59]  = "CrusaderLIX";
$ClassName[7, 59]  = "ArcherLIX";
$ClassName[8, 59]  = "WizardLIX";

$ClassName[1, 60]  = "PopeLX";
$ClassName[2, 60]  = "Head MysticLX";
$ClassName[3, 60]  = "KleptoLX";
$ClassName[4, 60]  = "Colonel LX";
$ClassName[5, 60]  = "Captain LX";
$ClassName[6, 60]  = "CrusaderLX";
$ClassName[7, 60]  = "ArcherLX";
$ClassName[8, 60]  = "WizardLX";

$ClassName[1, 61]  = "PopeLXI";
$ClassName[2, 61]  = "Head MysticLXI";
$ClassName[3, 61]  = "KleptoLXI";
$ClassName[4, 61]  = "Colonel LXI";
$ClassName[5, 61]  = "Captain LXI";
$ClassName[6, 61]  = "CrusaderLXI";
$ClassName[7, 61]  = "ArcherLXI";
$ClassName[8, 61]  = "WizardLXI";

$ClassName[1, 62]  = "PopeLXII";
$ClassName[2, 62]  = "Head MysticLXII";
$ClassName[3, 62]  = "KleptoLXII";
$ClassName[4, 62]  = "Colonel LXII";
$ClassName[5, 62]  = "Captain LXII";
$ClassName[6, 62]  = "CrusaderLXII";
$ClassName[7, 62]  = "ArcherLXII";
$ClassName[8, 62]  = "WizardLXII";

$ClassName[1, 63]  = "PopeLXIII";
$ClassName[2, 63]  = "Head MysticLXIII";
$ClassName[3, 63]  = "KleptoLXIII";
$ClassName[4, 63]  = "Colonel LXIII";
$ClassName[5, 63]  = "Captain LXIII";
$ClassName[6, 63]  = "CrusaderLXIII";
$ClassName[7, 63]  = "ArcherLXIII";
$ClassName[8, 63]  = "WizardLXIII";

$ClassName[1, 64]  = "PopeLXIV";
$ClassName[2, 64]  = "Head MysticLXIV";
$ClassName[3, 64]  = "KleptoLXIV";
$ClassName[4, 64]  = "Brigadier General LXIV";
$ClassName[5, 64]  = "Commodore LXIV";
$ClassName[6, 64]  = "CrusaderLXIV";
$ClassName[7, 64]  = "ArcherLXIV";
$ClassName[8, 64]  = "WizardLXIV";

$ClassName[1, 65]  = "PopeLXV";
$ClassName[2, 65]  = "Head MysticLXV";
$ClassName[3, 65]  = "KleptoLXV";
$ClassName[4, 65]  = "Brigadier General LXV";
$ClassName[5, 65]  = "Commodore LXV";
$ClassName[6, 65]  = "CrusaderLXV";
$ClassName[7, 65]  = "ArcherLXV";
$ClassName[8, 65]  = "WizardLXV";

$ClassName[1, 66]  = "PopeLXVI";
$ClassName[2, 66]  = "Head MysticLXVI";
$ClassName[3, 66]  = "KleptoLXVI";
$ClassName[4, 66]  = "Brigadier General LXVI";
$ClassName[5, 66]  = "Commodore LXVI";
$ClassName[6, 66]  = "CrusaderLXVI";
$ClassName[7, 66]  = "ArcherLXVI";
$ClassName[8, 66]  = "WizardLXVI";

$ClassName[1, 67]  = "PopeLXVII";
$ClassName[2, 67]  = "Head MysticLXVII";
$ClassName[3, 67]  = "KleptoLXVII";
$ClassName[4, 67]  = "Brigadier General LXVII";
$ClassName[5, 67]  = "Commodore LXVII";
$ClassName[6, 67]  = "CrusaderLXVII";
$ClassName[7, 67]  = "ArcherLXVII";
$ClassName[8, 67]  = "WizardLXVII";

$ClassName[1, 68]  = "PopeLXVIII";
$ClassName[2, 68]  = "Head MysticLXVIII";
$ClassName[3, 68]  = "KleptoLXVIII";
$ClassName[4, 68]  = "Brigadier General LXVIII";
$ClassName[5, 68]  = "Commodore LXVIII";
$ClassName[6, 68]  = "CrusaderLXVIII";
$ClassName[7, 68]  = "ArcherLXVIII";
$ClassName[8, 68]  = "WizardLXVIII";

$ClassName[1, 69]  = "PopeLXIX";
$ClassName[2, 69]  = "Head MysticLXIX";
$ClassName[3, 69]  = "KleptoLXIX";
$ClassName[4, 69]  = "Brigadier General LXIX";
$ClassName[5, 69]  = "Commodore LXIX";
$ClassName[6, 69]  = "CrusaderLXIX";
$ClassName[7, 69]  = "ArcherLXIX";
$ClassName[8, 69]  = "WizardLXIX";

$ClassName[1, 70]  = "PopeLXX";
$ClassName[2, 70]  = "Head MysticLXX";
$ClassName[3, 70]  = "KleptoLXX";
$ClassName[4, 70]  = "Major General LXX";
$ClassName[5, 70]  = "Rear Admiral LXX";
$ClassName[6, 70]  = "CrusaderLXX";
$ClassName[7, 70]  = "ArcherLXX";
$ClassName[8, 70]  = "WizardLXX";
	
$ClassName[1, 71]  = "PopeLXXI";
$ClassName[2, 71]  = "Head MysticLXXI";
$ClassName[3, 71]  = "KleptoLXXI";
$ClassName[4, 71]  = "Major General LXXI";
$ClassName[5, 71]  = "Rear Admiral LXXI";
$ClassName[6, 71]  = "CrusaderLXXI";
$ClassName[7, 71]  = "ArcherLXXI";
$ClassName[8, 71]  = "WizardLXXI";
	
$ClassName[1, 72]  = "PopeLXXII";
$ClassName[2, 72]  = "Head MysticLXXII";
$ClassName[3, 72]  = "KleptoLXXII";
$ClassName[4, 72]  = "Major General LXXII";
$ClassName[5, 72]  = "Rear Admiral LXXII";
$ClassName[6, 72]  = "CrusaderLXXII";
$ClassName[7, 72]  = "ArcherLXXII";
$ClassName[8, 72]  = "WizardLXXII";
	
$ClassName[1, 73]  = "PopeLXXIII";
$ClassName[2, 73]  = "Head MysticLXXIII";
$ClassName[3, 73]  = "KleptoLXXIII";
$ClassName[4, 73]  = "Major General LXXIII";
$ClassName[5, 73]  = "Rear Admiral LXXIII";
$ClassName[6, 73]  = "CrusaderLXXIII";
$ClassName[7, 73]  = "ArcherLXXIII";
$ClassName[8, 73]  = "WizardLXXIII";
	
$ClassName[1, 74]  = "PopeLXXIV";
$ClassName[2, 74]  = "Head MysticLXXIV";
$ClassName[3, 74]  = "KleptoLXXIV";
$ClassName[4, 74]  = "Major General LXXIV";
$ClassName[5, 74]  = "Rear Admiral LXXIV";
$ClassName[6, 74]  = "CrusaderLXXIV";
$ClassName[7, 74]  = "ArcherLXXIV";
$ClassName[8, 74]  = "WizardLXXIV";
	
$ClassName[1, 75]  = "PopeLXXV";
$ClassName[2, 75]  = "Head MysticLXXV";
$ClassName[3, 75]  = "KleptoLXXV";
$ClassName[4, 75]  = "Major General LXXV";
$ClassName[5, 75]  = "Rear Admiral LXXV";
$ClassName[6, 75]  = "CrusaderLXXV";
$ClassName[7, 75]  = "ArcherLXXV";
$ClassName[8, 75]  = "WizardLXXV";
	
$ClassName[1, 76]  = "PopeLXXVI";
$ClassName[2, 76]  = "Head MysticLXXVI";
$ClassName[3, 76]  = "KleptoLXXVI";
$ClassName[4, 76]  = "Major General LXXVI";
$ClassName[5, 76]  = "Rear Admiral LXXVI";
$ClassName[6, 76]  = "CrusaderLXXVI";
$ClassName[7, 76]  = "ArcherLXXVI";
$ClassName[8, 76]  = "WizardLXXVI";
	
$ClassName[1, 77]  = "PopeLXXVII";
$ClassName[2, 77]  = "Head MysticLXXVII";
$ClassName[3, 77]  = "KleptoLXXVII";
$ClassName[4, 77]  = "Major General LXXVII";
$ClassName[5, 77]  = "Rear Admiral LXXVII";
$ClassName[6, 77]  = "CrusaderLXXVII";
$ClassName[7, 77]  = "ArcherLXXVII";
$ClassName[8, 77]  = "WizardLXXVII";
	
$ClassName[1, 78]  = "PopeLXXVIII";
$ClassName[2, 78]  = "Head MysticLXXVIII";
$ClassName[3, 78]  = "KleptoLXXVIII";
$ClassName[4, 78]  = "Lieutenant General LXXVIII";
$ClassName[5, 78]  = "Vice-Admiral LXXVIII";
$ClassName[6, 78]  = "CrusaderLXXVIII";
$ClassName[7, 78]  = "ArcherLXXVIII";
$ClassName[8, 78]  = "WizardLXXVIII";
	
$ClassName[1, 79]  = "PopeLXXIX";
$ClassName[2, 79]  = "Head MysticLXXIX";
$ClassName[3, 79]  = "KleptoLXXIX";
$ClassName[4, 79]  = "Lieutenant General LXXIX";
$ClassName[5, 79]  = "Vice-Admiral LXXIX";
$ClassName[6, 79]  = "CrusaderLXXIX";
$ClassName[7, 79]  = "ArcherLXXIX";
$ClassName[8, 79]  = "WizardLXXIX";
	
$ClassName[1, 80]  = "PopeLXXX";
$ClassName[2, 80]  = "Head MysticLXXX";
$ClassName[3, 80]  = "KleptoLXXX";
$ClassName[4, 80]  = "Lieutenant General LXXX";
$ClassName[5, 80]  = "Vice-Admiral LXXX";
$ClassName[6, 80]  = "CrusaderLXXX";
$ClassName[7, 80]  = "ArcherLXXX";
$ClassName[8, 80]  = "WizardLXXX";
	
$ClassName[1, 81]  = "PopeLXXXI";
$ClassName[2, 81]  = "Head MysticLXXXI";
$ClassName[3, 81]  = "KleptoLXXXI";
$ClassName[4, 81]  = "Lieutenant General LXXXI";
$ClassName[5, 81]  = "Vice-Admiral LXXXI";
$ClassName[6, 81]  = "CrusaderLXXXI";
$ClassName[7, 81]  = "ArcherLXXXI";
$ClassName[8, 81]  = "WizardLXXXI";
	
$ClassName[1, 82]  = "PopeLXXXII";
$ClassName[2, 82]  = "Head MysticLXXXII";
$ClassName[3, 82]  = "KleptoLXXXII";
$ClassName[4, 82]  = "Lieutenant General LXXXII";
$ClassName[5, 82]  = "Vice-Admiral LXXXII";
$ClassName[6, 82]  = "CrusaderLXXXII";
$ClassName[7, 82]  = "ArcherLXXXII";
$ClassName[8, 82]  = "WizardLXXXII";
	
$ClassName[1, 83]  = "PopeLXXXIII";
$ClassName[2, 83]  = "Head MysticLXXXIII";
$ClassName[3, 83]  = "KleptoLXXXIII";
$ClassName[4, 83]  = "Lieutenant General LXXXIII";
$ClassName[5, 83]  = "Vice-Admiral LXXXIII";
$ClassName[6, 83]  = "CrusaderLXXXIII";
$ClassName[7, 83]  = "ArcherLXXXIII";
$ClassName[8, 83]  = "WizardLXXXIII";
	
$ClassName[1, 84]  = "PopeLXXXIV";
$ClassName[2, 84]  = "Head MysticLXXXIV";
$ClassName[3, 84]  = "KleptoLXXXIV";
$ClassName[4, 84]  = "Lieutenant General LXXXIV";
$ClassName[5, 84]  = "Vice-Admiral LXXXIV";
$ClassName[6, 84]  = "CrusaderLXXXIV";
$ClassName[7, 84]  = "ArcherLXXXIV";
$ClassName[8, 84]  = "WizardLXXXIV";
	
$ClassName[1, 85]  = "PopeLXXXV";
$ClassName[2, 85]  = "Head MysticLXXXV";
$ClassName[3, 85]  = "KleptoLXXXV";
$ClassName[4, 85]  = "Lieutenant General LXXXV";
$ClassName[5, 85]  = "Vice-Admiral LXXXV";
$ClassName[6, 85]  = "CrusaderLXXXV";
$ClassName[7, 85]  = "ArcherLXXXV";
$ClassName[8, 85]  = "WizardLXXXV";
	
$ClassName[1, 86]  = "PopeLXXXVI";
$ClassName[2, 86]  = "Head MysticLXXXVI";
$ClassName[3, 86]  = "KleptoLXXXVI";
$ClassName[4, 86]  = "Lieutenant General LXXXVI";
$ClassName[5, 86]  = "Vice-Admiral LXXXVI";
$ClassName[6, 86]  = "CrusaderLXXXVI";
$ClassName[7, 86]  = "ArcherLXXXVI";
$ClassName[8, 86]  = "WizardLXXXVI";
	
$ClassName[1, 87]  = "PopeLXXXVII";
$ClassName[2, 87]  = "Head MysticLXXXVII";
$ClassName[3, 87]  = "KleptoLXXXVII";
$ClassName[4, 87]  = "Lieutenant General LXXXVII";
$ClassName[5, 87]  = "Vice-Admiral LXXXVII";
$ClassName[6, 87]  = "CrusaderLXXXVII";
$ClassName[7, 87]  = "ArcherLXXXVII";
$ClassName[8, 87]  = "WizardLXXXVII";
	
$ClassName[1, 88]  = "PopeLXXXVIII";
$ClassName[2, 88]  = "Head MysticLXXXVIII";
$ClassName[3, 88]  = "KleptoLXXXVIII";
$ClassName[4, 88]  = "General LXXXVIII";
$ClassName[5, 88]  = "Admiral LXXXVIII";
$ClassName[6, 88]  = "CrusaderLXXXVIII";
$ClassName[7, 88]  = "ArcherLXXXVIII";
$ClassName[8, 88]  = "WizardLXXXVIII";
	
$ClassName[1, 89]  = "PopeLXXXIX";
$ClassName[2, 89]  = "Head MysticLXXXIX";
$ClassName[3, 89]  = "KleptoLXXXIX";
$ClassName[4, 89]  = "General LXXXIX";
$ClassName[5, 89]  = "Admiral LXXXIX";
$ClassName[6, 89]  = "CrusaderLXXXIX";
$ClassName[7, 89]  = "ArcherLXXXIX";
$ClassName[8, 89]  = "WizardLXXXIX";
	
$ClassName[1, 90]  = "PopeXC";
$ClassName[2, 90]  = "Head MysticXC";
$ClassName[3, 90]  = "KleptoXC";
$ClassName[4, 90]  = "General XC";
$ClassName[5, 90]  = "Admiral XC";
$ClassName[6, 90]  = "CrusaderXC";
$ClassName[7, 90]  = "ArcherXC";
$ClassName[8, 90]  = "WizardXC";
	
$ClassName[1, 91]  = "PopeXCI";
$ClassName[2, 91]  = "Head MysticXCI";
$ClassName[3, 91]  = "KleptoXCI";
$ClassName[4, 91]  = "General XCI";
$ClassName[5, 91]  = "Admiral XCI";
$ClassName[6, 91]  = "CrusaderXCI";
$ClassName[7, 91]  = "ArcherXCI";
$ClassName[8, 91]  = "WizardXCI";
	
$ClassName[1, 92]  = "PopeXCII";
$ClassName[2, 92]  = "Head MysticXCII";
$ClassName[3, 92]  = "KleptoXCII";
$ClassName[4, 92]  = "General XCII";
$ClassName[5, 92]  = "Admiral XCII";
$ClassName[6, 92]  = "CrusaderXCII";
$ClassName[7, 92]  = "ArcherXCII";
$ClassName[8, 92]  = "WizardXCII";
	
$ClassName[1, 93]  = "PopeXCIII";
$ClassName[2, 93]  = "Head MysticXCIII";
$ClassName[3, 93]  = "KleptoXCIII";
$ClassName[4, 93]  = "General XCIII";
$ClassName[5, 93]  = "Admiral XCIII";
$ClassName[6, 93]  = "CrusaderXCIII";
$ClassName[7, 93]  = "ArcherXCIII";
$ClassName[8, 93]  = "WizardXCIII";
	
$ClassName[1, 94]  = "PopeXCIV";
$ClassName[2, 94]  = "Head MysticXCIV";
$ClassName[3, 94]  = "KleptoXCIV";
$ClassName[4, 94]  = "General XCIV";
$ClassName[5, 94]  = "Admiral XCIV";
$ClassName[6, 94]  = "CrusaderXCIV";
$ClassName[7, 94]  = "ArcherXCIV";
$ClassName[8, 94]  = "WizardXCIV";
	
$ClassName[1, 95]  = "PopeXCV";
$ClassName[2, 95]  = "Head MysticXCV";
$ClassName[3, 95]  = "KleptoXCV";
$ClassName[4, 95]  = "General XCV";
$ClassName[5, 95]  = "Admiral XCV";
$ClassName[6, 95]  = "CrusaderXCV";
$ClassName[7, 95]  = "ArcherXCV";
$ClassName[8, 95]  = "WizardXCV";
	
$ClassName[1, 96]  = "PopeXCVI";
$ClassName[2, 96]  = "Head MysticXCVI";
$ClassName[3, 96]  = "KleptoXCVI";
$ClassName[4, 96]  = "General XCVI";
$ClassName[5, 96]  = "Admiral XCVI";
$ClassName[6, 96]  = "CrusaderXCVI";
$ClassName[7, 96]  = "ArcherXCVI";
$ClassName[8, 96]  = "WizardXCVI";
	
$ClassName[1, 97]  = "PopeXCVII";
$ClassName[2, 97]  = "Head MysticXCVII";
$ClassName[3, 97]  = "KleptoXCVII";
$ClassName[4, 97]  = "General XCVII";
$ClassName[5, 97]  = "Admiral XCVII";
$ClassName[6, 97]  = "CrusaderXCVII";
$ClassName[7, 97]  = "ArcherXCVII";
$ClassName[8, 97]  = "WizardXCVII";
	
$ClassName[1, 98]  = "PopeXCVIII";
$ClassName[2, 98]  = "Head MysticXCVIII";
$ClassName[3, 98]  = "KleptoXCVIII";
$ClassName[4, 98]  = "General XCVIII";
$ClassName[5, 98]  = "Admiral XCVIII";
$ClassName[6, 98]  = "CrusaderXCVIII";
$ClassName[7, 98]  = "ArcherXCVIII";
$ClassName[8, 98]  = "WizardXCVIII";
	
$ClassName[1, 99]  = "PopeXCIX";
$ClassName[2, 99]  = "Head MysticXCIX";
$ClassName[3, 99]  = "KleptoXCIX";
$ClassName[4, 99]  = "General XCIX";
$ClassName[5, 99]  = "Admiral XCIX";
$ClassName[6, 99]  = "CrusaderXCIX";
$ClassName[7, 99]  = "ArcherXCIX";
$ClassName[8, 99]  = "WizardXCIX";
	
$ClassName[1, 100]  = "God of Justice";
$ClassName[2, 100]  = "God of Truth";
$ClassName[3, 100]  = "God of Darkness";
$ClassName[4, 100]  = "Commandant of the Marines";
$ClassName[5, 100]  = "Admiral of the Fleet";
$ClassName[6, 100]  = "God of Wealth";
$ClassName[7, 100]  = "God of Indulgence";
$ClassName[8, 100]  = "God of War";

function getFinalCLASS(%clientId)
{
	dbecho($dbechoMode, "getFinalCLASS(" @ %clientId @ ")");

	%gender = Client::getGender(%clientId);
	%rl = fetchData(%clientId, "RemortStep");
	if(%rl <= 99)
	{
	   for(%i = 1; $ClassName[%i, 0] != ""; %i++)
	     {
		if(String::ICompare($ClassName[%i, 0], fetchData(%clientId, "CLASS")) == 0)
		   {
			if(%gender == "female" && $ClassnameF[%i, %rl] != "")
			   return $ClassNameF[%i, %rl];  
			else
			   return $ClassName[%i, %rl];
			break;
		   }
	     }
	}
	else if(%rl > 99)
	{
		if(%gender == "female" && $ClassnameF[%i, 100] != "")
		   return $ClassNameF[%i, 100];
		else
		   return $ClassName[%i, 100];
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