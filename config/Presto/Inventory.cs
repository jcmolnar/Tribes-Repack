// Using latest version by Writer - Mental Trousers

include ("presto\\writer\\inventory.cs");
Include("presto\\upgrade\\clientmessage.cs");

function Inventory::AccessOn(%client)
{
	if (%client != 0)
		return;
	Event::Trigger(eventEnterStation);
	return;
}

function Inventory::AccessOff(%client)
{
	if (%client != 0)
		return;
	Event::Trigger(eventExitStation);
	return;
}

//Event::Attach(eventClientMessage, Inventory::onClientMessage, inventory);
	msg::onMatch ("Station Access On", "Inventory::AccessOn(%client);");
	msg::onMatch ("Station Access Off", "Inventory::AccessOff(%client);");
