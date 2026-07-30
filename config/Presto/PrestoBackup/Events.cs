// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// Events.CS								Presto, March '99 
//
//	Some standard events people will want to keep track of.
//
//	These are the common events I have defined.  I'm sure there are more, 
//	but they'll eventually get added.  The only problem with introducing
//	events right now is that if they rev the server code (it's at version
//	1.3 right now) they might change some of the functions I had to
//	replace below.  That means that when the version is updated you
//	have to get a new EVENTS.CS if one is available!
//
//	I defined the following events:
//
//		flag eventClientMessage(%client, %msg); 
//			A common event which indicates a client message arrived.
//			Return "mute" to avoid displaying the message.
//		flag eventExtendedClientMessage(%client, %msg, %text); 
//			Normal client messages chop off the ~ part at the end,
//			this one still includes it.  The %text is the message
//			stripped of the ~ and beyond.
//			Return "mute" to avoid displaying the message.
// 		eventChangeMission(%nextMission)
//			The server is changing the mission.
// 		eventExit
//			The player just quit.
// 		eventClientJoin(%client)
//			A player just joined the server.
// 		eventClientDrop(%client)
//			A player just dropped from the server.
// 		eventClientChangeTeam(%client, %team)
//			A player changed teams.
// 		eventConnectionAccepted
//			Your connection to the server was accepted.
// 		eventConnectionRejected(%reason)
//			Your connection to the server was rejected.
// 		eventConnectionLost
//			Your connection to the server was lost.
// 		eventConnectionTimeout
//			Your attempted connection to the server timed out.
// 		eventConnected
//			This is the point at which server communication is 
//			established; you are "connected"
//		eventGuiOpen(%gui)
//		eventGuiClose(%gui)
//			This tracks the opening and closing of a particular
//			GUI - for instance the playscreen or the inventory
//			or the commander screen.
//
// ---------------------------------------------------------------------------
Include("presto\\Event.cs"); 

// ----------------------------------------------------------------------------------------
// flag eventClientMessage(%client, %msg); 
// flag eventExtendedClientMessage(%client, %msg);
//	return false to avoid display
//	Extended client messages include the ~ part at the end, you probably don't 
//	need this.
// ----------------------------------------------------------------------------------------
function onClientMessage(%client, %msg)
{
   if(%client)
      $lastClientMessage = %client;

   // Chop off the extended tags.
   %idx = String::FindSubStr(%msg, "~");
   if (%idx != -1) {
	%text = String::GetSubStr(%msg, %idx+1, 10000);
	%short = String::GetSubStr(%msg, 0, %idx);

	while (%text != "") {
		if (String::GetSubStr(%text, 0,1) == "w")
			break;
		%idx = String::FindSubStr(%text, "~");
		if (%idx == -1) {
			%str = %text;
			%text = "";
			}
		else	{
			%str = String::GetSubStr(%text, 0,%idx);
			%text = String::GetSubStr(%text, %idx+1, 10000);
			}

		%idx = String::FindSubStr(%str, ":");
		if (%idx == -1)
			Event::Trigger(eventClientTagMessage, %client, %str);
		else	Event::Trigger(eventClientTagMessage, %client,
					   String::GetSubStr(%str, 0,%idx),
					   String::GetSubStr(%str, %idx+1, 10000));
		}
	}
   else %short = %msg;

   %returnsExtended = Event::Trigger(eventExtendedClientMessage, %client, %msg, %short);
   %returns = Event::Trigger(eventClientMessage, %client, %short);
   if (Event::Returned(%returns, mute) || Event::Returned(%returnsExtended, mute))
	return false;
   return true;
}

// ----------------------------------------------------------------------------------------
// eventChangeMission(%nextMission)
// ----------------------------------------------------------------------------------------
function remoteMissionChangeNotify(%serverManagerId, %nextMission)
{
   if(%serverManagerId == 2048)
   {
	Event::Trigger(eventChangeMission, %nextMission);

      echo("Server mission complete - changing to mission: ", %nextMission);
      echo("Flushing Texture Cache");
      flushTextureCache();
      schedule("purgeResources(true);", 3);
   }
}

// ----------------------------------------------------------------------------------------
// eventClientJoin(%client)
// eventClientDrop(%client)
// eventClientChangeTeam(%client, %team)
// ----------------------------------------------------------------------------------------
function onClientJoin(%client) {
	Event::Trigger(eventClientJoin, %client);
	}
function onClientDrop(%client) {
	Event::Trigger(eventClientDrop, %client);
	}
function onClientChangeTeam(%client, %team) {
	Event::Trigger(eventClientChangeTeam, %client, %team);
	}

// ----------------------------------------------------------------------------------------
// eventExit
// ----------------------------------------------------------------------------------------
function onExit()
{
   Event::Trigger(eventExit);

   if(isObject(playGui))
      storeObject(playGui, "config\\play.gui");

   saveActionMap("config\\config.cs", "actionMap.sae", "playMap.sae", "pdaMap.sae");

	//update the video mode - since it can be changed with alt-enter
	$pref::VideoFullScreen = isFullScreenMode(MainWindow);

   checkMasterTranslation();
	echo("exporting pref::* to prefs.cs");
   export("pref::*", "config\\ClientPrefs.cs", False);
   export("Server::*", "config\\ServerPrefs.cs", False);
   export("pref::lastMission", "config\\ServerPrefs.cs", True);
   BanList::export("config\\banlist.cs");
}

// ----------------------------------------------------------------------------------------
// eventConnectionAccepted
// eventConnectionRejected(%reason)
// eventConnectionLost
// eventConnectionTimeout
// ----------------------------------------------------------------------------------------
function onConnection(%message)
{
   echo("Connection ", %message);
   $dataFinished = false;
   if(%message == "Accepted")
   {
      resetSimTime();
		//execute the custom script
		if ($PCFG::Script != "")
		{
			exec($PCFG::Script);
		}

      resetPlayDelegate();
      remoteEval(2048, "SetCLInfo", $PCFG::SkinBase, $PCFG::RealName, $PCFG::EMail, $PCFG::Tribe, $PCFG::URL, $PCFG::Info, $pref::autoWaypoint, $pref::noEnterInvStation, $pref::messageMask);

		if ($Pref::PlayGameMode == "JOIN")
		{
			cursorOn(MainWindow);
	      GuiLoadContentCtrl(MainWindow, "gui\\Loading.gui");
			renderCanvas(MainWindow);
		}
		Event::Trigger(eventConnectionAccepted);
   }
   else if(%message == "Rejected")
   {
		Quickstart();
      $errorString = "Connection to server rejected:\n" @ $errorString;
      GuiPushDialog(MainWindow, "gui\\MessageDialog.gui");
		schedule("Control::setValue(MessageDialogTextFormat, $errorString);", 0);
		Event::Trigger(eventConnectionRejected);
   }
   else
   {
      //startMainMenuScreen();
		Quickstart();

      if(%message == "Dropped")
      {
         if($errorString == "")
            $errorString = "Connection to server lost:\nServer went down.";
         else
            $errorString = "Connection to server lost:\n" @ $errorString;

	   Event::Trigger(eventConnectionLost, $errorString);

         GuiPushDialog(MainWindow, "gui\\MessageDialog.gui");
		   schedule("Control::setValue(MessageDialogTextFormat, $errorString);", 0);
      }
      else if(%message == "TimedOut")
      {
         $errorString = "Connection to server timed out.";
         GuiPushDialog(MainWindow, "gui\\MessageDialog.gui");
		   schedule("Control::setValue(MessageDialogTextFormat, $errorString);", 0);
	   Event::Trigger(eventConnectionTimeout);
      }
   }
}

function dataFinished()
{
   // called on client when all the dynamic data has
   // finished transfer.

        if ($cdMusic && !$pref::userCDOverride)
        {
                rbSetPlayMode (CD, 0);
                rbStop (CD);
        }
   Control::setValue(ProgressText, "<jc><f0>Get ready to rock n' roll!");
   Control::setValue(ProgressSlider, 0.9);

   $dataFinished = true;
   remoteEval(2048, dataFinished);

	Event::Trigger(eventConnected);

   echo("Flushing Texture Cache");
   flushTextureCache();
}

//There are a lot of GUIs.  So that I don't step on too much 
// (potential in new versions) code I'm only doing these ones
// so far.
function CmdInventoryGui::onOpen()
{
   if($curFavorites == -1)
   {
      CmdInventoryGui::favoritesSel(1);
      Control::performClick("FavButton1");
   }
	Event::Trigger(eventGuiOpen, CmdInventoryGui);
}
function CmdInventoryGui::onClose() {
	Event::Trigger(eventGuiClose, CmdInventoryGui);
}
function PlayGui::onOpen() {
	Event::Trigger(eventGuiOpen, PlayGui);
}
function PlayGui::onClose() {
	Event::Trigger(eventGuiClose, PlayGui);
}
function CommandGui::onOpen()
{
        //initialize the commander buttons

        if ($pref::mapFilter & 0x0001) Control::setValue(IDCTG_CMDO_PLAYERS, "TRUE");
        else Control::setValue(IDCTG_CMDO_PLAYERS, "FALSE");

        if ($pref::mapFilter & 0x0002) Control::setValue(IDCTG_CMDO_TURRETS, "TRUE");
        else Control::setValue(IDCTG_CMDO_TURRETS, "FALSE");

        if ($pref::mapFilter & 0x0004) Control::setValue(IDCTG_CMDO_ITEMS, "TRUE");
        else Control::setValue(IDCTG_CMDO_ITEMS, "FALSE");

        if ($pref::mapFilter & 0x0008) Control::setValue(IDCTG_CMDO_OBJECTS, "TRUE");
        else Control::setValue(IDCTG_CMDO_OBJECTS, "FALSE");

        if (String::ICompare($pref::mapSensorRange, "TRUE") == 0)
		 Control::setValue(IDCTG_CMDO_RADAR, "TRUE");
        else Control::setValue(IDCTG_CMDO_RADAR, "FALSE");

        if (String::ICompare($pref::mapNames, "TRUE") == 0)
		 Control::setValue(IDCTG_CMDO_EXTRA, "TRUE");
        else Control::setValue(IDCTG_CMDO_EXTRA, "FALSE");

	   Control::setValue("TVButton", false);
	Event::Trigger(eventGuiOpen, CommandGui);
	}
function CommandGui::onClose() {
	Event::Trigger(eventGuiClose, CommandGui);
}
// ----------------------------------------------------------------------------------------

