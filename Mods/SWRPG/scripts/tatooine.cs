if($FlyByShip == "" && ($FlyByDropships == true || $FlyByDropships == 1))
{
	schedule("InitFlyByShip();", 30);



	%vehtype = "TieFighter";
	$flybytie = newObject("",flier,%vehtype,true);
	Gamebase::setMapName(%vehicle,%vehtype.description);
	addToSet("MissionCleanup", %vehicle);
	GameBase::setTeam(%vehicle,0);
	GameBase::startFadeIn(%vehicle);
	GameBase::setPosition(%vehicle, "0 0 200");
	GameBase::setRotation(%vehicle, "0 0 0");
}