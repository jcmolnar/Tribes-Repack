// modified to add more details to popup
// -g

// requires remoteEP

$NetSet::interval = 4;

function NetSet::GameBinds::Init()
  after GameBinds::Init
{
	$GameBinds::CurrentMapHandle = GameBinds::GetActionMap2( "actionMap.sae");
	$GameBinds::CurrentMap = "actionMap.sae";
	GameBinds::addBindCommand( "Decrease Interpolate", "Interpolate::set(0);");
	GameBinds::addBindCommand( "Increase Interpolate", "Interpolate::set(1);");
	GameBinds::addBindCommand( "Decrease PFT", "PFT::set(0);");
	GameBinds::addBindCommand( "Increase PFT", "PFT::set(1);");
}



// CODE BELOW DON'T EDIT

function Interpolate::set(%x)
{
	%currentterp = $net::interpolateTime;

	if (%x==1) {
		if ($net::interpolateTime < 360) {
			%currentterp = %currentterp + $NetSet::interval;
		}
	}
	if (%x==0) {
		if (%currentterp > 0) {
			%currentterp = %currentterp - $NetSet::interval;
			$net::interpolateTime = %currentterp;
		}
	}

	$net::interpolateTime = %currentterp;
	NetSetCheck::Popup();
}

function PFT::set(%x)
{
	%currentpft = $net::predictForwardTime;

	if (%x==1) {
		if ($net::predictForwardTime < 360) {
			%currentpft = %currentpft + $NetSet::interval;
		}
	}
	if (%x==0) {
		if (%currentpft > 0) {
			%currentpft = %currentpft - $NetSet::interval;
			$net::predictForwardTime = %currentpft;
		}
	}

	$net::predictForwardTime = %currentpft;
	NetSetCheck::Popup();
}

function setNetSet()
{
	Interpolate::set($net::interpolateTime);
        PFT::set($net::predictForwardTime);
}

function NetSetCheck::Popup() { 
	if($LoaderPlugin::netset)
		%netsetcheck = "<jc><F1>NetSet?: <F2>Loaded";
	else
		%netsetcheck = "<jc><F1>NetSet?: <F2>Not Found";
	
	%terp = "\n<F1>Interpolate: <F2>"~$net::interpolateTime;
	%pft = "\n<F1>PFT: <F2>"~$net::predictForwardTime;
	%method = "\n<F1>Method: <F2>" ~ $net::predictForwardTimeMethod;
	
	remoteEP(%netsetcheck~%terp~%pft~%method,3,true,4,21,360);
}

function NetWriteInGamePrefs()
{
	echo("exporting $net in-game prefs...");
	export("$net::*", "config\\core\\netset.cs", false);
}

event::Attach(eventExit, NetWriteInGamePrefs);
event::Attach(eventConnected, setNetSet);