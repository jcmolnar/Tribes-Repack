//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Psionic Cloak
//
//  For installation information, see Install.txt
//  Created by <[DC]>Paladin
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Cloak] = 1;
$RemoteInvList[Cloak] = 1;

ItemImageData CloakImage 
{
  shapeFile = "sensor_small";
  mountPoint = 2;
  weaponType = 2;
  minEnergy = -1;
  maxEnergy = -2;
  sfxFire = SoundJammerOn;
  mountOffset = { 0, -0.05, 0 };
  mountRotation = { 0, 0, 0 };
  firstPerson = false;
};

ItemData Cloak 
{
  description = "Conceal";
  shapeFile = "sensorjampack";
  className = "Backpack";
  heading = $InvHead[ihPsi];
  shadowDetailMask = 4;
  imageType = CloakImage;
  price = 15;
  hudIcon = "sensorjamerpack";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function CloakImage::onActivate(%player,%imageSlot) 
{
  GameBase::startFadeout(%player);
  Client::sendMessage(Player::getClient(%player),0,"Chameleon Skin Forming");
  %rate = Player::getSensorSupression(%player) + 5;
  Player::setSensorSupression(%player,%rate);
  %player.guiLock = true;
  %c = Player::getClient(%player);
  %c.guiLock = true;
  %clientId.ghostDoneFlag = true;
  startGhosting(%cl);
}

function CloakImage::onDeactivate(%player,%imageSlot) 
{
  GameBase::startFadein(%player);
  Client::sendMessage(Player::getClient(%player),0,"Chameleon Skin Deforming");
  %rate = Player::getSensorSupression(%player) - 5;
  Player::setSensorSupression(%player,%rate);
  Player::trigger(%player,$BackpackSlot,false);
  %player.guiLock = "";
  %c = Player::getClient(%player);
  %c.guiLock = "";
}

function Cloak::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "This power is used like a backpack, but is far superior to other forms of \"cloaking\", due to the length it lasts.");
}