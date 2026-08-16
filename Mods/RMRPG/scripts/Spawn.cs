function InitSpawnPoints() {

	dbecho($dbechoMode, "InitSpawnPoints()");
	echo("===================================================");

	$AI::FailedToSpawnBot = 0;
	%group = nameToID("MissionGroup\\SpawnPoints");

	if(%group != -1) {

		for(%i = 0; %i <= Group::objectCount(%group)-1; %i++) {

		      %this = Group::getObject(%group, %i);
			%info = Object::getName(%this);

			$BotSpawnInfo::Data[%this, info] = %info;

			$zone[%this] = ObjectInWhichZone(%this);
			$ZoneSpawnId[%this] = $ZoneSpawnId;
			if(%info != "")
			{
				$numAIperSpawnPoint[%this] = 0;
				%indexes = "";

				for(%z = 5; (%w = GetWord(%info, %z)) != -1; %z++)
					%indexes = %indexes @ %w @ " ";

				$BotSpawnInfo::Data[%this, indexes] = %indexes;
				$BotSpawnInfo::Data[%this, indexesCnt] = %z-5;

				%maxspawn = $BotSpawnInfo::Data[%this, maxspawn] = GetWord(%info, 0);
				%minrad = $BotSpawnInfo::Data[%this, minrad] = GetWord(%info, 1);
				%maxrad = $BotSpawnInfo::Data[%this, maxrad] = GetWord(%info, 2);
				%mindelay = $BotSpawnInfo::Data[%this, mindelay] = GetWord(%info, 3);
				%maxdelay = $BotSpawnInfo::Data[%this, maxdelay] = GetWord(%info, 4);

				//echo("Spawn Point was initialized, %this = "@%this@" | Max spawn per: "@%maxspawn);
				//echo("Min radius: "@%minrad@" | Max radius: "@%maxrad@" Min delay: "@%mindelay@" | Max delay: "@%maxdelay);
				//echo("Spawn indexes: "@%indexes@" | Zone ID: "@$zone[%this]@" | Zone Name:"@$ZoneSpawnId[%this]);
				//echo("===================================================");

				SpawnLoop(%this);
			}
			else
				echo("Error: Object::getName("@%this@"); has no name.");
		}
	}
	else
		echo("Error: Couldn't find SpawnPoints. \"MissionGroup\\\\SpawnPoints\" ");
}

//$BotSpawnInfo::

//$BotSpawnInfo::Data[%this, info]
//$BotSpawnInfo::Data[%this, maxspawn]
//$BotSpawnInfo::Data[%this, minrad]
//$BotSpawnInfo::Data[%this, maxrad]
//$BotSpawnInfo::Data[%this, mindelay]
//$BotSpawnInfo::Data[%this, maxdelay]
//$BotSpawnInfo::Data[%this, indexes]
//$BotSpawnInfo::Data[%this, indexesCnt]

function SpawnLoop(%this) {
	//dbecho($dbechoMode, "SpawnLoop(" @ %this @ ")");

%delay = floor(getRandom() * ($BotSpawnInfo::Data[%this, maxdelay] - $BotSpawnInfo::Data[%this, mindelay])) + $BotSpawnInfo::Data[%this, mindelay];

	%flag = "";
	if($SelectiveZoneBotSpawning) {
		if(Zone::getNumPlayers($zone[%this]) > 0 || $zone[%this] == "")
			%flag = True;
	}
	else
		%flag = True;

	if(%flag && $numAIperSpawnPoint[%this] < $BotSpawnInfo::Data[%this, maxspawn]) {

		%index = GetWord($BotSpawnInfo::Data[%this, indexes], floor(getRandom() * ($BotSpawnInfo::Data[%this, indexesCnt])));

		%AIname = AI::helper($spawnIndex[%index], $spawnIndex[%index], "SpawnPoint " @ %this);
	}

	// always call back the spawn loop, in case a spot is freed up for a helper to spawn
      schedule("SpawnLoop(" @ %this @ ");", %delay);
}
