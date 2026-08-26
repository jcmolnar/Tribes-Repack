// by Edgecrusher

$InvList[FeedPack] = 1;
$RemoteInvList[FeedPack] = 1;

$NumDepend[FeedPack] = 1;
$Depends[FeedPack, 0] = HvyBolter;

ItemImageData FeedPackImage 
{
  shapeFile = "AmmoPack";
  weaponType = 2;
  mountPoint = 2;
  mountOffset = 
  {
    0, -0.1, 0 }
  ;
  minEnergy = 0;
  maxEnergy = 0;
  firstPerson = false;

};

ItemData FeedPack 
{
  description = "Belt Feeder";
  shapeFile = "AmmoPack";
  className = "Backpack";
  heading = $InvHead[ihBac];
  shadowDetailMask = 4;
  imageType = FeedPackImage;
  price = 10;
  hudIcon = "energypack";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function FeedPack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
  {
    Player::mountItem(%player,%item,$BackpackSlot);
  }
}

function FeedPack::onMount(%player,%item) 
{
  Player::trigger(%player,$BackpackSlot,true);
}

function FeedPack::onUnmount(%player,%item) 
{
  if (Player::getMountedItem(%player,$WeaponSlot) == HvyBolter) Player::unmountItem(%player,$WeaponSlot);
}
