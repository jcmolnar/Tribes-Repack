// Defense Emplacement
// by C|one
$TeamItemMax[CommandCenterPack] = 2;
$InvList[CommandCenterPack] = 1;
$RemoteInvList[CommandCenterPack] = 1;
$CanAlwaysTeamDestroy[CommandCenterPack] = 1;

function CommandCenterPack::Initialize()
{
	$TeamItemCount[0 @ CommandCenterPack] = 0; 
	$TeamItemCount[1 @ CommandCenterPack] = 0; 
	$TeamItemCount[2 @ CommandCenterPack] = 0; 
	$TeamItemCount[3 @ CommandCenterPack] = 0; 
	$TeamItemCount[4 @ CommandCenterPack] = 0; 
	$TeamItemCount[5 @ CommandCenterPack] = 0; 
	$TeamItemCount[6 @ CommandCenterPack] = 0; 
	$TeamItemCount[7 @ CommandCenterPack] = 0;
} 


ItemImageData CommandCenterPackImage
{
        shapeFile = "ammounit_remote";
        mountPoint = 2;
        mountOffset = { 0, -0.1, -0.3 };
        mountRotation = { 0, 0, 0 };
        mass = 1.0;
        firstPerson = false;
};

ItemData CommandCenterPack
{
        description = "Emplacement";
        shapeFile = "ammounit_remote";
        className = "Backpack";
        heading = $InvHead[ihDOb];
        shadowDetailMask = 4;
        imageType = CommandCenterPackImage;
        mass = 2.0;
        elasticity = 0.2;
        price = 300;
        hudIcon = "deployable";
        showWeaponBar = true;
        hiliteOnActive = true;
};


function CommandCenterPack::onUse(%player,%item)
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

function CommandCenterPack::onDeploy(%player,%item,%pos)
{
	if (CommandCenterPack::deployShape(%player,%item))
	Player::decItemCount(%player,%item);
}

function CommandCenterPack::deployShape(%player,%item)
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
                        


				%ccname = "CommCen" @ Client::getTeam(%player);

                        instant SimGroup %ccname  {
                                instant InteriorShape "CommCenBase" {
                                        filename="bunker4.dis";
                                        iscontainer="1";
                                        position=Vector::add(Gamebase::getPosition(%player), "0 0 0");
                                        rotation=GameBase::getRotation(%player);
                                        lightparams="1";
                                        locked="0";
                                        
                                        };


                                };

					  //Get rid of it when we change missions...
					  addToSet("MissionCleanup","CommCen" @ Client::getTeam(%player));

                                $TeamItemCount[GameBase::getTeam(%player) @ "CommandCenterPack"]++;
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