Autoload( "Core/*.cs" ); 
Autoload( "Modules/*.acs.cs" ); 

exec("gamebinds.cs");
exec("macchat.cs");
exec("trainer.cs");

$Server::warmupTime = 3;
$Server::respawnTime = 0;

