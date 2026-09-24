//==============================================================================
// Tribes 1.40.655 xLoader -- BORROWABLE PARTS (components.cs)
//
// Split out of hud.cs by tools/modernhud_components.py so another pack can draw these
// parts: the HUD designer's Provider choice sets $pref::HudSlot::<slot> to
// "xloader/<slot>", and ModernHUD::drawSlot execs this file on demand. It must define only
// xloader:: names -- ModernHUDPack:: belongs to whichever pack is the base.
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
// from Modules/ItemHUD/ItemHUD.acs.cs  (originally ItemHUD::Update)
function xloader::ItemHUD::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;


   %text = "";
   %kits = getItemCount("Repair Kit");
   %mines = getItemCount("Mine");



   $ItemHUD::Kits = %kits;
   $ItemHUD::Mines = %mines;

   %kits = ( %kits > 0 ) ? "Assets/Packs/xloader/modules/itemhud/kitdot.png" : "Assets/Packs/xloader/modules/itemhud/blankdot.png";

   %text = "<B0,0:" @ %kits @ ">";
   for ( %i = 0; %i < %mines; %i++ )
       %text = %text @ "<B0,0:Assets/Packs/xloader/modules/itemhud/minedot.png>";

   ModernHUD::markup(%x + 0, %y + 0, 140, %text, 255);
}

// from Modules/CTFHud/CTFHud.acs.cs  (originally CtfHUD::Row)
function xloader::CtfHUD::Row(%x, %y, %slot, %team)
{
   %score = Team::Score(%team);
   %loc = Team::Flag::Location(%team);
   switch(%loc)
   {
      case "home":
         %text = "<f3>Home";
         %bmp = (%slot == 0) ? "friendly.home.png" : "enemy.home.png";
         break;
      case "field":
         %text = "<f3>Dropped-><f2>" @ Team::Flag::Timer(%team);
         %bmp = (%slot == 0) ? "friendly.empty.png" : "enemy.empty.png";
         break;
      default:
         %text = "<f2>" @ String::escapeFormatting(Client::GetName(%loc));
         %bmp = (%slot == 0) ? "friendly.player.png" : "enemy.player.png";
         break;
   }
   ModernHUD::markup(%x, %y, 20,
                     "<b3,3:Assets/Packs/xloader/modules/ctfhud/" @ %bmp @ ">", 255);
   ModernHUD::markup(%x + 22, %y, 150,
                     "<f3>(<f2>" @ %score @ "<f3>)  " @ %text, 255);
}

// from Modules/CTFHud/CTFHud.acs.cs  (originally CtfHUD::Update)
function xloader::CtfHUD::Update()
{
   %x = $ModernHUD::PartX;
   %y = $ModernHUD::PartY;
   // The legacy container was HUD::New::Shaded (FearGui::ShadedHudCtrl, 170x40): a
   // backdrop in alpha-palette entry 254, the stock clock's. The converter dropped the
   // flag, so the rows drew bare. Black at 110 is that entry (stock2026's clock backdrop).
   glColor4ub(0, 0, 0, 110);
   glRectangle(%x, %y, 170, 40);
   // friendly on top, enemy below -- the legacy slot order.
   xloader::CtfHUD::Row(%x, %y, 0, Team::Friendly());
   xloader::CtfHUD::Row(%x, %y + 20, 1, Team::Enemy());
}

// Constants the legacy module's Init assigned and its Update reads.
function xloader::compInit()
{
   $ItemHUD::Awake = true;
}

function xloader::draw_CtfHUD_Container(%screen)
{
   %partW = 170;
   %at = ModernHUD::part("ModernHUD::CtfHUD_Container", "top-left", 180, 4, 170, 40, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from xloader::CtfHUD::Update
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   xloader::CtfHUD::Update();
}

function xloader::draw_ItemHUD_Container(%screen)
{
   %partW = 140;
   %at = ModernHUD::part("ModernHUD::ItemHUD_Container", "bottom-left", 3, 21, 140, 12, %screen);
   %x = getWord(%at, 0);
   %y = getWord(%at, 1);
   // lifted verbatim from xloader::ItemHUD::Update
   $ModernHUD::PartX = %x;
   $ModernHUD::PartY = %y;
   xloader::ItemHUD::Update();
}

//------------------------------------------------------------------------------
// Components: one per slot, each drawing that slot's parts (generated).
//------------------------------------------------------------------------------
function xloader::compReady()
{
   if($xloader::CompInitDone == "")
   {
      $xloader::CompInitDone = 1;
      xloader::compInit();
   }
   if(isFunction("xloader::compPrep"))
      xloader::compPrep();
}

function xloader::comp_ctf(%screen)
{
   xloader::compReady();
   xloader::draw_CtfHUD_Container(%screen);
}

function xloader::comp_items(%screen)
{
   xloader::compReady();
   xloader::draw_ItemHUD_Container(%screen);
}

ModernHUD::component("xloader", "ctf", "ctf", "xloader::comp_ctf");
ModernHUD::component("xloader", "items", "items", "xloader::comp_items");
