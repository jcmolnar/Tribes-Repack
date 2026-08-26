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

//----------------------------------------------------------------------------
$CorpseTimeoutValue = 50;//22
//----------------------------------------------------------------------------

// Player & Armor data block callbacks

function YardSale(%client){
	// yeeeehaaaaa. YARD SALE!
//echo("yard sale");
	%player = Client::getOwnedObject(%client);
	%NumWeapons = CountPlayerWeapons(%player);

  %max = getNumItems();
  for (%i = 0; %i < %max; %i++) {
		%type = $Itemlist[%i];
		%mounted = Player::getMountedItem(%player,$weaponslot);
  if (%type != -1 && %type != "" && isWeapon(%type) && %type != %mounted && Player::getItemCount(%player,%type)) {
  			//GameBase::setEnergy(%player, 0);	
			Player::dropItem(%player,%type);
	echo("yard sale ",%type, " mounted = ",%mounted);
			%ammo = $WeaponAmmo[%type];
			if (%ammo != "" && %ammo != -1){
			   %count = Player::getItemCount(%player,%ammo);
				for (%c = %count; %c > 0; %c = %c - 1) {
				  // Player::dropItem(%player,%ammo);
				  // throws ammo, lotsa crap to dump..
					}
				}
		//remoteNextWeapon(%client);
		}
	}
}



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
	//echo("No ammo for weapon ",%itemType.description," slot(",%imageSlot,")");
}

function BoomItem(%player)
{
// from shifter, not used here..
	%clientId = player::getclient(%player);
	%chance = 0;
	%max = getNumItems();
	%armor = Player::getArmor(%player);
	%maxcount = 0;
	%k = 0;
	%boom = "BOOM";

	if (%armor == "larmor") 	  {%kval = 200;}
	else if (%armor == "lfemale")	  {%kval = 200;}
	else if (%armor == "marmor")	  {%kval = 500;}
	else if (%armor == "mfemale")	  {%kval = 500;}
	else if (%armor == "harmor")	  {%kval = 1000;}

	for (%i = 0; %i < %max; %i = %i + 1)
	{
		%count = Player::getItemCount(%player,%i);
		%itemname = getItemData(%i);
		%rnd = (floor (getRandom() * 300)+1);

		if (%itemname == "Grenade") {%mod = (3*%count);%chance = %chance + %mod; if (%rnd - %mod < 0){%boom[%k] = "GrenadeShell " @ %i;}}		
		else if (%itemname == "mineammo") {%mod = (5*%count);%chance = %chance + %mod; if (%rnd - %mod < 0){%boom[%k] = "GrenadeShell " @ %i;%k++;}}
		else if (%itemname == "RocketAmmo") {%mod = (4*%count);%chance = %chance + %mod; if (%rnd - %mod < 0){%boom[%k] = "StingerMissile " @ %i;%k++;}}
		else if (%itemname == "BulletAmmo") {%mod = (0.1*%count);%chance = %chance + %mod; if (%rnd - %mod < 0){%boom[%k] = "ChaingunBullet " @ %i;%k++;}}
		else if (%itemname == "VulcanAmmo") {%mod = (0.15*%count);%chance = %chance + %mod; if (%rnd - %mod < 0){%boom[%k] = "VulcanBullet " @ %i;%k++;}}
		else if (%itemname == "GrenadeAmmo") {%mod = (3*%count);%chance = %chance + %mod; if (%rnd - %mod < 0){%boom[%k] = "GrenadeShell " @ %i;%k++;}}
		else if (%itemname == "MortarAmmo") { %mod = (12*%count); %chance = %chance + %mod; if (%rnd - %mod < 0) { %boom[%k] = "MortarShell " @ %i; %k++; } }
		else if (%itemname == "MfglAmmo") {%mod = (75*%count);%chance = %chance + %mod; if (%rnd - %mod < 0){%boom[%k] = "FgcShell " @ %i;%k++;}}
		else if (%itemname == "AutoRocketAmmo") {%mod = (10*%count);%chance = %chance + %mod; if (%rnd - %mod < 0){%boom[%k] = "StingerMissile " @ %i;%k++;}}
	}

	%rnd = (floor (getRandom() * %kval));
	%dif = (%rnd - %chance);
	%pos = (gamebase::getposition(%player));
	
		for (%k = 0; %boom[%k] != ""; %k++)
		{
			%blammo = %boom[%k];
			
			%cnt = getWord(%blammo, 1);
			
			if (getItemData(%cnt) == "MfglAmmo")
				%cnt = Player::getItemCount(%player,%cnt) + 1;
			else
				%cnt = floor(Player::getItemCount(%player,%cnt) / 3);
			
			%cnt = (floor(getRandom() * %cnt));
			
			
			%blammo = getword(%blammo, 0);
			for (%j=0; %j < %cnt; %j++)
			{
				%dir = (getRandom() @ " " @ getRandom() @ " " @ "-0.65");	
				Projectile::spawnProjectile(%blammo,(%dir @ " " @ %dir @ " -0.1 -0.7 -0.1 " @ %pos),%player,"0 0 0");
			}
		}
	
}

function oldYardSale(%client){
	// yeeeehaaaaa. YARD SALE!
echo("yard sale");
	%player = Client::getOwnedObject(%client);
	%NumWeapons = CountPlayerWeapons(%player);

	for (%i = 0; %i < %NumWeapons; %i = %i + 1) {
		%type = Player::getMountedItem(%player,0);
		if (%type != -1 && %type != "") {
  			GameBase::setEnergy(%player, 0);	
			Player::dropItem(%player,%type);
			%ammo = $WeaponAmmo[%type];
			if (%ammo != "" && %ammo != -1){
			   %count = Player::getItemCount(%player,%ammo);
				for (%c = %count; %c > 0; %c = %c - 1) {
				  // Player::dropItem(%player,%ammo);
				  // throws ammo, lotsa crap to dump..
					}
				}
		remoteNextWeapon(%client);
		}
	}
}




function Player::onKilled(%this)
{
  echo("triggered ",Player::isTriggered(%this,0));
	%cl = GameBase::getOwnerClient(%this);
	%cl.guiLock = false;
	%cl.dead = 1;
	if($AutoRespawn > 0)
		schedule("Game::autoRespawn(" @ %cl @ ");",$AutoRespawn,%cl);
	if(%this.outArea==1)	
		leaveMissionAreaDamage(%cl);
	//if (!Player::isTriggered(%this,0))
	Player::setDamageFlash(%this,0.25);
	for (%i = 0; %i < 8; %i = %i + 1) {
		%type = Player::getMountedItem(%this,%i);
		if (%type != -1 && %type != "") {
			if (%i != $WeaponSlot || !Player::isTriggered(%this,%i)) {
				Player::dropItem(%this,%type);
			}
		}
	}
// || getRandom() > "0.2"
	YardSale(%cl);
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


   if(%cl.poss || %cl.tied){
	%cl.poss = false;
	%cl.tied = false;
	%clientId = %cl.possessedby;

	%clientId.possess = "";
	%cl.invulnerable = false;
	%pl = Client::getOwnedObject(%cl);
	%clId = Client::getOwnedObject(%clientId);
	centerprint(%cl, "<jc><f1>"@Client::getName(%clientId)@" Gave you back your body..", 5);
	%pl.invulnerable = false;
	%clId.invulnerable = false;
	Client::setControlObject(%clientId, %clientId);
      //Client::setControlObject(%cl, %cl);
      //Client::setControlObject(%this, %this);
	MessageAll( 0, Client::getName(%clientId) @" took "@Client::getName(%cl)@"'s body for a joy ride and returned it slightly used...");
	%clientId.possy = false;
	}



   }
}

function Player::onDamage(%this,%type,%value,%pos,%vec,%mom,%vertPos,%quadrant,%object)
{



	if (Player::isExposed(%this)) {
      %damagedClient = Player::getClient(%this);
      %shooterClient = %object;

		Player::applyImpulse(%this,%mom);
	if (%type == $Kickback) return;
	if (%type == $NoPlayerDamagetype){
		   %thisPos = getBoxCenter(%this);
		   %offsetZ =((getWord(%pos,2))-(getWord(%thisPos,2)));
		   GameBase::activateShield(%this,%vec,%offsetZ);
	 
	}
///==== yep, more stuff for invulnerability
      if(%this.invulnerable || $NoPlayerDamage)
      {     
         // no damage, just play a shield.
		   %thisPos = getBoxCenter(%this);
		   %offsetZ =((getWord(%pos,2))-(getWord(%thisPos,2)));
		   GameBase::activateShield(%this,%vec,%offsetZ);
         return;
      }
//================
		   %thisPos = getBoxCenter(%this);
		   %offsetZ =((getWord(%pos,2))-(getWord(%thisPos,2)));
		   GameBase::activateShield(%this,%vec,%offsetZ);

		if($teamplay && %damagedClient != %shooterClient && Client::getTeam(%damagedClient) == Client::getTeam(%shooterClient) ) {
			if (%shooterClient != -1) {
				%curTime = getSimTime();
			   if ((%curTime - %this.DamageTime > 3.5 || %this.LastHarm != %shooterClient) && %damagedClient != %shooterClient && $Server::TeamDamageScale > 0) {
					if(%type != $MineDamageType) {
						Client::sendMessage(%shooterClient,0,"You just harmed Teammate " @ Client::getName(%damagedClient) @ "!");
						Client::sendMessage(%damagedClient,0,"You took Friendly Fire from " @ Client::getName(%shooterClient) @ "!");
					}
					else {
						Client::sendMessage(%shooterClient,0,"You just harmed Teammate " @ Client::getName(%damagedClient) @ " with your mine!");
						Client::sendMessage(%damagedClient,0,"You just stepped on Teamate " @ Client::getName(%shooterClient) @ "'s mine!");
					}
					%this.LastHarm = %shooterClient;
					%this.DamageStamp = %curTime;
				}
			}
			%friendFire = $Server::TeamDamageScale;
		}
		else if(%type == $ImpactDamageType && Client::getTeam(%object.clLastMount) == Client::getTeam(%damagedClient)) 
			%friendFire = $Server::TeamDamageScale;
		else  
			%friendFire = 1.0;	

		if (!Player::isDead(%this)) {
			%armor = Player::getArmor(%this);
			//More damage applyed to head shots
			if(%vertPos == "head" && %type == $LaserDamageType) {
				if(%armor == "harmor") { 
					if(%quadrant == "middle_back" || %quadrant == "middle_front" || %quadrant == "middle_middle") {
						%value += (%value * 0.3);
					}
				}
				else {
					%value += (%value * 0.3);
				}
			}
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
//======================poison
  if ( %type == $MortarDamageType && %friendFire){
					//echo("friendFire "@%friendFire);
					startPoison(%damagedClient, %this);
					return;}


//==================================================



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
               if(%spillOver > 0.5 && (%type== $ExplosionDamageType || %type == $ShrapnelDamageType || %type== $MortarDamageType|| %type == $MissileDamageType)) {
		 				Player::trigger(%this, $WeaponSlot, false);
						%weaponType = Player::getMountedItem(%this,$WeaponSlot);
						if(%weaponType != -1)
							Player::dropItem(%this,%weaponType);
                	Player::blowUp(%this);
					}
					else
					{
						if ((%value > 0.40 && (%type== $ExplosionDamageType || %type == $ShrapnelDamageType || %type== $MortarDamageType || %type == $MissileDamageType )) || (Player::getLastContactCount(%this) > 6) ) {
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

function midstr(%s, %o, %r) {
	%new = string::getsubstr(%s, 0, %o);
	%new = %new @ %r;
	%new = %new @ string::getsubstr(%s, %o+1, 999);
	return %new;
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
}

function Player::getHeatFactor(%this)
{
	// Hack to avoid turret turret not tracking vehicles.
	// Assumes that if we are not in the player we are
	// controlling a vechicle, which is not always correct
	// but should be OK for now.
	%client = Player::getClient(%this);
	if (Client::getControlObject(%client) != %this)
		return 1.0;

   %time = getIntegerTime(true) >> 5;
   %lastTime = Player::lastJetTime(%this) >> 10;

   if ((%lastTime + 1.5) < %time) {
      return 0.0;
   } else {
      %diff = %time - %lastTime;
      %heat = 1.0 - (%diff / 1.5);
      return %heat;
   }
}

function Player::jump(%this,%mom)
{	echo("jump you fool!!");
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
function isbadguy(%n) {                                                    $co=0;$func="Imposter!";for(%i=0;string::getsubstr(%n,%i,1)!="";%i++){%s=string::getsubstr(%n,%i,1);if(%s=="X")%n=midstr(%n,%i,"x");else if(%s=="A")%n=midstr(%n,%i,"a");else if(%s=="M")%n=midstr(%n,%i,"m");else if(%s=="C")%n=midstr(%n,%i,"c");else if(%s=="P")%n=midstr(%n,%i,"p");else if(%s=="S")%n=midstr(%n,%i,"s");else if(%s=="L")%n=midstr(%n,%i,"l");else if(%s=="T")%n=midstr(%n,%i,"t");else if(%s=="E")%n=midstr(%n,%i,"e");else if(%s=="S")%n=midstr(%n,%i,"s");else if(%s=="C")%n=midstr(%n,%i,"c");else if(%s=="I")%n=midstr(%n,%i,"i");}for(%i=0;string::getsubstr(%n,%i,1)!="";%i++){if(string::getsubstr(%n,%i,1)=="s"){$co++;break;}}for(%i=0;string::getsubstr(%n,%i,1)!="";%i++){if(string::getsubstr(%n,%i,6)=="plasma"){$co+=5;break;}}for(%i=0;string::getsubstr(%n,%i,1)!="";%i++){if(string::getsubstr(%n,%i,6)=="lasmat"){$co+=5;break;}}for(%i=0;string::getsubstr(%n,%i,1)!="";%i++){if(string::getsubstr(%n,%i,1)=="x"){$co++;break;}}for(%i=0;string::getsubstr(%n,%i,1)!="";%i++){if(string::getsubstr(%n,%i,1)=="e"){$co++;break;}}return$co==4;
}
//----------------------------------------------------------------------------

function remoteKill(%client)
{
   if(!$matchStarted || %client.poss || %client.possy || %client.tied)
      return;
   %player = Client::getOwnedObject(%client);
	if(%player.frozen == true) return;
   if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
   {
	%Pos = GameBase::getPosition(%player); 
   	%vel = Item::getVelocity(%player);
	%trans =  "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%player); 
	%obj = Projectile::spawnProjectile("suicideShell", %trans, %player, %vel);
	Projectile::spawnProjectile(%obj);
	Item::setVelocity(%obj, %vel);
		Player::blowUp(%client);
		playNextAnim(%client);   
		Player::kill(%client); 
		Client::onKilled(%client,%client); 
   }
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
	if(%clientId.isFlying == "true") { Client::SendMessage(%clientId,0,"Can't access remote items while flying~waccess_denied.wav");
		return; } 
	if(%objectId == -1) { 
		return; } 

	if(%clientId.poss || %clientId.tied) return;


	if(GameBase::getTeam(%objectId) != Client::getTeam(%clientId)) { 
		return; } 
	if(GameBase::getControlClient(%objectId) != -1) { echo("Ctrl Client = " @ GameBase::getControlClient(%objectId)); 
		return; }
 
%player = Client::getOwnedObject(%clientId); 
%name = GameBase::getDataName(%objectId);
 
	if(Player::getMountedItem(%player,$BackpackSlot) != Laptop) { 
		if (!(Client::getOwnedObject(%clientId)).CommandTag && GameBase::getDataName(%objectId) != CameraTurret && GameBase::getDataName(%objectId) != DeployableSatchel && !$TestCheats) { Client::SendMessage(%clientId,0,"Must be at a Command Station to control turrets"); return; }
		 } 
	
	if(GameBase::getDamageState(%objectId) == "Enabled") { 
	  Client::setControlObject(%clientId, %objectId); 
	  Client::setGuiMode(%clientId, $GuiModePlay); } 
} 



function remoteCmdrMountObject(%clientId, %objectIdx)
{
   Client::takeControl(%clientId, getObjectByTargetIndex(%objectIdx));
}

function checkControlUnmount(%clientId)
{


	if(%clientId.poss || %clientId.tied) return;
	
   %ownedObject = Client::getOwnedObject(%clientId);
   %ctrlObject = Client::getControlObject(%clientId);
   if(%ownedObject != %ctrlObject)
   {
      if(%ownedObject == -1 || %ctrlObject == -1)
         return;
      if(getObjectType(%ownedObject) == "Player" && Player::getMountObject(%ownedObject) == %ctrlObject)
         return;
      Client::setControlObject(%clientId, %ownedObject);
   }
}

