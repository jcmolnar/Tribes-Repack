//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Satellite Uplink
//
//  For installation information, see Install.txt
//  Created by <DC/SB> C|one , Orbital Cannon by Edgecrusher
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$TeamItemMax[SatelliteUplinkPack] = 1;
$InvList[SatelliteUplinkPack] = 1;
$RemoteInvList[SatelliteUplinkPack] = 0;
$CanControl[DeployableOrbCannon] = 1;

$CanAlwaysTeamDestroy[DeployableSatelliteUplink] = 1;

function deploySatelliteUplinkPack::Initialize()
{
  $TeamItemCount[0 @ SatelliteUplinkPack] = 0;
  $TeamItemCount[1 @ SatelliteUplinkPack] = 0;
  $TeamItemCount[2 @ SatelliteUplinkPack] = 0;
  $TeamItemCount[3 @ SatelliteUplinkPack] = 0;
  $TeamItemCount[4 @ SatelliteUplinkPack] = 0;
  $TeamItemCount[5 @ SatelliteUplinkPack] = 0;
  $TeamItemCount[6 @ SatelliteUplinkPack] = 0;
  $TeamItemCount[7 @ SatelliteUplinkPack] = 0;
}

LaserData OrbitalShot
{
  laserBitmapName   = "warp.bmp";
   hitName           = "laserhit.dts";

   damageConversion  = 0.055;
   baseDamageType    = $LaserDamageType;

   beamTime          = 1.0;

   lightRange        = 1.0;
   lightColor        = { 0.0, 1.25, 1.25 };

   detachFromShooter = false;
   hitSoundId        = SoundLaserHit;
};

function OrbitalShot::onAdd(%player,%item)
{
%client = Player::getclient(%player);
      bottomprint(%client, "<f0>Orbital Cannon<f1>\nFired.");
}


ItemImageData SatelliteUplinkPackImage 
{
  shapeFile = "magcargo";
  mountPoint = 2;
  mountOffset = { 0, 0, 0 };
  mountRotation = { 3.14, 0, 0 };
  firstPerson = false;
};

ItemData SatelliteUplinkPack 
{
  description = "Satellite Uplink";
  shapeFile = "magcargo";
  className = "Backpack";
  heading = $InvHead[ihDSe];
  imageType = SatelliteUplinkPackImage;
  shadowDetailMask = 4;
  mass = 2.0;
  elasticity = 0.2;
  price = 2500;
  hudIcon = "deployable";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function SatelliteUplinkPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::deployItem(%player,%item);
}

function SatelliteUplinkPack::onDeploy(%player,%item,%pos) 
{
  if (SatelliteUplinkPack::deployShape(%player,%item)) 
  {
bottomPrintAll("***WARNING: Null-Frequency Transmission Detected***", 10);
    Player::decItemCount(%player,SatelliteUplinkPack);
    $TeamItemCount[GameBase::getTeam(%player) @ "SatelliteUplinkPack"]++;
  }
}

function SatelliteUplinkPack::deployShape(%player,%item) 
{
  %client = Player::getClient(%player);
  if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]) 
  {
    if (GameBase::getLOSInfo(%player,3)) 
    {
      %obj = getObjectType($los::object);
      if (%obj == "SimTerrain") 
      {
        %prot = GameBase::getRotation(%player);
        %zRot = getWord(%prot,2);
        if (Vector::dot($los::normal,"0 0 1") > 1.6) 
          %rot = "0 0 " @ %zRot;
        else 
        {
          if (Vector::dot($los::normal,"0 0 -1") > 1.6) 
            %rot = "3.14159 0 " @ %zRot;
          else 
            %rot = Vector::getRotation($los::normal);
        }
        if(checkDeployArea(%client,$los::position)) 
        {
          %obj = newObject("","StaticShape",DeployableSatelliteUplink,true);
	  %team = GameBase::getTeam(%player);	  
          GameBase::setTeam(%obj,%team);
          addToSet("MissionCleanup", %obj);
          GameBase::setPosition(%obj,$los::position);
          GameBase::setRotation(%obj,%rot);
          Gamebase::setMapName(%obj,"Satellite Uplink");
          playSound(SoundPickupBackpack,$los::position);
	  %obj.disabled = false;
//          reportDeploy(%obj, %client);
		echo("MSG: ",%client," deployed a Satellite Uplink");
          Client::sendMessage(%client,0,"Satellite Uplink deployed");

          %obj2 = newObject("","Sensor",DeployableSatellite,true);
          %pos2 = Vector::add(GameBase::getPosition(%player), "0 0 450");
	  addToSet("MissionCleanup", %obj2); 
	  GameBase::setTeam(%obj2,%team); 
	  GameBase::setPosition(%obj2,%pos2); 
	  GameBase::setRotation(%obj2,"3.1416 0 0"); 
	  Gamebase::setMapName(%obj2,"Satellite"); 
	  playSound(SoundPickupBackpack,%pos2); 
//	    reportDeploy(%obj2,%client); 
	        echo("MSG: ",%client," deployed a Satellite");
	  %obj2.disabled = false; 
	  %obj.satellite = %obj2; 
	//  Client::sendMessage(%client,0,"Satellite launched"); 

          %obj3 = newObject("","Turret",DeployableOrbCannon,true);
          %pos3 = Vector::add(GameBase::getPosition(%player), "0 0 25");
	  addToSet("MissionCleanup", %obj3); 
	  GameBase::setTeam(%obj3,%team); 
	  GameBase::setPosition(%obj3,%pos3); 
	  GameBase::setRotation(%obj3,"3.1416 0 0"); 
	  Gamebase::setMapName(%obj3,"Satellite Defense Unit"); 
	  playSound(SoundPickupBackpack,%pos3); 
//	    reportDeploy(%obj3,%client); 
		echo("MSG: ",%client," deployed a Satellite Defense Unit");
	  %obj3.disabled = false; 
	  %obj2.orbcannon = %obj3; 
	  %obj.orbcannon = %obj3; 
	  %team.orbitalCannon = %obj3;
	//  Client::sendMessage(%client,0,"Satellite Defense Unit launched"); 

	  return true;
        }
      }
      else 
        Client::sendMessage(%client,0,"Can only deploy on terrain");
    }
    else 
      Client::sendMessage(%client,0,"Deploy position out of range");
  }
  else 
    Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
  return false;
}


SensorData DeployableSatellite
{
	description = "Satellite";
	className = "DeployableSensor";
	shapeFile = "radar_small";
	shadowDetailMask = 4;
	visibleToSensor = true;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	damageLevel = {0.8, 1.0};
	maxDamage = 1.0;
	debrisId = flashDebrisSmall;
	range = 785;
	castLOS = true;
	supression = false;
	mapFilter = 4;
	mapIcon = "M_Radar";
};

function DeployableSatelliteUplink::onDestroyed(%this) 
{
  Turret::onDestroyed(%this);
  schedule("deleteObject("@%this.orbcannon@");",0.2); 
}

TurretData DeployableOrbCannon 
{
  className = "Turret";
  shapeFile = "camera";
  projectileType = OrbitalShot;
  maxDamage = 2.5;
  maxEnergy = 70;
  minGunEnergy = 10;
  maxGunEnergy = 25;
  sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
  reloadDelay = 3.0;
  speed = 2.0;
  speedModifier = 2.0;
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
  activationSound = SoundMortarTurretOn;
  deactivateSound = SoundMortarTurretOff;
  whirSound = SoundMortarTurretTurn;
  explosionId = debrisExpLarge;
  description = "Orbital Cannon";
  damageSkinData = "objectDamageSkins";
};

StaticShapeData DeployableSatelliteUplink
{
	shapeFile = "sat_big";
	visibleToSensor = true;
	maxDamage = 6.5;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 4;
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	description = "Satellite Uplink";
	mapFilter = 4;
	sfxAmbient = SoundSensorPower;
	mapIcon = "M_generator";
};

function DeployableSatelliteUplink::onAdd(%this)
{
  schedule("DeployableSatelliteUplink::onEndSequence(" @ %this @ ");",1,%this);
  GameBase::setRechargeRate(%this,10);
  %this.shieldStrength = 0.003;
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Satellite Uplink");
}

function DeployableSatelliteUplink::onEndSequence(%this,%thread) 
{
  GameBase::setActive(%this,true);
}

function DeployableSatelliteUplink::onDestroyed(%this) 
{
bottomPrintAll("***Satellite Eliminated***", 10);
  $TeamItemCount[GameBase::getTeam(%this) @ "SatelliteUplinkPack"]--;
  Turret::onDestroyed(%this);
  schedule("deleteObject("@%this.satellite@");",0.2); 
  schedule("deleteObject("@%this.orbcannon@");",0.2); 
}

function DeployableSatelliteUplink::onPower(%this,%power,%generator) 
{
}

function DeployableSatelliteUplink::onEnabled(%this)
{
  GameBase::setRechargeRate(%this,10);
  GameBase::setActive(%this,true);
}

function DeployableOrbCannon::onAdd(%this) 
{
  schedule("DeployableOrbCannon::deploy(" @ %this @ ");",1,%this);
  GameBase::setRechargeRate(%this,7);
  %this.shieldStrength = 0.005;
  if (GameBase::getMapName(%this) == "") 
    GameBase::setMapName (%this, "Satellite Defense Unit");
}

function DeployableOrbCannon::deploy(%this) 
{
  GameBase::playSequence(%this,1,"deploy");
}

function DeployableOrbCannon::onEndSequence(%this,%thread) 
{
  GameBase::setActive(%this,true);
}

function DeployableOrbCannon::onEnabled(%this) 
{
  GameBase::setRechargeRate(%this,7);
  GameBase::setActive(%this,true);
}

function DeployableOrbCannon::onDestroyed(%this) 
{
  GameBase::getTeam(%this).orbitalCannon = -1;
  Turret::onDestroyed(%this);
}

function DeployableOrbCannon::setTarget(%object, %arg1) 
{
	GameBase::setIsTarget(%object, %arg1);
}