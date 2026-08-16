//----------------------------------------------------------------------------

$ItemFavoritesKey = "RPG";  // Change this if you add new items
                         // and don't want to mess up everyone's
                         // favorites - just put in something
                         // that uniquely describes your new stuff.

//----------------------------------------------------------------------------

$ItemPopTime = 30;

$ToolSlot=0;
$WeaponSlot=0;
$BackpackSlot=1;
$FlagSlot=2;
$DefaultSlot=3;

// Limit on number of special Items you can buy
$TeamItemMax[DepBasePack] = 128;

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
// Default Item object methods
//----------------------------------------------------------------------------

$PickupSound[Ammo] = "SoundPickupAmmo";
$PickupSound[Weapon] = "SoundPickupWeapon";
$PickupSound[Backpack] = "SoundPickupBackpack";
$PickupSound[Repair] = "SoundPickupHealth";
$PickupSound[Accessory] = "SoundPickupBackpack";

function Item::playPickupSound(%this)
{
	dbecho($dbechoMode, "Item::playPickupSound(" @ %this @ ")");

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
	dbecho($dbechoMode, "Item::respawn(" @ %this @ ")");

	// If the item is rotating we respawn it,
	if(Item::isRotating(%this)) {
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



//----------------------------------------------------------------------------
// Default Inventory methods


function Item::pop(%item)
{
	dbecho($dbechoMode, "Item::pop(" @ %item @ ")");

 	GameBase::startFadeOut(%item);
	schedule("deleteObject(" @ %item @ ");",2.5, %item);
}


//----------------------------------------------------------------------------
// Tools, Weapons & ammo
//----------------------------------------------------------------------------

ItemData Tool
{
	description = "Tool";
	showInventory = false;
};

function Tool::onUse(%player,%item)
{
	dbecho($dbechoMode, "Tool::onUse(" @ %player @ ", " @ %item @ ")");

	Player::mountItem(%player,%item,$ToolSlot);
}


//----------------------------------------------------------------------------

ItemData Ammo
{
	description = "Ammo";
	showInventory = false;
};

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
	dbecho($dbechoMode, "Backpack::onUse(" @ %player @ ", " @ %item @ ")");

	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::trigger(%player,$BackpackSlot);
	}
}

function checkDeployArea(%clientId, %pos)
{
	dbecho($dbechoMode, "checkDeployArea(" @ %clientId @ ", " @ %pos @ ")");

  	%set=newObject("set",SimSet);
	%num=containerBoxFillSet(%set,$StaticObjectType | $ItemObjectType | $SimPlayerObjectType,%pos,1,1,1,1);
	if(!%num) {
		deleteObject(%set);
		return 1;
	}
	else if(%num == 1 && getObjectType(Group::getObject(%set,0)) == "Player") { 
		%obj = Group::getObject(%set,0);	
		if(Player::getClient(%obj) == %clientId)	
			Client::sendMessage(%clientId, 0, "Unable to deploy - You're in the way");
		else
			Client::sendMessage(%clientId, 0, "Unable to deploy - Player in the way");
	}
	else
		return 1;

	//	Client::sendMessage(%clientId, 0, "Unable to deploy - Item in the way");

	deleteObject(%set);
	return 0;	
		

}
//----------------------------------------------------------------------------
// Remote deploy for items

function Item::deployShape(%player,%name,%shape,%item)
{
	dbecho($dbechoMode, "Item::deployShape(" @ %player @ ", " @ %name @ ", " @ %shape @ ", " @ %item @ ")");

	%clientId = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]) {
		if (GameBase::getLOSInfo(%player,3)) {
			// GetLOSInfo sets the following globals:
			// 	los::position
			// 	los::normal
			// 	los::object
			%obj = getObjectType($los::object);
			if (%obj == "SimTerrain" || %obj == "InteriorShape") {
				if (Vector::dot($los::normal,"0 0 1") > 0.7) {
					if(checkDeployArea(%clientId, $los::position)) {
						if(Zone::getType($zone[%clientId]) != "PROTECTED" || %clientId.adminLevel >= 4)
						{
							%sensor = newObject("","Sensor",%shape,true);
			 	        	   	addToSet("MissionCleanup", %sensor);
							GameBase::setTeam(%sensor,GameBase::getTeam(%player));
							GameBase::setPosition(%sensor,$los::position);
							Gamebase::setMapName(%sensor,%name);
							Client::sendMessage(%clientId, 0, %item.description @ " deployed");
							playSound(SoundPickupBackpack,$los::position);
							echo("MSG: ",%clientId," deployed a ",%name);
							return true;
						}
						else 
							Client::sendMessage(%clientId,0,"You are not allowed to deploy this item inside protected territory.");
					}
				}
				else 
					Client::sendMessage(%clientId,0,"Can only deploy on flat surfaces");
			}
			else 
				Client::sendMessage(%clientId,0,"Can only deploy on terrain or buildings");
		}
		else 
			Client::sendMessage(%clientId,0,"Deploy position out of range");
	}
	else
	 	Client::sendMessage(%clientId,0,"Deployable Item limit reached for " @ %name @ "s");
	return false;
}

//----------------------------------------------------------------------------

ItemData RepairPatch
{
	description = "Repair Patch";
	className = "Repair";
	shapeFile = "armorPatch";
	heading = "eMiscellany";
	shadowDetailMask = 4;
  	price = 1;
};

function RepairPatch::onCollision(%this,%object)
{
	dbecho($dbechoMode, "RepairPatch::onCollision(" @ %this @ ", " @ %object @ ")");

	if (getObjectType(%object) == "Player") {
		if(GameBase::getDamageLevel(%object)) {
			refreshHP(Player::getClient(%object), -0.125);
			%item = Item::getItemData(%this);
			Item::playPickupSound(%this);
			Item::respawn(%this);
		}
	}
}

function RepairPatch::onUse(%player,%item)
{
	dbecho($dbechoMode, "RepairPatch::onUse(" @ %player @ ", " @ %item @ ")");

	Player::decItemCount(%player,%item);
	refreshHP(Player::getClient(%player), -0.1);
}
