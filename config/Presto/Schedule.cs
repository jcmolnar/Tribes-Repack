// --   -----   -----   -----   -----   -----   -----   -----   -----   -----   -----   -----   ------
// Schedule.CS							Presto, May '99 
//
//	Safe scheduling routines which prevent infinite re-scheduling.
//
//	schedule() has two big problems:
//
//	1. If you call schedule(), you have no way to cancel the call later.
//	If the data goes out of date (for instance, you disconnect from
//	the server) the scheduled call will still happen!  This can be bad.
//
//	2. If you schedule a function which automatically reschedules itself,
//	everything works fine -- until you call that function a 2nd time, and
//	now you have *two* permanent rescheduling loops happening, and no
//	way to cancel them.
//
//	The safe scheduling routines in this module fix both of those problems.
//	Instead of
//		schedule("func();", 2);
//	use
//		Schedule::Add("func();", 2);
//
//	Later, you can call
//		Schedule::Cancel("func();");
//	if you want to cancel the scheduled call.  Of course, if it already ran,
//	the cancel will do nothing!
//
//	NOTE 1!  If you call
//		Schedule::Add("func();", 2);
//	again before the first one is executed, the previous schedule will 
//	automatically be cancelled!  This prevents over-scheduling, but can get 
//	in the way in the case that you actually want to schedule the same 
//	function to be called multiple times.
//
//	So, if you want to schedule the same function multiple times, you must
//	provide a third argument, which is a unique string that overrides the
//	function as the "name" of the schedule.  Example:
//		Schedule::Add("func();", 2, tag1);
//		Schedule::Add("func();", 7, tag2);
//		Schedule::Add("func();", 10, tag3);
//		Schedule::Cancel(tag2);
//	In this case, trying to cancel "func();" would do nothing, because each
//	call was given an overriding name.
//
//	NOTE 2!  You also probably want to provide a tag in the case where you
//	are protecting a scheduled function to which you pass arguments.  ie
//		Schedule::Add("func(1);", 1);
//		Schedule::Add("func(10);", 1);
//	This is because "func(1);" and "func(10);" are considered different 
//	and adding one won't cancel the other.  If you instead code it as
//		Schedule::Add("func(1);", 1, func);
//		Schedule::Add("func(10);", 1, func);
//	the 2nd one will correctly cancel the first.  And of course you can
//		Schedule::Cancel(func);
//	as well, just like the previous paragraph.
//
//	Okay, so why all this big deal over schedules?  Well, see if you can
//	spot the bug in the following code:
//		function myTimer() {
//			echo("timer was called!");
//			schedule("myTimer();", 1);
//			}
//		Event::Attach(eventConnected, myTimer);
//	Look what will happen!  *Every* time you connect to a server, a new
//	schedule will be called.  After you've connected to a second server,
//	myTimer() will actually be called twice every second instead of once
//	as expected.  I'm sure you can imagine examples when this would be a
//	big problem.  Simply changing it to this will make it work:
//		function myTimer() {
//			echo("timer was called!");
//			Schedule::Add("myTimer();", 1);
//			}
//		Event::Attach(eventConnected, myTimer);
//	(To the nitpicky: this could still result in one extra call to myTimer()
//	at the time the event is triggered, but the scheduling IS safe, if you 
//	care about this you can probably see why it would happen)
//
// ---------------------------------------------------------------------------

function Schedule::Add(%eval, %time, %tag) {
	if (%tag == "")
		%tag = %eval;	// Use function as tag if none provided.
	$Schedule::ID[%tag]++;
	$Schedule::eval[%tag] = %eval;
	schedule("Schedule::Exec(\""@escapestring(%tag)@"\", "@$Schedule::ID[%tag]@");", %time);
	}
function Schedule::Exec(%tag, %id) {
	if ($Schedule::ID[%tag] != %id)
		return;
	%eval = $Schedule::eval[%tag];
	Schedule::Cancel(%tag);
	eval(%eval);
	}
function Schedule::Cancel(%tag) {
	$Schedule::ID[%tag]++;
	$Schedule::eval[%tag] = "";
	}
// ---------------------------------------------------------------------------
