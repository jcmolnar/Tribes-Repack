$Server::HostName = "Star Wars RPG: www.swrpg.info";
$Server::MaxPlayers = "8";
$Server::HostPublicGame = true;
$Server::AutoAssignTeams = true;
$Server::Port = "28001";

$Server::timeLimit = 0;
$Server::warmupTime = 10;

if($pref::lastMission == "")
   $pref::lastMission = Tatooine;

$Server::MinVoteTime = 45;
$Server::VotingTime = 20;
$Server::VoteWinMargin = 0.55;
$Server::VoteAdminWinMargin = 0.66;
$Server::MinVotes = 1;
$Server::MinVotesPct = 0.5;
$Server::VoteFailTime = 30; // 30 seconds if your vote fails + $Server::MinVoteTime

$Server::TourneyMode = false;
$Server::TeamDamageScale = 1;

$Server::Info = "Running Star Wars RPG mod TESTING.\n<f1>www.swrpg.info <f0>to download the <f2>required<f1> client files.\n<f1>www.swrpg.info/forum <f0>to chat.\nAdmins: Hazor, Road-DoGg/Nevmin\ne-mail: <f2>hazor@swrpg.info<f0>\nPowered by RPG Mod ver 5.004\nplanettribes.com/rpg";
$Server::JoinMOTD = "<jc><f1>Got questions? Want to report a bug? Send an e-mail to hazor@swrpg.info.";

$Server::MasterAddressN0 = "t1m1.tribes0.com:28000 t1m1.masters.dynamix.com:28000 t1m2.masters.dynamix.com:28000 t1m3.masters.dynamix.com:28000";
$Server::MasterAddressN1 = "t1m1.tribes0.com:28000 t1ukm1.masters.dynamix.com:28000 t1ukm2.masters.dynamix.com:28000 t1ukm3.masters.dynamix.com:28000";
$Server::MasterAddressN2 = "t1m1.tribes0.com:28000 t1aum1.masters.dynamix.com:28000 t1aum2.masters.dynamix.com:28000 t1aum3.masters.dynamix.com:28000";
$Server::MasterName0 = "US Tribes Master";
$Server::MasterName1 = "UK Tribes Master";
$Server::MasterName2 = "Australian Tribes Master";
$Server::CurrentMaster = 0;

$Server::respawnTime = 0; // number of seconds before a respawn is allowed

// default translated masters:
$Server::XLMasterN0 = "IP:209.185.222.237:28000";
$Server::XLMasterN1 = "IP:209.67.28.148:28000";
$Server::XLMasterN2 = "IP:198.74.40.67:28000";
$Server::FloodProtectionEnabled = false;

$Server::FileURL = "http://www.swrpg.info/";
