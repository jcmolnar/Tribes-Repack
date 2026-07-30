// DELTA FORCE ADDED TURRETS
// --------------------------------------------------------

TurretData DeployableSAM
{
	maxDamage = 3.0;
	maxEnergy = 100;
	minGunEnergy = 60;
	maxGunEnergy = 60;
	range = 150;
	gunRange = 500;
	visibleToSensor = true;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = defaultDebrisLarge;
	className = "Turret";
	shapeFile = "missileturret";
	shieldShapeName = "shield_medium";
	speed = 2.0;
	speedModifier = 2.0;
	projectileType = TurretMissile;
	reloadDelay = 3.5;
	fireSound = SoundMissileTurretFire;
	activationSound = SoundMissileTurretOn;
	deactivateSound = SoundMissileTurretOff;
	whirSound = SoundMissileTurretTurn;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 8;
   targetableFovRatio = 0.5;
	explosionId = LargeShockwave;
	description = "SAM Launcher";
};

function DeployableSAM::verifyTarget(%this,%target)
{
   if (GameBase::virtual(%target, "getHeatFactor") >= 0.5){
	//if($targetcheck[%this, %target] && $targetcheck[%this, %target] == true){
		$SamTarget = GameBase::getOwnerClient(%target);
		return "True";
	//}
	//else {
	 //	$targetcheck[%this, %target] = true;
	   //  	Client::sendMessage(Player::getClient(%target),0,"** WARNING ** - an enemy SAM is obtaining a Missile Lock!~waccess_denied.wav");
		//schedule("Client::sendMessage(" @ Player::getClient(%target) @ ",0,\"~waccess_denied.wav\");",0.5);
	//	schedule("Client::sendMessage(" @ Player::getClient(%target) @ ",0,\"~waccess_denied.wav\");",1.0);
//		schedule("Client::sendMessage(" @ Player::getClient(%target) @ ",0,\"~waccess_denied.wav\");",1.5);
//		schedule("$targetcheck[" @ %this @ ", " @ %target @ "] = false;", 4);
//		return "True";
//	}
   }
   else
      return "False";
}

function DeployableSAM::onAdd(%this)
{
	schedule("DeployableSAM::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,5);
	%this.shieldStrength = 0;
	if (GameBase::getMapName(%this) == "") {
		GameBase::setMapName (%this, "SAM Launcher");
	}
}

function DeployableSAM::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function DeployableSAM::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function DeployableSAM::onDestroyed(%this)
{
	Turret::onDestroyed(%this);
  	$TeamItemCount[GameBase::getTeam(%this) @ "SAMPack"]--;
}

// Override base class just in case.
function DeployableSAM::onPower(%this,%power,%generator) 
{
	if (%power) {
		%this.shieldStrength = 0.03;
		GameBase::setRechargeRate(%this,14);
	}
	else {
		%this.shieldStrength = 0;
		GameBase::setRechargeRate(%this,0);
		Turret::checkOperator(%this);
	}
	GameBase::setActive(%this,%power);
}

function DeployableSAM::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);
}

// ---------------------------------------------------

TurretData DeployableAA { 
	className = "Turret"; 
	shapeFile = "hellfiregun"; 
	projectileType = AntiAirShell; 
	maxDamage = 3.5; 
	maxEnergy = 150; 
	minGunEnergy = 1; 
	maxGunEnergy = 3; 
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor }; 
	reloadDelay = 0.4; 
	speed = 2.0; 
	speedModifier = 1.5; 
	range = 100; 
	gunRange = 300;
	visibleToSensor = true; 
	shadowDetailMask = 4; 
	dopplerVelocity = 0; 
	castLOS = true; 
	supression = false; 
	mapFilter = 2; 
	mapIcon = "M_turret"; 
	debrisId = flashDebrisMedium; 
	shieldShapeName = "shield"; 
	fireSound = SoundMortarTurretFire; 
	activationSound = SoundPlasmaTurretOn; 
	deactivateSound = SoundPlasmaTurretOff; 
	whirSound = SoundPlasmaTurretTurn; 
	explosionId = flashExpMedium; 
	description = "AA Flak Gun"; 
	damageSkinData = "objectDamageSkins"; 
}; 

function DeployableAA::onAdd(%this) { 
	schedule("DeployableAA::deploy(" @ %this @ ");",1,%this); 
	GameBase::setRechargeRate(%this,0); 
	%this.shieldStrength = 0.0; 
	if (GameBase::getMapName(%this) == "") { 
		GameBase::setMapName (%this, "AA Flak Gun"); 
	} 
} 

function DeployableAA::verifyTarget(%this,%target)
{
   if (GameBase::virtual(%target, "getHeatFactor") >= 0.5)
      return "True";
   else
      return "False";
}

function DeployableAA::deploy(%this) { 
	GameBase::playSequence(%this,1,"deploy"); 
} 

function DeployableAA::onEndSequence(%this,%thread) { 
	GameBase::setActive(%this,true); 
} 

function DeployableAA::onDestroyed(%this) { 
	StaticShape::objectiveDestroyed(%this); 
	%this.shieldStrength = 0; 
	GameBase::setRechargeRate(%this,0); 
	Turret::onDeactivate(%this); 
	Turret::objectiveDestroyed(%this); 
	CalcRadiusDamage(%this,$DebrisDamageType,20,0.2,25,20,20,2.5,1.1,200,100); 
	$TeamItemCount[GameBase::getTeam(%this) @ "AAPack"]--;
	$TurretControl[%this] = 0; 
} 

function DeployableAA::onPower(%this,%power,%generator) {
	Turret::checkOperator(%this);
} 

function DeployableAA::onEnabled(%this) { 
	%this.shieldStrength = 0.03;
	GameBase::setRechargeRate(%this,0); 
	GameBase::setActive(%this,true); 
}

TurretData FlakTurret
{
	maxDamage = 3.5;
	maxEnergy = 100;
	minGunEnergy = 1;
	maxGunEnergy = 2;
	range = 130;
	gunRange = 300;
	visibleToSensor = true;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = defaultDebrisLarge;
	className = "Turret";
	shapeFile = "hellfiregun";
	shieldShapeName = "shield_medium";
	speed = 2.0;
	speedModifier = 1.5;
	projectileType = AntiAirShell;
	reloadDelay = 0.4;
	fireSound = SoundMortarTurretFire;
	activationSound = SoundMissileTurretOn;
	deactivateSound = SoundMissileTurretOff;
//	whirSound = SoundMissileTurretTurn;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 8;
   targetableFovRatio = 0.5;
	explosionId = LargeShockwave;
	description = "AA Flak Gun";
};

function FlakTurret::onPower(%this,%power,%generator)
{
	if (%power) {
		%this.shieldStrength = 0.03;
		GameBase::setRechargeRate(%this,0);
	}
	else {
		%this.shieldStrength = 0;
		GameBase::setRechargeRate(%this,0);
		Turret::checkOperator(%this);
	}
	GameBase::setActive(%this,%power);
}

function FlakTurret::verifyTarget(%this,%target)
{
   if (GameBase::virtual(%target, "getHeatFactor") >= 0.5)
      return "True";
   else
      return "False";
}

// ---------------------------------------------------

TurretData ControlledDeployableAA { 
	className = "Turret"; 
	shapeFile = "hellfiregun"; 
	//projectileType = AntiAirShell; 
	maxDamage = 3.5; 
	maxEnergy = 150; 
	minGunEnergy = 1; 
	maxGunEnergy = 3; 
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor }; 
	reloadDelay = 0.4; 
	speed = 2.0; 
	speedModifier = 1.5; 
	range = 100; 
	gunRange = 300;
	visibleToSensor = true; 
	shadowDetailMask = 4; 
	dopplerVelocity = 0; 
	castLOS = true; 
	supression = false; 
	mapFilter = 2; 
	mapIcon = "M_turret"; 
	debrisId = flashDebrisMedium; 
	shieldShapeName = "shield"; 
	fireSound = SoundMortarTurretFire; 
	activationSound = SoundPlasmaTurretOn; 
	deactivateSound = SoundPlasmaTurretOff; 
	whirSound = SoundPlasmaTurretTurn; 
	explosionId = flashExpMedium; 
	description = "AA Flak Gun"; 
	damageSkinData = "objectDamageSkins"; 
}; 

function ControlledDeployableAA::onAdd(%this) { 
	schedule("ControlledDeployableAA::deploy(" @ %this @ ");",1,%this); 
	GameBase::setRechargeRate(%this,0); 
	%this.shieldStrength = 0.0; 
	if (GameBase::getMapName(%this) == "") { 
		GameBase::setMapName (%this, "AA Flak Gun"); 
	} 
} 

function ControlledDeployableAA::onFire(%this, %target)
{
	if ($TurretCanNotFire[%this] != True )
	{
		if( gamebase::getenergy(%this) > 1 )
		{
			if(getword(gamebase::getmuzzletransform(%this),5) > 0.22) {
				Projectile::spawnProjectile("AntiAirShell",gamebase::getmuzzletransform(%this),%this,Item::getVelocity(%this));
				gamebase::setenergy(%this, gamebase::getenergy(%this) - 3);
				GameBase::playSound(%this, Gamebase::getdataname(%this).fireSound, 3);
			}
			else
				Client::sendMessage(GameBase::getControlClient(%this),0,"Flak must be at a minimum angle of 15 degrees.~waccess_denied.wav");	

		} else
			Client::sendMessage(GameBase::getControlClient(%this),0,"Out of ammo!");
		$TurretCanNotFire[%this] = True;
		%delay = Gamebase::getdataname(%this).reloadDelay;
		schedule("$TurretCanNotFire["@%this@"] = False;", %delay, %this);  		
	}
}

function ControlledDeployableAA::verifyTarget(%this,%target)
{
   if (GameBase::virtual(%target, "getHeatFactor") >= 0.5)
      return "True";
   else
      return "False";
}

function ControlledDeployableAA::deploy(%this) { 
	GameBase::playSequence(%this,1,"deploy"); 
} 

function ControlledDeployableAA::onEndSequence(%this,%thread) { 
	GameBase::setActive(%this,true); 
} 

function ControlledDeployableAA::onDestroyed(%this) { 
	StaticShape::objectiveDestroyed(%this); 
	%this.shieldStrength = 0; 
	GameBase::setRechargeRate(%this,0); 
	Turret::onDeactivate(%this); 
	Turret::objectiveDestroyed(%this); 
	CalcRadiusDamage(%this,$DebrisDamageType,20,0.2,25,20,20,2.5,1.1,200,100); 
	$TeamItemCount[GameBase::getTeam(%this) @ "AAPack"]--;
	$TurretControl[%this] = 0; 
} 

function ControlledDeployableAA::onPower(%this,%power,%generator) {
	Turret::checkOperator(%this);
} 

function ControlledDeployableAA::onEnabled(%this) { 
	%this.shieldStrength = 0.03;
	GameBase::setRechargeRate(%this,0); 
	GameBase::setActive(%this,true); 
}

TurretData ControlledFlakTurret
{
	maxDamage = 3.5;
	maxEnergy = 100;
	minGunEnergy = 1;
	maxGunEnergy = 2;
	range = 130;
	gunRange = 300;
	visibleToSensor = true;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = defaultDebrisLarge;
	className = "Turret";
	shapeFile = "hellfiregun";
	shieldShapeName = "shield_medium";
	speed = 2.0;
	speedModifier = 1.5;
	//projectileType = AntiAirShell;
	reloadDelay = 0.4;
	fireSound = SoundMortarTurretFire;
	activationSound = SoundMissileTurretOn;
	deactivateSound = SoundMissileTurretOff;
//	whirSound = SoundMissileTurretTurn;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 8;
   targetableFovRatio = 0.5;
	explosionId = LargeShockwave;
	description = "AA Flak Gun";
};

function ControlledFlakTurret::onFire(%this, %target)
{
	if ($TurretCanNotFire[%this] != True )
	{
		if( gamebase::getenergy(%this) > 1 )
		{
			if(getword(gamebase::getmuzzletransform(%this),5) > 0.22) {
				Projectile::spawnProjectile("AntiAirShell",gamebase::getmuzzletransform(%this),%this,Item::getVelocity(%this));
				gamebase::setenergy(%this, gamebase::getenergy(%this) - 3);
				GameBase::playSound(%this, Gamebase::getdataname(%this).fireSound, 3);
			}
			else
				Client::sendMessage(GameBase::getControlClient(%this),0,"Flak must be at a minimum angle of 15 degrees.~waccess_denied.wav");	

		} else
			Client::sendMessage(GameBase::getControlClient(%this),0,"Out of ammo!");
		$TurretCanNotFire[%this] = True;
		%delay = Gamebase::getdataname(%this).reloadDelay;
		schedule("$TurretCanNotFire["@%this@"] = False;", %delay, %this);  		
	}
}


function ControlledFlakTurret::onPower(%this,%power,%generator)
{
	if (%power) {
		%this.shieldStrength = 0.30;
		GameBase::setRechargeRate(%this,0);
	}
	else {
		%this.shieldStrength = 0;
		GameBase::setRechargeRate(%this,0);
		Turret::checkOperator(%this);
	}
	GameBase::setActive(%this,%power);
}

function ControlledFlakTurret::verifyTarget(%this,%target)
{
   if (GameBase::virtual(%target, "getHeatFactor") >= 0.5)
      return "True";
   else
      return "False";
}

// ---------------------------------------------------

TurretData DeployablePlasma { 
	className = "Turret"; 
	shapeFile = "hellfiregun"; 
	projectileType = NATOBullet; 
	maxDamage = 3.0; 
	maxEnergy = 500; 
	minGunEnergy = 1; 
	maxGunEnergy = 3; 
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor }; 
	reloadDelay = 0.1; 
	speed = 2.0; 
	speedModifier = 1.5; 
	range = 100; 
	visibleToSensor = true; 
	shadowDetailMask = 4; 
	dopplerVelocity = 0; 
	castLOS = true; 
	supression = false; 
	mapFilter = 2; 
	mapIcon = "M_turret"; 
	debrisId = flashDebrisMedium; 
	shieldShapeName = "shield"; 
	fireSound = SoundMortarTurretFire; 
	activationSound = SoundPlasmaTurretOn; 
	deactivateSound = SoundPlasmaTurretOff; 
	whirSound = SoundPlasmaTurretTurn; 
	explosionId = flashExpMedium; 
	description = "20mm Cannon Turret"; 
	damageSkinData = "objectDamageSkins"; 
}; 

function DeployablePlasma::onAdd(%this) { 
	schedule("DeployablePlasma::deploy(" @ %this @ ");",1,%this); 
	GameBase::setRechargeRate(%this,0); 
	%this.shieldStrength = 0.010; 
	if (GameBase::getMapName(%this) == "") { 
	GameBase::setMapName (%this, "20mm Cannon Turret"); 
} 
} 

function DeployablePlasma::deploy(%this) { 
	GameBase::playSequence(%this,1,"deploy"); 
} 

function DeployablePlasma::onEndSequence(%this,%thread) { 
	// GameBase::setActive(%this,true); 
} 

function DeployablePlasma::onDestroyed(%this) { 
	StaticShape::objectiveDestroyed(%this); 
	%this.shieldStrength = 0; 
	GameBase::setRechargeRate(%this,0); 
	Turret::onDeactivate(%this); 
	Turret::objectiveDestroyed(%this); 
	CalcRadiusDamage(%this,$DebrisDamageType,20,0.2,25,20,20,2.5,1.1,200,100); 
	$TeamItemCount[GameBase::getTeam(%this) @ "TwentyPack"]--;
	$TurretControl[%this] = 0; 
} 

function DeployablePlasma::onPower(%this,%power,%generator) {
	Turret::checkOperator(%this);
} 

function DeployablePlasma::onEnabled(%this) { 
	%this.shieldStrength = 0.03;
	GameBase::setRechargeRate(%this,0); 
	// GameBase::setActive(%this,true); 
} 

// ---------------------------------------------------------------------

TurretData DeployableMortar { 
      className = "Turret"; 
      shapeFile = "mortar_turret"; 
      projectileType = HowitzerShell; 
      maxDamage = 2.0; 
      maxEnergy = 45; 
      minGunEnergy = 1; 
	maxGunEnergy = 5; 
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor }; 
	reloadDelay = 4.0; 
	speed = 2.0; 
	speedModifier = 1.5; 
	range = 0; 
	visibleToSensor = true; 
	shadowDetailMask = 4; 
	dopplerVelocity = 0; 
	castLOS = true; 
	supression = false; 
	mapFilter = 2; 
	mapIcon = "M_turret"; 
	debrisId = flashDebrisMedium; 
	shieldShapeName = "shield"; 
	fireSound = SoundFireMortar; 
	activationSound = SoundMortarTurretOn; 
	deactivateSound = SoundMortarTurretOff; 
	whirSound = SoundMortarTurretTurn; 
	explosionId = LargeShockwave; 
	description = "Howitzer"; 
	damageSkinData = "objectDamageSkins"; 
}; 

function DeployableMortar::onAdd(%this) { 
	schedule("DeployableMortar::deploy(" @ %this @ ");",1, this); 
	GameBase::setRechargeRate(%this,0); 
	%this.shieldStrength = 0.005; 
	if (GameBase::getMapName(%this) == "") { 
		GameBase::setMapName (%this, "Howitzer"); 
	} 
} 

function DeployableMortar::deploy(%this) { 
	GameBase::playSequence(%this,1,"deploy"); 
} 

function DeployableMortar::onEndSequence(%this,%thread) { 	
	GameBase::setActive(%this,true);
} 

function DeployableMortar::onDestroyed(%this) { 
	Turret::onDestroyed(%this); 
	$TeamItemCount[GameBase::getTeam(%this) @ "HowitzerPack"]--;
	$TurretControl[%this] = 0; 
} 

function DeployableMortar::onPower(%this,%power,%generator) {
	Turret::checkOperator(%this);
} 

function DeployableMortar::onEnabled(%this) { 
	GameBase::setRechargeRate(%this,0);
	GameBase::setActive(%this,true); 
} 

// END DELTA FORCE ADDED TURRETS
// -----------------------------------------------------

//----------------------------------------------------------------------------
// TURRET DYNAMIC DATA

TurretData PlasmaTurret
{
	maxDamage = 2.0;
	maxEnergy = 500;
	minGunEnergy = 1;
	maxGunEnergy = 3;
	reloadDelay = 0.1;
	fireSound = SoundMortarTurretFire;
	activationSound = SoundPlasmaTurretOn;
	deactivateSound = SoundPlasmaTurretOff;
	whirSound = SoundPlasmaTurretTurn;
	range = 100;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	visibleToSensor = true;
	debrisId = defaultDebrisMedium;
	className = "Turret";
	shapeFile = "hellfiregun";
	shieldShapeName = "shield_medium";
	speed = 2.0;
	speedModifier = 2.0;
	projectileType = NATOBullet;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 8;
	explosionId = LargeShockwave;
	description = "20mm Cannon";
};

// DELTA FORCE

function PlasmaTurret::onPower(%this,%power,%generator)
{
	if (%power) {
		%this.shieldStrength = 0.03;
		GameBase::setRechargeRate(%this,0);
	}
	else {
		%this.shieldStrength = 0;
		GameBase::setRechargeRate(%this,0);
		Turret::checkOperator(%this);
	}
	// GameBase::setActive(%this,%power);
}

function PlasmaTurret::onEnabled(%this)
{
	if (GameBase::isPowered(%this)) {
		%this.shieldStrength = 0.03;
		GameBase::setRechargeRate(%this,0);
		// GameBase::setActive(%this,true);
	}
}

// END DELTA FORCE

TurretData RocketTurret
{
	maxDamage = 0.75;
	maxEnergy = 100;
	minGunEnergy = 60;
	maxGunEnergy = 60;
	range = 180;
	gunRange = 500;
	visibleToSensor = true;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = defaultDebrisLarge;
	className = "Turret";
	shapeFile = "missileturret";
	shieldShapeName = "shield_medium";
	speed = 2.0;
	speedModifier = 2.0;
	projectileType = TurretMissile;
//	reloadDelay = 3.5;
	fireSound = SoundMissileTurretFire;
	activationSound = SoundMissileTurretOn;
	deactivateSound = SoundMissileTurretOff;
//	whirSound = SoundMissileTurretTurn;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 8;
   targetableFovRatio = 0.5;
	explosionId = LargeShockwave;
	description = "SAM Emplacement";
};

function RocketTurret::onPower(%this,%power,%generator)
{
	if (%power) {
		%this.shieldStrength = 0.03;
		GameBase::setRechargeRate(%this,14);
	}
	else {
		%this.shieldStrength = 0;
		GameBase::setRechargeRate(%this,0);
		Turret::checkOperator(%this);
	}
	GameBase::setActive(%this,%power);
}

function RocketTurret::verifyTarget(%this,%target)
{
   if (GameBase::virtual(%target, "getHeatFactor") >= 0.5){
//	if($targetcheck[%this, %target] && $targetcheck[%this, %target] == true){
		$SamTarget = GameBase::getOwnerClient(%target);
		return "True";
//	}
//	else {
//	 	$targetcheck[%this, %target] = true;
//	     	Client::sendMessage(Player::getClient(%target),0,"** WARNING ** - an enemy SAM is obtaining a Missile Lock!~waccess_denied.wav");
//		schedule("Client::sendMessage(" @ Player::getClient(%target) @ ",0,\"~waccess_denied.wav\");",0.5);
//		schedule("Client::sendMessage(" @ Player::getClient(%target) @ ",0,\"~waccess_denied.wav\");",1.0);
//		schedule("Client::sendMessage(" @ Player::getClient(%target) @ ",0,\"~waccess_denied.wav\");",1.5);
//		schedule("$targetcheck[" @ %this @ ", " @ %target @ "] = false;", 4);
//		return "True";
//	}
   }
   else
      return "False";
}

//--------------------------------------------

TurretData MortarTurret
{
	maxDamage = 1.0;
	maxEnergy = 45;
	minGunEnergy = 1;
	maxGunEnergy = 5;
	reloadDelay = 4.0;
	fireSound = SoundFireMortar;
	activationSound = SoundMortarTurretOn;
	deactivateSound = SoundMortarTurretOff;
	whirSound = SoundMortarTurretTurn;
	range = 0;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	visibleToSensor = true;
	debrisId = defaultDebrisMedium;
	className = "Turret";
	shapeFile = "mortar_turret";
	shieldShapeName = "shield_medium";
	speed = 2.0;
	speedModifier = 2.0;
	projectileType = HowitzerShell;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 8;
	explosionId = LargeShockwave;
	description = "Howitzer";
};
					
function MortarTurret::onPower(%this,%power,%generator)
{
	if (%power) {
		%this.shieldStrength = 0.03;
		GameBase::setRechargeRate(%this,0);
	}
	else {
		%this.shieldStrength = 0;
		GameBase::setRechargeRate(%this,0);
		Turret::checkOperator(%this);
	}
	GameBase::setActive(%this,%power);
}

function MortarTurret::onEnabled(%this)
{
	if (GameBase::isPowered(%this)) {
		%this.shieldStrength = 0.03;
		GameBase::setRechargeRate(%this,0);
		GameBase::setActive(%this,true);
	}
}
																	 
//--------------------------------------------
//Gone

//TurretData IndoorTurret
//{
//	className = "Turret";
//	shapeFile = "indoorgun";
//	projectileType = SAWBullet;
//	maxDamage = 2.0;
//	maxEnergy = 60;
//	minGunEnergy = 20;
//	maxGunEnergy = 6;
//	reloadDelay = 0.4;
//	speed = 5.0;
//	speedModifier = 1.0;
//	range = 25;
//	visibleToSensor = true;
//	dopplerVelocity = 2;
//	castLOS = true;
//	supression = false;
//	supressable = false;
//	pinger = false;
//	mapFilter = 2;
//	mapIcon = "M_turret";
//	debrisId = defaultDebrisMedium;
//	shieldShapeName = "shield";
//	fireSound = SoundMortarTurretFire;
//	activationSound = SoundEnergyTurretOn;
//	deactivateSound = SoundEnergyTurretOff;
//	damageSkinData = "objectDamageSkins";
//	shadowDetailMask = 8;
//	explosionId = debrisExpMedium;
//	description = "Indoor Machine Gun";
//
//};


//--------------------------------------------

TurretData DeployableTurret
{
	className = "Turret";
	shapeFile = "remoteturret";
	projectileType = SAWBullet;
	maxDamage = 1.0;
	maxEnergy = 60;
	minGunEnergy = 6;
	maxGunEnergy = 5;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	reloadDelay = 0.6;
	speed = 4.0;
	speedModifier = 1.5;
	range = 20;
	visibleToSensor = true;
	shadowDetailMask = 4;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = flashDebrisMedium;
	shieldShapeName = "shield";
	fireSound = SoundMortarTurretFire;
	activationSound = SoundRemoteTurretOn;
	deactivateSound = SoundRemoteTurretOff;
	explosionId = flashExpMedium;
	description = "Remote Machine Gun";
	damageSkinData = "objectDamageSkins";
};

function DeployableTurret::onAdd(%this)
{
	schedule("DeployableTurret::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,5);
	%this.shieldStrength = 0;
	if (GameBase::getMapName(%this) == "") {
		GameBase::setMapName (%this, "Remote Machine Gun");
	}
}

function DeployableTurret::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function DeployableTurret::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function DeployableTurret::onDestroyed(%this)
{
	Turret::onDestroyed(%this);
  	$TeamItemCount[GameBase::getTeam(%this) @ "TurretPack"]--;
}

// Override base class just in case.
function DeployableTurret::onPower(%this,%power,%generator) {}
function DeployableTurret::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);
}	

// ---------------------------------------------------

TurretData TripodGun { 
	className = "Turret"; 
	shapeFile = "remoteturret"; 
	projectileType = SAWBullet; 
	maxDamage = 3.0; 
	maxEnergy = 300; 
	minGunEnergy = 1; 
	maxGunEnergy = 3; 
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor }; 
	reloadDelay = 0.08; 
	speed = 2.0; 
	speedModifier = 1.5; 
	range = 100; 
	visibleToSensor = false; 
	shadowDetailMask = 4; 
	dopplerVelocity = 0; 
	castLOS = true; 
	supression = false; 
	mapFilter = 2; 
	mapIcon = "M_turret"; 
	debrisId = flashDebrisSmall; 
	shieldShapeName = "shield"; 
	fireSound = SoundMortarTurretFire; 
	activationSound = SoundPlasmaTurretOn; 
	deactivateSound = SoundPlasmaTurretOff; 
	whirSound = SoundPlasmaTurretTurn; 
	explosionId = flashExpSmall; 
	description = "M240G Machine Gun"; 
	damageSkinData = "objectDamageSkins"; 
}; 

function TripodGun::onAdd(%this) { 
	schedule("TripodGun::deploy(" @ %this @ ");",1,%this); 
	GameBase::setRechargeRate(%this,0); 
	%this.shieldStrength = 0.0; 
	if (GameBase::getMapName(%this) == "") { 
	//GameBase::setMapName (%this, "20mm Cannon Turret"); 
} 
} 

function TripodGun::deploy(%this) { 
	GameBase::playSequence(%this,1,"deploy"); 
} 

function TripodGun::onEndSequence(%this,%thread) { 
	// GameBase::setActive(%this,true); 
} 

function TripodGun::onDestroyed(%this) { 
	//StaticShape::objectiveDestroyed(%this); 
	%this.shieldStrength = 0; 
	GameBase::setRechargeRate(%this,0); 
	Turret::onDeactivate(%this); 
	Turret::objectiveDestroyed(%this); 
	CalcRadiusDamage(%this,$DebrisDamageType,20,0.2,25,20,20,2.5,1.1,200,100); 
	$TeamItemCount[GameBase::getTeam(%this) @ "TripodPack"]--;
	$TurretControl[%this] = 0; 
} 

function TripodGun::onPower(%this,%power,%generator) {
	Turret::checkOperator(%this);
} 

function TripodGun::onEnabled(%this) { 
	%this.shieldStrength = 0.03;
	GameBase::setRechargeRate(%this,0); 
	// GameBase::setActive(%this,true); 
} 

//--------------------------------------------

TurretData CameraTurret
{
	className = "Turret";
	shapeFile = "camera";
	maxDamage = 0.25;
	maxEnergy = 10;
	speed = 20;
	speedModifier = 1.0;
	range = 50;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	visibleToSensor = true;
	shadowDetailMask = 4;
	castLOS = true;
	supression = false;
	supressable = false;
	mapFilter = 2;
	mapIcon = "M_camera";
	debrisId = defaultDebrisSmall;
	FOV = 0.707;
	pinger = false;
	explosionId = debrisExpMedium;
	description = "Camera";
};

function CameraTurret::onAdd(%this)
{
	schedule("CameraTurret::deploy(" @ %this @ ");",1,%this);
	if (GameBase::getMapName(%this) == "") {
		GameBase::setMapName (%this, "Camera");
	}
}

function CameraTurret::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function CameraTurret::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function CameraTurret::onDestroyed(%this)
{
	Turret::onDestroyed(%this);
  	$TeamItemCount[GameBase::getTeam(%this) @ "CameraPack"]--;
}	


//---------------------------------------------------

function Turret::onAdd(%this)
{
	if (GameBase::getMapName(%this) == "") {
		GameBase::setMapName (%this, "Turret");
	}
}

function Turret::onActivate(%this)
{
	GameBase::playSequence(%this,0,power);
}

function Turret::onDeactivate(%this)
{
	GameBase::stopSequence(%this,0);
	Turret::checkOperator(%this);
}

function Turret::onSetTeam(%this,%oldTeam)
{
	if(GameBase::getTeam(%this) != Client::getTeam(GameBase::getControlClient(%this))) 
		Turret::checkOperator(%this);

}

function Turret::checkOperator(%this)
{
	%cl = GameBase::getControlClient(%this);
	if(%cl != -1) {
		%pl = Client::getOwnedObject(%cl);
		Player::setMountObject(%pl, -1,0);
		Client::setControlObject(%cl, %pl);
   	}
   	Client::setGuiMode(%cl,2);
	if(GameBase::getDataName(%this) == MortarTurret || GameBase::getDataName(%this) == DeployableMortar) {
		GameBase::startFadeIn(%cl);
	}
}

function Turret::onPower(%this,%power,%generator)
{
	if (%power) {
		%this.shieldStrength = 0.03;
		if(GameBase::getDataName(%this) != PlasmaTurret) {
			GameBase::setRechargeRate(%this,10);
		} else {
			GameBase::setRechargeRate(%this,0);
		}
	}
	else {
		%this.shieldStrength = 0;
		GameBase::setRechargeRate(%this,0);
		Turret::checkOperator(%this);
	}
	if(GameBase::getDataName(%this) != PlasmaTurret) {
		GameBase::setActive(%this,%power);
	}
}

function Turret::onEnabled(%this)
{
	if (GameBase::isPowered(%this)) {
		%this.shieldStrength = 0.03;
		if(GameBase::getDataName(%this) != PlasmaTurret) {
			GameBase::setRechargeRate(%this,10);
			GameBase::setActive(%this,true);
		} else {
			GameBase::setRechargeRate(%this,0);
		}
	}
}

function Turret::onDisabled(%this)
{
	%this.shieldStrength = 0;
	GameBase::setRechargeRate(%this,0);
	Turret::onDeactivate(%this);
}

function Turret::onDestroyed(%this)
{
	StaticShape::objectiveDestroyed(%this);
	%this.shieldStrength = 0;
	GameBase::setRechargeRate(%this,0);
	Turret::onDeactivate(%this);
	Turret::objectiveDestroyed(%this);
	calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 9, 3, 0.40, 
		0.1, 200, 100);
	$TurretControl[%this] = 0; 
}

function Turret::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
   if(%this.objectiveLine)
		%this.lastDamageTeam = GameBase::getTeam(%object);
	%TDS= 1;
	if(GameBase::getTeam(%this) == GameBase::getTeam(%object)) {
		%name = GameBase::getDataName(%this);
		if(%name != DeployableTurret && %name != CameraTurret )	
			//To fix the TDS
			%TDS = $Server::TeamDamageScale / 100;
	}
	StaticShape::shieldDamage(%this,%type,%value * %TDS,%pos,%vec,%mom,%object);
}

function Turret::onControl (%this, %object)
{
	if(GameBase::getDataName(%this) == DeployableSAM || GameBase::getDataName(%this) == RocketTurret)
		return;
	%client = Player::getClient(%object);
	Client::sendMessage(%client,0,"Controlling turret " @ %this);
	$turretTime[%this] = 1;
}

function Turret::onDismount (%this, %object)
{
echo("Dismount");
	%name = Gamebase::getDataName(%this);
	%client = Player::getClient(%object);
	$TurretControl[%this] = 0;
	Client::sendMessage(%client,0,"Leaving turret " @ %this);
	if (%name == DeployableMortar || %name == MortarTurret) Gamebase::startFadeIn(%object);
	$turretTime[%this] = 0;
	startGracePeriod(%clientId, %player);
	

}

function Turret::onCollision (%this, %object) {
	// AIs can't control turrets...believe me, I've tried
	if(Player::isAIControlled(%object))
		return;
	
	if(getObjectType (%object) == "Player") {
		if($turretTime[%this] != 1)
		{
			%client = Player::getClient(%object); 
			%armor = Player::getArmor(%object); 
			%name = GameBase::getDataName(%this);
			if(GameBase::getDamageLevel(%this) <= (GameBase::getDataName(%this).maxDamage / 2)) { 
				if(GameBase::getTeam(%object) == GameBase::getTeam(%this)) { 
					if($gracePeriod[%client] == 0) {  
						if(%name == DeployablePlasma || %name == DeployableMortar || %name == MortarTurret || %name == PlasmaTurret || %name == DeployableAA || %name == FlakTurret  || %name == ControlledDeployableAA || %name == ControlledFlakTurret) {
							if(%name == DeployableMortar || %name == MortarTurret) {
								if(%armor != "aarmor" && %armor != "afemale" && %armor != "aarmor2" && %armor != "afemale2" && %armor != "aarmor3" && %armor != "afemale3") {
									Client::sendMessage(%client, 0, "Must be an Artillery officer to control a Howitzer.");
									return;
								}
							}
							if(GameBase::GetPosition(%this) != GameBase::GetPosition(%object)) { 
								%pos = Gamebase::Getposition(%this);
								if(%name == FlakTurret || %name == DeployableAA || %name == ControlledFlakTurret || %name == ControlledDeployableAA)
								{
									%rot = GameBase::getRotation(%this);
									if(%name == FlakTurret || %name == ControlledFlakTurret)
										%turret = newObject("hellfiregun","Turret",ControlledFlakTurret,false);
									else
										%turret = newObject("hellfiregun","Turret",ControlledDeployableAA,true);
									%group = GetGroup(%this);
									addToSet(%group, %turret); 
									GameBase::setTeam(%turret,GameBase::getTeam(%this)); 
									GameBase::setPosition(%turret,%pos); 
									GameBase::setRotation(%turret,%rot); 
									Gamebase::setMapName(%turret,GameBase::getMapName(%this));
									GameBase::setenergy(%turret,GameBase::getEnergy(%this));
									deleteobject(%this);
									%this = %turret;
									%name = GameBase::getDataName(%this);
								}
								%object.turretcontrol = %this;
								$TurretControl[%this] = %client;
								Client::getOwnedObject(%client).CommandTag = 1;  
								Client::takeControl(%client, %this);
								
								if (%name == DeployablePlasma || %name == PlasmaTurret || %name == ControlledDeployableAA || %name == ControlledFlakTurret) { 
									GameBase::setPosition(%object,vector::add(%pos,"0 0 0.1"));
								} else { // Mortar turrets
									Gamebase::startFadeOut(%object);
									%vec = Vector::getFromRot( GameBase::getRotation(%this), 3 );
									%vec = Vector::neg(%vec);
									%pos = GameBase::getPosition(%this);
									%newPos = getWord(%vec, 0) + getWord(%pos, 0) @ " " @ getWord(%vec, 1) + getWord(%pos, 1) @ " " @ getWord(%vec, 2) + getWord(%pos, 2);
									GameBase::setPosition(%object, %newPos);
									GameBase::setPosition(%object,vector::add(%newpos,"0 0 0.1"));
								
								}

								return;
							} 
						} 
					} else Client::sendMessage(%client,0,"Turret is cooling down."); 
				} else Client::sendMessage(%client,0,"--ACCESS DENIED-- Wrong Team ~waccess_denied.wav");
			} else Client::sendMessage(%client,0,"Turret is non-functional.");
		} 
	}
}

function startGracePeriod(%clientId, %player) { 
	Player::unmountItem(%player,$WeaponSlot);
	if($gracePeriod[%clientId] == 0) {
		%this = Client::getOwnedObject(%clientId).Turretcontrol;
		DecomissionTurret(%this);
		Client::getOwnedObject(%clientId).CommandTag = ""; 
		$gracePeriod[%clientId] = 1; 
		schedule("$gracePeriod["@%clientId@"] = 0;",6);
	}
}

function DecomissionTurret(%this)
{ 
	%name = Gamebase::getdataname(%this);
	if(%name == ControlledFlakTurret || %name == ControlledDeployableAA)
	{
		
		%pos = Gamebase::Getposition(%this);
		%rot = GameBase::getRotation(%this); 
		if ( %name == ControlledFlakTurret )
			%turret = newObject("hellfiregun","Turret",FlakTurret,false); 
		else
			%turret = newObject("hellfiregun","Turret",DeployableAA,true); 
		%group = GetGroup(%this);
		addToSet(%group, %turret); 
		GameBase::setTeam(%turret,GameBase::getTeam(%this)); 
		GameBase::setPosition(%turret,%pos); 
		GameBase::setRotation(%turret,%rot); 
		Gamebase::setMapName(%turret,GameBase::getMapName(%this));
		GameBase::setenergy(%turret,GameBase::getEnergy(%this));
		deleteobject(%this);
		%this = %turret;
	}

}
