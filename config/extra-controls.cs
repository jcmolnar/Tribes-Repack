//This is the meat of the numpad keybinds used in the Tribes Repack.
//The following function will be executed on the server:
//function remoteRawKey(%client, %key, %mod)
//On RPG mod, this function will be found in the remote.cs file
//On base, this function will be found in the server.cs file
//See either of those files for server documentation.



editActionMap("actionMap.sae");

bindCommand(keyboard0, make, "0", TO, "sendControl(\"0\");");


bindCommand(keyboard0, make, "numpad1", TO, "sendControl(\"numpad1\");");
bindCommand(keyboard0, make, "numpad2", TO, "sendControl(\"numpad2\");");
bindCommand(keyboard0, make, "numpad3", TO, "sendControl(\"numpad3\");");
bindCommand(keyboard0, make, "numpad4", TO, "sendControl(\"numpad4\");");
bindCommand(keyboard0, make, "numpad5", TO, "sendControl(\"numpad5\");");
bindCommand(keyboard0, make, "numpad6", TO, "sendControl(\"numpad6\");");
bindCommand(keyboard0, make, "numpad7", TO, "sendControl(\"numpad7\");");
bindCommand(keyboard0, make, "numpad8", TO, "sendControl(\"numpad8\");");
bindCommand(keyboard0, make, "numpad9", TO, "sendControl(\"numpad9\");");
bindCommand(keyboard0, make, "numpad0", TO, "sendControl(\"numpad0\");");
bindCommand(keyboard0, make, "numpad+", TO, "sendControl(\"numpad+\");");
bindCommand(keyboard0, make, "numpad-", TO, "sendControl(\"numpad-\");");
bindCommand(keyboard0, make, "numpad*", TO, "sendControl(\"numpad*\");");
bindCommand(keyboard0, make, "numpad/", TO, "sendControl(\"numpad/\");");
bindCommand(keyboard0, make, "numpadenter", TO, "sendControl(\"numpadenter\");");


bindCommand(keyboard0, make, control, "numpad1", TO, "sendControl(\"numpad1\", \"control\");");
bindCommand(keyboard0, make, control, "numpad2", TO, "sendControl(\"numpad2\", \"control\");");
bindCommand(keyboard0, make, control, "numpad3", TO, "sendControl(\"numpad3\", \"control\");");
bindCommand(keyboard0, make, control, "numpad4", TO, "sendControl(\"numpad4\", \"control\");");
bindCommand(keyboard0, make, control, "numpad5", TO, "sendControl(\"numpad5\", \"control\");");
bindCommand(keyboard0, make, control, "numpad6", TO, "sendControl(\"numpad6\", \"control\");");
bindCommand(keyboard0, make, control, "numpad7", TO, "sendControl(\"numpad7\", \"control\");");
bindCommand(keyboard0, make, control, "numpad8", TO, "sendControl(\"numpad8\", \"control\");");
bindCommand(keyboard0, make, control, "numpad9", TO, "sendControl(\"numpad9\", \"control\");");
bindCommand(keyboard0, make, control, "numpad0", TO, "sendControl(\"numpad0\", \"control\");");
bindCommand(keyboard0, make, control, "numpad+", TO, "sendControl(\"numpad+\", \"control\");");
bindCommand(keyboard0, make, control, "numpad-", TO, "sendControl(\"numpad-\", \"control\");");
bindCommand(keyboard0, make, control, "numpad*", TO, "sendControl(\"numpad*\", \"control\");");
bindCommand(keyboard0, make, control, "numpad/", TO, "sendControl(\"numpad/\", \"control\");");
bindCommand(keyboard0, make, control, "numpadenter", TO, "sendControl(\"numpadenter\", \"control\");");


bindCommand(keyboard0, make, shift, "numpad1", TO, "sendControl(\"numpad1\", \"shift\");");
bindCommand(keyboard0, make, shift, "numpad2", TO, "sendControl(\"numpad2\", \"shift\");");
bindCommand(keyboard0, make, shift, "numpad3", TO, "sendControl(\"numpad3\", \"shift\");");
bindCommand(keyboard0, make, shift, "numpad4", TO, "sendControl(\"numpad4\", \"shift\");");
bindCommand(keyboard0, make, shift, "numpad5", TO, "sendControl(\"numpad5\", \"shift\");");
bindCommand(keyboard0, make, shift, "numpad6", TO, "sendControl(\"numpad6\", \"shift\");");
bindCommand(keyboard0, make, shift, "numpad7", TO, "sendControl(\"numpad7\", \"shift\");");
bindCommand(keyboard0, make, shift, "numpad8", TO, "sendControl(\"numpad8\", \"shift\");");
bindCommand(keyboard0, make, shift, "numpad9", TO, "sendControl(\"numpad9\", \"shift\");");
bindCommand(keyboard0, make, shift, "numpad0", TO, "sendControl(\"numpad0\", \"shift\");");
bindCommand(keyboard0, make, shift, "numpad+", TO, "sendControl(\"numpad+\", \"shift\");");
bindCommand(keyboard0, make, shift, "numpad-", TO, "sendControl(\"numpad-\", \"shift\");");
bindCommand(keyboard0, make, shift, "numpad*", TO, "sendControl(\"numpad*\", \"shift\");");
bindCommand(keyboard0, make, shift, "numpad/", TO, "sendControl(\"numpad/\", \"shift\");");
bindCommand(keyboard0, make, shift, "numpadenter", TO, "sendControl(\"numpadenter\", \"shift\");");


bindCommand(keyboard0, make, alt, "numpad1", TO, "sendControl(\"numpad1\", \"alt\");");
bindCommand(keyboard0, make, alt, "numpad2", TO, "sendControl(\"numpad2\", \"alt\");");
bindCommand(keyboard0, make, alt, "numpad3", TO, "sendControl(\"numpad3\", \"alt\");");
bindCommand(keyboard0, make, alt, "numpad4", TO, "sendControl(\"numpad4\", \"alt\");");
bindCommand(keyboard0, make, alt, "numpad5", TO, "sendControl(\"numpad5\", \"alt\");");
bindCommand(keyboard0, make, alt, "numpad6", TO, "sendControl(\"numpad6\", \"alt\");");
bindCommand(keyboard0, make, alt, "numpad7", TO, "sendControl(\"numpad7\", \"alt\");");
bindCommand(keyboard0, make, alt, "numpad8", TO, "sendControl(\"numpad8\", \"alt\");");
bindCommand(keyboard0, make, alt, "numpad9", TO, "sendControl(\"numpad9\", \"alt\");");
bindCommand(keyboard0, make, alt, "numpad0", TO, "sendControl(\"numpad0\", \"alt\");");
bindCommand(keyboard0, make, alt, "numpad+", TO, "sendControl(\"numpad+\", \"alt\");");
bindCommand(keyboard0, make, alt, "numpad-", TO, "sendControl(\"numpad-\", \"alt\");");
bindCommand(keyboard0, make, alt, "numpad*", TO, "sendControl(\"numpad*\", \"alt\");");
bindCommand(keyboard0, make, alt, "numpad/", TO, "sendControl(\"numpad/\", \"alt\");");
//No alt+enter! toggles fullscreen!


//Following 3 sets added in Repack 6
bindCommand(keyboard0, make, "f1", TO, "sendControl(\"f1\");");
bindCommand(keyboard0, make, "f2", TO, "sendControl(\"f2\");");
bindCommand(keyboard0, make, "f3", TO, "sendControl(\"f3\");");
bindCommand(keyboard0, make, "f4", TO, "sendControl(\"f4\");");
bindCommand(keyboard0, make, "f5", TO, "sendControl(\"f5\");");
bindCommand(keyboard0, make, "f6", TO, "sendControl(\"f6\");");
bindCommand(keyboard0, make, "f7", TO, "sendControl(\"f7\");");
bindCommand(keyboard0, make, "f8", TO, "sendControl(\"f8\");");
bindCommand(keyboard0, make, "f9", TO, "sendControl(\"f9\");");
bindCommand(keyboard0, make, "f10", TO, "sendControl(\"f10\");");
bindCommand(keyboard0, make, "f11", TO, "sendControl(\"f11\");");
bindCommand(keyboard0, make, "f12", TO, "sendControl(\"f12\");");


bindCommand(keyboard0, make, control, "1", TO, "sendControl(\"1\", \"control\");");
bindCommand(keyboard0, make, control, "2", TO, "sendControl(\"2\", \"control\");");
bindCommand(keyboard0, make, control, "3", TO, "sendControl(\"3\", \"control\");");
bindCommand(keyboard0, make, control, "4", TO, "sendControl(\"4\", \"control\");");
bindCommand(keyboard0, make, control, "5", TO, "sendControl(\"5\", \"control\");");
bindCommand(keyboard0, make, control, "6", TO, "sendControl(\"6\", \"control\");");
bindCommand(keyboard0, make, control, "7", TO, "sendControl(\"7\", \"control\");");
bindCommand(keyboard0, make, control, "8", TO, "sendControl(\"8\", \"control\");");
bindCommand(keyboard0, make, control, "9", TO, "sendControl(\"9\", \"control\");");
bindCommand(keyboard0, make, control, "0", TO, "sendControl(\"0\", \"control\");");


bindCommand(keyboard0, make, alt, "1", TO, "sendControl(\"1\", \"alt\");");
bindCommand(keyboard0, make, alt, "2", TO, "sendControl(\"2\", \"alt\");");
bindCommand(keyboard0, make, alt, "3", TO, "sendControl(\"3\", \"alt\");");
bindCommand(keyboard0, make, alt, "4", TO, "sendControl(\"4\", \"alt\");");
bindCommand(keyboard0, make, alt, "5", TO, "sendControl(\"5\", \"alt\");");
bindCommand(keyboard0, make, alt, "6", TO, "sendControl(\"6\", \"alt\");");
bindCommand(keyboard0, make, alt, "7", TO, "sendControl(\"7\", \"alt\");");
bindCommand(keyboard0, make, alt, "8", TO, "sendControl(\"8\", \"alt\");");
bindCommand(keyboard0, make, alt, "9", TO, "sendControl(\"9\", \"alt\");");
bindCommand(keyboard0, make, alt, "0", TO, "sendControl(\"0\", \"alt\");");