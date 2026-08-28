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
// HOVER HELP (2026-08-14): pop-up descriptions on menu buttons and options rows.
// Default ON. $pref::uiHoverHelp is the numeric toggle the modern Options
// Interface tab binds to (FGHelpCtrl honours it alongside the legacy string
// $pref::HelpPopups, which stays untouched here).
//------------------------------------------------------------------------------
if($pref::uiHoverHelp == "") { $pref::uiHoverHelp = 1; }

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
// GATING (2026-08-14 review): seeds REMOVED. Unset means stock (jet nav OFF, tol 0.2);
// the engine defaults these per frame to the BotBrain-mission values (on / 3.0) only while
// $Server::BotBrain owns the mission (FearPlugin.cpp pref refresh). Apocalypse sets and
// restores its own values. Setting either pref here still wins over both.

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
// GATING (2026-08-14 review): seed REMOVED -- 4x stock target acquisition was reaching every
// mod's AI with BotBrain off. Unset now means stock (1); BotBrain missions default to 4 via
// the engine's pref refresh; Apocalypse manages its own. Set the pref here to pin a value.

//------------------------------------------------------------------------------
// GATING one-time reset (2026-08-15, audit finding): the seed-era values PERSISTED.
// Every install that booted while nativeDefaults seeded aiJetNav/aiJetNavTol/aiScanPerTick
// exported them into ClientPrefs.cs on quit, and a SET pref wins over the new
// BotBrain-conditional defaults by design -- so on existing installs the gating was inert
// forever. Same pattern as the font-set reset above: once ever, clear the three prefs IF
// they still hold exactly the old seeded values (1 / 3 / 4 -- any other value is a
// deliberate operator pin and survives), then never run again. Empty = unset to every
// reader, which re-enables the stock-unless-BotBrain defaults.
//------------------------------------------------------------------------------
if($pref::aiSeedResetV1 == "")
{
   if($pref::aiJetNav == 1)      { $pref::aiJetNav = ""; }
   if($pref::aiJetNavTol == 3)   { $pref::aiJetNavTol = ""; }
   if($pref::aiScanPerTick == 4) { $pref::aiScanPerTick = ""; }
   $pref::aiSeedResetV1 = 1;
   echo("[GATING] one-time AI seed-era preference reset applied");
}

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

echo("[NATIVE] defaults applied: net " @ $pref::PacketSize @ "/" @ $pref::PacketRate @ "/" @ $pref::PacketFrame @ "  aiJetNav='" @ $pref::aiJetNav @ "' aiScanPerTick='" @ $pref::aiScanPerTick @ "' (empty = stock, or BotBrain-mission defaults)");

// WHICH KEYBIND SET THIS SESSION BOOTED INTO.
//
// (Reconciled from the play tree 2026-08-14 by the commit-audit session: the WASD keybind work
// (11751b0) added this block to the play-tree copy only, and the repo copy diverged -- the audit's
// 3593589 follow-up. Content below is verbatim from the play tree.)
//
// console.cs:189 execs sae.cs, then saeModern.cs when $modList is the single word "base" (no -mod).
// saeModern.cs records $BindSet::active; empty means a -mod launch, which keeps that mod's own
// sae.cs and never reaches saeModern.
//
// DEFINED HERE, ARMED FROM autoexec.cs, REPORTED ON A DELAY -- all three parts are load-bearing:
//
//   * not inline, because the console log file is not open yet at this point in the boot chain.
//     Measured on a harness client: this file's own "[NATIVE] defaults applied" echo a few lines
//     up does NOT reach console.log, while the ModernHUD lines from Presto\install.cs
//     (autoexec.cs:7, two lines after this file is exec'd) do. Logging opens in between.
//   * not scheduled from HERE either, which was the first attempt and produced nothing:
//     console.cs creates ConsoleScheduler AFTER it execs autoexec.cs, so a schedule() call at
//     exec time has no scheduler and is silently dropped. autoexec.cs already documents this
//     constraint for KronosVideo::reapply ("called right before the startup apply, when the
//     console scheduler exists") -- the arming call lives in that same OptionsVideo::validate
//     override, and this function is armed alongside it.
function BindSet::report()
{
   if($BindSet::active == "")
   {
      echo("[BINDS] boot set: mod/stock sae.cs (-mod launch, modern seed skipped)");
   }
   else
   {
      echo("[BINDS] boot set: " @ $BindSet::active);
   }
}

// Called inline HERE for the dedicated server, and again on a delay from autoexec.cs for the
// client. Not a duplicate in either process: a dedicated server sets $Console::LogMode=1 at
// console.cs:22 so this inline line lands in console_host.log, and it never reaches the delayed
// arming at all (OptionsVideo::validate is in the client-only branch of console.cs -- there is
// no video to validate on a dedicated server). On the client the reverse holds: this inline call
// is swallowed because logging is not open yet, and the scheduled one is the copy you see.
BindSet::report();

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

//====================================================================================
// HOVER HELP (2026-08-14): tooltip text for screens/dialogs with no authored
// helpTags. Definitions only -- the native client invokes <Screen>Gui::onHelpSetup()
// when a screen or dialog opens, so this is inert under an older exe.
//====================================================================================
exec("hoverHelp.cs");

//====================================================================================
// GLB SHAPES (2026-08-16, Joe's ship call): the repaired .glb models ship in v16 --
// the 28 Mech Mayhem hercs (cel-strip/interior/mount/window/LOD fixes) plus the
// earlier base-asset conversions (~34 files, all in base\). The engine prefers
// <shape>.glb over .dts only when this master gate is on (ts_gltf.cpp resolver via
// resManager shapePref); without it every shipped GLB is inert bytes and mechs
// render the old inverted-wall .dts. Guarded: an explicit 0 survives.
//====================================================================================
if($pref::gltfShapes == "") { $pref::gltfShapes = 1; }

//====================================================================================
// BAKE LIGHTING AT LOAD (2026-08-23, Joe's ship call for v18): the raytraced
// relight-at-load can hitch on load and is not yet proven across different PCs,
// so it ships OFF. The engine treats an ABSENT $pref::gpuLmapBake as ON (mode 3,
// rt.cpp "absent bake -> 3"), so absence is not enough -- seed the explicit 0.
// Guarded: a player who turned it ON in Options -> Graphics keeps it.
//====================================================================================
if($pref::gpuLmapBake == "") { $pref::gpuLmapBake = 0; }

//====================================================================================
// PDA ZOOM KEYS (2026-08-27): restore the two pdaMap binds every preset declares
// (saeModern.cs:193-194, saeBase.cs, saeRPG.cs, base/scripts/sae.cs:129-130) and that
// NOBODY has actually had since forever. cowboy/CmdHUD.cs ran NewActionMap("pdaMap.sae")
// from autoexec on EVERY boot, which CLEARS the map, then bound only its enter toggle --
// so the presets' z zoom binds were wiped before the player ever saw them, and the
// wiped state is what exit-save wrote back to config.cs. CmdHUD now uses EditActionMap,
// but that alone does not help an EXISTING install: its config.cs still carries a pdaMap
// block holding nothing but enter, and config.cs does its own NewActionMap on load.
//
// bindDefault is the right tool and the reason it exists (see repack-whitelist.txt:130):
// it applies ONLY when the action is unbound AND the key is free, so this restores the
// zoom for everyone who lost it, skips silently once it is present, and never touches a
// player who deliberately put something else on z or moved zoom elsewhere.
//====================================================================================
editActionMap("pdaMap.sae");
bindDefault(keyboard0, make, "z", TO, IDACTION_ZOOM_MODE_ON);
bindDefault(keyboard0, break, "z", TO, IDACTION_ZOOM_MODE_OFF);
