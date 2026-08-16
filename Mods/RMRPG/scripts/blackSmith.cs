function BlackSmith::ModStrings() {

deleteVariables("Smith::Stuff*");
deleteVariables("Smith::Mod::*");
deleteVariables("RM_BLACK_SMITH_ITEM*");

$ServerClient = 2048;
// SmithCombo($ServerClient, "");

//  = "Brass Copper Bronze Silver Gold Iron Steel Platinum Mythril Titanium";

%i = 0;
$Smith::Stuff[%i++] = "Ring"; // ACCESSORY TYPES

$Smith::Stuff[%i++] = "Robe";

$Smith::Stuff[%i++] = "Suit";
$Smith::Stuff[%i++] = "Gear";
$Smith::Stuff[%i++] = "Outfit";

$Smith::Stuff[%i++] = "Armor";
$Smith::Stuff[%i++] = "Mail";
$Smith::Stuff[%i++] = "ChainMail";
$Smith::Stuff[%i++] = "RingMail";
$Smith::Stuff[%i++] = "Plate";
$Smith::Stuff[%i++] = "PlateMail";

$Smith::Stuff[%i++] = "Boots";
$Smith::Stuff[%i++] = "Back";

$Smith::Stuff[%i++] = "Buckler";
$Smith::Stuff[%i++] = "Shield";
$Smith::Stuff[%i++] = "HeavyShield";

$Smith::Stuff[%i++] = "Talisman";
$Smith::Stuff[%i++] = "Pendant";
$Smith::Stuff[%i++] = "Necklace";

$Smith::Stuff[%i++] = "Dagger";
$Smith::Stuff[%i++] = "ShortSword";
$Smith::Stuff[%i++] = "Sword";
$Smith::Stuff[%i++] = "LongSword";
$Smith::Stuff[%i++] = "Katana";
$Smith::Stuff[%i++] = "Saber";

$Smith::Stuff[%i++] = "Hatchet";
$Smith::Stuff[%i++] = "Axe";
$Smith::Stuff[%i++] = "Baridache";
$Smith::Stuff[%i++] = "BattleAxe";

$Smith::Stuff[%i++] = "Staff";
$Smith::Stuff[%i++] = "Rod";
$Smith::Stuff[%i++] = "Trident";
$Smith::Stuff[%i++] = "Spear";
$Smith::Stuff[%i++] = "Tip";

$Smith::Stuff[%i++] = "Club";
$Smith::Stuff[%i++] = "Mace";
$Smith::Stuff[%i++] = "Hammer";
$Smith::Stuff[%i++] = "Mallet";

$Smith::Stuff[%i++] = "Sling";
$Smith::Stuff[%i++] = "ShortBow";
$Smith::Stuff[%i++] = "LongBow";
$Smith::Stuff[%i++] = "CrossBow";
$Smith::Stuff[%i++] = "HeavyCrossBow";
$Smith::Stuff[%i++] = "CompBow";

$Smith::Stuff[%i++] = "Arrow";
$Smith::Stuff[%i++] = "Quarrel";

$Smith::Stuff[%i++] = "Helmet";
$Smith::Stuff[%i++] = "Hat";

$Smith::Stuff[%i++] = "Gauntlets";
$Smith::Stuff[%i++] = "Gloves";
$Smith::Stuff[%i++] = "Bracelet";

$Smith::Stuff[%i++] = "Pants";
$Smith::Stuff[%i++] = "Leggings";

$Smith::Stuff[%i++] = "Orb";

##
%i="";
##

//Material
$Smith::Stuff[100] = "Metal"; // So players can make there own stuff
$Smith::Stuff[101] = "Wood";
$Smith::Stuff[102] = "Rock";
$Smith::Stuff[103] = "Fabric";


##################################################################################
## Do not edit ##
// Build us a ref list
function BuildSmithRefList() {

	%ii = 0;
	for(%i = 1; $Smith::Stuff[%i] != "" && %i < 100; %i++) {

		eval("$Smith::Stuff[\""@$Smith::Stuff[%i]@"\"] = %i;");

		%stufflist = %stufflist@"Shape:_"@$Smith::Stuff[%i]@" ";

		if(%i % 10 == 0) {
			%ii++;
			$Smith::StuffToSend[%ii] = %stufflist;
			%stufflist = "";
		}
	}

	for(%i = 100; $Smith::Stuff[%i] != ""; %i++) {
		eval("$Smith::Stuff[\""@$Smith::Stuff[%i]@"\"] = %i;");
	}

} BuildSmithRefList();

##################################################################################


##################################################################################
%i="";##################### ACCESSORY TYPES #############################################
##################################################################################

$Smith::Mod[Ring] = "Ring";
$Smith::Mod::DEF[Ring] = "0.1";
//$Smith::Mod::ATK[Ring] = "1";
//$Smith::Mod::MDEF[Ring] = "1";
//$Smith::Mod::STR[Ring] = "1";
//$Smith::Mod::DEX[Ring] = "1";
//$Smith::Mod::CON[Ring] = "1";
//$Smith::Mod::INT[Ring] = "1";
//$Smith::Mod::WIS[Ring] = "1";
//$Smith::Mod::HP[Ring] = "1";
//$Smith::Mod::MP[Ring] = "1";
$Smith::Mod::ForceWeight[Ring] = "0.1";
$Smith::Mod::Type[Ring] = $RingAccessoryType;
$Smith::Mod::Stuff[Ring, Metal] = 100; //in percent
$Smith::Mod::Stuff[Ring, Wood] = 100;
$Smith::Mod::Stuff[Ring, Rock] = 100;
$Smith::Mod::Stuff[Ring, Fabric] = 100;

##################################################################################



$Smith::Mod[Suit] = "Suit";

$Smith::Mod[Gear] = "Gear";
$Smith::Mod::Attach[Gear] = "Suit";
$Smith::Mod[Outfit] = "Outfit";
$Smith::Mod::Attach[Outfit] = "Suit";

$Smith::Mod::DEF[Suit] = "0.5";
$Smith::Mod::HP[Suit] = "0.2";
$Smith::Mod::DEX[Suit] = "0.4";
$Smith::Mod::Weight[Suit] = "0.5";
$Smith::Mod::Type[Suit] = $BodyAccessoryType;
$Smith::Mod::AddSound[Suit] = true;
$Smith::Mod::Stuff[Suit, Metal] = 100;
$Smith::Mod::Stuff[Suit, Wood] = 0;
$Smith::Mod::Stuff[Suit, Rock] = 100;
$Smith::Mod::Stuff[Suit, Fabric] = 100;

$Smith::Mod[Armor] = "Armor";

$Smith::Mod[Mail] = "Mail";
$Smith::Mod::Attach[Mail] = "Armor";
$Smith::Mod[ChainMail] = "ChainMail";
$Smith::Mod::Attach[ChainMail] = "Armor";
$Smith::Mod[RingMail] = "RingMail";
$Smith::Mod::Attach[RingMail] = "Armor";
$Smith::Mod[Plate] = "Plate";
$Smith::Mod::Attach[Plate] = "Armor";
$Smith::Mod[PlateMail] = "PlateMail";
$Smith::Mod::Attach[PlateMail] = "Armor";

$Smith::Mod::DEF[Armor] = "1";
$Smith::Mod::HP[Armor] = "0.4";
$Smith::Mod::DEX[Armor] = "-0.4";
//$Smith::Mod::DEXMODVAR[Armor] = "%dexmod %weight $Smith::Mod::DEX[Armor]";
//$Smith::Mod::DEXMOD[Armor] = "%1 = floor(%2 / %3); if(%1 <= 10) %1 = 0; else %1 = -%1;";
//%str = sprintf($Smith::Mod::DEXMOD[Armor], "%dexmod", %weight, $Smith::Mod::DEX[Armor]);
//eval(%str);
$Smith::Mod::Weight[Armor] = "0.7";
$Smith::Mod::Type[Armor] = $BodyAccessoryType;
$Smith::Mod::AddSound[Armor] = true;
$Smith::Mod::Stuff[Armor, Metal] = 100;
$Smith::Mod::Stuff[Armor, Wood] = 75;
$Smith::Mod::Stuff[Armor, Rock] = 100;
$Smith::Mod::Stuff[Armor, Fabric] = 50;


$Smith::Mod[Robe] = "Robe";
$Smith::Mod::DEF[Robe] = "0.1";
$Smith::Mod::MDEF[Robe] = "1";
$Smith::Mod::MP[Robe] = "0.4";
$Smith::Mod::Weight[Robe] = "0.4";
$Smith::Mod::Type[Robe] = $BodyAccessoryType;
$Smith::Mod::AddSound[Robe] = true;
$Smith::Mod::Shape[Robe] = "Robed";
$Smith::Mod::Stuff[Robe, Metal] = 0;
$Smith::Mod::Stuff[Robe, Wood] = 50;
$Smith::Mod::Stuff[Robe, Rock] = 75;
$Smith::Mod::Stuff[Robe, Fabric] = 100;

##################################################################################

$Smith::Mod[Boots] = "Boots";
$Smith::Mod::DEF[Boots] = "0.5";
$Smith::Mod::DEX[Boots] = "1.25";
//$Smith::Mod::DEXMODVAR[Boots] = "%dexmod %def $Smith::Mod::DEX[Boots]";
//$Smith::Mod::DEXMOD[Boots] = "%1 = (%2 - %3); if(%1 < 0) %1 = 0;";
$Smith::Mod::Weight[Boots] = "0.4";
$Smith::Mod::Type[Boots] = $BootsAccessoryType;
$Smith::Mod::Stuff[Boots, Metal] = 100;
$Smith::Mod::Stuff[Boots, Wood] = 85;
$Smith::Mod::Stuff[Boots, Rock] = 100;
$Smith::Mod::Stuff[Boots, Fabric] = 75;

##################################################################################

//$BackAccessoryType;
//$Smith::Stuff[%i++] = "Back";

##################################################################################

//$Smith::Mod::DEXMODVAR[Shield] = "%dexmod %weight $Smith::Mod::DEX[Shield]";
//$Smith::Mod::DEXMOD[Shield] = "%1 = floor(%2 / %3); if(%1 <= 10) %1 = 0; else %1 = -%1;";

$Smith::Mod[Buckler] = "Buckler";
$Smith::Mod::DEF[Buckler] = "1";
$Smith::Mod::MDEF[Buckler] = "0.1";
$Smith::Mod::DEX[Buckler] = "-0.4";
$Smith::Mod::Weight[Buckler] = "0.5";
$Smith::Mod::Type[Buckler] = $ShieldAccessoryType;
$Smith::Mod::Shape[Buckler] = "shield1";
$Smith::Mod::Stuff[Buckler, Metal] = 100;
$Smith::Mod::Stuff[Buckler, Wood] = 50;
$Smith::Mod::Stuff[Buckler, Rock] = 100;
$Smith::Mod::Stuff[Buckler, Fabric] = 10;

$Smith::Mod[Shield] = "Shield";
$Smith::Mod::DEF[Shield] = "1.5";
$Smith::Mod::MDEF[Shield] = "0.2";
$Smith::Mod::DEX[Shield] = "-0.6";
$Smith::Mod::Weight[Shield] = "0.6";
$Smith::Mod::Type[Shield] = $ShieldAccessoryType;
$Smith::Mod::Shape[Shield] = "shield2";
$Smith::Mod::Stuff[Shield, Metal] = 100;
$Smith::Mod::Stuff[Shield, Wood] = 50;
$Smith::Mod::Stuff[Shield, Rock] = 100;
$Smith::Mod::Stuff[Shield, Fabric] = 0;

$Smith::Mod[HeavyShield] = "Heavy Shield";
$Smith::Mod::DEF[HeavyShield] = "2.5";
$Smith::Mod::MDEF[HeavyShield] = "0.3";
$Smith::Mod::DEX[HeavyShield] = "-0.8";
$Smith::Mod::Weight[HeavyShield] = "0.7";
$Smith::Mod::Type[HeavyShield] = $ShieldAccessoryType;
$Smith::Mod::Shape[HeavyShield] = "shield3";
$Smith::Mod::Stuff[HeavyShield, Metal] = 100;
$Smith::Mod::Stuff[HeavyShield, Wood] = 0;
$Smith::Mod::Stuff[HeavyShield, Rock] = 100;
$Smith::Mod::Stuff[HeavyShield, Fabric] = 0;

##################################################################################

$Smith::Mod[Talisman] = "Talisman";

$Smith::Mod[Pendant] = "Pendant";
$Smith::Mod::Attach[Pendant] = "Talisman";
$Smith::Mod[Necklace] = "Necklace";
$Smith::Mod::Attach[Necklace] = "Talisman";
$Smith::Mod::DEF[Talisman] = "0.1";
//$Smith::Mod::ATK[Talisman] = "1";
//$Smith::Mod::MDEF[Talisman] = "1";
$Smith::Mod::STR[Talisman] = "0.05";
$Smith::Mod::DEX[Talisman] = "0.05";
$Smith::Mod::CON[Talisman] = "0.05";
$Smith::Mod::INT[Talisman] = "0.05";
$Smith::Mod::WIS[Talisman] = "0.05";
$Smith::Mod::HP[Talisman] = "0.01";
$Smith::Mod::MP[Talisman] = "0.01";
$Smith::Mod::Weight[Talisman] = "0.1";
$Smith::Mod::Type[Talisman] = $TalismanAccessoryType;
$Smith::Mod::Stuff[Talisman, Metal] = 50;
$Smith::Mod::Stuff[Talisman, Wood] = 75;
$Smith::Mod::Stuff[Talisman, Rock] = 25;
$Smith::Mod::Stuff[Talisman, Fabric] = 100;

##################################################################################

$Smith::Mod[Dagger] = "Dagger";
$Smith::Mod::ATK[Dagger] = "0.6";
$Smith::Mod::Weight[Dagger] = "0.5";
$Smith::Mod::Range[Dagger] = "1.5";
$Smith::Mod::Delay[Dagger] = "1.5";
$Smith::Mod::Sound[Dagger] = "SoundSwing1";
$Smith::Mod::Type[Dagger] = $SwordAccessoryType;
$Smith::Mod::DType[Dagger] = $PiercingDamageType;
$Smith::Mod::Shape[Dagger] = "dagger";
$Smith::Mod::Stuff[Dagger, Metal] = 100;
$Smith::Mod::Stuff[Dagger, Wood] = 75;
$Smith::Mod::Stuff[Dagger, Rock] = 100;
$Smith::Mod::Stuff[Dagger, Fabric] = 0;

$Smith::Mod[ShortSword] = "Short Sword";
$Smith::Mod::ATK[ShortSword] = "0.7";
$Smith::Mod::Weight[ShortSword] = "0.6";
$Smith::Mod::Range[ShortSword] = "2";
$Smith::Mod::Delay[ShortSword] = "2";
$Smith::Mod::Sound[ShortSword] = "SoundSwing2";
$Smith::Mod::Type[ShortSword] = $SwordAccessoryType;
$Smith::Mod::DType[ShortSword] = $SlashingDamageType;
$Smith::Mod::Shape[ShortSword] = "short_sword";
$Smith::Mod::Stuff[ShortSword, Metal] = 100;
$Smith::Mod::Stuff[ShortSword, Wood] = 75;
$Smith::Mod::Stuff[ShortSword, Rock] = 100;
$Smith::Mod::Stuff[ShortSword, Fabric] = 0;

$Smith::Mod[Sword] = "Sword";
$Smith::Mod::ATK[Sword] = "2";
$Smith::Mod::Weight[Sword] = "0.7";
$Smith::Mod::Range[Sword] = "3.5";
$Smith::Mod::Delay[Sword] = "3";
$Smith::Mod::Sound[Sword] = "SoundSwing 5 6";
$Smith::Mod::Type[Sword] = $SwordAccessoryType;
$Smith::Mod::DType[Sword] = $SlashingDamageType;
$Smith::Mod::Shape[Sword] = "sword";
$Smith::Mod::Stuff[Sword, Metal] = 100;
$Smith::Mod::Stuff[Sword, Wood] = 75;
$Smith::Mod::Stuff[Sword, Rock] = 100;
$Smith::Mod::Stuff[Sword, Fabric] = 0;

$Smith::Mod[LongSword] = "Long Sword";
$Smith::Mod::ATK[LongSword] = "1.5";
$Smith::Mod::Weight[LongSword] = "0.7";
$Smith::Mod::Range[LongSword] = "4";
$Smith::Mod::Delay[LongSword] = "3";
$Smith::Mod::Sound[LongSword] = "SoundSwing6";
$Smith::Mod::Type[LongSword] = $SwordAccessoryType;
$Smith::Mod::DType[LongSword] = $SlashingDamageType;
$Smith::Mod::Shape[LongSword] = "long_sword";
$Smith::Mod::Stuff[LongSword, Metal] = 100;
$Smith::Mod::Stuff[LongSword, Wood] = 75;
$Smith::Mod::Stuff[LongSword, Rock] = 100;
$Smith::Mod::Stuff[LongSword, Fabric] = 0;

$Smith::Mod[Katana] = "Katana";
$Smith::Mod::ATK[Katana] = "1";
$Smith::Mod::Weight[Katana] = "0.55";
$Smith::Mod::Range[Katana] = "2";
$Smith::Mod::Delay[Katana] = "1.5";
$Smith::Mod::Sound[Katana] = "SoundSwing3";
$Smith::Mod::Type[Katana] = $SwordAccessoryType;
$Smith::Mod::DType[Katana] = $PiercingDamageType;
$Smith::Mod::Shape[Katana] = "katana";
$Smith::Mod::Stuff[Katana, Metal] = 100;
$Smith::Mod::Stuff[Katana, Wood] = 75;
$Smith::Mod::Stuff[Katana, Rock] = 100;
$Smith::Mod::Stuff[Katana, Fabric] = 0;

$Smith::Mod[Saber] = "Saber";
$Smith::Mod::ATK[Saber] = "1.75";
$Smith::Mod::Weight[Saber] = "0.6";
$Smith::Mod::Range[Saber] = "3";
$Smith::Mod::Delay[Saber] = "3";
$Smith::Mod::Sound[Saber] = "SoundSwing2";
$Smith::Mod::Type[Saber] = $SwordAccessoryType;
$Smith::Mod::DType[Saber] = $SlashingDamageType;
$Smith::Mod::Shape[Saber] = "elfinblade";
$Smith::Mod::Stuff[Saber, Metal] = 100;
$Smith::Mod::Stuff[Saber, Wood] = 75;
$Smith::Mod::Stuff[Saber, Rock] = 100;
$Smith::Mod::Stuff[Saber, Fabric] = 0;

##################################################################################

$Smith::Mod[Hatchet] = "Hatchet";
$Smith::Mod::ATK[Hatchet] = "0.5";
$Smith::Mod::Weight[Hatchet] = "0.5";
$Smith::Mod::Range[Hatchet] = "1.5";
$Smith::Mod::Delay[Hatchet] = "2";
$Smith::Mod::Sound[Hatchet] = "SoundSwing1";
$Smith::Mod::Type[Hatchet] = $AxeAccessoryType;
$Smith::Mod::DType[Hatchet] = $SlashingDamageType;
$Smith::Mod::Shape[Hatchet] = "hatchet";
$Smith::Mod::Stuff[Hatchet, Metal] = 100;
$Smith::Mod::Stuff[Hatchet, Wood] = 75;
$Smith::Mod::Stuff[Hatchet, Rock] = 100;
$Smith::Mod::Stuff[Hatchet, Fabric] = 0;

$Smith::Mod[Axe] = "Axe";
$Smith::Mod::ATK[Axe] = "1";
$Smith::Mod::Weight[Axe] = "0.6";
$Smith::Mod::Range[Axe] = "2";
$Smith::Mod::Delay[Axe] = "2";
$Smith::Mod::Sound[Axe] = "SoundSwing 3 5 6";
$Smith::Mod::Type[Axe] = $AxeAccessoryType;
$Smith::Mod::DType[Axe] = $SlashingDamageType;
$Smith::Mod::Shape[Axe] = "axe";
$Smith::Mod::Stuff[Axe, Metal] = 100;
$Smith::Mod::Stuff[Axe, Wood] = 75;
$Smith::Mod::Stuff[Axe, Rock] = 100;
$Smith::Mod::Stuff[Axe, Fabric] = 0;

$Smith::Mod[Baridache] = "Baridache";
$Smith::Mod[BattleAxe] = "Battle Axe";
$Smith::Mod::Attach[BattleAxe] = "Baridache"; //BattleAxe is the same as a Baridache...?
$Smith::Mod::ATK[Baridache] = "1.75";
$Smith::Mod::Weight[Baridache] = "0.7";
$Smith::Mod::Range[Baridache] = "5";
$Smith::Mod::Delay[Baridache] = "4";
$Smith::Mod::Sound[Baridache] = "SoundSwing7";
$Smith::Mod::Type[Baridache] = $AxeAccessoryType;
$Smith::Mod::DType[Baridache] = $SlashingDamageType;
$Smith::Mod::Shape[Baridache] = "battleaxe";
$Smith::Mod::Stuff[Baridache, Metal] = 100;
$Smith::Mod::Stuff[Baridache, Wood] = 75;
$Smith::Mod::Stuff[Baridache, Rock] = 100;
$Smith::Mod::Stuff[Baridache, Fabric] = 0;

##################################################################################

$Smith::Mod[Staff] = "Staff";
$Smith::Mod::ATK[Staff] = "0.75";
$Smith::Mod::Weight[Staff] = "0.4";
$Smith::Mod::Range[Staff] = "2";
$Smith::Mod::Delay[Staff] = "2";
$Smith::Mod::Sound[Staff] = "SoundSwing 3 4";
$Smith::Mod::Type[Staff] = $PolearmAccessoryType;
$Smith::Mod::DType[Staff] = $BludgeoningDamageType;
$Smith::Mod::Shape[Staff] = "longstaff";
$Smith::Mod::Stuff[Staff, Metal] = 75;
$Smith::Mod::Stuff[Staff, Wood] = 100;
$Smith::Mod::Stuff[Staff, Rock] = 75;
$Smith::Mod::Stuff[Staff, Fabric] = 0;

$Smith::Mod[Rod] = "Rod";
$Smith::Mod::ATK[Rod] = "0.25";
$Smith::Mod::Weight[Rod] = "0.4";
$Smith::Mod::Range[Rod] = "2";
$Smith::Mod::Delay[Rod] = "2";
$Smith::Mod::Sound[Rod] = "SoundSwing3";
$Smith::Mod::Type[Rod] = $PolearmAccessoryType;
$Smith::Mod::DType[Rod] = $BludgeoningDamageType;
$Smith::Mod::Shape[Rod] = "quarterstaff";
$Smith::Mod::Stuff[Rod, Metal] = 50;
$Smith::Mod::Stuff[Rod, Wood] = 100;
$Smith::Mod::Stuff[Rod, Rock] = 0;
$Smith::Mod::Stuff[Rod, Fabric] = 0;

$Smith::Mod[Trident] = "Trident";
$Smith::Mod::ATK[Trident] = "0.9";
$Smith::Mod::Weight[Trident] = "0.5";
$Smith::Mod::Range[Trident] = "3";
$Smith::Mod::Delay[Trident] = "3";
$Smith::Mod::Sound[Trident] = "SoundSwing3";
$Smith::Mod::Type[Trident] = $PolearmAccessoryType;
$Smith::Mod::DType[Trident] = $BludgeoningDamageType;
$Smith::Mod::Shape[Trident] = "trident";
$Smith::Mod::Stuff[Trident, Metal] = 75;
$Smith::Mod::Stuff[Trident, Wood] = 100;
$Smith::Mod::Stuff[Trident, Rock] = 0;
$Smith::Mod::Stuff[Trident, Fabric] = 0;

$Smith::Mod[Spear] = "Spear";
$Smith::Mod::ATK[Spear] = "1";
$Smith::Mod::Weight[Spear] = "0.6";
$Smith::Mod::Range[Spear] = "4";
$Smith::Mod::Delay[Spear] = "3";
$Smith::Mod::Sound[Spear] = "SoundSwing 3 6";
$Smith::Mod::Type[Spear] = $PolearmAccessoryType;
$Smith::Mod::DType[Spear] = $BludgeoningDamageType;
$Smith::Mod::Shape[Spear] = "spear";
$Smith::Mod::Stuff[Spear, Metal] = 100;
$Smith::Mod::Stuff[Spear, Wood] = 100;
$Smith::Mod::Stuff[Spear, Rock] = 100;
$Smith::Mod::Stuff[Spear, Fabric] = 0;

$Smith::Mod[Tip] = "Tip";
$Smith::Mod::ATK[Tip] = "2";
$Smith::Mod::Weight[Tip] = "0.6";
$Smith::Mod::Range[Tip] = "5";
$Smith::Mod::Delay[Tip] = "3.5";
$Smith::Mod::Sound[Tip] = "SoundSwing 3 6";
$Smith::Mod::Type[Tip] = $PolearmAccessoryType;
$Smith::Mod::DType[Tip] = $BludgeoningDamageType;
$Smith::Mod::Shape[Tip] = "spear";
$Smith::Mod::Stuff[Tip, Metal] = 100;
$Smith::Mod::Stuff[Tip, Wood] = 100;
$Smith::Mod::Stuff[Tip, Rock] = 100;
$Smith::Mod::Stuff[Tip, Fabric] = 0;

##################################################################################

$Smith::Mod[Club] = "Club";
$Smith::Mod::ATK[Club] = "0.75";
$Smith::Mod::Weight[Club] = "0.5";
$Smith::Mod::Range[Club] = "2";
$Smith::Mod::Delay[Club] = "1.5";
$Smith::Mod::Sound[Club] = "SoundSwing5";
$Smith::Mod::Type[Club] = $BludgeonAccessoryType;
$Smith::Mod::DType[Club] = $BludgeoningDamageType;
$Smith::Mod::Shape[Club] = "mace";
$Smith::Mod::Stuff[Club, Metal] = 0;
$Smith::Mod::Stuff[Club, Wood] = 100;
$Smith::Mod::Stuff[Club, Rock] = 0;
$Smith::Mod::Stuff[Club, Fabric] = 0;

$Smith::Mod[Mace] = "Mace";
$Smith::Mod::ATK[Mace] = "1.5";
$Smith::Mod::Weight[Mace] = "0.6";
$Smith::Mod::Range[Mace] = "2";
$Smith::Mod::Delay[Mace] = "1.5";
$Smith::Mod::Sound[Mace] = "SoundSwing6";
$Smith::Mod::Type[Mace] = $BludgeonAccessoryType;
$Smith::Mod::DType[Mace] = $BludgeoningDamageType;
$Smith::Mod::Shape[Mace] = "mace";
$Smith::Mod::Stuff[Mace, Metal] = 100;
$Smith::Mod::Stuff[Mace, Wood] = 75;
$Smith::Mod::Stuff[Mace, Rock] = 100;
$Smith::Mod::Stuff[Mace, Fabric] = 0;

$Smith::Mod[Hammer] = "Hammer";
$Smith::Mod[Mallet] = "Mallet";
$Smith::Mod::Attach[Mallet] = "Hammer";
$Smith::Mod::ATK[Hammer] = "2.5";
$Smith::Mod::Weight[Hammer] = "0.7";
$Smith::Mod::Range[Hammer] = "3";
$Smith::Mod::Delay[Hammer] = "4";
$Smith::Mod::Sound[Hammer] = "SoundSwing 6 7";
$Smith::Mod::Type[Hammer] = $BludgeonAccessoryType;
$Smith::Mod::DType[Hammer] = $BludgeoningDamageType;
$Smith::Mod::Shape[Hammer] = "hammer";
$Smith::Mod::Stuff[Hammer, Metal] = 100;
$Smith::Mod::Stuff[Hammer, Wood] = 75;
$Smith::Mod::Stuff[Hammer, Rock] = 100;
$Smith::Mod::Stuff[Hammer, Fabric] = 0;

##################################################################################
//Ammo mod is in Accessory.cs (around line 1370)

$Smith::Mod[CrossBow] = "CrossBow";
$Smith::Mod::ATK[CrossBow] = "0.5";
$Smith::Mod::Weight[CrossBow] = "0.5";
$Smith::Mod::Range[CrossBow] = "20";
$Smith::Mod::Delay[CrossBow] = "1.5";
$Smith::Mod::Sound[CrossBow] = "CrossbowShoot1";
$Smith::Mod::Type[CrossBow] = $RangedAccessoryType;
$Smith::Mod::DType[CrossBow] = "NULL";
$Smith::Mod::Shape[CrossBow] = "crossbow";
$Smith::Mod::Stuff[CrossBow, Metal] = 0;
$Smith::Mod::Stuff[CrossBow, Wood] = 100;
$Smith::Mod::Stuff[CrossBow, Rock] = 0;
$Smith::Mod::Stuff[CrossBow, Fabric] = 0;

$Smith::Mod[ShortBow] = "ShortBow";
$Smith::Mod::ATK[ShortBow] = "0.6";
$Smith::Mod::Weight[ShortBow] = "0.5";
$Smith::Mod::Range[ShortBow] = "120";
$Smith::Mod::Delay[ShortBow] = "2";
$Smith::Mod::Sound[ShortBow] = "CrossbowShoot1";
$Smith::Mod::Type[ShortBow] = $RangedAccessoryType;
$Smith::Mod::DType[ShortBow] = "NULL";
$Smith::Mod::Shape[ShortBow] = "longbow";
$Smith::Mod::Stuff[ShortBow, Metal] = 0;
$Smith::Mod::Stuff[ShortBow, Wood] = 100;
$Smith::Mod::Stuff[ShortBow, Rock] = 0;
$Smith::Mod::Stuff[ShortBow, Fabric] = 0;

$Smith::Mod[LongBow] = "LongBow";
$Smith::Mod::ATK[LongBow] = "0.65";
$Smith::Mod::Weight[LongBow] = "0.5";
$Smith::Mod::Range[LongBow] = "360";
$Smith::Mod::Delay[LongBow] = "2";
$Smith::Mod::Sound[LongBow] = "CrossbowShoot1";
$Smith::Mod::Type[LongBow] = $RangedAccessoryType;
$Smith::Mod::DType[LongBow] = "NULL";
$Smith::Mod::Shape[LongBow] = "longbow";
$Smith::Mod::Stuff[LongBow, Metal] = 0;
$Smith::Mod::Stuff[LongBow, Wood] = 100;
$Smith::Mod::Stuff[LongBow, Rock] = 0;
$Smith::Mod::Stuff[LongBow, Fabric] = 0;

$Smith::Mod[HeavyCrossBow] = "Heavy CrossBow";
$Smith::Mod::ATK[CrossBow] = "0.7";
$Smith::Mod::Weight[HeavyCrossBow] = "0.6";
$Smith::Mod::Range[HeavyCrossBow] = "360";
$Smith::Mod::Delay[HeavyCrossBow] = "3";
$Smith::Mod::Sound[HeavyCrossBow] = "CrossbowShoot1";
$Smith::Mod::Type[HeavyCrossBow] = $RangedAccessoryType;
$Smith::Mod::DType[HeavyCrossBow] = "NULL";
$Smith::Mod::Shape[HeavyCrossBow] = "crossbow";
$Smith::Mod::Stuff[HeavyCrossBow, Metal] = 0;
$Smith::Mod::Stuff[HeavyCrossBow, Wood] = 100;
$Smith::Mod::Stuff[HeavyCrossBow, Rock] = 0;
$Smith::Mod::Stuff[HeavyCrossBow, Fabric] = 0;

$Smith::Mod[CompBow] = "CompBow";
$Smith::Mod::ATK[CrossBow] = "0.65";
$Smith::Mod::Weight[CompBow] = "0.5";
$Smith::Mod::Range[CompBow] = "360";
$Smith::Mod::Delay[CompBow] = "1.5";
$Smith::Mod::Sound[CompBow] = "CrossbowShoot1";
$Smith::Mod::Type[CompBow] = $RangedAccessoryType;
$Smith::Mod::DType[CompBow] = "NULL";
$Smith::Mod::Shape[CompBow] = "comp_bow";
$Smith::Mod::Stuff[CompBow, Metal] = 0;
$Smith::Mod::Stuff[CompBow, Wood] = 100;
$Smith::Mod::Stuff[CompBow, Rock] = 0;
$Smith::Mod::Stuff[CompBow, Fabric] = 0;

##################################################################################

$Smith::Mod[Arrow] = "Arrow";
$Smith::Mod::ATK[Arrow] = "0.9";
$Smith::Mod::ForceWeight[Arrow] = "0.01";
$Smith::Mod::Type[Arrow] = $ProjectileAccessoryType;
$Smith::Mod::Shape[Arrow] = "tracer";
$Smith::Mod::Stuff[Arrow, Metal] = 100;
$Smith::Mod::Stuff[Arrow, Wood] = 100;
$Smith::Mod::Stuff[Arrow, Rock] = 100;
$Smith::Mod::Stuff[Arrow, Fabric] = 0;

$Smith::Mod[Quarrel] = "Quarrel";
$Smith::Mod::ATK[Quarrel] = "1.5";
$Smith::Mod::ForceWeight[Quarrel] = "0.05"; //keep our arrows light
$Smith::Mod::Type[Quarrel] = $ProjectileAccessoryType;
$Smith::Mod::Shape[Quarrel] = "bullet";
$Smith::Mod::Stuff[Quarrel, Metal] = 100;
$Smith::Mod::Stuff[Quarrel, Wood] = 100;
$Smith::Mod::Stuff[Quarrel, Rock] = 100;
$Smith::Mod::Stuff[Quarrel, Fabric] = 0;

##################################################################################

$Smith::Mod[Helmet] = "Helmet";
$Smith::Mod::DEF[Helmet] = "0.5";
$Smith::Mod::MDEF[Helmet] = "0.01";
$Smith::Mod::HP[Helmet] = "0.1";
$Smith::Mod::Weight[Helmet] = "0.5";
$Smith::Mod::Type[Helmet] = $HeadAccessoryType;
$Smith::Mod::Stuff[Helmet, Metal] = 100;
$Smith::Mod::Stuff[Helmet, Wood] = 75;
$Smith::Mod::Stuff[Helmet, Rock] = 100;
$Smith::Mod::Stuff[Helmet, Fabric] = 0;

$Smith::Mod[Hat] = "Hat";
$Smith::Mod::DEF[Hat] = "0.01";
$Smith::Mod::MDEF[Hat] = "0.5";
$Smith::Mod::HP[Hat] = "0.1";
$Smith::Mod::Weight[Hat] = "0.1";
$Smith::Mod::Type[Hat] = $HeadAccessoryType;
$Smith::Mod::Stuff[Hat, Metal] = 0;
$Smith::Mod::Stuff[Hat, Wood] = 0;
$Smith::Mod::Stuff[Hat, Rock] = 0;
$Smith::Mod::Stuff[Hat, Fabric] = 100;

##################################################################################

$Smith::Mod[Gauntlets] = "Gauntlets";
$Smith::Mod::DEF[Gauntlets] = "0.5";
$Smith::Mod::MDEF[Gauntlets] = "0.3";
$Smith::Mod::HP[Gauntlets] = "0.4";
$Smith::Mod::Weight[Gauntlets] = "0.6";
$Smith::Mod::Type[Gauntlets] = $HandsAccessoryType;
$Smith::Mod::Stuff[Gauntlets, Metal] = 100;
$Smith::Mod::Stuff[Gauntlets, Wood] = 25;
$Smith::Mod::Stuff[Gauntlets, Rock] = 100;
$Smith::Mod::Stuff[Gauntlets, Fabric] = 25;

$Smith::Mod[Gloves] = "Gloves";
$Smith::Mod::DEF[Gloves] = "0.2";
$Smith::Mod::MDEF[Gloves] = "0.4";
$Smith::Mod::HP[Gloves] = "0.5";
$Smith::Mod::Weight[Gloves] = "0.5";
$Smith::Mod::Type[Gloves] = $HandsAccessoryType;
$Smith::Mod::Stuff[Gloves, Metal] = 0;
$Smith::Mod::Stuff[Gloves, Wood] = 0;
$Smith::Mod::Stuff[Gloves, Rock] = 0;
$Smith::Mod::Stuff[Gloves, Fabric] = 100;

$Smith::Mod[Bracelet] = "Bracelet";
$Smith::Mod::DEF[Bracelet] = "0.1";
$Smith::Mod::MDEF[Bracelet] = "0.9";
$Smith::Mod::HP[Bracelet] = "0.3";
$Smith::Mod::Weight[Bracelet] = "0.55";
$Smith::Mod::Type[Bracelet] = $HandsAccessoryType;
$Smith::Mod::Stuff[Bracelet, Metal] = 100;
$Smith::Mod::Stuff[Bracelet, Wood] = 100;
$Smith::Mod::Stuff[Bracelet, Rock] = 100;
$Smith::Mod::Stuff[Bracelet, Fabric] = 100;

##################################################################################

$Smith::Mod[Leggings] = "Leggings";
$Smith::Mod::DEF[Leggings] = "0.6";
$Smith::Mod::Weight[Leggings] = "0.6";
$Smith::Mod::Type[Leggings] = $LegsAccessoryType;
$Smith::Mod::Stuff[Leggings, Metal] = 100;
$Smith::Mod::Stuff[Leggings, Wood] = 75;
$Smith::Mod::Stuff[Leggings, Rock] = 100;
$Smith::Mod::Stuff[Leggings, Fabric] = 75;

$Smith::Mod[Pants] = "Pants";
$Smith::Mod::DEF[Pants] = "0.2";
$Smith::Mod::HP[Pants] = "0.1";
$Smith::Mod::Weight[Pants] = "0.2";
$Smith::Mod::Type[Pants] = $LegsAccessoryType;
$Smith::Mod::Stuff[Pants, Metal] = 0;
$Smith::Mod::Stuff[Pants, Wood] = 0;
$Smith::Mod::Stuff[Pants, Rock] = 0;
$Smith::Mod::Stuff[Pants, Fabric] = 100;

##################################################################################

$Smith::Mod[Orb] = "Orb";
$Smith::Mod::DEF[Orb] = "0.2";
$Smith::Mod::MDEF[Orb] = "0.1";
$Smith::Mod::HP[Orb] = "0.3";
$Smith::Mod::Weight[Orb] = "0";
$Smith::Mod::Type[Orb] = $OrbAccessoryType;
$Smith::Mod::Stuff[Orb, Metal] = 100;
$Smith::Mod::Stuff[Orb, Wood] = 100;
$Smith::Mod::Stuff[Orb, Rock] = 100;
$Smith::Mod::Stuff[Orb, Fabric] = 100;

##################################################################################
##################################################################################
##################################################################################


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
} // BlackSmith::ModStrings   ////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function BlackSmithStuff() { //		BLACK SMITH ITEMS		//
	BlackSmith::ModStrings(); // Load Smith Mod data
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//SMITH 2 min (recommanded)

//		METAL
MakeItem("Brass", "Brass", "SMITH 2",				$Smith::Stuff[Metal], 1, "A piece of Brass Metal.",		$Headers::X, "NULL", "Material", "rpgbrigandine");
MakeItem("Copper", "Copper", "SMITH 5",			$Smith::Stuff[Metal], 1, "A piece of Copper Metal.",		$Headers::X, "NULL", "Material", "rpgchainmail");
MakeItem("Bronze", "Bronze", "SMITH 10",		$Smith::Stuff[Metal], 1, "A piece of Bronze Metal.",		$Headers::X, "NULL", "Material", "rpgbronzeplate");
MakeItem("Silver", "Silver", "SMITH 20",			$Smith::Stuff[Metal], 1, "A piece of Silver Metal.",		$Headers::X, "NULL", "Material", "rpgscalemail");
MakeItem("Gold", "Gold", "SMITH 40",				$Smith::Stuff[Metal], 1, "A piece of Gold Metal.",		$Headers::X, "NULL", "Material", "rpgbronzeplate");
MakeItem("Iron", "Iron", "SMITH 80",				$Smith::Stuff[Metal], 1, "A piece of Iron Metal.",		$Headers::X, "NULL", "Material", "rpgbandedmail");
MakeItem("Steel", "Steel", "SMITH 160",			$Smith::Stuff[Metal], 1, "A piece of Steel Metal.",		$Headers::X, "NULL", "Material", "rpgplatemail");
MakeItem("Platinum", "Platinum", "SMITH 240",	$Smith::Stuff[Metal], 1, "A piece of Platinum Metal.",	$Headers::X, "NULL", "Material", "rpgfieldplate");
MakeItem("Mythril", "Mythril", "SMITH 360",		$Smith::Stuff[Metal], 1, "A piece of Mythril Metal.",		$Headers::X, "NULL", "Material", "rpgfullplate");
MakeItem("Titanium", "Titanium", "SMITH 540",	$Smith::Stuff[Metal], 1, "A piece of Titanium Metal.",	$Headers::X, "NULL", "Material", "RMDarkPlate");

//		WOOD
MakeItem("Pine", "Pine", "SMITH 2",				$Smith::Stuff[Wood], 1, "A piece of Pine Wood.",		$Headers::X, "NULL", "Material");
MakeItem("White Pine", "White Pine", "SMITH 5",		$Smith::Stuff[Wood], 1, "A piece of Pine Wood.",		$Headers::X, "NULL", "Material");
MakeItem("Cyprus Pine", "Cyprus Pine", "SMITH 10",				$Smith::Stuff[Wood], 1, "A piece of Pine Wood.",		$Headers::X, "NULL", "Material");
MakeItem("Poplar", "Poplar", "SMITH 20",				$Smith::Stuff[Wood], 1, "A piece of Pine Wood.",		$Headers::X, "NULL", "Material");
MakeItem("Cider", "Cider", "SMITH 40",				$Smith::Stuff[Wood], 1, "A piece of Pine Wood.",		$Headers::X, "NULL", "Material");
MakeItem("Oak", "Oak", "SMITH 80",				$Smith::Stuff[Wood], 1, "A piece of Pine Wood.",		$Headers::X, "NULL", "Material");
MakeItem("Redwood", "Redwood", "SMITH 160",				$Smith::Stuff[Wood], 1, "A piece of Pine Wood.",		$Headers::X, "NULL", "Material");
MakeItem("Dogwood", "Dogwood", "SMITH 240",				$Smith::Stuff[Wood], 1, "A piece of Pine Wood.",		$Headers::X, "NULL", "Material");
MakeItem("Maple", "Maple", "SMITH 360",				$Smith::Stuff[Wood], 1, "A piece of Pine Wood.",		$Headers::X, "NULL", "Material");
MakeItem("Cherry wood", "Cherry wood", "SMITH 400",				$Smith::Stuff[Wood], 1, "A piece of Pine Wood.",		$Headers::X, "NULL", "Material");
MakeItem("Mahogany", "Mahongany", "SMITH 450",				$Smith::Stuff[Wood], 1, "A piece of Pine Wood.",		$Headers::X, "NULL", "Material");


//		ROCK
MakeItem("Ivory", "Ivory", "SMITH 2",				$Smith::Stuff[Rock], 1, "A piece of Ivory Rock.",			$Headers::X, "NULL", "Material", "RMStone");
MakeItem("Coral", "Coral", "SMITH 4",				$Smith::Stuff[Rock], 1, "A piece of Coral Rock.",			$Headers::X, "NULL", "Material", "RMStone");
MakeItem("Quartz", "Quartz", "SMITH 8",			$Smith::Stuff[Rock], 1, "A piece of Quartz Rock.",		$Headers::X, "NULL", "Material", "RMStone");
MakeItem("Crystal", "Crystal", "SMITH 16",		$Smith::Stuff[Rock], 1, "A piece of Crystal Rock.",		$Headers::X, "NULL", "Material", "RMStone");
MakeItem("Lapis", "Lapis", "SMITH 32",			$Smith::Stuff[Rock], 1, "A piece of Lapis Rock.",			$Headers::X, "NULL", "Material", "RMStone");
MakeItem("Pearl", "Pearl", "SMITH 64",			$Smith::Stuff[Rock], 1, "A piece of Pearl Rock.",			$Headers::X, "NULL", "Material", "RMStone");
MakeItem("Amber", "Amber", "SMITH 128",		$Smith::Stuff[Rock], 1, "A piece of Amber Rock.",		$Headers::X, "NULL", "Material", "RMStone");
MakeItem("Ruby", "Ruby", "SMITH 192",			$Smith::Stuff[Rock], 1, "A piece of Ruby Rock.",			$Headers::X, "NULL", "Material", "RMStone");
MakeItem("Emerald", "Emerald", "SMITH 288",	$Smith::Stuff[Rock], 1, "A piece of Emerald Rock.",		$Headers::X, "NULL", "Material", "RMStone");
MakeItem("Diamond", "Diamond", "SMITH 432",	$Smith::Stuff[Rock], 1, "A piece of Diamond Rock.",		$Headers::X, "NULL", "Material", "RMStone");
MakeItem("Valorite", "Valorite", "SMITH 648",	$Smith::Stuff[Rock], 1, "A piece of Valorite Rock.",		$Headers::X, "NULL", "Material", "RMStone");
MakeItem("Red Moon","Red Moon","SMITH 1000",	$Smith::Stuff[Rock], 1, "A piece of Red Moon Rock.",	$Headers::X, "NULL", "Material", "RMStone");

//		FABRIC
MakeItem("Leather Fabric", "Leather Fabric", "SMITH 2",				$Smith::Stuff[Fabric], 1, "A piece of Leather Fabric.",			$Headers::X, "NULL", "Material", "RMFineVest");
MakeItem("Hard Leather Fabric", "Hard Leather Fabric", "SMITH 4",	$Smith::Stuff[Fabric], 1, "A piece of Hard Leather Fabric.",	$Headers::X, "NULL", "Material", "rpgleather");
MakeItem("Hide Fabric", "Hide Fabric", "SMITH 6",						$Smith::Stuff[Fabric], 1, "A piece of Hide Fabric.",				$Headers::X, "NULL", "Material", "rpghide");


MakeItem("Pink Fabric", "Pink Fabric", "SMITH 2",			$Smith::Stuff[Fabric], 1, "A piece of Pink Fabric.",		$Headers::X, "NULL", "Material", "robepink");
MakeItem("Purple Fabric", "Purple Fabric", "SMITH 4",		$Smith::Stuff[Fabric], 1, "A piece of Purple Fabric.",	$Headers::X, "NULL", "Material", "robepurple");
MakeItem("Red Fabric", "Red Fabric", "SMITH 6",				$Smith::Stuff[Fabric], 1, "A piece of Red Fabric.",		$Headers::X, "NULL", "Material", "robered");
MakeItem("Black Fabric", "Black Fabric", "SMITH 12",		$Smith::Stuff[Fabric], 1, "A piece of Black Fabric.",		$Headers::X, "NULL", "Material", "robeblack");
MakeItem("Blue Fabric", "Blue Fabric", "SMITH 24",			$Smith::Stuff[Fabric], 1, "A piece of Blue Fabric.",		$Headers::X, "NULL", "Material", "robeblue");
MakeItem("Green Fabric", "Green Fabric", "SMITH 48",		$Smith::Stuff[Fabric], 1, "A piece of Green Fabric.",		$Headers::X, "NULL", "Material", "robegreen");
MakeItem("Dark Fabric", "Dark Fabric", "SMITH 96",			$Smith::Stuff[Fabric], 1, "A piece of Dark Fabric.",		$Headers::X, "NULL", "Material", "robeblack");
MakeItem("Light Fabric", "Light Fabric", "SMITH 144",		$Smith::Stuff[Fabric], 1, "A piece of Light Fabric.",		$Headers::X, "NULL", "Material", "robewhite");
MakeItem("Elven Fabric", "Elven Fabric", "SMITH 216",		$Smith::Stuff[Fabric], 1, "A piece of Elven made Fabric.",$Headers::X,"NULL", "Material", "robegreen");

//
for(%i = 1; (%model = $Smith::Stuff[%i]) != "" && %i < 100; %i++)
	MakeItem(%model, "Shape: "@%model, "SMITH 2",		"NULL", 1000, "Model to Smith.",	$Headers::X, "NULL", "Material");

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function tmpSmithList(%Client, %item) {

	%pos = String::findSubStr($ClientData[%Client, tmpSmith], %item@" ");
	if(%pos != -1) {
		%data = String::NEWgetSubStr($ClientData[%Client, tmpSmith], %pos, 60);
		%nitem = GetWord(%data, 0);
		%cnt = floor(GetWord(%data, 1));
		%ncnt = floor(%cnt+%amount);

		if(%ncnt > 0)
			$ClientData[%Client, tmpSmith] = String::replace($ClientData[%Client, tmpSmith], %item@" "@%cnt@" ", %nitem@" "@%ncnt@" ");
		else
			$ClientData[%Client, tmpSmith] = String::replace($ClientData[%Client, tmpSmith], %item@" "@%cnt@" ", "");
	}
	else
		$ClientData[%Client, tmpSmith] = $ClientData[%Client, tmpSmith]@%item@" ";

}

function SmithClick(%Client, %item, %bulk) {

	//%player = Client::getOwnedObject(%Client);

	%aiName = $TownBot[%Client.currentSmith, NAME];

	if($ItemData[%item, className] == Equipped) {
		return;
	}

	if(%Client.IsSmithing) {
		Client::sendMessage(%Client, $MsgRed, "The blacksmith is busy...");
		return;
	}

	%aiName = $TownBot[%Client.currentSmith, NAME];

	if($ClientData[%Client, SmithStage] == "stuff") {

		%bulk = 1;
		%type = $ItemData[%item, type];

		if(%type >= 100) {
			tmpSmithList(%Client, %item);
		}
		else {
			Client::sendMessage(%Client, 0, %aiName@" tells you, \"First I need a material type part. (ex Brass, Maple, Gold, or Leather Fabric etc)\"");
			SetupBlacksmith(%Client, %Client.currentSmith);
			return;
		}

		Client::sendMessage(%Client, 0, "Now I need to know what you want me to smith into this.");
		$ClientData[%Client, SmithStage] = "part";

	}
	else if($ClientData[%Client, SmithStage] == "part") {

		%bulk = 1;
		%type = $ItemData[%item, type];

		if(%type < 100) { //Ring, Body, Boots, ... Hands, Legs, Orb
			tmpSmithList(%Client, %item);
		}
		else {
			$ClientData[%Client, tmpSmith] = "";
			if(%type >= 100)
				tmpSmithList(%Client, %item);
			Client::sendMessage(%Client, 0, %aiName@" tells you, \"I need a accessory type part. (ex Armor, Sword, Boots, or Projectile etc)\"");
			SetupBlacksmith(%Client, %Client.currentSmith);
			return;
		}

		if((%item = GetSmithCombo(%Client, $ClientData[%Client, tmpSmith])) != 0) {

			$ClientData[%Client, SmithCost] = GetSmithComboCost(%Client, $ClientData[%Client, GiveSmithItem]); //%item);

			Client::sendMessage(%Client, $MsgWhite, %aiName@" tells you, \"It will cost you "@FixM($ClientData[%Client, SmithCost])@" gil to [#smith] these items.\"~wcanSmith.wav");
			$ClientData[%Client, SmithStage] = "canSmith";

			%msg = WhatIs(%Client, $ClientData[%Client, GiveSmithItem]);
			remoteEval(%Client, "SetPrint::TimeOut", floor(String::len(%msg) /10));
		}
		else {
			remotePlayMode(%Client);
			Client::sendMessage(%Client, 0, %aiName@" tells you, \"I am unable to smith this items.\"");
			return;
		}

	}

	SetupBlacksmith(%Client, %Client.currentSmith);

	return 0;
}

function CompleteSmith(%Client, %id, %cost, %item, %tempsmith, %multiplier) {
	%Client.IsSmithing = "";

	if(!Client::HasItem(%client, getword(%tempsmith,0), "ItemList"))
	{
		Client::sendmessage(%client, 1, "You don't have the "@getword(%tempsmith,0)@" for this item!");
		Client::sendmessage(%client, 1, "Get out of my site!");
		cage(%client,120);
		return;
	}

	if($COINS[%Client] < %cost)
		return;

	$COINS[%Client] -= %cost;

	SaveSmithItem(%Client);

	remotePlayMode(%Client);

	playSound(SoundMoney1, GameBase::getPosition(%Client));

	//GiveThisStuff(%Client, %item, True, %multiplier);

	//%multiplier arrives as the player's requested batch count (#smith N; %cost was
	//already charged as base cost x N). Arrows/Quarrels smith 99 per batch. The old
	//code compared word 1 of %tempsmith (a COUNT in the "item count item count" list,
	//so it never matched) and clobbered %multiplier with 1 - players paid for N but
	//always received 1 item and arrows never batched.
	%multiplier = floor(%multiplier);
	if(%multiplier < 1)
		%multiplier = 1;
	if(String::findSubStr(%tempsmith, "Shape:_Arrow") != -1 || String::findSubStr(%tempsmith, "Shape:_Quarrel") != -1)
		%give = 99 * %multiplier;	//NOTE: inventory caps may still limit what fits past 99
	else
		%give = %multiplier;

	Item::giveItem(%Client, %item, %give, 1);

	for(%i = 0; (%w = GetWord(%tempsmith, %i)) != -1; %i+=2)
	{
		%w2 = Cap(floor(GetWord(%tempsmith, %i+1)) * %multiplier, 1, 99);

		Client::addItemCount(%Client, %w, -%w2);
	}

	AI::sayLater(%Client, %id, "Here you go.", "NULL");
}

function SmithCombo(%Client, %list) {
	if(GetSmithCombo(%Client, %list))
		SaveSmithItem(%Client);
}

//168,531
function GetSmithCombo(%Client, %list) {

//	for(%i = 1; $SmithCombo[%i] != ""; %i++)
//	{
//		if(IsStuffStringEquiv(%tempsmith, $SmithCombo[%i], True))
//			return %i;
//	}
	%w1 = getWord(%list, 0);
	%part = $ItemData[%w1, FixCaps];
	%partname = $ItemData[%w1, Name];
	%type = $ItemData[%w1, type];	//echo(" w1:"@%w1@" part:"@%part@" ["@%type@" "@$Smith::Stuff[%type]@"]");
	%Equip = $ItemData[%w1, Equip];

	if($Smith::Stuff[%type] != "") {

		if(%type >= 100) { // Metal Wood Rock Fabric stuff is for making a new item

			//create new item

			%item = getWord(%list, 1);
			%pos = String::findSubStr(%item, ":_");
			if(%pos != -1)
				%item = String::getSubStr(%item, %pos+2, 60); //remove "Shape:_"

			else if(%pos == -1 && %Client == $ServerClient) //Server doesn't use "Shape:_"
				%item = %item;

			else {
				Client::sendMessage(%Client, 1, "Error in GetSmithCombo -- "@%item@" not valid.");
				echo("Error in GetSmithCombo("@%Client@") -- "@%item@" not valid.");
				return 0;
			}

			//fix caps
			%item = $Smith::Stuff[$Smith::Stuff[%item]]; //sword armor dagger boots etc..

			if($Smith::Stuff[%item] >= 100)
				return 0;

			%itemM = %item; //$Smith::Mod[%item];
			%attach = $Smith::Mod::Attach[%item];
			if(%attach != "")
				%itemM = $Smith::Stuff[$Smith::Stuff[%attach]]; //$Smith::Mod[%attach];



			%svar = $ItemData[%part, svar];
			%smith = AddItemSpecificPoints(%part, "SMITH");

			//$Smith::Mod::Stuff[Ring, Fabric] = 100;

			%smith *= floor($Smith::Mod::Stuff[%itemM, $Smith::Stuff[%type]]/100); //getPercentOf()

			if(%smith <= 0) {
				echo("Invalid Combinations. --"@%partname@" "@%itemM@"-- bonus:"@%smith@" ::"@$Smith::Stuff[%type]@" PERCENT:"@$Smith::Mod::Stuff[%itemM, $Smith::Stuff[%type]]@"% ");
				Client::SendMessage(%Client, 0, "Invalid Combinations");
				return 0;
			}

			%type = $Smith::Mod::Type[%itemM];	//echo("smith:"@%smith@" type:"@%type@" itemM:"@%itemM@" item"@%item@"");

			%shape = $Smith::Mod::Shape[%itemM];

			%header = getHeaderByType(%type);
			%className = getClassNameByType(%type);

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			%def = floor($Smith::Mod::DEF[%itemM] * %smith);
			if(%def < 1 && %def > 0) {
				if(%type != $TalismanAccessoryType || %className != "Weapon" || %type != $ProjectileAccessoryType || %type != $OrbAccessoryType)
					%def = 1;
			}

			%atk = floor($Smith::Mod::ATK[%itemM] * %smith);
			if(%atk < 1 && %atk > 0) {
				if(%className == "Weapon" && %type != $RangedAccessoryType)
					%atk = 1;
			}

			%mdef = floor($Smith::Mod::MDEF[%itemM] * %smith);
			if(%mdef < 1 && %mdef > 0) {
				if(%shape == "Robed" || %type == $TalismanAccessoryType)
					%mdef = 1;
			}

			%str = floor($Smith::Mod::STR[%itemM] * %smith);

			%dex = floor($Smith::Mod::DEX[%itemM] * %smith);

			%con = floor($Smith::Mod::CON[%itemM] * %smith);

			%int = floor($Smith::Mod::INT[%itemM] * %smith);

			%wis = floor($Smith::Mod::WIS[%itemM] * %smith);

			%hp = floor($Smith::Mod::HP[%itemM] * %smith);

			%mp = floor($Smith::Mod::MP[%itemM] * %smith);

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		////////////////////////////////////////
			%Nsvar = "";
			if(%atk != 0)
				%Nsvar = %Nsvar@"6 "@%atk@" ";
			if(%def != 0)
				%Nsvar = %Nsvar@"7 "@%def@" ";
			if(%mdef != 0)
				%Nsvar = %Nsvar@"15 "@%mdef@" ";
			if(%str != 0)
				%Nsvar = %Nsvar@"1 "@%str@" ";
			if(%dex != 0)
				%Nsvar = %Nsvar@"2 "@%dex@" ";
			if(%con != 0)
				%Nsvar = %Nsvar@"3 "@%con@" ";
			if(%int != 0)
				%Nsvar = %Nsvar@"4 "@%int@" ";
			if(%wis != 0)
				%Nsvar = %Nsvar@"5 "@%wis@" ";
			if(%hp != 0)
				%Nsvar = %Nsvar@"13 "@%hp@" ";
			if(%mp != 0)
				%Nsvar = %Nsvar@"14 "@%mp@" ";

			if(%Nsvar == "")
				%Nsvar = "7 1";

		////////////////////////////////////////

			if($Smith::Mod::ForceWeight[%itemM] == "") {
				if(%smith > 0 && $Smith::Mod::Weight[%itemM] > 0)
					%weight = Cap(FixDecimals(pow(%smith, Cap($Smith::Mod::Weight[%itemM], 0, 50))), 0.1, 500); //Cap here because if to high a overflow error pops-up (freezing the game)
				else
					%weight = 0;
			}
			else
				%weight = $Smith::Mod::ForceWeight[%itemM];

		////////////////////////////////////////

			%range = $Smith::Mod::Range[%itemM];
			%delay = $Smith::Mod::Delay[%itemM];

			%dtype = $Smith::Mod::DType[%itemM];

			%ammo = $Smith::Mod::Ammo[%itemM];

		////////////////////////////////////////
			%sound = $Smith::Mod::Sound[%itemM];
			if(getWordCount(%sound) > 1) {

				%s[0] = getWord(%sound, 0);

				for(%i = 1; (%s[%i] = getWord(%sound, %i)) != -1; %i++) {}

				%a = 1000/%i;
				for(%j = 0; %j < %i; %i--) {

					if(%smith >= %a*%i) {
						%Nsound = %s[0]@%s[%i];
						break;
					}
				}
				if(%Nsound != "")
					%sound = %Nsound;
				else
					%sound = %s[0]@%s[1];
			}
		////////////////////////////////////////

		////////////////////////////////////////
			if(%shape != "NULL" && %shape != "") {
				%fixshape = String::replace(%shape@%delay, ".", "");
				if(%grp == "NULL")
					%fixshape = "Bot"@%fixshape;
				else
					%grp = "";

				if($CheckFunc[%fixshape] != "Loaded" && $ServerLoaded)
					Client::sendMessage(%Client, 1, "New Model Shape. Due to limits, the shape will not load till server re-starts.");
			}
		/////////////////////////////////////////

			%activatesound = "";
			if(	%item == $Smith::Stuff[Sling] || %item == $Smith::Stuff[ShortBow] || %item == $Smith::Stuff[LongBow] ||
				%item == $Smith::Stuff[CrossBow] || %item == $Smith::Stuff[HeavyCrossBow] || %item == $Smith::Stuff[CompBow]) {
				 %activatesound = CrossbowSwitch1;
			 }

			%pos = String::findSubStr(%partname, "Fabric");
			if(%pos != -1)
				%partname = String::getSubStr(%partname, 0, %pos-1);

			if(%className == "Weapon")
				%Equip = false;

			if(%Equip != "" && $Smith::Mod::AddSound[%itemM] != true)
				%Equip = true;

			$ClientData[%Client, EvalSmithItem] = "MakeItem(\""@%partname@" "@$Smith::Mod[%item]@"\", \""@%partname@" "@$Smith::Mod[%item]@"\", \""@%Nsvar@"\", \""@%type@"\", \""@%weight@"\", \"A "@$Smith::Mod[%item]@" forged from <f1>"@%part@"<f0>.\", \""@%header@"\", \""@%shape@"\", \""@%className@"\", \""@%Equip@"\", \"\", \""@%dtype@"\", \""@%range@"\", \""@%delay@"\", \""@%sound@"\", \""@%activatesound@"\", \""@%ammo@"\");";
			eval($ClientData[%Client, EvalSmithItem]);
			$ClientData[%Client, GiveSmithItem] = String::ConvertSpaces(%partname@" "@$Smith::Mod[%item]);

		//	echo($ClientData[%Client, EvalSmithItem]);
			return 1;
		}
		//=============================================================================================================
		else { //alter a old item

			%dataname = getWord(%list, 1);

			//$ItemData[$ItemDataCounter, Count] = %dataname;
			//$ItemData[""@%Nname@"", DataName] = %dataname;
			//$ItemData[%dataname, FixCaps] = %dataname;
			//$ItemData[%dataname, Name] = %name;

			%svar = $ItemData[%dataname, svar];
			%type = $ItemData[%dataname, type];
			%weight = $ItemData[%dataname, weight];
			//%info = $ItemData[%dataname, info];
			%header = $ItemData[%dataname, header];
			%shape = $ItemData[%dataname, shape];
			%className = $ItemData[%dataname, className];

			%Equip = $ItemData[%dataname, Equip] = %Equip;

			%dtype = $ItemData[%dataname, DamageType];
			%sound = $ItemData[%dataname, Sound];
			%activatesound = $ItemData[%dataname, ASound];
			%range = $ItemData[%dataname, Range];
			%delay = $ItemData[%dataname, Delay];

			//---
			%pos = String::findSubStr(%partname, "Fabric");
			if(%pos != -1)
				%partname = String::getSubStr(%partname, 0, %pos-1);

			if(%className == "Weapon")
				%Equip = false;

			if(%Equip != "" && $Smith::Mod::AddSound[%itemM] != true) ///Only if AddSound is true && is accessory add sound
				%Equip = true;

			$ClientData[%Client, EvalSmithItem] = "MakeItem(\""@%partname@" "@$Smith::Mod[%item]@"\", \""@%partname@" "@$Smith::Mod[%item]@"\", \""@%Nsvar@"\", \""@%type@"\", \""@%weight@"\", \"A "@$Smith::Mod[%item]@" forged from <f1>"@%part@"<f0>.\", \""@%header@"\", \""@%shape@"\", \""@%className@"\", \""@%Equip@"\", \"\", \""@%dtype@"\", \""@%range@"\", \""@%delay@"\", \""@%sound@"\", \""@%activatesound@"\", \""@%ammo@"\");";
			eval($ClientData[%Client, EvalSmithItem]);
			$ClientData[%Client, GiveSmithItem] = String::ConvertSpaces(%partname@" "@$Smith::Mod[%item]);

//			$ClientData[%Client, EvalSmithItem] = "MakeItem(\""@%part@" "@%item@" +\", \""@%part@" "@%item@" +\", \""@%svar@"\", \""@%type@"\", \""@%weight@"\", \"A "@%item@" forged from <f1>"@%part@"<f0>.\", \""@%header@"\", \""@%shape@"\", \""@%className@"\", \""@%Equip@"\", \"\", \""@%dtype@"\", \""@%range@"\", \""@%delay@"\", \""@%sound@"\", \""@%activatesound@"\", \""@%ammo@"\");";
			return 1;
		}
	}

	return 0;
}

function getPercentOf(%num, %percent) {
	if(%num == "" || %percent == "") {
		echo("getPercentOf(Num, Percent);");
		return;
	}

	if(%percent > 100) %percent = 100;
	else if(%percent < 0) %percent = 0;

	%num = round(%num * (%percent/100));
	if(%num < 0) %num = 0;

	return %num;
}


//50 = 0.50%
//125 = 1.25%
//1,234 = 12.34%
//10,000 = 100.00%
function UseBlackSmithSkill(%Client, %item, %bool) {

	if(%bool) {

		%skill = $ClientData[%Client, Skill_BlackSmith];

		if(%skill >= 10000) //Maxed out
			return;

		%cost = $ItemCost[%item];

		%a = %skill * 15;
		%add = floor(%cost / %a); //echo("c:"@%cost@" a:"@%a@" add:"@%add@"");

		%showadd = ConvNum(%add);

		$ClientData[%Client, Skill_BlackSmith] += %add;

		if($ClientData[%Client, Skill_BlackSmith] >= 10000)
			$ClientData[%Client, Skill_BlackSmith] = 10000;
		else if($ClientData[%Client, Skill_BlackSmith] < 0)
			$ClientData[%Client, Skill_BlackSmith] = 1;

		%nSkill = ConvNum($ClientData[%Client, Skill_BlackSmith]);

		Client::SendMessage(%Client, 0, "BlackSmith skill raised by "@%showadd@"%. It is now "@%nSkill@"%.");

	}
	else { //Failed.. only get 0.01% +
		$ClientData[%Client, Skill_BlackSmith] += 1;
	}
}

function ConvNum(%n) {

	%len = String::len(%n);

	if(%len <= 2) {
		if(%len == 1)
			%a = 2;
		else
			%a = 1;

		for(%i = 0; %i < %a; %i++ && %len++ && %n = "0"@%n) {}

	}

	%loc = %len-2;

	return String::NEWgetSubStr(%n, 0, %loc)@"."@String::NEWgetSubStr(%n, %loc, 99999);
}

function GetSmithComboCost(%Client, %item) {
	return Cap( round((GetStuffStringCost(%Client, %item) * 0.75)), 1, "inf");
}

function _GetSmithComboCost(%Client) {

	return Cap( round((GetStuffStringCost(%Client, $ClientData[%Client, tmpSmith]) * 0.75)), 1, "inf");
}

function GetStuffStringCost(%Client, %itemlist) {

	%cost = 0;
	for(%i = 0; (%w = GetWord(%itemlist, %i)) != -1; %i++)
	{
		%w2 = Cap(floor(GetWord(%itemlist, %i+1)), 1, 99);
		%c = getBuyCost(%Client, %w) * %w2;
		%cost += %c;
	}

	return %cost;
}

function LoadBlackSmithItems() {

	exec("[RM]BlackSmithItems.cs");
	for(%i = 0; $RM_BLACK_SMITH_ITEMS[%i] != ""; %i++)
		eval($RM_BLACK_SMITH_ITEMS[%i]);

}

exec("[RM]BlackSmithItems.cs");

function SaveSmithItem(%Client) { //, %name, %dataname, %svar, %type, %w, %info, %header, %shape, %className, %Equip, %grp, %dmgtype, %range, %delay, %sound, %activatesound) {

	if(!isFile("temp\\[RM]BlackSmithItems.cs")) {

		%obj = "RM_BLACK_SMITH_ITEMS";

		flushExportText();

		newobject(%obj, FearGuiFormattedText);

		AddExportText("");
		AddExportText("///////////////////////////////////////////////////////");
		AddExportText("// These are the players smithed items");
		AddExportText("// Delete this file if you are clearing all chars");
		AddExportText("///////////////////////////////////////////////////////");
		AddExportText("");
		AddExportText("");
		AddExportText("//------------");
		AddExportText("// Do not edit");
		AddExportText("if(isObject("@%obj@"))");
		AddExportText("	deleteObject("@%obj@");");
		AddExportText("//------------");
		AddExportText("");
		AddExportText("");
		AddExportText("## -<<->>-<<->>-<<->>- ITEMS -<<->>-<<->>-<<->>- ##");
		AddExportText("");
		exportObjectToScript(%obj, "temp\\[RM]BlackSmithItems.cs");

		deleteobject(%obj);
		flushExportText();
	}

	for(%i = 0; $RM_BLACK_SMITH_ITEMS[%i] != ""; %i++) {}

	%res = eval($ClientData[%Client, EvalSmithItem]); //MakeItem(%name, %dataname, %svar, %type, %w, %info, %header, %shape, %className, %Equip, %grp, %dmgtype, %range, %delay, %sound, %activatesound);
	//echo("SaveItem: "@getWord($ClientData[%Client, EvalSmithItem], 2)@getWord($ClientData[%Client, EvalSmithItem], 3));

	//Item already made.
	if(%res == "declared") {}

	//errors
	else if(%res == "undefined") {
		Client::sendMessage(%Client, 1, "SMITHING ERROR! ITEM UNDEFINED");
	}
	else if(%res == "illegal") {
		Client::sendMessage(%Client, 1, "SMITHING ERROR! ILLEGAL ITEM NAME");
	}

	//NEW ITEM
	else {

		if(%Client != $ServerClient) { //if %Client == $ServerClient don't save item, its not a custom player item

			$RM_BLACK_SMITH_ITEMS[%i] = $ClientData[%Client, EvalSmithItem]; //"MakeItem(\""@%name@"\", \""@%dataname@"\", \""@%svar@"\", \""@%type@"\", \""@%w@"\", \""@%info@"\", \""@%header@"\", \""@%shape@"\", \""@%className@"\", \""@%Equip@"\", \""@%grp@"\", \""@%dmgtype@"\", \""@%range@"\", \""@%delay@"\", \""@%sound@"\", \""@%activatesound@"\");";

			export("$RM_BLACK_SMITH_ITEMS"@%i, "temp\\[RM]BlackSmithItems.cs", 1);
		}

	//	else {
	//		for(%i = 0; $RM_BLACK_SMITH_ITEMS_DEBUG[%i] != ""; %i++) {}
	//		$RM_BLACK_SMITH_ITEMS_DEBUG[%i] = $ClientData[%Client, EvalSmithItem];
	//		export("$RM_BLACK_SMITH_ITEMS_DEB*", "temp\\[RM]BlackSmithItems_DEBUG.cs");
	//	}

//$RM_BLACK_SMITH_ITEMS[0] = "MakeItem(\"Deus Ring\",\"Deus Ring\",\"1 99 2 99 3 99 4 99 5 99 6 99 7 99 11 99 13 99 14 99 15 99\",1,0,\"Deus Ring\",\"Rings\",\"NULL\",\"Accessory\",true,\",Warrior,\");";
// MakeItem("Deus Ring","Deus Ring","1 99 2 99 3 99 4 99 5 99 6 99 7 99 11 99 13 99 14 99 15 99",1,0,"Deus Ring","Rings","NULL","Accessory",true,",Warrior,");


	}
	$ClientData[%Client, EvalSmithItem] = "";
}