$ArmorType[Male, RebelCommando] = rebelCommand;
$ArmorType[Female, RebelCommando] = rebelCommand;
$ArmorName[rebelCommand] = RebelCommando;

ItemData RebelCommando
{
   heading = "aRebel";
	description = "Rebel Command";
	className = "Armor";
	price = 0;
	team=0;
};




//----------------------------------------------------------------------------
// Han Solo  Armor
//----------------------------------------------------------------------------
$DamageScale[rebelCommand, $LandingDamageType] = 1.0;
$DamageScale[rebelCommand, $ImpactDamageType] = 1.2;
$DamageScale[rebelCommand, $CrushDamageType] = 1.0;
$DamageScale[rebelCommand, $BulletDamageType] = 1.0;
$DamageScale[rebelCommand, $PlasmaDamageType] = 0.8;
$DamageScale[rebelCommand, $EnergyDamageType] = 1.2;
$DamageScale[rebelCommand, $ExplosionDamageType] = 1.0;
$DamageScale[rebelCommand, $MissileDamageType] = 0.8;
$DamageScale[rebelCommand, $DebrisDamageType] = 3.0;
$DamageScale[rebelCommand, $ShrapnelDamageType] = 1.2;
$DamageScale[rebelCommand, $LaserDamageType] = 1.0;
$DamageScale[rebelCommand, $MortarDamageType] = 1.2;
$DamageScale[rebelCommand, $BlasterDamageType] = 1.2;
$DamageScale[rebelCommand, $ElectricityDamageType] = 1.0;
$DamageScale[rebelCommand, $MineDamageType] = 1.2;

$DamageScale[rebelCommand, $AWINGDamageType] = 1.0;
$DamageScale[rebelCommand, $SNOWSPEEDERDamageType] = 1.0;
$DamageScale[rebelCommand, $TIEDamageType] = 1.0;
$DamageScale[rebelCommand, $TIEINTERCEPTORDamageType] = 1.0;
$DamageScale[rebelCommand, $TIEBOMBDamageType] = 1.3;
$DamageScale[rebelCommand, $YWINGDamageType] = 1.3;

$DamageScale[rebelCommand, $MSaberDamageType] = 1.1;
$DamageScale[rebelCommand, $BSaberDamageType] = 1.0;
$DamageScale[rebelCommand, $GSaberDamageType] = 1.0;
$DamageScale[rebelCommand, $RSaberDamageType] = 1.0;
$DamageScale[rebelCommand, $ForceDamageType] = 1.1;

$ItemMax[rebelCommand, RSaber] = 0;
$ItemMax[rebelCommand, BSaber] = 0;
$ItemMax[rebelCommand, GSaber] = 0;
$ItemMax[rebelCommand, MSaber] = 0;
$ItemMax[rebelCommand, Blaster] = 1;
$ItemMax[rebelCommand, TBlaster] = 1;
$ItemMax[rebelCommand, BBlaster] = 1;
$ItemMax[rebelCommand, HBlaster] = 1;
$ItemMax[rebelCommand, NBlaster] = 1;
$ItemMax[rebelCommand, BlasterRifle] = 1;
$ItemMax[rebelCommand, Repeater] = 0;
$ItemMax[rebelCommand, ScoutGun] = 1;
$ItemMax[rebelCommand, GuardGun] = 1;
$ItemMax[rebelCommand, TorpLauncher] = 0;
$ItemMax[rebelCommand, FlakCannon] = 1;

$ItemMax[rebelCommand, DesertRifle] = 1;
$ItemMax[rebelCommand, MineAmmo] = 3;
$ItemMax[rebelCommand, TimerMineAmmo] = 3;
$ItemMax[rebelCommand, GuardGunAmmo] = 50;
$ItemMax[rebelCommand, FlakAmmo] = 50;
$ItemMax[rebelCommand, Grenade] = 2;

$ItemMax[rebleCommand, PTorpedoAmmo] = 0;
$ItemMax[rebelCommand, DesertRifleAmmo] = 15;
$ItemMax[rebelCommand, BlasterAmmo] = 200;
$ItemMax[rebelCommand, BlasterRifleAmmo] = 50;
$ItemMax[rebelCommand, RepeaterAmmo] = 0;

$ItemMax[rebelCommand, ThermalDet] = 1;
$ItemMax[rebelCommand, EnergyPack] = 1;
$ItemMax[rebelCommand, RepairPack] = 1;
$ItemMax[rebelCommand, ShieldPack] = 1;

$ItemMax[rebelCommand, CameraPack] = 1;
$ItemMax[rebelCommand, TurretPack] = 1;
$ItemMax[rebelCommand, AmmoPack] = 0;
$ItemMax[rebelCommand, RepairKit] = 1;
$ItemMax[rebelCommand, DeployableInvPack] = 0;
$ItemMax[rebelCommand, DeployableAmmoPack] = 0;
$ItemMax[rebelCommand, ForceThrow] = 0;
$ItemMax[rebelCommand, ForceLightning] = 0;
$MaxWeapons[rebelCommand] = 3;


//------------------------------------------------------------------
// Rebel Pilot Armor data:
//------------------------------------------------------------------

PlayerData rebelCommand
{
   className = "Armor";
   shapeFile = "rebelCommand";
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

