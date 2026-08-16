$ArmorType[Male, thrawnArmor] = tharmor;
$ArmorType[Female, thrawnArmor] = tharmor;
$ArmorName[tharmor] = thrawnArmor;

ItemData thrawnArmor
{
   heading = "bImperial";
	description = "Thrawn";
	className = "Armor";
	price = 0;
	team=1;
};




//----------------------------------------------------------------------------
// Thrawn Armor
//----------------------------------------------------------------------------
$DamageScale[tharmor, $LandingDamageType] = 1.0;
$DamageScale[tharmor, $ImpactDamageType] = 1.2;
$DamageScale[tharmor, $CrushDamageType] = 1.0;
$DamageScale[tharmor, $BulletDamageType] = 0.9;
$DamageScale[tharmor, $PlasmaDamageType] = 0.8;
$DamageScale[tharmor, $EnergyDamageType] = 1.2;
$DamageScale[tharmor, $ExplosionDamageType] = 1.0;
$DamageScale[tharmor, $MissileDamageType] = 0.8;
$DamageScale[tharmor, $DebrisDamageType] = 3.0;
$DamageScale[tharmor, $ShrapnelDamageType] = 1.2;
$DamageScale[tharmor, $LaserDamageType] = 1.0;
$DamageScale[tharmor, $MortarDamageType] = 1.2;
$DamageScale[tharmor, $BlasterDamageType] = 1.2;
$DamageScale[tharmor, $ElectricityDamageType] = 1.0;
$DamageScale[tharmor, $MineDamageType] = 1.2;

$DamageScale[tharmor, $AWINGDamageType] = 0.5;
$DamageScale[tharmor, $SNOWSPEEDERDamageType] = 0.5;
$DamageScale[tharmor, $TIEDamageType] = 0.5;
$DamageScale[tharmor, $TIEINTERCEPTORDamageType] = 0.5;
$DamageScale[tharmor, $TIEBOMBDamageType] = 0.8;
$DamageScale[tharmor, $YWINGDamageType] = 0.8;

$DamageScale[tharmor, $MSaberDamageType] = 0.9;
$DamageScale[tharmor, $BSaberDamageType] = 1.0;
$DamageScale[tharmor, $GSaberDamageType] = 1.0;
$DamageScale[tharmor, $RSaberDamageType] = 0.9;
$DamageScale[tharmor, $ForceDamageType] = 0.9;

$ItemMax[tharmor, RSaber] = 1;
$ItemMax[tharmor, BSaber] = 0;
$ItemMax[tharmor, GSaber] = 0;
$ItemMax[tharmor, MSaber] = 1;
$ItemMax[tharmor, Blaster] = 0;
$ItemMax[tharmor, TBlaster] = 1;
$ItemMax[tharmor, BBlaster] = 0;
$ItemMax[tharmor, HBlaster] = 1;
$ItemMax[tharmor, NBlaster] = 0;
$ItemMax[tharmor, BlasterRifle] = 1;
$ItemMax[tharmor, Repeater] = 0;
$ItemMax[tharmor, ScoutGun] = 1;
$ItemMax[tharmor, GuardGun] = 1;

$ItemMax[tharmor, DesertRifle] = 0;
$ItemMax[tharmor, MineAmmo] = 3;
$ItemMax[tharmor, TimerMineAmmo] = 3;
$ItemMax[tharmor, GuardGunAmmo] = 50;
$ItemMax[tharmor, Grenade] = 2;


$ItemMax[tharmor, DesertRifleAmmo] = 0;
$ItemMax[tharmor, BlasterAmmo] = 200;
$ItemMax[tharmor, BlasterRifleAmmo] = 50;
$ItemMax[tharmor, RepeaterAmmo] = 0;

$ItemMax[tharmor, ThermalDet] = 1;
$ItemMax[tharmor, EnergyPack] = 1;
$ItemMax[tharmor, RepairPack] = 1;
$ItemMax[tharmor, ShieldPack] = 0;

$ItemMax[tharmor, CameraPack] = 1;
$ItemMax[tharmor, TurretPack] = 1;
$ItemMax[tharmor, AmmoPack] = 0;
$ItemMax[tharmor, RepairKit] = 1;
$ItemMax[tharmor, DeployableInvPack] = 0;
$ItemMax[tharmor, DeployableAmmoPack] = 0;
$ItemMax[tharmor, ForceThrow] = 0;
$ItemMax[tharmor, ForceLightning] = 0;
$MaxWeapons[tharmor] = 3;


//------------------------------------------------------------------
// Thrawn Armor data:
//------------------------------------------------------------------

PlayerData tharmor
{
   className = "Armor";
   shapeFile = "thrawn";
   damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
   flameShapeName = "lflame";
   shieldShapeName = "shield";
   shadowDetailMask = 1;

   visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
   canCrouch = true;

   maxJetSideForceFactor = 0.8;
   maxJetForwardVelocity = 22;
   minJetEnergy = 1;
   jetForce = 230;
   jetEnergyDrain = 1.0;

	maxDamage = 0.66;
   maxForwardSpeed = 12;
   maxBackwardSpeed = 11;
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
   animData[19] = { "jet", none, 1, true, true, true, false, 3 };

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

