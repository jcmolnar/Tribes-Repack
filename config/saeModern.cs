// saeModern.cs -- default keybind set for the Options > Controls "Modern (Tribes) Defaults" button,
// and the set a NO-MOD launch boots into (console.cs execs it right after sae.cs when $modList
// is one word and that word is "base" -- a word test, not a string compare; see the note there).
//
// SOURCE: the 1.40.655 xLoader-era WASD layout, which is what returning players expect from a
// modern Tribes install. Derived from config\saeRPG.cs (already WASD, already zooms on the engine
// action) with the remaining 655 deltas applied. NOT a generated copy -- unlike saeBase.cs and
// saeRPG.cs there is no upstream sae.cs behind this one, so edit it here.
//
// Movement: w forward / s back / a left / d right  (WASD)
//
// WHAT WAS DELIBERATELY *NOT* TAKEN FROM THE 655 CONFIG, and why -- all four were verified
// against this tree, not assumed:
//
//   1. Zoom::In / Zoom::Out / Zoom::Cycle, AutoBuy::SelectAndBuyLoadout, AutoBuy::litterItem,
//      StatHUD::Show / Hide, TV::Activate, Demo::SpeedControl -- NINE functions the 655 config
//      binds that are defined NOWHERE in this tree (scanned 1037 loose .cs + 776 zipped, no .vol
//      files exist). They are xLoader/Presto-mod functions. Binding them would produce keys that
//      print a console error and do nothing. Zoom is the dangerous one: 655 replaces the ENGINE
//      action IDACTION_SNIPER_FOV with script Zoom::In/Out, so adopting it wholesale would have
//      DELETED working zoom. The key moves to 'e' (655's choice, free once movement is WASD);
//      the action stays the engine's.
//
//   2. numpad8/2/6/4 look+turn and numpad5 centerview. 655 binds these in playMap.sae; the
//      Repack sendControl grid (extra-controls.cs) binds the whole numpad in actionMap.sae.
//      BOTH maps are live during play, so taking these would fire the camera turn AND send a
//      control keystroke to the server on every press. RPG mods depend on that grid.
//
//   3. F1-F6 (AutoBuy loadouts, TV). extra-controls.cs runs AFTER this file (console.cs:230) and
//      rebinds f1-f12 to sendControl(), so anything on the F-row is overwritten regardless.
//      shift+F1-F5 buyFavorites survives -- different modifier combo -- and CmdInventoryGui::
//      buyFavorites DOES exist here (base\scripts\GUI.CS), so it is restored below (saeRPG has
//      it commented out).
//
//   4. inventoryMap.sae / demoMap.sae. Both bind only AutoBuy:: and Demo:: functions -- see (1).
//
// The F1/F5 PlayMode+MEMode binds below are kept for consistency with saeBase/saeRPG, but note
// they are dead here too: extra-controls.cs claims the F-row (above), and MEMode() is only ever
// DEFINED when the game is launched with -edit <mission> (console.cs:313-319 is the only path
// that execs editor.cs).

//
// Actions common to play & PDA modes
//
newActionMap("actionMap.sae");
bindAction(keyboard,	make,	tab,	 	to,	IDACTION_MENU_PAGE, 1);
bindAction(keyboard,	make,	escape,	TO,	IDACTION_ESCAPE_PRESSED, 0);
bindAction(keyboard,	make,	k,	 	   TO,	IDACTION_MENU_PAGE, 2);

// 655 delta: global chat moves p -> t, and Backpack moves t -> p (see playMap below).
// It is a rotation -- take both or neither, or the two collide on t.
bindAction(keyboard,	make,	t,			TO,	IDACTION_CHAT, 0);
bindAction(keyboard,	make,	y,			TO,	IDACTION_CHAT, 1);
bindAction(keyboard,	make,	u,			TO,	IDACTION_CHAT_DISP_SIZE, -1);
bindAction(keyboard,	make,	prior,	TO,	IDACTION_CHAT_DISP_PAGE, -1);
bindAction(keyboard,	make,	next,	   TO,	IDACTION_CHAT_DISP_PAGE, 1);

// Restored from saeBase (saeRPG comments these out). The 655 config puts AutoBuy loadouts on the
// bare F-row instead; that function does not exist here and the bare F-row is claimed anyway.
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
bindAction(keyboard, make, a, to, IDACTION_MOVELEFT, 1.0);
bindAction(keyboard, break, a, to, IDACTION_MOVELEFT, 0.0);
bindAction(keyboard, make, d, to, IDACTION_MOVERIGHT, 1.0);
bindAction(keyboard, break, d, to, IDACTION_MOVERIGHT, 0.0);
bindAction(keyboard, make, s, to, IDACTION_MOVEBACK, 1.0);
bindAction(keyboard, break, s, to, IDACTION_MOVEBACK, 0.0);
bindAction(keyboard, make, w, to, IDACTION_MOVEFORWARD, 1.0);
bindAction(keyboard, break, w, to, IDACTION_MOVEFORWARD, 0.0);

// JUMP-JET on RIGHT MOUSE (button0 is left/fire, button1 is right).
//
// This is jumpJet(), NOT IDACTION_JET -- they are different controls. IDACTION_JET is plain
// thrust; jumpJet(1) is Lem's combo from the lem_ski/unhappyjump port: it asserts ONE
// jumpAction and then holds jetting for as long as the button is down
// (kronosNativeCmds.cpp c_jumpJet). Every other set in this tree binds plain IDACTION_JET
// here, which is why RMB has never given the jump kick.
bindCommand(mouse0, make,  button1, TO, "jumpJet(1);");
bindCommand(mouse0, break, button1, TO, "jumpJet(0);");

bindAction(mouse, make, button, TO, IDACTION_FIRE1);
bindAction(mouse, break, button, TO, IDACTION_BREAK1);

// Crouch on x, third-person on r -- the 655 pair. autoexec.cs used to claim x for
// PrestoAutoAttackToggle AFTER every preset had run (it is exec'd at console.cs:232, last),
// which silently ate crouch here and IDACTION_VIEW in saeBase; auto-attack moved to j.
bindAction(keyboard, make, x, TO, IDACTION_CROUCH);
bindAction(keyboard, break,x, TO, IDACTION_STAND);
bindAction(keyboard, make, r, TO, IDACTION_VIEW);

// SKI JUMPING on SPACE -- the NATIVE ski() command, not IDACTION_MOVEUP.
//
// ski(1) sets PlayerPSC::skiHeld, which keeps curMove.jumpAction asserted in the move stream
// for as long as the key is held (kronosNativeCmds.cpp c_ski); ski(0) releases it. That is the
// engine port of the 1.40 packs' skiing -- upstream it took unhappyjump.dll plus lem_ski.acs.cs
// re-posting IDACTION_MOVEUP every 40 ms, and plugin DLLs never load in this client.
//
// The plain `bindAction(space, IDACTION_MOVEUP)` that saeBase/saeRPG use is a ONE-SHOT jump per
// keypress -- it does not sustain, so it cannot ski. The old $pref::happyjump route is the other
// pre-native workaround: repack.cs's "ski" mode sets happyjump=True and SWAPS the keys, moving
// MOVEUP onto the left arrow and putting a 0.099974 TURNLEFT nudge on space. Left False here
// because the native command replaces that hack outright -- do not set it True alongside ski(),
// or repack's mode would move jump off space behind this file's back.
$pref::happyjump = False;
bindCommand(keyboard0, make,  "space", TO, "ski(1);");
bindCommand(keyboard0, break, "space", TO, "ski(0);");

// 655 puts zoom on e (free once movement is WASD). The ACTION stays the engine's --
// Zoom::In/Out/Cycle do not exist in this tree. See note (1) in the header.
bindAction(keyboard, make, e, TO, IDACTION_SNIPER_FOV, 1);
bindAction(keyboard, break,e, TO, IDACTION_SNIPER_FOV, 0);
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

bindCommand(keyboard, make, h, to, "use(\"Repair Kit\");");
// 655 delta: Backpack t -> p (the other half of the chat rotation above).
bindCommand(keyboard, make, p, to, "use(\"BackPack\");");
bindCommand(keyboard, make, control, p, to, "drop(BackPack);");
// 655 delta: drop Weapon ctrl+q -> ctrl+w. ctrl+w is free here because weapon cycling
// moved to the wheel (below); ctrl+q stays unbound.
bindCommand(keyboard, make, control, w, to, "drop(Weapon);");
bindCommand(keyboard, make, control, a, to, "drop(Ammo);");
bindCommand(keyboard, make, control, f, to, "drop(Flag);");
bindCommand(keyboard, make, control, k, to, "kill();");

// 655 delta: weapon cycling moves off the keyboard (saeBase w / saeRPG f) onto the wheel,
// which frees w for MOVEFORWARD.
//
// ORIENTATION NOTE: 655 has zaxis1=next / zaxis0=prev. This file uses the opposite pairing
// ON PURPOSE, to agree with autoexec.cs:86-87 which has bound the wheel this way in this tree
// for a long time -- and which runs LAST (console.cs:232), so it wins over whatever is written
// here anyway. Matching it means the preset states the truth instead of a value that is
// immediately overwritten. Flip both files together if the direction is ever wrong.
bindCommand(mouse0, zaxis0, TO, "nextWeapon();"); //Wheel forward
bindCommand(mouse0, zaxis1, TO, "prevWeapon();"); //Wheel backward

//
// Actions bound only in PDA mode
//
newActionMap("pdaMap.sae");
bindAction(keyboard,	make,	z, TO,	IDACTION_ZOOM_MODE_ON);
bindAction(keyboard,	break,z, TO,	IDACTION_ZOOM_MODE_OFF);

exec("extra-controls.cs");

// Proof of execution -- the same convention nativeDefaults.cs documents ("keep the
// proof-of-execution echo"). A syntax error in this console prints ONE line and silently
// abandons the rest of the file, so a set that half-applied would otherwise look identical
// to one that worked. If this variable is set, everything above it ran.
//
// A bare echo() here is NOT enough on its own: this file runs from console.cs:189, which is
// before the console log file is open (measured -- the first line console.log actually
// captures is from the autoexec chain at :232). So record it in a variable and let
// nativeDefaults.cs report it once logging is live.
$BindSet::active = "modern (WASD)";
echo("[BINDS] modern (WASD) defaults applied -- WASD, crouch x, view r, zoom e, chat t, backpack p, jumpJet RMB, ski space");
