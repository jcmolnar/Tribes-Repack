//animData[38]"sign over here"
//animData[39]"sign point"
//animData[40]"sign retreat"
//animData[41]"sign stop"
//animData[42]"sign salut"

$MYmaxclients = 32; //BOTmax plus PLAYERmax

$spellgoing=0;
for(%i = 1; %i <= $MYmaxclients; %i++) {
	$ttlholdid[%i] = 0;
	$ttlholdpos[%i] = 0;
	$ttlhold[%i] = 0;
	$ttltype[%i] = 0;
	$fwallttl[%i] = 0;
	$fwallid[%i] = 0;
}

function CastingSpell() {
	$spellgoing=0;
	for(%mynum=1;%mynum<$MYmaxclients;%mynum++) {
		if($fwallttl[%mynum]>0) {
			$fwallttl[%mynum]-=1;
			$spellgoing=1;
		}
		if($fwallttl[%mynum]<=0&&isObject($fwallid[%mynum])) {
			//Item::Pop("@$fwallid[%mynum]@");
			//deleteObject("@$fwallid[%mynum]@");
			$fwallid[%mynum]=0;
			$fwallttl[%mynum]=0;
		}
	}
	for(%mynum=1;%mynum<$MYmaxclients;%mynum++) {
//if($ttlholdid[%mynum] > 0) echo("CastingSpell: "@$ttlholdid[%mynum]@" | "@$ttlholdpos[%mynum]@" | "@$ttlhold[%mynum]@" | "@$ttltype[%mynum]);
		if($ttlhold[%mynum]>0) {
			$ttlhold[%mynum]-=1;
			$spellgoing=1;
			if($ttltype[%mynum]==12) {
				set_position($ttlholdid[%mynum],$ttlholdpos[%mynum]);
			}
		}

		if(isdead($ttlholdid[%mynum])||$ttlhold[%mynum]<=0) {
			if($ttltype[%mynum]==8) {
				//Client::sendMessage(%Client, $MsgBeige, "Confusion of "@Client::getName(%id)@"wears off");
				//if(%Client != %id)
				Client::sendMessage($ttlholdid[%mynum], $MsgBeige, "You are no longer confused");
				GameBase::setTeam($ttlholdid[%mynum],$ttlholdpos[%mynum]);
				Game::refreshClientScore($ttlholdid[%mynum]);
				UpdateSkin($ttlholdid[%mynum]);
			}
			if($ttltype[%mynum]==10) {
				Client::sendMessage($ttlholdpos[%mynum], $MsgBeige, "Manipulated by "@Client::getName(%id)@"wears off");
				if($ttlholdpos[%mynum] != $ttlholdid[%mynum])
					Client::sendMessage($ttlholdid[%mynum], $MsgGreen, "You are no longer Manipulated");
				//if(!Player::isAiControlled($ttlholdpos[%mynum]))
				//{
					//revert
					Client::setControlObject($ttlholdid[%mynum], $ttlholdid[%mynum]);
					Client::setControlObject($ttlholdpos[%mynum], $ttlholdpos[%mynum]);
					$dumbAIflag[$ttlholdpos[%mynum].possessId] = "";
				//}
				//else
				//{
				//	Client::setControlObject($ttlholdid[%mynum],$ttlholdid[%mynum]);
				//}
			}
			$ttlhold[%mynum]=0;
			$ttltype[%mynum]=0;
			$ttlholdid[%mynum]=0;
			$ttlholdpos[%mynum]=0;
		}
	}
	if($spellgoing==1)
		schedule("CastingSpell();", 1);
}

$MagicType[0] = "Fire";
$MagicType[1] = "Ice";
$MagicType[2] = "Lightning";
$MagicType[3] = "Water";
$MagicType[4] = "Earth";
$MagicType[5] = "Wind";
$MagicType[6] = "Black";
$MagicType[7] = "Status";

$MagicType[Fire] = 0;
$MagicType[Ice] = 1;
$MagicType[Lightning] = 2;
$MagicType[Water] = 3;
$MagicType[Earth] = 4;
$MagicType[Wind] = 5;
$MagicType[Black] = 6;
$MagicType[Status] = 7;

//Data for spells are defined in the SPELL DEFINITIONS section.
//Unfortunately, not everything can be designed there (ie, special effects etc)


$SpellMovementGraceDistance = 4;//2

//-- SPELL DEFINITIONS -------------------------------------------------------------------------------------------

//-----------transportation spells-------------
$Spell::keyword[1] = "teleportd";
$Spell::index[teleport] = 1;
$Spell::name[1] = "Teleport close to nearest zone";
$Spell::description[1] = "Teleports you near a zone";
$Spell::delay[1] = 3.5;
$Spell::recoveryTime[1] = 25;
$Spell::manaCost[1] = 50;
$Spell::startSound[1] = cheespellsound;
$Spell::endSound[1] = ActivateCH;
$Spell::classRestrictions[1] = "";
$Spell::minLevel[1] = 5;
$Spell::groupListCheck[1] = False;

$Spell::keyword[2] = "barqprz";
$Spell::index[transport] = 2;
$Spell::name[2] = "Teleporting to Rin Vale";
$Spell::description[2] = "Teleports to Rin Vale";
$Spell::delay[2] = 4.0;
$Spell::recoveryTime[2] = 35;
$Spell::manaCost[2] = 500;
$Spell::startSound[2] = cheespellsound;
$Spell::endSound[2] = ActivateCH;
$Spell::classRestrictions[2] = "";
$Spell::minLevel[2] = 30;
$Spell::groupListCheck[2] = False;

$Spell::keyword[3] = "advtransportd";
$Spell::index[advtransport] = 3;
$Spell::name[3] = "Advanced Transport to zone";
$Spell::description[3] = "Transports self OR person in line-of-sight to a specific zone";
$Spell::delay[3] = 4.0;
$Spell::recoveryTime[3] = 40;
$Spell::LOSrange[3] = 500;
$Spell::manaCost[3] = 800;
$Spell::startSound[3] = cheespellsound;
$Spell::endSound[3] = ActivateCH;
$Spell::classRestrictions[3] = "";
$Spell::minLevel[3] = 50;
$Spell::groupListCheck[3] = True;

$Spell::keyword[4] = "medic";
$Spell::index[medic] = 4;
$Spell::name[4] = "Curative Magic";
$Spell::description[4] = "Cure self.";
$Spell::delay[4] = 1.5;
$Spell::recoveryTime[4] = 5;
$Spell::damageValue[4] = -80;
$Spell::manaCost[4] = 5;
$Spell::startSound[4] = DeActivateWA;
$Spell::endSound[4] = lilheal;
$Spell::classRestrictions[4] = ",Cleric,Druid,Paladin,Ranger,Thief,Bard,Mage,Summoner,";
$Spell::minLevel[4] = 1;
$Spell::groupListCheck[4] = False;

$Spell::keyword[5] = "medic2";
$Spell::index[medic2] = 5;
$Spell::name[5] = "Curative Magic";
$Spell::description[5] = "Medic 2";
$Spell::delay[5] = 1.5;
$Spell::recoveryTime[5] =  20;
$Spell::damageValue[5] = -200;
$Spell::LOSrange[5] = 500;
$Spell::manaCost[5] = 30;
$Spell::startSound[5] = DeActivateWA;
$Spell::endSound[5] = lilheal;
$Spell::classRestrictions[5] = ",Mage,Summoner,Paladin,Cleric,Druid,Ranger,Bard,";
$Spell::minLevel[5] = 15;
$Spell::groupListCheck[5] = False;

$Spell::keyword[6] = "medic3";
$Spell::index[medic3] = 6;
$Spell::name[6] = "Curative Magic";
$Spell::description[6] = "Medic 3";
$Spell::delay[6] = 1.5;
$Spell::recoveryTime[6] = 30;
$Spell::damageValue[6] = -1000;
$Spell::LOSrange[6] = 500;
$Spell::manaCost[6] = 80;
$Spell::startSound[6] = DeActivateWA;
$Spell::endSound[6] = lilheal;
$Spell::classRestrictions[6] = ",Cleric,Druid,Mage,Summoner,Bard,";
$Spell::minLevel[6] = 40;
$Spell::groupListCheck[6] = False;

$Spell::keyword[7] = "medic4";
$Spell::index[medic4] = 7;
$Spell::name[7] = "Curative Magic";
$Spell::description[7] = "Medic 4";
$Spell::delay[7] = 1.5;
$Spell::recoveryTime[7] = 30;
$Spell::damageValue[7] = -5000;
$Spell::LOSrange[7] = 500;
$Spell::manaCost[7] = 80;
$Spell::startSound[7] = DeActivateWA;
$Spell::endSound[7] = lilheal;
$Spell::classRestrictions[7] = ",Cleric,Druid,Mage,Summoner,Bard,";
$Spell::minLevel[7] = 75;
$Spell::groupListCheck[7] = False;

$Spell::keyword[8] = "confusion";
$Spell::index[confusion] = 8;
$Spell::name[8] = "Status Magic";
$Spell::description[8] = "Confuses foe.";
$Spell::delay[8] = 4;
$Spell::recoveryTime[8] = 30;
$Spell::damageValue[8] = 0;
$Spell::LOSrange[8] = 100;
$Spell::manaCost[8] = 50;
$Spell::startSound[8] = spellstart;
$Spell::endSound[8] = ActivateAR;
$Spell::classRestrictions[8] = ",Mage,";
$Spell::minLevel[8] = 20;
$Spell::groupListCheck[8] = False;
$Spell::Type[8] = $MagicType[Status];

$Spell::keyword[9] = "remove";
$Spell::index[remove] = 9;
$Spell::name[9] = "Status Magic";
$Spell::description[9] = "Removes a person from battle. Distance counts on LVL.";
$Spell::delay[9] = 1.5;
$Spell::recoveryTime[9] = 15;
$Spell::damageValue[9] = 0;
$Spell::LOSrange[9] = 100;
$Spell::manaCost[9] = 10;
$Spell::startSound[9] = spellstart;
$Spell::endSound[9] = ActivateAR;
$Spell::classRestrictions[9] = ",Mage,Druid,";
$Spell::minLevel[9] = 5;
$Spell::groupListCheck[9] = False;
$Spell::Type[9] = $MagicType[Status];

$Spell::keyword[10] = "mindcontrol";
$Spell::index[mindcontrol] = 10;
$Spell::name[10] = "Status Magic";
$Spell::description[10] = "manipulates a person.";
$Spell::delay[10] = 5;
$Spell::recoveryTime[10] = 40;
$Spell::damageValue[10] = 0;
$Spell::LOSrange[10] = 100;
$Spell::manaCost[10] = 150;
$Spell::startSound[10] = spellstart;
$Spell::endSound[10] = ActivateAR;
$Spell::classRestrictions[10] = ",Mage,";
$Spell::minLevel[10] = 150;
$Spell::groupListCheck[10] = False;
$Spell::Type[10] = $MagicType[Status];

$Spell::keyword[11] = "flash";
$Spell::index[flash] = 11;
$Spell::name[11] = "F Element Magic";
$Spell::description[11] = "Level 1 fire magic.";
$Spell::delay[11] = 2;
$Spell::recoveryTime[11] = 5;
$Spell::damageValue[11] = 4;
$Spell::LOSrange[11] = 100;
$Spell::manaCost[11] = 2;
$Spell::startSound[11] = spellstart;
$Spell::endSound[11] = ActivateAR;
$Spell::classRestrictions[11] = ",Thief,Mage,Paladin,Cleric,Druid,Summoner,";
$Spell::minLevel[11] = 1;
$Spell::groupListCheck[11] = False;
$Spell::Type[11] = $MagicType[Fire];

$Spell::keyword[12] = "flash2";
$Spell::index[flash2] = 12;
$Spell::name[12] = "F Element Magic";
$Spell::description[12] = "level 2 fire magic.";
$Spell::delay[12] = 1.5;
$Spell::recoveryTime[12] = 8;
$Spell::radius[12] = 10;
$Spell::damageValue[12] = 55;
$Spell::LOSrange[12] = 500;
$Spell::manaCost[12] = 20;
$Spell::startSound[12] = spellstart;
$Spell::endSound[12] = ActivateAR;
$Spell::classRestrictions[12] = ",Mage,Druid,Summoner,";
$Spell::minLevel[12] = 25;
$Spell::groupListCheck[12] = False;
$Spell::Type[12] = $MagicType[Fire];

$Spell::keyword[13] = "flash3";
$Spell::index[flash3] = 13;
$Spell::name[13] = "F Element Magic";
$Spell::description[13] = "Level 3 fire magic.";
$Spell::delay[13] = 2;
$Spell::recoveryTime[13] = 9;
$Spell::damageValue[13] = 125;
$Spell::LOSrange[13] = 100;
$Spell::manaCost[13] = 50;
$Spell::startSound[13] = spellstart;
$Spell::endSound[13] = ActivateAR;
$Spell::classRestrictions[13] = ",Mage,Druid,Summoner,";
$Spell::minLevel[13] = 100;
$Spell::groupListCheck[13] = False;
$Spell::Type[13] = $MagicType[Fire];

$Spell::keyword[14] = "flash4";
$Spell::index[flash4] = 14;
$Spell::name[14] = "F Element Magic";
$Spell::description[14] = "Extreme Fire Magic.";
$Spell::delay[14] = 2;
$Spell::recoveryTime[14] = 10;
$Spell::damageValue[14] = 250;
$Spell::LOSrange[14] = 300;
$Spell::manaCost[14] = 100;
$Spell::startSound[14] = spellstart;
$Spell::endSound[14] = bigfire;
$Spell::classRestrictions[14] = ",Mage,";
$Spell::minLevel[14] = 200;
$Spell::groupListCheck[14] = False;
$Spell::Type[14] = $MagicType[Fire];

$Spell::keyword[15] = "redredwine";
$Spell::index[redredwine] = 15;
$Spell::name[15] = "I Element Magic";
$Spell::description[15] = "Level 3 magic.";
$Spell::delay[15] = 2;
$Spell::recoveryTime[15] = 8;
$Spell::damageValue[15] = 30;
$Spell::LOSrange[15] = 100;
$Spell::manaCost[15] = 75;
$Spell::startSound[15] = spellstart;
$Spell::endSound[15] = ActivateAR;
$Spell::classRestrictions[15] = ",Mage,Paladin,Druid,Summoner,";
$Spell::minLevel[15] = 50;
$Spell::groupListCheck[15] = False;
$Spell::Type[15] = $MagicType[Ice];

$Spell::keyword[16] = "cold";
$Spell::index[cold] = 16;
$Spell::name[16] = "I Element Magic";
$Spell::description[16] = "Level 1 Ice magic.";
$Spell::delay[16] = 2;
$Spell::recoveryTime[16] = 5;
$Spell::damageValue[16] = 15;
$Spell::LOSrange[16] = 100;
$Spell::manaCost[16] = 10;
$Spell::startSound[16] = spellstart;
$Spell::endSound[16] = ActivateAR;
$Spell::classRestrictions[16] = ",Bard,Ranger,Mage,Paladin,Druid,Summoner,";
$Spell::minLevel[16] = 3;
$Spell::groupListCheck[16] = False;
$Spell::Type[16] = $MagicType[Ice];

$Spell::keyword[17] = "cold2";
$Spell::index[cold2] = 17;
$Spell::name[17] = "I Element Magic";
$Spell::description[17] = "Level 2 Ice Elem.";
$Spell::delay[17] = 2;
$Spell::recoveryTime[17] = 6;
$Spell::damageValue[17] = 70;
$Spell::LOSrange[17] = 100;
$Spell::manaCost[17] = 50;
$Spell::startSound[17] = spellstart;
$Spell::endSound[17] = ActivateAR;
$Spell::classRestrictions[17] = ",Bard,Mage,Paladin,Druid,Summoner,";
$Spell::minLevel[17] = 40;
$Spell::groupListCheck[17] = False;
$Spell::Type[17] = $MagicType[Ice];

$Spell::keyword[18] = "cold3";
$Spell::index[cold3] = 18;
$Spell::name[18] = "I Element Magic";
$Spell::description[18] = "Level 3 Ice Elem.";
$Spell::delay[18] = 1;
$Spell::recoveryTime[18] = 8;
$Spell::damageValue[18] = 140;
$Spell::LOSrange[18] = 300;
$Spell::manaCost[18] = 80;
$Spell::startSound[18] = spellstart;
$Spell::endSound[18] = ActivateAR;
$Spell::classRestrictions[18] = ",Mage,Summoner,Druid,";
$Spell::minLevel[18] = 130;
$Spell::groupListCheck[18] = False;
$Spell::Type[18] = $MagicType[Ice];

$Spell::keyword[19] = "cold4";
$Spell::index[cold4] = 19;
$Spell::name[19] = "I Element Magic";
$Spell::description[19] = "ice magic.";
$Spell::delay[19] = 1.0;
$Spell::recoveryTime[19] = 10;
$Spell::damageValue[19] = 200;
$Spell::LOSrange[19] = 100;
$Spell::manaCost[19] = 100;
$Spell::startSound[19] = ActivateAR;
$Spell::endSound[19] = portal1;
$Spell::classRestrictions[19] = ",Mage,";
$Spell::minLevel[19] = 200;
$Spell::groupListCheck[19] = False;
$Spell::Type[19] = $MagicType[Ice];

$Spell::keyword[20] = "Aqua";
$Spell::index[Aqua] = 20;
$Spell::name[20] = "W Element Magic";
$Spell::description[20] = "Shoots a powerful beam of water.";
$Spell::delay[20] = 1;
$Spell::recoveryTime[20] = 4;
$Spell::damageValue[20] = 40;
$Spell::LOSrange[20] = 500;
$Spell::manaCost[20] = 50;
$Spell::startSound[20] = watershotstart;
$Spell::endSound[20] = watersplash;
$Spell::classRestrictions[20] = ",Bard,Druid,Mage,Paladin,Summoner,";
$Spell::minLevel[20] = 30;
$Spell::groupListCheck[20] = False;
$Spell::Type[20] = $MagicType[Water];

$Spell::keyword[21] = "Aqua2";
$Spell::index[Aqua2] = 21;
$Spell::name[21] = "W Element Magic";
$Spell::description[21] = "Fires water at foe to shoot them away.";
$Spell::delay[21] = 1.5;
$Spell::recoveryTime[21] = 6;
$Spell::damageValue[21] = 60;
$Spell::LOSrange[21] = 500;
$Spell::manaCost[21] = 80;
$Spell::startSound[21] = watershotstart;
$Spell::endSound[21] = watersplash;
$Spell::classRestrictions[21] = ",Druid,Mage,Summoner,";
$Spell::minLevel[21] = 50;
$Spell::groupListCheck[21] = False;
$Spell::Type[21] = $MagicType[Water];

$Spell::keyword[22] = "Aqua3";
$Spell::index[Aqua3] = 22;
$Spell::name[22] = "W Element Magic";
$Spell::description[22] = "Water Level 3 Magic.";
$Spell::delay[22] = 0;
$Spell::recoveryTime[22] = 3;
$Spell::damageValue[22] = 120;
$Spell::LOSrange[22] = 800;
$Spell::manaCost[22] = 120;
$Spell::startSound[22] = watershotstart;
$Spell::endSound[22] = watersplash;
$Spell::classRestrictions[22] = ",Mage,";
$Spell::minLevel[22] = 100;
$Spell::groupListCheck[22] = False;
$Spell::Type[22] = $MagicType[Water];

$Spell::keyword[23] = "Aqua4";
$Spell::index[Aqua4] = 23;
$Spell::name[23] = "W Element Magic";
$Spell::description[23] = "Water Level 4 Magic.";
$Spell::delay[23] = 1.5;
$Spell::recoveryTime[23] = 6;
$Spell::damageValue[23] = 300;
$Spell::manaCost[23] = 200;
$Spell::startSound[23] = watershotstart;
$Spell::endSound[23] = watersplash;
$Spell::classRestrictions[23] = ",Mage,";
$Spell::groupListCheck[23] = False;
$Spell::minLevel[23] = 200;
$Spell::Type[23] = $MagicType[Water];

$Spell::keyword[24] = "Shatter";
$Spell::index[Shatter] = 24;
$Spell::name[24] = "E Element Magic";
$Spell::description[24] = "Level 1 Earth magic.";
$Spell::delay[24] = 2;
$Spell::recoveryTime[24] = 5;
$Spell::damageValue[24] = 26;
$Spell::LOSrange[24] = 500;
$Spell::manaCost[24] = 25;
$Spell::startSound[24] = spellstart;
$Spell::endSound[24] = shockExplosion;
$Spell::classRestrictions[24] = ",Bard,Mage,Paladin,Druid,Summoner,";
$Spell::minLevel[24] = 30;
$Spell::groupListCheck[24] = False;
$Spell::Type[24] = $MagicType[Earth];

$Spell::keyword[25] = "Shatter2";
$Spell::index[Shatter2] = 25;
$Spell::name[25] = "E Element Magic";
$Spell::description[25] = "Level 2 Earth magic.";
$Spell::delay[25] = 3;
$Spell::recoveryTime[25] = 6;
$Spell::damageValue[25] = 100;
$Spell::LOSrange[25] = 500;
$Spell::manaCost[25] = 50;
$Spell::startSound[25] = spellstart;
$Spell::endSound[25] = shockExplosion;
$Spell::classRestrictions[25] = ",Bard,Mage,Paladin,Druid,Summoner,";
$Spell::minLevel[25] = 60;
$Spell::groupListCheck[25] = False;
$Spell::Type[25] = $MagicType[Earth];

$Spell::keyword[26] = "Shatter3";
$Spell::index[Shatter3] = 26;
$Spell::name[26] = "E Element Magic";
$Spell::description[26] = "Level 3 Earth magic.";
$Spell::delay[26] = 4;
$Spell::recoveryTime[26] = 8;
$Spell::damageValue[26] = 140;
$Spell::LOSrange[26] = 500;
$Spell::manaCost[26] = 100;
$Spell::startSound[26] = spellstart;
$Spell::endSound[26] = shockExplosion;
$Spell::classRestrictions[26] = ",Mage,Summoner,";
$Spell::minLevel[26] = 120;
$Spell::groupListCheck[26] = False;
$Spell::Type[26] = $MagicType[Earth];

$Spell::keyword[27] = "Shatter4";
$Spell::index[Shatter4] = 27;
$Spell::name[27] = "E Element Magic";
$Spell::description[27] = "Level 4 Earth magic.";
$Spell::delay[27] = 6;
$Spell::recoveryTime[27] = 10;
$Spell::damageValue[27] = 250;
$Spell::LOSrange[27] = 500;
$Spell::manaCost[27] = 200;
$Spell::startSound[27] = spellstart;
$Spell::endSound[27] = shockExplosion;
$Spell::classRestrictions[27] = ",Mage,";
$Spell::minLevel[27] = 230;
$Spell::groupListCheck[27] = False;
$Spell::Type[27] = $MagicType[Earth];

$Spell::keyword[28] = "spike";
$Spell::index[spike] = 28;
$Spell::name[28] = "White Attack Magic";
$Spell::description[28] = "Level 1 White Attack.";
$Spell::delay[28] = 1;
$Spell::recoveryTime[28] = 4;
$Spell::damageValue[28] = 2.5;
$Spell::LOSrange[28] = 400;
$Spell::manaCost[28] = 1;
$Spell::startSound[28] = spellstart;
$Spell::endSound[28] = mmsound;
$Spell::classRestrictions[28] = ",Cleric,Paladin,Thief,Bard,Mage,Summoner,";
$Spell::minLevel[28] = 1;
$Spell::groupListCheck[28] = False;
$Spell::Type[28] = $MagicType[White];

$Spell::keyword[29] = "wound";
$Spell::index[wound] = 29;
$Spell::name[29] = "White Attack Magic";
$Spell::description[29] = "Level 2 White Attack.";
$Spell::delay[29] = 1;
$Spell::recoveryTime[29] = 10;
$Spell::damageValue[29] = 10;
$Spell::LOSrange[29] = 200;
$Spell::manaCost[29] = 25;
$Spell::startSound[29] = spellstart;
$Spell::endSound[29] = ActivateAR;
$Spell::classRestrictions[29] = ",Cleric,Paladin,Thief,Bard,";
$Spell::minLevel[29] = 10;
$Spell::groupListCheck[29] = False;
$Spell::Type[29] = $MagicType[White];

$Spell::keyword[30] = "Fist";
$Spell::index[fist] = 30;
$Spell::name[30] = "White Attack Magic";
$Spell::description[30] = "Level 3 White Attack.";
$Spell::delay[30] = 1;
$Spell::recoveryTime[30] = 10;
$Spell::damageValue[30] = 46;
$Spell::LOSrange[30] = 100;
$Spell::manaCost[30] = 50;
$Spell::startSound[30] = spellstart;
$Spell::endSound[30] = ActivateAR;
$Spell::classRestrictions[30] = ",Cleric,Paladin,Thief,Bard,";
$Spell::minLevel[30] = 50;
$Spell::groupListCheck[30] = False;
$Spell::Type[30] = $MagicType[White];

$Spell::keyword[31] = "Missile";
$Spell::index[Missile] = 31;
$Spell::name[31] = "White Attack Magic";
$Spell::description[31] = "Level 4 White Attack.";
$Spell::delay[31] = 6;
$Spell::recoveryTime[31] = 9;
$Spell::damageValue[31] = 90;
$Spell::LOSrange[31] = 100;
$Spell::manaCost[31] = 100;
$Spell::startSound[31] = thunderlight;
$Spell::endSound[31] = shockExplosion;
$Spell::classRestrictions[29] = ",Cleric,Paladin,Bard,";
$Spell::minLevel[31] = 100;
$Spell::groupListCheck[31] = False;
$Spell::Type[31] = $MagicType[White];

$Spell::keyword[32] = "Cannon";
$Spell::index[Cannon] = 32;
$Spell::name[32] = "White Attack Magic";
$Spell::description[32] = "Level 5 White Attack.";
$Spell::delay[32] = 6;
$Spell::recoveryTime[32] = 9;
$Spell::damageValue[32] = 140;
$Spell::LOSrange[32] = 100;
$Spell::manaCost[32] = 150;
$Spell::startSound[32] = thunderlight;
$Spell::endSound[32] = shockExplosion;
$Spell::classRestrictions[32] = ",Cleric,Paladin,Bard,";
$Spell::minLevel[32] = 130;
$Spell::groupListCheck[32] = False;
$Spell::Type[32] = $MagicType[White];

$Spell::keyword[33] = "bomb";
$Spell::index[bomb] = 33;
$Spell::name[33] = "White Attack Magic";
$Spell::description[33] = "Level 6 White Attack.";
$Spell::delay[33] = 6;
$Spell::recoveryTime[33] = 10;
$Spell::damageValue[33] = 200;
$Spell::LOSrange[33] = 200;
$Spell::manaCost[33] = 200;
$Spell::startSound[33] = spellstart;
$Spell::endSound[33] = ActivateAR;
$Spell::classRestrictions[33] = ",Cleric,";
$Spell::minLevel[33] = 170;
$Spell::groupListCheck[33] = False;
$Spell::Type[33] = $MagicType[White];

$Spell::keyword[34] = "Star";
$Spell::index[star] = 34;
$Spell::name[34] = "White Attack Magic";
$Spell::description[34] = "Level 7 White Attack.";
$Spell::delay[34] = 2;
$Spell::recoveryTime[34] = 5;
$Spell::damageValue[34] = 300;
$Spell::LOSrange[34] = 800;
$Spell::manaCost[34] = 300;
$Spell::startSound[34] = ultimathunder;
$Spell::endSound[34] = spooky;
$Spell::classRestrictions[34] = ",Cleric,";
$Spell::minLevel[34] = 250;
$Spell::groupListCheck[34] = False;
$Spell::Type[34] = $MagicType[White];

$Spell::keyword[35] = "darkspike";
$Spell::index[darkspike] = 35;
$Spell::name[35] = "Black Magic";
$Spell::description[35] = "Level 1 Black Magic.";
$Spell::delay[35] = 2;
$Spell::recoveryTime[35] = 5;
$Spell::damageValue[35] = 10;
$Spell::LOSrange[35] = 300;
$Spell::manaCost[35] = 10;
$Spell::startSound[35] = spellstart;
$Spell::endSound[35] = ActivateAR;
$Spell::classRestrictions[35] = ",Mage,";
$Spell::minLevel[35] = 20;
$Spell::groupListCheck[35] = False;
$Spell::Type[35] = $MagicType[Black];

$Spell::keyword[36] = "darkshot";
$Spell::index[darkshot] = 36;
$Spell::name[36] = "Black Magic";
$Spell::description[36] = "Level 2 Black Magic.";
$Spell::delay[36] = 1;
$Spell::recoveryTime[36] = 5;
$Spell::damageValue[36] = 20;
$Spell::LOSrange[36] = 100;
$Spell::manaCost[36] = 50;
$Spell::startSound[36] = ActivateAR;
$Spell::endSound[36] = watersplash;
$Spell::classRestrictions[36] = ",Mage,";
$Spell::minLevel[36] = 50;
$Spell::groupListCheck[36] = False;
$Spell::Type[36] = $MagicType[Black];

$Spell::keyword[37] = "Surge";
$Spell::index[Surge] = 37;
$Spell::name[37] = "Black Magic";
$Spell::description[37] = "Level 3 Black Magic.";
$Spell::delay[37] = 8;
$Spell::recoveryTime[37] = 30;
$Spell::damageValue[37] = 900;
$Spell::LOSrange[37] = 300;
$Spell::manaCost[37] = 700;
$Spell::startSound[37] = ultimathunder;
$Spell::endSound[37] = spooky;
$Spell::classRestrictions[37] = ",Mage,";
$Spell::minLevel[37] = 200;
$Spell::groupListCheck[37] = False;
$Spell::Type[37] = $MagicType[Black];

$Spell::keyword[38] = "Storm";
$Spell::index[Storm] = 38;
$Spell::name[38] = "L Element Magic";
$Spell::description[38] = "Lightning Level 1 Magic.";
$Spell::delay[38] = 2;
$Spell::recoveryTime[38] = 6;
$Spell::damageValue[38] = 30;
$Spell::LOSrange[38] = 500;
$Spell::manaCost[38] = 40;
$Spell::startSound[38] = spellstart;
$Spell::endSound[38] = thunderlight;
$Spell::classRestrictions[38] = ",Bard,Mage,Druid,Paladin,Summoner,";
$Spell::minLevel[38] = 30;
$Spell::groupListCheck[38] = False;
$Spell::Type[38] = $MagicType[Lightning];

$Spell::keyword[39] = "Storm2";
$Spell::index[Storm2] = 39;
$Spell::name[39] = "L Element Magic";
$Spell::description[39] = "Lightning Level 2 Magic.";
$Spell::delay[39] = 2;
$Spell::recoveryTime[39] = 8;
$Spell::damageValue[39] = 70;
$Spell::LOSrange[39] = 500;
$Spell::manaCost[39] = 60;
$Spell::startSound[39] = spellstart;
$Spell::endSound[39] = thunderlight;
$Spell::classRestrictions[39] = ",Mage,Druid,Paladin,Summoner,";
$Spell::minLevel[39] = 50;
$Spell::groupListCheck[39] = False;
$Spell::Type[39] = $MagicType[Lightning];

$Spell::keyword[40] = "Storm3";
$Spell::index[Storm3] = 40;
$Spell::name[40] = "L Element Magic";
$Spell::description[40] = "Lightning Level 3 Magic.";
$Spell::delay[40] = 2;
$Spell::recoveryTime[40] = 9;
$Spell::damageValue[40] = 120;
$Spell::LOSrange[40] = 500;
$Spell::manaCost[40] = 80;
$Spell::startSound[40] = spellstart;
$Spell::endSound[40] = thunderlight;
$Spell::classRestrictions[40] = ",Mage,Druid,Paladin,Summoner,";
$Spell::minLevel[40] = 70;
$Spell::groupListCheck[40] = False;
$Spell::Type[40] = $MagicType[Lightning];

$Spell::keyword[41] = "Storm4";
$Spell::index[Storm4] = 41;
$Spell::name[41] = "L Element Magic";
$Spell::description[41] = "Lightning Level 4 Magic.";
$Spell::delay[41] = 4;
$Spell::recoveryTime[41] = 30;
$Spell::damageValue[41] = 500;
$Spell::LOSrange[41] = 500;
$Spell::manaCost[41] = 150;
$Spell::startSound[41] = spellstart;
$Spell::endSound[41] = thunderlight;
$Spell::classRestrictions[41] = ",Mage,";
$Spell::minLevel[41] = 150;
$Spell::groupListCheck[41] = False;
$Spell::Type[41] = $MagicType[Lightning];

$Spell::keyword[42] = "Gale";
$Spell::index[Gale] = 42;
$Spell::name[42] = "Wi Element Magic";
$Spell::description[42] = "Wind Level 1 Magic.";
$Spell::delay[42] = 2;
$Spell::recoveryTime[42] = 6;
$Spell::damageValue[42] = 30;
$Spell::LOSrange[42] = 500;
$Spell::manaCost[42] = 30;
$Spell::startSound[42] = spellstart;
$Spell::endSound[42] = thunderlight;
$Spell::radius[42] = "10";
$Spell::classRestrictions[42] = ",Bard,Mage,Druid,Paladin,Summoner,";
$Spell::minLevel[42] = 15;
$Spell::groupListCheck[42] = False;
$Spell::Type[42] = $MagicType[Wind];

$Spell::keyword[43] = "Gale2";
$Spell::index[Gale2] = 43;
$Spell::name[43] = "Wi Element Magic";
$Spell::description[43] = "Wind Level 2 Magic.";
$Spell::delay[43] = 2;
$Spell::recoveryTime[43] = 6;
$Spell::damageValue[43] = 100;
$Spell::LOSrange[43] = 500;
$Spell::manaCost[43] = 50;
$Spell::startSound[43] = spellstart;
$Spell::endSound[43] = thunderlight;
$Spell::radius[43] = "10";
$Spell::classRestrictions[43] = ",Mage,Druid,Paladin,Summoner,";
$Spell::minLevel[43] = 35;
$Spell::groupListCheck[43] = False;
$Spell::Type[43] = $MagicType[Wind];

$Spell::keyword[44] = "Gale3";
$Spell::index[Gale3] = 44;
$Spell::name[44] = "Wi Element Magic";
$Spell::description[44] = "Wind Level 3 Magic.";
$Spell::delay[44] = 3;
$Spell::recoveryTime[44] = 7;
$Spell::damageValue[44] = 200;
$Spell::LOSrange[44] = 500;
$Spell::manaCost[44] = 320;
$Spell::startSound[44] = spellstart;
$Spell::endSound[44] = thunderlight;
$Spell::radius[42] = "15";
$Spell::classRestrictions[44] = ",Mage,Druid,Paladin,Summoner,";
$Spell::minLevel[44] = 50;
$Spell::groupListCheck[44] = False;
$Spell::Type[44] = $MagicType[Wind];

$Spell::keyword[45] = "Gale4";
$Spell::index[Gale4] = 45;
$Spell::name[45] = "Wi Element Magic";
$Spell::description[45] = "Wind Level 4 Magic.";
$Spell::delay[45] = 4;
$Spell::recoveryTime[45] = 10;
$Spell::damageValue[45] = 500;
$Spell::LOSrange[45] = 500;
$Spell::manaCost[45] = 320;
$Spell::startSound[45] = spellstart;
$Spell::endSound[45] = thunderlight;
$Spell::radius[42] = "20";
$Spell::classRestrictions[45] = ",Mage,";
$Spell::minLevel[45] = 150;
$Spell::groupListCheck[45] = False;
$Spell::Type[45] = $MagicType[Wind];

$Spell::keyword[46] = "WindArrow";
$Spell::index[windarrow] = 46;
$Spell::name[46] = "Magic Arrow Ranger Mag.";
$Spell::description[46] = "Fire a magical arrow : elemental Wind.";
$Spell::delay[46] = 0;
$Spell::recoveryTime[46] = 5;
$Spell::damageValue[46] = 30;
$Spell::LOSrange[46] = 800;
$Spell::manaCost[46] = 20;
$Spell::startSound[46] = activateAR;
$Spell::endSound[46] = portal1;
$Spell::classRestrictions[46] = ",Ranger,";
$Spell::minLevel[46] = 10;
$Spell::groupListCheck[46] = False;
$Spell::Type[46] = $MagicType[Wind];

$Spell::keyword[47] = "FireArrow";
$Spell::index[firearrow] = 47;
$Spell::name[47] = "Magic Arrow Ranger Mag.";
$Spell::description[47] = "Fire a magical arrow : elemental Fire.";
$Spell::delay[47] = 0;
$Spell::recoveryTime[47] = 10;
$Spell::damageValue[47] = 50;
$Spell::LOSrange[47] = 800;
$Spell::manaCost[47] = 50;
$Spell::startSound[47] = activateAR;
$Spell::endSound[47] = portal1;
$Spell::classRestrictions[47] = ",Ranger,";
$Spell::minLevel[47] = 20;
$Spell::groupListCheck[47] = False;
$Spell::Type[47] = $MagicType[Fire];

$Spell::keyword[48] = "IceArrow";
$Spell::index[icearrow] = 48;
$Spell::name[48] = "Magic Arrow Ranger Mag.";
$Spell::description[48] = "Fire a magical arrow : elemental Ice.";
$Spell::delay[48] = 0;
$Spell::recoveryTime[48] = 15;
$Spell::damageValue[48] = 100;
$Spell::LOSrange[48] = 800;
$Spell::manaCost[48] = 80;
$Spell::startSound[48] = activateAR;
$Spell::endSound[48] = portal1;
$Spell::classRestrictions[48] = ",Ranger,";
$Spell::minLevel[48] = 30;
$Spell::groupListCheck[48] = False;
$Spell::Type[42] = $MagicType[Ice];

$Spell::keyword[49] = "blockfront";
$Spell::index[blockfront] = 49;
$Spell::name[49] = "Utility Magic";
$Spell::description[49] = "Barrier Magic.";
$Spell::delay[49] = 2;
$Spell::recoveryTime[49] = 5;
$Spell::damageValue[49] = 0;
$Spell::LOSrange[49] = 200;
$Spell::manaCost[49] = 20;
$Spell::startSound[49] = spellstart;
$Spell::endSound[49] = ActivateAR;
$Spell::classRestrictions[49] = ",Mage,Paladin,Fighter,Cleric,Druid,Ranger,Thief,Bard,Summoner,";
$Spell::minLevel[49] = 10;
$Spell::groupListCheck[49] = False;

$Spell::keyword[50] = "blockback";
$Spell::index[blockback] = 50;
$Spell::name[50] = "Utility Magic";
$Spell::description[50] = "Barrier Magic.";
$Spell::delay[50] = 1;
$Spell::recoveryTime[50] = 4;
$Spell::damageValue[50] = 0;
$Spell::LOSrange[50] = 200;
$Spell::manaCost[50] = 20;
$Spell::startSound[50] = spellstart;
$Spell::endSound[50] = ActivateAR;
$Spell::classRestrictions[50] = ",Mage,Paladin,Fighter,Cleric,Druid,Ranger,Thief,Bard,Summoner,";
$Spell::minLevel[50] = 10;
$Spell::groupListCheck[50] = False;

$Spell::keyword[51] = "light";
$Spell::index[light] = 51;
$Spell::name[51] = "Utility Magic";
$Spell::description[51] = "Beam Magic.";
$Spell::delay[51] = 0;
$Spell::recoveryTime[51] = 20;
$Spell::damageValue[51] = null;
$Spell::LOSrange[51] = 200;
$Spell::manaCost[51] = 15;
$Spell::startSound[51] = spellstart;
$Spell::endSound[51] = ActivateAR;
$Spell::classRestrictions[51] = ",Mage,Paladin,Fighter,Cleric,Druid,Ranger,Thief,Bard,Summoner,";
$Spell::minLevel[51] = 10;
$Spell::groupListCheck[51] = False;

$Spell::keyword[52] = "goods";
$Spell::index[goods] = 52;
$Spell::name[52] = "Utility Magic";
$Spell::description[52] = "Converts MP to STA.";
$Spell::delay[52] = 1.5;
$Spell::recoveryTime[52] = 4.5;
$Spell::damageValue[52] = -100;
$Spell::manaCost[52] = 50;
$Spell::startSound[52] = DeActivateWA;
$Spell::endSound[52] = ActivateAR;
$Spell::classRestrictions[52] = ",Cleric,Druid,Fighter,Paladin,Ranger,Thief,Bard,Mage,Summoner,";
$Spell::minLevel[52] = 20;
$Spell::groupListCheck[52] = False;

$Spell::keyword[53] = "goods2";
$Spell::index[goods2] = 53;
$Spell::name[53] = "Utility Magic";
$Spell::description[53] = "Converts MP to STA.";
$Spell::delay[53] = 1.5;
$Spell::recoveryTime[53] = 7;
$Spell::damageValue[53] = -200;
$Spell::manaCost[53] = 100;
$Spell::startSound[53] = DeActivateWA;
$Spell::endSound[53] = ActivateAR;
$Spell::classRestrictions[53] = ",Cleric,Druid,Fighter,Paladin,Ranger,Thief,Bard,Mage,Summoner,";
$Spell::minLevel[53] = 40;
$Spell::groupListCheck[53] = False;

$Spell::keyword[54] = "goods3";
$Spell::index[goods3] = 54;
$Spell::name[54] = "Utility Magic";
$Spell::description[54] = "Converts MP to STA.";
$Spell::delay[54] = 1.5;
$Spell::recoveryTime[54] = 9;
$Spell::damageValue[54] = -300;
$Spell::manaCost[54] = 120;
$Spell::startSound[54] = DeActivateWA;
$Spell::endSound[54] = ActivateAR;
$Spell::classRestrictions[54] = ",Cleric,Druid,Fighter,Paladin,Ranger,Bard,Mage,Summoner,";
$Spell::minLevel[54] = 60;
$Spell::groupListCheck[54] = False;

$Spell::keyword[55] = "goods4";
$Spell::index["goods4"] = %index;
$Spell::name[55] = "Utility Magic";
$Spell::description[55] = "Converts MP to STA.";
$Spell::delay[55] = 2.5;
$Spell::recoveryTime[55] = 14;
$Spell::damageValue[55] = -999;
$Spell::manaCost[55] = 275;
$Spell::startSound[55] = DeActivateWA;
$Spell::endSound[55] = ActivateAR;
$Spell::classRestrictions[55] = ",Cleric,Druid,Bard,Mage,";
$Spell::minLevel[55] = 100;
$Spell::groupListCheck[55] = False;

%index = 56;
$Spell::keyword[%index] = "death";
$Spell::index[$Spell::keyword[%index]] = %index;
$Spell::name[%index] = "Death Summon";
$Spell::description[%index] = "Level 100 Summon.";
$Spell::delay[%index] = 6;
$Spell::recoveryTime[%index] = 40;
$Spell::damageValue[%index] = 400;
$Spell::LOSrange[%index] = 200;
$Spell::manaCost[%index] = 250;
$Spell::startSound[%index] = summonchant;
$Spell::endSound[%index] = hadesgrr;
$Spell::classRestrictions[%index] = ",Summoner,";
$Spell::minLevel[%index] = 100;
$Spell::groupListCheck[%index] = False;

%index = 57;
$Spell::keyword[%index] = "rock";
$Spell::index[$Spell::keyword[%index]] = %index;
$Spell::name[%index] = "Rock Summon";
$Spell::description[%index] = "Level 10 Summon.";
$Spell::delay[%index] = 4;
$Spell::recoveryTime[%index] = 10;
$Spell::damageValue[%index] = 20;
$Spell::LOSrange[%index] = 200;
$Spell::manaCost[%index] = 25;
$Spell::startSound[%index] = summonchant;
$Spell::endSound[%index] = shockExplosion;
$Spell::classRestrictions[%index] = ",Summoner,";
$Spell::minLevel[%index] = 10;
$Spell::groupListCheck[%index] = False;

%index = 58;
$Spell::keyword[%index] = "blizzard";
$Spell::index[$Spell::keyword[%index]] = %index;
$Spell::name[%index] = "Blizzard Summon";
$Spell::description[%index] = "Level 20 Summon.";
$Spell::delay[%index] = 4;
$Spell::recoveryTime[%index] = 14;
$Spell::damageValue[%index] = 40;
$Spell::LOSrange[%index] = 150;
$Spell::manaCost[%index] = 50;
$Spell::startSound[%index] = summonchant;
$Spell::endSound[%index] = shockExplosion;
$Spell::classRestrictions[%index] = ",Summoner,";
$Spell::minLevel[%index] = 20;
$Spell::groupListCheck[%index] = False;

%index = 59;
$Spell::keyword[%index] = "battle";
$Spell::index[$Spell::keyword[%index]] = %index;
$Spell::name[%index] = "Battle Summon";
$Spell::description[%index] = "Level 30 Summon.";
$Spell::delay[%index] = 4;
$Spell::recoveryTime[%index] = 10;
$Spell::damageValue[%index] = 60;
$Spell::LOSrange[%index] = 200;
$Spell::manaCost[%index] = 50;
$Spell::startSound[%index] = summonchant;
$Spell::endSound[%index] = shockExplosion;
$Spell::classRestrictions[%index] = ",Summoner,";
$Spell::minLevel[%index] = 30;
$Spell::groupListCheck[%index] = False;

%index = 60;
$Spell::keyword[%index] = "sapper";
$Spell::index[$Spell::keyword[%index]] = %index;
$Spell::name[%index] = "Sapper Summon";
$Spell::description[%index] = "Level 40 Summon.";
$Spell::delay[%index] = 4;
$Spell::recoveryTime[%index] = 12;
$Spell::damageValue[%index] = 300;
$Spell::LOSrange[%index] = 250;
$Spell::manaCost[%index] = 175;
$Spell::startSound[%index] = summonchant;
$Spell::endSound[%index] = shockExplosion;
$Spell::classRestrictions[%index] = ",Summoner,";
$Spell::minLevel[%index] = 50;
$Spell::groupListCheck[%index] = False;

%index = 61;
$Spell::keyword[%index] = "pem315";
$Spell::index[$Spell::keyword[%index]] = %index;
$Spell::name[%index] = "Ashes to ashes";
$Spell::description[%index] = "Ashes to ashes.";
$Spell::delay[%index] = 1.0;
$Spell::recoveryTime[%index] = 2;
$Spell::LOSrange[%index] = 200;
$Spell::damageValue[%index] = 0;
$Spell::manaCost[%index] = 1;
$Spell::startSound[%index] = LoopSP;
$Spell::endSound[%index] = AbsorbABS;
$Spell::classRestrictions[%index] = ",Cleric,Druid,Ranger,Paladin,Fighter,Thief,Bard,Mage,Summoner,";
$Spell::minLevel[%index] = 1;
$Spell::groupListCheck[%index] = False;

%index = 62;
$Spell::keyword[%index] = "doppelgang";
$Spell::index[$Spell::keyword[%index]] = %index;
$Spell::name[%index] = "Dopplegang";
$Spell::description[%index] = "Copy the form of an enemy.";
$Spell::delay[%index] = 4.0;
$Spell::recoveryTime[%index] = 60;
$Spell::LOSrange[%index] = 80;
$Spell::damageValue[%index] = 0;
$Spell::manaCost[%index] = 100;
$Spell::startSound[%index] = LoopSP;
$Spell::endSound[%index] = AbsorbABS;
$Spell::classRestrictions[%index] = ",Summoner,";
$Spell::minLevel[%index] = 100;
$Spell::groupListCheck[%index] = False;


//================= RECALL RUNES
%index = 63;
$Spell::keyword[%index] = "setrecall";
$Spell::index[$Spell::keyword[%index]] = %index;
$Spell::name[%index] = "Set Recall.";
$Spell::description[%index] = "Set recall position. (#cast setrecall [rune color])";
$Spell::delay[%index] = 4.0;
$Spell::recoveryTime[%index] = 6;
$Spell::LOSrange[%index] = 0;
$Spell::damageValue[%index] = 0;
$Spell::manaCost[%index] = 15;
$Spell::startSound[%index] = LoopSP;
$Spell::endSound[%index] = AbsorbABS;
$Spell::classRestrictions[%index] = ",Cleric,Druid,Ranger,Paladin,Fighter,Thief,Bard,Mage,Summoner,";
$Spell::minLevel[%index] = 20;
$Spell::groupListCheck[%index] = False;

%index = 64;
$Spell::keyword[%index] = "recall";
$Spell::index[$Spell::keyword[%index]] = %index;
$Spell::name[%index] = "Recall";
$Spell::description[%index] = "Recall to last set position. (#cast recall [rune color])";
$Spell::delay[%index] = 4.0;
$Spell::recoveryTime[%index] = 8;
$Spell::LOSrange[%index] = 0;
$Spell::damageValue[%index] = 0;
$Spell::manaCost[%index] = 15;
$Spell::startSound[%index] = LoopSP;
$Spell::endSound[%index] = AbsorbABS;
$Spell::classRestrictions[%index] = ",Cleric,Druid,Ranger,Paladin,Fighter,Thief,Bard,Mage,Summoner,";
$Spell::minLevel[%index] = 20;
$Spell::groupListCheck[%index] = False;


%index = 65;
$Spell::keyword[%index] = "Medic5";
$Spell::index["Medic5"] = %index;
$Spell::name[%index] = "Curative Magic";
$Spell::description[%index] = "Restores caster or LOS.";
$Spell::delay[%index] = 1.5;
$Spell::recoveryTime[%index] = 40;
$Spell::damageValue[%index] = -99999;
$Spell::LOSrange[%index] = 500;
$Spell::manaCost[%index] = 100;
$Spell::startSound[%index] = DeActivateWA;
$Spell::endSound[%index] = bigheal;
$Spell::classRestrictions[%index] = ",Cleric,";
$Spell::minLevel[%index] = 150;
$Spell::groupListCheck[%index] = False;

%index = 66;
$Spell::keyword[%index] = "BlockHigh";
$Spell::index[blockhigh] = %index;
$Spell::name[%index] = "Utility Magic";
$Spell::description[%index] = "Barrier Magic.";
$Spell::delay[%index] = 1;
$Spell::recoveryTime[%index] = 4;
$Spell::damageValue[%index] = 0;
$Spell::LOSrange[%index] = 200;
$Spell::manaCost[%index] = 20;
$Spell::startSound[%index] = spellstart;
$Spell::endSound[%index] = ActivateAR;
$Spell::classRestrictions[%index] = ",Mage,Paladin,Fighter,Cleric,Druid,Ranger,Thief,Bard,Summoner,";
$Spell::minLevel[%index] = 10;
$Spell::groupListCheck[%index] = False;

%index = 67;
$Spell::keyword[%index] = "Cage";
$Spell::index[Cage] = %index;
$Spell::name[%index] = "Utility Magic";
$Spell::description[%index] = "Trap Magic.";
$Spell::delay[%index] = 0.9;
$Spell::recoveryTime[%index] = 20;
$Spell::damageValue[%index] = 0;
$Spell::LOSrange[%index] = 10;
$Spell::manaCost[%index] = 20;
$Spell::startSound[%index] = spellstart;
$Spell::endSound[%index] = ActivateAR;
$Spell::classRestrictions[%index] = ",Mage,Paladin,Fighter,Cleric,Druid,Ranger,Thief,Bard,Summoner,";
$Spell::minLevel[%index] = 5;
$Spell::groupListCheck[%index] = False;

%index = 68;
$Spell::keyword[%index] = "fix";
$Spell::index[$Spell::keyword[%index]] = %index;
$Spell::name[%index] = "fix";
$Spell::description[%index] = "Basic spell that is used to fix dmg bug.";
$Spell::delay[%index] = 20.0;
$Spell::recoveryTime[%index] = 50;
$Spell::LOSrange[%index] = 200;
$Spell::damageValue[%index] = 0;
$Spell::manaCost[%index] = 1;
$Spell::startSound[%index] = LoopSP;
$Spell::endSound[%index] = AbsorbABS;
$Spell::classRestrictions[%index] = ",Cleric,Druid,Ranger,Paladin,Fighter,Thief,Bard,Mage,Summoner,";
$Spell::minLevel[%index] = 1;
$Spell::groupListCheck[%index] = False;

//====================================================================================================================
//====================================================================================================================
//====================================================================================================================
%index="";

function dot_op_genSpellSkill(%index) {

	%tmp = "";

	%INTSkill += (20 / $Spell::recoveryTime[%index]);
	%INTSkill += (80 / $Spell::delay[%index]);

	if($Spell::damageValue[%index] > 0)
		%WISSkill += floor(pow($Spell::damageValue[%index], 1.15));
	else
		%WISSkill += floor(-$Spell::damageValue[%index]*0.02);

	if(%INTSkill > %WISSkill)
		%INTSkill = %INTSkill-30;

	%INTSkill = Cap(floor(%INTSkill), -$MaxAPStats, $MaxAPStats);
	%WISSkill = Cap(floor(%WISSkill), -$MaxAPStats, $MaxAPStats);

	if(%INTSkill > 0)
		%tmp = %tmp@"INT "@%INTSkill@" ";
	if(%WISSkill > 0)
		%tmp = %tmp@"WIS "@%WISSkill@" ";

	if($Spell::minLevel[%index] != "")
		%tmp = %tmp@"LVL "@$Spell::minLevel[%index]@" ";
	if($Spell::classRestrictions[%index] != "")
		%tmp = %tmp@"CLASS "@$Spell::classRestrictions[%index]@" ";

	return %tmp;
}

//deleteVariables("Spell::classRestriction*");
//deleteVariables("Spell::minLevel*");

function IntSpellSkills() {

	$Spell::List = " ";
	for(%i = 1; %i <= 1000; %i++) {

		$Spell::List = $Spell::List@$Spell::keyword[%i]@" ";
		$Spell::ToUseSkill[%i] = %i.genSpellSkill();

		if($Spell::keyword[%i] == "" && %ii++ > 3)
			return;
	}
}

//----------------------------------------------------------------------------------------------------------------


function SpellNum1(%Client, %castObj, %castPos, %w2) {

	%zoneId = GetNearestZone(%Client, %w2, 3);

	if(%zoneId != False) {
		Client::sendMessage(%Client, $MsgBeige, "Teleporting near "@Zone::getDesc(%zoneId));

		%system = Object::getName(%zoneId);
		%type = GetWord(%system, 0);
		%desc = String::getSubStr(%system, String::len(%type)+1, 9999);

		//get the two markers
		%tmpgroup = nameToId("MissionGroup\\Zones\\"@%system);

		%m1pos = GameBase::getPosition(Group::getObject(%tmpgroup, 0));
		%m2pos = GameBase::getPosition(Group::getObject(%tmpgroup, 1));

		%x1 = GetWord(%m2pos, 0);
		%y1 = GetWord(%m2pos, 1);
		%x2 = GetWord(%m1pos, 0);
		%y2 = GetWord(%m1pos, 1);

		%newx = (getRandom() * (%x2-%x1)) + %x1;
		%newy = (getRandom() * (%y2-%y1)) + %y1;
		%newz = GetWord(%m1pos, 2);

		%newpos = %newx@" "@%newy@" "@%newz;

		GameBase::startFadeIn(%Client);
		GameBase::setPosition(%Client, %newpos);

		Player::setDamageFlash(%Client, 0.7);
		%extraDelay = 0.22;	//sometimes the endSound doesn't get played unless there is sufficient delay

		%castPos = SetOnGround(%Client, 500);
		return "returnFlag 1"; //return "returnFlag 1";
	}
	else {
		Client::sendMessage(%Client, $MsgBeige, "Teleportation failed.");
		return "returnFlag 0"; //return "returnFlag 0";
	}
}

function SpellNum2(%Client, %castObj, %castPos, %w2) {

	//Transport zone spell
	%zoneId = GetZoneByKeywords(%Client, %w2, 3);

	if(%zoneId != False) {
		Client::sendMessage(%Client, $MsgBeige, "Transporting to "@Zone::getDesc(%zoneId));

		%system = Object::getName(%zoneId);
		%type = GetWord(%system, 0);
		%desc = String::getSubStr(%system, String::len(%type)+1, 9999);

		%castPos = TeleportToMarker(%Client, "Zones\\"@%system@"\\DropPoints", False, True);

		GameBase::startFadeIn(%Client);

		Player::setDamageFlash(%Client, 0.7);
		%extraDelay = 0.22;	//sometimes the endSound doesn't get played unless there is sufficient delay

		return "returnFlag 1";
	}
	else {
		Client::sendMessage(%Client, $MsgBeige, "Transportation failed.");
		return "returnFlag 0";
	}
}
function SpellNum3(%Client, %castObj, %castPos, %w2) {
	//Advanced Transport zone spell
	%zoneId = GetZoneByKeywords(%Client, %w2, 3);

		if(%zoneId != False) {
			if(getObjectType(%castObj) == "Player")
				%id = Player::getClient(%castObj);
			else
				%id = %Client;

			Client::sendMessage(%Client, $MsgBeige, "Transporting to "@Zone::getDesc(%zoneId));
			if(%Client != %id)
				Client::sendMessage(%id, $MsgBeige, "You are being transported to "@Zone::getDesc(%zoneId));

			%system = Object::getName(%zoneId);
			%type = GetWord(%system, 0);
			%desc = String::getSubStr(%system, String::len(%type)+1, 9999);

			%castPos = TeleportToMarker(%id, "Zones\\"@%system@"\\DropPoints", False, True);

			GameBase::startFadeIn(%id);

		Player::setDamageFlash(%id, 0.7);
		%extraDelay = 0.22;	//sometimes the endSound doesn't get played unless there is sufficient delay

		return "returnFlag 1";
	}
	else {
		Client::sendMessage(%Client, $MsgBeige, "Transportation failed.");
		return "returnFlag 0";
	}
}
function SpellNum4(%Client, %castObj, %castPos) {
	//Medic
	Client::sendMessage(%Client, $MsgGreen, "Healing self");
	%HealId = %Client;
	%r = $Spell::damageValue[4] / $TribesDamageToNumericDamage;
	refreshHP(%Client, %r);

//	%castPos = GameBase::getPosition(%Client);

	return "returnFlag 1"; //return "returnFlag 1";
}
function SpellNum5(%Client, %castObj, %castPos) {
	//Medic 2
	if(getObjectType(%castObj) == "Player")
		%HealId = Player::getClient(%castObj);
	else
		%HealId = %Client;

	Client::sendMessage(%Client, $MsgGreen, "Healing "@Client::getName(%HealId));
	if(%Client != %Healid)
		Client::sendMessage(%HealId, $MsgGreen, "You are being healed by "@Client::getName(%Client));

	%r = $Spell::damageValue[5] / $TribesDamageToNumericDamage;

	refreshHP(%HealId, %r);

//	%castPos = GameBase::getPosition(%HealId);

	return "returnFlag 1 HealId "@%HealId; //return "returnFlag 1";
}
function SpellNum6(%Client, %castObj, %castPos) {
	//Medic 3
	if(getObjectType(%castObj) == "Player")
		%HealId = Player::getClient(%castObj);
	else
		%HealId = %Client;

	Client::sendMessage(%Client, $MsgGreen, "Healing "@Client::getName(%HealId));
	if(%Client != %Healid)
		Client::sendMessage(%HealId, $MsgGreen, "You are being healed by "@Client::getName(%Client));

	%r = $Spell::damageValue[6] / $TribesDamageToNumericDamage;

	refreshHP(%HealId, %r);

//	%castPos = GameBase::getPosition(%HealId);

	return "returnFlag 1 HealId "@%HealId;
}
function SpellNum7(%Client, %castObj, %castPos) {
	//Medic 4
	if(getObjectType(%castObj) == "Player")
		%HealId = Player::getClient(%castObj);
	else
		%HealId = %Client;

	Client::sendMessage(%Client, $MsgGreen, "Restoring "@Client::getName(%HealId));
	if(%Client != %Healid)
		Client::sendMessage(%HealId, $MsgGreen, "You are being healed cured by "@Client::getName(%Client));

	%r = $Spell::damageValue[7] / $TribesDamageToNumericDamage;

	refreshHP(%HealId, %r);

//	%castPos = GameBase::getPosition(%HealId);

	return "returnFlag 1 HealId "@%HealId;
}

function SpellNum8(%Client, %castObj, %castPos) {
	//Confusion
	%index = 8;
	%id = Player::getClient(%castObj);
	if(%Client.adminlevel>=2||Player::isAiControlled(%id)) {
		if(Client::getName(%id)!="") {
			%castlvl=(getFinalLVL(%Client)-floor(getRandom()*getFinalLVL(%id)));
			if(%castlvl<0) {
				%reflection=1;
				%castlvl*=-1;
				%temp_id=%id;
				%id=%Client;
				%Client=%temp_id;
				Client::sendMessage(%Client, $MsgBeige, "Confusing "@Client::getName(%id));
				if(%Client != %id)
					Client::sendMessage(%id, $MsgBeige, "REFLECT! You have been confused by "@Client::getName(%Client));
			}
			else {
				Client::sendMessage(%Client, $MsgBeige, "Confusing "@Client::getName(%id));
				if(%Client != %id)
					Client::sendMessage(%id, $MsgBeige, "You have been confused by "@Client::getName(%Client));
			}
			Player::setAnimation(%Client,40);

			%castPos = GameBase::getPosition(%id);
			%temp_loc=0;
			for(%mynum=1;%mynum<$MYmaxclients;%mynum++) {
				if(($ttlholdid[%mynum]==%id&&$ttltype[%mynum]==%index)||isdead(%id)) {
					%temp_loc=1;
					break;
				}
			}
			if(%temp_loc==0) {
				for(%mynum=1;%mynum<$MYmaxclients;%mynum++) {
					if($ttlholdid[%mynum]==0) {
						$ttlholdid[%mynum]=%id;
						$ttlholdpos[%mynum]=GameBase::getTeam(%id);
						$ttlhold[%mynum]=10*%castlvl;
						$ttltype[%mynum]=%index;

						if($ttlholdpos[%mynum]==0) {
							GameBase::setTeam($ttlholdid[%mynum],2);
						}
						else {
							GameBase::setTeam($ttlholdid[%mynum],0);
						}
						Game::refreshClientScore($ttlholdid[%mynum]);
						UpdateSkin($ttlholdid[%mynum]);
						break;
					}
				}
			}
			if($spellgoing==0) {
				$spellgoing=1;
				schedule("CastingSpell();", 1);
			}
		}
	}
	else {
		Client::sendMessage(%Client, $MsgBeige, "Cant confuse player "@Client::getName(%id));
	}
	return "returnFlag 1";
}
function SpellNum9(%Client, %castObj, %castPos) {
	//Remove Spell
	%id = Player::getClient(%castObj);
	if(%Client.adminlevel>=2||Player::isAiControlled(%id)) {
		if(Client::getName(%id)!="") {
			%castlvl=(getFinalLVL(%Client)-floor(getRandom()*getFinalLVL(%id)));
			if(%castlvl<0) {
				%reflection=1;
				%castlvl*=-1;
				%temp_id=%id;
				%id=%Client;
				%Client=%temp_id;
				Client::sendMessage(%Client, $MsgBeige, "Removing "@Client::getName(%id));
				if(%Client != %id)
					Client::sendMessage(%id, $MsgBeige, "REFLECT! You were removed by "@Client::getName(%Client));
				}
				else {
					Client::sendMessage(%Client, $MsgBeige, "Removing "@Client::getName(%id));
					if(%Client != %id)
						Client::sendMessage(%id, $MsgBeige, "You were removed by "@Client::getName(%Client));
				}
				%castPos = GameBase::getPosition(%id);
				Item::setVelocity(%id,GetWord(Item::getVelocity(%id),0)@" "@GetWord(Item::getVelocity(%id),1)@" "@GetWord(Item::getVelocity(%id),2)+(3*%castlvl));
			}
		}
		else {
			Client::sendMessage(%Client, $MsgBeige, "Cant jump player "@Client::getName(%id));
		}
		return "returnFlag 1";
	}
function SpellNum10(%Client, %castObj, %castPos) {

	//Mind Control
	%id = Player::getClient(%castObj);
	%index = 10;
	if(%Client.adminlevel>=2||Player::isAiControlled(%id)) {
		if(Client::getName(%id)!="") {
				%castlvl=(getFinalLVL(%Client)-floor(getRandom()*getFinalLVL(%id)));
				if(%castlvl<0) {
					%reflection=1;
					%castlvl*=-1;
					%temp_id=%id;
					%id=%Client;
					%Client=%temp_id;
					Client::sendMessage(%Client, $MsgBeige, "Manipulating "@Client::getName(%id));
					if(%Client != %id)
						Client::sendMessage(%id, $MsgBeige, "REFLECT! -- Your mind has been controlled by "@Client::getName(%Client));
				}
				else {
					Client::sendMessage(%Client, $MsgBeige, "Attempting to manupulate "@Client::getName(%id));
					if(%Client != %id)
						Client::sendMessage(%id, $MsgBeige, "You have been manipulated by "@Client::getName(%Client));
				}
				%castPos = GameBase::getPosition(%id);
				%temp_loc=0;
				for(%mynum=1;%mynum<$MYmaxclients;%mynum++) {
					if(($ttlholdid[%mynum]==%id&&$ttltype[%mynum]==%index)||isdead(%id)) {
						%temp_loc=1;
						break;
					}
				}
				if(%temp_loc==0) {
					for(%mynum=1;%mynum<$MYmaxclients;%mynum++) {
						if($ttlholdid[%mynum]==0) {
							$ttlholdid[%mynum]=%id;
							$ttlholdpos[%mynum]=%Client;
							$ttlhold[%mynum]=2*%castlvl;
							$ttltype[%mynum]=%index;
							Client::setControlObject($ttlholdpos[%mynum].possessId, $ttlholdpos[%mynum].possessId);
							Client::setControlObject($ttlholdpos[%mynum], $ttlholdpos[%mynum]);
							$dumbAIflag[$ttlholdpos[%mynum].possessId] = "";

								if(Player::isAiControlled($ttlholdid[%mynum])) {
									$dumbAIflag[$ttlholdpos[%mynum]] = True;
									AI::setVar($BotInfoAiName[$ttlholdpos[%mynum]], SpotDist, 0);
									AI::newDirectiveRemove($BotInfoAiName[$ttlholdpos[%mynum]], 99);
									AI::newDirectiveRemove($BotInfoAiName[$ttlholdpos[%mynum]], 127);
								}
								$ttlholdpos[%mynum].possessId = $ttlholdid[%mynum];
								Client::setControlObject($ttlholdid[%mynum], -1);
								Client::setControlObject($ttlholdpos[%mynum], $ttlholdid[%mynum]);
							break;
						}
					}
				}
				if($spellgoing==0) {
					$spellgoing=1;
					schedule("CastingSpell();", 1);
				}
			}
		}
		else {
			Client::sendMessage(%Client, $MsgBeige, "Cant control player "@Client::getName(%id));
		}

	return "returnFlag 1";
}
function SpellNum11(%Client, %castObj, %castPos) {

	//Fire 1 Spell
	schedule("cast_fireball("@%Client@");",0.1);
	return "returnFlag 1";
}
function SpellNum12(%Client, %castObj, %castPos) {

	//Fire 2 Spell
	if(%castPos != "") {
		%index = 12;
		CreateAndDetBomb(%Client, "Bomb3", %castPos, %index);

		%overrideEndSound = True;
		return "returnFlag 1";
	}
	else {
		Client::sendMessage(%Client, $MsgRed, "Cannot Cast that far with this spell.");
		return "returnFlag 0";
	}
}
function SpellNum13(%Client, %castObj, %castPos) {
	//fire 3 Spell
	schedule("cast_flame("@%Client@");",0.1);
	schedule("cast_flame("@%Client@");",0.2);
	schedule("cast_flame("@%Client@");",0.3);
	schedule("cast_flame("@%Client@");",0.4);
	schedule("cast_flame("@%Client@");",0.5);
	schedule("cast_flame("@%Client@");",0.6);
	schedule("cast_flame("@%Client@");",0.7);
	schedule("cast_flame("@%Client@");",0.8);
	schedule("cast_flame("@%Client@");",0.9);
	schedule("cast_flame("@%Client@");",1.0);
	schedule("cast_flame("@%Client@");",1.1);
	schedule("cast_flame("@%Client@");",1.2);
	schedule("cast_flame("@%Client@");",1.3);
	schedule("cast_flame("@%Client@");",1.4);

	return "returnFlag 1";
}
function SpellNum14(%Client, %castObj, %castPos) {
	//Fire 4 Spell - Flare
	schedule("cast_flare("@%Client@");",0.1);
	schedule("cast_flare("@%Client@");",0.2);
	schedule("cast_flare("@%Client@");",0.3);
	schedule("cast_flare("@%Client@");",0.4);
	schedule("cast_flare("@%Client@");",0.5);
	schedule("cast_flare("@%Client@");",0.6);
	schedule("cast_flare("@%Client@");",0.7);
	schedule("cast_flare("@%Client@");",0.8);
	schedule("cast_flare("@%Client@");",0.9);
	schedule("cast_flare("@%Client@");",1.0);


//	schedule("ClearSpellDmg("@%Client@");", 1.5);

	return "returnFlag 1";
}
function SpellNum15(%Client, %castObj, %castPos) {
	//Fire 4 Spell - Flare

	//echo("test");
	//client::sendmessage(%client,1,"Disabled till futher notice.");
	//return false;

	schedule("cast_flare("@%Client@");",0.1);
	schedule("cast_flare("@%Client@");",0.2);
	schedule("cast_flare("@%Client@");",0.3);
	schedule("cast_flare("@%Client@");",0.4);
	schedule("cast_flare("@%Client@");",0.5);
	schedule("cast_flare("@%Client@");",0.6);
	schedule("cast_flare("@%Client@");",0.7);
	schedule("cast_flare("@%Client@");",0.8);
	schedule("cast_flare("@%Client@");",0.9);
	schedule("cast_flare("@%Client@");",1.0);

	return "returnFlag 1";
}
function SpellNum16(%Client, %castObj, %castPos) {
	//Ice Spell
	schedule("cast_iceball("@%Client@");",0.1);
	return "returnFlag 1";
}
function SpellNum17(%Client, %castObj, %castPos) {
	//Fixed Ice2 Spell - using create and det bomb instead of spawn projectile
	if(%castPos != "") {
		%index = 17;
		CreateAndDetBomb(%Client, "Bomb201", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb201", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb202", %castPos, %index);

		return "returnFlag 1 overrideEndSound 1";
	}
	else {
		Client::sendMessage(%Client, $MsgRed, "You cannot cast that far with this spell.");
		return "returnFlag 0";
	}
}
function SpellNum18(%Client, %castObj, %castPos) {
	//Ice 3
	schedule("cast_bliz("@%Client@");",0.1);
	schedule("cast_bliz("@%Client@");",0.2);
	schedule("cast_bliz("@%Client@");",0.3);
	schedule("cast_bliz("@%Client@");",0.4);
	schedule("cast_bliz("@%Client@");",0.5);
	schedule("cast_bliz("@%Client@");",0.6);
	schedule("cast_bliz("@%Client@");",0.7);
	schedule("cast_bliz("@%Client@");",0.8);
	schedule("cast_bliz("@%Client@");",0.9);
	schedule("cast_bliz("@%Client@");",1.0);
	schedule("cast_bliz("@%Client@");",1.1);
	schedule("cast_bliz("@%Client@");",1.2);
	schedule("cast_bliz("@%Client@");",1.3);
	schedule("cast_bliz("@%Client@");",1.4);
	schedule("cast_bliz("@%Client@");",1.5);
	schedule("cast_bliz("@%Client@");",1.6);

	return "returnFlag 1";
}
function SpellNum19(%Client, %castObj, %castPos) {
	//Ice 4 Spell - Freeze
	if(%castPos != "") {
		%index = 19;
		CreateAndDetBomb(%Client, "Bomb42", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb43", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb44", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb42", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb43", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb44", %castPos, %index);

		%overrideEndSound = True;
		return "returnFlag 1";
	}
	else {
		Client::sendMessage(%Client, $MsgRed, "You cannot cast that far with this spell.");
		return "returnFlag 0";
	}
}
function SpellNum20(%Client, %castObj, %castPos) {
	//Aqua1 Spell
	schedule("cast_waterwater("@%Client@");",0.1);
	return "returnFlag 1";
}
function SpellNum21(%Client, %castObj, %castPos) {
	//Aqua2 Spell
	schedule("cast_waterwaterwater("@%Client@");",0.1);
	return "returnFlag 1";
}
function SpellNum22(%Client, %castObj, %castPos) {
	// Aqua3 Spell
	if(%castPos != "") {
		%index = 22;

		%minrad = 0;
		%maxrad = 5;
		for(%i = 0; %i <= 24; %i++) {
			%tempPos = RandomPositionXY(%minrad, %maxrad);
			%xPos = GetWord(%tempPos, 0) + GetWord(%castPos, 0);
			%yPos = GetWord(%tempPos, 1) + GetWord(%castPos, 1);
			%zPos = GetWord(%castPos, 2) + 72 - (%i * 3);

			%newPos = %xPos@" "@%yPos@" "@%zPos;

			schedule("CreateAndDetBomb(\""@%Client@"\", \"Bomb300\", \""@%newPos@"\", False, \""@%index@"\");", %i / 16, %player);
		}
		schedule("CreateAndDetBomb(\""@%Client@"\", \"Bomb301\", \""@%castPos@"\", True, \""@%index@"\");", %i / 16, %player);

		%overrideEndSound = True;
		return "returnFlag 1";
	}
	else {
		Client::sendMessage(%Client, $MsgBeige, "Could not find a target.");
		return "returnFlag 0";
	}
}
function SpellNum23(%Client, %castObj, %castPos) {
	//Aqua4 Spell
	schedule("cast_waverly("@%Client@");",0.1);
	schedule("cast_waverly("@%Client@");",0.2);
	schedule("cast_waverly("@%Client@");",0.3);

	return "returnFlag 1";
}
function SpellNum24(%Client, %castObj, %castPos) {

	//Quake 1 Spell
	if(%castPos != "") {
		%index = 24;
		CreateAndDetBomb(%Client, "Bomb107", %castPos, %index);

		%overrideEndSound = True;
		return "returnFlag 1";
	}
	else {
		Client::sendMessage(%Client, $MsgRed, "You cannot cast that far with this spell.");
		return "returnFlag 0";
	}
}
function SpellNum25(%Client, %castObj, %castPos) {

	//Quake 2 Spell
	if(%castPos != "") {
		%index = 25;
		CreateAndDetBomb(%Client, "Bomb107", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb107", %castPos, %index);

		%overrideEndSound = True;
		return "returnFlag 1";
	}
	else {
		Client::sendMessage(%Client, $MsgRed, "You cannot cast that far with this spell.");
		return "returnFlag 0";
	}
}
function SpellNum26(%Client, %castObj, %castPos) {

	//Quake 3 Spell
	if(%castPos != "") {
		%index = 26;
		CreateAndDetBomb(%Client, "Bomb107", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb107", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb107", %castPos, %index);

		%overrideEndSound = True;
		return "returnFlag 1";
	}
	else {
		Client::sendMessage(%Client, $MsgRed, "You cannot cast that far with this spell.");
		return "returnFlag 0";
	}
}
function SpellNum27(%Client, %castObj, %castPos) {

	//Quake 4 Spell - Break
	if(%castPos != "") {
		%index = 27;
		CreateAndDetBomb(%Client, "Bomb107", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb107", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb107", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb107", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb107", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb107", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb107", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb107", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb107", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb108", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb108", %castPos, %index);
		CreateAndDetBomb(%Client, "Bomb108", %castPos, %index);

		%overrideEndSound = True;
		return "returnFlag 1";
	}
	else {
		Client::sendMessage(%Client, $MsgRed, "You cannot cast that far with this spell.");
		return "returnFlag 0";
	}
}
function SpellNum28(%Client, %castObj, %castPos) {

	//Spike
	schedule("cast_missile("@%Client@");",0.1);
	return "returnFlag 1";
}
function SpellNum29(%Client, %castObj, %castPos) {

	//Wound
	if(%castPos != "") {
		%index = 29;
		CreateAndDetBomb(%Client, "bomb950", %castPos, %index);

		%overrideEndSound = True;
		return "returnFlag 1";
	}
	else {
		Client::sendMessage(%Client, $MsgGreen, "Cannot Cast that far with this spell.");
		return "returnFlag 0";
	}
}
function SpellNum30(%Client, %castObj, %castPos) {

		//Fist
		schedule("cast_tfist("@%Client@");",0.1);
		schedule("cast_tfist("@%Client@");",0.2);

		return "returnFlag 1";
	}
function SpellNum31(%Client, %castObj, %castPos) {

	//Missile
	schedule("cast_shocklvtwo("@%Client@");",0.1);
	schedule("cast_shocklvtwo("@%Client@");",0.2);

	return "returnFlag 1";
}
function SpellNum32(%Client, %castObj, %castPos) {

	//Cannon
	schedule("cast_shocklvone("@%Client@");",0.1);
	return "returnFlag 1";
}
function SpellNum33(%Client, %castObj, %castPos) {

	//Bomb
	schedule("cast_surge("@%Client@");",0.1);
	return "returnFlag 1";
}
function SpellNum34(%Client, %castObj, %castPos) {

	// Star
	%player = Client::getOwnedObject(%Client);
	if(%castPos != "") {
		%index = 34;
		%minrad = 0;
		%maxrad = 4;
		for(%i = 0; %i <= 10; %i++) {
			%tempPos = RandomPositionXY(%minrad, %maxrad);
			%xPos = GetWord(%tempPos, 0) + GetWord(%castPos, 0);
			%yPos = GetWord(%tempPos, 1) + GetWord(%castPos, 1);
			%zPos = GetWord(%castPos, 2) + (%i / 4);
			%newPos = %xPos@" "@%yPos@" "@%zPos;

			schedule("CreateAndDetBomb("@%Client@", \"Bomb108\", \""@%newPos@"\", False, "@%index@");", %i / 20, %player);
		}
		for(%i = 0; %i <= 10; %i++) {
			%tempPos = RandomPositionXY(%minrad, %maxrad);

			%xPos = GetWord(%tempPos, 0) + GetWord(%castPos, 0);
			%yPos = GetWord(%tempPos, 1) + GetWord(%castPos, 1);
			%zPos = GetWord(%castPos, 2) + (%i / 4);

			%newPos = %xPos@" "@%yPos@" "@%zPos;

			schedule("CreateAndDetBomb("@%Client@", \"Bomb8\", \""@%newPos@"\", False, "@%index@");", %i / 20, %player);
		}
		schedule("CreateAndDetBomb("@%Client@", \"Bomb200\", \""@%castPos@"\", False, "@%index@");", 1.0, %player);
		schedule("CreateAndDetBomb("@%Client@", \"Bomb200\", \""@%castPos@"\", False, "@%index@");", 1.05, %player);
		schedule("CreateAndDetBomb("@%Client@", \"Bomb107\", \""@%castPos@"\", True, "@%index@");", 1.1, %player);

		%overrideEndSound = True;
		return "returnFlag 1";
	}
	else {
		Client::sendMessage(%Client, $MsgBeige, "Could not find a target.");
		return "returnFlag 0";
	}
}
function SpellNum35(%Client, %castObj, %castPos) {

	//Dark Spike
	schedule("cast_gravity("@%Client@");",0.1);
	return "returnFlag 1";
}
function SpellNum36(%Client, %castObj, %castPos) {

	//Dark Shot
	schedule("cast_bioone("@%Client@");",0.1);
	return "returnFlag 1";
}
function SpellNum37(%Client, %castObj, %castPos) {

	//Surge
	schedule("cast_show("@%Client@");",0.1);
	schedule("cast_show("@%Client@");",0.2);
	schedule("cast_show("@%Client@");",0.3);
	schedule("cast_ice("@%Client@");",0.4);
	schedule("cast_ice("@%Client@");",0.5);
	schedule("cast_flame("@%Client@");",0.6);
	schedule("cast_flame("@%Client@");",0.7);
	schedule("cast_ice("@%Client@");",0.8);
	schedule("cast_ice("@%Client@");",0.9);
	schedule("cast_show("@%Client@");",1.0);
	schedule("cast_show("@%Client@");",1.1);
	schedule("cast_tfist("@%Client@");",1.2);
	schedule("cast_tfist("@%Client@");",1.3);
	schedule("cast_flare("@%Client@");",1.4);
	schedule("cast_show("@%Client@");",1.5);
	schedule("cast_show("@%Client@");",1.6);
	schedule("cast_ice("@%Client@");",1.7);
	schedule("cast_ice("@%Client@");",1.8);
	schedule("cast_ice("@%Client@");",1.9);
	schedule("cast_ice("@%Client@");",2.0);
	schedule("cast_gravity("@%Client@");",2.1);
	schedule("cast_gravity("@%Client@");",2.2);
	schedule("cast_shocklvone("@%Client@");",2.3);
	schedule("cast_shocklvtwo("@%Client@");",2.4);
	schedule("cast_shocklvthree("@%Client@");",2.5);
	schedule("cast_flare("@%Client@");",2.6);
	schedule("cast_show("@%Client@");",2.7);
	schedule("cast_show("@%Client@");",2.8);
	return "returnFlag 1";
}
function SpellNum38(%Client, %castObj, %castPos) {

	// Storm
	%player = Client::getOwnedObject(%Client);
	if(%castPos != "") {
		%index = 38;

		%minrad = 0;
		%maxrad = 5;
		for(%i = 0; %i <= 24; %i++) {
			%tempPos = RandomPositionXY(%minrad, %maxrad);
			%xPos = GetWord(%tempPos, 0) + GetWord(%castPos, 0);
			%yPos = GetWord(%tempPos, 1) + GetWord(%castPos, 1);
			%zPos = GetWord(%castPos, 2) + 72 - (%i * 3);
			%newPos = %xPos@" "@%yPos@" "@%zPos;

			schedule("CreateAndDetBomb("@%Client@", \"Bomb305\", \""@%newPos@"\", False, "@%index@");", %i / 16, %player);
		}
		schedule("CreateAndDetBomb("@%Client@", \"Bomb302\", \""@%castPos@"\", True, "@%index@");", %i / 16, %player);

		%overrideEndSound = True;
		return "returnFlag 1";
	}
	else {
		Client::sendMessage(%Client, $MsgBeige, "Cannot Cast that far with this spell.");
		return "returnFlag 0";
	}
}
function SpellNum39(%Client, %castObj, %castPos) {

	// Storm 2
	%player = Client::getOwnedObject(%Client);
	if(%castPos != "") {
		%index = 39;
		%minrad = 0;
		%maxrad = 5;
		for(%i = 0; %i <= 24; %i++) {
			%tempPos = RandomPositionXY(%minrad, %maxrad);
			%xPos = GetWord(%tempPos, 0) + GetWord(%castPos, 0);
			%yPos = GetWord(%tempPos, 1) + GetWord(%castPos, 1);
			%zPos = GetWord(%castPos, 2) + 72 - (%i * 3);

			%newPos = %xPos@" "@%yPos@" "@%zPos;

			schedule("CreateAndDetBomb("@%Client@", \"Bomb306\", \""@%newPos@"\", False, "@%index@");", %i / 16, %player);
		}
		schedule("CreateAndDetBomb("@%Client@", \"Bomb303\", \""@%castPos@"\", True, "@%index@");", %i / 16, %player);

		%overrideEndSound = True;
		return "returnFlag 1";
	}
	else {
		Client::sendMessage(%Client, $MsgBeige, "Cannot Cast that far with this spell.");
		return "returnFlag 0";
	}
}
function SpellNum40(%Client, %castObj, %castPos) {

	// Storm 3
	%player = Client::getOwnedObject(%Client);
	if(%castPos != "") {
		%index = 40;
		%minrad = 0;
		%maxrad = 5;
		for(%i = 0; %i <= 24; %i++) {
			%tempPos = RandomPositionXY(%minrad, %maxrad);

			%xPos = GetWord(%tempPos, 0) + GetWord(%castPos, 0);
			%yPos = GetWord(%tempPos, 1) + GetWord(%castPos, 1);
			%zPos = GetWord(%castPos, 2) + 72 - (%i * 3);

			%newPos = %xPos@" "@%yPos@" "@%zPos;

			schedule("CreateAndDetBomb("@%Client@", \"Bomb307\", \""@%newPos@"\", False, "@%index@");", %i / 16, %player);
		}
		schedule("CreateAndDetBomb("@%Client@", \"Bomb304\", \""@%castPos@"\", True, "@%index@");", %i / 16, %player);

		%overrideEndSound = True;
		return "returnFlag 1";
	}
	else {
		Client::sendMessage(%Client, $MsgBeige, "Cannot Cast that far with this spell.");
		return "returnFlag 0";
	}
}
function SpellNum41(%Client, %castObj, %castPos) {

	//Storm 4
	%player = Client::getOwnedObject(%Client);
	if(%castPos != "") {
		%index = 41;
		%minrad = 0;
		%maxrad = $Spell::radius[%index] / 2;
		for(%i = 0; %i <= 8; %i++) {
			%tempPos = RandomPositionXY(%minrad, %maxrad);
			%xPos = GetWord(%tempPos, 0) + GetWord(%castPos, 0);
			%yPos = GetWord(%tempPos, 1) + GetWord(%castPos, 1);
			%zPos = GetWord(%castPos, 2);

			%newPos = %xPos@" "@%yPos@" "@%zPos;

			schedule("CreateAndDetBomb("@%Client@", \"Bomb444\", \""@%newPos@"\", False, "@%index@");", %i / 7, %player);
		}
		CreateAndDetBomb(%Client, "Bomb444", %castPos, True, %index);

		%overrideEndSound = True;
		return "returnFlag 1";
	}
	else {
		Client::sendMessage(%Client, $MsgBeige, "Cannot cast that far with this spell.");
		return "returnFlag 0";
	}
}
function SpellNum42(%Client, %castObj, %castPos) {

	//Gale
	if(%castPos != "") {
		%index = 42;
		CreateAndDetBomb(%Client, "Bomb6661", %castPos, True, %index);

		%overrideEndSound = True;
		return "returnFlag 1";
	}
	else {
		Client::sendMessage(%Client, $MsgBeige, "Could not find a target.");
		return "returnFlag 0";
	}
}
function SpellNum43(%Client, %castObj, %castPos) {

	//Gale2
	if(%castPos != "") {
		%index = 43;
		CreateAndDetBomb(%Client, "Bomb6662", %castPos, True, %index);

		%overrideEndSound = True;
		return "returnFlag 1";
	}
	else {
		Client::sendMessage(%Client, $MsgBeige, "Could not find a target.");
		return "returnFlag 0";
	}
}
function SpellNum44(%Client, %castObj, %castPos) {

	//Gale3
	if(%castPos != "") {
		%index = 44;
		CreateAndDetBomb(%Client, "Bomb6663", %castPos, True, %index);

		return "returnFlag 1 overrideEndSound 1";
	}
	else {
		Client::sendMessage(%Client, $MsgBeige, "Could not find a target.");
		return "returnFlag 0";
	}
}
function SpellNum45(%Client, %castObj, %castPos) {

	//Gale 4
	if(%castPos != "") {
		%index = 45;
		CreateAndDetBomb(%Client, "Bomb6664", %castPos, True, %index);

		return "returnFlag 1 overrideEndSound 1";
	}
	else {
		Client::sendMessage(%Client, $MsgBeige, "Could not find a target.");
		return "returnFlag 0";
	}
}
function SpellNum46(%Client, %castObj, %castPos) {

	//ranger wind spell
	schedule("cast_rangerwind("@%Client@");",0.1);
	return "returnFlag 1";
}
function SpellNum47(%Client, %castObj, %castPos) {

	//ranger fire spell
	schedule("cast_rangerfire("@%Client@");",0.1);
	return "returnFlag 1";
}

function SpellNum48(%Client, %castObj, %castPos) {

	//ranger ice spell
	schedule("cast_rangerice("@%Client@");",0.1);
	return "returnFlag 1";
}

function SpellNum49(%Client, %castObj, %castPos) {

	//Block Front Utility
	%passed = checkArea(%Client, 1);
	if(!%passed) {
		Client::sendMessage(%Client, $MsgBeige, "You are to close to an object. (wall or tree, etc)");
	}
	else {
		%wall = newObject("MagicWall", "StaticShape", bluebluegreen,true);//,false);
		if(%wall != 0) {
			addToSet("MissionCleanup", %wall);
			%time = floor(getFinalLVL(%Client)*4.5);
			schedule("Item::Pop("@%wall@");", %time, %wall);
			%pos = GameBase::getPosition(%Client);
			%wall.hp = floor(getFinalLVL(%client)*0.8);
			%wall.owner = client::getname(%client);
			GameBase::setPosition(%wall, GetWord(%pos,0)@" "@GetWord(%pos,1)@" "@GetWord(%pos,2)+1.5);
			GameBase::setRotation(%wall, GetWord(GameBase::getRotation(%Client),0)+1.6@" "@GetWord(GameBase::getRotation(%Client),1)@" "@GetWord(GameBase::getRotation(%Client),2));
			Client::sendmessage(%client,0,"You create a magic wall with "@%time@" seconds of life and "@%wall.hp@" hp!");
		}

		return "returnFlag 1";
	}
}

function SpellNum50(%Client, %castObj, %castPos) {

	//Block Back Utility
	%passed = checkArea(%Client, 5);
	if(!%passed) {
		Client::sendMessage(%Client, $MsgBeige, "You are to close to an object. (wall or tree, etc)");
	}
	else {
		%wall = newObject("MagicWall", "StaticShape", hvshield2,true);//,false);
		if(%wall!=0) {
			addToSet("MissionCleanup", %wall);
			%time = floor(getFinalLVL(%Client)*7.2);
			schedule("Item::Pop("@%wall@");", %time, %wall);
			%pos = GameBase::getPosition(%Client);
			%wall.hp = floor(getFinalLVL(%client)*0.9);
			%wall.owner = client::getname(%client);
			GameBase::setPosition(%wall, GetWord(%pos,0)@" "@GetWord(%pos,1)@" "@GetWord(%pos,2)+1.5);
			GameBase::setRotation(%wall, GetWord(GameBase::getRotation(%Client),0)+1.6@" "@GetWord(GameBase::getRotation(%Client),1)@" "@GetWord(GameBase::getRotation(%Client),2));
			Client::sendmessage(%client,0,"You create a magic wall with "@%time@" seconds of life and "@%wall.hp@" hp!");
		}

		return "returnFlag 1";
	}
}

function SpellNum51(%Client, %castObj, %castPos) {

	//Light Utility
	Client::sendmessage(%client,1,"Disabled till futher notice.");
	return false;
	schedule("cast_truelight("@%Client@");",0.1);

	return "returnFlag 1";
}
function SpellNum52(%Client, %castObj, %castPos) {

	//Goods
	if(getObjectType(%castObj) == "Player")
		%id = Player::getClient(%castObj);
	else
		%id = %Client;

	Client::sendMessage(%Client, $MsgGreen, "Casting Goods for "@Client::getName(%id));
	if(%Client != %id)
		Client::sendMessage(%id, $MsgGreen, "Goods has been cast for you by "@Client::getName(%Client));

	refreshSTAMINA(%Client, -50);

	%castPos = GameBase::getPosition(%id);

	return "returnFlag 1";
}
function SpellNum53(%Client, %castObj, %castPos) {

	//Goods 2
	if(getObjectType(%castObj) == "Player")
		%id = Player::getClient(%castObj);
	else
		%id = %Client;

	Client::sendMessage(%Client, $MsgGreen, "Casting Goods for "@Client::getName(%id));
	if(%Client != %id)
		Client::sendMessage(%id, $MsgGreen, "Goods has been cast for you by "@Client::getName(%Client));

	refreshSTAMINA(%Client, -200);

	%castPos = GameBase::getPosition(%id);

	return "returnFlag 1";
}
function SpellNum54(%Client, %castObj, %castPos) {

	//Goods 3
	if(getObjectType(%castObj) == "Player")
		%id = Player::getClient(%castObj);
	else
		%id = %Client;

	Client::sendMessage(%Client, $MsgGreen, "Casting Goods for "@Client::getName(%id));
	if(%Client != %id)
		Client::sendMessage(%id, $MsgGreen, "Goods has been cast for you by "@Client::getName(%Client));

	refreshSTAMINA(%Client, -400);

	%castPos = GameBase::getPosition(%id);

	return "returnFlag 1";
}
function SpellNum55(%Client, %castObj, %castPos) {

	//Goods 4
	if(getObjectType(%castObj) == "Player")
		%id = Player::getClient(%castObj);
	else
		%id = %Client;

	Client::sendMessage(%Client, $MsgGreen, "Casting Goods for "@Client::getName(%id));
	if(%Client != %id)
		Client::sendMessage(%id, $MsgGreen, "Goods has been cast for you by "@Client::getName(%Client));

	refreshSTAMINA(%Client, -999);

	%castPos = GameBase::getPosition(%id);

	return "returnFlag 1";
}

function SpellNum56(%Client, %castObj, %castPos) {
//echo("DEATH SPELL -- CPos: -"@%castPos@"- "); looks ok..
	//death
	if(%castPos != "") {
		%index = 56;
		%minrad = 0;
		%maxrad = 30;
		for(%i = 0; %i <= 150; %i++) {
			%tempPos = RandomPositionXY(%minrad, %maxrad);

			%xPos = GetWord(%tempPos, 0) + GetWord(%castPos, 0) + 450 -(%i * 3);
			%yPos = GetWord(%tempPos, 1) + GetWord(%castPos, 1) + 450 -(%i * 3);
			%zPos = GetWord(%castPos, 2) + 450 - (%i * 3);

			%newPos = %xPos@" "@%yPos@" "@%zPos;

			schedule("CreateAndDetBomb(\""@%Client@"\", \"Bomb611\", \""@%newPos@"\", False, \""@%index@"\");", %i / 50, %player);
		}
		%minrad = 0;
		%maxrad = 50;
		for(%i = 0; %i <= 100; %i++) {
			%tempPos = RandomPositionXY(%minrad, %maxrad);

			%xPos = GetWord(%tempPos, 0) + GetWord(%castPos, 0);
			%yPos = GetWord(%tempPos, 1) + GetWord(%castPos, 1);
			%zPos = GetWord(%castPos, 2);

			%newPos = %xPos@" "@%yPos@" "@%zPos;

			schedule("CreateAndDetBomb(\""@%Client@"\", \"Bomb612\", \""@%newPos@"\", true, \""@%index@"\");", %i  / 20 + 4, %player);
		}
		%minrad = 50;
		%maxrad = 70;
		for(%i = 0; %i <= 60; %i++) {
			%tempPos = RandomPositionXY(%minrad, %maxrad);

			%xPos = GetWord(%tempPos, 0) + GetWord(%castPos, 0);
			%yPos = GetWord(%tempPos, 1) + GetWord(%castPos, 1);
			%zPos = GetWord(%castPos, 2) + 0.5;

			%newPos = %xPos@" "@%yPos@" "@%zPos;

			schedule("CreateAndDetBomb(\""@%Client@"\", \"Bomb612\", \""@%newPos@"\", true, \""@%index@"\");", %i  / 15 + 5, %player);
		}
		%minrad = 70;
		%maxrad = 90;
		for(%i = 0; %i <= 60; %i++) {
			%tempPos = RandomPositionXY(%minrad, %maxrad);

			%xPos = GetWord(%tempPos, 0) + GetWord(%castPos, 0);
			%yPos = GetWord(%tempPos, 1) + GetWord(%castPos, 1);
			%zPos = GetWord(%castPos, 2) + 1;

			%newPos = %xPos@" "@%yPos@" "@%zPos;

			schedule("CreateAndDetBomb(\""@%Client@"\", \"Bomb611\", \""@%newPos@"\", true, \""@%index@"\");", %i  / 15 + 6, %player);
		}
		%minrad = 90;
		%maxrad = 100;
		for(%i = 0; %i <= 30; %i++) {
			%tempPos = RandomPositionXY(%minrad, %maxrad);

			%xPos = GetWord(%tempPos, 0) + GetWord(%castPos, 0);
			%yPos = GetWord(%tempPos, 1) + GetWord(%castPos, 1);
			%zPos = GetWord(%castPos, 2) + 1.5;

			%newPos = %xPos@" "@%yPos@" "@%zPos;

			schedule("CreateAndDetBomb(\""@%Client@"\", \"Bomb611\", \""@%newPos@"\", true, \""@%index@"\");", %i  / 7 + 7, %player);
		}

		return "returnFlag 1 overrideEndSound 1";
	}
	else {
		Client::sendMessage(%Client, $MsgBeige, "Could not find a target.");
		return "returnFlag 0";
	}
}
function SpellNum57(%Client, %castObj, %castPos) {

	//ROCK SUMMON Spell
	schedule("cast_rocksummon("@%Client@");",0.1);
	return "returnFlag 1";
}
function SpellNum58(%Client, %castObj, %castPos) {

	//BLIZZARD SUMMON Spell
	schedule("cast_blizzardblizzardboltfake("@%Client@");",0.1);
	schedule("cast_blizzardblizzardboltfake("@%Client@");",0.2);
	schedule("cast_blizzardblizzardboltfake("@%Client@");",0.3);
	schedule("cast_blizzardblizzardboltfake("@%Client@");",0.4);
	schedule("cast_blizzardblizzardboltfake("@%Client@");",0.5);
	schedule("cast_blizzardblizzardboltfake("@%Client@");",0.6);
	schedule("cast_blizzardblizzardboltfake("@%Client@");",0.7);
	schedule("cast_blizzardblizzardboltreal("@%Client@");",0.8);
	schedule("cast_blizzardblizzardboltreal("@%Client@");",0.9);

	return "returnFlag 1";
}
function SpellNum59(%Client, %castObj, %castPos) {

	//BATTLE SUMMON Spell

	schedule("cast_summonswordone("@%Client@");",0.1);
	schedule("cast_summonswordtwo("@%Client@");",0.2);
	schedule("cast_summonswordthree("@%Client@");",0.3);
	schedule("cast_summonswordfour("@%Client@");",0.4);

	return "returnFlag 1";
}
function SpellNum60(%Client, %castObj, %castPos) {

	//SAPPERS SUMMON Spell

	if(%castPos != "") {
		%index = 60;
		CreateAndDetBomb(%Client, "bomb88888", %castPos, %index);

		%overrideEndSound = True;
		return "returnFlag 1";
	}
	else {
		Client::sendMessage(%Client, $MsgGreen, "Cannot Cast that far with this spell.");

		return "returnFlag 0";
	}
}
function SpellNum61(%Client, %castObj, %castPos) {
	%index = 61;
	%player = Client::getOwnedObject(%Client);
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

			%newPos = %xPos@" "@%yPos@" "@%zPos;

			schedule("CreateAndDetBomb("@%Client@", \"Supercheebomb1\", \""@%newPos@"\", False, "@%index@");", %i / 7, %player);
		}
		CreateAndDetBomb(%Client, "Supercheebomb1", %castPos, True, %index);

		return "returnFlag 1 overrideEndSound 1";
	}
	else
	{
		Client::sendMessage(%Client, $MsgBeige, "Could not find a target.");
		return "returnFlag 0";
	}
}
function SpellNum62(%Client, %castObj, %castPos) { //mimic spell

	%index = 62;
	if(Zone::getType($zone[%Client]) == "PROTECTED") {
		Client::sendMessage(%Client, $MsgRed, "You can't cast mimic in protected territory.");
		return "returnFlag 0 overrideEndSound 1";
	}
	else if($isZombie[%Client]) {
		Client::sendMessage(%Client, $MsgRed, "You can't cast mimic while a Zombie Beast.");
		return "returnFlag 0 overrideEndSound 1";
	}
	else {
		%id = Player::getClient(%castObj);
		if(getObjectType(%castObj) == "Player") {

			%troll = getFinalLVL(%id) + floor(getRandom() * (getFinalINT(%id) + (getFinalMDEF(%id) * (1/2)) ));
			%yroll = getFinalLVL(%Client) + floor(getRandom() * getFinalWIS(%Client));

			if(%yroll > %troll) {
			//	$RACE[%Client] = $RACE[%id];
				$ClientData[%Client, "isMimic"] = True;
				ChangeRace(%Client, $RACE[%id]);

				UpdateTeam(%Client);
				RefreshAll(%Client);

				playSound(AbsorbABS, GameBase::getPosition(%Client));

				return "returnFlag 1";
			}
			else {
				Client::sendMessage(%Client, $MsgBeige, "Mimic failed.");
				return "returnFlag 0 overrideEndSound 1";

			}
		}
		else {
			Client::sendMessage(%Client, $MsgBeige, "Could not find a target.");
			return "returnFlag 0 overrideEndSound 1";
		}
	}
}

function SpellNum63(%Client, %castObj, %castPos, %color) { // setrecall

	%rune = $ItemData["Rune_"@%color, FixCaps];
	if(%rune == "") {
		Client::sendMessage(%Client, 0, "Invalid use. #cast setrecall [color]");
		return;
	}
	if($ClientData[%Client, "Rune", "State", %color] <= 0) {
		if(Client::HasItem(%Client, %rune)) { // do we have this Rune color?

			%wis = getFinalWIS(%Client)*getRandom();

			$ClientData[%Client, "Rune", "POS", %color] = "";
			$ClientData[%Client, "Rune", "State", %color] = "";

			if(%wis <= 10) {
				Client::addItemCount(%Client, %rune, -1);
				Client::sendMessage(%Client, 0, "Failed to setrecall on Rune "@%color@". Rune "@%color@" broke!"); // haha.
			}
			else if((getRandom()*100) == 0) {
				Client::addItemCount(%Client, %rune, -1);
				Client::sendMessage(%Client, 0, "Failed to setrecall on Rune "@%color@". Rune "@%color@" broke!"); // haha.
			}
			else if(%wis >= 11 && %wis <= 99) {
				$ClientData[%Client, "Rune", "POS", %color] = GameBase::GetPosition(%Client);
				$ClientData[%Client, "Rune", "State", %color] = Cap(floor(getRandom()*5), 2, 4); //2-4 time use
				Client::sendMessage(%Client, 0, "Rune "@%color@" is really weakened. (Charges "@$ClientData[%Client, "Rune", "State", %color]@")");
			}
			else if(%wis >= 100 && %wis <= 150) {
				$ClientData[%Client, "Rune", "POS", %color] = GameBase::GetPosition(%Client);
				$ClientData[%Client, "Rune", "State", %color] = Cap(floor(getRandom()*10), 4, 8);
				Client::sendMessage(%Client, 0, "Rune "@%color@" is a little weakened. (Charges "@$ClientData[%Client, "Rune", "State", %color]@")");
			}
			else if(%wis > 150 && %wis <= 250) {
				$ClientData[%Client, "Rune", "POS", %color] = GameBase::GetPosition(%Client);
				$ClientData[%Client, "Rune", "State", %color] = Cap(floor(getRandom()*15), 8, 12);
				Client::sendMessage(%Client, 0, "Rune "@%color@" is ok. (Charges "@$ClientData[%Client, "Rune", "State", %color]@")");
			}
			else if(%wis > 250 && %wis <= 350) {
				$ClientData[%Client, "Rune", "POS", %color] = GameBase::GetPosition(%Client);
				$ClientData[%Client, "Rune", "State", %color] = Cap(floor(getRandom()*22), 12, 16);
				Client::sendMessage(%Client, 0, "Rune "@%color@" is good. (Charges "@$ClientData[%Client, "Rune", "State", %color]@")");
			}
			else if(%wis > 350 && %wis <= 600) {
				$ClientData[%Client, "Rune", "POS", %color] = GameBase::GetPosition(%Client);
				$ClientData[%Client, "Rune", "State", %color] = Cap(floor(getRandom()*30), 16, 24);
				Client::sendMessage(%Client, 0, "Rune "@%color@" is fine. (Charges "@$ClientData[%Client, "Rune", "State", %color]@")");
			}
			else if(%wis > 600 && %wis <= 900) {
				$ClientData[%Client, "Rune", "POS", %color] = GameBase::GetPosition(%Client);
				$ClientData[%Client, "Rune", "State", %color] = Cap(floor(getRandom()*40), 24, 30);
				Client::sendMessage(%Client, 0, "Rune "@%color@" is strong. (Charges "@$ClientData[%Client, "Rune", "State", %color]@")");
			}
			else if(%wis > 900 && %wis <= 1500) {
				$ClientData[%Client, "Rune", "POS", %color] = GameBase::GetPosition(%Client);
				$ClientData[%Client, "Rune", "State", %color] = Cap(floor(getRandom()*50), 30, 36);
				Client::sendMessage(%Client, 0, "Rune "@%color@" is very strong. (Charges "@$ClientData[%Client, "Rune", "State", %color]@")");
			}
			else if(%wis > 1500 && %wis < 2000) {
				$ClientData[%Client, "Rune", "POS", %color] = GameBase::GetPosition(%Client);
				$ClientData[%Client, "Rune", "State", %color] = Cap(floor(getRandom()*65), 36, 50);
				Client::sendMessage(%Client, 0, "Rune "@%color@" is excellent. (Charges "@$ClientData[%Client, "Rune", "State", %color]@")");
			}
			else if(%wis >= 2000) {
				$ClientData[%Client, "Rune", "POS", %color] = GameBase::GetPosition(%Client);
				$ClientData[%Client, "Rune", "State", %color] = Cap(floor(getRandom()*100), 50, 100);
				Client::sendMessage(%Client, 0, "Rune "@%color@" is superior. (Charges "@$ClientData[%Client, "Rune", "State", %color]@")");
			}
		}
		else
			Client::sendMessage(%Client, 0, "SetRecall failed. You do not have the rune color "@%color@".");
	}
	else
		Client::sendMessage(%Client, 0, "Rune "@%color@" is already charged. (Charges "@$ClientData[%Client, "Rune", "State", %color]@")");
}

function SpellNum64(%Client, %castObj, %castPos, %color) { // recall

	%rune = $ItemData["Rune_"@%color, FixCaps];
	if(%rune == "") {
		Client::sendMessage(%Client, 0, "Invalid use. #cast recall [color]");
		return;
	}
	if(Client::HasItem(%Client, %rune)) { // do we have this Rune color?
		if($ClientData[%Client, "Rune", "State", %color] > 0) { // did we charge it?

			$ClientData[%Client, "Rune", "State", %color]--;
			GameBase::SetPosition(%Client, $ClientData[%Client, "Rune", "POS", %color]);

			Client::sendMessage(%Client, 0, "Recalled! Nearest zone is "@_GetNearestZone(%Client)@".");

			if($ClientData[%Client, "Rune", "State", %color] <= 0) {
				Client::sendMessage(%Client, 0, "Rune "@%color@" broke.");
				Client::addItemCount(%Client, %rune, -1);
			}
			else if($ClientData[%Client, "Rune", "State", %color] == 1) {
				Client::sendMessage(%Client, 0, "Rune "@%color@" has one charge left.");
			}

		}
		else
			Client::sendMessage(%Client, 0, "Rune "@%color@" not charge!  #cast setrecall "@%color@" ");
	}
	else
		Client::sendMessage(%Client, 0, "SetRecall failed. You do not have the rune color "@%color@".");
}

function SpellNum65(%Client, %castObj, %castPos) {
	//Medic 5
	if(getObjectType(%castObj) == "Player")
		%HealId = Player::getClient(%castObj);
	else
		%HealId = %Client;

	Client::sendMessage(%Client, $MsgGreen, "Restoring "@Client::getName(%HealId));
	if(%Client != %Healid)
		Client::sendMessage(%HealId, $MsgGreen, "You are being fully cured by "@Client::getName(%Client));

	%r = $Spell::damageValue[7] / $TribesDamageToNumericDamage;

	refreshHP(%HealId, %r);

//	%castPos = GameBase::getPosition(%HealId);

	return "returnFlag 1 HealId "@%HealId;
}

function SpellNum66(%Client, %castObj, %castPos) {
	%passed = checkArea(%Client, 5);
	if(!%passed) {
		Client::sendMessage(%Client, $MsgBeige, "You are to close to an object. (wall or tree, etc)");
	}
	else {
		%wall = newObject("MagicWall", "StaticShape", hvshield2,true);//,false);
		if(%wall!=0) {
			addToSet("MissionCleanup", %wall);
			%time = floor(getFinalLVL(%Client)*7.2);
			schedule("Item::Pop("@%wall@");", %time, %wall);
			%pos = GameBase::getPosition(%Client);
			%wall.hp = floor(getFinalLVL(%client)*0.9);
			%wall.owner = client::getname(%client);
			GameBase::setPosition(%wall, GetWord(%pos,0)@" "@GetWord(%pos,1)@" "@GetWord(%pos,2)+3.5);
			//GameBase::setRotation(%wall, GetWord(GameBase::getRotation(%Client),0)+1.6@" "@GetWord(GameBase::getRotation(%Client),1)@" "@(GetWord(GameBase::getRotation(%Client),2)+1.6));
			GameBase::setRotation(%wall, "0 0 1.6");
			Client::sendmessage(%client,0,"You create a magic wall with "@%time@" seconds of life and "@%wall.hp@" hp!");
	}
	return "returnFlag 1";
	}
}

function SpellNum67(%Client, %castObj, %castPos) {
	if(getObjectType(%castObj) == "Player")
		%id = Player::getClient(%castObj);
	else
		%id = %Client;

	%time = $AP[%Client, 1] - ($AP[%id, 1] /2);
	if(%time > 120)	%time = 120;

	if(%time <= 0)
	{
		%id = %client;
		remoteEval(%client,"ATKText", "<JC>REFLECT!", true);
		%time = 60;
	}

	cage(%id,%time,1);
	remoteEval(%client,"ATKText", "<JC>You caged "@client::getname(%id)@" for "@%time@" seconds.", true);
	if(%client != %id)
		remoteEval(%id,"ATKText", "<JC>You were caged by "@client::getname(%client)@" for "@%time@" seconds.", false);

	return "returnFlag 1";
}

function SpellNum68(%Client, %castObj, %castPos) {
	%index = 68;
	%player = Client::getOwnedObject(%Client);
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

			%newPos = %xPos@" "@%yPos@" "@%zPos;

			schedule("CreateAndDetBomb("@%Client@", \"Supercheebomb1\", \""@%newPos@"\", False, "@%index@");", %i / 7, %player);
		}
		CreateAndDetBomb(%Client, "Supercheebomb1", %castPos, True, %index);

		return "returnFlag 1 overrideEndSound 1";
	}
	else
	{
		Client::sendMessage(%Client, $MsgBeige, "Could not find a target.");
		return "returnFlag 0";
	}
}