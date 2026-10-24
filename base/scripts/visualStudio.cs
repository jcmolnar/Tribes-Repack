// Visual Studio owns only its private LOOPBACK session. No Game/Player callbacks
// are replaced: server.cs calls this after the local client's real ghost handshake.
$VisualStudio::ScriptVersion = 1;

function VisualStudio::onReady(%client)
{
   if(!$VisualStudio::Active ||
      String::NCompare(Client::getTransportAddress(%client), "LOOPBACK", 8))
      return;
   $VisualStudio::Client = %client;
   %client.guiLock = "";
   %client.nospawn = "";
   %client.justConnected = "";
   %client.observerMode = "";
   %client.dead = "";
   GameBase::setTeam(%client, 0);
   Client::setGuiMode(%client, $GuiModePlay);
   centerprint(%client, "");
   bottomprint(%client, "");
   VisualStudio::spawn();
}

function VisualStudio::spawn()
{
   if(!$VisualStudio::Active || $VisualStudio::Client == "")
      return false;
   %client = $VisualStudio::Client;
   if(String::NCompare(Client::getTransportAddress(%client), "LOOPBACK", 8))
      return false;
   %old = Client::getOwnedObject(%client);
   if(%old != -1 && getObjectType(%old) == "Player")
   {
      Client::setControlObject(%client, -1);
      Client::setOwnedObject(%client, -1);
      deleteObject(%old);
   }
   %client.guiLock = "";
   %client.nospawn = "";
   %client.observerMode = "";
   %client.dead = "";
   return Game::playerSpawn(%client, true);
}


// These are authored, valid Broadside drop points, not guessed world positions.
// Its Team0 Random group ends in two ground spawns outside the airship (indices
// 14/15 in the shipped mission); Start's first marker is the enclosed range.
function VisualStudio::place(%mode)
{
   if(!$VisualStudio::Active || $VisualStudio::Client == "")
      return false;
   %player = Client::getOwnedObject($VisualStudio::Client);
   if(%player == -1 || getObjectType(%player) != "Player")
      return false;
   if(%mode == 1)
   {
      %group = nameToID("MissionGroup/Teams/team0/DropPoints/Start");
      %index = 0;
   }
   else
   {
      %group = nameToID("MissionGroup/Teams/team0/DropPoints/Random");
      %index = Group::objectCount(%group) - 2;
      if(%mode == 2)
         %index++;
   }
   if(%group == -1 || %index < 0)
      return false;
   %marker = Group::getObject(%group, %index);
   if(%marker == -1)
      return false;
   GameBase::setPosition(%player, GameBase::getPosition(%marker));
   GameBase::setRotation(%player, GameBase::getRotation(%marker));
   Item::setVelocity(%player, "0 0 0");
   return true;
}
