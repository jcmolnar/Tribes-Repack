// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// Event.CS			idea and original implementation by Presto, March '99
//					  current script by Vis-DS.Writer, April '99 
//
//	Event triggers, aka "generic hook points"
//
//	One of the biggest pains in loading new scripts is trying to
//	manage having multiple scripts loaded.  You have to splice them
//	yourself, because if the scripts try to do it automatically,
//	they'll probably write over each other's changes.
//
//	To try to solve this problem, I'm introducing "events", which
//	are hook points that you can introduce at arbitrary points in
//	your code.  Anyone can attaach to your event, without you having
//	to be aware of it.  When your code triggers the event, they
//	will be called.
//
//	So, events are a form of cooperation between scriptwriters.
//	You can add events to your own code, to let other people hook in.
//	Or, you can use the ones I've created in EVENTS.CS ...
//
//	Usage examples:
//		Event::Trigger(eventPresto, 1,2,3);
//			If you put a line like this in your code, then anyone
//			who has attached to the event "eventPresto" will be
//			called with the arguments 1,2,3.
//
//		Event::Attach(eventPresto, MyFunction);
//			If you add this code to your scripts, then every time
//			eventPresto is triggered, it will call MyFunction( ... )
//			with whatever parameters are passed in to Event::Trigger.
//		Event::Attach(eventPresto, "MyOtherFunction(2);");
//			If you put a statement as the callback instead of a
//			function name, it will just execute the statement.  Of
//			course you lose the parameters that would have been
//			passed in.
//
//		Event::Attach(eventPresto, MyFunction, attachPresto);
//		Event::Detach(eventPresto, attachPresto);
//			This lets you remove an attached event handler.
//			In this case I have done the same code as above, but
//			provided a an "attachment tag" so I can remove it later.
//			Even if you never actually plan on detaching your
//			event handler it's still probably a pretty good	idea 
//			to give it a tag.  Why?  Two reasons:
//			1) Someone else might want to detach your event and
//			replace it with their own (disabling your script but
//			not requiring work on the user's part)
//			2) When you re-attach an event by the same tag, it will
//			first delete the old one.  This is good for those of
//			us who are debugging - otherwise you'll get the same
//			event handler attached over and over again, multiple
//			copies running each time!
//			It can only recognize that this is the same event
//			handler if you provide an attachment name.
//			Attachment tags will only conflict with other tags on
//			the same event so it's safe to use the same tag for
//			all the different events you attach to.
//
//			*** The default attachment tag is the exact string
//			passed in as the function.  If all you want is
//			protection against re-inclusion, you don't need to
//			use a tag.  Thanks to XP-Lux for the idea. ***
//
//	Note that there is no required order to adding events or event
//	actions.  If you call Event::Attach with eventWhatever, it will just
//	sit idle until someone eventually calls Event::Trigger with
//	eventWhatever (which may never happen).  There are no bad side effects
//	from attaching to an event that never gets triggered, or triggering
//	an event which has no actions attached.
//
//	In fact if you're writing code you expect someone might want to
//	modify, it's cool of you to define some new events for them.  Just
//	call Event::Trigger where you want them to get a chance at control.
//
//	Now, sometimes you want to get information back.  But because there 
//	could be any number of actions attached, and in any order, you won't 
//	just get a single return value.  So Event::Trigger stores all the 
//	return values from the procedures...  You can then look for specific
//	return values with this code:
//
//		%returns = Event::Trigger(eventPresto, "testing");
//		if (Event::Returned(%returns, "yes"))
//			echo("someone returned yes");
//		else	echo("no one returned yes");
//
//	I haven't yet written the code to let you get each return code
//	one by one, so for now you can only tell if someone returned
//	an exact result you are looking for...  (I'll write that other code
//	soon)
//
// ---------------------------------------------------------------------------
//
//	In early April '99 Vis-DS.Writer created a new version of my Event.CS
//	script.  Since his was a definite improvement on mine, we decided that
//	it's best to upgrade everyone to his - as such, it's now a part of the
//	PrestoPack!  Past this point it's all Writer's code.
//
//	You really should check out the rest of his scripts too, visit his
//	webpage at
// 		http://www.videon.wave.ca/~writer/tribes/
//

exec("presto\\writer\\event.cs");