//----------------------------------------------------------------------------
// Mech Mayhem -- shields, component damage, death spectacle (Stage 3).
//
// Overrides Player::onDamage wholesale (no super in TribesScript): the stock
// player.cs:88 body is replicated VERBATIM for non-mech armors; Herc armors
// divert into the mech pipeline:
//
//   shields  script pool (%this.mmShield) intercepts before hull. Energy
//            damage fully absorbed, ballistic 25% penetrates (the Starsiege
//            duality). Regen after a no-hit delay, driven from MechHeat's tick.
//   crits    from the engine's own positional signals (%vertPos head/torso/
//            legs, %quadrant front_*/back_*):
//              legs   accumulator -> Crippled twin swap (-40% speed)
//              head   cockpit bonus damage + sensor static flag (HUD, Stage 4)
//              torso rear = reactor: dissipation penalty + 2x death blast
//              torso side = weapon knockout roll (drops a mounted gun)
//   death    staged reactor detonation: warning sound, then blowUp + REAL
//            area damage to everything nearby (stand clear of dying mechs),
//            wreck corpse persists $MM::WreckSecs as battlefield dressing.
//----------------------------------------------------------------------------

$MM::ShieldMax[light] = 1.5;
$MM::ShieldMax[medium] = 2.5;
$MM::ShieldMax[heavy] = 3.5;
$MM::ShieldMax[assault] = 4.5;
$MM::ShieldMax[boss] = 6.0;
$MM::ShieldRegenDelay = 5;      // seconds since last hit before regen
$MM::ShieldRegenRate = 0.25;    // hull-units per heat tick while regenning
$MM::LegCritFrac = 0.30;        // leg damage (frac of hull) that cripples
$MM::GunCritFrac = 0.45;        // side-torso damage that risks a gun knockout
$MM::ReactorPenaltySecs = 15;
$MM::DetonateRadius = 15;
$MM::DetonateDamage = 1.2;      // at center, falls off linearly
$MM::WreckSecs = 60;

function MechDamage::isMech(%armor)
{
   return String::getSubStr(%armor, 0, 4) == "Herc";
}

//--- shields ----------------------------------------------------------------

function MechShield::init(%pl, %chassis)
{
   %cls = $MM::Class[%chassis];
   if (%cls == "")
      %cls = "medium";
   %pl.mmShieldMax = $MM::ShieldMax[%cls];
   %pl.mmShield = %pl.mmShieldMax;
   %pl.mmLastHit = -100;
   %pl.mmLegDmg = 0;
   %pl.mmSideDmg = 0;
   %pl.mmCrippled = 0;
   %pl.mmGunsLost = 0;
}

// called from MechHeat::tick for every live mech
function MechShield::regen(%obj)
{
   if (%obj.mmShieldMax == "" || %obj.mmShield >= %obj.mmShieldMax)
      return;
   if (getSimTime() - %obj.mmLastHit < $MM::ShieldRegenDelay)
      return;
   %s = %obj.mmShield + $MM::ShieldRegenRate;
   if (%s > %obj.mmShieldMax)
      %s = %obj.mmShieldMax;
   %obj.mmShield = %s;
}

// returns the damage that reaches the hull
function MechShield::absorb(%this, %type, %value)
{
   if (%this.mmShield <= 0)
      return %value;
   // ballistic/explosive punch through partially; energy is fully absorbed
   %pen = 0;
   if (%type == $BulletDamageType || %type == $ShrapnelDamageType
       || %type == $ExplosionDamageType || %type == $MortarDamageType
       || %type == $MissileDamageType || %type == $MineDamageType)
      %pen = 0.25;
   %toShield = %value * (1 - %pen);
   %through = %value * %pen;
   if (%toShield <= %this.mmShield) {
      %this.mmShield = %this.mmShield - %toShield;
   }
   else {
      %through = %through + (%toShield - %this.mmShield);
      %this.mmShield = 0;
      %cl = Player::getClient(%this);
      if (%cl > 0)
         Client::sendMessage(%cl, 1, "SHIELDS DOWN");
   }
   return %through;
}

//--- components -------------------------------------------------------------

function MechCrit::legs(%this, %value)
{
   %this.mmLegDmg = %this.mmLegDmg + %value;
   if (%this.mmCrippled == 1)
      return;
   %armor = Player::getArmor(%this);
   %hull = %armor.maxDamage;
   if (%this.mmLegDmg >= %hull * $MM::LegCritFrac) {
      %this.mmCrippled = 1;
      %base = MechHeat::baseChassis(%armor);
      %dmg = GameBase::getDamageLevel(%this);
      Player::setArmor(%this, %base @ "Crip");
      GameBase::setDamageLevel(%this, %dmg);
      %cl = Player::getClient(%this);
      if (%cl > 0)
         Client::sendMessage(%cl, 1, "LEG ACTUATORS DAMAGED -- speed reduced");
      echo("[MECHCRIT] legs: " @ %this @ " -> " @ %base @ "Crip");
   }
}

function MechCrit::sensors(%this)
{
   %this.mmSensorOut = getSimTime() + 20;
   %cl = Player::getClient(%this);
   if (%cl > 0)
      Client::sendMessage(%cl, 1, "SENSOR ARRAY HIT -- radar degraded");
   echo("[MECHCRIT] sensors: " @ %this);
}

function MechCrit::reactor(%this)
{
   %this.mmReactorHit = getSimTime() + $MM::ReactorPenaltySecs;
   %this.mmBigBlast = 1;
   // dissipation penalty: bleed a chunk of the heat pool now
   %e = GameBase::getEnergy(%this);
   GameBase::setEnergy(%this, %e * 0.5);
   %cl = Player::getClient(%this);
   if (%cl > 0)
      Client::sendMessage(%cl, 1, "REACTOR SHIELDING BREACHED");
   echo("[MECHCRIT] reactor: " @ %this);
}

function MechCrit::guns(%this, %value)
{
   %this.mmSideDmg = %this.mmSideDmg + %value;
   %armor = Player::getArmor(%this);
   if (%this.mmSideDmg < %armor.maxDamage * $MM::GunCritFrac)
      return;
   %this.mmSideDmg = 0;   // reset the accumulator; each threshold = one roll
   if (getRandom() > 0.5)
      return;
   // knock out one mounted mech weapon
   for (%i = 0; %i < 8; %i++) {
      %type = Player::getMountedItem(%this, %i);
      if (%type != -1 && String::getSubStr(getItemData(%type), 0, 4) == "Mech") {
         Player::dropItem(%this, %type);
         %this.mmGunsLost++;
         %cl = Player::getClient(%this);
         if (%cl > 0)
            Client::sendMessage(%cl, 1, "WEAPON DESTROYED -- hardpoint offline");
         echo("[MECHCRIT] gun knocked out: " @ %this);
         return;
      }
   }
}

//--- death spectacle --------------------------------------------------------

// staged chain: two escalating internal pops, then the reactor blast. Nearby
// pilots get camera shake at each stage (Player::camShake native hook).
function MechDeath::spectacle(%this)
{
   %pos = GameBase::getPosition(%this);
   playSound(SoundFireMortar, %pos);
   schedule("MechDeath::pop(" @ %this @ ", 1);", 0.4, %this);
   schedule("MechDeath::pop(" @ %this @ ", 2);", 0.8, %this);
   schedule("MechDeath::detonate(" @ %this @ ");", 1.2, %this);
}

function MechDeath::pop(%this, %stage)
{
   if (%this == "" || %this <= 0)
      return;
   %pos = GameBase::getPosition(%this);
   playSound(SoundFireGrenade, %pos);
   MechDeath::shakeNear(%pos, 0.15 * %stage, 30);
}

// camera shake for every human piloting a mech within range, linear falloff
function MechDeath::shakeNear(%pos, %amp, %radius)
{
   $MMShake::pos = %pos;
   $MMShake::amp = %amp;
   $MMShake::radius = %radius;
   Group::iterateRecursive(MissionCleanup, "MechDeath::shakeVisit");
}

function MechDeath::shakeVisit(%obj)
{
   %data = GameBase::getDataName(%obj);
   if (String::getSubStr(%data, 0, 4) != "Herc")
      return;
   %cl = Player::getClient(%obj);
   if (%cl <= 0)
      return;
   %d = Vector::getDistance(GameBase::getPosition(%obj), $MMShake::pos);
   if (%d > $MMShake::radius)
      return;
   remoteEval(%cl, "MMShake", $MMShake::amp * (1 - (%d / $MMShake::radius)));
}

function MechDeath::detonate(%this)
{
   if (%this == "" || %this <= 0)
      return;
   %pos = GameBase::getPosition(%this);
   %scale = 1;
   if (%this.mmBigBlast == 1)
      %scale = 2;
   $MMDet::pos = %pos;
   $MMDet::scale = %scale;
   $MMDet::src = %this;
   Player::blowUp(%this);
   playSound(bigExplosion1, %pos);
   Group::iterateRecursive(MissionCleanup, "MechDeath::damageNear");
   MechDeath::shakeNear(%pos, 0.5 * %scale, 60 * %scale);
   echo("[MECHDEATH] detonation at " @ %pos @ " x" @ %scale);
}

function MechDeath::damageNear(%obj)
{
   if (%obj == $MMDet::src)
      return;
   %data = GameBase::getDataName(%obj);
   if (String::getSubStr(%data, 0, 4) != "Herc")
      return;
   if (Player::isDead(%obj))
      return;
   %d = Vector::getDistance(GameBase::getPosition(%obj), $MMDet::pos);
   %r = $MM::DetonateRadius * $MMDet::scale;
   if (%d > %r)
      return;
   %dmg = $MM::DetonateDamage * $MMDet::scale * (1 - (%d / %r));
   %through = MechShield::absorb(%obj, $ExplosionDamageType, %dmg);
   GameBase::setDamageLevel(%obj, GameBase::getDamageLevel(%obj) + %through);
   echo("[MECHDEATH] splash " @ %obj @ " d=" @ %d @ " dmg=" @ %dmg);
}

//--- the override -----------------------------------------------------------

function MechDamage::apply(%this, %type, %value, %pos, %vec, %mom, %vertPos, %quadrant, %object)
{
   %damagedClient = Player::getClient(%this);
   %shooterClient = %object;

   Player::applyImpulse(%this, %mom);

   // friendly fire (stock rule, without the chat spam for bots)
   %friendFire = 1.0;
   if ($teamplay && %damagedClient != %shooterClient
       && Client::getTeam(%damagedClient) == Client::getTeam(%shooterClient))
      %friendFire = $Server::TeamDamageScale;

   if (Player::isDead(%this))
      return;

   %armor = Player::getArmor(%this);
   %value = $DamageScale[%armor, %type] * %value * %friendFire;
   if (%value <= 0)
      return;

   %this.mmLastHit = getSimTime();

   // cockpit hits hurt more (the head band IS the canopy)
   if (%vertPos == "head")
      %value = %value * 1.15;

   %through = MechShield::absorb(%this, %type, %value);
   if (%through <= 0)
      return;

   // component tracks (post-shield: armor is breached)
   if (%vertPos == "legs")
      MechCrit::legs(%this, %through);
   else if (%vertPos == "head") {
      if (%this.mmSensorOut < getSimTime() && getRandom() < 0.35)
         MechCrit::sensors(%this);
   }
   else if (%vertPos == "torso") {
      if (%quadrant == "back_left" || %quadrant == "back_right") {
         if (%this.mmReactorHit < getSimTime() && getRandom() < 0.4)
            MechCrit::reactor(%this);
      }
      else
         MechCrit::guns(%this, %through);
   }

   %dlevel = GameBase::getDamageLevel(%this) + %through;
   GameBase::setDamageLevel(%this, %dlevel);
   %flash = Player::getDamageFlash(%this) + %through * 2;
   if (%flash > 0.75)
      %flash = 0.75;
   Player::setDamageFlash(%this, %flash);

   if (Player::isDead(%this)) {
      MechDeath::spectacle(%this);
      if (%type == $ImpactDamageType && %object.clLastMount != "")
         %shooterClient = %object.clLastMount;
      Client::onKilled(%damagedClient, %shooterClient, %type);
   }
}

function Player::onDamage(%this, %type, %value, %pos, %vec, %mom, %vertPos, %quadrant, %object)
{
   %mmArmor = Player::getArmor(%this);
   if (MechDamage::isMech(%mmArmor)) {
      MechDamage::apply(%this, %type, %value, %pos, %vec, %mom, %vertPos, %quadrant, %object);
      return;
   }

   // ------- stock player.cs:88 body, verbatim, for everything else -------
	if (Player::isExposed(%this)) {
      %damagedClient = Player::getClient(%this);
      %shooterClient = %object;

		Player::applyImpulse(%this,%mom);
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

echo("[MECH] MechDamage loaded (shields + crits + death spectacle).");
