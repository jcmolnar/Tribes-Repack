// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// Mute.CS								Presto, April '99 
//
//	Client-side muting routines
//
//	If you've ever wanted to stop seeing annoying public messages, this
//	script might just help you do it.  This is a set of client-side
//	muting routines, which means that all the messages still get sent to
//	you by the server, but if you mute them you won't see them.
//
//	There are four pre-defined mute targets:
//
//		* all,""
//			This mute applies to everyone in the game.
//		* team,friendly
//			This mute applies to everyone on your team.
//		* team,enemy
//			This mute applies to anyone not on your team.
//		* client,#
//			This mute applies to a particular client,
//			specified by the number.
//
//	There are four pre-defined kinds of muting:
//		* all
//			This will mute EVERY message from the specified source.
//		* noise
//			This will mute any message with a sound file
//		* repeat
//			This will mute messages which repeat within a certain
//			amount of time.  So if they say something ten times in
//			a row very quickly you'll only see/hear it once.
//		* packs
//			This will mute messages which have been determined to
//			originate from a voicepack (ya know, those annoying
//			quotations people are always spouting)
//			Please read to the bottom of this document to learn how
//			to enable pack muting.
//
//	Read the bottom of this document for information about how you can
//	create your own kind of mute.
//
//	Examples:
//
//		Mute::Add(team,friendly, "repeat");
//			Mutes any repeated messages from your teammates.
//
//		Mute::Add(team,enemy, "noise");
//			Mutes any noisy messages from the enemy.
//
//		Mute::Add(all,"", "packs");
//			Mutes all pack messages from everyone in the game.
//
//		Mute::Add(client,2172, "all");
//			Completely mutes client #2172.
//
//		Mute::Remove(team,enemy, "noise");
//			Removes the muting we just put on the enemy team.
//
//		Mute::Clear(all,"");
//			Removes any mutings which were affecting all players.
//
//	You can also check for mutings yourself:
//
//		if (Mute::IsMuted(team,enemy, "noise")) {
//			echo("enemy team is noise-muted");
//			}
//
//		if (Mute::ClientIsMuted(2172, "packs")) {
//			echo("client 2172 is pack-muted");
//			}
//
//	This last example, the ClientIsMuted function, will check several
//	possible mutes:  the client itself, then his team, and then all
//	players.  If any of these are muted it will return true.
//
//	PACK MUTING
//
//	So, how does it know what messages have originated from packs?
//	Well, you'll need to include a pack-muting script which contains
//	a list of messages for any packs you want to mute.  Here is a
//	sample pack mute file for a fictional Terminator/DieHard pack:
//
//		Mute::AddPackMessage("I'll be back.");
//		Mute::AddPackMessage("Yippee ki-yay mother %x");
//		Mute::AddPackWav("arn_term");
//		Mute::AddPackWav("bruce%x");
//
//	After including this script, anyone who is pack muted will
//	be muted if they say "I'll be back."  Then, note the use of
//	parameter notation (%x ... %a, %3 or whatever is valid too)
//	to specify that anything past that point is matched.
//
//	Also, a message which tries to play the wav file arm_term.wav
//	will be muted, as will any message that plays bruce*.wav
//
//	It can be expensive to make a large muting pack, because
//	this means that every message will be checked against *all*
//	of the possible pack messages!
//
//	Since most packs use a consistent naming scheme, it may be 
//	possible to mute an entire pack with only a few checks,
//	for instance the way bruce*.wav was checked above would catch
//	every message in a pack that had bruce1.wav, bruce2.wav, etc.
//
//	NEW MUTE PACKS
//
//	You can also add personal kinds of mutings, which can be added
//	in the same way as "all" or "noise", etc.
//
//		Mute::AddPackMessage(mypack, "this is a test");
//		Mute::AddPackWav(mypack, "testing");
//
//	This creates a new kind of mute called "mypack".  You can now
//	use scripts to do, for instance,
//
//		Mute::Add(all,"", "mypack");
//
//	Good luck, and enjoy the silence.
//
// ---------------------------------------------------------------


Include("presto\\Match.cs");
Include("presto\\Event.cs");

function Mute::RemoveKind(%str, %kind) {
	if (Match::ParamString(%str, "^^^a["@%kind@"]^^^b","^^^"))
		return Match::Result(a) @ Match::Result(b);
	return %str;
	}
function Mute::KindIsMuted(%str, %kind) {
	if (Match::ParamString(%str, "^^^a["@%kind@"]^^^b","^^^"))
		return true;
	return false;
	}
function Mute::AddKind(%str, %kind) {
	if (Mute::KindIsMuted(%str, %kind))
		return %str;
	return %str@"["@%kind@"]";
	}

function Mute::IsEmpty(%class, %member) {
	return $Mute::[%class,%member]=="";
	}
function Mute::Add(%class, %member, %kind) {
	$Mute::[%class,%member] = Mute::AddKind($Mute::[%class,%member], %kind);
	}
function Mute::Remove(%class, %member, %kind) {
	$Mute::[%class,%member] = Mute::RemoveKind($Mute::[%class,%member], %kind);
	}
function Mute::Clear(%class, %member) {
	$Mute::[%class,%member] = "";
	if (%class == client) {
		deleteVariables("$Mute::messageTime"@%client@"*");
		deleteVariables("$Mute::messageAtTime"@%client@"*");
		}
	}
function Mute::IsMuted(%class, %member, %kind) {
	return Mute::KindIsMuted($Mute::[%class,%member], %kind);
	}

function Mute::Team(%team) {
	if (%team == Client::getTeam(getManagerId()))
		return friendly;
	return enemy;
	}
function Mute::ClientIsMuted(%client, %kind) {
 	if (Mute::IsMuted(all, "", %kind)) {
		$Mute::lastClientReason = "all";
		return true;
		}

	%team = Client::GetTeam(%client);
	if (Mute::IsMuted(team, Mute::Team(%team), %kind)) {
		$Mute::lastClientReason = "team";
		return true;
		}

	if (Mute::IsMuted(client, %client, %kind)) {
		$Mute::lastClientReason = "client";
		return true;
		}

	$Mute::lastClient = "";
	return false;
	}

function Mute::AddPack(%pack) {
	%num = $Mute::packNum[%pack];
	if (%num != "" && $Mute::pack[%num] == %pack)
		return;

	%num = ($Mute::packs + 0);
	$Mute::packs += 1;

	$Mute::pack[%num] = %pack;
	$Mute::packNum[%pack] = %num;
	}
function Mute::AddPackMessage(%pack, %msg) {
	if (%msg == "") {
		%msg = %pack;
		%pack = "packs";
		}
	Mute::AddPack(%pack);
	%num = $Mute::packMessageNum[%pack, %msg];
	if (%num != "" && $Mute::packMessage[%pack, %num] == %msg)
		return;

	%num = ($Mute::packMessages[%pack] + 0);
	$Mute::packMessages[%pack] += 1;

	$Mute::packMessage[%pack, %num] = %msg;
	$Mute::packMessageNum[%pack, %msg] = %num;
	}
function Mute::AddPackWav(%pack, %msg) {
	if (%msg == "") {
		%msg = %pack;
		%pack = "packs";
		}
	Mute::AddPack(%pack);
	%num = $Mute::packWavNum[%pack, %msg];
	if (%num != "" && $Mute::packWav[%pack, %num] == %msg)
		return;

	%num = ($Mute::packWavs[%pack] + 0);
	$Mute::packWavs[%pack] += 1;

	$Mute::packWav[%pack, %num] = %msg;
	$Mute::packWavNum[%pack, %msg] = %num;
	}
function Mute::IsPackMessage(%pack, %msg) {
	for (%i = 0; %i < $Mute::packMessages[%pack]; %i++) {
		if (Match::ParamString(%msg, $Mute::packMessage[%pack, %i]))
			return true;
		}
	return false;
	}
function Mute::IsPackWav(%pack, %msg) {
	for (%i = 0; %i < $Mute::packWavs[%pack]; %i++) {
		if (Match::ParamString(%msg, $Mute::packWav[%pack, %i]))
			return true;
		}
	return false;
	}

function Mute::ClearRepeat(%client,%time) {
	%msg = $Mute::messageAtTime[%client,%time];
	if (%msg == "")
		return;

	%time2 = $Mute::messageTime[%client, %msg];
	if (%time2 != %time)
		return;

	$Mute::messageTime[%client, %msg] = "";
	}
function Mute::IsRepeat(%client, %msg) {
	%time = "X"@getSimTime();
	
	%lastTime = $Mute::messageTime[%client,%msg];

	$Mute::messageTime[%client,%msg] = %time;
	$Mute::messageAtTime[%client, %time] = %msg;
	schedule("Mute::ClearRepeat("@%client@",\""@%time@"\");", $PrestoPref::MuteRepeatTime);
	if (%lastTime == "")
		return false;

	%time = String::GetSubStr(%time,1,10000);
	%lastTime = String::GetSubStr(%lastTime,1,10000);
	if (%time > %lastTime + $PrestoPref::MuteRepeatTime)
		return false;

	// Mute it baby!
	return true;
	}

function Mute::String(%msg, %pattern, %len) {
	if (%len == "*")
		return String::Match(%msg, %pattern);
	}

function Mute::Muted(%client,%reason,%msg) {
	echo("Muted(",%reason,") => ",Client::getName(%client),": ",%msg);
	$Mute::Muted++;
	if ($PrestoPref::MuteDisplayTime > 0) {
		schedule("Mute::UnReportMuted();", $PrestoPref::MuteDisplayTime);
		if ($PrestoPref::MuteDisplayMessage) {
			if (String::GetSubStr(%msg, 10,1) != "")
				%msg = "\""@String::GetSubStr(%msg,0,10) @ "...\"";
			else	%msg = "\""@%msg@"\"";
			}
		else	%msg = "";
		$Mute::lastMute = "Muted <f1>"@Client::getName(%client)@"<f0> ("@$Mute::lastClientReason@","@%reason@") "@%msg;
		HUD::Display(hudMute, true);
		}
	}
function Mute::UnReportMuted() {
	if ($Mute::Muted >= 0)
		$Mute::Muted--;
	else	$Mute::Muted = 0;
	}

function Mute::OnExtendedClientMessage(%client, %msg, %text) {
	if ((%client == 0) || (%client == getManagerId()))
		return;

	if (Match::ParamString(%msg, "%m~w%w") == 0)
		return;
	%wav = Match::Result(w);

	if (Mute::ClientIsMuted(%client, "noise")) {
		Mute::Muted(%client, "noise",%text);
		$Mute::muteNoise = getSimTime();
		return mute;
		}

	for (%i=0; %i < $Mute::packs; %i++) {
		if (Mute::ClientIsMuted(%client, $Mute::pack[%i]) &&
		    Mute::IsPackWav($Mute::pack[%i], %wav)) {
			Mute::Muted(%client, $Mute::pack[%i],%text);
			return mute;
			}
		}

	return;
	}

function Mute::OnClientMessage(%client, %msg) {
	if ((%client == 0) || (%client == getManagerId()))
		return;

	if ($Mute::muteNoise  == getSimTime())
		return;
 
	if (Mute::ClientIsMuted(%client, "all")) {
		Mute::Muted(%client, "all",%msg);
		return mute;
		}

	if (Mute::ClientIsMuted(%client, "repeat") && Mute::IsRepeat(%client, %msg)) {
		Mute::Muted(%client, "repeat",%msg);
		return mute;
		}

	for (%i=0; %i < $Mute::packs; %i++) {
		if (Mute::ClientIsMuted(%client, $Mute::pack[%i]) && 
		    Mute::IsPackMessage($Mute::pack[%i], %msg)) {
			Mute::Muted(%client, $Mute::pack[%i],%msg);
			return mute;
			}
		}

	return;
	}


function Mute::Joined(%client) {
	Mute::Clear(client, %client);
	}
function Mute::Left(%client) {
	Mute::Clear(client, %client);
	}

function Mute::Preference(%class, %member, %pref) {
	%i = 0;
	%w = getWord(%pref, 0);
	while (%w != -1) {
		Mute::Add(%class, %member, %w);

		%i++;
		%w = getWord(%pref, %i);
		}
	}
function Mute::Reset() {
	deleteVariables("$Mute::client*");
	deleteVariables("$Mute::team*");
	deleteVariables("$Mute::messageTime*");
	deleteVariables("$Mute::messageAtTime*");
	deleteVariables("$Mute::pack*");

	$Mute::Muted = 0;
	Mute::Preference(team, enemy, $PrestoPref::Mute[enemy]);
	Mute::Preference(team, friendly, $PrestoPref::Mute[friendly]);
	Mute::Preference(all, "", $PrestoPref::Mute[all]);
	}

function Mute::UpdateHUD() {
	if ($Mute::Muted == 0) {
		HUD::Display(hudMute, false);
		return 0;
		}

	if ($Mute::lastMute != "") {
		HUD::AddTextLine(hudMute, "<jc>"@$Mute::lastMute);
		$Mute::lastMute = "";
		}
	else if ($Mute::Muted == 1)
		HUD::AddTextLine(hudMute, "<jc><f1>1<f0> muted message.");
	else	HUD::AddTextLine(hudMute, "<jc><f1>"@$Mute::Muted@"<f0> muted messages.");
	return 2;
	}

function Mute::Init() {
	Event::Attach(eventExtendedClientMessage, Mute::OnExtendedClientMessage);
	Event::Attach(eventClientMessage, Mute::OnClientMessage);
	Event::Attach(eventTeammateJoined, Mute::Joined);
	Event::Attach(eventTeammateLeft, Mute::Left);
	Event::Attach(eventConnected, Mute::Reset);

	Mute::Reset();
	HUD::NewFrame(hudMute, "Mute::UpdateHUD", "50%",0,300,10);
	HUD::NewText(hudMute, false, 0,-4);
	HUD::Display(hudMute, true);
	}

if (Presto::Enabled(Mute))
	Mute::Init();
// ---------------------------------------------------------------
