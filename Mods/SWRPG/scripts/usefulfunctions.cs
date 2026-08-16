$Pi = "3.14159";
function whattimeisit() { echo(getsimtime()); }
function timeleft2() { return ($Server::timeLimit * 60) + $missionStartTime - getSimTime(); }

function decho(%msg)
{
	if($decho)
		echo(%msg);
}

function GenerateShopIndexes()
{
	for(%i = 0; %i < %max; %i++)
	{
		%item = getItemData(%i);
		if(String::getSubStr(%item, String::len(%item), 0) != 0)
		{
			$AccessoryVar["[\"" @ %item @ "\", $ShopIndex]"] = %i;
			export("AccessoryVar[\"" @ %item @ "\",*", "temp\\SWRPG_Shopping.cs", true);
		}
	}
}
function getdamagepercent(%object)
{
	return ((%object.MaxDamage / 100) * (GameBase::getDamageLevel(%object) / 100));
}

function GetFullName(%shortname)
{
	if(getclientbyname(%shortname) != false)
		return %shortname;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		%fullname = Client::getName(%cl);
		if(String::findSubStr(%fullname, %shortname) != -1)
			return %fullname;
	}
	if(ai::getid(%shortname) != false)
		return %shortname;
	return -1;
}

function ClientFromName(%name)
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		%cname = Client::getName(%cl);
		if(String::findSubStr(%cname, %name) != -1)
			return %cl;
	}
	if(ai::getid(%name) != false || getclientbyname(%name) != false)
		return getclientbyname(%name);
	return -1;
}

function ClientFromShortName(%name)
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		%cname = Client::getName(%cl);
		if(String::findSubStr(%cname, %name) != -1)
			return %cl;
	}
	if(ai::getid(%name) != false || getclientbyname(%name) != false)
		return getclientbyname(%name);
	return -1;
}

function Client::GetPlayer(%clientId)
{
	return client::getOwnedObject(%clientId);
}

function randomItems(%num, %an0, %an1, %an2, %an3, %an4, %an5, %an6)
{ //radnomitems function from player.cs
	return %an[floor(getRandom() * (%num - 0.01))];
} //I disliked the mistake in 'random'..

function csl()
{
	%Count = 0;
	%script = File::findFirst("*.cs");
	while (%script != "")
	{
		%Count++;
		%script = File::findNext("*.cs");
		which(%script);
	}
	echo("Count: " @ %count);
}

function csl2(%clientId)
{
	%Count = 0;
	%script = File::findFirst("*.cs");
	while (%script != "")
	{
		%Count++;
		%script = File::findNext("*.cs");
		client::sendMessage(%clientId, 0, which(%script));
	}
	client::sendMessage(%clientId, 0, "Count: " @ %count);
}

function Kick(%clientId, %msg)
{
	schedule("net::kick(" @ %clientId @ ", \"" @ %msg @ "\");", 0.2);
}

function pl(){lp();}
function lp()
{
	echo();
	%x = 0;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		%x++;
		echo("cl: " @ %cl @ ", pl: " @ client::getOwnedObject(%cl) @ ", lvl: " @ fetchData(%cl, LVL) @ "(" @ fetchData(%cl, RemortStep) @ "), Player(" @ client::getTeam(%cl) @"): " @ Client::getName(%cl)); // @ ", logins: " @ %cl.NumLogins);
	}
	echo();
	echo("Server: " @ $numconnections @ " connections since last restart");
	echo("Server: " @ %x @ " players in total");
	echo();
}
function oldlp()
{
	%n = 0;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		%n++;
		echo("cl: " @ %cl @ ", pl: " @ client::getOwnedObject(%cl) @ ", Player(" @ client::getTeam(%cl) @"): " @ Client::getName(%cl));
	}
	echo("Server: " @ %n @ " players");
}

function nl(){ln();}
function ln()
{
	echo();
	%i = 1;
	while($CNL[%i] != "")
	//for(%i = 1; $CNL[%i] != ""; %i++)
		echo(%i @ ": " @ $CNL[%i]);
	echo();
	echo("Total connections:" @ $numconnections);
	echo();
}

function Vector::multiply2(%vec1, %n)
{
    %vecX = getWord(%vec, 0);
    %vecY = getWord(%vec, 1);
    %vecZ = getWord(%vec, 2);

    %vec2X = %vecX * %n;
    %vec2Y = %vecY * %n;
    %vec2Z = %vecZ * %n;

    return %vec2X @ " " @ %vec2Y @ " " @ %vec2Z;
}


function msgt(%msg) { topprintall("<jc>" @ %msg); }
function msgc(%msg) { centerprintall("<jc>" @ %msg); }
function msgb(%msg) { bottomprintall("<jc>" @ %msg); }

// msg("omg hi 2 u!", 2);
// or if you don't wish to specify a color
// msg("omg hi back 2 u!");
function m(%msg, %c)
{
	if(%c == -1 || %c == "")
	%c = 2;
	messageall(%c, $HosterName @ ": " @ %msg);
	//echo("SERVER: \"" @ $HosterName @ ": " @  %msg @ "\"");
}//Quicker for hosts to type into the dedicated server console.

function cmsg(%cl, %msg, %c)
{
if(%c == -1 || %c == "")
%c = 0;
client::sendMessage(%cl, %c, $n @ ": " @ %msg);
}

function msgn(%n, %msg, %c){nmsg(%n, %msg, %c);}
function nmsg(%n, %msg, %c)
{
if(%c == -1 || %c == "")
%c = 0;
client::sendMessage(ClientFromName(GetFullName(%n)), %c, $HosterName @ ": " @ %msg);
}

function pmsg(%n, %msg, %t){pose(%n, %msg, %t);}
function pose(%n, %msg, %t)
{
	if(%t == "")
		%t = false;
	remoteSay(ClientFromName(GetFullName(%n)), %t, %msg);
}
