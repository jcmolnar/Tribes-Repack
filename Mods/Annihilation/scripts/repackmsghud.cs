//This file is part of Tribes RPG.
//Tribes RPG client side scripts
//Repack RPG additions written by Jason "phantom" Daley, tribesrpg.org

//	Copyright (C) 2016  Jason Daley

//	This program is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.

//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.

//	You should have received a copy of the GNU General Public License
//	along with this program.  If not, see <http://www.gnu.org/licenses/>.

//You may contact the author at beatme101@gmail.com or www.tribesrpg.org/contact.php

//This GPL does not apply to Starsiege: Tribes or any non-RPG related files included.
//Starsiege: Tribes, including the engine, retains a proprietary license forbidding resale.


//=====================================================
//HUD-Console Print
//Written by phantom - tribesrpg.org
//And written by rj - crurpg.org
//This function is designed for high frequency messages,
//such as damage messages in battles.
//It can handle 255 chars per input string, and has a
//variable number of supported lines.
//Messages are sent and handled similarly to the Tribes
//chat hud.
//Note: for now, not supporting size changing.
//Colours available, including extended, in order:
//Orange, beige, white, green, blue, pink
//
// End User Functions:
// remoteHUDConsolePrint(%server, %msg, %timeout) server sends messages through here. %timeout decides how long hud is visible.
// remoteRPGMsgHudPos(%server,%pos, %show) to reposition the hud, %pos takes a space-separated set of coordinates range 0-1
// .. and %show is used to decide if the hud should pop up
// remoteDisableRPGMsghud(%server, %hideOrUnload) allows hiding the hud immediately, as well as clearing the text.
//======================================================



//=====================================================
//Recharge bar HUD
//Written by phantom - tribesrpg.org
//The bar will display for as long as it takes to recharge,
//wait 3 seconds, and then disappear.
//Colours: Orange Beige White Green SkyBlue Pink Black Red Yellow Blue Grey Grey2
//           0      1     2     3     4      5     6    7    8     9    10   11
//Barnum lets you have more than one bar.
//Addtext lets you write a bit of text above the bar.
//
// End User Functions:
// remoterpgbarhud(%server,%recovTime, %colour, %ecolour, %chars, %barnum, %addtext)
//=====================================================

//Everything that follows probably shouldn't be touched

// Position of the Hud. Percent (0 to 1)
$rpghudposx = 1.0;
$rpghudposy = 1.0;

// Number of lines for each size setting.
$rpgmsghudlarge = 18;
$rpgmsghudmedium = 10;
$rpgmsghudsmall = 5;

$rpgbathudwidth = 375;
$rpgbarhudwidth = 400;//best not to mess with this, for now, it only affects the positioning

//==========================================================================================================

$rpghudlines = $rpgmsghudmedium;
function remoteHUDConsolePrint(%server, %msg, %timeout) {
	if(%server != 2048)
		return;
	//%max = floor(%max);//For now, not supporting size changing
	if(!$repackmsghudShowing)
		rpgmsghud::init();
	rpgmsghud::msg(%msg);
	$HUDPrintId++;
	schedule("clearrpgmsghud(" @ $HUDPrintId @ ");", %timeout);
}

function clearrpgmsghud(%id)
{
	if(%id == $HUDPrintId){
		if(!$keepHudVisible)
			rpgmsghud::reset();
	}
}

function remoteRPGMsgHudPos(%server,%pos, %show){
	if(string::compare(%pos, "") != 0){
		$rpghudposx = getWord(%pos,0);
		$rpghudposy = getWord(%pos,1);
	}

	if($repackmsghudShowing || %show){
		if(!$repackmsghudShowing)
			rpgmsghud::init();
		else {
			if(!$rpgmsghudEnabled){
				$rpgmsghudon = 1;
				$rpgmsgpage = 1;
			}
			rpgmsghud::load();
		}
		%on = $rpgmsghudon;
		rpgmsghud::updatehud(%on - 1);	
	}
	if(%show & 1)
		$keepHudVisible = %show;
}

function cleanuprepackhuds(){
	remoteDisableRPGMsghud(2048, True);
	rpgbarhud::reset("-1");
	urlhud::reset();
	centertip::reset();
}

function remoteDisableRPGMsghud(%server, %hideOrUnload)
{
	if (%server != 2048)
		return;
	if(!%hideOrUnload)
		rpgmsghud::reset();
	else
		rpgmsghud::unload();
	$keepHudVisible = 0;
}

function rpgmsghud::init()
{
	if(!$rpgmsghudEnabled){
		$rpgmsghudon = 1;
		$rpgmsgpage = 1;
	}
	$rpgmsgpageup = false;
	if (!$repackmsghudShowing){
		//$rpghudlines = rpgmsghud::count();
		rpgmsghud::load();
	}
}

//function remoteRPGMsgHud(%server,%msg,%color)
//{
//	if (%server != 2048)
//		return;
//	rpgmsghud::msg(%msg,%color);
//}

//function remoteInitRPGMsgHud(%server)
//{
//	if (%server != 2048)
//		return;
//	rpgmsghud::init(true);
//	remoteeval(2048,EnableRPGMsgHud);
//}
//rpgmsghud::init();

function rpgmsghud::charwidth(%v)
{
	%c = charCode(%v);

	return $charWidth[%c]+1;
}

function rpgmsghud::msg(%msg)
{
	%maxwidth = $rpgbathudwidth;
	%t = 0;
	%o = 0;
	%msg[%o] = "";
	for (%i = 0; (%g = getWord(%msg,%i)) != -1; %i++) {		
		%c = 0;
		%tempg = %g;
		%tempg = string::replaceall(%tempg,"<f0>","");
		%tempg = string::replaceall(%tempg,"<f1>","");
		%tempg = string::replaceall(%tempg,"<f2>","");
		for (%x = 0; (%v = string::getsubstr(%tempg,%x,1)) != ""; %x++)
			%c += rpgmsghud::charwidth(%v);
		if (((%c + 4) + %t) <= %maxwidth) {
			%t += (%c + 4);
			%msg[%o] = %msg[%o] @ %g @ " ";
		}
		else {
			if(%c > %maxwidth) return;
			%o++;
			%msg[%o] = %color @ " " @ %g @ " ";
			%t = (%c + 4);
		}
	}
	for (%i = 0; (%m = %msg[%i]) != ""; %i++)
		rpgmsghud::addmsg(%msg[%i]);
}

function rpgmsghud::addmsg(%msg)
{
	%on = $rpgmsghudon;
	$rpgmsg[%on] = %msg;
	if ($rpgmsgpageup == false) {
		$rpgmsgpage = $rpgmsghudon;
		rpgmsghud::updatehud(%on);
	}
	$rpgmsghudon++;	
}

function rpgmsghud::resize()
{
	%lines = $rpghudlines;
	if (%lines > $rpgmsghudmedium)
		$rpghudlines = $rpgmsghudsmall;
	else if (%lines == $rpgmsghudsmall)
		$rpghudlines = $rpgmsghudmedium;
	else
		$rpghudlines = $rpgmsghudlarge;
	//rpgmsghud::reset();
	rpgmsghud::load();
	%on = $rpgmsghudon;
	$rpgmsgpage = %on;
	$rpgmsgpageup = false;
	rpgmsghud::updatehud(%on - 1);	
}

function rpgmsghud::pageup()
{
	$rpgmsgpageup = true;
	control::setVisible("rpgchathud_button",true);
	%page = $rpgmsgpage - $rpghudlines;
	if (%page < 1) {
		%page = 1;
		return;
	}
	rpgmsghud::updatehud(%page-1);
	$rpgmsgpage = %page;
}

function rpgmsghud::pagedown()
{
	%page = $rpgmsgpage + $rpghudlines;
	if (%page >= $rpgmsghudon) {
		%page = $rpgmsghudon;
		$rpgmsgpageup = false;
		control::setVisible("rpgchathud_button",false);
	}
	rpgmsghud::updatehud(%page-1);
	$rpgmsgpage = %page;
}

function rpgmsghud::updatehud(%on)
{
	control::setValue("rpgchathud_"@$rpghudlines,$rpgmsg[%on]);
	for (%i = ($rpghudlines-1); %i >= 1; %i--) {
		%on--;
		control::setValue("rpgchathud_"@%i,$rpgmsg[%on]);
	}
}

function rpgmsghud::load()
{
	rpgmsghud::reset();
	%res = Repack::ScreenSize();
	%x = getWord(%res,0);
	%x -= $rpgbathudwidth;
	%posx = %x * $rpghudposx;
	//%posx += ($rpgbathudwidth/2);


	%y = getWord(%res,1);
	%height = floor($rpghudlines * 15.5);
	%y -= %height;
	%posy = %y * $rpghudposy;
	$rpghud[0] = newObject("rpgchathud_frame", FearGui::FearGuiMenu, %posx, %posy, $rpgbathudwidth, %height);
	%v = 0;
	for (%i = 1; %i <= $rpghudlines; %i++) {
		$rpghud[%i] = newObject("rpgchathud_"@%i, FearGuiFormattedText,%posx, %posy + %v, 15, 100);
		%v += 15;
		//control::setValue("rpgchathud_"@%i,$rpgmsg[%i]);
	}
	for (%i = 0; %i <= $rpghudlines; %i++)
		addToSet(PlayGui, $rpghud[%i]);

	%on = $rpgmsghudon;
	$rpgmsgpage = $rpgmsghudon;
	rpgmsghud::updatehud(%on);

	$repackmsghudShowing = true;
	$rpgmsghudEnabled = true;

//	$rpghudbutton = newObject("rpgchathud_button", FearGuiFormattedText,$rpghudposx + 350, ($rpghudposy + floor(($rpghudlines - 1) * 15.5)), 100, 100);
//	control::setValue("rpgchathud_button","<BHC_Down.bmp>");
//	control::setVisible("rpgchathud_button",false);
//	addToSet(PlayGui, $rpghudbutton);
}

function rpgmsghud::count()
{
	%c = -2;
	%ngs = nameToId(NamedGuiSet);
	%len = Group::objectCount(%ngs);
	for(%i = 0; %i < %len; %i++) {
 		%obj = Group::getObject(%ngs, %i);
  		%objectName = Object::getName(%obj);
  		if (string::FindSubStr(%objectname,"rpgchathud_") != -1)
		%c++;
	}
	return %c;
}

function rpgmsghud::reset()
{
	$repackmsghudShowing = false;
	%ngs = nameToId(NamedGuiSet);
	%len = Group::objectCount(%ngs);
	for(%i = 0; %i < %len; %i++) {
 		%obj = Group::getObject(%ngs, %i);
  		%objectName = Object::getName(%obj);
  		if (string::FindSubStr(%objectname,"rpgchathud_") != -1)
    			%list = %list @ %obj @ " ";
  	}
	for (%i = 0; (%g = getWord(%list,%i)) != -1; %i++)
		deleteObject(%g);
}
function rpgmsghud::unload()
{
	//for (%i = 0; %i <= $rpghudlines; %i++)
	for (%i = 0; $rpghud[%i] != ""; %i++){
		deleteObject($rpghud[%i]);
		$rpghud[%i] = "";
	}
	if($rpghudbutton > 0){
		deleteObject($rpghudbutton);
		$rpghudbutton = "";
	}
		$rpgmsghudon = 1;
		$rpgmsgpage = 1;
	$repackmsghudShowing = false;
}

function rpgbarhud::reset(%barnum){
	%ngs = nameToId(NamedGuiSet);
	%len = Group::objectCount(%ngs);
	for(%i = 0; %i < %len; %i++) {
 		%obj = Group::getObject(%ngs, %i);
  		%objectName = Object::getName(%obj);
		if(%barnum == -1){
			if(string::FindSubStr(%objectname,"rpgbarhud_") != -1)
				%list = %list @ %obj @ " ";
		}
  		else if (string::compare(%objectname,"rpgbarhud_"@%barnum) == 0 || string::compare(%objectname,"rpgbarhud_"@(%barnum+1)) == 0)
    			%list = %list @ %obj @ " ";
  	}
	for (%i = 0; (%g = getWord(%list,%i)) != -1; %i++)
		deleteObject(%g);
	if(%barnum == -1){
		for(%i = 0;%i < 10;%i+=2)
			schedule::cancel("rpgbarhud"@%i);
	}
	else
		schedule::cancel("rpgbarhud"@%barnum);
}
function rpgbarhud::load(%recovTime, %c, %ec, %chars, %barnum, %addtext){
	%barnum = %barnum * 2 + 2;
	rpgbarhud::reset(%barnum);
	%hudwidth = $rpgbarhudwidth;
	%res = Repack::ScreenSize();
	%x = getWord(%res,0);
	%x -= %hudwidth;
	%posx = %x * 0.5;

	%y = getWord(%res,1);
	%height = 26 * (%barnum-1);
	%y -= %height;
	%posy = %y;
	%object = newObject("rpgbarhud_"@%barnum, FearGuiFormattedText,%posx, %posy, 15, 100);
	addToSet(PlayGui, %object);
	if(%addtext != ""){
		%posy -= 15;
		%bn = (%barnum+1);
		%object = newObject("rpgbarhud_"@%bn, FearGuiFormattedText,%posx, %posy, 15, 100);
		addToSet(PlayGui, %object);
		control::setValue("rpgbarhud_"@%bn,%addtext);
	}
	%sch = string::getsubstr(%chars,0,1);
	%ech = string::getsubstr(%chars,1,1);
	%s_charwidth = rpgmsghud::charwidth(%sch)-1;
	%e_charwidth = rpgmsghud::charwidth(%ech)-1;
	%sch = string::printcolorbar(%c, %sch);
	%ech = string::printcolorbar(%ec, %ech);

	if(%chars == "||")
		rpgbarhud2::frame(0, %recovTime, %sch, %ech, %barnum);
	else
		rpgbarhud::frame(0, %recovTime, %s_charwidth, %e_charwidth, %sch, %ech, %barnum);
}
function rpgbarhud::frame(%state, %recovTime, %scw, %ecw, %sch, %ech, %barnum){
	%pct = floor((%state / %recovTime) * 400)+1;
	if(%pct > 400)
		%pct = 400;
	%remainder = 400 - %pct;
	if(%pct < 400){
		%speed = 0.065;
		%state += %speed;
		if(%state > %recovTime)
			%state = %recovTime;
		schedule::addf("rpgbarhud::frame("@%state@", "@%recovTime@","@%scw@","@%ecw@", \"" @ %sch @ "\", \"" @ %ech @ "\", " @ %barnum @ ");",%speed, "rpgbarhud"@%barnum);
	}
	else {
		schedule::addf("rpgbarhud::reset("@%barnum@");",3, "rpgbarhud"@%barnum);
	}
	%sch_width = floor(%pct / %scw);
	%msg = String::create(%sch, %sch_width);
	%ech_width = floor(%remainder / %ecw);
	%msg = %msg @ String::create(%ech, %ech_width);
	control::setValue("rpgbarhud_"@%barnum,%msg);

}
function rpgbarhud2::frame(%state, %recovTime, %sch, %ech, %barnum){
	%pct = floor((%state / %recovTime) * 400)+1;
	if(%pct > 400)
		%pct = 400;
	%remainder = 400 - %pct;
	if(%pct < 400){
		%speed = 0.055;
		%state += %speed;
		if(%state > %recovTime)
			%state = %recovTime;
		schedule::addf("rpgbarhud2::frame("@%state@", "@%recovTime@", \"" @ %sch @ "\", \"" @ %ech @ "\", " @ %barnum @ ");",%speed, "rpgbarhud"@%barnum);
	}
	else {
		schedule::addf("rpgbarhud::reset("@%barnum@");",3, "rpgbarhud"@%barnum);
	}
	%sch_width = floor(%pct);
	%msg = String::create(%sch, %sch_width);
	%ech_width = floor(%remainder);
	%msg = %msg @ String::create(%ech, %ech_width);
	control::setValue("rpgbarhud_"@%barnum,%msg);

}
//Feature added in repack 33
//It should be okay to use any two chars, but it'll look pretty weird for many that aren't the correct width
//Recommend just using "||" for chars
//%barnum and %addtext added in repack 34
function remoterpgbarhud(%server,%recovTime, %colour, %ecolour, %chars, %barnum, %addtext){
	if(%server != 2048)
		return;
	%color = floor(%colour);
	%ecolour = floor(%ecolour);
	%barnum = floor(%barnum);
	%recovTime += 1.0;
	%recovTime -= 1.0;
	if(%recovTime < 0.1)
		%recovTime = 0.1;
	rpgbarhud::load(%recovTime, %colour, %ecolour, %chars, %barnum, %addtext);
}

//Testing lines:
//remoterpgbarhud(2048, 45, 4, 2, "||", 0, "test");
//remoterpgbarhud(2048, 45, 4, 5, "||", 1, "meowers");

//==========================================================================================================

//font translation stuff written by phantom
//tribesrpg.org

$charWidth[32] = 5; $charWidth[33] = 2; $charWidth[34] = 3; $charWidth[35] = 9; $charWidth[36] = 7;
$charWidth[37] = 12; $charWidth[38] = 9; $charWidth[39] = 1; $charWidth[40] = 4; $charWidth[41] = 4;
$charWidth[42] = 5; $charWidth[43] = 7; $charWidth[44] = 3; $charWidth[45] = 3; $charWidth[46] = 2;
$charWidth[47] = 4; $charWidth[48] = 6; $charWidth[49] = 2; $charWidth[50] = 6; $charWidth[51] = 6;
$charWidth[52] = 6; $charWidth[53] = 6; $charWidth[54] = 6; $charWidth[55] = 6;$charWidth[56] = 6;
$charWidth[57] = 6; $charWidth[58] = 2; $charWidth[59] = 3; $charWidth[60] = 7; $charWidth[61] = 8;
$charWidth[62] = 7; $charWidth[63] = 4; $charWidth[64] = 12; $charWidth[65] = 10; $charWidth[66] = 7;
$charWidth[67] = 8; $charWidth[68] = 9; $charWidth[69] = 6; $charWidth[70] = 6; $charWidth[71] = 9;
$charWidth[72] = 8; $charWidth[73] = 2; $charWidth[74] = 4; $charWidth[75] = 9; $charWidth[76] = 6;
$charWidth[77] = 10; $charWidth[78] = 9; $charWidth[79] = 10; $charWidth[80] = 7; $charWidth[81] = 10;
$charWidth[82] = 8; $charWidth[83] = 7; $charWidth[84] = 8; $charWidth[85] = 8; $charWidth[86] = 9;
$charWidth[87] = 13; $charWidth[88] = 10; $charWidth[89] = 10; $charWidth[90] = 9; $charWidth[91] = 4;
$charWidth[92] = 4; $charWidth[93] = 4; $charWidth[94] = 8; $charWidth[95] = 7; $charWidth[96] = 3;
$charWidth[97] = 6; $charWidth[98] = 6; $charWidth[99] = 5; $charWidth[100] = 6; $charWidth[101] = 6;
$charWidth[102] = 5; $charWidth[103] = 7; $charWidth[104] = 6; $charWidth[105] = 2; $charWidth[106] = 3;
$charWidth[107] = 7; $charWidth[108] = 2; $charWidth[109] = 10; $charWidth[110] = 6; $charWidth[111] = 7;
$charWidth[112] = 6; $charWidth[113] = 6; $charWidth[114] = 5; $charWidth[115] = 5; $charWidth[116] = 5;
$charWidth[117] = 7; $charWidth[118] = 7; $charWidth[119] = 10; $charWidth[120] = 7; $charWidth[121] = 7;
$charWidth[122] = 6; $charWidth[123] = 5; $charWidth[124] = 1; $charWidth[125] = 5; $charWidth[126] = 8;

function charCode(%char){
	for(%i = 32; %i < 127; %i++)
	{
		if(String::Compare(%char, $char[%i]) == 0)
			return %i;
	}
	for(%i = 127; %i < 221; %i++)
	{
		if(String::Compare(%char, $char[%i]) == 0)
			return %i-94;
	}
	return 124;//1 width fallback
}

function bytetohex(%decimal)
{
    %hex[ 0] = "0"; %hex[ 1] = "1"; %hex[ 2] = "2"; %hex[ 3] = "3";
    %hex[ 4] = "4"; %hex[ 5] = "5"; %hex[ 6] = "6"; %hex[ 7] = "7";
    %hex[ 8] = "8"; %hex[ 9] = "9"; %hex[10] = "A"; %hex[11] = "B";
    %hex[12] = "C"; %hex[13] = "D"; %hex[14] = "E"; %hex[15] = "F";
    
    %b = floor(%decimal / 16);
    %r = %decimal % 16;
    
    %value = %hex[%b] @ %hex[%r];
    
    return %value;
}

$hex["00"] = "\x00"; $hex["01"] = "\x01"; $hex["02"] = "\x02"; $hex["03"] = "\x03";
$hex["04"] = "\x04"; $hex["05"] = "\x05"; $hex["06"] = "\x06"; $hex["07"] = "\x07";
$hex["08"] = "\x08"; $hex["09"] = "\x09"; $hex["0A"] = "\x0A"; $hex["0B"] = "\x0B";
$hex["0C"] = "\x0C"; $hex["0D"] = "\x0D"; $hex["0E"] = "\x0E"; $hex["0F"] = "\x0F";

$hex["10"] = "\x10"; $hex["11"] = "\x11"; $hex["12"] = "\x12"; $hex["13"] = "\x13";
$hex["14"] = "\x14"; $hex["15"] = "\x15"; $hex["16"] = "\x16"; $hex["17"] = "\x17";
$hex["18"] = "\x18"; $hex["19"] = "\x19"; $hex["1A"] = "\x1A"; $hex["1B"] = "\x1B";
$hex["1C"] = "\x1C"; $hex["1D"] = "\x1D"; $hex["1E"] = "\x1E"; $hex["1F"] = "\x1F";

$hex["20"] = "\x20"; $hex["21"] = "\x21"; $hex["22"] = "\x22"; $hex["23"] = "\x23";
$hex["24"] = "\x24"; $hex["25"] = "\x25"; $hex["26"] = "\x26"; $hex["27"] = "\x27";
$hex["28"] = "\x28"; $hex["29"] = "\x29"; $hex["2A"] = "\x2A"; $hex["2B"] = "\x2B";
$hex["2C"] = "\x2C"; $hex["2D"] = "\x2D"; $hex["2E"] = "\x2E"; $hex["2F"] = "\x2F";

$hex["30"] = "\x30"; $hex["31"] = "\x31"; $hex["32"] = "\x32"; $hex["33"] = "\x33";
$hex["34"] = "\x34"; $hex["35"] = "\x35"; $hex["36"] = "\x36"; $hex["37"] = "\x37";
$hex["38"] = "\x38"; $hex["39"] = "\x39"; $hex["3A"] = "\x3A"; $hex["3B"] = "\x3B";
$hex["3C"] = "\x3C"; $hex["3D"] = "\x3D"; $hex["3E"] = "\x3E"; $hex["3F"] = "\x3F";

$hex["40"] = "\x40"; $hex["41"] = "\x41"; $hex["42"] = "\x42"; $hex["43"] = "\x43";
$hex["44"] = "\x44"; $hex["45"] = "\x45"; $hex["46"] = "\x46"; $hex["47"] = "\x47";
$hex["48"] = "\x48"; $hex["49"] = "\x49"; $hex["4A"] = "\x4A"; $hex["4B"] = "\x4B";
$hex["4C"] = "\x4C"; $hex["4D"] = "\x4D"; $hex["4E"] = "\x4E"; $hex["4F"] = "\x4F";

$hex["50"] = "\x50"; $hex["51"] = "\x51"; $hex["52"] = "\x52"; $hex["53"] = "\x53";
$hex["54"] = "\x54"; $hex["55"] = "\x55"; $hex["56"] = "\x56"; $hex["57"] = "\x57";
$hex["58"] = "\x58"; $hex["59"] = "\x59"; $hex["5A"] = "\x5A"; $hex["5B"] = "\x5B";
$hex["5C"] = "\x5C"; $hex["5D"] = "\x5D"; $hex["5E"] = "\x5E"; $hex["5F"] = "\x5F";

$hex["60"] = "\x60"; $hex["61"] = "\x61"; $hex["62"] = "\x62"; $hex["63"] = "\x63";
$hex["64"] = "\x64"; $hex["65"] = "\x65"; $hex["66"] = "\x66"; $hex["67"] = "\x67";
$hex["68"] = "\x68"; $hex["69"] = "\x69"; $hex["6A"] = "\x6A"; $hex["6B"] = "\x6B";
$hex["6C"] = "\x6C"; $hex["6D"] = "\x6D"; $hex["6E"] = "\x6E"; $hex["6F"] = "\x6F";

$hex["70"] = "\x70"; $hex["71"] = "\x71"; $hex["72"] = "\x72"; $hex["73"] = "\x73";
$hex["74"] = "\x74"; $hex["75"] = "\x75"; $hex["76"] = "\x76"; $hex["77"] = "\x77";
$hex["78"] = "\x78"; $hex["79"] = "\x79"; $hex["7A"] = "\x7A"; $hex["7B"] = "\x7B";
$hex["7C"] = "\x7C"; $hex["7D"] = "\x7D"; $hex["7E"] = "\x7E"; $hex["7F"] = "\x7F";

$hex["80"] = "\x80"; $hex["81"] = "\x81"; $hex["82"] = "\x82"; $hex["83"] = "\x83";
$hex["84"] = "\x84"; $hex["85"] = "\x85"; $hex["86"] = "\x86"; $hex["87"] = "\x87";
$hex["88"] = "\x88"; $hex["89"] = "\x89"; $hex["8A"] = "\x8A"; $hex["8B"] = "\x8B";
$hex["8C"] = "\x8C"; $hex["8D"] = "\x8D"; $hex["8E"] = "\x8E"; $hex["8F"] = "\x8F";

$hex["90"] = "\x90"; $hex["91"] = "\x91"; $hex["92"] = "\x92"; $hex["93"] = "\x93";
$hex["94"] = "\x94"; $hex["95"] = "\x95"; $hex["96"] = "\x96"; $hex["97"] = "\x97";
$hex["98"] = "\x98"; $hex["99"] = "\x99"; $hex["9A"] = "\x9A"; $hex["9B"] = "\x9B";
$hex["9C"] = "\x9C"; $hex["9D"] = "\x9D"; $hex["9E"] = "\x9E"; $hex["9F"] = "\x9F";

$hex["A0"] = "\xA0"; $hex["A1"] = "\xA1"; $hex["A2"] = "\xA2"; $hex["A3"] = "\xA3";
$hex["A4"] = "\xA4"; $hex["A5"] = "\xA5"; $hex["A6"] = "\xA6"; $hex["A7"] = "\xA7";
$hex["A8"] = "\xA8"; $hex["A9"] = "\xA9"; $hex["AA"] = "\xAA"; $hex["AB"] = "\xAB";
$hex["AC"] = "\xAC"; $hex["AD"] = "\xAD"; $hex["AE"] = "\xAE"; $hex["AF"] = "\xAF";

$hex["B0"] = "\xB0"; $hex["B1"] = "\xB1"; $hex["B2"] = "\xB2"; $hex["B3"] = "\xB3";
$hex["B4"] = "\xB4"; $hex["B5"] = "\xB5"; $hex["B6"] = "\xB6"; $hex["B7"] = "\xB7";
$hex["B8"] = "\xB8"; $hex["B9"] = "\xB9"; $hex["BA"] = "\xBA"; $hex["BB"] = "\xBB";
$hex["BC"] = "\xBC"; $hex["BD"] = "\xBD"; $hex["BE"] = "\xBE"; $hex["BF"] = "\xBF";

$hex["C0"] = "\xC0"; $hex["C1"] = "\xC1"; $hex["C2"] = "\xC2"; $hex["C3"] = "\xC3";
$hex["C4"] = "\xC4"; $hex["C5"] = "\xC5"; $hex["C6"] = "\xC6"; $hex["C7"] = "\xC7";
$hex["C8"] = "\xC8"; $hex["C9"] = "\xC9"; $hex["CA"] = "\xCA"; $hex["CB"] = "\xCB";
$hex["CC"] = "\xCC"; $hex["CD"] = "\xCD"; $hex["CE"] = "\xCE"; $hex["CF"] = "\xCF";

$hex["D0"] = "\xD0"; $hex["D1"] = "\xD1"; $hex["D2"] = "\xD2"; $hex["D3"] = "\xD3";
$hex["D4"] = "\xD4"; $hex["D5"] = "\xD5"; $hex["D6"] = "\xD6"; $hex["D7"] = "\xD7";
$hex["D8"] = "\xD8"; $hex["D9"] = "\xD9"; $hex["DA"] = "\xDA"; $hex["DB"] = "\xDB";
$hex["DC"] = "\xDC"; $hex["DD"] = "\xDD"; $hex["DE"] = "\xDE"; $hex["DF"] = "\xDF";

$hex["E0"] = "\xE0"; $hex["E1"] = "\xE1"; $hex["E2"] = "\xE2"; $hex["E3"] = "\xE3";
$hex["E4"] = "\xE4"; $hex["E5"] = "\xE5"; $hex["E6"] = "\xE6"; $hex["E7"] = "\xE7";
$hex["E8"] = "\xE8"; $hex["E9"] = "\xE9"; $hex["EA"] = "\xEA"; $hex["EB"] = "\xEB";
$hex["EC"] = "\xEC"; $hex["ED"] = "\xED"; $hex["EE"] = "\xEE"; $hex["EF"] = "\xEF";

$hex["F0"] = "\xF0"; $hex["F1"] = "\xF1"; $hex["F2"] = "\xF2"; $hex["F3"] = "\xF3";
$hex["F4"] = "\xF4"; $hex["F5"] = "\xF5"; $hex["F6"] = "\xF6"; $hex["F7"] = "\xF7";
$hex["F8"] = "\xF8"; $hex["F9"] = "\xF9"; $hex["FA"] = "\xFA"; $hex["FB"] = "\xFB";
$hex["FC"] = "\xFC"; $hex["FD"] = "\xFD"; $hex["FE"] = "\xFE"; $hex["FF"] = "\xFF";

function generateCharCodes(){
	for(%i = 32; %i < 256; %i++){
		$char[%i] = $hex[bytetohex(%i)];
	}
}
generateCharCodes();

function charTranslate(%char){
	for(%i = 33; %i < 127; %i++)
	{
		if(String::Compare(%char, $char[%i]) == 0)
			return $char[%i+94];
	}
	return %char;
}
function charTranslate2(%char){
	for(%i = 64; %i < 100; %i++)
	{
		if(String::Compare(%char, $char[%i]) == 0)
			return $char[%i+94+62];
	}
	return %char;
}

function string::translate(%msg){
	%final = "";
	for(%i;(%char = String::getSubStr(%msg,%i,1))!="";%i++){
		%c = charTranslate(%char);
		%final = %final @ %c;
	}
	return %final;
}
function string::printcolorbar(%colour, %msg){
	if(%colour == 0)
		return "<f0>"@%msg;
	else if(%colour == 1)
		return "<f1>"@%msg;
	else if(%colour == 2)
		return "<f2>"@%msg;
	else if(%colour == 3)
		return "<f0>"@charTranslate(%msg);
	else if(%colour == 4)
		return "<f1>"@charTranslate(%msg);
	else if(%colour == 5)
		return "<f2>"@charTranslate(%msg);
	else {
		%colour-=5;
		return "<f0>"@$char[255-%colour];
	}
}

//echo("__RPGMSGHUD LOADED");


//Custom huds and escape screen text use the following fonts:
//IDFNT_10_STANDARD               = 00150111, "sf_orange214_10.pft";
//IDFNT_10_HILITE                 = 00150121, "sf_orange255_10.pft";
//IDFNT_10_SELECTED               = 00150161, "sf_white_10.pft";

//Centerprint uses the following fonts:
//"sf_orange214_10.pft"
//"sf_orange255_10.pft"
//"sf_white_9b.pft"

//Tabmenu uses the following fonts:
//"sf_orange255_10.pft"
//"sf_white_10.pft"

//Chat hud uses the following fonts:
//"if_w_10b.pft"
//"sf_red_10b.pft"
//"sf_yellow_10b.pft"
//"if_g_10b.pft"




//By phantom, tribesrpg.org, repack 36
//Supports up to two lines of text. Displays just under the centre of the screen.
//Currently the first line should be the longest one.
//Max 255 chars
function remoteCenterTip(%server, %text, %time){
	if(%server != 2048) return;
	centertip::reset();
	if(%text == "") return;
	%res = Repack::ScreenSize();
	%x = getWord(%res,0);
	%y = getWord(%res,1);
	%centery = %y * 0.5;
	%centery += 10;
	%centerx = %x * 0.5;
	%centerx -= 10;
	%bgheight = 18;
	%trimmed = %text;
	%ind1 = String::findsubstr(%text, "\n");
	if(%ind1 > 0){
		%trimmed = String::getSubStr(%text, 0, %ind1);
		%bgheight *= 2;
	}
	%xtr = String::removeBetween(%trimmed, "<", ">");
	while(%xtr != %trimmed){
		%trimmed = %xtr;
		%xtr = String::removeBetween(%trimmed, "<", ">");
	}
	
	for (%i = 0; (%v = string::getsubstr(%trimmed,%i,1)) != ""; %i++)
		%textw += rpgmsghud::charwidth(%v);
	%bgwidth = %textw+4;

	%y = getWord(%res,1);
	//%y -= %bgheight;
	%posy = %centery;

	%bgposx = %centerx - (%bgwidth * 0.5)-1;
	%titleposx = %centerx - (%textw * 0.5);

	%object = newObject("rpgcthud_frame", FearGui::FearGuiMenu, %bgposx, %posy, %bgwidth, %bgheight);
	addToSet(PlayGui, %object);

	%object = newObject("rpgcthud_0", FearGuiFormattedText,%titleposx, %posy, 15, 100);
	addToSet(PlayGui, %object);
	control::setValue("rpgcthud_0",%text);

	if(%time > 0)
		Schedule::Add("centertip::reset();", %time, "centertip");
}


function centertip::reset(){
	%ngs = nameToId(NamedGuiSet);
	%len = Group::objectCount(%ngs);
	for(%i = 0; %i < %len; %i++) {
 		%obj = Group::getObject(%ngs, %i);
  		%objectName = Object::getName(%obj);
		if(string::FindSubStr(%objectname,"rpgcthud_") != -1)
			%list = %list @ %obj @ " ";
	}
	for (%i = 0; (%g = getWord(%list,%i)) != -1; %i++)
		deleteObject(%g);
}