//==============================================================================
// Basic -- BORROWABLE PARTS (components.cs)
//
// Split out of hud.cs by tools/modernhud_components.py so another pack can draw these
// parts: the HUD designer's Provider choice sets $pref::HudSlot::<slot> to
// "basic/<slot>", and ModernHUD::drawSlot execs this file on demand. It must define only
// basic:: names -- ModernHUDPack:: belongs to whichever pack is the base.
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
// from Modules/CTFHud/CTFHud.acs.cs  (originally CTFHUD::EnemyTeamValue)
function basic::CTFHUD::EnemyTeamValue(%team, %score1)
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
       %team = Team::Enemy();
   %loc = Team::Flag::Location(Team::Enemy());

   basic::FlagFlash(%team);

   switch ( %loc ) {
       case "home":
           %loc = "<f:small-black-stroke.pft:FFFFFFFF:000000ff:1,1>Home";
           %bmp = "Assets/Packs/basic/modules/ctfhud/flag-icon.png:ff0000ff";
           break;
       case "field":
           %loc = $FlagDropFont ~ Team::Flag::Timer(%team);
           %bmp = $FlagDropIcon1;
           break;
       default:
           %loc = String::escapeFormatting(Client::GetName(%loc));
           %bmp = "Assets/Packs/basic/modules/ctfhud/flag-icon.png:00d2ffff";
           break;
   }
   ModernHUD::markup(%x + 55, %y + 32, 445, "<f:font-nostroke.pft:006880ff:006880ff:1,1>" ~ %loc, 255);
   ModernHUD::markup(%x + 55, %y + 32, 445, "<f:font-stroke.pft:00cfff>" ~ %loc, 255);

   ModernHUD::markup(%x + 0, %y + 25, 500, "<b3,3:"~%bmp~">", 255);
}

// from Modules/CTFHud/CTFHud.acs.cs  (originally CTFHUD::FriendlyTeamValue)
function basic::CTFHUD::FriendlyTeamValue(%team, %score0)
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
       %team = Team::Friendly();
   %loc = Team::Flag::Location(Team::Friendly());

   basic::FlagFlash(%team);

   switch ( %loc ) {
       case "home":
           %loc = "<f:small-black-stroke.pft:FFFFFFFF:000000ff:1,1>Home";
           %bmp = "Assets/Packs/basic/modules/ctfhud/flag-icon.png:00ff02ff";
           break;
       case "field":
           %loc = $FlagDropFont ~ Team::Flag::Timer(%team);
           %bmp = $FlagDropIcon0;
           break;
       default:
           %loc = String::escapeFormatting(Client::GetName(%loc));
           %bmp = "Assets/Packs/basic/modules/ctfhud/flag-icon.png:fdff00ff";
           break;
   }
   ModernHUD::markup(%x + 55, %y + 8, 445, "<f:font-nostroke.pft:807b00ff:807b00ff:1,1>" ~ %loc, 255);
   ModernHUD::markup(%x + 55, %y + 8, 445, "<f:font-stroke.pft:fff500ff>" ~ %loc, 255);

   ModernHUD::markup(%x + 0, %y + 1, 500, "<b3,3:"~%bmp~">", 255);
}

// from Modules/CTFHud/CTFHud.acs.cs  (originally CTFHUD::Update)
function basic::CTFHUD::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
       %score0 = Team::Score(Team::Friendly());
       %score1 = Team::Score(Team::Enemy());
   basic::CTFHUD::FriendlyTeamValue(%team);
   basic::CTFHUD::EnemyTeamValue(%team);
   ModernHUD::markup(%x + 33, %y + 8, 467, "<f:small-black-stroke.pft:228b01FF:000000ff:1,1>" ~%score0, 255);

   ModernHUD::markup(%x + 33, %y + 32, 467, "<f:small-black-stroke.pft:ff0000FF:000000ff:1,1>" ~%score1, 255);
}

// from Modules/CTFHud/CTFHud.acs.cs  (originally FlagFlash)
function basic::FlagFlash(%team)
{
   if (Team::Flag::Timer(%team) < 10) {
       $FlagDropIcon0 = "Assets/Packs/basic/modules/ctfhud/flag-icon.png:00ff02"~$CTFTimerDrop~"";
       $FlagDropIcon1 = "Assets/Packs/basic/modules/ctfhud/flag-icon.png:ff0000"~$CTFTimerDrop~"";
     } else {
       $FlagDropIcon0 = "Assets/Packs/basic/modules/ctfhud/flag-icon.png:ffffff";
       $FlagDropIcon1 = "Assets/Packs/basic/modules/ctfhud/flag-icon.png:ffffff";
     }
}

// from Modules/AmmoHud.acs.cs  (originally GAmmo::Update)
function basic::GAmmo::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
   if($Weapon::Ammo < 1) {
       %display = " ";
   } else {
       %display = $Weapon::Ammo;
   }

   if($health == 0 || $playingdemo || Client::GetTeam(getManagerId()) == -1) {
       %display = "";
   }



   ModernHUD::markup(%x + 0, %y + 0, 100, "<jc><f:white-default.pft:00cfffff:006880c8:2,2>" @ %display, 255);
}

// from Modules/aHENum/Energy.acs.cs  (originally GEnergy::Update)
function basic::GEnergy::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
       if ( $energy < 35 ) {
           ModernHUD::markup(%x + 0, %y + 0, 100, "<jc><f:small-black-stroke.pft:ff0000ff:000000ff:1,1>" @ $energy, 255);
       } else {
           ModernHUD::markup(%x + 0, %y + 0, 100, "<jc><f:small-black-stroke.pft:ffffffff:000000ff:1,1>" @ $energy, 255);
       }


}

// from Modules/aHENum/Health.acs.cs  (originally GHealth::Update)
function basic::GHealth::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
       if ( $health > 66 ) {
           ModernHUD::markup(%x + 0, %y + 0, 100, "<jc><f:small-black-stroke.pft:ffffffff:000000ff:1,1>" @ $health, 255);
       } else if ( $health > 32 ) {
           ModernHUD::markup(%x + 0, %y + 0, 100, "<jc><f:small-black-stroke.pft:fff000ff:000000ff:1,1>" @ $health, 255);
       } else {
           ModernHUD::markup(%x + 0, %y + 0, 100, "<jc><f:small-black-stroke.pft:ff0000:000000ff:1,1>" @ $health, 255);
       }


}

// from Modules/aHENum/Speed.acs.cs  (originally GSpeed::Update)
function basic::GSpeed::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
   ModernHUD::markup(%x + 0, %y + 0, 50, "<f8>" @ $speed, 255);
}

// from Modules/ItemHUD/ItemHUD.acs.cs  (originally ItemHUD::Update)
function basic::ItemHUD::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;



   %text = "";
   %kits = getItemCount("Repair Kit");
   %Grenades = getItemCount("Grenade");



   $ItemHUD::Kits = %kits;
   $ItemHUD::Grenades = %Grenades;

   %kits = ( %kits > 0 ) ? "Assets/Packs/basic/modules/itemhud/kitdot.png" : "Assets/Packs/basic/modules/itemhud/blankdot.png";

   %text = "<B0,0:" @ %kits @ ">";
   for ( %i = 0; %i < %Grenades; %i++ )
       %text = %text @ "<B0,0:Assets/Packs/basic/modules/itemhud/grendot.png>";

   ModernHUD::markup(%x + 0, %y + 0, 100, %text, 255);
}

// from Modules/fpsHUD/LegendzFPSHUD.acs.cs  (originally TimeFPS::Update)
function basic::TimeFPS::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
   %fps ="<f2> FPS:<f3> " @ floor($ConsoleWorld::FrameRate)@"";
   ModernHUD::markup(%x + 0, %y + 1, 100, %fps, 255);
}

// from Modules/ToastyHUD.acs.cs  (originally ToastyHUD::GetImageAndSound)
function basic::ToastyHUD::GetImageAndSound()
{
   // ★Gated.★ Legacy parked this 405x405 art off the right edge and slid it in;
   // drawing it unconditionally puts a permanent Dan Forden on the HUD, and
   // parking it offscreen cannot work because fitOnScreen pulls parts back on.
   if($ToastyHUD::ShowUntil == "" || getSimTime() > $ToastyHUD::ShowUntil)
      return;
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
   if(!$pref::ToastyCustomMode) {

       if($IMAGE_NAME == "")
           $IMAGE_NAME = "Assets/Packs/basic/modules/toastyhud/toasty.png";


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

// from Modules/ToastyHUD.acs.cs  (originally ToastyHUD::Trigger)
function basic::ToastyHUD::Trigger(%msg)
{
   if($playingDemo)
      return;
   if(String::findSubStr(%msg, "mid-air") == -1)
      return;
   // Legacy walked words until one was numeric; getWord past the end returns
   // the literal "-1", which is what ends the walk.
   %meters = -1;
   for(%i = 0; String::Trim(getWord(%msg, %i)) != -1; %i++)
   {
      %w = getWord(%msg, %i);
      if(chr(%w) == "")
      {
         %meters = %w;
         break;
      }
   }
   if(%meters < 50)
      return;
   $ToastyHUD::ShowUntil = getSimTime() + 1400;
   $ToastyHUD::PTotal++;
}

// Constants the legacy module's Init assigned and its Update reads.
function basic::compInit()
{
   $ItemHUD::Awake = true;
}

function basic::draw_CTFHUD_Container(%screen)
{
   %partW = 500;
   %at = ModernHUD::part("ModernHUD::CTFHUD_Container", "top-left", 41, 61, 500, 72, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from basic::CTFHUD::EnemyTeamValue, basic::CTFHUD::FriendlyTeamValue, basic::CTFHUD::Update
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   basic::CTFHUD::Update();
}

function basic::draw_GAmmo_Container(%screen)
{
   %partW = 100;
   %at = ModernHUD::part("ModernHUD::GAmmo_Container", "bottom-center", 29, 67, 100, 50, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from basic::GAmmo::Update
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   basic::GAmmo::Update();
}

function basic::draw_GEnergy_Container(%screen)
{
   %partW = 100;
   %at = ModernHUD::part("ModernHUD::GEnergy_Container", "top-left", 38, 34, 100, 20, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from basic::GEnergy::Update
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   basic::GEnergy::Update();
}

function basic::draw_GHealth_Container(%screen)
{
   %partW = 100;
   %at = ModernHUD::part("ModernHUD::GHealth_Container", "top-left", 38, 11, 100, 20, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from basic::GHealth::Update
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   basic::GHealth::Update();
}

function basic::draw_GSpeed_Container(%screen)
{
   %partW = 50;
   %at = ModernHUD::part("ModernHUD::GSpeed_Container", "bottom-right", 74, 27, 50, 20, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from basic::GSpeed::Update
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   basic::GSpeed::Update();
}

function basic::draw_ItemHUD_Container(%screen)
{
   %partW = 100;
   %at = ModernHUD::part("ModernHUD::ItemHUD_Container", "bottom-center", 24, 132, 100, 50, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from basic::ItemHUD::Update
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   basic::ItemHUD::Update();
}

function basic::draw_LegendzFPSHUD_Container(%screen)
{
   %partW = 106;
   %at = ModernHUD::part("ModernHUD::LegendzFPSHUD_Container", "top-right", 0, 4, 106, 26, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from basic::TimeFPS::Update
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   basic::TimeFPS::Update();
}

function basic::draw_ToastyHUD_Container(%screen)
{
   %partW = 405;
   %at = ModernHUD::part("ModernHUD::ToastyHUD_Container", "bottom-right", 0, 40, 405, 405, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from basic::ToastyHUD::GetImageAndSound
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   basic::ToastyHUD::GetImageAndSound();
}
ModernHUD::attach("eventServerMessage", "basic::ToastyHUD::Trigger");

//------------------------------------------------------------------------------
// Components: one per slot, each drawing that slot's parts (generated).
//------------------------------------------------------------------------------
function basic::compReady()
{
   if($basic::CompInitDone == "")
   {
      $basic::CompInitDone = 1;
      basic::compInit();
   }
   if(isFunction("basic::compPrep"))
      basic::compPrep();
}

function basic::comp_ctf(%screen)
{
   basic::compReady();
   basic::draw_CTFHUD_Container(%screen);
}

function basic::comp_weapon(%screen)
{
   basic::compReady();
   basic::draw_GAmmo_Container(%screen);
}

function basic::comp_healthenergy(%screen)
{
   basic::compReady();
   basic::draw_GEnergy_Container(%screen);
   basic::draw_GHealth_Container(%screen);
   basic::draw_GSpeed_Container(%screen);
}

function basic::comp_items(%screen)
{
   basic::compReady();
   basic::draw_ItemHUD_Container(%screen);
}

function basic::comp_fps(%screen)
{
   basic::compReady();
   basic::draw_LegendzFPSHUD_Container(%screen);
}

function basic::comp_toasty(%screen)
{
   basic::compReady();
   basic::draw_ToastyHUD_Container(%screen);
}

ModernHUD::component("basic", "ctf", "ctf", "basic::comp_ctf");
ModernHUD::component("basic", "weapon", "weapon", "basic::comp_weapon");
ModernHUD::component("basic", "healthenergy", "healthenergy", "basic::comp_healthenergy");
ModernHUD::component("basic", "items", "items", "basic::comp_items");
ModernHUD::component("basic", "fps", "fps", "basic::comp_fps");
ModernHUD::component("basic", "toasty", "toasty", "basic::comp_toasty");
