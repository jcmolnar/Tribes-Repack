//ASSAULT MARINE
// by Edgecrusher
$InvList[iarmorAssault] = 1;
$RemoteInvList[iarmorAssault] = 1;
$ArmorType[Male, iarmorAssault] = armormAssault;
$ArmorType[Female, iarmorAssault] = armorfAssault;
$ArmorName[armormAssault] = iarmorAssault;
$ArmorName[armorfAssault] = iarmorAssault;

$Needs[iarmorAssault] = AssaultPack;

ItemData iarmorAssault
{
	heading = $InvHead[ihMar];
	description = "Assault Marine";
	className = "Armor";
	price = 25;
};

PlayerData armormAssault
{
	className = "Armor";
	shapeFile = "marine";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	flameShapeName = "lflame";
	shieldShapeName = "shield";
	shadowDetailMask = 1;
	visibleToSensor = false;
	mapFilter = 1;
	mapIcon = "M_player";
	canCrouch = true;
	maxJetSideForceFactor = 0.8;
	maxJetForwardVelocity = 22;
	minJetEnergy = 1;
	jetForce = 340;
	jetEnergyDrain = 1.3;
	maxDamage = 1.0;
	maxForwardSpeed = 10;
	maxBackwardSpeed = 8;
	maxSideSpeed = 8;
	groundForce = 40 * 9.0;
	mass = 13.0;
	groundTraction = 3.0;
	maxEnergy = 70;
	drag = 1.0;
	density = 1.2;
	minDamageSpeed = 25;
	damageScale = 0.005;
	jumpImpulse = 150;
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
	animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };
	animData[50] = { "wave", none, 1, true, false, false, true, 1 };
	jetSound = SoundJetLight;
	rFootSounds = { SoundLFootRSoft, SoundLFootRHard, SoundLFootRSoft, SoundLFootRHard, SoundLFootRSoft, SoundLFootRSoft, SoundLFootRSoft, SoundLFootRHard, SoundLFootRSnow, SoundLFootRSoft, SoundLFootRSoft, SoundLFootRSoft, SoundLFootRSoft, SoundLFootRSoft, SoundLFootRSoft };
	lFootSounds = { SoundLFootLSoft, SoundLFootLHard, SoundLFootLSoft, SoundLFootLHard, SoundLFootLSoft, SoundLFootLSoft, SoundLFootLSoft, SoundLFootLHard, SoundLFootLSnow, SoundLFootLSoft, SoundLFootLSoft, SoundLFootLSoft, SoundLFootLSoft, SoundLFootLSoft, SoundLFootLSoft };
	footPrints = { 0, 1 };
	boxWidth = 0.5;
	boxDepth = 0.5;
	boxNormalHeight = 2.2;
	boxCrouchHeight = 1.8;
	boxNormalHeadPercentage = 0.83;
	boxNormalTorsoPercentage = 0.53;
	boxCrouchHeadPercentage = 0.6666;
	boxCrouchTorsoPercentage = 0.3333;
	boxHeadLeftPercentage = 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage = 0;
	boxHeadFrontPercentage = 1;
};

function armormAssault::onPlayerContact(%targetPlayer, %sourcePlayer)
{
	Armor::onPlayerContact(%targetPlayer, %sourcePlayer);
}

function armormAssault::onGrenade(%player)
{
	%obj = newObject("","Mine","Plasgren");
	Armor::ThrowGrenade(%player, %obj);
}

function armormAssault::onBeacon(%player, %item)
{
	Client::sendMessage(Player::getClient(%player),1, "Melta Bomb will detonate in 12 seconds");
	%obj = newObject("","Mine","Nukebomb");
	Armor::ThrowGrenade(%player, %obj);
Player::decItemCount(%player,%item);
}

function armormAssault::onRepairKit(%player)
{
	Armor::onRepairKit(%player);
}

function armormAssault::onMine(%player)
{
	Client::sendMessage(Player::getClient(%player),1, "Mine deployed.");
	if(%player.throwTime < getSimTime() )
	{
		%obj = newObject("","Mine","AntipersonelMine");
		%armor = Player::getArmor(%player);
		%client = Player::getClient(%player);
		GameBase::setTeam (%obj,GameBase::getTeam (%client)); 
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj, %player, 5,false); //* %clientid.throwStrength
		%player.throwTime = getSimTime() + 0.5;
		Player::decItemCount(%player,%item);
	}
}
PlayerData armorfAssault
{
	className = "Armor";
	shapeFile = "marine";
	flameShapeName = "lflame";
	shieldShapeName = "shield";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	shadowDetailMask = 1;
	visibleToSensor = false;
	mapFilter = 1;
	mapIcon = "M_player";
	canCrouch = true;
	maxJetSideForceFactor = 0.8;
	maxJetForwardVelocity = 22;
	minJetEnergy = 1;
	jetForce = 340;
	jetEnergyDrain = 1.3;
	maxDamage = 1.0;
	maxForwardSpeed = 10;
	maxBackwardSpeed = 8;
	maxSideSpeed = 8;
	groundForce = 40 * 9.0;
	mass = 13.0;
	groundTraction = 3.0;
	maxEnergy = 70;
	drag = 1.0;
	density = 1.2;
	minDamageSpeed = 25;
	damageScale = 0.005;
	jumpImpulse = 150;
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
	animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
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
	jetSound = SoundJetLight;
	rFootSounds = { SoundLFootRSoft, SoundLFootRHard, SoundLFootRSoft, SoundLFootRHard, SoundLFootRSoft, SoundLFootRSoft, SoundLFootRSoft, SoundLFootRHard, SoundLFootRSnow, SoundLFootRSoft, SoundLFootRSoft, SoundLFootRSoft, SoundLFootRSoft, SoundLFootRSoft, SoundLFootRSoft };
	lFootSounds = {SoundLFootLSoft, SoundLFootLHard, SoundLFootLSoft, SoundLFootLHard, SoundLFootLSoft, SoundLFootLSoft, SoundLFootLSoft, SoundLFootLHard, SoundLFootLSnow, SoundLFootLSoft, SoundLFootLSoft, SoundLFootLSoft, SoundLFootLSoft, SoundLFootLSoft, SoundLFootLSoft };
	footPrints = {0, 1 };
	boxWidth = 0.5;
	boxDepth = 0.5;
	boxNormalHeight = 2.2;
	boxCrouchHeight = 1.8;
	boxNormalHeadPercentage = 0.85;
	boxNormalTorsoPercentage = 0.53;
	boxCrouchHeadPercentage = 0.88;
	boxCrouchTorsoPercentage = 0.35;
	boxHeadLeftPercentage = 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage = 0;
	boxHeadFrontPercentage = 1;
};

function armorfAssault::onPlayerContact(%targetPlayer, %sourcePlayer)
{
	Armor::onPlayerContact(%targetPlayer, %sourcePlayer);
}

function armorfAssault::onGrenade(%player)
{
	%obj = newObject("","Mine","Plasgren");
	Armor::ThrowGrenade(%player, %obj);
}

function armorfAssault::onBeacon(%player, %item)
{
	Client::sendMessage(Player::getClient(%player),1, "Melta Bomb will detonate in 12 seconds");
	%obj = newObject("","Mine","Nukebomb");
	Armor::ThrowGrenade(%player, %obj);
Player::decItemCount(%player,%item);
}

function armorfAssault::onRepairKit(%player)
{
	Armor::onRepairKit(%player);
}

function armorfAssault::onMine(%player)
{
	Client::sendMessage(Player::getClient(%player),1, "Mine deployed.");
	if(%player.throwTime < getSimTime() )
	{
		%obj = newObject("","Mine","AntipersonelMine");
		%armor = Player::getArmor(%player);
		%client = Player::getClient(%player);
		GameBase::setTeam (%obj,GameBase::getTeam (%client)); 
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj, %player, 5,false); //* %clientid.throwStrength
		%player.throwTime = getSimTime() + 0.5;
		Player::decItemCount(%player,%item);
	}
}