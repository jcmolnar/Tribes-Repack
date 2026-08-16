
if($console::logmode == "") $console::logmode = 0;
if($dbechoMode == "") $dbechoMode = 2;
if($dbechoMode2 == "") $dbechoMode2 = 2;

$arenaOn = False;
if($underwaterEffects == "") $underwaterEffects = False;
if($SaveWorldFreq == "") $SaveWorldFreq = 15 * 60;
if($ChangeWeatherFreq == "") $ChangeWeatherFreq = 9999999;
if($initlck == "") $initlck = 30;
if($MaxDroppedPacksPerPlayer == "") $MaxDroppedPacksPerPlayer = 30; //10
if($MaxCanHoldItem == "") $MaxCanHoldItem = 99;
if($AIsmartFOVbots == "") $AIsmartFOVbots = False;
if($SelectiveZoneBotSpawning == "") $SelectiveZoneBotSpawning = True;

//-------------------- Night / Day stuff
$initHaze = 900;
$currentHaze = $initHaze;
$currentSky = "lushsky_night.dml";

$dayCycleSky[1] = "lushsky_night.dml";
$dayCycleSky[2] = "lushsky_night.dml";
$dayCycleSky[3] = "lushsky_night.dml";
$dayCycleSky[4] = "lushsky_night.dml";
$dayCycleSky[5] = "lushsky_night.dml";

$dayCycleHaze[0] = 900;
$dayCycleHaze[1] = 600;
$dayCycleHaze[2] = 300;
$dayCycleHaze[3] = -300;
$dayCycleHaze[4] = -600;
$dayCycleHaze[5] = -900;

if($fullCycleTime == "") $fullCycleTime = 60 * 60;
if($nightDayCycle == "") $nightDayCycle = True;
//--------------------------------------

$RecalcEconomyDelay = 60 * 5;
$resalePercentage = 10;
$CorpseTimeoutValue = 1.5;
$invalidChars = " ><?\\\"{}[]+=:;/.,~!@#$%^&*()|`";
$invalidChars2 = " ><?\\\"{}[]+=:;/.,!@#$%^&*()|`~";
$maxDamagedBy = 10;
$damagedByEraseDelay = 60;

$AImoveChance = 2;		//as long as the bot is a spawn-bot, there is a chance in
					//2 every 5 seconds that he will create himself a marker
					//at a certain distance and go to it.
$AIattackMarkerChance = 100;	//as long as the bot is a spawn-bot and not in a zone, there is a chance in
					//100 every 5 seconds that he will pick an attackMarker and
					//find his way to it.
$AIcloseEnoughMarkerDist = 300;
$AIspotDist = 4.0;
$AIFOVPan = 6.5;
$AImaxRange = 90;
$AIstartAttacking = 10;
$AIminrad = 10;
$AImaxrad = 100;

//$addedShopMsg = "  I also have a few things you might want to BUY.";
//$addedShopMsg = "";
$waitActionDelay = 0.75;
$sayDelay = 0.2;
$triggerDelay = 2;
$Server::respawnTime = 8; //How long you have to wait before you can spawn again.

//$LootbagPopTime = 60*30;//30mins
//$LootbagPopTime = 60*600;
$LootbagPopTime = -1;

$TribesDamageToNumericDamage = 100.0;
$RepairPerFiveSeconds = 1;

$waterDamageAmp = 0.1;


//steal vars.
$initstealskill = 15;
$maxstealskill = 100;
//$successStealFactor = 1.02;
//$failStealFactor = 0.0;
//$minStealPerc = 30;
//$maxStealPerc = 75;
//$maxSTEALdistVec = 2.3;

$stealDelay = 5;

$initbankcoins = 100;

$droppingAllowed = 1;

$sepchar = ",";

$maxSAYdistVec = 20;
$maxSHOUTdistVec = 60;
$maxAIdistVec = 5;

//$AItalkcolor = 3;
$AIdelayTalk = 0.65;
//$AIwait[merchant] = 10;
$AIwait[banker] = 25;
$AIwait[assassin] = 10;
$AIwait[mage] = 45;
$AIwait[porter] = 15;
$AIwait[manager] = 30;
$AIwait[quest] = 30;

$coinsrewardperlevel = 850;

$WorldSaveList = "|Lootbag|";//"|DepPlatSmallHorz|DepPlatMediumHorz|DepPlatSmallVert|DepPlatMediumVert|Lootbag|";

$SlashingDamageType	= 1;
$PiercingDamageType	= 2;
$BludgeoningDamageType	= 4;


$pref::pingTimeoutTime = 9000;
$pref::requestTimeoutTime = 90;