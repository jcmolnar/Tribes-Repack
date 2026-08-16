//----------------------------------------------------------------------------
// Default station animations

function Station::onActivate(%this)
{
	//echo("Activate " @ %this);
	%obj = Station::getTarget(%this);
	if (%obj != -1) {
                GameBase::playSequence(%this,1,"none");
		GameBase::setSequenceDirection(%this,1,1);
	}
	else 
		GameBase::setActive(%this,false);
}

function Station::onDeactivate(%this)
{
	//echo("Dectivate " @ %this);
	GameBase::stopSequence(%this,2);
	GameBase::setSequenceDirection(%this,1,0);
}

function Station::onEndSequence(%this,%thread)
{
	//echo("End sequence " @ %this);
 	if (%thread == 1 && GameBase::isActive(%this)) {
                GameBase::playSequence(%this,2,"none");
		return true;
	}
	%clientId = %this.target;
	if(%clientId == "") {
		%player = Station::getTarget(%this);
		%clientId = Player::getClient(%player);
	}
	if(%clientId != "") {
		if(Client::getGuiMode(%clientId) != 1)
			Client::setGuiMode(%clientId,1);
		
		%team = Client::getTeam(%clientId);
		if($TeamEnergy[%team] != "Infinite") {
			if(%this.clTeamEnergy != %clientId.TeamEnergy) {
				if(%clientId.teamEnergy < 0)
					Client::sendMessage(%clientId,0,"Your total mission purchases have come to " @ (%clientId.teamEnergy * -1) @ ".");
				else
					Client::sendMessage(%clientId,0,"You have increased the Team Energy by " @ %clientId.teamEnergy @ ".");
			}
			if((%clientId.teamEnergy -%clientId.EnergyWarning < $TeammateSpending) && ($TeammateSpending != 0) && !$TeamEnergyCheat) {
				TeamMessages(0, %team, "Teammate " @ Client::getName(%clientId) @ " has spent " @ (%clientId.teamEnergy *-1) @ " of the TeamEnergy"); 
				%clientId.EnergyWarning = %clientId.teamEnergy;
			}
			if($TeamEnergy[%team] < $WarnEnergyLow)  
				TeamMessages(0, %team, "TeamEnergy Low: " @ $TeamEnergy[%team]); 
		}
	}
	if(%this.target != "") {
		(Client::getOwnedObject(%this.target)).Station = "";
		%this.target = "";
	}
	if(GameBase::getDataName(%this) == VehicleStation && %this.vehiclePad.busy < getSimTime())
		VehiclePad::checkSeq(%this.vehiclePad, %this);
	%this.clTeamEnergy = "";
	return false;
}

function Station::onPower(%this,%power,%generator)
{
	if (%power) {
                GameBase::playSequence(%this,0,"none");
		GameBase::playSequence(%this,1);
	}
	else {
		GameBase::stopSequence(%this,0);
		GameBase::pauseSequence(%this,1);
		GameBase::pauseSequence(%this,2);
		Station::checkTarget(%this);
	}
}

function Station::onEnabled(%this)
{
	if (GameBase::isPowered(%this)) {		
                GameBase::playSequence(%this,0,"none");
		GameBase::playSequence(%this,1);
	}
}

function Station::checkTarget(%this)
{
	if(%this.target) {
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
	calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 2, 0.40, 
		0.1, 250, 100); 
}

function Station::getTarget(%this)
{
	if(GameBase::getLOSInfo(%this,1.5,"0 0 3.14")) {
	  	// GetLOSInfo sets the following globals:
	  	// 	los::position
	  	// 	los::normal
	  	// 	los::object
	  	%obj = getObjectType($los::object);
		dbecho(3, "STATION: LOS got " @ %obj);
	  	if (%obj == "Player") {
         if( Player::isAiControlled( $los::object ) != "True" ) {
			   return $los::object;
         }
		}
	}
	dbecho(3, "STATION: LOS Got None");
	return -1;
}	

function Station::onCollision(%this, %object)
{
	dbecho(3, "STATION: Collision (" @ %this @ "," @ %object @ ")");
	%obj = getObjectType(%object);
	if (%obj == "Player") {
  	 	%clientId = Player::getClient(%object);
 		if(GameBase::getTeam(%object) == GameBase::getTeam(%this) || GameBase::getTeam(%this) == -1) {
			if (GameBase::getDamageState(%this) == "Enabled") {
				if (GameBase::isPowered(%this)) { 
					if(%this.enterTime == "")
						%this.enterTime = getSimTime();
					GameBase::setActive(%this,true);
				}
				else 
					Client::sendMessage(%clientId,0,"Unit is not powered");
			}
			else 
				Client::sendMessage(%clientId,0,"Unit is disabled");
		}
		else if(Station::getTarget(%this) == %object)
      {
			%curTime = getSimTime();
			if(%curTime - %object.stationDeniedStamp > 3.5 && GameBase::getDamageState(%this) == "Enabled") {
				Client::clearItemShopping(%clientId);
				Station::onDeactivate(%this);
				Station::onEndSequence(%this,1);
				if(Client::getGuiMode(%clientId) != 1)
					Client::setGuiMode(%clientId,1);
				%object.stationDeniedStamp = %curTime;
                                Client::sendMessage(%clientId,0,"--ACCESS DENIED--~waccess_denied.wav");
			}
		}
	}
}

function Station::itemsToResupply(%player)
{
	%cnt = 0;
	//%cnt = %cnt + AmmoStation::resupply(%player,"",RepairPatch,1);
	return %cnt;
}

//----------------------------------------------------------------------------

StaticShapeData AmmoStation
{
   description = "Ammo Supply Unit";
	shapeFile = "ammounit";
	className = "Station";
	visibleToSensor = true;
        maxDamage = 999999999999.0;
	debrisId = flashDebrisLarge;
	mapFilter = 4;
	mapIcon = "M_station";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
   explosionId = flashExpLarge;
};

function AmmoStation::onEndSequence(%this,%thread)
{
	//echo("End Seq ",%thread);
	if(%this.clTeamEnergy == "")
		%this.clTeamEnergy = (Player::getClient(Station::getTarget(%this))).TeamEnergy;
	if (Station::onEndSequence(%this,%thread)) 
		AmmoStation::onResupply(%this);
}									
											
function AmmoStation::onResupply(%this)
{
	if (GameBase::isActive(%this)) {
		%player = Station::getTarget(%this);
		if (%player != -1) {
			// Hardcoded here for the ammo types
			%cnt = Station::itemsToResupply(%player);
			if(getSimTime() - %this.enterTime > 11)
				%cnt = 0;
			if (%cnt != 0) {
				schedule("AmmoStation::onResupply(" @ %this @ ");",0.5,%this);
				return;
			}
			%clientId = Player::getClient(%player);
			Client::sendMessage(%clientId,0,"Resupply Complete");
			Client::setInventoryText(%clientId, "<f1><jc>TEAM ENERGY: " @ $TeamEnergy[Client::getTeam(%clientId)]);
		}
		GameBase::setActive(%this,false);
		%this.enterTime="";
	}
}
		 											
function AmmoStation::resupply(%player,%weapon,%item,%delta)
{
	%delta = checkResources(%player,%item,%delta,1);		
	if(%delta > 0) {						
		if(%item == RepairPatch) {
                  BuySell(%player,%item.price * %delta * -1);
			refreshHP(Player::getClient(%player), -0.25);
		 	return %delta;
		}
		else if (Player::getItemCount(%player,%weapon)) {
                  BuySell(%player,%item.price * %delta * -1);
			Player::incItemCount(%player,%item,%delta);
		 	return %delta;
		}
	}
	return 0;
}

//----------------------------------------------------------------------------

StaticShapeData InventoryStation
{
   description = "Station Supply Unit";
	shapeFile = "inventory_sta";
	className = "Station";
	visibleToSensor = true;
        maxDamage = 999999999999.0;
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
	//echo("End Seq ",%thread);
	if (Station::onEndSequence(%this,%thread)) 
		InventoryStation::onResupply(%this,"InvList");
}

function InventoryStation::onResupply(%this,%InvShopList)
{
	dbecho(3, "STATION::Resupply");
	if (GameBase::isActive(%this)) {
		%player = Station::getTarget(%this);
		if (%player != -1) {
			%clientId = Player::getClient(%player);
			if (%this.target != %clientId) {
				%player.Station = %this;
				setupShoppingList(%clientId,%this,%InvShopList);
				updateBuyingList(%clientId);
				%this.target = %clientId;
				%this.clTeamEnergy = %clientId.TeamEnergy;
            if(!%clientId.noEnterInventory)
   				Client::setGuiMode(%clientId,$GuiModeInventory);
				Client::sendMessage(%clientId,0,"Station Access On");
				%player.ResupplyFlag = 1;
			}
			schedule("InventoryStation::onResupply(" @ %this @ ");",0.5,%this);
			if(%player.ResupplyFlag) 
			   %player.ResupplyFlag = resupply(%this);
			return;
		}
		GameBase::setActive(%this,false);
	}
	if (%this.target) {	   
		%player = Client::getOwnedObject(%this.target);
		Client::clearItemShopping(%this.target);
		Client::sendMessage(%this.target,0,"Station Access Off");
		Station::onEndSequence(%this);
		if(GameBase::getDataName(%player.Station) == DeployableInvStation) {
			Client::setInventoryText(%this.target, "<f1><jc>TEAM ENERGY: " @ $TeamEnergy[Client::getTeam(%this.target)]);
			if(Client::getGuiMode(%this.target) != 1)
				Client::setGuiMode(%this.target,1);
			%player.Station = "";
			%this.target = "";
		}
	}
	%this.enterTime="";
}


function resupply(%this)
{
	if (GameBase::isActive(%this)) {
		%player = Station::getTarget(%this);
		if (%player != -1) {
			// Hardcoded here for the ammo types
			%cnt = Station::itemsToResupply(%player);
			if(getSimTime() - %this.enterTime > 11)
				%cnt = 0;
			%clientId = Player::getClient(%player);
			if (%cnt != 0) {
				updateBuyingList(%clientId);
				return 1;
			}
			Client::sendMessage(%clientId,0,"Resupply Complete");
			return 0;
		}
	}
	return 0;
}


//----------------------------------------------------------------------------

//----------------------------------------------------------------------------
StaticShapeData CommandStation
{
   description = "Command Station";
	shapeFile = "cmdpnl";
	className = "Station";
	visibleToSensor = true;
        maxDamage = 999999999999.0;
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
	//echo("End Seq ",%thread);
	(Client::getOwnedObject(%this.target)).Station = "";
	%this.target = "";
	if (Station::onEndSequence(%this,%thread)) 
		CommandStation::onResupply(%this);
}

function CommandStation::onResupply(%this)
{
	if (GameBase::isActive(%this)) {
		%player = Station::getTarget(%this);
		if (%player != -1) {
			%clientId = Player::getClient(%player);
			if (%this.target != %clientId) {
				%this.target = %clientId;
				%player.CommandTag = 1;
				Client::setGuiMode(%clientId,2);
				Client::sendMessage(%clientId,0,"Command Access On");
			}
			schedule("CommandStation::onResupply(" @ %this @ ");",0.5,%this);
			return;
		}
		GameBase::setActive(%this,false);
	}
	if (%this.target) {
		Client::sendMessage(%this.target,0,"Command Access Off");
		(Client::getOwnedObject(%this.target)).CommandTag = "";
		checkControlUnmount(%this.target);
	}
	(Client::getOwnedObject(%this.target)).Station = "";
	%this.target = "";
}


//----------------------------------------------------------------------------
StaticShapeData VehicleStation
{
   description = "Station Vehicle Unit";
	shapeFile = "vehi_pur_pnl";
	className = "Station";
	visibleToSensor = true;
//   explosionId = DebrisExp;
        maxDamage = 999999999999.0;
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
	//echo("End Seq ",%thread);
	if (Station::onEndSequence(%this,%thread)) 
		VehicleStation::onBuyingVechicle(%this);
}

function VehicleStation::onBuyingVechicle(%this)
{
	if (GameBase::isActive(%this)) {
		%player = Station::getTarget(%this);
		if (%player != -1) {
			%clientId = Player::getClient(%player);
			if (%this.target != %clientId) {
				setupShoppingList(%clientId,%this,"VehicleInvList");
				updateBuyingList(%clientId);
				%this.target = %clientId;
				%this.clTeamEnergy = %clientId.TeamEnergy;
				Client::setGuiMode(%clientId,4);
				Client::sendMessage(%clientId,0,"Station Access On");
				%player.Station = %this;
			 	%numItems = Group::objectCount(GetGroup(%this));
				for(%i = 0 ; %i<%numItems ; %i++) { 
					%obj = Group::getObject(GetGroup(%this), %i);
					%name = GameBase::getDataName(%obj); 
					if(%name == VehiclePad) { 
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
	if (%this.target) {	   
		Client::clearItemShopping(%this.target);
		Client::sendMessage(%this.target,0,"Station Access Off");
		Station::onEndSequence(%this);
	}
}


function VehicleStation::checkBuying(%clientId,%item)
{
	%player = Client::getOwnedObject(%clientId);
	%obj = %player.Station.vehiclePad;
	if(GameBase::isPowered(%obj) && GameBase::getDamageState(%obj) == "Enabled") {
		%markerPos = GameBase::getPosition(%obj);
  		%set = newObject("set",SimSet);
		%mask = $VehicleObjectType | $SimPlayerObjectType | $ItemObjectType;
		%objInWay = containerBoxFillSet(%set,%mask,%markerPos,6,5,14,1);
		%station = %player.Station;
		if(%objInWay == 1) {
			%object = Group::getObject(%set, 0);	
			%sName = GameBase::getDataName(%object);
			if(%sName.className == Vehicle) {
				if(GameBase::getControlClient(%object) == -1) {
					if(%station.fadeOut == "") {
						if(%item != $VehicleToItem[%sname]) {
							%object.fading = 1;
							%station.fadeOut=1;
                                          BuySell(%player,$VehicleToItem[%sName].price);
							$TeamItemCount[Client::getTeam(%clientId) @ ($VehicleToItem[%sName])]--;
							GameBase::startFadeOut(%object);
							schedule("deleteObject(" @ %object @ ");",2.5,%object);
							schedule(%object @ ".fading = \"\";",2.5,%object);
							schedule(%station @ ".fadeOut = \"\";",2.5,%station);
							%objInWay--;
						}
						else
							return 2;
					}
					else {
						Client::SendMessage(%clientId,0,"ERROR - Vehicle creation pad busy"); 
						return 0;
					}
				}
				else { 
					Client::SendMessage(%clientId,0,"ERROR - Vehicle in creation area is mounted");
					return 0;
				}
			} 
		}
		if(!%objInWay)
		{
			if (checkResources(%player,%item,1))
			{	//bought vehicle
				%vehicle = newObject("",flier,$DataBlockName[%item],true);
				echo("bla: " @ %item.description);

				//**RPG
				$owner[%vehicle] = Client::getName(%clientId);
				//**

				Gamebase::setMapName(%vehicle,%item.description);
				%vehicle.clLastMount = %clientId;
				addToSet("MissionCleanup", %vehicle);
			  	%vehicle.fading = 1;
				GameBase::setTeam(%vehicle,Client::getTeam(%clientId));
				if(%object.fading) { 
					schedule("GameBase::startFadeIn(" @ %vehicle @ ");",2.5,%vehicle);
					schedule("GameBase::setPosition(" @ %vehicle @ ",\"" @ %markerPos @ "\");",2.5,%vehicle);
					schedule("GameBase::setRotation(" @ %vehicle @ ",\"" @ GameBase::getRotation(%obj) @ "\");",2.5,%vehicle);
					schedule(%vehicle @ ".fading = \"\"; VehiclePad::checkSeq(" @ %obj @ "," @ %player.Station @ ");",5,%vehicle);
					%obj.busy = getSimTime() + 5;
				}
				else {
					GameBase::startFadeIn(%vehicle);
					GameBase::setPosition(%vehicle,%markerPos);
					GameBase::setRotation(%vehicle,GameBase::getRotation(%obj));
				 	schedule(%vehicle @ ".fading = \"\"; VehiclePad::checkSeq(" @ %obj @ "," @ %player.Station @ ");",3,%vehicle);
					%obj.busy = getSimTime() + 3;
				}
				deleteObject(%set);
				$TeamItemCount[Client::getTeam(%clientId) @ %item]++;
				return 1;
			}
		}
		else
			Client::SendMessage(%clientId,0,"ERROR - Object in vehicle creation area");
		deleteObject(%set);
	}	
	else
		Client::SendMessage(%clientId,0,"ERROR - Vehicle Pad Disabled");

	return 0;
}


StaticShapeData VehiclePad
{
   description = "Vehicle Pad";
	shapeFile = "vehi_pur_poles";
	className = "Station";
	visibleToSensor = true;
        maxDamage = 999999999999.0;
	debrisId = flashDebrisLarge;
	mapFilter = 4;
	mapIcon = "M_station";
   explosionId = flashExpLarge;
	damageSkinData = "objectDamageSkins";
};



function VehiclePad::onActivate(%this)
{
        GameBase::playSequence(%this,1,"none");
}

function VehiclePad::onDeactivate(%this)
{
	GameBase::stopSequence(%this,1);
}

function VehiclePad::onEnabled(%this)
{
}

function VehiclePad::onAdd(%this)
{
}

function VehiclePad::onCollision(%this, %object)
{
}

function VehiclePad::onPower(%this,%power,%generator)
{
	if(!%power)
		GameBase::setActive(%this,false);
}

function VehiclePad::checkSeq(%this, %station)
{
	if(%station.target == "")
		GameBase::setActive(%this,false);
}
