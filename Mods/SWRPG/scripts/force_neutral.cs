//-- Neutral SPELL DEFINITIONS------------------------------------------------------------------------------------
//NOT defined alphabetically until new powers (after mimic)

$si = $lightfpowers;

$Spell::keyword[$si++] = "teleport";
$Spell::index[teleport] = $si;
$Spell::name[$si] = "Teleport close to nearest zone";
$Spell::description[$si] = "Teleports you near a zone";
$Spell::delay[$si] = 3.5;
$Spell::recoveryTime[$si] = 16.5;
$Spell::manaCost[$si] = 8;
$Spell::startSound[$si] = Portal11;
$Spell::endSound[$si] = ActivateCH;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = 0;
$Spell::graceDistance[$si] = 2;
$SkillType[teleport] = $SkillNeutralCasting;

$Spell::keyword[$si++] = "advteleport";
$Spell::index[advteleport] = $si;
$Spell::name[$si] = "Advanced Teleport close to nearest zone";
$Spell::description[$si] = "Teleport self OR person in line-of-sight close to nearest zone.";
$Spell::delay[$si] = 3.5;
$Spell::recoveryTime[$si] = 16.5;
$Spell::manaCost[$si] = 8;
$Spell::startSound[$si] = Portal11;
$Spell::endSound[$si] = ActivateCH;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = 0;
$Spell::graceDistance[$si] = 2;
$SkillType[teleport] = $SkillNeutralCasting;

$Spell::keyword[$si++] = "transport";
$Spell::index[transport] = $si;
$Spell::name[$si] = "Transport to zone";
$Spell::description[$si] = "Transports to a specific zone";
$Spell::delay[$si] = 4.0;
$Spell::recoveryTime[$si] = 23;
$Spell::manaCost[$si] = 12;
$Spell::startSound[$si] = RespawnB;
$Spell::endSound[$si] = ActivateCH;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = 0;
$Spell::graceDistance[$si] = 2;
$SkillType[transport] = $SkillNeutralCasting;

$Spell::keyword[$si++] = "advtransport";
$Spell::index[advtransport] = $si;
$Spell::name[$si] = "Advanced Transport to zone";
$Spell::description[$si] = "Transports self OR person in line-of-sight to a specific zone";
$Spell::delay[$si] = 4.0;
$Spell::recoveryTime[$si] = 27;
$Spell::LOSrange[$si] = 500;
$Spell::manaCost[$si] = 16;
$Spell::startSound[$si] = RespawnB;
$Spell::endSound[$si] = ActivateCH;
$Spell::groupListCheck[$si] = True;
$Spell::refVal[$si] = 0;
$Spell::graceDistance[$si] = 2;
$SkillType[advtransport] = $SkillNeutralCasting;

$Spell::keyword[$si++] = "masstransport";
$Spell::index[masstransport] = $si;
$Spell::name[$si] = "Mass Transport";
$Spell::description[$si] = "Transports self and all friendlies within a 6 meter radius to a specific zone.";
$Spell::delay[$si] = 4.0;
$Spell::recoveryTime[$si] = 45;
$Spell::radius[$si] = 6;
$Spell::manaCost[$si] = 50;
$Spell::startSound[$si] = RespawnB;
$Spell::endSound[$si] = ActivateCH;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = 0;
$Spell::graceDistance[$si] = 2;
$SkillType[masstransport] = $SkillNeutralCasting;

$Spell::keyword[$si++] = "transportfriend";
$Spell::index[transportfriend] = $si;
$Spell::name[$si] = "Transport friend to LOS";
$Spell::description[$si] = "Transports specified person in grouplist to line-of-sight. (Prompts to accept)";
$Spell::delay[$si] = 4.0;
$Spell::recoveryTime[$si] = 27;
$Spell::LOSrange[$si] = 500;
$Spell::manaCost[$si] = 16;
$Spell::startSound[$si] = RespawnB;
$Spell::endSound[$si] = ActivateCH;
$Spell::groupListCheck[$si] = True;
$Spell::refVal[$si] = 0;
$Spell::graceDistance[$si] = 2;
$SkillType[transportfriend] = $SkillNeutralCasting;

$Spell::keyword[$si++] = "masstransportfriend";
$Spell::index[masstransportfriend] = $si;
$Spell::name[$si] = "Mass Transport friends to LOS";
$Spell::description[$si] = "Transports all persons in grouplist to line-of-sight. (Prompts each to accept)";
$Spell::delay[$si] = 4.0;
$Spell::recoveryTime[$si] = 45;
$Spell::radius[$si] = 6;
$Spell::manaCost[$si] = 50;
$Spell::startSound[$si] = RespawnB;
$Spell::endSound[$si] = ActivateCH;
$Spell::groupListCheck[$si] = False; //Done by the spell itself, instead of in pre-casting functions.
$Spell::refVal[$si] = 0;
$Spell::graceDistance[$si] = 2;
$SkillType[masstransportfriend] = $SkillNeutralCasting;

$Spell::keyword[$si++] = "teleportlos";
$Spell::index[teleportlos] = $si;
$Spell::name[$si] = "Transport to LOS";
$Spell::description[$si] = "Transports specified person in grouplist to line-of-sight. (Prompts to accept)";
$Spell::delay[$si] = 4.0;
$Spell::recoveryTime[$si] = 27;
$Spell::LOSrange[$si] = 500;
$Spell::manaCost[$si] = 16;
$Spell::startSound[$si] = RespawnB;
$Spell::endSound[$si] = ActivateCH;
$Spell::groupListCheck[$si] = True;
$Spell::refVal[$si] = 0;
$Spell::graceDistance[$si] = 2;
$SkillType[teleportlos] = $SkillNeutralCasting;

$Spell::keyword[$si++] = "remort";
$Spell::index[remort] = $si;
$Spell::name[$si] = "Remort";
$Spell::description[$si] = "Remorts a level 101 character to level 1, with bonuses.";
$Spell::delay[$si] = 3.0;
$Spell::recoveryTime[$si] = 1;
$Spell::damageValue[$si] = 0;
$Spell::manaCost[$si] = 1;
$Spell::startSound[$si] = RespawnA;
$Spell::endSound[$si] = RespawnC;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = 0;
$Spell::graceDistance[$si] = 2;
$SkillType[remort] = $SkillNeutralCasting;

$Spell::keyword[$si++] = "mimic";
$Spell::index[mimic] = $si;
$Spell::name[$si] = "Mimic";
$Spell::description[$si] = "A very dangerous spell that transforms the caster into the creature in his/her LOS.";
$Spell::delay[$si] = 4.0;
$Spell::recoveryTime[$si] = 60;
$Spell::LOSrange[$si] = 80;
$Spell::damageValue[$si] = 0;
$Spell::manaCost[$si] = 80;
$Spell::startSound[$si] = LoopSP;
$Spell::endSound[$si] = AbsorbABS;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = 1;
$Spell::graceDistance[$si] = 2;
$SkillType[mimic] = $SkillNeutralCasting;

//New powers. Get alphabetical, nao.

$Spell::keyword[$si++] = "grip";
$Spell::index[$Spell::keyword[$si]] = $si;
$Spell::name[$si] = "Force Grip";
$Spell::description[$si] = "Let's you hold people and objects in the air";
$Spell::delay[$si] = 0;
$Spell::recoveryTime[$si] = 1.5;
$Spell::LOSrange[$si] = 80;
$Spell::manaCost[$si] = 20;
$Spell::startSound[$si] = ActivateAB;
$Spell::endSound[$si] = LaunchFB;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = 10;
$Spell::graceDistance[$si] = 5;
$SkillType[$Spell::keyword[$si]] = $SkillNeutralCasting;

$Spell::keyword[$si++] = "jump";
$Spell::index[$Spell::keyword[$si]] = $si;
$Spell::name[$si] = "Force Jump";
$Spell::description[$si] = "Let's you jump really high!";
$Spell::delay[$si] = 0;
$Spell::recoveryTime[$si] = 1.5;
$Spell::manaCost[$si] = 20;
$Spell::startSound[$si] = ActivateAB;
$Spell::endSound[$si] = LaunchFB;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = 10;
$Spell::graceDistance[$si] = 5;
$SkillType[$Spell::keyword[$si]] = $SkillNeutralCasting;

$Spell::keyword[$si++] = "push";
$Spell::index[$Spell::keyword[$si]] = $si;
$Spell::name[$si] = "Force Push";
$Spell::description[$si] = "Throws your enemies from you.";
$Spell::delay[$si] = 0;
$Spell::recoveryTime[$si] = 1.5;
$Spell::radius[$si] = 10;
$Spell::damageValue[$si] = "70"; //Push strength? No, that'll be based on skill level
$Spell::LOSrange[$si] = 80;
$Spell::manaCost[$si] = 1;
$Spell::startSound[$si] = ActivateAB;
$Spell::endSound[$si] = LaunchFB;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = 20;
$Spell::graceDistance[$si] = 5;
$SkillType[$Spell::keyword[$si]] = $SkillNeutralCasting;

$Spell::keyword[$si++] = "pull";
$Spell::index[$Spell::keyword[$si]] = $si;
$Spell::name[$si] = "Force Pull";
$Spell::description[$si] = "Pulls the objects you face toward you.";
$Spell::delay[$si] = 0;
$Spell::recoveryTime[$si] = 1.5;
$Spell::radius[$si] = 10;
$Spell::damageValue[$si] = "70";
$Spell::LOSrange[$si] = 80;
$Spell::manaCost[$si] = 1;
$Spell::startSound[$si] = ActivateAB;
$Spell::endSound[$si] = LaunchFB;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = 20;
$Spell::graceDistance[$si] = 5;
$SkillType[$Spell::keyword[$si]] = $SkillNeutralCasting;

$Spell::keyword[$si++] = "speed";
$Spell::index[$Spell::keyword[$si]] = $si;
$Spell::name[$si] = "Force Speed";
$Spell::description[$si] = "Increases your running speed temporarily.";
$Spell::delay[$si] = 2.0;
$Spell::recoveryTime[$si] = 12;
$Spell::damageValue[$si] = "DEF 70";
$Spell::ticks[$si] = 190;	//6:20 minutes
$Spell::manaCost[$si] = 15;
$Spell::startSound[$si] = ActivateTR;
$Spell::endSound[$si] = ActivateTD;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = -12;
$Spell::graceDistance[$si] = 2;
$SkillType[$Spell::keyword[$si]] = $SkillNeutralCasting;

$Spell::keyword[$si++] = "sense";
$Spell::index[$Spell::keyword[$si]] = $si;
$Spell::name[$si] = "Force Sense";
$Spell::description[$si] = "Allows you to .. do something. Not sure what yet..";
$Spell::delay[$si] = 3.0;
$Spell::recoveryTime[$si] = 12;
$Spell::ticks[$si] = 190;
$Spell::manaCost[$si] = 15;
$Spell::startSound[$si] = ActivateTR;
$Spell::endSound[$si] = ActivateTD;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = -12;
$Spell::graceDistance[$si] = 2;
$SkillType[$Spell::keyword[$si]] = $SkillNeutralCasting;

$Spell::keyword[$si++] = "stealth";
$Spell::index[$Spell::keyword[$si]] = $si;
$Spell::name[$si] = "Force Stealth";
$Spell::description[$si] = "Makes you invisible, much like the #hide command, but based on the force attunement skill instead of the hiding skill.";
$Spell::delay[$si] = 2.0;
$Spell::recoveryTime[$si] = 12;
$Spell::ticks[$si] = 190;
$Spell::LOSrange[$si] = 8;
$Spell::manaCost[$si] = 15;
$Spell::startSound[$si] = ActivateTR;
$Spell::endSound[$si] = ActivateTD;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = -12;
$Spell::graceDistance[$si] = 2;
$SkillType[$Spell::keyword[$si]] = $SkillNeutralCasting;

$neutralfpowers = $si;
$totalpowers = $si;

//-------------------------------------------------------------------------------------------------------------

function DoNeutralPowers(%clientId, %index, %oldpos, %castPos, %castObj, %player, %w2)
{
	if(%index == $Spell::index[teleport])
	{
		//teleport zone spell

		%zoneId = GetNearestZone(%clientId, %w2, 3);

		if(%zoneId != False)
		{
			Client::sendMessage(%clientId, $MsgBeige, "Teleporting near " @ Zone::getDesc(%zoneId));

			//teleport

			%mpos = Zone::getMarker(%zoneId);
			if(!fetchData(%clientId, "invisible"))
				GameBase::startFadeIn(%clientId);

			GameBase::setPosition(%clientId, %mpos);
			CheckAndBootFromArena(%clientId);
			NullItemList(%clientId, Lore, $MsgRed, "You lost all %1s you were carrying when you teleported.");

			Player::setDamageFlash(%clientId, 0.7);
			%extraDelay = 0.22;	//sometimes the endSound doesn't get played unless there is sufficient delay

			%castPos = SetOnGround(%clientId, 500);
			//%castPos = %newpos;

			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Teleportation failed.");
			%returnFlag = False;
		}
	}

	if(%index == $Spell::index[advteleport])
	{
		//adv teleport zone spell

		%zoneId = GetNearestZone(%clientId, %w2, 3);

		if(%zoneId != False)
		{
			Client::sendMessage(%clientId, $MsgBeige, "Teleporting near " @ Zone::getDesc(%zoneId));

			//teleport

			%mpos = Zone::getMarker(%zoneId);
			if(!fetchData(%clientId, "invisible"))
				GameBase::startFadeIn(%clientId);

			GameBase::setPosition(%clientId, %mpos);
			CheckAndBootFromArena(%clientId);
			NullItemList(%clientId, Lore, $MsgRed, "You lost all %1s you were carrying when you teleported.");

			Player::setDamageFlash(%clientId, 0.7);
			%extraDelay = 0.22;	//sometimes the endSound doesn't get played unless there is sufficient delay

			%castPos = SetOnGround(%clientId, 500);
			//%castPos = %newpos;

			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Teleportation failed.");
			%returnFlag = False;
		}
	}

	if(%index == $Spell::index[transport]) //Transport
	{
		//Transport zone spell

		%zoneId = GetZoneByKeywords(%clientId, %w2, 3);

		if(%zoneId != False)
		{
			Client::sendMessage(%clientId, $MsgBeige, "Transporting to " @ Zone::getDesc(%zoneId));

			//teleport

			%system = Object::getName(%zoneId);
			%type = GetWord(%system, 0);
			%desc = String::getSubStr(%system, String::len(%type)+1, 9999);

			%castPos = TeleportToMarker(%clientId, "Zones\\" @ %system @ "\\DropPoints", False, True);
			CheckAndBootFromArena(%clientId);
			NullItemList(%clientId, Lore, $MsgRed, "You lost all %1s you were carrying when you teleported.");

			if(!fetchData(%clientId, "invisible"))
				GameBase::startFadeIn(%clientId);

			Player::setDamageFlash(%clientId, 0.7);
			%extraDelay = 0.22;	//sometimes the endSound doesn't get played unless there is sufficient delay

			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Transportation failed.");
			%returnFlag = False;
		}
	}

	if(%index == $Spell::index[advtransport]) //AdvTransport
	{
		//Advanced Transport zone spell

		%zoneId = GetZoneByKeywords(%clientId, %w2, 3);

		if(%zoneId != False)
		{
			if(getObjectType(%castObj) == "Player")
				%id = Player::getClient(%castObj);
			else
				%id = %clientId;

			Client::sendMessage(%clientId, $MsgBeige, "Transporting to " @ Zone::getDesc(%zoneId));
			if(%clientId != %id)
				Client::sendMessage(%id, $MsgBeige, "You are being transported to " @ Zone::getDesc(%zoneId));

			//teleport

			%system = Object::getName(%zoneId);
			%type = GetWord(%system, 0);
			%desc = String::getSubStr(%system, String::len(%type)+1, 9999);

			%castPos = TeleportToMarker(%id, "Zones\\" @ %system @ "\\DropPoints", False, True);
			CheckAndBootFromArena(%id);
			NullItemList(%clientId, Lore, $MsgRed, "You lost all %1s you were carrying when you teleported.");

			if(!fetchData(%id, "invisible"))
				GameBase::startFadeIn(%id);

			Player::setDamageFlash(%id, 0.7);
			%extraDelay = 0.22;	//sometimes the endSound doesn't get played unless there is sufficient delay

			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Transportation failed.");
			%returnFlag = False;
		}
	}

	if(%index == $Spell::index[masstransport]) //MassTransport
	{
		//mass transport spell

		%zoneId = GetZoneByKeywords(%clientId, %w2, 3);

		if(%zoneId != False)
		{
			%b = $Spell::radius[%index] * 2;
			%set = newObject("set", SimSet);
			%n = containerBoxFillSet(%set, $SimPlayerObjectType, GameBase::getPosition(%clientId), %b, %b, %b, 0);

			Group::iterateRecursive(%set, DoBoxFunction, %clientId, %index, %zoneId);
			deleteObject(%set);

			%overrideEndSound = True;

			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Mass Transportation failed.");
			%returnFlag = False;
		}
	}
	
	if(%index == $Spell::index[transportfriend]) //TransportFriend
	{
		//Transport friend spell

		%id = clientFromName(%w2);

		if(%id != -1)
		{
			Client::sendMessage(%clientId, $MsgBeige, "Transporting to " @ Zone::getDesc(%zoneId));
			if(%clientId != %id)
				Client::sendMessage(%id, $MsgBeige, "You are being transported to " @ Zone::getDesc(%zoneId));
			//if(IsInCommaList(fetchData(%clientId, "grouplist"), Client::getName(%id))
				PromptTeleportToCaster(%clientId, %id, %castPos);

			%extraDelay = 0.22;

			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Transportation failed.");
			%returnFlag = False;
		}
	}

	if(%index == $Spell::index[masstransportfriend]) //MassTransportFriend
	{
		//mass transport friend(grouplist) spell

		%list = fetchData(%clientId, "grouplist");
		echo(%list);
		if(%list != "" && %list != ",")
		{
				for(%id = Client::getFirst(); %id != -1; %id = Client::getNext(%id))
				{
					if(!%id != %clientId && IsInCommaList(fetchData(%clientId, "grouplist"), Client::getName(%id)))
					{
						if(IsInCommaList(fetchData(%cl, "grouplist"), %clientId))
						{
							PromptTeleportToCaster(%clientId, %id, %castPos);
						}
						else
							Client::sendMessage(%clientId, $MsgRed, Client::getName(%id) @ " does not have you on his/her group-list.");
					}
				}

			%extraDelay = 0.22;

			%returnFlag = True;
		}
		else
		{
			Client::sendMessage(%clientId, $MsgBeige, "Mass Transportation failed, your grouplist is empty.");
			%returnFlag = False;
		}
	}

	if(%index == $Spell::index[remort]) //Remort
	{
		if(!fetchData(%clientId, "currentlyRemorting"))
		{
			%castPos = DoRemort(%clientId);		

			%extraDelay = 0.22;
			%returnFlag = True;
		}
		else
			%returnFlag = False;
	}

	if(%index == $Spell::index[mimic]) //Mimic
	{
		//mimic spell
		if(Zone::getType(fetchData(%clientId, "zone")) == "PROTECTED")
		{
			Client::sendMessage(%clientId, $MsgRed, "You can't cast mimic in protected territory.");
			%overrideEndsound = True;
			%returnFlag = False;
		}
		else
		{
			%id = Player::getClient(%castObj);
			if(getObjectType(%castObj) == "Player")
			{
				%skilltype = $SkillType[$Spell::keyword[%index]];
				%troll = fetchData(%id, "LVL") + floor(getRandom() * ($PlayerSkill[%id, %skilltype] + ($PlayerSkill[%id, $SkillSpellResistance] * (1/2)) ));
				%yroll = fetchData(%clientId, "LVL") + floor(getRandom() * $PlayerSkill[%clientId, %skilltype]);

				if(%yroll > %troll)
				{
// ** this code used to put all your items into storage upon mimic.
//					%max = getNumItems();
//					for(%i = 0; %i < %max; %i++)
//					{
//						%checkItem = getItemData(%i);
//						%checkItemCount = Player::getItemCount(%clientId, %checkItem);
//						if(%checkItemCount)
//						{
//							%b = %checkItem;
//							if(%b.className == "Equipped")
//								%b = String::getSubStr(%b, 0, String::len(%b)-1);
//			
//							storeData(%clientId, "BankStorage", SetStuffString(fetchData(%clientId, "BankStorage"), %b, %checkItemCount));
//							Player::setItemCount(%clientId, %checkItem, 0);
//						}
//					}
					storeData(%clientId, "RACE", fetchData(%id, "RACE"));
					storeData(%clientId, "isMimic", True);
				
					UpdateTeam(%clientId);
					RefreshAll(%clientId);
				
					%castPos = GameBase::getPosition(%clientId);
					%returnFlag = True;
				}
				else
				{
					Client::sendMessage(%clientId, $MsgBeige, "Mimic failed.");
					%overrideEndsound = True;
					%returnFlag = False;
				}
			}
			else
			{
				Client::sendMessage(%clientId, $MsgBeige, "Could not find a target.");
				%overrideEndsound = True;
				%returnFlag = False;
			}
		}
	}

	if(%index == $Spell::index[grip])
	{
		if(%player.moving)
		{
			%player.moveObj.beingMoved = "";

			%player.moving = false;
			%player.moveObj = "";
			%player.moveOff = "";
			%player.moveDst = "";
			%player.moveOriRot = "";
			%player.moveOriPos = "";

			Bottomprint(%clientId,"<jc><f2>Released object.",2);
		}
		else
		{
			if(GameBase::getLOSInfo(%Player,9000))
			{
				%nrm = $LOS::Normal;
				%pos = $LOS::Position;
				%obj = $LOS::Object;

				if(%obj.beingMoved) return;

				%objTyp = getObjectType($LOS::Object);
				%objPos = GameBase::getPosition(%obj);
				%objRot = GameBase::getRotation(%obj);

				%muzzPos = GetMuzzlePos(%player);
				%muzzRot = GetMuzzleRot(%player);

				if(%objTyp == "Item")
				{
					%player.moving = true;
					%player.moveObj = %obj;
					%player.moveOff = Vector::sub(%objPos,%pos);
					%player.moveDst = Vector::getDistance(%muzzPos,%pos);
					%player.moveOriRot = Vector::add(%muzzRot,%objRot);
					%obj.beingMoved = true;

					Grip::moveLoop(%Player);

					Bottomprint(%clientId,"<jc><f2>Grabbing an object.",2);
					%returnFlag = True;
				}
				else if(%objTyp == "Player")
				{
					%player.moving = true;
					%player.moveObj = %obj;
					%player.moveOff = Vector::sub(%objPos,%pos);
					%player.moveDst = Vector::getDistance(%muzzPos,%pos);
					%obj.beingMoved = true;

					Grip::gravityLoop(%player);
					if(!IsDead(%player))
						Schedule("IsDeadLoop("@ %player @");", 2, %player);

					//TrackPath(%player.moveObj);

					Bottomprint(%clientId,"<jc><f2>Gripping a person.",2);
					%returnFlag = True;
				}
				else
					%returnFlag = False;
			}
			else
				%returnFlag = False;
		}
		%overrideEndSound = True;
	}

	if(%index == $Spell::index[push] || %index == $Spell::index[pull])
	{
		%nc = GetPlayerSkill(%clientId, $SkillNeutralCasting);
		%r = $Spell::radius[%index] + (%nc *  0.02);
		%set = newObject("set", SimSet);
		%count = containerBoxFillSet(%set, $SimPlayerObjectType || $ItemObjectType || $VehicleObjectType, GameBase::getPosition(%clientId), %r, %r, %r, 0);

		if(%index == $Spell::index[push]) %nc *= -1;
		//%count = Group::objectCount(%set);
		for(%i = 0; %i < %count; %i++)
		{
			%object = Group::getObject(%set, %i);
			//if(%object != %player)
			//{
				%rot = GameBase::GetRotation(%clientId);
				%vec = Vector::getFromRot(%rot, %nc / 2, %nc / 50 + 5); echo(%vec);
				Player::applyImpulse(%object, %vec);
			//}
		}
		deleteObject(%set);

		%overrideEndSound = True;

		%returnFlag = True;
	}

	if(%index == $Spell::index[jump])
	{
		%vel = Item::getVelocity(%clientId);

		%nc = GetPlayerSkill(%clientId, $SkillNeutralCasting);
		%energy = GetPlayerSkill(%clientId, $SkillEnergy);
		%multiplier = %energy / 100 + 1;

		%zm = 1;
		if(getword(%vel, 2) < 0)
			%zm = -1;

		//Player::applyImpulse(%clientId, "0 0 " @ (%multiplier * %zm) + 20);
		%mvel = %multiplier @ " " @ %multiplier @ " " @ (%multiplier * %zm) + 20;

		echo(%energy @ ", " @ %multiplier @ ", " @ %zm, ", " @ %mvel);

		Player::applyImpulse(%clientId, vector::multiply(%vel, %mvel));

		%clientId.NoFallDamage = True;
		schedule(%clientId @ ".NoFallDamage = False;", 10);

		%overrideEndSound = True;

		%returnFlag = True;
	}

	if(%index == $Spell::index[speed])
	{
		if(getObjectType(%castObj) == "Player" && !Player::isAiControlled(%clientId))
			%id = Player::getClient(%castObj);
		else
			%id = %clientId;

		Client::sendMessage(%clientId, $MsgBeige, "Shielding " @ Client::getName(%id));
		if(%clientId != %id)
			Client::sendMessage(%id, $MsgBeige, Client::getName(%clientId) @ " is casting " @ $Spell::name[%index] @ " on you.");

		UpdateBonusState(%id, $Spell::damageValue[%index], $Spell::ticks[%index]);

		%castPos = GameBase::getPosition(%id);

		%returnFlag = True;
	}

	if(%index == $Spell::index[stealth])
	{
		if(!fetchData(%TrueClientId, "invisible") && !fetchData(%TrueClientId, "blockHide"))
		{
			%closeEnoughToWall = Cap($PlayerSkill[%TrueClientId, $SkillNeutralCasting] / 125, 3.5, 8);
	
			%pos = GameBase::getPosition(%TrueClientId);
	
			%closest = 10000;
			for(%i = 0; %i <= 6.283; %i+= 0.52)
			{
				GameBase::getLOSinfo(Client::getOwnedObject(%TrueClientId), 25, "0 0 " @ %i);
				%dist = Vector::getDistance(%pos, $los::position);
				if(%dist < %closest && $los::position != "0 0 0" && $los::position != "")
					%closest = %dist;
			}
	
			if(%closest <= %closeEnoughToWall)
			{
				Client::sendMessage(%clientId, $MsgBeige, "You successfully conceal yourself with the force.");
				GameBase::startFadeOut(%clientId);
				storeData(%TrueClientId, "invisible", True);
				%grace = Cap($PlayerSkill[%TrueClientId, $SkillNeutralCasting] / 10, 5, 100);
				WalkSlowInvisLoop(%clientId, 5, %grace);

				%castPos = GameBase::getPosition(%clientId);
				%returnFlag = True;
			}
			else
			{
				Client::sendMessage(%clientId, $MsgWhite, "You were unable to conceal yourself in the force");
				%returnFlag = False;
			}
		}
		%returnFlag = False;
	}

	return DoEndSpell(%clientId, %overrideEndSound, %extraDelay, %index, %castPos, %returnFlag);
}





function Grip::moveLoop(%player)
{
	if(%player.moving)
	{
		%obj = %player.moveObj;
		%off = %player.moveOff;
		%dst = %player.moveDst;
		%rot = %player.moveOriRot;

		%muzzPos = GetMuzzlePos(%player);
		%muzzRot = GetMuzzleRot(%player);

		%distVec = Vector::getFromRot(%muzzRot,%dst);
		%offVec = Vector::add(%distVec,%off);

		if(%player.gRotate)
		{
			%mRotX = getWord(%muzzRot,0)*2;
			%mRotY = getWord(%muzzRot,1)*2;
			%mRotZ = getWord(%muzzRot,2);
			%newRot = %mRotX@" "@%mRotY@" "@%mRotZ;
			GameBase::setRotation(%obj,%newRot);
		}

		GameBase::setPosition(%obj,Vector::add(%muzzPos,%offVec));

		Schedule("Grip::moveLoop("@ %Player @");", 0.01, %Player);
	}
}

function Grip::gravityLoop(%player)
{
	if(%player.moving)
	{
		%obj = %player.moveObj;
		%off = %player.moveOff;
		%dst = %player.moveDst;

		%muzzPos = GetMuzzlePos(%player);
		%muzzRot = GetMuzzleRot(%player);

		%distVec = Vector::getFromRot(%muzzRot,%dst);
		%distVec = Vector::add(%muzzPos,%distVec);
		%offVec = Vector::add(%distVec,%off);

		%vel = Item::getVelocity(%obj);
		%pos = GameBase::getPosition(%obj);

		%dist = Vector::getDistance(%pos,%distVec);
		%rot = Vector::getRotAim(%pos,%distVec);
		%vec = Vector::getFromRot(%rot,%dist);
		%mass = 10;
		%mul = Vector::multiply(%vec,%mass@" "@%mass@" "@%mass);

		if(%player.gMove)
		{
			Player::applyImpulse(%obj,%mul);
		}
		else
		{
			Item::setVelocity(%obj,%mul);
		}

		Schedule("Grip::gravityLoop("@ %player @");", 0.1, %player);
	}
}

StaticShapeData TrackerTracer
{
	shapeFile = "tracer";
	maxDamage = 10.0;
   	description = "Tracker Tracer";
	disableCollision = true;
};

function Vector::getVelRotation(%vel)
{
	%rotA = Vector::getRotation(%vel);
	%rot = Vector::add(%rotA,$Pi/-2@" 0 0");
	return %rot;
}

function TrackPath(%player)
{
	if(IsDead(%player)) return;
	%box = GetBoxCenter(%player);
	%vel = Item::getVelocity(%player);

	%obj = NewObject("Tracker",StaticShape,TrackerTracer,true);
	AddToSet("MissionCleanup",%obj);
	GameBase::setPosition(%obj,Vector::add(%box,"0 0 1`"));

	if(%vel != "0 0 0")
	{
		GameBase::setRotation(%obj,Vector::getVelRotation(%vel));
	}
	else
	{
		GameBase::setRotation(%obj,GameBase::getRotation(%Player));
	}

	Schedule("GameBase::startFadeOut("@ %Obj @");", 18, %obj);
	Schedule("deleteObject("@ %Obj @");", 20.5, %obj);

	Schedule("TrackPath("@ %Player @");", 0.2, %player);
}


function GetMuzzleRot(%Player)
{
	%Proj = Projectile::spawnProjectile("Blasterred",GameBase::getMuzzleTransform(%Player),%Player,"0 0 0");
	%Rotation = GameBase::getRotation(%Proj);
	DeleteObject(%Proj);
	return %Rotation;
} 
  
function GetMuzzlePos(%Player)
{
	%Proj = Projectile::spawnProjectile("Blasterred",GameBase::getMuzzleTransform(%Player),%Player,"0 0 0");
	%Position = GameBase::getPosition(%Proj);
	DeleteObject(%Proj);
	return %Position;
}

function Vector::getRotAim(%pos1,%pos2,%neg)
{
	%vec = Vector::normalize(Vector::neg(Vector::sub(%pos1,%pos2)));
	if(%neg)
		%vec = Vector::normalize(Vector::sub(%pos1,%pos2));
	%rot = Vector::add(Vector::getRotation(%vec),"1.570796327 0 0");
	return %rot;
}

function IsDeadLoop(%player)
{
	if(IsDead(%player.moveObj))
	{
		%player.moveObj.beingMoved = "";

		%player.moving = false;
		%player.moveObj = "";
		%player.moveOff = "";
		%player.moveDst = "";
		%player.moveOriRot = "";
		%player.moveOriPos = "";

		Bottomprint(%clientId,"<jc><f2>Released object.",2);
		return;
	}
	Schedule("IsDeadLoop("@ %player @");", 1, %player);
}

