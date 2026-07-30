// ---------------------------------------------------------------------------
// events.cs -- Version 1.6 -- February 18, 2001
// by Lorne Laliberte (writer@planetstarsiege.com)
//
// Based on Events.cs by Steve Eisner (Presto).
//
// I've redesigned some of the original code to improve efficiency, and added
// some new features and events.
//
// Thanks to MrPoop for the idea to trigger an eventServerMessage event.
//
// Overridden functions are based on Tribes version 1.10.5.
//
// http://www.writerscripts.com
// ---------------------------------------------------------------------------

include("writer\\version.cs");
version("writer\\events.cs", "1.6", "Lorne Laliberte", "Feb 18, 2001");


// eventServerMessage(%msg, %repeated)
//
//      Triggered whenever a message arrives that originated from the server,
//      with anything after a ~ stripped from the message (i.e. no tail).
//
//      %repeated will be true if the same message was issued less than 0.5 seconds earlier
//
//      Return "mute" to avoid displaying the message.
//
// eventClientMessage(%client, %msg, %repeated)
//
//      Triggered whenever a client message arrives, with anything after a ~
//      stripped from the message (i.e. no tail).
//
//      %client is the originator of the message (0 if server)
//      %repeated will be true if the same message was issued less than 0.5 seconds earlier
//
//      Return "mute" to avoid displaying the message.
//
// eventExtendedClientMessage(%client, %long, %msg, %repeated)
//
//      Triggered whenever a client message arrives. %msg is the same string
//      that gets passed to eventClientMessage. %long is the full message with
//      nothing removed.
//
//      %client is the originator of the message (0 if server)
//      %repeated will be true if the same message was issued less than 0.5 seconds earlier
//
//      Return "mute" to avoid displaying the message.
//
// eventClientTagMessage(%client, %tag, %value, %repeated)
//
//      Triggered once for each tag found in the message, whenever a client
//      message with a tail (anything after a ~) arrives.  %tag is the text
//      between a ~ and a :, and %value is the part found after a :.
//      Example: "hello~job:done" -- "job" is the %tag, "done" is the value
//
//      Only triggered if a function is attached.
//
//      %client is the originator of the message (0 if server)
//      %repeated will be true if the same message was issued less than 0.5 seconds earlier
//
//      Return "mute" to avoid displaying the message this tag was found in.
//
// eventOtherPlayerMessage(%client, %msg, %repeated)
//
//      Triggered whenever a message arrives from another player, with anything after a ~
//      stripped from the message (i.e. no tail).
//
//      %client is the originator of the message
//      %repeated will be true if the same message was issued less than 0.5 seconds earlier
//
//      Return "mute" to avoid displaying the message.
//
// eventChangeMission(%nextMission)
//
//      Triggered when the server changes the mission.
//
// eventMissionInfo(%server, %missionName, %missionType)
//
//      Triggered when the server offers the current mission name and type
//      (CTF, D&D, etc.)
//
// eventServerInfo(%server, %version, %hostname, %mod, %info, %favKey)
//
//      Triggered when you learn about the server you've connected to.
//
// eventServerMod(%modname)
//
//      Triggers after eventServerInfo, once for each word in the $ServerMod
//      string (including "base").
//
// eventMod_<modname>
//
//      This event is triggered once for each word in the $ServerMod string.
//      Use this instead of eventServerMod when you want to execute a function
//      only when a server is running a specific mod (including "base").
//
// eventClientTeamAdd(%team, %name)
//
//      Triggered when a team is added.
//
// eventClientJoin(%client)
//
//      Triggered whenever a player joins the server.
//
// eventClientDrop(%client)
//
//      Triggered whenever a player leaves the server.
//
// eventClientChangeTeam(%client, %team)
//
//      Triggered whenever a player changes teams.
//
// eventConnectionAccepted
//
//     Triggered once your connection to the server has been accepted.
//
// eventConnectionRejected(%reason)
//
//     Triggered if your connection to the server was rejected.
//
// eventConnectionLost(%reason)
//
//     Triggered if your connection to the server was lost.
//
// eventConnectionTimeout
//
//     Triggered if your attempt to connect to the server timed out.
//
// eventConnected
//
//     Triggered at the point where server communication is
//     established; you are officially "connected."
//
// eventGuiOpen(%gui)
//
//      Triggered whenever a gui opens. %gui is the name of the gui.
//
// eventGuiClose(%gui)
//
//      Triggered whenever a gui closes. %gui is the name of the gui.
//
// eventInventoryMode
//
//      Triggered whenever you switch to the inventory screen.
//
// eventCommandMode
//
//      Triggered whenever you switch to the command screen.
//
// eventPlayMode
//
//      Triggered whenever you switch to the play screen.
//
// eventScreenModeChanged
//
//      Triggered when you return to play mode after the screen mode has
//      changed.
//
// eventFullScreenMode
//
//      Triggered when you return to play mode after the screen mode has
//      changed from software mode to full screen mode.
//
// eventSoftwareMode
//
//      Triggered when you return to play mode after the screen mode has
//      changed from full screen mode to software mode.
//
// eventScreenSizeChanged
//
//      Triggered when you return to play mode after the full-screen resolution
//      has changed.
//
// eventObjectivesMode
//
//      Triggered whenever the Objectives screen is displayed.
//
// eventOptionsScreenOpened
//
//      Triggered whenever you switch to the options screen.
//
// eventOptionsScreenClosed
//
//      Triggered whenever you exit the options screen.
//
// eventExit
//
//      Triggered when the player has quit.
//
// eventQuit
//
//      Triggered when the player clicks on the Quit button.
//
//      Return "mute" to override -- i.e. to not quit.
//
//      Return "noconfirm" to quit immediately, skipping the
//      "Are you sure?" confirmation dialog.
//
// eventSaveConfig
//
//      Triggered whenever Tribes is about to save the config.
//
//      Return "nosave" to prevent the config data from being saved.
//      Note: returning "nosave" also prevents eventSavePlayGui and
//            eventExportPrefs
//
// eventSavePlayGui
//
//      Triggered whenever Tribes is about to save the playGui file.
//
//      Return "nosave" to prevent the playGui from being saved.
//
// eventExportPrefs
//
//      Triggered whenever Tribes is about to save the preferences files
//      (prefs.cs, ClientPrefs.cs, ServerPrefs.cs, and banlist.cs)
//
//      Return "nosave" to prevent these files from being saved.
//
// eventRemoteITXT
//
//      Triggered whenever you access a station.
//
//      Returns the "STATION ENERGY:" or "TEAM ENERGY:" text being sent from
//      the server.
//
// eventRemoteInventoryStation
//
//      Triggered whenever you access a remote inventory station.
//
// eventUpdateTime(%min, %sec)
//
//      Triggered at :00, :20, and :40 seconds each minute, counting down
//      the time remaining in the current mission.
//
// ---------------------------------------------------------------------------

include("presto\\Event.cs");
include("presto\\schedule.cs");

$Events::VideoFullScreen = $pref::VideoFullScreen;
$Events::VideoFullScreenRes = $pref::VideoFullScreenRes;

// Check for messages repeated within .5 seconds to ignore stuff like ELF spam
function CheckRepeatedMessages(%msg)
{
    if(%msg == $lastServerMessage)
        return true;

   $lastServerMessage = %msg;
   schedule::add("$lastServerMessage = \"!\";", 0.5, checkrepeatedmessages);
   return false;
}

// ---------------------------------------------------------------------------
// eventServerMessage(%client, %msg, %repeated);
// eventClientMessage(%client, %msg, %repeated);
// eventExtendedClientMessage(%client, %long, %msg, %repeated);
// eventClientTagMessage(%client, %tag, %value, %repeated);
// eventOtherPlayerMessage(%client, %msg, %repeated);
// ---------------------------------------------------------------------------
function onClientMessage(%client, %msg)
{
    %repeated = false;

    if(%client) // message is from another client (not the server)
        $lastClientMessage = %client;
    else if(CheckRepeatedMessages(%msg))
        %repeated = true;

    %muted = false;

    if((%index = String::findSubStr(%msg, "~")) != -1)
    {
        %text = String::getSubStr(%msg, %index + 1, 10000);
        %short = String::getSubStr(%msg, 0, %index);

        if($Event::count[eventClientTagMessage])
        {
            while(%text != "")
            {
                if(String::getSubStr(%text, 0, 1) == "w")
                    break;

                if((%index = String::findSubStr(%text, "~")) == -1)
                {
                    %tag = %text;
                    %text = "";
                }
                else
                {
                    %tag = String::getSubStr(%text, 0, %index);
                    %text = String::getSubStr(%text, %index + 1, 10000);
                }

                if((%index = String::FindSubStr(%tag, ":")) == -1)
                {
                    %value = "";
                }
                else
                {
                    %value = String::GetSubStr(%tag, %index+1, 10000);
                    %tag = String::GetSubStr(%tag, 0,%index);
                }

                Event::Trigger(eventClientTagMessage, %client, %tag, %value, %repeated);
                %muted = %muted || Event::Returned(eventClientTagMessage, mute);
            }
        }
    }
    else %short = %msg;

    if(!%client) // message is from the server
    {
        // Check for "Insufficient mana" message and handle AutoCast meditation
        if(String::findSubStr(%short, "Insufficient mana to cast this spell") != -1)
        {
            // Only handle if AutoCast is active
            if($DeusRPG::AutoCasting != "")
            {
                echo("AutoCast: Detected insufficient mana message from server");
                AutoCastHandleInsufficientMana();
            }
        }
        
        Event::Trigger(eventServerMessage, %short, %repeated);
        %muted = %muted || Event::Returned(eventServerMessage, mute);
    }
    else if(%client != getManagerID()) // message is from another player
    {
        Event::Trigger(eventOtherPlayerMessage, %short, %repeated);
        %muted = %muted || Event::Returned(eventOtherPlayerMessage, mute);
    }

    Event::Trigger(eventExtendedClientMessage, %client, %msg, %short, %repeated);
    %muted = %muted || Event::Returned(eventExtendedClientMessage, mute);

    Event::Trigger(eventClientMessage, %client, %short, %repeated);
    %muted = %muted || Event::Returned(eventClientMessage, mute);

    return !%muted;
}

// ---------------------------------------------------------------------------
// eventChangeMission(%nextMission)
// ---------------------------------------------------------------------------
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


// ---------------------------------------------------------------------------
// eventMissionInfo(%server, %missionName, $ServerMissionType)
// ---------------------------------------------------------------------------
function remoteMInfo(%server, %missionName)
{
    if(%server == 2048)
    {
        %file = "missions\\" @ %missionName @ ".dsc";
        $MDESC::Type = "";
        $MDESC::Text = "";
        if(File::findFirst(%file) != "")
            exec(%file);
        $ServerMission = %missionName;
        $ServerText = $MDESC::Text;
        $ServerMissionType = $MDESC::Type;
        if(isObject(LobbyGui))
            LobbyGui::onOpen();  // update lobby screen text.
    }

	Event::Trigger(eventMissionInfo, %server, %missionName, $ServerMissionType);
}


// ---------------------------------------------------------------------------
// eventServerInfo(%server, %version, %hostname, %mod, %info, %favKey)
// eventServerMod(%modname)
// eventMod_<modname>
// ---------------------------------------------------------------------------
function remoteSVInfo(%server, %version, %hostname, %mod, %info, %favKey)
{
    if(%server == 2048)
    {
        $ServerVersion = %version;
        $ServerName = %hostname;

        $modList = %mod;
        $ServerMod = $modList;
        $ServerInfo = %info;
        $ServerFavoritesKey = %favKey;

		Event::Trigger(eventServerInfo, %server, %version, %hostname, %mod, %info, %favKey);

        %i = 0;
        while((%modname = getWord(%mod, %i)) != -1)
        {
            Event::Trigger(eventServerMod, %modname);
            Event::Trigger(eventMod_ @ %modname);
            %i++;
        }

        EvalSearchPath();
    }
}


// ---------------------------------------------------------------------------
// eventClientTeamAdd(%team, %name)
// eventClientJoin(%client)
// eventClientDrop(%client)
// eventClientChangeTeam(%client, %team)
// ---------------------------------------------------------------------------
function onTeamAdd(%team, %name)
{
    Event::Trigger(eventClientTeamAdd, %team, %name);
}

function onClientJoin(%client)
{
    Event::Trigger(eventClientJoin, %client);
}

function onClientDrop(%client)
{
    Event::Trigger(eventClientDrop, %client);
}

function onClientChangeTeam(%client, %team)
{
    Event::Trigger(eventClientChangeTeam, %client, %team);
}


// ---------------------------------------------------------------------------
// eventQuit -- newInterface compatible :)
// ---------------------------------------------------------------------------
function onQuit()
{
    Event::Trigger(eventQuit);

    if(Event::returned(eventQuit, mute))
        return;

    if($pref::ConfirmQuit && !Event::returned(eventQuit, noconfirm))
        GuiPushDialog(MainWindow, "gui\\Exit.gui");
    else
        quit();
}


// ---------------------------------------------------------------------------
// eventExit
// eventSaveConfig
// eventSavePlayGui
// eventExportPrefs
// ---------------------------------------------------------------------------
function onExit()
{
	Event::trigger(eventExit);

	if(Event::returned(Event::trigger(eventSaveConfig), nosave))
		return;

	if(!Event::returned(Event::trigger(eventSavePlayGui), nosave))
	{
		if(isObject(playGui))
		{
			cleanuprepackhuds();
			RemoveClickFix(playGui);
			if(String::findSubStr($modList, "rmrpg") == -1)
				storeObject(playGui, "config\\play.gui");
		}
	}
	setRepackJump(False);
	if(String::findSubStr($modList, "crurpg") != -1)
		saveActionMap("config\\crurpg_config.cs", "actionMap.sae", "playMap.sae", "pdaMap.sae");
	else
		saveActionMap("config\\config.cs", "actionMap.sae", "playMap.sae", "pdaMap.sae");

	$pref::VideoFullScreen = isFullScreenMode(MainWindow);
	if(!$pref::VideoFullScreen)
	{
		if(isObject(playGui))
			$pref::softwareRes = Presto::ScreenSize();
	}
	revertDrawDistance();

	if($pref::StartAlwaysFullScreen)
		$pref::VideoFullScreen = true; // set fullscreenmode to true for next start
	else
		$pref::VideoFullScreen = isFullScreenMode(MainWindow); //update the video mode - since it can be changed with alt-enter

	checkMasterTranslation();

	if(Event::returned(Event::Trigger(eventExportPrefs), nosave))
		return;

	echo("exporting pref::* to prefs.cs");
	export("pref::*", "config\\ClientPrefs.cs", False);
	export("Server::*", "config\\ServerPrefs.cs", False);
	export("pref::lastMission", "config\\ServerPrefs.cs", True);
	BanList::export("config\\banlist.cs");
}


// ---------------------------------------------------------------------------
// eventConnectionAccepted
// eventConnectionRejected(%reason)
// eventConnectionLost(%reason)
// eventConnectionTimeout
// ---------------------------------------------------------------------------

function onConnection(%message)
{
	echo("Connection ", %message);
	$dataFinished = false;
	if(%message == "Accepted")
	{
		resetSimTime();
		//execute the custom script
		if ($PCFG::Script != "")
			exec($PCFG::Script);

		resetPlayDelegate();
		remoteEval(2048, "SetCLInfo", $PCFG::SkinBase, $PCFG::RealName, $PCFG::EMail, $PCFG::Tribe, $PCFG::URL, $PCFG::Info, $pref::autoWaypoint, $pref::noEnterInvStation, $pref::messageMask, $repackVer);

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
		Quickstart(true);
		$errorString = "Connection to server rejected:\n" @ $errorString;
		GuiPushDialog(MainWindow, "gui\\MessageDialog.gui");
		schedule("Control::setValue(MessageDialogTextFormat, $errorString);", 0);
		Event::Trigger(eventConnectionRejected, $errorString);
	}
	else
	{
		if($rpgSpaceKeyboardLockOn)
			setRepackJump(False, True);
		$rLoadedWorld = "";
		$rLoadedPal = "";
		$repackKeyOverride = "";
		if($errorString == "")
		{
			if($joinWorlds){
				$joinWorlds=2;
				Quickstart(true);//Send them to the player select screen temporarily
				schedule("$joinWorlds=1;Quickstart(false);",0.2);
			}else {
				Quickstart(true);//Send them to the server list screen temporarily
				$recon::addr = $server::address;
				schedule("autoReconnect();",0.2);
			}
			return;
		}
		//startMainMenuScreen();
		Quickstart(true);

		if(%message == "Dropped" || %message == "dropped")
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


// ---------------------------------------------------------------------------
// eventConnected
// ---------------------------------------------------------------------------
function dataFinished()
{
    // called on client when all the dynamic data has finished transfer

    if ($cdMusic && !$pref::userCDOverride)
    {
        if(!Event::Returned(Event::Trigger(eventStopTribesMusic), mute))
        {
            rbSetPlayMode(CD, 0);
            rbStop(CD);
        }
    }
    Control::setValue(ProgressText, "<jc><f0>Get ready to rock n' roll!");
    Control::setValue(ProgressSlider, 0.9);

    $dataFinished = true;
    remoteEval(2048, dataFinished);

    Event::Trigger(eventConnected);

    echo("Flushing Texture Cache");
    flushTextureCache();
}


// ---------------------------------------------------------------------------
// eventStopTribesMusic
// eventChangeTribesMusic(%track, %playmode)
// ---------------------------------------------------------------------------
function remoteSetMusic (%player, %track, %mode)
{
    if(%player == 2048)
    {
        $cdPlayMode = %mode;
        $cdTrack = %track;
        if(!$pref::userCDOverride)
        {
            if ($cdTrack == 0)
            {
                if(!Event::Returned(Event::Trigger(eventStopTribesMusic), mute))
                {
                    rbSetPlayMode (CD, 0);
                    rbStop (CD);
                }
            }
            else if($cdTrack != "")
            {
                Event::Trigger(eventChangeTribesMusic, $cdTrack, $cdPlayMode);

                if(!Event::Returned(eventChangeTribesMusic, mute))
                {
                    rbSetPlayMode (CD, 0);
                    rbStop (CD);
                    rbSetPlayMode (CD, $cdPlayMode);
                    if($pref::cdMusic)
                        rbPlay (CD, $cdTrack);
                }
            }
        }
    }
}


// ---------------------------------------------------------------------------
// eventGuiOpen(%gui)
// eventGuiClose(%gui)
// eventCommandMode
// eventInventoryMode
// eventPlayMode
// eventOptionsScreenOpened
// eventOptionsScreenClosed
// eventScreenModeChanged
// eventFullScreenMode
// eventSoftwareMode
// eventScreenSizeChanged
// ---------------------------------------------------------------------------
function OpenAGui(%gui)
{
    Event::Trigger(eventGuiOpen, %gui);

    // variable defined in the PrestoPack, which includes a fix for a Tribes bug
    if($needsClickFix)
        ClickFix(%gui);
}


function CmdInventoryGui::onOpen()
{
    if($curFavorites == -1)
    {
        CmdInventoryGui::favoritesSel(1);
        Control::performClick("FavButton1");
    }

    // [Presto:]
    // THIS IS REALLLLLLLLLLLLLLLY WEIRD.  When you push a favorites button
    // it automatically makes visible the last element of the CmdInventoryGui
    // set.  That means that any hidden dialogs automatically pop up!
    //
    // So, to fix this, I always add an unobtrusive object as the last item
    // in the list!
    // [/Presto]

    OpenAGui(CmdInventoryGui);

    $Mode::InventoryMode = true;
    $Mode::CommandMode =
    $Mode::ObjectivesMode =
    $Mode::OptionsMode =
    $Mode::PlayMode = false;

    Event::Trigger(eventInventoryMode);

    Event::Trigger(eventGuiOpen_CmdInventoryGui);


    if($Mode::ExitInventory)
    {
        $Mode::ExitInventory = false;
        remoteEval(2048, PlayMode);
    }
}


function CmdInventoryGui::onClose()
{
    $Mode::InventoryMode = false;
    Event::Trigger(eventGuiClose, CmdInventoryGui);
    Event::Trigger(eventGuiClose_CmdInventoryGui);
}


function PlayGui::onOpen()
{
    OpenAGui(PlayGui);

    $Mode::CommandMode =
    $Mode::InventoryMode =
    $Mode::ObjectivesMode =
    $Mode::OptionsMode = false;
    $Mode::PlayMode = true;

    Event::Trigger(eventPlayMode);
    Event::Trigger(eventGuiOpen_PlayGui);

    // Trigger events when screen mode or size changes
    if($pref::VideoFullScreen != $Events::VideoFullScreen)
    {
        Event::Trigger(eventScreenModeChanged);
        if($pref::VideoFullScreen)
            Event::Trigger(eventFullScreenMode);
        else
            Event::Trigger(eventSoftwareMode);
    }
    else if($pref::VideoFullScreen && ($pref::VideoFullScreenRes != $Events::VideoFullScreenRes))
    {
        Event::Trigger(eventScreenSizeChanged, $pref::VideoFullScreenRes);
    }

    $Events::VideoFullScreen = $pref::VideoFullScreen;
    $Events::VideoFullScreenRes = $pref::VideoFullScreenRes;
}


function PlayGui::onClose()
{
    Event::Trigger(eventGuiClose, PlayGui);
    Event::Trigger(eventGuiClose_PlayGui);
    $Mode::PlayMode = false;
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

    if (String::ICompare($pref::mapSensorRange, "TRUE") == 0) Control::setValue(IDCTG_CMDO_RADAR, "TRUE");
    else Control::setValue(IDCTG_CMDO_RADAR, "FALSE");

    if (String::ICompare($pref::mapNames, "TRUE") == 0) Control::setValue(IDCTG_CMDO_EXTRA, "TRUE");
    else Control::setValue(IDCTG_CMDO_EXTRA, "FALSE");

    Control::setValue("TVButton", false);

    OpenAGui(CommandGui);

    $Mode::CommandMode = true;
    $Mode::InventoryMode =
    $Mode::ObjectivesMode =
    $Mode::OptionsMode =
    $Mode::PlayMode = false;

    Event::Trigger(eventCommandMode);
    Event::Trigger(eventGuiOpen_CommandGui);

    if($Mode::ExitCommand)
    {
        $Mode::ExitCommand = false;
        remoteEval(2048, PlayMode);
    }
}

function CommandGui::onClose()
{
    Event::Trigger(eventGuiClose, CommandGui);
    Event::Trigger(eventGuiClose_CommandGui);
    $Mode::CommandMode = false;
}

function OptionsGui::onOpen()
{
    $Events::VideoFullScreen = $pref::VideoFullScreen;

    IRCOptions::init();

    OptionsMovement::init();
    OptionsVideo::init();
    OptionsGraphics::init();
    OptionsNetwork::init();
    OptionsSound::init();

    OpenAGui(OptionsGui);

    if(!$Mode::OptionsMode)
    {
        $Mode::OptionsMode = true;
        $Mode::CommandMode =
        $Mode::InventoryMode =
        $Mode::ObjectivesMode =
        $Mode::PlayMode = false;

        Event::Trigger(eventOptionsScreenOpened);
        Event::Trigger(eventGuiOpen_OptionsGui);
    }
}

function OptionsGui::onClose()
{
    Event::Trigger(eventGuiClose, OptionsGui);

    IRCOptions::Shutdown();

    OptionsNetwork::shutdown();

    $Mode::OptionsMode = false;

    Event::Trigger(eventOptionsScreenClosed);
    Event::Trigger(eventGuiClose_OptionsGui);
}

function CmdObjectivesGui::onOpen()
{
    OpenAGui(CmdObjectivesGui);

    $Mode::ObjectivesMode = true;
    $Mode::CommandMode =
    $Mode::InventoryMode =
    $Mode::ObjectivesMode =
    $Mode::OptionsMode =
    $Mode::PlayMode = false;

    Event::Trigger(eventObjectivesMode);
    Event::Trigger(eventGuiOpen_CmdObjectivesGui);
}

function CmdObjectivesGui::onClose()
{
    Event::Trigger(eventGuiClose, CmdObjectivesGui);
    Event::Trigger(eventGuiClose_CmdObjectivesGui);
    $Mode::ObjectivesMode = false;
}

// thanks to AKAPanamaJack for suggesting this use of the remoteITXT function :)
function remoteITXT(%manager, %msg)
{
    if(%manager == 2048)
    {
        Control::setValue(EnergyDisplayText, %msg);
        Event::Trigger(eventRemoteITXT, %msg);
        if(String::findSubStr(%msg, "STATION ENERGY") != -1)
            Event::trigger(eventRemoteInventoryStation);
    }
}

// ---------------------------------------------------------------------------
// eventUpdateTime(%min, %sec)
// ---------------------------------------------------------------------------
// remoteSetTime override courtesy of Andrew Mather - ]|sh|[ Ripley - anmat@git.com.au
//
// I've made only minor modifications to his code.
// His comments follow:

// If you have problems with the timer being out 1 second type $debug_time_event = true;
// at the console. The console will then print two lines, three times a minute. Add the
// bottom two times together, minutes and seconds, and add the tweak time and they should
// equal the top number. If not then adjust the $tweaktime value from 0.50 to 0.25 or
// 0.75 . This should not be necessary but it will work. I think its what they call float
// inaccuracy ?

$debug_time_event = false;
$tweakTime = 0.50;

function remoteSetTime(%server, %time)
{
    if(%server == 2048)
        setHudTimer(%time);

    if($debug_time_event)
    {
        %tempLogMode = $Console::LogMode;
        $Console::LogMode = 1;
        echo ("RemainingGameTime = " @ -%time);
        $Console::LogMode = %tempLogMode;
    }

    %time = (-%time + $tweakTime);  // show time as positive
    %min = Time::getMinutes(%time); // get minutes
    %sec = Time::getSeconds(%time); // get seconds

    if(%min > 59) // one hour limit
        return;

    Event::Trigger(eventUpdateTime, %min, %sec);

    if ($debug_time_event)
    {
        $Console::LogMode = 1;
        echo("converted time = " @ %min @ " minutes " @ %sec @ " seconds");
        echo("$tweaktime = " @ $tweaktime);
        $Console::LogMode = %tempLogMode;
    }
}
