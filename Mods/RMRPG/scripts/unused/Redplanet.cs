//ADDED time limit between spawns via Switches
//ADDED PASS FOR WEIGHT DIVISION
//%max = getNumItems();
//for (%i = 0; %i < %max; %i++)
//{
//	%item = getItemData(%i);
//	if(%item.className == "Weapon"||$AccessoryVar[%item, $AccessoryType] == $BodyAccessoryType)
//	{
//		$AccessoryVar[getCroppedItem(%item),$Weight]/=$full_item;
//	}
//}


$temp_x="";
$temp_y="";
$temp_z="";
$tempid="";

$bots=0;
$botmax=20;
$botid[1]=0;
$botid[2]=0;
$botid[3]=0;
$botid[4]=0;
$botid[5]=0;
$botid[6]=0;
$botid[7]=0;
$botid[8]=0;
$botid[9]=0;
$botid[10]=0;
$botid[11]=0;
$botid[12]=0;
$botid[13]=0;
$botid[14]=0;
$botid[15]=0;
$botid[16]=0;
$botid[17]=0;
$botid[18]=0;
$botid[19]=0;
$botid[20]=0;

$ttl[1]=0;
$ttl[2]=0;
$ttl[3]=0;
$ttl[4]=0;
$ttl[5]=0;
$ttl[6]=0;
$ttl[7]=0;
$ttl[8]=0;
$ttl[9]=0;
$ttl[10]=0;
$ttl[11]=0;
$ttl[12]=0;
$ttl[13]=0;
$ttl[14]=0;
$ttl[15]=0;
$ttl[16]=0;
$ttl[17]=0;
$ttl[18]=0;
$ttl[19]=0;
$ttl[20]=0;

$ghmax=3;
$ghid[1]=0;
$ghid[2]=0;
$ghid[3]=0;
$ghttl[1]=0;
$ghttl[2]=0;
$ghttl[3]=0;
$gh_going=false;
$ghnum=0;
$ghbotmax=5;
$ghbot[1]=0;
$ghbot[2]=0;
$ghbot[3]=0;
$ghbot[4]=0;
$ghbot[5]=0;
$ghbotid[1]=0;
$ghbotid[2]=0;
$ghbotid[3]=0;
$ghbotid[4]=0;
$ghbotid[5]=0;

$rentmax=1;
$botrented=0;
//$rentedbot[1]=0;
$rentedbotid[1]=0;
$rentedbotowner[1]=0;

function init_ttl()
{
//focusserver();
	schedule("ttl_it();", 1);
//	schedule("move_it();", 5);
//	schedule("golom_house();", 10);
//focusclient();
}

function rent_it()
{
	$botrented=0;
	for(%mynum=1;%mynum<=$rentmax;%mynum++)
	{
		if($rentedbotid[%mynum]!=0&&Player::isAiControlled($rentedbotid[%mynum])&&!isdead($rentedbotid[%mynum]))
		{
			$botrented=1;
			%dist=Vector::getDistance(GameBase::getPosition($rentedbotid[%mynum]),GameBase::getPosition($rentedbotowner[%mynum]));
			if(%dist<0)
				%dist*=-1;
			if(%dist>15)
			{
				$temp_x=GetWord(GameBase::getPosition($rentedbotowner[%mynum]), 0);
				$temp_y=GetWord(GameBase::getPosition($rentedbotowner[%mynum]), 1);
				$temp_z=GetWord(GameBase::getPosition($rentedbotowner[%mynum]), 2)+2;

				set_position($rentedbotid[%mynum],$temp_x @ " " @ $temp_y @ " " @ $temp_z);
			}
		}
		else
		{
			//$rentedbot[%mynum]=0;
			$rentedbotid[%mynum]=0;
			$rentedbotowner[%mynum]=0;
		}
	}
	if($botrented==1)
	{
//focusserver();
		schedule("rent_it();", 5);
//focusclient();
	}
}

function golom_house()
{
	$ghnum=0;
	for(%mynum=1;%mynum<=$ghmax;%mynum++)
	{
		if($ghid[%mynum]!=0)
		{
			$ghttl[%mynum]=0;
			$ghnum++;
		}
	}
	%alldead=true;
	for(%mynum=1;%mynum<=$ghbotmax;%mynum++)
	{
		if($ghbotid[%mynum]!=0&&Player::isAiControlled($ghbotid[%mynum])&&!isdead($ghbotid[%mynum]))
		{
			%alldead=false;
			$gh_going=true;
			break;
		}
	}
	if($gh_going!=true)//$ghnum==3&&
	{
		%alldead=false;
		$gh_going=true;
		for(%mynum=1;%mynum<=$ghbotmax;%mynum++)
		{
			if($ghbotid[%mynum]!=0&&Player::isAiControlled($ghbotid[%mynum]))
			{
				for(%fnum=1;%fnum<=$botmax;%fnum++)
				{
					if($botid[%fnum]==$ghbotid[%mynum])
					{
						$botid[%fnum]=0;
					}
				}
				felloffmap($ghbotid[%mynum]);
			}
			$ghbotid[%mynum]=0;
			$ghbot[%mynum]=0;
		}
		for(%mynum=1;%mynum<=$ghmax;%mynum++)
		{
			if($ghid[%mynum]!=0)
			{
				bottomprint($ghid[%mynum], "<f1><jc>Now Entering Golom House", 1);
			}
		}
		//GameBase::setPosition($ghid[1], "-1862.5 -856 -98");
		//GameBase::setPosition($ghid[2], "-1860 -856 -98");
		//GameBase::setPosition($ghid[3], "-1863 -848 -98");
		//for(%mynum=1;%mynum<=$ghmax;%mynum++)
		//{
		//	$ghid[%mynum]=0;
		//}
		%mynum=1;
		//if($bots<=$botmax)//&&$ghbot[%mynum]==0)
		//{
			$ghbot[%mynum]=1;
			spawn_ambush("346 -977 35.5",0,"minigolom");
		//}
		%mynum++;
		//if($bots<=$botmax)//&&$ghbot[%mynum]==0)
		//{
			$ghbot[%mynum]=1;
			spawn_ambush("358 -977 35.5",0,"minigolom");
		//}
		%mynum++;
		//if($bots<=$botmax)//&&$ghbot[%mynum]==0)
		//{
			$ghbot[%mynum]=1;
			spawn_ambush("350 -987 27.5",0,"minigolom");
		//}
		%mynum++;
		//if($bots<=$botmax)//&&$ghbot[%mynum]==0)
		//{
			$ghbot[%mynum]=1;
			spawn_ambush("354 -987 27.5",0,"minigolom");
		//}
		%mynum++;
		//if($bots<=$botmax)//&&$ghbot[%mynum]==0)
		//{
			$ghbot[%mynum]=1;
			spawn_ambush("352 -976 23.5",0,"golom");
		//}
		%avg=0;
		for(%fnum=1;%fnum<=$ghmax;%fnum++)
		{
			if($ghid[%fnum]!=0)
			{
				%avg+=getFinalLVL($ghid[%fnum]);
			}
		}
		%avg=(%avg/$ghmax)+2;
		for(%mynum=1;%mynum<=$ghbotmax-1;%mynum++)
		{
			if($ghbotid[%mynum]!=0&&Player::isAiControlled($ghbotid[%mynum]))
			{
				if(%avg>=8)
				{
					Player::setItemCount($ghbotid[%mynum], cutlass, 1);
					Player::setItemCount($ghbotid[%mynum], heavycrossbow, 1);
					Player::setItemCount($ghbotid[%mynum], HeavyQuarrel , 20);
					Player::setItemCount($ghbotid[%mynum], studdedleather, 1);
					$COINS[$ghbotid[%mynum]] = 900;
				}
				else if(%avg>=4)
				{
					Player::setItemCount($ghbotid[%mynum], broadsword, 1);
					Player::setItemCount($ghbotid[%mynum], shortbow, 1);
					Player::setItemCount($ghbotid[%mynum], flightarrow, 20);
					Player::setItemCount($ghbotid[%mynum], studdedleather, 1);
					$COINS[$ghbotid[%mynum]] = 500;
				}
				else if(%avg>=2)
				{
					Player::setItemCount($ghbotid[%mynum], shortsword, 1);
					Player::setItemCount($ghbotid[%mynum], sling, 1);
					Player::setItemCount($ghbotid[%mynum], smallrock, 20);
					//Player::setItemCount($ghbotid[%mynum], studdedleather, 1);
					$COINS[$ghbotid[%mynum]] = 200;
				}
				$LCK[$ghbotid[%mynum]]+= %avg;
				if((%avg/4)>1)
				{
					$EXP[$ghbotid[%mynum]]*= (%avg/4);
					$MaxHP[$ghbotid[mynum]]*= (%avg/4);
					setHP($ghbotid[mynum], $MaxHP[$ghbotid[%mynum]]);
				}
			}
		}
		if($ghbotid[$ghbotmax]!=0&&Player::isAiControlled($ghbotid[$ghbotmax]))
		{
			if(%avg>=8)
			{
				Player::setItemCount($ghbotid[$ghbotmax], rapier, 1);
				Player::setItemCount($ghbotid[$ghbotmax], heavycrossbow, 1);
				Player::setItemCount($ghbotid[$ghbotmax], HeavyQuarrel, 20);
				Player::setItemCount($ghbotid[$ghbotmax], splintmail, 1);
				$COINS[$ghbotid[$ghbotmax]] = 100000;
			}
			else if(%avg>=4)
			{
				Player::setItemCount($ghbotid[$ghbotmax], claymore, 1);
				Player::setItemCount($ghbotid[$ghbotmax], heavycrossbow, 1);
				Player::setItemCount($ghbotid[$ghbotmax], HeavyQuarrel, 20);
				Player::setItemCount($ghbotid[$ghbotmax], splintmail, 1);
				$COINS[$ghbotid[$ghbotmax]] = 60000;
			}
			else if(%avg>=2)
			{
				Player::setItemCount($ghbotid[$ghbotmax], BroadSword, 1);
				Player::setItemCount($ghbotid[$ghbotmax], shortbow, 1);
				Player::setItemCount($ghbotid[$ghbotmax], flightarrow, 20);
				Player::setItemCount($ghbotid[$ghbotmax], studdedleather, 1);
				$COINS[$ghbotid[$ghbotmax]] = 30000;
			}
			$LCK[$ghbotid[$ghbotmax]]+= %avg*4;
			if((%avg/2)>1)
			{
				$EXP[$ghbotid[$ghbotmax]]*= (%avg/2);
				$MaxHP[$ghbotid[$ghbotmax]]*= (%avg/2);
				setHP($ghbotid[$ghbotmax], $MaxHP[$ghbotid[$ghbotmax]]);
			}
		}
	}
	if(%alldead==true)
	{
		for(%mynum=1;%mynum<=$ghmax;%mynum++)
		{
			$ghid[%mynum]=0;
		}
		$gh_going=false;
		for(%mynum=1;%mynum<=$ghbotmax;%mynum++)
		{
			if($ghbotid[%mynum]!=0&&Player::isAiControlled($ghbotid[%mynum]))
			{
				for(%fnum=1;%fnum<=$botmax;%fnum++)
				{
					if($botid[%fnum]==$ghbotid[%mynum])
					{
						$botid[%fnum]=0;
					}
				}
				felloffmap($ghbotid[%mynum]);
				$ghbotid[%mynum]=0;
				$ghbot[%mynum]=0;
			}
		}
	}
	else
	{
//focusserver();
		schedule("golom_house();", 5);
//focusclient();
	}
}

function ttl_it()
{
	for(%mynum=1;%mynum<=$ghmax;%mynum++)
	{
		if($ghttl[%mynum]>0)
		{
			$ghttl[%mynum]-=20;//30
		}
		if($ghttl[%mynum]>0&&$ghid[%mynum]!=0)
		{
			$ghnum++;
		}
		else
		{
			$ghid[%mynum]=0;
		}
	}
	for(%mynum=1;%mynum<=$botmax;%mynum++)
	{
		for(%is_num=1;%is_num<=$botmax;%is_num++)
		{
			if(%is_num!=%mynum)
			{
				if($botid[%mynum]==$botid[%is_num])
				{
					if($ttl[%is_num]>$ttl[%mynum])
					{
						$ttl[%mynum]=$ttl[%is_num];
					}
					$ttl[%is_num]=0;
					$botid[%is_num]=0;
				}
			}
		}
		if($ttl[%mynum]>0)
		{
			$ttl[%mynum]-=20;//20
		}
		if($ttl[%mynum]<=0&&$botid[%mynum]!=0)
		{
			%die=True;
			for(%id=Client::getFirst();%id!=-1;%id=Client::getNext(%id))
			{
				//town==5dist,monstor==50dist
				%dist=Vector::getDistance(GameBase::getPosition($botid[%mynum]),GameBase::getPosition(%id));
				if(%dist<0)
					%dist*=-1;
				if(%dist<50)
				{
					%die=False;
					break;
				}
			}
			if(%die==True)
			{
				for(%fnum=1;%fnum<=$ghbotmax;%fnum++)
				{
					if($botid[%mynum]==$ghbotid[%fnum])
					{
						$ghbotid[%fnum]=0;
						$ghbot[%fnum]=0;
					}
				}
				if(Player::isAiControlled($botid[%mynum]))
				{
					felloffmap($botid[%mynum]);
				}
				$botid[%mynum]=0;
				$ttl[%mynum]=0;
				$bots--;
			}
			else
			{
				$ttl[%mynum]+=60;
			}
		}
		if(isdead($botid[%mynum])||!Player::isAiControlled($botid[%mynum])||Client::getName($botid[%mynum])=="")
		{
			for(%fnum=1;%fnum<=$ghbotmax;%fnum++)
			{
				if($botid[%mynum]==$ghbotid[%fnum])
				{
					$ghbotid[%fnum]=0;
					$ghbot[%fnum]=0;
				}
			}
			$botid[%mynum]=0;
			$ttl[%mynum]=0;
			$bots--;
		}
	}
//focusserver();
	schedule("ttl_it();", 20);//30
//focusclient();
}

//----------------------------------------
//TowerSwitch::onCollision()
//---------------------------------------

function TowerSwitch::onCollision(%this, %object)
{
	%playerId = Player::getClient(%object);
	%nogo=True;
	for(%id=Client::getFirst();%id!=-1;%id=Client::getNext(%id))
	{
		if(%id==%playerId)
		{
			%nogo=False;
			break;
		}
	}
	if(%nogo==True)
	{
	}
	else
	{
		$r=floor(getRandom()*100);
		$temp_x=GetWord(GameBase::getPosition(%this), 0);
		$temp_y=GetWord(GameBase::getPosition(%this), 1);
		$temp_z=GetWord(GameBase::getPosition(%this), 2)+2;

		if(%this.ObjectiveName=="HR2")
		{
			if($COINS[%playerId]>=1000)
			{
				$COINS[%playerId]-=1000;
				bottomprint(%playerId, "<f1><jc>Rented a horse for 1000 gil -- use #mount and #unmount on the horse.", 10);
				make_horse("-348 -148 50");
			}
			else
			{
				bottomprint(%playerId, "<f1><jc>Rented horse cost 1000 gold.", 1);
			}
		}
		else if(%this.ObjectiveName=="BOTMINE1")
		{
			if($botmine1==0)
			{
				//bottomprint(%playerId, "<f1><jc>You Spawned a BOTMINE1", 1);
				spawn_ambush($temp_x @ " " @ $temp_y @ " " @ $temp_z,getFinalLVL(%playerId),"scavpup");
				$botmine1=1;
//focusserver();
			      schedule("$botmine1=0;",$spawn_time);
//schedule("set_value(\"$botmine1\");",$spawn_time);
//focusclient();
			}
		}
		else if(%this.ObjectiveName=="BOTMINE2")
		{
			if($botmine2==0)
			{
				//bottomprint(%playerId, "<f1><jc>You Spawned a BOTMINE2", 1);
				spawn_ambush($temp_x @ " " @ $temp_y @ " " @ $temp_z,getFinalLVL(%playerId),"runtthief");
				$botmine2=1;
//focusserver();
			      schedule("$botmine2=0;",$spawn_time);
//schedule("set_value(\"$botmine2\");",$spawn_time);
//focusclient();
			}
		}
		else if(%this.ObjectiveName=="BOTMINE3")
		{
			if($botmine3==0)
			{
				//bottomprint(%playerId, "<f1><jc>You Spawned a BOTMINE3", 1);
				spawn_ambush($temp_x @ " " @ $temp_y @ " " @ $temp_z,getFinalLVL(%playerId),"raiderthief");
				$botmine3=1;
//focusserver();
			      schedule("$botmine3=0;",$spawn_time);
//schedule("set_value(\"$botmine3\");",$spawn_time);
//focusclient();
			}
		}
		else if(%this.ObjectiveName=="BOTTEMP1")
		{
			if($bottemp1==0)
			{
				//bottomprint(%playerId, "<f1><jc>You Spawned a BOTTEMP1", 1);
				spawn_ambush($temp_x @ " " @ $temp_y @ " " @ $temp_z, getFinalLVL(%playerId), "skelghoul");
				$bottemp1=1;
//focusserver();
			      schedule("$bottemp1=0;",$spawn_time);
//schedule("set_value(\"$bottemp1\");",$spawn_time);
//focusclient();
			}
		}
		else if(%this.ObjectiveName=="BOTFORE1")
		{
			if($botfore1==0)
			{
				//bottomprint(%playerId, "<f1><jc>You Spawned a BOTFORE1", 1);
				spawn_ambush($temp_x @ " " @ $temp_y @ " " @ $temp_z,getFinalLVL(%playerId),"protectorpeacekeeper");
				$botfore1=1;
//focusserver();
			      schedule("$botfore1=0;",$spawn_time);
//schedule("set_value(\"$botfore1\");",$spawn_time);
//focusclient();
			}
		}
		else if(%this.ObjectiveName=="BOTYOLA1")
		{
			if($botyola1==0)
			{
				//bottomprint(%playerId, "<f1><jc>You Spawned a BOTYOLA1", 1);
				spawn_ambush($temp_x @ " " @ $temp_y @ " " @ $temp_z,getFinalLVL(%playerId),"warlockruffian");
				$botyola1=1;
//focusserver();
			      schedule("$botyola1=0;",$spawn_time);
//schedule("set_value(\"$botyola1\");",$spawn_time);
//focusclient();
			}
		}
		else if(%this.ObjectiveName=="BOTYOLA2")
		{
			if($botyola2==0)
			{
				//bottomprint(%playerId, "<f1><jc>You Spawned a BOTYOLA2", 1);
				spawn_ambush($temp_x @ " " @ $temp_y @ " " @ $temp_z,getFinalLVL(%playerId),"warlockberserker");
				$botyola2=1;
//focusserver();
			      schedule("$botyola2=0;",$spawn_time);
//schedule("set_value(\"$botyola2\");",$spawn_time);
//focusclient();
			}
		}
		else if(%this.ObjectiveName=="BOTYOLA3")
		{
			if($botyola3==0)
			{
				//bottomprint(%playerId, "<f1><jc>You Spawned a BOTYOLA2", 1);
				spawn_ambush($temp_x @ " " @ $temp_y @ " " @ $temp_z,getFinalLVL(%playerId),"ravagerberserker");
				$botyola3=1;
//focusserver();
			      schedule("$botyola3=0;",$spawn_time);
//schedule("set_value(\"$botyola3\");",$spawn_time);
//focusclient();
			}
		}
		else if(%this.ObjectiveName=="BOTMINO3")
		{
			if($botmino3==0)
			{
				//bottomprint(%playerId, "<f1><jc>You Spawned a BOTYOLA2", 1);
				spawn_ambush($temp_x @ " " @ $temp_y @ " " @ $temp_z,getFinalLVL(%playerId),"reapergoliath");
				$botmino3=1;
//focusserver();
			      schedule("$botmino3=0;",$spawn_time);
//schedule("set_value(\"$botmino3\");",$spawn_time);
//focusclient();
			}
		}
		else if(%this.ObjectiveName=="CSHIP1")
		{
			if($travspawn1==0)
			{
				//bottomprint(%playerId, "<f1><jc>You Spawned a BOTYOLA1", 1);
				spawn_ambush($temp_x @ " " @ $temp_y @ " " @ $temp_z,getFinalLVL(%playerId),"knightknight");
				$travspawn1=1;
//focusserver();
			      schedule("$travspawn1=0;",$spawn_time);
//schedule("set_value(\"$botyola1\");",$spawn_time);
//focusclient();
			}
		}
		else if(%this.ObjectiveName=="TRAVSPAWN2")
		{
			if($travspawn2==0)
			{
				//bottomprint(%playerId, "<f1><jc>You Spawned a BOTYOLA1", 1);
				spawn_ambush($temp_x @ " " @ $temp_y @ " " @ $temp_z,getFinalLVL(%playerId),"bardbard");
				$travspawn2=1;
//focusserver();
			      schedule("$travspawn2=0;",$spawn_time);
//schedule("set_value(\"$botyola1\");",$spawn_time);
//focusclient();
			}
		}
		else if(%this.ObjectiveName=="CSHIP2")
		{
			if($travspawn3==0)
			{
				//bottomprint(%playerId, "<f1><jc>You Spawned a BOTYOLA1", 1);
				spawn_ambush($temp_x @ " " @ $temp_y @ " " @ $temp_z,getFinalLVL(%playerId),"paladinpaladin");
				$travspawn3=1;
//focusserver();
			      schedule("$travspawn3=0;",$spawn_time);
//schedule("set_value(\"$botyola1\");",$spawn_time);
//focusclient();
			}
		}
		else if(%this.ObjectiveName=="TRAVSPAWN4")
		{
			if($travspawn4==0)
			{
				//bottomprint(%playerId, "<f1><jc>You Spawned a BOTYOLA1", 1);
				spawn_ambush($temp_x @ " " @ $temp_y @ " " @ $temp_z,getFinalLVL(%playerId),"militiamilitia");
				$travspawn4=1;
//focusserver();
			      schedule("$travspawn4=0;",$spawn_time);
//schedule("set_value(\"$botyola1\");",$spawn_time);
//focusclient();
			}
		}
		else if(%this.ObjectiveName=="SLOTHUNDERLING")
		{
			if($slothunderling==0)
			{
				//bottomprint(%playerId, "<f1><jc>You Spawned a BOTYOLA1", 1);
				spawn_ambush($temp_x @ " " @ $temp_y @ " " @ $temp_z,getFinalLVL(%playerId),"slothunderling");
				$slothunderling=1;
//focusserver();
			      schedule("$slothunderling=0;",$spawn_time);
//schedule("set_value(\"$botyola1\");",$spawn_time);
//focusclient();
			}
		}
		else if(%this.ObjectiveName=="SLOTHGOHORT")
		{
			if($slothgohort==0)
			{
				//bottomprint(%playerId, "<f1><jc>You Spawned a BOTYOLA1", 1);
				spawn_ambush($temp_x @ " " @ $temp_y @ " " @ $temp_z,getFinalLVL(%playerId),"slothgohort");
				$slothgohort=1;
//focusserver();
			      schedule("$slothgohort=0;",$spawn_time);
//schedule("set_value(\"$botyola1\");",$spawn_time);
//focusclient();
			}
		}
		else if(%this.ObjectiveName=="GOHORTWRATH")
		{
			if($gohortwrath==0)
			{
				//bottomprint(%playerId, "<f1><jc>You Spawned a BOTYOLA1", 1);
				spawn_ambush($temp_x @ " " @ $temp_y @ " " @ $temp_z,getFinalLVL(%playerId),"gohortwrath");
				$gohortwrath=1;
//focusserver();
			      schedule("$gohortwrath=0;",$spawn_time);
//schedule("set_value(\"$botyola1\");",$spawn_time);
//focusclient();
			}
		}
		else if(%this.ObjectiveName=="LORDCHAMPION")
		{
			if($lordchampion==0)
			{
				//bottomprint(%playerId, "<f1><jc>You Spawned a BOTYOLA1", 1);
				spawn_ambush($temp_x @ " " @ $temp_y @ " " @ $temp_z,getFinalLVL(%playerId),"lordchampion");
				$lordchampion=1;
//focusserver();
			      schedule("$lordchampion=0;",$spawn_time);
//schedule("set_value(\"$botyola1\");",$spawn_time);
//focusclient();
			}
		}
		else if(%this.ObjectiveName=="Jail")
		{
			bottomprint(%playerId, "<f1><jc>Get outta JAIL", 1);
			GameBase::setPosition(%playerId, "1226 908 401");
		}
		else if(%this.ObjectiveName=="TOK")
		{
			if($r<25)//$bots<=$botmax&&
			{
				bottomprint(%playerId, "<f1><jc>AMBUSH", 1);
				GameBase::setPosition(%playerId, "-2442 -183 50");

				spawn_ambush("-2442 -183 55",getFinalLVL(%playerId),"ambusher");
			}
			else
			{
				bottomprint(%playerId, "<f1><jc>To KeldrinTown", 1);
				GameBase::setPosition(%playerId, "-2442 -183 50");
			}
		}
		else if(%this.ObjectiveName=="NEW")
		{
			if($r<25)//$bots<=$botmax&&
			{
				bottomprint(%playerId, "<f1><jc>AMBUSH", 1);
				GameBase::setPosition(%playerId, "-2422 -248 -9934.61");

				spawn_ambush("-2422 -248 -9929.61",getFinalLVL(%playerId),"ambusher");
			}
			else
			{
				bottomprint(%playerId, "<f1><jc>To NEWKeldrinTown", 1);
				GameBase::setPosition(%playerId, "-2422 -248 -9934.61");
			}
		}
		else if(%this.ObjectiveName=="ZOO")
		{
			bottomprint(%playerId, "<f1><jc>The Zoo", 1);
			GameBase::setPosition(%playerId, (getrandom()*348)-2555 @ " " @ (getrandom()*330)-420 @ " " @ -19889);
		}
		else if(%this.ObjectiveName=="TOKO")
		{
			if($r<25)//$bots<=$botmax&&
			{
				bottomprint(%playerId, "<f1><jc>AMBUSH", 1);
				GameBase::setPosition(%playerId, "1176.3 894.7 402");

				spawn_ambush("1176 894 406",getFinalLVL(%playerId),"ambusher");
			}
			else
			{
				//bottomprint(%playerId, "<f1><jc>To Fort Ethren", 1);
				bottomprint(%playerId, "<f1><jc>To Fort Chee", 1);
				GameBase::setPosition(%playerId, "1176.3 894.7 402");
			}
		}
		else if(%this.ObjectiveName=="mineseasy")
		{
			if($r<25)//$bots<=$botmax&&
			{
				bottomprint(%playerId, "<f1><jc>Caution", 1);
				GameBase::setPosition(%playerId, "1176.3 894.7 402");

				spawn_ambush("1176 894 406",getFinalLVL(%playerId),"ambusher");
			}
			else
			{
				//bottomprint(%playerId, "<f1><jc>To Fort Ethren", 1);
				bottomprint(%playerId, "<f1><jc>Dungeon - Mines - EASY", 1);
				GameBase::setPosition(%playerId, "1007.55 639.267 1367.47");
			}
		}
		else if(%this.ObjectiveName=="minesmoderate")
		{
			if($r<25)//$bots<=$botmax&&
			{
				bottomprint(%playerId, "<f1><jc>Caution", 1);
				GameBase::setPosition(%playerId, "1176.3 894.7 402");

				spawn_ambush("1176 894 406",getFinalLVL(%playerId),"ambusher");
			}
			else
			{
				//bottomprint(%playerId, "<f1><jc>To Fort Ethren", 1);
				bottomprint(%playerId, "<f1><jc>Dungeon - Mines - MODERATE", 1);
				GameBase::setPosition(%playerId, "947.006 623.48 1367.46");
			}
		}
		else if(%this.ObjectiveName=="mineshard")
		{
			if($r<25)//$bots<=$botmax&&
			{
				bottomprint(%playerId, "<f1><jc>Caution", 1);
				GameBase::setPosition(%playerId, "1176.3 894.7 402");

				spawn_ambush("1176 894 406",getFinalLVL(%playerId),"ambusher");
			}
			else
			{
				//bottomprint(%playerId, "<f1><jc>To Fort Ethren", 1);
				bottomprint(%playerId, "<f1><jc>Dungeon - Mines - HARD", 1);
				GameBase::setPosition(%playerId, "883.754 592.356 1373.25");
			}
		}
		else if(%this.ObjectiveName=="minesback")
		{
			if($r<25)//$bots<=$botmax&&
			{
				bottomprint(%playerId, "<f1><jc>Safety", 1);
				GameBase::setPosition(%playerId, "1176.3 894.7 402");

				spawn_ambush("1176 894 406",getFinalLVL(%playerId),"ambusher");
			}
			else
			{
				//bottomprint(%playerId, "<f1><jc>To Fort Ethren", 1);
				bottomprint(%playerId, "<f1><jc>Dungeon - Mines - Difficulty Select", 1);
				GameBase::setPosition(%playerId, "985.081 608.843 1359.34");
			}
		}
		else if(%this.ObjectiveName=="TOJN")
		{
			if($r<25)//$bots<=$botmax&&
			{
				bottomprint(%playerId, "<f1><jc>AMBUSH", 1);
				GameBase::setPosition(%playerId, "1710 -220 50");

				spawn_ambush("1710 -220 55",getFinalLVL(%playerId),"ambusher");
			}
			else
			{
				bottomprint(%playerId, "<f1><jc>To Jaten", 1);
				GameBase::setPosition(%playerId, "1710 -220 50");
			}
		}
		else if(%this.ObjectiveName=="TOMT")
		{
			if($r<25)//$bots<=$botmax&&
			{
				bottomprint(%playerId, "<f1><jc>AMBUSH", 1);
				GameBase::setPosition(%playerId, "-2652 630 402");

				spawn_ambush("-2652 630 406",getFinalLVL(%playerId), "ambusher");
			}
			else
			{
				bottomprint(%playerId, "<f1><jc>To MageTower", 1);
				GameBase::setPosition(%playerId, "-2652 630 402");
			}
		}
		else if(%this.ObjectiveName=="TOKM")
		{
			if($r<25)//$bots<=$botmax&&
			{
				bottomprint(%playerId, "<f1><jc>AMBUSH", 1);
				GameBase::setPosition(%playerId, "-1888 -852 50");

				spawn_ambush("-1888 -852 55",getFinalLVL(%playerId), "ambusher");
			}
			else
			{
				bottomprint(%playerId, "<f1><jc>To KeldrinMines", 1);
				GameBase::setPosition(%playerId, "-1888 -852 50");
			}
		}
		else if(%this.ObjectiveName=="TOYA")
		{
			if($r<25)//$bots<=$botmax&&
			{
				bottomprint(%playerId, "<f1><jc>AMBUSH", 1);
				GameBase::setPosition(%playerId, "-1099 -391 50");

				spawn_ambush("-1099 -391 55",getFinalLVL(%playerId), "ambusher");
			}
			else
			{
				bottomprint(%playerId, "<f1><jc>To Yolanda", 1);
				GameBase::setPosition(%playerId, "-1099 -391 50");
			}
		}
		else if(%this.ObjectiveName=="TOAT")
		{
			if($r<25)//$bots<=$botmax&&
			{
				bottomprint(%playerId, "<f1><jc>AMBUSH", 1);
				GameBase::setPosition(%playerId, "2064 -931 49");

				spawn_ambush("2064 -931 54",getFinalLVL(%playerId), "ambusher");
			}
			else
			{
				bottomprint(%playerId, "<f1><jc>To AncientTemple", 1);
				GameBase::setPosition(%playerId, "2064 -931 49");
			}
		}
		else if(%this.ObjectiveName=="TOEF")
		{
			if($r<25)//$bots<=$botmax&&
			{
				bottomprint(%playerId, "<f1><jc>AMBUSH", 1);
				GameBase::setPosition(%playerId, "2906.5 -265 50");

				spawn_ambush("2906.5 -295 55",getFinalLVL(%playerId), "ambusher");
			}
			else
			{
				bottomprint(%playerId, "<f1><jc>To ElfHideout", 1);
				GameBase::setPosition(%playerId, "2906.5 -265 50");
			}
		}
		else if(%this.ObjectiveName=="TONM")//to rentabot
		{
			if($r<25)//$bots<=$botmax&&
			{
				bottomprint(%playerId, "<f1><jc>AMBUSH", 1);
				GameBase::setPosition(%playerId, "-395 -106 50");

				spawn_ambush("-395 -106 55",getFinalLVL(%playerId), "ambusher");
			}
			else
			{
				bottomprint(%playerId, "<f1><jc>To Nevim", 1);
				GameBase::setPosition(%playerId, "-395 -106 50");
			}
		}
		else if(%this.ObjectiveName=="TOJY")
		{
			if($r<25)//$bots<=$botmax&&
			{
				bottomprint(%playerId, "<f1><jc>AMBUSH", 1);
				GameBase::setPosition(%playerId, "-1366.5 555.2 46");

				spawn_ambush("-1366.5 555.2 50",getFinalLVL(%playerId), "ambusher");
			}
			else
			{
				bottomprint(%playerId, "<f1><jc>To Jeffrey", 1);
				GameBase::setPosition(%playerId, "-1366.5 555.2 46");
			}
		}
		else if(%this.ObjectiveName=="KeldrinAjaten")
		{
			if($r<25)//$bots<=$botmax&&
			{
				bottomprint(%playerId, "<f1><jc>Ambush!", 1);
				GameBase::setPosition(%playerId, "1710 -220 50");

				spawn_ambush("-479.451 -578.448 495.715",getFinalLVL(%playerId),"ambusher");
			}
			else
			{
				bottomprint(%playerId, "<f1><jc>Welcome to Jaten Aerial", 1);
				GameBase::setPosition(%playerId, "-479.451 -578.448 490.715");
			}
		}
		else if(%this.ObjectiveName=="AjatenKeldrin")
		{
			if($r<25)//$bots<=$botmax&&
			{
				bottomprint(%playerId, "<f1><jc>Ambush!", 1);
				GameBase::setPosition(%playerId, "1710 -220 50");

				spawn_ambush("36.9145 46.675 10.28151",getFinalLVL(%playerId),"ambusher");
			}
			else
			{
				bottomprint(%playerId, "<f1><jc>Welcome to Keldrin Town", 1);
				GameBase::setPosition(%playerId, "36.9145 46.675 5.28151");
			}
		}
		else if(%this.ObjectiveName=="MinesKeldrin")
		{
			if($r<25)//$bots<=$botmax&&
			{
				bottomprint(%playerId, "<f1><jc>Ambush!", 1);
				GameBase::setPosition(%playerId, "1710 -220 50");

				spawn_ambush("36.9145 46.675 10.28151",getFinalLVL(%playerId),"ambusher");
			}
			else
			{
				bottomprint(%playerId, "<f1><jc>Welcome to Keldrin Town", 1);
				GameBase::setPosition(%playerId, "-76.2443 2.77734 2.97927");
			}
		}
		else if(%this.ObjectiveName=="KeldrinMines")
		{
			if($r<25)//$bots<=$botmax&&
			{
				bottomprint(%playerId, "<f1><jc>Ambush!", 1);
				GameBase::setPosition(%playerId, "1710 -220 50");

				spawn_ambush("36.9145 46.675 10.28151",getFinalLVL(%playerId),"ambusher");
			}
			else
			{
				bottomprint(%playerId, "<f1><jc>You have entered the dungeon : Keldrin Mines", 1);
				GameBase::setPosition(%playerId, "1101.37 577 1359.75");
			}
		}
		else if(%this.ObjectiveName=="minoin")
		{
			if($r<25)//$bots<=$botmax&&
			{
				bottomprint(%playerId, "<f1><jc>Ambush!", 1);
				GameBase::setPosition(%playerId, "1710 -220 50");

				spawn_ambush("36.9145 46.675 10.28151",getFinalLVL(%playerId),"ambusher");
			}
			else
			{
				bottomprint(%playerId, "<f1><jc>Minotaur Lair : Caution this is a dungeon!", 1);
				GameBase::setPosition(%playerId, "2208.48 974.178 9.93574");
			}
		}
		else if(%this.ObjectiveName=="FIGHT"&&$new_fighting_system==1&&(%playerId.inFight<=0))//start fight
		{
			bottomprint(%playerId, "<f1><jc>In Fight", 1);
			%bot=newObject("OrcRavager","Player",orcarmor,true);
			if(%bot)
			{
				addToSet("MissionCleanup",%bot);
				GameBase::setTeam(%bot,2);
				GameBase::setPosition(%bot,$temp_x @ " " @ $temp_y @ " " @ $temp_z);
				Gamebase::setMapName(%bot,"OrcRavager");
				UpdateSkin(%bot);
//Player::setAnimation(%bot,35);
//$MaxHP[%bot]=10;
//$MyMAXHP[%bot]=10;
//$MyHP[%bot]=10;
//ClearVariables(%bot);
				$BotInfoAiName[%bot]=Ravager;
				$RACE[%bot]=Orc;
				$LCKconsequence[%bot]="miss";
				$HasLoadedAndSpawned[%bot]=True;
//$botAttackMode[%bot] = 1;
//$tmpbotdata[%bot] = "";
//setHP(%bot, $MaxHP[%bot]);
//setMANA(%bot, $botinitmana);
//refreshHPREGEN(%bot);
//refreshMANAREGEN(%bot);
//$AP[%bot, 1] = $BotInfo[Ravager, STR];
//$AP[%bot, 2] = $BotInfo[Ravager, DEX];
//$AP[%bot, 3] = $BotInfo[Ravager, CON];
//$AP[%bot, 4] = $BotInfo[Ravager, INT];
//$AP[%bot, 5] = $BotInfo[Ravager, WIS];
//$LCK[%bot] = $BotInfo[Ravager, LCK];
//%items = $BotInfo[Ravager, ITEMS];
//if(%items == "")
				GiveThisStuff(%bot,$BotEquipment[Ravager]);
				RemoteNextWeapon(%bot);
				Game::refreshClientScore(%bot);
				add_to_fight(%bot,1,"bot");
				add_to_fight(Client::getName(%playerId),1,"human");
				%Rotation=GameBase::GetRotation(%playerId);
				%Length=100;
				%Zvalue=0;
				%Vector=Vector::getFromRot(%Rotation,%Length,%Zvalue);
				GameBase::setRotation(%bot,%Rotation);
				Player::applyImpulse(%bot,%Vector);
			}
		}
		else if(%this.ObjectiveName=="TOGH1"||%this.ObjectiveName=="TOGH2"||%this.ObjectiveName=="TOGH3")
		{
			%temp_loc=0;
			$ghnum=0;
			bottomprint(%playerId, "<f1><jc>To GolomHouse", 1);
			for(%mynum=1;%mynum<=$ghmax;%mynum++)
			{
				if($ghid[%mynum]==%playerId)
				{
					%temp_loc=1;
					break;
				}
			}
			if(%temp_loc==0)
			{
				for(%mynum=1;%mynum<=$ghmax;%mynum++)
				{
					if($ghid[%mynum]==0)
					{
						$ghttl[%mynum]=60;
						$ghid[%mynum]=%playerId;
						break;
					}
				}
			}
			for(%mynum=1;%mynum<=$ghmax;%mynum++)
			{
				if($ghid[%mynum]!=0)
				{
					$ghnum++;
				}
			}
			if($ghnum<3)
			{
				bottomprint(%playerId, "<f1><jc>You need 3 people to start a Golom Quest", 1);
			}
			else
			{
				GameBase::setPosition($ghid[1], "347.5 -983 42");
				GameBase::setPosition($ghid[2], "351 -983 42");
				GameBase::setPosition($ghid[3], "356 -983 42");
				$gh_going=false;
				golom_house();
			}
		}
	}
}

function spawn_ambush(%position,%plvl,%type,%GiveThisStuff)
{
	$r=floor(getRandom()*100);
	for(%mynum=1;%mynum<=$botmax;%mynum++)
	{
		if($botid[%mynum]==0)
		{
			%n=0;
			%avg=0;
			if(%plvl==0)
			{
				if(%type=="")
				{
					%avg=$zonerroamer;
				}
				else
				{
					for(%fnum=1;%fnum<=$ghmax;%fnum++)
					{
						if($ghid[%fnum]!=0)
						{
							%avg+=getFinalLVL($ghid[%fnum]);
						}
					}
					%avg=(%avg/$ghmax)+2;
				}
			}
			else
			{
				%avg=%plvl;
			}
			%it_team=0;
			%bottype=0;
			if(%type=="")
			{
				%bottype=floor(getRandom()*100);
			}
			if(%type=="scavpup"||(%type==""&&%bottype<10))
			{
				if(%avg>7)
				{
					%avg=7;
				}
				if(%avg<3)
				{
					%avg=3;
				}
				%it_team=3;
				if($r<25)
				{
					%n = AI::helper("scavenger", "GnollScavenger" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("pup", "GnollPup" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%type=="runtthief"||(%type==""&&%bottype<20))
			{
				if(%avg>5)
				{
					%avg=5;
				}
				if(%avg<2)
				{
					%avg=2;
				}
				%it_team=2;
				if($r<50)
				{
					%n = AI::helper("runt", "Goblin" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("thief", "Goblin" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%type=="raiderthief"||(%type==""&&%bottype<40))
			{
				if(%avg>5)
				{
					%avg=5;
				}
				if(%avg<2)
				{
					%avg=2;
				}
				%it_team=2;
				if($r<60)
				{
					%n = AI::helper("raider", "Goblin" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("thief", "Goblin" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%type=="skelghoul"||(%type==""&&%bottype<45))
			{
				if(%avg<7)
				{
					%avg=7;
				}
				%it_team=4;
				if($r<60)
				{
					%n = AI::helper("skeleton", "Skeleton" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("ghoul", "Skeleton" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%type=="warlockruffian"||(%type==""&&%bottype<55))
			{
				if(%avg>10)
				{
					%avg=10;
				}
				if(%avg<5)
				{
					%avg=5;
				}
				%it_team=2;
				if($r<70)
				{
					%n = AI::helper("warlock", "OrcWarlock" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("ruffian", "OgreRuffian" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%type=="warlockberserker"||(%type==""&&%bottype<60))
			{
				if(%avg>10)
				{
					%avg=10;
				}
				if(%avg<5)
				{
					%avg=5;
				}
				%it_team=2;
				if($r<50)
				{
					%n = AI::helper("warlock", "OrcWarlock" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("berserker", "OrcBerserker" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%type=="ravagerberserker"||(%type==""&&%bottype<65))
			{
				if(%avg>10)
				{
					%avg=10;
				}
				if(%avg<5)
				{
					%avg=5;
				}
				%it_team=2;
				if($r<50)
				{
					%n = AI::helper("ravager", "OrcRavager" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("berserker", "OrcBerserker" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%type=="protectorpeacekeeper"||(%type==""&&%bottype<70))
			{
				if(%avg>10)
				{
					%avg=10;
				}
				if(%avg<3)
				{
					%avg=3;
				}
				%it_team=5;
				if($r<25)
				{
					%n = AI::helper("protector", "ElfProtector" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("peacekeeper", "ElfPeaceKeeper" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%type=="reapergoliath"||(%type==""&&%bottype<75))
			{
				if(%avg>10)
				{
					%avg=10;
				}
				if(%avg<5)
				{
					%avg=5;
				}
				%it_team=6;
				if($r<50)
				{
					%n = AI::helper("reaper", "MinotaurReaper" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("goliath", "MinotaurGoliath" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%type=="knightknight"||(%type==""&&%bottype<80))
			{
				if(%avg>10)
				{
					%avg=10;
				}
				if(%avg<5)
				{
					%avg=5;
				}
				%it_team=6;
				if($r<50)
				{
					%n = AI::helper("knight", "SAM0bG" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("knight", "SAM0bG" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%type=="paladinpaladin"||(%type==""&&%bottype<85))
			{
				if(%avg>10)
				{
					%avg=10;
				}
				if(%avg<5)
				{
					%avg=5;
				}
				%it_team=6;
				if($r<50)
				{
					%n = AI::helper("paladin", "SAM1bG" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("paladin", "SAM1bG" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%type=="bardbard"||(%type==""&&%bottype<90))
			{
				if(%avg>10)
				{
					%avg=10;
				}
				if(%avg<5)
				{
					%avg=5;
				}
				%it_team=1;
				if($r<50)
				{
					%n = AI::helper("bard", "TravellerBard" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("bard", "TravellerBard" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%type=="militiamilitia"||(%type==""&&%bottype<95))
			{
				if(%avg>10)
				{
					%avg=10;
				}
				if(%avg<5)
				{
					%avg=5;
				}
				%it_team=1;
				if($r<50)
				{
					%n = AI::helper("militia", "TravellerMilitia" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("militia", "TravellerMilitia" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%type=="slothunderling"||(%type==""&&%bottype<100))
			{
				if(%avg>10)
				{
					%avg=10;
				}
				if(%avg<5)
				{
					%avg=5;
				}
				%it_team=7;
				if($r<50)
				{
					%n = AI::helper("sloth", "Sloth" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("underling", "Underling" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%type=="slothgohort"||(%type==""&&%bottype<105))
			{
				if(%avg>10)
				{
					%avg=10;
				}
				if(%avg<5)
				{
					%avg=5;
				}
				%it_team=7;
				if($r<50)
				{
					%n = AI::helper("sloth", "Sloth" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("gohort", "Gohort" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%type=="gohortwrath"||(%type==""&&%bottype<105))
			{
				if(%avg>10)
				{
					%avg=10;
				}
				if(%avg<5)
				{
					%avg=5;
				}
				%it_team=7;
				if($r<50)
				{
					%n = AI::helper("gohort", "Gohort" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("wrath", "Wrath" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%type=="lordchampion"||(%type==""&&%bottype<110))
			{
				if(%avg>10)
				{
					%avg=10;
				}
				if(%avg<5)
				{
					%avg=5;
				}
				%it_team=5;
				if($r<50)
				{
					%n = AI::helper("lord", "ElfLord" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("champion", "ElfChampion" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%type=="minigolom"||(%type==""&&%bottype<72))
			{
				%it_team=1;
				%n = AI::helper("pup", "MiniGolom", "MarkerSpawn 123");
			}
			else if(%type==""&&%bottype<=100)
			{
				break;
			}
			else if(%type=="golom")
			{
				%it_team=1;
				%n = AI::helper("ruffian", "GolomBoss", "MarkerSpawn 123");
				%avg+=4;//BOSS HARD
			}
			else if(%avg<=2&&%type!="")
			{
				if($r<50)
				{
					%n = AI::helper("thief", "Ambusher" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("runt", "Ambusher" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%avg<=3&&%type!="")
			{
				if($r<50)
				{
					%n = AI::helper("raider", "Ambusher" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("warlock", "Ambusher" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%avg<=4&&%type!="")
			{
				if($r<50)
				{
					%n = AI::helper("warlock", "Ambusher" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("pup", "Ambusher" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%avg<=5&&%type!="")
			{
				if($r<50)
				{
					%n = AI::helper("scavenger", "Ambusher" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("berserker", "Ambusher" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%avg<=6&&%type!="")
			{
				if($r<50)
				{
					%n = AI::helper("ghoul", "Ambusher" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("ruffian", "Ambusher" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%avg<=12&&%type!="")
			{
				if($r<50)
				{
					%n = AI::helper("ghoul", "Ambusher" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("skeleton", "Ambusher" @ %mynum, "MarkerSpawn 123");
				}
			}
			else if(%avg<=20&&%type!="")
			{
				if($r<80)
				{
					%n = AI::helper("skeleton", "Ambusher" @ %mynum, "MarkerSpawn 123");
				}
				else
				{
					%n = AI::helper("militia", "Ambusher" @ %mynum, "MarkerSpawn 123");
				}
			}
			else
			{
				echo("EVIL");
				break;
			}
			//if(%id!=0)
			//{
				%avg-=2;//MAKE BOTS EASIER
				%id = AI::getId(%n);
				%exped=(%avg*(getrandom()+0.5))*GetExp(%avg, "Fighter")*2;
				if(%exped>$EXP[%id])
				{
					$EXP[%id] = %exped;
					Game::refreshClientScore(%id);
				}
				%hped=(getrandom()+0.5)*GetRoll(%avg @ "d8");
				if(%hped>$MaxHP[%id])
				{
					$MaxHP[%id] = %hped;
					setHP(%id, $MaxHP[%id]);
				}

				GameBase::setPosition(%id, %position);
				if(%it_team==0)
				{
					%it_team=floor(getRandom()*4)+1;
					if(%it_team==1)
					{
						%it_team++;
					}
				}
				GameBase::setTeam(%id, %it_team);
				if(%GiveThisStuff!="")
				{
					GiveThisStuff(%id, %GiveThisStuff);
				}
				Game::refreshClientScore(%id);
				UpdateSkin(%id);
				$bots++;
				$botid[%mynum]=%id;
				if(%plvl==0&&%type!="")
				{
					for(%fnum=1;%fnum<=$ghbotmax;%fnum++)
					{
						if($ghbotid[%fnum]==0)
						{
							$ghbotid[%fnum]=%id;
							break;
						}
					}
				}
				$ttl[%mynum]=100;
			//}
			break;
		}
	}
}

function spawn_thieves()
{
//"93.1718 -129.896 6.6208"
//"99.2912 -133.646 6.61608"
//"93.5761 -140.271 6.29727"
	for(%mynum=1;%mynum<=$botmax;%mynum++)
	{
		if($botid[%mynum]==0)
		{
			%n = AI::helper("thief", "Thief" @ %mynum, "MarkerSpawn 123");
			//if(%id!=0)
			//{
				%id = AI::getId(%n);
				%exped=(1*(getrandom()+0.5))*GetExp(1, "Fighter");
				if(%exped>$EXP[%id])
				{
					$EXP[%id] = %exped;
					Game::refreshClientScore(%id);
				}
				%hped=(getrandom()+0.5)*GetRoll(1 @ "d8");
				if(%hped>$MaxHP[%id])
				{
					$MaxHP[%id] = %hped;
					setHP(%id, $MaxHP[%id]);
				}

				Player::setItemCount(%id, BlackStatue, 0);
				GameBase::setPosition(%id, "138.832 118.702 7.36712");
				GameBase::setTeam(%id, 2);
				Game::refreshClientScore(%id);
				UpdateSkin(%id);
				$bots++;
				$botid[%mynum]=%id;
				$ttl[%mynum]=500;
			//}
			break;
		}
	}
	for(%mynum=1;%mynum<=$botmax;%mynum++)
	{
		if($botid[%mynum]==0)
		{
			%n = AI::helper("thief", "Thief" @ %mynum, "MarkerSpawn 123");
			//if(%id!=0)
			//{
				%id = AI::getId(%n);
				%exped=(1*(getrandom()+0.5))*GetExp(1, "Fighter");
				if(%exped>$EXP[%id])
				{
					$EXP[%id] = %exped;
					Game::refreshClientScore(%id);
				}
				%hped=(getrandom()+0.5)*GetRoll(1 @ "d8");
				if(%hped>$MaxHP[%id])
				{
					$MaxHP[%id] = %hped;
					setHP(%id, $MaxHP[%id]);
				}

				Player::setItemCount(%id, BlackStatue, 0);
				GameBase::setPosition(%id, "138.832 118.702 7.36712");
				GameBase::setTeam(%id, 2);
				Game::refreshClientScore(%id);
				UpdateSkin(%id);
				$bots++;
				$botid[%mynum]=%id;
				$ttl[%mynum]=500;
			//}
			break;
		}
	}
	for(%mynum=1;%mynum<=$botmax;%mynum++)
	{
		if($botid[%mynum]==0)
		{
			%n = AI::helper("thief", "Thief" @ %mynum, "MarkerSpawn 123");
			//if(%id!=0)
			//{
				%id = AI::getId(%n);
				%exped=(2*(getrandom()+0.5))*GetExp(2, "Fighter");
				if(%exped>$EXP[%id])
				{
					$EXP[%id] = %exped;
					Game::refreshClientScore(%id);
				}
				%hped=(getrandom()+0.5)*GetRoll(1 @ "d8");
				if(%hped>$MaxHP[%id])
				{
					$MaxHP[%id] = %hped;
					setHP(%id, $MaxHP[%id]);
				}

				Player::setItemCount(%id, TOME, 1);
				GameBase::setPosition(%id, "138.832 118.702 7.36712");
				GameBase::setTeam(%id, 2);
				Game::refreshClientScore(%id);
				UpdateSkin(%id);
				$bots++;
				$botid[%mynum]=%id;
				$ttl[%mynum]=500;
			//}
			break;
		}
	}
}

function spawn_dwarves()
{
//"138.832 118.702 7.36712"
//"141.832 118.702 7.36712"
//"142.832 119.702 7.36712"
	for(%mynum=1;%mynum<=$botmax;%mynum++)
	{
		if($botid[%mynum]==0)
		{
			%n = AI::helper("protector", "Dwarf" @ %mynum, "MarkerSpawn 123");
			//if(%id!=0)
			//{
				%id = AI::getId(%n);
				%exped=(3*(getrandom()+0.5))*GetExp(3, "Fighter");
				if(%exped>$EXP[%id])
				{
					$EXP[%id] = %exped;
					Game::refreshClientScore(%id);
				}
				%hped=(getrandom()+0.5)*GetRoll(2 @ "d8");
				if(%hped>$MaxHP[%id])
				{
					$MaxHP[%id] = %hped;
					setHP(%id, $MaxHP[%id]);
				}

				Player::setItemCount(%id, RocFeather, 0);
				//GameBase::setPosition(%id, "-1709 -1275.5 65");
				GameBase::setPosition(%id, "-1528.5 -1090 18.5");
				GameBase::setTeam(%id, 5);
				Game::refreshClientScore(%id);
				UpdateSkin(%id);
				$bots++;
				$botid[%mynum]=%id;
				$ttl[%mynum]=500;
			//}
			break;
		}
	}
	for(%mynum=1;%mynum<=$botmax;%mynum++)
	{
		if($botid[%mynum]==0)
		{
			%n = AI::helper("protector", "Dwarf" @ %mynum, "MarkerSpawn 123");
			//if(%id!=0)
			//{
				%id = AI::getId(%n);
				%exped=(3*(getrandom()+0.5))*GetExp(3, "Fighter");
				if(%exped>$EXP[%id])
				{
					$EXP[%id] = %exped;
					Game::refreshClientScore(%id);
				}
				%hped=(getrandom()+0.5)*GetRoll(2 @ "d8");
				if(%hped>$MaxHP[%id])
				{
					$MaxHP[%id] = %hped;
					setHP(%id, $MaxHP[%id]);
				}

				Player::setItemCount(%id, RocFeather, 0);
				//GameBase::setPosition(%id, "-1693 -1275.5 65");
				GameBase::setPosition(%id, "-1524 -1087.5 19");
				GameBase::setTeam(%id, 5);
				Game::refreshClientScore(%id);
				UpdateSkin(%id);
				$bots++;
				$botid[%mynum]=%id;
				$ttl[%mynum]=500;
			//}
			break;
		}
	}
	for(%mynum=1;%mynum<=$botmax;%mynum++)
	{
		if($botid[%mynum]==0)
		{
			%n = AI::helper("protector", "Dwarf" @ %mynum, "MarkerSpawn 123");
			//if(%id!=0)
			//{
				%id = AI::getId(%n);
				%exped=(5*(getrandom()+0.5))*GetExp(5, "Fighter");
				if(%exped>$EXP[%id])
				{
					$EXP[%id] = %exped;
					Game::refreshClientScore(%id);
				}
				%hped=(getrandom()+0.5)*GetRoll(3 @ "d8");
				if(%hped>$MaxHP[%id])
				{
					$MaxHP[%id] = %hped;
					setHP(%id, $MaxHP[%id]);
				}

				Player::setItemCount(%id, RocFeather, 1);
				//GameBase::setPosition(%id, "-1700 -1267.5 65");
				GameBase::setPosition(%id, "-1519 -1090 18.57");
				GameBase::setTeam(%id, 5);
				Game::refreshClientScore(%id);
				UpdateSkin(%id);
				$bots++;
				$botid[%mynum]=%id;
				$ttl[%mynum]=500;
			//}
			break;
		}
	}
}

function set_position(%id,%position)
{
	GameBase::setPosition(%id, %position);
}
function set_value(%name,%value)
{
	if(%value==""||%value==-1)
	{
		%value=0;
	}
	if(%name=="$botmine1")
	{
		$botmine1=%value;
	}
	else if(%name=="$botmine2")
	{
		$botmine2=%value;
	}
	else if(%name=="$botmine3")
	{
		$botmine3=%value;
	}
	else if(%name=="$botfore1")
	{
		$botfore1=%value;
	}
	else if(%name=="$bottemp1")
	{
		$bottemp1=%value;
	}
	if(%name=="$botyola1")
	{
		$botyola1=%value;
	}
	else if(%name=="$botyola2")
	{
		$botyola2=%value;
	}
	else if(%name=="$botyola3")
	{
		$botyola3=%value;
	}
	else if(%name=="$botmino3")
	{
		$botmino3=%value;
	}
	else if(%name=="$travspawn1")
	{
		$travspawn1=%value;
	}
	else if(%name=="$travspawn2")
	{
		$travspawn2=%value;
	}
	else if(%name=="$travspawn3")
	{
		$travspawn3=%value;
	}
	else if(%name=="$travspawn4")
	{
		$travspawn4=%value;
	}
	else if(%name=="$slothunderling")
	{
		$slothunderling=%value;
	}
	else if(%name=="$slothgohort")
	{
		$slothgohort=%value;
	}
	else if(%name=="$gohortwrath")
	{
		$gohortwrath=%value;
	}
	else if(%name=="$lordchampion")
	{
		$lordchampion=%value;
	}
}


function make_building(%type,%pos,%rot)//any .dis, position, rotation
{
	%build=newObject("Interior","InteriorShape",%type,true);
	if(%build)
	{
		addToSet("MissionCleanup", %build);
		GameBase::setPosition(%build,%rot);
		GameBase::setRotation(%build,%pos);
	}
	else
	{
		CenterPrintAll("Couldnt Create" @ %type);
		//error
	}
}