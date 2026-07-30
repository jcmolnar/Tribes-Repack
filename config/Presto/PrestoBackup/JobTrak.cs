// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// JobTrak.CS								Presto, March '99 
//
//	Job-tracking code that watches for messages from your teammates
//	and keeps track of their status.
//
//	This is the receiving end of the job-transmit code in CHORES.CS, which
//	lets you keep track of what your teammmates are working on.
//
//	You can call two functions:
//
//		%jobCode = JobTrak::GetCurrentJob(%client);
//			Returns a job code for their current activity - iam-rep-gen
//			or something like it.  See CHORES.CS for how to decode this.
//		%icon = JobTrak::GetIcon(%client);
//			Returns a string suitable for formatted text, which will
//			display a bitmap indicative of the client's current job.
//
//	At the end of this file there is a function called 
//	JobTrak::OnClientMessage -- it basically watches for client messages
//	and translates them into job notifications when possible.  I've only
//	got a few, but I'm looking for suggestions for more!  Are there any
//	common job strings that translate into "I'm doing <task>" equivalents?
//
// --------------------------------------------------------------------------
Include("presto\\Match.cs");
Include("presto\\Event.cs");
Include("presto\\Chores.cs");
Include("presto\\TeamTrak.cs");

function JobTrak::GetCurrentJob(%client) {
	return $JobTrak::job[%client];
	}
function JobTrak::SetCurrentJob(%client,%str) {
	$JobTrak::job[%client] = %str;
	$JobTrak::icon[%client] = "";
	}
function JobTrak::TrackJob(%client, %jobStr) {
	%job = Job::GetFromCode(%jobStr, 0);
	if (%job == jobIAm || %job == jobWill) {
		JobTrak::SetCurrentJob(%client,%jobStr);
		Event::Trigger(eventJobUpdated, %client);
		}
	}
function JobTrak::OnClientTagMessage(%client, %tag, %jobStr) {
	if (%tag != "job")
		return;

	$JobTrak::usesJobs[%client] = true;
	JobTrak::TrackJob(%client, %jobStr);
	}
Event::Attach(eventClientTagMessage, JobTrak::OnClientTagMessage);

function JobTrak::GetIcon(%client) {
	%icon = $JobTrak::icon[%client];
	if (%icon != "")
		return %icon;

	%icon = Job::FollowTree(JobTrak::GetCurrentJob(%client), icon_start);
	$JobTrak::icon[%client] = %icon;
	return %icon;
	}

function JobTrak::Joined(%client) {
	JobTrak::SetCurrentJob(%client,"");
	}
function JobTrak::Left(%client) {
	JobTrak::SetCurrentJob(%client,"");
	$JobTrak::usesJobs[%client] = "";
	}
Event::Attach(eventTeammateJoined, JobTrak::Joined);
Event::Attach(eventTeammateLeft, JobTrak::Left);

function JobTrak::Reset() {
	deleteVariables("$JobTrak::*");
	}
Event::Attach(eventConnected, JobTrak::Reset);
Event::Attach(eventChangeMission, JobTrak::Reset);

function JobTrak::OnClientMessage(%client, %msg) {
	if (%client == 0)
		return;

	if (%msg == "Unable to complete objective") {
		JobTrak::TrackJob(%client, "unab");
		return;
		}
	if (Match::String(%msg, "Going offense.*")) {
		JobTrak::TrackJob(%client, "iam-att");
		return;
		}
	if (%msg == "Defending our base.") {
		JobTrak::TrackJob(%client, "iam-def");
		return;
		}
	return;
	}
Event::Attach(eventClientMessage, JobTrak::OnClientMessage);