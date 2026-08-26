//by Edgecrusher
$InvList[LaserPack] = 1;
$RemoteInvList[LaserPack] = 1;

$NumDepend[LaserPack] = 2;
$Depends[LaserPack, 0] = LasCannon;
$Depends[LaserPack, 1] = ScatterLas;


ItemImageData LaserPackImage 
{
	shapeFile = "advener";
	weaponType = 2;  // Sustained
	mountPoint = 2;
	mass = 3.0;
	minEnergy = -5;
 	maxEnergy = -7;
	firstPerson = false;
	lightType = 2;   // Pulsing
	lightRadius = 2;
	lightTime = 0.2;
	lightColor = { 0.1, 0.2, 0.7 };
};

ItemData LaserPack 
{
  description = "Adv. Energy Pack";
  shapeFile = "advener";
  className = "Backpack";
  heading = $InvHead[ihBac];
  shadowDetailMask = 4;
  imageType = LaserPackImage;
  price = 12;
  hudIcon = "energypack";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function LaserPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
  {
    Player::mountItem(%player,%item,$BackpackSlot);
  }
}

function LaserPack::onMount(%player,%item) 
{
  Player::trigger(%player,$BackpackSlot,true);
}

function LaserPack::onUnmount(%player,%item) 
{

  if (Player::getMountedItem(%player,$WeaponSlot) == LasCannon) Player::unmountItem(%player,$WeaponSlot);

if (Player::getMountedItem(%player,$WeaponSlot) == Demogun) Player::unmountItem(%player,$WeaponSlot);


}

