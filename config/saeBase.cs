// saeBase.cs -- default keybind set for the Options > Controls "Base (Tribes) Defaults" button.
//
// GENERATED COPY. Source: base\scripts.zip -> sae.cs
// Re-copy from there if the upstream set changes.
//
// WHY A COPY EXISTS: both upstream default sets are named "sae.cs" -- base's lives inside
// base\scripts.zip and RPG's is loose in RPG\scripts\ -- so a bare exec("sae.cs") resolves to
// whichever the search path hits first (loose beats packed) and can never reach base's copy while
// a mod is mounted. config\ is a search-path entry (how config.cs and Presto\*.cs resolve), so
// the two sets live here under distinct names and each button can target one unambiguously.
//
// Movement: e forward / d back / s left / f right  (ESDF -- stock Tribes)

//
// Actions common to play & PDA modes
//
newActionMap("actionMap.sae");
bindAction(keyboard,	make,	tab,	 	to,	IDACTION_MENU_PAGE, 1);
bindAction(keyboard,	make,	escape,	TO,	IDACTION_ESCAPE_PRESSED, 0);
bindAction(keyboard,	make,	k,	 	   TO,	IDACTION_MENU_PAGE, 2);

bindAction(keyboard,	make,	p,			TO,	IDACTION_CHAT, 0);
bindAction(keyboard,	make,	y,			TO,	IDACTION_CHAT, 1);
bindAction(keyboard,	make,	u,			TO,	IDACTION_CHAT_DISP_SIZE, -1);
bindAction(keyboard,	make,	prior,	TO,	IDACTION_CHAT_DISP_PAGE, -1);
bindAction(keyboard,	make,	next,	   TO,	IDACTION_CHAT_DISP_PAGE, 1);

bindCommand(keyboard, make, shift, f1, to, "CmdInventoryGui::buyFavorites(1);");
bindCommand(keyboard, make, shift, f2, to, "CmdInventoryGui::buyFavorites(2);");
bindCommand(keyboard, make, shift, f3, to, "CmdInventoryGui::buyFavorites(3);");
bindCommand(keyboard, make, shift, f4, to, "CmdInventoryGui::buyFavorites(4);");
bindCommand(keyboard, make, shift, f5, to, "CmdInventoryGui::buyFavorites(5);");

bindCommand(keyboard, make, f1, to, "remoteEval(2048, PlayMode);");
bindCommand(keyboard, make, f5, to, "MEMode();");
bindCommand(keyboard, make, o, to, "remoteEval(2048, ToggleObjectivesMode);");
bindCommand(keyboard, make, i, to, "remoteEval(2048, ToggleInventoryMode);");
bindCommand(keyboard, make, c, to,  "remoteEval(2048, ToggleCommandMode);");

bindCommand(keyboard, make, control, x, to, "commandAck();");
bindCommand(keyboard, make, control, d, to, "commandDeclined();");
bindCommand(keyboard, make, control, c, to, "commandCompleted();");

bindCommand(keyboard, make, control, y, to, "voteYes();");
bindCommand(keyboard, make, control, n, to, "voteNo();");

bindCommand(keyboard, make, control, e, to, "targetClient();");
bindCommand(keyboard0, make, "n", TO, "sendControl(\"n\");");
bindCommand(keyboard0, make, "q", TO, "sendControl(\"q\");");

//
// Actions bound only in play mode
//
newActionMap("playMap.sae");
bindAction(mouse, xaxis, TO, IDACTION_YAW, scale, 0.001, flip);
bindAction(mouse, yaxis, TO, IDACTION_PITCH, scale, 0.001, flip);
bindAction(keyboard, make, s, to, IDACTION_MOVELEFT, 1.0);
bindAction(keyboard, break, s, to, IDACTION_MOVELEFT, 0.0);
bindAction(keyboard, make, f, to, IDACTION_MOVERIGHT, 1.0);
bindAction(keyboard, break, f, to, IDACTION_MOVERIGHT, 0.0);
bindAction(keyboard, make, d, to, IDACTION_MOVEBACK, 1.0);
bindAction(keyboard, break, d, to, IDACTION_MOVEBACK, 0.0);
bindAction(keyboard, make, e, to, IDACTION_MOVEFORWARD, 1.0);
bindAction(keyboard, break, e, to, IDACTION_MOVEFORWARD, 0.0);

bindAction(mouse, make, button1, to, IDACTION_JET, 1.0);
bindAction(mouse, break, button1, to, IDACTION_JET, 0.0);

bindAction(mouse, make, button, TO, IDACTION_FIRE1);
bindAction(mouse, break, button, TO, IDACTION_BREAK1);
bindAction(keyboard, make, a, TO, IDACTION_CROUCH);
bindAction(keyboard, break,a, TO, IDACTION_STAND);
bindAction(keyboard, make, x, TO, IDACTION_VIEW);
$pref::happyjump=False;
//if($pref::happyjump){
//	bindAction(keyboard0, make, "space", TO, IDACTION_TURNLEFT, 0.1);
//	bindAction(keyboard0, break, "space", TO, IDACTION_TURNLEFT, 0.000000);
//	bindAction(keyboard0, make, "left", TO, IDACTION_MOVEUP);
//}
//else{
	bindAction(keyboard, make, space, TO, IDACTION_MOVEUP);
//}

bindAction(keyboard, make, r, TO, IDACTION_SNIPER_FOV, 1);
bindAction(keyboard, break,r, TO, IDACTION_SNIPER_FOV, 0);
bindAction(keyboard, make, z, TO, IDACTION_INC_SNIPER_FOV, 1.0);

bindCommand(keyboard, make, v, TO, "setCMMode(PlayChatMenu, 2);");
if($PrestoPref::NewChat != ""){
	bindCommand(keyboard0, make, "v", TO, "Menu::Display(menuChat);");
	bindCommand(keyboard0, break, "v", TO, "");
}
if($PrestoPref::OldChat == "control v"){
	bindCommand(keyboard0, make, control, "v", TO, "Menu::DisplayDefault();");
	bindCommand(keyboard0, break, control, "v", TO, "");
}

bindCommand(keyboard, make, b, TO, "use(\"Beacon\");");
bindCommand(keyboard, make, m, TO, "throwStart();");
bindCommand(keyboard, break, m, TO, "throwRelease(\"Mine\");");
bindCommand(keyboard, make, g, TO, "throwStart();");
bindCommand(keyboard, break, g, TO, "throwRelease(\"Grenade\");");

bindCommand(keyboard, make, 1, to, "use(\"Blaster\");");
bindCommand(keyboard, make, 2, to, "use(\"Plasma Gun\");");
bindCommand(keyboard, make, 3, to, "use(\"Chaingun\");");
bindCommand(keyboard, make, 4, to, "use(\"Disc Launcher\");");
bindCommand(keyboard, make, 5, to, "use(\"Grenade Launcher\");");
bindCommand(keyboard, make, 6, to, "use(\"Laser Rifle\");");
bindCommand(keyboard, make, 7, to, "use(\"ELF gun\");");
bindCommand(keyboard, make, 8, to, "use(\"Mortar\");");
bindCommand(keyboard, make, 9, to, "use(\"Targeting Laser\");");

//bindAction (keyboard, make,	numpad8,	TO,	IDACTION_LOOKUP,		0.1);
//bindAction (keyboard, break,	numpad8,	TO,	IDACTION_LOOKUP,		0.0);
//bindAction (keyboard, make,	numpad2,	TO,	IDACTION_LOOKDOWN, 	0.1);
//bindAction (keyboard, break,	numpad2,	TO,	IDACTION_LOOKDOWN, 	0.0);
//bindAction (keyboard, make,	numpad6,	TO,	IDACTION_TURNRIGHT, 	0.1);
//bindAction (keyboard, break,	numpad6,	TO,	IDACTION_TURNRIGHT, 	0.0);
//bindAction (keyboard, make,	numpad4,	TO,	IDACTION_TURNLEFT,	0.1);
//bindAction (keyboard, break,	numpad4,	TO,	IDACTION_TURNLEFT,	0.0);
//bindAction (keyboard, make,	numpad5,	TO,	IDACTION_CENTERVIEW);

bindCommand(keyboard, make, h, to, "use(\"Repair Kit\");");
bindCommand(keyboard, make, t, to, "use(\"BackPack\");");
bindCommand(keyboard, make, control, p, to, "drop(BackPack);");
bindCommand(keyboard, make, control, q, to, "drop(Weapon);");
bindCommand(keyboard, make, control, a, to, "drop(Ammo);");
bindCommand(keyboard, make, control, f, to, "drop(Flag);");
bindCommand(keyboard, make, control, k, to, "kill();");

bindCommand(keyboard, make, w, to, "nextWeapon();");
bindCommand(keyboard, make, control, w, to, "prevWeapon();");

//bindCommand(keyboard, make, f, to, "setFreeLook(TRUE);");
//bindCommand(keyboard, break, f, to, "setFreeLook(FALSE);");

//
// Actions bound only in PDA mode
//
newActionMap("pdaMap.sae");
bindAction(keyboard,	make,	z, TO,	IDACTION_ZOOM_MODE_ON);
bindAction(keyboard,	break,z, TO,	IDACTION_ZOOM_MODE_OFF);

exec("extra-controls.cs");
