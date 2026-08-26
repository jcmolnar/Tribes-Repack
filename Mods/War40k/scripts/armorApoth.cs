//Apothecary
// by Edgecrusher

$InvList[iarmorApoth] = 1;
$RemoteInvList[iarmorApoth] = 1;
$ArmorType[Male, iarmorApoth] = armormApoth;
$ArmorType[Female, iarmorApoth] = armorfApoth;
$ArmorName[armormApoth] = iarmorApoth;
$ArmorName[armorfApoth] = iarmorApoth;

ItemData iarmorApoth
{
	description = "Apothecary";
	heading = $InvHead[ihMar];
	className = "Armor";
	price = 20;
};

PlayerData armormApoth
{
	className = "Armor";
	shapeFile = "marine";
	flameShapeName = "mflame";
	shieldShapeName = "shield";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	shadowDetailMask = 1;
	canCrouch = false;
	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
	maxJetSideForceFactor = 1.1;
	maxJetForwardVelocity = 1;
	minJetEnergy = 1;
	jetForce = 4;
	jetEnergyDrain = 0.0;
	maxDamage = 1.7;
	maxForwardSpeed = 10.0;
	maxBackwardSpeed = 8.0;
	maxSideSpeed = 8.0;
	groundForce = 35 * 13.0;
	mass = 13.0;
	groundTraction = 3.0;
	maxEnergy = 90;
	drag = 1.0;
	density = 1.5;
	minDamageSpeed = 25;
	damageScale = 0.005;
	jumpImpulse = 110;
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
	jetSound = SoundJetLight;
	rFootSounds = { SoundMFootRSoft, SoundMFootRHard, SoundMFootRSoft, SoundMFootRHard, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRHard, SoundMFootRSnow, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft };
	lFootSounds = { SoundMFootLSoft, SoundMFootLHard, SoundMFootLSoft, SoundMFootLHard, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLHard, SoundMFootLSnow, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft };
	footPrints = { 2, 3 };
	boxWidth = 0.7;
	boxDepth = 0.7;
	boxNormalHeight = 2.4;
	boxNormalHeadPercentage = 0.83;
	boxNormalTorsoPercentage = 0.49;
	boxHeadLeftPercentage = 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage = 0;
	boxHeadFrontPercentage = 1;
};

function armormApoth::onPlayerContact(%targetPlayer, %sourcePlayer)
{
	Repair2(%targetPlayer, %sourcePlayer);
}

function armormApoth::onGrenade(%player)
{
	%obj = newObject("","Mine","Handgrenade");
	Armor::ThrowGrenade(%player, %obj);
}

function armormApoth::onBeacon(%player, %item)
{
	Client::sendMessage(Player::getClient(%player),1, "Nanobomb thrown!.");
	if(%player.throwTime < getSimTime() )
	{
		%obj = newObject("","Mine","Nanobomb");
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj, %player, 5,false); //* %clientid.throwStrength
		%player.throwTime = getSimTime() + 0.5;
		Player::decItemCount(%player,%item);
	}
}

function armormApoth::onRepairKit(%player)
{
	Armor::onRepairKit(%player);
}

function armormApoth::onMine(%player)
{
	Client::sendMessage(Player::getClient(%player),1, "Krak Mine deployed.");
	if(%player.throwTime < getSimTime() )
	{
		%obj = newObject("","Mine","KrakMine");
		%armor = Player::getArmor(%player);
		%client = Player::getClient(%player);
		GameBase::setTeam (%obj,GameBase::getTeam (%client)); 
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj, %player, 5,false); //* %clientid.throwStrength
		%player.throwTime = getSimTime() + 0.5;
		Player::decItemCount(%player,%item);
	}
}

PlayerData armorfApoth
{
	className = "Armor";
	shapeFile = "marine";
	flameShapeName = "mflame";
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
	canCrouch = false;
	maxDamage = 1.7;
	maxForwardSpeed = 10.0;
	maxBackwardSpeed = 8.0;
	maxSideSpeed = 8.0;
	groundForce = 35 * 13.0;
	mass = 13.0;
	groundTraction = 3.0;
	maxEnergy = 90;
	mass = 13.0;
	drag = 1.0;
	density = 1.5;
	minDamageSpeed = 25;
	damageScale = 0.005;
	jumpImpulse = 110;
	jumpSurfaceMinDot = 0.2;
	animData[0] = { "root", none, 1, true, true, true, false, 0 };
	animData[1] = { "run", none, 1, true, false, true, false, 3 };
	animData[2] = { "runback", none, 1, true, false, true, false, 3 };
	animData[3] = { "side left", none, 1, true, false, true, false, 3 };
	animData[4] = { "side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
	animData[7] = { "crouch root", none, 1, true, false, true, false, 3 };
	animData[8] = { "crouch root", none, 1, true, false, true, false, 3 };
	animData[9] = { "crouch root", none, -1, true, false, true, false, 3 };
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
	rFootSounds = { SoundMFootRSoft, SoundMFootRHard, SoundMFootRSoft, SoundMFootRHard, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRHard, SoundMFootRSnow, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft };
	lFootSounds = { SoundMFootLSoft, SoundMFootLHard, SoundMFootLSoft, SoundMFootLHard, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLHard, SoundMFootLSnow, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft };
	footPrints = { 2, 3 };
	boxWidth = 0.7;
	boxDepth = 0.7;
	boxNormalHeight = 2.4;
	boxNormalHeadPercentage = 0.84;
	boxNormalTorsoPercentage = 0.55;
	boxHeadLeftPercentage = 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage = 0;
	boxHeadFrontPercentage = 1;
};

function armorfApoth::onPlayerContact(%targetPlayer, %sourcePlayer)
{
	Repair2(%targetPlayer, %sourcePlayer);
}

function armorfApoth::onGrenade(%player)
{
	%obj = newObject("","Mine","Handgrenade");
	Armor::ThrowGrenade(%player, %obj);
}

function armorfApoth::onBeacon(%player, %item)
{
	Client::sendMessage(Player::getClient(%player),1, "Nanobomb thrown.");
	 if(%player.throwTime < getSimTime() )
	{
		%obj = newObject("","Mine","Nanobomb");
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj, %player, 5,false); //* %clientid.throwStrength
		%player.throwTime = getSimTime() + 0.5;
		Player::decItemCount(%player,%item);
	}
}

function armorfApoth::onRepairKit(%player)
{
	Armor::onRepairKit(%player);
}

function armorfApoth::onMine(%player)
{
	Client::sendMessage(Player::getClient(%player),1, "Krak Mine deployed.");
	if(%player.throwTime < getSimTime() )
	{
		%obj = newObject("","Mine","KrakMine");
		%armor = Player::getArmor(%player);
		%client = Player::getClient(%player);
		GameBase::setTeam (%obj,GameBase::getTeam (%client)); 
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj, %player, 5,false); //* %clientid.throwStrength
		%player.throwTime = getSimTime() + 0.5;
		Player::decItemCount(%player,%item);
	}
}