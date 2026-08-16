function InitFlyByShip()
{
	if($FlyByDropships != true && $FlyByDropships != 1)
	//if(!$FlyByDropships)
		return;

	$FlyByShip1 = newObject("","InteriorShape","shuttlegb.dis");	//Imperial shuttle
	$FlyByShip2 = newObject("","InteriorShape","skiffd.dis");	//Desert skiff
	$FlyByShip3 = newObject("","InteriorShape","sbarge2b.dis");	//Jabba's barge
	$FlyByShip5 = newObject("","InteriorShape","mf3d.dis");		//Mellenium Falcon
	$FlyByShip4 = newObject("","InteriorShape","gesd6d.dis");	//Big Imperial ship
	$FlyByShip5 = newObject("","InteriorShape","gcotpdrop.dis");	//COTP dropship

	addToSet("MissionCleanup", $FlyByShip1);
	addToSet("MissionCleanup", $FlyByShip2);
	addToSet("MissionCleanup", $FlyByShip3);
	addToSet("MissionCleanup", $FlyByShip4);
	addToSet("MissionCleanup", $FlyByShip5);
	//addToSet("MissionCleanup", $FlyByShip6);

	//schedule("MoveShip(0);", 60);

	schedule("MoveShip();", 60);
}

function MoveShip()
{
	if($FlyByDropships != true && $FlyByDropships != 1)
	//if(!$FlyByDropships)
		return;

	$FlyByShip = randomItems(5, $FlyByShip1, $FlyByShip2, $FlyByShip3, $FlyByShip4, $FlyByShip5, $FlyByShip6);

	%dir = randomItems(5, NY, PY, NX, PX, NW);
	if($FlyByShip == $FlyByShip4 || $FlyByShip == $FlyByShip5)
	{
		if(%dir == PX)
			%rot = "1.57";
		else if(%dir == PY)
			%rot = "3.14";
		else if(%dir == NX)
			%rot = "-1.57";
		else if(%dir == NY)
			%rot = "3.14";
		else if(%dir == NW)
			%rot = "-2.4";//3.8
	}
	else
	{
		if(%dir == PX)
			%rot = "3.14";
		else if(%dir == PY)
			%rot = "-1.57";
		else if(%dir == NX)
			%rot = "0";
		else if(%dir == NY)
			%rot = "-1.57";
		else if(%dir == NW)
			%rot = "2.3";//3.8
	}

	GameBase::setRotation($FlyByShip, %rot);

	//if($FlyByShip == $DSFlyByShip)
	//	GameBase::setRotation($FlyByShip, vector::add(GameBase::GetRotation($FlyByShip), "0 0 3.14"));

	$FlyByHeight = randomItems(5, 300, 400, 250, 350, 450);
	schedule("MoveShip" @ %dir @ "(0);", 9);
	//schedule("ShipAttack(0);", 5);
	gameBase::startFadeIn($FlyByShip);
	echo("**** DROPSHIP heading: " @ %dir);
}

function ShipAttack(%i)
{
	%trans = "0 0 1 0 0 1 0 0 -9 " @ vector::sub(gamebase::getposition($FlyByShip), "0 0 5");
	Projectile::spawnProjectile(FlyByBomb, %trans, %player, "0 0 1");
	if(%i < 60)
		schedule("ShipAttack(" @ %i++ @ ");", 2);
}

function MoveShipUp(%pos, %i)
{
	GameBase::setPosition($FlyByShip, vector::add(%pos, "0 0 " @ %i));
	if(%i < 600)
		schedule("MoveShipUp(\"" @ %pos @ "\", \"" @ %i + 2 @ "\");", 0.1);
	else {
		//if($FlyByTime < 60) $FlyByTime = 60;
		schedule("MoveShip();", $FlyByTime);
		GameBase::setPosition($FlyByShip, "0 0 -999"); }
}

function MoveShipNW(%i)
{
	GameBase::setPosition($FlyByShip, 999 - %i @ " " @ -999 + %i @ " " @ $FlyByHeight);
	if(%i < 1999)
		schedule("MoveShipNW(" @ %i + 2 @ ");", 0.05);
	else
		schedule("MoveShipUp(GameBase::getPosition(" @ $FlyByShip @ "), 0);", 9);
}

function MoveShipPX(%i)
{
	GameBase::setPosition($FlyByShip, -999 + %i @ " 0 " @ $FlyByHeight);
	if(%i < 1999)
		schedule("MoveShipPX(" @ %i + 2 @ ");", 0.05);
	else
		schedule("MoveShipUp(GameBase::getPosition(" @ $FlyByShip @ "), 0);", 9);
}

function MoveShipNX(%i)
{
	GameBase::setPosition($FlyByShip, 999 - %i @ " 00 " @ $FlyByHeight);
	if(%i < 1999)
		schedule("MoveShipNX(" @ %i + 2 @ ");", 0.05);
	else
		schedule("MoveShipUp(GameBase::getPosition(" @ $FlyByShip @ "), 0);", 9);
}

function MoveShipPY(%i)
{
	GameBase::setPosition($FlyByShip, "0 " @ -999 + %i @ " " @ $FlyByHeight);
	if(%i < 1999)
		schedule("MoveShipPY(" @ %i + 2 @ ");", 0.05);
	else
		schedule("MoveShipUp(GameBase::getPosition(" @ $FlyByShip @ "), 0);", 9);
}

function MoveShipNY(%i)
{
	GameBase::setPosition($FlyByShip, "0 " @ 999 - %i @ " " @ $FlyByHeight);
	if(%i < 1999)
		schedule("MoveShipNY(" @ %i + 2 @ ");", 0.05);
	else
		schedule("MoveShipUp(GameBase::getPosition(" @ $FlyByShip @ "), 0);", 9);
}