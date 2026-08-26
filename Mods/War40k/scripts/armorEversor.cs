// Eversor
// By Edgecrusher
// Altered and fixed by C|one (death explosion, crouch-walking, traction)
$InvList[iarmorEversor] = 1;
$RemoteInvList[iarmorEversor] = 1;
$ArmorType[Male, iarmorEversor] = armormEversor;
$ArmorType[Female, iarmorEversor] = armorfEversor;
$ArmorName[armormEversor] = iarmorEversor;
$ArmorName[armorfEversor] = iarmorEversor;

ItemData iarmorEversor //armormEversor
{
	heading = $InvHead[ihMar];
	description = "Eversor Assassin";
	className = "Armor";
	price = 55;
};

PlayerData armormEversor
{
	className = "Armor";
	shapeFile = "larmor";
	flameShapeName = "force";
	shieldShapeName = "shield";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	shadowDetailMask = 0;
	canCrouch = true;
	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_generator";
	maxJetSideForceFactor = 1.1;
	maxJetForwardVelocity = 1;
	minJetEnergy = 30;
	jetForce = 4;
	jetEnergyDrain = 0;
	maxDamage = 0.45;
	maxForwardSpeed = 18.0;
	maxBackwardSpeed = 18.0;
	maxSideSpeed = 18.0;
	groundForce = 35 * 13.0;
	mass = 13.0;
	groundTraction = 500.0;
	maxEnergy = 30;
	drag = 1.0;
	density = 1.5;
	minDamageSpeed = 25;
	damageScale = 0.005;
	jumpImpulse = 240;
	jumpSurfaceMinDot = 0.2;
	animData[0] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[1] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[2] = { "crouch forward", none, -1, true, false, true, false, 3 };
	animData[3] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[4] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
	animData[7] = { "root", none, 1, true, true, true, false, 0 };
	animData[8] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[10] = { "run", none, 1, true, false, true, false, 3 };
	animData[11] = { "runback", none, 1, true, false, true, false, 3 };
	animData[12] = { "side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "side left", none, -1, true, false, true, false, 3 };
	animData[14] = { "fall", none, 1, true, true, true, false, 3 };
	animData[15] = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16] = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17] = { "tumble loop", none, 1, true, false, false, false, 3 };
	animData[18] = { "tumble end", none, 1, true, false, false, false, 3 };
	animData[19] = { "root", none, 1, true, true, true, false, 3 };
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
	jetSound = SoundJetLight;
	rFootSounds = { SoundMFootRSoft, SoundMFootRHard, SoundMFootRSoft, SoundMFootRHard, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRHard, SoundMFootRSnow, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft };
	lFootSounds = { SoundMFootLSoft, SoundMFootLHard, SoundMFootLSoft, SoundMFootLHard, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLHard, SoundMFootLSnow, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft };
	footPrints = { 2, 3 };
	boxWidth = 0.5;
	boxDepth = 0.5;
	boxNormalHeight = 1.8;
	boxCrouchHeight = 2.3;
	boxNormalHeadPercentage= 0.6666;
	boxNormalTorsoPercentage = 0.3333;
	boxCrouchHeadPercentage= 0.83;
	boxCrouchTorsoPercentage = 0.53;
	boxHeadLeftPercentage= 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage= 0;
	boxHeadFrontPercentage = 1;
};

function armormEversor::onPlayerContact(%targetPlayer, %sourcePlayer)
{
	Drain(%targetPlayer, %sourcePlayer);
}

function armormEversor::onGrenade(%player)
{
	%obj = newObject("","Mine","Tranqgrenade");
	Armor::ThrowGrenade(%player, %obj);
}

function armormEversor::onBeacon(%player, %item)
{
	Armor::SpeedBooster(%player, %item, 500);
}

function armormEversor::onRepairKit(%player)
{
	Armor::onRepairKit(%player);
}

function armormEversor::onMine(%player)
{
	Client::sendMessage(Player::getClient(%player),1, "Viral Mine deployed.");
	if(%player.throwTime < getSimTime() )
	{
		%obj = newObject("","Mine","ViralMine");
		%armor = Player::getArmor(%player);
		%client = Player::getClient(%player);
		GameBase::setTeam (%obj,GameBase::getTeam (%client)); 
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj, %player, 5,false); //* %clientid.throwStrength
		%player.throwTime = getSimTime() + 0.5;
		Player::decItemCount(%player,%item);
	}
}

function armormEversor::onKilled(%this) 
{
	Player::onKilled(%this);
	%obj = newObject("","Mine","Everboom");
	Armor::ThrowGrenade(%this, %obj);
}

PlayerData armorfEversor
{
	className = "Armor";
	shapeFile = "lfemale";
	flameShapeName = "force";
	shieldShapeName = "shield";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	shadowDetailMask = 0;
	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_generator";
	maxJetSideForceFactor = 1.1;
	maxJetForwardVelocity = 1;
	minJetEnergy = 1;
	jetForce = 4;
	jetEnergyDrain = 0.0;
	maxDamage = 0.45;
	maxForwardSpeed = 18.0;
	maxBackwardSpeed = 18.0;
	maxSideSpeed = 18.0;
	groundForce = 35 * 13.0;
	mass = 13.0;
	groundTraction = 500.0;
	maxEnergy = 30;
	drag = 1.0;
	density = 1.5;
	minDamageSpeed = 25;
	damageScale = 0.005;
	jumpImpulse = 240;
	jumpSurfaceMinDot = 0.2;
	animData[0] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[1] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[2] = { "crouch forward", none, -1, true, false, true, false, 3 };
	animData[3] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[4] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
	animData[7] = { "root", none, 1, true, true, true, false, 0 };
	animData[8] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[10] = { "run", none, 1, true, false, true, false, 3 };
	animData[11] = { "runback", none, 1, true, false, true, false, 3 };
	animData[12] = { "side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "side left", none, -1, true, false, true, false, 3 };
	animData[14] = { "fall", none, 1, true, true, true, false, 3 };
	animData[15] = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16] = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17] = { "tumble loop", none, 1, true, false, false, false, 3 };
	animData[18] = { "tumble end", none, 1, true, false, false, false, 3 };
	animData[19] = { "root", none, 1, true, true, true, false, 3 };
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
	jetSound = SoundJetLight;
	rFootSounds = { SoundMFootRSoft, SoundMFootRHard, SoundMFootRSoft, SoundMFootRHard, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRHard, SoundMFootRSnow, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft };
	lFootSounds = { SoundMFootLSoft, SoundMFootLHard, SoundMFootLSoft, SoundMFootLHard, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLHard, SoundMFootLSnow, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft };
	footPrints = { 2, 3 };
	boxWidth = 0.5;
	boxDepth = 0.5;
	boxNormalHeight = 1.8;
	boxCrouchHeight = 2.3;
	boxNormalHeadPercentage= 0.6666;
	boxNormalTorsoPercentage = 0.3333;
	boxCrouchHeadPercentage= 0.83;
	boxCrouchTorsoPercentage = 0.53;
	boxHeadLeftPercentage= 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage= 0;
	boxHeadFrontPercentage = 1;
};

function armorfEversor::onPlayerContact(%targetPlayer, %sourcePlayer)
{
	Drain(%targetPlayer, %sourcePlayer);
}

function armorfEversor::onGrenade(%player)
{
	%obj = newObject("","Mine","Tranqgrenade");
	Armor::ThrowGrenade(%player, %obj);
}

function armorfEversor::onBeacon(%player, %item)
{
	Armor::SpeedBooster(%player, %item, 500);
}

function armorfEversor::onRepairKit(%player)
{
	Armor::onRepairKit(%player);
}

function armorfEversor::onMine(%player)
{
	Client::sendMessage(Player::getClient(%player),1, "Viral Mine deployed.");
	if(%player.throwTime < getSimTime() )
	{
		%obj = newObject("","Mine","ViralMine");
		%armor = Player::getArmor(%player);
		%client = Player::getClient(%player);
		GameBase::setTeam (%obj,GameBase::getTeam (%client)); 
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj, %player, 5,false); //* %clientid.throwStrength
		%player.throwTime = getSimTime() + 0.5;
		Player::decItemCount(%player,%item);
	}
}

function armorfEversor::onKilled(%this) 
{
	Player::onKilled(%this);
	%obj = newObject("","Mine","Everboom");
	Armor::ThrowGrenade(%this, %obj);
}