$EyepieceSlot=4;
$EyepieceSlot2=5;

$InvList[HeadSet] = 0;
$RemoteInvList[HeadSet] = 0;

ItemImageData HeadSetImage 
{
  shapeFile = "paintgun";
  mountPoint = 2;
  mountOffset = { 0.02, 0.25, 0.5 };
  mountRotation = { 3.14, 1.5, 0.4 };
  lightType = 2;
  lightRadius = 0.4;
  lightTime = 0.1;
  lightColor = { 0, 1, 0 };
  mass = 0.25;
  firstPerson = false;
};

ItemData HeadSet 
{
  description = "Right Eyepiece";
  shapeFile = "paintgun";
  className = "Backpack";
  heading = $InvHead[ihWea];
  shadowDetailMask = 4;
  imageType = HeadSetImage;
  price = 7;
  hudIcon = "sniper";
  showWeaponBar = false;
  hiliteOnActive = false;
  showInventory = false;
};

$InvList[HeadSet2] = 0;
$RemoteInvList[HeadSet2] = 0;

ItemImageData HeadSet2Image 
{
  shapeFile = "paintgun";
  mountPoint = 2;
  mountOffset = { -0.02, 0.25, 0.5 };
  mountRotation = { 3.14, -1.5, 0.4 };
  lightType = 2;
  lightRadius = 0.4;
  lightTime = 0.1;
  lightColor = { 0, 1, 0 };
  mass = 0.25;
  firstPerson = false;
};

ItemData HeadSet2 
{
  description = "Left Eyepiece";
  shapeFile = "paintgun";
  className = "Backpack";
  heading = $InvHead[ihWea];
  shadowDetailMask = 4;
  imageType = HeadSet2Image;
  price = 0;
  hudIcon = "sniper";
  showWeaponBar = false;
  hiliteOnActive = false;
  showInventory = false;
};

$InvList[Laptop] = 1;
$RemoteInvList[Laptop] = 1;

ItemImageData LaptopImage 
{
  shapeFile = "radar_small";
  mountPoint = 2;
  mountOffset = { 0, 0, 0.3 };
  mountRotation = { 1.1, 0, 0 };
  weaponType = 2;
  minEnergy = -1;
  maxEnergy = -1;
  mass = 0.5;
  firstPerson = false;
};

ItemData Laptop 
{
  description = "PCMDS";
  shapeFile = "radar_small";
  className = "Backpack";
  heading = $InvHead[ihBac];
  shadowDetailMask = 4;
  imageType = LaptopImage;
  price = 8;
  hudIcon = "energypack";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function Laptop::IsAvailable(%player)
{
  return (Player::getMountedItem(%player, $BackpackSlot) == Laptop);
}

function Laptop::Error(%client, %msg)
{
  Client::sendMessage(%client, 1, "CMDR: " @ %msg @ "~waccess_denied.wav");
}

function Laptop::Output(%client, %msg)
{
  Client::sendMessage(%client, 1, "CMDR: " @ %msg);
}

function Laptop::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
}

function Laptop::onMount(%player,%item) 
{
  Player::trigger(%player,$BackpackSlot,true);
  Bottomprint(%client, "<f1>The Portable Command Center System\n<f0>  Allows you to control turrets by accessing the commander screen.");
  Player::mountItem(%player,HeadSet,$EyepieceSlot);
  Player::mountItem(%player,HeadSet2,$EyepieceSlot2);
}

function Laptop::onUnmount(%player,%item) 
{
  Player::unmountItem(%player,$EyepieceSlot);
  Player::unmountItem(%player,$EyepieceSlot2);
}

function Laptop::onDrop(%player,%item) {
  Player::setItemCount(%player, HeadSet, 0);
  Player::setItemCount(%player, HeadSet2, 0);
  Item::onDrop(%player,%item);
}