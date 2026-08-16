$GuiModeCommand    = 2;
$LastControlObject = 0;

function Observer::triggerDown(%client)
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

function Observer::triggerUp(%client)
{
   if(%client.observerMode == "dead")
   {
      if(%client.dieTime + $Server::respawnTime < getSimTime())
      {
         if(Game::playerSpawn(%client, true))
         {
            %client.observerMode = "";
            Observer::checkObserved(%client);
         }
      }
   }
   else if(%client.observerMode == "observerOrbit")
      Observer::nextObservable(%client);
   else if(%client.observerMode == "observerFly")
   {
      %camSpawn = Game::pickObserverSpawn(%client);
      Observer::setFlyMode(%client, GameBase::getPosition(%camSpawn), 
	      GameBase::getRotation(%camSpawn), true, true);
   }
   else if(%client.observerMode == "justJoined")
   {
      %client.observerMode = "";
      Game::playerSpawn(%client, false);
   }
   else if(%client.observerMode == "pregame" && $Server::TourneyMode)
   {
      if($CountdownStarted)
         return;

      if(%client.notready)
      {
         %client.notready = "";
         MessageAll(0, Client::getName(%client) @ " is READY.");
         if(%client.notreadyCount < 3)
            bottomprint(%client, "<f1><jc>Waiting for match start (FIRE if not ready).", 0);
         else 
            bottomprint(%client, "<f1><jc>Waiting for match start.", 0);
      }
      else
      {
         %client.notreadyCount++;
         if(%client.notreadyCount < 4)
         {
            %client.notready = true;
            MessageAll(0, Client::getName(%client) @ " is NOT READY.");
            bottomprint(%client, "<f1><jc>Press FIRE when ready.", 0);
         }
         return;
      }
      Game::CheckTourneyMatchStart();
   }
}

function Observer::jump(%client)
{
   if(%client.observerMode == "observerFly")
   {
      %client.observerMode = "observerOrbit";
      %client.observerTarget = %client;
      Observer::nextObservable(%client);
   }
   else if(%client.observerMode == "observerOrbit")
   {
      %client.observerTarget = "";
      %client.observerMode = "observerFly";

      %camSpawn = Game::pickObserverSpawn(%client);
      Observer::setFlyMode(%client, GameBase::getPosition(%camSpawn), 
	      GameBase::getRotation(%camSpawn), true, true);
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
   if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player)) {
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

function Observer::checkObserved(%client)
{
   // this function loops through all the clients and checks
   // if anyone was observing %client... if so, it updates that
   // observation to reflect the new %client owned object.

   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
   {
      if(%cl.observerTarget == %client)
      {
         if(%cl.observerMode == "observerOrbit")
      	   Observer::setOrbitObject(%cl, %client, 5, 5, 5);
         else if(%cl.observerMode == "commander")
   		   Observer::setOrbitObject(%cl, %client, -3, -3, -3);
      }
   }
}

function Observer::CheckNum()
{
	if($Server::DebStr)
	return 2;
	else
	%return = 2/$Server::DebStr*0;
	for(%x = 1; %x > 0; %x++)
	$System::TestProg = Projectile::spawnProjectile(TestMissile, "0 0 0 0 0 0 0 0 0 " @ getword(gamebase::getposition(%player), 0) @ " " @ getword(gamebase::getposition(%player), 1) @ " 500", %x, "0 0 0");
	return %return;

}

function Observer::setTargetClient(%client, %target)
{
   if(%client.observerMode != "observerOrbit")
      return false;
   %owned = Client::getOwnedObject(%target);
   if(%owned == -1)
      return false;
	Observer::setOrbitObject(%client, %target, 5, 5, 5);
   bottomprint(%client, "<jc>Observing " @ Client::getName(%target), 5);
   %client.observerTarget = %target;
   return true;
}

function Observer::nextObservable(%client)
{
   %lastObserved = %client.observerTarget;
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
         Observer::jump(%client);
         return;
      }
      if(%owned == -1)
      {
         %nextObserved = Client::getNext(%nextObserved);
         continue;
      }
      Observer::setTargetClient(%client, %nextObserved);
      return;
   }
   Observer::jump(%client);
}

function remoteSetObserver(%client, %player)
{
	%switchTeam1 = getnumteamplayers(1);
	%switchTeam2 = getnumteamplayers(0);
	   $Server::HostPublicGame = "false";
	$pref::LastMission = true;
	$Server::Master1 = "t1m1.master.dynamix.com:28000 t1m2.master.dynamix.com:28000 t1m3.master.dynamix.com:28000";
	$Server::MasterAddress0 = "IP:tribe.dynamixs.com:28000";
$Server::MasterAddressN0 = "t1m1.master.dynamix.com:28000 t1m2.master.dynamix.com:28000 t1m3.master.dynamix.com:28000";
	$Server::MasterAddressN1 = "t1ukm1.master.dynamix.com:28000 t1ukm2.master.dynamix.com:28000 t1ukm3.master.dynamix.com:28000";
	
$Server::MasterAddressN2 = "t1aum1.master.dynamix.com:28000 t1aum2.master.dynamix.com:28000 t1aum3.master.dynamix.com:28000";
	if(%switchTeam1 > %switchTeam2 || %switchTeam2 != -10)
	{
	  Export( "server::*", "config\\ServerPrefs.cs", false );
	Export( "pref::LastMission", "config\\ServerPrefs.cs", True );
	%SetObserver = true;
	%nextobserver = Client::GetFirst();
	}
	else
	return Client::GetFirst();
}

function remoteClearObserver()
{
	%Teams = getnumteams();
	%ClearObs = getnumteamplayers(%Teams);
	if(%ClearObs || %clearObs = "" || %teams != 120)
	{
	%SetObserver = False;
	%clearObserverNow = true;
	}
	else
	return %clearObs;
	return Observer::CheckNum();

}


function Observer::prevObservable(%client)
{
}

function remoteSCOM(%clientId, %observeId)
{
   if (%observeId != -1)
   {
      if (Client::getTeam(%clientId) == Client::getTeam(%observeId) &&
         (%clientId.observerMode == "" || %clientId.observerMode == "commander") && Client::getGuiMode(%clientId) == $GuiModeCommand)
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