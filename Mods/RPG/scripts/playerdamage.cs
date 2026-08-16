//This file is part of Tribes RPG.
//Tribes RPG server side scripts
//Written by Jason "phantom" Daley,  Matthiew "JeremyIrons" Bouchard, and more (yet undetermined)

//	Copyright (C) 2014  Jason Daley

//	This program is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.

//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.

//	You should have received a copy of the GNU General Public License
//	along with this program.  If not, see <http://www.gnu.org/licenses/>.

//You may contact the author at beatme101@gmail.com or www.tribesrpg.org/contact.php

//This GPL does not apply to Starsiege: Tribes or any non-RPG related files included.
//Starsiege: Tribes, including the engine, retains a proprietary license forbidding resale.



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

	%iscp = Player::isAIcontrolled(%clientId);	if(%iscp)	{
		%AIname = fetchData(%clientId, "BotInfoAiName");
		storeData(%clientId, "frozen", True);		AI::setVar(%AIname, SpotDist, 0);		AI::newDirectiveRemove(%AIname, 99);		ai::callbackPeriodic(%aiName, 0, AI::Periodic);	}

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
				if($neverdropitem[%a] == True)
					%flag = False;//We don't want these to be found in loot bags. Typically set in globals.cs.

				if($StealProtectedItem[%a])
					%flag = False;

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
					if(String::len(%tmploot) > 200)
					{
						if(Player::isAiControlled(%clientId))
							TossLootbag(%clientId, %tmploot, 1, "*", 300);
						else
						{
							%namelist = Client::getName(%clientId) @ ",";
							if(fetchData(%clientId, "LCK") >= 0)
								%tehLootBag = TossLootbag(%clientId, %tmploot, 5, %namelist, Cap(fetchData(%clientId, "LVL") * 300, 300, 3600));
							else
								%tehLootBag = TossLootbag(%clientId, %tmploot, 5, %namelist, Cap(fetchData(%clientId, "LVL") * 0.2, 5, "inf"));
						}
						%tmploot = "";
					}
				}
			}
		}
		%weapon = Player::getMountedItem(%clientId,$WeaponSlot);		if(%weapon != -1) {			Player::unMountItem(%clientId,$WeaponSlot);		}
		%beltstuff = Belt::GetDeathItems(%clientid, %killerId);		if((String::len(%tmploot) + String::len(%beltstuff)) < 200)			%tmploot = %tmploot @ %beltstuff;		else		{			if(Player::isAiControlled(%clientId))				TossLootbag(%clientId, %beltstuff, 1, "*", 300);			else			{				%namelist = rpg::getname(%clientId) @ ",";				if(fetchData(%clientId, "LCK") >= 0)					%tehLootBag = TossLootbag(%clientId, %beltstuff, 5, %namelist, 0);				else					%tehLootBag = TossLootbag(%clientId, %beltstuff, 5, %namelist, Cap(fetchData(%clientId, "LVL") * 0.2, 5, "inf"));			}			%beltstuff = "";		}

		if(%tmploot != "" && %tmploot != " ")
		{
			if(Player::isAiControlled(%clientId))
				TossLootbag(%clientId, %tmploot, 1, "*", 300);
			else
			{
				%namelist = Client::getName(%clientId) @ ",";
				if(fetchData(%clientId, "LCK") >= 0)
					TossLootbag(%clientId, %tmploot, 5, %namelist, Cap(fetchData(%clientId, "LVL") * 300, 300, 3600));
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
		internalSay(%clientId, 0, %msg);
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
			schedule("internalSay(" @ %cl @ ", 0, \"" @ %pcmd @ "\", \"" @ %name @ "\");", 2);
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
		if(!Player::isAiControlled(%clientId)){

			%clientId.dead = 1;
			if($AutoRespawn > 0)
				schedule("Game::autoRespawn(" @ %clientId @ ");",$AutoRespawn,%clientId);

			Player::setDamageFlash(%this,0.75);

			//This AI check fixes spam related to a horrible mass-crashing bug,
			//but it does not seem to fix the crashing bug itself.
			//If you happen to know how to completely fix these problems,
			//please contact me at beatme101.com or beatme101@gmail.com.
			//This is as far as I could track the bug. It seems that this function,
			//when called on an AI under unknown circumstances, could cause
			//the AI to not be considered an AI by Player::isAiControlled.
			//Once the player is not considered an AI, other sets of code start to get confused.
			//The spam came from a section of code meant to handle a character's weight after being hit.
			//The function got called with a delay after the AI died, and it fails to check if the AI is dead,
			//so the server's console recieves a large amount of spam shortly after the AI dies.
			//The following function causes the bug if called on AI.
			Client::setOwnedObject(%clientId, -1);
			Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
			Observer::setOrbitObject(%clientId, %this, 15, 15, 15);
			//.properties can't be set on AI
			%clientId.observerMode = "dead";
			%clientId.dieTime = getSimTime();
		}
		storeData(%clientId, "isDead", True);
		schedule("GameBase::startFadeOut(" @ %this @ ");", $CorpseTimeoutValue, %this);
		schedule("deleteObject(" @ %this @ ");", $CorpseTimeoutValue + 2.5, %this);
	}
}

function Player::onDamage(%this,%type,%value,%pos,%vec,%mom,%vertPos,%rweapon,%object,%weapon,%preCalcMiss)
{
	dbecho($dbechoMode2, "Player::onDamage(" @ %this @ ", " @ %type @ ", " @ %value @ ", " @ %pos @ ", " @ %vec @ ", " @ %mom @ ", " @ %vertPos @ ", " @ %rweapon @ ", " @ %object @ ", " @ %weapon @ ", " @ %preCalcMiss @ ")");

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
					if(%skilltype == $SkillPiercing)
					{
						%multi += $PlayerSkill[%shooterClient, $SkillBackstabbing] / 175;
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
				%delay = 1;
				if(%skilltype == $SkillBludgeoning)
				{
					%multi += $PlayerSkill[%shooterClient, $SkillBashing] / 470;
					%shooterRotation = GameBase::getRotation(%shooterClient);
					%c = $PlayerSkill[%shooterClient, $SkillBashing] / 15;

					%Bash = True;

					%delay = Cap(101 - fetchData(%shooterClient, "LVL"), 5, 50);
				}				if(%shooterClient.repack > 33)					remoteEval(%shooterClient, "rpgbarhud", %delay, 7, 2, "||", 1, "Bash regen");

				schedule("storeData(" @ %shooterClient @ ", \"blockBash\", \"\");", %delay);
				storeData(%shooterClient, "NextHitBash", "");
			}

			if(%rweapon != "")
				%rweapondamage = GetRoll(GetWord(GetAccessoryVar(%rweapon, $SpecialVar), 1));
			else
				%rweapondamage = 0;
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
				%c1 = cap(%c1, 0, 1000);
				%c2 = %c1 / 10;
				%mom = Vector::getFromRot( %shooterRotation, %c1, %c2 );
			}

			%value = (%value / $TribesDamageToNumericDamage);
		}

		//------------- DETERMINE MISS OR HIT -------------
		if(%preCalcMiss == "")
		{
			if(%type != $LandingDamageType && %shooterClient != %damagedClient && %shooterClient != 0)
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
				if( !(IsInCommaList(fetchData(%damagedClient, "targetlist"), Client::getName(%shooterClient)) || IsInCommaList(fetchData(%shooterClient, "targetlist"), Client::getName(%damagedClient))) )
				{
					//no target-list involved
		
					%dhn = GetHouseNumber(fetchData(%damagedClient, "MyHouse"));
					%shn = GetHouseNumber(fetchData(%shooterClient, "MyHouse"));
					if(%dhn == %shn)
					{
						%value = 0;
						%isMiss = False;
						%noImpulse = True;
						%sameTeamNull = True;
					}
					else
					{
						if(%dhn == "" || %shn == "")
						{
							//one of the people involved is not in a house, so no damage occurs
							%value = 0;
							%isMiss = False;
							%noImpulse = True;
							%sameTeamNull = True;
						}
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
			%hitpresent = $skillHitPresent[%skilltype];			if(%hitpresent == "")				%hitpresent = "hit";

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
					newprintmsg(%shooterClient,"You try to "@%hitpresent@" <f1>" @ rpg::getname(%damagedClient) @ "<ff>, but miss!", %msgcolor);

					%time = getIntegerTime(true) >> 5;
					if(%time - %damagedClient.lastMissMessage > 2)
					{
						%damagedClient.lastMissMessage = %time;
						newprintmsg(%damagedClient,"<f1>"@%hitby @ "<ff> tries to "@%hitpresent@" you, but misses!", %msgcolor);
					}
				}
				else
				{
					newprintmsg(%shooterClient, "<f1>"@rpg::getname(%damagedClient) @ "<ff> resists your spell!", %msgcolor);
					newprintmsg(%damagedClient, "You resist <f1>" @ %hitby @ "<ff>'s spell!", %msgcolor);
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

				%prehithp = fetchData(%damagedClient,"HP");

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
					if(%skilltype == $SkillSlashing)
						PlaySound(%ahs, %damagedClientPos);
					else if(%skilltype == $SkillBludgeoning)
						PlaySound(%ahs, %damagedClientPos);
					else if(%skilltype == $SkillPiercing)
						PlaySound(%ahs, %damagedClientPos);
					else if(%skilltype == $SkillArchery)
						PlaySound(SoundArrowHit2, %damagedClientPos);
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
					{						%hitpast = $skillHitPast[%skilltype];						if(%hitpast == "")							%hitpast = "damaged";						%daction = %hitpast;						%saction = %hitpast;
					}

					//--------------------
					//display to involved
					//--------------------
					if(%shooterClient != %damagedClient)
						newprintmsg(%shooterClient, "You " @ %saction @ " <f1>" @ rpg::getname(%damagedClient) @ "<ff> - <f2>" @ %convValue @ "<f0> points", $MsgRed);
					newprintmsg(%damagedClient, "You were " @ %daction @ " by <f1>" @ %hitby @ "<ff> - <f2>" @ %convValue @ "<ff> points", $MsgRed);

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
					}					%hitpresentrad = %hitpresent;					if(%hitpresentrad == "slash")						%hitpresentrad = "slashe";
					radiusAllExcept(%damagedClient, %shooterClient, "<f1>"@%sname @ "<ff> "@%hitpresentrad@"s " @ %dname @ " for <f2>" @ %convValue @ "<ff> points of damage!", true);
				}
				else if(%convValue < 0)
				{
					//this happens when there's a LCK consequence as miss

					%hitby = Client::getName(%shooterClient);

					newprintmsg(%shooterClient, "You try to "@%hitpresent@" <f1>" @ rpg::getname(%damagedClient) @ "<ff>, but miss! (LCK)", $MsgRed);
					newprintmsg(%damagedClient, "<f1>"@%hitby @ "<ff> tries to "@%hitpresent@" you, but misses! (LCK)", $MsgRed);
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
				%flash = %prehithp;				%flash = %convValue / %flash;				%flash += 0.05;				if (%flash > 1)					%flash = 1;				Player::SetDamageFlash(%this,%flash);				%blood = floor(%flash*10);				for(%i=1; %i <= %blood; %i++)					bloodSpray(%damagedClient);

				//slow the player down for a bit
				if(!Player::isAiControlled(%damagedClient))
				{
					//This idea caused too many problems, really. The solution is more complicated but less problems.
				//	storeData(%damagedClient, "SlowdownHitFlag", True);
				//	RefreshWeight(%damagedClient);
				//	schedule("storeData(" @ %damagedClient @ ", \"SlowdownHitFlag\", False);", $SlowdownHitDelay);
				//	schedule("RefreshWeight(" @ %damagedClient @ ");", $SlowdownHitDelay, Client::getOwnedObject(%damagedClient));
				}

				//If player not dead then play a random hurt sound
				if(!Player::IsDead(%this))
				{
					if(%damagedClient.lastDamage < getSimTime())
					{
						%sound = radnomItems(3,injure1,injure2,injure3);
						playVoice(%damagedClient,%sound);
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
function bloodSpray(%client){
	if(BloodSpot.shapefile == False)
		return;	$los::object = "";	%player = Client::getOwnedObject(%client);	if(GameBase::getLOSInfo(%player,5,(getRandom()*6.29317)@" "@(getRandom()*6.29317)@" "@(getRandom()*6.29317))){		%target = $los::object;		%obj = getObjectType(%target);		if(%obj != "Player"){			%blood = newObject("Blood", StaticShape, BloodSpot, true);			%finalpos = $los::position;			gamebase::setposition(%blood,%finalpos);			gamebase::setRotation(%blood,vector::add(vector::getrotation($los::normal),"1.57 0 0"));			Item::pop(%blood);		}	}}

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
