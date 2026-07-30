// ChatHudEditor v2.0 by |HH|BigBunny (Apollo)
// Read the help for more information on this script... sorry :)
//
// Made for 1.40 by DaJ4ck3L
// www.thelandofoz.net

//to edit guis command is guieditor(); -DaJ4ck3L
//make sure to type this in game in the console
//you can use this function to make the chatbox longer
function guiEditor() {
	//winMouse();
	GuiInspect(MainWindow);
	GuiToolbar(MainWindow);
	GuiEditMode(MainWindow);
	cursorOn(MainWindow);
	tree();
}

echo("Executing - ChatHudEditor v2.0");
Event::Attach(eventExit, ChatHudEditor::exit);

if (isFile("config\\ChatHudEditorPrefs.cs"))
	include("ChatHudEditorPrefs.cs");
else // setDefaults
{
	$ChatHudEditorPref::Size[1] = 3;
	$ChatHudEditorPref::Size[2] = 6;
	$ChatHudEditorPref::Size[3] = 20;
	$ChatHudEditorPref::counter = 1;
}

function ChatHudEditor::exit()
{
	export("$ChatHudEditorPref::*", "config\\ChatHudEditorPrefs.cs", false);	
}

function ChatHudEditor::changeSize()
{
	$ChatHudEditorPref::counter++;
	if ($ChatHudEditorPref::counter > 3) $ChatHudEditorPref::counter = 1;
	EditActionMap("actionMap.sae");
	bindAction(keyboard0, make, "u", TO, IDACTION_CHAT_DISP_SIZE, $ChatHudEditorPref::Size[$ChatHudEditorPref::counter]);
}

//bind this badboy
editActionMap("playMap.sae");
bindAction(keyboard0, make, "u", TO, IDACTION_CHAT_DISP_SIZE, $ChatHudEditorPref::Size[$ChatHudEditorPref::counter]);
bindCommand(keyboard0, break,  "u", TO, "ChatHudEditor::changeSize();");

Event::Attach(eventExit, ChatHudEditor::exit);