
function ComChat_Admin(%Client, %message, %w1, %cropped) {
	      if(%w1 == "#spawndis")
		{
	            if(%Client.adminLevel >= 3)
	            {
	                  if(getword(%message,1) != "")
	                  {
					%f = GetWord(%message, 1);
					%tag = GetWord(%message, 2);
					%x = GetWord(%message, 3);
					%y = GetWord(%message, 4);
					%z = GetWord(%message, 5);
					%r1 = GetWord(%message, 6);
					%r2 = GetWord(%message, 7);
					%r3 = GetWord(%message, 8);

					if(%x == -1 && %y == -1 && %z == -1)
					{
						GameBase::getLOSinfo(Client::getOwnedObject(%client), 50000);
						%pos = $los::position;
					}
					else
						%pos = %x @ " " @ %y @ " " @ %z;

					if(%r1 == -1 && %r2 == -1 && %r3 == -1)
						%rot = -1;
					else
						%rot = %r1 @ " " @ %r2 @ " " @ %r3;

					%fname = %f @ ".dis";
					%object = newObject(%tag, InteriorShape, %fname);

					if(%object != 0 && %tag != -1)
					{
						if(IsInCommaList($DISlist, %tag))
						{
							%o = $tagToObjectId[%tag];
							deleteObject(%o);
							$tagToObjectId[%tag] = "";
							%w = "Replaced";
						}
						else
						{
							$DISlist = AddToCommaList($DISlist, %tag);
							%w = "Spawned";
						}

						addToSet("MissionCleanup", %object);
						$tagToObjectId[%tag] = %object;
						%object.tag = %tag;

						GameBase::setPosition(%object, %pos);
						if(%rot != -1)
							GameBase::setRotation(%object, %rot);

						if(!%echoOff) Client::sendMessage(%client, 0, %w @ " " @ %tag @ " (" @ %object @ ") at pos " @ %pos);
					}
					else
						Client::sendMessage(%client, 0, "Invalid DIS filename or tagname.");
				}
	                  else
					Client::sendMessage(%client, 0, "#spawndis filename tagname [x] [y] [z] [r1] [r2] [r3]. Do not specify .dis, this will automatically be added.");
	            }
			return;
	      }
	      if(%w1 == "#deldis")
		{
	            if(%Client.adminLevel >= 3)
	            {
				%tag = GetWord(%message, 1);

	                  if(getword(%message,1) != -1)
	                  {
					if($tagToObjectId[%tag] != "")
					{
						%object = $tagToObjectId[%tag];
						ClearEvents(%object);
						deleteObject(%object);
						$tagToObjectId[%tag] = "";
						$DISlist = RemoveFromCommaList($DISlist, %tag);

						if(!%echoOff) Client::sendMessage(%client, 0, "Deleted " @ %tag @ " (" @ %object @ ")");
					}
					else
						if(!%echoOff) Client::sendMessage(%client, 0, "Invalid tagname.");
				}
	                  else
					Client::sendMessage(%client, 0, "#deldis tagname.");
	            }
			return;
	      }
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
				%my_object=$los::object;
			else
				%my_object=%cropped;
			if(%my_object)
			{
				%obj=getObjectType(%my_object);
				if(Object::getName(%my_object)!="")
					Client::sendMessage(%Client, 0, "Object name "@Object::getName(%my_object));
				else
					Client::sendMessage(%Client, 0, "Type name "@GameBase::getDataName(%obj));

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
					$SpellCastStep[%id] = "";
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
			%name = GetWord(%cropped, 0);
			%id = NEWgetClientByName(%name);
			if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client) {
				Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				return;
			}
			%NewPL = GetWord(%cropped, 1);
			if(%cropped != "") {
				if(%NewPL != "") {
					if(%NewPL <= 0)
						%NewPL = 0;
					else if(%NewPL >= 5)
						%NewPL = 5;
				}
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
	if(%w1 == "#spawnchocobo") {
		if(%Client.adminLevel >= 3) {
			if(%cropped != "") {
				%pos = GameBase::getPosition(%Client);
				Chocobo::Spawn(%Client, %cropped, %pos);
			}
			else
				Client::sendMessage(%Client, 0, "Please enter a Chocobo # (Ex: #makeChocobo 1)");
		}
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
					refreshall(%id);
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
					refreshall(%id);
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
						refreshall(%id);
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
					refreshall(%id);
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
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" ("@%id@") EXP is "@ShowExp(%id)@".");
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
      if(%w1 == "#getpassword")
	{
            if(%Client.adminLevel >= 6)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" $password["@%id@"] is "@$password[%id]@".");
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
            if(%Client.adminLevel >= 6)
            {
                  if(%cropped != -1)
                  {
                        %id = NEWgetClientByName(%cropped);

				if(floor(%id.adminLevel) >= floor(%Client.adminLevel) && %id != %Client)
					Client::sendMessage(%Client, 0, "Could not process command: Target admin clearance level too high.");
				else if(%id != -1)
                              Client::sendMessage(%Client, 0, %cropped@" $Client::info["@%id@", 5] is "@$Client::info[%id, 5]@".");
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
				%ngender="MaleHumanTownBot";
			else
				%ngender="FemaleHumanTownBot";

			if(%position==-1)
			{
				%player=Client::getOwnedObject(%Client);
				GameBase::getLOSinfo(%player, 50000);
				%position=$los::position;
			}

			if(%name!=-1)
			{
				MakeTownBot(%ngender, %gender@"001", %name, %position);
				$TownBot[$townbot, TYPE]="extended";
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
					%temptownbot=" ";
					for(%i = 0; (%w = GetWord($TownBotList, %i)) != -1; %i++)
					{
						if(%townbot!=%w)
						{
							%temptownbot = %temptownbot @ %w@" ";
						}
					}

					$TownBotList=%temptownbot;
					deleteObject(%townbot);//then clear all possible vars

					deleteVariables("TownBot"@%townbot@"*");

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
}
