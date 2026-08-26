//-=-=-=-=-=-=-
// Warp Pack
//  Created by <DC/SB>C|one
//-=-=-=-=-=-=-=-=-=-=-=-=-=-
$InvList[Warpxpack] = 1;
$RemoteInvList[Warpxpack] = 1;
$WarpSafeTime = 8;
$WarpRange = 350;
$WarpChance = 65;

ItemImageData WarpxpackImage
{
        shapeFile = "shield";
        mountPoint = 2;
        weaponType = 2;
        minEnergy = 10;
        maxEnergy = 10;
        mountOffset = { 0, -0.05, 0 };
        mountRotation = { 3.14, 0, 0 };
        lightType = 3;
        lightRadius = 10;
        lightTime = 10;
        lightColor = { 0.3, 0.1, 0.6 };
        firstPerson = false;
};

ItemData Warpxpack
{
        description = "Warp Pack";
        shapeFile = "shield";
        className = "Backpack";
        heading = $InvHead[ihBac];
        shadowDetailMask = 4;
        imageType = WarpxpackImage;
        price = 15;
        hudIcon = "energypack";
        showWeaponBar = true;
        hiliteOnActive = true;
};

function WarpxpackImage::onActivate(%player,%imageSlot)
{
        %client = Player::getClient(%player);
        %player = Client::getOwnedObject(%client);
        if (GameBase::getLOSInfo(%player,$WarpRange))
        {
                %dest = $los::position;
                if($WarpTime[%client] > 0)
                {
                        Bottomprint(%client, "<jc><f1>Warning!\n<jc><f0>Warp field not stabilized");
                        if(floor(getrandom() * 100) > $WarpChance)
                        {
                                GameBase::setPosition(%client,"0 0 10000");
                                $WarpWrapTime[%client] = 0.6;
                                $WarpStable[%client] = 0;
                                WarpWrap(%client,%player,%dest);
                                GameBase::setEnergy(%client,0);
                                Player::kill(%client);
                                Client::onKilled(%client,%client);
                                $WarpTime[%client] = 0;
                                WarpxpackImage::onDeactivate(%client,%imageSlot);
                        }
                        else
                        {
                                GameBase::setPosition(%client,"0 0 10000");
                                $WarpWrapTime[%client] = 0.6;
                                $WarpStable[%client] = 1;
                                WarpWrap(%client,%player,%dest);
                                GameBase::setEnergy(%client,0);
                                WarpxpackImage::onDeactivate(%client,%imageSlot);
                        }
                }
                else
                {
                        GameBase::setPosition(%client,"0 0 10000");
                        $WarpWrapTime[%client] = 0.6;
                        $WarpStable[%client] = 2;
                        WarpWrap(%client,%player,%dest);
                        GameBase::setEnergy(%client,0);
                        WarpxpackImage::onDeactivate(%client,%imageSlot);
                        $WarpTime[%client] = $WarpSafeTime;
                        checkSafeWarp(%client,%player);
                }
        }
        else
        {
                Bottomprint(%client, "<jc>Warp position out of range");
                WarpxpackImage::onDeactivate(%client,%imageSlot);
        }
}

function WarpxpackImage::onDeactivate(%player,%imageSlot)
{
        Player::trigger(%player,$BackpackSlot,false);
}

function Warpxpack::onMount(%player,%item)
{
        %client = Player::getClient(%player);
        Bottomprint(%client, "<f1>Warp Pack:<f0> Allows short range teleportation via the users HUD crosshair - but can badly misfunction. ");
}

function checkSafeWarp(%client,%player)
{
        if ($WarpTime[%client] > 0)
        {
                $WarpTime[%client] -= 1;
                schedule("checkSafeWarp(" @ %client @ "," @ %player @ ");",1,%player);
        }
        else
        {
                $WarpTime[%client] = 0;
                Bottomprint(%client, "<jc><f1>Warp field stabilized");
        }
}

function WarpWrap(%client,%player,%dest)
{
        if ($WarpWrapTime[%client] > 0)
        {
                $WarpWrapTime[%client] -= 0.6;
                schedule("WarpWrap(" @ %client @ "," @ %player @ ",\"" @ %dest @ "\");",1,%player);
        }
        else
        {
                $WarpWrapTime[%client] = 0;
                GameBase::setPosition(%client,%dest);
                if ($WarpStable[%client] == 2)
                {
                        Bottomprint(%client, "<jc><f1>Warp Successful!\n<jc><f0>Warp field restabilizing");
                }
                else
                {
                        if ($WarpStable[%client] == 1)
                        {
                                Bottomprint(%client, "<jc><f1>Warp Successful!\n<jc><f0>Warp field still unstable");
                        }
                        else
                        {
                                Bottomprint(%client, "<jc><f1>Warp Unsuccessful!\n<jc><f0>The warp tears you to apart");
                        }
                }
        }
}


//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
//  LEG HIT DEADWEIGHT
//-=-=-=-==-=-=-=-==-=-=-=-=-=-
$InvList[DeadWeight] = 0;
$RemoteInvList[DeadWeight] = 0;

ItemImageData DeadWeightImage
{
	shapeFile = "breath";
	mountPoint = 4;
	mass = 80.0;
};

ItemData DeadWeight
{
	description = "Deadweight";
	className = "Tool";
	shapeFile = "grenammo";
	heading = "eDeployables";
	shadowDetailMask = 4;
	imageType = DeadWeightImage;
	price = 0;
	showWeaponBar = false;
	mass = 80.0;
	showInventory = false;
};

function DeadWeight::onDrop(%player, %item)
{
}