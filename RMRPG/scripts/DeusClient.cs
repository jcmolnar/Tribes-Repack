//Deus_ex_Machina
//
//	DeusClient ver 178


/////////////////////////////////////////////////////////////
//  USER OPTIONS

$SimTextMaxBuffer = 40;	//Saves 40 lines of text you sent
								//to the server. Add more if you
								//want too.

$StatsTextDelay = 9;		//Stats GUI pop delay (in-game: tab -> 1)

$StatsIsTransparent = 1;	//Set to 1 to make the Stats menu Transparent (in-game: tab -> 1)

$ShowAttachedCmd = 0;		//Set to 0 to not echo attached items

$RM::MuteMusic = false;		//Set to true to mute zone music

/////////////////////////////////////////////////////////////
//Neat if you want to exec or call a function when you joined (spawn)
//etc

$RM::LoadAttachCmds[$RM::AttachCount++] = "RM::CustomAttach"; // Your function name to be loaded
function RM::CustomAttach() { // <--

	//CALL ON JOIN
	//OnJoinedTeam.AttachCmd("Saying Hi", 0, "say(0, '%1');", "#group Sup");

	//OnJoinedTeam.AttachCmd("Admin me", "schedule 1", "say(%1, '%2%3 %4');", "0", "3admin dont you ", "wish you", "knew?"); //goes up to %8

	//$MyVar = 5;
	//OnJoinedTeam.AttachCmd("Schedule echo", "schedule 5", "echo('>> %1'); echo('>> $MyVar -> %2'); echo(' %3');", "This is how you can schedule this call", $MyVar, "=P");

	//OnJoinTeam.AttachCmd("exec myFile", 0, "exec('%1');", "myFile.cs");
	//OnJoinTeam.AttachCmd("load myQuest", "schedule 1", "MyQuest();");

	//OnJoinTeam.AttachCmd("SetWidth", "schedule 2", "Chathud::SetWidth(%1);", "370");

	OnJoinedTeam.AttachCmd("setWindowTitle", 0, "setWindowTitle(MainWindow, '%1');", "Red Moon RPG");

	//CALL ON DEATH
	//$MyChatType = "#global";
	//OnDeath.AttachCmd("My Death Msg", 0, "say(%1, '%2 %3');", "0", $MyChatType, "*Core overload* Machine melt down!");
}


//  END USER OPTIONS
/////////////////////////////////////////////////////////////
//
//
//
//
//
//
//
//======================================
//WARNING!
//Client-side RPG scripts!
//
//DO NOT EDIT or your status or any other
//client-side stuff won't work/show up right!
//(like remote damage text!)
//======================================
//
//
//
//

//$SimGame::TimeScale = 1.0;

function RM::loadAttachCmds() {

	ClearAllCmds();

	OnJoinedTeam.AttachCmd("RM SetUpClient - Do not remove", 0, "SetUpClient();");

	//Load $RM::LoadAttachCmds
	//That way clients can put some in their autoexec.cs
	for(%i = 1; $RM::LoadAttachCmds[%i] != ""; %i++) {
		Eval($RM::LoadAttachCmds[%i]@"();");
	}
}

function SetString(%chars, %num) {

	for(%i = 0; %i < %num; %i++)
	%string = %string @ %chars;

	return %string;
}

function Cap(%n, %lb, %ub) {

	if(%lb != "inf") {
		if(%n < %lb)
			%n = %lb;
	}
	if(%ub != "inf") {
		if(%n > %ub)
			%n = %ub;
	}
	return %n;
}

function GetWordCount(%list) {
	if(%list == "")
		return -1;

	for(%i = 0; GetWord(%list, %i) != -1; %i++) {}

	return %i-1;
}

function GetWords(%list, %start, %end, %debug) {
	if(%list == "" || %start == "" || %end == "") {
		echo("GetWords(words, start, end); --- input was: GetWords("@%list@", "@%start@", "@%end@", "@%debug@"); ---");
		return;
	}
	for(%i = floor(%start); %i <= floor(%end); %i++)
		%words = %words @ GetWord(%list, %i)@" ";

	return String::NEWgetSubStr(%words, 0, String::len(%words)-1);
}

function LoadGui(%gui) {
	GuiLoadContentCtrl(MainWindow, "gui\\"@%gui@".gui");
}

//if(!isObject("CmdInventoryGui"))
//	loadObject("CmdInventoryGui", "gui\\CmdInventory.gui");

//Set-up default stuff for client
for(%i = 1; %i < 6; %i++)
	$TextInUse[%i] = "";
$ZoneTextInUse = "";
$QuestTextInUse = "";
for(%i = 0; %i <= 100; %i++)
	$spacer[%i] = SetString(" ", %i);

function Chathud::SetXY(%x) { //, %y) { // can't change Y pos...

	%x = floor(%x);
	//%y = floor(%y);

	if(%x < 0) {// || %y < 0) {
		echo("Chathud::SetXY(%x)");//, %y)");
		return 0;
	}
	echo(">> If you need to change your Y pos, Press k (Tribes default key) and click & drag your ChatDisplay Hud."); //bindAction(keyboard0, make, "k", TO, IDACTION_MENU_PAGE, 2.000000);
	Control::SetPosition("chatDisplayHud", %x, 0);
}

$RM::LastWidth = 0;
function Chathud::SetWidth(%w) {

	if(%w == last)
		%w = $RM::LastWidth;

	%w = floor(%w);

	if(%w <= 0) {
		echo("Chathud::SetWidth(%width)");
		return;
	}

	%size = Control::getExtent("ChatDisplayHud");

	$RM::LastWidth = GetWord(%size, 0);

	Control::SetExtent("ChatDisplayHud", %w, GetWord(%size, 1));
}

//good if you need a screen shot
function Chathud::Toggle() {
	Control::setVisible("chatDisplayHud", !Control::getVisible("chatDisplayHud"));
}

function remoteFlushTexture(%manager) {
	if(%manager == 2048)
		flushTextureCache();
}

//remoteEval(%clientId,"ATKText", %text);
function remoteATKText(%manager,%text, %hit) { //bounce up and down text
	if(%manager == 2048) {
		for(%num = 1; %num < 6; %num++) { //Protection from getting too many ATKText pop-ups (only put 5 in GUI for that)
			if($TextInUse[%num] == "") {
				$TextInUse[%num] = true;
				break;
			}
		}

		if($TextInUse[%num] == "") //Flooded, return
			return;

		Control::setValue("ATKText"@%num, %text);

		if(%hit) { //Attacked

			Control::setPosition("ATKText"@%num, 0, 107);
			%x = 0;
			%y = 107;
			for(%i = 1; %i <= 10; %i++) {//%i <= 5
				%newY = %y + (5 * -%i);
				schedule("Control::setPosition(\"ATKText"@%num@"\", "@%x@", "@%newY@");", %i / 30); //%i / 15
			}
			%cnt = 0;
			for(%i = 9; %i >= 1; %i--) {//%i = 4
				%newY = %y + (5 * -%i);
				schedule("Control::setPosition(\"ATKText"@%num@"\", "@%x@", "@%newY@");", (%cnt++ / 20) + "0.5");
			}

			schedule("Control::setPosition(\"ATKText"@%num@"\", 0, 107);", (%cnt++ / 20) + "0.5"); //"0.8");
			Schedule("PopATKText("@%num@");", 2.5);
		}

		else if(!%hit) { //Took Damage

			Control::setValue("ATKText"@%num, "<f1>"@%text);

			Control::setPosition("ATKText"@%num, 0, 130);
			%x = 0;
			%y = 130;
			for(%i = 1; %i <= 10; %i++) {//%i <= 5
				%newY = %y + (5 * -%i);
				schedule("Control::setPosition(\"ATKText"@%num@"\", "@%x@", "@%newY@");", %i / 30);// %i / 15
			}
			%cnt = 0;
			for(%i = 9; %i >= 1; %i--) {//%i = 4
				%newY = %y + (5 * -%i);
				schedule("Control::setPosition(\"ATKText"@%num@"\", "@%x@", "@%newY@");", (%cnt++ / 20) + "0.5");
			}
			schedule("Control::setPosition(\"ATKText"@%num@"\", 0, 130);", (%cnt++ / 15) + "0.5");

			Schedule("PopATKText("@%num@");", 2.5);
		}

		else if(%hit == wait) { //Intro ;]
			Control::setPosition("ATKText"@%num, 0, 90);
			Schedule("PopATKText("@%num@");", 8);
		}
		else
			Schedule("PopATKText("@%num@");", 2.5);
	}
}

function PopATKText(%num) {

	$TextInUse[%num] = "";
	Control::setValue("ATKText"@%num, "");
	Control::setPosition("ATKText"@%num, 0, 107);
}

function remoteZONEText(%manager, %text) { //side-scrolling text
	if(%manager == 2048) {
		if($ZoneTextInUse == true)
			return;
		$ZoneTextInUse = true;
		$Zonechar = %text; //Array
		%cnt = 0;
		%len = String::len($Zonechar);
		if(%len > 100) {
			echo("Error: Zone text is higher than 100 chars! ("@%len@") This is for users \'protection\'");
			return;
		}
		%delay = Cap(%len / 6, 3, 10);
		%len2 = String::len($Zonechar);
		%kk = %len;
		for(%i = %len; %i >= 0; %i--) { //Moves text on the screen (from the left)
			if((%w = String::GetSubStr($Zonechar, %i, 1)) == "_")
				%txt = " "@%txt;
			else
				%txt = %w @ %txt;
			Schedule("Control::setValue(\"ZONEText\", \"<jc>"@%txt @ $spacer[%kk--]@"\");", (%cnt++ / 15));

		}
		%txt = "";
		for(%kk = 0; %kk <= %len; %kk++) { //Moves text off the screen (to the right)
			for(%ii = 0; (%w = String::GetSubStr($Zonechar, %ii, 1)) != ""; %ii++) {
				if(%w == "_")
					%txt = %txt@" ";
				else
					%txt = %txt @ %w;
			}

			%txt = $spacer[%kk] @ %txt;

			Schedule("Control::setValue(\"ZONEText\", \"<jc>"@%txt@"\");", (%kk / 15) + %delay);
			%txt = "";
			for(%iii = %len2--; %iii >= 0; %iii--) { //Remove one letter from the end
				if((%w = String::GetSubStr($Zonechar, %iii, 1)) == "_")
					%txt = " "@%txt;
				else
					%txt = %w @ %txt;
			}
			$ZoneChar = %txt;
			%txt = "";
		}
		Schedule("PopZoneText();", ((%kk / 15) + %delay + 0.1));
	}
}

function PopZoneText() {

	$ZoneTextInUse = "";
	Control::setValue("ZONEText", "");
}

function remoteStatsText(%manager, %text1, %text2, %text3, %text4, %text5) {
	if(%manager == 2048) {
		if($StatsTextOn)
			return;
		$StatsTextOn = True;

		Control::setVisible("StatsBox", 1);
		if($StatsIsTransparent)
			Control::setVisible("Stats::Trans", 1);
		else
			Control::setVisible("Stats::Black", 1);

		%pos = Control::getPosition("StatsBox");  // Have to getPosition becuase this is the "work area" we are moving, not the text thats inside.
		%x = GetWord(%pos, 0);					 // So if I don't getPosition, and make a $ver up, it'll look messed up on the player's screen thats on a different res.
		%y = GetWord(%pos, 1);
		for(%i = 0; %i <= %x+500; %i++) {
			%i += 4;
			schedule("Control::setPosition(\"StatsBox\", "@%i-500@", "@%y@");", %i/400);
		}
		schedule("Control::setPosition(\"StatsBox\", "@%x@", "@%y@");", (%i/400)+0.1); //Place it where it should really be.
		%t = SetString("<>-", 30);
		Control::setValue("STATS", "<f0>Player Stats:<jc>\n\n\n\n\n\n\n\n<f1>"@%t@"\n<f0>Red Moon RPG\n<f1>"@%t@"");
		if($StatsTextDelay > 3) {
			%delay = ($StatsTextDelay-3);
			for(%i = 0; %i <= 30; %i++) {
				%ii += 2;
				Schedule("StatsShow("@%i@", "@%ii@");", (%delay+(%i/10)));
			}
		}
		for(%i = 1; %i <= 5; %i++)
			Control::setValue("StatsText"@%i, %text[%i]);
		schedule("PopStatsText();", $StatsTextDelay);
	}
}

function StatsShow(%i, %ii) {
	%t = SetString("<>-", -%i + 31);
	%tt = SetString("<>-", -%ii + 31);
	%rm = "Red Moon RPG"; if(%i > 14) %rm = "Red Moon"; if(%i > 17) %rm = "Red"; if(%i >= 19) %rm = "";
	Control::setValue("Stats", "<f0>Player Stats:<jc>\n\n\n\n\n\n\n\n<f1>"@%t@$spacer[%i++]@"\n"@$spacer[%ii*2]@"<f0>"@%rm@"\n<f1>"@%tt@$spacer[%ii*2]);
}

function PopStatsText() {

	$StatsTextOn = "";
	Control::setValue("STATS", "");
	for(%i = 1; %i <= 5; %i++)
		Control::setValue("StatsText"@%i, "");

	Control::setVisible("StatsBox", 0);
	Control::setVisible("Stats::Trans", 0);
	Control::setVisible("Stats::Black", 0);
}

//Note to self-
// b6,4: = pos x y
// <B1,3:skull_small.bmp>

//remoteQuestText(2048, "txt");
function remoteQuestText(%manager, %quest, %done) {
	if(%manager == 2048 && !$QuestTextInUse) {

		$QuestTextInUse = true;

		setCursor(MainWindow, "Cur_Empty.bmp");
		PopActionMap("playMap.sae");
		PushActionMap("REPLYMap.sae");
		schedule("cursorOn(MainWindow);", 1);

		Control::setVisible("QuestFrame", 1);
		Control::SetValue("Quest", "<B0,0:Quest.bmp>");
		Control::SetValue("QuestText", "<jc><B0,0:Quest_"@%quest@".bmp>");
		if(%done)
			Control::SetValue("QuestDone", "<B0,0:QuestDone.bmp>");

		%pos = Control::getPosition("QuestBox");				// Have to getPosition becuase this is the "work area" we are moving, not the text thats inside.
		%x = $RM::Quest::X = GetWord(%pos, 0);				// So if I don't getPosition, and make a $ver up, it'll look messed up on the player's screen thats on a different res.
		%y = $RM::Quest::Y = GetWord(%pos, 1); //echo("X:"@$RM::Quest::X@" Y:"@$RM::Quest::Y);
		%tmpX = %x+500;
		for(%i = 0; %i <= %tmpX; %i++) {
			%i += 5;
			schedule("Control::setPosition(\"QuestBox\", "@%i-500@", "@%y@");", %i/400);
			schedule("Control::setPosition(\"QuestBox\", "@%x+%i@", "@%y@");", cap((%i/400)+6, 0, 7.6));
		}
		schedule("Control::setPosition(\"QuestBox\", \""@%x@"\", \""@%y@"\");", (%i/400)+0.1); //Place it where it should really be.
//schedule("echo(\"setPos Done\");", cap((%i/400)+6, 0, 7.6));
		schedule("PopQuestText();", 7.8);
	}
}

function PopQuestText() {

	Control::setPosition("QuestBox", $RM::Quest::X, $RM::Quest::Y);
//echo("getPos "@Control::getPosition("QuestBox"));
	Control::SetVisible("QuestFrame", 0);
	PushActionMap("playMap.sae");
	PopActionMap("REPLYMap.sae");
	$QuestTextInUse = "";

	Control::SetValue("Quest", "");
	Control::SetValue("QuestText", "");
	Control::SetValue("QuestDone", "");

	cursorOff(MainWindow);
	setCursor(MainWindow, "Cur_Arrow.bmp");
}

function remoteRefreshHPMPEXP(%manager, %HP, %MP, %STA, %EXP, %opt) { //%opt is for EXP toggle
	if(%manager == 2048) {
		if(IsObject("playGui") == true) {

			Control::setValue("NEW_HP", ($RMInv::HP = %HP));
			Control::setValue("NEW_MP", ($RMInv::MP = %MP));
			Control::setValue("stamina", ($RMInv::STA = %STA));
			if(isObject("CmdInventoryGui")) {
				Control::setValue("NEW_HP2", %HP);
				Control::setValue("NEW_MP2", %MP);
				Control::setValue("stamina2", %STA);
			}
			if(%opt == true && isObject("EXP_BAR") != true) {
				Control::setVisible("EXP_BAR", 1);
				Control::setVisible("EXP_BAR2", 1);
				Control::setValue("EXP_BAR", %EXP);
			}
			else if(%opt == true && isObject("EXP_BAR") == true)
				Control::setValue("EXP_BAR", %EXP);
			else {
				Control::setVisible("EXP_BAR", 0);
				Control::setVisible("EXP_BAR2", 0);
			}
		}
	}
}

function remoteRefreshHPset(%manager, %HP) { if(%manager == 2048) Control::setValue("NEW_HP", ($RMInv::HP = %HP)); Control::setValue("NEW_HP2", %HP); }
function remoteRefreshMPset(%manager, %MP) { if(%manager == 2048) Control::setValue("NEW_MP", ($RMInv::MP = %MP)); Control::setValue("NEW_MP2", %MP); }
function remoteRefreshSTAset(%manager, %STA) { if(%manager == 2048) Control::setValue("stamina", ($RMInv::STA =%STA)); Control::setValue("stamina2", %STA); }
function remoteRefreshEXPset(%manager, %EXP) { if(%manager == 2048) Control::setValue("EXP_BAR", %EXP); }
function remoteRefreshSTAMPset(%manager, %STA, %HP, %MP) { if(%manager == 2048) { Control::setValue("stamina", ($RMInv::STA = %STA)); Control::setValue("stamina2", %STA); Control::setValue("NEW_HP", ($RMInv::HP = %HP)); Control::setValue("NEW_HP2", %HP); Control::setValue("NEW_MP", ($RMInv::MP = %MP)); Control::setValue("NEW_MP2", %MP); } }


//=====================================================================
//NEW CLIENT INV FUNCTIONS
//Editing these is NOT recommoned!
//These ONLY affect what the client SEES
//

function remoteClearItemLists(%manager) {	//DO NOT CALL THIS FUNCTION EVER
	if(%manager == 2048) {					//THE SERVER WILL CALL IT WHEN NEEDED!!
		deleteVariables("ClientSide::Item*");
		deleteVariables("ClientSide::HeaderOrder*");
	}
}

function SaveItemBuildList() {//Do not call this func. Adds any ItemBuild
								//that the client made during this game.
								//This gets called on onExit();
	echo("Saving BuildItem...");
	export("ClientSide::BuildItem[\"*", "temp\\[RM]ItemBuildList.cs", true);
	export("ClientSide::BuildItem2[\"*", "temp\\[RM]ItemBuildList.cs", true);
	export("ClientSide::BuildItemSpacer[\"*", "temp\\[RM]ItemBuildList.cs", true);
}
function ClearItemBuildList() {
	deleteVariables("ClientSide::BuildItem*");
}
function LoadItemBuildList() {
	if(isFile("temp\\[RM]ItemBuildList.cs"))
		exec("[RM]ItemBuildList.cs");
} LoadItemBuildList();

function AddSpaces(%str, %num, %IsHeader) {		// Makes a new $ClientSide::ItemBuild
														// This way we can speed up the inv when showing the client
														// items he has seen already during this game

	if($ClientSide::BuildItem["[\""@%str@"\"]"] != "" && %num == "")
		return $ClientSide::BuildItem["[\""@%str@"\"]"]@$ClientSide::BuildItemSpacer["[\""@%str@"\"]"];
	if($ClientSide::BuildItem[%str] != "" && %num == "")
		return $ClientSide::BuildItem[%str]@$ClientSide::BuildItemSpacer[%str];

	%item = %str;

	if(%IsHeader) {
		for(%i = 0; (String::findSubStr(%str, "_") != -1); %i++)
			%str = String::replace(%str, "_", " ");
		$ClientSide::BuildItem["[\""@%item@"\"]"] = %str;
		return $ClientSide::BuildItem["[\""@%item@"\"]"];
	}
	if($ClientSide::BuildItemSpacer["[\""@%item@"\"]"] == "") {
		for(%i = 0; (String::findSubStr(%str, "_") != -1); %i++)
			%str = String::replace(%str, "_", " ");
		if(%num != "") {
			%str = String::getSubStr(%str, 0, String::len(%item)-1);//remove the 0
			$ClientSide::BuildItem["[\""@%item@"\"]"] = %str;
		//	$ClientSide::BuildItem2["[\""@%str@"\"]"] = %item;
			%len = floor(String::pixels(%str) / 6);
			$ClientSide::BuildItemSpacer["[\""@%item@"\"]"] = $spacer[30-%len]; //For equipped stuff (speeds things up real good!)
		}
	}
	if(%num != "") {
		%t =  $ClientSide::BuildItem["[\""@%item@"\"]"]@$ClientSide::BuildItemSpacer["[\""@%item@"\"]"]@%num@"      &EQUIPPED&";	//What you don't see won't hurt you
		if(%t != "")																																	//This is only so the client "knows" if its equipped or not
			return %t;																																	//Only used when client presses "Use" in the inv screen,
		else {																																			//that way you don't "Use" and un-equip if your trying
			%t =  $ClientSide::BuildItem[%item]@$ClientSide::BuildItemSpacer[%item]@%num@"      &EQUIPPED&";							//to equip...
			return %t;
		}
	}
	else {
		if(String::findSubStr(%str, "q&") != -1)
			%str = String::replace(%str, "q&", "");
		%len = floor(String::pixels(%str) / 6);
		$ClientSide::BuildItemSpacer["[\""@%item@"\"]"] = $spacer[30-%len];
		$ClientSide::BuildItem["[\""@%item@"\"]"] = %str;
		$ClientSide::BuildItem2["[\""@%str@"\"]"] = %item;
	}
	return $ClientSide::BuildItem["[\""@%item@"\"]"]@$ClientSide::BuildItemSpacer["[\""@%item@"\"]"];
}

//The server checks if the item your trying to "buy" is on the list, so adding items won't do you any good.
function remoteGiveClientShoppingData(%manager, %ShoppingList, %done) {

	if(%manager == 2048) {
		if($RMGotOldData) {
			$ClientSide::ItemBuyList = "";
			$RMGotOldData = "";
		}

		$ClientSide::ItemBuyList = $ClientSide::ItemBuyList@%ShoppingList; //shop / storage / loot
//echo("My BuyList: '"@%ShoppingList@"'");

		//allow the server to send data in parts (to keep it from losing some when sending..)
		if(%done) {
			TextList::clear(ItemBuyTextList);
			schedule("RMInvGui::SetUpShopItems();", 0.1);
			$ClientSide::ItemBuyList = $ClientSide::ItemBuyList@" ";
			$RMGotOldData = true;
		}
	}
}

function RMInvGui::SetUpShopItems() {
	if((%list = $ClientSide::ItemBuyList) != "") { //Client is at a shop
		for(%i = 0; (%item = GetWord(%list, %i)) != -1; %i++) {
			%item = AddSpaces(%item);
			TextList::addLine(ItemBuyTextList, %item);
		}
	}
}

//This is to keep track of your items on YOUR inv, the server has its own list, so adding items won't do you any good.
function remoteClient::addItemCount(%manager, %item, %amount, %list, %header) {

	if(%manager != 2048)
		return;

	if(%list == "")
		%list = "ItemList";

	if(%item == "") {
		echo("remoteClient::addItemCount(); %item is empty! ");
		return;
	}

	if($ClientSide::Item[%list] == "")
		$ClientSide::Item[%list] = " ";

	%pos = String::findSubStr(" "@$ClientSide::Item[%list], " "@%item@" ");

	if(%pos != -1) { //Already has this item
		%data = String::NEWgetSubStr($ClientSide::Item[%list], %pos, 60);
		%nitem = GetWord(%data, 0);
		%cnt = floor(GetWord(%data, 1));
		%ncnt = floor(%cnt+%amount);
		if(%ncnt > 0) {
			$ClientSide::Item[%list] = String::replace($ClientSide::Item[%list], " "@%item@" "@%cnt, " "@%nitem@" "@%ncnt);
			ClientSide::UpdateItemOrder(%item@" "@%cnt, "replace", %list, %header, %nitem@" "@%ncnt);
		}
		else {
			$ClientSide::Item[%list] = String::replace($ClientSide::Item[%list], " "@%item@" "@%cnt@" ", " ");
			ClientSide::UpdateItemOrder(%item@" "@%cnt, "remove", %list, %header);
		}
	}
	else {
		$ClientSide::Item[%list] = $ClientSide::Item[%list]@%item@" "@%amount@" ";
		ClientSide::UpdateItemOrder(%item@" "@%amount, "add", %list, %header);
	}

	TextList::clear(ItemSellTextList);
	$RM::InvScheduleID++;
	if($RM::InvScheduleID >= 25)
		$RM::InvScheduleID = 1;
	schedule("RMInvGui::Check("@$RM::InvScheduleID@", $RM::InvScheduleID);", 0.1);
	return true;
}

//Headers!
function ClientSide::UpdateItemOrder(%item, %opt, %list, %Iheader, %nitem) {

	//%item = "ITEM_NAME AMOUNT"
	if(%list == "EquipList")
		return;//no headers for equipped stuff

	%header = $ClientSide::HeaderOrder::[%Iheader, %list];

	if(%header == "")
		%header = " ";

	if(%opt == "add")
		$ClientSide::HeaderOrder::[%Iheader, %list] = %header@%item@" ";
	else if(%opt == "remove")
		$ClientSide::HeaderOrder::[%Iheader, %list] = String::replace(%header, " "@%item@" ", " ");
	else if(%opt == "replace")
		$ClientSide::HeaderOrder::[%Iheader, %list] = String::replace(%header, " "@%item@" ", " "@%nitem@" ");

	for(%i = 0; %i <= $ClientSide::HeadersMax; %i++) {
		%char = String::getSubStr($ClientSide::Headers::ABC, %i, 1);
		%header = $ClientSide::Headers::[%char];
		%items = $ClientSide::HeaderOrder::[%header, %list];
		if(getWord(%items, 1) != -1)
			%tmp = %tmp@$ClientSide::HeaderShow::[%char]@" "@%items@" ";
	}

	//echo("Show:"@%list@" = "@%tmp);
	$ClientSide::ItemShow[%list] = %tmp;
}

function remoteClearHeadersData(%manager) {
	if(%manager == 2048) {
		deleteVariables("$ClientSide::Header*");
	}
}

//Server sends client its headers data, so server can change, add, or remove headers
function remoteSetHeadersData(%manager, %header, %headerShow, %char) {
	if(%manager == 2048) {
		$ClientSide::Headers::[%char] = %header;	$ClientSide::HeaderShow::[%char] = %headershow;
		$ClientSide::Headers::ABC = $ClientSide::Headers::ABC@%char;
		$ClientSide::HeadersMax++;
	}
}

$EquipTag = "&";
function remoteSetEquipTag(%mngr, %tag) {
	if(%mngr == 2048)
		$EquipTag = %tag;
}

function QuestBTN() {

	$RM::toggleQuestBTN = (!$RM::toggleQuestBTN |= $RM::toggleQuestBTN);
	TextList::clear(ItemSellTextList);
	TextList::clear(ItemBuyTextList);
	if($RM::toggleQuestBTN)
		RMInvGui::SetUpItems("QuestList");
	else
		RMInvGui::SetUpItems();
}

//=====================================================================


//===
//From rpgfunk.cs
//for Clients
function String::len(%string) {
	%chunk = 10;
	%length = 0;
	for(%i = 0; String::NEWgetSubStr(%string, %i, 1) != ""; %i += %chunk)
		%length += %chunk;
	%length -= %chunk;
	%checkstr = String::NEWgetSubStr(%string, %length, 99999);
	for(%k = 0; String::NEWgetSubStr(%checkstr, %k, 1) != ""; %k++)
		%length++;
	if(%length == -%chunk)
		%length = 0;
	return %length;
}

function String::replace(%string, %search, %replace) {
	%loc = String::findSubStr(%string, %search);
	if(%loc != -1) {
		%ls = String::len(%search);
		%part1 = String::NEWgetSubStr(%string, 0, %loc);
		%part2 = String::NEWgetSubStr(%string, %loc + %ls, 99999);
		%string = %part1 @ %replace @ %part2;
	}
	return %string;
}

function String::NEWgetSubStr(%s, %x, %y) {
	%len = %y;
	%chunks = floor(%len / 255) + 1;
	%q = %len;
	%nx = %x;
	%final = "";
	for(%i = 1; %i <= %chunks; %i++) {
		%q = %q - 255;
		if(%q <= 0)
			%chunkLen = %q+255;
		else
			%chunkLen = 255;
		%final = %final @ String::getSubStr(%s, %nx, %chunkLen);
		%nx = %nx + %chunkLen;
	}
	return %final;
}
//===

function remoteClientDied(%manager) {
	if(%manager == 2048) {
		for(%i = 1; %i <= 5; %i++)
			schedule("Control::setvisible(\"Fade0"@%i@"\", 1);", %i/2);

		Control::setvalue(DeathText, "<jc>You died.");

		for(%i = 1; %i <= 5; %i++)
			schedule("Control::setvisible(\"Fade0"@%i@"\", 0);", (%i/2)+7);
		schedule("Control::setValue(DeathText, \"\");", 8);
	}
}

function SetUpClient() {

	for(%i = 1; %i <= 5; %i++)
		Control::setvisible("Fade0"@%i, 0);
	Control::setValue(DeathText, "");

	PopStatsText();
	PopQuestText();

//	if($RM_CLIENT_HASERROR == 1)
//		remoteEval(2048, reportError);
}

//Reply map -- Needed to chat with townbots.
//This will NOT bind over any of your keys
//It loads when you talk with a bot and will
//temp over-write your key binds.
//When you're done talking, your old key binds
//will kick back in
NewActionMap("REPLYMap.sae");
EditActionMap("REPLYMap.sae");
$abc::array = "abcdefghijklmnopqrstuvwxyz";
for(%i = 0; %i <= 25; %i++) { %char = String::getSubStr($abc::array, %i, 1); bindcommand(keyboard0, make, %char, TO, "Reply::Send($RM::Reply["@%char@"]);"); }
bindcommand(keyboard0, make, Period, TO, "Reply::Send($RM::Reply[\".\"]);");bindcommand(keyboard0, make, space, TO, "Reply::Send($RM::Reply[space]);");bindcommand(keyboard0, make, tab, TO, "Reply::Send($RM::Reply[tab]);");bindcommand(keyboard0, make, control, TO, "Reply::Send($RM::Reply[control]);");bindcommand(keyboard0, make, rcontrol, TO, "Reply::Send($RM::Reply[rcontrol]);");
bindcommand(keyboard0, make, rshift, TO, "Reply::Send($RM::Reply[rshift]);");bindcommand(keyboard0, make, Left, TO, "Reply::Send($RM::Reply[Left]);");bindcommand(keyboard0, make, Right, TO, "Reply::Send($RM::Reply[Right]);");bindcommand(keyboard0, make, Up, TO, "Reply::Send($RM::Reply[Up]);");bindcommand(keyboard0, make, Down, TO, "Reply::Send($RM::Reply[Down]);");bindcommand(keyboard0, make, enter, TO, "Reply::Send($RM::Reply[enter]);");
for(%i = 0; %i <= 9; %i++) { bindcommand(keyboard0, make, "Numpad"@%i, TO, "Reply::Send($RM::Reply[Numpad"@%i@"]);");bindcommand(keyboard0, make, %i, TO, "Reply::Send($RM::Reply["@%i@"]);");  }
$abc::array="";
function remoteSetUpKeys(%manager, %trigs) { // Server tells you what the bot replys are.
	if(%manager == 2048) {
		deleteVariables("$RM::Reply*");
		for(%i = 0; %i <= 11; %i++) {
			%key = GetWord(%trigs, %i); %i++;
			%cmd = GetWord(%trigs, %i);
			if(%key != -1 && %cmd != -1) {
				$RM::Reply[%key] = %cmd;
			//	%tst = %tst@ %key@" "@%cmd@" ";
			}
		}
	}
	// echo(%tst);
}
function Reply::Trigger(%msg, %func, %msg2) {
	if(%msg != "" && %func != "") {
		PopActionMap("playMap.sae");
		PushActionMap("REPLYMap.sae");
		$Reply::IsActive = true;
		$Reply::Func = %func;
		$Reply::Msg = %msg;
		if(%msg2 != "")
			$Reply::Msg2 = %msg2;
		else
			$Reply::Msg2 = "\n\n  <f1>Y<f0>es.\n  <f1>N<f0>o.";
		Reply::SetUp();
	}
}
function Reply::SetUp() { // Show message
	if($Reply::IsActive) {
		if($Reply::Msg2 == NULL) {
			deleteVariables("$RM::Reply*");
			$RM::Reply[o] = "NULL";
			$Reply::Msg2 = "\n\n  <f1>O<f0>k.";
			// schedule("Reply::Send(NULL);", 2);
		}
		cursorOn(MainWindow);
		setCursor(MainWindow, "Cur_Empty.bmp");
		Control::setVisible("RMShowBox", 1);
		Control::SetValue("RMShowPic", "<B0,0:"@$RM::Pic@".bmp>");
		Control::SetValue("RMShowText", $Reply::Msg@$Reply::Msg2);
	}
}

function Reply::Send(%bool) { // Your replys

	// echo("PRESSED:"@%bool);

	if(%bool == "")
		return;

	$Reply::IsActive = "";
	Control::setVisible("RMShowBox", 0);
	PopActionMap("REPLYMap.sae");
	PushActionMap("playMap.sae");
	setCursor(MainWindow, "Cur_Arrow.bmp");
	cursorOff(MainWindow);

	if($RM::func == "" && $Reply::Func != "") {
		eval($Reply::Func@"("@%bool@");");
	}
	else if($Reply::Func != "" && $RM::func != "") {
		eval($Reply::Func@"("@$RM::func@", "@%bool@");");
	}

	deleteVariables("$RM::Reply*");

	if(%bool == NULL)
		$RM::func = $Reply::Func = "";
}

//=================================
function remoteSetupQuest(%manager, %msg, %func, $RM::func, %msg2, %pic) {
	if(%manager == 2048) {
		if(%func == "")
			%func = "remoteEval";
		if($RM::func == "")
			$RM::func = "2048, QuestChat";
		$RM::Pic = %pic;
		Eval("Reply::Trigger(%msg, %func, %msg2);");
	}
}

// %msg = "<f0>Here they are, and have a <f1>Cure Potion<f0> too. Give this to\n<f1>Booga<f0>, hes a runt and he needs these to stay alive. He\nlives near the <f1>Goblin Pass<f0> just passed the bridge.\nGood Luck!";
function tst() {
	%msg = "<f0>Here they are, and have a <f1>Cure Potion<f0> too. Give this to\n<f1>Booga<f0>, hes a runt and he needs these to stay alive. He\nlives near the <f1>Goblin Pass<f0> just passed the bridge.\nGood Luck!";
	%RMfunc = "2048, QuestChat";
	%func = "remoteEval";

	remoteSetUpQuest(2048, %msg, %func, %RMfunc, %msg2, %pic);
} // remoteEval(%Client, "SetUpQuest", %msg, %func, %RMfunc, %msg2);

//=======================================================
//SimText -- no longer needed

function SimTextInput(%key) {
	$SimTextHold[$SimTextHeldId] = "";
	$SimTextHold[$SimTextHeldId++] =%key;
	if(%key == "BackSpace")
		SimTextBackSpace();
	else if(%key == "Left")
		SimTextLeftMsg();
	else if(%key == "Right")
		SimTextRightMsg();
	else
		SimTextOnMake(%key);
}

function SimTextBackSpace() {
	if($SimTextHold[$SimTextHeldId] != "") {
		if($SimTextTyped != "") {
			if($SimTextJustOpened) {
				$SimTextTyped = "";
				$SimLoc = 0;
			}
			else {
				if($SimLoc >= 1) {
					$SimTextTyped = String::NEWgetSubStr($SimTextTyped, 0, $SimLoc--)@String::NEWgetSubStr($SimTextTyped, $SimLoc++, 99999);
					$SimLoc--;
				}
			}
			UpdateSimText();
		}
		schedule("SimTextBackSpace();", 0.2);
	}
}

function SimTextOnMake(%key) {
	$SimTextHeldId++;
	if($SimTextHeldId > 50)
		$SimTextHeldId = 0;
	$SimTextHold[$SimTextHeldId] = %key;
	$SimTextTyped = String::NEWgetSubStr($SimTextTyped, 0, $SimLoc)@%key@String::NEWgetSubStr($SimTextTyped, $SimLoc, 99999);
	$SimLoc++;
	UpdateSimText();
}

function SimScheduleCheck(%bool) {
	if($SimTextHold[$SimTextHeldId] == $SimTextScheduleKey && $SimTextHoldKey != "") {
		if($SimTextScheduleKeyCheck++ > 1 && !%bool) {
			$SimTextTyped = String::NEWgetSubStr($SimTextTyped, 0, $SimLoc)@$SimTextScheduleKey@String::NEWgetSubStr($SimTextTyped, $SimLoc, 99999);
			$SimLoc++;
			UpdateSimText();
			schedule("SimScheduleCheck();", 0.2);
		}
	}
	else {
		$SimTextScheduleKeyCheck = 0;
		$SimTextScheduleKey = $SimTextHold[$SimTextHeldId];
	}
	if($SimTextScheduleKeyCheck == 0)
		schedule("SimScheduleCheck(true);", 1);
}

function SimTextOnBreak() {
	$SimTextHold[$SimTextHeldId] = "";
}

function UpdateSimText() {

	%SimTextTeamSay[0] = "<f1>";
	%SimTextTeamSay[1] = "<f0>";

	$SimTextJustOpened = false;
	remoteBP(2048, %SimTextTeamSay[$SimTextTeamSay]@"SAY: "@String::NEWgetSubStr($SimTextTyped, 0, $SimLoc)@"|"@String::NEWgetSubStr($SimTextTyped, $SimLoc, 99999)@"\n ");
}

function SimTextPrevMsg() {

	$SimTextTmpBufferCnt--;
	if($SimTextTmpBufferCnt < 0)
		$SimTextTmpBufferCnt = $SimTextBufferCount;
	$SimTextTyped = $SimTextBuffer[$SimTextTmpBufferCnt];

	UpdateSimText();
}

function SimTextNextMsg() {
	$SimTextTmpBufferCnt++;
	if($SimTextTmpBufferCnt > $SimTextMaxBuffer)
		$SimTextTmpBufferCnt = 0;
	$SimTextTyped = $SimTextBuffer[$SimTextTmpBufferCnt];
	if(getWord($SimTextTyped, 0) == -1 && SimTextTmpBufferCnt != "0")
		$SimTextTmpBufferCnt = 0;
	UpdateSimText();
}

function SimTextLeftMsg() {
	if($SimTextHold[$SimTextHeldId] != "") {
		$SimLoc--;
		if($SimLoc < 0)
			$SimLoc = 0;
		UpdateSimText();
		schedule("SimTextLeftMsg();", 0.25);
	}
}
function SimTextRightMsg() {
	if($SimTextHold[$SimTextHeldId] != "") {
		$SimLoc++;
		%len = String::len($SimTextTyped);
		if($SimLoc > %len)
			$SimLoc = %len;
		UpdateSimText();
		schedule("SimTextRightMsg();", 0.25);
	}
}
function SimText(%team) {
	if(%team > 1 || %team == "") {
		echo("Error: SimText(team); team 0 or 1 ");
		return;
	}
	if(!$SimTextOn) {
		$SimTextHoldKey = "";
		$SimTextHold = "";
		$SimTextOn = true;
		PopActionMap("actionMap.sae");
		PopActionMap("playMap.sae");
		PushActionMap("SimTextInputMap.sae");
		$SimTextTeamSay = %team;
		UpdateSimText();

		$SimTextTmpBufferCnt = $SimTextBufferCount;
		$SimTextTmpBufferCnt++;//FIXED
		$SimTextJustOpened = true;
	}
	if($SimTextSchedule == "") {
		$SimTextSchedule = "Loaded";
		SimScheduleCheck();
	}
}
function SimTextDone(%bool) {
	remoteBP(2048, "");
	$SimTextOn = "";
	PopActionMap("SimTextInputMap.sae");
	PushActionMap("playMap.sae");
	PushActionMap("actionMap.sae");
	$SimTextTmpBufferCnt = "";
	if(%bool && $SimTextTyped != "") {
		say($SimTextTeamSay, $SimTextTyped);
		SimTextAddToBuffer($SimTextTyped);
		$SimTextTyped = "";
		$SimLoc = 0;
	}
	//else
	//Save text for when they want to chat again
	//If they have saved text and the first key
	//they hit is backspace, the whole message gets
	//deleted. You'll also save sent
	//messages, Press up/down to find them.
}


function SimTextAddToBuffer(%text) {
	if($SimTextBufferCount == "") $SimTextBufferCount = 0;
	$SimTextBuffer[$SimTextBufferCount++] = %text;
	if($SimTextBufferCount >= $SimTextMaxBuffer)
		$SimTextBufferCount = 0;//start over-writing the buffer
}

function SimTextClearBuffer() {
	deleteVariables("SimTextBuffer*");
}

function MakeSimInput() {
	NewActionMap("SimTextInputMap.sae");
	EditActionMap("SimTextInputMap.sae");
	%chars = "1234567890abcdefghijklmnopqrstuvwxyz-=/;[]";
	%shiftchars = "!@#$%^&*()ABCDEFGHIJKLMNOPQRSTUVWXYZ_+?:\{}";
	%key = "1";
	%shiftkey = "!";
	while(%key != "") {
		bindCommand(Keyboard0, make, %key, TO, "SimTextInput(\""@%key@"\");");
		bindCommand(Keyboard0, break, %key, TO, "SimTextOnBreak();");
		bindCommand(Keyboard0, make, shift, %key, TO, "SimTextInput(\""@%shiftkey@"\");");
		bindCommand(Keyboard0, break, shift, %key, TO, "SimTextOnBreak();");
		bindCommand(Keyboard0, make, rshift, %key, TO, "SimTextInput(\""@%shiftkey@"\");");
		bindCommand(Keyboard0, break, rshift, %key, TO, "SimTextOnBreak();");
		%key = String::getSubStr(%chars, %i++, 1);
		%shiftKey = String::getSubStr(%shiftchars, %i, 1);
	}
	bindCommand(Keyboard0, make, "\\", TO, "SimTextInput('\\');");bindCommand(Keyboard0, break, "\\", TO, "SimTextOnBreak();");
	bindCommand(Keyboard0, make, shift, "\\", TO, "SimTextOnMake(\"|\");");bindCommand(Keyboard0, break, "\\", period, TO, "SimTextOnBreak();");
	bindCommand(Keyboard0, make, rshift, "\\", TO, "SimTextOnMake(\"|\");");bindCommand(Keyboard0, break, "\\", period, TO, "SimTextOnBreak();");
	bindCommand(Keyboard0, make, period, TO, "SimTextInput(\".\");");bindCommand(Keyboard0, break, period, TO, "SimTextOnBreak();");
	bindCommand(Keyboard0, make, shift, period, TO, "SimTextInput(\">\");");bindCommand(Keyboard0, break, shift, period, TO, "SimTextOnBreak();");
	bindCommand(Keyboard0, make, rshift, period, TO, "SimTextInput(\">\");");bindCommand(Keyboard0, break, rshift, period, TO, "SimTextOnBreak();");
	bindCommand(Keyboard0, make, comma, TO, "SimTextInput(\",\");");bindCommand(Keyboard0, break, comma, TO, "SimTextOnBreak();");
	bindCommand(Keyboard0, make, shift, comma, TO, "SimTextInput(\"<\");");bindCommand(Keyboard0, break, shift, comma, TO, "SimTextOnBreak();");
	bindCommand(Keyboard0, make, rshift, comma, TO, "SimTextInput(\"<\");");bindCommand(Keyboard0, break, rshift, comma, TO, "SimTextOnBreak();");
	bindCommand(Keyboard0, make, quote, TO, "SimTextInput(\"'\");");bindCommand(Keyboard0, break, quote, TO, "SimTextOnBreak();");

	bindCommand(Keyboard0, make, shift, quote, TO, "SimTextInput('\"');");bindCommand(Keyboard0, break, shift, quote, TO, "SimTextOnBreak();");
	bindCommand(Keyboard0, make, rshift, quote, TO, "SimTextInput('\"');");bindCommand(Keyboard0, break, rshift, quote, TO, "SimTextOnBreak();");

	bindCommand(Keyboard0, make, up, TO, "SimTextPrevMsg();");bindCommand(Keyboard0, break, up, TO, "SimTextOnBreak();");
	bindCommand(Keyboard0, make, down, TO, "SimTextNextMsg();");bindCommand(Keyboard0, break, down, TO, "SimTextOnBreak();");

	bindCommand(Keyboard0, make, Left, TO, "SimTextInput(\"Left\");");bindCommand(Keyboard0, break, Left, TO, "SimTextOnBreak();");
	bindCommand(Keyboard0, make, Right, TO, "SimTextInput(\"Right\");");bindCommand(Keyboard0, break, Right, TO, "SimTextOnBreak();");

	bindCommand(Keyboard0, make, "escape", TO, "SimTextDone(false);");
	bindCommand(Keyboard0, make, "enter", TO, "SimTextDone(true);");
	bindCommand(Keyboard0, make, "backspace", TO, "SimTextInput(\"BackSpace\");");bindCommand(Keyboard0, break, backspace, TO, "SimTextOnBreak();");
	bindCommand(Keyboard0, make, "space", TO, "SimTextInput(\" \");");bindCommand(Keyboard0, break, space, TO, "SimTextOnBreak();");

	bindCommand(Keyboard0, make, rshift, TO, "");
	bindCommand(Keyboard0, break, rshift, TO, "");
}
//MakeSimInput();

function remoteFlash(%manager) {
	if(%manager == 2048) {
		Control::SetVisible(flash, 1);
		schedule("Control::SetVisible(flash, 0);", "0.1");
	}
}

function remoteDoCmd(%mngr, %event) {
	if(%mngr == 2048) {
		%event.DoCmd();
	}
}

//RMRPG zone music
//SERVER SENDS CLIENT THE DATA TO SETUP SOUNDS
function remoteSFXPLAY(%mngr, %bool, %wav, %len, %noMultLen) {

	if(%mngr != 2048 || $RM::MuteMusic)
		return;

	if(%bool == "" || %wav == "" || %len == "")
		return;

	//check for a .wav ext
	%pos = string::findSubStr(%wav, ".wav");
	if(%pos != -1)
		%file = string::getsubstr(%wav, 0, %pos);
	else {
		%file = %wav;
		%wav = %wav@".wav";
	}

	if(%bool) {
		sfxClose();
		sfxOpen();
	}

	sfxAddPair("IDSFX_RM"@%file, IDPRF_2D, %wav);

	$sfx::schedule[$sfx::scheduleID] = "";

	$sfx::scheduleID++;

	if($sfx::scheduleID > 0xFF)
		$sfx::scheduleID = 0;

	$sfx::schedule[$sfx::scheduleID] = "IDSFX_RM"@%file;

	if(!%noMultLen)
		%len *= 2;

	sfxLoop($sfx::schedule[$sfx::scheduleID], %len);

}

function sfxLoop(%sfx, %delay) {

	if(%sfx == "" || $RM::MuteMusic) //is this ID done for?
		return; //if so... no need to loop anymore

	sfxPlay(%sfx);

	schedule("sfxLoop($sfx::schedule["@$sfx::scheduleID@"], "@%delay@");", %delay);
}

function sfxMute() {

	$RM::MuteMusic = true;
	sfxClose();
	sfxOpen();
}

function remoteSFXSTOP(%mngr) {
	if(%mngr == 2048 && !$RM::MuteMusic) {
		$sfx::schedule[$sfx::scheduleID] = "";
		sfxClose();
		sfxOpen();
	}
}

//$RM_Time = "ô¿@ Wed Feb 06 17:14:56 2002";
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

function remoteSetPrint::Title(%mngr, %title) {
	if(%mngr == 2048) {
		Control::SetVisible("Print::Menu", 1);
		Control::SetValue("Print::Title", %title);
		Control::setvisible("Print::Menu2", 1);
		Control::SetValue("Print::Title2", %title);
	}
}

function remoteSetPrint::Info(%mngr, %info) {
	if(%mngr == 2048) {
		Control::SetVisible("Print::Menu", 1);
		Control::SetValue("Print::Info", %info);
		Control::setvisible("Print::Menu2", 1);
		Control::SetValue("Print::Info2", %info);
	}
}

function remoteSetPrint::Size(%mngr, %size) {
	if(%mngr == 2048) {

		%ScreenX = getMaxX();
		%ScreenY = getMaxY();

		%oSize = Control::getExtent("Print::Menu");

		%x = getWord(%oSize, 0);

		%nSize = %size * 15 + 30;

		Control::SetExtent("Print::Menu", %x, %nSize);

		Control::SetPosition("Print::Menu", floor(%ScreenX/2)-floor(%x/2), (%ScreenY-30)-%nSize);
		Control::SetPosition("Print::Title", 15, 0);
		Control::SetPosition("Print::Info", 15, 30);


		if(isObject("CmdInventoryGui")) {

			Control::SetExtent("Print::Menu2", %x, %nSize);

			Control::SetPosition("Print::Menu2", floor(%ScreenX/2)-floor(%x/2), (%ScreenY-30)-%nSize);
			Control::SetPosition("Print::Title2", 15, 0);
			Control::SetPosition("Print::Info2", 15, 30);
		}

	}
}

function RMclearPrint(%id) {
	if(%id == $RMPrintId) {
		Control::SetValue("Print::Title", "");
		Control::SetValue("Print::Info", "");
		Control::SetVisible("Print::Menu", 0);

		Control::SetValue("Print::Title2", "");
		Control::SetValue("Print::Info2", "");
		Control::SetVisible("Print::menu2", 0);

		$RM::PrintX = "";
		$RM::PrintY = "";

	}
}

function remoteSetPrint::TimeOut(%mngr, %timeout) {
	if(%mngr == 2048) {
		$RMPrintId++;
		schedule("RMclearPrint(" @ $RMPrintId @ ");", %timeout);
	}
}

function getMaxXY() {
	return Control::getExtent("playGui");
}

function getMaxX() {
	return getWord(Control::getExtent("playGui"), 0);
}

function getMaxY() {
	return getWord(Control::getExtent("playGui"), 1);
}

function remoteRMText::SetMoving(%mngr, %min, %max, %speed, %skip) {
	if(%mngr == 2048) {
		$RMText::Cur = %max;
		$RMText::Min = %min;
		$RMText::Delay = %speed;
		$RMText::Skip = %skip;
		RMText::MoveLoop();
		//LoadGui(CmdObjectives);
	}
}
//RMIntro::001 - 003
function RMText::MoveLoop() {

	if($RMText::Cur >= $RMText::Min) {
		$RMText::Cur -= $RMText::Skip;

		if($RMText::Cur > 0)
			Control::SetPosition(RMText::Content, 0, $RMText::Cur);
		else {
			Control::SetPosition(RMText::Content, 0, 0);
			Control::SetPosition(RMText::Text, 0, $RMText::Cur);
		}
		Schedule("RMText::MoveLoop();", $RMText::Delay);
	}
}

//===================================================================
//Not sure if we'll ever use this code.

function remoteSetDelayVar(%manager, %delay) {
	$RM::WeaponDelay = %delay;
	//schedule("echo(\"You are ready to fire.\");", %delay);
}
function ACTION_FIRE() {
	if($FireHold) {
		%time = getIntegerTime(true) >> 5;
		if(%time - $RM::LastFireTime >= $RM::WeaponDelay) {
			postAction(2048, IDACTION_BREAK1, 1);
			$RM::LastFireTime = %time;
			//echo("FIRE!");
		}
		//else
		//	echo("CAN'T FIRE!");
		schedule("ACTION_FIRE();", 0.1);
	}
}
function ACTION_FIRESTATE() {
	$FireHold = (!$FireHold |= $FireHold);
	if($FireHold)
		ACTION_FIRE();
}
if($RM::fireButton == "") $RM::fireButton = "button0";
//editActionMap("playMap.sae");//So we won't bind these
//bindCommand(mouse0, make, $RM::fireButton, TO, "ACTION_FIRESTATE();");
//bindCommand(mouse0, break, $RM::fireButton, TO, "ACTION_FIRESTATE();");
//===================================================================

if(!exec("RM.strings"))
	$RM_CLIENT_HASERROR = 1;

////////////////////////////////////////////////////////////
//Do not edit

$__RM__DEUSCLIENT_CS = 0x1cf;
