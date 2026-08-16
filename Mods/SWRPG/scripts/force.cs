
//-- NOTES FOR MODDERS ---------------------------------------------------------------------------------------

// I will use the words spell, power, and force power interchangeably.
// Why? Because in the original RPG mod, and in too many areas in the code to bother
// changing, they're called spells

//For the spell code inside the cast functions, where checking if %index = X, go with:
// if(%index == $spell::index[keyword])
// 'cause we'll actually remember the keyword for our spell/power, whereas a
// number is more difficult.

//$si = 0; //start the spell index counter at 0. The first spell's index will be 1,
// due to the $si++ in the variable sets of every spell.

// $Spell::ticks
// A tick occurs every two seconds. So if your $spell::ticks is "1", it'll occur
// at the next tick which occurs after one second has passed, so theoretically it
// could last anywhere between exactly one second and 2.<infinite of nines>
// That is, if there is a tick at 2:17:30 and you cast a spell at 2:17:30.5, then
// the tick at 2:17:32 is where your spell will actually end, not at 2:17:31.5
// Why? Spell effects are in a loop that recurs every 2 seconds, which also
// includes zone checking and other bonus/special states, etc.

// Most powers have an $Spell::animation[%x] variable in them. This is the animation
// to be played upon using the power.
// For a full list of animations, see any *armors.cs file, and look at a playerdata.
// Here is a list of ones which are likely to be used. For the rest, see any *armordata.cs file
// 38 = the flail your arms and shout 'over here!' thing.
// 39 = point/"move out of the way!" (it defaults to this if no animation is specified.)
// 40 = "retreat", turn your head back and motion to move backward.
// 41 = raise hand/"stop!"
// 42 = salute
// 46 = the slap a clip into your gun kind of thing. .. not really sure what to call it. See the "How'd that feel?" taunt.
// 47 = shake fist (kinda like the pointing one, but your arm waves a little. I like it for many powers)
// 50 = wave, as in "Hi" or "Bye."

//-------------------------------------------------------------------------------------------------------------
//Spell selection menu. Builds a list of the spells you can use, and lets you select one for the cast key.

function processMenuPowerMenu(%clientId, %option)
{
	Client::buildMenu(%clientId, "Select a power:", "PowerMenu", true);

	for(%i = 1; %i < $si; %i++)
	{
		if(SkillCanUse(%clientId, $Spell::keyword[%i]))
			Client::addMenuItem(%clientId, %curitem++ @ $Spell::name[%i], $Spell::keyword[%i]);
		if(%i > 7)
		{
			Client::addMenuItem(%clientId, "nNext >>", "more " @ %list);
			break;
		}
	}
	Client::addMenuItem(%clientId, "xBack to inventory menu..." , "back");
}

function processMenuSelectPower(%clientId, %option)
{
	$ClientData[%clientId, "SelectedSpell"] = %option;
}

function MenuFP(%clientId, %page)
{
	dbecho($dbechoMode, "MenuSP(" @ %clientId @ ", " @ %page @ ")");
	
	Client::buildMenu(%clientId, "Select a power:", "FP", true);

	%clientId.bulkNum = "";
	
	%l = 6;
	%ns = $si;
	%np = floor(%ns / %l);
	
	%lb = (%page * %l) - (%l-1);
	%ub = %lb + (%l-1);
	if(%ub > %ns)
		%ub = %ns;
	echo("%page",%page," ","$si:",$si," ","%np:",%np," ","%lb",%lb," ","%ub",%ub);
	for(%i = %lb; %i <= %ub && %l < 7; %i++)
	//for(%i = %lb; %l < 7; %i++)
	{
		if($Spell::keyword[%i] != "" && %i > 0)
			if(SkillCanUse(%clientId, $Spell::keyword[%i]) == True)
			{
				%l++;
				Client::addMenuItem(%clientId, %cnt++ @ $Spell::keyword[%i] @ "0o", %i @ " " @ %page);
				echo(%i);
			}
			else
				%i--;
		else if($Spell::keyword[%i] == "" && %i > 0)
			break;
	}

	if(%page == 1)
	{
		Client::addMenuItem(%clientId, "nNext >>", "page " @ %page+1);
		Client::addMenuItem(%clientId, "xDone", "done");
	}
	else if(%page == %np)//+1)
	{
		Client::addMenuItem(%clientId, "p<< Prev", "page " @ %page-1);
		Client::addMenuItem(%clientId, "xDone", "done");
	}
	else
	{
		Client::addMenuItem(%clientId, "nNext >>", "page " @ %page+1);
		Client::addMenuItem(%clientId, "p<< Prev", "page " @ %page-1);
	}

	return;
}
function processMenuFP(%clientId, %opt)
{
	dbecho($dbechoMode, "processMenusp(" @ %clientId @ ", " @ %opt @ ")");
echo("opt: ",%opt);
	%o = GetWord(%opt, 0); echo("can use:",SkillCanUse(%clientId, $Spell::keyword[%o]));
	%p = GetWord(%opt, 1);

	if(%o != "page" && %o != "done")
		if(SkillCanUse(%clientId, $Spell::keyword[%o]))
			$ClientData[%clientId, "SelectedSpell"] = $Spell::keyword[%o];
		else
			client::sendMessage(%clientId, 1, "You can't use that power!");

	if(%o != "done")
		MenuFP(%clientId, %p);
}

function CycleFPUp(%clientId)
{
	%s = $ClientData[%clientId, "SelectedSpell"];

	for(%i = $Spell::index[%s] + 1; $Spell::keyword[%i] != ""; %i++)
	{
		if(SkillCanUse(%clientId, $Spell::keyword[%i]) == true)
		{
			$ClientData[%clientId, "SelectedSpell"] = $Spell::keyword[%i];
			break;
		}
	}
	bottomprint(%clientId, "You have selected: " @ $Spell::name[%i] @ ".");
}

function CycleFPDown(%clientId)
{
	%s = $ClientData[%clientId, "SelectedSpell"];

	for(%i = $Spell::index[%s] - 1; $Spell::keyword[%i] != ""; %i--)
	{
		if(SkillCanUse(%clientId, $Spell::keyword[%i]) == true)
		{
			$ClientData[%clientId, "SelectedSpell"] = $Spell::keyword[%i];
			break;
		}
	}
	bottomprint(%clientId, "You have selected: " @ $Spell::name[%i] @ ".");
}
//-------------------------------------------------------------------------------------------------------------

function BeginCastSpell(%clientId, %keyword)
{
	dbecho($dbechoMode, "BeginCastSpell(" @ %clientId @ ", " @ %keyword @ ")");

	%w1 = GetWord(%keyword, 0);
	%w2 = String::getSubStr(%keyword, String::len(%w1)+1, 99999);

	for(%i = 1; $Spell::keyword[%i] != ""; %i++)
	{
		if(String::ICompare($Spell::keyword[%i], %w1) == 0)
		{
			if(SkillCanUse(%clientId, $Spell::keyword[%i]))
			{
				if(fetchData(%clientId, "MANA") >= $Spell::manaCost[%i])
				{
					Client::sendMessage(%clientId, $MsgBeige, "Casting " @ $Spell::name[%i] @ ".");

					%player = Client::getOwnedObject(%clientId);
					if(GameBase::getLOSinfo(%player, $Spell::LOSrange[%i]))
					{
						%lospos = $los::position;
						%losobj = $los::object;
					}
					else
					{
						%lospos = "";
						%losobj = 0;
					}
	
					storeData(%clientId, "SpellCastStep", 1);
	
					%tempManaCost = floor($Spell::manaCost[%i] / 2);
					refreshMANA(%clientId, %tempManaCost);
					playSound($Spell::startSound[%i], GameBase::getPosition(%clientId));

					%skt = $SkillType[$Spell::keyword[%i]];
					%sk1 = $PlayerSkill[%clientId, %skt];
					%gsa = GetSkillAmount($Spell::keyword[%i], %skt);
					%sk2 = %sk1 - %gsa;
					%sk = Cap(%sk2, 0, "inf");
					%rt = $Spell::recoveryTime[%i];
					%a = %rt / 2;
					%b = (1000 - %sk) / 1000;
					%c = %b * %a;
					%recovTime = $Spell::delay[%i] + Cap(%a + %c, %a, %rt);	//recovery time is never smaller than half of the original and never bigger than the original.

					schedule("%retval=DoCastSpell(" @ %clientId @ ", " @ %i @ ", \"" @ GameBase::getPosition(%clientId) @ "\", \"" @ %lospos @ "\", \"" @ %losobj @ "\", \"" @ %w2 @ "\"); if(%retval){refreshMANA(" @ %clientId @ ", " @ %tempManaCost @ ");}", $Spell::delay[%i]);
					schedule("storeData(" @ %clientId @ ", \"SpellCastStep\", \"\");sendDoneRecovMsg(" @ %clientId @ ");", %recovTime);
		
					return True;
				}
				else
					Client::sendMessage(%clientId, $MsgWhite, "Insufficient mana to cast this spell.");
			}
			else
				Client::sendMessage(%clientId, $MsgWhite, "You can't use this force power because you lack the necessary skills.");

			return False;
		}
	}
	Client::sendMessage(%clientId, $MsgWhite, "This spell seems unfamiliar to you.");

	return False;
}

function DoCastSpell(%clientId, %index, %oldpos, %castPos, %castObj, %w2)
{
	dbecho($dbechoMode, "DoCastSpell(" @ %clientId @ ", " @ %index @ ", " @ %oldpos @ ", " @ %castPos @ ", " @ %castObj @ ", " @ %w2 @ ")");

	%player = Client::getOwnedObject(%clientId);

	if(Vector::getDistance(%oldpos, GameBase::getPosition(%clientId)) > $Spell::graceDistance[%index])
	{
		Client::sendMessage(%clientId, $MsgBeige, "Your casting was interrupted.");
		storeData(%clientId, "SpellCastStep", 2);

		return False;
	}

	//group-list check
	if($Spell::groupListCheck[%index])
	{
		%cl = Player::getClient(%castObj);
		if(%cl == -1)
			%cl = ClientFromName(%w2);
		if( !(IsInCommaList(fetchData(%clientId, "grouplist"), Client::getName(%cl)) && IsInCommaList(fetchData(%cl, "grouplist"), Client::getName(%clientId))) && %cl != %clientId && %cl != -1)
		{
			Client::sendMessage(%clientId, $MsgBeige, "You are not part of the target's group.");
			storeData(%clientId, "SpellCastStep", 2);

			return False;
		}
	}

	//==================================================================

	//unfortunately hard-coded part -- although that is the original purpose of Tribes scripting

	if($SkillType[$Spell::keyword[%index]] == $SkillNeutralCasting)
		return DoNeutralPowers(%clientId, %index, %oldpos, %castPos, %castObj, %player, %w2);
	if($SkillType[$Spell::keyword[%index]] == $SkillDefensiveCasting)
		return DoLightPowers(%clientId, %index, %oldpos, %castPos, %castObj, %player, %w2);
	if($SkillType[$Spell::keyword[%index]] == $SkillOffensiveCasting)
		return DoDarkPowers(%clientId, %index, %oldpos, %castPos, %castObj, %player, %w2);
}

function DoEndSpell(%clientId, %overrideEndSound, %extraDelay, %index, %castPos, %returnFlag)
{
	if(!$Spell::animation[%index])
		Player::setAnimation(%clientId, 39);
	else
		Player::setAnimation(%clientId, $Spell::keyword[%index]);


	if(!%overrideEndSound)
	{
		if(%extraDelay == "")
			playSound($Spell::endSound[%index], %castPos);
		else
			schedule("playSound(" @ $Spell::endSound[%index] @ ", \"" @ %castPos @ "\");", %extraDelay);
	}

	//==================================================================

	%skilltype = $SkillType[$Spell::keyword[%index]];
	if(%returnFlag == True)
	{
		storeData(%clientId, "SpellCastStep", 2);

		if(%skilltype == $SkillNeutralCasting || %skilltype == $SkillDefensiveCasting)
			UseSkill(%clientId, %skilltype, True, True);
		UseSkill(%clientId, $SkillEnergy, True, True);

		return True;
	}
	else if(%returnFlag == False || %returnFlag == "")
	{
		storeData(%clientId, "SpellCastStep", 2);

		UseSkill(%clientId, %skilltype, False, True);
		if(%skilltype != $SkillEnergy)
			UseSkill(%clientId, $SkillEnergy, False, True);
		//messageAll(1, "DBECHO TRUE");
		return False;
	}
}

function CreateAndDetBomb(%clientId, %b, %castPos, %doDamage, %index)
{
	dbecho($dbechoMode, "CreateAndDetBomb(" @ %clientId @ ", " @ %b @ ", " @ %castPos @ ", " @ %index @ ")");

	%player = Client::getOwnedObject(%clientId);

	%bomb = newObject("", "Mine", %b);

	addToSet("MissionCleanup", %bomb);

	//GameBase::Throw(%bomb, %player, 0, false);
	GameBase::setPosition(%bomb, %castPos);
	
	if(%doDamage)
		SpellRadiusDamage(%clientId, %castPos, %index);

	playSound($Spell::endSound[%index], %castPos);
}

function SpellDamage(%clientId, %targetId, %damageValue, %index)
{
	dbecho($dbechoMode, "SpellDamage(" @ %clientId @ ", " @ %targetId @ ", " @ %damageValue @ ", " @ %index @ ")");

	GameBase::virtual(%targetId, "onDamage", $SpellDamageType, %damageValue, "0 0 0", "0 0 0", "0 0 0", "torso", "front_right", %clientId, $Spell::keyword[%index]);
}

function SpellRadiusDamage(%clientId, %pos, %index)
{
	dbecho($dbechoMode, "SpellRadiusDamage(" @ %clientId @ ", " @ %pos @ ", " @ %index @ ")");

	%b = $Spell::radius[%index] * 2;
	%set = newObject("set", SimSet);
	%n = containerBoxFillSet(%set, $SimPlayerObjectType, %pos, %b, %b, %b, 0);

	Group::iterateRecursive(%set, DoSpellDamage, %clientId, %pos, %index);
	deleteObject(%set);
}
function DoSpellDamage(%object, %clientId, %pos, %index)
{
	dbecho($dbechoMode, "DoSpellDamage(" @ %object @ ", " @ %clientId @ ", " @ %pos @ ", " @ %index @ ")");

	%id = Player::getClient(%object);

	%percMin = 5;
	%percMax = 100;

	%dist = Vector::getDistance(%pos, GameBase::getPosition(%id));

	if(%dist <= $Spell::radius[%index])
	{
		%newDamage = SpellCalcRadiusDamage(%dist, $Spell::radius[%index], $Spell::damageValue[%index], %percMin, %percMax);
		SpellDamage(%clientId, %id, %newDamage, %index);
	}
}

function SpellCalcRadiusDamage(%dist, %radius, %dmg, %percMin, %percMax)
{
	dbecho($dbechoMode, "SpellCalcRadiusDamage(" @ %dist @ ", " @ %radius @ ", " @ %dmg @ ", " @ %percMin @ ", " @ %percMax @ ")");

	%newdmg = %dmg - (%dist * (%dmg / %radius));

	%p = (%newdmg * 100) / %dmg;

	if(%p < %percMin)
		%p = %percMin;
	else if(%p > %percMax)
		%p = %percMax;

	%newdmg = (%p * %dmg) / 100;

	return %newdmg;
}

function GetBestSpell(%clientId, %type, %semiRandomSpell)
{
	dbecho($dbechoMode, "GetBestSpell(" @ %clientId @ ", " @ %type @ ", " @ %semiRandomSpell @ ")");

	%wdelay = 10;	//weights
	%wrecov = 0.5;

	%bestSpell = -1;
	%backupSpell = "";
	%highest = 0.1;

	for(%i = 1; $Spell::keyword[%i] != ""; %i++)
	{
		if(SkillCanUse(%clientId, $Spell::keyword[%i]))
		{
			if(fetchData(%clientId, "MANA") >= $Spell::manaCost[%i])
			{
				%d = ( ($Spell::delay[%i] / %wdelay) + ($Spell::recoveryTime[%i] / %wrecov) );
				%x = (100 / %d) * $Spell::refVal[%i];
				%v =  %x * %type;

				if(%semiRandomSpell)
				{
					%r = getRandom() * 100;
					%rr = getRandom() * 100;
				}
				else
				{
					%r = 1;
					%rr = 0;
				}

				if(%v > %highest)
				{
					if(%r > %rr)
					{
						%bestSpell = %i;
						%highest = %v;
					}
					else
						%backupSpell = %i;
				}
			}
		}
	}
	if(%bestSpell == -1 && %backupSpell != "")
		%bestSpell = %backupSpell;

	return %bestSpell;
}

function CalcSpellMiss(%clientId, %targetId, %index)
{
	dbecho($dbechoMode, "CalcSpellMiss(" @ %clientId @ ", " @ %targetId @ ", " @ %index @ ")");

	%range = $Spell::LOSrange[%index];
	%dist = Vector::getDistance(GameBase::getPosition(%clientId), GameBase::getPosition(%targetId));

	%m = floor((getRandom() * %range)) + (%range / 6);

	//echo(%dist @ " / " @ %range @ " : --> " @ %m);
	if(%m > %dist)
		return False;
	else
		return True;
}

function sendDoneRecovMsg(%clientId)
{
	//this function is here just to make the schedule command where this is called easier to read
	Client::sendMessage(%clientId, $MsgBeige, "You are ready to cast.");
}

function DoBoxFunction(%object, %clientId, %index, %extra)
{
	dbecho($dbechoMode, "DoBoxFunction(" @ %object @ ", " @ %clientId @ ", " @ %index @ ", " @ %extra @ ")");

	%id = Player::getClient(%object);

	if(%index == $Spell::index[massheal]) //MassHeal
	{
		if(GameBase::getTeam(%clientId) == GameBase::getTeam(%id))
		{
			Client::sendMessage(%clientId, $MsgBeige, "Mass Healing " @ Client::getName(%id));
			if(%clientId != %id)
				Client::sendMessage(%id, $MsgBeige, "You are being Mass Healed by " @ Client::getName(%clientId));

			%r = $Spell::damageValue[%index] / $TribesDamageToNumericDamage;
			refreshHP(%id, %r);

			%castPos = GameBase::getPosition(%id);

			CreateAndDetBomb(%clientId, "Bomb10", %castPos, False, %index);
			playSound($Spell::endSound[%index], %castPos);
		}
	}
	if(%index == $Spell::index[massfullheal]) //MassFullHeal
	{
		if(GameBase::getTeam(%clientId) == GameBase::getTeam(%id))
		{
			Client::sendMessage(%clientId, $MsgBeige, "Mass Fully Healing " @ Client::getName(%id));
			if(%clientId != %id)
				Client::sendMessage(%id, $MsgBeige, "You are being Mass Fully Healed by " @ Client::getName(%clientId));

			setHP(%id, fetchData(%id, "MaxHP"));

			%castPos = GameBase::getPosition(%id);

			CreateAndDetBomb(%clientId, "Bomb10", %castPos, False, %index);
			playSound($Spell::endSound[%index], %castPos);
		}
	}
	if(%index == 25) //MassShield?
	{
		if(GameBase::getTeam(%clientId) == GameBase::getTeam(%id))
		{
			Client::sendMessage(%clientId, $MsgBeige, "Shielding " @ Client::getName(%id));
			if(%clientId != %id)
				Client::sendMessage(%id, $MsgBeige, Client::getName(%clientId) @ " is casting " @ $Spell::name[%index] @ " on you.");

			UpdateBonusState(%id, $Spell::damageValue[%index], $Spell::ticks[%index]);

			%castPos = GameBase::getPosition(%id);

			CreateAndDetBomb(%clientId, "Bomb10", %castPos, False, %index);
			playSound($Spell::endSound[%index], %castPos);
		}
	}
	if(%index == $Spell::index[masstransport])
	{
		if(IsInCommaList(fetchData(%clientId, "grouplist"), Client::getName(%id)) && IsInCommaList(fetchData(%id, "grouplist"), Client::getName(%clientId)) || %clientId == %id)
		{
			Client::sendMessage(%clientId, $MsgBeige, "Transporting " @ Client::getName(%id) @ " to " @ Zone::getDesc(%extra));
			if(%clientId != %id)
				Client::sendMessage(%id, $MsgBeige, Client::getName(%clientId) @ " is transporting you to " @ Zone::getDesc(%extra));

			//teleport <--what's this about? It's misleading. it's masstransport :/

			%system = Object::getName(%extra);
			%type = GetWord(%system, 0);
			%desc = String::getSubStr(%system, String::len(%type)+1, 9999);

			%castPos = TeleportToMarker(%id, "Zones\\" @ %system @ "\\DropPoints", False, True);
			CheckAndBootFromArena(%id);
			NullItemList(%clientId, Lore, $MsgRed, "You lost all %1s you were carrying when you teleported.");

			if(!fetchData(%id, "invisible"))
				GameBase::startFadeIn(%id);

			Player::setDamageFlash(%id, 0.7);

			%extraDelay = 0.22;
			schedule("playSound(" @ $Spell::endSound[%index] @ ", \"" @ %castPos @ "\");", %extraDelay);
		}
	}
}

function SpellCanCast(%clientId, %keyword)
{
	dbecho($dbechoMode, "SpellCanCast(" @ %clientId @ ", " @ %keyword @ ")");

	for(%i = 1; $Spell::keyword[%i] != ""; %i++)
	{
		if(String::ICompare($Spell::keyword[%i], %keyword) == 0)
		{
			if(SkillCanUse(%clientId, $Spell::keyword[%i]))
			{
				if(fetchData(%clientId, "MaxMANA") >= $Spell::manaCost[%i])
					return True;
			}
		}
	}
	return False;
}
function SpellCanCastNow(%clientId, %keyword)
{
	dbecho($dbechoMode, "SpellCanCastNow(" @ %clientId @ ", " @ %keyword @ ")");

	for(%i = 1; $Spell::keyword[%i] != ""; %i++)
	{
		if(String::ICompare($Spell::keyword[%i], %keyword) == 0)
		{
			if(SkillCanUse(%clientId, $Spell::keyword[%i]))
			{
				if(fetchData(%clientId, "MANA") >= $Spell::manaCost[%i])
					return True;
			}
		}
	}
	return False;
}

//-------------------------------------------------------------------------------------------------------------

function PromptTeleportToCaster(%casterId, %targetId, %castPos)
{
	Client::buildMenu(%targetId, "Accept " @ client::getName(%casterId) @ " transport?", "transportoffer", true);
	Client::addMenuItem(%clientId, "1Yes", "yes " @ %casterId @ " " @ %castPos);
	Client::addMenuItem(%clientId, "2No", "no " @ %casterId @ " " @ %castPos);
	Client::addMenuItem(%clientId, "yYes", "yes " @ %casterId@ " " @ %castPos);
	Client::addMenuItem(%clientId, "nNo", "no " @ %casterId@ " " @ %castPos);
}

function processMenuTransportOffer(%targetId, %option)
{
	dbecho($dbechoMode, "processMenuTransportOffer(" @ %clientId @ ", " @ %option @ ")");

	%opt = getWord(%option, 0);
	%casterId = getWord(%option, 1);
	%castPos = getWord(%option, 2);

	if(%opt == "yes")
	{
		CheckAndBootFromArena(%targetId);
		//NullItemList(%clientId, Lore, $MsgRed, "You lost all %1s you were carrying when you teleported.");

		if(!fetchData(%targetId, "invisible"))
			GameBase::startFadeIn(%targetId);

		Player::setDamageFlash(%targetId, 0.7);
		Client::sendMessage(%targetId, $MsgBeige, "You are being transported to " @ Client::getName(%client));
		playSound(ActivateCH, %castPos);
		gameBase::setPosition(%targetId, %castpos);
	}
	else if(%opt == "no")
	{
		Client::sendMessage(%targetId, $MsgBeige, "You declined " @ Client::getName(%casterId) @ "'s offer to transport you.");
		Client::sendMessage(%casterId, $MsgBeige, Client::getName(%targetId) @ " declined your transport offer.");
	}

	return;
}
