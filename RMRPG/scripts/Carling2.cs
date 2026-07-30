//-----------------------------------------//
// Date: 17/01/2004	  			 //
// Author: Carling		  		 //
// Purpose: To log and time stamp all pks! //
//-----------------------------------------//

function pk::getTime()
{
	%str = timestamp();
	%year = String::GetSubStr(%str, 0, 4);
	%month = String::GetSubStr(%str, 5, 2);
	%day = String::GetSubStr(%str, 8, 2);
	%hour = String::GetSubStr(%str, 11, 2);
	%minute = String::GetSubStr(%str, 14, 2);
	return %hour@" "@%minute@" "@%day@" "@%month@" "@%year;
}

function pk::monitor(%client,%killerid)
{
	if(%client == -1 || %killerid == -1)
		return;

	%cname = client::getname(%client);
	%kname = client::getname(%killerid);
	%clvl = getFinalLVL(%client);
	%klvl = getFinalLVL(%killerid);

	%chk = floor( %klvl / 2 ) + 10;
	%gap = %klvl - %clvl;

	%filename = "[PK]"@%kname @ ".cs";
	%time = pk::getTime();

	$ConsoleWorld::DefaultSearchPath = $ConsoleWorld::DefaultSearchPath;

	if(%gap <= %chk)		// Check for valid PK
		return;

	if(isFile("temp\\" @ %filename))
	{
		exec(%filename);
		%i = -1;
		while($PK[%kname,%i++] != "") {}
	}
	else
	{
		echo("PK: "@%kname@" first.");
		%i = 0;
	}

	if((%i % 5) == 0)
	{
		%time = %i * 10;
		messageall(1,"NOTICE: "@%kname@" has been caged for excess PKin lowbies.");
		cage(%killerid,900);
	}
	if(%i >= 7)	// Oh O! Player has to many lowbie pks!
	{

	}
	else
	{
		$PK["["@%kname@","@%i@"]"] = %cname@" "@%time;
		export("pk["@%kname@"*", "temp\\[PK]"@%kname@".cs", true);
	}
	deleteVariables("$PK[\""@%kname@"\"*");
	deleteVariables("$PK["@%kname@"*");
}

function pk::bancheck(%client)
{
	%filename = "[BL]rmr.cs";
	if(isFile("temp\\" @ %filename))
	{
		exec(%filename);

		%i = -1;
		while($PK[%kname,%i++] != "") {}

		echo("PK: "@%kname@" again. "@%i);
	}
	else
		return false;	//not banned
}