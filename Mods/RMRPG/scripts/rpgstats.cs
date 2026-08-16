
$MaxAPStats = 5000;

function getFinalLVL(%client) {
//echo(%client@": "@client::getname(%client));
	if($templvl[%Client] == "")
		%a = GetLevel(getFixedExp(%Client));
	else
		%a = $templvl[%Client];
	return Cap(%a, 1, 999);
}

function getTNL(%Client, %tag) {

	%a = getFinalLVL(%Client);

	if(%a == 999)
		return 0;

	%have = getFixedExp(%Client);
	%need = $EXPtable[%a+1];

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
	}

	if(%tag == "")
		return %wt1@" "@%wt2@" "@%wt3;
	else {
		if(%wt1 > 0)
			%tnl = %wt1@"b ";
		if(%wt2 > 0)
			%tnl = %tnl@%wt2@"m ";
		%tnl = %tnl@%wt3;
		return %tnl;
	}
}

function getGainedExp(%Client, %tag) {

	%a = getFinalLVL(%Client);

	if(%a < 999)
	{
		%w1 = $EXPtable[%a];
		%w2 = getFixedExp(%Client);

		%wt1 = GetWord(%w2, 0) - GetWord(%w1, 0);
		%wt2 = GetWord(%w2, 1) - GetWord(%w1, 1);
		%wt3 = GetWord(%w2, 2) - GetWord(%w1, 2);

		if(%wt2 < 0) {
			%wt1--;
			%wt2 += 1000;
		}

		if(%wt3 < 0) {
			%wt2--;
			%wt3 += 1000000;
		}

		if(%tag == "")
			return %wt1@" "@%wt2@" "@%wt3;
		else
			return %wt1@"b "@%wt2@"m "@%wt3;
	}
}

function getFinalSTA(%Client) {

	%a = $AP[%Client, 6];
	%b = AddPoints(%Client, 12);

	return Cap(%a + %b, 1, $MaxAPStats);
}

function getFinalSTR(%Client) {

	%a = $AP[%Client, 1];
	%b = AddPoints(%Client, 1);

	return Cap(%a + %b, 1, $MaxAPStats);
}

function WeightAllow(%Client) {

	%a = getFinalSTR(%Client);
	%b = Cap(%a * 1.25, 1, 1000);

	return %b;
}

function DmgAdj(%Client) {

	%a = getFinalSTR(%Client);
	%b = round(%a / 2); //2.5

	return %b;
}

function GetBashPow(%Client) {

	%a = getFinalSTR(%Client);
	%b = round(%a * 2);

	return %b;
}

function GetHitProb(%Client) {

	%a = getFinalSTR(%Client);
	%b = round(%a / 20);

	return %b;
}

function getFinalDEX(%Client) {

	%a = $AP[%Client, 2];
	%b = AddPoints(%Client, 2);

	return Cap(%a + %b, 1, $MaxAPStats);
}

function DefensiveAdj(%Client) {

	%a = getFinalDEX(%Client);
	%b = round(%a / 20);

	return %b;
}

function MAttackAdj(%Client) {

	%a =getFinalDEX(%Client);
	%b = round(%a / 20);

	return %b;
}
function getFinalCON(%Client) {

	%a = $AP[%Client, 3];
	%b = AddPoints(%Client, 3);

	return Cap(%a + %b, 1, $MaxAPStats);
}
function getFinalINT(%Client) {

	%a = $AP[%Client, 4];
	%b = AddPoints(%Client, 4);

	return Cap(%a + %b, 1, $MaxAPStats);
}
function getFinalWIS(%Client) {

	%a = $AP[%Client, 5];
	%b = AddPoints(%Client, 5);

	return Cap(%a + %b, 1, $MaxAPStats);
}

function MagicalDefAdj(%Client) {

	%a = getFinalWIS(%Client);
	%b = round(%a / 20);

	return %b;
}

function SpellFailure(%Client) {

	%a = getFinalWIS(%Client);
	if(%a <= 15)
		%a = 20;
	else if(%a >= 16 && %a <= 35)
		%a = 5;
	else if(%a >= 36 && %a <= 50)
		%a = 1;
	else if(%a >= 51)
		%a = -1;
	return %a;
}

function getFinalLCK(%Client) {
	return $LCK[%Client];
}

function getFinalMDEF(%Client) {

	%a = MagicalDefAdj(%Client);
	%b = AddPoints(%Client, 15);

	return (%a + %b);
}

function getFinalDEF(%Client) {

	%a = DefensiveAdj(%Client);
	%b = AddPoints(%Client, 7);

	return (%a + %b);
}

function GetTHAC() { echo("GetTHAC called"); }

function MenuAP(%Client) {

	if($ClientData[%Client, APPoints] > 0)
		Client::buildMenu(%Client, "Current stats: [AP "@$ClientData[%Client, APPoints]@"]", "addAPto", true);
	else
		Client::buildMenu(%Client, "Current stats:", "null", true);

	Client::addMenuItem(%Client, "1STR [ "@floor(getFinalSTR(%Client))@" ]", 1);
	Client::addMenuItem(%Client, "2DEX [ "@floor(getFinalDEX(%Client))@" ]", 2);
	Client::addMenuItem(%Client, "3CON [ "@floor(getFinalCON(%Client))@" ]", 3);
	Client::addMenuItem(%Client, "4INT [ "@floor(getFinalINT(%Client))@" ]", 4);
	Client::addMenuItem(%Client, "5WIS [ "@floor(getFinalWIS(%Client))@" ]", 5);
	Client::addMenuItem(%Client, "6STA [ "@floor(getFinalSTA(%Client))@" ]", 6);
}

function processMenuaddAPto(%Client, %stat) {

	if($ClientData[%Client, APPoints] < $MaxAPStats) {

		%Client.bulk = Cap(floor(%Client.bulk), 1, $ClientData[%Client, APPoints]);

		if(%Client.bulk+$ClientData[%Client, APPoints] > $MaxAPStats)
			%Client.bulk = 1;

		$ClientData[%Client, APPoints] -= %Client.bulk;

		$AP[%Client, %stat] += %Client.bulk;

		%Client.bulk = 1;

		refreshAll(%Client);

		MenuAP(%Client);
	}
}

function processMenunull(%Client) {}

function MenuGroup(%Client) {

	Client::buildMenu(%Client, "Pick a group:", "pickgroup", true);
	Client::addMenuItem(%Client, "1Priest", "Priest");
	Client::addMenuItem(%Client, "2Rogue", "Rogue");
	Client::addMenuItem(%Client, "3Warrior", "Warrior");
	Client::addMenuItem(%Client, "4Wizard", "Wizard");

}
function processMenupickgroup(%Client, %opt) {

	$GROUP[%Client] = %opt;

	%Client.choosingGroup = "";
	%Client.choosingClass = True;

	MenuClass(%Client);
}

function MenuClass(%Client) {

	Client::buildMenu(%Client, "Pick a class:", "pickclass", true);

	%num = 0;
	for(%i = 1; $ClassName[%i] != ""; %i++) {
		if(String::ICompare($GROUP[%Client], $ClassGroup[$ClassName[%i]]) == 0) {
			%num++;
			Client::addMenuItem(%Client, %num @ $ClassName[%i], $ClassName[%i]);
		}
	}
	Client::addMenuItem(%Client, "x<-- BACK", "back");

	return;
}
function processMenupickclass(%Client, %opt) {
	if(%opt == "back") {
		%Client.choosingClass = "";
		%Client.choosingGroup = True;
		$GROUP[%Client] = "";

		MenuGroup(%Client);
		return;
	}

	$CLASS[%Client] = %opt;

	//let the player enter the world
	%Client.choosingClass = "";
	Game::playerSpawn(%Client, false);

	//Set up some stats
	GiveStartUpStats(%Client, $CLASS[%Client]);

	//Client::sendmessage(%Client, 0, "Welcome new warrior to the world of Red Moon.");
	remoteEval(%Client,"ATKText", "<jc>Welcome new warrior to the world of \nRed Moon RPG.", wait);
	schedule("CheckIsBlessed("@%Client@");", 1);
	schedule("DoServerStatus("@%Client@");", 2);
	schedule("SaveCharacter("@%Client@");", 5);

	centerprint(%Client, "<f1>Server powered by the Red Moon RPG MOD version "@$RMver@"<f0>\n\n"@$loginMsg, 15);
}

function GiveStartUpStats(%Client, %class) {

	if($isBlessed[%Client] == true)
		%BlessedSoul = 2;
	else
		%BlessedSoul = 0;

	$AP[%Client, 1] += $AddAPSTRClass[%class] * 6 + %BlessedSoul;
	$AP[%Client, 2] += $AddAPDEXClass[%class] * 6 + %BlessedSoul;
	$AP[%Client, 3] += $AddAPCONClass[%class] * 6 + %BlessedSoul;
	$AP[%Client, 4] += $AddAPINTClass[%class] * 6 + %BlessedSoul;
	$AP[%Client, 5] += $AddAPWISClass[%class] * 6 + %BlessedSoul;
	$AP[%Client, 6] += $AddAPSTAClass[%class] * 6 + %BlessedSoul;

	setHP(%Client, AddHP(%Client, 1, True));
	setMANA(%Client, getMaxMANA(%Client));
	setSTAMINA(%Client, getSTAMINA(%Client));

	GiveThisStuff(%Client, $ClientData[%Client, "NEWItemList"], false);
	GiveThisStuff(%Client, $ClientData[%Client, "NEWEquipList"], false);
	GiveThisStuff(%Client, $ClientData[%Client, "NEWQuestList"], false);

	$ClientData[%Client, "NEWItemList"] = "";
	$ClientData[%Client, "NEWEquipList"] = "";
	$ClientData[%Client, "NEWQuestList"] = "";

	$ClientData[%Client, Skill_BlackSmith] = $SKILLBlackSmith[%class];

	$COINS[%Client] = Cap(GetRoll($initcoins[$GROUP[%Client]]), 0, $MaxCOINS[%Client]);
}

function GetLevel(%ex) {

	if(%ex == "   ")
		return;

	//for(%i = 1; %i <= 999; %i++) {
	//	if(GetWord(%ex, 0) > GetWord($EXPtable[%i], 0))
	//		%lvl = %i;
	//	else if(GetWord(%ex, 1) > GetWord($EXPtable[%i], 1) && GetWord(%ex, 0) >= GetWord($EXPtable[%i], 0))
	//		%lvl = %i;
	//	else if(GetWord(%ex, 2) >= GetWord($EXPtable[%i], 2) && GetWord(%ex, 1) >= GetWord($EXPtable[%i], 1) && GetWord(%ex, 0) >= GetWord($EXPtable[%i], 0))
	//		%lvl = %i;
	//	else {


	%ex = FixNum(%ex);
	for(%i = 1; %i <= 999; %i++) {
		if(%ex >= $EXPFixedtable[%i]) {//FixNum($EXPtable[%i])) {
			//echo("if "@%ex@" >= "@GetWord($EXPtable[%i], 0)@GetWord($EXPtable[%i], 1)@GetWord($EXPtable[%i], 2));
			%lvl = %i;
		}
		else
			break;
	}
//	echo("Got LVL: "@%lvl);
	if(%lvl != "")
		return %lvl;

	echo("Error: Getlevel(Exp); didn't return a level.");
}

function GetExp(%level) {

	if(%level >= 999)
		return 0;

	return $EXPtable[%level];
}

function DistributeExpForKilling(%damagedClient) {

	%dname = Client::getName(%damagedClient);
	%dlvl = getFinalLVL(%damagedClient);

	%count = 0;

	//parse $damagedBy and create %finalDamagedBy
	%nameCount = 0;
	%listCount = 0;
	for(%i = 1; %i <= $maxDamagedBy; %i++) {
		if($damagedBy[%dname, %i] != "") {
			%listCount++;

			%n = GetWord($damagedBy[%dname, %i], 0);
			%d = GetWord($damagedBy[%dname, %i], 1);

			%flag = 0;
			for(%z = 1; %z <= %nameCount; %z++) {
				if(%finalDamagedBy[%z] == %n) {
					%flag = 1;
					%dCounter[%n]++;// += %d;
				}
			}
			if(%flag == 0) {
				%nameCount++;
				%finalDamagedBy[%nameCount] = %n;
				%dCounter[%n]++; // = %d;

				%p = IsInWhichParty(%n);
				if(%p != -1) {
					%id = GetWord(%p, 0);
					%inv = GetWord(%p, 1);
					if(%inv == -1) {
						%tmppartylist[%id] = %tmppartylist[%id] @ %n @ " ";
						if(String::findSubStr(%tmpl, %id @ " ") == -1)
							%tmpl = %tmpl @ %id @ " ";
					}
				}
			}
			%total += %d;
		}
		$damagedBy[%dname, %i] = "";
	}

	//parse thru all tmppartylists and determine the number of same party members involved in exp split
	for(%w = 0; (%a = GetWord(%tmpl, %w)) != -1; %w++) {
		%n = CountObjInList(%tmppartylist[%a]);
		for(%ww = 0; (%aa = GetWord(%tmppartylist[%a], %ww)) != -1; %ww++)
			%partyFactor[%aa] = %n;
	}

	//distribute exp
	for(%i = 1; %i <= %nameCount; %i++) {

		if(%finalDamagedBy[%i] != "") {

			%listClientId = NEWgetClientByName(%finalDamagedBy[%i]);
			%slvl = getFinalLVL(%listClientId);

			%value = GetExpValue(%dlvl, %slvl, %listClientId);

			%final = round( %value * (%dCounter[%finalDamagedBy[%i]] / %listCount) );

			%pf = %partyFactor[%finalDamagedBy[%i]];
			if(%pf >= 2) //                1.0 +
				%pvalue = round(%final * (0.5 + (%pf * 0.1)));
			else
				%pvalue = 0;

			if(RPG::isAiControlled(%damagedClient)) {
				if(%final < 0)
				%final = 1;
			}
			else { //killed a human player
				if(%dlvl+50 > %slvl)
					%final = floor(%final/10);
				else
					%final = floor(%final/4);
			}

			GiveExp(%listClientId, %final);

			if(%final > 0)
				Client::sendMessage(%listClientId, 0, %dname@" has died and you gained "@FixM(%final)@" EXP!");
			else if(%final < 0)
				Client::sendMessage(%listClientId, 0, %dname@" has died and you lost "@-FixM(%final)@" EXP!");
			else
				Client::sendMessage(%listClientId, 0, %dname@" has died.");

			if(%pvalue != 0) {
				GiveExp(%listClientId, %pvalue);
				Client::sendMessage(%listClientId, $MsgWhite, "You have gained "@%pvalue@" party experience!");
			}

			Game::refreshClientScore(%listClientId);

			if($showexp[%listClientId]) {
				if(!Player::isAiControlled(%listclientId))
					remoteEval(%listClientId, "RefreshEXPset", Fix(getTNL(%listClientId, Strip), %listClientId, EXP));
			}
		}
	}
}

function GetExpValue(%dlvl, %slvl, %listClient) {

	%value = ((%dlvl * 7.2) * %dlvl) + 35;

	%a = (%dlvl - %slvl);
	if(%a != 0) {
		%value += %a * 1.5; //0.5;

	}
	%bonus = AddPoints(%listClient, "EXPB", "EquipList");
	if(%bonus > 0) {
		%value += round(%value * (%bonus/100));
	}

	return floor(%value);
}

function StartStatSelection(%Client) {
	%group = nameToId("MissionGroup\\ObserverDropPoints");
	%observerMarker = Group::getObject(%group, 0);

	Client::setControlObject(%Client, Client::getObserverCamera(%Client));
	Observer::setFlyMode(%Client, GameBase::getPosition(%observerMarker), GameBase::getRotation(%observerMarker), false, true);

	MenuGroup(%Client);
}

//===NEW AddSkill per lvl
%class = "Cleric"; //12
$AddAPSTRClass[%class] = 2;
$AddAPDEXClass[%class] = 2;
$AddAPCONClass[%class] = 2;
$AddAPINTClass[%class] = 2.5;
$AddAPWISClass[%class] = 2.5;
$AddAPSTAClass[%class] = 1;

$SKILLBlackSmith[%class] = 100; // = 1.00

$CLASSTOTALAPGAIN[%class] = $AddAPSTRClass[%class]+$AddAPDEXClass[%class]+$AddAPCONClass[%class]+$AddAPINTClass[%class]+$AddAPWISClass[%class]+$AddAPSTAClass[%class];

%class = "Druid"; //12
$AddAPSTRClass[%class] = 2;
$AddAPDEXClass[%class] = 2;
$AddAPCONClass[%class] = 1.5;
$AddAPINTClass[%class] = 2;
$AddAPWISClass[%class] = 3;
$AddAPSTAClass[%class] = 1.5;

$SKILLBlackSmith[%class] = 120; // = 1.2

$CLASSTOTALAPGAIN[%class] = $AddAPSTRClass[%class]+$AddAPDEXClass[%class]+$AddAPCONClass[%class]+$AddAPINTClass[%class]+$AddAPWISClass[%class]+$AddAPSTAClass[%class];

%class = "Thief";
$AddAPSTRClass[%class] = 1; //12
$AddAPDEXClass[%class] = 4;
$AddAPCONClass[%class] = 1;
$AddAPINTClass[%class] = 2;
$AddAPWISClass[%class] = 1;
$AddAPSTAClass[%class] = 3;

$SKILLBlackSmith[%class] = 50; // = 0.5

$CLASSTOTALAPGAIN[%class] = $AddAPSTRClass[%class]+$AddAPDEXClass[%class]+$AddAPCONClass[%class]+$AddAPINTClass[%class]+$AddAPWISClass[%class]+$AddAPSTAClass[%class];

%class = "Bard";
$AddAPSTRClass[%class] = 2; //12
$AddAPDEXClass[%class] = 2;
$AddAPCONClass[%class] = 2;
$AddAPINTClass[%class] = 2;
$AddAPWISClass[%class] = 2;
$AddAPSTAClass[%class] = 2;

$SKILLBlackSmith[%class] = 100;

$CLASSTOTALAPGAIN[%class] = $AddAPSTRClass[%class]+$AddAPDEXClass[%class]+$AddAPCONClass[%class]+$AddAPINTClass[%class]+$AddAPWISClass[%class]+$AddAPSTAClass[%class];

%class = "Fighter";
$AddAPSTRClass[%class] = 3.5; //12
$AddAPDEXClass[%class] = 2;
$AddAPCONClass[%class] = 3.5;
$AddAPINTClass[%class] = "0.5";
$AddAPWISClass[%class] = "0.5";
$AddAPSTAClass[%class] = 2;

$SKILLBlackSmith[%class] = 75;

$CLASSTOTALAPGAIN[%class] = $AddAPSTRClass[%class]+$AddAPDEXClass[%class]+$AddAPCONClass[%class]+$AddAPINTClass[%class]+$AddAPWISClass[%class]+$AddAPSTAClass[%class];

%class = "Paladin";
$AddAPSTRClass[%class] = 2;//12
$AddAPDEXClass[%class] = 2;
$AddAPCONClass[%class] = 3;
$AddAPINTClass[%class] = 2;
$AddAPWISClass[%class] = 2;
$AddAPSTAClass[%class] = 1;

$SKILLBlackSmith[%class] = 200;

$CLASSTOTALAPGAIN[%class] = $AddAPSTRClass[%class]+$AddAPDEXClass[%class]+$AddAPCONClass[%class]+$AddAPINTClass[%class]+$AddAPWISClass[%class]+$AddAPSTAClass[%class];

%class = "Ranger";
$AddAPSTRClass[%class] = 2;//12
$AddAPDEXClass[%class] = 3.5;
$AddAPCONClass[%class] = 2;
$AddAPINTClass[%class] = 1;
$AddAPWISClass[%class] = 1.5;
$AddAPSTAClass[%class] = 2;

$SKILLBlackSmith[%class] = 500;

$CLASSTOTALAPGAIN[%class] = $AddAPSTRClass[%class]+$AddAPDEXClass[%class]+$AddAPCONClass[%class]+$AddAPINTClass[%class]+$AddAPWISClass[%class]+$AddAPSTAClass[%class];

%class = "Mage";
$AddAPSTRClass[%class] = 1.5; //12
$AddAPDEXClass[%class] = 1;
$AddAPCONClass[%class] = 1.5;
$AddAPINTClass[%class] = 3;
$AddAPWISClass[%class] = 4.5;
$AddAPSTAClass[%class] = 0.5;

$SKILLBlackSmith[%class] = 10;

$CLASSTOTALAPGAIN[%class] = $AddAPSTRClass[%class]+$AddAPDEXClass[%class]+$AddAPCONClass[%class]+$AddAPINTClass[%class]+$AddAPWISClass[%class]+$AddAPSTAClass[%class];

%class = "Summoner";
$AddAPSTRClass[%class] = 1.5; //12
$AddAPDEXClass[%class] = 1;
$AddAPCONClass[%class] = 1.5;
$AddAPINTClass[%class] = 3.5;
$AddAPWISClass[%class] = 3.5;
$AddAPSTAClass[%class] = 1;

$SKILLBlackSmith[%class] = 10;

$CLASSTOTALAPGAIN[%class] = $AddAPSTRClass[%class]+$AddAPDEXClass[%class]+$AddAPCONClass[%class]+$AddAPINTClass[%class]+$AddAPWISClass[%class]+$AddAPSTAClass[%class];

%class = "Machine";
$AddAPSTRClass[%class] = 2;
$AddAPDEXClass[%class] = 2;
$AddAPCONClass[%class] = 2;
$AddAPINTClass[%class] = 2;
$AddAPWISClass[%class] = 2;
$AddAPSTAClass[%class] = 2;

$SKILLBlackSmith[%class] = 200;

$CLASSTOTALAPGAIN[%class] = $AddAPSTRClass[%class]+$AddAPDEXClass[%class]+$AddAPCONClass[%class]+$AddAPINTClass[%class]+$AddAPWISClass[%class]+$AddAPSTAClass[%class];

%class = "Zolom";
$AddAPSTRClass[%class] = 2;
$AddAPDEXClass[%class] = 2;
$AddAPCONClass[%class] = 2;
$AddAPINTClass[%class] = 2;
$AddAPWISClass[%class] = 2;
$AddAPSTAClass[%class] = 2;

$SKILLBlackSmith[%class] = 200;

$CLASSTOTALAPGAIN[%class] = $AddAPSTRClass[%class]+$AddAPDEXClass[%class]+$AddAPCONClass[%class]+$AddAPINTClass[%class]+$AddAPWISClass[%class]+$AddAPSTAClass[%class];


%class = "";
//===New EXP system===//

//Deus_ex_Machina
//
//

function DebugTNLEXP(%lvl) {
//	if(%lvl == 999)
//		return 0;
	%have = $EXPtable[%lvl];
	%need = $EXPtable[%lvl+1];
	%wt1 = GetWord(%need, 0) - GetWord(%have, 0);
	%wt2 = GetWord(%need, 1) - GetWord(%have, 1);
	if(%wt2 < 0) {
		%wt1--;
		%wt2 += 1000;
	}
	%wt3 = GetWord(%need, 2) - GetWord(%have, 2);
	if(%wt3 < 0) {
		%wt2--;
		%wt3 += 1000000;
	}
	if(%wt1 > 0)
		%tnl = %wt1@"b ";
	if(%wt2 > 0)
		%tnl = %tnl@" "@%wt2@"m ";
	%tnl = %tnl@" "@%wt3;
	return %tnl;
}


function SetUpEXPtable() {
	echo("Setting up EXP table...");

	$EXPtable[1] = "0 0 0";
	$EXPFixedtable[1] = "0";
	for(%i = 2; %i <= 99; %i++) {
		$EXPtable[%i] = FixM( (((%i * 50) * %i) * (%i)) , True); // / 10 // / 2;
		$EXPFixedtable[%i] = FixNum($EXPtable[%i]);
	}
	echo("Setting up EXP table done!");
}
//SetUpEXPtable();

exec("ExpTable.cs");

function DebugEXP() {

	for(%i = 1; %i <= 999; %i++)
		echo("LVL: "@%i@" | EXP:"@$EXPtable[%i]@" TNL:"@DebugTNLEXP(%i));
}

//DebugEXP();
//File::delete("NEWEXPSYSTEM.cs");
//export("$EXPtable*", "config\\NEWEXPSYSTEM.cs");


///////////////////////////////
//RM2 ExpTable.....
function tst() {
	exec(rpgstats);
	_tst();
}

function _tst() {
	echo("Setting up EXP table...");

	$EXPtable[1] = "0 0 0";
	$EXPFixedtable[1] = "0";
	for(%i = 2; %i <= 99; %i++) {
		$EXPtable[%i] = FixM(      ( (%i * 50) * (%i+2)) * %i + pow(Cap(%i-20, 0, "inf"), 5)      , True);

	//	echo($EXPtable[%i]);
		$EXPFixedtable[%i] = FixNum($EXPtable[%i]);

		export("EXPFixedtable"@%i, "temp\\ExpTable.cs", 1);

	}

	echo("Setting up EXP table done!");

//	Debugtst();
}

function Debugtst() {

	echo("----------------------------------------------");
	for(%i = 1; %i <= 99; %i++)
		//echo("LVL: "@%i@" | EXP:"@$EXPtable[%i]@" TNL:"@DebugTNLtst(%i));
		echo("LVL: "@%i@" | TNL:"@DebugTNLtst(%i));
	echo("----------------------------------------------");
}

function DebugTNLtst(%lvl) {

	if(%lvl == 99)
		return 0;

	%have = $EXPtable[%lvl];
	%need = $EXPtable[%lvl+1];
	%wt1 = GetWord(%need, 0) - GetWord(%have, 0);
	%wt2 = GetWord(%need, 1) - GetWord(%have, 1);
	if(%wt2 < 0) {
		%wt1--;
		%wt2 += 1000;
	}
	%wt3 = GetWord(%need, 2) - GetWord(%have, 2);
	if(%wt3 < 0) {
		%wt2--;
		%wt3 += 1000000;
	}
	if(%wt1 > 0)
		%tnl = %wt1@"b ";
	if(%wt2 > 0)
		%tnl = %tnl@" "@%wt2@"m ";
	%tnl = %tnl@" "@%wt3;
	return %tnl;
}
///////////////////////////////
