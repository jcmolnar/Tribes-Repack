$ArmorType[Male, RebelTrooper] = rebeltroop;
$ArmorType[Female, RebelTrooper] = rebeltroop;
$ArmorName[rebeltroop] = RebelTrooper;

ItemData RebelTrooper
{
   heading = "aRebel";
	description = "Rebel Troop";
	className = "Armor";
	price = 0;
	team=0;
};




//----------------------------------------------------------------------------
// Han Solo  Armor
//----------------------------------------------------------------------------
$DamageScale[rebeltroop, $LandingDamageType] = 1.0;
$DamageScale[rebeltroop, $ImpactDamageType] = 1.2;
$DamageScale[rebeltroop, $CrushDamageType] = 1.0;
$DamageScale[rebeltroop, $BulletDamageType] = 1.0;
$DamageScale[rebeltroop, $PlasmaDamageType] = 0.8;
$DamageScale[rebeltroop, $EnergyDamageType] = 1.2;
$DamageScale[rebeltroop, $ExplosionDamageType] = 1.0;
$DamageScale[rebeltroop, $MissileDamageType] = 0.8;
$DamageScale[rebeltroop, $DebrisDamageType] = 3.0;
$DamageScale[rebeltroop, $ShrapnelDamageType] = 1.2;
$DamageScale[rebeltroop, $LaserDamageType] = 1.0;
$DamageScale[rebeltroop, $MortarDamageType] = 1.2;
$DamageScale[rebeltroop, $BlasterDamageType] = 1.2;
$DamageScale[rebeltroop, $ElectricityDamageType] = 1.0;
$DamageScale[rebeltroop, $MineDamageType] = 1.2;

$DamageScale[rebeltroop, $AWINGDamageType] = 1.0;
$DamageScale[rebeltroop, $SNOWSPEEDERDamageType] = 1.0;
$DamageScale[rebeltroop, $TIEDamageType] = 1.0;
$DamageScale[rebeltroop, $TIEINTERCEPTORDamageType] = 1.0;
$DamageScale[rebeltroop, $TIEBOMBDamageType] = 1.3;
$DamageScale[rebeltroop, $YWINGDamageType] = 1.3;

$DamageScale[rebeltroop, $MSaberDamageType] = 1.1;
$DamageScale[rebeltroop, $BSaberDamageType] = 1.0;
$DamageScale[rebeltroop, $GSaberDamageType] = 1.0;
$DamageScale[rebeltroop, $RSaberDamageType] = 1.1;
$DamageScale[rebeltroop, $ForceDamageType] = 1.2;

$ItemMax[rebeltroop, RSaber] = 0;
$ItemMax[rebeltroop, BSaber] = 0;
$ItemMax[rebeltroop, GSaber] = 0;
$ItemMax[rebeltroop, MSaber] = 0;
$ItemMax[rebeltroop, Blaster] = 1;
$ItemMax[rebeltroop, TBlaster] = 1;
$ItemMax[rebeltroop, BBlaster] = 1;
$ItemMax[rebeltroop, HBlaster] = 1;
$ItemMax[rebeltroop, NBlaster] = 1;
$ItemMax[rebeltroop, BlasterRifle] = 1;
$ItemMax[rebeltroop, Repeater] = 0;
$ItemMax[rebeltroop, ScoutGun] = 1;
$ItemMax[rebeltroop, GuardGun] = 1;

$ItemMax[rebeltroop, DesertRifle] = 1;
$ItemMax[rebeltroop, MineAmmo] = 3;
$ItemMax[rebeltroop, TimerMineAmmo] = 3;
$ItemMax[rebeltroop, GuardGunAmmo] = 50;
$ItemMax[rebeltroop, Grenade] = 2;


$ItemMax[rebeltroop, DesertRifleAmmo] = 15;
$ItemMax[rebeltroop, BlasterAmmo] = 200;
$ItemMax[rebeltroop, BlasterRifleAmmo] = 50;
$ItemMax[rebeltroop, RepeaterAmmo] = 0;

$ItemMax[rebeltroop, ThermalDet] = 1;
$ItemMax[rebeltroop, EnergyPack] = 1;
$ItemMax[rebeltroop, RepairPack] = 1;
$ItemMax[rebeltroop, ShieldPack] = 1;

$ItemMax[rebeltroop, CameraPack] = 1;
$ItemMax[rebeltroop, TurretPack] = 1;
$ItemMax[rebeltroop, AmmoPack] = 0;
$ItemMax[rebeltroop, RepairKit] = 1;
$ItemMax[rebeltroop, DeployableInvPack] = 1;
$ItemMax[rebeltroop, DeployableAmmoPack] = 1;
$ItemMax[rebeltroop, ForceThrow] = 0;
$ItemMax[rebeltroop, ForceLightning] = 0;
$MaxWeapons[rebeltroop] = 3;


//------------------------------------------------------------------
// Rebel Pilot Armor data:
//------------------------------------------------------------------

PlayerData rebeltroop
{
   className = "Armor";
   shapeFile = "rebeltroop";
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

