// Assault Jump Pack
$InvList[AssaultPack] = 1;
$RemoteInvList[AssaultPack] = 1;

$NumDepend[AssaultPack] = 1;
$Depends[AssaultPack, 0] = iarmorAssault;

ItemImageData AssaultPackImage 
{
  shapeFile = "jpack";
  mountPoint = 2;
  weaponType = 2;
  maxEnergy = 0;
  sfxFire = SoundJammerOn;
  mountOffset = 
  {
    0, -0.05, 0.2 }
  ;
  mountRotation = 
  {
    0, 0, 1.62 }
  ;
  firstPerson = false;
};

ItemData AssaultPack 
{
  description = "Assault Jump Pack";
  shapeFile = "jpack";
  className = "Backpack";
  heading = $InvHead[ihBac];
  shadowDetailMask = 4;
  imageType = AssaultPackImage;
  price = 7;
  hudIcon = "sensorjamerpack";
  showWeaponBar = true;
  hiliteOnActive = true;
}
;
function AssaultPackImage::onActivate(%player,%imageSlot) 
{
  %rechRateStr = GameBase::getRechargeRate(%player);
  if(%rechRateStr != 0)
  {
    %rechRateStr += 3;
    GameBase::setRechargeRate(%player, %rechRateStr);
  }
  Client::sendMessage(Player::getClient(%player),0,"Boost Jets on");
}
function AssaultPackImage::onDeactivate(%player,%imageSlot) 
{
  %rechRateStr = GameBase::getRechargeRate(%player);
  if(%rechRateStr != 0)
  {
    %rechRateStr -= 3;
    GameBase::setRechargeRate(%player, %rechRateStr);
  }
  Client::sendMessage(Player::getClient(%player),0,"Boost Jets off");
  %rate = Player::getSensorSupression(%player) - 20;
}
