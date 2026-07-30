
function init_townbots() {

	echo("Setup TownBots - Start.");

	//Add keywords here
	$RM::KeyWords = "yes no buy deposit withdraw storage smith";

	$TownBotList = " ";

	///////////////////////////////////////////////////////////////////////////////////
	//RIN VALE TOWNBOTS

	MakeTownBot("cgoblin",	"goblin001", "Basic Weapons",	"-221.054 41.2381 40.0332");

	$TownBot[$townbot, TYPE] = "merchant";
	$TownBot[$townbot, SHOP] = "Practice_Sword Ivory_Dagger Copper_Dagger Bronze_Dagger Ivory_Sword Copper_Sword Bronze_Sword Ivory_Hatchet Copper_Hatchet Bronze_Hatchet Ivory_Axe Copper_Axe Bronze_Axe Bronze_Baridache Wood_Rod Apprentice_Rod ";
	$TownBot[$townbot, SayBye] = "See ya.";


	MakeTownBot("MaleHumanTownBot",	"Male001",	"Premium Weapons",	"-221.005 49.8416 40.0332");

	$TownBot[$townbot, TYPE] = "merchant";
	$TownBot[$townbot, SHOP] = "Blue_Sword_of_Strength Blue_Sword_of_Magic Sword_of_Aim Axe_of_Hok Brain_Crusher Demon Hard_Hitter Fire_Staff ";
	$TownBot[$townbot, SayBye] = "See ya.";


	MakeTownBot("MaleHumanTownBot",	"Male002",	"Black Smith",	"-225.83 49.2667 40.0332");
	$TownBot[$townbot, TYPE] = "blacksmith";
	$TownBot[$townbot, SayBye] = "See ya.";


	MakeTownBot("MaleHumanTownBot",	"Male002",	"Items",	"-220.559 27.5284 40.0332");

	$TownBot[$townbot, TYPE] = "merchant";
	$TownBot[$townbot, SHOP] = "Light_Potion Mid-Potion Hi-Potion Ether Mid-Ether Antidote Soft Tent Teleport_Scroll Compass ";
	$TownBot[$townbot, SayBye] = "See ya.";


	MakeTownBot("MaleHumanTownBot",	"Male001",	"Rune Mage",	"-225.832 16.9702 40");

	$TownBot[$townbot, TYPE] = "merchant";
	$TownBot[$townbot, SHOP] = "Ether Hi-Ether Rune_Red Rune_Green Rune_Blue Rune_Yellow Rune_White Rune_Black Rune_Cyan Rune_Orange Rune_Grey Rune_Pink Rune_Purple ";
	$TownBot[$townbot, SayBye] = "Farewell.";


	MakeTownBot("FemaleHumanTownBot",	"Female001",	"Accessorys",	"-220.387 36.2678 40.0332");

	$TownBot[$townbot, TYPE] = "merchant";
	$TownBot[$townbot, SHOP] = "Clothing Leather_Armor Brass_Armor Raider_Suit Klepto_Outfit Jerkilin Pink_Robe Purple_Robe Copper_Buckler Bandana Leather_Pants Leather_Boots Leather_Gloves Brass_Ring ";
	$TownBot[$townbot, SayBye] = "See ya.";


	MakeTownBot("FemaleHumanTownBot",	"Female002",	"Banker",	"-198.677 46.7033 40.0332");

	$TownBot[$townbot, TYPE] = "banker";
	$TownBot[$townbot, SayBye] = "Have a nice day.";


	MakeTownBot("MaleHumanTownBot",	"Male001",	"Captain",	"-189.863 -14.7828 60.5332");

	$TownBot[$townbot, TYPE] = "assassin";
	$TownBot[$townbot, SayBye] = "We shall meet again.";


	MakeTownBot("FemaleHumanTownBot",	"Female001",	"Mayor Alice",	"-112.968 -67.5339 47.2526");

	$TownBot[$townbot, TYPE] = "quester";
	$TownBot[$townbot, NEED] = "newcharpass 1";
	$TownBot[$townbot, GIVE] = "Small_Bag 1 Tiny_Bag -1 q&Cure_Potion 1 COINS 400 Rations 20 Practice_Sword 1 Light_Potion 5 Ether 1";

	$TownBot[$townbot, SAY, 1] = "<f0>Do you know what you have to do?";
	$TownBot[$townbot, SAY, 2] = "<f0>To the South East at the <f1>Dunega Graveyard<f0> is where all the trouble started <f1>You<f0> have to find out what is causing our dead to walk and cause trouble on our town before its too late!";

	$TownBot[$townbot, CUE, 1] = "\n\n  <f1>N<f0>o.\n  <f1>Y<f0>es.";
	$TownBot[$townbot, CUEKEY, 1] = "n y";

	$TownBot[$townbot, NSAY, 1] = "<f0>Hello, would you like your <f1>things<f0>?";
	$TownBot[$townbot, NSAY, 2] = "<f0>Here they are, and have a <f1>Cure Potion<f0> too. Give this to <f1>Booga<f0>, he is a runt and he needs these to stay alive. He should be near <f1>The Sewers<f0>.\nGood Luck!";

	$TownBot[$townbot, NQUEST, 1] = "BoogaQuest"; // starts new quest (see Booga bot)

	$TownBot[$townbot, NCUE, 1] = "\n\n  <f1>Y<f0>es.\n  <f1>N<f0>ot yet.";
	$TownBot[$townbot, NCUEKEY, 1] = "y n";

	$TownBot[$townbot, NSAYNO, 1] = "Oh, well come back when you're ready.";

	$TownBot[$townbot, SayBye] = "Bye bye!";


	MakeTownBot("MaleHumanTownBot",	"Male002",	"Jerico",	"-155.976 10.8753 40.0332");

	$TownBot[$townbot, TYPE] = "quester";
	$TownBot[$townbot, NEED] = "Playing_Card 1";
	$TownBot[$townbot, GIVE] = "LCK 1";

	$TownBot[$townbot, SAY, 1] = "There's nothing like a good game of cards!";
	$TownBot[$townbot, SAY, 2] = "If you've got a <f1>Playing card<f0>, I'll help you with your crappy luck.";

	$TownBot[$townbot, CUE, 1] = "\n\n  <f1>C<f0>ards.\n  <f1>B<f0>ye.";
	$TownBot[$townbot, CUEKEY, 1] = "c b";

	$TownBot[$townbot, NSAY, 1] = "Great! lets get started!";
	$TownBot[$townbot, NSAY, 2] = "You did great!.";

	$TownBot[$townbot, NCUE, 1] = "\n\n  <f1>O<f0>k.\n  <f1>N<f0>o thanks.";
	$TownBot[$townbot, NCUEKEY, 1] = "o n";

	$TownBot[$townbot, NSAYNO, 1] = "Darn!";

	$TownBot[$townbot, SayBye] = "See ya!";


	MakeTownBot("MaleHumanTownBot",	"Male001",	"Sama",	"-152.839 17.4292 40.0332");

	$TownBot[$townbot, TYPE] = "quester";
	$TownBot[$townbot, NEED] = "Playing_Card 52 CNT =0 AFFECT Sama";
	$TownBot[$townbot, GIVE] = "Lucky_Ring 1 CNT +1 AFFECT Sama";

	$TownBot[$townbot, SAY, 1] = "*sighs* Too bad I can't play cards with Jerico";
	$TownBot[$townbot, SAY, 2] = "I lost my deck. If you get me a deck (<f1>52 cards<f0>) I'll give you something.";

	$TownBot[$townbot, CUE, 1] = "\n\n  <f1>W<f0>hy?\n  <f1>B<f0>ye.";
	$TownBot[$townbot, CUEKEY, 1] = "w b";

	$TownBot[$townbot, NSAY, 1] = "Wow! You have a whole deck of cards! Can I have it?";
	$TownBot[$townbot, NSAY, 2] = "Thank you! Here, you can have this.";

	$TownBot[$townbot, NCUE, 1] = "\n\n  <f1>S<f0>ure.\n  <f1>N<f0>o way.";
	$TownBot[$townbot, NCUEKEY, 1] = "s n";

	$TownBot[$townbot, NSAYNO, 1] = "Darn!";

	$TownBot[$townbot, SayBye] = "Bye.";


	MakeTownBot("cgoblin",	"goblin001",	"Alchemist",	"-152.534 1.36332 40.0332");

	$TownBot[$townbot, TYPE] = "quester";
	$TownBot[$townbot, NEED] = "Zombie_powder 1 COINS 100";
	$TownBot[$townbot, TAKE] = "Zombie_powder 1 COINS 100";
	$TownBot[$townbot, GIVE] = "Poison_Dust 1";

	$TownBot[$townbot, SAY, 1] = "Hey, I can make you some <f1>Poison Dust<f0> if you have 100 coins and some <f1>Zombie Powder<f0>.";
	$TownBot[$townbot, SAY, 2] = "Sofar, all I know how to make is <f1>Poison Dust<f0>.";

	$TownBot[$townbot, CUE, 1] = "\n\n  <f1>A<f0>lchemy.\n  <f1>B<f0>ye.";
	$TownBot[$townbot, CUEKEY, 1] = "a b";

	$TownBot[$townbot, NSAY, 1] = "Ok, lemme have a second to mix this";
	$TownBot[$townbot, NSAY, 2] = "Here you go pal!";

	$TownBot[$townbot, NCUE, 1] = "\n\n  <f1>O<f0>k.\n  <f1>N<f0>o thanks.";
	$TownBot[$townbot, NCUEKEY, 1] = "o n";

	$TownBot[$townbot, NSAYNO, 1] = "Please return if you need my help";

	$TownBot[$townbot, SayBye] = "See ya!";


	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//GUARDS WITH BAGS


	MakeTownBot("MaleHumanTownBot",	"Male002",	"Guard Norris",	"-185.242 -15.4908 60.5332");

	$TownBot[$townbot, TYPE] = "quester";
	$TownBot[$townbot, NEED] = "Small_Bag 1 LVLG 4 ";
	$TownBot[$townbot, GIVE] = "Mid_Bag 1 ";

	$TownBot[$townbot, SAY, 1] = "*The guard does not respond.*";
	$TownBot[$townbot, SAY, 2] = "*The guard seems angry that you poked him...*";

	$TownBot[$townbot, CUE, 1] = "\n\n  <f1>.<f0>...";
	$TownBot[$townbot, CUEKEY, 1] = ".";

	$TownBot[$townbot, NSAY, 1] = "Wow, you're getting strong! Here have this.";
	$TownBot[$townbot, NSAY, 2] = "You're welcome!";

	$TownBot[$townbot, NCUE, 1] = "\n\n  <f1>O<f0>k.\n  <f1>N<f0>o thanks.";
	$TownBot[$townbot, NCUEKEY, 1] = "o n";
	$TownBot[$townbot, NSAYNO, 1] = "Oh, well come back when you want it.";

	$TownBot[$townbot, SayBye] = " ...";


	MakeTownBot("MaleHumanTownBot",	"Male001",	"Guard Guile",	"-216.103 -2.46757 40.0332");

	$TownBot[$townbot, TYPE] = "quester";
	$TownBot[$townbot, NEED] = "Mid_Bag 1 LVLG 14 ";
	$TownBot[$townbot, GIVE] = "Large_Bag 1 ";

	$TownBot[$townbot, SAY, 1] = "*The guard does not respond.*";
	$TownBot[$townbot, SAY, 2] = "*The guard seems angry that you poked him...*";

	$TownBot[$townbot, CUE, 1] = "\n\n  <f1>.<f0>...";
	$TownBot[$townbot, CUEKEY, 1] = ".";

	$TownBot[$townbot, NSAY, 1] = "Wow, you're getting strong! Here have this.";
	$TownBot[$townbot, NSAY, 2] = "You're welcome!";

	$TownBot[$townbot, NCUE, 1] = "\n\n  <f1>O<f0>k.\n  <f1>N<f0>o thanks.";
	$TownBot[$townbot, NCUEKEY, 1] = "o n";

	$TownBot[$townbot, NSAYNO, 1] = "Oh, well come back when you want it.";

	$TownBot[$townbot, SayBye] = "...";


	MakeTownBot("MaleHumanTownBot",	"Male001",	"Guard Jessef",	"-172.412 -6.17675 40");

	$TownBot[$townbot, TYPE] = "quester";

	$TownBot[$townbot, NEED] = "Large_Bag 1 LVLG 24 COINS 9999";
	$TownBot[$townbot, GIVE] = "Very_Large_Bag 1 ";

	$TownBot[$townbot, SAY, 1] = "*The guard does not respond.*";
	$TownBot[$townbot, SAY, 2] = "*The guard seems angry that you poked him...*";

	$TownBot[$townbot, CUE, 1] = "\n\n  <f1>.<f0>...";
	$TownBot[$townbot, CUEKEY, 1] = ".";

	$TownBot[$townbot, NSAY, 1] = "Wow, you're getting strong! Here have this.";
	$TownBot[$townbot, NSAY, 2] = "You're welcome!";

	$TownBot[$townbot, NCUE, 1] = "\n\n  <f1>O<f0>k.\n  <f1>N<f0>o thanks.";
	$TownBot[$townbot, NCUEKEY, 1] = "o n";

	$TownBot[$townbot, NSAYNO, 1] = "Oh, well come back when you want it.";

	$TownBot[$townbot, SayBye] = " ...";


	MakeTownBot("MaleHumanTownBot",	"Male002",	"Guard Jeffrey",	"-165.558 -53.5915 40");

	$TownBot[$townbot, TYPE] = "quester";

	$TownBot[$townbot, NEED] = "Very_Large_Bag 1 LVLG 49 COINS 99999";
	$TownBot[$townbot, GIVE] = "Super_Large_Bag 1 ";

	$TownBot[$townbot, SAY, 1] = "*The guard does not respond.*";
	$TownBot[$townbot, SAY, 2] = "*The guard seems angry that you poked him...*";

	$TownBot[$townbot, CUE, 1] = "\n\n  <f1>.<f0>...";
	$TownBot[$townbot, CUEKEY, 1] = ".";

	$TownBot[$townbot, NSAY, 1] = "Wow, you're getting strong! Here have this.";
	$TownBot[$townbot, NSAY, 2] = "You're welcome!";

	$TownBot[$townbot, NCUE, 1] = "\n\n  <f1>O<f0>k.\n  <f1>N<f0>o thanks.";
	$TownBot[$townbot, NCUEKEY, 1] = "o n";

	$TownBot[$townbot, NSAYNO, 1] = "Oh, well come back when you want it.";

	$TownBot[$townbot, SayBye] = " ...";


	MakeTownBot("MaleHumanTownBot",	"Male001",	"Guard Steven",	"-216.623 -178.077 83.2248");

	$TownBot[$townbot, TYPE] = "quester";

	$TownBot[$townbot, NEED] = "Super_Large_Bag 1 LVLG 149 COINS 9999999";
	$TownBot[$townbot, GIVE] = "Magic_Bag 1 ";

	$TownBot[$townbot, SAY, 1] = "*The guard does not respond.*";
	$TownBot[$townbot, SAY, 2] = "*The guard seems angry that you poked him...*";

	$TownBot[$townbot, CUE, 1] = "\n\n  <f1>.<f0>...";
	$TownBot[$townbot, CUEKEY, 1] = ".";

	$TownBot[$townbot, NSAY, 1] = "Wow, you're getting strong! Here have this.";
	$TownBot[$townbot, NSAY, 2] = "You're welcome!";

	$TownBot[$townbot, NCUE, 1] = "\n\n  <f1>O<f0>k.\n  <f1>N<f0>o thanks.";
	$TownBot[$townbot, NCUEKEY, 1] = "o n";

	$TownBot[$townbot, NSAYNO, 1] = "Oh, well come back when you want it.";

	$TownBot[$townbot, SayBye] = " ...";


	MakeTownBot("FemaleHumanTownBot",	"Female002",	"Nooe",	"-526.182 -319.309 153.031");

	$TownBot[$townbot, TYPE] = "quester";
	$TownBot[$townbot, NEED] = "Hi-potion 99";
	$TownBot[$townbot, GIVE] = "Uber_Burger 20";

	$TownBot[$townbot, SAY, 1] = "Welcome to 7th Heaven! How about you.";
	$TownBot[$townbot, SAY, 2] = "We have some great stuff here.";

	$TownBot[$townbot, CUE, 1] = "\n\n  <f1>B<f0>uy.\n  <f1>L<f0>ater.";
	$TownBot[$townbot, CUEKEY, 1] = "b l";

	$TownBot[$townbot, NSAY, 1] = "I see you have a lot of <f1>Hi-Potions<f0>... If you give them to me I'll give you something.";
	$TownBot[$townbot, NSAY, 2] = "Here, you can have this.";

	$TownBot[$townbot, NSAYNO, 1] = "Ok. Have a nice day.";

	$TownBot[$townbot, NCUE, 1] = "\n\n  <f1>Y<f0>es.\n  <f1>N<f0>o.";
	$TownBot[$townbot, NCUEKEY, 1] = "y n";


	$TownBot[$townbot, SayBye] = "Have a nice day.";
	$TownBot[$townbot, SHOP] = "Rations Cracker Bread Apple Salmon Beef Steak Steak_Sirloin Drink Beer Wine_Cooler Wine Captain_Morgan Moonshine ";


	MakeTownBot("cgoblin",	"goblin002",	"Ooga",	"-264.538 -98.9062 40");

	$TownBot[$townbot, TYPE] = "quester";

	$TownBot[$townbot, NEED] = "acorn 1";
	$TownBot[$townbot, GIVE] = "bread 1";

	$TownBot[$townbot, SAY, 1] = "Deez goblinz are meanz *shrugs* eyes onlyz herez for acornz.";
	$TownBot[$townbot, SAY, 2] = "Eyes would give youz 1 piece of <f1>Bread<f0> for 1 <f1>acorn<f0>.";

	$TownBot[$townbot, CUE, 1] = "\n\n  <f1>A<f0>cornz.\n  <f1>L<f0>atez.";
	$TownBot[$townbot, CUEKEY, 1] = "a l";

	$TownBot[$townbot, NSAY, 1] = "Yay! youz gotz an acorn, can eyes havz itz?";
	$TownBot[$townbot, NSAY, 2] = "Thankez.";

	$TownBot[$townbot, NSAYNO, 1] = "Nutz.";

	$TownBot[$townbot, NCUE, 1] = "\n\n  <f1>Y<f0>ez.\n  <f1>N<f0>o.";
	$TownBot[$townbot, NCUEKEY, 1] = "y n";

	$TownBot[$townbot, SayBye] = "Byez.";
	$TownBot[$townbot, SHOP] = "Cracker ";


	MakeTownBot("cgoblin",	"goblin001",	"Booga",	"-275.427 -97.04 40");

	$TownBot[$townbot, TYPE] = "quester";
	$TownBot[$townbot, NEED] = "q&cure_Potion 1";
	$TownBot[$townbot, GIVE] = "Booga_Nut 1";

	$TownBot[$townbot, SAY, 1] = "All the nize goblinz are in Gooba.";
	$TownBot[$townbot, SAY, 2] = "Iz zity filled wid frendlyz goblinz that iz very far away.";

	$TownBot[$townbot, CUE, 1] = "\n\n  <f1>G<f0>ooba.\n  <f1>B<f0>yez.";
	$TownBot[$townbot, CUEKEY, 1] = "g b";

	$TownBot[$townbot, NSAY, 1] = "Ooohz! <f1>Cure Potion<f0>! mez havz?";
	$TownBot[$townbot, NSAY, 2] = "Thankez! Iz needz thesz to ztayz alivz. Havz zome nutz.";

	$TownBot[$townbot, NQUESTDONE, 1] = "BoogaQuest"; //finish this quest if Client gives item ^

	$TownBot[$townbot, NSAYNO, 1] = "Butz Iz needz thesz to ztayz alivz... Iz havz baz poizon.";

	$TownBot[$townbot, NCUE, 1] = "\n\n  <f1>Y<f0>ez.\n  <f1>N<f0>o.";
	$TownBot[$townbot, NCUEKEY, 1] = "y n";

	$TownBot[$townbot, SayBye] = "Byez.";

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//DENGAN BOTS UPPER SHOP


//////
	//DENGAN BOTS UPPER SHOP


	MakeTownBot("FemaleHumanTownBot",	"Female001",	"Basic Weapons",	"193.2 -862.485 5054.7951");

	$TownBot[$townbot, TYPE] = "merchant";
	$TownBot[$townbot, SHOP] = "Silver_Dagger Steel_Dagger Silver_Sword Steel_Sword Silver_Hatchet Steel_Hatchet Silver_Axe Steel_Axe Silver_Katana Steel_Katana Steel_Baridache Silver_Tip Steel_Tip Acolyte_Rod Necro_Rod ";
	$TownBot[$townbot, SayBye] = "See ya.";


	MakeTownBot("MaleHumanTownBot",	"Male002",	"Premium Weapons",	"197.491 -862.318 5054.7951");

	$TownBot[$townbot, TYPE]="merchant";
	$TownBot[$townbot, SHOP] = "Monster_Killer Avenger Power_Mallet Eye_Poker Ice_Staff ";
	$TownBot[$townbot, SayBye] = "See ya.";


	MakeTownBot("cgoblin",	"goblin001",	"Items",	"187.051 -862.395 5054.7951");

	$TownBot[$townbot, TYPE] = "merchant";
	$TownBot[$townbot, SHOP] = "Light_Potion Mid-Potion Hi-Potion X-Potion Ether Mid-Ether Hi-Ether Antidote Cure_Dark Un-Mute Soft Adv_Compass ";
	$TownBot[$townbot, SayBye] = "See ya.";


	MakeTownBot("FemaleHumanTownBot",	"Female001",	"Accessorys",	"179.953 -862.349 5054.7951");

	$TownBot[$townbot, TYPE] = "merchant";
	$TownBot[$townbot, SHOP] = "Rogue_Gear Plated_Jerkilin Ninja_Gear Copper_ChainMail Bronze_Mail Silver_Mail Military_Uniform Red_Robe Black_Robe Blue_Robe Protect_Shield Copper_Shield Copper_Helmet Copper_Leggings Copper_Boots Copper_Ring Bronze_Gauntlets ";
	$TownBot[$townbot, SayBye] = "See ya.";

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//DENGAN BOTS LOWER SHOP


	MakeTownBot("FemaleHumanTownBot",	"Female001",	"Basic Weapons",	"2632.1 931.557 -4912.42");

	$TownBot[$townbot, TYPE] = "merchant";
	$TownBot[$townbot, SHOP] = "Iron_Dagger Iron_Sword Iron_Hatchet Iron_Katana Iron_Axe Iron_Baridache Iron_Tip Iron_Mallet Prism_Rod Success_Rod Guard_Rod ";
	$TownBot[$townbot, SayBye] = "See ya.";


	MakeTownBot("MaleHumanTownBot",	"Male002",	"Premium Weapons",	"2632.73 927.692 -4912.42");

	$TownBot[$townbot, TYPE] = "merchant";
	$TownBot[$townbot, SHOP] = "Battle_Foil Pride_Killer Military_Spear Dragoon_Lance Judgement_Hammer Military_Hammer Trogla Earth_Staff ";
	$TownBot[$townbot, SayBye] = "See ya.";


	MakeTownBot("cgoblin",	"goblin001",	"Items",	"2633.87 918.519 -4912.42");

	$TownBot[$townbot, TYPE] = "merchant";
	$TownBot[$townbot, SHOP] = "X-Potion Full-Potion X-Ether Full-Ether Antidote Cure_Dark Un-Mute Soft ";
	$TownBot[$townbot, SayBye] = "Seez yaz.";


	MakeTownBot("cgoblin",	"goblin002",	"Accessorys",	"2635.41 909.689 -4912.42");

	$TownBot[$townbot, TYPE] = "merchant";
	$TownBot[$townbot, SHOP] = " Green_Robe Dark_Robe Light_Robe Bronze_Shield Silver_Shield Bronze_Helmet Klepto_Leggings Bronze_Leggings Bronze_Boots Bronze_Gauntlets Dark_Plate Silver_Helmet Silver_Leggings Silver_Boots Silver_Gauntlets ";
	$TownBot[$townbot, SayBye] = "Seez yaz.";


//Gold_Armor Gold_Shield Gold_Helmet Gold_Leggings Gold_Boots Gold_Gauntlets

	//GOOBA BOTS


	MakeTownBot("cgoblin",	"goblin002",	"Basic Weapons",	"329.094 -464.01 -2963.54");

	$TownBot[$townbot, TYPE] = "merchant";
	$TownBot[$townbot, SHOP] = "Mythril_Dagger Mythril_Sword Mythril_Hatchet Mythril_Katana Mythril_Axe Mythril_Baridache Mythril_Tip Mythril_Mallet ";
	$TownBot[$townbot, SayBye] = "See ya.";


	MakeTownBot("cgoblin",	"goblin001",	"Premium Weapons",	"328.314 -459.525 -2963.54");

	$TownBot[$townbot, TYPE] = "merchant";
	$TownBot[$townbot, SHOP] = "Earth_Pounder Bam_Club Goblin_Club Potato_Masher ";
	$TownBot[$townbot, SayBye] = "See ya.";


	MakeTownBot("cgoblin",	"goblin001",	"Items",	"329.974 -474.628 -2963.54");

	$TownBot[$townbot, TYPE] = "merchant";
	$TownBot[$townbot, SHOP] = "X-Potion Full-Potion X-Ether Full-Ether Antidote Cure_Dark Un-Mute Soft Adv_Compass Salmon Bad_Mushroom Dark Mute Rock ";
	$TownBot[$townbot, SayBye] = "Seez yaz.";


	MakeTownBot("cgoblin",	"goblin001",	"Accessorys",	"329.466 -470.155 -2963.54");

	$TownBot[$townbot, TYPE] = "merchant";
	$TownBot[$townbot, SHOP] = "Iron_Helmet Iron_Leggings Iron_Boots Antitoxinal_Helmet Soft_Helmet Loudmouth_Helmet Bright_Helmet Iron_Gauntlets Full_Plate Valorite_PlateMail Robe_of_Darkness Robe_of_Mysticism Elven_robe ";
	$TownBot[$townbot, SayBye] = "Seez yaz.";


	MakeTownBot("cgoblin",	"goblin002",	"Banker",	"331.805 -482.397 -2963.54");

	$TownBot[$townbot, TYPE] = "banker";
	$TownBot[$townbot, SayBye] = "Have a nize day.";


	MakeTownBot("cgoblin",	"goblin002",	"Black Smith",	"-405.147 -387.924 1569.31");
	$TownBot[$townbot, TYPE] = "blacksmith";
	$TownBot[$townbot, SayBye] = "See yaz.";


	MakeTownBot("invisibleman",	"goblin001",	"ChiefCaracal",	"247.666 -477.868 -2962.53");

	$TownBot[$townbot, TYPE] = "quester";
	$TownBot[$townbot, NEED] = "Practice_Sword 99";
	$TownBot[$townbot, GIVE] = "Practice_Sword 99 ";

	$TownBot[$townbot, SAY, 1] = "Please don't talk to me while I am making a speech.";
	$TownBot[$townbot, SAY, 2] = "I already said don't talk to me!";

	$TownBot[$townbot, CUE, 1] = "\n\n  <f1>.<f0>...";
	$TownBot[$townbot, CUEKEY, 1] = ".";

	$TownBot[$townbot, NSAY, 1] = "Wow, 99 practice swords! We could sure use those...";
	$TownBot[$townbot, NSAY, 2] = "You're welcome!";

	$TownBot[$townbot, NCUE, 1] = "\n\n  <f1>S<f0>ure.\n  <f1>N<f0>o way!";
	$TownBot[$townbot, NCUEKEY, 1] = "s n";
	$TownBot[$townbot, NSAYNO, 1] = "Oh, well you can just leave then!";

	$TownBot[$townbot, SayBye] = " ...";


//RANDOM TOWNBOTS
	MakeTownBot("FemaleHumanTownBot",	"Female001",	"Shildra",	"436.27 -425.191 210.05");

	$TownBot[$townbot, TYPE] = "quester";
	$TownBot[$townbot, NEED] = "COINS 100";
	$TownBot[$townbot, GIVE] = "room_key 1";

	$TownBot[$townbot, SAY, 1] = "I'm sorry, our rooms are all filled for tonight.";
	$TownBot[$townbot, SAY, 2] = "You can have a <f1>Room Key<f0> for <f1>100 Gil<f0>.";

	$TownBot[$townbot, CUE, 1] = "\n\n  <f1>R<f0>oom.\n  <f1>N<f0>evermind.";
	$TownBot[$townbot, CUEKEY, 1] = "r n";

	$TownBot[$townbot, NSAY, 1] = "Hello there, looking for a room? It's only <f1>100gil<f0> per stay.";
	$TownBot[$townbot, NSAY, 2] = "Thanks, here is your room key. The room is to the left of my desk.";

	$TownBot[$townbot, NSAYNO, 1] = "Oh, ok, come visit again sometime.";

	$TownBot[$townbot, NCUE, 1] = "\n\n  <f1>Y<f0>es.\n  <f1>N<f0>o.";
	$TownBot[$townbot, NCUEKEY, 1] = "y n";

	$TownBot[$townbot, SayBye] = "Good Night.";

	MakeTownBot("FemaleHumanTownBot",	"Female001",	"Keia",	"-264.991 -64.6794 40");

	$TownBot[$townbot, TYPE] = "quester";
	$TownBot[$townbot, NEED] = "Zombie_Powder 99 COINS 9";
	$TownBot[$townbot, GIVE] = "room_key 1";

	$TownBot[$townbot, SAY, 1] = "Its dangerous in the sewers you shouldnt \n go down there unless you are level <f1>60<f0>.";
	$TownBot[$townbot, SAY, 2] = "Its dangerous in the sewers you shouldnt \n go down there unless you are level <f1>60<f0>.";

	$TownBot[$townbot, CUE, 1] = "\n\n  <f1>T<f0>hanks.\n  <f1>I<f0>gnore.";
	$TownBot[$townbot, CUEKEY, 1] = "t i";

	$TownBot[$townbot, NSAY, 1] = "Its dangerous in the sewers you shouldnt \n go down there unless you are level <f1>60<f0>.";
	$TownBot[$townbot, NSAY, 2] = "Hey bud thanks for listening to me.";

	$TownBot[$townbot, NSAYNO, 1] = "Screw you! im only trying to help.";

	$TownBot[$townbot, NCUE, 1] = "\n\n  <f1>Y<f0>es.\n  <f1>N<f0>o.";
	$TownBot[$townbot, NCUEKEY, 1] = "y n";

	$TownBot[$townbot, SayBye] = "Good Night.";



	////////////////////////////////////////
	$RM::KeyWords = $RM::KeyWords@" ";//
	echo("TownBots setup - Completed.");	//
}

function MakeTownBot(%shape, %pic, %name, %pos) {

	$townbot = newObject("", "Item", %shape, 1, false);
	//$townbot = newObject("", "StaticShape", %shape, true);

	$townbotname = %name;
	addToSet("MissionCleanup", $townbot);
	GameBase::setMapName($townbot, $townbotname);
	GameBase::setPosition($townbot, %pos);
	GameBase::setTeam($townbot, 0);
	GameBase::playSequence($townbot, 0, "root");

	$TownBotList = $TownBotList @ $townbot @ " ";

	$TownBot[$townbot, NAME] = $townbotname;
	$TownBot[$townbot, PIC] = %pic;
}

