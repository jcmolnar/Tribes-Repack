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

function _SetUpItems() {//Only the server will need to call this

	blackSmithStuff();

//Dataname len limited (keep under 40...)
// 1234567890123456789012345678901234567890
// That is too long of a name anyways...
// Max recommended is 30 or so.
//
//Calling MakeItem in any order will not effect anything UNLESS making 2 items with the same dataname!
//
//
//MakeItem(Name, Dataname,  svar, 					type,   weight,      info, 					   header,    shape,       className,  Equip, ",GroupRestrictions,") //Note on Equip, if your making an armor set this to the skin, if its just a ring or other type with no skin just set to true (See ARMORS)
//
//	Utilities
MakeItem("Light Potion", "Light Potion", "HEAL 100",				"NULL", 1, "A Potion that restores 100 HP",	$Headers::M, "NULL", "Potion", false);
$ItemCost["Light_Potion"] = 25;
MakeItem("Mid-Potion", "Mid-Potion", "HEAL 500",	"NULL", 2, "A Potion that restores 500 HP",	$Headers::M, "NULL", "Potion", false);
$ItemCost["Mid-Potion"] = 100;
MakeItem("Hi-Potion", "Hi-Potion", "HEAL 5000",		"NULL", 2, "A Potion that restores 5000 HP",	$Headers::M, "NULL", "Potion", false);
$ItemCost["Hi-Potion"] = 500;
MakeItem("X-Potion", "X-Potion", "HEAL 10000",		"NULL", 5, "A Potion that restores 10000 HP",	$Headers::M, "NULL", "Potion", false);
$ItemCost["X-Potion"] = 1000;
MakeItem("Full-Potion", "Full-Potion", "HEAL 99999",	"NULL", 10, "A Potion that restores all HP",	$Headers::M, "NULL", "Potion", false);
$ItemCost["Full-Potion"] = 5000;
MakeItem("Ether", "Ether", "MP 25",					"NULL", 1, "A Potion that restores 25 MP",	$Headers::M, "NULL", "Potion", false);
$ItemCost["Ether"] = 50;
MakeItem("Mid-Ether", "Mid-Ether", "MP 50",			"NULL", 2, "A Potion that restores 50 MP",	$Headers::M, "NULL", "Potion", false);
$ItemCost["Mid-Ether"] = 100;
MakeItem("Hi-Ether", "Hi-Ether", "MP 100",			"NULL", 3, "A Potion that restores 100 MP",	$Headers::M, "NULL", "Potion", false);
$ItemCost["Hi-Ether"] = 200;
MakeItem("X-Ether", "X-Ether", "MP 500",			"NULL", 4, "A Potion that restores 500 MP",	$Headers::M, "NULL", "Potion", false);
$ItemCost["X-Ether"] = 1000;
MakeItem("Full-Ether", "Full-Ether", "MP 999",		"NULL", 5, "A Potion that restores all MP",	$Headers::M, "NULL", "Potion", false);
$ItemCost["Full-Ether"] = 5000;

MakeItem("Elixir", "Elixir", "HEAL 100 MP 50",		"NULL", 6, "A Potion that restores 100 HP and 50 MP",	$Headers::M, "NULL", "Potion", false);
$ItemCost["Elixer"] = 1000;
MakeItem("Hyper Elixir", "Hyper Elixir", "HEAL 5000 MP 500",		"NULL", 8, "A Potion that restores 5000 HP and 500 MP",	$Headers::M, "NULL", "Potion", false);
$ItemCost["Hyper_Elixer"] = 5000;
MakeItem("Mega Elixir", "Mega Elixir", "HEAL 99999 MP 999",		"NULL", 11, "A Potion that restores all HP and all MP",	$Headers::M, "NULL", "Potion", false);
$ItemCost["Mega_Elixer"] = 10000;

MakeItem("Tent", "Tent", "NULL",	"NULL",	10, "A tent. Use #camp to it setup and #sleep in it.",	$Headers::R, "NULL", "Tent", false);
$ItemCost["Tent"] = 5000;

//	FOOD
MakeItem("Rations", "Rations", "STA 2",			"NULL", "0.1", "Some rations. Restores 2 STAMINA.", 	$Headers::O, "NULL", "Food", false);
MakeItem("Cracker", "Cracker", "STA 6",			"NULL", "0.1", "A Cracker. Restores 6 STAMINA.", 		$Headers::O, "NULL", "Food", false);
MakeItem("Bread", "Bread", "STA 15",				"NULL", "0.2", "A tasty Slice of bread. Restores 14 STAMINA.",	$Headers::O, "NULL", "Food", false);
MakeItem("Apple", "Apple", "STA 20",				"NULL", "0.2", "An Apple. Restores 20 STAMINA.",					$Headers::O, "NULL", "Food", false);
MakeItem("Salmon", "Salmon", "STA 50",			"NULL", "1", "Salmon meat from fish caught at port town. Restores 50 STAMINA.",	$Headers::O, "NULL", "Food", false);
MakeItem("Beef", "Beef", "STA 100",				"NULL", "1.2", "A piece of Beef. Restores 100 STAMINA.",			$Headers::O, "NULL", "Food", false);
MakeItem("Steak", "Steak", "STA 200",			"NULL", "1.2", "A large, healthy slice of steak. Restores 200 STAMINA.",	$Headers::O, "NULL", "Food", false);
MakeItem("Steak Sirloin", "Steak Sirloin", "STA 500",	"NULL", "2", "A tender Steak Sirloin. Restores 500 STAMINA.",	$Headers::O, "NULL", "Food", false);
MakeItem("Uber Burger","Uber Burger","STA 999",		"NULL", "5", "A Uber Burger. Restores all STAMINA.",				$Headers::O, "NULL", "Food", false);

//	CURE POTIONS (STATUS)
MakeItem("Antidote", "Antidote", "21 Poison",		"NULL", "0.1", "A cure for Poison.",	$Headers::N, "NULL", "Cure Potion", false);
MakeItem("Cure Dark", "Cure Dark", "21 Blind",	"NULL", "0.1", "A cure for Blind.",		$Headers::N, "NULL", "Cure Potion", false);
MakeItem("Un-Mute", "Un-Mute", "21 Mute",		"NULL", "0.1", "A cure for Mute.",		$Headers::N, "NULL", "Cure Potion", false);
MakeItem("Soft", "Soft", "21 Petrify",			"NULL", "0.1", "A cure for Petrify. Stand near the stone to use this cure.",		$Headers::N, "NULL", "Cure Potion", false);

//AREA only coded for these potions right now.
//AREA syntax
//AREA <radius> <status> <pow> [<status> <pow>...]

//	STATUS POTIONS
MakeItem("Poison Dust", "Poison Dust", "30 5",		"NULL", "0.2", "A small bag of poison dust.",		$Headers::P, "NULL", "Status Potion", false);
MakeItem("Bad Mushroom", "Bad Mushroom", "30 25",		"NULL", "0.2", "A Bad Mushroom that Poisons.",		$Headers::P, "NULL", "Status Potion", false);
MakeItem("Mad Mushroom", "Mad Mushroom", "30 50",	"NULL", "0.4", "A Mad Mushroom that Poisons.",		$Headers::P, "NULL", "Status Potion", false);
MakeItem("Poison Cloud", "Poison Cloud", "AREA 15 30 100",		"NULL", 1, "A bag of Poison powder. when used, poisons everyone in the area.",	$Headers::P, "NULL", "Status Potion", false);
MakeItem("Magic Poison Cloud", "Magic Poison Cloud", "AREA 25 30 200",		"NULL", 1, "A bag of Magic Poison powder. when used, poisons everyone in the area.",	$Headers::P, "NULL", "Status Potion", false);

MakeItem("Dark", "Dark", "31 25",		"NULL", 1, "Causes Blind.",	$Headers::P, "NULL", "Status Potion", false);
MakeItem("Ink", "Ink", "31 75",		"NULL", 1, "Causes Blind.",	$Headers::P, "NULL", "Status Potion", false);
MakeItem("Flash", "Flash", "31 125",	"NULL", 1, "Causes Blind.",	$Headers::P, "NULL", "Status Potion", false);
MakeItem("Flash Bomb", "Flash Bomb", "AREA 25 31 150",	"NULL", 2, "Causes Blind to your nearby victim(s).",	$Headers::P, "NULL", "Status Potion", false);

MakeItem("Mute", "q&Mute", "32 25",			"NULL", 1, "Mutes your victim.",	$Headers::P, "NULL", "Status Potion", false);
MakeItem("Mute x2", "Mute x2", "32 50",	"NULL", 1, "Mutes your victim.",	$Headers::P, "NULL", "Status Potion", false);
MakeItem("Mute Cloud", "Mute Cloud", "AREA 15 32 125",	"NULL", 1, "Mutes your nearby victim(s).",	$Headers::P, "NULL", "Status Potion", false);

MakeItem("Rock", "Rock", "33 25",			"NULL", 1, "Turn your victim to stone.",		$Headers::P, "NULL", "Status Potion", false);
MakeItem("Petrify", "Petrify", "33 50",		"NULL", 1, "Turn your victim to stone.",		$Headers::P, "NULL", "Status Potion", false);
MakeItem("Petrify Cloud", "Petrify Cloud", "AREA 15 33 100",		"NULL", 1, "Turn your nearby victim(s) to stone.",	$Headers::P, "NULL", "Status Potion", false);

MakeItem("Status Hell Cloud", "Status Hell Cloud", "AREA 25 31 100 32 100 33 100", "NULL", 2, "Status cloud from hell.", $Headers::P, "NULL", "Status Potion", false);

MakeItem("Cookie", "Cookie", "NULL",	"NULL", 1, "A Cookie for FENIX",	$Headers::N, "NULL", "Potion", false);
$ItemDataOnUseFunc["Cookie"] = True;
function Item::CookieOnUse(%player, %Client, %item) {

	refreshHP(%Client, -1);
	refreshSTAMINA(%Client, -25);
	Client::addItemCount(%Client, %item, -1);
	refreshAll(%Client);
	remoteSay(%Client, 0, "#shout Yum! yum! cookie!");
}
MakeItem("Potato", "Potato", "NULL",	"NULL", 1, "A magic potato!",	$Headers::N, "NULL", "Potion", false);
$ItemDataOnUseFunc["Potato"] = True;
function Item::PotatoOnUse(%player, %Client, %item) {

	refreshHP(%Client, -1);
	refreshSTAMINA(%Client, -25);
	Client::addItemCount(%Client, %item, -1);
	refreshAll(%Client);
	remoteSay(%Client, 0, "#shout WOWZIES! THAT WAS A SPICY POTATO!");
}
MakeItem("Milk", "Milk", "NULL",	"NULL", 1, "Some Milk",	$Headers::N, "NULL", "Potion", false);
$ItemDataOnUseFunc["Milk"] = True;
function Item::MilkOnUse(%player, %Client, %item) {

	refreshHP(%Client, -1);
	refreshSTAMINA(%Client, -25);
	Client::addItemCount(%Client, %item, -1);
	refreshAll(%Client);
	remoteSay(%Client, 0, "#shout *gulp* *gulp*");
}
MakeItem("Coca-Cola", "Coca Cola", "NULL",	"NULL", 1, "Some coke.",	$Headers::N, "NULL", "Potion", false);
$ItemDataOnUseFunc["Coca_Cola"] = True;
function Item::Coca_ColaOnUse(%player, %Client, %item) {

	refreshHP(%Client, -1);
	refreshSTAMINA(%Client, -25);
	Client::addItemCount(%Client, %item, -1);
	refreshAll(%Client);
	remoteSay(%Client, 0, "#shout aaah... refreshing!*");
}
MakeItem("Teleport Scroll", "Teleport Scroll", "NULL",	"NULL", 1, "A scroll that allows teleportation to <f1>Rin Vale<f0>.",	$Headers::N, "NULL", "Potion", false);
$ItemCost["Teleport_Scroll"] = 10000;
$ItemDataOnUseFunc["Teleport_Scroll"] = True;
function Item::Teleport_ScrollOnUse(%player, %Client, %item)
{
	if($SpellCastStep[%client] == 1)
		Client::sendMessage(%client, 0, "Your already casting something else.");
	else if($SpellCastStep[%client] == 2)
		Client::sendMessage(%client, 0, "You are still recovering from your last casting.");
	else
	{
		if(BeginCastSpell(%client, "barqprz",0) == True)//pem315
		{
			Client::addItemCount(%Client, %item, -1);
		}
	}
}

//	ALCOHOL
MakeItem("Drink", "Drink", "Alvl 0.2",					"NULL", "1", "A alcoholic Drink. Alcohol lvl 0.2",				$Headers::T, "NULL", "Alcohol", false);
MakeItem("Beer", "Beer", "Alvl 1",						"NULL", "1", "A alcoholic Drink. Alcohol lvl 1",				$Headers::T, "NULL", "Alcohol", false);
MakeItem("Wine Cooler", "Wine Cooler", "Alvl 10",	"NULL", "1", "A alcoholic Drink. Alcohol lvl 10",				$Headers::T, "NULL", "Alcohol", false);
MakeItem("Wine", "Wine", "Alvl 20",					"NULL", "1", "A alcoholic Drink. Alcohol lvl 20",				$Headers::T, "NULL", "Alcohol", false);
MakeItem("Captain Morgan", "Captain Morgan", "Alvl 40",		"NULL", "1", "A alcoholic Drink. Alcohol lvl 40",		$Headers::T, "NULL", "Alcohol", false);
MakeItem("The Special", "The Special", "Alvl 60",				"NULL", "1", "The in house specialty.",			$Headers::T, "NULL", "Alcohol", false);
MakeItem("Moonshine", "Moonshine", "Alvl 100",				"NULL", "1", "A alcoholic Drink. Alcohol lvl 100, this shit will knock your socks off!",			$Headers::T, "NULL", "Alcohol", false);

//============================================================================================
//===================================== WEAPON DATA =========================================
//============================================================================================
MakeItem("Dummy", "Dummy", "6 1",	"NULL", 1, "NULL", $Headers::B, "Mace", "Weapon"); //Do not remove!
//Keep this the first made weapon


// ENEMY WEAPONS																															 //For Bot weapons that only bots use, use NULL for GroupRestrictions (This will add to the shape model "Bot"@%shape
MakeItem("esmissile","esmissile", "6 3d12+200",			$SwordAccessoryType, 1, "A missile used by cyborgs.", $Headers::B, "cyborggun", "Weapon", false, "NULL",	$SlashingDamageType, 50, 3.5, esmissilesound, cyborgswitch);
MakeItem("cuttinglaser", "cuttinglaser", "6 3d12+150",	$SwordAccessoryType, 1, "A powerful laser used by cyborgs.", $Headers::B, "cyborggun", "Weapon", false, "NULL",	$SlashingDamageType, 50, 3.5);
function Botcyborggun35Image::onFire(%player, %slot) {//HAVE TO DO IF, ELSE IF, BECUASE TWO OR MORE MONSTER USE THE SAME WEAPON MODEL!
	%cl = GameBase::getOwnerClient(%player); $los::object = "";
	if($ClientData[%cl, UsingWeapon] == "esmissile") {
		GameBase::getLOSinfo(%player, $maxSpellRange);
		esrockets.Weapon_SpellAttack(%cl, %player, $los::object);
	}
	else if($ClientData[%cl, UsingWeapon] == "cuttinglaser") {
		GameBase::getLOSinfo(%player, $maxSpellRange);
		cuttinglaser.Weapon_SpellAttack(%cl, %player, $los::object);//hardcoded type
	}

	//Need this here
	MeleeAttack(%player, $minSpellRange, $ClientData[%cl, UsingWeapon]);
}

MakeItem("runt goblinpunch", "runt goblinpunch", "6 10 2 100",				$SwordAccessoryType, 1, "A punch from a goblin.", $Headers::B, "boulder", "Weapon", false, "NULL",		$BludgeoningDamageType, 1, 1.5);
MakeItem("goblinpunch", "goblinpunch", "6 25 1 50 2 100",				$SwordAccessoryType, 1, "A punch from a goblin.", $Headers::B, "boulder", "Weapon", false, "NULL",		$BludgeoningDamageType, 1, 1.5);
MakeItem("screech", "screech", "6 50 1 50 2 100",				$SwordAccessoryType, 1, "A very loud screech.", $Headers::B, "boulder", "Weapon", false, "NULL",			$BludgeoningDamageType, 50, 3.5);
// NOTE: screech shares the "boulder"/3.5 model (Botboulder35Image) with dodgethis/dodgethisc;
// its onFire branch lives in the single merged Botboulder35Image::onFire defined further below.
MakeItem("uuagscreech", "uuagscreech", "6 3d30+10",		$SwordAccessoryType, 1, "A very loud screech.", $Headers::B, "plant2", "Weapon", false, "NULL",			$BludgeoningDamageType, 50, 1);
function Botboulder1Image::onFire(%player, %slot) {
	%cl = GameBase::getOwnerClient(%player); $los::object = "";
	if($ClientData[%cl, UsingWeapon] == "uuagscreech") {
		GameBase::getLOSinfo(%player, $maxSpellRange);
		screechbolt.Weapon_SpellAttack(%cl, %player, $los::object);
		screechbolt.Weapon_SpellAttack(%cl, %player, $los::object);
	}
	MeleeAttack(%player, $minSpellRange, $ClientData[%cl, UsingWeapon]);
}
MakeItem("fspear", "fspear", "6 150 1 75 2 200",					$SwordAccessoryType, 1, "Flaming Spears.", $Headers::B, "trident", "Weapon", false, "NULL",					$PiercingDamageType, 50, 2);
function BotTrident2Image::onFire(%player, %slot) {
	%cl = GameBase::getOwnerClient(%player); $los::object = "";
	if($ClientData[%cl, UsingWeapon] == "fspear") {
		GameBase::getLOSinfo(%player, $maxSpellRange);
		fspearshot.Weapon_SpellAttack(%cl, %player, $los::object);
	}
	MeleeAttack(%player, $minSpellRange, $ClientData[%cl, UsingWeapon]);
}
MakeItem("combustible", "combustible", "6 3d22+400",	$SwordAccessoryType, 1, "Very very hot and toasty.", $Headers::B, "cyborggun", "Weapon", false, "NULL",				$SlashingDamageType, 50, 5);
MakeItem("deathcoil", "deathcoil", "6 3d22+100",			$SwordAccessoryType, 1, "Zombies rip away a part of your health.", $Headers::B, "sword", "Weapon", false, "NULL",	$SlashingDamageType, 50, 2);
function Botsword5lImage::onFire(%player, %slot) {
	%cl = GameBase::getOwnerClient(%player); $los::object = "";
	if($ClientData[%cl, UsingWeapon] == "deathcoil") {
		GameBase::getLOSinfo(%player, $maxSpellRange);
		deathcoilshot.Weapon_SpellAttack(%cl, %player, $los::object);
	}
	MeleeAttack(%player, $minSpellRange, $ClientData[%cl, UsingWeapon]);
}
MakeItem("double", "double", "6 3d12+100",				$SwordAccessoryType, 1, "Zombies create a double to attack you.", $Headers::B, "sword", "Weapon", false, "NULL",	$SlashingDamageType, 50, 4);
function Botsword4Image::onFire(%player, %slot) {
	%cl = GameBase::getOwnerClient(%player); $los::object = "";
	if($ClientData[%cl, UsingWeapon] == "double") {
		GameBase::getLOSinfo(%player, $maxSpellRange);
		doubleshot.Weapon_SpellAttack(%cl, %player, $los::object);
	}
	MeleeAttack(%player, $minSpellRange, $ClientData[%cl, UsingWeapon]);
}
MakeItem("throwstones", "throwstones", "6 3d12+100",	$SwordAccessoryType, 1, "Stones to throw.", $Headers::B, "elfinblade", "Weapon", false, "NULL",	$BludgeoningDamageType, 50, 5);
MakeItem("lightbeam", "lightbeam", "6 3d12+100",		$SwordAccessoryType, 1, "Stones to throw.", $Headers::B, "elfinblade", "Weapon", false, "NULL",	$BludgeoningDamageType, 100, 1);
function Botelfinblade5Image::onFire(%player, %slot) {
	%cl = GameBase::getOwnerClient(%player); $los::object = "";
	if($ClientData[%cl, UsingWeapon] == "throwstones") {
		GameBase::getLOSinfo(%player, $maxSpellRange);
		amazonthrowstones.Weapon_SpellAttack(%cl, %player, $los::object);
	}
	else if($ClientData[%cl, UsingWeapon] == "lightbeam") {
		GameBase::getLOSinfo(%player, $maxSpellRange);
		amazonlightbeam.Weapon_SpellAttack(%cl, %player, $los::object);
	}
	MeleeAttack(%player, $minSpellRange, $ClientData[%cl, UsingWeapon]);
}
MakeItem("dodgethis", "dodgethis", "6 500",				$SwordAccessoryType, 1, "Better not get hit.", $Headers::B, "boulder", "Weapon", false, "NULL",			$BludgeoningDamageType, 50, 3.5);
MakeItem("dodgethis", "dodgethisc", "6 1999 13 15000 15 10000",$SwordAccessoryType,1,"Better not get hit.",$Headers::B, "boulder", "Weapon", false, "NULL",$BludgeoningDamageType, 50, 3.5);

function Botboulder35Image::onFire(%player, %slot) {
	%cl = GameBase::getOwnerClient(%player); $los::object = "";
	if($ClientData[%cl, UsingWeapon] == "screech") {
		GameBase::getLOSinfo(%player, $maxSpellRange);
		screechbolt.Weapon_SpellAttack(%cl, %player, $los::object);
	}
	else if($ClientData[%cl, UsingWeapon] == "dodgethis" || $ClientData[%cl, UsingWeapon] == "dodgethisc") {
		GameBase::getLOSinfo(%player, $maxSpellRange);
		dodgethisbolt.Weapon_SpellAttack(%cl, %player, $los::object, 1);
	}
	MeleeAttack(%player, $minSpellRange, $ClientData[%cl, UsingWeapon]);
}
MakeItem("fishspitshooter", "fishspitshooter", "6 150",				$SwordAccessoryType, 1, "Better not get hit.", $Headers::B, "invisable", "Weapon", false, "NULL",			$BludgeoningDamageType, 50, 3.5);
function BotInvisable35Image::onFire(%player, %slot) {
	%cl = GameBase::getOwnerClient(%player); $los::object = "";
	if($ClientData[%cl, UsingWeapon] == "fishspitshooter") {
		GameBase::getLOSinfo(%player, $maxSpellRange);
		fishspit.Weapon_SpellAttack(%cl, %player, $los::object);
	}
	MeleeAttack(%player, $minSpellRange, $ClientData[%cl, UsingWeapon]);
}

MakeItem("zombiesworda", "zombiesworda", "6 1",				$SwordAccessoryType, 1, "A sword.", $Headers::B, "sword", "Weapon", false, "NULL",		$BludgeoningDamageType, 3, 2);
MakeItem("zombieswordb", "zombieswordb", "6 300 7 150 15 400",				$SwordAccessoryType, 1, "A sword.", $Headers::B, "sword", "Weapon", false, "NULL",		$BludgeoningDamageType, 3, 2);

MakeItem("uuagweapon", "uuagweapon", "6 400",				$SwordAccessoryType, 1, "A sword.", $Headers::B, "axe", "Weapon", false, "NULL",		$BludgeoningDamageType, 3, 2);
MakeItem("godeyesword", "godeyesword", "6 800 15 10000",				$SwordAccessoryType, 1, "A sword.", $Headers::B, "elfinblade", "Weapon", false, "NULL",		$BludgeoningDamageType, 3, 2);
MakeItem("godeyesword", "godeyeswordb", "6 1000 15 100000",$SwordAccessoryType, 1, "A sword.", $Headers::B, "elfinblade", "Weapon", false, "NULL",	$BludgeoningDamageType, 3, 2);
MakeItem("godeyesword", "godeyeswordc", "6 1500 15 150000",$SwordAccessoryType, 1, "A sword.", $Headers::B, "elfinblade", "Weapon", false, "NULL",	$BludgeoningDamageType, 3, 1.5);
MakeItem("golemaxe", "golemaxe", "6 400",				$SwordAccessoryType, 1, "A axe.", $Headers::B, "BattleAxe", "Weapon", false, "NULL",		$BludgeoningDamageType, 3, 2);
MakeItem("ghostweapon", "ghostweapon", "6 100 2 10000",				$SwordAccessoryType, 1, "A weapon that is not really there...", $Headers::B, "invisable", "Weapon", false, "NULL",		$BludgeoningDamageType, 1, 1.5);
MakeItem("blobweapon", "blobweapon", "6 500 2 10000",				$SwordAccessoryType, 1, "A weapon that is not really there...", $Headers::B, "domefiled", "Weapon", false, "NULL",		$BludgeoningDamageType, 3, 1.5);
MakeItem("guardsword", "guardbow", "6 6666 2 10000 1 10000 3 10000",				$SwordAccessoryType, 1, "A weapon that is not really there...", $Headers::B, "long_sword", "Weapon", false, "NULL",		$BludgeoningDamageType, 3, 1.5);
MakeItem("guardbow", "guardsword", "6 6666 2 10000 1 10000 3 10000",				$SwordAccessoryType, 1, "A weapon that is not really there...", $Headers::B, "comp_bow", "Weapon", false, "NULL",		$BludgeoningDamageType, 40, 2);

MakeItem("gnollspeara", "gnollspeara", "6 150 1 75 2 500",				$SwordAccessoryType, 1, "A gnoll spear.", $Headers::B, "Trident", "Weapon", false, "NULL",		$BludgeoningDamageType, 4, 3);

MakeItem("grusword", "grusword", "6 400 1 75 2 800 4 -550 5 -550",				$SwordAccessoryType, 1, "A gru spear.", $Headers::B, "elfinblade", "Weapon", false, "NULL",		$BludgeoningDamageType, 4, 2);
MakeItem("grunganaxe", "grunganaxe", "6 400 1 100 2 800 4 -550 5 -550 7 100 15 -200",				$SwordAccessoryType, 1, "A axe.", $Headers::B, "BattleAxe", "Weapon", false, "NULL",		$BludgeoningDamageType, 6, 8);

MakeItem("Zompot", "zompot", "30 20",		"NULL", "0.2", "A small pouch of poisonous powder.",		$Headers::P, "NULL", "Status Potion", false);

//RANDOM SPELL SHOOTER EXAMPLE
MakeItem("Goblin Chief Caster", "Goblin Chief Caster", "6 10 2 60",				$SwordAccessoryType, 1, "Spell Shooter", $Headers::B, "boulder", "Weapon", false, "NULL",			$BludgeoningDamageType, 50, 4);
MakeItem("Spell Shooter", "Spell Shooter", "6 500",				$SwordAccessoryType, 1, "Spell Shooter", $Headers::B, "orb", "Weapon", false, "NULL",			$BludgeoningDamageType, 50, 4);
MakeItem("Low Lich Caster", "Low Lich Caster", "6 300 7 -15",				$SwordAccessoryType, 1, "CastingWeapon for lich", $Headers::B, "orb", "Weapon", false, "NULL",			$BludgeoningDamageType, 50, 4);
MakeItem("High Lich Caster", "High Lich Caster", "6 500 7 -15",				$SwordAccessoryType, 1, "CastingWeapon for lich", $Headers::B, "orb", "Weapon", false, "NULL",			$BludgeoningDamageType, 50, 4);
MakeItem("Rock Giant Caster", "Rock Giant Caster", "6 800 7 80 15 -15",				$SwordAccessoryType, 1, "CastingWeapon for Rock Giant", $Headers::B, "orb", "Weapon", false, "NULL",			$BludgeoningDamageType, 50, 4);

function BotOrb4Image::onFire(%player, %slot) {
	%cl = GameBase::getOwnerClient(%player); $los::object = "";
	if($ClientData[%cl, UsingWeapon] == "Spell_Shooter") {
		GameBase::getLOSinfo(%player, $maxSpellRange);
															//ADD AS MANY AS YOU LIKE
		AI::ShootRandomSpell(%cl, %player, $los::object, "flash flash2 flash3");
	}
	else if($ClientData[%cl, UsingWeapon] == "Goblin_Chief_Caster") {
		GameBase::getLOSinfo(%player, $maxSpellRange);
		AI::ShootRandomSpell(%cl, %player, $los::object, "medic spike flash cold");
	}
	else if($ClientData[%cl, UsingWeapon] == "Low_Lich_Caster") {
		GameBase::getLOSinfo(%player, $maxSpellRange);
		AI::ShootRandomSpell(%cl, %player, $los::object, "medic shatter shatter2");
	}
	else if($ClientData[%cl, UsingWeapon] == "High_Lich_Caster") {
		GameBase::getLOSinfo(%player, $maxSpellRange);
		AI::ShootRandomSpell(%cl, %player, $los::object, "medic flash2 shatter2 cold2 storm2 darkshot");
	}
	else if($ClientData[%cl, UsingWeapon] == "Rock_Giant_Caster") {
		GameBase::getLOSinfo(%player, $maxSpellRange);
		AI::ShootRandomSpell(%cl, %player, $los::object, "medic shatter shatter2 shatter3 shatter4");
	}

	MeleeAttack(%player, $minSpellRange, $ClientData[%cl, UsingWeapon]);
}



//DO NOT DELETE THIS WEAPON
MakeItem("Practice Sword", "Practice Sword", "6 1",		$SwordAccessoryType, 2, "Initial Equipment.",	$Headers::B, "sword", "Weapon", false, ",Priest,Warrior,Rogue,Wizard,",	$SlashingDamageType, 3, 1.5, SoundSwing6);
//						->												->													->		 				 		->										//	%damagetype,     %range,%delay, %sound, %activatesound)

//BASIC WEAPONS
SmithCombo($ServerClient, "Ivory Dagger");
SmithCombo($ServerClient, "Brass Dagger");
SmithCombo($ServerClient, "Copper Dagger");
SmithCombo($ServerClient, "Bronze Dagger");
SmithCombo($ServerClient, "Silver Dagger");
SmithCombo($ServerClient, "Gold Dagger");
SmithCombo($ServerClient, "Iron Dagger");
SmithCombo($ServerClient, "Steel Dagger");
SmithCombo($ServerClient, "Platinum Dagger");
SmithCombo($ServerClient, "Mythril Dagger");
SmithCombo($ServerClient, "Titanium Dagger");

SmithCombo($ServerClient, "Ivory ShortSword");
SmithCombo($ServerClient, "Brass ShortSword");
SmithCombo($ServerClient, "Copper ShortSword");
SmithCombo($ServerClient, "Bronze ShortSword");
SmithCombo($ServerClient, "Silver ShortSword");
SmithCombo($ServerClient, "Gold ShortSword");
SmithCombo($ServerClient, "Iron ShortSword");
SmithCombo($ServerClient, "Steel ShortSword");
SmithCombo($ServerClient, "Platinum ShortSword");
SmithCombo($ServerClient, "Mythril ShortSword");
SmithCombo($ServerClient, "Titanium ShortSword");

SmithCombo($ServerClient, "Ivory Sword");
SmithCombo($ServerClient, "Brass Sword");
SmithCombo($ServerClient, "Copper Sword");
SmithCombo($ServerClient, "Bronze Sword");
SmithCombo($ServerClient, "Silver Sword");
SmithCombo($ServerClient, "Gold Sword");
SmithCombo($ServerClient, "Iron Sword");
SmithCombo($ServerClient, "Steel Sword");
SmithCombo($ServerClient, "Platinum Sword");
SmithCombo($ServerClient, "Mythril Sword");
SmithCombo($ServerClient, "Titanium Sword");

SmithCombo($ServerClient, "Ivory LongSword");
SmithCombo($ServerClient, "Brass LongSword");
SmithCombo($ServerClient, "Copper LongSword");
SmithCombo($ServerClient, "Bronze LongSword");
SmithCombo($ServerClient, "Silver LongSword");
SmithCombo($ServerClient, "Gold LongSword");
SmithCombo($ServerClient, "Iron LongSword");
SmithCombo($ServerClient, "Steel LongSword");
SmithCombo($ServerClient, "Platinum LongSword");
SmithCombo($ServerClient, "Mythril LongSword");
SmithCombo($ServerClient, "Titanium LongSword");

SmithCombo($ServerClient, "Ivory Hatchet");
SmithCombo($ServerClient, "Brass Hatchet");
SmithCombo($ServerClient, "Copper Hatchet");
SmithCombo($ServerClient, "Bronze Hatchet");
SmithCombo($ServerClient, "Silver Hatchet");
SmithCombo($ServerClient, "Gold Hatchet");
SmithCombo($ServerClient, "Iron Hatchet");
SmithCombo($ServerClient, "Steel Hatchet");
SmithCombo($ServerClient, "Platinum Hatchet");
SmithCombo($ServerClient, "Mythril Hatchet");
SmithCombo($ServerClient, "Titanium Hatchet");

SmithCombo($ServerClient, "Ivory Katana");
SmithCombo($ServerClient, "Brass Katana");
SmithCombo($ServerClient, "Copper Katana");
SmithCombo($ServerClient, "Bronze Katana");
SmithCombo($ServerClient, "Silver Katana");
SmithCombo($ServerClient, "Gold Katana");
SmithCombo($ServerClient, "Iron Katana");
SmithCombo($ServerClient, "Steel Katana");
SmithCombo($ServerClient, "Platinum Katana");
SmithCombo($ServerClient, "Mythril Katana");
SmithCombo($ServerClient, "Titanium Katana");

SmithCombo($ServerClient, "Ivory Saber");
SmithCombo($ServerClient, "Brass Saber");
SmithCombo($ServerClient, "Copper Saber");
SmithCombo($ServerClient, "Bronze Saber");
SmithCombo($ServerClient, "Silver Saber");
SmithCombo($ServerClient, "Gold Saber");
SmithCombo($ServerClient, "Iron Saber");
SmithCombo($ServerClient, "Steel Saber");
SmithCombo($ServerClient, "Platinum Saber");
SmithCombo($ServerClient, "Mythril Saber");
SmithCombo($ServerClient, "Titanium Saber");

SmithCombo($ServerClient, "Ivory Axe");
SmithCombo($ServerClient, "Brass Axe");
SmithCombo($ServerClient, "Copper Axe");
SmithCombo($ServerClient, "Bronze Axe");
SmithCombo($ServerClient, "Silver Axe");
SmithCombo($ServerClient, "Gold Axe");
SmithCombo($ServerClient, "Iron Axe");
SmithCombo($ServerClient, "Steel Axe");
SmithCombo($ServerClient, "Platinum Axe");
SmithCombo($ServerClient, "Mythril Axe");
SmithCombo($ServerClient, "Titanium Axe");

SmithCombo($ServerClient, "Ivory Baridache");
SmithCombo($ServerClient, "Brass Baridache");
SmithCombo($ServerClient, "Copper Baridache");
SmithCombo($ServerClient, "Bronze Baridache");
SmithCombo($ServerClient, "Silver Baridache");
SmithCombo($ServerClient, "Gold Baridache");
SmithCombo($ServerClient, "Iron Baridache");
SmithCombo($ServerClient, "Steel Baridache");
SmithCombo($ServerClient, "Platinum Baridache");
SmithCombo($ServerClient, "Mythril Baridache");
SmithCombo($ServerClient, "Titanium Baridache");

SmithCombo($ServerClient, "Ivory Tip");
SmithCombo($ServerClient, "Brass Tip");
SmithCombo($ServerClient, "Copper Tip");
SmithCombo($ServerClient, "Bronze Tip");
SmithCombo($ServerClient, "Silver Tip");
SmithCombo($ServerClient, "Gold Tip");
SmithCombo($ServerClient, "Iron Tip");
SmithCombo($ServerClient, "Steel Tip");
SmithCombo($ServerClient, "Platinum Tip");
SmithCombo($ServerClient, "Mythril Tip");
SmithCombo($ServerClient, "Titanium Tip");

SmithCombo($ServerClient, "Ivory Mallet");
SmithCombo($ServerClient, "Brass Mallet");
SmithCombo($ServerClient, "Copper Mallet");
SmithCombo($ServerClient, "Bronze Mallet");
SmithCombo($ServerClient, "Silver Mallet");
SmithCombo($ServerClient, "Gold Mallet");
SmithCombo($ServerClient, "Iron Mallet");
SmithCombo($ServerClient, "Steel Mallet");
SmithCombo($ServerClient, "Platinum Mallet");
SmithCombo($ServerClient, "Mythril Mallet");
SmithCombo($ServerClient, "Titanium Mallet");

//$Smith::Stuff[%i++] = "Sling";
//$Smith::Stuff[%i++] = "ShortBow";
//$Smith::Stuff[%i++] = "LongBow";
//$Smith::Stuff[%i++] = "CrossBow";
//$Smith::Stuff[%i++] = "HeavyCrossBow";
//$Smith::Stuff[%i++] = "CompBow";

//--------------------------------
//	RANGED WEAPONS     //svar NULL																																	Bow have NULL dmg types (arrows do the dmg)
MakeItem("Crossbow", "Crossbow", "NULL",					$RangedAccessoryType, 1, "A crossbow purchased in Shildrik.", $Headers::B, "crossbow", "Weapon",false, ",Priest,Warrior,Rogue,Wizard,",			"NULL", 20, 1.5, CrossbowShoot1, CrossbowSwitch1);
$ItemData["Crossbow", Ammo] = ",Wood_Quarrel,Ivory_Quarrel,Copper_Quarrel,";
$ItemCost["Crossbow"] = 2000;
$ItemData["Crossbow", ToUseSkill] = "STR 1 DEX 5";

MakeItem("Heavy Crossbow", "Heavy Crossbow", "NULL",					$RangedAccessoryType, 1, "A crossbow purchased in Shildrik.", $Headers::B, "crossbow", "Weapon",false, ",Priest,Warrior,Rogue,Wizard,",			"NULL", 20, 1.5, CrossbowShoot1, CrossbowSwitch1);
$ItemData["Heavy_Crossbow", Ammo] = ",Bronze_Quarrel,Silver_Quarrel,Steel_Quarrel,Iron_Quarrel,Mythril_Quarrel,";
$ItemCost["Heavy_Crossbow"] = 50000;
$ItemData["Heavy_Crossbow", ToUseSkill] = "STR 200 DEX 100";

MakeItem("Short Bow", "Short Bow", "NULL",		$RangedAccessoryType, 3, "A Bow purchased in Edmire.", $Headers::B, "longbow", "Weapon",false, ",Priest,Warrior,Rogue,Wizard,",	"NULL", 120, 2, CrossbowShoot1, CrossbowSwitch1);
$ItemData["Short_Bow", Ammo] = ",Basic_Arrow,Stone_Arrow,";
$ItemCost["Short_Bow"] = 30000;
$ItemData["Short_bow", ToUseSkill] = "STR 100 DEX 50";

MakeItem("Combat Bow", "Combat Bow", "NULL",		$RangedAccessoryType, 4, "A bow purchased in Koba.", $Headers::B, "comp_bow", "Weapon",false, ",Priest,Warrior,Rogue,",	"NULL", 360, 1.5, CrossbowShoot1, CrossbowSwitch1);
$ItemData["Combat_Bow", Ammo] = ",Basic_Arrow,Combat_Arrow,Flight_Arrow,";
$ItemCost["Combat_Bow"] = 120000;
$ItemData["Combat_bow", ToUseSkill] = "STR 150 DEX 75";
//-------------------------------------

//from blacksmith.cs (easier to edit)
$Smith::Mod::Ammo[Crossbow] = ",Wood_Quarrel,Ivory_Quarrel,Copper_Quarrel,";
$Smith::Mod::Ammo[ShortBow] = ",Basic_Arrow,Stone_Arrow,";
$Smith::Mod::Ammo[LongBow] = ",Basic_Arrow,Combat_Arrow,Flight_Arrow,";
$Smith::Mod::Ammo[HeavyCrossbow] = ",Bronze_Quarrel,Silver_Quarrel,Steel_Quarrel,Iron_Quarrel,Mythril_Quarrel,";
$Smith::Mod::Ammo[CompBow] = ",Basic_Arrow,Combat_Arrow,Flight_Arrow,";

//	PROJECTILES
MakeItem("Wood Quarrel", "Wood Quarrel", "6 5",		$ProjectileAccessoryType, "0.01", "A short arrow made of <f1>Wood<f0>.",			$Headers::Q, "bullet", "Projectile", false);
$ItemCost["Wood_Quarrel"] = 10;
MakeItem("Ivory Quarrel", "Ivory Quarrel", "6 10",		$ProjectileAccessoryType, "0.01", "A short arrow made of <f1>Ivory<f0>.",			$Headers::Q, "bullet", "Projectile", false);
$ItemCost["Ivory_Quarrel"] = 20;
MakeItem("Copper Quarrel", "Copper Quarrel", "6 30",		$ProjectileAccessoryType, "0.01", "A short arrow made of <f1>Copper<f0>.",			$Headers::Q, "bullet", "Projectile", false);
$ItemCost["Copper_Quarrel"] = 60;
MakeItem("Bronze Quarrel", "Bronze Quarrel", "6 60",		$ProjectileAccessoryType, "0.01", "A short arrow made of <f1>Bronze<f0>.",			$Headers::Q, "bullet", "Projectile", false);
$ItemCost["Bronze_Quarrel"] = 120;
MakeItem("Silver Quarrel", "Silver Quarrel", "6 100",		$ProjectileAccessoryType, "0.01", "A short arrow made of <f1>Silver<f0>.",			$Headers::Q, "bullet", "Projectile", false);
$ItemCost["Silver_Quarrel"] = 180;
MakeItem("Iron Quarrel", "Iron Quarrel", "6 150",		$ProjectileAccessoryType, "0.01", "A short arrow made of <f1>Iron<f0>.",			$Headers::Q, "bullet", "Projectile", false);
$ItemCost["Iron_Quarrel"] = 225;
MakeItem("Mythril Quarrel", "Mythril Quarrel", "6 200",		$ProjectileAccessoryType, "0.01", "A short arrow made of <f1>Mythril<f0>.",			$Headers::Q, "bullet", "Projectile", false);
$ItemCost["Mythril_Quarrel"] = 300;

MakeItem("Basic Arrow", "Basic Arrow", "6 50",		$ProjectileAccessoryType, "0.01", "A long arrow made of <f1>Wood<f0>.",			$Headers::Q, "tracer", "Projectile", false);
$ItemCost["Basic_Arrow"] = 50;
MakeItem("Stone Arrow", "Stone Arrow", "6 100",		$ProjectileAccessoryType, "0.01", "A long arrow made of <f1>Stone<f0>.",			$Headers::Q, "tracer", "Projectile", false);
$ItemCost["Stone_Arrow"] = 100;
MakeItem("Combat Arrow", "Combat Arrow", "6 300",		$ProjectileAccessoryType, "0.01", "A long arrow made of <f1>Steel<f0>.",			$Headers::Q, "tracer", "Projectile", false);
$ItemCost["Combat_Arrow"] = 300;
MakeItem("Flight Arrow", "Flight Arrow", "6 200",		$ProjectileAccessoryType, "0.01", "A long arrow made of <f1>Feathered<f0>.",			$Headers::Q, "tracer", "Projectile", false);
$ItemCost["Flight_Arrow"] = 200;

//PREMIUM WEAPONS
MakeItem("Contender", "Contender", "6 60 1 4 2 -1", $SwordAccessoryType, 40, "A strange sword...",	$Headers::B, "short_sword", "Weapon", false, ",Warrior,Rogue,",		$SlashingDamageType, 3, 1.5, SoundSwing6);
//$ItemData["Contender", ToUseSkill] = "LVL 35";
MakeItem("Blue Sword of Strength", "Blue Sword of Strength", "6 100 1 100", $SwordAccessoryType, 40, "A sword that increases strength.",	$Headers::B, "short_sword", "Weapon", false, ",Warrior,Rogue,",		$SlashingDamageType, 3, 1.5, SoundSwing6);
//$ItemData["Blue_Sword_of_Strength", ToUseSkill] = "LVL 75";//25";
MakeItem("Blue Sword of Magic", "Blue Sword of Magic", "6 100 5 100", $SwordAccessoryType, 40, "A sword that increases magic.",	$Headers::B, "short_sword", "Weapon", false, ",Warrior,Rogue,",		$SlashingDamageType, 3, 1.5, SoundSwing6);
//$ItemData["Blue_Sword_of_Magic", ToUseSkill] = "LVL 75";//25";
MakeItem("Sword of Aim", "Sword of Aim", "6 100 2 100", $SwordAccessoryType, 40, "A sword that increases accuracy.",	$Headers::B, "short_sword", "Weapon", false, ",Warrior,Rogue,",		$SlashingDamageType, 3, 1.5, SoundSwing6);
//$ItemData["Sword_of_Aim", ToUseSkill] = "LVL 75";//25";
MakeItem("Axe of Hok", "Axe of Hok", "6 300 2 10", $SwordAccessoryType, 40, "The holy axe formerly owned by Hok",	$Headers::B, "BattleAxe", "Weapon", false, ",Warrior,",		$SlashingDamageType, 3, 3, SoundSwing6);
//$ItemData["Axe_of_Hok", ToUseSkill] = "LVL 90";//30";
MakeItem("Brain Crusher", "Brain Crusher", "6 200 1 100 5 -100", $SwordAccessoryType, 40, "A sword that increases accuracy.",	$Headers::B, "sword", "Weapon", false, ",Warrior,",		$SlashingDamageType, 3, 1.5, SoundSwing6);
//$ItemData["Brain_Crusher", ToUseSkill] = "LVL 90";//30";
MakeItem("Demon", "Demon", "6 200 2 10 3 -50 4 20 5 20 6 10", $SwordAccessoryType, 40, "A powerful sword said to have been owned by a dark warlord.",	$Headers::B, "elfinblade", "Weapon", false, ",Warrior,",		$SlashingDamageType, 3, 1.5, SoundSwing6);
//$ItemData["Demon", ToUseSkill] = "LVL 120";//40";
MakeItem("Hard Hitter", "Hard Hitter", "6 300 1 50 2 50 4 -50 5 -50", $SwordAccessoryType, 40, "A powerful club.",	$Headers::B, "Mace", "Weapon", false, ",Priest,Warrior,",		$SlashingDamageType, 3, 1.5, SoundSwing6);
//$ItemData["Hard_Hitter", ToUseSkill] = "LVL 150";//50";

MakeItem("Monster Killer", "Monster Killer", "6 400 2 50",	$PolearmAccessoryType, 10, "A hard Hitting Katana that adds to your <f1>Dex<f0> stat.",	$Headers::B, "katana", "Weapon", false, ",Warrior,Rogue,",			$PiercingDamageType, 4, 2.5,  SoundSwing3);
MakeItem("Avenger", "Avenger", "6 200 3 -20",	$PolearmAccessoryType, 20, "A fast swinging katana.",	$Headers::B, "katana", "Weapon", false, ",Rogue,Wizard,",			$PiercingDamageType, 4, 1,  SoundSwing3);
//$ItemData["Avenger", ToUseSkill] = "LVL 50";
MakeItem("Power Mallet", "Power Mallet", "6 500 1 100",							$BludgeonAccessoryType, 20, "A hammer that adds to your <f1>STR<f0> stat.",				$Headers::B, "hammer", "Weapon", false, ",Warrior,",		$BludgeoningDamageType, 2, 2.5, SoundSwing6);
//$ItemData["Power_Mallet", ToUseSkill] = "LVL 60";
MakeItem("Eye Poker", "Eye Poker", "6 300 31 30",			$PolearmAccessoryType, 8, "A spear that sometimes causes <f1>Blind<f0>.",		$Headers::B, "spear", "Weapon", false, ",Rogue,",				$PiercingDamageType, 5, 3.5,  SoundSwing3);
//$ItemData["Eye_Poker", ToUseSkill] = "LVL 50";

MakeItem("Battle Foil", "Battle Foil", "6 450 1 50 2 20 3 -40 4 -40 5 -40",	$PolearmAccessoryType, 2, "A special katana.",	$Headers::B, "katana", "Weapon", false, ",Priest,Warrior,Rogue,",			$PiercingDamageType, 4, 1,  SoundSwing3);
MakeItem("Pride Killer", "Pride Killer", "6 500 31 200",	$PolearmAccessoryType, 10, "A katana that sometimes causes <f1>Blind<f0>.",	$Headers::B, "katana", "Weapon", false, ",Priest,Warrior,Rogue,",			$PiercingDamageType, 4, 1,  SoundSwing3);
MakeItem("Military Spear", "Military Spear", "6 200",			$PolearmAccessoryType, 8, "A spear used by the military.",		$Headers::B, "spear", "Weapon", false, ",Warrior,Rogue,",				$PiercingDamageType, 5, 3.5,  SoundSwing3);
MakeItem("Dragoon Lance", "Dragoon Lance", "6 500 2 -20",			$PolearmAccessoryType, 8, "A spear used by dragoons.",		$Headers::B, "spear", "Weapon", false, ",Warrior,Rogue,",				$PiercingDamageType, 5, 3.5,  SoundSwing3);
MakeItem("Judgement Hammer", "Judgement Hammer", "6 800 2 -100 3 -50",							$BludgeonAccessoryType, 20, "A slow hard hitting hammer.",				$Headers::B, "hammer", "Weapon", false, ",Priest,Warrior,",		$BludgeoningDamageType, 2, 8, SoundSwing6);
MakeItem("Military Hammer", "Military Hammer", "6 300",							$BludgeonAccessoryType, 20, "A hammer used by the military.",				$Headers::B, "hammer", "Weapon", false, ",Priest,Warrior,",		$BludgeoningDamageType, 2, 2.5, SoundSwing6);
MakeItem("Trogla", "Trogla", "6 600",							$BludgeonAccessoryType, 20, "A hammer made by friendly Uuags.",				$Headers::B, "hammer", "Weapon", false, ",Warrior,",		$BludgeoningDamageType, 2, 2.5, SoundSwing6);

//MOON ROCK ITEMS
MakeItem("Moon Rock Axe", "Moon Rock Axe", "6 1000", $SwordAccessoryType, 40, "A rare powerful Axe",	$Headers::B, "BattleAxe", "Weapon", false, ",Warrior,",		$SlashingDamageType, 3, 3, SoundSwing6);
$ItemData["Moon_Rock_Axe", ToUseSkill] = "LVL 200";

MakeItem("Moon Rock Sword", "Moon Rock Sword", "6 1000", $SwordAccessoryType, 40, "A rare powerful Sword",	$Headers::B, "elfinblade", "Weapon", false, ",Warrior,",		$SlashingDamageType, 3, 3, SoundSwing6);
$ItemData["Moon_Rock_Sword", ToUseSkill] = "LVL 200";

MakeItem("Moon Rock Katana", "Moon Rock Katana", "6 800",	$SwordAccessoryType, 2, "A rare light-weight Katana",	$Headers::B, "katana", "Weapon", false, ",Rogue,",	$PiercingDamageType, 1, 2.5, SoundSwing3);
$ItemData["Moon_Rock_Katana", ToUseSkill] = "LVL 200";

MakeItem("Moon Rock Hammer", "Moon Rock Hammer", "6 800",	$BludgeonAccessoryType, 43, "A rare Hammer",	$Headers::B, "hammer", "Weapon", false, ",Priest,Warrior,",	$BludgeoningDamageType, 2, 2.5, SoundSwing7);
$ItemData["Moon_Rock_Hammer", ToUseSkill] = "LVL 200";

MakeItem("Hard Moon Rock Armor", "Hard Moon Rock Armor",	"7 1000",		$BodyAccessoryType, 128, "A rare Armor, only a Master Warrior can wear one with-out it falling to dust..",	$Headers::C, "NULL", "Accessory", "rpghuman7",	",Warrior,");
$ItemData["Moon_Rock_Armor", ToUseSkill] = "LVL 200";

MakeItem("Soft Moon Rock Robe", "Soft Moon Rock Robe", "7 200 15 1000 13 200",	$BodyAccessoryType, 0, "A rare Robe, only a Master Wizard can wear one with-out it falling to dust..",	$Headers::C, "Robed", "Accessory", "robered",		",Wizard,");
$ItemData["Soft_Moon_Rock_Robe", ToUseSkill] = "Master Wizard";

MakeItem("Moon Rock Shield", "Moon Rock Shield", "7 1000 2 -500 13 400",	$ShieldAccessoryType, 128, "A rare Shield",	$Headers::D, "shield3", "Accessory", true, ",Priest,Warrior,");
$ItemData["Moon_Rock_Shield", ToUseSkill] = "LVL 200";

MakeItem("Moon Rock Helmet", "Moon Rock Helmet", "7 100",		$HeadAccessoryType, 21, "A rare Helmet",				$Headers::E, "NULL", "Accessory", true,		",Priest,Warrior,");
$ItemData["Moon_Rock_Helmets", ToUseSkill] = "LVL 200";

MakeItem("Moon Rock Pendant", "Moon Rock Pendant", "1 200 2 200 3 200 4 200 5 200",		$TalismanAccessoryType, 1, "A rare Pendant",	$Headers::J, "NULL", "Accessory", true,	",Priest,Warrior,Wizard,");
$ItemData["Moon_Rock_Pendant", ToUseSkill] = "LVL 200 ";

MakeItem("Moon Rock Boots", "Moon Rock Boots", "2 500 8 1 13 50",	$BootsAccessoryType, 34, "A pair of rare Boots",			$Headers::G, "NULL", "Accessory", true,	",Warrior,");
$ItemData["Moon_Rock_Boots", ToUseSkill] = "LVL 200";

MakeItem("Moon Rock Gauntlets", "Moon Rock Gauntlets", "6 50 7 100 10 25",	$HandsAccessoryType, 34, "A pair of rare Gauntlets.",	$Headers::I, "NULL", "Accessory", true,	",Priest,Rogue,Warrior,");
$ItemData["Moon_Rock_Gauntlets", ToUseSkill] = "LVL 200";

MakeItem("Moon Rock Ring", "Moon Rock Ring", "20 Poison 20 Mute 20 Blind 20 Petrify",	$RingAccessoryType, $RingWeight, "A rare Ring.",		$Headers::H, "NULL", "Accessory", true, ",Priest,Wizard,");
$ItemData["Moon_Rock_Ring", ToUseSkill] = "LVL 200";

MakeItem("Moon Rock Leggings", "Moon Rock Leggings", "7 100 13 50",		$LegsAccessoryType, 1, "A pair of rare leggings",		$Headers::F, "NULL", "Accessory", true,	",Priest,Rogue,Warrior,");
$ItemData["Moon_Rock_Leggings", ToUseSkill] = "LVL 200";

MakeItem("Moon Rock Bow", "Moon Rock Bow",	"NULL",	$RangedAccessoryType, 19, "A rare Bow",	$Headers::B, "comp_bow", "Weapon", false, ",Warrior,", "NULL", 320, 1, CrossbowShoot1, CrossbowSwitch1);
$ItemData["Moon_Rock_Bow", ToUseSkill] = "LVL 200 CLASS ,Ranger,";

MakeItem("Moon Rock Arrow", "Moon Rock Arrow", "6 1000",	$ProjectileAccessoryType, 1, "A rare Moon Rock will make 99 of this deadly arrows.",	$Headers::Q, "bullet", "Projectile", false);

//RM ORIGINALS

MakeItem("Poultry Killer", "Poultry Killer", "6 5",	$SwordAccessoryType, 1, "A knife purchased in Shildrik.",	$Headers::B, "Dagger", "Weapon", false, ",Priest,Warrior,Rogue,",		$PiercingDamageType, 4, 1.5,  SoundSwing2);
MakeItem("Rogue Dagger", "Rogue Dagger", "6 10",	$SwordAccessoryType, 1, "A dagger purchased in Shildrik.",	$Headers::B, "short_sword", "Weapon", false, ",Priest,Warrior,Rogue,",		$PiercingDamageType, 4, 1.5,  SoundSwing2);
MakeItem("Dirk", "Dirk", "6 20",	$SwordAccessoryType, 1, "A dagger purchased in Shildrik.",	$Headers::B, "short_sword", "Weapon", false, ",Priest,Warrior,Rogue,",		$PiercingDamageType, 4, 1.5,  SoundSwing2);
MakeItem("Thief Knife", "Thief Knife", "6 40",	$SwordAccessoryType, 2, "A dagger purchased in Edmire.",	$Headers::B, "short_sword", "Weapon", false, ",Priest,Warrior,Rogue,",		$PiercingDamageType, 4, 1.5,  SoundSwing2);
MakeItem("Assassin", "Assassin", "6 80",	$SwordAccessoryType, 2, "A dagger purchased in Edmire.",	$Headers::B, "short_sword", "Weapon", false, ",Priest,Warrior,Rogue,",		$PiercingDamageType, 4, 1.5,  SoundSwing2);
MakeItem("Spar Foil", "Spar Foil", "6 100",	$PolearmAccessoryType, 2, "A begginers katana purchased in Edmire.",	$Headers::B, "katana", "Weapon", false, ",Priest,Warrior,Rogue,",			$PiercingDamageType, 4, 1,  SoundSwing3);

//RM ITEM LIST - PIERCING - KATANAS - ROGUE - WARRIOR

MakeItem("Pike", "Pike", "6 10",			$PolearmAccessoryType, 8, "A spear purchased in Shildrik.",		$Headers::B, "spear", "Weapon", false, ",Warrior,Rogue,",				$PiercingDamageType, 5, 3.5,  SoundSwing3);
MakeItem("Javelin", "Javelin", "6 20",			$PolearmAccessoryType, 8, "A spear purchased in Shildrik.",		$Headers::B, "spear", "Weapon", false, ",Warrior,Rogue,",				$PiercingDamageType, 5, 3.5,  SoundSwing3);
MakeItem("Lance", "Lance", "6 40",			$PolearmAccessoryType, 8, "A spear purchased in Shildrik.",		$Headers::B, "spear", "Weapon", false, ",Warrior,Rogue,",				$PiercingDamageType, 5, 3.5,  SoundSwing3);
MakeItem("Hunting Spear", "Hunting Spear", "6 80",			$PolearmAccessoryType, 8, "A spear purchased in Shildrik.",		$Headers::B, "spear", "Weapon", false, ",Warrior,Rogue,",				$PiercingDamageType, 5, 3.5,  SoundSwing3);
MakeItem("Partisian", "Partisian", "6 150",			$PolearmAccessoryType, 8, "A spear purchased in Edmire.",		$Headers::B, "spear", "Weapon", false, ",Priest,Warrior,Rogue,",				$PiercingDamageType, 5, 3.5,  SoundSwing3);

//RM ITEM LIST - BLUDGEONING - CLUBS N' HAMMERS - WARRIOR PRIESTS

MakeItem("Club", "Club", "6 5",							$BludgeonAccessoryType, 2, "A club purchased in Shildrik.",				$Headers::B, "Mace", "Weapon", false, ",Priest,Warrior,Rogue,Wizard,",		$BludgeoningDamageType, 2, 2.5, SoundSwing6);
MakeItem("Seed Crusher", "Seed Crusher", "6 10",							$BludgeonAccessoryType, 2, "A club purchased in Shildrik.",				$Headers::B, "Mace", "Weapon", false, ",Priest,Warrior,",		$BludgeoningDamageType, 2, 2.5, SoundSwing6);
MakeItem("Earth Pounder", "Earth Pounder", "6 20",							$BludgeonAccessoryType, 2, "A club purchased in Edmire.",				$Headers::B, "Mace", "Weapon", false, ",Priest,Warrior,",		$BludgeoningDamageType, 2, 2.5, SoundSwing6);
MakeItem("Bam Club", "Bam Club", "6 40",							$BludgeonAccessoryType, 4, "A club purchased in Edmire.",				$Headers::B, "Mace", "Weapon", false, ",Priest,Warrior,",		$BludgeoningDamageType, 2, 2.5, SoundSwing6);
MakeItem("Goblin Club", "Goblin Club", "6 60",							$BludgeonAccessoryType, 4, "A club purchased in Gooba.",				$Headers::B, "Mace", "Weapon", false, ",Priest,Warrior,Rogue,Wizard,",		$BludgeoningDamageType, 2, 2.5, SoundSwing6);
MakeItem("Potato Masher", "Potato Masher", "6 80",							$BludgeonAccessoryType, 10, "A club purchased in Gooba.",				$Headers::B, "Mace", "Weapon", false, ",Priest,Warrior,Rogue,Wizard,",		$BludgeoningDamageType, 2, 2.5, SoundSwing6);
MakeItem("Smith Hammer", "Smith Hammer", "6 80",							$BludgeonAccessoryType, 10, "A hammer purchased in Koba",				$Headers::B, "hammer", "Weapon", false, ",Priest,Warrior,",		$BludgeoningDamageType, 2, 2.5, SoundSwing6);

//	MAGISTRAL STAFFS
MakeItem("Wood Rod", "Wood Rod", "6 2",							$BludgeonAccessoryType, 10, "A casters rod.",				$Headers::B, "quarterstaff", "Weapon", false, ",Priest,Wizard,",		$BludgeoningDamageType, 3, 2, SoundSwing6);
$ItemData["Wood_Rod", ToUseSkill] = "LVL 1";
MakeItem("Apprentice Rod", "Apprentice Rod", "6 10 5 1",							$BludgeonAccessoryType, 10, "A casters rod.",				$Headers::B, "quarterstaff", "Weapon", false, ",Priest,Wizard,",		$BludgeoningDamageType, 3, 2, SoundSwing6);
$ItemData["Apprentice_Rod", ToUseSkill] = "LVL 5";
MakeItem("Acolyte Rod", "Acolyte Rod", "6 20 3 10 4 10",							$BludgeonAccessoryType, 10, "A casters rod.",				$Headers::B, "quarterstaff", "Weapon", false, ",Priest,",		$BludgeoningDamageType, 3, 2, SoundSwing6);
$ItemData["Acolyte_Rod", ToUseSkill] = "LVL 15";
MakeItem("Necro Rod", "Necro Rod", "6 20 4 10 5 10",							$BludgeonAccessoryType, 10, "A casters rod.",				$Headers::B, "quarterstaff", "Weapon", false, ",Wizard,",		$BludgeoningDamageType, 3, 2, SoundSwing6);
$ItemData["Necro_Rod", ToUseSkill] = "LVL 15";
MakeItem("Guard Rod", "Guard Rod", "6 50 2 5 3 5 5 15",							$BludgeonAccessoryType, 10, "A casters rod.",				$Headers::B, "quarterstaff", "Weapon", false, ",Priest,Wizard,",		$BludgeoningDamageType, 3, 2, SoundSwing6);
$ItemData["Guard_Rod", ToUseSkill] = "LVL 30";
MakeItem("Prism Rod", "Prism Rod", "6 60 4 50 5 50",							$BludgeonAccessoryType, 10, "A casters rod.",				$Headers::B, "quarterstaff", "Weapon", false, ",Priest,Wizard,",		$BludgeoningDamageType, 3, 2, SoundSwing6);
$ItemData["Prism_Rod", ToUseSkill] = "LVL 50";
MakeItem("Success Rod", "Success Rod", "6 65 4 100",							$BludgeonAccessoryType, 10, "A casters rod.",				$Headers::B, "quarterstaff", "Weapon", false, ",Priest,Wizard,",		$BludgeoningDamageType, 3, 2, SoundSwing6);
$ItemData["Success_Rod", ToUseSkill] = "LVL 80";
MakeItem("Mana Rod", "Mana Rod", "6 70 14 300",							$BludgeonAccessoryType, 10, "A casters rod.",				$Headers::B, "quarterstaff", "Weapon", false, ",Priest,Wizard,",		$BludgeoningDamageType, 3, 2, SoundSwing6);
$ItemData["Mana_Rod", ToUseSkill] = "LVL 100";

MakeItem("Fire Staff", "Fire Staff", "6 50",				$SwordAccessoryType, 1, "A staff with fire affinity.", $Headers::B, "longstaff", "Weapon", false, ",Wizard,",	$SlashingDamageType, 50, 4);
$ItemCost["Fire_Staff"] = 100000;
MakeItem("Ice Staff", "Ice Staff", "6 100",				$SwordAccessoryType, 1, "A staff with ice affinity.", $Headers::B, "longstaff", "Weapon", false, ",Wizard,",	$SlashingDamageType, 50, 4);
$ItemCost["Ice_Staff"] = 200000;
MakeItem("Earth Staff", "Earth Staff", "6 150",				$SwordAccessoryType, 1, "A staff with earth affinity.", $Headers::B, "longstaff", "Weapon", false, ",Wizard,",	$SlashingDamageType, 50, 4);
$ItemCost["Earth_Staff"] = 250000;
MakeItem("Aqua Staff", "Aqua Staff", "6 200",				$SwordAccessoryType, 1, "A staff with water affinity.", $Headers::B, "longstaff", "Weapon", false, ",Wizard,",    $SlashingDamageType, 50, 4);
$ItemCost["Aqua_Staff"] = 300000;
MakeItem("Storm Staff", "Storm Staff", "6 250",				$SwordAccessoryType, 1, "A staff with storm affinity.", $Headers::B, "longstaff", "Weapon", false, ",Wizard,",  $SlashingDamageType, 50, 4);
$ItemCost["Storm_Staff"] = 350000;
MakeItem("Power Staff", "Power Staff", "6 500",				$SwordAccessoryType, 1, "A powerful staff.", $Headers::B, "longstaff", "Weapon", false,  ",Wizard,",	$SlashingDamageType, 50, 4);
$ItemCost["Power_Staff"] = 700000;

function longstaff4Image::onFire(%player, %slot) {
	%cl = GameBase::getOwnerClient(%player); $los::object = "";
	if($ClientData[%cl, UsingWeapon] == "Fire_Staff") {
		GameBase::getLOSinfo(%player, $ItemData[$ClientData[%cl, UsingWeapon], Range]);
		FlameBolt.Weapon_SpellAttack(%cl, %player, $los::object);
	}
	if($ClientData[%cl, UsingWeapon] == "Ice_Staff") {
		GameBase::getLOSinfo(%player, $ItemData[$ClientData[%cl, UsingWeapon], Range]);
		IceBallBolt.Weapon_SpellAttack(%cl, %player, $los::object);
	}
	if($ClientData[%cl, UsingWeapon] == "Earth_Staff") {
		GameBase::getLOSinfo(%player, $ItemData[$ClientData[%cl, UsingWeapon], Range]);
		dodgethisbolt.Weapon_SpellAttack(%cl, %player, $los::object);
	}
	if($ClientData[%cl, UsingWeapon] == "Aqua_Staff") {
		GameBase::getLOSinfo(%player, $ItemData[$ClientData[%cl, UsingWeapon], Range]);
		waterfinal.Weapon_SpellAttack(%cl, %player, $los::object);
	}
	if($ClientData[%cl, UsingWeapon] == "Storm_Staff") {
		GameBase::getLOSinfo(%player, $ItemData[$ClientData[%cl, UsingWeapon], Range]);
		electricitybolt.Weapon_SpellAttack(%cl, %player, $los::object);
	}
	if($ClientData[%cl, UsingWeapon] == "Power_Staff") {
		GameBase::getLOSinfo(%player, $ItemData[$ClientData[%cl, UsingWeapon], Range]);
		Gravityshot.Weapon_SpellAttack(%cl, %player, $los::object);
	}
	if($ClientData[%cl, UsingWeapon] == "Drag_Sword") {	//Drag Sword is longstaff/delay4 -> shares this model
		GameBase::getLOSinfo(%player, $ItemData[$ClientData[%cl, UsingWeapon], Range]);
		voodooshot.Weapon_SpellAttack(%cl, %player, $los::object);
	}
	MeleeAttack(%player, 2, $ClientData[%cl, UsingWeapon]);
}

MakeItem("Drag Sword", "Drag Sword", "6 100 1 50 2 50 3 50 4 50 5 50 6 50 20 Poison 20 Petrify 20 Blind 20 Mute 20",				$SwordAccessoryType, 1, "A unique sword forged from Leviathan teeth.", $Headers::B, "longstaff", "Weapon", false, ",Wizard,",	$SlashingDamageType, 50, 4);
$ItemCost["Drag_Sword"] = 100000;
$ItemData["Drag_Sword", ToUseSkill] = "LVL 50";
// Drag Sword's voodooshot branch moved into longstaff4Image::onFire above (its real model,
// longstaff/delay4). The old greensword040Image handler was a dead model (no weapon uses it).

MakeItem("Punisher", "Punisher", "6 0",				$SwordAccessoryType, 1, "The very powerful staff owned by <f1>Chee2<f0>.", $Headers::B, "greensword", "Weapon", false,  ",Priest,Warrior,Rogue,Wizard,",	$SlashingDamageType, 5, 1.25);
function greensword125Image::onFire(%player, %slot) {	//Punisher is greensword/delay1.25 -> this model
	%cl = GameBase::getOwnerClient(%player); $los::object = "";
	if($ClientData[%cl, UsingWeapon] == "Punisher") {
		GameBase::getLOSinfo(%player, $ItemData[$ClientData[%cl, UsingWeapon], Range]);
		Gravityshot.Weapon_SpellAttack(%cl, %player, $los::object);
	}
	MeleeAttack(%player, 2, $ClientData[%cl, UsingWeapon]);
}

//============================================================================================
//======================================= EQUIP DATA =========================================
//============================================================================================

MakeItem("Old Rags", "Old Rags", "7 1",							$BodyAccessoryType, 4, "Some old torn up rags.",						$Headers::C, "NULL", "Accessory", "rpghide",				",Priest,Warrior,Rogue,Wizard,");
SmithCombo($ServerClient, "Leather_Fabric Suit");
SmithCombo($ServerClient, "Brass Armor");
SmithCombo($ServerClient, "Copper ChainMail");
SmithCombo($ServerClient, "Bronze Mail");
SmithCombo($ServerClient, "Silver Armor");
SmithCombo($ServerClient, "Gold Armor");
SmithCombo($ServerClient, "Iron Armor");
SmithCombo($ServerClient, "Steel Mail");
SmithCombo($ServerClient, "Platinum PlateMail");
SmithCombo($ServerClient, "Mythril Mail");
SmithCombo($ServerClient, "Titanium PlateMail");
SmithCombo($ServerClient, "Valorite PlateMail");

MakeItem("Raider Suit", "Raider Suit", "7 5 13 40",			$BodyAccessoryType, 4, "Body Armor.",			$Headers::C, "NULL", "Accessory", "RMInitialEquipment",				",Priest,Warrior,Rogue,Wizard,");
MakeItem("Klepto Outfit", "Klepto Outfit", "7 10 2 20 13 50",		$BodyAccessoryType, 3, "Body Armor.",				$Headers::C, "NULL", "Accessory", "RMNightWarrior",				",Rogue,");
MakeItem("Jerkilin", "Jerkilin", "7 20 3 10 2 -12 13 80",	$BodyAccessoryType, 32, "Body Armor.",		$Headers::C, "NULL", "Accessory", "RMJerkilin",	",Priest,Warrior,Rogue,");
MakeItem("Rogue Gear", "Rogue Gear", "7 40 2 50 13 100",							$BodyAccessoryType, 3, "Body Armor.",				$Headers::C, "NULL", "Accessory", "rpghuman1",				",Rogue,");
MakeItem("Plated Jerkilin", "Plated Jerkilin", "7 40 3 20 2 -50 13 150",		$BodyAccessoryType, 70, "Body Armor.",	$Headers::C, "NULL", "Accessory", "RMJerkilinPlate",				",Priest,Warrior,Rogue,");
MakeItem("Ninja Gear", "Ninja Gear", "7 50 3 15 2 -40 13 200",		$BodyAccessoryType, 60, "Body Armor.",	$Headers::C, "NULL", "Accessory", "RMNinjaGear",				",Priest,Warrior,Rogue,");
MakeItem("Military Uniform", "Military Uniform", "7 25 2 -5 13 50",					$BodyAccessoryType, 25, "Body Armor.",		$Headers::C, "NULL", "Accessory", "RMShinraU",				",Priest,Warrior,");
MakeItem("Dark Plate", "Dark Plate", "7 60 3 20 2 -50 13 200",	$BodyAccessoryType, 70, "Body Armor.",	$Headers::C, "NULL", "Accessory", "RMDarkPlate",	",Priest,Warrior,");
MakeItem("Full Plate", "Full Plate", "7 80 3 25 2 -50 13 260",		$BodyAccessoryType, 70, "Body Armor",	$Headers::C, "NULL", "Accessory", "rpgplatemail",	",Priest,Warrior,");

MakeItem("Thud Gear", "Thud Gear", "7 250 15 10 3 25 13 300",				$BodyAccessoryType, 67, "Gear made from enchanted earth and metals.",	$Headers::C, "NULL", "Accessory", "RMGaiaGear",	",Warrior,");
MakeItem("Brr Gear", "Brr Gear", "7 250 15 10 3 25 13 300",				$BodyAccessoryType, 67, "Gear made from enchanted earth and metals.",	$Headers::C, "NULL", "Accessory", "RMColdScale",	",Warrior,");
MakeItem("Zap Gear", "Zap Gear", "7 250 15 10 3 25 13 300",				$BodyAccessoryType, 67, "Gear made from enchanted earth and metals.",	$Headers::C, "NULL", "Accessory", "RMLightning",	",Warrior,");
MakeItem("Burn Gear", "Burn Gear", "7 250 15 10 3 25 13 300",				$BodyAccessoryType, 67, "Gear made from enchanted earth and metals.",	$Headers::C, "NULL", "Accessory", "RMFireMail",	",Warrior,");

//	ROBES
SmithCombo($ServerClient, "Pink_Fabric Robe");
SmithCombo($ServerClient, "Purple_Fabric Robe");
SmithCombo($ServerClient, "Red_Fabric Robe");
SmithCombo($ServerClient, "Black_Fabric Robe");
SmithCombo($ServerClient, "Blue_Fabric Robe");
SmithCombo($ServerClient, "Green_Fabric Robe");
SmithCombo($ServerClient, "Dark_Fabric Robe");
SmithCombo($ServerClient, "Light_Fabric Robe");
SmithCombo($ServerClient, "Elven_Fabric Robe");

MakeItem("Robe of Darkness", "Robe of Darkness", "15 200 4 40 5 50 7 100 10 10 11 80 13 90 14 100",	$BodyAccessoryType, 2, "A Robe of Darkness",	$Headers::C, "Robed", "Accessory", "robeblack",		",Wizard,");
MakeItem("Robe of Mysticism", "Robe of Mysticism", "3 10 4 5 5 5 7 50 10 10 11 20 13 500 14 100",				$BodyAccessoryType, 2, "A Robe of Mysticism",			$Headers::C, "Robed", "Accessory", "robepink",		",Wizard,");

//	SHIELDS

SmithCombo($ServerClient, "Brass Buckler");
SmithCombo($ServerClient, "Copper Buckler");
SmithCombo($ServerClient, "Bronze Shield");
SmithCombo($ServerClient, "Silver Shield");
SmithCombo($ServerClient, "Gold Shield");
SmithCombo($ServerClient, "Iron HeavyShield");
SmithCombo($ServerClient, "Steel HeavyShield");
SmithCombo($ServerClient, "Platinum HeavyShield");
SmithCombo($ServerClient, "Mythril HeavyShield");
SmithCombo($ServerClient, "Titanium HeavyShield");

MakeItem("Aegis Shield", "Aegis Shield", "7 300 4 30 2 -50 13 250",	$ShieldAccessoryType, 60, "A shield.",	$Headers::D, "shield3", "Accessory", true,	",Priest,Warrior,");

//	HELMETS/HEAD GEAR
MakeItem("Bandana", "Bandana", "7 1 13 10",							$HeadAccessoryType, 1, "A bandana made od <f1>Leather<f0>.",				$Headers::E, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");

SmithCombo($ServerClient, "Brass Helmet");
SmithCombo($ServerClient, "Copper Helmet");
SmithCombo($ServerClient, "Bronze Helmet");
SmithCombo($ServerClient, "Silver Helmet");
SmithCombo($ServerClient, "Gold Helmet");
SmithCombo($ServerClient, "Iron Helmet");
SmithCombo($ServerClient, "Steel Helmet");
SmithCombo($ServerClient, "Platinum Helmet");
SmithCombo($ServerClient, "Mythril Helmet");
SmithCombo($ServerClient, "Titanium Helmet");

MakeItem("Antitoxinal Helmet", "Antitoxinal Helmet", "7 10 20 Poison",				$HeadAccessoryType, 4, "A helmet that protects against <f1>Poison<f0>.",			$Headers::E, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
MakeItem("Soft Helmet", "Soft Helmet", "7 10 20 Petrify",				$HeadAccessoryType, 4, "A helmet that protects against <f1>Petrification<f0>.",			$Headers::E, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
MakeItem("Loudmouth Helmet", "Loudmouth Helmet", "7 10 20 Mute",				$HeadAccessoryType, 4, "A helmet that protects against <f1>Mute<f0>.",			$Headers::E, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
MakeItem("Bright Helmet", "Bright Helmet", "7 10 20 Blind",				$HeadAccessoryType, 4, "A helmet that protects against <f1>Blind<f0>.",			$Headers::E, "NULL", "Accessory", true,		",Preist,Warrior,Rogue,Wizard,");

SmithCombo($ServerClient, "Leather_Fabric Hat");

SmithCombo($ServerClient, "Pink_Fabric Hat");
SmithCombo($ServerClient, "Purple_Fabric Hat");
SmithCombo($ServerClient, "Red_Fabric Hat");
SmithCombo($ServerClient, "Black_Fabric Hat");
SmithCombo($ServerClient, "Blue_Fabric Hat");
SmithCombo($ServerClient, "Green_Fabric Hat");
SmithCombo($ServerClient, "Dark_Fabric Hat");
SmithCombo($ServerClient, "Light_Fabric Hat");
SmithCombo($ServerClient, "Elven_Fabric Hat");

//	LEGS
SmithCombo($ServerClient, "Leather_Fabric Pants");

SmithCombo($ServerClient, "Brass Leggings");
SmithCombo($ServerClient, "Copper Leggings");
SmithCombo($ServerClient, "Bronze Leggings");
SmithCombo($ServerClient, "Silver Leggings");
SmithCombo($ServerClient, "Gold Leggings");
SmithCombo($ServerClient, "Iron Leggings");
SmithCombo($ServerClient, "Steel Leggings");
//SmithCombo($ServerClient, "Platinum Leggings");
//SmithCombo($ServerClient, "Mythril Leggings");
//SmithCombo($ServerClient, "Titanium Leggings");

MakeItem("Klepto Leggings", "Klepto Leggings", "7 10 2 25 13 15",		$LegsAccessoryType, 2, "A pair of leggings made with <f1>Light Leather<f0>.",		$Headers::F, "NULL", "Accessory", true, ",Rogue,");

//	BOOTS/FOOR WEAR
SmithCombo($ServerClient, "Hard_Leather_Fabric Boots");

SmithCombo($ServerClient, "Brass Boots");
SmithCombo($ServerClient, "Copper Boots");
SmithCombo($ServerClient, "Bronze Boots");
SmithCombo($ServerClient, "Silver Boots");
SmithCombo($ServerClient, "Gold Boots");
SmithCombo($ServerClient, "Iron Boots");
SmithCombo($ServerClient, "Steel Boots");
//SmithCombo($ServerClient, "Platinum Boots");
//SmithCombo($ServerClient, "Mythril Boots");
//SmithCombo($ServerClient, "Titanium Boots");

//	HANDS
MakeItem("Wizard Bracelet", "Wizard Bracelet", "11 20 15 5",	$HandsAccessoryType, 1, "A bracelet of intelligence, used by wizards.",	$Headers::I, "NULL", "Accessory", true,		",Wizard,");
MakeItem("Power Glove", "Power Glove", "1 50 2 10",				$HandsAccessoryType, 1, "A Glove of Strength, used by fighters.",		$Headers::I, "NULL", "Accessory", true,			",Warrior,Rogue,");
MakeItem("Monks Code", "Monks Code", "3 10 10 50",			$HandsAccessoryType, 1, "A robe piece of healing, used by Priests.",	$Headers::I, "NULL", "Accessory", true,			",Priests,");

SmithCombo($ServerClient, "Leather_Fabric Gloves");

SmithCombo($ServerClient, "Brass Gauntlets");
SmithCombo($ServerClient, "Copper Gauntlets");
SmithCombo($ServerClient, "Bronze Gauntlets");
SmithCombo($ServerClient, "Silver Gauntlets");
SmithCombo($ServerClient, "Gold Gauntlets");
SmithCombo($ServerClient, "Iron Gauntlets");
SmithCombo($ServerClient, "Steel Gauntlets");

MakeItem("Gauntlets of Invulnerability", "Gauntlets of Invulnerability", "6 20 7 70 9 10 10 10 11 10",	$HandsAccessoryType, 25, "Gauntlets made with <f1>????<f0>.",		$Headers::I, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");

//	RINGS
%i = 1;
MakeItem("Ring of Str+"@%i, "Ring of Str+"@%i, "1 "@%i,		$RingAccessoryType, $RingWeight, "A Ring of Str+"@%i,	$Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
MakeItem("Ring of Dex+"@%i, "Ring of Dex+"@%i, "2 "@%i,		$RingAccessoryType, $RingWeight, "A Ring of Dex+"@%i,	$Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
MakeItem("Ring of Con+"@%i, "Ring of Con+"@%i, "3 "@%i,		$RingAccessoryType, $RingWeight, "A Ring of Con+"@%i,	$Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
MakeItem("Ring of Int+"@%i, "Ring of Int+"@%i, "4 "@%i,		$RingAccessoryType, $RingWeight, "A Ring of Int+"@%i,	$Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
MakeItem("Ring of Wis+"@%i, "Ring of Wis+"@%i, "5 "@%i,		$RingAccessoryType, $RingWeight, "A Ring of Wis+"@%i,	$Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
MakeItem("Ring of Atk+"@%i, "Ring of Atk+"@%i, "6 "@%i,		$RingAccessoryType, $RingWeight, "A Ring of Atk+"@%i,	$Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
MakeItem("Ring of Def+"@%i, "Ring of Def+"@%i, "7 "@%i,		$RingAccessoryType, $RingWeight, "A Ring of Def+"@%i,	$Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
MakeItem("Ring of Sta+"@%i, "Ring of Sta+"@%i, "12 "@%i,		$RingAccessoryType, $RingWeight, "A Ring of Sta+"@%i,	$Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
MakeItem("Ring of MDef+"@%i, "Ring of MDef+"@%i, "15 "@%i,	$RingAccessoryType, $RingWeight, "A Ring of MDef+"@%i,$Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
for(%i = 5; %i <= 100; %i+=20) {
	MakeItem("Ring of Str+"@%i, "Ring of Str+"@%i, "1 "@%i,		$RingAccessoryType, $RingWeight, "A Ring of Str+"@%i,	$Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
	MakeItem("Ring of Dex+"@%i, "Ring of Dex+"@%i, "2 "@%i,		$RingAccessoryType, $RingWeight, "A Ring of Dex+"@%i,	$Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
	MakeItem("Ring of Con+"@%i, "Ring of Con+"@%i, "3 "@%i,		$RingAccessoryType, $RingWeight, "A Ring of Con+"@%i,	$Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
	MakeItem("Ring of Int+"@%i, "Ring of Int+"@%i, "4 "@%i,		$RingAccessoryType, $RingWeight, "A Ring of Int+"@%i,	$Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
	MakeItem("Ring of Wis+"@%i, "Ring of Wis+"@%i, "5 "@%i,		$RingAccessoryType, $RingWeight, "A Ring of Wis+"@%i, $Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
	MakeItem("Ring of Atk+"@%i, "Ring of Atk+"@%i, "6 "@%i,		$RingAccessoryType, $RingWeight, "A Ring of Atk+"@%i,	$Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
	MakeItem("Ring of Def+"@%i, "Ring of Def+"@%i, "7 "@%i,		$RingAccessoryType, $RingWeight, "A Ring of Def+"@%i,	$Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
	MakeItem("Ring of Sta+"@%i, "Ring of Sta+"@%i, "12 "@%i,		$RingAccessoryType, $RingWeight, "A Ring of Sta+"@%i,	$Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
	MakeItem("Ring of MDef+"@%i, "Ring of MDef+"@%i, "15 "@%i,	$RingAccessoryType, $RingWeight, "A Ring of MDef+"@%i,$Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
	%ii = pow(%i, 2);
	MakeItem("Ring of HP+"@%ii, "Ring of HP+"@%ii, "13 "@%ii,		$RingAccessoryType, $RingWeight, "A Ring of HP+"@%ii, $Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
	%ii = %i * 3;
	MakeItem("Ring of MP+"@%ii, "Ring of MP+"@%ii, "14 "@%ii,		$RingAccessoryType, $RingWeight, "A Ring of MP+"@%ii, $Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
}
%i="";

MakeItem("Lucky Ring", "Lucky Ring", "NULL",					$RingAccessoryType, $RingWeight, "A Lucky Ring that increases your chances for a critical hit.",	$Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
MakeItem("Ring of Power", "Ring of Power", "6 200",			$RingAccessoryType, $RingWeight, "A Ring of Power. Atk+200",		$Headers::H, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,");
MakeItem("Ring of Wisdom", "Ring of Wisdom", "5 200",		$RingAccessoryType, $RingWeight, "A Ring of Wisdom. Wis+200",	$Headers::H, "NULL", "Accessory", true,		",Wizard,");
MakeItem("Ring of Defense", "Ring of Defense", "7 200",		$RingAccessoryType, $RingWeight, "A Ring of Defense. Def+200",	$Headers::H, "NULL", "Accessory", true,		",Priest,Warrior,");
MakeItem("Ring of Constitution", "Ring of Constitution", "3 200",		$RingAccessoryType, $RingWeight, "A Ring of Con. Con+200",	$Headers::H, "NULL", "Accessory", true,			",Priest,");
MakeItem("Ring of Dexterity", "Ring of Dexterity", "2 200",				$RingAccessoryType, $RingWeight, "A Ring of Dex. Dex+200",	$Headers::H, "NULL", "Accessory", true,			",Rogue,");
MakeItem("Ring of Strength", "Ring of Strength", "1 200",	$RingAccessoryType, $RingWeight, "A Ring of Strenght+200",	$Headers::H, "NULL", "Accessory", true,			",Warrior,");

MakeItem("Ring of Exp Boost+5", "Ring of Exp Boost+5",	"EXPB 5",		$RingAccessoryType, $RingWeight, "A Ring of Exp boost+5. %5 Boost.",		$Headers::H, "NULL", "Accessory", true,	",Priest,Rogue,Warrior,Wizard,");
MakeItem("Ring of Exp Boost+10", "Ring of Exp Boost+10",	"EXPB 10",	$RingAccessoryType, $RingWeight, "A Ring of Exp boost+10. %10 Boost.",	$Headers::H, "NULL", "Accessory", true,	",Priest,Rogue,Warrior,Wizard,");
MakeItem("Ring of Exp Boost+15", "Ring of Exp Boost+15",	"EXPB 15",	$RingAccessoryType, $RingWeight, "A Ring of Exp boost+15. %15 Boost.",	$Headers::H, "NULL", "Accessory", true,	",Priest,Rogue,Warrior,Wizard,");
MakeItem("Ring of Exp Boost+25", "Ring of Exp Boost+25",	"EXPB 25",	$RingAccessoryType, $RingWeight, "A Ring of Exp boost+25. %25 Boost.",	$Headers::H, "NULL", "Accessory", true,	",Priest,Rogue,Warrior,Wizard,");
MakeItem("Ring of Exp Boost+50", "Ring of Exp Boost+50 ",	"EXPB 50",	$RingAccessoryType, $RingWeight, "A Ring of Exp boost+50. %50 Boost.",	$Headers::H, "NULL", "Accessory", true,	",Priest,Rogue,Warrior,Wizard,");
MakeItem("Ring of Exp Boost+75", "Ring of Exp Boost+75",	"EXPB 75",	$RingAccessoryType, $RingWeight, "A Ring of Exp boost+75. %75 Boost.",	$Headers::H, "NULL", "Accessory", true,	",Priest,Rogue,Warrior,Wizard,");
MakeItem("Ring of Exp Boost+100", "Ring of Exp Boost+100",	"EXPB 100",	$RingAccessoryType, $RingWeight, "A Ring of Exp boost+100. %100 Boost.",	$Headers::H, "NULL", "Accessory", true,	",Priest,Rogue,Warrior,Wizard,");


SmithCombo($ServerClient, "Brass Ring");
SmithCombo($ServerClient, "Copper Ring");

//	Talisman
for(%i = 5; %i <= 100; %i+=20) {
	MakeItem("Sta Talisman Regen+"@%i, "Sta Talisman Regen+"@%i, "9 "@%i,	$TalismanAccessoryType, 0.5, "A Talisman of Sta regen+"@%i, $Headers::J, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
	MakeItem("HP Talisman Regen+"@%i, "HP Talisman Regen+"@%i, "10 "@%i,	$TalismanAccessoryType, 0.5, "A Talisman of HP regen+"@%i, $Headers::J, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
	MakeItem("MP Talisman Regen+"@%i, "MP Talisman Regen+"@%i, "11 "@%i,	$TalismanAccessoryType, 0.5, "A Talisman of MP regen+"@%i, $Headers::J, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
}

MakeItem("Hero Pendant", "Hero Pendant", "1 100 2 100",		$TalismanAccessoryType, "0.1", "A Hero pendant",		$Headers::J, "NULL", "Accessory", true,			",Warrior,Rogue,");
MakeItem("Pendant of Hope", "Pendant of Hope", "11 200",		$TalismanAccessoryType, "0.1", "A Pendant of Hope and deepest desires.",	$Headers::J, "NULL", "Accessory", true,		",Priest,Wizard,");
MakeItem("Blessed Pendant", "Blessed Pendant", "4 75 5 75",		$TalismanAccessoryType, "0.1", "A Blessed Pendant",				$Headers::J, "NULL", "Accessory", true,			",Wizard,");


//============================================================================================
//======================================= QUEST DATA =========================================
//============================================================================================
//-IMPORTANT-
//All Quest Items must have header as $Headers::Z
//
MakeItem("Newbie Ticket", "newcharpass", "NULL",			"NULL", 1, "A Newbie's ticket to start off his/her great quest.",	$Headers::Z, "NULL", "Ticket", false);

//	BAGS
MakeItem("Tiny Bag", "Tiny Bag", "MaxCOINS 99",						"NULL", 1, "A Tiny Bag that lets you carry max 99 gil.",					$Headers::Z, "NULL", "Bag", false);
MakeItem("Small Bag", "Small Bag", "MaxCOINS 999",					"NULL", 1, "A Small Bag that lets you carry max 999 gil.",				$Headers::Z, "NULL", "Bag", false);
MakeItem("Mid Bag", "Mid Bag", "MaxCOINS 9999",						"NULL", 5, "A Mid Bag that lets you carry max 9,999 gil.",				$Headers::Z, "NULL", "Bag", false);
MakeItem("Large Bag","Large Bag", "MaxCOINS 99999",					"NULL",10,"A Large Bag that lets you carry max 99,999 gil.",			$Headers::Z, "NULL", "Bag", false);
MakeItem("Very Large Bag","Very Large Bag", "MaxCOINS 999999",		"NULL",15,"A XLarge Bag that lets you carry max 999,999 gil.",			$Headers::Z, "NULL", "Bag", false);
MakeItem("Super Large Bag","Super Large Bag", "MaxCOINS 9999999",	"NULL",20,"A XXLarge Bag that lets you carry max 9,999,999 gil.",		$Headers::Z, "NULL", "Bag", false);
MakeItem("Magic Bag", "Magic Bag", "MaxCOINS 999999999",			"NULL", 10,"A Magical Bag that lets you carry max 9,999,999,999 gil.",$Headers::Z, "NULL", "Bag", false);

//						Clients will strip q& off -- so if you have items and want one to be a quest item too (you never know...)
MakeItem("Cure Potion", "q&Cure Potion", "NULL",			"NULL", 1, "A Cure Potion.",			$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Booga Nut", "Booga Nut", "NULL",				"NULL", 1, "A present from booga.",	$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Bag of Acorns", "Bag of Acorns", "NULL",		"NULL", 1, "A bag of acorns.",			$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Rare Coin", "Rare Coin", "NULL",				"NULL", 1, "A odd looking Coin.",		$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Magic Dust", "Magic Dust", "NULL",				"NULL", 1, "Some Magic Dust.",			$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Rusty Key", "Rusty Key", "NULL",				"NULL", 1, "A rusty old key.",				$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Magic Orb", "Magic Orb", "NULL",				"NULL", 1, "A magical Orb.",				$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Stone Key", "Stone Key", "NULL",				"NULL", 1, "The key to Golemai.",			$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Jerky", "Jerky", "NULL",						"NULL", 1, "Some jerky for soup to taste good.",			$Headers::Z, "NULL", "QuestItem", false);

MakeItem("Fire Stone", "Fire Stone", "NULL",			"NULL", 1, "info.",			$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Ice Stone", "Ice Stone", "NULL",			"NULL", 1, "info.",			$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Lightning Stone", "Lightning Stone", "NULL","NULL", 1, "info.",			$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Water Stone", "Water Stone", "NULL",		"NULL", 1, "info.",			$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Earth Stone", "Earth Stone", "NULL",		"NULL", 1, "info.",			$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Wind Stone", "Wind Stone", "NULL",		"NULL", 1, "info.",			$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Black Stone", "Black Stone", "NULL",		"NULL", 1, "info.",			$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Status Stone", "Status Stone", "NULL",	"NULL", 1, "info.",			$Headers::Z, "NULL", "QuestItem", false);

MakeItem("Fire Armor", "Fire Magic Armor", "7 150 3 20 2 -50 Fire 20",						$BodyAccessoryType, 60, "A Fire Armor",		$Headers::C, "NULL", "Accessory", "rpghuman4",				",Priest,Warrior,");
MakeItem("Ice Armor", "Ice Magic Armor", "7 150 3 20 2 -50 Ice 20",							$BodyAccessoryType, 60, "A Ice Armor",		$Headers::C, "NULL", "Accessory", "rpghuman4",				",Priest,Warrior,");
MakeItem("Lightning Armor", "Lightning Magic Armor", "7 150 3 20 2 -50 Lightning 20",		$BodyAccessoryType, 60, "A Lightning Armor",	$Headers::C, "NULL", "Accessory", "rpghuman4",		",Priest,Warrior,");
MakeItem("Water Armor", "Water Magic Armor", "7 150 3 20 2 -50 Water 20",		$BodyAccessoryType, 60, "A Water Armor",	$Headers::C, "NULL", "Accessory", "rpghuman4",		",Priest,Warrior,");
MakeItem("Earth Armor", "Earth Magic Armor", "7 150 3 20 2 -50 Earth 20",			$BodyAccessoryType, 60, "A Earth Armor",	$Headers::C, "NULL", "Accessory", "rpghuman4",		",Priest,Warrior,");
MakeItem("Wind Armor", "Wind Magic Armor", "7 150 3 20 2 -50 Wind 20",			$BodyAccessoryType, 60, "A Wind Armor",		$Headers::C, "NULL", "Accessory", "rpghuman4",		",Priest,Warrior,");
MakeItem("Black Armor", "Black Magic Armor", "7 150 3 20 2 -50 Black 20",			$BodyAccessoryType, 60, "A Black Armor",		$Headers::C, "NULL", "Accessory", "rpghuman4",		",Priest,Warrior,");
MakeItem("Status Armor", "Status Magic Armor", "7 150 3 20 2 -50 Status 20",		$BodyAccessoryType, 60, "A Status Armor",	$Headers::C, "NULL", "Accessory", "rpghuman4",		",Priest,Warrior,");

MakeItem("Fire Robe", "Fire Magic Robe", "7 8 15 100 14 5 Fire 30",		$BodyAccessoryType, 4, "A Fire Robe",			$Headers::C, "Robed", "Accessory", "robered",		",Wizard,");
MakeItem("Ice Robe", "Ice Magic Robe", "7 8 15 100 14 5 Ice 30",			$BodyAccessoryType, 4, "A Ice Robe",			$Headers::C, "Robed", "Accessory", "robeblue",	",Wizard,");
MakeItem("Lightning Robe", "Lightning Magic Robe", "7 8 15 100 14 5 Lightning 30",		$BodyAccessoryType, 4, "A Lightning Robe",			$Headers::C, "Robed", "Accessory", "robewhite",	",Wizard,");
MakeItem("Water Robe", "Water Magic Robe", "7 8 15 100 14 5 Water 30",		$BodyAccessoryType, 4, "A Water Robe",			$Headers::C, "Robed", "Accessory", "robeblue",	",Wizard,");
MakeItem("Earth Robe", "Earth Magic Robe", "7 8 15 100 14 5 Earth 30",			$BodyAccessoryType, 4, "A Earth Robe",			$Headers::C, "Robed", "Accessory", "robebrown",	",Wizard,");
MakeItem("Wind Robe", "Wind Magic Robe", "7 8 15 100 14 5 Wind 30",			$BodyAccessoryType, 4, "A Wind Robe",			$Headers::C, "Robed", "Accessory", "robewhite",	",Wizard,");
MakeItem("Black Robe", "Black Magic Robe", "7 8 15 100 14 5 Black 30",			$BodyAccessoryType, 4, "A Black Robe",			$Headers::C, "Robed", "Accessory", "robeblack",	",Wizard,");
MakeItem("Status Robe", "Status Magic Robe", "7 8 15 100 14 5 Status 30",		$BodyAccessoryType, 4, "A Status Robe",			$Headers::C, "Robed", "Accessory", "robepink",	",Wizard,");

MakeItem("Fire Shield", "Fire Magic Shield", "7 100 2 -50 Fire 20",			$ShieldAccessoryType, 14, "A Fire Shield",	$Headers::D, "shield1", "Accessory", true,	".Priest,Warrior,");
MakeItem("Ice Shield", "Ice Magic Shield", "7 100 2 -50 Ice 20",			$ShieldAccessoryType, 14, "A Ice Shield",		$Headers::D, "shield1", "Accessory", true,	".Priest,Warrior,");
MakeItem("Lightning Shield", "Lightning Magic Shield", "7 100 2 -50 Lightning 20",		$ShieldAccessoryType, 14, "A Lightning Shield",		$Headers::D, "shield1", "Accessory", true,	".Priest,Warrior,");
MakeItem("Water Shield", "Water Magic Shield", "7 100 2 -50 Water 20",		$ShieldAccessoryType, 14, "A Water Shield",		$Headers::D, "shield1", "Accessory", true,	".Priest,Warrior,");
MakeItem("Earth Shield", "Earth Magic Shield", "7 100 2 -50 Earth 20",		$ShieldAccessoryType, 14, "A Earth Shield",		$Headers::D, "shield1", "Accessory", true,	".Priest,Warrior,");
MakeItem("Wind Shield", "Wind Magic Shield", "7 100 2 -50 Wind 20",			$ShieldAccessoryType, 14, "A Wind Shield",		$Headers::D, "shield1", "Accessory", true,	".Priest,Warrior,");
MakeItem("Black Shield", "Black Magic Shield", "7 100 2 -50 Black 20",			$ShieldAccessoryType, 14, "A Black Shield",		$Headers::D, "shield2", "Accessory", true,	".Priest,Warrior,");
MakeItem("Status Shield", "Status Magic Shield", "7 100 2 -50 Status 20",		$ShieldAccessoryType, 14, "A Status Shield",		$Headers::D, "shield3", "Accessory", true,	".Priest,Warrior,");

//Needed to 'smith' quest items-- like the Dragon Saber


MakeItem("Plain Map", "Plain Map", "NULL",		"NULL", 1, "A plain map, not very useful.",	$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Drawing Pen", "Drawing Pen", "NULL",	"NULL", 1, "A drawing pen.",					$Headers::Z, "NULL", "QuestItem", false);

MakeItem("Compass", "Compass", "NULL",				"NULL", 1, "A Compass.",		$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Adv Compass", "Adv Compass", "NULL",		"NULL", 1, "A Adv Compass.",	$Headers::Z, "NULL", "QuestItem", false);
$ItemCost["Compass"] = 500;
$ItemCost["Adv_Compass"] = 2000;


//Mix to make a Burning Door (questItem version)
MakeItem("Magic Door", "Magic Door", "NULL",		"NULL", 1, "A magic door, useless until used with...",			$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Magic Flame", "Magic Flame", "NULL",		"NULL", 1, "A magic flame, useless until used with...",		$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Burning door", "q&Burning door", "NULL",	"NULL", 1, "A door that opens warp holes.",					$Headers::Z, "NULL", "QuestItem", false);

//Key item for a client to 'Master' their class
MakeItem("Priest's Stone", "Priest Stone", "NULL",	"NULL", 1, "A Priest's Stone.",			$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Rogue's Stone", "Rogue Stone", "NULL",	"NULL", 1, "A Rogue's Stone.",			$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Warrior's Stone", "Warrior Stone", "NULL",	"NULL", 1, "A Warrior's Stone.",		$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Wizard's Stone", "Wizard Stone", "NULL",		"NULL", 1, "A Wizard's Stone.",		$Headers::Z, "NULL", "QuestItem", false);

//
MakeItem("Moon Rock", "Moon Rock", "NULL",				"NULL", 1, "A Moon Rock",			$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Soft Moon Rock", "Soft Moon Rock", "NULL",	"NULL", 1, "A Soft Moon Rock",	$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Hard Moon Rock", "Hard Moon Rock", "NULL",	"NULL", 1, "A Hard Moon Rock",	$Headers::Z, "NULL", "QuestItem", false);

//Holy Dragon Saber
MakeItem("Sword Piece (1)", "Sword Piece (1)","NULL",		"NULL", 4, "A piece of the ancient holy sword, the Dragon Saber (1/4)",	$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Sword Piece (2)", "Sword Piece (2)","NULL",		"NULL", 3, "A piece of the ancient holy sword, the Dragon Saber (2/4)",	$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Sword Piece (3)", "Sword Piece (3)","NULL",		"NULL", 6, "A piece of the ancient holy sword, the Dragon Saber (3/4)",	$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Sword Piece (4)", "Sword Piece (4)","NULL",		"NULL", 10, "A piece of the ancient holy sword, the Dragon Saber (4/4)",	$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Dragon Scale", "Dragon Scale","NULL",				"NULL", 12, "A piece of a dragon's scale.",						$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Holy Dragon Saber", "Holy Dragon Saber", "6 800 1 200 2 200 3 200 4 200 5 200",	$SwordAccessoryType, 35, "A Holy Dragon Saber",	$Headers::B, "greensword", "Weapon", false, ",Priest,Warrior,Rogue,Wizard,",		$PiercingDamageType, 1.5, 2);

//Blessed Sword of Divine Intervention
MakeItem("Melted Metal", "Melted Metal", "NULL", 			"NULL", 1, "Some Melted Metal from a Machine.",		$Headers::Z, "NULL", "QuestItem", false);
MakeItem("Blessed Sword of Divine Intervention", "Blessed Sword of Divine Intervention","6 1000 1 50 2 -50",	$SwordAccessoryType, 43, "A Blessed Sword of Divine Intervention, smithed by a God",	$Headers::B, "katana", "Weapon", false, ",Warrior,",	$PiercingDamageType, 2, 2);

//Tech Blade (Need Code + some other stuff)
MakeItem("I/O Tech Blade", "Tech Blade","6 200 2 -10",	$SwordAccessoryType, 43, "A I/O system Tech Blade, created by a programmer.",	$Headers::B, "sword", "Weapon", false, ",Warrior,",	$PiercingDamageType, 3, 1.5);

//--------------------------------------------------------------------------------------------
// Hidden Keys (for special 1 time quests only <storyline help>)
//--------------------------------------------------------------------------------------------

MakeItem("a key", "Awira Spawn Key","NULL",		"NULL", 4, "Key to fight Awira spawn boss",	$Headers::Z, "NULL", "QuestItem", false);
MakeItem("a key", "Awira Final Key","NULL",		"NULL", 4, "Key to fight Awira",				$Headers::Z, "NULL", "QuestItem", false);
MakeItem("a key", "Reiak Spawn Key","NULL",		"NULL", 4, "Key to fight Reiak spawn boss",	$Headers::Z, "NULL", "QuestItem", false);
MakeItem("a key", "Reiak Final Key","NULL",		"NULL", 4, "Key to fight Reiak",				$Headers::Z, "NULL", "QuestItem", false);

MakeItem("a key", "Bam Boss Key","NULL",		"NULL", 4, "Key to fight goblin <f1>Bam<f0> for <f1>Bam Club<f0>",	$Headers::Z, "NULL", "QuestItem", false);

MakeItem("a key", "Sword Fragment 1 Key","NULL",		"NULL", 4, "Key to sword piece 1",	$Headers::Z, "NULL", "QuestItem", false);
MakeItem("a key", "Sword Fragment 2 Key","NULL",		"NULL", 4, "Key to sword piece 2",	$Headers::Z, "NULL", "QuestItem", false);
MakeItem("a key", "Sword Fragment 3 Key","NULL",		"NULL", 4, "Key to sword piece 3",	$Headers::Z, "NULL", "QuestItem", false);
MakeItem("a key", "Sword Fragment 4 Key","NULL",		"NULL", 4, "Key to sword piece 4",	$Headers::Z, "NULL", "QuestItem", false);

//============================================================================================
//============================ EMENY STUFF (BATTLE SPOILS) ===================================
//============================================================================================
MakeItem("Playing Card", "Playing Card", "NULL",		"NULL", "0.1", "A gamblers favorite toy. Show this to a gambler and he will help you out.",	$Headers::W, "NULL", "Spoils", false);
$ItemCost["Playing_Card"] = 100;
MakeItem("Burning Door", "Burning Door", "NULL",		"NULL", 1, "A magical item which creates doors to places unknown.", 							$Headers::W, "NULL", "Spoils", false);
$ItemCost["Burning_Door"] = 10000;
MakeItem("Acorn", "Acorn", "NULL",					"NULL", "0.1", "Battle Spoils from fighting goblins. A goblins favorite snack.",					$Headers::W, "NULL", "Spoils", false);
$ItemCost["Acorn"] = 100;
MakeItem("Dog tooth", "Dog tooth", "NULL",			"NULL", 1, "Battle Spoils from fighting gnolls. A Hounds Tooth. Said to cure disease when injested.",		$Headers::W, "NULL", "Spoils", false);
$ItemCost["Dog_tooth"] = 500;
MakeItem("Zombie Powder", "Zombie Powder", "NULL",	"NULL", 1, "Battle Spoils from fighting zombies. Powder that was used to bring the zombies back to life.",	$Headers::W, "NULL", "Spoils", false);
$ItemCost["Zombie_Powder"] = 5000;
MakeItem("Motor Oil", "Motor Oil", "NULL",			"NULL", "1.5", "Battle Spoils from fighting Cyborgs. Oil that makes cyborgs work like new.",	$Headers::W, "NULL", "Spoils", false);
$ItemCost["Motor_Oil"] = 10000;
MakeItem("Shrapnel", "Shrapnel", "NULL",				"NULL", 1, "Battle Spoils from fighting Cyborgs. Robot parts.",									$Headers::W, "NULL", "Spoils", false);
$ItemCost["Shrapnel"] = 10000;
MakeItem("Soldier Badge", "Soldier Badge", "NULL", 	"NULL", 1, "Battle Spoils from fighting uuags. A badge worn by uuag soldiers.",				$Headers::W, "NULL", "Spoils", false);
$ItemCost["Soldier_Badge"] = 5000;
MakeItem("Leader Badge", "Leader Badge", "NULL", 	"NULL", 1, "Battle Spoils from fighting uuags. A badge worn by uuag leaders.",					$Headers::W, "NULL", "Spoils", false);
$ItemCost["Leader_Badge"] = 10000;
MakeItem("Tear Drop", "Tear Drop", "NULL",			"NULL", 1, "Battle Spoils from fighting Godeyes.",		$Headers::W, "NULL", "Spoils", false);
$ItemCost["Tear_Drop"] = 2000;
MakeItem("Fish Scale", "Fish Scale", "NULL",		"NULL", "0.1", "A hard fish scale.",	$Headers::W, "NULL", "Spoils", false);
$ItemCost["Fish_Scale"] = 50000;

//MakeItem("Pearl", "Pearl", "NULL", 						"NULL", 1, "A beautiful pearl.",							$Headers::W, "NULL", "Spoils", false);
//$ItemCost["Pearl"] = 100000;

MakeItem("Goblin Shield", "GoblinShield", "7 20",				$ShieldAccessoryType, 1, "NULL", $Headers::D, "shield3", "Accessory", true);
MakeItem("Gnoll Shield", "GnollShield", "7 50",				$ShieldAccessoryType, 1, "NULL", $Headers::D, "shield3", "Accessory", true);
MakeItem("Golem Shield", "GolemShield", "7 100",				$ShieldAccessoryType, 1, "NULL", $Headers::D, "shield3", "Accessory", true);
MakeItem("Godeye Shield", "GodeyeShield", "7 300 1 100",	$ShieldAccessoryType, 1, "NULL", $Headers::D, "shield3", "Accessory", true);

//	MASTERBOT SPOILS
MakeItem("Goblin Chief necklace", "Goblin Chief necklace",  "13 50 14 25 12 25",		$TalismanAccessoryType, "0.1", "A GoblinChief's acorn necklace.",	$Headers::J, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
MakeItem("Gnoll Hunter's knife", "Gnoll Hunters knife", "6 3d12",						$SwordAccessoryType, 6, "A Gnoll Hunter's proud hunting knife.", 								$Headers::B, "Dagger", "Weapon", false,	",Priest,Warrior,Rogue,Wizard,",	$PiercingDamageType, 1, 1,  SoundSwing1, SoundGnollTaunt1);
MakeItem("Zombie Ring", "Zombie Ring",	"NULL",										$RingAccessoryType, "0.1", "Even the Zombies don't use these...", 				$Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
MakeItem("Golem Armor", "Golem Armor", "7 150 3 50 15 -50 2 -50",					$BodyAccessoryType, 82, "A Golem rock-like Armor", 										$Headers::C, "NULL", "Accessory", "RMStone",	",Priest,Warrior,Rogue,");
MakeItem("Code", "Code", "NULL",															"NULL", 1, "A Code for a program I/O system",		$Headers::Z, "NULL", "QuestItem");
MakeItem("GodEye Ring", "GodEye Ring", "20 Blind 20 Petrify",							$RingAccessoryType, $RingWeight, "A Godeye Ring",	$Headers::H, "NULL", "Accessory", true,	",Priest,Warrior,Wizard,");

//USEFUL MONSTER DROPS
MakeItem("Rusty Ring", "Rusty Ring S", "1 10",			$RingAccessoryType, $RingWeight, "A rusty ring. Attributes <f1>Unknown<f0>.",		$Headers::H, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemData["Rusty_Ring_S", BlockInfo] = true;
MakeItem("Rusty Ring", "Rusty Ring D", "2 10",			$RingAccessoryType, $RingWeight, "A rusty ring. Attributes <f1>Unknown<f0>.",		$Headers::H, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemData["Rusty_Ring_D", BlockInfo] = true;
MakeItem("Rusty Ring", "Rusty Ring C", "3 10",			$RingAccessoryType, $RingWeight, "A rusty ring. Attributes <f1>Unknown<f0>.",		$Headers::H, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemData["Rusty_Ring_C", BlockInfo] = true;
MakeItem("Rusty Ring", "Rusty Ring I", "4 10",			$RingAccessoryType, $RingWeight, "A rusty ring. Attributes <f1>Unknown<f0>.",		$Headers::H, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemData["Rusty_Ring_I", BlockInfo] = true;
MakeItem("Rusty Ring", "Rusty Ring W", "5 10",			$RingAccessoryType, $RingWeight, "A rusty ring. Attributes <f1>Unknown<f0>.",		$Headers::H, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemData["Rusty_Ring_W", BlockInfo] = true;
MakeItem("Rusty Ring", "Rusty Ring E", "6 10",			$RingAccessoryType, $RingWeight, "A rusty ring. Attributes <f1>Unknown<f0>.",		$Headers::H, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemData["Rusty_Ring_E", BlockInfo] = true;

MakeItem("Giant Gauntlets", "Giant Gauntlets", "13 200",					$HandsAccessoryType, 3, "A pair of very large <f1>Gauntlets<f0>.",		$Headers::I, "NULL", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
MakeItem("Uuag Boots", "Uuag Boots", "7 50 2 50 13 50",				$BootsAccessoryType, 18, "A pair of Uuag Boots.", 		$Headers::G, "NULL", "Accessory", true,	",Warrior,");

//BROTHERS RINGS
MakeItem("Ring of Reiak", "Ring of Reiak", "6 1000 1 100 2 100 3 100 6 100 EXPB 20 20 Poison 20 Blind",			$RingAccessoryType, $RingWeight, "The ring of Reiak. <f1>Physical Stats +<f0>.",		$Headers::H, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemData["Ring_of_Reiak", ToUseSkill] = "LVL 200";
MakeItem("Ring of Awira", "Ring of Awira", "6 500 3 50 4 100 5 100 6 50 EXPB 20 20 Mute 20 Petrify",			$RingAccessoryType, $RingWeight, "The ring of Awira. <f1>Magic Stats +<f0>.",		$Headers::H, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemData["Ring_of_Awira", ToUseSkill] = "LVL 200";

//Rare Items
MakeItem("Burning Key", "Burning Key", "NULL","NULL","3","A burning key said to open a burning door.",$Headers::R, "NULL", "Spoils", false);
$ItemCost["Burning_Key"] = 100000;

MakeItem("Beast Shadow", "Beast Shadow", "6 200",			$PolearmAccessoryType, 20, "A rare and powerful weapon.",	$Headers::B, "elfinblade", "Weapon", false, ",Priest,Warrior,Rogue,Wizard,",			$PiercingDamageType, 4, 2.5,  SoundSwing3);
$ItemData["Beast_Shadow", ToUseSkill] = "LVL 30";
MakeItem("Dream Sword", "Dream Sword", "6 100",			$SwordAccessoryType, 5, "A sword smithed in the world of dreams.",				$Headers::B, "sword", "Weapon", false, ",Priest,Warrior,Rogue,Wizard,",		$SlashingDamageType, 2, 2, SoundSwing6);
$ItemData["Dream_Sword", ToUseSkill] = "LVL 20";
MakeItem("Holy Sword of Light", "Holy Sword of Light", "6 400",				$SwordAccessoryType, 10, "The sword of a powerful priest.",				$Headers::B, "long_sword", "Weapon", false, ",Priest,Warrior,Rogue,Wizard,",		$SlashingDamageType, 2, 2, SoundSwing6);
$ItemData["Holy_Sword_of_Light", ToUseSkill] = "LVL 40";
MakeItem("Torment", "Torment", "6 800",							$SwordAccessoryType, 20, "An extremely rare axe.",				$Headers::B, "BattleAxe", "Weapon", false, ",Priest,Warrior,Rogue,Wizard,",		$SlashingDamageType, 4, 3, SoundSwing6);
$ItemData["Torment", ToUseSkill] = "LVL 60";
MakeItem("Daemon", "Daemon", "6 400 1 50 2 40 4 40 4 -40 5 -40",			$SwordAccessoryType, 20, "An extremely rare sword.",				$Headers::B, "elfinblade", "Weapon", false, ",Priest,Warrior,Rogue,Wizard,",		$SlashingDamageType, 4, 3, SoundSwing6);
$ItemData["Daemon", ToUseSkill] = "LVL 20";
MakeItem("Silent Night", "Silent Night", "6 100 32 50",							$SwordAccessoryType, 10, "An extremely rare knife that mutes.",				$Headers::B, "dagger", "Weapon", false, ",Priest,Warrior,Rogue,",		$SlashingDamageType, 4, 3, SoundSwing6);
$ItemData["Silent_Night", ToUseSkill] = "LVL 50";
MakeItem("Goriks Hammer", "Goriks Hammer", "6 100",							$SwordAccessoryType, 20, "A unique item.",				$Headers::B, "hammer", "Weapon", false, ",Priest,Warrior,Rogue,Wizard,",		$SlashingDamageType, 4, 3, SoundSwing6);
$ItemData["Goriks_Hammer", ToUseSkill] = "LVL 10";
MakeItem("Sinister Foil", "Sinister Foil", "6 600",							$SwordAccessoryType, 5, "A unique item.",				$Headers::B, "katana", "Weapon", false, ",Rogue,",		$SlashingDamageType, 4, 3, SoundSwing6);
$ItemData["Sinister_Foil", ToUseSkill] = "LVL 40";

//=================================ABILITY GEMS=========================================

MakeItem("Light Green Gem", "Light Green Gem", "1 5",			$GemAccessoryType, $RingWeight, "A gem that increases <f1>Str<f0>.",		$Headers::U, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemCost["Light_Green_Gem"] = 25000;
MakeItem("Green Gem", "Green Gem", "1 25",			$GemAccessoryType, $RingWeight, "A gem that increases <f1>Str<f0>.",		$Headers::U, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemCost["Green_Gem"] = 200000;
MakeItem("Dark Green Gem", "Dark Green Gem", "1 50",			$GemAccessoryType, $RingWeight, "A gem that increases <f1>Str<f0>.",		$Headers::U, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemCost["Dark_Green_Gem"] = 400000;

MakeItem("Light Yellow Gem", "Light Yellow Gem", "2 5",			$GemAccessoryType, $RingWeight, "A gem that increases <f1>Dex<f0>.",		$Headers::U, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemCost["Light_Yellow_Gem"] = 25000;
MakeItem("Yellow Gem", "Yellow Gem", "2 25",			$GemAccessoryType, $RingWeight, "A gem that increases <f1>Dex<f0>.",		$Headers::U, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemCost["Yellow_Gem"] = 200000;
MakeItem("Dark Yellow Gem", "Dark Yellow Gem", "2 50",			$GemAccessoryType, $RingWeight, "A gem that increases <f1>Dex<f0>.",		$Headers::U, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemCost["Dark_Yellow_Gem"] = 400000;

MakeItem("Light Red Gem", "Light Red Gem", "3 5",			$GemAccessoryType, $RingWeight, "A gem that increases <f1>Con<f0>.",		$Headers::U, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemCost["Light_Red_Gem"] = 25000;
MakeItem("Red Gem", "Red Gem", "3 25",			$GemAccessoryType, $RingWeight, "A gem that increases <f1>Con<f0>.",		$Headers::U, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemCost["Red_Gem"] = 200000;
MakeItem("Dark Red Gem", "Dark Red Gem", "3 50",			$GemAccessoryType, $RingWeight, "A gem that increases <f1>Con<f0>.",		$Headers::U, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemCost["Dark_Red_Gem"] = 400000;

MakeItem("Light Purple Gem", "Light Purple Gem", "4 5",			$GemAccessoryType, $RingWeight, "A gem that increases <f1>Wis<f0>.",		$Headers::U, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemCost["Light_Purple_Gem"] = 25000;
MakeItem("Purple Gem", "Purple Gem", "4 25",			$GemAccessoryType, $RingWeight, "A gem that increases <f1>Wis<f0>.",		$Headers::U, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemCost["Purple_Gem"] = 200000;
MakeItem("Dark Purple Gem", "Dark Purple Gem", "4 50",			$GemAccessoryType, $RingWeight, "A gem that increases <f1>Wis<f0>.",		$Headers::U, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemCost["Dark_Purple_Gem"] = 400000;

MakeItem("Light Blue Gem", "Light Blue Gem", "5 5",			$GemAccessoryType, $RingWeight, "A gem that increases <f1>Int<f0>.",		$Headers::U, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemCost["Light_Blue_Gem"] = 25000;
MakeItem("Blue Gem", "Blue Gem", "5 25",			$GemAccessoryType, $RingWeight, "A gem that increases <f1>Int<f0>.",		$Headers::U, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemCost["Blue_Gem"] = 200000;
MakeItem("Dark Blue Gem", "Dark Blue Gem", "5 50",			$GemAccessoryType, $RingWeight, "A gem that increases <f1>Int<f0>.",		$Headers::U, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemCost["Dark_Blue_Gem"] = 400000;

MakeItem("Light Orange Gem", "Light Orange Gem", "6 5",			$GemAccessoryType, $RingWeight, "A gem that increases <f1>Sta<f0>.",		$Headers::U, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemCost["Light_Orange_Gem"] = 25000;
MakeItem("Orange Gem", "Orange Gem", "6 25",			$GemAccessoryType, $RingWeight, "A gem that increases <f1>Sta<f0>.",		$Headers::U, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemCost["Orange_Gem"] = 200000;
MakeItem("Dark Orange Gem", "Dark Orange Gem", "6 50",			$GemAccessoryType, $RingWeight, "A gem that increases <f1>Sta<f0>.",		$Headers::U, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
$ItemCost["Dark_Orange_Gem"] = 400000;

//============================================================================================
//=================================== MISCELLANY DATA =======================================
//============================================================================================
MakeItem("Life Stream Key", "Life Stream Key", "NULL",			"NULL", "0.1", "A key to ???",									$Headers::R, "NULL", "Key");
$LoreItem["Life_Stream_Key"] = True;
MakeItem("Red Chocobo Egg", "Red Chocobo Egg", "NULL",		"NULL", 35, "A very large and heavy Red Chocobo Egg.",		$Headers::R, "NULL", "Egg");
MakeItem("Blue Chocobo Egg", "Blue Chocobo Egg", "NULL",		"NULL", 35, "A very large and heavy Blue Chocobo Egg.",		$Headers::R, "NULL", "Egg");
MakeItem("White Chocobo Egg", "White Chocobo Egg", "NULL",	"NULL", 35, "A very large and heavy White Chocobo Egg.",	$Headers::R, "NULL", "Egg");
MakeItem("Gold Chocobo Egg", "Gold Chocobo Egg", "NULL",		"NULL", 35, "A very large and heavy Gold Chocobo Egg.",		$Headers::R, "NULL", "Egg");

MakeItem("Weapon","Weapon","NULL",		"NULL", 1, "NULL",		$Headers::R, "dagger", "Weapon");//Dummy -For Drop("Weapon"); will get mounted weapon and drop that.
MakeItem("BackPack","BackPack","NULL",	"NULL", 1, "NULL",		$Headers::R, "NULL", "BackPack");//Dummy -For on consider
MakeItem("toggleShield","toggleShield","NULL",	"NULL", 1, "NULL",		$Headers::R, "NULL", "toggleShield");//Dummy -For on toggle Shield

MakeItem("Loot Bag","LootBag","NULL",		"NULL", 1, "Loot Bag",	$Headers::R, "ammo2", "Lootbag");//Dummy -For Loot Bags
if($CheckFunc[ammo2] != "Loaded") {
	if($CheckFuncCnt >= 200) {
		echo("Warning to many Model Shapes (MAX 200) Tried to add Shape 'ammo2' -THIS SHAPE IS NEEDED! The LootBag!-");
		return;
	}
	ItemData LootBag { description = "Backpack"; className = "Lootbag"; shapeFile = "ammo2"; heading = "eMiscellany"; shadowDetailMask = 4; price = 0; };
	$CheckFunc[ammo2] = "Loaded";
	$CheckFuncCnt++;
	if($ShowMakeModel) Echo("Model Shape Added: Shape:'ammo2' Type:LootBag");
}

MakeItem("Comiat", "Comiat", "EXPB 100 20 Poison 20 Blind",			$RingAccessoryType, $RingWeight, "A unique ring Chee found one day while wandering the <f1>Lower Stratum<f0>.",		$Headers::H, "NULL", "Accessory", true,		",Priest,Warrior,Rogue,Wizard,");
MakeItem("Sorga Shield", "Sorga Shield", "7 20 1 40 2 40 13 200 20 Petrify 20 Mute",	$ShieldAccessoryType, 5, "A unique shield Chee found one day while wandering the <f1>Howling Earth<f0>.",	$Headers::D, "shield2", "Accessory", true,	",Priest,Warrior,Rogue,Wizard,");
$ItemData["Sorga_Shield", ToUseSkill] = "LVL 1";

MakeItem("Room Key", "Room Key", "NULL",		"NULL", 0.1, "A key to a room at <f1>Shildra's Inn<f0>.",		$Headers::R, "NULL", false);

MakeItem("Orb of Light", "Orb of Light", "NULL",		$OrbAccessoryType, 1, "A Orb of Light.",	$Headers::R, "Orb", "Accessory", true, ",Priest,Warrior,Rogue,Wizard,");

if($CheckFunc[orb] != "Loaded") {
	if($CheckFuncCnt >= 200) {
		echo("Warning to many Model Shapes (MAX 200) Tried to add Shape 'orb'");
		return;
	}
	ItemImageData OrbImage { shapeFile = "orb"; mountPoint = 3; mountOffset = {0.0, 0.0, 1.8}; mountRotation = {5, 3, 3}; lightType = 2; lightRadius = 13; lightTime = 9999; lightColor = { 0.95, 0.85, 0.55 }; };
	ItemData Orb { description = "Orb Model"; className = "Accessory"; shapeFile = "orb"; imageType = OrbImage; heading = "eMiscellany"; price = 0; };

	$CheckFunc[orb] = "Loaded";
	$CheckFuncCnt++;
	if($ShowMakeModel) Echo("Model Shape Added: Shape:'orb' Type:Accessory");
}

// staticshape.cs
ItemData MaleHumanTownBot
{
	description = "Male Town Bot";
	className = "TownBot";
	shapeFile = "rpgmalehuman";

	disableCollision = false;
	visibleToSensor = true;
	mapFilter = 1;
};
ItemData FemaleHumanTownBot
{
	description = "Female Town Bot";
	className = "TownBot";
	shapeFile = "lfemalehuman";

	disableCollision = false;
	visibleToSensor = true;
	mapFilter = 1;
};

//shapeFile = "goblin";
//disableCollision = false;
//description = "funny goblin";
//visibleToSensor = true;

ItemData cgoblin
{
	description = "funny goblin";
	className = "TownBot";
	shapeFile = "goblin";

	disableCollision = false;
	visibleToSensor = true;
	mapFilter = 1;
};

// Invisible townbot datablock for hidden quest NPCs (e.g. ChiefCaracal in TownBots.cs).
// Was missing, so MakeTownBot("invisibleman",...) failed newObject -> $townbot=0 ->
// "addToSet: Object 0 doesn't exist". shapeFile "invisable" = RMRPG\invisable.dts.
ItemData invisibleman
{
	description = "Invisible Town Bot";
	className = "TownBot";
	shapeFile = "invisable";

	disableCollision = true;
	visibleToSensor = true;
	mapFilter = 1;
};

	$CheckFunc[MaleHumanTownBot] = "Loaded";
	$CheckFuncCnt++;
	$CheckFunc[FemaleHumanTownBot] = "Loaded";
	$CheckFuncCnt++;
	$CheckFunc[cgoblin] = "Loaded";
	$CheckFuncCnt++;
	$CheckFunc[invisibleman] = "Loaded";
	$CheckFuncCnt++;
	if($CheckFuncCnt >= 200) {
		echo("Warning to many Model Shapes (MAX 200) Tried to add TownBots!");
		return;
	}
//============================================================================================
//===================================== RUNES DATA ==========================================
//============================================================================================


MakeItem("Rune Red", "Rune Red", "NULL",			"NULL", 0.5, "A Red Rune.",		$Headers::Y, "NULL", "Runes");
$ItemCost["Rune_Red"] = 10000;
MakeItem("Rune Green", "Rune Green", "NULL",		"NULL", 0.5, "A Green Rune.",		$Headers::Y, "NULL", "Runes");
$ItemCost["Rune_Green"] = 10000;
MakeItem("Rune Blue", "Rune Blue", "NULL",			"NULL", 0.5, "A Blue Rune.",		$Headers::Y, "NULL", "Runes");
$ItemCost["Rune_Blue"] = 10000;
MakeItem("Rune Yellow", "Rune Yellow", "NULL",		"NULL", 0.5, "A Yellow Rune.",		$Headers::Y, "NULL", "Runes");
$ItemCost["Rune_Yellow"] = 10000;
MakeItem("Rune White", "Rune White", "NULL",		"NULL", 0.5, "A White Rune.",		$Headers::Y, "NULL", "Runes");
$ItemCost["Rune_White"] = 10000;
MakeItem("Rune Black", "Rune Black", "NULL",		"NULL", 0.5, "A Black Rune.",		$Headers::Y, "NULL", "Runes");
$ItemCost["Rune_Black"] = 10000;
MakeItem("Rune Cyan", "Rune Cyan", "NULL",		"NULL", 0.5, "A Cyan Rune.",		$Headers::Y, "NULL", "Runes");
$ItemCost["Rune_Cyan"] = 10000;
MakeItem("Rune Orange", "Rune Orange", "NULL",		"NULL", 0.5, "A Orange Rune.",		$Headers::Y, "NULL", "Runes");
$ItemCost["Rune_Orange"] = 10000;
MakeItem("Rune Grey", "Rune Grey", "NULL",		"NULL", 0.5, "A Grey Rune.",		$Headers::Y, "NULL", "Runes");
$ItemCost["Rune_Grey"] = 10000;
MakeItem("Rune Pink", "Rune Pink", "NULL",		"NULL", 0.5, "A Pink Rune.",		$Headers::Y, "NULL", "Runes");
$ItemCost["Rune_Pink"] = 10000;
MakeItem("Rune Purple", "Rune Purple", "NULL",		"NULL", 0.5, "A Purple Rune.",		$Headers::Y, "NULL", "Runes");
$ItemCost["Rune_Purple"] = 10000;

$RuneColorList[0] = "Red";
$RuneColorList[1] = "Green";
$RuneColorList[2] = "Blue";
$RuneColorList[3] = "Yellow";
$RuneColorList[4] = "White";
$RuneColorList[5] = "Black";
$RuneColorList[6] = "Cyan";
$RuneColorList[7] = "Orange";
$RuneColorList[8] = "Grey";
$RuneColorList[9] = "Pink";
$RuneColorList[10] = "Purple";


LoadBlackSmithItems();

//=================
if($ItemDataErrorCount > 0)
	$ItemData::ExtraInfo = " Type  Itemlog();  to view error(s).";
echo(">> ItemCount: "@$ItemDataCounter@" - ModelCount: "@$CheckFuncCnt@" - Errors reported: "@$ItemDataErrorCount@$ItemData::ExtraInfo);
}