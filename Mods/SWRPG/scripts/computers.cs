//Begain:  11:36 PM, Friday October 19th, 2007
//So, eh. Yeah. Watching comedy, and it's 11:59 now. I ought to go to bed, but I'll start the first function. *Goes to  check what he called it*
// kay, so I started more than one function.

// When I add the PDA, make sure to add the item to the check in remote.cs under remoteCommandMenu

function Computer::Initialize(%clientId, %computer)
{
		%clientId.computer = %computer;
		Computer::Root(%clientId);
		Computer::RootText(%clientId, %computer);
		client::sendMessage(%clientId, 0, "~wcomp_on.wav");
}

function Computer::Root(%clientId)
{
		Client::buildMenu(%clientId, "Enter Command:", "computer", true);
		Client::addMenuItem(%clientId, "1Send Messages", "smsg");
		Client::addMenuItem(%clientId, "2View Messages", "vmsg");
		Client::addMenuItem(%clientId, "3Bounties", "bnty");
		Client::addMenuItem(%clientId, "4Records", "rcrd");
		Client::addMenuItem(%clientId, "5Information", "help");
		Client::addMenuItem(%clientId, "6GPS", "gps"); //make this only in the portable?
		Client::addMenuItem(%clientId, "xClose program", "close");
}
//consider making roottext into an if check for %text in root?
function Computer::RootText(%clientId)
{
		if(!(%clientId.computer > 0)) %word = "Portable, version 1"; // 0 = portable computer.
		else %word = "version 1";
		bottomprint(%clientId, " HazOS " @ %word @ ". Root: \n"
				@"\n 1. View received messages"
				@"\n 2. Send a new message"
				@"\n 3. View bounties"
				@"\n 4. View Swoop and Pod racing records"
				@"\n 5. System information"
				@"\n 6. Access the Global Positioning Satelite network"
				@"\n x. Close program", 60, 1);
}

function processMenucomputer(%clientId, %option)
{
	bottomprint(%clientId, "", 1);
	if(%option == "smsg")
		Computer::NewMessage(%clientId);
	else if(%option == "vmsg")
		Computer::ViewMessages(%clientId);
	else if(%option == "bnty")
		processMenuCMType(%clientId, "medic 0");
		//Computer::Bounties(%clientId);
	else if(%option == "rcrd")
		Computer::Records(%clientId);
	else if(%option == "help")
	{
		bottomprint(%clientId, "These computer terminals allow you to send and receive messages, even to people offline. Kind of like e-mail. You'll also be able to do other things.. Sometime. Gimme ideas if you have any, btw. ^_^", 10);
		schedule("Computer::RootText(" @ %clientId @ ");", 10);
		Computer::Root(%clientId);
	}
	else if(%option == "gps")
	{
		Client::clearItemShopping(%clientId);
		Client::clearItemBuying(%clientId);
		ClearCurrentShopVars(%clientId);
		%clientId.computer = "";
		remoteSCOM(%clientId, -1);
		Client::setGuiMode(%clientId, $GuiModeCommand);
	}
	else if(%option == "close"){}//closing events? I dunno.
}

function Computer::NewMessage(%clientId)
{
client::sendMessage(%clientId, 0, "Messaging not yet added.");
Client::cancelMenu(%clientId);
}

function Computer::ViewMessages(%clientId)
{
client::sendMessage(%clientId, 0, "Messaging not yet added.");
Client::cancelMenu(%clientId);
}

function Computer::Records(%clientId)
{
client::sendMessage(%clientId, 0, "Records not yet added.");
Client::cancelMenu(%clientId);
}

$MTitle[client::getName(%clientId), %i] = "Meep"; //What was this about? lol
$MPage[client::getName(%clientId), %i, %p] = "Woot";

function Computer::Bounties(%clientId)
{
	Client::buildMenu(%clientId, "Select a name:", "bounties", true);
	%highest = -1;
	%list = GetPlayerIdList();
	for(%i = 0; (%id = GetWord(%list, %i)) != -1; %i++)
	{
		Client::addMenuItem(%clientId, %i @ " " @ (%n = client::getName(%id)), %n);
		if(%i == 6)
		{
			Client::addMenuItem(%clientId, %i @ " " @ (%n = client::getName(%id)), %n);
		}
	}
	Client::addMenuItem(%clientId, "xClose program", "close");
}

function processMenubounties(%clientId, %name)
{
	if((%b = fetchData(%id, "bounty")) == "")
	{
		storeData(%id, "bounty", 0);
		%b = 0;
	}

}

function processMenuCMType(%clientId, %options)
{
	%curItem = 0;
	%option = getWord(%options, 0);
	%first = getWord(%options, 1);

	Client::buildMenu(%clientId, "Select a player:", "cmission", true);
   
	for(%i = 0; (%misIndex = getWord(GetPlayerIdList(), %first + %i)) != -1; %i++)
	{
		if(%i > 6)
		{
			Client::addMenuItem(%clientId, %i+1 @ "More bounties...", "more " @ %first + %i @ " " @ %option);
			break;
		}
		Client::addMenuItem(%clientId, %i+1 @ client::getName(%misIndex), %misIndex @ " " @ %option);
	}
}

function processMenuCMission(%clientId, %option)
{
	if(getWord(%option, 0) == "more")
	{
		%first = getWord(%option, 1);
		%type = getWord(%option, 2);
		processMenuCMType(%clientId, %type @ " " @ %first);
		return;
	}
	%mi = getWord(%option, 0);
	%mt = getWord(%option, 1);

	%misName = client::getName(%mi);
	//%misType = $MLIST::Type[%mt];

	// verify that this is a valid mission:

	for(%i = 0; true; %i++)
	{
		%misIndex = getWord(GetPlayerIdList(), %i);
		if(%misIndex == %mi)
			break;
		if(%misIndex == -1)
			return;
	}
	if((%b = fetchData(%id, "bounty")) == "")
	{
		storeData(%id, "bounty", 0);
		%b = 0;
	}
	bottomprint(%clientId, " HazOS v.01 Alpha Root: \n", 6);
}