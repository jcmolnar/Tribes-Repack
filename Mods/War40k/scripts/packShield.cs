$InvList[ShieldPack] = 1;
$RemoteInvList[ShieldPack] = 1;

ItemImageData ShieldPackImage 
{
  shapeFile = "shieldPack";
  mountPoint = 2;
  weaponType = 2;
  minEnergy = 8;
  maxEnergy = 10;
  sfxFire = SoundShieldOn;
  firstPerson = false;
lightType = 2;   // Pulsing
	lightRadius = 4;
	lightTime = 0.5;
	lightColor = { 0, 0.25, 3 };
}
;
ItemData ShieldPack 
{
  description = "Shield Pack";
  shapeFile = "shieldPack";
  className = "Backpack";
  heading = $InvHead[ihBac];
  shadowDetailMask = 4;
  imageType = ShieldPackImage;
  price = 14;
  hudIcon = "shieldpack";
  showWeaponBar = true;
  hiliteOnActive = true;
}
;
function ShieldPackImage::onActivate(%player,%imageSlot) 
{
  Client::sendMessage(Player::getClient(%player),0,"Shield On");
  %player.shieldStrength = 0.012;
}
function ShieldPackImage::onDeactivate(%player,%imageSlot) 
{
  Client::sendMessage(Player::getClient(%player),0,"Shield Off");
  Player::trigger(%player,$BackpackSlot,false);
  %player.shieldStrength = 0;
}
