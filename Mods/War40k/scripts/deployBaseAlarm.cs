$InvList[BaseAlarm] = 1;
$RemoteInvList[BaseAlarm] = 1;
$TeamItemMax[BaseAlarm] = 10;
$CanAlwaysTeamDestroy[BaseAlarm] = 1;

$totalNumAlarms[-1] = 0;
$totalNumAlarms[0] = 0;
$totalNumAlarms[1] = 0;
$totalNumAlarms[2] = 0;
$totalNumAlarms[3] = 0;
$totalNumAlarms[4] = 0;
$totalNumAlarms[5] = 0;
$totalNumAlarms[6] = 0;
$totalNumAlarms[7] = 0;

function deployBaseAlarm::Initialize()
{
 $TeamItemCount[0 @ BaseAlarm] = 0;
 $TeamItemCount[1 @ BaseAlarm] = 0;
 $TeamItemCount[2 @ BaseAlarm] = 0;
 $TeamItemCount[3 @ BaseAlarm] = 0;
 $TeamItemCount[4 @ BaseAlarm] = 0;
 $TeamItemCount[5 @ BaseAlarm] = 0;
 $TeamItemCount[6 @ BaseAlarm] = 0;
 $TeamItemCount[7 @ BaseAlarm] = 0;
}

ItemImageData BaseAlarmImage
{
 shapeFile = "sensor_small";
 mountPoint = 2;
 mountOffset = { 0, 0, 0.1 };
 mountRotation = { 1.57, 0, 0 };
 firstPerson = false;
};

ItemData BaseAlarm {
 description = "Base Alarm";
 shapeFile = "sensor_small";
 className = "Backpack";
 heading = $InvHead[ihDSe];
 shadowDetailMask = 4;
 imageType = BaseAlarmImage;
 mass = 2.0;
 elasticity = 0.2;
 price = 15;
 hudIcon = "deployable";
 showWeaponBar = true;
 hiliteOnActive = true;
};

function BaseAlarm::onUse(%player,%item) {
 if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
  Player::mountItem(%player,%item,$BackpackSlot);
 }
 else {
  Player::deployItem(%player,%item);
 }
}

function BaseAlarm::onDeploy(%player,%item,%pos){
if (BaseAlarm::deployShape(%player,%item)) {
 Player::decItemCount(%player,%item);
 }
}

function BaseAlarm::deployShape(%player,%item) {
%client = Player::getClient(%player);
if (GameBase::getLOSInfo(%player,3)) {
 %obj = getObjectType($los::object);
 if (%obj == "InteriorShape") {
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
  if(checkDeployArea(%client,$los::position)) {
   %alarm = newObject("","StaticShape", "AlarmKit",true);
   addToSet("MissionCleanup", %alarm);
   GameBase::setTeam(%alarm,GameBase::getTeam(%player));
   GameBase::setRotation(%alarm,%rot);
   GameBase::setPosition(%alarm,$los::position);
   $totalNumAlarms[GameBase::getTeam(gamebase::getteam(%player))]++;
   Gamebase::setMapName(%alarm,"Base Alarm #" @ $totalNumAlarms[GameBase::getTeam(%player)]++);
   Client::sendMessage(%client,0,"Alarm #" @ $totalNumAlarms[GameBase::getTeam(%player)] @ " deployed");
   $TeamItemCount[GameBase::getTeam(gamebase::getteam(%player)) @ "BaseAlarm"]++;
   $totalNumAlarms[GameBase::getTeam(gamebase::getteam(%player))]++;
   return true;
   }
  }
  else {
   Client::sendMessage(%client,0,"Can only deploy on buildings");
  }
 }
 else {
  Client::sendMessage(%client,0,"Deploy position out of range");
 }
 return false;
}

StaticShapeData AlarmKit {
 description = "Base Alarm";
 shapeFile = "sensor_small";
 debrisId = flashDebrisSmall;
 sfxAmbient = SoundBeaconActive;
 maxDamage = 1.0;
 mapIcon = "M_marker";
 damageSkinData = "objectDamageSkins";
 visibleToSensor = true;
 triggerRadius = 10.0;
};

function AlarmKit::onEnabled(%this) {
 schedule("AlarmKit::check(" @ %this @ ");", 0.01, %this);
}

function AlarmKit::onDisabled(%this) {
 TeamMessages(1, %itemTeam,GameBase::GetMapName(%this) @ " has malfunctioned!~wLeftMissionArea.wav");
}
function AlarmKit::onDestroyed(%this) {
 StaticShape::objectiveDestroyed(%this);
 $TeamItemCount[GameBase::getTeam(gamebase::getteam(%player)) @ "BaseAlarm"]--;
 $totalNumAlarms[GameBase::getTeam(gamebase::getteam(%player))]--;
}

function AlarmKit::check(%this) {
 if(GameBase::getDamageState(%this) != "Enabled") { deleteObject(%set); schedule("AlarmKit::check(" @ %this @ ");", 6.0, %this); return; }
 %Set = newObject("set",SimSet);
 %Pos = GameBase::getPosition(%this);
 %Mask = $SimPlayerObjectType;
 containerBoxFillSet(%Set, %Mask, %Pos, 3, 3, 0.2,0);
 %num = Group::objectCount(%Set);
 for(%i; %i < %num; %i++) {
  %obj = Group::getObject(%Set, %i);
  if (%obj != %this) {
   if(GameBase::getTeam(%obj) != GameBase::getTeam(%this)) {
    %name = Player::getClient(%obj);
    %name = Client::getName(%name);
    TeamMessages(1, %itemTeam,GameBase::GetMapName(%this) @ " has been triggered by " @ %name @ "~wLeftMissionArea.wav");
   }
   else { }
  }
 }
}