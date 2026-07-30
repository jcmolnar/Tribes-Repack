Autoload( "Core/*.cs" ); 
Autoload( "Modules/*.acs.cs" );

exec("gamebinds.cs");

$Server::warmupTime = 3;
$Server::respawnTime = 0;

//////////////////////Save WeapRet (reticle) position////////////////////
$pref::hudPositionsWepRet::Container = "1025 465||0.500488 0.501078 0";
$pref::hudPositionsHUDOverlay::Container = "40 41||0.018349 0.038679 0";
/////////////////////////////////////////////////////////////////////////

function Binds::GameBinds::Init()
  after GameBinds::Init
{
	$GameBinds::CurrentMapHandle = GameBinds::GetActionMap2( "actionMap.sae");
	$GameBinds::CurrentMap = "actionMap.sae";
	GameBinds::addBindCommand( "GET FLAG", 	"say(1, \"** \x03\x03\x03\x03\x03\x03  GET THE FLAG   \x03\x03\x03\x03\x03\x03 **~wgeteflg\");");
	GameBinds::addBindCommand( "RET FLAG", 	"say(1, \"** \x03\x03\x03\x03\x03\x03 RETURN THE FLAG \x03\x03\x03\x03\x03\x03 **~wretflag\");");
    GameBinds::addBindCommand( "PASS FLAG", "say(1, \"** \x03\x03\x03\x03\x03\x03 PASS THE FLAG   \x03\x03\x03\x03\x03\x03 **~whaveflg\");");
    GameBinds::addBindCommand( "NORTH", 	"say(1, \"** \x03\x03\x03\x03\x03\x03      NORTH      \x03\x03\x03\x03\x03\x03 **~wincom2\");"); 
    GameBinds::addBindCommand( "SOUTH", 	"say(1, \"** \x03\x03\x03\x03\x03\x03      SOUTH      \x03\x03\x03\x03\x03\x03 **~wincom2\");");
    GameBinds::addBindCommand( "WEST", 	"say(1, \"** \x03\x03\x03\x03\x03\x03      WEST       \x03\x03\x03\x03\x03\x03 **~wincom2\");");
    GameBinds::addBindCommand( "EAST", 	"say(1, \"** \x03\x03\x03\x03\x03\x03      EAST       \x03\x03\x03\x03\x03\x03 **~wincom2\");");

}