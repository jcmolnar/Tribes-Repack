//-=-=-==-==-=-=-=--=-=-=-=-=-=-
//   Force Field Door
// Original creation Edgecrusher
//-=-=-===-==-=-=-=-=-=-=-=-=-=-=

$TeamItemMax[DoorPack] = 10;
$InvList[DoorPack] = 1;
$RemoteInvList[DoorPack] = 1;
$CanAlwaysTeamDestroy[DoorPack] = 1;

function deployDoorPack::Initialize()
{
	$TeamItemCount[0 @ DoorPack] = 0; 
	$TeamItemCount[1 @ DoorPack] = 0; 
	$TeamItemCount[2 @ DoorPack] = 0; 
	$TeamItemCount[3 @ DoorPack] = 0; 
	$TeamItemCount[4 @ DoorPack] = 0; 
	$TeamItemCount[5 @ DoorPack] = 0; 
	$TeamItemCount[6 @ DoorPack] = 0; 
	$TeamItemCount[7 @ DoorPack] = 0;
} 

ItemImageData DoorPackImage
{
        //shapeFile = "forcefield_5x5";
        shapeFile = "AmmoPack";
        mountPoint = 2;
        mountOffset = { 0, -0.03, 0 };
        mass = 2.5;
        firstPerson = false;
};

ItemData DoorPack
{
        description = "Small Force Door";
        //shapeFile = "forcefield_5x5";
        shapeFile = "AmmoPack";
        className = "Backpack";
        heading = $InvHead[ihDrs];
        imageType = DoorPackImage;
        shadowDetailMask = 4;
        mass = 2.5;
        elasticity = 0.2;
        price = 50;
        hudIcon = "deployable";
        showWeaponBar = true;
        hiliteOnActive = true;
};

function DoorPack::onUse(%player,%item)
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

function DoorPack::onDeploy(%player,%item,%pos)
{
        if (DoorPack::deployShape(%player,%item))
        {
                Player::decItemCount(%player,%item);
        }
}

function DoorPack::deployShape(%player,%item)
{
         %client = Player::getClient(%player);
        if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]) {
                if (GameBase::getLOSInfo(%player,3)) {

                        %obj = getObjectType($los::object);
                                %rot = GameBase::getRotation(%player);

                                        %camera = newObject("DoorPack","StaticShape",DoorPackShape,true);
                                        addToSet("MissionCleanup", %camera);
                                        GameBase::setTeam(%camera,GameBase::getTeam(%player));
                                        GameBase::setRotation(%camera,%rot);
                                        GameBase::setPosition(%camera,$los::position);
                                        Gamebase::setMapName(%camera,"Small Force Door#"@ $totalNumCameras++ @ " " @ Client::getName(%client));
                                        Client::sendMessage(%client,0,"Small Force Door deployed");
                                        playSound(SoundPickupBackpack,$los::position);
                                        $TeamItemCount[GameBase::getTeam(%camera) @ "doorthreebyfourForceFieldPack"]++;
                                        echo("MSG: ",%client," deployed a Small Force Door ");
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

StaticShapeData DoorPackShape
{
className = "LargeForceField";
damageSkinData = "objectDamageSkins";
shapeFile = "forcefield_5x5";
maxDamage = 10.0;
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
description = "Small Force Door";
};
function DoorPackShape::Destruct(%this)
{
DoorPackShape::doDamage(%this);
}
function DoorPackShape::doDamage(%this) {
calcRadiusDamage(%this, $DebrisDamageType, 5, 0.5, 25, 15, 4, 0.4, 0.1, 250, 100);
}
function DoorPackShape::onDestroyed(%this)
{
DoorPackShape::doDamage(%this);
$TeamItemCount[GameBase::getTeam(%this) @ "LargeForceField"]--;
}
function DoorPackShape::onCollision(%this,%obj)
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
DoorPackShape::openDoor(%this);
return;
}
function DoorPackShape::openDoor(%this) {

GameBase::startfadeout(%this);

%pos=GameBase::getPosition(%this);
%pos=Vector::add(%pos,"0 0 6");
GameBase::setPosition(%this,%pos);
schedule("GameBase::playSound("@%this@",ForceFieldOpen,0);",0.15);
schedule("DoorPackShape::closeDoor("@%this@");",4);
}
function DoorPackShape::closeDoor(%this) {
%pos=GameBase::getPosition(%this);
%pos=Vector::add(%pos,"0 0 -6");
GameBase::setPosition(%this,%pos);
GameBase::startfadein(%this);
schedule("GameBase::playSound("@%this@",ForceFieldClose,0);",0.15);

}

function DoorPackShape::openDoor(%this) {

GameBase::startfadeout(%this);

%pos=GameBase::getPosition(%this);
%pos=Vector::add(%pos,"0 0 6");
GameBase::setPosition(%this,%pos);
schedule("GameBase::playSound("@%this@",ForceFieldOpen,0);",0.15);
schedule("DoorPackShape::closeDoor("@%this@");",4);
}




