// by Edgecrusher
$InvList[MindPack] = 1;
$RemoteInvList[MindPack] = 1;

$NumDepend[MindPack] = 7;
$Depends[MindPack, 0] = Rain;
$Depends[MindPack, 1] = Flamewall;
$Depends[MindPack, 2] = Disc;
$Depends[MindPack, 3] = DCannon;
$Depends[MindPack, 4] = RokkitLauncher;
$Depends[MindPack, 5] = Gravi;
$Depends[MindPack, 6] = Zap;

ItemImageData MindPackImage 
{
  shapeFile = "jetPack";
  weaponType = 2;
  mountPoint = 2;
  mountOffset = 
  {
    0, -0.1, 0 }
  ;
  minEnergy = -8;
  maxEnergy = -16;
  firstPerson = false;

lightType = 2;   // Pulsing
	lightRadius = 4;
	lightTime = 0.5;
	lightColor = { 0.13, 0.25, 1 };
};

ItemData MindPack 
{
  description = "Concentrate";
  shapeFile = "jetPack";
  className = "Backpack";
  heading = $InvHead[ihPsi];
  shadowDetailMask = 4;
  imageType = MindPackImage;
  price = 8;
  hudIcon = "energypack";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function MindPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
  {
    Player::mountItem(%player,%item,$BackpackSlot);
  }
}

function MindPack::onMount(%player,%item) 
{
  Player::trigger(%player,$BackpackSlot,true);
}

function MindPack::onUnmount(%player,%item) 
{
  if (Player::getMountedItem(%player,$WeaponSlot) == LaserRifle) Player::unmountItem(%player,$WeaponSlot);
}
