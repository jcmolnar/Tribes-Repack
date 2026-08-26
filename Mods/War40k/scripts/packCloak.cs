$InvList[CloakingDevice] = 1;
$RemoteInvList[CloakingDevice] = 1;

ItemImageData CloakingDeviceImage 
{
  shapeFile = "sensorjampack";
  mountPoint = 2;
  weaponType = 2;
  minEnergy = 4;
  maxEnergy = 11;
  sfxFire = SoundJammerOn;
  mountOffset = { 0, -0.05, 0 };
  mountRotation = { 0, 0, 0 };
  firstPerson = false;
};

ItemData CloakingDevice 
{
  description = "Cameleoline Cloak";
  shapeFile = "sensorjampack";
  className = "Backpack";
  heading = $InvHead[ihBac];
  shadowDetailMask = 4;
  imageType = CloakingDeviceimage;
  price = 8;
  hudIcon = "sensorjamerpack";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function CloakingDeviceImage::onActivate(%player,%imageSlot) 
{
  GameBase::startFadeout(%player);
  Client::sendMessage(Player::getClient(%player),0,"Cameleoline On");
  %rate = Player::getSensorSupression(%player) + 5;
  Player::setSensorSupression(%player,%rate);
  %player.guiLock = true;
  %c = Player::getClient(%player);
  %c.guiLock = true;
  %clientId.ghostDoneFlag = true;
  startGhosting(%cl);
}

function CloakingDeviceImage::onDeactivate(%player,%imageSlot) 
{
  GameBase::startFadein(%player);
  Client::sendMessage(Player::getClient(%player),0,"Cameleoline Off");
  %rate = Player::getSensorSupression(%player) - 5;
  Player::setSensorSupression(%player,%rate);
  Player::trigger(%player,$BackpackSlot,false);
  %player.guiLock = "";
  %c = Player::getClient(%player);
  %c.guiLock = "";
}

function CloakingDevice::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "The Cameleoline Cloak is woven from special materials which blend into their surroundings.");
}