// NATIVE-EDITOR (2026-08-22): user/extended palette entries. RegisterObjects.cs
// execs this at its end so the editor palette can grow without editing shipped
// scripts. Runs only in editing sessions (ME::init exec chain).

//----------------------------------------------------------------------------
// Weather: editor-placeable thunderstorm (weather.cs defines ME::AddStorm and
// the server-side strike loop; datablocks load from server.cs)
MissionRegObject( Sky, "Thunderstorm", ME::AddStorm );

//----------------------------------------------------------------------------
// Physical zones: SimVolumetric is the ESF water/lava volume -- currentVelocity
// pushes (river), density gives buoyancy, damagePerSec burns (lava; the
// inspector bug that zeroed it on read is fixed in the native build). The
// palette's plain "Volume" entry is the .vol ARCHIVE class, a different thing.
// The DML is what makes these LOOK like anything. With no material list,
// BoxRenderDmlImage::render takes its no-DML branch, which hardcodes a flat
// constant-colour cube -- the yellow slab. With one, it goes down the translucent
// textured path on the GPU and animates through the list.
//
// base\liquids\wzwater.dml  -- 40 frames, 128px
// base\liquids\wzlava.dml   --  5 frames, 32px source AI-upscaled to 128px
// The DMLs name .bmp; Material::load prefers the .png siblings we actually ship.
function ME::AddWaterZone()
{
   %obj = MissionCreateObject(WaterZone, SimVolumetric);
   focusServer();
   %obj.currentVelocity = "3 0 0";
   %obj.currentDrag = 1;
   %obj.density = 1;
   ME::SetVolumeDML(%obj, "wzwater.dml");
   focusClient();
}

function ME::AddLavaZone()
{
   %obj = MissionCreateObject(LavaZone, SimVolumetric);
   focusServer();
   %obj.damagePerSec = 5;
   // NATIVE-EDITOR (2026-08-23, Joe): lava is DENSE -- you should stop dead when
   // you fall in, sink slowly, and fight to fly out. Density just under the
   // player's 1.2 neutral = slow sink; high containerDrag = momentum eaten on
   // entry and a heavy exit. Density 2 made players pop out like corks.
   // Joe-tuned 2026-08-23 (in-game feel pass): stop dead on entry, sink slow,
   // heavy exit, swim-kick to escape.
   %obj.density = 1;
   %obj.containerDrag = 15;
   %obj.currentDrag = 0;
   ME::SetVolumeDML(%obj, "wzlava.dml");
   focusClient();
}

MissionRegObject( Mission, "Water Zone (current/buoyancy)", ME::AddWaterZone );
MissionRegObject( Mission, "Lava Zone (damage)", ME::AddLavaZone );

//----------------------------------------------------------------------------
// Interior auto-scan: register EVERY mounted .dis in the palette, grouped by
// first letter. File::findFirst/findNext walk the whole mounted resource list
// (all zips), so this sees every interior pack that is installed. Mission-lit
// per-instance variants (<base>.<n>.dis) are skipped.

function ME::RegisterAllInteriors()
{
   %count = 0;
   %file = File::findFirst("*.dis");
   while(%file != "" && %count < 900)
   {
      %base = File::getBase(%file);
      %skip = false;
      // skip mission-lit per-instance clones: base name ends in .0 - .9
      for(%d = 0; %d <= 9; %d++)
         if(String::ends(%base, "." @ %d))
            %skip = true;
      if(!%skip)
      {
         %letter = String::toUpper(String::char(%base, 0));
         MissionRegDis("Interiors " @ %letter, %base);
         %count++;
      }
      %file = File::findNext("*.dis");
   }
   echo("ME: palette scan registered " @ %count @ " interiors");
}

ME::RegisterAllInteriors();
