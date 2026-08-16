$ArmorType[Male, xenaArmor] = xarmor;
$ArmorType[Female, xenaArmor] = xarmor;
$ArmorName[xarmor] = xenaArmor;

ItemData xenaArmor
{
   heading = "bImperial";
	description = "Xena";
	className = "Armor";
	price = 0;
	team=1;
};




//----------------------------------------------------------------------------
// Xena Armor
//----------------------------------------------------------------------------
$DamageScale[xarmor, $LandingDamageType] = 1.0;
$DamageScale[xarmor, $ImpactDamageType] = 1.2;
$DamageScale[xarmor, $CrushDamageType] = 1.0;
$DamageScale[xarmor, $BulletDamageType] = 1.0;
$DamageScale[xarmor, $PlasmaDamageType] = 0.8;
$DamageScale[xarmor, $EnergyDamageType] = 1.2;
$DamageScale[xarmor, $ExplosionDamageType] = 1.0;
$DamageScale[xarmor, $MissileDamageType] = 0.8;
$DamageScale[xarmor, $DebrisDamageType] = 3.0;
$DamageScale[xarmor, $ShrapnelDamageType] = 1.2;
$DamageScale[xarmor, $LaserDamageType] = 1.0;
$DamageScale[xarmor, $MortarDamageType] = 1.2;
$DamageScale[xarmor, $BlasterDamageType] = 1.2;
$DamageScale[xarmor, $ElectricityDamageType] = 1.0;
$DamageScale[xarmor, $MineDamageType] = 1.2;

$DamageScale[xarmor, $AWINGDamageType] = 1.0;
$DamageScale[xarmor, $SNOWSPEEDERDamageType] = 1.0;
$DamageScale[xarmor, $TIEDamageType] = 1.0;
$DamageScale[xarmor, $TIEINTERCEPTORDamageType] = 1.0;
$DamageScale[xarmor, $TIEBOMBDamageType] = 1.3;
$DamageScale[xarmor, $YWINGDamageType] = 1.3;

$DamageScale[xarmor, $MSaberDamageType] = 1.0;
$DamageScale[xarmor, $BSaberDamageType] = 1.0;
$DamageScale[xarmor, $GSaberDamageType] = 1.0;
$DamageScale[xarmor, $RSaberDamageType] = 1.0;
$DamageScale[xarmor, $ForceDamageType] = 1.2;

$ItemMax[xarmor, RSaber] = 1;
$ItemMax[xarmor, BSaber] = 0;
$ItemMax[xarmor, GSaber] = 0;
$ItemMax[xarmor, MSaber] = 0;
$ItemMax[xarmor, Blaster] = 0;
$ItemMax[xarmor, TBlaster] = 1;
$ItemMax[xarmor, BBlaster] = 1;
$ItemMax[xarmor, HBlaster] = 0;
$ItemMax[xarmor, NBlaster] = 0;
$ItemMax[xarmor, BlasterRifle] = 1;
$ItemMax[xarmor, Repeater] = 1;
$ItemMax[xarmor, ScoutGun] = 0;
$ItemMax[xarmor, GuardGun] = 0;

$ItemMax[xarmor, DesertRifle] = 0;
$ItemMax[xarmor, MineAmmo] = 3;
$ItemMax[xarmor, TimerMineAmmo] = 3;
$ItemMax[xarmor, GuardGunAmmo] = 50;
$ItemMax[xarmor, Grenade] = 2;

$ItemMax[xarmor, DesertRifleAmmo] = 0;
$ItemMax[xarmor, BlasterAmmo] = 200;
$ItemMax[xarmor, BlasterRifleAmmo] = 50;
$ItemMax[xarmor, RepeaterAmmo] = 200;

$ItemMax[xarmor, ThermalDet] = 1;
$ItemMax[xarmor, EnergyPack] = 1;
$ItemMax[xarmor, RepairPack] = 1;
$ItemMax[xarmor, ShieldPack] = 1;

$ItemMax[xarmor, CameraPack] = 1;
$ItemMax[xarmor, TurretPack] = 1;
$ItemMax[xarmor, AmmoPack] = 0;
$ItemMax[xarmor, RepairKit] = 1;
$ItemMax[xarmor, DeployableInvPack] = 0;
$ItemMax[xarmor, DeployableAmmoPack] = 0;
$ItemMax[xarmor, ForceThrow] = 1;
$ItemMax[xarmor, ForceLightning] = 0;
$MaxWeapons[xarmor] = 3;


//------------------------------------------------------------------
// Xena Armor data:
//------------------------------------------------------------------

PlayerData xarmor
{
   className = "Armor";
   shapeFile = "xenaimp";
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
