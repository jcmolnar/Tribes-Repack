$InvList[iarmorSDaemon] = 1;
$RemoteInvList[iarmorSDaemon] = 0;
$ArmorName[armorSDaemon] = iarmorSDaemon;
$ArmorType[Male, iarmorSDaemon] = armorSDaemon;
$ArmorType[Female, iarmorSDaemon] = armorSDaemon;

ItemData iarmorSDaemon
{
	heading = $InvHead[ihEld];
	description = "Storm Daemon";
	className = "Armor";
	price = 42;
};

PlayerData armorSDaemon
{
	className = "Armor";
	shapeFile = "sdaemon";
	flameShapeName = "shield_large";
	shieldShapeName = "shield_large";
	damageSkinData = "objectDamageSkins";
	debrisId = defaultDebrisLarge;
	shadowDetailMask = 0;
	canCrouch = false;
	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
	maxJetSideForceFactor = 1.0;
	maxJetForwardVelocity = 10;
	minJetEnergy = 0.2;
	jetForce = 470;
	jetEnergyDrain = 1.0;
	maxDamage = 3.0;
	maxForwardSpeed = 6.8;
	maxBackwardSpeed = 5.5;
	maxSideSpeed = 5.5;
	groundForce = 80 * 20.0;
	mass = 20.0;
	groundTraction = 100.0;
	maxEnergy = 180;
	drag = 1.0;
	density = 5.0;
	minDamageSpeed = 325;
	damageScale = 0.005;
	jumpImpulse = 175;
	jumpSurfaceMinDot = 0.4;
	animData[0]= { "root", SoundElevatorStop, 1, true, true, true, false, 0 };
	animData[1]= { "run", none, 1, true, false, true, false, 3 };
	animData[2]= { "runback", none, 1, true, false, true, false, 3 };
	animData[3]= { "side left", none, 1, true, false, true, false, 3 };
	animData[4]= { "side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", SoundActivateMotionSensor, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", SoundActivatePDA, 1, true, false, true, false, 3 };
	animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
	animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[14]= { "fall", SoundBeaconUse, 1, true, true, true, false, 3 };
	animData[15]= { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16]= { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17]= { "tumble loop", SoundElevatorStop, 1, true, false, false, false, 3 };
	animData[18]= { "tumble end", SoundElevatorStop, 1, true, false, false, false, 3 };
	animData[19] = { "PDA access", SoundActivateMotionSensor, 1, true, true, true, false, 3 };
	animData[20] = { "PDA access", SoundElevatorStop, 1, true, false, false, false, 3 };
	animData[21] = { "throw", none, 1, true, false, false, false, 3 };
	animData[22] = { "flyer root", SoundElevatorStop, 1, false, false, false, false, 3 };
	animData[23] = { "apc root", SoundElevatorStop, 1, true, true, true, false, 3 };
	animData[24] = { "apc pilot", SoundElevatorStop, 1, false, false, false, false, 3 };
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
	animData[38] = { "sign over here",SoundElevatorStop, 1, true, false, false, false, 2 };
	animData[39] = { "sign point", SoundElevatorStop, 1, true, false, false, true, 1 };
	animData[40] = { "sign retreat",SoundElevatorStop, 1, true, false, false, false, 2 };
	animData[41] = { "sign stop", SoundElevatorStop, 1, true, false, false, true, 1 };
	animData[42] = { "sign salut", SoundElevatorStop, 1, true, false, false, true, 1 }; 
	animData[43] = { "celebration 1", SoundActivatePDA, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", SoundActivatePDA, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", SoundActivatePDA, 1, true, false, false, false, 2 };
	animData[46] = { "taunt 1", SoundActivatePDA, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", SoundActivatePDA, 1, true, false, false, false, 2 };
	animData[48] = { "pose kneel", SoundActivatePDA, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", SoundActivatePDA, 1, true, false, false, true, 1 };
	animData[50] = { "wave", SoundActivatePDA, 1, true, false, false, true, 1 };
	jetSound = SoundLaserIdle;
	rFootSounds = { SoundHFootRHard, SoundHFootRHard, SoundHFootRHard, SoundHFootRHard, SoundHFootRHard, SoundHFootRHard, SoundHFootRHard, SoundHFootRHard, SoundHFootRHard, SoundHFootRHard, SoundHFootRHard, SoundHFootRHard, SoundHFootRHard, SoundHFootRHard, SoundHFootRHard};
	lFootSounds = { SoundHFootLHard, SoundHFootLHard, SoundHFootLHard, SoundHFootLHard, SoundHFootLHard, SoundHFootLHard, SoundHFootLHard, SoundHFootLHard, SoundHFootLHard, SoundHFootLHard, SoundHFootLHard, SoundHFootLHard, SoundHFootLHard, SoundHFootLHard, SoundHFootLHard};
	footPrints = { 2, 3 };
	boxWidth = 0.9;
	boxDepth = 0.9;
	boxNormalHeight = 2.5;
	boxNormalHeadPercentage = 0;
	boxNormalTorsoPercentage = 1;
	boxHeadLeftPercentage = 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage = 0;
	boxHeadFrontPercentage = 1;
};

function armorSDaemon::onPlayerContact(%targetPlayer, %sourcePlayer)
{
	Armor::onPlayerContact(%targetPlayer, %sourcePlayer);
}

function armorSDaemon::onGrenade(%player)
{
	%obj = newObject("","Mine","Mortarbomb");
	Armor::ThrowGrenade(%player, %obj);
}

function armorSDaemon::onBeacon(%player, %item)
{
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Projectile::spawnProjectile("FusionBlast",%trans,%player,%vel);
	Player::decItemCount(%player,%item);
}

function armorSDaemon::onRepairKit(%player)
{
	Armor::onRepairKit(%player);
}

function armorSDaemon::onMine(%player)
{
	Client::sendMessage(Player::getClient(%player),1, "Haywire Mine deployed.");
	if(%player.throwTime < getSimTime() )
	{
		%obj = newObject("","Mine","EMPMine");
		%armor = Player::getArmor(%player);
		%client = Player::getClient(%player);
		GameBase::setTeam (%obj,GameBase::getTeam (%client)); 
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj, %player, 5,false); //* %clientid.throwStrength
		%player.throwTime = getSimTime() + 0.5;
		Player::decItemCount(%player,%item);
	}
}

function armorSDaemon::onKilled(%this) 
{
	Player::onKilled(%this);
	%obj = newObject("","Mine","SDBoom");
	Armor::ThrowGrenade(%this, %obj);
}

RocketData FusionBlast 
{
	bulletShapeName = "fusionbolt.dts";
	explosionTag = turretExp;
	collisionRadius = 0.0;
	mass = 2.0;
	damageClass = 1;
	damageValue = 0.45;
	damageType = $DeathDamageType;
	explosionRadius = 20;
	kickBackStrength = 0.0;
	muzzleVelocity = 200.0;
	terminalVelocity = 200.0;
	acceleration = 5.0;
	totalTime = 1.3;
	liveTime = 1.3;
	lightRange = 5.0;
	lightColor = { 0.0, 0.2, 1.5 };
	inheritedVelocityScale = 0.5;
	trailType = 1;
	trailLength = 125;
	trailWidth = 0.9;
};