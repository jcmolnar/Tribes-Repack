$CatSystem = 1;
$CatDeploy = 2;
$CatPlayerData = 4;
$CatTeamData = 8;
$CatGame = 16;
$CatText = 32;
$CatVoting = 64;

 // Always keep CatSystem on
//$LoggingEnabled = $CatSystem | $CatDeploy | $CatPlayerData | $CatGame | $CatText;
//$DisplayEnabled = $CatSystem | $CatDeploy | $CatPlayerData | $CatGame | $CatText;

$LoggingEnabled = $CatSystem | $CatGame | $CatText | $CatVoting;
$DisplayEnabled = $CatSystem | $CatDeploy | $CatPlayerData | $CatGame | $CatText | $CatVoting;

function reportError(%msg)
{
  report(-1, $CatSystem, "ERROR", %msg);
}

function reportConnect(%client)
{
  report(%client, $CatPlayerData, "CONNECT", "(" @ Client::getTransportAddress(%client) @ "):\"" @ escapeString(Client::GetName(%client)) @ "\"");
}

function reportDisconnect(%client)
{
  report(%client, $CatPlayerData, "DISCONNECT", "");
}

function reportSpawn(%client, %marker, %armor)
{
  report(%client, $CatPlayerData, "SPAWN", %marker);
}

function reportKill(%clKiller, %clVictim, %damageType)
{
  report(%clKiller, $CatPlayerData, "KILL", %clVictim @ ":" @ %damageType);
}

function reportTeam(%client, %line)
{
  report(%client, $CatTeam, "TEAM", %line);
}

function reportDeploy(%item, %client)
{
   //
   // ex.  DEP:1234:Ion Turret:2049:0 0 0
   //
  report(%client, $CatDeploy, "DEP", %item @ ":\"" @ escapeString(GameBase::getMapName(%item)) @ "\":" @ %client @ ":" @ GameBase::getPosition(%item));
}

function reportGame(%line)
{
  report(-1, $CatGame, "GAME", %line);
}

function reportText(%client, %command, %line)
{
  if ($LoggingEnabled & $CatText)
    log(%client, %command @ ":\"" @ escapeString(%line) @ "\"");
  if ($DisplayEnabled & $CatText)
    echo(%command @ ":" @ %client @ ":\"" @ escapeString(%line) @ "\"");
}

function reportInitiateVote(%client, %topic, %action, %option)
{
  if ($LoggingEnabled & $CatVoting)
    log(%client, "VOTE:\"" @ escapeString(%topic) @ "\":" @ %action @ ":" @ %option);
  if ($DisplayEnabled & $CatVoting)
    echo("VOTE:" @ %client @ ":\"" @ escapeString(%topic) @ "\":" @ %action @ ":" @ %option);
}

function reportFailVote(%votesFor, %votesAgainst, %abst)
{
  if ($LoggingEnabled & $CatVoting)
    log(-1, "VOTEFAIL:" @ %votesFor @ ":" @ %votesAgainst @ ":" @ %abst);
  if ($DisplayEnabled & $CatVoting)
    echo("VOTEFAIL:" @ %votesFor @ ":" @ %votesAgainst @ ":" @ %abst);
}

function reportSucceedVote(%votesFor, %votesAgainst, %abst)
{
  if ($LoggingEnabled & $CatVoting)
    log(-1, "VOTESUCC:" @ %votesFor @ ":" @ %votesAgainst @ ":" @ %abst);
  if ($DisplayEnabled & $CatVoting)
    echo("VOTESUCC:" @ %votesFor @ ":" @ %votesAgainst @ ":" @ %abst);
}

 //-=-=-=-

function report(%client, %cat, %command, %line)
{
  if ($LoggingEnabled & %cat)
    log(%client, %command @ ": " @ %line);
  if ($DisplayEnabled & %cat)
    echo(%command @ ":" @ %client @ ":" @ %line);
}

function log(%client, %line)
{
  if (%client > 0)
  {
    %ip = Client::getTransportAddress(%client);
    %name = Client::GetName(%client) @ " [" @ %client @ "]";
    $l=%ip @ ":" @ %name @ ":" @ %line; 
  } 
  else
    $l = "GLOBAL:" @ %line;
    
  export("$l","config\\log.cs",true);
}
