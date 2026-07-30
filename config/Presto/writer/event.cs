// ---------------------------------------------------------------------------
// event.cs -- Version 1.3 -- April 7, 1999
// by Vis-DS.Writer (Lorne Laliberte, writer@videon.wave.ca)
//
// This script includes material from other scripts or is based upon an
// existing script by another author.
//
// http://www.videon.wave.ca/~writer/tribes/
// ---------------------------------------------------------------------------

//
// Attach a function to an event
//
// Returns true if function is added
// Returns false if function was already attached
//
function Event::Attach(%event, %function)
{
    // We will add this function only if it isn't already attached to this event
    if(!$Event::isAttached[%event, %function])
    {
        // Increment count of functions attached to this event.
        // Since we'll be adding an element to the end of the $Event::function array, we
        // can use the same value for the index into our $Event::function array.
        // Storing that index in our $Event::index array makes it easier to
        // quickly determine which element to access in the $Event::function array 
        // when we only know the event and the function.  (Otherwise we'd have to
        // iterate through the $Event::function array looking for a match.)
        %index = $Event::index[%event, %function] = $Event::count[%event]++;

        // Add this function to the event
        $Event::function[%event, %index] = %function;

        // Set flag to show this function is attached
        $Event::isAttached[%event, %function]++;

        // Set flag if this function is a statement, clear it if it's just a function name
    	$Event::isStatement[%event, %index] = (String::FindSubStr(%function, ";") != -1);

        // If this is a new event, add it to the master event list
        if(!Event::Exists(%event))
        {
            $Event::list[$Event::totalEvents++] = %event;
        }

        return true;
    }
    else
    {
        // We may want to test to see if our function was attached...if nothing else
        // this allows a script to know whether it's the first time it tried to attach
        // the function :)
        return false;
    }
}

//
// Detach a function from an event -- note that the order of the attached functions
// is not preserved
//
// Returns true if function was attached
// Returns false if function was not attached
//
function Event::Detach(%event, %function)
{
    // Return false if function wasn't attached, might be useful
    if(!$Event::isAttached[%event, %function])
        return false;

    // Clear flag to show this function isn't attached
    $Event::isAttached[%event, %function] = 0;

    // Move the last function in the event's function array into
    // the empty array element left behind by the detached function
    // Note that this does NOT preserve the order of the attached functions!
    $Event::function[%event, $Event::index[%event, %function]] = $Event::function[%event, $Event::count[%event]];
    
    // Decrement the count of functions attached to this event
    $Event::count[%event]--;

    // Not really necessary, but just in case something weird happens,
    // don't let the count fall below below zero
    if($Event::count[%event] < 0)
        $Event::count[%event] = 0;
        
    // Return true to say "function was attached, we detached it" :)
    return true;
}

//
// Call all the functions attached to an event and pass from 0 to 9
// parameters to each function
//
// Note:  Use Event::Trigger(<event name>, <parameters to pass>) wherever
// you want an event that functions can be attached to
//
// Returns the event to make Event::Returned calls Presto pack compatible
//
function Event::Trigger(%event, %p0, %p1, %p2, %p3, %p4, %p5, %p6, %p7, %p8, %p9)
{
    // Call every attached function in turn
    for(%i = 1; %i <= $Event::count[%event]; %i++)
    {
        %function = $Event::function[%event, %i];
        if($Event::isStatement[%event, %i])
        {
            // This function is a statement so don't pass any parameters
            // Note that we store the return value in two different arrays so we can retrieve it
            // by index or by function
            $Event::return[%event, %i] = $Event::return[%event, %function] = eval(%function);
        }
        else
        {
            // This function is not a statement so pass the parameters to it
            // Note that we store the return value in two different arrays so we can retrieve it
            // by index or by function
            $Event::return[%event, %i] = $Event::return[%event, %function] = eval(%function @ "(%p0,%p1,%p2,%p3,%p4,%p5,%p6,%p7,%p8,%p9);" );
        }
    }
    return %event; // makes my Event::Returned() function Presto pack compatible :)
}


//
// Call all the functions attached to an event, passing from 0 to 9
// parameters to each function, and stopping at the first function that
// returns %value
//
// Returns true if an attached function returned %value, otherwise returns false
//
function Event::TriggerUntil(%value, %event, %p0, %p1, %p2, %p3, %p4, %p5, %p6, %p7, %p8, %p9)
{
    // Call every attached function in turn
    for(%i = 1; %i <= $Event::count[%event]; %i++)
    {
        %function = $Event::function[%event, %i];
        %return = "";
        if($Event::isStatement[%event, %i])
        {
            // This function is a statement so don't pass any parameters
            // Note that we store the return value in two different arrays so we can retrieve it
            // by index or by function
            %return = $Event::return[%event, %i] = $Event::return[%event, %function] = eval(%function);
        }
        else
        {
            // This function is not a statement so pass the parameters to it
            // Note that we store the return value in two different arrays so we can retrieve it
            // by index or by function
            %return = $Event::return[%event, %i] = $Event::return[%event, %function] = eval(%function @ "(%p0,%p1,%p2,%p3,%p4,%p5,%p6,%p7,%p8,%p9);" );
        }

        // Stop at first function whose return value matches %value
        if (%return == %value)
            return true;
    }
    return false;
}

//
// Check to see if a given event exists (i.e. at least one function has been
// attached to the event)
//
function Event::Exists(%event)
{
    for(%i = 1; %i <= $Event::totalEvents; %i++)
    {
        if(%event == $Event::list[%i])
            return true;
    }
    return false;
}

//
// Returns the number of functions attached to an event,
// or the number of events that exist
//
function Event::GetCount(%event) 
{
    if(%event = "")
        return $Event::totalEvents;
    else
    	return $Event::count[%event];
}

//
// Check to see if a specific return value was among the values returned
// by all the functions attached to an event
//
function Event::Returned(%event, %target_return_value)
{
    for( %i = 1; %i <= $Event::count[%event]; %i++)
    {
        // Compare the return value with our target
        if($Event::return[%event, %i] == %target_return_value) {
		echo(%event,"->",$Event::function[%event, %i]," returned ",%target_return_value);
            return true; // match found
		}
    }
    return false; // match not found
}

//
// Check how many functions attached to an event returned a specific return value
//
function Event::countMatchingReturns(%event, %target_return_value)
{
    %found = 0;
    for( %i = 1; %i <= $Event::count[%event]; %i++)
    {
        // Compare the return value with our target
        if($Event::return[%event, %i] == %target_return_value)
            %found++; // another match found
    }
    return %found; // return number of matching return values
}

//
// Get the value returned from a specific function attached to an event
//
function Event::getReturn(%event, %function)
{
    return $Event::return[%event, %function];
}

// Include the default events:
Include("presto\\Events.cs");
