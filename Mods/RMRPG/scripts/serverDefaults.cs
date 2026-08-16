$Server::teamName[0] = "Northfolk";
$Server::teamSkin[0] = "rpghuman";
$Server::teamName[1] = "Enemy";
$Server::teamSkin[1] = "RMSkins1";
$Server::teamName[2] = "Enemy";
$Server::teamSkin[2] = "RMSkins2";
$Server::teamName[3] = "Enemy";
$Server::teamSkin[3] = "RMSkins3";
$Server::teamName[4] = "Enemy";
$Server::teamSkin[4] = "RMSkins4";
$Server::teamName[5] = "Golom";
$Server::teamSkin[5] = "DaFathom";
$Server::teamName[6] = "Creatures";
$Server::teamSkin[6] = "min";
$Server::teamName[7] = "Uber";
$Server::teamSkin[7] = "fedmonster";

$Server::HostName = "RMRPG MOD Server";
$Server::MaxPlayers = "16";
$Server::HostPublicGame = true;
$Server::AutoAssignTeams = true;
$Server::Port = "28002";

$Server::timeLimit = 0;
$Server::warmupTime = 10;

if($pref::lastMission == "")
   $pref::lastMission = "RMR"; // chee4 has no .mis (24-byte stub .vol only); RMR is the real RMRPG map

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

$Server::MasterAddressN0 = "t1m1.masters.dynamix.com:28000 t1m2.masters.dynamix.com:28000 t1m3.masters.dynamix.com:28000";
$Server::MasterAddressN1 = "t1ukm1.masters.dynamix.com:28000 t1ukm2.masters.dynamix.com:28000 t1ukm3.masters.dynamix.com:28000";
$Server::MasterAddressN2 = "t1aum1.masters.dynamix.com:28000 t1aum2.masters.dynamix.com:28000 t1aum3.masters.dynamix.com:28000";
$Server::MasterName0 = "US Tribes Master";
$Server::MasterName1 = "UK Tribes Master";
$Server::MasterName2 = "Australian Tribes Master";
$Server::CurrentMaster = 0;

$Server::respawnTime = 7; // number of seconds before a respawn is allowed

// default translated masters:
$Server::XLMasterN0 = "IP:209.185.222.237:28000";
$Server::XLMasterN1 = "IP:209.67.28.148:28000";
$Server::XLMasterN2 = "IP:198.74.40.67:28000";
$Server::FloodProtectionEnabled = false;
