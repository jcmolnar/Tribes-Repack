//-------------------------------------------------------------------
// mission editor specific configuration file
//-------------------------------------------------------------------


//-------------------------------------------------------------------
// these functions act like hotkeys in ted for switching between
// mouse functions for a brief moment
//-------------------------------------------------------------------
function pushMouseAction(%action)
{
   $prevMouseAction = Ted::getActionName(Ted::getLButtonActionIndex());
   Ted::setLButtonAction(%action);
}

function popMouseAction()
{
   Ted::setLButtonAction($prevMouseAction);
}

echo("RPG editorconfig is being run.");
bindCommand(keyboard, make, "t", to, "pushMouseAction(select);");
bindCommand(keyboard, break, "t", to, "popMouseAction();");

bindCommand(keyboard, make, "g", to, "pushMouseAction(deselect);");
bindCommand(keyboard, break, "g", to, "popMouseAction();");

bindCommand(keyboard, make, "b", to, "pushMouseAction(smooth);");
bindCommand(keyboard, break, "b", to, "popMouseAction();");

bindCommand(keyboard, make, "n", to, "pushMouseAction(lowerHeight);");
bindCommand(keyboard, break, "n", to, "popMouseAction();");
bindCommand(keyboard, make, "m", to, "pushMouseAction(raiseHeight);");
bindCommand(keyboard, break, "m", to, "popMouseAction();");

bindCommand(keyboard, make, "h", to, "pushMouseAction(depress);");
bindCommand(keyboard, break, "h", to, "popMouseAction();");
bindCommand(keyboard, make, "j", to, "pushMouseAction(elevate);");
bindCommand(keyboard, break, "j", to, "popMouseAction();");

bindCommand(keyboard, make, "l", to, "pushMouseAction(setHeight);");
bindCommand(keyboard, break, "l", to, "popMouseAction();");

function autosave(%m)
{
	echo("attempting to AutoSave... (use $stopautosave = True to stop autosave)");
	if(!$stopautosave)
	{
		focusServer();
		saveMission(8251, "base\\MISSIONS\\" @ $missionName @ ".mis");
		focusClient();
		schedule("autosave(" @ %m @ ");", %m * 60);
	}
}