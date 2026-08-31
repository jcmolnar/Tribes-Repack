//====================================================================================
// caster.cs -- shoutcaster observer tools, stage 1 (2026-08-28).
//
// Community request (sol black, Discord 08-26): follow-the-flag-carrier orbit cam +
// flag cameras for shoutcasters. Scope + abuse analysis:
// SHOUTCAST-TOOLS-NETCODE-SCOPE-2026-08-28.md at the repo root.
//
// THE GATE IS THE FEATURE: every remote handler checks a server-granted role
// (%cl.isAdmin from the stock admin password, or %cl.isCaster granted BY an admin
// through remoteCasterGrant) AND that the client is actually observing (team -1).
// Nothing is client-trusted: team index is validated against $teamFlag[], distances
// are clamped into [OrbitMin,OrbitMax] (harness-observed: garbage strings like "abc"
// lex-compare high and clamp to the max, 30 -- still in range), targets resolve
// server-side. No
// ghost-scope change of any kind -- the orbit target rides the stock force-scope in
// ObserverCamera::buildScopeAndCameraInfo (observerCamera.cpp:271).
//
// Camera plumbing is the stock engine observer API (ScriptPlugin.cpp:201-206):
//   Observer::setOrbitObject(%cl, %obj, %cur, %min, %max)   360 orbit of an object
//   Observer::setOrbitPoint(%cl, "x y z", %cur, %min, %max) 360 orbit of a point
//
// FOLLOW model: the flag ITEM is hidden while carried (Flag::onCollision/onDrop in
// objectives.cs), so "follow the carrier" orbits %flag.carrier (the player object)
// while set, and the flag object itself when at base or dropped. A 0.5s poll
// re-targets on pickup/drop/cap. Carrier deletion is crash-safe natively:
// ObserverCamera::onDeleteNotify nulls the target and falls back to point orbit
// (observerCamera.cpp:91-103). base\scripts\observer.cs owns
// Observer::orbitObjectDeleted -- deliberately NOT redefined here (console function
// definition is last-wins); the poll re-acquires within one tick anyway.
//
// DIALECT NOTES (the traps this file is written around):
//   * %cl.casterFollow stores TEAM+1 (0/unset = off) so every test stays numeric --
//     comparing "" against team 0 with == is ambiguous in this parser.
//   * No continue/break; nested ifs instead.
//   * Schedules are DISCARDED on mission change (ConsoleScheduler is recreated by
//     Server::loadMission), so Server::finishMissionLoad schedules
//     Caster::missionStart() exactly like the SpoonBot/Storm hooks beside it.
//====================================================================================

$Caster::OrbitMin = 3;
$Caster::OrbitMax = 30;
$Caster::DefaultDist = 12;
$Caster::PollSecs = 0.5;

echo("[CASTER] shoutcaster tools loaded (stage 1).");

function Caster::privileged(%cl)
{
   if(%cl.isAdmin || %cl.isCaster)
      return true;
   Client::sendMessage(%cl, 0, "Caster tools are admin-granted. An admin can enable you with CasterGrant(<your id>, 1).");
   return false;
}

function Caster::observing(%cl)
{
   if(Client::getTeam(%cl) == -1)
      return true;
   Client::sendMessage(%cl, 0, "Caster cameras only work while observing.");
   return false;
}

function Caster::validTeam(%cl, %team)
{
   if(%team >= 0 && %team <= 7 && $teamFlag[%team] > 0)
      return true;
   Client::sendMessage(%cl, 0, "No flag for team " @ %team @ " on this mission.");
   return false;
}

function Caster::clampDist(%d)
{
   if(!(%d >= $Caster::OrbitMin))
      return $Caster::DefaultDist;
   if(%d > $Caster::OrbitMax)
      return $Caster::OrbitMax;
   return %d;
}

function remoteCasterGrant(%cl, %target, %on)
{
   if(!%cl.isAdmin)
   {
      Client::sendMessage(%cl, 0, "CasterGrant is admin-only.");
      return;
   }
   %found = false;
   for(%c = Client::getFirst(); %c != -1; %c = Client::getNext(%c))
   {
      if(%c == %target)
         %found = true;
   }
   if(!%found)
   {
      Client::sendMessage(%cl, 0, "CasterGrant: no client with id " @ %target @ ".");
      return;
   }
   if(%on)
   {
      %target.isCaster = true;
      Client::sendMessage(%cl, 0, "Caster tools granted to " @ Client::getName(%target) @ ".");
      Client::sendMessage(%target, 0, "Caster tools granted by " @ Client::getName(%cl) @ ". While observing: NUMPAD 1/2 follow a flag, 4/5 flag-stand cam, +/- distance, 0 detach, * help.");
   }
   else
   {
      %target.isCaster = "";
      %target.casterFollow = 0;
      %target.casterTarget = 0;
      Client::sendMessage(%cl, 0, "Caster tools revoked from " @ Client::getName(%target) @ ".");
      Client::sendMessage(%target, 0, "Caster tools revoked.");
   }
}

function Caster::retarget(%cl)
{
   %team = %cl.casterFollow - 1;
   %flag = $teamFlag[%team];
   if(%flag > 0)
   {
      %target = %flag;
      if(%flag.carrier > 0)
         %target = %flag.carrier;
      if(%target != %cl.casterTarget)
      {
         %cl.casterTarget = %target;
         Observer::setOrbitObject(%cl, %target, Caster::clampDist(%cl.casterDist), $Caster::OrbitMin, $Caster::OrbitMax);
      }
   }
}

function remoteCasterFollow(%cl, %team)
{
   if(!Caster::privileged(%cl))
      return;
   if(!Caster::observing(%cl))
      return;
   if(!Caster::validTeam(%cl, %team))
      return;
   %cl.casterFollow = %team + 1;
   %cl.casterTarget = 0;
   Caster::retarget(%cl);
   Caster::startTick();
   Client::sendMessage(%cl, 0, "Following the " @ getTeamName(%team) @ " flag (carrier when carried). CasterStop() detaches.");
}

function remoteCasterFlagCam(%cl, %team)
{
   if(!Caster::privileged(%cl))
      return;
   if(!Caster::observing(%cl))
      return;
   if(!Caster::validTeam(%cl, %team))
      return;
   %cl.casterFollow = 0;
   %cl.casterTarget = 0;
   %flag = $teamFlag[%team];
   Observer::setOrbitPoint(%cl, %flag.originalPosition, Caster::clampDist(%cl.casterDist), $Caster::OrbitMin, $Caster::OrbitMax);
   Client::sendMessage(%cl, 0, getTeamName(%team) @ " flag stand camera.");
}

function remoteCasterDist(%cl, %d)
{
   if(!Caster::privileged(%cl))
      return;
   %cl.casterDist = Caster::clampDist(%d);
   %cl.casterTarget = 0;
   if(%cl.casterFollow > 0)
      Caster::retarget(%cl);
   Client::sendMessage(%cl, 0, "Caster orbit distance " @ %cl.casterDist @ "m (range " @ $Caster::OrbitMin @ "-" @ $Caster::OrbitMax @ ").");
}

function remoteCasterStop(%cl)
{
   %cl.casterFollow = 0;
   %cl.casterTarget = 0;
   Client::sendMessage(%cl, 0, "Caster follow off.");
}

function Caster::startTick()
{
   if($Caster::tickLive == 1)
      return;
   $Caster::tickLive = 1;
   schedule("Caster::tick();", $Caster::PollSecs);
}

function Caster::tick()
{
   %any = 0;
   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
   {
      if(%cl.casterFollow > 0)
      {
         if(!(%cl.isAdmin || %cl.isCaster) || Client::getTeam(%cl) != -1)
         {
            %cl.casterFollow = 0;
            %cl.casterTarget = 0;
         }
         else
         {
            %any = 1;
            Caster::retarget(%cl);
         }
      }
   }
   if(%any)
      schedule("Caster::tick();", $Caster::PollSecs);
   else
      $Caster::tickLive = 0;
}

function Caster::missionStart()
{
   %any = 0;
   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
   {
      if(%cl.casterFollow > 0)
      {
         %cl.casterTarget = 0;
         %any = 1;
      }
   }
   $Caster::tickLive = 0;
   if(%any)
      Caster::startTick();
   schedule("Caster::momentTick();", 2);
}

//====================================================================================
// STAGE 2 (2026-08-28): moment classification + auto-record support.
//
// Server side of sol black's "auto capture MAs, CKs, Caps, Returns into a folder":
// the SERVER classifies moments and remoteEvals them to the involved client; the
// CLIENT (nativeDefaults.cs, $pref::casterAutoRecord) auto-records every match and
// writes a sidecar .events.cs index next to the .rec. A client that never opted in
// just ignores the eval.
//
// Kills arrive via ONE added line in objectives.cs Game::clientKilled (the stock
// empty-stub dispatch point; spoonbot's hooks_base.cs replaces Flag::* but not it).
// Flag moments are POLLED from flag STATE (.carrier / .atHome / .enemyCaps --
// fields both stock objectives.cs and spoonbot hooks_base.cs maintain), so they
// work under either implementation without touching Flag::onCollision:
//   GRAB   = carrier transition to a player; attribution = Player::getClient.
//   CAP    = enemyCaps delta on the captured flag; attribution = last carrier.
//   RETURN = atHome restored with no cap; attribution = nearest same-team client
//            within 6m of where the flag WAS (casterLastPos, tracked while afield --
//            onCollision teleports the flag home BEFORE the poll can look). Timeout
//            returns have nobody nearby and stay unattributed on purpose.
// MA = no terrain/interior/static within $Caster::maHeight below the victim
// (GetLosInfo point ray, script mask 3); CK = victim carried a flag. Both server
// prefs, tune later. Bots (empty transport address) never receive evals.
//====================================================================================

$Caster::maHeight = 8;
$Caster::momentPoll = 0.5;

function Caster::clientKilled(%victimCl, %killerCl)
{
   if(%killerCl == 0 || %killerCl == "" || %killerCl == -1 || %killerCl == %victimCl)
      return;
   if(Client::getTeam(%killerCl) == Client::getTeam(%victimCl))
      return;
   %vPl = Client::getOwnedObject(%victimCl);
   if(%vPl == -1)
      return;
   %ck = 0;
   if(%vPl.carryFlag != "")
      %ck = 1;
   %ma = 0;
   %pos = GameBase::getPosition(%vPl);
   %below = getWord(%pos, 0) @ " " @ getWord(%pos, 1) @ " " @ (getWord(%pos, 2) - $Caster::maHeight);
   if(GetLosInfo(%pos, %below, 3) == "False")
      %ma = 1;
   %victim = Client::getName(%victimCl);
   if(%ma == 1 && %ck == 1)
      Caster::moment(%killerCl, "MA-CK", "midair killed carrier " @ %victim);
   else if(%ck == 1)
      Caster::moment(%killerCl, "CK", "killed carrier " @ %victim);
   else if(%ma == 1)
      Caster::moment(%killerCl, "MA", "midair killed " @ %victim);
}

function Caster::moment(%cl, %type, %detail)
{
   if(%cl <= 0)
      return;
   if(Client::getTransportAddress(%cl) == "")
      return;
   remoteEval(%cl, "CasterMoment", %type, %detail, $missionName);
   Client::sendMessage(%cl, 0, "Moment: " @ %type @ " -- " @ %detail);
}

function Caster::nearestTeamClient(%f, %team)
{
   %pos = %f.casterLastPos;
   if(%pos == "")
      return -1;
   %best = -1;
   %bestD = 6;
   for(%c = Client::getFirst(); %c != -1; %c = Client::getNext(%c))
   {
      if(Client::getTeam(%c) == %team)
      {
         %pl = Client::getOwnedObject(%c);
         if(%pl != -1)
         {
            %d = Vector::getDistance(GameBase::getPosition(%pl), %pos);
            if(%d < %bestD)
            {
               %bestD = %d;
               %best = %c;
            }
         }
      }
   }
   return %best;
}

//====================================================================================
// NUMPAD CONTROLS + HELP (2026-08-28) -- "some form of control" integration.
//
// Every repack client already relays numpad presses to the server: extra-controls.cs
// binds them (bindCommandDefault, zero keybind clobber) -> sendControl (repack.cs:63)
// -> remoteEval(2048, RawKey) -> server remoteRawKey. The stock base placeholder in
// server.cs now dispatches here FIRST for granted casters in observer mode; everyone
// else keeps the stock courtesy message, and mods that own the relay (RPG remote.cs)
// define their own remoteRawKey later and win, unchanged.
//
// Layout (make only): 1/2 follow team 0/1, 4/5 flag-stand cam 0/1, +/- orbit
// distance in 5m steps, 0 detach, * help. Teams 2-7 stay console-only.
//====================================================================================

// Is %key one of the eight caster control keys? Used by remoteRawKey to answer a
// privileged-but-not-observing press with the observer hint instead of the stock
// "does not support extra keybinds" line (2026-08-29). Keep in sync with
// Caster::rawKey below.
function Caster::isCasterKey(%key)
{
   if(String::ICompare(%key, "numpad1") == 0)
      return true;
   if(String::ICompare(%key, "numpad2") == 0)
      return true;
   if(String::ICompare(%key, "numpad4") == 0)
      return true;
   if(String::ICompare(%key, "numpad5") == 0)
      return true;
   if(String::ICompare(%key, "numpad+") == 0)
      return true;
   if(String::ICompare(%key, "numpad-") == 0)
      return true;
   if(String::ICompare(%key, "numpad0") == 0)
      return true;
   if(String::ICompare(%key, "numpad*") == 0)
      return true;
   return false;
}

function Caster::rawKey(%cl, %key)
{
   if(String::ICompare(%key, "numpad1") == 0)
   {
      remoteCasterFollow(%cl, 0);
      return true;
   }
   if(String::ICompare(%key, "numpad2") == 0)
   {
      remoteCasterFollow(%cl, 1);
      return true;
   }
   if(String::ICompare(%key, "numpad4") == 0)
   {
      remoteCasterFlagCam(%cl, 0);
      return true;
   }
   if(String::ICompare(%key, "numpad5") == 0)
   {
      remoteCasterFlagCam(%cl, 1);
      return true;
   }
   if(String::ICompare(%key, "numpad+") == 0)
   {
      remoteCasterDist(%cl, Caster::clampDist(%cl.casterDist) + 5);
      return true;
   }
   if(String::ICompare(%key, "numpad-") == 0)
   {
      remoteCasterDist(%cl, Caster::clampDist(%cl.casterDist) - 5);
      return true;
   }
   if(String::ICompare(%key, "numpad0") == 0)
   {
      remoteCasterStop(%cl);
      return true;
   }
   if(String::ICompare(%key, "numpad*") == 0)
   {
      remoteCasterHelp(%cl);
      return true;
   }
   return false;
}

function remoteCasterHelp(%cl)
{
   Client::sendMessage(%cl, 0, "-- Caster tools (admin-granted, observer-only) --");
   Client::sendMessage(%cl, 0, "NUMPAD while observing: 1/2 = follow team 0/1 flag (tracks the carrier), 4/5 = flag-stand cam, +/- = orbit distance, 0 = detach, * = this help.");
   Client::sendMessage(%cl, 0, "Console: CasterFollow(team) CasterFlagCam(team) CasterDist(m) CasterStop(); teams 2-7 via console only.");
   Client::sendMessage(%cl, 0, "Admins: CasterGrant(clientId, 1) grants, (clientId, 0) revokes. Client ids are on the admin player list.");
   Client::sendMessage(%cl, 0, "Auto-record + highlight index: $pref::casterAutoRecord = 1; in your console. See README - Modern Client.txt, SHOUTCASTING.");
}

function Caster::momentTick()
{
   for(%c = Client::getFirst(); %c != -1; %c = Client::getNext(%c))
   {
      if(%c.casterHello == "" && Client::getTransportAddress(%c) != "")
      {
         %c.casterHello = 1;
         remoteEval(%c, "CasterHello");
      }
   }
   for(%t = 0; %t < 8; %t = %t + 1)
   {
      %f = $teamFlag[%t];
      if(%f > 0)
      {
         %car = %f.carrier;
         %prevCar = %f.casterPrevCarrier;
         if(%prevCar == "")
            %prevCar = -1;
         %caps = %f.enemyCaps;
         if(%caps == "")
            %caps = 0;
         %prevCaps = %f.casterPrevCaps;
         if(%prevCaps == "")
            %prevCaps = %caps;
         %home = %f.atHome;
         %prevHome = %f.casterPrevHome;
         if(%prevHome == "")
            %prevHome = %home;

         if(%home != 1)
            %f.casterLastPos = GameBase::getPosition(%f);

         if(%car != %prevCar && %car > 0)
         {
            %cl = Player::getClient(%car);
            %f.casterLastCarrierCl = %cl;
            if(%cl > 0)
               Caster::moment(%cl, "GRAB", "grabbed the " @ getTeamName(%t) @ " flag");
         }
         if(%caps > %prevCaps)
         {
            %cl = %f.casterLastCarrierCl;
            if(%cl > 0)
               Caster::moment(%cl, "CAP", "capped the " @ getTeamName(%t) @ " flag");
         }
         else if(%home == 1 && %prevHome != 1 && %car <= 0)
         {
            %cl = Caster::nearestTeamClient(%f, %t);
            if(%cl > 0)
               Caster::moment(%cl, "RETURN", "returned the " @ getTeamName(%t) @ " flag");
         }
         %f.casterPrevCarrier = %car;
         %f.casterPrevCaps = %caps;
         %f.casterPrevHome = %home;
      }
   }
   schedule("Caster::momentTick();", $Caster::momentPoll);
}
