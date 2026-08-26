//by Edgecrusher (not for final release, will be perma-mounted on Hawk armor in final)
$InvList[HawkPack] = 1;
$RemoteInvList[HawkPack] = 1;

$NumDepend[HawkPack] = 1;
$Depends[HawkPack, 0] = iarmorSwHawk;

ItemImageData HawkPackImage 
{
  shapeFile = "Hawk_Wings";
  mountPoint = 2;
  weaponType = 2;
  maxEnergy = 0;
  sfxFire = SoundJammerOn;
  mountOffset = 
  {
    0, -0.05, 0 }
  ;
  mountRotation = 
  {
    0, 0, 0 }
  ;
  firstPerson = false;
};

ItemData HawkPack 
{
  description = "Hawk Flight Pack";
  shapeFile = "Hawk_Wings";
  className = "Backpack";
  heading = $InvHead[ihBac];
  shadowDetailMask = 4;
  imageType = HawkPackImage;
  price = 7;
  hudIcon = "sensorjamerpack";
  showWeaponBar = true;
  hiliteOnActive = true;
}
;
function HawkPackImage::onActivate(%player,%imageSlot) 
{
  %rechRateStr = GameBase::getRechargeRate(%player);
  if(%rechRateStr != 0)
  {
    %rechRateStr += 4;
    GameBase::setRechargeRate(%player, %rechRateStr);
  }
  Client::sendMessage(Player::getClient(%player),0,"Wings on");
}
function HawkPackImage::onDeactivate(%player,%imageSlot) 
{
  %rechRateStr = GameBase::getRechargeRate(%player);
  if(%rechRateStr != 0)
  {
    %rechRateStr -= 4;
    GameBase::setRechargeRate(%player, %rechRateStr);
  }
  Client::sendMessage(Player::getClient(%player),0,"Wings off");
  Player::trigger(%player,$BackpackSlot,false);
}
