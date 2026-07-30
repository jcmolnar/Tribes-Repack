// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// KillTrak.CS								Presto, March '99 
//
//	Track kills and suicides
//
//	This script watches the death messages that the server announces, and
//	identifies the killer, victim, and weapon.  It then sends an event
//		eventKillTrak(%killer, %victim, %weapon)
//	which clients can use to watch the deaths without needing to parse the
//	messages themselves.
//
//	The death messages are listed in section B, below, with an explanation
//	of the format.
//
//	This code is based on (and supercedes) the JHUD code by Josh.  I want
//	to thank him for his cool kill-tracking HUD (the JHUD) which he 
//	released soon after the first PrestoPack.  He's stopped coding scripts
//	but gave me permission to take over the kill tracking code he'd
//	written.
//
// A) The Code
// --------------------------------------------------------------------------
Include("presto\\Inventory.cs"); // for Mod...
Include("presto\\Match.cs");
Include("presto\\Event.cs");

function KillTrak::Reset() {
	for (%i = 0; %i < $KillTrak::numDeaths; %i++)
		$KillTrak::message[$KillTrak::death[%num]] = "";
	$KillTrak::numDeaths = 0;
	}

function KillTrak::DeathMessage(%msg, %weapon, %mod) {
	%num = $KillTrak::message[%msg];
	if (%num == "") {
		%num = $KillTrak::numDeaths;
		if (%num == "")
			%num = 0;
		$KillTrak::numDeaths = %num + 1;
		}

	$KillTrak::message[%msg] = %num;
	$KillTrak::death[%num] = %msg;
	$KillTrak::death[%num, mod] = %mod;
	$KillTrak::death[%num, weapon] = %weapon;
	$KillTrak::death[%num, suicide] = (String::FindSubStr(%msg, "%1") == -1);
	}

function KillTrak::GetKiller() {
	return $KillTrak::killer;
	}
function KillTrak::GetVictim() {
	return $KillTrak::victim;
	}
function KillTrak::GetWeapon() {
	return $KillTrak::weapon;
	}
function KillTrak::OnClientMessage(%client, %msg) {
	if (%client != 0)
		return;
	for (%i = 0; %i < $KillTrak::numDeaths; %i++) {
		%mod = $KillTrak::death[%i, mod];
		if (%mod != "" && %mod != $Mod::current)
			continue;
		if (Match::ParamString(%msg, $KillTrak::death[%i])) {
			$KillTrak::victim = getClientByName(Match::Result(2));
			if ($KillTrak::death[%i, suicide])
				$KillTrak::killer = $KillTrak::victim;
			else	$KillTrak::killer = getClientByName(Match::Result(1));
			$KillTrak::weapon = $KillTrak::death[%i, weapon];
			Event::Trigger(eventKillTrak, $KillTrak::killer, $KillTrak::victim, $KillTrak::weapon);
			return;
			}
		}
	return;
	}

function KillTrak::Init() {
	Event::Attach(eventClientMessage, KillTrak::OnClientMessage);
	}

if (Presto::Enabled(KillTrak)) {
	KillTrak::Init();
	}

// B) The Death Messages
//	The format of this section is:
//		KillTrak::DeathMessage(message, weapon);
//	The "message" is the text of the
//		Use %1 to match the killer's name
//		Use %2 to match the victim's name
//		Use %3 to match the killer's possessive pronoun (?)
//			For instance "his" or "her"
//		And %4 to match the victim's.
//
//	Yes, you can add new death messages!  Don't put them in this
//	file.  Just add them to your own script somewhere, for instance
//	in your autoexec.cs --
//	People who make server-side mods could also release this list
//	so that kill-tracking works for their clients.
// --------------------------------------------------------------------------
KillTrak::DeathMessage("%2 falls to %3 death.", "Falling");
KillTrak::DeathMessage("%2 forgot to tie %3 bungie cord.", "Falling");
KillTrak::DeathMessage("%2 bites the dust in a forceful manner.", "Falling");
KillTrak::DeathMessage("%2 fall down go boom.", "Falling");

KillTrak::DeathMessage("%1 makes quite an impact on %2.", "Vehicle");
KillTrak::DeathMessage("%2 becomes the victim of a fly-by from %1.", "Vehicle");
KillTrak::DeathMessage("%2 leaves a nasty dent in %1's fender.", "Vehicle");
KillTrak::DeathMessage("%1 says, 'Hey %2, you scratched my paint job!'", "Vehicle");

KillTrak::DeathMessage("%1 ventilates %2 with %3 chaingun.", "Chaingun");
KillTrak::DeathMessage("%1 gives %2 an overdose of lead.", "Chaingun");
KillTrak::DeathMessage("%1 fills %2 full of holes.", "Chaingun");
KillTrak::DeathMessage("%1 guns down %2.", "Chaingun");

KillTrak::DeathMessage("%2 dies of turret trauma.", "Turret");
KillTrak::DeathMessage("%2 is chewed to pieces by a turret.", "Turret");
KillTrak::DeathMessage("%2 walks into a stream of turret fire.", "Turret");
KillTrak::DeathMessage("%2 ends up on the wrong side of a turret.", "Turret");

KillTrak::DeathMessage("%2 feels the warm glow of %1's plasma.", "Plasma");
KillTrak::DeathMessage("%1 gives %2 a white-hot plasma injection.", "Plasma");
KillTrak::DeathMessage("%1 asks %2, 'Got plasma?'", "Plasma");
KillTrak::DeathMessage("%1 gives %2 a plasma transfusion.", "Plasma");

KillTrak::DeathMessage("%2 catches a Frisbee of Death thrown by %1.", "Disc Launcher");
KillTrak::DeathMessage("%1 blasts %2 with a well-placed disc.", "Disc Launcher");
KillTrak::DeathMessage("%1's spinfusor caught %2 by surprise.", "Disc Launcher");
KillTrak::DeathMessage("%2 falls victim to %1's Stormhammer.", "Disc Launcher");

KillTrak::DeathMessage("%1 blows %2 up real good.", "Explosives");
KillTrak::DeathMessage("%2 gets a taste of %1's explosive temper.", "Explosives");
KillTrak::DeathMessage("%1 gives %2 a fatal concussion.", "Explosives");
KillTrak::DeathMessage("%2 never saw it coming from %1.", "Explosives");

KillTrak::DeathMessage("%1 adds %2 to %3 list of sniper victims.", "Laser Rifle");
KillTrak::DeathMessage("%1 fells %2 with a sniper shot.", "Laser Rifle");
KillTrak::DeathMessage("%2 becomes a victim of %1's laser rifle.", "Laser Rifle");
KillTrak::DeathMessage("%2 stayed in %1's crosshairs for too long.", "Laser Rifle");

KillTrak::DeathMessage("%1 mortars %2 into oblivion.", "Mortar");
KillTrak::DeathMessage("%2 didn't see that last mortar from %1.", "Mortar");
KillTrak::DeathMessage("%1 inflicts a mortal mortar wound on %2.", "Mortar");
KillTrak::DeathMessage("%1's mortar takes out %2.", "Mortar");

KillTrak::DeathMessage("%2 gets a blast out of %1.", "Blaster");
KillTrak::DeathMessage("%2 succumbs to %1's rain of blaster fire.", "Blaster");
KillTrak::DeathMessage("%1's puny blaster shows %2 a new world of pain.", "Blaster");
KillTrak::DeathMessage("%2 meets %1's master blaster.", "Blaster");

KillTrak::DeathMessage("%2 gets zapped with %1's ELF gun.", "ELF Gun");
KillTrak::DeathMessage("%1 gives %2 a nasty jolt.", "ELF Gun");
KillTrak::DeathMessage("%2 gets a real shock out of meeting %1.", "ELF Gun");
KillTrak::DeathMessage("%1 short-circuits %2's systems.", "ELF Gun");

KillTrak::DeathMessage("%2 didn't stay away from the moving parts.", "Crushed");
KillTrak::DeathMessage("%2 is crushed.", "Crushed");
KillTrak::DeathMessage("%2 gets smushed flat.", "Crushed");
KillTrak::DeathMessage("%2 gets caught in the machinery.", "Crushed");

KillTrak::DeathMessage("%2 is a victim among the wreckage.", "Debris");
KillTrak::DeathMessage("%2 is killed by debris.", "Debris");
KillTrak::DeathMessage("%2 becomes a victim of collateral damage.", "Debris");
KillTrak::DeathMessage("%2 got too close to the exploding stuff.", "Debris");

KillTrak::DeathMessage("%2 takes a missile up the keister.", "Missile");
KillTrak::DeathMessage("%2 gets shot down.", "Missile");
KillTrak::DeathMessage("%2 gets real friendly with a rocket.", "Missile");
KillTrak::DeathMessage("%2 feels the burn from a warhead.", "Missile");

KillTrak::DeathMessage("%2 ends it all.", "Suicide");
KillTrak::DeathMessage("%2 takes %3 own life.", "Suicide");
KillTrak::DeathMessage("%2 kills %3 own dumb self.", "Suicide");
KillTrak::DeathMessage("%2 decides to see what the afterlife is like.", "Suicide");
KillTrak::DeathMessage("%2 dies.", "Suicide");
