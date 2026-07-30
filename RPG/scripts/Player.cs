$PlayerAnim::Crouching = 25;
$PlayerAnim::DieChest = 26;
$PlayerAnim::DieHead = 27;
$PlayerAnim::DieGrabBack = 28;
$PlayerAnim::DieRightSide = 29;
$PlayerAnim::DieLeftSide = 30;
$PlayerAnim::DieLegLeft = 31;
$PlayerAnim::DieLegRight = 32;
$PlayerAnim::DieBlownBack = 33;
$PlayerAnim::DieSpin = 34;
$PlayerAnim::DieForward = 35;
$PlayerAnim::DieForwardKneel = 36;
$PlayerAnim::DieBack = 37;

// Player & Armor data block callbacks

function Player::onAdd(%this)
{
	dbecho($dbechoMode, "Player::onAdd(" @ %this @ ")");

	//reset the player's recharge rates for HP and MANA
      GameBase::setRechargeRate(%this, 0);
	GameBase::setAutoRepairRate(%this, 0);
}

function Player::onRemove(%this)
{
	//do nothing
	//look into this for possible SaveCharacter?
}

function Player::onNoAmmo(%player,%imageSlot,%itemType)
{
	//echo("No ammo for weapon ",%itemType.description," slot(",%imageSlot,")");
}


function radnomItems(%num, %an0, %an1, %an2, %an3, %an4, %an5, %an6)
{
	return %an[floor(getRandom() * (%num - 0.01))];
}

function Player::onCollision(%this,%object)
{
	dbecho($dbechoMode, "Player::onCollision(" @ %this @ ", " @ %object @ ")");

	%clientId = Player::getClient(%object);
}

function Player::getHeatFactor(%this)
{
	dbecho($dbechoMode, "Player::getHeatFactor(" @ %this @ ")");

        // Hack to avoid turret turret not tracking vehicles.
        // Assumes that if we are not in the player we are
        // controlling a vechicle, which is not always correct
        // but should be OK for now.
        %clientId = Player::getClient(%this);
        if (Client::getControlObject(%clientId) != %this)
                return 1.0;

   %time = getIntegerTime(true) >> 5;
   %lastTime = Player::lastJetTime(%this) >> 10;

   if ((%lastTime + 1.5) < %time) {
      return 0.0;
   } else {
      %diff = %time - %lastTime;
      %heat = 1.0 - (%diff / 1.5);
      return %heat;
   }
}

function Player::jump(%this,%mom)
{
	dbecho($dbechoMode, "Player::jump(" @ %this @ ", " @ %mom @ ")");

   %cl = GameBase::getControlClient(%this);
   if(%cl != -1)
   {
      %vehicle = Player::getMountObject (%this);
                %this.lastMount = %vehicle;
                %this.newMountTime = getSimTime() + 3.0;
                Player::setMountObject(%this, %vehicle, 0);
                Player::setMountObject(%this, -1, 0);
                Player::applyImpulse(%pl,%mom);
                playSound (GameBase::getDataName(%this).dismountSound, GameBase::getPosition(%this));
   }
}


//----------------------------------------------------------------------------

$animNumber = 25;
function playNextAnim(%clientId)
{
	dbecho($dbechoMode, "playNextAnim(" @ %clientId @ ")");

        if($animNumber > 36)
                $animNumber = 25;
        Player::setAnimation(%clientId, $animNumber++);
}

function Client::takeControl(%clientId, %objectId)
{
	dbecho($dbechoMode, "Client::takeControl(" @ %clientId @ ", " @ %objectId @ ")");

   // remote control
   if(%objectId == -1)
   {
      //echo("objectId = " @ %objectId);
      return;
   }
   if(GameBase::getTeam(%objectId) != Client::getTeam(%clientId))
   {
      //echo(GameBase::getTeam(%objectId) @ " " @ Client::getTeam(%clientId));
      return;
   }
   if(GameBase::getControlClient(%objectId) != -1)
   {
      echo("Ctrl Client = " @ GameBase::getControlClient(%objectId));
      return;
   }
        %name = GameBase::getDataName(%objectId);
        if(%name != CameraTurret && %name != DeployableTurret)
   {
           if(!GameBase::isPowered(%objectId))
                {
              // echo("Turret " @ %objectId @ " not powered.");
              return;
                }
   }
   if(!(Client::getOwnedObject(%clientId)).CommandTag && GameBase::getDataName(%objectId) != CameraTurret &&
      !$TestCheats) {
                Client::SendMessage(%clientId,0,"Must be at a Command Station to control turrets");
                return;
   }
   if(GameBase::getDamageState(%objectId) == "Enabled") {
        Client::setControlObject(%clientId, %objectId);
        Client::setGuiMode(%clientId, $GuiModePlay);
        }
}

function remoteCmdrMountObject(%clientId, %objectIdx)
{
	dbecho($dbechoMode, "remoteCmdrMountObject(" @ %clientId @ ", " @ %objectIdx @ ")");

   Client::takeControl(%clientId, getObjectByTargetIndex(%objectIdx));
}

function checkControlUnmount(%clientId)
{
	dbecho($dbechoMode, "checkControlUnmount(" @ %clientId @ ")");

   %ownedObject = Client::getOwnedObject(%clientId);
   %ctrlObject = Client::getControlObject(%clientId);
   if(%ownedObject != %ctrlObject)
   {
      if(%ownedObject == -1 || %ctrlObject == -1)
         return;
      if(getObjectType(%ownedObject) == "Player" && Player::getMountObject(%ownedObject) == %ctrlObject)
         return;
      Client::setControlObject(%clientId, %ownedObject);
   }
}

