$numArenaPlayers = 0;
$ArenaSpawnIndexes = "44";
$teleportInArenaCost = 0;
$maxroster = 0;
$ArenaPickDuelersTime = 200;
$ArenaStartTime = 300;
$DoCheckMatchWin = False;
$ArenaBotMatchLengthInTicks = 36;
$ArenaBotMatchTicker = 0;
function InitArena() { }
function ScheduleArenaMatch() { }
function StartArenaMatch() { }
function GiveArenaEquipment(%clientId) { }
function CreateArenaDueler(%num) { }
function AddToRoster(%clientId) { }
function CreateArenaStorage(%clientId) { }
function RemoveFromRoster(%clientId) { }
function IsInRoster(%clientId) { }
function ClearRoster() { }
function ClearArenaDueler() { }
function GetArenaDuelerIndex(%clientId) { }
function IsInArenaDueler(%clientId) { }
function IsStillArenaFighting(%clientId) { }
function RemoveFromArenaDueler(%clientId) { }
function RemoveArenaEquipment(%clientId) { }
function RestoreArenaStorage(%clientId) { }
function CheckMatchWin() { }
function ReturnToArenaLobby() { }
function FillWaitingRoom() { }
function DetermineArenaPrize() { }
function RestorePreviousEquipment() { }
function RefreshArenaTextBox() { }
function StringArenaTextBox() { }
function CloseArenaTextBox() { }
function CheckAndBootFromArena() { }