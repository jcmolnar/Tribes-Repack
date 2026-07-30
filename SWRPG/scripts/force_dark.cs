//-- SPELL DEFINITIONS -------------------------------------------------------------------------------------------
//Defined alphabetically

$si = 0;

$Spell::keyword[$si++] = "blind";
$Spell::index[$Spell::keyword[$si]] = $si;
$Spell::name[$si] = "Force Blindness";
$Spell::description[$si] = "Blinds your opponent, making them unable to attack you.. (duration = sqrt(level) + 4 seconds)";
$Spell::delay[$si] = 0;
$Spell::ticks[$si] = 2;
$Spell::recoveryTime[$si] = 1.5;
//$Spell::radius[$si] = 10;
//$Spell::damageValue[$si] = "70";
$Spell::LOSrange[$si] = 80;
$Spell::manaCost[$si] = 1;
$Spell::startSound[$si] = ActivateAB;
$Spell::endSound[$si] = LaunchFB;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = 20;
$Spell::graceDistance[$si] = 5;
$SkillType[$Spell::keyword[$si]] = $SkillOffensiveCasting;

$Spell::keyword[$si++] = "choke";
$Spell::index[$Spell::keyword[$si]] = $si;
$Spell::name[$si] = "Force Choke";
$Spell::description[$si] = "Choke your target. Darth Vader style.";
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
$SkillType[$Spell::keyword[$si]] = $SkillOffensiveCasting;

$Spell::keyword[$si++] = "drain";
$Spell::index[$Spell::keyword[$si]] = $si;
$Spell::name[$si] = "Force Drain";
$Spell::description[$si] = "Drain the life from your opponents, and give it to yourself.";
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
$SkillType[$Spell::keyword[$si]] = $SkillOffensiveCasting;

$Spell::keyword[$si++] = "fear";
$Spell::index[$Spell::keyword[$si]] = $si;
$Spell::name[$si] = "Force Fear";
$Spell::description[$si] = "Scare your opponent. .. Somehow. It'll work, srsly.";
$Spell::delay[$si] = 0;
$Spell::recoveryTime[$si] = 1.5;
$Spell::radius[$si] = 10;
$Spell::LOSrange[$si] = 80;
$Spell::manaCost[$si] = 1;
$Spell::startSound[$si] = ActivateAB;
$Spell::endSound[$si] = LaunchFB;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = 20;
$Spell::graceDistance[$si] = 5;
$SkillType[$Spell::keyword[$si]] = $SkillOffensiveCasting;

$Spell::keyword[$si++] = "lightning";
$Spell::index[$Spell::keyword[$si]] = $si;
$Spell::name[$si] = "Force Lightning";
$Spell::description[$si] = "Send bolts of lightning into your opponent!";
$Spell::delay[$si] = 0;
$Spell::recoveryTime[$si] = 1.5;
$Spell::radius[$si] = 10;
$Spell::damageValue[$si] = "70";
$Spell::ticks[$si] = 10;
$Spell::LOSrange[$si] = 80;
$Spell::manaCost[$si] = 1;
$Spell::startSound[$si] = ActivateAB;
$Spell::endSound[$si] = LaunchFB;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = 20;
$Spell::graceDistance[$si] = 5;
$SkillType[$Spell::keyword[$si]] = $SkillOffensiveCasting;

$Spell::keyword[$si++] = "plague";
$Spell::index[$Spell::keyword[$si]] = $si;
$Spell::name[$si] = "Force Plague";
$Spell::description[$si] = "Cause your target to acquire a horrible illness.";
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
$SkillType[$Spell::keyword[$si]] = $SkillOffensiveCasting;

$Spell::keyword[$si++] = "rage";
$Spell::index[$Spell::keyword[$si]] = $si;
$Spell::name[$si] = "Force Rage";
$Spell::description[$si] = "Gather all that rage which boils within you, and unleash it upon your foes!";
$Spell::delay[$si] = 0;
$Spell::damageValue[$si] = "DEF -50 ATK 100 MDEF -10";
$Spell::ticks[$si] = 150; //5 minutes
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
$SkillType[$Spell::keyword[$si]] = $SkillOffensiveCasting;

$darkfpowers = $si;

//-------------------------------------------------------------------------------------------------------------

function DoDarkPowers(%clientId, %index, %oldpos, %castPos, %castObj, %player, %w2)
{
	if(%index == $Spell:index[blind])
	{
		if(getObjectType(%castObj) == "Player")
		{
			if(!Player::isAiControlled(%clientId))
			{
				Player::setDamageFlash(%damagedClient, 1);
				%m = floor(sqrt(fetchData(%clientId, LVL))) + $Spell::ticks[%index];
				if(%m > 15) %m = 15;
				for(%i = 1; %i < %m; %i++)
					schedule("Player::setDamageFlash(" @ %damagedClient @ ", 1);", %i);
			}
			//else
				//add stuff to disable bot following/attacking

			%castPos = GameBase::getPosition(%castObj);

			%returnFlag = True;
		}
		else
			%returnFlag = False;
	}

	else if(%index == $Spell:index[choke])
	{
		%castPos = GameBase::getPosition(%id);

		%returnFlag = True;
	}

	else if(%index == $Spell:index[drain])
	{
		%castPos = GameBase::getPosition(%id);

		%returnFlag = True;
	}

	else if(%index == $Spell:index[fear])
	{
		%castPos = GameBase::getPosition(%id);

		%returnFlag = True;
	}

	else if(%index == $Spell::index[lightning])
	{
		if(getObjectType(%castObj) == "Player")
			%id = Player::getClient(%castObj);

		%trans = GameBase::getMuzzleTransform(%clientId);
		%proj = Projectile::spawnProjectile("lightning1", %trans, %player, "0 0 0", 1.0);

		//%mom1 = Vector::getFromRot( GameBase::getRotation(%clientId), -60, 1 );
		//Player::applyImpulse(%clientId, %mom1);
	
		if(%id != "")
		{
			//%miss = CalcSpellMiss(%clientId, %id, %index);

			SpellDamage(%clientId, %id, $Spell::damageValue[%index], %index);
			%mom2 = Vector::getFromRot( GameBase::getRotation(%clientId), 50, 1 );
			Player::applyImpulse(%id, %mom2);
		}

		%castPos = GameBase::getPosition(%castObj);

		%returnFlag = True;

		schedule("deleteObject(" @ %proj @ ");", $Spell::ticks[$Spell::index[lightning]], %proj);
	}

	else if(%index == $Spell:index[plague])
	{
		%castPos = GameBase::getPosition(%id);

		%returnFlag = True;
	}

	else if(%index == $Spell:index[rage])
	{
		UpdateBonusState(%clientId, $Spell::damageValue[%index], fetchData(%clientId, LVL));
		%castPos = GameBase::getPosition(%player);
		%returnFlag = True;
	}

	return DoEndSpell(%clientId, %overrideEndSound, %extraDelay, %index, %castPos, %returnFlag);
}