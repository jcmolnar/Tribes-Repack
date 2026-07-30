// PluginLoader Info:
// -----------------------------------------------------------------------------
// CommandLine Forced Load or No-Load < Over-rides Script-Based Load  >
// -----------------------------------------------------------------------------
// Forced Load Example: Tribes.exe +p DoSFix
// Forced No-Load Example: Tribes.exe -p DoSFix
// Multiple Plugin Example: Tribes.exe +p DoSFix,crcBypass,etc.
// -----------------------------------------------------------------------------
// Script-Based Load
// Example: $PluginLoader::DoSFix = true;
// -----------------------------------------------------------------------------
if($dedicated)
	$PluginLoader::DoSFix = true;
else
	$PluginLoader::DoSFix = false; //Because dosfix doesn't play nice with special chats

$PluginLoader::MathPlugin = true;
$PluginLoader::StringPlugin = true;
$PluginLoader::GraphicPlugin = true;
$PluginLoader::PatchesPlugin = true;
$PluginLoader::CommLinkPlugin = true;
$PluginLoader::ExperimentPlugin = true;
$PluginLoader::ClientSideAddonPlugin = true;
$PluginLoader::kronosfix = true;
$PluginLoader::kronos_fullbodyanim = true;
// ScriptGL keyboard text input (chat composer): glTextInput + keydispatch detour.
// CLIENT ONLY - it patches the keyboard-dispatch code, which must NOT run on the
// dedicated server (same install loads this Plugins\ folder; descriptor flags=3
// would otherwise load it server-side and hang the server on connect).
if(!$dedicated)
	$PluginLoader::kronos_textinput = true;
else
	$PluginLoader::kronos_textinput = false;
// Animated textures (water test on the WOOD4 cube). CLIENT ONLY - it detours the
// render/texture-cache code which must not run on the dedicated server.
if(!$dedicated)
	$PluginLoader::kronos_animtex = true;
else
	$PluginLoader::kronos_animtex = false;
//$PluginLoader::Attachment = false;
//$PluginLoader::TerrainInfo = true;
//$PluginLoader::Extras = true;