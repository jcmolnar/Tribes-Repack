//==============================================================================
// OPS -- hand-authored ModernHUD pack.       (manifest: pack.json)
//
// Opsaya's config HUD, redrawn in ModernHUD (see the header of components.cs for
// the module-by-module mapping). "authoring": "manual", so the generator never
// overwrites this file.
//
// What the pack sets outside its own parts, all saved first and restored by
// ModernHUDPack::restore() when another pack is picked:
//   * the chat log: 370 wide at y 6, centred -- the anchor the top cluster
//     hangs off (Opsaya's play.gui: chatDisplayHud 370x61 at 782,6)
//   * nameplates ($mj::, from Modules/guistuff.acs.cs); a player's Options rows
//     for these still win (resolved above $mj:: in the engine)
//   * the minimap: square, rotating, 302 wide, zoom 1.75 (Opsaya's ClientPrefs)
//
// Also carried: GrabSpeed.acs.cs -- "You grabbed going N m/s!" when you take a flag.
//==============================================================================

exec("ModernHUD/Framework.cs");
exec("ModernHUD/Packs/ops/components.cs");   // borrowable parts (HUD designer mix-and-match)

$ModernHUD::Enabled = true;
$ModernHUD::Pack = "OPS";
$ModernHUD::PackId = "ops";

function ModernHUDPack::ownsSlot(%value)
{
   if(%value == "")
      return true;
   if(%value == "off")
      return false;
   return String::findSubStr(%value, "OPS::") == 0;
}

//------------------------------------------------------------------------------
// Client-wide settings -- saved before written, restored on unload. Written out
// longhand: the console has no dynamic variable assignment (see vantage/hud.cs).
//------------------------------------------------------------------------------
function OPS::apply()
{
   if($OPS::Saved == "")
   {
      $OPS::Saved = 1;

      $OPS::Sav::greenlines     = $mj::greenlines;
      $OPS::Sav::shownames      = $mj::shownames;
      $OPS::Sav::showhpbars     = $mj::showhpbars;
      $OPS::Sav::showjetbars    = $mj::showjetbars;
      $OPS::Sav::showhptext     = $mj::showhptext;
      $OPS::Sav::barscrouch     = $mj::barscrouch;
      $OPS::Sav::passhelper     = $mj::passhelper;
      $OPS::Sav::passhelpermm   = $mj::passhelpermm;
      $OPS::Sav::fontdefault    = $mj::fontdefault;
      $OPS::Sav::fontpass       = $mj::fontpass;
      $OPS::Sav::bar_width      = $mj::bar_width;
      $OPS::Sav::bar_height     = $mj::bar_height;
      $OPS::Sav::bar_border     = $mj::bar_border_width;

      $OPS::Sav::ChatModX       = $pref::ChatDisplayModMethodX;
      $OPS::Sav::ChatModY       = $pref::ChatDisplayModMethodY;
      $OPS::Sav::ChatX          = $pref::ChatDisplayX;
      $OPS::Sav::ChatY          = $pref::ChatDisplayY;
      $OPS::Sav::ChatWidth      = $pref::ChatDisplayWidth;
   }

   // -- nameplates (guistuff.acs.cs) -----------------------------------------
   $mj::greenlines       = "false";
   $mj::shownames        = "true";
   $mj::showhpbars       = "true";
   $mj::showjetbars      = "false";
   $mj::showhptext       = "false";
   $mj::barscrouch       = "false";
   $mj::passhelper       = "true";
   $mj::passhelpermm     = "true";
   $mj::fontdefault      = "sf_white_10b.pft";
   $mj::fontpass         = "if_g_10b.pft";
   $mj::bar_width        = "25";
   $mj::bar_height       = "5";
   $mj::bar_border_width = "1";

   // -- the chat log: absolute y 6, 370 wide; X is centred per screen width in
   // OPS::placeChat (the first draw knows the screen).
   $pref::ChatDisplayModMethodX = "1";
   $pref::ChatDisplayModMethodY = "2";
   $pref::ChatDisplayY          = "6";
   $pref::ChatDisplayWidth      = "370";
   $OPS::ChatForW = "";
   // Opsaya's chat is 5 lines (61px) -- the height the vitals+clock stack (61px) and the
   // flag panel are drawn to match. The line count is a per-HUD player setting
   // (FearGuiChatDisplay applyUserChatSize); without an OPS entry the chat kept whatever
   // the previous HUD left, measured at ~680px. Seed OPS's own entry once, so a player
   // who resizes the chat under OPS keeps their size.
   if($pref::HudSlotSize::ops::chatDisplayHud::lines == "")
      $pref::HudSlotSize::ops::chatDisplayHud::lines = "5";
   if($pref::HudSlotSize::ops::chatDisplayHud::width == "")
      $pref::HudSlotSize::ops::chatDisplayHud::width = "370";
   OPS::chatFontApply();
}

// Centre the chat once per screen width. Only on a width CHANGE, so a player who
// drags the chat keeps it there (the drag writes $pref::ChatDisplayX itself).
function OPS::placeChat(%screen)
{
   %sw = getWord(%screen, 0);
   if(%sw <= 0 || $OPS::ChatForW == %sw)
      return;
   $OPS::ChatForW = %sw;
   $pref::ChatDisplayX = floor((%sw - OPS::chatW()) / 2);
}

//------------------------------------------------------------------------------
// Chat FONT (Joe 2026-09-24: "use our scriptGL for the chat font -- whichever font we
// have that is closest; we are deprecating the old pft fonts"). Opsaya's chat was the
// stock 1998 log: the *_10b.pft bitmaps (FearGuiChatDisplay.cpp), a 10px bold narrow
// sans, ~11px a line, so 5 lines filled the 61px strip the vitals and flag panel are
// drawn to. The chat already renders through ScriptGL TrueType text in every HUD; its
// face/size/weight are three GLOBAL prefs. Closest installed face: Tahoma (then Verdana,
// then Segoe UI), 11px (the engine's floor), semibold.
//
// Global prefs, so OPS switches them on load and puts the player's back on unload --
// and REMEMBERS a change the player makes while under OPS ($pref::OPS::Chat*), so it
// is not overwritten at the next load. $pref::OPS::ChatOn persists "OPS holds the
// chat font", so quitting with OPS active does not later "restore" OPS's own font as
// the player's.
//------------------------------------------------------------------------------
function OPS::chatFace()
{
   if(glFontExists("Tahoma") != "")  return "Tahoma";
   if(glFontExists("Verdana") != "") return "Verdana";
   return "Segoe UI";
}

function OPS::chatFontApply()
{
   if($pref::OPS::ChatOn != 1)
   {
      $pref::OPS::ChatOn       = 1;
      $pref::OPS::PrevChatFont = $pref::ChatFont;
      $pref::OPS::PrevChatSize = $pref::ChatFontSize;
      $pref::OPS::PrevChatBold = $pref::ChatFontBold;
   }
   if($pref::OPS::ChatFont == "")
   {
      $pref::OPS::ChatFont = OPS::chatFace();
      $pref::OPS::ChatSize = "11";
      $pref::OPS::ChatBold = "1";
   }
   $pref::ChatFont     = $pref::OPS::ChatFont;
   $pref::ChatFontSize = $pref::OPS::ChatSize;
   $pref::ChatFontBold = $pref::OPS::ChatBold;
}

function OPS::chatFontRestore()
{
   if($pref::OPS::ChatOn != 1)
      return;
   $pref::OPS::ChatFont = $pref::ChatFont;        // keep the player's OPS choice
   $pref::OPS::ChatSize = $pref::ChatFontSize;
   $pref::OPS::ChatBold = $pref::ChatFontBold;
   $pref::ChatFont      = $pref::OPS::PrevChatFont;
   $pref::ChatFontSize  = $pref::OPS::PrevChatSize;
   $pref::ChatFontBold  = $pref::OPS::PrevChatBold;
   $pref::OPS::ChatOn   = 0;
}

function ModernHUDPack::restore()
{
   OPS::chatFontRestore();
   if($OPS::Saved == "")
      return;

   $mj::greenlines       = $OPS::Sav::greenlines;
   $mj::shownames        = $OPS::Sav::shownames;
   $mj::showhpbars       = $OPS::Sav::showhpbars;
   $mj::showjetbars      = $OPS::Sav::showjetbars;
   $mj::showhptext       = $OPS::Sav::showhptext;
   $mj::barscrouch       = $OPS::Sav::barscrouch;
   $mj::passhelper       = $OPS::Sav::passhelper;
   $mj::passhelpermm     = $OPS::Sav::passhelpermm;
   $mj::fontdefault      = $OPS::Sav::fontdefault;
   $mj::fontpass         = $OPS::Sav::fontpass;
   $mj::bar_width        = $OPS::Sav::bar_width;
   $mj::bar_height       = $OPS::Sav::bar_height;
   $mj::bar_border_width = $OPS::Sav::bar_border;

   $pref::ChatDisplayModMethodX = $OPS::Sav::ChatModX;
   $pref::ChatDisplayModMethodY = $OPS::Sav::ChatModY;
   $pref::ChatDisplayX          = $OPS::Sav::ChatX;
   $pref::ChatDisplayY          = $OPS::Sav::ChatY;
   $pref::ChatDisplayWidth      = $OPS::Sav::ChatWidth;

   deleteVariables("$OPS::Sav::*");
   $OPS::Saved = "";
}

//------------------------------------------------------------------------------
// Pack lifecycle.
//------------------------------------------------------------------------------
function ModernHUDPack::prefs()
{
   $pref::miniMapAlpha   = "1";
   $pref::miniMapRotate  = "True";
   $pref::miniMapSquare  = "True";
   $pref::miniMapWidth   = "302";
   $pref::miniMapZoom    = "1.75";
}

function ModernHUDPack::stockHuds()
{
   // The WHOLE set (see vantage/hud.cs): Opsaya's play.gui keeps only the
   // crosshair, the chat log and the minimap.
   Control::SetVisible(crosshairHud,   true);
   ModernHUD::stock(chatDisplayHud, true);
   ModernHUD::stock(Minimap,        true);
   ModernHUD::stock(clockHud,       false);
   ModernHUD::stock(healthHud,      false);
   ModernHUD::stock(jetPackHud,     false);
   ModernHUD::stock(weaponHud,      false);
   ModernHUD::stock(compassHud,     false);
   ModernHUD::stock(sensorHUD,      false);
   Control::SetVisible(reticleCompass, false);
}

function ModernHUDPack::detachRetained()
{
   // OPS replaces no retained container.
}

function ModernHUDPack::init()
{
   OPS::apply();
}

function ModernHUDPack::draw(%screen)
{
   OPS::placeChat(%screen);

   if(ModernHUDPack::ownsSlot($pref::HudSlot::healthenergy))
      OPS::draw_vitals(%screen);
   else
      ModernHUD::hide("ModernHUD::OpsVitals");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::clock))
      OPS::draw_clock(%screen);
   else
      ModernHUD::hide("ModernHUD::OpsClock");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::ctf))
      OPS::draw_ctf(%screen);
   else
      ModernHUD::hide("ModernHUD::OpsCtf");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::enemy))
      OPS::draw_enemy(%screen);
   else
      ModernHUD::hide("ModernHUD::OpsEnemy");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::weapon))
      OPS::draw_weapons(%screen);
   else
      ModernHUD::hide("ModernHUD::OpsWeapons");
}

// GrabSpeed.acs.cs: the same bottom print when YOU take a flag. The original hung
// off its own config's eventFlagGrab; this client's flag events come from Presto's
// TeamTrak as eventFlagTaken(teamFlag, client).
function OPS::grabSpeed(%team, %cl)
{
   if(%cl == getManagerId())
      remoteBP(2048, "<JC><F2>You <F1>grabbed going <F2>" @ $Speed @ " <F1>m/s!", 3);
}

function ModernHUDPack::onPlayGuiOpen()
{
   Schedule::Add("ModernHUDPack::stockHuds();", 0);
}

//------------------------------------------------------------------------------
// K panel: only settings the PACK owns. A row here is a HUD setting with a declared
// default -- the Configs tab calls the HUD "MODIFIED" when a row differs from it and
// "Reset" writes the default back (configModules.cpp CfgModules_factoryModified).
// Opsaya's in-game "Groove" menu (vGrooves.acs.cs) was all CLIENT-WIDE settings, so it
// does NOT belong here: first cut put five of them in this panel, and in-game
// verification (2026-09-24) showed OPS reading MODIFIED from the player's own shadow
// setting, with Reset about to rewrite it. Those five stay where client settings
// live: Options > 09 Advanced lists every $pref (IgnoreTargets, OOBGridPercent,
// shadowDetailMask/Scale). The rest of Groove was already in Options or is dead here.
//
// EnemyHUD's five "EnemyHUD Toggle n" binds: show or hide one row of the panel.
ModernHUD::setting("bool", "pref::OPS::EnemyRow1", "Enemy row 1", "1", "", "", "ModernHUD::OpsEnemy");
ModernHUD::setting("bool", "pref::OPS::EnemyRow2", "Enemy row 2", "1", "", "", "ModernHUD::OpsEnemy");
ModernHUD::setting("bool", "pref::OPS::EnemyRow3", "Enemy row 3", "1", "", "", "ModernHUD::OpsEnemy");
ModernHUD::setting("bool", "pref::OPS::EnemyRow4", "Enemy row 4", "1", "", "", "ModernHUD::OpsEnemy");
ModernHUD::setting("bool", "pref::OPS::EnemyRow5", "Enemy row 5", "1", "", "", "ModernHUD::OpsEnemy");
// The chat-font rows, registered HERE so commonSettings() skips its own (it checks
// hasSetting): same prefs, but OPS's defaults, so MODIFIED/Reset mean "vs OPS's
// Tahoma 11 semibold", not "vs Segoe UI auto". While OPS is active these prefs hold
// OPS's font (OPS::chatFontApply), so this is the one place a player changes it.
$OPS::Faces = ModernHUD::ttfSpec();
if($OPS::Faces != "")
   ModernHUD::setting("enum", "pref::ChatFont", "Chat font", OPS::chatFace(), $OPS::Faces, "", "chatDisplayHud");
ModernHUD::setting("int", "pref::ChatFontSize", "Chat font size (px, 0 = auto)", "11", "0|48|1", "", "chatDisplayHud");
ModernHUD::setting("enum", "pref::ChatFontBold", "Chat font weight", "1", "Semibold|1;Regular|0", "", "chatDisplayHud");

ModernHUD::attach("eventGuiOpen_PlayGui", "ModernHUDPack::onPlayGuiOpen");
ModernHUD::attach("eventFlagTaken", "OPS::grabSpeed");
ModernHUDPack::prefs();
ModernHUDPack::stockHuds();
ModernHUDPack::init();

// Font-scope Stage 3: load-completion sentinel -- MUST stay the final statement.
$ModernHUD::LoadComplete = "ops";
