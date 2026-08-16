// There is a limit on SoundData
// I commented out some turret
// sounds to free up some. If
// you add some SoundData and
// start crashing/getting
// "Invalid packet" that maybe
// why.

// Use "NoSound" for no sounds.

//----------------------------------------------------------------------------
// IMPORTANT: 3d voice profile must go first (if voices are allowed)
SoundProfileData Profile3dVoice {   baseVolume = 0;   minDistance = 10.0;   maxDistance = 70.0;   flags = SFX_IS_HARDWARE_3D; };
//----------------------------------------------------------------------------
SoundProfileData Profile2d{   baseVolume = 0.0;};SoundProfileData Profile2dLoop{   baseVolume = 0.0;   flags = SFX_IS_LOOPING;};SoundProfileData Profile3dNear{   baseVolume = 0;   minDistance = 5.0;   maxDistance = 40.0;   flags = SFX_IS_HARDWARE_3D;};SoundProfileData Profile3dMedium{   baseVolume = 0;   minDistance = 8.0;   maxDistance = 100.0;   flags = SFX_IS_HARDWARE_3D;};SoundProfileData Profile3dFar{   baseVolume = 0;   minDistance = 8.0;   maxDistance = 500.0;   flags = SFX_IS_HARDWARE_3D;};SoundProfileData Profile3dLudicrouslyFar{   baseVolume = 0;   minDistance = 2.0;   maxDistance = 700.0;   flags = SFX_IS_HARDWARE_3D;};SoundProfileData Profile3dNearLoop{   baseVolume = 0;   minDistance = 2.0;   maxDistance = 40.0;   flags = { SFX_IS_HARDWARE_3D, SFX_IS_LOOPING };};SoundProfileData Profile3dMediumLoop{   baseVolume = 0;   minDistance = 2.0;   maxDistance = 100.0;   flags = { SFX_IS_HARDWARE_3D, SFX_IS_LOOPING };};SoundProfileData Profile3dFoot{   baseVolume = 0;   minDistance = 2.0;   maxDistance = 30.0;  flags = SFX_IS_HARDWARE_3D;};
//----------------------------------------------------------------------------
// sound data
SoundData SoundLandOnGround{   wavFileName = "Land_On_Ground.wav";   profile = Profile3dNear;};
SoundData SoundJetLight{   wavFileName = "thrust.wav";   profile = Profile3dMediumLoop;};SoundData SoundJetHeavy{   wavFileName = "heavy_thrust.wav";   profile = Profile3dMediumLoop;};
SoundData SoundRain{   wavFileName = "rain.wav";   profile = Profile2dLoop;};SoundData SoundSnow{   wavFileName = "snow.wav";   profile = Profile2dLoop;};
SoundData SoundWindAmbient{   wavFileName = "wind1.wav";   profile = Profile2dLoop;};SoundData SoundWindGust{   wavFileName = "wind2.wav";   profile = Profile3dNear;};
SoundData SoundShellClick{   wavFileName = "shell_click.wav";   profile = Profile2d;};SoundData SoundShellHilight{   wavFileName = "shell_hilite.wav";   profile = Profile2d;};
SoundData SoundDoorOpen{   wavFileName = "door1.wav";   profile = Profile3dNear;};SoundData SoundDoorClose{   wavFileName = "door2.wav";   profile = Profile3dNear;};
SoundData ForceFieldOpen{   wavFileName = "ForceOpen.wav";   profile = Profile3dNear;};SoundData ForceFieldClose{   wavFileName = "ForceClose.wav";   profile = Profile3dNear;};
SoundData SoundElevatorRun{   wavFileName = "generator.wav";   profile = Profile3dNearLoop;};SoundData SoundElevatorBlocked{   wavFileName = "turret_whir.wav";   profile = Profile3dNearLoop;};SoundData SoundElevatorStart{   wavFileName = "elevator1.wav";   profile = Profile3dNear;};SoundData SoundElevatorStop{   wavFileName = "elevator2.wav";   profile = Profile3dNear;};
//----------------------------------------------------------------------------
// foot sounds
SoundData SoundLFootRSoft{   wavFileName = "pl_dirt3a.wav";   profile = Profile3dFoot;};SoundData SoundLFootRHard{   wavFileName = "pl_dirt1a.wav";   profile = Profile3dFoot;};SoundData SoundLFootRSnow{   wavFileName = "lfootrsnow.wav";   profile = Profile3dFoot;};SoundData SoundLFootLSoft{   wavFileName = "pl_dirt4a.wav";   profile = Profile3dFoot;};SoundData SoundLFootLHard{   wavFileName = "pl_dirt2a.wav";   profile = Profile3dFoot;};SoundData SoundLFootLSnow{   wavFileName = "lfootlsnow.wav";   profile = Profile3dFoot;};
SoundData SoundMFootRSoft{   wavFileName = "mfootrsoft.wav";   profile = Profile3dFoot;};SoundData SoundMFootRHard{   wavFileName = "mfootrhard.wav";   profile = Profile3dFoot;};SoundData SoundMFootRSnow{   wavFileName = "mfootrsnow.wav";   profile = Profile3dFoot;};SoundData SoundMFootLSoft{   wavFileName = "mfootlsoft.wav";   profile = Profile3dFoot;};SoundData SoundMFootLHard{   wavFileName = "mfootlhard.wav";   profile = Profile3dFoot;};SoundData SoundMFootLSnow{   wavFileName = "mfootlsnow.wav";   profile = Profile3dFoot;};
SoundData SoundHFootRSoft{   wavFileName = "hfootrsoft.wav";   profile = Profile3dFoot;};SoundData SoundHFootRHard{   wavFileName = "hfootrhard.wav";   profile = Profile3dFoot;};SoundData SoundHFootRSnow{   wavFileName = "hfootrsnow.wav";   profile = Profile3dFoot;};SoundData SoundHFootLSoft{   wavFileName = "hfootlsoft.wav";   profile = Profile3dFoot;};SoundData SoundHFootLHard{   wavFileName = "hfootlhard.wav";   profile = Profile3dFoot;};SoundData SoundHFootLSnow{   wavFileName = "hfootlsnow.wav";   profile = Profile3dFoot;};
//----------------------------------------------------------------------------
// SoundData SoundFallScream {   wavFileName = "fall_scream.wav";   profile = Profile3dNear; };
//----------------------------------------------------------------------------
// turret sound
//SoundData SoundPlasmaTurretOn{   wavFileName = "turretOn4.wav";   profile = Profile3dNear;};SoundData SoundPlasmaTurretOff{   wavFileName = "turretOff4.wav";   profile = Profile3dNear;};
//SoundData SoundPlasmaTurretFire{   wavFileName = "turretFire4.wav";   profile = Profile3dMedium;};SoundData SoundPlasmaTurretTurn{   wavFileName = "turretTurn4.wav";   profile = Profile3dNear;};
//
//SoundData SoundChainTurretOn{   wavFileName = "turretOn1.wav";   profile = Profile3dNear;};SoundData SoundChainTurretOff{   wavFileName = "turretOff1.wav";   profile = Profile3dNear;};
//SoundData SoundChainTurretTurn{   wavFileName = "turretTurn1.wav";   profile = Profile3dNear;};SoundData SoundChainTurretFire{   wavFileName = "machinegun.wav";   profile = Profile3dMedium;};
//
//SoundData SoundMissileTurretOn{   wavFileName = "turretOn1.wav";   profile = Profile3dNear;};SoundData SoundMissileTurretOff{   wavFileName = "turretOff1.wav";   profile = Profile3dNear;};
//SoundData SoundMissileTurretTurn{   wavFileName = "turretTurn1.wav";   profile = Profile3dNear;};SoundData SoundMissileTurretFire{   wavFileName = "turretFire1.wav";   profile = Profile3dMedium;};
//
//SoundData SoundMortarTurretOn{   wavFileName = "turretOn2.wav";   profile = Profile3dNear;};SoundData SoundMortarTurretOff{   wavFileName = "turretOff2.wav";   profile = Profile3dNear;};
//SoundData SoundMortarTurretTurn{   wavFileName = "turretTurn2.wav";   profile = Profile3dNear;};SoundData SoundMortarTurretFire{   wavFileName = "turretFire2.wav";   profile = Profile3dMedium;};
//
//SoundData SoundEnergyTurretOn{   wavFileName = "turretOn4.wav";   profile = Profile3dNear;};SoundData SoundEnergyTurretOff{   wavFileName = "turretOff4.wav";   profile = Profile3dNear;};
//SoundData SoundEnergyTurretTurn{   wavFileName = "turretTurn4.wav";   profile = Profile3dNear;};SoundData SoundEnergyTurretFire{   wavFileName = "rifle1.wav";   profile = Profile3dMedium;};
//
//SoundData SoundRemoteTurretOn{   wavFileName = "turretOn2.wav";   profile = Profile3dNear;};SoundData SoundRemoteTurretOff{   wavFileName = "turretOff2.wav";   profile = Profile3dNear;};
//SoundData SoundRemoteTurretTurn{   wavFileName = "turretTurn2.wav";   profile = Profile3dNear;};SoundData SoundRemoteTurretFire{   wavFileName = "rifle1.wav";   profile = Profile3dMedium;};
//----------------------------------------------------------------------------
// Item
SoundData SoundWeaponSelect{   wavFileName = "weapon5.wav";   profile = Profile3dNear;};
SoundData SoundFireBlaster{   wavFileName = "rifle1.wav";   profile = Profile3dNear;};
SoundData SoundFireChaingun{   wavFileName = "machinegun.wav";   profile = Profile3dMediumLoop;};SoundData SoundSpinUp{   wavFileName = "Machgun3.wav";   profile = Profile3dNear;};SoundData SoundSpinDown{   wavFileName = "Machgun2.wav";   profile = Profile3dNear;};
SoundData SoundDryFire{   wavFileName = "Dryfire1.wav";   profile = Profile3dNear;};
SoundData SoundFireGrenade{   wavFileName = "Grenade.wav";   profile = Profile3dNear;};
SoundData SoundFirePlasma{   wavFileName = "plasma2.wav";   profile = Profile3dNear;};
SoundData SoundSpinUpDisc{   wavFileName = "discspin.wav";   profile = Profile3dNear;};SoundData SoundFireDisc{   wavFileName = "rocket2.wav";   profile = Profile3dNear;};SoundData SoundDiscReload{   wavFileName = "discreload.wav";   profile = Profile3dNear;};SoundData SoundDiscSpin{   wavFileName = "discloop.wav";   profile = Profile3dNearLoop;};
SoundData SoundFireLaser{   wavFileName = "sniper.wav";   profile = Profile3dNear;};SoundData SoundLaserHit{   wavFileName = "laserhit.wav";   profile = Profile3dMedium;};
SoundData SoundFireTargetingLaser{   wavFileName = "tgt_laser.wav";   profile = Profile3dNearLoop;};
SoundData SoundLaserIdle{   wavFileName = "sniper2.wav";   profile = Profile3dNearLoop;};SoundData SoundTargetLaser{   wavFileName = "tgt_laser.wav";   profile = Profile3dNear;};
SoundData SoundFireMortar{   wavFileName = "mortar_fire.wav";   profile = Profile3dNear;};SoundData SoundMortarIdle{   wavFileName = "mortar_idle.wav";   profile = Profile3dNearLoop;};SoundData SoundMortarReload{   wavFileName = "mortar_reload.wav";   profile = Profile3dNearLoop;};
SoundData SoundFireSeeking{   wavFileName = "seek_fire.wav";   profile = Profile3dNear;};
SoundData SoundMineActivate{   wavFileName = "mine_act.wav";   profile = Profile3dNear;};
SoundData SoundFloatMineTarget{   wavFileName = "float_target.wav";   profile = Profile3dNear;};
SoundData SoundFireFlierRocket{	wavFileName = "flierrocket.wav";	profile = Profile3dMedium;};
SoundData SoundELFFire{	wavFileName = "elf_fire.wav";	profile = Profile3dMediumLoop;};SoundData SoundELFIdle{	wavFileName = "lightning_idle.wav";	profile = Profile3dNearLoop;};
//----------------------------------------------------------------------------
// Inventory sounds
SoundData SoundPickupItem{   wavFileName = "Pku_weap.wav";   profile = Profile3dNear;};
SoundData SoundPickupHealth{   wavFileName = "Pku_hlth.wav";   profile = Profile3dNear;};
SoundData SoundPickupBackpack{   wavFileName = "Dryfire1.wav";   profile = Profile3dNear;};
SoundData SoundPickupWeapon{   wavFileName = "Pku_weap.wav";   profile = Profile3dNear;};
SoundData SoundPickupAmmo{   wavFileName = "Pku_ammo.wav";   profile = Profile3dNear;};
SoundData SoundActivatePDA{   wavFileName = "pda_on.wav";   profile = Profile3dNear;};
SoundData SoundPDAButtonHard{   wavFileName = "button_hard.wav";   profile = Profile3dNear;};
SoundData SoundPDAButtonSoft{   wavFileName = "button_soft.wav";   profile = Profile3dNear;};
//----------------------------------------------------------------------------
// Inventory equipment
SoundData SoundActivateAmmoStation{   wavFileName = "ammo_activate.wav";   profile = Profile3dNear;};
SoundData SoundUseAmmoStation{   wavFileName = "ammo_use.wav";   profile = Profile3dNearLoop;};
SoundData SoundAmmoStationPower{   wavFileName = "ammo_power.wav";   profile = Profile3dNear;};
SoundData SoundActivateInventoryStation{   wavFileName = "inv_activate.wav";   profile = Profile3dNear;};
SoundData SoundUseInventoryStation{   wavFileName = "inv_use.wav";   profile = Profile3dNearLoop;};
SoundData SoundInventoryStationPower{   wavFileName = "inv_power.wav";   profile = Profile3dNear;};
SoundData SoundActivateCommandStation{   wavFileName = "command_activate.wav";   profile = Profile3dNear;};
SoundData SoundUseCommandStation{   wavFileName = "command_use.wav";   profile = Profile3dNearLoop;};
//----------------------------------------------------------------------------
// Item sounds
// Uncommented: these were behind // so the datablocks were undefined -> "Sound data
// block undefined" at their sequenceSound reference sites (StaticShape generators,
// Station.cs, sensor.cs). SoundCommandStationPower was missing entirely; added it.
SoundData SoundGeneratorPower{   wavFileName = "generator.wav";   profile = Profile3dNearLoop;};
SoundData SoundActivateMotionSensor{   wavFileName = "motion_activate.wav";   profile = Profile3dNear;};
SoundData SoundSensorPower{   wavFileName = "pulse_power.wav";   profile = Profile3dNearLoop;};
SoundData SoundCommandStationPower{   wavFileName = "command_power.wav";   profile = Profile3dNear;};
//SoundData SoundTeleportPower{   wavFileName = "activateTele.wav";   profile = Profile3dNearLoop;};
//SoundData SoundBeaconActive{   wavFileName = "activateBeacon.wav";   profile = Profile3dNearLoop;};SoundData SoundBeaconUse{   wavFileName = "teleport2.wav";   profile = Profile3dNear;};
SoundData SoundPackUse{   wavFileName = "usepack.wav";   profile = Profile3dNear;};SoundData SoundPackFail{   wavFileName = "failpack.wav";   profile = Profile3dNear;};
SoundData SoundThrowItem{   wavFileName = "throwitem.wav";   profile = Profile3dNear;};
//SoundData SoundShieldOn{   wavFileName = "shield_on.wav";   profile = Profile3dNearLoop;};SoundData SoundEnergyPackOn{   wavFileName = "energypackon.wav";   profile = Profile3dNearLoop;};SoundData SoundJammerOn{   wavFileName = "jammer_on.wav";   profile = Profile3dNearLoop;};
//SoundData SoundRepairItem{   wavFileName = "repair.wav";   profile = Profile3dNearLoop;};
//SoundData SoundDeploySensor{   wavFileName = "sensor_deploy.wav";   profile = Profile3dNear;};SoundData SoundActiveSensor{   wavFileName = "sensor_active.wav";   profile = Profile3dNear;};
//SoundData SoundTurretDeploy{   wavFileName = "rmt_turret.wav";   profile = Profile3dNear;};SoundData SoundRadarDeploy{   wavFileName = "rmt_radar.wav";   profile = Profile3dNear;};SoundData SoundCameraDeploy{   wavFileName = "rmt_camera.wav";   profile = Profile3dNear;};
//----------------------------------------------------------------------------
// Explosion Sounds
SoundData bigExplosion1{   wavFileName = "bxplo1.wav";   profile     = Profile3dFar;};SoundData bigExplosion2{   wavFileName = "bxplo2.wav";   profile     = Profile3dFar;};
SoundData bigExplosion3{   wavFileName = "bxplo3.wav";   profile     = Profile3dFar;};SoundData bigExplosion4{   wavFileName = "bxplo4.wav";   profile     = Profile3dFar;};
SoundData explosion3{   wavFileName = "explo3.wav";   profile     = Profile3dFar;};SoundData explosion4{   wavFileName = "explo4.wav";   profile     = Profile3dFar;};
SoundData ricochet1{   wavFileName = "ricoche1.wav";   profile     = Profile3dNear;};
SoundData ricochet2{   wavFileName = "ricoche2.wav";   profile     = Profile3dNear;}; // was undefined (only 1 and 3 existed); used by baseExpData.cs ricochet FX. ricoche2.wav ships with ricoche1/3.
SoundData SoundFireplace{   wavFileName = "fireplace.wav";   profile     = Profile3dNear;};SoundData ricochet3{   wavFileName = "ricoche3.wav";   profile     = Profile3dNear;};
SoundData energyExplosion{   wavFileName = "energyexp.wav";   profile     = Profile3dMedium;};
SoundData rocketExplosion{   wavFileName = "rockexp.wav";   profile     = Profile3dLudicrouslyFar;};
SoundData shockExplosion{   wavFileName = "shockexp.wav";   profile     = Profile3dLudicrouslyFar;};
SoundData turretExplosion{   wavFileName = "turretexp.wav";   profile     = Profile3dMedium;};
SoundData mineExplosion{   wavFileName = "mine_exp.wav";   profile     = Profile3dFar;};
SoundData floatMineExplosion{   wavFileName = "float_explode.wav";   profile     = Profile3dFar;};
SoundData debrisSmallExplosion{   wavFileName = "debris_small.wav";   profile     = Profile3dNear;};
SoundData debrisMediumExplosion{   wavFileName = "debris_medium.wav";   profile     = Profile3dMedium;};
SoundData debrisLargeExplosion{   wavFileName = "debris_large.wav";   profile     = Profile3dFar;};
//----------------------------------------------------------------------------
// Vehicle Sounds
//SoundData SoundFlyerMount { wavFileName = "flyer_mount.wav"; profile = Profile3dNear; };SoundData SoundFlyerDismount { wavFileName = "flyer_dismount.wav"; profile = Profile3dNear;};SoundData SoundFlyerActive {   wavFileName = "flyer_fly.wav";   profile = Profile3dMediumLoop;};SoundData SoundFlyerIdle {   wavFileName = "flyer_idle.wav";   profile = Profile3dMediumLoop;};SoundData SoundFlierCrash {   wavFileName = "crash.wav";   profile = Profile3dMedium; };
//SoundData SoundTankMount {   wavFileName = "flyer_mount.wav";   profile = Profile3dNear;};SoundData SoundTankDismount {   wavFileName = "flyer_dismount.wav";   profile = Profile3dNear;};SoundData SoundTankActive {   wavFileName = "flyer_fly.wav";   profile = Profile3dMediumLoop; };SoundData SoundTankIdle {   wavFileName = "flyer_idle.wav";   profile = Profile3dMediumLoop; }; SoundData SoundTankCrash {   wavFileName = "crash.wav";   profile = Profile3dMedium; };
//----------------------------------------------------------------------------
//----------------------------------------------------------------------------
// RPG sounds
//

SoundData NoSound{   wavFileName = "null.wav";   profile     = Profile3dNear;};
SoundData SoundSpawn2{   wavFileName = "RespawnC.wav";   profile = Profile3dMedium;};
SoundData SoundGrunt1{   wavFileName = "grunt1a.wav";   profile = Profile3dNear;};
SoundData SoundGrunt2{   wavFileName = "grunt3a.wav";   profile = Profile3dNear;};
SoundData SoundGrunt3{   wavFileName = "grunt3a.wav";   profile = Profile3dNear;};
//SoundData SoundHarvest1{   wavFileName = "harvest1.wav";   profile = Profile3dNearLoop;};
SoundData SoundSplash1{   wavFileName = "water3.wav";   profile = Profile3dMedium;};
SoundData SoundSwing1{   wavFileName = "swish.wav";   profile = Profile3dNear;};
SoundData SoundSwing2{   wavFileName = "swish2.wav";   profile = Profile3dNear;};
SoundData SoundSwing3{   wavFileName = "swish3.wav";   profile = Profile3dNear;};
SoundData SoundSwing4{   wavFileName = "swish4.wav";   profile = Profile3dNear;};
SoundData SoundSwing5{   wavFileName = "swish5.wav";   profile = Profile3dNear;};
SoundData SoundSwing6{   wavFileName = "swish6.wav";   profile = Profile3dNear;};
SoundData SoundSwing7{   wavFileName = "swish7.wav";   profile = Profile3dNear;};
SoundData SoundSwordHit1{   wavFileName = "hit1.wav";   profile = Profile3dNear;};
SoundData SoundArrowHit1{   wavFileName = "arrowhit.wav";   profile = Profile3dNear;};
SoundData SoundHitFlesh{   wavFileName = "Hit_Flesh.wav";   profile = Profile3dNear;};
SoundData SoundHitLeather{   wavFileName = "Hit_Leather.wav";   profile = Profile3dNear;};
SoundData SoundHitChain{   wavFileName = "Hit_Chain.wav";   profile = Profile3dNear;};
SoundData SoundHitPlate{   wavFileName = "Hit_Plate.wav";   profile = Profile3dNear;};
SoundData SoundHitShield{   wavFileName = "Hit_Shield.wav";   profile = Profile3dNear;};
SoundData SoundMoney1{   wavFileName = "money.wav";   profile = Profile3dNear;};

//=== WOT SOUNDS ================================
//explosion with little rocks at the end
SoundData ExplodeLM{   wavFileName = "ExplodeLM.wav";   profile     = Profile3dFar;};
//Power-up like sound
SoundData ActivateBF{   wavFileName = "ActivateBF.wav";   profile     = Profile3dNear;};
//6.7 second loop
SoundData LoopWA{   wavFileName = "LoopWA.wav";   profile     = Profile3dNear;};
SoundData Portal11{   wavFileName = "Portal11.wav";   profile     = Profile3dNear;};
SoundData ActivateCH{   wavFileName = "ActivateCH.wav";   profile     = Profile3dNear;};
SoundData RespawnB{   wavFileName = "RespawnB.wav";   profile     = Profile3dNear;};
SoundData ActivateAR{   wavFileName = "ActivateAR.wav";   profile     = Profile3dNear;};
SoundData DeActivateWA{   wavFileName = "DeActivateWA.wav";   profile     = Profile3dNear;};
//crossbow firing sound
SoundData CrossbowShoot1{   wavFileName = "Crossbow_Shoot1.wav";   profile     = Profile3dNear;};
//crossbow switching sound
SoundData CrossbowSwitch1{   wavFileName = "Crossbow_Switch1.wav";   profile     = Profile3dNear;};
//crossbow hitting sound
SoundData SoundArrowHit2{   wavFileName = "Crossbow_HitWall1.wav";   profile = Profile3dNear;};
//axe slashing
SoundData AxeSlash2{   wavFileName = "Axe_Slash2.wav";   profile = Profile3dNear;};
//high pitch ooooo loop
SoundData SoundGliders{   wavFileName = "Gliders.wav";   profile = Profile3dMediumLoop;};
//loud wind-like loop
SoundData SoundWindWalkers{   wavFileName = "Windy.wav";   profile = Profile3dMediumLoop;};
//boat sound
SoundData SoundBoat{   wavFileName = "AmbBoat2m.wav";   profile = Profile3dMediumLoop;};

//chee's stuff is below this line
//SoundData lairenter{   wavFileName = "lairenter.wav";   profile = Profile3dNear;};
SoundData DeActivateDG{   wavFileName = "DeActivateDG.wav";   profile = Profile3dNear;};
SoundData ultimathunder{   wavFileName = "LaunchLS.wav";   profile = Profile3dNear;};
SoundData mmsound{   wavFileName = "HitLevelDT.wav";   profile = Profile3dNear;};
SoundData spellstart{   wavFileName = "spellcast.wav";   profile = Profile3dNear;};
SoundData thunderlight{   wavFileName = "LoopLS.wav";   profile = Profile3dNear;};
SoundData cheespellsound{   wavFileName = "spellcast.wav";   profile = Profile3dNear;};
//SoundData grunt{   wavFileName = "OgreTaunt2.wav";   profile = Profile3dNear;};
//SoundData grunt2{   wavFileName = "OgreTaunt1.wav";   profile = Profile3dNear;};
SoundData lilheal{   wavFileName = "medicspell.wav";   profile = Profile3dNear;};
SoundData bigheal{   wavFileName = "medic4spell.wav";   profile = Profile3dNear;};
SoundData bigfire{   wavFileName = "LaunchET.wav";   profile = Profile3dNear;};
SoundData spooky{   wavFileName = "UndeadAcquired1.wav";   profile = Profile3dNear;};
SoundData stop{   wavFileName = "ActivateAS.wav";   profile = Profile3dNear;};
SoundData absorb{   wavFileName = "heartbeat.wav";   profile = Profile3dNear;};
SoundData heavyswing1{   wavFileName = "swish6.wav";   profile = Profile3dNear;};
SoundData watershotstart{   wavFileName = "ActivateTD.wav";   profile = Profile3dNear;};
SoundData watersplash{   wavFileName = "Water3.wav";   profile = Profile3dNear;};
//SoundData newswordactivate{   wavFileName = "UndeadAquired1.wav";   profile = Profile3dNear;};
//SoundData newswordswing{   wavFileName = "MinotaurAquired1.wav";   profile = Profile3dNear;};
//SoundData makogunfire{   wavFileName = "ActivateDE.wav";   profile = Profile3dNear;};
SoundData Ssssoo{   wavFileName = "shieldhit.wav";   profile     = Profile3dMedium;};
SoundData kapsshh{   wavFileName = "explo3.wav";   profile     = Profile3dLudicrouslyFar;};
SoundData SoundFountain{   wavFileName = "fountain.wav";   profile     = Profile3dNear;};
//SoundData SoundKeldrin{   wavFileName = "KeldrinTown.wav";   profile     = Profile3dLudicrouslyFar;};
SoundData SoundCheeShot{   wavFileName = "PortalLoop3.wav";   profile     = Profile3dNear;};
//SoundData SoundCheeBowFire{   wavFileName = "swish7.wav";   profile     = Profile3dNear;};
SoundData Summonchant{   wavFileName = "loopSP.wav";   profile     = Profile3dNear;};
//SoundData hadesgrr{   wavFileName = "UndeadDeath1.wav";   profile     = Profile3dLudicrouslyFar;};
//SoundData mypack{   wavFileName = "drums.wav";   profile     = Profile3dLudicrouslyFar;};

//-------SOUNDS FOR ENEMY SKILLS-------//
SoundData cyborgswitch{   wavFileName = "cyborgswitch.wav";   profile     = Profile3dLudicrouslyFar;};
SoundData esmissilesound{   wavFileName = "cyborgfire1.wav";   profile     = Profile3dLudicrouslyFar;};
SoundData esmissilesound2{   wavFileName = "cyborgfire2.wav";   profile     = Profile3dLudicrouslyFar;};
SoundData goblinpunchsound{   wavFileName = "punch.wav";   profile     = Profile3dLudicrouslyFar;};
SoundData goblinscreech{   wavFileName = "screech.wav";   profile     = Profile3dLudicrouslyFar;};
SoundData uuagman{   wavFileName = "uuag.wav";   profile     = Profile3dLudicrouslyFar;};
//SoundData arrowhit{   wavFileName = "arrowhit.wav";   profile     = Profile3dNear;};
//SoundData startroomambient{   wavFileName = "drums.wav";   profile     = Profile3dNearLoop;};
//SoundData deathcoilfire{   wavFileName = "AbsorbABS.wav";   profile     = Profile3dNear;};
//SoundData deathcoilactivate{   wavFileName = "UndeadRandom1.wav";   profile     = Profile3dNear;};
//SoundData zswordactivate{   wavFileName = "UndeadTaunt1.wav";   profile     = Profile3dNear;};
//SoundData doubleactivate{   wavFileName = "UndeadAcquired1.wav";   profile     = Profile3dNear;};
//SoundData doublefire{   wavFileName = "UndeadDeath1.wav";   profile     = Profile3dNear;};
//SoundData stonethrowswish{   wavFileName = "swish4.wav";   profile     = Profile3dMedium;};
//SoundData AmazonYell1{   wavFileName = "swish4.wav";   profile     = Profile3dMedium;};
//SoundData AmazonYell2{   wavFileName = "swish4.wav";   profile     = Profile3dMedium;};
//SoundData ambroseswing{   wavFileName = "ActivateAs.wav";   profile     = Profile3dLudicrouslyFar;};
SoundData ambroseactivate {   wavFileName = "ambrosesworda.wav";   profile     = Profile3dLudicrouslyFar;};
SoundData Explode3FW {   wavFileName = "Explode3FW.wav";   profile     = Profile3dMedium;};

//=======MONSTER TAUNT SOUNDS=======
//UUAG SOUNDS
SoundData SoundUuagDeath1 {	wavFileName = "OgreDeath1.wav";	profile = Profile3dNear;};
SoundData SoundUuagHit1 {	wavFileName = "uuagbark.wav";	profile = Profile3dNear;};
SoundData SoundUuagTaunt1 {	wavFileName = "uuagsnarl.wav";	profile = Profile3dNear;};
SoundData SoundUuagRandom1 {	wavFileName = "uuagroar.wav";	profile = Profile3dNear;};
//UNDEAD SOUNDS
SoundData SoundUndeadDeath1 {	wavFileName = "UndeadDeath1.wav";	profile = Profile3dNear;};
SoundData SoundUndeadAcquired1 {	wavFileName = "UndeadAcquired1.wav";	profile = Profile3dNear;};
SoundData SoundUndeadRandom1 {	wavFileName = "UndeadRandom1.wav";	profile = Profile3dNear;};
SoundData SoundUndeadTaunt1 {	wavFileName = "UndeadTaunt1.wav";	profile = Profile3dNear;};
SoundData SoundUndeadHit1 {	wavFileName = "UndeadHit1.wav";	profile = Profile3dNear;};
SoundData SoundUndeadHit2 {	wavFileName = "UndeadHit2.wav";	profile = Profile3dNear;};
//TRAVELLER SOUNDS
SoundData SoundTravellerDeath1 {	wavFileName = "TravellerDeath1.wav";	profile = Profile3dNear;};
SoundData SoundTravellerAcquired1 {	wavFileName = "TravellerAcquired1.wav";	profile = Profile3dNear;};
SoundData SoundTravellerAcquired2 {	wavFileName = "TravellerAcquired2.wav";	profile = Profile3dNear;};
SoundData SoundTravellerAcquired3 {	wavFileName = "TravellerAcquired3.wav";	profile = Profile3dNear;};
SoundData SoundTravellerHit1 {	wavFileName = "TravellerHit1.wav";	profile = Profile3dNear;};
SoundData SoundTravellerHit2 {	wavFileName = "TravellerHit2.wav";	profile = Profile3dNear;};
SoundData SoundTravellerHit3 {	wavFileName = "TravellerHit3.wav";	profile = Profile3dNear;};
//GHOST SOUNDS
SoundData SoundGhostDeath1 {	wavFileName = "evillaff4.wav";	profile = Profile3dNear;};
SoundData SoundGhostAcquired1 {	wavFileName = "ghost1.wav";	profile = Profile3dNear;};
SoundData SoundGhostAcquired2 {	wavFileName = "3ghostsmoan.wav";	profile = Profile3dNear;};
SoundData SoundGhostHit1 {	wavFileName = "boo.wav";	profile = Profile3dNear;};
SoundData SoundGhostRandom1 {	wavFileName = "boo.wav";	profile = Profile3dNear;};
//MINOTAUR SOUNDS
SoundData SoundMinotaurDeath1 {	wavFileName = "MinotaurDeath1.wav";	profile = Profile3dNear;};
SoundData SoundMinotaurAcquired1 {	wavFileName = "MinotaurAcquired1.wav";	profile = Profile3dNear;};
SoundData SoundMinotaurAcquired2 {	wavFileName = "MinotaurAcquired2.wav";	profile = Profile3dNear;};
SoundData SoundMinotaurHit1 {	wavFileName = "MinotaurHit1.wav";	profile = Profile3dNear;};
//ROCK GIANT SOUNDS
SoundData SoundRockGiantAcquired1 {	wavFileName = "rockmonstersound.wav";	profile = Profile3dNear;};
SoundData SoundRockGiantAcquired2 {	wavFileName = "rockmonstersound2.wav";	profile = Profile3dNear;};
SoundData SoundRockGiantDeath1 {	wavFileName = "rockmonsterDeath1.wav";	profile = Profile3dNear;};
//FISH SOUNDS
SoundData SoundFishAcquired1 {	wavFileName = "fish1.wav";	profile = Profile3dNear;};
SoundData SoundFishAcquired2 {	wavFileName = "fish2.wav";	profile = Profile3dNear;};
SoundData SoundFishAcquired3 {	wavFileName = "fish3.wav";	profile = Profile3dNear;};
SoundData fishywalk {	wavFileName = "fishwalk.wav";	profile = Profile3dNear;};

// Enemy footstep sounds referenced by EnemyArmors.cs rFootSounds (darkeye / godeye).
// Were undefined -> "Sound data block undefined" + silent steps. No darkeyewalk.wav /
// godeyestepsound.wav ship, so fall back to fishwalk.wav (an existing creature-move sound).
SoundData darkeyewalk {	wavFileName = "fishwalk.wav";	profile = Profile3dFoot;};
SoundData godeyestepsound {	wavFileName = "fishwalk.wav";	profile = Profile3dFoot;};
//GOBLIN SOUNDS
SoundData SoundGoblinDeath1 {	wavFileName = "GoblinDeath1.wav";	profile = Profile3dNear;};
SoundData SoundGoblinDeath2 {	wavFileName = "GoblinDeath2.wav";	profile = Profile3dNear;};
SoundData SoundGoblinAcquired1 {	wavFileName = "GoblinAcquired1.wav";	profile = Profile3dNear;};
SoundData SoundGoblinAcquired2 {	wavFileName = "GoblinAcquired2.wav";	profile = Profile3dNear;};
SoundData SoundGoblinAcquired3 {	wavFileName = "GoblinAcquired3.wav";	profile = Profile3dNear;};
SoundData SoundGoblinTaunt1 {	wavFileName = "GoblinTaunt1.wav";	profile = Profile3dNear;};
SoundData SoundGoblinRandom1 {	wavFileName = "GoblinRandom1.wav";	profile = Profile3dNear;};
SoundData SoundGoblinHit1 {	wavFileName = "GoblinHit1.wav";	profile = Profile3dNear;};
SoundData SoundGoblinHit2 {	wavFileName = "GoblinHit2.wav";	profile = Profile3dNear;};
SoundData SoundGoblinHit3 {	wavFileName = "GoblinHit3.wav";	profile = Profile3dNear;};
SoundData SoundGoblinHit4 {	wavFileName = "GoblinHit4.wav";	profile = Profile3dNear;};
SoundData SoundGoblinHit5 {	wavFileName = "GoblinHit5.wav";	profile = Profile3dNear;};
SoundData SoundGoblinHit6 {	wavFileName = "GoblinHit6.wav";	profile = Profile3dNear;};
SoundData SoundGoblinHit7 {	wavFileName = "GoblinHit7.wav";	profile = Profile3dNear;};
SoundData SoundGoblinHit8 {	wavFileName = "GoblinHit8.wav";	profile = Profile3dNear;};
//GNOLL SOUNDS
SoundData SoundGnollDeath1 {	wavFileName = "GnollDeath1.wav";	profile = Profile3dNear;};
SoundData SoundGnollDeath2 {	wavFileName = "GnollDeath2.wav";	profile = Profile3dNear;};
SoundData SoundGnollAcquired1 {	wavFileName = "GnollAcquired1.wav";	profile = Profile3dNear;};
SoundData SoundGnollTaunt1 {	wavFileName = "GnollTaunt1.wav";	profile = Profile3dNear;};
SoundData SoundGnollRandom1 {	wavFileName = "GnollRandom1.wav";	profile = Profile3dNear;};
SoundData SoundGnollRandom2{	wavFileName = "GnollRandom2.wav";	profile = Profile3dNear;};
SoundData SoundGnollHit1 {	wavFileName = "GnollHit1.wav";	profile = Profile3dNear;};
SoundData SoundGnollHit2 {	wavFileName = "GnollHit2.wav";	profile = Profile3dNear;};
//================================================

SoundData SoundLevelUp {   wavFileName = "LevelUp.wav";   profile = Profile3dNear;};
SoundData null { wavFileName = "null.wav"; profile = Profile3dMedium; };
SoundData SoundPlayerDeathMale {   wavFileName = "Death_Male.wav";   profile = Profile3dMedium;};
SoundData SoundPlayerDeathFemale {   wavFileName = "Death_Female.wav";   profile = Profile3dMedium;};
SoundData BonusStateExpire {   wavFileName = "DeActivateIC.wav";   profile     = Profile3dNear;};
//===


function RandomRaceSound(%race, %type)
{
	for(%i = 1; $RaceSound[%race, %type, %i] != ""; %i++){}
	%i--;

	%r = floor(getRandom() * %i) + 1;
	%s = $RaceSound[%race, %type, %r];

	if(%s != "")
		return %s;
	else
		return "NoSound";
}
function InitSoundPoints()
{
	dbecho($dbechoMode, "InitSoundPoints()");

	%group = nameToID("MissionGroup\\SoundPoints");

	if(%group != -1)
	{
		for(%i = 0; %i <= Group::objectCount(%group)-1; %i++)
		{
		      %this = Group::getObject(%group, %i);
			%info = Object::getName(%this);

			if(%info != "")
			{
				GameBase::playSound(%this, %info, 0);
				//echo("Playing sound " @ %info @ " for object " @ %this);
			}
		}
	}
}
