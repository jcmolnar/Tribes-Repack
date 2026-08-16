
function RMText::addText(%text, %overWriteLine) {

	if(%text == "")
		%text = " ";

	if(%overWriteLine == "") {
		Team::setObjective(0, $RMText::CurLine, %text);
		$RMText::CurLine++;
	}
	else {
		Team::setObjective(0, %overWriteLine, %text);
	}
}

function RMText::Clear() {
	for(%i = 0; %i <= $RMText::CurLine; %i++) {
		Team::setObjective(0, %i, " ");
	}
	$RMText::CurLine = 1;
}

function RMText::LoadIntro(%Client) {

	Client::setGuiMode(%Client, $GuiModeObjectives);
//	Client::setGuiMode(%Client, $GuiModeObjectives);

	%Client.guiLock = true;

	remoteEval(%Client, SFXPLAY, 1, "rmrmus6.wav", 167, 1); //rmr2 -118

	%min = 650; //"-600";
	%max = 500; //"500";
	%delay = "0.25";
	%skip = "2";

	remoteEval(%Client, "RMText::SetMoving", -%min, %max, %delay, %skip);

	%a = %min * -1;
	%b = %min + %max;
	%c = (%b / %skip) * %delay;

	echo("delay: "@(%c/60)@"min");

	Schedule("RMText::IntroDone("@%Client@");", %c);

}

function RMText::IntroDone(%Client) {
	echo("IntroDone!");
	%Client.guiLock = "";
	Client::setGuiMode(%Client, 3);
	Client::setGuiMode(%Client, $GuiModePlay);
	remoteEval(%Client, SFXSTOP);
}

//
function remoteG_(%c) {
	RMText::LoadIntro(%c);
}
function g() {
	remoteEval(2048, G_);
}//

function InitObjectives() {
	//Team::setObjective(0, 0, "<f0>f0<f1>f1<f2>f2<f3>f3<f4>f4<f5>f5<f6>f6<f7>f7<f8>f8<f9>f9");
//	RMText::addText("<jc><f8>Welcome to the world Red Moon.");
//	RMText::addText("");
//	RMText::addText("<f5>Important Links:");
//	RMText::addText("<f0>http://www.geocities.com/redmoonrpg<f2>-Red Moon RPG Website");
//	RMText::addText("<f0>http://www.planettribes.com/rpg <f2>-RPG Website");
//	RMText::addText("<f0>http://www.mp3.com/theoryoftrance <f2>-Help JI out");
//	RMText::addText("<f5>Getting Started:");
//	RMText::addText("<jc><f4>IT IS RECCOMENDED THAT YOU VISIT THE RED MOON RPG WEBSITE BEFORE PLAYING!");
//	RMText::addText("");
//	RMText::addText("");
//	RMText::addText("");
//	RMText::addText("<f1>Your stats grow with level. The way a stat grows depends on what class you are.");
//	RMText::addText("<f1>Example, if you are a fighter your STR (Strength) Stat will grow faster than a mages. And a mages INT (intelligence) stat will grow faster than a fighters would.");
//	RMText::addText("<f1>There are also several stat modifiers in the game. Example : Modifier Gems, Rings, Weapons, and Armor");
//	RMText::addText("<f4>Class Info:");
//	RMText::addText("<f1>Cleric	: Clerics can cast all curative magic, and some white attack spells. Since they are casters, there are some limits on the weapons they can use.");
//	RMText::addText("<f1>Druid	: Druids are trained in both warlock spells, and curative spells. They are moderate casters, but have several weapon limitations for that reason.");
//	RMText::addText("<f1>Fighter	: Fighters can use all normal weapons in the game. They gain STR quickly and are extremely powerful with the sword, but weak with magic.");
//	RMText::addText("<f1>Paladin	: Paladins are fighter - mages. They can use almost all the weapons in the game, and can cast most of the spells.");
//	RMText::addText("<f1>Ranger	: Rangers are an extremely resourceful class. They are good with the bow and are excellent trackers.");
//	RMText::addText("<f1>Thief	: Thefts are excellent with piercing weapons and stealing. If you are a thief, how well you can steal depends on your level.");
//	RMText::addText("<f1>Bards	: Bards are similar to thieves, however they are masters of trickery.");
//	RMText::addText("<f1>Mage		: Mages can cast all the spells in the game, however , they are generally weak physically, and can use very few weapons..");
//	RMText::addText("<jc><f4>If you ever encounter a bug or glitch of any sort in the game, make sure you can duplicate it and then send an e-mail to Chee39@hotmail.com with all of the information on the bug and instructions on how to duplicate it.");
//	RMText::addText("<jc><f4>I (Chee2) will try my best to fix it.");



//	RMText::addText("<f0>f0<f1>f1<f2>f2<f3>f3<f4>f4<f5>f5<f6>f6<f7>f7<f8>f8<f9>f9");

	//There is a limit on how much text can be put in this GUI, so we will take pics and load those:p
	RMText::addText("<jc><f8>Welcome to the world Red Moon.");
	RMText::addText("");
	RMText::addText("");
	RMText::addText("TC created by Chee2 and Deus_ex_Machina");
	RMText::addText("");
	RMText::addText("Special Thanks to JeremyIrons for creating the original AD&D version of the rpgmod!");
	RMText::addText("");
	RMText::addText("");
	RMText::addText("");
	RMText::addText("");
	RMText::addText("");
	RMText::addText("");
	RMText::addText("   For the past two centuries, Gaia had experienced peace. It was a world without evil, and mankind reigned supreme, along with the Goblin King. Though peaceful at the time, it was not always like that.");
	RMText::addText("");
	RMText::addText("");
	RMText::addText("");
	RMText::addText(" <f8>Raise of Awira and Reiak");
	RMText::addText("");
	RMText::addText("   The two warlord brothers, Awira, and Reiak brought fear and destruction as they led their dark armies of horrible beasts, and undead to the good people of Gaia two hundred years ago.");
	RMText::addText("");
	RMText::addText("");
	RMText::addText("   Just when all seemed lost, one last alliance of Humans and Goblins stood in the way of total domination of the two brothers. Among the Human armies, was Hok the Great Warrior, and with the Goblin armies was the Goblin King, Zortag.");
	RMText::addText("");
	RMText::addText("");
	RMText::addText("   The battle with the dark armies waged on for countless years, and the dark army gradually started showing signs of defeat.");
	//RMText::Clear();
	RMText::addText("");
	RMText::addText("");
	RMText::addText("   However, the power of the two brothers stood virtually unchallenged as they summoned death and decimated the mighty Human, and Goblin warriors in enormous numbers.");
	RMText::addText("");
	RMText::addText("");
	RMText::addText("   Finally, Hok and Zortag joined forces and casted the most powerful spell the world had ever seen, \"Ultima\". This spell required huge amounts of strength and wisdom to cast, and with a price,");
	RMText::addText("the caster's life. They were not wise enough to cast this spell alone, thus they casted the spell together and weaken the two brothers greatly allowing Skilled Mages to cast \"hold\" and banish");
	RMText::addText("their souls deep within Gaia’s fortress to ensure that they would never wreak havoc again.");
	RMText::addText("");
	RMText::addText("");
	RMText::addText("   Now, the brothers by some means have broken their seal, and have begun to summon their dark armies, and corrupt the minds of innocent people once again. Turning the people of Gaia into evil adversaries.");
	RMText::addText("");
	RMText::addText("");
	RMText::addText("   It is up to you to venture deep within the world and destroy the two brothers by any means possible, so that they may never return again...");
	RMText::addText("");
	RMText::addText("");
	RMText::addText("");
	RMText::addText(" <f8>What do we want you to do?");
	RMText::addText("");
	RMText::addText("<jc>To start, please pick your class and play your character by making firends and enemys, when you feel you have to power to stop Awira and Reiak, join a group of other powerful warriors.");
	RMText::addText("");
	RMText::addText("Mayor Alice is waiting for you South East of Rin Vale. Please see her for your things before you start.");
	RMText::addText("");

	RMText::Clear();

//	RMText::addText("<B0,0:_Intro001.bmp>");
	RMText::addText("<B0,0:_sIntro001.bmp>");
//	RMText::addText("<B0,0:_Intro002.bmp>");

//	for(%i = 1; %i < getNumTeams(); %i++) {
//
//		Team::setObjective(%i, 1, "<f7><jc>KILL ALL HUMAN PLAYERS");
//		Team::setObjective(%i, 2, "<f7><jc>KILL ALL HUMAN PLAYERS");
//		Team::setObjective(%i, 3, "<f7><jc>KILL ALL HUMAN PLAYERS");
//		Team::setObjective(%i, 4, "<f7><jc>KILL ALL HUMAN PLAYERS");
//		Team::setObjective(%i, 5, "<f7><jc>KILL ALL HUMAN PLAYERS");
//		Team::setObjective(%i, 6, "<f7><jc>KILL ALL HUMAN PLAYERS");
//		Team::setObjective(%i, 7, "<f7><jc>KILL ALL HUMAN PLAYERS");
//		Team::setObjective(%i, 8, "<f7><jc>KILL ALL HUMAN PLAYERS");
//		Team::setObjective(%i, 9, "<f7><jc>KILL ALL HUMAN PLAYERS");
//		Team::setObjective(%i, 10,"<f7><jc>KILL ALL HUMAN PLAYERS");
//		Team::setObjective(%i, 11,"<f7><jc>KILL ALL HUMAN PLAYERS");
//	}
}
