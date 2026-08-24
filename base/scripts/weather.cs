// NATIVE-EDITOR (2026-08-22): editor-placeable thunderstorms -- v0, pure script.
//
// Pattern proven by DeltaAirForce's LightningStrike (Mods\DeltaAirForce\Scripts\
// baseProjData.cs:3286-3305): an instantly-detonating grenade whose explosion
// carries a big dynamic light (terrain-wide flash on the GPU render path) and a
// far-carrying thunder sound. All assets are stock (shockexp.wav, mortarex.dts),
// so vanilla remote clients need nothing -- datablocks stream at join.
//
// A storm is a placed Marker named Storm* with dynamic fields (persisted into
// the .mis by ME::Save). Storm::MissionStart (scheduled from server.cs at
// mission start) finds the markers and runs one strike loop each.
//
// Dialect care: every schedule() lives inside a function (top-level schedules
// are silently dropped), and each function is small so one syntax error cannot
// take the whole system down.

//----------------------------------------------------------------------------
// datablocks (exec'd from server.cs BEFORE preloadServerDataBlocks)

SoundProfileData StormThunderProfile
{
   baseVolume  = 0;
   minDistance = 30.0;
   maxDistance = 2000.0;
   flags       = SFX_IS_HARDWARE_3D;
};

SoundData StormThunder
{
   wavFileName = "shockexp.wav";
   profile     = StormThunderProfile;
};

ExplosionData StormFlash
{
   // NATIVE-EDITOR (2026-08-23, Joe feedback): mortarex read as a BOMB, and
   // lightRange 1200 lit the whole map. enex is the stock energy-weapon burst
   // (blue-white sparks -- the most lightning-like 1998 asset), range 350 lights
   // the strike area only.
   shapeName  = "stormbolt.glb";   // A/B: audible explosion + bolt mesh in ONE object
   soundId    = StormThunder;
   faceCamera = true;
   randomSpin = false;
   hasLight   = true;
   lightRange = 200.0;
   timeScale  = 1.0;
   timeZero   = 0.0;
   timeOne    = 0.500;
   colors[0]  = { 1.0, 1.0, 1.0 };
   colors[1]  = { 1.0, 1.0, 1.0 };
   colors[2]  = { 1.0, 1.0, 1.0 };
   radFactors = { 1.0, 1.0, 1.0 };
};

// The bolt COLUMN: silent copies of the flash, stacked above the impact by
// Storm::strike, so a luminous line connects sky to ground for the strike
// instant. No soundId -- one thunder per strike, not four.
ExplosionData StormFlashAir
{
   // stormbolt.glb: generated 150-unit crossed-ribbon bolt (tools; the stock
   // zap.dts bolt is Starsiege prop scale -- a couple of units -- Joe: "tiny").
   shapeName  = "stormbolt.glb";
   faceCamera = false;
   randomSpin = false;
   hasLight   = false;
   lightRange = 0.0;
   timeScale  = 4.0;    // flash sequence is 0.1 s -> bolt on screen ~0.4 s
   timeZero   = 0.0;
   timeOne    = 0.500;
   colors[0]  = { 1.0, 1.0, 1.0 };
   colors[1]  = { 1.0, 1.0, 1.0 };
   colors[2]  = { 1.0, 1.0, 1.0 };
   radFactors = { 1.0, 1.0, 1.0 };
};

GrenadeData StormBoltAir
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = StormFlashAir;
   collideWithOwner   = True;
   ownerGraceMS       = 400;
   collisionRadius    = 0.0;
   mass               = -2.0;
   elasticity         = 0.0;
   damageClass        = 1;
   damageValue        = 0.0;
   damageType         = $ElectricityDamageType;
   explosionRadius    = 8.0;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 0.0;
   totalTime          = 0.005;
   liveTime           = 0.005;
   projSpecialTime    = 0.005;
   inheritedVelocityScale = 1.0;
};

GrenadeData StormBolt
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = StormFlash;
   collideWithOwner   = True;
   ownerGraceMS       = 400;
   collisionRadius    = 0.0;
   mass               = -2.0;
   elasticity         = 0.0;
   damageClass        = 1;                  // radius
   damageValue        = 0.0;                // cosmetic by default; stormDamage field can raise it
   damageType         = $ElectricityDamageType;
   explosionRadius    = 8.0;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 0.0;
   totalTime          = 0.005;              // detonate where spawned
   liveTime           = 0.005;
   projSpecialTime    = 0.005;
   inheritedVelocityScale = 1.0;
};

//----------------------------------------------------------------------------
// editor palette hook (called from registerUserObjects.cs when editing)

function ME::AddStorm()
{
   %obj = MissionCreateObject(Storm, Marker, MapMarker);
   focusServer();
   %obj.stormMinDelay = 4;      // seconds between strikes (min)
   %obj.stormMaxDelay = 18;     // seconds between strikes (max)
   %obj.stormRadius   = 250;    // strike scatter radius around the marker
   %obj.stormHeight   = 5;      // bolt base near the ground (mesh rises 150 units)
   %obj.stormDamage   = 0;      // damage at the strike point (0 = cosmetic); radius 10
   // NATIVE-EDITOR (2026-08-23): start the strike loop NOW -- Storm::MissionStart
   // runs once, 6 s after mission start, so a storm placed mid-edit never fired
   // until the mission was reloaded (Joe: "I cant hear ... thunderstorms").
   schedule("Storm::run(" @ %obj @ ");", 2);
   focusClient();
}

//----------------------------------------------------------------------------
// server runtime

function Storm::MissionStart()
{
   // MissionCreateObject names carriers Storm1..StormN; manager names resolve
   // globally. Probe a bounded range so a deleted Storm2 doesn't hide Storm3.
   for(%i = 1; %i <= 8; %i++)
   {
      // NATIVE-EDITOR (2026-08-23): editor-placed objects live INSIDE MissionGroup
      // and bare-name lookup cannot see them (5c31018: nameToID("Storm1") = -1 but
      // "MissionGroup/Storm1" resolves) -- so the loop never found a single storm.
      %obj = nameToId("MissionGroup/Storm" @ %i);
      if(%obj == -1 || %obj == "" || %obj == 0)
         %obj = nameToId("Storm" @ %i);
      if(%obj != -1 && %obj != "" && %obj != 0)
         Storm::run(%obj);
   }
}

function Storm::strike(%obj)
{
   %pos = GameBase::getPosition(%obj);
   %x = getWord(%pos, 0) + (getRandom() * 2 - 1) * %obj.stormRadius;
   %y = getWord(%pos, 1) + (getRandom() * 2 - 1) * %obj.stormRadius;
   %z = getWord(%pos, 2) + %obj.stormHeight;
   %transform = "0 0 0 0 0 0 90 0 0 " @ %x @ " " @ %y @ " " @ %z @ "";
   // explicit shooter 0: an empty arg leaves spawnProjectile's sscanf'd
   // shooterId UNINITIALIZED (FearPlugin.cpp:4354)
   Projectile::spawnProjectile("StormBolt", %transform, 0, "0 0 0");
   // NATIVE-EDITOR: strike damage -- same pattern as the lava bridge, but the
   // radius-damage console command already existed (FearPlugin.cpp
   // GameBase::applyRadiusDamage(type,pos,radius,value,force,srcId)).
   %dmg = %obj.stormDamage;
   if(%dmg != "" && %dmg > 0)
      GameBase::applyRadiusDamage(0, %x @ " " @ %y @ " " @ %z, 10, %dmg, 200, 0);
   // The strike object IS the bolt: stormbolt.glb (150-unit generated mesh) +
   // thunder + light in one explosion. The separate silent "air column" spawns
   // never rendered and are gone -- the A/B that put the bolt mesh on the
   // AUDIBLE explosion is what finally showed lightning.
}

function Storm::run(%obj)
{
   if(!isObject(%obj))
      return;
   Storm::strike(%obj);
   %min = %obj.stormMinDelay;
   %max = %obj.stormMaxDelay;
   if(%min == "" || %min < 1) %min = 4;
   if(%max == "" || %max <= %min) %max = %min + 10;
   %delay = %min + getRandom() * (%max - %min);
   schedule("Storm::run(" @ %obj @ ");", %delay);
}
