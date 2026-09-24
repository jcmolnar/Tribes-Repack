//==============================================================================
// OPS STATS -- Opsaya's stat collector + stat sheet, rebuilt for this client.
// Options > 07 SCRIPTS "Stat sheet" ($pref::scriptStats). Hold the "Show stat sheet"
// key (Options > Controls; Opsaya's was Left-Alt+S) to see it.
//
// Source: his config\Modules\Stats\Collector.acs.cs + StatHUD.acs.cs. Those sat on his
// own event core (Core\*.cs + LegacyBridge/ZadminBridge/TagString), which redefines
// functions Presto owns here (onClientMessage handling, remoteITXT, KillTrak), so
// instead of loading it this file feeds the same tallies from the sources this
// client already has:
//   kills      killPop.cpp: eventClientKilled / eventClientTeamKilled (every server)
//   flags      Presto TeamTrak: eventFlagTaken / Dropped / Captured / Returned
//   stat RPCs  remoteT "ST" / "KD" (nativeDefaults.cs) on servers that send them --
//              the richer events (mid-airs, catches, interceptions, damage, score)
//              only exist there, exactly as in the original
// Once a server sends KD (or flag ST) messages, the chat-derived kills (or flags)
// are ignored so nothing counts twice.
//
// NOT carried: the end-of-map file export (collect.cs / out.cs -- out.cs was
// export("$*"), every global variable including saved passwords), flag/team time
// tracking (fed only that export), ratings.
//
// Tallies are keyed by player NAME, as the original's were, and reset on connect
// and at "Match started.". The announcer (opsAnnouncer.cs) reads them too, so they
// are collected whenever either script is on.
//==============================================================================

function OpsStats::on()
{
   return ($pref::scriptStats == 1) || ($pref::scriptAnnouncer == 1);
}

function OpsStats::clear()
{
   deleteVariables("$OpsStats::T*");     // tallies ($OpsStats::T<stat>[name])
   deleteVariables("$OpsStats::Tag*");   // "this server speaks KD / ST"
   deleteVariables("$OpsStats::Flag*");
   deleteVariables("$OpsStats::DeathAt*");
   deleteVariables("$OpsStats::DropAt*");
   deleteVariables("$OpsStats::Killer*");
   deleteVariables("$OpsStats::CK*");
   $OpsStats::TeamCaps[0] = 0;
   $OpsStats::TeamCaps[1] = 0;
   $OpsStats::FlagState[0] = "home";
   $OpsStats::FlagState[1] = "home";
}

function OpsStats::add(%stat, %cl, %n)
{
   %name = Client::getName(%cl);
   if(%name == "")
      return;
   if(%n == "") %n = 1;
   $OpsStats::T[%stat, %name] += %n;
}

function OpsStats::get(%stat, %name)
{
   %v = $OpsStats::T[%stat, %name];
   return (%v == "") ? 0 : %v;
}

// Grenade / Mine / killPop's "Explosives" are one column, as unifyDamageTypes did.
function OpsStats::weapon(%w)
{
   if(%w == "Grenade" || %w == "Mine" || %w == "Explosives")
      return "Explosive";
   return %w;
}

//------------------------------------------------------------------------------
// The derived events. Every source funnels into these, and each one tells the
// announcer too (OpsAnn::* exists only when opsAnnouncer.cs is loaded).
//------------------------------------------------------------------------------
function OpsStats::kill(%killer, %victim, %weapon)
{
   %weapon = OpsStats::weapon(%weapon);
   if(%killer == %victim || %killer == "" || %killer == 0)
   {
      OpsStats::add(Deaths, %victim);
      OpsStats::add(Suicides, %victim);
      if(isFunction("OpsAnn::suicide")) OpsAnn::suicide(%victim);
      return;
   }
   OpsStats::add(Kills, %killer);
   OpsStats::add(Deaths, %victim);
   $OpsStats::T[Kills, Client::getName(%killer), %weapon]++;
   $OpsStats::T[Deaths, Client::getName(%victim), %weapon]++;
   OpsStats::deathForCK(%killer, %victim);
   if(isFunction("OpsAnn::kill")) OpsAnn::kill(%killer, %victim, %weapon);
}

function OpsStats::teamKill(%killer, %victim, %weapon)
{
   OpsStats::add(TeamKills, %killer);
   OpsStats::add(TeamDeaths, %victim);
   if(isFunction("OpsAnn::teamKill")) OpsAnn::teamKill(%killer, %victim);
}

function OpsStats::carrierKill(%killer)
{
   OpsStats::add(CarrierKills, %killer);
   if(isFunction("OpsAnn::carrierKill")) OpsAnn::carrierKill(%killer);
}

function OpsStats::grab(%team, %cl)
{
   $OpsStats::FlagState[%team] = "carried";
   OpsStats::add(Grabs, %cl);
   if(isFunction("OpsAnn::grab")) OpsAnn::grab(%team, %cl);
}

function OpsStats::pickup(%team, %cl)
{
   $OpsStats::FlagState[%team] = "carried";
   OpsStats::add(Pickups, %cl);
   if(isFunction("OpsAnn::pickup")) OpsAnn::pickup(%team, %cl);
}

function OpsStats::drop(%team, %cl)
{
   $OpsStats::FlagState[%team] = "field";
   OpsStats::add(Drops, %cl);
   OpsStats::dropForCK(%cl);
   if(isFunction("OpsAnn::drop")) OpsAnn::drop(%team, %cl);
}

function OpsStats::ret(%team, %cl)
{
   $OpsStats::FlagState[%team] = "home";
   if(%cl != "" && %cl != 0)
      OpsStats::add(Returns, %cl);
   if(isFunction("OpsAnn::ret")) OpsAnn::ret(%team, %cl);
}

function OpsStats::cap(%team, %cl)
{
   $OpsStats::FlagState[0] = "home";
   $OpsStats::FlagState[1] = "home";
   OpsStats::add(Caps, %cl);
   $OpsStats::TeamCaps[%team ^ 1]++;       // %team is the CAPTURED flag's team
   if(isFunction("OpsAnn::cap")) OpsAnn::cap(%team, %cl);
}

// A carrier kill has no line of its own off a stats server: the death and the flag
// drop arrive as two messages, in either order. Pair them when they land within half
// a second, once per death.
function OpsStats::deathForCK(%killer, %victim)
{
   %now = getSimTime();
   $OpsStats::DeathAt[%victim] = %now;
   $OpsStats::Killer[%victim] = %killer;
   $OpsStats::CKDone[%victim] = "";
   if($OpsStats::TagFlags) return;
   %d = %now - $OpsStats::DropAt[%victim];
   if($OpsStats::DropAt[%victim] != "" && %d >= 0 && %d < 0.5)
   {
      $OpsStats::CKDone[%victim] = 1;
      OpsStats::carrierKill(%killer);
   }
}

function OpsStats::dropForCK(%cl)
{
   %now = getSimTime();
   $OpsStats::DropAt[%cl] = %now;
   if($OpsStats::TagFlags || $OpsStats::CKDone[%cl]) return;
   %d = %now - $OpsStats::DeathAt[%cl];
   if($OpsStats::DeathAt[%cl] != "" && %d >= 0 && %d < 0.5)
   {
      $OpsStats::CKDone[%cl] = 1;
      OpsStats::carrierKill($OpsStats::Killer[%cl]);
   }
}

//------------------------------------------------------------------------------
// Source 1 + 2: killPop and Presto (every server).
//------------------------------------------------------------------------------
function OpsStats::onKilled(%killer, %victim, %weapon)
{
   if(!OpsStats::on() || $OpsStats::TagKills) return;
   OpsStats::kill(%killer, %victim, %weapon);
}

function OpsStats::onTeamKilled(%killer, %victim, %weapon)
{
   if(!OpsStats::on() || $OpsStats::TagKills) return;
   OpsStats::teamKill(%killer, %victim, %weapon);
}

// Presto's "took" covers both: from the stand is a grab, from the field a pickup.
function OpsStats::onTaken(%team, %cl)
{
   if(!OpsStats::on() || $OpsStats::TagFlags) return;
   if($OpsStats::FlagState[%team] == "field")
      OpsStats::pickup(%team, %cl);
   else
      OpsStats::grab(%team, %cl);
}

function OpsStats::onDropped(%team, %cl)
{
   if(!OpsStats::on() || $OpsStats::TagFlags) return;
   OpsStats::drop(%team, %cl);
}

function OpsStats::onReturned(%team, %cl)
{
   if(!OpsStats::on() || $OpsStats::TagFlags) return;
   OpsStats::ret(%team, %cl);
}

function OpsStats::onCaptured(%team, %cl)
{
   if(!OpsStats::on() || $OpsStats::TagFlags) return;
   OpsStats::cap(%team, %cl);
}

function OpsStats::onMatchStarted(%client, %msg)
{
   if(%client != 0) return;
   OpsStats::clear();
   if(isFunction("OpsAnn::matchStarted")) OpsAnn::matchStarted();
}

function OpsStats::onCountdown(%client, %msg)
{
   if(%client != 0) return;
   if(Match::String(%msg, "Match starts in * seconds."))
      if(isFunction("OpsAnn::countdown")) OpsAnn::countdown(Match::Result(0));
}

//------------------------------------------------------------------------------
// Source 3: tagged stat RPCs (nativeDefaults.cs remoteT hands ST / KD here).
//   KD  template "type/killweight/deathweight", p0 killer, p1 victim
//   ST  template "Category/weight", p0 client, p1 amount, p2.. the category's args
//       (TagString.cs: ClientEvents::on<Category>(client, p2, p3, ...))
//------------------------------------------------------------------------------
function OpsStats::onTagged(%type, %tag, %p0, %p1, %p2, %p3, %p4, %p5, %p6)
{
   if(!OpsStats::on()) return;
   if(%type == "KD")
   {
      if(String::explode(%tag, "/", "opsKD") != 3) return;
      $OpsStats::TagKills = 1;
      OpsStats::score(%p0, $opsKD[1], 1);
      OpsStats::score(%p1, $opsKD[2], 1);
      if(%p0 == %p1 || %p0 == "" || %p0 == 0 || Client::getTeam(%p0) != Client::getTeam(%p1))
         OpsStats::kill(%p0, %p1, $opsKD[0]);
      else
         OpsStats::teamKill(%p0, %p1, $opsKD[0]);
      return;
   }
   if(%type != "ST") return;
   if(String::explode(%tag, "/", "opsST") != 2) return;
   %cat = $opsST[0];
   OpsStats::score(%p0, $opsST[1], %p1);
   if(String::findSubStr(%cat, "Flag") == 0)
      $OpsStats::TagFlags = 1;
   %fn = "OpsStats::st" @ %cat;
   if(isFunction(%fn))
      *%fn(%p0, %p2, %p3, %p4, %p5);
}

function OpsStats::score(%cl, %weight, %amt)
{
   if(%amt == "") %amt = 1;
   OpsStats::add(Score, %cl, %weight * %amt);
}

// ClientEvents::on<Category>(%cl, ...) signatures, from the original Core\FlagEvents.cs
// and the Collector's handlers.
function OpsStats::stFlagGrab(%cl, %team)            { OpsStats::grab(%team, %cl); }
function OpsStats::stFlagPickup(%cl, %team)          { OpsStats::pickup(%team, %cl); }
function OpsStats::stFlagDrop(%cl, %team)            { OpsStats::drop(%team, %cl); }
function OpsStats::stFlagReturn(%cl, %team)          { OpsStats::ret(%team, %cl); }
function OpsStats::stFlagStandoffReturn(%cl, %team)  { OpsStats::add(StandoffReturns, %cl); }
function OpsStats::stFlagCap(%cl, %team)             { OpsStats::cap(%team, %cl); }
function OpsStats::stFlagAssist(%cl)                 { OpsStats::add(Assists, %cl); }
function OpsStats::stFlagCarrierKill(%cl)            { OpsStats::carrierKill(%cl); }
function OpsStats::stFlagEGrab(%cl)                  { OpsStats::add(EGrabs, %cl); }
function OpsStats::stFlagClutchReturn(%cl)           { OpsStats::add(ClutchReturns, %cl); }
function OpsStats::stFlagInterception(%cl, %team)
{
   OpsStats::add(Interceptions, %cl);
   if(isFunction("OpsAnn::interception")) OpsAnn::interception(%team, %cl);
}
function OpsStats::stFlagCatch(%cl, %team)
{
   OpsStats::add(Catches, %cl);
   if(isFunction("OpsAnn::catch")) OpsAnn::catch(%team, %cl);
}
function OpsStats::stMidAirDisc(%cl, %victim)
{
   OpsStats::add(MAGiven, %cl);
   OpsStats::add(MATaken, %victim);
   if(isFunction("OpsAnn::midAirDisc")) OpsAnn::midAirDisc(%cl, %victim);
}
function OpsStats::stMidAirNade(%cl)                 { OpsStats::add(MANades, %cl); }
function OpsStats::stMidAirCK(%cl)
{
   OpsStats::add(MidAirCK, %cl);
   if(isFunction("OpsAnn::midAirCK")) OpsAnn::midAirCK(%cl);
}
function OpsStats::stBodyBlock(%cl, %victim)
{
   OpsStats::add(BBGiven, %cl);
   OpsStats::add(BBTaken, %victim);
}
function OpsStats::stDamageDealt(%cl, %victim, %dmg)
{
   OpsStats::add(DamageOut, %cl, %dmg);
   OpsStats::add(DamageIn, %victim, %dmg);
}
function OpsStats::stTeamDamageDealt(%cl, %victim, %dmg)
{
   OpsStats::add(TeamDamageOut, %cl, %dmg);
   OpsStats::add(TeamDamageIn, %victim, %dmg);
}
function OpsStats::stNadeJump(%cl)                   { OpsStats::add(NadeJumps, %cl); }

//------------------------------------------------------------------------------
// The stat sheet (StatHUD.acs.cs): a tab-aligned centre print, one block per team,
// sorted by score then kills, your own row highlighted. Held on a key: make shows,
// break hides ($StatHUD::HoldDisplay = 1 in the original).
//------------------------------------------------------------------------------
$OpsStats::Font = "sf_orange214_10.pft";

function OpsStats::px(%str)
{
   return Font::getStringPixelWidth($OpsStats::Font, %str);
}

// Column widths in "spacer" units (three spaces, how SimGui::TextFormat sizes a tab).
$OpsStats::ColUnits = "10 4 3 3 3 3 3 3 4 3 3 4 4 3 4 4 4";

function OpsStats::cell(%col, %text)
{
   %sp = OpsStats::px(" ") * 3;
   if(%sp <= 0) %sp = 9;
   %colPx = getWord($OpsStats::ColUnits, %col) * %sp;
   %tabs = floor((%colPx - OpsStats::px(%text) + %sp - 1) / %sp);
   if(%tabs < 1) %tabs = 1;
   if(%col == 0)
      %text = String::escapeFormatting(%text);
   return %text @ String::Dup("\t", %tabs);
}

function OpsStats::pair(%a, %b)
{
   return OpsStats::num(%a) @ "/" @ OpsStats::num(%b);
}

function OpsStats::num(%n)
{
   return (%n == "") ? 0 : %n;
}

function OpsStats::row(%me, %c0, %c1, %c2, %c3, %c4, %c5, %c6, %c7, %c8, %c9, %c10, %c11, %c12, %c13, %c14, %c15, %c16)
{
   %line = "\t\t\t\t" @ (%me ? "<f0>" : "<f1>");
   for(%i = 0; %i < 17; %i++)
      %line = %line @ OpsStats::cell(%i, %c[%i]);
   return %line @ "\n";
}

function OpsStats::nameRow(%n)
{
   %me = (%n == Client::getName(getManagerId()));
   return OpsStats::row(%me, %n,
      OpsStats::get(Score, %n),
      OpsStats::pair($OpsStats::T[Kills, %n], $OpsStats::T[Deaths, %n]),
      OpsStats::pair($OpsStats::T[TeamKills, %n], $OpsStats::T[TeamDeaths, %n]),
      OpsStats::pair($OpsStats::T[Kills, %n, "Disc"], $OpsStats::T[Deaths, %n, "Disc"]),
      OpsStats::pair($OpsStats::T[Kills, %n, "Explosive"], $OpsStats::T[Deaths, %n, "Explosive"]),
      OpsStats::pair($OpsStats::T[Kills, %n, "Chaingun"], $OpsStats::T[Deaths, %n, "Chaingun"]),
      OpsStats::get(CarrierKills, %n),
      OpsStats::get(Returns, %n) + OpsStats::get(StandoffReturns, %n),
      OpsStats::pair($OpsStats::T[BBGiven, %n], $OpsStats::T[BBTaken, %n]),
      OpsStats::get(Grabs, %n),
      OpsStats::get(Pickups, %n),
      OpsStats::get(Assists, %n),
      OpsStats::get(Caps, %n),
      OpsStats::pair($OpsStats::T[MAGiven, %n], $OpsStats::T[MATaken, %n]),
      OpsStats::pair(floor(OpsStats::get(DamageOut, %n) / 10), floor(OpsStats::get(DamageIn, %n) / 10)),
      OpsStats::pair(floor(OpsStats::get(TeamDamageOut, %n) / 10), floor(OpsStats::get(TeamDamageIn, %n) / 10)));
}

function OpsStats::better(%a, %b)
{
   %sa = OpsStats::get(Score, %a);  %sb = OpsStats::get(Score, %b);
   if(%sa != %sb) return %sa > %sb;
   return OpsStats::get(Kills, %a) > OpsStats::get(Kills, %b);
}

function OpsStats::sheet()
{
   %text = OpsStats::row(false, "Name", "Rating", "K/D", "TKs", "Disc", "Nade", "Chain", "CKills",
                         "Returns", "BBs", "Grabs", "Pickups", "Assists", "Caps", "MAs", "Dmg", "TmDmg") @ "\n";
   for(%team = -1; %team <= 1; %team++)
   {
      %n = 0;
      %cl = Client::getFirst();
      for(%g = 0; %g < 128 && %cl != -1 && %cl != ""; %g++)
      {
         if(Client::getTeam(%cl) == %team)
         {
            %name = Client::getName(%cl);
            // insertion sort, best first
            %pos = %n;
            for(%j = 0; %j < %n; %j++)
               if(OpsStats::better(%name, %list[%j])) { %pos = %j; break; }
            for(%j = %n; %j > %pos; %j--)
               %list[%j] = %list[%j - 1];
            %list[%pos] = %name;
            %n++;
         }
         %cl = Client::getNext(%cl);
      }
      for(%j = 0; %j < %n; %j++)
         %text = %text @ OpsStats::nameRow(%list[%j]);
      if(%n > 0)
         %text = %text @ "\n";
   }
   return %text;
}

function OpsStats::show(%down)
{
   if($pref::scriptStats != 1)
      return;
   if(%down)
      remoteCP(2048, OpsStats::sheet(), 100);
   else
      remoteCP(2048, "", 0);
}

//------------------------------------------------------------------------------
// Hooks. Called from NativeDefaults::attachEvents, which runs again after Presto
// installs its event table (see autoexec.cs).
//------------------------------------------------------------------------------
function OpsStats::attach()
{
   Event::Attach(eventClientKilled,       OpsStats::onKilled);
   Event::Attach(eventClientTeamKilled,   OpsStats::onTeamKilled);
   Event::Attach(eventFlagTaken,          OpsStats::onTaken);
   Event::Attach(eventFlagDropped,        OpsStats::onDropped);
   Event::Attach(eventFlagReturned,       OpsStats::onReturned);
   Event::Attach(eventFlagCaptured,       OpsStats::onCaptured);
   Event::Attach(eventConnectionAccepted, OpsStats::clear);
   // Presto's chat matcher; handlers stack, so TeamTrak keeps its own. Once only.
   if(isFunction("msg::onMatch") && !$OpsStats::MsgHooked)
   {
      $OpsStats::MsgHooked = true;
      msg::onMatch("Match started.", "OpsStats::onMatchStarted(%client, %msg);");
      msg::onMatch("Match starts in * seconds.", "OpsStats::onCountdown(%client, %msg);");
   }
}

OpsStats::clear();
