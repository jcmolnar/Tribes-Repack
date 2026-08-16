//------------------------------------
//
// GUI screen functions
//
//------------------------------------

// NATIVE: scan the WHOLE search path for skin textures matching %pattern and append each
// UNIQUE skin to $SkinArray (dedup key = base + %delim gender). Finds skins in ANY folder
// and ANY image format the repack ships (.bmp legacy + .png/.gif converted).
function Player::addSkinFiles(%pattern, %delim)
{
	%skin = File::findFirst(%pattern);
	while (%skin != "")
	{
		%idx = String::findSubStr(%skin, %delim);
		if (%idx >= 0)
		{
			%key = String::getSubStr(%skin, 0, %idx) @ %delim;
			%dup = 0;
			%i = 0;
			while (%i < $SkinCount)
			{
				%eidx = String::findSubStr($SkinArray[%i], %delim);
				if (%eidx >= 0 && ! String::ICompare(String::getSubStr($SkinArray[%i], 0, %eidx) @ %delim, %key))
					%dup = 1;
				%i = %i + 1;
			}
			if (%dup == 0)
			{
				$SkinArray[$SkinCount] = %skin;
				$SkinCount = $SkinCount + 1;
			}
		}
		%skin = File::findNext(%pattern);
	}
}

function ClearPlayerConfig()
{
	Control::setValue(IDCTG_PLYR_CFG_NAME, "");
	Control::setValue(IDCTG_PLYR_CFG_EMAIL, "");
	Control::setValue(IDCTG_PLYR_CFG_TRIBE, "");
	Control::setValue(IDCTG_PLYR_CFG_URL, "");
	Control::setValue(IDCTG_PLYR_CFG_INFO, "");
	Control::setValue(IDCTG_PLYR_CFG_SCRIPT, "");
	Control::setValue(IDCTG_PLYR_CFG_GENDER_M, "TRUE");
	Control::setValue(IDCTG_PLYR_CFG_GENDER_F, "FALSE");
	FGCombo::clear(IDCTG_PLYR_CFG_SKIN);

	$PCFG::Name			= "";
	$PCFG::RealName	= ""; 
	$PCFG::EMail		= ""; 
	$PCFG::Tribe		= ""; 
	$PCFG::URL			= ""; 
	$PCFG::Info			= ""; 
	$PCFG::Gender		= "MALE"; 
	$PCFG::Script		= ""; 
	$PCFG::Voice		= ""; 
	$PCFG::SkinBase	= ""; 
}

function PlayerSetupGui::onOpen()
{
   // Must do twice.  A dml resource could be referencing bitmaps. DMM
   purgeResources();
   purgeResources();

	//clear all the config controls to start
	ClearPlayerConfig();

	//first add all the players to the player list combo box
	$PCFG::LastPlayer = -1;
   for(%i = 0; $PCFG::Name[%i] != ""; %i++)
	{
		FGCombo::addEntry(IDCTG_PLYR_CFG_COMBO, $PCFG::Name[%i], %i);
		$PCFG::LastPlayer = %i;
	}

	//verify the current player
	if (($PCFG::CurrentPlayer == "") || ($PCFG::name[$PCFG::CurrentPlayer] == "") ||
													($PCFG::CurrentPlayer > $PCFG::LastPlayer))
	{
		if ($PCFG::LastPlayer < 0)
		{
			$PCFG::CurrentPlayer = -1;
		}
		else
		{
			$PCFG::CurrentPlayer = 0;
		}
	}

	//NATIVE: mount skin/voice asset-pack zips ONCE so voices (base\voices\*.zip) and
	//skins (base\Skins\*.zip) packed in archives become searchable -- a loose zip is
	//only a FILE until mounted. Must run BEFORE the voice + skin scans below. Skips
	//general art/misc archives (e.g. RPG\rpg_misc.zip) that could shadow base art.
	if ($AssetZipsMounted == "")
	{
		$AssetZipsMounted = 1;
		%zip = File::findFirst("*.zip");
		while (%zip != "")
		{
			if (String::findSubStr(%zip, "Skins") >= 0 || String::findSubStr(%zip, "voices") >= 0)
				File::mountVolume(%zip);
			%zip = File::findNext("*.zip");
		}
	}

	//now add all voices to the voice list box
	FGCombo::clear(IDCTG_PLYR_CFG_VOICE);
	%voiceCount = 0;
	%voiceSet = File::findFirst("*.whello.wav");
	while (%voiceSet != "")
	{
		%strIndex = String::findSubStr(%voiceSet, ".whello.wav");
		%voiceBase = String::getSubStr(%voiceSet, 0, %strIndex);

		if (%voiceBase != "")
		{
			$VoiceArray[%voiceCount] = %voiceBase;
			FGCombo::addEntry(IDCTG_PLYR_CFG_VOICE, %voiceBase, %voiceCount);
			%voiceCount = %voiceCount + 1;
		}

      %voiceSet = File::findNext("*.whello.wav");
	}

	//NATIVE: build the skin list -- EVERY armor skin across the whole search path, in any
	//image format the repack ships (legacy .bmp + converted .png/.gif). Dedup by
	//base+gender (Player::addSkinFiles, defined in RPG PlayerSetup.cs -- loaded first in
	//the rpg+rmrpg stack). Clear the array first so a longer prior open leaves no stale tail.
	%i = 0;
	while (%i < 256)
	{
		$SkinArray[%i] = "";
		%i = %i + 1;
	}
	$SkinCount = 0;
	Player::addSkinFiles("*.larmor.bmp",  ".larmor");
	Player::addSkinFiles("*.larmor.png",  ".larmor");
	Player::addSkinFiles("*.larmor.gif",  ".larmor");
	Player::addSkinFiles("*.lfemale.bmp", ".lfemale");
	Player::addSkinFiles("*.lfemale.png", ".lfemale");
	Player::addSkinFiles("*.lfemale.gif", ".lfemale");
	%skinCount = $SkinCount;

	if ($PCFG::CurrentPlayer >= 0)
	{
		ReadPlayerConfig($PCFG::CurrentPlayer);
	}
	else
	{
		OpenNewPlayerDialog();
	}
}

function OpenNewPlayerDialog()
{
	//push the dialog
   GuiPushDialog(MainWindow, "gui\\NewPlayer.gui");

	//if we don't have any players, disable the cancel button
	if ($PCFG::LastPlayer < 0)
	{
		Control::setActive(IDCTG_PLYR_CFG_CANCEL, FALSE);
	}
	else
	{
		Control::setActive(IDCTG_PLYR_CFG_CANCEL, TRUE);
	}

	//disable the done button
	Control::setActive(IDCTG_NEW_PLAYER, FALSE);
}

function PlayerSetupGui::onClose()
{
	WriteCurrentPlayerConfig();
   export("PCFG::*", "config\\Players.cs", False);
}

function ReadPlayerConfig(%cfgNum)
{
	echo("Reading player: " @ %cfgNum);

	if (%cfgNum >= 0 && %cfgNum <= $PCFG::LastPlayer)
	{
		//load the values into the gui controls
		Control::setValue(IDCTG_PLYR_CFG_NAME, $PCFG::RealName[%cfgNum]);
		Control::setValue(IDCTG_PLYR_CFG_EMAIL, $PCFG::EMail[%cfgNum]);
		Control::setValue(IDCTG_PLYR_CFG_TRIBE, $PCFG::Tribe[%cfgNum]);
		Control::setValue(IDCTG_PLYR_CFG_URL, $PCFG::URL[%cfgNum]);
		Control::setValue(IDCTG_PLYR_CFG_INFO, $PCFG::Info[%cfgNum]);
		Control::setValue(IDCTG_PLYR_CFG_Script, $PCFG::Script[%cfgNum]);

		if (String::ICompare($PCFG::Gender[%cfgNum], "MALE") == 0)
		{
			Control::setValue(IDCTG_PLYR_CFG_GENDER_M, "TRUE");
			Control::setValue(IDCTG_PLYR_CFG_GENDER_F, "FALSE");
		}
		else
		{
			Control::setValue(IDCTG_PLYR_CFG_GENDER_M, "FALSE");
			Control::setValue(IDCTG_PLYR_CFG_GENDER_F, "TRUE");
		}

		//copy the values into the current set used by FearCSDelegate.cpp
		$PCFG::Name			= $PCFG::Name[%cfgNum];
		$PCFG::RealName	= $PCFG::RealName[%cfgNum];
		$PCFG::EMail		= $PCFG::EMail[%cfgNum];
		$PCFG::Tribe		= $PCFG::Tribe[%cfgNum];
		$PCFG::URL			= $PCFG::URL[%cfgNum];
		$PCFG::Info			= $PCFG::Info[%cfgNum];
		$PCFG::Gender		= $PCFG::Gender[%cfgNum];
		$PCFG::Script		= $PCFG::Script[%cfgNum];
		
		//set the selected player in the combo box
		$PCFG::CurrentPlayer = %cfgNum;
		FGCombo::setSelected(IDCTG_PLYR_CFG_COMBO, $PCFG::CurrentPlayer);

		//make sure the voice exists
		%voiceSet = 0;
		%found = -1;
		while ($VoiceArray[%voiceSet] != "")
		{
			if (String::ICompare($VoiceArray[%voiceSet], $PCFG::Voice[%cfgNum]) == 0)
			{
				%found = %voiceSet;
		   }
			%voiceSet = %voiceSet + 1;
		}

		//see if we found the voice set
		if (%found >= 0)
		{
			$PCFG::Voice		= $PCFG::Voice[%cfgNum];
			FGCombo::setSelected(IDCTG_PLYR_CFG_VOICE, %found);
		}
		else
		{
			//find a voice set that exists
			%searchVoiceSet = $PCFG::Gender[$PCFG::CurrentPlayer] @ 1;
			%voiceSet = 0;
			%found = -1;
			while ($VoiceArray[%voiceSet] != "")
			{
				if (String::ICompare($VoiceArray[%voiceSet], %searchVoiceSet) == 0)
				{
					%found = %voiceSet;
			   }
				%voiceSet = %voiceSet + 1;
			}
			if (%found >= 0)
			{
				$PCFG::Voice[%cfgNum]	= %searchVoiceSet;
				$PCFG::Voice				= $PCFG::Voice[%cfgNum];
				FGCombo::setSelected(IDCTG_PLYR_CFG_VOICE, %found);
			}
		}

		//add the correct skins to the combo box
		FGCombo::clear(IDCTG_PLYR_CFG_SKIN);
		%skin = 0;
		%found = -1;
		while ($SkinArray[%skin] != "")
		{
			if (! String::ICompare($PCFG::Gender[$PCFG::CurrentPlayer], "MALE"))
			{
				%strIndex = String::findSubStr($SkinArray[%skin], ".larmor");
			}
			else
			{
				%strIndex = String::findSubStr($SkinArray[%skin], ".lfemale");
			}
			if (%strIndex >= 0)
			{
				%skinBase = String::getSubStr($SkinArray[%skin], 0, %strIndex);
            if(String::ICompare(%skinBase, $PCFG::SkinBase[%cfgNum]) == 0)
               %found = %skin;
				FGCombo::addEntry(IDCTG_PLYR_CFG_SKIN, %skinBase, %skin);
			}
			%skin++;
		}

		//see if we found the skin
		if (%found >= 0)
		{
			setSkinBase(%found, $PCFG::Gender[%cfgNum]);
		}
		else
		{
			//find a default skin
			%skin = 0;
			%found = -1;
			if (! String::ICompare($PCFG::Gender[$PCFG::CurrentPlayer], "MALE"))
			{
				while ($SkinArray[%skin] != "")
				{
					%strIndex = String::findSubStr($SkinArray[%skin], ".larmor");
					if (%strIndex >= 0)
					{
						%found = %skin;
					}
					%skin = %skin + 1;
				}
			}
			else
			{
				while ($SkinArray[%skin] != "")
				{
					%strIndex = String::findSubStr($SkinArray[%skin], ".lfemale");
					if (%strIndex >= 0)
					{
						%found = %skin;
					}
					%skin = %skin + 1;
				}
			}

			if (%found >= 0)
			{
				$PCFG::SkinBase[%cfgNum]	= $SkinArray[%found];
				setSkinBase(%found, $PCFG::Gender[%cfgNum]);
			}
		}
	}
}
	
function WriteCurrentPlayerConfig()
{
	if ($PCFG::CurrentPlayer >= 0 && $PCFG::CurrentPlayer <= $PCFG::LastPlayer)
	{
		//get the values from the gui controls
		$PCFG::RealName[$PCFG::CurrentPlayer]	= Control::getValue(IDCTG_PLYR_CFG_NAME);
		$PCFG::EMail[$PCFG::CurrentPlayer]		= Control::getValue(IDCTG_PLYR_CFG_EMAIL);
		$PCFG::Tribe[$PCFG::CurrentPlayer]		= Control::getValue(IDCTG_PLYR_CFG_TRIBE);
		$PCFG::URL[$PCFG::CurrentPlayer]			= Control::getValue(IDCTG_PLYR_CFG_URL);
		$PCFG::Info[$PCFG::CurrentPlayer]		= Control::getValue(IDCTG_PLYR_CFG_INFO);
		$PCFG::Script[$PCFG::CurrentPlayer]		= Control::getValue(IDCTG_PLYR_CFG_SCRIPT);

		//Gender
		if (String::ICompare(Control::getValue(IDCTG_PLYR_CFG_GENDER_M), "TRUE") == 0)
		{
			$PCFG::Gender[$PCFG::CurrentPlayer]	= "MALE";
		}
		else
		{
			$PCFG::Gender[$PCFG::CurrentPlayer]	= "FEMALE";
		}

		//voice
		$PCFG::Voice[$PCFG::CurrentPlayer]		= $VoiceArray[FGCombo::getSelected(IDCTG_PLYR_CFG_VOICE)];

		//skin
		$PCFG::SkinBase[$PCFG::CurrentPlayer]		= $PCFG::SkinBase;

		//update the set of vars used by FearCSDelegate
		$PCFG::Name			= $PCFG::Name[$PCFG::CurrentPlayer];
		$PCFG::RealName	= $PCFG::RealName[$PCFG::CurrentPlayer];
		$PCFG::EMail		= $PCFG::EMail[$PCFG::CurrentPlayer];
		$PCFG::Tribe		= $PCFG::Tribe[$PCFG::CurrentPlayer];
		$PCFG::URL			= $PCFG::URL[$PCFG::CurrentPlayer];
		$PCFG::Info			= $PCFG::Info[$PCFG::CurrentPlayer];
		$PCFG::Gender		= $PCFG::Gender[$PCFG::CurrentPlayer];
		$PCFG::Script		= $PCFG::Script[$PCFG::CurrentPlayer];
		$PCFG::Voice		= $PCFG::Voice[$PCFG::CurrentPlayer];
		$PCFG::SkinBase 	= $PCFG::SkinBase[$PCFG::CurrentPlayer];
	}
}
	
function SelectedPlayerConfig()
{
	WriteCurrentPlayerConfig();
	ReadPlayerConfig(FGCombo::getSelected(IDCTG_PLYR_CFG_COMBO));
}

function AddPlayerConfig(%newPlayer)
{
	WriteCurrentPlayerConfig();
	if (%newPlayer != "")
	{
		//first search, see if the player name already exists
		%found = -1;
		%plyr = 0;
		while (%plyr <= $PCFG::lastPlayer)
		{
			if (! String::ICompare($PCFG::Name[%plyr], %newPlayer))
			{
				%found = %plyr;
			}
			%plyr = %plyr + 1;
		}

		if (%found >= 0)
		{
			ReadPlayerConfig(%found);
		}
		else
		{
			//add it to the end
			$PCFG::LastPlayer = $PCFG::LastPlayer + 1;
			
			//create the vars
			$PCFG::Name[$PCFG::LastPlayer]		= %newPlayer;
			$PCFG::RealName[$PCFG::LastPlayer]	= "";
			$PCFG::EMail[$PCFG::LastPlayer]		= "";
			$PCFG::Tribe[$PCFG::LastPlayer]		= "";
			$PCFG::URL[$PCFG::LastPlayer]			= "";
			$PCFG::Info[$PCFG::LastPlayer]		= "";
			$PCFG::Gender[$PCFG::LastPlayer]		= "MALE";
			$PCFG::Script[$PCFG::LastPlayer]		= "";
			$PCFG::Voice[$PCFG::LastPlayer]		= "";
			$PCFG::SkinBase[$PCFG::LastPlayer]	= "";

			//add it to the list
			FGCombo::addEntry(IDCTG_PLYR_CFG_COMBO, $PCFG::Name[$PCFG::LastPlayer], $PCFG::LastPlayer);

			//set the added player as the current
			ReadPlayerConfig($PCFG::LastPlayer);
		}
	}
}

function DeleteCurrentPlayerConfig()
{
	//first close the dialog popup
	GuiPopDialog(MainWindow, 0);

	if ($PCFG::CurrentPlayer >= 0 && $PCFG::CurrentPlayer <= $PCFG::LastPlayer)
	{
		//copy the last player entries over top of the one being deleted
		if ($PCFG::CurrentPlayer < $PCFG::LastPlayer)
		{
			$PCFG::Name[$PCFG::CurrentPlayer]		= $PCFG::Name[$PCFG::LastPlayer];
			$PCFG::RealName[$PCFG::CurrentPlayer]	= $PCFG::RealName[$PCFG::LastPlayer];
			$PCFG::EMail[$PCFG::CurrentPlayer]		= $PCFG::EMail[$PCFG::LastPlayer];
			$PCFG::Tribe[$PCFG::CurrentPlayer]		= $PCFG::Tribe[$PCFG::LastPlayer];
			$PCFG::URL[$PCFG::CurrentPlayer]			= $PCFG::URL[$PCFG::LastPlayer];
			$PCFG::Info[$PCFG::CurrentPlayer]		= $PCFG::Info[$PCFG::LastPlayer];
			$PCFG::Gender[$PCFG::CurrentPlayer]		= $PCFG::Gender[$PCFG::LastPlayer];
			$PCFG::Script[$PCFG::CurrentPlayer]		= $PCFG::Script[$PCFG::LastPlayer];
			$PCFG::Voice[$PCFG::CurrentPlayer]		= $PCFG::Voice[$PCFG::LastPlayer];
			$PCFG::SkinBase[$PCFG::CurrentPlayer]	= $PCFG::SkinBase[$PCFG::LastPlayer];


			//delete it from the combo box
			FGCombo::deleteEntry(IDCTG_PLYR_CFG_COMBO, $PCFG::CurrentPlayer);

			//delete the last entry, and re-add it with the new ID number
			FGCombo::deleteEntry(IDCTG_PLYR_CFG_COMBO, $PCFG::LastPlayer);
			FGCombo::addEntry(IDCTG_PLYR_CFG_COMBO, $PCFG::Name[$PCFG::CurrentPlayer], $PCFG::CurrentPlayer);
		}
		else
		{
			//delete it from the combo box
			FGCombo::deleteEntry(IDCTG_PLYR_CFG_COMBO, $PCFG::CurrentPlayer);
		}
		
		//erase the last player config
		$PCFG::Name[$PCFG::LastPlayer]		= "";
		$PCFG::RealName[$PCFG::LastPlayer]	= "";
		$PCFG::EMail[$PCFG::LastPlayer]		= "";
		$PCFG::Tribe[$PCFG::LastPlayer]		= "";
		$PCFG::URL[$PCFG::LastPlayer]			= "";
		$PCFG::Info[$PCFG::LastPlayer]		= "";
		$PCFG::Gender[$PCFG::LastPlayer]		= "";
		$PCFG::Script[$PCFG::LastPlayer]		= "";
		$PCFG::Voice[$PCFG::LastPlayer]		= "";
		$PCFG::SkinBase[$PCFG::LastPlayer]	= "";

		//decriment the last player index
		$PCFG::LastPlayer = $PCFG::LastPlayer - 1;

		//now set the current player to be the first
		if ($PCFG::LastPlayer >= 0)
		{
			ReadPlayerConfig(0);
		}
		else
		{
			ClearPlayerConfig();
			OpenNewPlayerDialog();
		}
	}
}

function SwitchGender(%gender)
{
	//see if we actually switched genders
	if (String::ICompare(%gender, $PCFG::Gender[$PCFG::CurrentPlayer]) != 0)
	{
		$PCFG::Gender[$PCFG::CurrentPlayer] = %gender;

		//find a voice set that exists
		%searchVoiceSet = $PCFG::Gender[$PCFG::CurrentPlayer] @ "1";
		%voiceSet = 0;
		%found = -1;
		while ($VoiceArray[%voiceSet] != "")
		{
			if (String::ICompare($VoiceArray[%voiceSet], %searchVoiceSet) == 0)
			{
				%found = %voiceSet;
		   }
			%voiceSet = %voiceSet + 1;
		}
		if (%found >= 0)
		{
			$PCFG::Voice[$PCFG::CurrentPlayer]	= %searchVoiceSet;
			FGCombo::setSelected(IDCTG_PLYR_CFG_VOICE, %found);
		}

		//set the skin combo
		//add the correct skins to the combo box
		FGCombo::clear(IDCTG_PLYR_CFG_SKIN);
		%skin = 0;
		while ($SkinArray[%skin] != "")
		{
			if (! String::ICompare($PCFG::Gender[$PCFG::CurrentPlayer], "MALE"))
			{
				%strIndex = String::findSubStr($SkinArray[%skin], ".larmor");
			}
			else
			{
				%strIndex = String::findSubStr($SkinArray[%skin], ".lfemale");
			}
			if (%strIndex >= 0)
			{
				%skinBase = String::getSubStr($SkinArray[%skin], 0, %strIndex);
				FGCombo::addEntry(IDCTG_PLYR_CFG_SKIN, %skinBase, %skin);
			}
			%skin = %skin + 1;
		}

		//see if we can set the original
		%skin = FGCombo::findEntry(IDCTG_PLYR_CFG_SKIN, $PCFG::SkinBase);
		if (%skin < 0)
		{
			FGCombo::selectNext(IDCTG_PLYR_CFG_SKIN);
			%skin = FGCombo::getSelected(IDCTG_PLYR_CFG_SKIN);
		}

		if (%skin >= 0)
		{
			$PCFG::SkinBase[$PCFG::CurrentPlayer] = $SkinArray[%skin];
			setSkinBase(%skin, $PCFG::Gender[$PCFG::CurrentPlayer]);
		}
	}
}

function setSkinBase(%index, %gender)
{
	//strip off the extention
	if (! String::ICompare(%gender, "MALE"))
	{
		%strIndex = String::findSubStr($SkinArray[%index], ".larmor");
	}
	else
	{
		%strIndex = String::findSubStr($SkinArray[%index], ".lfemale");
	}
	if (%strIndex >= 0)
	{
		%skinBase = String::getSubStr($SkinArray[%index], 0, %strIndex);
		$PCFG::SkinBase = %skinBase;
	}
	else
	{
		$PCFG::SkinBase = "";
	}

	FGCombo::setSelected(IDCTG_PLYR_CFG_SKIN, %index);
	if ($PCFG::SkinBase != "")
	{
		FGSkin::set(IDCTG_PLAYER_TS, $PCFG::SkinBase, %gender); 
	}
}

function selectPlayerSkin()
{
	setSkinBase(FGCombo::getSelected(IDCTG_PLYR_CFG_SKIN), $PCFG::Gender[$PCFG::CurrentPlayer]);
}

