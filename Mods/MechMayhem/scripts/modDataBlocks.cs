//----------------------------------------------------------------------------
// Mech Mayhem -- server-side datablocks.
// Exec'd from base\scripts\server.cs via $Mod::ServerDataBlocks, after the
// stock datablock scripts (so overrides win the last-wins race) and before
// preloadServerDataBlocks() (so anything declared here actually registers).
//
// Order is load-bearing:
//   MechWeapons  new weapon/projectile datablocks
//   MechTwins    Shutdown/Crippled twin PlayerData (full generated copies)
//   MechChassis  dot-assignment stat tuning on the ALREADY-DECLARED herc
//                armors + twins (ArmorData.cs exec'd them earlier in
//                createServer), plus the $DamageScale table and $MM::* registry
//----------------------------------------------------------------------------

exec(MechWeapons);
exec(MechTwins);
exec(MechChassis);

$MechMayhem::DataBlocksLoaded = 1;
echo("[MECH] modDataBlocks.cs exec'd (pre-preload hook OK).");
