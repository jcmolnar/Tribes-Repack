// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// Chores.CS								Presto, March '99 
//
//	A system for tracking the actions of your teammates.
//
//	This file itself implements a new menu I call the Job Menu, which lets
//	you say very specifically what you are doing.  The Job menu works by
//	constructing sentences out of a request, an action, and a target.  At
//	least in most cases - there are some exceptions.
//
//	For instance, you can say "I am / repairing / the generator" or
//	"We need someone to / repair / the generator"  In these two cases the
//	action and target are the same but the "request" part, as I call it,
//	changed from "I am" to "We need someone to".  I tried to keep the list
//	of requests, actions, and targets to a fairly small & memorable set.
//
//	The other part of the job script is that it will send out, in messages,
//	your actions.  Not only can your teammates can see what's going on, but
//	automatic scripts can track your progess.  This status is sent as a 
//	tagged part of the chat message, which is not displayed.
//
//	There are a couple of functions you can call to make use of the Job
//	tracking yourself.
//
//		Job::MakeCode(jobIAm, jobRepair, jobGenerator);
//			This would return the code for "I am / repairing / the 
//			generator" which is "iam-rep-gen".
//	
//		Job::Do("Hey, I'm fixin the gens!", "iam-rep-gen", sayHey);
//			This sets your current job status.  It will send a
//			team message, and transmit the code to everyone.
//			The last option is a voice chat command from SAY.CS
//			and is optional- if provided it'll use the wavfile!
//
//		%job = Job::GetFromCode("iam-rep-gen", 0);
//			This decodes the short form code into a job.  The second
//			parameter is the word number (each word between the
//			dashes is a separate part of the code.)
//			In this example, you'll get back jobIAm.
//
//	Take a look at section B, below, for a list of the various jobs.
//
//	There's still some work to be done on this implementation, so it could
//	change over the next few releases.
//
//	A) The Routines
// ---------------------------------------------------------------------------
Include("presto\\Menu.cs");
Include("presto\\Say.cs");
Include("presto\\Event.cs");

function Job::GetForm(%job) {
	return $Job::job[%job, form];
	}
function Job::GetFormText(%job, %form) {
	return $Job::job[%job, form, %form];
	}
function Job::SetFormText(%job, %form, %text) {
	$Job::job[%job, form, %form] = %text;
	}
function Job::New(%type,%letter, %job,%form, %f0,%t0, %f1,%t1, %f2,%t2, %f3,%t3, %f4,%t4, %f5,%t5, %f6,%t6, %f7,%t7, %f8,%t8, %f9,%t9) {
	$Job::byLetter[%type,%letter] = %job;
	$Job::letter[%job] = %letter;
	$Job::job[%job, form] = %form;
	for (%i = 0; %f[%i] != ""; %i++)
		{
		if (%f[%i] == code)
			$Job::code[%t[%i]] = %job;
		Job::SetFormText(%job, %f[%i], %t[%i]);
		}

	if (%type == typeRequest) {
		%textStart = Job::GetFormText(%job, start);
		Menu::AddChoice("menuJob::"@%type, %letter @ %textStart @ "...", "Job::Choose("@%type@","@%job@");");
		}
	else	{
		if (%type == typeAction) {
			%textAction = Job::GetFormText(%job, active);
			Menu::AddChoice(menuJob::typeRequest, %letter @ %textAction @ "...", "Job::Choose("@%type@","@%job@");");
			}
		Menu::AddChoice("menuJob::"@%type, %letter, "Job::Choose("@%type@","@%job@");");
		}

	if (%letter == ".")
		$Job::none[%type] = %job;
	}
function Job::Tree(%state, %job, %newState, %text) {
	$Job::state[%state, %job, next] = %newState;
	$Job::state[%state, %job, text] = %text;
	}

function Job::End() {
	Menu::SetTitle(menuJob::typeRequest, "Job menu");
	Menu::AddChoice(menuJob::typeRequest, " (Report current action)", "Job::ChooseCurrent(typeRequest);");
	Menu::AddChoice(menuJob::typeAction, " (current action)", "Job::ChooseCurrent(typeAction);");
	Menu::AddChoice(menuJob::typeTarget, " (done)", "Job::Choose(typeTarget, Job::None(typeTarget));");
	Menu::AddChoice(menuJob::typeDeploy, " (done)", "Job::Choose(typeDeploy, Job::None(typeDeploy));");

	$job::key2[typeRequest] = -1;
	$job::key2[typeAction] = Menu::AddChoice(menuJob::typeAction, ";", "Job::Choose(typeAction, "@ Job::None(typeAction) @");");
	$job::key2[typeTarget] = Menu::AddChoice(menuJob::typeTarget, ";", "Job::Choose(typeTarget, "@ Job::None(typeTarget) @");");
	$job::key2[typeDeploy] = Menu::AddChoice(menuJob::typeDeploy, ";", "Job::Choose(typeDeploy, "@ Job::None(typeDeploy) @");");
	}

function Job::Type(%form, %type) {
	$Job::type[%form] = %type;
	$Job::menu[%form] = "menuJob::"@%type;
	Menu::New(Job::GetMenu(%form), "", "Job::InitMenu("@%type@", "@Job::GetMenu(%form)@");");
	}
function Job::GetMenu(%form) {
	return $Job::menu[%form];
	}
function Job::GetType(%form) {
	return $Job::type[%form];
	}
function Job::None(%type) {
	return $Job::none[%type];
	}

function Job::Start() {
	$Job::message = "";
	$Job::letters = "";
	$Job::code = "";
	$Job::currentForm = start;
	$Job::jobRequest = "";
	$Job::state = wav_start;
	$Job::say = sayNone;
	$Job::punctuation = Job::GetFormText(jobIAm, punc);
	$Job::[new, typeRequest] = $Job::[current, typeRequest];
	$Job::[new, typeAction] = $Job::[current, typeAction];
	$Job::[new, typeTarget] = $Job::[current, typeTarget];
	$Job::[new, typeDeploy] = $Job::[current, typeDeploy];
	Menu::Display(menuJob::typeRequest);
	}
function Job::GetTreeStr(%state, %job) {
	return $Job::state[%state, %job, text];
	}
function Job::WalkTree(%state, %job) {
	%state = $Job::state[%state, %job, next];
	if (%state == "")
		return end;
	return %state;
	}
function Job::FollowTree(%jobStr, %state) {
	%str = "";
	%i = 0;
	%job = Job::GetFromCode(%jobStr, %i);
	while (%job != "") {
		%w = Job::GetTreeStr(%state, %job);
		if (%w != "")
			%str = %w;
		%state = Job::WalkTree(%state, %job);
		if (%state == end)
			break;

		%i++;
		%job = Job::GetFromCode(%jobStr, %i);
		}
	return %str;
	}

function Job::AppendJob(%type, %job) {
	if (%type == typeRequest) {
		$Job::jobRequest = %job;
		$Job::punctuation = Job::GetFormText(%job, punc);
		}
	else if ($Job::jobRequest == "")
		Job::AppendJob(typeRequest, jobIAm);

	$Job::lastLetter = $Job::letter[%job];
	$Job::[new, %type] = %job;
	
	if ($Job::state != end) {
		%say = Job::GetTreeStr($Job::state, %job);
		if (%say != "")
			$Job::say = %say;

		$Job::state = Job::WalkTree($job::state, %job);
		}

	if (%job != Job::None(%type)) {
		if ($Job::message == "") {
			$Job::message = Job::GetFormText(%job, $Job::currentForm);
			$Job::code = Job::GetFormText(%job, code);
			}
		else	{
			$Job::message = $Job::message @ " " @ Job::GetFormText(%job, $Job::currentForm);
			$Job::code = $Job::code @ "-" @ Job::GetFormText(%job, code);
			}
		}
	$Job::letters = $Job::letters @ $Job::letter[%job];
	$Job::currentForm = Job::GetForm(%job);
	if ($Job::currentForm == -1)
		$Job::message = $Job::message @ $Job::punctuation;
	return $Job::currentForm;
	}
function Job::Choose(%type, %job) {
	%form = Job::AppendJob(%type, %job);
	if (%form == -1)
		Job::Do($Job::message, $Job::code, $Job::say);
	else	Menu::Display(Job::GetMenu(%form));
	}
function Job::ChooseCurrent(%type) {
	%form = Job::AppendJob(%type, $Job::[current,%type]);
	while (%form != -1) {
		if (%form == "") { echo("Job::ChooseCurrent error!!!"); return; }
		%type = Job::GetType(%form);
		%form = Job::AppendJob(%type, $Job::[current,%type]);
		}
	Job::Do($Job::message, $Job::code, $Job::say);
	}

function Job::MatchesCurrent() {
	%form = Job::GetForm($Job::[current, typeRequest]);	// so far jobIAm but that could change.
	while (%form != -1) {
		%type = Job::GetType(%form);
		if ($Job::[new, %type] == Job::None(%type))
			return true;
		%job = $Job::[current, %type];
		if ($Job::[new, %type] != %job)
			return false;
		%form = Job::GetForm(%job);
		}
	return true;
	}
function Job::GetFromCode(%code, %i) {
	%w = "";
	while (%i >= 0) {
		%idx = String::FindSubStr(%code, "-");
		if (%idx == -1) {
			%w = %code;
			%code = "";
			}
		else	{
			%w = String::GetSubStr(%code, 0,%idx);
			%code = String::GetSubStr(%code, %idx + 1, 10000);
			}
		if (%w == "")
			return "";
		%i--;
		}
	return $Job::code[%w];
	}
function Job::MakeCode(%job0,%job1,%job2,%job3,%job4,%job5,%job6, %job7,%job8,%job9) {
	%i = 0;
	while (%job[%i] != "") {
		%codeJob = Job::GetFormText(%job, code);
		if (%i == 0)
			%code = %codeJob;
		else	%code = %code @ "-" @ %codeJob;
		%i++;
		}
	return %code;
	}
function Job::Do(%message, %code, %say) {
	if (%code == "")
		return;
	if (%say == "")
		%say = sayNone;
	%request = Job::GetFromCode(%code, 0);
	if (%request == jobIAm || %request == jobWill) {
		$Job::[current, typeRequest] = jobIAm;
		$Job::[current, typeAction] = $Job::[new, typeAction];
		$Job::[current, typeTarget] = $Job::[new, typeTarget];
		$Job::[current, typeDeploy] = $Job::[new, typeDeploy];
		}
	else if (%request == jobFinished || %request == jobUnable) {
		if (Job::MatchesCurrent()) {
			Job::Reset();
			%code = %code @ "~job:iam-wait";
			}
		}
	Say::Team(%say, %message @ "~job:" @ %code);
	}

function Job::InitMenu(%type,%menu) {
	if (%type != typeRequest)	// Leave root menu alone
		Menu::SetTitle(%menu, $Job::message);
	%jobs = Menu::GetNumChoices(%menu);
	%k = $Job::key2[%type];
	%letterRepeat = $Job::lastLetter;
	for (%i = 0; %i < %jobs; %i ++) {
		%letter = String::GetSubStr(Menu::GetChoice(%menu, %i), 0,1);
		if (%i == %k)
			continue;
		if (%letter == " ")
			continue;	// leave "current" untouched.

		if (%letter == %letterRepeat)
			%letterRepeat = "";

		%job = $Job::byLetter[%type, %letter];
		if (%job == "")	// Special case, root menu has two types.
			%job = $Job::byLetter[typeAction, %letter];

		if (%letter == ".")
			%textTarget = "(done)";
		else	%textTarget = Job::GetFormText(%job, $Job::currentForm);

		if (Job::GetForm(%job) == -1)
			%textPunc = $Job::punctuation;
		else	%textPunc = "...";
		
		Menu::SetChoice(%menu, %i, %letter @ %textTarget @ %textPunc);
		}
	if (%k != -1) {
		if (%letterRepeat != "") 
			Menu::SetChoice(%menu, %k, %letterRepeat @ "(done)" @ $Job::punctuation);
		else	Menu::SetChoice(%menu, %k, ",");
		}
	}

//	B) The Jobs
// ---------------------------------------------------------------------------

//Required order: All menus, all actions, then all requests, then all targets/etc.
// this is to make sure the request menu has actions at the top.

Job::Type(start, typeRequest);
Job::Type(order, typeAction);
Job::Type(active, typeAction);
Job::Type(deploy, typeDeploy);
Job::Type(enemy, typeTarget);
Job::Type(to, typeTarget);
Job::Type(at, typeTarget);
Job::Type(friendly, typeTarget);

// format:
//	Job::New(type, letter in menu, name of job,     <form,text> pairs);
//		The type is which menu it will appear in - so far there are four types,
//		typeRequest, typeAction, typeTarget, and typeDeploy.  Later I might add more.

Job::New(typeAction,"g", jobAttack,enemy, order,"attack", active,"attacking", start,"Attacking", code,"att");
Job::New(typeAction,"d", jobDefend,friendly, order,"defend", active,"defending", start,"Defending", code,"def");
Job::New(typeAction,"l", jobSnipe,enemy, order,"snipe", active,"sniping", start,"Sniping", code,"snipe");
Job::New(typeAction,"p", jobPilot,to, order,"pilot", active,"piloting", start,"Piloting", code,"pilot");
Job::New(typeAction,"r", jobRepair,friendly, order,"repair", active,"repairing", start,"Repairing", code,"rep");
Job::New(typeAction,"t", jobTarget,enemy, order,"target", active,"targeting", start,"Targeting", code,"targ");
Job::New(typeAction,"y", jobDeploy,deploy, order,"deploy", active,"deploying", start,"Deploying", code,"depl");
Job::New(typeAction,"w", jobWait,-1, order,"wait for orders", active,"waiting for orders", start,"Waiting for orders", code,"wait");
Job::New(typeAction,"z", jobGone,-1, order,"go away temporarily", active,"away temporarily", start,"Away temporarily", code,"away");
Job::New(typeAction,".", jobNoAction,-1, enemy,"*", friendly,"*", code,"a*");

Job::New(typeRequest,"a", jobIsAnyone,active, start,"Is anyone", punc,"?", code,"any");
Job::New(typeRequest,"c", jobCanAnyone,order, start,"Can anyone", punc,"?", code,"can");
Job::New(typeRequest,"e", jobEveryone,order, start,"Everyone should", punc,"!", code,"every");
Job::New(typeRequest,"f", jobFinished,active, start,"I have finished", punc,".", code,"fin");
Job::New(typeRequest,"i", jobIAm,active, start,"I am", punc,".", code,"iam");
Job::New(typeRequest,"m", jobMore,active, start,"We need more people", punc,"!", code,"more");
Job::New(typeRequest,"n", jobNeed,order, start,"We need someone to", punc,"!", code,"need");
Job::New(typeRequest,"u", jobUnable,order, start,"Unable to", punc,".", code,"unab");
Job::New(typeRequest,"h", jobWill,order, start,"I will", punc,".", code,"will");

Job::New(typeTarget,"b", jobBase,-1, enemy,"the enemy base", friendly,"our base", to,"to the base", at,"at the base", code,"base");
Job::New(typeTarget,"e", jobSensors,-1, enemy,"the enemy's sensors", friendly,"our sensors", at,"at the sensor", to,"to the sensor", code,"sens" );
Job::New(typeTarget,"f", jobFlag,-1, enemy,"the enemy flag", friendly,"our flag", to,"to the flag", at,"at the base", code,"flag");
Job::New(typeTarget,"g", jobGenerator,-1, enemy,"the enemy generator", friendly,"our generator", to,"to the generator", at, "at the generator", code,"gen");
Job::New(typeTarget,"m", jobMidfield,-1, enemy,"at medium range", friendly,"at medium range", to, "to mid-field", at,"at medium range", code,"mid");
Job::New(typeTarget,"o", jobObjective,-1, enemy,"the objective", friendly,"the objective", to,"to the objective", at,"at the objective", code,"obj");
Job::New(typeTarget,"r", jobRoof,-1, enemy,"the enemy's roof", friendly,"our roof", to,"to the roof", at,"at the roof", code,"roof");
Job::New(typeTarget,"s", jobStations,-1, enemy,"the enemy's stations", friendly,"our stations", at,"at the station", to,"to the station", code,"stat" );
Job::New(typeTarget,"t", jobTurrets,-1, enemy,"the enemy turrets", friendly,"our turrets", to,"to the turret", at,"at the turret", code,"tur");
Job::New(typeTarget,"v", jobVehiclePad,-1, enemy,"the enemy's vehicle pad", friendly,"our vehicle pad", to,"to the vehicle pad", at,"at the vehicle pad", code,"vpad");
Job::New(typeTarget,"y", jobDeployables,-1, enemy,"the enemy's deployables", friendly,"our deployables", to, "to the deployables", at,"at the deployables", code,"deps");
Job::New(typeTarget,".", jobNoTarget,-1, enemy,"*", friendly,"*", to,"*", at,"*", code,"t*");

Job::New(typeDeploy,"a", jobAmmo,-1, deploy,"an ammo station", code,"dammo");
Job::New(typeDeploy,"c", jobCameras,-1, deploy,"cameras", code,"dcam");
Job::New(typeDeploy,"i", jobInventory,-1, deploy,"an inventory station", code,"dinv");
Job::New(typeDeploy,"j", jobJammer,-1, deploy,"sensor jammers", code,"djam");
Job::New(typeDeploy,"m", jobMines,-1, deploy,"mines", code,"dmin");
Job::New(typeDeploy,"s", jobDSensors,-1, deploy,"sensors", code,"dsen");
Job::New(typeDeploy,"t", jobDTurrets,-1, deploy,"turrets", code,"dtur");
Job::New(typeDeploy,".", jobNoDeploy,-1, deploy,"*", code,"d*");

Job::End(); // must come at end of all Job::News

// Following are state diagrams that will help figure out the wave file used for a job code.

Job::Tree(wav_start, jobCanAnyone, wav_need);
Job::Tree(wav_start, jobNeed, wav_need);
Job::Tree(wav_start, jobMore, wav_need);
Job::Tree(wav_start, jobEveryone, wav_need);
Job::Tree(wav_start, jobIAm, wav_statusAm);
Job::Tree(wav_start, jobWill, wav_statusWill);
Job::Tree(wav_start, jobUnable, end, orderNotCompleted);
Job::Tree(wav_start, jobFinished, end, orderCompleted);
Job::Tree(wav_need, jobAttack, wav_attack, attackEnemy);
Job::Tree(wav_need, jobSnipe, wav_attack, attackEnemy);
Job::Tree(wav_need, jobDefend, wav_defend, defendNeed);
Job::Tree(wav_need, jobRepair, wav_repair, needRepairs);
Job::Tree(wav_need, jobTarget, end, targetLocation);
Job::Tree(wav_statusAm, jobAttack, wav_attacking, attackGoing);
Job::Tree(wav_statusAm, jobDefend, wav_defending, defendGoing);
Job::Tree(wav_statusAm, jobWait, end, orderReady);
Job::Tree(wav_statusAm, jobTarget, end, targetAcquired);
Job::Tree(wav_statusWill, jobAttack, wav_attacking, attackGoing);
Job::Tree(wav_statusWill, jobDefend, wav_defending, defendGoing);
Job::Tree(wav_statusWill, jobWait, end, orderReady);
Job::Tree(wav_attack, jobObjective, end, objectiveAttack);
Job::Tree(wav_attack, jobFlag, end, flagGet);
Job::Tree(wav_attack, jobTurrets, end, orderDestroyTurret);
Job::Tree(wav_attack, jobGenerator, end, orderDestroyGenerator);
Job::Tree(wav_attack, jobBase, end, attackBase);
Job::Tree(wav_defend, jobObjective, end, objectiveDefend);
Job::Tree(wav_defend, jobBase, end, defendBase);
Job::Tree(wav_repair, jobObjective, end, objectiveRepair);
//Job::Tree(wav_attacking,   ?? no appropriate wavs
//Job::Tree(wav_defending,   ?? no appropriate wavs

// Following are state diagrams that will help figure out the icon representing a job code.

Job::Tree(icon_start, jobIAm, icon_status,"<B1,3:CMD_ItemActive.bmp>");
Job::Tree(icon_start, jobWill, icon_status,"<B1,3:CMD_ItemActive.bmp>");
Job::Tree(icon_status, jobAttack, icon_attack, "<B1,2:CMD_DamageHigh.bmp>");
Job::Tree(icon_status, jobSnipe, icon_attack, "<B1,2:CMD_DamageHigh.bmp>");
Job::Tree(icon_status, jobTarget, end, "");
Job::Tree(icon_status, jobDefend, icon_defend, "<B1,2:CMD_DamageLow.bmp>");
Job::Tree(icon_status, jobRepair, icon_repair, "<B1,4:M_repair.bmp>");
Job::Tree(icon_status, jobWait, end, "<B1,3:CMD_ItemInactive.bmp>");
Job::Tree(icon_status, jobAway, end, "<B1,3:CMD_ItemInactive.bmp>");
Job::Tree(icon_status, jobDeploy, icon_deploy);
Job::Tree(icon_status, jobPilot, end, "<B1,3:M_vehicle_green.bmp>");
Job::Tree(icon_deploy, jobDTurrets, end, "<B1,5:M_turret_green.bmp>");
Job::Tree(icon_deploy, jobAmmo, end, "<B1,3:M_station_green.bmp>");
Job::Tree(icon_deploy, jobInventory, end, "<B1,3:M_station_green.bmp>");
Job::Tree(icon_deploy, jobCameras, end, "<B1,3:M_camera_green.bmp>");
Job::Tree(icon_deploy, jobSensors, end, "<B1,3:M_motionsensor_green.bmp>");
Job::Tree(icon_deploy, jobJammer, end, "<B1,3:M_sensorjammer_green.bmp>");
Job::Tree(icon_repair, jobTurrets, end, "<B-2,4:M_repair.bmp><B1,5:M_turret_green.bmp>");
Job::Tree(icon_repair, jobStations, end, "<B-2,4:M_repair.bmp><B1,3:M_station_green.bmp>");
Job::Tree(icon_repair, jobDeployables, end, "<B-2,4:M_repair.bmp><B1,3:M_recon.bmp>");
Job::Tree(icon_repair, jobVehiclePad, end, "<B-2,4:M_repair.bmp><B1,3:M_vehicle_green.bmp>");
Job::Tree(icon_repair, jobGenerator, end, "<B-2,4:M_repair.bmp><B1,4:M_generator_green.bmp>");
Job::Tree(icon_defend, jobGenerator, end, "<B-1,2:CMD_DamageLow.bmp><B1,4:M_generator_green.bmp>");
Job::Tree(icon_defend, jobVehiclePad, end, "<B-1,2:CMD_DamageLow.bmp><B1,3:M_vehicle_green.bmp>");
Job::Tree(icon_defend, jobStations, end, "<B-1,2:CMD_DamageLow.bmp><B1,3:M_station_green.bmp>");
Job::Tree(icon_defend, jobTurrets, end, "<B-1,2:CMD_DamageLow.bmp><B1,5:M_turret_green.bmp>");
Job::Tree(icon_attack, jobGenerator, end, "<B-1,2:CMD_DamageHigh.bmp><B1,2:M_generator_red.bmp>");
Job::Tree(icon_attack, jobVehiclePad, end, "<B-1,2:CMD_DamageHigh.bmp><B1,1:M_vehicle_red.bmp>");
Job::Tree(icon_attack, jobStations, end, "<B-1,2:CMD_DamageHigh.bmp><B1,1:M_station_red.bmp>");
Job::Tree(icon_attack, jobTurrets, end, "<B-1,2:CMD_DamageHigh.bmp><B1,3:M_turret_red.bmp>");

function Job::Reset() {
	$Job::[current,typeRequest] = jobIAm;
	$Job::[current,typeAction] = jobWait;
	$Job::[current,typeTarget] = jobNoTarget;
	$Job::[current,typeDeploy] = jobNoDeploy;
	}
Event::Attach(eventConnected, Job::Reset);
Event::Attach(eventChangeMission, Job::Reset);
//
// C) The End
// ---------------------------------------------------------------------------