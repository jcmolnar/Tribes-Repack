$Server::teamName[0] = "Blood Eagle";
$Server::teamSkin[0] = "beagle";
$Server::teamName[1] = "Diamond Sword";
$Server::teamSkin[1] = "dsword";
$Server::teamName[2] = "Children of the Phoenix";
$Server::teamSkin[2] = "cphoenix";
$Server::teamName[3] = "Starwolf";
$Server::teamSkin[3] = "swolf";
$Server::teamName[4] = "Generic 1";
$Server::teamSkin[4] = "base";
$Server::teamName[5] = "Generic 2";
$Server::teamSkin[5] = "base";
$Server::teamName[6] = "Generic 3";
$Server::teamSkin[6] = "base";
$Server::teamName[7] = "Generic 4";
$Server::teamSkin[7] = "base";

$Server::HostName = "TRIBES Server";
$Server::MaxPlayers = "8";
$Server::HostPublicGame = false;
$Server::AutoAssignTeams = true;
$Server::Port = "28001";

$Server::timeLimit = 25;
$Server::warmupTime = 20;

if($pref::lastMission == "")
   $pref::lastMission = Raindance;

$Server::MinVoteTime = 45;
$Server::VotingTime = 20;
$Server::VoteWinMargin = 0.55;
$Server::VoteAdminWinMargin = 0.66;
$Server::MinVotes = 1;
$Server::MinVotesPct = 0.5;
$Server::VoteFailTime = 30; // 30 seconds if your vote fails + $Server::MinVoteTime

$Server::TourneyMode = false;

// NATIVE-PORT (SpoonBot): default OFF. $Server:: not $pref:: -- export("pref::*")
// sweeps the whole namespace at exit and would persist this into every install.
// Takes effect on the NEXT createServer, i.e. a server restart, not a mission change.
$Server::SpoonBots = 0;

// NATIVE-PORT (Track C4): route bots over the native nav graph
// (config\nav\<mission>.nav)
// instead of SpoonBot treefiles. DEFAULT OFF -- Track A is unchanged unless this is set,
// and with it off not one line of hooks_nav.cs is even exec'd. Requires SpoonBots = 1.
$Server::SpoonBotsNativeNav = 0;
$Server::TeamDamageScale = 0;

// NATIVE-PORT (MOTD, Joe 2026-08-29): the server description a browser shows. This is the
// SHIPPING default -- console.cs execs serverDefaults.cs before serverPrefs.cs, so this is what
// a fresh install advertises until the player edits it, and it is what every mod without its
// own serverDefaults.cs inherits (MechMayhem and Annihilation both do).
//
// It said "Default TRIBES server setup / Admin: Unknown / Email: Unknown" -- stock 1998 text.
// Separately, every config pack's exported ServerPrefs.cs carried
// "Running RPG Mod 6.9 - www.tribesrpg.org", one old RPG session fossilised into base,
// MechMayhem and Annihilation alike (mods sweep their whole $Server::* namespace into
// ServerPrefs.cs on exit -- see the leak documented at main.cpp:383). Those exports are fixed
// in the play tree, but they are user state that rewrites on quit, so THIS line is the part
// that actually ships correct on a fresh install.
$Server::Info = "Modern Tribes";
$Server::JoinMOTD = "<jc><f1>Message of the Day:\nWelcome to TRIBES!\n\nFire to spawn.";

$Server::MasterAddressN0 = "kigen.ath.cx:28000 t1m1.tribesmasterserver.com:28000 skbmaster.ath.cx:28000 t1m1.pu.net:28000 t1m1.tribes0.com t1m1.masters.dynamix.com:28000 t1m2.masters.dynamix.com:28000 t1m3.masters.dynamix.com:28000";
$Server::MasterAddressN1 = "kigen.ath.cx:28000 t1m1.tribesmasterserver.com:28000 skbmaster.ath.cx:28000 t1m1.pu.net:28000 t1m1.tribes0.com t1ukm1.masters.dynamix.com:28000 t1ukm2.masters.dynamix.com:28000 t1ukm3.masters.dynamix.com:28000";
$Server::MasterAddressN2 = "kigen.ath.cx:28000 t1m1.tribesmasterserver.com:28000 skbmaster.ath.cx:28000 t1m1.pu.net:28000 t1m1.tribes0.com t1aum1.masters.dynamix.com:28000 t1aum2.masters.dynamix.com:28000 t1aum3.masters.dynamix.com:28000";
$Server::MasterName0 = "US Tribes Master";
$Server::MasterName1 = "UK Tribes Master";
$Server::MasterName2 = "Australian Tribes Master";
$Server::CurrentMaster = 0;

$Server::respawnTime = 2; // number of seconds before a respawn is allowed

// default translated masters:
//$Server::XLMasterN0 = "IP:209.185.222.237:28000";
//$Server::XLMasterN1 = "IP:209.67.28.148:28000";
//$Server::XLMasterN2 = "IP:198.74.40.67:28000";
//$Server::XLMasterN3 = "IP:70.250.189.58:28000";
//$Server::XLMasterN4 = "IP:216.249.100.66:28000";
$Server::XLMasterN0 = "IP:75.126.191.58:28000";
$Server::XLMasterN1 = "IP:66.39.167.52:28000";
$Server::XLMasterN2 = "IP:216.249.100.66:28000";
$Server::XLMasterN3 = "IP:209.223.236.114:28000";
$Server::XLMasterN4 = "IP:209.223.236.114:28000";
$Server::XLMasterN5 = "IP:66.39.167.52:28000";
$Server::XLMasterN6 = "IP:216.249.100.66:28000";
$Server::XLMasterN7 = "IP:209.223.236.114:28000";
$Server::FloodProtectionEnabled = true;
