//
//
//
function newTeam(%teamNum)
{
   focusServer();
   if(!isObject(MissionGroup))
   {
      echo("Must create a mission first.");
      focusClient();
      return;
   }
	newObject("Team" @ %teamNum, TeamGroup);
	addToSet("MissionGroup\\Teams",  "Team" @ %teamNum);

	// Default team setup
	//
   newObject("DropPoints", SimGroup);
	newObject("Start", SimGroup);
	newObject("Random", SimGroup);
	addToSet("DropPoints", "Start", "Random");
	addToSet("MissionGroup\\Teams\\team" @ %teamNum, DropPoints);
   focusClient();
}

//
//
//
function newMission(%missionName, %worldName, %timeOfDay)
{
   if(focusServer)
   {
	   if(isObject(MissionGroup))
      {
		   echo("Already running a mission");
   		focusClient();
		   return;
      }
   }
   focusClient();
   if(%timeOfDay == "")
   {
	   echo("newFearMission: missionName worldName timeOfDay");
      return;
   }

   %worldVolName = strcat(%worldName, "World", ".vol");
   %terrVolName = strcat(%worldName, "Terrain", ".vol");
   %terrFileName = strcat(%missionName, ".dtf");
   %rulesName = strcat(%worldName, ".rules.dat");
   %dmlName = strcat(%worldName, ".dml");
   %palName = strcat(%worldName, ".", %timeOfDay, ".ppl");
   %gridName = strcat(%worldName, ".grid.dat");
   %tedFileName = strcat(%missionName, ".ted");
   %dscName = "base\\missions\\" @ %missionName @ ".dsc";
   %missionName = strcat("base\\missions\\", %missionName, ".mis");

   // Default groups
   //
   echo("Default Groups");
   newObject("MissionGroup", SimGroup);
   newObject("Volumes", SimGroup);
   newObject("World", SimGroup);
   newObject("Landscape", SimGroup);
   newObject("Lights", SimGroup);
   addToSet("MissionGroup", "Volumes", "World", "Landscape", "Lights" );

   // Default volumes
   //

   // entities.vol loading has been moved to console.cs
   // newObject("Entities", SimVolume, "entities.vol");

   newObject("World", SimVolume , %worldVolName);
   newObject("WorldTerrain", SimVolume , %terrVolName);
   newObject("InterfaceVol", SimVolume, "interface.vol");
   addToSet("MissionGroup\\Volumes", "InterfaceVol", "Entities", "World", "WorldTerrain");

   // Register the world's DML volumes (Contained in %terrVolName)
   //
   exec("createWorldVolumes.cs");

   // World palette
   //
   //echo("Default World palette");
   newObject("Palette", SimPalette, %palName, true);
   addToSet("MissionGroup\\World", "Palette");

   // Default Sky
   //
   //echo("Default Sky");
   newObject("Sky", Sky);
   addToSet("MissionGroup\\Landscape", "Sky");

   // Default sun light
   //echo("Default Sun Light");
   newObject("Sun", Planet, 0, 60, 0, T, F, 1, 1, 1, 0, 0, 0);
   addToSet("MissionGroup\\Landscape", "Sun");

   // Default Team 0
   //
   newObject("Teams", SimGroup);
   addToSet(MissionGroup, Teams);
   newTeam(0);

   // Create flat sim terrain object
   //
   //echo("Default Terrain");
   newObject("Terrain", SimTerrain, Create, %terrFileName, 3, 3, 256, 0, 0, -3072, -3072, 0, 0, 0, 0);
	newObject("MissionCenter", MissionCenterPos, -1024, -1024, 2048, 2048);
   setTerrainDetail(Terrain, 100, 120);
   setTerrainVisibility(Terrain, 1500, 700);
   SetTerrainContainer(Terrain,"0 0 -20.0",0,10000);  //  (Terrain,Gravity,Drag,Height)
   LSCreate(1);
   LSRules(%rulesName);
   LSTextures(%dmlName, %gridName);
   LSApplyLandScape();
   LSApplyTextures();

   // Light the terrain and save it off
   //
   //echo("Light terrain here...");
   //lightTerrain(Terrain);
   saveTerrain(Terrain, "base\\missions\\" @ %tedFileName);

   // force resource manager to re-search its path.

   $MDESC::Type = "New Mission";
   $MDESC::Planet = "unknown";
   $MDESC::Weather = "Clear";
   $MDESC::TimeOfDay = "Midday";
   $MDESC::TeamCount = 1;
   $MDESC::Desc = "Blah Blab Blah";

   export("$MDESC::*", %dscName);

   $ConsoleWorld::DefaultSearchPath = $ConsoleWorld::DefaultSearchPath;

   newObject("TedFile", SimVolume, "missions\\" @ %tedFileName);

   addToSet("MissionGroup\\Volumes", "TedFile");
   addToSet("MissionGroup\\Landscape", "Terrain");
   addToSet("MissionGroup\\World", "MissionCenter");

   // Save the mission file
   //
   //echo("Store Mission");
	exportObjectToScript(MissionGroup, "", %missionName);
   deleteObject( MissionGroup );
   purgeResources();
   echo("Done");
}
