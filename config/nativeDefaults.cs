//==============================================================================
// nativeDefaults.cs -- fixes that should apply to EVERY session, not just stress runs.
//
// These were all found by config\apocalypse.cs, but none of them are stress settings: they are
// corrections to defaults that are either 1998-era or outright broken for a modern server. The
// harness set them itself, which meant they only ever applied while a stress test was running --
// exactly backwards, since normal play is what benefits most.
//
// Exec'd from autoexec.cs, which console.cs:232 runs AFTER clientPrefs.cs and serverPrefs.cs.
// That ordering is the whole point: these values must win over the saved prefs, because the saved
// prefs are the problem.
//
// ★NOT included here: diagnostics.★ $pref::srvProfLog, ghostSkipDiag, frameProfile and hitchMs
// stay OFF by default -- they exist to be switched on for a measured run. Apocalypse still sets
// those itself and clears them afterward.
//
// Engine-side fixes from the same work need nothing here; they are compiled in and always
// active: PlayerManager::getFreeId's NULL guard (a full client-rep pool used to kill the server
// outright), Player/ShapeBase::applyDamage dispatching by argv instead of re-running the script
// parser on every bullet that connects, and the SimMovementCollisionEvent pool.
//==============================================================================

//------------------------------------------------------------------------------
// 1. NETWORK SEND BUDGET.
//
// The engine's own defaults are already right (Rate 60, Size 800 -- netPacketStream.cpp:371-372).
// The problem is the SAVED values: this install shipped 450/30 with PacketFrame 32, and has been
// observed rewriting itself to 96 -- all 28.8k-modem-era numbers. PacketFrame is the floor on how
// often a CLIENT sends its own moves and acks (gPacketSendTime, netPacketStream.cpp:361), so 96
// means input leaves the client about ten times a second, which is felt directly as lag.
//
// ★These are NEGOTIATED, and the receiver takes the conservative side★ -- max(updateDelay), i.e.
// the lower rate, and min(packetSize) (netPacketStream.cpp:365-368). So raising the server alone
// achieves nothing if the client is still on modem values, and vice versa. Both ends need it,
// which is why this lives in a file both ends exec.
//
// Applied as a FLOOR, not an assignment: a user who deliberately went higher keeps their value.
// Set $pref::netKeepSaved = 1 before this runs to opt out entirely.
//------------------------------------------------------------------------------
// ONE-SHOT (2026-07-31): this floor used to re-apply EVERY boot, so a player who
// deliberately set lower values in Options > Network was silently re-floored on the
// next start -- testers found $pref::netKeepSaved = 1 as the workaround, which proves
// the collision. Now it migrates legacy modem values ONCE (flagged by netFloorApplied)
// and never touches a saved value again. netKeepSaved = 1 still opts out entirely.
if($pref::netKeepSaved != 1 && $pref::netFloorApplied != 1)
{
   if($pref::PacketSize == "" || $pref::PacketSize < 800)   { $pref::PacketSize = 800; }
   if($pref::PacketRate == "" || $pref::PacketRate < 60)    { $pref::PacketRate = 60; }
   // Lower is faster here -- it is a minimum interval in ms, so this one is a ceiling.
   if($pref::PacketFrame == "" || $pref::PacketFrame > 16)  { $pref::PacketFrame = 16; }
   $pref::netFloorApplied = 1;
}

//------------------------------------------------------------------------------
// 2. AI OBSTACLE TRAVERSAL.
//
// jetTowardLoc/JetSkill were complete in aiObj.cpp but compiled out behind _JETNAVDEV_, which was
// defined nowhere. ★There is no navigation graph in ANY Tribes build★ -- aiGraph.cpp sits behind
// INCLUDE_AI_GRAPH_CODE (defined nowhere) and the shipped 1.40 binary has no Graph symbols
// either, so bots steer in straight lines and grind into walls. Jet nav is the only mitigation
// the engine has.
//
// Safe to default ON because the gate is DOUBLE: this pref plus a per-bot `jetNavigation` var
// that defaults to 0. Nothing changes for existing bots or mods unless they also opt in per bot.
//
// The tolerance matters as much as the switch: the recovered default of 0.2 units (3D) is
// unreachable for a bot arcing through the air, so it orbits its waypoint forever and one-way
// paths never complete. That is almost certainly why Dynamix left _JETNAVDEV_ off.
//------------------------------------------------------------------------------
if($pref::aiJetNav == "")    { $pref::aiJetNav = 1; }
if($pref::aiJetNavTol == "") { $pref::aiJetNavTol = 3.0; }

//------------------------------------------------------------------------------
// 3. AI TARGET ACQUISITION RATE.
//
// Stock re-scans exactly ONE bot per server tick (aiObj.cpp), which was fine for the handful of
// bots a 1998 match ran and scales badly: at 40 bots each one only re-acquires every 40 ticks,
// i.e. well over a second at 32Hz. Bots are not slow to shoot, they are slow to NOTICE, and that
// latency is most of why a large bot fight feels sluggish.
//
// 4 is deliberately modest -- enough to fix acquisition on a normal bot server without spending
// real CPU on scanning. Apocalypse raises it far higher for stress runs. 1 restores stock exactly.
//------------------------------------------------------------------------------
if($pref::aiScanPerTick == "") { $pref::aiScanPerTick = 4; }

//------------------------------------------------------------------------------
// 4. DEDICATED-SERVER CONSOLE LOG BUFFER.
//
// The in-RAM console scrollback is forced on every session and retains every line for the life of
// the process, drained only by conDump. On a client that is bounded by how long you play; on a
// long-running dedicated server it grows without limit. Off for dedicated only -- console.log on
// disk still gets everything, so nothing is lost.
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
// 5. BOT HARNESS, loaded and armed on dedicated servers.
//
// Loading apocalypse.cs only DEFINES its functions -- it spawns nothing and changes nothing on
// its own (that inertness is deliberate; see Apoc::lethalOverride for what happens when a harness
// file has side effects at load time).
//
// $Apoc::autoRun = 1 is what arms it. base\scripts\server.cs calls Apoc::autoStart at the end of
// Server::finishMissionLoad, so bots appear on the first mission and after every mission change
// with no manual step. Defaults are the PLAYABLE ones: 15 bots, stock movement and projectile
// speed, infinite ammo/energy, faster target acquisition.
//
// ★Comments live OUTSIDE the braced block, and the block ECHOES.★ The first version buried a
// 15-line comment inside `if($dedicated) { ... }` and the block silently did not run -- no error,
// no banner, and auto-start never fired while the file's closing echo still printed, so it looked
// like it had worked. Keep the body to statements, and keep the proof-of-execution echo.
//
// Set $Apoc::autoRun = 0 for a plain server with no bots.
//------------------------------------------------------------------------------
if($dedicated)
{
   $pref::consoleLogBuffer = 0;
   exec("apocalypse.cs");
   // ★Assigned, not defaulted.★ `if($Apoc::autoRun == "")` does not work here: exec'ing the
   // harness runs Apoc::defaults(), which has already set it to "0", so the empty-test never
   // matches and auto-run stayed off while the banner happily reported success. Edit this line
   // to 0 for a plain dedicated server with no bots.
   $Apoc::autoRun = 0;
   // ===================================================================================
   // RELEASE BLOCKER -- see re\RELEASE_BLOCKERS.md item 1.
   // The assignment above ARMS the stress harness on EVERY dedicated server. Deliberate
   // for testing; it must be 0 (or removed) before the client ships.
   // ===================================================================================
   if($Apoc::autoRun == 1)
      echo("[NATIVE] ***** APOCALYPSE HARNESS ARMED (autoRun=1 botTarget=" @ $Apoc::botTarget @ ") -- NOT FOR RELEASE *****");
   else
      echo("[NATIVE] dedicated: apocalypse harness AVAILABLE, not armed (autoRun=" @ $Apoc::autoRun @ ")");
}

echo("[NATIVE] defaults applied: net " @ $pref::PacketSize @ "/" @ $pref::PacketRate @ "/" @ $pref::PacketFrame @ "  aiJetNav=" @ $pref::aiJetNav @ "  aiScanPerTick=" @ $pref::aiScanPerTick);

//====================================================================================
// SHIPPED DEFAULTS -- append new prefs and keybinds HERE (smart-updater convention).
//
// This file SHIPS with every repack update (see tools\repack-whitelist.txt in the
// source repo); ClientPrefs.cs and config.cs are user state and NEVER ship. To give
// testers a new setting without resetting theirs:
//
//   PREFS   -- set only when unset. ALWAYS test == "" (an unset pref compares equal
//              to 0 numerically -- the classic console trap):
//                  if($pref::MyNewThing == "") { $pref::MyNewThing = 1; }
//              The value then persists into the USER'S own ClientPrefs.cs via the
//              normal autosave/exit export.
//
//   KEYBINDS -- bindDefault applies a default binding ONLY when the action is not
//              already bound for that make/break phase AND the key is not claimed:
//                  editActionMap("playMap.sae");
//                  bindDefault(keyboard0, make,  "g", TO, IDACTION_MYTHING, 1);
//                  bindDefault(keyboard0, break, "g", TO, IDACTION_MYTHING, 0);
//              A user's existing binding always wins; keys are never stolen.
//====================================================================================

//====================================================================================
// OOB BOUNDARY GRID (1.40 parity, GPU pass in rt.cpp grDrawOOBGrid). 1.40 pref names +
// defaults; Style is ours: 0=classic static grid, 1=animated (default), 2=plasma.
// rt.cpp treats OOBGridVisible unset as ON; seeded here so a future Options row reads 1.
if($pref::OOBGridVisible == "")      { $pref::OOBGridVisible = 1; }
if($pref::OOBGridAlpha == "")        { $pref::OOBGridAlpha = 0.45; }
if($pref::OOBGridPercent == "")      { $pref::OOBGridPercent = 0.4; }
if($pref::OOBGridStyle == "")        { $pref::OOBGridStyle = 5; }
if($pref::OOBGridSpeed == "")        { $pref::OOBGridSpeed = 1; }
if($pref::OOBGridSpacing == "")      { $pref::OOBGridSpacing = 32; }
if($pref::OOBGridColor == "")        { $pref::OOBGridColor = "0.3 0.3 0.6"; }
if($pref::OOBGridColorOutside == "") { $pref::OOBGridColorOutside = "1 0 0"; }

//====================================================================================
// PROTECTED-PREFS SNAPSHOT (paired with the restore at the END of autoexec.cs).
// nativeDefaults.cs is the FIRST thing autoexec runs, i.e. the earliest point after
// the player's ClientPrefs has loaded. Snapshot the collision-prone prefs here; the
// restore re-asserts them after the whole autoexec chain (Presto suite etc.) has run,
// so no later script can stomp a value the player actually saved. Empty snapshot
// (fresh install / new pref) restores nothing.
$PrefGuard::list = "packetRate packetSize packetFrame PlayerFov netVehicleInterpolateTime netVehiclePredictForwardTime interpolateTime predictForwardTime";
for(%i = 0; (%pg = getWord($PrefGuard::list, %i)) != -1; %i++) {
	$PrefGuard::val[%pg] = $pref::[%pg];
}
