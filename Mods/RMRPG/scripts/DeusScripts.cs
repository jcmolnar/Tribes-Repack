//Deus_ex_Machina
//
//

//For Weapons	plasma.ShootThis(%player);
function dot_op_ShootThis(%proj, %player) {

	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);

	Projectile::spawnProjectile(%proj,%trans,%player,%vel);
}

//For Spells
function dot_op_ShootSpell(%proj, %Client) {
	%player = Client::getOwnedObject(%Client);
	Player::unmountItem(%player, $WeaponSlot);
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client, 39);
	Projectile::spawnProjectile(%proj, %trans, %player, %vel);
}

//For Bots
function dot_op_Weapon_SpellAttack(%proj, %AiId, %player, %los, %bool) {

	%cl = Player::getClient(%los);

	if($invisible[%cl]) {
		if(!OddsAre(4))
			return;
	}

	if(%cl != -1)
		$BotShootingAt_WithId[%cl] = %AiId;
	else {
		//%cl = GetClosestClient(%AiId, 50, 100, 10);
		if(GameBase::getFOVinfo(%AiId, 500)) {
			%cl = Player::getClient($fov::object);
		}
		if(%cl != -1)
			$BotShootingAt_WithId[%cl] = %AiId;
		else
			$BotShootingAt_WithId[%AiId] = %AiId;
	}

	if(%proj == "cuttinglaser") {
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = rotVector( "15 0 -15", GameBase::getRotation(%player));
		Projectile::spawnProjectile("lasercutter",%trans,%player,%vel);
		%vel = rotVector( "15 0 -15", GameBase::getRotation(%player));
		Projectile::spawnProjectile("esrockets2",%trans,%player,%vel);
	}

	else if(%bool) {
		%trans = GameBase::getMuzzleTransform(%player);
		//%vel = Item::getVelocity(%player);
		%vel = "0 0 0";

		%_trans = getWord(%trans, 0)@" "@getWord(%trans, 1)@" "@getWord(%trans, 2)@" "@getWord(%trans, 3)@" "@
				getWord(%trans, 4)@" "@getWord(%trans, 5)@" "@getWord(%trans, 6)@" "@getWord(%trans, 7)@" "@
				getWord(%trans, 8);

		if(%cl != -1) {
			%pos = GameBase::getPosition(%player);

			if(Vector::getDistance(%pos, GameBase::getPosition(%cl)) <= 3)
				%pos = GameBase::getPosition(%cl);

		}
		else
			%pos = GameBase::getPosition(%player);

		%pos = Vector::Add(%pos, "0 0 1.5");

		//%pos = getWord(%pos, 0)@" "@getWord(%pos, 1)@" "@getWord(%pos, 2) + 2;

//		%vel = rotVector( "-10 0 -1.5", GameBase::getRotation(%player));

	//	%vel = rotVector( $t_x @" "@$_y@" "@$_z, GameBase::getRotation(%player));
		Projectile::spawnProjectile(%proj, %_trans@" "@%pos, %player, %vel, GameBase::getPosition(%cl));
	}

	else {
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);
		Projectile::spawnProjectile(%proj, %trans, %player, %vel, %los);

	}
}

function GameBase::getFOVinfo(%Client, %range) { //used mainly for bots

	//find all clients that could be in the %Client's FOV and not on the same team.
	%Client = Player::getClient(%Client);
	%player = Client::getOwnedObject(%Client);

	if(%player == -1)
		return False;

	%team = GameBase::getTeam(%Client);
	%pos = GameBase::getPosition(%Client);

	%rot = GetWord(GameBase::getRotation(%Client), 2);

	if(Player::isAiControlled(%Client))
		%list = GetPlayerIdList();
	else
		%list = GetBotIdList();

	for(%i = 0; (%id = GetWord(%list, %i)) != -1; %i++) {

		if(GameBase::getTeam(%id) != %team && !$invisible[%id]) {

			%dist = Vector::getDistance(%pos, GameBase::getPosition(%id));
			if(%dist <= %range) {

				//GameBase::getLOSinfo(%player, %range, %rot);
				%vec = Vector::sub(GameBase::getPosition(%id), %pos);
				%vecRot = GetWord(Vector::getRotation(%vec), 2);

				if(%vecRot >= %rot - 0.5 && %vecRot <= %rot + 0.5) {
					%idList[%c++] = %id;
				}
			}
		}
	}

//	for(%i = 1; %idList[%i] != ""; %i++) {
//		MessageAll(1, "FOV ID["@%i@"]: "@%idList[%i]);
//	}

	if(%c > 0) {
		if(%c > 1)
			%cl = %idList[floor(%c/2)];
		else
			%cl = %idList[%c-1];

		$fov::position = GameBase::getPosition(%cl);
		$fov::object = Client::getOwnedObject(%cl);
		$fov::normal = Vector::normalize($fov::position); //is this how $los::normal does it?

		return True;
	}
	else
		return False;
}

function blah(%Client) {
	if(%Client=="") return;
	$_player = Client::getOwnedObject(%Client);
	$_trans = GameBase::getMuzzleTransform($_player);
	$_vel = Item::getVelocity($_player);
	Projectile::spawnProjectile(dodgethisbolt, $_trans, $_player, $_vel);
}


function checkArea(%Client, %area) {

	%pos = GameBase::getPosition(%Client);
	%passed = true;

	%pos = getWord(%pos, 0)@" "@getWord(%pos, 1)@" "@getWord(%pos, 2); //+4;

	%set = newObject("set", SimSet); //$SimPlayerObjectType
	%n = containerBoxFillSet(%set, $StaticObjectType | $ItemObjectType | $SimInteriorObjectType, %pos, %area, %area, 0, 0);

	if(Group::objectCount(%set) > 0)
		%passed = false;

	for(%i = 0; %i < Group::objectCount(%set); %i++) {
		%id = Group::getObject(%set, %i);	echo("Box: Id: "@%id@" Type:"@getObjectType(%id)@"  Total:"@Group::objectCount(%set));
	}

	deleteObject(%set);

	if(!%passed)
		return false;
	else
		return true;

}

function nexec(%file) {
	focusServer();
	focusClient();
	exec(%file);
}

function GetClosestClient(%Client, %X, %Y, %Z) { //GameBase::getFOVInfo is better

	%pos = GameBase::getPosition(%Client);
	%closest = 50000;
	%closestId = -1;
	if(%X != "" && %Y == "")
		%bx = %by = %bz = %X;
	else if(%X != "" && %Y != "" && %Z != "") {
		%bx = %X; %by = %Y; %bz = %Z;
	}
	else
		%bx = %by = %bz = 4;

	%set = newObject("set", SimSet);
	%n = containerBoxFillSet(%set, $SimPlayerObjectType, %pos, %bx, %by, %bz);

	for(%i = 0; %i < Group::objectCount(%set); %i++) {
		%id = Player::getClient(Group::getObject(%set, %i));	//echo("Box: Id: "@%id);
		if(!$invisible[%id] && %id != %Client) {
			%dist = Vector::getDistance(%pos, GameBase::getPosition(%id));	//echo("Box: DIST: "@%dist);
			if(%dist < %closest) {
				%closest = %dist;
				%closestId = %id;
			}
		}
	}
	deleteObject(%set);

	return %closestId;
}

function AI::ShootRandomSpell(%AiId, %player, %los, %list) {
	if($SpellCastStep[%clientId] == 1 || $SpellCastStep[%clientId] == 2)
		return "not ready";
	%cl = Player::getClient(%los);
	if($invisible[%cl]) {
		if(!OddsAre(4))
			return "invisible";
	}
	%spell = getRandomItemFromList(%list);
	BeginCastSpell(%AiId, %spell, 0);
	return true;
}

function getRandomItemFromList(%list) {
	%list.getRandomItemFromList();
}

//"a b c d e f g".getRandomItemFromList();
function dot_op_getRandomItemFromList(%list) {
	for(%i = 0; (%w[%i] = getWord(%list, %i)) != -1; %i++){}
	%r = (floor(getRandom() * %i));
	return %w[%r];
}

function DetBomb(%Client, %b, %Pos, %sound) {

	%player = Client::getOwnedObject(%Client);
	if(%Pos == "")
		%Pos = GameBase::getPosition(%Client);
	if(%b == "")
		%b = bomb1;
	%bomb = newObject("", "Mine", %b);

	addToSet("MissionCleanup", %bomb);
	GameBase::Throw(%bomb, %player, 0, false);
	GameBase::setPosition(%bomb, %Pos);
	if(%sound == "")
		%sound = NoSound;
	playSound(%sound, %Pos);
}

function Client::Bumped(%this, %object) {

	%sClient = Player::getClient(%object); //bumped
	%dClient = Player::getClient(%this); //being bumped...

	%time = getIntegerTime(true) >> 5;
	if(%time - %sClient.lastBumpTime <= 0.2)
		  return;

	%sClient.lastBumpTime = %time;

	if($ClientData[%sClient, lastBump] == %dClient) {

		%sSTR = getFinalSTR(%sClient);
		%dSTR = getFinalSTR(%dClient);

		if((%sClient.adminLevel > %dClient.adminLevel) || (%dClient.adminLevel < 1 && %sSTR > %dSTR) ) {
			%b = GameBase::getRotation(%sClient);
			%c1 = Cap(20 + (%sSTR-%dSTR), 0, 100);
			%c2 = Cap(%c1 / 4, 0, 90);
			%mom = Vector::getFromRot( %b, %c1, %c2 );

			Player::applyImpulse(%dClient, %mom);
			$ClientData[%sClient, lastBump] = "";
		}
	}
	else
		$ClientData[%sClient, lastBump] = %dClient;
}

//function FixM(%num) {
//
//	for(%i = 1; %i < 10000; %i++) {
//		%tn = %i * 1000000;
//		if(%num >= %tn) {
//			%n = %i * 1000000;
//			continue;
//		}
//		else
//			break;
//	}
//	if(%i > 1)
//		%fix = %i-1@"m "@%num - %n;
//	else
//		%fix = %num;
//
//	return %fix;
//}

function FixM(%num, %opt) {	//Re-wrote FixM(); because the first one sucks and it's slow...
								//And this one will return billions. (You can gain will over one Billion exp in this game...)
	if(%num == "")
		echo("FixM(Num, NoTag);");

	%m = floor(%num / 1000000);

	if(%m >= 1) { //Has more then a million
		%b = floor(%num / 1000000000);

		if(%b >= 1) { //Has more then a billion
			%m = %m - (1000 * %b);
			%fix = %num - (%b * 1000000000) - (%m * 1000000);
			if(%opt == "")
				%fix = %b@"b "@%m@"m "@%fix;
			else
				%fix = %b@" "@%m@" "@%fix;

			return %fix;
		}
		else {
			%fix = %num - (%m * 1000000);
			if(%opt == "")
				%fix = %m@"m "@%fix;
			else
				%fix = "0 "@%m@" "@%fix;

			return %fix;
		}
	}
	else {
		if(%opt == "")
			return %num; //No Million =p
		else
			return "0 0 "@%num;
	}
}

function getFixedExp(%Client) {
	return $Exp1[%Client]@" "@$Exp2[%Client]@" "@$Exp3[%Client];
}

function ShowExp(%Client) {

	if($Exp1[%Client] > 0)
		%exp1 = $Exp1[%Client]@"b ";
	else
		%exp1 = "";
	if($Exp2[%Client] > 0)
		%exp2 = $Exp2[%Client]@"m ";
	else {
		if(%exp1 > 0)
			%exp2 = "0m ";
	}
	if($Exp3[%Client] > 0)
		%exp3 = $Exp3[%Client];
	else
		%exp3 = "0";

	return %exp1 @ %exp2 @ %exp3;
}

function GiveExp(%Client, %exp, %opt) {

	if(%opt != Force) //Force is only use when GiveThisStuff(); gives you LVL(s)
		%exp = FixM(%exp, True);//True means it won't return tags (Ex: if your exp was "1b 20m 1380", it'll return "1 20 1380")
//echo("GiveExp "@%exp);
	%w1 = GetWord(%exp, 0);
	%w2 = GetWord(%exp, 1);
	%w3 = GetWord(%exp, 2);

	if(%opt != Set) {
		$Exp1[%Client] += %w1;
		$Exp2[%Client] += %w2;
		$Exp3[%Client] += %w3;
	}
	else if(%opt == Set) {
		$Exp1[%Client] = %w1;
		$Exp2[%Client] = %w2;
		$Exp3[%Client] = %w3;
	}
	if($Exp3[%Client] >= 1000000) {
		$Exp3[%Client] -= 1000000;
		$Exp2[%Client]++;
		GiveExp(%Client, 0);//Call this again (may have gotten +2 million exp...)
	}
	if($Exp3[%Client] < 0) {
		$Exp3[%Client] += 1000000;
		$Exp2[%Client]--;
		GiveExp(%Client, 0);//Call this again (may have gotten -2 million exp...)
	}
	if($Exp2[%Client] >= 1000) {
		$Exp2[%Client] -= 1000;
		$Exp1[%Client]++;
		GiveExp(%Client, 0);
	}
	if($Exp2[%Client] < 0) {
		$Exp2[%Client] += 1000;
		$Exp1[%Client]--;
		GiveExp(%Client, 0);
	}
	if($Exp1[%Client] < 0 && $Exp2[%Client] < 0 && $Exp3[%Client] < 0) {
		$Exp1[%Client] = 0;
		$Exp2[%Client] = 0;
		$Exp3[%Client] = 0;
	}
	return true;
}

function Between(%l, %h) {

	if(%l == "" || %h == "" || %l > %h) {
		echo("Between(LowNum, HighNum);");
		return;
	}
	%perc = floor(getRandom() * 100);

	%lh = %h - %l;

	%num = Cap(round((%lh * (%perc/100)) + %l), %l, %h);

	return %num;
}

function NumPercent(%num, %perc) {
	if(%num == "" || %perc == "") {
		echo("NumPercent(Num, Percent);");
		return;
	}

	%percent = floor(getRandom() * (100-%perc))+%perc+1;
	if(%percent > 100) %percent = 100;

	%num = round(%num * (%percent/100));
	if(%num < 0) %num = 0;

	return %num;
}

function ScreenRefresh() {
	%list = GetPlayerIdList();
	for(%i = 0; GetWord(%list, %i) != -1; %i++) {
		%id = GetWord(%list, %i);
		remoteEval(%id, "FlushTexture");
	}
}

function CheckClientFiles(%Client) {

	$ClientData[%Client, CCF] = 1;

	remoteEval(%Client, "rmCheck", "$__RM__CLIENT_CS");
	remoteEval(%Client, "rmCheck", "$__RM__DEUSCLIENT_CS");
	remoteEval(%Client, "rmCheck", "$__RM__GUI_CS");

	Schedule("CheckClientFilesDone("@%Client@");", 12);
}

function remotermReport(%Client, %mask) {
//echo("rmReport("@%mask@") ");
	if($ClientData[%Client, CCF] >= 1) {
		if(%mask == 0x0cf || %mask == 0x1cf || %mask == 0x2cf) {
			$ClientData[%Client, CCF]++;
		}
	}
}

function CheckClientFilesDone(%Client) {
	if($ClientData[%Client, CCF] != 4) { // something is wrong!
		Client::sendMessage(%Client, 1, "SERVER: Error! Not all of your needed files were report loaded.");
		Client::sendMessage(%Client, 1, "SERVER: Make sure you have all the needed client files.");
	}
	$ClientData[%Client, CCF] = "";
}


function FixSpaces(%string) {

	//for(%i = 0; %i < String::len(%string)+1; %i++) {
	//	if(String::GetSubStr(%string, %i, 1) == " ")
	//		%NEWstring = %NEWstring@"_";
	//	else
	//		%NEWstring = %NEWstring @ String::GetSubStr(%string, %i, 1);
	//}
	//return %NEWstring;


	//w00t
	return String::convertSpaces(%string);
}

function Fix(%fix, %Client, %opt) {
	if(Player::isAiControlled(%Client))
		return;
	if(%opt == HP)
		%a = %fix / $MaxHP[%Client];
	else if(%opt == MP)
		%a = %fix / getMaxMANA(%Client);
	else if(%opt == STA)
		%a = %fix / getSTAMINA(%Client);
	else if(%opt == EXP)  {
		%lvl = $templvl[%Client];
		%have = $EXPtable[%lvl];
		%need = $EXPtable[%lvl+1];
		%wt1 = floor(GetWord(%need, 0) - GetWord(%have, 0));
		%wt2 = floor(GetWord(%need, 1) - GetWord(%have, 1));
		%wt3 = floor(GetWord(%need, 2) - GetWord(%have, 2));
		if(%wt2 < 0) {
			%wt1--;
			%wt2 += 1000;
		}
		if(%wt3 < 0) {
			%wt2--;
			%wt3 += 1000000;
		}//echo(%wt1@" "@%wt2@" "@%wt3);
		%a = FixNum(%wt1@" "@%wt2@" "@%wt3);	//echo("a "@%a);
		%fix = ""@FixNum(getTNL(%Client))@"";		//echo("TNL "@%fix);
		%b = %fix / %a;		//echo("b "@%b);
		%a = -%b + 1;		//echo("EXPBAR: "@%a);
	}
	else
		echo("Error: \'Fix("@%fix@", "@%opt@");\' Unknown %opt: "@%opt@".");

	return %a;
}

// echo(FixNum("1 234 567890"));
function FixNum(%num) {

	%wt1 = GetWord(%num, 0);	//Billions doesn't need to be fixed
	%wt2 = GetWord(%num, 1);	//Millions should have len of 3
	%wt3 = GetWord(%num, 2);	//Rest should have len of 6

	%ml = String::len(%wt2);
	for(%i = %ml; %i <= 2; %i++)
		%wt2 = "0"@%wt2@"";
	%rl = String::len(%wt3);
	for(%i = %rl; %i <= 5; %i++)
		%wt3 = "0"@%wt3@"";

	return %wt1@%wt2@%wt3;
}

function GetFileName(%filename) {
	for(%i = 0; %i <= 10; %i++) {
		if(isFile("temp\\[RM]"@%filename@"["@%i@"].cs")) {
			return "[RM]"@%filename@"["@%i@"].cs";
		}
	}
	return False;
}

function ClearFileName(%name) {
	for(%i = 0; %i <= 10; %i++) {
		if(isFile("temp\\[RM]"@%name@"["@%i@"].cs")) {
			File::Delete("temp\\[RM]"@%name@"["@%i@"].cs");
		}
	}
	return True;
}

function FindWord(%list, %word) {

	%found = "";
	%i = 0;

	while(%found == "") {

		%w1 = getWord(%list, %i);

		if(%w1 == %word)
			return %i;
		else if(%w1 == -1)
			return -1; //didn't find %word

		%i++;

	}
}

function GetWordCount(%list) {
	if(%list == "")
		return -1;

	for(%i = 0; GetWord(%list, %i) != -1; %i++) {}

	return %i-1;
}

function GetWords(%list, %start, %end) {

	for(%i = %start; %i <= %end; %i++)
		%words = %words @ GetWord(%list, %i)@" ";

	return String::NEWgetSubStr(%words, 0, String::len(%words)-1);
}

function Flash(%Client) {
	remoteEval(%Client, "Flash");
}


//$RM_Time = "��@ Wed Feb 06 17:14:56 2002";
function StartRMTime() {
	if(!$StartRMTime) {
		$StartRMTime = true;

		//%asdf = getWord($RM_Time, 0);
		$RMTime::WeekDay = getWord($RM_Time, 1);
		$RMTime::Month = getWord($RM_Time, 2);
		$RMTime::Day = getWord($RM_Time, 3);
		%fullTime = getWord($RM_Time, 4);// HR:MIN:SEC
		$RM::Year = getWord($RM_Time, 5);

		//break apart %fullTime
		%fullTime = String::Replace(String::Replace(%fullTime, ":", " "), ":", " ");// Remove Both :
		$RMTime::Hour = getWord(%fullTime, 0);
		$RMTime::Min = getWord(%fullTime, 1);
		$RMTime::Sec = getWord(%fullTime, 2);

		RMSimTime();
	}
}
$RMDays[1] = "Sun";
$RMDays[2] = "Mon";
$RMDays[3] = "Tue";
$RMDays[4] = "Wed";
$RMDays[5] = "Tur";
$RMDays[6] = "Fri";
$RMDays[7] = "Sat";
$RMDays[8] = "NULL";

$RMDays["Sun"] = 1;
$RMDays["Mon"] = 2;
$RMDays["Tue"] = 3;
$RMDays["Wed"] = 4;
$RMDays["Tur"] = 5;
$RMDays["Fri"] = 6;
$RMDays["Sat"] = 7;

function RMSimTime() {//DO NOT CALL THIS, THE SERVER WILL WHEN NEEDED
	$RMTime::Sec++;
	if($RMTime::Sec >= 60) {
		$RMTime::Sec = 0;
		$RMTime::Min++;
	}
	if($RMTime::Min >= 60) {
		$RMTime::Min = 0;
		$RMTime::Hour++;
	}
	if($RMTime::Hour >= 24) {
		$RMTime::Hour = 0;
		$RMTime::Day++;
		$RMTime::WeekDay = $RMDays[$RMDays[$RMTime::WeekDay]++];
		if($RMTime::WeekDay == "NULL")
			$RMTime::WeekDay = "Mon";
	}
	if($RMTime::Day >= $MONTHLIMITS[$RMTime::Month, $RM::Year]) {
		$RMTime::Month = "";
	}
	schedule("RMSimTime();", 1);
}

$RMMonths[1] = "Jan";
$RMMonths[2] = "Feb";
$RMMonths[3] = "Mar";
$RMMonths[4] = "Apr";
$RMMonths[5] = "May";
$RMMonths[6] = "Jun";
$RMMonths[7] = "Jul";
$RMMonths[8] = "Aug";
$RMMonths[9] = "Sep";
$RMMonths[10] = "Oct";
$RMMonths[11] = "Nov";
$RMMonths[12] = "Dec";

$MONTHLIMITS[Feb, 2002] = 28;
$MONTHLIMITS[Mar, 2002] = 31;
$MONTHLIMITS[Apr, 2002] = 30;
$MONTHLIMITS[May, 2002] = 31;
$MONTHLIMITS[Jun, 2002] = 30;
$MONTHLIMITS[Jul, 2002] = 31;
$MONTHLIMITS[Aug, 2002] = 30;
$MONTHLIMITS[Sep, 2002] = 31;
$MONTHLIMITS[Oct, 2002] = 30;
$MONTHLIMITS[Nov, 2002] = 31;
$MONTHLIMITS[Dec, 2002] = 30;

$MONTHLIMITS[Jan, 2003] = 31;
$MONTHLIMITS[Feb, 2003] = 28;
$MONTHLIMITS[Mar, 2003] = 31;
$MONTHLIMITS[Apr, 2003] = 30;
$MONTHLIMITS[May, 2003] = 31;
$MONTHLIMITS[Jun, 2003] = 30;
$MONTHLIMITS[Jul, 2003] = 31;
$MONTHLIMITS[Aug, 2003] = 30;
$MONTHLIMITS[Sep, 2003] = 31;
$MONTHLIMITS[Oct, 2003] = 30;
$MONTHLIMITS[Nov, 2003] = 31;
$MONTHLIMITS[Dec, 2003] = 30;

$MONTHLIMITS[Jan, 2004] = 31;
$MONTHLIMITS[Feb, 2004] = 29;
$MONTHLIMITS[Mar, 2004] = 31;
$MONTHLIMITS[Apr, 2004] = 30;
$MONTHLIMITS[May, 2004] = 31;
$MONTHLIMITS[Jun, 2004] = 30;
$MONTHLIMITS[Jul, 2004] = 31;
$MONTHLIMITS[Aug, 2004] = 30;
$MONTHLIMITS[Sep, 2004] = 31;
$MONTHLIMITS[Oct, 2004] = 30;
$MONTHLIMITS[Nov, 2004] = 31;
$MONTHLIMITS[Dec, 2004] = 30;

$MONTHLIMITS[Jan, 2005] = 31;
$MONTHLIMITS[Feb, 2005] = 28;
$MONTHLIMITS[Mar, 2005] = 31;
$MONTHLIMITS[Apr, 2005] = 30;
$MONTHLIMITS[May, 2005] = 31;
$MONTHLIMITS[Jun, 2005] = 30;
$MONTHLIMITS[Jul, 2005] = 31;
$MONTHLIMITS[Aug, 2005] = 30;
$MONTHLIMITS[Sep, 2005] = 31;
$MONTHLIMITS[Oct, 2005] = 30;
$MONTHLIMITS[Nov, 2005] = 31;
$MONTHLIMITS[Dec, 2005] = 30;

function RMgetTime(%time) {
	if(%time == "")
		return $RMTime::Hour@":"@$RMTime::Min@":"@$RMTime::Sec;
	if(%time == Hour || %time == H)
		return $RMTime::Hour;
	if(%time == Min || %time == M)
		return $RMTime::Min;
	if(%time == Sec || %time == S)
		return $RMTime::Sec;
	if(%time == Day || %time == D)
		return $RMTime::Day;
	if(%time == WeekDay || %time == WD)
		return $RMTime::WeekDay;
	if(%time == Month || %time == Mo)
		return $RMTime::Month;
	if(%time == Year || %time == Y)
		return $RMTime::Year;
	if(%time == MDY)
		return $RMTime::Month@"/"@$RMTime::Day@"/"@$RMTime::Year;
	if(%time == Full || %time == F)
		return $RMTime::WeekDay@" "@$RMTime::Month@" "@$RMTime::Day@" "@$RMTime::Year@" "@$RMTime::Hour@":"@$RMTime::Min@":"@$RMTime::Sec;
}


function FixDataString::Save(%Client, %name) {

	%tmpStr[0] = $BankStorage[%Client];
	%tmpStr[1] = $ClientData[%Client, "ItemList"];

	%max = 1;

	%SaveTo[0] = 16; // BankStorage
	%SaveTo[1] = 45; // ItemList

	%len[0] = String::len(%tmpStr[0]);
	%len[1] = String::len(%tmpStr[1]);

	%maxlen = 450;

	%checklen[0] = floor(%len[0] / %maxlen)+1;
	%checklen[1] = floor(%len[1] / %maxlen)+1;

	for(%i = 0; %i <= %max; %i++) {

		%chunks = 1;
		%curpos = 0;

		while(!%isdone) {

			if(%checklen[%i] > %chunks) {

				$SaveData["[\""@%name@"\", 0, "@%SaveTo[%i]@", "@%chunks@"]"] = String::NEWgetSubStr(%tmpStr[%i], %curpos, %maxlen);

				%curpos += %maxlen;

				%tmpStr[%i] = String::NEWgetSubStr(%tmpStr[%i], %curpos, 99999);

				%chunks++;

			}
			else {

				$SaveData["[\""@%name@"\", 0, "@%SaveTo[%i]@", "@%chunks@"]"] = %tmpStr[%i];
				break;
			}
		}
	}
}

function FixDataString::Load(%Client, %name) {

	//TEMP FOR BETA ============
	%tmpStr[0] = $SaveData[%name, 0, 16];
	%tmpStr[1] = $SaveData[%name, 0, 45];

	if(%tmpStr[0] != "")
		$BankStorage[%Client] = $SaveData[%name, 0, 16];
	if(%tmpStr[1] != "")
		$Client::tmp[%Client, "ItemList"] = $SaveData[%name, 0, 45];
	//==========================

	for(%i=1;$SaveData[%name, 0, 16, %i] != "" && $SaveData[%name, 0, 16, %i] != -1;%i++) {
		$BankStorage[%client] = $BankStorage[%client] @ $SaveData[%name, 0, 16, %i];
		$SaveData[%name, 0, 16, %i] = "";
	}
	for(%i=1;$SaveData[%name, 0, 45, %i] != "" && $SaveData[%name, 0, 45, %i] != -1;%i++) {
		$Client::tmp[%Client, "ItemList"] = $Client::tmp[%Client, "ItemList"] @ $SaveData[%name, 0, 45, %i];
		$SaveData[%name, 0, 45, %i] = "";
	}
}


//EmplacementPack::rotVector(%vec,%rot)
//From mod Shifter
function rotVector(%vec, %rot) {

	%vec_x = getWord(%vec, 0);
	%vec_y = getWord(%vec, 1);
	%vec_z = getWord(%vec, 2);
	%basevec = %vec_x@" "@%vec_y@" 0";
	%basedis = Vector::getDistance( "0 0 0", %basevec);
	%normvec = Vector::normalize( %basevec );
	%baserot = Vector::add( Vector::getRotation( %normvec ), "1.571 0 0" );
	%newrot = Vector::add( %baserot, %rot );
	%newvec = Vector::getFromRot( %newrot, %basedis, %vec_z );
	return %newvec;
}

function ClientJetSmoke(%Client, %bool) {
	%this = Client::getOwnedObject(%Client);
	if(%bool) {
		$MakeSmoke[%this] = True;
		CheckIsJetting(%this);
	}
	else
		$MakeSmoke[%this] = "";
}


function ClearScreen(%n, %m) {
	//echo("Clear "@%n@" "@%m);
	Control::setValue("ATKText"@%n, "");
	$TextInUse[%n] = "";
	if(%m == fix)
		Control::setPosition("ATKText"@%n, 0, 107);
}

function Clear() {  for(%num = 1; %num < 6; %num++) { Control::setValue("ATKText"@%num, ""); $TextInUse[%num] = ""; } Control::setValue("ZONEText", ""); $ZoneTextInUse = ""; }

//%pos = Control::getPosition("ATKText1"); echo(%pos);

//function tst() { %x = 2; %y = 5;
//	for(%i = 1; %i <= 5; %i++) {
//		%newY = %y * -%i;
//		schedule("hhhh(\"TEXT\", "@%x@", "@%newY@");", %i / 15);
//	}
//	%cnt = 0;
//	for(%i = 4; %i >= 1; %i--) {
//		%newY = %y * -%i;
//		schedule("hhhh(\"TEXT\", "@%x@", "@%newY@");", (%cnt++ / 15) + "0.5");
//	}
//}

function hhhh(%a,%b,%c,%d) { echo(%a@" "@%b@" "@%c@" "@%d); }

//if($sdhkjkdshf){
ExplosionData ArmorsmokeExp
{
	shapeName = "smoke.dts";
	faceCamera = true;
	randomSpin = true;
	hasLight = false;
	lightRange = 0;
	timeScale = 2;	//15
	timeZero = 0.2;
	timeOne = 0.9;
	colors[0] = { 0.25, 0.25, 1.0 };
	colors[1] = { 0.25, 0.25, 1.0 };
	colors[2] = { 1.0, 1.0, 1.0 };
	radFactors = { 1.0, 1.0, 1.0 };
};

ExplosionData fakeExp
{
	shapeName = "smoke.dts";
	faceCamera = true;
	randomSpin = true;
	hasLight = false;
	lightRange = 0;
	timeScale = 0.01;
	timeZero = 0.2;
	timeOne = 0.9;
	colors[0] = { 0.25, 0.25, 1.0 };
	colors[1] = { 0.25, 0.25, 1.0 };
	colors[2] = { 1.0, 1.0, 1.0 };
	radFactors = { 1.0, 1.0, 1.0 };
};

MineData ArmorDummy
{
	className = "smokepuff";
    shapeFile = "smoke";
    shadowDetailMask = 1;
    explosionId = fakeExp;
	explosionRadius = 0;
	damageValue = 0;
	damageType = $NullDamageType;
	kickBackStrength = 0;
	triggerRadius = 0;
	maxDamage = 0;
	destroyDamage = 2.0;
	damageLevel = {1.0, 1.0};
	mass = -1;
	drag = 0;
};

GrenadeData ArmorSmokeGren
{
	explosionTag       = ArmorsmokeExp;
	collideWithOwner   = True;
	ownerGraceMS       = 250;
	collisionRadius    = 0;
	mass               = -1;
	elasticity         = 0;
	damageClass        = 1;
	damageValue        = 0;
	damageType         = $NullDamageType;
	explosionRadius    = 0;
	kickBackStrength   = 0;
	maxLevelFlightDist = 0;
	totalTime          = 0.3;
	liveTime           = 0.3;
	projSpecialTime    = 0.01;
};

//}

function dot_op_ArmorDummyDetonate(%this, %vel) {
	%player = Client::getOwnedObject(%this.deployer);
	if(!%player)
		return;
	%pos = Gamebase::getposition(%this);
	%rot = Gamebase::getrotation(%this);
	%dir = Vector::getFromRot(%rot);
	%pos = Vector::add(%pos, "0 0 2");
	%trans = "0 0 0 0 0 0 0 0 0 "@%pos;

	%velX = getWord(%vel, 0);
	%velY = getWord(%vel, 1);
	if(%velX >= %velY)
		%topvel = %velX;
	else
		%topvel = %velY;
	%timeperiod = ((%topvel / %this.maxSpeed) * -0.20);
	if(%timeperiod > 0)
		%timeperiod = %timeperiod * -1;
	%timeperiod = %timeperiod + 1;
	schedule("Projectile::spawnProjectile(ArmorSmokeGren, \""@%trans@"\", \""@%player@"\", \"0 0 0\");",%timeperiod);
}

function CheckIsJetting(%this) {
	if(IsDead(%this) || !$MakeSmoke[%this])
		return;
	else if(Player::isJetting(%this) == true)
		%this.ArmorSetSmoke();
	Schedule("CheckIsJetting("@%this@");", 0.5, %this);
}

function dot_op_ArmorSetSmoke(%this) {
	if(%this) {
		%pos = GameBase::getPosition(%this);
		%x = getWord(%pos, 0);
		%y = getWord(%pos, 1);
		%z = getWord(%pos, 2) + 0.8;//-3

		%ExpPos = %x@" "@%y@" "@%z;

		%obj[%this] = newObject("","Mine","ArmorDummy");
		addToSet("MissionCleanup", %obj[%this]);
		GameBase::setPosition(%obj[%this], %ExpPos);

		%obj[%this].ArmorDummyDetonate(Item::getVelocity(%this));

		deleteObject(%obj[%this]);
	}
}

