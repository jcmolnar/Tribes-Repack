// Bolter & Shurcata
$SellAmmo[BolterAmmo] = 25;
$InvList[BolterAmmo] = 1;
$RemoteInvList[BolterAmmo] = 1;

addAmmo(Bolter, BolterAmmo, 25);
addAmmo(ShurCata, BolterAmmo, 20);

ItemData BolterAmmo 
{
	description = "Pistol Rounds";
	className = "Ammo";
	heading = $InvHead[ihAmm];
	shapeFile = "ammo1";
	shadowDetailMask = 4;
	price = 0;
};

// HvyBolter & ShurCannon
$SellAmmo[HvyBolterAmmo] = 30;
$InvList[HvyBolterAmmo] = 1;
$RemoteInvList[HvyBolterAmmo] = 1;

addAmmo(HvyBolter, HvyBolterAmmo, 30);
addAmmo(ShurCannon, HvyBolterAmmo, 20);

ItemData HvyBolterAmmo 
{
	description = "Heavy Weapon Ammo";
	className = "Ammo";
	heading = $InvHead[ihAmm];
	shapeFile = "ammo1";
	shadowDetailMask = 4;
	price = 0;
};

// SniperRifle & Long Rifle
$SellAmmo[SniperAmmo] = 15;
$InvList[SniperAmmo] = 1;
$RemoteInvList[SniperAmmo] = 1;

addAmmo(SniperRifle, SniperAmmo, 15);
addAmmo(LongRifle, SniperAmmo, 15);

ItemData SniperAmmo 
{
	description = "Sniper Ammo";
	className = "Ammo";
	heading = $InvHead[ihAmm];
	shapeFile = "ammo1";
	shadowDetailMask = 4;
	price = 0;
};

// RocketLauncher & ERocketLauncher
$SellAmmo[RocketAmmo] = 8;
$InvList[RocketAmmo] = 1;
$RemoteInvList[RocketAmmo] = 1;

addAmmo(RocketLauncher, RocketAmmo, 10);
addAmmo(ERocketLauncher, RocketAmmo, 10);

ItemData RocketAmmo
{
	description = "Rocket Ammo";
	className = "Ammo";
	shapeFile = "rocket";
	heading = $InvHead[ihAmm];
	shadowDetailMask = 4;
	price = 0;
};

// Boltpist & ShurPist
$SellAmmo[BoltPistAmmo] = 25;
$InvList[BoltPistAmmo] = 1;
$RemoteInvList[BoltPistAmmo] = 1;

addAmmo(BoltPist, BoltPistAmmo, 25);
addAmmo(ShurPist, BoltPistAmmo, 20);

ItemData BoltPistAmmo 
{
	description = "Sidearm Rounds";
	className = "Ammo";
	heading = $InvHead[ihAmm];
	shapeFile = "ammo1";
	shadowDetailMask = 4;
	price = 0;
};

// Cyclone & ReaperLauncher
$SellAmmo[CycloneAmmo] = 25;
$InvList[CycloneAmmo] = 1;
$RemoteInvList[CycloneAmmo] = 1;

addAmmo(Cyclone, CycloneAmmo, 25);
addAmmo(Reaper, CycloneAmmo, 20);

ItemData CycloneAmmo
{
	description = "Heavy Missile";
	className = "Ammo";
	shapeFile = "rocket";
	heading = $InvHead[ihAmm];
	shadowDetailMask = 4;
	price = 0;
};

// TrackerMissile & BioMissile
$SellAmmo[TrackerMissileAmmo] = 5;
$InvList[TrackerMissileAmmo] = 1;
$RemoteInvList[TrackerMissileAmmo] = 1;

addAmmo(TrackerMissilePack, TrackerMissileAmmo, 10);
addAmmo(BioMissilePack, TrackerMissileAmmo, 10);


ItemData TrackerMissileAmmo 
{
	description = "Pack Missile";
	className = "Ammo";
	heading = $InvHead[ihAmm];
	shapeFile = "rocket";
	shadowDetailMask = 4;
	price = 0;
};


// Autogun
$SellAmmo[AutoAmmo] = 100;
$InvList[AutoAmmo] = 1;
$RemoteInvList[AutoAmmo] = 1;

addAmmo(Autogun, AutoAmmo, 50);

ItemData AutoAmmo
{
	description = "Bullet";
	className = "Ammo";
	shapeFile = "ammo1";
	heading = $InvHead[ihAmm];
	shadowDetailMask = 4;
	price = 0;
};

// AutoCannon
$SellAmmo[AutoCannonAmmo] = 25;
$InvList[AutoCannonAmmo] = 1;
$RemoteInvList[AutoCannonAmmo] = 1;

addAmmo(AutoCannon,AutoCannonAmmo,60);

ItemData AutoCannonAmmo 
{
	description = "Heavy Bullet";
	className = "Ammo";
	shapeFile = "ammo1";
	heading = $InvHead[ihAmm];
	shadowDetailMask = 4;
	price = 0;
};

// Virus
$SellAmmo[VirusAmmo] = 25;
$InvList[VirusAmmo] = 1;
$RemoteInvList[VirusAmmo] = 1;

addAmmo(Virus, VirusAmmo, 25);

ItemData VirusAmmo
{
	description = "Neurotoxin";
	className = "Ammo";
	heading = $InvHead[ihAmm];
	shapeFile = "mortarammo";
	shadowDetailMask = 4;
	price = 0;
};

// TranqGun
$SellAmmo[TranqAmmo] = 25;
$InvList[TranqAmmo] = 1;
$RemoteInvList[TranqAmmo] = 1;

addAmmo(TranqGun, TranqAmmo, 25);

ItemData TranqAmmo 
{
	description = "Poison Dart";
	className = "Ammo";
	heading = $InvHead[ihAmm];
	shapeFile = "ammo1";
	shadowDetailMask = 4;
	price = 0;
};

// DemoGun
$SellAmmo[DemoGunAmmo] = 50;
$InvList[DemogunAmmo] = 1;
$RemoteInvList[DemogunAmmo] = 1;

addAmmo(DemoGun, DemoGunAmmo,25);

ItemData DemoGunAmmo 
{
	description = "Isolanth Charges";
	className = "Ammo";
	heading = $InvHead[ihAmm];
	shapeFile = "ammo1";
	shadowDetailMask = 4;
	price = 0;
}; 

// EMP
$SellAmmo[EMPAmmo] = 10;
$InvList[EMPAmmo] = 1;
$RemoteInvList[EMPAmmo] = 1;

addAmmo(EMP, EMPAmmo, 10);

ItemData EMPAmmo
{
	description = "Haywire Grenade";
	className = "Ammo";
	shapeFile = "Ammo2";
	heading = $InvHead[ihAmm];
	shadowDetailMask = 4;
	price = 0;
};

// EverBolter
$SellAmmo[EvBolterAmmo] = 25;
$InvList[EvBolterAmmo] = 1;
$RemoteInvList[EvBolterAmmo] = 1;

addAmmo(EvBolter, EvBolterAmmo, 25);

ItemData EvBolterAmmo 
{
	description = "Eversor Pistol Rounds";
	className = "Ammo";
	heading = $InvHead[ihAmm];
	shapeFile = "ammo1";
	shadowDetailMask = 4;
	price = 0;
};

// Poison
$SellAmmo[PoisonAmmo] = 25;
$InvList[PoisonAmmo] = 1;
$RemoteInvList[PoisonAmmo] = 1;

addAmmo(Poison, PoisonAmmo, 25);

ItemData PoisonAmmo 
{
	description = "Poison Rounds";
	className = "Ammo";
	heading = $InvHead[ihAmm];
	shapeFile = "ammo1";
	shadowDetailMask = 4;
	price = 0;
};

// Grenade Launcher
$SellAmmo[GrenadeAmmo] = 10;
$InvList[GrenadeAmmo] = 1;
$RemoteInvList[GrenadeAmmo] = 1;

addAmmo(GrenadeLauncher, GrenadeAmmo, 10);

ItemData GrenadeAmmo 
{ 
	description = "Grenade Ammo"; 
	className = "Ammo"; 
	shapeFile = "grenammo"; 
	heading = $InvHead[ihAmm]; 
	shadowDetailMask = 4; 
	price = 0;
}; 

// Melta
$SellAmmo[MeltaAmmo] = 5;
$InvList[MeltaAmmo] = 1;
$RemoteInvList[MeltaAmmo] = 1;

addAmmo(Melta, MeltaAmmo, 8);

ItemData MeltaAmmo
{
	description = "Melta Charge";
	className = "Ammo";
	shapeFile = "mortarammo";
	heading = $InvHead[ihAmm];
	shadowDetailMask = 4;
	price = 0;
};

// PlasAuto
$SellAmmo[PlasAutoAmmo] = 25;
$InvList[PlasAutoAmmo] = 1;
$RemoteInvList[PlasAutoAmmo] = 1;

addAmmo(PlasAuto,PlasAutoAmmo,60);

ItemData PlasAutoAmmo 
{
	description = "Plasma Bolts";
	className = "Ammo";
	shapeFile = "ammo1";
	heading = $InvHead[ihAmm];
	shadowDetailMask = 4;
	price = 0;
};

// Plascan
$SellAmmo[PlasCanAmmo] = 25;
$InvList[PlasCanAmmo] = 1;
$RemoteInvList[PlasCanAmmo] = 1;

addAmmo(PlasCan, PlasCanAmmo, 25);

ItemData PlasCanAmmo 
{
	description = "Heavy Plasma Charges";
	className = "Ammo";
	heading = $InvHead[ihAmm];
	shapeFile = "plasammo";
	shadowDetailMask = 4;
	price = 0;
};

// PlasmaGun
$SellAmmo[PlasmaAmmo] = 15;
$InvList[PlasmaAmmo] = 1;
$RemoteInvList[PlasmaAmmo] = 1;

addAmmo(PlasmaGun, PlasmaAmmo, 15);

ItemData PlasmaAmmo
{
	description = "Plasma Bolt";
	heading = $InvHead[ihAmm];
	className = "Ammo";
	shapeFile = "plasammo";
	shadowDetailMask = 4;
	price = 0;
};

// Shotgun
$SellAmmo[ShotgunAmmo] = 10;
$InvList[ShotgunAmmo] = 1;
$RemoteInvList[ShotgunAmmo] = 1;

addAmmo(Shotgun, ShotgunAmmo, 10);

ItemData ShotgunAmmo
{
	description = "Shotgun Shells";
	className = "Ammo";
	heading = $InvHead[ihAmm];
	shapeFile = "ammo1";
	shadowDetailMask = 4;
	price = 0;
};

// STBolter
$SellAmmo[StBolterAmmo] = 50;
$InvList[StBolterAmmo] = 1;
$RemoteInvList[StBolterAmmo] = 1;

addAmmo(StBolter, StBolterAmmo, 50);

ItemData StBolterAmmo 
{
	description = "Bolter Shells";
	className = "Ammo";
	shapeFile = "ammo1";
	heading = $InvHead[ihAmm];
	shadowDetailMask = 4;
	price = 0;
};

// VibroCannon
$SellAmmo[VibroCannonAmmo] = 15;
$InvList[VibroCannonAmmo] = 1;
$RemoteInvList[VibroCannonAmmo] = 1;

addAmmo(VibroCannon, VibroCannonAmmo, 10);

ItemData VibroCannonAmmo 
{
	description = "Vibro Shell";
	className = "Ammo";
	heading = $InvHead[ihAmm];
	shapeFile = "mortarammo";
	shadowDetailMask = 4;
	price = 0;
};

// MineLauncher
$SellAmmo[MinelAmmo] = 10;
$InvList[MinelAmmo] = 1;
$RemoteInvList[MinelAmmo] = 1;

addAmmo(MineLauncher, MinelAmmo, 10);

ItemData MinelAmmo
{
	description = "Mine Launcher Ammo";
	className = "Ammo";
	shapeFile = "grenammo";
	heading = $InvHead[ihAmm];
	shadowDetailMask = 4;
	price = 0;
};
