// config\clientDefaults.cs -- THE master client defaults (Modern Tribes).
//
// console.cs does exec("clientDefaults.cs") by bare name and "config" is the
// FIRST entry on the resource search path, so this file SHADOWS every per-mod
// copy (base\scripts, RPG\scripts, RMRPG, SWRPG, StarWars, DeltaAirForce, TSC).
// Those copies disagreed with each other (packetRate 30 vs 15 vs 10, packetSize
// 400 vs 200, shadow detail 1 vs 0) and most set values UNCONDITIONALLY, so the
// first mod you ever booted decided your settings. This file is the single
// authority: every default is guarded, so a value the player has saved (or set
// in Options) is NEVER overwritten. Ships via the repack whitelist.
//
// NOTE: every if() body uses braces -- a braceless top-level if is a SILENT
// syntax error in this console and the whole file would do nothing.

if($pref::ShadowDetailMask == "")       { $pref::ShadowDetailMask = 7; }
if($pref::shadowDetailScale == "")      { $pref::shadowDetailScale = 1; }

// Network (the modern-connection values; both ends clamp, safe on any server)
if($pref::packetRate == "")             { $pref::packetRate = 30; }
if($pref::packetSize == "")             { $pref::packetSize = 400; }
if($pref::packetFrame == "")            { $pref::packetFrame = 32; }

// Command map
if($pref::mapFilter == "")              { $pref::mapFilter = 15; }
if($pref::mapNames == "")               { $pref::mapNames = true; }
if($pref::mapSensorRange == "")         { $pref::mapSensorRange = true; }
if($pref::mapSensorTranslucent == "")   { $pref::mapSensorTranslucent = false; }

// Player / view
if($pref::PlayerZoomSpeed == "")        { $pref::PlayerZoomSpeed = 0.01; }
if($pref::PlayerFov == "")              { $pref::PlayerFov = 90; }

// Console / browser
if($Console::History == "")             { $Console::History = 45; }
if($pref::ConnectionGoodPing == "")     { $pref::ConnectionGoodPing = 250; }
if($pref::ConnectionPoorPing == "")     { $pref::ConnectionPoorPing = 350; }
if($pref::maxConcurrentPings == "")     { $pref::maxConcurrentPings = 10; }
if($pref::pingTimeoutTime == "")        { $pref::pingTimeoutTime = 900; }
if($pref::pingRetryCount == "")         { $pref::pingRetryCount = 4; }
if($pref::maxConcurrentRequests == "")  { $pref::maxConcurrentRequests = 6; }
if($pref::requestTimeoutTime == "")     { $pref::requestTimeoutTime = 900; }
if($pref::requestRetryCount == "")      { $pref::requestRetryCount = 2; }
if($pref::resolveHostnames == "")       { $pref::resolveHostnames = True; }
if($pref::noIpx == "")                  { $pref::noIpx = false; }
if($pref::lanOnly == "")                { $pref::lanOnly = false; }

// Game / UI
if($pref::quickStart == "")             { $pref::quickStart = true; }
if($pref::lastTrainingMission == "")    { $pref::lastTrainingMission = "1_Welcome"; }
if($pref::autoWaypoint == "")           { $pref::autoWaypoint = true; }
if($pref::helpPopups == "")             { $pref::helpPopups = true; }
if($pref::VideoFullScreen == "")        { $pref::VideoFullScreen = True; }
if($pref::noEnterInvStation == "")      { $pref::noEnterInvStation = false; }
if($pref::messageMask == "")            { $pref::messageMask = -1; }
if($pref::filterBadWords == "")         { $pref::filterBadWords = true; }
