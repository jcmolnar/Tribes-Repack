// HAND-AUTHORED pack (authoring: manual) -- the generator does not own this file.
// Graduated from CustomConfigs\Tribes_Overstep\config\modern.hud.cs by Phase 3:
// same script, but living under Packs/overstep with its art imported to
// Assets/Packs/overstep so it no longer needs the legacy tree on the search path.
// Overstep Phase A image/animation gate.
//
// This is intentionally a small, measurable slice, not the final pack. It
// proves native PNG alpha, GIF animation, content-sized layout, per-draw fade
// and per-digit number art before the other HUD parts are converted.

exec("ModernHUD/Framework.cs");
exec("ModernHUD/Packs/overstep/components.cs");   // borrowable parts (HUD designer mix-and-match)

$ModernHUD::Enabled = true;
$ModernHUD::Pack = "Tribes_Overstep";
// Phase 1 identity: every position/scale key is qualified with this.
$ModernHUD::PackId = "overstep";

function ModernHUDPack::noop()
{
}

// A legacy module borrowed from another pack owns the selected slot until the
// player changes it again.  ModernHUD is immediate-mode, so hiding/unloading
// retained controls alone cannot disable one of its parts: the draw dispatch
// must yield that slot explicitly.
function ModernHUDPack::ownsSlot(%value)
{
   if(%value == "")
      return true;
   if(%value == "off")
      return false;
   return String::findSubStr(%value, "Tribes_Overstep::") == 0;
}

function ModernHUDPack::hideHandle(%name)
{
   %handle = $ModernHUD::Handle[%name];
   if(isObject(%handle))
      Control::SetVisible(%handle, false);
}

function ModernHUDPack::detachRetained()
{
   // No retained client HUDs or Presto scheduler exist on a dedicated server; the
   // six Schedule::Cancel calls below each logged "Unknown command." there.
   // (TribesMod handoff, 2026-09-06.)
   if($dedicated)
      return;

   %status = ModernHUDPack::ownsSlot($pref::HudSlot::healthenergy);
   %weapon = ModernHUDPack::ownsSlot($pref::HudSlot::weapon);
   %items = ModernHUDPack::ownsSlot($pref::HudSlot::items);
   %ctf = ModernHUDPack::ownsSlot($pref::HudSlot::ctf);
   %clock = ModernHUDPack::ownsSlot($pref::HudSlot::clock);
   %score = ModernHUDPack::ownsSlot($pref::HudSlot::ratings);

   if(%status)
   {
      Schedule::Cancel("GHealth::Update();");
      Schedule::Cancel("GSpeed::Update();");
      Schedule::Cancel("GEnergy::Update();");
   }
   if(%weapon)
      Schedule::Cancel("WH::Update();");
   if(%items)
      Schedule::Cancel("ItemHUD::Update();");
   // Hiding these in the immediate draw callback is too late: retained GUI
   // rendering and K-editor hit-testing have already happened. Detach the
   // converted containers from playGui so only their data-producing scripts
   // remain and there is exactly one movable identity per component.
   //
   // SetVisible(false) must happen BEFORE removeFromSet: it contributes the
   // control's old rectangle to the canvas damage list while it still has a
   // root. Removing first clears root, so the old retained pixels can never
   // be invalidated and appear as a frozen second HUD.
   if(%status)
   {
      Control::SetVisible("GHealth::Container", false);
      Control::SetVisible("GSpeed::Container", false);
      Control::SetVisible("GEnergy::Container", false);
      removeFromSet(playGui, "GHealth::Container");
      removeFromSet(playGui, "GSpeed::Container");
      removeFromSet(playGui, "GEnergy::Container");
      // The repair-kit "+" is drawn by drawStatus now (on top of the health plate,
      // where it belongs); its retained control has to go with the rest of the
      // status group or both would be on screen at once.
      Schedule::Cancel("aaRepKitHUD::Update();");
      Control::SetVisible("aaRepKitHUD::Container", false);
      removeFromSet(playGui, "aaRepKitHUD::Container");
   }
   if(%weapon)
   {
      Control::SetVisible("WeaponHUD::Container", false);
      removeFromSet(playGui, "WeaponHUD::Container");
   }
   if(%items)
   {
      Control::SetVisible("ItemHUD::Container", false);
      removeFromSet(playGui, "ItemHUD::Container");
   }
   if(%ctf)
   {
      Control::SetVisible("CtfHUD::Container", false);
      removeFromSet(playGui, "CtfHUD::Container");
   }
   if(%clock)
   {
      Control::SetVisible("clock::Container", false);
      removeFromSet(playGui, "clock::Container");
   }
   if(%score)
   {
      Control::SetVisible("RatingsHUD::Container", false);
      removeFromSet(playGui, "RatingsHUD::Container");
   }
   Control::SetVisible("HUDOverlay::Container", false);
   removeFromSet(playGui, "HUDOverlay::Container");
}

// The STOCK huds this pack turns on and off, from its own play.gui.cs.
//
// ★A pack must state its whole stock-HUD set, not just the ones it wants.★ Our
// client never executed any pack's play.gui.cs -- it is parsed for text only -- so
// stock visibility was simply inherited from whichever pack loaded before. Once the
// GENERATED packs began applying theirs, this hand-authored one became the hole:
// selecting xLoader turned its stock huds on, and coming back to Overstep left them
// on, because nothing here ever turned them off. Reported live.
//
// Visibility only. Placement rides these controls' own $pref::hudPositions, which the
// engine re-anchors; their play.gui.cs geometry is absolute pixels against this
// pack's 2560x1440 canvas and would be wrong on any other screen.
function ModernHUDPack::stockHuds()
{
   Control::SetVisible(crosshairHud, true);
   ModernHUD::stock(clockHud, false);
   ModernHUD::stock(sensorHUD, false);
   ModernHUD::stock(compassHud, false);
   ModernHUD::stock(jetPackHud, false);
   ModernHUD::stock(healthHud, false);
   ModernHUD::stock(weaponHud, false);
   Control::SetVisible(ChatDisplayHUD, true);
   // Chat/minimap resize handoff: this pack drives chat visibility directly
   // (no settings row), so it never passed the stock() chokepoint and the chat
   // was NOT an editor target here. Explicit registration, visibility logic
   // unchanged.
   ModernHUD::editTarget(ChatDisplayHUD);
   ModernHUD::stock(Minimap, true);
   Control::SetVisible(reticleCompass, false);
}

// The shared team/timer data layer this pack's CTF readout reads through.
//
// ★Every other pack requires these; this hand-authored one did not.★ The CTF
// part calls Team::Friendly, Team::Enemy, Team::Score, Team::Flag::Location and
// Team::Flag::Timer from the per-frame render hook. Presto's TeamTrak.cs happens
// to define the first two, so the gap was invisible on a Presto server and total
// everywhere else -- the other three resolved to nothing.
//
// An undefined call is not silent: eval.cpp returns the STRING "False" and logs
// "<name>: Unknown command." So the miss cost two console lines per frame AND
// fed "False" to the digit art, which is why the log carried five
// "image load FAILED 'Modules/numHUD/Black/{F,a,l,s,e}.png'" lines. A user
// console.log came back 30 MB with 850,910 of its 860,673 lines from this.
// Team.cs calls Timer::FormatSeconds/New/Dec, so it does not stand alone.
ModernHUD::require("ModernHUD/Core/Data/Team.cs");
ModernHUD::require("ModernHUD/Core/Data/Timer.cs");

// The pack's own stock-HUD PREFERENCES, carried from its ClientPrefs.cs.
//
// ★A pack must state its minimap the same way it states stock visibility.★ Every
// GENERATED pack emits a prefs() with its $pref::miniMap* values; this hand-authored
// one had none, so the minimap was simply whatever the PREVIOUS config left behind --
// the identical predecessor-dependent hole that stockHuds() above exists to close, and
// it is just as invisible when the previous pack happens to agree.
//
// Measured in the parity run: 14.86% of the frame differed OUTSIDE every declared part,
// almost all of it the minimap drawn at a different size. These are Overstep's own
// authored values (CustomConfigs\Tribes_Overstep\config\ClientPrefs.cs:87-92), so this
// makes explicit what the pack always intended rather than changing its look.
function ModernHUDPack::prefs()
{
   $pref::miniMapAlpha = "1";
   $pref::miniMapAutosize = "False";
   $pref::miniMapRotate = "True";
   $pref::miniMapSquare = "False";
   $pref::miniMapWidth = "302";
   $pref::miniMapZoom = "1.75";
}

// ★Bound to eventGuiOpen_PlayGui, NOT eventGuiOpen plus a gui-name test.★
// TWO independent firers raise eventGuiOpen with DIFFERENT spellings: the
// ENGINE, with the control's real name -- `playGui` (simGuiCanvas.cpp:907,
// kronosFireEvent1) -- and PRESTO, with a hardcoded bare word -- `PlayGui`
// (Presto/events.cs:681 via OpenAGui(PlayGui) at :737). A string VALUE
// comparison is case-SENSITIVE even though NAME lookup is not (compare() falls
// through to strcmp for two non-numeric strings, eval.cpp), so the old
// `%gui == "playGui"` test matched the engine's spelling and ignored Presto's.
// It WORKED -- verified live: oldSeen=playGui, oldHits=1 per transition -- but
// only because the two spellings happen to differ. Presto fires an
// argument-free eventGuiOpen_PlayGui from the same function (events.cs:746);
// binding that removes the string test altogether, at the same frequency
// (verified newHits=1). A robustness change, not a bug fix.
function ModernHUDPack::onPlayGuiOpen()
{
   Schedule::Add("ModernHUDPack::detachRetained();", 0);
   // Re-applied per gui open: the stock controls are recreated with the gui, so
   // setting them once at load does not survive the first transition.
   Schedule::Add("ModernHUDPack::stockHuds();", 0);
}

function ModernHUDPack::draw(%screen)
{
   ModernHUDPack::detachRetained();

   // Phase 3b parts. No ownsSlot gate: these three have no competing implementation
   // in any other pack, so there is nothing to yield to -- adding a gate would only
   // create a slot the picker would list with a single entry.
   Overstep::drawRepKit(%screen);
   Overstep::drawLowHealth(%screen);
   // Same reasoning: no other pack implements a flag banner or a toasty, so there
   // is no slot to yield. Both early-out unless their event has armed them, so the
   // cost when idle is one variable read each.
   Overstep::drawFlagPopup(%screen);
   Overstep::drawToasty(%screen);

   if(ModernHUDPack::ownsSlot($pref::HudSlot::minimap))
      Overstep::drawMinimapFrame(%screen);

   // CTF and clock share one authored Overstep plate. If either half is
   // replaced, yield the composite so the borrowed control is never covered.
   if(ModernHUDPack::ownsSlot($pref::HudSlot::ctf) &&
      ModernHUDPack::ownsSlot($pref::HudSlot::clock))
      Overstep::drawCtfClock(%screen);
   else
      ModernHUDPack::hideHandle("ModernHUD::CtfClock");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::healthenergy))
      Overstep::drawStatus(%screen);
   else
   {
      ModernHUDPack::hideHandle("ModernHUD::Health");
      ModernHUDPack::hideHandle("ModernHUD::Speed");
      ModernHUDPack::hideHandle("ModernHUD::Energy");
   }

   if(ModernHUDPack::ownsSlot($pref::HudSlot::weapon))
      Overstep::drawWeapons(%screen);
   else
      ModernHUDPack::hideHandle("ModernHUD::Weapons");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::items))
      Overstep::drawItems(%screen);
   else
      ModernHUDPack::hideHandle("ModernHUD::Items");

   if(ModernHUDPack::ownsSlot($pref::HudSlot::ratings))
      Overstep::drawScore(%screen);
   else
      ModernHUDPack::hideHandle("ModernHUD::Score");
}

// ModernHUD::attach, not a raw Event::Attach: the framework revokes tracked
// handlers in detachAll() on unload, so this cannot outlive its own pack.
ModernHUD::attach("eventGuiOpen_PlayGui", "ModernHUDPack::onPlayGuiOpen");
ModernHUDPack::stockHuds();
ModernHUDPack::detachRetained();

//----------------------------------------------------------------------------
// FlagPopup -- the 1560x67 banner shown when YOU take a flag.
// Converted from CustomConfigs\Tribes_Overstep\config\Modules\FlagPopup\flagpopup.acs.cs
//
// ★The fade is arithmetic on the clock, not a timer.★ Legacy showed the banner,
// scheduled FadeOut at $popupTime, and then FadeOut rescheduled ITSELF every
// 0.006s stepping $fadeOutCount 100 -> 0 purely to walk %fade.alpha down. That is
// ~167 console evaluations a second whose only job is to make a number decrease.
// getSimTime() already increases; alpha is a pure function of it, so the entire
// loop collapses into three lines in the draw and leaves no schedule to leak.
//
// Faithful details kept: 5s hold at full opacity ($popupTime), ~0.6s fade
// (100 steps x 0.006s), only for the local player, and the legacy Drop/Cap reset
// condition -- which fires when the flag whose team is NOT mine leaves my hands.
//----------------------------------------------------------------------------
$Overstep::FlagPopupHold = 5000;    // legacy $popupTime = 5 seconds
$Overstep::FlagPopupFade = 600;     // legacy 100 steps x 0.006s

//----------------------------------------------------------------------------
// ToastyHUD -- Dan Forden slides in from the right on a 50m+ mid-air.
// Converted from CustomConfigs\Tribes_Overstep\config\Modules\ToastyHUD\ToastyHUD.acs.cs
//
// ★The slide is derived from the clock too.★ Legacy animated by rescheduling
// ToastyHUD::Animate every 0.0002s -- a 5000Hz timer walking a counter to 400 and
// assigning %obj.position each step. Same reasoning as the fade above, but far
// worse: it also mutated a retained control's position, which is exactly the
// second movable identity the framework exists to remove.
//
// ★DEVIATION (logged): String::explode -> String::Explode.★ The legacy parser
// called String::explode, which the engine does not register under any casing, so
// $player[0]/$player[1] were always empty, %shooter/%victim never matched
// $PCFG::Name, and this feature has never once triggered in a shipped Overstep.
// Converting it faithfully would mean converting dead code. Same fix in the praise
// list used by the centre print.
//
// Not carried across: the screenshot mode. It worked by walking playGui's children
// and hiding every control except its own, which has no meaning for immediate-mode
// parts that are not playGui children. See CONVERSION_NOTES.md.
//----------------------------------------------------------------------------
$Overstep::ToastyWidth = 401;       // the shipped toasty.png is 401x401
$Overstep::ToastySlide = 200;       // ms for the slide in/out
$Overstep::ToastyHold  = 800;       // ms fully on screen (legacy: anim in at
                                         // 0.2s, out at 1.0s)
$Overstep::ToastyMinMeters = 50;    // legacy $MA_METER_DIST

// ★One-time: forget the part sizes saved before sizes did anything here.★ Until the HUD
// designer's stage 6 this pack's parts never drew at their saved size (Overstep::handle had no
// glPartScale), so a resize changed only the invisible grab box -- and a player who scrolled
// a part smaller saw nothing happen and kept going, down to the 0.25 floor (found in a real
// ClientPrefs: CtfClock at 0.25). Applying those now would shrink parts nobody chose to
// shrink. They were never visible, so clearing them loses nothing. The legacy claim marker is
// set too, or the unqualified pre-Phase-1 value would be claimed straight back
// (Framework.cs ModernHUD::claimLegacyLayout).
function Overstep::forgetInvisibleSizes()
{
   if($pref::ModernHUD::overstep::SizesLive == "1")
      return;
   %n = "Health Speed Energy Weapons Items CtfClock Score RepKit LowHealth FlagPopup Toasty";
   for(%i = 0; (%w = getWord(%n, %i)) != -1; %i++)
   {
      %name = "ModernHUD::" @ %w;
      ModernHUD::settingSet("pref::hudScale" @ ModernHUD::qualify(%name), "");
      if($pref::ModernHUD::LegacyClaimed[%name] == "")
         $pref::ModernHUD::LegacyClaimed[%name] = "overstep";
   }
   $pref::ModernHUD::overstep::SizesLive = "1";
}
Overstep::forgetInvisibleSizes();

ModernHUDPack::prefs();
ModernHUD::legacyMap();

// Font-scope Stage 3: load-completion sentinel -- MUST stay the final statement.
$ModernHUD::LoadComplete = "overstep";
