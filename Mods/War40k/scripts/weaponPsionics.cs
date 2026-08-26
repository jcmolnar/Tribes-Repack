//NEW PSI TO MAKE
// Smite: Area effect harm
// Scan: Eye of God(unarmed)
// Vampire: Area effect, hurts targets, replenishes friendlies
// Launch: Launches the psycher into the air
// Holocaust: Lights all units in large area on fire
// Eviscerate: Tear targets energy and give to self as psi
// Ark Field: Cube forcefield trap which detonates after 10 seconds
// Pain Bridge: a line of psionic eruptions that deciamte anythign in their path
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Telemechanics 
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Distort] = 1;
$RemoteInvList[Distort] = 1;
$AutoUse[Distort] = True;
$WeaponAmmo[Distort] = "";

addWeapon(Distort);

RocketData DistortShell
{
   bulletShapeName  = "plasmaex.dts";
   explosionTag     = LargeShockwave;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.15;
   damageType       = $FlashDamageType;

   explosionRadius  = 20.5;
   kickBackStrength = 20.0;
   muzzleVelocity   = 60.0;
   terminalVelocity = 150.0;
   acceleration     = 5.0;
   totalTime        = 5.0;
   liveTime         = 5.0;
   lightRange       = 2.0;
   lightColor       = { 0.4, 0.4, 5.0 };
   inheritedVelocityScale = 0.5;
   soundId = MineExplosion;
};

ItemImageData DistortImage
{
	shapeFile = "sensor_small";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	projectileType = DistortShell;
	accuFire = true;
	reloadTime = 2.0;
	fireTime = 0.05;
        damageClass = 1;
        damageValue = 0.3;
	minEnergy = 100;
	maxEnergy = 100;

	lightType = 3;  // Weapon Fire
	lightRadius = 2;
	lightTime = 1;
	lightColor = { 1, 0, 0 };

	sfxFire = SoundFireLaser;
	sfxActivate = SoundPickUpWeapon;
};

ItemData Distort
{
	description = "Telemechanics";
	className = "Weapon";
	shapeFile = "sensor_small";
	hudIcon = "energyRifle";
   heading = $InvHead[ihPsi];
	shadowDetailMask = 4;
	imageType = DistortImage;
	price = 16;
	showWeaponBar = true;
};

function Distort::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      Bottomprint(%client, "Telemechanics: This power causes energy systems to disrupt for several seconds.");
}


//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Pyrokinesis
//  By <[DC]>Paladin
//
//  Warhammer 40k Mod
//    
//
//  For installation information, see Install.txt
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$InvList[Fireball] = 1;
$RemoteInvList[Fireball] = 1;
$AutoUse[Fireball] = True;
$Use[Fireball] = True;
$WeaponAmmo[Fireball] = "";

addWeapon(Fireball);

BulletData FireballShot
{
   bulletShapeName    = "plasmabolt.dts";
   explosionTag       = mortarExp;

   damageClass        = 1;
   damageValue        = 0.35;
   damageType         = $PlasmaDamageType;
   explosionRadius    = 20.0;

   muzzleVelocity     = 30.0;
   totalTime          = 3.45;
   liveTime           = 3.45;
   lightRange         = 3.0;
   lightColor         = { 1, 1, 0 };
   inheritedVelocityScale = 0.3;
   isVisible          = True;

   soundId = SoundJetLight;
};


ItemImageData FireballImage
{
   shapeFile  = "sensor_small";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 1.5;
	fireTime = 0.05;
	minEnergy = 150;
	maxEnergy = 150;

	projectileType = FireballShot;
	accuFire = true;

	sfxFire = SoundFirePlasma;
	sfxActivate = SoundPickUpWeapon;
};

ItemData Fireball
{
   heading = $InvHead[ihPsi];
	description = "Pyrokinesis";
	className = "Weapon";
   shapeFile  = "sensor_small";
	hudIcon = "blaster";
	shadowDetailMask = 4;
	imageType = FireballImage;
	price = 12;
	showWeaponBar = true;
};

function Fireball::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "Pyrokinesis: This power hurls a searing hot ball of flame at the enemy.");
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Pyrokinetic Burst
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$WeaponAmmo[Flamewall] = "";
$InvList[Flamewall] = 1;
$AutoUse[Flamewall] = False;
$RemoteInvList[Flamewall] = 1;

addWeapon(Flamewall);

$Needs[Flamewall] = MindPack;


//=====================================================================//=== Da Flamewall

RocketData FlameBarrier
{
   bulletShapeName  = "plasmatrail.dts";
   explosionTag     = mortarExp;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.4;
   damageType       = $PlasmaDamageType;

   explosionRadius  = 9.5;
   kickBackStrength = 50.0;
   muzzleVelocity   = 65.0;
   terminalVelocity = 80.0;
   acceleration     = 5.0;
   totalTime        = 10.0;
   liveTime         = 11.0;
   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };
   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "plasmatrail.dts";
   smokeDist   = 1.8;

   soundId = SoundJetHeavy;
};

function FlameBarrier::onAdd(%this)
{
	schedule("DeployBomblets(" @ %this @ " , 5);",1.0,%this);
}

function DeployBomblets(%this, %count) 
{
	if(%count && %this)
	{
		%obj = newObject("","Mine","Firewall");
 		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj,%this,20,false);
		
		%obj = newObject("","Mine","Firewall");
	 	addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj,%this,10,false);
		%obj = newObject("","Mine","Firewall");
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj,%this,5,false);
		%count -= 1;
		schedule("DeployBomblets(" @ %this @ " , " @ %count @ ");",0.5,%this);
	}
}


//=====================================================================//==

ItemImageData FlamewallImage
{
   shapeFile  = "sensor_small";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 1.4;
	fireTime = 0.25;
	minEnergy = 120;
	maxEnergy = 120;

	projectileType = FlameBarrier;
	accuFire = true;

	sfxFire = SoundFireBlaster;
	sfxActivate = SoundPickUpWeapon;
};

ItemData Flamewall
{
   heading = $InvHead[ihPsi];
	description = "Pyro. Burst";
	className = "Weapon";
   shapeFile  = "sensor_small";
	hudIcon = "blaster";
	shadowDetailMask = 4;
	imageType = FlamewallImage;
	price = 15;
	showWeaponBar = true;
};


function Flamewall::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "Pyrokinetic Burst: A raging stream of fire, capable of burning everything in it's path, and leaving an after effect behind it.");
}

function Flamewall::onUse(%player,%item)
{
	if(Player::getMountedItem(%player,$BackpackSlot) == MindPack)
		Weapon::onUse(%player,%item);
	else
		Client::sendMessage(Player::getClient(%player),0,
			"Must Concentrate to use Pyrokinetic Burst."); }

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Mend Flesh
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Heal] = 1;
$RemoteInvList[Heal] = 1;
$AutoUse[Heal] = True;

addWeapon(Heal);

RepairEffectData HealBolt 
{
  bitmapName = "fuex00.bmp";
  boltLength = 100.0;
  segmentDivisions = 4;
  beamWidth = 0.125;
  updateTime = 450;
  skipPercent = 0.6;
  displaceBias = 0.15;
  lightRange = 3.0;
  lightColor = { 0.85, 0.25, 0.25 };
};

function HealBolt::onAcquire(%this, %player, %target) 
{
  %client = Player::getClient(%player);
  if (%target == %player) 
  {
    %player.repairTarget = -1;
    if (GameBase::getDamageLevel(%player) != 0) 
    {
      %player.repairRate = 0.1;
      %player.repairTarget = %player;
      Client::sendMessage(%client, 0, "Healing");
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
    %player.repairRate = 0.1;
    if (getObjectType(%player.repairTarget) == "Player") 
    {
      %rclient = Player::getClient(%player.repairTarget);
      %name = Client::getName(%rclient);
//Sniper Leg hit Effect Removal
      Player::decItemCount(%this, DeadWeight);
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
      Client::sendMessage(%rclient,0,"Being healed by " @ Client::getName(%client));
    }
    Client::sendMessage(%client,0,"Healing " @ %name);
  }
  %rate = GameBase::getAutoRepairRate(%player.repairTarget) + %player.repairRate;
  GameBase::setAutoRepairRate(%player.repairTarget,%rate);
}

function HealBolt::onRelease(%this, %player) 
{
  %object = %player.repairTarget;
  if (getObjectType(%player.repairTarget) == "Player") %type = Player::getArmor(%object);
  else %type = GameBase::getDataName(%object);
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

		%fixpoints = (floor(%type.maxdamage - (%type.maxdamage - %object.mindamage)));
		if(%fixpoints < 1) %fixpoints = 1;
		%object.mindamage = 0;
	      %playerClient = GameBase::getControlClient(%object.lastDamageObject);
	   	if(%client != %playerClient)
		{
			if(GameBase::getTeam(%object) == GameBase::getTeam(%client))
			{
				if (getObjectType(%player.repairTarget) == "Player") 
				{
					%client.score = %client.score + %fixpoints;
					bottomprint(%client, "<f0>Score:<f1> +" @ %fixpoints);
					Game::refreshClientScore(%client);
				}
				else if (GameBase::getDataName(%this).mapFilter != -1)
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

function HealBolt::checkDone(%this, %player) 
{
  if (Player::isTriggered(%player,$WeaponSlot) && Player::getMountedItem(%player,$WeaponSlot) == Heal && %player.repairTarget != -1) 
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

ItemImageData HealImage 
{
  shapeFile = "sensor_small";
  mountPoint = 0;
  weaponType = 2;
  projectileType = HealBolt;
  minEnergy = 14;
  maxEnergy = 25;
  lightType = 3;
  lightRadius = 1;
  lightTime = 1;
  lightColor = { 0.25, 1, 0.25 };
  sfxFire = SoundRepairItem;
  sfxActivate = SoundPickUpWeapon;
};

ItemData Heal 
{
  description = "Mend Flesh";
  className = "Tool";
  shapeFile = "sensor_small";
  hudIcon = "targetlaser";
  heading = $InvHead[ihPsi];
  shadowDetailMask = 4;
  imageType = HealImage;
  price = 9;
  showWeaponBar = false;
};

function Heal::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "Mend Flesh: Regenerates living tissue.");
}


//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Electrokinesis
//  
//
//  For installation information, see Install.txt
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$InvList[Kannon] = 1;
$RemoteInvList[Kannon] = 1;
$AutoUse[Kannon] = False;
$WeaponAmmo[Kannon] = "";

addWeapon(Kannon);

LightningData KannonShot
{
   bitmapName       = "lightningnew.bmp";

   damageType       = $ElectricityDamageType;
   boltLength       = 40.0;
   coneAngle        = 35.0;
   damagePerSec      = 0.556;
   energyDrainPerSec = 0.0;
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

ItemImageData KannonImage 
{
   shapeFile = "discb";
   mountPoint = 0;
   weaponType = 2;  // Sustained
   projectileType = kannonshot;
   minEnergy = 13;
   maxEnergy = 21;  // Energy used/sec for sustained weapons
   reloadTime = 0.2;
   lightType = 3;  // Weapon Fire
   lightRadius = 2;
   lightTime = 1;
   lightColor = { 0.25, 0.25, 0.85 };

   sfxActivate = SoundPickUpWeapon;
   sfxFire     = SoundELFIdle;
};

ItemData Kannon
{
  description = "Electrokinesis";
  className = "Weapon";
  shapeFile = "discb";
  hudIcon = "targetlaser";
  heading = $InvHead[ihPsi];
  shadowDetailMask = 4;
  imageType = KannonImage;
  price = 25;
  showWeaponBar = true;
};

function Kannon::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "Electrokinesis: Emits a brutal stream of electrical psionic energy, quickly killing targets.");
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Focus Disc
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$WeaponAmmo[Disc] = "";
$InvList[Disc] = 1;
$AutoUse[Disc] = False;
$RemoteInvList[Disc] = 1;

addWeapon(Disc);

$Needs[Disc] = MindPack;

//=====================================================================//=== Da Disc

RocketData DiscBolt
{
  bulletShapeName = "discb.dts";
  explosionTag = rocketExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.623;
  damageType = $PsiDamageType;
  explosionRadius = 7.5;
  kickBackStrength = 150.0;
  muzzleVelocity = 165.0;
  terminalVelocity = 280.0;
  acceleration = 5.0;
  totalTime = 6.5;
  liveTime = 8.0;
  lightRange = 5.0;
  lightColor = { 0.4, 0.4, 1.0 };
  inheritedVelocityScale = 0.5;
  trailType = 1;
  trailLength = 15;
  trailWidth = 0.3;
  soundId = SoundDiscSpin;
};
//=====================================================================//=== Psionic Disc

ItemImageData DiscImage
{
   shapeFile  = "sensor_small";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 1.4;
	fireTime = 0.25;
	minEnergy = 200;
	maxEnergy = 200;

	projectileType = DiscBolt;
	accuFire = true;

	sfxFire = SoundFireBlaster;
	sfxActivate = SoundPickUpWeapon;
};

ItemData Disc
{
   heading = $InvHead[ihPsi];
	description = "Focus Disc";
	className = "Weapon";
   shapeFile  = "sensor_small";
	hudIcon = "blaster";
	shadowDetailMask = 4;
	imageType = DiscImage;
	price = 12;
	showWeaponBar = true;
};


function Disc::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "Focus Disc: A condsended disc of mental energy, truly a power of finesse..");
}

function Disc::onUse(%player,%item)
{
	if(Player::getMountedItem(%player,$BackpackSlot) == MindPack)
		Weapon::onUse(%player,%item);
	else
		Client::sendMessage(Player::getClient(%player),0,
			"Must Concentrate to use Focus Disc."); }

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Psi Beam
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[PsiLaser] = 1;
$RemoteInvList[PsiLaser] = 1;
$AutoUse[PsiLaser] = True;
$WeaponAmmo[PsiLaser] = "";

addWeapon(PsiLaser);

LaserData PsiLaserBolt
{
   laserBitmapName   = "paintpulse.bmp";
   hitName           = "laserhit.dts";

   damageConversion  = 0.013;
   baseDamageType    = $PsiDamageType;

   beamTime          = 1.0;

   lightRange        = 1.0;
   lightColor        = { 0.0, 1.25, 0.25 };

   detachFromShooter = false;
   hitSoundId        = SoundLaserHit;
};


ItemImageData PsiLaserImage
{
   shapeFile  = "sensor_small";
	mountPoint = 0;

      weaponType = 0; // Single Shot
	projectileType = PsiLaserBolt;
	reloadTime = 1.3;
	fireTime = 0.1;
	minEnergy = 150;
	maxEnergy = 150;

	accuFire = true;

	sfxFire = SoundFireLaser;
	sfxActivate = SoundPickUpWeapon;
};

ItemData PsiLaser
{
   heading = $InvHead[ihPsi];
	description = "Beam";
	className = "Weapon";
   shapeFile  = "sensor_small";
	hudIcon = "blaster";
	shadowDetailMask = 4;
	imageType = PsiLaserImage;
	price = 15;
	showWeaponBar = true;
};

function PsiLaser::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "Beam:This psionic beam acts as an effective long range weapon. Drains a lot of Psy, however.");
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Mental Burst
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[DCannon] = 1;
$RemoteInvList[DCannon] = 1;
$AutoUse[DCannon] = True;
$WeaponAmmo[DCannon] = "";

addWeapon(DCannon);


$Needs[DCannon] = MindPack;

RocketData DCannonShell
{
   bulletShapeName  = "";
   explosionTag     = LargeShockwave;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.5;
   damageType       = $PsiDamageType;

   explosionRadius  = 30.5;
   kickBackStrength = 50.0;
   muzzleVelocity = 165.0;
   terminalVelocity = 2000.0;
   acceleration     = 5.0;
   totalTime        = 6.0;
   liveTime         = 6.0;
   lightRange       = 2.0;
   lightColor       = { 0.4, 0.4, 5.0 };
   inheritedVelocityScale = 0.5;
   soundId = MineExplosion;
};

ItemImageData DCannonImage
{
	shapeFile = "sensor_small";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	projectileType = DCannonShell;
	accuFire = true;
	reloadTime = 2.0;
	fireTime = 1.0;
        damageClass = 1;
        damageValue = 0.3;
	minEnergy = 225;
	maxEnergy = 225;

	lightType = 3;  // Weapon Fire
	lightRadius = 2;
	lightTime = 1;
	lightColor = { 1, 0, 0 };

	sfxFire = SoundFireLaser;
	sfxActivate = SoundPickUpWeapon;
};

ItemData DCannon
{
	description = "Mental Burst";
	className = "Weapon";
	shapeFile = "sensor_small";
	hudIcon = "energyRifle";
   heading = $InvHead[ihPsi];
	shadowDetailMask = 4;
	imageType = DCannonImage;
	price = 15;
	showWeaponBar = true;
};

function DCannon::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      Bottomprint(%client, "Mental Burst: A burst of amplified psionic energy, which causes a massive shockwave at the target location.");
}

function DCannon::onUse(%player,%item)
{
	if(Player::getMountedItem(%player,$BackpackSlot) == MindPack)
		Weapon::onUse(%player,%item);
	else
		Client::sendMessage(Player::getClient(%player),0,
			"Must concentrate to use Mental Burst."); }

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Destructor
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Stream] = 1;
$RemoteInvList[Stream] = 1;
$WeaponAmmo[Stream] = "";
$AutoUse[Stream] = True;

addWeapon(Stream);

BulletData StreamBolt 
{
  bulletShapeName = "mortartrail.dts";
  explosionTag = PlasmaExp;
  damageClass = 1;
  damageValue = 0.08;
  damageType = $PsiDamageType;
  explosionRadius = 8.0;
  muzzleVelocity = 30.0;
  totalTime = 4.5;
  liveTime = 4.5;
  lightRange = 1.0;
  lightColor = { 0, 2, 0 };
  inheritedVelocityScale = 0.3;
  isVisible = True;
  soundId = SoundJetLight;
};
ItemImageData StreamImage 
{
  shapeFile = "sensor_small";
  mountPoint = 0;
  weaponType = 0;
  reloadTime = 0.05;
  fireTime = 0.05;
  minEnergy = 5;
  maxEnergy = 6;
  projectileType = StreamBolt;
  accuFire = true;
  sfxFire = SoundJetHeavy;
  sfxActivate = SoundPickUpWeapon;
};
ItemData Stream 
{
  heading = $InvHead[ihPsi];
  description = "Destructor";
  className = "Weapon";
  shapeFile = "sensor_small";
  hudIcon = "plasma";
  shadowDetailMask = 4;
  imageType = StreamImage;
  price = 12;
  showWeaponBar = true;
};

function Stream::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "This power shoots a solid stream of chaotic energy forth, easily killing most targets.");
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Psionic Storm
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Pull] = 1;
$RemoteInvList[Pull] = 1;
$AutoUse[Pull] = False;
$WeaponAmmo[Pull] = "";

addWeapon(Pull);

GrenadeData Pullshot
{
   bulletShapeName    = "laserhit.dts";
   explosionTag       = debrisExpSmall;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.2;
   mass               = 1.0;
   elasticity         = 0.45;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.0;
   damageType         = $PsiDamageType;

   explosionRadius    = 10;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 150;
   totalTime          = 40.0;    // special meaning for grenades...
   liveTime           = 1.0;
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 0.5;

   smokeName              = "laserhit.dts";
};

function Pullshot::onAdd(%this)
{
	schedule("DeployPullshot(" @ %this @ " , 5);",1.0,%this);//5++++++
}

function DeployPullshot(%this, %count) 
{
	if(%count && %this)
	{
		%obj = newObject("","Mine","Pullshot1");
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj,%this,6.0,false);//0++++	
		%obj = newObject("","Mine","Pullshot2");
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj,%this,15.0,false);
		%obj = newObject("","Mine","Pullshot1");
		addToSet("MissionCleanup", %obj);
		GameBase::throw(%obj,%this,30.0,false);
		%count -= 1;
		schedule("DeployPullshot(" @ %this @ " , " @ %count @ ");",1.0,%this);//0.5++++++++
	}
}

MineData Pullshot1
{
  mass = 1.0;
  drag = 1.0;
  density = 1.0;
  elasticity = 0.1;
  friction = 0.2;
  className = "Handgrenade";
  description = "Handgrenade";
  shapeFile = "fusionbolt";
  shadowDetailMask = 4;
  explosionId = turretExp;
  explosionRadius = 20.0;
  damageValue = 0.35;
  damageType = $PsiDamageType;
  kickBackStrength = 250;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

function Pullshot1::onAdd(%this)
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",3.0,%this);
}

MineData Pullshot2
{
  mass = 1.0;
  drag = 1.0;
  density = 1.0;
  elasticity = 0.1;
  friction = 0.2;
  className = "Handgrenade";
  description = "Handgrenade";
  shapeFile = "fusionbolt";
  shadowDetailMask = 4;
  explosionId = turretExp;
  explosionRadius = 20.0;
  damageValue = 0.35;
  damageType = $PsiDamageType;
  kickBackStrength = 250;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

function Pullshot2::onAdd(%this)
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",1.0,%this);
}

ItemImageData PullImage
{
   shapeFile = "sensor_small";
   mountPoint = 0;

   weaponType = 0;  
   projectileType = PullShot;
   minEnergy = 140;
   maxEnergy = 140;  
   reloadTime = 2.0;
			
   lightType = 3;  // Weapon Fire
   lightRadius = 2;
   lightTime = 1;
   lightColor = { 0.85, 0.85, 0.15 };

   sfxActivate = SoundPickUpWeapon;
   sfxFire     = SoundELFIdle;
};

ItemData Pull
{
  className = "Weapon";
  description = "Psionic Storm";
  heading = $InvHead[ihPsi];
  hudIcon = "energyRifle";
  imageType = PullImage;
  price = 9;
  shadowDetailMask = 4;
  shapeFile = "sensor_small";
  showWeaponBar = true;
};

function Pull::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "Psionic Storm: A spreading blast of brutal psionic detonations.");
}
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Mind Blast
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$WeaponAmmo[Rain] = "";
$InvList[Rain] = 1;
$AutoUse[Rain] = True;
$RemoteInvList[Rain] = 1;

addWeapon(Rain);

$Needs[Rain] = MindPack;

RocketData RainMissile 
{
  bulletShapeName = "shockwave_large.dts";
  explosionTag = LargeShockwave;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 2.0;
  damageType = $PsiDamageType;
  explosionRadius = 6.5;
  kickBackStrength = 150.0;
  muzzleVelocity = 265.0;
  terminalVelocity = 1000.0;
  acceleration = 200.0;
  totalTime = 1.5;
  liveTime = 1.5;
  lightRange = 2.0;
  lightColor = { 1.0, 0.7, 0.5 };
  inheritedVelocityScale = 0.5;
  trailType = 2;
  trailString = "paint.dts";
  smokeDist = 1.8;
  soundId = SoundJetHeavy;
};

ItemImageData RainImage 
{
  shapeFile = "sensor_small";
  mountPoint = 0;
  weaponType = 0;
  projectileType = RainMissile;
  accuFire = true;
  reloadTime = 3.5;
  fireTime = 3.5;
  minEnergy = 300;
  maxEnergy = 300;
  lightType = 3;
  lightRadius = 3;
  lightTime = 1;
  lightColor = { 0.6, 1, 1.0 };
  sfxFire = SoundWindGust;
  sfxActivate = SoundPickUpWeapon;
  sfxReady = SoundLaserIdle;
};
ItemData Rain 
{
  description = "Mind Blast";
  className = "Weapon";
  shapeFile = "sensor_small";
  hudIcon = "mortar";
  heading = $InvHead[ihPsi];
  shadowDetailMask = 4;
  imageType = RainImage;
  price = 20;
  showWeaponBar = true;
};

function Rain::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "Mind Blast: A supersonic wave of psionic energy capable of leveling almost any living being with ease.");
}
function Rain::onUse(%player,%item)
{
	if(Player::getMountedItem(%player,$BackpackSlot) == MindPack)
		Weapon::onUse(%player,%item);
	else
		Client::sendMessage(Player::getClient(%player),0,
			"Must concentrate to use Mind Blast."); }
//-==-=-=-==-==-=-=-=-=-=--=-=-=-=-
//     Arch Psionic
//    by <[DC]>Paladin/Edgecrusher
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=--=

$InvList[RokkitLauncher] = 1;
$RemoteInvList[RokkitLauncher] = 1;
$AutoUse[RokkitLauncher] = False;
$WeaponAmmo[RokkitLauncher] = "";

addWeapon(RokkitLauncher);

$Needs[RokkitLauncher] = MindPack;

GrenadeData RokkitMissile 
{
  bulletShapeName = "plasmabolt.dts";
  explosionTag = mortarExp;
  collideWithOwner = True;
  ownerGraceMS = 250;
  collisionRadius = 0.3;
  mass = 5.0;
  elasticity = 0.6;
  damageClass = 1;
  damageValue = 1.0;
  damageType = $PsiDamageType;
  explosionRadius = 20.0;
  kickBackStrength = 250.0;
  maxLevelFlightDist = 475;
  totalTime = 30.0;
  liveTime = 2.0;
  projSpecialTime = 0.01;
  inheritedVelocityScale = 0.5;
  smokeName = "plasmabolt.dts";
};


ItemImageData RokkitImage 
{
  shapeFile = "sensor_small";
  mountPoint = 0;
  weaponType = 0;
  projectileType = RokkitMissile;
  accuFire = true;
  reloadTime = 3.0;
  fireTime = 0.01;
  minEnergy = 250;
  maxEnergy = 250;
  lightType = 3;
  lightRadius = 3;
  lightTime = 1;
  lightColor = { 0.6, 1, 1.0 };
  sfxFire = SoundMissileTurretFire;
  sfxActivate = SoundPickUpWeapon;
  sfxReload = SoundMortarReload;
  sfxReady = SoundMortarIdle;
};

ItemData RokkitLauncher 
{
  description = "Arch";
  className = "Weapon";
  shapeFile = "sensor_small";
  hudIcon = "mortar";
  heading = $InvHead[ihPsi];
  shadowDetailMask = 4;
  imageType = RokkitImage;
  price = 22;
  showWeaponBar = true;
};

function RokkitLauncher::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "Arch: A construct of anti-matter is flung at the desired target. Bounces on impact. A tough power to use.");
}

function RokkitLauncher::onUse(%player,%item)
{
	if(Player::getMountedItem(%player,$BackpackSlot) == MindPack)
		Weapon::onUse(%player,%item);
	else
		Client::sendMessage(Player::getClient(%player),0,
			"Must concentrate to use Arch."); }

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Kinetic Blast
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Burst] = 1;
$RemoteInvList[Burst] = 1;
$WeaponAmmo[Burst] = "";
$AutoUse[Burst] = False;

addWeapon(Burst);


//======================================================================== Burst Blast

BulletData BurstBlast
{
   bulletShapeName    = "paint.dts";
   explosionTag       = rocketExp;
   expRandCycle       = 3;
   mass               = 0.07;
   bulletHoleIndex    = 0;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.3;
   explosionRadius    = 5.0;
   damageType         = $PsiDamageType;

   aimDeflection      = 0.009;
   muzzleVelocity     = 200.0;
   totalTime          = 2;
   inheritedVelocityScale = 1.0;
   isVisible          = True;

   soundId = SoundJetLight;
   
};

//======================================================================== Burst Power


ItemImageData BurstImage 
{
	shapeFile = "sensor_small";
    mountPoint = 0;

	projectileType = BurstBlast;
	weaponType = 0; // Single Shot
	reloadTime = 0.2;
	fireTime = 0.08;
	minEnergy = 15;
	maxEnergy = 15;
                        
	accuFire = false;

	 lightType = 3;
	 lightRadius = 3;
	 lightTime = 1;
	 lightColor = { 1.0, 0.7, 0.5 };

	sfxActivate = SoundPickUpWeapon;
	sfxFire     = SoundFirePlasma;
   
};

ItemData Burst
{
    description = "Kinetic Blast";
	shapeFile = "sensor_small";
	hudIcon = "blaster";
	heading = $InvHead[ihPsi];
    className = "Weapon";
    shadowDetailMask = 4;
    imageType = BurstImage;
	showWeaponBar = true;
    price = 10;
};


function Burst::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "Kinetic Blast: A mental storm of psionic energy bolts. Extremely deadly.");
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Delusion
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Gravi] = 1;
$RemoteInvList[Gravi] = 1;
$AutoUse[Gravi] = True;
$WeaponAmmo[Gravi] = "";

addWeapon(Gravi);

$Needs[Gravi] = MindPack;

RocketData GraviShot
{
   bulletShapeName  = "";
   explosionTag     = LargeShockwave;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.3;
   damageType       = $EnergyDamageType;

   explosionRadius  = 30.5;
   kickBackStrength = -180.0;
   muzzleVelocity   = 75.0;
   terminalVelocity = 200.0;
   acceleration     = 5.0;
   totalTime        = 10.0;
   liveTime         = 10.0;
   lightRange       = 9.0;
   lightColor       = { 0.4, 0.4, 1.0 };
   inheritedVelocityScale = 0.5;

   soundId = MineExplosion;
};

ItemImageData GraviImage
{
	shapeFile = "sensor_small";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	projectileType = GraviShot;
	accuFire = true;
	reloadTime = 5.0;
	fireTime = 0.2;
        damageClass = 1;
        damageValue = 0.05;
	minEnergy = 350;
	maxEnergy = 350;

	lightType = 3;  // Weapon Fire
	lightRadius = 2;
	lightTime = 1;
	lightColor = { 1, 0, 0 };

	sfxFire = SoundFireLaser;
	sfxActivate = SoundPickUpWeapon;
};

ItemData Gravi
{
	description = "Delusion";
	className = "Weapon";
	shapeFile = "sensor_small";
	hudIcon = "energyRifle";
   heading = $InvHead[ihPsi];
	shadowDetailMask = 4;
	imageType = GraviImage;
	price = 15;
	showWeaponBar = true;
};

function Gravi::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
      Bottomprint(%client, "Delusion: This deadly power makes a being believe it is gravely ill, and it's body mimics that illnesses effect.");
}

function Gravi::onUse(%player,%item)
{
	if(Player::getMountedItem(%player,$BackpackSlot) == MindPack)
		Weapon::onUse(%player,%item);
	else
		Client::sendMessage(Player::getClient(%player),0,
			"Must concentrate to use Delusion."); }

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Electokinesis Blast
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$WeaponAmmo[Zap] = "";
$InvList[Zap] = 1;
$AutoUse[Zap] = True;
$RemoteInvList[Zap] = 1;

addWeapon(Zap);

$Needs[Zap] = MindPack;

//=====================================================================//=== Psionic Zap

RocketData ZapBolt
{
  bulletShapeName = "fusionbolt.dts";
  explosionTag = rocketExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 0.55;
  damageType = $ElectricityDamageType;
  explosionRadius = 20.5;
  kickBackStrength = 150.0;
  muzzleVelocity = 65.0;
  terminalVelocity = 130.0;
  acceleration = 5.0;
  totalTime = 6.5;
  liveTime = 6.5;
  lightRange = 5.0;
  lightColor = { 1.0, 0.5, 0.2 };
  inheritedVelocityScale = 0.0;
  trailType = 2;
  trailString = "fusionbolt.dts";
  smokeDist = 1.0;
  soundId = SoundJetHeavy;
};

//=====================================================================//=== Psionic Zap

ItemImageData ZapImage
{
   shapeFile  = "sensor_small";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 1.0;
	fireTime = 1.0;
	minEnergy = 200;
	maxEnergy = 200;

	projectileType = ZapBolt;
	accuFire = true;

	sfxFire = SoundFireBlaster;
	sfxActivate = SoundPickUpWeapon;
};

ItemData Zap
{
   heading = $InvHead[ihPsi];
	description = "Electro. Blast";
	className = "Weapon";
   shapeFile  = "sensor_small";
	hudIcon = "blaster";
	shadowDetailMask = 4;
	imageType = ZapImage;
	price = 22;
	showWeaponBar = true;
};


function Zap::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "Shoots a deadly stream of psionic energy forth, annihilating targets with ease.");
}

function Zap::onUse(%player,%item)
{
	if(Player::getMountedItem(%player,$BackpackSlot) == MindPack)
		Weapon::onUse(%player,%item);
	else
		Client::sendMessage(Player::getClient(%player),0,
			"Must concentrate to use Electrokinesis Blast."); }


//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Psionic Blast
//  By <[DC]>Paladin
//
//   
//    
//
//  For installation information, see Install.txt
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$InvList[ShockBlast] = 1;
$RemoteInvList[ShockBlast] = 1;
$AutoUse[ShockBlast] = False;
$WeaponAmmo[ShockBlast] = "";

addWeapon(ShockBlast);

RocketData ShockBlastShot
{
  bulletShapeName = "fusionbolt.dts";
  explosionTag = turretExp;
  collisionRadius = 0.0;
  mass = 2.0;
  damageClass = 1;
  damageValue = 1.0;
  damageType = $ElectricityDamageType;
  explosionRadius = 6.0;
  kickBackStrength = 1.0;
  muzzleVelocity = 65.0;
  terminalVelocity = 430.0;
  acceleration = 5.0;
  totalTime = 5.5;
  liveTime = 5.5;
  lightRange = 2.0;
  lightColor = { 2.20, 1.7, 1.5 };
  inheritedVelocityScale = 0.0;
  soundId = SoundFirePlasma;
};

ItemImageData ShockBlastImage 
{
  shapeFile = "sensor_small";
  mountPoint = 0;
  weaponType = 0;
  projectileType = ShockBlastShot;
  accuFire = true;
  reloadTime = 2.4;
  fireTime = 0.0;
  minEnergy = 100;
  maxEnergy = 100;
  lightType = 3;
  lightRadius = 6;
  lightTime = 2;
  lightColor = { 0, 0, 3.0 };
  sfxFire = SoundFireMortar;
  sfxActivate = SoundPickUpWeapon;
  sfxReady = SoundMortarIdle;
};

ItemData ShockBlast
{
  description = "Psionic Blast";
  className = "Weapon";
  shapeFile = "sensor_small";
  hudIcon = "targetlaser";
  heading = $InvHead[ihPsi];
  shadowDetailMask = 4;
  imageType = ShockBlastImage;
  price = 15;
  showWeaponBar = true;
};

function ShockBlast::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "Psionic Blast: An explosive blast of mental feedback guaranteed to cause agony to the target.");
}

