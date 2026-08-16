// ---------------------------------------------------------------------------
// Red Moon RPG (RMRPG) dedicated-server config.
//
// Why this file exists:
//   Hosting shares the single root config\ folder with the RPG (Kronos) mod.
//   The engine execs config\ServerPrefs.cs during boot, which carries RPG-mod
//   values ($pref::LastMission = "rpgmap6", HostName "RPG v6.7", MaxPlayers 7).
//   Under -mod rmrpg that made the server try to load rpgmap6.mis (an RPG map
//   RMRPG can't load) and crash on spawn.
//
//   RMRPG\scripts\Server.cs execs this file at the top of createServer() -- the
//   same pattern RPG\scripts\Server.cs uses with rpgserv.cs -- so these values
//   win over ServerPrefs.cs right before the mission is chosen. Only RMRPG execs
//   it, so RPG/Kronos hosting is unaffected.
//
//   Launch via rmrpg-server.bat (NativeTribes.exe -mod rmrpg -dedicated). This
//   override applies regardless of which exe hosts.
// ---------------------------------------------------------------------------

$Server::HostName    = "Red Moon RPG";  // Server name shown in the browser
$Server::Password    = "";              // Server password ("" = open)
$Server::JoinPassword = "";
$Server::MaxPlayers  = "16";            // Max players
$Server::Port        = "28002";         // Listen port (forward this in your router) - RMRPG-only; RPG/Kronos uses 28001
$Server::HostPublicGame = "true";       // Advertise to the master list?
$Server::FloodProtectionEnabled = "false";

// Live community master servers (copied from config\ServerPrefs.cs = the working
// Kronos setup). RMRPG\serverDefaults.cs points only at the DEAD Dynamix masters
// (t1m1.masters.dynamix.com), so RMRPG heartbeats nowhere reachable and never
// shows on the list. These execs run in createServer AFTER serverDefaults, so
// they win -- and only for RMRPG.
$Server::CurrentMaster = "0";
$Server::numMasters = "9";
$Server::MasterAddressN0 = "t1m1.pu.net:28000 tribes.lock-load.org:28000 t1m1.kigen.co:28000 t1m2.kigen.co:28000 t1m1.tribesmasterserver.com:28000 t1m1.tribes0.com:28000 t1m1.masters.dynamix.com:28000 t1m2.masters.dynamix.com:28000 t1m3.masters.dynamix.com:28000";
$Server::MasterAddressN1 = "t1m1.pu.net:28000 tribes.lock-load.org:28000 t1m1.kigen.co:28000 t1m2.kigen.co:28000 t1m1.tribesmasterserver.com:28000 t1m1.tribes0.com:28000 t1ukm1.masters.dynamix.com:28000 t1ukm2.masters.dynamix.com:28000 t1ukm3.masters.dynamix.com:28000";
$Server::MasterAddressN2 = "t1m1.pu.net:28000 tribes.lock-load.org:28000 t1m1.kigen.co:28000 t1m2.kigen.co:28000 t1m1.tribesmasterserver.com:28000 t1m1.tribes0.com:28000 t1aum1.masters.dynamix.com:28000 t1aum2.masters.dynamix.com:28000 t1aum3.masters.dynamix.com:28000";
$Server::XLMasterN0 = "IP:16.58.93.213:28000";
$Server::XLMasterN1 = "IP:72.54.15.185:28000";
$Server::XLMasterN2 = "IP:15.204.204.222:28000";
$Server::XLMasterN3 = "IP:51.81.187.48:28000";
$Server::XLMasterN4 = "IP:204.80.187.27:28000";
$Server::XLMasterN5 = "IP:173.24.176.235:28000";
$Server::XLMasterN6 = "IP:216.249.100.66:28000";
$Server::XLMasterN7 = "IP:209.223.236.114:28000";

// The only RMRPG mission with a real .mis. "chee4" (old default in
// serverDefaults.cs) has no .mis -- only a 24-byte stub .vol -- so it can't load.
$pref::LastMission   = "RMR";

// Admin passwords (fill any/all; RMRPG checks $AdminPassword[1..5]).
$AdminPassword[1] = "";
$AdminPassword[2] = "";
$AdminPassword[3] = "";
$AdminPassword[4] = "";
$AdminPassword[5] = "";

$extrainfo = "";  // Extra info shown in the lobby / escape screen

$console::logmode = 1;  // Log console output to console.log (0 = off)
