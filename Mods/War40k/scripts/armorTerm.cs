//TERMINATOR
// by Edgecrusher
$InvList[iarmorTerm] = 1;
$RemoteInvList[iarmorTerm] = 0;
$ArmorType[Male, iarmorTerm] = armorTerm;
$ArmorType[Female, iarmorTerm] = armorTerm;
$ArmorName[armorTerm] = iarmorTerm;

ItemData iarmorTerm
{
	heading = $InvHead[ihMar];
	description = "Terminator";
	className = "Armor";
	price = 42;
};

PlayerData armorTerm
{
	className = "Armor";
	shapeFile = "TDA";
	flameShapeName = "hflame";
	shieldShapeName = "shield";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	shadowDetailMask = 1;
	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
	maxJetSideForceFactor = 1.1;
	maxJetForwardVelocity = 1;
	minJetEnergy = 1;
	jetForce = 4;
	jetEnergyDrain = 0.0;
	maxDamage = 4.00;
	maxForwardSpeed = 7.6;
	maxBackwardSpeed = 6.0;
	maxSideSpeed = 6.0;
	groundForce = 35 * 18.0;
	groundTraction = 4.5;
	mass = 18.0;
	maxEnergy = 140;
	drag = 1.0;
	density = 2.5;
	canCrouch = false;
	minDamageSpeed = 25;
	damageScale = 0.006;
	jumpImpulse = 200;
	jumpSurfaceMinDot = 0.2;
	animData[0] = { "root", none, 1, true, true, true, false, 0 };
	animData[1] = { "run", none, 1, true, false, true, false, 3 };
	animData[2] = { "runback", none, 1, true, false, true, false, 3 };
	animData[3] = { "side left", none, 1, true, false, true, false, 3 };
	animData[4] = { "side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
	animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
	animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[14] = { "fall", none, 1, true, true, true, false, 3 };
	animData[15] = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16] = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17] = { "tumble loop", none, 1, true, false, false, false, 3 };
	animData[18] = { "tumble end", none, 1, true, false, false, false, 3 };
	animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
	animData[21] = { "throw", none, 1, true, false, false, false, 3 };
	animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
	animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
	animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
	animData[25] = { "crouch die", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[26] = { "die chest", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[27] = { "die head", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[28] = { "die grab back", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[29] = { "die right side", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[30] = { "die left side", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[31] = { "die leg left", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[32] = { "die leg right", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[33] = { "die blown back", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[34] = { "die spin", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[35] = { "die forward", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[36] = { "die forward kneel", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[37] = { "die back", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[38] = { "sign over here", none, 1, true, false, false, false, 2 };
	animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
	animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
	animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
	animData[42] = { "sign salut", none, 1, true, false, false, true, 1 };
	animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };
	animData[50] = { "wave", none, 1, true, false, false, true, 1 };
	jetSound = SoundJetHeavy;
	rFootSounds = { SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk };
	lFootSounds = { SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk, SoundHeavyWalk };
	footPrints = { 4, 5 };
	boxWidth = 0.8;
	boxDepth = 0.8;
	boxNormalHeight = 2.6;
	boxNormalHeadPercentage = 0.70;
	boxNormalTorsoPercentage = 0.45;
	boxHeadLeftPercentage = 0.48;
	boxHeadRightPercentage = 0.70;
	boxHeadBackPercentage = 0.48;
	boxHeadFrontPercentage = 0.60;
};

function armorTerm::onPlayerContact(%targetPlayer, %sourcePlayer)
{
	Armor::onPlayerContact(%targetPlayer, %sourcePlayer);
}

function armorTerm::onGrenade(%player)
{
	%obj = newObject("","Mine","Mortarbomb");
	Armor::ThrowGrenade(%player, %obj);
}

function armorTerm::onBeacon(%player, %item)
{
	startShield(Player::getClient(%player), %player);
	Player::decItemCount(%player,%item);
}

function armorTerm::onRepairKit(%player)
{
	Armor::onRepairKit(%player);
}

function armorTerm::onMine(%player)
{
	Client::sendMessage(Player::getClient(%player),1, "Heavy Mine deployed.");
	if(%player.throwTime < getSimTime() )
	{
		%obj = newObject("","Mine","HeavyMine");
		%armor = Player::getArmor(%player);
		%client = Player::getClient(%player);
		GameBase::setTeam (%obj,GameBase::getTeam (%client)); 
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj, %player, 5,false); //* %clientid.throwStrength
		%player.throwTime = getSimTime() + 0.5;
		Player::decItemCount(%player,%item);
	}
}