

$ItemFavoritesKey = "SEX30";
$TeamItemMax[ScoutVehicle] = 3;
$TeamItemMax[HAPCVehicle] = 1;
$TeamItemMax[LAPCVehicle] = 2;


//----------------------------------------------------------------------------

$ItemPopTime = 30;

$ToolSlot=0;
$WeaponSlot=0;
$BackpackSlot=1;
$FlagSlot=2;
$DefaultSlot=3;

$AutoUse[ChargeGun] = True;

// Global object damage skins (staticShapes Turrets Stations Sensors)
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

//----------------------------------------------------------------------------
// Server side methods
// The client side inventory dialogs call buyItem, sellItem,
// useItem and dropItem through remoteEvals.

function teamEnergyBuySell(%player,%cost)
{
	%client = Player::getClient(%player);
	%team = Client::getTeam(%client);
	// IF - Cost positive selling    IF - Cost Negitive buying 
	%station = %player.Station;
	%stationName = GameBase::getDataName(%station); 
	if(%stationName == DeployableInvStation || %stationName == DeployableAmmoStation) {
		%station.Energy += %cost;			//Remote StationEnergy
		if(%station.Energy < 1)
			%station.Energy = 0;
	}
	else if($TeamEnergy[%team] != "Infinite") { 
		$TeamEnergy[%team] += %cost;    //Total TeamEnergy
 		%client.teamEnergy += %cost;   //Personal TeamEnergy
	}
}
function isWeapon(%weap){
	for (%i = 1; %i < $weaponCount +1; %i = %i + 1) {
		if ($weaponlist[%i] != "" &&  $weaponlist[%i] == %weap) return true;
		}
	return false;
}

function CountPlayerWeapons(%player){
	for (%i = 1; %i < $weaponCount +1; %i = %i + 1) {
		if ($weaponlist[%i] != "" && Player::getItemCount(%player,$weaponlist[%i])) %count+=1;
		}
	return %count;
}

function QuickInv(%client){
	//$Invflag[%client] = 1;
   	//if (!$Grace[%client]) Client::setGuiMode(%client,$GuiModeInventory);

	%player = Client::getOwnedObject(%client);
	setupQuickShoppingList(%client);
	updateQuickBuyingList(%client);	
	Client::sendMessage(%client,0,"Uplink established");
		%weapon = Player::getMountedItem(%player,$WeaponSlot);
		if(%weapon != -1) {
			%player.lastWeapon = %weapon;
			Player::unMountItem(%player,$WeaponSlot);
		}
//InventoryCheck(%client);
}

function SpawnInvTime(%client){
	%client.InvAcc = false;
	%client.spawnGrace = false;
	if (!%client.spawnGrace) Client::sendMessage(%client,0,"Uplink signal lost");
	if (Client::getGuiMode(%client) == $GuiModeInventory)remoteToggleInventoryMode(%client);
	
	}


function InventoryCheck(%client){
			//echo(Client::getGuiMode(%client));
%player = Client::getOwnedObject(%client);
if (!$Grace[%client] && $Invflag[%client] <= 0 )
	QuickInvOff(%client);
if ($quickinv[%player]) schedule("InventoryCheck(" @ %client @ ");", 0.2);
}


function QuickInvOff(%client){

	%player = Client::getOwnedObject(%client);
$quickinv[%player]  = false;
	Client::clearItemShopping(%client);
		Client::sendMessage(%client,0,"Uplink signal lost");
//			if(Client::getGuiMode(%client) != 1){
//				Client::setGuiMode(%client,1);
//			}
		if(Player::getMountedItem(%player,$WeaponSlot) == -1){
			if(%player.lastWeapon != "") {
				//echo(%player.lastWeapon);
				Player::useItem(%player,%player.lastWeapon);		 	
				%player.lastWeapon = "";
	  		}
		}
}

function setupQuickShoppingList(%client)
{
	%max = getNumItems();
	GameBase::playSound(%client, Sensor_Deploy, 0);
		for (%i = 0; %i < %max;  %i++) {
			%item = $Itemlist[%i];
			if(%item != "") {
				Client::setItemShopping(%client, %item);
		// && %item.ShowQuick

			}
		}
}




function updateQuickBuyingList(%client)
{
//Client::setInventoryText(%client, "<f1><jc>Sexual Energy: A bunch!");
%player = Client::getOwnedObject(%client);
%armor = Player::getArmor(%client);
%max = getNumItems();
	for (%i = 0; %i < %max; %i++) {
		%item = $Itemlist[%i];
		if($ArmorName[%armor] != %item &&(%item == LightArmor || %item == MediumArmor || %item == HeavyArmor )) {
			Client::setItemBuying(%client, %item);
			}
		else if($ArmorName[%armor] == %item) {
			Client::setItemShopping(%client, %item);
			}
					if(isWeapon(%item)) {
						if(%item != "" && $ItemMax[%armor, %item] > Player::getItemCount(%player,%item) && CountPlayerWeapons(%player) < $MaxWeapons[%armor])					
							Client::setItemBuying(%client, %item);
					}
		if(%item != "" && $ItemMax[%armor, %item] > Player::getItemCount(%player,%item)&&!isWeapon(%item)){
								client::setItemBuying(%client, %item);
							}
		else {} //Client::setItemShopping(%client, %item);
							


	}

}



function isPlayerBusy(%client)
{
	// Can't buy things if busy shooting.
	%state = Player::getItemState(%client,$WeaponSlot);
	//echo(%state);
	return %state == "Fire" || %state == "Reload";
}

function remoteBuyFavorites(%client,%favItem0,%favItem1,%favItem2,%favItem3,%favItem4,%favItem5,%favItem6,%favItem7,%favItem8,%favItem9,%favItem10,%favItem11,%favItem12,%favItem13,%favItem14,%favItem15,%favItem16,%favItem17,%favItem18,%favItem19)
{
	if (isPlayerBusy(%client))
		return;

   // only can buy fav every 1/2 second
   %time = getIntegerTime(true) >> 4; // int half seconds
   if(%time <= %client.lastBuyFavTime)
      return;

   %client.lastBuyFavTime = %time;
	%station = (Client::getOwnedObject(%client)).Station;
	if(%station != "" ) {
		%stationName = GameBase::getDataName(%station); 
		if(%stationName == DeployableInvStation || %stationName == DeployableAmmoStation) 
			%energy = %station.Energy;
		else 
			%energy = $TeamEnergy[Client::getTeam(%client)];
		if(%energy == "Infinite" || %energy > 0) {
			%error = 0;
			%bought = 0;
			%max = getNumItems();
			for (%i = 0; %i < %max; %i = %i + 1) { 
				%item = getItemData(%i);
				if ($ServerCheats || Client::isItemShoppingOn(%client,%item)|| $TestCheats) {
					%count = Player::getItemCount(%client,%item);
					if(%count) {
						if(%item.className != Armor) 
							teamEnergyBuySell(Client::getOwnedObject(%client),(%item.price * %count));
						Player::setItemCount(%client, %item, 0);  
					}
				}
			}
			for (%i = 0; %i < 20; %i++) { 
				if(%favItem[%i] != "") {
					%item = getItemData(%favItem[%i]);
					if ((Client::isItemShoppingOn(%client,%item)) && ($ItemMax[Player::getArmor(%client),  %item] > Player::getItemCount(%client,%item) || %item.className == Armor)) {
						if(!buyItem(%client,%item))  
							%error = 1;
						else
							%bought++;
					}
				}
		  	}
			if(%bought) {
				if(%error) 
					Client::sendMessage(%client,0,"~wC_BuySell.wav");
				else 
					Client::SendMessage(%client,0,"~wbuysellsound.wav");
			}
			updateBuyingList(%client);
		}
	}
	%player = Client::getOwnedObject(%client);
	if($treed[%player] ||%client.InvAcc || %client.spawnGrace && Client::getGuiMode(%client) == 4) {
		//echo("quick inv faves");
		%energy = "Infinite";
		if(%energy == "Infinite" || %energy > 0) {
			%error = 0;
			%bought = 0;
			%max = getNumItems();
			for (%i = 0; %i < %max; %i = %i + 1) { 
				%item = $Itemlist[%i];
				if ($ServerCheats || Client::isItemShoppingOn(%client,%item)|| $TestCheats) {
					%count = true;
					if(%count) {
						if(%item.className != Armor) 
							teamEnergyBuySell(Client::getOwnedObject(%client),(%item.price * %count));
						Player::setItemCount(%client, %item, 0);  
					}
				}
			}
			for (%i = 0; %i < 20; %i++) { 
				if(%favItem[%i] != "") {
					%item = getItemData(%favItem[%i]);
					if ((Client::isItemShoppingOn(%client,%item)) && ($ItemMax[Player::getArmor(%client),  %item] > Player::getItemCount(%client,%item) || %item.className == Armor)) {
						if(!buyItem(%client,%item))  
							%error = 1;
						else
							%bought++;
					}
				}
		  	}
			if(%bought) {
				if(%error) 
					Client::sendMessage(%client,0,"~wC_BuySell.wav");
				else 
					Client::SendMessage(%client,0,"~wbuysellsound.wav");
			}
			updateBuyingList(%client);
			if ($treed[Client::getOwnedObject(%client)])InvTreeUpdate(%client);
		}
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
	%extraAmmo = 0 ;
	if (Player::getMountedItem(%client,$BackpackSlot) == ammopack && $AmmoPackMax[%item] != "") {
		%extraAmmo = $AmmoPackMax[%item];
		if(%delta == $ItemMax[Player::getArmor(%client), %item]) 
			%delta = %delta + %extraAmmo;
	}
	if($TestCheats == 0 && %client.spawn == "") {
		%energy = $TeamEnergy[%team];
    	%station = %player.Station;
		%sName = GameBase::getDataName(%station);
		if(%sName == DeployableInvStation || %sName == DeployableAmmoStation){
			%energy = %station.Energy;
		}
		if(%energy != "Infinite") {
			if (%item.price * %delta > %energy)	
				%delta = %energy / %item.price; 
			if(%delta < 1 ) {
				if(%noMessage == "")
					Client::sendMessage(%client,0,"Couldn't buy " @ %item.description @ " - "@ %energy @ " Energy points left");
				return 0;
			}
		}
	}
	if(%item.className == Weapon) {
		%armor = Player::getArmor(%client);
		%wcount = Player::getItemClassCount(%client,"Weapon");
		if (Player::getItemClassCount(%client,"Weapon") >= $MaxWeapons[%armor]) {
			Client::sendMessage(%client,0,"To many weapons for " @ $ArmorName[%armor].description @ " to carry");
			return 0;
		}
  	}
	else if(%item == RepairPatch) {
		%pDamage = GameBase::getDamageLevel(%player);
		if(GameBase::getDamageLevel(%player) > 0) 
			return 1;
		return 0;
   }
   else if($TeamItemMax[%item] != "" && !$TestCheats) {
		if($TeamItemMax[%item] <= $TeamItemCount[%team, %item]) {
			Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
			return 0;
		}
	}
	if(%item.className != Armor && %item.className != Vehicle) {
	   %count = Player::getItemCount(%client,%item);
	  	%max = $ItemMax[(Player::getArmor(%client)), %item] + %extraAmmo ;
	   if(%delta + %count >= %max) 
			%delta = %max - %count;
	}
	return %delta;
}

function buyItem(%client,%item)
{
	%player = Client::getOwnedObject(%client);
	%armor = Player::getArmor(%client);
	if (($ServerCheats || Client::isItemShoppingOn(%client,%item) || $TestCheats || %client.spawn) && 
			($ItemMax[%armor, %item] || %item.className == Armor || %item.className == Vehicle || $TestCheats)) {
		if (%item.className == Armor) {
			// Assign armor by requested type & gender 
			%buyarmor = $ArmorType[Client::getGender(%client), %item];
			if(%armor != %buyarmor || Player::getItemCount(%client,%item) == 0)	{
				teamEnergyBuySell(%player,$ArmorName[%armor].price);
				if(checkResources(%player,%item,1)) {
					teamEnergyBuySell(%player,$ArmorName[%buyarmor].price * -1);
					Player::setArmor(%client,%buyarmor);
					checkMax(%client,%buyarmor);
					armorChange(%client);
     				Player::setItemCount(%client, $ArmorName[%armor], 0);  
     				Player::setItemCount(%client, %item, 1);  
					if (Player::getMountedItem(%client,$BackpackSlot) == ammopack) 
						fillAmmoPack(%client);	
					return 1;
				}

				teamEnergyBuySell(%player,$ArmorName[%armor].price * -1);
			}
		}
		else if (%item.className == Backpack) {
			if($TeamItemMax[%item] != "") {						
				if($TeamItemCount[GameBase::getTeam(%client) @ %item] >= $TeamItemMax[%item])
			 	  return 0;
			 }

			// Only one backpack per armor.
			%pack = Player::getMountedItem(%client,$BackpackSlot);
			if (%pack != -1) {
				if(%pack == ammopack) 
					checkMax(%client,%armor);
				else if(%pack == EnergyPack) {
				if(Player::getItemCount(%client,"SoulSucker") > 0 && !$build) {
						Client::sendMessage(%client,0,"Sold Energy Pack - Auto Selling Soul Sucker");

				Player::decItemCount(%client,SoulSucker);
				teamEnergyBuySell(%player,(200));					
					}
				}	
				teamEnergyBuySell(%player,%pack.price);
				Player::decItemCount(%client,%pack);
			}			   
			if (checkResources(%player,%item,1) || $testCheats) {
				teamEnergyBuySell(%player,%item.price * -1);
				Player::incItemCount(%client,%item);
				Player::useItem(%client,%item);									 
				if(%item == ammopack) 
					fillAmmoPack(%client);
				return 1;
			}
			else if(%pack != -1) {
				teamEnergyBuySell(%player,%pack.price * -1);
				Player::incItemCount(%client,%pack);
				Player::useItem(%client,%pack);									 
				if(%pack == ammopack) 
					fillAmmoPack(%client);
			}				 
		}
		else if(%item.className == Weapon) {
			if(checkResources(%player,%item,1)) {
				if(%item == SoulSucker && Player::getItemCount(%client,"EnergyPack") == 0 && !$build) {
					buyItem(%client,"EnergyPack");
					Client::sendMessage(%client,0,"Bought Soul Sucker - Auto buying Energy Pack");
				}
				Player::incItemCount(%client,%item);
				teamEnergyBuySell(%player,(%item.price * -1));
				%ammoItem =  %item.imageType.ammoType; 
				if(%ammoItem != "") {
					%delta = checkResources(%player,%ammoItem,$ItemMax[%armor, %ammoItem]);
					if(%delta || $testCheats) {
						teamEnergyBuySell(%player,(%ammoItem.price * -1 * %delta));
						Player::incItemCount(%client,%ammoitem,%delta);
					}
				}
				return 1;
			}
		}
	 	else if(%item.className == Vehicle) {
		   if($TeamItemCount[GameBase::getTeam(%client) @ %item] < $TeamItemMax[%item]) {
				%shouldBuy = VehicleStation::checkBuying(%client,%item);
				if(%shouldBuy == 1) {
					teamEnergyBuySell(%player,(%item.price * -1));
					return 1;
				}			
 				else if(%shouldBuy == 2)
					return 1;
			}
		}
		else {
			if($TeamItemMax[%item] != "") {						
				if($TeamItemCount[GameBase::getTeam(%client) @ %item] >= $TeamItemMax[%item])
			 	  return 0;
			 }
		    %delta = checkResources(%player,%item,$ItemMax[%armor, %item]);
			 if(%delta || $testCheats) {
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
	if(%client.respawn == "" && %player.Station != "") {
		%sPos = GameBase::getPosition(%player.Station);
		%pPos	= GameBase::getPosition(%client);
		%posX = getWord(%sPos,0);
		%posY = getWord(%sPos,1);
		%posZ = getWord(%pPos,2);
		%vec = Vector::getFromRot(GameBase::getRotation(%player.Station),-1);	
	  	%newPosX = (getWord(%vec,0) * 1) + %posX;		 
		%newPosY = (getWord(%vec,1) * 1) + %posY;
		GameBase::setPosition(%client, %newPosX @ " " @ %newPosY @ " " @ %posZ);
		for(%i=0; %i < 6.28; %i += 1.256) {
			%trans = Vector::add(getBoxCenter(%player), Vector::getFromRot("0 0 " @ %i, 1.3, 1.2));
			Projectile::spawnProjectile("ArmChan", "0 0 1 0 0 0 0 0 1 " @ %trans, %player, "0 0 -2");
		}
	}
}

function remoteBuyItem(%client,%type)
{
	if (isPlayerBusy(%client))
		return;

	%item = getItemData(%type);
	if(buyItem(%client,%item)) {
 		Client::sendMessage(%client,0,"~wbuysellsound.wav");
		updateBuyingList(%client);
		if ($treed[Client::getOwnedObject(%client)])InvTreeUpdate(%client);
	}
	else 
  		Client::sendMessage(%client,0,"You couldn't buy "@ %item.description @"~wC_BuySell.wav");
}

function remoteSellItem(%client,%type)
{
	if (isPlayerBusy(%client))
		return;

	%item = getItemData(%type);
	%player = Client::getOwnedObject(%client);
//players can 2click to use, or script a hot key.
	if(%item == InvPatch && !Client::isItemShoppingOn(%client,%item)) {
		InvPatch::onUse(%player,%item);
		return;
		}

	if ($ServerCheats || Client::isItemShoppingOn(%client,%item) || $TestCheats) {
		if(Player::getItemCount(%client,%item) && %item.className != Armor) {
			%numsell = 1;
			if(%item.className == Ammo || %item.className == HandAmmo) {
				%count = Player::getItemCount(%client, %item);
				if(%count < $SellAmmo[%item]) 
					%numsell = %count; 
				else 
					%numsell = $SellAmmo[%item];
			}
			else if (%item == ammopack) 
				checkMax(%client,Player::getArmor(%client));
			else if($TeamItemMax[%item] != "") {
				if(%item.className == Vehicle) 
					$TeamItemCount[(Client::getTeam(%client)) @ %item]--;
			}
			else if(%item == EnergyPack) { 
				if(Player::getItemCount(%client,"SoulSucker") > 0 && !$build) {
					Client::sendMessage(%client,0,"Sold Energy Pack - Auto Selling Soul Sucker");
					//remoteSellItem(%client,22);	
					//Player::setItemCount(%client,"SoulSucker",0); 	
				Player::decItemCount(%client,soulsucker);
				teamEnergyBuySell(%player,(200));
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
$WaitThrowTime = 1;
function remoteUseItem(%player,%type)
{
	%client = Player::getClient(%player);
	%item = getItemData(%type);
	if($debug) echo(%client@"Use item: " @ %type @ " " @ %item@" guimode "@Client::getGuiMode(%client));
	%client.throwStrength = 1;


	if(%item == "Backpack") {
		//plasmatic
		if (Client::getGuiMode(%client) == $GuiModeInventory)
			Client::sendMessage(%client,0,"Sun Tzu Says NO! ~waccess_denied.wav");
		else %item = Player::getMountedItem(%client,$BackpackSlot);
		}
	else
	{
		if(%item == Weapon) 
			%item = Player::getMountedItem(%client,$WeaponSlot);
	}
	Player::useItem(%client,%item);
}


function remoteThrowItem(%client,%type,%strength)
{
	%player = Client::getOwnedObject(%client);
	if(%player.Station == "" && %player.waitThrowTime + $WaitThrowTime <= getSimTime()) {
		if(GameBase::getControlClient(%player) != -1 || %player.vehicle != "") {
		//if(GameBase::getControlClient(%player) != -1) {
	  		//echo("Throw item: " @ %type @ " " @ %strength);
			%item = getItemData(%type);
			if (%item == Grenade || %item == MineAmmo) {
				if (%strength < 0)
					%strength = 0;
				else
					if (%strength > 100)
						%strength = 100;
				%client.throwStrength = 0.3 + 0.7 * (%strength / 100);
				Player::useItem(%client,%item);
			}
		}
	}
}

function remoteDropItem(%client,%type)
{
	if((Client::getOwnedObject(%client)).driver != 1) {

		%client.throwStrength = 1;

		%item = getItemData(%type);
		//echo("Drop item: ",%item  ," type: ",%type );
		if (%item == Backpack) {
			%item = Player::getMountedItem(%client,$BackpackSlot);
			Player::dropItem(%client,%item);
		}
	    else if (%item == Weapon) {
			%item = Player::getMountedItem(%client,$WeaponSlot);
			Player::dropItem(%client,%item);
		}
		else if (%item == Ammo) {
			%item = Player::getMountedItem(%client,$WeaponSlot);
			if(%item.className == Weapon) {
				%item = %item.imageType.ammoType;
				Player::dropItem(%client,%item);
			}
		}
		else 
			Player::dropItem(%client,%item);
	}
}

function remoteDeployItem(%client,%type)
{
    echo(%client@ " Deploy item: ",%type);
	%item = getItemData(%type);
	Player::deployItem(%client,%item);
}

$FirstWeapon = "";
$LastWeapon = "";
$Item = 0;
$weaponCount = 0;

function AddItem(%Item,%dontallow){
	//damn Dynamix for creating Functions within the .exe and not showing them to me...
	$Item += 1;
	$Itemlist[$Item] = %Item;
	if (!%dontallow) {
	$quickbanlistmax++;
	%item.ShowQuick = true;
	$quickbanlist[$quickbanlistmax] = %item;
	}
}




function addWeapon(%weap)
{
additem(%weap);
additem($WeaponAmmo[%weap]);
$weaponCount += 1;
$weaponlist[$weaponCount] = %weap;


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

function oldAddSpawnWeapon(%weapon)
	{ 
	for(%i=0; true; %i++) {
		echo(%i);
		if ($spawnBuyList[%i] == "") {
			$spawnBuyList[%i] = %weapon;
			$spawnBuyList[%i+1] = $WeaponAmmo[%weapon];
			$spawnBuyList[%i+2] = "";
			$MaxWeapons[larmor]++;
			$MaxWeapons[lfemale]++;
			break;
		}
	}
}


//===========================
// code by Plasmatic
// fall 2001
// ziptiezmail@netscape.net
// please contact me if you would like to use this
//================================

function WeaponKickBack(%player,%push){
//function by Plasmatic
//plasmatica.cjb.net
%trans = GameBase::getMuzzleTransform(%player);
%vel = Item::getVelocity(%player); 
	if (%push < 0){
		
		for(%i=%push; %i < 0; %i++) {
			Projectile::spawnProjectile("KickforwardShell", %trans, %player, %vel);
			}
	}
	for(%i=0; %i < %push; %i++) {
	Projectile::spawnProjectile("KickBackShell", %trans, %player, %vel);
	}
}

//===============================



function e(%client) {
	return !(%client.observerMode == "" || %client.observerMode == "pregame");
}

function remoteNextWeapon(%client)
{

	if(e(%client)) return;

	if(%client.poss) return;
	if(%client.possy) %client = %client.possess;


	%item = Player::getMountedItem(%client,$WeaponSlot);
	if (%item == -1 || %item == "" || $NextWeapon[%item] == "")
		selectValidWeapon(%client);
	else {
		for (%weapon = $NextWeapon[%item]; %weapon != %item;
				%weapon = $NextWeapon[%weapon]) {
			if (isSelectableWeapon(%client,%weapon)) {
				Player::useItem(%client,%weapon);
				// Make sure it mounted (laser may not), or at least
				// next in line to be mounted.
				if (Player::getMountedItem(%client,$WeaponSlot) == %weapon ||
						Player::getNextMountedItem(%client,$WeaponSlot) == %weapon)
					break;
			}
		}
	}
}

function remotePrevWeapon(%client)
{

	if(e(%client)) return;

	if(%client.poss) return;
	if(%client.possy) %client = %client.possess;


	%item = Player::getMountedItem(%client,$WeaponSlot);
	if (%item == -1 || $PrevWeapon[%item] == "")
		selectValidWeapon(%client);
	else {
		for (%weapon = $PrevWeapon[%item]; %weapon != %item;
				%weapon = $PrevWeapon[%weapon]) {
			if (isSelectableWeapon(%client,%weapon)) {
				Player::useItem(%client,%weapon);
				// Make sure it mounted (laser may not), or at least
				// next in line to be mounted.
				if (Player::getMountedItem(%client,$WeaponSlot) == %weapon ||
						Player::getNextMountedItem(%client,$WeaponSlot) == %weapon)
					break;
			}
		}
	}
}

function selectValidWeapon(%client)
{
%player = Client::getOwnedObject(%client);
	//%item = EnergyRifle;
	%item = $FirstWeapon;
if (%player.prevweapon != "" && %player.prevweapon != -1 ) 
	%item = $prevWeapon[%player.prevweapon];
	for (%weapon = $nextWeapon[%item]; %weapon != %item;
			%weapon = $nextWeapon[%weapon]) {
		if (isSelectableWeapon(%client,%weapon)) {
			Player::useItem(%client,%weapon);
			break;
		}
	}
	if (isSelectableWeapon(%client,$FirstWeapon)) {
			Player::useItem(%client,$FirstWeapon);
			break;
	}

}

function isSelectableWeapon(%client,%weapon)
{
	if (Player::getItemCount(%client,%weapon)) {
		%ammo = $WeaponAmmo[%weapon];
		if (%ammo == "" || Player::getItemCount(%client,%ammo) > 0)
			return true;
	}
	return false;
}


//----------------------------------------------------------------------------
// Default item scripts
//----------------------------------------------------------------------------

function Item::giveItem(%player,%item,%delta)
{
	%armor = Player::getArmor(%player);
	if($ItemMax[%armor, %item]) {		  
		%client = Player::getClient(%player);
		if (%item.className == Backpack) {
			// Only one backpack per armor, and it's always mounted
			if (Player::getMountedItem(%player,$BackpackSlot) == -1) {
		 		Player::incItemCount(%player,%item);
		 		Player::useItem(%player,%item);
				Client::sendMessage(%client,0,"You received a " @ %item @ " backpack");
		 		return 1;
			}
		}
  		else {
			// Check num weapons carried by player can't have more then max
			if (%item.className == Weapon) {
				if (Player::getItemClassCount(%player,"Weapon") >= $MaxWeapons[%armor]) 
					return 0;
			}  
			%extraAmmo = 0 ;
			if (Player::getMountedItem(%client,$BackpackSlot) == ammopack && $AmmoPackMax[%item] != "") 
				%extraAmmo = $AmmoPackMax[%item];
			// Make sure it doesn't exceed carrying capacity
			%count = Player::getItemCount(%player,%item);
			if (%count + %delta > $ItemMax[%armor, %item] + %extraAmmo) 
				%delta = ($ItemMax[%armor, %item] + %extraAmmo) - %count;
			if (%delta > 0) {
				Player::incItemCount(%player,%item,%delta);
				if (%count == 0 && $AutoUse[%item]) 
					Player::useItem(%player,%item);
				Client::sendMessage(%client,0,"You received " @ %delta @ " " @ %item.description);
				return %delta;
			}
		}
   }
	return 0;
}


//----------------------------------------------------------------------------
// Default Item object methods

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
	else {
		// Generic item sound
		playSound(SoundPickupItem,GameBase::getPosition(%this));
	}
}	

function Item::respawn(%this)
{
	// If the item is rotating we respawn it,
	if (Item::isRotating(%this)) {
		Item::hide(%this,True);
		schedule("Item::hide(" @ %this @ ",false); GameBase::startFadeIn(" @ %this @ ");",$ItemRespawnTime,%this);
	}
	else { 
		deleteObject(%this);
	}
}	

function Item::onAdd(%this)
{
}

function Item::onCollision(%this,%object)
{
	if (getObjectType(%object) == "Player") {
		%item = Item::getItemData(%this);
		%count = Player::getItemCount(%object,%item);
		if (Item::giveItem(%object,%item,Item::getCount(%this))) {
			Item::playPickupSound(%this);
			Item::respawn(%this);
		}
	}
}


//----------------------------------------------------------------------------
// Default Inventory methods

function Item::onMount(%player,%item)
{
}

function Item::onUnmount(%player,%item)
{
}

function Item::onUse(%player,%item)
{
	//echo("Item used: ",%player," ",%item);
	Player::mountItem(%player,%item,$DefaultSlot);
}

function Item::pop(%item)
{
 	GameBase::startFadeOut(%item);
   schedule("deleteObject(" @ %item @ ");",2.5, %item);
}

//==========================New Code here by Plasmatic
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function Item::onDrop(%player,%item)
{
//Made this function alot more flexable -Plasmatic
	if($matchStarted) {
		if(%item.className != Armor) {
			//echo("Item dropped: ",%player," ",%item);
//crap for posess -Plasmatic
			%client = Player::getClient(%player);
			if(%client.poss && %item.className != Weapon) return;
			if(%client.possy) { %player = Client::getOwnedObject(%client.possess); }
//end posess
			%obj = newObject("","Item",%item,1,false);
 	 	  	schedule("Item::Pop(" @ %obj @ ");", $ItemPopTime, %obj);
 	 	 	addToSet("MissionCleanup", %obj);
		
	if (Player::isDead(%player)) 
		GameBase::throw(%obj,%player,10,true);
	else {
		if(GameBase::getEnergy(%player) < 4 || %Player.rThrow && !%player.rThStr)
			GameBase::throw(%obj,%player,5,true);
		else if(GameBase::getEnergy(%player) < 4 || %Player.rThrow && %player.rThStr)
			GameBase::throw(%obj,%player,%player.rThStr,true);	 			

		else GameBase::throw(%obj,%player,15,false);
		Item::playPickupSound(%obj);
		}
			
	Player::decItemCount(%player,%item,1);
	return %obj;
		}
	}
}


function Item::onDeploy(%player,%item,%pos)
{
	echo("item::ondeploy");
}


//----------------------------------------------------------------------------
// Flags
//----------------------------------------------------------------------------

function Flag::onUse(%player,%item)
{
	Player::mountItem(%player,%item,$FlagSlot);
}


//----------------------------------------------------------------------------

ItemImageData FlagImage
{
	shapeFile = "flag";
	mountPoint = 2;
	mountOffset = { 0, 0, -0.35 };
	mountRotation = { 0, 0, 0 };

	lightType = 2;   // Pulsing
	lightRadius = 4;
	lightTime = 1.5;
	lightColor = { 1, 1, 1};
};

ItemData Flag
{
	description = "Flag";
	shapeFile = "flag";
	imageType = FlagImage;
	showInventory = false;
	shadowDetailMask = 4;
   validateShape = true;

	lightType = 2;   // Pulsing
	lightRadius = 4;
	lightTime = 1.5;
	lightColor = { 1, 1, 1 };
};

ItemData RaceFlag
{
	description = "Race Flag";
	shapeFile = "flag";
	imageType = FlagImage;
	showInventory = false;
	shadowDetailMask = 4;

	lightType = 2;   // Pulsing
	lightRadius = 4;
	lightTime = 1.5;
	lightColor = { 1, 1, 1 };
};

//----------------------------------------------------------------------------
// Vehicles
//----------------------------------------------------------------------------

ItemData ScoutVehicle
{
	description = "Scout";
	className = "Vehicle";
   heading = "aVehicle";
	price = 600;
};

ItemData LAPCVehicle
{
	description = "LPC";
	className = "Vehicle";
   heading = "aVehicle";
	price = 675;
};

ItemData HAPCVehicle
{
	description = "HPC";
	className = "Vehicle";
   heading = "aVehicle";
	price = 875;
};


//----------------------------------------------------------------------------
// Tools, Weapons & ammo
//----------------------------------------------------------------------------

ItemData Weapon
{
	description = "Weapon";
	showInventory = false;
};

function Weapon::onDrop(%player,%item)
{
	%client = Player::getClient(%player);
	if(%client.poss) return;
	if(%client.possy) 
		%player = Client::getOwnedObject(%client.possess);
	%mounted = Player::getMountedItem(%player,$WeaponSlot);
	%state = Player::getItemState(%player,$WeaponSlot);
	if (%state != "Fire" && %state != "Reload")
		Item::onDrop(%player,%item);
}	

function Weapon::onUse(%player,%item)
{
	if(%player.Station=="" && !%player.penis){
		%ammo = %item.imageType.ammoType;
		if (%ammo == "") {
			// Energy weapons dont have ammo types
			Player::mountItem(%player,%item,$WeaponSlot);
		}
		else {
			if (Player::getItemCount(%player,%ammo) > 0) 
				Player::mountItem(%player,%item,$WeaponSlot);
			else {
				Client::sendMessage(Player::getClient(%player),0,
				strcat(%item.description," has no ammo"));
//newnext
			NextWeapon(%player);

			}
		}
	}
}


//----------------------------------------------------------------------------

ItemData Tool
{
	description = "Tool";
	showInventory = false;
};

function Tool::onUse(%player,%item)
{
	Player::mountItem(%player,%item,$ToolSlot);
}



//----------------------------------------------------------------------------

ItemData Ammo
{
	description = "Ammo";
	showInventory = false;
};
////////////////////new code here by Plasmatic///////////////////
//////////////////////////
/////////////////////

function Ammo::onDrop(%player,%item)
{
	if($matchStarted) {
		%count = Player::getItemCount(%player,%item);
		%delta = $SellAmmo[%item];
		if(%count <= %delta) { 
			if( %item == BulletAmmo || (Player::getMountedItem(%player,$WeaponSlot)).imageType.ammoType != %item)
				%delta = %count;
			else 
				%delta = %count - 1;

		}
		if(%delta > 0) {
			%obj = newObject("","Item",%item,%delta,false);
      	schedule("Item::Pop(" @ %obj @ ");", $ItemPopTime, %obj);

      	addToSet("MissionCleanup", %obj);
			if(GameBase::getEnergy(%player) < 4){
			GameBase::throw(%obj,%player,9,true);
	 			}

		else GameBase::throw(%obj,%player,20,false);
			Item::playPickupSound(%obj);
			Player::decItemCount(%player,%item,%delta);
		}
	}
}	

//----------------------------------------------------------------------------
// Backpacks
//----------------------------------------------------------------------------

ItemData Backpack
{				
	description = "Backpack";
	showInventory = false;
};

function Backpack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::trigger(%player,$BackpackSlot);
	}
}


//----------------------------------------------------------------------------

function checkDeployArea(%client,%pos)
{
  	%set=newObject("set",SimSet);
	%num=containerBoxFillSet(%set,$StaticObjectType | $ItemObjectType | $SimPlayerObjectType,%pos,1,1,1,1);
	if(!%num) {
		deleteObject(%set);
		return 1;
	}
	else if(%num == 1 && getObjectType(Group::getObject(%set,0)) == "Player") { 
		%obj = Group::getObject(%set,0);	
		if(Player::getClient(%obj) == %client)	
			Client::sendMessage(%client,0,"Unable to deploy - You're in the way");
		else
			Client::sendMessage(%client,0,"Unable to deploy - Player in the way");
	}
	else
		Client::sendMessage(%client,0,"Unable to deploy - Item in the way");

	deleteObject(%set);
	return 0;	
		

}

//----------------------------------------------------------------------------
// Remote deploy for items

function Item::deployShape(%player,%name,%shape,%item)
{
	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]) {
		if (GameBase::getLOSInfo(%player,3)) {
			// GetLOSInfo sets the following globals:
			// 	los::position
			// 	los::normal
			// 	los::object
			%obj = getObjectType($los::object);
			if (%obj == "SimTerrain" || %obj == "InteriorShape") {
				if (Vector::dot($los::normal,"0 0 1") > 0.7) {
					if(checkDeployArea(%client,$los::position)) {
						%sensor = newObject("","Sensor",%shape,true);
 	        	   	addToSet("MissionCleanup", %sensor);
						GameBase::setTeam(%sensor,GameBase::getTeam(%player));
						GameBase::setPosition(%sensor,$los::position);
						Gamebase::setMapName(%sensor,%name);
						Client::sendMessage(%client,0,%item.description @ " deployed");
						playSound(SoundPickupBackpack,$los::position);
						echo("MSG: ",Client::getName(%client)," cl# ",%client," deployed a ",%name);
						return true;
					}
				}
				else 
					Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
			}
			else 
				Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");
		}
		else 
			Client::sendMessage(%client,0,"Deploy position out of range");
	}
	else
	 	Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %name @ "s");
	return false;
}

//----------------------------------------------------------------------------
// function remoteGiveAll(%clientId)
//----------------------------------------------------------------------------
function remoteGiveAll(%clientId)
{
	if ($TestCheats) {
		Player::setItemCount(%clientId,Blaster,1);
		Player::setItemCount(%clientId,ShotDiscGun,1);
		Player::setItemCount(%clientId,AgedonGun,1);
		Player::setItemCount(%clientId,Miner,1);
		Player::setItemCount(%clientId,Chaingun,1);
		Player::setItemCount(%clientId,PlasmaGun,1);
		Player::setItemCount(%clientId,GrenadeLauncher,1);
		Player::setItemCount(%clientId,DiscLauncher,1);
		Player::setItemCount(%clientId,LaserRifle,1);
		Player::setItemCount(%clientId,EnergyRifle,1);
		Player::setItemCount(%clientId,TargetingLaser,1);
		Player::setItemCount(%clientId,Mortar,1);
		Player::setItemCount(%clientId,SoulSucker,1);
		Player::setItemCount(%clientId,RockLauncher,1);
		Player::setItemCount(%clientId,PencilGun,1);
		Player::setItemCount(%clientId,HoTosser,1);
		Player::setItemCount(%clientId,needlelauncher,1);
		Player::setItemCount(%clientId,HuckerGun,1);
		Player::setItemCount(%clientId,ShotDiscAmmo,200);
		Player::setItemCount(%clientId,AgedonAmmo,200);
		Player::setItemCount(%clientId,MinerAmmo,200);

   %pl = Client::getOwnedObject(%clientId);
   Player::setItemCount(%clientId,DeployableInvPack,1); 
   Player::mountItem(%pl,DeployableInvPack,$BackpackSlot); 

		Player::setItemCount(%clientId,HO,200);
		Player::setItemCount(%clientId,HuckerAmmo,200);
		Player::setItemCount(%clientId,RocketAmmo,200);
		Player::setItemCount(%clientId,PencilAmmo,200);
		Player::setItemCount(%clientId,RockAmmo,200);




	
		Player::setItemCount(%clientId,BulletAmmo,200);
		Player::setItemCount(%clientId,PlasmaAmmo,200);
		Player::setItemCount(%clientId,GrenadeAmmo,200);
		Player::setItemCount(%clientId,DiscAmmo,200);
		Player::setItemCount(%clientId,MortarAmmo,200);

      Player::setItemCount(%clientId,Grenade, 200);
      Player::setItemCount(%clientId,MineAmmo, 200);
		Player::setItemCount(%clientId,Beacon,  200);

		Player::setItemCount(%clientId,RepairKit,200);
	}
	else if($ServerCheats) {
		%armor = Player::getArmor(%clientId);
		Player::setItemCount(%clientId,BulletAmmo,$ItemMax[%armor, BulletAmmo]);
		Player::setItemCount(%clientId,PlasmaAmmo,$ItemMax[%armor, PlasmaAmmo]);
		Player::setItemCount(%clientId,GrenadeAmmo,$ItemMax[%armor, GrenadeAmmo]);
		Player::setItemCount(%clientId,DiscAmmo,$ItemMax[%armor, DiscAmmo]);
		Player::setItemCount(%clientId,MortarAmmo,$ItemMax[%armor, MortarAmmo]);

      Player::setItemCount(%clientId,Grenade, $ItemMax[%armor, Grenade]);
      Player::setItemCount(%clientId,MineAmmo,$ItemMax[%armor, MineAmmo]);
		Player::setItemCount(%clientId,Beacon,$ItemMax[%armor, Beacon]);

		Player::setItemCount(%clientId,RepairKit,1);
	}
}




function checkMax(%client,%armor)
{
 	%weaponflag = 0;
	%numweapon = Player::getItemClassCount(%client,"Weapon");
	if (%numweapon > $MaxWeapons[%armor]) {
	   %weaponflag = %numweapon - $MaxWeapons[%armor];
	}
	%max = getNumItems();
	for (%i = 0; %i < %max; %i = %i + 1) {
		%item = getItemData(%i);
		%maxnum = $ItemMax[%armor, %item];
		if(%maxnum != "") {
			%numsell = 0;
			%count = Player::getItemCount(%client,%item);
			if(%count > %maxnum) {
				%numsell =  %count - %maxnum;
			}
			if (%count > 0 && %weaponflag && %item.className == Weapon) {
				%numsell = 1;
				%weaponflag = %weaponflag - 1;
			}
			if(%numsell > 0) {
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
	if($TeamEnergy[%team] != "Infinite") {
		if(%client.teamEnergy > ($InitialPlayerEnergy * -1) ) {
			if(%client.teamEnergy >= 0)
				%diff = $InitialPlayerEnergy;
			else 
				%diff = $InitialPlayerEnergy + %client.teamEnergy;
			$TeamEnergy[%team] -= %diff;
		}
	}
}

exec(missionreinitdata);


$AutoUse[Blaster] = True;
$Use[Blaster] = True;
$WeaponAmmo[Blaster] = "";

//--------------------------------------


//--------------------------------------
function Blaster::onUse(%player,%item)
{
	bottomprint(Player::getClient(%player), "<f2> Cherry Gun, sends exploding cherries at someone you love.", 10);Weapon::onUse(%player,%item);
}


//--------------------------------------

ItemImageData BlasterImage
{
   shapeFile  = "energygun";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = 0.1;
	minEnergy = 5;
	maxEnergy = 2;

//	projectileType = BlasterBolt;
	accuFire = true;

	sfxFire = SoundFloatMineTarget;//SoundFireBlaster
	sfxActivate = SoundPickUpWeapon;
};

ItemData Blaster
{
   heading = "bWeapons";
	description = "Cherry Gun";
	className = "Weapon";
   shapeFile  = "energygun";
	hudIcon = "blaster";
	shadowDetailMask = 4;
	imageType = BlasterImage;
	price = 85;
	showWeaponBar = true;
};
function BlasterImage::onFire(%player) {

   %energy = GameBase::getEnergy(%player);
   %energy = %energy - 1;
   if (%energy < 1)  %energy = 0;
   GameBase::setEnergy(%player, %energy);

	%trans = GameBase::getMuzzleTransform(%player);

	%vel = Item::getVelocity(%player);
		for(%i=0; %i < 5; %i += 1) {
		Projectile::spawnProjectile("plasmaticShell", %trans, %player, %vel);
		}
		if ($build == 1) {
		Projectile::spawnProjectile("PlasmaticBuildShell", %trans, %player, %vel);
		}


//plasmaticShell
	}




function  BlasterBolt::damagetarget(%target, %timeSlice, %damPerSec, %enDrainPerSec, %pos, %vec, %mom, %shooterId)
{

echo("BLASTER");
   GameBase::applyDamage(%target, $BlasterDamageType, 0.25, %pos, %vec, %mom, %shooterId);
%object = getObjectType(%target);

}




$AutoUse[Chaingun] = True;
$SellAmmo[BulletAmmo] = 25;
$AmmoPackMax[BulletAmmo] = 150;
$AmmoPackItems[0] = BulletAmmo;
$WeaponAmmo[Chaingun] = BulletAmmo;

//--------------------------------------



function Chaingun::onUse(%player,%item)
{
	bottomprint(Player::getClient(%player), "<f2> Pepper gun, Season your foe.", 10);Weapon::onUse(%player,%item);
}





//--------------------------------------

//--------------------------------------

ItemData BulletAmmo
{
	description = "Pulse Bullet";
	className = "Ammo";
	shapeFile = "ammo1";
   heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 1;
};

ItemImageData ChaingunImage
{
	shapeFile = "chaingun";
	mountPoint = 0;

	weaponType = 1; // Spinning
	reloadTime = 0;
	spinUpTime = 0.3;
	spinDownTime = 0.2;
	fireTime = 0.1;

	ammoType = BulletAmmo;
//	projectileType = ChaingunBullet;
	accuFire = False;//true

	lightType = 3;  // Weapon Fire 2//pulsing
	lightRadius = 4;
	lightTime = 3;
	lightColor = { 0.6, 1, 1 };

	sfxFire = SoundFireChaingun;
	sfxActivate = SoundPickUpWeapon;
	sfxSpinUp = SoundSpinUp;
	sfxSpinDown = SoundSpinDown;
};

ItemData Chaingun
{
	description = "Pepper gun";
	className = "Weapon";
	shapeFile = "chaingun";
   validateShape = true;
	hudIcon = "chain";
   heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = ChaingunImage;
	price = 125;
	showWeaponBar = true;
};


function ChaingunImage::onFire(%this) 
{
   %client = GameBase::getOwnerClient(%this);
   %AmmoCount = (Player::getItemCount(%client,BulletAmmo));
   %trans = GameBase::getMuzzleTransform(%this);
   %vel = Item::getVelocity(%this);
	if(%AmmoCount){
	  Player::decItemCount(%client,BulletAmmo);

		%xrnd = floor(getRandom() *10)/100 -0.05;
		%yrnd = floor(getRandom() *10)/100 -0.05;
		%zrnd = floor(getRandom() *10)/100 -0.05;
		%trans1= getWord(%trans,0);
		%trans2= getWord(%trans,1);
		%trans3= getWord(%trans,2);
		%trans4= getWord(%trans,3) + %xrnd;
		%trans5= getWord(%trans,4) + %yrnd;
		%trans6= getWord(%trans,5) + %zrnd;
		%trans7= getWord(%trans,6);
		%trans8= getWord(%trans,7);
		%trans9= getWord(%trans,8);
		%trans10=getWord(%trans,9);
		%trans11=getWord(%trans,10);
		%trans12=getWord(%trans,11);
		
		%NewTrans = %trans1 @" "@ %trans2 @" "@ %trans3 @" "@ %trans4 @" "@ %trans5 @" "@ %trans6 @" "@ %trans7 @" "@ %trans8 @" "@ %trans9 @" "@ %trans10 @" "@ %trans11 @" "@ %trans12;

		Projectile::spawnProjectile("ChaingunBullet", %NewTrans, %this, %vel);
	}
}




///-----------------------------------------------
//
// Code by Plasmatic
// Please get ahold of me if you want to use this
// ziptiezmail@netscape.net
//
///-----------------------------------------------

$SellAmmo[RocketAmmo] = 10;
$AmmoPackMax[RocketAmmo] = 15;
$AmmoPackItems[8] = RocketAmmo;
$WeaponAmmo[needlelauncher] = RocketAmmo;

//--------------------------------------



//--------------------------------------
function needlelauncher::onUse(%player,%item)
{
	bottomprint(Player::getClient(%player), "<f2> Needle Launcher, one shot satisfaction.. if you get a target lock.", 10);Weapon::onUse(%player,%item);

}




RocketData HandRocket { bulletShapeName = "bullet.dts"; explosionTag = bulletExp0; collisionRadius = 0.0; mass = 2.0; damageClass = 1; damageValue = 0.5; damageType = $BulletDamageType; explosionRadius = 7; kickBackStrength = 85.0; muzzleVelocity = 100.0; terminalVelocity = 85.0; acceleration = 5.0; totalTime = 10.0; liveTime = 11.0; lightRange = 5.0; lightColor = { 1.0, 0.7, 0.5 }; inheritedVelocityScale = 0.65; trailType = 2; 
trailType   = 1;
 collideWithOwner   = FALSE;
   trailLength = 15;
   trailWidth  = 0.1;
soundId = SoundJetLight; }; 

SeekingMissileData AvengerMissile { bulletShapeName = "bullet.dts"; 
explosionTag = LargeShockwave; //bulletExp0
collisionRadius = 0.0; mass = 2.0; damageClass = 1; damageValue = 1.5; damageType = $BulletDamageType; //$MissileDamageType
explosionRadius = 9.5; kickBackStrength = 200.0; muzzleVelocity = 80.0; terminalVelocity = 100.0; acceleration = 5.0; totalTime = 20.0; liveTime = 21.0; lightRange = 5.0; lightColor = { 1.0, 0.7, 0.5 }; inheritedVelocityScale = 0.76; seekingTurningRadius = 0.0; nonSeekingTurningRadius = 5.0; proximityDist = 1.5; trailType = 2; 
trailType   = 1;
   trailLength = 15;
   trailWidth  = 0.1;
soundId = SoundJetLight; }; 






function LockWarning(%clientId, %target, %targetType) { 
	%targetId = GameBase::getControlClient(%target);
	%targetName = Client::getName(%targetId); 
	%name = Client::getName(%clientId); 
	if(%targetType == Flier) {
		if(%targetName != "") 
			%msg = "Vehicle Piloted by " @ %targetName; 
		else 
			%msg = "Vehicle"; 
	} else 
		%msg = %targetName;
		//echo(%msg); 
	Client::sendMessage(%clientId,0,"Lock Aquired: " @ %msg @ "~wmine_act.wav");
	if(%targetType == Flier || (Player::getArmor(%target)) != "") {
		Client::sendMessage(%targetId,0,"WARNING - " @ %name @ " has a Needle lock!~waccess_denied.wav");
		schedule("Client::sendMessage(" @ %targetId @ ",0,\"~waccess_denied.wav\");", 0.5);
		schedule("Client::sendMessage(" @ %targetId @ ",0,\"~waccess_denied.wav\");", 1.0); 
	}
} 



ItemData RocketAmmo { description = "Needle"; className = "Ammo"; heading = "xAmmunition"; shapeFile = "rocket"; shadowDetailMask = 4; price = 6; }; 


ItemImageData RocketgunImage { shapeFile = "sniper"; mountPoint = 0; mountRotation = { 0,-1.57, 0 }; weaponType = 0; ammoType = RocketAmmo; accuFire = true; reloadTime = 0.5; fireTime = 0.1; lightType = 3; lightRadius = 3; lightTime = 1; lightColor = { 0.6, 1, 1.0 }; sfxFire = SoundDryFire; sfxActivate = SoundPickUpWeapon; sfxReload = SoundMortarReload; }; 

ItemData NeedleLauncher { description = "Needle Gun"; className = "Weapon"; shapeFile = "sniper"; hudIcon = "blaster"; heading = "bProjectile Weapons"; shadowDetailMask = 4; imageType = RocketgunImage; price = 300; showWeaponBar = true; };

function RocketgunImage::onFire(%player, %slot) { 
	%client = GameBase::getOwnerClient(%player); 
	Player::decItemCount(%player,$WeaponAmmo[needleLauncher],1); 
	%trans = GameBase::getMuzzleTransform(%player); 
	%vel = Item::getVelocity(%player); 
	if(GameBase::getLOSInfo(%player,750)) { 
		%object = getObjectType($los::object); 
		if(%object == "Player" || %object == "Flier") { 
			LockWarning(%client, $los::object, %object);
			 %newObj = Projectile::spawnProjectile("missileSearch", %trans, %player, %vel);
			schedule("fireTheThing(" @ %newObj @ ", " @ %client @ ");", 0.1, %player);
			Projectile::spawnProjectile("AvengerMissile",%trans,%player,%vel,$los::object); 
		} else {
			%client.targetLock = 0;
			Projectile::spawnProjectile("HandRocket",%trans,%player,%vel,%player);
			return;
		}
		
	} else {
		%client.targetLock = 0;
		Projectile::spawnProjectile("HandRocket",%trans,%player,%vel,%player);

	}
} 

function fireTheThing(%newObj, %client) {
	%player = Client::getOwnedObject(%client);
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	
	if(%client.targetLock) {
		//echo("HandRocket");
		Projectile::spawnProjectile("HandRocket",%trans,%player,%vel,%client.targetLock);//LilTurretMissi1e HandRocket
		
	} else {
		//echo("AvengerMissile");
				
	}
	%client.targetLock = 0;
	deleteobject(%newObj);
}

//========================================================


//==========================================================



LightningData missileSearch {
	bitmapName       = "lightningNew.bmp"; //lightningNew.bmp
   	boltLength       = 500.0;
   	coneAngle        = 15.0;
   	damagePerSec = 0;
   	energyDrainPerSec = 0;
   	segmentDivisions = 0;
   	numSegments = 1;
   	beamWidth = 0.25;
   	updateTime   = 30;
   	skipPercent  = 0.5;
   	displaceBias = 0.15;
   	lightRange = 0.0;
   	lightColor = { 0.0, 0.0, 0.0 };
   	isVisible = false;
};
function missileSearch::damageTarget(%target, %timeSlice, %damPerSec, %enDrainPerSec, %pos, %vec, %mom, %shooterId) {
	if(getObjectType(%target) == "Flier" && GameBase::getDataName(%target).shapeFile == "rocket") {
		%shooterId.targetLock = %target;
	}
}
$AutoUse[PlasmaGun] = True;
$SellAmmo[PlasmaAmmo] = 15;
$AmmoPackMax[PlasmaAmmo] = 30;
$AmmoPackItems[1] = PlasmaAmmo;
$WeaponAmmo[PlasmaGun] = PlasmaAmmo;

//--------------------------------------



//--------------------------------------
function PlasmaGun::onUse(%player,%item)
{
	bottomprint(Player::getClient(%player), "<f2> Plasma gun, the standard weapon of choice.", 10);Weapon::onUse(%player,%item);
}
//--------------------------------------


ItemData PlasmaAmmo
{
	description = "Plasma Bolt";
   heading = "xAmmunition";
	className = "Ammo";
	shapeFile = "plasammo";
	shadowDetailMask = 4;
	price = 2;
};

ItemImageData PlasmaGunImage
{
	shapeFile = "plasma";
	mountPoint = 0;
	mountOffset = {0.065, 0, 0.07}; 
	mountRotation = {0, 0, 0}; //-1.57,-0.3

	weaponType = 0; // Single Shot
	ammoType = PlasmaAmmo;
	projectileType = PlasmaBolt;
	accuFire = true;
	reloadTime = 0.1;
	fireTime = 0.1;

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 1, 1, 0.2 };

	sfxFire = SoundFirePlasma;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;
};

ItemData PlasmaGun
{
	description = "Plasma Gun";
	className = "Weapon";
	shapeFile = "plasma";
	hudIcon = "plasma";
   heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = PlasmaGunImage;
	price = 175;
	showWeaponBar = true;
   validateShape = true;
};
///-----------------------------------------------
//
// Code by Plasmatic
// Please get ahold of me if you want to use this
// ziptiezmail@netscape.net
//
///-----------------------------------------------
//--------------------------------------

$SellAmmo[PencilAmmo] = 20;
$AmmoPackMax[PencilAmmo] = 100;
$AmmoPackItems[2] = PencilAmmo;
$WeaponAmmo[PencilGun] = PencilAmmo;

//--------------------------------------



//--------------------------------------
function PencilGun::onUse(%player,%item)
{
	bottomprint(Player::getClient(%player), "<f2> Pencil Gun, sends sharpened #2 pencils at the enemys eye.", 10);Weapon::onUse(%player,%item);
}


//--------------------------------------

ItemData PencilAmmo
{
	description = "Pencil";
	className = "Ammo";
	shapeFile = "force";//mortarammo
   heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 2;
};

ItemImageData PencilGunImage
{
	shapeFile = "sniper";//disc paintgun
	mountPoint = 0;
	//mountOffset = {-0.1,0.1,-0.1};//{0.065, 0.1, -0.1}
	//mountRotation = {0, -1.57, 0}; //-1.57



	weaponType = 0; // 0 singleshot,3 disk, 1 spinning,2 sustained
	ammoType = PencilAmmo;
	projectileType = PencilShell;
	accuFire = true;
	reloadTime = 0.1;
	fireTime = 0;
	//spinUpTime = 0;

	sfxFire = ricochet3;
	sfxActivate = SoundPickUpWeapon;
	//sfxReload = SoundDiscReload;
	//sfxReady = SoundMortarTurretTurn;
};

ItemData PencilGun
{
	description = "Pencil Gun";
	className = "Weapon";
	shapeFile = "sniper";// paintgun
	hudIcon = "mortar";
   heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = PencilGunImage;//DiscLauncherImage
	price = 220;
	showWeaponBar = true;
};
addWeapon(PencilGun);

//-------------------------------------------------------------
// ShotDisc Gun
// Coded by Plasmatic
// Plasmaca.cjb.net
// Ziptiezmail@netscape.net
// Please contact me if you would like to use any part of this.
//--------------------------------------------------


$InvList[ShotDiscGun] = 1;
$RemoteInvList[ShotDiscGun] = 1;

$ItemMax[marmor, ShotDiscGun] = 1;
$ItemMax[mfemale, ShotDiscGun] = 1;
$ItemMax[larmor, ShotDiscGun] = 0;
$ItemMax[lfemale, ShotDiscGun] = 0;
$ItemMax[harmor, ShotDiscGun] = 1;


$ItemMax[marmor, ShotDiscAmmo] = 40;
$ItemMax[mfemale, ShotDiscAmmo] = 40;
$ItemMax[larmor, ShotDiscAmmo] = 0;
$ItemMax[lfemale, ShotDiscAmmo] = 0;
$ItemMax[harmor, ShotDiscAmmo] = 50;

$WeaponAmmo[ShotDiscGun] = "ShotDiscAmmo";

$InvList[ShotDiscAmmo] = 1;
$RemoteInvList[ShotDiscAmmo] = 1;

$SellAmmo[ShotDiscAmmo]			= 10;

$AmmoPackMax[ShotDiscAmmo] = 30;
$AmmoPackItems[13] = ShotDiscAmmo;



//--------------------------------------
ItemData ShotDiscAmmo
{
	description = "Blue Shells";
	className = "Ammo";
heading = "xAmmunition";
	shapeFile = "grenammo";
	shadowDetailMask = 4;
	price = 10;
};

ItemImageData ShotDiscImage
{
	shapeFile = "shotgun";//disc,mortargun
	mountPoint = 0;
	weaponType = 0;
	ammoType = ShotDiscAmmo;
	mountPoint = 0;
	mountRotation = { 0, 0.78, 0 };//1.57
	mountOffset = {-0.065, 0, 0.07}; 
	//projectileType = ShotDiscShell;//messes with imagefire
	accuFire = false;
	reloadTime = 0.75;
	fireTime = 1.5;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
//	lightColor = { 0.6, 1, 1.0 };
   lightColor = { 0.25, 0.25, 0.85 };
	sfxFire = mineExplosion;//turretfire4;//SoundFireMortar;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundActivateAmmoStation;//SoundDiscReload;//SoundMortarReload;
	//sfxReady = AgedonIdle;//SoundMortarIdle;

};



ItemData ShotDiscGun
{
	description = "Blue sex0r";
	className = "Weapon";
	shapeFile = "shotgun";//mortargun
	hudIcon = "mortar";
	heading = "bWeapons";
	shadowDetailMask = 4;
        imageType = ShotDiscImage;
	price = 7500;
	showWeaponBar = true;
};





function ShotDiscGun::onUse(%player,%item)
{
	Weapon::onUse(%player,%item);
	bottomprint(Player::getClient(%player), "<f2> Blue sex, Disc launcher shotgun style.", 10);
}



function ShotDiscImage::onFire(%this) 
{
   %client = GameBase::getOwnerClient(%this);
   %AmmoCount = (Player::getItemCount(%client,ShotDiscAmmo));
   %trans = GameBase::getMuzzleTransform(%this);
   %vel = Item::getVelocity(%this);
	if(%AmmoCount){
	Player::decItemCount(%client,ShotDiscAmmo);
	for(%i=0; %i < 15; %i++) {
		%xrnd = floor(getRandom() *10)/100 -0.05;
		%yrnd = floor(getRandom() *10)/100 -0.05;
		%zrnd = floor(getRandom() *10)/100 -0.05;
		%trans1= getWord(%trans,0);
		%trans2= getWord(%trans,1);
		%trans3= getWord(%trans,2);
		%trans4= getWord(%trans,3) + %xrnd;
		%trans5= getWord(%trans,4) + %yrnd;
		%trans6= getWord(%trans,5) + %zrnd;
		%trans7= getWord(%trans,6);
		%trans8= getWord(%trans,7);
		%trans9= getWord(%trans,8);
		%trans10=getWord(%trans,9);
		%trans11=getWord(%trans,10);
		%trans12=getWord(%trans,11);
		
		%NewTrans = %trans1 @" "@ %trans2 @" "@ %trans3 @" "@ %trans4 @" "@ %trans5 @" "@ %trans6 @" "@ %trans7 @" "@ %trans8 @" "@ %trans9 @" "@ %trans10 @" "@ %trans11 @" "@ %trans12;

		Projectile::spawnProjectile("ShotDicer", %NewTrans, %this, %vel);

       	}
	}
}




///-----------------------------------------------
//
// Code by Plasmatic
// Please get ahold of me if you want to use this
// ziptiezmail@netscape.net
//
///-----------------------------------------------
//--------------------------------------

$AutoUse[HoTosser] = True;
$SellAmmo[HO] = 5;
$AmmoPackMax[HO] = 15;
$AmmoPackItems[14] = HO;
$WeaponAmmo[HoTosser] = HO;

//--------------------------------------

$ItemMax[larmor, HoTosser] = 0;
$ItemMax[lfemale, HoTosser] = 0;
$ItemMax[Marmor, HoTosser] = 0;
$ItemMax[mfemale, HoTosser] = 0;
$ItemMax[Harmor, HoTosser] = 1;

$ItemMax[larmor, HO] = 10;
$ItemMax[lfemale, HO] = 10;
$ItemMax[marmor, HO] = 10;
$ItemMax[mfemale, HO] = 10;
$ItemMax[harmor, HO] = 100;



//--------------------------------------
function HoTosser::onUse(%player,%item)
{
	bottomprint(Player::getClient(%player), "<f2> Home Wrecker, send nitrous filled blow up dolls at your enemy. Earn some extra cash.", 10);Weapon::onUse(%player,%item);
}



//--------------------------------------

ItemData HO
{
	description = "Blow up HO";
	className = "Ammo";
	shapeFile = "lfemale";//grenammo
   heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 2;
};

ItemImageData HoTosserImage
{
	shapeFile = "grenadeL";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	ammoType = HO;
	projectileType = HoShell;
	accuFire = false;
	reloadTime = 0.2;
	fireTime = 0.2;

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = SoundFireGrenade;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;
};

ItemData HoTosser
{
	description = "Home Wrecker";
	className = "Weapon";
	shapeFile = "grenadeL";
	hudIcon = "grenade";
   heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = HoTosserImage;
	price = 150;
	showWeaponBar = true;
   validateShape = true;
};

//--------------------------------------

$AutoUse[Mortar] = True;
$SellAmmo[MortarAmmo] = 5;
$AmmoPackMax[MortarAmmo] = 10;
$AmmoPackItems[6] = MortarAmmo;
$WeaponAmmo[Mortar] = MortarAmmo;

//--------------------------------------



//--------------------------------------
function Mortar::onUse(%player,%item)
{
	bottomprint(Player::getClient(%player), "<f2> Funk Tosser, sends Stink Cans at their feet.", 10);Weapon::onUse(%player,%item);
}

function startPoison(%clientId, %player) { 
	Client::sendMessage(%clientId,1,"You choke on some funk."); 
	%drrate = GameBase::getDamageLevel(%player) + 0.05; 
	GameBase::setDamageLevel(%player, %drrate);
	Player::setDamageFlash(%player,0.5);
			if (Player::isDead(%player)) {
			messageall(0, Client::getName(%clientId) @ " imploded."); 
			%clientId.scoreDeaths++;

 		playNextAnim(%player); 
		Player::blowUp(%player); 
		Player::kill(%player); 
 
			%clientId.score--; 
			Game::refreshClientScore(%clientId); 
			$poisonTime[%clientId] = 0; } 

		if($poisonTime[%clientId] == 0) { 
		Player::setDamageFlash(%player,0.75); 
		$poisonTime[%clientId] = 30; 
		checkPoison(%clientId, %player); 
		} 
	else $poisonTime[%clientId] = 30; 
	} 

function checkPoison(%clientId, %player) { 
	if($poisonTime[%clientId] > 0) { $poisonTime[%clientId] -= 2; 
	%drrate = GameBase::getDamageLevel(%player) + 0.05; 
		if (!Player::isDead(%player)) { GameBase::setDamageLevel(%player, %drrate); 
		Player::setDamageFlash(%player,0.25); 
			if (Player::isDead(%player)) {
			messageall(0, Client::getName(%clientId) @ " imploded."); 
			%clientId.scoreDeaths++;

 		playNextAnim(%player); 
		Player::blowUp(%player); 
		Player::kill(%player); 
 
			%clientId.score--; 
			Game::refreshClientScore(%clientId); 
			$poisonTime[%clientId] = 0; } 
			} 
		else { $poisonTime[%clientId] = 0; 
		} 
		schedule("checkPoison(" @ %clientId @ ", " @ %player @ ");",1,%player); } 
	else { Client::sendMessage(%clientId,1,"The effects of the stink wear off."); 
	} 
} 
addweapon(Mortar);
//-------------------------------------------------------------
// Agedon Gun
//--------------------------------------------------
// Oct 1 - 17, 2001
// Code by Plasmatic
// Please get ahold of me if you want to use any part of this
// ziptiezmail@netscape.net
//-------------------------------------------------------------
// Agedon Gun
//--------------------------------------------------

ItemData AgedonAmmo
{
	description = "Agedon Shells";
	className = "Ammo";
heading = "xAmmunition";
	shapeFile = "grenammo";
	shadowDetailMask = 4;
	price = 10;
};

$InvList[AgedonAmmo] = 1;
$RemoteInvList[AgedonAmmo] = 1;

$SellAmmo[AgedonAmmo]			= 10;

$AmmoPackMax[AgedonAmmo] = 30;
$AmmoPackItems[13] = AgedonAmmo;

ItemImageData AgedonImage
{
	shapeFile = "mortargun";
	mountPoint = 0;
	weaponType = 0;
	ammoType = AgedonAmmo;
	mountPoint = 0;
	mountRotation = { 0, 1.57, 0 };
	mountOffset = {-0.065, 0, 0.07}; 
	//projectileType = AgedonShell;// interferes with image fire
	accuFire = false;
	reloadTime = 0.75;
	fireTime = 1.5;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };
	sfxFire = turretfire4;//SoundFireMortar;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDiscReload;//SoundMortarReload;
	sfxReady = AgedonIdle;//SoundMortarIdle;

};

ItemData AgedonGun
{
	description = "Agedon Gun";
	className = "Weapon";
	shapeFile = "mortargun";
	hudIcon = "mortar";
heading = "bWeapons";
	shadowDetailMask = 4;
        imageType = AgedonImage;
	price = 7500;
	showWeaponBar = true;
};

$InvList[AgedonGun] = 1;
$RemoteInvList[AgedonGun] = 1;

$ItemMax[marmor, AgedonGun] = 1;
$ItemMax[mfemale, AgedonGun] = 1;
$ItemMax[larmor, AgedonGun] = 0;
$ItemMax[lfemale, AgedonGun] = 0;
$ItemMax[harmor, AgedonGun] = 1;

$WeaponAmmo[AgedonGun] = "AgedonAmmo";

$ItemMax[marmor, AgedonAmmo] = 20;
$ItemMax[mfemale, AgedonAmmo] = 20;
$ItemMax[larmor, AgedonAmmo] = 0;
$ItemMax[lfemale, AgedonAmmo] = 0;
$ItemMax[harmor, AgedonAmmo] = 30;

function AgedonImage::onFire(%this) 
{ 
	%client = GameBase::getOwnerClient(%this);
	%AmmoCount = (Player::getItemCount(%client,AgedonAmmo));
  	%trans = GameBase::getMuzzleTransform(%this);
   	%vel = Item::getVelocity(%this);
	if(%AmmoCount){
	Player::decItemCount(%client,AgedonAmmo);
	if(GameBase::getLOSInfo(%this,800)){
		%Dist=Vector::getDistance(GameBase::getPosition(%this),$los::position);
		if(%Dist == 0)  %time = 1;
		else	%time=(%Dist/180)-0.05;
 		%newObj = Projectile::spawnProjectile("AgedonShell", %trans, %this, %vel);
		schedule("SplitAgedon(" @ %newObj @ ", " @ %this @ ");", %time - 0.15);
		schedule("SpawnAgedon(" @ %newObj @ ", " @ %this @ ");", %time);
		
	}
	else{
		%time = 1; 
 		%newObj = Projectile::spawnProjectile("AgedonShell", %trans, %this, %vel);
		schedule("SplitAgedon(" @ %newObj @ ", " @ %this @ ");", %time - 0.15);
		schedule("SpawnAgedon(" @ %newObj @ ", " @ %this @ ");", %time);
	}
	}
}


function SplitAgedon(%newobj,%this){

		%Pos = GameBase::getPosition(%newobj); 
   		%vel = Item::getVelocity(%newobj);
// Pretty shell split

	%trans =  "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%newObj);
 	%obj = Projectile::spawnProjectile("PrettySplit", %trans, %this, %vel);
	Projectile::spawnProjectile(%obj);
	GameBase::setPosition(%obj, %pos);
	Item::setVelocity(%obj, %vel);
}




function SpawnAgedon(%newobj,%this){

		%Pos = GameBase::getPosition(%newobj); 
   		%vel = Item::getVelocity(%newobj);
		%xvel = getWord(%vel,0);
		%yvel = getWord(%vel,1);
		%zvel = getWord(%vel,2);

// Pretty shell split
//
//	%trans =  "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%newObj);
// 	%obj = Projectile::spawnProjectile("PrettySplit", %trans, %this, %vel);
//	Projectile::spawnProjectile(%obj);
//	GameBase::setPosition(%obj, %pos);

// Spawn butterflies

if (GameBase::getPosition(%newObj)){
	for(%i=0; %i < 15; %i += 1) {

		%xrnd = %xvel + floor(getRandom() * 60) -30;
		%yrnd = %yvel + floor(getRandom() * 60) -30;
		%zrnd = %zvel + floor(getRandom() * 60) -30;

	%forceVel = %xrnd@" "@%yrnd@" "@%zrnd;

	%trans =  "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%newObj);
 		%obj = Projectile::spawnProjectile("AgedonGrenadeShell", %trans, %this, %vel);
		Projectile::spawnProjectile(%obj);
		GameBase::setPosition(%obj, %pos);

	Item::setVelocity(%obj, %forceVel);

		}
	deleteobject(%newObj);
	}
}

function AgedonGun::onUse(%player,%item)
{
	Weapon::onUse(%player,%item);
	bottomprint(Player::getClient(%player), "<f2> Agedon Gun, Deadlier that a horde of exploding butterflies.", 10);
}


//--------------------------------------

ItemData MortarAmmo
{
	description = "Stink Can";
	className = "Ammo";
   heading = "xAmmunition";
	shapeFile = "mortarammo";
	shadowDetailMask = 4;
	price = 5;
};

ItemImageData MortarImage
{
	shapeFile = "mortargun";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	ammoType = MortarAmmo;
	projectileType = MortarShell;
	accuFire = false;
	reloadTime = 0.5;
	fireTime = 0;

	lightType = 3;  // Weapon Fire
	lightRadius = 1;
	lightTime = 1;
	lightColor = { 1, 0.1, 0.6 };

	sfxFire = SoundFireMortar;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundMortarReload;
	sfxReady = SoundMortarIdle;
};

ItemData Mortar
{
	description = "Funk Tosser";
	className = "Weapon";
	shapeFile = "mortargun";
	hudIcon = "mortar";
   heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = MortarImage;
	price = 375;
	showWeaponBar = true;
   validateShape = true;
};

///-----------------------------------------------

// Code by Plasmatic
// Please get ahold of me if you want to use this
// ziptiezmail@netscape.net

///-----------------------------------------------
//-----------------------------------

		
$AutoUse[HuckerGun]			= False;
$WeaponAmmo[HuckerGun]		= HuckerAmmo;

//--------------------------------------
// damage scale listed in turret.cs
// for team destroyable setting



$SellAmmo[HuckerAmmo] = 5;
$AmmoPackMax[HuckerAmmo] = 15;
$AmmoPackItems[9] = HuckerAmmo;
$WeaponAmmo[HuckerGun] = HuckerAmmo;

$ItemMax[larmor, HuckerGun] = 0;
$ItemMax[lfemale, HuckerGun] = 0;
$ItemMax[Marmor, HuckerGun] = 0;
$ItemMax[mfemale, HuckerGun] = 0;
$ItemMax[Harmor, HuckerGun] = 1;

$ItemMax[larmor, HuckerAmmo] = 0;
$ItemMax[lfemale, HuckerAmmo] = 0;
$ItemMax[marmor, HuckerAmmo] = 0;
$ItemMax[mfemale, HuckerAmmo] = 0;
$ItemMax[harmor, HuckerAmmo] = 100;

$TeamItemMax[DeployableHucker] = 50;

//----------------------------------
function HuckerGun::onUse(%player,%item)
{	%it = "DeployableHucker";
	bottomprint(Player::getClient(%player), "<f2> Hucker Gun, set up those pesky little turrets like a PRO!!\n Team Max is "@$TeamItemMax[%it]@". You can set "@($TeamItemMax[%it] - $TeamItemCount[GameBase::getTeam(%player) @ %it])@ " more. \n Total Huckers fired this mission ="@$hucky, 10);Weapon::onUse(%player,%item);

}



ItemData HuckerAmmo
{
	description = "Hucker Ammo";
	className = "Ammo";
	shapeFile = "camera";//mortarammo
   heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 2;
};


//--------------------------------------



RocketData HuckerFlier
{	bulletShapeName		= "camera.dts";
	explosionTag		= HuckerSetExp;
	collisionRadius		= 0.0;
	mass				= 1.0;

	damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.7;//0.7
   damageType       = $NoPlayerDamagetype;//
   explosionRadius  = 4;// how close can they set them up without killing others
   kickBackStrength = 80.0;

	kickBackStrength		= 0.1;
	muzzleVelocity		= 200.0;
	terminalVelocity		= 200.0;
	acceleration		= 1.0;
	totalTime			= 22.0;
	liveTime			= 23.0;
	lightRange			= 5.0;
	lightColor			= { 0.2, 0.7, 0.5 };
	inheritedVelocityScale = 0.5;
	// rocket specific
   trailType   = 1;
   trailLength = 15;
   trailWidth  = 0.3;
	soundId			= SoundJetHeavy;
};

ItemImageData HuckerGunImage
{	shapeFile = "mortargun";
	mountPoint		= 0;
	mountOffset		= { 0.15, 0, 0 };
	mountRotation	= { 0, -0.785, 0};
	weaponType		= 0;
	reloadTime		= 0.8;
	fireTime		= 0.7;
	minEnergy		= 15;	
	maxEnergy		= 50;	
	ammoType		= HuckerAmmo;
	accuFire		= true;
	sfxActivate		= SoundPickUpWeapon;
};

ItemData HuckerGun
{	heading			= "bWeapons";
	description			= "Hucker Gun";
	classname			= "Weapon";
	shapeFile 			= "mortargun";
	hudIcon			= "mortar";	
	shadowDetailMask		= 4;
	imageType			= HuckerGunImage;	
	price				= 450;				
	showWeaponBar		= true;	
};


//========================
//Turret info
TurretData DeployableHucker
{
	className = "Turret"; //Turret
	shapeFile = "camera";
   validateShape = true;
   validateMaterials = false;
	projectileType = HuckerBullet;
	maxDamage = 0.6; 	//0.5
	maxEnergy = 60;
	minGunEnergy = 6;
	maxGunEnergy = 5;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	reloadDelay = 0.4;
	speed = 4.0;
	speedModifier = 1.5;
	range = 30;
	visibleToSensor = true;
	shadowDetailMask = 4;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	supressable = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	pinger = true;
	debrisId = flashDebrisMedium;
	shieldShapeName = "shield";
	fireSound = SoundRemoteTurretFire;
	activationSound = SoundRemoteTurretOn;
	deactivateSound = SoundRemoteTurretOff;
	explosionId = flashExpMedium;
	description = "DeployableHucker"; //RemoteTurretMine
	damageSkinData = "objectDamageSkins";
};


//--------------------------------------

function DeployAHucker(%player)
{	%position=$Location[$hucky];
	%rot=$Rotation;
	%Turret = newObject("remoteHucker","Turret",DeployableHucker,true);
	if($traceObj) Echo($Ver,"|Created New Object :",%Turret," Beacon");
	addToSet("MissionCleanup", %Turret);
	GameBase::setTeam(%Turret,GameBase::getTeam(%player));
	GameBase::setRotation(%Turret,%rot);
	GameBase::setPosition(%Turret,%position);
	Gamebase::setMapName(%turret,"Hucker#" @ $totalNumHuckers++ @ " " @ Client::getName(%client));
	 Client::setOwnedObject(%client, %turret); //Give the setter 
	 Client::setOwnedObject(%client, %player); //points for kill
	//Beacon::onEnabled(%Turret);// you can map this like a beacon.
	deployableHucker::onEnabled(%Turret);// or a turret.
}


//===========================Fire the gun
//=======================================
// reworked feb 2002
function HuckerGunImage::onFire(%player,%slot)
{	
	%client = GameBase::getOwnerClient(%player);
	%AmmoCount = (Player::getItemCount(%player, $WeaponAmmo[HuckerGun]) && Player::getItemCount(%player,HuckerAmmo)) ;
	if(%AmmoCount){
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);
		if(GameBase::getLOSInfo(%player,250))
		{	%object = getObjectType($los::object);
			if (%object!="Player" && %object !="Flier" && %object !="") // == "SimTerrain" || %object == "InteriorShape" || %object=="StaticShape") 
{
	//Is target near a flag stand?
	%PlayerTeam = Gamebase::getTeam(%player);
	%numTeams = getNumTeams();
	for(%i = 0; %i < %numTeams; %i = %i + 1){ 
		%flagpos = $teamFlag[%i].originalPosition;
		%FlagDist = Vector::getDistance($los::position, %flagpos);
		if($debug) Client::sendMessage(Player::getClient(%player),1,"Target dist from enemy flag(s) "@%FlagDist);
		if (%FlagDist < 55 && %PlayerTeam != %i){
	
			Client::sendMessage(Player::getClient(%player),1,"No base turreting! ");
			echo(Client::getName(GameBase::getOwnerClient(%player))@" "@Player::getClient(%player)@" "@%player@" Trying to hucker near flag...");
			return;
			}
		}
//------------------------

				//--------------------
				// Try to stick it straight up or down, otherwise
				// just use the surface normal
				if (Vector::dot($los::normal,"0 0 1") > 0.6) 
				{	%rot = "0 0 0";}
				else 
				{	if (Vector::dot($los::normal,"0 0 -1") > 0.6) 
					{	%rot = "3.14159 0 0";}
					else 
					{	%rot = Vector::getRotation($los::normal);}
				}
				%team = GameBase::getTeam(%player);
				%item = "DeployableHucker";				
				if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item])
				{	%Dist=Vector::getDistance(GameBase::getPosition(%player),$los::position);
					if(%Dist==0){	%tTime=1;}
					else{	%tTime=(%Dist/200)+0.1;}
					//hack so the flier won't destroy the new turret, 
					//remove the +0.1 for some randomness

					playSound(SoundDryFire, GameBase::getPosition(%player));
					Projectile::spawnProjectile("HuckerFlier",%trans,%player,%vel,$los::object);
					playSound(SoundMissileReload, GameBase::getPosition(%player));
					$hucky++;
					$Location[$hucky]=$los::position;
					$Rotation=%rot;
					schedule("DeployAHucker(" @ %player @ " );",%tTime);
					$missionItems++;
					$TeamItemCount[GameBase::getTeam(%player) @ %item]++;
					Player::decItemCount(%player,HuckerAmmo);

					%it = "DeployableHucker";
					bottomprint(Player::getClient(%player), "<f2> Hucker Gun, set up those pesky little turrets like a PRO!!\n Team Max is "@$TeamItemMax[%it]@". You can set up "@($TeamItemMax[%it] - $TeamItemCount[GameBase::getTeam(%player) @ %it])@ " more. \n Total Huckers fired this mission ="@$hucky, 10);
					}
				else
				Client::sendMessage(%client,0,"No more Huckers for you!! ~Error_Message.wav");
			}
			else
			Client::sendMessage(%client,0,"You wanna put that Hucker WHERE?  ~wError_Message.wav");
		}
		else
		Client::sendMessage(%client,0,"Too far away! ~wError_Message.wav");
	}
	else
	{	if (!Player::getItemCount(%player, $WeaponAmmo[HuckerGun]))
	{	Client::sendMessage(%client,0,"You ran out of Huckers. ~waccess_denied.wav");}
	else
	{ 	Client::sendMessage(%client,0,"You ran out of Huckers.~waccess_denied.wav");}
	}

}


//----------------------------------------------------------

function DeployableHucker::onAdd(%this)
{
	schedule("DeployableHucker::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,5);
	%this.shieldStrength = 0;
	if (GameBase::getMapName(%this) == "") {
		GameBase::setMapName (%this, "Hucker");
	}
}

function DeployableHucker::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function DeployableHucker::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function DeployableHucker::onDestroyed(%this)
{
	//Hucker::onDestroyed(%this);
	%item = "DeployableHucker";$missionItems--;
  	$TeamItemCount[GameBase::getTeam(%this) @ %item]--;$missionItems--;
}

// Override base class just in case.
function DeployableHucker::onPower(%this,%power,%generator) {}
function DeployableHucker::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);
}
//--------------------------------------

//--------------------------------------
// Origional code by Plasmatic
// Please get ahold of me if you want to use this
// ziptiezmail@netscape.net
//--------------------------------------
//--------------------------------------
$WeaponAmmo[Disclauncher] = DiscAmmo;

function Disclauncher::onUse(%player,%item)
{
	bottomprint(Player::getClient(%player), "<f2> UFO Launcher, sends flying disks at your target.", 10);Weapon::onUse(%player,%item);
}


//--------------------------------------

ItemData DiscAmmo
{
	description = "UFO";
	className = "Ammo";
	shapeFile = "discb";//discammo
   heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 2;
};

ItemImageData DiscLauncherImage
{
	shapeFile = "disc";
	mountPoint = 0;

	weaponType = 0; // 0, single 1, spinning, 2 cont ,3disc
	reloadTime = 0.5;
	accuFire = false;
	fireTime = 0.25;

	ammoType =DiscAmmo;
//	projectileType = DiscShell; //messes with onimagefire
	accuFire = True;

	lightType = 3;  // Weapon Fire
	lightRadius = 4;
	lightTime = 3;
//	lightColor = { 0.6, 1, 1 };
   lightColor = { 0.25, 0.25, 0.85 };
	spinUpTime = 0.25;
	sfxFire = SoundFireDisc;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDiscReload;
	sfxReady = SoundDiscSpin;
};

ItemData DiscLauncher
{
	description = "UFO Launcher";
	className = "Weapon";
	shapeFile = "disc";
	hudIcon = "disk";
   heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = DiscLauncherImage;
	price = 150;
	showWeaponBar = true;
};

function DiscLauncherImage::onFire(%this) 
{
   %client = GameBase::getOwnerClient(%this);
   %AmmoCount = (Player::getItemCount(%client,DiscAmmo));
   %trans = GameBase::getMuzzleTransform(%this);
   %vel = Item::getVelocity(%this);
	if(%AmmoCount){
	  Player::decItemCount(%client,DiscAmmo);
	  for(%i=0; %i < 5; %i++) {

		%xrnd = (floor(getRandom() *21)-10)/100;
		%yrnd = (floor(getRandom() *21)-10)/100;
		%zrnd = (floor(getRandom() *21)-10)/100;

		%trans1= getWord(%trans,0);
		%trans2= getWord(%trans,1);
		%trans3= getWord(%trans,2);
		%trans4= getWord(%trans,3) + %xrnd;
		%trans5= getWord(%trans,4) + %yrnd;
		%trans6= getWord(%trans,5) + %zrnd;
		%trans7= getWord(%trans,6);
		%trans8= getWord(%trans,7);
		%trans9= getWord(%trans,8);
		%trans10=getWord(%trans,9);
		%trans11=getWord(%trans,10);
		%trans12=getWord(%trans,11);
//	echo(%trans4 @" "@ %trans5 @" "@ %trans6);	
		%NewTrans = %trans1 @" "@ %trans2 @" "@ %trans3 @" "@ %trans4 @" "@ %trans5 @" "@ %trans6 @" "@ %trans7 @" "@ %trans8 @" "@ %trans9 @" "@ %trans10 @" "@ %trans11 @" "@ %trans12;

		Projectile::spawnProjectile("DiscShell", %NewTrans, %this, %vel);

	     	}
	}
}




addWeapon(DiscLauncher);

addWeapon(EnergyRifle);
addWeapon(Blaster);
addWeapon(PlasmaGun);
addWeapon(ShotDiscGun);
addWeapon(SoulSucker);



addWeapon(Chaingun);


addWeapon(AgedonGun);
addWeapon(HoTosser);
addWeapon(needlelauncher);
addWeapon(HuckerGun);


//-------------------------------------------------------------
// Phrailter
//--------------------------------------------------

//-------------------------------------------------------------
// Phrailter
//--------------------------------------------------
// nov 8, 2001
// Origional code by Plasmatic
// Please get ahold of me if you want to use any part of this 
// ziptiezmail@netscape.net
//==================================
//-------------------------------------------------------------
// Phrailter
//--------------------------------------------------

$AutoUse[PlasmaticGun] = True;
$SellAmmo[PlasmaticGunAmmo] = 25;
$AmmoPackMax[PlasmaticGunAmmo] = 150;
$AmmoPackItems[0] = PlasmaticGunAmmo;

$WeaponAmmo[PlasmaticGun] = PlasmaticGunAmmo;

$InvList[PlasmaticGunAmmo] = 1;
$RemoteInvList[PlasmaticGunAmmo] = 1;
$InvList[PlasmaticGun] = 1;
$RemoteInvList[PlasmaticGun] = 1;

$ItemMax[marmor, PlasmaticGun] = 1;
$ItemMax[mfemale, PlasmaticGun] = 1;
$ItemMax[larmor, PlasmaticGun] = 1;
$ItemMax[lfemale, PlasmaticGun] = 1;
$ItemMax[harmor, PlasmaticGun] = 1;

addweapon(PlasmaticGun);

$ItemMax[marmor, PlasmaticGunAmmo] = 40;
$ItemMax[mfemale, PlasmaticGunAmmo] = 40;
$ItemMax[larmor, PlasmaticGunAmmo] = 30;
$ItemMax[lfemale, PlasmaticGunAmmo] = 30;
$ItemMax[harmor, PlasmaticGunAmmo] = 50;

function reload(%player) {
			%player.reloading = "";
		//echo("ready");
		playSound(MortarReload, GameBase::getPosition(%player));
		}


ItemImageData ScopeImage
{ 
shapeFile = "shotgun"; 
mountPoint = 0; 
mountOffset = { 0.02,-0.15,0.085 }; //{ -0.02,-0.15,0.085 }
	mountRotation = {0, -1.57, 0}; //{0, 1.57, 0}

weaponType = 0;  // 0, single 1, spinning 2, Sustained
projectileType = PhrailterLaser;	//ScopeLaser; sniperLaser PhrailterLaser  targetLaser
accuFire = true; 
	minEnergy = -60;	//10
	maxEnergy = -6;	//60
reloadTime = 0.1; 
fireTime = 0.5; 
lightType = 3;  // Weapon Fire 
lightRadius = 1; 
lightTime = 1; 
lightColor = { 1, 0, 0 };	//{ 0.6, 1, 1.0 }; 
}; 

ItemData Scope 
{ 
description = "Scope"; 
className = "Weapon"; 
shapeFile = "Paintgun"; 
//hudIcon = "Sniper"; 
heading = "bWeapons"; 
shadowDetailMask = 4; 
imageType = ScopeImage; 
price = 250; 
showWeaponBar = false; 
showInventory = false; 
}; 


function charger(%player)
	{
//	if (%player.lasercharging != "True") return;
	if (%player.reloading == "True") return;

	if (Player::isTriggered(%player,$WeaponSlot)){
		%player.Charge ++ ;	//0.1
if (%player.Charge > 90 ) %player.Charge = 90;

		%player.lasercharging = "True";
		schedule("charger(" @ %player @ ");",0.05); 
	%energy = GameBase::getEnergy(%player);
 	%energy -= 4;
	GameBase::setEnergy(%player,%energy);
%client = GameBase::getOwnerClient(%player);
centerprint(%client, "<jc><f2>"@%player.Charge@"                        "@%player.Charge, 0.5);
		}
	if (!Player::isTriggered(%player,$WeaponSlot)){
		%trans = GameBase::getMuzzleTransform(%player);
		//centerprint(%client, "bite me", 1);
		%vel = Item::getVelocity(%player);
		%smack = %player.Charge / 10;
	playSound(SoundFireLaser, GameBase::getPosition(%player));
	if (%smack < 2) playSound(SoundMissileTurretFire, GameBase::getPosition(%player));
	if (%smack >= 2) playSound(SoundPlasmaTurretFire, GameBase::getPosition(%player));
	if (%smack >= 3) playSound(explo3, GameBase::getPosition(%player));
	Player::trigger(%player, 6, true);
	//Projectile::spawnProjectile("PhrailterLaser", %trans, %player, %vel);

		for(%i=0; %i < %smack; %i += 1) {
			Projectile::spawnProjectile("sniperShell", %trans, %player, %vel);
			}
		%player.Charge = 0;
		Player::decItemCount(%player,$WeaponAmmo[PlasmaticGun],1); 
		%player.lasercharging = "";
		%player.reloading = "True";
		Player::trigger(%player, 6, false);
		schedule("reload(" @ %player @ ");",2.0); 
		WeaponKickBack(%player,%smack);
		}

}
function DeleteLaser(%newobj){
	deleteobject(%newObj);
}




ItemData PlasmaticGunAmmo
{
	description = "Phrailter charge";
	className = "Ammo";
	shapeFile = "ammo1";
   heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 10;
};

ItemImageData PlasmaticGunImage
{
	shapeFile = "sniper";
	mountPoint = 0;

	weaponType = 1; // 0, single 1, spinning, 2 cont.
	accuFire = false;

	reloadTime = 1.0;
	spinUpTime = 0.01;
	spinDownTime = 5;

	sfxSpinUp = SoundSpinUp;
	sfxSpinDown = SoundSpinDown;
   lightRange        = 2.0;
   lightColor        = { 1.0, 0.25, 0.25 };

	ammoType = PlasmaticGunAmmo;
//	projectileType = sniperShell;

	sfxActivate = SoundPickUpWeapon;
};

ItemData PlasmaticGun
{
	description = "Phrailter";
	className = "Weapon";
	shapeFile = "sniper";
   validateShape = true;
	hudIcon = "mortar";
   heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = PlasmaticGunImage;
	price = 125;
	showWeaponBar = true;
};


function PlasmaticGun::onUse(%player,%item)
{
	if(Player::getMountedItem(%player,$BackpackSlot) == EnergyPack){
		bottomprint(Player::getClient(%player), "<f2> Phrailter. Throws super charged particles, charge longer for more damage.", 20);
		%player.Charge = 0;
		%player.lasercharging = "";
		%player.reloading = "";
		Weapon::onUse(%player,%item);}
	else
		Client::sendMessage(Player::getClient(%player),0,
			"Must have an Energy Pack to use Phrailter."); 
}

function PlasmaticGunImage::onfire(%player) 
	{	
		if (%player.lasercharging) return;

		%player.Charge = 0;
		charger( %player );
	}

function PlasmaticGun::onMount(%player, %slot)
{
 	Player::mountItem(%player,Scope,6);
		bottomprint(Player::getClient(%player), "<f2> Phrailter. Throws super charged particles, charge longer for more damage.", 20);

}
function PlasmaticGun::onUnmount(%player, %slot)
{
Player::unmountItem(%player,6);
} 



//===============================

//-------------------------------------------------------------
// Miner
//--------------------------------------------------
// Oct 19, 2001
// Origional code by Plasmatic
// Please get ahold of me if you want to use any part of this 
// ziptiezmail@netscape.net
//-------------------------------------------------------------
// Miner
//--------------------------------------------------

$InvList[Miner] = 1;
$RemoteInvList[Miner] = 1;

$ItemMax[marmor, Miner] = 1;
$ItemMax[mfemale, Miner] = 1;
$ItemMax[larmor, Miner] = 0;
$ItemMax[lfemale, Miner] = 0;
$ItemMax[harmor, Miner] = 1;

$WeaponAmmo[Miner] = "MinerAmmo";

$ItemMax[marmor, MinerAmmo] = 20;
$ItemMax[mfemale, MinerAmmo] = 20;
$ItemMax[larmor, MinerAmmo] = 0;
$ItemMax[lfemale, MinerAmmo] = 0;
$ItemMax[harmor, MinerAmmo] = 30;


$InvList[MinerAmmo] = 1;
$RemoteInvList[MinerAmmo] = 1;

$SellAmmo[MinerAmmo]			= 10;

$AmmoPackMax[MinerAmmo] = 30;
$AmmoPackItems[15] = MinerAmmo;


ItemData MinerAmmo
{
	description = "Mine Shells";
	className = "Ammo";
heading = "xAmmunition";
	shapeFile = "grenammo";
	shadowDetailMask = 4;
	price = 10;
};


ItemImageData MinerImage
{
	shapeFile = "grenadeL";
	mountPoint = 0;
	weaponType = 0;
	ammoType = MinerAmmo;
	mountPoint = 0;
	mountRotation = { 0, 0.785, 0 };
	mountOffset = {-0.065, 0, 0.07}; 
	//projectileType = MinerShell;// interferes with image fire
	accuFire = false;
	reloadTime = 0.75;
	fireTime = 1.5;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };
	sfxFire = SoundFireGrenade;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;

};

ItemData Miner
{
	description = "Magic Miner";
	className = "Weapon";
	shapeFile = "grenadeL";
	hudIcon = "grenade";
heading = "bWeapons";
	shadowDetailMask = 4;
        imageType = MinerImage;
	price = 7500;
	showWeaponBar = true;
};


function MinerImage::onFire(%this) 
{
	%client = GameBase::getOwnerClient(%this);
	%AmmoCount = (Player::getItemCount(%client,MinerAmmo));
  	%trans = GameBase::getMuzzleTransform(%this);
   	%vel = Item::getVelocity(%this);
	if(%AmmoCount){
	Player::decItemCount(%client,MinerAmmo);

 		%newObj = Projectile::spawnProjectile("MinerShell", %trans, %this, %vel);
		schedule("SplitMines(" @ %newObj @ ", " @ %this @ ");", 2.8);
		schedule("TossMines(" @ %newObj @ ", " @ %this @ ");", 3);

	}
}

function SplitMines(%newobj,%this){

		%Pos = GameBase::getPosition(%newobj); 
   		%vel = Item::getVelocity(%newobj);
// Pretty shell split

	%trans =  "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%newObj);
 	%obj = Projectile::spawnProjectile("PrettySplit", %trans, %this, %vel);
	Projectile::spawnProjectile(%obj);
	GameBase::setPosition(%obj, %pos);
	Item::setVelocity(%obj, %vel);
}

function TossMines(%newobj,%this){

		%Pos = GameBase::getPosition(%newobj); 
   		%vel = Item::getVelocity(%newobj);
		%xvel = getWord(%vel,0);
		%yvel = getWord(%vel,1);
		%zvel = getWord(%vel,2);

// Spawn butterflies

if (GameBase::getPosition(%newObj)){
	for(%i=0; %i < 15; %i += 1) {

		%xrnd = %xvel + floor(getRandom() * 60) -30;
		%yrnd = %yvel + floor(getRandom() * 60) -30;
		%zrnd = %zvel + floor(getRandom() * 20);

	%forceVel = %xrnd@" "@%yrnd@" "@%zrnd;

	%trans =  "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%newObj);
 		%obj = Projectile::spawnProjectile("MineFloaters", %trans, %this, %vel);
		Projectile::spawnProjectile(%obj);
		GameBase::setPosition(%obj, %pos);
		Item::setVelocity(%obj, %forceVel);
	%rnd = (floor(getRandom()*20)/80);
		schedule("SetMines(" @ %obj @ ", " @ %this @ ");", 2 - %rnd);
		}
	deleteobject(%newObj);
	}
}
function SetMines(%newobj,%this){

	if (GameBase::getPosition(%newObj)){
		%Pos = GameBase::getPosition(%newobj); 
   		%vel = Item::getVelocity(%newobj);
		%Mine = newObject("","Mine","MinerMine");
		GameBase::setTeam (%Mine,GameBase::getTeam (%this));
 		addToSet("MissionCleanup", %Mine);
      	GameBase::throw(%Mine,%this,-1,true);
		GameBase::setPosition(%Mine, %pos);
		Item::setVelocity(%Mine, %vel);
	deleteobject(%newObj);
	}
}
function Miner::onUse(%player,%item)
{
	Weapon::onUse(%player,%item);
	bottomprint(Player::getClient(%player), "<f2> Miner, lower property values in your neighborhood.", 10);
}

addWeapon(Miner);



//--------------------------------------
// Origional code by Plasmatic
// Please get ahold of me if you want to use this
// ziptiezmail@netscape.net
//--------------------------------------

$AutoUse[SoulSucker] = True;
$WeaponAmmo[SoulSucker] = "";

//--------------------------------------



//--------------------------------------


SoundData SoundCrash
{
   wavFileName = "crash.wav";
   profile = Profile3dFar;
};

SoundData SoundNope
{
   wavFileName = "failpack.wav";
   profile = Profile3dMedium;
};

function StopSuck(%shooterId){
	%shooterId.lock = false;//echo("falsied");
	Player::trigger(%shooterId, $WeaponSlot, false);
	}

function LaserSearch::damageTarget(%target, %timeSlice, %damPerSec, %enDrainPerSec, %pos, %vec, %mom, %shooterId)
{
%player = Client::getOwnedObject(%shooterId);
%energy = GameBase::getEnergy(%player);
%object = getObjectType(%target); 
//GameBase::setEnergy(%player,(%energy-10));
if (%energy < 20 && (!$build ||%object == "Player" )) {
		Player::trigger(%shooterId, $WeaponSlot, false);
		//GameBase::playSound( %shooterId,SoundNope,0);
		return;}
//Player::trigger(%shooterId, $WeaponSlot, true);
//%shooterId.lock = true;
GameBase::playSound(%shooterId,SoundCrash,0);
if ($build && %object != "Player" ) GameBase::setEnergy(%player,(%energy+15));
if (!$build) schedule("Player::trigger(%shooterId, $WeaponSlot, false);", 2);
if (!$build) schedule("StopSuck(%shooterId);", 2);

if(%object == "Player" || %object == "Flier") { 
laserWarning(%shooterId, %target, %object);}

if (!$build ||%object == "Player" )  GameBase::setEnergy(%player,(%energy-50));
if ($build && %object != "Player" )GameBase::applyDamage(%target, $ElectricityDamageType, 50, %pos, %vec, %mom, %shooterId);

   GameBase::applyDamage(%target, $ElectricityDamageType, 2, %pos, %vec, %mom, %shooterId);


}

LightningData LaserSearch
 {
	bitmapName       = "lightningNew.bmp"; //lightningNew.bmp
   	boltLength       = 50.0;
   	coneAngle        = 15.0;
   	damagePerSec = 10;
   	energyDrainPerSec = 250;
   	segmentDivisions = 0;
   	numSegments = 1;
   	beamWidth = 0.25;
   	updateTime   = 30;
   	skipPercent  = 0.5;
   	displaceBias = 0.15;
   	lightRange = 3.0;
   	lightColor = { 0.5, 0.5, 0.5 };
   	isVisible = false;//false


};


//--------------------------------------

ItemImageData SoulSuckerImage
{
	shapeFile = "paintgun";//shotgun
   mountPoint = 0;

   weaponType = 2;  // 2continuous 0singleshot 1spinning
	projectileType = LaserSearch;//lightningcharge
   minEnergy = 50;
   maxEnergy = 150;  // Energy used/sec for sustained weapons 11
	reloadTime = 1;
                        
   lightType = 3;  // Weapon Fire
   lightRadius = 2;
   lightTime = 1;
   lightColor = { 0.25, 0.25, 0.85 };

   sfxActivate = SoundPickUpWeapon;
   sfxFire     = SoundNope;
};




ItemData SoulSucker
{
	description = "Soul Sucker";
	className = "Weapon";
	shapeFile = "paintgun";//sniper
	hudIcon = "sniper";
   heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = SoulSuckerImage;
	price = 200;
	showWeaponBar = true;
   validateShape = true;
   validateMaterials = false;
};

ItemImageData Laser1Image { shapeFile = "paintgun"; mountPoint = 0; mountOffset = {0.065, 0.05, 0}; mountRotation = {0, -1.57, 0}; weaponType = 0; reloadTime = 0.45; accuFire = true; fireTime = 0.45; }; 

ItemData Laser1 { heading = "bProjectile Weapons"; description = "Shotgun"; className = "Weapon"; shapeFile = "shotgun"; hudIcon = "ammopack"; shadowDetailMask = 4; imageType = Laser1Image; price = 125; showWeaponBar = false; showInventory = false; }; 

function SoulSucker::onMount(%player, %slot) {	
	Player::mountItem(%player, Laser1, 6); 
}
function SoulSucker::onUnmount(%player, %slot) { 
	Player::unmountItem(%player, 6); 
}



function SoulSucker::onUse(%player,%item)
{ 	
	if(Player::getMountedItem(%player,$BackpackSlot) == EnergyPack || $build){
		Weapon::onUse(%player,%item);bottomprint(Player::getClient(%player), "<f2> Soul Sucker, auto aim weapon.", 10);GameBase::playSound( %player,ForceFieldOpen,0);}
}

function laserWarning(%clientId, %target, %targetType) { 
	%targetId = GameBase::getControlClient(%target);
	%targetName = Client::getName(%targetId); 
	%name = Client::getName(%clientId); 
	if(%targetType == Flier) {
		if(%targetName != "") 
			%msg = "Vehicle Piloted by " @ %targetName; 
		else 
			%msg = "Vehicle"; 
	} else 
		%msg = %targetName;
		//echo(%msg); 
	Client::sendMessage(%clientId,0,"Soul Lock Aquired: " @ %msg @ "~wmine_act.wav");
	if(%targetType == Flier || (Player::getArmor(%target)) != "") {
		Client::sendMessage(%targetId,0, %name @ " Hit you with a soul sucker!~waccess_denied.wav");
		schedule("Client::sendMessage(" @ %targetId @ ",0,\"~waccess_denied.wav\");", 0.5);
		schedule("Client::sendMessage(" @ %targetId @ ",0,\"~waccess_denied.wav\");", 1.0); 


	}
} 






///-----------------------------------------------
//
// Code by Plasmatic
// Please get ahold of me if you want to use this
// ziptiezmail@netscape.net
//
///-----------------------------------------------

$SellAmmo[RockAmmo] = 15;
$AmmoPackMax[RockAmmo] = 15;
$AmmoPackItems[7] = RockAmmo;
$WeaponAmmo[RockLauncher] = RockAmmo;

//--------------------------------------



//--------------------------------------
function Rocklauncher::onUse(%player,%item)
{
	bottomprint(Player::getClient(%player), "<f2> Rock Launcher, sends fiery meteors at your foe, does little damage.", 10);Weapon::onUse(%player,%item);
}



//--------------------------------------


ItemData RockAmmo
{
	description = "Rock";
	className = "Ammo";
	shapeFile = "mortar";
   heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 2;
};

ItemImageData RockLauncherImage
{
	shapeFile = "plasma";//disc plasma
	mountPoint = 0;

	weaponType = 3; // DiscLauncher
	ammoType = RockAmmo;
	projectileType = RockShell;
	accuFire = true;
	reloadTime = 0.1;
	fireTime = 0;
	spinUpTime = 0;

	sfxFire = SoundFirePlasma;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDiscReload;
	sfxReady = SoundMortarTurretTurn;
};

ItemData RockLauncher
{
	description = "Rock Launcher";
	className = "Weapon";
	shapeFile = "plasma";//
	hudIcon = "mortar";
   heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = RockLauncherImage;//DiscLauncherImage
	price = 220;
	showWeaponBar = true;
};

addWeapon(RockLauncher);


//--------------------------------------

$AutoUse[TargetingLaser] = False;

//--------------------------------------



//--------------------------------------

function TargetingLaser::onUse(%player,%item)
{
	bottomprint(Player::getClient(%player), "<f2> Targeting Laser, Light up your life.", 10);Weapon::onUse(%player,%item);

}


additem(TargetingLaser);
//--------------------------------------

ItemImageData TargetingLaserImage
{
	shapeFile = "paintgun";
	mountPoint = 0;

	weaponType = 2; // Sustained
	projectileType = targetLaser;
	accuFire = true;
	minEnergy = 5;
	maxEnergy = 6;//15
	reloadTime = 1.0;

	lightType   = 3;  // Weapon Fire
	lightRadius = 1;
	lightTime   = 1;
	lightColor  = { 0.25, 1, 0.25 };

	sfxFire     = SoundFireTargetingLaser;
	sfxActivate = SoundPickUpWeapon;
};

ItemData TargetingLaser
{
	description   = "Targeting Laser";
	className     = "Tool";
	shapeFile     = "paintgun";
	hudIcon       = "targetlaser";
   heading = "bWeapons";
	shadowDetailMask = 4;
	imageType     = TargetingLaserImage;
	price         = 50;
	showWeaponBar = false;
};

//--------------------------------------

$AutoUse[EnergyRifle] = True;
$WeaponAmmo[EnergyRifle] = "";

//--------------------------------------



function EnergyRifle::onUse(%player,%item)
{
	bottomprint(Player::getClient(%player), "<f2> Orgasm gun, excite your enemy into giving up all their goodies, fix team equipment.", 10);Weapon::onUse(%player,%item);
}


//--------------------------------------


//--------------------------------------

ItemImageData EnergyRifleImage
{
	shapeFile = "shotgun";
   mountPoint = 0;

   weaponType = 2;  // Sustained
	projectileType = lightningCharge;
   minEnergy = 3;
   maxEnergy = 15;  // Energy used/sec for sustained weapons 11
	reloadTime = 0.2;
                        
   lightType = 3;  // Weapon Fire
   lightRadius = 2;
   lightTime = 1;
   lightColor = { 0.25, 0.25, 0.85 };

   sfxActivate = SoundPickUpWeapon;
   sfxFire     = SoundELFIdle;
};

ItemData EnergyRifle
{
   description = "Orgasm Gun";
	shapeFile = "shotgun";
	hudIcon = "energyRifle";
   className = "Weapon";
   heading = "bWeapons";
   shadowDetailMask = 4;
   imageType = EnergyRifleImage;
	showWeaponBar = true;
   price = 125;
   validateShape = true;
};


$TractorPower = 30;

function lightningCharge::damageTarget(%target, %timeSlice, %damPerSec, %enDrainPerSec, %pos, %vec, %mom, %shooterId) 
{
  %obj1 = %target;
  %obj2 = %shooterId;
   // Get object's mass  
  if (getObjectType(%target) == "Player" &&  (GameBase::getTeam(%target) != Client::getTeam(%shooterId) || $Server::TeamDamageScale))
	{
//begin drain energy, drop weapon code by plasmatic

   %damVal = %timeSlice * %damPerSec;
   %enVal  = %timeSlice * %enDrainPerSec;
   %energy = GameBase::getEnergy(%target);
   %energy = %energy - %enVal;
   if (%energy < 0) {
%c = Player::getClient(%target);
//echo(%target);
%item = Player::getMountedItem(%c,$WeaponSlot);
if(%item.className == Weapon) {
		%state = Player::getItemState(%c,$WeaponSlot);
		%it = Player::getMountedItem(%c,$WeaponSlot);
	if(%state == "Fire" && (%it != "Blaster" && %it != "EnergyRifle")) {
		%item = %item.imageType.ammoType;
		Player::dropItem(%c,%item);
		}
	if(%it != "Blaster" && %it != "EnergyRifle" && %it != "") {
		Player::dropItem(%c,%item);
		}
	if(%it == "Blaster" || %it == "EnergyRifle") {
		%fl = Player::getMountedItem(%c,$FlagSlot);
	if(%fl != -1 && %state != "Fire")
      	Player::dropItem(%c, %item);
	if(%fl == -1 && %state != "Fire")
      	Player::dropItem(%c, %item);
		//drop flag below
	if(%fl != -1 && %state == "Fire")
      	Player::dropItem(%c, %fl);}
		%it = Player::getMountedItem(%c,$WeaponSlot);// spam check
	if(%it == -1){
	 	%mf =Client::getGender(%c);
		if (%mf == "Female") {
		 messageAll(0,Client::getName(%c)@" fumbles her " @ %item);}
		else messageAll(0,Client::getName(%c)@" fumbles his " @ %item);
		}
	}
if(%item == "RepairGun")
		{
	 	Player::dropItem(%c,Player::getMountedItem(%c,$BackpackSlot));
		messageAll(0,Client::getName(%c)@" droped a Fixit Gun");
		}
%energy = 0;
   }
   GameBase::setEnergy(%target, %energy);
 %obj1mass = Player::getArmor(%obj1).mass;
}
//end drain energy,drop weapon
if (GameBase::getTeam(%target) == Client::getTeam(%shooterId))
{ 
//we gotta team object here grasshopper!
//get busy fixing it, w00t!
	%player = %shooterId;
	%client = Player::getClient(%player);

	if (%target == %player) %player.repairTarget = -1;	
	else {
      %player.repairTarget = %target;
		//%player.repairRate   = 0.1;
		if (getObjectType(%player.repairTarget) == "Player") {
			%rclient = Player::getClient(%player.repairTarget);
			%name = Client::getName(%rclient);
		}
		else { 
			%name = GameBase::getMapName(%target);
			if(%name == "") {
				%name = (GameBase::getDataName(%player.repairTarget)).description;
			}
		}
		if (GameBase::getDamageLevel(%player.repairTarget) == 0) {
			//Client::sendMessage(%client,0,%name @ " is not damaged");
			//Player::trigger(%player,$WeaponSlot,false);
			//%player.repairTarget = -1;
		}
		if (getObjectType(%player.repairTarget) == "Player") {
			//Client::sendMessage(%rclient,0,"Being repaired by " @ Client::getName(%client));
			%dlevel = GameBase::getDamageLevel(%player.repairTarget) - 0.08;
			GameBase::setDamageLevel(%player.repairTarget,%dlevel);
			//return;
		}

		//Client::sendMessage(%client,0,"Repairing " @ %name);
	}
	%dlevel = GameBase::getDamageLevel(%player.repairTarget) - 0.02;
	GameBase::setDamageLevel(%player.repairTarget,%dlevel);
	
}
// end team object repair
// begin grapple code

  if (getObjectType(%target) != "Player") %obj1mass = 3; 

%obj2mass = Player::getArmor(%obj2).mass;

   // 
  %vec = Vector::Normalize(Vector::Sub(GameBase::getPosition(%obj1), GameBase::getPosition(%obj2)));

  if (getObjectType(%obj1) == "Player")
  {
    %mul = $TractorPower - ($TractorPower * %obj1mass) / (%obj1mass + %obj2mass);
    %nvec = (getWord(%vec, 0) * %mul * -1) @ " " @
            (getWord(%vec, 1) * %mul * -1) @ " " @
            (getWord(%vec, 2) * %mul * -1);
    //Player::applyImpulse(%obj1, %nvec);
    Item::setVelocity(%obj1, %nvec);
  }

   // obj2 is always a player
  %mul = $TractorPower - ($TractorPower * %obj2mass) / (%obj1mass + %obj2mass);
  %nvec = (getWord(%vec, 0) * %mul) @ " " @
          (getWord(%vec, 1) * %mul) @ " " @
          (getWord(%vec, 2) * %mul);
  //Player::applyImpulse(%obj2, %nvec);
  Item::setVelocity(%obj2, %nvec);
}




//--------------------------------------

$TeamItemMax[DeployableInvPack] = 50;
additem(DeployableInvPack);
//--------------------------------------

ItemImageData DeployableInvPackImage
{
	shapeFile = "invent_remote";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.3 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData DeployableInvPack
{
	description = "Inventory Station";
	shapeFile = "invent_remote";
	className = "Backpack";
   heading = "dDeployables";
	shadowDetailMask = 4	;
	imageType = DeployableInvPackImage;
	mass = 2.0;
	elasticity = 0.2;
	price = $RemoteInvEnergy + 200;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};


function DeployableInvPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function DeployableInvPack::onDeploy(%player,%item,%pos)
{	
	if (DeployableInvPack::deployShape(%player,%item)&& !$build) {
		Player::decItemCount(%player,%item);
	}
}	
function DeployableInvStation::onDestroyed(%this) 
{ 
	$TeamItemCount[GameBase::getTeam(%this) @ "DeployableInvPack"]--; $missionItems--;
} 

function DeployableInvPack::deployShape(%player,%item)
{
	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]|| $build) {
		if (GameBase::getLOSInfo(%player,3)) {
			%obj = getObjectType($los::object);
			if (%obj == "SimTerrain" || %obj == "InteriorShape" || %obj == "StaticShape") {
				if (Vector::dot($los::normal,"0 0 1") > 0.7) {
					if(checkDeployArea(%client,$los::position) || %obj == "StaticShape" ) {
						%inv = newObject("ammounit_remote","StaticShape","DeployableInvStation",true);
 	 		         addToSet("MissionCleanup", %inv);
						%rot = GameBase::getRotation(%player); 
						GameBase::setTeam(%inv,GameBase::getTeam(%player));
						GameBase::setPosition(%inv,$los::position);
						GameBase::setRotation(%inv,%rot);
						Gamebase::setMapName(%inv,%name);
						if(!$build)Client::sendMessage(%client,0,"Inventory Station deployed");
						playSound(SoundPickupBackpack,$los::position);
						$missionItems++;
						$TeamItemCount[GameBase::getTeam(%inv) @ "DeployableInvPack"]++;
						$Dinv = $Dinv +1;
			echo("MSG: ",Client::getName(%client)," CL# ",%client," deployed a Inventory station");
						return true;
					}
				}
				else {
					if ($build) Client::sendMessage(%client,0,"~waccess_denied.wav");
					else Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
				}
			}
			else {
			if ($build) Client::sendMessage(%client,0,"~waccess_denied.wav");
			else Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");
			}
		}
		else {
			GameBase::playSound(%player, usepack, 0);
			if (!$build) Client::sendMessage(%client,0,"Deploy position out of range");
		}
	}
	else																						  
	 	Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
	return false;
}

//--------------------------------------

//===============================

///-----------------------------------------------

//Origional code by Plasmatic
//Please get ahold of me if you want to use this
//ziptiezmail@netscape.net

///-----------------------------------------------
///-----------------------------------------------


$ItemMax[harmor, InvTree] = 1;
$ItemMax[larmor, InvTree] = 1;
$ItemMax[lfemale, InvTree] = 1;
$ItemMax[marmor, InvTree] = 1;
$ItemMax[mfemale, InvTree] = 1;
$TeamItemMax[InvTree] = 1000;
additem(InvTree);
$InvList[InvTree] = 1;
$RemoteInvList[InvTree] = 0;

///-----------------------------------------------


ItemImageData InvTreeImage
{
        shapeFile = "plant2";//Tree1";//newdoor5
        mountPoint = 2;
        //mountOffset = { -0.375, 0, 0 };
        //mountRotation = { 0, 1.57, 0 };
        mass = 2.5;
        firstPerson = false;
};

ItemData InvTree
{
        description = "Inventory bush";
        shapeFile = "plant2";//Tree1";//newdoor5
        className = "Backpack";
        heading = "dDeployables";//        heading = "dDeployables";
        imageType = InvTreeImage;
        shadowDetailMask = 4;
        mass = 2.5;
        elasticity = 0.2;
        price = 5;
        hudIcon = "deployable";
        showWeaponBar = true;
        hiliteOnActive = true;
        dismountSound = SoundFireGrenade;
};


TurretData InvTreeShape { 
	className = "Turret"; 
	shapeFile = "plant2";	
	//projectileType = BushSmoke;	//PrettySplit;	//PencilTurretBullet; 
	maxDamage = 20.5; 
	maxEnergy = 60; 
	minGunEnergy = 1; 
	maxGunEnergy = 1; 
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor }; 
	reloadDelay = 0.9; 
	//speed = 4.0; 
	//speedModifier = 1.5; 
	range = 70; 
	visibleToSensor = true; shadowDetailMask = 4; dopplerVelocity = 0; 
	castLOS = true; supression = false; mapFilter = 2; 
	mapIcon = "M_station"; 
	debrisId = flashDebrisMedium; shieldShapeName = "shield"; 
	//fireSound = ricochet3; 	
	activationSound = SoundRemoteTurretOn; 
	deactivateSound = SoundRemoteTurretOff; 
	explosionId = flashExpMedium; 
	description = "Inventory bush"; 
	damageSkinData = "objectDamageSkins"; 
	
	};
	
	


function InvTree::onMount(%player,%item){
		%client = Player::getClient(%player);

		Client::sendMessage(%client,0,"got bush?");
		
	}



function InvTree::onUse(%player,%item)
{
        if (Player::getMountedItem(%player,$BackpackSlot) != %item)
        {
                Player::mountItem(%player,%item,$BackpackSlot);
        }
        else
        {
                Player::deployItem(%player,%item);
        }
}

function InvTreeShape::onAdd(%this) { 
	
	GameBase::setActive(%this,true);
	GameBase::setRechargeRate(%this,5); 
	%this.shieldStrength = 0; 
		if (GameBase::getMapName(%this) == "") { 
		GameBase::setMapName (%this, "Inventory bush");
		 }
	} 
		
		
function InvTree::onDeploy(%player,%item,%pos)
{
        if (InvTree::deployShape(%player,%item) && !$build)
        {
	Player::decItemCount(%player,%item);
        }
}

function InvTree::deployShape(%player,%item)
{
      %client = Player::getClient(%player);
	%AmmoCount = ( Player::getItemCount(%player,Seed)+1) ;
	if(%AmmoCount){
        if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]|| $build) {
                if (GameBase::getLOSInfo(%player,3)) {

                        %obj = getObjectType($los::object);
                        if (%obj != "SimTerrain"){
                        	Client::sendMessage(%client,0,"Inventory Bushes only deploy on flat ground");
				return;
                        	}
				%prot = GameBase::getRotation(%player);
				%zRot = getWord(%prot,2);
				if (Vector::dot($los::normal,"0 0 1") > 0.7) {
					%rot = "0 0 " @ %zRot;
				}
				else {
					Client::sendMessage(%client,0,"Inventory Bushes only deploy on flat ground");
					return;

	
				}

			%camera = newObject("Camera","Turret",InvTreeShape,true);

	   	      addToSet("MissionCleanup", %camera);
					GameBase::setTeam(%camera,GameBase::getTeam(%player));
					GameBase::setRotation(%camera,%rot);
					GameBase::setPosition(%camera,$los::position);
					GameBase::startFadeIn(%camera);
                              Gamebase::setMapName(%camera,"Inventory bush#"@ $totalNumInvTree++ @ " " @ Client::getName(%client));
					if(!$build)Client::sendMessage(%client,0,"Inventory bush set");
					playSound(SoundPickupBackpack,$los::position);
                                        $TeamItemCount[GameBase::getTeam(%camera) @ "InvTree"]++;$InvTree++;
			echo("MSG: ",Client::getName(%client)," CL# ",%client," deployed a Inventory bush");
					return true;

                        }
                       else {
			GameBase::playSound(%player, usepack, 0);
			if (!$build) Client::sendMessage(%client,0,"Cannot deploy here.");
                        }

        }
        else
                 Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");

        return false;
}}




function InvTreeShape::onDestroyed(%this)
{
   StaticShape::onDestroyed(%this);
   $TeamItemCount[GameBase::getTeam(%this) @ "InvTree"]--;
}
function ResetTree(%this){
	%player = Client::getOwnedObject(%this);
	$treed[%player] = "";
	//echo("reset "@%player);
	}


function InvTreeShape::onCollision(%this,%obj){
	//echo("collision "@%this,%obj,%obj.treed);
	if(getObjectType(%obj)!="Player" || Player::isDead(%obj) || $treed[%obj]) {
		return;
	}
	%c = Player::getClient(%obj);
	echo("tree id "@%this);
	%playerTeam = GameBase::getTeam(%obj);
	%InvTreeTeam = GameBase::getTeam(%this);
		if(%InvTreeTeam != %playerTeam){
		   %energy = GameBase::getEnergy(%obj);
 		   %energy = %energy - 30;
		   if (%energy < 0)  %energy = 0;	GameBase::setEnergy(%obj, %energy);
		   //client::sendmessage(%c, 0, "~wmale1.wwatchsh.wav"); 
		   client::sendmessage(%c, 1, "Access Denied ~wAccess_Denied.wav");
		   Player::setDamageFlash(%obj,0.5);
		   %jumpDir = Vector::getFromRot(GameBase::getRotation(%obj),-100,20);
		   Player::applyImpulse(%obj,%jumpDir);
		   return;
		   }
	%c = Player::getClient(%obj);
	%playerpos = GameBase::GetPosition(%obj);
	%Treepos = GameBase::GetPosition(%this);
	if(getWord(%playerpos,2) - getWord(%Treepos,2) >1 ){
//  		echo(getWord(%playerpos,2) - getWord(%Treepos,2));
		%cl = GameBase::getControlClient(%this);
		if(%cl == -1) 
		{
			GameBase::setPosition(%c, Vector::add(%Treepos,"0 0 -0.75"));
			$treed[%obj] = true;
			%c.InvAcc = true;
//			%c.spawnGrace = true;
			Client::setOwnedObject(%c, %this);
			Client::setOwnedObject(%c, %obj);		
			Client::setControlObject(%c,%this);		
		
			schedule("quickinv("@%c@");",0.1);
			remoteInventoryMode(%c);
			%c.guiLock = true;	
			//client::sendmessage(%c, 0, "~wmale1.wwatchsh.wav"); 
		return;
		}
	}
}

function InvTreeUpdate(%cl){
	%tr = Client::getControlObject(%cl);
	%pl = Client::getOwnedObject(%cl);
	echo(%tr,%pl);
//	Player::setMountObject(%pl, -1,0);
	Client::setControlObject(%cl, %pl);
	schedule("Client::setControlObject("@%cl@" ,"@%tr@");",0.2);
	}


function Turret::Jump(%this, %mom) 
{
//	for inv bush only, does NOT work if ither turrets are controlable in mod.
	%cl = GameBase::getControlClient(%this);
	%treepos = GameBase::GetPosition(%this);
	if(%cl != -1) 
	{
		%pl = Client::getOwnedObject(%cl);

		Player::setMountObject(%pl, -1,0);
		Client::setControlObject(%cl, %pl);
		%playerpos = GameBase::GetPosition(%pl);
		echo(getWord(%playerpos,2)@" "@getWord(%Treepos,2));
		if(getWord(%playerpos,2) - getWord(%Treepos,2) <0 && getWord(%playerpos,2) != getWord(%Treepos,2))
		{
				

		
		%player = Client::getOwnedObject(%cl);
		%cl.InvAcc = false;
		%cl.guiLock = "";
		remotePlayMode(%cl);
		schedule("resetTree("@%cl@");",3);
		Client::clearItemShopping(%cl);
		if(Player::getMountedItem(%pl,$WeaponSlot) == -1){
			if(%pl.lastWeapon != "") {
				//echo(%player.lastWeapon);
				Player::useItem(%pl,%pl.lastWeapon);		 	
				%pl.lastWeapon = "";
	  		}
		}


		%mass = GameBase::getDataName(%pl).mass;		
		GameBase::setPosition(%cl, Vector::add(%playerpos,"0 0 1"));
		%jumpDir = Vector::getFromRot(GameBase::getRotation(%player),-10,%mass * 20);
		Player::applyImpulse(%player,%jumpDir);
		playSound(SoundFireGrenade, GameBase::getPosition(%cl));
		}
	}	
}


//--------------------------------------

$TeamItemMax[CameraPack] = 15;
additem(CameraPack);
//--------------------------------------

ItemImageData CameraPackImage
{
	shapeFile = "camera";
	mountPoint = 2;
	mountOffset = { 0, -0.1, -0.06 };
	mountRotation = { 0, 0, 0 };
	firstPerson = false;
};

ItemData CameraPack
{
	description = "Camera";
	shapeFile = "camera";
	className = "Backpack";
   heading = "dDeployables";
	imageType = CameraPackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 100;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
   validateShape = true;
   validateMaterials = false;
};

function CameraPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function CameraPack::onDeploy(%player,%item,%pos)
{	
	if (CameraPack::deployShape(%player,%item)&& !$build) {
		Player::decItemCount(%player,%item);
	}
}

function CameraPack::deployShape(%player,%item)
{
 	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]|| $build) {
		if (GameBase::getLOSInfo(%player,3)) {
			// GetLOSInfo sets the following globals:
			// 	los::position
			// 	los::normal
			// 	los::object
			%obj = getObjectType($los::object);
			if (%obj == "SimTerrain" || %obj == "InteriorShape") {
				// Try to stick it straight up or down, otherwise
				// just use the surface normal
				%prot = GameBase::getRotation(%player);
				%zRot = getWord(%prot,2);
				if (Vector::dot($los::normal,"0 0 1") > 0.6) {
					%rot = "0 0 " @ %zRot;
				}
				else {
					if (Vector::dot($los::normal,"0 0 -1") > 0.6) {
						%rot = "3.14159 0 " @ %zRot;
					}
					else {
						%rot = Vector::getRotation($los::normal);
					}
				}
				if(checkDeployArea(%client,$los::position)) {
					%camera = newObject("Camera","Turret",CameraTurret,true);
	   	      addToSet("MissionCleanup", %camera);
					GameBase::setTeam(%camera,GameBase::getTeam(%player));
					GameBase::setRotation(%camera,%rot);
					GameBase::setPosition(%camera,$los::position);
					Gamebase::setMapName(%camera,"Camera#"@ $totalNumCameras++ @ " " @ Client::getName(%client));
					if(!$build)Client::sendMessage(%client,0,"Camera deployed");
					playSound(SoundPickupBackpack,$los::position);
					$missionItems++;$TeamItemCount[GameBase::getTeam(%camera) @ "CameraPack"]++;
					$Camera = $Camera +1;
					echo("MSG: ",Client::getName(%client)," CL# ",%client," deployed a Camera");
					return true;
				}
			}
			else {
				if ($build) Client::sendMessage(%client,0,"~waccess_denied.wav");
			else Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");
			}
		}
		else {
			GameBase::playSound(%player, usepack, 0);
			if (!$build) Client::sendMessage(%client,0,"Deploy position out of range");		
		}
	}
	else																						  
	 	Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
	
	return false;
}

//--------------------------------------

TurretData CameraTurret
{
	className = "Turret";
	shapeFile = "camera";
	maxDamage = 0.25;
	maxEnergy = 10;
	speed = 20;
	speedModifier = 1.0;
	range = 50;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	visibleToSensor = true;
	shadowDetailMask = 4;
	castLOS = true;
	supression = false;
	supressable = false;
	mapFilter = 2;
	mapIcon = "M_camera";
	debrisId = defaultDebrisSmall;
	FOV = 0.707;
	pinger = false;
	explosionId = debrisExpMedium;
	description = "Camera";
};

function CameraTurret::onAdd(%this)
{
	schedule("CameraTurret::deploy(" @ %this @ ");",1,%this);
	if (GameBase::getMapName(%this) == "") {
		GameBase::setMapName (%this, "Camera");
	}
}

function CameraTurret::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function CameraTurret::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function CameraTurret::onDestroyed(%this)
{
	Turret::onDestroyed(%this);
  	$TeamItemCount[GameBase::getTeam(%this) @ "CameraPack"]--;$missionItems--;
}




//=================================================================
// Origional code from Scavenger
// HIGHLY modified by Plasmatic
//-----------------------------------------------------
//we're gonna let them set up as many as they want, 
//we are going to limit how close they can be set though.


//BaseProj.cs

//deployVortexTurret.cs


//$CanAlwaysTeamDestroy[vortexTurretPack] = 1;

function deployVortexTurret::Initialize()
{	$TeamItemCount[0 @ vortexTurretPack] = 0;
	$TeamItemCount[1 @ vortexTurretPack] = 0;
	$TeamItemCount[2 @ vortexTurretPack] = 0;
	$TeamItemCount[3 @ vortexTurretPack] = 0;
	$TeamItemCount[4 @ vortexTurretPack] = 0;
	$TeamItemCount[5 @ vortexTurretPack] = 0;
	$TeamItemCount[6 @ vortexTurretPack] = 0;
	$TeamItemCount[7 @ vortexTurretPack] = 0;
}

ItemImageData vortexTurretPackImage 
{	shapeFile = "hellfiregun";//camera
	mountPoint = 2;
	mountOffset = { 0, -0.1, -0.06 };
	mountRotation = { 0, 0, 0 };
	firstPerson = false;
};

ItemData vortexTurretPack 
{	description = "Vortex Turret";
	shapeFile = "hellfiregun";
	className = "Backpack";
   heading = "gTurrets";
	imageType = vortexTurretPackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 300;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function vortexTurretPack::onUse(%player,%item) 
{	if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
		Player::mountItem(%player,%item,$BackpackSlot);
	else 
		Player::deployItem(%player,%item);
}


TurretData Deployablevortex 
{	className = "Turret";
	shapeFile = "hellfiregun";//camera,
	projectileType = IonEEBolt;
	maxDamage = 0.65;
	maxEnergy = 100;
	minGunEnergy = 1.0;
	maxGunEnergy = 1;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	reloadDelay = 0.4;
	speed = 3.0;
	speedModifier = 1.5;
	range = 60;
	visibleToSensor = true;
	shadowDetailMask = 4;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = flashDebrisMedium;
	shieldShapeName = "shield";
	fireSound = SoundFireLaser;
	activationSound = SoundRemoteTurretOn;
	deactivateSound = SoundRemoteTurretOff;
	explosionId = flashExpMedium;
	description = "vortex Turret";
	damageSkinData = "objectDamageSkins";
};

function Deployablevortex::onAdd(%this)
{	schedule("Deployablevortex::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,5);
	%this.shieldStrength = 0.05;
	if (GameBase::getMapName(%this) == "") 
		GameBase::setMapName (%this, "Vortex Turret");
}

function Deployablevortex::deploy(%this) 
{	GameBase::playSequence(%this,1,"deploy");
}

function Deployablevortex::onEndSequence(%this,%thread) 
{	GameBase::setActive(%this,true);
}

function Deployablevortex::onDestroyed(%this) 
{	Turret::onDestroyed(%this);
	$missionItems--;$TeamItemCount[GameBase::getTeam(%this) @ "vortexTurretPack"]--;$missionItems--;
}

function Deployablevortex::onPower(%this,%power,%generator) 
{
}

function Deployablevortex::onEnabled(%this) 
{	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);
}

function vortexTurretPack::onDeploy(%player,%item,%pos) { 
	
	if (vortexTurretPack::deployShape(%player,%item)&& !$build) { 
	Player::decItemCount(%player,%item); } 
	%client = Player::getClient(%player); 
	if(Client::isItemShoppingOn(%client,%item)) {  
		GameBase::playSound(%this, SoundFireMortar, 0.2);
		%velocity = 0;
		%zVec = 100;
		%jumpDir = Vector::getFromRot(GameBase::getRotation(%c),%velocity,%zVec);
		Player::applyImpulse(%client,%jumpDir);
		centerprint(%client, "<jc><f1>Plasmatic Sticks his BOOT up your ASS.", 10);
		Player::blowUp(%client); //This can be fun without killing them...
		playNextAnim(%client);   //Let the ceiling kill them...
		Player::kill(%client);   //Landing damage is disabled in SEX mod...
		//Client::onKilled(%c,%client); //Global death message

		//Client::sendMessage(%client,0,"[KmA]^Plasmatic^ Sticks his BOOT up your ASS.");// Chat box message
  	}
 } 

//------------relocated here from game.cs by Plasmatic broken down by deployable name

$TeamItemMax[vortexTurretPack] = 300;
additem(vortexTurretPack);
$MaxNumVortexInBox = 2;     //Number of remote turrets allowed in the area
$VortexBoxMaxLength = 50;    //Define Max Length of the area
$VortexBoxMaxWidth =  50;    //Define Max Width of the area
$VortexBoxMaxHeight = 15;    //Define Max Height of the area

//------------

function vortexTurretPack::deployShape(%player,%item) { 
%client = Player::getClient(%player); 
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]|| $build) { 
		if (GameBase::getLOSInfo(%player,3)) { 
		%obj = getObjectType($los::object); 
		%set = newObject("set",SimSet); 
		%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$VortexBoxMaxLength,$VortexBoxMaxWidth,$VortexBoxMaxHeight,0); 
	  	%num = CountObjects(%set,"Deployablevortex",%num);
		deleteObject(%set); 
		  if($MaxNumVortexInBox > %num) { 
			if (%obj == "SimTerrain" || %obj == "InteriorShape" || %obj == "DeployablePlatform") { 
			%prot = GameBase::getRotation(%player); 
			%zRot = getWord(%prot,2); 
			  if (Vector::dot($los::normal,"0 0 1") > 0.6) { %rot = "0 0 " @ %zRot; } else { if (Vector::dot($los::normal,"0 0 -1") > 0.6) { 
			  %rot = "3.14159 0 " @ %zRot; } 

			else { %rot = Vector::getRotation($los::normal); } } 
		if(checkDeployArea(%client,$los::position)) { 
			%camera = newObject("Camera","Turret",Deployablevortex,true);  
			addToSet("MissionCleanup", %camera);  
			GameBase::setTeam(%camera,GameBase::getTeam(%player));  
			GameBase::setRotation(%camera,%rot);  
			GameBase::setPosition(%camera,$los::position); 
			Gamebase::setMapName(%camera,"Vortex Turret#"@ $totalNumVortex++ @ " " @ Client::getName(%client)); 
 			echo("MSG: ",Client::getName(%client)," CL# ",%client," deployed a Vortex Turret");
			if(!$build)Client::sendMessage(%client,0,"Vortex Turret deployed");  
			playSound(SoundPickupBackpack,$los::position);  
			$missionItems++;$TeamItemCount[GameBase::getTeam(%camera) @ "vortexTurretPack"]++; 
			 $vortex = $vortex +1;
			return true; } }  
	  else { 
			if ($build) Client::sendMessage(%client,0,"~waccess_denied.wav");
			else Client::sendMessage(%client,0,"Can only deploy on terrain or buildings"); 
	} 
}  
	else 
		{	

			GameBase::playSound(%player, mine_act, 0);
			if (!$build) Client::sendMessage(%client,0,"Interference from other Vortex turrets in the area");} 
	}  
    else { 
			GameBase::playSound(%player, usepack, 0);
			if (!$build) Client::sendMessage(%client,0,"Deploy position out of range"); 
	}
 }  
  else Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s"); return false; } 


//--------------------------------------

//--------------------------------------



//--------------------------------------

$TeamItemMax[TurretPack] = 300;
additem(TurretPack);
//--------------------------------------

ItemImageData TurretPackImage
{
	shapeFile = "remoteturret";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

//------relocated here by Plasmatic


$MaxNumTurretsInBox = 20;     //Number of remote turrets allowed in the area
$TurretBoxMaxLength = 50;    //Define Max Length of the area
$TurretBoxMaxWidth =  50;    //Define Max Width of the area
$TurretBoxMaxHeight = 25;    //Define Max Height of the area

$TurretBoxMinLength = 20;	  //Define Min Length from another turret
$TurretBoxMinWidth =  2;	  //Define Min Width from another turret
$TurretBoxMinHeight = 2;    //Define Min Height from another turret



ItemData TurretPack
{
	description = "Turret";
	shapeFile = "remoteturret";
	className = "Backpack";
   heading = "gTurrets";
	imageType = TurretPackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 350;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function TurretPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function TurretPack::onDeploy(%player,%item,%pos)
{	
	if (TurretPack::deployShape(%player,%item)&& !$build) {
		Player::decItemCount(%player,%item);
	}
}

function CountObjects(%set,%name,%num) 
{
	%count = 0;
	for(%i=0;%i<%num;%i++) {
		%obj=Group::getObject(%set,%i);
		if(GameBase::getDataName(Group::getObject(%set,%i)) == %name) 
			%count++;
	}
	return %count;
}

function TurretPack::deployShape(%player,%item)
{
	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]|| $build) {
		if (GameBase::getLOSInfo(%player,3)) {
			%obj = getObjectType($los::object);
			if (%obj == "SimTerrain" || %obj == "InteriorShape") {
	    		%set = newObject("set",SimSet);
				%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMaxLength,$TurretBoxMaxWidth,$TurretBoxMaxHeight,0);
				%num = CountObjects(%set,"DeployableTurret",%num);
				deleteObject(%set);
				if($MaxNumTurretsInBox > %num) {
		    		%set = newObject("set",SimSet);
					%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMinLength,$TurretBoxMinWidth,$TurretBoxMinHeight,0);
					%num = CountObjects(%set,"DeployableTurret",%num);
					deleteObject(%set);
					if(0 == %num) {
						if (Vector::dot($los::normal,"0 0 1") > 0.7) {
							if(checkDeployArea(%client,$los::position)) {
								%rot = GameBase::getRotation(%player); 
								%turret = newObject("remoteTurret","Turret",DeployableTurret,true);
	                     addToSet("MissionCleanup", %turret);
								GameBase::setTeam(%turret,GameBase::getTeam(%player));
								GameBase::setPosition(%turret,$los::position);
								GameBase::setRotation(%turret,%rot);
								Gamebase::setMapName(%turret,"RMT Turret#" @ $totalNumTurrets++ @ " " @ Client::getName(%client));
								if(!$build)Client::sendMessage(%client,0,"Remote Turret deployed");
								playSound(SoundPickupBackpack,$los::position);
								$missionItems++;
								$TeamItemCount[GameBase::getTeam(%player) @ "TurretPack"]++;
								$Turret = $Turret +1;
								echo("MSG: ",Client::getName(%client)," CL# ",%client," deployed a Remote Turret");
								//	Remote turrets - kill points to player that deploy them
								Client::setOwnedObject(%client, %turret); 
								Client::setOwnedObject(%client, %player);
								return true;
							}
						}
						else {
			if ($build) Client::sendMessage(%client,0,"~waccess_denied.wav");
			else Client::sendMessage(%client,0,"Turrets only deploy on flat surfaces");}
					} 
					else{

			GameBase::playSound(%player, mine_act, 0);
			if (!$build) Client::sendMessage(%client,0,"Ionic interference with other turrets.");
			}
				}
			   else {

			GameBase::playSound(%player, mine_act, 0);
			if (!$build) Client::sendMessage(%client,0,"Interference from other remote turrets.");
			}
			}
			else {
			if ($build) Client::sendMessage(%client,0,"~waccess_denied.wav");
			else Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");}
		}
		else {
			GameBase::playSound(%player, usepack, 0);
			if (!$build) Client::sendMessage(%client,0,"Deploy position out of range");}
	}
	else																						  
	 	Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");

	return false;
}

//--------------------------------------

TurretData DeployableTurret
{
	className = "Turret";
	shapeFile = "remoteturret";
   validateShape = true;
//   validateMaterials = true;
	projectileType = MiniFusionBolt;
	maxDamage = 0.75;
	maxEnergy = 200;
	minGunEnergy = 75;//6
	maxGunEnergy = 1;//5
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	reloadDelay = 0.2;
	speed = 4.0;
	speedModifier = 1.5;
	range = 30;
	visibleToSensor = true;
	shadowDetailMask = 4;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = flashDebrisMedium;
	shieldShapeName = "shield";
	fireSound = SoundRemoteTurretFire;
	activationSound = SoundRemoteTurretOn;
	deactivateSound = SoundRemoteTurretOff;
	explosionId = flashExpMedium;
	description = "Remote Turret";
	damageSkinData = "objectDamageSkins";
};

function DeployableTurret::onAdd(%this)
{
	schedule("DeployableTurret::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,50);
	%this.shieldStrength = 0;
	if (GameBase::getMapName(%this) == "") {
		GameBase::setMapName (%this, "Remote Turret");
	}
}

function DeployableTurret::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function DeployableTurret::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function DeployableTurret::onDestroyed(%this)
{
	Turret::onDestroyed(%this);
  	$TeamItemCount[GameBase::getTeam(%this) @ "TurretPack"]--;$missionItems--;
}

// Override base class just in case.
function DeployableTurret::onPower(%this,%power,%generator) {}
function DeployableTurret::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,50);
	GameBase::setActive(%this,true);
}

//=========================================
// Origional Zapper code by Plasmatic
// Please get ahold of me if you want to use this
// ziptiezmail@netscape.net





LightningData ZapperCharge
{
   bitmapName       = "repairadd.bmp"; //lightningNew.bmp
   damageType       = $ElectricityDamageType;
   boltLength       = 40.0;
   coneAngle        = 35.0;
   damagePerSec      = 0.06;
   energyDrainPerSec = 60.0;
   segmentDivisions = 4;
   numSegments      = 5;
   beamWidth        = 0.125;

   updateTime   = 120;
   skipPercent  = 0.5;
   displaceBias = 0.15;

   lightRange = 3.0;
   lightColor = { 0.25, 0.25, 0.85 };

   soundId = SoundELFFire;
};

//--------------------------------------

//--------------------------------------

ItemImageData ZapperPackImage
{
	shapeFile = "indoorgun"; //remoteturret,camera
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = true; //false
};

ItemData ZapperPack
{
	description = "Zapper"; //TurretMine
	shapeFile = "indoorgun"; //camera,remoteturret
	className = "Backpack";
   heading = "gTurrets";
	imageType = ZapperPackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 350;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function ZapperPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function ZapperPack::onDeploy(%player,%item,%pos)
{	
	if (ZapperPack::deployShape(%player,%item)&& !$build) {
		Player::decItemCount(%player,%item);
	}
}

function CountObjects(%set,%name,%num) 
{
	%count = 0;
	for(%i=0;%i<%num;%i++) {
		%obj=Group::getObject(%set,%i);
		if(GameBase::getDataName(Group::getObject(%set,%i)) == %name) 
			%count++;
	}
	return %count;
}
//------------relocated here from game.cs by plasmatic broken down by deployable name
//--------------------------------------

//limit how many can be set in box
$TeamItemMax[ZapperPack] = 300;
additem(ZapperPack);
$MaxNumZappersInBox = 2;     //Number of remote turrets allowed in the area
$ZapperBoxMaxLength = 50;    //Define Max Length of the area
$ZapperBoxMaxWidth =  50;    //Define Max Width of the area
$ZapperBoxMaxHeight = 15;    //Define Max Height of the area

//------------
function ZapperPack::deployShape(%player,%item) { %client = Player::getClient(%player); if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]|| $build){ if (GameBase::getLOSInfo(%player,3)) { %obj = getObjectType($los::object); %set = newObject("set",SimSet); %num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$ZapperBoxMaxLength,$ZapperBoxMaxWidth,$ZapperBoxMaxHeight,0);   %num = CountObjects(%set,"DeployableZapper",%num);   deleteObject(%set); if($MaxNumZappersInBox > %num) { if (%obj == "SimTerrain" || %obj == "InteriorShape" || %obj == "DeployablePlatform") { 
				%prot = GameBase::getRotation(%player); 
				%zRot = getWord(%prot,2); 
				if (Vector::dot($los::normal,"0 0 1") > 0.6) { %rot = "0 0 " @ %zRot; } else { if (Vector::dot($los::normal,"0 0 -1") > 0.6) { 
		%rot = "3.14159 0 " @ %zRot; } 

	else { %rot = Vector::getRotation($los::normal); } } 
		if(checkDeployArea(%client,$los::position)) { 
			%camera = newObject("Camera","Turret",DeployableZapper,true);  
				addToSet("MissionCleanup", %camera);  
				GameBase::setTeam(%camera,GameBase::getTeam(%player));  
				GameBase::setRotation(%camera,%rot);  
				GameBase::setPosition(%camera,$los::position); 
				Gamebase::setMapName(%camera,"Zapper Turret#"@ $totalNumZapper++ @ " " @ Client::getName(%client));  
				if(!$build)Client::sendMessage(%client,0,"Zapper deployed");
  				echo("MSG: ",Client::getName(%client)," CL# ",%client," deployed a Zapper");
				playSound(SoundPickupBackpack,$los::position);  
				$missionItems++;
				$TeamItemCount[GameBase::getTeam(%camera) @ "ZapperPack"]++;
				 $Zapper = $Zapper +1;
					return true; } }  
			else { if ($build) Client::sendMessage(%client,0,"~waccess_denied.wav");
			else Client::sendMessage(%client,0,"Can only deploy on terrain or buildings"); 
			}
 }  
		else{
			GameBase::playSound(%player, mine_act, 0);
			if (!$build) Client::sendMessage(%client,0,"Interference from other Zapper turrets in the area"); 
		}
		}  
			else { 
			GameBase::playSound(%player, usepack, 0);
			if (!$build) Client::sendMessage(%client,0,"Deploy position out of range"); }
		 }  
			else Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
			 return false; 
	} 


//--------------------------------------

TurretData DeployableZapper
{
	className = "Turret"; //Turret
	shapeFile = "indoorgun";//camera
   validateShape = true;
//   validateMaterials = true;
	projectileType = ZapperCharge;
	maxDamage = 0.65;
	maxEnergy = 100;
	minGunEnergy = 10;//6
	maxGunEnergy = 90;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	reloadDelay = 0.4;
	speed = 5.0;
	speedModifier = 1.5;
	range = 30;
	visibleToSensor = true;
	shadowDetailMask = 8;//4
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	supressable = false;
	pinger = true;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = flashDebrisMedium;
	shieldShapeName = "shield";
	//fireSound        = SoundGeneratorPower;

	activationSound = SoundRemoteTurretOn;
	deactivateSound = SoundRemoteTurretOff;
	explosionId = flashExpMedium;
	description = "DeployableZapper"; //RemoteTurretMine
	damageSkinData = "objectDamageSkins";
	
	isSustained     = true;
   firingTimeMS    = 750;
   energyRate      = 30.0;


};

function DeployableZapper::onAdd(%this)
{
	schedule("DeployableZapper::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,5);
	%this.shieldStrength = 0.01;
	if (GameBase::getMapName(%this) == "") {
		GameBase::setMapName (%this, "Zapper");
	}
}

function DeployableZapper::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function DeployableZapper::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function DeployableZapper::onDestroyed(%this)
{
	Turret::onDestroyed(%this);
  	$TeamItemCount[GameBase::getTeam(%this) @ "DeployableZapper"]--;$missionItems--;
	GameBase::setRechargeRate(%this,0); GameBase::setActive(%this,false);
}

// Override base class just in case.
function DeployableZapper::onPower(%this,%power,%generator) {}
function DeployableZapper::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);
}


//=====================================
//Origional code by Plasmatic
//Please get ahold of me if you want to use this
//ziptiezmail@netscape.net






//--------------------------------------




ItemImageData PencilPackImage 
{ 
shapeFile = "camera"; 
mountPoint = 2; 
mountOffset = { 0, -0.1, -0.06 }; 
mountRotation = { 0, 0, 0 }; 
firstPerson = false; 
};

ItemData PencilPack { description = "Pencil Turret"; 
	shapeFile = "camera"; 
	className = "Backpack"; 
   heading = "gTurrets";
	imageType = PencilPackImage; 
	shadowDetailMask = 4; mass = 2.0; elasticity = 0.2; price = 500; 
	hudIcon = "deployable"; showWeaponBar = true; hiliteOnActive = true; }; 

function PencilPack::onUse(%player,%item) {
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) { 
	Player::mountItem(%player,%item,$BackpackSlot); 
	GameBase::setActive(%item,true);
	GameBase::playSequence(%item,1,"deploy");
	GameBase::setMapName (%item, "Pencil Turret");
	} 
	else { Player::deployItem(%player,%item); } } 

function PencilPack::onDeploy(%player,%item,%pos) { 
	
	if (PencilPack::deployShape(%player,%item)&& !$build) { 
	Player::decItemCount(%player,%item); } 
	%client = Player::getClient(%player); 
	if(!$build && Client::isItemShoppingOn(%client,%item)) {  
		playNextAnim(%client); 
		Player::kill(%client); 
		centerprint(%client, "<jc><f1>No exploiting Inventories!! By order of Plasmatic.", 10);
		Client::sendMessage(%client,0,"No exploiting Inventories!!");  }
		 } 

//------------relocated here from game.cs by Plasmatic broken down by deployable name

$TeamItemMax[PencilPack] = 300;//large numbers mean large fun...
additem(PencilPack);
$MaxNumPencilInBox = 5;     //Number of remote turrets allowed in the area
$PencilBoxMaxLength = 50;    //Define Max Length of the area
$PencilBoxMaxWidth =  50;    //Define Max Width of the area
$PencilBoxMaxHeight = 15;    //Define Max Height of the area

//we dont care how close they're set, just how many in a given area
//------------

function PencilPack::deployShape(%player,%item) { 
	%client = Player::getClient(%player); 
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]|| $build) { 
	  if (GameBase::getLOSInfo(%player,3)) { %obj = getObjectType($los::object); 
	  %set = newObject("set",SimSet); 
	  %num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$PencilBoxMaxLength,$PencilBoxMaxWidth,$PencilBoxMaxHeight,0); 
	  %num = CountObjects(%set,"DeployablePencil",%num); 
	  deleteObject(%set); 
	    if($MaxNumPencilInBox > %num) { 
		 if (%obj == "SimTerrain" || %obj == "InteriorShape" ) { 
		   %prot = GameBase::getRotation(%player); 
		   %zRot = getWord(%prot,2); 
			if (Vector::dot($los::normal,"0 0 1") > 0.6) { 
			%rot = "0 0 " @ %zRot; } 
			  else { if (Vector::dot($los::normal,"0 0 -1") > 0.6) { 
			  %rot = "3.14159 0 " @ %zRot; } 
				else { %rot = Vector::getRotation($los::normal); 
			} 
		} 
			if(checkDeployArea(%client,$los::position)) {
 			  %camera = newObject("Camera","Turret",DeployablePencil,true); 
			  addToSet("MissionCleanup", %camera); 
			  GameBase::setTeam(%camera,GameBase::getTeam(%player)); 
			  GameBase::setRotation(%camera,%rot); 
			  GameBase::setPosition(%camera,$los::position); 
			  Gamebase::setMapName(%camera,"Pencil Turret# "@ $totalNumPencil++ @ " " @ Client::getName(%client));
			  if(!$build)Client::sendMessage(%client,0,"Pencil Turret deployed"); 
			echo("MSG: ",Client::getName(%client)," CL# ",%client," deployed a Pencil Turret");
			  playSound(SoundPickupBackpack,$los::position); 
		//	Remote turrets - kill points to player that deploy them
		 Client::setOwnedObject(%client, %camera); 
		 Client::setOwnedObject(%client, %player);
				$missionItems++;
				$TeamItemCount[GameBase::getTeam(%camera) @ "PencilPack"]++; 
			  $Pencil = $Pencil +1;
			  return true; } } 

		  else { if ($build) Client::sendMessage(%client,0,"~waccess_denied.wav");
			else Client::sendMessage(%client,0,"Can only deploy on terrain or buildings"); }
	 } 
		else{
			GameBase::playSound(%player, mine_act, 0);
			if (!$build) Client::sendMessage(%client,0,"Interference from other Pencil Turrets in the area");
			}
		 } 
	  else { 	
			GameBase::playSound(%player, usepack, 0);
			if (!$build) Client::sendMessage(%client,0,"Deploy position out of range"); } 
	} 
	else Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s"); 
	return false;
	 } 


TurretData DeployablePencil { className = "Turret"; 
	shapeFile = "camera";	projectileType = PencilTurretBullet; 
	maxDamage = 0.5; 
	maxEnergy = 60; 
	minGunEnergy = 1; 
	maxGunEnergy = 1; 
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor }; 
	reloadDelay = 0.1; 
	speed = 4.0; 
	speedModifier = 1.5; 
	range = 70; 
	visibleToSensor = true; shadowDetailMask = 4; dopplerVelocity = 0; 
	castLOS = true; supression = false; mapFilter = 2; mapIcon = "M_turret"; 
	debrisId = flashDebrisMedium; shieldShapeName = "shield"; 
	fireSound = ricochet3; 	activationSound = SoundRemoteTurretOn; 
	deactivateSound = SoundRemoteTurretOff; explosionId = flashExpMedium; 
	description = "Pencil Turret"; 
	damageSkinData = "objectDamageSkins"; }; 

function DeployablePencil::onAdd(%this) { 
	schedule("DeployablePencil::deploy(" @ %this @ ");",1,%this); 
	GameBase::setActive(%this,true);
	GameBase::setRechargeRate(%this,5); 
	%this.shieldStrength = 0; 
		if (GameBase::getMapName(%this) == "") { GameBase::setMapName (%this, "Pencil Turret"); } } 


function DeployablePencil::deploy(%this) 
{ 
	GameBase::playSequence(%this,1,"deploy"); 
} 

function DeployablePencil::onEndSequence(%this,%thread) 
{ 
GameBase::setActive(%this,true); 
} 

function DeployablePencil::onDestroyed(%this) 
{ 
	Turret::onDestroyed(%this); 
	$TeamItemCount[GameBase::getTeam(%this) @ "PencilPack"]--; $missionItems--;
} 

function DeployablePencil::onPower(%this,%power,%generator) 
{
} 

function DeployablePencil::onEnabled(%this) 
{ 
GameBase::setRechargeRate(%this,5); 	
GameBase::setActive(%this,true); 
} 


///-----------------------------------------------


$TeamItemMax[door5x5pack] = 1000;
additem(door5x5pack);
$ItemMax[larmor, door5x5pack] = 1;
$ItemMax[lfemale, door5x5pack] = 1;
$ItemMax[marmor, door5x5pack] = 1;
$ItemMax[mfemale, door5x5pack] = 1;
$ItemMax[harmor, door5x5pack] = 1;

///-----------------------------------------------

ItemImageData door5x5packImage
{
        //shapeFile = "forcefield_5x5";
        shapeFile = "armorkit";//AmmoPack
        mountPoint = 2;
        mountOffset = { -0.375, 0, 0 };
        mountRotation = { 0, 1.57, 0 };
        mass = 2.5;
        firstPerson = false;
};

ItemData door5x5pack
{
        description = "Blue Door";
        //shapeFile = "forcefield_5x5";
        shapeFile = "armorkit";//
        className = "Backpack";
        heading = "jBarricades";//        heading = "dDeployables";
        imageType = door5x5packImage;
        shadowDetailMask = 4;
        mass = 2.5;
        elasticity = 0.2;
        price = 300;
        hudIcon = "deployable";
        showWeaponBar = true;
        hiliteOnActive = true;
};

function door5x5pack::onUse(%player,%item)
{
        if (Player::getMountedItem(%player,$BackpackSlot) != %item)
        {
                Player::mountItem(%player,%item,$BackpackSlot);
        }
        else
        {
                Player::deployItem(%player,%item);
        }
}

function door5x5pack::onDeploy(%player,%item,%pos)
{	
        if (door5x5pack::deployShape(%player,%item)&& !$build)
        {
                Player::decItemCount(%player,%item);
        }
}

function door5x5pack::deployShape(%player,%item)
{
         %client = Player::getClient(%player);
        if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]|| $build) {
                if (GameBase::getLOSInfo(%player,3)) {

                        %obj = getObjectType($los::object);
                                %rot = GameBase::getRotation(%player);

                                        %camera = newObject("door5x5pack","StaticShape",doorfivebyfiveForceFieldShape,true);
                                        addToSet("MissionCleanup", %camera);
                                        GameBase::setTeam(%camera,GameBase::getTeam(%player));
                                        GameBase::setRotation(%camera,%rot);
                                        GameBase::setPosition(%camera,$los::position);
                                        Gamebase::setMapName(%camera,"Blue Door #"@ $totalNumBlue++ @ " " @ Client::getName(%client));
                                        if(!$build)Client::sendMessage(%client,0,"Blue Door deployed");
                                        playSound(SoundPickupBackpack,$los::position);
                                        $missionItems++;
					$TeamItemCount[GameBase::getTeam(%camera) @ "door5x5pack"]++;
                                        $bluedoor = $bluedoor +1;
			echo("MSG: ",Client::getName(%client)," CL# ",%client," deployed a Blue Door");
                                        return true;

                        }
                       else {
			GameBase::playSound(%player, usepack, 0);
			if (!$build) Client::sendMessage(%client,0,"I didn't say 'Simon sez...'");
                        }

        }
        else
                 Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");

        return false;
}

///-----------------------------------------------


StaticShapeData doorfivebyfiveForceFieldShape
{
className = "LargeForceField";
damageSkinData = "objectDamageSkins";
shapeFile = "forcefield_5x5";//
maxDamage = 30.0;
maxEnergy = 200;
mapFilter = 2;
visibleToSensor = true;
explosionId = mortarExp;
debrisId = flashDebrisLarge;
lightRadius = 12.0;
lightType=2;
lightColor = {1.0,0.2,0.2};
side = "single";
isTranslucent = true;
description = "Blue Door";
};
function doorfivebyfiveForceFieldShape::Destruct(%this)
{
doorfivebyfiveForceFieldShape::doDamage(%this);
}
function doorfivebyfiveForceFieldShape::doDamage(%this) {
calcRadiusDamage(%this, $DebrisDamageType, 5, 0.5, 25, 15, 4, 0.4, 0.1, 250, 100);
}
function doorfivebyfiveForceFieldShape::onDestroyed(%this)
{
doorfivebyfiveForceFieldShape::doDamage(%this);
$TeamItemCount[GameBase::getTeam(%this) @ "LargeForceField"]--;$missionItems--;
}
function doorfivebyfiveForceFieldShape::onCollision(%this,%obj)
{
if(getObjectType(%obj)!="Player" || Player::isDead(%obj)) {
return;
}
%c = Player::getClient(%obj);
%playerTeam = GameBase::getTeam(%obj);
%fieldTeam = GameBase::getTeam(%this);
if(%fieldTeam != %playerTeam)
{
return;
}
doorfivebyfiveForceFieldShape::openDoor(%this);
return;
}
function doorfivebyfiveForceFieldShape::openDoor(%this) {

GameBase::startfadeout(%this);

%pos=GameBase::getPosition(%this);
%pos=Vector::add(%pos,"0 0 6");
GameBase::setPosition(%this,%pos);
schedule("GameBase::playSound("@%this@",ForceFieldOpen,0);",0.15);
schedule("doorfivebyfiveForceFieldShape::closeDoor("@%this@");",4);
}
function doorfivebyfiveForceFieldShape::closeDoor(%this) {
%pos=GameBase::getPosition(%this);
%pos=Vector::add(%pos,"0 0 -6");
GameBase::setPosition(%this,%pos);
GameBase::startfadein(%this);
schedule("GameBase::playSound("@%this@",ForceFieldClose,0);",0.15);

}

function doorfivebyfiveForceFieldShape::openDoor(%this) {

GameBase::startfadeout(%this);

%pos=GameBase::getPosition(%this);
%pos=Vector::add(%pos,"0 0 6");
GameBase::setPosition(%this,%pos);
schedule("GameBase::playSound("@%this@",ForceFieldOpen,0);",0.15);
schedule("doorfivebyfiveForceFieldShape::closeDoor("@%this@");",2);
}


///-----------------------------------------------


$ItemMax[larmor, door4x14pack] = 1;
$ItemMax[lfemale, door4x14pack] = 1;
$ItemMax[marmor, door4x14pack] = 1;
$ItemMax[mfemale, door4x14pack] = 1;
$ItemMax[harmor, door4x14pack] = 1;

$TeamItemMax[door4x14pack] = 1000;
additem(door4x14pack);
///-----------------------------------------------


ItemImageData door4x14packImage
{
        //shapeFile = "forcefield_4x14";
        shapeFile = "ammo2";//AmmoPack
        mountPoint = 2;
        mountOffset = { -0.25, 0, 0 };
        mountRotation = { 1.57, 1.57, 0 };
        mass = 2.5;
        firstPerson = false;
};

ItemData door4x14pack
{
        description = "Big Blue Door";
        //shapeFile = "forcefield_4x14";
        shapeFile = "ammo2";//AmmoPack
        className = "Backpack";
        heading = "jBarricades";//          heading = "dDeployables"; 
        imageType = door4x14packImage;
        shadowDetailMask = 4;
        mass = 2.5;
        elasticity = 0.2;
        price = 600;
        hudIcon = "deployable";
        showWeaponBar = true;
        hiliteOnActive = true;
};

function door4x14pack::onUse(%player,%item)
{
        if (Player::getMountedItem(%player,$BackpackSlot) != %item)
        {
                Player::mountItem(%player,%item,$BackpackSlot);
        }
        else
        {
                Player::deployItem(%player,%item);
        }
}

function door4x14pack::onDeploy(%player,%item,%pos)
{	
        if (door4x14pack::deployShape(%player,%item) && !$build)
        {
                Player::decItemCount(%player,%item);
        }
}

function door4x14pack::deployShape(%player,%item)
{
         %client = Player::getClient(%player);
        if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]|| $build) {
                if (GameBase::getLOSInfo(%player,3)) {

                        %obj = getObjectType($los::object);
                                %rot = GameBase::getRotation(%player);

                                        %camera = newObject("door4x14pack","StaticShape",BigBlueDoorShape,true);
                                        addToSet("MissionCleanup", %camera);
                                        GameBase::setTeam(%camera,GameBase::getTeam(%player));
                                        GameBase::setRotation(%camera,%rot);
                                        GameBase::setPosition(%camera,$los::position);
                                        Gamebase::setMapName(%camera,"Big Blue Door #"@ $totalNumBigBlue++ @ " " @ Client::getName(%client));
                                        if(!$build)Client::sendMessage(%client,0,"Big Blue Door deployed");
                                        playSound(SoundPickupBackpack,$los::position);
                                        $missionItems++;
					$TeamItemCount[GameBase::getTeam(%camera) @ "door4x14pack"]++;
					$bigbluedoor = $bigbluedoor +1;
			echo("MSG: ",Client::getName(%client)," CL# ",%client," deployed a Big Blue Door");
                                        return true;

                        }
                       else   {
			GameBase::playSound(%player, usepack, 0);
			if (!$build) Client::sendMessage(%client,0,"Too far away.");

                        }

         }
        else
                 Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");

        return false;
}



StaticShapeData BigBlueDoorShape
{
className = "LargeForceField";
damageSkinData = "objectDamageSkins";
shapeFile = "forcefield_4x14";
maxDamage = 30.0;
maxEnergy = 200;
mapFilter = 2;
visibleToSensor = true;
explosionId = mortarExp;
debrisId = flashDebrisLarge;
lightRadius = 12.0;
lightType=2;
lightColor = {1.0,0.2,0.2};
side = "single";
isTranslucent = true;
description = "Big Blue Door";
};

function BigBlueDoorShape::Destruct(%this)
      {BigBlueDoorShape::doDamage(%this);}

function BigBlueDoorShape::doDamage(%this) {
calcRadiusDamage(%this, $DebrisDamageType, 5, 0.5, 25, 15, 4, 0.4, 0.1, 250, 100);
}

function BigBlueDoorShape::onDestroyed(%this)
{	BigBlueDoorShape::doDamage(%this);
	$TeamItemCount[GameBase::getTeam(%this) @ "LargeForceField"]--;$missionItems--;
}

function BigBlueDoorShape::onCollision(%this,%obj)
{	if(getObjectType(%obj)!="Player" || Player::isDead(%obj)) {
	return;
}
		%c = Player::getClient(%obj);
		%playerTeam = GameBase::getTeam(%obj);
		%fieldTeam = GameBase::getTeam(%this);
		if(%fieldTeam != %playerTeam)
		{return;}
			BigBlueDoorShape::openDoor(%this);return;
}

function BigBlueDoorShape::openDoor(%this) {

GameBase::startfadeout(%this);

%pos=GameBase::getPosition(%this);
%pos=Vector::add(%pos,"0 0 6");
GameBase::setPosition(%this,%pos);
schedule("GameBase::playSound("@%this@",ForceFieldOpen,0);",0.15);
schedule("BigBlueDoorShape::closeDoor("@%this@");",4);
}

function BigBlueDoorShape::closeDoor(%this) {
%pos=GameBase::getPosition(%this);
%pos=Vector::add(%pos,"0 0 -6");
GameBase::setPosition(%this,%pos);
GameBase::startfadein(%this);
schedule("GameBase::playSound("@%this@",ForceFieldClose,0);",0.15);

}

function BigBlueDoorShape::openDoor(%this) {

GameBase::startfadeout(%this);

%pos=GameBase::getPosition(%this);
%pos=Vector::add(%pos,"0 0 6");
GameBase::setPosition(%this,%pos);
schedule("GameBase::playSound("@%this@",ForceFieldOpen,0);",0.15);
schedule("BigBlueDoorShape::closeDoor("@%this@");",2);
}


///-----------------------------------------------



$ItemMax[harmor, BlastWall] = 1;
$ItemMax[larmor, BlastWall] = 1;
$ItemMax[lfemale, BlastWall] = 1;
$ItemMax[marmor, BlastWall] = 1;
$ItemMax[mfemale, BlastWall] = 1;

///-----------------------------------------------

$TeamItemMax[BlastWall] = 200;
additem(BlastWall);
ItemImageData BlastWallImage
{
        shapeFile = "AmmoPack";//newdoor5
        mountPoint = 2;
        //mountOffset = { -0.375, 0, 0 };
        //mountRotation = { 0, 1.57, 0 };
        mass = 2.5;
        firstPerson = false;
};

ItemData BlastWall
{
        description = "Blast Wall";
        shapeFile = "AmmoPack";//newdoor5
        className = "Backpack";
        heading = "jBarricades";//dDeployables";
        imageType = BlastWallImage;
        shadowDetailMask = 4;
        mass = 2.5;
        elasticity = 0.2;
        price = 500;
        hudIcon = "deployable";
        showWeaponBar = true;
        hiliteOnActive = true;
};

function BlastWall::onUse(%player,%item)
{
        if (Player::getMountedItem(%player,$BackpackSlot) != %item)
        {
                Player::mountItem(%player,%item,$BackpackSlot);
        }
        else
        {
                Player::deployItem(%player,%item);
        }
}

function BlastWall::onDeploy(%player,%item,%pos)
{	
        if (BlastWall::deployShape(%player,%item)&& !$build)
        {
                Player::decItemCount(%player,%item);
        }
}

function BlastWall::deployShape(%player,%item)
{
         %client = Player::getClient(%player);
        if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]|| $build) {
                if (GameBase::getLOSInfo(%player,3)) {

                        %obj = getObjectType($los::object);
                                %rot = GameBase::getRotation(%player);

                                        %camera = newObject("BlastWall","StaticShape",BlastWallShape,true);
                                        addToSet("MissionCleanup", %camera);
                                        GameBase::setTeam(%camera,GameBase::getTeam(%player));
                                        GameBase::setRotation(%camera,%rot);
                                        GameBase::setPosition(%camera,$los::position);
                                        Gamebase::setMapName(%camera,"Wall#"@ $totalNumBlast++ @ " " @ Client::getName(%client));
                                        if(!$build)Client::sendMessage(%client,0,"Wall deployed");
                                        playSound(SoundPickupBackpack,$los::position);
                                        $missionItems++;
					$TeamItemCount[GameBase::getTeam(%camera) @ "BlastWall"]++;
                                        $blast = $blast +1;
			echo("MSG: ",Client::getName(%client)," CL# ",%client," deployed a Blast wall");
                                        return true;

                        }
                       else {
			GameBase::playSound(%player, usepack, 0);
			if (!$build) Client::sendMessage(%client,0,"Cannot deploy here.");
                        }

        }
        else
                 Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");

        return false;
}

StaticShapeData BlastWallShape
{
        shapeFile = "newdoor5";
        debrisId = defaultDebrisLarge;
        maxDamage = 60.0;
        visibleToSensor = true;
        isTranslucent = true;
        description = "Portable Wall";
};

function BlastWallShape::onDestroyed(%this)
{
   StaticShape::onDestroyed(%this);
   $TeamItemCount[GameBase::getTeam(%this) @ "BlastWall"]--;$missionItems--;

}


///-----------------------------------------------



$ItemMax[harmor, plat] = 1;
$ItemMax[larmor, plat] = 1;
$ItemMax[lfemale, plat] = 1;
$ItemMax[marmor, plat] = 1;
$ItemMax[mfemale, plat] = 1;

///-----------------------------------------------

$TeamItemMax[plat] = 2000;
additem(plat);
ItemImageData platImage
{
        shapeFile = "AmmoPack";//newdoor5
        mountPoint = 2;
        //mountOffset = { -0.375, 0, 0 };
        //mountRotation = { 0, 1.57, 0 };
        mass = 2.5;
        firstPerson = false;
};

ItemData plat
{
        description = "Platform";
        shapeFile = "AmmoPack";//newdoor5
        className = "Backpack";
         heading = "jBarricades";//       heading = "dDeployables";
        imageType = platImage;
        shadowDetailMask = 4;
        mass = 2.5;
        elasticity = 0.2;
        price = 500;
        hudIcon = "deployable";
        showWeaponBar = true;
        hiliteOnActive = true;
};

function plat::onUse(%player,%item)
{
        if (Player::getMountedItem(%player,$BackpackSlot) != %item)
        {
                Player::mountItem(%player,%item,$BackpackSlot);
        }
        else
        {
                Player::deployItem(%player,%item);
        }
}

function plat::onDeploy(%player,%item,%pos)
{
	
        if (plat::deployShape(%player,%item)&& !$build)
        {
                Player::decItemCount(%player,%item);
        }
}

function plat::deployShape(%player,%item)
{
         %client = Player::getClient(%player);
        if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]|| $build) {
                if (GameBase::getLOSInfo(%player,3)) {

                        %obj = getObjectType($los::object);
                                %prot = GameBase::getRotation(%player);
					  %xRot = getWord(%prot,0);
					  %yRot = getWord(%prot,1);
					  %zRot = getWord(%prot,2);
				if (Vector::dot($los::normal,"0 0 1") > 0.6) {
						%rot = GameBase::getRotation(%player);//floor
				}
				else {
					if (Vector::dot($los::normal,"0 0 -1") > 0.6) {
						%rot =  %xRot + 3.14159@" " @%yRot@" "@%zRot;//ceiling...
					}
					else {
						%rot = GameBase::getRotation(%player);//other
					}
				}

                                        %camera = newObject("plat","StaticShape",platShape,true);
                                        addToSet("MissionCleanup", %camera);
                                        GameBase::setTeam(%camera,GameBase::getTeam(%player));
                                        GameBase::setRotation(%camera,%rot);
                                        GameBase::setPosition(%camera,$los::position);
                                        Gamebase::setMapName(%camera,"Platform#"@ $totalNumPlat++ @ " " @ Client::getName(%client));
                                        if(!$build)Client::sendMessage(%client,0,"Platform deployed");
                                        playSound(SoundPickupBackpack,$los::position);
                                        $missionItems++;
					$TeamItemCount[GameBase::getTeam(%camera) @ "plat"]++;
                                        $plat = $plat +1;
			echo("MSG: ",Client::getName(%client)," CL# ",%client," deployed a Platform");
                                        return true;

                        }
                       else {
			GameBase::playSound(%player, usepack, 0);
			if (!$build) Client::sendMessage(%client,0,"Cannot deploy here.");
                        }

        }
        else
                 Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");

        return false;
}

StaticShapeData platShape
{
        shapeFile = "elevator6X6thin";//newdoor5,elevator_4x4,elevpad2,mainpad,logo,// elevator6X6thin
        debrisId = defaultDebrisLarge;
        maxDamage = 25.0;
        visibleToSensor = true;
        isTranslucent = true;
        description = "Platform";
};

function platShape::onDestroyed(%this)
{
   StaticShape::onDestroyed(%this);
   $TeamItemCount[GameBase::getTeam(%this) @ "plat"]--;$missionItems--;

}



///-----------------------------------------------

//Origional code by Plasmatic
//Please get ahold of me if you want to use this
//ziptiezmail@netscape.net

///-----------------------------------------------
///-----------------------------------------------


$ItemMax[harmor, Twig] = 1;
$ItemMax[larmor, Twig] = 1;
$ItemMax[lfemale, Twig] = 1;
$ItemMax[marmor, Twig] = 1;
$ItemMax[mfemale, Twig] = 1;



$SellAmmo[Seed] = 5;
$AmmoPackMax[Seed] = 20;
$AmmoPackItems[10] = Seed;
$WeaponAmmo[JumpGun] = Seed;

$ItemMax[larmor, Seed] = 10;
$ItemMax[lfemale, Seed] = 10;
$ItemMax[marmor, Seed] = 5;
$ItemMax[mfemale, Seed] = 5;
$ItemMax[harmor, Seed] = 1;
additem(seed);
///-----------------------------------------------

$TeamItemMax[Twig] = 1000;
additem(Twig);
ItemImageData TwigImage
{
        shapeFile = "MrTwig";//newdoor5
        mountPoint = 2;
        //mountOffset = { -0.375, 0, 0 };
        //mountRotation = { 0, 1.57, 0 };
        mass = 2.5;
        firstPerson = false;
};

ItemData Twig
{
        description = "Mr Twig";
        shapeFile = "MrTwig";//newdoor5
        className = "Backpack";
        heading = "jBarricades";//        heading = "dDeployables";
        imageType = TwigImage;
        shadowDetailMask = 4;
        mass = 2.5;
        elasticity = 0.2;
        price = 5;
        hudIcon = "deployable";
        showWeaponBar = true;
        hiliteOnActive = true;
};

ItemData Seed
{
	description = "Seed";
	className = "Ammo";
	shapeFile = "mortarammo";//mortarammo,endarrow,endarrow
   heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 2;
};



function Twig::onMount(%player,%item){
		%client = Player::getClient(%player);
		%AmmoCount = (Player::getItemCount(%player,Seed));
		if(%AmmoCount == 0 && Client::isItemShoppingOn(%client,%item)){	

		buyItem(%client,Seed);
		//Client::sendMessage(%client,0,"Auto buying seeds");
		}
	}



function Twig::onUse(%player,%item)
{
        if (Player::getMountedItem(%player,$BackpackSlot) != %item)
        {
                Player::mountItem(%player,%item,$BackpackSlot);
        }
        else
        {
                Player::deployItem(%player,%item);
        }
}

function Twig::onDeploy(%player,%item,%pos)
{	
        if (Twig::deployShape(%player,%item))
        {
	%AmmoCount = ( Player::getItemCount(%player,Seed)) ;
	if(%AmmoCount)Player::decItemCount(%player,Seed);

	else Player::decItemCount(%player,%item);
        }
}
function Twig::deployShape(%player,%item)
{
      %client = Player::getClient(%player);
	%AmmoCount = ( Player::getItemCount(%player,Seed)+1) ;
	if(%AmmoCount){
        if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]|| $build) {
                if (GameBase::getLOSInfo(%player,3)) {

                        %obj = getObjectType($los::object);
				%prot = GameBase::getRotation(%player);
				%zRot = getWord(%prot,2);
				if (Vector::dot($los::normal,"0 0 1") > 0.6) {
					%rot = "0 0 " @ %zRot;
				}
				else {
					if (Vector::dot($los::normal,"0 0 -1") > 0.6) {
						%rot = "3.14159 0 " @ %zRot;
					}
					else {
						%rot = Vector::getRotation($los::normal);
					}
				}
	                  %camera = newObject("Twig","StaticShape",TwigShape,true);
	   	      addToSet("MissionCleanup", %camera);
					GameBase::setTeam(%camera,GameBase::getTeam(%player));
					GameBase::setRotation(%camera,%rot);
					GameBase::setPosition(%camera,$los::position);
                              Gamebase::setMapName(%camera,"Mr Twig#"@ $totalNumTwig++ @ " " @ Client::getName(%client));
					if(!$build)Client::sendMessage(%client,0,"Mr Twig set");
					playSound(SoundPickupBackpack,$los::position);
                                        $missionItems++;
					$TeamItemCount[GameBase::getTeam(%camera) @ "Twig"]++;
                                        $twig = $twig +1;
			echo("MSG: ",Client::getName(%client)," CL# ",%client," deployed a Mr. Twig");
					return true;

                        }
                       else {
			GameBase::playSound(%player, usepack, 0);
			if (!$build) Client::sendMessage(%client,0,"Cannot deploy here.");
                        }

        }
        else
                 Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");

        return false;
}}

StaticShapeData TwigShape
{
        shapeFile = "bigTwig";//newdoor5,elevator_4x4,elevpad2,mainpad,logo
        debrisId = flashDebrisLarge;//defaultDebrisLarge
        maxDamage = 2.0;
        visibleToSensor = true;
        isTranslucent = true;
        description = "Mr Twig";
};




function TwigShape::onDestroyed(%this)
{
   StaticShape::onDestroyed(%this);
   $TeamItemCount[GameBase::getTeam(%this) @ "Twig"]--;$missionItems--;

}

function TwigShape::onCollision(%this,%obj){
	if(getObjectType(%obj)!="Player" || Player::isDead(%obj)) {
		return;
	}
	%c = Player::getClient(%obj);
	%playerTeam = GameBase::getTeam(%obj);
	%TwigTeam = GameBase::getTeam(%this);
		if(%TwigTeam != %playerTeam){
		%dlevel = GameBase::getDamageLevel(%obj) + 0.2;
		%data = GameBase::getDataName(%this);
		GameBase::setDamageLevel(%this, %data.maxDamage);
		GameBase::setDamageLevel(%obj, %dlevel); 
		Player::setDamageFlash(%obj,0.5);
	if(Player::isDead(%obj)){
		%name = Client::getName(%c); 
		Messageall(0,%name@" partys down with Mr. Twig"); 
 		playNextAnim(%obj); 
		//Player::blowUp(%obj); 
		//Player::kill(%obj); 
	}
		return;
		}
	TwigShape::openDoor(%this);
	client::sendmessage(%c, 0, "~wmale1.wwatchsh.wav"); 
	return;
	}


function TwigShape::openDoor(%this) {

	GameBase::startfadeout(%this);
	%pos=GameBase::getPosition(%this);
	%pos=Vector::add(%pos,"0 0 -2");
	GameBase::setPosition(%this,%pos);
	schedule("GameBase::playSound("@%this@",ForceFieldOpen,0);",0.05);
	schedule("TwigShape::closeDoor("@%this@");",1);
	}

function TwigShape::closeDoor(%this) {
	%pos=GameBase::getPosition(%this);
	%pos=Vector::add(%pos,"0 0 2");
	GameBase::setPosition(%this,%pos);
	GameBase::startfadein(%this);
	schedule("GameBase::playSound("@%this@",ForceFieldClose,0);",0.05);
	}

//----------------------------------------
//------------------------------------------


//should of been added by dynamix a LONG time ago
//unrelated to twig
function CargoBarrel::onDestroyed(%this)
{
schedule("deleteobject(" @ %this @ ");", 2);
}

function CargoCrate::onDestroyed(%this)
{
schedule("deleteobject(" @ %this @ ");", 2);
}   


///-----------------------------------------------

//Origional code by Plasmatic
//Please get ahold of me if you want to use this
//ziptiezmail@netscape.net

///-----------------------------------------------



$ItemMax[harmor, Girl] = 1;
$ItemMax[larmor, Girl] = 1;
$ItemMax[lfemale, Girl] = 1;
$ItemMax[marmor, Girl] = 1;
$ItemMax[mfemale, Girl] = 1;

///-----------------------------------------------

$TeamItemMax[Girl] = 1000;
additem(Girl);
ItemImageData GirlImage
{
        shapeFile = "lfemale";//newdoor5
        mountPoint = 2;
        mountOffset = { 0, -0.1, 0 };
        mountRotation = { -0.2, 0, 0 };
        mass = 1.5;
        firstPerson = false;
};

ItemData Girl
{
        description = "Girly Girl";
        shapeFile = "lfemale";//newdoor5
        className = "Backpack";
        heading = "jBarricades";//        heading = "dDeployables";
        imageType = GirlImage;
        shadowDetailMask = 4;
        mass = 1.5;
        elasticity = 0.2;
        price = 25;
        hudIcon = "deployable";
        showWeaponBar = true;
        hiliteOnActive = true;
};

function Girl::onUse(%player,%item)
{
        if (Player::getMountedItem(%player,$BackpackSlot) != %item)
        {
                Player::mountItem(%player,%item,$BackpackSlot);
        }
        else
        {
                Player::deployItem(%player,%item);
        }
}

function Girl::onDeploy(%player,%item,%pos)
{	
        if (Girl::deployShape(%player,%item)&& !$build)
        {
                Player::decItemCount(%player,%item);
        }
}
function Girl::deployShape(%player,%item)
{
         %client = Player::getClient(%player);
        if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]|| $build) {
                if (GameBase::getLOSInfo(%player,3)) {

                        %obj = getObjectType($los::object);
				%prot = GameBase::getRotation(%player);
				%zRot = getWord(%prot,2);
					%xpos = getWord($los::position,0);
					%ypos = getWord($los::position,1);
					%zpos = getWord($los::position,2);
				if (Vector::dot($los::normal,"0 0 1") > 0.6) {
					%rot = "0 0 " @ %zRot + 3.14159;%zpos += 1.1;//floor
				}
				else {
					if (Vector::dot($los::normal,"0 0 -1") > 0.6) {
						%rot = "3.14159 0 " @ %zRot;%zpos -= 1.1;//ceiling
					}
					else {
						%rot = Vector::getRotation($los::normal);//other
					}
				}

	                  %camera = newObject("Girl","StaticShape",GirlShape,true);
	   	      addToSet("MissionCleanup", %camera);
					GameBase::setTeam(%camera,GameBase::getTeam(%player));
					GameBase::setRotation(%camera,%rot);				
					GameBase::setPosition(%camera,%xpos@" "@%ypos@" "@%zpos);
                              Gamebase::setMapName(%camera,"Girl# "@ $totalNumGirl++ @ " " @ Client::getName(%client));
					if(!$build)Client::sendMessage(%client,0,"Girly set");
					playSound(SoundPickupBackpack,$los::position);
                                        $missionItems++;
					$TeamItemCount[GameBase::getTeam(%camera) @ "Girl"]++;$girl = $girl +1;
			echo("MSG: ",Client::getName(%client)," CL# ",%client," deployed a Girl");
					return true;
					}
                       else {
			GameBase::playSound(%player, usepack, 0);
			if (!$build) Client::sendMessage(%client,0,"Cannot deploy here.");
                        }

        }
        else
                 Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");

        return false;
}

StaticShapeData GirlShape
{
        shapeFile = "lfemale";//newdoor5,elevator_4x4,elevpad2,mainpad,logo,lfemale
        debrisId = defaultDebrisLarge;
        maxDamage = 20.0;
        visibleToSensor = true;
        isTranslucent = true;
        description = "Blow Up";
};




function GirlShape::onDestroyed(%this)
{
   StaticShape::onDestroyed(%this);
   $TeamItemCount[GameBase::getTeam(%this) @ "Girl"]--;$missionItems--;

}

function GirlShape::onCollision(%this,%obj){
	if(getObjectType(%obj)!="Player" || Player::isDead(%obj)) {
		return;
	}
	%c = Player::getClient(%obj);
	%playerTeam = GameBase::getTeam(%obj);
	%GirlTeam = GameBase::getTeam(%this);
		if(%GirlTeam != %playerTeam){
			return;
		}
	GirlShape::openDoor(%this);
	client::sendmessage(%c, 0, "~wfemale5.whello.wav");
	schedule("Client::sendMessage("@%c@", 0,\"~wfemale1.wtaunt10.wav\");", 0.8);
	return;
	}


function GirlShape::openDoor(%this) {

	GameBase::startfadeout(%this);
	%pos=GameBase::getPosition(%this);
	%pos=Vector::add(%pos,"0 0 4");
	GameBase::setPosition(%this,%pos);
	//schedule("GameBase::playSound("@%this@",ForceFieldOpen,0);",0.05);
	schedule("GirlShape::closeDoor("@%this@");",1);
	}

function GirlShape::closeDoor(%this) {
	%pos=GameBase::getPosition(%this);
	%pos=Vector::add(%pos,"0 0 -4");
	GameBase::setPosition(%this,%pos);
	GameBase::startfadein(%this);

	}


// BLAH BLAH BLAH BLAH!!!!!!


//--------------------------------------



//--------------------------------------

ItemImageData EnergyPackImage
{
	shapeFile = "jetPack";
	weaponType = 2;  // Sustained

	mountPoint = 2;
	mountOffset = { 0, -0.1, 0 };

	minEnergy = -3;
 	maxEnergy = -10;
	firstPerson = false;
};
additem(EnergyPack);
ItemData EnergyPack
{
	description = "Energy Pack";
	shapeFile = "jetPack";
	className = "Backpack";
   heading = "cBackpacks";
	shadowDetailMask = 4;
	imageType = EnergyPackImage;
	price = 150;
	hudIcon = "energypack";
	showWeaponBar = true;
	hiliteOnActive = true;
   validateShape = true;
   validateMaterials = false;
};

function EnergyPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
}

function EnergyPack::onMount(%player,%item)
{
	Player::trigger(%player,$BackpackSlot,true);
}

function EnergyPack::onUnmount(%player,%item)
{	if (Player::getMountedItem(%player,$WeaponSlot) == SoulSucker && !$build) {
		Client::sendMessage(Player::getClient(%player),0,"Must have an Energy Pack to use Soul Sucker. Auto selecting next weapon.~waccess_denied.wav");
		RemoteNextWeapon(Player::getClient(%player));
	}
}




//--------------------------------------



//--------------------------------------

//--------------------------------------

ItemImageData RepairGunImage
{
	shapeFile = "repairgun";
	mountPoint = 0;

	weaponType = 2;  // Sustained
	projectileType = RepairBolt;
	minEnergy  = 3;
	maxEnergy = 20;  // Energy used/sec for sustained weapons

	lightType   = 3;  // Weapon Fire
	lightRadius = 1;
	lightTime   = 1;
	lightColor  = { 0.25, 1, 0.25 };

	sfxActivate = SoundPickUpWeapon;
	sfxFire = SoundRepairItem;
};

ItemData RepairGun
{
	description = "Fixit Gun";
	shapeFile = "repairgun";
	className = "Weapon";
	shadowDetailMask = 4;
	imageType = RepairGunImage;
	showInventory = false;
	price = 125;
   validateShape = true;
};

function RepairGun::onMount(%player,$weaponslot)
{	bottomprint(Player::getClient(%player), "<f2> Fixit pack, Show a turret you really care.", 10);
	Player::trigger(%player,$BackpackSlot,true);

}







function RepairGun::onUnmount(%player,%imageSlot)
{
	Player::trigger(%player,$BackpackSlot,false);
}

//--------------------------------------

ItemImageData RepairPackImage
{
	shapeFile = "armorPack";
	mountPoint = 2;
	weaponType = 2;  // Sustained
   minEnergy = 0;
	maxEnergy = 0;   // Energy used/sec for sustained weapons
  	mountOffset = { 0, -0.05, 0 };
  	mountRotation = { 0, 0, 0 };
	firstPerson = false;
};
additem(RepairPack);
ItemData RepairPack
{
	description = "Fixit Pack";
	shapeFile = "armorPack";
	className = "Backpack";
   heading = "cBackpacks";
	shadowDetailMask = 4;
	imageType = RepairPackImage;
	price = 125;
	hudIcon = "repairpack";
	showWeaponBar = true;
	hiliteOnActive = true;
   validateShape = true;
   validateMaterials = false;
};

function RepairPack::onUnmount(%player,%item)
{
	if (Player::getMountedItem(%player,$WeaponSlot) == RepairGun) {
		Player::unmountItem(%player,$WeaponSlot);
	}
}

function RepairPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::mountItem(%player,RepairGun,$WeaponSlot);
	}
}

function RepairPack::onDrop(%player,%item)
{
	if($matchStarted) {
		%mounted = Player::getMountedItem(%player,$WeaponSlot);
		if (%mounted == RepairGun) {
			Player::unmountItem(%player,$WeaponSlot);
		}
		else {
			// Remount the existing weapon to make sure the RepairGun
			// is not on the delayed mount "stack".
			Player::mountItem(%player,%mounted,$WeaponSlot);
		}
		Item::onDrop(%player,%item);
	}
}
///-----------------------------------------------
//---------------------------
// Optical Rocket
//------------------------------
//
// Code by Plasmatic
// Please get ahold of me if you want to use this
// ziptiezmail@netscape.net
//
///-----------------------------------------------
//--------------------------------------



$AutoUse[OSLauncher] = True;
$SellAmmo[OSAmmo] = 10;
$WeaponAmmo[OSLauncher] = OSAmmo;

$ItemMax[larmor, OSLauncher] = 0;
$ItemMax[lfemale, OSLauncher] = 0;
$ItemMax[Marmor, OSLauncher] = 1;
$ItemMax[mfemale, OSLauncher] = 1;
$ItemMax[Harmor, OSLauncher] = 0;

$ItemMax[larmor, OSAmmo] = 0;
$ItemMax[lfemale, OSAmmo] = 0;
$ItemMax[marmor, OSAmmo] = 100;
$ItemMax[mfemale, OSAmmo] = 100;
$ItemMax[harmor, OSAmmo] = 0;
additem(OSAmmo);

ItemData OSAmmo 
{	description = "OS Rockets";
	className = "Ammo";
   heading = "xAmmunition";
	shapeFile = "rocket";
	shadowDetailMask = 4;
	price = 50;
};



ItemImageData OSLauncherImage
{	shapeFile = "grenadel";
	ammoType = OSAmmo;
	mountPoint = 0;
	mountRotation = { 0, 1.57, 0 };
	weaponType = 0; // Single Shot
	minEnergy = 32;
	maxEnergy = 32;
	accuFire = true;
	reloadTime = 0.5;
	fireTime = 2.5;
	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };
	sfxFire = SoundFireFlierRocket;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;
};

ItemData OSLauncher 
{	description = "OS Laucher";
	className = "Weapon";
	shapeFile = "grenadel";
	hudIcon = "fear";
	heading			= "bWeapons";
	shadowDetailMask = 4;
	imageType = OSLauncherImage;
	price = 5000;
	showWeaponBar = true;
};
additem(OSpack);
function OSLauncher::onMount(%player,%item,$WeaponSlot)
{	%client = Player::getclient(%player);
	bottomprint(%client, "<f2> Optical Rocket Launcher. Play space cowboy at your enemys expense.");
}

function OSLauncherImage::onFire(%this)
{	%client = Player::getClient(%this);
	%weapon = Player::getMountedItem(%client,$WeaponSlot);
	%trans = GameBase::getMuzzleTransform(%client);
	if(!%player.vehicle)
	{	
		%rot = GameBase::getRotation(%client);
		%posX = getWord(%trans,9);
		%posY = getWord(%trans,10);
		%posZ = getWord(%trans,11) + 1.5; // +3.0
		%position = %posX@" "@%posY@" "@%posZ;
		%obj = newObject("Optical Seeking Missile",flier,OSMissile,true);
		%obj.OpOwner = %this;
		addToSet("MissionCleanup",%obj);
		Gamebase::setMapName(%obj,"Optical Seeking Missile");
		GameBase::setTeam(%obj,GameBase::getTeam(%this));
		GameBase::setPosition(%obj,%position);
		GameBase::setRotation(%obj,%rot);
		GameBase::startFadeIn(%obj);

		%player = Client::getOwnedObject(%client);
	 	//Client::setOwnedObject(%client, %vehicle);
		
		Client::setOwnedObject(%client, %obj);
		Client::setOwnedObject(%client, %player);
				
		Client::setControlObject(%client,%obj);
		
		%this.vehicle = %obj;
		%this.lastWeapon = %weapon;
		Player::unMountItem(%this,$WeaponSlot);
	}
	Player::decItemCount(%client,$WeaponAmmo[OSLauncher],1);
}

ItemImageData OSPackImage
{
	shapeFile = "sensor_small";//armorPack,discb,jetPack,plant2
	mountPoint = 2;
	weaponType = 2;  // Sustained
   minEnergy = 0;
	maxEnergy = 0;   // Energy used/sec for sustained weapons
  	mountOffset = { 0, -0.1, 0.2 };
  	mountRotation = { 1.0, -3.14, 0};
	firstPerson = false;
};

ItemData OSPack
{
	description = "Optical Rocket Pack";
	shapeFile = "sensor_small";//armorPack,plant2,diskb,discb,jetPack
	className = "Backpack";
   heading = "cBackpacks";
	shadowDetailMask = 4;
	imageType = OSPackImage;
	price = 125;
	hudIcon = "repairpack";
	showWeaponBar = true;
	hiliteOnActive = true;
   validateShape = true;
   validateMaterials = false;
};



function OSPack::onMount(%player,%item){
		%client = Player::getClient(%player);
		bottomprint(%client, "<f2> Optical Rocket Launcher. Play space cowboy at your enemys expense.", 10);
		%AmmoCount = (Player::getItemCount(%player, $WeaponAmmo[OSLauncher]) && Player::getItemCount(%player,OSAmmo)) ;
		if(%AmmoCount == 0 && Client::isItemShoppingOn(%client,%item)){	

		buyItem(%client,"OSAmmo");
		Client::sendMessage(%client,0,"Auto buying Optical Rockets");
		}
	}

function OSPack::onUnmount(%player,%item)
{
	if (Player::getMountedItem(%player,$WeaponSlot) == OSLauncher) {
		Player::unmountItem(%player,$WeaponSlot);
	}
}

function OSPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
	%AmmoCount = (Player::getItemCount(%player, $WeaponAmmo[OSLauncher]) && Player::getItemCount(%player,OSAmmo)) ;
	if(%AmmoCount){
		%mounted = Player::getMountedItem(%player,$WeaponSlot);
		%player.prevweapon = %mounted;
		Player::mountItem(%player,OSLauncher,$WeaponSlot);
			}
		else
		Client::sendMessage(Player::getClient(%player),0,"No Optical Rockets to fire. ~waccess_denied.wav");
	}
}

function OSPack::onDrop(%player,%item)
{
	if($matchStarted) {
		%mounted = Player::getMountedItem(%player,$WeaponSlot);
		if (%mounted == OSLauncher) {
			Player::unmountItem(%player,$WeaponSlot);
		}
		else {
			// Remount the existing weapon to make sure the pack
			// is not on the delayed mount "stack".
			Player::mountItem(%player,%mounted,$WeaponSlot);
		}
		Item::onDrop(%player,%item);
	}
}


MineData OpticalSmoke
{
	className = "Mine";
   	description = "smoke";
   	shapeFile = "smoke";
   	shadowDetailMask = 4;
   	explosionId = smokeExp;
	explosionRadius = 0.1;
	damageValue = 0;
	kickBackStrength = 0;
	triggerRadius = 0;
	maxDamage = 0.5;
	collideWithOwner   = False;
	shadowDetailMask = 0;
	destroyDamage = 1.0;
	damageLevel = {1.0, 1.0};
};
function OpticalSmoke::onAdd(%this) {schedule("Mine::Detonate(" @ %this @ ");",0.2,%this); }

FlierData OSMissile
{	explosionId = WickedBadExp;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
	shapeFile = "rocket";
	shieldShapeName = "shield_medium";
	mass = 7.5; //0.1
	drag = 1.0;
	density = 1.2;
	maxBank = 10.5;
	maxPitch = 11.15;
	maxSpeed = 100; //60
	minSpeed = 40;
	lift = 0; //.85
	maxAlt = 20000;
	minAlt = -1000;
	maxVertical = 10; //200
	maxDamage = 0.001;
	damageLevel = {1.0, 1.0};
	maxEnergy = 60; // time in seconds until explodes
	accel = 60; //1.0
	groundDamageScale = 20.0;
	repairRate = 0;
	damageSound = shockExplosion;
	ramDamage = 2.5;
	ramDamageType = $ImpactDamageType;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	visibleToSensor = true;
	shadowDetailMask = 2;
	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundJetHeavy;
	moveSound = SoundJetHeavy;
	visibleDriver = false;
	driverPose = 22;

	trailType = 2; 
	trailString = "smoke.dts"; //rsmoke.dts
	smokeDist = 2.1; 
 //  trailType   = 1;
 //  trailLength = 25;
 //  trailWidth  = 0.3;
	description = "Optical Seeking Missile";
	//because this is a flier, it can shoot projectiles
	//This things allready too tough...
	//projectileType = MortarShell;
	//reloadDelay = 1;
	//fireSound = SoundFireMortar;
};

function OSMissile::onAdd(%this)
{	GameBase::setRechargeRate (%this,0);
	schedule("OSMissile::exhaustFuel("@%this@");",1,%this);
}

function KillRocket (%this){
		%Pos = GameBase::getPosition(%this); 
   		%vel = Item::getVelocity(%this);

	%trans =  "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%this);
 	%obj = Projectile::spawnProjectile("RocketKilla", %trans, %this.OpOwner, %vel);
	Projectile::spawnProjectile(%obj);
	GameBase::setPosition(%obj, %pos);
}


function OSMissile::onCollision(%this,%object)
{	
KillRocket (%this);
}

function OSMissile::jump(%this,%mom)
{	
	KillRocket (%this);
}


function OSMissile::onDestroyed (%this,%mom)
{
	KillRocket (%this);

	%cl = GameBase::getControlClient(%this);
	%pl = Client::getOwnedObject(%cl);
//echo("%pl :",%pl," %this.OpOwner :",%this.OpOwner);
%wep = %this.OpOwner;
	if(%pl != -1) 
	{	
		Player::setMountObject(%pl, -1, 0);
		Client::setControlObject(%cl, %pl);
	}
		if(%wep.lastWeapon != "") 
		{	
			Player::mountItem(%wep,%wep.lastWeapon,$WeaponSlot);		 	
			%wep.lastWeapon = "";
		}
		%wep.driver = "";
		%wep.vehicle= "";
}


function OSMissile::exhaustFuel(%this)
{	%fuel = GameBase::getEnergy(%this);
	if(%fuel < 1) KillRocket (%this);
	else 
	{
		GameBase::setEnergy(%this,%fuel - 1);
		schedule("OSMissile::exhaustFuel("@%this@");",0.5,%this);

		%Pos = GameBase::getPosition(%this); 
   		%vel = Item::getVelocity(%this);
		%smoke = newObject("","Mine","OpticalSmoke");
 		addToSet("MissionCleanup", %smoke);
      	GameBase::throw(%smoke,%this.OpOwner,-1,true);
		GameBase::setPosition(%smoke, %pos);
	}
}


//OpticalSmoke


//function Vehicle::getHeatFactor(%this) 
//{  return 1.0;
//}

$DamageScale[OSMissile, $ImpactDamageType] = 1.0;
$DamageScale[OSMissile, $BulletDamageType] = 1.0;
$DamageScale[OSMissile, $PlasmaDamageType] = 1.0;
$DamageScale[OSMissile, $EnergyDamageType] = 1.0;
$DamageScale[OSMissile, $ExplosionDamageType] = 1.0;
$DamageScale[OSMissile, $ShrapnelDamageType] = 1.0;
$DamageScale[OSMissile, $DebrisDamageType] = 1.0;
$DamageScale[OSMissile, $MissileDamageType] = 1.0;
$DamageScale[OSMissile, $LaserDamageType] = 0.5;
$DamageScale[OSMissile, $MortarDamageType] = 1.0;
$DamageScale[OSMissile, $BlasterDamageType] = 0.5;
$DamageScale[OSMissile, $ElectricityDamageType] = 1.0;
$DamageScale[OSMissile, $MineDamageType] = 1.0;


//--------------------------------------


//-------------------------------------------------------------
// Hell Pack
// Coded by Plasmatic
// Plasmaca.cjb.net
// Ziptiezmail@netscape.net
// Please contact me if you would like to use this.
//--------------------------------------------------


///-----------------------------------------------
$ItemMax[harmor, HellPack] = 0;
$ItemMax[larmor, HellPack] = 1;
$ItemMax[lfemale, HellPack] = 1;
$ItemMax[marmor, HellPack] = 0;
$ItemMax[mfemale, HellPack] = 0;
$InvList[HellPack] = 1;
additem(HellPack);
$RemoteInvList[HellPack] = 1;
///-----------------------------------------------

$TeamItemMax[HellPack] = 200;

ItemImageData HellPackImage
{
        shapeFile = "grenadeL";//grenadeL
        mountPoint = 2;
        //mountOffset = { -0.375, 0, 0 };
        mountRotation = {1.57,0, 3.14 };
        mass = 2.5;
        firstPerson = false;
	lightType = 3;   // Pulsing 2 pulsing, 3 fire, 1 continuous
	lightRadius = 2;
//	lightTime = 0.1;
	lightColor = { 0.3, 0.4, 0.5};
};

ItemImageData HellPack2Image 
{	  
        shapeFile = "discb";//grenadeL
        mountPoint = 2;
       mountOffset = { 0 , -0.1, 0.05 };
        mountRotation = {1.57,0, 3.14 };
	  weaponType = 0;
	lightType = 1;   // Pulsing, 2 pulsing, 3 fire, 1 continuous
	lightRadius = 2;
//	lightTime = 0.1;
	lightColor = { 0.3, 0.4, 0.5};

 }; 


ItemData HellPack
{
        description = "Hell Pack";
        shapeFile = "grenadeL";//grenadeL
        className = "Backpack";
        heading = "cBackpacks";
        imageType = HellPackImage;
        shadowDetailMask = 4;
        mass = 2.5;
        elasticity = 0.2;
        price = 500;
        hudIcon = "deployable";
        showWeaponBar = true;
        hiliteOnActive = true;
};


ItemData Hell2Pack
{
        description = "Hell Pack";
        shapeFile = "discb";//grenadeL
        className = "Backpack"; 
        heading = "cBackpacks";
        imageType = HellPack2Image;
        shadowDetailMask = 4;
        mass = 2.5;
        elasticity = 0.2;
        price = 500;
        hudIcon = "deployable";
        showWeaponBar = true;
        hiliteOnActive = true;
};

function HellPack::onUse(%player,%item)
{
        if (Player::getMountedItem(%player,$BackpackSlot) != %item)
        {
	Player::mountItem(%player,%item,$BackpackSlot);
	bottomprint(Player::getClient(%player), "<f2> Hell Fire Pack, rain death from above.", 10);
        }
        else
        {
		if (!%player.hellfire)  {
			%player.hellfire = true;
			Client::sendMessage(Player::getClient(%player), 1, "Hell Fire Pack on. Generating Disc's.");
			%c = Player::getClient(%player);
			gamebase::setautorepairrate(client::getownedobject(%c), 0.04);
			%player.repairRate = 0.04;
			StartHellFire(%player,%this);
			}
		else {
		%player.hellfire = false;
		%c = Player::getClient(%player);
		gamebase::setautorepairrate(client::getownedobject(%c), 0.02);
		%player.repairRate = 0.02;
		Client::sendMessage(Player::getClient(%player), 1, "Hell Fire Pack off.");
		}
        }
}
function HellPack::onMount(%player, %slot) {	
	%c = Player::getClient(%player);
	gamebase::setautorepairrate(client::getownedobject(%c), 0.02);
	%player.repairRate = 0.02;
	Player::mountItem(%player, Hell2Pack, 7); 
}


function HellPack::onUnmount(%player, %slot) { 
	%c = Player::getClient(%player);
	gamebase::setautorepairrate(client::getownedobject(%c), 0);
	%player.repairRate = 0;
	Player::unmountItem(%player, 7); 
	%player.hellfire = false;

}



function Firegrenade::onAdd(%this)
{
	%data = GameBase::getDataName(%this);
	schedule("Mine::Detonate(" @ %this @ ");",4.0,%this);
}
function Firegrenade::onCollision(%this,%object) 
{  
if (GameBase::getTeam(%this) != GameBase::getTeam(%object))
	GameBase::setDamageLevel(%this, 2); //detonate Firegrenade on contact
}

function StartHellFire(%player,%this){ 
	if (Player::getMountedItem(%player,$backpackslot)!="HellPack"){
		%player.hellfire = false;
		Client::sendMessage(Player::getClient(%player), 1, "Hell Fire Pack off.");
	}
	if (%player.hellfire) {

		%rnd = floor(getRandom() * 10);
		%obj = newObject("","Mine","Firegrenade");
		GameBase::SetTeam(%obj,GameBase::getTeam(%player));
 		addToSet("MissionCleanup", %obj);
      	GameBase::throw(%obj,%player,-%rnd-1,false);

	  gamebase::setautorepairrate(client::getownedobject(%c), 0.04);
	  %player.repairRate = 0.04;
  if (!Player::isDead(%player)) schedule("StartHellFire(" @ %player @ ");", 0.2);

	}
}
//----------------------------------------------------

//-------------------------------------------------------------
// Ghost Pack
// Coded by Plasmatic
// Plasmaca.cjb.net
// Ziptiezmail@netscape.net
// Please contact me if you would like to use this.
//--------------------------------------------------



$InvList[GhostPack] = 1;
$MobileInvList[GhostPack] = 1;
$RemoteInvList[GhostPack] = 0;
additem(ghostpack);
//-----------------------------------

$ItemMax[larmor, GhostPack] = 1;
$ItemMax[lfemale, GhostPack] = 1;
$ItemMax[Marmor, GhostPack] = 0;
$ItemMax[mfemale, GhostPack] = 0;
$ItemMax[Harmor, GhostPack] = 0;

ItemData GhostArmor
{
   heading = "aArmor";
	description = "Ghosting";
	className = "Armor";
	price = 175;
showInventory = false;		//armor won't show in any inv, including when you're wearing it.
};


ItemImageData GhostPackImage
{
        shapeFile = "grenadeL";//grenadeL
        mountPoint = 2;
        //mountOffset = { -0.375, 0, 0 };
        mountRotation = {1.57,0, 3.14 };
        mass = 2.5;
        firstPerson = false;
	lightType = 3;   // Pulsing 2 pulsing, 3 fire, 1 continuous
	lightRadius = 5;
//	lightTime = 0.1;
	lightColor = { 0.3, 0.4, 0.5};
};

ItemImageData GhostPack2Image 
{	  
        shapeFile = "mine";//grenadeL,plasmaex
        mountPoint = 2;
       mountOffset = { 0 , -0.3, 0.2 };
        mountRotation = {-1.57,0, 0 };//{1.57,0, 3.14 }
	  weaponType = 0;
	lightType = 1;   // Pulsing, 2 pulsing, 3 fire, 1 continuous
	lightRadius = 5;
//	lightTime = 0.1;
	lightColor = { 0.3, 0.4, 0.5};

 }; 


ItemData GhostPack
{
        description = "Ghost Pack";
        shapeFile = "grenadeL";//grenadeL
        className = "Backpack";
        heading = "cBackpacks";
        imageType = GhostPackImage;
        shadowDetailMask = 4;
        mass = 2.5;
        elasticity = 0.2;
        price = 500;
        hudIcon = "deployable";
        showWeaponBar = true;
        hiliteOnActive = true;
};


ItemData GhostPack2
{
        description = "gpack";
        shapeFile = "discb";//grenadeL
        className = "Backpack"; 
        heading = "cBackpacks";
        imageType = GhostPack2Image;
        shadowDetailMask = 4;
        mass = 2.5;
        elasticity = 0.2;
        price = 500;
        hudIcon = "deployable";
        showWeaponBar = true;
        hiliteOnActive = true;
};


ItemImageData GhostPack3Image
{	  
        shapeFile = "grenade";//grenadeL,shield,discb
        mountPoint = 2;
       mountOffset = { 0 , 0.05, 0.15 };
        mountRotation = {1.57,0, 0 };
	  weaponType = 0;
	lightType = 1;   // Pulsing, 2 pulsing, 3 fire, 1 continuous
	lightRadius = 5;
//	lightTime = 0.1;
	lightColor = { 0.3, 0.4, 0.5};

 };


ItemData GhostPack3
{
	description = "Ghost Pack";
	shapeFile = "grenade";
	className = "Backpack";
   heading = "cBackpacks";
	shadowDetailMask = 4;
	imageType = GhostPack3Image;
	price = 1500;
	hudIcon = "energypack";
	showWeaponBar = true;
	hiliteOnActive = true;

};


function GhostPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
}

function GhostPack::onMount(%player,%item)
{
	//%player.shieldStrength = -0.052;
	Player::mountItem(%player, GhostPack2, 7);
	gamebase::setautorepairrate(%player, 0.01);
	bottomprint(Player::getClient(%player), "<f2> Ghost Pack, Shadow shift through walls.", 10);
}

function noghost(%player){
	gamebase::setautorepairrate(%player, 0.01);
	Player::unmountItem(%player, 6);
	%client = GameBase::getOwnerClient(%player);
	%player.Ghoster = false;
	Player::trigger(%player,$BackpackSlot,false);
	%c = Player::getClient(%player);
	gamebase::setautorepairrate(%player, 0.01);
	%player.repairRate = 0.00;
	Client::sendMessage(Player::getClient(%player), 1, "Ghost Pack off. Rephasing entities.");
	%armor = Player::getArmor(%client);
	%buyarmor = $ArmorType[Client::getGender(%client), LightArmor];
   	%energy = GameBase::getEnergy(%player);
	Player::setArmor(%client,%buyarmor);
   	GameBase::setEnergy(%player,%energy);
     	Player::setItemCount(%client, $ArmorName[%armor], 0);  
     	Player::setItemCount(%client, LightArmor, 1); 
		}

//function GhostPack::onUnmount(%player,%item)
//{	
//	noghost(%player);
//	Player::unmountItem(%player, 7);
//	gamebase::setautorepairrate(%player, 0.0);
//}
function GhostPack::onUnmount(%player,%item)
{	
	if (Player::getItemCount(%player, GhostArmor) == 1) noghost(%player);
	Player::unmountItem(%player, 7);
	gamebase::setautorepairrate(%player, 0.0);
}
$DropFlagChance = 15;


function GhostPack::onUse(%player,%item)
{
	%client = GameBase::getOwnerClient(%player);

	if (Player::getMountedItem(%player,$BackpackSlot) != %item)
	{
	Player::mountItem(%player,%item,$BackpackSlot);
	gamebase::setautorepairrate(%player, 0.01);
	bottomprint(Player::getClient(%player), "<f2> Ghost Pack, Shadow shift through walls.", 10);
	}
	else
	{
	if (!%player.ghoster)  {
		%player.ghoster = true;
		Player::mountItem(%player, GhostPack3, 6);
		Player::trigger(%player,$BackpackSlot,true);
		Casper( %player);
		Client::sendMessage(Player::getClient(%player), 1, "Ghost Pack on. Collapsing shadows.");
		%c = Player::getClient(%player);
		gamebase::setautorepairrate(%player, 0.0);
		%player.repairRate = -0.04;
		%armor = Player::getArmor(%client);
		%buyarmor = $ArmorType[Client::getGender(%client), GhostArmor];
		%energy = GameBase::getEnergy(%player);
		Player::setArmor(%client,%buyarmor);
		GameBase::setEnergy(%player,%energy);
		Player::setItemCount(%client, $ArmorName[%armor], 0);  
		Player::setItemCount(%client, GhostArmor, 1);
		}
		else NoGhost(%player);
        }
}


function Casper (%player){
	if (%player.ghoster)  {
		GameBase::playSound(%player, SoundActivateMotionSensor, 0);

		%svec = Vector::getFromRot(GameBase::getRotation(Player::getClient(%player)),10,0);
		GameBase::activateShield(%player,%svec, 0.75);


		Player::setDamageFlash(%player,0.2);
		%c = Player::getClient(%player);

	if(Player::getMountedItem(%player, 2) == "flag")
	{
		if(floor(getrandom() * 100) < $DropFlagChance)
		{
			Player::dropItem(%player,Player::getMountedItem(%player, 2));
			Client::sendMessage(Player::getClient(%player),1,"You dropped the flag while shadow shifted...~wError_Message.wav");
		}
	}

		%name = Client::getName(%c);
            %dlevel = GameBase::getDamageLevel(%player) + 0.05;
		if (!$noplayerdamage)GameBase::setDamageLevel(%player,%dlevel);
	if(Player::isDead(%player)){
 
		Messageall(0,%name@" became a dead shadow."); 
 		playNextAnim(%player); 
	%player.Ghoster = false;
		Player::blowUp(%obj); 
		Player::kill(%obj); 
	}
	schedule("Casper(" @ %player @  ");", 0.75);
}
}

//-----------------------


//--------------------------------------
additem(ShieldPack);
ItemImageData ShieldPackImage
{
	shapeFile = "shieldPack";
	mountPoint = 2;
	weaponType = 2;  // Sustained
	minEnergy = 4;
	maxEnergy = 9;   // Energy/sec for sustained weapons
	sfxFire = SoundShieldOn;
	firstPerson = false;
};

ItemData ShieldPack
{
	description = "Shield Pack";
	shapeFile = "shieldPack";
	className = "Backpack";
   heading = "cBackpacks";
	shadowDetailMask = 4;
	imageType = ShieldPackImage;
	price = 175;
	hudIcon = "shieldpack";
	showWeaponBar = true;
	hiliteOnActive = true;
   validateShape = true;
   validateMaterials = false;
};

function ShieldPackImage::onActivate(%player,%imageSlot)
{
	Client::sendMessage(Player::getClient(%player),0,"Shield On");
	bottomprint(Player::getClient(%player), "<f2> Shield pack on, now get busy!", 10);
	%player.shieldStrength = 0.052;//0.012
}

function ShieldPackImage::onDeactivate(%player,%imageSlot)
{
	Client::sendMessage(Player::getClient(%player),0,"Shield Off");
	bottomprint(Player::getClient(%player), "<f2> Shield pack off, You feeling lucky PUNK?", 10);
	Player::trigger(%player,$BackpackSlot,false);
	%player.shieldStrength = 0;
}

///-----------------------------------------------
//
// Code by Plasmatic
// Please get ahold of me if you want to use this
// ziptiezmail@netscape.net
//
///-----------------------------------------------
//--------------------------------------


$ItemMax[larmor, JackPack] = 0;
$ItemMax[lfemale, JackPack] = 0;
$ItemMax[Marmor, JackPack] = 1;
$ItemMax[mfemale, JackPack] = 1;
$ItemMax[Harmor, JackPack] = 1;

additem(JackPack);
//--------------------------------------



//--------------------------------------

ItemImageData VolcanoImage { 
	shapeFile = "GrenadeL"; 
	mountPoint = 0; 
	weaponType = 0; 
	reloadTime = 0.05; 
	fireTime = 0; 
	minEnergy = 5;//5 
	maxEnergy = 6; //6
	projectileType = VolcanoBolt; 
	accuFire = true; 
	sfxFire = SoundJetHeavy; 
	sfxActivate = SoundPickUpWeapon;
	 }; 

ItemData Volcano { 
	heading = "bWeapons"; 
	description = "Jacker"; 
	className = "Weapon"; 
	shapeFile = "GrenadeL"; 
	hudIcon = "plasma"; 
	shadowDetailMask = 4; 
	imageType = VolcanoImage; 
	price = 385; 
	showWeaponBar = true; 
	};
ItemImageData VolcanoImage1 { 
	shapeFile = "GrenadeL"; 
	mountPoint = 0; 
	weaponType = 0; 
	reloadTime = 0.05; 
	fireTime = 0; 
	minEnergy = 2;//5 
	maxEnergy = 1; //6
	projectileType = VolcanoBolt; 
	accuFire = true; 
	sfxFire = SoundJetHeavy; 
	sfxActivate = SoundPickUpWeapon;
	 }; 

ItemData Volcano1 { 
	heading = "bWeapons"; 
	description = "Jacker"; 
	className = "Weapon"; 
	shapeFile = "GrenadeL"; 
	hudIcon = "plasma"; 
	shadowDetailMask = 4; 
	imageType = VolcanoImage1; 
	price = 385; 
	showWeaponBar = true; 
	};

function Volcano::onMount(%player,%imageSlot)
{	bottomprint(Player::getClient(%player), "<f2> Volcano pack, do your Mt. Saint Helens impersonation.", 10);
	 Player::trigger(%player,$BackpackSlot,true);
}

function Volcano::onUnmount(%player,%imageSlot)
{
	 Player::trigger(%player,$BackpackSlot,false);
}
function Volcano1::onMount(%player,%imageSlot)
{	bottomprint(Player::getClient(%player), "<f2> Volcano pack, do your Mt. Saint Helens impersonation.", 10);
	 Player::trigger(%player,$BackpackSlot,true);
}

function Volcano1::onUnmount(%player,%imageSlot)
{
	 Player::trigger(%player,$BackpackSlot,false);
}

//--------------------------------------

ItemImageData JackPackImage
{
	shapeFile = "mine";//armorPack
	mountPoint = 2;
	weaponType = 2;  // Sustained
   minEnergy = -3;
	maxEnergy = -10;   // Energy used/sec for sustained weapons
  	mountOffset = { 0, 0.3, 0.20 };
  	mountRotation = { -1.57, -3.14, 0};
	firstPerson = false;
};

ItemData JackPack
{
	description = "Volcano pack";
	shapeFile = "mine";//armorPack
	className = "Backpack";
   heading = "cBackpacks";
	shadowDetailMask = 4;
	imageType = JackPackImage;
	price = 125;
	hudIcon = "repairpack";
	showWeaponBar = true;
	hiliteOnActive = true;
   validateShape = true;
   validateMaterials = false;
};

function JackPack::onUnmount(%player,%item)
{
%weap = Player::getMountedItem(%player,$WeaponSlot);
	if (%weap == Volcano || %weap == Volcano1) {
		Player::unmountItem(%player,$WeaponSlot);
	}
}

function VolcanoCheck(%player){
	%weap = Player::getMountedItem(%player,$WeaponSlot);
		if (%weap == Volcano1){
			 if (CountPlayerWeapons(%player) != "" )Player::mountItem(%player,Volcano,$WeaponSlot);
		schedule("VolcanoCheck(" @ %player @ ");", 1);
			}

}


function JackPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		if (CountPlayerWeapons(%player) >= 1)Player::mountItem(%player,Volcano,$WeaponSlot);
		else {	Player::mountItem(%player,Volcano1,$WeaponSlot);
				schedule("VolcanoCheck(" @ %player @ ");", 0.2);

		}
	}
}

function JackPack::onDrop(%player,%item)
{
	if($matchStarted) {
		%mounted = Player::getMountedItem(%player,$WeaponSlot);
		if (%mounted == Volcano || %mounted == Volcano1) {
			Player::unmountItem(%player,$WeaponSlot);
		}
		else {
			// Remount the existing weapon to make sure the Volcano
			// is not on the delayed mount "stack".
			Player::mountItem(%player,%mounted,$WeaponSlot);
		}
		Item::onDrop(%player,%item);
	}
}
//------------------------------------




///-----------------------------------------------
//
// Code by Plasmatic
// Please get ahold of me if you want to use this
// ziptiezmail@netscape.net
//
///-----------------------------------------------
//--------------------------------------
//--------------------------------------



$ItemMax[larmor, JumperPack] = 1;
$ItemMax[lfemale, JumperPack] = 1;
$ItemMax[Marmor, JumperPack] = 0;
$ItemMax[mfemale, JumperPack] = 0;
$ItemMax[Harmor, JumperPack] = 0;
additem(JumperPack);


$SellAmmo[JumpAmmo] = 5;
$AmmoPackMax[JumpAmmo] = 20;
$AmmoPackItems[11] = JumpAmmo;
$WeaponAmmo[JumpGun] = JumpAmmo;

$ItemMax[larmor, JumpAmmo] = 20;
$ItemMax[lfemale, JumpAmmo] = 20;
$ItemMax[marmor, JumpAmmo] = 0;
$ItemMax[mfemale, JumpAmmo] = 0;
$ItemMax[harmor, JumpAmmo] = 0;
additem(JumpAmmo);
//--------------------------------------
ItemData JumpAmmo
{
	description = "Jump Charge";
	className = "Ammo";
	shapeFile = "endarrow";//mortarammo,endarrow
   heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 2;
};




//--------------------------------------

ItemImageData JumpGunImage { 
	shapeFile = "rocket"; 
	mountPoint = 0; 
	weaponType = 0; 
	reloadTime = 0; 
	fireTime = 0.5; minEnergy = 5; 
	maxEnergy = 6; 
	//projectileType = JumpGunBolt; 
	accuFire = true; 
	ammoType		= JumpAmmo;
	sfxFire = SoundJetHeavy; 
	sfxActivate = SoundPickUpWeapon;
	//mountOffset = {0.2,-1.2,-1};//{0.065, 0.1, -0.1}
	//mountRotation = {0, -1.57, 0}; //-1.57
	 }; 

ItemData JumpGun { 
	heading = "bWeapons"; 
	description = "Jacker"; 
	className = "Weapon"; 
	shapeFile = "GrenadeL"; 
	hudIcon = "plasma"; 
	shadowDetailMask = 4; 
	imageType = JumpGunImage; 
	price = 385; 
	showWeaponBar = true; 
	};

function JumpGun::onMount(%player,%imageSlot)
{	bottomprint(Player::getClient(%player), "<f2> Jumper pack. Get there in a hurry!", 10);
	Player::trigger(%player,$BackpackSlot,true);
}

function JumpGun::onUnmount(%player,%imageSlot)
{
	Player::trigger(%player,$BackpackSlot,false);
}

//--------------------------------------

ItemImageData JumperPackImage
{
	shapeFile = "jetPack";//armorPack,discb
	mountPoint = 2;
	weaponType = 2;  // Sustained
   minEnergy = 0;
	maxEnergy = 0;   // Energy used/sec for sustained weapons
  	mountOffset = { 0, -0.1, 0 };
  	mountRotation = { -2.57, -3.14, 0};
	firstPerson = false;
};

ItemData JumperPack
{
	description = "Jumper pack";
	shapeFile = "jetPack";//armorPack,plant2,diskb,discb
	className = "Backpack";
   heading = "cBackpacks";
	shadowDetailMask = 4;
	imageType = JumperPackImage;
	price = 125;
	hudIcon = "repairpack";
	showWeaponBar = true;
	hiliteOnActive = true;
   validateShape = true;
   validateMaterials = false;
};

function JumperPack::onMount(%player,%item){
		%client = Player::getClient(%player);
		bottomprint(%client, "<f2> Jumper pack. Get there in a hurry!", 10);
		%AmmoCount = (Player::getItemCount(%player,JumpAmmo)) ;
		if(%AmmoCount == 0 && Client::isItemShoppingOn(%client,%item)){	

		buyItem(%client,"JumpAmmo");
		Client::sendMessage(%client,0,"Auto buying Jump Charges");
		}
	}

function JumperPack::onUnmount(%player,%item)
{
	if (Player::getMountedItem(%player,$WeaponSlot) == JumpGun) {
		Player::unmountItem(%player,$WeaponSlot);
	}
}

function JumperPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
	%AmmoCount = (Player::getItemCount(%player,JumpAmmo)) ;
	if(%AmmoCount){

		Player::mountItem(%player,JumpGun,$WeaponSlot);
			}
		else
		Client::sendMessage(Player::getClient(%player),0,"No Jump charges detected. ~waccess_denied.wav");
	}
}

function JumperPack::onDrop(%player,%item)
{
	if($matchStarted) {
		%mounted = Player::getMountedItem(%player,$WeaponSlot);
		if (%mounted == JumpGun) {
			Player::unmountItem(%player,$WeaponSlot);
		}
		else {
			// Remount the existing weapon to make sure the JumpGun
			// is not on the delayed mount "stack".
			Player::mountItem(%player,%mounted,$WeaponSlot);
		}
		Item::onDrop(%player,%item);
	}
}

//--------------------------------------
function JumpGunImage::onFire(%player,%slot)
{	
	%client = GameBase::getOwnerClient(%player);
	%AmmoCount = (Player::getItemCount(%player,JumpAmmo)) ;
	if(%AmmoCount){
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);
		if(GameBase::getLOSInfo(%player,1000))
		{	%object = getObjectType($los::object);
			if (%object!="Player" && %object !="Flier" && %object !="") 
{
	//Is target near a flag?
	//-----------
 		%set = newObject("set",SimSet);
		%num = containerBoxFillSet(%set,$ItemObjectType,$los::position,60,60,50,0);
		%num2 = CountObjects(%set,"flag",%num);
		%totalnum = Group::objectCount(%set);
		%enemyflag=0;
		for(%i = 0; %i < %totalnum; %i++)
		{
			%obj = Group::getObject(%set, %i);
			%name = Item::getItemData(%obj);
			//client::sendMessage(Player::getClient(%player),1,"Itemdata: "@ %i @ " " @ %name);
			if(%name == "flag" && (GameBase::getTeam(%obj) != Gamebase::getTeam(%player)))
			{     echo(Client::getName(GameBase::getOwnerClient(%player))@" "@Player::getClient(%player)@" "@%player@" Trying to Jump near flag...");
				Client::sendMessage(Player::getClient(%player),1,"Phase instability at target. ~wError_Message.wav");%enemyflag++;

			}
		}
		deleteObject(%set);
	
if (%enemyflag != 0) return;
//------------------------
			if(!$JumpUsed[%player]) 
			{
	GameBase::startFadeout(%player); 
				//--------------------
				// hack so players dont 
				// end up in walls
                        %obj = getObjectType($los::object);
				%prot = GameBase::getRotation(%player);
				%zRot = getWord(%prot,2);
					%xpos = getWord($los::position,0);
					%ypos = getWord($los::position,1);
					%zpos = getWord($los::position,2);
					
					//echo($los::normal);
				if (Vector::dot($los::normal,"0 0 1") > 0.6) {
					%zpos += 1;//floor
				}
				else {
					if (Vector::dot($los::normal,"0 0 -1") > 0.6) {
						%zpos -= 3;//ceiling
					}
				else if (Vector::dot($los::normal,"0 0 -1") >= -0.1 || Vector::dot($los::normal,"0 0 -1") <= 0.1 ) {
						//wall

					%xopos = getWord($los::normal,0);
					%yopos = getWord($los::normal,1);
					%xpos = %xpos + %xopos + %xopos;
					%ypos = %ypos + %yopos + %yopos;

					}

				else {
						%rot = Vector::getRotation($los::normal);//other
					}

				}

					$Jumpy++;
				 		if(Player::getMountedItem(%player, 2) == "flag")
						{
							Player::dropItem(%player,Player::getMountedItem(%player, 2));
							Client::sendMessage(Player::getClient(%player),1,"You dropped the flag while jumping... NICE JOB.");
						}
//-------------set their position
	GameBase::startFadeout(%player); 
      %player.invulnerable = true;
	%vel = Item::getVelocity(%player);
	%trans = "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%player);
 	Projectile::spawnProjectile("BuildShock",%trans,%player,%vel,%player);
	Item::setVelocity(%client, 0);
      GameBase::setPosition(%player, %xpos@" "@%ypos@" "@%zpos);
	Item::setVelocity(%client, 0);
	%vel = Item::getVelocity(%player);
	%trans = "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%player);
 	Projectile::spawnProjectile("BuildShock",%trans,%player,%vel,%player);
	Player::decItemCount(%player,JumpAmmo);
	playSound(SoundDryFire, GameBase::getPosition(%player));
	playSound(SoundMissileReload, GameBase::getPosition(%player));
				$JumpUsed[%player] = true;
			      %time = 20; //45 30
				schedule("JumpWaitTime(" @ %time @ ", " @ %player @ ");", 1, %player);
			      %Build = 5;
		      %weapon = Player::getMountedItem(%player,$WeaponSlot);
		      if(%weapon != -1) {
			      Player::unMountItem(%player,$WeaponSlot);
				bottomprint(%client,"",0);
		      }

	         Client::setControlObject(%client, Client::getObserverCamera(%client));
	         Observer::setOrbitObject(%client, %player, 3, 3, 3);
				schedule("JumpBuildTime(" @ %build @ ", " @ %player @ ");", 1, %player);
		GameBase::startFadein(%player);



}		
			else
			Client::sendMessage(%client,0,"Jumper pack not ready.  ~wError_Message.wav");
			}
			else
			Client::sendMessage(%client,0,"Cannot Jump there.  ~wError_Message.wav");
		}
		else
		Client::sendMessage(%client,0,"Target exceeds Jumper gun range ~wError_Message.wav");
	}
	else
	Client::sendMessage(%client,0,"You ran out of Jump charges. ~waccess_denied.wav");
	
}



function JumpWaitTime(%time, %player)
{
	if(%time > 0)
	{
		schedule("JumpWaitTime( " @ %time - 1 @ ", " @ %player @ ");", 1, %player);
	}
	else if(%time == 0)
	{
		JumpReset(%player);
	}
}

function JumpBuildTime(%Build, %player)
{


	%client = GameBase::getOwnerClient(%player);
	if(%Build > 0)
	{
	Item::setVelocity(%client, 0);
	%vel = Item::getVelocity(%player);
	%trans = "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%player);
 	Projectile::spawnProjectile("BuildShock",%trans,%player,%vel,%player);
	schedule("JumpBuildTime( " @ %Build -1 @ ", " @ %player @ ");", 1, %player);
	}
	else if(%Build <= 0)
	{
      %player.invulnerable = "";
      Client::setControlObject(%client, %client);
      GameBase::setAutoRepairRate(%player, 0);
	%weapon = Player::getMountedItem(%player,$WeaponSlot);
	   if(%weapon == -1) {
		remotenextweapon(%client);
	    }



	}
}

function JumpReset(%player)
{
	$JumpUsed[%player] = false;
	Client::sendMessage(Player::getClient(%player),3,"Jumper pack ready...");
}














//--------------------------------------
ItemImageData TeleportPackImage
{
	shapeFile = "sensor_small";
	mountPoint = 2;

	maxEnergy = 0;
	weaponType = 2;

	mountOffset= { 0, 0.05, 0 };
	mountRotation = { 0, 0, 0 };
	firstPerson = false;
	
	//sfxReady = SoundShieldOn;
	sfxFire = ForceFieldOpen;
	//explosionTag     = LargeShockwave;
};
additem(TeleportPack);
ItemData TeleportPack
{
	description = "Teleport Pack";
	className = "Backpack";
	shapeFile = "sensor_small";
	hudIcon = "energypack";
	heading = "cBackpacks";
	shadowDetailMask = 4;
	imageType = TeleportPackImage;
	price = 300;
	showWeaponBar = true;
	hiliteOnActive = true;
	
};

function TeleportPack::onMount(%player,%item)
{
	%client = Player::getClient(%player);
	//gamebase::playsound(%item,SoundTeleportPower);
	Bottomprint(%client, "<f2> Teleport Pack, instantly transport you and any nearby teammates to your current waypoint. Use at your own risk.",10);
}


function TeleportPackImage::onActivate(%player, %imageSlot)
{
	%client = Player::getClient(%player);
	%waypoint= $WayX @ " " @ $WayY;
	%waypoint= $point[%client];
	if (%waypoint== " 0 0" || !%waypoint)
	{
		Client::sendMessage(%client,1,"The Teleport Pack has no waypoint to lockon to.  Transport aborted.");
		Player::trigger(%player,$BackpackSlot,false);
	}
	else
	{
		%teamspot = WaypointToWorld(%waypoint);
		%vertical = "0 0 400";
 		%teamspot = Vector::add(%teamspot,%vertical);
 		%set = newObject("set",SimSet);
		%num = containerBoxFillSet(%set,$ItemObjectType,%teamspot,50,50,800,0);
		%num2 = CountObjects(%set,"flag",%num);
		//Client::sendMessage(%client,1,"Waypoint: "@ %waypoint);
		%totalnum = Group::objectCount(%set);
		%enemyflag=0;
		for(%i = 0; %i < %totalnum; %i++)
		{
			%obj = Group::getObject(%set, %i);
			%name = Item::getItemData(%obj);
			//client::sendMessage(Player::getClient(%player),1,"Itemdata: "@ %i @ " " @ %name);
			if(%name == "flag")
			{
				if(GameBase::getTeam(%obj) != Gamebase::getTeam(%player)) %enemyflag = %enemyflag+1;
			}
		}
		deleteObject(%set);
		if (%enemyflag == 0)
		{
			if(!$TeleUsed[%player]) 
			{
				GameBase::playSound(%player,ForceFieldOpen,0);
				//Client::sendMessage(Player::getClient(%player),3,"You successfully teleported!");
				Client::sendMessage(Player::getClient(%player),3,"Your armor's teleport module is now recharging.");
				%client = Player::getClient(%player);
				%object = Client::getOwnedObject(%client);
				%set = newObject("set",SimSet);
				%num = containerBoxFillSet(%set,$SimPlayerObjectType,GameBase::getPosition(%player),5,5,5,0);
				%totalnum = Group::objectCount(%set);
				//Client::sendMessage(Player::getClient(%player),1,"There are "@ %totalnum @ " players near your jump point");
				%destinationspot[%player]=%teamspot;
				%mass=0;
				$point[%client]=" 0 0";
				for(%i = 0; %i < %totalnum; %i++)
				{
				 	%obj = Group::getObject(%set, %i);
				 	if(GameBase::GetTeam(%obj) == GameBase::GetTeam(%player))
					{
				 		%mass=%mass+getmass(%obj);
				 		if (%obj != %player)
				 		{
				 			%posobj = GameBase::getPosition(%obj);
				 			%posplayer = GameBase::getPosition(%player);
				 			%resultant = Vector::sub(%posplayer, %posobj);
				 			//Client::sendMessage(Player::getClient(%player),1,"vector to object: "@ %resultant @ " Dist: " @ %dist);
				 			%destinationspot[%obj]=Vector::add(%teamspot,%resultant);
				 		}
				 	}
				}
				%rnd = floor(getRandom() * 55);
				for(%i = 0; %i < %totalnum; %i++)
				{
				 	
				 	%obj = Group::getObject(%set, %i);
				 	if(GameBase::GetTeam(%obj) == GameBase::GetTeam(%player))
					{
				 		%hasflag = Player::getMountedItem(%obj, 2);
				 		%armor = player::GetArmor(%obj);
				 		%PackClient = Player::getClient(%player);
				 		if(%hasflag!=-1 && %hasflag!="DeadWeight")
						{
							Player::dropItem(%obj, %hasflag);
							Client::sendMessage(Player::getClient(%obj),1,"While teleporting, you dropped the flag... NICE JOB.");
						}
						if (%rnd > %mass)//%rnd > %mass
						{
							if (%armor ==larmor || %armor == lfemale || %armor == marmor || %armor == mfemale)
							{
				 				TeleportPlayer(%obj,%destinationspot[%obj]);
				 				Client::sendMessage(Player::getClient(%obj),3,"You successfully teleported!");
				 				//Client::sendMessage(Player::getClient(%player),1,"Total Mass: "@ %mass @ " Rnd: " @ %rnd);
				 				GameBase::playSound(%obj,ForceFieldOpen,0);
				 			}
				 		}
				 		else
				 		{
				 			TeleportPlayer(%obj,%destinationspot[%obj]);
				 			%damagedClient = Player::getClient(%obj);
				 			Client::sendMessage(Player::getClient(%obj),1,"Malfunction in transport subsystem!");
				 			GameBase::applyDamage(%obj,$CrushDamageType,0.5,%destinationspot[%obj],"0 0 0","0 0 0",%damagedClient);
				 			//Player::kill(%obj);
				 			if(%damagedClient.lastDamage < getSimTime()) 
							{
								%sound = radnomItems(3,injure1,injure2,injure3);
								playVoice(%damagedClient,%sound);
								%damagedClient.lastdamage = getSimTime() + 1.5;
							}
				 			Player::blowUp(%obj);
				 		}
					}
				}
				//bottomprintall("Total Mass: " @ %mass,0)
				deleteObject(%set);
				setCommandStatus(%client, 0, "Objective completed.~wobjcomp");
				//Client::sendMessage(Player::getClient(%player),1,"Position" @ GameBase::getPosition(%player));
				Player::trigger(%player,$BackpackSlot,false);
				$TeleUsed[%player] = true;
			        %time = 20; //45 30
				schedule("TelePackCooling(" @ %time @ ", " @ %player @ ");", 1, %player);
			}
			else 
			{
				Client::sendMessage(Player::getClient(%player),1,"Your armor's teleport module is still recharging.");
				Player::trigger(%player,$BackpackSlot,false);
			}
	
		}
		else
		{
			Client::sendMessage(Player::getClient(%player),1,"Your waypoint is too near an enemy flag.  Teleport not possible.");
			Player::trigger(%player,$BackpackSlot,false);
		}
	}
	
}

function TeleportPackImage::onDeactivate(%player, %imageSlot)
{
	//TeleportPackImage::onActivate(%player, %imageSlot);
}

function getmass(%obj)
{
	%mass = 13;
	%armor = player::GetArmor(%obj);
	if(%armor==harmor) %mass =37;

	if(%armor==larmor) %mass =19;
	if(%armor==lfemale) %mass =19;
	if(%armor==marmor) %mass =25;
	if(%armor==mfemale) %mass =25;

	return %mass;
}


function TeleportPlayer(%player,%tportspot)
{
	%vel = Item::getVelocity(%player);
	%trans = "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%player);
 	Projectile::spawnProjectile("TeleportShock",%trans,%player,%vel,%player);
	Player::setDamageFlash(%player, 0.4);
	GameBase::startFadein(%player);
	%camera = newObject("Camera","Turret",cameraturret,true);
	GameBase::setPosition(%camera,%tportspot);
	GameBase::getLOSInfo(%camera,600,"-1.5708 0 0");
	DeleteObject(%camera);
	GameBase::setPosition(%player, $los::position);
	%vel = Item::getVelocity(%player);
	%trans = "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%player);
 	Projectile::spawnProjectile("TeleportShock",%trans,%player,%vel,%player);
}



function TelePackCooling(%time, %player)
{
	if(%time > 0)
	{
		schedule("TelePackCooling( " @ %time - 1 @ ", " @ %player @ ");", 1, %player);
	}
	else if(%time == 0)
	{
		TeleReset(%player);
	}
}

function TeleReset(%player)
{
	$TeleUsed[%player] = false;
	Client::sendMessage(Player::getClient(%player),3,"Teleport module is now available...");
}

///-----------------------------------------------
//
// Code by Plasmatic
// Please get ahold of me if you want to use this
// ziptiezmail@netscape.net
//
///-----------------------------------------------
//--------------------------------------



//--------------------------------------

ItemImageData SensorJammerPackImage
{
	shapeFile = "sensorjampack";
	mountPoint = 2;
	weaponType = 2;  // Sustained
	maxEnergy = 10;  // Energy used/sec for sustained weapons
	sfxFire = SoundJammerOn;
  	mountOffset = { 0, -0.05, 0 };
  	mountRotation = { 0, 0, 0 };
	firstPerson = false;
};
additem(SensorJammerPack);
ItemData SensorJammerPack
{
	description = "Cloak- Jammer";
	shapeFile = "sensorjampack";
	className = "Backpack";
   heading = "cBackpacks";
	shadowDetailMask = 4;
	imageType = SensorJammerPackImage;
	price = 200;
	hudIcon = "sensorjamerpack";
	showWeaponBar = true;
	hiliteOnActive = true;
   validateShape = true;
   validateMaterials = false;
};

function SensorJammerPackImage::onActivate(%player,%imageSlot)
{	bottomprint(Player::getClient(%player), "<f2> Jammer pack on, You feeling lucky PUNK?", 10);
	Client::sendMessage(Player::getClient(%player),0,"Sensor Jammer On");
	%rate = Player::getSensorSupression(%player) + 10;//20
	Player::setSensorSupression(%player,%rate);
	gamebase::playsound(%item,SoundTeleportPower);
	%player.guiLock = true; 
	%c = Player::getClient(%player); 
	%c.guiLock = true; 
	%clientId.ghostDoneFlag = true; 
	startGhosting(%cl);
	GameBase::startFadeout(%player); 
}

function SensorJammerPack::onMount(%player,%item)
{	//echo("get jam pack");
	%client = Player::getClient(%player);
	//gamebase::playsound(%item,SoundTeleportPower);
	Bottomprint(%client, "<f2> Jammer Pack, makes you a sneakey bastard",10);
	%rate = Player::getSensorSupression(%player);
	//Player::setSensorSupression(%player,%rate +1);

}

function SensorJammerPack::onUnmount(%player,%item)
{	//echo("drop jam pack");
	%client = Player::getClient(%player);
	%rate = Player::getSensorSupression(%player);
	//Player::setSensorSupression(%player,%rate -1);
}

function SensorJammerPackImage::onDeactivate(%player,%imageSlot)
{
	Client::sendMessage(Player::getClient(%player),0,"Sensor Jammer Off");
	%rate = Player::getSensorSupression(%player) - 10;//20
	Player::setSensorSupression(%player,%rate);
bottomprint(Player::getClient(%player), "<f2> Jammer pack off!", 5);
	Player::trigger(%player,$BackpackSlot,false);
	%player.guiLock = ""; 
	%c = Player::getClient(%player); 
	%c.guiLock = "";
	GameBase::startFadein(%player);

}

//--------------------------------------

ItemImageData CloakingDeviceImage { 
	shapeFile = "sensorjampack"; 
	mountPoint = 2; 
	weaponType = 2; 
	maxEnergy = 10; 
	sfxFire = SoundJammerOn; 
	mountOffset = { 0, -0.05, 0 }; 
	mountRotation = { 0, 0, 0 }; 
	firstPerson = false; };

additem(CloakingDevice);
ItemData CloakingDevice { 
	description = "Cloaking Device"; 
	shapeFile = "sensorjampack"; 
	className = "Backpack"; 
	heading = "cBackpacks"; 
	shadowDetailMask = 4; 
	imageType = CloakingDeviceimage; 
	price = 600; 
	hudIcon = "sensorjamerpack"; 
	showWeaponBar = true; 
	hiliteOnActive = true; }; 

function CloakingDeviceImage::onActivate(%player,%imageSlot) { 
	GameBase::startFadeout(%player); 
	Client::sendMessage(Player::getClient(%player),0,"Cloaking Device On"); 
	%rate = Player::getSensorSupression(%player) + 5; 
	Player::setSensorSupression(%player,%rate); 
	%player.guiLock = true; 
	%c = Player::getClient(%player); 
	%c.guiLock = true; 
	%clientId.ghostDoneFlag = true; 
	startGhosting(%cl); } 

function CloakingDeviceImage::onDeactivate(%player,%imageSlot) { 
	GameBase::startFadein(%player); 
	Client::sendMessage(Player::getClient(%player),0,"Cloaking Device Off"); 
	%rate = Player::getSensorSupression(%player) - 5; 
	Player::setSensorSupression(%player,%rate); 
	Player::trigger(%player,$BackpackSlot,false); 
	%player.guiLock = ""; 
	%c = Player::getClient(%player); 
	%c.guiLock = ""; } 


//--------------------------------------



//--------------------------------------

ItemImageData AmmoPackImage
{
	shapeFile = "AmmoPack";
	mountPoint = 2;
   mountOffset = { 0, -0.03, 0 };
//   mountRotation = { 1.57, 0, 0 };
	firstPerson = false;
};
additem(AmmoPack);
ItemData AmmoPack
{
	description = "Ammo Pack";
	shapeFile = "AmmoPack";
	className = "Backpack";
   heading = "cBackpacks";
	imageType = AmmoPackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 325;
	hudIcon = "ammopack";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function AmmoPack::onDrop(%player, %item)
{
	if($matchStarted) {
		%item = Item::onDrop(%player,%item);
		for(%i = 0; %i < 20 ; %i = %i +1) {
			%numPack = 0;
			%ammoItem = $AmmoPackItems[%i];
			%maxnum = $ItemMax[Player::getArmor(%player), %ammoItem];
			%pCount = Player::getItemCount(%player, %ammoItem);
			if(%pCount > %maxnum) {
				%numPack = %pCount - %maxnum;
				Player::decItemCount(%player,%ammoItem,%numPack);
			}	
			if(%i == 0) {
	 	    	%item.BulletAmmo = %numPack;
			}
			else if(%i == 1) {
	 	    	%item.PlasmaAmmo = %numPack;
			}
			else if(%i == 2) {
	 	    	%item.DiscAmmo = %numPack;
			}
			else if(%i == 3) {
	 	    	%item.GrenadeAmmo = %numPack;
			}
			else if(%i == 4) {
	 	    	%item.Grenade = %numPack;
			}
			else if(%i == 5) {
	 	    	%item.MortarAmmo = %numPack;
			}
			else {
	 	    	%item.MineAmmo = %numPack;
			}
		}
	}
}

function AmmoPack::onCollision(%this,%object)
{
	if (getObjectType(%object) == "Player") {
		%item = Item::getItemData(%this);
		%count = Player::getItemCount(%object,%item);
		if (Item::giveItem(%object,%item,Item::getCount(%this))) {
			Item::playPickupSound(%this);
			checkPacksAmmo(%object, %this);
			Item::respawn(%this);
//
	%player = Client::getOwnedObject(%object);
	for(%i = 0; %i < 30 ; %i = %i +1) {
		%item = $AmmoPackItems[%i];
		%maxnum = $AmmoPackMax[%item];
		%maxnum = checkResources(%object,%item,%maxnum); 
		if(%maxnum) {
			Player::incItemCount(%object,%item,%maxnum);
			teamEnergyBuySell(%object,%item.price * %maxnum * -1);
		}	
	}
//
		}
	}
}

function checkPacksAmmo(%player, %item)
{
	for(%i = 0; %i < 7 ; %i = %i +1) {
		%ammoItem = $AmmoPackItems[%i];
		if(%i == 0) {
	        %numAdd = %item.BulletAmmo;
		}
		else if(%i == 1) {
	    	%numAdd = %item.PlasmaAmmo;
		}
		else if(%i == 2) {
	    	%numAdd = %item.DiscAmmo;
		}
		else if(%i == 3) {
	    	%numAdd = %item.GrenadeAmmo;
		}
		else if(%i == 4) {
	    	%numAdd = %item.Grenade;
		}
		else if(%i == 5) {
 	    	%numAdd = %item.MortarAmmo;
		}
		else {
			%numAdd = %item.MineAmmo;
		}
		Player::incItemCount(%player,%ammoItem,%numAdd);
	}						 
}

function fillAmmoPack(%client)
{
	%player = Client::getOwnedObject(%client);
	for(%i = 0; %i < 30 ; %i = %i +1) {
		%item = $AmmoPackItems[%i];
		%maxnum = $AmmoPackMax[%item];
		%maxnum = checkResources(%player,%item,%maxnum); 
		if(%maxnum) {
			Player::incItemCount(%client,%item,%maxnum);
			teamEnergyBuySell(%player,%item.price * %maxnum * -1);
		}	
	}
}

// BLAH...;)



//--------------------------------------



//--------------------------------------

$TeamItemMax[DeployableAmmoPack] = 7;
additem(DeployableAmmoPack);
//--------------------------------------

ItemImageData DeployableAmmoPackImage
{
	shapeFile = "ammounit_remote";
	mountPoint = 2;
	mountOffset = { 0, -0.1, -0.3 };
	mountRotation = { 0, 0, 0 };
	mass = 1.0;
	firstPerson = false;
};

ItemData DeployableAmmoPack
{
	description = "Ammo Station";
	shapeFile = "ammounit_remote";
	className = "Backpack";
   heading = "dDeployables";
	shadowDetailMask = 4;
	imageType = DeployableAmmoPackImage;
	mass = 2.0;
	elasticity = 0.2;
	price = $RemoteAmmoEnergy;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};


function DeployableAmmoPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function DeployableAmmoPack::onDeploy(%player,%item,%pos)
{	
	if (DeployableAmmoPack::deployShape(%player,%item)&& !$build) {
		Player::decItemCount(%player,%item);
	}
}	
function DeployableAmmoStation::onDestroyed(%this) 
{ 
	$TeamItemCount[GameBase::getTeam(%this) @ "DeployableAmmoPack"]--; $missionItems--;
} 

function DeployableAmmoPack::deployShape(%player,%item)
{
	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]|| $build) {
		if (GameBase::getLOSInfo(%player,3)) {
			%obj = getObjectType($los::object);
			if (%obj == "SimTerrain" || %obj == "InteriorShape") {
				if (Vector::dot($los::normal,"0 0 1") > 0.7) {
					if(checkDeployArea(%client,$los::position)) {
						%inv = newObject("ammounit_remote","StaticShape","DeployableAmmoStation",true);
	         	   addToSet("MissionCleanup", %inv);
						%rot = GameBase::getRotation(%player); 
						GameBase::setTeam(%inv,GameBase::getTeam(%player));
						GameBase::setPosition(%inv,$los::position);
						GameBase::setRotation(%inv,%rot);
						Gamebase::setMapName(%inv,%name);
						if(!$build)Client::sendMessage(%client,0,"Ammo Station deployed");
						playSound(SoundPickupBackpack,$los::position);
						$missionItems++;
						$TeamItemCount[GameBase::getTeam(%inv) @ "DeployableAmmoPack"]++;
						$ammostation = $ammostation +1;
						echo("MSG: ",Client::getName(%client)," CL# ",%client," deployed an Ammo Station");
						return true;
					}
				}
				else 
					if ($build) Client::sendMessage(%client,0,"~waccess_denied.wav");
			else Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
			}
			else 
				if ($build) Client::sendMessage(%client,0,"~waccess_denied.wav");
			else Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");
		}
		else 
			GameBase::playSound(%player, usepack, 0);
			if (!$build) Client::sendMessage(%client,0,"Deploy position out of range");
	}
	else																						  
	 	Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
	return false;
}

//--------------------------------------

StaticShapeData DeployableAmmoStation
{
   description = "Remote Ammo Unit";
	shapeFile = "ammounit_remote";
   validateShape = true;
//   validateMaterials = true;
	className = "DeployableStation";
	maxDamage = 0.25;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	sequenceSound[1] = { "use", SoundUseAmmoStation };
	sequenceSound[2] = { "power", SoundAmmoStationPower };

	visibleToSensor = true;
	shadowDetailMask = 4;
	castLOS = true;
	supression = false;
	supressable = false;
	mapFilter = 4;
	mapIcon = "M_station";
	debrisId = flashDebrisSmall;
	damageSkinData = "objectDamageSkins";
   explosionId = flashExpMedium;
};


function DeployableAmmoStation::onAdd(%this)
{
	schedule("DeployableStation::deploy(" @ %this @ ");",1,%this);
	if (GameBase::getMapName(%this) == "") 
		GameBase::setMapName (%this, "R-Ammo Station");
	%this.Energy = $RemoteAmmoEnergy;
}

function DeployableAmmoStation::onActivate(%this)
{
	if(%this.deployed == 1) {
		GameBase::playSequence(%this,1,"use");
		//echo("Activate " @ %this);
		schedule("AmmoStation::onResupply(" @ %this @ ");",0.5,%this);
		%this.lastPlayer = Station::getTarget(%this);
		%player = %this.lastPlayer; 
		%player.Station = %this;
		%this.target = Player::getClient(Station::getTarget(%this));
		%weapon = Player::getMountedItem(%player,$WeaponSlot);
		if(%weapon != -1) {
			%player.lastWeapon = %weapon;
			Player::unMountItem(%player,$WeaponSlot);
		}
	}
	else 
		GameBase::setActive(%this,false);	
}


//--------------------------------------



//--------------------------------------

$TeamItemMax[MotionSensorPack] = 15;

//--------------------------------------

ItemImageData MotionSensorPackImage
{
	shapeFile = "sensor_small";
	mountPoint = 2;
	mountOffset = { 0, 0, 0.1 };
	mountRotation = { 1.57, 0, 0 };
	firstPerson = false;
};
additem(MotionSensorPack);
ItemData MotionSensorPack
{
	description = "Motion Sensor";
	shapeFile = "sensor_small";
	className = "Backpack";
   heading = "dDeployables";
	imageType = MotionSensorPackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 125;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function MotionSensorPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function MotionSensorPack::onDeploy(%player,%item,%pos)
{	
	if (MotionSensorPack::deployShape(%player,%item)&& !$build) {
		Player::decItemCount(%player,%item);
		$missionItems++;
		$TeamItemCount[GameBase::getTeam(%player) @ "MotionSensorPack"]++;
	}
}
function DeployableMotionSensor::onDestroyed(%this)
{
	$missionItems--;
}
function MotionSensorPack::deployShape(%player,%item)
{
 	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]|| $build) {
		if (GameBase::getLOSInfo(%player,3)) {
			// GetLOSInfo sets the following globals:
			// 	los::position
			// 	los::normal
			// 	los::object
			%obj = getObjectType($los::object);
			if (%obj == "SimTerrain" || %obj == "InteriorShape") {
				// Try to stick it straight up or down, otherwise
				// just use the surface normal
				%prot = GameBase::getRotation(%player);
				%zRot = getWord(%prot,2);
				if (Vector::dot($los::normal,"0 0 1") > 0.6) {
					%rot = "0 0 " @ %zRot;
				}
				else {
					if (Vector::dot($los::normal,"0 0 -1") > 0.6) {
						%rot = "3.14159 0 " @ %zRot;
					}
					else {
						%rot = Vector::getRotation($los::normal);
					}
				}
				if(checkDeployArea(%client,$los::position)) {
					%mSensor = newObject("","Sensor",DeployableMotionSensor,true);
	   	      addToSet("MissionCleanup", %mSensor);
					GameBase::setTeam(%mSensor,GameBase::getTeam(%player));
					GameBase::setRotation(%mSensor,%rot);
					GameBase::setPosition(%mSensor,$los::position);
					Gamebase::setMapName(%mSensor,"Motion Sensor");
					if(!$build)Client::sendMessage(%client,0,"Motion Sensor deployed");
					playSound(SoundPickupBackpack,$los::position);
					$MotionSensor = $MotionSensor +1;$missionItems++;
					echo("MSG: ",Client::getName(%client)," CL# ",%client," deployed a Motion Sensor");
					return true;
				}
			}
			else {
				if ($build) Client::sendMessage(%client,0,"~waccess_denied.wav");
			else Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");
			}
		}
		else {
			GameBase::playSound(%player, usepack, 0);
			if (!$build) Client::sendMessage(%client,0,"Deploy position out of range");		
		}
	}
	else																						  
	 	Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
	
	return false;
}


ItemImageData PulseSensorPackImage
{
	shapeFile = "radar_small";
	mountPoint = 2;
	mountOffset = { 0, 0, 0.1 };
	mountRotation = { 1.57, 0, 0 };
	firstPerson = false;
};
additem(PulseSensorPack);
ItemData PulseSensorPack
{
	description = "Pulse Sensor";
	shapeFile = "radar_small";
	className = "Backpack";
   heading = "dDeployables";
	imageType = PulseSensorPackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 125;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function PulseSensorPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}
function DeployablePulseSensor::onDestroyed(%this)
{
	$missionItems--;
}
function PulseSensorPack::onDeploy(%player,%item,%pos)
{	
	if (Item::deployShape(%player,"Pulse Sensor",DeployablePulseSensor,%item)) {
		if(!$build)Player::decItemCount(%player,%item);
		$missionItems++;
		$TeamItemCount[GameBase::getTeam(%player) @ "PulseSensorPack"]++;
		$PulseSensor = $PulseSensor +1;
	}
}
ItemImageData DeployableSensorJamPackImage
{
	shapeFile = "sensor_jammer";
 	mountPoint = 2;
  	mountOffset = { 0, 0.03, 0.1 };
  	mountRotation = { 1.57, 0, 0 };
	firstPerson = false;
};
additem(DeployableSensorJammerPack);
ItemData DeployableSensorJammerPack
{
	description = "Sensor Jammer";
  	shapeFile = "sensor_jammer";
  	className = "Backpack";
   heading = "dDeployables";
	imageType = DeployableSensorJamPackImage;
  	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
  	price = 225;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function DeployableSensorJammerPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}
function DeployableSensorJammer::onDestroyed(%this) 
{ 
	$TeamItemCount[GameBase::getTeam(%this) @ "DeployableSensorJammerPack"]--; $missionItems--;
} 

function DeployableSensorJammerPack::onDeploy(%player,%item,%pos)
{	
	if (Item::deployShape(%player,"Sensor Jammer",DeployableSensorJammer,%item)) {
		if(!$build)Player::decItemCount(%player,%item);
		$missionItems++;
		$TeamItemCount[GameBase::getTeam(%player) @ "DeployableSensorJammerPack"]++;
		$SensorJammer = $SensorJammer +1;
	}
}

//---------------------------
// misc items
//--------------------------------------


$ItemMax[harmor, InvPatch] = 1;
$ItemMax[larmor, InvPatch] = 1;
$ItemMax[lfemale, InvPatch] = 1;
$ItemMax[marmor, InvPatch] = 1;
$ItemMax[mfemale, InvPatch] = 1;
$SellAmmo[InvPatch] = 1;
$InvList[InvPatch] = 1;
$RemoteInvList[InvPatch] = 1;

additem(InvPatch,true);

///-----------------------------------------------
ItemImageData InvPatchImage
{
        shapeFile = "armorpatch";//newdoor5
 //       mountPoint = 2;
        //mountOffset = { -0.375, 0, 0 };
        //mountRotation = { 0, 1.57, 0 };
        mass = 2.5;
        firstPerson = false;
};

ItemData InvPatch
{
        description = "Inventory Patch";
        shapeFile = "armorpatch";//newdoor5
	className = "HandAmmo";
   heading = "qMiscellany";//        heading = "dDeployables";
        imageType = InvPatchImage;
        shadowDetailMask = 4;
//       mass = 2.5;
//        elasticity = 0.2;
        price = 2000;
//        hudIcon = "deployable";
//        showWeaponBar = true;
//        hiliteOnActive = true;
};
function InvPatch::onUse(%player,%item)
{
   if($matchStarted){
	%client = Player::getClient(%player);
      QuickInv(%client);
	Player::decItemCount(%client,InvPatch);
	%client.InvAcc = true;
   }
}

//additem(InvPatch);
//=====================================

additem(RepairKit);
ItemData RepairKit
{
   description = "Repair Kit";
   shapeFile = "armorKit";
   heading = "qMiscellany";
   shadowDetailMask = 4;
   price = 35;
   validateShape = true;
   validateMaterials = false;
};


ItemData RepairPatch
{
	description = "Repair Patch";
	className = "Repair";
	shapeFile = "armorPatch";
   heading = "qMiscellany";
	shadowDetailMask = 4;
  	price = 2;
   validateShape = true;
   validateMaterials = false;
};

additem(MineAmmo);
ItemData MineAmmo 
{
	description = "Mine"; 
	shapeFile = "mineammo"; 
	heading = "qMiscellany"; 
	shadowDetailMask = 4; 
	price = 10; 
	className = "HandAmmo"; 
}; 

ItemData Grenade
{
   description = "Grenade";
   shapeFile = "grenade";
   heading = "qMiscellany";
   shadowDetailMask = 4;
   price = 5;
	className = "HandAmmo";
   validateShape = true;
   validateMaterials = false;
};
additem(Grenade);
//-----------------------------------------

additem(Beacon);
ItemData Beacon
{
   description = "Beacon";
   shapeFile = "sensor_small";
   heading = "qMiscellany";
   shadowDetailMask = 4;
   price = 5;
	className = "HandAmmo";
   validateShape = true;
   validateMaterials = false;
};

additem(LightArmor);
ItemData LightArmor
{
   heading = "aArmor";
	description = "Light Armor";
	className = "Armor";
	price = 175;
};

additem(MediumArmor);
ItemData MediumArmor
{
   heading = "aArmor";
	description = "Medium Armor";
	className = "Armor";
	price = 250;
};

ItemData PenisPack
{
	heading = "cBackPacks";
	description = "Penis";
	className = "BackPack";
	shapeFile  = "cactus1";
	shadowDetailMask = 4;
	imageType = PenisPackImage;
	price = 0;
	showWeaponBar = true;
};
additem(HeavyArmor);
ItemData HeavyArmor
{
   heading = "aArmor";
	description = "Heavy Armor";
	className = "Armor";
	price = 400;
};
function Beacon::onUse(%player,%item)
{
	if (Beacon::deployShape(%player,%item)) {
		Player::decItemCount(%player,%item);
	}
}

function Beacon::deployShape(%player,%item)
{
 	%client = Player::getClient(%player);
	if (GameBase::getLOSInfo(%player,3)) {
		// GetLOSInfo sets the following globals:
		// 	los::position
		// 	los::normal
		// 	los::object
		%obj = getObjectType($los::object);
		if (%obj == "SimTerrain" || %obj == "InteriorShape") {
			// Try to stick it straight up or down, otherwise
			// just use the surface normal
			if (Vector::dot($los::normal,"0 0 1") > 0.6) {
				%rot = "0 0 0";
			}
			else {
				if (Vector::dot($los::normal,"0 0 -1") > 0.6) {
					%rot = "3.14159 0 0";
				}
				else {
					%rot = Vector::getRotation($los::normal);
				}
			}
		  	%set=newObject("set",SimSet);
			%num=containerBoxFillSet(%set,$StaticObjectType | $ItemObjectType | $SimPlayerObjectType,$los::position,0.3,0.3,0.3,1);
			deleteObject(%set);
			if(!%num) {
				%team = GameBase::getTeam(%player);
				if($TeamItemMax[%item] > $TeamItemCount[%team @ %item] || $TestCheats) {
					%beacon = newObject("Target Beacon", "StaticShape", "DefaultBeacon", true);
				   addToSet("MissionCleanup", %beacon);
					//, CameraTurret, true);
					GameBase::setTeam(%beacon,GameBase::getTeam(%player));
					GameBase::setRotation(%beacon,%rot);
					GameBase::setPosition(%beacon,$los::position);
					Gamebase::setMapName(%beacon,"Target Beacon");
   			   Beacon::onEnabled(%beacon);
					Client::sendMessage(%client,0,"Beacon deployed");
					//playSound(SoundPickupBackpack,$los::position);
					$missionItems++;
					$TeamItemCount[GameBase::getTeam(%beacon) @ "Beacon"]++;
					return true;
				}
				else
					Client::sendMessage(%client,0,"Deployable Item limit reached");
			}
			else
				Client::sendMessage(%client,0,"Unable to deploy - Item in the way");
		}
		else {
			Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");
		}
	}
	else {
		Client::sendMessage(%client,0,"Deploy position out of range");
	}
	return false;
}

//--------------------------------------


// BLAH!
// I'm sure you have something better to do than look at my code?!
// get outside, get some sun and a girlfriend...
// yer palms are getting hairy spanky...
// -Plasmatic
// ziptiezmail@netscape.net


