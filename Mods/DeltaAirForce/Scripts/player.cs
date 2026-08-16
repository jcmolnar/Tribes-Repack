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

// Font <f0> = IDFNT_FONTIS_3      normal green   
// Font <f1> = IDFNT_FONTIS_2      pale yellow
// Font <f2> = IDFNT_FONTIS_1      bright yellow
// Font <f3> = IDFNT_FONTIS_4      bright green
// Font <f4> = IDFNT_FONTIS_5      gray
// Font <f5> = IDFNT_FONTIS_6      red
// Font <f6> = IDFNT_FONTIS_3I     normal italic

//----------------------------------------------------------------------------
$CorpseTimeoutValue = 22;
//----------------------------------------------------------------------------

// Player & Armor data block callbacks

function Player::onAdd(%this)
{
	GameBase::setRechargeRate(%this,8);
}

function Player::onRemove(%this)
{
	// Drop anything left at the players pos
	for (%i = 0; %i < 8; %i = %i + 1) {
		%type = Player::getMountedItem(%this,%i);
		if (%type != -1) {
			// Note: Player::dropItem ius not called here.
			%item = newObject("","Item",%type,1,false);
         schedule("Item::Pop(" @ %item @ ");", $ItemPopTime, %item);

         addToSet("MissionCleanup", %item);
			GameBase::setPosition(%item,GameBase::getPosition(%this));
		}
	}
}

function Player::onNoAmmo(%player,%imageSlot,%itemType)
{
	%clip = $Clip[%itemType];
	if(%clip != "") {
		%clipCount = Player::getItemCount(%player,%clip);
		if(%clipCount > 0)
			Player::useItem(%player,%clip);
		else
			bottomprint(Player::getClient(%player), "<jc><f0>Out of Clips!!!", 2);
	}
}

function Player::onKilled(%this)
{
	if( %this.TurretControl && %this.TurretControl != -1 && %this.TurretControl != 0 ) 
	{
		DecomissionTurret(%this.TurretControl);
	}
	%cl = GameBase::getOwnerClient(%this);
	%cl.dead = 1;
	if($AutoRespawn > 0)
		schedule("Game::autoRespawn(" @ %cl @ ");",$AutoRespawn,%cl);
	if(%this.outArea==1)	
		leaveMissionAreaDamage(%cl);
	Player::setDamageFlash(%this,0.75);
	for (%i = 0; %i < 8; %i = %i + 1) {
		%type = Player::getMountedItem(%this,%i);
		if (%type != -1) {
			if (%i != $WeaponSlot || !Player::isTriggered(%this,%i) || getRandom() > "0.2") 
				Player::dropItem(%this,%type);
		}
	}
	// DELTAFORCE
	%team = GameBase::getTeam(%cl);
	//echo("Hostage freed!");
	if($Game::missionType == "Hostage" || $hostageGame == 1 || $Game::missionType == "Jail") {
		for(%x = 0; %x < 3; %x++) {
			if($Hostages[%cl, %x] != 0) {
				//echo("I'm called! Yay!");
				GameBase::setTeam($Hostages[%cl, %x], getNumTeams() - 1);
				$hostageTeam[$Hostages[%cl, %x]] = getNumTeams() - 1;
				AI::DirectiveFollow(Client::getName($Hostages[%cl, %x]), getNearestAITeammate($Hostages[%cl, %x]), 0, 1024);
				messageAll(1, Client::getName(%cl) @ " lost " @ Client::getName($Hostages[%cl, %x]) @ "!");
				//AI::DirectiveWaypoint(Client::getName($Hostages[%cl, %x]), $AISpawn[$Hostages[%cl, %x]] , 0);
				$HostageOwner[$Hostages[%cl, %x]] = -1;
				$Hostages[%cl, %x] = 0;
				if( $Game::missionType != "Jail")
				if($deltaTeamScore[%team] >= 5)
					$deltaTeamScore[%team] -= 5;
				$TakenHostages[%cl]--;
			}
		}
	}

	// Sniper limitation code
	if(Player::getArmor(%this) == "sarmor" || Player::getArmor(%this) == "sfemale" ||Player::getArmor(%this) == "sarmor2" || Player::getArmor(%this) == "sfemale2" || Player::getArmor(%this) == "sarmor3" || Player::getArmor(%this) == "sfemale3")
		$SniperCount[%team]--;

	if(Player::getItemCount(%this, FuelPack) == 1) {
		%trans = GameBase::getMuzzleTransform(%cl);
		Projectile::spawnProjectile("FlameExplosion", %trans, %cl, Item::getVelocity(%cl));
	}
	$PlayerBurning[%cl] = 0;
	$PlayerBleeding[%cl] = 0;
	$woundedWeaponArm[%cl] = 0;
	$woundedLegs[%cl] = 0;
	%cl.inVehicle = false;

	if($Game::missionType == "Assassination") {
		if(%cl == $Hunted) {
			if($HuntedEscaped != 1) {
				//%killerName = Client::getName(%killerId);
				for(%i = Client::getFirst(); %i != -1; %i = Client::getNext(%i)) {
					remoteEval(%i, CP, "<jc><f0>The VIP was assassinated!", 4);
					Player::kill(%i);
				}
				$teamScore[1] += 1;
			}
		}
	}
	// END DELTAFORCE

   if(%cl != -1)
   {
		if(%this.vehicle != "")	{
			if(%this.driver != "") {
				%this.driver = "";
        	 	Client::setControlObject(Player::getClient(%this), %this);
        	 	Player::setMountObject(%this, -1, 0);
			}
			else {
				%this.vehicle.Seat[%this.vehicleSlot-2] = "";
				%this.vehicleSlot = "";
			}
			%this.vehicle = "";		
		}
      schedule("GameBase::startFadeOut(" @ %this @ ");", $CorpseTimeoutValue, %this);
      Client::setOwnedObject(%cl, -1);
      Client::setControlObject(%cl, Client::getObserverCamera(%cl));
      Observer::setOrbitObject(%cl, %this, 5, 5, 5);
      schedule("deleteObject(" @ %this @ ");", $CorpseTimeoutValue + 2.5, %this);
      %cl.observerMode = "dead";
      %cl.dieTime = getSimTime();
   }
   
   if( $Game::missionType == "Jail")
	if( %team == 1 )
		$teamScore[0]++;

}

// DELTAFORCE
function BurnTick(%this, %object) {
	if($PlayerBurning[Player::getClient(%this)] == 1) {
		GameBase::applyDamage(%this, $PlasmaDamageType, 0.1, GameBase::getPosition(%this), "0 0 0", "0 0 0", %object);
		schedule("BurnTick("@%this@", "@%object@");", 1, %this);
	}
}

function BleedTick(%this, %object) {
	if($PlayerBleeding[Player::getClient(%this)] == 1) {
		GameBase::applyDamage(%this, $BleedDamageType, 0.005, GameBase::getPosition(%this), "0 0 0", "0 0 0", %this);
		//remoteEval(Player::getClient(%this), BP, "<f5><jc>You are BLEEDING to death! Seekest thou a medic!!", 3);
		bottomprint(Player::getClient(%this), "<f5><jc>You are BLEEDING to death! Seekest thou a medic!!", 3);
		schedule("BleedTick("@%this@", "@%object@");", 2, %this);		
	}
}

function PoisonTick(%this, %object) {
	if($PlayerPoisoned[Player::getClient(%this)] == 1) {
		GameBase::applyDamage(%this, $PoisonDamageType, 0.05, GameBase::getPosition(%this), "0 0 0", "0 0 0", %this);
		//remoteEval(Player::getClient(%this), BP, "<f5><jc>GAH! Covered with Nerve Gas! Your flippin out!", 3);
		bottomprint(Player::getClient(%this), "<f5><jc>GAH! Covered with Nerve Gas! Your flippin out!", 3);
		schedule("PoisonTick("@%this@", "@%object@");", 1, %this);		
	}
}
// END DELTAFORCE


function Player::onDamage(%this,%type,%value,%pos,%vec,%mom,%vertPos,%quadrant,%object)
{
	if (Player::isExposed(%this)) {
		%damagedClient = Player::getClient(%this);
		%shooterClient = %object;

		Player::applyImpulse(%this,%mom);
		if($teamplay && %damagedClient != %shooterClient && Client::getTeam(%damagedClient) == Client::getTeam(%shooterClient) ) {
			if (%shooterClient != -1) {
				%curTime = getSimTime();
				if ((%curTime - %this.DamageTime > 3.5 || %this.LastHarm != %shooterClient) && %damagedClient != %shooterClient && $Server::TeamDamageScale > 0) {
					//if(%type != $MineDamageType) {
						Client::sendMessage(%shooterClient,0,"You just harmed Teammate " @ Client::getName(%damagedClient) @ "!");
						Client::sendMessage(%damagedClient,0,"You took Friendly Fire from " @ Client::getName(%shooterClient) @ "!");
					//} else {
					//	Client::sendMessage(%shooterClient,0,"You just harmed Teammate " @ Client::getName(%damagedClient) @ " with your mine!");
					//	Client::sendMessage(%damagedClient,0,"You just stepped on Teamate " @ Client::getName(%shooterClient) @ "'s mine!");
					//}
					%this.LastHarm = %shooterClient;
					%this.DamageStamp = %curTime;
				}
			}
			%friendFire = $Server::TeamDamageScale / 100;
		} else if(%type == $ImpactDamageType && Client::getTeam(%object.clLastMount) == Client::getTeam(%damagedClient)) 
			%friendFire = $Server::TeamDamageScale / 100;
		else  
			%friendFire = 1.0;	

		// DELTAFORCE
		if(%type == $PlasmaDamageType) {
			if($PlayerBurning[%damagedClient] == 0) {
				$PlayerBurning[%damagedClient] = 1;
				Client::sendMessage(%damagedClient,1,"You are on fire!");
				BurnTick(%this, %object);
				schedule("$PlayerBurning["@%damagedClient@"] = 0;", 5);
				schedule("Client::sendMessage("@%damagedClient@",1,\"You stop burning.\");", 5);
			}
		}
		
		if(%type == $PoisonDamageType) {
			if($PlayerPoisoned[%damagedClient] == 0) {
				$PlayerPoisoned[%damagedClient] = 1;
				Client::sendMessage(%damagedClient,1,"Nerve Gas! All around you! AHH!!");
				schedule("PoisonTick("@%this@", "@%object@");", 1, %this);
				schedule("$PlayerPoisoned["@%damagedClient@"] = 0;", 20);
				schedule("Client::sendMessage("@%damagedClient@",1,\"Your cured!.\");", 20);
			}
		}
		if(%type == $SmokeDamageType) {
			
			Client::sendMessage(%damagedClient,1,"Smoke in your eyes and lungs!");
			Player::setDamageFlash(%damagedClient, 5.0);
			//schedule("Player::setDamageFlash("@%damagedClient@",0.0);", 5.0, %damagedClient);
			schedule("Client::sendMessage("@%damagedClient@",1,\"**Cough Cough**!.\");", 2);
		}
		
		// END DELTAFORCE

		if (!Player::isDead(%this) && ($Server::teamDamageScale !=0 || (Client::getTeam(%damagedClient) != Client::getTeam(%shooterClient) || %damagedClient == %shooterClient))) {
			%armor = Player::getArmor(%this);
			//More damage applyed to head shots
			// DELTAFORCE
			
			//Set up damage amounts and wound effects for each body position
			if(%vertPos == "head" && (%type == $BulletDamageType || %type == $ShrapnelDamageType || %type == $LaserDamageType)) {
				if(%type == $StabDamageType){
					bottomprint(%shooterClient, "<f5><jc>Head Shot!", 3);
					bottomprint(%damagedClient, "<f5><jc>You've been stabbed in the head!", 3);
				}else{
					bottomprint(%shooterClient, "<f5><jc>Head Shot!", 3);
					bottomprint(%damagedClient, "<f5><jc>You've been shot in the head!", 3);
				}
				if(%armor == "harmor") { 
					if(%quadrant == "middle_back" || %quadrant == "middle_front" || %quadrant == "middle_middle") {
						//%value += (%value * 0.3);
						// A head shot with *ANYTHING* kills instantly
						%value *= 100;
					}
				} else {
					// A head shot with *ANYTHING* kills instantly
					%value *= 100;
				}
			} else if((%vertPos == "torso") && (Player::getItemCount(%this, FuelPack) > 0)) {
				if((%quadrant == "back_left") || (%quadrant == "back_right") || (%quadrant == "back_middle")) {
					Player::blowUp(%this);
					GameBase::playSound(%this, shockExplosion, 3);
					Player::setItemCount(%this, FuelPack, 0);
					%value *= 100;
					%pos = GameBase::getPosition(%this);
					%pos1 = Vector::Add(%pos,"0 0 "@$FuelPackBlowHeight);
					%vel = "0 0 0";
					//-Ice's Fuel Pack Explosion Method
					if($FuelPackBlowMethod == "" || $FuelPackBlowMethod == 1) {
						%rot1  = GameBase::getRotation(%this);                                  	   //- rotation aim forward
						%rot2  = Vector::Add(GameBase::getRotation(%this),"0 0 3.141592654");   	   //- rotation aim back
						%rot3  = Vector::Add(GameBase::getRotation(%this),"0 0 -1.570796327");  	   //- rotation aim left
						%rot4  = Vector::Add(GameBase::getRotation(%this),"0 0 1.570796327");   	   //- rotation aim right
						%rot5  = Vector::Add(GameBase::getRotation(%this),"1.570796327 0 0");   	   //- rotation aim up
						%rot6  = Vector::Add(GameBase::getRotation(%this),"-1.570796327 0 0");  	   //- rotation aim down
						%rot7  = Vector::Add(GameBase::getRotation(%this),"-0.7853981635 0 0");		   //- rotation aim down/forward
						%rot8  = Vector::Add(GameBase::getRotation(%this),"0.7853981635 0 0");  	   //- rotation aim up/forward
						%rot9  = Vector::Add(GameBase::getRotation(%this),"-3.9269908175 0 0");            //- rotation aim down/back
						%rot10 = Vector::Add(GameBase::getRotation(%this),"3.9269908175 0 0");             //- rotation aim up/back
						%rot11 = Vector::Add(GameBase::getRotation(%this),"0 0 -0.7853981635");            //- rotation aim left/forward
						%rot12 = Vector::Add(GameBase::getRotation(%this),"0 0 0.7853981635");             //- rotation aim right/forward
						%rot13 = Vector::Add(GameBase::getRotation(%this),"0 0 -3.9269908175");            //- rotation aim left/back
						%rot14 = Vector::Add(GameBase::getRotation(%this),"0 0 3.9269908175");             //- rotation aim right/back
						%rot15 = Vector::Add(GameBase::getRotation(%this),"1.570796327 0.7853981635 0");   //- rotation aim up/right
						%rot16 = Vector::Add(GameBase::getRotation(%this),"-1.570796327 0.7853981635 0");  //- rotation aim down/right
						%rot17 = Vector::Add(GameBase::getRotation(%this),"1.570796327 -0.7853981635 0");  //- rotation aim up/left
						%rot18 = Vector::Add(GameBase::getRotation(%this),"-1.570796327 -0.7853981635 0"); //- rotation aim down/left
	
						%vec[1]  = Vector::getFromRot(%rot1,$FuelPackBlowRadius);
						%vec[2]  = Vector::getFromRot(%rot2,$FuelPackBlowRadius);
						%vec[3]  = Vector::getFromRot(%rot3,$FuelPackBlowRadius);
						%vec[4]  = Vector::getFromRot(%rot4,$FuelPackBlowRadius);
						%vec[5]  = Vector::getFromRot(%rot5,$FuelPackBlowRadius);
						%vec[6]  = Vector::getFromRot(%rot6,$FuelPackBlowRadius);
						%vec[7]  = Vector::getFromRot(%rot7,$FuelPackBlowRadius);
						%vec[8]  = Vector::getFromRot(%rot8,$FuelPackBlowRadius);
						%vec[9]  = Vector::getFromRot(%rot9,$FuelPackBlowRadius);
						%vec[10] = Vector::getFromRot(%rot10,$FuelPackBlowRadius);
						%vec[11] = Vector::getFromRot(%rot11,$FuelPackBlowRadius);
						%vec[12] = Vector::getFromRot(%rot12,$FuelPackBlowRadius);
						%vec[13] = Vector::getFromRot(%rot13,$FuelPackBlowRadius);
						%vec[14] = Vector::getFromRot(%rot14,$FuelPackBlowRadius);
						%vec[15] = Vector::getFromRot(%rot15,$FuelPackBlowRadius);
						%vec[16] = Vector::getFromRot(%rot16,$FuelPackBlowRadius);
						%vec[17] = Vector::getFromRot(%rot17,$FuelPackBlowRadius);
						%vec[18] = Vector::getFromRot(%rot18,$FuelPackBlowRadius);
					
						%pos[19] = Vector::add(%pos1,%vec[1]);
						%pos[20] = Vector::add(%pos1,%vec[2]);
						%pos[21] = Vector::add(%pos1,%vec[3]);
						%pos[22] = Vector::add(%pos1,%vec[4]);
						%pos[23] = Vector::add(%pos1,%vec[5]);
						%pos[24] = Vector::add(%pos1,%vec[6]);
						%pos[25] = Vector::add(%pos1,%vec[7]);
						%pos[26] = Vector::add(%pos1,%vec[8]);
						%pos[27] = Vector::add(%pos1,%vec[9]);
						%pos[28] = Vector::add(%pos1,%vec[10]);
						%pos[29] = Vector::add(%pos1,%vec[11]);
						%pos[30] = Vector::add(%pos1,%vec[12]);
						%pos[31] = Vector::add(%pos1,%vec[13]);
						%pos[32] = Vector::add(%pos1,%vec[14]);
						%pos[33] = Vector::add(%pos1,%vec[15]);
						%pos[34] = Vector::add(%pos1,%vec[16]);
						%pos[35] = Vector::add(%pos1,%vec[17]);
						%pos[36] = Vector::add(%pos1,%vec[18]);
			
						%trans = %rot1@" "@%vec[1]@" "@%rot1@" "@%pos[19];
						projectile::spawnProjectile(FlameExplosionThree,%trans,%this,%vel);
						for(%i = 1; %i <= 18; %i++) {
							%trans = %rot1@" "@%vec[%i]@" "@%rot1@" "@%pos[%i+18];
							if($FuelPackBlowType == 1 || $FuelPackBlowType == "")
								schedule("Projectile::spawnProjectile(FlameExplosionTwo,\""@%trans@"\",\""@%this@"\",\""@%vel@"\");",%i/18);
							else
								projectile::spawnProjectile(FlameExplosionTwo,%trans,%this,%vel);
						}
					}
					else if($FuelPackBlowMethod == 2) {
						%trans = "0 0 0 0 0 0 0 0 0 " @ %pos;
							
						%transOne = "0 0 0 0 0 0 0 0 0 " @ getWord(%pos, 0) @ " " @ getWord(%pos, 1) @ " " @ getWord(%pos, 2) + 1;
						%transTwo = "0 0 0 0 0 0 0 0 0 " @ getWord(%pos, 0) @ " " @ getWord(%pos, 1) @ " " @ getWord(%pos, 2) + 1.5;
							
						%transI = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) + 3 @ " " @ getWord(%pos, 1) @ " " @ getWord(%pos, 2);
						%transII = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) @ " " @ getWord(%pos, 1) + 3 @ " " @ getWord(%pos, 2);
						%transIII = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) @ " " @ getWord(%pos, 1) @ " " @ getWord(%pos, 2) + 3;
						%transIV = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) + 3 @ " " @ getWord(%pos, 1) + 3 @ " " @ getWord(%pos, 2) + 3;
						%transV = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) @ " " @ getWord(%pos, 1) + 3 @ " " @ getWord(%pos, 2) + 3;
						%transVI = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) + 3 @ " " @ getWord(%pos, 1) @ " " @ getWord(%pos, 2) + 3;
						%transVII = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) + 3 @ " " @ getWord(%pos, 1) + 3 @ " " @ getWord(%pos, 2);
	
						%transVIII = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) - 3 @ " " @ getWord(%pos, 1) @ " " @ getWord(%pos, 2);
						%transIX = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) @ " " @ getWord(%pos, 1) - 3 @ " " @ getWord(%pos, 2);
						%transX = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) @ " " @ getWord(%pos, 1) @ " " @ getWord(%pos, 2) - 3;
						%transXI = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) - 3 @ " " @ getWord(%pos, 1) - 3 @ " " @ getWord(%pos, 2) - 3;
						%transXII = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) - 3 @ " " @ getWord(%pos, 1) - 3 @ " " @ getWord(%pos, 2);
						%transXIII = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) - 3 @ " " @ getWord(%pos, 1) @ " " @ getWord(%pos, 2) - 3;
						%transXIV = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) @ " " @ getWord(%pos, 1) - 3 @ " " @ getWord(%pos, 2) - 3;
		
						%transXV = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) + 3 @ " " @ getWord(%pos, 1) - 3 @ " " @ getWord(%pos, 2);
						%transXVI = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) - 3 @ " " @ getWord(%pos, 1) + 3 @ " " @ getWord(%pos, 2);
						%transXVII = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) @ " " @ getWord(%pos, 1) - 3 @ " " @ getWord(%pos, 2) + 3;
						%transXVIII = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) @ " " @ getWord(%pos, 1) + 3 @ " " @ getWord(%pos, 2) - 3;
						%transXIX = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) + 3 @ " " @ getWord(%pos, 1) @ " " @ getWord(%pos, 2) - 3;
						%transXX = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) - 3 @ " " @ getWord(%pos, 1) @ " " @ getWord(%pos, 2) + 3;
		
						%transXXI = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) + 3 @ " " @ getWord(%pos, 1) + 3 @ " " @ getWord(%pos, 2) - 3;
						%transXXII = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) + 3 @ " " @ getWord(%pos, 1) - 3 @ " " @ getWord(%pos, 2) + 3;
						%transXXIII = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) - 3 @ " " @ getWord(%pos, 1) + 3 @ " " @ getWord(%pos, 2) + 3;
						%transXXIV = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) - 3 @ " " @ getWord(%pos, 1) - 3 @ " " @ getWord(%pos, 2) + 3;
						%transXXV = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) + 3 @ " " @ getWord(%pos, 1) - 3 @ " " @ getWord(%pos, 2) - 3;
						%transXXVI = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) - 3 @ " " @ getWord(%pos, 1) + 3 @ " " @ getWord(%pos, 2) - 3;
		
						%transXXVII = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) - 3 @ " " @ getWord(%pos, 1) @ " " @ getWord(%pos, 2);
						%transXXVIII = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) @ " " @ getWord(%pos, 1) - 3 @ " " @ getWord(%pos, 2);
						%transXXIX = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) @ " " @ getWord(%pos, 1) @ " " @ getWord(%pos, 2) - 3;
						%transXxX = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) - 3 @ " " @ getWord(%pos, 1) - 3 @ " " @ getWord(%pos, 2) - 3;
						%transXXXI = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) - 3 @ " " @ getWord(%pos, 1) - 3 @ " " @ getWord(%pos, 2);
						%transXXXII = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) - 3 @ " " @ getWord(%pos, 1) @ " " @ getWord(%pos, 2) - 3;
						%transXXXIII = "0 0 0 0 0 0 0 0 0" @ getWord(%pos, 0) @ " " @ getWord(%pos, 1) - 3 @ " " @ getWord(%pos, 2) - 3;
						
						schedule("GameBase::applyRadiusDamage(3, " @ %pos @ ", 5, 1, 30, " @ %object @ ");", 0.1);
						Projectile::spawnProjectile("FlameExplosionTwo", %transOne, %this, "0 0 0");
						Projectile::spawnProjectile("FlameExplosionThree", %transTwo, %this, "0 0 0");
	
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transI 		@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.3);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transVIII 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.3);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXV 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.3);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXXI 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.3);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXXVII 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.3);
						
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transII 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.4);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transIX 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.4);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXVI 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.4);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXXII 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.4);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXXVIII 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.4);
						
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transIII 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.5);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transX 		@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.5);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXVII 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.5);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXXIII	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.5);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXXIX 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.5);
						
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transIV 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.6);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXI 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.6);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXVIII 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.6);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXXIV 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.6);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXXX	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.6);
						
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transV 		@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.7);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXII 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.7);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXIX	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.7);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXXV 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.7);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXXXI 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.7);
						
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transVI 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.8);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXIII 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.8);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXX 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.8);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXXVI 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.8);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXXXII 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.8);
						
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transVII 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.9);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXIV 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.9);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXXI 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.9);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXXVII 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.9);
						schedule("Projectile::spawnProjectile(FlameExplosionTwo, \"" @ %transXXXIII 	@ "\", \"" @ %this @ "\", \"" @ %vel @ "\");", 0.9);
					}
				}
			} else if((%type == $BulletDamageType  || %type == $ShrapnelDamageType || %type == $LaserDamageType) && %vertPos == "torso" && (%quadrant == "front_right" || %quadrant == "back_right")) {
				if($woundedWeaponArm[%damagedClient] != 1){
					%weaponType = Player::getMountedItem(%this,$WeaponSlot);
					Player::trigger(%this, $WeaponSlot, false);	
					if(%weaponType == "OICW") {
						Player::dropItem(%this, "OICW");
					} else if(%weaponType != -1 && %weaponType != "RepairGun" && %weaponType != "MedicGun") {
						Player::dropItem(%this,%weaponType);
	       		   		} else if (%weaponType == "MedicGun") {
						Player::unmountItem(%this,$WeaponSlot);
					} else if (%weaponType == "RepairGun") {
						Player::unmountItem(%this,$WeaponSlot);
					}
					$woundedWeaponArm[%damagedClient] = 1;
					//remoteEval(%damagedClient, BP, "<f5><jc>You've been shot in the arm and cannot hold a weapon!", 3);
					bottomprint(%damagedClient, "<f5><jc>You've been shot in the arm and cannot hold a weapon!", 3);
				}
				$PlayerBleeding[Player::getClient(%this)] = 1;
				schedule("BleedTick("@%this@", "@%object@");", 3, %this);
				%value = %value*0.66666666;
			} else if((%type == $BulletDamageType || %type == $ShrapnelDamageType || %type == $LaserDamageType) && %vertPos == "legs") {
				if($woundedLegs[%damagedClient] < 1){
					$woundedLegs[%damagedClient] = 1;
					//change armor to half speed armor
					//remoteEval(%damagedClient, BP, "<f5><jc>You've been shot in the leg and can no longer move as fast!", 3);
					bottomprint(%damagedClient, "<f5><jc>You've been shot in the leg and can no longer move as fast!", 3);
					if(Player::getArmor(%damagedClient) == "marmor")
						Player::setArmor(%damagedClient,marmor2);
   					else if(Player::getArmor(%damagedClient) == "sarmor")
						Player::setArmor(%damagedClient,sarmor2);
					else if(Player::getArmor(%damagedClient) == "iarmor")
						Player::setArmor(%damagedClient,iarmor2);
					else if(Player::getArmor(%damagedClient) == "garmor")
						Player::setArmor(%damagedClient,garmor2);
					else if(Player::getArmor(%damagedClient) == "carmor")
						Player::setArmor(%damagedClient,carmor2);
					else if(Player::getArmor(%damagedClient) == "earmor")
						Player::setArmor(%damagedClient,earmor2);
					else if(Player::getArmor(%damagedClient) == "larmor")
						Player::setArmor(%damagedClient,larmor2);
					else if(Player::getArmor(%damagedClient) == "aarmor")
						Player::setArmor(%damagedClient,aarmor2);
					else if(Player::getArmor(%damagedClient) == "mfemale")
						Player::setArmor(%damagedClient,mfemale2);
					else if(Player::getArmor(%damagedClient) == "sfemale")
						Player::setArmor(%damagedClient,sfemale2);
					else if(Player::getArmor(%damagedClient) == "ifemale")
						Player::setArmor(%damagedClient,ifemale2);
					else if(Player::getArmor(%damagedClient) == "gfemale")
						Player::setArmor(%damagedClient,gfemale2);
					else if(Player::getArmor(%damagedClient) == "cfemale")
						Player::setArmor(%damagedClient,cfemale2);
					else if(Player::getArmor(%damagedClient) == "efemale")
						Player::setArmor(%damagedClient,efemale2);
					else if(Player::getArmor(%damagedClient) == "lfemale")
						Player::setArmor(%damagedClient,lfemale2);
					else if(Player::getArmor(%damagedClient) == "afemale")
						Player::setArmor(%damagedClient,afemale2);
					armorChange(%damagedClient);
					%value = 0;
	
				} else if($woundedLegs[%damagedClient] < 2){
					$woundedLegs[%damagedClient] = 2;
					//change armor to no speed armor
					//remoteEval(%damagedClient, BP, "<f5><jc>You've been shot in both your legs and cannot move at all!", 3);
					bottomprint(%damagedClient, "<f5><jc>You've been shot in both your legs and are reduced to CRAWLING!", 3);
					if(Player::getArmor(%damagedClient) == "marmor2")
						Player::setArmor(%damagedClient,marmor3);
   					else if(Player::getArmor(%damagedClient) == "sarmor2")
						Player::setArmor(%damagedClient,sarmor3);
					else if(Player::getArmor(%damagedClient) == "iarmor2")
						Player::setArmor(%damagedClient,iarmor3);
					else if(Player::getArmor(%damagedClient) == "garmor2")
						Player::setArmor(%damagedClient,garmor3);
					else if(Player::getArmor(%damagedClient) == "carmor2")
						Player::setArmor(%damagedClient,carmor3);
					else if(Player::getArmor(%damagedClient) == "earmor2")
						Player::setArmor(%damagedClient,earmor3);
					else if(Player::getArmor(%damagedClient) == "larmor2")
						Player::setArmor(%damagedClient,larmor3);
					else if(Player::getArmor(%damagedClient) == "aarmor2")
						Player::setArmor(%damagedClient,aarmor3);
					else if(Player::getArmor(%damagedClient) == "mfemale2")
						Player::setArmor(%damagedClient,mfemale3);
					else if(Player::getArmor(%damagedClient) == "sfemale2")
						Player::setArmor(%damagedClient,sfemale3);
					else if(Player::getArmor(%damagedClient) == "ifemale2")
						Player::setArmor(%damagedClient,ifemale3);
					else if(Player::getArmor(%damagedClient) == "gfemale2")
						Player::setArmor(%damagedClient,gfemale3);
					else if(Player::getArmor(%damagedClient) == "cfemale2")
						Player::setArmor(%damagedClient,cfemale3);
					else if(Player::getArmor(%damagedClient) == "efemale2")
						Player::setArmor(%damagedClient,efemale3);
					else if(Player::getArmor(%damagedClient) == "lfemale2")
						Player::setArmor(%damagedClient,lfemale3);
					else if(Player::getArmor(%damagedClient) == "afemale2")
						Player::setArmor(%damagedClient,afemale3);
					armorChange(%damagedClient);
					%value = 0;
				}
				$PlayerBleeding[Player::getClient(%this)] = 1;
				schedule("BleedTick("@%this@", "@%object@");", 3, %this);
			} else if((%type == $BulletDamageType || %type == $ShrapnelDamageType || %type == $LaserDamageType) && %vertPos == "torso" && (%quadrant == "front_left" || %quadrant == "back_left")) {
				
				$PlayerBleeding[Player::getClient(%this)] = 1;
				schedule("BleedTick("@%this@", "@%object@");", 3, %this);
				
			} 
			

			// END DELTAFORCE
			//If Shield Pack is on
			if (%type != -1 && %this.shieldStrength) {
				%energy = GameBase::getEnergy(%this);
				%strength = %this.shieldStrength;
				if (%type == $ShrapnelDamageType || %type == $MortarDamageType)
					%strength *= 0.75;
				%absorb = %energy * %strength;
				if (%value < %absorb) {
					GameBase::setEnergy(%this,%energy - ((%value / %strength)*%friendFire));
					%thisPos = getBoxCenter(%this);
					%offsetZ =((getWord(%pos,2))-(getWord(%thisPos,2)));
					GameBase::activateShield(%this,%vec,%offsetZ);
					%value = 0;
				}
				else {
					GameBase::setEnergy(%this,0);
					%value = %value - %absorb;
				}
			}
  			if (%value) {
				%value = $DamageScale[%armor, %type] * %value * %friendFire;
          			%dlevel = GameBase::getDamageLevel(%this) + %value;
          			%spillOver = %dlevel - %armor.maxDamage;
				GameBase::setDamageLevel(%this,%dlevel);
				%flash = Player::getDamageFlash(%this) + %value * 2;
				if (%flash > 0.75) 
					%flash = 0.75;
				Player::setDamageFlash(%this,%flash);
				//If player not dead then play a random hurt sound
				if(!Player::isDead(%this)) { 
					if(%damagedClient.lastDamage < getSimTime()) {
						%sound = radnomItems(3,injure1,injure2,injure3);
						playVoice(%damagedClient,%sound);
						%damagedClient.lastdamage = getSimTime() + 1.5;
					}
				}
				else {
             				if(%spillOver > 0.5 && (%type== $ExplosionDamageType || %type == $GrenadeDamageType || %type== $MortarDamageType || %type == $MissileDamageType)) {
		 				Player::trigger(%this, $WeaponSlot, false);
						%weaponType = Player::getMountedItem(%this,$WeaponSlot);
						if(%weaponType != -1)
							Player::dropItem(%this,%weaponType);
                				Player::blowUp(%this);
					} else if(%spillOver > 0.5 && %type == $PlasmaDamageType ) {
						deleteObject(%this);	 
					
					} else {
						if ((%value > 0.40 && (%type== $ExplosionDamageType || %type == $GrenadeDamageType || %type== $MortarDamageType || %type == $MissileDamageType )) || (Player::getLastContactCount(%this) > 6) ) {
					  		if(%quadrant == "front_left" || %quadrant == "front_right") 
								%curDie = $PlayerAnim::DieBlownBack;
							else
								%curDie = $PlayerAnim::DieForward;
						}
						else if( Player::isCrouching(%this) ) 
							%curDie = $PlayerAnim::Crouching;							
						else if(%vertPos=="head") {
							if(%quadrant == "front_left" ||	%quadrant == "front_right"	) 
								%curDie = radnomItems(2, $PlayerAnim::DieHead, $PlayerAnim::DieBack);
						  	else 
								%curDie = radnomItems(2, $PlayerAnim::DieHead, $PlayerAnim::DieForward);
						}
						else if (%vertPos == "torso") {
							if(%quadrant == "front_left" ) 
								%curDie = radnomItems(3, $PlayerAnim::DieLeftSide, $PlayerAnim::DieChest, $PlayerAnim::DieForwardKneel);
							else if(%quadrant == "front_right") 
								%curDie = radnomItems(3, $PlayerAnim::DieChest, $PlayerAnim::DieRightSide, $PlayerAnim::DieSpin);
							else if(%quadrant == "back_left" ) 
								%curDie = radnomItems(4, $PlayerAnim::DieLeftSide, $PlayerAnim::DieGrabBack, $PlayerAnim::DieForward, $PlayerAnim::DieForwardKneel);
							else if(%quadrant == "back_right") 
								%curDie = radnomItems(4, $PlayerAnim::DieGrabBack, $PlayerAnim::DieRightSide, $PlayerAnim::DieForward, $PlayerAnim::DieForwardKneel);
						}
						else if (%vertPos == "legs") {
							if(%quadrant == "front_left" ||	%quadrant == "back_left") 
								%curDie = $PlayerAnim::DieLegLeft;
							if(%quadrant == "front_right" ||	%quadrant == "back_right") 
								%curDie = $PlayerAnim::DieLegRight;
						}
						Player::setAnimation(%this, %curDie);
					}
					if(%type == $ImpactDamageType && %object.clLastMount != "")  
						%shooterClient = %object.clLastMount;
					Client::onKilled(%damagedClient,%shooterClient, %type);
				}
			}
		}
	}
}

function radnomItems(%num, %an0, %an1, %an2, %an3, %an4, %an5, %an6)
{
	return %an[floor(getRandom() * (%num - 0.01))];
}

function Player::onCollision(%this,%object)
{
	if (Player::isDead(%this)) {
		if (getObjectType(%object) == "Player") {
			// Transfer all our items to the player
			%sound = false;
			%max = getNumItems();
			for (%i = 0; %i < %max; %i = %i + 1) {
				%count = Player::getItemCount(%this,%i);
				if (%count) {
					%delta = Item::giveItem(%object,getItemData(%i),%count);
					if (%delta > 0) {
						Player::decItemCount(%this,%i,%delta);
						%sound = true;
					}
				}
			}
			if (%sound) {
				// Play pickup if we gave him anything
				playSound(SoundPickupItem,GameBase::getPosition(%this));
			}
		}
	}
	// DELTAFORCE
	else if (getObjectType(%object) == "Player" && Player::getMountedItem(%object,$WeaponSlot) == "Knife") 
	{
		if (Player::isDead(%object))
		{
		} else {
		
			%thisId = Player::getClient(%this);
			%objectId = Player::getClient(%object);
			if(GameBase::getTeam(%object) != GameBase::getTeam(%this))
			{
				%dm = 0.5;
				if(Player::getArmor(%objectid) == "carmor" || Player::getArmor(%objectid) == "cfemale" || Player::getArmor(%objectid) == "carmor2" || Player::getArmor(%objectid) == "cfemale2" || Player::getArmor(%objectid) == "carmor3" || Player::getArmor(%objectid) == "cfemale3")
					%dm  = 1.1;

				BleedTick(%this, %object);
				GameBase::applyDamage(%this,$StabDamageType,%dm,GameBase::getPosition(%this),"0 0 0","0 0 0",%objectId);  
				Client::sendMessage(Player::getClient(%object),1,"You stabbed "  @ Client::getName(%thisId) @ "!");				
				Client::sendMessage(Player::getClient(%this),1,"You have been stabbed by "  @ Client::getName(%objectId) @ "!");				
			//} else if($Server::teamDamageScale != 0){
			//	BleedTick(%this, %object);
			//	GameBase::applyDamage(%this,$StabDamageType,0.5*$Server::teamDamageScale,GameBase::getPosition(%this),"0 0 0","0 0 0",%objectId);  
			//	Client::sendMessage(Player::getClient(%object),1,"You stabbed teammate"  @ Client::getName(%thisId) @ "!");				
			//	Client::sendMessage(Player::getClient(%this),1,"You have been stabbed by teammate"  @ Client::getName(%objectId) @ "!");
			//}
			
			// not gonna let people use knives on teammates anymore.
			} else {
				Client::sendMessage(Player::getClient(%object),1,"That would be silly! "  @ Client::getName(%thisId) @ " is your teammate!");				
						

			}	 
		}
	}
	else if($Game::missionType == "Jail")
	{
		if(Player::isAiControlled(%this) && !Player::isAiControlled(%object)) {
			//echo("Debug: One of us is an AI and the other isn't...");
			if(getObjectType(%object) == "Player") {
				//echo("Debug: Hey! You're a player!");
				if(!Player::isDead(%object) && !Player::isDead(%this)) {
					//echo("Debug: Neither of us are dead...");
					%cl = GameBase::getOwnerClient(%object);
					%aiClient = Player::getClient(%this);
					if($TakenHostages[%cl] < 3) {
						//echo("You haven't reached the hostage limit yet...");
						// Capture the hostage for the player's teams
						%oldTeam = $hostageTeam[%aiClient];
						%playerTeam = GameBase::getTeam(%object);
						if (%oldTeam == %playerTeam) {
							return;
						}			
				
						$hostageTeam[%aiClient] = %playerTeam;
			
						GameBase::setTeam(%this, %playerTeam);
	
						for (%c = Client::getFirst(); %c != -1; %c = Client::getNext(%c)) {
							Client::SendMessage(%c, 1, Client::getName(%aiClient) @ " has been taken by " @ Client::getName(%cl) @ "!");
						}
			
						%aiName = Client::getName(%aiClient);	
						AI::DirectiveFollow(%aiName, %cl, 0, 1024);
						$TakenHostages[%cl] += 1;
						$HostageOwner[%aiClient] = %cl;
						%c = 0;
						%q = 0;
						while(%q < 1 && %c < 3) {
							if($Hostages[%cl, %c] == 0) {
								$Hostages[%cl, %c] = %aiClient;
								%q = 1;
							}
							%c++;
						}
					} else if($HostageOwner[%aiClient] != %cl) {
						Client::SendMessage(%cl, 0, "You cannot take any more hostages!");
					}
				}
			}
		}

	}
	else if($Game::missionType == "Hostage" || $hostageGame == 1)
	{
		if(Player::isAiControlled(%this) && !Player::isAiControlled(%object)) {
			//echo("Debug: One of us is an AI and the other isn't...");
			if(getObjectType(%object) == "Player") {
				//echo("Debug: Hey! You're a player!");
				if(!Player::isDead(%object) && !Player::isDead(%this)) {
					//echo("Debug: Neither of us are dead...");
					%cl = GameBase::getOwnerClient(%object);
					%aiClient = Player::getClient(%this);
					if($TakenHostages[%cl] < 3) {
						//echo("You haven't reached the hostage limit yet...");
						// Capture the hostage for the player's teams
						%oldTeam = $hostageTeam[%aiClient];
						%playerTeam = GameBase::getTeam(%object);
						if (%oldTeam == %playerTeam) {
							return;
						}			
				
						if (%oldTeam != -1 && %oldTeam != getNumTeams() - 1) {
							$deltaTeamScore[%oldTeam] -= 5;
						}
   						$deltaTeamScore[%playerTeam] += 5;
						$hostageTeam[%aiClient] = %playerTeam;
			
						GameBase::setTeam(%this, %playerTeam);
	
						for (%c = Client::getFirst(); %c != -1; %c = Client::getNext(%c)) {
							Client::SendMessage(%c, 1, Client::getName(%aiClient) @ " has been taken by " @ Client::getName(%cl) @ "!");
						}
			
						%aiName = Client::getName(%aiClient);	
						AI::DirectiveFollow(%aiName, %cl, 0, 1024);
						$TakenHostages[%cl] += 1;
						$HostageOwner[%aiClient] = %cl;
						%c = 0;
						%q = 0;
						while(%q < 1 && %c < 3) {
							if($Hostages[%cl, %c] == 0) {
								$Hostages[%cl, %c] = %aiClient;
								%q = 1;
							}
							%c++;
						}
					} else if($HostageOwner[%aiClient] != %cl) {
						Client::SendMessage(%cl, 0, "You cannot take any more hostages!");
					}
				}
			}
		}

	}
	

	// END DELTAFORCE
}

// Delta Force players and ground vehicles can't be tracked by SAMs...only Apaches and Blackhawks can
function Player::getHeatFactor(%this)
{
	// Hack to avoid turret turret not tracking vehicles.
	// Assumes that if we are not in the player we are
	// controlling a vechicle, which is not always correct
	// but should be OK for now.
	%client = Player::getClient(%this);
	%vehicle = Client::getControlObject(%client);
	if (%vehicle != %this) {
		if (Gamebase::getDataName(%vehicle) == Apache || Gamebase::getDataName(%vehicle) == Fighter || Gamebase::getDataName(%vehicle) == Fighter2 || Gamebase::getDataName(%vehicle) == Blackhawk || Gamebase::getDataName(%vehicle) == Warthog || Gamebase::getDataName(%vehicle) == MoveWarthog || Gamebase::getDataName(%vehicle) == Bomber || Gamebase::getDataName(%vehicle) == Hind || Gamebase::getDataName(%vehicle) == Hercules || Gamebase::getDataName(%vehicle) == Gunship){
			//Client::sendMessage(%client,0,"** WARNING ** - an enemy SAM is tracking you!~waccess_denied.wav");
			//schedule("Client::sendMessage(" @ %client @ ",0,\"~waccess_denied.wav\");",0.5);
			//schedule("Client::sendMessage(" @ %client @ ",0,\"~waccess_denied.wav\");",1.0);
			//schedule("Client::sendMessage(" @ %client @ ",0,\"~waccess_denied.wav\");",1.5);	
			//topprint(%client, "*WARNING*: You are being tracked by a turret! No warning will be given if it obtains a lock!", 1);
			return 1.0;
		}
	}
   // %time = getIntegerTime(true) >> 5;
   // %lastTime = Player::lastJetTime(%this) >> 10;

   // if ((%lastTime + 1.5) < %time) {
      return 0.0;
   // } else {
   //    %diff = %time - %lastTime;
   //   %heat = 1.0 - (%diff / 1.5);
   //   return %heat;
  // }
}

function Player::jump(%this,%mom)
{
   %cl = GameBase::getControlClient(%this);
   if(%cl != -1)
   {
      %vehicle = Player::getMountObject (%this);
		%this.lastMount = %vehicle;
		%this.newMountTime = getSimTime() + 3.0;
		Player::setMountObject(%this, %vehicle, 0);
		Player::setMountObject(%this, -1, 0);
		Player::applyImpulse(%pl,%mom);
		playSound (GameBase::getDataName(%this).dismountSound, GameBase::getPosition(%this));
   }
}


//----------------------------------------------------------------------------

function remoteKill(%client)
{
   if(!$matchStarted)
      return;

   %player = Client::getOwnedObject(%client);
   if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
   {
		playNextAnim(%client);
	   Player::kill(%client);
	   Client::onKilled(%client,%client);
   }
   %client.inVehicle = false;
}

$animNumber = 25;	 
function playNextAnim(%client)
{
	if($animNumber > 36) 
		$animNumber = 25;		
	Player::setAnimation(%client,$animNumber++);
}

function Client::takeControl(%clientId, %objectId)
{
   // remote control
   if(%objectId == -1)
   {
      //echo("objectId = " @ %objectId);
      return;
   }

	%pl = Client::getOwnedObject(%clientId);
	// If mounted to a vehicle then can't mount any other objects
	if(%pl.driver != "" || %pl.vehicleSlot != "")
		return;

   if(GameBase::getTeam(%objectId) != Client::getTeam(%clientId))
   {
      //echo("Teams: " @GameBase::getTeam(%objectId) @ " " @ Client::getTeam(%clientId));
      return;
   }
   if(GameBase::getControlClient(%objectId) != -1)
   {
      //echo("Ctrl Client = " @ GameBase::getControlClient(%objectId));
      return;
   }
	%name = GameBase::getDataName(%objectId);
	if(%name != CameraTurret && %name != DeployableTurret && %name != DeployablePlasma && %name != DeployableMortar && name != DeployableSAM && %name != DeployableAA && %name != ControlledDeployableAA)
   {
	   if(!GameBase::isPowered(%objectId)) 
		{
	      //echo("Turret " @ %objectId @ " not powered.");
		DecomissionTurret(%objectId);
		
	      return;
		}
   }
   if(!(Client::getOwnedObject(%clientId)).CommandTag && GameBase::getDataName(%objectId) != CameraTurret &&
      !$TestCheats) {
		Client::SendMessage(%clientId,0,"Must be at a Command Station to control turrets");
   		return;
   }
   if( %name == DeployableSAM || %name == RocketTurret ) {
	Client::SendMessage(%clientId,0,"Controling SAMs is no long an option.");
	return;
   }
   if(GameBase::getDamageState(%objectId) == "Enabled") {
	%object = Client::getOwnedObject(%clientID);
	if( %name == DeployableAA || %name == FlakTurret || %name == ControlledDeployableAA || %name == ControlledFlakTurret ) 
	{
		%pos = Gamebase::Getposition(%objectId);
		%rot = GameBase::getRotation(%objectId); 
		if(%name == FlakTurret || %name == ControlledFlakTurret)
			%turret = newObject("hellfiregun","Turret",ControlledFlakTurret,false);
		else
			%turret = newObject("hellfiregun","Turret",ControlledDeployableAA,true);
		%group = GetGroup(%objectId);
		addToSet(%group, %turret); 
		GameBase::setTeam(%turret,GameBase::getTeam(%objectId)); 
		GameBase::setPosition(%turret,%pos); 
		GameBase::setRotation(%turret,%rot); 
		Gamebase::setMapName(%turret,GameBase::getMapName(%objectId));
		GameBase::setenergy(%turret,GameBase::getEnergy(%objectId));
		deleteobject(%objectId);
		%objectId = %turret;
		%name = GameBase::getDataName(%objectId);
	}
	%object.turretcontrol = %objectId;								
   	Client::setControlObject(%clientId, %objectId);
   	Client::setGuiMode(%clientId, $GuiModePlay);

   } else
	DecomissionTurret(%objectId);

}

function remoteCmdrMountObject(%clientId, %objectIdx)
{
   Client::takeControl(%clientId, getObjectByTargetIndex(%objectIdx));
}

function checkControlUnmount(%clientId)
{
   %ownedObject = Client::getOwnedObject(%clientId);
   %ctrlObject = Client::getControlObject(%clientId);
   %clientId.inVehicle = false;
   if(%ownedObject != %ctrlObject)
   {
      if(%ownedObject == -1 || %ctrlObject == -1)
         return;
      if (Gamebase::getDataName(%ctrlObject) == RocketTurret || Gamebase::getDataName(%ctrlObject) == DeployableSAM)
	 return;
      if(getObjectType(%ownedObject) == "Player" && Player::getMountObject(%ownedObject) == %ctrlObject)
         return;
      Client::setControlObject(%clientId, %ownedObject);
	startGracePeriod(%clientId, %ownedObject);
	if (Gamebase::getDataName(%ctrlObject) == MortarTurret || Gamebase::getDataName(%ctrlObject) == DeployableMortar) Gamebase::startFadeIn(%ownedObject); 
   }
}

