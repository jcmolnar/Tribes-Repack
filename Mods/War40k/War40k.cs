$TurretScoring = "true";
$War40k::Weapons = True;
//$War40k::NoBotsAtAll = TRUE;  // These two have been left in in case they are implemented later.
//$War40k::AreThereBots = TRUE;
$War40k::VoteAdmin = TRUE;
$War40k::VoteKick = TRUE;
$War40k::VoteDTD = TRUE;
$War40k::VoteFFA = TRUE;
$War40k::KeepBalanced = TRUE;
$War40k::Meteor = TRUE;
$War40k::Warlives = 5;
$RandomMissions = FALSE;
$DefaultTeamEnergy = "Infinite";
$TeamEnergy[-1] = $DefaultTeamEnergy;
$TeamEnergy[0] = $DefaultTeamEnergy;
$TeamEnergy[1] = $DefaultTeamEnergy;
$TeamEnergy[2] = $DefaultTeamEnergy;
$TeamEnergy[3] = $DefaultTeamEnergy;
$TeamEnergy[4] = $DefaultTeamEnergy;
$TeamEnergy[5] = $DefaultTeamEnergy;
$TeamEnergy[6] = $DefaultTeamEnergy;
$TeamEnergy[7] = $DefaultTeamEnergy;
$TeamEnergyCheat = 0;
$MaxTeamEnergy = 8000;
$incTeamEnergy = 50;
$secTeamEnergy = 7;
$ItemRespawnTime = 30;
$RemoteAmmoEnergy = 1000;
$RemoteInvEnergy = 5000;
$RemoteComEnergy = 1000;
$TeammateSpending = -4000;
$WarnEnergyLow = 250;
$InitialPlayerEnergy = 100;
$ServerCheats = 0;
$TestCheats = 0;
$AutoRespawn = 0;
$Server::RaceOption = 0; 

// Uncomment the following lines if you want to have someone auto adminned, auto superadminned, autokicked, or muted to all.
// If you need to add more lines be sure to increment the nuber in the []'s with each new entry
$Server::AutoAdmin[0] = "{DS} Stiletto";
$Server::AutoSuperAdmin[0] = "{DS} Stiletto";
$Server::AutoAdminAddr[0] = "IP:24.15.239.254";
$Server::AutoAdmin[1] = "[D.Lord]Guarder";
$Server::AutoSuperAdmin[1] = "<[DC]>Remmah";
$Server::AutoAdminAddr[1] = "IP:24.68.36.14";
$Server::AutoAdmin[2] = "Edgecrusher";
$Server::AutoSuperAdmin[2] = "Edgecrusher";
//$Server::AutoBan[0] = "";
//$Server::AutoBanAddr[0] = "IP:171.217.198.121";
//$Server::AutoBan[1] = "";
//$Server::AutoBanAddr[1] = "IP:171.217.198.121";
//$Server::AutoMute[0] = "";

// Use the following if you want to preset a players team so they are always on it.  Remember, teams start at 0.
//$teamPreset["{DS} Stiletto"] = 1;