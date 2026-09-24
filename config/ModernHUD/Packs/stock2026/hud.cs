// pack: stock2026  (Stock (2026))    -- HAND-AUTHORED ("authoring": "manual")
//
// The 1998 HUD, redrawn as ModernHUD parts. Same art, same geometry, same colour
// rules as the retained controls in program\code (HealthHud.cpp, FearGuiJetHud.cpp,
// CurWeapHud.cpp, compasshud.cpp, clockhud.cpp, FearHudRadarPing.cpp) -- but every
// panel is a part: movable in the K editor, scalable with the HUD-size row, and
// borrowable by other packs as stock2026/healthenergy, stock2026/weapon and
// stock2026/clock.
//
// ART: the very files the stock controls load, by bare name through the resource
// manager. base\Entities.zip carries the panels UPSCALED 4x (hudTrans 512x128,
// compass 256x256, ping 128x128, ammoSh 128x128), so those are drawn at the 1998
// pixel size (128x32, 64x64, 32x32, 32x18); base\Interface.zip icons and end caps
// are original-size and drawn as-is.
//
// DATA: $health/$energy/$Weapon::Ammo are the framework's per-frame exports;
// $compassHeading/$compassSin/$compassCos and $sensorPing were added for this pack
// (kronosNativeCmds.cpp CfgSyncHudVars_now) because the compass and the ping light
// read state that lived only inside their retained controls.
//
// NOT carried from 1998: the compass waypoint arrow (cg.wayPoint is not exported)
// and the flipped right-hand bar end cap (drawn unflipped, 6 px).
//
// Positions are play.gui's 1080p authoring: health 50,7 / jet 50,28 / clock 0,1062 /
// sensor 1504,0 / compass 1856,376 / weapons 0,761.

exec("ModernHUD/Framework.cs");
exec("ModernHUD/Packs/stock2026/components.cs");   // borrowable parts (HUD designer mix-and-match)

$ModernHUD::Enabled = true;
$ModernHUD::Pack = "Stock (2026)";
$ModernHUD::PackId = "stock2026";

//------------------------------------------------------------------------------
// Slots.
//------------------------------------------------------------------------------
function ModernHUDPack::ownsSlot(%value)
{
   if(%value == "")
      return true;
   if(%value == "off")
      return false;
   return String::findSubStr(%value, "stock2026/") == 0;
}

// The 1.40 config-era retained containers, same list as Stock (1998).
function ModernHUDPack::detachRetained()
{
   ModernHUD::detachContainer("AmmoHUD::Container");
   ModernHUD::detachContainer("ChatOverlay::Container");
   ModernHUD::detachContainer("clock::Container");
   ModernHUD::detachContainer("CTFHUD::Container");
   ModernHUD::detachContainer("CtfHUD::Container");
   ModernHUD::detachContainer("GAmmo::Container");
   ModernHUD::detachContainer("GEnergy::Container");
   ModernHUD::detachContainer("GHealth::Container");
   ModernHUD::detachContainer("GSpeed::Container");
   ModernHUD::detachContainer("ItemHUD::Container");
   ModernHUD::detachContainer("killHUD::Container");
   ModernHUD::detachContainer("LegendzFPSHUD::Container");
   ModernHUD::detachContainer("RadarOverlay::Container");
   ModernHUD::detachContainer("RadarRing::Container");
   ModernHUD::detachContainer("ToastyHUD::Container");
}

function ModernHUDPack::prefs()
{
}

//------------------------------------------------------------------------------
// Lifecycle.
//------------------------------------------------------------------------------
function ModernHUDPack::stockHuds()
{
   // The WHOLE set. Everything this pack REDRAWS is hidden with a direct
   // SetVisible and gets NO stock row -- a stock row would switch the retained 1998
   // control back on underneath our part (Joe: "turning them on redraws the stock
   // HUD"). The pack's own rows below gate the drawn parts instead. Only the two
   // controls that really are stock here keep a stock row: chat, and the Minimap
   // (not 1998, defaults off).
   Control::SetVisible(clockHud,   false);
   Control::SetVisible(sensorHUD,  false);
   Control::SetVisible(compassHud, false);
   Control::SetVisible(jetPackHud, false);
   Control::SetVisible(healthHud,  false);
   Control::SetVisible(weaponHud,  false);
   ModernHUD::stock(chatDisplayHud, true);
   ModernHUD::stock(Minimap,        false);
   Control::SetVisible(reticleCompass, true);   // isObject() cannot see play.gui controls
}

function ModernHUDPack::init()
{
   S26::tables();
   // The reticle is the stock crosshairHud's art, drawn only while this GLOBAL pref
   // is off; packs with their own reticle leave it at 1 behind them.
   $pref::hideCrosshairArt = "0";
}

function ModernHUDPack::draw(%screen)
{
   if($S26::WepCount == "")
      S26::tables();

   // Each part draws only when this pack owns its slot AND its own K row is on;
   // otherwise its handle is hidden so the editor cannot grab an invisible box.
   %own = ModernHUDPack::ownsSlot($pref::HudSlot::healthenergy);
   if(%own && S26::on("Health"))
   {
      %at = ModernHUD::part("ModernHUD::S26Health", "top-left", 50, 7, 131, 35, %screen);
      S26::health(getWord(%at, 0), getWord(%at, 1));
   }
   else
      ModernHUD::hide("ModernHUD::S26Health");
   if(%own && S26::on("Jet"))
   {
      %at = ModernHUD::part("ModernHUD::S26Jet", "top-left", 50, 28, 131, 35, %screen);
      S26::jet(getWord(%at, 0), getWord(%at, 1));
   }
   else
      ModernHUD::hide("ModernHUD::S26Jet");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::weapon) && S26::on("Weapons"))
      S26::weapons(%screen);
   else
      ModernHUD::hide("ModernHUD::S26Weapons");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::clock) && S26::on("Clock"))
      S26::clock(%screen);
   else
      ModernHUD::hide("ModernHUD::S26Clock");

   // No native slot answers the compass or the ping light; they are ours whenever
   // their rows say so.
   if(S26::on("Compass"))
      S26::compass(%screen);
   else
      ModernHUD::hide("ModernHUD::S26Compass");
   if(S26::on("Sensor"))
      S26::sensor(%screen);
   else
      ModernHUD::hide("ModernHUD::S26Sensor");

   glPartScale(0, 0, 1);
}

function ModernHUDPack::onPlayGuiOpen()
{
   schedule("ModernHUDPack::stockHuds();", 0.1);
}

function ModernHUDPack::menuReset()
{
   ModernHUDPack::stockHuds();
}

$ModernHUD::MenuTitle = "STOCK (2026)";

// The pack's part switches (K menu and the Configs tab). These gate what THIS pack
// draws; the stock controls they replace stay hidden regardless.
// Each is tagged with its part, so the HUD designer lists it under that part (while the
// part is switched off it is not on the preview, and the row shows under the whole HUD).
ModernHUD::setting("bool", "pref::ModernHUD::stock2026::Health",  "Health bar",   "1", "", "", "ModernHUD::S26Health");
ModernHUD::setting("bool", "pref::ModernHUD::stock2026::Jet",     "Jetpack bar",  "1", "", "", "ModernHUD::S26Jet");
ModernHUD::setting("bool", "pref::ModernHUD::stock2026::Weapons", "Weapon list",  "1", "", "", "ModernHUD::S26Weapons");
ModernHUD::setting("bool", "pref::ModernHUD::stock2026::Clock",   "Clock",        "1", "", "", "ModernHUD::S26Clock");
ModernHUD::setting("bool", "pref::ModernHUD::stock2026::Compass", "Compass",      "1", "", "", "ModernHUD::S26Compass");
ModernHUD::setting("bool", "pref::ModernHUD::stock2026::Sensor",  "Sensor light", "1", "", "", "ModernHUD::S26Sensor");

ModernHUD::attach("eventGuiOpen_PlayGui", "ModernHUDPack::onPlayGuiOpen");
ModernHUDPack::prefs();
ModernHUDPack::stockHuds();
ModernHUDPack::init();
echo("[STOCK2026] pack ready");
$ModernHUD::LoadComplete = "stock2026";
