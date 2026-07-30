//Dedicated Football Server Prefs file by Spike <spike@amishrabbit.com>
//Football FTP archive: http://amishrabbit.com/tribes/football
//Football on IRC: irc.dynamix.com channel #football -- all welcome
//Updated 2 September 2003

$Server::HostName = "Football"; //name that shows up in server list
$Server::MaxPlayers = "8"; //MAX should be 8 for cable/DSL, 16 for T1, 32 for T3 or fatter pipe.
$Server::HostPublicGame = "True";
$Server::Info = "SERVERNAME\nAdmin: NAME (EMAIL)\nMod info: amishrabbit.com/tribes/football"; 

//displays as the player connects to the server
$MODInfo = "<f0>It's Football--Tribes style!\n\n<f1>'' If all the year were playing holidays, To sport would be as tedious as to work. ''  --Shakespeare";

//displays after they connect but before they join a team.
//MAXIMUM 71 CHARS PER MOTD STRING
$Server::JoinMOTD = "<f1><jc>Welcome to Tribes Football 1.3 (Tasermod build)\n<f2>Hang up your spinfusor at the door.\nYou won't need it here.";

$Server::Port = "28001";
$Server::Password = "";
$Server::TimeLimit = "15";
$Server::AutoAssignTeams = "True";
$Server::TourneyMode = "False";
$Server::TeamDamageScale = "0";
$Server::HostPublicGame = "True";
$AdminPassword = "CHANGEME";
$Server::warmupTime = 20;
$Server::respawnTime = 1;
$Server::VotingTime = 15;
$Server::VoteWinMargin = 0.80;
$telnetport = "11111"; //change to something else
$telnetpassword = "CHANGEMETOO";
$pref::PacketRate = "15"; //lower to 10 for modem players
$pref::PacketSize = "300"; //lower to 200 for modem players
$TCTimer = 10; //Teamchange timer: you cannot change teams again after a team change for x seconds
$VoteAdminAllowed = false; //vote admin function on or off
$halftimeDelay = 20; //delay (seconds) before game resumes at half time, if under 5 - no halftime

//Team names and skins
$Server::teamSkin0 = "beagle"; //Blood Eagle skins - red
$Server::teamName0 = "TEAM 1"; //this name shows up in the score window
$Server::teamSkin1 = "NFLPATRIOTS"; //use "green" if you don't have the football skins
$Server::teamName1 = "TEAM 2"; //this name shows up in the score window

//Additional Code
$console::logmode=1; //set to 0 if you want NO LOG

//football missions only
exec(missionlist);
MissionList::clear();
Missionlist::initNextMission();
$pref::lastmission = "HighSchool"; //first mission to launch when you startup

//Missionlist: order in reverse alphabetical here to make them alpha in the menu
MissionList::addMission("TunnelBall");
MissionList::addMission("TK-421_Extended_Stadium");
MissionList::addMission("TK-421_Stadium");
MissionList::addMission("TheRuins");
MissionList::addMission("Tech_Ball");
MissionList::addMission("SpaceStation_Stadium");
MissionList::addMission("SpaceBall");
MissionList::addMission("SnowField");
MissionList::addMission("Snowball_Express2");
MissionList::addMission("Snowball_Express");
MissionList::addMission("SlamDunk_Stadium");
MissionList::addMission("Shooters_Stadium");
MissionList::addMission("RomanFootBall");
MissionList::addMission("Redshirt_Memorial_Stadium");
MissionList::addMission("RamppedTouchdown");
MissionList::addMission("PrisonCampRedux");
MissionList::addMission("PrisonCamp");
MissionList::addMission("PikaDropStadium");
MissionList::addMission("Official_Stadium9");
MissionList::addMission("Official_Stadium8");
MissionList::addMission("Official_Stadium7");
MissionList::addMission("Official_Stadium6");
MissionList::addMission("Official_Stadium5");
MissionList::addMission("Official_Stadium4");
MissionList::addMission("Official_Stadium3");
MissionList::addMission("Official_Stadium2");
MissionList::addMission("Official_Stadium1");
MissionList::addMission("NoFear_Stadium");
MissionList::addMission("MudBowlMX");
MissionList::addMission("MudBowl");
MissionList::addMission("MoonJump_Stadium");
MissionList::addMission("MontyCrisco_Stadium");
MissionList::addMission("Midnight_Run");
MissionList::addMission("Longest_Yard");
MissionList::addMission("LN2_Stadium");
MissionList::addMission("LaputaStadium");
MissionList::addMission("Kings_Stadium");
MissionList::addMission("InDoorsArena");
MissionList::addMission("IndoorFootballStyle");
MissionList::addMission("IceBall");
MissionList::addMission("HoopsMX");
MissionList::addMission("Hoops");
MissionList::addMission("HolyMoly_Stadium");
MissionList::addMission("HighSchool");
MissionList::addMission("Highjump_Stadium");
MissionList::addMission("HighEndzone");
MissionList::addMission("HeiligeScheisseStadium");
MissionList::addMission("HalfpipeMX_Stadium");
MissionList::addMission("Halfpipe_Stadium");
MissionList::addMission("Ghostrider_Stadium");
MissionList::addMission("Frisbee_ConversionA");
MissionList::addMission("Fortress_Stadium");
MissionList::addMission("football");
MissionList::addMission("FlyingNights_Stadium");
MissionList::addMission("FlyingKnightsMX_Stadium");
MissionList::addMission("FlyBall");
MissionList::addMission("Extreme_Stadium");
MissionList::addMission("EEPROM_Stadium");
MissionList::addMission("DesertHigh_StadiumMX");
MissionList::addMission("DesertHigh_Stadium");
MissionList::addMission("DesertBowl");
MissionList::addMission("Cyclone_Stadium");
MissionList::addMission("Cavern_Stadium");
MissionList::addMission("BlueBalls_Stadium");

//Rotation paradigm: $nextMission["CURRENT"] = "NEXT";
$nextMission["football"] = "RamppedTouchdown";
$nextMission["RamppedTouchdown"] = "Official_Stadium1";
$nextMission["Official_Stadium1"] = "Official_Stadium2";
$nextMission["Official_Stadium2"] = "Official_Stadium3";
$nextMission["Official_Stadium3"] = "Official_Stadium4";
$nextMission["Official_Stadium4"] = "Official_Stadium5";
$nextMission["Official_Stadium5"] = "Official_Stadium6";
$nextMission["Official_Stadium6"] = "Official_Stadium7";
$nextMission["Official_Stadium7"] = "Official_Stadium8";
$nextMission["Official_Stadium8"] = "Official_Stadium9";
$nextMission["Official_Stadium9"] = "Snowball_Express2";
$nextMission["Snowball_Express2"] = "DesertHigh_Stadium";
$nextMission["DesertHigh_Stadium"] = "EEPROM_Stadium";
$nextMission["EEPROM_Stadium"] = "DesertHigh_StadiumMX";
$nextMission["DesertHigh_StadiumMX"] = "Halfpipe_Stadium";
$nextMission["Halfpipe_Stadium"] = "Hoops";
$nextMission["Hoops"] = "BlueBalls_Stadium";
$nextMission["BlueBalls_Stadium"] = "RomanFootBall";
$nextMission["RomanFootBall"] = "TunnelBall";
$nextMission["TunnelBall"] = "Tech_Ball";
$nextMission["Tech_Ball"] = "FlyingNights_Stadium";
$nextMission["FlyingNights_Stadium"] = "Cyclone_Stadium";
$nextMission["Cyclone_Stadium"] = "MontyCrisco_Stadium";
$nextMission["MontyCrisco_Stadium"] = "Kings_Stadium";
$nextMission["Kings_Stadium"] = "SlamDunk_Stadium";
$nextMission["SlamDunk_Stadium"] = "NoFear_Stadium";
$nextMission["NoFear_Stadium"] = "Ghostrider_Stadium";
$nextMission["Ghostrider_Stadium"] = "Geometric_Stadium";
$nextMission["Geometric_Stadium"] = "FlyingKnightsMX_Stadium";
$nextMission["FlyingKnightsMX_Stadium"] = "Extreme_Stadium";
$nextMission["Extreme_Stadium"] = "HalfpipeMX_Stadium";
$nextMission["HalfpipeMX_Stadium"] = "HoopsMX";
$nextMission["HoopsMX"] = "Shooters_Stadium";
$nextMission["Shooters_Stadium"] = "Longest_Yard";
$nextMission["Longest_Yard"] = "Midnight_Run";
$nextMission["Midnight_Run"] = "MudBowl";
$nextMission["MudBowl"] = "LN2_Stadium";
$nextMission["LN2_Stadium"] = "MoonJump_Stadium";
$nextMission["MoonJump_Stadium"] = "Frisbee_ConversionA";
$nextMission["Frisbee_ConversionA"] = "HolyMoly_Stadium";
$nextMission["HolyMoly_Stadium"] = "MudBowlMX";
$nextMission["MudBowlMX"] = "SpaceStation_Stadium";
$nextMission["SpaceStation_Stadium"] = "Highjump_Stadium";
$nextMission["Highjump_Stadium"] = "DesertBowl";
$nextMission["DesertBowl"] = "FlyBall";
$nextMission["FlyBall"] = "Fortress_Stadium";
$nextMission["Fortress_Stadium"] = "HighEndzone";
$nextMission["HighEndzone"] = "HighSchool";
$nextMission["HighSchool"] = "IceBall";
$nextMission["IceBall"] = "InDoorsArena";
$nextMission["InDoorsArena"] = "PrisonCamp";
$nextMission["PrisonCamp"] = "HeiligeScheisseStadium";
$nextMission["HeiligeScheisseStadium"] = "LaputaStadium";
$nextMission["LaputaStadium"] = "TK-421_Extended_Stadium";
$nextMission["TK-421_Extended_Stadium"] = "football";
//last rotation map should always point back to the first.
