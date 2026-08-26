$InvList[AmmoPack] = 1;
$RemoteInvList[AmmoPack] = 1;

$AmmoPackMult = 0.5;

ItemImageData AmmoPackImage 
{
  shapeFile = "retro_utilpack";
  mountPoint = 2;
  mountOffset = { 0, -0.03, 0 };
  firstPerson = false;
  mass = 1.5;
};


ItemData AmmoPack 
{
  description = "Ammo Pack";
  shapeFile = "retro_utilpack";
  className = "Backpack";
  heading = $InvHead[ihBac];
  imageType = AmmoPackImage;
  shadowDetailMask = 4;
  mass = 1.5;
  elasticity = 0.2;
  price = 8;
  hudIcon = "ammopack";
  showWeaponBar = true;
  hiliteOnActive = true;
};

function AmmoPack::onDrop(%player, %item) 
{
  if ($matchStarted) 
  {
    %item = Item::onDrop(%player,%item);
     // When the pack is dropped, subtract all ammo that can't normally be carried
    for(%i = 0; %i < $AmmoCount; %i++) 
    {
      %numPack = 0;
      %ammoItem = $Ammo_Ammo[%i];
      %maxnum = $ItemMax[Player::getArmor(%player), $Ammo_Ammo[%i]];
      %pCount = Player::getItemCount(%player, %ammoItem);
      if(%pCount > %maxnum) 
      {
        %numPack = %pCount - %maxnum;
        Player::decItemCount(%player,%ammoItem,%numPack);
      }

       // Set the amount that is thrown with the pack
      $Item_Ammo[%item, %i] = %numPack;
    }
  }
}

function AmmoPack::onRemove(%this)
{
   // Flush out the settings array (to conserve memory hopefully)
  for (%i = 0; i < $AmmoCount; %i++)
    $Item_Ammo[%this, %i] = "";  
}

function AmmoPack::onCollision(%this,%object) 
{
  if (getObjectType(%object) == "Player") 
  {
    %item = Item::getItemData(%this);
    %count = Player::getItemCount(%object,%item);
    if (Item::giveItem(%object,%item,Item::getCount(%this))) 
    {
      Item::playPickupSound(%this);
      checkPacksAmmo(%object, %this);
      Item::respawn(%this);
    }
  }
}

function checkPacksAmmo(%player, %item) 
{
  for(%i = 0; %i < $AmmoCount; %i++) 
  {
     // Make sure the player isn't carrying more than they could normally with the AmmoPack
     // If this code holds true for base, you should be able to have a heavy toss a scout
     // a fully loaded ammo back--not sure though.
    %numAdd = $Item_Ammo[%item, %i];
    %maxnum = floor($ItemMax[Player::getArmor(%player), $Ammo_Ammo[%i]] * (1 + $AmmoPackMult));
    if (%numAdd > %maxnum) 
      %numAdd = %maxnum;
    Player::incItemCount(%player, $Ammo_Ammo[%i], %numAdd);
  }
}

function fillAmmoPack(%client) 
{
  %player = Client::getOwnedObject(%client);

  for(%i = 0; %i < $AmmoCount; %i++) 
  {
    %item = $Ammo_Ammo[%i];
    %maxnum = floor($ItemMax[Player::getArmor(%client), $Ammo_Ammo[%i]] * $AmmoPackMult);
    if (%maxnum < 1) continue;
    if ($Ammo_Weapon[%i] != "Misc")
    {
      if (Player::getItemCount(%client,$Ammo_Weapon[%i]) < 1) continue;
    }
    //%maxnum = $AmmoPackMax[%item];
    %maxnum = checkResources(%player,%item,%maxnum);
    if (%maxnum) 
    {
      Player::incItemCount(%client,%item,%maxnum);
      teamEnergyBuySell(%player,%item.price * %maxnum * -1);
    }
  }
}

function AmmoPack::onMount(%player,%item) 
{
  %client = Player::getClient(%player);
  Bottomprint(%client, "The Ammo Pack contains an extraordinary amount of ammo for its user. Weight can prove undesireable");
}
