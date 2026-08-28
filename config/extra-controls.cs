//This is the meat of the numpad keybinds used in the Tribes Repack.
//The following function will be executed on the server:
//function remoteRawKey(%client, %key, %mod)
//On RPG mod, this function will be found in the remote.cs file
//On base, this function will be found in the server.cs file
//See either of those files for server documentation.



// NATIVE FIX 2026-08-27: every bind below is now bindCommandDefault, not bindCommand.
// console.cs runs this file at :289 -- AFTER config.cs at :288 -- so a plain bindCommand
// re-stamped all 92 events over the player's saved map on every single boot. Rebinding
// any numpad key, F-key or ctrl/alt digit in Options looked like it worked and silently
// reverted at the next launch.
//
// bindCommandDefault (simInputPlugin.cpp) applies a bind ONLY when the command is not
// already bound somewhere in this map AND the target key is free. On a fresh install the
// map is empty and all of these apply exactly as before; on an existing one they all
// skip, because config.cs already carries them. Either way the relay ends up bound --
// it just stops overwriting the player.
//
// The preset path still works: saeModern.cs:196 execs this file right after
// newActionMap() has CLEARED actionMap.sae, so every key is free and all 92 land.

editActionMap("actionMap.sae");

bindCommandDefault(keyboard0, make, "0", TO, "sendControl(\"0\");");


bindCommandDefault(keyboard0, make, "numpad1", TO, "sendControl(\"numpad1\");");
bindCommandDefault(keyboard0, make, "numpad2", TO, "sendControl(\"numpad2\");");
bindCommandDefault(keyboard0, make, "numpad3", TO, "sendControl(\"numpad3\");");
bindCommandDefault(keyboard0, make, "numpad4", TO, "sendControl(\"numpad4\");");
bindCommandDefault(keyboard0, make, "numpad5", TO, "sendControl(\"numpad5\");");
bindCommandDefault(keyboard0, make, "numpad6", TO, "sendControl(\"numpad6\");");
bindCommandDefault(keyboard0, make, "numpad7", TO, "sendControl(\"numpad7\");");
bindCommandDefault(keyboard0, make, "numpad8", TO, "sendControl(\"numpad8\");");
bindCommandDefault(keyboard0, make, "numpad9", TO, "sendControl(\"numpad9\");");
bindCommandDefault(keyboard0, make, "numpad0", TO, "sendControl(\"numpad0\");");
bindCommandDefault(keyboard0, make, "numpad+", TO, "sendControl(\"numpad+\");");
bindCommandDefault(keyboard0, make, "numpad-", TO, "sendControl(\"numpad-\");");
bindCommandDefault(keyboard0, make, "numpad*", TO, "sendControl(\"numpad*\");");
bindCommandDefault(keyboard0, make, "numpad/", TO, "sendControl(\"numpad/\");");
bindCommandDefault(keyboard0, make, "numpadenter", TO, "sendControl(\"numpadenter\");");


bindCommandDefault(keyboard0, make, control, "numpad1", TO, "sendControl(\"numpad1\", \"control\");");
bindCommandDefault(keyboard0, make, control, "numpad2", TO, "sendControl(\"numpad2\", \"control\");");
bindCommandDefault(keyboard0, make, control, "numpad3", TO, "sendControl(\"numpad3\", \"control\");");
bindCommandDefault(keyboard0, make, control, "numpad4", TO, "sendControl(\"numpad4\", \"control\");");
bindCommandDefault(keyboard0, make, control, "numpad5", TO, "sendControl(\"numpad5\", \"control\");");
bindCommandDefault(keyboard0, make, control, "numpad6", TO, "sendControl(\"numpad6\", \"control\");");
bindCommandDefault(keyboard0, make, control, "numpad7", TO, "sendControl(\"numpad7\", \"control\");");
bindCommandDefault(keyboard0, make, control, "numpad8", TO, "sendControl(\"numpad8\", \"control\");");
bindCommandDefault(keyboard0, make, control, "numpad9", TO, "sendControl(\"numpad9\", \"control\");");
bindCommandDefault(keyboard0, make, control, "numpad0", TO, "sendControl(\"numpad0\", \"control\");");
bindCommandDefault(keyboard0, make, control, "numpad+", TO, "sendControl(\"numpad+\", \"control\");");
bindCommandDefault(keyboard0, make, control, "numpad-", TO, "sendControl(\"numpad-\", \"control\");");
bindCommandDefault(keyboard0, make, control, "numpad*", TO, "sendControl(\"numpad*\", \"control\");");
bindCommandDefault(keyboard0, make, control, "numpad/", TO, "sendControl(\"numpad/\", \"control\");");
bindCommandDefault(keyboard0, make, control, "numpadenter", TO, "sendControl(\"numpadenter\", \"control\");");


bindCommandDefault(keyboard0, make, shift, "numpad1", TO, "sendControl(\"numpad1\", \"shift\");");
bindCommandDefault(keyboard0, make, shift, "numpad2", TO, "sendControl(\"numpad2\", \"shift\");");
bindCommandDefault(keyboard0, make, shift, "numpad3", TO, "sendControl(\"numpad3\", \"shift\");");
bindCommandDefault(keyboard0, make, shift, "numpad4", TO, "sendControl(\"numpad4\", \"shift\");");
bindCommandDefault(keyboard0, make, shift, "numpad5", TO, "sendControl(\"numpad5\", \"shift\");");
bindCommandDefault(keyboard0, make, shift, "numpad6", TO, "sendControl(\"numpad6\", \"shift\");");
bindCommandDefault(keyboard0, make, shift, "numpad7", TO, "sendControl(\"numpad7\", \"shift\");");
bindCommandDefault(keyboard0, make, shift, "numpad8", TO, "sendControl(\"numpad8\", \"shift\");");
bindCommandDefault(keyboard0, make, shift, "numpad9", TO, "sendControl(\"numpad9\", \"shift\");");
bindCommandDefault(keyboard0, make, shift, "numpad0", TO, "sendControl(\"numpad0\", \"shift\");");
bindCommandDefault(keyboard0, make, shift, "numpad+", TO, "sendControl(\"numpad+\", \"shift\");");
bindCommandDefault(keyboard0, make, shift, "numpad-", TO, "sendControl(\"numpad-\", \"shift\");");
bindCommandDefault(keyboard0, make, shift, "numpad*", TO, "sendControl(\"numpad*\", \"shift\");");
bindCommandDefault(keyboard0, make, shift, "numpad/", TO, "sendControl(\"numpad/\", \"shift\");");
bindCommandDefault(keyboard0, make, shift, "numpadenter", TO, "sendControl(\"numpadenter\", \"shift\");");


bindCommandDefault(keyboard0, make, alt, "numpad1", TO, "sendControl(\"numpad1\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "numpad2", TO, "sendControl(\"numpad2\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "numpad3", TO, "sendControl(\"numpad3\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "numpad4", TO, "sendControl(\"numpad4\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "numpad5", TO, "sendControl(\"numpad5\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "numpad6", TO, "sendControl(\"numpad6\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "numpad7", TO, "sendControl(\"numpad7\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "numpad8", TO, "sendControl(\"numpad8\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "numpad9", TO, "sendControl(\"numpad9\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "numpad0", TO, "sendControl(\"numpad0\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "numpad+", TO, "sendControl(\"numpad+\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "numpad-", TO, "sendControl(\"numpad-\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "numpad*", TO, "sendControl(\"numpad*\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "numpad/", TO, "sendControl(\"numpad/\", \"alt\");");
//No alt+enter! toggles fullscreen!


//Following 3 sets added in Repack 6
bindCommandDefault(keyboard0, make, "f1", TO, "sendControl(\"f1\");");
bindCommandDefault(keyboard0, make, "f2", TO, "sendControl(\"f2\");");
bindCommandDefault(keyboard0, make, "f3", TO, "sendControl(\"f3\");");
bindCommandDefault(keyboard0, make, "f4", TO, "sendControl(\"f4\");");
bindCommandDefault(keyboard0, make, "f5", TO, "sendControl(\"f5\");");
bindCommandDefault(keyboard0, make, "f6", TO, "sendControl(\"f6\");");
bindCommandDefault(keyboard0, make, "f7", TO, "sendControl(\"f7\");");
bindCommandDefault(keyboard0, make, "f8", TO, "sendControl(\"f8\");");
bindCommandDefault(keyboard0, make, "f9", TO, "sendControl(\"f9\");");
bindCommandDefault(keyboard0, make, "f10", TO, "sendControl(\"f10\");");
bindCommandDefault(keyboard0, make, "f11", TO, "sendControl(\"f11\");");
bindCommandDefault(keyboard0, make, "f12", TO, "sendControl(\"f12\");");


bindCommandDefault(keyboard0, make, control, "1", TO, "sendControl(\"1\", \"control\");");
bindCommandDefault(keyboard0, make, control, "2", TO, "sendControl(\"2\", \"control\");");
bindCommandDefault(keyboard0, make, control, "3", TO, "sendControl(\"3\", \"control\");");
bindCommandDefault(keyboard0, make, control, "4", TO, "sendControl(\"4\", \"control\");");
bindCommandDefault(keyboard0, make, control, "5", TO, "sendControl(\"5\", \"control\");");
bindCommandDefault(keyboard0, make, control, "6", TO, "sendControl(\"6\", \"control\");");
bindCommandDefault(keyboard0, make, control, "7", TO, "sendControl(\"7\", \"control\");");
bindCommandDefault(keyboard0, make, control, "8", TO, "sendControl(\"8\", \"control\");");
bindCommandDefault(keyboard0, make, control, "9", TO, "sendControl(\"9\", \"control\");");
bindCommandDefault(keyboard0, make, control, "0", TO, "sendControl(\"0\", \"control\");");


bindCommandDefault(keyboard0, make, alt, "1", TO, "sendControl(\"1\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "2", TO, "sendControl(\"2\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "3", TO, "sendControl(\"3\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "4", TO, "sendControl(\"4\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "5", TO, "sendControl(\"5\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "6", TO, "sendControl(\"6\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "7", TO, "sendControl(\"7\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "8", TO, "sendControl(\"8\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "9", TO, "sendControl(\"9\", \"alt\");");
bindCommandDefault(keyboard0, make, alt, "0", TO, "sendControl(\"0\", \"alt\");");