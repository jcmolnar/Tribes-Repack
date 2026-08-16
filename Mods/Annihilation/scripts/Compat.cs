//-----------------------------------------------------------------------------
// Compat.cs -- console commands this config expects from the old 1.40 plugin
// DLLs (netset.dll / LoDFix.dll in plugins\), reimplemented in script.
//
// Those plugins are 1.40-era binaries and are not loaded by this client, so
// every call to them logged "Unknown command" and returned nothing -- which
// silently blanked every server log line, every admin list column and every
// stats identifier. Scripted equivalents keep the config working with no
// plugin dependency at all.
//
// Loaded from common.cs, before anything that uses them.
//-----------------------------------------------------------------------------

// Pad %str on the RIGHT with spaces until it is %width long. Never crops --
// matches the documented plugin behaviour (see plugins\StringPlugin.txt).
function String::rpad( %str, %width )
{
   for ( %len = String::len( %str ); %len < %width; %len++ )
      %str = %str @ " ";

   return %str;
}

// Per-player identity, used only to tag stats rows. The plugin returned a
// WON/IA GUID; nothing here persists stats off-box, so the client id is a
// stable enough identifier for a single session.
function Client::getGuid( %cl )
{
   return %cl;
}

// Stats event sink. The plugin streamed these to an external collector that
// this build has no counterpart for, so swallow them -- the callers only ever
// push, they never read anything back.
function StatLog::Push( %type, %ident, %a1, %a2, %a3 )
{
}
