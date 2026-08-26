$InvList[LightningPack] = 1;
$RemoteInvList[LightningPack] = 1;

LightningData lightningCharge 
{
  bitmapName = "lightningNew.bmp";
  damageType = $ElectricityDamageType;
  boltLength = 40.0;
  coneAngle = 35.0;
  damagePerSec = 0.25;
  energyDrainPerSec = 0.0;
  segmentDivisions = 4;
  numSegments = 5;
  beamWidth = 0.125;
  updateTime = 120;
  skipPercent = 0.5;
  displaceBias = 0.15;
  lightRange = 3.0;
  lightColor = { 0.25, 0.25, 0.85 };
  soundId = SoundELFFire;
};

ItemImageData LightningPackImage 
{
  shapeFile = "shieldpack";
  mountPoint = 2;
  weaponType = 2;
  projectileType = lightningCharge;
  minEnergy = 9;
  maxEnergy = 10;
  reloadTime = 0.2;
  sfxFire = SoundELFIdle;

lightType = 2;   // Pulsing
	lightRadius = 4;
	lightTime = 0.5;
	lightColor = { 10, 10, 10 };
};

ItemData LightningPack 
{
  description = "Electron Pack";
  shapeFile = "shieldpack";
  className = "Backpack";
  heading = $InvHead[ihBac];
  shadowDetailMask = 4;
  imageType = LightningPackImage;
  price = 35;
  hudIcon = "shieldpack";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function LightningPackImage::onActivate(%player,%imageSlot) 
{
  Client::sendMessage(Player::getClient(%player),0,"Lightning Field On");
}

function LightningPackImage::onDeactivate(%player,%imageSlot) 
{
  Client::sendMessage(Player::getClient(%player),0,"Lightning Field Off");
  Player::trigger(%player,$BackpackSlot,false);
}
