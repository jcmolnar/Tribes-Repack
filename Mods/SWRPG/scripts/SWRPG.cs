$NewsT[1] = "Visit the forum!";
$News[1] = " Visit <f2>www.swrpg.info/forum <f0> and register!";
$NewsT[2] = "";
$News[2] = " ";
$NewsT[3] = "";
$News[3] = "";
$NewsT[4] = "";
$News[4] = "";
$NewsT[5] = "";
$News[5] = "";

$MODInfo = "Star Wars RPG mod by <f1>Hazor and <f1>RoAd-DoGg, see <f2>Star Wars RPG Read Me.txt <f0> in the mod folder for a full list of contributors.\nType <f1>#news<f0> for information on recent updates.\nRegister on the forum please! <f2s>www.swrpg.info/forum";
$Server::FileURL = "http://www.swrpg.info"; //Is this variable ever even used?...
$modurl = "http://www.swrpg.info"; //Where did I use this? o0

function InitObjectives()
{
	dbecho($dbechoMode, "InitObjectives()");

    for(%t = 0; %t < getNumTeams(); %t++)
    {
	%l = -1;
	Team::setObjective(%t, %l++, "<jc><f8>Welcome To Star Wars RPG!");
	Team::setObjective(%t, %l++, "<f5>Important Links:");
	Team::setObjective(%t, %l++, "<f0>http://www.www.swrpg.info");
	Team::setObjective(%t, %l++, "<f0>http://www.swrpg.info/forum <f2>-SWRPG Forum");
	Team::setObjective(%t, %l++, "");
	Team::setObjective(%t, %l++, "<f5>Getting Started:");
	Team::setObjective(%t, %l++, "<jc><f4>Nnew players, please read the whole thing. People familiar with Tribes RPG can skip the new player introduction as it contains no information you do not already know.");
	Team::setObjective(%t, %l++, "<jc><f4>IT IS HIGHLY RECOMMENDED that you visit www.swrpg.info/forum and read the new player information thread before attempting to play!");
	Team::setObjective(%t, %l++, "<f1>Use the TAB key to access your skills and assign your SP (skill points) to them.");
	Team::setObjective(%t, %l++, "<f1>To find the maximum SP capability for your level, take your level #, multiply it by 10 and add 20 to the result and add another 1 for each time you've remorted.");
	Team::setObjective(%t, %l++, "<f1>Once you reach level 101, you stop gaining EXP by killing bots or players, and you can then cast the remort spell which resets your skills and sets you back to level 1 but as a stronger class with a +.1 skill multiplier.");
	Team::setObjective(%t, %l++, "<f1>To talk to an NPC, just say 'hello' while standing right next to one.  Their response will have keywords that you can type to trigger different things such as BUY, DEPOSIT, WITHDRAW, etc.");
	Team::setObjective(%t, %l++, "<f1>It is a good idea to download the SWRPG script pack from the SWRPG forum.  It makes playing easier/smoother(See thread for details).");
	Team::setObjective(%t, %l++, "<f1>If you ever become lost, just stand still and type '#recall'.  This command will require that you wait " @ $recallDelay / 60 @ " minute(s) for it to work.  Just be patient.");
	Team::setObjective(%t, %l++, "<f1>If you ever fall off of the map, or through it, use the same '#recall' command.  Before you do, however, disconnect from the server and reconnect so that your character isn't moving in any direction other than down while falling.");
	Team::setObjective(%t, %l++, "<f1>If your LCK is set to DEATH or if you have 0 LCK, you are able to be killed.  When killed, you automatically drop all of your items in a pack.  If you had at least 1 LCK at the time of death, then your pack will be protected.");
	Team::setObjective(%t, %l++, "<f1>Only you can pick up or #sharepack one of your protected packs.  If you had 0 LCK at the time of death, your pack will be unprotected and bots or other players could take it.");
	Team::setObjective(%t, %l++, "<f1>If your LCK is set to MISS, each hit that would normally kill you instead subtracts from your LCK each time you are 'hit'.");
	Team::setObjective(%t, %l++, "<f1>Pay attention to the skills that each class specializes in.  If you're interested in using the force, then you won't have much luck training your spell-casting as a Pilot, because your spellcasting multipliers are too low.");
	Team::setObjective(%t, %l++, "<f1>You can always find info on items, weapons, force powers, armor, skills, and chat commands with the #w command. ");
	Team::setObjective(%t, %l++, "<f1>If you wanted to find info on Holdout Blaster, just type in '#w holdout'.  Notice not to use any spaces or special characters in the name of the item/power/skill.");
	Team::setObjective(%t, %l++, "<jc><f1>In the tab menu, you will notice a new option, the 'Belt'. It contains your books, badges, and some personal options.");

//	Team::setObjective(%t, %l++, "<jc><f1>Books contain useful information, like the smithing lists and force powers. Badges can be earned by exploring, completing quests, and other things.");

	Team::setObjective(%t, %l+=2, "<jc><f4>If you ever encounter a bug or glitch of any sort in the game,send an e-mail to hazorswrpg.info, or post in the SWRPG forum (<f1>www.swrpg.info/forum<f4>), with all of the information on the bug and, if possible, instructions on how to duplicate it.");
//	Team::setObjective(%t, %l++, "<jc><f4>Hazor will try his best to reward you appropriately depending on how severe the bug/glitch is.");
	Team::setObjective(%t, %l+=2, "<jc><f3>This objectives page is brought to you courtesy of Blitz Deg'al, edited for SWRPG by Hazor.");

	Team::setObjective(%t, %l+=3, "<jc><f4>List of force powers:");
	//for(%si = 1; $Spell::keyword[%si] != ""; %si++)
	//Team::setObjective(%t, %l++, "<jc><f1>" @ $Spell::keyword[%si]);
	%numberoflines = 0;
	for(%si = 1; $Spell::keyword[%si] != ""; %si++)
	{
		if(String::NEWgetSubStr(%si, 1, 1) == 0)
			%numberoflines++;
		%list[%numberoflines] = %list[%numberoflines] @ ", " @ $Spell::keyword[%si];
	}
	for(%i = 0; %i < %numberoflines; %i++)
	 Team::setObjective(%t, %l + %numberoflines, "<jc><f1>" @ %list[%numberoflines]);
    }
}