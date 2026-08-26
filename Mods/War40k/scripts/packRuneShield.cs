//by Edgecrusher
$InvList[RuneShield] = 1;
$RemoteInvList[RuneShield] = 1;

ItemImageData RuneShieldImage 
{
  shapeFile = "shieldPack";
  mountPoint = 2;
  weaponType = 2;
  minEnergy = -1;
  maxEnergy = -2;
  sfxFire = SoundShieldOn;
  firstPerson = false;
lightType = 2;   // Pulsing
	lightRadius = 4;
	lightTime = 0.5;
	lightColor = { 3, 1.25, 0.25 };
}
;
ItemData RuneShield 
{
  description = "Rune Shield";
  shapeFile = "shieldPack";
  className = "Backpack";
  heading = $InvHead[ihPsi];
  shadowDetailMask = 4;
  imageType = RuneShieldImage;
  price = 20;
  hudIcon = "shieldpack";
  showWeaponBar = true;
  hiliteOnActive = true;
}
;
function RuneShieldImage::onActivate(%player,%imageSlot) 
{
  Client::sendMessage(Player::getClient(%player),0,"Rune Aura Focused.");
  %player.shieldStrength = 0.012;
}
function RuneShieldImage::onDeactivate(%player,%imageSlot) 
{
  Client::sendMessage(Player::getClient(%player),0,"Rune Aura Dispelled.");
  Player::trigger(%player,$BackpackSlot,false);
  %player.shieldStrength = 0;
}
