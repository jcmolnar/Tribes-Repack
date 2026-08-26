# Damage Types#
echo("Loading Damage Types...");
$ImpactDamageType = -1;
$LandingDamageType = 0;
$BulletDamageType = 1;
$EnergyDamageType = 2;
$PlasmaDamageType = 3;
$ExplosionDamageType = 4;
$ShrapnelDamageType = 5;
$LaserDamageType = 6;
$MortarDamageType = 7;
$BlasterDamageType = 8;
$ElectricityDamageType = 9;
$CrushDamageType = 10;
$DebrisDamageType = 11;
$MissileDamageType = 12;
$MineDamageType = 13;
$SniperDamageType = 14;
$FlashDamageType = 15;
$ShellDamageType = 16;
$MeltaDamageType = 17;
$DDamageType = 18;
$ReaperDamageType = 19;
$FlamerDamageType = 20;
$ShurikenDamageType = 21;
$DeathDamageType = 22;
$PsiDamageType = 23;
$ChemDamageType = 24;
$KrakenDamageType = 25;
$AcidDamageType = 26;
$WebDamageType = 27;

BulletData MiniFusionBolt
{
	bulletShapeName = "enbolt.dts";
	explosionTag = energyExp;
	damageClass = 0;
	damageValue = 0.1;
	damageType = $DeathDamageType;
	muzzleVelocity = 80.0;
	totalTime = 4.0;
	liveTime = 2.0;
	lightRange = 3.0;
	lightColor = { 0.25, 0.25, 1.0 };
	inheritedVelocityScale = 0.5;
	isVisible = True;
	rotationPeriod = 1;
};

RocketData IonBolt 
{
	bulletShapeName = "enbolt.dts";
	explosionTag = turretExp;
	collisionRadius = 0.0;
	mass = 2.0;
	damageClass = 1;
	damageValue = 0.15;
	damageType = $DeathDamageType;
	explosionRadius = 6;
	kickBackStrength = 0.0;
	muzzleVelocity = 200.0;
	terminalVelocity = 200.0;
	acceleration = 5.0;
	totalTime = 0.42;
	liveTime = 0.42;
	lightRange = 5.0;
	lightColor = { 1.0, 0.7, 0.5 };
	inheritedVelocityScale = 0.5;
	trailType = 1;
	trailLength = 50;
	trailWidth = 0.3;
	soundId = SoundJetHeavy;
};

SeekingMissileData TurretMissile 
{
	bulletShapeName = "rocket.dts";
	explosionTag = rocketExp;
	collisionRadius = 0.0;
	mass = 2.0;
	damageClass = 1;
	damageValue = 0.67;
	damageType = $MissileDamageType;
	explosionRadius = 15.0;
	kickBackStrength = 175.0;
	muzzleVelocity = 72.0;
	totalTime = 10;
	liveTime = 10;
	seekingTurningRadius = 15;
	nonSeekingTurningRadius = 75.0;
	proximityDist = 1.5;
	smokeDist = 1.75;
	lightRange = 5.0;
	lightColor = { 0.4, 0.4, 1.0 };
	inheritedVelocityScale = 0.5;
	soundId = SoundJetHeavy;
};

function SeekingMissile::updateTargetPercentage(%target) 
{
	return GameBase::virtual(%target, "getHeatFactor");
}

 RepairEffectData Apoth 
{
	bitmapName = "discglow1.bmp";
	boltLength = 100.0;
	segmentDivisions = 4;
	beamWidth = 0.125;
	updateTime = 450;
	skipPercent = 0.6;
	displaceBias = 0.15;
	lightRange = 3.0;
	lightColor = { 0.85, 0.25, 0.25 };
};

function Apoth::onAcquire(%this, %player, %target) 
{
	%client = Player::getClient(%player);
	if (%target == %player) 
	{
		%player.repairTarget = -1;
		if (GameBase::getDamageLevel(%player) != 0) 
		{
			%player.repairRate = 0.3;
			%player.repairTarget = %player;
			Client::sendMessage(%client, 0, "Healing Unit");
		}
		else 
		{
			Client::sendMessage(%client,0,"Nothing in range");
			Player::trigger(%player, $WeaponSlot, false);
			return;
		}
	}
	else 
	{
		%player.repairTarget = %target;
		if (getObjectType(%player.repairTarget) == "Player") 
		{
			%player.repairRate = 0.3;
			%rclient = Player::getClient(%player.repairTarget);
			%name = Client::getName(%rclient);
		}
		else 
		{
			Client::sendMessage(%client,0,"Med Gun does not heal objects.");
			%player.repairRate = 0.0;
			Player::trigger(%player,$WeaponSlot,false);
			return; 
		}
		if (GameBase::getDamageLevel(%player.repairTarget) == 0) 
		{
			Client::sendMessage(%client,0,%name @ " is not damaged");
			Player::trigger(%player,$WeaponSlot,false);
			%player.repairTarget = -1;
			return;
		}
		if (getObjectType(%player.repairTarget) == "Player") 
		{
			Client::sendMessage(%rclient,0,"Being healed by " @ Client::getName(%client));
		}
		Client::sendMessage(%client,0,"Healing " @ %name);
	}
	%rate = GameBase::getAutoRepairRate(%player.repairTarget) + %player.repairRate;
	GameBase::setAutoRepairRate(%player.repairTarget,%rate);
}

function Apoth::onRelease(%this, %player) 
{
	%object = %player.repairTarget;
	if (%object != -1) 
	{
		%client = Player::getClient(%player);
		if (%object == %player) 
		{
			Client::sendMessage(%client,0,"Stopped Healing");
		}
		else 
		{
			if (GameBase::getDamageLevel(%object) == 0) 
			{
				Client::sendMessage(%client,0,"Repair Done");
				%type = Player::getArmor(%object);
				%fixpoints = (floor(%type.maxdamage - (%type.maxdamage - %object.mindamage)));
				if(%fixpoints < 1) %fixpoints = 1;
				%object.mindamage = 0;
				%playerClient = GameBase::getControlClient(%object.lastDamageObject);
				if(%client != %playerClient)
				{
					if(GameBase::getTeam(%object) == GameBase::getTeam(%client))
					{
						%client.score = %client.score + %fixpoints;
						bottomprint(%client, "<f0>Score:<f1> +" @ %fixpoints);
						Game::refreshClientScore(%client);
					}
				}
				else
				{
					bottomprint(%client, "<f0>Score:<f1> +0. You were the last person to damage.");
				}
			}
			else 
			{
				Client::sendMessage(%client,0,"Repair Stopped");
			}
		}
		%rate = GameBase::getAutoRepairRate(%object) - %player.repairRate;
		if (%rate < 0) %rate = 0;
		GameBase::setAutoRepairRate(%object,%rate);
	}
}

function Apoth::checkDone(%this, %player) 
{
	if (Player::isTriggered(%player,$WeaponSlot) && %player.repairTarget != -1) 
	{
		%object = %player.repairTarget;
		if (%object == %player) 
		{
			if (GameBase::getDamageLevel(%player) == 0) 
			{
				Player::trigger(%player,$WeaponSlot,false);
				return;
			}
		}
		else 
		{
			if (GameBase::getDamageLevel(%object) == 0) 
			{
				Player::trigger(%player,$WeaponSlot,false);
				return;
			}
		}
	}
}