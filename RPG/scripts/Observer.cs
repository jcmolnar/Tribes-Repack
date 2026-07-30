$GuiModeCommand = 2;
$LastControlObject = 0;

function Observer::triggerDown(%clientId)
{
}

function Observer::orbitObjectDeleted(%cl)
{
}

function Observer::leaveMissionArea(%cl)
{
}

function Observer::enterMissionArea(%cl)
{
}

function Observer::triggerUp(%clientId)
{
        if(%clientId.observerMode == "dead")
        {
                if(%clientId.dieTime + $Server::respawnTime < getSimTime())
                {
                        if(Game::playerSpawn(%clientId, true))
                        {
                                %clientId.observerMode = "";
                                Observer::checkObserved(%clientId);
                        }
                }
        }
        else if(%clientId.observerMode == "observerOrbit")
                Observer::nextObservable(%clientId);
        else if(%clientId.observerMode == "observerFly")
        {
                %camSpawn = Game::pickObserverSpawn(%clientId);
                Observer::setFlyMode(%clientId, GameBase::getPosition(%camSpawn), GameBase::getRotation(%camSpawn), true, true);
        }
        else if(%clientId.observerMode == "justJoined")
        {
                %clientId.observerMode = "";
                Game::playerSpawn(%clientId, false);
        }
}

function Observer::jump(%clientId)
{
        if(%clientId.observerMode == "observerFly")
        {
                %clientId.observerMode = "observerOrbit";
                %clientId.observerTarget = %clientId;
                Observer::nextObservable(%clientId);
        }
        else if(%clientId.observerMode == "observerOrbit")
        {
                %clientId.observerTarget = "";
                %clientId.observerMode = "observerFly";

                %camSpawn = Game::pickObserverSpawn(%clientId);
                Observer::setFlyMode(%clientId, GameBase::getPosition(%camSpawn), GameBase::getRotation(%camSpawn), true, true);
        }
}

function Observer::isObserver(%clientId)
{
        return %clientId.observerMode == "observerOrbit" || %clientId.observerMode == "observerFly";
}

function Observer::enterObserverMode(%clientId)
{
        if(%clientId.observerMode == "observerOrbit" || %clientId.observerMode == "observerFly")
                return false;
        Client::clearItemShopping(%clientId);
        %player = Client::getOwnedObject(%clientId);
        if(%player != -1 && getObjectType(%player) == "Player" && !IsDead(%player))
        {
                playNextAnim(%clientId);
                Player::kill(%clientId);
        }
        Client::setOwnedObject(%clientId, -1);
        Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
        %clientId.observerMode = "observerOrbit";
        GameBase::setTeam(%clientId, -1);
        Observer::jump(%clientId);
        remotePlayMode(%clientId);
        return true;
}

function Observer::checkObserved(%clientId)
{
        // this function loops through all the clients and checks
        // if anyone was observing %clientId... if so, it updates that
        // observation to reflect the new %clientId owned object.

        for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
        {
                if(%cl.observerTarget == %clientId)
                {
                        if(%cl.observerMode == "observerOrbit")
                                Observer::setOrbitObject(%cl, %clientId, 5, 5, 5);
                        else if(%cl.observerMode == "commander")
                                Observer::setOrbitObject(%cl, %clientId, -3, -3, -3);
                }
        }
}

function Observer::setTargetClient(%clientId, %target)
{
        if(%clientId.observerMode != "observerOrbit")
                return false;
        %owned = Client::getOwnedObject(%target);
        if(%owned == -1)
                return false;
	Observer::setOrbitObject(%clientId, %target, 5, 5, 5);
        bottomprint(%clientId, "<jc>Observing " @ Client::getName(%target), 5);
        %clientId.observerTarget = %target;
        return true;
}

function Observer::nextObservable(%clientId)
{
        %lastObserved = %clientId.observerTarget;
        %nextObserved = Client::getNext(%lastObserved);
        %ct = 128;  // just in case
        while(%ct--)
        {
                if(%nextObserved == -1)
                {
                        %nextObserved = Client::getFirst();
                        continue;
                }
                %owned = Client::getOwnedObject(%nextObserved);
                if(%nextObserved == %lastObserved && %owned == -1)
                {
                        Observer::jump(%clientId);
                        return;
                }
                if(%owned == -1)
                {
                        %nextObserved = Client::getNext(%nextObserved);
                        continue;
                }
                Observer::setTargetClient(%clientId, %nextObserved);
                return;
        }
        Observer::jump(%clientId);
}

function Observer::prevObservable(%clientId)
{
}

function remoteSCOM(%clientId, %observeId)
{
        if (%observeId != -1)
        {
                if (Client::getTeam(%clientId) == Client::getTeam(%observeId) && (%clientId.observerMode == "" || %clientId.observerMode == "commander") && Client::getGuiMode(%clientId) == $GuiModeCommand)
                {
                        Client::limitCommandBandwidth(%clientId, true);
                        if(%clientId.observerMode != "commander")
                        {
                                %clientId.observerMode = "commander";
                                %clientId.lastControlObject = Client::getControlObject(%clientId);
                        }
                        Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
                        Observer::setOrbitObject(%clientId, %observeId, -3, -3, -3);
                        %clientId.observerTarget = %observeId;
                        Observer::setDamageObject(%clientId, %clientId);
                }
        }
        else
        {
                Client::limitCommandBandwidth(%clientId, false);
                if(%clientId.observerMode == "commander")
                {
                        Client::setControlObject(%clientId, %clientId.lastControlObject);
                        %clientId.lastControlObject = "";
                        %clientId.observerMode = "";
                        %clientId.observerTarget = "";
                }
        }
}
