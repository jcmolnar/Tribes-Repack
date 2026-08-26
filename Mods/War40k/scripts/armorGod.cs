// Hidden Armor
// by C|one
$InvList[iarmorGod] = 0;
$ArmorName[armorGod] = iarmorGod;
$ArmorType[Male, iarmorGod] = armorGod;
$ArmorType[Female, iarmorGod] = armorGod;


ItemData iarmorGod
{
	$InvHead[ihArm] = "aGeneric Troops";
	description = "God";
	className = "Armor";
	price = 0;
};

PlayerData armorGod
{
	shieldStrength = 100;
	isTranslucent = true;
	className = "Armor";
	shapeFile = "logo";
	flameShapeName = "undefined";
	shieldShapeName = "undefined";
	damageSkinData = "armorDamageSkins";
	debrisId = defaultDebrisLarge;
	shadowDetailMask = 0;
	canCrouch = false;
	visibleToSensor = false;
	mapFilter = 1;
	mapIcon = "M_player";
	maxJetSideForceFactor = 1.0;
	maxJetForwardVelocity = 20;
	minJetEnergy = 0;
	jetForce = 500;
	jetEnergyDrain = 0;
	maxDamage = 100;
	maxForwardSpeed = 20.0;
	maxBackwardSpeed = 20.0;
	maxSideSpeed = 20.0;
	groundForce = 80 * 35.0;
	mass = 20.0;
	groundTraction = 80.0;
	maxEnergy = 10000;
	drag = 1.0;
	density = 1.0;
	minDamageSpeed = 9999;
	damageScale = 0;
	jumpImpulse = 300;
	jumpSurfaceMinDot = 0.4;
// animation data:
// animation name, one shot, exclude, direction
// firstPerson, chaseCam, thirdPerson, signalThread
// movement animations:
	animData[0]= { "root", none, 1, true, true, true, false, 0 };
	animData[1]= { "root", none, 1, true, true, true, false, 0 };
	animData[2]= { "root", none, 1, true, true, true, false, 0 };
	animData[3]= { "root", none, 1, true, true, true, false, 0 };
	animData[4]= { "root", none, 1, true, true, true, false, 0 };
	animData[5] = { "root", none, 1, true, true, true, false, 0 };
	animData[6] = { "root", none, 1, true, true, true, false, 0 };
	animData[7] = { "root", none, 1, true, true, true, false, 0 };
	animData[8] = { "root", none, 1, true, true, true, false, 0 };
	animData[9] = { "root", none, 1, true, true, true, false, 0 };
	animData[10] = { "root", none, 1, true, true, true, false, 0 };
	animData[11] = { "root", none, 1, true, true, true, false, 0 };
	animData[12] = { "root", none, 1, true, true, true, false, 0 };
	animData[13] = { "root", none, 1, true, true, true, false, 0 };
	animData[14]= { "root", none, 1, true, true, true, false, 0 };
	animData[15]= { "root", none, 1, true, true, true, false, 0 };
	animData[16]= { "root", none, 1, true, true, true, false, 0 };
	animData[17]= { "root", none, 1, true, true, true, false, 0 };
	animData[18]= { "root", none, 1, true, true, true, false, 0 };
	animData[19] = { "root", none, 1, true, true, true, false, 0 };
// misc. animations:
	animData[20] = { "PDA access", SoundElevatorStop, 1, true, false, false, false, 3 };
	animData[21] = { "throw", none, 1, true, false, false, false, 3 };
	animData[22] = { "flyer root", SoundElevatorStop, 1, false, false, false, false, 3 };
	animData[23] = { "apc root", SoundElevatorStop, 1, true, true, true, false, 3 };
	animData[24] = { "apc pilot", SoundElevatorStop, 1, false, false, false, false, 3 };
// death animations:
	animData[25] = { "crouch die", shockExplosion, 1, false, false, false, false, 4 };
	animData[26] = { "die chest", shockExplosion, 1, false, false, false, false, 4 };
	animData[27] = { "die head", shockExplosion, 1, false, false, false, false, 4 };
	animData[28] = { "die grab back", shockExplosion, 1, false, false, false, false, 4 };
	animData[29] = { "die right side", shockExplosion, 1, false, false, false, false, 4 };
	animData[30] = { "die left side", shockExplosion, 1, false, false, false, false, 4 };
	animData[31] = { "die leg left", shockExplosion, 1, false, false, false, false, 4 };
	animData[32] = { "die leg right", shockExplosion, 1, false, false, false, false, 4 };
	animData[33] = { "die blown back", shockExplosion, 1, false, false, false, false, 4 };
	animData[34] = { "die spin", shockExplosion, 1, false, false, false, false, 4 };
	animData[35] = { "die forward", shockExplosion, 1, false, false, false, false, 4 };
	animData[36] = { "die forward kneel", shockExplosion, 1, false, false, false, false, 4 };
	animData[37] = { "die back", shockExplosion, 1, false, false, false, false, 4 };
// signal moves:
	animData[38] = { "sign over here",SoundElevatorStop, 1, true, false, false, false, 2 };
	animData[39] = { "sign point", SoundElevatorStop, 1, true, false, false, true, 1 };
	animData[40] = { "sign retreat",SoundElevatorStop, 1, true, false, false, false, 2 };
	animData[41] = { "sign stop", SoundElevatorStop, 1, true, false, false, true, 1 };
	animData[42] = { "sign salut", SoundElevatorStop, 1, true, false, false, true, 1 }; 
// celebraton animations:
	animData[43] = { "celebration 1", SoundActivatePDA, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", SoundActivatePDA, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", SoundActivatePDA, 1, true, false, false, false, 2 };
// taunt anmations:
	animData[46] = { "taunt 1", SoundActivatePDA, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", SoundActivatePDA, 1, true, false, false, false, 2 };
// poses:
	animData[48] = { "pose kneel", SoundActivatePDA, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", SoundActivatePDA, 1, true, false, false, true, 1 };
// Bonus wave
	animData[50] = { "wave", SoundActivatePDA, 1, true, false, false, true, 1 };
	jetSound = SoundLaserIdle;
	rFootSounds = { undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined}; 
	lFootSounds = { undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined};
	footPrints = { 6, 6 };
	boxWidth = 0.01;
	boxDepth = 0.01;
	boxNormalHeight = 0.01;
	boxNormalHeadPercentage = 0;
	boxNormalTorsoPercentage = 1;
	boxHeadLeftPercentage = 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage = 0;
	boxHeadFrontPercentage = 1;
};

function armorGod::onPlayerContact(%targetPlayer, %sourcePlayer)
{
}

function armorGod::onGrenade(%player)
{
	Client::sendMessage(Player::getClient(%player),1, "Elemination Device Activated");
	%obj = newObject("","Mine","EverBoom");
	Armor::ThrowGrenade(%player, %obj);
}

function armorGod::onBeacon(%player, %item)
{
	Client::sendMessage(Player::getClient(%player),1, "Elemination Device Activated");
	%obj = newObject("","Mine","EverBoom");
	Armor::ThrowGrenade(%player, %obj);
}

function armorGod::onRepairKit(%player)
{
	Armor::onRepairKit(%player);
}