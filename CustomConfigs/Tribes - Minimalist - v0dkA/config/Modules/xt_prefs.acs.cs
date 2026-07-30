// Set your average ping here.  Used for setting timeNudge on vanilla servers.
//$xt::ping = 104;

$net::timeNudge = -64;

// How far to allow the client's synced clock to drift from the server before correcting
$net::clientClockCorrection = 8;

// How far back in time to allow lag compensation to
$net::maxLagCompensation = 250;

// Customize the size of chaingun tracer bullets
$pref::tracerWidth = 0.5;
$pref::tracerLength = 0.5;

// Center third person camera above head like 1.40
$pref::newThirdPerson = false;

// Change damage flash opacity (only on XT servers)
$pref::damageFlash = 0.35;

//function XT::SetTimeNudge() {
//    if (getNetcodeVersion() >= 1.02) {
//        $net::timeNudge = 48 - $xt::ping;
//    } else {
//	$net::timeNudge = (32 + $xt::ping) * -1;
//    }
//}

//Event::Attach(eventConnected, XT::SetTimeNudge);