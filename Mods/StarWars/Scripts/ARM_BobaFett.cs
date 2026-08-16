$ArmorType[Male, bobaarmor] = bbarmor;
$ArmorType[Female, bobaarmor] = bbarmor;
$ArmorName[bbarmor] = bobaarmor;

ItemData bobaarmor
{
   heading = "bImperial";
	description = "Boba Fett";
	className = "Armor";
	price = 0;
	team=1;
};




//----------------------------------------------------------------------------
// Boba Fett Armor
//----------------------------------------------------------------------------
$DamageScale[bbarmor, $LandingDamageType] = 1.0;
$DamageScale[bbarmor, $ImpactDamageType] = 1.2;
$DamageScale[bbarmor, $CrushDamageType] = 1.0;
$DamageScale[bbarmor, $BulletDamageType] = 1.0;
$DamageScale[bbarmor, $PlasmaDamageType] = 0.8;
$DamageScale[bbarmor, $EnergyDamageType] = 1.2;
$DamageScale[bbarmor, $ExplosionDamageType] = 1.0;
$DamageScale[bbarmor, $MissileDamageType] = 0.8;
$DamageScale[bbarmor, $DebrisDamageType] = 3.0;
$DamageScale[bbarmor, $ShrapnelDamageType] = 1.2;
$DamageScale[bbarmor, $LaserDamageType] = 1.0;
$DamageScale[bbarmor, $MortarDamageType] = 1.2;
$DamageScale[bbarmor, $BlasterDamageType] = 1.2;
$DamageScale[bbarmor, $ElectricityDamageType] = 1.0;
$DamageScale[bbarmor, $MineDamageType] = 1.2;

$DamageScale[bbarmor, $AWINGDamageType] = 1.0;
$DamageScale[bbarmor, $SNOWSPEEDERDamageType] = 1.0;
$DamageScale[bbarmor, $TIEDamageType] = 1.0;
$DamageScale[bbarmor, $TIEINTERCEPTORDamageType] = 1.0;
$DamageScale[bbarmor, $TIEBOMBDamageType] = 1.3;
$DamageScale[bbarmor, $YWINGDamageType] = 1.3;

$DamageScale[bbarmor, $MSaberDamageType] = 1.0;
$DamageScale[bbarmor, $BSaberDamageType] = 1.0;
$DamageScale[bbarmor, $GSaberDamageType] = 1.0;
$DamageScale[bbarmor, $RSaberDamageType] = 1.0;
$DamageScale[bbarmor, $ForceDamageType] = 1.2;

$ItemMax[bbarmor, RSaber] = 0;
$ItemMax[bbarmor, BSaber] = 0;
$ItemMax[bbarmor, GSaber] = 0;
$ItemMax[bbarmor, MSaber] = 0;
$ItemMax[bbarmor, Blaster] = 0;
$ItemMax[bbarmor, TBlaster] = 1;
$ItemMax[bbarmor, BBlaster] = 1;
$ItemMax[bbarmor, HBlaster] = 0;
$ItemMax[bbarmor, NBlaster] = 0;
$ItemMax[bbarmor, BlasterRifle] = 1;
$ItemMax[bbarmor, Repeater] = 0;
$ItemMax[bbarmor, ScoutGun] = 0;
$ItemMax[bbarmor, GuardGun] = 0;

$ItemMax[bbarmor, DesertRifle] = 1;
$ItemMax[bbarmor, MineAmmo] = 3;
$ItemMax[bbarmor, TimerMineAmmo] = 3;
$ItemMax[bbarmor, GuardGunAmmo] = 50;
$ItemMax[bbarmor, Grenade] = 2;


$ItemMax[bbarmor, DesertRifleAmmo] = 15;
$ItemMax[bbarmor, BlasterAmmo] = 200;
$ItemMax[bbarmor, BlasterRifleAmmo] = 50;
$ItemMax[bbarmor, RepeaterAmmo] = 200;

$ItemMax[bbarmor, ThermalDet] = 1;
$ItemMax[bbarmor, EnergyPack] = 1;
$ItemMax[bbarmor, RepairPack] = 1;
$ItemMax[bbarmor, ShieldPack] = 1;

$ItemMax[bbarmor, CameraPack] = 1;
$ItemMax[bbarmor, TurretPack] = 1;
$ItemMax[bbarmor, AmmoPack] = 0;
$ItemMax[bbarmor, RepairKit] = 1;
$ItemMax[bbarmor, DeployableInvPack] = 0;
$ItemMax[bbarmor, DeployableAmmoPack] = 0;
$ItemMax[bbarmor, ForceThrow] = 0;
$ItemMax[bbarmor, ForceLightning] = 0;
$MaxWeapons[bbarmor] = 3;


//------------------------------------------------------------------
// Thrawn Armor data:
//------------------------------------------------------------------

PlayerData bbarmor
{
   className = "Armor";
   shapeFile = "bobafett";
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
   jetEnergyDrain = 0.7;

	maxDamage = 0.66;
   maxForwardSpeed = 11;
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

