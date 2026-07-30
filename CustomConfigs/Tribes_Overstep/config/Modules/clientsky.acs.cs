//Skyrotation speed, set to 0 to disable this shitty plugin
$mj::skyrotationspeed = 20;


function mj::addSky(%mission, %dml, %skysize)
{
	$mj::ClientSky[%mission, dml] = %dml;
	$mj::ClientSky[%mission, size] = %skysize;
}


//Remove // to active Default sky for every map not defined by mj::addsky
mj::addSky("Default","litesky.dml",450);

//to add more skies just add more calls to mj::addSky() 
mj::addSky("HillkingLT","litesky.dml",300);
mj::addSky("DangerousCrossingLT","litesky.dml",450);
mj::addSky("SnowBlind","litesky.dml",450);
mj::addSky("IceRidge","litesky.dml",450);
mj::addSky("Stonehenge","lushdayclear.dml",350);
mj::addSky("StonehengeLT","litesky.dml",400);
mj::addSky("Raindance","lushdayclear.dml", 600);
mj::addSky("DangerousCrossing","litesky.dml",450);
mj::addSky("HammerDownLT","lushdayclear.dml",350);
mj::addSky("BulwarkLT","litesky.dml",350);
mj::addSky("HildebrandLT","litesky.dml",200);

//Tribes adds the current world (lush/mars/alien etc) as ghosting object so if you try to set skies of different worlds it will fail, we need to load our worlds dynamically
//don't delete
$mj::skyToWorlds["aliensky_cloudyday.dml"] = "AlienWorld.zip";
$mj::skyToWorlds["alienight.dml"] = "AlienWorld.zip";
$mj::skyToWorlds["aliengreysky.dml"] = "AlienWorld.zip";

$mj::skyToWorlds["greysky.dml"] = "IceWorld.zip";
$mj::skyToWorlds["icenitesky.dml"] = "IceWorld.zip";

$mj::skyToWorlds["lushdayclear.dml"] = "LushWorld.zip";
$mj::skyToWorlds["litesky.dml"] = "LushWorld.zip";
$mj::skyToWorlds["lushsky_night.dml"] = "LushWorld.zip";

$mj::skyToWorlds["marsday.dml"] = "MarsWorld.zip";
$mj::skyToWorlds["marsdaycloud.dml"] = "MarsWorld.zip";
$mj::skyToWorlds["marsdusk.dml"] = "MarsWorld.zip";

$mj::skyToWorlds["mudsky_day.dml"] = "MudWorld.zip";
$mj::skyToWorlds["mudusk.dml"] = "MudWorld.zip";
$mj::skyToWorlds["mudcloudnight.dml"] = "MudWorld.zip";

$mj::skyToWorlds["deserttansky.dml"] = "DesertWorld.zip";
$mj::skyToWorlds["nitesky.dml"] = "DesertWorld.zip";


