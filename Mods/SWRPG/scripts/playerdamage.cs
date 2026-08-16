function Client::onKilled(%clientId, %killerId, %damageType)
{
	dbecho($dbechoMode, "Client::onKilled(" @ %clientId @ ", " @ %killerId @ ", " @ %damageType @ ")");

	//This function is NOT an event, it must be MANUALLY CALLED!
	//At this point, the client can still be queried for getItemCounts, but is not considered an object anymore.

	//we can award the other players exp
	DistributeExpForKilling(%clientId);

	//The player with the killshot gets the official "kill"
	if(!IsInCommaList(fetchData(%killerId, "TempKillList"), Client::getName(%clientId)))
		storeData(%killerId, "TempKillList", AddToCommaList(fetchData(%killerId, "TempKillList"), Client::getName(%clientId)));

	if(%killerId != %clientId)
	{
		//a human player killed %clientId
		%n = Client::getName(%killerId);

		Client::sendMessage(%clientId, 0, "You were killed by " @ %n @ "!");

		//if(fetchData(%killerId, "bounty") == Client::getName(%clientId))
		//	storeData(%killerId, "bounty", fetchData(%clientId, "LVL") @ " !Q@W#E$R%T^Y&U*I(O)P");
	}
	else if(%killerId == %clientId)
	{
		Client::sendMessage(%clientId, 0, "You killed yourself!");
	}
	else if(%damageType == 11 || %damageType == 12 || %damageType == 2)
	{
		Client::sendMessage(%clientId, 0, "You were killed!");
	}

	UnHide(%clientId);

//========================================================================================================================

	echo("GAME: kill " @ %killerId @ " " @ %clientId @ " " @ %damageType);
	%clientId.guiLock = true;
	Client::setGuiMode(%clientId, $GuiModePlay);

	Game::clientKilled(%clientId, %killerId);
}

function Game::clientKilled(%playerId, %killerId)
{
	dbecho($dbechoMode, "Game::clientKilled(" @ %playerId @ ", " @ %killerId @ ")");

	%set = nameToID("MissionCleanup/ObjectivesSet");
	for(%i = 0; (%obj = Group::getObject(%set, %i)) != -1; %i++)
		GameBase::virtual(%obj, "clientKilled", %playerId, %killerId);
}

function Player::onKilled(%this)
{
	dbecho($dbechoMode, "Player::onKilled(" @ %this @ ")");

	//At this point, the client can still be queried for getItemCounts, and is also still an object
	//Player::Kill calls this function

	%clientId = Player::getClient(%this);
	%killerId = fetchData(%clientId, "tmpkillerid");
	storeData(%clientId, "tmpkillerid", "");

	%vehicle = %object.vehicle;
	%vehicle.clLastMount = "";
	%vehicle.pilot = "";
	%object.driver = "";
	%object.vehicle = "";

	//revert
	Client::setControlObject(%clientId.possessId, %clientId.possessId);
	Client::setControlObject(%clientId, %clientId);
	storeData(%clientId.possessId, "dumbAIflag", "");
	$possessedBy[%clientId.possessId] = "";

	if(IsStillArenaFighting(%clientId))
	{
		//player's dueling flag is still at ALIVE, make him DEAD

		%a = GetArenaDuelerIndex(%clientId);
		$ArenaDueler[%a] = GetWord($ArenaDueler[%a], 0) @ " DEAD";

		if(!Player::IsAiControlled(%clientId))
			%clientId.RespawnMeInArena = True;
	}
	else if(IsInRoster(%clientId))
	{
		//player was in the waiting room
		//the only way someone could have died in there is if a player was added
		//to the roster, and an AI was killed to make way for this player.
		//so don't drop lootbag

		if(Player::isAiControlled(%clientId)) //if it was an AI, remove him right away, the same AI never spawns back
			RemoveFromRoster(%clientId);
	}
	else if(fetchData(%clientId, "noDropLootbagFlag"))
	{
		//do nothing
	}
	else
	{
		//player died when not dueling

		//drop lootbag
		%tmploot = "";

		if(fetchData(%clientId, "COINS") > 0)
			%tmploot = %tmploot @ "COINS " @ floor(fetchData(%clientId, "COINS")) @ " ";
		storeData(%clientId, "COINS", 0);

		%max = getNumItems();
		for (%i = 0; %i < %max; %i++)
		{
			%a = getItemData(%i);
			%itemcount = Player::getItemCount(%clientId, %a);

			if(%itemcount)
			{
				%flag = False;

				if(fetchData(%clientId, "LCK") >= 0)
				{
					//currently mounted weapon and all equipped stuff + lore items are thrown into lootbag.
					if(Player::getMountedItem(%clientId, $WeaponSlot) == %a || %a.className == "Equipped" || $LoreItem[%a] == True)
						%flag = True;
				}
				else
					%flag = True;

				if(fetchData(%clientId, "LCK") < 0 && Player::isAiControlled(%clientId))
					%flag = True;

				//if(%a == CastingBlade || %a == CastingSaber)
				if($NoDrop[%a] || $StealProtectedItem[%a])
					%flag = False;		//HARDCODED because we DONT want any players to have this item.
				
				//if($StealProtectedItem[%a])
					//%flag = False;

				if(%flag)
				{
					%b = %a;
					if(%b.className == "Equipped")
						%b = String::getSubStr(%b, 0, String::len(%b)-1);

					if(Player::getMountedItem(%clientId, $WeaponSlot) == %a)
					{
						//special handling for currently held weapon
						%tmploot = %tmploot @ %b @ " 1 ";
						Player::decItemCount(%clientId, %a);
					}
					else
					{
						%tmploot = %tmploot @ %b @ " " @ Player::getItemCount(%clientId, %a) @ " ";
						Player::setItemCount(%clientId, %a, 0);
					}
				}
			}
		}

		if(%tmploot != "")
		{
			if(Player::isAiControlled(%clientId))
				TossLootbag(%clientId, %tmploot, 1, "*", 300);
			else
			{
				%namelist = Client::getName(%clientId) @ ",";
				if(fetchData(%clientId, "LCK") >= 0)
				{
					TossLootbag(%clientId, %tmploot, 5, %namelist, Cap(fetchData(%clientId, "LVL") * 300, 300, 3600));
				}
				else
					TossLootbag(%clientId, %tmploot, 5, %namelist, Cap(fetchData(%clientId, "LVL") * 0.2, 5, "inf"));
			}
		}

		updateSpawnStuff(%clientId);

		//house stuff
		%victimH = fetchData(%clientId, "MyHouse");
		%killerH = fetchData(%killerId, "MyHouse");
		%vhn = GetHouseNumber(%victimH);
		%khn = GetHouseNumber(%killerH);
		if(%vhn != "")
		{
			//a house member is killed

			if(fetchData(%clientId, "LCK") < 0)
			{
				//this house member had no LCK at time of death

				if(fetchData(%clientId, "RankPoints") <= 0)
				{
					//no LCK and no Rank Points! you're booted!
					BootFromCurrentHouse(%clientId, True);
				}
				else
					Client::sendMessage(%clientId, $MsgRed, "You have lost all your Rank Points because you died with 0 LCK!");

				storeData(%clientId, "RankPoints", 0);
			}

			//victim loses two rank points
			Client::sendMessage(%clientId, $MsgWhite, "You lost 2 Rank Points.");
			storeData(%clientId, "RankPoints", 2, "dec");

			if(%khn != "")
			{
				if(%khn != %vhn)
				{
					//both contenders are in a house, different from each other
					Client::sendMessage(%killerId, $MsgWhite, "You gained 1 Rank Point!");
					storeData(%killerId, "RankPoints", 1, "inc");
				}
				else
				{
					//both contenders are in the same house, happens if one target-lists the other.
					Client::sendMessage(%killerId, $MsgWhite, "You lost 1 Rank Point.");
					storeData(%killerId, "RankPoints", 1, "dec");
				}
			}
		}
		else if(%vhn == "" && %khn != "")
		{
			//a house member killed a non-house member. no bonuses or punishments
		}

		//CLEAR!!!!
		if(!IsInArenaDueler(%clientId) && !Player::isAiControlled(%clientId) && fetchData(%clientId, "LCK") < 0)
		{
			//HARDCORE player reset (must turn on $hardcore switch)
			if($hardcore)
			{
				if(fetchData(%clientId, "LVL") > 8 && fetchData(%clientId, "RemortStep") > 0)
				{
					if(!Player::isAiControlled(%killerId))
						ResetPlayer(%clientId);
				}
			}

			storeData(%clientId, "zone", "");	//so the player spawns back at start points
		}
	}

	if(fetchData(%clientId, "deathmsg") != "")
	{
		%kitem = Player::getMountedItem(%killerId, $WeaponSlot);
		%msg = nsprintf(fetchData(%clientId, "deathmsg"), Client::getName(%killerId), Client::getName(%clientId), %kitem.description);
		remoteSay(%clientId, 0, %msg);
	}

	if(Player::isAiControlled(%clientId))
	{
		//event stuff
		%i = GetEventCommandIndex(%clientId, "onkill");
		if(%i != -1)
		{
			%name = GetWord($EventCommand[%clientId, %i], 0);
			%type = GetWord($EventCommand[%clientId, %i], 1);
			%cl = NEWgetClientByName(%name);
			if(%cl == -1)
				%cl = 2048;

			%cmd = String::NEWgetSubStr($EventCommand[%clientId, %i], String::findSubStr($EventCommand[%clientId, %i], ">")+1, 99999);
			%pcmd = ParseBlockData(%cmd, %clientId, %killerid);
			$EventCommand[%clientId, %i] = "";
			schedule("remoteSay(" @ %cl @ ", 0, \"" @ %pcmd @ "\", \"" @ %name @ "\");", 2);
		}
		ClearEvents(%clientId);
	}

	storeData(%clientId, "noDropLootbagFlag", "");

	storeData(%clientId, "SpellCastStep", "");
	%clientId.sleepMode = "";
	refreshHPREGEN(%clientId);
	refreshMANAREGEN(%clientId);

	Client::setControlObject(%clientId, %clientId);
	storeData(%clientId, "dumbAIflag", "");

	//remember the last zone the player was in.
	storeData(%clientId, "lastzone", fetchData(%clientId, "zone"));

	PlaySound(RandomRaceSound(fetchData(%clientId, "RACE"), Death), GameBase::getPosition(%clientId));

//========================================================================================================================

	%clientId.dead = 1;
	if($AutoRespawn > 0)
		schedule("Game::autoRespawn(" @ %clientId @ ");",$AutoRespawn,%clientId);

	Player::setDamageFlash(%this,0.75);

	if(%clientId != -1)
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

		if(!Player::isAiControlled(%this))
			%time = $PlayerCorpseTimeoutValue;
		else
			%time = $CorpseTimeoutValue;
		schedule("GameBase::startFadeOut(" @ %this @ ");", %time, %this);
		schedule("deleteObject(" @ %this @ ");", %time + 2.5, %this);

		Client::setOwnedObject(%clientId, -1);
		Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
		Observer::setOrbitObject(%clientId, %this, 15, 15, 15);
		%clientId.observerMode = "dead";
		%clientId.dieTime = getSimTime();
	}
}

function Player::onDamage(%this,%type,%value,%pos,%vec,%mom,%vertPos,%rweapon,%object,%weapon,%preCalcMiss)
{
	dbecho($dbechoMode2, "Player::onDamage(" @ %this @ ", " @ %type @ ", " @ %value @ ", " @ %pos @ ", " @ %vec @ ", " @ %mom @ ", " @ %vertPos @ ", " @ %rweapon @ ", " @ %object @ ", " @ %weapon @ ", " @ %preCalcMiss @ ")");
	//decho("Player::onDamage(" @ %this @ ", " @ %type @ ", " @ %value @ ", " @ %pos @ ", " @ %vec @ ", " @ %mom @ ", " @ %vertPos @ ", " @ %rweapon @ ", " @ %object @ ", " @ %weapon @ ", " @ %preCalcMiss @ ")");


	if(%type > 49)
	{
		if($TypeToItem[%type] != "")
			%weapon = $TypeToItem[%type];
		else
			messageall(1, "Error with grenades. Someone tell Hazor.");
		%type = $ShrapnelDamageType;
		%ThrownObject = %weapon @ "T";

//if 2, do nothing. If 2.1, do the following. This was the only way I could figure out how to
//do it. -.- tried all sorts of onAdds, adding another variable to the minedata, and so forth.
//Only this has worked out. The damageType variable won't take a text string. =/
		if(%ThrownObject.maxDamage > 2)
			schedule(%ThrownObject @ "::onExp(" @ %object @ ", " @ Player::getClient(%this) @ ", \"" @ %pos @ "\");", 0.01);
	}
	else if(%weapon == "")
	{
		%weapon = %object.LastAmmo;
		%rweapon = %object.LastFired;
	}

	%skilltype = $SkillType[%weapon];

	if(Player::isExposed(%this) && %object != -1 && %type != $NullDamageType && !Player::IsDead(%this))
	{
		%damagedClient = Player::getClient(%this);
		%shooterClient = %object;

		%damagedClientPos = GameBase::getPosition(%damagedClient);
		%shooterClientPos = GameBase::getPosition(%shooterClient);

		%damagedCurrentArmor = GetCurrentlyWearingArmor(%damagedClient);

		//==============
		//PROCESS STATS
		//==============
		%isMiss = False;
		%Backstab = False;
		%Bash = False;
		%arenaNull = False;
		%sameTeamNull = False;

		//------------- CREATE DAMAGE VALUE -------------
		if(%type == $SpellDamageType)
		{
			//For the case of SPELLS, the initial damage has already been determined before calling this function

			%dmg = %value;
			%value = round(((%dmg / 1000) * $PlayerSkill[%shooterClient, %skilltype]));

			%ab = (getRandom() * (fetchData(%damagedClient, "MDEF") / 10)) + 1;
			%value = Cap(%value - %ab, 0, "inf");

			%value = (%value / $TribesDamageToNumericDamage);
		}
		if(%type == $ShrapnelDamageType)
		{
			//For the case of grenades, the radius damage modifier has already been determined before calling this function

			%weapondamage = GetRoll(GetWord(GetAccessoryVar(%weapon, $SpecialVar), 1));

			%value =  round(((%weapondamage / 1000) * $PlayerSkill[%shooterClient, %skilltype]) * %value * 2);

			%ab = (getRandom() * (fetchData(%damagedClient, "DEF") / 10)) + 1;
			%value = Cap(%value - %ab, 1, "inf");

			%value = (%value / $TribesDamageToNumericDamage);
		}
		else if(%type != $LandingDamageType)
		{
			%multi = 1;

			//Backstab
			if(fetchData(%shooterClient, "invisible"))
			{
				%dRot = GetWord(GameBase::getRotation(%damagedClient), 2);
				%sRot = GetWord(GameBase::getRotation(%shooterClient), 2);
				%diff = %dRot - %sRot;
				if(%diff >= -0.9 && %diff <= 0.9)
				{
					if(%skilltype == $SkillLightsabers || %skilltype == $SkillMelee)
					{
						%multi += $PlayerSkill[%shooterClient, $SkillBashing] / 175;
						%Backstab = True;
						%dotherdebugmsg = "\n\nyou were backstabbed";
						%sotherdebugmsg = "\n\nyou successfully backstabbed!";
					}
				}
				if(%shooterClient.adminLevel < 5)
					UnHide(%shooterClient);
			}
			if(fetchData(%damagedClient, "invisible") && %damagedClient.adminLevel < 5)
			{
				UnHide(%damagedClient);
			}

			//Bash
			if(fetchData(%shooterClient, "NextHitBash"))
			{
				if($AccessoryVar[%weapon, $AccessoryType] == $BludgeonAccessoryType || $AccessoryVar[%weapon, $AccessoryType] == $PolearmAccessoryType)
				{
					%multi += $PlayerSkill[%shooterClient, $SkillBashing] / 470;
					%b = GameBase::getRotation(%shooterClient);
					%c = $PlayerSkill[%shooterClient, $SkillBashing] / 15;

					%Bash = True;

					%delay = Cap(101 - fetchData(%shooterClient, "LVL"), 5, 50);
					schedule("storeData(" @ %shooterClient @ ", \"blockBash\", \"\");", %delay);
				}

				storeData(%shooterClient, "NextHitBash", "");
			}

			if(%rweapon != "")
				%rweapondamage = GetRoll(GetWord(GetAccessoryVar(%rweapon, $SpecialVar), 1));
			else
				%rweapondamage = 0;
			if(String::findSubStr(%weapon, "Lightsaber") != -1)
				%weapondamage = AddBonusStatePoints(%shooterClient, "ATK") + GetRoll(GetWord(GetAccessoryVar(%weapon, $SpecialVar), 1)) + (floor(sqrt($PlayerSkill[%clientId, $SkillLightsabers]) + sqrt($PlayerSkill[%clientId, $SkillEnergy])) * 4);
			else
				%weapondamage = GetRoll(GetWord(GetAccessoryVar(%weapon, $SpecialVar), 1));

			%value = round((( (%weapondamage + %rweapondamage) / 1000) * $PlayerSkill[%shooterClient, %skilltype]) * %multi);

			%ab = (getRandom() * (fetchData(%damagedClient, "DEF") / 10)) + 1;
			%value = Cap(%value - %ab, 1, "inf");

			%a = (%value * 0.15);
			%r = round((getRandom() * (%a*2)) - %a);
			%value += %r;
			if(%value < 1)
				%value = 1;

			if(%Bash)	//i'm doing this condition here because %mom is dependant on %value
			{
				%c1 = ( %c / 15 * %value );
				%c2 = %c2 / 10;
				%mom = Vector::getFromRot( %b, %c1, %c2 );
			}

			%value = (%value / $TribesDamageToNumericDamage);
		}

		//------------- DETERMINE MISS OR HIT -------------
		if(%preCalcMiss == "")
		{
			if(%type != $LandingDamageType && %shooterClient != %damagedClient && %shooterClient != 0 && %type != $ShrapnelDamageType)
			{
				if(%type == $SpellDamageType)
					%x = (fetchData(%damagedClient, "MDEF") / 5) + $PlayerSkill[%damagedClient, $SkillSpellResistance] + 5;
				else
					%x = (fetchData(%damagedClient, "DEF") / 5) + $PlayerSkill[%damagedClient, $SkillDodging] + 5;
				%y = $PlayerSkill[%shooterClient, %skilltype] + 5;
	
				%n = %x + %y;
	
				%r = floor(getRandom() * %n) + 1;
	
				if(%r <= %x)
					%isMiss = True;
			}
		}

		//=======================================|WATER CHECKS|=========================================
		//------------------------------------
		// CHECK IF PLAYER LANDED ON WATER
		//------------------------------------
		if(%damagedClient == %shooterClient && %type == $LandingDamageType)
		{
			%object = "";
			for(%i = 0; %i >= -3.15; %i -= 1.57)
			{
				if(GameBase::getLOSInfo(Client::getOwnedObject(%damagedClient), 5, %i @ " 0 0"))
				{
					if(getObjectType($los::object) == "InteriorShape" && String::getSubStr(Object::getName($los::object), 0, 5) == "water")
					{
						%object = $los::object;
						break;
					}
				}
			}
			
			if(%object != "")
			{
				%value *= $waterDamageAmp;
				playSound(SoundSplash1, %damagedClientPos);
			}
		}

		//---------------------------------------
		// CHECK IF PLAYER LANDED WHILE IN WATER
		//---------------------------------------
		if(%damagedClient == %shooterClient && %type == $LandingDamageType)
		{
			if(Zone::getType(fetchData(%damagedClient, "zone")) == "WATER")
				%value *= $waterDamageAmp;
		}
		//============================================================================================

		//-------------------------------------------------
		// IF PLAYER IS ADMIN, NULLIFY LANDING DAMAGE
		// IF PLAYER IS SUPERADMIN, NULLIFY ALL DAMAGE
		//-------------------------------------------------
		if(%damagedClient.adminLevel >= 4 && %type == $LandingDamageType)
			%value = 0;
		if(%damagedClient.adminLevel >= 5)
			%value = 0;

		//-------------------------------------------------
		// If the player has the NoFallDamage flag, i.e.
		// from force jump, nullify landing damage.
		//-------------------------------------------------
		if(%damagedClient.NoFallDamage && %type == $LandingDamageType)
			%value = 0;

		//------------------------------------------------
		// SAME TEAM CHECKS
		//------------------------------------------------
		if(Client::getTeam(%damagedClient) == Client::getTeam(%shooterClient) && %shooterClient != %damagedClient)
		{
			if(!HasTheftFlag(%damagedClient))
			{
				if(Zone::getType(fetchData(%damagedClient, "zone")) == "PROTECTED" && Zone::getType(fetchData(%shooterClient, "zone")) != "PROTECTED")
				{
					//echo("guy inside gets hit by guy outside, or vice-versa, no damage");
					%value = 0;
					%isMiss = False;
					%noImpulse = True;
					%sameTeamNull = True;
				}
				else if(Zone::getType(fetchData(%damagedClient, "zone")) != "PROTECTED" && Zone::getType(fetchData(%shooterClient, "zone")) != "PROTECTED")
				{
					//echo("both guys outside, do damage");
				}
				else if(Zone::getType(fetchData(%damagedClient, "zone")) == "PROTECTED" && Zone::getType(fetchData(%shooterClient, "zone")) == "PROTECTED" && %shooterClient != %damagedClient)
				{
					//echo("both inside, so no damage");
					%value = 0;
					%isMiss = False;
					%noImpulse = True;
					%sameTeamNull = True;
				}

				//not in the same house
				if(

				!IsInCommaList(fetchData(%damagedClient, "targetlist"), Client::getName(%shooterClient)
				||
				IsInCommaList(fetchData(%shooterClient, "targetlist"), Client::getName(%damagedClient))) )

				{
					//no target-list involved
		
					%dhn = GetHouseNumber(fetchData(%damagedClient, "MyHouse"));
					%shn = GetHouseNumber(fetchData(%shooterClient, "MyHouse"));
					if(%dhn == %shn || %dhn == "" || %shn == "")
					{ //either both in same house, or one of the people involved is not in a house, so no damage occurs
						%value = 0;
						%isMiss = False;
						%noImpulse = True;
						%sameTeamNull = True;
					}
				}
				else
				{
					//one of the people involved has the other one on his/her target-list.
					//so let damage go thru
				}
	
				//AI same team check
				//if(Player::isAiControlled(%damagedClient))
				//{
				//	%value = 0;
				//	%isMiss = False;
				//	%noImpulse = True;
				//}
			}
		}
		//-------------------------------------------------
		// SAME PLAYER CHECKS
		//-------------------------------------------------
		if(%damagedClient == %shooterClient)
		{
			if(%type == $SpellDamageType)
				%value = %value / 3;
		}

		//-------------------------------------------------
		// ARENA DAMAGE CHECKS
		//-------------------------------------------------
		if(IsStillArenaFighting(%damagedClient) != IsStillArenaFighting(%shooterClient))
		{
			%value = 0;						//example: spectator shooting in arena
			%arenaNull = True;
		}
		if(IsInRoster(%damagedClient) != IsInRoster(%shooterClient))
		{
			%value = 0;						//example: roster shooting in arena
			%arenaNull = True;
		}
		if(IsInRoster(%damagedClient))
		{
			%value = 0;						//example: arena shooting in roster
			%arenaNull = True;
		}

		if(!IsDead(%this))
		{
			%armor = Player::getArmor(%this);
			storeData(%damagedClient, "tmpkillerid", %shooterClient);

			%hitby = Client::getName(%shooterClient);
			%msgcolor = "";

			if(%isMiss)
			{
				%msgcolor = $MsgRed;
				%value = 0;
			}
			else if(!%isMiss && %value == 0 && %shooterClient != %damagedClient)
			{
				%msgcolor = $MsgWhite;
			}
			if(%msgcolor != "")
			{
				if(%type != $SpellDamageType)
				{
					Client::sendMessage(%shooterClient, %msgcolor, "You try to hit " @ Client::getName(%damagedClient) @ ", but miss!");

					%time = getIntegerTime(true) >> 5;
					if(%time - %damagedClient.lastMissMessage > 2)
					{
						%damagedClient.lastMissMessage = %time;
						Client::sendMessage(%damagedClient, %msgcolor, %hitby @ " tries to hit you, but misses!");
					}
				}
				else
				{
					Client::sendMessage(%shooterClient, %msgcolor, Client::getName(%damagedClient) @ " resists your spell!");
					Client::sendMessage(%damagedClient, %msgcolor, "You resist " @ %hitby @ "'s spell!");
				}
			}

			//-------------------------------------------------
			// SKILLS
			//-------------------------------------------------
			if(%skilltype >= 1 && !%arenaNull && !%sameTeamNull && %shooterClient != %damagedClient)
			{
				%base1 = Cap(35 + (fetchData(%shooterClient, "LVL") - fetchData(%damagedClient, "LVL")), 1, "inf");
				%base2 = Cap(35 + (fetchData(%damagedClient, "LVL") - fetchData(%shooterClient, "LVL")), 1, "inf");
				if(%isMiss)
				{
					UseSkill(%shooterClient, %skilltype, False, True);
					UseSkill(%damagedClient, $SkillEndurance, True, True, 60);
					if(%type == $SpellDamageType)
						UseSkill(%damagedClient, $SkillSpellResistance, True, True, %base2);
					else
						UseSkill(%damagedClient, $SkillDodging, True, True, %base2 * (3/5));
				}
				else if(!%isMiss && %value == 0)
				{
					UseSkill(%shooterClient, %skilltype, False, True);
					UseSkill(%damagedClient, $SkillEndurance, True, True, 60);
					if(%type == $SpellDamageType)
						UseSkill(%damagedClient, $SkillSpellResistance, True, True, %base2);
					else
						UseSkill(%damagedClient, $SkillDodging, True, True, %base2 * (3/5));
				}
				else
				{
					UseSkill(%shooterClient, %skilltype, True, True, %base1);
					if(%type == $SpellDamageType)
						UseSkill(%damagedClient, $SkillSpellResistance, True, True, %base2);
				}

				if(%Backstab)
					UseSkill(%shooterClient, $SkillBackstabbing, True, True);
				if(%Bash)
					UseSkill(%shooterClient, $SkillBashing, True, True);
			}

			if(%value)
			{
				if(%value < 0)
					%value = 0;
				%backupValue = %value;

				%rhp = refreshHP(%damagedClient, %value);

				if(%rhp == -1)
					%value = -1;	//There was an LCK miss
				else
				{
					if(!%noImpulse) Player::applyImpulse(%this,%mom);
					%noImpulse = "";

					if(%damagedCurrentArmor != "")
						%ahs = $ArmorHitSound[%damagedCurrentArmor];
					else
						%ahs = SoundHitFlesh;
					if(%skilltype == $SkillMelee)
						PlaySound(%ahs, %damagedClientPos);
					else if(%skilltype == $SkillLightsabers)
						PlaySound(%ahs, %damagedClientPos);
					else if(%skilltype == $SkillArchery)
					{
						if($BlastColor[%object.LastFired] != "") // Kind of a ghetto way to make it know when it's a blaster and not a bow, but... it works.
							PlaySound(SoundJetHeavyNoLoop, %damagedClientPos);
						else
							PlaySound(SoundArrowHit2, %damagedClientPos);
					}
				}

				if(Player::isAiControlled(%damagedClient) && fetchData(%damagedClient, "SpawnBotInfo") != "")
				{
					if(!IsDead(%damagedClient))
					{
						if(AI::getTarget(fetchData(%damagedClient, "BotInfoAiName")) != %shooterClient)
							AI::SelectMovement(fetchData(%damagedClient, "BotInfoAiName"));
					}

					PlaySound(RandomRaceSound(fetchData(%damagedClient, "RACE"), Hit), %damagedClientPos);
				}

				//display amount of damage caused
				%convValue = round(%value * $TribesDamageToNumericDamage);

				if(%convValue > 0)
				{
					if(%shooterClient == %damagedClient)
					{
						if(%type == $CrushDamageType)
							%hitby = "moving object";
						else if(%type == $DebrisDamageType)
							%hitby = "debris";
						else
							%hitby = "yourself";
					}
					else if(%shooterClient == 0)
						%hitby = "an NPC";
					else
					{
						if(fetchData(%shooterClient, "invisible"))
							%hitby = "an unknown assailant";
						else
							%hitby = Client::getName(%shooterClient);
					}

					if(%Backstab)
					{
						%daction = "backstabbed";
						%saction = "backstabbed";
					}
					else if(%Bash)
					{
						%daction = "bashed";
						%saction = "bashed";
					}
					else
					{
						%daction = "damaged";
						%saction = "damaged";
					}

					//--------------------
					//display to involved
					//--------------------
					if(%shooterClient != %damagedClient)
						Client::sendMessage(%shooterClient, $MsgRed, "You " @ %saction @ " " @ Client::getName(%damagedClient) @ " for " @ %convValue @ " points of damage!");
					Client::sendMessage(%damagedClient, $MsgRed, "You were " @ %daction @ " by " @ %hitby @ " for " @ %convValue @ " points of damage!");

					//--------------------
					//display to radius
					//--------------------
					if(%shooterClient == 0)
					{
						%sname = "An NPC";
						%dname = Client::getName(%damagedClient);
					}
					else if(%shooterClient == %damagedClient)
					{
						%sname = Client::getName(%shooterClient);
						if(String::ICompare(Client::getGender(%damagedClient), "Male") == 0)
							%dname = "himself";
						else if(String::ICompare(Client::getGender(%damagedClient), "Female") == 0)
							%dname = "herself";
						else
							%dname = "itself";
					}
					else
					{
						if(fetchData(%shooterClient, "invisible"))
							%sname = "An unknown assailant";
						else
							%sname = Client::getName(%shooterClient);
						%dname = Client::getName(%damagedClient);
					}

					radiusAllExcept(%damagedClient, %shooterClient, %sname @ " hits " @ %dname @ " for " @ %convValue @ " points of damage!");
				}
				else if(%convValue < 0)
				{
					//this happens when there's a LCK consequence as miss

					%hitby = Client::getName(%shooterClient);

					Client::sendMessage(%shooterClient, $MsgRed, "You try to hit " @ Client::getName(%damagedClient) @ ", but miss! (LCK)");
					Client::sendMessage(%damagedClient, $MsgRed, %hitby @ " tries to hit you, but misses! (LCK)");
				}

				//-------------------------------------------
				//add entry to damagedClient's damagedBy list
				//-------------------------------------------

				//make new entry with shooter's name
				if( %shooterClient != 0 && !%isMiss)
				{
					if(%shooterClient == 0)
						%sname = "NPC";
					else
						%sname = Client::getName(%shooterClient);
					%dname = Client::getName(%damagedClient);
					if(%shooterClient != %damagedClient)
					{
						%index = "";
						for(%i = 1; %i <= $maxDamagedBy; %i++)
						{
							if($damagedBy[%dname, %i] == "" && %index == "")
								%index = %i;
						}
						if(%index != "")
						{
							$damagedBy[%dname, %index] = %sname @ " " @ %backupValue;
							schedule("$damagedBy[\"" @ %dname @ "\", " @ %index @ "] = \"\";", $damagedByEraseDelay);
						}
						else
						{
							//too many hits on waiting list, he doesn't get in on exp.
						}
					}
				}

				%flash = Player::getDamageFlash(%this) + %value * 2;
				if(%flash > 0.75)
					%flash = 0.75;
				Player::setDamageFlash(%this,%flash);

				if(%type == $LandingDamageType)
				{
					%pos = Gamebase::getposition(%this);
					%trans = "0 0 -1 0 0 -1 0 0 -1 " @ %pos;
					echo(getObjectType(%object));
					%proj = Projectile::spawnProjectile("LandingDust",%trans,%this,%vel);
					//%proj = Projectile::spawnProjectile("LandingSnow",%trans,%this,%vel);
				}

				//slow the player down for a bit
				if($SlowdownHitDelay > 0 && !Player::isAiControlled(%damagedClient))
				{
					storeData(%damagedClient, "SlowdownHitFlag", True);
					RefreshWeight(%damagedClient);
					schedule("storeData(" @ %damagedClient @ ", \"SlowdownHitFlag\", False);", $SlowdownHitDelay);
					schedule("RefreshWeight(" @ %damagedClient @ ");", $SlowdownHitDelay, Client::getOwnedObject(%damagedClient));
				}

				//If player not dead then play a random hurt sound
				if(!Player::IsDead(%this))
				{
					if(%damagedClient.lastDamage < getSimTime())
					{
						//%sound = radnomItems(3,injure1,injure2,injure3);
						//playVoice(%damagedClient,%sound);
						%damagedClient.lastdamage = getSimTime() + 1.5;
					}
				}
				else		//player died
				{
					if(Player::isAiControlled(%shooterClient))
					{
						RemotePlayAnim(%shooterClient, 8);
						PlaySound(RandomRaceSound(fetchData(%shooterClient, "RACE"), Taunt), %shooterClientPos);
					}

					if( Player::isCrouching(%this) )
						%curDie = $PlayerAnim::Crouching;
					else
						%curDie = radnomItems(3, $PlayerAnim::DieLeftSide, $PlayerAnim::DieChest, $PlayerAnim::DieForwardKneel);

					Player::setAnimation(%this, %curDie);

					if(%type == $ImpactDamageType && %object.clLastMount != "")
						%shooterClient = %object.clLastMount;

					Client::onKilled(%damagedClient, %shooterClient, %type);
				}
			}

			if(%isMiss)
			{
				if(fetchData(%damagedClient, "isBonused"))
				{
					GameBase::activateShield(%this, "0 0 1.57", 1.47);
					PlaySound(SoundHitShield, %damagedClientPos);
				}
			}
		}
	}
}

function remoteKill(%clientId)
{
	dbecho($dbechoMode2, "remoteKill(" @ %clientId @ ")");

	if(!$matchStarted)
		return;
	if(IsJailed(%clientId))
		return;

	%player = Client::getOwnedObject(%clientId);
	if(%player != -1 && getObjectType(%player) == "Player" && !IsDead(%clientId) && IsInRoster(%clientId) == False)
	{
		storeData(%clientId, "LCK", 1, "dec");

		if(fetchData(%clientId, "LCK") >= 0)
			Client::sendMessage(%clientId, $MsgRed, "You have permanently lost an LCK point!");

		playNextAnim(%clientId);
		Player::kill(%clientId);
	}
}
