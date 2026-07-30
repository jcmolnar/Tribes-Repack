//------------------------------------------------------------------------
// Generic static shapes
//------------------------------------------------------------------------


//------------------------------------------------------------------------
// Default power animation behavior for all static shapes

function StaticShape::onPower(%this,%power,%generator)
{
	if (%power) 
		GameBase::playSequence(%this,0,"power");
	else 
		GameBase::stopSequence(%this,0);
}

function StaticShape::onEnabled(%this)
{
	if (GameBase::isPowered(%this)) 
		GameBase::playSequence(%this,0,"power");
}

function StaticShape::onDisabled(%this)
{
	GameBase::stopSequence(%this,0);
}

function StaticShape::onDestroyed(%this)
{
	GameBase::stopSequence(%this,0);
   StaticShape::objectiveDestroyed(%this);
	calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 2, 0.40, 
		0.1, 250, 100); 
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function smokeEffects(%this)
{
	%pos = GameBase::getPosition(%this);
	%vName = GameBase::getDataName(%this); 
	%cDam = GameBase::getDamageLevel(%this);
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	
	%velocI = Item::getVelocity(%this); 
	%xvelocI = getword(%velocI, 0);
	%yvelocI = getword(%velocI, 1);
	%zvelocI = getword(%velocI, 2);
	%speedI = sqrt(%xvelocI*%xvelocI + %yvelocI*%yvelocI + %zvelocI*%zvelocI);
	
	if((%vName.maxDamage - %cDam) < %vName.maxDamage*0.5) { // if the plane is damaged more then half
		if( %vName == Bradley || %vName == Humvee || %vName == Abrams || %speedI < %vName.maxspeed *0.5) {
		// Then spew some smoke	
		Projectile::spawnProjectile("VehicleSmokeExpulsion", %trans, %this, "0 0 50");
		$Smoking[%this] = 1;
		schedule("smokeEffects(\""@%this@"\");", 0.2, %this);
		
		} else {
		
		// Then spew some smoke	
		Projectile::spawnProjectile("VehicleSmokeExpulsion", %trans, %this, "0 0 0");
		$Smoking[%this] = 1;
		schedule("smokeEffects(\""@%this@"\");", 0.2, %this);
		}
	}
}

function fireEffects(%this)
{

	%velocI = Item::getVelocity(%this); 
	%xvelocI = getword(%velocI, 0);
	%yvelocI = getword(%velocI, 1);
	%zvelocI = getword(%velocI, 2);
	%speedI = sqrt(%xvelocI*%xvelocI + %yvelocI*%yvelocI + %zvelocI*%zvelocI);
	
	%pos = GameBase::getPosition(%this);
	%vName = GameBase::getDataName(%this); 
	%cDam = GameBase::getDamageLevel(%this);
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	if((%vName.maxDamage - %cDam) < %vName.maxDamage*0.3){ // if the plane is damaged more then 1/4
		if( %vName == Bradley || %vName == Humvee || %vName == Abrams || %speedI < %vName.maxspeed *0.5){
			// Then spew some fire upwards
			Projectile::spawnProjectile("VehicleFlameExpulsion", %trans, %this, "0 0 50");
			$Flamming[%this] = 1;	
			schedule("fireEffects(\""@%this@"\");", 0.05, %this);
		} else {
			// Then spew some fire too
			Projectile::spawnProjectile("VehicleFlameExpulsion", %trans, %this, "0 0 0");
			$Flamming[%this] = 1;	
			schedule("fireEffects(\""@%this@"\");", 0.05, %this);
		}
	}
}

function plummetEffects(%this)
{	
	%pos = GameBase::getPosition(%this);
	%vName = GameBase::getDataName(%this); 
	%cDam = GameBase::getDamageLevel(%this);
	%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
	
	%velocI = Item::getVelocity(%this); 
	%xvelocI = getword(%velocI, 0);
	%yvelocI = getword(%velocI, 1);
	%zvelocI = getword(%velocI, 2);
	%speedI = sqrt(%xvelocI*%xvelocI + %yvelocI*%yvelocI + %zvelocI*%zvelocI);
	
	%refHeight = "-1.575 0 0";
	
	if(GameBase::getLOSInfo(%this,20,%refHeight))
	{
		%VgroundPos = $los::position;
		%VvehiclePos = GameBase::getPosition(%this);
		%VvehicleZ = getword(%VvehiclePos, 2);
		%Vgroundz = getword(%VgroundPos, 2);
		%Vheight = %VvehicleZ - %VgroundZ;
	}

	// if its a flying vehicle, have it plummet. *irrelevent unless I put in damage for tanks*
	//if( %vName== Fighter || %vName == Fighter2 || %vName == DiveBomber || %vName == MoveWarthog || %vName == Warthog || %vName == Bomber || %vName == Gunship || %vName == Hercules || %vName == Hind || %vName == Apache || %vName == Blackhawk) {
	//}
	// if the plane is damaged almost to destruction (90% dead, 95% for larger planes)
	if( ((GameBase::getDataName(%this) == Fighter || GameBase::getDataName(%this) == Fighter2 || GameBase::getDataName(%this) == DiveBomber || GameBase::getDataName(%this) == Apache) && ((%vName.maxDamage - %cDam) < %vName.maxDamage*0.1)) 
	|| ((GameBase::getDataName(%this) == Warthog || GameBase::getDataName(%this) == MoveWarthog || GameBase::getDataName(%this) == Hind || GameBase::getDataName(%this) == BlackHawk) && ((%vName.maxDamage - %cDam) < %vName.maxDamage*0.08)) || 
	((GameBase::getDataName(%this) == Bomber || GameBase::getDataName(%this) == Gunship || GameBase::getDataName(%this) == Hercules) && ((%vName.maxDamage - %cDam) < %vName.maxDamage*0.05))) {
		$plummeting[%this] = 1;
		if(%speedI > 0.1 ) {
			%OldRot = GameBase::getRotation(%this);
			%NewXRot = getword(%OldRot, 0);
			%NewYRot = getword(%OldRot, 1);
			%NewZRot = getword(%OldRot, 2);
			if(%NewXRot > -1.4){
				%NewXRot = %NewXRot - 0.1;
			}else if(%NewXRot < -1.8){
				%NewXRot = %NewXRot + 0.1;
			}
			%NewRot = %NewXRot @ " " @ %NewYRot @ " " @ %NewZRot;
			GameBase::setRotation(%this,%NewRot);
			
			if(%speedI < %vName.Maxspeed) {
				Player::applyImpulse(%this, "0 0 -75");
			}
			
		}
		schedule("plummetEffects(\""@%this@"\");", 0.05, %this);
	}
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function StaticShape::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	%damageLevel = GameBase::getDamageLevel(%this);
	%dValue = %damageLevel + %value;
	if(GameBase::getDataName(%this).className == Vehicle){
		if(%object && %object != -1){
		   %this.lastDamageObject = %object;
		   %this.lastDamageTeam = GameBase::getTeam(%object);
		}
	} else {
		%this.lastDamageObject = %object;
		%this.lastDamageTeam = GameBase::getTeam(%object);
				
	}

	if(GameBase::getTeam(%this) == GameBase::getTeam(%object)) {
		%name = GameBase::getDataName(%this);
		if(%name.className == Generator || %name.className == Station || %name.className == Sensor ) { 
			%TDS = $Server::TeamDamageScale / 100;
			%dValue = %damageLevel + %value * %TDS;
			%disable = GameBase::getDisabledDamage(%this);
			if(!$Server::TourneyMode && %dValue > %disable - 0.05) {
            if(%damageLevel > %disable - 0.05)
               return;
            else
               %dValue = %disable - 0.05;
			}
		}
	}
	GameBase::setDamageLevel(%this,%dValue);
	if(GameBase::getDataName(%this).className == Vehicle){
		if($Smoking[%this] != 1){
			smokeEffects(%this);
		}
		if($Flamming[%this] != 1){
			fireEffects(%this);
		}
		if($Plummeting[%this] != 1 && (GameBase::getDataName(%this) != Abrams && GameBase::getDataName(%this) != Bradley && GameBase::getDataName(%this) != Humvee && GameBase::getDataName(%this) != LineBacker && GameBase::getDataName(%this) != MLRS)){
			plummetEffects(%this);
		}
	}
}

function StaticShape::shieldDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	%damageLevel = GameBase::getDamageLevel(%this);
   %this.lastDamageObject = %object;
   %this.lastDamageTeam = GameBase::getTeam(%object);
	if (%this.shieldStrength) {
		%energy = GameBase::getEnergy(%this);
		%strength = %this.shieldStrength;
		if (%type == $ShrapnelDamageType)
			%strength *= 0.5;
		else
			if (%type == $MortarDamageType)
				%strength *= 0.25;
			else
				if (%type == $BlasterDamageType)
					%strength *= 2.0;
		%absorb = %energy * %strength;
		if (%value < %absorb) {
			GameBase::setEnergy(%this,%energy - (%value / %strength));
			//%centerPos = getBoxCenter(%this);
			//%sphereVec = findPointOnSphere(getBoxCenter(%object),%centerPos,%vec,%this);
			//%centerPosX = getWord(%centerPos,0);
			//%centerPosY = getWord(%centerPos,1);
			//%centerPosZ = getWord(%centerPos,2);

			//%pointX = getWord(%pos,0);
			//%pointY = getWord(%pos,1);
			//%pointZ = getWord(%pos,2);

			//%newVecX = %centerPosX - %pointX;
			//%newVecY = %centerPosY - %pointY;
			//%newVecZ = %centerPosZ - %pointZ;
			//%norm = Vector::normalize(%newVecX @ " " @ %newVecY @ " " @ %newVecZ);
			//%zOffset = 0;
			//if(GameBase::getDataName(%this) == PulseSensor)
			//	%zOffset = (%pointZ-%centerPosZ) * 0.5;
			//GameBase::activateShield(%this,%sphereVec,%zOffset);
		}
		else {
			GameBase::setEnergy(%this,0);
			StaticShape::onDamage(%this,%type,%value - %absorb,%pos,%vec,%mom,%object);
		}
	}
	else {
		StaticShape::onDamage(%this,%type,%value,%pos,%vec,%mom,%object);
	}
}

StaticShapeData FlagStand
{
   description = "Flag Stand";
	shapeFile = "flagstand";
	visibleToSensor = false;
};


function calcRadiusDamage(%this,%type,%radiusRatio,%damageRatio,%forceRatio,
	%rMax,%rMin,%dMax,%dMin,%fMax,%fMin) 
{
	%radius = GameBase::getRadius(%this);
	if(%radius) {
		%radius *= %radiusRatio;
		%damageValue = %radius * %damageRatio;
		%force = %radius * %forceRatio;
		if(%radius > %rMax)
			%radius = %rMax;
		else if(%radius < %rMin)
			%radius = %rMin;
		if(%damageValue > %dMax)
			%damageValue = %dMax; 
		else if(%damageValue < %dMin)
			%damageValue = %dMin;
		if(%force > %fMax)
			%force = %fMax; 
		else if(%force < %fMin)
			%force = %fMin;
		GameBase::applyRadiusDamage(%type,getBoxCenter(%this), %radius,
			%damageValue,%force,%this);
	}
}



function FlagStand::onDamage()
{
}

//------------------------------------------------------------------------
// Generators
//------------------------------------------------------------------------

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
	calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 3, 0.55, 
		0.30, 250, 170); 
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

//

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

StaticShapeData DeployablePortGen 
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

function DeployablePortGen::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	%damageLevel = GameBase::getDamageLevel(%this);
	%dValue = %damageLevel + %value;
  	%this.lastDamageObject = %object;
   	%this.lastDamageTeam = GameBase::getTeam(%object);
	GameBase::setDamageLevel(%this,%dValue);
}

function DeployablePortGen::onDestroyed(%this) 
{
	%num = Group::objectCount($PowerSet[%this]);
	if(%num > 0) {
		for (%i = 0; %i < %num; %i++) {
			%obj = Group::getObject($PowerSet[%this], %i);
			%objName = GameBase::getDataName(%obj);
			$PortGenPower[%obj] = 0;
			if (%objName == InventoryStation || %objName == AmmoStation || %objName == VehicleStation || %objName == CommandStation || %objName == VehiclePad || %objName == JetStation|| %objName == JetPad || %objName == ChopperStation || %objName == ChopperPad || %objName == TankStation || %objName == TankPad) {
				if (GameBase::getPowerCount(%obj) == 0) {
					if (%objName != VehiclePad) {
						GameBase::stopSequence(%obj,0);
						GameBase::pauseSequence(%obj,1);
						GameBase::pauseSequence(%obj,2);
						Station::checkTarget(%obj);						
					}
					GameBase::setActive(%obj, false);
				}
			}
			//else if(%objName == elevator4x4 || %objName == elevator6x6 || %objName == elevator6x6Octa || %objName == elevator8x4 || %objName == elevator8x6 || %objName == elevator9x9 || %objName == elevator16x16Octa || %objName == elevator6x4 || %objName == elevator6x5 || %objName == elevator8x8 || %objName == elevator6x4thin || %objName == elevator6x6thin || %objName == elevator5x5 || %objName == elevator4x5) {
			else if(GameBase::getClassName(%obj) == "Elevator") {			
				if(GameBase::getPowerCount(%obj) == 0) {
					Elevator::onPower(%obj,false);
				} 
			}
			//else if(%objName == DoorOneTop || %objName == DoorOneBottom || %objName == DoorOneLeft || %objName == DoorOneRight || %objName == DoorTwoLeft || %objName == DoorTwoRight || %objName == DoorThreeLeft || %objName == DoorThreeRight || %objName == DoorFourLeft || %objName == DoorFourRight || %objName == DoorSixLeft || %objName == DoorSixRight || %objName == DoorSevenLeft || %objName == DoorSevenRight || %objName == DoorFive || %objName == DoorDiagonal) {
			else if(GameBase::getClassName(%obj) == "Door") {
				if(GameBase::getPowerCount(%obj) == 0) {
					Door::onPower(%obj,false);
				}
			}
		}
	}
	$TeamItemCount[GameBase::getTeam(%this) @ "PortGenPack"]--;
	deleteObject($PowerSet[%this]);
	StaticShape::onDestroyed(%this);
}

//------------------------------------------------------------------------

//------------------------------------------------------------------------
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

//------------------------------------------------------------------------
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

//------------------------------------------------------------------------
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

//------------------------------------------------------------------------
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

//------------------------------------------------------------------------
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

//------------------------------------------------------------------------
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

//------------------------------------------------------------------------
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

//------------------------------------------------------------------------
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

//------------------------------------------------------------------------
StaticShapeData SquarePanel
{
	shapeFile = "teleport_square";
	debrisId = flashDebrisSmall;
	maxDamage = 0.3;
	damageSkinData = "objectDamageSkins";
	explosionId = flashExpMedium;
   description = "Panel";
};

//------------------------------------------------------------------------
StaticShapeData VerticalPanel
{
	shapeFile = "teleport_vertical";
	debrisId = defaultDebrisSmall;
	explosionId = debrisExpMedium;
	maxDamage = 0.5;
	damageSkinData = "objectDamageSkins";
   description = "Panel";
};

//------------------------------------------------------------------------
StaticShapeData BluePanel
{
	shapeFile = "panel_blue";
	debrisId = flashDebrisSmall;
	explosionId = flashExpMedium;
	maxDamage = 0.5;
	damageSkinData = "objectDamageSkins";
   description = "Panel";
};

//------------------------------------------------------------------------
StaticShapeData YellowPanel
{
	shapeFile = "panel_yellow";
	debrisId = defaultDebrisSmall;
	explosionId = debrisExpMedium;
	maxDamage = 0.5;
	damageSkinData = "objectDamageSkins";
   description = "Panel";
};

//------------------------------------------------------------------------
StaticShapeData SetPanel
{
	shapeFile = "panel_set";
	debrisId = flashDebrisSmall;
	explosionId = flashExpMedium;
	maxDamage = 0.5;
	damageSkinData = "objectDamageSkins";
   description = "Panel";
};

//------------------------------------------------------------------------
StaticShapeData VerticalPanelB
{
	shapeFile = "panel_vertical";
	debrisId = defaultDebrisSmall;
	explosionId = debrisExpMedium;
	maxDamage = 0.5;
	damageSkinData = "objectDamageSkins";
   description = "Panel";
};

//------------------------------------------------------------------------
StaticShapeData DisplayPanelOne
{
	shapeFile = "display_one";
	debrisId = flashDebrisSmall;
	explosionId = flashExpMedium;
	maxDamage = 0.5;
	damageSkinData = "objectDamageSkins";
   description = "Panel";
};

//------------------------------------------------------------------------
StaticShapeData DisplayPanelTwo
{
	shapeFile = "display_two";
	debrisId = defaultDebrisSmall;
	explosionId = debrisExpMedium;
	maxDamage = 0.5;
	damageSkinData = "objectDamageSkins";
   description = "Panel";
};

//------------------------------------------------------------------------
StaticShapeData DisplayPanelThree
{
	shapeFile = "display_three";
	debrisId = flashDebrisSmall;
	explosionId = flashExpMedium;
	maxDamage = 0.5;
	damageSkinData = "objectDamageSkins";
   description = "Panel";
};

//------------------------------------------------------------------------
StaticShapeData HOnePanel
{
	shapeFile = "dsply_h1";
	debrisId = defaultDebrisSmall;
	explosionId = debrisExpMedium;
	maxDamage = 0.5;
	damageSkinData = "objectDamageSkins";
   description = "Panel";
};

//------------------------------------------------------------------------
StaticShapeData HTwoPanel
{
	shapeFile = "dsply_h2";
	debrisId = flashDebrisSmall;
	explosionId = flashExpMedium;
	maxDamage = 0.5;
	damageSkinData = "objectDamageSkins";
   description = "Panel";
};

//------------------------------------------------------------------------
StaticShapeData SOnePanel
{
	shapeFile = "dsply_s1";
	debrisId = defaultDebrisSmall;
	explosionId = debrisExpMedium;
	maxDamage = 0.5;
	damageSkinData = "objectDamageSkins";
   description = "Panel";
};

//------------------------------------------------------------------------
StaticShapeData STwoPanel
{
	shapeFile = "dsply_s2";
	debrisId = flashDebrisSmall;
	explosionId = flashExpMedium;
	maxDamage = 0.5;
	damageSkinData = "objectDamageSkins";
   description = "Panel";
};

//------------------------------------------------------------------------
StaticShapeData VOnePanel
{
	shapeFile = "dsply_v1";
	debrisId = defaultDebrisSmall;
	explosionId = debrisExpMedium;
	maxDamage = 0.5;
	damageSkinData = "objectDamageSkins";
   description = "Panel";
};

//------------------------------------------------------------------------
StaticShapeData VTwoPanel
{
	shapeFile = "dsply_v2";
	debrisId = flashDebrisSmall;
	explosionId = flashExpMedium;
	maxDamage = 0.5;
	damageSkinData = "objectDamageSkins";
   description = "Panel";
};

//------------------------------------------------------------------------
StaticShapeData ForceField
{
	shapeFile = "forcefield";
	debrisId = defaultDebrisSmall;
	maxDamage = 10000.0;
	isTranslucent = true;
   description = "Force Field";
};

//------------------------------------------------------------------------
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

//function to fade in electrical beam based on base power.
function PoweredElectricalBeam::onPower(%this, %power, %generator)
{
   if(%power)
	  GameBase::startFadeIn(%this);
   else
      GameBase::startFadeOut(%this);
}
      
//-----------------------------------------------------------------------
StaticShapeData Cactus1
{
	shapeFile = "cactus1";
	debrisId = defaultDebrisSmall;
	maxDamage = 0.4;
   description = "Cactus";
};
//------------------------------------------------------------------------
StaticShapeData Cactus2
{
	shapeFile = "cactus2";
	debrisId = defaultDebrisSmall;
	maxDamage = 0.4;
   description = "Cactus";
};
//------------------------------------------------------------------------
StaticShapeData Cactus3
{
	shapeFile = "cactus3";
	debrisId = defaultDebrisSmall;
	maxDamage = 0.4;
   description = "Cactus";
};

//------------------------------------------------------------------------
StaticShapeData SteamOnGrass
{
	shapeFile = "steamvent_grass";
	maxDamage = 999.0;
	isTranslucent = "True";
   description = "Steam Vent";
};

//------------------------------------------------------------------------
StaticShapeData SteamOnMud
{
	shapeFile = "steamvent_mud";
	maxDamage = 999.0;
	isTranslucent = "True";
   description = "Steam Vent";
};

//------------------------------------------------------------------------
StaticShapeData TreeShape
{
	shapeFile = "tree1";
	maxDamage = 10.0;
	isTranslucent = "True";
   description = "Tree";
};

//------------------------------------------------------------------------
StaticShapeData TreeShapeTwo
{
	shapeFile = "tree2";
	maxDamage = 10.0;
	isTranslucent = "True";
   description = "Tree";
};

//------------------------------------------------------------------------
StaticShapeData SteamOnGrass2
{
	shapeFile = "steamvent2_grass";
	maxDamage = 999.0;
	isTranslucent = "True";
};

//------------------------------------------------------------------------
StaticShapeData SteamOnMud2
{
	shapeFile = "steamvent2_mud";
	maxDamage = 999.0;
	isTranslucent = "True";
   description = "Steam Vent";
};
//------------------------------------------------------------------------
StaticShapeData PlantOne
{
	shapeFile = "plant1";
	debrisId = defaultDebrisSmall;
	maxDamage = 0.4;
   description = "Plant";
};

//------------------------------------------------------------------------
StaticShapeData PlantTwo
{
	shapeFile = "plant2";
	debrisId = defaultDebrisSmall;
	maxDamage = 0.4;
   description = "Plant";
};

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

StaticShapeData SandBag
{
	shapeFile = "cactus2";
	debrisId = flashDebrisSuperSmall;
	maxDamage = 3.0;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
   description = "Sand Bag";
};


function SandBag::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	
	if( $SandBagOwner )
		echo( "This sandbag was deployed by " @ %this.deployerName );
	%damageLevel = GameBase::getDamageLevel(%this);
	if( %type == $BulletDamageType || %type == $ShrapnelDamageType || %type == $LaserDamageType || %type == $DebrisDamageType || %type == $StabDamageType || %type == $PlasmaDamageType ){
 		%value = %value /10;
	}
	if( %type == $SmokeDamageType || %type == $PoisonDamageType || %type == $BleedDamageType || %type == $ElectricityDamageType || %type == $EnergyDamageType || %type == $BlasterDamageType ){
 		%value = 0;
	}
	%dValue = %damageLevel + %value;
	%this.lastDamageObject = %object;
	%this.lastDamageTeam = GameBase::getTeam(%object);
	GameBase::setDamageLevel(%this,%dValue);
	
}

function SandBag::onDestroyed(%this)
{
	$TeamItemCount[GameBase::getTeam(%this) @ "SandBag"]--;
}
