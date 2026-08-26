function StaticShape::onPower(%this,%power,%generator)
{
	if (%power) GameBase::playSequence(%this,0,"power");
	else GameBase::stopSequence(%this,0);
}

function StaticShape::onEnabled(%this)
{
	if (GameBase::isPowered(%this)) GameBase::playSequence(%this,0,"power");
}

function StaticShape::onDisabled(%this)
{
	GameBase::stopSequence(%this,0);
}

function StaticShape::onDestroyed(%this)
{
echo(%item);
//	if(%this.className = "Turret")
//	{
//		%killpoints = (floor(%this.maxdamage * 3));
//		if(%killpoints < 1) %killpoints = 1;
//	}
//	else
//	{
		%killpoints = (floor(%this.maxdamage / 2));
		if(%killpoints < 1) %killpoints = 1;
//   	}
	%destroyerTeam = %this.lastDamageTeam;
	%thisTeam = GameBase::getTeam(%this);
      %playerClient = GameBase::getControlClient(%this.lastDamageObject);
      if(%playerClient != -1) %clientName = Client::getName(%playerClient);

   	if(%thisTeam != %destroyerTeam)
	{
		if(%playerClient != -1)
		{
			if (GameBase::getDataName(%this).mapFilter != -1)
			{
				%playerClient.score = %playerClient.score + %killpoints;
				bottomprint(%playerClient, "<f0>Score:<f1> +" @ %killpoints);
				Game::refreshClientScore(%playerClient);
			}
		}
	}
   	else
	{
		if(%playerClient != -1)
		{
			if (GameBase::getDataName(%this).mapFilter != -1)
			{
				%playerClient.score = %playerClient.score - %killpoints;
				bottomprint(%playerClient, "<f0>Score:<f1> -" @ %killpoints);
				Game::refreshClientScore(%playerClient);
			}
		}
	}

	GameBase::stopSequence(%this,0);
	StaticShape::objectiveDestroyed(%this);
	calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 2, 0.40, 0.1, 250, 100); 
}

function StaticShape::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	%damageLevel = GameBase::getDamageLevel(%this);
	%dValue = %damageLevel + %value;
	%this.lastDamageObject = %object;
	%this.lastDamageTeam = GameBase::getTeam(%object);
	if(GameBase::getTeam(%this) == GameBase::getTeam(%object)) 
	{
		%name = GameBase::getDataName(%this);
		if(%name.className == Generator || %name.className == Station) 
		{ 
			%TDS = $Server::TeamDamageScale;
			%dValue = %damageLevel + %value * %TDS;
			%disable = GameBase::getDisabledDamage(%this);
			if(!$Server::TourneyMode && %dValue > %disable - 0.05) 
			{
				if(%damageLevel > %disable - 0.05)
				return;
				else %dValue = %disable - 0.05;
			}
		}
	}
	GameBase::setDamageLevel(%this,%dValue);
	%damageLevel = GameBase::getDamageLevel(%this);
	%this.mindamage = %damageLevel;
}

function StaticShape::shieldDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	%damageLevel = GameBase::getDamageLevel(%this);
	%this.lastDamageObject = %object;
	%this.lastDamageTeam = GameBase::getTeam(%object);
	if (%this.shieldStrength) 
	{
		%energy = GameBase::getEnergy(%this);
		%strength = %this.shieldStrength;
//		if (%type == $BulletDamageType) %strength *= 1;
		if (%type == $EnergyDamageType) %strength *= 1.5;
		if (%type == $PlasmaDamageType) %strength *= 1.5;
//		if (%type == $ExplosionDamageType) %strength *= 1;
//		if (%type == $ShrapnelDamageType) %strength *= 1;
		if (%type == $LaserDamageType) %strength *= 1.5;
//		if (%type == $MortarDamageType) %strength *= 1;
		if (%type == $BlasterDamageType) %strength *= 0.5;
		if (%type == $ElectricityDamageType) %strength *= 0.5;
//		if (%type == $DebrisDamageType) %strength *= 1;
		if (%type == $MissileDamageType) %strength *= 0.75;
		if (%type == $MineDamageType) %strength *= 0.75;
		if (%type == $SniperDamageType) %strength *= 0.5;
		if (%type == $FlashDamageType) GameBase::setEnergy(%this,0);
//		if (%type == $ShellDamageType) %strength *= 1;
		if (%type == $MeltaDamageType) %strength *= 1.5;
		if (%type == $DDamageType) %strength *= 0.25;
		if (%type == $ReaperDamageType) %strength *= 0.75;
		if (%type == $FlamerDamageType) %strength *= 0.8;
//		if (%type == $ShurikenDamageType) %strength *= 1;
		if (%type == $DeathDamageType) %strength *= 0.4;
		if (%type == $PsiDamageType) %strength *= 0.25;
		if (%type == $ChemDamageType) %strength *= 1.5;
		if (%type == $KrakenDamageType) %strength *= 0.25;
		if (%type == $AcidDamageType) %strength *= 0.5;
                if (%type == $WebDamageType) %strength *= 0.01;
		%absorb = %energy * %strength;
		if (%value < %absorb) 
		{
			GameBase::setEnergy(%this, %energy - (%value / %strength));
			%centerPos = getBoxCenter(%this);
			%sphereVec = findPointOnSphere(getBoxCenter(%object),%centerPos,%vec,%this);
			%centerPosX = getWord(%centerPos,0);
			%centerPosY = getWord(%centerPos,1);
			%centerPosZ = getWord(%centerPos,2);
			%pointX = getWord(%pos,0);
			%pointY = getWord(%pos,1);
			%pointZ = getWord(%pos,2);
			%newVecX = %centerPosX - %pointX;
			%newVecY = %centerPosY - %pointY;
			%newVecZ = %centerPosZ - %pointZ;
			%norm = Vector::normalize(%newVecX @ " " @ %newVecY @ " " @ %newVecZ);
			%zOffset = 0;
			if(GameBase::getDataName(%this) == PulseSensor) %zOffset = (%pointZ-%centerPosZ) * 0.5;
			GameBase::activateShield(%this,%sphereVec,%zOffset);
		}
		else 
		{
			GameBase::setEnergy(%this,0);
			StaticShape::onDamage(%this,%type,%value - %absorb,%pos,%vec,%mom,%object);
		}
	}
	else 
	{
		if (%type == $FlashDamageType) GameBase::setEnergy(%this,0);
		StaticShape::onDamage(%this,%type,%value,%pos,%vec,%mom,%object);
	}
}

StaticShapeData FlagStand
{
	description = "Flag Stand";
	shapeFile = "flagstand";
	visibleToSensor = false;
};


function calcRadiusDamage(%this,%type,%radiusRatio,%damageRatio,%forceRatio,%rMax,%rMin,%dMax,%dMin,%fMax,%fMin) 
{
	%radius = GameBase::getRadius(%this);
	if(%radius) 
	{
		%radius *= %radiusRatio;
		%damageValue = %radius * %damageRatio;
		%force = %radius * %forceRatio;
		if(%radius > %rMax) %radius = %rMax;
		else if(%radius < %rMin) %radius = %rMin;
		if(%damageValue > %dMax) %damageValue = %dMax; 
		else if(%damageValue < %dMin) %damageValue = %dMin;
		if(%force > %fMax) %force = %fMax; 
		else if(%force < %fMin) %force = %fMin;
		GameBase::applyRadiusDamage(%type,getBoxCenter(%this), %radius, %damageValue,%force,%this);
	}
}

function FlagStand::onDamage() 
{ } 

function Generator::onEnabled(%this) 
{ 
	GameBase::setActive(%this,true); 
} 

function Generator::onDisabled(%this) 
{ 
	GameBase::stopSequence(%this,0); 
	GameBase::generatePower(%this, false); 
} 

function Generator::onDestroyed(%this) 
{ 
	Generator::onDisabled(%this); 
	StaticShape::objectiveDestroyed(%this); 
	calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 3, 0.55, 0.30, 250, 170); 
} 

function Generator::onActivate(%this) 
{ 
	GameBase::playSequence(%this,0,"power"); 
	GameBase::generatePower(%this, true); 
} 

function Generator::onDeactivate(%this) 
{ 
	GameBase::stopSequence(%this,0); 
	GameBase::generatePower(%this, false); 
} 

StaticShapeData TowerSwitch 
{ 
	description = "Tower Control Switch"; 
	className = "towerSwitch"; 
	shapeFile = "tower"; 
	showInventory = "false"; 
	visibleToSensor = true; 
	mapFilter = 4; 
	mapIcon = "M_generator"; 
}; 

StaticShapeData Generator 
{ 
	description = "Generator"; 
	shapeFile = "generator"; 
	className = "Generator"; 
	sfxAmbient = SoundGeneratorPower; 
	debrisId = flashDebrisLarge; 
	explosionId = flashExpLarge; 
	maxDamage = 2.0; 
	visibleToSensor = true; 
	mapFilter = 4; 
	mapIcon = "M_generator"; 
	damageSkinData = "objectDamageSkins"; 
	shadowDetailMask = 16; 
}; 

StaticShapeData SolarPanel 
{ 
	description = "Solar Panel"; 
	shapeFile = "solar_med"; 
	className = "Generator"; 
	debrisId = flashDebrisMedium; 
	maxDamage = 1.0; 
	visibleToSensor = true; 
	mapFilter = 4; 
	mapIcon = "M_generator"; 
	damageSkinData = "objectDamageSkins"; 
	shadowDetailMask = 16; 
	explosionId = flashExpLarge; 
}; 

StaticShapeData PortGenerator 
{ 
	description = "Portable Generator"; 
	shapeFile = "generator_p"; 
	className = "Generator"; 
	debrisId = flashDebrisSmall; 
	sfxAmbient = SoundGeneratorPower; 
	maxDamage = 1.6; 
	mapIcon = "M_generator"; 
	damageSkinData = "objectDamageSkins"; 
	shadowDetailMask = 16; 
	explosionId = flashExpMedium; 
	visibleToSensor = true; 
	mapFilter = 4; 
}; 

StaticShapeData SmallAntenna 
{ 
	shapeFile = "anten_small"; 
	debrisId = defaultDebrisSmall; 
	maxDamage = 1.0; 
	damageSkinData = "objectDamageSkins"; 
	shadowDetailMask = 16; 
	explosionId = flashExpMedium; 
	description = "Small Antenna"; 
}; 

StaticShapeData MediumAntenna 
{ 
	shapeFile = "anten_med"; 
	debrisId = flashDebrisSmall; 
	maxDamage = 1.5; 
	damageSkinData = "objectDamageSkins"; 
	shadowDetailMask = 16; 
	explosionId = flashExpMedium; 
	description = "Medium Antenna"; 
}; 

StaticShapeData LargeAntenna 
{ 
	shapeFile = "anten_lrg"; 
	debrisId = defaultDebrisSmall; 
	maxDamage = 1.5; 
	damageSkinData = "objectDamageSkins"; 
	shadowDetailMask = 16; 
	explosionId = debrisExpMedium; 
	description = "Large Antenna"; 
}; 

StaticShapeData ArrayAntenna 
{ 
	shapeFile = "anten_lava"; 
	debrisId = flashDebrisSmall; 
	maxDamage = 1.5; 
	damageSkinData = "objectDamageSkins"; 
	shadowDetailMask = 16; 
	explosionId = flashExpMedium; 
	description = "Array Antenna"; 
}; 

StaticShapeData RodAntenna 
{ 
	shapeFile = "anten_rod"; 
	debrisId = defaultDebrisSmall; 
	maxDamage = 1.5; 
	damageSkinData = "objectDamageSkins"; 
	shadowDetailMask = 16; 
	explosionId = debrisExpMedium; 
	description = "Rod Antenna"; 
}; 

StaticShapeData ForceBeacon 
{ 
	shapeFile = "force"; 
	debrisId = defaultDebrisSmall; 
	maxDamage = 0.5; 
	damageSkinData = "objectDamageSkins"; 
	shadowDetailMask = 16; 
	explosionId = debrisExpMedium; 
	description = "Force Beacon"; 
}; 

StaticShapeData CargoCrate 
{ 
	shapeFile = "magcargo"; 
	debrisId = flashDebrisSmall; 
	maxDamage = 1.0; 
	damageSkinData = "objectDamageSkins"; 
	shadowDetailMask = 16; 
	explosionId = flashExpMedium; 
	description = "Cargo Crate"; 
}; 

StaticShapeData CargoBarrel 
{ 
	shapeFile = "liqcyl"; 
	debrisId = defaultDebrisSmall; 
	maxDamage = 1.0; 
	damageSkinData = "objectDamageSkins"; 
	shadowDetailMask = 16; 
	explosionId = debrisExpMedium; 
	description = "Cargo Barrel"; 
}; 

StaticShapeData SquarePanel 
{ 
	shapeFile = "teleport_square"; 
	debrisId = flashDebrisSmall; 
	maxDamage = 0.3; 
	damageSkinData = "objectDamageSkins"; 
	explosionId = flashExpMedium; 
	description = "Panel"; 
}; 

StaticShapeData VerticalPanel 
{ 
	shapeFile = "teleport_vertical"; 
	debrisId = defaultDebrisSmall; 
	explosionId = debrisExpMedium; 
	maxDamage = 0.5; 
	damageSkinData = "objectDamageSkins"; 
	description = "Panel"; 
}; 

StaticShapeData BluePanel 
{ 
	shapeFile = "panel_blue"; 
	debrisId = flashDebrisSmall; 
	explosionId = flashExpMedium; 
	maxDamage = 0.5; 
	damageSkinData = "objectDamageSkins"; 
	description = "Panel"; 
}; 

StaticShapeData YellowPanel 
{ 
	shapeFile = "panel_yellow"; 
	debrisId = defaultDebrisSmall; 
	explosionId = debrisExpMedium; 
	maxDamage = 0.5; 
	damageSkinData = "objectDamageSkins"; 
	description = "Panel"; 
}; 

StaticShapeData SetPanel 
{ 
	shapeFile = "panel_set"; 
	debrisId = flashDebrisSmall; 
	explosionId = flashExpMedium; 
	maxDamage = 0.5; 
	damageSkinData = "objectDamageSkins"; 
	description = "Panel"; 
}; 

StaticShapeData VerticalPanelB 
{ 
	shapeFile = "panel_vertical"; 
	debrisId = defaultDebrisSmall; 
	explosionId = debrisExpMedium; 
	maxDamage = 0.5; 
	damageSkinData = "objectDamageSkins"; 
	description = "Panel"; 
}; 

StaticShapeData DisplayPanelOne 
{ 
	shapeFile = "display_one"; 
	debrisId = flashDebrisSmall; 
	explosionId = flashExpMedium; 
	maxDamage = 0.5; 
	damageSkinData = "objectDamageSkins"; 
	description = "Panel"; 
}; 

StaticShapeData DisplayPanelTwo 
{ 
	shapeFile = "display_two"; 
	debrisId = defaultDebrisSmall; 
	explosionId = debrisExpMedium; 
	maxDamage = 0.5; 
	damageSkinData = "objectDamageSkins"; 
	description = "Panel"; 
}; 

StaticShapeData DisplayPanelThree 
{ 
	shapeFile = "display_three"; 
	debrisId = flashDebrisSmall; 
	explosionId = flashExpMedium; 
	maxDamage = 0.5; 
	damageSkinData = "objectDamageSkins"; 
	description = "Panel"; 
}; 

StaticShapeData HOnePanel 
{ 
	shapeFile = "dsply_h1"; 
	debrisId = defaultDebrisSmall; 
	explosionId = debrisExpMedium; 
	maxDamage = 0.5; 
	damageSkinData = "objectDamageSkins"; 
	description = "Panel"; 
}; 

StaticShapeData HTwoPanel 
{ 
	shapeFile = "dsply_h2"; 
	debrisId = flashDebrisSmall; 
	explosionId = flashExpMedium; 
	maxDamage = 0.5; 
	damageSkinData = "objectDamageSkins"; 
	description = "Panel"; 
}; 

StaticShapeData SOnePanel 
{ 
	shapeFile = "dsply_s1"; 
	debrisId = defaultDebrisSmall; 
	explosionId = debrisExpMedium; 
	maxDamage = 0.5; 
	damageSkinData = "objectDamageSkins"; 
	description = "Panel"; 
}; 

StaticShapeData STwoPanel 
{ 
	shapeFile = "dsply_s2"; 
	debrisId = flashDebrisSmall; 
	explosionId = flashExpMedium; 
	maxDamage = 0.5; 
	damageSkinData = "objectDamageSkins"; 
	description = "Panel"; 
}; 

StaticShapeData VOnePanel 
{ 
	shapeFile = "dsply_v1"; 
	debrisId = defaultDebrisSmall; 
	explosionId = debrisExpMedium; 
	maxDamage = 0.5; 
	damageSkinData = "objectDamageSkins"; 
	description = "Panel"; 
}; 

StaticShapeData VTwoPanel 
{ 
	shapeFile = "dsply_v2"; 
	debrisId = flashDebrisSmall; 
	explosionId = flashExpMedium; 
	maxDamage = 0.5; 
	damageSkinData = "objectDamageSkins"; 
	description = "Panel"; 
}; 

StaticShapeData ForceField 
{ 
	shapeFile = "forcefield"; 
	debrisId = defaultDebrisSmall; 
	maxDamage = 10000.0; 
	isTranslucent = true; 
	description = "Force Field"; 
}; 

StaticShapeData ElectricalBeam 
{ 
	shapeFile = "zap"; 
	maxDamage = 10000.0; 
	isTranslucent = true; 
	description = "Electrical Beam"; 
	disableCollision = true; 
}; 

StaticShapeData ElectricalBeamBig 
{ 
	shapeFile = "zap_5"; 
	maxDamage = 10000.0; 
	isTranslucent = true; 
	description = "Electrical Beam"; 
	disableCollision = true; 
}; 

StaticShapeData PoweredElectricalBeam 
{ 
	shapeFile = "zap"; 
	maxDamage = 10000.0; 
	isTranslucent = true; 
	description = "Electrical Beam"; 
	disableCollision = true; 
}; 

function PoweredElectricalBeam::onPower(%this, %power, %generator) 
{ 
	if(%power) GameBase::startFadeIn(%this); 
	else GameBase::startFadeOut(%this); 
} 

StaticShapeData Cactus1 
{ 
	shapeFile = "cactus1"; 
	debrisId = defaultDebrisSmall; 
	maxDamage = 0.4; 
	description = "Cactus"; 
}; 

StaticShapeData Cactus2 
{ 
	shapeFile = "cactus2"; 
	debrisId = defaultDebrisSmall; 
	maxDamage = 0.4; 
	description = "Cactus"; 
}; 

StaticShapeData Cactus3 
{ 
	shapeFile = "cactus3"; 
	debrisId = defaultDebrisSmall; 
	maxDamage = 0.4; 
	description = "Cactus"; 
}; 

StaticShapeData SteamOnGrass 
{ 
	shapeFile = "steamvent_grass"; 
	maxDamage = 999.0; 
	isTranslucent = "True"; 
	description = "Steam Vent"; 
}; 

StaticShapeData SteamOnMud 
{ 
	shapeFile = "steamvent_mud"; 
	maxDamage = 999.0; 
	isTranslucent = "True"; 
	description = "Steam Vent"; 
}; 

StaticShapeData TreeShape 
{ 
	shapeFile = "tree1"; 
	maxDamage = 10.0; 
	isTranslucent = "True"; 
	description = "Tree"; 
}; 

StaticShapeData TreeShapeTwo 
{ 
	shapeFile = "tree2"; 
	maxDamage = 10.0; 
	isTranslucent = "True"; 
	description = "Tree"; 
}; 

StaticShapeData SteamOnGrass2 
{ 
	shapeFile = "steamvent2_grass"; 
	maxDamage = 999.0; 
	isTranslucent = "True"; 
}; 

StaticShapeData SteamOnMud2 
{ 
	shapeFile = "steamvent2_mud"; 
	maxDamage = 999.0; 
	isTranslucent = "True"; 
	description = "Steam Vent"; 
}; 

StaticShapeData PlantOne 
{ 
	shapeFile = "plant1"; 
	debrisId = defaultDebrisSmall; 
	maxDamage = 0.4; 
	description = "Plant"; 
}; 

StaticShapeData PlantTwo 
{ 
	shapeFile = "plant2"; 
	debrisId = defaultDebrisSmall; 
	maxDamage = 0.4; 
	description = "Plant"; 
}; 

StaticShapeData DeployableCactus2 
{ 
	shapeFile = "cactus2"; 
	debrisId = defaultDebrisSmall; 
	maxDamage = 0.5; 
	description = "Cactus"; 
}; 

function DeployableCactus2::onDestroyed(%this) 
{ 
	StaticShape::onDestroyed(%this); 
	$TeamItemCount[GameBase::getTeam(%this) @ "PlantPack"]--; 
} 

function DeployableCactus2::onCollision(%this,%obj) 
{ 
	if(getObjectType(%obj) != "Player") 
	{ 
		return; 
	} 
	if(Player::isDead(%obj)) 
	{ 
		return; 
	} 
	%c = Player::getClient(%obj); 
	%playerTeam = GameBase::getTeam(%obj); 
	%teleTeam = GameBase::getTeam(%this); 
	if(%teleTeam != %playerTeam) 
	{ } 
	if(GameBase::getDamageLevel(%obj)) 
	{ 
		GameBase::repairDamage(%obj,0.3); 
		GameBase::playSound(%this,ForceFieldOpen,0); 
	} 
	$poisonTime[%c] = 0; 
} 
