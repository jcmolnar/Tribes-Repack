$InvList[StealthShieldPack] = 1;
$RemoteInvList[StealthShieldPack] = 1;

ItemImageData StealthShieldPackImage 
{
  shapeFile = "shieldPack";
  mountPoint = 2;
  weaponType = 2;
  minEnergy = 6;
  maxEnergy = 9;
  sfxFire = SoundShieldOn;
  firstPerson = false;
}
;
ItemData StealthShieldPack 
{
  description = "Eversor Pack";
  shapeFile = "shieldPack";
  className = "Backpack";
  heading = $InvHead[ihBac];
  shadowDetailMask = 4;
  imageType = StealthShieldPackImage;
  price = 10;
  hudIcon = "shieldpack";
  showWeaponBar = true;
  hiliteOnActive = true;
}
;
function StealthShieldPackImage::onActivate(%player,%imageSlot) 
{
  Client::sendMessage(Player::getClient(%player),0,"Stealth Activated");
  %rate = Player::getSensorSupression(%player) + 60;
  Player::setSensorSupression(%player,%rate);
}
function StealthShieldPackImage::onDeactivate(%player,%imageSlot) 
{
  Client::sendMessage(Player::getClient(%player),0,"Stealth Deactivated");
  Player::trigger(%player,$BackpackSlot,false);
  %rate = Player::getSensorSupression(%player) - 60;
  Player::setSensorSupression(%player,%rate);
}
