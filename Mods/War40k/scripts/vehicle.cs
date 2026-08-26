
function Vehicle::onAdd(%this) 
{
  %this.shieldStrength = 0.0;
  GameBase::setRechargeRate (%this, 10);
  GameBase::setMapName (%this, "Vehicle");
}

function Vehicle::onCollision (%this, %object) 
{
  %vehname = GameBase::getDataName(%this);
  if(GameBase::getDamageLevel(%this) < %vehname.maxDamage) 
  {
    if (getObjectType (%object) == "Player" && (getSimTime() > %object.newMountTime || %object.lastMount != %this) && %this.fading == "") 
    {
      %armor = Player::getArmor(%object);
      %client = Player::getClient(%object);

       // Try to pilot the vehicle
      if (($VehicleUse[%armor, %vehname] & $CanPilot) && Vehicle::canMount(%this, %object))
      {
        %this.Pilot = %object;

         // Unmount the player's weapon as the pilot
        %weapon = Player::getMountedItem(%object,$WeaponSlot);
        if(%weapon != -1) 
        {
          %object.lastWeapon = %weapon;
          Player::unMountItem(%object,$WeaponSlot);
        }

        Player::setMountObject(%object, %this, 1);
        Client::setControlObject(%client, %this);
        playSound(%vehname.mountSound, GameBase::getPosition(%this));
        %object.driver= 1;
        %object.vehicle = %this;
        %this.clLastMount = %client;

//        eval(%vehname @ "::onPilot(" @ %this @ ", " @ %object @ ");");
        GameBase::virtual(%this, onPilot, %this, %object);
      }

       // Check if it is too small to ride
      else if ($VehicleSlots[%vehname] == 0)
        Client::sendMessage(Player::getClient(%object),0,"Cannot pilot this vehicle in your current armor class.~wError_Message.wav");

       // Try to ride the vehicle
      else if ($VehicleUse[%armor, %vehname] & $CanRide) 
      {
        %mountSlot= Vehicle::findEmptySeat(%this,%client);
        if(%mountSlot) 
        {
          %object.vehicleSlot = %mountSlot;
          %object.vehicle = %this;
          Player::setMountObject(%object, %this, %mountSlot);
          playSound (GameBase::getDataName(%this).mountSound, GameBase::getPosition(%this));
        }
        else
          Client::sendMessage(Player::getClient(%object),0,"No slot.  Dismount the vehicle to free it for takeoff.~wError_Message.wav");
      }
      else if (GameBase::getControlClient(%this) == -1) 
        Client::sendMessage(Player::getClient(%object),0,"Cannot pilot or ride this vehicle in your current armor class.~wError_Message.wav");
    }
  }
}

function Vehicle::findEmptySeat(%this,%client) 
{
  %numSlots = $VehicleSlots[GameBase::getDataName(%this)];
  %count=0;
  for(%i=0; %i<%numSlots; %i++) 
    if(%this.Seat[%i] == "") 
    {
      %slotPos[%count] = Vehicle::getMountPoint(%this,%i+2);
      %slotVal[%count] = %i+2;
      %lastEmpty = %i+2;
      %count++;
    }
  if(%count == 1) 
  {
    %this.Seat[%lastEmpty-2] = %client;
    return %lastEmpty;
  }
  else if (%count > 1) 
  {
    %freeSlot = %slotVal[getClosestPosition(%count,GameBase::getPosition(%client),%slotPos[0],%slotPos[1],%slotPos[2],%slotPos[3])];
    %this.Seat[%freeSlot-2] = %client;
    return %freeSlot;
  }
  else return "False";
}

function getClosestPosition(%num,%playerPos,%slotPos0,%slotPos1,%slotPos2,%slotPos3) 
{
  %playerX = getWord(%playerPos,0);
  %playerY = getWord(%playerPos,1);
  for(%i = 0; %i<%num; %i++) 
  {
    %x = (getWord(%slotPos[%i],0)) - %playerX;
    %y = (getWord(%slotPos[%i],1)) - %playerY;
    if(%x < 0) %x *= -1;
    if(%y < 0) %y *= -1;
    %newDistance = sqrt((%x*%x)+(%y*%y));
    if(%newDistance < %distance || %distance == "") 
    {
      %distance = %newDistance;
      %closePos = %i;
    }
  }
  return %closePos;
}

function Vehicle::passengerJump(%this,%passenger,%mom) 
{
   // May need to work on this
  %height = 2;
  %velocity = 140;
  %zVec = 110;

//  %armor = Player::getArmor(%passenger);
//  if(%armor == "larmor" || %armor == "lfemale") 
//  {
//    %height = 2;
//    %velocity = 70;
//    %zVec = 70;
//  }
//  else if(%armor == "marmor" || %armor == "mfemale") 
//  {
//    %height = 2;
//    %velocity = 100;
//    %zVec = 100;
//  }
//  else if(%armor == "harmor") 
//  {
//    %height = 2;
//    %velocity = 140;
//    %zVec = 110;
//  }
//  else 
//  {
//   %height = 2;
//    %velocity = 70;
//    %zVec = 70;
//  }

  %pos = GameBase::getPosition(%passenger);
  %posX = getWord(%pos,0);
  %posY = getWord(%pos,1);
  %posZ = getWord(%pos,2);
  if(GameBase::testPosition(%passenger,%posX @ " " @ %posY @ " " @ (%posZ + %height))) 
  {
    %client = Player::getClient(%passenger);
    %this.Seat[%passenger.vehicleSlot-2] = "";
    %passenger.vehicleSlot = "";
    %passenger.vehicle= "";
    Player::setMountObject(%passenger, -1, 0);
    %rotZ = getWord(GameBase::getRotation(%passenger),2);
    GameBase::setRotation(%passenger, "0 0 " @ %rotZ);
    GameBase::setPosition(%passenger,%posX @ " " @ %posY @ " " @ (%posZ + %height));
    %jumpDir = Vector::getFromRot(GameBase::getRotation(%passenger),%velocity,%zVec);
    Player::applyImpulse(%passenger,%jumpDir);
  }
  else Client::sendMessage(Player::getClient(%passanger),0,"Can not dismount - Obstacle in the way.~wError_Message.wav");
}

function Vehicle::jump(%this,%mom) 
{
  Vehicle::dismount(%this,%mom);
}

function Vehicle::dismount(%this,%mom) 
{
  %cl = GameBase::getControlClient(%this);
  if(%cl != -1) 
  {
    %pl = Client::getOwnedObject(%cl);
    if(getObjectType(%pl) == "Player") 
    {
      if(GameBase::testPosition(%pl, Vehicle::getMountPoint(%this,0))) 
      {
        %pl.lastMount = %this;
        %pl.newMountTime = getSimTime() + 3.0;
        Player::setMountObject(%pl, %this, 0);
        Player::setMountObject(%pl, -1, 0);
        %rot = GameBase::getRotation(%this);
        %rotZ = getWord(%rot,2);
        GameBase::setRotation(%pl, "0 0 " @ %rotZ);
        Player::applyImpulse(%pl,%mom);
        Client::setControlObject(%cl, %pl);
        playSound (GameBase::getDataName(%this).dismountSound, GameBase::getPosition(%this));
        if(%pl.lastWeapon != "") 
        {
          Player::useItem(%pl,%pl.lastWeapon);
          %pl.lastWeapon = "";
        }
        %this.Pilot = "";
        GameBase::virtual(%this, onUnPilot, %this, %pl);
        %pl.driver = "";
        %pl.vehicle = "";
      }
      else Client::sendMessage(%cl,0,"Can not dismount - Obstacle in the way.~wError_Message.wav");
    }
  }
}

function Vehicle::onDestroyed (%this,%mom) 
{
  %this.Pilot = "";
  $TeamItemCount[GameBase::getTeam(%this) @ $VehicleToItem[GameBase::getDataName(%this)]]--;
  %cl = GameBase::getControlClient(%this);
  %pl = Client::getOwnedObject(%cl);
  if(%pl != -1 && getObjectType(%pl) == "Player") 
  {
    Player::setMountObject(%pl, -1, 0);
    Client::setControlObject(%cl, %pl);
    if(%pl.lastWeapon != "") 
    {
      Player::useItem(%pl,%pl.lastWeapon);
      %pl.lastWeapon = "";
    }
    %pl.driver = "";
    %pl.vehicle = "";
  }
  for (%i = 0; %i < 4; %i++) 
   if(%this.Seat[%i] != "") 
    {
      %pl = Client::getOwnedObject(%this.Seat[%i]);
      Player::setMountObject(%pl, -1, 0);
      Client::setControlObject(%this.Seat[%i], %pl);
      %pl.vehicleslot = "";
      %pl.vehicle = "";
    }
  calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 2, 0.55, 0.1, 225, 100);
}

function Vehicle::onDamage(%this,%type,%value,%pos,%vec,%mom,%object) 
{
  %value *= $damageScale[GameBase::getDataName(%this), %type];
  StaticShape::onDamage(%this,%type,%value,%pos,%vec,%mom,%object);
}

function Vehicle::getHeatFactor(%this) 
{
  return 1.0;
}
