//This file is part of Tribes RPG.
//Tribes RPG server side scripts
//Written by Jason "phantom" Daley,  Matthiew "JeremyIrons" Bouchard, and more (yet undetermined)

//	Copyright (C) 2014  Jason Daley

//	This program is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.

//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.

//	You should have received a copy of the GNU General Public License
//	along with this program.  If not, see <http://www.gnu.org/licenses/>.

//You may contact the author at beatme101@gmail.com or www.tribesrpg.org/contact.php

//This GPL does not apply to Starsiege: Tribes or any non-RPG related files included.
//Starsiege: Tribes, including the engine, retains a proprietary license forbidding resale.



//----------------------------------------------------------------------------
// IMPORTANT: 3d voice profile must go first (if voices are allowed)
SoundProfileData Profile3dVoice
{
   baseVolume = 0;
   minDistance = 10.0;
   maxDistance = 70.0;
   flags = SFX_IS_HARDWARE_3D;
};
//----------------------------------------------------------------------------

SoundProfileData Profile2d
{
   baseVolume = 0.0;
};

SoundProfileData Profile2dLoop
{
   baseVolume = 0.0;
   flags = SFX_IS_LOOPING;
};

SoundProfileData Profile3dNear
{
   baseVolume = 0;
   minDistance = 5.0;
   maxDistance = 40.0;
   flags = SFX_IS_HARDWARE_3D;
};

SoundProfileData Profile3dMedium
{
   baseVolume = 0;
   minDistance = 8.0;
   maxDistance = 100.0;
   flags = SFX_IS_HARDWARE_3D;
};

SoundProfileData Profile3dFar
{
   baseVolume = 0;
   minDistance = 8.0;
   maxDistance = 500.0;
   flags = SFX_IS_HARDWARE_3D;
};

SoundProfileData Profile3dLudicrouslyFar
{
   baseVolume = 0;
   minDistance = 2.0;
   maxDistance = 700.0;
   flags = SFX_IS_HARDWARE_3D;
};

SoundProfileData Profile3dNearLoop
{
   baseVolume = 0;
   minDistance = 2.0;
   maxDistance = 40.0;
   flags = { SFX_IS_HARDWARE_3D, SFX_IS_LOOPING };
};

SoundProfileData Profile3dMediumLoop
{
   baseVolume = 0;
   minDistance = 2.0;
   maxDistance = 100.0;
   flags = { SFX_IS_HARDWARE_3D, SFX_IS_LOOPING };
};

SoundProfileData Profile3dFoot
{
   baseVolume = 0;
   minDistance = 2.0;
   maxDistance = 30.0;
   flags = SFX_IS_HARDWARE_3D;
};

//RPG
SoundProfileData Profile3dFarLoop
{
   baseVolume = 0;
   minDistance = 2.0;
   maxDistance = 500.0;
   flags = { SFX_IS_HARDWARE_3D, SFX_IS_LOOPING };
};
SoundProfileData Profile3dVeryFarLoop
{
   baseVolume = 0;
   minDistance = 2.0;
   maxDistance = 1000.0;
   flags = { SFX_IS_HARDWARE_3D, SFX_IS_LOOPING };
};
SoundProfileData Profile3dVeryVeryFarLoop
{
   baseVolume = 0;
   minDistance = 2.0;
   maxDistance = 2500.0;
   flags = { SFX_IS_HARDWARE_3D, SFX_IS_LOOPING };
};

//----------------------------------------------------------------------------
// sound data

SoundData SoundLandOnGround
{
   wavFileName = "Land_On_Ground.wav";
   profile = Profile3dNear;
};

SoundData SoundPlayerDeath
{
   wavFileName = "player_death.wav";
   profile = Profile3dMedium;
};

SoundData SoundJetLight
{
   wavFileName = "thrust.wav";
   profile = Profile3dMediumLoop;
};

SoundData SoundJetHeavy
{
   wavFileName = "heavy_thrust.wav";
   profile = Profile3dMediumLoop;
};

SoundData SoundRain
{
   wavFileName = "rain.wav";
   profile = Profile2dLoop;
};

SoundData SoundSnow
{
   wavFileName = "snow.wav";
   profile = Profile2dLoop;
};

SoundData SoundWindAmbient
{
   wavFileName = "wind1.wav";
   profile = Profile2dLoop;
};

SoundData SoundWindGust
{
   wavFileName = "wind2.wav";
   profile = Profile3dNear;
};

SoundData SoundShellClick
{
   wavFileName = "shell_click.wav";
   profile = Profile2d;
};

SoundData SoundShellHilight
{
   wavFileName = "shell_hilite.wav";
   profile = Profile2d;
};

SoundData SoundDoorOpen
{
   wavFileName = "door1.wav";
   profile = Profile3dNear;
};

SoundData SoundDoorClose
{
   wavFileName = "door2.wav";
   profile = Profile3dNear;
};

SoundData ForceFieldOpen
{
   wavFileName = "ForceOpen.wav";
   profile = Profile3dNear;
};

SoundData ForceFieldClose
{
   wavFileName = "ForceClose.wav";
   profile = Profile3dNear;
};

SoundData SoundElevatorRun
{
   wavFileName = "generator.wav";
   profile = Profile3dNearLoop;
};

SoundData SoundElevatorBlocked
{
   wavFileName = "turret_whir.wav";
   profile = Profile3dNearLoop;
};

SoundData SoundElevatorStart
{
   wavFileName = "elevator1.wav";
   profile = Profile3dNear;
};

SoundData SoundElevatorStop
{
   wavFileName = "elevator2.wav";
   profile = Profile3dNear;
};

//----------------------------------------------------------------------------
// foot sounds

SoundData SoundLFootRSoft
{
   wavFileName = "footsoft1.wav";
   profile = Profile3dFoot;
};

SoundData SoundLFootRHard
{
   wavFileName = "foothard1.wav";
   profile = Profile3dFoot;
};

SoundData SoundLFootRSnow
{
   wavFileName = "lfootrsnow.wav";
   profile = Profile3dFoot;
};

SoundData SoundLFootLSoft
{
   wavFileName = "footsoft2.wav";
   profile = Profile3dFoot;
};

SoundData SoundLFootLHard
{
   wavFileName = "foothard2.wav";
   profile = Profile3dFoot;
};

SoundData SoundLFootLSnow
{
   wavFileName = "lfootlsnow.wav";
   profile = Profile3dFoot;
};


SoundData SoundMFootRSoft
{
   wavFileName = "mfootrsoft.wav";
   profile = Profile3dFoot;
};

SoundData SoundMFootRHard
{
   wavFileName = "mfootrhard.wav";
   profile = Profile3dFoot;
};

SoundData SoundMFootRSnow
{
   wavFileName = "mfootrsnow.wav";
   profile = Profile3dFoot;
};

SoundData SoundMFootLSoft
{
   wavFileName = "mfootlsoft.wav";
   profile = Profile3dFoot;
};

SoundData SoundMFootLHard
{
   wavFileName = "mfootlhard.wav";
   profile = Profile3dFoot;
};

SoundData SoundMFootLSnow
{
   wavFileName = "mfootlsnow.wav";
   profile = Profile3dFoot;
};


SoundData SoundHFootRSoft
{
   wavFileName = "hfootrsoft.wav";
   profile = Profile3dFoot;
};

SoundData SoundHFootRHard
{
   wavFileName = "hfootrhard.wav";
   profile = Profile3dFoot;
};

SoundData SoundHFootRSnow
{
   wavFileName = "hfootrsnow.wav";
   profile = Profile3dFoot;
};

SoundData SoundHFootLSoft
{
   wavFileName = "hfootlsoft.wav";
   profile = Profile3dFoot;
};

SoundData SoundHFootLHard
{
   wavFileName = "hfootlhard.wav";
   profile = Profile3dFoot;
};

SoundData SoundHFootLSnow
{
   wavFileName = "hfootlsnow.wav";
   profile = Profile3dFoot;
};

//----------------------------------------------------------------------------

// SoundData SoundFallScream
// {
//   wavFileName = "fall_scream.wav";
//   profile = Profile3dNear;
// };

//----------------------------------------------------------------------------
// turret sound

//SoundData SoundPlasmaTurretOn
//{
//   wavFileName = "turretOn4.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundPlasmaTurretOff
//{
//   wavFileName = "turretOff4.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundPlasmaTurretFire
//{
//   wavFileName = "turretFire4.wav";
//   profile = Profile3dMedium;
//};

//SoundData SoundPlasmaTurretTurn
//{
//   wavFileName = "turretTurn4.wav";
//   profile = Profile3dNear;
//};


//
//SoundData SoundChainTurretOn
//{
//   wavFileName = "turretOn1.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundChainTurretOff
//{
//   wavFileName = "turretOff1.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundChainTurretTurn
//{
//   wavFileName = "turretTurn1.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundChainTurretFire
//{
//   wavFileName = "machinegun.wav";
//   profile = Profile3dMedium;
//};

//
//SoundData SoundMissileTurretOn
//{
//   wavFileName = "turretOn1.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundMissileTurretOff
//{
//   wavFileName = "turretOff1.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundMissileTurretTurn
//{
//   wavFileName = "turretTurn1.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundMissileTurretFire
//{
//   wavFileName = "turretFire1.wav";
//   profile = Profile3dMedium;
//};

//
//SoundData SoundMortarTurretOn
//{
//   wavFileName = "turretOn2.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundMortarTurretOff
//{
//   wavFileName = "turretOff2.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundMortarTurretTurn
//{
//   wavFileName = "turretTurn2.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundMortarTurretFire
//{
//   wavFileName = "turretFire2.wav";
//   profile = Profile3dMedium;
//};

//
//SoundData SoundEnergyTurretOn
//{
//   wavFileName = "turretOn4.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundEnergyTurretOff
//{
//   wavFileName = "turretOff4.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundEnergyTurretTurn
//{
//   wavFileName = "turretTurn4.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundEnergyTurretFire
//{
//   wavFileName = "rifle1.wav";
//   profile = Profile3dMedium;
//};

//
//SoundData SoundRemoteTurretOn
//{
//   wavFileName = "turretOn2.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundRemoteTurretOff
//{
//   wavFileName = "turretOff2.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundRemoteTurretTurn
//{
//   wavFileName = "turretTurn2.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundRemoteTurretFire
//{
//   wavFileName = "rifle1.wav";
//   profile = Profile3dMedium;
//};


//----------------------------------------------------------------------------
// Item

SoundData SoundWeaponSelect
{
   wavFileName = "weapon5.wav";
   profile = Profile3dNear;
};

SoundData SoundFireBlaster
{
   wavFileName = "rifle1.wav";
   profile = Profile3dNear;
};

SoundData SoundFireChaingun
{
   wavFileName = "machinegun.wav";
   profile = Profile3dMediumLoop;
};

SoundData SoundSpinUp
{
   wavFileName = "Machgun3.wav";
   profile = Profile3dNear;
};

SoundData SoundSpinDown
{
   wavFileName = "Machgun2.wav";
   profile = Profile3dNear;
};

SoundData SoundDryFire
{
   wavFileName = "Dryfire1.wav";
   profile = Profile3dNear;
};

SoundData SoundFireGrenade
{
   wavFileName = "Grenade.wav";
   profile = Profile3dNear;
};

SoundData SoundFirePlasma
{
   wavFileName = "plasma2.wav";
   profile = Profile3dNear;
};

SoundData SoundSpinUpDisc
{
   wavFileName = "discspin.wav";
   profile = Profile3dNear;
};

SoundData SoundFireDisc
{
   wavFileName = "rocket2.wav";
   profile = Profile3dNear;
};

SoundData SoundDiscReload
{
   wavFileName = "discreload.wav";
   profile = Profile3dNear;
};

SoundData SoundDiscSpin
{
   wavFileName = "discloop.wav";
   profile = Profile3dNearLoop;
};

SoundData SoundFireLaser
{
   wavFileName = "sniper.wav";
   profile = Profile3dNear;
};

SoundData SoundLaserHit
{
   wavFileName = "laserhit.wav";
   profile = Profile3dMedium;
};

SoundData SoundFireTargetingLaser
{
   wavFileName = "tgt_laser.wav";
   profile = Profile3dNearLoop;
};

SoundData SoundLaserIdle
{
   wavFileName = "sniper2.wav";
   profile = Profile3dNearLoop;
};

SoundData SoundTargetLaser
{
   wavFileName = "tgt_laser.wav";
   profile = Profile3dNear;
};

SoundData SoundFireMortar
{
   wavFileName = "mortar_fire.wav";
   profile = Profile3dNear;
};

SoundData SoundMortarIdle
{
   wavFileName = "mortar_idle.wav";
   profile = Profile3dNearLoop;
};

SoundData SoundMortarReload
{
   wavFileName = "mortar_reload.wav";
   profile = Profile3dNearLoop;
};

SoundData SoundFireSeeking
{
   wavFileName = "seek_fire.wav";
   profile = Profile3dNear;
};

SoundData SoundMineActivate
{
   wavFileName = "mine_act.wav";
   profile = Profile3dNear;
};

SoundData SoundFloatMineTarget
{
   wavFileName = "float_target.wav";
   profile = Profile3dNear;
};

SoundData SoundFireFlierRocket
{
	wavFileName = "flierrocket.wav";
	profile = Profile3dMedium;
};

SoundData SoundELFFire
{
	wavFileName = "elf_fire.wav";
	profile = Profile3dMediumLoop;
};

SoundData SoundELFIdle
{
	wavFileName = "lightning_idle.wav";
	profile = Profile3dNearLoop;
};


//----------------------------------------------------------------------------
// Inventory sounds

SoundData SoundPickupItem
{
   wavFileName = "Pku_weap.wav";
   profile = Profile3dNear;
};

SoundData SoundPickupHealth
{
   wavFileName = "Pku_hlth.wav";
   profile = Profile3dNear;
};

SoundData SoundPickupBackpack
{
   wavFileName = "Dryfire1.wav";
   profile = Profile3dNear;
};

SoundData SoundPickupWeapon
{
   wavFileName = "Pku_weap.wav";
   profile = Profile3dNear;
};

SoundData SoundPickupAmmo
{
   wavFileName = "Pku_ammo.wav";
   profile = Profile3dNear;
};

SoundData SoundActivatePDA
{
   wavFileName = "pda_on.wav";
   profile = Profile3dNear;
};

SoundData SoundPDAButtonHard
{
   wavFileName = "button_hard.wav";
   profile = Profile3dNear;
};

SoundData SoundPDAButtonSoft
{
   wavFileName = "button_soft.wav";
   profile = Profile3dNear;
};


//----------------------------------------------------------------------------
// Inventory equipment

//SoundData SoundActivateAmmoStation
//{
//   wavFileName = "ammo_activate.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundUseAmmoStation
//{
//   wavFileName = "ammo_use.wav";
//   profile = Profile3dNearLoop;
//};

//SoundData SoundAmmoStationPower
//{
//   wavFileName = "ammo_power.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundActivateInventoryStation
//{
//   wavFileName = "inv_activate.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundUseInventoryStation
//{
//   wavFileName = "inv_use.wav";
//   profile = Profile3dNearLoop;
//};

//SoundData SoundInventoryStationPower
//{
//   wavFileName = "inv_power.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundActivateCommandStation
//{
//   wavFileName = "command_activate.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundUseCommandStation
//{
//   wavFileName = "command_use.wav";
//   profile = Profile3dNearLoop;
//};

//SoundData SoundCommandStationPower
//{
//   wavFileName = "command_power.wav";
//   profile = Profile3dNear;
//};

//----------------------------------------------------------------------------
// Item sounds

//SoundData SoundGeneratorPower
//{
//   wavFileName = "generator.wav";
//   profile = Profile3dNearLoop;
//};

//SoundData SoundActivateMotionSensor
//{
//   wavFileName = "motion_activate.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundSensorPower
//{
//   wavFileName = "pulse_power.wav";
//   profile = Profile3dNearLoop;
//};

//SoundData SoundTeleportPower
//{
//   wavFileName = "activateTele.wav";
//   profile = Profile3dNearLoop;
//};

//SoundData SoundBeaconActive
//{
//   wavFileName = "activateBeacon.wav";
//   profile = Profile3dNearLoop;
//};

//SoundData SoundBeaconUse
//{
//   wavFileName = "teleport2.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundPackUse
//{
//   wavFileName = "usepack.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundPackFail
//{
//   wavFileName = "failpack.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundThrowItem
//{
//   wavFileName = "throwitem.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundShieldOn
//{
//   wavFileName = "shield_on.wav";
//   profile = Profile3dNearLoop;
//};

//SoundData SoundEnergyPackOn
//{
//   wavFileName = "energypackon.wav";
//   profile = Profile3dNearLoop;
//};

//SoundData SoundJammerOn
//{
//   wavFileName = "jammer_on.wav";
//   profile = Profile3dNearLoop;
//};

//SoundData SoundRepairItem
//{
//   wavFileName = "repair.wav";
//   profile = Profile3dNearLoop;
//};

//SoundData SoundFlagCaptured
//{
//   wavFileName = "flagcapture.wav";
//   profile = Profile3dMedium;
//};

//SoundData SoundFlagReturned
//{
//   wavFileName = "flagreturn.wav";
//   profile = Profile3dMedium;
//};

//SoundData SoundFlagPickup
//{
//   wavFileName = "flag1.wav";
//   profile = Profile3dMedium;
//};

//SoundData SoundFlagFlap
//{
//   wavFileName = "flagflap.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundDeploySensor
//{
//   wavFileName = "sensor_deploy.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundActiveSensor
//{
//   wavFileName = "sensor_active.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundTurretDeploy
//{
//   wavFileName = "rmt_turret.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundRadarDeploy
//{
//   wavFileName = "rmt_radar.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundCameraDeploy
//{
//   wavFileName = "rmt_camera.wav";
//   profile = Profile3dNear;
//};

//----------------------------------------------------------------------------
// Explosion Sounds

SoundData bigExplosion1
{
   wavFileName = "bxplo1.wav";
   profile     = Profile3dFar;
};

SoundData bigExplosion2
{
   wavFileName = "bxplo2.wav";
   profile     = Profile3dFar;
};

SoundData bigExplosion3
{
   wavFileName = "bxplo3.wav";
   profile     = Profile3dFar;
};

SoundData bigExplosion4
{
   wavFileName = "bxplo4.wav";
   profile     = Profile3dFar;
};

SoundData explosion3
{
   wavFileName = "explo3.wav";
   profile     = Profile3dFar;
};

SoundData explosion4
{
   wavFileName = "explo4.wav";
   profile     = Profile3dFar;
};

SoundData ricochet1
{
   wavFileName = "ricoche1.wav";
   profile     = Profile3dNear;
};

SoundData ricochet2
{
   wavFileName = "ricoche2.wav";
   profile     = Profile3dNear;
};

SoundData ricochet3
{
   wavFileName = "ricoche3.wav";
   profile     = Profile3dNear;
};

SoundData energyExplosion
{
   wavFileName = "energyexp.wav";
   profile     = Profile3dMedium;
};

SoundData rocketExplosion
{
   wavFileName = "rockexp.wav";
   profile     = Profile3dLudicrouslyFar;
};

SoundData shockExplosion
{
   wavFileName = "shockexp.wav";
   profile     = Profile3dLudicrouslyFar;
};


SoundData turretExplosion
{
   wavFileName = "turretexp.wav";
   profile     = Profile3dMedium;
};

SoundData mineExplosion
{
   wavFileName = "mine_exp.wav";
   profile     = Profile3dFar;
};

SoundData floatMineExplosion
{
   wavFileName = "float_explode.wav";
   profile     = Profile3dFar;
};

SoundData debrisSmallExplosion
{
   wavFileName = "debris_small.wav";
   profile     = Profile3dNear;
};

SoundData debrisMediumExplosion
{
   wavFileName = "debris_medium.wav";
   profile     = Profile3dMedium;
};

SoundData debrisLargeExplosion
{
   wavFileName = "debris_large.wav";
   profile     = Profile3dFar;
};

//----------------------------------------------------------------------------
// Vehicle Sounds

//SoundData SoundFlyerMount
//{
//   wavFileName = "flyer_mount.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundFlyerDismount
//{
//   wavFileName = "flyer_dismount.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundFlyerActive
//{
//   wavFileName = "flyer_fly.wav";
//   profile = Profile3dMediumLoop;
//};

//SoundData SoundFlyerIdle
//{
//   wavFileName = "flyer_idle.wav";
//   profile = Profile3dMediumLoop;
//};

//SoundData SoundFlierCrash
//{
//   wavFileName = "crash.wav";
//   profile = Profile3dMedium;
//};

//SoundData SoundTankMount
//{
//   wavFileName = "flyer_mount.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundTankDismount
//{
//   wavFileName = "flyer_dismount.wav";
//   profile = Profile3dNear;
//};

//SoundData SoundTankActive
//{
//   wavFileName = "flyer_fly.wav";
//   profile = Profile3dMediumLoop;
//};

//SoundData SoundTankIdle
//{
//   wavFileName = "flyer_idle.wav";
//   profile = Profile3dMediumLoop;
//};

//SoundData SoundTankCrash
//{
//   wavFileName = "crash.wav";
//   profile = Profile3dMedium;
//};


//----------------------------------------------------------------------------
// RPG sounds

SoundData NoSound
{
   wavFileName = "";
   profile     = Profile3dNear;
};

SoundData SoundSpawn2
{
   wavFileName = "spawn2.wav";
   profile = Profile3dMedium;
};

SoundData SoundGrunt1
{
   wavFileName = "grunt1a.wav";
   profile = Profile3dNear;
};

SoundData SoundGrunt2
{
   wavFileName = "grunt2a.wav";
   profile = Profile3dNear;
};

SoundData SoundGrunt3
{
   wavFileName = "grunt3a.wav";
   profile = Profile3dNear;
};

SoundData SoundHarvest1
{
   wavFileName = "harvest1.wav";
   profile = Profile3dNearLoop;
};

SoundData SoundSplash1
{
   wavFileName = "water3.wav";
   profile = Profile3dMedium;
};

SoundData SoundSwing1
{
   wavFileName = "swish.wav";
   profile = Profile3dNear;
};
SoundData SoundSwing2
{
   wavFileName = "swish2.wav";
   profile = Profile3dNear;
};
SoundData SoundSwing3
{
   wavFileName = "swish3.wav";
   profile = Profile3dNear;
};
SoundData SoundSwing4
{
   wavFileName = "swish4.wav";
   profile = Profile3dNear;
};
SoundData SoundSwing5
{
   wavFileName = "swish5.wav";
   profile = Profile3dNear;
};
SoundData SoundSwing6
{
   wavFileName = "swish6.wav";
   profile = Profile3dNear;
};
SoundData SoundSwing7
{
   wavFileName = "swish7.wav";
   profile = Profile3dNear;
};

SoundData SoundSwordHit1
{
   wavFileName = "hit1.wav";
   profile = Profile3dNear;
};
SoundData SoundArrowHit1
{
   wavFileName = "arrowhit.wav";
   profile = Profile3dNear;
};
SoundData SoundHitFlesh
{
   wavFileName = "Hit_Flesh.wav";
   profile = Profile3dNear;
};
SoundData SoundHitLeather
{
   wavFileName = "Hit_Leather.wav";
   profile = Profile3dNear;
};
SoundData SoundHitChain
{
   wavFileName = "Hit_Chain.wav";
   profile = Profile3dNear;
};
SoundData SoundHitPlate
{
   wavFileName = "Hit_Plate.wav";
   profile = Profile3dNear;
};
SoundData SoundHitShield
{
   wavFileName = "Hit_Shield.wav";
   profile = Profile3dNear;
};

SoundData SoundMoney1
{
   wavFileName = "money.wav";
   profile = Profile3dNear;
};

SoundData SoundSmith
{
   wavFileName = "smith.wav";
   profile = Profile3dMedium;
};

SoundData SoundDrown1
{
   wavFileName = "drown1.wav";
   profile = Profile3dNear;
};

SoundData SoundDrown2
{
   wavFileName = "drown2.wav";
   profile = Profile3dNear;
};

SoundData SoundDrown3
{
   wavFileName = "h2odeath.wav";
   profile = Profile3dNear;
};

SoundData SoundHitore
{
   wavFileName = "hitore.wav";
   profile = Profile3dNear;
};
SoundData SoundHitore2
{
   wavFileName = "hitore2.wav";
   profile = Profile3dNear;
};

//=== WOT SOUNDS ================================

//explosion with little rocks at the end
SoundData ExplodeLM
{
   wavFileName = "ExplodeLM.wav";
   profile     = Profile3dFar;
};

//Power-up like sound
SoundData ActivateBF
{
   wavFileName = "ActivateBF.wav";
   profile     = Profile3dNear;
};

//6.7 second loop
SoundData LoopWA
{
   wavFileName = "LoopWA.wav";
   profile     = Profile3dNear;
};

SoundData Portal11
{
   wavFileName = "Portal11.wav";
   profile     = Profile3dNear;
};

SoundData ActivateCH
{
   wavFileName = "ActivateCH.wav";
   profile     = Profile3dNear;
};

SoundData ActivateAR
{
   wavFileName = "ActivateAR.wav";
   profile     = Profile3dNear;
};

SoundData DeActivateWA
{
   wavFileName = "DeActivateWA.wav";
   profile     = Profile3dNear;
};

SoundData HitBF
{
   wavFileName = "HitBF.wav";
   profile     = Profile3dNear;
};

SoundData HitLevelDT
{
   wavFileName = "HitLevelDT.wav";
   profile     = Profile3dNear;
};

SoundData ActivateFK
{
   wavFileName = "ActivateFK.wav";
   profile     = Profile3dNear;
};

SoundData DeflectAS
{
   wavFileName = "DeflectAS.wav";
   profile     = Profile3dNear;
};

SoundData ActivateAB
{
   wavFileName = "ActivateAB.wav";
   profile     = Profile3dNear;
};

SoundData LaunchFB
{
   wavFileName = "LaunchFB.wav";
   profile     = Profile3dFar;
};

SoundData HitPawnDT
{
   wavFileName = "HitPawnDT.wav";
   profile     = Profile3dNear;
};

SoundData ImpactTR
{
   wavFileName = "ImpactTR.wav";
   profile     = Profile3dNear;
};

SoundData Reflected
{
   wavFileName = "Reflected.wav";
   profile     = Profile3dNear;
};

SoundData UnravelAM
{
   wavFileName = "UnravelAM.wav";
   profile     = Profile3dNear;
};

SoundData LaunchLS
{
   wavFileName = "LaunchLS.wav";
   profile     = Profile3dNear;
};

SoundData LaunchET
{
   wavFileName = "LaunchET.wav";
   profile     = Profile3dMedium;
};

SoundData LoopLS
{
   wavFileName = "LoopLS.wav";
   profile     = Profile3dNear;
};

SoundData Explode3FW
{
   wavFileName = "Explode3FW.wav";
   profile     = Profile3dMedium;
};

SoundData LoopLG
{
   wavFileName = "LoopLG.wav";
   profile = Profile3dNearLoop;
};

SoundData LoopLT
{
   wavFileName = "LoopLT.wav";
   profile = Profile3dNearLoop;
};

SoundData PortalLoop1
{
   wavFileName = "PortalLoop1.wav";
   profile = Profile3dFarLoop;
};

SoundData PortalLoop3
{
   wavFileName = "PortalLoop3.wav";
   profile = Profile3dFarLoop;
};

SoundData River
{
   wavFileName = "River.wav";
   profile = Profile3dVeryFarLoop;
};

SoundData Windy
{
   wavFileName = "Windy.wav";
   profile = Profile3dVeryFarLoop;
};

SoundData Windy2
{
   wavFileName = "Windy2.wav";
   profile = Profile3dVeryFarLoop;
};

SoundData Moutain
{
   wavFileName = "Mountain.wav";
   profile = Profile3dVeryVeryFarLoop;
};

SoundData LightWind
{
   wavFileName = "LightWind.wav";
   profile = Profile3dVeryVeryFarLoop;
};

SoundData RespawnA
{
   wavFileName = "RespawnA.wav";
   profile     = Profile3dNear;
};

SoundData RespawnB
{
   wavFileName = "RespawnB.wav";
   profile     = Profile3dNear;
};

SoundData RespawnC
{
   wavFileName = "RespawnC.wav";
   profile     = Profile3dNear;
};

SoundData DeActivateDG
{
   wavFileName = "DeActivateDG.wav";
   profile     = Profile3dNear;
};

SoundData Portal6
{
   wavFileName = "Portal6.wav";
   profile     = Profile3dNear;
};

SoundData LoopDT
{
   wavFileName = "LoopDT.wav";
   profile = Profile3dMediumLoop;
};

SoundData PlaceSeal
{
   wavFileName = "PlaceSeal.wav";
   profile     = Profile3dNear;
};

SoundData ActivateTR
{
   wavFileName = "ActivateTR.wav";
   profile     = Profile3dNear;
};

SoundData ActivateTD
{
   wavFileName = "ActivateTD.wav";
   profile     = Profile3dNear;
};

SoundData BonusStateExpire
{
   wavFileName = "DeActivateIC.wav";
   profile     = Profile3dNear;
};

SoundData AbsorbABS
{
   wavFileName = "AbsorbABS.wav";
   profile     = Profile3dNear;
};

SoundData LoopSP
{
   wavFileName = "LoopSP.wav";
   profile     = Profile3dNear;
};

SoundData ActivateAS
{
   wavFileName = "ActivateAS.wav";
   profile     = Profile3dNear;
};

//crossbow firing sound
SoundData CrossbowShoot1
{
   wavFileName = "Crossbow_Shoot1.wav";
   profile     = Profile3dNear;
};

//crossbow switching sound
SoundData CrossbowSwitch1
{
   wavFileName = "Crossbow_Switch1.wav";
   profile     = Profile3dNear;
};

//crossbow hitting sound
SoundData SoundArrowHit2
{
   wavFileName = "Crossbow_HitWall1.wav";
   profile = Profile3dNear;
};

//axe slashing
SoundData AxeSlash2
{
   wavFileName = "Axe_Slash2.wav";
   profile = Profile3dNear;
};

//high pitch ooooo loop
SoundData SoundGliders
{
   wavFileName = "Gliders.wav";
   profile = Profile3dMediumLoop;
};

//loud wind-like loop
SoundData SoundWindWalkers
{
   wavFileName = "WindWalkers.wav";
   profile = Profile3dMediumLoop;
};

//boat sound
SoundData SoundBoat
{
   wavFileName = "AmbBoat2m.wav";
   profile = Profile3dMediumLoop;
};

SoundData SoundFountain
{
   wavFileName = "fountain.wav";
   profile = Profile3dMediumLoop;
};
SoundData SoundFeast
{
   wavFileName = "feast.wav";
   profile = Profile3dFar;
};

SoundData SoundLevelUp
{
   wavFileName = "LevelUp.wav";
   profile = Profile3dNear;
};

//OGRE SOUNDS
SoundData SoundOgreDeath1
{
	wavFileName = "OgreDeath1.wav";
	profile = Profile3dNear;
};
SoundData SoundOgreAcquired1
{
	wavFileName = "OgreAcquired1.wav";
	profile = Profile3dNear;
};
SoundData SoundOgreAcquired2
{
	wavFileName = "OgreAcquired2.wav";
	profile = Profile3dNear;
};
SoundData SoundOgreHit1
{
	wavFileName = "OgreHit1.wav";
	profile = Profile3dNear;
};
SoundData SoundOgreHit2
{
	wavFileName = "OgreHit2.wav";
	profile = Profile3dNear;
};
SoundData SoundOgreTaunt1
{
	wavFileName = "OgreTaunt1.wav";
	profile = Profile3dNear;
};
SoundData SoundOgreTaunt2
{
	wavFileName = "OgreTaunt2.wav";
	profile = Profile3dNear;
};
SoundData SoundOgreRandom1
{
	wavFileName = "OgreRandom1.wav";
	profile = Profile3dNear;
};
SoundData SoundOgreRandom2
{
	wavFileName = "OgreRandom2.wav";
	profile = Profile3dNear;
};

//UNDEAD SOUNDS
SoundData SoundUndeadDeath1
{
	wavFileName = "UndeadDeath1.wav";
	profile = Profile3dNear;
};
SoundData SoundUndeadAcquired1
{
	wavFileName = "UndeadAcquired1.wav";
	profile = Profile3dNear;
};
SoundData SoundUndeadRandom1
{
	wavFileName = "UndeadRandom1.wav";
	profile = Profile3dNear;
};
SoundData SoundUndeadTaunt1
{
	wavFileName = "UndeadTaunt1.wav";
	profile = Profile3dNear;
};
SoundData SoundUndeadHit1
{
	wavFileName = "UndeadHit1.wav";
	profile = Profile3dNear;
};
SoundData SoundUndeadHit2
{
	wavFileName = "UndeadHit2.wav";
	profile = Profile3dNear;
};

//TRAVELLER SOUNDS
SoundData SoundTravellerDeath1
{
	wavFileName = "TravellerDeath1.wav";
	profile = Profile3dNear;
};
SoundData SoundTravellerAcquired1
{
	wavFileName = "TravellerAcquired1.wav";
	profile = Profile3dNear;
};
SoundData SoundTravellerAcquired2
{
	wavFileName = "TravellerAcquired2.wav";
	profile = Profile3dNear;
};
SoundData SoundTravellerAcquired3
{
	wavFileName = "TravellerAcquired3.wav";
	profile = Profile3dNear;
};
SoundData SoundTravellerHit1
{
	wavFileName = "TravellerHit1.wav";
	profile = Profile3dNear;
};
SoundData SoundTravellerHit2
{
	wavFileName = "TravellerHit2.wav";
	profile = Profile3dNear;
};
SoundData SoundTravellerHit3
{
	wavFileName = "TravellerHit3.wav";
	profile = Profile3dNear;
};

//UBER SOUNDS
SoundData SoundUberDeath1
{
	wavFileName = "UberDeath1.wav";
	profile = Profile3dNear;
};
SoundData SoundUberAcquired1
{
	wavFileName = "UberAcquired1.wav";
	profile = Profile3dNear;
};
SoundData SoundUberAcquired2
{
	wavFileName = "UberAcquired2.wav";
	profile = Profile3dNear;
};
SoundData SoundUberHit1
{
	wavFileName = "UberHit1.wav";
	profile = Profile3dNear;
};
SoundData SoundUberRandom1
{
	wavFileName = "UberRandom1.wav";
	profile = Profile3dNear;
};
//MINOTAUR SOUNDS
SoundData SoundMinotaurDeath1
{
	wavFileName = "MinotaurDeath1.wav";
	profile = Profile3dNear;
};
SoundData SoundMinotaurAcquired1
{
	wavFileName = "MinotaurAcquired1.wav";
	profile = Profile3dNear;
};
SoundData SoundMinotaurAcquired2
{
	wavFileName = "MinotaurAcquired2.wav";
	profile = Profile3dNear;
};
SoundData SoundMinotaurHit1
{
	wavFileName = "MinotaurHit1.wav";
	profile = Profile3dNear;
};
//GOBLIN SOUNDS
SoundData SoundGoblinDeath1
{
	wavFileName = "GoblinDeath1.wav";
	profile = Profile3dNear;
};
SoundData SoundGoblinDeath2
{
	wavFileName = "GoblinDeath2.wav";
	profile = Profile3dNear;
};
SoundData SoundGoblinAcquired1
{
	wavFileName = "GoblinAcquired1.wav";
	profile = Profile3dNear;
};
SoundData SoundGoblinAcquired2
{
	wavFileName = "GoblinAcquired2.wav";
	profile = Profile3dNear;
};
SoundData SoundGoblinAcquired3
{
	wavFileName = "GoblinAcquired3.wav";
	profile = Profile3dNear;
};
SoundData SoundGoblinTaunt1
{
	wavFileName = "GoblinTaunt1.wav";
	profile = Profile3dNear;
};
SoundData SoundGoblinRandom1
{
	wavFileName = "GoblinRandom1.wav";
	profile = Profile3dNear;
};
SoundData SoundGoblinHit1
{
	wavFileName = "GoblinHit1.wav";
	profile = Profile3dNear;
};
SoundData SoundGoblinHit2
{
	wavFileName = "GoblinHit2.wav";
	profile = Profile3dNear;
};
SoundData SoundGoblinHit3
{
	wavFileName = "GoblinHit3.wav";
	profile = Profile3dNear;
};
SoundData SoundGoblinHit4
{
	wavFileName = "GoblinHit4.wav";
	profile = Profile3dNear;
};
SoundData SoundGoblinHit5
{
	wavFileName = "GoblinHit5.wav";
	profile = Profile3dNear;
};
SoundData SoundGoblinHit6
{
	wavFileName = "GoblinHit6.wav";
	profile = Profile3dNear;
};
SoundData SoundGoblinHit7
{
	wavFileName = "GoblinHit7.wav";
	profile = Profile3dNear;
};
SoundData SoundGoblinHit8
{
	wavFileName = "GoblinHit8.wav";
	profile = Profile3dNear;
};
//GNOLL SOUNDS
SoundData SoundGnollDeath1
{
	wavFileName = "GnollDeath1.wav";
	profile = Profile3dNear;
};
SoundData SoundGnollDeath2
{
	wavFileName = "GnollDeath2.wav";
	profile = Profile3dNear;
};
SoundData SoundGnollAcquired1
{
	wavFileName = "GnollAcquired1.wav";
	profile = Profile3dNear;
};
SoundData SoundGnollTaunt1
{
	wavFileName = "GnollTaunt1.wav";
	profile = Profile3dNear;
};
SoundData SoundGnollRandom1
{
	wavFileName = "GnollRandom1.wav";
	profile = Profile3dNear;
};
SoundData SoundGnollRandom2
{
	wavFileName = "GnollRandom2.wav";
	profile = Profile3dNear;
};
SoundData SoundGnollHit1
{
	wavFileName = "GnollHit1.wav";
	profile = Profile3dNear;
};
SoundData SoundGnollHit2
{
	wavFileName = "GnollHit2.wav";
	profile = Profile3dNear;
};


SoundData SoundCanSmith
{
	wavFileName = "canSmith.wav";
	profile = Profile3dNear;
};


SoundData MClip5
{
	wavFileName = "mclip5.wav";
	profile = Profile3dNear;
};
SoundData MClip6
{
	wavFileName = "mclip6.wav";
	profile = Profile3dNear;
};
//================================================

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