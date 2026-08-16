function InitSpawnPoints()
{
	dbecho($dbechoMode, "InitSpawnPoints()");

	%group = nameToID("MissionGroup\\SpawnPoints");

	if(%group != -1)
	{
		for(%i = 0; %i <= Group::objectCount(%group)-1; %i++)
		{
		      %this = Group::getObject(%group, %i);
			%info = Object::getName(%this);

			$MarkerZone[%this] = ObjectInWhichZone(%this);

			if(%info != "")
			{
				$numAIperSpawnPoint[%this] = 0;
				%indexes = "";

				for(%z = 5; GetWord(%info, %z) != -1; %z++)
					%indexes = %indexes @ GetWord(%info, %z) @ " ";

				echo("===================================================");
				echo("Spawn Point was initialized, %this = " @ %this);
				echo("Max spawn per: " @ GetWord(%info, 0));
				echo("Min radius: " @ GetWord(%info, 1));
				echo("Max radius: " @ GetWord(%info, 2));
				echo("Min delay: " @ GetWord(%info, 3));
				echo("Max delay: " @ GetWord(%info, 4));
				echo("Spawn indexes: " @ %indexes);
				echo("Marker Zone ID: " @ $MarkerZone[%this]);
				echo("===================================================");

				SpawnLoop(%this);
			}
		}
	}
}

function SpawnLoop(%this)
{
	dbecho($dbechoMode, "SpawnLoop(" @ %this @ ")");

	%info = Object::getName(%this);

	%mindelay = GetWord(%info, 3);
	%maxdelay = GetWord(%info, 4);
	%diff = %maxdelay - %mindelay;
	%delay = floor(getRandom() * %diff) + %mindelay;

	%indexes = "";
	for(%i = 5; GetWord(%info, %i) != -1; %i++)
		%indexes = %indexes @ GetWord(%info, %i) @ " ";

	%r = floor(getRandom() * (%i-5));
	%index = GetWord(%indexes, %r);

	%flag = "";
	if($SelectiveZoneBotSpawning)
	{
		if(Zone::getNumPlayers($MarkerZone[%this]) > 0 || $MarkerZone[%this] == "")
			%flag = True;
	}
	else
		%flag = True;

	%maxs = Cap(round(GetWord(%info, 0) * $spawnMultiplier), 0, "inf");
	if(%flag && $numAIperSpawnPoint[%this] < %maxs)
		%AIname = AI::helper($spawnIndex[%index], $spawnIndex[%index], "SpawnPoint " @ %this);

	//always call back the spawn loop, in case a spot is freed up for a helper to spawn
      schedule("SpawnLoop(" @ %this @ ");", %delay);
}