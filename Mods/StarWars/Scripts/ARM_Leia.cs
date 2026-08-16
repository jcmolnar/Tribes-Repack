$ArmorType[Male, leiaArmor] = plarmor;
$ArmorType[Female, leiaArmor] = plarmor;
$ArmorName[plarmor] = leiaArmor;

ItemData LeiaArmor
{
   heading = "aRebel";
	description = "Princess Leia";
	className = "Armor";
	price = 0;
	team=0;
};




//----------------------------------------------------------------------------
// Princess Leia Armor
//----------------------------------------------------------------------------
$DamageScale[plarmor, $LandingDamageType] = 1.0;
$DamageScale[plarmor, $ImpactDamageType] = 1.2;
$DamageScale[plarmor, $CrushDamageType] = 1.0;
$DamageScale[plarmor, $BulletDamageType] = 1.0;
$DamageScale[plarmor, $PlasmaDamageType] = 0.8;
$DamageScale[plarmor, $EnergyDamageType] = 1.2;
$DamageScale[plarmor, $ExplosionDamageType] = 1.0;
$DamageScale[plarmor, $MissileDamageType] = 0.8;
$DamageScale[plarmor, $DebrisDamageType] = 3.0;
$DamageScale[plarmor, $ShrapnelDamageType] = 1.2;
$DamageScale[plarmor, $LaserDamageType] = 1.0;
$DamageScale[plarmor, $MortarDamageType] = 1.2;
$DamageScale[plarmor, $BlasterDamageType] = 1.2;
$DamageScale[plarmor, $ElectricityDamageType] = 1.0;
$DamageScale[plarmor, $MineDamageType] = 1.2;

$DamageScale[plarmor, $AWINGDamageType] = 1.0;
$DamageScale[plarmor, $SNOWSPEEDERDamageType] = 1.0;
$DamageScale[plarmor, $TIEDamageType] = 1.0;
$DamageScale[plarmor, $TIEINTERCEPTORDamageType] = 1.0;
$DamageScale[plarmor, $TIEBOMBDamageType] = 1.3;
$DamageScale[plarmor, $YWINGDamageType] = 1.3;

$DamageScale[plarmor, $MSaberDamageType] = 1.1;
$DamageScale[plarmor, $BSaberDamageType] = 1.0;
$DamageScale[plarmor, $GSaberDamageType] = 1.0;
$DamageScale[plarmor, $RSaberDamageType] = 1.1;
$DamageScale[plarmor, $ForceDamageType] = 1.2;

$ItemMax[plarmor, RSaber] = 0;
$ItemMax[plarmor, BSaber] = 1;
$ItemMax[plarmor, GSaber] = 0;
$ItemMax[plarmor, MSaber] = 0;
$ItemMax[plarmor, Blaster] = 1;
$ItemMax[plarmor, TBlaster] = 1;
$ItemMax[plarmor, BBlaster] = 1;
$ItemMax[plarmor, HBlaster] = 0;
$ItemMax[plarmor, NBlaster] = 1;
$ItemMax[plarmor, BlasterRifle] = 1;
$ItemMax[plarmor, Repeater] = 1;
$ItemMax[plarmor, ScoutGun] = 1;
$ItemMax[plarmor, GuardGun] = 0;
$ItemMax[plarmor, TorpLauncher] = 0;

$ItemMax[plarmor, DesertRifle] = 0;
$ItemMax[plarmor, MineAmmo] = 3;
$ItemMax[plarmor, TimerMineAmmo] = 3;
$ItemMax[plarmor, GuardGunAmmo] = 50;
$ItemMax[plarmor, Grenade] = 2;

$ItemMax[plarmor, PTorpedoAmmo] = 0;
$ItemMax[plarmor, DesertRifleAmmo] = 0;
$ItemMax[plarmor, BlasterAmmo] = 200;
$ItemMax[plarmor, BlasterRifleAmmo] = 50;
$ItemMax[plarmor, RepeaterAmmo] = 200;

$ItemMax[plarmor, ThermalDet] = 1;
$ItemMax[plarmor, EnergyPack] = 1;
$ItemMax[plarmor, RepairPack] = 1;
$ItemMax[plarmor, ShieldPack] = 1;

$ItemMax[plarmor, CameraPack] = 1;
$ItemMax[plarmor, TurretPack] = 1;
$ItemMax[plarmor, AmmoPack] = 0;
$ItemMax[plarmor, RepairKit] = 1;
$ItemMax[plarmor, DeployableInvPack] = 0;
$ItemMax[plarmor, DeployableAmmoPack] = 0;
$ItemMax[plarmor, ForceThrow] = 0;
$ItemMax[plarmor, ForceLightning] = 0;
$MaxWeapons[plarmor] = 3;


//------------------------------------------------------------------
// Princess Leia Armor data:
//------------------------------------------------------------------

PlayerData plarmor
{
   className = "Armor";
   shapeFile = "pleia";
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";

   canCrouch = true;
   maxJetSideForceFactor = 0.8;
   maxJetForwardVelocity = 24;
   minJetEnergy = 1;
   jetForce = 236;
   jetEnergyDrain = 0.8;

	maxDamage = 0.66;
   maxForwardSpeed = 12;
   maxBackwardSpeed = 10;
   maxSideSpeed = 10;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundTraction = 3.0;
	maxEnergy = 60;
   drag = 1.0;
   density = 1.2;

	minDamageSpeed = 25;
	damageScale = 0.005;

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
   animData[19] = { "jet", none, 1, true, true, true, false, 3 };

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

   boxNormalHeadPercentage  = 0.85;
   boxNormalTorsoPercentage = 0.53;
   boxCrouchHeadPercentage  = 0.88;
   boxCrouchTorsoPercentage = 0.35;

   boxHeadLeftPercentage  = 0;
   boxHeadRightPercentage = 1;
   boxHeadBackPercentage  = 0;
   boxHeadFrontPercentage = 1;
};