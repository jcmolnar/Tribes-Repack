$ArmorType[Male, HothArmor] = htarmor;
$ArmorType[Female, HothArmor] = htarmor;
$ArmorName[htarmor] = HothArmor;

ItemData HothArmor
{
   heading = "aRebel";
	description = "Hoth Trooper";
	className = "Armor";
	price = 0;
	team=0;
};




//----------------------------------------------------------------------------
// Han Solo  Armor
//----------------------------------------------------------------------------
$DamageScale[htarmor, $LandingDamageType] = 1.0;
$DamageScale[htarmor, $ImpactDamageType] = 1.2;
$DamageScale[htarmor, $CrushDamageType] = 1.0;
$DamageScale[htarmor, $BulletDamageType] = 1.0;
$DamageScale[htarmor, $PlasmaDamageType] = 0.8;
$DamageScale[htarmor, $EnergyDamageType] = 1.2;
$DamageScale[htarmor, $ExplosionDamageType] = 1.0;
$DamageScale[htarmor, $MissileDamageType] = 0.8;
$DamageScale[htarmor, $DebrisDamageType] = 3.0;
$DamageScale[htarmor, $ShrapnelDamageType] = 1.2;
$DamageScale[htarmor, $LaserDamageType] = 1.0;
$DamageScale[htarmor, $MortarDamageType] = 1.2;
$DamageScale[htarmor, $BlasterDamageType] = 1.2;
$DamageScale[htarmor, $ElectricityDamageType] = 1.0;
$DamageScale[htarmor, $MineDamageType] = 1.2;

$DamageScale[htarmor, $AWINGDamageType] = 1.0;
$DamageScale[htarmor, $SNOWSPEEDERDamageType] = 1.0;
$DamageScale[htarmor, $TIEDamageType] = 1.0;
$DamageScale[htarmor, $TIEINTERCEPTORDamageType] = 1.0;
$DamageScale[htarmor, $TIEBOMBDamageType] = 1.3;
$DamageScale[htarmor, $YWINGDamageType] = 1.3;

$DamageScale[htarmor, $MSaberDamageType] = 1.1;
$DamageScale[htarmor, $BSaberDamageType] = 1.0;
$DamageScale[htarmor, $GSaberDamageType] = 1.0;
$DamageScale[htarmor, $RSaberDamageType] = 1.1;
$DamageScale[htarmor, $ForceDamageType] = 1.2;

$ItemMax[htarmor, RSaber] = 0;
$ItemMax[htarmor, BSaber] = 0;
$ItemMax[htarmor, GSaber] = 0;
$ItemMax[htarmor, MSaber] = 0;
$ItemMax[htarmor, Blaster] = 0;
$ItemMax[htarmor, TBlaster] = 1;
$ItemMax[htarmor, BBlaster] = 1;
$ItemMax[htarmor, HBlaster] = 1;
$ItemMax[htarmor, NBlaster] = 1;
$ItemMax[htarmor, BlasterRifle] = 1;
$ItemMax[htarmor, Repeater] = 1;
$ItemMax[htarmor, ScoutGun] = 0;
$ItemMax[htarmor, GuardGun] = 1;

$ItemMax[htarmor, DesertRifle] = 0;
$ItemMax[htarmor, MineAmmo] = 3;
$ItemMax[htarmor, TimerMineAmmo] = 3;
$ItemMax[htarmor, GuardGunAmmo] = 50;
$ItemMax[htarmor, Grenade] = 2;


$ItemMax[htarmor, DesertRifleAmmo] = 15;
$ItemMax[htarmor, BlasterAmmo] = 200;
$ItemMax[htarmor, BlasterRifleAmmo] = 50;
$ItemMax[htarmor, RepeaterAmmo] = 200;

$ItemMax[htarmor, ThermalDet] = 1;
$ItemMax[htarmor, EnergyPack] = 1;
$ItemMax[htarmor, RepairPack] = 1;
$ItemMax[htarmor, ShieldPack] = 1;

$ItemMax[htarmor, CameraPack] = 1;
$ItemMax[htarmor, TurretPack] = 1;
$ItemMax[htarmor, AmmoPack] = 0;
$ItemMax[htarmor, RepairKit] = 1;
$ItemMax[htarmor, DeployableInvPack] = 0;
$ItemMax[htarmor, DeployableAmmoPack] = 0;
$ItemMax[htarmor, ForceThrow] = 0;
$ItemMax[htarmor, ForceLightning] = 0;
$MaxWeapons[htarmor] = 3;


//------------------------------------------------------------------
// Rebel Pilot Armor data:
//------------------------------------------------------------------

PlayerData htarmor
{
   className = "Armor";
   shapeFile = "hothtroop";
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

