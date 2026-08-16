$Server::HostName = "RPG v6.5";
$Server::MaxPlayers = "16";
$Server::HostPublicGame = true;
$Server::AutoAssignTeams = true;
$Server::Port = "28001";

$Server::timeLimit = 0;
$Server::warmupTime = 10;

if($pref::lastMission == "")
   $pref::lastMission = rpgmap6;

$Server::MinVoteTime = 45;
$Server::VotingTime = 20;
$Server::VoteWinMargin = 0.55;
$Server::VoteAdminWinMargin = 0.66;
$Server::MinVotes = 1;
$Server::MinVotesPct = 0.5;
$Server::VoteFailTime = 30; // 30 seconds if your vote fails + $Server::MinVoteTime

$Server::TourneyMode = false;
$Server::TeamDamageScale = 1;

$Server::Info = "";
$Server::JoinMOTD = "";

$Server::MasterAddressN0 = "t1m1.pu.net:28000 tribes.lock-load.org:28000 t1m1.kigen.co:28000 t1m2.kigen.co:28000 t1m1.tribesmasterserver.com:28000 t1m1.tribes0.com:28000 t1m1.masters.dynamix.com:28000 t1m2.masters.dynamix.com:28000 t1m3.masters.dynamix.com:28000";
$Server::MasterAddressN1 = "t1m1.pu.net:28000 tribes.lock-load.org:28000 t1m1.kigen.co:28000 t1m2.kigen.co:28000 t1m1.tribesmasterserver.com:28000 t1m1.tribes0.com:28000 t1ukm1.masters.dynamix.com:28000 t1ukm2.masters.dynamix.com:28000 t1ukm3.masters.dynamix.com:28000";
$Server::MasterAddressN2 = "t1m1.pu.net:28000 tribes.lock-load.org:28000 t1m1.kigen.co:28000 t1m2.kigen.co:28000 t1m1.tribesmasterserver.com:28000 t1m1.tribes0.com:28000 t1aum1.masters.dynamix.com:28000 t1aum2.masters.dynamix.com:28000 t1aum3.masters.dynamix.com:28000";
$Server::MasterName0 = "US Tribes Master";
$Server::MasterName1 = "UK Tribes Master";
$Server::MasterName2 = "Australian Tribes Master";
$Server::CurrentMaster = 0;

$Server::respawnTime = 0; // number of seconds before a respawn is allowed

// default translated masters:
//$Server::XLMasterN0 = "IP:209.185.222.237:28000";
//$Server::XLMasterN1 = "IP:209.67.28.148:28000";
//$Server::XLMasterN2 = "IP:198.74.40.67:28000";
$Server::XLMasterN0 = "IP:75.126.191.58:28000";
$Server::XLMasterN1 = "IP:66.39.167.52:28000";
$Server::XLMasterN2 = "IP:216.249.100.66:28000";
$Server::XLMasterN3 = "IP:209.223.236.114:28000";
$Server::XLMasterN4 = "IP:209.223.236.114:28000";
$Server::XLMasterN5 = "IP:66.39.167.52:28000";
$Server::XLMasterN6 = "IP:216.249.100.66:28000";
$Server::XLMasterN7 = "IP:209.223.236.114:28000";
$Server::FloodProtectionEnabled = false;

$Server::FileURL = "http://tribesrpg.org";
