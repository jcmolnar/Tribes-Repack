//==============================================================================
// netlabhost.cs -- server half of the netcode lab rig.
//
// LAUNCH
// ------
//   TribesNative.exe -dedicated Broadside -mod base +exec netlabhost.cs +netlabsrv 1
//   (or point TribesHost.exe's "additional arguments" at the same +exec)
//
// WHAT IT DOES
// ------------
//  1. Raises the player cap, because the stock default (32) silently rejects the
//     back half of a forty-machine room.
//  2. SPAWNS THE JOINERS. A client that connects lands in observer mode waiting
//     for someone to pick a team and pull a trigger. With nobody at the keyboards
//     that never happens, and a room full of observers is not a netcode test --
//     observers neither send a move stream nor own a ghosted player object, so
//     the two things under test are exactly the two things absent.
//  3. Keeps them alive: anything that dies is put back in the world.
//  4. Writes [NETLABSRV], one line per client per second, from the host's own
//     side of every connection -- see main.cpp. Paired with the clients' own
//     [NETLAB] lines, that is what separates "the network dropped it" from "the
//     client never sent it".
//
// OPTIONS (read off the same command line; console.cs ignores what it does not know)
//   +netlabsrv <1>     turn the rig on. Without it this file does nothing.
//   +labmax <N>        $Server::MaxPlayers (default 64).
//   +labspawn <0|1>    auto team-assign and spawn joiners (default 1).
//   +labrespawn <SEC>  put a dead client back this many seconds later
//                      (default 5; 0 = leave them dead).
//   +labsettle <SEC>   how long after a client is in the world before the host
//                      forgets the RTT history its mission load produced
//                      (default 15; match the clients' own +labsettle).
//   +labrate <MS>      $pref::PacketRate on the host side. NOTE the negotiation
//                      takes the CONSERVATIVE side of the two ends, so a slow
//                      host caps every client no matter what they asked for.
//   +labbots <0|1>     run BotBrain (default 1). Bots are what put MOVEMENT in the
//                      world, and without movement almost nothing is ghosted
//                      downstream -- see the block below. Roster size = the number
//                      of `bot` lines in config\botbrain.cfg.
//   +labpublic <0|1>   register with the public masters (default 0 -- a test rig
//                      should not turn up in everyone's server browser).
//   +labport <N>       listen port (default 28001). THE port to forward when the
//                      clients are not on this network.
//
// A VALUE MUST NOT START WITH '-' OR '+'.
//
// HOSTING FOR CLIENTS THAT ARE NOT ON THIS NETWORK
// ------------------------------------------------
// Same rig, one router in the way. What the far end needs from this one:
//
//   1. the ADDRESS -- a public one. tools\netlab\netlab-netcheck.ps1 prints it,
//      along with the exact command line to hand over.
//   2. the PORT, forwarded to this machine as UDP. +labport pins it so the
//      forward and the listener cannot drift apart.
//   3. a PASSWORD, via the stock +password on both ends. A public address plus
//      auto-spawn plus open slots is an invitation.
//   4. the same BUILD and the same MISSION. Version is checked in the connect
//      handshake and a rejected client says so only in its own log, two thousand
//      kilometres away -- netlab.cs now echoes the reason for exactly this case.
//
// What changes in the numbers: ping stops being noise and becomes the signal,
// and links now drop and come back. Both ends already measure that; the client's
// GAP/RESUMED/REJOIN lines and the report's reconnect count are what read it.
//==============================================================================

function NetLabHost::arg(%opt)
{
   for(%i = 1; $cargv[%i] != ""; %i++)
   {
      if($cargv[%i] == %opt)
         return $cargv[%i + 1];
   }
   return "";
}

function NetLabHost::argOr(%opt, %default)
{
   %v = NetLabHost::arg(%opt);
   if(%v == "")
      return %default;
   return %v;
}

//------------------------------------------------------------------------------
// Once a second, from the engine (main.cpp serverProcess). See netlab.cs for why
// this is not a schedule().
//
// Deliberately a POLL over the client list rather than a redefinition of
// Game::initialMissionDrop. Every mod that matters redefines that function, and
// replacing it here would silently drop whatever the mod does in it -- a rig that
// changes the thing it is measuring. Polling reads state and acts on it, so it
// composes with base, Kronos, Annihilation or anything else unchanged.
//------------------------------------------------------------------------------
function NetLabHost::tick()
{
   $netlabsrv::t++;

   if(!$netlabsrv::spawn)
      return;

   %n = getNumClients();
   %spawned = 0;
   %alive = 0;
   for(%i = 0; %i < %n; %i++)
   {
      %cl = getClientByIndex(%i);
      if(%cl == -1 || %cl == "")
         continue;

      // Already driving a player object: nothing to do. Clear the waiting counter
      // so the same client gets the full grace period again next time it dies.
      if(Client::getOwnedObject(%cl) != -1)
      {
         $netlabsrv::waiting[%cl] = 0;
         %alive++;

         // SETTLE, host side. Our VC for this client watched it go silent through
         // the whole mission load and recorded a multi-second round trip; left
         // alone that owns rttmax for the rest of the match. Throw it away once --
         // per client, so the forty already playing keep their history.
         if(!$netlabsrv::reset[%cl] && $netlabsrv::t >= $netlabsrv::resetAt[%cl])
         {
            $netlabsrv::reset[%cl] = 1;
            // Echo the PRODUCER, not just the effect: without this line a reset that
            // silently never fired is indistinguishable from one that fired and was
            // immediately followed by a genuine stall.
            echo("[NETLABSRV] reset id=", %cl, " (", netLabReset(%cl), " conn) t=",
                 $netlabsrv::t);
         }
         continue;
      }

      // No player object. Either still joining, or dead. A client that has not
      // been given a team yet is still coming in, so assign and let the next tick
      // find it -- one state change per tick keeps this from racing the game's own
      // join handling.
      if(Client::getTeam(%cl) == -1)
      {
         Game::assignClientTeam(%cl);
         $netlabsrv::waiting[%cl] = 0;
         // Client ids are REUSED as people come and go, so clear the per-id
         // history here -- this is the one state every new arrival passes through.
         $netlabsrv::everSpawned[%cl] = 0;
         $netlabsrv::reset[%cl] = 0;
         $netlabsrv::resetAt[%cl] = $netlabsrv::t + $netlabsrv::settle;
         continue;
      }

      // A client that has held a team but never held a player object is a fresh
      // joiner: put it in the world immediately. After that first spawn, an empty
      // control object means it DIED, and +labrespawn governs.
      if($netlabsrv::everSpawned[%cl])
      {
         if($netlabsrv::respawn <= 0)
            continue;                    // operator asked for dead to stay dead
         // Counted in ticks, which are seconds. The grace period is what keeps this
         // from fighting the game's own death sequence -- respawning someone the
         // instant they die skips the corpse, the kill message and the scoring.
         $netlabsrv::waiting[%cl] = $netlabsrv::waiting[%cl] + 1;
         if($netlabsrv::waiting[%cl] < $netlabsrv::respawn)
            continue;
      }
      $netlabsrv::waiting[%cl] = 0;
      $netlabsrv::everSpawned[%cl] = 1;

      // respawn=1 skips the initial drop cinematic, which no unattended client
      // needs and which costs a few seconds of not-yet-in-the-world per join.
      Game::playerSpawn(%cl, 1);
      %spawned++;
   }

   if(%spawned > 0)
      echo("[NETLABSRV] spawned=", %spawned, " alive=", %alive, " clients=", %n,
           " t=", $netlabsrv::t);
}

//------------------------------------------------------------------------------
// Boot.
//------------------------------------------------------------------------------
if(NetLabHost::arg("+netlabsrv") == "")
{
   echo("[NETLABSRV] netlabhost.cs exec'd without +netlabsrv -- inactive.");
}
else
{
   $netlabsrv::t = 0;
   $netlabsrv::spawn   = NetLabHost::argOr("+labspawn", 1);
   $netlabsrv::respawn = NetLabHost::argOr("+labrespawn", 5);
   // Matches the clients' own +labsettle: how long after a client is in the world
   // before we forget the RTT history its mission load produced.
   $netlabsrv::settle  = NetLabHost::argOr("+labsettle", 15);

   $Server::MaxPlayers = NetLabHost::argOr("+labmax", 64);
   $Server::AutoAssignTeams = true;

   // THE LISTEN PORT, and the reason this is settable at all: a wide-area run is
   // reached through somebody's router, and the port you forward is the port the
   // server had better be on.
   //
   // ★This works only because of WHERE we are in the boot.★ console.cs execs
   // serverDefaults.cs (which hard-sets $Server::Port = "28001") at :280, its
   // +exec files at :294, and only then calls createServer at :302 -- which is
   // what hands the port to the IP transport (base\scripts\server.cs:183). So a
   // port set here survives; the same line in an autoexec would not.
   //
   // ★The default is 28001, not 28000.★ Every example in this rig used to say
   // 28000, and nothing here has ever listened there: 28000 is the MASTER-server
   // port by convention ($Server::MasterAddress* in serverDefaults.cs), while
   // serverDefaults.cs and every shipped ServerPrefs.cs put the game on 28001.
   // A client aimed at the wrong port gets silence, and silence at the far end of
   // a long link is the single most expensive thing to debug.
   $Server::Port = NetLabHost::argOr("+labport", 28001);

   // BOTS, and this is not a detail -- it decides whether the run measures anything.
   //
   // Forty unattended clients stand perfectly still. Their upstream move stream is
   // realistic (an idle client still sends moves), but almost NOTHING is ghosted
   // downstream, because nothing in the world is moving. Downstream bandwidth per
   // client under a full server is the single most interesting number a lab run can
   // produce, and a room of statues does not produce it.
   //
   // A dozen bots running a CTF map fix that: every one of the forty connections
   // gets a continuous stream of position updates, which is what a real match looks
   // like from the wire's point of view.
   //
   // Must be set BEFORE the mission loads -- botBrain.cpp reads $Server::BotBrain
   // only on the not-yet-initialised path -- and this file is exec'd ahead of
   // createServer(), which is why it works here and would not work later.
   //
   // ★The ROSTER SIZE is the number of `bot` lines in config\botbrain.cfg★ (twelve
   // as shipped). For a heavier test, add lines there; there is no count knob.
   if(NetLabHost::argOr("+labbots", 1) == 0)
      $Server::BotBrain = "0";
   else
      $Server::BotBrain = "1";

   // A test rig does not advertise. Left alone, a dedicated server heartbeats to the
   // public Tribes masters and a forty-slot room full of machines named LAB01..LAB41
   // turns up in every player's browser as a real server. Opt back in with
   // +labpublic 1 if the run is deliberately public.
   //
   // Must be the explicit string "false": an EMPTY value reads as TRUE at heartbeat
   // time (netCSDelegate.cpp), so deleting the pref is not the same as clearing it.
   if(NetLabHost::argOr("+labpublic", 0) == 0)
      $Server::HostPublicGame = "false";

   %v = NetLabHost::arg("+labrate");
   if(%v != "") { $pref::PacketRate = %v; }

   // Its own log file: a host and a client launched from the same folder otherwise
   // interleave into console.log and neither can be read afterwards.
   conLogFile("netlab-host.log");
   $Console::LogMode = 1;
   $pref::netLabSrvLog = 1;
   $pref::srvProfLog = 1;          // [SRVPROF]/[SRVPROF2]: the aggregate the per-client
                                   // lines cannot show -- ghost list vs the 1024 cap,
                                   // client reps vs 127, frame ms, event queue depth.

   echo("[NETLABSRV] ARMED max=", $Server::MaxPlayers,
        " port=", $Server::Port,
        " spawn=", $netlabsrv::spawn, " respawn=", $netlabsrv::respawn,
        " settle=", $netlabsrv::settle, " bots=", $Server::BotBrain,
        " public=", $Server::HostPublicGame, " rate=", $pref::PacketRate);

   // Say whether a password is in force, and NEVER say what it is -- this log is
   // the file the operator mails around after a run.
   //
   // A wide-area host is on a public address with auto-spawn and sixty-odd open
   // slots. It is not advertised (HostPublicGame above), but "not advertised" is
   // not "not findable", and one stranger wandering in mid-run is a whole
   // measurement wasted. +password is the stock console.cs option and sets both
   // ends' variables, so the same flag works on the host and on every client.
   if($Server::Password == "")
      echo("[NETLABSRV] password=none -- anyone who learns the address can join. ",
           "Pass +password <word> on BOTH ends for a run across the internet.");
   else
      echo("[NETLABSRV] password=set");

   // The line the remote operator needs, printed by the machine that knows the
   // truth rather than transcribed by hand from a plan. The address is the one
   // thing this end cannot know -- see tools\netlab\netlab-netcheck.ps1.
   echo("[NETLABSRV] clients run:  netlab-client.bat <this-host>:", $Server::Port,
        " <seconds>");

   $netlab::srvTick = "NetLabHost::tick();";
}
