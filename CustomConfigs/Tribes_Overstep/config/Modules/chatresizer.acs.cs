// chathud "U" over-ride for Tribes 1.40+
// this version requires gamebinds.cs

GameBinds::ignore( "actionMap.sae", "Chat HUD Size", __FILE__ ~ " is better");
function chatresizer::addBindsToMenu() after GameBinds::Init
{
	GameBinds::SetMapNoClearBinds( "actionMap.sae" );
	GameBinds::addBindCommand( "ChatHud Size + 4", "ChatResizer::changeSize();", "" );
}

function ChatResizer::changeSize()
{
	$pref::ChatHud::Lines = $pref::ChatHud::Lines +3;
	if ($pref::ChatHud::Lines > 12) $pref::ChatHud::Lines = 3;
	postAction(nameToId("SimGui::PlayDelegate"), IDACTION_CHAT_DISP_SIZE, $pref::ChatHud::Lines);
}