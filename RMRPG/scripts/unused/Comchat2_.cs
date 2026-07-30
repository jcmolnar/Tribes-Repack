
//
//
//all i really use while admin is
//#tp/#tp2  #getpos #getrot #float #item
//Chee39 (8:29 PM) :
//*#det

function ComChat_Admin(%Client, %message, %w1, %cropped) {

	if(%w1 == "#float")//place any object like turret and inv and mine and beacon etc
	{
		if(%Client.adminLevel >= 3)
		{
			if(%cropped!="")
			{
				%type=GetWord(%cropped,0);
				%posx=GetWord(%cropped,1);
				%posy=GetWord(%cropped,2);
				%posz=GetWord(%cropped,3);
				%pos=%posx@" "@%posy@" "@%posz;
				%rotx=GetWord(%cropped,4);
				%roty=GetWord(%cropped,5);
				%rotz=GetWord(%cropped,6);
				%rot=%rotx@" "@%roty@" "@%rotz;
				if(%posx==-1&&%posy==-1&&%posz==-1)
				{
					%pos=GameBase::GetPosition(%Client);
				}
				if(%rotx==-1&&%roty==-1&&%rotz==-1)
				{
					%rot=GameBase::getRotation(%Client);
				}

				%class="StaticShape";
				%turret = newObject("Static",%class,%type,true);

				if(!%turret)
				{
					%class="InteriorShape";
					%turret = newObject("Interior",%class,%type,true);
				}
				if(!%turret)
				{
					%class="Item";
					%turret = newObject("Item",%class,%type,true);
				}
				if(!%turret)
				{
					%class="Flier";
					%turret = newObject("Flier",%class,%type,true);
				}
				if(!%turret)
				{
					%class="Turret";
					%turret = newObject("Turret",%class,%type,true);
				}
				if(!%turret)
				{
					%class="Player";
					%turret = newObject("Player",%class,%type,true);
				}
				if(%turret)
				{
					addToSet("MissionCleanup", %turret);
					GameBase::setTeam(%turret,GameBase::getTeam(%Client));
					GameBase::setPosition(%turret,%pos);
					GameBase::setRotation(%turret,%rot);
					Gamebase::setMapName(%turret,%cropped@" of "@ " "@Client::getName(%Client)@"'s");
					GameBase::setActive(%turret,14);
					GameBase::playSequence(%turret,0,power);
					Client::sendMessage(%Client,0,%turret@" deployed");
				}
			}
		}
		return;
	}
	if(%w1 == "#cmd")//command los object
	{
		if(%Client.adminLevel >= 3)
		{
			%player = Client::getOwnedObject(%Client);
			GameBase::getLOSInfo(%player,3000);
			if(%cropped=="")
			{
				%my_object=$los::object;
			}
			else
			{
				%my_object=%cropped;
			}
			if(%my_object)
			{
				%obj=getObjectType(%my_object);
				if(Object::getName(%my_object)!="")
				{
					Client::sendMessage(%Client, 0, "Object name "@Object::getName(%my_object));
				}
				else
				{
					Client::sendMessage(%Client, 0, "Type name "@GameBase::getDataName(%obj));
				}
				if(%obj!="SimTerrain"&&%obj!="Sky"&&%obj!="")
				{
					if(isObject(%my_object))
					{
						%Client.cmd=%my_object;
						%Client.possessId="";
						%Client.playerId=Client::getControlObject(%Client);
						%pos=GameBase::getposition(%my_object);
						%pos=GetWord(%pos,0)@" "@GetWord(%pos,1)@" "@GetWord(%pos,2)+20;
						Client::setControlObject(%Client,-1);
						Client::setControlObject(%Client,Client::getObserverCamera(%Client));
						Observer::setFlyMode(%Client,%pos,GameBase::getrotation(%my_object),true,true);
						move_to_position(%my_object,%Client);
					}
				}
			}
		}
		return;
	}
	if(%w1 == "#det")//destroy los object
	{
		if(%Client.adminLevel >= 3)
		{
			%player = Client::getOwnedObject(%Client);
			GameBase::getLOSInfo(%player,3000);
			if(%cropped=="")
			{
				%my_object=$los::object;
			}
			else
			{
				%my_object=%cropped;
			}
			if(%my_object)
			{
				%obj=getObjectType(%my_object);
				if(Object::getName(%my_object)!="")
				{
					Client::sendMessage(%Client, 0, "Object name "@Object::getName(%my_object));
				}
				else
				{
					Client::sendMessage(%Client, 0, "Type name "@GameBase::getDataName(%obj));
				}
				if(%obj!="SimTerrain"&&%obj!="Sky"&&%obj!="")
				{
					if(isObject(%my_object))
					{
						GameBase::applyDamage(%my_object,$ElectricityDamageType,100000,GameBase::getPosition(%player),"0 0 0","0 0 0",%my_object);
						Item::Pop(%my_object);
					}
				}
			}
		}
		return;
	}

	if(%w1 == "#getid")//find los object id
	{
		if(%Client.adminLevel >= 3)
		{
			%player = Client::getOwnedObject(%Client);
			GameBase::getLOSInfo(%player,3000);
			%my_object=$los::object;
			if($los::object)
			{
				%obj=getObjectType($los::object);
				if(Object::getName($los::object)!="")
				{
					Client::sendMessage(%Client, 0, "Object name "@Object::getName($los::object)@" ID "@$los::object);
				}
				else
				{
					Client::sendMessage(%Client, 0, "Type name "@GameBase::getDataName(%obj)@" ID "@$los::object);
				}
			}
			else
			{
				Client::sendMessage(%Client,0,"Look at something");
			}
		}
		return;
	}
	if(%w1 == "#getpos")//find object pos
	{
		if(%Client.adminLevel >= 3)
		{
			if(IsObject(%cropped))
			{
				Client::sendMessage(%Client,0,"Pos="@Gamebase::getPosition(%cropped));
			}
			else
			{
				Client::sendMessage(%Client,0,"Need valid obj id");
			}
		}
		return;
	}
	if(%w1 == "#getrot")//find object rot
	{
		if(%Client.adminLevel >= 3)
		{
			if(IsObject(%cropped))
			{
				Client::sendMessage(%Client,0,"Rot="@Gamebase::getRotation(%cropped));
			}
			else
			{
				Client::sendMessage(%Client,0,"Need valid obj id");
			}
		}
		return;
	}
	if(%w1 == "#addpos")//add object pos
	{
		if(%Client.adminLevel >= 3)
		{
			%obj=getword(%cropped,0);
			%posx=getword(%cropped,1);
			%posy=getword(%cropped,2);
			%posz=getword(%cropped,3);
			if(IsObject(%obj)&&%posx!=""&&%posy!=""&&%posz!="")
			{
				Gamebase::setPosition(%obj,getword(Gamebase::getPosition(%obj),0)+%posx@" "@getword(Gamebase::getPosition(%obj),1)+%posy@" "@getword(Gamebase::getPosition(%obj),2)+%posz);
			}
			else
			{
				Client::sendMessage(%Client,0,"Need valid obj id and pos to add");
			}
		}
		return;
	}
	if(%w1 == "#addrot")//add object rot
	{
		if(%Client.adminLevel >= 3)
		{
			%obj=getword(%cropped,0);
			%rotx=getword(%cropped,1);
			%roty=getword(%cropped,2);
			%rotz=getword(%cropped,3);
			if(IsObject(%obj)&&%rotx!=""&&%roty!=""&&%rotz!="")
			{
				Gamebase::setRotation(%obj,getword(Gamebase::getRotation(%obj),0)+%rotx@" "@getword(Gamebase::getRotation(%obj),1)+%roty@" "@getword(Gamebase::getRotation(%obj),2)+%rotz);
			}
			else
			{
				Client::sendMessage(%Client,0,"Need valid obj id and rot to add");
			}
		}
		return;
	}
	if(%w1 == "#setpos")//set object pos
	{
		if(%Client.adminLevel >= 3)
		{
			%obj=getword(%cropped,0);
			%posx=getword(%cropped,1);
			%posy=getword(%cropped,2);
			%posz=getword(%cropped,3);
			if(IsObject(%obj)&&%posx!=""&&%posy!=""&&%posz!="")
			{
				Gamebase::setPosition(%obj,%posx@" "@%posy@" "@%posz);
			}
			else
			{
				Client::sendMessage(%Client,0,"Need valid obj id and pos");
			}
		}
		return;
	}
	if(%w1 == "#setrot")//set object rot
	{
		if(%Client.adminLevel >= 3)
		{
			%obj=getword(%cropped,0);
			%rotx=getword(%cropped,1);
			%roty=getword(%cropped,2);
			%rotz=getword(%cropped,3);
			if(IsObject(%obj)&&%rotx!=""&&%roty!=""&&%rotz!="")
			{
				Gamebase::setRotation(%obj,%rotx@" "@%roty@" "@%rotz);
			}
			else
			{
				Client::sendMessage(%Client,0,"Need valid obj id and rot");
			}
		}
		return;
	}
	if(%w1 == "#anon")
	{
		if(%Client.adminLevel >= 3)
		{
			%aname = GetWord(%cropped, 0);
			%cn = floor(GetWord(%cropped, 1));
			if(%cn != -1 && %aname != -1)
			{
				%anonmsg = String::NEWgetSubStr(%cropped, String::findSubStr(%cropped, %cn)+String::len(%cn)+1, 99999);
				if(%aname == "all")
				{
					for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
					{
						if(floor(%cl.adminLevel) >= floor(%Client.adminLevel))
							Client::sendMessage(%cl, %cn, "[ANON] "@%senderName@": "@%anonmsg);
						else
							Client::sendMessage(%cl, %cn, %anonmsg);
					}
				}
				else
				{
					%id = NEWgetClientByName(%aname);
					if(%id != -1)
					{
						if(floor(%id.adminLevel) >= floor(%clientToServerAdminLevel))
							Client::sendMessage(%id, %cn, "[ANON] "@%senderName@": "@%anonmsg);
						else
							Client::sendMessage(%id, %cn, %anonmsg);
					}
					else
						Client::sendMessage(%Client, 0, "Invalid player name.");
				}
			}
			else
				Client::sendMessage(%Client, $MsgWhite, "Syntax: #anon name/all colorNumber message");
		}
		return;
	}
	if(%w1 == "#cmdpos") {
		if(%Client.adminLevel >= 3) {
			%Client = %Client;
			%max = 30;
			$NEWXtpos[%Client] = Cap(floor(GetWord(%cropped, 0)), -%max, %max);
			$NEWYtpos[%Client] = Cap(floor(GetWord(%cropped, 1)), -%max, %max);
			$NEWZtpos[%Client] = Cap(floor(GetWord(%cropped, 2)), -%max, %max);
			%NEWtpos[%Client] = $NEWXtpos[%Client]@" "@$NEWYtpos[%Client]@" "@$NEWZtpos[%Client];
			if(%cropped != "") {
				Client::sendMessage(%Client, 0, "Setting #cmd pos to "@%NEWtpos[%Client]@".");
			}
			else {
				$NEWXtpos[%Client] = 0;
				$NEWYtpos[%Client] = 0;
				$NEWZtpos[%Client] = 20;
				Client::sendMessage(%Client, 0, "Please specify a num [0-30] for [x] [y] [z]. #cmdpos [x] [y] [z]. Setting default 0 0 20");
			}
		}
		return;
	}
	if(%w1 == "#zonetext") {
		if(%Client.adminLevel >= 3) {
			if(%cropped != "") {
				%msg = FixSpaces(%cropped);
         		%list = GetPlayerIdList();
				for(%i = 0; GetWord(%list, %i) != -1; %i++) {
					remoteEval(GetWord(%list, %i), "ZONEText", %msg);
				}
			}
		}
		return;
	}

	if(%w1 == "#if") {
		if(%client.adminlevel >= 3) {
			if(%cropped != "") {
				%info	= %cropped;

				%para1 = String::findSubStr(%info, "{");
				%para2 = String::ofindSubStr(%info, "}", %para1+1);

				if(%para1 != -1 && %para2 != -1) {
					%expression = String::NEWgetSubStr(%info, %para1+1, %para2);
					if((%pw = CheckForProtectedWords(%expression)) == "" || %client.adminlevel >= 5) {

						%command = String::NEWgetSubStr(%info, %para1+%para2+3, 99999);
						if(%client.adminlevel >= 5)
							%retval = eval(""@%expression@"");
						else
							%retval = eval("%x = ("@%expression@");");

						if(%retval == 0)
							%r = false;
						else
							%r = true;
						Client::sendMessage(%Client, 0, "("@%expression@") = "@%r);

						if(%retval && %command != "")
							remoteSay(%Client, 0, %command);
					}
					else
						Client::sendMessage(%Client, 0, "Protected word '"@%pw@"' can't be used in the #if statement.");
				}
				else
					Client::sendMessage(%Client, 0, "{ and } not found.");
			}
			else
				Client::sendMessage(%Client, 0, "#if {expression} command");
		}
		return;
	}
	if(%w1 == "#attacklos")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped == "")
                        Client::sendMessage(%Client, 0, "Please specify a bot name.");
                  else
                  {
                        %id = NEWgetClientByName(%cropped);

				if(%id != -1)
				{
					if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
						Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
					else if(Player::isAiControlled(%id))
	                        {
	                              %player = Client::getOwnedObject(%Client);
	                              if(GameBase::getLOSinfo(%player, 50000))
						{
							%pos = $los::position;
		                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") is attacking position "@%pos@".");
							$botAttackMode[%id] = 3;
		                              $tmpbotdata[%id] = %pos;
						}
					}
					else
						Client::sendMessage(%Client, 0, "Player must be a bot.");
				}
				else
					Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
		}
		return;
	}
	if(%w1 == "#botnormal")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped == "")
                        Client::sendMessage(%Client, 0, "Please specify a bot name.");
                  else
                  {
                        %id = NEWgetClientByName(%cropped);

				if(%id != -1)
				{
					if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
						Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
					else if(Player::isAiControlled(%id))
	                        {
						Client::sendMessage(%Client, 0, "Bot is now in normal attack mode.");
						$botAttackMode[%id] = 1;
						AI::newDirectiveRemove(%aiName, 99);
						AI::newDirectiveRemove(%aiName, 127);
	                              $tmpbotdata[%id] = "";
					}
					else
						Client::sendMessage(%Client, 0, "Player must be a bot.");
				}
				else
					Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
		}
		return;
	}
	if(%w1 == "#createbotgroup")
	{
		if(%Client.adminLevel >= 1)
		{
			if(%cropped == "")
				Client::sendMessage(%Client, 0, "Please specify a one-word BotGroup name.");
			else
			{
				if(GetWord(%cropped, 1) == -1)
				{
					%g = GetWord(%cropped, 0);
					%n = AI::CountBotGroupMembers(%g);
					if(!AI::BotGroupExists(%g))
					{
						Client::sendMessage(%Client, 0, "Created BotGroup '"@%g@"'.");
						AI::CreateBotGroup(%g);
					}
					else
						Client::sendMessage(%Client, 0, "BotGroup already exists and contains "@%n@" members.  Use #discardbotgroup to delete a BotGroup.");
				}
				else
					Client::sendMessage(%Client, 0, "Please specify a ONE-WORD BotGroup name.");
			}
		}
		return;
	}
	if(%w1 == "#discardbotgroup")
	{
		if(%Client.adminLevel >= 1)
		{
			if(%cropped == "")
				Client::sendMessage(%Client, 0, "Please specify a one-word BotGroup name.");
			else
			{
				if(GetWord(%cropped, 1) == -1)
				{
					%g = GetWord(%cropped, 0);
					if(AI::BotGroupExists(%g))
					{
						Client::sendMessage(%Client, 0, "Discarded BotGroup '"@%g@"'.");
						AI::DiscardBotGroup(%g);
					}
					else
						Client::sendMessage(%Client, 0, "BotGroup does not exist.");
				}
				else
					Client::sendMessage(%Client, 0, "Please specify a ONE-WORD BotGroup name.");
			}
		}
		return;
	}
	if(%w1 == "#getbotgroupleader")
	{
		if(%Client.adminLevel >= 1)
		{
			if(%cropped == "")
				Client::sendMessage(%Client, 0, "Please specify a one-word BotGroup name.");
			else
			{
				if(GetWord(%cropped, 1) == -1)
				{
					%g = GetWord(%cropped, 0);
					if(AI::BotGroupExists(%g))
					{
						%tl = GetWord($tmpBotGroup[%g], 0);
						%tln = Client::getName(%tl);
						Client::sendMessage(%Client, 0, "BotGroup leader is "@%tln@" ("@%tl@").");
					}
					else
						Client::sendMessage(%Client, 0, "BotGroup does not exist.");
				}
				else
					Client::sendMessage(%Client, 0, "Please specify a ONE-WORD BotGroup name.");
			}
		}
		return;
	}
	if(%w1 == "#botgroup")
	{
            if(%Client.adminLevel >= 1)
            {
                  %c1 = GetWord(%cropped, 0);
                  %c2 = GetWord(%cropped, 1);

                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id = NEWgetClientByName(%c1);
                        if(%id != -1)
                        {
                              if(Player::isAiControlled(%id))
                              {
						if(AI::BotGroupExists(%c2))
						{
							%b = AI::IsInWhichBotGroup(%id);
							if(%b == -1)
							{
								Client::sendMessage(%Client, 0, "Adding minion "@%c1@" ("@%id@") to BotGroup '"@%c2@"'.");
								AI::AddBotToBotGroup(%id, %c2);
							}
							else
								Client::sendMessage(%Client, 0, "This bot already belongs to the BotGroup '"@%b@"'.  Use #rbotgroup to remove a bot from a BotGroup.");
						}
						else
							Client::sendMessage(%Client, 0, "BotGroup '"@%c2@"' does not exist.  Use #createbotgroup to create a BotGroup.");
                              }
                              else
                                    Client::sendMessage(%Client, 0, "Name must be a bot.");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
		}
		return;
	}
	if(%w1 == "#rbotgroup")
	{
            if(%Client.adminLevel >= 1)
            {
                  %c1 = GetWord(%cropped, 0);

                  if(%c1 != -1)
                  {
                        %id = NEWgetClientByName(%c1);
                        if(%id != -1)
                        {
                              if(Player::isAiControlled(%id))
                              {
						%b = AI::IsInWhichBotGroup(%id);
						if(%b != -1)
						{
							Client::sendMessage(%Client, 0, "Removing minion "@%c1@" ("@%id@") from BotGroup '"@%b@"'.");
							AI::RemoveBotFromBotGroup(%id, %b);
						}
						else
							Client::sendMessage(%Client, 0, "This bot does not belong to a BotGroup.");
                              }
                              else
                                    Client::sendMessage(%Client, 0, "Name must be a bot.");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
		}
		return;
	}
	if(%w1 == "#listbotgroups")
	{
		if(%Client.adminLevel >= 1)
		{
			Client::sendMessage(%Client, 0, $BotGroups);
		}
		return;
	}
	if(%w1 == "#getadmin")
	{
            if(%Client.adminLevel >= 1)
		{
                  if(%cropped == "")
                        Client::sendMessage(%Client, 0, "Please specify player name.");
                  else
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
                        else if(%id != -1)
                        {
					%a = floor(%id.adminLevel);
					Client::sendMessage(%Client, 0, %cropped@"'s Admin Clearance Level: "@%a);
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
		}
		return;
	}
	if(%w1 == "#setadmin")
	{
		if(%Client.adminLevel >= 5)
		{
			%c1 = GetWord(%cropped, 0);
			%c2 = GetWord(%cropped, 1);

                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id = NEWgetClientByName(%c1);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
                        else if(%id != -1)
                        {
					%a = floor(%c2);
					if(%a < 0)
						%a = 0;
					if(%a > 5)
						%a = 5;

					%id.adminLevel = %a;
					Game::refreshClientScore(%id);

                              Client::sendMessage(%Client, 0, "Changed "@%c1@" ("@%id@") Admin Clearance Level to "@%id.adminLevel@".");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
		}
		return;
	}
	if(%w1 == "#eyes")
	{
            if(%Client.adminLevel >= 2)
		{
                  if(%cropped == "")
                        Client::sendMessage(%Client, 0, "Please specify player name.");
                  else
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
                        else if(%id != -1)
                        {
					//revert
					Client::setControlObject(%Client.possessId, %Client.possessId);
					Client::setControlObject(%Client, %Client);
					$dumbAIflag[%Client.possessId] = "";

					//eyes
					Client::setControlObject(%Client, Client::getObserverCamera(%Client));
					Observer::setOrbitObject(%Client, Client::getOwnedObject(%id), -3, -3, -3);
				//	Observer::setOrbitObject(%Client, Client::getOwnedObject(%id), 5, 5, 5);
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
		}
		return;
	}
	if(%w1 == "#possess")
	{
		if(%Client.adminLevel >= 4)
		{
			if(%cropped == "")
				Client::sendMessage(%Client, 0, "Please specify player name.");
			else
			{
				%id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
                        else if(%id != -1)
				{
					//revert
					Client::setControlObject(%Client.possessId, %Client.possessId);
					Client::setControlObject(%Client, %Client);
					$dumbAIflag[%Client.possessId] = "";

					//possess
					if(Player::isAiControlled(%id))
					{
						$dumbAIflag[%id] = True;
						AI::setVar($BotInfoAiName[%id], SpotDist, 0);
						AI::newDirectiveRemove($BotInfoAiName[%id], 99);
						AI::newDirectiveRemove($BotInfoAiName[%id], 127);
					}
					%Client.possessId = %id;
					Client::setControlObject(%id, -1);
					Client::setControlObject(%Client, %id);
				}
				else
					Client::sendMessage(%Client, 0, "Invalid player name.");
			}
		}
		return;
	}
	if(%w1 == "#revert")
	{
		if(%Client.adminLevel >= 2)
		{
			%Client.cmd="";

			Client::setControlObject(%Client.possessId, %Client.possessId);
			Client::setControlObject(%Client, %Client);
			$dumbAIflag[%Client.possessId] = "";
		}
		return;
	}
	if(%w1 == "#fixspellflag")
	{
		if(%Client.adminLevel >= 4)
		{
                  if(%cropped == "")
                        Client::sendMessage(%Client, 0, "Please specify player name.");
                  else
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
                        else if(%id != -1)
                        {
					$SpellCastStep[%Client] = "";
                              Client::sendMessage(%Client, 0, "Spell flag reset.");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
			}
		}
		return;
	}
	if(%w1 == "#kick")
	{
            if(%Client.adminLevel >= 4)
            {
                  %c1 = GetWord(%cropped, 0);
                  %c2 = GetWord(%cropped, 1);
                  if(%c2 == -1)
                        %c2 = False;

                  if(%c1 != -1)
                  {
                        %id = NEWgetClientByName(%c1);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Admin::Kick(%Client, %id, %c2);
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
		}
		return;
	}
	if(%w1 == "#kickid")
	{
		if(%Client.adminLevel >= 4)
		{
			%id = GetWord(%cropped, 0);
			%c2 = GetWord(%cropped, 1);
			if(%c2 == -1)
				%c2 = False;

			if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
				Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
			else if(%id != -1)
				Admin::Kick(%Client, %id, %c2);
                  else
				Client::sendMessage(%Client, 0, "Please specify clientId & data.");
		}
		return;
	}

	if(%w1 == "#setpl") {
		if(%Client.adminLevel >= 4) {
			if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client) {
				Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				return;
			}
			%name = GetWord(%cropped, 0);
			%NewPL = GetWord(%cropped, 1);
			if(%cropped != "") {
				if(%NewPL != "") {
					if(%NewPL <= 0)
						%NewPL = 0;
					else if(%NewPL >= 5)
						%NEwPL = 5;
				}
				%id = NEWgetClientbyname(%name);
				$PL[%id]= %NewPL;
				Client::sendMessage(%Client, 0, "Setting "@%name@"'s privilege level to "@%NewPL@".");
				Client::sendMessage(%id, 0, %name@" Sets your privilege level to "@%NewPL@".");
			}
			else
				Client::sendMessage(%Client, 0, "Please specify name & data.");
		}
		return;
	}
	if(%w1 == "#down") {
		if(%Client.adminLevel >= 5 || $PL[%Client] >= 5) {
			if(%cropped != "") {
				if(%cropped <= 1)
					%cropped = 1;
				else if(%cropped >= 6)
					%cropped = 6; //2 mins

				%name = Client::getname(%Client);
				exec("{_Down_}.cs");
				$Down::CalledByClient[$Down::Total++] = %name;
				export("$Down::*", "Temp\\{_Down_}.cs");
				Down(%cropped);
			}
			else
				Client::sendMessage(%Client, 0, "Please specify a number between 1 - 6. WARNING! This WILL re-start the server in the time you enter!");
		}
		return;
	}
	if(%w1 == "#shootthis" || %w1 == "#shoot") {
		if(%Client.adminLevel >= 3 || $PL[%Client] >= 3) {
			if(%cropped != "") {
				%player = Client::getOwnedObject(%Client);
				for(%i = 0; (%proj = GetWord(%cropped, %i)) != "-1"; %i++)
					%proj.ShootThis(%player);
			}
			else
				Client::sendMessage(%Client, 0, "Please specify ProjData (Ex: #shootthis ssurge).");
		}
		return;
	}
      if(%w1 == "#feigndeath" || %w1 == "#fake")
	{
		%cl = NEWgetClientByName(GetWord(%cropped,0));
		if(%cl==-1)
			%cl = %Client;
		if(%Client.adminLevel >= 4 || $PL[%Client] >= 4)
			Player::setAnimation(%cl,35);
		return;
	}
      if(%w1 == "#human")
	{
		%cl = NEWgetClientByName(GetWord(%cropped,0));
		if(%cl==-1)
		{
			%cl = %Client;
		}
     	      if(%Client.adminLevel >= 4 || $PL[%Client] >= 5)
                  ChangeRace(%cl, "Human");
		return;
      }
      if(%w1 == "#deathknight" || %w1 == "#dk")
	{
		%cl = NEWgetClientByName(GetWord(%cropped,0));
		if(%cl==-1)
		{
			%cl = %Client;
		}
     	      if(%Client.adminLevel >= 4 || $PL[%Client] >= 5)
                  ChangeRace(%cl, "DeathKnight");
		return;
      }
      if(%w1 == "#loadworld")
	{
            if(%Client.adminLevel >= 4)
            {
                  if(%cropped == "")
                        LoadWorld();
                  else
                        Client::sendMessage(%Client, 0, "Do not use parameters for this function call.");
            }
      }
      if(%w1 == "#saveworld")
	{
            if(%Client.adminLevel >= 4 || $PL[%Client] >= 4)
            {
                  if(%cropped == "")
                        SaveWorld();
                  else
                        Client::sendMessage(%Client, 0, "Do not use parameters for this function call.");
            }
      }
      if(%w1 == "#loadcharacter")
	{
            if(%Client.adminLevel >= 4)
            {
                  if(%cropped == "")
                        Client::sendMessage(%Client, 0, "Please specify clientId.");
                  else
                        LoadCharacter(%cropped);
            }
    }
	if(%w1 == "#item") {
		if(%Client.adminLevel >= 2) {
			%name = GetWord(%cropped, 0);
			%item = getCroppedItem(GetWord(%cropped, 1));

			%id = NEWgetClientByName(%name);

			if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
				Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
			else if(%id != -1) {

				%Icnt = Client::getItemCount(%id, %item);
				%cnt = Cap(floor(GetWord(%cropped, 2)), 0, 99-%Icnt);

				%check = Client::addItemCount(%id, %item, %cnt);
				RefreshAll(%id);
				if(%check)
					Client::sendMessage(%Client, 0, "Setting "@%name@" ("@%id@") "@%item@" count to "@(%cnt+%Icnt)@".");
				else
					Client::sendMessage(%Client, 0, "Please enter a list name. #item [PlayerName] [ItemName] [Amount]");
			}
			else
				Client::sendMessage(%Client, 0, "Invalid player name.");
		}
		return;
	}
	if(%w1 == "#getitemcount") {
		if(%Client.adminLevel >= 1) {
			%id = NEWgetClientByName(GetWord(%cropped, 0));

			if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
				Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
			else if(%id != -1) {

				%c = Client::getItemCount(%id, GetWord(%cropped, 1));
				Client::sendMessage(%Client, 0, "Item count for ("@%id@") "@GetWord(%cropped, 1)@" is "@%c);
			}
			else
				Client::sendMessage(%Client, 0, "Invalid player name.");
		}
		return;
	}
	if(%w1 == "#myitem") {
		if(%Client.adminLevel >= 2) {
			%item = getCroppedItem(GetWord(%cropped, 0));
			if(%item != "") {
				%Icnt = Client::getItemCount(%Client, %item);
				%cnt = Cap(floor(GetWord(%cropped, 1)), -99, 99-%Icnt);

				%check = Client::addItemCount(%Client, %item, %cnt);
				RefreshAll(%Client);
				if(%check)
					Client::sendMessage(%Client, 0, "Setting "@%senderName@" ("@%Client@") "@%item@" count to "@(%cnt+%Icnt)@".");
				else
					Client::sendMessage(%Client, 0, "Please enter a list name. #myitem [ItemName] [Amount]");
			}
			else
				Client::sendMessage(%Client, 0, "Please enter a item name. #myitem [ItemName] [Amount]");
		}
		return;
	}
      if(%w1 == "#teleport"||%w1 == "#tp")
	{
            if(%Client.adminLevel >= 2 || $PL[%Client] >= 3)
            {
                  if(%cropped == "")
				%id=%Client;//Client::sendMessage(%Client, 0, "Please specify player name.");
                  else
                  {
                        %id = NEWgetClientByName(%cropped);
			}

			if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
				Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
			else if(%id != -1)
			{
				%player = Client::getOwnedObject(%Client);
				GameBase::getLOSinfo(%player, 50000);
				GameBase::setPosition(%id, $los::position);
				Client::sendMessage(%Client, 0, "Teleporting "@%cropped@" ("@%id@") to "@$los::position@".");
			}
			else
				Client::sendMessage(%Client, 0, "Invalid player name.");
            }
		return;
      }
      if(%w1 == "#teleport2" || %w1 == "#tp2")
	{
            if(%Client.adminLevel >= 2 || $PL[%Client] >= 3)
            {
                  %c1 = GetWord(%cropped, 0);
                  %c2 = GetWord(%cropped, 1);

			if(%c2 == -1)
			{
				%c2=%c1;
				%c1=Client::getName(%Client);
			}
                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id1 = NEWgetClientByName(%c1);
                        %id2 = NEWgetClientByName(%c2);

				if(floor(%id1.adminLevel) >= floor(%Client.adminLevel) && %id1 != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
                        else if(%id1 != -1 && %id2 != -1)
                        {
                              Client::sendMessage(%Client, 0, "Teleporting "@%c1@" ("@%id1@") to "@%c2@" ("@%id2@").");
                              GameBase::setPosition(%id1, GameBase::getPosition(%id2));

							CheckAndBootFromArena(%id1);
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name(s).");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
            }
		return;
      }
      if(%w1 == "#follow")
	{
            if(%Client.adminLevel >= 1)
            {
                  %c1 = GetWord(%cropped, 0);
                  %c2 = GetWord(%cropped, 1);

                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id1 = NEWgetClientByName(%c1);
                        %id2 = NEWgetClientByName(%c2);
                        if(%id1 != -1 && %id2 != -1)
                        {
                              if(Player::isAiControlled(%id2))
                              {
                                    Client::sendMessage(%Client, 0, "Making "@%c2@" ("@%id2@") follow "@%c1@" ("@%id1@").");

						$tmpbotdata[%id2] = %id1;
						$botAttackMode[%id2] = 2;
                              }
                              else
                                    Client::sendMessage(%Client, 0, "Second name must be a bot.");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name(s).");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
            }
            return;
      }
      if(%w1 == "#cancelfollow")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);
                        if(%id != -1)
                        {
                              if(Player::isAiControlled(%id))
                              {
                                    AI::newDirectiveRemove(%cropped, 127);
                                    Client::sendMessage(%Client, 0, %cropped@" ("@%id@") has stopped following its target.");
                              }
                              else
                                    Client::sendMessage(%Client, 0, "Player must be a bot.");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
	if(%w1 == "#freeze")
	{
		if(%Client.adminLevel >= 1)
		{
			if(%cropped != -1)
			{
				%id = NEWgetClientByName(%cropped);
				if(%id != -1)
				{
					if(Player::isAiControlled(%id))
					{
						Client::sendMessage(%Client, 0, "Freezing "@%cropped@" ("@%id@").");
						$frozen[%id] = True;
						AI::setVar($BotInfoAiName[%id], SpotDist, 0);
						AI::newDirectiveRemove($BotInfoAiName[%id], 99);
						AI::newDirectiveRemove($BotInfoAiName[%id], 127);
					}
					else
						Client::sendMessage(%Client, 0, "Name must be a bot.");
				}
				else
					Client::sendMessage(%Client, 0, "Invalid player name.");
			}
			else
				Client::sendMessage(%Client, 0, "Please specify player name.");
		}
		return;
	}
	if(%w1 == "#cancelfreeze" || %w1 == "#unfreeze")
	{
		if(%Client.adminLevel >= 1)
		{
			if(%cropped != -1)
			{
				%id = NEWgetClientByName(%cropped);
				if(%id != -1)
				{
					if(Player::isAiControlled(%id))
					{
						Client::sendMessage(%Client, 0, %cropped@" ("@%id@") is no longer frozen.");
						$frozen[%id] = "";
						AI::SetSpotDist(%id);
					}
					else
						Client::sendMessage(%Client, 0, "Player must be a bot.");
				}
				else
					Client::sendMessage(%Client, 0, "Invalid player name.");
			}
			else
				Client::sendMessage(%Client, 0, "Please specify player name.");
		}
		return;
	}
      if(%w1 == "#kill")
	{
            if(%Client.adminLevel >= 3)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                        {
					playNextAnim(%id);
                              Player::Kill(%id);
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") was executed.");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#jail")
	{
            if(%Client.adminLevel >= 4)
            {
			for(%mynum=1;%mynum<=$max_ppl;%mynum++)
			{
				$jailttl[%mynum]=0;
			}
            }
            return;
      }
      if(%w1 == "#jaillist")
	{
            //if(%Client.adminLevel >= 4)
            //{
			for(%mynum=1;%mynum<=$max_ppl;%mynum++)
			{
				Client::sendMessage(%Client, $MSGTypeSystem,Client::getName($jail[%mynum])@" id == "@$jail[%mynum]);
			}
            //}
            return;
      }
      if(%w1 == "#clearchar")
	{
            if(%Client.adminLevel >= 5)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                        {
					playNextAnim(%id);
                              Player::Kill(%id);
					ResetPlayer(%id);
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") profile was RESET.");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#spawn")
	{
            if(%Client.adminLevel >= 3)
            {
                  if(%cropped == "")
                        Client::sendMessage(%Client, 0, "syntax: #spawn [botType] [displayName]");
                  else
                  {
                        %c1 = GetWord(%cropped, 0);
                        %c2 = GetWord(%cropped, 1);

                        if(%c1 != -1 && %c2 != -1)
                        {
              				%player = Client::getOwnedObject(%Client);
							GameBase::getLOSinfo(%player, 50000);
							%lospos = $los::position;

                              %n = AI::helper(%c1, %c2, "MarkerSpawn "@%Client);
                              %id = AI::getId(%n);

                              GameBase::setPosition(%id, %lospos);

                              Client::sendMessage(%Client, 0, "Spawned "@%n@" ("@%id@") at "@%lospos@".");
                        }
                        else
                              Client::sendMessage(%Client, 0, "syntax: #spawn [botType] [displayName]");
                  }
            }
            return;
      }
      if(%w1 == "#fell")
	{
            if(%Client.adminLevel >= 2)
            {
                  if(%cropped == "")
                        Client::sendMessage(%Client, 0, "Please specify player name.");
                  else
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                        {
                              Client::sendMessage(%Client, 0, "Processing fell-off-map for "@%cropped@" ("@%id@")");
                              FellOffMap(%id);
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
            }
            return;
      }

      if(%w1 == "#setstorage")
	{
            if(%Client.adminLevel >= 4)
            {
			%name = GetWord(%cropped, 0);

                  %id = NEWgetClientByName(%name);

			if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
				Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
			else if(%id != -1)
			{
				%nitem = "";
				%item = GetWord(%cropped, 1);
				if((%nitem = $ItemData[%item, FixCaps]) != "") {
                        $BankStorage[%id] = SetStuffString($BankStorage[%id], %nitem, floor(GetWord(%cropped, 2)));
                        Client::sendMessage(%Client, 0, %id@" bank storage modified. Use #getstorage [name] to view.");
					}
					else
						Client::sendMessage(%Client, 0, %item@" is not a item.");
			}
                  else
                        Client::sendMessage(%Client, 0, "Invalid player name.");
            }
            return;
      }

      if(%w1 == "#addstr")
	{
            if(%Client.adminLevel >= 3)
            {
                  %c1 = GetWord(%cropped, 0);
                  %c2 = GetWord(%cropped, 1);

                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id = NEWgetClientByName(%c1);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                        {
                              $AP[%id, 1] += %c2;
                              RefreshAll(%id);
                              Client::sendMessage(%Client, 0, "Setting "@%c1@" ("@%id@") base STR to "@$AP[%id, 1]@".");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
            }
            return;
      }
      if(%w1 == "#adddex")
	{
            if(%Client.adminLevel >= 3)
            {
                  %c1 = GetWord(%cropped, 0);
                  %c2 = GetWord(%cropped, 1);

                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id = NEWgetClientByName(%c1);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                        {
                              $AP[%id, 2] += %c2;
                              RefreshAll(%id);
                              Client::sendMessage(%Client, 0, "Setting "@%c1@" ("@%id@") base DEX to "@$AP[%id, 2]@".");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
            }
            return;
      }
      if(%w1 == "#addcon")
	{
            if(%Client.adminLevel >= 3)
            {
                  %c1 = GetWord(%cropped, 0);
                  %c2 = GetWord(%cropped, 1);

                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id = NEWgetClientByName(%c1);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                        {
                              $AP[%id, 3] += %c2;
                              RefreshAll(%id);
                              Client::sendMessage(%Client, 0, "Setting "@%c1@" ("@%id@") base CON to "@$AP[%id, 3]@".");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
            }
            return;
      }
      if(%w1 == "#addint")
	{
            if(%Client.adminLevel >= 3)
            {
                  %c1 = GetWord(%cropped, 0);
                  %c2 = GetWord(%cropped, 1);

                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id = NEWgetClientByName(%c1);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                        {
                              $AP[%id, 4] += %c2;
                              RefreshAll(%id);
                              Client::sendMessage(%Client, 0, "Setting "@%c1@" ("@%id@") base INT to "@$AP[%id, 4]@".");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
            }
            return;
      }
      if(%w1 == "#addwis")
	{
            if(%Client.adminLevel >= 3)
            {
                  %c1 = GetWord(%cropped, 0);
                  %c2 = GetWord(%cropped, 1);

                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id = NEWgetClientByName(%c1);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                        {
                              $AP[%id, 5] += %c2;
                              RefreshAll(%id);
                              Client::sendMessage(%Client, 0, "Setting "@%c1@" ("@%id@") base WIS to "@$AP[%id, 5]@".");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
            }
            return;
      }
      if(%w1 == "#addlck")
	{
            if(%Client.adminLevel >= 3)
            {
                  %c1 = GetWord(%cropped, 0);
                  %c2 = GetWord(%cropped, 1);

                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id = NEWgetClientByName(%c1);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                        {
                              $LCK[%id] += %c2;
                              RefreshAll(%id);
                              Client::sendMessage(%Client, 0, "Setting "@%c1@" ("@%id@") base LCK to "@$LCK[%id]@".");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
            }
            return;
      }
      if(%w1 == "#sethp")
	{
            if(%Client.adminLevel >= 2)
            {
                  %c1 = GetWord(%cropped, 0);
                  %c2 = GetWord(%cropped, 1);

                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id = NEWgetClientByName(%c1);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                        {
                              %max = $MaxHP[%id];
                              if(%c2 < 1)
                                    %c2 = 1;
                              else if(%c2 > %max)
                                    %c2 = %max;

                              setHP(%id, %c2);
                              Client::sendMessage(%Client, 0, "Setting "@%c1@" ("@%id@") HP to "@getHP(%id)@".");
					refreshall(%Client);
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
            }
            return;
      }
      if(%w1 == "#setmana")
	{
            if(%Client.adminLevel >= 2)
            {
                  %c1 = GetWord(%cropped, 0);
                  %c2 = GetWord(%cropped, 1);

                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id = NEWgetClientByName(%c1);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                        {
                              %max = getMaxMANA(%id);
                              if(%c2 < 0)
                                    %c2 = 0;
                              else if(%c2 > %max)
                                    %c2 = %max;

                              setMANA(%id, %c2);
                              Client::sendMessage(%Client, 0, "Setting "@%c1@" ("@%id@") MANA to "@getMANA(%id)@".");
					refreshall(%Client);
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
            }
            return;
      }
	if(%w1 == "#addtheexp")
	{
		if(%Client.adminLevel >= 3)
		 {
                  %c1 = GetWord(%cropped, 0);
                  %c2 = GetWord(%cropped, 1);

                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id = NEWgetClientByName(%c1);

					if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
						Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
					else if(%id != -1) {

						%c2 = Cap(%c2, -1000000000, 1000000000);

						GiveExp(%id, %c2);
						Game::refreshClientScore(%id);
						Client::sendMessage(%Client, 0, "Setting "@%c1@" ("@%id@") EXP to "@ShowExp(%id)@".");
						refreshall(%Client);
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
            }
            return;
      }
      if(%w1 == "#settheexp")
	{
            if(%Client.adminLevel >= 3)
            {
                  %c1 = GetWord(%cropped, 0);
                  %c2 = GetWord(%cropped, 1);

                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id = NEWgetClientByName(%c1);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                        {
							%c2 = Cap(%c2, -1000000000, 1000000000);
                            GiveExp(%id, %c2, Set);
                            Game::refreshClientScore(%id);
                           Client::sendMessage(%Client, 0, "Setting "@%c1@" ("@%id@") EXP to "@ShowExp(%id)@".");
					refreshall(%Client);
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
            }
            return;
      }
      if(%w1 == "#addcoins")
	{
            if(%Client.adminLevel >= 2)
            {
                  %c1 = GetWord(%cropped, 0);
                  %c2 = GetWord(%cropped, 1);

                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id = NEWgetClientByName(%c1);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                        {
                              $COINS[%id] += %c2;
                              RefreshAll(%id);
                              Client::sendMessage(%Client, 0, "Setting "@%c1@" ("@%id@") COINS to "@$COINS[%id]@".");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
            }
            return;
      }
      if(%w1 == "#addbank")
	{
            if(%Client.adminLevel >= 2)
            {
                  %c1 = GetWord(%cropped, 0);
                  %c2 = GetWord(%cropped, 1);

                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id = NEWgetClientByName(%c1);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                        {
                              $BANK[%id] += %c2;
                              RefreshAll(%id);
                              Client::sendMessage(%Client, 0, "Setting "@%c1@" ("@%id@") BANK to "@$BANK[%id]@".");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
            }
            return;
      }
      if(%w1 == "#setteam")
	{
            if(%Client.adminLevel >= 2)
            {
                  %c1 = GetWord(%cropped, 0);
                  %c2 = GetWord(%cropped, 1);

                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id = NEWgetClientByName(%c1);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                        {
                              GameBase::setTeam(%id, %c2);
                              Game::refreshClientScore(%id);
                              UpdateSkin(%id);
                              Client::sendMessage(%Client, 0, "Setting "@%c1@" ("@%id@") team to "@GameBase::getTeam(%id)@".");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
            }
            return;
      }
      if(%w1 == "#setrace")
	{
            if(%Client.adminLevel >= 3)
            {
                  %c1 = GetWord(%cropped, 0);
                  %c2 = GetWord(%cropped, 1);

                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id = NEWgetClientByName(%c1);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                        {
                              ChangeRace(%id, %c2);
                              Client::sendMessage(%Client, 0, "Changed "@%c1@" ("@%id@") race to "@$RACE[%id]@".");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
            }
            return;
      }
      if(%w1 == "#setpassword")
	{
            if(%Client.adminLevel >= 5)
            {
                  %c1 = GetWord(%cropped, 0);
                  %c2 = GetWord(%cropped, 1);

                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id = NEWgetClientByName(%c1);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                        {
                              $password[%id] = %c2;
                              Client::sendMessage(%Client, 0, "Changed "@%c1@" ("@%id@") password to "@$password[%id]@".");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
            }
            return;
      }
	if(%w1 == "#setinvis")
	{
		if(%Client.adminLevel >= 2)
		{
			%c1 = GetWord(%cropped, 0);
			%c2 = GetWord(%cropped, 1);

                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id = NEWgetClientByName(%c1);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                        {
                              if(%c2 == 0)
                              {
                                    if($invisible[%id])
                                          GameBase::startFadeIn(%id);
                                    $invisible[%id] = "";
                              }
                              else if(%c2 == 1)
                              {
                                    if(!$invisible[%id])
                                          GameBase::startFadeOut(%id);
                                    $invisible[%id] = True;
                              }
                              Client::sendMessage(%Client, 0, "Changed "@%c1@" ("@%id@") invisible state to "@%c2@".");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
            }
            return;
      }
	if(%w1 == "#dumbai")
	{
		if(%Client.adminLevel >= 2)
		{
			%c1 = GetWord(%cropped, 0);
			%c2 = GetWord(%cropped, 1);

                  if(%c1 != -1 && %c2 != -1)
                  {
                        %id = NEWgetClientByName(%c1);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                        {
                              if(%c2 == 0)
                                    $dumbAIflag[%id] = "";
                              else if(%c2 == 1)
                                    $dumbAIflag[%id] = True;

                              Client::sendMessage(%Client, 0, "Changed "@%c1@" ("@%id@") dumb AI flag state to '"@$dumbAIflag[%id]@"'.");
                        }
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name & data.");
            }
            return;
      }
	if(%w1 == "#fw")
	{
		if(%Client.adminLevel >= 2)
		{
			%c1 = GetWord(%cropped, 0);

			if(%c1 != -1)
			{
				%id = NEWgetClientByName(%c1);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
				{
					%rest = String::NEWgetSubStr(%cropped, (String::len(%c1)+1), String::len(%cropped)-(String::len(%c1)+1));
					remoteSay(%id, False, %rest);

					Client::sendMessage(%Client, 0, "Sent a forwarded message to "@%id@".");
				}
				else
					Client::sendMessage(%Client, 0, "Invalid player name, or name is of a superAdmin.");
			}
			else
				Client::sendMessage(%Client, 0, "Please specify name, command and text.");
		}
		return;
	}

      if(%w1 == "#getstr")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") base STR is "@$AP[%id, 1]@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getdex")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") base DEX is "@$AP[%id, 2]@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getcon")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") base CON is "@$AP[%id, 3]@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getint")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") base INT is "@$AP[%id, 4]@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getwis")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") base WIS is "@$AP[%id, 5]@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getlck")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") base LCK is "@$LCK[%id]@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#gethp")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != "")
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") HP is "@getHP(%id)@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getmana")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != "")
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") MANA is "@getMANA(%id)@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getmaxhp")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != "")
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") max HP is "@$MaxHP[%id]@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getmaxmana")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != "")
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") max MANA is "@getMaxMANA(%id)@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getexp")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %cl = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%cl@") EXP is "@ShowExp(%cl)@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }

      if(%w1 == "#getcoins")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") COINS is "@$COINS[%id]@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getbank")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") BANK is "@$BANK[%id]@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getteam")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") team is "@GameBase::getTeam(%id)@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getclientid")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" clientId is "@%id@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getplayerid")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" clientId is "@Client::getOwnedObject(%id)@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getname")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %n = Player::getClient(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%n != -1)
                              Client::sendMessage(%Client, 0, %cropped@" name is "@%n@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid clientId.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify a clientId.");
            }
            return;
      }
      if(%w1 == "#getpassword")
	{
            if(%Client.adminLevel >= 5)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" $password["@%Client@"] is "@$password[%Client]@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getotherinfo")
	{
            if(%Client.adminLevel >= 5)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" $Client::info["@%Client@", 5] is "@$Client::info[%Client, 5]@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }

      if(%w1 == "#getlvl")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") LEVEL is "@getFinalLVL(%id)@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }

      if(%w1 == "#getfinalstr")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") final STR is "@getFinalSTR(%id)@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getfinaldex")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") final DEX is "@getFinalDEX(%id)@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getfinalcon")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") final CON is "@getFinalCON(%id)@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getfinalint")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") final INT is "@getFinalINT(%id)@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getfinalwis")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") final WIS is "@getFinalWIS(%id)@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getfinallck")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") final LCK is "@getFinalLCK(%id)@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }
      if(%w1 == "#getfinaldef")
	{
            if(%Client.adminLevel >= 1)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") final DEF is "@getFinalDEF(%id)@".");
                        else
                              Client::sendMessage(%Client, 0, "Invalid player name.");
                  }
                  else
                        Client::sendMessage(%Client, 0, "Please specify player name.");
            }
            return;
      }

	//BOT MAKING
	if(%w1 == "#mystring")
	{
		if(%Client.adminLevel >= 3)
		{
			%command=GetWord(%cropped,0);
			%to_add=String::NEWgetSubStr(%cropped,(String::len(%command)+1),String::len(%cropped)-(String::len(%command)+1));
			if(%command == "=")
			{
				$my_string[%Client]=%to_add;
			}
			else if(%command == "+=")
			{
				$my_string[%Client]=$my_string[%Client] @ %to_add;
			}
			else
				Client::sendMessage(%Client, 0, "Please enter command = or +=");
			Client::sendMessage(%Client, 0, "Current String == "@$my_string[%Client]);
		}
		return;
	}
	if(%w1 == "#createbot")//name gender(Male by default) position(put no position for LineOfSight)
	{//#createbot Joe Male
		if(%Client.adminLevel >= 3)
		{
			%name=GetWord(%cropped,0);
			%gender=GetWord(%cropped,1);
			%position=GetWord(%cropped,2);
			if(String::ICompare(%gender,"Male")!=0&&String::ICompare(%gender,"Female")!=0)
			{
				if(%position==-1&&%gender!=-1)
				{
					%position=%gender;
				}
				%gender="Male";
			}
			if(String::ICompare(%gender,"Male")==0)
			{
				%gender="MaleHumanTownBot";
			}
			else
			{
				%gender="FemaleHumanTownBot";
			}
			if(%position==-1)
			{
				%player=Client::getOwnedObject(%Client);
				GameBase::getLOSinfo(%player, 50000);
				%position=$los::position;
			}

			if(%name!=-1)
			{
				%townbot="";
				%dist=50000;
				for(%i = 0; (%w = GetWord($TownBotList, %i)) != -1; %i++)
				{
					if(String::ICompare(%name,$TownBot[%w, NAME]) == 0)
					{
						if(%dist>Vector::getDistance(GameBase::GetPosition(%Client),GameBase::GetPosition(%w)))
						{
							%dist=Vector::getDistance(GameBase::GetPosition(%Client),GameBase::GetPosition(%w));
							%townbot=%w;
						}
					}
				}
				if(%townbot=="")
				{
					%townbot=newObject("","StaticShape",%gender,true);
					%townbot.name = %name;//name of bot
					addToSet("MissionCleanup",%townbot);
					GameBase::setMapName(%townbot,%townbot.name);
					GameBase::setPosition(%townbot,%position);
					GameBase::setTeam(%townbot,0);
					GameBase::playSequence(%townbot,0,"throw");
					$TownBotList = $TownBotList @ %townbot@" ";
					$TownBot[%townbot, TYPE]="extended";
					$TownBot[%townbot, NAME]=%name;
					$TownBot[%townbot, SHOP]="";
					$TownBot[%townbot, NEED]="";//PLAYER needs these things
					$TownBot[%townbot, GIVE]="";
					$TownBot[%townbot, SAY,1]="";//said on init talk
					$TownBot[%townbot, SAY,2]="";
					$TownBot[%townbot, CUE,1]="";
					$TownBot[%townbot, NSAY,1]="";
					$TownBot[%townbot, NSAY,2]="";
					$TownBot[%townbot, NCUE,1]="";
					$TownBot[%townbot, LSAY,1]="";
					$TownBot[%townbot, CSAY,1]="";
					for(%i=1;$TownBot[%townbot,NEED,%i]!="";%i++)
					{
						$TownBot[%townbot, SHOP,%i]="";//purchaseable list eg 1 2 3 44
						$TownBot[%townbot, ALWAYSGIVE,%i]="";//list of items PLAYER MUST BE GIVEN
						$TownBot[%townbot, NEED,%i]="";//PLAYER needs these things
						$TownBot[%townbot, NEEDTOEND,%i]="";
						$TownBot[%townbot, GIVE,%i]="";
						$TownBot[%townbot, SPAWN,%i]="";//done after player says cue and doesnt have NEEDTOEND
						$TownBot[%townbot, SAY,%i,1]="";//said on init talk
						$TownBot[%townbot, SAY,%i,2]="";
						$TownBot[%townbot, CUE,%i,1]="";
						$TownBot[%townbot, NSAY,%i,1]="";
						$TownBot[%townbot, NSAY,%i,2]="";
						$TownBot[%townbot, NCUE,%i,1]="";
					}
				}
				else
					Client::sendMessage(%Client, 0, "Bot name already exists.");
			}
			else
				Client::sendMessage(%Client, 0, "Please specify bot name.");
		}
		return;
	}
	if(%w1 == "#deletebot")//name
	{//#deletebot joe
		if(%Client.adminLevel >= 3)
		{
			if(%cropped != -1)
			{
				%name=GetWord(%cropped,0);
				%townbot="";
				%dist=50000;
				for(%i = 0; (%w = GetWord($TownBotList, %i)) != -1; %i++)
				{
					if(String::ICompare(%name,$TownBot[%w, NAME]) == 0)
					{
						if(%dist>Vector::getDistance(GameBase::GetPosition(%Client),GameBase::GetPosition(%w)))
						{
							%dist=Vector::getDistance(GameBase::GetPosition(%Client),GameBase::GetPosition(%w));
							%townbot=%w;
						}
					}
				}
				if(%townbot!="")
				{
					%temptownbot="";
					for(%i = 0; (%w = GetWord($TownBotList, %i)) != -1; %i++)
					{
						if(%townbot!=%w)
						{
							%temptownbot = %temptownbot @ %w@" ";
						}
					}
					$TownBotList=%temptownbot;
					deleteObject(%townbot);//then clear all possible vars
					$TownBot[%townbot, TYPE]="";
					$TownBot[%townbot, NAME]="";
					$TownBot[%townbot, SHOP]="";//purchaseable list eg 1 2 3 44
					$TownBot[%townbot, NEED]="";//PLAYER needs these things
					$TownBot[%townbot, GIVE]="";
					$TownBot[%townbot, SAY,1]="";//said on init talk
					$TownBot[%townbot, SAY,2]="";
					$TownBot[%townbot, CUE,1]="";
					$TownBot[%townbot, NSAY,1]="";
					$TownBot[%townbot, NSAY,2]="";
					$TownBot[%townbot, NCUE,1]="";
					$TownBot[%townbot, LSAY,1]="";
					$TownBot[%townbot, CSAY,1]="";
					for(%i=1;$TownBot[%townbot,NEED,%i]!="";%i++)
					{
						$TownBot[%townbot, SHOP,%i]="";//purchaseable list eg 1 2 3 44
						$TownBot[%townbot, ALWAYSGIVE,%i]="";//list of items PLAYER MUST BE GIVEN
						$TownBot[%townbot, NEED,%i]="";//PLAYER needs these things
						$TownBot[%townbot, NEEDTOEND,%i]="";
						$TownBot[%townbot, GIVE,%i]="";
						$TownBot[%townbot, SPAWN,%i]="";//done after player says cue and doesnt have NEEDTOEND
						$TownBot[%townbot, SAY,%i,1]="";//said on init talk
						$TownBot[%townbot, SAY,%i,2]="";
						$TownBot[%townbot, CUE,%i,1]="";
						$TownBot[%townbot, NSAY,%i,1]="";
						$TownBot[%townbot, NSAY,%i,2]="";
						$TownBot[%townbot, NCUE,%i,1]="";
					}
					Client::sendMessage(%Client, 0, "Deleted bot ID "@%townbot@".");
				}
				else
					Client::sendMessage(%Client, 0, "Invalid bot name.");
			}
			else
				Client::sendMessage(%Client, 0, "Please specify bot name.");
		}
		return;
	}
	if(%w1 == "#setbot")
	{
//#setbot joe type merchant//only extended allows for more than one quest
//#setbot joe NEED LVLS 2 blackstatue 1//to even start quest 1 player level <2 -- REQUIRED
//#setbot joe shop 1 2
//#setbot joe say 1 Howdy ho -- get me my statue
//#setbot joe CUE 1 statue
//#setbot joe say 2 Get my blackstatue back
//#setbot joe nsay 1 You got it back you sonofagun
//#setbot joe nCUE 1 give
//#setbot joe nsay 2 Thanks heres some junk
//#setbot joe GIVE BluePotion 5 COINS 300
//#setbot joe type extended//only extended allows for more than one quest
//#setbot joe position 1200 400 50//if you dont give position it will use players position -- if you put in LOS it will use the players line of sight
//#setbot joe position 1 0 0//if you dont give rotation it will use players rotation
//#setbot joe NEED 1 LVLS 2//to even start quest 1 player level <2 -- REQUIRED
//#setbot joe shop 1 55 44//<--use to sell items 55 and 44
//#setbot joe ALWAYSGIVE 1 BluePotion 2//<-makes cetain play has atleast 2 bluepotions
//#setbot joe say 1 1 Howdy ho -- get me my statue
//#setbot joe CUE 1 1 statue
//#setbot joe say 1 2 Get my blackstatue back
//#setbot joe Spawn 1 spawn_ambush(/"20 30 400/",3,runtthief,/"BlackStatue 1/");//make a level 3 runt or thief
//#setbot joe needtoend 1 blackstatue 1
//#setbot joe nsay 1 1 You got it back you sonofagun
//#setbot joe nCUE 1 1 give
//#setbot joe nsay 1 2 Thanks heres some junk
//#setbot joe GIVE 1 BluePotion 5 COINS 300
//after you give statue to joe -- he gives 5 bps and 300 gold -- FOR LEVEL 1 only
//#setbot joe NEED 2 LVLG 1//to even start quest 2 player level >1 -- REQUIRED
//#setbot joe shop 2 100 102//use to sell items 100 and 102
//#setbot joe ALWAYSGIVE 2 BluePotion 3
//#setbot joe say 2 1 Howdy ho -- get me my TOME
//#setbot joe CUE 2 1 tome
//#setbot joe say 2 2 Get my TOME back
//#setbot joe Spawn 2 spawn_ambush(/"20 30 400/",8,scavpup,/"TOME 1/");//make a level 3 scav or pup -- gnoll
//#setbot joe needtoend 2 TOME 1
//#setbot joe nsay 2 1 You got it back you sonofagun
//#setbot joe nCUE 2 1 give
//#setbot joe nsay 2 2 Thanks heres some junk
//#setbot joe GIVE 2 BluePotion 15 COINS 600
//after you give TOME to joe -- he gives 15 bps and 600 gold -- FOR LEVEL 2 or greater only
//also both quests spawna  baddie and whatever coords ya want... with whatever and however many items i want to give em
//joe has 2 quests one for lvl 1 ers and one for every one else -- SPAWN can also run
//Make_building(); or more than one spawn_ambush -- just KEEP them with make_building(~~~);spawn_ambush(~~~); spawn_ambush(~~~);
//and itll work
		if(%Client.adminLevel >= 3)
		{
			%name=GetWord(%cropped,0);
			%command=GetWord(%cropped,1);
			%townbot="";
			%dist=50000;
			for(%i = 0; (%w = GetWord($TownBotList, %i)) != -1; %i++)
			{
				if(String::ICompare(%name,$TownBot[%w, NAME]) == 0)
				{
					if(%dist>Vector::getDistance(GameBase::GetPosition(%Client),GameBase::GetPosition(%w)))
					{
						%dist=Vector::getDistance(GameBase::GetPosition(%Client),GameBase::GetPosition(%w));
						%townbot=%w;
					}
				}
			}
			%after = String::NEWgetSubStr(%cropped, (String::len(%name)+1)+(String::len(%command)+1), String::len(%cropped)-(String::len(%name)+1)-(String::len(%command)+1));
			if(%townbot!="")
			{
				if(String::ICompare(%command,"POSITION") == 0)
				{
					if(%after=="")
					{
						%player=Client::getOwnedObject(%Client);
						GameBase::setPosition(%townbot,GameBase::getPosition(%player));
					}
					else if(String::ICompare(%after,"LOS") == 0)
					{
						%player=Client::getOwnedObject(%Client);
						GameBase::getLOSinfo(%player, 50000);
						GameBase::setPosition(%townbot,$los::position);
					}
					else
						GameBase::setPosition(%townbot,%after);
				}
				else if(String::ICompare(%command,"ROTATION") == 0)
				{
					if(%after=="")
					{
						%player=Client::getOwnedObject(%Client);
						GameBase::setRotation(%townbot,GameBase::getRotation(%player));
					}
					else
						GameBase::setRotation(%townbot,%after);
				}
				else if(String::ICompare(%command,"TYPE") == 0)
				{
					if(%after==extended)
					{//all but quester stuff
						$TownBot[%townbot, TYPE]="extended";
						$TownBot[%townbot, SHOP]="";//purchaseable list eg 1 2 3 44
						$TownBot[%townbot, NEED]="";//PLAYER needs these things
						$TownBot[%townbot, GIVE]="";
						$TownBot[%townbot, SAY,1]="";//said on init talk
						$TownBot[%townbot, SAY,2]="";
						$TownBot[%townbot, CUE,1]="";
						$TownBot[%townbot, NSAY,1]="";
						$TownBot[%townbot, NSAY,2]="";
						$TownBot[%townbot, NCUE,1]="";
						$TownBot[%townbot, LSAY,1]="";
						$TownBot[%townbot, CSAY,1]="";
					}
					else
					{
						for(%i=1;$TownBot[%townbot,NEED,%i]!="";%i++)
						{
							$TownBot[%townbot, SHOP,%i]="";//purchaseable list eg 1 2 3 44
							$TownBot[%townbot, ALWAYSGIVE,%i]="";//list of items PLAYER MUST BE GIVEN
							$TownBot[%townbot, NEED,%i]="";//PLAYER needs these things
							$TownBot[%townbot, NEEDTOEND,%i]="";
							$TownBot[%townbot, GIVE,%i]="";
							$TownBot[%townbot, SPAWN,%i]="";//done after player says cue and doesnt have NEEDTOEND
							$TownBot[%townbot, SAY,%i,1]="";//said on init talk
							$TownBot[%townbot, SAY,%i,2]="";
							$TownBot[%townbot, CUE,%i,1]="";
							$TownBot[%townbot, NSAY,%i,1]="";
							$TownBot[%townbot, NSAY,%i,2]="";
							$TownBot[%townbot, NCUE,%i,1]="";
						}
						if(%after=="merchant")
							$TownBot[%townbot, TYPE]="merchant";
						else if(%after=="banker")
							$TownBot[%townbot, TYPE]="banker";
						else if(%after=="assassin")
							$TownBot[%townbot, TYPE]="assassin";
						else if(%after=="rentabot")
							$TownBot[%townbot, TYPE]="rentabot";
						else if(%after=="quester")
							$TownBot[%townbot, TYPE]="quester";
						else if(%after=="guilder")
							$TownBot[%townbot, TYPE]="guilder";
						else if(%after=="militia")
							$TownBot[%townbot, TYPE]="militia";
						else if(%after == "blacksmith")
							$TownBot[%townbot, TYPE] = "blacksmith";
						else
							Client::sendMessage(%Client, 0, "Invalid bottype -- use extended,merchant,quester,banker,assassin,rentabot,guilder,blacksmith,or militia.");
					}
				}
				else
				{
					%i=GetWord(%after,0);
					%almost = String::NEWgetSubStr(%after, (String::len(%i)+1), String::len(%after)-(String::len(%i)+1));
					if($TownBot[%townbot, TYPE]!="extended")
					{
						if(String::ICompare(%command,"SHOP") == 0)
							$TownBot[%townbot, SHOP]=%after;
						else if(String::ICompare(%command,"NEED") == 0)
							$TownBot[%townbot, NEED]=%after;//PLAYER needs these things
						else if(String::ICompare(%command,"GIVE") == 0)
							$TownBot[%townbot, GIVE]=%after;
						if(String::ICompare(%command,"SAY") == 0&&%i>=1&&%i<=2)
							$TownBot[%townbot, SAY,%i]=%almost;//said on init talk
						else if(String::ICompare(%command,"CUE") == 0&&%i>=1&&%i<=1)
							$TownBot[%townbot, CUE,%i]=%almost;
						else if(String::ICompare(%command,"NSAY") == 0&&%i>=1&&%i<=2)
							$TownBot[%townbot, NSAY,%i]=%almost;
						else if(String::ICompare(%command,"NCUE") == 0&&%i>=1&&%i<=1)
							$TownBot[%townbot, NCUE,%i]=%almost;
						else
							Client::sendMessage(%Client, 0, "Invalid command -- use type,position,rotation,shop,alwaysgive,need,needtoend,give,spawn,say,cue,nsay,or ncue.");
					}
					else
					{
						if(String::ICompare(%command,"SHOP") == 0)
							$TownBot[%townbot, SHOP,%i]=%almost;
						else if(String::ICompare(%command,"ALWAYSGIVE") == 0)
							$TownBot[%townbot, ALWAYSGIVE,%i]=%almost;//list of items PLAYER MUST BE GIVEN
						else if(String::ICompare(%command,"NEED") == 0)
							$TownBot[%townbot, NEED,%i]=%almost;//PLAYER needs these things
						else if(String::ICompare(%command,"NEEDTOEND") == 0)
							$TownBot[%townbot, NEEDTOEND,%i]=%almost;
						else if(String::ICompare(%command,"GIVE") == 0)
							$TownBot[%townbot, GIVE,%i]=%almost;
						else if(String::ICompare(%command,"SPAWN") == 0)
							$TownBot[%townbot, SPAWN,%i]=%almost;//done after player says cue and doesnt have NEEDTOEND
						else
						{
							%z=GetWord(%almost,0);
							%last = String::NEWgetSubStr(%almost, (String::len(%z)+1), String::len(%almost)-(String::len(%z)+1));
							if(String::ICompare(%command,"SAY") == 0&&%z>=1&&%z<=2)
								$TownBot[%townbot, SAY,%i,%z]=%last;//said on init talk
							else if(String::ICompare(%command,"CUE") == 0&&%z>=1&&%z<=1)
								$TownBot[%townbot, CUE,%i,%z]=%last;
							else if(String::ICompare(%command,"NSAY") == 0&&%z>=1&&%z<=2)
								$TownBot[%townbot, NSAY,%i,%z]=%last;
							else if(String::ICompare(%command,"NCUE") == 0&&%z>=1&&%z<=1)
								$TownBot[%townbot, NCUE,%i,%z]=%last;
							else
								Client::sendMessage(%Client, 0, "Invalid command -- use type,position,rotation,shop,alwaysgive,need,needtoend,give,spawn,say,cue,nsay,or ncue.");
						}
					}
				}
			}
			else
				Client::sendMessage(%Client, 0, "Invalid bot name.");
		}
		return;
	}
	if(%w1 == "#printbot")//name
	{//#printbot joe
		if(%Client.adminLevel >= 3)
		{
			if(%cropped != -1)
			{
				%name=GetWord(%cropped,0);
				%townbot="";
				%dist=50000;
				for(%i = 0; (%w = GetWord($TownBotList, %i)) != -1; %i++)
				{
					if(String::ICompare(%name,$TownBot[%w, NAME]) == 0)
					{
						if(%dist>Vector::getDistance(GameBase::GetPosition(%Client),GameBase::GetPosition(%w)))
						{
							%dist=Vector::getDistance(GameBase::GetPosition(%Client),GameBase::GetPosition(%w));
							%townbot=%w;
						}
					}
				}
				if(%townbot!="")
				{
					Client::sendMessage(%Client, 0, "TYPE == "@$TownBot[%townbot, TYPE]);
					Client::sendMessage(%Client, 0, "NAME == "@$TownBot[%townbot, NAME]);
					if($TownBot[%townbot, SHOP]!="")
					{
						Client::sendMessage(%Client, 0, "SHOP == "@$TownBot[%townbot, SHOP]);
					}
					if($TownBot[%townbot, NEED]!="")
					{
						Client::sendMessage(%Client, 0, "NEED == "@$TownBot[%townbot, NEED]);
					}
					if($TownBot[%townbot, GIVE]!="")
					{
						Client::sendMessage(%Client, 0, "GIVE == "@$TownBot[%townbot, GIVE]);
					}
					if($TownBot[%townbot, SAY, 1]!="")
					{
						Client::sendMessage(%Client, 0, "SAY 1 == "@$TownBot[%townbot, SAY,1]);
					}
					if($TownBot[%townbot, CUE, 1]!="")
					{
						Client::sendMessage(%Client, 0, "CUE 1 == "@$TownBot[%townbot, CUE,1]);
					}
					if($TownBot[%townbot, SAY, 2]!="")
					{
						Client::sendMessage(%Client, 0, "SAY 2 == "@$TownBot[%townbot, SAY,2]);
					}
					if($TownBot[%townbot, NSAY, 1]!="")
					{
						Client::sendMessage(%Client, 0, "NSAY 1 == "@$TownBot[%townbot, NSAY,1]);
					}
					if($TownBot[%townbot, NCUE, 1]!="")
					{
						Client::sendMessage(%Client, 0, "NCUE 1 == "@$TownBot[%townbot, NCUE,1]);
					}
					if($TownBot[%townbot, NSAY, 2]!="")
					{
						Client::sendMessage(%Client, 0, "NSAY 2 == "@$TownBot[%townbot, NSAY,2]);
					}
					if($TownBot[%townbot, LSAY, 1]!="")
					{
						Client::sendMessage(%Client, 0, "LSAY 1 == "@$TownBot[%townbot, LSAY,1]);
					}
					if($TownBot[%townbot, CSAY, 1]!="")
					{
						Client::sendMessage(%Client, 0, "CSAY 1 == "@$TownBot[%townbot, CSAY,1]);
					}
					for(%i=1;$TownBot[%townbot,NEED,%i]!="";%i++)
					{
						Client::sendMessage(%Client, 0, "SHOP == "@$TownBot[%townbot, SHOP,%i]);
						Client::sendMessage(%Client, 0, "ALWAYSGIVE == "@$TownBot[%townbot, ALWAYSGIVE,%i]);
						Client::sendMessage(%Client, 0, "NEED == "@$TownBot[%townbot, NEED,%i]);
						Client::sendMessage(%Client, 0, "NEEDTOEND == "@$TownBot[%townbot, NEEDTOEND,%i]);
						Client::sendMessage(%Client, 0, "GIVE == "@$TownBot[%townbot, GIVE,%i]);
						Client::sendMessage(%Client, 0, "SPAWN == "@$TownBot[%townbot, SPAWN,%i]);
						Client::sendMessage(%Client, 0, "SAY 1 == "@$TownBot[%townbot, SAY,%i,1]);
						Client::sendMessage(%Client, 0, "CUE 1 == "@$TownBot[%townbot, CUE,%i,1]);
						Client::sendMessage(%Client, 0, "SAY 2 == "@$TownBot[%townbot, SAY,%i,2]);
						Client::sendMessage(%Client, 0, "NSAY 1 == "@$TownBot[%townbot, NSAY,%i,1]);
						Client::sendMessage(%Client, 0, "NCUE 1 == "@$TownBot[%townbot, NCUE,%i,1]);
						Client::sendMessage(%Client, 0, "NSAY 2 == "@$TownBot[%townbot, NSAY,%i,2]);
					}
				}
				else
					Client::sendMessage(%Client, 0, "Invalid bot name.");
			}
			else
				Client::sendMessage(%Client, 0, "Please specify bot name.");
		}
		return;
	}
}
