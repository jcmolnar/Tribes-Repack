//==============================================================================
// OPS FLAG SOUNDS -- Opsaya's config\Modules\Flagsounds.acs.cs.
// Options > 07 SCRIPTS "Flag return sounds" ($pref::scriptFlagSounds).
//
// A distinct cue when a flag goes home: one for YOUR team's flag, one for theirs.
// His module also defined drop / grab / cap cues but had them switched off, so only
// the return pair ships (config\ModernHUD\Scripts\sounds\opsflag_ret{F,E}.ogg).
// Fed by Presto's eventFlagReturned(teamFlag, client), which covers a player
// return, the server's auto-return and an out-of-bounds return.
//==============================================================================

function OpsFlagSounds::onReturned(%team, %cl)
{
   if($pref::scriptFlagSounds != 1)
      return;
   if(%team == Client::getTeam(getManagerId()))
      localSound("opsflag_retF");
   else
      localSound("opsflag_retE");
}

function OpsFlagSounds::attach()
{
   Event::Attach(eventFlagReturned, OpsFlagSounds::onReturned);
}
