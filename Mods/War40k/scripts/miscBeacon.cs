
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Beacon (Beacon)
//  By Dynamix, Mephisto, Alazane, and Mjolnir
//    see Contrib.txt
//
//  For installation information, see Install.txt
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//  Depends on:
//    Only Alliance armor being used (onBeacon support)
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$SellAmmo[Beacon] = 5;
$TeamItemMax[Beacon] = 40;
$InvList[Beacon] = 1;
$RemoteInvList[Beacon] = 1;

addAmmo(Misc, Beacon, 1);

function miscBeacon::Initialize()
{
  $TeamItemCount[0 @ Beacon] = 0;
  $TeamItemCount[1 @ Beacon] = 0;
  $TeamItemCount[2 @ Beacon] = 0;
  $TeamItemCount[3 @ Beacon] = 0;
  $TeamItemCount[4 @ Beacon] = 0;
  $TeamItemCount[5 @ Beacon] = 0;
  $TeamItemCount[6 @ Beacon] = 0;
  $TeamItemCount[7 @ Beacon] = 0;
}

ItemData Beacon 
{
  description = "Beacon";
  shapeFile = "sensor_small";
  heading = $InvHead[ihMis];
  shadowDetailMask = 4;
  price = 1;
  className = "HandAmmo";
};

StaticShapeData DefaultBeacon
{
  className = "Beacon";
  damageSkinData = "objectDamageSkins";

  shapeFile = "sensor_small";
  maxDamage = 0.1;
  maxEnergy = 200;

  castLOS = true;
  supression = false;
  mapFilter = 2;
  //mapIcon = "M_marker";
  visibleToSensor = true;
  explosionId = flashExpSmall;
  debrisId = flashDebrisSmall;
};
																						 
function Beacon::onEnabled(%this)
{
  GameBase::setIsTarget(%this,true);
  %data = GameBase::getDataName(%this);
  schedule("GameBase::setDamageLevel(" @ %this @ "," @ %data.maxDamage @ ");", 200);
}

function Beacon::onDisabled(%this)
{
  GameBase::setIsTarget(%this,false);
}

function Beacon::onDestroyed(%this)
{
  GameBase::setIsTarget(%this,false);
  $TeamItemCount[GameBase::getTeam(%this) @ "Beacon"]--;
}

function Beacon::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
  if(GameBase::getTeam(%this) == GameBase::getTeam(%object)) 
    return;

  %damageLevel = GameBase::getDamageLevel(%this);
  %dValue = %damageLevel + %value;
  %this.lastDamageObject = %object;
  %this.lastDamageTeam = GameBase::getTeam(%object);
  if (GameBase::getTeam(%this) == GameBase::getTeam(%object)) 
  {
    %name = GameBase::getDataName(%this);
    if (%name.className == Generator || %name.className == Station) 
    {
      %TDS = $Server::TeamDamageScale;
      %dValue = %damageLevel + %value * %TDS;
      %disable = GameBase::getDisabledDamage(%this);
      if (!$Server::TourneyMode && %dValue > %disable - 0.05) 
      {
        if (%damageLevel > %disable - 0.05)
          return;
        else
          %dValue = %disable - 0.05;
      }
    }
  }
  GameBase::setDamageLevel(%this,%dValue);
  %damageLevel = GameBase::getDamageLevel(%this);
  %this.mindamage = %damageLevel;
}

function Beacon::onUse(%player,%item) 
{
  if (!$matchStarted) return;
  %armor = Player::getArmor(%player);
  eval(%armor @ "::onBeacon(" @ %player @ ", " @ %item @ ");");
}

function Beacon::deployShape(%player,%item)
{
	// This is the original code for deploying a beacon.  An armor class does not have to use
        // a call back to this code in its onBeacon event, but it may.
        //
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

 // This can most likely be ripped

ItemData RepairPatch 
{
  description = "Repair Patch";
  className = "Repair";
  shapeFile = "armorPatch";
  heading = $InvHead[ihMis];
  shadowDetailMask = 4;
  price = 2;
};

function RepairPatch::onCollision(%this,%object) 
{
  if (getObjectType(%object) == "Player") 
  {
    if(GameBase::getDamageLevel(%object)) 
    {
      GameBase::repairDamage(%object,0.125);
      %c = Player::getClient(%object);
      $poisonTime[%c] = 0;
      %item = Item::getItemData(%this);
      Item::playPickupSound(%this);
      Item::respawn(%this);
    }
  }
}

function RepairPatch::onUse(%player,%item) 
{
  Player::decItemCount(%player,%item);
  GameBase::repairDamage(%player,0.1);
}


//-=-=-=-=-=-=-=-=-=
// Special Abilities
//-=-=-=-=-=-=-=-=-=
//-=-=-=-=-=-=-=--=-
// Tech Repair Touch
//-=-=-=-=-=-=-=-=-=
function Repair(%targetobject, %sourcePlayer)
{
	if (Player::isDead(%sourcePlayer)) return;
	if (GameBase::getDamageLevel(%targetobject))
	{
		GameBase::repairDamage(%targetPlayer, 0.50);
		GameBase::playSound(%targetobject, ForceFieldOpen,0);
	}
}
//=-=-=-=-=-=-=-=-
//Apoth Heal Touch
//-=-=-=-=-=-=-=-=
function Repair2(%targetPlayer, %sourcePlayer)
{
	if (Player::isDead(%sourcePlayer)) return;
	if (GameBase::getDamageLevel(%targetPlayer))
	{
		GameBase::repairDamage(%targetPlayer, 0.95);
		GameBase::playSound(%targetPlayer, ForceFieldOpen,0);
//Sniper Leg Hit Effect Removal
		Player::decItemCount(%this, DeadWeight);
	}
}
//-=-=-=-=-=-=-=
// Warlock Leech
//-=-=-=-=-=-=-=
function Leech(%player,%item) 
{
	Client::sendMessage(Player::getClient(%player),1, "You leech psionic energy from around you. You have regained Psi.");
	GameBase::setEnergy(%player,800);
	Player::decItemCount(%player,%item);
}

//-=-=-=--=-=-=-=-=-
// Eversor Touch
//-=-=-=-=-=-=-=-=-=
function Drain(%damagedPlayer, %damagingPlayer)
{
	if (GameBase::getTeam(%damagedPlayer) == GameBase::getTeam(%damagingPlayer) || Player::isDead(%damagingPlayer)) return;
	GameBase::applyDamage(%damagedPlayer,$ChemDamageType,0.2,GameBase::getPosition(%damagedPlayer),"0 0 0","0 0 0",%damagingPlayer);
	GameBase::setEnergy(%damagedPlayer, GameBase::getEnergy(%damagedPlayer) - 60);
	%lev = GameBase::getDamageLevel(%damagingPlayer);
	if (%lev <0.2) GameBase::setDamageLevel(%damagingPlayer, 0);
	else GameBase::setDamageLevel(%damagingPlayer,%lev-0.2);
	GameBase::setEnergy(%damagingPlayer, GameBase::getEnergy(%damagingPlayer) + 120);
	GameBase::playSound(%damagedPlayer,ForceFieldOpen,0);
	Client::sendMessage(Player::getClient(%damagingPlayer), 1, "You poison " @ Client::getName(Player::getClient(%damagedPlayer)) @ "with biotoxin!");
}

//-=-=-=-=-=-=-=-=-=-=-=
// Shield
//-=-=-=-=-=-=-==-=-=-=-
function startShield(%clientId, %player) 
{ 
	Client::sendMessage(%clientId,0,"Breaching Shield Activated");
	GameBase::playSound(%player,ForceFieldOpen,0);
	%player.shieldStrength = 0.50;
	if($shieldTime[%clientId] == 0) 
	{
		$shieldTime[%clientId] = 4;
		checkPlayerShield(%clientId, %player);
	}
	else $shieldTime[%clientId] = 4;
}

// Taken straight from Renegades
function checkPlayerShield(%clientId, %player) 
{ 
	if ($shieldTime[%clientId] > 0&& !Player::isDead(%player)) 
	{
		$shieldTime[%clientId] -= 2;
		schedule("checkPlayerShield(" @ %clientId @ ", " @ %player @ ");",2,%player);
	}
	else 
	{
		$shieldTime[%clientId] = 0;
		Client::sendMessage(%clientId,0,"Breaching Shield Exhausted.");
		%player.shieldStrength = 0.0;
		GameBase::playSound(%player,ForceFieldOpen,0);
	}
}

//-=-==-=-=-=-=-=-=
// Cloaking beacon
//-=-=-==-=-=-=-==-
function startCloak(%clientId, %player) 
{ 
	%armor = Player::getArmor(%player);
	Client::sendMessage(%clientId,0,"Cloaking On");
	GameBase::playSound(%player,ForceFieldOpen,0);
	GameBase::startFadeout(%player);
	%rate = Player::getSensorSupression(%player) + 3;
	Player::setSensorSupression(%player,%rate);
	if($cloakTime[%clientId] == 0) 
	{
		$cloakTime[%clientId] = 30;
		checkPlayerCloak(%clientId, %player);
	}
	else $cloakTime[%clientId] = 30;
}

function checkPlayerCloak(%clientId, %player) 
{
	%armor = Player::getArmor(%player);
	if (!Player::isDead(%player) && $cloakTime[%clientId] > 0) 
	{
		$cloakTime[%clientId] -= 2;
		schedule("checkPlayerCloak(" @ %clientId @ ", " @ %player @ ");",2,%player);
	}
	else 
	{
		$cloakTime[%clientId] = 0;
		Client::sendMessage(%clientId,0,"Cloaking Off");
		GameBase::playSound(%player,ForceFieldOpen,0);
		GameBase::startFadein(%player);
		%rate = Player::getSensorSupression(%player) - 5;
		Player::setSensorSupression(%player,0);
	}
}

//-=-=-=-=-=-=-=-=-=-=
// Disarmer
//-=-=-=-=-=-=-=-=-=-=
//Player::trigger(%player, $WeaponSlot, false);
	//	%weaponType = Player::getMountedItem(%player, $WeaponSlot);
	//      if(%weaponType != -1) Player::dropItem(%player, %weaponType);
	//	return;
