// Appearance controls shared by authored and converted ScriptGL parts.
// White preserves the pack's original colors. RGB tint multiplies every pixel,
// including bitmap-font markup, cropped bars, animated art and TrueType text.
// Registered as part settings so the HUD designer and K menu use one registry.

function ModernHUD::partTint(%name, %provider)
{
   // A borrowed component owns its colors even while another pack is the base.
   // Existing position keys deliberately retain their historical base ownership.
   // The optional provider is used while preregistering manifest parts, before
   // their first frame (or first transient event), so presets can restore tint.
   if(%provider == "")
      %provider = $ModernHUD::DrawPack;
   if(%provider == "")
      %provider = $ModernHUD::PackId;
   %key = "pref::ModernHUD::Tint::" @ %provider @ "::" @ %name;
   %red = %key @ "::Red";
   %green = %key @ "::Green";
   %blue = %key @ "::Blue";

   // Validate the cached registry row, not a persistent 'registered' flag:
   // clearSettings() discards rows on every pack switch. This also avoids a
   // linear scan through every setting for every part on every render frame.
   %row = $ModernHUD::TintRow[%key];
   // Console variable names are case-insensitive. Some manual manifests use
   // lowercase handle names while their draw functions use TitleCase.
   if(%row == "" || String::ICompare($ModernHUD::Setting[%row, key], %red) != 0)
   {
      %row = $ModernHUD::SettingCount;
      if(%row == "") %row = 0;
      $ModernHUD::TintRow[%key] = %row;
      // Keep player-facing rows readable; persistence keys are implementation
      // details and can be longer than the settings panel itself.
      %label = %name;
      if(String::findSubStr(%label, "ModernHUD::") == 0)
         %label = String::getSubStr(%label, 11, String::len(%label) - 11);
      if(String::findSubStr(%label, "Stock") == 0)
         %label = String::getSubStr(%label, 5, String::len(%label) - 5);
      %suffix = String::findSubStr(%label, "_Container");
      if(%suffix > 0) %label = String::getSubStr(%label, 0, %suffix);
      ModernHUD::setting("int", %red, %label @ " tint red", "255", "0|255|5", "", %name);
      ModernHUD::setting("int", %green, %label @ " tint green", "255", "0|255|5", "", %name);
      ModernHUD::setting("int", %blue, %label @ " tint blue", "255", "0|255|5", "", %name);
   }
   return getVariable(%red) @ " " @ getVariable(%green) @ " " @ getVariable(%blue);
}
