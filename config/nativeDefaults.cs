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
// 1b. THE OPTIONS VALIDATOR -- the SECOND owner of those same three prefs.
//
// Setting the floor above is only half the job: base/scripts/Options.cs defines
// OptionsNetwork::validate(), which OptionsGui::onOpen runs through OptionsNetwork::init()
// and onClose again through shutdown(). Its 1998 body clamps packetRate to 30, packetSize
// to 500, and rewrites any packetFrame below 32 to 96. So merely OPENING Options undid the
// floor above -- and the next boot put it back. A permanent 16 -> 96 -> 16 fight.
//
// MEASURED 2026-08-28: client booted on 60/800/16, opened Options, read 30/500/96.
//
// It is redefined HERE, and not in the C++ (configModules.cpp, where it used to live and
// never once took effect), because a console function is late-bound and console.cs execs in
// this order:
//     :264  exec("Options.cs")     <- the 1998 definition
//     :267  ExecModScripts()       <- each mod ships its own copy; 11 of them in this tree
//     :291  exec("autoexec.cs")    <- execs THIS file
// Last definition wins, and this file is last. That also means one definition covers every
// mod, instead of patching eleven copies of Options.cs.
//
// Bounds mirror PacketStream::checkMaxRate (netPacketStream.cpp:344-358) exactly:
// rate 1..100, size 100..1000, frame 16..128. A value outside the range is CLAMPED to the
// nearest legal one -- never converted into some unrelated number, which is what the "< 32
// then 96" line did.
//------------------------------------------------------------------------------
function OptionsNetwork::validate()
{
   if($pref::AutoRefresh == "") { $pref::AutoRefresh = TRUE; }
   if($Server::HostPublicGame == "") { $Server::HostPublicGame = FALSE; }

   if($pref::packetRate < 1)    { $pref::packetRate = 10; }
   if($pref::packetRate > 100)  { $pref::packetRate = 100; }
   Control::setValue(NetworkPacketRate, $pref::packetRate);

   if($pref::packetSize < 100)  { $pref::packetSize = 200; }
   if($pref::packetSize > 1000) { $pref::packetSize = 1000; }
   Control::setValue(NetworkPacketSize, $pref::packetSize);

   if($pref::packetFrame < 16)  { $pref::packetFrame = 16; }
   if($pref::packetFrame > 128) { $pref::packetFrame = 128; }
   OptionsNetwork::setPacketFrame();
}

// The legacy NetworkPacketFrame control is an INVERTED 0..1 fraction. Both halves of the
// stock mapping hardcode the 32 ms modem bottom, and the get half is therefore a hard FLOOR
// of 32 -- value 0 yields 32, so the engine's real minimum of 16 was unreachable through
// that control no matter what validate() did. Re-mapped over 16..128, so 16 sits exactly at
// value 1.0 (the "fastest" end) and the round trip is lossless.
//
// Modern Options does not use this control at all -- it binds pref::packetFrame directly --
// but $pref::optionsModern = 0 still gets the authored 1998 screen, and it should work.
function OptionsNetwork::setPacketFrame()
{
   %value = 1.0 - (($pref::packetFrame - 16) / (128 - 16));
   Control::setValue(NetworkPacketFrame, %value);
}

function NetworkPacketFrame::onAction()
{
   %value = 1.0 - Control::getValue(NetworkPacketFrame);
   $pref::packetFrame = 16 + (%value * (128 - 16));
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

//====================================================================================
// KEYMAP SAVER (2026-08-28). ONE definition of "write the keymap", called by the two
// exit paths (base\scripts\GUI.CS, config\Presto\events.cs -- the latter overrides the
// former by namespace collision) and by the native save-on-edit flush.
//
// It exists because those two sites had drifted: GUI.CS branched on $crurpg_config,
// events.cs on String::findSubStr($modList, "crurpg"), and the two can disagree. The
// Crucible RPG branch is gone entirely as of today (see console.cs), so there is now
// exactly one destination and no test to get wrong.
//
// $Repack::keymapSaver is the readiness flag the native flush checks before evaluating
// this -- a boot-time flush must not call a function that has not been defined yet.
//====================================================================================
function Repack::saveKeymap()
{
	saveActionMap("config\\config.cs", "actionMap.sae", "playMap.sae", "pdaMap.sae");
}
$Repack::keymapSaver = 1;

//====================================================================================
// CASTER TOOLS (2026-08-28) -- client-side console wrappers for the shoutcaster
// observer cameras (base\scripts\caster.cs on the server). Thin by design: the
// SERVER enforces the role gate (isAdmin, or isCaster granted by an admin) and
// validates every argument, so these are just typing convenience for casters and
// something to bind keys to. No default keybinds on purpose (config.cs is user
// state; see the keybind-clobber history).
//
//   CasterFollow(0);    360 orbit cam glued to team 0's flag -- tracks the carrier
//                       while carried, the flag itself at base / dropped
//   CasterFlagCam(1);   360 orbit cam around team 1's flag stand
//   CasterDist(20);     orbit distance in meters (server clamps to 3..30)
//   CasterStop();       detach
//   CasterGrant(id,1);  admin only: grant (1) / revoke (0) caster tools
//====================================================================================
function CasterFollow(%team)
{
	remoteEval(2048, "CasterFollow", %team);
}
function CasterFlagCam(%team)
{
	remoteEval(2048, "CasterFlagCam", %team);
}
function CasterDist(%meters)
{
	remoteEval(2048, "CasterDist", %meters);
}
function CasterStop()
{
	remoteEval(2048, "CasterStop");
}
function CasterGrant(%clientId, %on)
{
	remoteEval(2048, "CasterGrant", %clientId, %on);
}
function CasterHelp()
{
	remoteEval(2048, "CasterHelp");
}

//====================================================================================
// AUTO-RECORD + MOMENT INDEX (2026-08-28) -- client side of caster stage 2.
//
// $pref::casterAutoRecord = 1 records EVERY match to recordings\auto-<datetime>.rec
// and writes a sidecar index (recordings\auto-<datetime>.rec.events.cs) of the
// moments the server attributes to you (MA, CK, GRAB, CAP, RETURN). Default OFF --
// recording every match costs disk and is a player choice. Turn on from the console:
//   $pref::casterAutoRecord = 1;   (takes effect from the NEXT connect)
//
// HOW THE NAME HANDOFF WORKS (order is the whole trick): the engine reads
// $recorderFileName ONCE, at connect (netCSDelegate.cpp:339). So boot arms name #1;
// when the server's caster.cs says hello (remoteCasterHello, shortly after connect)
// we remember name #1 as THIS match's recording and immediately arm name #2 for the
// next connect. No disconnect detection needed. On servers without caster.cs there
// is no hello, so a second connect that session reuses (overwrites) the same file --
// documented v1 caveat, updated servers do not have it.
//
// The index is rewritten whole on every moment (crash-safe, no append machinery) and
// is exec()-able: $CasterRec::count, $CasterRec::rec, $CasterRec::moment[i] =
// "type | detail | seconds-into-match | wall clock". seconds-into-match is
// getSimTime() minus the hello stamp -- close enough to seek a playback.
//
// Manual recordings: leave the pref OFF and nothing here ever touches
// $recorderFileName.
//====================================================================================
if($pref::casterAutoRecord == "")
	$pref::casterAutoRecord = 0;

function Caster::nextRecName()
{
	%ts = timestamp();
	%name = "recordings\\auto-" @ String::getSubStr(%ts, 0, 10) @ "-" @ String::getSubStr(%ts, 11, 2) @ String::getSubStr(%ts, 14, 2) @ String::getSubStr(%ts, 17, 2) @ ".rec";
	$recorderFileName = %name;
	$Caster::pendingRec = %name;
}

if($pref::casterAutoRecord == 1)
	Caster::nextRecName();

function remoteCasterHello(%server)
{
	if($pref::casterAutoRecord != 1)
		return;
	$Caster::curRec = $Caster::pendingRec;
	$Caster::momentCount = 0;
	$Caster::connectSim = getSimTime();
	Caster::nextRecName();
	echo("[CASTER] auto-record: this match -> " @ $Caster::curRec);
}

function remoteCasterMoment(%server, %type, %detail, %mission)
{
	if($pref::casterAutoRecord != 1)
		return;
	if($Caster::curRec == "")
		return;
	%i = $Caster::momentCount;
	$Caster::momentCount = %i + 1;
	$CasterRec::count = $Caster::momentCount;
	$CasterRec::rec = $Caster::curRec;
	$CasterRec::mission = %mission;
	$CasterRec::moment[%i] = %type @ " | " @ %detail @ " | " @ floor(getSimTime() - $Caster::connectSim) @ "s | " @ timestamp();
	export("CasterRec::*", $Caster::curRec @ ".events.cs", False);
	echo("[CASTER] moment " @ %type @ " -> " @ $Caster::curRec @ ".events.cs");
}

//====================================================================================
// DEMO FREE-CAM + SEEK (2026-08-29) -- client side of caster stage 4.
//
// While a recording (.rec) plays back, the engine can swap the recorded first-person
// view for a film camera orbiting the recorded player's eye point. $pref::demoFreeCam
// gates it (default OFF = stock playback); $DemoCam::yaw/pitch (degrees, world
// absolute) and $DemoCam::dist (meters, 1..200) aim it, re-read every frame by the
// engine (simGuiTSCtrl.cpp). getDemoTime() / demoSeek(seconds) are engine commands
// (netPlugin.cpp). Seek is FORWARD-only -- a .rec is a sequential packet log -- and
// replays every skipped event instantly, so expect a short burst of sounds/effects
// on a long jump. To go BACK, restart the demo and seek forward.
//
// CONTROLS: the numpad, through the same sendControl relay the live caster cameras
// use. During playback that relay has no server to talk to, so this copy of
// sendControl (nativeDefaults runs via autoexec.cs AFTER client.cs:809 exec'd
// repack.cs, so it wins at boot) grows a playback-only branch; outside playback it
// falls through to the stock repack body verbatim. Mods that redefine sendControl at
// connect time (RPG remote.cs) still win live play; if you play a modded server and
// THEN watch a demo in the same session, the mod's copy may eat the numpad -- restart
// the game before filming. The .rec.events.cs sidecar index (auto-record, above)
// gives the timestamps worth seeking to.
//
//   numpad1     free-cam ON/OFF          numpad0    free-cam OFF
//   numpad4/6   orbit left/right (yaw)   numpad8/2  camera up/down (pitch)
//   numpad+/-   distance +/- 5 m
//   numpad9     seek +10 s               numpad3    seek +60 s
//   numpad7     telestrator (draw on screen; see the TELESTRATOR section below)
//   numpad*     help, on screen
//
// Steps are tunable: $DemoCam::yawStep/pitchStep/distStep/seekShort/seekLong.
//====================================================================================
if($DemoCam::yaw == "") $DemoCam::yaw = 0;
if($DemoCam::pitch == "") $DemoCam::pitch = 20;
if($DemoCam::dist == "") $DemoCam::dist = 12;
if($DemoCam::yawStep == "") $DemoCam::yawStep = 15;
if($DemoCam::pitchStep == "") $DemoCam::pitchStep = 10;
if($DemoCam::distStep == "") $DemoCam::distStep = 5;
if($DemoCam::seekShort == "") $DemoCam::seekShort = 10;
if($DemoCam::seekLong == "") $DemoCam::seekLong = 60;

function DemoCam::print(%msg)
{
	$centerPrintId++;
	schedule("clearCenterPrint(" @ $centerPrintId @ ");", 3);
	Client::centerPrint(%msg, 1);
}

function DemoCam::status()
{
	if($pref::demoFreeCam == 1)
		%s = "ON";
	else
		%s = "off";
	DemoCam::print("FreeCam " @ %s @ "   yaw " @ $DemoCam::yaw @ "   pitch " @ $DemoCam::pitch @ "   dist " @ $DemoCam::dist @ "m   t=" @ getDemoTime() @ "s");
}

function DemoCam::key(%key)
{
	if(String::ICompare(%key, "numpad1") == 0)
	{
		if($pref::demoFreeCam == 1)
			$pref::demoFreeCam = 0;
		else
			$pref::demoFreeCam = 1;
		DemoCam::status();
		return true;
	}
	if(String::ICompare(%key, "numpad0") == 0)
	{
		$pref::demoFreeCam = 0;
		DemoCam::status();
		return true;
	}
	if(String::ICompare(%key, "numpad4") == 0)
	{
		%y = $DemoCam::yaw - $DemoCam::yawStep;
		if(%y < 0)
			%y = %y + 360;
		$DemoCam::yaw = %y;
		DemoCam::status();
		return true;
	}
	if(String::ICompare(%key, "numpad6") == 0)
	{
		%y = $DemoCam::yaw + $DemoCam::yawStep;
		if(%y >= 360)
			%y = %y - 360;
		$DemoCam::yaw = %y;
		DemoCam::status();
		return true;
	}
	if(String::ICompare(%key, "numpad8") == 0)
	{
		%p = $DemoCam::pitch + $DemoCam::pitchStep;
		if(%p > 85)
			%p = 85;
		$DemoCam::pitch = %p;
		DemoCam::status();
		return true;
	}
	if(String::ICompare(%key, "numpad2") == 0)
	{
		%p = $DemoCam::pitch - $DemoCam::pitchStep;
		if(%p < -85)
			%p = -85;
		$DemoCam::pitch = %p;
		DemoCam::status();
		return true;
	}
	if(String::ICompare(%key, "numpad+") == 0)
	{
		%d = $DemoCam::dist + $DemoCam::distStep;
		if(%d > 200)
			%d = 200;
		$DemoCam::dist = %d;
		DemoCam::status();
		return true;
	}
	if(String::ICompare(%key, "numpad-") == 0)
	{
		%d = $DemoCam::dist - $DemoCam::distStep;
		if(%d < 1)
			%d = 1;
		$DemoCam::dist = %d;
		DemoCam::status();
		return true;
	}
	if(String::ICompare(%key, "numpad9") == 0)
	{
		demoSeek(getDemoTime() + $DemoCam::seekShort);
		DemoCam::status();
		return true;
	}
	if(String::ICompare(%key, "numpad3") == 0)
	{
		demoSeek(getDemoTime() + $DemoCam::seekLong);
		DemoCam::status();
		return true;
	}
	if(String::ICompare(%key, "numpad*") == 0)
	{
		DemoCam::print("DEMO FREE-CAM\nnumpad1 on/off   numpad0 off\nnumpad4/6 orbit   numpad8/2 raise/lower   numpad+/- distance\nnumpad9 seek +10s   numpad3 seek +60s   numpad7 telestrator");
		return true;
	}
	return false;
}

function sendControl(%val, %mod, %release)
{
	// DEMO FREE-CAM branch (caster stage 4): getDemoTime() is -1 unless a .rec is
	// actually playing, so live play never enters here. Presses only -- releases
	// fall through like stock (their remoteEval goes nowhere in playback, harmless).
	if(%release == "" || %release == 0)
	{
		// TELESTRATOR (caster stage 5): numpad7 cycles the on-screen marker. Active
		// during playback always; in LIVE play only when the caster opted in with
		// $pref::casterDraw = 1 -- an unconditional grab would eat numpad7 from mods
		// (RPG remote.cs binds numpad keys to real functions).
		if(String::ICompare(%val, "numpad7") == 0 && (getDemoTime() != -1 || $pref::casterDraw == 1))
		{
			Telestrator::cycle();
			return;
		}
		if(getDemoTime() != -1)
		{
			if(DemoCam::key(%val))
				return;
		}
	}
	// Stock repack.cs:63 body, verbatim, so live play is untouched.
	if(string::getsubstr(%val, 0, 1) == "f"){//repack 36
		if(urlhud::isenabled()){
			if(%val == "f1")
				open(-2);
			else if(%val == "f2")
				urlhud::reset();
			return;
		}
	}
	if(%release)
		remoteEval(2048,ReleaseKey,%val, %mod);
	else
		remoteEval(2048,RawKey,%val, %mod);
}

//====================================================================================
// CASTER MODIFIER (2026-08-29) -- reach the numpad caster controls on a keyboard that
// has no numpad. Joe plays on a tenkeyless: every caster and film-camera control is a
// numpad key (extra-controls.cs binds numpad0-9 + - * to sendControl), so on a TKL the
// entire shoutcasting feature set is unreachable and Options offered nothing to rebind.
//
// This is ONE assignable hold bind ("Caster Modifier (hold)", fearGuiModernOptions
// kBinds, ships unbound). While it is held, the TOP-ROW digits emit the SAME
// sendControl("numpadN") strings the numpad does -- so the server relay
// (Caster::rawKey), the demo film camera and the telestrator all keep working with no
// change whatsoever on the receiving end.
//
// ★Why an action map and not bindCommand rebinding★: SimActionHandler::push is
// push_front and input matches from the FRONT, so the LAST map pushed wins every key
// collision, and any key it does NOT bind still falls through (dlgPlay.cpp:698 documents
// this -- it was a real bug once). So pushing a map that binds ONLY the digits hijacks
// exactly those for exactly as long as the key is held, and leaves movement, chat and
// everything else alone. No saving/restoring of existing binds, nothing to clobber.
//
// ★TRAP -- NewActionMap CLEARS an existing map★ (simInputPlugin.cpp:757-772: it finds the
// resource and calls ->clear() when it already exists). So NEVER call NewActionMap on
// actionMap.sae or playMap.sae to "restore" the current-map pointer; that would erase
// every stock binding. It also leaves ITS map as the target of later bindCommand calls,
// which is why this builds ONCE, lazily, on the first press -- long after the boot chain
// has finished binding -- instead of at exec time.
$Caster::modHeld = 0;
$Caster::modMapBuilt = 0;

function Caster::buildModMap()
{
	if($Caster::modMapBuilt == 1)
		return;
	$Caster::modMapBuilt = 1;
	NewActionMap("casterMap.sae");
	bindCommand(keyboard0, make, "1", TO, "sendControl(\"numpad1\");");
	bindCommand(keyboard0, make, "2", TO, "sendControl(\"numpad2\");");
	bindCommand(keyboard0, make, "3", TO, "sendControl(\"numpad3\");");
	bindCommand(keyboard0, make, "4", TO, "sendControl(\"numpad4\");");
	bindCommand(keyboard0, make, "5", TO, "sendControl(\"numpad5\");");
	bindCommand(keyboard0, make, "6", TO, "sendControl(\"numpad6\");");
	bindCommand(keyboard0, make, "7", TO, "sendControl(\"numpad7\");");
	bindCommand(keyboard0, make, "8", TO, "sendControl(\"numpad8\");");
	bindCommand(keyboard0, make, "9", TO, "sendControl(\"numpad9\");");
	bindCommand(keyboard0, make, "0", TO, "sendControl(\"numpad0\");");
	bindCommand(keyboard0, make, "=", TO, "sendControl(\"numpad+\");");
	bindCommand(keyboard0, make, "-", TO, "sendControl(\"numpad-\");");
	bindCommand(keyboard0, make, "[", TO, "sendControl(\"numpad*\");");
	echo("[CASTER] modifier map built (top-row digits -> numpad while held)");
}

function Caster::modOn()
{
	if($Caster::modHeld == 1)
		return;
	Caster::buildModMap();
	$Caster::modHeld = 1;
	pushActionMap("casterMap.sae");
}

function Caster::modOff()
{
	if($Caster::modHeld != 1)
		return;
	$Caster::modHeld = 0;
	popActionMap("casterMap.sae");
}

// TELESTRATOR (caster stage 5, 2026-08-29) -- draw on the screen while casting.
//
// numpad7 cycles three states: OFF -> DRAW (a cursor appears; hold the left mouse
// button and drag to mark the screen) -> SHOW (the marks stay up, the mouse goes
// back to the game/camera) -> OFF (marks cleared). Works while watching a recording
// (numpad7 is always live there) and in live play once the caster sets
// $pref::casterDraw = 1; (deliberate opt-in: an unconditional numpad7 grab would
// steal the key from mods that bind it). Drawing is LOCAL -- the caster's screen is
// the stream, so the audience sees the marks through the cast.
//
// Strokes are stored in normalized 0..1 coordinates (resolution independent) and
// rendered as thick quads from ScriptGL's onPostDraw hook. That hook is owned by
// config\Presto\KronosShop.cs (LAST definition wins -- see its own comment), which
// calls Telestrator::render(%dimensions) guarded by isFunction. Tunables:
// $pref::telestratorWidth (px), $pref::telestratorColor ("r g b" bytes).
//====================================================================================
if($pref::telestratorWidth == "") $pref::telestratorWidth = 4;
// WORLD-ANCHORED MARKS (2026-08-29). 0 = the original behaviour: marks are stored in screen
// space and sit still while the camera moves -- right for circling something in a freeze
// frame. 1 = each point is anchored to the WORLD where it was drawn, so a route traced over
// terrain stays on that terrain as the camera moves, which is what a route callout needs.
// Costs a projection per point per frame, so if a long stroke ever feels heavy this is the
// knob to try first.
if($pref::telestratorWorld == "") $pref::telestratorWorld = 1;
// ONE-TIME migration, same idiom as the Font Set reset above. cef718d shipped this pref
// defaulted to 0 because world mode was broken, and the exit export("pref::*") froze that 0
// into every ClientPrefs.cs that ran it. A saved 0 outlives a default change, so flipping the
// line above reaches nobody who took that build -- including the machine this was fixed on.
// Re-seed exactly once; the marker survives the export sweep, so a deliberate 0 set AFTER the
// migration is never touched again.
if($pref::telestratorWorldFixV2 == "")
{
   $pref::telestratorWorld = 1;
   $pref::telestratorWorldFixV2 = 1;
   echo("[TELESTRATOR] one-time world-anchored default restored");
}
// A pixel aimed at open sky hits nothing to anchor to. Rather than drop the point and tear a
// hole in the line, anchor it this far along the ray -- it then floats and drifts with
// parallax, which is honest for a mark drawn on nothing.
if($pref::telestratorSkyDist == "") $pref::telestratorSkyDist = 150;
if($pref::telestratorColor == "") $pref::telestratorColor = "255 40 40";
$Tele::mode = 0;
$Tele::strokes = 0;
$Tele::maxPts = 400;
$Tele::totalPts = 0;

function Telestrator::clear()
{
	$Tele::strokes = 0;
	$Tele::totalPts = 0;
	$Tele::penDown = 0;
}

function Telestrator::cycle()
{
	if($Tele::mode == 0)
	{
		$Tele::mode = 1;
		Telestrator::clear();
		cursorOn(MainWindow);
		DemoCam::print("TELESTRATOR: draw with the left mouse button. numpad7 = keep marks, again = clear.");
	}
	else if($Tele::mode == 1)
	{
		$Tele::mode = 2;
		$Tele::penDown = 0;
		CursorOff(MainWindow);
		DemoCam::print("TELESTRATOR: marks pinned. numpad7 clears them.");
	}
	else
	{
		$Tele::mode = 0;
		Telestrator::clear();
		DemoCam::print("TELESTRATOR off.");
	}
}

// Called every frame from ScriptGL::playGui::onPostDraw (KronosShop.cs).
//
// ★TWO CALLERS, ONE PAINT.★ KronosShop.cs owns onPostDraw, but autoexec.cs only execs
// it inside `if($Config::Name == "")` (:128-165) -- so under an active 1.4 custom
// config this entry never fires and the telestrator silently did not render: keys
// worked, strokes were stored, nothing appeared. The fix is the ModernHUD shoutcaster
// pack, whose ModernHUDPack::draw is dispatched by FIXED NAME from the engine
// (ScriptGL_renderHook -> ModernHUD::onDraw) and therefore runs under every config.
// On a default config BOTH callers exist, and painting translucent strokes twice
// darkens them, so this entry stands down while that pack owns the draw. The real
// work lives in Telestrator::paint, which the pack calls directly.
function Telestrator::render(%dims)
{
	if($ModernHUD::LoadComplete == "shoutcaster")
		return;
	Telestrator::paint(%dims);
}

function Telestrator::paint(%dims)
{
	if($Tele::mode == 0)
		return;
	%w = getWord(%dims, 0);
	%h = getWord(%dims, 1);
	if(%w == "" || %w == 0 || %h == "" || %h == 0)
		return;

	// Capture while drawing: sample the mouse, append points >2px apart.
	if($Tele::mode == 1)
	{
		%mp = glMousePos();
		%mx = getWord(%mp, 0);
		%my = getWord(%mp, 1);
		%lmb = getWord(%mp, 2);
		if(%lmb == 1 && $Tele::totalPts < $Tele::maxPts)
		{
			if($Tele::penDown == 0)
			{
				%s = $Tele::strokes;
				$Tele::strokes = %s + 1;
				$Tele::pts[%s] = 0;
				$Tele::penDown = 1;
			}
			%s = $Tele::strokes - 1;
			%n = $Tele::pts[%s];
			%add = 0;
			if(%n == 0)
				%add = 1;
			else
			{
				if($pref::telestratorWorld == 1)
				{
					// In world mode the stored point is world XYZ, so the screen-space spacing
					// test has to project it back first -- comparing a world coord against a
					// pixel would append a point every single frame.
					%prev = glWorldToScreen($Tele::px[%s, %n - 1], $Tele::py[%s, %n - 1], $Tele::pz[%s, %n - 1]);
					if(%prev == "")
						%add = 1;
					else
					{
						%dx = %mx - getWord(%prev, 0);
						%dy = %my - getWord(%prev, 1);
						if(%dx > 2 || %dx < -2 || %dy > 2 || %dy < -2)
							%add = 1;
					}
				}
				else
				{
					%dx = %mx - $Tele::px[%s, %n - 1] * %w;
					%dy = %my - $Tele::py[%s, %n - 1] * %h;
					if(%dx > 2 || %dx < -2 || %dy > 2 || %dy < -2)
						%add = 1;
				}
			}
			if(%add)
			{
				if($pref::telestratorWorld == 1)
				{
					// Anchor this pixel to the world ONCE, here. glScreenToWorldRay inverts the
					// same projection the engine draws with; GetLosInfo then finds what the ray
					// actually hit (mask 3 = terrain + interiors + statics -- the things a route
					// is drawn ON). $los::position is the hit point.
					%ray = glScreenToWorldRay(%mx, %my);
					if(%ray != "")
					{
						%ro = getWord(%ray, 0) @ " " @ getWord(%ray, 1) @ " " @ getWord(%ray, 2);
						%re = getWord(%ray, 3) @ " " @ getWord(%ray, 4) @ " " @ getWord(%ray, 5);
						if(GetLosInfo(%ro, %re, 3) == "True")
							%wp = $los::position;
						else
						{
							// Sky: no anchor exists, so place it a fixed distance down the ray.
							%d = $pref::telestratorSkyDist;
							%wp = (getWord(%ray,0) + (getWord(%ray,3) - getWord(%ray,0)) * %d / 2000)
							  @ " " @ (getWord(%ray,1) + (getWord(%ray,4) - getWord(%ray,1)) * %d / 2000)
							  @ " " @ (getWord(%ray,2) + (getWord(%ray,5) - getWord(%ray,2)) * %d / 2000);
						}
						$Tele::px[%s, %n] = getWord(%wp, 0);
						$Tele::py[%s, %n] = getWord(%wp, 1);
						$Tele::pz[%s, %n] = getWord(%wp, 2);
						$Tele::pts[%s] = %n + 1;
						$Tele::totalPts++;
					}
				}
				else
				{
					$Tele::px[%s, %n] = %mx / %w;
					$Tele::py[%s, %n] = %my / %h;
					$Tele::pts[%s] = %n + 1;
					$Tele::totalPts++;
				}
			}
		}
		else if(%lmb != 1)
			$Tele::penDown = 0;
	}

	// Draw all strokes as thick segments.
	%hw = $pref::telestratorWidth / 2;
	if(%hw < 1) %hw = 1;
	glColor(getWord($pref::telestratorColor, 0), getWord($pref::telestratorColor, 1), getWord($pref::telestratorColor, 2), 230);
	for(%s = 0; %s < $Tele::strokes; %s++)
	{
		%n = $Tele::pts[%s];
		if(%n == 1)
		{
			// lone click: a small dot
			if($pref::telestratorWorld == 1)
			{
				%sp = glWorldToScreen($Tele::px[%s, 0], $Tele::py[%s, 0], $Tele::pz[%s, 0]);
				if(%sp != "")
				{
					%x = getWord(%sp, 0);
					%y = getWord(%sp, 1);
					glRectangle(%x - %hw, %y - %hw, %hw * 2, %hw * 2);
				}
			}
			else
			{
				%x = $Tele::px[%s, 0] * %w;
				%y = $Tele::py[%s, 0] * %h;
				glRectangle(%x - %hw, %y - %hw, %hw * 2, %hw * 2);
			}
		}
		// Seed the carried projection for the world-mode loop below.
		if($pref::telestratorWorld == 1 && %n > 1)
			%a = glWorldToScreen($Tele::px[%s, 0], $Tele::py[%s, 0], $Tele::pz[%s, 0]);
		for(%i = 1; %i < %n; %i++)
		{
			%skip = 0;
			if($pref::telestratorWorld == 1)
			{
				// Re-project against the LIVE camera. glWorldToScreen returns "" for a point
				// behind the eye, and a segment with one end behind the camera cannot be drawn
				// from two screen points -- it would snap across the view. Dropping just that
				// segment leaves the rest of the stroke intact, which reads as the line running
				// off the edge rather than as a glitch.
				//
				// Only the FAR end is projected here: consecutive segments share a point, so
				// carrying the previous %b forward halves the projections per frame (400 points
				// = 400 calls, not 800). %a is seeded before the loop.
				%b = glWorldToScreen($Tele::px[%s, %i], $Tele::py[%s, %i], $Tele::pz[%s, %i]);
				if(%a == "" || %b == "")
					%skip = 1;
				else
				{
					%x1 = getWord(%a, 0);
					%y1 = getWord(%a, 1);
					%x2 = getWord(%b, 0);
					%y2 = getWord(%b, 1);
				}
				%a = %b;
			}
			else
			{
				%x1 = $Tele::px[%s, %i - 1] * %w;
				%y1 = $Tele::py[%s, %i - 1] * %h;
				%x2 = $Tele::px[%s, %i] * %w;
				%y2 = $Tele::py[%s, %i] * %h;
			}
			if(%skip == 1)
				continue;
			%dx = %x2 - %x1;
			%dy = %y2 - %y1;
			%len = Vector::getDistance(%x1 @ " " @ %y1 @ " 0", %x2 @ " " @ %y2 @ " 0");
			if(%len < 1) %len = 1;
			%nx = 0 - (%dy / %len) * %hw;
			%ny = (%dx / %len) * %hw;
			// Vertex order matters: screen-CCW quads are GL back faces here and get
			// silently culled (the glAngledPolygon winding trap). This order survives.
			glAngledPolygon(%x1 - %nx, %y1 - %ny, %x2 - %nx, %y2 - %ny, %x2 + %nx, %y2 + %ny, %x1 + %nx, %y1 + %ny);
		}
	}
}

// PLAYBACK ACROSS MATCH ENDS (caster stage 4). Stock ELM() (client.cs:791) kills
// demo playback the moment the RECORDED match ends: the recorded EnterLobbyMode
// replays, sees $playingDemo, and disconnects -- so a recording that spans a match
// end (auto-record is per-CONNECT, not per-match) could never be watched past match
// one, and demoSeek targets beyond the boundary silently ended the demo. This copy
// (loaded after client.cs) keeps rolling while a demo is genuinely playing:
// getDemoTime() is -1 the instant playback is over, so the stock disconnect branch
// still runs when the recording itself ends.
// 2026-09-03 (player report: "pressing Escape does not exit a demo"). The ELM() guard
// below is right for the RECORDED match-end, but the player's Escape reached the same
// function and was swallowed with it. The engine now sends the player's key here during
// playback (dlgPlay.cpp, SimGame::isPlayback), and this ends the demo unconditionally --
// the same exit stock ELM() takes once a recording is over.
// DEFERRED like stock EnterLobbyMode (schedule("ELM();", 0)): the engine calls this from
// INSIDE the action-map dispatch of the Escape key, and disconnect() tears down the
// play delegate and its maps while SimActionHandler::onSimInputEvent is still walking
// them -- Joe's first try crashed there (simAction.cpp:520, freed-memory read).
function DemoPlaybackEscape()
{
	schedule("DemoPlaybackEscapeNow();", 0);
}

function DemoPlaybackEscapeNow()
{
	setCursor(MainWindow, "Cur_Arrow.bmp");
	disconnect();
	startMainMenuScreen();
	GuiLoadContentCtrl(MainWindow, "gui\\Recordings.gui");
}

function ELM()
{
	if($playingDemo && getDemoTime() != -1)
		return;
	if($playingDemo)
	{
		setCursor(MainWindow, "Cur_Arrow.bmp");
		disconnect();
		startMainMenuScreen();
		GuiLoadContentCtrl(MainWindow, "gui\\Recordings.gui");
		return;
	}
	$InLobbyMode = true;
	GuiLoadContentCtrl(MainWindow, "gui\\Lobby.gui");
	CursorOn(MainWindow);
}

//------------------------------------------------------------------------------
// Mech Mayhem turn-rate cap -- CLIENT channel.
//
// Player::integrateMoveRotation clamps a herc's yaw to its hercRot() pair. The
// datablock fields carrying those numbers are UNPACKED (v16 wire compatibility),
// so a client never gets them with the ghost -- the server pushes them here
// instead, via MechMayhem::pushTurnCap on every loadout grant.
//
// This lives in nativeDefaults rather than in the mechcockpit HUD pack ON PURPOSE.
// The client MUST clamp with the same numbers as the server: it predicts its own
// view, and if it did not clamp, the player would aim somewhere the server does not
// agree with -- an aim desync, not a cosmetic one. Putting the handler in a HUD pack
// would make correct aim depend on having that pack enabled.
//
// Inert everywhere else: a non-mech server never calls it, and the engine treats a
// zero cap as uncapped.
// UNTRUSTED INPUT BY CONSTRUCTION: this handler is reachable from every server the
// player ever joins, so treat the arguments as malformed until proven otherwise. The
// interesting case is not a hostile server (one already owns your movement authority
// and can do worse) but a BUGGY or older mech server sending "" or a non-numeric --
// a bad clamp on the client's own view prediction reads to the player as "my mouse is
// broken", and nothing in that experience points back at the server.
//
// +0 coerces a non-numeric to 0, and the engine reads any cap below 1 deg/s as absent,
// so every malformed value fails OPEN to the uncapped behaviour that shipped before.
function remoteMMTurn(%server, %slow, %fast)
{
   %slow = %slow + 0;
   %fast = %fast + 0;
   if (%slow < 0) %slow = 0;
   if (%fast < 0) %fast = 0;
   $MMC::turnSlow = %slow;
   $MMC::turnFast = %fast;
}

//====================================================================================
// GAMEPLAY SCRIPTS (2026-08-30) -- the classic community QoL modules (retro 1.41's
// config\Modules\*.acs.cs), ported here behind $pref::script* toggles. Each has a row
// on Options > Scripts (fearGuiModernOptions.cpp g_scripts); everything ships OFF.
//
// AUTOKIT (port of autokit.acs.cs): use a Repair Kit automatically when health drops
// below 65 (engine exports $Health 0-100 every frame, CfgSyncHudVars_now). The tick
// chain rides schedule(), which dies with the sim manager on disconnect -- so
// eventConnected (fired by events.cs dataFinished on every connect/mission load)
// restarts it, and the generation stamp keeps a restart from stacking a second chain.
// The PREF gates the tick BODY, not the chain, so the Options toggle applies on the
// next tick with no reconnect.
//
// Station hold: at an inventory station healing is free, so firing a kit there wastes
// it. The classic script watched $Station::Type (set by community config packs'
// Core\Station.cs); our stock GUI signal is $Mode::InventoryMode (events.cs
// CmdInventoryGui::onOpen). Watch both, and keep holding 9 seconds after leaving,
// exactly like the original's station defer.
//====================================================================================
if($pref::scriptAutokit == "")
	$pref::scriptAutokit = 0;

function Autokit::tick(%gen)
{
	if(%gen != $Autokit::gen)
		return;
	schedule("Autokit::tick(" @ %gen @ ");", 0.1);
	if($pref::scriptAutokit != 1)
		return;
	if($Station::Type != "" || $Mode::InventoryMode)
	{
		$Autokit::holdUntil = getSimTime() + 9;
		return;
	}
	if(getSimTime() < $Autokit::holdUntil)
		return;
	if($Health <= 0 || $Health >= 65)
		return;
	if(getItemCount("Repair Kit") > 0)
		useItem(getItemType("Repair Kit"));
}

function Autokit::start()
{
	$Autokit::gen = $Autokit::gen + 1;
	$Autokit::holdUntil = 0;
	Autokit::tick($Autokit::gen);
}
Event::Attach(eventConnected, Autokit::start);

//====================================================================================
// DEMO NAMER (port of DemoNamer.acs.cs, replaces stock base\scripts\client.cs
// setupRecorderFile -- this file execs LAST, so this definition wins). Stock named
// demos recordings\recordingN.rec (first free slot: sorts terribly, reuses deleted
// slots); name them by wall clock instead -- recordings\2026-08-30-21.14.05.rec.
// timestamp() format is the TimestampPlugin contract "YYYY-MM-DD HH:MM:SS.mmm"
// (kronosNativeCmds.cpp); colons must NOT reach the filename (the engine sanitizer
// would rewrite them, netCSDelegate.cpp:339), hence dots in the time part.
//
// Callers: the Join page Record Demo checkbox and the modern Host page toggle (both
// eval "setupRecorderFile();"); an explicit %fileName still wins, stock-style. The
// engine reads $recorderFileName ONCE per connect, so eventConnected/eventLeaveServer
// re-arm a FRESH name for the NEXT connect -- without that, a second join with the
// box still ticked reused the old name and overwrote the first demo.
//
// $pref::casterAutoRecord owns $recorderFileName when it is on (see the AUTO-RECORD
// block above) -- never touch the variable in that mode.
//====================================================================================
function setupRecorderFile(%fileName)
{
	if($pref::casterAutoRecord == 1)
		return "True";
	if(!$recordDemo)
	{
		$recorderFileName = "";
		return "True";
	}
	if(%fileName != "" && %fileName != "False")
		$recorderFileName = "recordings\\" @ %fileName;
	else
	{
		%ts = timestamp();
		$recorderFileName = "recordings\\" @ String::getSubStr(%ts, 0, 10) @ "-" @ String::getSubStr(%ts, 11, 2) @ "." @ String::getSubStr(%ts, 14, 2) @ "." @ String::getSubStr(%ts, 17, 2) @ ".rec";
	}
	echo("Recording to - " @ $recorderFileName);
	return "True";
}

function DemoNamer::rearm()
{
	if($pref::casterAutoRecord == 1)
		return;
	setupRecorderFile();
}
Event::Attach(eventConnected, DemoNamer::rearm);
Event::Attach(eventLeaveServer, DemoNamer::rearm);
