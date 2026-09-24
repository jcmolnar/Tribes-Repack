//==============================================================================
// netlab.cs -- unattended lab client for netcode load testing.
//
// WHAT THIS IS FOR
// ----------------
// Putting N real clients on one server with nobody sitting at them, and getting
// back a machine-readable record of what each connection actually did. The case
// it was written for is a university computer lab: one operator, forty-odd
// machines, one match, and afterwards a single question -- did anything go wrong
// with the connections, and if so, whose.
//
// LAUNCH
// ------
//   TribesNative.exe +exec netlab.cs +netlab LAB07 +name LAB07 +connect 10.0.0.5:28000
//
// +connect, +password, -mod and +exec are STOCK console.cs options and do the
// stock thing. Everything below is read HERE, from the same command line: the
// console.cs argument loop ignores tokens it does not recognise, so these ride
// along untouched and this file re-scans $cargv for them. That is the whole
// reason no engine or console.cs change is needed to add an option -- add it to
// the scan below and it works.
//
//   +netlab <TAG>    turn lab mode ON. TAG names this client's log file
//                    (netlab-<TAG>.log) and appears in every line it writes.
//                    Without +netlab this file does nothing at all.
//   +name <NAME>     the name this client joins under ($PCFG::Name).
//   +labtime <SEC>   quit this many seconds after the client comes up. 0 = never.
//   +labjoin <SEC>   give up on ONE join attempt after this long (default 120),
//                    then spend a rejoin if any are left. A lab machine that
//                    silently sat at a menu for an hour is the failure mode this
//                    exists to prevent.
//   +labretry <N>    rejoin attempts, for a join that fails and for a link that
//                    drops mid-run (default 3; 0 = the old quit-on-anything
//                    behaviour). See WIDE-AREA RUNS below.
//   +labdrop <SEC>   how long a silent link may stay silent before it counts as
//                    a drop (default 30). Runs ALONGSIDE the engine's own
//                    recovery, never shorter than it -- see NetLab::tick.
//   +labvctimeout <MS>  $pref::vcTimeoutMs -- the engine's own silence budget on
//                    an established link (default 30000; 0 = leave it alone).
//   +labsettle <SEC> wait this long after connecting before starting telemetry
//                    (default 15). Mission load freezes the client, and the acks
//                    that follow read as a multi-second round trip; see the
//                    SETTLE block below. Raise it on slow machines or big maps.
//   +labrate <MS>    $pref::PacketRate   -- client send cadence, ms.
//   +labsize <N>     $pref::PacketSize   -- bytes.
//   +labsmooth <N>   $pref::netSmoothMode  0 manual / 1 automatic / 2 smart.
//   +labsnap <0|1>   $pref::netSnapInterp -- MUST be set before connect (the
//                    client only offers the snapshot capability in its hello).
//   +labshot <0|1>   $pref::predictProjectiles.
//   +labrender <0|1> 1 (default) plays normally; 0 drops to the cheapest view
//                    it can, for a machine that cannot hold frame rate. NOTE a
//                    client that cannot render is still a client that cannot
//                    send moves on time, so 0 changes what you are measuring.
//
// A VALUE MUST NOT START WITH '-' OR '+'. console.cs reads the whole command
// line before this file does, and a leading dash would look like an option to it.
//
// WIDE-AREA RUNS (clients here, server somewhere else entirely)
// -------------------------------------------------------------
// The original rig assumed one room and one switch. Pointing it across the
// internet needs no new connect path -- +connect takes a hostname as readily as
// an address (UDPNet.cpp resolves it), and +password rides the stock console.cs
// option -- but it does need the client to stop treating a silent second as the
// end of the world:
//
//   * a real link drops and comes back, and the engine already knows how to ride
//     that out ($pref::vcTimeoutMs, then three 1.50 resume attempts). Quitting on
//     the first zero-rate tick killed that recovery before it started.
//   * a join can fail to something transient, on a machine nobody can go and
//     nudge.
//   * a run's REAL result may be "it reconnected four times", which no
//     per-second line can say on its own.
//
// Hence +labretry / +labdrop, and the GAP / RESUMED / REJOIN lines. On a LAN
// none of them ever fire and the log is byte-for-byte what it always was.
//
// WHAT COMES BACK
// ---------------
// netlab-<TAG>.log, one [NETLAB] line per second (emitted by the engine under
// $pref::netLabLog -- FearPlugin.cpp netStatTick), plus the ordinary console
// output around it, which is where a disconnect or a rejection will explain
// itself. tools\netlab\netlab_report.py merges a roomful of them.
//==============================================================================

function NetLab::arg(%opt)
{
   // Value that follows %opt on the command line, or "" if absent. Indices start
   // at 1, exactly as console.cs's own loop does.
   for(%i = 1; $cargv[%i] != ""; %i++)
   {
      if($cargv[%i] == %opt)
         return $cargv[%i + 1];
   }
   return "";
}

function NetLab::argOr(%opt, %default)
{
   %v = NetLab::arg(%opt);
   if(%v == "")
      return %default;
   return %v;
}

//------------------------------------------------------------------------------
// The once-a-second tick.
//
// Driven by the ENGINE, not by schedule(): this file is exec'd by +exec, which
// console.cs runs BEFORE it creates the ConsoleScheduler, so a schedule() armed
// from here is dropped on the floor. Instead $netlab::tick publishes the
// expression and FearPlugin.cpp's netLabHeartbeat evaluates it at 1 Hz. That
// also means no mod's console.cs can displace the lab timer.
//
// Time is counted in TICKS, not getSimTime(): the sim clock rewinds on a mission
// load, and a run that quietly extends itself past its slot is worse than one
// that stops early.
//------------------------------------------------------------------------------
function NetLab::tick()
{
   $netlab::t++;

   // ---- first tick: the things that must be set AFTER console.cs, not during --
   //
   // This file is exec'd from console.cs's +exec loop, which is ~90 lines BEFORE
   // console.cs acts on +connect. Anything it sets that console.cs also sets is
   // overwritten a moment later, silently. Two of those matter here, and both are
   // the difference between a rejoin working and the process simply vanishing:
   //
   //   $quitOnDisconnect -- console.cs:382 sets it TRUE on the +connect path, and
   //                        EndGame() (GUI.CS:158) quits the instant a drop
   //                        reaches the main menu. Our own ending still runs:
   //                        NetLab::finish calls quit() explicitly.
   //   $Server::Address  -- console.cs:386 publishes it from +connect.
   if($netlab::t == 1)
   {
      if($netlab::retry > 0)
         $quitOnDisconnect = false;
      if($netlab::addr == "")
         $netlab::addr = $Server::Address;
      echo("[NETLAB] WAN tag=", $netlab::tag, " addr=", $netlab::addr,
           " retry=", $netlab::retry, " dropAfter=", $netlab::dropAfter,
           "s vcTimeoutMs=", $pref::vcTimeoutMs);
   }

   // Connected? $Net::ping is republished every second by the same engine tick
   // and is only meaningful with a live packet stream, so a rising $Net::rateHz
   // is the cheapest honest "we are in" test available to script.
   %connected = ($Net::rateHz > 0);

   if(%connected)
   {
      if(!$netlab::everConnected)
      {
         $netlab::everConnected = 1;
         $netlab::connectedAt = $netlab::t;
         echo("[NETLAB] CONNECTED tag=", $netlab::tag, " after ", $netlab::t, "s");
      }
      else if($netlab::gapAt > 0)
      {
         // Came back. On a LAN this line never appears; across the internet it is
         // the headline of the whole run -- a link that keeps coming back is a
         // different fault from one that never dropped and a different one again
         // from one that died. Count it, and re-settle: the acks that arrive
         // either side of a hole are stale for the same reason a mission load's
         // are, and left alone they own rttmax for the rest of the match.
         %gap = $netlab::t - $netlab::gapAt;
         $netlab::gapAt = 0;
         $netlab::gaps++;
         $netlab::downtime = $netlab::downtime + %gap;
         $netlab::logging = 0;
         $pref::netLabLog = 0;
         $netlab::connectedAt = $netlab::t;
         $errorString = "";
         // ★No units inside a key=value★ -- the report parses these with a plain
         // float(), so `downtime=12s` reads as nothing at all. That is not
         // hypothetical: the DONE line below has carried `ran=900s` since the rig
         // was written and the report's `ran` column has been blank ever since.
         echo("[NETLAB] RESUMED tag=", $netlab::tag, " at ", $netlab::t, "s after ",
              %gap, "s down -- gaps=", $netlab::gaps, " downtime=", $netlab::downtime,
              " rejoins=", $netlab::rejoins);
      }

      // SETTLE. Telemetry does not start the instant we connect, and this is not
      // politeness -- it is the difference between a readable report and a useless
      // one. Measured on the first end-to-end run: a joining client reports
      // rttmax=4650 and jit=300 because it was FROZEN LOADING THE MISSION, so the
      // acks it finally sent were four seconds stale. Both numbers are true and
      // neither is a network event, and being session extremes they would then own
      // the report for the rest of the run -- for every machine in the room.
      //
      // So: wait for the load to finish, throw away the RTT history it produced
      // (netLabReset), and only then start writing lines. What lands in the log is
      // then about the network, which is what the log is for.
      if(!$netlab::logging && ($netlab::t - $netlab::connectedAt) >= $netlab::settle)
      {
         $netlab::logging = 1;
         netLabReset();
         $pref::netLabLog = 1;
         echo("[NETLAB] SETTLED tag=", $netlab::tag, " at ", $netlab::t,
              "s -- telemetry starts here");
      }
   }
   else if($netlab::everConnected)
   {
      // We were in, and now we are not. On a LAN that means dropped and the run
      // for this machine is over. ACROSS THE INTERNET IT USUALLY DOES NOT, and
      // treating it as terminal was the one thing in this rig that a wide-area
      // run would have broken outright:
      //
      //   * the VC gives up on an established link only after $pref::vcTimeoutMs
      //     of silence (vcprotocol.cpp, default 30s), and
      //   * the 1.50 client then makes THREE resume attempts of its own
      //     (FearCSDelegate.cpp net150ScheduleRejoin -- 1s apart, each failing a
      //     handshake in ~2.4s), about ten seconds' work.
      //
      // $Net::rateHz goes to zero at the START of that, so quitting on the first
      // zero tick -- which is what this did -- kills the client's own reconnect
      // before it has tried once, and reports `dropped` for a link that would
      // have come back. On a LAN the case never arises; on a real link it is the
      // normal case. So: wait out the engine, THEN take over.
      if($netlab::gapAt <= 0)
      {
         $netlab::gapAt = $netlab::t;
         $pref::netLabLog = 0;       // nothing true to say about a dead link
         echo("[NETLAB] GAP tag=", $netlab::tag, " at ", $netlab::t,
              "s -- no packets; waiting up to ", $netlab::dropAfter, "s");
      }

      // Two ways to know the engine is finished trying. $errorString is the
      // definite one -- onConnection(TimedOut) sets it once the resume budget is
      // spent -- and the timer is the backstop for every path that does not.
      %down = $netlab::t - $netlab::gapAt;
      if(%down >= $netlab::dropAfter || $errorString != "")
      {
         if($netlab::rejoins < $netlab::retry)
         {
            NetLab::rejoin();
            return;
         }
         if(!$netlab::dropped)
         {
            // $Console::LogMode 1 flushes per line, so this survives the process
            // going away. Without it a dropped client is just a log that stops,
            // indistinguishable from a machine someone switched off.
            $netlab::dropped = 1;
            $pref::netLabLog = 0;
            echo("[NETLAB] DROPPED tag=", $netlab::tag, " at ", $netlab::t,
                 "s after ", %down, "s down, ", $netlab::rejoins,
                 " rejoin(s) spent -- why=", $errorString);
            NetLab::finish("dropped");
            return;
         }
      }
   }
   else if($netlab::joinTimeout > 0 && ($netlab::t - $netlab::joinAt) > $netlab::joinTimeout)
   {
      // Never got in. Worth one more try before writing the machine off: a join
      // across the internet can lose its handshake to ordinary packet loss, or
      // simply arrive before the host finished loading its mission -- and a
      // machine two thousand kilometres away is not one anybody can go and nudge.
      if($netlab::rejoins < $netlab::retry)
      {
         NetLab::rejoin();
         return;
      }
      // Say WHY, in the log, on the machine. A client rejected for version,
      // password or a full server otherwise sits on a modal dialog nobody will
      // ever see (client.cs onConnection, "Rejected" branch) and the operator is
      // left with forty logs that all say the same nothing.
      echo("[NETLAB] GIVING UP tag=", $netlab::tag, " -- no connection after ",
           $netlab::t, "s, ", $netlab::rejoins, " rejoin(s) spent -- why=",
           $errorString);
      NetLab::finish("nojoin");
      return;
   }

   if($netlab::runTime > 0 && $netlab::t >= $netlab::runTime)
   {
      echo("[NETLAB] TIME UP tag=", $netlab::tag, " after ", $netlab::t, "s");
      NetLab::finish("complete");
      return;
   }
}

//------------------------------------------------------------------------------
// Go back in.
//
// The address is the one off OUR command line, not $Server::Address: a mod, the
// repack's own autoReconnect (base\scripts\repack.cs) or a redirect can have
// rewritten that by the time we get here, and a lab client must keep measuring
// the server the operator aimed it at or it is measuring nothing.
//
// JoinGame() rather than connect() directly, because that is the path every mod
// hooks and the one the stock client uses -- a rejoin should exercise what a
// player's rejoin exercises.
//------------------------------------------------------------------------------
function NetLab::rejoin()
{
   $netlab::rejoins++;
   $netlab::joinAt = $netlab::t;
   $netlab::gapAt  = $netlab::t;

   echo("[NETLAB] REJOIN tag=", $netlab::tag, " attempt ", $netlab::rejoins,
        " of ", $netlab::retry, " -> ", $netlab::addr, " at ", $netlab::t,
        "s -- last=", $errorString);

   $errorString = "";
   $Server::Address = $netlab::addr;
   JoinGame();
}

function NetLab::finish(%why)
{
   // One last summary line, so a merged report can state the outcome per machine
   // without inferring it from where the log stops. gaps/downtime/rejoins are
   // zero for every LAN run ever recorded and are the whole point of a wide-area
   // one -- a link that came back four times is not the same result as one that
   // never faltered, and the per-second lines alone cannot say which happened.
   echo("[NETLAB] DONE tag=", $netlab::tag, " reason=", %why,
        " ran=", $netlab::t, " connected=", $netlab::everConnected,
        " joinedAt=", $netlab::connectedAt, " settled=", $netlab::logging,
        " gaps=", $netlab::gaps, " downtime=", $netlab::downtime,
        " rejoins=", $netlab::rejoins);
   $netlab::tick = "";      // stop the heartbeat before the window goes away
   quit();
}

//------------------------------------------------------------------------------
// Boot.
//------------------------------------------------------------------------------
$netlab::tag = NetLab::arg("+netlab");
if($netlab::tag == "")
{
   // Not a lab launch. Leave absolutely nothing behind.
   echo("[NETLAB] netlab.cs exec'd without +netlab -- inactive.");
}
else
{
   $netlab::t = 0;
   $netlab::everConnected = 0;
   $netlab::connectedAt = -1;
   $netlab::runTime    = NetLab::argOr("+labtime", 0);
   $netlab::joinTimeout = NetLab::argOr("+labjoin", 120);
   $netlab::settle     = NetLab::argOr("+labsettle", 15);
   $netlab::logging    = 0;
   $netlab::dropped    = 0;

   // ---- wide-area operation ------------------------------------------------
   // Defaults chosen so that a LAN run behaves as it always did (neither fires)
   // and an internet run survives the things only an internet run does.
   //
   // dropAfter 30: measured against the engine, not guessed. An established link
   // is given up after $pref::vcTimeoutMs (vcprotocol.cpp, default 30000) and the
   // 1.50 resume then spends three attempts over about ten seconds. $Net::rateHz
   // reads zero from the start of that, so this timer runs alongside the engine's
   // own recovery and must outlast it -- shorter and we would cut off the very
   // mechanism a wide-area test exists to exercise.
   $netlab::dropAfter  = NetLab::argOr("+labdrop", 30);
   $netlab::retry      = NetLab::argOr("+labretry", 3);
   $netlab::gapAt      = 0;
   $netlab::gaps       = 0;
   $netlab::downtime   = 0;
   $netlab::rejoins    = 0;
   $netlab::joinAt     = 0;

   // The server this client was aimed at, kept for rejoins -- see NetLab::rejoin.
   $netlab::addr = NetLab::arg("+connect");
   if($netlab::addr == "")
      $netlab::addr = $Server::Address;

   // An absolute silence budget, exposed because it is the knob a wide-area run
   // actually wants to move: 30s is generous on fibre and can be mean on a link
   // that stalls. 0 leaves the engine default alone.
   %v = NetLab::argOr("+labvctimeout", 0);
   if(%v > 0) { $pref::vcTimeoutMs = %v; }

   $errorString = "";

   // ---- identity -----------------------------------------------------------
   // $PCFG::Name is what FearCSDelegate writes into the connect request, so it
   // has to be right BEFORE console.cs reaches its JoinGame() -- which is why
   // this file is exec'd by +exec and not from a post-connect hook.
   %name = NetLab::arg("+name");
   if(%name != "")
      $PCFG::Name = %name;
   if($PCFG::Name == "")
      $PCFG::Name = $netlab::tag;
   if($PCFG::Gender == "")   { $PCFG::Gender = "Male"; }
   if($PCFG::Voice == "")    { $PCFG::Voice = "Standard"; }
   if($PCFG::SkinBase == "") { $PCFG::SkinBase = "base"; }

   // ---- its own log file ---------------------------------------------------
   // Mode 1 = open, append, close per line. Slower than mode 2 and that is the
   // point: a lab machine that is killed, crashes or is power-cycled still has
   // every line it wrote up to that instant. At one line a second the cost is
   // not measurable.
   conLogFile("netlab-" @ $netlab::tag @ ".log");
   $Console::LogMode = 1;
   // NOT enabled here. NetLab::tick turns it on once the connection has settled --
   // see the SETTLE block for why that matters. The ordinary console output still
   // records the join, so nothing about the run is invisible in the meantime.
   $pref::netLabLog = 0;

   // ---- unattended operation ----------------------------------------------
   // backgroundFrameMs is the one that actually decides whether this test means
   // anything: an unfocused window otherwise sleeps 100 ms per frame (gwMain.cpp),
   // so forty background clients would all be sending moves at ~9 Hz and the
   // "jitter" measured would be the screensaver, not the network.
   $pref::backgroundFrameMs = 0;
   $pref::skipIntro = 1;
   $pref::quickstart = 0;
   $pref::cdMusic = 0;
   $pref::sfx2DVolume = 0;
   $pref::sfx3DVolume = 0;
   $pref::playVoices = 0;
   $pref::netPacketDiag = 1;

   // ---- netcode knobs under test ------------------------------------------
   // Each is a no-op unless the operator asked for it, so half a room can run
   // one setting and half another IN THE SAME MATCH -- which is the only way to
   // A/B a netcode change against a shared, identical network.
   %v = NetLab::arg("+labrate");   if(%v != "") { $pref::PacketRate = %v; }
   %v = NetLab::arg("+labsize");   if(%v != "") { $pref::PacketSize = %v; }
   %v = NetLab::arg("+labsmooth"); if(%v != "") { $pref::netSmoothMode = %v; }
   %v = NetLab::arg("+labsnap");   if(%v != "") { $pref::netSnapInterp = %v; }
   %v = NetLab::arg("+labshot");   if(%v != "") { $pref::predictProjectiles = %v; }

   if(NetLab::argOr("+labrender", 1) == 0)
   {
      // For a machine that cannot hold frame rate. Read the warning in the header
      // before using this on a whole room.
      $pref::TerrainVisibleDistance = 300;
      $pref::ObjectVisibleDistance = 150;
      $pref::Shadows = 0;
      $pref::particleDensity = 0;
   }

   echo("[NETLAB] ARMED tag=", $netlab::tag, " name=", $PCFG::Name,
        " runTime=", $netlab::runTime, " joinTimeout=", $netlab::joinTimeout,
        " settle=", $netlab::settle,
        " rate=", $pref::PacketRate, " size=", $pref::PacketSize,
        " smooth=", $pref::netSmoothMode, " snap=", $pref::netSnapInterp,
        " shot=", $pref::predictProjectiles);

   // Arm the engine heartbeat LAST: everything it reads is now set.
   $netlab::tick = "NetLab::tick();";
}
