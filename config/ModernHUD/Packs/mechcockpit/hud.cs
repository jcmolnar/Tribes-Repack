//------------------------------------------------------------------------------
// Mech Cockpit -- ModernHUD pack for Mech Mayhem (hand-authored).
//
// Vector-drawn (glRectangle + TrueType via glSetFont/glDrawString -- the
// vantage/vector approach, zero art assets). Draws ONLY while the server says
// this client is piloting a Herc; otherwise every slot is declined and the
// pack is invisible -- so it is safe to leave selected on non-mech servers.
//
// State channel: the Mech Mayhem server remoteEvals MMState once a second
// (see Mods\MechMayhem\scripts\MechGame.cs). remoteMMState below stores it in
// $MMC::* globals; staleness beyond 3s deactivates the cockpit (server without
// the mod = instant graceful degrade).
//------------------------------------------------------------------------------

exec("ModernHUD/Framework.cs");

$ModernHUD::Enabled = true;
$ModernHUD::Pack = "MechCockpit";
$ModernHUD::PackId = "mechcockpit";

// Cockpit typeface: this pack's HUD text (incl. the objectives page, which
// resolves through the HUD-set chain) defaults to the Terminal font set --
// the only shipped set covering all ten objectives slots. GUARDED: a pilot's
// own per-config pick (Options > Configs > Font Set) or a saved pref wins.
if($pref::ModernHUD::FontSet::mechcockpit == "")
   $pref::ModernHUD::FontSet::mechcockpit = "Terminal";

//--- state channel ------------------------------------------------------------
// server: remoteEval(%cl, MMState, heatPct, shield, shieldMax, legs, guns,
//                    sens, rctr, chassis, cv, tickets0, tickets1, shutdown);
function remoteMMState(%server, %heatPct, %shield, %shieldMax, %legs, %guns,
                       %sens, %rctr, %chassis, %cv, %t0, %t1, %down,
                       %kills, %deaths, %wave, %salv)
{
   $MMC::heat = %heatPct;
   $MMC::shield = %shield;
   $MMC::shieldMax = %shieldMax;
   $MMC::legs = %legs;
   $MMC::guns = %guns;
   $MMC::sens = %sens;
   $MMC::rctr = %rctr;
   $MMC::chassis = %chassis;
   $MMC::cv = %cv;
   $MMC::t0 = %t0;
   $MMC::t1 = %t1;
   $MMC::down = %down;
   $MMC::kills = %kills;
   $MMC::deaths = %deaths;
   $MMC::wave = %wave;
   $MMC::salv = %salv;
   $MMC::stamp = getSimTime();
}

// shield broadcast: once a second the server sends every living mech as
// "<clientId> <frac>" pairs in one string. Stored raw -- BOTH consumers parse
// it engine-side (ShieldFx bubbles + the $mj nameplate shield bars), so script
// never loops over it. Cleared when the cockpit deactivates (see draw below).
function remoteMMShields(%server, %str)
{
   $MMC::shields = %str;
}

// rack panel: pushed once per loadout grant (spawn / chassis change). All mech
// weapons are REACTOR-FED -- heat is the ammo, so there is no count to show;
// the selected entry is resolved client-side from getMountedItem(0).
function remoteMMRack(%server, %n, %w0, %w1, %w2, %w3, %w4, %w5)
{
   $MMC::rackN = %n;
   $MMC::rack[0] = %w0;
   $MMC::rack[1] = %w1;
   $MMC::rack[2] = %w2;
   $MMC::rack[3] = %w3;
   $MMC::rack[4] = %w4;
   $MMC::rack[5] = %w5;
}

// nearby reactor events rock the camera: the native decay lives in
// Player::getCameraTransform, reading this client-side amplitude
function remoteMMShake(%server, %amp)
{
   if (%amp > $MM::camShake)
      $MM::camShake = %amp;
}

function MechCockpit::active()
{
   if ($MMC::stamp == "")
      return false;
   return (getSimTime() - $MMC::stamp) < 3;
}

//--- drawing helpers ----------------------------------------------------------

function MechCockpit::hex2(%v)
{
   %v = floor(%v);
   if (%v < 0) %v = 0;
   if (%v > 255) %v = 255;
   %hi = floor(%v / 16);
   return MechCockpit::nib(%hi) @ MechCockpit::nib(%v - %hi * 16);
}

function MechCockpit::nib(%n)
{
   %n = floor(%n);
   if (%n <= 9) return %n;
   if (%n == 10) return "a";
   if (%n == 11) return "b";
   if (%n == 12) return "c";
   if (%n == 13) return "d";
   if (%n == 14) return "e";
   return "f";
}

function MechCockpit::text(%x, %y, %rgb, %str, %px)
{
   glSetFont("Verdana", %px);
   %tag = "<" @ MechCockpit::hex2(getWord(%rgb, 0)) @ MechCockpit::hex2(getWord(%rgb, 1))
        @ MechCockpit::hex2(getWord(%rgb, 2)) @ "ff>";
   glDrawString(%x + 1, %y + 1, "<000000a0>" @ %str);
   glDrawString(%x, %y, %tag @ %str);
}

// bar with border: %frac 0..1 of %rgb fill
function MechCockpit::bar(%x, %y, %w, %h, %frac, %rgb)
{
   if (%frac < 0) %frac = 0;
   if (%frac > 1) %frac = 1;
   glColor4ub(20, 26, 30, 190);
   glRectangle(%x, %y, %w, %h);
   glColor4ub(getWord(%rgb, 0), getWord(%rgb, 1), getWord(%rgb, 2), 235);
   glRectangle(%x + 2, %y + 2, floor((%w - 4) * %frac), %h - 4);
}

// component lamp: label + state colour (0 ok, 1 degraded, 2 out)
function MechCockpit::lamp(%x, %y, %label, %state)
{
   if (%state == 2)      { %r = 255; %g = 60;  %b = 40; }
   else if (%state == 1) { %r = 255; %g = 200; %b = 40; }
   else                  { %r = 70;  %g = 220; %b = 90; }
   glColor4ub(%r, %g, %b, 220);
   glRectangle(%x, %y, 14, 14);
   MechCockpit::text(%x + 20, %y, "200 210 215", %label, 12);
}

//--- the frame ---------------------------------------------------------------

function ModernHUDPack::draw(%screen)
{
   if (!MechCockpit::active()) {
      // leaving the cockpit: put the player's own nameplate knobs back and
      // stop feeding the engine shield consumers (stale pairs would keep
      // bubbles alive on a server that no longer broadcasts).
      if ($MMC::mjOn == 1) {
         $mj::shownames = $MMC::mjSaveNames;
         $mj::showhpbars = $MMC::mjSaveHp;
         $mj::showshieldbars = "";
         $mj::eyesight = $MMC::mjSaveEye;
         $MMC::shields = "";
         $MMC::mjOn = 0;
      }
      return;
   }
   if ($MMC::mjOn != 1) {
      // entering the cockpit: save whatever the player's config set, then turn
      // on names + hull + shield bars over visible mechs (engine nameplates,
      // fearGuiCrosshair.cpp -- sensor- and LOS-gated there, not a wallhack).
      $MMC::mjSaveNames = $mj::shownames;
      $MMC::mjSaveHp = $mj::showhpbars;
      $MMC::mjSaveEye = $mj::eyesight;
      $mj::shownames = true;
      $mj::showhpbars = true;
      $mj::showshieldbars = true;
      $mj::eyesight = true;
      $MMC::mjOn = 1;
   }

   %w = getWord(%screen, 0);
   %h = getWord(%screen, 1);

   // ---- bottom-center: heat + shield + hull stack ----
   %bw = floor(%w * 0.30);
   %bx = floor((%w - %bw) / 2);
   %by = %h - 88;

   // heat (inverted energy): redline past 85%
   %heat = $MMC::heat;
   if (%heat > 0.85 || $MMC::down == 1)
      %heatCol = "255 60 40";
   else if (%heat > 0.6)
      %heatCol = "255 170 40";
   else
      %heatCol = "80 200 255";
   MechCockpit::bar(%bx, %by, %bw, 16, %heat, %heatCol);
   MechCockpit::text(%bx - 52, %by + 1, "200 210 215", "HEAT", 12);

   // shield
   %sfrac = 0;
   if ($MMC::shieldMax > 0)
      %sfrac = $MMC::shield / $MMC::shieldMax;
   MechCockpit::bar(%bx, %by + 20, %bw, 12, %sfrac, "120 160 255");
   MechCockpit::text(%bx - 52, %by + 19, "200 210 215", "SHLD", 12);

   // hull ($health is the stock 0..100 client global)
   MechCockpit::bar(%bx, %by + 36, %bw, 12, $health / 100, "120 230 120");
   MechCockpit::text(%bx - 52, %by + 35, "200 210 215", "HULL", 12);

   // shutdown banner
   if ($MMC::down == 1) {
      glSetFont("Verdana", 30);
      %msg = "REACTOR OFFLINE";
      %mw = getWord(glGetStringDimensions(%msg), 0);
      glDrawString(floor((%w - %mw) / 2) + 2, floor(%h * 0.4) + 2, "<000000c0>" @ %msg);
      glDrawString(floor((%w - %mw) / 2), floor(%h * 0.4), "<ff3c28ff>" @ %msg);
   }
   else if (%heat > 0.85) {
      glSetFont("Verdana", 20);
      %msg = "HEAT CRITICAL";
      %mw = getWord(glGetStringDimensions(%msg), 0);
      glDrawString(floor((%w - %mw) / 2), %by - 26, "<ff9628ff>" @ %msg);
   }

   // ---- right panel: component lamps ----
   %px = %w - 170;
   %py = %h - 190;
   MechCockpit::text(%px, %py - 20, "140 170 190", $MMC::chassis, 13);
   MechCockpit::lamp(%px, %py,      "LEGS", $MMC::legs);
   MechCockpit::lamp(%px, %py + 22, "GUNS", $MMC::guns);
   MechCockpit::lamp(%px, %py + 44, "SENS", $MMC::sens);
   MechCockpit::lamp(%px, %py + 66, "RCTR", $MMC::rctr);

   // ---- top-center: CV ticket pools (escalation) or wave status (incursion) ----
   glSetFont("Verdana", 16);
   if ($MMC::wave != "")
      %tmsg = "WAVE " @ $MMC::wave @ "   SALVAGE " @ $MMC::salv;
   else
      %tmsg = $MMC::t0 @ "  CV  " @ $MMC::t1;
   %tw = getWord(glGetStringDimensions(%tmsg), 0);
   %tx = floor((%w - %tw) / 2);
   glDrawString(%tx + 1, 13, "<000000a0>" @ %tmsg);
   glDrawString(%tx, 12, "<9adcffff>" @ %tmsg);

   // ---- top-right: personal score ----
   %smsg = "K " @ ($MMC::kills != "" ? $MMC::kills : 0)
         @ "  D " @ ($MMC::deaths != "" ? $MMC::deaths : 0);
   %sw = getWord(glGetStringDimensions(%smsg), 0);
   glDrawString(%w - %sw - 15, 13, "<000000a0>" @ %smsg);
   glDrawString(%w - %sw - 16, 12, "<c8d2d7ff>" @ %smsg);

   // ---- bottom-left: weapon rack ----
   // Chained fire means every listed weapon IS live on its pod; the highlighted
   // row is the slot-0 weapon (what next/prev-weapon cycles). Reactor-fed: no
   // ammo counts exist -- the HEAT bar is the ammo gauge.
   if ($MMC::rackN != "" && $MMC::rackN > 0) {
      %sel = getItemDesc(getMountedItem(0));
      %ry = %h - 88 - ($MMC::rackN * 17);
      MechCockpit::text(20, %ry - 20, "140 170 190", "RACK", 13);
      for (%i = 0; %i < $MMC::rackN && %i < 6; %i++) {
         %nm = $MMC::rack[%i];
         if (%nm == "")
            continue;
         // selected = the mounted description starts with the rack name
         // (descriptions are "<name> [<size>]")
         if (%sel != "" && String::findSubStr(%sel, %nm) == 0)
            MechCockpit::text(20, %ry, "120 220 255", "> " @ %nm, 13);
         else
            MechCockpit::text(20, %ry, "170 180 185", "  " @ %nm, 13);
         %ry = %ry + 17;
      }
   }
}

//--- pack contract -----------------------------------------------------------

function ModernHUDPack::ownsSlot(%value)
{
   // own the health/energy cluster only while the cockpit is live; every
   // other slot stays with whatever pack/module the player chose
   if (!MechCockpit::active())
      return false;
   if (%value == "")
      return true;
   if (%value == "off")
      return false;
   return String::findSubStr(%value, "MechCockpit::") == 0;
}

function ModernHUDPack::detachRetained()
{
}

function ModernHUDPack::prefs()
{
}

function ModernHUDPack::stockHuds()
{
   // hide the stock health readout while the cockpit is live; everything else
   // untouched (visibility only -- see AUTHORING.md)
   if (MechCockpit::active())
      ModernHUD::stock("healthHud", false);
   else
      ModernHUD::stock("healthHud", true);
}

function ModernHUDPack::init()
{
}

ModernHUD::attach("eventGuiOpen_PlayGui", "ModernHUDPack::stockHuds");

// Third-person chase camera pushback while piloting a mech. The engine reads
// $pref::hercCamScale EVERY FRAME (player.cpp getCameraTransform) and multiplies
// the stock 4-unit chase distance by it; left unset it auto-computes
// radius/2.3 (~4 for a herc), which parks the camera inside the mech's own
// silhouette. Live: dragging the slider moves the camera immediately.
ModernHUD::setting("int", "pref::hercCamScale", "Mech chase camera distance", "8",
   "3|14|1", "");

$ModernHUD::LoadComplete = "mechcockpit";
echo("[MECHHUD] Mech Cockpit pack loaded.");
