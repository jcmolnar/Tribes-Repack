//----------------------------------------------------------------------------

TriggerData GroupTrigger
{
	className = "Trigger";
	rate = 1.0;
};

exec(TriggerFunctions);	// defining GroupTrigger:: functions elsewhere without datablock 
			// so we can redefine each map change
			// cause people like to mess with em... -plasmatic	
