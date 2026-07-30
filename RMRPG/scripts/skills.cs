

function SkillCanUse(%Client, %thing, %echo) {

	if(%Client.adminLevel >= 5 || Player::isAiControlled(%Client))
		return True;

	if($GroupRestrictions[%thing] != "" && String::findSubStr($GroupRestrictions[%thing], "," @ $GROUP[%Client] @ ",") == -1) {
		if(%echo) Client::sendMessage(%Client, 1, "You can't equip this item because of your class.~wC_BuySell.wav");
		return False;
	}

	for(%i = 0; (%s = GetWord($ItemData[%thing, ToUseSkill], %i)) != -1; %i++) { %i++;
		%n = GetWord($ItemData[%thing, ToUseSkill], %i);

		if(%s == "LVL") {
			if(getFinalLVL(%Client) < %n) {
				if(%echo) Client::sendMessage(%Client, 1, "Your level is to low to use this.~wC_BuySell.wav");
				return False;
			}
		}
		else if(%s == "A") {
			if(%Client.adminLevel < %n) {
				if(%echo) Client::sendMessage(%Client, 1, "Only admins may use this.~wC_BuySell.wav");
				return False;
			}
		}
		else if(%s == "CLASS") {
			if(String::findSubStr(%n, ","@$CLASS[%Client]@",") == -1) {
				if(%echo) Client::sendMessage(%Client, 1, "You can't equip this item because of your class.~wC_BuySell.wav");
				return False;
			}
		}
		else if(%s == "GROUP") {
			if(String::findSubStr(%n, ","@$GROUP[%Client]@",") == -1) {
				if(%echo) Client::sendMessage(%Client, 1, "You can't equip this item because of your class.~wC_BuySell.wav");
				return False;
			}
		}
		else {
			%ap = Eval("getFinal"@%s@"(%Client);");
			if(%ap < %n) {
				if(%echo) Client::sendMessage(%Client, 1, "You need more "@%s@" to use this.~wC_BuySell.wav");
				return False;
			}
		}
	}
	return True;
}

function SkillCanUseSpell(%Client, %index, %spelltype, %echo) {

	if(%Client.adminLevel >= 5 || Player::isAiControlled(%Client) || %spelltype == 0)
		return True;

	if(getMANA(%Client) < $Spell::manaCost[%index]) {
		if(%echo) Client::sendMessage(%Client, $MsgBeige, "Insufficient mana to cast this spell.");
		return False;
	}

	if($Spell::classRestrictions[%index] != "" && String::findSubStr($Spell::classRestrictions[%index], "," @ $CLASS[%Client] @ ",") == -1) {
		if(%echo) Client::sendMessage(%Client, 1, "You can't cast this spell because of your class.~wC_BuySell.wav");
		return False;
	}

	for(%i = 0; (%s = GetWord($Spell::ToUseSkill[%index], %i)) != -1; %i++) { %i++;
		%n = GetWord($Spell::ToUseSkill[%index], %i);

		if(%s == "LVL") {
			if(getFinalLVL(%Client) < %n) {
				if(%echo) Client::sendMessage(%Client, 1, "Your level is to low to cast this spell.~wC_BuySell.wav");
				return False;
			}
		}
		else if(%s == "A") {
			if(%Client.adminLevel < %n) {
				if(%echo) Client::sendMessage(%Client, 1, "Only admins may cast this spell.~wC_BuySell.wav");
				return False;
			}
		}
		else if(%s == "CLASS") {
			if(String::findSubStr(%n, ","@$CLASS[%Client]@",") == -1) {
				if(%echo) Client::sendMessage(%Client, 1, "You can't cast this spell because of your class.~wC_BuySell.wav");
				return False;
			}
		}
		else if(%s == "GROUP") {
			if(String::findSubStr(%n, ","@$GROUP[%Client]@",") == -1) {
				if(%echo) Client::sendMessage(%Client, 1, "You can't cast this spell because of your class.~wC_BuySell.wav");
				return False;
			}
		}
		else {
			%ap = Eval("getFinal"@%s@"(%Client);");
			if(%ap < %n) {
				if(%echo) Client::sendMessage(%Client, 1, "You need more "@%s@" to cast this spell.~wC_BuySell.wav");
				return False;
			}
		}
	}
	return True;
}








######################################################################

$SlashingDamageType	= 1;
$PiercingDamageType	= 2;
$ProjectileDamageType = 3;
$BludgeoningDamageType	= 4;

// Aliases for $AttackBonus indexing. rpgfunk (new-player init + ClearVariables) and Ai.cs
// (bot skill setup) referenced $SkillSlashing/$SkillBludgeoning/$SkillPiercing/$SkillArchery,
// which were never defined -> they resolved to "" and wrote attack bonuses to a bogus empty
// key. That meant bots never got their attack bonus, and a new player on a reused client slot
// could inherit the previous player's bonus (the real key was never cleared/initialized). The
// damage read (playerdamage) and save/load use the $*DamageType keys, so alias to those.
$SkillSlashing    = $SlashingDamageType;
$SkillPiercing    = $PiercingDamageType;
$SkillArchery     = $ProjectileDamageType;
$SkillBludgeoning = $BludgeoningDamageType;

$MaxAttackBonusSkill = 10;

//$SkillDifficulty[$SlashingDamageType] = 1.0;
//$SkillDifficulty[$PiercingDamageType] = 1.0;
//$SkillDifficulty[$ProjectileDamageType] = 1.0;
//$SKillDifficulty[$BludgeoningDamageType] = 1.0;

$SkillDesc[$SlashingDamageType] = "Slashing";
$SkillDesc[$PiercingDamageType] = "Piercing";
$SkillDesc[$ProjectileDamageType] = "Archery";
$SkillDesc[$BludgeoningDamageType] = "Bludgeoning";



function SkillCounter(%Client, %dClient, %skilltype, %showmsg) {

	%skillRangePerLevel = 0.0201; //about every 50 levels
	%ub = (%skillRangePerLevel * getFinalLVL(%Client));

	if($Skill[%Client, %skilltype] < $MaxAttackBonusSkill && $Skill[%Client, %skilltype] < %ub && getFinalLVL(%Client)-100 < getFinalLVL(%dClient)) {

		$SkillCounter[%Client, %skilltype]++;

		%a = $Skill[%Client, %skilltype];
		%b = %a + 1 * 500;

		if($SkillCounter[%Client, %skilltype] >= %b) {
			$SkillCounter[%Client, %skilltype] -= %b;
			$AttackBonus[%skilltype, %Client]++;

			if(%showmsg)
				Client::sendMessage(%Client, $MsgBeige, "Your skill in "@$SkillDesc[%skilltype]@" increased by 1. ("@$AttackBonus[%skilltype, %Client]@")");
		}
	}
}


//function SkillCounter(%Client, %dClient, %skilltype, %showmsg) {
//
//	//%skillRangePerLevel = 0.5;
//	//%ub = (%skillRangePerLevel * getFinalLVL(%Client));
//
//	if($Skill[%Client, %skilltype] < 10) {
//
//		$SkillCounter[%Client, %skilltype]++;
//
//		%a = floor($Skill[%Client, %skilltype] / %skillRangePerLevel);
//		%b = $Skill[%Client, %skilltype] / %skillRangePerLevel;
//		%c = (%b - %a) * %skillRangePerLevel;
//		%d = round(%c + 1);
//
//		%e = floor( ((pow(2, %d)/2) + 16) * $SkillDifficulty[%skilltype]);
//
//		//echo("%d: " @ %d);
//		//echo("%e: " @ %e);
//		//echo("$SkillCounter[" @ %Client @ ", " @ %skilltype @ "]: " @ $SkillCounter[%Client, %skilltype]);
//
//		if($SkillCounter[%Client, %skilltype] >= %e)
//		{
//			$SkillCounter[%Client, %skilltype] = 0;
//			//$Skill[%Client, %skilltype]++;
//			$AttackBonus[%skilltype, %sClient]++;
//
//			if(%showmsg)
//				Client::sendMessage(%Client, $MsgBeige, "Your skill in "@$SkillDesc[%skilltype]@" increased ("@$AttackBonus[%skilltype, %sClient]@")");
//		}
//	}
//}