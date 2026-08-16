//----------------------------------------------------------------------------
// Mech Mayhem -- boot script.
// Exec'd by ExecModScripts (console.cs) when "-mod mechmayhem" is on the
// command line / modlist.txt, which runs before any createServer, so the
// flags set here gate datablock registration at the correct time.
//----------------------------------------------------------------------------

// Register the 31 Starsiege Herc armors: ArmorData.cs execs MechArmorData.cs
// behind this flag, inside createServer, BEFORE preloadServerDataBlocks().
$MechPack::Enable = 1;

// Generic pre-preload datablock hook in base\scripts\server.cs: the named
// script is exec'd after the stock datablock scripts and before
// preloadServerDataBlocks(). exec() resolves through the mod search path
// (isFile() would not -- it is a raw file stream, simGame.cpp IsFile).
$Mod::ServerDataBlocks = "modDataBlocks.cs";

// Prefer .glb mech shapes over .dts (shape resolver in resManager.cpp:
// .glb > .gltf > .dts when this pref is set). The GLB herc set has correct
// per-mesh winding; the .dts hercs show inverted invisible walls.
// cy_seek / mg_seek / pl_judg have no .glb yet (duplicate object names in
// the source) and fall through to their .dts automatically.
$pref::gltfShapes = 1;

$MechMayhem::Loaded = 1;
echo("[MECH] Mech Mayhem boot: MechPack enabled, datablock hook armed, glTF shapes on.");
