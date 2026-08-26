$PlayerAnim::Crouching = 25;
$PlayerAnim::DieChest = 26;
$PlayerAnim::DieHead = 27;
$PlayerAnim::DieGrabBack = 28;
$PlayerAnim::DieRightSide = 29;
$PlayerAnim::DieLeftSide = 30;
$PlayerAnim::DieLegLeft = 31;
$PlayerAnim::DieLegRight = 32;
$PlayerAnim::DieBlownBack = 33;
$PlayerAnim::DieSpin = 34;
$PlayerAnim::DieForward = 35;
$PlayerAnim::DieForwardKneel = 36;
$PlayerAnim::DieBack = 37;
$PlayerAnim::FirstDeath = $PlayerAnim::DieChest;
$PlayerAnim::LastDeath = $PlayerAnim::DieBack;
$CorpseTimeoutValue = 22;

function Player::onAdd(%this) 
{
	GameBase::setRechargeRate(%this,8);
}

function Player::onRemove(%this) 
{
	for (%i = 0; %i < 8; %i = %i + 1) 
	{
		%type = Player::getMountedItem(%this,%i);
		if (%type != -1) 
		{
			%item = newObject("","Item",%type,1,false);
			schedule("Item::Pop(" @ %item @ ");", $ItemPopTime, %item);
			addToSet("MissionCleanup", %item);
			GameBase::setPosition(%item,GameBase::getPosition(%this));
		}
	}
}

function Player::onNoAmmo(%player,%imageSlot,%itemType) 
{
}

function Player::onKilled(%this) 
{
	%cl = GameBase::getOwnerClient(%this);
	%cl.dead = 1;
	if($AutoRespawn > 0) schedule("Game::autoRespawn(" @ %cl @ ");",$AutoRespawn,%cl);
//	if(%this.outArea==1) leaveMissionAreaDamage(%cl);
	Player::setDamageFlash(%this,0.75);
	for (%i = 0; %i < 8; %i = %i + 1) 
	{
		%type = Player::getMountedItem(%this,%i);
		if (%type != -1) 
		{
			if (%i != $WeaponSlot || !Player::isTriggered(%this,%i) || getRandom() > "0.5") Player::dropItem(%this,%type);
		}
	}
	if(%cl != -1) 
	{
//---- Move to vehicle.cs
		if(%this.vehicle != "") 
		{
			if(%this.driver != "") 
			{
				%this.driver = "";
				%this.vehicle.Pilot = "";
				Client::setControlObject(Player::getClient(%this), %this);
				Player::setMountObject(%this, -1, 0);
				GameBase::virtual(%this.vehicle, onUnPilot, %this.vehicle, %this);
			}
			else 
			{
				%this.vehicle.Seat[%this.vehicleSlot-2] = "";
				%this.vehicleSlot = "";
			}
			%this.vehicle = "";
		}
//----
		schedule("GameBase::startFadeOut(" @ %this @ ");", $CorpseTimeoutValue, %this);
		Client::setOwnedObject(%cl, -1);
		Client::setControlObject(%cl, Client::getObserverCamera(%cl));
		Observer::setOrbitObject(%cl, %this, 5, 5, 5);
		schedule("deleteObject(" @ %this @ ");", $CorpseTimeoutValue + 2.5, %this);
		%cl.observerMode = "dead";
		%cl.dieTime = getSimTime();
	}
}

function Player::onDamage(%this,%type,%value,%pos,%vec,%mom,%vertPos,%quadrant,%object) 
{
	if (GameBase::getControlClient(%object) != "")
	{
		%this.lastDamageObject = %object;
		%this.LastHarm = %object;
	}
	if (Player::isExposed(%this)) 
	{
		%damagedClient = Player::getClient(%this);
		%shooterClient = %object;
		Player::applyImpulse(%this,%mom);
		if($teamplay && %damagedClient != %shooterClient && Client::getTeam(%damagedClient) == Client::getTeam(%shooterClient))
		{
			if (%shooterClient != -1) 
			{
				if(%this.DamageTime == "") %this.DamageTime = 0;
				%time = getIntegerTime(true) >> 5;
				%diff = %time - %this.DamageTime;
				if ((%diff > 10 || %this.LastHarm != %shooterClient) && %damagedClient != %shooterClient && $Server::TeamDamageScale > 0)
				{
					if(%type != $MineDamageType)
					{
						Client::sendMessage(%shooterClient,0,"You just shot your comrade, " @ Client::getName(%damagedClient) @ "!");
						Client::sendMessage(%damagedClient,0,"You took Friendly Fire from " @ Client::getName(%shooterClient) @ "!");
					}
					else
					{
						Client::sendMessage(%shooterClient,0,"You just hurt your comrade, " @ Client::getName(%damagedClient) @ " with your mine!");
						Client::sendMessage(%damagedClient,0,"You just stepped on Teamate " @ Client::getName(%shooterClient) @ "'s mine!");
					}
					%this.DamageTime = %time;
				}
			}
			%friendFire = $Server::TeamDamageScale;
		}
		else if(%type == $ImpactDamageType && Client::getTeam(%object.clLastMount) == Client::getTeam(%damagedClient)) %friendFire = $Server::TeamDamageScale;
		else %friendFire = 1.0;
		if (!Player::isDead(%this)) 
		{
			if((Client::getTeam(%damagedClient) == Client::getTeam(%shooterClient)) && $Server::TeamDamageScale == 0) return;
			%armor = Player::getArmor(%this);
			if((%vertPos == "head") && (%type == $SniperDamageType)) 
			{
				%value += (%value * 3.0);
				Client::sendMessage(%shooterClient,0,"You hit " @ Client::getName(%damagedClient) @ " in the head!");
Client::sendMessage(%damagedClient,0,"You were hit in the head by " @ Client::getName(%shooterClient) @ "'s bullet!");
			}
			//NEW FOR 40k ---LEG HITS
			else if((%vertPos == "legs") && (%type == $SniperDamageType)) 
			{
				%value += (%value * 0.001);
				Client::sendMessage(%shooterClient,0,"You hit " @ Client::getName(%damagedClient) @ " in the leg!");
				Player::incItemCount(%this, DeadWeight);
                Player::mountItem(%this, DeadWeight, 4);
                schedule("Player::decItemCount(" @ %this @ ",DeadWeight);", 7);
                Client::sendMessage(%damagedClient,0,"You were hit in the leg by " @ Client::getName(%shooterClient) @ "'s bullet!");
		}
			if (%type != -1 && %this.shieldStrength)
			{
				%energy = GameBase::getEnergy(%this);
				%strength = %this.shieldStrength;
				if (%type == $BulletDamageType) %strength *= 1;
				if (%type == $EnergyDamageType) %strength *= 1.5;
				if (%type == $PlasmaDamageType) %strength *= 1.5;
				if (%type == $ExplosionDamageType) %strength *= 1;
				if (%type == $ShrapnelDamageType) %strength *= 1;
				if (%type == $LaserDamageType) %strength *= 1.5;
				if (%type == $MortarDamageType) %strength *= 1;
				if (%type == $BlasterDamageType) %strength *= 0.5;
				if (%type == $ElectricityDamageType) %strength *= 0.5;
				if (%type == $DebrisDamageType) %strength *= 1;
				if (%type == $MissileDamageType) %strength *= 0.75;
				if (%type == $MineDamageType) %strength *= 0.75;
				if (%type == $SniperDamageType) %strength *= 0.5;
				if (%type == $FlashDamageType) GameBase::setEnergy(%this,0);
				if (%type == $ShellDamageType) %strength *= 1;
				if (%type == $MeltaDamageType) %strength *= 1.5;
				if (%type == $DDamageType) %strength *= 0.005;
				if (%type == $ReaperDamageType) %strength *= 0.25;
				if (%type == $FlamerDamageType) %strength *= 0.8;
				if (%type == $ShurikenDamageType) %strength *= 1;
				if (%type == $DeathDamageType) %strength *= 0.4;
				if (%type == $PsiDamageType) %strength *= 0.01;
				if (%type == $ChemDamageType) %strength *= 0.7;
				if (%type == $KrakenDamageType) %strength *= 0.25;
				if (%type == $AcidDamageType) %strength *= 0.5;
				%absorb = %energy * %strength;
				if (%value < %absorb)
				{
					GameBase::setEnergy(%this,%energy - ((%value / %strength) * %friendFire));
					%thisPos = getBoxCenter(%this);
					%offsetZ =((getWord(%pos,2))-(getWord(%thisPos,2)));
					GameBase::activateShield(%this,%vec,%offsetZ);
					%value = 0;
				}
				else
				{
					GameBase::setEnergy(%this,0);
					%value = %value - %absorb;
				}
			}
			if (%type != -1 && (!%this.shieldStrength || GameBase::getEnergy(%this) <= 0) || %type == $PsiDamageType || %type == $DDamageType || %type == $FlashDamageType)
// Handle long-term side effects
			{
				%armor = Player::getArmor(%this);
				if (%type == $FlashDamageType) Armor::specDamage(%damagedClient, %this, %shooterClient, $FlashDamageType, 1);
				else if (%type == $EnergyDamageType)
				{
					Armor::specDamage(%damagedClient, %this, %shooterClient, $EnergyDamageType, 1);
					if (Player::isDead(%player)) return;
				}
				else if (%type == $PlasmaDamageType)
				{
					Armor::specDamage(%damagedClient, %this, %shooterClient, $PlasmaDamageType, 1);
					if (Player::isDead(%player)) return;
				}
				else if (%type == $ChemDamageType)
				{
					Armor::specDamage(%damagedClient, %this, %shooterClient, $ChemDamageType, 1);
					if (Player::isDead(%player)) return;
				}
				else if (%type == $AcidDamageType)
				{
					Armor::specDamage(%damagedClient, %this, %shooterClient, $AcidDamageType, 1);
					if (Player::isDead(%player)) return;
				}
                        else if (%type == $WebDamageType)
				{
echo("got here, damage player.");
					Armor::specDamage(%damagedClient, %this, %shooterClient, $WebDamageType, 1);
					if (Player::isDead(%player)) return;
				}
//-=-=-=-==-=BOMB DATA
				if (%value)
				{
					%hitdamageval = 0.05;
					%hittolerance = 0.25;
					%weaponType = Player::getMountedItem(%this,$WeaponSlot);
					if((Player::getMountedItem(%this,$BackpackSlot) == LaserPack))
					{
						if(((%type == $LaserDamageType) || (%type == $SniperDamageType)) && (%quadrant == "middle_back" || %quadrant == "middle_front" || %quadrant == "middle_middle") && (Client::getTeam(%damagedClient) != Client::getTeam(%shooterClient)))
						{
							MessageAllExcept(Player::getClient(%damagedClient), 0, Client::getName(%shooterClient) @ " sniped the Adv.Energy Pack on " @ Client::getName(%damagedClient) @ "'s back!");
							Client::sendMessage(Player::getClient(%damagedClient),0,"Your Adv.Energy Pack exploded!");
							Player::unmountItem(%this,$BackpackSlot);
							%obj = newObject("","Mine","EnerPackBoom");
							addToSet("MissionCleanup", %obj);
							GameBase::throw(%obj,%this,9 * %client.throwStrength,false);
						}
					}
					if ((%vertPos == "torso") && (%quadrant == "front_right") && (%type == $LaserDamageType) && (%value > %hittolerance) && (%weaponType != -1 && %weaponType != "RepairGun"))
					{
						Player::dropItem(%this,%weaponType);
						%dlevel = GameBase::getDamageLevel(%this) + 0.05;
						Client::sendMessage(Player::getClient(%shooterClient),0, "You knocked the " @ %weaponType @ " out of " @ Client::getName(%damagedClient) @ "'s hand!");
					}
					else 
					{
						%value = $DamageScale[%armor, %type] * %value * %friendFire;
						%dlevel = GameBase::getDamageLevel(%this) + %value;
					}
					%spillOver = %dlevel - %armor.maxDamage;
					GameBase::setDamageLevel(%this,%dlevel);
					%flash = Player::getDamageFlash(%this) + %value * 2;
					if (%flash > 0.75) %flash = 0.75;
					Player::setDamageFlash(%this,%flash);
					if(!Player::isDead(%this)) 
					{
						if(%damagedClient.lastDamage < getSimTime()) 
						{
							%sound = radnomItems(3,injure1,injure2,injure3);
							playVoice(%damagedClient,%sound);
							%damagedClient.lastdamage = getSimTime() + 1.5;
						}
					}
					else 
					{
						if((%spillOver > 0.5 && (%type== $ExplosionDamageType || %type == $ShrapnelDamageType || %type== $MortarDamageType || %type == $MissileDamageType)) || %type == $ElectricityDamageType) 
						{
							Player::trigger(%this, $WeaponSlot, false);
							%weaponType = Player::getMountedItem(%this,$WeaponSlot);
							if(%weaponType != -1) Player::dropItem(%this,%weaponType);
							Player::blowUp(%this);
						}
						else 
						{
							if ((%value > 0.40 && (%type== $ExplosionDamageType || %type == $ShrapnelDamageType || %type== $MortarDamageType || %type == $MissileDamageType )) || (Player::getLastContactCount(%this) > 6) ) 
							{
								if(%quadrant == "front_left" || %quadrant == "front_right") %curDie = $PlayerAnim::DieBlownBack;
								else %curDie = $PlayerAnim::DieForward;
							}
							else if( Player::isCrouching(%this) ) %curDie = $PlayerAnim::Crouching;
							else if(%vertPos=="head") 
							{
								if(%quadrant == "front_left" || %quadrant == "front_right" ) %curDie = radnomItems(2, $PlayerAnim::DieHead, $PlayerAnim::DieBack);
								else %curDie = radnomItems(2, $PlayerAnim::DieHead, $PlayerAnim::DieForward);
							}
							else if (%vertPos == "torso") 
							{
								if(%quadrant == "front_left" ) %curDie = radnomItems(3, $PlayerAnim::DieLeftSide, $PlayerAnim::DieChest, $PlayerAnim::DieForwardKneel);
								else if(%quadrant == "front_right") %curDie = radnomItems(3, $PlayerAnim::DieChest, $PlayerAnim::DieRightSide, $PlayerAnim::DieSpin);
								else if(%quadrant == "back_left" ) %curDie = radnomItems(4, $PlayerAnim::DieLeftSide, $PlayerAnim::DieGrabBack, $PlayerAnim::DieForward, $PlayerAnim::DieForwardKneel);
								else if(%quadrant == "back_right") %curDie = radnomItems(4, $PlayerAnim::DieGrabBack, $PlayerAnim::DieRightSide, $PlayerAnim::DieForward, $PlayerAnim::DieForwardKneel);
							}
							else if (%vertPos == "legs") 
							{
								if(%quadrant == "front_left" || %quadrant == "back_left") %curDie = $PlayerAnim::DieLegLeft;
								if(%quadrant == "front_right" || %quadrant == "back_right") %curDie = $PlayerAnim::DieLegRight;
							}
							Player::setAnimation(%this, %curDie);
						}
						if(%type == $ImpactDamageType && %object.clLastMount != "") %shooterClient = %object.clLastMount;
						Client::onKilled(%damagedClient,%shooterClient, %type);
					}
				}
			}
		}
	}
	%damageLevel = GameBase::getDamageLevel(%this);
	%this.mindamage = %damageLevel;
}

function radnomItems(%num, %an0, %an1, %an2, %an3, %an4, %an5, %an6)
{
	return %an[floor(getRandom() * (%num - 0.01))];
}

function Player::onCollision(%this,%object)
{
	if (Player::isDead(%this))
	{
		if (getObjectType(%object) == "Player")
		{
			%sound = false;
			%max = $TotalItems;
			for (%i = 0; %i < %max; %i++)
			{
				%count = Player::getItemCount(%this,%i);
				if (%count)
				{
					%delta = Item::giveItem(%object,getItemData(%i),%count);
					if (%delta > 0)
					{
						Player::decItemCount(%this,%i,%delta);
						%sound = true;
					}
				}
			}
			if (%sound)
			{
				playSound(SoundPickupItem,GameBase::getPosition(%this));
			}
		}
	}
	if (getObjectType(%object) == "Player" && !Player::isDead(%this))
	{
		%cliendId = Player::getClient(%object);
		%thisId = Player::getClient(%this);
		%armor = Player::getArmor(%object);
		eval(%armor @ "::onPlayerContact(" @ %this @ ", " @ %object @ ");");
	}
}

function Player::getHeatFactor(%this)
{
	%client = Player::getClient(%this);
	if (Client::getControlObject(%client) != %this) return 1.0;
	%time = getIntegerTime(true) >> 5;
	%lastTime = Player::lastJetTime(%this) >> 10;
	if ((%lastTime + 1.5) < %time)
	{
		return 0.0;
	}
	else
	{
		%diff = %time - %lastTime;
		%heat = 1.0 - (%diff / 1.5);
		return %heat;
	}
}

function Player::jump(%this,%mom) 
{
	%cl = GameBase::getControlClient(%this);
	if (%cl != -1) 
	{
		%vehicle = Player::getMountObject(%this);
		%this.lastMount = %vehicle;
		%this.newMountTime = getSimTime() + 3.0;
		Player::setMountObject(%this, %vehicle, 0);
		Player::setMountObject(%this, -1, 0);
		Player::applyImpulse(%pl,%mom);
		playSound (GameBase::getDataName(%this).dismountSound, GameBase::getPosition(%this));
	}
}

function remoteKill(%client) 
{
	%player = Client::getOwnedObject(%client);
	if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player)) 
	{
		if(Player::getMountedItem(%player,$BackpackSlot) == SuicidePack) 
		{
			Player::unmountItem(%player,$BackpackSlot);
			%obj = newObject("","Mine","Suicidebomb");
			addToSet("MissionCleanup", %obj);
			%client = Player::getClient(%player);
			GameBase::throw(%obj,%player,9 * %client.throwStrength,false);
		}
		else 
		{
			playNextAnim(%client);
			Player::kill(%client);
			Client::onKilled(%client,%client);
		}
	}
}

function RemotePlayFakeDeath(%client)
{
	%anim = floor(getRandom() * ($PlayerAnim::LastDeath - $PlayerAnim::FirstDeath));
	Player::setAnimation(%client, $PlayerAnim::FirstDeath + %anim);
}

$animNumber = 25;

function playNextAnim(%client) 
{
	if($animNumber > 36) $animNumber = 25;
	Player::setAnimation(%client,$animNumber++);
}

function Client::takeControl(%clientId, %objectId) 
{
	%pl = Client::getOwnedObject(%clientId);
	if (%objectId == -1 || GameBase::getTeam(%objectId) != Client::getTeam(%clientId) || GameBase::getControlClient(%objectId) != -1 || GameBase::getDamageState(%objectId) != "Enabled" || %pl.driver != "" || %pl.vehicleSlot != "") return;
	Turret::onAttemptControl(%objectId, %clientId);
}

function remoteCmdrMountObject(%clientId, %objectIdx) 
{
	Client::takeControl(%clientId, getObjectByTargetIndex(%objectIdx));
}

function checkControlUnmount(%clientId) 
{
	%ownedObject = Client::getOwnedObject(%clientId);
	%ctrlObject = Client::getControlObject(%clientId);
	if(%ownedObject != %ctrlObject) 
	{
		if (%ownedObject == -1 || %ctrlObject == -1 || (getObjectType(%ownedObject) == "Player" && Player::getMountObject(%ownedObject) == %ctrlObject)) return;
		Client::setControlObject(%clientId, %ownedObject);
	}
	GameBase::virtual(%ctrlObject, onDismount, %ctrlObject, %clientID);
	} 
