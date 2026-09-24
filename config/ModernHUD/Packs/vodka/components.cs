//==============================================================================
// Tribes - Minimalist - v0dkA -- BORROWABLE PARTS (components.cs)
//
// Split out of hud.cs by tools/modernhud_components.py so another pack can draw these
// parts: the HUD designer's Provider choice sets $pref::HudSlot::<slot> to
// "vodka/<slot>", and ModernHUD::drawSlot execs this file on demand. It must define only
// vodka:: names -- ModernHUDPack:: belongs to whichever pack is the base.
// hud.cs execs this file too, so the pack's own draw uses the same code.
//==============================================================================

ModernHUD::require("ModernHUD/Core/Data/Team.cs");
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
// from Modules/WeaponHUD/AmmoHUD.acs.cs  (originally AmmoHUD::Init)
function vodka::AmmoHUD::Init()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
   ModernHUD::markup(%x + 0, %y + 0, 159, "<B0,0:Assets/Packs/vodka/modules/weaponhud/ammohudbg-50.png>", 255);
}

// from Modules/WeaponHUD/AmmoHUD.acs.cs  (originally AmmoHUD::Update)
function vodka::AmmoHUD::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;

   if($Weapon::Ammo < 0)
       %ammo = "~";
   else
       %ammo = $Weapon::Ammo;

   ModernHUD::markup(%x + 100, %y + 13, 40, "<jc><f:small-black-stroke.pft:ffffffff:000000ff:1,1>" @ %ammo, 255);





   %weapon = getItemDesc(getMountedItem(0));
   if(%weapon == "Disc Launcher")
       %icon = "Assets/Packs/vodka/modules/weaponhud/disc.png";
   else if(%weapon == "Grenade Launcher")
       %icon = "Assets/Packs/vodka/modules/weaponhud/grenade.png";
   else if(%weapon == "Chaingun")
       %icon = "Assets/Packs/vodka/modules/weaponhud/chaingun.png";
   else if(%weapon == "Mortar")
       %icon = "Assets/Packs/vodka/modules/weaponhud/mortar.png";
   else if(%weapon == "Plasma Gun")
       %icon = "Assets/Packs/vodka/modules/weaponhud/plasma.png";
   else if(%weapon == "Laser Rifle")
       %icon = "Assets/Packs/vodka/modules/weaponhud/sniper.png";
   else if(%weapon == "Blaster")
       %icon = "Assets/Packs/vodka/modules/weaponhud/blaster.png";
   else if(%weapon == "ELF Gun")
       %icon = "Assets/Packs/vodka/modules/weaponhud/elf.png";
   else if(%weapon == "Targeting Laser")
       %icon = "Assets/Packs/vodka/modules/weaponhud/target.png";
   else
       %icon = "";

   if(%icon != "")
       ModernHUD::markup(%x + 25, %y + 13, 134, "<B0,0:" @ %icon @ ">", 255);
   else
       ModernHUD::markup(%x + 25, %y + 13, 134, "", 255);

}

// from Modules/ChatOverlay/ChatBorder.acs.cs  (originally ChatOverlay::Init)
function vodka::ChatOverlay::Init()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
   ModernHUD::markup(%x + 0, %y + 0, 664, "<B0,0:Assets/Packs/vodka/modules/chatoverlay/chatbg.png>", 255);
}

// from Modules/gameclock.acs.cs  (originally clock::Iterate)
function vodka::clock::Iterate()
{
   if ($clock::CountingDown)
   {
       if ($clock::Sec > 0)
           $clock::Sec--;
       else
       {
           $clock::Sec = 59;

           if ($clock::Min > 0)
               $clock::Min--;
           else
           {
               $clock::Min = 59;
               $clock::Hour--;
           }
       }
   }
   else
   {
       if ($clock::Sec < 59)
           $clock::Sec++;
       else
       {
           $clock::Sec = 0;

           if ($clock::Min < 59)
               $clock::Min++;
           else
           {
               $clock::Min = 0;
               $clock::Hour++;
           }
       }
   }
   vodka::clock::Update();
   Schedule::Add("vodka::clock::Iterate();", 1);
}

// from Modules/gameclock.acs.cs  (originally clock::Reset)
function vodka::clock::Reset()
{
   $clock::Hour = 0;
   $clock::Min = 0;
   $clock::Sec = 0;
   $clock::CountingDown = false;
   Schedule::Add("vodka::clock::Iterate();", 1);
}

// from Modules/gameclock.acs.cs  (originally clock::SetReverse)
function vodka::clock::SetReverse()
{
   $clock::Hour = $clock::Min = $clock::Sec = 0;
   $clock::CountingDown = false;

   Schedule::Add("vodka::clock::Iterate();", 1);
   vodka::clock::Update();
}

// from Modules/gameclock.acs.cs  (originally clock::Update)
function vodka::clock::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
   ModernHUD::markup(%x + 0, %y + 0, 180, "<jc><f1>" @ ( ( $clock::Hour < 10 ) ? "0" @ $clock::Hour : $clock::Hour ) @ ":" @ ( ( $clock::Min < 10 ) ? "0" @ $clock::Min : $clock::Min ) @ ":" @ ( ( $clock::Sec < 10 ) ? "0" @ $clock::Sec : $clock::Sec ), 255);
}

// from Modules/gameclock.acs.cs  (originally clock::UpdateTime)
// (2026-09-01) The eventUpdateTime bridge (Presto events.cs remoteSetTime) assumes every
// setTime is a countdown and negates it; an unlimited match now syncs a POSITIVE count-up
// clock (objectives.cs Game::timeLimitTick), which arrived here as negative minutes and
// ran the private timer below zero. Read the engine clock directly instead: sign = direction.
function vodka::clock::UpdateTime(%min, %sec)
{
   %t = getHudTimer();
   if(%t == "")
      %t = 0;
   $clock::CountingDown = (%t < 0);
   if(%t < 0)
      %t = -%t;
   %t = floor(%t);
   $clock::Hour = floor(%t / 3600);
   $clock::Min = floor((%t - $clock::Hour * 3600) / 60);
   $clock::Sec = %t % 60;
   Schedule::Add("vodka::clock::Iterate();", 1);
}

// from Modules/CTFHud/CTFHud.acs.cs  (originally CTFHUD::EnemyTeamValue)
function vodka::CTFHUD::EnemyTeamValue(%team, %score1)
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
       %team = Team::Enemy();
   %loc = Team::Flag::Location(Team::Enemy());
   vodka::FlagFlash(%team);

   switch ( %loc ) {
       case "home":
           %loc = "<f:CTFHud-Font.pft>:::::: Home ::::::";
           break;
       case "field":
           %loc = $FlagDropFontE ~ Team::Flag::Timer(%team);
           break;
       default:
           %loc = "<f:CTFHud-Font.pft:3ffd04>" ~ String::escapeFormatting(Client::GetName(%loc));
           break;
   }

   ModernHUD::markup(%x + 16, %y + 56, 178, "<jc>" ~ %loc, 255);
}

// from Modules/CTFHud/CTFHud.acs.cs  (originally CTFHUD::FriendlyTeamValue)
function vodka::CTFHUD::FriendlyTeamValue(%team, %score0)
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
       %team = Team::Friendly();
   %loc = Team::Flag::Location(Team::Friendly());
   vodka::FlagFlash(%team);

   switch ( %loc ) {
       case "home":
           %loc = "<f:CTFHud-Font.pft>:::::: Home ::::::";
           break;
       case "field":
           %loc = $FlagDropFontF ~ Team::Flag::Timer(%team);
           break;
       default:
           %loc = "<f:CTFHud-Font.pft:d20808>" ~ String::escapeFormatting(Client::GetName(%loc));
           break;
   }

   ModernHUD::markup(%x + 16, %y + 18, 178, "<jc><f1>" ~ %loc, 255);
}

// from Modules/CTFHud/CTFHud.acs.cs  (originally CTFHUD::Init)
function vodka::CTFHUD::Init()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
   ModernHUD::markup(%x + 0, %y + 0, 200, "<b3,3:Assets/Packs/vodka/modules/ctfhud/ctfbg.png:ffffff80>", 255);
   ModernHUD::markup(%x + 14, %y + 5, 186, "<f1>Score:", 255);
   ModernHUD::markup(%x + 14, %y + 43, 186, "<f1>Score:", 255);
   ModernHUD::markup(%x + 14, %y + 18, 186, "<f1>Flag:", 255);
   ModernHUD::markup(%x + 14, %y + 56, 186, "<f1>Flag:", 255);
}

// from Modules/CTFHud/CTFHud.acs.cs  (originally CTFHUD::Update)
function vodka::CTFHUD::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
       %score0 = Team::Score(Team::Friendly());
       %score1 = Team::Score(Team::Enemy());
   vodka::CTFHUD::FriendlyTeamValue(%team);
   vodka::CTFHUD::EnemyTeamValue(%team);
   ModernHUD::markup(%x + 64, %y + 5, 136, "<f1>" ~%score0, 255);
   ModernHUD::markup(%x + 64, %y + 43, 136, "<f1>" ~%score1, 255);
}

// from Modules/CTFHud/CTFHud.acs.cs  (originally FlagFlash)
function vodka::FlagFlash(%team)
{
   if (Team::Flag::Timer(%team) < 10) {
       $FlagDropFontE = "<f:CTFHud-Font.pft:ff0000"~$CTFTimerDropID~":ffffff>";
       $FlagDropFontF = "<f:CTFHud-Font.pft:3ffd04"~$CTFTimerDropID~":ffffff>";
     } else {
       $FlagDropFontE = "<f:CTFHud-Font.pft:FFFFFF>";
       $FlagDropFontF = "<f:CTFHud-Font.pft:FFFFFF>";
     }
}

// from Modules/HeEnHUD/Energy.acs.cs  (originally GEnergy::Init)
function vodka::GEnergy::Init()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
   ModernHUD::markup(%x + 0, %y + 0, 156, "<B0,0:Assets/Packs/vodka/modules/heenhud/frame.png>", 255);
}

// from Modules/HeEnHUD/Health.acs.cs  (originally GHealth::Init)
function vodka::GHealth::Init()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
   ModernHUD::markup(%x + 0, %y + 0, 156, "<B0,0:Assets/Packs/vodka/modules/heenhud/frame.png>", 255);
}

// from Modules/HeEnHUD/Speed.acs.cs  (originally GSpeed::Update)
function vodka::GSpeed::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
   ModernHUD::markup(%x + 0, %y + 5, 80, "<jc><f:Speed-Font.pft:ffc600FF>" @ $speed, 255);
}

// from Modules/WeaponHUD/AmmoHUD.acs.cs  (originally ItemHUD::Update)
function vodka::ItemHUD::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;


   %Gren = getItemCount("Grenade");
   %Beac = getItemCount("Beacon");
   %RKit = getItemCount("Repair Kit");


   $ItemHUD::RKit = %RKit;
   $ItemHUD::Gren = %Gren;
   $ItemHUD::Beac = %Beac;





   %countFont = "<jc><f:small-black-stroke.pft:ffffffff:000000ff:1,1>";
   %GrenadeTxt = %countFont @ %Gren;
   %BeaconTxt = %countFont @ %Beac;
   %RepKitTxt = %countFont @ %RKit;

     ModernHUD::markup(%x + 15, %y + 53, 144, "<B0,0:Assets/Packs/vodka/modules/weaponhud/repkit.png>", 255);
     ModernHUD::markup(%x + 34, %y + 53, 30, %RepKitTxt, 255);

     ModernHUD::markup(%x + 96, %y + 53, 63, " <B0,0:Assets/Packs/vodka/modules/weaponhud/gren.png>", 255);
     ModernHUD::markup(%x + 119, %y + 53, 30, %GrenadeTxt, 255);

     ModernHUD::markup(%x + 51, %y + 53, 108, " <B0,0:Assets/Packs/vodka/modules/weaponhud/beacon.png>", 255);
     ModernHUD::markup(%x + 76, %y + 53, 30, %BeaconTxt, 255);
}

// from Modules/minimap/RadarBorder.acs.cs  (originally RadarOverlay::Init)
function vodka::RadarOverlay::Init()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
   ModernHUD::markup(%x + 0, %y + 0, 308, "<B0,0:Assets/Packs/vodka/modules/minimap/radar.png:keyblack>", 255);
}

// from Modules/minimap/RadarRing.acs.cs  (originally RadarRing::Init)
function vodka::RadarRing::Init()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
   ModernHUD::markup(%x + 0, %y + 0, 300, "<B0,0:Assets/Packs/vodka/modules/minimap/ring.png>", 255);
}

// from Modules/fpsHUD/LegendzFPSHUD.acs.cs  (originally TimeFPS::Update)
function vodka::TimeFPS::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
   %fps ="<f2> FPS:<f:FPS-Font.pft:ffde00> " @ floor($ConsoleWorld::FrameRate)@"";
   ModernHUD::markup(%x + 0, %y + 1, 100, %fps, 255);
}

// from Modules/ToastyHUD.acs.cs  (originally ToastyHUD::GetImageAndSound)
function vodka::ToastyHUD::GetImageAndSound()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
   if(!$pref::ToastyCustomMode) {

       if($IMAGE_NAME == "")
           $IMAGE_NAME = "Assets/Packs/vodka/modules/toastyhud/toasty.png";


       if($SND_NAME == "")
           $SND_NAME = "mk.toasty.ogg";
   }


   // A custom $IMAGE_NAME (pref::ToastyCustomMode) is still a bare filename
   // relative to Modules/ToastyHUD/; the pack default is already a full path.
   if(String::findSubStr($IMAGE_NAME, "Assets/Packs/") == 0)
       %img = $IMAGE_NAME;
   else
       %img = "Modules/ToastyHUD/" @ $IMAGE_NAME;
   ModernHUD::markup(%x + 0, %y + 0, 405, "<B0,0:" @ %img @ ">", 255);
}

// Constants the legacy module's Init assigned and its Update reads.
function vodka::compInit()
{
   $AmmoHUD::Loaded = true;
   $AmmoHUD::Awake = false;
   $ItemHUD::RKit = -1;
   $ItemHUD::Gren = -1;
   $ItemHUD::Beac = -1;
   $AmmoHUD::Awake = true;
   $CTFHUD::Loaded = true;
   $CTFTimerDropID = xFont::NewTimer("CTFTimerDrop", 0, 255, 25, 0.05, 0);
}

function vodka::draw_AmmoHUD_Container(%screen)
{
   %partW = 159;
   %at = ModernHUD::part("ModernHUD::AmmoHUD_Container", "top-center", 346, 19, 159, 88, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from vodka::AmmoHUD::Init, vodka::AmmoHUD::Update, vodka::ItemHUD::Update
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   vodka::AmmoHUD::Init();
   vodka::AmmoHUD::Update();
   vodka::ItemHUD::Update();
}

function vodka::draw_ChatOverlay_Container(%screen)
{
   %partW = 664;
   %at = ModernHUD::part("ModernHUD::ChatOverlay_Container", "top-center", -76, 18, 664, 89, %screen);
   %at = ModernHUD::dockTo("ModernHUD::ChatOverlay_Container", "chatDisplayHud", -7, -5, %at, 664, 89);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from vodka::ChatOverlay::Init
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   vodka::ChatOverlay::Init();
}

function vodka::draw_clock_Container(%screen)
{
   %partW = 180;
   %at = ModernHUD::part("ModernHUD::clock_Container", "top-left", 171, 4, 180, 20, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from vodka::clock::Update
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   vodka::clock::Update();
}

function vodka::draw_CTFHUD_Container(%screen)
{
   %partW = 200;
   %at = ModernHUD::part("ModernHUD::CTFHUD_Container", "top-right", 0, 7, 200, 100, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from vodka::CTFHUD::EnemyTeamValue, vodka::CTFHUD::FriendlyTeamValue, vodka::CTFHUD::Init, vodka::CTFHUD::Update
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   vodka::CTFHUD::Init();
   vodka::CTFHUD::Update();
}

function vodka::draw_GEnergy_Container(%screen)
{
   %partW = 156;
   %at = ModernHUD::part("ModernHUD::GEnergy_Container", "top-right", 33, 115, 156, 28, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   ModernHUD::bar(%x + 11, %y + 11, $energy*1.35, 7, "Assets/Packs/vodka/modules/heenhud/energybar.png", 255);
   // lifted verbatim from vodka::GEnergy::Init
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   vodka::GEnergy::Init();
}

function vodka::draw_GHealth_Container(%screen)
{
   %partW = 156;
   %at = ModernHUD::part("ModernHUD::GHealth_Container", "top-right", 33, 90, 156, 28, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   ModernHUD::bar(%x + 11, %y + 11, $health*1.35, 7, "Assets/Packs/vodka/modules/heenhud/healthbar.png", 255);
   // lifted verbatim from vodka::GHealth::Init
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   vodka::GHealth::Init();
}

function vodka::draw_GSpeed_Container(%screen)
{
   %partW = 80;
   %at = ModernHUD::part("ModernHUD::GSpeed_Container", "bottom-right", 91, 103, 80, 30, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from vodka::GSpeed::Update
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   vodka::GSpeed::Update();
}

function vodka::draw_LegendzFPSHUD_Container(%screen)
{
   %partW = 106;
   %at = ModernHUD::part("ModernHUD::LegendzFPSHUD_Container", "top-left", 27, 3, 106, 26, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from vodka::TimeFPS::Update
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   vodka::TimeFPS::Update();
}

function vodka::draw_RadarOverlay_Container(%screen)
{
   %partW = 308;
   %at = ModernHUD::part("ModernHUD::RadarOverlay_Container", "top-left", 5, 17, 308, 308, %screen);
   %at = ModernHUD::dockTo("ModernHUD::RadarOverlay_Container", "Minimap", 4, 4, %at, 308, 308);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from vodka::RadarOverlay::Init
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   vodka::RadarOverlay::Init();
}

function vodka::draw_RadarRing_Container(%screen)
{
   %partW = 300;
   %at = ModernHUD::part("ModernHUD::RadarRing_Container", "top-left", 8, 21, 300, 300, %screen);
   %at = ModernHUD::dockTo("ModernHUD::RadarRing_Container", "Minimap", 8, 8, %at, 300, 300);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from vodka::RadarRing::Init
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   vodka::RadarRing::Init();
}

function vodka::draw_ToastyHUD_Container(%screen)
{
   %partW = 405;
   %at = ModernHUD::part("ModernHUD::ToastyHUD_Container", "bottom-right", -405, -5, 405, 405, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from vodka::ToastyHUD::GetImageAndSound
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   vodka::ToastyHUD::GetImageAndSound();
}
ModernHUD::attach("eventConnected", "vodka::clock::Reset");
ModernHUD::attach("eventMatchStarted", "vodka::clock::SetReverse");
ModernHUD::attach("eventUpdateTime", "vodka::clock::UpdateTime");

//------------------------------------------------------------------------------
// Components: one per slot, each drawing that slot's parts (generated).
//------------------------------------------------------------------------------
function vodka::compReady()
{
   if($vodka::CompInitDone == "")
   {
      $vodka::CompInitDone = 1;
      vodka::compInit();
   }
   if(isFunction("vodka::compPrep"))
      vodka::compPrep();
}

function vodka::comp_weapon(%screen)
{
   vodka::compReady();
   vodka::draw_AmmoHUD_Container(%screen);
}

function vodka::comp_chat(%screen)
{
   vodka::compReady();
   vodka::draw_ChatOverlay_Container(%screen);
}

function vodka::comp_clock(%screen)
{
   vodka::compReady();
   vodka::draw_clock_Container(%screen);
}

function vodka::comp_ctf(%screen)
{
   vodka::compReady();
   vodka::draw_CTFHUD_Container(%screen);
}

function vodka::comp_healthenergy(%screen)
{
   vodka::compReady();
   vodka::draw_GEnergy_Container(%screen);
   vodka::draw_GHealth_Container(%screen);
   vodka::draw_GSpeed_Container(%screen);
}

function vodka::comp_fps(%screen)
{
   vodka::compReady();
   vodka::draw_LegendzFPSHUD_Container(%screen);
}

function vodka::comp_minimap(%screen)
{
   vodka::compReady();
   vodka::draw_RadarOverlay_Container(%screen);
   vodka::draw_RadarRing_Container(%screen);
}

function vodka::comp_toasty(%screen)
{
   vodka::compReady();
   vodka::draw_ToastyHUD_Container(%screen);
}

ModernHUD::component("vodka", "weapon", "weapon", "vodka::comp_weapon");
ModernHUD::component("vodka", "chat", "chat", "vodka::comp_chat");
ModernHUD::component("vodka", "clock", "clock", "vodka::comp_clock");
ModernHUD::component("vodka", "ctf", "ctf", "vodka::comp_ctf");
ModernHUD::component("vodka", "healthenergy", "healthenergy", "vodka::comp_healthenergy");
ModernHUD::component("vodka", "fps", "fps", "vodka::comp_fps");
ModernHUD::component("vodka", "minimap", "minimap", "vodka::comp_minimap");
ModernHUD::component("vodka", "toasty", "toasty", "vodka::comp_toasty");
