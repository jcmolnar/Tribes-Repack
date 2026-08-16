//There are FOUR hard-coded groups:
//-Priest
//-Rogue
//-Warrior
//-Wizard

//Each of these has classes.  They are specified in here.

$initcoins[Priest] = "3d6x10";
$initcoins[Rogue] = "2d6x10";
$initcoins[Warrior] = "5d4x10";
$initcoins[Wizard] = "1d4+1x10";

$ClassName[1] = "Cleric";
$ClassName[2] = "Druid";
$ClassName[3] = "Thief";
$ClassName[4] = "Bard";
$ClassName[5] = "Fighter";
$ClassName[6] = "Paladin";
$ClassName[7] = "Ranger";
$ClassName[8] = "Mage";
$ClassName[9] = "Summoner";

$ClassName[10] = "Machine";
$ClassName[11] = "Zolom";

$ClassGroup[Cleric] = "Priest";
$ClassGroup[Druid] = "Priest";
$ClassGroup[Thief] = "Rogue";
$ClassGroup[Bard] = "Rogue";
$ClassGroup[Fighter] = "Warrior";
$ClassGroup[Paladin] = "Warrior";
$ClassGroup[Ranger] = "Warrior";
$ClassGroup[Mage] = "Wizard";
$ClassGroup[Summoner] = "Wizard";

//===================================
// Warrior 'upgrade' names
//===================================
$ClassName[1, 0] = "Cleric";
$ClassName[2, 0] = "Druid";
$ClassName[3, 0] = "Thief";
$ClassName[4, 0] = "Bard";
$ClassName[5, 0] = "Fighter";
$ClassName[6, 0] = "Paladin";
$ClassName[7, 0] = "Ranger";
$ClassName[8, 0] = "Mage";
$ClassName[9, 0] = "Conjurer";
$ClassName[10, 0] = "Wisp";
$ClassName[11, 0] = "Snake";

$ClassName[1, 1] = "Bishop";
$ClassName[2, 1] = "Archdruid";
$ClassName[3, 1] = "Bandit";
$ClassName[4, 1] = "Lyricist";
$ClassName[5, 1] = "Berzerker";
$ClassName[6, 1] = "Avenger";
$ClassName[7, 1] = "Woodsman";
$ClassName[8, 1] = "Archmage";
$ClassName[9, 1] = "Summoner";
$ClassName[10, 1] = "Death";
$ClassName[11, 1] = "Viper";

$ClassName[1, 2] = "Saint";
$ClassName[2, 2] = "Mystic";
$ClassName[3, 2] = "Outlaw";
$ClassName[4, 2] = "Musician";
$ClassName[5, 2] = "Knight";
$ClassName[6, 2] = "Centurian";
$ClassName[7, 2] = "Marksman";
$ClassName[8, 2] = "Sorcerer";
$ClassName[9, 2] = "Summoner";
$ClassName[10, 2] = "Reaper";
$ClassName[11, 2] = "Cow";

$ClassName[1, 3] = "Pope";
$ClassName[2, 3] = "Head Mystic";
$ClassName[3, 3] = "Klepto";
$ClassName[4, 3] = "Producer";
$ClassName[5, 3] = "Guardian";
$ClassName[6, 3] = "Crusader";
$ClassName[7, 3] = "Archer";
$ClassName[8, 3] = "Wizard";
$ClassName[9, 3] = "Master Summoner";
$ClassName[10, 3] = "Doom Reaper";
$ClassName[11, 3] = "Zolom";

$ClassName[1, 4] = "Godly Pope";
$ClassName[2, 4] = "Head Mystic God";
$ClassName[3, 4] = "Klepto God";
$ClassName[4, 4] = "Producer God";
$ClassName[5, 4] = "Godly Guardian";
$ClassName[6, 4] = "Godly Crusader";
$ClassName[7, 4] = "Godly Archer";
$ClassName[8, 4] = "Godly Wizard";
$ClassName[9, 4] = "Master Summoner God";
$ClassName[10, 4] = "Doom Reaper God";
$ClassName[11, 4] = "Zolom God";


//===Limit can #steal

$LimitSteal[Priest] = "-150";
$LimitSteal[Rogue] = "0";
$LimitSteal[Warrior] = "-100";
$LimitSteal[Wizard] = "-200";

//

function IsAClass(%class) {

	for(%i = 1; $ClassName[%i] != ""; %i++)
	{
		if(String::ICompare(%class, $ClassName[%i]) == 0)
			return True;
	}

	return False;
}

function UpdateClassName(%clientId) {

	%a = getFinalLVL(%clientId);

	for(%i = 1; $ClassName[%i] != ""; %i++) {
		if($ClassName[%i] == $CLASS[%ClientId]) {
			%ClassNum = %i;
			break;
		}
	}
	if(%a >= 50 && %a <= 99)
		$CLASSN[%clientId] = $ClassName[%ClassNum, 1];
	else if(%a >= 100 && %a <= 299)
		$CLASSN[%clientId] = $ClassName[%ClassNum, 2];
	else if(%a >= 300 && %a <= 998)
		$CLASSN[%clientId] = $ClassName[%ClassNum, 3];
	else if(%a >= 999)
		$CLASSN[%clientId] = $ClassName[%ClassNum, 4];
	else
		$CLASSN[%clientId] = $ClassName[%ClassNum, 0];
}

function CheckClassNames(%name) {
	if(%name == "") {
		for(%i = 1; $ClassName[%i] != ""; %i++) {	//Make sure there is a name for the 'upgrade' names...
			for(%j = 0; %j <= 3; %j++) {
				if($ClassName[%i, %j] == "") {
					echo(">> Error: Class \'"@$ClassName[%i]@"\' has no name for upgrade level "@%j@". Setting name "@$ClassName[%i] @ %j);
					$ClassName[%i, %j] = $ClassName[%i] @ %j;
				}
			}
		}
	}
	else {
		for(%i = 1; $ClassName[%i] != ""; %i++) {
			if($ClassName[%i] == %name) {
				%ClassNum = %i;
				break;
			}
		}
		for(%j = 0; %j <= 3; %j++) {
			if($ClassName[%ClassNum, %j] == "") {
				echo(">> Error: Class \'"@$ClassName[%ClassNum]@"\' has no name for upgrade level "@%j@". Setting name "@$ClassName[%ClassNum] @ %j);
				$ClassName[%ClassNum, %j] = $ClassName[%ClassNum] @ %j;
			}
		}
	}
}
CheckClassNames();