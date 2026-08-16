$ArmorType[Male, SLukeArmor] = Sluarmor;
$ArmorType[Female, SLukeArmor] = Sluarmor;
$ArmorName[Sluarmor] = SLukeArmor;



//----------------------------------------------------------------------------
// Force Speed Luke Skywalker  Armor
//----------------------------------------------------------------------------
$DamageScale[Sluarmor, $LandingDamageType] = 1.0;
$DamageScale[Sluarmor, $ImpactDamageType] = 1.2;
$DamageScale[Sluarmor, $CrushDamageType] = 1.0;
$DamageScale[Sluarmor, $BulletDamageType] = 1.0;
$DamageScale[Sluarmor, $PlasmaDamageType] = 0.8;
$DamageScale[Sluarmor, $EnergyDamageType] = 1.2;
$DamageScale[Sluarmor, $ExplosionDamageType] = 1.0;
$DamageScale[Sluarmor, $MissileDamageType] = 0.8;
$DamageScale[Sluarmor, $DebrisDamageType] = 3.0;
$DamageScale[Sluarmor, $ShrapnelDamageType] = 1.2;
$DamageScale[Sluarmor, $LaserDamageType] = 1.0;
$DamageScale[Sluarmor, $MortarDamageType] = 1.2;
$DamageScale[Sluarmor, $BlasterDamageType] = 1.2;
$DamageScale[Sluarmor, $ElectricityDamageType] = 0.9;
$DamageScale[Sluarmor, $MineDamageType] = 1.2;

$DamageScale[Sluarmor, $AWINGDamageType] = 1.0;
$DamageScale[Sluarmor, $SNOWSPEEDERDamageType] = 1.0;
$DamageScale[Sluarmor, $TIEDamageType] = 0.5;
$DamageScale[Sluarmor, $TIEINTERCEPTORDamageType] = 0.5;
$DamageScale[Sluarmor, $TIEBOMBDamageType] = 0.8;
$DamageScale[Sluarmor, $YWINGDamageType] = 0.8;

$DamageScale[Sluarmor, $MSaberDamageType] = 1.0;
$DamageScale[Sluarmor, $BSaberDamageType] = 0.9;
$DamageScale[Sluarmor, $GSaberDamageType] = 0.9;
$DamageScale[Sluarmor, $RSaberDamageType] = 1.0;
$DamageScale[Sluarmor, $ForceDamageType] = 0.8;

$ItemMax[Sluarmor, RSaber] = 1;
$ItemMax[Sluarmor, BSaber] = 1;
$ItemMax[Sluarmor, GSaber] = 1;
$ItemMax[Sluarmor, MSaber] = 1;
$ItemMax[Sluarmor, Blaster] = 1;
$ItemMax[Sluarmor, TBlaster] = 1;
$ItemMax[Sluarmor, BBlaster] = 1;
$ItemMax[Sluarmor, HBlaster] = 1;
$ItemMax[Sluarmor, NBlaster] = 1;
$ItemMax[Sluarmor, BlasterRifle] = 1;
$ItemMax[Sluarmor, Repeater] = 0;
$ItemMax[Sluarmor, ScoutGun] = 1;

$ItemMax[Sluarmor, DesertRifle] = 0;
$ItemMax[Sluarmor, MineAmmo] = 3;
$ItemMax[Sluarmor, TimerMineAmmo] = 3;
$ItemMax[Sluarmor, Grenade] = 2;


$ItemMax[Sluarmor, DesertRifleAmmo] = 0;
$ItemMax[Sluarmor, BlasterAmmo] = 200;
$ItemMax[Sluarmor, BlasterRifleAmmo] = 50;
$ItemMax[Sluarmor, RepeaterAmmo] = 0;

$ItemMax[Sluarmor, ThermalDet] = 1;
$ItemMax[Sluarmor, EnergyPack] = 1;
$ItemMax[Sluarmor, RepairPack] = 1;
$ItemMax[Sluarmor, ShieldPack] = 0;

$ItemMax[Sluarmor, CameraPack] = 1;
$ItemMax[Sluarmor, TurretPack] = 1;
$ItemMax[Sluarmor, AmmoPack] = 0;
$ItemMax[Sluarmor, RepairKit] = 1;
$ItemMax[Sluarmor, DeployableInvPack] = 0;
$ItemMax[Sluarmor, DeployableAmmoPack] = 0;
$ItemMax[Sluarmor, ForceThrow] = 1;
$ItemMax[Sluarmor, ForceLightning] = 1;
$ItemMax[Sluarmor, ForceSpeed] = 1;
$MaxWeapons[Sluarmor] = 3;


//------------------------------------------------------------------
// Luke Skywalker Armor data:
//------------------------------------------------------------------

PlayerData sluarmor
{
   className = "Armor";
   shapeFile = "lukesw";
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
   maxJetForwardVelocity = 30;
   minJetEnergy = 0;
   jetForce = 0;
   jetEnergyDrain = 0.0;

	maxDamage = 0.66;
   maxForwardSpeed = 40;
   maxBackwardSpeed = 40;
   maxSideSpeed = 30;
   groundForce = 40 * 9.0;
   mass = 9.0;
   groundTraction = 6.0;
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

//   jetSound = SoundJetLight;
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

