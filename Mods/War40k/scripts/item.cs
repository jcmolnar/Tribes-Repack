$ItemPopTime = 30;
$ToolSlot=0;
$WeaponSlot=0;
$BackpackSlot=1;
$FlagSlot=2;
$DefaultSlot=3;

DamageSkinData objectDamageSkins 
{
  bmpName[0] = "dobj1_object";
  bmpName[1] = "dobj2_object";
  bmpName[2] = "dobj3_object";
  bmpName[3] = "dobj4_object";
  bmpName[4] = "dobj5_object";
  bmpName[5] = "dobj6_object";
  bmpName[6] = "dobj7_object";
  bmpName[7] = "dobj8_object";
  bmpName[8] = "dobj9_object";
  bmpName[9] = "dobj10_object";
};

function teamEnergyBuySell(%player,%cost) 
{
  %client = Player::getClient(%player);
  %team = Client::getTeam(%client);
  %station = %player.Station;
  %stationName = GameBase::getDataName(%station);
  if(%stationName == DeployableInvStation || %stationName == DeployableAmmoStation) 
  {
    %station.Energy += %cost;
    if(%station.Energy < 1) %station.Energy = 0;
  }
  else if($TeamEnergy[%team] != "Infinite") 
  {
    $TeamEnergy[%team] += %cost;
    %client.teamEnergy += %cost;
  }
}

function remoteBuyFavorites(%client,%favItem0,%favItem1,%favItem2,%favItem3,%favItem4,%favItem5,%favItem6,%favItem7,%favItem8,%favItem9,%favItem10,%favItem11,%favItem12,%favItem13,%favItem14,%favItem15,%favItem16,%favItem17,%favItem18,%favItem19,%favItem20,%favItem21,%favItem22,%favItem23,%favItem24,%favItem25) 
{
//echo(%client @ " remoteBuyFavorites");
  %player=Client::getownedobject(%client);
  if((%player==-1) || Player::IsDead(%player)) return;
  %time = getIntegerTime(true) >> 4;
  if(%time <= %client.lastBuyFavTime) return;
  %client.lastBuyFavTime = %time;
  %station = (Client::getOwnedObject(%client)).Station;
  if(%station == "" ) return;
    %stationName = GameBase::getDataName(%station);
    if (%stationName == DeployableInvStation || %stationName == DeployableAmmoStation) %energy = %station.Energy;
    else %energy = $TeamEnergy[Client::getTeam(%client)];
    if(%energy == "Infinite" || %energy > 0) 
    {
      %error = 0;
      %bought = 0;
      %max = $TotalItems;
      for (%i = 0; %i < %max; %i = %i + 1) 
      {
        %item = getItemData(%i);
        if (Client::isItemShoppingOn(%client,%item)) 
        {
          %count = Player::getItemCount(%client,%item);
          if(%count) 
          {
            if (%item.className != Armor) teamEnergyBuySell(Client::getOwnedObject(%client),(%item.price * %count));
            Player::setItemCount(%client, %item, 0);
          }
        }
      }

      for (%i = 0; %i < 26; %i++) 
      {
        if(%favItem[%i] != "") 
        {
          %item = getItemData(%favItem[%i]);
          if ((Client::isItemShoppingOn(%client,%item)) && ($ItemMax[Player::getArmor(%client), %item] > Player::getItemCount(%client,%item) || %item.className == Armor)) 
          {
            ECHO("BUY " @ %item);
            if(!buyItem(%client,%item)) %error = 1;
            else %bought++;
          }
        }
      }

      if(%bought) 
      {
        if (%error) Client::sendMessage(%client,0,"~wC_BuySell.wav");
        else Client::SendMessage(%client,0,"~wbuysellsound.wav");
      }
      updateBuyingList(%client);
    }
}

function replenishTeamEnergy(%team) 
{
  $TeamEnergy[%team] += $incTeamEnergy;
  schedule("replenishTeamEnergy(" @ %team @ ");", $secTeamEnergy);
}

function checkResources(%player,%item,%delta,%noMessage) 
{
  %client = Player::getClient(%player);
  %team = Client::getTeam(%client);
  %armor = Player::getArmor(%client);

   // I'd love for this to be moved out of here
  %extraAmmo = 0;
  if (Player::getMountedItem(%client,$BackpackSlot) == ammopack) %extraAmmo = floor($ItemMax[%armor, %item] * $AmmoPackMult);
   // Only increase the delta if they own the ammo pack and are asking for fill up.
  if (%extraAmmo > 0 && %delta == $ItemMax[%armor, %item]) %delta = %delta + %extraAmmo;
  
   // Sap energy out of the station
  if (%client.spawn == "") 
  {
    %energy = $TeamEnergy[%team];
    %station = %player.Station;
    %sName = GameBase::getDataName(%station);
    if(%sName == DeployableInvStation || %sName == DeployableAmmoStation) %energy = %station.Energy;
    if(%energy != "Infinite") 
    {
      if (%item.price * %delta > %energy) %delta = %energy / %item.price;
      if(%delta < 1 ) 
      {
        if(%noMessage == "") Client::sendMessage(%client,0,"Couldn't buy " @ %item.description @ " - "@ %energy @ " Energy points left");
        return 0;
      }
    }
  }
  if($TeamItemMax[%item] != "") 
  {
    if($TeamItemMax[%item] <= $TeamItemCount[%team, %item]) 
    {
      Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
      return 0;
    }
  }
  if (%item.className != Armor && %item.className != Vehicle) 
  {
    %count = Player::getItemCount(%client,%item);
    %max = $ItemMax[(Player::getArmor(%client)), %item] + %extraAmmo;
    if(%delta + %count >= %max) %delta = %max - %count;
  }
  return %delta;
}

function buyItem(%client,%item) 
{
  %player = Client::getOwnedObject(%client);
  %armor = Player::getArmor(%client);
  if ((Client::isItemShoppingOn(%client,%item) || %client.spawn) && ($ItemMax[%armor, %item] || %item.className == Armor || %item.className == Vehicle)) 
  {
    if (%item.className == Armor) 
    {
      %buyarmor = $ArmorType[Client::getGender(%client), %item];
      if (%armor != %buyarmor || Player::getItemCount(%client,%item) == 0) 
      {
        teamEnergyBuySell(%player,$ArmorName[%armor].price);
        if(checkResources(%player,%item,1)) 
        {
          teamEnergyBuySell(%player,$ArmorName[%buyarmor].price * -1);
          Player::setArmor(%client,%buyarmor);
			%station = %player.Station;
			setupShoppingList(%client,%station);
          %client.armortype = %buyarmor;
          checkMax(%client,%buyarmor);
          armorChange(%client);
          Player::setItemCount(%client, $ArmorName[%armor], 0);
          Player::setItemCount(%client, %item, 1);

          if($Needs[%item] != "" && Player::getItemCount(%client,$Needs[%item]) == 0)
          {
            buyItem(%client,$Needs[%item]);
            Client::sendMessage(%client,0,"Bought " @ %item.description @ " - Auto buying " @ $Needs[%item].description);
          }
           // XXX Get any additional data for the ammo pack.  Move into onBuy
          if (Player::getMountedItem(%client,$BackpackSlot) == ammopack) fillAmmoPack(%client);
          return 1;
        }
        teamEnergyBuySell(%player,$ArmorName[%armor].price * -1);
      }
    }
    else if (%item.className == Backpack) 
    {
      %pack = Player::getMountedItem(%client,$BackpackSlot);
      if (%pack != -1) 
      {
        if(%pack == ammopack) checkMax(%client,%armor);
        %ammoItem = %item.imageType.ammoType;
        if(%ammoItem != "") 
        {
          %delta = checkResources(%player,%ammoItem,$ItemMax[%armor, %ammoItem]);
          if(%delta || $testCheats) 
          {
            teamEnergyBuySell(%player,(%ammoItem.price * -1 * %delta));
            Player::incItemCount(%client,%ammoitem,%delta);
          }
        }
        else if($NumDepend[%pack] != "")
        {
          for(%i=0; %i < $NumDepend[%pack]; %i++)
          {
            %desc = $Depends[%pack, %i];
            if (%desc == $ArmorName[%armor])
            {
              Client::sendMessage(%client,0,%armor.description @ " requires " @ %pack.description @ ". Can't sell.");
              return 0;
            }
            else if (Player::getItemCount(%client,%desc) > 0) 
            {
              Client::sendMessage(%client,0,"Sold " @ %pack.description @ " - Auto Selling " @ %desc.description);
              remoteSellItem2(%client, %desc);
            }
          }
        }
        teamEnergyBuySell(%player,%pack.price);
        Player::decItemCount(%client,%pack);
      }
      if (checkResources(%player,%item,1) || $testCheats) 
      {
        teamEnergyBuySell(%player,%item.price * -1);
        Player::incItemCount(%client,%item);
        Player::useItem(%client,%item);
        if(%item == ammopack) fillAmmoPack(%client);
        return 1;
      }
      else if(%pack != -1) 
      {
        teamEnergyBuySell(%player,%pack.price * -1);
        Player::incItemCount(%client,%pack);
        Player::useItem(%client,%pack);
        if(%pack == ammopack) fillAmmoPack(%client);
      }
    }
    else if(%item.className == Weapon) 
    {
      if(checkResources(%player,%item,1)) 
      {
        if($Needs[%item] != "" && Player::getItemCount(%client,$Needs[%item]) == 0)
        {
          buyItem(%client,$Needs[%item]);
          Client::sendMessage(%client,0,"Bought " @ %item.description @ " - Auto buying " @ $Needs[%item].description);
        }
        Player::incItemCount(%client,%item);
        teamEnergyBuySell(%player,(%item.price * -1));
        %ammoItem = %item.imageType.ammoType;
        if(%ammoItem != "") 
        {
          %delta = checkResources(%player,%ammoItem,$ItemMax[%armor, %ammoItem]);
          if(%delta || $testCheats) 
          {
            teamEnergyBuySell(%player,(%ammoItem.price * -1 * %delta));
            Player::incItemCount(%client,%ammoitem,%delta);
          }
        }
        return 1;
      }
      }
    else if(%item.className == Vehicle) 
    {
      %shouldBuy = VehicleStation::checkBuying(%client,%item);
      if(%shouldBuy == 1) 
      {
        teamEnergyBuySell(%player,(%item.price * -1));
        return 1;
      }
      else if(%shouldBuy == 2) return 1;
    }
    else 
    {
      %delta = checkResources(%player,%item,$ItemMax[%armor, %item]);
      if(%delta || $testCheats) 
      {
        teamEnergyBuySell(%player,(%item.price * -1 * %delta));
        Player::incItemCount(%client,%item,%delta);
        return 1;
      }
    }
  }
  return 0;
}

function armorChange(%client) 
{
  %player = Client::getOwnedObject(%client);
  if(%client.respawn == "" && %player.Station != "") 
  {
    %sPos = GameBase::getPosition(%player.Station);
    %pPos = GameBase::getPosition(%client);
    %posX = getWord(%sPos,0);
    %posY = getWord(%sPos,1);
    %posZ = getWord(%pPos,2);
    %vec = Vector::getFromRot(GameBase::getRotation(%player.Station),-1);
    %newPosX = (getWord(%vec,0) * 1) + %posX;
    %newPosY = (getWord(%vec,1) * 1) + %posY;
    GameBase::setPosition(%client, %newPosX @ " " @ %newPosY @ " " @ %posZ);
  }
}

function remoteBuyItem(%client,%type) 
{
  %player=Client::getownedobject(%client);
  if((%player==-1) || Player::IsDead(%player))
    return;

  %item = getItemData(%type);
  if(buyItem(%client,%item)) 
  {
    Client::sendMessage(%client,0,"~wbuysellsound.wav");
    updateBuyingList(%client);
  }
  else Client::sendMessage(%client,0,"You couldn't buy "@ %item.description @"~wC_BuySell.wav");
}

function remoteSellItem(%client,%type) 
{
//echo(%client @ " remoteSellItem");
  %player=Client::getownedobject(%client);
  if((%player==-1) || Player::IsDead(%player))
    return;

  %item = getItemData(%type);
  if (Client::isItemShoppingOn(%client,%item)) 
  {
    if (Player::getItemCount(%client,%item) && %item.className != Armor) 
    {
      %numsell = 1;
      if(%item.className == Ammo || %item.className == HandAmmo) 
      {
        %count = Player::getItemCount(%client, %item);
        if(%count < $SellAmmo[%item]) %numsell = %count;
        else %numsell = $SellAmmo[%item];
      }
      else if (%item == ammopack) checkMax(%client,Player::getArmor(%client));
      else if($TeamItemMax[%item] != "") 
      {
        if(%item.className == Vehicle) $TeamItemCount[(Client::getTeam(%client)) @ %item]--;
      }
      else if($NumDepend[%item] != "")
      {
        %armor = $ArmorName[Player::getArmor(%client)];
        for(%i=0; %i < $NumDepend[%item]; %i++)
        {
          %desc = $Depends[%item, %i];
          if (%desc == %armor)
          {
            Client::sendMessage(%client,0,%armor.description @ " requires " @ %item.description @ ". Can't sell.");
            return 0;
          }
          else if (Player::getItemCount(%client,%desc) > 0) 
          {
            Client::sendMessage(%client,0,"Sold " @ %item.description @ " - Auto Selling " @ %desc.description);
            remoteSellItem2(%client, %desc);
          }
        }
      }
      teamEnergyBuySell(%player,%item.price * %numsell);
      Player::setItemCount(%player,%item,(%count-%numsell));
      updateBuyingList(%client);
      Client::SendMessage(%client,0,"~wbuysellsound.wav");
      return 1;
    }
  }
  Client::sendMessage(%client,0,"Cannot sell item ~wC_BuySell.wav");
}

function remoteSellItem2(%client,%item) 
{
  %player=Client::getownedobject(%client);
  %numsell = 1;
  teamEnergyBuySell(%player,%item.price);
  Player::setItemCount(%player,%item,0);
  updateBuyingList(%client);
  Client::SendMessage(%client,0,"~wbuysellsound.wav");
  return 1;
}

// WARNING WARNING WARNING!!!  This is a VERY WEIRD remote function
// It takes a PLAYER as the first paramter, not a client.  You 
// have been warned!!!!
function remoteUseItem(%player,%type) 
{
  if((%player==-1) || Player::IsDead(%player))
    return;

  %client.throwStrength = 1;
  %item = getItemData(%type);
  if (%item == Backpack) %item = Player::getMountedItem(%player,$BackpackSlot);
  else 
  {
    if (%item == Weapon) %item = Player::getMountedItem(%player,$WeaponSlot);
  }
  Player::useItem(%player,%item);
}

function remoteThrowItem(%client,%type,%strength) 
{
  %player=Client::getownedobject(%client);
  if((%player==-1) || Player::IsDead(%player))
    return;

  %item = getItemData(%type);
  if (%item == Grenade || %item == MineAmmo) 
  {
    if (%strength < 0) %strength = 0;
    else if (%strength > 100) %strength = 100;
    %client.throwStrength = 0.3 + 0.7 * (%strength / 100);
    Player::useItem(%client,%item);
  }
}

function remoteDropItem(%client,%type) 
{
  %player=Client::getownedobject(%client);
  if(%player==-1)
    return;

  if(%player.driver != 1) 
  {
    %client.throwStrength = 1;
    %item = getItemData(%type);
    if (%item == Backpack) 
    {
      %item = Player::getMountedItem(%client,$BackpackSlot);
      Player::dropItem(%client,%item);
    }
    else if (%item == Weapon) 
    {
      %item = Player::getMountedItem(%client,$WeaponSlot);
      Player::dropItem(%client,%item);
    }
    else if (%item == Ammo) 
    {
      %item = Player::getMountedItem(%client,$WeaponSlot);
      if(%item.className == Weapon) 
      {
        %item = %item.imageType.ammoType;
        Player::dropItem(%client,%item);
      }
    }
    else if($NumDepend[%item] != "")
    {
      %armor = $ArmorName[Player::getArmor(%client)];
      for(%i=0; %i < $NumDepend[%item]; %i++)
      {
        %desc = $Depends[%item, %i];
        if (%desc == %armor)
        {
          Client::sendMessage(%client,0,%armor.description @ " requires " @ %item.description @ ". Can't drop.");
          return 0;
        }
      }
    }
    else Player::dropItem(%client,%item);
  }
}

function remoteDeployItem(%client,%type) 
{
  %player=Client::getownedobject(%client);
  if((%player==-1) || Player::IsDead(%player))
    return;

  %item = getItemData(%type);
  Player::deployItem(%client,%item);
}

 //-=-=-=- Ammo handling -=-=-=-

$AmmoCount = 0;

function addAmmo(%weapon, %ammo, %count)
{
  $Ammo_Weapon[$AmmoCount] = %weapon;
  $Ammo_Ammo[$AmmoCount] = %ammo;
  $Ammo_Count[$AmmoCount] = %count;  
  $AmmoCount++;
}

 //-=-=-=- Weapon handling -=-=-=-

$FirstWeapon = "";
$LastWeapon = "";

function addWeapon(%weap)
{
  if ($FirstWeapon == "")
    $FirstWeapon = %weap;
  if ($LastWeapon == "") 
    $LastWeapon = %weap;

  $PrevWeapon[%weap] = $LastWeapon;
  $NextWeapon[%weap] = $FirstWeapon;
  $PrevWeapon[$FirstWeapon] = %weap;
  $NextWeapon[$LastWeapon] = %weap;

  $LastWeapon = %weap;
}

function remoteNextWeapon(%client) 
{
  %player=Client::getownedobject(%client);
  if((%player==-1) || Player::IsDead(%player))
    return;

  %pl = Client::getControlObject(%client);
  if (getObjectType(%pl) != "Player") return;

  %item = Player::getMountedItem(%client,$WeaponSlot);
  if (%item == -1 || $NextWeapon[%item] == "") 
    selectValidWeapon(%client);
  else 
  {
    for (%weapon = $NextWeapon[%item]; %weapon != %item; %weapon = $NextWeapon[%weapon]) 
    {
      if (isSelectableWeapon(%client,%weapon)) 
      {
        Player::useItem(%client,%weapon);
        if (Player::getMountedItem(%client,$WeaponSlot) == %weapon || Player::getNextMountedItem(%client,$WeaponSlot) == %weapon) break;
      }
    }
  }
}

function remotePrevWeapon(%client) 
{
  %player=Client::getownedobject(%client);
  if((%player==-1) || Player::IsDead(%player))
    return;

  %pl = Client::getControlObject(%client);
  if (getObjectType(%pl) != "Player") return;

  %item = Player::getMountedItem(%client,$WeaponSlot);
  if (%item == -1 || $PrevWeapon[%item] == "") 
    selectValidWeapon(%client);
  else 
  {
    for (%weapon = $PrevWeapon[%item]; %weapon != %item; %weapon = $PrevWeapon[%weapon]) 
    {
      if (isSelectableWeapon(%client,%weapon)) 
      {
        Player::useItem(%client,%weapon);
        if (Player::getMountedItem(%client,$WeaponSlot) == %weapon || Player::getNextMountedItem(%client,$WeaponSlot) == %weapon) break;
      }
    }
  }
}

function selectValidWeapon(%client) 
{
  %item = $FirstWeapon;
  for (%weapon = $NextWeapon[%item]; %weapon != %item; %weapon = $NextWeapon[%weapon]) 
  {
    if (isSelectableWeapon(%client,%weapon)) 
    {
      Player::useItem(%client,%weapon);
      break;
    }
  }
}

function isSelectableWeapon(%client,%weapon) 
{
  if (Player::getItemCount(%client,%weapon)) 
  {
    %ammo = $WeaponAmmo[%weapon];
    if (%ammo == "" || Player::getItemCount(%client,%ammo) > 0) return true;
  }
  return false;
}

 //-=-=-=-

function Item::giveItem(%player,%item,%delta) 
{
  %armor = Player::getArmor(%player);
  if($ItemMax[%armor, %item]) 
  {
    %client = Player::getClient(%player);
    if (%item.className == Backpack) 
    {
      if (Player::getMountedItem(%player,$BackpackSlot) == -1) 
      {
        Player::incItemCount(%player,%item);
        Player::useItem(%player,%item);
        Client::sendMessage(%client,0,"You received a " @ %item.description @ " backpack");
        return 1;
      }
      }
    else 
    {
      if (%item.className == Weapon) 
      {
        if (Player::getItemClassCount(%player,"Weapon") >= $MaxWeapons[%armor]) return 0;
      }
      %extraAmmo = 0 ;
      if (Player::getMountedItem(%client,$BackpackSlot) == ammopack && $AmmoPackMax[%item] != "") %extraAmmo = $AmmoPackMax[%item];
      %count = Player::getItemCount(%player,%item);
      if (%count + %delta > $ItemMax[%armor, %item] + %extraAmmo) %delta = ($ItemMax[%armor, %item] + %extraAmmo) - %count;
      if (%delta > 0) 
      {
        Player::incItemCount(%player,%item,%delta);
        if (%count == 0 && $AutoUse[%item]) Player::useItem(%player,%item);
        Client::sendMessage(%client,0,"You received " @ %delta @ " " @ %item.description);
        return %delta;
      }
    }
  }
  return 0;
}

$PickupSound[Ammo] = "SoundPickupAmmo";
$PickupSound[Weapon] = "SoundPickupWeapon";
$PickupSound[Backpack] = "SoundPickupBackpack";
$PickupSound[Repair] = "SoundPickupHealth";

function Item::playPickupSound(%this) 
{
  %item = Item::getItemData(%this);
  %sound = $PickupSound[%item.className];
  if (%sound != "") 
    playSound(%sound,GameBase::getPosition(%this));
  else 
    playSound(SoundPickupItem,GameBase::getPosition(%this));
}

function Item::respawn(%this) 
{
  if (Item::isRotating(%this)) 
  {
    Item::hide(%this,True);
    schedule("Item::hide(" @ %this @ ",false);GameBase::startFadeIn(" @ %this @ ");",$ItemRespawnTime,%this);
  }
  else 
    deleteObject(%this);
}

function Item::onAdd(%this) {}

function Item::onCollision(%this,%object) 
{
  if (getObjectType(%object) == "Player") 
  {
    %item = Item::getItemData(%this);
    %count = Player::getItemCount(%object,%item);
    if (Item::giveItem(%object,%item,Item::getCount(%this))) 
    {
      Item::playPickupSound(%this);
      Item::respawn(%this);
    }
  }
}

function Item::onMount(%player,%item) {}

function Item::onUnmount(%player,%item) 
{
}

function Item::onUse(%player,%item) 
{
  Player::mountItem(%player,%item,$DefaultSlot);
}

function Item::pop(%item) 
{
  GameBase::startFadeOut(%item);
  schedule("deleteObject(" @ %item @ ");",2.5, %item);
}

function Item::onDrop(%player,%item) 
{
  if($matchStarted) 
  {
    if(%item.className != Armor) 
    {
      %obj = newObject("","Item",%item,1,false);
      schedule("Item::Pop(" @ %obj @ ");", $ItemPopTime, %obj);
      addToSet("MissionCleanup", %obj);
      if (Player::isDead(%player)) GameBase::throw(%obj,%player,10,true);
      else 
      {
        GameBase::throw(%obj,%player,15,false);
        Item::playPickupSound(%obj);
      }
      Player::decItemCount(%player,%item,1);
      return %obj;
    }
  }
}

function Item::onDeploy(%player,%item,%pos) {}

function Flag::onUse(%player,%item) 
{
  Player::mountItem(%player,%item,$FlagSlot);
}

ItemImageData FlagImage 
{
  shapeFile = "liqcyl";
  mountPoint = 2;
  mountOffset = { 0, 0, -0.35 };
  mountRotation = { 0, 0, 0 };
  lightType = 2;
  lightRadius = 4;
  lightTime = 1.5;
  lightColor = {1, 1, 1};
  mass = 5.0;
};

ItemData Flag 
{
  description = "Flag";
  shapeFile = "liqcyl";
  imageType = FlagImage;
  showInventory = false;
  shadowDetailMask = 4;
  lightType = 2;
  lightRadius = 4;
  lightTime = 1.5;
  lightColor = { 1, 1, 1 };
  mass = 5.0;
};

ItemData RaceFlag 
{
  description = "Race Flag";
  shapeFile = "flag";
  imageType = FlagImage;
  showInventory = false;
  shadowDetailMask = 4;
  lightType = 2;
  lightRadius = 4;
  lightTime = 1.5;
  lightColor = { 1, 1, 1 };
  mass = 3.0;
};

ItemData Weapon 
{
  description = "Weapon";
  showInventory = false;
};

function Weapon::onUse(%player,%item) 
{
  %ammo = %item.imageType.ammoType;
  if (%ammo == "") 
    Player::mountItem(%player,%item,$WeaponSlot);
  else 
  {
    if (Player::getItemCount(%player,%ammo) > 0) 
      Player::mountItem(%player,%item,$WeaponSlot);
    else 
      Client::sendMessage(Player::getClient(%player),0, strcat(%item.description," has no ammo"));
  }
}

ItemData Tool 
{
  description = "Tool";
  showInventory = false;
};

function Tool::onUse(%player,%item) 
{
  Player::mountItem(%player,%item,$ToolSlot);
}

ItemData Ammo 
{
  description = "Ammo";
  showInventory = false;
};

function Ammo::onDrop(%player,%item) 
{
  if($matchStarted) 
  {
    %count = Player::getItemCount(%player,%item);
    %delta = $SellAmmo[%item];
    if(%count <= %delta) 
    {
      if( %item == BulletAmmo || (Player::getMountedItem(%player,$WeaponSlot)).imageType.ammoType != %item) %delta = %count;
      else %delta = %count - 1;
    }
    if(%delta > 0) 
    {
      %obj = newObject("","Item",%item,%delta,false);
      schedule("Item::Pop(" @ %obj @ ");", $ItemPopTime, %obj);
      addToSet("MissionCleanup", %obj);
      GameBase::throw(%obj,%player,20,false);
      Item::playPickupSound(%obj);
      Player::decItemCount(%player,%item,%delta);
    }
  }
}

 //-=-=-=- Backpack

ItemData Backpack 
{
  description = "Backpack";
  showInventory = false;
};

function Backpack::onUse(%player,%item) 
{
  if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
    Player::mountItem(%player,%item,$BackpackSlot);
  else 
    Player::trigger(%player,$BackpackSlot);
}

 //-=-=-=-

function CountObjects(%set,%name,%num) 
{
  %count = 0;
  for(%i=0; %i < %num; %i++)
  {
    %obj=Group::getObject(%set,%i);
    if(GameBase::getDataName(Group::getObject(%set,%i)) == %name) %count++;
  }
  return %count;
}

function checkDeployArea(%client,%pos) 
{
  %set=newObject("set",SimSet);
  %num=containerBoxFillSet(%set,$StaticObjectType | $ItemObjectType | $SimPlayerObjectType,%pos,1,1,1,1);
  if(!%num) 
  {
    deleteObject(%set);
    return 1;
  }
  else if(%num == 1 && getObjectType(Group::getObject(%set,0)) == "Player") 
  {
    %obj = Group::getObject(%set,0);
    if(Player::getClient(%obj) == %client) Client::sendMessage(%client,0,"Unable to deploy - You're in the way");
    else Client::sendMessage(%client,0,"Unable to deploy - Player in the way");
  }
  else Client::sendMessage(%client,0,"Unable to deploy - Item in the way");
  deleteObject(%set);
  return 0;
}

function Item::deployShape(%player,%name,%shape,%item) 
{
  %client = Player::getClient(%player);
  if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]) 
  {
    if (GameBase::getLOSInfo(%player,3)) 
    {
      %obj = getObjectType($los::object);
      if (%obj == "SimTerrain" || %obj == "InteriorShape" || %obj == "DeployablePlatform") 
      {
        if (Vector::dot($los::normal,"0 0 1") > 0.7) 
        {
          if(checkDeployArea(%client,$los::position)) 
          {
            %sensor = newObject("","Sensor",%shape,true);
            addToSet("MissionCleanup", %sensor);
            GameBase::setTeam(%sensor,GameBase::getTeam(%player));
            GameBase::setPosition(%sensor,$los::position);
            Gamebase::setMapName(%sensor,%name);
            Client::sendMessage(%client,0,%item.description @ " deployed");
            playSound(SoundPickupBackpack,$los::position);
//            reportDeploy(%sensor, %client);
		echo("MSG: ",%client," deployed a " @ %item.description);
            return true;
          }
        }
        else Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
      }
      else Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");
    }
    else Client::sendMessage(%client,0,"Deploy position out of range");
  }
  else Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %name @ "s");
  return false;
}

function checkMax(%client,%armor) 
{
  %weaponflag = 0;
  %numweapon = Player::getItemClassCount(%client,"Weapon");
  if (%numweapon > $MaxWeapons[%armor]) 
    %weaponflag = %numweapon - $MaxWeapons[%armor];
  %max = $TotalItems;
  for (%i = 0;
  %i < %max;
  %i = %i + 1) 
  {
    %item = getItemData(%i);
    %maxnum = $ItemMax[%armor, %item];
    if(%maxnum != "") 
    {
      %numsell = 0;
      %count = Player::getItemCount(%client,%item);
      if(%count > %maxnum) 
      {
        %numsell = %count - %maxnum;
      }
      if (%count > 0 && %weaponflag && %item.className == Weapon) 
      {
        %numsell = 1;
        %weaponflag = %weaponflag - 1;
      }
      if(%numsell > 0) 
      {
       Client::sendMessage(%client,0,"SOLD " @ %numsell @ " " @ %item);
        teamEnergyBuySell(Client::getOwnedObject(%client),(%item.price * %numsell));
        Player::setItemCount(%client, %item, %count - %numsell);
        updateBuyingList(%client);
      }
    }
  }
}

function checkPlayerCash(%client) 
{
  %team = Client::getTeam(%client);
  if($TeamEnergy[%team] != "Infinite") 
  {
    if(%client.teamEnergy > ($InitialPlayerEnergy * -1) ) 
    {
      if(%client.teamEnergy >= 0) %diff = $InitialPlayerEnergy;
      else %diff = $InitialPlayerEnergy + %client.teamEnergy;
      $TeamEnergy[%team] -= %diff;
    }
  }
}

function Mission::reinitData() 
{
  for (%i = 0; %i < 8; %i++) 
//    $TeamItemCount[%i @ BigInvPack] = 0;
  $totalNumCameras = 0;
  $totalNumTurrets = 0;
  for(%i = -1; %i < 8; %i++) 
    $TeamEnergy[%i] = $DefaultTeamEnergy;
}