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
 Menu::AddLetter(menuAnimation, "x", "Stand pose", "remoteEval(2048, playAnim, 11);"); // no SAY
 Menu::AddAnimation(menuAnimation, "q", 5, yellYeah);
 Menu::AddAnimation(menuAnimation, "e", 6, yellWoohoo);
 Menu::AddAnimation(menuAnimation, "w", 7, yellAllRight);
 Menu::AddAnimation(menuAnimation, "v", 8, tauntHowdThatFeel);
 Menu::AddAnimation(menuAnimation, "g", 9, tauntComeGetSome);
 Menu::AddAnimation(menuAnimation, "h", 12, sayHi, "Wave hi");
 Menu::AddAnimation(menuAnimation, "b", 12, sayBye, "Wave bye");
 Menu::AddAnimation(menuAnimation, "m", 12, tauntMissedMe);

Menu::New(menuResponse, "Response"); Menu::AddLocalChat(menuResponse, "q", orderNotCompleted, "(local) Unable to complete.."); Menu::AddLocalChat(menuResponse, "a", orderAcknowledged, "(local) Acknowledged"); Menu::AddLocalChat(menuResponse, "z", orderCompleted, "(local) Objective completed.");
 Menu::AddResponse(menuResponse, "w", 0, orderNotCompleted); Menu::AddResponse(menuResponse, "s", 1, orderAcknowledged); Menu::AddResponse(menuResponse, "x", 0, orderCompleted);

 Menu::AddLetter(menuResponse, "e", "(public) Unable to complete..", "Say::Public(orderNotCompleted);"); Menu::AddLetter(menuResponse, "d", "(public) Acknowledged", "Say::Public(orderAcknowledged);"); Menu::AddLetter(menuResponse, "c", "(public) Objective completed.", "Say::Public(orderCompleted);");


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
// C) The End
// ---------------------------------------------------------------------------
