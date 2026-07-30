//------------------------------------
//
// Game/network functions
//
//------------------------------------

function selectNewMaster()
{
   translateMasters();
}

function checkMasterTranslation()
{
   for(%i = 0; %i < $Server::numMasters; %i++)
   {
      %mstr = DNet::getResolvedMaster(%i);
      if(%mstr != "")
         $Server::XLMasterN[%i] = %mstr;
      $inet::master[%i+1] = $Server::XLMasterN[%i];
   }
}

function translateMasters()
{
   for(%i = 0; (%word = getWord($Server::MasterAddressN[$Server::CurrentMaster], %i)) != -1; %i++)
      %mlist[%i] = %word;

   $Server::numMasters = DNet::resolveMasters(%mlist0, %mlist1, %mlist2, %mlist3, %mlist4, %mlist5, %mlist6, %mlist7, %mlist8, %mlist9);
}

function refreshWindowSize(){
	if($pref::softwareRes == "")
		$pref::softwareRes = "936 702";
	if(!isFullScreenMode(MainWindow)){
		setWindowSize(MainWindow, getWord($pref::softwareRes,0), getWord($pref::softwareRes,1));
	}
}

function QuickStart()
{
	refreshWindowSize();
	revertDrawDistance();	$rLoadedWorld = "";	$rLoadedPal = "";
	$repackKeyOverride = "";
	$ConnectedToServer = FALSE;
   setCursor(MainWindow, "Cur_Arrow.bmp");
	cursorOn(MainWindow);
	$QuickStart = TRUE;
	if ($PlayingDemo)
	{
		$PlayingDemo = FALSE;
		GuiLoadContentCtrl(MainWindow, "gui\\Recordings.gui");
	}
	else if ($PCFG::Name == "")
	   GuiLoadContentCtrl(MainWindow, "gui\\PlayerSetup.gui");
	else if ($pref::PlayGameMode == "")
	   GuiLoadContentCtrl(MainWindow, "gui\\Connect.gui");
	else if ($Pref::PlayGameMode == "JOIN")
	   GuiLoadContentCtrl(MainWindow, "gui\\JoinGame.gui");
	else if ($pref::PlayGameMode == "TRAIN")
	   GuiLoadContentCtrl(MainWindow, "gui\\training.gui");
	else
	   GuiLoadContentCtrl(MainWindow, "gui\\CreateServer.gui");
}

function PlayerSetupNext()
{
	if ($QuickStart == "TRUE")
		QuickStart();
	else
	{
		if ($PCFG::Name == "")
			OpenNewPlayerDialog();
		else
		   GuiLoadContentCtrl(MainWindow, "gui\\Connect.gui");
	}
}

function JoinGame()
{
	if ($IRC::DisconnectInSim == "")
		$IRC::DisconnectInSim = true;

	//disconnect from IRC if req'd
	if ($IRC::DisconnectInSim == true)
	{
		ircDisconnect();
		$IRCConnected = FALSE;
		$IRCJoinedRoom = FALSE;
	}
	//load the "connecting" gui
	GuiLoadContentCtrl(MainWindow, "gui\\Connecting.gui");

   $SinglePlayer = False;
   purgeResources();
   connect($Server::Address);
}

function EndGame()
{
	$ConnectedToServer = FALSE;
   setCursor(MainWindow, "Cur_Arrow.bmp");
   disconnect();
   if ($SinglePlayer)
	   TrainingEndGameCallback();

	if ($pref::userCDOverride == False)
	{
		rbSetPlayMode (CD, 0);
		rbStop (CD);
		$cdTrack = "";
	}
	$recordDemo = FALSE;
   startMainMenuScreen();
   deleteServer();
	QuickStart();
   deleteObject(ConsoleScheduler);
   newObject(ConsoleScheduler, SimConsoleScheduler);
   if($quitOnDisconnect)
      schedule("quit();", 0);
}

function Canvas::handleEscapeKey()
{
	if ($ConnectedToServer && $InLobbyMode)
	{
		Client::exitLobbyMode();
		return "true";
	}
	return "false";
}

function startMainMenuScreen()
{
   GuiLoadContentCtrl(MainWindow, "gui\\MainMenu.gui");

	if ($pref::userCDOverride == False)
	{
		rbSetPlayMode (CD, 0);
		rbStop (CD);
		$cdPlayMode = 1;
		$cdTrack = 2;
		if ($pref::cdMusic)
			rbPlay (CD, $cdTrack);
	}
	else if ($Override == "")
	{
		$Override = True;
		rbStop (CD);
		rbSetPlayMode (CD, 2);
		rbPlay (CD);
	}
   setCursor(MainWindow, "Cur_Arrow.bmp");
   cursorOn(MainWindow);

 //  SetUpRMGUI();
}

function SetUpRMGUI() {
	echo(">> Setting up RM GUI");

	File::Delete("config\\play.bck");
	File::Copy("config\\play.gui", "config\\play.bck");

	if(isfile("config\\RMplay.gui")) {
		File::Delete("config\\play.gui");
		File::Copy("config\\RMplay.gui", "config\\play.gui");
	}
}

function loadPlayGui() {

//	if(File::FindFirst("play.gui") != "")
//			GuiLoadContentCtrl(MainWindow, "play.gui");
//	else
		GuiLoadContentCtrl(MainWindow, "gui\\play.gui");
}// loadObject("MenusGUI", "gui\\Menus.gui");

function switchToChat()
{
	if ($IRCConnected == "TRUE")
		GuiLoadContentCtrl(MainWindow, "gui\\IRCChat.gui");
	else
		GuiLoadContentCtrl(MainWindow, "gui\\IRCLogin.gui");
}

//-----------------------------------------------
//
// onExit() is called when the program terminates
//
//-----------------------------------------------

function onExit() {

//	if(isObject(playGui))
//		storeObject(playGui, "config\\RMplay.gui"); //Save RM GUI
//	File::Delete("config\\play.gui");
//	File::Copy("config\\play.bck", "config\\play.gui");
//	File::Delete("config\\play.bck");

	SaveItemBuildList();

	if(String::findSubStr($modList, "crurpg") != -1)
		saveActionMap("config\\crurpg_config.cs", "actionMap.sae", "playMap.sae", "pdaMap.sae");
	else
		saveActionMap("config\\config.cs", "actionMap.sae", "playMap.sae", "pdaMap.sae");

//	saveActionMap("config\\RMmap.cs", "SimTextInputMap.sae");

	//update the video mode - since it can be changed with alt-enter
	$pref::VideoFullScreen = isFullScreenMode(MainWindow);
	if(!$pref::VideoFullScreen)
	{
		if(isObject(playGui))
			$pref::softwareRes = Control::getExtent(PlayGui);
	}
	revertDrawDistance();

   checkMasterTranslation();
	echo("exporting pref::* to prefs.cs");
   export("pref::*", "config\\ClientPrefs.cs", False);
   export("Server::*", "config\\ServerPrefs.cs", False);
   export("pref::lastMission", "config\\ServerPrefs.cs", True);
   BanList::export("config\\banlist.cs");
}

//-----------------------------------------------
//
// onQuit() is called when the user clicks the
// in-shell close box
//
//-----------------------------------------------

function onQuit() {
	GuiPushDialog(MainWindow, "gui\\Exit.gui");
}

function JoinGameGui::onOpen()
{
   checkMasterTranslation();
	//update the record button
	if (! $recordDemo) $recorderFileName = "";
	Control::setValue(RecordDemo, $recordDemo);
}

function CreateServerGui::onOpen()
{
   TextList::clear(MissionTextList);
   FGCombo::clear(MissionTypePopup);
   for(%i = 0; %i < $MLIST::TypeCount; %i++)
      if($MLIST::Type[%i] != "Training")
         FGCombo::addEntry(MissionTypePopup, $MLIST::Type[%i], %i);

   if($pref::LastMissionType == "" || $pref::LastMissionType >= $MLIST::TypeCount)
      $pref::LastMissionType = 0;
   FGCombo::setSelected(MissionTypePopup, $pref::LastMissionType);
   MissionTypePopup::buildList();

	//verify the max number of players as well...
	verifyMaxPlayers();

	//sort the mission text list
	FGTextList::sort(MissionTextList);

	//now set the selected
   MissionTextList::onAction();

	//update the record button
	if (! $recordDemo) $recorderFileName = "";
	Control::setValue(RecordDemo, $recordDemo);
}

function enableHostGame()
{
	%valid = TRUE;

	//check the max number of players
	if ($Server::MaxPlayers < 1 || $Server::MaxPlayers > 128)
	{
		%valid = FALSE;
	}

	//check the server name
	if (String::empty($Server::HostName))
	{
		%valid = FALSE;
	}

	//set the button state
	Control::setActive("CreateServerNow", %valid);
}

function verifyServerName()
{
	if (String::Empty(Control::getValue("CreateServerHostName")))
	{
		Control::setValue("CreateServerHostName", "TRIBES Server");
	}
}

function verifyMaxPlayers()
{
	if (Control::getValue(IDCTG_MAX_PLAYERS) < 1) Control::setValue(IDCTG_MAX_PLAYERS, 8);
	else if (Control::getValue(IDCTG_MAX_PLAYERS) > 32) Control::setValue(IDCTG_MAX_PLAYERS, 32);
	enableHostGame();
}

function TrainingGui::onOpen()
{
   TextList::clear(TrainingMissionTextList);
   for(%i = 0; %i < $MLIST::TypeCount; %i++)
      if($MLIST::Type[%i] == "Training")
         break;

   for(%j = 0; (%mis = getWord($MLIST::MissionList[%i], %j)) != -1; %j++)
      TextList::addLine(TrainingMissionTextList, $MLIST::EName[%mis]);

	//sort the training mission text list
	FGTextList::sort(TrainingMissionTextList);

   Control::setValue(TrainingMissionTextList, $pref::LastTrainingMission);
   TrainingMissionTextList::onAction();

   %prev = getWord($MLIST::MissionList[%i], 0);
   %ml = $MLIST::MissionList[%i] @ %prev;
   %prevName = $MLIST::EName[%prev];

	//update the record button
	if (! $recordDemo) $recorderFileName = "";
	Control::setValue(RecordDemo, $recordDemo);
}

function MissionTypePopup::buildList()
{
   %sel = FGCombo::getSelected(MissionTypePopup);
   TextList::clear(MissionTextList);

	//see if it gets added
	%found = FALSE;
   for(%i = 0; (%mis = getWord($MLIST::MissionList[%sel], %i)) != -1; %i++)
	{
      TextList::addLine(MissionTextList, $MLIST::EName[%mis]);
		if ($MLIST::EName[%mis] == $pref::LastMission) %found = TRUE;
	}

	//if the last mission selected was not added, choose the first in the list
	if (%found == FALSE)
	{
	   $pref::lastMission = $MLIST::EName[getWord($MLIST::MissionList[%sel], 0)];
	}

	//sort the mission text list
	FGTextList::sort(MissionTextList);

   Control::setValue(MissionTextList, $pref::LastMission);
   $pref::lastMissionType = %sel;
   MissionTextList::onAction();
}

function MissionTextList::onAction()
{
   $pref::lastMission = Control::getValue(MissionTextList);

   // find the mission number...
   for(%i = 0; %i < $MLIST::Count; %i++)
      if($MLIST::EName[%i] == $pref::lastMission)
         break;

   Control::setValue(MissionDescText, $MLIST::EText[%i]);

}

function TrainingMissionTextList::onAction()
{
   $pref::lastTrainingMission = Control::getValue(TrainingMissionTextList);

   // find the mission number...
   for(%i = 0; %i < $MLIST::Count; %i++)
      if($MLIST::EName[%i] == $pref::lastTrainingMission)
         break;

   Control::setValue(MissionDescText, $MLIST::EText[%i]);

}
//--------------------------------------------------------------------------------------------------
function JoinGameGui::onOpen()
{
	//now clear the list, and re-enter it
	FGCombo::clear(JoinGameBuddyCombo);

	%i = 0;
	while ($pref::buddyList[%i] != "")
	{
	   FGCombo::addEntry(JoinGameBuddyCombo, $pref::buddyList[%i], %i);
		%i++;
	}

	Control::setValue(JoinGameBuddyList, $pref::buddyList);
}

function JGBuddyCombo::select()
{
   %sel = FGCombo::getSelected(JoinGameBuddyCombo);
	$pref::buddyList = $pref::buddyList[%sel];
	Control::setValue(JoinGameBuddyList, $pref::buddyList[%sel]);

	//push the selected to the top of the combo
	for (%i = %sel; %i > 0; %i--)
	{
		$pref::buddyList[%i] = $pref::buddyList[%i - 1];
	}
	$pref::buddyList[0] = $pref::buddyList;

	//now recreate the combo
	JoinGameGui::onOpen();

	Server::ResortList(IDCTG_SERVER_SELECT_LIST);
}

function updateBuddyList()
{
	$pref::buddyList = Control::getValue(JoinGameBuddyList);

	//if we're searching for a new buddy, push it at the front of the list, and bump the rest down
	if ($pref::buddyList != $pref::buddyList[0] && $pref::buddyList != "")
	{
		for (%i = 15; %i > 0; %i--)
		{
			$pref::buddyList[%i] = $pref::buddyList[%i - 1];
		}
		$pref::buddyList[0] = $pref::buddyList;

		//now recreate the combo
		JoinGameGui::onOpen();
	}

	Server::ResortList(IDCTG_SERVER_SELECT_LIST);
}

function JGNewServer::verify()
{
	%name = Control::getValue(JoinGameNewServerName);
	%address = Control::getValue(JoinGameNewServerAddress);

	if (%name != "" && %address != "")
	{
		Control::setActive(DialogReturnButton, TRUE);
	}
	else
	{
		Control::setActive(DialogReturnButton, FALSE);
	}
}

function JGNewServer()
{
	%name = Control::getValue(JoinGameNewServerName);
	%address = Control::getValue(JoinGameNewServerAddress);
	echo("DEBUG: " @ %name @ " " @ %address);

	addGameServer(%address, %name, "N/A", "-1", "TRUE");
	GuiPopDialog(MainWindow, 0);
}

//--------------------------------------------------------------------------------------------------

function RecordingsGui::onOpen()
{
	$RecordingsCount = 0;
   EvalSearchPath();
	TextList::clear(RecordingsTextList);
   %rec = File::FindFirst("*.rec");
   while(%rec != "")
   {
      %demo = File::getBase(%rec);
      TextList::addLine(RecordingsTextList, %demo);
      %rec = File::FindNext("*.rec");

		$Recording[$RecordingsCount] = %demo;
		$RecordingsCount++;
   }
	RecordingsGui::verify();
	FGTextList::sort(RecordingsTextList);
	Control::setValue(RecordingsContinuous, $pref::RecordingsContinuous);
}

function RecordingsGui::verify()
{
	if (Control::getValue(RecordingsTextList) == "")
	{
		%selected = FALSE;
	}
	else
	{
		%selected = TRUE;
	}
	Control::setActive(RecordingsRemove, %selected);
	Control::setActive(RecordingsRename, %selected);
	Control::setActive(RecordingsPlayDemo, %selected);
}

function RecordingsGui::PlayDemo()
{
	%fileName = "recordings\\" @ Control::getValue(RecordingsTextList) @ ".rec";
	if (isFile(%fileName))
	{
		$PlayingDemo = TRUE;
	   playDemo("recordings\\" @ Control::getValue(RecordingsTextList) @ ".rec");
		cursorOn(MainWindow);
	   GuiLoadContentCtrl(MainWindow, "gui\\Loading.gui");
		renderCanvas(MainWindow);
	}
	else
	{
		RecordingsGui::onOpen();
	}
}

function RecordingsGui::removeSelectedDemo()
{
	if (Control::getValue(RecordingsTextList) != "")
	{
		%srcFile = "recordings\\" @ Control::getValue(RecordingsTextList) @ ".rec";

		//delete the file
		File::delete(%srcFile);

		//now repopulate the list
		RecordingsGui::onOpen();
	}
}

function RecordingsGui::renameSelectedDemo()
{
	//first copy the old
	%newName = Control::getValue(RenameDemoText);
	if (Control::getValue(RecordingsTextList) != "" && %newName != "")
	{
		%srcFile = "recordings\\" @ Control::getValue(RecordingsTextList) @ ".rec";
		if (String::findSubStr(%newName, ".rec") >= 1)
		{
			%destFile = "recordings\\" @ %newName;
		}
		else
		{
			%destFile = "recordings\\" @ %newName @ ".rec";
		}

		//copy the file
		if (File::copy(%srcFile, %destFile))
		{
			//delete the old
			File::delete(%srcFile);

			//now repopulate the list
			RecordingsGui::onOpen();

			//reselect the new name
			TextList::setSelected(RecordingsTextList, %newName);
			RecordingsGui::verify();
			GuiPopDialog(MainWindow, 0);
		}
		else
		{
			GuiPopDialog(MainWindow, 0);
			GuiPushDialog(MainWindow, "gui\\MessageDialog.gui");
			Control::setValue(MessageDialogTextFormat, "Unable to save recording as <f1>" @ %newName @ ".<f0>  Please ensure the new name is a valid file name.");
		}
	}
}

function RenameDemoText::onAction()
{
	if (! String::empty(Control::getValue(RenameDemoText)))
	{
		Control::setActive(IDCTG_RENAME_DEMO_DONE, TRUE);
	}
	else
	{
		Control::setActive(IDCTG_RENAME_DEMO_DONE, FALSE);
	}
}

function RecordingsGui::playRandom()
{
	if ($RecordingsCount > 0 && $pref::RecordingsContinuous)
	{
		%randomNum = floor(getRandom() * ($RecordingsCount - 0.01));
		Control::setValue(RecordingsTextList, $Recording[%randomNum]);
		RecordingsGui::PlayDemo();
	}
}

//--------------------------------------------------------------------------------------------------

function CmdInventoryGui::favoritesSel(%favList) {}
function CmdInventory::setFavorites() {}
function CmdInventoryGui::markFavorites() {}
function CmdInventoryGui::buyFavorites() {}

function ConnectGui::ChooseGame()
{
   if($pref::playGameMode == "JOIN")
      GuiLoadContentCtrl(MainWindow, "gui\\joingame.gui");
   else if($pref::playGameMode == "TRAIN")
      GuiLoadContentCtrl(MainWindow, "gui\\training.gui");
   else
      GuiLoadContentCtrl(MainWindow, "gui\\createServer.gui");
}

function LoadingGui::onOpen()
{
   Control::setValue(ProgressSlider, 0.01);
}

//--------------------------------------------------------------------------------------------------
function userCD ()
{
 	$cdPlayMode = 2;
	$pref::cdMusic = False;
	$pref::userCDOverride = True;
	rbStop (CD);
	rbSetPlayMode (CD, $cdPlayMode);
	rbPlay (CD, 1);
}

function gameCD ()
{
	$pref::userCDOverride = False;
	$pref::cdMusic = True;
	rbStop (CD);
	$cdPlayMode = 0;
	rbSetPlayMode (CD, $cdPlayMode);
	rbPlay (CD, 2);
}

//--------------------------------------------------------------------------------------------------
function LobbyGui::onOpen()
{
	Control::setValue(LobbyServerName, $ServerName);
	Control::setValue(LobbyServerType, $ServerMod);
	Control::setValue(LobbyMissionName, $ServerMission);
	Control::setValue(LobbyMissionType, $ServerMissionType);
	Control::setValue(LobbyVersion, $ServerVersion);
	Control::setValue(LobbyAddress, $Server::Address);

   Control::setValue(LobbyServerText, $ServerInfo);
   Control::setValue(LobbyMissionDesc, $ServerText);
}

//temp
function RMTextGui::onOpen() {
	cursorOn(MainWindow);
}
function RMTextGui::onClose() {
	cursorOff(MainWindow);
}//

//==================================================================================================
//Deus_ex_Machina
//
// NEW INV SYSTEM


function CmdInventoryGui::onOpen(%opt) {

	setCursor(MainWindow, "Cur_Arrow.bmp");
	//cursorOn(MainWindow);
//	if($RM::Reply[o] == "NULL")	//if client is chating and forced into INV, send null reply
		Reply::Send("NULL");		//if null was sent.

//Send NULL anyways...
//should fix some inv gui problams

	Control::setVisible("ToDropEdit", 0);

	RMInvGui::SetUpItems(%opt);

	Control::setValue("NEW_HP2", $RMInv::HP);
	Control::setValue("NEW_MP2", $RMInv::MP);
	Control::setValue("stamina2", $RMInv::STA);
	Control::setValue("ToDropText", "<f1>Bulk: "@$RM::Bulk);
}

function RMInvGui::Check(%id, %newid) {
	if(%id == %newid)
		RMinvGui::SetUpItems();
}

function RMInvGui::SetUpItems(%opt) {

	if(!isObject("CmdInventoryGui"))
		return; // No need to update if you are not looking at it.

	$RM::toggleQuestBTN = false;

	if(%opt == QuestList) { //Client pressed the Quest view item btn
		$RM::toggleQuestBTN = true;
		TextList::addLine(ItemSellTextList, "---- Quest Items ----");
		if((%list = $ClientSide::ItemQuestList) != "") {
			for(%i = 0; %i <= 50; %i+=2) {
				%Item = GetWord(%list, %i);
				if(%Item != -1) {
					%item = AddSpaces(%item);
					TextList::addLine(ItemSellTextList, %Item);
				}
				else
					break;
			}
		}
		return; //QuestList is only thing we want to view
	}

	%list = $ClientSide::ItemEquipList;
	if(getWord(%list, 1) != -1) { //Client has equipped stuff on! This goes on top
		TextList::addLine(ItemSellTextList, "------- Equipped -------");
		for(%i = 0; (%item = GetWord(%list, %i)) != -1; %i++) {
			%i++;
			%item = AddSpaces(%item, GetWord(%list, %i));
		//	$RMInvItemCountList[%cnt++] = %item;
			TextList::addLine(ItemSellTextList, %item);
		}
	}

	if((%list = $ClientSide::ItemShowItemList) != "") { //Items
		for(%i = 0; (%item = GetWord(%list, %i)) != -1; %i++) {
			%i++;
			if(String::findSubStr(%item, "----") == -1)
				%item = AddSpaces(%item)@GetWord(%list, %i);
			else {
				%item = AddSpaces(%item, "", true);
				%item = %item@" "@getWord(%list, %i);
			}

			TextList::addLine(ItemSellTextList, %item);
		}
	}

	RMInv::SetSelected(ItemSellTextList, $RMInv::LastSellItem);
	ItemSellTextList::onAction(true);

//	schedule("Control::setVisible(ToDropEdit, 1);", 1);
	Control::setValue("ToDropEdit", 1);
	CheckBulk();
}

function CmdInventoryGui::onClose() { //Keep the inv updated with the server, remove these will/can only affect what the client sees
	TextList::clear(ItemSellTextList);
	TextList::clear(ItemBuyTextList);
	$ClientSide::ItemBuyList = "";
	Control::setActive(ItemUse, 0);
	Control::setActive(ItemDrop, 0);
	Control::setActive(ItemSell, 0);
}
//FGTextList::sort(ItemSellTextList);//Makes everything go in ABC order..

//function CheckBulk() {
//	schedule("_CheckBulk();", 0.2);
//}
function CheckBulk() {
	//Control::setValue("ToDropEdit",
	$RM::Bulk = Cap(floor($RM::Bulk), 1, 99); //);
	Control::setValue("ToDropText", "<f1>Bulk: "@$RM::Bulk);
}

function toggleBulkShow() {
	Control::SetVisible(ToDropEdit, !Control::GetVisible(ToDropEdit));
}

function RMInv::SetSelected(%control, %lastitem) {

	if(%control == "") {
		echo("RMInv::SetSelected(control, item);");
		return;
	}

	if(%lastitem == "")
		return; // hasn't clicked yet.

	%wc = GetWordCount(%lastitem);
	if(String::findSubStr(%lastitem, "&EQUIPPED&") != -1) {
		%wc--;
		%equip = "      &EQUIPPED&";
		%z = $EquipTag;
		%list = $ClientSide::Item[EquipList];
	}
	else {
		%equip = "";
		%z = "";
		%list = $ClientSide::Item[ItemList];
	}

	if($RM::toggleQuestBTN) {
		Control::setValue(%control, %lastitem);
		return;
	}

	%item = getWords(%lastitem, 0, %wc-1);

	%a = $ClientSide::BuildItem2[%item];
	if(%a == "")
		%a = $ClientSide::BuildItem2["[\""@%item@"\"]"];
	if(%a == "")
		%a = $ClientSide::BuildItem2[%item@" "];
	if(%a == "")
		%a = $ClientSide::BuildItem2["[\""@%item@" \"]"];

	%a = %a@%z; // add 0 if equipped

	%b = $ClientSide::BuildItem[%a];
	if(%b == "")
		%b = $ClientSide::BuildItem["[\""@%a@"\"]"];

	%space = $ClientSide::BuildItemSpacer[%a];
	if(%space == "")
		%space = $ClientSide::BuildItemSpacer["[\""@%a@"\"]"];

	%pos = String::findSubStr(%list, %a@" ");
	if(%pos != -1) {
		%data = String::NEWgetSubStr(%list, %pos, 60);

		%cnt = floor(GetWord(%data, 1));
	}
	Control::setValue(%control, %b@%space@%cnt@%equip);
//echo("SetSel: '"@%b@%space@%cnt@%equip@"' --- wc:"@%wc@" lastitem:'"@%lastitem@"' item:'"@%item@"' a:'"@%a@"' ");
}

function GetItemFromList(%TextList, %IsBuyList) {

	%val = Control::getValue(%TextList);

	if($RM::toggleQuestBTN) {
		return %val;
	}

	if(getWord(%val, 0) == -1)
		return "NULL";

	%wc = GetWordCount(%val);
	if(String::findSubStr(%val, "&EQUIPPED&") != -1)//Clicked on a Equipped Item
		%wc--;
	if(%IsBuyList)
		%item = getWords(%val, 0, %wc, "ItemFromList IsBuyList");
	else
		%item = getWords(%val, 0, %wc-1, "ItemFromList");

	return %item;
}

//Editing these is NOT recommoned!
//This checks to make sure you didn't click on equipped stuff
//If you edit and lose an item(selling or dropping) by trying
//something funny, don't come crying to me.

$RMInvLastClickTime = 0;
$DoubleClickDelay = "0.5";

function ItemSellTextList2::onAction() {
	Control::setPosition(ItemSellTextList2, 180, getWord(Control::getPosition(ItemSellTextList), 1));
}

function ItemSellTextList::onAction(%check) {

	ItemSellTextList2::onAction();

	if(String::findSubStr(Control::GetValue(ItemSellTextList), "----") != -1) {
		RMInv::SetSelected(ItemSellTextList, $RMInv::LastSellItem);
		return;
	}

	Control::setValue("ToDropEdit", Cap(floor($RM::Bulk), 1, 99));

	%item = GetItemFromList(ItemSellTextList);
	%ritem = Control::GetValue(ItemSellTextList);

	Control::setActive(ItemUse, 1);
	Control::setActive(ItemDrop, 1);
	if($ClientSide::ItemBuyList != "")
		Control::setActive(ItemSell, 1);
	if(%ritem == $RMInv::LastSellItem) { // if(%item == $RMInvLastItemClicked) {
		%time = getIntegerTime(true) >> 5;
		if(%time - $RMInvLastClickTime <= $DoubleClickDelay && !%check) { // 'Double Click'
			ItemSellTextList::onDoubleClick(%item);
			$RMInvLastClickTime = %time;
		}
		else
			$RMInvLastClickTime = %time;
	}
	else {
		$RMInv::LastSellItem = Control::GetValue(ItemSellTextList);
//		$RMInvLastItemClicked = %item;
	}
}

function ItemSellTextList::onDoubleClick(%sel) {
	if(String::findSubStr(Control::getValue(ItemSellTextList), "&EQUIPPED&") != -1)//Clicked on a Equipped Item
		return; //Can't sell equipped stuff so don't ask the server
	remoteEval(2048, "sellItem", %sel, $RM::Bulk);
}

function ItemBuyTextList::onAction() {

	%item = GetItemFromList(ItemBuyTextList, true);
	if(%item == $RMInvLastItemClicked) {
		%time = getIntegerTime(true) >> 5;
		if(%time - $RMInvLastClickTime <= $DoubleClickDelay) { // 'Double Click'
			ItemBuyTextList::onDoubleClick(%item);
			$RMInvLastClickTime = %time;
		}
		else
			$RMInvLastClickTime = %time;
	}
	else {
		$RMInvLastItemClicked = %item;
	}
}

function ItemBuyTextList::onDoubleClick(%sel) {
	remoteEval(2048, "buyItem", %sel, $RM::Bulk);
}

function ItemTextList::sellSelected() {
	%sel = GetItemFromList(ItemSellTextList);
	if(String::findSubStr(Control::getValue(ItemSellTextList), "&EQUIPPED&") != -1)//Clicked on a Equipped Item
		return; //Can't sell equipped stuff so no need to ask the server

	remoteEval(2048, "sellItem", %sel, $RM::Bulk);
}

function ItemTextList::dropSelected() {
   %sel = GetItemFromList(ItemSellTextList);

	if(String::findSubStr(Control::getValue(ItemSellTextList), "&EQUIPPED&") != -1)//Clicked on a Equipped Item
		return; //Can't drop equipped stuff so no need to ask the server
	remoteEval(2048, "dropItem", %sel, "", $RM::Bulk);
}

function ItemTextList::useSelected() {
	if(String::findSubStr(Control::getValue(ItemSellTextList), "&EQUIPPED&") != -1)//Clicked on a Equipped Item
		%equip = true;
	%sel = GetItemFromList(ItemSellTextList);
//echo("equipped?:"@%equip@" ITEM: '"@%sel@"'");
	remoteEval(2048, "useItem", %sel, %equip);
}

function ItemTextList::buySelected() {
	%item = GetItemFromList(ItemBuyTextList, true);
	remoteEval(2048, "buyItem", %item, $RM::Bulk);
}

//================================
//  Written by Tim "Zear" Hammock [moded just a bit]
//
// Start

function String::ascii(%string, %idx) {
	if(String::len(%string) <= %idx || %idx < 0)
	    return -1;
	%char = String::NEWgetSubStr(%string, %idx, 1);
	%idx = String::findSubStr($String::asciiString, %char);
	if(%idx < 0)
		return -1;
	if(String::Compare(%char, String::getSubStr($String::asciiString, %idx, 1)) == 0)
		return %idx + 32;
    else
        return %idx + 64;
}

function String::pixels(%string) {
    %pixels = 0;
    %len = String::len(%string);
    for(%i = 0; %i < %len; %i++) {
        if((%val = String::ascii(%string, %i)) != -1)
            %pixels += $String::asciipixels[%val];
    }
    return %pixels;
}

$String::asciiString = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";

$String::asciipixels[32] = 6;	//	space
$String::asciipixels[33] = 3;	//	!
$String::asciipixels[34] = 4;	//	"
$String::asciipixels[35] = 10;	//	#
$String::asciipixels[36] = 8;	//	$
$String::asciipixels[37] = 13;	//	%
$String::asciipixels[38] = 10;	//	&
$String::asciipixels[39] = 2;	//	'
$String::asciipixels[40] = 5;	//	(
$String::asciipixels[41] = 5;	//	)
$String::asciipixels[42] = 6;	//	*
$String::asciipixels[43] = 8;	//	+
$String::asciipixels[44] = 4;	//	,
$String::asciipixels[45] = 4;	//	-
$String::asciipixels[46] = 3;	//	.
$String::asciipixels[47] = 5;	//	/
$String::asciipixels[48] = 7;	//	0
$String::asciipixels[49] = 3;	//	1
$String::asciipixels[50] = 7;	//	2
$String::asciipixels[51] = 7;	//	3
$String::asciipixels[52] = 7;	//	4
$String::asciipixels[53] = 7;	//	5
$String::asciipixels[54] = 7;	//	6
$String::asciipixels[55] = 7;	//	7
$String::asciipixels[56] = 7;	//	8
$String::asciipixels[57] = 7;	//	9
$String::asciipixels[58] = 3;	//	:
$String::asciipixels[59] = 4;	//  ;
$String::asciipixels[60] = 8;	//	<
$String::asciipixels[61] = 9;	//	=
$String::asciipixels[62] = 8;	//	>
$String::asciipixels[63] = 5;	//	?
$String::asciipixels[64] = 13;	//	@
$String::asciipixels[65] = 11;	//	A
$String::asciipixels[66] = 8;	//	B
$String::asciipixels[67] = 9;	//	C
$String::asciipixels[68] = 10;	//	D
$String::asciipixels[69] = 7;	//	E
$String::asciipixels[70] = 7;	//	F
$String::asciipixels[71] = 10;	//	G
$String::asciipixels[72] = 9;	//	H
$String::asciipixels[73] = 3;	//	I
$String::asciipixels[74] = 5;	//	J
$String::asciipixels[75] = 10;	//	K
$String::asciipixels[76] = 7;	//	L
$String::asciipixels[77] = 11;	//	M
$String::asciipixels[78] = 10;	//	N
$String::asciipixels[79] = 11;	//	O
$String::asciipixels[80] = 8;	//	P
$String::asciipixels[81] = 11;	//	Q
$String::asciipixels[82] = 9;	//	R
$String::asciipixels[83] = 8;	//	S
$String::asciipixels[84] = 9;	//	T
$String::asciipixels[85] = 9;	//	U
$String::asciipixels[86] = 10;	//	V
$String::asciipixels[87] = 14;	//	W
$String::asciipixels[88] = 11;	//	X
$String::asciipixels[89] = 11;	//	Y
$String::asciipixels[90] = 10;	//	Z
$String::asciipixels[91] = 5;	//	[
$String::asciipixels[92] = 5;	//	\
$String::asciipixels[93] = 5;	//	]
$String::asciipixels[94] = 9;	//	^
$String::asciipixels[95] = 8;	//	_
$String::asciipixels[96] = 4;	//	`
$String::asciipixels[97] = 7;	//	a
$String::asciipixels[98] = 7;	//	b
$String::asciipixels[99] = 6;	//	c
$String::asciipixels[100] = 7;	//	d
$String::asciipixels[101] = 7;	//	e
$String::asciipixels[102] = 6;	//	f
$String::asciipixels[103] = 8;	//	g
$String::asciipixels[104] = 7;	//	h
$String::asciipixels[105] = 3;	//	i
$String::asciipixels[106] = 4;	//	j
$String::asciipixels[107] = 8;	//	k
$String::asciipixels[108] = 3;	//	l
$String::asciipixels[109] = 11;	//	m
$String::asciipixels[110] = 7;	//	n
$String::asciipixels[111] = 8;	//	o
$String::asciipixels[112] = 7;	//	p
$String::asciipixels[113] = 7;	//	q
$String::asciipixels[114] = 6;	//	r
$String::asciipixels[115] = 6;	//	s
$String::asciipixels[116] = 6;	//	t
$String::asciipixels[117] = 7;	//	u
$String::asciipixels[118] = 8;	//	v
$String::asciipixels[119] = 11;	//	w
$String::asciipixels[120] = 8;	//	x
$String::asciipixels[121] = 8;	//	y
$String::asciipixels[122] = 7;	//	z
$String::asciipixels[123] = 6;	//	{
$String::asciipixels[124] = 2;	//	|
$String::asciipixels[125] = 6;	//	}
$String::asciipixels[126] = 9;	//	~

// End
//
//===========================================

////////////////////////////////////////////////////////////
//Do not edit
$__RM__GUI_CS       = 0x2cf;
