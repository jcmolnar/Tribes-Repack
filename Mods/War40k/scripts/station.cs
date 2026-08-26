function Station::onActivate(%this) 
{
	%obj = Station::getTarget(%this);
	if (%obj != -1) 
	{    
		GameBase::playSequence(%this,1,"activate");
		GameBase::setSequenceDirection(%this,1,1);
		%this.lastPlayer = %obj;
	}
	else GameBase::setActive(%this,false);
}

function Station::onDeactivate(%this) 
{
	GameBase::stopSequence(%this,2);
	GameBase::setSequenceDirection(%this,1,0);
}

function Station::onEndSequence(%this,%thread) 
{
	if (%thread == 1 && GameBase::isActive(%this)) 
	{
		GameBase::playSequence(%this,2,"use");
		return true;
	}
	%client = %this.target;
	if(%client == "") 
	{
		%player = Station::getTarget(%this);
		%client = Player::getClient(%player);
	}
	if(%client != "") 
	{
		if(Client::getGuiMode(%client) != 1) Client::setGuiMode(%client,1);
		%team = Client::getTeam(%client);
		if($TeamEnergy[%team] != "Infinite") 
		{
			if(%this.clTeamEnergy != %client.TeamEnergy) 
			{
				if(%client.teamEnergy < 0) Client::sendMessage(%client,0,"Your total mission purchases have come to " @ (%client.teamEnergy * -1) @ ".");
				else Client::sendMessage(%client,0,"You have increased the Team Energy by " @ %client.teamEnergy @ ".");
			}
			if((%client.teamEnergy -%client.EnergyWarning < $TeammateSpending) && ($TeammateSpending != 0) && !$TeamEnergyCheat) 
			{
				TeamMessages(0, %team, "Teammate " @ Client::getName(%client) @ " has spent " @ (%client.teamEnergy *-1) @ " of the TeamEnergy");
				%client.EnergyWarning = %client.teamEnergy;
			}
			if($TeamEnergy[%team] < $WarnEnergyLow) TeamMessages(0, %team, "TeamEnergy Low: " @ $TeamEnergy[%team]);
		}
	}
	if(%this.target != "") 
	{
		(Client::getOwnedObject(%this.target)).Station = "";
		%this.target = "";
	}
	if(GameBase::getDataName(%this) == VehicleStation && %this.vehiclePad.busy < getSimTime()) VehiclePad::checkSeq(%this.vehiclePad, %this);
	%this.clTeamEnergy = "";
	return false;
}

function Station::onPower(%this,%power,%generator) 
{
	if (%power) 
	{
		GameBase::playSequence(%this,0,"power");
		GameBase::playSequence(%this,1);
	}
	else 
	{
		GameBase::stopSequence(%this,0);
		GameBase::pauseSequence(%this,1);
		GameBase::pauseSequence(%this,2);
		Station::checkTarget(%this);
	}
}

function Station::onEnabled(%this) 
{
	if (GameBase::isPowered(%this)) 
	{
		GameBase::playSequence(%this,0,"power");
		GameBase::playSequence(%this,1);
	}
}

function Station::checkTarget(%this) 
{
	if (%this.target) 
	{
		Client::setGuiMode(%this.target,1);
		GameBase::setActive(%this,false);
	}
}

function Station::onDisabled(%this) 
{
	GameBase::stopSequence(%this,0);
	GameBase::setSequenceDirection(%this,1,0);
	GameBase::pauseSequence(%this,1);
	GameBase::stopSequence(%this,2);
	Station::checkTarget(%this);
}

function Station::onDestroyed(%this) 
{
	StaticShape::objectiveDestroyed(%this);
	GameBase::stopSequence(%this,0);
	GameBase::stopSequence(%this,1);
	GameBase::stopSequence(%this,2);
	Station::checkTarget(%this);
	calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 2, 0.40, 0.1, 250, 100);
}

function Station::getTarget(%this) 
{
	if(GameBase::getLOSInfo(%this,1.5,"0 0 3.14")) 
	{
		%obj = getObjectType($los::object);
		dbecho(3, "STATION: LOS got " @ %obj);
		if (%obj == "Player") 
		{
			if( Player::isAiControlled( $los::object ) != "True" ) 
			return $los::object;
		}
	}
	dbecho(3, "STATION: LOS Got None");
	return -1;
}

function Station::onCollision(%this, %object) 
{
	dbecho(3, "STATION: Collision (" @ %this @ "," @ %object @ ")");
	%obj = getObjectType(%object);
	if (%obj == "Player") 
	{
		%client = Player::getClient(%object);
		if(GameBase::getTeam(%object) == GameBase::getTeam(%this) || GameBase::getTeam(%this) == -1) 
		{
			if (GameBase::getDamageState(%this) == "Enabled") 
			{
				if (GameBase::isPowered(%this)) 
				{
					if(%this.enterTime == "") %this.enterTime = getSimTime();
					GameBase::setActive(%this,true);
				}
				else Client::sendMessage(%client,0,"Power Unit offline");
			}
			else Client::sendMessage(%client,0,"Supply station damaged");
		}
		else if(Station::getTarget(%this) == %object) 
		{
			%curTime = getSimTime();
			if(%curTime - %object.stationDeniedStamp > 3.5 && GameBase::getDamageState(%this) == "Enabled") 
			{
				Client::clearItemShopping(%client);
				Station::onDeactivate(%this);
				Station::onEndSequence(%this,1);
				if(Client::getGuiMode(%client) != 1) Client::setGuiMode(%client,1);
				%object.stationDeniedStamp = %curTime;
				Client::sendMessage(%client,0,"--ACCESS DENIED-- Wrong Team ~waccess_denied.wav");
			}
		}
	}
}

function Station::itemsToResupply(%player) 
{
	%cnt = 0;
	for (%i = 0; %i < $AmmoCount; %i++) %cnt = %cnt + AmmoStation::resupply(%player, $Ammo_Weapon[%i],$Ammo_Ammo[%i],$Ammo_Count[%i]);
	return %cnt;
}

function DeployableStation::onActivate(%this) 
{
	%obj = Station::getTarget(%this);
	if (%obj != -1) 
	{
		GameBase::playSequence(%this,1,"activate");
		GameBase::setSequenceDirection(%this,1,1);
	}
	else GameBase::setActive(%this,false);
}

function DeployableStation::onEndSequence(%this,%thread) 
{
	if(!%thread) 
	{
		%this.deployed = 1;
		GameBase::playSequence(%this,2,"power");
	}
}

function DeployableStation::deploy(%this) 
{
	GameBase::playSequence(%this,0,"deploy");
}

function DeployableStation::onDeactivate(%this) 
{
	GameBase::stopSequence(%this,1);
}

function DeployableStation::onEnabled(%this) 
{
	GameBase::playSequence(%this,2,"power");
}

function DeployableStation::onDisabled(%this) 
{
	GameBase::stopSequence(%this,2);
	GameBase::stopSequence(%this,1);
	Station::checkTarget(%this);
}

function DeployableStation::onDestroyed(%this) 
{
	DeployableStation::onDisabled(%this);
	%stationName = GameBase::getDataName(%this);
	if(%stationName == DeployableInvStation) $TeamItemCount[GameBase::getTeam(%this) @ "DeployableInvPack"]--;
	else if( %stationName == DeployableAmmoStation) $TeamItemCount[GameBase::getTeam(%this) @ "DeployableAmmoPack"]--;
	calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 2, 0.30, 0.1, 200, 100);
}

function DeployableStation::onCollision(%this, %object) 
{
	%obj = getObjectType(%object);
	if (%obj == "Player") 
	{
		%client = Player::getClient(%object);
		if(GameBase::getTeam(%object) == GameBase::getTeam(%this) || GameBase::getTeam(%this) == -1) 
		{
			if (GameBase::getDamageState(%this) == "Enabled") 
			{
				if(%this.enterTime == "") %this.enterTime = getSimTime();
				GameBase::setActive(%this,true);
			}
			else Client::sendMessage(%client,0,"Unit is disabled");
		}
		else if(Station::getTarget(%this) == %object) 
		{
			%curTime = getSimTime();
			if(%curTime - %object.stationDeniedStamp > 3.5 && GameBase::getDamageState(%this) == "Enabled") 
			{
				%object.stationDeniedStamp = %curTime;
				Client::sendMessage(%client,0,"--ACCESS DENIED-- Wrong Team ~waccess_denied.wav");
			}
		}
	}
}

StaticShapeData AmmoStation 
{
	description = "Ammo Supply Unit";
	shapeFile = "ammounit";
	className = "Station";
	visibleToSensor = true;
	sequenceSound[0] = { "activate", SoundActivateAmmoStation };
	sequenceSound[1] = { "power", SoundAmmoStationPower };
	sequenceSound[2] = { "use", SoundUseAmmoStation };
	maxDamage = 1.0;
	debrisId = flashDebrisLarge;
	mapFilter = 4;
	mapIcon = "M_station";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	explosionId = flashExpLarge;
};

function AmmoStation::onEndSequence(%this,%thread) 
{
	if(%this.clTeamEnergy == "") %this.clTeamEnergy = (Player::getClient(Station::getTarget(%this))).TeamEnergy;
	if (Station::onEndSequence(%this,%thread)) AmmoStation::onResupply(%this);
}

function AmmoStation::onResupply(%this) 
{
	if (GameBase::isActive(%this)) 
	{
		%player = Station::getTarget(%this);
		if (%player != -1 && %this.lastPlayer == %player) 
		{
// XXX Resupply needs work
			GameBase::SetDamageLevel(%player,0);
			%cnt = Station::itemsToResupply(%player);
			if(getSimTime() - %this.enterTime > 11) %cnt = 0;
			if (%cnt != 0) 
			{
				schedule("AmmoStation::onResupply(" @ %this @ ");",0.5,%this);
				return;
			}
			%client = Player::getClient(%player);
			Client::sendMessage(%client,0,"Resupply Complete");
			Client::setInventoryText(%client, "<f1><jc>TEAM ENERGY: " @ $TeamEnergy[Client::getTeam(%client)]);
		}
		GameBase::setActive(%this,false);
		%this.enterTime="";
	}
}

function AmmoStation::resupply(%player,%weapon,%item,%delta) 
{
	%delta = checkResources(%player,%item,%delta,1);
	if(%delta > 0) 
	{
		if(%item == RepairPatch) 
		{
			teamEnergyBuySell(%player,%item.price * %delta * -1);
			GameBase::repairDamage(%player,0.06);
			return %delta;
		}
// Weaponless item such as mine, grenade, repairkit, etc 
		else if (%weapon=="Misc" && %item!="")
		{
			teamEnergyBuySell(%player,%item.price * %delta * -1);
			Player::incItemCount(%player,%item,%delta);
			return %delta;
		}
// Weapon item (ammo)
		else if (%weapon!="" && Player::getItemCount(%player,%weapon)>0) 
		{
			teamEnergyBuySell(%player,%item.price * %delta * -1);
			Player::incItemCount(%player,%item,%delta);
			return %delta;
		}
	}
	return 0;
}

StaticShapeData InventoryStation 
{
	description = "Station Supply Unit";
	shapeFile = "inventory_sta";
	className = "Station";
	visibleToSensor = true;
	sequenceSound[0] = { "activate", SoundActivateInventoryStation };
	sequenceSound[1] = { "power", SoundInventoryStationPower };
	sequenceSound[2] = { "use", SoundUseInventoryStation };
	maxDamage = 3.0;
	debrisId = flashDebrisLarge;
	mapFilter = 4;
	mapIcon = "M_station";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	triggerRadius = 1.5;
	explosionId = flashExpLarge;
};

function InventoryStation::onEndSequence(%this,%thread) 
{
	if (Station::onEndSequence(%this,%thread)) InventoryStation::onResupply(%this,"InvList");
}

function InventoryStation::onResupply(%this,%InvShopList) 
{
	dbecho(3, "STATION::Resupply");
	if (GameBase::isActive(%this)) 
	{
		%player = Station::getTarget(%this);
// Heal Special Damage types
		$time[$FlashDamageType, %player] = 0;
		$time[$EnergyDamageType, %player] = 0;
		$time[$PlasmaDamageType, %player] = 0;
		$time[$ChemDamageType, %player] = 0;
		$time[$AcidDamageType, %player] = 0;
		if (%player != -1 && %this.lastPlayer == %player) 
		{
// XXX Resupply needs work
			GameBase::SetDamageLevel(%player,0);
			%client = Player::getClient(%player);
			if (%this.target != %client) 
			{
				%player.Station = %this;
				setupShoppingList(%client,%this,%InvShopList);
				updateBuyingList(%client);
				%this.target = %client;
				%this.clTeamEnergy = %client.TeamEnergy;
				if(!%client.noEnterInventory) Client::setGuiMode(%client,$GuiModeInventory);
				Client::sendMessage(%client,0,"Accessing War Gear");
				%player.ResupplyFlag = 1;
			}
			schedule("InventoryStation::onResupply(" @ %this @ ");",0.5,%this);
			if(%player.ResupplyFlag) %player.ResupplyFlag = resupply(%this);
			return;
		}
		GameBase::setActive(%this,false);
	}
	if (%this.target) 
	{
		%player = Client::getOwnedObject(%this.target);
		Client::clearItemShopping(%this.target);
		Client::sendMessage(%this.target,0,"War Gear Equipped");
		Station::onEndSequence(%this);
		if(GameBase::getDataName(%player.Station) == DeployableInvStation) 
		{
			Client::setInventoryText(%this.target, "<f1><jc>TEAM ENERGY: " @ $TeamEnergy[Client::getTeam(%this.target)]);
			if(Client::getGuiMode(%this.target) != 1) Client::setGuiMode(%this.target,1);
			%player.Station = "";
			%this.target = "";
		}
	}
	%this.enterTime="";
}

function resupply(%this) 
{
	if (GameBase::isActive(%this)) 
	{
		%player = Station::getTarget(%this);
		if (%player != -1) 
		{
			%cnt = Station::itemsToResupply(%player);
			if(getSimTime() - %this.enterTime > 11) %cnt = 0;
			%client = Player::getClient(%player);
			if (%cnt != 0) 
			{
				updateBuyingList(%client);
				return 1;
			}
			Client::sendMessage(%client,0,"Resupply Complete");
			return 0;
		}
	}
	return 0;
}

function setupShoppingList(%client,%station) 
{
// Fill the inventory station with the appropriate items
	Client::clearItemShopping(%client);
	%armor = Player::getArmor(%client);
	%max = $TotalItems;
	%ListType = GameBase::getDataName((Client::getOwnedObject(%client)).Station);
	if ($Server::RaceOption == "") $Server::RaceOption = 0; 
	%team = (client::getteam(%client)+1)%2;  // Team Specific Races
	if ($Server::RaceOption == 1) %team = 1; // Space Marines Only
	if ($Server::RaceOption == 2) %team = 0; // Eldar Only
	if ($Server::RaceOption == 3) %team = 2; // All available
	if(%ListType == InventoryStation) 
	{
		for (%i = 0; %i < %max; %i++) 
		{
			%item = getItemData(%i);
			if ($InvList[%item] && $InvList[%item] != "" && !%station.dontSell[%item]) 
			{
				if(%item.classname == Armor)
				{
					if(%team == 1)
					{
						if(%item.heading == "bEldar Troops") continue;
						Client::setItemShopping(%client, %item);
						continue;
					}
					else if(%team == 0)
					{
						if(%item.heading == "aSpace Marine Troops") continue;
						Client::setItemShopping(%client, %item);
						continue;
					}
					else					
					{
						Client::setItemShopping(%client, %item);
						continue;
					}
				}
				if($itemmax[%armor, %item] == 0 || $itemmax[%armor, %item] == "") continue;
				Client::setItemShopping(%client, %item);
			}
		}
		return;
	}
	else if(%ListType == DeployableInvStation) 
	{
		for (%i = 0; %i < %max; %i++) 
		{
			%item = getItemData(%i);
			if ($RemoteInvList[%item] != "" && $RemoteInvList[%item] && !%station.dontSell[%item]) 
			{
				if(%item.classname == Armor)
				{
					if(%team == 1)
					{
						if(%item.heading == "bEldar Troops") continue;
						Client::setItemShopping(%client, %item);
						continue;
					}
					else if(%team == 0)
					{
						if(%item.heading == "aSpace Marine Troops") continue;
						Client::setItemShopping(%client, %item);
						continue;
					}
					else					
					{
						Client::setItemShopping(%client, %item);
						continue;
					}
				}
				if($itemmax[%armor, %item] == 0 || $itemmax[%armor, %item] == "") continue;
				Client::setItemShopping(%client, %item);
			}
		}
		return;
	}
	else
	{
		for (%i = 0; %i < %max; %i = %i + 1) 
		{
			%item = getItemData(%i);
			if($VehicleInvList[%item] != "" && $VehicleInvList[%item] && !%station.dontSell[%item]) 
			Client::setItemShopping(%client, %item);
		}
	}
}

function updateBuyingList(%client) 
{
	Client::clearItemBuying(%client);
	%station = (Client::getOwnedObject(%client)).Station;
	%stationName = GameBase::getDataName(%station);
	if(%stationName == DeployableInvStation) 
	{
		%energy = %station.Energy;
		Client::setInventoryText(%client, "<f1><jc>STATION ENERGY: " @ %energy );
	}
	else 
	{
		%energy = $TeamEnergy[Client::getTeam(%client)];
		Client::setInventoryText(%client, "<f1><jc>TEAM ENERGY: " @ %energy);
	}
	%armor = Player::getArmor(%client);
	%max = $TotalItems;
	for (%i = 0; %i < %max; %i++) 
	{
		%item = getItemData(%i);
		if(!Client::isItemShoppingOn(%client,%i)) continue;
		if(%energy < %item.price) continue;
		if(%stationName == InventoryStation || %stationName == DeployableInvStation ) 
		{
			if(%item.classname == Armor)
			{
				if(%item == $ArmorName[%armor]) continue;
				Client::setItemBuying(%client, %item);
				continue;
			}
			if($ItemMax[%armor, %item] == "") continue;
			if(%item.className == Weapon) 
			{
				if($ItemMax[%armor, %item] <= Player::getItemCount(%client,%item)) continue;
				if(Player::getItemClassCount(%client,"Weapon") < $MaxWeapons[%armor]) Client::setItemBuying(%client, %item);
				continue;
			}
			if(%item.className == Tool) 
			{
				if($ItemMax[%armor, %item] <= Player::getItemCount(%client,%item)) continue;
				Client::setItemBuying(%client, %item);
				continue;
			}
			if(%item.className == Backpack) 
			{
				if($TeamItemMax[%item] != "") 
				{
					if($ItemMax[%armor, %item] <= Player::getItemCount(%client,%item)) continue;
					if($TeamItemCount[GameBase::getTeam(%client) @ %item] < $TeamItemMax[%item]) Client::setItemBuying(%client, %item);
					continue;
				}
				if($ItemMax[%armor, %item] <= Player::getItemCount(%client,%item)) continue;
				Client::setItemBuying(%client, %item);
				continue;
			}
			%extraAmmo = 0;
			if(Player::getMountedItem(%client,$BackpackSlot) ==ammopack) %extraAmmo = floor($ItemMax[%armor, %item] * $AmmoPackMult);
			if($ItemMax[%armor, %item] + %extraAmmo > Player::getItemCount(%client,%item))
			{
				Client::setItemBuying(%client, %item);
				continue;
			}
		}
		else if($TeamItemCount[client::getTeam(%client) @ %item] < $TeamItemMax[%item]) 
		{
			%vehicle = $itemToVehicle[%item];
			if(($vehicleUse[%armor, %vehicle] % 2) == 1)
				Client::setItemBuying(%client, %item);
		}
	}
}

StaticShapeData CommandStation 
{
	description = "Command Station";
	shapeFile = "cmdpnl";
	className = "Station";
	visibleToSensor = true;
	sequenceSound[0] = { "activate", SoundActivateCommandStation };
	sequenceSound[1] = { "power", SoundCommandStationPower };
	sequenceSound[2] = { "use", SoundUseCommandStation };
	maxDamage = 1.0;
	debrisId = flashDebrisMedium;
	mapFilter = 4;
	mapIcon = "M_station";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	triggerRadius = 1.5;
	explosionId = flashExpLarge;
};

function CommandStation::onEndSequence(%this,%thread) 
{
	(Client::getOwnedObject(%this.target)).Station = "";
	%this.target = "";
	if (Station::onEndSequence(%this,%thread)) CommandStation::onResupply(%this);
}

function CommandStation::onResupply(%this) 
{
	if (GameBase::isActive(%this)) 
	{
		%player = Station::getTarget(%this);
		if (%player != -1 && %this.lastPlayer == %player) 
		{
			%client = Player::getClient(%player);
			if (%this.target != %client) 
			{
				%this.target = %client;
				%player.CommandTag = 1;
				Client::setGuiMode(%client,2);
				Client::sendMessage(%client,0,"Command Access On");
			}
			schedule("CommandStation::onResupply(" @ %this @ ");",0.5,%this);
			return;
		}
		GameBase::setActive(%this,false);
	}
	if (%this.target) 
	{
		Client::sendMessage(%this.target,0,"Command Access Off");
		(Client::getOwnedObject(%this.target)).CommandTag = "";
		checkControlUnmount(%this.target);
	}
	(Client::getOwnedObject(%this.target)).Station = "";
	%this.target = "";
}

StaticShapeData VehicleStation 
{
	description = "Station Vehicle Unit";
	shapeFile = "vehi_pur_pnl";
	className = "Station";
	visibleToSensor = true;
	sequenceSound[0] = { "activate", SoundActivateInventoryStation };
	sequenceSound[1] = { "power", SoundInventoryStationPower };
	sequenceSound[2] = { "use", SoundUseInventoryStation };
	maxDamage = 1.0;
	debrisId = flashDebrisLarge;
	mapFilter = 4;
	mapIcon = "M_station";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	triggerRadius = 1.5;
	explosionId = flashExpLarge;
};

function VehicleStation::onEndSequence(%this,%thread) 
{
if (Station::onEndSequence(%this,%thread)) VehicleStation::onBuyingVechicle(%this);
}

function VehicleStation::onBuyingVechicle(%this) 
{
	if (GameBase::isActive(%this)) 
	{
		%player = Station::getTarget(%this);
		if (%player != -1 && %this.lastPlayer == %player) 
		{
			%client = Player::getClient(%player);
			if (%this.target != %client) 
			{
				setupShoppingList(%client,%this,"VehicleInvList");
				updateBuyingList(%client);
				%this.target = %client;
				%this.clTeamEnergy = %client.TeamEnergy;
				Client::setGuiMode(%client,4);
				Client::sendMessage(%client,0,"Station Access On");
				%player.Station = %this;
				%numItems = Group::objectCount(GetGroup(%this));
				for(%i = 0; %i<%numItems; %i++) 
				{
					%obj = Group::getObject(GetGroup(%this), %i);
					%name = GameBase::getDataName(%obj);
					if(%name == VehiclePad) 
					{
						%this.vehiclePad = %obj;
						GameBase::setActive(%this.vehiclePad,true);
						%i = %numItems;
					}
				}
			}
			schedule("VehicleStation::onBuyingVechicle(" @ %this @ ");",0.5,%this);
			return;
		}
		GameBase::setActive(%this,false);
	}
	if (%this.target) 
	{
		Client::clearItemShopping(%this.target);
		Client::sendMessage(%this.target,0,"Station Access Off");
		Station::onEndSequence(%this);
	}
}

function VehicleStation::checkBuying(%client,%item) 
{
	%player = Client::getOwnedObject(%client);
	%obj = %player.Station.vehiclePad;
	if(GameBase::isPowered(%obj) && GameBase::getDamageState(%obj) == "Enabled") 
	{
		%markerPos = GameBase::getPosition(%obj);
		%set = newObject("set",SimSet);
		%mask = $VehicleObjectType | $SimPlayerObjectType | $ItemObjectType;
		%objInWay = containerBoxFillSet(%set,%mask,%markerPos,6,5,14,1);
		%station = %player.Station;
		if(%objInWay == 1) 
		{
			%object = Group::getObject(%set, 0);
			%sName = GameBase::getDataName(%object);
			if(%sName.className == Vehicle) 
			{
				if(GameBase::getControlClient(%object) == -1) 
				{
					if(%station.fadeOut == "") 
					{
						if(%item != $VehicleToItem[%sname]) 
						{
							%object.fading = 1;
							%station.fadeOut=1;
							teamEnergyBuySell(%player,$VehicleToItem[%sName].price);
							$TeamItemCount[Client::getTeam(%client) @ ($VehicleToItem[%sName])]--;
							GameBase::startFadeOut(%object);
							schedule("deleteObject(" @ %object @ ");",2.5,%object);
							schedule(%object @ ".fading = \"\";",2.5,%object);
							schedule(%station @ ".fadeOut = \"\";",2.5,%station);
							%objInWay--;
						}
						else return 2;
					}
					else 
					{
						Client::SendMessage(%client,0,"ERROR - Vehicle creation pad busy");
						return 0;
					}
				}
				else 
				{
					Client::SendMessage(%client,0,"ERROR - Vehicle in creation area is mounted");
					return 0;
				}
			}
		}
		if(!%objInWay) 
		{
			if (checkResources(%player,%item,1)) 
			{
				%vehicle = newObject("",flier,$DataBlockName[%item],true);
				Gamebase::setMapName(%vehicle,%item.description);
				%vehicle.clLastMount = %client;
// Set the vehicle weapon
				if(%item.description == "Vyper") 
				{
					if (%client.vvOpt == "") %client.vvOpt = 0;
					%vehicle.weap = %client.vvOpt;
				}
				if(%item.description == "Land Speeder") 
				{
					if (%client.vlOpt == "") %client.vlOpt = 0;
					%vehicle.weap = %client.vlOpt;
				}
// End Set
				addToSet("MissionCleanup", %vehicle);
				%vehicle.fading = 1;
				GameBase::setTeam(%vehicle,Client::getTeam(%client));
				if(%object.fading) 
				{
					schedule("GameBase::startFadeIn(" @ %vehicle @ ");",2.5,%vehicle);
					schedule("GameBase::setPosition(" @ %vehicle @ ",\"" @ %markerPos @ "\");",2.5,%vehicle);
					schedule("GameBase::setRotation(" @ %vehicle @ ",\"" @ GameBase::getRotation(%obj) @ "\");",2.5,%vehicle);
					schedule(%vehicle @ ".fading = \"\";VehiclePad::checkSeq(" @ %obj @ "," @ %player.Station @ ");",5,%vehicle);
					%obj.busy = getSimTime() + 5;
				}
				else 
				{
					GameBase::startFadeIn(%vehicle);
					GameBase::setPosition(%vehicle,%markerPos);
					GameBase::setRotation(%vehicle,GameBase::getRotation(%obj));
					schedule(%vehicle @ ".fading = \"\";VehiclePad::checkSeq(" @ %obj @ "," @ %player.Station @ ");",3,%vehicle);
					%obj.busy = getSimTime() + 3;
				}
				deleteObject(%set);
				$TeamItemCount[Client::getTeam(%client) @ %item]++;
				return 1;
			}
		}
		else Client::SendMessage(%client,0,"ERROR - Object in vehicle creation area");
		deleteObject(%set);
	}
	else Client::SendMessage(%client,0,"ERROR - Vehicle Pad Disabled");
	return 0;
}

StaticShapeData VehiclePad 
{
	description = "Vehicle Pad";
	shapeFile = "vehi_pur_poles";
	className = "Station";
	visibleToSensor = true;
	sequenceSound[0] = { "activate", SoundActivateInventoryStation };
	sequenceSound[1] = { "power", SoundInventoryStationPower };
	sequenceSound[2] = { "use", SoundUseInventoryStation };
	maxDamage = 3.0;
	debrisId = flashDebrisLarge;
	mapFilter = 4;
	mapIcon = "M_station";
	explosionId = flashExpLarge;
	damageSkinData = "objectDamageSkins";
};

function VehiclePad::onActivate(%this) 
{
	GameBase::playSequence(%this,1,"use");
}

function VehiclePad::onDeactivate(%this) 
{
	GameBase::stopSequence(%this,1);
}

function VehiclePad::onEnabled(%this) 
{ }

function VehiclePad::onAdd(%this) 
{ }

function VehiclePad::onCollision(%this, %object) 
{ }

function VehiclePad::onPower(%this,%power,%generator) 
{
	if(!%power) GameBase::setActive(%this,false);
}

function VehiclePad::checkSeq(%this, %station) 
{
	if(%station.target == "") GameBase::setActive(%this,false);
}
