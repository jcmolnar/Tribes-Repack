// requires chatInput.dll & gamebinds.cs 
// resets chatinput location to underneath chatdisplay everytime you hit T or Y
// remember to rebind your T and Y keys in the binds menu if you add/remove this script!

// gamebinds.cs stuff: remove default binding and add new ones
// old ways either hardcode the keybind or leave the old keybind in the menu

GameBinds::ignore( "actionMap.sae", "Global Chat", __FILE__ ~ " is better");
GameBinds::ignore( "actionMap.sae", "Team Chat", __FILE__ ~ " is better");

function chatInput::addBindsToMenu() after GameBinds::Init
{
	GameBinds::allow( "actionMap.sae", "Global Chat", "now its ok?" );
	GameBinds::allow( "actionMap.sae", "Team Chat", "i dont know" );
	GameBinds::SetMapNoClearBinds( "actionMap.sae" );
	GameBinds::addBindCommand( "Global Chat", "sayChat(everyone);", "" );
	GameBinds::addBindCommand( "Team Chat", "sayChat(team);", "" );
}		

function sayChat(%who) {
	// set the y location 
	$pref::ChatInputY = getWord(Control::GetPosition(ChatDisplayHUD),1) + getWord(Control::GetExtent(ChatDisplayHUD),1) + 5;
	
	// if the chatdisplay is too low to show input below it, move to on top instead
	if ( ($pref::ChatInputY + 35) > getWord(Control::GetExtent(PlayGui),1) ) $pref::ChatInputY = getWord(Control::GetPosition(ChatDisplayHUD),1) - 40;
	
	// set the X location too! only works if $pref::ChatInputModMethodX = 1
	$pref::ChatInputX = getWord(Control::GetPosition(ChatDisplayHUD),0) ;
	
	// now send the actual chat commands
	switch(%who){
		case "everyone":
			postAction(nameToId("SimGui::PlayDelegate"), IDACTION_CHAT, 0);
			break;
		case "team":
			postAction(nameToId("SimGui::PlayDelegate"), IDACTION_CHAT, 1);
			break;
		default:
			echoc(2,"specify 'everyone' or 'team' ");
			break;
	}	
}