//----------------------------------------------------------------------------
// Mech Mayhem -- LAST STAND: ejected-pilot datablocks.
//
// The MechPilot armor is what a Herc pilot becomes when the ejection system
// fires (MechEject.cs). It is a full copy of stock larmor (same shape, same
// client assets -- zero-install joins keep working) with a hardened flight
// suit: more hull, more capacitor, and a minDamageSpeed high enough to
// survive the ejection landing without jets.
//
// The anti-Herc kit is three energy-fed weapons (the suit capacitor IS the
// ammo, same doctrine as the mechs):
//   PilotSidearm  machine blaster; energy damage, so mech shields drink it.
//                 It is for the OTHER ejected pilot, not the mech.
//   PilotLance    shoulder-fired seeking AP missile; conc warhead punches 25%
//                 through shields. Costs half the capacitor per shot.
//   PilotCharge   lobbed demolition satchel; the real mech-killer if you can
//                 get under one. Nearly drains the capacitor.
//
// Exec'd from modDataBlocks.cs AFTER MechWeapons (reuses its explosion tags,
// missile shapes and sounds) -- pre-preload, so everything registers.
//----------------------------------------------------------------------------

//--- the pilot ---------------------------------------------------------------

PlayerData MechPilot
{
   className = "Armor";
   shapeFile = "larmor";
   damageSkinData = "armorDamageSkins";
   debrisId = playerDebris;
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   shadowDetailMask = 1;
   validateShape = true;

   visibleToSensor = True;
   mapFilter = 1;
   mapIcon = "M_player";
   canCrouch = true;

   maxJetSideForceFactor = 0.8;
   maxJetForwardVelocity = 22;
   minJetEnergy = 1;
   jetForce = 260;
   jetEnergyDrain = 0.7;

   // hardened flight suit: nearly twice a light trooper's hull, bigger
   // capacitor (it feeds the weapons), and a landing tolerance that survives
   // the ejection arc even with the jets untouched.
   maxDamage = 1.2;
   maxForwardSpeed = 12.5;
   maxBackwardSpeed = 10;
   maxSideSpeed = 10;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundTraction = 3.0;
   maxEnergy = 80;
   drag = 1.0;
   density = 1.2;

   minDamageSpeed = 40;
   damageScale = 0.004;

   jumpImpulse = 75;
   jumpSurfaceMinDot = 0.2;

   // animation data (stock larmor set, verbatim):
   // animation name, one shot, direction
   // firstPerson, chaseCam, thirdPerson, signalThread
   animData[0]  = { "root", none, 1, true, true, true, false, 0 };
   animData[1]  = { "run", none, 1, true, false, true, false, 3 };
   animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
   animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
   animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
   animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
   animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
   animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
   animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
   animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
   animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
   animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
   animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
   animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
   animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
   animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
   animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
   animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
   animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
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

   animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 };

   animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   jetSound = SoundJetLight;
   rFootSounds =
   {
     SoundLFootRSoft,
     SoundLFootRHard,
     SoundLFootRSoft,
     SoundLFootRHard,
     SoundLFootRSoft,
     SoundLFootRSoft,
     SoundLFootRSoft,
     SoundLFootRHard,
     SoundLFootRSnow,
     SoundLFootRSoft,
     SoundLFootRSoft,
     SoundLFootRSoft,
     SoundLFootRSoft,
     SoundLFootRSoft,
     SoundLFootRSoft
   };
   lFootSounds =
   {
      SoundLFootLSoft,
      SoundLFootLHard,
      SoundLFootLSoft,
      SoundLFootLHard,
      SoundLFootLSoft,
      SoundLFootLSoft,
      SoundLFootLSoft,
      SoundLFootLHard,
      SoundLFootLSnow,
      SoundLFootLSoft,
      SoundLFootLSoft,
      SoundLFootLSoft,
      SoundLFootLSoft,
      SoundLFootLSoft,
      SoundLFootLSoft
   };

   footPrints = { 0, 1 };

   boxWidth = 0.5;
   boxDepth = 0.5;
   boxNormalHeight = 2.3;
   boxCrouchHeight = 1.8;

   boxNormalHeadPercentage  = 0.83;
   boxNormalTorsoPercentage = 0.53;
   boxCrouchHeadPercentage  = 0.6666;
   boxCrouchTorsoPercentage = 0.3333;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// ★every new armor needs ALL 15 rows -- an unset row multiplies damage by ""
// which is ZERO and weapons silently do nothing (player.cs:151)★
$DamageScale[MechPilot, $LandingDamageType] = 1.0;
$DamageScale[MechPilot, $ImpactDamageType] = 1.0;
$DamageScale[MechPilot, $CrushDamageType] = 1.0;
$DamageScale[MechPilot, $BulletDamageType] = 1.2;
$DamageScale[MechPilot, $PlasmaDamageType] = 1.0;
$DamageScale[MechPilot, $EnergyDamageType] = 1.3;
$DamageScale[MechPilot, $ExplosionDamageType] = 1.0;
$DamageScale[MechPilot, $MissileDamageType] = 1.0;
$DamageScale[MechPilot, $DebrisDamageType] = 1.2;
$DamageScale[MechPilot, $ShrapnelDamageType] = 1.2;
$DamageScale[MechPilot, $LaserDamageType] = 1.0;
$DamageScale[MechPilot, $MortarDamageType] = 1.3;
$DamageScale[MechPilot, $BlasterDamageType] = 1.3;
$DamageScale[MechPilot, $ElectricityDamageType] = 1.0;
$DamageScale[MechPilot, $MineDamageType] = 1.2;

$ItemMax[MechPilot, PilotSidearm] = 1;
$ItemMax[MechPilot, PilotLance] = 1;
$ItemMax[MechPilot, PilotCharge] = 1;
$ItemMax[MechPilot, EnergyPack] = 1;
$ItemMax[MechPilot, RepairKit] = 1;
$ItemMax[MechPilot, Grenade] = 3;
$MaxWeapons[MechPilot] = 3;

//--- the kit -----------------------------------------------------------------

// sidearm: fast machine blaster, stock bolt. Energy type = fully absorbed by
// mech shields (the Starsiege duality) -- this one is pilot-vs-pilot.
ItemImageData PilotSidearmImage
{
   shapeFile  = "energygun";
   mountPoint = 0;
   weaponType = 0;
   reloadTime = 0;
   fireTime   = 0.22;
   minEnergy  = 3;
   maxEnergy  = 4;
   projectileType = BlasterBolt;
   accuFire   = true;
   sfxFire    = SoundFireBlaster;
   sfxActivate = SoundPickUpWeapon;
};

ItemData PilotSidearm
{
   heading = "bWeapons";
   description = "Pilot Sidearm";
   className = "Weapon";
   shapeFile  = "energygun";
   hudIcon = "blaster";
   shadowDetailMask = 4;
   imageType = PilotSidearmImage;
   price = 85;
   showWeaponBar = true;
};

// shoulder lance: seeking AP missile (the player fire path laser-designates
// whatever the pilot is aiming at). Conc warhead -- 25% punches through mech
// shields. Half the capacitor per shot.
SeekingMissileData PilotLanceShot
{
   bulletShapeName = "pr_shrk.dts";
   explosionTag    = MechImpMd;
   collisionRadius = 0.0;
   mass            = 2.0;
   damageClass     = 1;
   damageValue     = 1.05;
   damageType      = $MissileDamageType;
   explosionRadius = 5.0;
   kickBackStrength = 250.0;
   muzzleVelocity   = 110.0;
   totalTime        = 4.0;
   liveTime         = 4.0;
   seekingTurningRadius    = 9.0;
   nonSeekingTurningRadius = 70.0;
   proximityDist     = 2.5;
   smokeDist         = 1.75;
   lightRange       = 4.0;
   lightColor       = { 1.0, 0.75, 0.2 };
   inheritedVelocityScale = 0.5;
   soundId = SoundJetHeavy;
};

ItemImageData PilotLanceImage
{
   shapeFile  = "disc";
   mountPoint = 0;
   weaponType = 0;
   reloadTime = 0;
   fireTime   = 3.0;
   minEnergy  = 40;
   maxEnergy  = 38;
   projectileType = PilotLanceShot;
   accuFire   = true;
   sfxFire    = MechSfx_SHRK;
   sfxActivate = SoundPickUpWeapon;
};

ItemData PilotLance
{
   heading = "bWeapons";
   description = "HERC Lance";
   className = "Weapon";
   shapeFile  = "disc";
   hudIcon = "disk";
   shadowDetailMask = 4;
   imageType = PilotLanceImage;
   price = 500;
   showWeaponBar = true;
};

// demolition satchel: lobbed, short fuse, big conc blast. Gets dangerous when
// a mech lets a pilot stand at its feet. Nearly drains the capacitor.
GrenadeData PilotChargeShot
{
   bulletShapeName    = "pr_ara.dts";
   explosionTag       = MechExpMd;
   collideWithOwner   = True;
   ownerGraceMS       = 600;
   collisionRadius    = 0.25;
   mass               = 6.0;
   elasticity         = 0.2;
   damageClass        = 1;
   damageValue        = 2.2;
   damageType         = $MineDamageType;
   explosionRadius    = 9.0;
   kickBackStrength   = 400.0;
   maxLevelFlightDist = 120;
   totalTime          = 30.0;
   liveTime           = 2.5;
   projSpecialTime    = 0.05;
   inheritedVelocityScale = 0.5;
   smokeName          = "smoke.dts";
};

ItemImageData PilotChargeImage
{
   shapeFile  = "grenadeL";
   mountPoint = 0;
   weaponType = 0;
   reloadTime = 0;
   fireTime   = 1.0;
   minEnergy  = 50;
   maxEnergy  = 48;
   projectileType = PilotChargeShot;
   accuFire   = false;
   sfxFire    = SoundFireGrenade;
   sfxActivate = SoundPickUpWeapon;
};

ItemData PilotCharge
{
   heading = "bWeapons";
   description = "Demo Charge";
   className = "Weapon";
   shapeFile  = "grenadeL";
   hudIcon = "grenade";
   shadowDetailMask = 4;
   imageType = PilotChargeImage;
   price = 300;
   showWeaponBar = true;
};

// closed cycling ring for the kit -- the stock 8-ring and the mech ring are
// both closed loops, so the pilot weapons need their own (nextweapon walks
// $NextWeapon until it returns to the start; joining the stock ring would
// corrupt trooper cycling on every non-mech mission the mod boots under).
$NextWeapon[PilotLance]   = "PilotCharge";
$NextWeapon[PilotCharge]  = "PilotSidearm";
$NextWeapon[PilotSidearm] = "PilotLance";
$PrevWeapon[PilotLance]   = "PilotSidearm";
$PrevWeapon[PilotCharge]  = "PilotLance";
$PrevWeapon[PilotSidearm] = "PilotCharge";

echo("[MECH] MechPilot loaded (Last Stand pilot armor + kit).");
