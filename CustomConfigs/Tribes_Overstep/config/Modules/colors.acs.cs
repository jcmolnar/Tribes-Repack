// Grooves NON-ScriptGL config color style changer & keybind
// updated 2016, cleaned and remoteEP
// removing this file will lock huds to the default Black style ?

$Colorchange::Color[1] = "Grey";
$Colorchange::Color[2] = "Dark";
$Colorchange::Color[3] = "Green";
$Colorchange::Color[4] = "BE Red";
$Colorchange::Color[5] = "DS Blue";
$Colorchange::currentstyle = 1;

// increment counter, loop at 5, set pref, remoteEP style name, apply the style
function Colorchange::styleup()	{
	$Colorchange::currentstyle += 1;
	if ($Colorchange::currentstyle > 5) $Colorchange::currentstyle = 1;
	$pref::vhud::Background::style = $Colorchange::Color[$Colorchange::currentstyle];
	remoteEP("<L5>HUD Style set to <f2>" @ $Colorchange::Color[$Colorchange::currentstyle] @ ".", 5, 1,1, 24, 300);
	Colorchange::applystyle();
	}

// apply the texture settings changes for the various styles
function Colorchange::applystyle()
{
	switch($pref::vhud::Background::style)
	{
		case "Grey":
			Colorchange::default(); 
			break;
		case "DS Blue":
			// 11 = TimeTextColor. default is 255,255,255
			groovcolor(11,255,255,255);
			// 13 = All Scripted HUDS backdrop color (ctfhud etc). default for all the backdrops is 1,1,1
			groovcolor(13,40,80,200);
			// 14 = ChatHUD backdrop color
			groovcolor(14,40,80,200);
			// 15 = TimeHUD backdrop color
			groovcolor(15,40,80,200);
			// 20 = MiniMap grey background. default is 1,1,1
			groovcolor(20,40,80,200);
			break;
		case "Green":
			groovcolor(11,255,255,255);
			groovcolor(13,80,200,80);
			groovcolor(14,80,200,80);
			groovcolor(15,80,200,80);
			groovcolor(20,80,200,80);
			break;
		case "BE Red":
			groovcolor(11,255,255,255);
			groovcolor(13,200,80,80);
			groovcolor(14,200,80,80);
			groovcolor(15,200,80,80);
			groovcolor(20,200,80,80);
			break;
		case "Dark":
			groovcolor(11,255,255,255);
			groovcolor(13,1,1,1);
			groovcolor(14,1,1,1);
			groovcolor(15,1,1,1);
			groovcolor(20,1,1,1);
			break;
		default:
			Colorchange::default();
			break;
	}
}

function Colorchange::default() {
	echoc(2,"loading color style defaults");
	// 11 = TimeTextColor. default is 255,255,255
	groovcolor(11,255,255,255);
	// 13 = All Scripted HUDS backdrop color (ctfhud etc). default for all the backdrops is 1,1,1
	groovcolor(13,220,220,220);
	// 14 = ChatHUD backdrop color
	groovcolor(14,220,220,220);
	// 15 = TimeHUD backdrop color
	groovcolor(15,220,220,220);
	// 20 = MiniMap grey background. default is 1,1,1
	groovcolor(20,220,220,220);
	}

//hack to insert binds into the menu
function Colorchange::addBindsToMenu() after GameBinds::Init
{
	$GameBinds::CurrentMap = "actionMap.sae";
	$GameBinds::CurrentMapHandle = GameBinds::GetActionMap2( "actionMap.sae" );
	GameBinds::addBindCommand( "Change HUD Style", "Colorchange::styleup();", "" );
}

//Event::attach(eventGuiOpen, Colorchange::applystyle);
//Event::attach(eventGuiOpen, Schedule::Add("Colorchange::applystyle",2) );