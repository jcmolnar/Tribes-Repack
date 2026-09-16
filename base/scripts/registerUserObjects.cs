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
// Tribes 2 buildings: GENERATED from the Asset Store's 'tribes2' pack (store gen 16, pack v2).
// The interior scan below files these under "Tribes 2 <letter>" instead of "Interiors <letter>".
// It only registers buildings it actually finds, so without the pack this table changes nothing.
// Regenerate when the pack's buildings change: one line per .dis in the pack's store.json entry,
// in lower case.
$ME::T2Interior["bbase1"] = 1;
$ME::T2Interior["bbase4cm"] = 1;
$ME::T2Interior["bbase6"] = 1;
$ME::T2Interior["bbase7"] = 1;
$ME::T2Interior["bbase9"] = 1;
$ME::T2Interior["bbase_ccb5"] = 1;
$ME::T2Interior["bbase_nefhillside"] = 1;
$ME::T2Interior["bbrdg0"] = 1;
$ME::T2Interior["bbrdg1"] = 1;
$ME::T2Interior["bbrdg2"] = 1;
$ME::T2Interior["bbrdg3"] = 1;
$ME::T2Interior["bbrdg4"] = 1;
$ME::T2Interior["bbrdg5"] = 1;
$ME::T2Interior["bbrdg6"] = 1;
$ME::T2Interior["bbrdg7"] = 1;
$ME::T2Interior["bbrdg8"] = 1;
$ME::T2Interior["bbrdg9"] = 1;
$ME::T2Interior["bbrdga"] = 1;
$ME::T2Interior["bbrdgb"] = 1;
$ME::T2Interior["bbrdgn"] = 1;
$ME::T2Interior["bbrdgo"] = 1;
$ME::T2Interior["bbunk1"] = 1;
$ME::T2Interior["bbunk2"] = 1;
$ME::T2Interior["bbunk5"] = 1;
$ME::T2Interior["bbunk7"] = 1;
$ME::T2Interior["bbunk8"] = 1;
$ME::T2Interior["bbunk9"] = 1;
$ME::T2Interior["bbunkb"] = 1;
$ME::T2Interior["bbunkc"] = 1;
$ME::T2Interior["bbunkd"] = 1;
$ME::T2Interior["bbunke"] = 1;
$ME::T2Interior["bmisc1"] = 1;
$ME::T2Interior["bmisc2"] = 1;
$ME::T2Interior["bmisc3"] = 1;
$ME::T2Interior["bmisc4"] = 1;
$ME::T2Interior["bmisc5"] = 1;
$ME::T2Interior["bmisc6"] = 1;
$ME::T2Interior["bmisc7"] = 1;
$ME::T2Interior["bmisc8"] = 1;
$ME::T2Interior["bmisc9"] = 1;
$ME::T2Interior["bmisc_-nef_flagstand1_x"] = 1;
$ME::T2Interior["bmisc_nefledge1"] = 1;
$ME::T2Interior["bmisc_neftrstand1"] = 1;
$ME::T2Interior["bmisc_nefvbay"] = 1;
$ME::T2Interior["bplat1"] = 1;
$ME::T2Interior["bplat2"] = 1;
$ME::T2Interior["bplat3"] = 1;
$ME::T2Interior["bplat4"] = 1;
$ME::T2Interior["bplat6"] = 1;
$ME::T2Interior["bpower1"] = 1;
$ME::T2Interior["brock6"] = 1;
$ME::T2Interior["brock7"] = 1;
$ME::T2Interior["brock8"] = 1;
$ME::T2Interior["brocka"] = 1;
$ME::T2Interior["brockc"] = 1;
$ME::T2Interior["bspir1"] = 1;
$ME::T2Interior["bspir2"] = 1;
$ME::T2Interior["bspir3"] = 1;
$ME::T2Interior["bspir4"] = 1;
$ME::T2Interior["bspir5"] = 1;
$ME::T2Interior["btf_turretplatform_c"] = 1;
$ME::T2Interior["btowr2"] = 1;
$ME::T2Interior["btowr5"] = 1;
$ME::T2Interior["btowr6"] = 1;
$ME::T2Interior["btowr8"] = 1;
$ME::T2Interior["btowra"] = 1;
$ME::T2Interior["bvpad"] = 1;
$ME::T2Interior["bwall1"] = 1;
$ME::T2Interior["bwall2"] = 1;
$ME::T2Interior["bwall3"] = 1;
$ME::T2Interior["bwall4"] = 1;
$ME::T2Interior["cannon"] = 1;
$ME::T2Interior["cannon2"] = 1;
$ME::T2Interior["cap"] = 1;
$ME::T2Interior["dbase2"] = 1;
$ME::T2Interior["dbase3"] = 1;
$ME::T2Interior["dbase4"] = 1;
$ME::T2Interior["dbase_broadside_nef"] = 1;
$ME::T2Interior["dbase_neffloat1"] = 1;
$ME::T2Interior["dbase_neffloat2"] = 1;
$ME::T2Interior["dbase_neficeridge"] = 1;
$ME::T2Interior["dbase_nefraindance"] = 1;
$ME::T2Interior["dbrdg1"] = 1;
$ME::T2Interior["dbrdg10"] = 1;
$ME::T2Interior["dbrdg11"] = 1;
$ME::T2Interior["dbrdg2"] = 1;
$ME::T2Interior["dbrdg3"] = 1;
$ME::T2Interior["dbrdg3a"] = 1;
$ME::T2Interior["dbrdg4"] = 1;
$ME::T2Interior["dbrdg5"] = 1;
$ME::T2Interior["dbrdg6"] = 1;
$ME::T2Interior["dbrdg7"] = 1;
$ME::T2Interior["dbrdg7a"] = 1;
$ME::T2Interior["dbrdg8"] = 1;
$ME::T2Interior["dbrdg9"] = 1;
$ME::T2Interior["dbrdg9a"] = 1;
$ME::T2Interior["dbunk5"] = 1;
$ME::T2Interior["dbunk6"] = 1;
$ME::T2Interior["dbunk_nef_invbunk1"] = 1;
$ME::T2Interior["dbunk_nefcliffside"] = 1;
$ME::T2Interior["dbunk_nefdcbunk"] = 1;
$ME::T2Interior["dbunk_nefsmall"] = 1;
$ME::T2Interior["dbunk_snowblind"] = 1;
$ME::T2Interior["dbunk_stonehenge1"] = 1;
$ME::T2Interior["dbunk_vbunk1"] = 1;
$ME::T2Interior["dmisc1"] = 1;
$ME::T2Interior["dmisc_nefbridge"] = 1;
$ME::T2Interior["dmisc_nefflagstand2"] = 1;
$ME::T2Interior["dmisc_nefflagstand3"] = 1;
$ME::T2Interior["dmisc_nefobj1"] = 1;
$ME::T2Interior["dmisc_nefobj2"] = 1;
$ME::T2Interior["dmisc_nefplat1"] = 1;
$ME::T2Interior["dmisc_nefplug1"] = 1;
$ME::T2Interior["dmisc_nefrdbridge1"] = 1;
$ME::T2Interior["dmisc_neftower1"] = 1;
$ME::T2Interior["dmisc_neftower2"] = 1;
$ME::T2Interior["dmisc_neftower3"] = 1;
$ME::T2Interior["dmisc_stonehenge1"] = 1;
$ME::T2Interior["dmisc_stonehenge2"] = 1;
$ME::T2Interior["dmisc_stonehenge3"] = 1;
$ME::T2Interior["doubleramp2"] = 1;
$ME::T2Interior["dplat1"] = 1;
$ME::T2Interior["dplat2"] = 1;
$ME::T2Interior["dplat3"] = 1;
$ME::T2Interior["dpole1"] = 1;
$ME::T2Interior["drock6"] = 1;
$ME::T2Interior["drock7"] = 1;
$ME::T2Interior["drock8"] = 1;
$ME::T2Interior["drocka"] = 1;
$ME::T2Interior["dspir1"] = 1;
$ME::T2Interior["dspir2"] = 1;
$ME::T2Interior["dspir3"] = 1;
$ME::T2Interior["dspir4"] = 1;
$ME::T2Interior["dspir5"] = 1;
$ME::T2Interior["dtowr1"] = 1;
$ME::T2Interior["dtowr2"] = 1;
$ME::T2Interior["dtowr4"] = 1;
$ME::T2Interior["dtowr_classic1"] = 1;
$ME::T2Interior["dvent"] = 1;
$ME::T2Interior["dvpad"] = 1;
$ME::T2Interior["dvpad1"] = 1;
$ME::T2Interior["dwall1"] = 1;
$ME::T2Interior["flagbridge"] = 1;
$ME::T2Interior["infbutch_blackairinv13"] = 1;
$ME::T2Interior["infbutch_blackbase5618_final"] = 1;
$ME::T2Interior["infbutch_blackturret8"] = 1;
$ME::T2Interior["nef_bowl1"] = 1;
$ME::T2Interior["nef_bowl2"] = 1;
$ME::T2Interior["nef_bowl3"] = 1;
$ME::T2Interior["nef_ramp1"] = 1;
$ME::T2Interior["pbase3"] = 1;
$ME::T2Interior["pbase_nef_giant"] = 1;
$ME::T2Interior["pbase_nef_vbase1"] = 1;
$ME::T2Interior["pbrdg0"] = 1;
$ME::T2Interior["pbrdg1"] = 1;
$ME::T2Interior["pbrdg2"] = 1;
$ME::T2Interior["pbrdg3"] = 1;
$ME::T2Interior["pbrdg4"] = 1;
$ME::T2Interior["pbrdgn"] = 1;
$ME::T2Interior["pbrdgo"] = 1;
$ME::T2Interior["pbrdgp"] = 1;
$ME::T2Interior["pbunk1"] = 1;
$ME::T2Interior["pbunk2"] = 1;
$ME::T2Interior["pbunk3"] = 1;
$ME::T2Interior["pbunk4a_cc"] = 1;
$ME::T2Interior["pbunk5"] = 1;
$ME::T2Interior["pbunk6"] = 1;
$ME::T2Interior["pbunk7"] = 1;
$ME::T2Interior["pbunk7a_cc"] = 1;
$ME::T2Interior["pbunk8"] = 1;
$ME::T2Interior["pmisc1"] = 1;
$ME::T2Interior["pmisc2"] = 1;
$ME::T2Interior["pmisc3"] = 1;
$ME::T2Interior["pmisc4"] = 1;
$ME::T2Interior["pmisc5"] = 1;
$ME::T2Interior["pmisca"] = 1;
$ME::T2Interior["pmiscb"] = 1;
$ME::T2Interior["pmiscc"] = 1;
$ME::T2Interior["pplat1"] = 1;
$ME::T2Interior["pplat2"] = 1;
$ME::T2Interior["pplat3"] = 1;
$ME::T2Interior["pplat4"] = 1;
$ME::T2Interior["pplat5"] = 1;
$ME::T2Interior["prock6"] = 1;
$ME::T2Interior["prock7"] = 1;
$ME::T2Interior["prock8"] = 1;
$ME::T2Interior["procka"] = 1;
$ME::T2Interior["prockb"] = 1;
$ME::T2Interior["prockc"] = 1;
$ME::T2Interior["pspir1"] = 1;
$ME::T2Interior["pspir2"] = 1;
$ME::T2Interior["pspir3"] = 1;
$ME::T2Interior["pspir4"] = 1;
$ME::T2Interior["pspir5"] = 1;
$ME::T2Interior["ptowr1"] = 1;
$ME::T2Interior["ptowr2"] = 1;
$ME::T2Interior["ptowr4"] = 1;
$ME::T2Interior["ptowr5"] = 1;
$ME::T2Interior["ptowr7"] = 1;
$ME::T2Interior["pvbay1"] = 1;
$ME::T2Interior["pvpad"] = 1;
$ME::T2Interior["pwall1"] = 1;
$ME::T2Interior["ram_base"] = 1;
$ME::T2Interior["ram_tower"] = 1;
$ME::T2Interior["ram_wall4"] = 1;
$ME::T2Interior["ramp1"] = 1;
$ME::T2Interior["rilke_domain2_boundrymarker"] = 1;
$ME::T2Interior["rilke_domain2_boundrymarker2"] = 1;
$ME::T2Interior["rilke_domain2_bridge1"] = 1;
$ME::T2Interior["rilke_domain2_mainbase"] = 1;
$ME::T2Interior["rilke_domain_turretbase1"] = 1;
$ME::T2Interior["rilke_whitedwarf_bridge"] = 1;
$ME::T2Interior["rilke_whitedwarf_mainbase"] = 1;
$ME::T2Interior["rilke_whitedwarf_platform1"] = 1;
$ME::T2Interior["rilke_whitedwarf_towerbunker"] = 1;
$ME::T2Interior["ruin1"] = 1;
$ME::T2Interior["ruin2"] = 1;
$ME::T2Interior["ruin3"] = 1;
$ME::T2Interior["ruin4"] = 1;
$ME::T2Interior["ruinarch"] = 1;
$ME::T2Interior["sbase1"] = 1;
$ME::T2Interior["sbase3"] = 1;
$ME::T2Interior["sbase5"] = 1;
$ME::T2Interior["sbrdg1"] = 1;
$ME::T2Interior["sbrdg2"] = 1;
$ME::T2Interior["sbrdg3"] = 1;
$ME::T2Interior["sbrdg4"] = 1;
$ME::T2Interior["sbrdg5"] = 1;
$ME::T2Interior["sbrdg6"] = 1;
$ME::T2Interior["sbrdg7"] = 1;
$ME::T2Interior["sbrdgn"] = 1;
$ME::T2Interior["sbrdgo"] = 1;
$ME::T2Interior["sbunk2"] = 1;
$ME::T2Interior["sbunk9"] = 1;
$ME::T2Interior["sbunk_nef1"] = 1;
$ME::T2Interior["siege"] = 1;
$ME::T2Interior["singleramp"] = 1;
$ME::T2Interior["smisc1"] = 1;
$ME::T2Interior["smisc3"] = 1;
$ME::T2Interior["smisc4"] = 1;
$ME::T2Interior["smisc5"] = 1;
$ME::T2Interior["smisc_nef1"] = 1;
$ME::T2Interior["smisca"] = 1;
$ME::T2Interior["smiscb"] = 1;
$ME::T2Interior["smiscc"] = 1;
$ME::T2Interior["spawnbase"] = 1;
$ME::T2Interior["spawnbase2"] = 1;
$ME::T2Interior["splat1"] = 1;
$ME::T2Interior["splat3"] = 1;
$ME::T2Interior["splat7"] = 1;
$ME::T2Interior["srock6"] = 1;
$ME::T2Interior["srock7"] = 1;
$ME::T2Interior["srock8"] = 1;
$ME::T2Interior["srocka"] = 1;
$ME::T2Interior["srockb"] = 1;
$ME::T2Interior["srockc"] = 1;
$ME::T2Interior["sspir1"] = 1;
$ME::T2Interior["sspir2"] = 1;
$ME::T2Interior["sspir3"] = 1;
$ME::T2Interior["sspir4"] = 1;
$ME::T2Interior["starfallen"] = 1;
$ME::T2Interior["stowr1"] = 1;
$ME::T2Interior["stowr3"] = 1;
$ME::T2Interior["stowr4"] = 1;
$ME::T2Interior["stowr6"] = 1;
$ME::T2Interior["svpad"] = 1;
$ME::T2Interior["swall1"] = 1;
$ME::T2Interior["t2_rail1"] = 1;
$ME::T2Interior["t2_sphere"] = 1;
$ME::T2Interior["t_bbase_ccb2a"] = 1;
$ME::T2Interior["t_bmisc_tunl_ccb1"] = 1;
$ME::T2Interior["t_bwall2a_cnr_cc"] = 1;
$ME::T2Interior["t_bwall2a_lrg_cc"] = 1;
$ME::T2Interior["t_bwall2a_sm_cc"] = 1;
$ME::T2Interior["xbase1"] = 1;
$ME::T2Interior["xbase2"] = 1;
$ME::T2Interior["xbrdg0"] = 1;
$ME::T2Interior["xbrdg1"] = 1;
$ME::T2Interior["xbrdg10"] = 1;
$ME::T2Interior["xbrdg2"] = 1;
$ME::T2Interior["xbrdg3"] = 1;
$ME::T2Interior["xbrdg4"] = 1;
$ME::T2Interior["xbrdg5"] = 1;
$ME::T2Interior["xbrdg6"] = 1;
$ME::T2Interior["xbrdg7"] = 1;
$ME::T2Interior["xbrdg8"] = 1;
$ME::T2Interior["xbrdg9"] = 1;
$ME::T2Interior["xbrdga"] = 1;
$ME::T2Interior["xbrdgb"] = 1;
$ME::T2Interior["xbrdgn"] = 1;
$ME::T2Interior["xbrdgo"] = 1;
$ME::T2Interior["xbunk1"] = 1;
$ME::T2Interior["xbunk2"] = 1;
$ME::T2Interior["xbunk5"] = 1;
$ME::T2Interior["xbunk6"] = 1;
$ME::T2Interior["xbunk9"] = 1;
$ME::T2Interior["xbunkb"] = 1;
$ME::T2Interior["xmisc1"] = 1;
$ME::T2Interior["xmisc2"] = 1;
$ME::T2Interior["xmisc3"] = 1;
$ME::T2Interior["xmisc4"] = 1;
$ME::T2Interior["xmisc5"] = 1;
$ME::T2Interior["xmisca"] = 1;
$ME::T2Interior["xmiscb"] = 1;
$ME::T2Interior["xmiscc"] = 1;
$ME::T2Interior["xplat1"] = 1;
$ME::T2Interior["xplat2"] = 1;
$ME::T2Interior["xplat3"] = 1;
$ME::T2Interior["xrock6"] = 1;
$ME::T2Interior["xrock7"] = 1;
$ME::T2Interior["xrock8"] = 1;
$ME::T2Interior["xrocka"] = 1;
$ME::T2Interior["xrockb"] = 1;
$ME::T2Interior["xrockc"] = 1;
$ME::T2Interior["xspir1"] = 1;
$ME::T2Interior["xspir2"] = 1;
$ME::T2Interior["xspir3"] = 1;
$ME::T2Interior["xspir5"] = 1;
$ME::T2Interior["xtowr1"] = 1;
$ME::T2Interior["xtowr3"] = 1;
$ME::T2Interior["xtowr4"] = 1;
$ME::T2Interior["xtowr7"] = 1;
$ME::T2Interior["xvpad"] = 1;
$ME::T2Interior["xwall1"] = 1;

//----------------------------------------------------------------------------
// Interior auto-scan: register EVERY mounted .dis in the palette, grouped by
// first letter. File::findFirst/findNext walk the whole mounted resource list
// (all zips), so this sees every interior pack that is installed. Mission-lit
// per-instance variants (<base>.<n>.dis) are skipped. A name in $ME::T2Interior
// goes to a "Tribes 2 <letter>" group instead; those register after the scan,
// so the Tribes 2 groups follow the Interiors groups in the list.

function ME::RegisterAllInteriors()
{
   %count = 0;
   %t2 = 0;
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
         %key = String::toLower(%base);
         if($ME::T2Interior[%key])
         {
            $ME::T2Found[%t2] = %base;
            %t2++;
         }
         else
            MissionRegDis("Interiors " @ String::toUpper(String::char(%base, 0)), %base);
         %count++;
      }
      %file = File::findNext("*.dis");
   }
   for(%i = 0; %i < %t2; %i++)
      MissionRegDis("Tribes 2 " @ String::toUpper(String::char($ME::T2Found[%i], 0)), $ME::T2Found[%i]);
   echo("ME: palette scan registered " @ %count @ " interiors, " @ %t2 @ " of them Tribes 2");
}

ME::RegisterAllInteriors();
