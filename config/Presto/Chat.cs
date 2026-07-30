// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// Chat.CS									Presto, March '99 
//
//	Reimplemented V chat menus.
//
//	This will let you customize new chat menus using a system that's a 
//	little easier than Dynamix's was.
//
//	>>>>> Are you customizing the Chat menus?  Read this whole note!
//	Especially the part marked about a page down.
//
//	I created some new Menu:: functions, see below.  These are
//	shortcuts for adding voices & animations to a menu.
//
//	Quick & dirty:
//
//		All you need to do is bind a key to "Menu::Display(menuChat);".
//		You're probably going to want to bind it to V for familiarity.
//		This is taken care of by my script, see PrestoPrefs.cs
//
//	Usage examples:
//
//		Menu::AddLocalChat(menuPresto, "h", sayHello);
//			This adds an H key to menuPresto, which when pressed will
//			cause you to say hello locally (no text message, but
//			people nearby will hear it).  The menu choice text
//			is the default text for sayHello, "Hello."
//		Menu::AddLocalChat(menuPresto, "h", sayHello, "hellooooooooo");
//			Same as above, but I have overridden the default text
//			so my menu will look like "h: helloooooooo", but still
//			play the "Hello." wavfile.
//
//		Menu::AddPublicChat(menuPresto, "g", sayGoodbye);
//		Menu::AddPublicChat(menuPresto, "c", yellCrap, "crap!!#@$!@");
//			Like Menu::AddLocalChat, but these will make it a public
//			message to everyone.
//
//		Menu::AddTeamChat(menuPresto, "z", yellDoh);
//		Menu::AddTeamChat(menuPresto, "t", sayThanks);
//			These ones are team-only messages, with and without the
//			default text ....
//
//		Menu::AddAnimation(menuPresto, "w", 12, sayHi);
//		Menu::AddAnimation(menuPresto, "w", 12, sayHi, "say Hi...");
//			This will perform an animation while doing a local
//			voice.  Again, you can explictly set the text or just
//			leave it as the default.  The 12 is an animation
//			sequence number, in this case the waving sequence.
//
//		Menu::AddResponse(menuPresto, "x", 0, orderNotCompleted);
//		Menu::AddResponse(menuPresto, "x", 0, orderNotCompleted, "I'm outta here");
//			This performs a command response - used to tell the
//			commander 'yes' or 'no' when asked to perform a 
//			task.
//	
//	>>>>> READ THIS!
//
//	Note that it's perfectly okay to add to the existing chat menus
//	later in your own scripts - but I don't do any checking for duplicate
//	letters so if you add two entries with the same letter, it will
//	only put one of them in the menu.  This is most likely to happen
//	if the uesr loads two scripts that each try to add new chat menu
//	choices.
//
//	Let me explain this again because some people didn't seem to
//	understand.  You can add to these chat menus, but put the addition
//	in your autoexec.cs or your own new file!  In fact I'd prefer it
//	because that way you're not changing CHAT.CS which, when I release
//	a new version, might change.
//
//	For instance the line
//		Menu::AddPublicChat(menuPresto, "g", sayGoodbye);
//	will add a menu choice to a menu, even if the menu was defined
//	in another file!
//
//	Soon you will be able to add/delete/replace menu items so you'll be
//	able to completely configure the menus from outside this file.
//
//	See section B below for the list of menus if you want to add to them.
//
// ---------------------------------------------------------------------------
// A) The ugly code
// ---------------------------------------------------------------------------
Include("presto\\Menu.cs");
Include("presto\\Say.cs");

function Menu::AddLocalChat(%menu, %letter, %say, %text) {
	if (%text != "")
		Menu::AddLetter(%menu, %letter, %text, "Say::Local("@%say@");");
	else	Menu::AddLetter(%menu, %letter, Say::GetText(%say), "Say::Local("@%say@");");
}
function Menu::AddTeamChat(%menu, %letter, %say, %text) {
	if (%text != "")
		Menu::AddLetter(%menu, %letter, %text, "Say::Team("@%say@",\""@%text@"\");");
	else	Menu::AddLetter(%menu, %letter, Say::GetText(%say), "Say::Team("@%say@");");
}
function Menu::AddPublicChat(%menu, %letter, %say, %text) {
	if (%text != "")
		Menu::AddLetter(%menu, %letter, %text, "Say::Public("@%say@",\""@%text@"\");");
	else	Menu::AddLetter(%menu, %letter, Say::GetText(%say), "Say::Public("@%say@");");
}
function Menu::AddAnimation(%menu, %letter, %anim, %say, %text) {
	if (%text != "")
		Menu::AddLetter(%menu, %letter, %text, "Say::Animation("@%say@", "@%anim@");");
	else	Menu::AddLetter(%menu, %letter, Say::GetText(%say), "Say::Animation("@%say@", "@%anim@");");
}
function Menu::AddResponse(%menu, %letter, %action, %say, %text) {
	if (%text != "")
		Menu::AddLetter(%menu, %letter, %text, "Say::Response("@%say@", "@%action@");");
	else	Menu::AddLetter(%menu, %letter, Say::GetText(%say), "Say::Response("@%say@", "@%action@");");
}
function Menu::AddTaunt(%menu, %letter1, %letter2, %say, %text) {
	Menu::AddLocalChat(%menu, %letter1, %say, %text);
	if (%text != "")
		Menu::AddLetter(%menu, %letter2, "", "Say::Public("@%say@",\""@%text@"\");");
	else	Menu::AddLetter(%menu, %letter2, "", "Say::Public("@%say@");");
}

// ---------------------------------------------------------------------------
// B) The definitions
// ---------------------------------------------------------------------------

Menu::New(menuChatOffense, "Offense");
 Menu::AddTeamChat(menuChatOffense,"a", attack);
 Menu::AddTeamChat(menuChatOffense,"w", orderWaitSignal);
 Menu::AddTeamChat(menuChatOffense,"c", orderCeaseFire);
 Menu::AddTeamChat(menuChatOffense,"m", orderMoveOut);
 Menu::AddTeamChat(menuChatOffense,"r", orderRetreat);
 Menu::AddTeamChat(menuChatOffense,"h", orderHitTheDeck);
 Menu::AddTeamChat(menuChatOffense,"e", orderRegroup);
 Menu::AddTeamChat(menuChatOffense,"v", orderCoverMe);
 Menu::AddTeamChat(menuChatOffense,"g", attackGoing);
 Menu::AddTeamChat(menuChatOffense,"z", statusAPCReady);
 Menu::AddTeamChat(menuChatOffense,"o", attackGo);
// Menu::AddTeamChat(menuChatOffense," ", attack2, "Attack! (#2)");
 Menu::AddTeamChat(menuChatOffense,"j", objectiveCapture);
 Menu::AddTeamChat(menuChatOffense,"t", objectiveGet);
 Menu::AddTeamChat(menuChatOffense,"b", attackBase);
 Menu::AddTeamChat(menuChatOffense,"n", attackEnemy);

Menu::New(menuChatTarget, "Target");
 Menu::AddTeamChat(menuChatTarget,"z", targetAcquired);
 Menu::AddTeamChat(menuChatTarget,"f", targetFire);
 Menu::AddTeamChat(menuChatTarget,"n", targetNeeded);
 Menu::AddTeamChat(menuChatTarget,"o", targetOutOfRange);
 Menu::AddTeamChat(menuChatTarget,"d", orderDestroyGenerator);
 Menu::AddTeamChat(menuChatTarget,"e", statusGeneratorDestroyed);
 Menu::AddTeamChat(menuChatTarget,"t", orderDestroyTurret);
 Menu::AddTeamChat(menuChatTarget,"s", statusTurretDestroyed);
 Menu::AddTeamChat(menuChatTarget,"r", orderDestroyRadar);
 Menu::AddTeamChat(menuChatTarget,"q", statusRadarDestroyed);
 Menu::AddTeamChat(menuChatTarget,"l", targetLocation);

Menu::New(menuChatDefense, "Defense");
 Menu::AddTeamChat(menuChatDefense,"i", defendIncoming);
 Menu::AddTeamChat(menuChatDefense,"a", defendAttacked);
 Menu::AddTeamChat(menuChatDefense,"e", baseUnderAttack);
 Menu::AddTeamChat(menuChatDefense,"n", defendNeed);
 Menu::AddTeamChat(menuChatDefense,"t", baseTaken);
 Menu::AddTeamChat(menuChatDefense,"c", baseSecure, "Base clear");
 Menu::AddTeamChat(menuChatDefense,"q", baseClear);
 Menu::AddTeamChat(menuChatDefense,"g", defendGo);
 Menu::AddTeamChat(menuChatDefense,"d", defendGoing);
 Menu::AddTeamChat(menuChatDefense,"o", objectiveDefend);

Menu::New(menuChatFlag, "Flag");
 Menu::AddLetter(menuChatFlag,"b", "Flag gone.", "Say::Team(flagNotInBase);");
 Menu::AddTeamChat(menuChatFlag,"e", flagTaken);
 Menu::AddTeamChat(menuChatFlag,"h", flagHave);
 Menu::AddTeamChat(menuChatFlag,"s", flagSecure);
 Menu::AddTeamChat(menuChatFlag,"r", flagReturn);
 Menu::AddTeamChat(menuChatFlag,"f", flagGet);
 Menu::AddTeamChat(menuChatFlag,"m", flagMined);
 Menu::AddTeamChat(menuChatFlag,"c", flagClearMines);
 Menu::AddTeamChat(menuChatFlag,"d", statusMinesCleared);
 Menu::AddTeamChat(menuChatFlag,"n", flagMine);
 Menu::AddTeamChat(menuChatFlag,"o", objectiveMine);
 Menu::AddTeamChat(menuChatFlag,"l", objectiveClearMines);

Menu::New(menuChatNeed, "Need");
 Menu::AddTeamChat(menuChatNeed,"r", needRepairs);
 Menu::AddTeamChat(menuChatNeed,"a", needAPC);
 Menu::AddTeamChat(menuChatNeed,"e", needEscort);
 Menu::AddTeamChat(menuChatNeed,"t", needAmmo);
 Menu::AddLocalChat(menuChatNeed,"h", needStation);
 Menu::AddTeamChat(menuChatNeed,"c", needChaingunAmmo);
 Menu::AddTeamChat(menuChatNeed,"d", needDiscLauncherAmmo);
 Menu::AddTeamChat(menuChatNeed,"g", needGrenadeLauncherAmmo);
 Menu::AddTeamChat(menuChatNeed,"y", needGrenades);
 Menu::AddTeamChat(menuChatNeed,"x", needMines);
 Menu::AddTeamChat(menuChatNeed,"m", needMortarAmmo);
 Menu::AddTeamChat(menuChatNeed,"p", needPlasmaGunAmmo);
 Menu::AddTeamChat(menuChatNeed,"r", needRepairs);

Menu::New(menuChatTeam, "Team");
 Menu::AddTeamChat(menuChatTeam,"w", yellWatchShooting);
 Menu::AddTeamChat(menuChatTeam,"d", sayIDontKnow);
 Menu::AddTeamChat(menuChatTeam,"n", sayNo);
 Menu::AddTeamChat(menuChatTeam,"y", sayYes);
 Menu::AddTeamChat(menuChatTeam,"t", sayThanks);
 Menu::AddTeamChat(menuChatTeam,"a", sayNoProblem);
 Menu::AddTeamChat(menuChatTeam,"s", saySorry);
 Menu::AddLocalChat(menuChatTeam,"h", needStation);
 Menu::AddTeamChat(menuChatTeam,"z", yellDoh);
 Menu::AddTeamChat(menuChatTeam,"o", yellOops);
// Menu::AddTeamChat(menuChatTeam,"s", yellShazbot);		// S conflict!
 Menu::AddTeamChat(menuChatTeam,"q", yellDammit);
 Menu::AddTeamChat(menuChatTeam,"c", yellCrap);
 Menu::AddTeamChat(menuChatTeam,"e", yellDuh);
 Menu::AddTeamChat(menuChatTeam,"x", yellYouIdiot);
 Menu::AddTeamChat(menuChatTeam,"r", orderReady);
 Menu::AddTeamChat(menuChatTeam,"b", orderBelay);
 Menu::AddTeamChat(menuChatTeam," ", yellHelp);
 Menu::AddLocalChat(menuChatTeam,"f", orderBoardAPC);
 Menu::AddLocalChat(menuChatTeam,"p", orderProceed);

Menu::New(menuChatGlobal, "Global");
 Menu::AddPublicChat(menuChatGlobal,"z", yellDoh);
 Menu::AddPublicChat(menuChatGlobal,"o", yellOops);
 Menu::AddPublicChat(menuChatGlobal,"s", yellShazbot);
 Menu::AddPublicChat(menuChatGlobal,"q", yellDammit);
 Menu::AddPublicChat(menuChatGlobal,"c", yellCrap);
 Menu::AddPublicChat(menuChatGlobal,"e", yellDuh);
 Menu::AddPublicChat(menuChatGlobal,"x", yellYouIdiot);
 Menu::AddPublicChat(menuChatGlobal,"n", sayNo);
 Menu::AddPublicChat(menuChatGlobal,"y", sayYes);
 Menu::AddPublicChat(menuChatGlobal,"d", sayIDontKnow);
 Menu::AddPublicChat(menuChatGlobal,"t", sayThanks);
 Menu::AddPublicChat(menuChatGlobal,"a", sayNoProblem);
 Menu::AddPublicChat(menuChatGlobal,"h", sayHi);
 Menu::AddPublicChat(menuChatGlobal,"b", sayBye);

Menu::New(menuChatLocal, "Local");
 Menu::AddLocalChat(menuChatLocal,"q", yellDammit);
 Menu::AddLocalChat(menuChatLocal,"w", yellWatchShooting);
 Menu::AddLocalChat(menuChatLocal,"e", yellDuh);
 Menu::AddLocalChat(menuChatLocal,"t", sayThanks);
 Menu::AddLocalChat(menuChatLocal,"y", sayYes);
 Menu::AddLocalChat(menuChatLocal,"o", yellOops);
 Menu::AddLocalChat(menuChatLocal,"p", orderProceed);

 Menu::AddLocalChat(menuChatLocal,"a", sayNoProblem);
 Menu::AddLocalChat(menuChatLocal,"s", saySorry);
 Menu::AddLocalChat(menuChatLocal,"d", tauntDance);
 Menu::AddLocalChat(menuChatLocal,"f", yellShazbot);
// Menu::AddLocalChat(menuChatLocal,"f", orderBoardAPC); // doesn't work
// Menu::AddLocalChat(menuChatLocal,"h", needStation);
 Menu::AddLocalChat(menuChatLocal,"h", yellHey);
 Menu::AddLocalChat(menuChatLocal,"j", yellDeath);
 Menu::AddLocalChat(menuChatLocal,"k", sayIDontKnow);

 Menu::AddLocalChat(menuChatLocal,"z", yellDoh);
 Menu::AddLocalChat(menuChatLocal,"x", yellYouIdiot);
 Menu::AddLocalChat(menuChatLocal,"c", yellCrap);
 Menu::AddLocalChat(menuChatLocal,"v", orderOverHere);
 Menu::AddLocalChat(menuChatLocal,"n", sayNo);
 Menu::AddLocalChat(menuChatLocal,"m", tauntMissedMe);
 Menu::AddLocalChat(menuChatLocal," ", yellHelp);



Menu::New(menuChatLDefense, "Local Defense");
 Menu::AddLocalChat(menuChatLDefense,"q", baseClear);
 Menu::AddLocalChat(menuChatLDefense,"w", defendAttacked);
 Menu::AddLocalChat(menuChatLDefense,"e", needEscort);
 Menu::AddLocalChat(menuChatLDefense,"r", needRepairs);
 Menu::AddLocalChat(menuChatLDefense,"t", baseTaken);
 Menu::AddLocalChat(menuChatLDefense,"y", orderReady);
 Menu::AddLocalChat(menuChatLDefense,"i", defendIncoming);
 Menu::AddLocalChat(menuChatLDefense,"o", objectiveDefend);

 Menu::AddLocalChat(menuChatLDefense,"a", needAPC);
 Menu::AddLocalChat(menuChatLDefense,"d", defendGoing);
 Menu::AddLocalChat(menuChatLDefense,"g", defendGo);

 Menu::AddLocalChat(menuChatLDefense,"c", baseSecure, "Base clear");
 Menu::AddLocalChat(menuChatLDefense,"b", baseUnderAttack);
 Menu::AddLocalChat(menuChatLDefense,"v", orderBelay);
 Menu::AddLocalChat(menuChatLDefense,"n", defendNeed);
 Menu::AddLocalChat(menuChatLDefense,"m", needAmmo);


Menu::New(menuChatLOffense, "Local Offense");
 Menu::AddLocalChat(menuChatLOffense,"a", attack);
 Menu::AddLocalChat(menuChatLOffense,"w", orderWaitSignal);
 Menu::AddLocalChat(menuChatLOffense,"c", orderCeaseFire);
 Menu::AddLocalChat(menuChatLOffense,"m", orderMoveOut);
 Menu::AddLocalChat(menuChatLOffense,"r", orderRetreat);
 Menu::AddLocalChat(menuChatLOffense,"h", orderHitTheDeck);
 Menu::AddLocalChat(menuChatLOffense,"e", orderRegroup);
 Menu::AddLocalChat(menuChatLOffense,"v", orderCoverMe);
 Menu::AddLocalChat(menuChatLOffense,"g", attackGoing);
 Menu::AddLocalChat(menuChatLOffense,"z", statusAPCReady);
 Menu::AddLocalChat(menuChatLOffense,"o", attackGo);
 Menu::AddLocalChat(menuChatLOffense,"j", objectiveCapture);
 Menu::AddLocalChat(menuChatLOffense,"t", objectiveGet);
 Menu::AddLocalChat(menuChatLOffense,"b", attackBase);
 Menu::AddLocalChat(menuChatLOffense,"n", attackEnemy);


Menu::New(menuAnimation, "Animations");
 Menu::AddAnimation(menuAnimation, "o", 0, orderOverHere);
 Menu::AddAnimation(menuAnimation, "d", 1, orderMoveAside);
 Menu::AddAnimation(menuAnimation, "r", 2, orderRetreat);
 Menu::AddAnimation(menuAnimation, "s", 3, orderStop);
 Menu::AddAnimation(menuAnimation, "f", 4, sayYes);
 Menu::AddLetter(menuAnimation, "z", "Kneel Pose", "remoteEval(2048, playAnim, 10);"); // no SAY
 //Menu::AddLetter(menuAnimation, "x", "Stand pose", "remoteEval(2048, playAnim, 11);"); // no SAY
 Menu::AddLetter(menuAnimation, "x", "Celebration", "remoteEval(2048, playAnim, 43);"); // no SAY
 Menu::AddAnimation(menuAnimation, "q", 5, yellYeah);
 Menu::AddAnimation(menuAnimation, "e", 6, yellWoohoo);
 Menu::AddAnimation(menuAnimation, "w", 7, yellAllRight);
 Menu::AddAnimation(menuAnimation, "v", 8, tauntHowdThatFeel);
 Menu::AddAnimation(menuAnimation, "g", 9, tauntComeGetSome);
 Menu::AddAnimation(menuAnimation, "h", 12, sayHi, "Wave hi");
 Menu::AddAnimation(menuAnimation, "b", 12, sayBye, "Wave bye");
 Menu::AddAnimation(menuAnimation, "m", 12, tauntMissedMe);


Menu::New(menuResponse, "Response");
 Menu::AddLocalChat(menuResponse, "q", orderNotCompleted, "(local) Unable to complete..");
 Menu::AddLocalChat(menuResponse, "a", orderAcknowledged, "(local) Acknowledged");
 Menu::AddLocalChat(menuResponse, "z", orderCompleted, "(local) Objective completed.");

 Menu::AddResponse(menuResponse, "w", 0, orderNotCompleted);
 Menu::AddResponse(menuResponse, "s", 1, orderAcknowledged);
 Menu::AddResponse(menuResponse, "x", 0, orderCompleted);

 Menu::AddLetter(menuResponse, "e", "(public) Unable to complete..", "Say::Public(orderNotCompleted);");
 Menu::AddLetter(menuResponse, "d", "(public) Acknowledged", "Say::Public(orderAcknowledged);");
 Menu::AddLetter(menuResponse, "c", "(public) Objective completed.", "Say::Public(orderCompleted);");


Menu::New(menuTaunts, "Taunts");
 Menu::AddLocalChat(menuTaunts, "y", tauntYoohoo); 
 Menu::AddLocalChat(menuTaunts, "h", tauntHowdThatFeel); 
 Menu::AddLocalChat(menuTaunts, "i", tauntIveHadWorse);
 Menu::AddLocalChat(menuTaunts, "m", tauntMissedMe);
 Menu::AddLocalChat(menuTaunts, "d", tauntDance);
 Menu::AddLocalChat(menuTaunts, "c", tauntComeGetSome); 

Menu::New(menuPublicTaunts, "Taunts");
 Menu::AddPublicChat(menuPublicTaunts, "y", tauntYoohoo); 
 Menu::AddPublicChat(menuPublicTaunts, "h", tauntHowdThatFeel); 
 Menu::AddPublicChat(menuPublicTaunts, "i", tauntIveHadWorse);
 Menu::AddPublicChat(menuPublicTaunts, "m", tauntMissedMe);
 Menu::AddPublicChat(menuPublicTaunts, "d", tauntDance);
 Menu::AddPublicChat(menuPublicTaunts, "c", tauntComeGetSome); 

Menu::New(menuChat, "Chat");
 Menu::AddMenu(menuChat,"v", menuChatOffense);
 Menu::AddMenu(menuChat,"t", menuChatTarget);
 Menu::AddMenu(menuChat,"d", menuChatDefense);
 Menu::AddMenu(menuChat,"f", menuChatFlag);
 Menu::AddMenu(menuChat,"r", menuChatNeed);
 Menu::AddMenu(menuChat,"e", menuChatTeam);
 Menu::AddMenu(menuChat,"g", menuChatGlobal);
 Menu::AddMenu(menuChat,"l", menuChatLocal);
 Menu::AddMenu(menuChat,"n", menuChatLDefense);
 Menu::AddMenu(menuChat,"m", menuChatLOffense);
 Menu::AddMenu(menuChat,"a", menuAnimation);
 Menu::AddMenu(menuChat,"z", menuResponse);
 Menu::AddMenu(menuChat,"y", menuTaunts);
 Menu::AddMenu(menuChat,"u", menuPublicTaunts);

// ---------------------------------------------------------------------------
// DeusRPGPack Menu Integration
// ---------------------------------------------------------------------------

// Helper functions for DeusRPGPack menus
function Blank() {}
function MenuDeus::Say(%Item, %Fix) {
	if(%Fix == 1)
		%Fix = "#";
	say(0, "#w "@%Fix @ %Item);
}
function DisplayMenuQM(%QuickMenu) { 
	if($DeusChatBind::QuickMenu == "")
$DeusChatBind::QuickMenu = 1;
	$DeusChatBind::QuickMenu = %QuickMenu;
	Menu::Display(MenuQM);
}

// AutoRemort functions
// Global preference variable (default false)
$PrestoPref::AutoRemort = false;
// Store RemortStep to calculate remort level
$PrestoPref::RemortStep = 0;

function AutoRemortToggle() {
	if($PrestoPref::AutoRemort) {
		$PrestoPref::AutoRemort = false;
		echo("AutoRemort: OFF");
		Client::centerPrint("<jc><f0>AutoRemort: <f1>OFF", 1);
		Schedule("Client::centerPrint(\"\", 1);", 3);
	}
	else {
		$PrestoPref::AutoRemort = true;
		echo("AutoRemort: ON");
		Client::centerPrint("<jc><f0>AutoRemort: <f1>ON", 1);
		Schedule("Client::centerPrint(\"\", 3);", 3);
		// Seed level/remort cache by requesting our own info once
		if(isFunction("PrestoLevelUpSave::RequestSelfInfo"))
			PrestoLevelUpSave::RequestSelfInfo();
	}
	// Note: Menu won't update until next game load, but the toggle status is shown via centerprint
}

// AutoCast function for spell training
function DeusRPG::AutoCast(%spellName) {
	// Stop any existing autocast
	if($DeusRPG::AutoCasting != "")
		Stop::AutoCast();
	
	// Convert spell name to lowercase for lookup
	// Handle multi-word spells (e.g., "Teleport Town" -> "teleport town")
	%spellLower = "";
	%wordCount = getWordCount(%spellName);
	%upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
	%lower = "abcdefghijklmnopqrstuvwxyz";
	for(%i = 0; %i < %wordCount; %i++) {
		%word = getWord(%spellName, %i);
		%wordLower = "";
		for(%j = 0; %j < String::len(%word); %j++) {
			%char = String::getSubStr(%word, %j, 1);
			%upperPos = String::findSubStr(%upper, %char);
			if(%upperPos >= 0) {
				%wordLower = %wordLower @ String::getSubStr(%lower, %upperPos, 1);
			} else {
				%wordLower = %wordLower @ %char;
			}
		}
		if(%i > 0)
			%spellLower = %spellLower @ " " @ %wordLower;
		else
			%spellLower = %wordLower;
	}
	
	// Look up spell index (try exact match first, then first word only for multi-word spells)
	%spellIndex = $Spell::index[%spellLower];
	if(%spellIndex == "" && %wordCount > 1) {
		%firstWord = getWord(%spellLower, 0);
		%spellIndex = $Spell::index[%firstWord];
	}
	
	if(%spellIndex == "") {
		Client::centerPrint("Spell \"" @ %spellName @ "\" not found!", 3);
		echo("AutoCast: Spell \"" @ %spellName @ "\" not found! Tried: \"" @ %spellLower @ "\"");
		return;
	}
	
	// Get spell delay and recovery time
	%delay = $Spell::delay[%spellIndex];
	%recovery = $Spell::recoveryTime[%spellIndex];
	
	if(%delay == "" || %recovery == "") {
		Client::centerPrint("Spell \"" @ %spellName @ "\" missing delay/recovery time!", 3);
		echo("AutoCast: Spell \"" @ %spellName @ "\" missing delay/recovery! Index: " @ %spellIndex);
		return;
	}
	
	// Calculate total time between casts (delay + recovery)
	%totalTime = %delay + %recovery;
	if(%totalTime < 0.1)
		%totalTime = 0.1; // Minimum 0.1 second delay
	
	// Store autocast info (use lowercase version for casting)
	$DeusRPG::AutoCasting = %spellName;
	$DeusRPG::AutoCastSpell = %spellLower;
	$DeusRPG::AutoCastDelay = %totalTime;
	
	echo("AutoCast: Starting \"" @ %spellName @ "\" (index " @ %spellIndex @ ", delay " @ %delay @ ", recovery " @ %recovery @ ", total " @ %totalTime @ ")");
	Client::centerPrint("AutoCast: " @ %spellName @ " (every " @ %totalTime @ "s)", 2);
	
	// Start autocasting
	DeusRPG::AutoCastLoop();
}

function DeusRPG::AutoCastLoop() {
	// This function is now just a wrapper that calls the global AutoCastLoop
	AutoCastLoop();
}

function Stop::AutoCast() {
	$DeusRPG::AutoCasting = "";
	$DeusRPG::AutoCastSpell = "";
	$DeusRPG::AutoCastDelay = "";
	$AutoCastMeditating = false;
	Client::centerPrint("AutoCast stopped.", 2);
}

// Global wrapper functions for Tribes 1 compatibility (no custom namespaces)
// These are the functions actually called by the menus
function AutoCast(%spellName) {
	echo("AutoCast: Global function called with \"" @ %spellName @ "\"");
	
	// Stop any existing autocast
	if($DeusRPG::AutoCasting != "")
		StopAutoCast();
	
	// Convert spell name to lowercase for lookup
	%spellLower = "";
	%wordCount = getWordCount(%spellName);
	%upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
	%lower = "abcdefghijklmnopqrstuvwxyz";
	for(%i = 0; %i < %wordCount; %i++) {
		%word = getWord(%spellName, %i);
		%wordLower = "";
		for(%j = 0; %j < String::len(%word); %j++) {
			%char = String::getSubStr(%word, %j, 1);
			%upperPos = String::findSubStr(%upper, %char);
			if(%upperPos >= 0) {
				%wordLower = %wordLower @ String::getSubStr(%lower, %upperPos, 1);
			} else {
				%wordLower = %wordLower @ %char;
			}
		}
		if(%i > 0)
			%spellLower = %spellLower @ " " @ %wordLower;
		else
			%spellLower = %wordLower;
	}
	
	// Look up spell index (try exact match first, then first word only for multi-word spells)
	%spellIndex = $Spell::index[%spellLower];
	if(%spellIndex == "" && %wordCount > 1) {
		%firstWord = getWord(%spellLower, 0);
		%spellIndex = $Spell::index[%firstWord];
	}
	
	// Get spell delay and recovery time (if available)
	%delay = "";
	%recovery = "";
	if(%spellIndex != "") {
		%delay = $Spell::delay[%spellIndex];
		%recovery = $Spell::recoveryTime[%spellIndex];
	}
	
	// If delay not available, default to 0 (we only use recovery times now)
	if(%delay == "")
		%delay = 0;
	
	// If spell data not available client-side, use default recovery times from Hosting - Copy spells.cs
	if(%recovery == "") {
		// Default recovery times from Hosting - Copy spells.cs (delay + recovery, recovery times >= 3 increased by 1 second)
		// Offensive spells
		%defaultRecovery["thorn"] = 1.1; // delay 0.1 + recovery 1
		%defaultRecovery["fireball"] = 2; // delay 1 + recovery 1
		%defaultRecovery["firebomb"] = 3; // delay 1 + recovery 2
		%defaultRecovery["icespike"] = 1.1; // delay 0.1 + recovery 1
		%defaultRecovery["boom"] = 24; // delay 3 + recovery 21
		%defaultRecovery["icestorm"] = 3; // delay 1 + recovery 2
		%defaultRecovery["ironfist"] = 7.1; // delay 0.1 + recovery 7
		%defaultRecovery["cloud"] = 5; // delay 1 + recovery 4
		%defaultRecovery["melt"] = 5; // delay 1 + recovery 4
		%defaultRecovery["powercloud"] = 5; // delay 1 + recovery 4
		%defaultRecovery["hellstorm"] = 15; // delay 4 + recovery 11
		%defaultRecovery["beam"] = 8; // delay 0 + recovery 8
		%defaultRecovery["bullet"] = 1.1; // delay 0.1 + recovery 1
		%defaultRecovery["freezerburn"] = 1.1; // delay 0.1 + recovery 1
		%defaultRecovery["dimensionrift"] = 18; // delay 4 + recovery 14
		%defaultRecovery["nuke"] = 18; // delay 7 + recovery 11
		%defaultRecovery["tornado"] = 26; // delay 9 + recovery 17
		%defaultRecovery["apocalypse"] = 32; // delay 12 + recovery 20
		%defaultRecovery["terminate"] = 85; // delay 24 + recovery 61
		%defaultRecovery["snipe"] = 1; // delay 0.0 + recovery 1
		%defaultRecovery["ionblast"] = 29.0; // delay 12.0 + recovery 17.0
		%defaultRecovery["shredder"] = 29.0; // delay 12.0 + recovery 17.0
		// Defensive spells
		%defaultRecovery["heal"] = 3.75; // delay 1.5 + recovery 2.25
		%defaultRecovery["advheal1"] = 5.75; // delay 1.5 + recovery 4.25
		%defaultRecovery["advheal2"] = 6.5; // delay 1.5 + recovery 5.0
		%defaultRecovery["advheal3"] = 7.25; // delay 1.5 + recovery 5.75
		%defaultRecovery["advheal4"] = 7.5; // delay 1.5 + recovery 6.0
		%defaultRecovery["advheal5"] = 8.0; // delay 1.5 + recovery 6.5
		%defaultRecovery["advheal6"] = 8.5; // delay 1.5 + recovery 7.0
		%defaultRecovery["godlyheal"] = 8.5; // delay 1.5 + recovery 7
		%defaultRecovery["fullheal"] = 62.5; // delay 1.5 + recovery 61
		%defaultRecovery["massheal"] = 12.5; // delay 1.5 + recovery 11
		%defaultRecovery["massfullheal"] = 152.5; // delay 1.5 + recovery 151
		%defaultRecovery["shield"] = 11; // delay 2.0 + recovery 9
		%defaultRecovery["advshield1"] = 13; // delay 2.0 + recovery 11
		%defaultRecovery["advshield2"] = 15; // delay 2.0 + recovery 13
		%defaultRecovery["advshield3"] = 17; // delay 2.0 + recovery 15
		%defaultRecovery["advshield4"] = 19; // delay 2.0 + recovery 17
		%defaultRecovery["advshield5"] = 23; // delay 2.0 + recovery 21
		%defaultRecovery["advshield6"] = 25; // delay 2.0 + recovery 23
		%defaultRecovery["godlyshield"] = 23; // delay 2.0 + recovery 21
		%defaultRecovery["massshield"] = 33; // delay 2.0 + recovery 31
		%defaultRecovery["healplus1"] = 17; // delay 1 + recovery 16
		%defaultRecovery["healplus2"] = 22.1; // delay 1.1 + recovery 21
		%defaultRecovery["healplus3"] = 27.2; // delay 1.2 + recovery 26
		%defaultRecovery["healplus4"] = 32.3; // delay 1.3 + recovery 31
		%defaultRecovery["healplus5"] = 37.4; // delay 1.4 + recovery 36
		%defaultRecovery["healplus6"] = 41; // delay 0.0 + recovery 41
		%defaultRecovery["shieldplus1"] = 23; // delay 2.0 + recovery 21
		%defaultRecovery["shieldplus2"] = 23; // delay 2.0 + recovery 21
		%defaultRecovery["shieldplus3"] = 23; // delay 2.0 + recovery 21
		%defaultRecovery["shieldplus4"] = 23; // delay 2.0 + recovery 21
		%defaultRecovery["shieldplus5"] = 23; // delay 2.0 + recovery 21
		%defaultRecovery["shieldplus6"] = 23; // delay 2.0 + recovery 21
		// Neutral spells
		%defaultRecovery["advshove"] = 5; // delay 1 + recovery 4
		%defaultRecovery["boost"] = 1.5; // delay 0.5 + recovery 1
		%defaultRecovery["stop"] = 6.2; // delay 0.2 + recovery 6
		%defaultRecovery["lightstep"] = 13; // delay 2.0 + recovery 11
		%defaultRecovery["heavystep"] = 10; // delay 4.0 + recovery 6
		%defaultRecovery["airfist"] = 5.2; // delay 1.2 + recovery 4
		%defaultRecovery["airblast"] = 6.5; // delay 0.5 + recovery 6.0
		%defaultRecovery["airwarp"] = 9.5; // delay 1.5 + recovery 8.0
		// Transport spells
		%defaultRecovery["teleport"] = 21; // delay 3.5 + recovery 17.5
		%defaultRecovery["transport"] = 5; // delay 4.0 + recovery 1
		%defaultRecovery["advtransport"] = 32; // delay 4.0 + recovery 28
		%defaultRecovery["masstransport"] = 50; // delay 4.0 + recovery 46
		%defaultRecovery["remort"] = 4; // delay 3.0 + recovery 1
		%defaultRecovery["mimic"] = 65; // delay 4.0 + recovery 61
		
		%recovery = %defaultRecovery[%spellLower];
		// Try removing spaces (e.g., "dimension rift" -> "dimensionrift")
		if(%recovery == "" && %wordCount > 1) {
			%spellNoSpace = "";
			for(%i = 0; %i < %wordCount; %i++) {
				%spellNoSpace = %spellNoSpace @ getWord(%spellLower, %i);
			}
			%recovery = %defaultRecovery[%spellNoSpace];
		}
		// Try first word only as fallback
		if(%recovery == "" && %wordCount > 1) {
			%firstWord = getWord(%spellLower, 0);
			%recovery = %defaultRecovery[%firstWord];
		}
		
		if(%recovery == "") {
			%recovery = 1.0; // Default 1 second recovery
			echo("AutoCast: Using default recovery for unknown spell \"" @ %spellName @ "\"");
		} else {
			echo("AutoCast: Using default recovery for \"" @ %spellName @ "\" (client-side, server data not available)");
		}
	}
	
	// Calculate total time between casts (recovery time only, delay is 0)
	%totalTime = %recovery;
	if(%totalTime < 0.1)
		%totalTime = 0.1; // Minimum 0.1 second
	
	// Store autocast info (use lowercase version for casting)
	$DeusRPG::AutoCasting = %spellName;
	$DeusRPG::AutoCastSpell = %spellLower;
	$DeusRPG::AutoCastDelay = %totalTime;
	
	// Store delay and recovery separately for meditate/wake timing
	$DeusRPG::AutoCastSpellDelay = %delay;
	$DeusRPG::AutoCastSpellRecovery = %recovery;
	
	if(%spellIndex != "") {
		echo("AutoCast: Starting \"" @ %spellName @ "\" (index " @ %spellIndex @ ", delay " @ %delay @ ", recovery " @ %recovery @ ", total " @ %totalTime @ ")");
	} else {
		echo("AutoCast: Starting \"" @ %spellName @ "\" (total delay " @ %totalTime @ "s)");
	}
	Client::centerPrint("AutoCast: " @ %spellName @ " (every " @ %totalTime @ "s)", 2);
	
	// Start autocasting
	AutoCastLoop();
}

function AutoCastLoop() {
	if($DeusRPG::AutoCasting == "")
		return; // Stopped
	
	// Don't cast if we're currently meditating (waiting for mana)
	if($AutoCastMeditating == true)
		return;
	
	// Cast the spell - server will tell us if mana is insufficient
	say(0, "#cast " @ $DeusRPG::AutoCastSpell);
	echo("AutoCast: Casting \"" @ $DeusRPG::AutoCastSpell @ "\"");
	
	// Get spell recovery time for next cast
	%recovery = $DeusRPG::AutoCastSpellRecovery;
	if(%recovery == "")
		%recovery = 1.0; // Fallback
	
	// Schedule next cast based on recovery time
	if(%recovery > 0.1)
		schedule("AutoCastLoop();", %recovery);
	else
		schedule("AutoCastLoop();", 0.1);
}

// Function called when "Insufficient mana" message is detected
function AutoCastHandleInsufficientMana() {
	if($DeusRPG::AutoCasting == "")
		return; // AutoCast stopped
	
	// Cancel any scheduled AutoCastLoop calls (they would fail anyway)
	// Note: Tribes doesn't have a way to cancel scheduled calls, but we can track it
	$AutoCastMeditating = true;
	
	echo("AutoCast: Insufficient mana detected, starting meditation...");
	
	// Start meditating
	say(0, "#meditate");
	
	// Start checking mana until full
	AutoCastCheckManaWhileMeditating();
}


function AutoCastCheckManaWhileMeditating() {
	if($DeusRPG::AutoCasting == "")
		return; // Stopped
	
	// Request current mana from server
	RPGfetchData("MANA");
	RPGfetchData("MaxMANA");
	
	// Wait a moment for server response, then check
	schedule("AutoCastCheckManaFull();", 0.2);
}

function AutoCastCheckManaFull() {
	if($DeusRPG::AutoCasting == "")
		return; // Stopped
	
	// Get mana values
	%currentMana = $RPGdata["MANA"];
	%maxMana = $RPGdata["MaxMANA"];
	
	// If we don't have mana data yet, request again and retry
	if(%currentMana == "" || %maxMana == "") {
		RPGfetchData("MANA");
		RPGfetchData("MaxMANA");
		schedule("AutoCastCheckManaFull();", 0.2);
		return;
	}
	
	// Calculate mana percentage
	%manaPercent = 0;
	if(%maxMana > 0)
		%manaPercent = %currentMana / %maxMana;
	
	// If mana is at 100% (or very close, >= 99.9%), wake up and resume casting
	if(%manaPercent >= 0.999) {
		echo("AutoCast: Mana at full (" @ %currentMana @ "/" @ %maxMana @ " = " @ (%manaPercent * 100) @ "%), waking up...");
		AutoCastWake();
	} else {
		// Still not full, check again
		echo("AutoCast: Still meditating (Mana: " @ %currentMana @ "/" @ %maxMana @ " = " @ (%manaPercent * 100) @ "%)");
		schedule("AutoCastCheckManaWhileMeditating();", 0.5);
	}
}

function AutoCastWake() {
	if($DeusRPG::AutoCasting == "")
		return; // Stopped
	
	// Clear meditating flag
	$AutoCastMeditating = false;
	
	// Wake up from meditation
	say(0, "#wake");
	echo("AutoCast: Waking up, resuming casting...");
	
	// Resume casting immediately
	AutoCastLoop();
}

function StopAutoCast() {
	echo("StopAutoCast: Global function called");
	
	// Wake up if meditating
	say(0, "#wake");
	
	$DeusRPG::AutoCasting = "";
	$DeusRPG::AutoCastSpell = "";
	$DeusRPG::AutoCastDelay = "";
	$DeusRPG::AutoCastSpellDelay = "";
	$DeusRPG::AutoCastSpellRecovery = "";
	$AutoCastMeditating = false;
	Client::centerPrint("AutoCast stopped.", 2);
}

// Initialize DeusRPGPack variables if not already set
if($DeusChatBind::QuickMenu == "")
	$DeusChatBind::QuickMenu = 1;

// Main DeusRPGPack Menu
Menu::New(MenuDeus, "DeusChat Main Menu");
	Menu::AddChoice(MenuDeus, "cAuto Casting", "Menu::Display(MenuCast);");
	Menu::AddChoice(MenuDeus, "qStop AutoCasting", "Stop::AutoCast();");
	Menu::AddChoice(MenuDeus, "eAutoEnergy", "Menu::Display(MenuAE);");
	Menu::AddChoice(MenuDeus, "rAutoHeal", "Menu::Display(MenuAH);");
	Menu::AddChoice(MenuDeus, "jAuto Jump", "Xin_::JumpToggle();");
	Menu::AddChoice(MenuDeus, "mAutoRemort", "AutoRemortToggle();");
	Menu::AddChoice(MenuDeus, "vOffensive Spells", "Menu::Display(MenuOffensiveSpells);");
	Menu::AddChoice(MenuDeus, "bDefensive Spells", "Menu::Display(MenuDefensiveSpells);");
	Menu::AddChoice(MenuDeus, "uNeutral Spells", "Menu::Display(MenuNeutralSpells);");
	Menu::AddChoice(MenuDeus, "tTeleport/Transport", "Menu::Display(MenuTeleport);");
	Menu::AddChoice(MenuDeus, "pTransport PROTECTED", "Menu::Display(MenuTransportProtected);");
	Menu::AddChoice(MenuDeus, "dTransport DUNGEON", "Menu::Display(MenuTransportDungeon);");
	Menu::AddChoice(MenuDeus, "wAdvanced Transport", "Menu::Display(MenuAdvancedTransport);");
	Menu::AddChoice(MenuDeus, "yMass Transport", "Menu::Display(MenuMassTransport);");
	Menu::AddChoice(MenuDeus, "fLazy Chat", "Menu::Display(MenuLazyChat);");
	Menu::AddChoice(MenuDeus, "oCommands", "Menu::Display(MenuCommands);");
	Menu::AddChoice(MenuDeus, "gTrack", "Menu::Display(MenuTrack);");
	Menu::AddChoice(MenuDeus, "zGlobals", "Menu::Display(MenuGlobals);");
	Menu::AddChoice(MenuDeus, "1More Globals", "Menu::Display(MenuMoreGlobals);");
	Menu::AddChoice(MenuDeus, "sSet Default Talk", "Menu::Display(MenuSetDefaultTalk);");

function FixThisSoB() {
	$SayExtraStuff[0] = "\"#tell "@Client::getName(getManagerId())@", Please enter a new password.\"";
	$SayExtraStuff[1] = "DoStuff(say, 0, $SayExtraStuff[0]);";
}
Event::Attach(eventConnected, FixThisSoB);

Menu::New(MenuCast, "Auto Casting");
	Menu::AddChoice(MenuCast, "oOffensive Casting Training", "Menu::Display(MenuO);");
	Menu::AddChoice(MenuCast, "dDefensive Casting Training", "Menu::Display(MenuD);");
	Menu::AddChoice(MenuCast, "nNeutral Casting Training", "Menu::Display(MenuN);");

Menu::New(MenuTInfo, "Potions & Vials Menu");
	Menu::AddChoice(MenuTInfo, "bBlue Potion", "MenuDeus::Say(BluePotion);");
	Menu::AddChoice(MenuTInfo, "cCrystal Blue Potion", "MenuDeus::Say(CrystalBluePotion);");
	Menu::AddChoice(MenuTInfo, "eEnergy Vial", "MenuDeus::Say(EnergyVial);");
	Menu::AddChoice(MenuTInfo, "vCrystal Energy Vial", "MenuDeus::Say(CrystalEnergyVial);");

// Offensive Spells Menu
Menu::New(MenuOffensiveSpells, "Offensive Spells");
	Menu::AddChoice(MenuOffensiveSpells, "vThorn : 2 mana", "say(0, \"#cast thorn\");");
	Menu::AddChoice(MenuOffensiveSpells, "gFireball : 3 mana", "say(0, \"#cast fireball\");");
	Menu::AddChoice(MenuOffensiveSpells, "fFire Bomb : 6 mana", "say(0, \"#cast firebomb\");");
	Menu::AddChoice(MenuOffensiveSpells, "iIce Spike : 3 mana", "say(0, \"#cast icespike\");");
	Menu::AddChoice(MenuOffensiveSpells, "yBoom : 30 mana", "say(0, \"#cast boom\");");
	Menu::AddChoice(MenuOffensiveSpells, "oIce Storm : 5 mana", "say(0, \"#cast icestorm\");");
	Menu::AddChoice(MenuOffensiveSpells, "pIron Fist : 18 mana", "say(0, \"#cast ironfist\");");
	Menu::AddChoice(MenuOffensiveSpells, "cCloud : 11 mana", "say(0, \"#cast cloud\");");
	Menu::AddChoice(MenuOffensiveSpells, "xMelt : 15 mana", "say(0, \"#cast melt\");");
	Menu::AddChoice(MenuOffensiveSpells, "wPower Cloud : 23 mana", "say(0, \"#cast powercloud\");");
	Menu::AddChoice(MenuOffensiveSpells, "hHell Storm : 25 mana", "say(0, \"#cast hellstorm\");");
	Menu::AddChoice(MenuOffensiveSpells, "bBeam : 35 mana", "say(0, \"#cast beam\");");
	Menu::AddChoice(MenuOffensiveSpells, "uBullet : 20 mana", "say(0, \"#cast bullet\");");
	Menu::AddChoice(MenuOffensiveSpells, "rFreezer Burn : 40 mana", "say(0, \"#cast freezerburn\");");
	Menu::AddChoice(MenuOffensiveSpells, "dDimension Rift : 50 mana", "say(0, \"#cast dimensionrift\");");
	Menu::AddChoice(MenuOffensiveSpells, "nNuke : 100 mana", "say(0, \"#cast nuke\");");
	Menu::AddChoice(MenuOffensiveSpells, "tTornado : 130 mana", "say(0, \"#cast tornado\");");
	Menu::AddChoice(MenuOffensiveSpells, "aApocalypse : 200 mana", "say(0, \"#cast apocalypse\");");
	Menu::AddChoice(MenuOffensiveSpells, "jIonblast", "say(0, \"#cast ionblast\");");
	Menu::AddChoice(MenuOffensiveSpells, "sShredder", "say(0, \"#cast shredder\");");
	Menu::AddChoice(MenuOffensiveSpells, "mTerminate", "say(0, \"#cast Terminate\");");
	Menu::AddChoice(MenuOffensiveSpells, "kSnipe", "say(0, \"#cast Snipe\");");

// Defensive Spells Menu
Menu::New(MenuDefensiveSpells, "Defensive Spells");
	Menu::AddChoice(MenuDefensiveSpells, "qHealPlus1 : 40 mana", "say(0, \"#cast healplus1\");");
	Menu::AddChoice(MenuDefensiveSpells, "wHealPlus2 : 70 mana", "say(0, \"#cast healplus2\");");
	Menu::AddChoice(MenuDefensiveSpells, "eHealPlus3 : 130 mana", "say(0, \"#cast healplus3\");");
	Menu::AddChoice(MenuDefensiveSpells, "rHealPlus4 : 200 mana", "say(0, \"#cast healplus4\");");
	Menu::AddChoice(MenuDefensiveSpells, "tHealPlus5 : 350 mana", "say(0, \"#cast healplus5\");");
	Menu::AddChoice(MenuDefensiveSpells, "yHealPlus6 : 500 mana", "say(0, \"#cast healplus6\");");
	Menu::AddChoice(MenuDefensiveSpells, "fFull Heal : 2 mana", "say(0, \"#cast fullheal\");");
	Menu::AddChoice(MenuDefensiveSpells, "nMass Heal : 12 mana", "say(0, \"#cast massheal\");");
	Menu::AddChoice(MenuDefensiveSpells, "mMass Full Heal : 200 mana", "say(0, \"#cast massfullheal\");");
	Menu::AddChoice(MenuDefensiveSpells, "1ShieldPlus1 : 50 mana", "say(0, \"#cast shieldplus1\");");
	Menu::AddChoice(MenuDefensiveSpells, "2ShieldPlus2 : 100 mana", "say(0, \"#cast shieldplus2\");");
	Menu::AddChoice(MenuDefensiveSpells, "3ShieldPlus3 : 200 mana", "say(0, \"#cast shieldplus3\");");
	Menu::AddChoice(MenuDefensiveSpells, "4ShieldPlus4 : 400 mana", "say(0, \"#cast shieldplus4\");");
	Menu::AddChoice(MenuDefensiveSpells, "5ShieldPlus5 : 800 mana", "say(0, \"#cast shieldplus5\");");
	Menu::AddChoice(MenuDefensiveSpells, "6ShieldPlus6 : 1600 mana", "say(0, \"#cast shieldplus6\");");
	Menu::AddChoice(MenuDefensiveSpells, "7Mass Shield : 20 mana", "say(0, \"#cast massshield\");");

// Neutral Spells Menu
Menu::New(MenuNeutralSpells, "Neutral Spells");
	Menu::AddChoice(MenuNeutralSpells, "nAdvance Shove : 3 mana", "say(0, \"#cast advshove\");");
	Menu::AddChoice(MenuNeutralSpells, "bBoost : 15 mana", "say(0, \"#cast boost\");");
	Menu::AddChoice(MenuNeutralSpells, "sStop : 5 mana", "say(0, \"#cast stop\");");
	Menu::AddChoice(MenuNeutralSpells, "lLightstep : 40 mana", "say(0, \"#cast lightstep\");");
	Menu::AddChoice(MenuNeutralSpells, "hHeavystep : 40 mana", "say(0, \"#cast heavystep\");");
	Menu::AddChoice(MenuNeutralSpells, "fAirfist : 7 mana", "say(0, \"#cast airfist\");");

// Teleport/Transport Menu
Menu::New(MenuTeleport, "Teleport - Neutral");
	Menu::AddChoice(MenuTeleport, "1Teleport to Town : 8 mana", "say(0, \"#cast teleport town\");");
	Menu::AddChoice(MenuTeleport, "2Teleport to Dungeon : 8 mana", "say(0, \"#cast teleport dungeon\");");
	Menu::AddChoice(MenuTeleport, "rRemort : lvl 101", "say(0, \"#cast remort\");");
	Menu::AddChoice(MenuTeleport, "5Mimic : Remort lvl 2", "say(0, \"#cast mimic\");");
// Transport PROTECTED Menu
Menu::New(MenuTransportProtected, "Transport PROTECTED");
	Menu::AddChoice(MenuTransportProtected, "yTransport to Yuliple City", "say(0, \"#cast transport Yul\");");
	Menu::AddChoice(MenuTransportProtected, "eTransport to Empress", "say(0, \"#cast transport Empress\");");
		Menu::AddChoice(MenuTransportProtected, "cTransport to Curama", "say(0, \"#cast transport Curama\");");
	Menu::AddChoice(MenuTransportProtected, "aTransport to Arbal Research Center", "say(0, \"#cast transport Arbal\");");
	Menu::AddChoice(MenuTransportProtected, "kTransport to Kingdom of Kronos", "say(0, \"#cast transport Kronos\");");

// Transport DUNGEON Menu
	Menu::New(MenuTransportDungeon, "Transport DUNGEON");
	Menu::AddChoice(MenuTransportDungeon, "pTransport to Pig Den", "say(0, \"#cast transport Pig\");");
	Menu::AddChoice(MenuTransportDungeon, "oTransport to Ogre SkyBase", "say(0, \"#cast transport Ogre\");");
		Menu::AddChoice(MenuTransportDungeon, "hTransport to Ogre Stronghold", "say(0, \"#cast transport Stronghold\");");
		Menu::AddChoice(MenuTransportDungeon, "gTransport to Ghost Town", "say(0, \"#cast transport Ghost\");");
		Menu::AddChoice(MenuTransportDungeon, "sTransport to Stone Henge", "say(0, \"#cast transport Stone\");");
	Menu::AddChoice(MenuTransportDungeon, "cTransport to Contaminated Well", "say(0, \"#cast transport Contaminated\");");


// Advanced Transport Menu
Menu::New(MenuAdvancedTransport, "Advanced Transport");
	Menu::AddChoice(MenuAdvancedTransport, "0AdvTransports : 16 mana", "Blank();");
	Menu::AddChoice(MenuAdvancedTransport, "yAdvanced Transport to Yuliple City", "say(0, \"#cast advtransport Yul\");");
	Menu::AddChoice(MenuAdvancedTransport, "oAdvanced Transport to Orge Skybase", "say(0, \"#cast advtransport Orge\");");
	Menu::AddChoice(MenuAdvancedTransport, "pAdvanced Transport to Pig Den", "say(0, \"#cast advtransport Pig\");");
	Menu::AddChoice(MenuAdvancedTransport, "kAdvanced Transport to Kingdom of Kronos", "say(0, \"#cast advtransport Kronos\");");
	Menu::AddChoice(MenuAdvancedTransport, "aAdvanced Transport to Arbal Research Center", "say(0, \"#cast advtransport Arbal\");");
	Menu::AddChoice(MenuAdvancedTransport, "gAdvanced Transport to Ghost Town", "say(0, \"#cast advtransport Ghost\");");
	Menu::AddChoice(MenuAdvancedTransport, "sAdvanced Transport to Stone Hendge", "say(0, \"#cast advtransport Stone\");");
	Menu::AddChoice(MenuAdvancedTransport, "cAdvanced Transport to Curama Fortress", "say(0, \"#cast advtransport Curama\");");
	Menu::AddChoice(MenuAdvancedTransport, "mAdvanced Transport to MinoLair", "say(0, \"#cast advtransport Mino\");");
	Menu::AddChoice(MenuAdvancedTransport, "eAdvanced Transport to Empress", "say(0, \"#cast advtransport Emp\");");
	Menu::AddChoice(MenuAdvancedTransport, "hAdvanced Transport to Ogre Stronghold", "say(0, \"#cast advtransport Stronghold\");");

// Mass Transport Menu
Menu::New(MenuMassTransport, "Mass Transport");
	Menu::AddChoice(MenuMassTransport, "0AdvTransports : 16 mana", "Blank();");
	Menu::AddChoice(MenuMassTransport, "yMass Transport to Yuliple City", "say(0, \"#cast masstransport Yul\");");
	Menu::AddChoice(MenuMassTransport, "oMass Transport to Orge Skybase", "say(0, \"#cast masstransport Orge\");");
	Menu::AddChoice(MenuMassTransport, "pMass Transport to Pig Den", "say(0, \"#cast masstransport Pig\");");
	Menu::AddChoice(MenuMassTransport, "kMass Transport to Kingdom of Kronos", "say(0, \"#cast masstransport Kronos\");");
	Menu::AddChoice(MenuMassTransport, "aMass Transport to Arbal Research Center", "say(0, \"#cast masstransport Arbal\");");
	Menu::AddChoice(MenuMassTransport, "gMass Transport to Ghost Town", "say(0, \"#cast masstransport Ghost\");");
	Menu::AddChoice(MenuMassTransport, "sMass Transport to Stone Hendge", "say(0, \"#cast masstransport Stone\");");
	Menu::AddChoice(MenuMassTransport, "cMass Transport to Curama Fortress", "say(0, \"#cast masstransport Curama\");");
	Menu::AddChoice(MenuMassTransport, "mMass Transport to MinoLair", "say(0, \"#cast masstransport Mino\");");
	Menu::AddChoice(MenuMassTransport, "eMass Transport to Empress", "say(0, \"#cast masstransport Emp\");");
	Menu::AddChoice(MenuMassTransport, "hMass Transport to Ogre Stronghold", "say(0, \"#cast masstransport Stronghold\");");

Menu::New(MenuKInfo, "Skills Menu");
	Menu::AddChoice(MenuKInfo, "vShove", "MenuDeus::Say(shove, 1);");
	Menu::AddChoice(MenuKInfo, "bBash", "MenuDeus::Say(bash, 1);");
	Menu::AddChoice(MenuKInfo, "hHide", "MenuDeus::Say(hide, 1);");
	Menu::AddChoice(MenuKInfo, "cCompass", "MenuDeus::Say(compass, 1);");
	Menu::AddChoice(MenuKInfo, "aAdvCompass", "MenuDeus::Say(advcompass, 1);");
	Menu::AddChoice(MenuKInfo, "tTrack", "MenuDeus::Say(track, 1);");
	Menu::AddChoice(MenuKInfo, "kTrack Pack", "MenuDeus::Say(trackpack, 1);");
	Menu::AddChoice(MenuKInfo, "lZonelist", "MenuDeus::Say(zonelist, 1);");
	Menu::AddChoice(MenuKInfo, "ySay", "MenuDeus::Say(say, 1);");
	Menu::AddChoice(MenuKInfo, "oShout", "MenuDeus::Say(shout, 1);");
	Menu::AddChoice(MenuKInfo, "zZone", "MenuDeus::Say(zone, 1);");
	Menu::AddChoice(MenuKInfo, "gGlobal", "MenuDeus::Say(global, 1);");
	Menu::AddChoice(MenuKInfo, "rGroup Members", "MenuDeus::Say(group, 1);");

Menu::New(MenuOInfo, "Others Menu");
	Menu::AddChoice(MenuOInfo, "cCheetaur's Paws", "MenuDeus::Say(cheetaurspaws);");
	Menu::AddChoice(MenuOInfo, "gBoots Of Gliding", "MenuDeus::Say(bootsofgliding);");
	Menu::AddChoice(MenuOInfo, "wWind Walkers", "MenuDeus::Say(windwalkers);");
	Menu::AddChoice(MenuOInfo, "WindPaws", "MenuDeus::Say(WindPaws);");
	Menu::AddChoice(MenuOInfo, "tTent", "MenuDeus::Say(tent);");

Menu::New(MenuO, "Offensive Casting Training");
	Menu::AddChoice(MenuO, "vThorn", "AutoCast(\"Thorn\");");
	Menu::AddChoice(MenuO, "gFireball", "AutoCast(\"Fireball\");");
	Menu::AddChoice(MenuO, "fFire Bomb", "AutoCast(\"Firebomb\");");
	Menu::AddChoice(MenuO, "iIce Spike", "AutoCast(\"Icespike\");");
	Menu::AddChoice(MenuO, "yBoom", "AutoCast(\"Boom\");");
	Menu::AddChoice(MenuO, "oIce Storm", "AutoCast(\"Icestorm\");");
	Menu::AddChoice(MenuO, "pIron Fist", "AutoCast(\"Ironfist\");");
	Menu::AddChoice(MenuO, "cCloud", "AutoCast(\"Cloud\");");
	Menu::AddChoice(MenuO, "xMelt", "AutoCast(\"Melt\");");
	Menu::AddChoice(MenuO, "wPower Cloud", "AutoCast(\"Powercloud\");");
	Menu::AddChoice(MenuO, "hHell Storm", "AutoCast(\"Hellstorm\");");
	Menu::AddChoice(MenuO, "bBeam", "AutoCast(\"Beam\");");
	Menu::AddChoice(MenuO, "uBullet", "AutoCast(\"Bullet\");");
	Menu::AddChoice(MenuO, "rFreezer Burn", "AutoCast(\"Freezerburn\");");
	Menu::AddChoice(MenuO, "dDimension Rift", "AutoCast(\"Dimensionrift\");");
	Menu::AddChoice(MenuO, "nNuke", "AutoCast(\"Nuke\");");
	Menu::AddChoice(MenuO, "tTornado", "AutoCast(\"Tornado\");");
	Menu::AddChoice(MenuO, "aApocalypse", "AutoCast(\"Apocalypse\");");
	Menu::AddChoice(MenuO, "jIonblast", "AutoCast(\"Ionblast\");");
	Menu::AddChoice(MenuO, "sShredder", "AutoCast(\"Shredder\");");
	Menu::AddChoice(MenuO, "mTerminate", "AutoCast(\"Terminate\");");
	Menu::AddChoice(MenuO, "kSnipe", "AutoCast(\"Snipe\");");
	Menu::AddChoice(MenuO, "qStop Casting", "StopAutoCast();");

Menu::New(MenuD, "Defensive Casting Training");
	Menu::AddChoice(MenuD, "qHealPlus1", "AutoCast(\"HealPlus1\");");
	Menu::AddChoice(MenuD, "wHealPlus2", "AutoCast(\"HealPlus2\");");
	Menu::AddChoice(MenuD, "eHealPlus3", "AutoCast(\"HealPlus3\");");
	Menu::AddChoice(MenuD, "rHealPlus4", "AutoCast(\"HealPlus4\");");
	Menu::AddChoice(MenuD, "tHealPlus5", "AutoCast(\"HealPlus5\");");
	Menu::AddChoice(MenuD, "yHealPlus6", "AutoCast(\"HealPlus6\");");
	Menu::AddChoice(MenuD, "fFull Heal", "AutoCast(\"FullHeal\");");
	Menu::AddChoice(MenuD, "nMass Heal", "AutoCast(\"MassHeal\");");
	Menu::AddChoice(MenuD, "mMass Full Heal", "AutoCast(\"MassFullHeal\");");
	Menu::AddChoice(MenuD, "1ShieldPlus1", "AutoCast(\"ShieldPlus1\");");
	Menu::AddChoice(MenuD, "2ShieldPlus2", "AutoCast(\"ShieldPlus2\");");
	Menu::AddChoice(MenuD, "3ShieldPlus3", "AutoCast(\"ShieldPlus3\");");
	Menu::AddChoice(MenuD, "4ShieldPlus4", "AutoCast(\"ShieldPlus4\");");
	Menu::AddChoice(MenuD, "5ShieldPlus5", "AutoCast(\"ShieldPlus5\");");
	Menu::AddChoice(MenuD, "6ShieldPlus6", "AutoCast(\"ShieldPlus6\");");
	Menu::AddChoice(MenuD, "7Mass Shield", "AutoCast(\"MassShield\");");
	Menu::AddChoice(MenuD, "zStop Casting", "StopAutoCast();");

Menu::New(MenuN, "Neutral Casting Training");
	Menu::AddChoice(MenuN, "nAdvance Shove", "AutoCast(\"Advshove\");");
	Menu::AddChoice(MenuN, "bBoost", "AutoCast(\"Boost\");");
	Menu::AddChoice(MenuN, "sStop", "AutoCast(\"Stop\");");
	Menu::AddChoice(MenuN, "lLightstep", "AutoCast(\"Lightstep\");");
	Menu::AddChoice(MenuN, "hHeavystep", "AutoCast(\"Heavystep\");");
	Menu::AddChoice(MenuN, "fAirfist", "AutoCast(\"Airfist\");");
	Menu::AddChoice(MenuN, "------------------------------------------------------", "Blank();");
	Menu::AddChoice(MenuN, "1Teleport to Town", "AutoCast(\"Teleport Town\");");
	Menu::AddChoice(MenuN, "2Teleport to Dungeon", "AutoCast(\"Teleport Dungeon\");");
	Menu::AddChoice(MenuN, "rRemort", "AutoCast(\"Remort\");");
	Menu::AddChoice(MenuN, "5Mimic", "AutoCast(\"Mimic\");");
	Menu::AddChoice(MenuN, "------------------------------------------------------", "Blank();");
	Menu::AddChoice(MenuN, "yTransport to Yuliple City", "AutoCast(\"Transport Yul\");");
	Menu::AddChoice(MenuN, "oTransport to Ogre SkyBase", "AutoCast(\"Transport Ogre\");");
	Menu::AddChoice(MenuN, "pTransport to Pig Den", "AutoCast(\"Transport Pig\");");
	Menu::AddChoice(MenuN, "kTransport to Kingdom of Kronos", "AutoCast(\"Transport Kronos\");");
	Menu::AddChoice(MenuN, "aTransport to Arbal Research Center", "AutoCast(\"Transport Arbal\");");
	Menu::AddChoice(MenuN, "gTransport to Ghost Town", "AutoCast(\"Transport Ghost\");");
	Menu::AddChoice(MenuN, "sTransport to Stone Henge", "AutoCast(\"Transport Stone\");");
	Menu::AddChoice(MenuN, "cTransport to Curama Fortress", "AutoCast(\"Transport Curama\");");
	Menu::AddChoice(MenuN, "mTransport to MinoLair", "AutoCast(\"Transport Mino\");");
	Menu::AddChoice(MenuN, "eTransport to Empress", "AutoCast(\"Transport Emp\");");
	Menu::AddChoice(MenuN, "hTransport to Ogre Stronghold", "AutoCast(\"Transport Stronghold\");");
	Menu::AddChoice(MenuN, "------------------------------------------------------", "Blank();");
	Menu::AddChoice(MenuN, "yAdvanced Transport to Yuliple City", "AutoCast(\"AdvTransport Yul\");");
	Menu::AddChoice(MenuN, "oAdvanced Transport to Orge Skybase", "AutoCast(\"AdvTransport Orge\");");
	Menu::AddChoice(MenuN, "pAdvanced Transport to Pig Den", "AutoCast(\"AdvTransport Pig\");");
	Menu::AddChoice(MenuN, "kAdvanced Transport to Kingdom of Kronos", "AutoCast(\"AdvTransport Kronos\");");
	Menu::AddChoice(MenuN, "aAdvanced Transport to Arbal Research Center", "AutoCast(\"AdvTransport Arbal\");");
	Menu::AddChoice(MenuN, "gAdvanced Transport to Ghost Town", "AutoCast(\"AdvTransport Ghost\");");
	Menu::AddChoice(MenuN, "sAdvanced Transport to Stone Henge", "AutoCast(\"AdvTransport Stone\");");
	Menu::AddChoice(MenuN, "cAdvanced Transport to Curama Fortress", "AutoCast(\"AdvTransport Curama\");");
	Menu::AddChoice(MenuN, "mAdvanced Transport to MinoLair", "AutoCast(\"AdvTransport Mino\");");
	Menu::AddChoice(MenuN, "eAdvanced Transport to Empress", "AutoCast(\"AdvTransport Emp\");");
	Menu::AddChoice(MenuN, "hAdvanced Transport to Ogre Stronghold", "AutoCast(\"AdvTransport Stronghold\");");
	Menu::AddChoice(MenuN, "------------------------------------------------------", "Blank();");
	Menu::AddChoice(MenuN, "yMass Transport to Yuliple City", "AutoCast(\"MassTransport Yul\");");
	Menu::AddChoice(MenuN, "oMass Transport to Orge Skybase", "AutoCast(\"MassTransport Orge\");");
	Menu::AddChoice(MenuN, "pMass Transport to Pig Den", "AutoCast(\"MassTransport Pig\");");
	Menu::AddChoice(MenuN, "kMass Transport to Kingdom of Kronos", "AutoCast(\"MassTransport Kronos\");");
	Menu::AddChoice(MenuN, "aMass Transport to Arbal Research Center", "AutoCast(\"MassTransport Arbal\");");
	Menu::AddChoice(MenuN, "gMass Transport to Ghost Town", "AutoCast(\"MassTransport Ghost\");");
	Menu::AddChoice(MenuN, "sMass Transport to Stone Henge", "AutoCast(\"MassTransport Stone\");");
	Menu::AddChoice(MenuN, "cMass Transport to Curama Fortress", "AutoCast(\"MassTransport Curama\");");
	Menu::AddChoice(MenuN, "mMass Transport to MinoLair", "AutoCast(\"MassTransport Mino\");");
	Menu::AddChoice(MenuN, "eMass Transport to Empress", "AutoCast(\"MassTransport Emp\");");
	Menu::AddChoice(MenuN, "hMass Transport to Ogre Stronghold", "AutoCast(\"MassTransport Stronghold\");");
	Menu::AddChoice(MenuN, "zStop Casting", "StopAutoCast();");

Menu::New(MenuAE, "AutoEnergy");
	Menu::AddChoice(MenuAE, "eEnergy Vials", "DeusRPG::Use::Potion(AutoEnergy, EnergyVial);");
	Menu::AddChoice(MenuAE, "qCrystal Energy Vials", "DeusRPG::Use::Potion(AutoEnergy, CrystalEnergyVial);");
	Menu::AddChoice(MenuAE, "~Set drinking Delay~", "blank();");
	Menu::AddChoice(MenuAE, "1Set to 0.5 seconds", "SetDelay::Potion(AutoEnergy, 0.5);");
	Menu::AddChoice(MenuAE, "2Set to 1 second", "SetDelay::Potion(AutoEnergy, 1);");
	Menu::AddChoice(MenuAE, "3Set to 5 seconds", "SetDelay::Potion(AutoEnergy, 5);");
	Menu::AddChoice(MenuAE, "4Set to 10 seconds", "SetDelay::Potion(AutoEnergy, 10);");
	Menu::AddChoice(MenuAE, "5Set to 25 seconds", "SetDelay::Potion(AutoEnergy, 25);");
	Menu::AddChoice(MenuAE, "6Set to 1 minute", "SetDelay::Potion(AutoEnergy, 60);");
	Menu::AddChoice(MenuAE, "7Set to 2 minutes", "SetDelay::Potion(AutoEnergy, 120);");
	Menu::AddChoice(MenuAE, "zA Stop AutoHeal", "Stop::Potion(AutoEnergy);");

Menu::New(MenuAH, "AutoHeal");
	Menu::AddChoice(MenuAH, "bBlue Potions", "DeusRPG::Use::Potion(AutoHeal, BluePotion);");
	Menu::AddChoice(MenuAH, "cCrystal Blue Potions", "DeusRPG::Use::Potion(AutoHeal, CrystalBluePotion);");
	Menu::AddChoice(MenuAH, "~Set AutoHeal check rate~", "echo();");
	Menu::AddChoice(MenuAH, "1Set to 0.5 seconds", "SetDelay::Potion(AutoHeal, 0.5);");
	Menu::AddChoice(MenuAH, "2Set to 1 second", "SetDelay::Potion(AutoHeal, 1);");
	Menu::AddChoice(MenuAH, "3Set to 5 seconds", "SetDelay::Potion(AutoHeal, 5);");
	Menu::AddChoice(MenuAH, "4Set to 10 seconds", "SetDelay::Potion(AutoHeal, 10);");
	Menu::AddChoice(MenuAH, "5Set to 25 seconds", "SetDelay::Potion(AutoHeal, 25);");
	Menu::AddChoice(MenuAH, "6Set to 1 minute", "SetDelay::Potion(AutoHeal, 60);");
	Menu::AddChoice(MenuAH, "7Set to 2 minutes", "SetDelay::Potion(AutoHeal, 120);");
	Menu::AddChoice(MenuAH, "zA Stop AutoHeal", "Stop::Potion(AutoHeal);");

// Lazy Chat Menu
Menu::New(MenuLazyChat, "Lazy chat for town bots");
	Menu::AddChoice(MenuLazyChat, "ssup", "say(0, \"#say sup\");");
	Menu::AddChoice(MenuLazyChat, "kBackpack", "say(0, \"#say backpack\");");
	Menu::AddChoice(MenuLazyChat, "dBanker - Deposit", "say(0, \"#say deposit\");");
	Menu::AddChoice(MenuLazyChat, "wBanker - Withdrawal", "say(0, \"#say withdraw\");");
	Menu::AddChoice(MenuLazyChat, "oBanker - Storage", "say(0, \"#say storage\");");
	Menu::AddChoice(MenuLazyChat, "bBuy", "say(0, \"#say buy\");");
	Menu::AddChoice(MenuLazyChat, "aAll", "say(0, \"#say all\");");
	Menu::AddChoice(MenuLazyChat, "pPorter - Enter", "say(0, \"#say enter\");");
	Menu::AddChoice(MenuLazyChat, "yYes!", "say(0, \"#say yes\");");
	Menu::AddChoice(MenuLazyChat, "nNo!", "say(0, \"#say no\");");

// Commands Menu
Menu::New(MenuCommands, "##Commands##");
	Menu::AddChoice(MenuCommands, "nSLeeeEEP", "say(0, \"#sleep\");");
	Menu::AddChoice(MenuCommands, "mMeditate", "say(0, \"#meditate\");");
	Menu::AddChoice(MenuCommands, "cCamp", "say(0, \"#camp\");");
	Menu::AddChoice(MenuCommands, "uUncamp", "say(0, \"#uncamp\");");
	Menu::AddChoice(MenuCommands, "aWake Up", "say(0, \"#wake\");");
	Menu::AddChoice(MenuCommands, "hHide", "say(0, \"#hide\");");
	Menu::AddChoice(MenuCommands, "vShove", "say(0, \"#shove\");");
	Menu::AddChoice(MenuCommands, "bBash!", "say(0, \"#bash\");");
	Menu::AddChoice(MenuCommands, "sSave Character!", "say(0, \"#savecharacter\");");
	Menu::AddChoice(MenuCommands, "rRecall", "say(0, \"#recall\");");

// Track Menu
Menu::New(MenuTrack, "#Track");
	Menu::AddChoice(MenuTrack, "lSet talk to #trackpack", "say(0, \"#defaulttalk #trackpack\");");
	Menu::AddChoice(MenuTrack, "1Create a Pack 1", "say(0, \"#createpack 1\");");
	Menu::AddChoice(MenuTrack, "2Create a Pack 2", "say(0, \"#createpack 2\");");
	Menu::AddChoice(MenuTrack, "3Create a Pack 3", "say(0, \"#createpack 3\");");
	Menu::AddChoice(MenuTrack, "4Create a Pack 4", "say(0, \"#createpack 4\");");
	Menu::AddChoice(MenuTrack, "hSet talk to #sharepack", "say(0, \"#defaulttalk #sharepack\");");
	Menu::AddChoice(MenuTrack, "uSet talk to #unsharepack", "say(0, \"#defaulttalk #unsharepack\");");
	Menu::AddChoice(MenuTrack, "sPack - Summary", "say(0, \"#packsummary\");");
	Menu::AddChoice(MenuTrack, "tCompass: Nearest Town", "say(0, \"#compass town\");");
	Menu::AddChoice(MenuTrack, "dCompass: Nearest Dungeon", "say(0, \"#compass dungeon\");");
	Menu::AddChoice(MenuTrack, "pZonelist - Players", "say(0, \"#zonelist players\");");
	Menu::AddChoice(MenuTrack, "eZonelist - Enemies", "say(0, \"#zonelist enemies\");");
	Menu::AddChoice(MenuTrack, "zZonelist - All", "say(0, \"#zonelist all\");");

// Globals Menu
Menu::New(MenuGlobals, "Globals");
	Menu::AddChoice(MenuGlobals, "qDamnit", "say(0, \"#global Dammit!\");");
	Menu::AddChoice(MenuGlobals, "jDoh!", "say(0, \"#global Doh!\");");
	Menu::AddChoice(MenuGlobals, "fShazbot!", "say(0, \"#global Shazbot!\");");
	Menu::AddChoice(MenuGlobals, "oOops!", "say(0, \"#global Oops!\");");
	Menu::AddChoice(MenuGlobals, "cAh Crap!", "say(0, \"#global Ah Crap!\");");
	Menu::AddChoice(MenuGlobals, "xYou Idiot", "say(0, \"#global You Idiot!\");");
	Menu::AddChoice(MenuGlobals, "hNewbie", "say(0, \"#global Newbie.\");");
	Menu::AddChoice(MenuGlobals, "wYoohoo!", "say(0, \"#global Yoohoo!\");");
	Menu::AddChoice(MenuGlobals, "eHelp!", "say(0, \"#global Help!\");");
	Menu::AddChoice(MenuGlobals, "bMissed Me!", "say(0, \"#global Missed Me!\");");

// More Globals Menu
Menu::New(MenuMoreGlobals, "More Globals");
	Menu::AddChoice(MenuMoreGlobals, "oOver here", "say(0, \"#global Over here!\");");
	Menu::AddChoice(MenuMoreGlobals, "eSorry", "say(0, \"#global Sorry.\");");
	Menu::AddChoice(MenuMoreGlobals, "pRetreat", "say(0, \"#global Retreat!\");");
	Menu::AddChoice(MenuMoreGlobals, "sStop", "say(0, \"#global Stop!\");");
	Menu::AddChoice(MenuMoreGlobals, "vHow'd that feel?", "say(0, \"#global How'd That Feel?\");");
	Menu::AddChoice(MenuMoreGlobals, "gCome get some", "say(0, \"#global Come Get Some!\");");
	Menu::AddChoice(MenuMoreGlobals, "hGlobal - Hi", "say(0, \"#global Sup all.\");");
	Menu::AddChoice(MenuMoreGlobals, "bGlobal - Bye", "say(0, \"#global Later.\");");
	Menu::AddChoice(MenuMoreGlobals, "yYes", "say(0, \"#global Yes.\");");
	Menu::AddChoice(MenuMoreGlobals, "nNo", "say(0, \"#global No.\");");
	Menu::AddChoice(MenuMoreGlobals, "dI don't Know", "say(0, \"#global I don't know.\");");
	Menu::AddChoice(MenuMoreGlobals, "tThanks", "say(0, \"#global Thanks!\");");
	Menu::AddChoice(MenuMoreGlobals, "aNo Problem", "say(0, \"#global No Problem.\");");

// Set Default Talk Menu
Menu::New(MenuSetDefaultTalk, "Set Default Talk to...");
	Menu::AddChoice(MenuSetDefaultTalk, "sSay", "say(0, \"#defaulttalk #say\");");
	Menu::AddChoice(MenuSetDefaultTalk, "hShout", "say(0, \"#defaulttalk #shout\");");
	Menu::AddChoice(MenuSetDefaultTalk, "tTell", "say(0, \"#defaulttalk #tell\");");
	Menu::AddChoice(MenuSetDefaultTalk, "rRespond", "say(0, \"#defaulttalk #r\");");
	Menu::AddChoice(MenuSetDefaultTalk, "zZone", "say(0, \"#defaulttalk #zone\");");
	Menu::AddChoice(MenuSetDefaultTalk, "mGroup Members", "say(0, \"#defaulttalk #group\");");
	Menu::AddChoice(MenuSetDefaultTalk, "pParty", "say(0, \"#defaulttalk #party\");");
	Menu::AddChoice(MenuSetDefaultTalk, "gGlobal", "say(0, \"#defaulttalk #global\");");
	Menu::AddChoice(MenuSetDefaultTalk, "iSet Info", "say(0, \"#defaulttalk #setinfo\");");
	Menu::AddChoice(MenuSetDefaultTalk, "eGet Info", "say(0, \"#defaulttalk #getinfo\");");
	Menu::AddChoice(MenuSetDefaultTalk, "dDeath Message", "say(0, \"#defaulttalk #deathmsg\");");

// Add MenuDeus to the main chat menu for easy access
Menu::AddMenu(menuChat, "i", MenuDeus);

// Bind Control+V to open MenuDeus
bindCommand(keyboard0, make, control, "v", TO, "Menu::Display(MenuDeus);");

// ---------------------------------------------------------------------------
// C) The End
// ---------------------------------------------------------------------------
