//----------------------------------------------------------------------------
// Light Armor
//----------------------------------------------------------------------------
// DEAD FOR NOW

//----------------------------------------------------------------------------
// Medium Armor
//----------------------------------------------------------------------------
// DEAD FOR NOW

//----------------------------------------------------------------------------
// Heavy Armor
//----------------------------------------------------------------------------
// DEAD FOR NOW

//----------------------------------------------------------------------------
// light Female Armor
//----------------------------------------------------------------------------
// DEAD FOR NOW

//----------------------------------------------------------------------------
// Medium Female Armor
//----------------------------------------------------------------------------
// DEAD FOR NOW

//------------------------------------------------------------------
// light armor data:
//------------------------------------------------------------------

DamageSkinData armorDamageSkins
{
   bmpName[0] = "dskin1_armor";
   bmpName[1] = "dskin2_armor";
   bmpName[2] = "dskin3_armor";
   bmpName[3] = "dskin4_armor";
   bmpName[4] = "dskin5_armor";
   bmpName[5] = "dskin6_armor";
   bmpName[6] = "dskin7_armor";
   bmpName[7] = "dskin8_armor";
   bmpName[8] = "dskin9_armor";
   bmpName[9] = "dskin10_armor";
};


//------------------------------------------------------------------
// Medium Armor data:
//------------------------------------------------------------------

//------------------------------------------------------------------
// Heavy Armor data:
//------------------------------------------------------------------

PlayerData harmor
{
   className = "Armor";
   shapeFile = "harmor";
   flameShapeName = "hflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   maxJetSideForceFactor = 0.8;
   maxJetForwardVelocity = 12;
   minJetEnergy = 1;
   jetForce = 385;
   jetEnergyDrain = 1.1;

	maxDamage = 1.32;
   maxForwardSpeed = 5.0;
   maxBackwardSpeed = 4.0;
   maxSideSpeed = 4.0;
   groundForce = 35 * 18.0;
   groundTraction = 4.5;
   mass = 18.0;
	maxEnergy = 110;
   drag = 1.0;
   density = 2.5;
   canCrouch = false;

	minDamageSpeed = 25;
	damageScale = 0.006;

   jumpImpulse = 150;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetHeavy;

   rFootSounds = 
   {
     SoundHFootRSoft,
     SoundHFootRHard,
     SoundHFootRSoft,
     SoundHFootRHard,
     SoundHFootRSoft,
     SoundHFootRSoft,
     SoundHFootRSoft,
     SoundHFootRHard,
     SoundHFootRSnow,
     SoundHFootRSoft,
     SoundHFootRSoft,
     SoundHFootRSoft,
     SoundHFootRSoft,
     SoundHFootRSoft,
     SoundHFootRSoft
  }; 
   lFootSounds =
   {
      SoundHFootLSoft,
      SoundHFootLHard,
      SoundHFootLSoft,
      SoundHFootLHard,
      SoundHFootLSoft,
      SoundHFootLSoft,
      SoundHFootLSoft,
      SoundHFootLHard,
      SoundHFootLSnow,
      SoundHFootLSoft,
      SoundHFootLSoft,
      SoundHFootLSoft,
      SoundHFootLSoft,
      SoundHFootLSoft,
      SoundHFootLSoft
   };

   footPrints = { 4, 5 };

   boxWidth = 0.8;
   boxDepth = 0.8;
   boxNormalHeight = 2.6;

   boxNormalHeadPercentage  = 0.70;
   boxNormalTorsoPercentage = 0.45;

   boxHeadLeftPercentage  = 0.48;
   boxHeadRightPercentage = 0.70;
   boxHeadBackPercentage  = 0.48;
   boxHeadFrontPercentage = 0.60;
};

//------------------------------------------------------------------
// Light female data:
//------------------------------------------------------------------


//------------------------------------------------------------------
// Medium female data:
//------------------------------------------------------------------

//------------------------------------------------------------------
//
//------------------------------------------------------------------

// DELTA FORCE ARMOR
// -------------------------------------------

// MEDIC

$DamageScale[marmor, $LandingDamageType] = 1.0;
$DamageScale[marmor, $ImpactDamageType] = 1.0;
$DamageScale[marmor, $CrushDamageType] = 1.0;
$DamageScale[marmor, $BulletDamageType] = 1.2;
$DamageScale[marmor, $PlasmaDamageType] = 1.0;
$DamageScale[marmor, $EnergyDamageType] = 1.3;
$DamageScale[marmor, $ExplosionDamageType] = 1.0;
$DamageScale[marmor, $MissileDamageType] = 1.0;
$DamageScale[marmor, $DebrisDamageType] = 1.2;
$DamageScale[marmor, $ShrapnelDamageType] = 1.2;
$DamageScale[marmor, $LaserDamageType] = 1.0;
$DamageScale[marmor, $MortarDamageType] = 1.3;
$DamageScale[marmor, $BlasterDamageType] = 1.3;
$DamageScale[marmor, $ElectricityDamageType] = 1.0;
$DamageScale[marmor, $MineDamageType] = 1.2;
$DamageScale[marmor, $ChargeDamageType] = 1.2;
$DamageScale[marmor, $AirstrikeDamageType] = 1.0;
$DamageScale[marmor, $BleedDamageType] = 1.0;
$DamageScale[marmor, $GrenadeDamageType] = 1.0;
$DamageScale[marmor, $PoisonDamageType] = 1.0;
$DamageScale[marmor, $SmokeDamageType] = 1.0;
$DamageScale[marmor, $StabDamageType] = 1.0;

$ItemMax[marmor, Blaster] = 1;
$ItemMax[marmor, Chaingun] = 1;
$ItemMax[marmor, Disclauncher] = 1;
$ItemMax[marmor, GrenadeLauncher] = 1;
$ItemMax[marmor, Mortar] = 0;
$ItemMax[marmor, PlasmaGun] = 1;
$ItemMax[marmor, LaserRifle] = 1;
$ItemMax[marmor, EnergyRifle] = 1;
$ItemMax[marmor, TargetingLaser] = 1;
$ItemMax[marmor, BouncingMinePack] = 0;
$ItemMax[marmor, APMinePack] = 0;
$ItemMax[marmor, AAMinePack] = 0;

$ItemMax[marmor, Grenade] = 2;
$ItemMax[marmor, Beacon]  = 3;
$ItemMax[marmor, Knife] = 1;
$ItemMax[marmor, SOCOM] = 1;
$ItemMax[marmor, OICW] = 0;
$ItemMax[marmor, SAW] = 0;
$ItemMax[marmor, MP5] = 1;
$ItemMax[marmor, PSG1] = 0;
$ItemMax[marmor, FiftyCal] = 0;
$ItemMax[marmor, LAW] = 0;
$ItemMax[marmor, AutoShotgun] = 0;
$ItemMax[marmor, Howitzer] = 0;
$ItemMax[marmor, Airstrike] = 0;
$ItemMax[marmor, GrappleHook] = 0;
$ItemMax[marmor, Stinger] = 0;
$ItemMax[marmor, Flamethrower] = 0;

$ItemMax[marmor, BulletAmmo] = 100;
$ItemMax[marmor, PlasmaAmmo] = 30;
$ItemMax[marmor, DiscAmmo] = 15;
$ItemMax[marmor, GrenadeAmmo] = 10;
$ItemMax[marmor, MortarAmmo] = 10;
$ItemMax[marmor, SOCOMAmmo] = 15;
$ItemMax[marmor, OICWAmmo] = 0;
$ItemMax[marmor, SAWAmmo] = 0;
$ItemMax[marmor, MP5Ammo] = 30;
$ItemMax[marmor, PSG1Ammo] = 0;
$ItemMax[marmor, FiftyCalAmmo] = 0;
$ItemMax[marmor, LAWAmmo] = 0;
$ItemMax[marmor, AutoShotgunAmmo] = 0;
$ItemMax[marmor, HowitzerAmmo] = 0;
$ItemMax[marmor, StingerAmmo] = 0;
$ItemMax[marmor, FlameAmmo] = 0;

$ItemMax[marmor, SOCOMClip] = 1;
$ItemMax[marmor, OICWClip] = 0;
$ItemMax[marmor, SAWClip] = 0;
$ItemMax[marmor, MP5Clip] = 1;
$ItemMax[marmor, PSG1Clip] = 0;
$ItemMax[marmor, FiftyCalClip] = 0;
$ItemMax[marmor, AutoShotgunClip] = 0;
$ItemMax[marmor, StingerClip] = 0;

$ItemMax[marmor, EnergyPack] = 0;
$ItemMax[marmor, RepairPack] = 0;
$ItemMax[marmor, ShieldPack] = 0;
$ItemMax[marmor, SensorJammerPack] = 0;
$ItemMax[marmor, MotionSensorPack] = 0;
$ItemMax[marmor, PulseSensorPack] = 0;
$ItemMax[marmor, DeployableSensorJammerPack] = 0;
$ItemMax[marmor, DeployableHealthPack] = 1;
$ItemMax[marmor, CameraPack] = 0;
$ItemMax[marmor, TurretPack] = 0;
$ItemMax[marmor, AmmoPackSmall] = 1;
$ItemMax[marmor, AmmoPackHeavy] = 0;
$ItemMax[marmor, AmmoPackExp] = 0;
$ItemMax[marmor, RepairKit] = 1;
$ItemMax[marmor, DeployableInvPack] = 0;
$ItemMax[marmor, DeployableAmmoPack] = 0;
$ItemMax[marmor, TwentyPack] = 0;
$ItemMax[marmor, HowitzerPack] = 0;
$ItemMax[marmor, SAMPack] = 0;
$ItemMax[marmor, Charge] = 0;
$ItemMax[marmor, AirstrikePack] = 0;
$ItemMax[marmor, GrapplePack] = 0;
$ItemMax[marmor, ReloaderPack] = 0;
$ItemMax[marmor, Parachute] = 1;
$ItemMax[marmor, FuelPack] = 0;
$ItemMax[marmor, PortGenPack] = 0;
$ItemMax[marmor, AAPack] = 0;
$ItemMax[marmor, MedicPack] = 1;

$MaxWeapons[marmor] = 2;

// SNIPER

$DamageScale[sarmor, $LandingDamageType] = 1.0;
$DamageScale[sarmor, $ImpactDamageType] = 1.0;
$DamageScale[sarmor, $CrushDamageType] = 1.0;
$DamageScale[sarmor, $BulletDamageType] = 1.2;
$DamageScale[sarmor, $PlasmaDamageType] = 1.0;
$DamageScale[sarmor, $EnergyDamageType] = 1.3;
$DamageScale[sarmor, $ExplosionDamageType] = 1.0;
$DamageScale[sarmor, $MissileDamageType] = 1.0;
$DamageScale[sarmor, $DebrisDamageType] = 1.2;
$DamageScale[sarmor, $ShrapnelDamageType] = 1.2;
$DamageScale[sarmor, $LaserDamageType] = 1.0;
$DamageScale[sarmor, $MortarDamageType] = 1.3;
$DamageScale[sarmor, $BlasterDamageType] = 1.3;
$DamageScale[sarmor, $ElectricityDamageType] = 1.0;
$DamageScale[sarmor, $MineDamageType] = 1.2;
$DamageScale[sarmor, $ChargeDamageType] = 1.2;
$DamageScale[sarmor, $AirstrikeDamageType] = 1.0;
$DamageScale[sarmor, $BleedDamageType] = 1.0;
$DamageScale[sarmor, $GrenadeDamageType] = 1.0;
$DamageScale[sarmor, $PoisonDamageType] = 1.0;
$DamageScale[sarmor, $SmokeDamageType] = 1.0;
$DamageScale[sarmor, $StabDamageType] = 1.0;

$ItemMax[sarmor, Blaster] = 1;
$ItemMax[sarmor, Chaingun] = 1;
$ItemMax[sarmor, Disclauncher] = 1;
$ItemMax[sarmor, GrenadeLauncher] = 1;
$ItemMax[sarmor, Mortar] = 0;
$ItemMax[sarmor, PlasmaGun] = 1;
$ItemMax[sarmor, LaserRifle] = 1;
$ItemMax[sarmor, EnergyRifle] = 1;
$ItemMax[sarmor, TargetingLaser] = 1;
$ItemMax[sarmor, BouncingMinePack] = 0;
$ItemMax[sarmor, APMinePack] = 0;
$ItemMax[sarmor, AAMinePack] = 0;
$ItemMax[sarmor, Grenade] = 2;
$ItemMax[sarmor, Beacon]  = 3;
$ItemMax[sarmor, Knife] = 1;
$ItemMax[sarmor, SOCOM] = 1;
$ItemMax[sarmor, OICW] = 0;
$ItemMax[sarmor, SAW] = 0;
$ItemMax[sarmor, MP5] = 0;
$ItemMax[sarmor, PSG1] = 1;
$ItemMax[sarmor, FiftyCal] = 1;
$ItemMax[sarmor, LAW] = 0;
$ItemMax[sarmor, AutoShotgun] = 0;
$ItemMax[sarmor, Howitzer] = 0;
$ItemMax[sarmor, Airstrike] = 0;
$ItemMax[sarmor, Stinger] = 0;
$ItemMax[sarmor, Flamethrower] = 0;

$ItemMax[sarmor, BulletAmmo] = 100;
$ItemMax[sarmor, PlasmaAmmo] = 30;
$ItemMax[sarmor, DiscAmmo] = 15;
$ItemMax[sarmor, GrenadeAmmo] = 10;
$ItemMax[sarmor, MortarAmmo] = 10;
$ItemMax[sarmor, SOCOMAmmo] = 15;
$ItemMax[sarmor, OICWAmmo] = 0;
$ItemMax[sarmor, SAWAmmo] = 0;
$ItemMax[sarmor, MP5Ammo] = 0;
$ItemMax[sarmor, PSG1Ammo] = 5;
$ItemMax[sarmor, FiftyCalAmmo] = 5;
$ItemMax[sarmor, LAWAmmo] = 0;
$ItemMax[sarmor, AutoShotgunAmmo] = 0;
$ItemMax[sarmor, HowitzerAmmo] = 0;
$ItemMax[sarmor, StingerAmmo] = 0;
$ItemMax[sarmor, FlameAmmo] = 0;

$ItemMax[sarmor, SOCOMClip] = 1;
$ItemMax[sarmor, OICWClip] = 0;
$ItemMax[sarmor, SAWClip] = 0;
$ItemMax[sarmor, MP5Clip] = 0;
$ItemMax[sarmor, PSG1Clip] = 3;
$ItemMax[sarmor, FiftyCalClip] = 2;
$ItemMax[sarmor, AutoShotgunClip] = 0;
$ItemMax[sarmor, StingerClip] = 0;


$ItemMax[sarmor, EnergyPack] = 0;
$ItemMax[sarmor, RepairPack] = 0;
$ItemMax[sarmor, ShieldPack] = 0;
$ItemMax[sarmor, SensorJammerPack] = 1;
$ItemMax[sarmor, MotionSensorPack] = 0;
$ItemMax[sarmor, PulseSensorPack] = 0;
$ItemMax[sarmor, DeployableSensorJammerPack] = 0;
$ItemMax[sarmor, DeployableHealthPack] = 0;
$ItemMax[sarmor, CameraPack] = 0;
$ItemMax[sarmor, TurretPack] = 0;
$ItemMax[sarmor, AmmoPackSmall] = 1;
$ItemMax[sarmor, AmmoPackHeavy] = 1;
$ItemMax[sarmor, AmmoPackExp] = 0;
$ItemMax[sarmor, RepairKit] = 1;
$ItemMax[sarmor, DeployableInvPack] = 0;
$ItemMax[sarmor, DeployableAmmoPack] = 0;
$ItemMax[sarmor, TwentyPack] = 0;
$ItemMax[sarmor, HowitzerPack] = 0;
$ItemMax[sarmor, SAMPack] = 0;
$ItemMax[sarmor, Charge] = 0;
$ItemMax[sarmor, AirstrikePack] = 0;
$ItemMax[sarmor, GrapplePack] = 0;
$ItemMax[sarmor, GrappleHook] = 0;
$ItemMax[sarmor, ReloaderPack] = 0;
$ItemMax[sarmor, Parachute] = 1;
$ItemMax[sarmor, FuelPack] = 0;
$ItemMax[sarmor, PortGenPack] = 0;
$ItemMax[sarmor, AAPack] = 0;
$ItemMax[sarmor, MedicPack] = 0;

$MaxWeapons[sarmor] = 1;

// INFANTRY

$DamageScale[iarmor, $LandingDamageType] = 1.0;
$DamageScale[iarmor, $ImpactDamageType] = 1.0;
$DamageScale[iarmor, $CrushDamageType] = 1.0;
$DamageScale[iarmor, $BulletDamageType] = 1.0;
$DamageScale[iarmor, $PlasmaDamageType] = 0.6;
$DamageScale[iarmor, $EnergyDamageType] = 1.0;
$DamageScale[iarmor, $ExplosionDamageType] = 1.0;
$DamageScale[iarmor, $MissileDamageType] = 1.0;
$DamageScale[iarmor, $ShrapnelDamageType] = 1.0;
$DamageScale[iarmor, $DebrisDamageType] = 1.0;
$DamageScale[iarmor, $LaserDamageType] = 1.0;
$DamageScale[iarmor, $MortarDamageType] = 1.0;
$DamageScale[iarmor, $BlasterDamageType] = 1.0;
$DamageScale[iarmor, $ElectricityDamageType] = 1.0;
$DamageScale[iarmor, $MineDamageType] = 1.0;
$DamageScale[iarmor, $ChargeDamageType] = 1.0;
$DamageScale[iarmor, $AirstrikeDamageType] = 1.0;
$DamageScale[iarmor, $BleedDamageType] = 1.0;
$DamageScale[iarmor, $GrenadeDamageType] = 1.0;
$DamageScale[iarmor, $PoisonDamageType] = 1.0;
$DamageScale[iarmor, $SmokeDamageType] = 1.0;
$DamageScale[iarmor, $StabDamageType] = 1.0;

$ItemMax[iarmor, Blaster] = 1;
$ItemMax[iarmor, Chaingun] = 1;
$ItemMax[iarmor, Disclauncher] = 1;
$ItemMax[iarmor, GrenadeLauncher] = 1;
$ItemMax[iarmor, Mortar] = 0;
$ItemMax[iarmor, PlasmaGun] = 1;
$ItemMax[iarmor, LaserRifle] = 1;
$ItemMax[iarmor, EnergyRifle] = 1;
$ItemMax[iarmor, TargetingLaser] = 1;
$ItemMax[iarmor, BouncingMinePack] = 0;
$ItemMax[iarmor, APMinePack] = 1;
$ItemMax[iarmor, AAMinePack] = 0;
$ItemMax[iarmor, Grenade] = 3;
$ItemMax[iarmor, Beacon]  = 3;
$ItemMax[iarmor, Knife] = 1;
$ItemMax[iarmor, SOCOM] = 1;
$ItemMax[iarmor, OICW] = 1;
$ItemMax[iarmor, SAW] = 1;
$ItemMax[iarmor, MP5] = 0;
$ItemMax[iarmor, PSG1] = 0;
$ItemMax[iarmor, FiftyCal] = 0;
$ItemMax[iarmor, LAW] = 1;
$ItemMax[iarmor, AutoShotgun] = 1;
$ItemMax[iarmor, Stinger] = 0;
$ItemMax[iarmor, Flamethrower] = 1;
$ItemMax[iarmor, Howitzer] = 0;
$ItemMax[iarmor, Airstrike] = 0;

$ItemMax[iarmor, BulletAmmo] = 100;
$ItemMax[iarmor, PlasmaAmmo] = 30;
$ItemMax[iarmor, DiscAmmo] = 15;
$ItemMax[iarmor, GrenadeAmmo] = 10;
$ItemMax[iarmor, MortarAmmo] = 10;
$ItemMax[iarmor, SOCOMAmmo] = 15;
$ItemMax[iarmor, OICWAmmo] = 30;
$ItemMax[iarmor, SAWAmmo] = 200;
$ItemMax[iarmor, MP5Ammo] = 0;
$ItemMax[iarmor, PSG1Ammo] = 0;
$ItemMax[iarmor, FiftyCalAmmo] = 0;
$ItemMax[iarmor, LAWAmmo] = 1;
$ItemMax[iarmor, AutoShotgunAmmo] = 7;
$ItemMax[iarmor, FlameAmmo] = 150;
$ItemMax[iarmor, StingerAmmo] = 0;
$ItemMax[iarmor, HowitzerAmmo] = 0;

$ItemMax[iarmor, SOCOMClip] = 1;
$ItemMax[iarmor, OICWClip] = 2;
$ItemMax[iarmor, SAWClip] = 1;
$ItemMax[iarmor, MP5Clip] = 0;
$ItemMax[iarmor, PSG1Clip] = 0;
$ItemMax[iarmor, FiftyCalClip] = 0;
$ItemMax[iarmor, AutoShotgunClip] = 5;
$ItemMax[iarmor, StingerClip] = 0;

$ItemMax[iarmor, EnergyPack] = 0;
$ItemMax[iarmor, RepairPack] = 1;
$ItemMax[iarmor, ShieldPack] = 0;
$ItemMax[iarmor, SensorJammerPack] = 0;
$ItemMax[iarmor, MotionSensorPack] = 0;
$ItemMax[iarmor, PulseSensorPack] = 0;
$ItemMax[iarmor, DeployableSensorJammerPack] = 0;
$ItemMax[iarmor, DeployableHealthPack] = 0;
$ItemMax[iarmor, CameraPack] = 0;
$ItemMax[iarmor, TurretPack] = 0;
$ItemMax[iarmor, AmmoPackSmall] = 1;
$ItemMax[iarmor, AmmoPackHeavy] = 1;
$ItemMax[iarmor, AmmoPackExp] = 1;
$ItemMax[iarmor, RepairKit] = 1;
$ItemMax[iarmor, DeployableInvPack] = 0;
$ItemMax[iarmor, DeployableAmmoPack] = 0;
$ItemMax[iarmor, TwentyPack] = 0;
$ItemMax[iarmor, HowitzerPack] = 0;
$ItemMax[iarmor, SAMPack] = 0;
$ItemMax[iarmor, Charge] = 0;
$ItemMax[iarmor, AirstrikePack] = 0;
$ItemMax[iarmor, GrapplePack] = 0;
$ItemMax[iarmor, GrappleHook] = 0;
$ItemMax[iarmor, ReloaderPack] = 1;
$ItemMax[iarmor, Parachute] = 1;
$ItemMax[iarmor, FuelPack] = 1;
$ItemMax[iarmor, PortGenPack] = 1;
$ItemMax[iarmor, AAPack] = 0;
$ItemMax[iarmor, MedicPack] = 0;

$MaxWeapons[iarmor] = 2;

// Grenadier

$DamageScale[garmor, $LandingDamageType] = 1.0;
$DamageScale[garmor, $ImpactDamageType] = 1.0;
$DamageScale[garmor, $CrushDamageType] = 1.0;
$DamageScale[garmor, $BulletDamageType] = 1.0;
$DamageScale[garmor, $PlasmaDamageType] = 0.6;
$DamageScale[garmor, $EnergyDamageType] = 1.0;
$DamageScale[garmor, $ExplosionDamageType] = 1.0;
$DamageScale[garmor, $MissileDamageType] = 1.0;
$DamageScale[garmor, $ShrapnelDamageType] = 0.8;
$DamageScale[garmor, $DebrisDamageType] = 1.0;
$DamageScale[garmor, $LaserDamageType] = 1.0;
$DamageScale[garmor, $MortarDamageType] = 1.0;
$DamageScale[garmor, $BlasterDamageType] = 1.0;
$DamageScale[garmor, $ElectricityDamageType] = 1.0;
$DamageScale[garmor, $MineDamageType] = 1.0;
$DamageScale[garmor, $ChargeDamageType] = 1.0;
$DamageScale[garmor, $AirstrikeDamageType] = 1.0;
$DamageScale[garmor, $BleedDamageType] = 1.0;
$DamageScale[garmor, $GrenadeDamageType] = 0.8;
$DamageScale[garmor, $PoisonDamageType] = 1.0;
$DamageScale[garmor, $SmokeDamageType] = 1.0;
$DamageScale[garmor, $StabDamageType] = 1.0;

$ItemMax[garmor, Blaster] = 1;
$ItemMax[garmor, Chaingun] = 1;
$ItemMax[garmor, Disclauncher] = 1;
$ItemMax[garmor, GrenadeLauncher] = 1;
$ItemMax[garmor, Mortar] = 0;
$ItemMax[garmor, PlasmaGun] = 1;
$ItemMax[garmor, LaserRifle] = 1;
$ItemMax[garmor, EnergyRifle] = 1;
$ItemMax[garmor, TargetingLaser] = 1;
$ItemMax[garmor, BouncingMinePack] = 1;
$ItemMax[garmor, APMinePack] = 1;
$ItemMax[garmor, AAMinePack] = 0;
$ItemMax[garmor, Grenade] = 10;
$ItemMax[garmor, Beacon]  = 3;
$ItemMax[garmor, Knife] = 1;
$ItemMax[garmor, SOCOM] = 1;
$ItemMax[garmor, OICW] = 1;
$ItemMax[garmor, SAW] = 0;
$ItemMax[garmor, MP5] = 0;
$ItemMax[garmor, PSG1] = 0;
$ItemMax[garmor, FiftyCal] = 0;
$ItemMax[garmor, LAW] = 0;
$ItemMax[garmor, AutoShotgun] = 0;
$ItemMax[garmor, Stinger] = 0;
$ItemMax[garmor, Flamethrower] = 0;
$ItemMax[garmor, Howitzer] = 0;
$ItemMax[garmor, Airstrike] = 0;

$ItemMax[garmor, BulletAmmo] = 100;
$ItemMax[garmor, PlasmaAmmo] = 30;
$ItemMax[garmor, DiscAmmo] = 15;
$ItemMax[garmor, GrenadeAmmo] = 10;
$ItemMax[garmor, MortarAmmo] = 10;
$ItemMax[garmor, SOCOMAmmo] = 15;
$ItemMax[garmor, OICWAmmo] = 30;
$ItemMax[garmor, SAWAmmo] = 0;
$ItemMax[garmor, MP5Ammo] = 0;
$ItemMax[garmor, PSG1Ammo] = 0;
$ItemMax[garmor, FiftyCalAmmo] = 0;
$ItemMax[garmor, LAWAmmo] = 0;
$ItemMax[garmor, AutoShotgunAmmo] = 0;
$ItemMax[garmor, FlameAmmo] = 0;
$ItemMax[garmor, StingerAmmo] = 0;
$ItemMax[garmor, HowitzerAmmo] = 0;

$ItemMax[garmor, SOCOMClip] = 1;
$ItemMax[garmor, OICWClip] = 1;
$ItemMax[garmor, SAWClip] = 0;
$ItemMax[garmor, MP5Clip] = 0;
$ItemMax[garmor, PSG1Clip] = 0;
$ItemMax[garmor, FiftyCalClip] = 0;
$ItemMax[garmor, AutoShotgunClip] = 0;
$ItemMax[garmor, StingerClip] = 0;

$ItemMax[garmor, EnergyPack] = 0;
$ItemMax[garmor, RepairPack] = 1;
$ItemMax[garmor, ShieldPack] = 0;
$ItemMax[garmor, SensorJammerPack] = 0;
$ItemMax[garmor, MotionSensorPack] = 0;
$ItemMax[garmor, PulseSensorPack] = 0;
$ItemMax[garmor, DeployableSensorJammerPack] = 0;
$ItemMax[garmor, DeployableHealthPack] = 0;
$ItemMax[garmor, CameraPack] = 0;
$ItemMax[garmor, TurretPack] = 0;
$ItemMax[garmor, AmmoPackSmall] = 1;
$ItemMax[garmor, AmmoPackHeavy] = 1;
$ItemMax[garmor, AmmoPackExp] = 1;
$ItemMax[garmor, RepairKit] = 1;
$ItemMax[garmor, DeployableInvPack] = 0;
$ItemMax[garmor, DeployableAmmoPack] = 0;
$ItemMax[garmor, TwentyPack] = 0;
$ItemMax[garmor, HowitzerPack] = 0;
$ItemMax[garmor, SAMPack] = 0;
$ItemMax[garmor, Charge] = 1;
$ItemMax[garmor, AirstrikePack] = 0;
$ItemMax[garmor, GrapplePack] = 0;
$ItemMax[garmor, GrappleHook] = 0;
$ItemMax[garmor, ReloaderPack] = 0;
$ItemMax[garmor, Parachute] = 1;
$ItemMax[garmor, FuelPack] = 0;
$ItemMax[garmor, PortGenPack] = 0;
$ItemMax[garmor, AAPack] = 0;
$ItemMax[garmor, MedicPack] = 0;

$MaxWeapons[garmor] = 1;

// SPECOPS

$DamageScale[carmor, $LandingDamageType] = 1.0;
$DamageScale[carmor, $ImpactDamageType] = 1.0;
$DamageScale[carmor, $CrushDamageType] = 1.0;
$DamageScale[carmor, $BulletDamageType] = 1.2;
$DamageScale[carmor, $PlasmaDamageType] = 1.0;
$DamageScale[carmor, $EnergyDamageType] = 1.3;
$DamageScale[carmor, $ExplosionDamageType] = 1.0;
$DamageScale[carmor, $MissileDamageType] = 1.0;
$DamageScale[carmor, $DebrisDamageType] = 1.2;
$DamageScale[carmor, $ShrapnelDamageType] = 1.2;
$DamageScale[carmor, $LaserDamageType] = 1.0;
$DamageScale[carmor, $MortarDamageType] = 1.3;
$DamageScale[carmor, $BlasterDamageType] = 1.3;
$DamageScale[carmor, $ElectricityDamageType] = 1.0;
$DamageScale[carmor, $MineDamageType] = 1.2;
$DamageScale[carmor, $ChargeDamageType] = 1.2;
$DamageScale[carmor, $AirstrikeDamageType] = 1.0;
$DamageScale[carmor, $BleedDamageType] = 1.0;
$DamageScale[carmor, $GrenadeDamageType] = 1.0;
$DamageScale[carmor, $PoisonDamageType] = 1.0;
$DamageScale[carmor, $SmokeDamageType] = 1.0;
$DamageScale[carmor, $StabDamageType] = 1.0;

$ItemMax[carmor, Blaster] = 1;
$ItemMax[carmor, Chaingun] = 1;
$ItemMax[carmor, Disclauncher] = 1;
$ItemMax[carmor, GrenadeLauncher] = 1;
$ItemMax[carmor, Mortar] = 0;
$ItemMax[carmor, PlasmaGun] = 1;
$ItemMax[carmor, LaserRifle] = 1;
$ItemMax[carmor, EnergyRifle] = 1;
$ItemMax[carmor, TargetingLaser] = 1;
$ItemMax[carmor, BouncingMinePack] = 0;
$ItemMax[carmor, APMinePack] = 0;
$ItemMax[carmor, AAMinePack] = 0;
$ItemMax[carmor, Grenade] = 3;
$ItemMax[carmor, Beacon]  = 3;
$ItemMax[carmor, Knife] = 1;
$ItemMax[carmor, SOCOM] = 1;
$ItemMax[carmor, OICW] = 0;
$ItemMax[carmor, SAW] = 0;
$ItemMax[carmor, MP5] = 1;
$ItemMax[carmor, PSG1] = 0;
$ItemMax[carmor, FiftyCal] = 0;
$ItemMax[carmor, LAW] = 0;
$ItemMax[carmor, AutoShotgun] = 0;
$ItemMax[carmor, Howitzer] = 0;
$ItemMax[carmor, Airstrike] = 1;
$ItemMax[carmor, Stinger] = 1;
$ItemMax[carmor, Flamethrower] = 0;

$ItemMax[carmor, BulletAmmo] = 100;
$ItemMax[carmor, PlasmaAmmo] = 30;
$ItemMax[carmor, DiscAmmo] = 15;
$ItemMax[carmor, GrenadeAmmo] = 10;
$ItemMax[carmor, MortarAmmo] = 10;
$ItemMax[carmor, SOCOMAmmo] = 15;
$ItemMax[carmor, OICWAmmo] = 0;
$ItemMax[carmor, SAWAmmo] = 0;
$ItemMax[carmor, MP5Ammo] = 30;
$ItemMax[carmor, PSG1Ammo] = 0;
$ItemMax[carmor, FiftyCalAmmo] = 0;
$ItemMax[carmor, LAWAmmo] = 0;
$ItemMax[carmor, AutoShotgunAmmo] = 0;
$ItemMax[carmor, HowitzerAmmo] = 0;
$ItemMax[carmor, StingerAmmo] = 1;
$ItemMax[carmor, FlameAmmo] = 0;

$ItemMax[carmor, SOCOMClip] = 1;
$ItemMax[carmor, OICWClip] = 0;
$ItemMax[carmor, SAWClip] = 0;
$ItemMax[carmor, MP5Clip] = 2;
$ItemMax[carmor, PSG1Clip] = 0;
$ItemMax[carmor, FiftyCalClip] = 0;
$ItemMax[carmor, AutoShotgunClip] = 0;
$ItemMax[carmor, StingerClip] = 1;

$ItemMax[carmor, EnergyPack] = 0;
$ItemMax[carmor, RepairPack] = 0;
$ItemMax[carmor, ShieldPack] = 0;
$ItemMax[carmor, SensorJammerPack] = 1;
$ItemMax[carmor, MotionSensorPack] = 0;
$ItemMax[carmor, PulseSensorPack] = 0;
$ItemMax[carmor, DeployableSensorJammerPack] = 0;
$ItemMax[carmor, DeployableHealthPack] = 0;
$ItemMax[carmor, CameraPack] = 1;
$ItemMax[carmor, TurretPack] = 0;
$ItemMax[carmor, AmmoPackSmall] = 1;
$ItemMax[carmor, AmmoPackHeavy] = 0;
$ItemMax[carmor, AmmoPackExp] = 1;
$ItemMax[carmor, RepairKit] = 1;
$ItemMax[carmor, DeployableInvPack] = 0;
$ItemMax[carmor, DeployableAmmoPack] = 0;
$ItemMax[carmor, TwentyPack] = 0;
$ItemMax[carmor, HowitzerPack] = 0;
$ItemMax[carmor, SAMPack] = 0;
$ItemMax[carmor, Charge] = 1;
$ItemMax[carmor, AirstrikePack] = 1;
$ItemMax[carmor, GrapplePack] = 1;
$ItemMax[carmor, GrappleHook] = 1;
$ItemMax[carmor, ReloaderPack] = 1;
$ItemMax[carmor, Parachute] = 1;
$ItemMax[carmor, FuelPack] = 0;
$ItemMax[carmor, PortGenPack] = 0;
$ItemMax[carmor, AAPack] = 0;
$ItemMax[carmor, MedicPack] = 0;

$MaxWeapons[carmor] = 3;

// ENGINEER

$DamageScale[earmor, $LandingDamageType] = 1.0;
$DamageScale[earmor, $ImpactDamageType] = 1.0;
$DamageScale[earmor, $CrushDamageType] = 1.0;
$DamageScale[earmor, $BulletDamageType] = 1.0;
$DamageScale[earmor, $PlasmaDamageType] = 0.6;
$DamageScale[earmor, $EnergyDamageType] = 1.0;
$DamageScale[earmor, $ExplosionDamageType] = 1.0;
$DamageScale[earmor, $MissileDamageType] = 1.0;
$DamageScale[earmor, $ShrapnelDamageType] = 1.0;
$DamageScale[earmor, $DebrisDamageType] = 1.0;
$DamageScale[earmor, $LaserDamageType] = 1.0;
$DamageScale[earmor, $MortarDamageType] = 1.0;
$DamageScale[earmor, $BlasterDamageType] = 1.0;
$DamageScale[earmor, $ElectricityDamageType] = 1.0;
$DamageScale[earmor, $MineDamageType] = 1.0;
$DamageScale[earmor, $ChargeDamageType] = 1.0;
$DamageScale[earmor, $AirstrikeDamageType] = 1.0;
$DamageScale[earmor, $BleedDamageType] = 1.0;
$DamageScale[earmor, $GrenadeDamageType] = 1.0;
$DamageScale[earmor, $PoisonDamageType] = 1.0;
$DamageScale[earmor, $SmokeDamageType] = 1.0;
$DamageScale[earmor, $StabDamageType] = 1.0;

$ItemMax[earmor, Blaster] = 1;
$ItemMax[earmor, Chaingun] = 1;
$ItemMax[earmor, Disclauncher] = 1;
$ItemMax[earmor, GrenadeLauncher] = 1;
$ItemMax[earmor, Mortar] = 0;
$ItemMax[earmor, PlasmaGun] = 1;
$ItemMax[earmor, LaserRifle] = 1;
$ItemMax[earmor, EnergyRifle] = 1;
$ItemMax[earmor, TargetingLaser] = 1;
$ItemMax[earmor, BouncingMinePack] = 1;
$ItemMax[earmor, APMinePack] = 1;
$ItemMax[earmor, AAMinePack] = 1;
$ItemMax[earmor, Grenade] = 2;
$ItemMax[earmor, Beacon]  = 3;
$ItemMax[earmor, Knife] = 1;
$ItemMax[earmor, SOCOM] = 1;
$ItemMax[earmor, OICW] = 1;
$ItemMax[earmor, SAW] = 0;
$ItemMax[earmor, MP5] = 0;
$ItemMax[earmor, PSG1] = 0;
$ItemMax[earmor, FiftyCal] = 0;
$ItemMax[earmor, LAW] = 0;
$ItemMax[earmor, AutoShotgun] = 1;
$ItemMax[earmor, Howitzer] = 0;
$ItemMax[earmor, Airstrike] = 0;
$ItemMax[earmor, Stinger] = 0;
$ItemMax[earmor, Flamethrower] = 0;

$ItemMax[earmor, BulletAmmo] = 100;
$ItemMax[earmor, PlasmaAmmo] = 30;
$ItemMax[earmor, DiscAmmo] = 15;
$ItemMax[earmor, GrenadeAmmo] = 10;
$ItemMax[earmor, MortarAmmo] = 10;
$ItemMax[earmor, SOCOMAmmo] = 15;
$ItemMax[earmor, OICWAmmo] = 30;
$ItemMax[earmor, SAWAmmo] = 0;
$ItemMax[earmor, MP5Ammo] = 0;
$ItemMax[earmor, PSG1Ammo] = 0;
$ItemMax[earmor, FiftyCalAmmo] = 0;
$ItemMax[earmor, LAWAmmo] = 0;
$ItemMax[earmor, AutoShotgunAmmo] = 7;
$ItemMax[earmor, HowitzerAmmo] = 0;
$ItemMax[earmor, StingerAmmo] = 0;
$ItemMax[earmor, FlameAmmo] = 0;

$ItemMax[earmor, SOCOMClip] = 1;
$ItemMax[earmor, OICWClip] = 1;
$ItemMax[earmor, SAWClip] = 0;
$ItemMax[earmor, MP5Clip] = 0;
$ItemMax[earmor, PSG1Clip] = 0;
$ItemMax[earmor, FiftyCalClip] = 0;
$ItemMax[earmor, AutoShotgunClip] = 4;
$ItemMax[earmor, StingerClip] = 0;

$ItemMax[earmor, EnergyPack] = 0;
$ItemMax[earmor, RepairPack] = 1;
$ItemMax[earmor, ShieldPack] = 0;
$ItemMax[earmor, SensorJammerPack] = 1;
$ItemMax[earmor, MotionSensorPack] = 1;
$ItemMax[earmor, PulseSensorPack] = 1;
$ItemMax[earmor, DeployableSensorJammerPack] = 1;
$ItemMax[earmor, DeployableHealthPack] = 0;
$ItemMax[earmor, CameraPack] = 1;
$ItemMax[earmor, TurretPack] = 1;
$ItemMax[earmor, AmmoPackSmall] = 1;
$ItemMax[earmor, AmmoPackHeavy] = 1;
$ItemMax[earmor, AmmoPackExp] = 0;
$ItemMax[earmor, RepairKit] = 1;
$ItemMax[earmor, DeployableInvPack] = 1;
$ItemMax[earmor, DeployableAmmoPack] = 1;
$ItemMax[earmor, TwentyPack] = 1;
$ItemMax[earmor, HowitzerPack] = 1;
$ItemMax[earmor, SAMPack] = 1;
$ItemMax[earmor, Charge] = 0;
$ItemMax[earmor, AirstrikePack] = 0;
$ItemMax[earmor, GrapplePack] = 0;
$ItemMax[earmor, GrappleHook] = 0;
$ItemMax[earmor, ReloaderPack] = 1;
$ItemMax[earmor, Parachute] = 1;
$ItemMax[earmor, FuelPack] = 0;
$ItemMax[earmor, PortGenPack] = 1;
$ItemMax[earmor, AAPack] = 1;
$ItemMax[earmor, MedicPack] = 0;

$MaxWeapons[earmor] = 2;

// PILOT

$DamageScale[larmor, $LandingDamageType] = 1.0;
$DamageScale[larmor, $ImpactDamageType] = 1.0;
$DamageScale[larmor, $CrushDamageType] = 1.0;
$DamageScale[larmor, $BulletDamageType] = 1.2;
$DamageScale[larmor, $PlasmaDamageType] = 1.0;
$DamageScale[larmor, $EnergyDamageType] = 1.3;
$DamageScale[larmor, $ExplosionDamageType] = 1.0;
$DamageScale[larmor, $MissileDamageType] = 1.0;
$DamageScale[larmor, $DebrisDamageType] = 1.2;
$DamageScale[larmor, $ShrapnelDamageType] = 1.2;
$DamageScale[larmor, $LaserDamageType] = 1.0;
$DamageScale[larmor, $MortarDamageType] = 1.3;
$DamageScale[larmor, $BlasterDamageType] = 1.3;
$DamageScale[larmor, $ElectricityDamageType] = 1.0;
$DamageScale[larmor, $MineDamageType] = 1.2;
$DamageScale[larmor, $ChargeDamageType] = 1.2;
$DamageScale[larmor, $AirstrikeDamageType] = 1.0;
$DamageScale[larmor, $BleedDamageType] = 1.0;
$DamageScale[larmor, $GrenadeDamageType] = 1.0;
$DamageScale[larmor, $PoisonDamageType] = 1.0;
$DamageScale[larmor, $SmokeDamageType] = 1.0;
$DamageScale[larmor, $StabDamageType] = 1.0;

$ItemMax[larmor, Blaster] = 1;
$ItemMax[larmor, Chaingun] = 1;
$ItemMax[larmor, Disclauncher] = 1;
$ItemMax[larmor, GrenadeLauncher] = 1;
$ItemMax[larmor, Mortar] = 0;
$ItemMax[larmor, PlasmaGun] = 1;
$ItemMax[larmor, LaserRifle] = 1;
$ItemMax[larmor, EnergyRifle] = 1;
$ItemMax[larmor, TargetingLaser] = 1;
$ItemMax[larmor, BouncingMinePack] = 0;
$ItemMax[larmor, APMinePack] = 0;
$ItemMax[larmor, AAMinePack] = 0;
$ItemMax[larmor, Grenade] = 2;
$ItemMax[larmor, Beacon]  = 3;
$ItemMax[larmor, Knife] = 1;
$ItemMax[larmor, SOCOM] = 1;
$ItemMax[larmor, OICW] = 0;
$ItemMax[larmor, SAW] = 0;
$ItemMax[larmor, MP5] = 0;
$ItemMax[larmor, PSG1] = 0;
$ItemMax[larmor, FiftyCal] = 0;
$ItemMax[larmor, LAW] = 0;
$ItemMax[larmor, Howitzer] = 0;
$ItemMax[larmor, AutoShotgun] = 0;
$ItemMax[larmor, Airstrike] = 0;
$ItemMax[larmor, Stinger] = 0;
$ItemMax[larmor, Flamethrower] = 0;

$ItemMax[larmor, BulletAmmo] = 100;
$ItemMax[larmor, PlasmaAmmo] = 30;
$ItemMax[larmor, DiscAmmo] = 15;
$ItemMax[larmor, GrenadeAmmo] = 10;
$ItemMax[larmor, MortarAmmo] = 10;
$ItemMax[larmor, SOCOMAmmo] = 15;
$ItemMax[larmor, OICWAmmo] = 0;
$ItemMax[larmor, SAWAmmo] = 0;
$ItemMax[larmor, MP5Ammo] = 0;
$ItemMax[larmor, PSG1Ammo] = 0;
$ItemMax[larmor, FiftyCalAmmo] = 0;
$ItemMax[larmor, LAWAmmo] = 0;
$ItemMax[larmor, AutoShotgun] = 0;
$ItemMax[larmor, HowitzerAmmo] = 0;
$ItemMax[larmor, StingerAmmo] = 0;
$ItemMax[larmor, FlameAmmo] = 0;

$ItemMax[larmor, SOCOMClip] = 1;
$ItemMax[larmor, OICWClip] = 0;
$ItemMax[larmor, SAWClip] = 0;
$ItemMax[larmor, MP5Clip] = 0;
$ItemMax[larmor, PSG1Clip] = 0;
$ItemMax[larmor, FiftyCalClip] = 0;
$ItemMax[larmor, AutoShotgunClip] = 0;
$ItemMax[larmor, StingerClip] = 0;

$ItemMax[larmor, EnergyPack] = 0;
$ItemMax[larmor, RepairPack] = 0;
$ItemMax[larmor, ShieldPack] = 0;
$ItemMax[larmor, SensorJammerPack] = 0;
$ItemMax[larmor, MotionSensorPack] = 0;
$ItemMax[larmor, PulseSensorPack] = 0;
$ItemMax[larmor, DeployableSensorJammerPack] = 0;
$ItemMax[larmor, DeployableHealthPack] = 0;
$ItemMax[larmor, CameraPack] = 0;
$ItemMax[larmor, TurretPack] = 0;
$ItemMax[larmor, AmmoPackSmall] = 1;
$ItemMax[larmor, AmmoPackHeavy] = 0;
$ItemMax[larmor, AmmoPackExp] = 0;
$ItemMax[larmor, RepairKit] = 1;
$ItemMax[larmor, DeployableInvPack] = 0;
$ItemMax[larmor, DeployableAmmoPack] = 0;
$ItemMax[larmor, TwentyPack] = 0;
$ItemMax[larmor, HowitzerPack] = 0;
$ItemMax[larmor, SAMPack] = 0;
$ItemMax[larmor, Charge] = 0;
$ItemMax[larmor, AirstrikePack] = 0;
$ItemMax[larmor, GrapplePack] = 0;
$ItemMax[larmor, GrappleHook] = 0;
$ItemMax[larmor, ReloaderPack] = 0;
$ItemMax[larmor, Parachute] = 1;
$ItemMax[larmor, FuelPack] = 0;
$ItemMax[larmor, PortGenPack] = 0;
$ItemMax[larmor, AAPack] = 0;
$ItemMax[larmor, MedicPack] = 0;

$MaxWeapons[larmor] = 1;

// ARTILLERY

$DamageScale[aarmor, $LandingDamageType] = 1.0;
$DamageScale[aarmor, $ImpactDamageType] = 1.0;
$DamageScale[aarmor, $CrushDamageType] = 1.0;
$DamageScale[aarmor, $BulletDamageType] = 1.2;
$DamageScale[aarmor, $PlasmaDamageType] = 1.0;
$DamageScale[aarmor, $EnergyDamageType] = 1.3;
$DamageScale[aarmor, $ExplosionDamageType] = 1.0;
$DamageScale[aarmor, $MissileDamageType] = 1.0;
$DamageScale[aarmor, $DebrisDamageType] = 1.2;
$DamageScale[aarmor, $ShrapnelDamageType] = 1.2;
$DamageScale[aarmor, $LaserDamageType] = 1.0;
$DamageScale[aarmor, $MortarDamageType] = 1.3;
$DamageScale[aarmor, $BlasterDamageType] = 1.3;
$DamageScale[aarmor, $ElectricityDamageType] = 1.0;
$DamageScale[aarmor, $MineDamageType] = 1.2;
$DamageScale[aarmor, $ChargeDamageType] = 1.2;
$DamageScale[aarmor, $AirstrikeDamageType] = 1.0;
$DamageScale[aarmor, $BleedDamageType] = 1.0;
$DamageScale[aarmor, $GrenadeDamageType] = 1.0;
$DamageScale[aarmor, $PoisonDamageType] = 1.0;
$DamageScale[aarmor, $SmokeDamageType] = 1.0;
$DamageScale[aarmor, $StabDamageType] = 1.0;

//$DamageScale[aarmor, $LandingDamageType] = 1.0;
//$DamageScale[aarmor, $ImpactDamageType] = 1.0;
//$DamageScale[aarmor, $CrushDamageType] = 1.0;
//$DamageScale[aarmor, $BulletDamageType] = 0.6;
//$DamageScale[aarmor, $PlasmaDamageType] = 0.4;
//$DamageScale[aarmor, $EnergyDamageType] = 0.7;
//$DamageScale[aarmor, $ExplosionDamageType] = 0.6;
//$DamageScale[aarmor, $MissileDamageType] = 0.6;
//$DamageScale[aarmor, $DebrisDamageType] = 0.8;
//$DamageScale[aarmor, $ShrapnelDamageType] = 0.8;
//$DamageScale[aarmor, $LaserDamageType] = 0.6;
//$DamageScale[aarmor, $MortarDamageType] = 0.7;
//$DamageScale[aarmor, $BlasterDamageType] = 0.7;
//$DamageScale[aarmor, $ElectricityDamageType] = 1.0;
//$DamageScale[aarmor, $MineDamageType] = 0.8;
//$DamageScale[aarmor, $ChargeDamageType] = 0.8;
//$DamageScale[aarmor, $AirstrikeDamageType] = 1.0;

$ItemMax[aarmor, Blaster] = 0;
$ItemMax[aarmor, Chaingun] = 0;
$ItemMax[aarmor, Disclauncher] = 0;
$ItemMax[aarmor, GrenadeLauncher] = 0;
$ItemMax[aarmor, Mortar] = 0;
$ItemMax[aarmor, PlasmaGun] = 0;
$ItemMax[aarmor, LaserRifle] = 0;
$ItemMax[aarmor, EnergyRifle] = 0;
$ItemMax[aarmor, TargetingLaser] = 1;
$ItemMax[aarmor, BouncingMinePack] = 1;
$ItemMax[aarmor, APMinePack] = 1;
$ItemMax[aarmor, AAMinePack] = 1;
$ItemMax[aarmor, Grenade] = 3;
$ItemMax[aarmor, Beacon]  = 3;
$ItemMax[aarmor, Knife] = 1;
$ItemMax[aarmor, SOCOM] = 1;
$ItemMax[aarmor, OICW] = 1;
$ItemMax[aarmor, SAW] = 0;
$ItemMax[aarmor, MP5] = 0;
$ItemMax[aarmor, PSG1] = 0;
$ItemMax[aarmor, FiftyCal] = 0;
$ItemMax[aarmor, LAW] = 0;
$ItemMax[aarmor, AutoShotgun] = 0;
$ItemMax[aarmor, Howitzer] = 0;
$ItemMax[aarmor, Airstrike] = 1;
$ItemMax[aarmor, Stinger] = 0;
$ItemMax[aarmor, Flamethrower] = 0;

$ItemMax[aarmor, BulletAmmo] = 0;
$ItemMax[aarmor, PlasmaAmmo] = 0;
$ItemMax[aarmor, DiscAmmo] = 0;
$ItemMax[aarmor, GrenadeAmmo] = 10;
$ItemMax[aarmor, MortarAmmo] = 10;
$ItemMax[aarmor, SOCOMAmmo] = 15;
$ItemMax[aarmor, OICWAmmo] = 30;
$ItemMax[aarmor, SAWAmmo] = 0;
$ItemMax[aarmor, MP5Ammo] = 0;
$ItemMax[aarmor, PSG1Ammo] = 0;
$ItemMax[aarmor, FiftyCalAmmo] = 0;
$ItemMax[aarmor, LAWAmmo] = 0;
$ItemMax[aarmor, AutoShotgunAmmo] = 0;
$ItemMax[aarmor, HowitzerAmmo] = 0;
$ItemMax[aarmor, Stinger] = 0;
$ItemMax[aarmor, FlameAmmo] = 0;

$ItemMax[iarmor, SOCOMClip] = 1;
$ItemMax[iarmor, OICWClip] = 1;
$ItemMax[iarmor, SAWClip] = 0;
$ItemMax[iarmor, MP5Clip] = 0;
$ItemMax[iarmor, PSG1Clip] = 0;
$ItemMax[iarmor, FiftyCalClip] = 0;
$ItemMax[iarmor, AutoShotgunClip] = 0;
$ItemMax[iarmor, StingerClip] = 0;

$ItemMax[aarmor, EnergyPack] = 0;
$ItemMax[aarmor, RepairPack] = 0;
$ItemMax[aarmor, ShieldPack] = 0;
$ItemMax[aarmor, SensorJammerPack] = 0;
$ItemMax[aarmor, MotionSensorPack] = 0;
$ItemMax[aarmor, PulseSensorPack] = 0;
$ItemMax[aarmor, DeployableSensorJammerPack] = 0;
$ItemMax[aarmor, DeployableHealthPack] = 0;
$ItemMax[aarmor, CameraPack] = 0;
$ItemMax[aarmor, TurretPack] = 0;
$ItemMax[aarmor, AmmoPack] = 1;
$ItemMax[aarmor, RepairKit] = 1;
$ItemMax[aarmor, DeployableInvPack] = 0;
$ItemMax[aarmor, DeployableAmmoPack] = 0;
$ItemMax[aarmor, TwentyPack] = 0;
$ItemMax[aarmor, HowitzerPack] = 1;
$ItemMax[aarmor, SAMPack] = 0;
$ItemMax[aarmor, Charge] = 0;
$ItemMax[aarmor, AirstrikePack] = 1;
$ItemMax[aarmor, GrapplePack] = 0;
$ItemMax[aarmor, GrappleHook] = 0;
$ItemMax[aarmor, ReloaderPack] = 1;
$ItemMax[aarmor, Parachute] = 1;
$ItemMax[aarmor, FuelPack] = 0;
$ItemMax[aarmor, PortGenPack] = 0;
$ItemMax[aarmor, AAPack] = 0;
$ItemMax[aarmor, MedicPack] = 0;

$MaxWeapons[aarmor] = 2;

// MEDIC FEMALE

$DamageScale[mfemale, $LandingDamageType] = 1.0;
$DamageScale[mfemale, $ImpactDamageType] = 1.0;
$DamageScale[mfemale, $CrushDamageType] = 1.0;
$DamageScale[mfemale, $BulletDamageType] = 1.2;
$DamageScale[mfemale, $PlasmaDamageType] = 1.0;
$DamageScale[mfemale, $EnergyDamageType] = 1.3;
$DamageScale[mfemale, $ExplosionDamageType] = 1.0;
$DamageScale[mfemale, $MissileDamageType] = 1.0;
$DamageScale[mfemale, $DebrisDamageType] = 1.2;
$DamageScale[mfemale, $ShrapnelDamageType] = 1.2;
$DamageScale[mfemale, $LaserDamageType] = 1.0;
$DamageScale[mfemale, $MortarDamageType] = 1.3;
$DamageScale[mfemale, $BlasterDamageType] = 1.3;
$DamageScale[mfemale, $ElectricityDamageType] = 1.0;
$DamageScale[mfemale, $MineDamageType] = 1.2;
$DamageScale[mfemale, $ChargeDamageType] = 1.2;
$DamageScale[mfemale, $AirstrikeDamageType] = 1.0;
$DamageScale[mfemale, $BleedDamageType] = 1.0;
$DamageScale[mfemale, $GrenadeDamageType] = 1.0;
$DamageScale[mfemale, $PoisonDamageType] = 1.0;
$DamageScale[mfemale, $SmokeDamageType] = 1.0;
$DamageScale[mfemale, $StabDamageType] = 1.0;

$ItemMax[mfemale, Blaster] = 1;
$ItemMax[mfemale, Chaingun] = 1;
$ItemMax[mfemale, Disclauncher] = 1;
$ItemMax[mfemale, GrenadeLauncher] = 1;
$ItemMax[mfemale, Mortar] = 0;
$ItemMax[mfemale, PlasmaGun] = 1;
$ItemMax[mfemale, LaserRifle] = 1;
$ItemMax[mfemale, EnergyRifle] = 1;
$ItemMax[mfemale, TargetingLaser] = 1;
$ItemMax[mfemale, BouncingMinePack] = 0;
$ItemMax[mfemale, APMinePack] = 0;
$ItemMax[mfemale, AAMinePack] = 0;
$ItemMax[mfemale, Grenade] = 2;
$ItemMax[mfemale, Beacon]  = 3;
$ItemMax[mfemale, Knife] = 1;
$ItemMax[mfemale, SOCOM] = 1;
$ItemMax[mfemale, OICW] = 0;
$ItemMax[mfemale, SAW] = 0;
$ItemMax[mfemale, MP5] = 1;
$ItemMax[mfemale, PSG1] = 0;
$ItemMax[mfemale, FiftyCal] = 0;
$ItemMax[mfemale, LAW] = 0;
$ItemMax[mfemale, AutoShotgun] = 0;
$ItemMax[mfemale, Howitzer] = 0;
$ItemMax[mfemale, Airstrike] = 0;
$ItemMax[mfemale, Stinger] = 0;
$ItemMax[mfemale, Flamethrower] = 0;

$ItemMax[mfemale, BulletAmmo] = 100;
$ItemMax[mfemale, PlasmaAmmo] = 30;
$ItemMax[mfemale, DiscAmmo] = 15;
$ItemMax[mfemale, GrenadeAmmo] = 10;
$ItemMax[mfemale, MortarAmmo] = 10;
$ItemMax[mfemale, SOCOMAmmo] = 15;
$ItemMax[mfemale, OICWAmmo] = 0;
$ItemMax[mfemale, SAWAmmo] = 0;
$ItemMax[mfemale, MP5Ammo] = 30;
$ItemMax[mfemale, PSG1Ammo] = 0;
$ItemMax[mfemale, FiftyCalAmmo] = 0;
$ItemMax[mfemale, LAWAmmo] = 0;
$ItemMax[mfemale, AutoShotgunAmmo] = 0;
$ItemMax[mfemale, HowitzerAmmo] = 0;
$ItemMax[mfemale, StingerAmmo] = 0;
$ItemMax[mfemale, FlameAmmo] = 0;

$ItemMax[mfemale, SOCOMClip] = 1;
$ItemMax[mfemale, OICWClip] = 0;
$ItemMax[mfemale, SAWClip] = 0;
$ItemMax[mfemale, MP5Clip] = 1;
$ItemMax[mfemale, PSG1Clip] = 0;
$ItemMax[mfemale, FiftyCalClip] = 0;
$ItemMax[mfemale, AutoShotgunClip] = 0;
$ItemMax[mfemale, StingerClip] = 0;

$ItemMax[mfemale, EnergyPack] = 0;
$ItemMax[mfemale, RepairPack] = 0;
$ItemMax[mfemale, ShieldPack] = 0;
$ItemMax[mfemale, SensorJammerPack] = 0;
$ItemMax[mfemale, MotionSensorPack] = 0;
$ItemMax[mfemale, PulseSensorPack] = 0;
$ItemMax[mfemale, DeployableSensorJammerPack] = 0;
$ItemMax[mfemale, DeployableHealthPack] = 1;
$ItemMax[mfemale, CameraPack] = 0;
$ItemMax[mfemale, TurretPack] = 0;
$ItemMax[mfemale, AmmoPackSmall] = 1;
$ItemMax[mfemale, AmmoPackHeavy] = 0;
$ItemMax[mfemale, AmmoPackExp] = 0;
$ItemMax[mfemale, RepairKit] = 1;
$ItemMax[mfemale, DeployableInvPack] = 0;
$ItemMax[mfemale, DeployableAmmoPack] = 0;
$ItemMax[mfemale, TwentyPack] = 0;
$ItemMax[mfemale, HowitzerPack] = 0;
$ItemMax[mfemale, SAMPack] = 0;
$ItemMax[mfemale, Charge] = 0;
$ItemMax[mfemale, AirstrikePack] = 0;
$ItemMax[mfemale, GrapplePack] = 0;
$ItemMax[mfemale, GrappleHook] = 0;
$ItemMax[mfemale, ReloaderPack] = 0;
$ItemMax[mfemale, Parachute] = 1;
$ItemMax[mfemale, FuelPack] = 0;
$ItemMax[mfemale, PortGenPack] = 0;
$ItemMax[mfemale, AAPack] = 0;
$ItemMax[mfemale, MedicPack] = 1;

$MaxWeapons[mfemale] = 2;

// SNIPER FEMALE

$DamageScale[sfemale, $LandingDamageType] = 1.0;
$DamageScale[sfemale, $ImpactDamageType] = 1.0;	
$DamageScale[sfemale, $CrushDamageType] = 1.0;	
$DamageScale[sfemale, $BulletDamageType] = 1.2;
$DamageScale[sfemale, $PlasmaDamageType] = 1.0;
$DamageScale[sfemale, $EnergyDamageType] = 1.3;
$DamageScale[sfemale, $ExplosionDamageType] = 1.0;
$DamageScale[sfemale, $MissileDamageType] = 1.0;
$DamageScale[sfemale, $ShrapnelDamageType] = 1.2;
$DamageScale[sfemale, $DebrisDamageType] = 1.2;
$DamageScale[sfemale, $LaserDamageType] = 1.0;
$DamageScale[sfemale, $MortarDamageType] = 1.3;
$DamageScale[sfemale, $BlasterDamageType] = 1.3;
$DamageScale[sfemale, $ElectricityDamageType] = 1.0;
$DamageScale[sfemale, $MineDamageType] = 1.2;
$DamageScale[sfemale, $ChargeDamageType] = 1.2;
$DamageScale[sfemale, $AirstrikeDamageType] = 1.0;
$DamageScale[sfemale, $BleedDamageType] = 1.0;
$DamageScale[sfemale, $GrenadeDamageType] = 1.0;
$DamageScale[sfemale, $PoisonDamageType] = 1.0;
$DamageScale[sfemale, $SmokeDamageType] = 1.0;
$DamageScale[sfemale, $StabDamageType] = 1.0;

$ItemMax[sfemale, Blaster] = 1;
$ItemMax[sfemale, Chaingun] = 1;
$ItemMax[sfemale, Disclauncher] = 1;
$ItemMax[sfemale, GrenadeLauncher] = 1;
$ItemMax[sfemale, Mortar] = 0;
$ItemMax[sfemale, PlasmaGun] = 1;
$ItemMax[sfemale, LaserRifle] = 1;
$ItemMax[sfemale, EnergyRifle] = 1;
$ItemMax[sfemale, TargetingLaser] = 1;
$ItemMax[sfemale, BouncingMinePack] = 0;
$ItemMax[sfemale, APMinePack] = 0;
$ItemMax[sfemale, AAMinePack] = 0;
$ItemMax[sfemale, Grenade] = 2;
$ItemMax[sfemale, Beacon] = 3;
$ItemMax[sfemale, Knife] = 1;
$ItemMax[sfemale, SOCOM] = 1;
$ItemMax[sfemale, OICW] = 0;
$ItemMax[sfemale, SAW] = 0;
$ItemMax[sfemale, MP5] = 0;
$ItemMax[sfemale, PSG1] = 1;
$ItemMax[sfemale, FiftyCal] = 1;
$ItemMax[sfemale, LAW] = 0;
$ItemMax[sfemale, AutoShotgun] = 0;
$ItemMax[sfemale, Howitzer] = 0;
$ItemMax[sfemale, Airstrike] = 0;
$ItemMax[sfemale, Stinger] = 0;
$ItemMax[sfemale, Flamethrower] = 0;

$ItemMax[sfemale, BulletAmmo] = 100;
$ItemMax[sfemale, PlasmaAmmo] = 30;
$ItemMax[sfemale, DiscAmmo] = 15;
$ItemMax[sfemale, GrenadeAmmo] = 10;
$ItemMax[sfemale, MortarAmmo] = 10;
$ItemMax[sfemale, SOCOMAmmo] = 15;
$ItemMax[sfemale, OICWAmmo] = 0;
$ItemMax[sfemale, SAWAmmo] = 0;
$ItemMax[sfemale, MP5Ammo] = 0;
$ItemMax[sfemale, PSG1Ammo] = 5;
$ItemMax[sfemale, FiftyCalAmmo] = 5;
$ItemMax[sfemale, LAWAmmo] = 0;
$ItemMax[sfemale, AutoShotgunAmmo] = 0;
$ItemMax[sfemale, HowitzerAmmo] = 0;
$ItemMax[sfemale, StingerAmmo] = 0;
$ItemMax[sfemale, FlameAmmo] = 0;

$ItemMax[sfemale, SOCOMClip] = 1;
$ItemMax[sfemale, OICWClip] = 0;
$ItemMax[sfemale, SAWClip] = 0;
$ItemMax[sfemale, MP5Clip] = 0;
$ItemMax[sfemale, PSG1Clip] = 3;
$ItemMax[sfemale, FiftyCalClip] = 2;
$ItemMax[sfemale, AutoShotgunClip] = 0;
$ItemMax[sfemale, StingerClip] = 0;

$ItemMax[sfemale, EnergyPack] = 0;
$ItemMax[sfemale, RepairPack] = 0;
$ItemMax[sfemale, ShieldPack] = 0;
$ItemMax[sfemale, SensorJammerPack] = 1;
$ItemMax[sfemale, MotionSensorPack] = 0;
$ItemMax[sfemale, PulseSensorPack] = 0;
$ItemMax[sfemale, DeployableSensorJammerPack] = 0;
$ItemMax[sfemale, DeployableHealthPack] = 0;
$ItemMax[sfemale, CameraPack] = 0;
$ItemMax[sfemale, TurretPack] = 0;
$ItemMax[sfemale, AmmoPackSmall] = 1;
$ItemMax[sfemale, AmmoPackHeavy] = 1;
$ItemMax[sfemale, AmmoPackExp] = 0;
$ItemMax[sfemale, RepairKit] = 1;
$ItemMax[sfemale, DeployableInvPack] = 0;
$ItemMax[sfemale, DeployableAmmoPack] = 0;
$ItemMax[sfemale, TwentyPack] = 0;
$ItemMax[sfemale, HowitzerPack] = 0;
$ItemMax[sfemale, SAMPack] = 0;
$ItemMax[sfemale, Charge] = 0;
$ItemMax[sfemale, AirstrikePack] = 0;
$ItemMax[sfemale, GrapplePack] = 0;
$ItemMax[sfemale, GrappleHook] = 0;
$ItemMax[sfemale, ReloaderPack] = 0;
$ItemMax[sfemale, Parachute] = 1;
$ItemMax[sfemale, FuelPack] = 0;
$ItemMax[sfemale, PortGenPack] = 0;
$ItemMax[sfemale, AAPack] = 0;
$ItemMax[sfemale, MedicPack] = 0;

$MaxWeapons[sfemale] = 1;

// INFANTRY FEMALE

$DamageScale[ifemale, $LandingDamageType] = 1.0;
$DamageScale[ifemale, $ImpactDamageType] = 1.0;
$DamageScale[ifemale, $CrushDamageType] = 1.0;
$DamageScale[ifemale, $BulletDamageType] = 1.0;
$DamageScale[ifemale, $EnergyDamageType] = 1.0;
$DamageScale[ifemale, $PlasmaDamageType] = 0.6;
$DamageScale[ifemale, $ExplosionDamageType] = 1.0;
$DamageScale[ifemale, $MissileDamageType] = 1.0;
$DamageScale[ifemale, $ShrapnelDamageType] = 1.0;
$DamageScale[ifemale, $DebrisDamageType] = 1.0;
$DamageScale[ifemale, $LaserDamageType] = 1.0;
$DamageScale[ifemale, $MortarDamageType] = 1.0;
$DamageScale[ifemale, $BlasterDamageType] = 1.0;
$DamageScale[ifemale, $ElectricityDamageType] = 1.0;
$DamageScale[ifemale, $MineDamageType] = 1.0;
$DamageScale[ifemale, $ChargeDamageType] = 1.0;
$DamageScale[ifemale, $AirstrikeDamageType] = 1.0;
$DamageScale[ifemale, $BleedDamageType] = 1.0;
$DamageScale[ifemale, $GrenadeDamageType] = 1.0;
$DamageScale[ifemale, $PoisonDamageType] = 1.0;
$DamageScale[ifemale, $SmokeDamageType] = 1.0;
$DamageScale[ifemale, $StabDamageType] = 1.0;

$ItemMax[ifemale, Blaster] = 1;
$ItemMax[ifemale, Chaingun] = 1;
$ItemMax[ifemale, Disclauncher] = 1;
$ItemMax[ifemale, GrenadeLauncher] = 1;
$ItemMax[ifemale, Mortar] = 0;
$ItemMax[ifemale, PlasmaGun] = 1;
$ItemMax[ifemale, LaserRifle] = 0;
$ItemMax[ifemale, EnergyRifle] = 1;
$ItemMax[ifemale, TargetingLaser] = 1;
$ItemMax[ifemale, BouncingMinePack] = 0;
$ItemMax[ifemale, APMinePack] = 1;
$ItemMax[ifemale, AAMinePack] = 0;
$ItemMax[ifemale, Grenade] = 3;
$ItemMax[ifemale, Beacon] = 3;
$ItemMax[ifemale, Knife] = 1;
$ItemMax[ifemale, SOCOM] = 1;
$ItemMax[ifemale, OICW] = 1;
$ItemMax[ifemale, SAW] = 1;
$ItemMax[ifemale, MP5] = 0;
$ItemMax[ifemale, PSG1] = 0;
$ItemMax[ifemale, FiftyCal] = 0;
$ItemMax[ifemale, LAW] = 1;
$ItemMax[ifemale, AutoShotgun] = 1;
$ItemMax[ifemale, Howitzer] = 0;
$ItemMax[ifemale, Flamethrower] = 1;
$ItemMax[ifemale, Airstrike] = 0;
$ItemMax[ifemale, Stinger] = 0;

$ItemMax[ifemale, BulletAmmo] = 150;
$ItemMax[ifemale, PlasmaAmmo] = 40;
$ItemMax[ifemale, DiscAmmo] = 15;
$ItemMax[ifemale, GrenadeAmmo] = 10;
$ItemMax[ifemale, MortarAmmo] = 10;
$ItemMax[ifemale, SOCOMAmmo] = 15;
$ItemMax[ifemale, OICWAmmo] = 30;
$ItemMax[ifemale, SAWAmmo] = 200;
$ItemMax[ifemale, MP5Ammo] = 0;
$ItemMax[ifemale, PSG1Ammo] = 0;
$ItemMax[ifemale, FiftyCalAmmo] = 0;
$ItemMax[ifemale, LAWAmmo] = 1;
$ItemMax[ifemale, AutoShotgunAmmo] = 7;
$ItemMax[ifemale, HowitzerAmmo] = 0;
$ItemMax[ifemale, StingerAmmo] = 0;
$ItemMax[ifemale, FlameAmmo] = 150;

$ItemMax[ifemale, SOCOMClip] = 1;
$ItemMax[ifemale, OICWClip] = 2;
$ItemMax[ifemale, SAWClip] = 1;
$ItemMax[ifemale, MP5Clip] = 0;
$ItemMax[ifemale, PSG1Clip] = 0;
$ItemMax[ifemale, FiftyCalClip] = 0;
$ItemMax[ifemale, AutoShotgunClip] = 5;
$ItemMax[ifemale, StingerClip] = 0;

$ItemMax[ifemale, EnergyPack] = 0;
$ItemMax[ifemale, RepairPack] = 1;
$ItemMax[ifemale, ShieldPack] = 0;
$ItemMax[ifemale, SensorJammerPack] = 0;
$ItemMax[ifemale, MotionSensorPack] = 0;
$ItemMax[ifemale, PulseSensorPack] = 0;
$ItemMax[ifemale, DeployableSensorJammerPack] = 0;
$ItemMax[ifemale, DeployableHealthPack] = 0;
$ItemMax[ifemale, CameraPack] = 1;
$ItemMax[ifemale, TurretPack] = 0;
$ItemMax[ifemale, AmmoPackSmall] = 1;
$ItemMax[ifemale, AmmoPackHeavy] = 1;
$ItemMax[ifemale, AmmoPackExp] = 1;
$ItemMax[ifemale, RepairKit] = 1;
$ItemMax[ifemale, DeployableInvPack] = 0;
$ItemMax[ifemale, DeployableAmmoPack] = 0;
$ItemMax[ifemale, TwentyPack] = 0;
$ItemMax[ifemale, HowitzerPack] = 0;
$ItemMax[ifemale, SAMPack] = 0;
$ItemMax[ifemale, Charge] = 0;
$ItemMax[ifemale, AirstrikePack] = 0;
$ItemMax[ifemale, GrapplePack] = 0;
$ItemMax[ifemale, GrappleHook] = 0;
$ItemMax[ifemale, ReloaderPack] = 1;
$ItemMax[ifemale, Parachute] = 1;
$ItemMax[ifemale, FuelPack] = 1;
$ItemMax[ifemale, PortGenPack] = 1;
$ItemMax[ifemale, AAPack] = 0;
$ItemMax[ifemale, MedicPack] = 0;

$MaxWeapons[ifemale] = 2;

// Grenadier Female

$DamageScale[gfemale, $LandingDamageType] = 1.0;
$DamageScale[gfemale, $ImpactDamageType] = 1.0;
$DamageScale[gfemale, $CrushDamageType] = 1.0;
$DamageScale[gfemale, $BulletDamageType] = 1.0;
$DamageScale[gfemale, $PlasmaDamageType] = 0.6;
$DamageScale[gfemale, $EnergyDamageType] = 1.0;
$DamageScale[gfemale, $ExplosionDamageType] = 1.0;
$DamageScale[gfemale, $MissileDamageType] = 1.0;
$DamageScale[gfemale, $ShrapnelDamageType] = 0.8;
$DamageScale[gfemale, $DebrisDamageType] = 1.0;
$DamageScale[gfemale, $LaserDamageType] = 1.0;
$DamageScale[gfemale, $MortarDamageType] = 1.0;
$DamageScale[gfemale, $BlasterDamageType] = 1.0;
$DamageScale[gfemale, $ElectricityDamageType] = 1.0;
$DamageScale[gfemale, $MineDamageType] = 1.0;
$DamageScale[gfemale, $ChargeDamageType] = 1.0;
$DamageScale[gfemale, $AirstrikeDamageType] = 1.0;
$DamageScale[gfemale, $BleedDamageType] = 1.0;
$DamageScale[gfemale, $GrenadeDamageType] = 1.0;
$DamageScale[gfemale, $PoisonDamageType] = 1.0;
$DamageScale[gfemale, $SmokeDamageType] = 1.0;
$DamageScale[gfemale, $StabDamageType] = 1.0;

$ItemMax[gfemale, Blaster] = 1;
$ItemMax[gfemale, Chaingun] = 1;
$ItemMax[gfemale, Disclauncher] = 1;
$ItemMax[gfemale, GrenadeLauncher] = 1;
$ItemMax[gfemale, Mortar] = 0;
$ItemMax[gfemale, PlasmaGun] = 1;
$ItemMax[gfemale, LaserRifle] = 1;
$ItemMax[gfemale, EnergyRifle] = 1;
$ItemMax[gfemale, TargetingLaser] = 1;
$ItemMax[gfemale, BouncingMinePack] = 1;
$ItemMax[gfemale, APMinePack] = 1;
$ItemMax[gfemale, AAMinePack] = 0;
$ItemMax[gfemale, Grenade] = 10;
$ItemMax[gfemale, Beacon]  = 3;
$ItemMax[gfemale, Knife] = 1;
$ItemMax[gfemale, SOCOM] = 1;
$ItemMax[gfemale, OICW] = 1;
$ItemMax[gfemale, SAW] = 0;
$ItemMax[gfemale, MP5] = 0;
$ItemMax[gfemale, PSG1] = 0;
$ItemMax[gfemale, FiftyCal] = 0;
$ItemMax[gfemale, LAW] = 0;
$ItemMax[gfemale, AutoShotgun] = 0;
$ItemMax[gfemale, Stinger] = 0;
$ItemMax[gfemale, Flamethrower] = 0;
$ItemMax[gfemale, Howitzer] = 0;
$ItemMax[gfemale, Airstrike] = 0;

$ItemMax[gfemale, BulletAmmo] = 100;
$ItemMax[gfemale, PlasmaAmmo] = 30;
$ItemMax[gfemale, DiscAmmo] = 15;
$ItemMax[gfemale, GrenadeAmmo] = 10;
$ItemMax[gfemale, MortarAmmo] = 10;
$ItemMax[gfemale, SOCOMAmmo] = 15;
$ItemMax[gfemale, OICWAmmo] = 30;
$ItemMax[gfemale, SAWAmmo] = 0;
$ItemMax[gfemale, MP5Ammo] = 0;
$ItemMax[gfemale, PSG1Ammo] = 0;
$ItemMax[gfemale, FiftyCalAmmo] = 0;
$ItemMax[gfemale, LAWAmmo] = 0;
$ItemMax[gfemale, AutoShotgunAmmo] = 0;
$ItemMax[gfemale, FlameAmmo] = 0;
$ItemMax[gfemale, StingerAmmo] = 0;
$ItemMax[gfemale, HowitzerAmmo] = 0;

$ItemMax[gfemale, SOCOMClip] = 1;
$ItemMax[gfemale, OICWClip] = 1;
$ItemMax[gfemale, SAWClip] = 0;
$ItemMax[gfemale, MP5Clip] = 0;
$ItemMax[gfemale, PSG1Clip] = 0;
$ItemMax[gfemale, FiftyCalClip] = 0;
$ItemMax[gfemale, AutoShotgunClip] = 0;
$ItemMax[gfemale, StingerClip] = 0;

$ItemMax[gfemale, EnergyPack] = 0;
$ItemMax[gfemale, RepairPack] = 1;
$ItemMax[gfemale, ShieldPack] = 0;
$ItemMax[gfemale, SensorJammerPack] = 0;
$ItemMax[gfemale, MotionSensorPack] = 0;
$ItemMax[gfemale, PulseSensorPack] = 0;
$ItemMax[gfemale, DeployableSensorJammerPack] = 0;
$ItemMax[gfemale, DeployableHealthPack] = 0;
$ItemMax[gfemale, CameraPack] = 0;
$ItemMax[gfemale, TurretPack] = 0;
$ItemMax[gfemale, AmmoPackSmall] = 1;
$ItemMax[gfemale, AmmoPackHeavy] = 1;
$ItemMax[gfemale, AmmoPackExp] = 1;
$ItemMax[gfemale, RepairKit] = 1;
$ItemMax[gfemale, DeployableInvPack] = 0;
$ItemMax[gfemale, DeployableAmmoPack] = 0;
$ItemMax[gfemale, TwentyPack] = 0;
$ItemMax[gfemale, HowitzerPack] = 0;
$ItemMax[gfemale, SAMPack] = 0;
$ItemMax[gfemale, Charge] = 1;
$ItemMax[gfemale, AirstrikePack] = 0;
$ItemMax[gfemale, GrapplePack] = 0;
$ItemMax[gfemale, GrappleHook] = 0;
$ItemMax[gfemale, ReloaderPack] = 0;
$ItemMax[gfemale, Parachute] = 1;
$ItemMax[gfemale, FuelPack] = 0;
$ItemMax[gfemale, PortGenPack] = 0;
$ItemMax[gfemale, AAPack] = 0;
$ItemMax[gfemale, MedicPack] = 0;

$MaxWeapons[gfemale] = 1;

// SPECOPS FEMALE

$DamageScale[cfemale, $LandingDamageType] = 1.0;
$DamageScale[cfemale, $ImpactDamageType] = 1.0;	
$DamageScale[cfemale, $CrushDamageType] = 1.0;	
$DamageScale[cfemale, $BulletDamageType] = 1.2;
$DamageScale[cfemale, $PlasmaDamageType] = 1.0;
$DamageScale[cfemale, $EnergyDamageType] = 1.3;
$DamageScale[cfemale, $ExplosionDamageType] = 1.0;
$DamageScale[cfemale, $MissileDamageType] = 1.0;
$DamageScale[cfemale, $ShrapnelDamageType] = 1.2;
$DamageScale[cfemale, $DebrisDamageType] = 1.2;
$DamageScale[cfemale, $LaserDamageType] = 1.0;
$DamageScale[cfemale, $MortarDamageType] = 1.3;
$DamageScale[cfemale, $BlasterDamageType] = 1.3;
$DamageScale[cfemale, $ElectricityDamageType] = 1.0;
$DamageScale[cfemale, $MineDamageType] = 1.2;
$DamageScale[cfemale, $ChargeDamageType] = 1.2;
$DamageScale[cfemale, $AirstrikeDamageType] = 1.0;
$DamageScale[cfemale, $BleedDamageType] = 1.0;
$DamageScale[cfemale, $GrenadeDamageType] = 1.0;
$DamageScale[cfemale, $PoisonDamageType] = 1.0;
$DamageScale[cfemale, $SmokeDamageType] = 1.0;
$DamageScale[cfemale, $StabDamageType] = 1.0;

$ItemMax[cfemale, Blaster] = 1;
$ItemMax[cfemale, Chaingun] = 1;
$ItemMax[cfemale, Disclauncher] = 1;
$ItemMax[cfemale, GrenadeLauncher] = 1;
$ItemMax[cfemale, Mortar] = 0;
$ItemMax[cfemale, PlasmaGun] = 1;
$ItemMax[cfemale, LaserRifle] = 1;
$ItemMax[cfemale, EnergyRifle] = 1;
$ItemMax[cfemale, TargetingLaser] = 1;
$ItemMax[cfemale, BouncingMinePack] = 0;
$ItemMax[cfemale, APMinePack] = 0;
$ItemMax[cfemale, AAMinePack] = 0;
$ItemMax[cfemale, Grenade] = 3;
$ItemMax[cfemale, Beacon] = 3;
$ItemMax[cfemale, SOCOM] = 1;
$ItemMax[cfemale, OICW] = 0;
$ItemMax[cfemale, SAW] = 0;
$ItemMax[cfemale, MP5] = 1;
$ItemMax[cfemale, PSG1] = 0;
$ItemMax[cfemale, FiftyCal] = 0;
$ItemMax[cfemale, LAW] = 0;
$ItemMax[cfemale, AutoShotgun] = 0;
$ItemMax[cfemale, Howitzer] = 0;
$ItemMax[cfemale, Airstrike] = 1;
$ItemMax[cfemale, Stinger] = 1;
$ItemMax[cfemale, Flamethrower] = 0;

$ItemMax[cfemale, BulletAmmo] = 100;
$ItemMax[cfemale, PlasmaAmmo] = 30;
$ItemMax[cfemale, DiscAmmo] = 15;
$ItemMax[cfemale, GrenadeAmmo] = 10;
$ItemMax[cfemale, MortarAmmo] = 10;
$ItemMax[cfemale, Knife] = 1;
$ItemMax[cfemale, SOCOMAmmo] = 15;
$ItemMax[cfemale, OICWAmmo] = 0;
$ItemMax[cfemale, SAWAmmo] = 0;
$ItemMax[cfemale, MP5Ammo] = 30;
$ItemMax[cfemale, PSG1Ammo] = 0;
$ItemMax[cfemale, FiftyCalAmmo] = 0;
$ItemMax[cfemale, LAWAmmo] = 0;
$ItemMax[cfemale, AutoShotgunAmmo] = 0;
$ItemMax[cfemale, HowitzerAmmo] = 0;
$ItemMax[cfemale, StingerAmmo] = 1;
$ItemMax[cfemale, FlameAmmo] = 0;

$ItemMax[cfemale, SOCOMClip] = 1;
$ItemMax[cfemale, OICWClip] = 0;
$ItemMax[cfemale, SAWClip] = 0;
$ItemMax[cfemale, MP5Clip] = 2;
$ItemMax[cfemale, PSG1Clip] = 0;
$ItemMax[cfemale, FiftyCalClip] = 0;
$ItemMax[cfemale, AutoShotgunClip] = 0;
$ItemMax[cfemale, StingerClip] = 1;

$ItemMax[cfemale, EnergyPack] = 0;
$ItemMax[cfemale, RepairPack] = 0;
$ItemMax[cfemale, ShieldPack] = 0;
$ItemMax[cfemale, SensorJammerPack] = 1;
$ItemMax[cfemale, MotionSensorPack] = 0;
$ItemMax[cfemale, PulseSensorPack] = 0;
$ItemMax[cfemale, DeployableSensorJammerPack] = 1;
$ItemMax[cfemale, DeployableHealthPack] = 0;
$ItemMax[cfemale, CameraPack] = 1;
$ItemMax[cfemale, TurretPack] = 0;
$ItemMax[cfemale, AmmoPackSmall] = 1;
$ItemMax[cfemale, AmmoPackHeavy] = 0;
$ItemMax[cfemale, AmmoPackExp] = 1;
$ItemMax[cfemale, RepairKit] = 1;
$ItemMax[cfemale, DeployableInvPack] = 0;
$ItemMax[cfemale, DeployableAmmoPack] = 0;
$ItemMax[cfemale, TwentyPack] = 0;
$ItemMax[cfemale, HowitzerPack] = 0;
$ItemMax[cfemale, SAMPack] = 0;
$ItemMax[cfemale, Charge] = 1;
$ItemMax[cfemale, AirstrikePack] = 1;
$ItemMax[cfemale, GrapplePack] = 1;
$ItemMax[cfemale, GrappleHook] = 1;
$ItemMax[cfemale, ReloaderPack] = 1;
$ItemMax[cfemale, Parachute] = 1;
$ItemMax[cfemale, FuelPack] = 0;
$ItemMax[cfemale, PortGenPack] = 0;
$ItemMax[cfemale, AAPack] = 0;
$ItemMax[cfemale, MedicPack] = 0;

$MaxWeapons[cfemale] = 3;

// ENGINEER FEMALE

$DamageScale[efemale, $LandingDamageType] = 1.0;
$DamageScale[efemale, $ImpactDamageType] = 1.0;
$DamageScale[efemale, $CrushDamageType] = 1.0;
$DamageScale[efemale, $BulletDamageType] = 1.0;
$DamageScale[efemale, $EnergyDamageType] = 1.0;
$DamageScale[efemale, $PlasmaDamageType] = 0.6;
$DamageScale[efemale, $ExplosionDamageType] = 1.0;
$DamageScale[efemale, $MissileDamageType] = 1.0;
$DamageScale[efemale, $ShrapnelDamageType] = 1.0;
$DamageScale[efemale, $DebrisDamageType] = 1.0;
$DamageScale[efemale, $LaserDamageType] = 1.0;
$DamageScale[efemale, $MortarDamageType] = 1.0;
$DamageScale[efemale, $BlasterDamageType] = 1.0;
$DamageScale[efemale, $ElectricityDamageType] = 1.0;
$DamageScale[efemale, $MineDamageType] = 1.0;
$DamageScale[efemale, $ChargeDamageType] = 1.0;
$DamageScale[efemale, $AirstrikeDamageType] = 1.0;
$DamageScale[efemale, $BleedDamageType] = 1.0;
$DamageScale[efemale, $GrenadeDamageType] = 1.0;
$DamageScale[efemale, $PoisonDamageType] = 1.0;
$DamageScale[efemale, $SmokeDamageType] = 1.0;
$DamageScale[efemale, $StabDamageType] = 1.0;

$ItemMax[efemale, Blaster] = 1;
$ItemMax[efemale, Chaingun] = 1;
$ItemMax[efemale, Disclauncher] = 1;
$ItemMax[efemale, GrenadeLauncher] = 1;
$ItemMax[efemale, Mortar] = 0;
$ItemMax[efemale, PlasmaGun] = 1;
$ItemMax[efemale, LaserRifle] = 0;
$ItemMax[efemale, EnergyRifle] = 1;
$ItemMax[efemale, TargetingLaser] = 1;
$ItemMax[efemale, BouncingMinePack] = 1;
$ItemMax[efemale, APMinePack] = 1;
$ItemMax[efemale, AAMinePack] = 1;
$ItemMax[efemale, Grenade] = 2;
$ItemMax[efemale, Beacon] = 3;
$ItemMax[efemale, Knife] = 1;
$ItemMax[efemale, SOCOM] = 1;
$ItemMax[efemale, OICW] = 1;
$ItemMax[efemale, SAW] = 0;
$ItemMax[efemale, MP5] = 0;
$ItemMax[efemale, PSG1] = 0;
$ItemMax[efemale, FiftyCal] = 0;
$ItemMax[efemale, LAW] = 0;
$ItemMax[efemale, AutoShotgun] = 1;
$ItemMax[efemale, Howitzer] = 0;
$ItemMax[efemale, Airstrike] = 0;
$ItemMax[efemale, Stinger] = 0;
$ItemMax[efemale, Flamethrower] = 0;

$ItemMax[efemale, BulletAmmo] = 150;
$ItemMax[efemale, PlasmaAmmo] = 40;
$ItemMax[efemale, DiscAmmo] = 15;
$ItemMax[efemale, GrenadeAmmo] = 10;
$ItemMax[efemale, MortarAmmo] = 10;
$ItemMax[efemale, SOCOMAmmo] = 15;
$ItemMax[efemale, OICWAmmo] = 30;
$ItemMax[efemale, SAWAmmo] = 0;
$ItemMax[efemale, MP5Ammo] = 0;
$ItemMax[efemale, PSG1Ammo] = 0;
$ItemMax[efemale, FiftyCalAmmo] = 0;
$ItemMax[efemale, LAWAmmo] = 0;
$ItemMax[efemale, AutoShotgunAmmo] = 7;
$ItemMax[efemale, HowitzerAmmo] = 0;
$ItemMax[efemale, StingerAmmo] = 0;
$ItemMax[efemale, FlameAmmo] = 0;

$ItemMax[efemale, SOCOMClip] = 1;
$ItemMax[efemale, OICWClip] = 1;
$ItemMax[efemale, SAWClip] = 0;
$ItemMax[efemale, MP5Clip] = 0;
$ItemMax[efemale, PSG1Clip] = 0;
$ItemMax[efemale, FiftyCalClip] = 0;
$ItemMax[efemale, AutoShotgunClip] = 4;
$ItemMax[efemale, StingerClip] = 0;

$ItemMax[efemale, EnergyPack] = 0;
$ItemMax[efemale, RepairPack] = 1;
$ItemMax[efemale, ShieldPack] = 0;
$ItemMax[efemale, SensorJammerPack] = 1;
$ItemMax[efemale, MotionSensorPack] = 1;
$ItemMax[efemale, PulseSensorPack] = 1;
$ItemMax[efemale, DeployableSensorJammerPack] = 1;
$ItemMax[efemale, DeployableHealthPack] = 0;
$ItemMax[efemale, CameraPack] = 1;
$ItemMax[efemale, TurretPack] = 1;
$ItemMax[efemale, AmmoPackSmall] = 1;
$ItemMax[efemale, AmmoPackHeavy] = 1;
$ItemMax[efemale, AmmoPackExp] = 0;
$ItemMax[efemale, RepairKit] = 1;
$ItemMax[efemale, DeployableInvPack] = 1;
$ItemMax[efemale, DeployableAmmoPack] = 1;
$ItemMax[efemale, TwentyPack] = 1;
$ItemMax[efemale, HowitzerPack] = 1;
$ItemMax[efemale, SAMPack] = 1;
$ItemMax[efemale, Charge] = 0;
$ItemMax[efemale, AirstrikePack] = 0;
$ItemMax[efemale, GrapplePack] = 0;
$ItemMax[efemale, GrappleHook] = 0;
$ItemMax[efemale, ReloaderPack] = 1;
$ItemMax[efemale, Parachute] = 1;
$ItemMax[efemale, FuelPack] = 0;
$ItemMax[efemale, PortGenPack] = 1;
$ItemMax[efemale, AAPack] = 1;
$ItemMax[efemale, MedicPack] = 0;

$MaxWeapons[efemale] = 2;

// PILOT FEMALE

$DamageScale[lfemale, $LandingDamageType] = 1.0;
$DamageScale[lfemale, $ImpactDamageType] = 1.0;	
$DamageScale[lfemale, $CrushDamageType] = 1.0;	
$DamageScale[lfemale, $BulletDamageType] = 1.2;
$DamageScale[lfemale, $PlasmaDamageType] = 1.0;
$DamageScale[lfemale, $EnergyDamageType] = 1.3;
$DamageScale[lfemale, $ExplosionDamageType] = 1.0;
$DamageScale[lfemale, $MissileDamageType] = 1.0;
$DamageScale[lfemale, $ShrapnelDamageType] = 1.2;
$DamageScale[lfemale, $DebrisDamageType] = 1.2;
$DamageScale[lfemale, $LaserDamageType] = 1.0;
$DamageScale[lfemale, $MortarDamageType] = 1.3;
$DamageScale[lfemale, $BlasterDamageType] = 1.3;
$DamageScale[lfemale, $ElectricityDamageType] = 1.0;
$DamageScale[lfemale, $MineDamageType] = 1.2;
$DamageScale[lfemale, $ChargeDamageType] = 1.2;
$DamageScale[lfemale, $AirstrikeDamageType] = 1.0;
$DamageScale[lfemale, $BleedDamageType] = 1.0;
$DamageScale[lfemale, $GrenadeDamageType] = 1.0;
$DamageScale[lfemale, $PoisonDamageType] = 1.0;
$DamageScale[lfemale, $SmokeDamageType] = 1.0;
$DamageScale[lfemale, $StabDamageType] = 1.0;

$ItemMax[lfemale, Blaster] = 1;
$ItemMax[lfemale, Chaingun] = 1;
$ItemMax[lfemale, Disclauncher] = 1;
$ItemMax[lfemale, GrenadeLauncher] = 1;
$ItemMax[lfemale, Mortar] = 0;
$ItemMax[lfemale, PlasmaGun] = 1;
$ItemMax[lfemale, LaserRifle] = 1;
$ItemMax[lfemale, EnergyRifle] = 1;
$ItemMax[lfemale, TargetingLaser] = 1;
$ItemMax[lfemale, BouncingMinePack] = 0;
$ItemMax[lfemale, APMinePack] = 0;
$ItemMax[lfemale, AAMinePack] = 0;
$ItemMax[lfemale, Grenade] = 2;
$ItemMax[lfemale, Beacon] = 3;
$ItemMax[lfemale, Knife] = 1;
$ItemMax[lfemale, SOCOM] = 1;
$ItemMax[lfemale, OICW] = 0;
$ItemMax[lfemale, SAW] = 0;
$ItemMax[lfemale, MP5] = 0;
$ItemMax[lfemale, PSG1] = 0;
$ItemMax[lfemale, FiftyCal] = 0;
$ItemMax[lfemale, LAW] = 0;
$ItemMax[lfemale, AutoShotgun] = 0;
$ItemMax[lfemale, Howitzer] = 0;
$ItemMax[lfemale, Airstrike] = 0;
$ItemMax[lfemale, Stinger] = 0;
$ItemMax[lfemale, Flamethrower] = 0;

$ItemMax[lfemale, BulletAmmo] = 100;
$ItemMax[lfemale, PlasmaAmmo] = 30;
$ItemMax[lfemale, DiscAmmo] = 15;
$ItemMax[lfemale, GrenadeAmmo] = 10;
$ItemMax[lfemale, MortarAmmo] = 10;
$ItemMax[lfemale, SOCOMAmmo] = 15;
$ItemMax[lfemale, OICWAmmo] = 0;
$ItemMax[lfemale, SAWAmmo] = 0;
$ItemMax[lfemale, MP5Ammo] = 0;
$ItemMax[lfemale, PSG1Ammo] = 0;
$ItemMax[lfemale, FiftyCalAmmo] = 0;
$ItemMax[lfemale, LAWAmmo] = 0;
$ItemMax[lfemale, AutoShotgunAmmo] = 0;
$ItemMax[lfemale, HowitzerAmmo] = 0;
$ItemMax[lfemale, StingerAmmo] = 0;
$ItemMax[lfemale, FlameAmmo] = 0;

$ItemMax[lfemale, SOCOMClip] = 1;
$ItemMax[lfemale, OICWClip] = 0;
$ItemMax[lfemale, SAWClip] = 0;
$ItemMax[lfemale, MP5Clip] = 0;
$ItemMax[lfemale, PSG1Clip] = 0;
$ItemMax[lfemale, FiftyCalClip] = 0;
$ItemMax[lfemale, AutoShotgunClip] = 0;
$ItemMax[lfemale, StingerClip] = 0;

$ItemMax[lfemale, EnergyPack] = 0;
$ItemMax[lfemale, RepairPack] = 0;
$ItemMax[lfemale, ShieldPack] = 0;
$ItemMax[lfemale, SensorJammerPack] = 0;
$ItemMax[lfemale, MotionSensorPack] = 0;
$ItemMax[lfemale, PulseSensorPack] = 0;
$ItemMax[lfemale, DeployableSensorJammerPack] = 0;
$ItemMax[lfemale, DeployableHealthPack] = 0;
$ItemMax[lfemale, CameraPack] = 0;
$ItemMax[lfemale, TurretPack] = 0;
$ItemMax[lfemale, AmmoPackSmall] = 1;
$ItemMax[lfemale, AmmoPackHeavy] = 0;
$ItemMax[lfemale, AmmoPackExp] = 0;
$ItemMax[lfemale, RepairKit] = 1;
$ItemMax[lfemale, DeployableInvPack] = 0;
$ItemMax[lfemale, DeployableAmmoPack] = 0;
$ItemMax[lfemale, TwentyPack] = 0;
$ItemMax[lfemale, HowitzerPack] = 0;
$ItemMax[lfemale, SAMPack] = 0;
$ItemMax[lfemale, Charge] = 0;
$ItemMax[lfemale, AirstrikePack] = 0;
$ItemMax[lfemale, GrapplePack] = 0;
$ItemMax[lfemale, GrappleHook] = 0;
$ItemMax[lfemale, ReloaderPack] = 0;
$ItemMax[lfemale, Parachute] = 1;
$ItemMax[lfemale, FuelPack] = 0;
$ItemMax[lfemale, PortGenPack] = 0;
$ItemMax[lfemale, AAPack] = 0;
$ItemMax[lfemale, MedicPack] = 0;

$MaxWeapons[lfemale] = 1;

// ARTILLERY FEMALE

$DamageScale[afemale, $LandingDamageType] = 1.0;
$DamageScale[afemale, $ImpactDamageType] = 1.0;
$DamageScale[afemale, $CrushDamageType] = 1.0;
$DamageScale[afemale, $BulletDamageType] = 1.2;
$DamageScale[afemale, $PlasmaDamageType] = 1.0;
$DamageScale[afemale, $EnergyDamageType] = 1.3;
$DamageScale[afemale, $ExplosionDamageType] = 1.0;
$DamageScale[afemale, $MissileDamageType] = 1.0;
$DamageScale[afemale, $DebrisDamageType] = 1.2;
$DamageScale[afemale, $ShrapnelDamageType] = 1.2;
$DamageScale[afemale, $LaserDamageType] = 1.0;
$DamageScale[afemale, $MortarDamageType] = 1.3;
$DamageScale[afemale, $BlasterDamageType] = 1.3;
$DamageScale[afemale, $ElectricityDamageType] = 1.0;
$DamageScale[afemale, $MineDamageType] = 1.2;
$DamageScale[afemale, $ChargeDamageType] = 1.2;
$DamageScale[afemale, $AirstrikeDamageType] = 1.0;
$DamageScale[afemale, $BleedDamageType] = 1.0;
$DamageScale[afemale, $GrenadeDamageType] = 1.0;
$DamageScale[afemale, $PoisonDamageType] = 1.0;
$DamageScale[afemale, $SmokeDamageType] = 1.0;
$DamageScale[afemale, $StabDamageType] = 1.0;

//$DamageScale[afemale, $LandingDamageType] = 1.0;
//$DamageScale[afemale, $ImpactDamageType] = 1.0;
//$DamageScale[afemale, $CrushDamageType] = 1.0;
//$DamageScale[afemale, $BulletDamageType] = 0.6;
//$DamageScale[afemale, $PlasmaDamageType] = 0.4;
//$DamageScale[afemale, $EnergyDamageType] = 0.7;
//$DamageScale[afemale, $ExplosionDamageType] = 0.6;
//$DamageScale[afemale, $MissileDamageType] = 0.6;
//$DamageScale[afemale, $DebrisDamageType] = 0.8;
//$DamageScale[afemale, $ShrapnelDamageType] = 0.8;
//$DamageScale[afemale, $LaserDamageType] = 0.6;
//$DamageScale[afemale, $MortarDamageType] = 0.7;
//$DamageScale[afemale, $BlasterDamageType] = 0.7;
//$DamageScale[afemale, $ElectricityDamageType] = 1.0;
//$DamageScale[afemale, $MineDamageType] = 0.8;
//$DamageScale[afemale, $ChargeDamageType] = 0.8;
//$DamageScale[afemale, $AirstrikeDamageType] = 1.0;

$ItemMax[afemale, Blaster] = 0;
$ItemMax[afemale, Chaingun] = 0;
$ItemMax[afemale, Disclauncher] = 0;
$ItemMax[afemale, GrenadeLauncher] = 0;
$ItemMax[afemale, Mortar] = 0;
$ItemMax[afemale, PlasmaGun] = 0;
$ItemMax[afemale, LaserRifle] = 0;
$ItemMax[afemale, EnergyRifle] = 0;
$ItemMax[afemale, TargetingLaser] = 1;
$ItemMax[afemale, BouncingMinePack] = 1;
$ItemMax[afemale, APMinePack] = 1;
$ItemMax[afemale, AAMinePack] = 1;
$ItemMax[afemale, Grenade] = 3;
$ItemMax[afemale, Beacon]  = 3;
$ItemMax[afemale, Knife] = 1;
$ItemMax[afemale, SOCOM] = 1;
$ItemMax[afemale, OICW] = 1;
$ItemMax[afemale, SAW] = 0;
$ItemMax[afemale, MP5] = 0;
$ItemMax[afemale, PSG1] = 0;
$ItemMax[afemale, FiftyCal] = 0;
$ItemMax[afemale, LAW] = 0;
$ItemMax[afemale, AutoShotgun] = 0;
$ItemMax[afemale, Howitzer] = 0;
$ItemMax[afemale, Airstrike] = 1;
$ItemMax[afemale, Stinger] = 0;
$ItemMax[afemale, Flamethrower] = 0;

$ItemMax[afemale, BulletAmmo] = 0;
$ItemMax[afemale, PlasmaAmmo] = 0;
$ItemMax[afemale, DiscAmmo] = 0;
$ItemMax[afemale, GrenadeAmmo] = 10;
$ItemMax[afemale, MortarAmmo] = 10;
$ItemMax[afemale, SOCOMAmmo] = 15;
$ItemMax[afemale, OICWAmmo] = 30;
$ItemMax[afemale, SAWAmmo] = 0;
$ItemMax[afemale, MP5Ammo] = 0;
$ItemMax[afemale, PSG1Ammo] = 0;
$ItemMax[afemale, FiftyCalAmmo] = 0;
$ItemMax[afemale, LAWAmmo] = 0;
$ItemMax[afemale, AutoShotgunAmmo] = 0;
$ItemMax[afemale, HowitzerAmmo] = 0;
$ItemMax[afemale, Stinger] = 0;
$ItemMax[afemale, FlameAmmo] = 0;

$ItemMax[afemale, SOCOMClip] = 1;
$ItemMax[afemale, OICWClip] = 1;
$ItemMax[afemale, SAWClip] = 0;
$ItemMax[afemale, MP5Clip] = 0;
$ItemMax[afemale, PSG1Clip] = 0;
$ItemMax[afemale, FiftyCalClip] = 0;
$ItemMax[afemale, AutoShotgunClip] = 0;
$ItemMax[afemale, StingerClip] = 0;

$ItemMax[afemale, EnergyPack] = 0;
$ItemMax[afemale, RepairPack] = 0;
$ItemMax[afemale, ShieldPack] = 0;
$ItemMax[afemale, SensorJammerPack] = 0;
$ItemMax[afemale, MotionSensorPack] = 0;
$ItemMax[afemale, PulseSensorPack] = 0;
$ItemMax[afemale, DeployableSensorJammerPack] = 0;
$ItemMax[afemale, DeployableHealthPack] = 0;
$ItemMax[afemale, CameraPack] = 0;
$ItemMax[afemale, TurretPack] = 0;
$ItemMax[afemale, AmmoPack] = 1;
$ItemMax[afemale, RepairKit] = 1;
$ItemMax[afemale, DeployableInvPack] = 0;
$ItemMax[afemale, DeployableAmmoPack] = 0;
$ItemMax[afemale, TwentyPack] = 0;
$ItemMax[afemale, HowitzerPack] = 1;
$ItemMax[afemale, SAMPack] = 0;
$ItemMax[afemale, Charge] = 0;
$ItemMax[afemale, AirstrikePack] = 1;
$ItemMax[afemale, GrapplePack] = 0;
$ItemMax[afemale, GrappleHook] = 0;
$ItemMax[afemale, ReloaderPack] = 1;
$ItemMax[afemale, Parachute] = 1;
$ItemMax[afemale, FuelPack] = 0;
$ItemMax[afemale, PortGenPack] = 0;
$ItemMax[afemale, AAPack] = 0;
$ItemMax[afemale, MedicPack] = 0;

$MaxWeapons[afemale] = 2;


// MEDIC

PlayerData marmor
{
   className = "Armor";
   shapeFile = "larmor";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
   canCrouch = true;

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0.0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 11;
   maxBackwardSpeed = 10;
   maxSideSpeed = 10;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 75;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, direction
	// firstPerson, chaseCam, thirdPerson, signalThread
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
   animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 


    // celebration animations:
   animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
    // taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
    // poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;
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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// SNIPER

PlayerData sarmor
{
   className = "Armor";
   shapeFile = "larmor";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
   canCrouch = true;

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0.0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 11;
   maxBackwardSpeed = 10;
   maxSideSpeed = 10;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 75;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, direction
	// firstPerson, chaseCam, thirdPerson, signalThread
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
   animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 


    // celebration animations:
   animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
    // taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
    // poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;
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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// INFANTRY

PlayerData iarmor
{
   className = "Armor";
   shapeFile = "marmor";
   flameShapeName = "mflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   canCrouch = false;
   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 1.0;
   maxForwardSpeed = 8.0;
   maxBackwardSpeed = 7.0;
   maxSideSpeed = 7.0;
   groundForce = 35 * 13.0;
   mass = 13.0;
   groundtraction = 3.0;
	
	maxEnergy = 80;
   drag = 1.0;
   density = 1.5;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 110;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;

   rFootSounds = 
   {
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSnow,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft
  }; 
   lFootSounds =
   {
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSnow,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft
   };

   footPrints = { 2, 3 };

   boxWidth = 0.4;
   boxDepth = 0.4;
   boxNormalHeight = 2.25;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.20;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// Grenadier

PlayerData garmor
{
   className = "Armor";
   shapeFile = "marmor";
   flameShapeName = "mflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   canCrouch = false;
   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 1.0;
   maxForwardSpeed = 8.0;
   maxBackwardSpeed = 7.0;
   maxSideSpeed = 7.0;
   groundForce = 35 * 13.0;
   mass = 13.0;
   groundtraction = 3.0;
	
	maxEnergy = 80;
   drag = 1.0;
   density = 1.5;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 110;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;

   rFootSounds = 
   {
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSnow,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft
  }; 
   lFootSounds =
   {
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSnow,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft
   };

   footPrints = { 2, 3 };

   boxWidth = 0.4;
   boxDepth = 0.4;
   boxNormalHeight = 2.25;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.20;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// SPECOPS

PlayerData carmor
{
   className = "Armor";
   shapeFile = "larmor";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   shadowDetailMask = 1;

   visibleToSensor = False;
	mapFilter = 1;
	mapIcon = "M_player";
   canCrouch = true;

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 11;
   maxBackwardSpeed = 10;
   maxSideSpeed = 10;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 75;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, direction
	// firstPerson, chaseCam, thirdPerson, signalThread
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 


    // celebration animations:
   animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
    // taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
    // poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;
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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// ENGINEER

PlayerData earmor
{
   className = "Armor";
   shapeFile = "marmor";
   flameShapeName = "mflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   canCrouch = false;
   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 1.0;
   maxForwardSpeed = 8.0;
   maxBackwardSpeed = 7.0;
   maxSideSpeed = 7.0;
   groundForce = 35 * 13.0;
   mass = 13.0;
   groundtraction = 3.0;
	
	maxEnergy = 80;
   drag = 1.0;
   density = 1.5;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 110;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;

   rFootSounds = 
   {
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSnow,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft
  }; 
   lFootSounds =
   {
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSnow,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft
   };

   footPrints = { 2, 3 };

   boxWidth = 0.4;
   boxDepth = 0.4;
   boxNormalHeight = 2.25;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.20;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// PILOT

PlayerData larmor
{
   className = "Armor";
   shapeFile = "larmor";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
   canCrouch = true;

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 11;
   maxBackwardSpeed = 10;
   maxSideSpeed = 10;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 75;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, direction
	// firstPerson, chaseCam, thirdPerson, signalThread
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
   animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 


    // celebration animations:
   animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
    // taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
    // poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;
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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// ARTILLERY

PlayerData aarmor
{
   className = "Armor";
   shapeFile = "larmor";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
   canCrouch = false;

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0.0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 11;
   maxBackwardSpeed = 10;
   maxSideSpeed = 10;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 75;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, direction
	// firstPerson, chaseCam, thirdPerson, signalThread
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
   animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 


    // celebration animations:
   animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
    // taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
    // poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;
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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.20;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// MEDIC FEMALE

PlayerData mfemale
{
   className = "Armor";
   shapeFile = "lfemale";
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   canCrouch = true;
   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 11;
   maxBackwardSpeed = 10;
   maxSideSpeed = 10;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 75;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };


   //jetSound = SoundJetLight;

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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// SNIPER FEMALE

PlayerData sfemale
{
   className = "Armor";
   shapeFile = "lfemale";
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   canCrouch = true;
   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 11;
   maxBackwardSpeed = 10;
   maxSideSpeed = 10;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 75;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };


   //jetSound = SoundJetLight;

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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// INFANTRY FEMALE

PlayerData ifemale
{
   className = "Armor";
   shapeFile = "mfemale";
   flameShapeName = "mflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

   cancrouch = false;
	maxDamage = 1.0;
   maxForwardSpeed = 8.0;
   maxBackwardSpeed = 7.0;
   maxSideSpeed = 7.0;
   groundForce = 35 * 13.0;
   mass = 13.0;
   groundtraction = 3.0;
	maxEnergy = 80;
   mass = 13.0;
   drag = 1.0;
   density = 1.5;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 110;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   animData[0]  = { "root", none, 1, true, true, true, false, 0 };
   animData[1]  = { "run", none, 1, true, false, true, false, 3 };
   animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
   animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
   animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
   animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
   animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
   animData[7] = { "crouch root", none, 1, true, false, true, false, 3 };
   animData[8] = { "crouch root", none, 1, true, false, true, false, 3 };
   animData[9] = { "crouch root", none, -1, true, false, true, false, 3 };
   animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
   animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
   animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
   animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
   animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
   animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
   animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
   animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
   animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;

   rFootSounds = 
   {
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSnow,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft
  }; 
   lFootSounds =
   {
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSnow,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft
   };

   footPrints = { 2, 3 };

   boxWidth = 0.4;
   boxDepth = 0.4;
   boxNormalHeight = 2.25;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.20;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// Grenadier FEMALE

PlayerData gfemale
{
   className = "Armor";
   shapeFile = "mfemale";
   flameShapeName = "mflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

   cancrouch = false;
	maxDamage = 1.0;
   maxForwardSpeed = 8.0;
   maxBackwardSpeed = 7.0;
   maxSideSpeed = 7.0;
   groundForce = 35 * 13.0;
   mass = 13.0;
   groundtraction = 3.0;
	maxEnergy = 80;
   mass = 13.0;
   drag = 1.0;
   density = 1.5;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 110;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   animData[0]  = { "root", none, 1, true, true, true, false, 0 };
   animData[1]  = { "run", none, 1, true, false, true, false, 3 };
   animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
   animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
   animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
   animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
   animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
   animData[7] = { "crouch root", none, 1, true, false, true, false, 3 };
   animData[8] = { "crouch root", none, 1, true, false, true, false, 3 };
   animData[9] = { "crouch root", none, -1, true, false, true, false, 3 };
   animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
   animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
   animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
   animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
   animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
   animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
   animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
   animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
   animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;

   rFootSounds = 
   {
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSnow,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft
  }; 
   lFootSounds =
   {
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSnow,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft
   };

   footPrints = { 2, 3 };

   boxWidth = 0.4;
   boxDepth = 0.4;
   boxNormalHeight = 2.25;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// SPECOPS FEMALE

PlayerData cfemale
{
   className = "Armor";
   shapeFile = "lfemale";
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = False;
	mapFilter = 1;
	mapIcon = "M_player";

   canCrouch = true;
   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 11;
   maxBackwardSpeed = 10;
   maxSideSpeed = 10;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 75;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };


   //jetSound = SoundJetLight;

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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// ENGINEER FEMALE

PlayerData efemale
{
   className = "Armor";
   shapeFile = "mfemale";
   flameShapeName = "mflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

   cancrouch = false;
	maxDamage = 1.0;
   maxForwardSpeed = 8.0;
   maxBackwardSpeed = 7.0;
   maxSideSpeed = 7.0;
   groundForce = 35 * 13.0;
   mass = 13.0;
   groundtraction = 3.0;
	maxEnergy = 80;
   mass = 13.0;
   drag = 1.0;
   density = 1.5;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 110;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   animData[0]  = { "root", none, 1, true, true, true, false, 0 };
   animData[1]  = { "run", none, 1, true, false, true, false, 3 };
   animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
   animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
   animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
   animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
   animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
   animData[7] = { "crouch root", none, 1, true, false, true, false, 3 };
   animData[8] = { "crouch root", none, 1, true, false, true, false, 3 };
   animData[9] = { "crouch root", none, -1, true, false, true, false, 3 };
   animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
   animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
   animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
   animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
   animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
   animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
   animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
   animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
   animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;

   rFootSounds = 
   {
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSnow,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft
  }; 
   lFootSounds =
   {
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSnow,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft
   };

   footPrints = { 2, 3 };

   boxWidth = 0.4;
   boxDepth = 0.4;
   boxNormalHeight = 2.25;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.20;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// PILOT FEMALE

PlayerData lfemale
{
   className = "Armor";
   shapeFile = "lfemale";
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   canCrouch = true;
   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 11;
   maxBackwardSpeed = 10;
   maxSideSpeed = 10;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 75;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };


   //jetSound = SoundJetLight;

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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// ARTILLERY FEMALE

PlayerData afemale
{
   className = "Armor";
   shapeFile = "lfemale";
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   cancrouch = false;
   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 11;
   maxBackwardSpeed = 10;
   maxSideSpeed = 10;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 75;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };


   //jetSound = SoundJetLight;

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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//WOUNDED ARMORS
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

// MEDIC

$DamageScale[marmor2, $LandingDamageType] = 1.0;
$DamageScale[marmor2, $ImpactDamageType] = 1.0;
$DamageScale[marmor2, $CrushDamageType] = 1.0;
$DamageScale[marmor2, $BulletDamageType] = 1.2;
$DamageScale[marmor2, $PlasmaDamageType] = 1.0;
$DamageScale[marmor2, $EnergyDamageType] = 1.3;
$DamageScale[marmor2, $ExplosionDamageType] = 1.0;
$DamageScale[marmor2, $MissileDamageType] = 1.0;
$DamageScale[marmor2, $DebrisDamageType] = 1.2;
$DamageScale[marmor2, $ShrapnelDamageType] = 1.2;
$DamageScale[marmor2, $LaserDamageType] = 1.0;
$DamageScale[marmor2, $MortarDamageType] = 1.3;
$DamageScale[marmor2, $BlasterDamageType] = 1.3;
$DamageScale[marmor2, $ElectricityDamageType] = 1.0;
$DamageScale[marmor2, $MineDamageType] = 1.2;
$DamageScale[marmor2, $ChargeDamageType] = 1.2;
$DamageScale[marmor2, $AirstrikeDamageType] = 1.0;
$DamageScale[marmor2, $BleedDamageType] = 1.0;
$DamageScale[marmor2, $GrenadeDamageType] = 1.0;
$DamageScale[marmor2, $PoisonDamageType] = 1.0;
$DamageScale[marmor2, $SmokeDamageType] = 1.0;
$DamageScale[marmor2, $StabDamageType] = 1.0;

$ItemMax[marmor2, Blaster] = 1;
$ItemMax[marmor2, Chaingun] = 1;
$ItemMax[marmor2, Disclauncher] = 1;
$ItemMax[marmor2, GrenadeLauncher] = 1;
$ItemMax[marmor2, Mortar] = 0;
$ItemMax[marmor2, PlasmaGun] = 1;
$ItemMax[marmor2, LaserRifle] = 1;
$ItemMax[marmor2, EnergyRifle] = 1;
$ItemMax[marmor2, TargetingLaser] = 1;
$ItemMax[marmor2, BouncingMinePack] = 0;
$ItemMax[marmor2, APMinePack] = 0;
$ItemMax[marmor2, AAMinePack] = 0;
$ItemMax[marmor2, Grenade] = 2;
$ItemMax[marmor2, Beacon]  = 3;
$ItemMax[marmor2, Knife] = 1;
$ItemMax[marmor2, SOCOM] = 1;
$ItemMax[marmor2, OICW] = 0;
$ItemMax[marmor2, SAW] = 0;
$ItemMax[marmor2, MP5] = 1;
$ItemMax[marmor2, PSG1] = 0;
$ItemMax[marmor2, FiftyCal] = 0;
$ItemMax[marmor2, LAW] = 0;
$ItemMax[marmor2, AutoShotgun] = 0;
$ItemMax[marmor2, Howitzer] = 0;
$ItemMax[marmor2, Airstrike] = 0;
$ItemMax[marmor2, GrappleHook] = 0;
$ItemMax[marmor2, Stinger] = 0;
$ItemMax[marmor2, Flamethrower] = 0;

$ItemMax[marmor2, BulletAmmo] = 100;
$ItemMax[marmor2, PlasmaAmmo] = 30;
$ItemMax[marmor2, DiscAmmo] = 15;
$ItemMax[marmor2, GrenadeAmmo] = 10;
$ItemMax[marmor2, MortarAmmo] = 10;
$ItemMax[marmor2, SOCOMAmmo] = 15;
$ItemMax[marmor2, OICWAmmo] = 0;
$ItemMax[marmor2, SAWAmmo] = 0;
$ItemMax[marmor2, MP5Ammo] = 30;
$ItemMax[marmor2, PSG1Ammo] = 0;
$ItemMax[marmor2, FiftyCalAmmo] = 0;
$ItemMax[marmor2, LAWAmmo] = 0;
$ItemMax[marmor2, AutoShotgunAmmo] = 0;
$ItemMax[marmor2, HowitzerAmmo] = 0;
$ItemMax[marmor2, StingerAmmo] = 0;
$ItemMax[marmor2, FlameAmmo] = 0;

$ItemMax[marmor2, SOCOMClip] = 1;
$ItemMax[marmor2, OICWClip] = 0;
$ItemMax[marmor2, SAWClip] = 0;
$ItemMax[marmor2, MP5Clip] = 1;
$ItemMax[marmor2, PSG1Clip] = 0;
$ItemMax[marmor2, FiftyCalClip] = 0;
$ItemMax[marmor2, AutoShotgunClip] = 0;
$ItemMax[marmor2, StingerClip] = 0;

$ItemMax[marmor2, EnergyPack] = 0;
$ItemMax[marmor2, RepairPack] = 0;
$ItemMax[marmor2, ShieldPack] = 0;
$ItemMax[marmor2, SensorJammerPack] = 0;
$ItemMax[marmor2, MotionSensorPack] = 0;
$ItemMax[marmor2, PulseSensorPack] = 0;
$ItemMax[marmor2, DeployableSensorJammerPack] = 0;
$ItemMax[marmor2, DeployableHealthPack] = 1;
$ItemMax[marmor2, CameraPack] = 0;
$ItemMax[marmor2, TurretPack] = 0;
$ItemMax[marmor2, AmmoPackSmall] = 1;
$ItemMax[marmor2, AmmoPackHeavy] = 0;
$ItemMax[marmor2, AmmoPackExp] = 0;
$ItemMax[marmor2, RepairKit] = 1;
$ItemMax[marmor2, DeployableInvPack] = 0;
$ItemMax[marmor2, DeployableAmmoPack] = 0;
$ItemMax[marmor2, TwentyPack] = 0;
$ItemMax[marmor2, HowitzerPack] = 0;
$ItemMax[marmor2, SAMPack] = 0;
$ItemMax[marmor2, Charge] = 0;
$ItemMax[marmor2, AirstrikePack] = 0;
$ItemMax[marmor2, GrapplePack] = 0;
$ItemMax[marmor2, ReloaderPack] = 0;
$ItemMax[marmor2, Parachute] = 1;
$ItemMax[marmor2, FuelPack] = 0;
$ItemMax[marmor2, PortGenPack] = 0;
$ItemMax[marmor2, AAPack] = 0;
$ItemMax[marmor2, MedicPack] = 1;

$MaxWeapons[marmor2] = 2;

// SNIPER

$DamageScale[sarmor2, $LandingDamageType] = 1.0;
$DamageScale[sarmor2, $ImpactDamageType] = 1.0;
$DamageScale[sarmor2, $CrushDamageType] = 1.0;
$DamageScale[sarmor2, $BulletDamageType] = 1.2;
$DamageScale[sarmor2, $PlasmaDamageType] = 1.0;
$DamageScale[sarmor2, $EnergyDamageType] = 1.3;
$DamageScale[sarmor2, $ExplosionDamageType] = 1.0;
$DamageScale[sarmor2, $MissileDamageType] = 1.0;
$DamageScale[sarmor2, $DebrisDamageType] = 1.2;
$DamageScale[sarmor2, $ShrapnelDamageType] = 1.2;
$DamageScale[sarmor2, $LaserDamageType] = 1.0;
$DamageScale[sarmor2, $MortarDamageType] = 1.3;
$DamageScale[sarmor2, $BlasterDamageType] = 1.3;
$DamageScale[sarmor2, $ElectricityDamageType] = 1.0;
$DamageScale[sarmor2, $MineDamageType] = 1.2;
$DamageScale[sarmor2, $ChargeDamageType] = 1.2;
$DamageScale[sarmor2, $AirstrikeDamageType] = 1.0;
$DamageScale[sarmor2, $BleedDamageType] = 1.0;
$DamageScale[sarmor2, $GrenadeDamageType] = 1.0;
$DamageScale[sarmor2, $PoisonDamageType] = 1.0;
$DamageScale[sarmor2, $SmokeDamageType] = 1.0;
$DamageScale[sarmor2, $StabDamageType] = 1.0;

$ItemMax[sarmor2, Blaster] = 1;
$ItemMax[sarmor2, Chaingun] = 1;
$ItemMax[sarmor2, Disclauncher] = 1;
$ItemMax[sarmor2, GrenadeLauncher] = 1;
$ItemMax[sarmor2, Mortar] = 0;
$ItemMax[sarmor2, PlasmaGun] = 1;
$ItemMax[sarmor2, LaserRifle] = 1;
$ItemMax[sarmor2, EnergyRifle] = 1;
$ItemMax[sarmor2, TargetingLaser] = 1;
$ItemMax[sarmor2, BouncingMinePack] = 0;
$ItemMax[sarmor2, APMinePack] = 0;
$ItemMax[sarmor2, AAMinePack] = 0;
$ItemMax[sarmor2, Grenade] = 2;
$ItemMax[sarmor2, Beacon]  = 3;
$ItemMax[sarmor2, Knife] = 1;
$ItemMax[sarmor2, SOCOM] = 1;
$ItemMax[sarmor2, OICW] = 0;
$ItemMax[sarmor2, SAW] = 0;
$ItemMax[sarmor2, MP5] = 0;
$ItemMax[sarmor2, PSG1] = 1;
$ItemMax[sarmor2, FiftyCal] = 1;
$ItemMax[sarmor2, LAW] = 0;
$ItemMax[sarmor2, AutoShotgun] = 0;
$ItemMax[sarmor2, Howitzer] = 0;
$ItemMax[sarmor2, Airstrike] = 0;
$ItemMax[sarmor2, Stinger] = 0;
$ItemMax[sarmor2, Flamethrower] = 0;

$ItemMax[sarmor2, BulletAmmo] = 100;
$ItemMax[sarmor2, PlasmaAmmo] = 30;
$ItemMax[sarmor2, DiscAmmo] = 15;
$ItemMax[sarmor2, GrenadeAmmo] = 10;
$ItemMax[sarmor2, MortarAmmo] = 10;
$ItemMax[sarmor2, SOCOMAmmo] = 15;
$ItemMax[sarmor2, OICWAmmo] = 0;
$ItemMax[sarmor2, SAWAmmo] = 0;
$ItemMax[sarmor2, MP5Ammo] = 0;
$ItemMax[sarmor2, PSG1Ammo] = 5;
$ItemMax[sarmor2, FiftyCalAmmo] = 5;
$ItemMax[sarmor2, LAWAmmo] = 0;
$ItemMax[sarmor2, AutoShotgunAmmo] = 0;
$ItemMax[sarmor2, HowitzerAmmo] = 0;
$ItemMax[sarmor2, StingerAmmo] = 0;
$ItemMax[sarmor2, FlameAmmo] = 0;

$ItemMax[sarmor2, SOCOMClip] = 1;
$ItemMax[sarmor2, OICWClip] = 0;
$ItemMax[sarmor2, SAWClip] = 0;
$ItemMax[sarmor2, MP5Clip] = 0;
$ItemMax[sarmor2, PSG1Clip] = 3;
$ItemMax[sarmor2, FiftyCalClip] = 2;
$ItemMax[sarmor2, AutoShotgunClip] = 0;
$ItemMax[sarmor2, StingerClip] = 0;

$ItemMax[sarmor2, EnergyPack] = 0;
$ItemMax[sarmor2, RepairPack] = 0;
$ItemMax[sarmor2, ShieldPack] = 0;
$ItemMax[sarmor2, SensorJammerPack] = 1;
$ItemMax[sarmor2, MotionSensorPack] = 0;
$ItemMax[sarmor2, PulseSensorPack] = 0;
$ItemMax[sarmor2, DeployableSensorJammerPack] = 0;
$ItemMax[sarmor2, DeployableHealthPack] = 0;
$ItemMax[sarmor2, CameraPack] = 0;
$ItemMax[sarmor2, TurretPack] = 0;
$ItemMax[sarmor2, AmmoPackSmall] = 1;
$ItemMax[sarmor2, AmmoPackHeavy] = 1;
$ItemMax[sarmor2, AmmoPackExp] = 0;
$ItemMax[sarmor2, RepairKit] = 1;
$ItemMax[sarmor2, DeployableInvPack] = 0;
$ItemMax[sarmor2, DeployableAmmoPack] = 0;
$ItemMax[sarmor2, TwentyPack] = 0;
$ItemMax[sarmor2, HowitzerPack] = 0;
$ItemMax[sarmor2, SAMPack] = 0;
$ItemMax[sarmor2, Charge] = 0;
$ItemMax[sarmor2, AirstrikePack] = 0;
$ItemMax[sarmor2, GrapplePack] = 0;
$ItemMax[sarmor2, GrappleHook] = 0;
$ItemMax[sarmor2, ReloaderPack] = 0;
$ItemMax[sarmor2, Parachute] = 1;
$ItemMax[sarmor2, FuelPack] = 0;
$ItemMax[sarmor2, PortGenPack] = 0;
$ItemMax[sarmor2, AAPack] = 0;
$ItemMax[sarmor2, MedicPack] = 0;

$MaxWeapons[sarmor2] = 1;

// INFANTRY

$DamageScale[iarmor2, $LandingDamageType] = 1.0;
$DamageScale[iarmor2, $ImpactDamageType] = 1.0;
$DamageScale[iarmor2, $CrushDamageType] = 1.0;
$DamageScale[iarmor2, $BulletDamageType] = 1.0;
$DamageScale[iarmor2, $PlasmaDamageType] = 0.6;
$DamageScale[iarmor2, $EnergyDamageType] = 1.0;
$DamageScale[iarmor2, $ExplosionDamageType] = 1.0;
$DamageScale[iarmor2, $MissileDamageType] = 1.0;
$DamageScale[iarmor2, $ShrapnelDamageType] = 1.0;
$DamageScale[iarmor2, $DebrisDamageType] = 1.0;
$DamageScale[iarmor2, $LaserDamageType] = 1.0;
$DamageScale[iarmor2, $MortarDamageType] = 1.0;
$DamageScale[iarmor2, $BlasterDamageType] = 1.0;
$DamageScale[iarmor2, $ElectricityDamageType] = 1.0;
$DamageScale[iarmor2, $MineDamageType] = 1.0;
$DamageScale[iarmor2, $ChargeDamageType] = 1.0;
$DamageScale[iarmor2, $AirstrikeDamageType] = 1.0;
$DamageScale[iarmor2, $BleedDamageType] = 1.0;
$DamageScale[iarmor2, $GrenadeDamageType] = 1.0;
$DamageScale[iarmor2, $PoisonDamageType] = 1.0;
$DamageScale[iarmor2, $SmokeDamageType] = 1.0;
$DamageScale[iarmor2, $StabDamageType] = 1.0;

$ItemMax[iarmor2, Blaster] = 1;
$ItemMax[iarmor2, Chaingun] = 1;
$ItemMax[iarmor2, Disclauncher] = 1;
$ItemMax[iarmor2, GrenadeLauncher] = 1;
$ItemMax[iarmor2, Mortar] = 0;
$ItemMax[iarmor2, PlasmaGun] = 1;
$ItemMax[iarmor2, LaserRifle] = 1;
$ItemMax[iarmor2, EnergyRifle] = 1;
$ItemMax[iarmor2, TargetingLaser] = 1;
$ItemMax[iarmor2, BouncingMinePack] = 0;
$ItemMax[iarmor2, APMinePack] = 1;
$ItemMax[iarmor2, AAMinePack] = 0;
$ItemMax[iarmor2, Grenade] = 3;
$ItemMax[iarmor2, Beacon]  = 3;
$ItemMax[iarmor2, Knife] = 1;
$ItemMax[iarmor2, SOCOM] = 1;
$ItemMax[iarmor2, OICW] = 1;
$ItemMax[iarmor2, SAW] = 1;
$ItemMax[iarmor2, MP5] = 0;
$ItemMax[iarmor2, PSG1] = 0;
$ItemMax[iarmor2, FiftyCal] = 0;
$ItemMax[iarmor2, LAW] = 1;
$ItemMax[iarmor2, AutoShotgun] = 1;
$ItemMax[iarmor2, Stinger] = 0;
$ItemMax[iarmor2, Flamethrower] = 1;
$ItemMax[iarmor2, Howitzer] = 0;
$ItemMax[iarmor2, Airstrike] = 0;

$ItemMax[iarmor2, BulletAmmo] = 100;
$ItemMax[iarmor2, PlasmaAmmo] = 30;
$ItemMax[iarmor2, DiscAmmo] = 15;
$ItemMax[iarmor2, GrenadeAmmo] = 10;
$ItemMax[iarmor2, MortarAmmo] = 10;
$ItemMax[iarmor2, SOCOMAmmo] = 15;
$ItemMax[iarmor2, OICWAmmo] = 30;
$ItemMax[iarmor2, SAWAmmo] = 200;
$ItemMax[iarmor2, MP5Ammo] = 0;
$ItemMax[iarmor2, PSG1Ammo] = 0;
$ItemMax[iarmor2, FiftyCalAmmo] = 0;
$ItemMax[iarmor2, LAWAmmo] = 1;
$ItemMax[iarmor2, AutoShotgunAmmo] = 7;
$ItemMax[iarmor2, FlameAmmo] = 150;
$ItemMax[iarmor2, StingerAmmo] = 0;
$ItemMax[iarmor2, HowitzerAmmo] = 0;

$ItemMax[iarmor2, SOCOMClip] = 1;
$ItemMax[iarmor2, OICWClip] = 2;
$ItemMax[iarmor2, SAWClip] = 1;
$ItemMax[iarmor2, MP5Clip] = 0;
$ItemMax[iarmor2, PSG1Clip] = 0;
$ItemMax[iarmor2, FiftyCalClip] = 0;
$ItemMax[iarmor2, AutoShotgunClip] = 5;
$ItemMax[iarmor2, StingerClip] = 0;

$ItemMax[iarmor2, EnergyPack] = 0;
$ItemMax[iarmor2, RepairPack] = 1;
$ItemMax[iarmor2, ShieldPack] = 0;
$ItemMax[iarmor2, SensorJammerPack] = 0;
$ItemMax[iarmor2, MotionSensorPack] = 0;
$ItemMax[iarmor2, PulseSensorPack] = 0;
$ItemMax[iarmor2, DeployableSensorJammerPack] = 0;
$ItemMax[iarmor2, DeployableHealthPack] = 0;
$ItemMax[iarmor2, CameraPack] = 0;
$ItemMax[iarmor2, TurretPack] = 0;
$ItemMax[iarmor2, AmmoPackSmall] = 1;
$ItemMax[iarmor2, AmmoPackHeavy] = 1;
$ItemMax[iarmor2, AmmoPackExp] = 1;
$ItemMax[iarmor2, RepairKit] = 1;
$ItemMax[iarmor2, DeployableInvPack] = 0;
$ItemMax[iarmor2, DeployableAmmoPack] = 0;
$ItemMax[iarmor2, TwentyPack] = 0;
$ItemMax[iarmor2, HowitzerPack] = 0;
$ItemMax[iarmor2, SAMPack] = 0;
$ItemMax[iarmor2, Charge] = 0;
$ItemMax[iarmor2, AirstrikePack] = 0;
$ItemMax[iarmor2, GrapplePack] = 0;
$ItemMax[iarmor2, GrappleHook] = 0;
$ItemMax[iarmor2, ReloaderPack] = 1;
$ItemMax[iarmor2, Parachute] = 1;
$ItemMax[iarmor2, FuelPack] = 1;
$ItemMax[iarmor2, PortGenPack] = 1;
$ItemMax[iarmor2, AAPack] = 0;
$ItemMax[iarmor2, MedicPack] = 0;

$MaxWeapons[iarmor2] = 2;

// Grenadier

$DamageScale[garmor2, $LandingDamageType] = 1.0;
$DamageScale[garmor2, $ImpactDamageType] = 1.0;
$DamageScale[garmor2, $CrushDamageType] = 1.0;
$DamageScale[garmor2, $BulletDamageType] = 1.0;
$DamageScale[garmor2, $PlasmaDamageType] = 0.6;
$DamageScale[garmor2, $EnergyDamageType] = 1.0;
$DamageScale[garmor2, $ExplosionDamageType] = 1.0;
$DamageScale[garmor2, $MissileDamageType] = 1.0;
$DamageScale[garmor2, $ShrapnelDamageType] = 0.8;
$DamageScale[garmor2, $DebrisDamageType] = 1.0;
$DamageScale[garmor2, $LaserDamageType] = 1.0;
$DamageScale[garmor2, $MortarDamageType] = 1.0;
$DamageScale[garmor2, $BlasterDamageType] = 1.0;
$DamageScale[garmor2, $ElectricityDamageType] = 1.0;
$DamageScale[garmor2, $MineDamageType] = 1.0;
$DamageScale[garmor2, $ChargeDamageType] = 1.0;
$DamageScale[garmor2, $AirstrikeDamageType] = 1.0;
$DamageScale[garmor2, $BleedDamageType] = 1.0;
$DamageScale[garmor2, $GrenadeDamageType] = 0.8;
$DamageScale[garmor2, $PoisonDamageType] = 1.0;
$DamageScale[garmor2, $SmokeDamageType] = 1.0;
$DamageScale[garmor2, $StabDamageType] = 1.0;

$ItemMax[garmor2, Blaster] = 1;
$ItemMax[garmor2, Chaingun] = 1;
$ItemMax[garmor2, Disclauncher] = 1;
$ItemMax[garmor2, GrenadeLauncher] = 1;
$ItemMax[garmor2, Mortar] = 0;
$ItemMax[garmor2, PlasmaGun] = 1;
$ItemMax[garmor2, LaserRifle] = 1;
$ItemMax[garmor2, EnergyRifle] = 1;
$ItemMax[garmor2, TargetingLaser] = 1;
$ItemMax[garmor2, BouncingMinePack] = 1;
$ItemMax[garmor2, APMinePack] = 1;
$ItemMax[garmor2, AAMinePack] = 0;
$ItemMax[garmor2, Grenade] = 10;
$ItemMax[garmor2, Beacon]  = 3;
$ItemMax[garmor2, Knife] = 1;
$ItemMax[garmor2, SOCOM] = 1;
$ItemMax[garmor2, OICW] = 1;
$ItemMax[garmor2, SAW] = 0;
$ItemMax[garmor2, MP5] = 0;
$ItemMax[garmor2, PSG1] = 0;
$ItemMax[garmor2, FiftyCal] = 0;
$ItemMax[garmor2, LAW] = 0;
$ItemMax[garmor2, AutoShotgun] = 0;
$ItemMax[garmor2, Stinger] = 0;
$ItemMax[garmor2, Flamethrower] = 0;
$ItemMax[garmor2, Howitzer] = 0;
$ItemMax[garmor2, Airstrike] = 0;

$ItemMax[garmor2, BulletAmmo] = 100;
$ItemMax[garmor2, PlasmaAmmo] = 30;
$ItemMax[garmor2, DiscAmmo] = 15;
$ItemMax[garmor2, GrenadeAmmo] = 10;
$ItemMax[garmor2, MortarAmmo] = 10;
$ItemMax[garmor2, SOCOMAmmo] = 15;
$ItemMax[garmor2, OICWAmmo] = 30;
$ItemMax[garmor2, SAWAmmo] = 0;
$ItemMax[garmor2, MP5Ammo] = 0;
$ItemMax[garmor2, PSG1Ammo] = 0;
$ItemMax[garmor2, FiftyCalAmmo] = 0;
$ItemMax[garmor2, LAWAmmo] = 0;
$ItemMax[garmor2, AutoShotgunAmmo] = 0;
$ItemMax[garmor2, FlameAmmo] = 0;
$ItemMax[garmor2, StingerAmmo] = 0;
$ItemMax[garmor2, HowitzerAmmo] = 0;

$ItemMax[garmor2, SOCOMClip] = 1;
$ItemMax[garmor2, OICWClip] = 1;
$ItemMax[garmor2, SAWClip] = 0;
$ItemMax[garmor2, MP5Clip] = 0;
$ItemMax[garmor2, PSG1Clip] = 0;
$ItemMax[garmor2, FiftyCalClip] = 0;
$ItemMax[garmor2, AutoShotgunClip] = 0;
$ItemMax[garmor2, StingerClip] = 0;

$ItemMax[garmor2, EnergyPack] = 0;
$ItemMax[garmor2, RepairPack] = 1;
$ItemMax[garmor2, ShieldPack] = 0;
$ItemMax[garmor2, SensorJammerPack] = 0;
$ItemMax[garmor2, MotionSensorPack] = 0;
$ItemMax[garmor2, PulseSensorPack] = 0;
$ItemMax[garmor2, DeployableSensorJammerPack] = 0;
$ItemMax[garmor2, DeployableHealthPack] = 0;
$ItemMax[garmor2, CameraPack] = 0;
$ItemMax[garmor2, TurretPack] = 0;
$ItemMax[garmor2, AmmoPackSmall] = 1;
$ItemMax[garmor2, AmmoPackHeavy] = 1;
$ItemMax[garmor2, AmmoPackExp] = 1;
$ItemMax[garmor2, RepairKit] = 1;
$ItemMax[garmor2, DeployableInvPack] = 0;
$ItemMax[garmor2, DeployableAmmoPack] = 0;
$ItemMax[garmor2, TwentyPack] = 0;
$ItemMax[garmor2, HowitzerPack] = 0;
$ItemMax[garmor2, SAMPack] = 0;
$ItemMax[garmor2, Charge] = 1;
$ItemMax[garmor2, AirstrikePack] = 0;
$ItemMax[garmor2, GrapplePack] = 0;
$ItemMax[garmor2, GrappleHook] = 0;
$ItemMax[garmor2, ReloaderPack] = 0;
$ItemMax[garmor2, Parachute] = 1;
$ItemMax[garmor2, FuelPack] = 0;
$ItemMax[garmor2, PortGenPack] = 0;
$ItemMax[garmor2, AAPack] = 0;
$ItemMax[garmor2, MedicPack] = 0;

$MaxWeapons[garmor2] = 1;

// SPECOPS

$DamageScale[carmor2, $LandingDamageType] = 1.0;
$DamageScale[carmor2, $ImpactDamageType] = 1.0;
$DamageScale[carmor2, $CrushDamageType] = 1.0;
$DamageScale[carmor2, $BulletDamageType] = 1.2;
$DamageScale[carmor2, $PlasmaDamageType] = 1.0;
$DamageScale[carmor2, $EnergyDamageType] = 1.3;
$DamageScale[carmor2, $ExplosionDamageType] = 1.0;
$DamageScale[carmor2, $MissileDamageType] = 1.0;
$DamageScale[carmor2, $DebrisDamageType] = 1.2;
$DamageScale[carmor2, $ShrapnelDamageType] = 1.2;
$DamageScale[carmor2, $LaserDamageType] = 1.0;
$DamageScale[carmor2, $MortarDamageType] = 1.3;
$DamageScale[carmor2, $BlasterDamageType] = 1.3;
$DamageScale[carmor2, $ElectricityDamageType] = 1.0;
$DamageScale[carmor2, $MineDamageType] = 1.2;
$DamageScale[carmor2, $ChargeDamageType] = 1.2;
$DamageScale[carmor2, $AirstrikeDamageType] = 1.0;
$DamageScale[carmor2, $BleedDamageType] = 1.0;
$DamageScale[carmor2, $GrenadeDamageType] = 1.0;
$DamageScale[carmor2, $PoisonDamageType] = 1.0;
$DamageScale[carmor2, $SmokeDamageType] = 1.0;
$DamageScale[carmor2, $StabDamageType] = 1.0;

$ItemMax[carmor2, Blaster] = 1;
$ItemMax[carmor2, Chaingun] = 1;
$ItemMax[carmor2, Disclauncher] = 1;
$ItemMax[carmor2, GrenadeLauncher] = 1;
$ItemMax[carmor2, Mortar] = 0;
$ItemMax[carmor2, PlasmaGun] = 1;
$ItemMax[carmor2, LaserRifle] = 1;
$ItemMax[carmor2, EnergyRifle] = 1;
$ItemMax[carmor2, TargetingLaser] = 1;
$ItemMax[carmor2, BouncingMinePack] = 0;
$ItemMax[carmor2, APMinePack] = 0;
$ItemMax[carmor2, AAMinePack] = 0;
$ItemMax[carmor2, Grenade] = 3;
$ItemMax[carmor2, Beacon]  = 3;
$ItemMax[carmor2, Knife] = 1;
$ItemMax[carmor2, SOCOM] = 1;
$ItemMax[carmor2, OICW] = 0;
$ItemMax[carmor2, SAW] = 0;
$ItemMax[carmor2, MP5] = 1;
$ItemMax[carmor2, PSG1] = 0;
$ItemMax[carmor2, FiftyCal] = 0;
$ItemMax[carmor2, LAW] = 0;
$ItemMax[carmor2, AutoShotgun] = 0;
$ItemMax[carmor2, Howitzer] = 0;
$ItemMax[carmor2, Airstrike] = 1;
$ItemMax[carmor2, Stinger] = 1;
$ItemMax[carmor2, Flamethrower] = 0;

$ItemMax[carmor2, BulletAmmo] = 100;
$ItemMax[carmor2, PlasmaAmmo] = 30;
$ItemMax[carmor2, DiscAmmo] = 15;
$ItemMax[carmor2, GrenadeAmmo] = 10;
$ItemMax[carmor2, MortarAmmo] = 10;
$ItemMax[carmor2, SOCOMAmmo] = 15;
$ItemMax[carmor2, OICWAmmo] = 0;
$ItemMax[carmor2, SAWAmmo] = 0;
$ItemMax[carmor2, MP5Ammo] = 30;
$ItemMax[carmor2, PSG1Ammo] = 0;
$ItemMax[carmor2, FiftyCalAmmo] = 0;
$ItemMax[carmor2, LAWAmmo] = 0;
$ItemMax[carmor2, AutoShotgunAmmo] = 0;
$ItemMax[carmor2, HowitzerAmmo] = 0;
$ItemMax[carmor2, StingerAmmo] = 1;
$ItemMax[carmor2, FlameAmmo] = 0;

$ItemMax[carmor2, SOCOMClip] = 1;
$ItemMax[carmor2, OICWClip] = 0;
$ItemMax[carmor2, SAWClip] = 0;
$ItemMax[carmor2, MP5Clip] = 2;
$ItemMax[carmor2, PSG1Clip] = 0;
$ItemMax[carmor2, FiftyCalClip] = 0;
$ItemMax[carmor2, AutoShotgunClip] = 0;
$ItemMax[carmor2, StingerClip] = 1;

$ItemMax[carmor2, EnergyPack] = 0;
$ItemMax[carmor2, RepairPack] = 0;
$ItemMax[carmor2, ShieldPack] = 0;
$ItemMax[carmor2, SensorJammerPack] = 1;
$ItemMax[carmor2, MotionSensorPack] = 0;
$ItemMax[carmor2, PulseSensorPack] = 0;
$ItemMax[carmor2, DeployableSensorJammerPack] = 0;
$ItemMax[carmor2, DeployableHealthPack] = 0;
$ItemMax[carmor2, CameraPack] = 1;
$ItemMax[carmor2, TurretPack] = 0;
$ItemMax[carmor2, AmmoPackSmall] = 1;
$ItemMax[carmor2, AmmoPackHeavy] = 0;
$ItemMax[carmor2, AmmoPackExp] = 1;
$ItemMax[carmor2, RepairKit] = 1;
$ItemMax[carmor2, DeployableInvPack] = 0;
$ItemMax[carmor2, DeployableAmmoPack] = 0;
$ItemMax[carmor2, TwentyPack] = 0;
$ItemMax[carmor2, HowitzerPack] = 0;
$ItemMax[carmor2, SAMPack] = 0;
$ItemMax[carmor2, Charge] = 1;
$ItemMax[carmor2, AirstrikePack] = 1;
$ItemMax[carmor2, GrapplePack] = 1;
$ItemMax[carmor2, GrappleHook] = 1;
$ItemMax[carmor2, ReloaderPack] = 1;
$ItemMax[carmor2, Parachute] = 1;
$ItemMax[carmor2, FuelPack] = 0;
$ItemMax[carmor2, PortGenPack] = 0;
$ItemMax[carmor2, AAPack] = 0;
$ItemMax[carmor2, MedicPack] = 0;

$MaxWeapons[carmor2] = 3;

// ENGINEER

$DamageScale[earmor2, $LandingDamageType] = 1.0;
$DamageScale[earmor2, $ImpactDamageType] = 1.0;
$DamageScale[earmor2, $CrushDamageType] = 1.0;
$DamageScale[earmor2, $BulletDamageType] = 1.0;
$DamageScale[earmor2, $PlasmaDamageType] = 0.6;
$DamageScale[earmor2, $EnergyDamageType] = 1.0;
$DamageScale[earmor2, $ExplosionDamageType] = 1.0;
$DamageScale[earmor2, $MissileDamageType] = 1.0;
$DamageScale[earmor2, $ShrapnelDamageType] = 1.0;
$DamageScale[earmor2, $DebrisDamageType] = 1.0;
$DamageScale[earmor2, $LaserDamageType] = 1.0;
$DamageScale[earmor2, $MortarDamageType] = 1.0;
$DamageScale[earmor2, $BlasterDamageType] = 1.0;
$DamageScale[earmor2, $ElectricityDamageType] = 1.0;
$DamageScale[earmor2, $MineDamageType] = 1.0;
$DamageScale[earmor2, $ChargeDamageType] = 1.0;
$DamageScale[earmor2, $AirstrikeDamageType] = 1.0;
$DamageScale[earmor2, $BleedDamageType] = 1.0;
$DamageScale[earmor2, $GrenadeDamageType] = 1.0;
$DamageScale[earmor2, $PoisonDamageType] = 1.0;
$DamageScale[earmor2, $SmokeDamageType] = 1.0;
$DamageScale[earmor2, $StabDamageType] = 1.0;

$ItemMax[earmor2, Blaster] = 1;
$ItemMax[earmor2, Chaingun] = 1;
$ItemMax[earmor2, Disclauncher] = 1;
$ItemMax[earmor2, GrenadeLauncher] = 1;
$ItemMax[earmor2, Mortar] = 0;
$ItemMax[earmor2, PlasmaGun] = 1;
$ItemMax[earmor2, LaserRifle] = 1;
$ItemMax[earmor2, EnergyRifle] = 1;
$ItemMax[earmor2, TargetingLaser] = 1;
$ItemMax[earmor2, BouncingMinePack] = 1;
$ItemMax[earmor2, APMinePack] = 1;
$ItemMax[earmor2, AAMinePack] = 1;
$ItemMax[earmor2, Grenade] = 2;
$ItemMax[earmor2, Beacon]  = 3;
$ItemMax[earmor2, Knife] = 1;
$ItemMax[earmor2, SOCOM] = 1;
$ItemMax[earmor2, OICW] = 1;
$ItemMax[earmor2, SAW] = 0;
$ItemMax[earmor2, MP5] = 0;
$ItemMax[earmor2, PSG1] = 0;
$ItemMax[earmor2, FiftyCal] = 0;
$ItemMax[earmor2, LAW] = 0;
$ItemMax[earmor2, AutoShotgun] = 1;
$ItemMax[earmor2, Howitzer] = 0;
$ItemMax[earmor2, Airstrike] = 0;
$ItemMax[earmor2, Stinger] = 0;
$ItemMax[earmor2, Flamethrower] = 0;

$ItemMax[earmor2, BulletAmmo] = 100;
$ItemMax[earmor2, PlasmaAmmo] = 30;
$ItemMax[earmor2, DiscAmmo] = 15;
$ItemMax[earmor2, GrenadeAmmo] = 10;
$ItemMax[earmor2, MortarAmmo] = 10;
$ItemMax[earmor2, SOCOMAmmo] = 15;
$ItemMax[earmor2, OICWAmmo] = 30;
$ItemMax[earmor2, SAWAmmo] = 0;
$ItemMax[earmor2, MP5Ammo] = 0;
$ItemMax[earmor2, PSG1Ammo] = 0;
$ItemMax[earmor2, FiftyCalAmmo] = 0;
$ItemMax[earmor2, LAWAmmo] = 0;
$ItemMax[earmor2, AutoShotgunAmmo] = 7;
$ItemMax[earmor2, HowitzerAmmo] = 0;
$ItemMax[earmor2, StingerAmmo] = 0;
$ItemMax[earmor2, FlameAmmo] = 0;

$ItemMax[earmor2, SOCOMClip] = 1;
$ItemMax[earmor2, OICWClip] = 1;
$ItemMax[earmor2, SAWClip] = 0;
$ItemMax[earmor2, MP5Clip] = 0;
$ItemMax[earmor2, PSG1Clip] = 0;
$ItemMax[earmor2, FiftyCalClip] = 0;
$ItemMax[earmor2, AutoShotgunClip] = 4;
$ItemMax[earmor2, StingerClip] = 0;

$ItemMax[earmor2, EnergyPack] = 0;
$ItemMax[earmor2, RepairPack] = 1;
$ItemMax[earmor2, ShieldPack] = 0;
$ItemMax[earmor2, SensorJammerPack] = 1;
$ItemMax[earmor2, MotionSensorPack] = 1;
$ItemMax[earmor2, PulseSensorPack] = 1;
$ItemMax[earmor2, DeployableSensorJammerPack] = 1;
$ItemMax[earmor2, DeployableHealthPack] = 0;
$ItemMax[earmor2, CameraPack] = 1;
$ItemMax[earmor2, TurretPack] = 1;
$ItemMax[earmor2, AmmoPackSmall] = 1;
$ItemMax[earmor2, AmmoPackHeavy] = 1;
$ItemMax[earmor2, AmmoPackExp] = 0;
$ItemMax[earmor2, RepairKit] = 1;
$ItemMax[earmor2, DeployableInvPack] = 1;
$ItemMax[earmor2, DeployableAmmoPack] = 1;
$ItemMax[earmor2, TwentyPack] = 1;
$ItemMax[earmor2, HowitzerPack] = 1;
$ItemMax[earmor2, SAMPack] = 1;
$ItemMax[earmor2, Charge] = 0;
$ItemMax[earmor2, AirstrikePack] = 0;
$ItemMax[earmor2, GrapplePack] = 0;
$ItemMax[earmor2, GrappleHook] = 0;
$ItemMax[earmor2, ReloaderPack] = 1;
$ItemMax[earmor2, Parachute] = 1;
$ItemMax[earmor2, FuelPack] = 0;
$ItemMax[earmor2, PortGenPack] = 1;
$ItemMax[earmor2, AAPack] = 1;
$ItemMax[earmor2, MedicPack] = 0;

$MaxWeapons[earmor2] = 2;

// PILOT

$DamageScale[larmor2, $LandingDamageType] = 1.0;
$DamageScale[larmor2, $ImpactDamageType] = 1.0;
$DamageScale[larmor2, $CrushDamageType] = 1.0;
$DamageScale[larmor2, $BulletDamageType] = 1.2;
$DamageScale[larmor2, $PlasmaDamageType] = 1.0;
$DamageScale[larmor2, $EnergyDamageType] = 1.3;
$DamageScale[larmor2, $ExplosionDamageType] = 1.0;
$DamageScale[larmor2, $MissileDamageType] = 1.0;
$DamageScale[larmor2, $DebrisDamageType] = 1.2;
$DamageScale[larmor2, $ShrapnelDamageType] = 1.2;
$DamageScale[larmor2, $LaserDamageType] = 1.0;
$DamageScale[larmor2, $MortarDamageType] = 1.3;
$DamageScale[larmor2, $BlasterDamageType] = 1.3;
$DamageScale[larmor2, $ElectricityDamageType] = 1.0;
$DamageScale[larmor2, $MineDamageType] = 1.2;
$DamageScale[larmor2, $ChargeDamageType] = 1.2;
$DamageScale[larmor2, $AirstrikeDamageType] = 1.0;
$DamageScale[larmor2, $BleedDamageType] = 1.0;
$DamageScale[larmor2, $GrenadeDamageType] = 1.0;
$DamageScale[larmor2, $PoisonDamageType] = 1.0;
$DamageScale[larmor2, $SmokeDamageType] = 1.0;
$DamageScale[larmor2, $StabDamageType] = 1.0;

$ItemMax[larmor2, Blaster] = 1;
$ItemMax[larmor2, Chaingun] = 1;
$ItemMax[larmor2, Disclauncher] = 1;
$ItemMax[larmor2, GrenadeLauncher] = 1;
$ItemMax[larmor2, Mortar] = 0;
$ItemMax[larmor2, PlasmaGun] = 1;
$ItemMax[larmor2, LaserRifle] = 1;
$ItemMax[larmor2, EnergyRifle] = 1;
$ItemMax[larmor2, TargetingLaser] = 1;
$ItemMax[larmor2, BouncingMinePack] = 0;
$ItemMax[larmor2, APMinePack] = 0;
$ItemMax[larmor2, AAMinePack] = 0;
$ItemMax[larmor2, Grenade] = 2;
$ItemMax[larmor2, Beacon]  = 3;
$ItemMax[larmor2, Knife] = 1;
$ItemMax[larmor2, SOCOM] = 1;
$ItemMax[larmor2, OICW] = 0;
$ItemMax[larmor2, SAW] = 0;
$ItemMax[larmor2, MP5] = 0;
$ItemMax[larmor2, PSG1] = 0;
$ItemMax[larmor2, FiftyCal] = 0;
$ItemMax[larmor2, LAW] = 0;
$ItemMax[larmor2, Howitzer] = 0;
$ItemMax[larmor2, AutoShotgun] = 0;
$ItemMax[larmor2, Airstrike] = 0;
$ItemMax[larmor2, Stinger] = 0;
$ItemMax[larmor2, Flamethrower] = 0;

$ItemMax[larmor2, BulletAmmo] = 100;
$ItemMax[larmor2, PlasmaAmmo] = 30;
$ItemMax[larmor2, DiscAmmo] = 15;
$ItemMax[larmor2, GrenadeAmmo] = 10;
$ItemMax[larmor2, MortarAmmo] = 10;
$ItemMax[larmor2, SOCOMAmmo] = 15;
$ItemMax[larmor2, OICWAmmo] = 0;
$ItemMax[larmor2, SAWAmmo] = 0;
$ItemMax[larmor2, MP5Ammo] = 0;
$ItemMax[larmor2, PSG1Ammo] = 0;
$ItemMax[larmor2, FiftyCalAmmo] = 0;
$ItemMax[larmor2, LAWAmmo] = 0;
$ItemMax[larmor2, AutoShotgun] = 0;
$ItemMax[larmor2, HowitzerAmmo] = 0;
$ItemMax[larmor2, StingerAmmo] = 0;
$ItemMax[larmor2, FlameAmmo] = 0;

$ItemMax[larmor2, SOCOMClip] = 1;
$ItemMax[larmor2, OICWClip] = 0;
$ItemMax[larmor2, SAWClip] = 0;
$ItemMax[larmor2, MP5Clip] = 0;
$ItemMax[larmor2, PSG1Clip] = 0;
$ItemMax[larmor2, FiftyCalClip] = 0;
$ItemMax[larmor2, AutoShotgunClip] = 0;
$ItemMax[larmor2, StingerClip] = 0;

$ItemMax[larmor2, EnergyPack] = 0;
$ItemMax[larmor2, RepairPack] = 0;
$ItemMax[larmor2, ShieldPack] = 0;
$ItemMax[larmor2, SensorJammerPack] = 0;
$ItemMax[larmor2, MotionSensorPack] = 0;
$ItemMax[larmor2, PulseSensorPack] = 0;
$ItemMax[larmor2, DeployableSensorJammerPack] = 0;
$ItemMax[larmor2, DeployableHealthPack] = 0;
$ItemMax[larmor2, CameraPack] = 0;
$ItemMax[larmor2, TurretPack] = 0;
$ItemMax[larmor2, AmmoPackSmall] = 1;
$ItemMax[larmor2, AmmoPackHeavy] = 0;
$ItemMax[larmor2, AmmoPackExp] = 0;
$ItemMax[larmor2, RepairKit] = 1;
$ItemMax[larmor2, DeployableInvPack] = 0;
$ItemMax[larmor2, DeployableAmmoPack] = 0;
$ItemMax[larmor2, TwentyPack] = 0;
$ItemMax[larmor2, HowitzerPack] = 0;
$ItemMax[larmor2, SAMPack] = 0;
$ItemMax[larmor2, Charge] = 0;
$ItemMax[larmor2, AirstrikePack] = 0;
$ItemMax[larmor2, GrapplePack] = 0;
$ItemMax[larmor2, GrappleHook] = 0;
$ItemMax[larmor2, ReloaderPack] = 0;
$ItemMax[larmor2, Parachute] = 1;
$ItemMax[larmor2, FuelPack] = 0;
$ItemMax[larmor2, PortGenPack] = 0;
$ItemMax[larmor2, AAPack] = 0;
$ItemMax[larmor2, MedicPack] = 0;

$MaxWeapons[larmor2] = 1;

// ARTILLERY

$DamageScale[aarmor2, $LandingDamageType] = 1.0;
$DamageScale[aarmor2, $ImpactDamageType] = 1.0;
$DamageScale[aarmor2, $CrushDamageType] = 1.0;
$DamageScale[aarmor2, $BulletDamageType] = 1.2;
$DamageScale[aarmor2, $PlasmaDamageType] = 1.0;
$DamageScale[aarmor2, $EnergyDamageType] = 1.3;
$DamageScale[aarmor2, $ExplosionDamageType] = 1.0;
$DamageScale[aarmor2, $MissileDamageType] = 1.0;
$DamageScale[aarmor2, $DebrisDamageType] = 1.2;
$DamageScale[aarmor2, $ShrapnelDamageType] = 1.2;
$DamageScale[aarmor2, $LaserDamageType] = 1.0;
$DamageScale[aarmor2, $MortarDamageType] = 1.3;
$DamageScale[aarmor2, $BlasterDamageType] = 1.3;
$DamageScale[aarmor2, $ElectricityDamageType] = 1.0;
$DamageScale[aarmor2, $MineDamageType] = 1.2;
$DamageScale[aarmor2, $ChargeDamageType] = 1.2;
$DamageScale[aarmor2, $AirstrikeDamageType] = 1.0;
$DamageScale[aarmor2, $BleedDamageType] = 1.0;
$DamageScale[aarmor2, $GrenadeDamageType] = 1.0;
$DamageScale[aarmor2, $PoisonDamageType] = 1.0;
$DamageScale[aarmor2, $SmokeDamageType] = 1.0;
$DamageScale[aarmor2, $StabDamageType] = 1.0;

//$DamageScale[aarmor, $LandingDamageType] = 1.0;
//$DamageScale[aarmor, $ImpactDamageType] = 1.0;
//$DamageScale[aarmor, $CrushDamageType] = 1.0;
//$DamageScale[aarmor, $BulletDamageType] = 0.6;
//$DamageScale[aarmor, $PlasmaDamageType] = 0.4;
//$DamageScale[aarmor, $EnergyDamageType] = 0.7;
//$DamageScale[aarmor, $ExplosionDamageType] = 0.6;
//$DamageScale[aarmor, $MissileDamageType] = 0.6;
//$DamageScale[aarmor, $DebrisDamageType] = 0.8;
//$DamageScale[aarmor, $ShrapnelDamageType] = 0.8;
//$DamageScale[aarmor, $LaserDamageType] = 0.6;
//$DamageScale[aarmor, $MortarDamageType] = 0.7;
//$DamageScale[aarmor, $BlasterDamageType] = 0.7;
//$DamageScale[aarmor, $ElectricityDamageType] = 1.0;
//$DamageScale[aarmor, $MineDamageType] = 0.8;
//$DamageScale[aarmor, $ChargeDamageType] = 0.8;
//$DamageScale[aarmor, $AirstrikeDamageType] = 1.0;

$ItemMax[aarmor2, Blaster] = 0;
$ItemMax[aarmor2, Chaingun] = 0;
$ItemMax[aarmor2, Disclauncher] = 0;
$ItemMax[aarmor2, GrenadeLauncher] = 0;
$ItemMax[aarmor2, Mortar] = 0;
$ItemMax[aarmor2, PlasmaGun] = 0;
$ItemMax[aarmor2, LaserRifle] = 0;
$ItemMax[aarmor2, EnergyRifle] = 0;
$ItemMax[aarmor2, TargetingLaser] = 1;
$ItemMax[aarmor2, BouncingMinePack] = 1;
$ItemMax[aarmor2, APMinePack] = 1;
$ItemMax[aarmor2, AAMinePack] = 1;
$ItemMax[aarmor2, Grenade] = 3;
$ItemMax[aarmor2, Beacon]  = 3;
$ItemMax[aarmor2, Knife] = 1;
$ItemMax[aarmor2, SOCOM] = 1;
$ItemMax[aarmor2, OICW] = 1;
$ItemMax[aarmor2, SAW] = 0;
$ItemMax[aarmor2, MP5] = 0;
$ItemMax[aarmor2, PSG1] = 0;
$ItemMax[aarmor2, FiftyCal] = 0;
$ItemMax[aarmor2, LAW] = 0;
$ItemMax[aarmor2, AutoShotgun] = 0;
$ItemMax[aarmor2, Howitzer] = 0;
$ItemMax[aarmor2, Airstrike] = 1;
$ItemMax[aarmor2, Stinger] = 0;
$ItemMax[aarmor2, Flamethrower] = 0;

$ItemMax[aarmor2, BulletAmmo] = 0;
$ItemMax[aarmor2, PlasmaAmmo] = 0;
$ItemMax[aarmor2, DiscAmmo] = 0;
$ItemMax[aarmor2, GrenadeAmmo] = 10;
$ItemMax[aarmor2, MortarAmmo] = 10;
$ItemMax[aarmor2, SOCOMAmmo] = 15;
$ItemMax[aarmor2, OICWAmmo] = 30;
$ItemMax[aarmor2, SAWAmmo] = 0;
$ItemMax[aarmor2, MP5Ammo] = 0;
$ItemMax[aarmor2, PSG1Ammo] = 0;
$ItemMax[aarmor2, FiftyCalAmmo] = 0;
$ItemMax[aarmor2, LAWAmmo] = 0;
$ItemMax[aarmor2, AutoShotgunAmmo] = 0;
$ItemMax[aarmor2, HowitzerAmmo] = 0;
$ItemMax[aarmor2, Stinger] = 0;
$ItemMax[aarmor2, FlameAmmo] = 0;

$ItemMax[aarmor2, SOCOMClip] = 1;
$ItemMax[aarmor2, OICWClip] = 1;
$ItemMax[aarmor2, SAWClip] = 0;
$ItemMax[aarmor2, MP5Clip] = 0;
$ItemMax[aarmor2, PSG1Clip] = 0;
$ItemMax[aarmor2, FiftyCalClip] = 0;
$ItemMax[aarmor2, AutoShotgunClip] = 0;
$ItemMax[aarmor2, StingerClip] = 0;

$ItemMax[aarmor2, EnergyPack] = 0;
$ItemMax[aarmor2, RepairPack] = 0;
$ItemMax[aarmor2, ShieldPack] = 0;
$ItemMax[aarmor2, SensorJammerPack] = 0;
$ItemMax[aarmor2, MotionSensorPack] = 0;
$ItemMax[aarmor2, PulseSensorPack] = 0;
$ItemMax[aarmor2, DeployableSensorJammerPack] = 0;
$ItemMax[aarmor2, DeployableHealthPack] = 0;
$ItemMax[aarmor2, CameraPack] = 0;
$ItemMax[aarmor2, TurretPack] = 0;
$ItemMax[aarmor2, AmmoPack] = 1;
$ItemMax[aarmor2, RepairKit] = 1;
$ItemMax[aarmor2, DeployableInvPack] = 0;
$ItemMax[aarmor2, DeployableAmmoPack] = 0;
$ItemMax[aarmor2, TwentyPack] = 0;
$ItemMax[aarmor2, HowitzerPack] = 1;
$ItemMax[aarmor2, SAMPack] = 0;
$ItemMax[aarmor2, Charge] = 0;
$ItemMax[aarmor2, AirstrikePack] = 1;
$ItemMax[aarmor2, GrapplePack] = 0;
$ItemMax[aarmor2, GrappleHook] = 0;
$ItemMax[aarmor2, ReloaderPack] = 1;
$ItemMax[aarmor2, Parachute] = 1;
$ItemMax[aarmor2, FuelPack] = 0;
$ItemMax[aarmor2, PortGenPack] = 0;
$ItemMax[aarmor2, AAPack] = 0;
$ItemMax[aarmor2, MedicPack] = 0;

$MaxWeapons[aarmor2] = 2;

// MEDIC FEMALE

$DamageScale[mfemale2, $LandingDamageType] = 1.0;
$DamageScale[mfemale2, $ImpactDamageType] = 1.0;
$DamageScale[mfemale2, $CrushDamageType] = 1.0;
$DamageScale[mfemale2, $BulletDamageType] = 1.2;
$DamageScale[mfemale2, $PlasmaDamageType] = 1.0;
$DamageScale[mfemale2, $EnergyDamageType] = 1.3;
$DamageScale[mfemale2, $ExplosionDamageType] = 1.0;
$DamageScale[mfemale2, $MissileDamageType] = 1.0;
$DamageScale[mfemale2, $DebrisDamageType] = 1.2;
$DamageScale[mfemale2, $ShrapnelDamageType] = 1.2;
$DamageScale[mfemale2, $LaserDamageType] = 1.0;
$DamageScale[mfemale2, $MortarDamageType] = 1.3;
$DamageScale[mfemale2, $BlasterDamageType] = 1.3;
$DamageScale[mfemale2, $ElectricityDamageType] = 1.0;
$DamageScale[mfemale2, $MineDamageType] = 1.2;
$DamageScale[mfemale2, $ChargeDamageType] = 1.2;
$DamageScale[mfemale2, $AirstrikeDamageType] = 1.0;
$DamageScale[mfemale2, $BleedDamageType] = 1.0;
$DamageScale[mfemale2, $GrenadeDamageType] = 1.0;
$DamageScale[mfemale2, $PoisonDamageType] = 1.0;
$DamageScale[mfemale2, $SmokeDamageType] = 1.0;
$DamageScale[mfemale2, $StabDamageType] = 1.0;

$ItemMax[mfemale2, Blaster] = 1;
$ItemMax[mfemale2, Chaingun] = 1;
$ItemMax[mfemale2, Disclauncher] = 1;
$ItemMax[mfemale2, GrenadeLauncher] = 1;
$ItemMax[mfemale2, Mortar] = 0;
$ItemMax[mfemale2, PlasmaGun] = 1;
$ItemMax[mfemale2, LaserRifle] = 1;
$ItemMax[mfemale2, EnergyRifle] = 1;
$ItemMax[mfemale2, TargetingLaser] = 1;
$ItemMax[mfemale2, BouncingMinePack] = 0;
$ItemMax[mfemale2, APMinePack] = 0;
$ItemMax[mfemale2, AAMinePack] = 0;
$ItemMax[mfemale2, Grenade] = 2;
$ItemMax[mfemale2, Beacon]  = 3;
$ItemMax[mfemale2, SOCOM] = 1;
$ItemMax[mfemale2, OICW] = 0;
$ItemMax[mfemale2, SAW] = 0;
$ItemMax[mfemale2, MP5] = 1;
$ItemMax[mfemale2, PSG1] = 0;
$ItemMax[mfemale2, FiftyCal] = 0;
$ItemMax[mfemale2, LAW] = 0;
$ItemMax[mfemale2, AutoShotgun] = 0;
$ItemMax[mfemale2, Howitzer] = 0;
$ItemMax[mfemale2, Airstrike] = 0;
$ItemMax[mfemale2, Stinger] = 0;
$ItemMax[mfemale2, Flamethrower] = 0;

$ItemMax[mfemale2, BulletAmmo] = 100;
$ItemMax[mfemale2, PlasmaAmmo] = 30;
$ItemMax[mfemale2, DiscAmmo] = 15;
$ItemMax[mfemale2, GrenadeAmmo] = 10;
$ItemMax[mfemale2, MortarAmmo] = 10;
$ItemMax[mfemale2, Knife] = 1;
$ItemMax[mfemale2, SOCOMAmmo] = 15;
$ItemMax[mfemale2, OICWAmmo] = 0;
$ItemMax[mfemale2, SAWAmmo] = 0;
$ItemMax[mfemale2, MP5Ammo] = 30;
$ItemMax[mfemale2, PSG1Ammo] = 0;
$ItemMax[mfemale2, FiftyCalAmmo] = 0;
$ItemMax[mfemale2, LAWAmmo] = 0;
$ItemMax[mfemale2, AutoShotgunAmmo] = 0;
$ItemMax[mfemale2, HowitzerAmmo] = 0;
$ItemMax[mfemale2, StingerAmmo] = 0;
$ItemMax[mfemale2, FlameAmmo] = 0;

$ItemMax[mfemale2, SOCOMClip] = 1;
$ItemMax[mfemale2, OICWClip] = 0;
$ItemMax[mfemale2, SAWClip] = 0;
$ItemMax[mfemale2, MP5Clip] = 1;
$ItemMax[mfemale2, PSG1Clip] = 0;
$ItemMax[mfemale2, FiftyCalClip] = 0;
$ItemMax[mfemale2, AutoShotgunClip] = 0;
$ItemMax[mfemale2, StingerClip] = 0;

$ItemMax[mfemale2, EnergyPack] = 0;
$ItemMax[mfemale2, RepairPack] = 0;
$ItemMax[mfemale2, ShieldPack] = 0;
$ItemMax[mfemale2, SensorJammerPack] = 0;
$ItemMax[mfemale2, MotionSensorPack] = 0;
$ItemMax[mfemale2, PulseSensorPack] = 0;
$ItemMax[mfemale2, DeployableSensorJammerPack] = 0;
$ItemMax[mfemale2, DeployableHealthPack] = 1;
$ItemMax[mfemale2, CameraPack] = 0;
$ItemMax[mfemale2, TurretPack] = 0;
$ItemMax[mfemale2, AmmoPackSmall] = 1;
$ItemMax[mfemale2, AmmoPackHeavy] = 0;
$ItemMax[mfemale2, AmmoPackExp] = 0;
$ItemMax[mfemale2, RepairKit] = 1;
$ItemMax[mfemale2, DeployableInvPack] = 0;
$ItemMax[mfemale2, DeployableAmmoPack] = 0;
$ItemMax[mfemale2, TwentyPack] = 0;
$ItemMax[mfemale2, HowitzerPack] = 0;
$ItemMax[mfemale2, SAMPack] = 0;
$ItemMax[mfemale2, Charge] = 0;
$ItemMax[mfemale2, AirstrikePack] = 0;
$ItemMax[mfemale2, GrapplePack] = 0;
$ItemMax[mfemale2, GrappleHook] = 0;
$ItemMax[mfemale2, ReloaderPack] = 0;
$ItemMax[mfemale2, Parachute] = 1;
$ItemMax[mfemale2, FuelPack] = 0;
$ItemMax[mfemale2, PortGenPack] = 0;
$ItemMax[mfemale2, AAPack] = 0;
$ItemMax[mfemale2, MedicPack] = 1;

$MaxWeapons[mfemale2] = 2;

// SNIPER FEMALE

$DamageScale[sfemale2, $LandingDamageType] = 1.0;
$DamageScale[sfemale2, $ImpactDamageType] = 1.0;	
$DamageScale[sfemale2, $CrushDamageType] = 1.0;	
$DamageScale[sfemale2, $BulletDamageType] = 1.2;
$DamageScale[sfemale2, $PlasmaDamageType] = 1.0;
$DamageScale[sfemale2, $EnergyDamageType] = 1.3;
$DamageScale[sfemale2, $ExplosionDamageType] = 1.0;
$DamageScale[sfemale2, $MissileDamageType] = 1.0;
$DamageScale[sfemale2, $ShrapnelDamageType] = 1.2;
$DamageScale[sfemale2, $DebrisDamageType] = 1.2;
$DamageScale[sfemale2, $LaserDamageType] = 1.0;
$DamageScale[sfemale2, $MortarDamageType] = 1.3;
$DamageScale[sfemale2, $BlasterDamageType] = 1.3;
$DamageScale[sfemale2, $ElectricityDamageType] = 1.0;
$DamageScale[sfemale2, $MineDamageType] = 1.2;
$DamageScale[sfemale2, $ChargeDamageType] = 1.2;
$DamageScale[sfemale2, $AirstrikeDamageType] = 1.0;
$DamageScale[sfemale2, $BleedDamageType] = 1.0;
$DamageScale[sfemale2, $GrenadeDamageType] = 1.0;
$DamageScale[sfemale2, $PoisonDamageType] = 1.0;
$DamageScale[sfemale2, $SmokeDamageType] = 1.0;
$DamageScale[sfemale2, $StabDamageType] = 1.0;

$ItemMax[sfemale2, Blaster] = 1;
$ItemMax[sfemale2, Chaingun] = 1;
$ItemMax[sfemale2, Disclauncher] = 1;
$ItemMax[sfemale2, GrenadeLauncher] = 1;
$ItemMax[sfemale2, Mortar] = 0;
$ItemMax[sfemale2, PlasmaGun] = 1;
$ItemMax[sfemale2, LaserRifle] = 1;
$ItemMax[sfemale2, EnergyRifle] = 1;
$ItemMax[sfemale2, TargetingLaser] = 1;
$ItemMax[sfemale2, BouncingMinePack] = 0;
$ItemMax[sfemale2, APMinePack] = 0;
$ItemMax[sfemale2, AAMinePack] = 0;
$ItemMax[sfemale2, Grenade] = 2;
$ItemMax[sfemale2, Beacon] = 3;
$ItemMax[sfemale2, Knife] = 1;
$ItemMax[sfemale2, SOCOM] = 1;
$ItemMax[sfemale2, OICW] = 0;
$ItemMax[sfemale2, SAW] = 0;
$ItemMax[sfemale2, MP5] = 0;
$ItemMax[sfemale2, PSG1] = 1;
$ItemMax[sfemale2, FiftyCal] = 1;
$ItemMax[sfemale2, LAW] = 0;
$ItemMax[sfemale2, AutoShotgun] = 0;
$ItemMax[sfemale2, Howitzer] = 0;
$ItemMax[sfemale2, Airstrike] = 0;
$ItemMax[sfemale2, Stinger] = 0;
$ItemMax[sfemale2, Flamethrower] = 0;

$ItemMax[sfemale2, BulletAmmo] = 100;
$ItemMax[sfemale2, PlasmaAmmo] = 30;
$ItemMax[sfemale2, DiscAmmo] = 15;
$ItemMax[sfemale2, GrenadeAmmo] = 10;
$ItemMax[sfemale2, MortarAmmo] = 10;
$ItemMax[sfemale2, SOCOMAmmo] = 15;
$ItemMax[sfemale2, OICWAmmo] = 0;
$ItemMax[sfemale2, SAWAmmo] = 0;
$ItemMax[sfemale2, MP5Ammo] = 0;
$ItemMax[sfemale2, PSG1Ammo] = 5;
$ItemMax[sfemale2, FiftyCalAmmo] = 5;
$ItemMax[sfemale2, LAWAmmo] = 0;
$ItemMax[sfemale2, AutoShotgunAmmo] = 0;
$ItemMax[sfemale2, HowitzerAmmo] = 0;
$ItemMax[sfemale2, StingerAmmo] = 0;
$ItemMax[sfemale2, FlameAmmo] = 0;

$ItemMax[sfemale2, SOCOMClip] = 1;
$ItemMax[sfemale2, OICWClip] = 0;
$ItemMax[sfemale2, SAWClip] = 0;
$ItemMax[sfemale2, MP5Clip] = 0;
$ItemMax[sfemale2, PSG1Clip] = 3;
$ItemMax[sfemale2, FiftyCalClip] = 2;
$ItemMax[sfemale2, AutoShotgunClip] = 0;
$ItemMax[sfemale2, StingerClip] = 0;

$ItemMax[sfemale2, EnergyPack] = 0;
$ItemMax[sfemale2, RepairPack] = 0;
$ItemMax[sfemale2, ShieldPack] = 0;
$ItemMax[sfemale2, SensorJammerPack] = 1;
$ItemMax[sfemale2, MotionSensorPack] = 0;
$ItemMax[sfemale2, PulseSensorPack] = 0;
$ItemMax[sfemale2, DeployableSensorJammerPack] = 0;
$ItemMax[sfemale2, DeployableHealthPack] = 0;
$ItemMax[sfemale2, CameraPack] = 0;
$ItemMax[sfemale2, TurretPack] = 0;
$ItemMax[sfemale2, AmmoPackSmall] = 1;
$ItemMax[sfemale2, AmmoPackHeavy] = 1;
$ItemMax[sfemale2, AmmoPackExp] = 0;
$ItemMax[sfemale2, RepairKit] = 1;
$ItemMax[sfemale2, DeployableInvPack] = 0;
$ItemMax[sfemale2, DeployableAmmoPack] = 0;
$ItemMax[sfemale2, TwentyPack] = 0;
$ItemMax[sfemale2, HowitzerPack] = 0;
$ItemMax[sfemale2, SAMPack] = 0;
$ItemMax[sfemale2, Charge] = 0;
$ItemMax[sfemale2, AirstrikePack] = 0;
$ItemMax[sfemale2, GrapplePack] = 0;
$ItemMax[sfemale2, GrappleHook] = 0;
$ItemMax[sfemale2, ReloaderPack] = 0;
$ItemMax[sfemale2, Parachute] = 1;
$ItemMax[sfemale2, FuelPack] = 0;
$ItemMax[sfemale2, PortGenPack] = 0;
$ItemMax[sfemale2, AAPack] = 0;
$ItemMax[sfemale2, MedicPack] = 0;

$MaxWeapons[sfemale2] = 1;

// INFANTRY FEMALE

$DamageScale[ifemale2, $LandingDamageType] = 1.0;
$DamageScale[ifemale2, $ImpactDamageType] = 1.0;
$DamageScale[ifemale2, $CrushDamageType] = 1.0;
$DamageScale[ifemale2, $BulletDamageType] = 1.0;
$DamageScale[ifemale2, $EnergyDamageType] = 1.0;
$DamageScale[ifemale2, $PlasmaDamageType] = 0.6;
$DamageScale[ifemale2, $ExplosionDamageType] = 1.0;
$DamageScale[ifemale2, $MissileDamageType] = 1.0;
$DamageScale[ifemale2, $ShrapnelDamageType] = 1.0;
$DamageScale[ifemale2, $DebrisDamageType] = 1.0;
$DamageScale[ifemale2, $LaserDamageType] = 1.0;
$DamageScale[ifemale2, $MortarDamageType] = 1.0;
$DamageScale[ifemale2, $BlasterDamageType] = 1.0;
$DamageScale[ifemale2, $ElectricityDamageType] = 1.0;
$DamageScale[ifemale2, $MineDamageType] = 1.0;
$DamageScale[ifemale2, $ChargeDamageType] = 1.0;
$DamageScale[ifemale2, $AirstrikeDamageType] = 1.0;
$DamageScale[ifemale2, $BleedDamageType] = 1.0;
$DamageScale[ifemale2, $GrenadeDamageType] = 1.0;
$DamageScale[ifemale2, $PoisonDamageType] = 1.0;
$DamageScale[ifemale2, $SmokeDamageType] = 1.0;
$DamageScale[ifemale2, $StabDamageType] = 1.0;

$ItemMax[ifemale2, Blaster] = 1;
$ItemMax[ifemale2, Chaingun] = 1;
$ItemMax[ifemale2, Disclauncher] = 1;
$ItemMax[ifemale2, GrenadeLauncher] = 1;
$ItemMax[ifemale2, Mortar] = 0;
$ItemMax[ifemale2, PlasmaGun] = 1;
$ItemMax[ifemale2, LaserRifle] = 0;
$ItemMax[ifemale2, EnergyRifle] = 1;
$ItemMax[ifemale2, TargetingLaser] = 1;
$ItemMax[ifemale2, BouncingMinePack] = 0;
$ItemMax[ifemale2, APMinePack] = 1;
$ItemMax[ifemale2, AAMinePack] = 0;
$ItemMax[ifemale2, Grenade] = 3;
$ItemMax[ifemale2, Beacon] = 3;
$ItemMax[ifemale2, Knife] = 1;
$ItemMax[ifemale2, SOCOM] = 1;
$ItemMax[ifemale2, OICW] = 1;
$ItemMax[ifemale2, SAW] = 1;
$ItemMax[ifemale2, MP5] = 0;
$ItemMax[ifemale2, PSG1] = 0;
$ItemMax[ifemale2, FiftyCal] = 0;
$ItemMax[ifemale2, LAW] = 1;
$ItemMax[ifemale2, AutoShotgun] = 1;
$ItemMax[ifemale2, Howitzer] = 0;
$ItemMax[ifemale2, Flamethrower] = 1;
$ItemMax[ifemale2, Airstrike] = 0;
$ItemMax[ifemale2, Stinger] = 0;

$ItemMax[ifemale2, BulletAmmo] = 150;
$ItemMax[ifemale2, PlasmaAmmo] = 40;
$ItemMax[ifemale2, DiscAmmo] = 15;
$ItemMax[ifemale2, GrenadeAmmo] = 10;
$ItemMax[ifemale2, MortarAmmo] = 10;
$ItemMax[ifemale2, SOCOMAmmo] = 15;
$ItemMax[ifemale2, OICWAmmo] = 30;
$ItemMax[ifemale2, SAWAmmo] = 200;
$ItemMax[ifemale2, MP5Ammo] = 0;
$ItemMax[ifemale2, PSG1Ammo] = 0;
$ItemMax[ifemale2, FiftyCalAmmo] = 0;
$ItemMax[ifemale2, LAWAmmo] = 1;
$ItemMax[ifemale2, AutoShotgunAmmo] = 7;
$ItemMax[ifemale2, HowitzerAmmo] = 0;
$ItemMax[ifemale2, StingerAmmo] = 0;
$ItemMax[ifemale2, FlameAmmo] = 150;

$ItemMax[ifemale2, SOCOMClip] = 1;
$ItemMax[ifemale2, OICWClip] = 2;
$ItemMax[ifemale2, SAWClip] = 1;
$ItemMax[ifemale2, MP5Clip] = 0;
$ItemMax[ifemale2, PSG1Clip] = 0;
$ItemMax[ifemale2, FiftyCalClip] = 0;
$ItemMax[ifemale2, AutoShotgunClip] = 5;
$ItemMax[ifemale2, StingerClip] = 0;

$ItemMax[ifemale2, EnergyPack] = 0;
$ItemMax[ifemale2, RepairPack] = 1;
$ItemMax[ifemale2, ShieldPack] = 0;
$ItemMax[ifemale2, SensorJammerPack] = 0;
$ItemMax[ifemale2, MotionSensorPack] = 0;
$ItemMax[ifemale2, PulseSensorPack] = 0;
$ItemMax[ifemale2, DeployableSensorJammerPack] = 0;
$ItemMax[ifemale2, DeployableHealthPack] = 0;
$ItemMax[ifemale2, CameraPack] = 1;
$ItemMax[ifemale2, TurretPack] = 0;
$ItemMax[ifemale2, AmmoPackSmall] = 1;
$ItemMax[ifemale2, AmmoPackHeavy] = 1;
$ItemMax[ifemale2, AmmoPackExp] = 1;
$ItemMax[ifemale2, RepairKit] = 1;
$ItemMax[ifemale2, DeployableInvPack] = 0;
$ItemMax[ifemale2, DeployableAmmoPack] = 0;
$ItemMax[ifemale2, TwentyPack] = 0;
$ItemMax[ifemale2, HowitzerPack] = 0;
$ItemMax[ifemale2, SAMPack] = 0;
$ItemMax[ifemale2, Charge] = 0;
$ItemMax[ifemale2, AirstrikePack] = 0;
$ItemMax[ifemale2, GrapplePack] = 0;
$ItemMax[ifemale2, GrappleHook] = 0;
$ItemMax[ifemale2, ReloaderPack] = 1;
$ItemMax[ifemale2, Parachute] = 1;
$ItemMax[ifemale2, FuelPack] = 1;
$ItemMax[ifemale2, PortGenPack] = 1;
$ItemMax[ifemale2, AAPack] = 0;
$ItemMax[ifemale2, MedicPack] = 0;

$MaxWeapons[ifemale2] = 2;

// Grenadier Female

$DamageScale[gfemale2, $LandingDamageType] = 1.0;
$DamageScale[gfemale2, $ImpactDamageType] = 1.0;
$DamageScale[gfemale2, $CrushDamageType] = 1.0;
$DamageScale[gfemale2, $BulletDamageType] = 1.0;
$DamageScale[gfemale2, $PlasmaDamageType] = 0.6;
$DamageScale[gfemale2, $EnergyDamageType] = 1.0;
$DamageScale[gfemale2, $ExplosionDamageType] = 1.0;
$DamageScale[gfemale2, $MissileDamageType] = 1.0;
$DamageScale[gfemale2, $ShrapnelDamageType] = 0.8;
$DamageScale[gfemale2, $DebrisDamageType] = 1.0;
$DamageScale[gfemale2, $LaserDamageType] = 1.0;
$DamageScale[gfemale2, $MortarDamageType] = 1.0;
$DamageScale[gfemale2, $BlasterDamageType] = 1.0;
$DamageScale[gfemale2, $ElectricityDamageType] = 1.0;
$DamageScale[gfemale2, $MineDamageType] = 1.0;
$DamageScale[gfemale2, $ChargeDamageType] = 1.0;
$DamageScale[gfemale2, $AirstrikeDamageType] = 1.0;
$DamageScale[gfemale2, $BleedDamageType] = 1.0;
$DamageScale[gfemale2, $GrenadeDamageType] = 0.8;
$DamageScale[gfemale2, $PoisonDamageType] = 1.0;
$DamageScale[gfemale2, $SmokeDamageType] = 1.0;
$DamageScale[gfemale2, $StabDamageType] = 1.0;

$ItemMax[gfemale2, Blaster] = 1;
$ItemMax[gfemale2, Chaingun] = 1;
$ItemMax[gfemale2, Disclauncher] = 1;
$ItemMax[gfemale2, GrenadeLauncher] = 1;
$ItemMax[gfemale2, Mortar] = 0;
$ItemMax[gfemale2, PlasmaGun] = 1;
$ItemMax[gfemale2, LaserRifle] = 1;
$ItemMax[gfemale2, EnergyRifle] = 1;
$ItemMax[gfemale2, TargetingLaser] = 1;
$ItemMax[gfemale2, BouncingMinePack] = 1;
$ItemMax[gfemale2, APMinePack] = 1;
$ItemMax[gfemale2, AAMinePack] = 0;
$ItemMax[gfemale2, Grenade] = 10;
$ItemMax[gfemale2, Beacon]  = 3;
$ItemMax[gfemale2, Knife] = 1;
$ItemMax[gfemale2, SOCOM] = 1;
$ItemMax[gfemale2, OICW] = 1;
$ItemMax[gfemale2, SAW] = 0;
$ItemMax[gfemale2, MP5] = 0;
$ItemMax[gfemale2, PSG1] = 0;
$ItemMax[gfemale2, FiftyCal] = 0;
$ItemMax[gfemale2, LAW] = 0;
$ItemMax[gfemale2, AutoShotgun] = 0;
$ItemMax[gfemale2, Stinger] = 0;
$ItemMax[gfemale2, Flamethrower] = 0;
$ItemMax[gfemale2, Howitzer] = 0;
$ItemMax[gfemale2, Airstrike] = 0;

$ItemMax[gfemale2, BulletAmmo] = 100;
$ItemMax[gfemale2, PlasmaAmmo] = 30;
$ItemMax[gfemale2, DiscAmmo] = 15;
$ItemMax[gfemale2, GrenadeAmmo] = 10;
$ItemMax[gfemale2, MortarAmmo] = 10;
$ItemMax[gfemale2, SOCOMAmmo] = 15;
$ItemMax[gfemale2, OICWAmmo] = 30;
$ItemMax[gfemale2, SAWAmmo] = 0;
$ItemMax[gfemale2, MP5Ammo] = 0;
$ItemMax[gfemale2, PSG1Ammo] = 0;
$ItemMax[gfemale2, FiftyCalAmmo] = 0;
$ItemMax[gfemale2, LAWAmmo] = 0;
$ItemMax[gfemale2, AutoShotgunAmmo] = 0;
$ItemMax[gfemale2, FlameAmmo] = 0;
$ItemMax[gfemale2, StingerAmmo] = 0;
$ItemMax[gfemale2, HowitzerAmmo] = 0;

$ItemMax[gfemale2, SOCOMClip] = 1;
$ItemMax[gfemale2, OICWClip] = 1;
$ItemMax[gfemale2, SAWClip] = 0;
$ItemMax[gfemale2, MP5Clip] = 0;
$ItemMax[gfemale2, PSG1Clip] = 0;
$ItemMax[gfemale2, FiftyCalClip] = 0;
$ItemMax[gfemale2, AutoShotgunClip] = 0;
$ItemMax[gfemale2, StingerClip] = 0;

$ItemMax[gfemale2, EnergyPack] = 0;
$ItemMax[gfemale2, RepairPack] = 1;
$ItemMax[gfemale2, ShieldPack] = 0;
$ItemMax[gfemale2, SensorJammerPack] = 0;
$ItemMax[gfemale2, MotionSensorPack] = 0;
$ItemMax[gfemale2, PulseSensorPack] = 0;
$ItemMax[gfemale2, DeployableSensorJammerPack] = 0;
$ItemMax[gfemale2, DeployableHealthPack] = 0;
$ItemMax[gfemale2, CameraPack] = 0;
$ItemMax[gfemale2, TurretPack] = 0;
$ItemMax[gfemale2, AmmoPackSmall] = 1;
$ItemMax[gfemale2, AmmoPackHeavy] = 1;
$ItemMax[gfemale2, AmmoPackExp] = 1;
$ItemMax[gfemale2, RepairKit] = 1;
$ItemMax[gfemale2, DeployableInvPack] = 0;
$ItemMax[gfemale2, DeployableAmmoPack] = 0;
$ItemMax[gfemale2, TwentyPack] = 0;
$ItemMax[gfemale2, HowitzerPack] = 0;
$ItemMax[gfemale2, SAMPack] = 0;
$ItemMax[gfemale2, Charge] = 1;
$ItemMax[gfemale2, AirstrikePack] = 0;
$ItemMax[gfemale2, GrapplePack] = 0;
$ItemMax[gfemale2, GrappleHook] = 0;
$ItemMax[gfemale2, ReloaderPack] = 0;
$ItemMax[gfemale2, Parachute] = 1;
$ItemMax[gfemale2, FuelPack] = 0;
$ItemMax[gfemale2, PortGenPack] = 0;
$ItemMax[gfemale2, AAPack] = 0;
$ItemMax[gfemale2, MedicPack] = 0;

$MaxWeapons[gfemale2] = 1;

// SPECOPS FEMALE

$DamageScale[cfemale2, $LandingDamageType] = 1.0;
$DamageScale[cfemale2, $ImpactDamageType] = 1.0;	
$DamageScale[cfemale2, $CrushDamageType] = 1.0;	
$DamageScale[cfemale2, $BulletDamageType] = 1.2;
$DamageScale[cfemale2, $PlasmaDamageType] = 1.0;
$DamageScale[cfemale2, $EnergyDamageType] = 1.3;
$DamageScale[cfemale2, $ExplosionDamageType] = 1.0;
$DamageScale[cfemale2, $MissileDamageType] = 1.0;
$DamageScale[cfemale2, $ShrapnelDamageType] = 1.2;
$DamageScale[cfemale2, $DebrisDamageType] = 1.2;
$DamageScale[cfemale2, $LaserDamageType] = 1.0;
$DamageScale[cfemale2, $MortarDamageType] = 1.3;
$DamageScale[cfemale2, $BlasterDamageType] = 1.3;
$DamageScale[cfemale2, $ElectricityDamageType] = 1.0;
$DamageScale[cfemale2, $MineDamageType] = 1.2;
$DamageScale[cfemale2, $ChargeDamageType] = 1.2;
$DamageScale[cfemale2, $AirstrikeDamageType] = 1.0;
$DamageScale[cfemale2, $BleedDamageType] = 1.0;
$DamageScale[cfemale2, $GrenadeDamageType] = 1.0;
$DamageScale[cfemale2, $PoisonDamageType] = 1.0;
$DamageScale[cfemale2, $SmokeDamageType] = 1.0;
$DamageScale[cfemale2, $StabDamageType] = 1.0;

$ItemMax[cfemale2, Blaster] = 1;
$ItemMax[cfemale2, Chaingun] = 1;
$ItemMax[cfemale2, Disclauncher] = 1;
$ItemMax[cfemale2, GrenadeLauncher] = 1;
$ItemMax[cfemale2, Mortar] = 0;
$ItemMax[cfemale2, PlasmaGun] = 1;
$ItemMax[cfemale2, LaserRifle] = 1;
$ItemMax[cfemale2, EnergyRifle] = 1;
$ItemMax[cfemale2, TargetingLaser] = 1;
$ItemMax[cfemale2, BouncingMinePack] = 0;
$ItemMax[cfemale2, APMinePack] = 0;
$ItemMax[cfemale2, AAMinePack] = 0;
$ItemMax[cfemale2, Grenade] = 3;
$ItemMax[cfemale2, Beacon] = 3;
$ItemMax[cfemale2, Knife] = 1;
$ItemMax[cfemale2, SOCOM] = 1;
$ItemMax[cfemale2, OICW] = 0;
$ItemMax[cfemale2, SAW] = 0;
$ItemMax[cfemale2, MP5] = 1;
$ItemMax[cfemale2, PSG1] = 0;
$ItemMax[cfemale2, FiftyCal] = 0;
$ItemMax[cfemale2, LAW] = 0;
$ItemMax[cfemale2, AutoShotgun] = 0;
$ItemMax[cfemale2, Howitzer] = 0;
$ItemMax[cfemale2, Airstrike] = 1;
$ItemMax[cfemale2, Stinger] = 1;
$ItemMax[cfemale2, Flamethrower] = 0;

$ItemMax[cfemale2, BulletAmmo] = 100;
$ItemMax[cfemale2, PlasmaAmmo] = 30;
$ItemMax[cfemale2, DiscAmmo] = 15;
$ItemMax[cfemale2, GrenadeAmmo] = 10;
$ItemMax[cfemale2, MortarAmmo] = 10;
$ItemMax[cfemale2, SOCOMAmmo] = 15;
$ItemMax[cfemale2, OICWAmmo] = 0;
$ItemMax[cfemale2, SAWAmmo] = 0;
$ItemMax[cfemale2, MP5Ammo] = 30;
$ItemMax[cfemale2, PSG1Ammo] = 0;
$ItemMax[cfemale2, FiftyCalAmmo] = 0;
$ItemMax[cfemale2, LAWAmmo] = 0;
$ItemMax[cfemale2, AutoShotgunAmmo] = 0;
$ItemMax[cfemale2, HowitzerAmmo] = 0;
$ItemMax[cfemale2, StingerAmmo] = 1;
$ItemMax[cfemale2, FlameAmmo] = 0;

$ItemMax[cfemale2, SOCOMClip] = 1;
$ItemMax[cfemale2, OICWClip] = 0;
$ItemMax[cfemale2, SAWClip] = 0;
$ItemMax[cfemale2, MP5Clip] = 2;
$ItemMax[cfemale2, PSG1Clip] = 0;
$ItemMax[cfemale2, FiftyCalClip] = 0;
$ItemMax[cfemale2, AutoShotgunClip] = 0;
$ItemMax[cfemale2, StingerClip] = 1;

$ItemMax[cfemale2, EnergyPack] = 0;
$ItemMax[cfemale2, RepairPack] = 0;
$ItemMax[cfemale2, ShieldPack] = 0;
$ItemMax[cfemale2, SensorJammerPack] = 1;
$ItemMax[cfemale2, MotionSensorPack] = 0;
$ItemMax[cfemale2, PulseSensorPack] = 0;
$ItemMax[cfemale2, DeployableSensorJammerPack] = 1;
$ItemMax[cfemale2, DeployableHealthPack] = 0;
$ItemMax[cfemale2, CameraPack] = 1;
$ItemMax[cfemale2, TurretPack] = 0;
$ItemMax[cfemale2, AmmoPackSmall] = 1;
$ItemMax[cfemale2, AmmoPackHeavy] = 0;
$ItemMax[cfemale2, AmmoPackExp] = 1;
$ItemMax[cfemale2, RepairKit] = 1;
$ItemMax[cfemale2, DeployableInvPack] = 0;
$ItemMax[cfemale2, DeployableAmmoPack] = 0;
$ItemMax[cfemale2, TwentyPack] = 0;
$ItemMax[cfemale2, HowitzerPack] = 0;
$ItemMax[cfemale2, SAMPack] = 0;
$ItemMax[cfemale2, Charge] = 1;
$ItemMax[cfemale2, AirstrikePack] = 1;
$ItemMax[cfemale2, GrapplePack] = 1;
$ItemMax[cfemale2, GrappleHook] = 1;
$ItemMax[cfemale2, ReloaderPack] = 1;
$ItemMax[cfemale2, Parachute] = 1;
$ItemMax[cfemale2, FuelPack] = 0;
$ItemMax[cfemale2, PortGenPack] = 0;
$ItemMax[cfemale2, AAPack] = 0;
$ItemMax[cfemale2, MedicPack] = 0;

$MaxWeapons[cfemale2] = 3;

// ENGINEER FEMALE

$DamageScale[efemale2, $LandingDamageType] = 1.0;
$DamageScale[efemale2, $ImpactDamageType] = 1.0;
$DamageScale[efemale2, $CrushDamageType] = 1.0;
$DamageScale[efemale2, $BulletDamageType] = 1.0;
$DamageScale[efemale2, $EnergyDamageType] = 1.0;
$DamageScale[efemale2, $PlasmaDamageType] = 0.6;
$DamageScale[efemale2, $ExplosionDamageType] = 1.0;
$DamageScale[efemale2, $MissileDamageType] = 1.0;
$DamageScale[efemale2, $ShrapnelDamageType] = 1.0;
$DamageScale[efemale2, $DebrisDamageType] = 1.0;
$DamageScale[efemale2, $LaserDamageType] = 1.0;
$DamageScale[efemale2, $MortarDamageType] = 1.0;
$DamageScale[efemale2, $BlasterDamageType] = 1.0;
$DamageScale[efemale2, $ElectricityDamageType] = 1.0;
$DamageScale[efemale2, $MineDamageType] = 1.0;
$DamageScale[efemale2, $ChargeDamageType] = 1.0;
$DamageScale[efemale2, $AirstrikeDamageType] = 1.0;
$DamageScale[efemale2, $BleedDamageType] = 1.0;
$DamageScale[efemale2, $GrenadeDamageType] = 1.0;
$DamageScale[efemale2, $PoisonDamageType] = 1.0;
$DamageScale[efemale2, $SmokeDamageType] = 1.0;
$DamageScale[efemale2, $StabDamageType] = 1.0;

$ItemMax[efemale2, Blaster] = 1;
$ItemMax[efemale2, Chaingun] = 1;
$ItemMax[efemale2, Disclauncher] = 1;
$ItemMax[efemale2, GrenadeLauncher] = 1;
$ItemMax[efemale2, Mortar] = 0;
$ItemMax[efemale2, PlasmaGun] = 1;
$ItemMax[efemale2, LaserRifle] = 0;
$ItemMax[efemale2, EnergyRifle] = 1;
$ItemMax[efemale2, TargetingLaser] = 1;
$ItemMax[efemale2, BouncingMinePack] = 1;
$ItemMax[efemale2, APMinePack] = 1;
$ItemMax[efemale2, AAMinePack] = 1;
$ItemMax[efemale2, Grenade] = 2;
$ItemMax[efemale2, Beacon] = 3;
$ItemMax[efemale2, Knife] = 1;
$ItemMax[efemale2, SOCOM] = 1;
$ItemMax[efemale2, OICW] = 1;
$ItemMax[efemale2, SAW] = 0;
$ItemMax[efemale2, MP5] = 0;
$ItemMax[efemale2, PSG1] = 0;
$ItemMax[efemale2, FiftyCal] = 0;
$ItemMax[efemale2, LAW] = 0;
$ItemMax[efemale2, AutoShotgun] = 1;
$ItemMax[efemale2, Howitzer] = 0;
$ItemMax[efemale2, Airstrike] = 0;
$ItemMax[efemale2, Stinger] = 0;
$ItemMax[efemale2, Flamethrower] = 0;

$ItemMax[efemale2, BulletAmmo] = 150;
$ItemMax[efemale2, PlasmaAmmo] = 40;
$ItemMax[efemale2, DiscAmmo] = 15;
$ItemMax[efemale2, GrenadeAmmo] = 10;
$ItemMax[efemale2, MortarAmmo] = 10;
$ItemMax[efemale2, SOCOMAmmo] = 15;
$ItemMax[efemale2, OICWAmmo] = 30;
$ItemMax[efemale2, SAWAmmo] = 0;
$ItemMax[efemale2, MP5Ammo] = 0;
$ItemMax[efemale2, PSG1Ammo] = 0;
$ItemMax[efemale2, FiftyCalAmmo] = 0;
$ItemMax[efemale2, LAWAmmo] = 0;
$ItemMax[efemale2, AutoShotgunAmmo] = 7;
$ItemMax[efemale2, HowitzerAmmo] = 0;
$ItemMax[efemale2, StingerAmmo] = 0;
$ItemMax[efemale2, FlameAmmo] = 0;

$ItemMax[efemale2, SOCOMClip] = 1;
$ItemMax[efemale2, OICWClip] = 1;
$ItemMax[efemale2, SAWClip] = 0;
$ItemMax[efemale2, MP5Clip] = 0;
$ItemMax[efemale2, PSG1Clip] = 0;
$ItemMax[efemale2, FiftyCalClip] = 0;
$ItemMax[efemale2, AutoShotgunClip] = 4;
$ItemMax[efemale2, StingerClip] = 0;

$ItemMax[efemale2, EnergyPack] = 0;
$ItemMax[efemale2, RepairPack] = 1;
$ItemMax[efemale2, ShieldPack] = 0;
$ItemMax[efemale2, SensorJammerPack] = 1;
$ItemMax[efemale2, MotionSensorPack] = 1;
$ItemMax[efemale2, PulseSensorPack] = 1;
$ItemMax[efemale2, DeployableSensorJammerPack] = 1;
$ItemMax[efemale2, DeployableHealthPack] = 0;
$ItemMax[efemale2, CameraPack] = 1;
$ItemMax[efemale2, TurretPack] = 1;
$ItemMax[efemale2, AmmoPackSmall] = 1;
$ItemMax[efemale2, AmmoPackHeavy] = 1;
$ItemMax[efemale2, AmmoPackExp] = 0;
$ItemMax[efemale2, RepairKit] = 1;
$ItemMax[efemale2, DeployableInvPack] = 1;
$ItemMax[efemale2, DeployableAmmoPack] = 1;
$ItemMax[efemale2, TwentyPack] = 1;
$ItemMax[efemale2, HowitzerPack] = 1;
$ItemMax[efemale2, SAMPack] = 1;
$ItemMax[efemale2, Charge] = 0;
$ItemMax[efemale2, AirstrikePack] = 0;
$ItemMax[efemale2, GrapplePack] = 0;
$ItemMax[efemale2, GrappleHook] = 0;
$ItemMax[efemale2, ReloaderPack] = 1;
$ItemMax[efemale2, Parachute] = 1;
$ItemMax[efemale2, FuelPack] = 0;
$ItemMax[efemale2, PortGenPack] = 1;
$ItemMax[efemale2, AAPack] = 1;
$ItemMax[efemale2, MedicPack] = 0;

$MaxWeapons[efemale2] = 2;

// PILOT FEMALE

$DamageScale[lfemale2, $LandingDamageType] = 1.0;
$DamageScale[lfemale2, $ImpactDamageType] = 1.0;	
$DamageScale[lfemale2, $CrushDamageType] = 1.0;	
$DamageScale[lfemale2, $BulletDamageType] = 1.2;
$DamageScale[lfemale2, $PlasmaDamageType] = 1.0;
$DamageScale[lfemale2, $EnergyDamageType] = 1.3;
$DamageScale[lfemale2, $ExplosionDamageType] = 1.0;
$DamageScale[lfemale2, $MissileDamageType] = 1.0;
$DamageScale[lfemale2, $ShrapnelDamageType] = 1.2;
$DamageScale[lfemale2, $DebrisDamageType] = 1.2;
$DamageScale[lfemale2, $LaserDamageType] = 1.0;
$DamageScale[lfemale2, $MortarDamageType] = 1.3;
$DamageScale[lfemale2, $BlasterDamageType] = 1.3;
$DamageScale[lfemale2, $ElectricityDamageType] = 1.0;
$DamageScale[lfemale2, $MineDamageType] = 1.2;
$DamageScale[lfemale2, $ChargeDamageType] = 1.2;
$DamageScale[lfemale2, $AirstrikeDamageType] = 1.0;
$DamageScale[lfemale2, $BleedDamageType] = 1.0;
$DamageScale[lfemale2, $GrenadeDamageType] = 1.0;
$DamageScale[lfemale2, $PoisonDamageType] = 1.0;
$DamageScale[lfemale2, $SmokeDamageType] = 1.0;
$DamageScale[lfemale2, $StabDamageType] = 1.0;

$ItemMax[lfemale2, Blaster] = 1;
$ItemMax[lfemale2, Chaingun] = 1;
$ItemMax[lfemale2, Disclauncher] = 1;
$ItemMax[lfemale2, GrenadeLauncher] = 1;
$ItemMax[lfemale2, Mortar] = 0;
$ItemMax[lfemale2, PlasmaGun] = 1;
$ItemMax[lfemale2, LaserRifle] = 1;
$ItemMax[lfemale2, EnergyRifle] = 1;
$ItemMax[lfemale2, TargetingLaser] = 1;
$ItemMax[lfemale2, BouncingMinePack] = 0;
$ItemMax[lfemale2, APMinePack] = 0;
$ItemMax[lfemale2, AAMinePack] = 0;
$ItemMax[lfemale2, Grenade] = 2;
$ItemMax[lfemale2, Beacon] = 3;
$ItemMax[lfemale2, Knife] = 1;
$ItemMax[lfemale2, SOCOM] = 1;
$ItemMax[lfemale2, OICW] = 0;
$ItemMax[lfemale2, SAW] = 0;
$ItemMax[lfemale2, MP5] = 0;
$ItemMax[lfemale2, PSG1] = 0;
$ItemMax[lfemale2, FiftyCal] = 0;
$ItemMax[lfemale2, LAW] = 0;
$ItemMax[lfemale2, AutoShotgun] = 0;
$ItemMax[lfemale2, Howitzer] = 0;
$ItemMax[lfemale2, Airstrike] = 0;
$ItemMax[lfemale2, Stinger] = 0;
$ItemMax[lfemale2, Flamethrower] = 0;

$ItemMax[lfemale2, BulletAmmo] = 100;
$ItemMax[lfemale2, PlasmaAmmo] = 30;
$ItemMax[lfemale2, DiscAmmo] = 15;
$ItemMax[lfemale2, GrenadeAmmo] = 10;
$ItemMax[lfemale2, MortarAmmo] = 10;
$ItemMax[lfemale2, SOCOMAmmo] = 15;
$ItemMax[lfemale2, OICWAmmo] = 0;
$ItemMax[lfemale2, SAWAmmo] = 0;
$ItemMax[lfemale2, MP5Ammo] = 0;
$ItemMax[lfemale2, PSG1Ammo] = 0;
$ItemMax[lfemale2, FiftyCalAmmo] = 0;
$ItemMax[lfemale2, LAWAmmo] = 0;
$ItemMax[lfemale2, AutoShotgunAmmo] = 0;
$ItemMax[lfemale2, HowitzerAmmo] = 0;
$ItemMax[lfemale2, StingerAmmo] = 0;
$ItemMax[lfemale2, FlameAmmo] = 0;

$ItemMax[lfemale, SOCOMClip] = 1;
$ItemMax[lfemale, OICWClip] = 0;
$ItemMax[lfemale, SAWClip] = 0;
$ItemMax[lfemale, MP5Clip] = 0;
$ItemMax[lfemale, PSG1Clip] = 0;
$ItemMax[lfemale, FiftyCalClip] = 0;
$ItemMax[lfemale, AutoShotgunClip] = 0;
$ItemMax[lfemale, StingerClip] = 0;

$ItemMax[lfemale2, EnergyPack] = 0;
$ItemMax[lfemale2, RepairPack] = 0;
$ItemMax[lfemale2, ShieldPack] = 0;
$ItemMax[lfemale2, SensorJammerPack] = 0;
$ItemMax[lfemale2, MotionSensorPack] = 0;
$ItemMax[lfemale2, PulseSensorPack] = 0;
$ItemMax[lfemale2, DeployableSensorJammerPack] = 0;
$ItemMax[lfemale2, DeployableHealthPack] = 0;
$ItemMax[lfemale2, CameraPack] = 0;
$ItemMax[lfemale2, TurretPack] = 0;
$ItemMax[lfemale2, AmmoPackSmall] = 1;
$ItemMax[lfemale2, AmmoPackHeavy] = 0;
$ItemMax[lfemale2, AmmoPackExp] = 0;
$ItemMax[lfemale2, RepairKit] = 1;
$ItemMax[lfemale2, DeployableInvPack] = 0;
$ItemMax[lfemale2, DeployableAmmoPack] = 0;
$ItemMax[lfemale2, TwentyPack] = 0;
$ItemMax[lfemale2, HowitzerPack] = 0;
$ItemMax[lfemale2, SAMPack] = 0;
$ItemMax[lfemale2, Charge] = 0;
$ItemMax[lfemale2, AirstrikePack] = 0;
$ItemMax[lfemale2, GrapplePack] = 0;
$ItemMax[lfemale2, GrappleHook] = 0;
$ItemMax[lfemale2, ReloaderPack] = 0;
$ItemMax[lfemale2, Parachute] = 1;
$ItemMax[lfemale2, FuelPack] = 0;
$ItemMax[lfemale2, PortGenPack] = 0;
$ItemMax[lfemale2, AAPack] = 0;
$ItemMax[lfemale2, MedicPack] = 0;

$MaxWeapons[lfemale2] = 1;

// ARTILLERY FEMALE

$DamageScale[afemale2, $LandingDamageType] = 1.0;
$DamageScale[afemale2, $ImpactDamageType] = 1.0;
$DamageScale[afemale2, $CrushDamageType] = 1.0;
$DamageScale[afemale2, $BulletDamageType] = 1.2;
$DamageScale[afemale2, $PlasmaDamageType] = 1.0;
$DamageScale[afemale2, $EnergyDamageType] = 1.3;
$DamageScale[afemale2, $ExplosionDamageType] = 1.0;
$DamageScale[afemale2, $MissileDamageType] = 1.0;
$DamageScale[afemale2, $DebrisDamageType] = 1.2;
$DamageScale[afemale2, $ShrapnelDamageType] = 1.2;
$DamageScale[afemale2, $LaserDamageType] = 1.0;
$DamageScale[afemale2, $MortarDamageType] = 1.3;
$DamageScale[afemale2, $BlasterDamageType] = 1.3;
$DamageScale[afemale2, $ElectricityDamageType] = 1.0;
$DamageScale[afemale2, $MineDamageType] = 1.2;
$DamageScale[afemale2, $ChargeDamageType] = 1.2;
$DamageScale[afemale2, $AirstrikeDamageType] = 1.0;
$DamageScale[afemale2, $BleedDamageType] = 1.0;
$DamageScale[afemale2, $GrenadeDamageType] = 1.0;
$DamageScale[afemale2, $PoisonDamageType] = 1.0;
$DamageScale[afemale2, $SmokeDamageType] = 1.0;
$DamageScale[afemale2, $StabDamageType] = 1.0;

//$DamageScale[afemale, $LandingDamageType] = 1.0;
//$DamageScale[afemale, $ImpactDamageType] = 1.0;
//$DamageScale[afemale, $CrushDamageType] = 1.0;
//$DamageScale[afemale, $BulletDamageType] = 0.6;
//$DamageScale[afemale, $PlasmaDamageType] = 0.4;
//$DamageScale[afemale, $EnergyDamageType] = 0.7;
//$DamageScale[afemale, $ExplosionDamageType] = 0.6;
//$DamageScale[afemale, $MissileDamageType] = 0.6;
//$DamageScale[afemale, $DebrisDamageType] = 0.8;
//$DamageScale[afemale, $ShrapnelDamageType] = 0.8;
//$DamageScale[afemale, $LaserDamageType] = 0.6;
//$DamageScale[afemale, $MortarDamageType] = 0.7;
//$DamageScale[afemale, $BlasterDamageType] = 0.7;
//$DamageScale[afemale, $ElectricityDamageType] = 1.0;
//$DamageScale[afemale, $MineDamageType] = 0.8;
//$DamageScale[afemale, $ChargeDamageType] = 0.8;
//$DamageScale[afemale, $AirstrikeDamageType] = 1.0;

$ItemMax[afemale2, Blaster] = 0;
$ItemMax[afemale2, Chaingun] = 0;
$ItemMax[afemale2, Disclauncher] = 0;
$ItemMax[afemale2, GrenadeLauncher] = 0;
$ItemMax[afemale2, Mortar] = 0;
$ItemMax[afemale2, PlasmaGun] = 0;
$ItemMax[afemale2, LaserRifle] = 0;
$ItemMax[afemale2, EnergyRifle] = 0;
$ItemMax[afemale2, TargetingLaser] = 1;
$ItemMax[afemale2, BouncingMinePack] = 1;
$ItemMax[afemale2, APMinePack] = 1;
$ItemMax[afemale2, AAMinePack] = 1;
$ItemMax[afemale2, Grenade] = 3;
$ItemMax[afemale2, Beacon]  = 3;
$ItemMax[afemale2, Knife] = 1;
$ItemMax[afemale2, SOCOM] = 1;
$ItemMax[afemale2, OICW] = 1;
$ItemMax[afemale2, SAW] = 0;
$ItemMax[afemale2, MP5] = 0;
$ItemMax[afemale2, PSG1] = 0;
$ItemMax[afemale2, FiftyCal] = 0;
$ItemMax[afemale2, LAW] = 0;
$ItemMax[afemale2, AutoShotgun] = 0;
$ItemMax[afemale2, Howitzer] = 0;
$ItemMax[afemale2, Airstrike] = 1;
$ItemMax[afemale2, Stinger] = 0;
$ItemMax[afemale2, Flamethrower] = 0;

$ItemMax[afemale2, BulletAmmo] = 0;
$ItemMax[afemale2, PlasmaAmmo] = 0;
$ItemMax[afemale2, DiscAmmo] = 0;
$ItemMax[afemale2, GrenadeAmmo] = 10;
$ItemMax[afemale2, MortarAmmo] = 10;
$ItemMax[afemale2, SOCOMAmmo] = 15;
$ItemMax[afemale2, OICWAmmo] = 30;
$ItemMax[afemale2, SAWAmmo] = 0;
$ItemMax[afemale2, MP5Ammo] = 0;
$ItemMax[afemale2, PSG1Ammo] = 0;
$ItemMax[afemale2, FiftyCalAmmo] = 0;
$ItemMax[afemale2, LAWAmmo] = 0;
$ItemMax[afemale2, AutoShotgunAmmo] = 0;
$ItemMax[afemale2, HowitzerAmmo] = 0;
$ItemMax[afemale2, Stinger] = 0;
$ItemMax[afemale2, FlameAmmo] = 0;

$ItemMax[afemale2, SOCOMClip] = 1;
$ItemMax[afemale2, OICWClip] = 1;
$ItemMax[afemale2, SAWClip] = 0;
$ItemMax[afemale2, MP5Clip] = 0;
$ItemMax[afemale2, PSG1Clip] = 0;
$ItemMax[afemale2, FiftyCalClip] = 0;
$ItemMax[afemale2, AutoShotgunClip] = 0;
$ItemMax[afemale2, StingerClip] = 0;

$ItemMax[afemale2, EnergyPack] = 0;
$ItemMax[afemale2, RepairPack] = 0;
$ItemMax[afemale2, ShieldPack] = 0;
$ItemMax[afemale2, SensorJammerPack] = 0;
$ItemMax[afemale2, MotionSensorPack] = 0;
$ItemMax[afemale2, PulseSensorPack] = 0;
$ItemMax[afemale2, DeployableSensorJammerPack] = 0;
$ItemMax[afemale2, DeployableHealthPack] = 0;
$ItemMax[afemale2, CameraPack] = 0;
$ItemMax[afemale2, TurretPack] = 0;
$ItemMax[afemale2, AmmoPack] = 1;
$ItemMax[afemale2, RepairKit] = 1;
$ItemMax[afemale2, DeployableInvPack] = 0;
$ItemMax[afemale2, DeployableAmmoPack] = 0;
$ItemMax[afemale2, TwentyPack] = 0;
$ItemMax[afemale2, HowitzerPack] = 1;
$ItemMax[afemale2, SAMPack] = 0;
$ItemMax[afemale2, Charge] = 0;
$ItemMax[afemale2, AirstrikePack] = 1;
$ItemMax[afemale2, GrapplePack] = 0;
$ItemMax[afemale2, GrappleHook] = 0;
$ItemMax[afemale2, ReloaderPack] = 1;
$ItemMax[afemale2, Parachute] = 1;
$ItemMax[afemale2, FuelPack] = 0;
$ItemMax[afemale2, PortGenPack] = 0;
$ItemMax[afemale2, AAPack] = 0;
$ItemMax[afemale2, MedicPack] = 0;

$MaxWeapons[afemale2] = 2;


// MEDIC

PlayerData marmor2
{
   className = "Armor";
   shapeFile = "larmor";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
   canCrouch = true;

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0.0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 6;
   maxBackwardSpeed = 5;
   maxSideSpeed = 5;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, direction
	// firstPerson, chaseCam, thirdPerson, signalThread
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
   animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 


    // celebration animations:
   animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
    // taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
    // poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;
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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// SNIPER

PlayerData sarmor2
{
   className = "Armor";
   shapeFile = "larmor";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
   canCrouch = true;

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0.0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 6;
   maxBackwardSpeed = 5;
   maxSideSpeed = 5;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, direction
	// firstPerson, chaseCam, thirdPerson, signalThread
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
   animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 


    // celebration animations:
   animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
    // taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
    // poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;
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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// INFANTRY

PlayerData iarmor2
{
   className = "Armor";
   shapeFile = "marmor";
   flameShapeName = "mflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   cancrouch = false;
   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 1.0;
   maxForwardSpeed = 4.0;
   maxBackwardSpeed = 3.5;
   maxSideSpeed = 3.5;
   groundForce = 35 * 13.0;
   mass = 13.0;
   groundtraction = 3.0;
	
	maxEnergy = 80;
   drag = 1.0;
   density = 1.5;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;

   rFootSounds = 
   {
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSnow,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft
  }; 
   lFootSounds =
   {
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSnow,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft
   };

   footPrints = { 2, 3 };

   boxWidth = 0.4;
   boxDepth = 0.4;
   boxNormalHeight = 2.25;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.20;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// Grenadier

PlayerData garmor2
{
   className = "Armor";
   shapeFile = "marmor";
   flameShapeName = "mflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   cancrouch = false;
   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 1.0;
   maxForwardSpeed = 4.0;
   maxBackwardSpeed = 3.5;
   maxSideSpeed = 3.5;
   groundForce = 35 * 13.0;
   mass = 13.0;
   groundtraction = 3.0;
	
	maxEnergy = 80;
   drag = 1.0;
   density = 1.5;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;

   rFootSounds = 
   {
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSnow,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft
  }; 
   lFootSounds =
   {
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSnow,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft
   };

   footPrints = { 2, 3 };

   boxWidth = 0.4;
   boxDepth = 0.4;
   boxNormalHeight = 2.25;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.20;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// SPECOPS

PlayerData carmor2
{
   className = "Armor";
   shapeFile = "larmor";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   shadowDetailMask = 1;

   visibleToSensor = False;
	mapFilter = 1;
	mapIcon = "M_player";
   canCrouch = true;

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 6;
   maxBackwardSpeed = 5;
   maxSideSpeed = 5;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, direction
	// firstPerson, chaseCam, thirdPerson, signalThread
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 


    // celebration animations:
   animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
    // taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
    // poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;
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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// ENGINEER

PlayerData earmor2
{
   className = "Armor";
   shapeFile = "marmor";
   flameShapeName = "mflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   cancrouch = false;
   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 1.0;
   maxForwardSpeed = 4.0;
   maxBackwardSpeed = 3.5;
   maxSideSpeed = 3.5;
   groundForce = 35 * 13.0;
   mass = 13.0;
   groundtraction = 3.0;
	
	maxEnergy = 80;
   drag = 1.0;
   density = 1.5;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;

   rFootSounds = 
   {
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSnow,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft
  }; 
   lFootSounds =
   {
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSnow,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft
   };

   footPrints = { 2, 3 };

   boxWidth = 0.4;
   boxDepth = 0.4;
   boxNormalHeight = 2.25;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.20;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// PILOT

PlayerData larmor2
{
   className = "Armor";
   shapeFile = "larmor";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
   canCrouch = true;

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 6;
   maxBackwardSpeed = 5;
   maxSideSpeed = 5;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, direction
	// firstPerson, chaseCam, thirdPerson, signalThread
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
   animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 


    // celebration animations:
   animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
    // taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
    // poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;
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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// ARTILLERY

PlayerData aarmor2
{
   className = "Armor";
   shapeFile = "larmor";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
   cancrouch = false;

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0.0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 6;
   maxBackwardSpeed = 5;
   maxSideSpeed = 5;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, direction
	// firstPerson, chaseCam, thirdPerson, signalThread
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
   animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 


    // celebration animations:
   animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
    // taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
    // poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;
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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// MEDIC FEMALE

PlayerData mfemale2
{
   className = "Armor";
   shapeFile = "lfemale";
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   canCrouch = true;
   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 6;
   maxBackwardSpeed = 5;
   maxSideSpeed = 5;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };


   //jetSound = SoundJetLight;

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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// SNIPER FEMALE

PlayerData sfemale2
{
   className = "Armor";
   shapeFile = "lfemale";
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   canCrouch = true;
   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 6;
   maxBackwardSpeed = 5;
   maxSideSpeed = 5;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };


   //jetSound = SoundJetLight;

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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// INFANTRY FEMALE

PlayerData ifemale2
{
   className = "Armor";
   shapeFile = "mfemale";
   flameShapeName = "mflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

   cancrouch = false;
	maxDamage = 1.0;
   maxForwardSpeed = 4.0;
   maxBackwardSpeed = 3.5;
   maxSideSpeed = 3.5;
   groundForce = 35 * 13.0;
   mass = 13.0;
   groundtraction = 3.0;
	maxEnergy = 80;
   mass = 13.0;
   drag = 1.0;
   density = 1.5;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   animData[0]  = { "root", none, 1, true, true, true, false, 0 };
   animData[1]  = { "run", none, 1, true, false, true, false, 3 };
   animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
   animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
   animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
   animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
   animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
   animData[7] = { "crouch root", none, 1, true, false, true, false, 3 };
   animData[8] = { "crouch root", none, 1, true, false, true, false, 3 };
   animData[9] = { "crouch root", none, -1, true, false, true, false, 3 };
   animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
   animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
   animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
   animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
   animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
   animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
   animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
   animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
   animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;

   rFootSounds = 
   {
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSnow,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft
  }; 
   lFootSounds =
   {
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSnow,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft
   };

   footPrints = { 2, 3 };

   boxWidth = 0.4;
   boxDepth = 0.4;
   boxNormalHeight = 2.25;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.20;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// Grenadier FEMALE

PlayerData gfemale2
{
   className = "Armor";
   shapeFile = "mfemale";
   flameShapeName = "mflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

   cancrouch = false;
	maxDamage = 1.0;
   maxForwardSpeed = 4.0;
   maxBackwardSpeed = 3.5;
   maxSideSpeed = 3.5;
   groundForce = 35 * 13.0;
   mass = 13.0;
   groundtraction = 3.0;
	maxEnergy = 80;
   mass = 13.0;
   drag = 1.0;
   density = 1.5;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   animData[0]  = { "root", none, 1, true, true, true, false, 0 };
   animData[1]  = { "run", none, 1, true, false, true, false, 3 };
   animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
   animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
   animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
   animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
   animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
   animData[7] = { "crouch root", none, 1, true, false, true, false, 3 };
   animData[8] = { "crouch root", none, 1, true, false, true, false, 3 };
   animData[9] = { "crouch root", none, -1, true, false, true, false, 3 };
   animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
   animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
   animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
   animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
   animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
   animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
   animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
   animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
   animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;

   rFootSounds = 
   {
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSnow,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft
  }; 
   lFootSounds =
   {
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSnow,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft
   };

   footPrints = { 2, 3 };

   boxWidth = 0.4;
   boxDepth = 0.4;
   boxNormalHeight = 2.25;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.20;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// SPECOPS FEMALE

PlayerData cfemale2
{
   className = "Armor";
   shapeFile = "lfemale";
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = False;
	mapFilter = 1;
	mapIcon = "M_player";

   canCrouch = true;
   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 6;
   maxBackwardSpeed = 5;
   maxSideSpeed = 5;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };


   //jetSound = SoundJetLight;

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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// ENGINEER FEMALE

PlayerData efemale2
{
   className = "Armor";
   shapeFile = "mfemale";
   flameShapeName = "mflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

   cancrouch = false;
	maxDamage = 1.0;
   maxForwardSpeed = 4.0;
   maxBackwardSpeed = 3.5;
   maxSideSpeed = 3.5;
   groundForce = 35 * 13.0;
   mass = 13.0;
   groundtraction = 3.0;
	maxEnergy = 80;
   mass = 13.0;
   drag = 1.0;
   density = 1.5;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   animData[0]  = { "root", none, 1, true, true, true, false, 0 };
   animData[1]  = { "run", none, 1, true, false, true, false, 3 };
   animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
   animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
   animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
   animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
   animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
   animData[7] = { "crouch root", none, 1, true, false, true, false, 3 };
   animData[8] = { "crouch root", none, 1, true, false, true, false, 3 };
   animData[9] = { "crouch root", none, -1, true, false, true, false, 3 };
   animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
   animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
   animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
   animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
   animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
   animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
   animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
   animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
   animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;

   rFootSounds = 
   {
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSnow,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft
  }; 
   lFootSounds =
   {
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSnow,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft
   };

   footPrints = { 2, 3 };

   boxWidth = 0.4;
   boxDepth = 0.4;
   boxNormalHeight = 2.25;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.20;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// PILOT FEMALE

PlayerData lfemale2
{
   className = "Armor";
   shapeFile = "lfemale";
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   canCrouch = true;
   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 6;
   maxBackwardSpeed = 5;
   maxSideSpeed = 5;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };


   //jetSound = SoundJetLight;

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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// ARTILLERY FEMALE

PlayerData afemale2
{
   className = "Armor";
   shapeFile = "lfemale";
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   cancrouch = false;
   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 6;
   maxBackwardSpeed = 5;
   maxSideSpeed = 5;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };


   //jetSound = SoundJetLight;

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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Heavy leg wounds (both out)
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

// MEDIC

$DamageScale[marmor3, $LandingDamageType] = 1.0;
$DamageScale[marmor3, $ImpactDamageType] = 1.0;
$DamageScale[marmor3, $CrushDamageType] = 1.0;
$DamageScale[marmor3, $BulletDamageType] = 1.2;
$DamageScale[marmor3, $PlasmaDamageType] = 1.0;
$DamageScale[marmor3, $EnergyDamageType] = 1.3;
$DamageScale[marmor3, $ExplosionDamageType] = 1.0;
$DamageScale[marmor3, $MissileDamageType] = 1.0;
$DamageScale[marmor3, $DebrisDamageType] = 1.2;
$DamageScale[marmor3, $ShrapnelDamageType] = 1.2;
$DamageScale[marmor3, $LaserDamageType] = 1.0;
$DamageScale[marmor3, $MortarDamageType] = 1.3;
$DamageScale[marmor3, $BlasterDamageType] = 1.3;
$DamageScale[marmor3, $ElectricityDamageType] = 1.0;
$DamageScale[marmor3, $MineDamageType] = 1.2;
$DamageScale[marmor3, $ChargeDamageType] = 1.2;
$DamageScale[marmor3, $AirstrikeDamageType] = 1.0;
$DamageScale[marmor3, $BleedDamageType] = 1.0;
$DamageScale[marmor3, $GrenadeDamageType] = 1.0;
$DamageScale[marmor3, $PoisonDamageType] = 1.0;
$DamageScale[marmor3, $SmokeDamageType] = 1.0;
$DamageScale[marmor3, $StabDamageType] = 1.0;

$ItemMax[marmor3, Blaster] = 1;
$ItemMax[marmor3, Chaingun] = 1;
$ItemMax[marmor3, Disclauncher] = 1;
$ItemMax[marmor3, GrenadeLauncher] = 1;
$ItemMax[marmor3, Mortar] = 0;
$ItemMax[marmor3, PlasmaGun] = 1;
$ItemMax[marmor3, LaserRifle] = 1;
$ItemMax[marmor3, EnergyRifle] = 1;
$ItemMax[marmor3, TargetingLaser] = 1;
$ItemMax[marmor3, BouncingMinePack] = 0;
$ItemMax[marmor3, APMinePack] = 0;
$ItemMax[marmor3, AAMinePack] = 0;
$ItemMax[marmor3, Grenade] = 2;
$ItemMax[marmor3, Beacon]  = 3;
$ItemMax[marmor3, Knife] = 1;
$ItemMax[marmor3, SOCOM] = 1;
$ItemMax[marmor3, OICW] = 0;
$ItemMax[marmor3, SAW] = 0;
$ItemMax[marmor3, MP5] = 1;
$ItemMax[marmor3, PSG1] = 0;
$ItemMax[marmor3, FiftyCal] = 0;
$ItemMax[marmor3, LAW] = 0;
$ItemMax[marmor3, AutoShotgun] = 0;
$ItemMax[marmor3, Howitzer] = 0;
$ItemMax[marmor3, Airstrike] = 0;
$ItemMax[marmor3, GrappleHook] = 0;
$ItemMax[marmor3, Stinger] = 0;
$ItemMax[marmor3, Flamethrower] = 0;

$ItemMax[marmor3, BulletAmmo] = 100;
$ItemMax[marmor3, PlasmaAmmo] = 30;
$ItemMax[marmor3, DiscAmmo] = 15;
$ItemMax[marmor3, GrenadeAmmo] = 10;
$ItemMax[marmor3, MortarAmmo] = 10;
$ItemMax[marmor3, SOCOMAmmo] = 5;
$ItemMax[marmor3, OICWAmmo] = 0;
$ItemMax[marmor3, SAWAmmo] = 0;
$ItemMax[marmor3, MP5Ammo] = 30;
$ItemMax[marmor3, PSG1Ammo] = 0;
$ItemMax[marmor3, FiftyCalAmmo] = 0;
$ItemMax[marmor3, LAWAmmo] = 0;
$ItemMax[marmor3, AutoShotgunAmmo] = 0;
$ItemMax[marmor3, HowitzerAmmo] = 0;
$ItemMax[marmor3, StingerAmmo] = 0;
$ItemMax[marmor3, FlameAmmo] = 0;

$ItemMax[marmor3, SOCOMClip] = 1;
$ItemMax[marmor3, OICWClip] = 0;
$ItemMax[marmor3, SAWClip] = 0;
$ItemMax[marmor3, MP5Clip] = 1;
$ItemMax[marmor3, PSG1Clip] = 0;
$ItemMax[marmor3, FiftyCalClip] = 0;
$ItemMax[marmor3, AutoShotgunClip] = 0;
$ItemMax[marmor3, StingerClip] = 0;

$ItemMax[marmor3, EnergyPack] = 0;
$ItemMax[marmor3, RepairPack] = 0;
$ItemMax[marmor3, ShieldPack] = 0;
$ItemMax[marmor3, SensorJammerPack] = 0;
$ItemMax[marmor3, MotionSensorPack] = 0;
$ItemMax[marmor3, PulseSensorPack] = 0;
$ItemMax[marmor3, DeployableSensorJammerPack] = 0;
$ItemMax[marmor3, DeployableHealthPack] = 1;
$ItemMax[marmor3, CameraPack] = 0;
$ItemMax[marmor3, TurretPack] = 0;
$ItemMax[marmor3, AmmoPackSmall] = 1;
$ItemMax[marmor3, AmmoPackHeavy] = 0;
$ItemMax[marmor3, AmmoPackExp] = 0;
$ItemMax[marmor3, RepairKit] = 1;
$ItemMax[marmor3, DeployableInvPack] = 0;
$ItemMax[marmor3, DeployableAmmoPack] = 0;
$ItemMax[marmor3, TwentyPack] = 0;
$ItemMax[marmor3, HowitzerPack] = 0;
$ItemMax[marmor3, SAMPack] = 0;
$ItemMax[marmor3, Charge] = 0;
$ItemMax[marmor3, AirstrikePack] = 0;
$ItemMax[marmor3, GrapplePack] = 0;
$ItemMax[marmor3, ReloaderPack] = 0;
$ItemMax[marmor3, Parachute] = 1;
$ItemMax[marmor3, FuelPack] = 0;
$ItemMax[marmor3, PortGenPack] = 0;
$ItemMax[marmor3, AAPack] = 0;
$ItemMax[marmor3, MedicPack] = 1;

$MaxWeapons[marmor3] = 2;

// SNIPER

$DamageScale[sarmor3, $LandingDamageType] = 1.0;
$DamageScale[sarmor3, $ImpactDamageType] = 1.0;
$DamageScale[sarmor3, $CrushDamageType] = 1.0;
$DamageScale[sarmor3, $BulletDamageType] = 1.2;
$DamageScale[sarmor3, $PlasmaDamageType] = 1.0;
$DamageScale[sarmor3, $EnergyDamageType] = 1.3;
$DamageScale[sarmor3, $ExplosionDamageType] = 1.0;
$DamageScale[sarmor3, $MissileDamageType] = 1.0;
$DamageScale[sarmor3, $DebrisDamageType] = 1.2;
$DamageScale[sarmor3, $ShrapnelDamageType] = 1.2;
$DamageScale[sarmor3, $LaserDamageType] = 1.0;
$DamageScale[sarmor3, $MortarDamageType] = 1.3;
$DamageScale[sarmor3, $BlasterDamageType] = 1.3;
$DamageScale[sarmor3, $ElectricityDamageType] = 1.0;
$DamageScale[sarmor3, $MineDamageType] = 1.2;
$DamageScale[sarmor3, $ChargeDamageType] = 1.2;
$DamageScale[sarmor3, $AirstrikeDamageType] = 1.0;
$DamageScale[sarmor3, $BleedDamageType] = 1.0;
$DamageScale[sarmor3, $GrenadeDamageType] = 1.0;
$DamageScale[sarmor3, $PoisonDamageType] = 1.0;
$DamageScale[sarmor3, $SmokeDamageType] = 1.0;
$DamageScale[sarmor3, $StabDamageType] = 1.0;

$ItemMax[sarmor3, Blaster] = 1;
$ItemMax[sarmor3, Chaingun] = 1;
$ItemMax[sarmor3, Disclauncher] = 1;
$ItemMax[sarmor3, GrenadeLauncher] = 1;
$ItemMax[sarmor3, Mortar] = 0;
$ItemMax[sarmor3, PlasmaGun] = 1;
$ItemMax[sarmor3, LaserRifle] = 1;
$ItemMax[sarmor3, EnergyRifle] = 1;
$ItemMax[sarmor3, TargetingLaser] = 1;
$ItemMax[sarmor3, BouncingMinePack] = 0;
$ItemMax[sarmor3, APMinePack] = 0;
$ItemMax[sarmor3, AAMinePack] = 0;
$ItemMax[sarmor3, Grenade] = 2;
$ItemMax[sarmor3, Beacon]  = 3;
$ItemMax[sarmor3, Knife] = 1;
$ItemMax[sarmor3, SOCOM] = 1;
$ItemMax[sarmor3, OICW] = 0;
$ItemMax[sarmor3, SAW] = 0;
$ItemMax[sarmor3, MP5] = 0;
$ItemMax[sarmor3, PSG1] = 1;
$ItemMax[sarmor3, FiftyCal] = 1;
$ItemMax[sarmor3, LAW] = 0;
$ItemMax[sarmor3, AutoShotgun] = 0;
$ItemMax[sarmor3, Howitzer] = 0;
$ItemMax[sarmor3, Airstrike] = 0;
$ItemMax[sarmor3, Stinger] = 0;
$ItemMax[sarmor3, Flamethrower] = 0;

$ItemMax[sarmor3, BulletAmmo] = 100;
$ItemMax[sarmor3, PlasmaAmmo] = 30;
$ItemMax[sarmor3, DiscAmmo] = 15;
$ItemMax[sarmor3, GrenadeAmmo] = 10;
$ItemMax[sarmor3, MortarAmmo] = 10;
$ItemMax[sarmor3, SOCOMAmmo] = 15;
$ItemMax[sarmor3, OICWAmmo] = 0;
$ItemMax[sarmor3, SAWAmmo] = 0;
$ItemMax[sarmor3, MP5Ammo] = 0;
$ItemMax[sarmor3, PSG1Ammo] = 5;
$ItemMax[sarmor3, FiftyCalAmmo] = 5;
$ItemMax[sarmor3, LAWAmmo] = 0;
$ItemMax[sarmor3, AutoShotgunAmmo] = 0;
$ItemMax[sarmor3, HowitzerAmmo] = 0;
$ItemMax[sarmor3, StingerAmmo] = 0;
$ItemMax[sarmor3, FlameAmmo] = 0;

$ItemMax[sarmor3, SOCOMClip] = 1;
$ItemMax[sarmor3, OICWClip] = 0;
$ItemMax[sarmor3, SAWClip] = 0;
$ItemMax[sarmor3, MP5Clip] = 0;
$ItemMax[sarmor3, PSG1Clip] = 3;
$ItemMax[sarmor3, FiftyCalClip] = 2;
$ItemMax[sarmor3, AutoShotgunClip] = 0;
$ItemMax[sarmor3, StingerClip] = 0;

$ItemMax[sarmor3, EnergyPack] = 0;
$ItemMax[sarmor3, RepairPack] = 0;
$ItemMax[sarmor3, ShieldPack] = 0;
$ItemMax[sarmor3, SensorJammerPack] = 1;
$ItemMax[sarmor3, MotionSensorPack] = 0;
$ItemMax[sarmor3, PulseSensorPack] = 0;
$ItemMax[sarmor3, DeployableSensorJammerPack] = 0;
$ItemMax[sarmor3, DeployableHealthPack] = 0;
$ItemMax[sarmor3, CameraPack] = 0;
$ItemMax[sarmor3, TurretPack] = 0;
$ItemMax[sarmor3, AmmoPackSmall] = 1;
$ItemMax[sarmor3, AmmoPackHeavy] = 1;
$ItemMax[sarmor3, AmmoPackExp] = 0;
$ItemMax[sarmor3, RepairKit] = 1;
$ItemMax[sarmor3, DeployableInvPack] = 0;
$ItemMax[sarmor3, DeployableAmmoPack] = 0;
$ItemMax[sarmor3, TwentyPack] = 0;
$ItemMax[sarmor3, HowitzerPack] = 0;
$ItemMax[sarmor3, SAMPack] = 0;
$ItemMax[sarmor3, Charge] = 0;
$ItemMax[sarmor3, AirstrikePack] = 0;
$ItemMax[sarmor3, GrapplePack] = 0;
$ItemMax[sarmor3, GrappleHook] = 0;
$ItemMax[sarmor3, ReloaderPack] = 0;
$ItemMax[sarmor3, Parachute] = 1;
$ItemMax[sarmor3, FuelPack] = 0;
$ItemMax[sarmor3, PortGenPack] = 0;
$ItemMax[sarmor3, AAPack] = 0;
$ItemMax[sarmor3, MedicPack] = 0;

$MaxWeapons[sarmor3] = 1;

// INFANTRY

$DamageScale[iarmor3, $LandingDamageType] = 1.0;
$DamageScale[iarmor3, $ImpactDamageType] = 1.0;
$DamageScale[iarmor3, $CrushDamageType] = 1.0;
$DamageScale[iarmor3, $BulletDamageType] = 1.0;
$DamageScale[iarmor3, $PlasmaDamageType] = 0.6;
$DamageScale[iarmor3, $EnergyDamageType] = 1.0;
$DamageScale[iarmor3, $ExplosionDamageType] = 1.0;
$DamageScale[iarmor3, $MissileDamageType] = 1.0;
$DamageScale[iarmor3, $ShrapnelDamageType] = 1.0;
$DamageScale[iarmor3, $DebrisDamageType] = 1.0;
$DamageScale[iarmor3, $LaserDamageType] = 1.0;
$DamageScale[iarmor3, $MortarDamageType] = 1.0;
$DamageScale[iarmor3, $BlasterDamageType] = 1.0;
$DamageScale[iarmor3, $ElectricityDamageType] = 1.0;
$DamageScale[iarmor3, $MineDamageType] = 1.0;
$DamageScale[iarmor3, $ChargeDamageType] = 1.0;
$DamageScale[iarmor3, $AirstrikeDamageType] = 1.0;
$DamageScale[iarmor3, $BleedDamageType] = 1.0;
$DamageScale[iarmor3, $GrenadeDamageType] = 1.0;
$DamageScale[iarmor3, $PoisonDamageType] = 1.0;
$DamageScale[iarmor3, $SmokeDamageType] = 1.0;
$DamageScale[iarmor3, $StabDamageType] = 1.0;

$ItemMax[iarmor3, Blaster] = 1;
$ItemMax[iarmor3, Chaingun] = 1;
$ItemMax[iarmor3, Disclauncher] = 1;
$ItemMax[iarmor3, GrenadeLauncher] = 1;
$ItemMax[iarmor3, Mortar] = 0;
$ItemMax[iarmor3, PlasmaGun] = 1;
$ItemMax[iarmor3, LaserRifle] = 1;
$ItemMax[iarmor3, EnergyRifle] = 1;
$ItemMax[iarmor3, TargetingLaser] = 1;
$ItemMax[iarmor3, BouncingMinePack] = 0;
$ItemMax[iarmor3, APMinePack] = 1;
$ItemMax[iarmor3, AAMinePack] = 0;
$ItemMax[iarmor3, Grenade] = 3;
$ItemMax[iarmor3, Beacon]  = 3;
$ItemMax[iarmor3, Knife] = 1;
$ItemMax[iarmor3, SOCOM] = 1;
$ItemMax[iarmor3, OICW] = 1;
$ItemMax[iarmor3, SAW] = 1;
$ItemMax[iarmor3, MP5] = 0;
$ItemMax[iarmor3, PSG1] = 0;
$ItemMax[iarmor3, FiftyCal] = 0;
$ItemMax[iarmor3, LAW] = 1;
$ItemMax[iarmor3, AutoShotgun] = 1;
$ItemMax[iarmor3, Stinger] = 0;
$ItemMax[iarmor3, Flamethrower] = 1;
$ItemMax[iarmor3, Howitzer] = 0;
$ItemMax[iarmor3, Airstrike] = 0;

$ItemMax[iarmor3, BulletAmmo] = 100;
$ItemMax[iarmor3, PlasmaAmmo] = 30;
$ItemMax[iarmor3, DiscAmmo] = 15;
$ItemMax[iarmor3, GrenadeAmmo] = 10;
$ItemMax[iarmor3, MortarAmmo] = 10;
$ItemMax[iarmor3, SOCOMAmmo] = 15;
$ItemMax[iarmor3, OICWAmmo] = 30;
$ItemMax[iarmor3, SAWAmmo] = 200;
$ItemMax[iarmor3, MP5Ammo] = 0;
$ItemMax[iarmor3, PSG1Ammo] = 0;
$ItemMax[iarmor3, FiftyCalAmmo] = 0;
$ItemMax[iarmor3, LAWAmmo] = 1;
$ItemMax[iarmor3, AutoShotgunAmmo] = 7;
$ItemMax[iarmor3, FlameAmmo] = 150;
$ItemMax[iarmor3, StingerAmmo] = 0;
$ItemMax[iarmor3, HowitzerAmmo] = 0;

$ItemMax[iarmor3, SOCOMClip] = 1;
$ItemMax[iarmor3, OICWClip] = 2;
$ItemMax[iarmor3, SAWClip] = 1;
$ItemMax[iarmor3, MP5Clip] = 0;
$ItemMax[iarmor3, PSG1Clip] = 0;
$ItemMax[iarmor3, FiftyCalClip] = 0;
$ItemMax[iarmor3, AutoShotgunClip] = 5;
$ItemMax[iarmor3, StingerClip] = 0;

$ItemMax[iarmor3, EnergyPack] = 0;
$ItemMax[iarmor3, RepairPack] = 1;
$ItemMax[iarmor3, ShieldPack] = 0;
$ItemMax[iarmor3, SensorJammerPack] = 0;
$ItemMax[iarmor3, MotionSensorPack] = 0;
$ItemMax[iarmor3, PulseSensorPack] = 0;
$ItemMax[iarmor3, DeployableSensorJammerPack] = 0;
$ItemMax[iarmor3, DeployableHealthPack] = 0;
$ItemMax[iarmor3, CameraPack] = 0;
$ItemMax[iarmor3, TurretPack] = 0;
$ItemMax[iarmor3, AmmoPackSmall] = 1;
$ItemMax[iarmor3, AmmoPackHeavy] = 1;
$ItemMax[iarmor3, AmmoPackExp] = 1;
$ItemMax[iarmor3, RepairKit] = 1;
$ItemMax[iarmor3, DeployableInvPack] = 0;
$ItemMax[iarmor3, DeployableAmmoPack] = 0;
$ItemMax[iarmor3, TwentyPack] = 0;
$ItemMax[iarmor3, HowitzerPack] = 0;
$ItemMax[iarmor3, SAMPack] = 0;
$ItemMax[iarmor3, Charge] = 0;
$ItemMax[iarmor3, AirstrikePack] = 0;
$ItemMax[iarmor3, GrapplePack] = 0;
$ItemMax[iarmor3, GrappleHook] = 0;
$ItemMax[iarmor3, ReloaderPack] = 1;
$ItemMax[iarmor3, Parachute] = 1;
$ItemMax[iarmor3, FuelPack] = 1;
$ItemMax[iarmor3, PortGenPack] = 1;
$ItemMax[iarmor3, AAPack] = 0;
$ItemMax[iarmor3, MedicPack] = 0;

$MaxWeapons[iarmor3] = 2;

// Grenadier

$DamageScale[garmor3, $LandingDamageType] = 1.0;
$DamageScale[garmor3, $ImpactDamageType] = 1.0;
$DamageScale[garmor3, $CrushDamageType] = 1.0;
$DamageScale[garmor3, $BulletDamageType] = 1.0;
$DamageScale[garmor3, $PlasmaDamageType] = 0.6;
$DamageScale[garmor3, $EnergyDamageType] = 1.0;
$DamageScale[garmor3, $ExplosionDamageType] = 1.0;
$DamageScale[garmor3, $MissileDamageType] = 1.0;
$DamageScale[garmor3, $ShrapnelDamageType] = 0.8;
$DamageScale[garmor3, $DebrisDamageType] = 1.0;
$DamageScale[garmor3, $LaserDamageType] = 1.0;
$DamageScale[garmor3, $MortarDamageType] = 1.0;
$DamageScale[garmor3, $BlasterDamageType] = 1.0;
$DamageScale[garmor3, $ElectricityDamageType] = 1.0;
$DamageScale[garmor3, $MineDamageType] = 1.0;
$DamageScale[garmor3, $ChargeDamageType] = 1.0;
$DamageScale[garmor3, $AirstrikeDamageType] = 1.0;
$DamageScale[garmor3, $BleedDamageType] = 1.0;
$DamageScale[garmor3, $GrenadeDamageType] = 0.8;
$DamageScale[garmor3, $PoisonDamageType] = 1.0;
$DamageScale[garmor3, $SmokeDamageType] = 1.0;
$DamageScale[garmor3, $StabDamageType] = 1.0;

$ItemMax[garmor3, Blaster] = 1;
$ItemMax[garmor3, Chaingun] = 1;
$ItemMax[garmor3, Disclauncher] = 1;
$ItemMax[garmor3, GrenadeLauncher] = 1;
$ItemMax[garmor3, Mortar] = 0;
$ItemMax[garmor3, PlasmaGun] = 1;
$ItemMax[garmor3, LaserRifle] = 1;
$ItemMax[garmor3, EnergyRifle] = 1;
$ItemMax[garmor3, TargetingLaser] = 1;
$ItemMax[garmor3, BouncingMinePack] = 1;
$ItemMax[garmor3, APMinePack] = 1;
$ItemMax[garmor3, AAMinePack] = 0;
$ItemMax[garmor3, Grenade] = 10;
$ItemMax[garmor3, Beacon]  = 3;
$ItemMax[garmor3, Knife] = 1;
$ItemMax[garmor3, SOCOM] = 1;
$ItemMax[garmor3, OICW] = 1;
$ItemMax[garmor3, SAW] = 0;
$ItemMax[garmor3, MP5] = 0;
$ItemMax[garmor3, PSG1] = 0;
$ItemMax[garmor3, FiftyCal] = 0;
$ItemMax[garmor3, LAW] = 0;
$ItemMax[garmor3, AutoShotgun] = 0;
$ItemMax[garmor3, Stinger] = 0;
$ItemMax[garmor3, Flamethrower] = 0;
$ItemMax[garmor3, Howitzer] = 0;
$ItemMax[garmor3, Airstrike] = 0;

$ItemMax[garmor3, BulletAmmo] = 100;
$ItemMax[garmor3, PlasmaAmmo] = 30;
$ItemMax[garmor3, DiscAmmo] = 15;
$ItemMax[garmor3, GrenadeAmmo] = 10;
$ItemMax[garmor3, MortarAmmo] = 10;
$ItemMax[garmor3, SOCOMAmmo] = 15;
$ItemMax[garmor3, OICWAmmo] = 30;
$ItemMax[garmor3, SAWAmmo] = 0;
$ItemMax[garmor3, MP5Ammo] = 0;
$ItemMax[garmor3, PSG1Ammo] = 0;
$ItemMax[garmor3, FiftyCalAmmo] = 0;
$ItemMax[garmor3, LAWAmmo] = 0;
$ItemMax[garmor3, AutoShotgunAmmo] = 0;
$ItemMax[garmor3, FlameAmmo] = 0;
$ItemMax[garmor3, StingerAmmo] = 0;
$ItemMax[garmor3, HowitzerAmmo] = 0;

$ItemMax[garmor3, SOCOMClip] = 1;
$ItemMax[garmor3, OICWClip] = 1;
$ItemMax[garmor3, SAWClip] = 0;
$ItemMax[garmor3, MP5Clip] = 0;
$ItemMax[garmor3, PSG1Clip] = 0;
$ItemMax[garmor3, FiftyCalClip] = 0;
$ItemMax[garmor3, AutoShotgunClip] = 0;
$ItemMax[garmor3, StingerClip] = 0;

$ItemMax[garmor3, EnergyPack] = 0;
$ItemMax[garmor3, RepairPack] = 1;
$ItemMax[garmor3, ShieldPack] = 0;
$ItemMax[garmor3, SensorJammerPack] = 0;
$ItemMax[garmor3, MotionSensorPack] = 0;
$ItemMax[garmor3, PulseSensorPack] = 0;
$ItemMax[garmor3, DeployableSensorJammerPack] = 0;
$ItemMax[garmor3, DeployableHealthPack] = 0;
$ItemMax[garmor3, CameraPack] = 0;
$ItemMax[garmor3, TurretPack] = 0;
$ItemMax[garmor3, AmmoPackSmall] = 1;
$ItemMax[garmor3, AmmoPackHeavy] = 1;
$ItemMax[garmor3, AmmoPackExp] = 1;
$ItemMax[garmor3, RepairKit] = 1;
$ItemMax[garmor3, DeployableInvPack] = 0;
$ItemMax[garmor3, DeployableAmmoPack] = 0;
$ItemMax[garmor3, TwentyPack] = 0;
$ItemMax[garmor3, HowitzerPack] = 0;
$ItemMax[garmor3, SAMPack] = 0;
$ItemMax[garmor3, Charge] = 1;
$ItemMax[garmor3, AirstrikePack] = 0;
$ItemMax[garmor3, GrapplePack] = 0;
$ItemMax[garmor3, GrappleHook] = 0;
$ItemMax[garmor3, ReloaderPack] = 0;
$ItemMax[garmor3, Parachute] = 1;
$ItemMax[garmor3, FuelPack] = 0;
$ItemMax[garmor3, PortGenPack] = 0;
$ItemMax[garmor3, AAPack] = 0;
$ItemMax[garmor3, MedicPack] = 0;

$MaxWeapons[garmor3] = 1;

// SPECOPS

$DamageScale[carmor3, $LandingDamageType] = 1.0;
$DamageScale[carmor3, $ImpactDamageType] = 1.0;
$DamageScale[carmor3, $CrushDamageType] = 1.0;
$DamageScale[carmor3, $BulletDamageType] = 1.2;
$DamageScale[carmor3, $PlasmaDamageType] = 1.0;
$DamageScale[carmor3, $EnergyDamageType] = 1.3;
$DamageScale[carmor3, $ExplosionDamageType] = 1.0;
$DamageScale[carmor3, $MissileDamageType] = 1.0;
$DamageScale[carmor3, $DebrisDamageType] = 1.2;
$DamageScale[carmor3, $ShrapnelDamageType] = 1.2;
$DamageScale[carmor3, $LaserDamageType] = 1.0;
$DamageScale[carmor3, $MortarDamageType] = 1.3;
$DamageScale[carmor3, $BlasterDamageType] = 1.3;
$DamageScale[carmor3, $ElectricityDamageType] = 1.0;
$DamageScale[carmor3, $MineDamageType] = 1.2;
$DamageScale[carmor3, $ChargeDamageType] = 1.2;
$DamageScale[carmor3, $AirstrikeDamageType] = 1.0;
$DamageScale[carmor3, $BleedDamageType] = 1.0;
$DamageScale[carmor3, $GrenadeDamageType] = 1.0;
$DamageScale[carmor3, $PoisonDamageType] = 1.0;
$DamageScale[carmor3, $SmokeDamageType] = 1.0;
$DamageScale[carmor3, $StabDamageType] = 1.0;

$ItemMax[carmor3, Blaster] = 1;
$ItemMax[carmor3, Chaingun] = 1;
$ItemMax[carmor3, Disclauncher] = 1;
$ItemMax[carmor3, GrenadeLauncher] = 1;
$ItemMax[carmor3, Mortar] = 0;
$ItemMax[carmor3, PlasmaGun] = 1;
$ItemMax[carmor3, LaserRifle] = 1;
$ItemMax[carmor3, EnergyRifle] = 1;
$ItemMax[carmor3, TargetingLaser] = 1;
$ItemMax[carmor3, BouncingMinePack] = 0;
$ItemMax[carmor3, APMinePack] = 0;
$ItemMax[carmor3, AAMinePack] = 0;
$ItemMax[carmor3, Grenade] = 3;
$ItemMax[carmor3, Beacon]  = 3;
$ItemMax[carmor3, Knife] = 1;
$ItemMax[carmor3, SOCOM] = 1;
$ItemMax[carmor3, OICW] = 0;
$ItemMax[carmor3, SAW] = 0;
$ItemMax[carmor3, MP5] = 1;
$ItemMax[carmor3, PSG1] = 0;
$ItemMax[carmor3, FiftyCal] = 0;
$ItemMax[carmor3, LAW] = 0;
$ItemMax[carmor3, AutoShotgun] = 0;
$ItemMax[carmor3, Howitzer] = 0;
$ItemMax[carmor3, Airstrike] = 1;
$ItemMax[carmor3, Stinger] = 1;
$ItemMax[carmor3, Flamethrower] = 0;

$ItemMax[carmor3, BulletAmmo] = 100;
$ItemMax[carmor3, PlasmaAmmo] = 30;
$ItemMax[carmor3, DiscAmmo] = 15;
$ItemMax[carmor3, GrenadeAmmo] = 10;
$ItemMax[carmor3, MortarAmmo] = 10;
$ItemMax[carmor3, SOCOMAmmo] = 15;
$ItemMax[carmor3, OICWAmmo] = 0;
$ItemMax[carmor3, SAWAmmo] = 0;
$ItemMax[carmor3, MP5Ammo] = 30;
$ItemMax[carmor3, PSG1Ammo] = 0;
$ItemMax[carmor3, FiftyCalAmmo] = 0;
$ItemMax[carmor3, LAWAmmo] = 0;
$ItemMax[carmor3, AutoShotgunAmmo] = 0;
$ItemMax[carmor3, HowitzerAmmo] = 0;
$ItemMax[carmor3, StingerAmmo] = 1;
$ItemMax[carmor3, FlameAmmo] = 0;

$ItemMax[carmor, SOCOMClip] = 1;
$ItemMax[carmor, OICWClip] = 0;
$ItemMax[carmor, SAWClip] = 0;
$ItemMax[carmor, MP5Clip] = 2;
$ItemMax[carmor, PSG1Clip] = 0;
$ItemMax[carmor, FiftyCalClip] = 0;
$ItemMax[carmor, AutoShotgunClip] = 0;
$ItemMax[carmor, StingerClip] = 1;

$ItemMax[carmor3, EnergyPack] = 0;
$ItemMax[carmor3, RepairPack] = 0;
$ItemMax[carmor3, ShieldPack] = 0;
$ItemMax[carmor3, SensorJammerPack] = 1;
$ItemMax[carmor3, MotionSensorPack] = 0;
$ItemMax[carmor3, PulseSensorPack] = 0;
$ItemMax[carmor3, DeployableSensorJammerPack] = 0;
$ItemMax[carmor3, DeployableHealthPack] = 0;
$ItemMax[carmor3, CameraPack] = 1;
$ItemMax[carmor3, TurretPack] = 0;
$ItemMax[carmor3, AmmoPackSmall] = 1;
$ItemMax[carmor3, AmmoPackHeavy] = 0;
$ItemMax[carmor3, AmmoPackExp] = 1;
$ItemMax[carmor3, RepairKit] = 1;
$ItemMax[carmor3, DeployableInvPack] = 0;
$ItemMax[carmor3, DeployableAmmoPack] = 0;
$ItemMax[carmor3, TwentyPack] = 0;
$ItemMax[carmor3, HowitzerPack] = 0;
$ItemMax[carmor3, SAMPack] = 0;
$ItemMax[carmor3, Charge] = 1;
$ItemMax[carmor3, AirstrikePack] = 1;
$ItemMax[carmor3, GrapplePack] = 1;
$ItemMax[carmor3, GrappleHook] = 1;
$ItemMax[carmor3, ReloaderPack] = 1;
$ItemMax[carmor3, Parachute] = 1;
$ItemMax[carmor3, FuelPack] = 0;
$ItemMax[carmor3, PortGenPack] = 0;
$ItemMax[carmor3, AAPack] = 0;
$ItemMax[carmor3, MedicPack] = 0;

$MaxWeapons[carmor3] = 3;

// ENGINEER

$DamageScale[earmor3, $LandingDamageType] = 1.0;
$DamageScale[earmor3, $ImpactDamageType] = 1.0;
$DamageScale[earmor3, $CrushDamageType] = 1.0;
$DamageScale[earmor3, $BulletDamageType] = 1.0;
$DamageScale[earmor3, $PlasmaDamageType] = 0.6;
$DamageScale[earmor3, $EnergyDamageType] = 1.0;
$DamageScale[earmor3, $ExplosionDamageType] = 1.0;
$DamageScale[earmor3, $MissileDamageType] = 1.0;
$DamageScale[earmor3, $ShrapnelDamageType] = 1.0;
$DamageScale[earmor3, $DebrisDamageType] = 1.0;
$DamageScale[earmor3, $LaserDamageType] = 1.0;
$DamageScale[earmor3, $MortarDamageType] = 1.0;
$DamageScale[earmor3, $BlasterDamageType] = 1.0;
$DamageScale[earmor3, $ElectricityDamageType] = 1.0;
$DamageScale[earmor3, $MineDamageType] = 1.0;
$DamageScale[earmor3, $ChargeDamageType] = 1.0;
$DamageScale[earmor3, $AirstrikeDamageType] = 1.0;
$DamageScale[earmor3, $BleedDamageType] = 1.0;
$DamageScale[earmor3, $GrenadeDamageType] = 1.0;
$DamageScale[earmor3, $PoisonDamageType] = 1.0;
$DamageScale[earmor3, $SmokeDamageType] = 1.0;
$DamageScale[earmor3, $StabDamageType] = 1.0;

$ItemMax[earmor3, Blaster] = 1;
$ItemMax[earmor3, Chaingun] = 1;
$ItemMax[earmor3, Disclauncher] = 1;
$ItemMax[earmor3, GrenadeLauncher] = 1;
$ItemMax[earmor3, Mortar] = 0;
$ItemMax[earmor3, PlasmaGun] = 1;
$ItemMax[earmor3, LaserRifle] = 1;
$ItemMax[earmor3, EnergyRifle] = 1;
$ItemMax[earmor3, TargetingLaser] = 1;
$ItemMax[earmor3, BouncingMinePack] = 1;
$ItemMax[earmor3, APMinePack] = 1;
$ItemMax[earmor3, AAMinePack] = 1;
$ItemMax[earmor3, Grenade] = 2;
$ItemMax[earmor3, Beacon]  = 3;
$ItemMax[earmor3, Knife] = 1;
$ItemMax[earmor3, SOCOM] = 1;
$ItemMax[earmor3, OICW] = 1;
$ItemMax[earmor3, SAW] = 0;
$ItemMax[earmor3, MP5] = 0;
$ItemMax[earmor3, PSG1] = 0;
$ItemMax[earmor3, FiftyCal] = 0;
$ItemMax[earmor3, LAW] = 0;
$ItemMax[earmor3, AutoShotgun] = 1;
$ItemMax[earmor3, Howitzer] = 0;
$ItemMax[earmor3, Airstrike] = 0;
$ItemMax[earmor3, Stinger] = 0;
$ItemMax[earmor3, Flamethrower] = 0;

$ItemMax[earmor3, BulletAmmo] = 100;
$ItemMax[earmor3, PlasmaAmmo] = 30;
$ItemMax[earmor3, DiscAmmo] = 15;
$ItemMax[earmor3, GrenadeAmmo] = 10;
$ItemMax[earmor3, MortarAmmo] = 10;
$ItemMax[earmor3, SOCOMAmmo] = 15;
$ItemMax[earmor3, OICWAmmo] = 30;
$ItemMax[earmor3, SAWAmmo] = 0;
$ItemMax[earmor3, MP5Ammo] = 0;
$ItemMax[earmor3, PSG1Ammo] = 0;
$ItemMax[earmor3, FiftyCalAmmo] = 0;
$ItemMax[earmor3, LAWAmmo] = 0;
$ItemMax[earmor3, AutoShotgunAmmo] = 7;
$ItemMax[earmor3, HowitzerAmmo] = 0;
$ItemMax[earmor3, StingerAmmo] = 0;
$ItemMax[earmor3, FlameAmmo] = 0;

$ItemMax[earmor3, SOCOMClip] = 1;
$ItemMax[earmor3, OICWClip] = 1;
$ItemMax[earmor3, SAWClip] = 0;
$ItemMax[earmor3, MP5Clip] = 0;
$ItemMax[earmor3, PSG1Clip] = 0;
$ItemMax[earmor3, FiftyCalClip] = 0;
$ItemMax[earmor3, AutoShotgunClip] = 4;
$ItemMax[earmor3, StingerClip] = 0;

$ItemMax[earmor3, EnergyPack] = 0;
$ItemMax[earmor3, RepairPack] = 1;
$ItemMax[earmor3, ShieldPack] = 0;
$ItemMax[earmor3, SensorJammerPack] = 1;
$ItemMax[earmor3, MotionSensorPack] = 1;
$ItemMax[earmor3, PulseSensorPack] = 1;
$ItemMax[earmor3, DeployableSensorJammerPack] = 1;
$ItemMax[earmor3, DeployableHealthPack] = 0;
$ItemMax[earmor3, CameraPack] = 1;
$ItemMax[earmor3, TurretPack] = 1;
$ItemMax[earmor3, AmmoPackSmall] = 1;
$ItemMax[earmor3, AmmoPackHeavy] = 1;
$ItemMax[earmor3, AmmoPackExp] = 0;
$ItemMax[earmor3, RepairKit] = 1;
$ItemMax[earmor3, DeployableInvPack] = 1;
$ItemMax[earmor3, DeployableAmmoPack] = 1;
$ItemMax[earmor3, TwentyPack] = 1;
$ItemMax[earmor3, HowitzerPack] = 1;
$ItemMax[earmor3, SAMPack] = 1;
$ItemMax[earmor3, Charge] = 0;
$ItemMax[earmor3, AirstrikePack] = 0;
$ItemMax[earmor3, GrapplePack] = 0;
$ItemMax[earmor3, GrappleHook] = 0;
$ItemMax[earmor3, ReloaderPack] = 1;
$ItemMax[earmor3, Parachute] = 1;
$ItemMax[earmor3, FuelPack] = 0;
$ItemMax[earmor3, PortGenPack] = 1;
$ItemMax[earmor3, AAPack] = 1;
$ItemMax[earmor3, MedicPack] = 0;

$MaxWeapons[earmor3] = 2;

// PILOT

$DamageScale[larmor3, $LandingDamageType] = 1.0;
$DamageScale[larmor3, $ImpactDamageType] = 1.0;
$DamageScale[larmor3, $CrushDamageType] = 1.0;
$DamageScale[larmor3, $BulletDamageType] = 1.2;
$DamageScale[larmor3, $PlasmaDamageType] = 1.0;
$DamageScale[larmor3, $EnergyDamageType] = 1.3;
$DamageScale[larmor3, $ExplosionDamageType] = 1.0;
$DamageScale[larmor3, $MissileDamageType] = 1.0;
$DamageScale[larmor3, $DebrisDamageType] = 1.2;
$DamageScale[larmor3, $ShrapnelDamageType] = 1.2;
$DamageScale[larmor3, $LaserDamageType] = 1.0;
$DamageScale[larmor3, $MortarDamageType] = 1.3;
$DamageScale[larmor3, $BlasterDamageType] = 1.3;
$DamageScale[larmor3, $ElectricityDamageType] = 1.0;
$DamageScale[larmor3, $MineDamageType] = 1.2;
$DamageScale[larmor3, $ChargeDamageType] = 1.2;
$DamageScale[larmor3, $AirstrikeDamageType] = 1.0;
$DamageScale[larmor3, $BleedDamageType] = 1.0;
$DamageScale[larmor3, $GrenadeDamageType] = 1.0;
$DamageScale[larmor3, $PoisonDamageType] = 1.0;
$DamageScale[larmor3, $SmokeDamageType] = 1.0;
$DamageScale[larmor3, $StabDamageType] = 1.0;

$ItemMax[larmor3, Blaster] = 1;
$ItemMax[larmor3, Chaingun] = 1;
$ItemMax[larmor3, Disclauncher] = 1;
$ItemMax[larmor3, GrenadeLauncher] = 1;
$ItemMax[larmor3, Mortar] = 0;
$ItemMax[larmor3, PlasmaGun] = 1;
$ItemMax[larmor3, LaserRifle] = 1;
$ItemMax[larmor3, EnergyRifle] = 1;
$ItemMax[larmor3, TargetingLaser] = 1;
$ItemMax[larmor3, BouncingMinePack] = 0;
$ItemMax[larmor3, APMinePack] = 0;
$ItemMax[larmor3, AAMinePack] = 0;
$ItemMax[larmor3, Grenade] = 2;
$ItemMax[larmor3, Beacon]  = 3;
$ItemMax[larmor3, Knife] = 1;
$ItemMax[larmor3, SOCOM] = 1;
$ItemMax[larmor3, OICW] = 0;
$ItemMax[larmor3, SAW] = 0;
$ItemMax[larmor3, MP5] = 0;
$ItemMax[larmor3, PSG1] = 0;
$ItemMax[larmor3, FiftyCal] = 0;
$ItemMax[larmor3, LAW] = 0;
$ItemMax[larmor3, Howitzer] = 0;
$ItemMax[larmor3, AutoShotgun] = 0;
$ItemMax[larmor3, Airstrike] = 0;
$ItemMax[larmor3, Stinger] = 0;
$ItemMax[larmor3, Flamethrower] = 0;

$ItemMax[larmor3, BulletAmmo] = 100;
$ItemMax[larmor3, PlasmaAmmo] = 30;
$ItemMax[larmor3, DiscAmmo] = 15;
$ItemMax[larmor3, GrenadeAmmo] = 10;
$ItemMax[larmor3, MortarAmmo] = 10;
$ItemMax[larmor3, SOCOMAmmo] = 15;
$ItemMax[larmor3, OICWAmmo] = 0;
$ItemMax[larmor3, SAWAmmo] = 0;
$ItemMax[larmor3, MP5Ammo] = 0;
$ItemMax[larmor3, PSG1Ammo] = 0;
$ItemMax[larmor3, FiftyCalAmmo] = 0;
$ItemMax[larmor3, LAWAmmo] = 0;
$ItemMax[larmor3, AutoShotgun] = 0;
$ItemMax[larmor3, HowitzerAmmo] = 0;
$ItemMax[larmor3, StingerAmmo] = 0;
$ItemMax[larmor3, FlameAmmo] = 0;

$ItemMax[larmor, SOCOMClip] = 1;
$ItemMax[larmor, OICWClip] = 0;
$ItemMax[larmor, SAWClip] = 0;
$ItemMax[larmor, MP5Clip] = 0;
$ItemMax[larmor, PSG1Clip] = 0;
$ItemMax[larmor, FiftyCalClip] = 0;
$ItemMax[larmor, AutoShotgunClip] = 0;
$ItemMax[larmor, StingerClip] = 0;

$ItemMax[larmor3, EnergyPack] = 0;
$ItemMax[larmor3, RepairPack] = 0;
$ItemMax[larmor3, ShieldPack] = 0;
$ItemMax[larmor3, SensorJammerPack] = 0;
$ItemMax[larmor3, MotionSensorPack] = 0;
$ItemMax[larmor3, PulseSensorPack] = 0;
$ItemMax[larmor3, DeployableSensorJammerPack] = 0;
$ItemMax[larmor3, DeployableHealthPack] = 0;
$ItemMax[larmor3, CameraPack] = 0;
$ItemMax[larmor3, TurretPack] = 0;
$ItemMax[larmor3, AmmoPackSmall] = 1;
$ItemMax[larmor3, AmmoPackHeavy] = 0;
$ItemMax[larmor3, AmmoPackExp] = 0;
$ItemMax[larmor3, RepairKit] = 1;
$ItemMax[larmor3, DeployableInvPack] = 0;
$ItemMax[larmor3, DeployableAmmoPack] = 0;
$ItemMax[larmor3, TwentyPack] = 0;
$ItemMax[larmor3, HowitzerPack] = 0;
$ItemMax[larmor3, SAMPack] = 0;
$ItemMax[larmor3, Charge] = 0;
$ItemMax[larmor3, AirstrikePack] = 0;
$ItemMax[larmor3, GrapplePack] = 0;
$ItemMax[larmor3, GrappleHook] = 0;
$ItemMax[larmor3, ReloaderPack] = 0;
$ItemMax[larmor3, Parachute] = 1;
$ItemMax[larmor3, FuelPack] = 0;
$ItemMax[larmor3, PortGenPack] = 0;
$ItemMax[larmor3, AAPack] = 0;
$ItemMax[larmor3, MedicPack] = 0;

$MaxWeapons[larmor3] = 1;

// ARTILLERY

$DamageScale[aarmor3, $LandingDamageType] = 1.0;
$DamageScale[aarmor3, $ImpactDamageType] = 1.0;
$DamageScale[aarmor3, $CrushDamageType] = 1.0;
$DamageScale[aarmor3, $BulletDamageType] = 1.2;
$DamageScale[aarmor3, $PlasmaDamageType] = 1.0;
$DamageScale[aarmor3, $EnergyDamageType] = 1.3;
$DamageScale[aarmor3, $ExplosionDamageType] = 1.0;
$DamageScale[aarmor3, $MissileDamageType] = 1.0;
$DamageScale[aarmor3, $DebrisDamageType] = 1.2;
$DamageScale[aarmor3, $ShrapnelDamageType] = 1.2;
$DamageScale[aarmor3, $LaserDamageType] = 1.0;
$DamageScale[aarmor3, $MortarDamageType] = 1.3;
$DamageScale[aarmor3, $BlasterDamageType] = 1.3;
$DamageScale[aarmor3, $ElectricityDamageType] = 1.0;
$DamageScale[aarmor3, $MineDamageType] = 1.2;
$DamageScale[aarmor3, $ChargeDamageType] = 1.2;
$DamageScale[aarmor3, $AirstrikeDamageType] = 1.0;
$DamageScale[aarmor3, $BleedDamageType] = 1.0;
$DamageScale[aarmor3, $GrenadeDamageType] = 1.0;
$DamageScale[aarmor3, $PoisonDamageType] = 1.0;
$DamageScale[aarmor3, $SmokeDamageType] = 1.0;
$DamageScale[aarmor3, $StabDamageType] = 1.0;

//$DamageScale[aarmor, $LandingDamageType] = 1.0;
//$DamageScale[aarmor, $ImpactDamageType] = 1.0;
//$DamageScale[aarmor, $CrushDamageType] = 1.0;
//$DamageScale[aarmor, $BulletDamageType] = 0.6;
//$DamageScale[aarmor, $PlasmaDamageType] = 0.4;
//$DamageScale[aarmor, $EnergyDamageType] = 0.7;
//$DamageScale[aarmor, $ExplosionDamageType] = 0.6;
//$DamageScale[aarmor, $MissileDamageType] = 0.6;
//$DamageScale[aarmor, $DebrisDamageType] = 0.8;
//$DamageScale[aarmor, $ShrapnelDamageType] = 0.8;
//$DamageScale[aarmor, $LaserDamageType] = 0.6;
//$DamageScale[aarmor, $MortarDamageType] = 0.7;
//$DamageScale[aarmor, $BlasterDamageType] = 0.7;
//$DamageScale[aarmor, $ElectricityDamageType] = 1.0;
//$DamageScale[aarmor, $MineDamageType] = 0.8;
//$DamageScale[aarmor, $ChargeDamageType] = 0.8;
//$DamageScale[aarmor, $AirstrikeDamageType] = 1.0;

$ItemMax[aarmor3, Blaster] = 0;
$ItemMax[aarmor3, Chaingun] = 0;
$ItemMax[aarmor3, Disclauncher] = 0;
$ItemMax[aarmor3, GrenadeLauncher] = 0;
$ItemMax[aarmor3, Mortar] = 0;
$ItemMax[aarmor3, PlasmaGun] = 0;
$ItemMax[aarmor3, LaserRifle] = 0;
$ItemMax[aarmor3, EnergyRifle] = 0;
$ItemMax[aarmor3, TargetingLaser] = 1;
$ItemMax[aarmor3, BouncingMinePack] = 1;
$ItemMax[aarmor3, APMinePack] = 1;
$ItemMax[aarmor3, AAMinePack] = 1;
$ItemMax[aarmor3, Grenade] = 3;
$ItemMax[aarmor3, Beacon]  = 3;
$ItemMax[aarmor3, Knife] = 1;
$ItemMax[aarmor3, SOCOM] = 1;
$ItemMax[aarmor3, OICW] = 1;
$ItemMax[aarmor3, SAW] = 0;
$ItemMax[aarmor3, MP5] = 0;
$ItemMax[aarmor3, PSG1] = 0;
$ItemMax[aarmor3, FiftyCal] = 0;
$ItemMax[aarmor3, LAW] = 0;
$ItemMax[aarmor3, AutoShotgun] = 0;
$ItemMax[aarmor3, Howitzer] = 0;
$ItemMax[aarmor3, Airstrike] = 1;
$ItemMax[aarmor3, Stinger] = 0;
$ItemMax[aarmor3, Flamethrower] = 0;

$ItemMax[aarmor3, BulletAmmo] = 0;
$ItemMax[aarmor3, PlasmaAmmo] = 0;
$ItemMax[aarmor3, DiscAmmo] = 0;
$ItemMax[aarmor3, GrenadeAmmo] = 10;
$ItemMax[aarmor3, MortarAmmo] = 10;
$ItemMax[aarmor3, SOCOMAmmo] = 15;
$ItemMax[aarmor3, OICWAmmo] = 30;
$ItemMax[aarmor3, SAWAmmo] = 0;
$ItemMax[aarmor3, MP5Ammo] = 0;
$ItemMax[aarmor3, PSG1Ammo] = 0;
$ItemMax[aarmor3, FiftyCalAmmo] = 0;
$ItemMax[aarmor3, LAWAmmo] = 0;
$ItemMax[aarmor3, AutoShotgunAmmo] = 0;
$ItemMax[aarmor3, HowitzerAmmo] = 0;
$ItemMax[aarmor3, Stinger] = 0;
$ItemMax[aarmor3, FlameAmmo] = 0;

$ItemMax[iarmor3, SOCOMClip] = 1;
$ItemMax[iarmor3, OICWClip] = 1;
$ItemMax[iarmor3, SAWClip] = 0;
$ItemMax[iarmor3, MP5Clip] = 0;
$ItemMax[iarmor3, PSG1Clip] = 0;
$ItemMax[iarmor3, FiftyCalClip] = 0;
$ItemMax[iarmor3, AutoShotgunClip] = 0;
$ItemMax[iarmor3, StingerClip] = 0;

$ItemMax[aarmor3, EnergyPack] = 0;
$ItemMax[aarmor3, RepairPack] = 0;
$ItemMax[aarmor3, ShieldPack] = 0;
$ItemMax[aarmor3, SensorJammerPack] = 0;
$ItemMax[aarmor3, MotionSensorPack] = 0;
$ItemMax[aarmor3, PulseSensorPack] = 0;
$ItemMax[aarmor3, DeployableSensorJammerPack] = 0;
$ItemMax[aarmor3, DeployableHealthPack] = 0;
$ItemMax[aarmor3, CameraPack] = 0;
$ItemMax[aarmor3, TurretPack] = 0;
$ItemMax[aarmor3, AmmoPack] = 1;
$ItemMax[aarmor3, RepairKit] = 1;
$ItemMax[aarmor3, DeployableInvPack] = 0;
$ItemMax[aarmor3, DeployableAmmoPack] = 0;
$ItemMax[aarmor3, TwentyPack] = 0;
$ItemMax[aarmor3, HowitzerPack] = 1;
$ItemMax[aarmor3, SAMPack] = 0;
$ItemMax[aarmor3, Charge] = 0;
$ItemMax[aarmor3, AirstrikePack] = 1;
$ItemMax[aarmor3, GrapplePack] = 0;
$ItemMax[aarmor3, GrappleHook] = 0;
$ItemMax[aarmor3, ReloaderPack] = 1;
$ItemMax[aarmor3, Parachute] = 1;
$ItemMax[aarmor3, FuelPack] = 0;
$ItemMax[aarmor3, PortGenPack] = 0;
$ItemMax[aarmor3, AAPack] = 0;
$ItemMax[aarmor3, MedicPack] = 0;

$MaxWeapons[aarmor3] = 2;

// MEDIC FEMALE

$DamageScale[mfemale3, $LandingDamageType] = 1.0;
$DamageScale[mfemale3, $ImpactDamageType] = 1.0;
$DamageScale[mfemale3, $CrushDamageType] = 1.0;
$DamageScale[mfemale3, $BulletDamageType] = 1.2;
$DamageScale[mfemale3, $PlasmaDamageType] = 1.0;
$DamageScale[mfemale3, $EnergyDamageType] = 1.3;
$DamageScale[mfemale3, $ExplosionDamageType] = 1.0;
$DamageScale[mfemale3, $MissileDamageType] = 1.0;
$DamageScale[mfemale3, $DebrisDamageType] = 1.2;
$DamageScale[mfemale3, $ShrapnelDamageType] = 1.2;
$DamageScale[mfemale3, $LaserDamageType] = 1.0;
$DamageScale[mfemale3, $MortarDamageType] = 1.3;
$DamageScale[mfemale3, $BlasterDamageType] = 1.3;
$DamageScale[mfemale3, $ElectricityDamageType] = 1.0;
$DamageScale[mfemale3, $MineDamageType] = 1.2;
$DamageScale[mfemale3, $ChargeDamageType] = 1.2;
$DamageScale[mfemale3, $AirstrikeDamageType] = 1.0;
$DamageScale[mfemale3, $BleedDamageType] = 1.0;
$DamageScale[mfemale3, $GrenadeDamageType] = 1.0;
$DamageScale[mfemale3, $PoisonDamageType] = 1.0;
$DamageScale[mfemale3, $SmokeDamageType] = 1.0;
$DamageScale[mfemale3, $StabDamageType] = 1.0;

$ItemMax[mfemale3, Blaster] = 1;
$ItemMax[mfemale3, Chaingun] = 1;
$ItemMax[mfemale3, Disclauncher] = 1;
$ItemMax[mfemale3, GrenadeLauncher] = 1;
$ItemMax[mfemale3, Mortar] = 0;
$ItemMax[mfemale3, PlasmaGun] = 1;
$ItemMax[mfemale3, LaserRifle] = 1;
$ItemMax[mfemale3, EnergyRifle] = 1;
$ItemMax[mfemale3, TargetingLaser] = 1;
$ItemMax[mfemale3, BouncingMinePack] = 0;
$ItemMax[mfemale3, APMinePack] = 0;
$ItemMax[mfemale3, AAMinePack] = 0;
$ItemMax[mfemale3, Grenade] = 2;
$ItemMax[mfemale3, Beacon]  = 3;
$ItemMax[mfemale3, Knife] = 1;
$ItemMax[mfemale3, SOCOM] = 1;
$ItemMax[mfemale3, OICW] = 0;
$ItemMax[mfemale3, SAW] = 0;
$ItemMax[mfemale3, MP5] = 1;
$ItemMax[mfemale3, PSG1] = 0;
$ItemMax[mfemale3, FiftyCal] = 0;
$ItemMax[mfemale3, LAW] = 0;
$ItemMax[mfemale3, AutoShotgun] = 0;
$ItemMax[mfemale3, Howitzer] = 0;
$ItemMax[mfemale3, Airstrike] = 0;
$ItemMax[mfemale3, Stinger] = 0;
$ItemMax[mfemale3, Flamethrower] = 0;

$ItemMax[mfemale3, BulletAmmo] = 100;
$ItemMax[mfemale3, PlasmaAmmo] = 30;
$ItemMax[mfemale3, DiscAmmo] = 15;
$ItemMax[mfemale3, GrenadeAmmo] = 10;
$ItemMax[mfemale3, MortarAmmo] = 10;
$ItemMax[mfemale3, SOCOMAmmo] = 15;
$ItemMax[mfemale3, OICWAmmo] = 0;
$ItemMax[mfemale3, SAWAmmo] = 0;
$ItemMax[mfemale3, MP5Ammo] = 30;
$ItemMax[mfemale3, PSG1Ammo] = 0;
$ItemMax[mfemale3, FiftyCalAmmo] = 0;
$ItemMax[mfemale3, LAWAmmo] = 0;
$ItemMax[mfemale3, AutoShotgunAmmo] = 0;
$ItemMax[mfemale3, HowitzerAmmo] = 0;
$ItemMax[mfemale3, StingerAmmo] = 0;
$ItemMax[mfemale3, FlameAmmo] = 0;

$ItemMax[mfemale3, SOCOMClip] = 1;
$ItemMax[mfemale3, OICWClip] = 0;
$ItemMax[mfemale3, SAWClip] = 0;
$ItemMax[mfemale3, MP5Clip] = 1;
$ItemMax[mfemale3, PSG1Clip] = 0;
$ItemMax[mfemale3, FiftyCalClip] = 0;
$ItemMax[mfemale3, AutoShotgunClip] = 0;
$ItemMax[mfemale3, StingerClip] = 0;

$ItemMax[mfemale3, EnergyPack] = 0;
$ItemMax[mfemale3, RepairPack] = 0;
$ItemMax[mfemale3, ShieldPack] = 0;
$ItemMax[mfemale3, SensorJammerPack] = 0;
$ItemMax[mfemale3, MotionSensorPack] = 0;
$ItemMax[mfemale3, PulseSensorPack] = 0;
$ItemMax[mfemale3, DeployableSensorJammerPack] = 0;
$ItemMax[mfemale3, DeployableHealthPack] = 1;
$ItemMax[mfemale3, CameraPack] = 0;
$ItemMax[mfemale3, TurretPack] = 0;
$ItemMax[mfemale3, AmmoPackSmall] = 1;
$ItemMax[mfemale3, AmmoPackHeavy] = 0;
$ItemMax[mfemale3, AmmoPackExp] = 0;
$ItemMax[mfemale3, RepairKit] = 1;
$ItemMax[mfemale3, DeployableInvPack] = 0;
$ItemMax[mfemale3, DeployableAmmoPack] = 0;
$ItemMax[mfemale3, TwentyPack] = 0;
$ItemMax[mfemale3, HowitzerPack] = 0;
$ItemMax[mfemale3, SAMPack] = 0;
$ItemMax[mfemale3, Charge] = 0;
$ItemMax[mfemale3, AirstrikePack] = 0;
$ItemMax[mfemale3, GrapplePack] = 0;
$ItemMax[mfemale3, GrappleHook] = 0;
$ItemMax[mfemale3, ReloaderPack] = 0;
$ItemMax[mfemale3, Parachute] = 1;
$ItemMax[mfemale3, FuelPack] = 0;
$ItemMax[mfemale3, PortGenPack] = 0;
$ItemMax[mfemale3, AAPack] = 0;
$ItemMax[mfemale3, MedicPack] = 1;

$MaxWeapons[mfemale3] = 2;

// SNIPER FEMALE

$DamageScale[sfemale3, $LandingDamageType] = 1.0;
$DamageScale[sfemale3, $ImpactDamageType] = 1.0;	
$DamageScale[sfemale3, $CrushDamageType] = 1.0;	
$DamageScale[sfemale3, $BulletDamageType] = 1.2;
$DamageScale[sfemale3, $PlasmaDamageType] = 1.0;
$DamageScale[sfemale3, $EnergyDamageType] = 1.3;
$DamageScale[sfemale3, $ExplosionDamageType] = 1.0;
$DamageScale[sfemale3, $MissileDamageType] = 1.0;
$DamageScale[sfemale3, $ShrapnelDamageType] = 1.2;
$DamageScale[sfemale3, $DebrisDamageType] = 1.2;
$DamageScale[sfemale3, $LaserDamageType] = 1.0;
$DamageScale[sfemale3, $MortarDamageType] = 1.3;
$DamageScale[sfemale3, $BlasterDamageType] = 1.3;
$DamageScale[sfemale3, $ElectricityDamageType] = 1.0;
$DamageScale[sfemale3, $MineDamageType] = 1.2;
$DamageScale[sfemale3, $ChargeDamageType] = 1.2;
$DamageScale[sfemale3, $AirstrikeDamageType] = 1.0;
$DamageScale[sfemale3, $BleedDamageType] = 1.0;
$DamageScale[sfemale3, $GrenadeDamageType] = 1.0;
$DamageScale[sfemale3, $PoisonDamageType] = 1.0;
$DamageScale[sfemale3, $SmokeDamageType] = 1.0;
$DamageScale[sfemale3, $StabDamageType] = 1.0;

$ItemMax[sfemale3, Blaster] = 1;
$ItemMax[sfemale3, Chaingun] = 1;
$ItemMax[sfemale3, Disclauncher] = 1;
$ItemMax[sfemale3, GrenadeLauncher] = 1;
$ItemMax[sfemale3, Mortar] = 0;
$ItemMax[sfemale3, PlasmaGun] = 1;
$ItemMax[sfemale3, LaserRifle] = 1;
$ItemMax[sfemale3, EnergyRifle] = 1;
$ItemMax[sfemale3, TargetingLaser] = 1;
$ItemMax[sfemale3, BouncingMinePack] = 0;
$ItemMax[sfemale3, APMinePack] = 0;
$ItemMax[sfemale3, AAMinePack] = 0;
$ItemMax[sfemale3, Grenade] = 2;
$ItemMax[sfemale3, Beacon] = 3;
$ItemMax[sfemale3, Knife] = 1;
$ItemMax[sfemale3, SOCOM] = 1;
$ItemMax[sfemale3, OICW] = 0;
$ItemMax[sfemale3, SAW] = 0;
$ItemMax[sfemale3, MP5] = 0;
$ItemMax[sfemale3, PSG1] = 1;
$ItemMax[sfemale3, FiftyCal] = 1;
$ItemMax[sfemale3, LAW] = 0;
$ItemMax[sfemale3, AutoShotgun] = 0;
$ItemMax[sfemale3, Howitzer] = 0;
$ItemMax[sfemale3, Airstrike] = 0;
$ItemMax[sfemale3, Stinger] = 0;
$ItemMax[sfemale3, Flamethrower] = 0;

$ItemMax[sfemale3, BulletAmmo] = 100;
$ItemMax[sfemale3, PlasmaAmmo] = 30;
$ItemMax[sfemale3, DiscAmmo] = 15;
$ItemMax[sfemale3, GrenadeAmmo] = 10;
$ItemMax[sfemale3, MortarAmmo] = 10;
$ItemMax[sfemale3, SOCOMAmmo] = 15;
$ItemMax[sfemale3, OICWAmmo] = 0;
$ItemMax[sfemale3, SAWAmmo] = 0;
$ItemMax[sfemale3, MP5Ammo] = 0;
$ItemMax[sfemale3, PSG1Ammo] = 5;
$ItemMax[sfemale3, FiftyCalAmmo] = 5;
$ItemMax[sfemale3, LAWAmmo] = 0;
$ItemMax[sfemale3, AutoShotgunAmmo] = 0;
$ItemMax[sfemale3, HowitzerAmmo] = 0;
$ItemMax[sfemale3, StingerAmmo] = 0;
$ItemMax[sfemale3, FlameAmmo] = 0;

$ItemMax[sfemale3, SOCOMClip] = 1;
$ItemMax[sfemale3, OICWClip] = 0;
$ItemMax[sfemale3, SAWClip] = 0;
$ItemMax[sfemale3, MP5Clip] = 0;
$ItemMax[sfemale3, PSG1Clip] = 3;
$ItemMax[sfemale3, FiftyCalClip] = 2;
$ItemMax[sfemale3, AutoShotgunClip] = 0;
$ItemMax[sfemale3, StingerClip] = 0;

$ItemMax[sfemale3, EnergyPack] = 0;
$ItemMax[sfemale3, RepairPack] = 0;
$ItemMax[sfemale3, ShieldPack] = 0;
$ItemMax[sfemale3, SensorJammerPack] = 1;
$ItemMax[sfemale3, MotionSensorPack] = 0;
$ItemMax[sfemale3, PulseSensorPack] = 0;
$ItemMax[sfemale3, DeployableSensorJammerPack] = 0;
$ItemMax[sfemale3, DeployableHealthPack] = 0;
$ItemMax[sfemale3, CameraPack] = 0;
$ItemMax[sfemale3, TurretPack] = 0;
$ItemMax[sfemale3, AmmoPackSmall] = 1;
$ItemMax[sfemale3, AmmoPackHeavy] = 1;
$ItemMax[sfemale3, AmmoPackExp] = 0;
$ItemMax[sfemale3, RepairKit] = 1;
$ItemMax[sfemale3, DeployableInvPack] = 0;
$ItemMax[sfemale3, DeployableAmmoPack] = 0;
$ItemMax[sfemale3, TwentyPack] = 0;
$ItemMax[sfemale3, HowitzerPack] = 0;
$ItemMax[sfemale3, SAMPack] = 0;
$ItemMax[sfemale3, Charge] = 0;
$ItemMax[sfemale3, AirstrikePack] = 0;
$ItemMax[sfemale3, GrapplePack] = 0;
$ItemMax[sfemale3, GrappleHook] = 0;
$ItemMax[sfemale3, ReloaderPack] = 0;
$ItemMax[sfemale3, Parachute] = 1;
$ItemMax[sfemale3, FuelPack] = 0;
$ItemMax[sfemale3, PortGenPack] = 0;
$ItemMax[sfemale3, AAPack] = 0;
$ItemMax[sfemale3, MedicPack] = 0;

$MaxWeapons[sfemale3] = 1;

// INFANTRY FEMALE

$DamageScale[ifemale3, $LandingDamageType] = 1.0;
$DamageScale[ifemale3, $ImpactDamageType] = 1.0;
$DamageScale[ifemale3, $CrushDamageType] = 1.0;
$DamageScale[ifemale3, $BulletDamageType] = 1.0;
$DamageScale[ifemale3, $EnergyDamageType] = 1.0;
$DamageScale[ifemale3, $PlasmaDamageType] = 0.6;
$DamageScale[ifemale3, $ExplosionDamageType] = 1.0;
$DamageScale[ifemale3, $MissileDamageType] = 1.0;
$DamageScale[ifemale3, $ShrapnelDamageType] = 1.0;
$DamageScale[ifemale3, $DebrisDamageType] = 1.0;
$DamageScale[ifemale3, $LaserDamageType] = 1.0;
$DamageScale[ifemale3, $MortarDamageType] = 1.0;
$DamageScale[ifemale3, $BlasterDamageType] = 1.0;
$DamageScale[ifemale3, $ElectricityDamageType] = 1.0;
$DamageScale[ifemale3, $MineDamageType] = 1.0;
$DamageScale[ifemale3, $ChargeDamageType] = 1.0;
$DamageScale[ifemale3, $AirstrikeDamageType] = 1.0;
$DamageScale[ifemale3, $BleedDamageType] = 1.0;
$DamageScale[ifemale3, $GrenadeDamageType] = 1.0;
$DamageScale[ifemale3, $PoisonDamageType] = 1.0;
$DamageScale[ifemale3, $SmokeDamageType] = 1.0;
$DamageScale[ifemale3, $StabDamageType] = 1.0;

$ItemMax[ifemale3, Blaster] = 1;
$ItemMax[ifemale3, Chaingun] = 1;
$ItemMax[ifemale3, Disclauncher] = 1;
$ItemMax[ifemale3, GrenadeLauncher] = 1;
$ItemMax[ifemale3, Mortar] = 0;
$ItemMax[ifemale3, PlasmaGun] = 1;
$ItemMax[ifemale3, LaserRifle] = 0;
$ItemMax[ifemale3, EnergyRifle] = 1;
$ItemMax[ifemale3, TargetingLaser] = 1;
$ItemMax[ifemale3, BouncingMinePack] = 0;
$ItemMax[ifemale3, APMinePack] = 1;
$ItemMax[ifemale3, AAMinePack] = 0;
$ItemMax[ifemale3, Grenade] = 3;
$ItemMax[ifemale3, Beacon] = 3;
$ItemMax[ifemale3, Knife] = 1;
$ItemMax[ifemale3, SOCOM] = 1;
$ItemMax[ifemale3, OICW] = 1;
$ItemMax[ifemale3, SAW] = 1;
$ItemMax[ifemale3, MP5] = 0;
$ItemMax[ifemale3, PSG1] = 0;
$ItemMax[ifemale3, FiftyCal] = 0;
$ItemMax[ifemale3, LAW] = 1;
$ItemMax[ifemale3, AutoShotgun] = 1;
$ItemMax[ifemale3, Howitzer] = 0;
$ItemMax[ifemale3, Flamethrower] = 1;
$ItemMax[ifemale3, Airstrike] = 0;
$ItemMax[ifemale3, Stinger] = 0;

$ItemMax[ifemale3, BulletAmmo] = 150;
$ItemMax[ifemale3, PlasmaAmmo] = 40;
$ItemMax[ifemale3, DiscAmmo] = 15;
$ItemMax[ifemale3, GrenadeAmmo] = 10;
$ItemMax[ifemale3, MortarAmmo] = 10;
$ItemMax[ifemale3, SOCOMAmmo] = 15;
$ItemMax[ifemale3, OICWAmmo] = 30;
$ItemMax[ifemale3, SAWAmmo] = 200;
$ItemMax[ifemale3, MP5Ammo] = 0;
$ItemMax[ifemale3, PSG1Ammo] = 0;
$ItemMax[ifemale3, FiftyCalAmmo] = 0;
$ItemMax[ifemale3, LAWAmmo] = 1;
$ItemMax[ifemale3, AutoShotgunAmmo] = 7;
$ItemMax[ifemale3, HowitzerAmmo] = 0;
$ItemMax[ifemale3, StingerAmmo] = 0;
$ItemMax[ifemale3, FlameAmmo] = 150;

$ItemMax[ifemale3, SOCOMClip] = 1;
$ItemMax[ifemale3, OICWClip] = 2;
$ItemMax[ifemale3, SAWClip] = 1;
$ItemMax[ifemale3, MP5Clip] = 0;
$ItemMax[ifemale3, PSG1Clip] = 0;
$ItemMax[ifemale3, FiftyCalClip] = 0;
$ItemMax[ifemale3, AutoShotgunClip] = 5;
$ItemMax[ifemale3, StingerClip] = 0;

$ItemMax[ifemale3, EnergyPack] = 0;
$ItemMax[ifemale3, RepairPack] = 1;
$ItemMax[ifemale3, ShieldPack] = 0;
$ItemMax[ifemale3, SensorJammerPack] = 0;
$ItemMax[ifemale3, MotionSensorPack] = 0;
$ItemMax[ifemale3, PulseSensorPack] = 0;
$ItemMax[ifemale3, DeployableSensorJammerPack] = 0;
$ItemMax[ifemale3, DeployableHealthPack] = 0;
$ItemMax[ifemale3, CameraPack] = 1;
$ItemMax[ifemale3, TurretPack] = 0;
$ItemMax[ifemale3, AmmoPackSmall] = 1;
$ItemMax[ifemale3, AmmoPackHeavy] = 1;
$ItemMax[ifemale3, AmmoPackExp] = 1;
$ItemMax[ifemale3, RepairKit] = 1;
$ItemMax[ifemale3, DeployableInvPack] = 0;
$ItemMax[ifemale3, DeployableAmmoPack] = 0;
$ItemMax[ifemale3, TwentyPack] = 0;
$ItemMax[ifemale3, HowitzerPack] = 0;
$ItemMax[ifemale3, SAMPack] = 0;
$ItemMax[ifemale3, Charge] = 0;
$ItemMax[ifemale3, AirstrikePack] = 0;
$ItemMax[ifemale3, GrapplePack] = 0;
$ItemMax[ifemale3, GrappleHook] = 0;
$ItemMax[ifemale3, ReloaderPack] = 1;
$ItemMax[ifemale3, Parachute] = 1;
$ItemMax[ifemale3, FuelPack] = 1;
$ItemMax[ifemale3, PortGenPack] = 1;
$ItemMax[ifemale3, AAPack] = 0;
$ItemMax[ifemale3, MedicPack] = 0;

$MaxWeapons[ifemale3] = 2;

// Grenadier Female

$DamageScale[gfemale3, $LandingDamageType] = 1.0;
$DamageScale[gfemale3, $ImpactDamageType] = 1.0;
$DamageScale[gfemale3, $CrushDamageType] = 1.0;
$DamageScale[gfemale3, $BulletDamageType] = 1.0;
$DamageScale[gfemale3, $PlasmaDamageType] = 0.6;
$DamageScale[gfemale3, $EnergyDamageType] = 1.0;
$DamageScale[gfemale3, $ExplosionDamageType] = 1.0;
$DamageScale[gfemale3, $MissileDamageType] = 1.0;
$DamageScale[gfemale3, $ShrapnelDamageType] = 0.8;
$DamageScale[gfemale3, $DebrisDamageType] = 1.0;
$DamageScale[gfemale3, $LaserDamageType] = 1.0;
$DamageScale[gfemale3, $MortarDamageType] = 1.0;
$DamageScale[gfemale3, $BlasterDamageType] = 1.0;
$DamageScale[gfemale3, $ElectricityDamageType] = 1.0;
$DamageScale[gfemale3, $MineDamageType] = 1.0;
$DamageScale[gfemale3, $ChargeDamageType] = 1.0;
$DamageScale[gfemale3, $AirstrikeDamageType] = 1.0;
$DamageScale[gfemale3, $BleedDamageType] = 1.0;
$DamageScale[gfemale3, $GrenadeDamageType] = 0.8;
$DamageScale[gfemale3, $PoisonDamageType] = 1.0;
$DamageScale[gfemale3, $SmokeDamageType] = 1.0;
$DamageScale[gfemale3, $StabDamageType] = 1.0;

$ItemMax[gfemale3, Blaster] = 1;
$ItemMax[gfemale3, Chaingun] = 1;
$ItemMax[gfemale3, Disclauncher] = 1;
$ItemMax[gfemale3, GrenadeLauncher] = 1;
$ItemMax[gfemale3, Mortar] = 0;
$ItemMax[gfemale3, PlasmaGun] = 1;
$ItemMax[gfemale3, LaserRifle] = 1;
$ItemMax[gfemale3, EnergyRifle] = 1;
$ItemMax[gfemale3, TargetingLaser] = 1;
$ItemMax[gfemale3, BouncingMinePack] = 1;
$ItemMax[gfemale3, APMinePack] = 1;
$ItemMax[gfemale3, AAMinePack] = 0;
$ItemMax[gfemale3, Grenade] = 10;
$ItemMax[gfemale3, Beacon]  = 3;
$ItemMax[gfemale3, Knife] = 1;
$ItemMax[gfemale3, SOCOM] = 1;
$ItemMax[gfemale3, OICW] = 1;
$ItemMax[gfemale3, SAW] = 0;
$ItemMax[gfemale3, MP5] = 0;
$ItemMax[gfemale3, PSG1] = 0;
$ItemMax[gfemale3, FiftyCal] = 0;
$ItemMax[gfemale3, LAW] = 0;
$ItemMax[gfemale3, AutoShotgun] = 0;
$ItemMax[gfemale3, Stinger] = 0;
$ItemMax[gfemale3, Flamethrower] = 0;
$ItemMax[gfemale3, Howitzer] = 0;
$ItemMax[gfemale3, Airstrike] = 0;

$ItemMax[gfemale3, BulletAmmo] = 100;
$ItemMax[gfemale3, PlasmaAmmo] = 30;
$ItemMax[gfemale3, DiscAmmo] = 15;
$ItemMax[gfemale3, GrenadeAmmo] = 10;
$ItemMax[gfemale3, MortarAmmo] = 10;
$ItemMax[gfemale3, SOCOMAmmo] = 15;
$ItemMax[gfemale3, OICWAmmo] = 30;
$ItemMax[gfemale3, SAWAmmo] = 0;
$ItemMax[gfemale3, MP5Ammo] = 0;
$ItemMax[gfemale3, PSG1Ammo] = 0;
$ItemMax[gfemale3, FiftyCalAmmo] = 0;
$ItemMax[gfemale3, LAWAmmo] = 0;
$ItemMax[gfemale3, AutoShotgunAmmo] = 0;
$ItemMax[gfemale3, FlameAmmo] = 0;
$ItemMax[gfemale3, StingerAmmo] = 0;
$ItemMax[gfemale3, HowitzerAmmo] = 0;

$ItemMax[gfemale3, SOCOMClip] = 1;
$ItemMax[gfemale3, OICWClip] = 1;
$ItemMax[gfemale3, SAWClip] = 0;
$ItemMax[gfemale3, MP5Clip] = 0;
$ItemMax[gfemale3, PSG1Clip] = 0;
$ItemMax[gfemale3, FiftyCalClip] = 0;
$ItemMax[gfemale3, AutoShotgunClip] = 0;
$ItemMax[gfemale3, StingerClip] = 0;

$ItemMax[gfemale3, EnergyPack] = 0;
$ItemMax[gfemale3, RepairPack] = 1;
$ItemMax[gfemale3, ShieldPack] = 0;
$ItemMax[gfemale3, SensorJammerPack] = 0;
$ItemMax[gfemale3, MotionSensorPack] = 0;
$ItemMax[gfemale3, PulseSensorPack] = 0;
$ItemMax[gfemale3, DeployableSensorJammerPack] = 0;
$ItemMax[gfemale3, DeployableHealthPack] = 0;
$ItemMax[gfemale3, CameraPack] = 0;
$ItemMax[gfemale3, TurretPack] = 0;
$ItemMax[gfemale3, AmmoPackSmall] = 1;
$ItemMax[gfemale3, AmmoPackHeavy] = 1;
$ItemMax[gfemale3, AmmoPackExp] = 1;
$ItemMax[gfemale3, RepairKit] = 1;
$ItemMax[gfemale3, DeployableInvPack] = 0;
$ItemMax[gfemale3, DeployableAmmoPack] = 0;
$ItemMax[gfemale3, TwentyPack] = 0;
$ItemMax[gfemale3, HowitzerPack] = 0;
$ItemMax[gfemale3, SAMPack] = 0;
$ItemMax[gfemale3, Charge] = 1;
$ItemMax[gfemale3, AirstrikePack] = 0;
$ItemMax[gfemale3, GrapplePack] = 0;
$ItemMax[gfemale3, GrappleHook] = 0;
$ItemMax[gfemale3, ReloaderPack] = 0;
$ItemMax[gfemale3, Parachute] = 1;
$ItemMax[gfemale3, FuelPack] = 0;
$ItemMax[gfemale3, PortGenPack] = 0;
$ItemMax[gfemale3, AAPack] = 0;
$ItemMax[gfemale3, MedicPack] = 0;

$MaxWeapons[gfemale3] = 1;

// SPECOPS FEMALE

$DamageScale[cfemale3, $LandingDamageType] = 1.0;
$DamageScale[cfemale3, $ImpactDamageType] = 1.0;	
$DamageScale[cfemale3, $CrushDamageType] = 1.0;	
$DamageScale[cfemale3, $BulletDamageType] = 1.2;
$DamageScale[cfemale3, $PlasmaDamageType] = 1.0;
$DamageScale[cfemale3, $EnergyDamageType] = 1.3;
$DamageScale[cfemale3, $ExplosionDamageType] = 1.0;
$DamageScale[cfemale3, $MissileDamageType] = 1.0;
$DamageScale[cfemale3, $ShrapnelDamageType] = 1.2;
$DamageScale[cfemale3, $DebrisDamageType] = 1.2;
$DamageScale[cfemale3, $LaserDamageType] = 1.0;
$DamageScale[cfemale3, $MortarDamageType] = 1.3;
$DamageScale[cfemale3, $BlasterDamageType] = 1.3;
$DamageScale[cfemale3, $ElectricityDamageType] = 1.0;
$DamageScale[cfemale3, $MineDamageType] = 1.2;
$DamageScale[cfemale3, $ChargeDamageType] = 1.2;
$DamageScale[cfemale3, $AirstrikeDamageType] = 1.0;
$DamageScale[cfemale3, $BleedDamageType] = 1.0;
$DamageScale[cfemale3, $GrenadeDamageType] = 1.0;
$DamageScale[cfemale3, $PoisonDamageType] = 1.0;
$DamageScale[cfemale3, $SmokeDamageType] = 1.0;
$DamageScale[cfemale3, $StabDamageType] = 1.0;

$ItemMax[cfemale3, Blaster] = 1;
$ItemMax[cfemale3, Chaingun] = 1;
$ItemMax[cfemale3, Disclauncher] = 1;
$ItemMax[cfemale3, GrenadeLauncher] = 1;
$ItemMax[cfemale3, Mortar] = 0;
$ItemMax[cfemale3, PlasmaGun] = 1;
$ItemMax[cfemale3, LaserRifle] = 1;
$ItemMax[cfemale3, EnergyRifle] = 1;
$ItemMax[cfemale3, TargetingLaser] = 1;
$ItemMax[cfemale3, BouncingMinePack] = 0;
$ItemMax[cfemale3, APMinePack] = 0;
$ItemMax[cfemale3, AAMinePack] = 0;
$ItemMax[cfemale3, Grenade] = 3;
$ItemMax[cfemale3, Beacon] = 3;
$ItemMax[cfemale3, Knife] = 1;
$ItemMax[cfemale3, SOCOM] = 1;
$ItemMax[cfemale3, OICW] = 0;
$ItemMax[cfemale3, SAW] = 0;
$ItemMax[cfemale3, MP5] = 1;
$ItemMax[cfemale3, PSG1] = 0;
$ItemMax[cfemale3, FiftyCal] = 0;
$ItemMax[cfemale3, LAW] = 0;
$ItemMax[cfemale3, AutoShotgun] = 0;
$ItemMax[cfemale3, Howitzer] = 0;
$ItemMax[cfemale3, Airstrike] = 1;
$ItemMax[cfemale3, Stinger] = 1;
$ItemMax[cfemale3, Flamethrower] = 0;

$ItemMax[cfemale3, BulletAmmo] = 100;
$ItemMax[cfemale3, PlasmaAmmo] = 30;
$ItemMax[cfemale3, DiscAmmo] = 15;
$ItemMax[cfemale3, GrenadeAmmo] = 10;
$ItemMax[cfemale3, MortarAmmo] = 10;
$ItemMax[cfemale3, SOCOMAmmo] = 15;
$ItemMax[cfemale3, OICWAmmo] = 0;
$ItemMax[cfemale3, SAWAmmo] = 0;
$ItemMax[cfemale3, MP5Ammo] = 30;
$ItemMax[cfemale3, PSG1Ammo] = 0;
$ItemMax[cfemale3, FiftyCalAmmo] = 0;
$ItemMax[cfemale3, LAWAmmo] = 0;
$ItemMax[cfemale3, AutoShotgunAmmo] = 0;
$ItemMax[cfemale3, HowitzerAmmo] = 0;
$ItemMax[cfemale3, StingerAmmo] = 1;
$ItemMax[cfemale3, FlameAmmo] = 0;

$ItemMax[cfemale3, SOCOMClip] = 1;
$ItemMax[cfemale3, OICWClip] = 0;
$ItemMax[cfemale3, SAWClip] = 0;
$ItemMax[cfemale3, MP5Clip] = 2;
$ItemMax[cfemale3, PSG1Clip] = 0;
$ItemMax[cfemale3, FiftyCalClip] = 0;
$ItemMax[cfemale3, AutoShotgunClip] = 0;
$ItemMax[cfemale3, StingerClip] = 1;

$ItemMax[cfemale3, EnergyPack] = 0;
$ItemMax[cfemale3, RepairPack] = 0;
$ItemMax[cfemale3, ShieldPack] = 0;
$ItemMax[cfemale3, SensorJammerPack] = 1;
$ItemMax[cfemale3, MotionSensorPack] = 0;
$ItemMax[cfemale3, PulseSensorPack] = 0;
$ItemMax[cfemale3, DeployableSensorJammerPack] = 1;
$ItemMax[cfemale3, DeployableHealthPack] = 0;
$ItemMax[cfemale3, CameraPack] = 1;
$ItemMax[cfemale3, TurretPack] = 0;
$ItemMax[cfemale3, AmmoPackSmall] = 1;
$ItemMax[cfemale3, AmmoPackHeavy] = 0;
$ItemMax[cfemale3, AmmoPackExp] = 1;
$ItemMax[cfemale3, RepairKit] = 1;
$ItemMax[cfemale3, DeployableInvPack] = 0;
$ItemMax[cfemale3, DeployableAmmoPack] = 0;
$ItemMax[cfemale3, TwentyPack] = 0;
$ItemMax[cfemale3, HowitzerPack] = 0;
$ItemMax[cfemale3, SAMPack] = 0;
$ItemMax[cfemale3, Charge] = 1;
$ItemMax[cfemale3, AirstrikePack] = 1;
$ItemMax[cfemale3, GrapplePack] = 1;
$ItemMax[cfemale3, GrappleHook] = 1;
$ItemMax[cfemale3, ReloaderPack] = 1;
$ItemMax[cfemale3, Parachute] = 1;
$ItemMax[cfemale3, FuelPack] = 0;
$ItemMax[cfemale3, PortGenPack] = 0;
$ItemMax[cfemale3, AAPack] = 0;
$ItemMax[cfemale3, MedicPack] = 0;

$MaxWeapons[cfemale3] = 3;

// ENGINEER FEMALE

$DamageScale[efemale3, $LandingDamageType] = 1.0;
$DamageScale[efemale3, $ImpactDamageType] = 1.0;
$DamageScale[efemale3, $CrushDamageType] = 1.0;
$DamageScale[efemale3, $BulletDamageType] = 1.0;
$DamageScale[efemale3, $EnergyDamageType] = 1.0;
$DamageScale[efemale3, $PlasmaDamageType] = 0.6;
$DamageScale[efemale3, $ExplosionDamageType] = 1.0;
$DamageScale[efemale3, $MissileDamageType] = 1.0;
$DamageScale[efemale3, $ShrapnelDamageType] = 1.0;
$DamageScale[efemale3, $DebrisDamageType] = 1.0;
$DamageScale[efemale3, $LaserDamageType] = 1.0;
$DamageScale[efemale3, $MortarDamageType] = 1.0;
$DamageScale[efemale3, $BlasterDamageType] = 1.0;
$DamageScale[efemale3, $ElectricityDamageType] = 1.0;
$DamageScale[efemale3, $MineDamageType] = 1.0;
$DamageScale[efemale3, $ChargeDamageType] = 1.0;
$DamageScale[efemale3, $AirstrikeDamageType] = 1.0;
$DamageScale[efemale3, $BleedDamageType] = 1.0;
$DamageScale[efemale3, $GrenadeDamageType] = 1.0;
$DamageScale[efemale3, $PoisonDamageType] = 1.0;
$DamageScale[efemale3, $SmokeDamageType] = 1.0;
$DamageScale[efemale3, $StabDamageType] = 1.0;

$ItemMax[efemale3, Blaster] = 1;
$ItemMax[efemale3, Chaingun] = 1;
$ItemMax[efemale3, Disclauncher] = 1;
$ItemMax[efemale3, GrenadeLauncher] = 1;
$ItemMax[efemale3, Mortar] = 0;
$ItemMax[efemale3, PlasmaGun] = 1;
$ItemMax[efemale3, LaserRifle] = 0;
$ItemMax[efemale3, EnergyRifle] = 1;
$ItemMax[efemale3, TargetingLaser] = 1;
$ItemMax[efemale3, BouncingMinePack] = 1;
$ItemMax[efemale3, APMinePack] = 1;
$ItemMax[efemale3, AAMinePack] = 1;
$ItemMax[efemale3, Grenade] = 2;
$ItemMax[efemale3, Beacon] = 3;
$ItemMax[efemale3, Knife] = 1;
$ItemMax[efemale3, SOCOM] = 1;
$ItemMax[efemale3, OICW] = 1;
$ItemMax[efemale3, SAW] = 0;
$ItemMax[efemale3, MP5] = 0;
$ItemMax[efemale3, PSG1] = 0;
$ItemMax[efemale3, FiftyCal] = 0;
$ItemMax[efemale3, LAW] = 0;
$ItemMax[efemale3, AutoShotgun] = 1;
$ItemMax[efemale3, Howitzer] = 0;
$ItemMax[efemale3, Airstrike] = 0;
$ItemMax[efemale3, Stinger] = 0;
$ItemMax[efemale3, Flamethrower] = 0;

$ItemMax[efemale3, BulletAmmo] = 150;
$ItemMax[efemale3, PlasmaAmmo] = 40;
$ItemMax[efemale3, DiscAmmo] = 15;
$ItemMax[efemale3, GrenadeAmmo] = 10;
$ItemMax[efemale3, MortarAmmo] = 10;
$ItemMax[efemale3, SOCOMAmmo] = 15;
$ItemMax[efemale3, OICWAmmo] = 30;
$ItemMax[efemale3, SAWAmmo] = 0;
$ItemMax[efemale3, MP5Ammo] = 0;
$ItemMax[efemale3, PSG1Ammo] = 0;
$ItemMax[efemale3, FiftyCalAmmo] = 0;
$ItemMax[efemale3, LAWAmmo] = 0;
$ItemMax[efemale3, AutoShotgunAmmo] = 7;
$ItemMax[efemale3, HowitzerAmmo] = 0;
$ItemMax[efemale3, StingerAmmo] = 0;
$ItemMax[efemale3, FlameAmmo] = 0;

$ItemMax[efemale, SOCOMClip] = 1;
$ItemMax[efemale, OICWClip] = 1;
$ItemMax[efemale, SAWClip] = 0;
$ItemMax[efemale, MP5Clip] = 0;
$ItemMax[efemale, PSG1Clip] = 0;
$ItemMax[efemale, FiftyCalClip] = 0;
$ItemMax[efemale, AutoShotgunClip] = 4;
$ItemMax[efemale, StingerClip] = 0;

$ItemMax[efemale3, EnergyPack] = 0;
$ItemMax[efemale3, RepairPack] = 1;
$ItemMax[efemale3, ShieldPack] = 0;
$ItemMax[efemale3, SensorJammerPack] = 1;
$ItemMax[efemale3, MotionSensorPack] = 1;
$ItemMax[efemale3, PulseSensorPack] = 1;
$ItemMax[efemale3, DeployableSensorJammerPack] = 1;
$ItemMax[efemale3, DeployableHealthPack] = 0;
$ItemMax[efemale3, CameraPack] = 1;
$ItemMax[efemale3, TurretPack] = 1;
$ItemMax[efemale3, AmmoPackSmall] = 1;
$ItemMax[efemale3, AmmoPackHeavy] = 1;
$ItemMax[efemale3, AmmoPackExp] = 0;
$ItemMax[efemale3, RepairKit] = 1;
$ItemMax[efemale3, DeployableInvPack] = 1;
$ItemMax[efemale3, DeployableAmmoPack] = 1;
$ItemMax[efemale3, TwentyPack] = 1;
$ItemMax[efemale3, HowitzerPack] = 1;
$ItemMax[efemale3, SAMPack] = 1;
$ItemMax[efemale3, Charge] = 0;
$ItemMax[efemale3, AirstrikePack] = 0;
$ItemMax[efemale3, GrapplePack] = 0;
$ItemMax[efemale3, GrappleHook] = 0;
$ItemMax[efemale3, ReloaderPack] = 1;
$ItemMax[efemale3, Parachute] = 1;
$ItemMax[efemale3, FuelPack] = 0;
$ItemMax[efemale3, PortGenPack] = 1;
$ItemMax[efemale3, AAPack] = 1;
$ItemMax[efemale3, MedicPack] = 0;

$MaxWeapons[efemale3] = 2;

// PILOT FEMALE

$DamageScale[lfemale3, $LandingDamageType] = 1.0;
$DamageScale[lfemale3, $ImpactDamageType] = 1.0;	
$DamageScale[lfemale3, $CrushDamageType] = 1.0;	
$DamageScale[lfemale3, $BulletDamageType] = 1.2;
$DamageScale[lfemale3, $PlasmaDamageType] = 1.0;
$DamageScale[lfemale3, $EnergyDamageType] = 1.3;
$DamageScale[lfemale3, $ExplosionDamageType] = 1.0;
$DamageScale[lfemale3, $MissileDamageType] = 1.0;
$DamageScale[lfemale3, $ShrapnelDamageType] = 1.2;
$DamageScale[lfemale3, $DebrisDamageType] = 1.2;
$DamageScale[lfemale3, $LaserDamageType] = 1.0;
$DamageScale[lfemale3, $MortarDamageType] = 1.3;
$DamageScale[lfemale3, $BlasterDamageType] = 1.3;
$DamageScale[lfemale3, $ElectricityDamageType] = 1.0;
$DamageScale[lfemale3, $MineDamageType] = 1.2;
$DamageScale[lfemale3, $ChargeDamageType] = 1.2;
$DamageScale[lfemale3, $AirstrikeDamageType] = 1.0;
$DamageScale[lfemale3, $BleedDamageType] = 1.0;
$DamageScale[lfemale3, $GrenadeDamageType] = 1.0;
$DamageScale[lfemale3, $PoisonDamageType] = 1.0;
$DamageScale[lfemale3, $SmokeDamageType] = 1.0;
$DamageScale[lfemale3, $StabDamageType] = 1.0;

$ItemMax[lfemale3, Blaster] = 1;
$ItemMax[lfemale3, Chaingun] = 1;
$ItemMax[lfemale3, Disclauncher] = 1;
$ItemMax[lfemale3, GrenadeLauncher] = 1;
$ItemMax[lfemale3, Mortar] = 0;
$ItemMax[lfemale3, PlasmaGun] = 1;
$ItemMax[lfemale3, LaserRifle] = 1;
$ItemMax[lfemale3, EnergyRifle] = 1;
$ItemMax[lfemale3, TargetingLaser] = 1;
$ItemMax[lfemale3, BouncingMinePack] = 0;
$ItemMax[lfemale3, APMinePack] = 0;
$ItemMax[lfemale3, AAMinePack] = 0;
$ItemMax[lfemale3, Grenade] = 2;
$ItemMax[lfemale3, Beacon] = 3;
$ItemMax[lfemale3, Knife] = 1;
$ItemMax[lfemale3, SOCOM] = 1;
$ItemMax[lfemale3, OICW] = 0;
$ItemMax[lfemale3, SAW] = 0;
$ItemMax[lfemale3, MP5] = 0;
$ItemMax[lfemale3, PSG1] = 0;
$ItemMax[lfemale3, FiftyCal] = 0;
$ItemMax[lfemale3, LAW] = 0;
$ItemMax[lfemale3, AutoShotgun] = 0;
$ItemMax[lfemale3, Howitzer] = 0;
$ItemMax[lfemale3, Airstrike] = 0;
$ItemMax[lfemale3, Stinger] = 0;
$ItemMax[lfemale3, Flamethrower] = 0;

$ItemMax[lfemale3, BulletAmmo] = 100;
$ItemMax[lfemale3, PlasmaAmmo] = 30;
$ItemMax[lfemale3, DiscAmmo] = 15;
$ItemMax[lfemale3, GrenadeAmmo] = 10;
$ItemMax[lfemale3, MortarAmmo] = 10;
$ItemMax[lfemale3, SOCOMAmmo] = 15;
$ItemMax[lfemale3, OICWAmmo] = 0;
$ItemMax[lfemale3, SAWAmmo] = 0;
$ItemMax[lfemale3, MP5Ammo] = 0;
$ItemMax[lfemale3, PSG1Ammo] = 0;
$ItemMax[lfemale3, FiftyCalAmmo] = 0;
$ItemMax[lfemale3, LAWAmmo] = 0;
$ItemMax[lfemale3, AutoShotgunAmmo] = 0;
$ItemMax[lfemale3, HowitzerAmmo] = 0;
$ItemMax[lfemale3, StingerAmmo] = 0;
$ItemMax[lfemale3, FlameAmmo] = 0;

$ItemMax[lfemale3, SOCOMClip] = 1;
$ItemMax[lfemale3, OICWClip] = 0;
$ItemMax[lfemale3, SAWClip] = 0;
$ItemMax[lfemale3, MP5Clip] = 0;
$ItemMax[lfemale3, PSG1Clip] = 0;
$ItemMax[lfemale3, FiftyCalClip] = 0;
$ItemMax[lfemale3, AutoShotgunClip] = 0;
$ItemMax[lfemale3, StingerClip] = 0;

$ItemMax[lfemale3, EnergyPack] = 0;
$ItemMax[lfemale3, RepairPack] = 0;
$ItemMax[lfemale3, ShieldPack] = 0;
$ItemMax[lfemale3, SensorJammerPack] = 0;
$ItemMax[lfemale3, MotionSensorPack] = 0;
$ItemMax[lfemale3, PulseSensorPack] = 0;
$ItemMax[lfemale3, DeployableSensorJammerPack] = 0;
$ItemMax[lfemale3, DeployableHealthPack] = 0;
$ItemMax[lfemale3, CameraPack] = 0;
$ItemMax[lfemale3, TurretPack] = 0;
$ItemMax[lfemale3, AmmoPackSmall] = 1;
$ItemMax[lfemale3, AmmoPackHeavy] = 0;
$ItemMax[lfemale3, AmmoPackExp] = 0;
$ItemMax[lfemale3, RepairKit] = 1;
$ItemMax[lfemale3, DeployableInvPack] = 0;
$ItemMax[lfemale3, DeployableAmmoPack] = 0;
$ItemMax[lfemale3, TwentyPack] = 0;
$ItemMax[lfemale3, HowitzerPack] = 0;
$ItemMax[lfemale3, SAMPack] = 0;
$ItemMax[lfemale3, Charge] = 0;
$ItemMax[lfemale3, AirstrikePack] = 0;
$ItemMax[lfemale3, GrapplePack] = 0;
$ItemMax[lfemale3, GrappleHook] = 0;
$ItemMax[lfemale3, ReloaderPack] = 0;
$ItemMax[lfemale3, Parachute] = 1;
$ItemMax[lfemale3, FuelPack] = 0;
$ItemMax[lfemale3, PortGenPack] = 0;
$ItemMax[lfemale3, AAPack] = 0;
$ItemMax[lfemale3, MedicPack] = 0;

$MaxWeapons[lfemale3] = 1;

// ARTILLERY FEMALE

$DamageScale[afemale3, $LandingDamageType] = 1.0;
$DamageScale[afemale3, $ImpactDamageType] = 1.0;
$DamageScale[afemale3, $CrushDamageType] = 1.0;
$DamageScale[afemale3, $BulletDamageType] = 1.2;
$DamageScale[afemale3, $PlasmaDamageType] = 1.0;
$DamageScale[afemale3, $EnergyDamageType] = 1.3;
$DamageScale[afemale3, $ExplosionDamageType] = 1.0;
$DamageScale[afemale3, $MissileDamageType] = 1.0;
$DamageScale[afemale3, $DebrisDamageType] = 1.2;
$DamageScale[afemale3, $ShrapnelDamageType] = 1.2;
$DamageScale[afemale3, $LaserDamageType] = 1.0;
$DamageScale[afemale3, $MortarDamageType] = 1.3;
$DamageScale[afemale3, $BlasterDamageType] = 1.3;
$DamageScale[afemale3, $ElectricityDamageType] = 1.0;
$DamageScale[afemale3, $MineDamageType] = 1.2;
$DamageScale[afemale3, $ChargeDamageType] = 1.2;
$DamageScale[afemale3, $AirstrikeDamageType] = 1.0;
$DamageScale[afemale3, $BleedDamageType] = 1.0;
$DamageScale[afemale3, $GrenadeDamageType] = 1.0;
$DamageScale[afemale3, $PoisonDamageType] = 1.0;
$DamageScale[afemale3, $SmokeDamageType] = 1.0;
$DamageScale[afemale3, $StabDamageType] = 1.0;

//$DamageScale[afemale, $LandingDamageType] = 1.0;
//$DamageScale[afemale, $ImpactDamageType] = 1.0;
//$DamageScale[afemale, $CrushDamageType] = 1.0;
//$DamageScale[afemale, $BulletDamageType] = 0.6;
//$DamageScale[afemale, $PlasmaDamageType] = 0.4;
//$DamageScale[afemale, $EnergyDamageType] = 0.7;
//$DamageScale[afemale, $ExplosionDamageType] = 0.6;
//$DamageScale[afemale, $MissileDamageType] = 0.6;
//$DamageScale[afemale, $DebrisDamageType] = 0.8;
//$DamageScale[afemale, $ShrapnelDamageType] = 0.8;
//$DamageScale[afemale, $LaserDamageType] = 0.6;
//$DamageScale[afemale, $MortarDamageType] = 0.7;
//$DamageScale[afemale, $BlasterDamageType] = 0.7;
//$DamageScale[afemale, $ElectricityDamageType] = 1.0;
//$DamageScale[afemale, $MineDamageType] = 0.8;
//$DamageScale[afemale, $ChargeDamageType] = 0.8;
//$DamageScale[afemale, $AirstrikeDamageType] = 1.0;

$ItemMax[afemale3, Blaster] = 0;
$ItemMax[afemale3, Chaingun] = 0;
$ItemMax[afemale3, Disclauncher] = 0;
$ItemMax[afemale3, GrenadeLauncher] = 0;
$ItemMax[afemale3, Mortar] = 0;
$ItemMax[afemale3, PlasmaGun] = 0;
$ItemMax[afemale3, LaserRifle] = 0;
$ItemMax[afemale3, EnergyRifle] = 0;
$ItemMax[afemale3, TargetingLaser] = 1;
$ItemMax[afemale3, BouncingMinePack] = 1;
$ItemMax[afemale3, APMinePack] = 1;
$ItemMax[afemale3, AAMinePack] = 1;
$ItemMax[afemale3, Grenade] = 3;
$ItemMax[afemale3, Beacon]  = 3;
$ItemMax[afemale3, Knife] = 1;
$ItemMax[afemale3, SOCOM] = 1;
$ItemMax[afemale3, OICW] = 1;
$ItemMax[afemale3, SAW] = 0;
$ItemMax[afemale3, MP5] = 0;
$ItemMax[afemale3, PSG1] = 0;
$ItemMax[afemale3, FiftyCal] = 0;
$ItemMax[afemale3, LAW] = 0;
$ItemMax[afemale3, AutoShotgun] = 0;
$ItemMax[afemale3, Howitzer] = 0;
$ItemMax[afemale3, Airstrike] = 1;
$ItemMax[afemale3, Stinger] = 0;
$ItemMax[afemale3, Flamethrower] = 0;

$ItemMax[afemale3, BulletAmmo] = 0;
$ItemMax[afemale3, PlasmaAmmo] = 0;
$ItemMax[afemale3, DiscAmmo] = 0;
$ItemMax[afemale3, GrenadeAmmo] = 10;
$ItemMax[afemale3, MortarAmmo] = 10;
$ItemMax[afemale3, SOCOMAmmo] = 15;
$ItemMax[afemale3, OICWAmmo] = 30;
$ItemMax[afemale3, SAWAmmo] = 0;
$ItemMax[afemale3, MP5Ammo] = 0;
$ItemMax[afemale3, PSG1Ammo] = 0;
$ItemMax[afemale3, FiftyCalAmmo] = 0;
$ItemMax[afemale3, LAWAmmo] = 0;
$ItemMax[afemale3, AutoShotgunAmmo] = 0;
$ItemMax[afemale3, HowitzerAmmo] = 0;
$ItemMax[afemale3, Stinger] = 0;
$ItemMax[afemale3, FlameAmmo] = 0;

$ItemMax[afemale3, SOCOMClip] = 1;
$ItemMax[afemale3, OICWClip] = 1;
$ItemMax[afemale3, SAWClip] = 0;
$ItemMax[afemale3, MP5Clip] = 0;
$ItemMax[afemale3, PSG1Clip] = 0;
$ItemMax[afemale3, FiftyCalClip] = 0;
$ItemMax[afemale3, AutoShotgunClip] = 0;
$ItemMax[afemale3, StingerClip] = 0;

$ItemMax[afemale3, EnergyPack] = 0;
$ItemMax[afemale3, RepairPack] = 0;
$ItemMax[afemale3, ShieldPack] = 0;
$ItemMax[afemale3, SensorJammerPack] = 0;
$ItemMax[afemale3, MotionSensorPack] = 0;
$ItemMax[afemale3, PulseSensorPack] = 0;
$ItemMax[afemale3, DeployableSensorJammerPack] = 0;
$ItemMax[afemale3, DeployableHealthPack] = 0;
$ItemMax[afemale3, CameraPack] = 0;
$ItemMax[afemale3, TurretPack] = 0;
$ItemMax[afemale3, AmmoPack] = 1;
$ItemMax[afemale3, RepairKit] = 1;
$ItemMax[afemale3, DeployableInvPack] = 0;
$ItemMax[afemale3, DeployableAmmoPack] = 0;
$ItemMax[afemale3, TwentyPack] = 0;
$ItemMax[afemale3, HowitzerPack] = 1;
$ItemMax[afemale3, SAMPack] = 0;
$ItemMax[afemale3, Charge] = 0;
$ItemMax[afemale3, AirstrikePack] = 1;
$ItemMax[afemale3, GrapplePack] = 0;
$ItemMax[afemale3, GrappleHook] = 0;
$ItemMax[afemale3, ReloaderPack] = 1;
$ItemMax[afemale3, Parachute] = 1;
$ItemMax[afemale3, FuelPack] = 0;
$ItemMax[afemale3, PortGenPack] = 0;
$ItemMax[afemale3, AAPack] = 0;
$ItemMax[afemale3, MedicPack] = 0;

$MaxWeapons[afemale3] = 2;


// MEDIC

PlayerData marmor3
{
   className = "Armor";
   shapeFile = "larmor";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
   canCrouch = true;

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0.0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 1;
   maxBackwardSpeed = 1;
   maxSideSpeed = 1;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, direction
	// firstPerson, chaseCam, thirdPerson, signalThread
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
   animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 


    // celebration animations:
   animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
    // taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
    // poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;
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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// SNIPER

PlayerData sarmor3
{
   className = "Armor";
   shapeFile = "larmor";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
   canCrouch = true;

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0.0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 1;
   maxBackwardSpeed = 1;
   maxSideSpeed = 1;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, direction
	// firstPerson, chaseCam, thirdPerson, signalThread
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
   animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 


    // celebration animations:
   animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
    // taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
    // poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;
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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// INFANTRY

PlayerData iarmor3
{
   className = "Armor";
   shapeFile = "marmor";
   flameShapeName = "mflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   cancrouch = false;
   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 1.0;
   maxForwardSpeed = 1;
   maxBackwardSpeed = 1;
   maxSideSpeed = 1;
   groundForce = 35 * 13.0;
   mass = 13.0;
   groundtraction = 3.0;
	
	maxEnergy = 80;
   drag = 1.0;
   density = 1.5;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;

   rFootSounds = 
   {
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSnow,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft
  }; 
   lFootSounds =
   {
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSnow,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft
   };

   footPrints = { 2, 3 };

   boxWidth = 0.4;
   boxDepth = 0.4;
   boxNormalHeight = 2.25;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.20;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// Grenadier

PlayerData garmor3
{
   className = "Armor";
   shapeFile = "marmor";
   flameShapeName = "mflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   cancrouch = false;
   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 1.0;
   maxForwardSpeed = 1;
   maxBackwardSpeed = 1;
   maxSideSpeed = 1;
   groundForce = 35 * 13.0;
   mass = 13.0;
   groundtraction = 3.0;
	
	maxEnergy = 80;
   drag = 1.0;
   density = 1.5;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;

   rFootSounds = 
   {
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSnow,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft
  }; 
   lFootSounds =
   {
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSnow,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft
   };

   footPrints = { 2, 3 };

   boxWidth = 0.4;
   boxDepth = 0.4;
   boxNormalHeight = 2.25;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.20;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// SPECOPS

PlayerData carmor3
{
   className = "Armor";
   shapeFile = "larmor";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   shadowDetailMask = 1;

   visibleToSensor = False;
	mapFilter = 1;
	mapIcon = "M_player";
   canCrouch = true;

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 1;
   maxBackwardSpeed = 1;
   maxSideSpeed = 1;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, direction
	// firstPerson, chaseCam, thirdPerson, signalThread
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 


    // celebration animations:
   animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
    // taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
    // poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;
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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// ENGINEER

PlayerData earmor3
{
   className = "Armor";
   shapeFile = "marmor";
   flameShapeName = "mflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   cancrouch = false;
   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 1.0;
   maxForwardSpeed = 1;
   maxBackwardSpeed = 1;
   maxSideSpeed = 1;
   groundForce = 35 * 13.0;
   mass = 13.0;
   groundtraction = 3.0;
	
	maxEnergy = 80;
   drag = 1.0;
   density = 1.5;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;

   rFootSounds = 
   {
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSnow,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft
  }; 
   lFootSounds =
   {
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSnow,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft
   };

   footPrints = { 2, 3 };

   boxWidth = 0.4;
   boxDepth = 0.4;
   boxNormalHeight = 2.25;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.20;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// PILOT

PlayerData larmor3
{
   className = "Armor";
   shapeFile = "larmor";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
   canCrouch = true;

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 1;
   maxBackwardSpeed = 1;
   maxSideSpeed = 1;
    groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, direction
	// firstPerson, chaseCam, thirdPerson, signalThread
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
   animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 


    // celebration animations:
   animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
    // taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
    // poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;
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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// ARTILLERY

PlayerData aarmor3
{
   className = "Armor";
   shapeFile = "larmor";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
   cancrouch = false;

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0.0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 1;
   maxBackwardSpeed = 1;
   maxSideSpeed = 1;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, direction
	// firstPerson, chaseCam, thirdPerson, signalThread
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
   animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 


    // celebration animations:
   animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
 
    // taunt animations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
 
    // poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;
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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// MEDIC FEMALE

PlayerData mfemale3
{
   className = "Armor";
   shapeFile = "lfemale";
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   canCrouch = true;
   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 1;
   maxBackwardSpeed = 1;
   maxSideSpeed = 1;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };


   //jetSound = SoundJetLight;

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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// SNIPER FEMALE

PlayerData sfemale3
{
   className = "Armor";
   shapeFile = "lfemale";
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   canCrouch = true;
   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 1;
   maxBackwardSpeed = 1;
   maxSideSpeed = 1;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };


   //jetSound = SoundJetLight;

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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// INFANTRY FEMALE

PlayerData ifemale3
{
   className = "Armor";
   shapeFile = "mfemale";
   flameShapeName = "mflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

   cancrouch = false;
	maxDamage = 1.0;
   maxForwardSpeed = 1.0;
   maxBackwardSpeed = 1.0;
   maxSideSpeed = 1.0;
   groundForce = 35 * 13.0;
   mass = 13.0;
   groundtraction = 3.0;
	maxEnergy = 80;
   mass = 13.0;
   drag = 1.0;
   density = 1.5;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   animData[0]  = { "root", none, 1, true, true, true, false, 0 };
   animData[1]  = { "run", none, 1, true, false, true, false, 3 };
   animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
   animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
   animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
   animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
   animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
   animData[7] = { "crouch root", none, 1, true, false, true, false, 3 };
   animData[8] = { "crouch root", none, 1, true, false, true, false, 3 };
   animData[9] = { "crouch root", none, -1, true, false, true, false, 3 };
   animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
   animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
   animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
   animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
   animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
   animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
   animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
   animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
   animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;

   rFootSounds = 
   {
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSnow,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft
  }; 
   lFootSounds =
   {
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSnow,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft
   };

   footPrints = { 2, 3 };

   boxWidth = 0.4;
   boxDepth = 0.4;
   boxNormalHeight = 2.25;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.20;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// Grenadier FEMALE

PlayerData gfemale3
{
   className = "Armor";
   shapeFile = "mfemale";
   flameShapeName = "mflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

   cancrouch = false;
	maxDamage = 1.0;
   maxForwardSpeed = 1.0;
   maxBackwardSpeed = 1.0;
   maxSideSpeed = 1.0;
   groundForce = 35 * 13.0;
   mass = 13.0;
   groundtraction = 3.0;
	maxEnergy = 80;
   mass = 13.0;
   drag = 1.0;
   density = 1.5;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   animData[0]  = { "root", none, 1, true, true, true, false, 0 };
   animData[1]  = { "run", none, 1, true, false, true, false, 3 };
   animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
   animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
   animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
   animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
   animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
   animData[7] = { "crouch root", none, 1, true, false, true, false, 3 };
   animData[8] = { "crouch root", none, 1, true, false, true, false, 3 };
   animData[9] = { "crouch root", none, -1, true, false, true, false, 3 };
   animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
   animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
   animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
   animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
   animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
   animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
   animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
   animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
   animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;

   rFootSounds = 
   {
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSnow,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft
  }; 
   lFootSounds =
   {
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSnow,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft
   };

   footPrints = { 2, 3 };

   boxWidth = 0.4;
   boxDepth = 0.4;
   boxNormalHeight = 2.25;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.20;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// SPECOPS FEMALE

PlayerData cfemale3
{
   className = "Armor";
   shapeFile = "lfemale";
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = False;
	mapFilter = 1;
	mapIcon = "M_player";

   canCrouch = true;
   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 1;
   maxBackwardSpeed = 1;
   maxSideSpeed = 1;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };


   //jetSound = SoundJetLight;

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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// ENGINEER FEMALE

PlayerData efemale3
{
   className = "Armor";
   shapeFile = "mfemale";
   flameShapeName = "mflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

   cancrouch = false;
	maxDamage = 1.0;
   maxForwardSpeed = 1.0;
   maxBackwardSpeed = 1.0;
   maxSideSpeed = 1.0;
   groundForce = 35 * 13.0;
   mass = 13.0;
   groundtraction = 3.0;
	maxEnergy = 80;
   mass = 13.0;
   drag = 1.0;
   density = 1.5;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   animData[0]  = { "root", none, 1, true, true, true, false, 0 };
   animData[1]  = { "run", none, 1, true, false, true, false, 3 };
   animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
   animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
   animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
   animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
   animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
   animData[7] = { "crouch root", none, 1, true, false, true, false, 3 };
   animData[8] = { "crouch root", none, 1, true, false, true, false, 3 };
   animData[9] = { "crouch root", none, -1, true, false, true, false, 3 };
   animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
   animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
   animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
   animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
   animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
   animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
   animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
   animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
   animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };

   //jetSound = SoundJetLight;

   rFootSounds = 
   {
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRHard,
     SoundMFootRSnow,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft,
     SoundMFootRSoft
  }; 
   lFootSounds =
   {
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLHard,
      SoundMFootLSnow,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft,
      SoundMFootLSoft
   };

   footPrints = { 2, 3 };

   boxWidth = 0.4;
   boxDepth = 0.4;
   boxNormalHeight = 2.25;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.20;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// PILOT FEMALE

PlayerData lfemale3
{
   className = "Armor";
   shapeFile = "lfemale";
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   canCrouch = true;
   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 1;
   maxBackwardSpeed = 1;
   maxSideSpeed = 1;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };


   //jetSound = SoundJetLight;

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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// ARTILLERY FEMALE

PlayerData afemale3
{
   className = "Armor";
   shapeFile = "lfemale";
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   cancrouch = false;
   maxJetSideForceFactor = 0.0;
   maxJetForwardVelocity = 0;
   minJetEnergy = 200;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 1;
   maxBackwardSpeed = 1;
   maxSideSpeed = 1;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundtraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 12;
	damageScale = 0.01;

   jumpImpulse = 0;
   jumpSurfaceMinDot = 0.2;

   // animation data:
   // animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread

   // movement animations:
   // movement animations:
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
   //animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[19] = { "run", none, 1, true, false, true, false, 3 };

   // misc. animations:
   animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
   animData[21] = { "throw", none, 1, true, false, false, false, 3 };
   animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
   animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
   animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
   
   // death animations:
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

   // signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
   animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
   animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
   animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
   animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 

    // celebraton animations:
   animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
   animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
   animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };

    // taunt anmations:
   animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
   animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };

    // poses:
   animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
   animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };

	// Bonus wave
   animData[50] = { "wave", none, 1, true, false, false, true, 1 };


   //jetSound = SoundJetLight;

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

   boxWidth = 0.35;
   boxDepth = 0.35;
   boxNormalHeight = 2.05;
   boxCrouchHeight = 1.5;

   boxNormalHeadPercentage  = 0.90;
   boxNormalTorsoPercentage = 0.25;
   boxCrouchHeadPercentage  = 0.80;
   boxCrouchTorsoPercentage = 0.30;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};

// END DELTA FORCE ARMOR
// ----------------------------------------------------

//---Sandbags. i.e. im lazy.

$ItemMax[marmor,	SandBagpack] = 0;		     $ItemMax[mfemale,	SandBagpack] = 0;
$ItemMax[sarmor,	SandBagpack] = 0;		     $ItemMax[sfemale,	SandBagpack] = 0;
$ItemMax[iarmor,	SandBagpack] = 0;		     $ItemMax[ifemale,	SandBagpack] = 0;
$ItemMax[garmor,	SandBagpack] = 0;		     $ItemMax[gfemale,	SandBagpack] = 0;
$ItemMax[earmor,	SandBagpack] = 1;		     $ItemMax[efemale,	SandBagpack] = 1;
$ItemMax[carmor,	SandBagpack] = 0;		     $ItemMax[cfemale,	SandBagpack] = 0;
$ItemMax[larmor,	SandBagpack] = 0;		     $ItemMax[lfemale,	SandBagpack] = 0;
$ItemMax[aarmor,	SandBagpack] = 0;		     $ItemMax[afemale,	SandBagpack] = 0;

$ItemMax[marmor2,	SandBagpack] = 0;		     $ItemMax[mfemale2,	SandBagpack] = 0;
$ItemMax[sarmor2,	SandBagpack] = 0;		     $ItemMax[sfemale2,	SandBagpack] = 0;
$ItemMax[iarmor2,	SandBagpack] = 0;		     $ItemMax[ifemale2,	SandBagpack] = 0;
$ItemMax[garmor2,	SandBagpack] = 0;		     $ItemMax[gfemale2,	SandBagpack] = 0;
$ItemMax[earmor2,	SandBagpack] = 1;		     $ItemMax[efemale2,	SandBagpack] = 1;
$ItemMax[carmor2,	SandBagpack] = 0;		     $ItemMax[cfemale2,	SandBagpack] = 0;
$ItemMax[larmor2,	SandBagpack] = 0;		     $ItemMax[lfemale2,	SandBagpack] = 0;
$ItemMax[aarmor2,	SandBagpack] = 0;		     $ItemMax[afemale2,	SandBagpack] = 0;

$ItemMax[marmor3,	SandBagpack] = 0;		     $ItemMax[mfemale3,	SandBagpack] = 0;
$ItemMax[sarmor3,	SandBagpack] = 0;		     $ItemMax[sfemale3,	SandBagpack] = 0;
$ItemMax[iarmor3,	SandBagpack] = 0;		     $ItemMax[ifemale3,	SandBagpack] = 0;
$ItemMax[garmor3,	SandBagpack] = 0;		     $ItemMax[gfemale3,	SandBagpack] = 0;
$ItemMax[earmor3,	SandBagpack] = 1;		     $ItemMax[efemale3,	SandBagpack] = 1;
$ItemMax[carmor3,	SandBagpack] = 0;		     $ItemMax[cfemale3,	SandBagpack] = 0;
$ItemMax[larmor3,	SandBagpack] = 0;		     $ItemMax[lfemale3,	SandBagpack] = 0;
$ItemMax[aarmor3,	SandBagpack] = 0;		     $ItemMax[afemale3,	SandBagpack] = 0;



//----------------------------------------------------==================Module ItemMaxes for all armor

$ItemMax[marmor,	SideWinderModule] = 0;		     $ItemMax[mfemale,	SideWinderModule] = 0;
$ItemMax[marmor,	PhoenixModule] = 0;		     $ItemMax[mfemale,	PhoenixModule] = 0;
$ItemMax[marmor,	HellFireModule] = 0;		     $ItemMax[mfemale,	HellFireModule] = 0;
$ItemMax[marmor,	HydraModule] = 0;		     $ItemMax[mfemale,	HydraModule] = 0;
$ItemMax[marmor,	MaverickModule] = 0;		     $ItemMax[mfemale,	MaverickModule] = 0;
$ItemMax[marmor,	HARMModule] = 0;		     $ItemMax[mfemale,	HARMModule] = 0;
$ItemMax[marmor,	Mk82Module] = 0;		     $ItemMax[mfemale,	Mk82Module] = 0;
$ItemMax[marmor,	Mk84Module] = 0;		     $ItemMax[mfemale,	Mk84Module] = 0;
$ItemMax[marmor,	Mk82PackageModule] = 0;		     $ItemMax[mfemale,	Mk82PackageModule] = 0;
$ItemMax[marmor,	Mk84PackageModule] = 0;		     $ItemMax[mfemale,	Mk84PackageModule] = 0;
$ItemMax[marmor,	APClusterModule] = 0;		     $ItemMax[mfemale,	APClusterModule] = 0;
$ItemMax[marmor,	MineClusterModule] = 0;		     $ItemMax[mfemale,	MineClusterModule] = 0;
$ItemMax[marmor,	BombletClusterModule] = 0;	     $ItemMax[mfemale,	BombletClusterModule] = 0;
$ItemMax[marmor,	BunkerBusterModule] = 0;	     $ItemMax[mfemale,	BunkerBusterModule] = 0;
$ItemMax[marmor,	TankSmokeModule] = 0;		     $ItemMax[mfemale,	TankSmokeModule] = 0;
$ItemMax[marmor,	TankNerveGasModule] = 0;	     $ItemMax[mfemale,	TankNerveGasModule] = 0;
$ItemMax[marmor,	IncendiaryNapamModule] = 0;	     $ItemMax[mfemale,	IncendiaryNapamModule] = 0;
$ItemMax[marmor,	NuclearModule] = 0;		     $ItemMax[mfemale,	NuclearModule] = 0;
$ItemMax[marmor,	EMCModule] = 0;	    		     $ItemMax[mfemale,	EMCModule] = 0;
$ItemMax[marmor,	DaisyCutterModule] = 0;		     $ItemMax[mfemale,	DaisyCutterModule] = 0;
$ItemMax[marmor,	SupplyCrateModule] = 0;		     $ItemMax[mfemale,	SupplyCrateModule] = 0;

$ItemMax[sarmor,	SideWinderModule] = 0;		     $ItemMax[sfemale,	SideWinderModule] = 0;
$ItemMax[sarmor,	PhoenixModule] = 0;		     $ItemMax[sfemale,	PhoenixModule] = 0;
$ItemMax[sarmor,	HellFireModule] = 0;		     $ItemMax[sfemale,	HellFireModule] = 0;
$ItemMax[sarmor,	HydraModule] = 0;		     $ItemMax[sfemale,	HydraModule] = 0;
$ItemMax[sarmor,	MaverickModule] = 0;		     $ItemMax[sfemale,	MaverickModule] = 0;
$ItemMax[sarmor,	HARMModule] = 0;		     $ItemMax[sfemale,	HARMModule] = 0;
$ItemMax[sarmor,	Mk82Module] = 0;		     $ItemMax[sfemale,	Mk82Module] = 0;
$ItemMax[sarmor,	Mk84Module] = 0;		     $ItemMax[sfemale,	Mk84Module] = 0;
$ItemMax[sarmor,	Mk82PackageModule] = 0;		     $ItemMax[sfemale,	Mk82PackageModule] = 0;
$ItemMax[sarmor,	Mk84PackageModule] = 0;		     $ItemMax[sfemale,	Mk84PackageModule] = 0;
$ItemMax[sarmor,	APClusterModule] = 0;		     $ItemMax[sfemale,	APClusterModule] = 0;
$ItemMax[sarmor,	MineClusterModule] = 0;		     $ItemMax[sfemale,	MineClusterModule] = 0;
$ItemMax[sarmor,	BombletClusterModule] = 0;	     $ItemMax[sfemale,	BombletClusterModule] = 0;
$ItemMax[sarmor,	BunkerBusterModule] = 0;	     $ItemMax[sfemale,	BunkerBusterModule] = 0;
$ItemMax[sarmor,	TankSmokeModule] = 0;		     $ItemMax[sfemale,	TankSmokeModule] = 0;
$ItemMax[sarmor,	TankNerveGasModule] = 0;	     $ItemMax[sfemale,	TankNerveGasModule] = 0;
$ItemMax[sarmor,	IncendiaryNapamModule] = 0;	     $ItemMax[sfemale,	IncendiaryNapamModule] = 0;
$ItemMax[sarmor,	NuclearModule] = 0;		     $ItemMax[sfemale,	NuclearModule] = 0;
$ItemMax[sarmor,	EMCModule] = 0;	    		     $ItemMax[sfemale,	EMCModule] = 0;
$ItemMax[sarmor,	DaisyCutterModule] = 0;		     $ItemMax[sfemale,	DaisyCutterModule] = 0;
$ItemMax[sarmor,	SupplyCrateModule] = 0;		     $ItemMax[sfemale,	SupplyCrateModule] = 0;

$ItemMax[iarmor,	SideWinderModule] = 0;		     $ItemMax[ifemale,	SideWinderModule] = 0;
$ItemMax[iarmor,	PhoenixModule] = 0;		     $ItemMax[ifemale,	PhoenixModule] = 0;
$ItemMax[iarmor,	HellFireModule] = 0;		     $ItemMax[ifemale,	HellFireModule] = 0;
$ItemMax[iarmor,	HydraModule] = 0;		     $ItemMax[ifemale,	HydraModule] = 0;
$ItemMax[iarmor,	MaverickModule] = 0;		     $ItemMax[ifemale,	MaverickModule] = 0;
$ItemMax[iarmor,	HARMModule] = 0;		     $ItemMax[ifemale,	HARMModule] = 0;
$ItemMax[iarmor,	Mk82Module] = 0;		     $ItemMax[ifemale,	Mk82Module] = 0;
$ItemMax[iarmor,	Mk84Module] = 0;		     $ItemMax[ifemale,	Mk84Module] = 0;
$ItemMax[iarmor,	Mk82PackageModule] = 0;		     $ItemMax[ifemale,	Mk82PackageModule] = 0;
$ItemMax[iarmor,	Mk84PackageModule] = 0;		     $ItemMax[ifemale,	Mk84PackageModule] = 0;
$ItemMax[iarmor,	APClusterModule] = 0;		     $ItemMax[ifemale,	APClusterModule] = 0;
$ItemMax[iarmor,	MineClusterModule] = 0;		     $ItemMax[ifemale,	MineClusterModule] = 0;
$ItemMax[iarmor,	BombletClusterModule] = 0;	     $ItemMax[ifemale,	BombletClusterModule] = 0;
$ItemMax[iarmor,	BunkerBusterModule] = 0;	     $ItemMax[ifemale,	BunkerBusterModule] = 0;
$ItemMax[iarmor,	TankSmokeModule] = 0;		     $ItemMax[ifemale,	TankSmokeModule] = 0;
$ItemMax[iarmor,	TankNerveGasModule] = 0;	     $ItemMax[ifemale,	TankNerveGasModule] = 0;
$ItemMax[iarmor,	IncendiaryNapamModule] = 0;	     $ItemMax[ifemale,	IncendiaryNapamModule] = 0;
$ItemMax[iarmor,	NuclearModule] = 0;		     $ItemMax[ifemale,	NuclearModule] = 0;
$ItemMax[iarmor,	EMCModule] = 0;	    		     $ItemMax[ifemale,	EMCModule] = 0;
$ItemMax[iarmor,	DaisyCutterModule] = 0;		     $ItemMax[ifemale,	DaisyCutterModule] = 0;
$ItemMax[iarmor,	SupplyCrateModule] = 0;		     $ItemMax[ifemale,	SupplyCrateModule] = 0;

$ItemMax[garmor,	SideWinderModule] = 0;		     $ItemMax[gfemale,	SideWinderModule] = 0;
$ItemMax[garmor,	PhoenixModule] = 0;		     $ItemMax[gfemale,	PhoenixModule] = 0;
$ItemMax[garmor,	HellFireModule] = 0;		     $ItemMax[gfemale,	HellFireModule] = 0;
$ItemMax[garmor,	HydraModule] = 0;		     $ItemMax[gfemale,	HydraModule] = 0;
$ItemMax[garmor,	MaverickModule] = 0;		     $ItemMax[gfemale,	MaverickModule] = 0;
$ItemMax[garmor,	HARMModule] = 0;		     $ItemMax[gfemale,	HARMModule] = 0;
$ItemMax[garmor,	Mk82Module] = 0;		     $ItemMax[gfemale,	Mk82Module] = 0;
$ItemMax[garmor,	Mk84Module] = 0;		     $ItemMax[gfemale,	Mk84Module] = 0;
$ItemMax[garmor,	Mk82PackageModule] = 0;		     $ItemMax[gfemale,	Mk82PackageModule] = 0;
$ItemMax[garmor,	Mk84PackageModule] = 0;		     $ItemMax[gfemale,	Mk84PackageModule] = 0;
$ItemMax[garmor,	APClusterModule] = 0;		     $ItemMax[gfemale,	APClusterModule] = 0;
$ItemMax[garmor,	MineClusterModule] = 0;		     $ItemMax[efemale,	MineClusterModule] = 0;
$ItemMax[garmor,	BombletClusterModule] = 0;	     $ItemMax[gfemale,	BombletClusterModule] = 0;
$ItemMax[garmor,	BunkerBusterModule] = 0;	     $ItemMax[gfemale,	BunkerBusterModule] = 0;
$ItemMax[garmor,	TankSmokeModule] = 0;		     $ItemMax[gfemale,	TankSmokeModule] = 0;
$ItemMax[garmor,	TankNerveGasModule] = 0;	     $ItemMax[gfemale,	TankNerveGasModule] = 0;
$ItemMax[garmor,	IncendiaryNapamModule] = 0;	     $ItemMax[gfemale,	IncendiaryNapamModule] = 0;
$ItemMax[garmor,	NuclearModule] = 0;		     $ItemMax[gfemale,	NuclearModule] = 0;
$ItemMax[garmor,	EMCModule] = 0;	    		     $ItemMax[gfemale,	EMCModule] = 0;
$ItemMax[garmor,	DaisyCutterModule] = 0;		     $ItemMax[gfemale,	DaisyCutterModule] = 0;
$ItemMax[garmor,	SupplyCrateModule] = 0;		     $ItemMax[gfemale,	SupplyCrateModule] = 0;

$ItemMax[carmor,	SideWinderModule] = 0;		     $ItemMax[cfemale,	SideWinderModule] = 0;
$ItemMax[carmor,	PhoenixModule] = 0;		     $ItemMax[cfemale,	PhoenixModule] = 0;
$ItemMax[carmor,	HellFireModule] = 0;		     $ItemMax[efemale,	HellFireModule] = 0;
$ItemMax[carmor,	HydraModule] = 0;		     $ItemMax[cfemale,	HydraModule] = 0;
$ItemMax[carmor,	MaverickModule] = 0;		     $ItemMax[cfemale,	MaverickModule] = 0;
$ItemMax[carmor,	HARMModule] = 0;		     $ItemMax[cfemale,	HARMModule] = 0;
$ItemMax[carmor,	Mk82Module] = 0;		     $ItemMax[cfemale,	Mk82Module] = 0;
$ItemMax[carmor,	Mk84Module] = 0;		     $ItemMax[cfemale,	Mk84Module] = 0;
$ItemMax[carmor,	Mk82PackageModule] = 0;		     $ItemMax[cfemale,	Mk82PackageModule] = 0;
$ItemMax[carmor,	Mk84PackageModule] = 0;		     $ItemMax[cfemale,	Mk84PackageModule] = 0;
$ItemMax[carmor,	APClusterModule] = 0;		     $ItemMax[cfemale,	APClusterModule] = 0;
$ItemMax[carmor,	MineClusterModule] = 0;		     $ItemMax[cfemale,	MineClusterModule] = 0;
$ItemMax[carmor,	BombletClusterModule] = 0;	     $ItemMax[cfemale,	BombletClusterModule] = 0;
$ItemMax[carmor,	BunkerBusterModule] = 0;	     $ItemMax[cfemale,	BunkerBusterModule] = 0;
$ItemMax[carmor,	TankSmokeModule] = 0;		     $ItemMax[cfemale,	TankSmokeModule] = 0;
$ItemMax[carmor,	TankNerveGasModule] = 0;	     $ItemMax[cfemale,	TankNerveGasModule] = 0;
$ItemMax[carmor,	IncendiaryNapamModule] = 0;	     $ItemMax[cfemale,	IncendiaryNapamModule] = 0;
$ItemMax[carmor,	NuclearModule] = 0;		     $ItemMax[cfemale,	NuclearModule] = 0;
$ItemMax[carmor,	EMCModule] = 0;	    		     $ItemMax[cfemale,	EMCModule] = 0;
$ItemMax[carmor,	DaisyCutterModule] = 0;		     $ItemMax[cfemale,	DaisyCutterModule] = 0;
$ItemMax[carmor,	SupplyCrateModule] = 0;		     $ItemMax[cfemale,	SupplyCrateModule] = 0;

$ItemMax[earmor,	SideWinderModule] = 1;		     $ItemMax[efemale,	SideWinderModule] = 1;
$ItemMax[earmor,	PhoenixModule] = 1;		     $ItemMax[efemale,	PhoenixModule] = 1;
$ItemMax[earmor,	HellFireModule] = 1;		     $ItemMax[efemale,	HellFireModule] = 1;
$ItemMax[earmor,	HydraModule] = 1;		     $ItemMax[efemale,	HydraModule] = 1;
$ItemMax[earmor,	MaverickModule] = 1;		     $ItemMax[efemale,	MaverickModule] = 1;
$ItemMax[earmor,	HARMModule] = 1;		     $ItemMax[efemale,	HARMModule] = 1;
$ItemMax[earmor,	Mk82Module] = 1;		     $ItemMax[efemale,	Mk82Module] = 1;
$ItemMax[earmor,	Mk84Module] = 1;		     $ItemMax[efemale,	Mk84Module] = 1;
$ItemMax[earmor,	Mk82PackageModule] = 1;		     $ItemMax[efemale,	Mk82PackageModule] = 1;
$ItemMax[earmor,	Mk84PackageModule] = 1;		     $ItemMax[efemale,	Mk84PackageModule] = 1;
$ItemMax[earmor,	APClusterModule] = 1;		     $ItemMax[efemale,	APClusterModule] = 1;
$ItemMax[earmor,	MineClusterModule] = 1;		     $ItemMax[efemale,	MineClusterModule] = 1;
$ItemMax[earmor,	BombletClusterModule] = 1;	     $ItemMax[efemale,	BombletClusterModule] = 1;
$ItemMax[earmor,	BunkerBusterModule] = 1;	     $ItemMax[efemale,	BunkerBusterModule] = 1;
$ItemMax[earmor,	TankSmokeModule] = 1;		     $ItemMax[efemale,	TankSmokeModule] = 1;
$ItemMax[earmor,	TankNerveGasModule] = 1;	     $ItemMax[efemale,	TankNerveGasModule] = 1;
$ItemMax[earmor,	IncendiaryNapamModule] = 1;	     $ItemMax[efemale,	IncendiaryNapamModule] = 1;
$ItemMax[earmor,	NuclearModule] = 1;		     $ItemMax[efemale,	NuclearModule] = 1;
$ItemMax[earmor,	EMCModule] = 1;	    		     $ItemMax[efemale,	EMCModule] = 1;
$ItemMax[earmor,	DaisyCutterModule] = 1;		     $ItemMax[efemale,	DaisyCutterModule] = 1;
$ItemMax[earmor,	SupplyCrateModule] = 1;		     $ItemMax[efemale,	SupplyCrateModule] = 1;

$ItemMax[larmor,	SideWinderModule] = 0;		     $ItemMax[lfemale,	SideWinderModule] = 0;
$ItemMax[larmor,	PhoenixModule] = 0;		     $ItemMax[lfemale,	PhoenixModule] = 0;
$ItemMax[larmor,	HellFireModule] = 0;		     $ItemMax[lfemale,	HellFireModule] = 0;
$ItemMax[larmor,	HydraModule] = 0;		     $ItemMax[lfemale,	HydraModule] = 0;
$ItemMax[larmor,	MaverickModule] = 0;		     $ItemMax[lfemale,	MaverickModule] = 0;
$ItemMax[larmor,	HARMModule] = 0;		     $ItemMax[lfemale,	HARMModule] = 0;
$ItemMax[larmor,	Mk82Module] = 0;		     $ItemMax[lfemale,	Mk82Module] = 0;
$ItemMax[larmor,	Mk84Module] = 0;		     $ItemMax[lfemale,	Mk84Module] = 0;
$ItemMax[larmor,	Mk82PackageModule] = 0;		     $ItemMax[lfemale,	Mk82PackageModule] = 0;
$ItemMax[larmor,	Mk84PackageModule] = 0;		     $ItemMax[lfemale,	Mk84PackageModule] = 0;
$ItemMax[larmor,	APClusterModule] = 0;		     $ItemMax[lfemale,	APClusterModule] = 0;
$ItemMax[larmor,	MineClusterModule] = 0;		     $ItemMax[lfemale,	MineClusterModule] = 0;
$ItemMax[larmor,	BombletClusterModule] = 0;	     $ItemMax[lfemale,	BombletClusterModule] = 0;
$ItemMax[larmor,	BunkerBusterModule] = 0;	     $ItemMax[lfemale,	BunkerBusterModule] = 0;
$ItemMax[larmor,	TankSmokeModule] = 0;		     $ItemMax[lfemale,	TankSmokeModule] = 0;
$ItemMax[larmor,	TankNerveGasModule] = 0;	     $ItemMax[lfemale,	TankNerveGasModule] = 0;
$ItemMax[larmor,	IncendiaryNapamModule] = 0;	     $ItemMax[lfemale,	IncendiaryNapamModule] = 0;
$ItemMax[larmor,	NuclearModule] = 0;		     $ItemMax[lfemale,	NuclearModule] = 0;
$ItemMax[larmor,	EMCModule] = 0;	    		     $ItemMax[lfemale,	male,	EMCModule] = 0;
$ItemMax[larmor,	DaisyCutterModule] = 0;		     $ItemMax[lfemale,	DaisyCutterModule] = 0;
$ItemMax[larmor,	SupplyCrateModule] = 0;		     $ItemMax[lfemale,	SupplyCrateModule] = 0;

$ItemMax[aarmor,	SideWinderModule] = 0;		     $ItemMax[afemale,	SideWinderModule] = 0;
$ItemMax[aarmor,	PhoenixModule] = 0;		     $ItemMax[afemale,	PhoenixModule] = 0;
$ItemMax[aarmor,	HellFireModule] = 0;		     $ItemMax[afemale,	HellFireModule] = 0;
$ItemMax[aarmor,	HydraModule] = 0;		     $ItemMax[afemale,	HydraModule] = 0;
$ItemMax[aarmor,	MaverickModule] = 0;		     $ItemMax[afemale,	MaverickModule] = 0;
$ItemMax[aarmor,	HARMModule] = 0;		     $ItemMax[afemale,	HARMModule] = 0;
$ItemMax[aarmor,	Mk82Module] = 0;		     $ItemMax[afemale,	Mk82Module] = 0;
$ItemMax[aarmor,	Mk84Module] = 0;		     $ItemMax[afemale,	Mk84Module] = 0;
$ItemMax[aarmor,	Mk82PackageModule] = 0;		     $ItemMax[afemale,	Mk82PackageModule] = 0;
$ItemMax[aarmor,	Mk84PackageModule] = 0;		     $ItemMax[afemale,	Mk84PackageModule] = 0;
$ItemMax[aarmor,	APClusterModule] = 0;		     $ItemMax[afemale,	APClusterModule] = 0;
$ItemMax[aarmor,	MineClusterModule] = 0;		     $ItemMax[afemale,	MineClusterModule] = 0;
$ItemMax[aarmor,	BombletClusterModule] = 0;	     $ItemMax[afemale,	BombletClusterModule] = 0;
$ItemMax[aarmor,	BunkerBusterModule] = 0;	     $ItemMax[afemale,	BunkerBusterModule] = 0;
$ItemMax[aarmor,	TankSmokeModule] = 0;		     $ItemMax[afemale,	TankSmokeModule] = 0;
$ItemMax[aarmor,	TankNerveGasModule] = 0;	     $ItemMax[afemale,	TankNerveGasModule] = 0;
$ItemMax[aarmor,	IncendiaryNapamModule] = 0;	     $ItemMax[afemale,	IncendiaryNapamModule] = 0;
$ItemMax[aarmor,	NuclearModule] = 0;		     $ItemMax[afemale,	NuclearModule] = 0;
$ItemMax[aarmor,	EMCModule] = 0;	    		     $ItemMax[afemale,	EMCModule] = 0;
$ItemMax[aarmor,	DaisyCutterModule] = 0;		     $ItemMax[afemale,	DaisyCutterModule] = 0;
$ItemMax[aarmor,	SupplyCrateModule] = 0;		     $ItemMax[afemale,	SupplyCrateModule] = 0;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
$ItemMax[marmor2,	SideWinderModule] = 0;		     $ItemMax[mfemale2,	SideWinderModule] = 0;
$ItemMax[marmor2,	PhoenixModule] = 0;		     $ItemMax[mfemale2,	PhoenixModule] = 0;
$ItemMax[marmor2,	HellFireModule] = 0;		     $ItemMax[mfemale2,	HellFireModule] = 0;
$ItemMax[marmor2,	HydraModule] = 0;		     $ItemMax[mfemale2,	HydraModule] = 0;
$ItemMax[marmor2,	MaverickModule] = 0;		     $ItemMax[mfemale2,	MaverickModule] = 0;
$ItemMax[marmor2,	HARMModule] = 0;		     $ItemMax[mfemale2,	HARMModule] = 0;
$ItemMax[marmor2,	Mk82Module] = 0;		     $ItemMax[mfemale2,	Mk82Module] = 0;
$ItemMax[marmor2,	Mk84Module] = 0;		     $ItemMax[mfemale2,	Mk84Module] = 0;
$ItemMax[marmor2,	Mk82PackageModule] = 0;		     $ItemMax[mfemale2,	Mk82PackageModule] = 0;
$ItemMax[marmor2,	Mk84PackageModule] = 0;		     $ItemMax[mfemale2,	Mk84PackageModule] = 0;
$ItemMax[marmor2,	APClusterModule] = 0;		     $ItemMax[mfemale2,	APClusterModule] = 0;
$ItemMax[marmor2,	MineClusterModule] = 0;		     $ItemMax[mfemale2,	MineClusterModule] = 0;
$ItemMax[marmor2,	BombletClusterModule] = 0;	     $ItemMax[mfemale2,	BombletClusterModule] = 0;
$ItemMax[marmor2,	BunkerBusterModule] = 0;	     $ItemMax[mfemale2,	BunkerBusterModule] = 0;
$ItemMax[marmor2,	TankSmokeModule] = 0;		     $ItemMax[mfemale2,	TankSmokeModule] = 0;
$ItemMax[marmor2,	TankNerveGasModule] = 0;	     $ItemMax[mfemale2,	TankNerveGasModule] = 0;
$ItemMax[marmor2,	IncendiaryNapamModule] = 0;	     $ItemMax[mfemale2,	IncendiaryNapamModule] = 0;
$ItemMax[marmor2,	NuclearModule] = 0;		     $ItemMax[mfemale2,	NuclearModule] = 0;
$ItemMax[marmor2,	EMCModule] = 0;	    		     $ItemMax[mfemale2,	EMCModule] = 0;
$ItemMax[marmor2,	DaisyCutterModule] = 0;		     $ItemMax[mfemale2,	DaisyCutterModule] = 0;
$ItemMax[marmor2,	SupplyCrateModule] = 0;		     $ItemMax[mfemale2,	SupplyCrateModule] = 0;

$ItemMax[sarmor2,	SideWinderModule] = 0;		     $ItemMax[sfemale2,	SideWinderModule] = 0;
$ItemMax[sarmor2,	PhoenixModule] = 0;		     $ItemMax[sfemale2,	PhoenixModule] = 0;
$ItemMax[sarmor2,	HellFireModule] = 0;		     $ItemMax[sfemale2,	HellFireModule] = 0;
$ItemMax[sarmor2,	HydraModule] = 0;		     $ItemMax[sfemale2,	HydraModule] = 0;
$ItemMax[sarmor2,	MaverickModule] = 0;		     $ItemMax[sfemale2,	MaverickModule] = 0;
$ItemMax[sarmor2,	HARMModule] = 0;		     $ItemMax[sfemale2,	HARMModule] = 0;
$ItemMax[sarmor2,	Mk82Module] = 0;		     $ItemMax[sfemale2,	Mk82Module] = 0;
$ItemMax[sarmor2,	Mk84Module] = 0;		     $ItemMax[sfemale2,	Mk84Module] = 0;
$ItemMax[sarmor2,	Mk82PackageModule] = 0;		     $ItemMax[sfemale2,	Mk82PackageModule] = 0;
$ItemMax[sarmor2,	Mk84PackageModule] = 0;		     $ItemMax[sfemale2,	Mk84PackageModule] = 0;
$ItemMax[sarmor2,	APClusterModule] = 0;		     $ItemMax[sfemale2,	APClusterModule] = 0;
$ItemMax[sarmor2,	MineClusterModule] = 0;		     $ItemMax[sfemale2,	MineClusterModule] = 0;
$ItemMax[sarmor2,	BombletClusterModule] = 0;	     $ItemMax[sfemale2,	BombletClusterModule] = 0;
$ItemMax[sarmor2,	BunkerBusterModule] = 0;	     $ItemMax[sfemale2,	BunkerBusterModule] = 0;
$ItemMax[sarmor2,	TankSmokeModule] = 0;		     $ItemMax[sfemale2,	TankSmokeModule] = 0;
$ItemMax[sarmor2,	TankNerveGasModule] = 0;	     $ItemMax[sfemale2,	TankNerveGasModule] = 0;
$ItemMax[sarmor2,	IncendiaryNapamModule] = 0;	     $ItemMax[sfemale2,	IncendiaryNapamModule] = 0;
$ItemMax[sarmor2,	NuclearModule] = 0;		     $ItemMax[sfemale2,	NuclearModule] = 0;
$ItemMax[sarmor2,	EMCModule] = 0;	    		     $ItemMax[sfemale2,	EMCModule] = 0;
$ItemMax[sarmor2,	DaisyCutterModule] = 0;		     $ItemMax[sfemale2,	DaisyCutterModule] = 0;
$ItemMax[sarmor2,	SupplyCrateModule] = 0;		     $ItemMax[sfemale2,	SupplyCrateModule] = 0;

$ItemMax[iarmor2,	SideWinderModule] = 0;		     $ItemMax[ifemale2,	SideWinderModule] = 0;
$ItemMax[iarmor2,	PhoenixModule] = 0;		     $ItemMax[ifemale2,	PhoenixModule] = 0;
$ItemMax[iarmor2,	HellFireModule] = 0;		     $ItemMax[ifemale2,	HellFireModule] = 0;
$ItemMax[iarmor2,	HydraModule] = 0;		     $ItemMax[ifemale2,	HydraModule] = 0;
$ItemMax[iarmor2,	MaverickModule] = 0;		     $ItemMax[ifemale2,	MaverickModule] = 0;
$ItemMax[iarmor2,	HARMModule] = 0;		     $ItemMax[ifemale2,	HARMModule] = 0;
$ItemMax[iarmor2,	Mk82Module] = 0;		     $ItemMax[ifemale2,	Mk82Module] = 0;
$ItemMax[iarmor2,	Mk84Module] = 0;		     $ItemMax[ifemale2,	Mk84Module] = 0;
$ItemMax[iarmor2,	Mk82PackageModule] = 0;		     $ItemMax[ifemale2,	Mk82PackageModule] = 0;
$ItemMax[iarmor2,	Mk84PackageModule] = 0;		     $ItemMax[ifemale2,	Mk84PackageModule] = 0;
$ItemMax[iarmor2,	APClusterModule] = 0;		     $ItemMax[ifemale2,	APClusterModule] = 0;
$ItemMax[iarmor2,	MineClusterModule] = 0;		     $ItemMax[ifemale2,	MineClusterModule] = 0;
$ItemMax[iarmor2,	BombletClusterModule] = 0;	     $ItemMax[ifemale2,	BombletClusterModule] = 0;
$ItemMax[iarmor2,	BunkerBusterModule] = 0;	     $ItemMax[ifemale2,	BunkerBusterModule] = 0;
$ItemMax[iarmor2,	TankSmokeModule] = 0;		     $ItemMax[ifemale2,	TankSmokeModule] = 0;
$ItemMax[iarmor2,	TankNerveGasModule] = 0;	     $ItemMax[ifemale2,	TankNerveGasModule] = 0;
$ItemMax[iarmor2,	IncendiaryNapamModule] = 0;	     $ItemMax[ifemale2,	IncendiaryNapamModule] = 0;
$ItemMax[iarmor2,	NuclearModule] = 0;		     $ItemMax[ifemale2,	NuclearModule] = 0;
$ItemMax[iarmor2,	EMCModule] = 0;	    		     $ItemMax[ifemale2,	EMCModule] = 0;
$ItemMax[iarmor2,	DaisyCutterModule] = 0;		     $ItemMax[ifemale2,	DaisyCutterModule] = 0;
$ItemMax[iarmor2,	SupplyCrateModule] = 0;		     $ItemMax[ifemale2,	SupplyCrateModule] = 0;

$ItemMax[garmor2,	SideWinderModule] = 0;		     $ItemMax[gfemale2,	SideWinderModule] = 0;
$ItemMax[garmor2,	PhoenixModule] = 0;		     $ItemMax[gfemale2,	PhoenixModule] = 0;
$ItemMax[garmor2,	HellFireModule] = 0;		     $ItemMax[gfemale2,	HellFireModule] = 0;
$ItemMax[garmor2,	HydraModule] = 0;		     $ItemMax[gfemale2,	HydraModule] = 0;
$ItemMax[garmor2,	MaverickModule] = 0;		     $ItemMax[gfemale2,	MaverickModule] = 0;
$ItemMax[garmor2,	HARMModule] = 0;		     $ItemMax[gfemale2,	HARMModule] = 0;
$ItemMax[garmor2,	Mk82Module] = 0;		     $ItemMax[gfemale2,	Mk82Module] = 0;
$ItemMax[garmor2,	Mk84Module] = 0;		     $ItemMax[gfemale2,	Mk84Module] = 0;
$ItemMax[garmor2,	Mk82PackageModule] = 0;		     $ItemMax[gfemale2,	Mk82PackageModule] = 0;
$ItemMax[garmor2,	Mk84PackageModule] = 0;		     $ItemMax[gfemale2,	Mk84PackageModule] = 0;
$ItemMax[garmor2,	APClusterModule] = 0;		     $ItemMax[gfemale2,	APClusterModule] = 0;
$ItemMax[garmor2,	MineClusterModule] = 0;		     $ItemMax[efemale2,	MineClusterModule] = 0;
$ItemMax[garmor2,	BombletClusterModule] = 0;	     $ItemMax[gfemale2,	BombletClusterModule] = 0;
$ItemMax[garmor2,	BunkerBusterModule] = 0;	     $ItemMax[gfemale2,	BunkerBusterModule] = 0;
$ItemMax[garmor2,	TankSmokeModule] = 0;		     $ItemMax[gfemale2,	TankSmokeModule] = 0;
$ItemMax[garmor2,	TankNerveGasModule] = 0;	     $ItemMax[gfemale2,	TankNerveGasModule] = 0;
$ItemMax[garmor2,	IncendiaryNapamModule] = 0;	     $ItemMax[gfemale2,	IncendiaryNapamModule] = 0;
$ItemMax[garmor2,	NuclearModule] = 0;		     $ItemMax[gfemale2,	NuclearModule] = 0;
$ItemMax[garmor2,	EMCModule] = 0;	    		     $ItemMax[gfemale2,	EMCModule] = 0;
$ItemMax[garmor2,	DaisyCutterModule] = 0;		     $ItemMax[gfemale2,	DaisyCutterModule] = 0;
$ItemMax[garmor2,	SupplyCrateModule] = 0;		     $ItemMax[gfemale2,	SupplyCrateModule] = 0;

$ItemMax[carmor2,	SideWinderModule] = 0;		     $ItemMax[cfemale2,	SideWinderModule] = 0;
$ItemMax[carmor2,	PhoenixModule] = 0;		     $ItemMax[cfemale2,	PhoenixModule] = 0;
$ItemMax[carmor2,	HellFireModule] = 0;		     $ItemMax[efemale2,	HellFireModule] = 0;
$ItemMax[carmor2,	HydraModule] = 0;		     $ItemMax[cfemale2,	HydraModule] = 0;
$ItemMax[carmor2,	MaverickModule] = 0;		     $ItemMax[cfemale2,	MaverickModule] = 0;
$ItemMax[carmor2,	HARMModule] = 0;		     $ItemMax[cfemale2,	HARMModule] = 0;
$ItemMax[carmor2,	Mk82Module] = 0;		     $ItemMax[cfemale2,	Mk82Module] = 0;
$ItemMax[carmor2,	Mk84Module] = 0;		     $ItemMax[cfemale2,	Mk84Module] = 0;
$ItemMax[carmor2,	Mk82PackageModule] = 0;		     $ItemMax[cfemale2,	Mk82PackageModule] = 0;
$ItemMax[carmor2,	Mk84PackageModule] = 0;		     $ItemMax[cfemale2,	Mk84PackageModule] = 0;
$ItemMax[carmor2,	APClusterModule] = 0;		     $ItemMax[cfemale2,	APClusterModule] = 0;
$ItemMax[carmor2,	MineClusterModule] = 0;		     $ItemMax[cfemale2,	MineClusterModule] = 0;
$ItemMax[carmor2,	BombletClusterModule] = 0;	     $ItemMax[cfemale2,	BombletClusterModule] = 0;
$ItemMax[carmor2,	BunkerBusterModule] = 0;	     $ItemMax[cfemale2,	BunkerBusterModule] = 0;
$ItemMax[carmor2,	TankSmokeModule] = 0;		     $ItemMax[cfemale2,	TankSmokeModule] = 0;
$ItemMax[carmor2,	TankNerveGasModule] = 0;	     $ItemMax[cfemale2,	TankNerveGasModule] = 0;
$ItemMax[carmor2,	IncendiaryNapamModule] = 0;	     $ItemMax[cfemale2,	IncendiaryNapamModule] = 0;
$ItemMax[carmor2,	NuclearModule] = 0;		     $ItemMax[cfemale2,	NuclearModule] = 0;
$ItemMax[carmor2,	EMCModule] = 0;	    		     $ItemMax[cfemale2,	EMCModule] = 0;
$ItemMax[carmor2,	DaisyCutterModule] = 0;		     $ItemMax[cfemale2,	DaisyCutterModule] = 0;
$ItemMax[carmor2,	SupplyCrateModule] = 0;		     $ItemMax[cfemale2,	SupplyCrateModule] = 0;

$ItemMax[earmor2,	SideWinderModule] = 1;		     $ItemMax[efemale2,	SideWinderModule] = 1;
$ItemMax[earmor2,	PhoenixModule] = 1;		     $ItemMax[efemale2,	PhoenixModule] = 1;
$ItemMax[earmor2,	HellFireModule] = 1;		     $ItemMax[efemale2,	HellFireModule] = 1;
$ItemMax[earmor2,	HydraModule] = 1;		     $ItemMax[efemale2,	HydraModule] = 1;
$ItemMax[earmor2,	MaverickModule] = 1;		     $ItemMax[efemale2,	MaverickModule] = 1;
$ItemMax[earmor2,	HARMModule] = 1;		     $ItemMax[efemale2,	HARMModule] = 1;
$ItemMax[earmor2,	Mk82Module] = 1;		     $ItemMax[efemale2,	Mk82Module] = 1;
$ItemMax[earmor2,	Mk84Module] = 1;		     $ItemMax[efemale2,	Mk84Module] = 1;
$ItemMax[earmor2,	Mk82PackageModule] = 1;		     $ItemMax[efemale2,	Mk82PackageModule] = 1;
$ItemMax[earmor2,	Mk84PackageModule] = 1;		     $ItemMax[efemale2,	Mk84PackageModule] = 1;
$ItemMax[earmor2,	APClusterModule] = 1;		     $ItemMax[efemale2,	APClusterModule] = 1;
$ItemMax[earmor2,	MineClusterModule] = 1;		     $ItemMax[efemale2,	MineClusterModule] = 1;
$ItemMax[earmor2,	BombletClusterModule] = 1;	     $ItemMax[efemale2,	BombletClusterModule] = 1;
$ItemMax[earmor2,	BunkerBusterModule] = 1;	     $ItemMax[efemale2,	BunkerBusterModule] = 1;
$ItemMax[earmor2,	TankSmokeModule] = 1;		     $ItemMax[efemale2,	TankSmokeModule] = 1;
$ItemMax[earmor2,	TankNerveGasModule] = 1;	     $ItemMax[efemale2,	TankNerveGasModule] = 1;
$ItemMax[earmor2,	IncendiaryNapamModule] = 1;	     $ItemMax[efemale2,	IncendiaryNapamModule] = 1;
$ItemMax[earmor2,	NuclearModule] = 1;		     $ItemMax[efemale2,	NuclearModule] = 1;
$ItemMax[earmor2,	EMCModule] = 1;	    		     $ItemMax[efemale2,	EMCModule] = 1;
$ItemMax[earmor2,	DaisyCutterModule] = 1;		     $ItemMax[efemale2,	DaisyCutterModule] = 1;
$ItemMax[earmor2,	SupplyCrateModule] = 1;		     $ItemMax[efemale2,	SupplyCrateModule] = 1;

$ItemMax[larmor2,	SideWinderModule] = 0;		     $ItemMax[lfemale2,	SideWinderModule] = 0;
$ItemMax[larmor2,	PhoenixModule] = 0;		     $ItemMax[lfemale2,	PhoenixModule] = 0;
$ItemMax[larmor2,	HellFireModule] = 0;		     $ItemMax[lfemale2,	HellFireModule] = 0;
$ItemMax[larmor2,	HydraModule] = 0;		     $ItemMax[lfemale2,	HydraModule] = 0;
$ItemMax[larmor2,	MaverickModule] = 0;		     $ItemMax[lfemale2,	MaverickModule] = 0;
$ItemMax[larmor2,	HARMModule] = 0;		     $ItemMax[lfemale2,	HARMModule] = 0;
$ItemMax[larmor2,	Mk82Module] = 0;		     $ItemMax[lfemale2,	Mk82Module] = 0;
$ItemMax[larmor2,	Mk84Module] = 0;		     $ItemMax[lfemale2,	Mk84Module] = 01;
$ItemMax[larmor2,	Mk82PackageModule] = 0;		     $ItemMax[lfemale2,	Mk82PackageModule] = 0;
$ItemMax[larmor2,	Mk84PackageModule] = 0;		     $ItemMax[lfemale2,	Mk84PackageModule] = 0;
$ItemMax[larmor2,	APClusterModule] = 0;		     $ItemMax[lfemale2,	APClusterModule] = 0;
$ItemMax[larmor2,	MineClusterModule] = 0;		     $ItemMax[lfemale2,	MineClusterModule] = 0;
$ItemMax[larmor2,	BombletClusterModule] = 0;	     $ItemMax[lfemale2,	BombletClusterModule] = 0;
$ItemMax[larmor2,	BunkerBusterModule] = 0;	     $ItemMax[lfemale2,	BunkerBusterModule] = 0;
$ItemMax[larmor2,	TankSmokeModule] = 0;		     $ItemMax[lfemale2,	TankSmokeModule] = 0;
$ItemMax[larmor2,	TankNerveGasModule] = 0;	     $ItemMax[lfemale2,	TankNerveGasModule] = 0;
$ItemMax[larmor2,	IncendiaryNapamModule] = 0;	     $ItemMax[lfemale2,	IncendiaryNapamModule] = 0;
$ItemMax[larmor2,	NuclearModule] = 0;		     $ItemMax[lfemale2,	NuclearModule] = 0;
$ItemMax[larmor2,	EMCModule] = 0;	    		     $ItemMax[lfemale2,	male,	EMCModule] = 0;
$ItemMax[larmor2,	DaisyCutterModule] = 0;		     $ItemMax[lfemale2,	DaisyCutterModule] = 0;
$ItemMax[larmor2,	SupplyCrateModule] = 0;		     $ItemMax[lfemale2,	SupplyCrateModule] = 0;

$ItemMax[aarmor2,	SideWinderModule] = 0;		     $ItemMax[afemale2,	SideWinderModule] = 0;
$ItemMax[aarmor2,	PhoenixModule] = 0;		     $ItemMax[afemale2,	PhoenixModule] = 0;
$ItemMax[aarmor2,	HellFireModule] = 0;		     $ItemMax[afemale2,	HellFireModule] = 0;
$ItemMax[aarmor2,	HydraModule] = 0;		     $ItemMax[afemale2,	HydraModule] = 0;
$ItemMax[aarmor2,	MaverickModule] = 0;		     $ItemMax[afemale2,	MaverickModule] = 0;
$ItemMax[aarmor2,	HARMModule] = 0;		     $ItemMax[afemale2,	HARMModule] = 0;
$ItemMax[aarmor2,	Mk82Module] = 0;		     $ItemMax[afemale2,	Mk82Module] = 0;
$ItemMax[aarmor2,	Mk84Module] = 0;		     $ItemMax[afemale2,	Mk84Module] = 0;
$ItemMax[aarmor2,	Mk82PackageModule] = 0;		     $ItemMax[afemale2,	Mk82PackageModule] = 0;
$ItemMax[aarmor2,	Mk84PackageModule] = 0;		     $ItemMax[afemale2,	Mk84PackageModule] = 0;
$ItemMax[aarmor2,	APClusterModule] = 0;		     $ItemMax[afemale2,	APClusterModule] = 0;
$ItemMax[aarmor2,	MineClusterModule] = 0;		     $ItemMax[afemale2,	MineClusterModule] = 0;
$ItemMax[aarmor2,	BombletClusterModule] = 0;	     $ItemMax[afemale2,	BombletClusterModule] = 0;
$ItemMax[aarmor2,	BunkerBusterModule] = 0;	     $ItemMax[afemale2,	BunkerBusterModule] = 0;
$ItemMax[aarmor2,	TankSmokeModule] = 0;		     $ItemMax[afemale2,	TankSmokeModule] = 0;
$ItemMax[aarmor2,	TankNerveGasModule] = 0;	     $ItemMax[afemale2,	TankNerveGasModule] = 0;
$ItemMax[aarmor2,	IncendiaryNapamModule] = 0;	     $ItemMax[afemale2,	IncendiaryNapamModule] = 0;
$ItemMax[aarmor2,	NuclearModule] = 0;		     $ItemMax[afemale2,	NuclearModule] = 0;
$ItemMax[aarmor2,	EMCModule] = 0;	    		     $ItemMax[afemale2,	EMCModule] = 0;
$ItemMax[aarmor2,	DaisyCutterModule] = 0;		     $ItemMax[afemale2,	DaisyCutterModule] = 0;
$ItemMax[aarmor2,	SupplyCrateModule] = 0;		     $ItemMax[afemale2,	SupplyCrateModule] = 0;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
$ItemMax[marmor3,	SideWinderModule] = 0;		     $ItemMax[mfemale3,	SideWinderModule] = 0;
$ItemMax[marmor3,	PhoenixModule] = 0;		     $ItemMax[mfemale3,	PhoenixModule] = 0;
$ItemMax[marmor3,	HellFireModule] = 0;		     $ItemMax[mfemale3,	HellFireModule] = 0;
$ItemMax[marmor3,	HydraModule] = 0;		     $ItemMax[mfemale3,	HydraModule] = 0;
$ItemMax[marmor3,	MaverickModule] = 0;		     $ItemMax[mfemale3,	MaverickModule] = 0;
$ItemMax[marmor3,	HARMModule] = 0;		     $ItemMax[mfemale3,	HARMModule] = 0;
$ItemMax[marmor3,	Mk82Module] = 0;		     $ItemMax[mfemale3,	Mk82Module] = 0;
$ItemMax[marmor3,	Mk84Module] = 0;		     $ItemMax[mfemale3,	Mk84Module] = 0;
$ItemMax[marmor3,	Mk82PackageModule] = 0;		     $ItemMax[mfemale3,	Mk82PackageModule] = 0;
$ItemMax[marmor3,	Mk84PackageModule] = 0;		     $ItemMax[mfemale3,	Mk84PackageModule] = 0;
$ItemMax[marmor3,	APClusterModule] = 0;		     $ItemMax[mfemale3,	APClusterModule] = 0;
$ItemMax[marmor3,	MineClusterModule] = 0;		     $ItemMax[mfemale3,	MineClusterModule] = 0;
$ItemMax[marmor3,	BombletClusterModule] = 0;	     $ItemMax[mfemale3,	BombletClusterModule] = 0;
$ItemMax[marmor3,	BunkerBusterModule] = 0;	     $ItemMax[mfemale3,	BunkerBusterModule] = 0;
$ItemMax[marmor3,	TankSmokeModule] = 0;		     $ItemMax[mfemale3,	TankSmokeModule] = 0;
$ItemMax[marmor3,	TankNerveGasModule] = 0;	     $ItemMax[mfemale3,	TankNerveGasModule] = 0;
$ItemMax[marmor3,	IncendiaryNapamModule] = 0;	     $ItemMax[mfemale3,	IncendiaryNapamModule] = 0;
$ItemMax[marmor3,	NuclearModule] = 0;		     $ItemMax[mfemale3,	NuclearModule] = 0;
$ItemMax[marmor3,	EMCModule] = 0;	    		     $ItemMax[mfemale3,	EMCModule] = 0;
$ItemMax[marmor3,	DaisyCutterModule] = 0;		     $ItemMax[mfemale3,	DaisyCutterModule] = 0;
$ItemMax[marmor3,	SupplyCrateModule] = 0;		     $ItemMax[mfemale3,	SupplyCrateModule] = 0;

$ItemMax[sarmor3,	SideWinderModule] = 0;		     $ItemMax[sfemale3,	SideWinderModule] = 0;
$ItemMax[sarmor3,	PhoenixModule] = 0;		     $ItemMax[sfemale3,	PhoenixModule] = 0;
$ItemMax[sarmor3,	HellFireModule] = 0;		     $ItemMax[sfemale3,	HellFireModule] = 0;
$ItemMax[sarmor3,	HydraModule] = 0;		     $ItemMax[sfemale3,	HydraModule] = 0;
$ItemMax[sarmor3,	MaverickModule] = 0;		     $ItemMax[sfemale3,	MaverickModule] = 0;
$ItemMax[sarmor3,	HARMModule] = 0;		     $ItemMax[sfemale3,	HARMModule] = 0;
$ItemMax[sarmor3,	Mk82Module] = 0;		     $ItemMax[sfemale3,	Mk82Module] = 0;
$ItemMax[sarmor3,	Mk84Module] = 0;		     $ItemMax[sfemale3,	Mk84Module] = 0;
$ItemMax[sarmor3,	Mk82PackageModule] = 0;		     $ItemMax[sfemale3,	Mk82PackageModule] = 0;
$ItemMax[sarmor3,	Mk84PackageModule] = 0;		     $ItemMax[sfemale3,	Mk84PackageModule] = 0;
$ItemMax[sarmor3,	APClusterModule] = 0;		     $ItemMax[sfemale3,	APClusterModule] = 0;
$ItemMax[sarmor3,	MineClusterModule] = 0;		     $ItemMax[sfemale3,	MineClusterModule] = 0;
$ItemMax[sarmor3,	BombletClusterModule] = 0;	     $ItemMax[sfemale3,	BombletClusterModule] = 0;
$ItemMax[sarmor3,	BunkerBusterModule] = 0;	     $ItemMax[sfemale3,	BunkerBusterModule] = 0;
$ItemMax[sarmor3,	TankSmokeModule] = 0;		     $ItemMax[sfemale3,	TankSmokeModule] = 0;
$ItemMax[sarmor3,	TankNerveGasModule] = 0;	     $ItemMax[sfemale3,	TankNerveGasModule] = 0;
$ItemMax[sarmor3,	IncendiaryNapamModule] = 0;	     $ItemMax[sfemale3,	IncendiaryNapamModule] = 0;
$ItemMax[sarmor3,	NuclearModule] = 0;		     $ItemMax[sfemale3,	NuclearModule] = 0;
$ItemMax[sarmor3,	EMCModule] = 0;	    		     $ItemMax[sfemale3,	EMCModule] = 0;
$ItemMax[sarmor3,	DaisyCutterModule] = 0;		     $ItemMax[sfemale3,	DaisyCutterModule] = 0;
$ItemMax[sarmor3,	SupplyCrateModule] = 0;		     $ItemMax[sfemale3,	SupplyCrateModule] = 0;

$ItemMax[iarmor3,	SideWinderModule] = 0;		     $ItemMax[ifemale3,	SideWinderModule] = 0;
$ItemMax[iarmor3,	PhoenixModule] = 0;		     $ItemMax[ifemale3,	PhoenixModule] = 0;
$ItemMax[iarmor3,	HydraModule] = 0;		     $ItemMax[ifemale3,	HydraModule] = 0;
$ItemMax[iarmor3,	HellFireModule] = 0;		     $ItemMax[ifemale3,	HellFireModule] = 0;
$ItemMax[iarmor3,	MaverickModule] = 0;		     $ItemMax[ifemale3,	MaverickModule] = 0;
$ItemMax[iarmor3,	HARMModule] = 0;		     $ItemMax[ifemale3,	HARMModule] = 0;
$ItemMax[iarmor3,	Mk82Module] = 0;		     $ItemMax[ifemale3,	Mk82Module] = 0;
$ItemMax[iarmor3,	Mk84Module] = 0;		     $ItemMax[ifemale3,	Mk84Module] = 0;
$ItemMax[iarmor3,	Mk82PackageModule] = 0;		     $ItemMax[ifemale3,	Mk82PackageModule] = 0;
$ItemMax[iarmor3,	Mk84PackageModule] = 0;		     $ItemMax[ifemale3,	Mk84PackageModule] = 0;
$ItemMax[iarmor3,	APClusterModule] = 0;		     $ItemMax[ifemale3,	APClusterModule] = 0;
$ItemMax[iarmor3,	MineClusterModule] = 0;		     $ItemMax[ifemale3,	MineClusterModule] = 0;
$ItemMax[iarmor3,	BombletClusterModule] = 0;	     $ItemMax[ifemale3,	BombletClusterModule] = 0;
$ItemMax[iarmor3,	BunkerBusterModule] = 0;	     $ItemMax[ifemale3,	BunkerBusterModule] = 0;
$ItemMax[iarmor3,	TankSmokeModule] = 0;		     $ItemMax[ifemale3,	TankSmokeModule] = 0;
$ItemMax[iarmor3,	TankNerveGasModule] = 0;	     $ItemMax[ifemale3,	TankNerveGasModule] = 0;
$ItemMax[iarmor3,	IncendiaryNapamModule] = 0;	     $ItemMax[ifemale3,	IncendiaryNapamModule] = 0;
$ItemMax[iarmor3,	NuclearModule] = 0;		     $ItemMax[ifemale3,	NuclearModule] = 0;
$ItemMax[iarmor3,	EMCModule] = 0;	    		     $ItemMax[ifemale3,	EMCModule] = 0;
$ItemMax[iarmor3,	DaisyCutterModule] = 0;		     $ItemMax[ifemale3,	DaisyCutterModule] = 0;
$ItemMax[iarmor3,	SupplyCrateModule] = 0;		     $ItemMax[ifemale3,	SupplyCrateModule] = 0;

$ItemMax[garmor3,	SideWinderModule] = 0;		     $ItemMax[gfemale3,	SideWinderModule] = 0;
$ItemMax[garmor3,	PhoenixModule] = 0;		     $ItemMax[gfemale3,	PhoenixModule] = 0;
$ItemMax[garmor3,	HellFireModule] = 0;		     $ItemMax[gfemale3,	HellFireModule] = 0;
$ItemMax[garmor3,	HydraModule] = 0;		     $ItemMax[gfemale3,	HydraModule] = 0;
$ItemMax[garmor3,	MaverickModule] = 0;		     $ItemMax[gfemale3,	MaverickModule] = 0;
$ItemMax[garmor3,	HARMModule] = 0;		     $ItemMax[gfemale3,	HARMModule] = 0;
$ItemMax[garmor3,	Mk82Module] = 0;		     $ItemMax[gfemale3,	Mk82Module] = 0;
$ItemMax[garmor3,	Mk84Module] = 0;		     $ItemMax[gfemale3,	Mk84Module] = 0;
$ItemMax[garmor3,	Mk82PackageModule] = 0;		     $ItemMax[gfemale3,	Mk82PackageModule] = 0;
$ItemMax[garmor3,	Mk84PackageModule] = 0;		     $ItemMax[gfemale3,	Mk84PackageModule] = 0;
$ItemMax[garmor3,	APClusterModule] = 0;		     $ItemMax[gfemale3,	APClusterModule] = 0;
$ItemMax[garmor3,	MineClusterModule] = 0;		     $ItemMax[efemale3,	MineClusterModule] = 0;
$ItemMax[garmor3,	BombletClusterModule] = 0;	     $ItemMax[gfemale3,	BombletClusterModule] = 0;
$ItemMax[garmor3,	BunkerBusterModule] = 0;	     $ItemMax[gfemale3,	BunkerBusterModule] = 0;
$ItemMax[garmor3,	TankSmokeModule] = 0;		     $ItemMax[gfemale3,	TankSmokeModule] = 0;
$ItemMax[garmor3,	TankNerveGasModule] = 0;	     $ItemMax[gfemale3,	TankNerveGasModule] = 0;
$ItemMax[garmor3,	IncendiaryNapamModule] = 0;	     $ItemMax[gfemale3,	IncendiaryNapamModule] = 0;
$ItemMax[garmor3,	NuclearModule] = 0;		     $ItemMax[gfemale3,	NuclearModule] = 0;
$ItemMax[garmor3,	EMCModule] = 0;	    		     $ItemMax[gfemale3,	EMCModule] = 0;
$ItemMax[garmor3,	DaisyCutterModule] = 0;		     $ItemMax[gfemale3,	DaisyCutterModule] = 0;
$ItemMax[garmor3,	SupplyCrateModule] = 0;		     $ItemMax[gfemale3,	SupplyCrateModule] = 0;

$ItemMax[carmor3,	SideWinderModule] = 0;		     $ItemMax[cfemale3,	SideWinderModule] = 0;
$ItemMax[carmor3,	PhoenixModule] = 0;		     $ItemMax[cfemale3,	PhoenixModule] = 0;
$ItemMax[carmor3,	HellFireModule] = 0;		     $ItemMax[efemale3,	HellFireModule] = 0;
$ItemMax[carmor3,	HydraModule] = 0;		     $ItemMax[cfemale3,	HydraModule] = 0;
$ItemMax[carmor3,	MaverickModule] = 0;		     $ItemMax[cfemale3,	MaverickModule] = 0;
$ItemMax[carmor3,	HARMModule] = 0;		     $ItemMax[cfemale3,	HARMModule] = 0;
$ItemMax[carmor3,	Mk82Module] = 0;		     $ItemMax[cfemale3,	Mk82Module] = 0;
$ItemMax[carmor3,	Mk84Module] = 0;		     $ItemMax[cfemale3,	Mk84Module] = 0;
$ItemMax[carmor3,	Mk82PackageModule] = 0;		     $ItemMax[cfemale3,	Mk82PackageModule] = 0;
$ItemMax[carmor3,	Mk84PackageModule] = 0;		     $ItemMax[cfemale3,	Mk84PackageModule] = 0;
$ItemMax[carmor3,	APClusterModule] = 0;		     $ItemMax[cfemale3,	APClusterModule] = 0;
$ItemMax[carmor3,	MineClusterModule] = 0;		     $ItemMax[cfemale3,	MineClusterModule] = 0;
$ItemMax[carmor3,	BombletClusterModule] = 0;	     $ItemMax[cfemale3,	BombletClusterModule] = 0;
$ItemMax[carmor3,	BunkerBusterModule] = 0;	     $ItemMax[cfemale3,	BunkerBusterModule] = 0;
$ItemMax[carmor3,	TankSmokeModule] = 0;		     $ItemMax[cfemale3,	TankSmokeModule] = 0;
$ItemMax[carmor3,	TankNerveGasModule] = 0;	     $ItemMax[cfemale3,	TankNerveGasModule] = 0;
$ItemMax[carmor3,	IncendiaryNapamModule] = 0;	     $ItemMax[cfemale3,	IncendiaryNapamModule] = 0;
$ItemMax[carmor3,	NuclearModule] = 0;		     $ItemMax[cfemale3,	NuclearModule] = 0;
$ItemMax[carmor3,	EMCModule] = 0;	    		     $ItemMax[cfemale3,	EMCModule] = 0;
$ItemMax[carmor3,	DaisyCutterModule] = 0;		     $ItemMax[cfemale3,	DaisyCutterModule] = 0;
$ItemMax[carmor3,	SupplyCrateModule] = 0;		     $ItemMax[cfemale3,	SupplyCrateModule] = 0;

$ItemMax[earmor3,	SideWinderModule] = 1;		     $ItemMax[efemale3,	SideWinderModule] = 1;
$ItemMax[earmor3,	PhoenixModule] = 1;		     $ItemMax[efemale3,	PhoenixModule] = 1;
$ItemMax[earmor3,	HellFireModule] = 1;		     $ItemMax[efemale3,	HellFireModule] = 1;
$ItemMax[earmor3,	HydraModule] = 1;		     $ItemMax[efemale3,	HydraModule] = 1;
$ItemMax[earmor3,	MaverickModule] = 1;		     $ItemMax[efemale3,	MaverickModule] = 1;
$ItemMax[earmor3,	HARMModule] = 1;		     $ItemMax[efemale3,	HARMModule] = 1;
$ItemMax[earmor3,	Mk82Module] = 1;		     $ItemMax[efemale3,	Mk82Module] = 1;
$ItemMax[earmor3,	Mk84Module] = 1;		     $ItemMax[efemale3,	Mk84Module] = 1;
$ItemMax[earmor3,	Mk82PackageModule] = 1;		     $ItemMax[efemale3,	Mk82PackageModule] = 1;
$ItemMax[earmor3,	Mk84PackageModule] = 1;		     $ItemMax[efemale3,	Mk84PackageModule] = 1;
$ItemMax[earmor3,	APClusterModule] = 1;		     $ItemMax[efemale3,	APClusterModule] = 1;
$ItemMax[earmor3,	MineClusterModule] = 1;		     $ItemMax[efemale3,	MineClusterModule] = 1;
$ItemMax[earmor3,	BombletClusterModule] = 1;	     $ItemMax[efemale3,	BombletClusterModule] = 1;
$ItemMax[earmor3,	BunkerBusterModule] = 1;	     $ItemMax[efemale3,	BunkerBusterModule] = 1;
$ItemMax[earmor3,	TankSmokeModule] = 1;		     $ItemMax[efemale3,	TankSmokeModule] = 1;
$ItemMax[earmor3,	TankNerveGasModule] = 1;	     $ItemMax[efemale3,	TankNerveGasModule] = 1;
$ItemMax[earmor3,	IncendiaryNapamModule] = 1;	     $ItemMax[efemale3,	IncendiaryNapamModule] = 1;
$ItemMax[earmor3,	NuclearModule] = 1;		     $ItemMax[efemale3,	NuclearModule] = 1;
$ItemMax[earmor3,	EMCModule] = 1;	    		     $ItemMax[efemale3,	EMCModule] = 1;
$ItemMax[earmor3,	DaisyCutterModule] = 1;		     $ItemMax[efemale3,	DaisyCutterModule] = 1;
$ItemMax[earmor3,	SupplyCrateModule] = 1;		     $ItemMax[efemale3,	SupplyCrateModule] = 1;

$ItemMax[larmor3,	SideWinderModule] = 0;		     $ItemMax[lfemale3,	SideWinderModule] = 0;
$ItemMax[larmor3,	PhoenixModule] = 0;		     $ItemMax[lfemale3,	PhoenixModule] = 0;
$ItemMax[larmor3,	HellFireModule] = 0;		     $ItemMax[lfemale3,	HellFireModule] = 0;
$ItemMax[larmor3,	HydraModule] = 0;		     $ItemMax[lfemale3,	HydraModule] = 0;
$ItemMax[larmor3,	MaverickModule] = 0;		     $ItemMax[lfemale3,	MaverickModule] = 0;
$ItemMax[larmor3,	HARMModule] = 0;		     $ItemMax[lfemale3,	HARMModule] = 0;
$ItemMax[larmor3,	Mk82Module] = 0;		     $ItemMax[lfemale3,	Mk82Module] = 0;
$ItemMax[larmor3,	Mk84Module] = 0;		     $ItemMax[lfemale3,	Mk84Module] = 01;
$ItemMax[larmor3,	Mk82PackageModule] = 0;		     $ItemMax[lfemale3,	Mk82PackageModule] = 0;
$ItemMax[larmor3,	Mk84PackageModule] = 0;		     $ItemMax[lfemale3,	Mk84PackageModule] = 0;
$ItemMax[larmor3,	APClusterModule] = 0;		     $ItemMax[lfemale3,	APClusterModule] = 0;
$ItemMax[larmor3,	MineClusterModule] = 0;		     $ItemMax[lfemale3,	MineClusterModule] = 0;
$ItemMax[larmor3,	BombletClusterModule] = 0;	     $ItemMax[lfemale3,	BombletClusterModule] = 0;
$ItemMax[larmor3,	BunkerBusterModule] = 0;	     $ItemMax[lfemale3,	BunkerBusterModule] = 0;
$ItemMax[larmor3,	TankSmokeModule] = 0;		     $ItemMax[lfemale3,	TankSmokeModule] = 0;
$ItemMax[larmor3,	TankNerveGasModule] = 0;	     $ItemMax[lfemale3,	TankNerveGasModule] = 0;
$ItemMax[larmor3,	IncendiaryNapamModule] = 0;	     $ItemMax[lfemale3,	IncendiaryNapamModule] = 0;
$ItemMax[larmor3,	NuclearModule] = 0;		     $ItemMax[lfemale3,	NuclearModule] = 0;
$ItemMax[larmor3,	EMCModule] = 0;	    		     $ItemMax[lfemale3,	male,	EMCModule] = 0;
$ItemMax[larmor3,	DaisyCutterModule] = 0;		     $ItemMax[lfemale3,	DaisyCutterModule] = 0;
$ItemMax[larmor3,	SupplyCrateModule] = 0;		     $ItemMax[lfemale3,	SupplyCrateModule] = 0;

$ItemMax[aarmor3,	SideWinderModule] = 0;		     $ItemMax[afemale3,	SideWinderModule] = 0;
$ItemMax[aarmor3,	PhoenixModule] = 0;		     $ItemMax[afemale3,	PhoenixModule] = 0;
$ItemMax[aarmor3,	HellFireModule] = 0;		     $ItemMax[afemale3,	HellFireModule] = 0;
$ItemMax[aarmor3,	HydraModule] = 0;		     $ItemMax[afemale3,	HydraModule] = 0;
$ItemMax[aarmor3,	MaverickModule] = 0;		     $ItemMax[afemale3,	MaverickModule] = 0;
$ItemMax[aarmor3,	HARMModule] = 0;		     $ItemMax[afemale3,	HARMModule] = 0;
$ItemMax[aarmor3,	Mk82Module] = 0;		     $ItemMax[afemale3,	Mk82Module] = 0;
$ItemMax[aarmor3,	Mk84Module] = 0;		     $ItemMax[afemale3,	Mk84Module] = 0;
$ItemMax[aarmor3,	Mk82PackageModule] = 0;		     $ItemMax[afemale3,	Mk82PackageModule] = 0;
$ItemMax[aarmor3,	Mk84PackageModule] = 0;		     $ItemMax[afemale3,	Mk84PackageModule] = 0;
$ItemMax[aarmor3,	APClusterModule] = 0;		     $ItemMax[afemale3,	APClusterModule] = 0;
$ItemMax[aarmor3,	MineClusterModule] = 0;		     $ItemMax[afemale3,	MineClusterModule] = 0;
$ItemMax[aarmor3,	BombletClusterModule] = 0;	     $ItemMax[afemale3,	BombletClusterModule] = 0;
$ItemMax[aarmor3,	BunkerBusterModule] = 0;	     $ItemMax[afemale3,	BunkerBusterModule] = 0;
$ItemMax[aarmor3,	TankSmokeModule] = 0;		     $ItemMax[afemale3,	TankSmokeModule] = 0;
$ItemMax[aarmor3,	TankNerveGasModule] = 0;	     $ItemMax[afemale3,	TankNerveGasModule] = 0;
$ItemMax[aarmor3,	IncendiaryNapamModule] = 0;	     $ItemMax[afemale3,	IncendiaryNapamModule] = 0;
$ItemMax[aarmor3,	NuclearModule] = 0;		     $ItemMax[afemale3,	NuclearModule] = 0;
$ItemMax[aarmor3,	EMCModule] = 0;	    		     $ItemMax[afemale3,	EMCModule] = 0;
$ItemMax[aarmor3,	DaisyCutterModule] = 0;		     $ItemMax[afemale3,	DaisyCutterModule] = 0;
$ItemMax[aarmor3,	SupplyCrateModule] = 0;		     $ItemMax[afemale3,	SupplyCrateModule] = 0;
