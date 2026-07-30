// Script: Volume.cs
// Author: powdahound & CyNiC
// Website: http://hosted.tribalwar.com/powdahound http://www.CyNiC.ws
// Email: powdahound@gamer-insight.com CyNiC@CyNiC.ws
// Date: 5-31-2002 Edited On 11-15-2002
// Function: Cycle between different volume levels in-game.
// Comments: Edited By CyNiC To Remove A "lame array workaround", More Editing By CyNiC to Tighten Code A LOT
//			 He edited all the variables and functions to his name too!!! oh well :p -powda
// Version: 2.0 Edited to 3.0?
EditActionMap("playMap.sae");

// =============
//  Preferences
// =============

// main binds
bindCommand(keyboard0, make, "numpad-", TO, "cynic::volume(-$cynic::volinc);");
bindCommand(keyboard0, make, "numpad+", TO, "cynic::volume($cynic::volinc);");

// uncomment these lines if you want to bind it to your mouse wheel (VERY COOL!!!) :p
//   note tribes can't tell the difference between scroll up and scroll down so they have to be the same thing :(
//bindCommand(mouse0, zaxis0, TO, "cynic::volume($cynic::volinc);");  
//bindCommand(mouse0, zaxis1, TO, "cynic::volume($cynic::volinc);");

$cynic::currentvol = 30;	// if you want to start at a different percentage, change this (default is '50' which is 50%)
$cynic::volinc = 5;		// if you want to increase the volume by a different percentage, change this (default is '10' which is 10%)

function cynic::volume(%inc)
{
	$cynic::currentvol = $cynic::currentvol+%inc; 							//change volume

	if ($cynic::currentvol < 0)
		$cynic::currentvol = 100; 											//if below 0% then loop around to 100%
	if ($cynic::currentvol > 100)
		$cynic::currentvol = 0; 											//if above 100% then loop around to 0%

	remoteBP(2048, "<jc><L5>Volume: <f2>" @ $cynic::currentvol @ "%", 5); 	// output volume

	$pref::sfx3dVolume = $cynic::currentvol/100; 							//set the 3d volume
	$pref::sfx2dVolume = $cynic::currentvol/100; 							//set the 2d volume
}