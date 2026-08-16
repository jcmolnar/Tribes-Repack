#Minerva - Goddess of Wisdom

$EquipTag = "&";


$ShowMakeModel = true;
$ShowMakeItem = false;

//=========================
//  svar list:
//=========================
//1: STR
//2: DEX
//3: CON
//4: INT
//5: WIS

//6: ATK
//7: DEF
//8: Internal armor switching variable
//9: STA regen
//10: HP regen
//11: Mana regen
//12 STA

//13: HP
//14: MP

//15: MDEF

//HEAL: res HP
//MP: res MP
//STA: res STA

//20: Stop [Status]
//21: Cures [Status]

//AREA: radius (for status potions)
//Causes
//30: Poison
//31: Blind
//32: Mute
//33: Petrify

//EXPB: % exp boost

//Fire: % less dmg from Fire
//Ice:
//Lightning:
//Water:
//Earth:
//Wind:
//Black:
//Status: Status magic

$SpecialVarDesc[1] = "Str";
$SpecialVarDesc[2] = "Dex";
$SpecialVarDesc[3] = "Con";
$SpecialVarDesc[4] = "Int";
$SpecialVarDesc[5] = "Wis";
$SpecialVarDesc[6] = "Atk";
$SpecialVarDesc[7] = "Def";
$SpecialVarDesc[8] = "[Internal]";
$SpecialVarDesc[9] = "Sta regen";
$SpecialVarDesc[10] = "HP regen";
$SpecialVarDesc[11] = "Mana regen";
$SpecialVarDesc[12] = "Sta";
$SpecialVarDesc[13] = "HP";
$SpecialVarDesc[14] = "MP";
$SpecialVarDesc[15] = "MDef";

$SpecialVarDesc[HEAL] = "Restores HP";
$SpecialVarDesc[MP] = "Restores MP";
$SpecialVarDesc[STA] = "Restores STA";

$SpecialVarDesc[20] = "Stops";
$SpecialVarDesc[21] = "Cures";

$SpecialVarDesc[AREA] = "Cloud Radius";
$SpecialVarDesc[30] = "Poison lvl";
$SpecialVarDesc[31] = "Blind lvl";
$SpecialVarDesc[32] = "Mute lvl";
$SpecialVarDesc[33] = "Petrify lvl";

$SpecialVarDesc[EXPB] = "Exp Bonus %";

$SpecialVarDesc[Alvl] = "Alcohol lvl";

$RingAccessoryType = 1;
$BodyAccessoryType = 2;
$BootsAccessoryType = 3;
$BackAccessoryType = 4;
$ShieldAccessoryType = 5;
$TalismanAccessoryType = 6;
$SwordAccessoryType = 7;
$AxeAccessoryType = 8;
$PolearmAccessoryType = 9;
$BludgeonAccessoryType = 10;
$RangedAccessoryType = 11;
$ProjectileAccessoryType = 12;
$HeadAccessoryType = 13;
$HandsAccessoryType = 14;
$LegsAccessoryType = 15;
$OrbAccessoryType = 16;

$MetalAccessoryType = 17; // For Black Smith
$WoodAccessoryType = 18;
$RockAccessoryType = 19;
$FabricAccessoryType = 20;

$LocationDesc[$RingAccessoryType] = "Ring";
$LocationDesc[$BodyAccessoryType] = "Body";
$LocationDesc[$BootsAccessoryType] = "Feet";
$LocationDesc[$BackAccessoryType] = "Back";
$LocationDesc[$ShieldAccessoryType] = "Shield";
$LocationDesc[$TalismanAccessoryType] = "Talisman";
$LocationDesc[$SwordAccessoryType] = "Sword";
$LocationDesc[$AxeAccessoryType] = "Axe";
$LocationDesc[$PolearmAccessoryType] = "Polearm";
$LocationDesc[$BludgeonAccessoryType] = "Bludgeon";
$LocationDesc[$RangedAccessoryType] = "Ranged";
$LocationDesc[$ProjectileAccessoryType] = "Projectile";
$LocationDesc[$HeadAccessoryType] = "Head";
$LocationDesc[$HandsAccessoryType] = "Hands";
$LocationDesc[$LegsAccessoryType] = "Legs";
$LocationDesc[$OrbAccessoryType] = "Floating";

$maxAccessory[$GemAccessoryType] = 40;
$maxAccessory[$RingAccessoryType] = 10;
$maxAccessory[$BodyAccessoryType] = 1;
$maxAccessory[$BootsAccessoryType] = 1;
$maxAccessory[$BackAccessoryType] = 1;
$maxAccessory[$ShieldAccessoryType] = 1;
$maxAccessory[$TalismanAccessoryType] = 1;
$maxAccessory[$HeadAccessoryType] = 1;
$maxAccessory[$HandsAccessoryType] = 1;
$maxAccessory[$LegsAccessoryType] = 1;
$maxAccessory[$OrbAccessoryType] = 1;

$RingWeight = "0.1";



//=====================
// ACESSORY FUNCTIONS
//=====================

function GetAccessoryVar(%item, %type) {

	%nitem = getCroppedItem(%item);

	return $ItemData[%nitem, %type];
}

function getCroppedItem(%item) {

	//some REALLY weird stuff was happening here, where if i did
	//an if(String::getSubStr(%item, String::len(%item)-1, 1) == "0")
	//it wouldn't work properly.  so i added a xx at the end and that
	//helped with the if statement.

	%zitem = %item @ "xx";
	if(String::ICompare(String::getSubStr(%zitem, String::len(%zitem)-3, 3), $EquipTag@"xx") == 0)
		%nitem = String::getSubStr(%item, 0, String::len(%item)-1);
	else
		%nitem = %item;

	return %nitem;
}

function GetAccessoryList(%Client, %type, %filter) { echo("GetAccessoryList called"); }

function AddPoints(%Client, %char, %list2) {
	%add = 0;
	if(%char == "")
		return "no %char";
	if(%list2 == "QuestList")
		%list = $ClientData[%Client, "QuestList"];
	else {
		%list2 = "EquipList";
		%list = $ClientData[%Client, "EquipList"];
	}
	for(%i = 0; (%w = GetWord(%list, %i)) != -1; %i+=2) {
	//	%slot = "";
		%flag = true;

		if($ItemData[%w, type] == $ShieldAccessoryType) {
			if(!$ClientData[%Client, toggleShield])
				%flag = false;//Equipped a Shield BUT toggled it off!
		}
		//else if($ItemData[%w, className] == Backpack)
		//	%slot = $BackpackSlot;

		%count = getWord(%list, %i+1); //Client::getItemCount(%Client, %w, %list2);
		%tmp = $ItemData[%w, svar];

		for(%j = 0; (%e = GetWord(%tmp, %j)) != -1; %j+=2) {
			if(%e == %char && %flag)
				%add += GetRoll(GetWord(%tmp, %j+1)) * %count;
		}
	}

	//Check weapon in hand
	if((%w = $ClientData[%Client, UsingWeapon]) != "-1") {
		%tmp = $ItemData[%w, svar];
		for(%j = 0; (%e = GetWord(%tmp, %j)) != -1; %j+=2) {
			if(%e == %char)
				%add += GetRoll(GetWord(%tmp, %j+1));
		}
	}
	return %add;
}

function AddItemSpecificPoints(%item, %char, %DoRoll) {

	%tmp = $ItemData[%item, svar];

	for(%j = 0; (%e = GetWord(%tmp, %j)) != -1; %j+=2) {
		if(%e == %char) {
			if(!%DoRoll)
				%info = GetWord(%tmp, %j+1);
			else
				%info = GetRoll(GetWord(%tmp, %j+1));
			break;
		}
	}
	return %info;
}

function WhatSpecialVars(%item) {

	%tmp = $ItemData[%item, svar];

	if(%tmp == "NULL")
		return "None";

	if($ItemData[%item, BlockInfo])
		return "N/A";

	%t = "";
	for(%i = 0; (%s = GetWord(%tmp, %i)) != -1; %i+=2) {
		%n = GetWord(%tmp, %i+1);

		%t = %t @ $SpecialVarDesc[%s] @ ": " @ %n @ ", ";
	}
	return String::getSubStr(%t, 0, String::len(%t)-2);
}

function FindStatusPoints(%Client, %status, %list2) {

	if(%list2 == "QuestList")
		%list = $ClientData[%Client, "QuestList"];
	else {
		%list2 = "EquipList";
		%list = $ClientData[%Client, "EquipList"];
	}
	for(%i = 0; (%w = GetWord(%list, %i)) != -1; %i+=2) {
		%flag = true;
		if($ItemData[%w, type] == $ShieldAccessoryType) {
			if(!$ClientData[%Client, toggleShield])
				%flag = false;//Equipped a Shield BUT toggled it off!
		}
		%tmp = $ItemData[%w, svar];
		for(%j = 0; (%e = GetWord(%tmp, %j)) != -1; %j+=2) {
			if(%e == 20 && %flag)
				if(GetWord(%tmp, %j+1) == %status)
					return true;
		}
	}

	//Check weapon in hand
	if((%w = $ClientData[%Client, UsingWeapon]) != "-1") {
		%tmp = $ItemData[%w, svar];
		for(%j = 0; (%e = GetWord(%tmp, %j)) != -1; %j+=2) {
			if(%e == 20)
				if(GetWord(%tmp, %j+1) == %status)
					return true;
		}
	}
	return false;
}

//======== HEADERS ==========
//

function MakeHeaders() {

	$Headers::A = "Equipped";		$HeaderShow::A = "NULL";//hardcoded -on its own list

	$Headers::B = "Weapons";		$HeaderShow::B = "-------_Weapons -------";		//Only edit headers B - Y

	$Headers::C = "Armors";			$HeaderShow::C = "--------_Armors -------";
	$Headers::D = "Shields";		$HeaderShow::D = "--------_Shields -------";	//Cheap way to show headers
	$Headers::E = "Helmets";		$HeaderShow::E = "-----_Head_Gear ------";	//The first dashes should have a min of 4
	$Headers::F = "Legs";			$HeaderShow::F = "------_Leggings -------";	//word 0 should be ----_HEADER_NAME
	$Headers::G = "Boots";			$HeaderShow::G = "--------_Boots --------";	//word 1 should always be the rest of the
	$Headers::H = "Rings";			$HeaderShow::H = "--------_Rings --------";	//dashes
	$Headers::I = "Hands";			$HeaderShow::I = "--------_Hands --------";
	$Headers::J = "Talisman";		$HeaderShow::J = "-------_Talisman -------";

	$Headers::K = "";				$HeaderShow::K = "---- ----";
	$Headers::L = "";				$HeaderShow::L = "---- ----";

	$Headers::M = "Consumables";	$HeaderShow::M = "-------_Consumables -------";
	$Headers::N = "Cure Potions";	$HeaderShow::N = "-----_Cure_Potions -----";
	$Headers::O = "Food";			$HeaderShow::O = "--------_Foods --------";
	$Headers::P = "Status Potions";	$HeaderShow::P = "-----_Status_Potions ----";
	$Headers::Q = "Arrows";		$HeaderShow::Q = "--------_Arrows -------";
	$Headers::R = "Miscellany";		$HeaderShow::R = "------_Miscellany -------";
	$Headers::S = "";				$HeaderShow::S = "---- ----";
	$Headers::T = "Alcohol";		$HeaderShow::T = "-------_Alcohol -------";
	$Headers::U = "Gems";			$HeaderShow::U = "--------_Gems --------";
	$Headers::V = "";				$HeaderShow::V = "---- ----";
	$Headers::W = "Battle Spoils";	$HeaderShow::W = "-----_Battle_Spoils -----";
	$Headers::X = "Material";		$HeaderShow::X = "------_Materials ------";
	$Headers::Y = "Runes";			$HeaderShow::Y = "-------_Runes -------";

	$Headers::Z = "Quest Items";	$HeaderShow::Z = "NULL"; //hardcoded -on its own gui

	//Now check what Headers != "" && HeaderShow != NULL
	$HeadersMax = 0;
	$Headers::ABC = "";
	%abc = "abcdefghijklmnopqrstuvwxyz";
	for(%i = 0; %i <= 25; %i++) {
		%char = String::getSubStr(%abc, %i, 1);
		if($Headers::[%char] != "" && $HeaderShow::[%char] != "NULL") {
			$Headers::ABC = $Headers::ABC@%char;
			$HeadersMax++;
		}
	}
																		//a = Equip header z = Quest header
	echo(">> Active Headers: "@$HeadersMax@"  Active Letters: 'a"@$Headers::ABC@"z' ");
}

MakeHeaders();

function GiveClientHeadersData(%Client) {	//Gives Client the servers Active Headers, so if
												// you added new ones or edited old ones the
												// client will see them.
	for(%i = 0; %i <= $HeadersMax; %i++) {
		%char = String::getSubStr($Headers::ABC, %i, 1);
		remoteEval(%Client, "SetHeadersData", $Headers::[%char], $HeaderShow::[%char], %char);
	}
}

function getHeaderByType(%type) {

	if(%type == $RingAccessoryType)
		return $Headers::H;
	else if(%type == $BodyAccessoryType)
		return $Headers::C;
	else if(%type == $BootsAccessoryType)
		return $Headers::G;
	else if(%type == $ShieldAccessoryType)
		return $Headers::D;
	else if(%type == $TalismanAccessoryType)
		return $Headers::J;
	else if(%type == $SwordAccessoryType || %type == $AxeAccessoryType || %type == $PolearmAccessoryType || %type == $BludgeonAccessoryType || %type == $RangedAccessoryType)
		return $Headers::B;
	else if(%type == $ProjectileAccessoryType)
		return $Headers::Q;
	else if(%type == $HeadAccessoryType)
		return $Headers::E;
	else if(%type == $HandsAccessoryType)
		return $Headers::I;
	else if(%type == $LegsAccessoryType)
		return $Headers::F;
	else if(%type == $OrbAccessoryType)
		return $Headers::R;
}

//
//===========================

function logItemError(%error, %name, %dataname, %svar, %type, %w, %info, %header, %shape, %className, %Equip, %grp, %dmgtype, %range, %delay, %sound, %activatesound) {
	$ItemDataErrorCount++;
	$ItemErrorLog::Item[$ItemDataErrorCount] = %error@" - "@%name@" "@%dataname@" '"@%svar@"' '"@%type@"' '"@%w@"' '"@%info@"' '"@%header@"' '"@%shape@"' '"@%className@"' "@%Equip@" "@%grp@" "@%dmgtype@" "@%range@" "@%delay@" "@%sound@" "@%activatesound@"##";
}

function Itemlog() {
	%i = 1;
	%errors = $ItemDataErrorCount;

	echo("");
	echo("Total errors reported: "@%errors);
	echo("");
	while(%i <= %errors) {
		schedule("echo(\" "@%i@") "@$ItemErrorLog::Item[%i]@"\");", %i);
		%i++;
	}
	schedule("echo(\"\");", %i);
	schedule("echo(\"Itemlog finished. ("@%errors@" errors reported)\");", %i++);
	schedule("echo(\"\");", %i++);
}

function ExportItemlog(%file) {

	if(%file == "")
		%path = "temp\\ItemLog.cs";
	else
		%path = "temp\\"@%file@".cs";

	export("$ItemErrorLog::*", %path);
}

function ExportItemData(%file) {
	if(%file == "")
		%path = "temp\\ItemData.cs";
	else
		%path = "temp\\"@%file@".cs";

	export("$ItemData*", %path);
}

function ClearItemlog() {
	deleteVariables("ItemErrorLog::*");
}

function ClearItemData() {

	%tmp = $ItemDataOverWriteProtect;

	deleteVariables("ItemData*");

	while(%i <= 1000) { %i++; } // small delay

	$ItemDataOverWriteProtect = %tmp;
}

function ReIntItems(%bool) {

	echo("||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
	echo("");
	echo(">> -------------------------------------------");
	echo(">>  Re-initializing Items...");
	echo(">> -------------------------------------------");
	echo("");
	if(%bool) {
		echo(">> Exec Accessory.cs and AccessoryData.cs");
		exec("Accessory.cs");
		exec("AccessoryData.cs");
		echo("");
	}
	echo(">> Focusing server...");
	if(!focusServer()) {
		echo("Error: No server found.");
		return;
	}
	echo("");

	schedule("ReInitializingItems();", 0.5);
}

function ReInitializingItems() {

	ClearItemlog();
	ClearItemData();

	if($ItemDataOverWriteProtect == "") $ItemDataOverWriteProtect = true;
	if($ItemDataCounter == "" || $ItemDataOverWriteProtect == false) {
		$ItemDataCounter = 0;
		$ItemDataErrorCount = 0;
	}
	if($CheckFuncCnt == "") $CheckFuncCnt = 0;

//	$ItemData["", FixCaps] = "This will fix some stuff."; // So I don't have to slow down the functions by checking if the item string has some data.
//	$ItemData["", DataName] = "This will fix some stuff.";
//	$ItemData["", Name] = "This will fix some stuff.";
	_SetUpItems();

	echo("");
	echo(">> -------------------------------------------");
	echo(">>  Re-initializing Items done!");
	echo(">> -------------------------------------------");
	echo("");
}

$minRange = 2;
$minSpellRange = 4;
$maxSpellRange = 500;

if($ItemDataOverWriteProtect == "") $ItemDataOverWriteProtect = true;

if($ItemDataCounter == "" || $ItemDataOverWriteProtect == false) {
	$ItemDataCounter = 0;
	$ItemDataErrorCount = 0;
	ClearItemData();
	ClearItemlog();
}

if($CheckFuncCnt == "") $CheckFuncCnt = 0;

function RemoveThisFromList(%char, %list, %replace) {
	if(%replace == "")
		%replace = " ";
	if(%char == %replace || %char == "")
		return %list;
	while(String::findSubStr(%list, %char) != -1) {
		%list = String::replace(%list, %char, %replace);
	}
	return %list;
}

//$ItemData["", FixCaps] = "This will fix some stuff."; // So I don't have to slow down the functions by checking if the item string has some data.
//$ItemData["", DataName] = "This will fix some stuff.";
//$ItemData["", Name] = "This will fix some stuff.";

//	MakeItem("Rune Red","Rune Red","NULL","NULL",0.5,"A Red Rune.",$Headers::Y, "NULL", "Runes");
function MakeItem(%name, %dataname, %svar, %type, %w, %info, %header, %shape, %className, %Equip, %grp, %dmgtype, %range, %delay, %sound, %activatesound) {

	if($ItemData[%dataname, Name] != "" && $ItemDataOverWriteProtect) {
		echo("Error: Item "@%dataname@" already declared.  >>>> -WriteProtection-");

		logItemError("DECLARED", %name, %dataname, %svar, %type, %w, %info, %header, %shape, %className, %Equip, %grp, %dmgtype, %range, %delay, %sound, %activatesound);

		return "declared";
	}
	if(%name == "" || %dataname == "" || %svar == "" || %header == "" || %className == "") {
		echo("Error: Item "@%name@" "@%dataname@"  >>>> -undefined-");

		logItemError("UNDEFINED", %name, %dataname, %svar, %type, %w, %info, %header, %shape, %className, %Equip, %grp, %dmgtype, %range, %delay, %sound, %activatesound);

		return "undefined";
	}
	if(string::findSubStr(%name, "_") != -1 || string::findSubStr(%dataname, "_") != -1) {
		echo("Error: Item "@%name@" "@%dataname@"  >>>> -illegal char in name- '_'");
		logItemError("ILLEGAL NAME", %name, %dataname, %svar, %type, %w, %info, %header, %shape, %className, %Equip, %grp, %dmgtype, %range, %delay, %sound, %activatesound);

		return "illegal";
	}

	//%Nname = RemoveThisFromList("_", %dataname, " ");
	%Nname = %dataname;
	%dataname = String::ConvertSpaces(%dataname);

	$ItemData[$ItemDataCounter, Count] = %dataname;
	$ItemData[""@%Nname@"", DataName] = %dataname;
	$ItemData[%dataname, FixCaps] = %dataname; //Used on Client::addItemCount to insure that the Item Has the Right Caps!
	$ItemData[%dataname, Name] = %name;

	$ItemData[%dataname, svar] = %svar;
	$ItemData[%dataname, type] = %type;
	$ItemData[%dataname, weight] = %w;
	$ItemData[%dataname, info] = %info;
	$ItemData[%dataname, header] = %header;
	$ItemData[%dataname, shape] = %shape;
	$ItemData[%dataname, className] = %className;
	if(%Equip == "")
		%Equip = false;
	$ItemData[%dataname, Equip] = %Equip;//Skin or true if armor, else false
	$ItemData[%dataname, ToUseSkill] = "";
	//$HeaderItemList::[%header] = $HeaderItemList::[%header]@%dataname@" ";

	//WEAPONS
	if(%className == Weapon) {
		$ItemData[%dataname, DamageType] = %dmgtype;
		if(%sound == "")//Make sure we have some important info, if not add default ones
			%sound = "SoundSwing1";
		if(%activatesound == "")
			%activatesound = "AxeSlash2";
		if(%range == "")
			%range = 2;
		if(%delay == "")
			%delay = 2;
		$ItemData[%dataname, Sound] = %sound;
		$ItemData[%dataname, ASound] = %activatesound;
		$ItemData[%dataname, Range] = $minRange + %range;
		$ItemData[%dataname, Delay] = %delay;
		$ItemData[%dataname, ToUseSkill] = %dataname.genSkill();
		if(%grp == "NULL")
			$ItemData[%dataname, NoDrop] = true;
		if(%type == $RangedAccessoryType) {
			$ItemData[%dataname, svar] = "NULL";						//------------------------------------------------------
			%svar = "NULL";												//  --WEAPON MODEL NOTES--
		}																	//
		%fixshape = String::replace(%shape@%delay, ".", "");			//MODELS WILL USE DELAY AS PART OF THE SHAPE NAME
		if(%grp == "NULL")												//BUT WILL REPLACE THE . WITH ""
			%fixshape = "Bot"@%fixshape;								//BECAUSE . WILL CAUSE ERRORS
		$ItemData[%dataname, shape] = %fixshape;					//IF GROUPRESTRICTIONS == NULL
		if($CheckFunc[%fixshape] != "Loaded") { 						//IT WILL APPEND Bot TO THE NAME
			if($CheckFuncCnt >= 200) {									//------------------------------------------------------
				echo("Warning to many Model Shapes (MAX 200) Tried to add Shape '"@%fixshape@"'");
				return;
			}

			%tmp0 = "b"@%header; %tmp1 = %shape@"Model";//  /me loves Eval();
			%string = "ItemImageData "@%fixshape@"Image { shapeFile = "@%shape@"; mountPoint = 0; weaponType = 0; reloadTime = 0; fireTime = "@%delay@"; minEnergy = 0; maxEnergy = 0; accuFire = true; sfxFire = NoSound; sfxActivate = NoSound; };"; //echo(%string);
			Eval(%string);

			%string = "ItemData "@%fixshape@" { heading = "@%tmp0@"; description = "@%tmp1@"; className = "@%className@"; shapeFile  = "@%shape@"; hudIcon = blaster; shadowDetailMask = 4; imageType = "@%fixshape@"Image; price = 0; showWeaponBar = false; };"; //echo(%string);
			Eval(%string);
	//		%string = "function "@%fixshape@"Image::onFire(%player, %slot) { %cl = Player::getClient(%player); if(CheckCanFire(%cl)) MeleeAttack(%player, $ItemData[$ClientData[%cl, UsingWeapon], Range], $ClientData[%cl, UsingWeapon]); }"; //echo(%string);

			if(%svar == "NULL") {//Only bows should have NULL svar
				%string = "function "@%fixshape@"Image::onFire(%player, %slot) { %cl = Player::getClient(%player); ProjectileAttack(%player, $ClientData[%cl, UsingWeapon]); }"; //echo(%string);
				$ItemData[%dataname, Ammo] = %ammo;
			}
			else
				%string = "function "@%fixshape@"Image::onFire(%player, %slot) { %cl = Player::getClient(%player); MeleeAttack(%player, $ItemData[$ClientData[%cl, UsingWeapon], Range], $ClientData[%cl, UsingWeapon]); }"; //echo(%string);
			Eval(%string);

			if($ShowMakeModel) Echo("Model Shape Added: Shape:'"@%fixshape@"' Type:Weapon");

			$CheckFunc[%fixshape] = "Loaded";
			$CheckFuncCnt++;
		}
		//
//		$GroupRestrictions[%dataname] = %grp;
	}
	//SHIELDS
	else if(%type == $ShieldAccessoryType) {
		if($CheckFunc[%shape] != "Loaded") {
			if($CheckFuncCnt >= 200) {
				echo("Warning to many Model Shapes (MAX 200) Tried to add Shape '"@%shape@"'!");
				return;
			}
			%string = "ItemImageData "@%shape@"Image { shapeFile = "@%shape@"; mountPoint = 2; mountOffset = {0.18, -0.1, -0.2}; mountRotation = {0, 0, 0.5}; };";
			Eval(%string);
			%string = "ItemData "@%shape@" { description = Shield; className = "@%className@"; shapeFile = "@%shape@"; imageType = "@%shape@"Image; heading = eMiscellany; price = 0; };";
			Eval(%string);																									//{-left +right,+forward -backward,+up,-down}//{+up -down, rolling, +left -right}
			%string = "ItemImageData "@%shape@"OnBackImage { shapeFile = "@%shape@"; mountPoint = 2; mountOffset = {-0.09, 0.7, -0.2}; mountRotation = {0.1, 0, 3.1}; };";
			Eval(%string);
			%string = "ItemData "@%shape@"OnBack { description = Shield; className = "@%className@"; shapeFile = "@%shape@"; imageType = "@%shape@"OnBackImage; heading = eMiscellany; price = 0; };";
			Eval(%string);
			if($ShowMakeModel) Echo("Model Shape Added: Shape:'"@%shape@"' Type:Shield");
			$CheckFunc[%shape] = "Loaded";
			$CheckFuncCnt++;
		}
	}
	//PROJECTILES
	else if(%className == "Projectile") {
		$ItemData[%dataname, DamageType] = $ProjectileDamageType;
		if($CheckFunc[%shape] != "Loaded") {
			if($CheckFuncCnt >= 200) {
				echo("Warning to many Model Shapes (MAX 200) Tried to add Shape '"@%shape@"'!");
				return;
			}
			%string = "ItemData "@%shape@" { description = "@%shape@"; className = Projectile; shapeFile = "@%shape@"; heading = xAmmunition; shadowDetailMask = 4; price = 0; };";
			Eval(%string);
			if($ShowMakeModel) Echo("Model Shape Added: Shape:'"@%shape@"' Type:Projectile");
			$CheckFunc[%shape] = "Loaded";
			$CheckFuncCnt++;
		}
	}

	$ItemCost[%dataname] = %dataname.genCost(); // hardcoded genCost
	if($ShowMakeItem) echo("Item added["@$ItemDataCounter@"]: Name:"@%name@" Dataname:"@%dataname@" SVAR:'"@%svar@"' Type:"@%type@" W:"@%w@" Info:'"@%info@"' Header:"@%header@" shape:"@%shape@" class:"@%className@" Equip:"@%Equip@" GRP:'"@%grp@"'");
	$ItemDataCounter++;

	//EQUIPPED
	if(%Equip != false) { //%Equip != false  means its trying to make an armor!
		$ArmorHitSound[%dataname] = %Equip.SetArmorHitSound();
		$ItemData[%dataname, ToUseSkill] = %dataname.genSkill();
//		$GroupRestrictions[%dataname] = %grp;
		%dataname = %dataname@$EquipTag;
		$ItemData[$ItemDataCounter, Count] = %dataname;
		$ItemData[""@%Nname@$EquipTag, DataName] = %dataname;
		$ItemData[%dataname, FixCaps] = %dataname;
		$ItemData[%dataname, Name] = %name;
		$ItemData[%dataname, svar] = %svar;
		$ItemData[%dataname, type] = %type;
		$ItemData[%dataname, weight] = %w;
		$ItemData[%dataname, info] = "Equipped State.";
		$ItemData[%dataname, header] = $Headers::A; //Equipped Header
		$ItemData[%dataname, shape] = %shape;
		$ItemData[%dataname, Equip] = %Equip;
		$ItemData[%dataname, className] = "Equipped";
		if($ShowMakeItem) echo("Item added["@$ItemDataCounter@"]: Armor- Dataname:"@%dataname@" SVAR:'"@%svar@"' Type:"@%type@" class:"@%className@" Equip:"@%Equip@" GRP:'"@%grp@"'");
		$ItemDataCounter++;
	}
}


function CheckCanFire(%Client) { //New weapon fire will give the client the delay needed so they will not flood, unless you edit it.
									//if we use it....
	%delay = $ItemData[$ClientData[%Client, UsingWeapon], Delay];

	if(%Client.FireFlood >= 5)
		Client::SendMessage(%Client, 1, "FIRE FLOOD WARNING! Keep it up and you'll be kicked!");
	else if(%Client.FireFlood >= 10) { //die!
		exec("[RM]FireFloodLog.cs");
		%time = GetSimTime();
		$FIREFLOODERS::[Client::getName(%Client), $Server::StartCounter, %time] = "SimTime: "@%time;
		File::delete("temp\\[RM]FireFloodLog.cs");
		export("FIREFLOODERS::*", "temp\\[RM]FireFloodLog.cs");
		$FIREFLOODERS::[Client::getName(%Client), $Server::StartCounter, %time] =  "";
		NET::KICK(%Client, "FIRE FLOODED. You flooded with weapon fire!\nIf you did not mean to and don't know how to fix this,\n READ THE 'RM READ ME.txt'!");
		return false;
	}
	%time =  getIntegerTime(true) >> 5;
	if((%time - %Client.lastCheckFireTime) >= %delay) {
		%Client.lastCheckFireTime = %time;
		%Client.FireFlood = "";
		return true;
	}
	else
		%Client.FireFlood++;

	return false;
}

function NEWgetNumItems(%echo) { if(%echo) echo($ItemDataCounter); return $ItemDataCounter; }
function NEWgetItemData(%n, %echo) {
	if(IsNum(%n)) {
		%data = $ItemData[%n, Count];
		if($ItemData[%data, className] != "") {
			if(%echo) echo($ItemData[%data, className]);
			return $ItemData[%data, className];
		}
	}
	else {
		if($ItemData[%n, className] != "") {
			if(%echo) echo($ItemData[%n, className]);
			return $ItemData[%n, className];
		}
		%data = $ItemData[%n, DataName];
		if(%data != "")
			return $ItemData[%data, className];
	}
	if(%echo) echo("-1");
	return -1;
}
function NEWgetItemName(%n, %echo) {

	if(IsNum(%n)) {
		if(%echo) echo($ItemData[%n, Count]);
		return $ItemData[%n, Count];
	}
	if(%echo) echo("-1");
	return -1;
}
function NEWgetItemType(%name, %equip, %echo) {

	if($ItemData[%name, DataName] != "") {
		if(%equip)
			%data = $ItemData[%name, DataName];
		else
			%data = getCroppedItem($ItemData[%name, DataName]);
		if(%echo) echo(%data);
		return %data;
	}
	if(%echo) echo("-1");
	return -1;
}

function getClassNameByType(%type) {

	if(%type == $RingAccessoryType || %type == $BodyAccessoryType || %type == $BootsAccessoryType || %type == $ShieldAccessoryType || %type == $TalismanAccessoryType || %type == $HeadAccessoryType || %type == $HandsAccessoryType || %type == $LegsAccessoryType || %type == $OrbAccessoryType)
		return "Accessory";
	else if(%type == $SwordAccessoryType || %type == $AxeAccessoryType || %type == $PolearmAccessoryType || %type == $BludgeonAccessoryType || %type == $RangedAccessoryType)
		return "Weapon";
	else if(%type == $ProjectileAccessoryType)
		return "Projectile";
}

function IsNum(%list) {
	%i = 0;
	%len = String::len(%list);
	while( %len > 0 ) {
		%char = String::NEWgetSubStr(%list, %i, 1); //echo(%char);
		if(%char == '1' || %char == '2' || %char == '3' || %char == '4' || %char == '5' || %char == '6' || %char == '7' || %char == '8' || %char == '9' || %char == '0' || %char == '.')
			%IsNum = true;
		else
			return false;
		%len--;
		%i++;
	}
	return %IsNum;
}

function IsNumW(%list) {
	%i = 0;
	%len = String::len(%list);
	while( %len > 0 ) {
		%char = String::NEWgetSubStr(%list, %i, 1); //echo(%char);
		if(%char == '1' || %char == '2' || %char == '3' || %char == '4' || %char == '5' || %char == '6' || %char == '7' || %char == '8' || %char == '9' || %char == '0')
			%IsNum = true;
		else
			return false;
		%len--;
		%i++;
	}
	return %IsNum;
}

$genCost[1] = 175;	//STR
$genCost[2] = 175;	//DEX
$genCost[3] = 175;	//CON
$genCost[4] = 175;	//INT
$genCost[5] = 175;	//WIS
$genCost[6] = 25;	//ATK
$genCost[7] = 100;	//DEF
$genCost[8] = 5000;	//Internal armor switching variable
$genCost[9] = 100;	//STA regen
$genCost[10] = 120;	//HP regen
$genCost[11] = 120;	//Mana regen
$genCost[12] = 75;	//STA
$genCost[13] = 15;	//HP
$genCost[14] = 25;	//MP
$genCost[15] = 150;	//MDEF

$genCost[HEAL] = 0.5;//potions
$genCost[MP] = 4;
$genCost[STA] = 2;

$genCost[20] = 50000;
$genCost[21] = 1000;

$genCost[30] = 100;	//Poison
$genCost[31] = 110;	//Blind
$genCost[32] = 200;	//Mute
$genCost[33] = 500;	//Petrify

$genCost[AREA] = 10000;

$genCost[EXPB] = 105000;

$genCost[Alvl] = 5;

$genCost[MaxCOINS] = 0.01;

$genCost[SMITH] = 3;


function dot_op_genCost(%dataname) {

	%data = $ItemData[%dataname, svar];

	if($ItemData[%dataname, type] == $RingAccessoryType)
		%mult = 2.5; //Rings cost more
	else
		%mult = 1;
	%class = $ItemData[%dataname, className];
	if(%class == "Weapon") {

		if($ItemData[%data, Delay] >= 10)
			$ItemData[%data, Delay] = 9.9;

		%wf = floor( pow((10-$ItemData[%data, Delay]), 2));

		if($ItemData[%dataname, type] != $RangedAccessoryType)
			%wf += floor(pow($ItemData[%dataname, Range], 4));
		else
			%wf += $ItemData[%dataname, Range];
	}
	else if(%class == "Accessory") {
		%isArmor = True;
	}
	for(%i = 0; (%w = getWord(%data, %i)) != -1; %i++) {
		%i++;
		%f = 1;

		if((%w2 = getWord(%data, %i)) < 0)
			%f = 3;//Item has neg on a bonus, do not neg the cost full for that bonus

		else if(floor(%w2) == 0)
			%w2 = 1;

		if(%w2 > 0)
			%w2 = floor(pow(%w2, 1.7));//2

		%cost += floor( ((floor($genCost[%w]/%f) * %w2) * %mult) ); //+ floor( pow(%w2, 2.5)) );
	}

	if(%class == "Projectile")
		%cost = floor(%cost / 20);

	%cost  = (%cost) + pow(%i, 4);

	if(%cost <= 0)
		%cost = 1;//Make sure the item doesn't not have a neg cost!...

	return %cost+%wf;
}

$NumToSkill[1] = "STR";
$NumToSkill[2] = "DEX";
$NumToSkill[3] = "CON";
$NumToSkill[4] = "INT";
$NumToSkill[5] = "WIS";
$NumToSkill[12] = "STA";

$SkillToNum[STR] = 1;
$SkillToNum[DEX] = 2;
$SkillToNum[CON] = 3;
$SkillToNum[INT] = 4;
$SkillToNum[WIS] = 5;
$SkillToNum[STA] = 12;

//6: ATK
$genSkillFactorsToSTR[6] = 0.8;//6
$genSkillFactorsToDEX[6] = 0.4;//2.5
$genSkillFactorsToSTA[6] = 0.45;//2

//7: DEF
$genSkillFactorsToSTR[7] = 1.5;//1.2
$genSkillFactorsToDEX[7] = 0.85;//0.75
$genSkillFactorsToCON[7] = 0.7;//0.6

//15: MDef
$genSkillFactorsToINT[15] = 0.9;
$genSkillFactorsToWIS[15] = 1.2;

//9: STA regen
$genSkillFactorsToSTA[9] = 0.3;

//10: HP regen
$genSkillFactorsToCON[10] = 0.3;

//11: Mana regen
$genSkillFactorsToINT[11] = 0.3;


function dot_op_genSkill(%dataname) {

	//===========================================================================
	//WEIGHT
	%w = $ItemData[%dataname, Weight];
	%STRSkill = FixDecimals(0.02 * %w);//0.05
//	%STASkill = FixDecimals(0.05 * %w);
	//===========================================================================
	%svar = $ItemData[%dataname, svar];
	//===========================================================================
	//STR DEX CON INT WIS STA
	for(%i = 0; (%skill = getWord(%svar, %i)) != -1; %i++) { %i++;
		%amount = getWord(%svar, %i);
		if(%amount > 0)
			%amount = floor(pow(%amount, 1.15));// 1.25  1.3
		%STRSkill += ($genSkillFactorsToSTR[%skill] * %amount);
		%DEXSkill += ($genSkillFactorsToDEX[%skill] * %amount);
		%CONSkill += ($genSkillFactorsToCON[%skill] * %amount);
		%INTSkill += ($genSkillFactorsToINT[%skill] * %amount);
		%WISSkill += ($genSkillFactorsToWIS[%skill] * %amount);
		%STASkill += ($genSkillFactorsToSTA[%skill] * %amount);
	}
	//===========================================================================
	%STRSkill = Cap(floor(%STRSkill), -$MaxAPStats, $MaxAPStats);
	%DEXSkill = Cap(floor(%DEXSkill), -$MaxAPStats, $MaxAPStats);
	%CONSkill = Cap(floor(%CONSkill), -$MaxAPStats, $MaxAPStats);
	%INTSkill = Cap(floor(%INTSkill), -$MaxAPStats, $MaxAPStats);
	%WISSkill = Cap(floor(%WISSkill), -$MaxAPStats, $MaxAPStats);
	%STASkill = Cap(floor(%STASkill), -$MaxAPStats, $MaxAPStats);
	%tmp = "";
	if(%STRSkill > 0)
		%tmp = "STR "@%STRSkill@" ";
	if(%DEXSkill > 0)
		%tmp = %tmp@"DEX "@%DEXSkill@" ";
	if(%CONSkill > 0)
		%tmp = %tmp@"CON "@%CONSkill@" ";
	if(%INTSkill > 0)
		%tmp = %tmp@"INT "@%INTSkill@" ";
	if(%WISSkill > 0)
		%tmp = %tmp@"WIS "@%WISSkill@" ";
	if(%STASkill > 0)
		%tmp = %tmp@"STA "@%STASkill@" ";

	return %tmp;
}

function dot_op_SetArmorHitSound(%skin) {

	if(%skin == true)
		return;

	%leather =	",rpgpadded,rpgleather,rpgstudleather,rpgspiked,rpghide,rpghuman1,RMNightWarrior,RMNinjaGear,";
	%chain =		",rpgscalemail,rpgbrigandine,rpgchainmail,rpgringmail,rpgbandedmail,rpgsplintmail,rpghuman2,rpghuman6,RMColdScale,RMCrystalArmor,RMJerkilin,RMLightning,RMStone,RMVlacx,";
	%plate =		",rpgbronzeplate,rpgplatemail,rpgfieldplate,rpgfullplate,rpghuman3,rpghuman4,rpghuman7,rpghuman8,rpghuman9,rpghuman10,RMDarkPlate,RMFireMail,RMGaiaGear,RMJerkilinPlate,RMZionSteel,";

	if(String::findSubStr(%leather, ","@%skin@",") != -1)
		return SoundHitLeather;
	else if(String::findSubStr(%chain, ","@%skin@",") != -1)
		return SoundHitChain;
	else if(String::findSubStr(%plate, ","@%skin@",") != -1)
		return SoundHitPlate;
	else
		return SoundHitFlesh;//Robes etc
}