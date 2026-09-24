//==============================================================================
// OPS ANNOUNCER -- Opsaya's Quake-style announcer (config\Modules\announcer.acs.cs),
// rebuilt for this client. Options > 07 SCRIPTS "Announcer" ($pref::scriptAnnouncer).
//
// It calls out EVERYONE's play, not just yours: kill streaks, carrier-kill and cap
// milestones, first cap, lead changes, cap streaks, match start, the countdown and
// the result. Its events come from opsStats.cs (which it needs; both files load
// together), so the flag-pass calls (catch, interception, mid-air) only happen on
// servers that send tagged stat messages -- as in the original.
//
// Sounds: config\ModernHUD\Scripts\sounds\opsann_<name>.ogg, Opsaya's own set,
// prefixed so the Star Wars mod's same-named voices can never shadow them.
// Three names the original called had no file in his install (battle_prepare_04,
// coupdegras, nasty); they are left out of the random picks instead of playing
// silence.
//
// Fixed while porting (all three were dead in the original):
//   * "taken the lead / lost the lead" compared the capping team's NUMBER with a
//     cap count; it compares cap counts now
//   * the end-of-map "smackdown" called an undefined function
//   * the time callouts (5 min .. 1) never ran -- their hook was commented out and
//     the fallback timer counted from nothing. They read the match clock now.
//==============================================================================

// Tunables, the original's values and names.
// (none may start with "S": OpsAnn::reset() clears $OpsAnn::S* -- the live state)
$OpsAnn::BUFFER             = 2;     // SOUND_BUFFER: seconds between sounds from one event
$OpsAnn::KILL_STREAK_TIME   = 5;
$OpsAnn::TEAMKILL_TIME      = 10;
$OpsAnn::LONG_CATCH_TIME    = 3.5;
$OpsAnn::ASSIST_TIME        = 5;
$OpsAnn::MASSACRE_THRESHOLD = 4;
$OpsAnn::PASS_STREAK_THRESHOLD = 3;

function OpsAnn::on()
{
   return $pref::scriptAnnouncer == 1;
}

function OpsAnn::play(%sound, %delay)
{
   if(%delay <= 0)
      localSound("opsann_" @ %sound);
   else
      schedule("localSound(\"opsann_" @ %sound @ "\");", %delay);
}

// One of N, uniformly.
function OpsAnn::pick(%list, %delay)
{
   %n = 0;
   for(%i = 0; %i < 8; %i++)
   {
      %w = getWord(%list, %i);
      if(%w == "" || %w == "-1") break;
      %n++;
   }
   if(%n == 0) return;
   OpsAnn::play(getWord(%list, floor(getRandom() * %n)), %delay);
}

function OpsAnn::reset()
{
   deleteVariables("$OpsAnn::S*");       // per-player / per-team state
   $OpsAnn::SlastTeamToCap = -1;
   $OpsAnn::SfirstCap = false;
   $OpsAnn::ScountdownStarted = false;
}

//------------------------------------------------------------------------------
// Match lifecycle
//------------------------------------------------------------------------------
function OpsAnn::matchStarted()
{
   if(!OpsAnn::on()) return;
   OpsAnn::reset();
   OpsAnn::pick("battle_begin_01 battle_begin_02 battle_begin_03 battle_begin_04 battle_begin_05", 0);
   OpsAnn::clockStart();
}

function OpsAnn::countdown(%time)
{
   if(!OpsAnn::on()) return;
   $OpsAnn::ScountdownStarted = true;
   OpsAnn::pick("battle_prepare_01 battle_prepare_02 battle_prepare_03", 1);
   if(%time == 30)
      OpsAnn::play("count_battle_10", 20);
}

// Mission over (Presto's eventChangeMission): the result, from the caps counted
// this map, before opsStats clears them at the next "Match started.".
function OpsAnn::missionOver()
{
   if(!OpsAnn::on()) return;
   %a = $OpsStats::TeamCaps[0];  %b = $OpsStats::TeamCaps[1];
   if(%a == "") %a = 0;
   if(%b == "") %b = 0;
   %buf = $OpsAnn::BUFFER;
   %count = 1;
   if(%a == %b)
   {
      OpsAnn::pick("humiliating laugh_1 laugh_8", %count * %buf);
      return;
   }
   if(%a == 8 || %b == 8)
   {
      OpsAnn::pick("killingblow mercykill", %count * %buf);
      %count++;
   }
   %winner = (%a > %b) ? 0 : 1;
   %hi = (%a > %b) ? %a : %b;
   %lo = (%a > %b) ? %b : %a;
   if(%hi >= %lo + $OpsAnn::MASSACRE_THRESHOLD)
      OpsAnn::play("smackdown", %count * %buf);
   else if(Team::Friendly() == %winner)
      OpsAnn::pick("you_win congratulations", %count * %buf);
   else
      OpsAnn::pick("you_lose loser", %count * %buf);
}

// Time callouts off the match clock (getHudTimer: negative = counting down).
// A 1 s loop per connection, generation-stamped so a restart never doubles it.
function OpsAnn::clockStart()
{
   $OpsAnn::ClockGen++;
   $OpsAnn::ClockLast = "";
   schedule("OpsAnn::clockTick(" @ $OpsAnn::ClockGen @ ");", 1);
}

function OpsAnn::clockTick(%gen)
{
   if(%gen != $OpsAnn::ClockGen)
      return;
   schedule("OpsAnn::clockTick(" @ %gen @ ");", 1);
   if(!OpsAnn::on() || $OpsAnn::ScountdownStarted)
      return;
   %t = getHudTimer();
   if(%t == "" || %t >= 0)
   {
      $OpsAnn::ClockLast = "";
      return;
   }
   %left = floor(-%t);
   %last = $OpsAnn::ClockLast;
   $OpsAnn::ClockLast = %left;
   if(%last == "")
      return;
   // announce each mark crossed since the last tick
   %marks = "300 180 60 30 10 9 8 7 6 5 4 3 2 1";
   %names = "cd5min cd3min cd1min cd30sec cd10 cd9 cd8 cd7 cd6 cd5 cd4 cd3 cd2 cd1";
   for(%i = 0; %i < 14; %i++)
   {
      %m = getWord(%marks, %i);
      if(%last > %m && %left <= %m)
         OpsAnn::play(getWord(%names, %i), 0);
   }
}

//------------------------------------------------------------------------------
// Kills
//------------------------------------------------------------------------------
function OpsAnn::kill(%killer, %victim, %weapon)
{
   if(!OpsAnn::on()) return;
   %now = getSimTime();
   $OpsAnn::Sspree[%victim] = 0;
   %last = $OpsAnn::SlastKill[%killer];
   %buf = $OpsAnn::BUFFER;
   if(%last == "")
   {
      $OpsAnn::Sstreak[%killer] = 1;
      $OpsAnn::Sspree[%killer] = 1;
   }
   else if(%now - %last < $OpsAnn::KILL_STREAK_TIME)
   {
      $OpsAnn::Sstreak[%killer]++;
      $OpsAnn::Sspree[%killer]++;
      if($OpsAnn::Sstreak[%killer] == 2)      OpsAnn::play("doublekill", 0);
      else if($OpsAnn::Sstreak[%killer] == 3) OpsAnn::play("triplekill", 0);
      else if($OpsAnn::Sstreak[%killer] > 3)  OpsAnn::play("ultrakill", 0);
   }
   else
   {
      $OpsAnn::Sstreak[%killer] = 1;
      $OpsAnn::Sspree[%killer]++;
      if($OpsAnn::Sspree[%killer] > 2)
         OpsAnn::play("killingspree", 0);
   }
   $OpsAnn::SlastKill[%killer] = %now;
}

function OpsAnn::teamKill(%killer, %victim)
{
   if(!OpsAnn::on()) return;
   %now = getSimTime();
   $OpsAnn::Sspree[%victim] = 0;
   %last = $OpsAnn::SlastTK[%killer];
   $OpsAnn::SlastTK[%killer] = %now;
   if(%last != "" && %now - %last < $OpsAnn::TEAMKILL_TIME)
   {
      $OpsAnn::StkStreak[%killer]++;
      if($OpsAnn::StkStreak[%killer] > 1)
         OpsAnn::play("teamkiller", 0);
   }
   else
      $OpsAnn::StkStreak[%killer] = 1;
}

function OpsAnn::suicide(%victim)
{
   $OpsAnn::Sspree[%victim] = 0;
}

function OpsAnn::carrierKill(%killer)
{
   if(!OpsAnn::on()) return;
   %ck = OpsStats::get(CarrierKills, Client::getName(%killer));
   if(%ck == 15)       OpsAnn::play("dominating", 0);
   else if(%ck == 20)  OpsAnn::play("godlike", 0);
   else if(%ck >= 25)  OpsAnn::play("holyshit", 0);
   $OpsAnn::SlastCK[Client::getTeam(%killer)] = %killer;
}

//------------------------------------------------------------------------------
// Flags (%team = the flag's team, %cl = who did it)
//------------------------------------------------------------------------------
function OpsAnn::cap(%team, %cl)
{
   if(!OpsAnn::on()) return;
   %buf = $OpsAnn::BUFFER;
   %count = 0;

   if(!$OpsAnn::SfirstCap)
   {
      $OpsAnn::SfirstCap = true;
      OpsAnn::play("firstblood_4", %count * %buf);
      %count++;
   }

   %caps = OpsStats::get(Caps, Client::getName(%cl));
   if(%caps == 3)      { OpsAnn::play("hattrick", %count * %buf);      %count++; }
   else if(%caps == 4) { OpsAnn::play("unstoppable_1", %count * %buf); %count++; }
   else if(%caps > 4)  { OpsAnn::play("rampage", %count * %buf);       %count++; }

   // cap soon after a teammate's pickup
   %pk = $OpsAnn::SlastPickup[%team];
   if(%pk != "" && getSimTime() - %pk < $OpsAnn::ASSIST_TIME &&
      $OpsAnn::SlastCarrier[%team] != "" && $OpsAnn::SlastCarrier[%team] != %cl)
   {
      OpsAnn::play("assist", %count * %buf);
      %count++;
   }

   %capping = %team ^ 1;
   %flagCaps = $OpsStats::TeamCaps[%team];      if(%flagCaps == "") %flagCaps = 0;
   %capCaps  = $OpsStats::TeamCaps[%capping];   if(%capCaps == "")  %capCaps = 0;

   if(%flagCaps == %capCaps)
   {
      OpsAnn::play("teams_tied", %count * %buf);
      %count++;
   }
   else if(%capCaps == %flagCaps + 1)
   {
      if(%capping == Team::Friendly())
         OpsAnn::play("taken_lead_1", %count * %buf);
      else
         OpsAnn::play("lost_lead_1", %count * %buf);
      %count++;
   }

   // a team's cap streak
   if($OpsAnn::SlastTeamToCap == %capping)
   {
      $OpsAnn::ScapStreak[%capping]++;
      if($OpsAnn::ScapStreak[%capping] > 2)
      {
         OpsAnn::play("ownage", %count * %buf);
         %count++;
      }
   }
   else
      $OpsAnn::ScapStreak[%capping] = 1;
   $OpsAnn::SlastTeamToCap = %capping;

   if(%capCaps >= %flagCaps + $OpsAnn::MASSACRE_THRESHOLD)
   {
      OpsAnn::play("massacre", %count * %buf);
      %count++;
   }

   // "finish it" when we are one cap from the balanced-mode 8
   if(%team == Team::Enemy() && %capCaps == 7 && %flagCaps != 7)
      OpsAnn::pick("finishit endit", %count * %buf);
}

function OpsAnn::drop(%team, %cl)
{
   $OpsAnn::Sdropped[%team] = true;
   $OpsAnn::SlastDrop[%team] = getSimTime();
}

function OpsAnn::grab(%team, %cl)
{
   $OpsAnn::SlastGrab[%team] = getSimTime();
   $OpsAnn::SlastCK[%team] = -1;
   $OpsAnn::Sdropped[%team] = false;
}

function OpsAnn::pickup(%team, %cl)
{
   $OpsAnn::SlastPickup[%team] = getSimTime();
   $OpsAnn::SlastCarrier[%team] = %cl;
   if(!$OpsAnn::SwasCaught[%team])
      $OpsAnn::ScatchStreak[%team] = 0;
   $OpsAnn::SwasCaught[%team] = false;
   $OpsAnn::SlastCK[%team] = -1;
}

function OpsAnn::ret(%team, %cl)
{
   $OpsAnn::ScatchStreak[%team] = 0;
   $OpsAnn::SlastReturn[%team] = getSimTime();
}

function OpsAnn::interception(%team, %cl)
{
   if(!OpsAnn::on()) return;
   OpsAnn::play("denied", $OpsAnn::BUFFER);
}

function OpsAnn::catch(%team, %cl)
{
   if(!OpsAnn::on()) return;
   %buf = $OpsAnn::BUFFER;
   %count = 0;
   $OpsAnn::SwasCaught[%team] = true;
   if(getSimTime() - $OpsAnn::SlastDrop[%team] > $OpsAnn::LONG_CATCH_TIME)
   {
      OpsAnn::play("impressive_1", 0);
      %count++;
   }
   $OpsAnn::ScatchStreak[%team]++;
   if($OpsAnn::ScatchStreak[%team] > $OpsAnn::PASS_STREAK_THRESHOLD)
      OpsAnn::play("excellent_1", %count * %buf);
}

function OpsAnn::midAirCK(%cl)
{
   if(!OpsAnn::on()) return;
   OpsAnn::play("headshot_2", 0);
}

function OpsAnn::midAirDisc(%shooter, %victim)
{
   if(!OpsAnn::on()) return;
   if(OpsStats::get(MAGiven, Client::getName(%shooter)) == 15)
      OpsAnn::play("headhunter", $OpsAnn::BUFFER);
}

//------------------------------------------------------------------------------
// Hooks (NativeDefaults::attachEvents).
//------------------------------------------------------------------------------
function OpsAnn::attach()
{
   Event::Attach(eventChangeMission, OpsAnn::missionOver);
   Event::Attach(eventConnected,     OpsAnn::clockStart);
}

OpsAnn::reset();
