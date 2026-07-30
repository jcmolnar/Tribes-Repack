// ModernHUD pixel-diff capture (deployed as config/ModernHUD/Tools/capture.cs)
//
// ★"Exactly the same look" must be MEASURED, not judged.★ This captures the
// before/after pair the diff tool compares: for each pack, one frame of the
// LEGACY retained HUD and one frame of its CONVERTED ModernHUD counterpart, at
// the resolution the client is currently running.
//
// Run it in game, standing somewhere with a live HUD (in a mission, spawned --
// an empty main menu proves nothing about a HUD):
//
//     exec("ModernHUD/Tools/capture.cs");
//     MHCapture::run();                 // every installed pack, both variants
//     MHCapture::run("basic");          // just one
//
// Then, at a shell:
//
//     python tools/modernhud_pixeldiff.py --resolution 1920x1080
//
// ★Do it at BOTH 1920x1080 and 800x600.★ 1920x1080 is the fidelity check -- the
// packs were authored there, so the converted part should land on the original.
// 800x600 is where the legacy absolute-pixel bugs live (a 405x405 container over
// half the screen), so there the converted HUD should be BETTER than the original,
// not identical: expect differences and read them, do not chase them to zero.
//
// A pack swap is deferred by design (the heavy load runs a frame after the menu
// closes and the play HUD is the live surface again), so every step here waits
// rather than assuming. Chained schedules, not a loop: the console has no sleep.

$MHCapture::Delay = 1.5;      // seconds between load and shot; a swap needs frames

function MHCapture::run(%only)
{
   $MHCapture::Only = %only;
   $MHCapture::Index = 0;
   $MHCapture::Legacy = "";
   $MHCapture::Restore = $Config::Name;
   echo("[MHCAP] capture start; will restore '" @ $MHCapture::Restore @ "' when done");
   MHCapture::next();
}

// Legacy folder name for a converted pack id. The manifest records this as
// source.pack; the table here is the same mapping the converter used, kept
// explicit so a capture never guesses which legacy pack a conversion came from.
function MHCapture::legacyOf(%id)
{
   if(%id == "basic")     return "Basic";
   if(%id == "proconfig") return "ProConfigVol4-1.41";
   if(%id == "vodka")     return "Tribes - Minimalist - v0dkA";
   if(%id == "xloader")   return "Tribes 1.40.655 xLoader";
   if(%id == "overstep")  return "Tribes_Overstep";
   return "";
}

function MHCapture::next()
{
   %id = ModernHUD::packAt($MHCapture::Index);
   if(%id == "")
   {
      echo("[MHCAP] done; restoring '" @ $MHCapture::Restore @ "'");
      Config::apply($MHCapture::Restore);
      return;
   }
   $MHCapture::Index++;

   if($MHCapture::Only != "" && $MHCapture::Only != %id)
   {
      MHCapture::next();
      return;
   }

   $MHCapture::Id = %id;
   echo("[MHCAP] pack " @ %id @ ": loading LEGACY " @ MHCapture::legacyOf(%id));
   Config::apply(MHCapture::legacyOf(%id));
   schedule("MHCapture::shotLegacy();", $MHCapture::Delay);
}

function MHCapture::shotLegacy()
{
   ModernHUD::shot($MHCapture::Id @ "-legacy");
   echo("[MHCAP] pack " @ $MHCapture::Id @ ": loading CONVERTED");
   Config::apply("ModernHUD:" @ $MHCapture::Id);
   schedule("MHCapture::shotModern();", $MHCapture::Delay);
}

function MHCapture::shotModern()
{
   ModernHUD::shot($MHCapture::Id @ "-modern");
   schedule("MHCapture::next();", 0.2);
}
