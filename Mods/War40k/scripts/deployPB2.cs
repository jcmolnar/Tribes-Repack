// Bunker
// by C|one
$TeamItemMax[CommandCenterPack2] = 3;
$InvList[CommandCenterPack2] = 1;
$RemoteInvList[CommandCenterPack2] = 1;
$CanAlwaysTeamDestroy[CommandCenterPack2] = 1;


// ebunker, arena2,command1,common1
function CommandCenterPack2::Initialize()
{
	$TeamItemCount[0 @ CommandCenterPack2] = 0; 
	$TeamItemCount[1 @ CommandCenterPack2] = 0; 
	$TeamItemCount[2 @ CommandCenterPack2] = 0; 
	$TeamItemCount[3 @ CommandCenterPack2] = 0; 
	$TeamItemCount[4 @ CommandCenterPack2] = 0; 
	$TeamItemCount[5 @ CommandCenterPack2] = 0; 
	$TeamItemCount[6 @ CommandCenterPack2] = 0; 
	$TeamItemCount[7 @ CommandCenterPack2] = 0;
} 


ItemImageData CommandCenterPack2Image
{
        shapeFile = "ammounit_remote";
        mountPoint = 2;
        mountOffset = { 0, -0.1, -0.3 };
        mountRotation = { 0, 0, 0 };
        mass = 1.0;
        firstPerson = false;
};

ItemData CommandCenterPack2
{
        description = "Pillbox";
        shapeFile = "ammounit_remote";
        className = "Backpack";
        heading = $InvHead[ihDOb];
        shadowDetailMask = 4;
        imageType = CommandCenterPack2Image;
        mass = 2.0;
        elasticity = 0.2;
        price = 450;
        hudIcon = "deployable";
        showWeaponBar = true;
        hiliteOnActive = true;
};


function CommandCenterPack2::onUse(%player,%item)
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

function CommandCenterPack2::onDeploy(%player,%item,%pos)
{
	if (CommandCenterPack2::deployShape(%player,%item))
	Player::decItemCount(%player,%item);
}

function CommandCenterPack2::deployShape(%player,%item)
{
        %client = Player::getClient(%player);
        if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item])
        {
                if (GameBase::getLOSInfo(%player,3))
                {
                        %obj = getObjectType($los::object);
                        if (%obj == "SimTerrain")
                        {
                                if (Vector::dot($los::normal,"0 0 1") > 0.7)
                                {
                        

				%rot = getWord(GameBase::getRotation(%player), 2);
				%ccname = "CommCen" @ Client::getTeam(%player);

                        instant SimGroup %ccname  {
                                instant InteriorShape "CommCenBase" {
                                        filename="ebunker.dis";
                                        iscontainer="1";
                                        position=Vector::add(Gamebase::getPosition(%player), "0 0 -0.3");
                                        rotation="0 0 " @ %rot;
                                        lightparams="1";
                                        locked="0";
                                        
                                        };


                                };

					  //Get rid of it when we change missions...
					  addToSet("MissionCleanup","CommCen" @ Client::getTeam(%player));

                                $TeamItemCount[GameBase::getTeam(%player) @ "CommandCenterPack2"]++;
                                %playerpos = GameBase::getPosition(%player);
                                %newplayerpos = Vector::add(%playerpos, "0 0 0.2");
                                GameBase::setPosition(%player,%newplayerpos);
                                
                                return true;
                                }
                                else
                                        Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
                        }
                        else
                                Client::sendMessage(%client,0,"Can only deploy on terrain.");
                }
                else
                        Client::sendMessage(%client,0,"Deploy position out of range");
        }
        else
                 Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
                 return false;
}