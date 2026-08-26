// by Edgecrusher
$InvList[FlamePack] = 1;
$RemoteInvList[FlamePack] = 1;


$NumDepend[Flamepack] = 2;
$Depends[Flamepack, 0] = Firepike;
$Depends[Flamepack, 1] = HFlamer;

ItemImageData FlamePackImage 
{
  shapeFile = "nappack";
  weaponType = 2;
  mountPoint = 2;
  minEnergy = 0;
  maxEnergy = 0;
  firstPerson = false;

lightType = 2;   // Pulsing
	lightRadius = 4;
	lightTime = 0.5;
	lightColor = { 0.13, 0.25, 1 };
};

ItemData FlamePack 
{
  description = "Napalm Pack";
  shapeFile = "nappack";
  className = "Backpack";
  heading = $InvHead[ihBac];
  shadowDetailMask = 4;
  imageType = FlamePackImage;
  price = 15;
  hudIcon = "energypack";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function FlamePack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
  {
    Player::mountItem(%player,%item,$BackpackSlot);
  }
}

function FlamePack::onMount(%player,%item) 
{
  Player::trigger(%player,$BackpackSlot,true);
}

function FlamePack::onUnmount(%player,%item) 
{
 
  if (Player::getMountedItem(%player,$WeaponSlot) == HFlamer) Player::unmountItem(%player,$WeaponSlot);

if (Player::getMountedItem(%player,$WeaponSlot) == Firepike) Player::unmountItem(%player,$WeaponSlot);
}
