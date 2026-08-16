
function Client::onKilled(%Client, %killerId, %damageType, %legal) { //, %dmg) {

	//This function is NOT an event, it must be MANUALLY CALLED!
	//At this point, the client can still be queried for getItemCounts, but is not considered an object anymore.

	if(Zone::getType($zone[%Client]) != "FREEFORALL")
		%zoneflag = False;
	else
		%zoneflag = True;

	//Check if the kill was legal
	if(CheckBonus(%Client, "PKer", "Thief", "StatusUser"))
		%legal = true;

	if($ClientData[%killerId, "GotHitBy"@%Client] == true) { // turn off PK mode if %killer killed the person who attacked him with PK mode off
//		$PKflag[%killerId] = "";
//		Client::sendMessage(%killerId, $MsgRed, "PvP Disabled.");
		%legal = true;
	}

//	$ClientData[%killerId, "GotHitBy"@%Client] = "";
	deleteVariables("ClientData"@%killerId@"_GotHitBy*");

	if(!%zoneflag)
		DistributeExpForKilling(%Client);

	remoteEval(%Client, "ClientDied");

	remoteEval(%Client, "DoCmd", "OnDeath");

	%KillerN = Client::getName(%killerId);
	%ViticmN = Client::getName(%Client);

	if(String::ICompare(Client::getGender(%dClient), "Male") == 0)
		%gender = "himself";
	else if(String::ICompare(Client::getGender(%dClient), "Female") == 0)
		%gender = "herself";
	else
		%gender = "itself";

	if(!Player::isAiControlled(%Client))
		%lvl = getFinalLVL(%Client);

	if(%killerId != %Client) {

		//a human player killed %Client

		Client::sendMessage(%Client, 0, "You were killed by "@%KillerN@"!"); //Killing hit "@%dmg@" dmg!");
		radiusAllExcept(%Client, %killerId, "You see "@%ViticmN@" killed by "@%KillerN@"!");

		if(!RPG::isAiControlled(%Client) && !Player::isAiControlled(%KillerId) && !%zoneflag && !%legal) {//Make sure both are human players
			UpdateBonusState(%KillerId, "PKer", 30 * 15, "add"); //15 mins
			pk::monitor(%client,%killerid);
			Client::sendMessage(%KillerId, 1, "You are now marked as a PKer!");
			if($bounty[%killerId] == Client::getName(%Client))
				$bounty[%killerId] = getFinalLVL(%Client)@" !Q@W#E$R%T^Y&U*I(O)P";
		}

		if(%lvl > 10 && !Player::isAiControlled(%Client) && !%zoneflag)// 'protection' for the lower lvls and bots... :P
			TakeDeathEXP(%Client, %lvl, Killed);
	}
	else if(%killerId == %Client) {
		Client::sendMessage(%Client, 0, "You killed yourself!");
		radiusAllExcept(%Client, %killerId, "You see "@%ViticmN@" kill "@%gender@"!");

		if(%lvl > 10 && !Player::isAiControlled(%Client) && !%zoneflag)// 'protection' for the lower lvls and bots... :P
			TakeDeathEXP(%Client, %lvl, Self);
	}
	else if(%damageType == 11 || %damageType == 12 || %damageType == 2) {
		Client::sendMessage(%Client, 0, "You were killed!");// Killing hit "@%dmg@" dmg!");
		radiusAllExcept(%Client, %killerId, "You see "@%ViticmN@" killed by an unknown force!");

		if(%lvl > 10 && !Player::isAiControlled(%Client) && !%zoneflag)// 'protection' for the lower lvls and bots... :P
			TakeDeathEXP(%Client, %lvl, killed);
	}

	$invisible[%Client] = "";

	echo("GAME: kill "@%killerId@" "@%Client@" "@%damageType);
	%Client.guiLock = true;
	Client::setGuiMode(%Client, $GuiModePlay);

	Game::clientKilled(%Client, %killerId);
}

function TakeDeathEXP(%Client, %lvl, %how) {

	%table = getGainedExp(%Client);
	%ge = floor(GetWord(%table, 0)@GetWord(%table, 1)@GetWord(%table,2) / 2);
	if(%how == Killed)
		%lostexp = floor(%ge + pow(%lvl, 2));//(%lvl * 2))+1;
	else if(%how == Self) {
		%lostexp = floor(%ge + pow(%lvl, 2.5));//(%lvl * 4))+1;
		if($CrtlKed[%Client] == true)	// kill();
			%lostexp += 500 * %lvl;
	}

	$CrtlKed[%Client] = "";
	if(%lostexp < -1) //overflowed into -
		%lostexp = "500000000";

	GiveExp(%Client, (-Cap(%lostexp, 1, 500000000)));

	Client::Sendmessage(%Client, 1, "You have lost "@FixM(%lostexp)@" EXP!");

	remoteEval(%Client,"RefreshEXPset", Fix(0, %Client, EXP));

}

function Game::clientKilled(%playerId, %killerId) {

	%set = nameToID("MissionCleanup/ObjectivesSet");
	for(%i = 0; (%obj = Group::getObject(%set, %i)) != -1; %i++)
		GameBase::virtual(%obj, "clientKilled", %playerId, %killerId);
}

function Player::onKilled(%this) {

	//At this point, the client can still be queried for getItemCounts, and is also still an object
	//Player::Kill calls this function
	%Client = Player::getClient(%this);

	if(%Client == -1 || %Client == "")
		return;

	if(!$noDropLootbagFlag[%Client]) {
		if(Zone::getType($zone[%Client]) != "FREEFORALL") {

			%tmploot = "";
			if($COINS[%Client] > 0)
				%tmploot = %tmploot@"COINS "@floor($COINS[%Client])@" ";

			$COINS[%Client] = 0;

			%list = $ClientData[%Client, "ItemList"];
			for(%i = 0; (%a = getWord(%list, %i)) != -1; %i+=2) {

				if((%Icnt = Client::getItemCount(%Client, %a)) > 0) {

					%flag = False;
					if(getFinalLCK(%Client) >= 0) {
						//currently mounted weapon and all equipped stuff + lore items are thrown into lootbag.
						if($ClientData[%Client, UsingWeapon] == %a || $LoreItem[%a] == True)
							%flag = True;
					}
					else //has no more LCK, throw all items in pack
						%flag = True;

				//	if(%a == esmissile  || %a == goblinpunch  || %a == cuttinglaser  || %a == runt_goblinpunch  || %a == screech  || %a == deathcoil
				//	|| %a == zombiesworda || %a == zombieswordb || %a == godeyesword || %a == golemaxe ||  %a == double || %a == fspear
				//	|| %a == gnollspear  || %a == combustible  || %a == ghostweapon  || %a == Low_Lich_Caster  || %a == High_Lich_Caster
				//	|| %a == Goblin_Chief_Caster  || %a == dodgethis  || %a == Rock_Giant_Caster  || %a == blobweapon || %a == runt_goblinpunch
				//	|| %a == uuagweapon  || %a == gnollspeara  || %a == grusword || %a == fishspitshooter || %a == grunganaxe)
					if($ItemData[%a, NoDrop])
						%flag = False;		//HARDCODED because we DONT want any players to have this item.

					if(%flag) {

						if($ClientData[%Client, UsingWeapon] == %a) {
							%tmploot = %tmploot @ %a @ " 1 ";
							Client::addItemCount(%Client, %a, -1);
						}
						else {
							%tmploot = %tmploot @%a@" "@%Icnt@" ";
							Client::addItemCount(%Client, %a, -%Icnt);
						}
					}
				}
			}
			if(!Player::isAiControlled(%Client)) {
				%list = $ClientData[%Client, "EquipList"];
				for(%i = 0; (%a = getWord(%list, %i)) != -1; %i+=2) {
					if((%Icnt = Client::getItemCount(%Client, %a, "EquipList")) > 0) {
						%tmploot = %tmploot@getCroppedItem(%a)@" "@%Icnt@" ";
						Client::addItemCount(%Client, %a, -%Icnt, "EquipList");
					}
				}
			}
			if(%tmploot != -1 || %tmploot != "") {
//	echo("tmploot: '"@%tmploot@"'");
				if(Player::isAiControlled(%Client))
					TossLootbag(%Client, %tmploot, 1, False, 0, true);
				else {
					if(getFinalLCK(%Client) >= 0)
						%tehLootBag = TossLootbag(%Client, %tmploot, 5, True, 300, true);
					else
						%tehLootBag = TossLootbag(%Client, %tmploot, 5, True, 5, true);
					echo("pack num: "@%tehLootBag);
					echo("Items: "@String::getSubStr(%tmploot, 0, 240));
				}
			}
			if(!Player::isAiControlled(%Client) && getFinalLCK(%Client) < 0)
				$zone[%Client] = "";	//so the player spawns back at start points

			$ClientData[%Client, toggleShield] = "";
		}
	}

	$noDropLootbagFlag[%Client] = "";
	if($SpellCastStep[%Client] > 0)
		Schedule("$SpellCastStep["@%Client@"] = \"\";", 6);


	%Client.sleepMode = "";

	if(!Player::isAiControlled(%Client)) {
		remoteEval(%Client, "ClearItemLists");
		$Client::tmp[%Client, "ItemList"] = $ClientData[%Client, "ItemList"];
		$Client::tmp[%Client, "EquipList"] = $ClientData[%Client, "EquipList"];
		$Client::tmp[%Client, "QuestList"] = $ClientData[%Client, "QuestList"];

		refreshHPREGEN(%Client);
		refreshMANAREGEN(%Client);

		//remember the last zone the player was in.
		$lastzone[%Client] = $zone[%Client];

		if($LCK[%Client] < 0)		//reset LCK back to 0 if it's at -1 (or lower, which shouldn't happen)
			$LCK[%Client] = 0;

		Player::setDamageFlash(%this,0.75);
		playSound("SoundPlayerDeath"@Client::getGender(%Client), GameBase::getPosition(%Client));
	}
	else {

		PlaySound(RandomRaceSound($RACE[%Client], Death), GameBase::getPosition(%Client));

		%aiName = $AIEvents::[%Client, Name]; //echo(%aiName);
		if($AIEvents::[%Client, Alive]) {
			$AIEvents::[%Client, Alive] = "";
			$AIEvents::[%aiName, time] = getIntegerTime(true) >> 5;
			if($MasterBotDieSay[%aiName] != "") {
				//AI::SayZone(%Client, $MasterBotDieSay[%aiName]);
				//radiusAllExcept(%Client, 0, $MasterBotDieSay[%aiName]);
				remoteSay(%Client, true, "#zone "@$MasterBotDieSay[%aiName]);
			}
		}
	}

	$Weight[%Client] = 0;
	$ClientData[%Client, "ItemList"] = " ";
	$ClientData[%Client, "EquipList"] = " ";
	$ClientData[%Client, "QuestList"] = " ";
	$ClientData[%Client, UsingWeapon] = "-1";

	GiveThisStuff(%Client, $Client::tmp[%Client, "QuestList"], false);
	GiveThisStuff(%Client, $Client::tmp[%Client, "ItemList"], false);

	$Client::tmp[%Client, "ItemList"] = "";
	$Client::tmp[%Client, "QuestList"] = "";
	//---------------

	for(%i  = 0; $StatusLookUpList[%i] != ""; %i++) {
		if($ClientData[%Client, $StatusLookUpList[%i]] > 0) {
			$ClientData[%Client, $StatusLookUpList[%i]] = -666;
		}
	}

//========================================================================================================================

	%Client.dead = 1;
	if($AutoRespawn > 0)
		schedule("Game::autoRespawn(" @ %Client @ ");",$AutoRespawn,%Client);

	if(%Client != -1)
	{
		if(%this.vehicle != "")
		{
			if(%this.driver != "")
			{
				%this.driver = "";
				Client::setControlObject(Player::getClient(%this), %this);
				Player::setMountObject(%this, -1, 0);
			}
			else
			{
				%this.vehicle.Seat[%this.vehicleSlot-2] = "";
				%this.vehicleSlot = "";
			}
			%this.vehicle = "";
		}
		schedule("GameBase::startFadeOut(" @ %this @ ");", $CorpseTimeoutValue, %this);
		Client::setOwnedObject(%Client, -1);
		Client::setControlObject(%Client, Client::getObserverCamera(%Client));
		Observer::setOrbitObject(%Client, %this, 15, 15, 15);
		schedule("deleteObject(" @ %this @ ");", $CorpseTimeoutValue + 2.5, %this);
		%Client.observerMode = "dead";
		%Client.dieTime = getSimTime();
	}
}


function ForcedMiss(%sClient, %dClient) {
	if(%sClient == 0)
		%hitby = "An NPC";
	else
		%hitby = Client::getName(%sClient);

	remoteEval(%sClient,"ATKText", "<jc>MISS! (STA)", true);
	remoteEval(%dClient,"ATKText", "<jc>"@%hitby@" MISSED! (STA)", false);
}


function Player::onDamage(%this,%type,%value,%pos,%vec,%mom,%vertPos,%quadrant,%object,%weapon) {

	%dClient = Player::getClient(%this);

	if($BotShootingAt_WithId[%dClient] == "")
		%sClient = %object;
	else {
		%object = $BotShootingAt_WithId[%dClient];
		%sClient = %object;
		$BotShootingAt_WithId[%dClient] = "";
		$CanDoSpellDmg[%sClient] = True;
		$ClientData[%sClient, SpellDmg] = AddPoints(%sClient, 6);
	}

	%weapon = $ClientData[%sClient, UsingWeapon];

if($debug == true) echo("=------| %Obj SHOOTER: "@Client::getname(Player::getClient(%object))@" | %this DAMAGED: "@Client::getname(Player::getClient(%this))@" | [%type = "@%type@" | %weapon "@%weapon@"] |------=");

	if(IsDead(%sClient) || IsDead(%dClient))
		return;

	if(Player::isExposed(%this) && %object != -1 && %type != $NullDamageType) {

		%dClientPos = GameBase::getPosition(%dClient);
		%sClientPos = GameBase::getPosition(%sClient);

		$dataplayerId[%dClient] = Client::getOwnedObject(%dClient);

		if($isZombie[%dClient] && %sClient != %dClient) {//BAM DEAD!
			Zombie::Kill(%dClient, %this);
			Client::onKilled(%dClient, %sClient, %type, true);
			return;
		}

		//==============
		//PROCESS STATS
		//==============
		%isMiss = False;
		%Backstab = False;
		%Bash = False;

		//------------- CREATE DAMAGE VALUE --------
		if((%type == 0 && $CanDoSpellDmg[%sClient]) || %type == $SpellDamageType || $CanDoSpellDmg[%sClient]) {
			//$CanDoSpellDmg[%sClient] = "";

			if($ClientData[%sClient, SpellDmg] == null)
				return;

			%a = getFinalINT(%sClient);
			%b = getFinalINT(%dClient);
			if(%b > %a) {
				if((%b-50) > %a) {
					if((%b-200) > %a) {
						if((%b-500) > %a)
							%toHit = 40;
						else
							%toHit = GetRoll("1d30");
					}
					else
						%toHit = GetRoll("1d25");
				}
				else
					%toHit = GetRoll("1d23");
			}
			else if(%a > %b) {
				if((%a-50) > %b) {
					if((%a-200) > %b) {
						if((%a-500) > %b)
							%toHit = 1;
						else
							%toHit = GetRoll("1d10");
					}
					else
						%toHit = GetRoll("1d15");
				}
				else
					%toHit = GetRoll("1d18");
			}
			else
				%toHit = GetRoll("1d20");

			%roll = GetRoll("1d40") + 5;

			if(%toHit > %roll)
				%isMiss = true;

			if(!%isMiss) {

				%a = $ClientData[%sClient, SpellDmg]; //(%value * 100);
				if(%a == "") //blah, Weapon_SpellAttack
					%a = AddPoints(%sClient, 6);

if($debug == true) echo("VALUE: "@%a);
				%b = round(getFinalWIS(%sClient) * 0.2);
			//	%c = round(getFinalWIS(%dClient) * 0.4);
				%c = getFinalMDEF(%dClient);
				%prec = AddPoints(%dClient, $MagicType[$ClientData[%sClient, SpellType]]);

				%d = getFinalLVL(%sClient);
				%e = getFinalLVL(%dClient);
if($debug == true) echo("sClient wis "@%b@" | dClient MDEF "@%c@" | dClient prec "@%prec);
				%f = (%a) + (%b-%c) + (%d-%e);
				%f -= round(%f * (%prec/100));

if($debug == true) echo("Spell dmg: "@%f);
				if(%sClient != %dClient)
					%value = Cap(round(%f), -99999, 99999);
				else
					%value = Cap(round(%f/4), -99999, 99999);//Shoots himself.

if($debug == true) echo("NEW VALUE: "@%value);
				if(%value > 0)
					%type = $SpellDamageType;

				%value = (%value / $TribesDamageToNumericDamage);
			}
			if(%value <= 0 || (%isMiss)) {
				if(%sClient == 0)
					%hitby = "an NPC";
				else
					%hitby = Client::getName(%sClient);

				%time = getIntegerTime(true) >> 5;
				if(%time - %dClient.lastMissMessage > 2) {

					%dClient.lastMissMessage = %time;

					if(!Player::isAiControlled(%dClient))
						remoteEval(%dClient,"ATKText", "<jc>"@%hitby@" MISSED!", false);

					if(!Player::isAiControlled(%sClient))
						remoteEval(%sClient,"ATKText", "<jc>MISS!", true);
				}

				//return;
			}
		}
		else if(%type != $LandingDamageType && %type != $SpellDamageType) {

			DegradeableEffects(%object, %this, %weapon);

			if(!Player::isAiControlled(%sClient)) {//Doesn't effect bots =p
				%sta = $STA[%sClient];
				if(%sta <= 2) {
					$STA[%sClient] = 1;
					ForcedMiss(%sClient, %dClient);
					return;
				}
				else if(%sta <= 20 && getFinalLVL(%sClient) >= 20) {
					if(OddsAre(4)) {
						ForcedMiss(%sClient, %dClient);
						return;
					}
				}
				else if(%sta <= 50 && %sta >= 20 && getFinalLVL(%sClient) >= 50) {
					if(OddsAre(5)) {
						ForcedMiss(%sClient, %dClient);
						return;
					}
				}
			}
			if(GetAccessoryVar(%weapon, $AccessoryType) == $ProjectileAccessoryType)
				%dmgadj = 0;
			else
				%dmgadj = DmgAdj(%sClient);

			%defadj = DefensiveAdj(%dClient);

			//Backstab
			if($invisible[%sClient]) {
				%dRot = GetWord(GameBase::getRotation(%dClient), 2);
				%sRot = GetWord(GameBase::getRotation(%sClient), 2);
				%diff = %dRot - %sRot;
				if(%diff >= -0.9 && %diff <= 0.9) {
					if(%type == $PiercingDamageType) {
						%multi *= round(getFinalLVL(%sClient)/4)+1;
						%Backstab = True;
					}
				}
				GameBase::startFadeIn(%sClient);
				$invisible[%sClient] = "";
			}
			if($invisible[%dClient] && %dClient.adminLevel < 5) {
				GameBase::startFadeIn(%dClient);
				$invisible[%dClient] = "";
			}

			//Bash
			if($NextHitBash[%sClient]) {
				if(%type == $BludgeoningDamageType) {
					%multi += Cap(round(getFinalLVL(%sClient)/10), -1, 2);
					%b = GameBase::getRotation(%sClient);
					%c = GetBashPow(%sClient);

					%Bash = True;
				}//FIXED!
				schedule("$blockBash[" @ %sClient @ "] = \"\";", Cap(150-getFinalLVL(%sClient), 5, 50));
				$NextHitBash[%sClient] = "";
			}

			%HasBonus = "";
			%ATKBonus = 0;
			%BonusCnt = $AttackBonus[%type, %sClient];

			%weaponroll = AddPoints(%sClient, 6);

			if(%BonusCnt > 0) {
				%HasBonus = true;
				%ATKBonus = %weaponroll; //GetRoll(GetWord(GetAccessoryVar(%weapon, $SpecialVar), 1));
				//for(%i = 1; %i <= %BonusCnt; %i++) {
				//	%ATKBonus += %ATKBonus / 4.5;
				//	if(%ATKBonus >= 99999)
				//		break;
				//}
				%ATKBonus += (%ATKBonus * %BonusCnt) / 5;
			}
if($debug == true) echo("ATKBONUS: "@%ATKBonus);

			%weaponroll += floor(%weaponroll * getRandom()); // boost atk power

			%value = Cap(%weaponroll+ (%dmgadj - %defadj) + %ATKBonus, 0, 99999);

if($debug == true) echo("DMGADJ:"@%dmgadj@" | DEFADJ:"@%defadj@" | WEAP ROLL:"@%weaponroll@" | VALUE:"@%value);

			if(%Bash) {	//i'm doing this condition here because %mom is dependant on %value
				%c1 = Cap(%c / 15 * %value, 0, 750); //1000
				%c2 = Cap(%c / 10, 0, 650); //900
				%mom = Vector::getFromRot( %b, %c1, %c2 );
			}
		}
		//------------------------------------------

		if(%type != $LandingDamageType && %sClient != %dClient && %sClient != 0 && %type != $SpellDamageType) {

			if(GetAccessoryVar(%weapon, $AccessoryType) == $ProjectileAccessoryType)
				%mattackadj = MAttackAdj(%sClient);
			else
				%mattackadj = 0;

			%Def = getFinalDEF(%dClient);

if($debug == true) echo("%DEF:"@%Def);

			%sDex = getFinalDEX(%sClient);
			%dDex = getFinalDEX(%dClient);
			%sLVL = getFinalLVL(%sClient);
			%dLVL = getFinalLVL(%dClient);

			if(%sDex > %dDex)
				%toHit = %sDex-%dDex;
			else
				%toHit = %dDex-%sDex;
if($debug == true) echo("-----%toHit:"@%toHit@" | %sDex:"@%sDex@" - %dDex:"@%dDex);
			if(%toHit > 50 && %dDex > %sDex) {
				if(%toHit > 100) {
					if(%toHit > 200) {
						if(%toHit > 500)//Damaged Client is to fast
							%toHit = 20;
						else
							%toHit = GetRoll("1d17");
					}
					else
						%toHit = GetRoll("1d15");
				}
				else
					%toHit = GetRoll("1d10");
			}
			else if(%toHit > 50 && %sDex > %dDex) {
				if(%toHit > 100) {
					if(%toHit > 200) {
						if(%toHit > 500)//Shooter Client is to fast
							%toHit = 1;
						else
							%toHit = GetRoll("1d3");
					}
					else
						%toHit = GetRoll("1d5");//>100
				}
				else
					%toHit = GetRoll("1d6");//>50
			}//Both have about the same DEX
			else
				%toHit = GetRoll("1d7");//<50

if($debug == true) echo("-----%toHit:"@%toHit);

			%critical = "";

			%sRing = Client::HasItem(%sClient, "Lucky_Ring&", "EquipList");
			%dRing = Client::HasItem(%dClient, "Lucky_Ring&", "EquipList");
			if(%sRing || %dRing) {
				if(%sRing && %dRing) {//both have ring
					%roll = GetRoll("1d20") + %mattackadj;
					if(%roll >= 20) { %roll = 20; %critical = true; }
					if(%roll < 1) %roll = 1;
				}
				else if(%sRing && !%dRing) {//shooter has ring
					%roll = GetRoll("1d20") + %mattackadj;
					if(%roll >= 15) { %roll = 20; %critical = true; }
					if(%roll < 1) %roll = 1;
				}
				else if(!%sRing && %dRing) {//damaged has ring
					%roll = GetRoll("1d20") + %mattackadj;
					if(%roll >= 20) { %roll = 20; %critical = true; }
					if(%roll < 5) %roll = 1;
				}
			}
			else {						//none have ring
				%roll = GetRoll("1d20") + %mattackadj;
				if(%roll >= 20) { %roll = 20; %critical = true; }
				if(%roll < 1) %roll = 1;
			}

			if(%sClient.adminLevel >= 5) { %roll = 20; %critical = true; }
			if((%roll < %toHit || %roll == 1) && %roll != 20)
				%isMiss = True;
			else
				%value = GetRollDmg(%value, %roll, %Def);

if($debug == true) echo("%roll:"@%roll@" | New %value:"@%value);

			if(%value == 0 || %isMiss)
				%isMiss = True;
			else {
				if(%critical == true)
					%value *= 2;

				if($isZombie[%sClient])
					%value *= 4;
				if($ClientData[%sClient, toggleShield])
					%value = %value / 3;

				%value = Cap(floor(%value), 0, 99999);
				%value = (%value / $TribesDamageToNumericDamage);
			}
			if(%isMiss == true) {
				refreshSTAMINA(%sClient, Cap(%sLVL / 30, 0.01, 10));
				refreshSTAMINA(%dClient, Cap(%dLVL / 60, 0.01, 10)); // %dLVL / 50
			}
			else if(%critical == true) {
				refreshSTAMINA(%sClient, Cap(%sLVL / 10, 0.01, 100));
				refreshSTAMINA(%dClient, Cap(%dLVL / 20, 0.01, 100));
			}
			else {
				refreshSTAMINA(%sClient, Cap(%sLVL / 20, 0.01, 30));
				refreshSTAMINA(%dClient, Cap(%dLVL / 40, 0.01, 15));
			}
		}
		if(%dClient == %sClient && %type == $LandingDamageType) {
			%object = "";
			for(%i = 0; %i >= -3.15; %i -= 1.57) {
				if(GameBase::getLOSInfo(Client::getOwnedObject(%dClient), 5, %i @ " 0 0")) {
					if(getObjectType($los::object) == "InteriorShape" && String::getSubStr(Object::getName($los::object), 0, 5) == "water") {
						%object = $los::object;
						break;
					}
				}
			}
			if(%object != "") {
				%value *= $waterDamageAmp;
				playSound(SoundSplash1, %dClientPos);
			}

			%value = round(%value * $TribesDamageToNumericDamage);
			%value = Cap(%value-25, 0, "inf");
			%value = (%value / $TribesDamageToNumericDamage);

		}
		%sZone = Zone::getType($zone[%sClient]);
		%dZone = Zone::getType($zone[%dClient]);
		//if(%dClient == %sClient && %type == $LandingDamageType) {
		//	if(%dZone == "WATER")
		//		%value *= $waterDamageAmp;
		//}
		if(%dClient.adminLevel >= 4 && %type == $LandingDamageType)
			%value = 0;
		if(%dClient.adminLevel >= 5)
			%value = 0;
		//------------------------------------------------
		// SAME TEAM CHECKS
		//------------------------------------------------
		if(!$isZombie[%sClient]) {
			if(Client::getTeam(%dClient) == Client::getTeam(%sClient) && %sClient != %dClient) {
				if(!CheckBonus(%dClient, "PKer", "Thief", "StatusUser")) { //Damaged client has a Pker, or Thief flag on.
					if(!(%dZone == "FREEFORALL" && %sZone == "FREEFORALL")) {
						//if(!($PKflag[%dClient] && $PKflag[%sClient])
						if(!$PKflag[%sClient] && !Player::isAiControlled(%dClient) && !Player::isAiControlled(%sClient)) {

							%value = 0;
							%isMiss = False;	//override in case a miss was determined earlier
							%noImpulse = True;
							Client::sendMessage(%sClient, $MsgRed, "PvP rules are preventing you from damaging "@Client::getName(%dClient)@".");
						}
						else if($PKflag[%sClient] && !$PKflag[%dClient]) {

							$ClientData[%dClient, "GotHitBy"@%sClient] = true; // so if %dClient kills %sClient, no PK tag

							//$PKflag[%dClient] = True;
							//Client::sendMessage(%dClient, $MsgRed, "PvP Enabled.");

						}
					}
					if(%dZone == "PROTECTED" && %sZone != "PROTECTED") {
						//echo("guy inside gets hit by guy outside, or vice-versa, no damage");
						%value = 0;
						%isMiss = False;
 						%noImpulse = True;
					}
					else if(%dZone != "PROTECTED" && %sZone != "PROTECTED") {}
					else if(%dZone == "PROTECTED" && %sZone == "PROTECTED" && %sClient != %dClient) {
            	      	//echo("both inside mission area, so no damage");
						%value = 0;
						%isMiss = False;
						%noImpulse = True;
					}
					else if(%dZone == "PROTECTED" && %sZone == "PROTECTED" && %sClient == %dClient){}
				}
			//	else
			//		echo("dClient is a Pker, Thief, or status user do damage");
			}
		}
		if(!IsDead(%this)) {
			%armor = Player::getArmor(%this);
			if(%isMiss) {
				if(%sClient == 0)
					%hitby = "an NPC";
				else
					%hitby = Client::getName(%sClient);

				%time = getIntegerTime(true) >> 5;
				if(%time - %dClient.lastMissMessage > 2) {

					%dClient.lastMissMessage = %time;

					if(!Player::isAiControlled(%dClient))
						remoteEval(%dClient,"ATKText", "<jc>"@%hitby@" MISSED!", false);

					if(!Player::isAiControlled(%sClient))
						remoteEval(%sClient,"ATKText", "<jc>MISS!", true);
				}
				%value = 0;
				return;
			}
		}

		if(%value) {

			if(%value < 0)
				%value = 0;

			%backupValue = %value;

			%rhp = refreshHP(%dClient, %value);

			if(%rhp == -1)
				%value = -1;	//There was an LCK miss
			else {

				if(!%noImpulse) Player::applyImpulse(%this,%mom);
					%noImpulse = "";

				if(%type == $SlashingDamageType || %type == $BludgeoningDamageType || %type == $PiercingDamageType) {
					if((%darmor = Client::getItemType(%dClient, $BodyAccessoryType, "EquipList")) != -1)
						PlaySound($ArmorHitSound[getCroppedItem(%darmor)], %dClientPos);
					else
						PlaySound(SoundHitFlesh, %dClientPos);
				}
				else if(%type != $LandingDamageType && %type != $SpellDamageType)
					PlaySound(SoundArrowHit2, %dClientPos);
			}
			if(Player::isAiControlled(%dClient) && $SpawnBotInfo[%dClient] != "") {
				if(AI::getTarget($BotInfoAiName[%dClient]) != %sClient)
					AI::SelectMovement($BotInfoAiName[%dClient]);
				PlaySound(RandomRaceSound($RACE[%dClient], Hit), %dClientPos);
			}
			Schedule("AttackUpdateSet("@%sClient@", "@%dClient@");", 0.1); //This updates 1/10sec after you attack
																					//I still think it don't update right =\
			%convValue = round(%value * $TribesDamageToNumericDamage);
			if(%convValue > 0) {

				%Val1 = %convValue@" DMG!";
				%Val2 = %convValue@" DMG!";

				if(%Backstab) {
					%Val1 = "Backstabbed! "@%Val1;
					%Val2 = "Backstabbed! "@%Val2;
				}
				else if(%Bash) {
					%Val1 = "Bashed! "@%Val1;
					%Val2 = "Bashed! "@%Val2;
				}
				if(%critical == true) {
					%Val1 = "<f2>Critical hit! <f0>"@%Val1;
					%Val2 = "<f2>Critical hit! <f1>"@%Val2;
				}
			//	if(%HasBonus) {
			//		%Val1 = %Val1@" (+Bonus)";
			//		%Val2 = %Val2@" (+Bonus)";
			//	}
				//--------------------
				//display to involved
				//--------------------
				if(%sClient != %dClient)
					remoteEval(%sClient,"ATKText", "<jc>"@%Val1, true);
				remoteEval(%dClient,"ATKText", "<jc>"@%Val2, false);

			}
			else if(%convValue < 0) {
				//this happens when there's a LCK consequence as miss
				%hitby = Client::getName(%sClient);
				if(!Player::isAiControlled(%sClient))
					remoteEval(%sClient,"ATKText", "<jc>MISS! (LCK)", true);
				if(!Player::isAiControlled(%dClient))
					remoteEval(%dClient,"ATKText", "<jc>"@%hitby@" MISSED! (LCK)", false);
			}

			//-------------------------------------------
			//add entry to %dClient's damagedBy list
			//-------------------------------------------
			//make new entry with shooter's name, as long as both aren't bots
			if(!%sClient == 0 || !%isMiss) {
				if(%sClient == 0)
					%sname = "NPC";
				else
					%sname = Client::getName(%sClient);
				%dname = Client::getName(%dClient);
				if(%sClient != %dClient) {
					for(%i = 1; %i <= $maxDamagedBy; %i++) {
						if($damagedBy[%dname, %i] == "") {
							$damagedBy[%dname, %i] = %sname@" "@%backupValue;
							schedule("$damagedBy[\""@%dname@"\", "@%i@"] = \"\";", $damagedByEraseDelay);
							break;
						}
					}
				}
			}
			if(!Player::isAiControlled(%dClient)) {
				%flash = Player::getDamageFlash(%this) + (%value / 10); //* 2;
				if(%flash > 0.75)
					%flash = 0.75;
				Player::setDamageFlash(%this,%flash);
			}
			//If player not dead then play a random hurt sound
			if(!Player::isDead(%this)) {

				if(!%isMiss) {

					SkillCounter(%sClient, %dClient, %type, 1);

					%svar = $ItemData[%weapon, svar];
					for(%i = 0; (%res = GetWord(%svar, %i)) != -1; %i++) { //echo("res:"@%res);
						%i++;
						if($StatusLookUpList[%res-30] != "") {
							%pow = getWord(%svar, %i);
							Eval("Status::"@$StatusLookUpList[%res-30]@"("@%dClient@", "@%sClient@", "@%pow@");");
						}
					}
				}

				//if(%dClient.lastDamage < getSimTime() && !Player::isAiControlled(%dClient)) {
				//	%sound = radnomItems(3,injure1, injure2, injure3);
				//	playVoice(%dClient, %sound);
				//	%dClient.lastdamage = getSimTime() + 1.5;
				//}
			}
			else {	//player died
				if(%armor == GolemArmor || $fun == true)
					Player::blowUp(%this);	// =]
				else
					Player::setAnimation(%this, $PlayerAnim::DieForwardKneel);

				if(%type == $ImpactDamageType && %object.clLastMount != "")
					%sClient = %object.clLastMount;

				Client::onKilled(%dClient, %sClient, %type);
			}
		}
	}
}

function GetRollDmg(%value, %roll, %def) {

	%atk = NumPercent(%value, (( (%roll/20) + ((getRandom()/2)+0.1) ) * 100)); // (((%roll/20)+0.1)*100));
	//if(%def < 0) %def = 0;
	%def = NumPercent(%def+floor(%def * getRandom()), Between(20, 80));
	%c = %atk - %def;
	if(%c < 1)
		%c = 0;
	return %c;
}

function AttackUpdateSet(%sClient, %dClient) {
	if(!Player::isAiControlled(%dClient))
		remoteEval(%dClient, "RefreshHPset", Fix(getHP(%dClient), %dClient, HP));
	if(!Player::isAiControlled(%sClient))
		remoteEval(%sClient, "RefreshSTAset", Fix(getSTA(%sClient), %sClient, STA));
}

//	degradeable weapons and armor effects
function DegradeableEffects(%object, %this, %weapon) {
	%Client = Player::getclient(%object);
	if(!Player::isAiControlled(%Client)) {
		%count = Client::getItemCount(%Client, %weapon);
		if(%weapon != "" && %count >= 1) {
			if(floor(getRandom() * 1234) == 50) {
				Client::addItemCount(%Client, %weapon, -1);
				Client::sendmessage(%Client, 1, "Your "@$ItemData[%weapon, Name]@" broke!");
				CheckMountWeapon(%Client, %weapon);
			}
		}
	}
	%dClient = Player::getclient(%this);
	if(!Player::isAiControlled(%dClient)) {
		%damagedArmor = "";
		for(%i = 0; (%checkItem = getWord($ClientData[%dClient, "EquipList"], %i)) != -1; %i+=2) {
			if($ItemData[%checkItem, type] == $BodyAccessoryType) {
				%damagedArmor = %checkItem;
				break;
			}
		}
		if(%damagedArmor != "") {
			if(floor(getRandom() * 1234) == 50) {
				Item::onUse(%this, %damagedArmor, true);
				Client::addItemCount(%dClient, getCroppedItem(%damagedArmor), -1);
				Client::sendmessage(%dClient, 1, "Your "@$ItemData[%damagedArmor, Name]@" broke!");
			}
		}
	}
}

function Zombie::Kill(%Client, %this) {
	$isZombie[%Client] = "";
	$RACE[%Client] = $ClientData[%Client, tmpRACE];
	$LCK[%Client]--;
	if(getFinalLCK(%Client) >= 0)
		Client::sendMessage(%Client, $MsgRed, "You have permanently lost an LCK point!");
	Player::setAnimation(%this, $PlayerAnim::DieForwardKneel);
	Player::kill(%Client);
}

function remoteKill(%Client) {
	if(!$HasLoadedAndSpawned[%Client])
		return;
	%player = Client::getOwnedObject(%Client);
	if(%player != -1 && getObjectType(%player) == "Player") { // && !IsDead(%Client) && IsInRoster(%Client) == False) {

		$LCK[%Client]--;
		if(getFinalLCK(%Client) >= 0)
			Client::sendMessage(%Client, $MsgRed, "You have permanently lost an LCK point!");

		$CrtlKed[%Client] = true;
		playNextAnim(%Client);
		Player::kill(%Client);

		Client::onKilled(%Client, %Client, "Self");
	}
}
