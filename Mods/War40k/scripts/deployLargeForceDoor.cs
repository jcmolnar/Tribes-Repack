///-----------------------------------------------
/// description = "4x8 Force Field Door";
/// Created by TriCon Team C3 & graphfx    2857646
/// http://www.planetstarsiege.com/tricon
///-----------------------------------------------
$InvList[LargeDoorPack] = 1;
$RemoteInvList[LargeDoorPack] = 1;
$TeamItemMax[LargeDoorPack] = 6;
$CanAlwaysTeamDestroy[LargeDoorPack] = 1;


function deployLargeForceDoor::Initialize()
{
	$TeamItemCount[0 @ LargeDoorPack] = 0; 
	$TeamItemCount[1 @ LargeDoorPack] = 0; 
	$TeamItemCount[2 @ LargeDoorPack] = 0; 
	$TeamItemCount[3 @ LargeDoorPack] = 0; 
	$TeamItemCount[4 @ LargeDoorPack] = 0; 
	$TeamItemCount[5 @ LargeDoorPack] = 0; 
	$TeamItemCount[6 @ LargeDoorPack] = 0; 
	$TeamItemCount[7 @ LargeDoorPack] = 0; 

}

ItemImageData LargeDoorPackImage
{
        //shapeFile = "forcefield";
        shapeFile = "AmmoPack";
        mountPoint = 2;
        mountOffset = { 0, -0.03, 0 };
        mass = 2.5;
        firstPerson = false;
};

ItemData LargeDoorPack
{
        description = "Large Force Door";
        //shapeFile = "forcefield";
        shapeFile = "AmmoPack";
        className = "Backpack";
          heading = $InvHead[ihDrs];
        imageType = LargeDoorPackImage;
        shadowDetailMask = 4;
        mass = 2.5;
        elasticity = 0.2;
        price = 150;
        hudIcon = "deployable";
        showWeaponBar = true;
        hiliteOnActive = true;
};

function LargeDoorPack::onUse(%player,%item)
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

function LargeDoorPack::onDeploy(%player,%item,%pos)
{
        if (LargeDoorPack::deployShape(%player,%item))
        {
                Player::decItemCount(%player,%item);
        }
}

function LargeDoorPack::deployShape(%player,%item)
{
         %client = Player::getClient(%player);
        if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]) {
                if (GameBase::getLOSInfo(%player,3)) {

                        %obj = getObjectType($los::object);
                                %rot = GameBase::getRotation(%player);

                                        %camera = newObject("LargeDoorPack","StaticShape",LargeDoorPackShape,true);
                                        addToSet("MissionCleanup", %camera);
                                        GameBase::setTeam(%camera,GameBase::getTeam(%player));
                                        GameBase::setRotation(%camera,%rot);
                                        GameBase::setPosition(%camera,$los::position);
                                        Gamebase::setMapName(%camera,"Large Force Door#"@ $totalNumCameras++ @ " " @ Client::getName(%client));
                                        Client::sendMessage(%client,0,"Large Force Door deployed");
                                        playSound(SoundPickupBackpack,$los::position);
                                        $TeamItemCount[GameBase::getTeam(%camera) @ "LargeDoorPack"]++;
                                        echo("MSG: ",%client," deployed a Large Force Door ");
                                        return true;

                        }
                       else {
                                Client::sendMessage(%client,0,"Cannot deploy here.");
                        }

        }
        else
                 Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");

        return false;
}


StaticShapeData LargeDoorPackShape
{
className = "LargeForceField";
damageSkinData = "objectDamageSkins";
shapeFile = "forcefield";
maxDamage = 20.0;
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
description = "Large Force Door";
};
function LargeDoorPackShape::Destruct(%this)
{
LargeDoorPackShape::doDamage(%this);
}
function LargeDoorPackShape::doDamage(%this) {
calcRadiusDamage(%this, $DebrisDamageType, 5, 0.5, 25, 15, 4, 0.4, 0.1, 250, 100);
}
function LargeDoorPackShape::onDestroyed(%this)
{
LargeDoorPackShape::doDamage(%this);
$TeamItemCount[GameBase::getTeam(%this) @ "LargeForceField"]--;
}
function LargeDoorPackShape::onCollision(%this,%obj)
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
LargeDoorPackShape::openDoor(%this);
return;
}
function LargeDoorPackShape::openDoor(%this) {

GameBase::startfadeout(%this);

%pos=GameBase::getPosition(%this);
%pos=Vector::add(%pos,"0 0 6");
GameBase::setPosition(%this,%pos);
schedule("GameBase::playSound("@%this@",ForceFieldOpen,0);",0.15);
schedule("LargeDoorPackShape::closeDoor("@%this@");",4);
}
function LargeDoorPackShape::closeDoor(%this) {
%pos=GameBase::getPosition(%this);
%pos=Vector::add(%pos,"0 0 -6");
GameBase::setPosition(%this,%pos);
GameBase::startfadein(%this);
schedule("GameBase::playSound("@%this@",ForceFieldClose,0);",0.15);

}

function LargeDoorPackShape::openDoor(%this) {

GameBase::startfadeout(%this);

%pos=GameBase::getPosition(%this);
%pos=Vector::add(%pos,"0 0 6");
GameBase::setPosition(%this,%pos);
schedule("GameBase::playSound("@%this@",ForceFieldOpen,0);",0.15);
schedule("LargeDoorPackShape::closeDoor("@%this@");",4);
}


