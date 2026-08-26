
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Tractor Device (TractorDevice)
//  By Alazane,
//    see Contrib.txt
//
//  For installation information, see Install.txt
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$InvList[TractorDevice] = 1;
$RemoteInvList[TractorDevice] = 1;
$AutoUse[TractorDevice] = False;
$WeaponAmmo[TractorDevice] = "";

addWeapon(TractorDevice);

LightningData TractorBeam
{
   bitmapName       = "lightningNew.bmp";

   damageType       = $ElectricityDamageType;
   boltLength       = 100.0; // 120
   coneAngle        = 35.0;
   damagePerSec      = 0.0;
   energyDrainPerSec = 0.0;
   segmentDivisions = 5;
   numSegments      = 1;
   beamWidth        = 0.5;//0.125//075;

   updateTime   = 120;
   skipPercent  = 0.25;
   displaceBias = 0.15;

   lightRange = 3.0;
   lightColor = { 0.85, 0.85, 0.15 };

   soundId = SoundELFFire;
};

$TractorPower = 50;

function TractorBeam::damageTarget(%target, %timeSlice, %damPerSec, %enDrainPerSec, %pos, %vec, %mom, %shooterId) 
{
  %obj1 = %target;
  %obj2 = %shooterId;

// Get object's mass  
  if (getObjectType(%obj1) == "Player") %obj1mass = Player::getArmor(%obj1).mass;
  else if(getObjectType(%obj1) == "Mine") %obj1mass = GameBase::getDataName(%obj1).mass;
  else %obj1mass = 1000;
  %obj2mass = Player::getArmor(%obj2).mass;
  %vec = Vector::Normalize(Vector::Sub(GameBase::getPosition(%obj1), GameBase::getPosition(%obj2)));

  if (%obj1mass > %obj2mass)
  {
    %mul = $TractorPower - ($TractorPower * %obj2mass) / (%obj1mass + %obj2mass);
    %nvec = (getWord(%vec, 0) * %mul) @ " " @
            (getWord(%vec, 1) * %mul) @ " " @
            (getWord(%vec, 2) * %mul);
    Item::setVelocity(%obj2, %nvec);
  }

  else
  {
    %mul = $TractorPower - ($TractorPower * %obj1mass) / (%obj1mass + %obj2mass);
    %nvec = (getWord(%vec, 0) * %mul * -1) @ " " @
            (getWord(%vec, 1) * %mul * -1) @ " " @
            (getWord(%vec, 2) * %mul * -1);
    Item::setVelocity(%obj1, %nvec);
  }

//  if (getObjectType(%obj1) == "Player")
//  {
//    %mul = $TractorPower - ($TractorPower * %obj1mass) / (%obj1mass + %obj2mass);
//    %nvec = (getWord(%vec, 0) * %mul * -1) @ " " @
//            (getWord(%vec, 1) * %mul * -1) @ " " @
//            (getWord(%vec, 2) * %mul * -1);
//    Item::setVelocity(%obj1, %nvec);
//  }
// obj2 is always a player
//  %mul = $TractorPower - ($TractorPower * %obj2mass) / (%obj1mass + %obj2mass);
//  %nvec = (getWord(%vec, 0) * %mul) @ " " @
//          (getWord(%vec, 1) * %mul) @ " " @
//          (getWord(%vec, 2) * %mul);
//  Item::setVelocity(%obj2, %nvec);
}

ItemImageData TractorDeviceImage
{
   shapeFile = "shieldpack";
   mountPoint = 0;

   weaponType = 2;  // Sustained
   projectileType = TractorBeam;
   minEnergy = 3;
   maxEnergy = 11;  // Energy used/sec for sustained weapons
   reloadTime = 0.2;
			
   lightType = 3;  // Weapon Fire
   lightRadius = 2;
   lightTime = 1;
   lightColor = { 0.85, 0.85, 0.15 };

   sfxActivate = SoundPickUpWeapon;
   sfxFire     = SoundELFIdle;
};

ItemData TractorDevice
{
  className = "Tool";
  description = "Grav Beam";
  heading = $InvHead[ihTls];
  hudIcon = "energyRifle";
  imageType = TractorDeviceImage;
  price = 12;
  shadowDetailMask = 4;
  shapeFile = "shieldpack";
  showWeaponBar = true;
};

function TractorDevice::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Grav Beam<f1>\nNot a weapon, but a tool for rapid movement of Tech armors. Has other interesting uses.");
}


//-=-=-=-=-=-=-=-=-=-=-=-=-=
// Med Gun
//-=-=-=-=-=-=-=-=-=-=-=-=-=-

$InvList[ApothHeal] = 1;
$RemoteInvList[ApothHeal] = 1;
$AutoUse[ApothHeal] = False;

addWeapon(ApothHeal);


ItemImageData ApothHealImage 
{
  shapeFile = "repairgun";
  mountPoint = 0;
  weaponType = 2;
  projectileType = Apoth;
  minEnergy = 5;
  maxEnergy = 10;
  lightType = 3;
  lightRadius = 1;
  lightTime = 1;
  lightColor = { 1.00, 0.25, 0.25 };
  sfxFire = SoundRepairItem;
  sfxActivate = SoundPickUpWeapon;
};

ItemData ApothHeal 
{
  description = "Med Gun";
  className = "Tool";
  shapeFile = "repairgun";
  hudIcon = "targetlaser";
  heading = $InvHead[ihTls];
  shadowDetailMask = 4;
  imageType = ApothHealImage;
  price = 5;
  showWeaponBar = false;
};

function ApothHeal::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Med Gun<f1>\nUsed to heal friendly units.");
}

//-=-=-=-=-=-=-=--=-=
// Tech Gun
//-=-=-=-=-=-=-=-=-=-=
$InvList[Fixit] = 1;
$RemoteInvList[Fixit] = 1;
$AutoUse[Fixit] = False;

addWeapon(Fixit);

RepairEffectData FixitBolt 
{
  bitmapName = "lightningTemp.bmp";
  boltLength = 40.0;
  segmentDivisions = 4;
  beamWidth = 0.225;
  updateTime = 450;
  skipPercent = 0.6;
  displaceBias = 0.15;
  lightRange = 3.0;
  lightColor = { 0.85, 0.25, 0.25 };
};

function FixitBolt::onAcquire(%this, %player, %target) 
{
  %client = Player::getClient(%player);
  if (%target == %player) 
  {
    %player.repairTarget = -1;
    if (GameBase::getDamageLevel(%player) != 0) 
    {
      %player.repairRate = 0.0;
      %player.repairTarget = %player;
      Client::sendMessage(%client, 0, "Attempting repair...failed. This is a living object.");
      Player::trigger(%player,$WeaponSlot,false);
	return;
    }
    else 
    {
      Client::sendMessage(%client,0,"No objects seem to be in range.");
      Player::trigger(%player, $WeaponSlot, false);
      return;
    }
  }
  else 
  {
    %player.repairTarget = %target;
    %player.repairRate = 0.55;
    if (getObjectType(%player.repairTarget) == "Player") 
    {
      %rclient = Player::getClient(%player.repairTarget);
      %name = Client::getName(%rclient);
    }
    else 
    {
      %name = GameBase::getMapName(%target);
      if(%name == "") 
      {
        %name = (GameBase::getDataName(%player.repairTarget)).description;
      }
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
	%player.repairRate = 0.0;
      Client::sendMessage(%client, 0, "Attempting repair...failed. This is a living object.");
      Player::trigger(%player,$WeaponSlot,false);
	return;
    }
    Client::sendMessage(%client,0,"Healing " @ %name);
  }
  %rate = GameBase::getAutoRepairRate(%player.repairTarget) + %player.repairRate;
  GameBase::setAutoRepairRate(%player.repairTarget,%rate);
}

function FixitBolt::onRelease(%this, %player) 
{
  %object = %player.repairTarget;
  %type = GameBase::getDataName(%object);
  if (%object != -1) 
  {
    %client = Player::getClient(%player);
    if (%object == %player) 
    {
      Client::sendMessage(%client,0,"Stopped Repairing");
    }
    else 
    {
      if (GameBase::getDamageLevel(%object) == 0) 
      {
		Client::sendMessage(%client,0,"Repair Done");

		%fixpoints = (floor(%type.maxdamage - (%type.maxdamage - %object.mindamage)));
		if(%fixpoints < 1) %fixpoints = 1;
		%object.mindamage = 0;
	      %playerClient = GameBase::getControlClient(%object.lastDamageObject);
	   	if(%client != %playerClient)
		{
			if(GameBase::getTeam(%object) == GameBase::getTeam(%client))
			{
				if (GameBase::getDataName(%this).mapFilter != -1)
				{
					%client.score = %client.score + %fixpoints;
					bottomprint(%client, "<f0>Score:<f1> +" @ %fixpoints);
					Game::refreshClientScore(%client);
				}
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

function FixitBolt::checkDone(%this, %player) 
{
  if (Player::isTriggered(%player,$WeaponSlot) && Player::getMountedItem(%player,$WeaponSlot) == Fixit && %player.repairTarget != -1) 
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

ItemImageData FixitImage 
{
  shapeFile = "repairgun";
  mountPoint = 0;
  weaponType = 2;
  projectileType = FixitBolt;
  minEnergy = 5;
  maxEnergy = 10;
  lightType = 3;
  lightRadius = 1;
  lightTime = 1;
  lightColor = { 1.00, 0.25, 0.25 };
  sfxFire = SoundRepairItem;
  sfxActivate = SoundPickUpWeapon;
};

ItemData Fixit 
{
  description = "Tech Repair Pistol";
  className = "Tool";
  shapeFile = "repairgun";
  hudIcon = "targetlaser";
  heading = $InvHead[ihTls];
  shadowDetailMask = 4;
  imageType = FixitImage;
  price = 10;
  showWeaponBar = false;
};

function Fixit::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Tech Repair Pistol<f1>\nRepairs non-living objects at an excellent rate.");
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//  Mine Launcher
//  Thanks to Minimod for this one!
//
//  For installation information, see Install.txt
//  
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[MineLauncher] = 1;
$RemoteInvList[MineLauncher] = 1;
$AutoUse[MineLauncher] = False;
$WeaponAmmo[MineLauncher] = MinelAmmo;

addWeapon(MineLauncher);

//-=-=-==-The Mines and related Data for Mine Launcher
GrenadeData MineShell
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = debrisExpSmall;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.2;
   mass               = 1.0;
   elasticity         = 0.45;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.0;
   damageType         = $shrapnelDamageType;

   explosionRadius    = 10;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 150;
   totalTime          = 40.0;    // special meaning for grenades...
   liveTime           = 1.0;
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 0.5;

   smokeName              = "smoke.dts";
};

function MineShell::onAdd(%this)
{
	schedule("DeployStuff(" @ %this @ " , 5);",1.0,%this);//5++++++
}

function DeployStuff(%this, %count) 
{
	if(%count && %this)
	{
		%obj = newObject("","Mine","Nuke1");
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj,%this,-6.0,false);//0++++	
//		%obj = newObject("","Mine","Nuke2");
//		addToSet("MissionCleanup", %obj);
//		GameBase::throw(%obj,%this,-15.0,false);
		%obj = newObject("","Mine","Nuke3");
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj,%this,-30.0,false);
		%count -= 1;
		schedule("DeployStuff(" @ %this @ " , " @ %count @ ");",1.0,%this);//0.5++++++++
	}
}

MineData Nuke1
{
   	mass = 5.0;
   	drag = 1.0;
   	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Bomblet";
	shapeFile = "mine";
	shadowDetailMask = 4;
	explosionId = grenadeExp;
	explosionRadius = 10.0;
	damageValue = 0.438;
	damageType = $shrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 1.6;
	maxDamage = 0.5;
};
function Nuke1::onAdd(%this)
{
	%this.damage = 0;
	Nuke1::deployCheck(%this);
}

function Nuke1::onCollision(%this,%object)
{
	%type = getObjectType(%object);
	%data = GameBase::getDataName(%this);
	if ((%type == "Player" || %data == AntipersonelMine || %data == Vehicle || %type == "Moveable") &&
			GameBase::isActive(%this)
			&& (GameBase::getTeam(%this)!=GameBase::getTeam(%object)) //no teamdmg
			) 
		GameBase::setDamageLevel(%this, %data.maxDamage);
}

function Nuke1::deployCheck(%this)
{
	if (GameBase::isAtRest(%this)) {
		GameBase::playSequence(%this,1,"deploy");
	 	GameBase::setActive(%this,true);
		%set = newObject("set",SimSet);
		if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),1,1,1,0)) {
			%data = GameBase::getDataName(%this);
			GameBase::setDamageLevel(%this, %data.maxDamage);
		}
		deleteObject(%set); //
	}
	else 
		schedule("Nuke1::deployCheck(" @ %this @ ");", 3, %this);
}
//------------------
MineData Nuke2
{
   	mass = 5.0;
   	drag = 1.0;
   	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Bomblet";
	shapeFile = "mine";
	shadowDetailMask = 4;
	explosionId = grenadeExp;
	explosionRadius = 10.0;
	damageValue = 0.436;
	damageType = $shrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 1.5;
	maxDamage = 0.5;
};
function Nuke2::onAdd(%this)
{
	%this.damage = 0;
	Nuke2::deployCheck(%this);
        %armor = Player::getArmor(%player);
        %client = Player::getClient(%player);
        GameBase::setTeam (%obj,GameBase::getTeam (%client));
}

function Nuke2::onCollision(%this,%object)
{
	%type = getObjectType(%object);
	%data = GameBase::getDataName(%this);
	if ((%type == "Player" || %data == AntipersonelMine || %data == Vehicle || %type == "Moveable") &&
			GameBase::isActive(%this)
			&& (GameBase::getTeam(%this)!=GameBase::getTeam(%object)) //no teamdmg
			) 
		GameBase::setDamageLevel(%this, %data.maxDamage);
}

function Nuke2::deployCheck(%this)
{
	if (GameBase::isAtRest(%this)) {
		GameBase::playSequence(%this,1,"deploy");
	 	GameBase::setActive(%this,true);
		%set = newObject("set",SimSet);
		if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),1,1,1,0)) {
			%data = GameBase::getDataName(%this);
			GameBase::setDamageLevel(%this, %data.maxDamage);
		}
		deleteObject(%set); //
	}
	else 
		schedule("Nuke2::deployCheck(" @ %this @ ");", 3, %this);
}
//----------------------
MineData Nuke3
{
   	mass = 5.0;
   	drag = 1.0;
   	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Bomblet";
	shapeFile = "mine";
	shadowDetailMask = 4;
	explosionId = grenadeExp;
	explosionRadius = 10.0;
	damageValue = 0.436;
	damageType = $shrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 1.5;
	maxDamage = 0.5;
};
function Nuke3::onAdd(%this)
{
	%this.damage = 0;
	Nuke3::deployCheck(%this);
        %armor = Player::getArmor(%player);
        %client = Player::getClient(%player);
        GameBase::setTeam (%obj,GameBase::getTeam (%client));
}

function Nuke3::onCollision(%this,%object)
{
	%type = getObjectType(%object);
	%data = GameBase::getDataName(%this);
	if ((%type == "Player" || %data == AntipersonelMine || %data == Vehicle || %type == "Moveable") &&
			GameBase::isActive(%this)
			&& (GameBase::getTeam(%this)!=GameBase::getTeam(%object)) //no teamdmg
			) 
		GameBase::setDamageLevel(%this, %data.maxDamage);
}

function Nuke3::deployCheck(%this)
{
	if (GameBase::isAtRest(%this)) {
		GameBase::playSequence(%this,1,"deploy");
	 	GameBase::setActive(%this,true);
		%set = newObject("set",SimSet);
		if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),1,1,1,0)) {
			%data = GameBase::getDataName(%this);
			GameBase::setDamageLevel(%this, %data.maxDamage);
		}
		deleteObject(%set); //
	}
	else 
		schedule("Nuke3::deployCheck(" @ %this @ ");", 3, %this);
}

//-=-=-=-=--The Mine Launcher

ItemImageData MineLauncherImage
{
	shapeFile = "grenadeL";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	ammoType = MinelAmmo;
	projectileType = MineShell; 
	accuFire = false;
	reloadTime = 1.4;
	fireTime = 0.5;

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = SoundFireGrenade;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;
};

ItemData MineLauncher
{
	description = "Mine Layer";
	className = "Weapon";
	shapeFile = "grenadeL";
	hudIcon = "grenade";
    heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = MineLauncherImage;
	price = 15;
	showWeaponBar = true;
};

function MineLauncher::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Mine Layer<f1>\nFires a MIRV type projectile, which splits into several mines.");
}


//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Targeting Laser (TargetingLaser)
//  By Dynamix
//
//  Alliance version by Mjolnir, 
//    see Contrib.txt
//
//  For installation information, see Install.txt
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$InvList[TargetingLaser] = 1;
$RemoteInvList[TargetingLaser] = 1;
$AutoUse[TargetingLaser] = False;

// Targeting Laser is not in the cycle chain, so
// it's not added, but it could be if you want.
//addWeapon(TargetingLaser);

TargetLaserData targetLaser 
{
  laserBitmapName = "laserPulse.bmp";
  damageConversion = 0.0;
  baseDamageType = 0;
  lightRange = 2.0;
  lightColor = { 0.25, 1.0, 0.25 };
  detachFromShooter = false;
};

ItemImageData TargetingLaserImage 
{
  shapeFile = "paintgun";
  mountPoint = 0;
  weaponType = 2;
  projectileType = targetLaser;
  accuFire = true;
  minEnergy = 5;
  maxEnergy = 15;
  reloadTime = 1.0;
  lightType = 3;
  lightRadius = 1;
  lightTime = 1;
  lightColor = { 0.25, 1, 0.25 };
  sfxFire = SoundFireTargetingLaser;
  sfxActivate = SoundPickUpWeapon;
};

ItemData TargetingLaser 
{
  description = "Targeting Laser";
  className = "Tool";
  shapeFile = "paintgun";
  hudIcon = "targetlaser";
  heading = $InvHead[ihWea];
  shadowDetailMask = 4;
  imageType = TargetingLaserImage;
  price = 1;
  showWeaponBar = false;
};
function TargetingLaser::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Targeting Laser<f1>\nUsed to mark targets to alert fellows of enemy locations, turrets, etc for assault purposes.");
}



//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Drainer
//  By <[DC]>Paladin
//
//
//  For installation information, see Install.txt
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$InvList[ELF] = 1;
$RemoteInvList[ELF] = 1;
$AutoUse[ELF] = False;
$WeaponAmmo[ELF] = "";

addWeapon(ELF);

LightningData zapperCharge
{
   bitmapName       = "grn_blink2.bmp";

   damageType       = $ElectricityDamageType;
   boltLength       = 40.0;
   coneAngle        = 35.0;
   damagePerSec      = 0.03;
   energyDrainPerSec = 150.0;
   segmentDivisions = 4;
   numSegments      = 5;
   beamWidth        = 0.125;//075;

   updateTime   = 120;
   skipPercent  = 0.5;
   displaceBias = 0.15;

   lightRange = 3.0;
   lightColor = { 0.25, 0.25, 0.85 };

   soundId = SoundELFFire;
};

ItemImageData ELFImage
{
   shapeFile = "shotgun";
   mountPoint = 0;
   weaponType = 2;  // Sustained
   projectileType = zapperCharge;
   minEnergy = 3;
   maxEnergy = 11;  // Energy used/sec for sustained weapons
   reloadTime = 0.2;
   lightType = 3;  // Weapon Fire
   lightRadius = 2;
   lightTime = 1;
   lightColor = { 0.25, 0.25, 0.85 };

   sfxActivate = SoundPickUpWeapon;
   sfxFire     = SoundELFIdle;
};

ItemData ELF
{
  className = "Weapon";
  description = "Drainer";
  heading = $InvHead[ihWea];
  hudIcon = "energyRifle";
  imageType = ELFImage;
  price = 5;
  shadowDetailMask = 4;
  shapeFile = "shotgun";
  showWeaponBar = true;
};

function ELF::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      bottomprint(%client, "<f0>The Drainer<f1>\nMore a tool than a weapon, it completely drains energy sources from it's targets almost instantly.");
}