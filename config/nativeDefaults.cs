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
if($pref::netKeepSaved != 1)
{
   if($pref::PacketSize == "" || $pref::PacketSize < 800)   { $pref::PacketSize = 800; }
   if($pref::PacketRate == "" || $pref::PacketRate < 60)    { $pref::PacketRate = 60; }
   // Lower is faster here -- it is a minimum interval in ms, so this one is a ceiling.
   if($pref::PacketFrame == "" || $pref::PacketFrame > 16)  { $pref::PacketFrame = 16; }
}

//------------------------------------------------------------------------------
// FONT SCOPE Stage 5: one-time Font Set preference reset (release migration).
//
// Under the OLD global font chain, a Font Set pick silently restyled the shell
// and even the console, and stuck across sessions (the bug the whole font-scope
// work fixes). Every install's saved pref::ModernHUD::FontSet* values therefore
// encode choices made against the WRONG behavior -- reset them exactly once so
// everyone returns to stock faces and re-picks under the v2 rules. The marker
// survives the exit export("pref::*") sweep, so this can never run twice.
// Same wildcard convention export() uses (no leading $).
//------------------------------------------------------------------------------
// ★Explicit empty-assignments, not deleteVariables.★ Measured: the wildcard delete
// ran (echo fired) but the native publisher still read the old value through
// Console->getVariable -- script assignment is the one bridge PROVEN two-way by the
// whole prefs system, so it is the one this migration uses. Empty = unset to every
// reader. The seven shipped pack ids are enumerated; a third-party pack's key
// surviving is harmless (it only applies while that pack is current, and its owner
// chose it).
if($pref::fontSetResetV2 == "")
{
   $pref::ModernHUD::FontSet = "";
   $pref::ModernHUD::FontSet::basic = "";
   $pref::ModernHUD::FontSet::overstep = "";
   $pref::ModernHUD::FontSet::proconfig = "";
   $pref::ModernHUD::FontSet::vantage = "";
   $pref::ModernHUD::FontSet::vector = "";
   $pref::ModernHUD::FontSet::vodka = "";
   $pref::ModernHUD::FontSet::xloader = "";
   $pref::fontSetResetV2 = 1;
   echo("[FONTSCOPE] one-time Font Set preference reset applied");
}

//------------------------------------------------------------------------------
// FONT SCOPE: the boot publication (rev-9 font-context plan, Stage 1).
//
// ClientPrefs has just loaded (we run from the top of autoexec.cs) and the shell
// GUIs have NOT been constructed yet -- console.cs builds them AFTER autoexec
// returns. Publishing here is the only point where both hold; measured with the
// $pref::cfgDiag [FNTSLOT] lines, the first shell fonts seed inside console.cs,
// so the later native publish in main.cpp (kept as the safety net) is too late
// for them. Also snapshots the restart-scoped $pref::fontScopeV2 and closes the
// StockBaseline provenance window. Harmless no-op on builds without the command.
//------------------------------------------------------------------------------
FontScope::bootPublish();

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
   echo("[NATIVE] dedicated: harness loaded, autoRun=" @ $Apoc::autoRun @ " botTarget=" @ $Apoc::botTarget);
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
// DIAGNOSTIC-PREF RESET.
//
// WHY THIS EXISTS. Measured 2026-08-05 on a bot server: $pref::fireDiag had been left on
// since a projectile-origin hunt on 2026-07-28 and was writing 3 lines PER SHOT. With
// $Console::logMode 1 -- which a dedicated server sets, and which OPENS AND CLOSES the
// log file on every single line -- ten bots in combat drove msPerFrame to 132.25
// (7.5 fps) against a 32 ms tick. Clearing it took the same server to 3.91 ms/frame.
// A 34x swing from one forgotten debug flag.
//
// It is not a one-off. export("pref::*") at exit sweeps the WHOLE namespace, so every
// diagnostic anyone ever enables is persisted forever and silently. TEN were found on at
// once. The cost is invisible in normal play -- one human shooting occasionally -- and
// only appears under sustained load, which is exactly what bots produce.
//
// Forces them OFF at boot. Runs from autoexec.cs AFTER ClientPrefs has loaded and assigns
// unconditionally, so it beats any persisted value. Enable a diagnostic at RUNTIME while
// you need it; to keep one across restarts, remove it from this list.
//
// Escape hatch: set $DiagReset::skip = 1 before autoexec runs.
//====================================================================================
if($DiagReset::skip != 1) {
	$DiagReset::list = "fireDiag frameProfile heapDiag aiTaskDiag mouseDiag netTimeoutDiag playerPreviewDiag uiBtnDiag uiRectDiag localSkinDebug uiThemeCoverageDiag ghostSkipDiag srvProfLog allocProfile";
	for(%i = 0; (%dg = getWord($DiagReset::list, %i)) != -1; %i++) {
		if($pref::[%dg] != "" && $pref::[%dg] != "0") {
			echo("[DIAGRESET] $pref::" @ %dg @ " was " @ $pref::[%dg] @ " -- forcing 0 (see nativeDefaults.cs)");
		}
		$pref::[%dg] = "0";
	}
}
