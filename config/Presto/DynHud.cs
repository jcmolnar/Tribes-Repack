// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// DynHUD.CS								Presto, March '99 
//								originally by KillerBunny
//
//	The DynHUD is a popup HUD which keeps track of the flags in a CTF
//	game.
//
//	DYNHUD.CS is an example of using my generic HUD code.
//	I did not originally write this code.  That credit goes to KillerBunny,
//	whose generous release of the dyn_HUD.cs source enabled my generic
//	HUDs to exist in the fist place.  I just converted his code to use
//	my HUD functions.
//
//	So, I've left the HUD called DynHud even though I don't know what it
//	means!
//
// >>>>>>>	You have to bind a key to "HUD::Display(hudDyn);" somewhere in 
// >>>>>>>	your presto\autoexec.cs file.
//
//	I have added Kill-tracking into the DynHUD - the HUD tracks three
//	things:
//		* Number of painful deaths you've suffered
//		* Number of victorious kills you've inflicted
//		* Number of kills by each type of weapon
//		Dyn::ShowKills(%weapon); picks which weapon you'd like to see
//		in the HUD.
//		When you make a kill with a weapon, the HUD automatically
//		will display that weapon's # of kills.
//
// ---------------------------------------------------------------------------
Include("presto\\Event.cs");
Include("presto\\HUD.cs");
Include("presto\\TeamTrak.cs");
Include("presto\\KillTrak.cs");

// ======================================
// Dyn routines
// ======================================

function pluralize(%num, %word) {
	if (%num == 1)
		return "1 "@%word;
	return %num @ " "@%word@"s";
	}
function Dyn::UpdateHUD() {
	HUD::AddTextLine(hudDyn, pluralize($Dyn::KillCount,"kill")@", "@pluralize($Dyn::DeathCount,"death"));

	%kills = $Dyn::WeaponKills[$Dyn::WeaponKills];
	if (%kills == "")
		%kills = "0";
	if ($Dyn::WeaponKills != "")
		HUD::AddTextLine(hudDyn, pluralize(%kills, getWord($Dyn::WeaponKills,0)@" kill"));
	else	HUD::AddTextLine(hudDyn);

	%teamFlag = Team::GetFlagLocation(Team::Friendly());
	%enemyFlag = Team::GetFlagLocation(Team::Enemy());

	if (%teamFlag == $Trak::locationHome)
		HUD::AddTextLine(hudDyn, "<f1>Your flag: home");
	else if (%teamFlag == $Trak::locationField)
		HUD::AddTextLine(hudDyn, "<f1>Your flag: dropped");
	else if (%teamFlag == "")
		HUD::AddTextLine(hudDyn, "<f1>Your flag: home?");
	else	HUD::AddTextLine(hudDyn, "<f1>Kill:<f2> " @ %teamFlag);

	if (%enemyFlag == $Trak::locationHome)
		HUD::AddTextLine(hudDyn, "<f1>Enemy's: home");
	else if (%enemyFlag == $Trak::locationField)
		HUD::AddTextLine(hudDyn, "<f1>Enemy's: dropped");
	else if (%enemyFlag == "")
		HUD::AddTextLine(hudDyn, "<f1>Enemy's: home?");
	else	{
		if (%enemyFlag == client::getname(getManagerId()))
			HUD::AddTextLine(hudDyn, "<f2>You have theirs!");
		else	HUD::AddTextLine(hudDyn, "<f1>Help:<f2> " @ %enemyFlag);
		}

	if (%teamFlag == $Trak::locationHome || %teamFlag == "")
		HUD::AddText(hudDyn, "<B0,4:flag_atbase.bmp><L9>");
	else
		HUD::AddText(hudDyn, "<B0,4:flag_enemycaptured.bmp><L9>");

	HUD::AddTextLine(hudDyn, "<f1>Mines: <f2>" @ getItemCount("Mine"));
	HUD::AddTextLine(hudDyn, "<f1>Grens: <f2>" @ getItemCount("Grenade"));
	if (getItemCount("Repair Kit") > 0)
		HUD::AddTextLine(hudDyn, "<f1>Repair: <f2>Y");
	else	HUD::AddTextLine(hudDyn, "<f1>Repair: <f2>N");
	return 2;	// 2 second delay before automatically calling back
}

function Dyn::ShowKills(%weapon) {
	$Dyn::WeaponKills = %weapon;
	HUD::Update(hudDyn);
	}

function Dyn::Killed(%killer, %victim, %weapon) {
	%me = getManagerId();
	if (%victim == %me) {
		$Dyn::DeathCount++;
		HUD::Update(hudDyn);
		return;
		}
	if (%killer == %me) {
		$Dyn::KillCount++;

		$Dyn::WeaponKills[%weapon]++;
		Dyn::ShowKills(%weapon);
		return;
		}
	}

function Dyn::Reset() {
	$Dyn::KillCount = 0;
	$Dyn::DeathCount = 0;
	deleteVariables("$Dyn::WeaponKills*");
	HUD::Update(hudDyn);
	}

function DynHud::Init() {
	HUD::New(hudDyn, Dyn::UpdateHUD, $PrestoPref::DynHudPosition);
	HUD::Display(hudDyn, Presto::Enabled(DynHudDisplayOnEntry));

	// Event methods
	Event::Attach(eventFlagsUpdated, "HUD::Update(hudDyn);");
	Event::Attach(eventConnected, Dyn::Reset);
	Event::Attach(eventChangeMission, Dyn::Reset);
	Event::Attach(eventKillTrak, Dyn::Killed);
	}

if (Presto::Enabled(DynHud))
	DynHUD::Init();