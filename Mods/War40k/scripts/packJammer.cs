$InvList[SensorJammerPack] = 1;
$RemoteInvList[SensorJammerPack] = 1;

ItemImageData SensorJammerPackImage 
{
  shapeFile = "sensorjampack";
  mountPoint = 2;
  weaponType = 2;
  maxEnergy = 5;
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

ItemData SensorJammerPack 
{
  description = "Sensor Jammer Pack";
  shapeFile = "sensorjampack";
  className = "Backpack";
  heading = $InvHead[ihBac];
  shadowDetailMask = 4;
  imageType = SensorJammerPackImage;
  price = 7;
  hudIcon = "sensorjamerpack";
  showWeaponBar = true;
  hiliteOnActive = true;
}
;
function SensorJammerPackImage::onActivate(%player,%imageSlot) 
{
  Client::sendMessage(Player::getClient(%player),0,"Sensor Jammer On");
  %rate = Player::getSensorSupression(%player) + 20;
  Player::setSensorSupression(%player,%rate);
}
function SensorJammerPackImage::onDeactivate(%player,%imageSlot) 
{
  Client::sendMessage(Player::getClient(%player),0,"Sensor Jammer Off");
  %rate = Player::getSensorSupression(%player) - 20;
  Player::setSensorSupression(%player,%rate);
  Player::trigger(%player,$BackpackSlot,false);
}
