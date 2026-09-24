//==============================================================================
// ProConfigVol4-1.41 -- BORROWABLE PARTS (components.cs)
//
// Split out of hud.cs by tools/modernhud_components.py so another pack can draw these
// parts: the HUD designer's Provider choice sets $pref::HudSlot::<slot> to
// "proconfig/<slot>", and ModernHUD::drawSlot execs this file on demand. It must define only
// proconfig:: names -- ModernHUDPack:: belongs to whichever pack is the base.
// hud.cs execs this file too, so the pack's own draw uses the same code.
//==============================================================================

ModernHUD::require("ModernHUD/Core/Data/proconfig.Team.cs");
ModernHUD::require("ModernHUD/Core/Data/Timer.cs");


// ---- helpers carried from the legacy pack -------------------------
// A lifted body calls these; the converted pack does not execute the
// legacy module, so they have to come along or the call resolves to
// nothing and the part renders wrong without erroring.
//
// ★Prefixed with the pack id.★ Five packs define CTFHUD::Update; the
// console has one namespace and a definition outlives the pack that made
// it, so under the original names a leftover handler from a pack that is
// no longer loaded would call OUR body. The originals are recorded below
// each definition.
// from Modules/AmmoHud/AmmoHUD.acs.cs  (originally AmmoHUD::Update)
function proconfig::AmmoHUD::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;

   if($Weapon::Ammo < 0)
       %ammo = "~";
   else
       %ammo = $Weapon::Ammo;

       if(GetItemdesc(GetMountedItem(0)) == "Disc Launcher")
       if(GetItemdesc(GetMountedItem(0)) == "Grenade Launcher")
       if(GetItemdesc(GetMountedItem(0)) == "Chaingun")

   ModernHUD::markup(%x + 3, %y + 0, 222, "<f2>"~%ammo, 255);
}

// from Modules/CTFHud/CTFHud.acs.cs  (originally CtfHUD::EnemyTeamValue)
function proconfig::CtfHUD::EnemyTeamValue(%team, %score1)
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
       %team = Team::Enemy();
   %loc = Team::Flag::Location(Team::Enemy());

   switch ( %loc ) {
       case "home":
           %loc = "<f0> - Home -";
           break;
       case "field":
           %loc = "<f0>- Dropped -> <f2>" ~ Team::Flag::Timer(%team);
           break;
       default:
           %loc = "<f1>" ~ String::escapeFormatting(Client::GetName(%loc));
           break;
   }
   ModernHUD::markup(%x + 400, %y + 2, 150, %loc, 255);
}

// from Modules/CTFHud/CTFHud.acs.cs  (originally CtfHUD::FriendlyTeamValue)
function proconfig::CtfHUD::FriendlyTeamValue(%team, %score0)
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
       %team = Team::Friendly();
   %loc = Team::Flag::Location(Team::Friendly());

   switch ( %loc ) {
       case "home":
           %loc = "<f1> - Home -";
           break;
       case "field":
           %loc = "<f1>- Dropped -> <f2>" ~ Team::Flag::Timer(%team);
           break;
       default:
           %loc = "<f0>" ~ String::escapeFormatting(Client::GetName(%loc));
           break;
   }
   ModernHUD::markup(%x + 2, %y + 2, 150, %loc, 255);
}

// from Modules/CTFHud/CTFHud.acs.cs  (originally CtfHUD::Update)
function proconfig::CtfHUD::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
       %score0 = Team::Score(Team::Friendly());
       %score1 = Team::Score(Team::Enemy());
   proconfig::CtfHUD::FriendlyTeamValue(%team);
   proconfig::CtfHUD::EnemyTeamValue(%team);
   ModernHUD::markup(%x + 195, %y + 2, 505, "<f2>[ <f1>" ~%score0 @ "<f2> ]", 255);
   ModernHUD::markup(%x + 335, %y + 2, 365, "<f2>[ <f0>" ~%score1 @ "<f2> ]", 255);
}

// from Modules/HealthNrg/Energy.acs.cs  (originally GEnergy::Update)
function proconfig::GEnergy::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
   ModernHUD::markup(%x + 3, %y + 0, 222, "<f2>" ~ $energy, 255);
}

// from Modules/HealthNrg/Health.acs.cs  (originally GHealth::Update)
function proconfig::GHealth::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
   ModernHUD::markup(%x + 3, %y + 0, 222, "<f2>" ~ $health, 255);

}

// from Modules/HealthNrg/Speed.acs.cs  (originally GSpeed::Update)
function proconfig::GSpeed::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
   ModernHUD::markup(%x + 3, %y + 0, 222, "<f2>" ~ $Speed, 255);
}

// from Modules/KillHUD/killhud.acs.cs  (originally killHUD::Clear)
function proconfig::killHUD::Clear()
{
   for(%i=0;%i<=$killHUD::Lines;%i++)
       $killHUD::Row[%i] = "";

   proconfig::killHUD::Update();
}

// from Modules/KillHUD/killhud.acs.cs  (originally killHUD::getname)
function proconfig::killHUD::getname(%damageType)
{
   if (%damagetype == "Blaster") { return "[BLASTER]"; }
   if (%damagetype == "Chaingun") { return "[CHAINGUN]"; }
   if (%damagetype == "Disc") { return "[DISC]"; }
   if (%damagetype == "ELF") { return "[ELF]"; }
   if (%damagetype == "Explosives") { return "[EXPLO]"; }
   if (%damagetype == "Explosive") { return "[EXPLO]"; }
   if (%damagetype == "Laser") { return "[LASER]"; }
   if (%damagetype == "Mortar") { return "[MORTAR]"; }
   if (%damagetype == "Plasma") { return "[PLASMA]"; }
   if (%damagetype == "Suicide") { return "[SUICIDE]"; }
   if (%damagetype == "Turret") { return "[TURRET]"; }
   return "[SUICIDE]";
}

// from Modules/KillHUD/killhud.acs.cs  (originally killHUD::onClientKilled)
function proconfig::killHUD::onClientKilled(%killer, %victim, %damagetype)
{
                 %kname = String::Escape(Client::GetName(%killer));
   %vname = String::Escape(Client::GetName(%victim));
                 %font3 = "<f2>";
   if(Client::GetTeam(%killer) == Team::Friendly())
       %font = "<f1>";
   else
       %font = "<f0>";

   if(Client::GetTeam(%victim) == Team::Friendly())
       %font2 = "<f1>";
   else
       %font2 = "<f0>";

   proconfig::killHUD::Rotate();
   if((%killer == %victim) || (%kname == ""))
       $killHUD::Row[$killHUD::Lines] = %font@%vname@"<f1> : "@proconfig::killHUD::getname(%damageType);
   else
       $killHUD::Row[$killHUD::Lines] = %font@%kname@" "@%font3@proconfig::killHUD::getname(%damageType)@" "@%font2@%vname;
               proconfig::killHUD::Update();
}

// from Modules/KillHUD/killhud.acs.cs  (originally killHUD::onClientTeamKilled)
function proconfig::killHUD::onClientTeamKilled(%killer, %victim, %damagetype)
{
                 %kname = String::Escape(Client::GetName(%killer));
   %vname = String::Escape(Client::GetName(%victim));
                 %font3 = "<f2>";
   if(Client::GetTeam(%killer) == Team::Friendly())
       %font = "<f1>";
   else
       %font = "<f0>";

   if(Client::GetTeam(%victim) == Team::Friendly())
       %font2 = "<f1>";
   else
       %font2 = "<f0>";

   proconfig::killHUD::Rotate();
   if((%killer == %victim) || (%kname == ""))
       $killHUD::Row[$killHUD::Lines] = %font@%vname@"<f1> : "@"[TEAMKILLED]";
   else
       $killHUD::Row[$killHUD::Lines] = %font@%kname@" "@%font3@"[TEAMKILLED]"@" "@%font2@%vname;
               proconfig::killHUD::Update();
}

// from Modules/KillHUD/killhud.acs.cs  (originally killHUD::Rotate)
function proconfig::killHUD::Rotate()
{
   for(%i=0;%i<$killHUD::Lines;%i++)
       $killHUD::Row[%i] = $killHUD::Row[%i+1];
}

// from Modules/KillHUD/killhud.acs.cs  (originally killHUD::Update)
function proconfig::killHUD::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
   for(%i=0;%i<=$killHUD::Lines;%i++)
       %display = %display @ $killHUD::Row[%i] @ "\n";

   ModernHUD::markup(%x + 15, %y + -1, 500, %display, 255);
}

// Constants the legacy module's Init assigned and its Update reads.
function proconfig::compInit()
{
   $AmmoHUD::Awake = true;
}

function proconfig::draw_AmmoHUD_Container(%screen)
{
   %partW = 225;
   %at = ModernHUD::part("ModernHUD::AmmoHUD_Container", "bottom-right", 0, 25, 225, 15, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   ModernHUD::bar(%x + 40, %y, $Weapon::Ammo*1.8, 15, "Assets/Packs/proconfig/modules/ammohud/ammobar.png", 255);
   // lifted verbatim from proconfig::AmmoHUD::Update
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   proconfig::AmmoHUD::Update();
}

function proconfig::draw_CtfHUD_Container(%screen)
{
   %partW = 700;
   %at = ModernHUD::part("ModernHUD::CtfHUD_Container", "top-center", 59, 23, 700, 23, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from proconfig::CtfHUD::EnemyTeamValue, proconfig::CtfHUD::FriendlyTeamValue, proconfig::CtfHUD::Update
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   proconfig::CtfHUD::Update();
}

function proconfig::draw_GEnergy_Container(%screen)
{
   %partW = 225;
   %at = ModernHUD::part("ModernHUD::GEnergy_Container", "top-left", 16, 162, 225, 15, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   ModernHUD::bar(%x + 40, %y, $energy*1.8, 15, "Assets/Packs/proconfig/modules/healthnrg/energybar.png", 255);
   // lifted verbatim from proconfig::GEnergy::Update
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   proconfig::GEnergy::Update();
}

function proconfig::draw_GHealth_Container(%screen)
{
   %partW = 225;
   %at = ModernHUD::part("ModernHUD::GHealth_Container", "top-left", 16, 132, 225, 15, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   ModernHUD::bar(%x + 40, %y, $health*1.8, 15, "Assets/Packs/proconfig/modules/healthnrg/healthbar.png", 255);
   // lifted verbatim from proconfig::GHealth::Update
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   proconfig::GHealth::Update();
}

function proconfig::draw_GSpeed_Container(%screen)
{
   %partW = 225;
   %at = ModernHUD::part("ModernHUD::GSpeed_Container", "bottom-right", 0, 62, 225, 15, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   ModernHUD::bar(%x + 40, %y, $Speed*1.8, 15, "Assets/Packs/proconfig/modules/healthnrg/speedbar.png", 255);
   // lifted verbatim from proconfig::GSpeed::Update
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   proconfig::GSpeed::Update();
}

function proconfig::draw_killHUD_Container(%screen)
{
   %partW = 500;
   %at = ModernHUD::part("ModernHUD::killHUD_Container", "top-left", 1, 19, 500, 90, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from proconfig::killHUD::Update
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   proconfig::killHUD::Update();
}
ModernHUD::attach("eventConnectionAccepted", "proconfig::killHUD::Clear");
ModernHUD::attach("eventChangeMission", "proconfig::killHUD::Clear");
ModernHUD::attach("eventClientKilled", "proconfig::killHUD::onClientKilled");
ModernHUD::attach("eventClientSuicided", "proconfig::killHUD::onClientKilled");
ModernHUD::attach("eventClientTeamKilled", "proconfig::killHUD::onClientTeamKilled");

//------------------------------------------------------------------------------
// Components: one per slot, each drawing that slot's parts (generated).
//------------------------------------------------------------------------------
function proconfig::compReady()
{
   if($proconfig::CompInitDone == "")
   {
      $proconfig::CompInitDone = 1;
      proconfig::compInit();
   }
   if(isFunction("proconfig::compPrep"))
      proconfig::compPrep();
}

function proconfig::comp_weapon(%screen)
{
   proconfig::compReady();
   proconfig::draw_AmmoHUD_Container(%screen);
}

function proconfig::comp_ctf(%screen)
{
   proconfig::compReady();
   proconfig::draw_CtfHUD_Container(%screen);
}

function proconfig::comp_healthenergy(%screen)
{
   proconfig::compReady();
   proconfig::draw_GEnergy_Container(%screen);
   proconfig::draw_GHealth_Container(%screen);
   proconfig::draw_GSpeed_Container(%screen);
}

function proconfig::comp_killfeed(%screen)
{
   proconfig::compReady();
   proconfig::draw_killHUD_Container(%screen);
}

ModernHUD::component("proconfig", "weapon", "weapon", "proconfig::comp_weapon");
ModernHUD::component("proconfig", "ctf", "ctf", "proconfig::comp_ctf");
ModernHUD::component("proconfig", "healthenergy", "healthenergy", "proconfig::comp_healthenergy");
ModernHUD::component("proconfig", "killfeed", "killfeed", "proconfig::comp_killfeed");
