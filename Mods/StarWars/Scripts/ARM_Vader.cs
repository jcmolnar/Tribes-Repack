$ArmorType[Male, VaderArmor] = dvarmor;
$ArmorType[Female, VaderArmor] = dvarmor;
$ArmorName[dvarmor] = VaderArmor;

ItemData VaderArmor
{
   heading = "bImperial";
	description = "Darth Vader";
	className = "Armor";
	price = 0;
	team=1;
};




//----------------------------------------------------------------------------
// Darth Vader Armor
//----------------------------------------------------------------------------
$DamageScale[dvarmor, $LandingDamageType] = 0.8;
$DamageScale[dvarmor, $ImpactDamageType] = 1.1;
$DamageScale[dvarmor, $CrushDamageType] = 1.0;
$DamageScale[dvarmor, $BulletDamageType] = 1.1;
$DamageScale[dvarmor, $PlasmaDamageType] = 0.8;
$DamageScale[dvarmor, $EnergyDamageType] = 1.2;
$DamageScale[dvarmor, $ExplosionDamageType] = 1.0;
$DamageScale[dvarmor, $MissileDamageType] = 0.8;
$DamageScale[dvarmor, $DebrisDamageType] = 3.0;
$DamageScale[dvarmor, $ShrapnelDamageType] = 1.2;
$DamageScale[dvarmor, $LaserDamageType] = 1.0;
$DamageScale[dvarmor, $MortarDamageType] = 1.2;
$DamageScale[dvarmor, $BlasterDamageType] = 1.1;
$DamageScale[dvarmor, $ElectricityDamageType] = 1.1;
$DamageScale[dvarmor, $MineDamageType] = 1.2;

$DamageScale[dvarmor, $AWINGDamageType] = 1.0;
$DamageScale[dvarmor, $SNOWSPEEDERDamageType] = 1.0;
$DamageScale[dvarmor, $TIEDamageType] = 1.0;
$DamageScale[dvarmor, $TIEINTERCEPTORDamageType] = 1.0;
$DamageScale[dvarmor, $TIEBOMBDamageType] = 1.3;
$DamageScale[dvarmor, $YWINGDamageType] = 1.3;

$DamageScale[dvarmor, $MSaberDamageType] = 0.8;
$DamageScale[dvarmor, $BSaberDamageType] = 0.8;
$DamageScale[dvarmor, $GSaberDamageType] = 0.8;
$DamageScale[dvarmor, $RSaberDamageType] = 0.8;
$DamageScale[dvarmor, $ForceDamageType] = 0.8;

$ItemMax[dvarmor, RSaber] = 1;
$ItemMax[dvarmor, BSaber] = 0;
$ItemMax[dvarmor, GSaber] = 0;
$ItemMax[dvarmor, MSaber] = 0;
$ItemMax[dvarmor, Blaster] = 0;
$ItemMax[dvarmor, TBlaster] = 0;
$ItemMax[dvarmor, BBlaster] = 0;
$ItemMax[dvarmor, HBlaster] = 1;
$ItemMax[dvarmor, NBlaster] = 0;
$ItemMax[dvarmor, BlasterRifle] = 0;
$ItemMax[dvarmor, Repeater] = 0;
$ItemMax[dvarmor, FlakCannon] = 0;
$ItemMax[dvarmor, TorpLauncher] = 0;
$ItemMax[dvarmor, GuardGun] = 0;

$ItemMax[dvarmor, DesertRifle] = 0;
$ItemMax[dvarmor, MineAmmo] = 5;
$ItemMax[dvarmor, TimerMineAmmo] = 5;
$ItemMax[dvarmor, FlakAmmo] = 50;
$ItemMax[dvarmor, GuardGunAmmo] = 50;

$ItemMax[dvarmor, Grenade] = 4;

$ItemMax[dvarmor, PTorpedoAmmo] = 6;
$ItemMax[dvarmor, DesertRifleAmmo] = 15;
$ItemMax[dvarmor, BlasterAmmo] = 200;
$ItemMax[dvarmor, BlasterRifleAmmo] = 50;
$ItemMax[dvarmor, RepeaterAmmo] = 200;

$ItemMax[dvarmor, ThermalDet] = 2;
$ItemMax[dvarmor, EnergyPack] = 1;
$ItemMax[dvarmor, RepairPack] = 1;
$ItemMax[dvarmor, ShieldPack] = 0;

$ItemMax[dvarmor, CameraPack] = 1;
$ItemMax[dvarmor, TurretPack] = 1;
$ItemMax[dvarmor, AmmoPack] = 0;
$ItemMax[dvarmor, RepairKit] = 1;
$ItemMax[dvarmor, DeployableInvPack] = 1;
$ItemMax[dvarmor, DeployableAmmoPack] = 1;
$ItemMax[dvarmor, ForceThrow] = 1;
$ItemMax[dvarmor, ForceLightning] = 1;
$MaxWeapons[dvarmor] = 4;


//------------------------------------------------------------------
// Dark Trooper Armor data:
//------------------------------------------------------------------

PlayerData dvarmor
{
   className = "Armor";
   shapeFile = "dvader";
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
   maxJetForwardVelocity = 15;
   minJetEnergy = 1;
   jetForce = 200;
   jetEnergyDrain = 1.0;

	maxDamage = 1.3;
   maxForwardSpeed = 10;
   maxBackwardSpeed = 9;
   maxSideSpeed = 8;
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
   animData[42] = { "sign sadtt", none, 1, true, false, false, true, 1 }; 


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

