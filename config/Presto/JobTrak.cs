// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// JobTrak.CS								Wizard_TPG, August 2000
//
//
//	Edited for MT Parser Compliency July 2000 by Wizard_TPG
//
// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// JobTrak.CS								Presto, March '99
//
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
Include("presto\\upgrade\\clientmessage.cs");
Include("presto\\Match.cs");
Include("presto\\Event.cs");
Include("presto\\Chores.cs");
Include("presto\\TeamTrak.cs");

function JobTrak::GetCurrentJob(%client)
{
	return $JobTrak::job[%client];
}

function JobTrak::SetCurrentJob(%client,%str)
{
	$JobTrak::job[%client] = %str;
	$JobTrak::icon[%client] = "";
}

function JobTrak::TrackJob(%client, %jobStr)
{
	%job = Job::GetFromCode(%jobStr, 0);
	if (%job == jobIAm || %job == jobWill)
	{
		JobTrak::SetCurrentJob(%client,%jobStr);
		Event::Trigger(eventJobUpdated, %client);
	}
}



msg::onMatch ("ttack", "jobTrak::JobAction (%client,att);");
msg::onMatch ("efend", "jobTrak::JobAction (%client,def);");
msg::onMatch ("nip", "jobTrak::JobAction (%client,snipe);");
msg::onMatch ("ilot", "jobTrak::JobAction (%client,pilot);");
msg::onMatch ("epair", "jobTrak::JobAction (%client,rep);");
msg::onMatch ("target", "jobTrak::JobAction (%client,targ);");
msg::onMatch ("eploy", "jobTrak::JobAction (%client,depl);");
msg::onMatch ("ait*for orders", "jobTrak::JobAction (%client,wait);");
msg::onMatch ("way temporarily", "jobTrak::JobAction (%client,away);");
msg::onMatch ("~job:%a*", "jobTrak::JobAction (%client,a*);","%");
function jobTrak::JobAction(%client,%jobstring)
{
	if(client::getTeam(%client) == client::getTeam(getManagerId()))
	{
		$JobTrak::JobAction = %jobstring;
		Event::Trigger(eventJobFound, %client);
	}
}


msg::onMatch (" base", "jobTrak::JobTarget (%client,base);");
msg::onMatch (" sensor", "jobTrak::JobTarget (%client,sens);");
msg::onMatch (" flag", "jobTrak::JobTarget (%client,flag);");
msg::onMatch (" generator", "jobTrak::JobTarget (%client,gen);");
msg::onMatch (" panel", "jobTrak::JobTarget (%client,gen);");
msg::onMatch ("at medium range", "jobTrak::JobTarget (%client,mid);");
msg::onMatch ("to mid-field", "jobTrak::JobTarget (%client,mid);");
msg::onMatch ("the objective", "jobTrak::JobTarget (%client,obj);");
msg::onMatch (" roof", "jobTrak::JobTarget (%client,roof);");
msg::onMatch (" station", "jobTrak::JobTarget (%client,stat);");
msg::onMatch (" turret", "jobTrak::JobTarget (%client,tur);");
msg::onMatch (" vehicle pad", "jobTrak::JobTarget (%client,vpad);");
msg::onMatch (" deployables", "jobTrak::JobTarget (%client,deps);");
function jobTrak::JobTarget(%client,%jobstring)
{
	if(client::getTeam(%client) == client::getTeam(getManagerId()))
	{
		$JobTrak::JobTarget = %jobstring;
		Event::Trigger(eventJobFound, %client);
	}
}



msg::onMatch ("an ammo station", "jobTrak::JobDeploy (%client,dammo);");
msg::onMatch ("cameras", "jobTrak::JobDeploy (%client,dcam);");
msg::onMatch ("an inventory station", "jobTrak::JobDeploy (%client,dinv);");
msg::onMatch ("sensor jammers", "jobTrak::JobDeploy (%client,djam);");
msg::onMatch ("mines", "jobTrak::JobDeploy (%client,dmin);");
msg::onMatch ("sensors", "jobTrak::JobDeploy (%client,dsen);");
msg::onMatch ("turrets", "jobTrak::JobDeploy (%client,dtur);");
msg::onMatch ("~job:%d*", "jobTrak::JobDeploy (%client,d*);","%");
function jobTrak::JobDeploy(%client,%jobstring)
{
	if(client::getTeam(%client) == client::getTeam(getManagerId()))
	{
		$JobTrak::JobDeploy = %jobstring;
		Event::Trigger(eventJobFound, %client);
	}
}



msg::onMatch ("Is anyone*?", "jobTrak::JobRequest (%client,any);");
msg::onMatch ("Can anyone*?", "jobTrak::JobRequest (%client,can);");
msg::onMatch ("Everyone should*!", "jobTrak::JobRequest (%client,every);");
msg::onMatch ("I have finished*.", "jobTrak::JobRequest (%client,fin);");
msg::onMatch ("I am*.", "jobTrak::JobRequest (%client,iam);");
msg::onMatch ("We need more people*!", "jobTrak::JobRequest (%client,more);");
msg::onMatch ("We need someone to*!", "jobTrak::JobRequest (%client,need);");
msg::onMatch ("Unable to*.", "jobTrak::JobRequest (%client,unab);");
msg::onMatch ("I will*.", "jobTrak::JobRequest (%client,will);");
function jobTrak::JobRequest(%client,%jobstring)
{
	if(client::getTeam(%client) == client::getTeam(getManagerId()))
	{
		$JobTrak::JobRequest = %jobstring;
		Event::Trigger(eventJobFound, %client);
	}
}


function JobTrak::CompileJobString(%client)
{
	if($JobTrak::ScheduleCheck == "")
		$JobTrak::ScheduleCheck = false;

	%jobStr = $JobTrak::JobRequest;
	if($JobTrak::JobAction != "")
	{
		%jobStr = %jobStr @ "-" @ $JobTrak::JobAction;
	}
	if($JobTrak::JobDeploy != "")
	{
		%jobStr = %jobStr @ "-" @ $JobTrak::JobDeploy;
	}
	if($JobTrak::JobTarget != "" && %testcode != 3)
	{
		%jobStr = %jobStr @ "-" @ $JobTrak::JobTarget;
	}

	$JobTrak::usesJobs[%client] = true;
	JobTrak::TrackJob(%client, %jobStr);

	if(!$JobTrak::ScheduleCheck)
	{
		schedule("JobTrak::ClearJobRegister();",0.1);
		$JobTrak::ScheduleCheck = true;
	}
}

function JobTrak::ClearJobRegister()
{
		$JobTrak::JobRequest = "";
		$JobTrak::JobAction = "";
		$JobTrak::JobDeploy = "";
		$JobTrak::JobTarget = "";
		$JobTrak::ScheduleCheck = false;
}

//Event::Attach(eventClientTagMessage, JobTrak::OnClientTagMessage);

function JobTrak::GetIcon(%client)
{
	%icon = $JobTrak::icon[%client];
	if (%icon != "")
		return %icon;

	%icon = Job::FollowTree(JobTrak::GetCurrentJob(%client), icon_start);
	$JobTrak::icon[%client] = %icon;
	return %icon;
}

function JobTrak::Joined(%client)
{
	JobTrak::SetCurrentJob(%client,"");
}

function JobTrak::Left(%client)
{
	JobTrak::SetCurrentJob(%client,"");
	$JobTrak::usesJobs[%client] = "";
}

Event::Attach(eventTeammateJoined, JobTrak::Joined);
Event::Attach(eventTeammateLeft, JobTrak::Left);

function JobTrak::Reset()
{
	deleteVariables("$JobTrak::*");
}

Event::Attach(eventJobFound, JobTrak::CompileJobString);
Event::Attach(eventConnected, JobTrak::Reset);
Event::Attach(eventChangeMission, JobTrak::Reset);


msg::onMatch ("Unable to complete objective", "jobTrak::unable (%client);");
function jobTrak::unable (%client)
{
	if (%client!=0)
		JobTrak::TrackJob(%client, "unab");
}


msg::onMatch ("Going offense.", "jobTrak::goingO (%client);");
function jobTrak::goingO (%client)
{
	if (%client!=0)
		JobTrak::TrackJob(%client, "iam-att");
}

msg::onMatch ("Defending our base.", "jobTrak::goingD (%client);");
function jobTrak::goingD (%client)
{
	if (%client!=0)
		JobTrak::TrackJob(%client, "iam-def");
}

msg::onMatch ("Repairing", "jobTrak::Repairing(%client);");
function jobTrak::Repairing (%client)
{
	if (%client == 0 || %client == -1)
		JobTrak::TrackJob(getManagerId(), "iam-rep");
	else
		JobTrak::TrackJob(%client, "iam-rep");
}


//Event::Attach(eventClientMessage, JobTrak::OnClientMessage);
