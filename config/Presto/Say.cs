// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// Say.CS									Presto, March '99 
//
//	A more organized way of storing the chat messages and their associated
//	wavfiles.
//
//	I wrote this as support code for a few other functions I'm writing, but
//	it does have some standalone use.  Code that uses these functions should
//	be easier to understand.
//
//	Usage examples:
//		Say::Public(yellDammit);
//			This will send the default text "Dammit!" to everyone in the
//			game, and play the "dammit" wavfile.
//
//		Say::Public(waiting, "We're waiting for everyone to start...");
//			An example of over-riding the default text but still using 
//			the "Waiting..." wavfile.
//
//		Say::Team(yellDoh);
//			This message will go to your team with the default text
//			"Doh!"
//
//		Say::Team(sayHelp, "Help me out here guys!");
//			A message to your team, overriding the default text.
//
//		Say::Local(needStation);
//			No text in the message window - just a message that everyone
//			nearby will hear:  "Hurry up with that station!"
//
//		Say::Animation(yellCrap, 3);
//			A message that everyone nearby will hear:  "Crap!", and an
//			animation to go along with it.
//
//		Say::Response(orderCompleted, 0);
//			Your team will hear the voice message, but the important 
//			part is the second parameter.  0 means "not working on
//			my assigned task" and 1 means "working on it!"
//
//	You can also override the defaults in your own scripts if you load this
//	first.  Example:
//
//		exec("Say.cs");
//		Say::SetText(yellDammit, "Darnit!");
//		bindCommand(keyboard0, make, "d", TO, "Say::Local(yellDammit);");
//
// ---------------------------------------------------------------------------
// A) The ugly code
// ---------------------------------------------------------------------------

Include("presto\\Event.cs");

function Say::SetWav(%name, %wav) {
	$Say::[%name, wav] = %wav;
	}
function Say::SetText(%name, %defaultText) {
	$Say::[%name, text] = %defaultText;
	}
function Say::GetWav(%name) {
	return $Say::[%name, wav];
	}
function Say::GetText(%name) {
	return $Say::[%name, text];
	}

// Say::Public, Say::Team, Say::Local	3 ways of speaking
function Say::Public(%name, %msg) {
	if (%msg == "")
		%msg = Say::GetText(%name);
	remoteEval(2048, say, 0, %msg @ "~w" @ Say::GetWav(%name) );
	}
function Say::Team(%name, %msg) {
	if (%msg == "")
		%msg = Say::GetText(%name);
	remoteEval(2048, say, 1, %msg @ "~w" @ Say::GetWav(%name) );
	}
function Say::Local(%name) {
	remoteEval(2048, lmsg, Say::GetWav(%name) );
	}
function Say::Animation(%name, %anim) {
	remoteEval(2048, playAnimWav, %anim, Say::GetWav(%name) );
	}
function Say::Response(%name, %status, %msg) {
	if (%msg == "")
		%msg = Say::GetText(%name);
	remoteEval(2048, CStatus, %anim, %msg @ "~w" @ Say::GetWav(%name) );
	}

// Say::New		create a new saying.
function Say::New(%name, %sayWavfile, %defaultSayText) {
	Say::SetWav(%name, %sayWavFile);
	Say::SetText(%name, %defaultSayText);
	}

function Flood::Protect(%tag, %protectTime) {
	%lastTime = $Flood::[%tag];
	%time = GetSimTime();
	$Flood::[%tag] = %time;
	return %lastTime == "" || (%time - %lastTime >= %protectTime );
	}
function Flood::Reset() {
	deleteVariables("$Flood::*");
	}
Event::Attach(eventChangeMission, Flood::Reset);
Event::Attach(eventConnected, Flood::Reset);

// ---------------------------------------------------------------------------
// B) The sayings
// ---------------------------------------------------------------------------

Say::New( sayNone,			"",		"I have nothing to say.");

// Attack sayings
Say::New( attack,			"attack",	"Attack!");
Say::New( attack2,		"attac2",	"Attack!");
Say::New( attackBase,		"attbase",	"Attack the enemy base.");
Say::New( attackEnemy,		"attenem",	"Attack the enemy.");
Say::New( attackGo,		"gooff",	"Go on the offensive.");
Say::New( attackGoing,		"ono",	"Going offense...");

// Base info sayings
Say::New( baseClear,		"isbsclr",	"Is our base clear?");
Say::New( baseSecure,		"bsclr2",	"Our base is secure.");
Say::New( baseTaken,		"basetkn",	"Our base has been taken!");
Say::New( baseUnderAttack,	"basundr",	"The enemy is attacking our base!");

// Chat sayings
Say::New( sayThanks, 		"thanks",	"Thanks.");
Say::New( sayNoProblem,		"noprob",	"No problem.");
Say::New( sayHey,			"wshoot1",	"Hey.");
Say::New( sayBye,			"bye",	"Bye!");
Say::New( sayYes,			"yes",	"Yes.");
Say::New( sayNo,			"no",		"No.");
Say::New( saySorry,		"sorry",	"Sorry.");
Say::New( sayHi,			"hello",	"Hi.");
Say::New( sayIDontKnow,		"dontkno",	"I don't know");
Say::New( sayHmm,			"color3",	"Hmm...");

// Command sayings
Say::New( orderAcknowledged,	"acknow",	"Acknowledged");
Say::New( orderBelay,	 	"belay", 	"Belay Order!");
Say::New( orderBoard, 		"boarda", 	"Board APC.");
Say::New( orderCanceled,	"ordcan",	"Order cancelled.");
Say::New( orderCompleted,	"objcomp",	"Objective completed.");
Say::New( orderDestroyGenerator, "desgen","Destroy the enemy generator.");
Say::New( orderDestroyTurret,	"destur",	"Destroy the enemy turret.");
Say::New( orderNotCompleted, 	"objxcmp", "Unable to complete objective.");

// Coordination
Say::New( orderCeaseFire,	"cease",	"Cease fire!");
Say::New( orderCoverMe,		"coverme",	"Cover me!");
Say::New( orderHitTheDeck,	"hitdeck",	"Hit the deck!");
Say::New( orderMoveAside,	"outway",	"Move out of the way!");
Say::New( orderMoveOut,		"moveout",	"Move out!");
Say::New( orderOverHere,	"ovrhere",	"Over here!");
Say::New( orderProceed,		"proceed",	"Proceed ahead.");
Say::New( orderReady,		"ready",	"Ready.");
Say::New( orderRegroup,		"regroup",	"Regroup!");
Say::New( orderRetreat,		"retreat",	"Retreat!");
Say::New( orderStop,		"stop",	"Stop!");
Say::New( orderTakeCover,	"takcovr",	"Take cover!");
Say::New( orderWait,		"wait1",	"Wait...");
Say::New( orderWaiting,		"wait2",	"Waiting...");
Say::New( orderWaitSignal,	"waitsig",	"Wait for my signal to attack.");

// Defense sayings
Say::New( defendAttacked,	"basatt",	"We're being attacked!");
Say::New( defendBase,		"defbase",	"Defend our base!");
Say::New( defendGo,		"godef",	"Go on the defensive.");
Say::New( defendGoing,		"defend",	"Defending our base.");
Say::New( defendIncoming, 	"incom2",	"Incoming enemies!");
Say::New( defendNeed,		"needdef",	"We need more defense!");

// Deploy sayings
Say::New( deployAmmo,		"depamo",	"Deploy ammo station at waypoint.");
Say::New( deployBeacon,		"depbecn",	"Deploy beacon at waypoint.");
Say::New( deployCamera,		"depcam",	"Deploy camera at waypoint.");
Say::New( deployInventory,	"depinv",	"Deploy inventory station at waypoint.");
Say::New( deployJammer,		"depjamr",	"Deploy sensor jammer at waypoint.");
Say::New( deployMotion,		"depmot",	"Deploy motion sensor at waypoint.");
Say::New( deploySensor,		"deppuls",	"Deploy pulse sensor at waypoint.");
Say::New( deployTurret,		"deptur",	"Deploy turret at waypoint.");

// Flag sayings
Say::New( flagClearMines,	"clrflg",	"Clear the mines from our flag.");
Say::New( flagGet,		"geteflg",	"Get the enemy flag!");
Say::New( flagHave,		"haveflg",	"I have the enemy flag, heading back to our base.");
Say::New( flagMine,		"mineflg",	"Mine the flag.");
Say::New( flagMined,		"flgmine",	"Our flag is mined.");
Say::New( flagNotInBase,	"flgtkn1",	"Our flag is not in the base!");
Say::New( flagReturn,		"retflag",	"Return our flag to our base.");
Say::New( flagSecure,		"flaghm",	"Our flag is secure.");
Say::New( flagTaken,		"flgtkm2",	"The enemy has our flag!");

// Need
Say::New( needAmmo,		"needamo",	"Can anyone bring me some ammo?");
Say::New( needAPC,		"needpku",	"I need an APC pickup");
Say::New( needChaingunAmmo,	"needamo",	"I need Chaingun ammo.");
Say::New( needDiscLauncherAmmo, "needamo","I need Disc Launcher ammo.");
Say::New( needEscort,		"needesc",	"I need an escort back to base.");
Say::New( needGrenadeLauncherAmmo,"needamo","I need Grenade Launcher ammo.");
Say::New( needGrenades,		"needamo",	"I need Grenades.");
Say::New( needMines,		"needamo",	"I need Mines.");
Say::New( needMortarAmmo,	"needamo",	"I need Mortar ammo.");
Say::New( needPlasmaGunAmmo,	"needamo",	"I need Plasma Gun ammo.");
Say::New( needRepairs,		"needrep",	"Need repairs.");
Say::New( needStation,		"hurystn",	"Hurry up with that station!");

// Objective sayings
Say::New( objectiveAttack,	"attobj",	"Attack objective.");
Say::New( objectiveCapture,	"capobj", 	"Capture the objective.");
Say::New( objectiveClearMines,"clrobj",	"Clear the mines from our objective.");
Say::New( objectiveDefend,	"defobj",	"Defend the objective.");
Say::New( objectiveGet,		"getobj", 	"Get the objective.");
Say::New( objectiveMine,	"mineobj",	"Our objective is mined.");
Say::New( objectiveRepair,	"repobj",	"Repair the objective.");

// Status messages
Say::New( statusGeneratorDestroyed,"gendes","Enemy generator destroyed.");
Say::New( statusMinesCleared,	"mineclr",	"Mines have been cleared.");
Say::New( statusTurretDestroyed,"turdes",	"Enemy turret destroyed.");
Say::New( statusAPCReady,	"waitpas",	"APC Ready to go... waiting for passengers.");

// Targeting sayings
Say::New( targetFire,		"firetgt",	"Fire on my target.");
Say::New( targetAcquired,	"tgtacq",	"Target acquired.");
Say::New( targetLocation,	"tgtobj",	"Target location.");
Say::New( targetOutOfRange,	"tgtout",	"Target out of range.");
Say::New( targetNeeded,		"needtgt",	"I need a target.");

// Taunt sayings
Say::New( tauntYoohoo,		"taunt1",	"Yooo hooo!");
Say::New( tauntHowdThatFeel,	"taunt10",	"How'd that feel?");
Say::New( tauntIveHadWorse,	"tautn11",	"I've had worse...");
Say::New( tauntMissedMe,	"taunt2",	"Missed me!");
Say::New( tauntDance,		"taunt3",	"Dance!");
Say::New( tauntComeGetSome,	"taunt4",	"Come get some!");

// Waypoint sayings
Say::New( waypointAttack,	"attway",	"Attack enemy at waypoint.");
Say::New( waypointDefend, 	"defway",	"Defend the waypoint.");
Say::New( waypointEscort,	"escfr",	"Escort friendly at waypoint.");
Say::New( waypointGo,		"goway",	"Proceed to waypoint.");
Say::New( waypointPilot,	"wpilot",	"Pilot APC to waypoint");
Say::New( waypointRepairPlayer,"repplyr",	"Repair friendly at waypoint.");
Say::New( waypointRepairEquipment,"repeqp","Repair equipment at waypoint.");
Say::New( waypointRepairItem,	"repitem",	"Repair item at waypoint.");

// Yellings
Say::New( yellShazbot,		"color2",	"Shazbot!");
Say::New( yellDammit,		"color6",	"Dammit!");
Say::New( yellCrap,		"color7",	"Ahhh crap!");
Say::New( yellDuh,		"dsgst1",	"Duh!");
Say::New( yellYouIdiot,		"dsgst2",	"You idiot!");
Say::New( yellWatchShooting,	"wshoot3",	"Watch where you're shooting!");
Say::New( yellHey,		"wshoot1",	"Hey!");
Say::New( yellArgh,		"dsgst4",	"Argh!");
Say::New( yellSigh,		"dsgst5",	"(sigh)");
Say::New( yellDoh,		"oops1",	"Doh!");
Say::New( yellOops,		"oops2",	"Oops!");
Say::New( yellYeah,		"cheer1",	"Yeah!");
Say::New( yellWoohoo,		"cheer2",	"Woo hoo!");
Say::New( yellAllRight,		"cheer3",	"All right!");
Say::New( yellHelp, 		"help",	"Help!");
Say::New( yellDeath, 		"death",	"Death (male5 & female2-5 only)");


// ---------------------------------------------------------------------------
// C) The End
// ---------------------------------------------------------------------------