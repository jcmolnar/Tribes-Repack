//==============================================================================
// Vantage -- BORROWABLE PARTS (components.cs)
//
// Split out of hud.cs by tools/modernhud_components.py so another pack can draw these
// parts: the HUD designer's Provider choice sets $pref::HudSlot::<slot> to
// "vantage/<slot>", and ModernHUD::drawSlot execs this file on demand. It must define only
// Vantage:: names -- ModernHUDPack:: belongs to whichever pack is the base.
// hud.cs execs this file too, so the pack's own draw uses the same code.
//==============================================================================

ModernHUD::require("ModernHUD/Core/Data/Team.cs");
ModernHUD::require("ModernHUD/Core/Data/Timer.cs");


//------------------------------------------------------------------------------
// The palette. One place, so the whole HUD moves together.
//
// These are RGB triples fed to $pref::Hud::Color* (which resolve through the
// engine palette's GetNearestColor) and to glColor4ub for our own draws. Using
// the SAME numbers for both is the point: our bars and the engine's brackets,
// crosshair box and chat end up the same colour.
//------------------------------------------------------------------------------
// Per-frame prep for a BORROWED Vantage part: the base pack runs Vantage::apply() at
// init (which sets the palette and swaps live prefs); a borrowed part must not swap
// anything, so it takes only the palette.
function Vantage::compPrep()
{
   if($ModernHUD::PackId != "vantage")
      Vantage::palette();
}

function Vantage::palette()
{
   $Vantage::Primary  = "0 200 255";      // cyan -- the HUD's voice
   $Vantage::Dim      = "0 62 78";        // the same hue, backgrounded
   $Vantage::Accent   = "255 190 60";     // amber -- "attention", not "danger"
   $Vantage::Warn     = "255 60 60";      // red   -- danger only
   $Vantage::Text     = "235 245 255";
   $Vantage::Pass     = "255 105 180";    // flag carrier
}

// Set the raw-GL draw colour from one of the palette entries.
function Vantage::color(%rgb, %alpha)
{
   glColor4ub(getWord(%rgb, 0), getWord(%rgb, 1), getWord(%rgb, 2), %alpha);
}

// A filled bar with a dim bed behind it. %frac is 0..1.
function Vantage::meter(%x, %y, %w, %h, %frac, %rgb, %alpha)
{
   if(%frac < 0) %frac = 0;
   if(%frac > 1) %frac = 1;

   Vantage::color($Vantage::Dim, %alpha * 0.55);
   glRectangle(%x, %y, %w, %h);

   %fill = floor(%w * %frac);
   if(%fill > 0)
   {
      Vantage::color(%rgb, %alpha);
      glRectangle(%x, %y, %fill, %h);
   }
}

// Text in a palette colour. Uses the self-contained <f:file:rgba:shadow:dx,dy>
// markup form rather than <fN> so the pack never depends on whatever font table
// the previously-loaded pack happened to leave behind.
function Vantage::text(%x, %y, %width, %rgb, %str, %alpha)
{
   %hex = Vantage::hex(%rgb);
   ModernHUD::markup(%x, %y, %width,
      "<f:sf_white_10b.pft:" @ %hex @ "ff:000000c0:1,1>" @ %str, %alpha);
}

function Vantage::textSmall(%x, %y, %width, %rgb, %str, %alpha)
{
   %hex = Vantage::hex(%rgb);
   ModernHUD::markup(%x, %y, %width,
      "<f:sf_white_7.pft:" @ %hex @ "ff:000000c0:1,1>" @ %str, %alpha);
}

// "r g b" -> "rrggbb". The console has no printf, so this is a nibble table.
function Vantage::hex(%rgb)
{
   return Vantage::hex2(getWord(%rgb, 0)) @ Vantage::hex2(getWord(%rgb, 1)) @
          Vantage::hex2(getWord(%rgb, 2));
}

function Vantage::hex2(%v)
{
   if(%v < 0)   %v = 0;
   if(%v > 255) %v = 255;
   return Vantage::nib(floor(%v / 16)) @ Vantage::nib(%v - floor(%v / 16) * 16);
}

function Vantage::nib(%n)
{
   if(%n <= 9)
      return %n;
   if(%n == 10) return "a";
   if(%n == 11) return "b";
   if(%n == 12) return "c";
   if(%n == 13) return "d";
   if(%n == 14) return "e";
   return "f";
}

//------------------------------------------------------------------------------
// PART: vitals -- health + energy, bottom centre, with a DAMAGE TRAIL.
//
// ★The trail is the widget this pack exists for.★ The health bar drops to the
// real value instantly; a second, lighter bar behind it falls to meet it over
// ~400ms. You see how much you just LOST, not only what is left -- which in a
// game where damage arrives as discrete disc hits is the more useful fact.
//
// A retained control cannot do this: it has no per-frame tick, which is why
// every legacy health plate is a number that teleports. Immediate mode plus
// glTicks (a real ms wall clock) makes it three lines.
//------------------------------------------------------------------------------
function Vantage::Vitals(%x, %y, %w)
{
   %health = $health;
   %energy = $energy;
   if(%health == "") %health = 0;
   if(%energy == "") %energy = 0;

   %now = glTicks();

   // Trail state. Seeded on first draw so the bar does not sweep in from zero
   // the moment you spawn.
   if($Vantage::TrailInit == "")
   {
      $Vantage::TrailInit = 1;
      $Vantage::Trail = %health;
      $Vantage::TrailAt = %now;
   }

   if(%health > $Vantage::Trail)
   {
      // Healed: no trail. A trail on the way UP would be showing you a deficit
      // you no longer have.
      $Vantage::Trail = %health;
   }
   else if($Vantage::Trail > %health)
   {
      // 400ms to close the gap, frame-rate independent because the step is
      // scaled by the REAL elapsed ms, not by a per-frame constant.
      %dt = %now - $Vantage::TrailAt;
      if(%dt < 0)    %dt = 0;      // clock reset (map change) -- do not lurch
      if(%dt > 250)  %dt = 250;    // long stall -- do not snap
      %step = ($Vantage::Trail - %health) * (%dt / 400);
      $Vantage::Trail = $Vantage::Trail - %step;
      if($Vantage::Trail < %health)
         $Vantage::Trail = %health;
   }
   $Vantage::TrailAt = %now;

   // Colour by threshold. Amber is "pay attention", red is "you are dying" --
   // two distinct states, not a gradient, because a gradient tells you nothing
   // you can act on at a glance.
   if(%health > 66)      %hc = $Vantage::Primary;
   else if(%health > 33) %hc = $Vantage::Accent;
   else                  %hc = $Vantage::Warn;

   %barW = %w;

   // trail first, behind
   if($Vantage::Trail > %health)
   {
      Vantage::color($Vantage::Warn, 90);
      glRectangle(%x, %y, floor(%barW * ($Vantage::Trail / 100)), 7);
   }

   Vantage::meter(%x, %y, %barW, 7, %health / 100, %hc, 235);
   Vantage::meter(%x, %y + 9, %barW, 4, %energy / 100, $Vantage::Primary, 190);

   // ★The number appears only when it matters.★ A readout you look at every
   // frame is a readout you have stopped seeing; one that only exists below 40
   // is an alarm.
   if(%health < 40)
      Vantage::text(%x, %y - 18, %barW, $Vantage::Warn, "<jc>" @ floor(%health), 255);
}

//------------------------------------------------------------------------------
// PART: weapon -- name + ammo, bottom right.
//------------------------------------------------------------------------------
function Vantage::Weapon(%x, %y, %w)
{
   %wep = GetItemDesc(GetMountedItem(0));
   if(%wep == "")
      return;

   %ammo = $Weapon::Ammo;

   Vantage::text(%x, %y, %w, $Vantage::Text, "<jr>" @ %wep, 220);

   if(%ammo == "" || %ammo < 0)
   {
      // Energy weapons report no ammo count. Say so rather than drawing a bar
      // that is always empty.
      Vantage::textSmall(%x, %y + 16, %w, $Vantage::Dim, "<jr>--", 200);
      return;
   }

   if(%ammo <= 2) %ac = $Vantage::Warn;
   else           %ac = $Vantage::Accent;

   Vantage::text(%x, %y + 14, %w, %ac, "<jr>" @ %ammo, 255);
}

//------------------------------------------------------------------------------
// PART: ctf -- both scores and both flag states, top centre.
//
// Uses the shared Team.cs data layer that ships with the framework's Core/Data,
// the same one the converted packs require. Degrades to scores alone when the
// layer is not present rather than erroring per frame.
//------------------------------------------------------------------------------
function Vantage::Ctf(%x, %y, %w)
{
   %mine = Team::Friendly();
   %theirs = Team::Enemy();
   if(%mine == "" || %theirs == "")
      return;

   %s0 = Team::Score(%mine);
   %s1 = Team::Score(%theirs);
   if(%s0 == "") %s0 = 0;
   if(%s1 == "") %s1 = 0;

   %half = floor(%w / 2);

   Vantage::text(%x, %y, %half - 10, $Vantage::Primary, "<jr>" @ %s0, 255);
   Vantage::text(%x + %half + 10, %y, %half - 10, $Vantage::Warn, "<jl>" @ %s1, 255);
   Vantage::textSmall(%x, %y + 3, %w, $Vantage::Dim, "<jc>/", 200);

   Vantage::flagState(%x, %y + 18, %half - 10, %mine, $Vantage::Primary, "<jr>");
   Vantage::flagState(%x + %half + 10, %y + 18, %half - 10, %theirs, $Vantage::Warn, "<jl>");
}

function Vantage::flagState(%x, %y, %w, %team, %rgb, %just)
{
   %loc = Team::Flag::Location(%team);
   if(%loc == "")
      return;

   if(%loc == "home")
      Vantage::textSmall(%x, %y, %w, $Vantage::Dim, %just @ "home", 190);
   else if(%loc == "field")
      Vantage::textSmall(%x, %y, %w, $Vantage::Accent, %just @ "dropped", 235);
   else
      Vantage::textSmall(%x, %y, %w, %rgb,
         %just @ String::escapeFormatting(Client::GetName(%loc)), 255);
}

//------------------------------------------------------------------------------
// PART: items -- grenades / beacons / repair kit, top left.
//
// Dimmed at zero rather than hidden: a slot that disappears makes the row jump,
// and "I have none" is information too.
//------------------------------------------------------------------------------
function Vantage::Items(%x, %y, %w)
{
   Vantage::itemRow(%x, %y,      %w, "Grenade",    "GREN");
   Vantage::itemRow(%x, %y + 18, %w, "Beacon",     "BCN");
   Vantage::itemRow(%x, %y + 36, %w, "Repair Kit", "KIT");
}

function Vantage::itemRow(%x, %y, %w, %item, %label)
{
   %n = GetItemCount(%item);
   if(%n == "") %n = 0;

   if(%n > 0) { %c = $Vantage::Text;  %a = 235; }
   else       { %c = $Vantage::Dim;   %a = 160; }

   Vantage::textSmall(%x, %y, %w, %c, %label @ "  " @ %n, %a);
}

//------------------------------------------------------------------------------
// PART: clock -- the match clock, top centre under the flags.
//
// getHudTimer() is cg.clockTime, the continuously-advanced client clock the stock
// ClockHud draws (clockhud.cpp): negative while a timed match counts DOWN, so the
// magnitude is the time and the sign says which way it runs. The last minute of a
// countdown goes amber -- the one moment the clock is worth a glance.
//------------------------------------------------------------------------------
function Vantage::Clock(%x, %y, %w)
{
   %t = getHudTimer();
   if(%t == "") %t = 0;
   %down = (%t < 0);
   if(%down) %t = -%t;

   %whole = floor(%t);
   %h = floor(%whole / 3600);
   %m = floor((%whole - %h * 3600) / 60);
   %s = %whole - %h * 3600 - %m * 60;
   %str = Vantage::pad2(%m) @ ":" @ Vantage::pad2(%s);
   if(%h > 0)
      %str = %h @ ":" @ %str;

   if(%down && %whole < 60) { %c = $Vantage::Accent; %a = 255; }
   else                     { %c = $Vantage::Text;   %a = 215; }
   Vantage::text(%x, %y, %w, %c, "<jc>" @ %str, %a);
}

function Vantage::pad2(%n)
{
   return (%n < 10) ? "0" @ %n : %n;
}

//------------------------------------------------------------------------------
// KILL DATA -- one event layer feeding both the kill feed and the scoreboard.
//
// killPop.cpp matches every obituary line and fires
//    eventClientKilled(%killer, %victim, %weapon)       (killer == victim: suicide/world)
//    eventClientTeamKilled(%killer, %victim, %weapon)
// with client ids. Names and teams are captured AT THE EVENT, so a row still reads
// right after its player has left the server.
//
// ★Attached here, in components.cs, not in hud.cs.★ A borrowed part only gets this
// file (ModernHUD::drawSlot), so an attach in hud.cs would leave a borrowed feed
// empty forever. ModernHUD::attach is revoked on unload, and dedupes, so the base
// pack's own exec of this file cannot double-register.
//------------------------------------------------------------------------------
ModernHUD::attach("eventClientKilled",       "Vantage::onKill");
ModernHUD::attach("eventClientTeamKilled",   "Vantage::onTeamKill");
ModernHUD::attach("eventFlagCaptured",       "Vantage::onCapture");
ModernHUD::attach("eventConnectionAccepted", "Vantage::killReset");
ModernHUD::attach("eventChangeMission",      "Vantage::killReset");
ModernHUD::attach("eventClientJoin",         "Vantage::rosterDirty");
ModernHUD::attach("eventClientDrop",         "Vantage::rosterDirty");
ModernHUD::attach("eventClientChangeTeam",   "Vantage::rosterDirty");

$Vantage::KfMax = 5;          // feed rows
$Vantage::KfLife = 8000;      // ms a row stays up; the last second fades

function Vantage::killReset()
{
   deleteVariables("$Vantage::Kf*");
   deleteVariables("$Vantage::St*");
   $Vantage::KfMax = 5;
   $Vantage::KfLife = 8000;
   $Vantage::KfN = 0;
   Vantage::rosterDirty();
}

function Vantage::rosterDirty()
{
   $Vantage::SbDirty = 1;
}

function Vantage::onKill(%killer, %victim, %weapon)
{
   Vantage::killEvent(%killer, %victim, %weapon, false);
}

function Vantage::onTeamKill(%killer, %victim, %weapon)
{
   Vantage::killEvent(%killer, %victim, %weapon, true);
}

function Vantage::killEvent(%killer, %victim, %weapon, %tk)
{
   // -- the scoreboard's tallies. A death always counts; only a clean kill of
   // someone else credits the killer.
   $Vantage::StD[%victim]++;
   if(!%tk && %killer != %victim)
      $Vantage::StK[%killer]++;
   $Vantage::SbDirty = 1;

   // -- the feed: newest on top, older rows shift down, the oldest falls off.
   %n = $Vantage::KfN;
   if(%n == "") %n = 0;
   if(%n < $Vantage::KfMax)
      %n++;
   for(%i = %n - 1; %i > 0; %i--)
   {
      $Vantage::KfK[%i]  = $Vantage::KfK[%i - 1];
      $Vantage::KfKT[%i] = $Vantage::KfKT[%i - 1];
      $Vantage::KfV[%i]  = $Vantage::KfV[%i - 1];
      $Vantage::KfVT[%i] = $Vantage::KfVT[%i - 1];
      $Vantage::KfW[%i]  = $Vantage::KfW[%i - 1];
      $Vantage::KfMe[%i] = $Vantage::KfMe[%i - 1];
      $Vantage::KfAt[%i] = $Vantage::KfAt[%i - 1];
   }
   $Vantage::KfN = %n;

   %me = getManagerId();
   $Vantage::KfV[0]  = Client::GetName(%victim);
   $Vantage::KfVT[0] = Client::GetTeam(%victim);
   if(%killer == %victim)
   {
      $Vantage::KfK[0]  = "";
      $Vantage::KfKT[0] = "";
   }
   else
   {
      $Vantage::KfK[0]  = Client::GetName(%killer);
      $Vantage::KfKT[0] = Client::GetTeam(%killer);
   }
   $Vantage::KfW[0]  = %tk ? "TEAMKILL" : %weapon;
   $Vantage::KfMe[0] = (%killer == %me || %victim == %me);
   $Vantage::KfAt[0] = glTicks();
}

function Vantage::onCapture(%flagTeam, %cl)
{
   // Presto's (teamFlag, client): %cl carried the flag home.
   if(%cl != "" && %cl != 0)
      $Vantage::StC[%cl]++;
   $Vantage::SbDirty = 1;
}

// A player's name as a markup-safe string, clipped so a long name cannot wrap the
// row onto a second line (glDrawMarkup wraps at the width it is given).
function Vantage::name(%name, %max)
{
   if(String::len(%name) > %max)
      %name = String::getSubStr(%name, 0, %max - 1) @ ".";
   return String::escapeFormatting(%name);
}

// A team's colour from where the viewer stands: ours cyan, theirs red, anything
// else (observers, a player already gone) dim.
function Vantage::teamColor(%team)
{
   if(%team == "" || %team < 0)
      return $Vantage::Dim;
   return (%team == Team::Friendly()) ? $Vantage::Primary : $Vantage::Warn;
}

//------------------------------------------------------------------------------
// PART: killfeed -- the last five obituaries, top right under the minimap.
//
// Right-aligned, newest on top, each row fading over its last second. A row that
// involves YOU gets a bed behind it: in a busy fight the one line you care about
// is the one with your name in it.
//------------------------------------------------------------------------------
function Vantage::KillFeed(%x, %y, %w)
{
   // The Options preview and the K editor draw sample rows, so the part can be seen
   // and placed. Live rows are never touched by either.
   if($ModernHUD::PvDemo || ($Config::HudListVisible == 1 && ($Vantage::KfN == "" || $Vantage::KfN == 0)))
   {
      Vantage::feedDemo(%x, %y, %w);
      return;
   }

   %n = $Vantage::KfN;
   if(%n == "" || %n == 0)
      return;

   %now = glTicks();
   %row = 0;
   for(%i = 0; %i < %n; %i++)
   {
      %age = %now - $Vantage::KfAt[%i];
      if(%age < 0 || %age > $Vantage::KfLife)
         continue;                 // expired rows stay in the ring until pushed out
      %a = 255;
      if(%age > $Vantage::KfLife - 1000)
         %a = floor(255 * ($Vantage::KfLife - %age) / 1000);
      Vantage::feedRow(%x, %y + %row * 16, %w, $Vantage::KfK[%i], $Vantage::KfKT[%i],
                       $Vantage::KfV[%i], $Vantage::KfVT[%i], $Vantage::KfW[%i],
                       $Vantage::KfMe[%i], %a);
      %row++;
   }
}

// One row, laid out right-to-left: victim at the edge, weapon, then killer.
function Vantage::feedRow(%x, %y, %w, %k, %kt, %v, %vt, %weap, %mine, %a)
{
   %vName = Vantage::name(%v, 18);
   %kName = Vantage::name(%k, 18);
   %weapTag = "[" @ %weap @ "]";

   if(%mine)
   {
      Vantage::color($Vantage::Dim, floor(%a * 0.6));
      glRectangle(%x, %y - 1, %w, 15);
   }

   %wc = (%weap == "TEAMKILL") ? $Vantage::Accent : $Vantage::Text;

   // Three runs in one markup string, so the engine measures and right-aligns the
   // line as a whole -- no per-font width tables in script.
   %line = "<jr>";
   if(%kName != "")
      %line = %line @ Vantage::run(Vantage::teamColor(%kt), %kName) @ "  ";
   %line = %line @ Vantage::run(%wc, %weapTag) @ "  " @ Vantage::run(Vantage::teamColor(%vt), %vName);
   ModernHUD::markup(%x, %y, %w - 4, %line, %a);
}

// A self-contained coloured text run (see Vantage::text for why not <fN>).
function Vantage::run(%rgb, %str)
{
   return "<f:sf_white_7.pft:" @ Vantage::hex(%rgb) @ "ff:000000c0:1,1>" @ %str;
}

function Vantage::feedDemo(%x, %y, %w)
{
   %me = getManagerId();
   %mine = Team::Friendly();
   %name = Client::GetName(%me);
   if(%name == "") %name = "You";
   Vantage::feedRow(%x, %y,      %w, %name,        %mine,     "Enemy Heavy", %mine ^ 1, "Disc",     true,  255);
   Vantage::feedRow(%x, %y + 16, %w, "Enemy Heavy", %mine ^ 1, "Teammate",    %mine,     "Chaingun", false, 235);
   Vantage::feedRow(%x, %y + 32, %w, "Teammate",    %mine,     "Enemy Light", %mine ^ 1, "Mortar",   false, 200);
}

//------------------------------------------------------------------------------
// PART: scoreboard -- both teams at a glance, bottom right above the weapon.
//
// Per team: a header (name, caps, head count) and its top four by kills, with
// K / D / C (flag captures). You are always on it: if you are not in your team's
// top four you replace its fourth row.
//
// ★The tallies are what THIS client has seen.★ The engine has no structured score
// on the wire -- the server's scoreboard is a text template -- so kills, deaths and
// captures are counted from the same obituary and flag events the feed uses, from
// the moment you joined, and reset on every map. The stock Tab scoreboard is still
// the server's own word.
//
// Rebuilt at most once a second (or on a roster/kill event), not per frame: a sort
// over every client in script, every frame, is exactly the tree-walk cost that
// once made the K menu stutter.
//------------------------------------------------------------------------------
$Vantage::SbRows = 4;

function Vantage::Scoreboard(%x, %y, %w)
{
   %now = glTicks();
   if($Vantage::SbDirty || $ModernHUD::PvDemo || %now - $Vantage::SbAt > 1000 || %now < $Vantage::SbAt)
   {
      Vantage::sbBuild();
      $Vantage::SbAt = %now;
      $Vantage::SbDirty = "";
   }

   %mine = Team::Friendly();
   // Bed + column titles.
   Vantage::color($Vantage::Dim, 70);
   glRectangle(%x, %y, %w, 162);
   Vantage::textSmall(%x + %w - 96, %y + 2, 32, $Vantage::Dim, "<jr>K", 220);
   Vantage::textSmall(%x + %w - 64, %y + 2, 32, $Vantage::Dim, "<jr>D", 220);
   Vantage::textSmall(%x + %w - 32, %y + 2, 28, $Vantage::Dim, "<jr>C", 220);

   Vantage::sbTeam(%x, %y + 12, %w, %mine);
   Vantage::sbTeam(%x, %y + 90, %w, %mine ^ 1);
}

function Vantage::sbTeam(%x, %y, %w, %team)
{
   %rgb = (%team == Team::Friendly()) ? $Vantage::Primary : $Vantage::Warn;

   // Team edge bar, then the header: name left, caps and head count right.
   Vantage::color(%rgb, 220);
   glRectangle(%x, %y, 2, 72);

   %tname = $Team::Name[%team];
   if(%tname == "" && isFunction("Team::GetName"))
      %tname = Team::GetName(%team);
   if(%tname == "")
      %tname = (%team == Team::Friendly()) ? "Your team" : "Enemy";
   %caps = Team::Score(%team);
   if(%caps == "") %caps = 0;
   %count = $Vantage::SbCount[%team];
   if(%count == "") %count = 0;

   Vantage::text(%x + 8, %y, %w - 90, %rgb, Vantage::name(%tname, 20), 245);
   Vantage::textSmall(%x + %w - 90, %y + 3, 86, $Vantage::Text,
      "<jr>" @ %caps @ " caps  " @ %count @ "p", 200);

   %n = $Vantage::SbN[%team];
   if(%n == "") %n = 0;
   for(%i = 0; %i < %n; %i++)
   {
      %cl = $Vantage::SbCl[%team, %i];
      %ry = %y + 16 + %i * 14;
      %isMe = (%cl == getManagerId());
      if(%isMe)
      {
         Vantage::color($Vantage::Dim, 150);
         glRectangle(%x + 2, %ry - 1, %w - 2, 14);
         %c = $Vantage::Text; %a = 255;
      }
      else
      {
         %c = %rgb; %a = 215;
      }
      %k = $Vantage::StK[%cl]; if(%k == "") %k = 0;
      %d = $Vantage::StD[%cl]; if(%d == "") %d = 0;
      %cp = $Vantage::StC[%cl]; if(%cp == "") %cp = 0;
      Vantage::textSmall(%x + 8, %ry, %w - 110, %c, Vantage::name(Client::GetName(%cl), 20), %a);
      Vantage::textSmall(%x + %w - 96, %ry, 32, %c, "<jr>" @ %k, %a);
      Vantage::textSmall(%x + %w - 64, %ry, 32, %c, "<jr>" @ %d, %a);
      Vantage::textSmall(%x + %w - 32, %ry, 28, %c, "<jr>" @ %cp, %a);
   }
}

// Rebuild $Vantage::SbCl[team, i] (top rows by kills, then fewest deaths) and
// $Vantage::SbCount[team] from the live roster.
function Vantage::sbBuild()
{
   for(%t = 0; %t < 2; %t++)
   {
      $Vantage::SbN[%t] = 0;
      $Vantage::SbCount[%t] = 0;
   }

   if($ModernHUD::PvDemo)
   {
      // The preview's three fixture clients (Framework.cs previewBegin), with tallies
      // that only exist inside this branch.
      Vantage::sbDemo(0, 9001, 4, 1, 1);
      Vantage::sbDemo(0, 9003, 2, 3, 0);
      Vantage::sbDemo(1, 9002, 5, 2, 0);
      return;
   }

   %me = getManagerId();
   %myTeam = Client::GetTeam(%me);
   %cl = Client::getFirst();
   for(%guard = 0; %guard < 128 && %cl != -1 && %cl != ""; %guard++)
   {
      %t = Client::GetTeam(%cl);
      if(%t == 0 || %t == 1)
      {
         $Vantage::SbCount[%t]++;
         Vantage::sbInsert(%t, %cl);
      }
      %cl = Client::getNext(%cl);
   }

   // You are always on your team's board.
   if(%myTeam == 0 || %myTeam == 1)
   {
      %found = false;
      for(%i = 0; %i < $Vantage::SbN[%myTeam]; %i++)
         if($Vantage::SbCl[%myTeam, %i] == %me)
            %found = true;
      if(!%found && $Vantage::SbN[%myTeam] > 0)
         $Vantage::SbCl[%myTeam, $Vantage::SbN[%myTeam] - 1] = %me;
   }
}

// Insertion into a team's top-N, ordered by kills desc then deaths asc.
function Vantage::sbInsert(%t, %cl)
{
   %n = $Vantage::SbN[%t];
   %pos = %n;
   for(%i = 0; %i < %n; %i++)
   {
      if(Vantage::sbBetter(%cl, $Vantage::SbCl[%t, %i]))
      {
         %pos = %i;
         break;
      }
   }
   if(%pos >= $Vantage::SbRows)
      return;
   if(%n < $Vantage::SbRows)
      %n++;
   for(%i = %n - 1; %i > %pos; %i--)
      $Vantage::SbCl[%t, %i] = $Vantage::SbCl[%t, %i - 1];
   $Vantage::SbCl[%t, %pos] = %cl;
   $Vantage::SbN[%t] = %n;
}

function Vantage::sbBetter(%a, %b)
{
   %ka = $Vantage::StK[%a]; if(%ka == "") %ka = 0;
   %kb = $Vantage::StK[%b]; if(%kb == "") %kb = 0;
   if(%ka != %kb)
      return %ka > %kb;
   %da = $Vantage::StD[%a]; if(%da == "") %da = 0;
   %db = $Vantage::StD[%b]; if(%db == "") %db = 0;
   return %da < %db;
}

function Vantage::sbDemo(%t, %cl, %k, %d, %c)
{
   // Keyed by the fixture ids (9001+), which no live client has, so the numbers can
   // never show on a real row; the next map's killReset drops them anyway.
   $Vantage::StK[%cl] = %k;
   $Vantage::StD[%cl] = %d;
   $Vantage::StC[%cl] = %c;
   $Vantage::SbCl[%t, $Vantage::SbN[%t]] = %cl;
   $Vantage::SbN[%t]++;
   $Vantage::SbCount[%t]++;
}

function Vantage::restore()
{
   if($Vantage::Saved == "")
   {
      echo("Vantage: nothing to restore.");
      return;
   }

   $pref::Hud::ColorPrimary = $Vantage::Sav::ColorPrimary;
   $pref::Hud::ColorDim     = $Vantage::Sav::ColorDim;
   $pref::Hud::ColorAccent  = $Vantage::Sav::ColorAccent;
   $pref::Hud::ColorWarn    = $Vantage::Sav::ColorWarn;
   $pref::Hud::ColorText    = $Vantage::Sav::ColorText;
   $pref::Hud::ColorPass    = $Vantage::Sav::ColorPass;

   $mj::shownames        = $Vantage::Sav::ShowNames;
   $mj::showhpbars       = $Vantage::Sav::ShowHpBars;
   $mj::showjetbars      = $Vantage::Sav::ShowJetBars;
   $mj::showhptext       = $Vantage::Sav::ShowHpText;
   $mj::barscrouch       = $Vantage::Sav::BarsCrouch;
   $mj::bar_width        = $Vantage::Sav::BarW;
   $mj::bar_height       = $Vantage::Sav::BarH;
   $mj::bar_border_width = $Vantage::Sav::BarB;
   $mj::fontdefault      = $Vantage::Sav::FontDefault;
   $mj::fontpass         = $Vantage::Sav::FontPass;
   $mj::passhelper       = $Vantage::Sav::PassHelper;
   $mj::passhelpermm     = $Vantage::Sav::PassHelperMM;

   $xChat::HiderEnabled  = $Vantage::Sav::HiderEnabled;
   $xChat::HiderTimeout  = $Vantage::Sav::HiderTimeout;
   $xChat::ScrollTimeout = $Vantage::Sav::ScrollTimeout;
   $xChat::HideCmdMsg    = $Vantage::Sav::HideCmdMsg;
   $xChat::TransChat     = $Vantage::Sav::TransChat;

   $pref::ChatDisplayModMethodX = $Vantage::Sav::ChatModX;
   $pref::ChatDisplayX          = $Vantage::Sav::ChatX;
   $pref::ChatDisplayWidth      = $Vantage::Sav::ChatWidth;

   deleteVariables("$Vantage::Sav::*");
   $Vantage::Saved = "";
   echo("Vantage: client settings restored.");
}

function Vantage::draw_vitals(%screen)
{
   %partW = 260;
   %at = ModernHUD::part("ModernHUD::VantageVitals", "bottom-center", 0, 96, 260, 22, %screen);
   Vantage::Vitals(getWord(%at, 0), getWord(%at, 1), %partW);
}

function Vantage::draw_weapon(%screen)
{
   %partW = 220;
   %at = ModernHUD::part("ModernHUD::VantageWeapon", "bottom-right", 18, 96, 220, 34, %screen);
   Vantage::Weapon(getWord(%at, 0), getWord(%at, 1), %partW);
}

function Vantage::draw_ctf(%screen)
{
   %partW = 420;
   %at = ModernHUD::part("ModernHUD::VantageCtf", "top-center", 0, 14, 420, 34, %screen);
   Vantage::Ctf(getWord(%at, 0), getWord(%at, 1), %partW);
}

function Vantage::draw_items(%screen)
{
   %partW = 150;
   %at = ModernHUD::part("ModernHUD::VantageItems", "top-left", 18, 14, 150, 56, %screen);
   Vantage::Items(getWord(%at, 0), getWord(%at, 1), %partW);
}

function Vantage::draw_clock(%screen)
{
   %partW = 120;
   %at = ModernHUD::part("ModernHUD::VantageClock", "top-center", 0, 52, 120, 16, %screen);
   Vantage::Clock(getWord(%at, 0), getWord(%at, 1), %partW);
}

function Vantage::draw_killfeed(%screen)
{
   %partW = 300;
   %at = ModernHUD::part("ModernHUD::VantageKillFeed", "top-right", 18, 206, 300, 80, %screen);
   Vantage::KillFeed(getWord(%at, 0), getWord(%at, 1), %partW);
}

function Vantage::draw_scoreboard(%screen)
{
   %partW = 240;
   %at = ModernHUD::part("ModernHUD::VantageScoreboard", "bottom-right", 18, 150, 240, 162, %screen);
   Vantage::Scoreboard(getWord(%at, 0), getWord(%at, 1), %partW);
}

//------------------------------------------------------------------------------
// Components: one per slot, each drawing that slot's parts (generated).
//------------------------------------------------------------------------------
function Vantage::compReady()
{
   if(isFunction("Vantage::compPrep"))
      Vantage::compPrep();
}

function Vantage::comp_healthenergy(%screen)
{
   Vantage::compReady();
   Vantage::draw_vitals(%screen);
}

function Vantage::comp_weapon(%screen)
{
   Vantage::compReady();
   Vantage::draw_weapon(%screen);
}

function Vantage::comp_ctf(%screen)
{
   Vantage::compReady();
   Vantage::draw_ctf(%screen);
}

function Vantage::comp_items(%screen)
{
   Vantage::compReady();
   Vantage::draw_items(%screen);
}

ModernHUD::component("vantage", "healthenergy", "healthenergy", "Vantage::comp_healthenergy");
ModernHUD::component("vantage", "weapon", "weapon", "Vantage::comp_weapon");
ModernHUD::component("vantage", "ctf", "ctf", "Vantage::comp_ctf");
ModernHUD::component("vantage", "items", "items", "Vantage::comp_items");

function Vantage::comp_clock(%screen)
{
   Vantage::compReady();
   Vantage::draw_clock(%screen);
}

function Vantage::comp_killfeed(%screen)
{
   Vantage::compReady();
   Vantage::draw_killfeed(%screen);
}

function Vantage::comp_scoreboard(%screen)
{
   Vantage::compReady();
   Vantage::draw_scoreboard(%screen);
}

ModernHUD::component("vantage", "clock", "clock", "Vantage::comp_clock");
ModernHUD::component("vantage", "killfeed", "killfeed", "Vantage::comp_killfeed");
ModernHUD::component("vantage", "scoreboard", "scoreboard", "Vantage::comp_scoreboard");
