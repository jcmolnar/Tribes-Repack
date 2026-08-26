$InvList[EnergyPack] = 1;
$RemoteInvList[EnergyPack] = 1;

$NumDepend[EnergyPack] = 2;
$Depends[EnergyPack, 0] = Brightlance;
$Depends[EnergyPack, 1] = PlasCan;

ItemImageData EnergyPackImage 
{
  shapeFile = "jetPack";
  weaponType = 2;
  mountPoint = 2;
  mountOffset = 
  {
    0, -0.1, 0 }
  ;
  minEnergy = -3;
  maxEnergy = -5;
  firstPerson = false;

lightType = 2;   // Pulsing
	lightRadius = 4;
	lightTime = 0.5;
	lightColor = { 0.13, 0.25, 1 };
};

ItemData EnergyPack 
{
  description = "Energy Pack";
  shapeFile = "jetPack";
  className = "Backpack";
  heading = $InvHead[ihBac];
  shadowDetailMask = 4;
  imageType = EnergyPackImage;
  price = 4;
  hudIcon = "energypack";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function EnergyPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
  {
    Player::mountItem(%player,%item,$BackpackSlot);
  }
}

function EnergyPack::onMount(%player,%item) 
{
  Player::trigger(%player,$BackpackSlot,true);
}

function EnergyPack::onUnmount(%player,%item) 
{
  if (Player::getMountedItem(%player,$WeaponSlot) == Brightlance) Player::unmountItem(%player,$WeaponSlot);
 if (Player::getMountedItem(%player,$WeaponSlot) == Demogun) Player::unmountItem(%player,$WeaponSlot);
}
