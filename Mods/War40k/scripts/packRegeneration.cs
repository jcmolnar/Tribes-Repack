$InvList[RegenerationPack] = 1;
$RemoteInvList[RegenerationPack] = 1;

RepairEffectData AutoBolt 
{
  bitmapName = "repairadd.bmp";
  boltLength = 5.0;
  segmentDivisions = 4;
  beamWidth = 0.125;
  updateTime = 450;
  skipPercent = 0.6;
  displaceBias = 0.15;
  lightRange = 3.0;
  lightColor = { 0.85, 0.25, 0.25 };
};

function AutoBolt::onAcquire(%this, %player, %target) 
{
  %client = Player::getClient(%player);
  if (%target == %player) 
  {
    %player.repairTarget = -1;
    if (GameBase::getDamageLevel(%player) != 0) 
    {
      %player.repairRate = 0.9;
      %player.repairTarget = %player;
      Client::sendMessage(%client, 0, "AutoRepair On");
    }
    
  }
  %rate = GameBase::getAutoRepairRate(%player.repairTarget) + %player.repairRate;
  GameBase::setAutoRepairRate(%player.repairTarget,%rate);
}

function AutoBolt::onRelease(%this, %player) 
{
  %object = %player.repairTarget;
  if (%object != -1) 
  {
    %client = Player::getClient(%player);
    if (%object == %player) 
      Client::sendMessage(%client,0,"AutoRepair Off");
    %rate = GameBase::getAutoRepairRate(%object) - %player.repairRate;
    if (%rate < 0) %rate = 0;
    GameBase::setAutoRepairRate(%object,%rate);
  }
}

function AutoBolt::checkDone(%this, %player) 
{
  if (%player.repairTarget != -1) 
  {
    %object = %player.repairTarget;
    if (%object == %player) 
    {
      if (GameBase::getDamageLevel(%player) == 0) 
      {
        Player::trigger(%player,$BackpackSlot,false);
        return;
      }
    }
  }
}

ItemImageData RegenerationPackImage 
{
  shapeFile = "armorkit";
  mountPoint = 2;
  weaponType = 2;
  minEnergy = 8;
  maxEnergy = 14;
  sfxFire = SoundRepairItem;
  projectileType = AutoBolt;
lightType = 2;   // Pulsing
	lightRadius = 4;
	lightTime = 0.5;
	lightColor = { 1, 0, 0 };
}
;
ItemData RegenerationPack 
{
  description = "Regeneration Pack";
  shapeFile = "armorkit";
  className = "Backpack";
  heading = $InvHead[ihBac];
  shadowDetailMask = 4;
  imageType = RegenerationPackImage;
  price = 15;
  hudIcon = "shieldpack";
  showWeaponBar = true;
  hiliteOnActive = true;
}
;
function RegenerationPackImage::onActivate(%player,%imageSlot) 
{
  %clientId = Player::getClient(%player);
}
function RegenerationPackImage::onDeactivate(%player,%imageSlot) 
{
  Player::trigger(%player,$BackpackSlot,false);
}
