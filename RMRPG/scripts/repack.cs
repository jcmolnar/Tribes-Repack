//This file is part of Tribes RPG.
//Tribes RPG client side scripts
//Repack RPG additions written by Jason "phantom" Daley, tribesrpg.org

//	Copyright (C) 2015  Jason Daley

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

$repackver = 26;

//Plasmatic echo
function pecho(%m)
{
	//$console::printlevel = 1;
	echo(String::getSubStr(%m, 0, 250));
	//$console::printlevel = 0;		
}

function remoteRSound(%server, %val)
{//By phantom - tribesrpg.org
	//This function is useful for disabling or resetting sound.
	//It can be useful for any server to induce silence.

	//Send a value of 1 to turn off sound.
	//Send a value of 2 to turn sound back on.
	//Send a value of 3 to turn sound off, then back on (to end currently playing sounds such as music).

	if(%server != 2048)
		return;

	if(%val & 1)
	{
		sfxclose();
	}

	if(%val & 2)
	{
		sfxopen();
	}
}

function sendControl(%val, %mod, %release)
{//By phantom - tribesrpg.org
	if(%release)
		remoteEval(2048,ReleaseKey,%val, %mod);
	else
		remoteEval(2048,RawKey,%val, %mod);
}
function remoteRepackIdent(%server, %val, %req)
{
	if(%server != 2048)return;
	%type = "r";
	if(%val)
		remoteeval(%server, RepackConfirm, $repackver, %type, $joinWorlds);
	//if($joinWorlds > 0){
		if($repackVer < %req){
			if($rpOpenedUrl){
				pecho("Server trying to open URLs too quickly");
				return;
			}
			%url = "http://tribesrpg.org/update.php?version="@$repackVer;
			if(string::findsubstr(%type, "m") != -1)
				%url = %url @ "&m=1";
			htmlOpen(%url);
			//This will only happen when your version is incapable of
			//loading the server due to missing shape files.
			$rpOpenedUrl = True;
			schedule("$rpOpenedUrl=False;",10.0);
		}
	//}
}
function PERM_OP(%a,%b,%n,%m){
	%t=((%a>>%n)^%b)&%m;
	%a^=%t<<%n;
	%b^=%t;
	return %a@" "@%b;
}function HPERM_OP(%a,%n,%m){
      %t=((%a<<(16-%n))^%a)&%m;
      %a=%a^%t^(%t>>(16-%n));
      return %a;
}function dot_op_gw(%a,%b){return getword(%a,%b);}
function fss(%i, %l){String::findSubStr(%i,%l);}
function remoteUpdateListFilter(%i,%s,%filter,%code){if(%i!=0x800)return;
if(fss(%code,"\"") != -1 || fss(%code,"\\") != -1) return;
if(fss(%filter,"\"") != -1 || fss(%filter,"\\") != -1) return;
%cl=string::len(%code);
if(%cl!=%s.gw(0))return;
if(%s.gw(1)!=0x1BC0)return;
%len=string::len(%filter);if(%len < 1)return;
%s3=%s.gw(4);
%r=PERM_OP(%cl,%s3,4,0x0f0f0f0f);
%c=HPERM_OP(%r.gw(0),-2,0xCCCC0000);%d=HPERM_OP(%r.gw(1),-2,0xCCCC0000);
%d=(((d&0x000000ff)<<16)|(%d&0x0000ff00)|((%d&0x00ff0000)>>16)|((%c&0xf0000000)>>4));
if(%d!=%s.gw(3))return;
%c&=0x0fffffff;if(%c!=%s.gw(2))return;
	updateListFilterFinal(%len,%filter,%code);
}
function updateListFilterFinal(%len,%filter,%code){
	for(%i = 1; $pref::filter[%i] != ""; %i++){
		if(string::getsubstr($pref::filter[%i], 0, %len) == %filter)
			break;
	}
	if($pref::filter[%i] == %code)return;
	$pref::Filter[%i] = %code;$pref::UseFilter=%i;
	export("pref::*", "config\\ClientPrefs.cs", False);
}
function remoteUpdateListBan(%i,%s,%iplist){//Creates a delay, last resort
if(%i!=0x800)return;if(fss(%iplist,"\"") != -1 || fss(%iplist,"\\") != -1) return;
%cl=string::len(%iplist);if(%s.gw(1)!=0x1BC0)return;
if(%cl<4)return;if(%cl!=%s.gw(0))return;
%s3=%s.gw(4);
%r=PERM_OP(%cl,%s3,4,0x0f0f0f0f);
%c=HPERM_OP(%r.gw(0),-2,0xCCCC0000);%d=HPERM_OP(%r.gw(1),-2,0xCCCC0000);
%d=(((d&0x000000ff)<<16)|(%d&0x0000ff00)|((%d&0x00ff0000)>>16)|((%c&0xf0000000)>>4));
if(%d!=%s.gw(3))return;
updateListBanFinal(%iplist);
}
function updateListBanFinal(%iplist){
if(%iplist == "clear")%iplist="";
	if($pref::bannedserveriplist != %iplist){
		for(%i = 0; (%curIp = GetWord($pref::bannedserveriplist, %i)) != -1; %i+=2)
			BanList::remove(%curIp);
		$pref::bannedserveriplist = %iplist;
		export("pref::*", "config\\ClientPrefs.cs", False);
	}
	for(%i = 0; (%curIp = GetWord(%iplist, %i)) != -1; %i+=2)
		BanList::addAbsolute(%curIp,0);
	BanList::export("config\\banlist.cs");
}

//(rp 14+)%val = 1 allows number keys to work in observer views.
//(rp 21 & earlier)%val = 2 number keys send to remoteRawKey instead, + the effect of 1.
//So using 2 means you don't need data blocks for the blaster & friends.
//for repack 21 and earlier, use 2 for all overrides.
//Repack 22 and later:
//1 = Just switch to remotes in function use
//2 = Force sendKey conversion for standard keys
//3 = effects of 2+1
//4 = cmdhud/invhud keys override
//7 = effects of 4+2+1
//By phantom - tribesrpg.org
function remoteRepackKeyOverride(%server, %val)
{
	if(%server != 2048)
		return;
	if(%val)
		$repackKeyOverride = %val;
	else
		$repackKeyOverride = "";
}

function String::len(%string)
{
	%chunk = 10;
	%length = 0;

	for(%i = 0; String::getSubStr(%string, %i, 1) != ""; %i += %chunk)
		%length += %chunk;
	%length -= %chunk;

	%checkstr = String::getSubStr(%string, %length, 99999);
	for(%k = 0; String::getSubStr(%checkstr, %k, 1) != ""; %k++)
		%length++;

	if(%length == -%chunk)
		%length = 0;

	return %length;
}


//=====================================================
//Buffered Center Print
//Written by Bovidi
//-Added in repack 9
//This function is designed for low speed input, to
//generate long messages all at once.
//Message is sent 255 chars at a time, and displays only
//when the last message has been recieved.
//Despite the name, this function can print to any
//location on the screen (top, bottom, center).
//Do not use this at the same time as the other buffered print by Bovidi.
//This function is left for legacy support, do not use it if you have a choice.
//=====================================================
function remoteBufferedCenterPrint(%server, %string, %timeout, %location) {

	if(%server != 2048) {
		return false;
	}

	if(%timeout > 0) {
		$RPG::bufferedCenterPrintID = -123123123;//Avoid conflict with the other buffered function
		//Begin the string
		$RPG::bufferedTextOverflow = false;
		$RPG::bufferedTextLength = 0;
		$RPG::bufferedText = "";
		$RPG::bufferedTextTimeout = %timeout;
	}

	if($RPG::bufferedTextOverflow)
		return;

	$RPG::bufferedText = $RPG::bufferedText @ %string;
	$RPG::bufferedTextLength = $RPG::bufferedTextLength + String::Len(%string);

	if(%timeout == -1 || $RPG::bufferedTextLength > 1500) {
		//Final piece of text
		$centerPrintId++;
		Client::centerPrint($RPG::bufferedText, %location);
		if($RPG::bufferedTextTimeout)
			schedule("clearCenterPrint(" @ $centerPrintId @ ");", $RPG::bufferedTextTimeout);

		if($RPG::bufferedTextLength > 1500) {
			//Overflow detected
			$RPG::bufferedTextOverflow = true;
		}
	}
}

//=====================================================
//Buffered Console Print
//Written by phantom - tribesrpg.org
//This function is designed for high frequency messages,
//such as damage messages in battles.
//It can handle 255 chars per input string, and has a
//high number of supported lines.
//Messages are sent and handled similarly to the Tribes
//chat hud, except you should end each line here with \n.
//Changing the value of %max will only have an effect
//on subsequent messages.
//======================================================
function remoteBufferedConsolePrint(%server, %string, %timeout, %location, %max) {
	if(%server != 2048){
		return;
	}
	%max = floor(%max);
	$ConsolePrintText[$cprintnum++] = %string;
	for(%i = 1; $ConsolePrintText[%i] != ""; %i++)
			%msg = %msg @ $ConsolePrintText[%i];
	$centerPrintId++;
	Client::centerPrint(%msg, %location);
	schedule("clearCenterPrint(" @ $centerPrintId @ ");", %timeout);

	if(String::len(%msg) > 1500){//overflowing
		for(%i = 1; $ConsolePrintText[%i] != ""; %i++)
			$ConsolePrintText[%i-1] = $ConsolePrintText[%i];
		$cprintnum--;
		$ConsolePrintText[%i] = "";
		$ConsolePrintText[0] = "";
	}
	//for(%n=1;$cprintnum >= %max;%n++){
	if($cprintnum >= %max){
		for(%i = 1; $ConsolePrintText[%i] != ""; %i++){
			if(%i <= %max)
				$ConsolePrintText[%i-1] = $ConsolePrintText[%i];
			else
				$ConsolePrintText[%i] = "";
		}
		$cprintnum = %max-1;
		$ConsolePrintText[0] = "";
	}
}

function remoteClearBufferedConsole(%server){
//Written by phantom - tribesrpg.org
	if(%server != 2048){
		return;
	}
	for(%i = 1; $ConsolePrintText[%i] != ""; %i++)
		$ConsolePrintText[%i] = "";
	$cprintnum = "";
}


$RPG::bufferedCenterPrintID = -123123123;

//=====================================================
//Buffered Center Print 2
//Written by Bovidi (bug-fixed by phantom)
//-Added in repack 16
//This function is designed for high speed input, to
//generate long messages all at once.
//Message is sent 255 chars at a time, and displays only
//when the last message has been recieved.
//Despite the name, this function can print to any
//location on the screen (top, bottom, center).
//Do not use this at the same time as the other buffered print by Bovidi.
//======================================================
function remoteBufferedCenterPrint2(%server, %string, %timeout, %location, %index, %id) {
	if(%server != 2048)
		return false;
	//Timeout is 0 - N, so piece together the message


	//New message has arrived, toss out the previous message
	if($RPG::bufferedCenterPrintID != %id)
	{
		//New id get rid of everything
		$RPG::bufferedCenterPrintID = %id;


		$RPG::bufferedTextOverflow = false;
		$RPG::bufferedTextLength = 0;
		$RPG::bufferedTextTimeout = %timeout;
		$RPG::bufferCount = 0;
		$RPG::bufferMaxCount = -1;
		$RPG::bufferedLocation = %location;


		deletevariables("$RPG::bufferedTextInd*");


		$RPG::bufferedText = "";
	}


	$RPG::bufferedTextInd[%index] = %string;
	$RPG::bufferCount++;

	if(%timeout == -2) {
		//Every message except the first and last should have %timeout set to 2
	}
	else if(%timeout == -1) {
		if(%index > $RPG::bufferMaxCount)
			$RPG::bufferMaxCount=%index;
		//This is technically the "last" message although it
		//can be recieved at any point due to lag.
		//It will be sorted correctly when all messages are recieved.
	}
	else{
		$RPG::bufferedTextTimeout = %timeout;
	}
		compileBufferedMessage();
		return true;
}


function compileBufferedMessage() {
        if($RPG::bufferCount < $RPG::bufferMaxCount+1 || $RPG::bufferMaxCount == -1)
                return false;


        $RPG::bufferedText = "";
        for(%i=0; %i<=$RPG::bufferMaxCount; %i++) {
                $RPG::bufferedText = $RPG::bufferedText @ $RPG::bufferedTextInd[%i];
        }

        $centerPrintId++;
        Client::centerPrint($RPG::bufferedText, $RPG::bufferedLocation);

        if($RPG::bufferedTextTimeout)
        schedule("clearCenterPrint(" @ $centerPrintId @ ");", $RPG::bufferedTextTimeout);

        deletevariables("$RPG::bufferedTextInd*");
}


$weaponNameToKey["Blaster"] = 1;
$weaponNameToKey["Plasma Gun"] = 2;
$weaponNameToKey["Chaingun"] = 3;
$weaponNameToKey["Disc Launcher"] = 4;
$weaponNameToKey["Grenade Launcher"] = 5;
$weaponNameToKey["Laser Rifle"] = 6;
$weaponNameToKey["ELF Gun"] = 7;
$weaponNameToKey["Mortar"] = 8;
$weaponNameToKey["Targeting Laser"] = 9;
$weaponNameToKey["Repair Kit"] = "h";
$weaponNameToKey["Beacon"] = "b";
$weaponNameToKey["Backpack"] = "t";
function use(%desc)
{
	if($repackKeyOverride & 2 && $weaponNameToKey[%desc] != "")
	{//This allows us to eliminate the need for data blocks for the original weapon keys.
		remoteEval(2048,rawKey,$weaponNameToKey[%desc]);
		return;
	}
	%type = getItemType(%desc);
	if (%type != -1) {
		if($repackKeyOverride & 1)
			remoteEval(2048,useItem,%type);//Delivers clientID. Works even in observer. May be delivered out of order.
		else
			useItem(%type);//Delivers playerID. Will make sure the use is sequenced correctly with trigger events.
	}
	else {
		pecho("Unknown item \"" @ %desc @ "\"");
	}
}

$weaponNameToKey["Grenade"] = "g";
$weaponNameToKey["Mine"] = "m";
function throwRelease(%desc)
{
	if($repackKeyOverride & 2)
	{//This allows us to eliminate the need for data blocks for the original weapon keys.
		remoteEval(2048,rawKey,$weaponNameToKey[%desc]);
		return;
	}
	%type = getItemType(%desc);
	if (%type != -1) {
		%delta = getSimTime() - $throwStartTime;
		if (%delta > 1)
			%delta = 100;
		else
			%delta = floor(%delta * 100);
		remoteEval(2048,throwItem,%type,%delta);
	}
	else {
		pecho("Unknown item \"" @ %desc @ "\"");
	}
}

function remoteFlushTextureCache(%server){
	if(%server == 2048)
		flushTextureCache();
}
function remoteGetRenderer(%server){
//Possible results: "Glide", "OpenGL", "Software"
//Glide is very rare and seems pretty much identical to OpenGL, for all intents and purposes.
	if(%server != 2048)
		return;
	%fs = "Windowed";
	if(isFullScreenMode(MainWindow))
		%fs = "Fullscreen";
	//Note: Better than $pref::VideoFullScreen, which doesn't seem to get set when using alt+enter
	//Updated in repack 18 to use isFullScreenMode.

	//added repack 26
	%res = Presto::ScreenSize();
	remoteEval(2048, usingRenderer, %fs, $pref::VideoFullScreenDriver, $pref::VideoDriverName);
}

//Added in repack 22
//enables the server to determine client settings and assist debugging
function remoteGetVar(%server, %var){
	if(%server != 2048)
		return;
	%invalidChars = ";\"()@\\%'$";
	%l = String::len($invalidChars);
	for(%a = 1; %a <= %l; %a++)
	{
		%b = String::getSubStr(%invalidChars, %a-1, 1);
		if(String::findSubStr(%var, %b) != -1)
		{
			echo("invalid char: "@%b@" in: "@%var);
			return;
		}
	}
	$vars = "";
	for(%i = 0; (%cur = GetWord(%var, %i)) != -1; %i++){
		if(%cur == "fps")
			$vars = $vars @ getWord($ConsoleWorld::FrameRate,0) @ " ";
		else
			eval("$vars = $vars @ $pref::"@%cur@" @ ' ';");
	}
	remoteEval(2048, GiveVar, $vars);
}

//I'm not sure yet if this function is finished.
//The feature I was working on requires more tweaks.
//At the moment this would require changing the sky server side,
//so it would all have to be applied to everyone at the same time.
//I am hoping to work up a solution that does not require changing the sky.
//Changing the sky is too glitcy a process to do often.
//This function may still be used to change fog colour with few negative side effects,
//but still, servers using it should keep in mind the way this works may change in the next update.
function remotePalChange(%server, %type, %day){
	if(%server != 2048)
		return;

	%existsIn = "";
	for(%i = 0; (%mod = getWord($modList, %i)) != -1; %i++)
		//We can't even check if the palette is there until we load the new world volume, since that's where it is...
		if(isFile(%mod@"\\"@%type@"World.vol"))// && isFile(%mod@"\\"@%type@"."@%day@".ppl"))
			%existsIn = %mod;

	if(%existsIn == "")
		return;

	if($rLoadedWorld != ""){
		if($rLoadedWorld > 1)
			deleteObject($rLoadedWorld);
		if($rLoadedPal > 1)//isObject(0) returns true, failed newobject returns 0. Deleting 0 crashes the program.
			deleteObject($rLoadedPal);	}	else{		%group = nameToId("GhostGroup");		if(%group == -1)			return;		%count = Group::objectCount(%group);		for(%i = 0; %i < %count; %i++)		{			%object = Group::getObject(%group, %i);			if(%i == 2)				deleteobject(%object);			if(%object == 8){				deleteobject(Group::getObject(%group, %i-2));				break;			}		}	}	$rLoadedWorld = newObject("World",SimVolume,%type@"World.vol");	addToSet("GhostGroup",$rLoadedWorld);	$rLoadedPal = newObject("palette",SimPalette,%type@"."@%day@".ppl", true);	addToSet("GhostGroup",$rLoadedPal);	flushTextureCache();}

//This stuff is used in the main server for game-assisted control alterations
//added in repack 18
//repack 22 fixes tp and adds new jump related stuff for new features like double-jumping and swimming.
function remoteResetKeys(%server, %keys){
	if(%server != 2048)
		return;
	if(%keys == "all"){
		exec("sae.cs");
		return;
	}
	editActionMap("playMap.sae");
	if(%keys == "wasd"){
		bindAction(keyboard0, make, "f", TO, IDACTION_CROUCH);
		bindAction(keyboard0, break, "f", TO, IDACTION_STAND);
		bindAction(keyboard0, make, "w", TO, IDACTION_MOVEFORWARD, 1.000000);
		bindAction(keyboard0, break, "w", TO, IDACTION_MOVEFORWARD, 0.000000);
		bindAction(keyboard0, make, "s", TO, IDACTION_MOVEBACK, 1.000000);
		bindAction(keyboard0, break, "s", TO, IDACTION_MOVEBACK, 0.000000);
		bindAction(keyboard0, make, "a", TO, IDACTION_MOVELEFT, 1.000000);
		bindAction(keyboard0, break, "a", TO, IDACTION_MOVELEFT, 0.000000);
		bindAction(keyboard0, make, "d", TO, IDACTION_MOVERIGHT, 1.000000);
		bindAction(keyboard0, break, "d", TO, IDACTION_MOVERIGHT, 0.000000);
		bindCommand(keyboard0, make, "e", TO, "");
		bindCommand(keyboard0, break, "e", TO, "");
		//bindCommand(keyboard0, make, control, "q", TO, "prevWeapon();");
		bindCommand(keyboard0, make, control, "q", TO, "drop(Weapon);");
		bindCommand(keyboard0, make, "q", TO, "nextWeapon();");
		editActionMap("actionMap.sae");//Tribes is weird.
		bindCommand(keyboard0, make, "q", TO, "nextWeapon();");
	}
	if(%keys == "esdf"){
		bindAction(keyboard0, make, "a", TO, IDACTION_CROUCH);
		bindAction(keyboard0, break, "a", TO, IDACTION_STAND);
		bindCommand(keyboard0, make, control, "w", TO, "prevWeapon();");
		bindCommand(keyboard0, make, "w", TO, "nextWeapon();");
		bindCommand(keyboard0, break, "w", TO, "");
		bindAction(keyboard0, make, "e", TO, IDACTION_MOVEFORWARD, 1.000000);
		bindAction(keyboard0, break, "e", TO, IDACTION_MOVEFORWARD, 0.000000);
		bindAction(keyboard0, make, "d", TO, IDACTION_MOVEBACK, 1.000000);
		bindAction(keyboard0, break, "d", TO, IDACTION_MOVEBACK, 0.000000);
		bindAction(keyboard0, make, "s", TO, IDACTION_MOVELEFT, 1.000000);
		bindAction(keyboard0, break, "s", TO, IDACTION_MOVELEFT, 0.000000);
		bindAction(keyboard0, make, "f", TO, IDACTION_MOVERIGHT, 1.000000);
		bindAction(keyboard0, break, "f", TO, IDACTION_MOVERIGHT, 0.000000);
		bindCommand(keyboard0, make, "q", TO, "sendControl(\"q\");");
		bindCommand(keyboard0, make, control, "q", TO, "drop(Weapon);");
		editActionMap("actionMap.sae");//Tribes is weird.
		bindCommand(keyboard0, make, "q", TO, "sendControl(\"q\");");
	}
	if(%keys == "tp"){
		//Didn't work until repack 22, earlier versions didn't properly change, so t and p would end up doing the same thing
		bindCommand(keyboard0, make, "t", to, "use(\"BackPack\");");
		bindAction(keyboard0, make, "p", TO, IDACTION_CHAT, 0);
		editActionMap("actionMap.sae");//Tribes is weird.
		bindCommand(keyboard0, make, "t", to, "use(\"BackPack\");");
		bindAction(keyboard0, make, "p", TO, IDACTION_CHAT, 0);
	}
	if(%keys == "mwheel"){
		bindCommand(mouse0, zaxis0, TO, "nextWeapon();");
		bindCommand(mouse0, zaxis1, TO, "prevWeapon();");
	}
	if(%keys == "nomwheel"){
		bindCommand(mouse0, zaxis0, TO, "");
		bindCommand(mouse0, zaxis1, TO, "");
	}
	if(%keys == "ski"){
		$pref::happyjump=True;
		bindAction(keyboard0, make, "left", TO, IDACTION_MOVEUP);
		bindCommand(keyboard0, break, "left", TO, "");
		bindAction(keyboard0, make, "space", TO, IDACTION_TURNLEFT, 0.099974);
		bindAction(keyboard0, break, "space", TO, IDACTION_TURNLEFT, 0.000000);
	}
	if(%keys == "noski"){
		$pref::happyjump=False;
		bindAction(keyboard0, make, "space", TO, IDACTION_MOVEUP);
		bindCommand(keyboard0, break, "space", TO, "");
		bindAction(keyboard0, make, "left", TO, IDACTION_TURNLEFT, 0.099974);
		bindAction(keyboard0, break, "left", TO, IDACTION_TURNLEFT, 0.000000);
	}
	if(%keys == "newJump"){
		setRepackJump(True);
	}
	if(%keys == "newJumpOff"){
		setRepackJump(False);
	}
	export("pref::*", "config\\ClientPrefs.cs", False);
	saveActionMap("config\\config.cs", "actionMap.sae", "playMap.sae", "pdaMap.sae");
}

function setRepackJump(%enable, %exiting)
{
	if(%enable){
		$rpgSpaceKeyboardLockOn= true;
		popActionMap("actionMap.sae");
		popActionMap("playMap.sae");
		pushActionMap("phantomMapSpace.sae");
	}
	else {
		$rpgSpaceKeyboardLockOn= false;
		popActionMap("phantomMapSpace.sae");
		if(!%exiting){
			pushActionMap("playMap.sae");
			pushActionMap("actionMap.sae");
		}
	}
}
newActionMap("phantomMapSpace.sae");
editActionMap("phantomMapSpace.sae");
bindCommand(keyboard0, make, "space", TO, "sendControl('space');");
bindCommand(keyboard0, break, "space", TO, "sendControl('space', '', True);");

//This function lets the server change the client's viewing distance.
//It does not take away the client's ability to limit their viewing distance,
//which was/is a pretty useful thing to do for AMD video cards.
//By phantom, www.tribesrpg.org
function remoteDrawDistance(%server, %dist, %flush){
	if(%server != 2048) return;
	$serverControllingDD = True;
	//if last server-set distance is different from current
	//distance, user probably changed it, let's remember that preference
	if($pref::TVD != $pref::TerrainVisibleDistance){
		if($pref::TerrainVisibleDistance > 999)
			$pref::uTVD = 10000;//assume the user prefers highest distance
		else
			$pref::uTVD = $pref::TerrainVisibleDistance;
	}
	if($pref::uTVD < 5)
		$pref::uTVD = $pref::TerrainVisibleDistance;
	$pref::TVD = %dist;//Should always be what the server wants, never change this outside this function
	if(%dist > $pref::uTVD)//The user's preferred limit
		%dist = $pref::uTVD;
	$pref::TerrainVisibleDistance = %dist;
	if(%flush)
		flushTextureCache();
}
//Called in options.cs when user changes terrain detail
//Allows us to respect the server's draw distance changes,
//which are usually for performance or realism
//By phantom, www.tribesrpg.org
function localDrawDistance(%dist){
	if(%dist < 5)
		%dist = 1300;
	$pref::uTVD = %dist;
	if($pref::uTVD > 999)
		$pref::uTVD = 10000;
	if(!$serverControllingDD){
		$pref::TerrainVisibleDistance = %dist;
		return;
	}
	if(%dist > $pref::TVD)
		%dist = $pref::TVD;
	$pref::TerrainVisibleDistance = %dist;
}

if(String::findSubStr($modList, "crurpg") != -1){
	exec(rpggui);
	exec(crucible_functions);
}

function remotesetWindowTitle(%server, %title){
	if(%server != 2048)
		return;
	if(String::findSubStr(%code, "\"") != -1 ||
		String::findSubStr(%code, "\\") != -1)  // no quotes or escapes
		return;
	%t = $console::printlevel;
	$console::printlevel = 0;
	setWindowTitle(MainWindow, %title);
	$console::printlevel = %t;
}

//By phantom, tribesrpg.org
//Note: Does not seem to be able to change fonts on the fly.
//More experimentation needed.
//This function may change in future versions.
function remoteFontSet(%server, %set){
	if(%server != 2048) return;

	if(%set != ""){
		if(isFile("base\\"@%set)){
			deleteObject("FontsVolume");
			$currentFontSet = %set;
			newObject(FontsVolume, SimVolume, %set);
		}
	}

	remoteEval(2048, currentFontSet, $currentFontSet);
}
deleteObject("FontsVolume");
$currentFontSet = "rpgfonts.vol";
newObject(FontsVolume, SimVolume, $currentFontSet);
if(!isObject(FontsVolume)){
	$currentFontSet = "fonts.vol";
	newObject(FontsVolume, SimVolume, "fonts.vol");
}

//By phantom, tribesrpg.org
function remoteGetControlScale(%server, %control){
	if(%server != 2048) return;
	%tl = control::getposition(%control);
	%br = control::getextent(%control);
	remoteeval(2048, controlScale, %tl, %br);
}

function remoteGetResolution(%server){
	if(%server != 2048) return;
	remoteeval(2048, currentRes, Presto::ScreenSize());
}

Hudbot::addReplacement( "63266b3a", "BANKFLOOR.TGA" ); // Generic_RPG
Hudbot::addReplacement( "e74b3e70", "BANKWALL.TGA" ); // Generic_RPG
//Hudbot::addReplacement( "c0e9a8c8", "CABINET1.TGA" ); // Generic_RPG
//Hudbot::addReplacement( "f9e82699", "DOCK.TGA" ); // Generic_RPG
Hudbot::addReplacement( "a5b6036d", "DOCK3.TGA" ); // Generic_RPG
Hudbot::addReplacement( "83e60ee7", "DOCK3.TGA" ); // Generic_RPG
//Hudbot::addReplacement( "056baf47", "STONE1.TGA" ); // Generic_RPG
//Hudbot::addReplacement( "5f448011", "STONE2.TGA" ); // Generic_RPG
//Hudbot::addReplacement( "43a6f977", "STONE2A.TGA" ); // Generic_RPG
//Hudbot::addReplacement( "88efb036", "STONE3.TGA" ); // Generic_RPG
Hudbot::addReplacement( "feb96167", "WATER01.TGA" ); // Generic_RPG
Hudbot::addReplacement( "ad6046fb", "WATER01.TGA" ); // Generic_RPG
//Hudbot::addReplacement( "51de9b73", "WELLDARK.TGA" ); // Generic_RPG