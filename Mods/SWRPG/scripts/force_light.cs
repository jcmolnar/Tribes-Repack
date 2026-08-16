//-- SPELL DEFINITIONS -------------------------------------------------------------------------------------------
//Defined alphabetically

$si = $darkfpowers;

$Spell::keyword[$si++] = "absorb";
$Spell::index[absorb] = $si;
$Spell::name[$si] = "Force Absorb";
$Spell::description[$si] = "Absorb adds 50 FDEF to the caster.";
$Spell::delay[$si] = 2.0;
$Spell::recoveryTime[$si] = 8;
$Spell::damageValue[$si] = "MDEF 50";
$Spell::ticks[$si] = 150;	//5 minutes
$Spell::manaCost[$si] = 5;
$Spell::startSound[$si] = ActivateTR;
$Spell::endSound[$si] = ActivateTD;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = -10;
$Spell::graceDistance[$si] = 2;
$SkillType[absorb] = $SkillDefensiveCasting;

$Spell::keyword[$si++] = "battlemeditation";
$Spell::index[battlemeditation] = $si;
$Spell::name[$si] = "Battle Meditation";
$Spell::description[$si] = "Does leet stuff.";
$Spell::delay[$si] = 1.5;
$Spell::recoveryTime[$si] = 10;
$Spell::radius[$si] = 10;
$Spell::damageValue[$si] = -30;
$Spell::manaCost[$si] = 12;
$Spell::startSound[$si] = DeActivateWA;
$Spell::endSound[$si] = ActivateAR;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = -30;
$Spell::graceDistance[$si] = 2;
$SkillType[battlemeditation] = $SkillDefensiveCasting; //Contemplating making this a neutral one..

$Spell::keyword[$si++] = "fullheal";
$Spell::index[fullheal] = $si;
$Spell::name[$si] = "Full Heal, Self";
$Spell::description[$si] = "Fully heals the caster.";
$Spell::delay[$si] = 1.5;
$Spell::recoveryTime[$si] = 60;
$Spell::damageValue[$si] = 0;
$Spell::manaCost[$si] = 2;
$Spell::startSound[$si] = DeActivateWA;
$Spell::endSound[$si] = PlaceSeal;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = -9998;
$Spell::graceDistance[$si] = 2;
$SkillType[fullheal] = $SkillDefensiveCasting;

$Spell::keyword[$si++] = "heal";
$Spell::index[heal] = $si;
$Spell::name[$si] = "Force Heal";
$Spell::description[$si] = "Heals the caster or targeted friendly.";
$Spell::delay[$si] = 1.5;
$Spell::recoveryTime[$si] = 2.25;
$Spell::damageValue[$si] = -6;
$Spell::manaCost[$si] = 2;
$Spell::startSound[$si] = DeActivateWA;
$Spell::endSound[$si] = ActivateAR;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = -6;
$Spell::graceDistance[$si] = 2;
$SkillType[heal] = $SkillDefensiveCasting;

$Spell::keyword[$si++] = "massheal";
$Spell::index[massheal] = $si;
$Spell::name[$si] = "Mass Heal";
$Spell::description[$si] = "Heals caster and friendlies 10 meters around.";
$Spell::delay[$si] = 1.5;
$Spell::recoveryTime[$si] = 10;
$Spell::radius[$si] = 10;
$Spell::damageValue[$si] = -30;
$Spell::manaCost[$si] = 12;
$Spell::startSound[$si] = DeActivateWA;
$Spell::endSound[$si] = ActivateAR;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = -30;
$Spell::graceDistance[$si] = 2;
$SkillType[massheal] = $SkillDefensiveCasting;

$Spell::keyword[$si++] = "massfullheal";
$Spell::index[massfullheal] = $si;
$Spell::name[$si] = "Mass Full Heal";
$Spell::description[$si] = "Fully Heals caster and friendlies 12 meters around.";
$Spell::delay[$si] = 1.5;
$Spell::recoveryTime[$si] = 300;
$Spell::radius[$si] = 12;
$Spell::damageValue[$si] = 0;
$Spell::manaCost[$si] = 200;
$Spell::startSound[$si] = DeActivateWA;
$Spell::endSound[$si] = PlaceSeal;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = -9999;
$Spell::graceDistance[$si] = 2;
$SkillType[massfullheal] = $SkillDefensiveCasting;

$Spell::keyword[$si++] = "protect";
$Spell::index[shield] = $si;
$Spell::name[$si] = "Force Protect";
$Spell::description[$si] = "A shield of force energy that adds 50 DEF to the caster.";
$Spell::delay[$si] = 2.0;
$Spell::recoveryTime[$si] = 8;
$Spell::damageValue[$si] = "DEF 50";
$Spell::ticks[$si] = 150;	//5 minutes
$Spell::manaCost[$si] = 5;
$Spell::startSound[$si] = ActivateTR;
$Spell::endSound[$si] = ActivateTD;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = -10;
$Spell::graceDistance[$si] = 2;
$SkillType[protect] = $SkillDefensiveCasting;

$Spell::keyword[$si++] = "massprotect";
$Spell::index[shield] = $si;
$Spell::name[$si] = "Force Protect";
$Spell::description[$si] = "A shield of force energy that adds 115 DEF to all friendlies within a 10 meter radius.";
$Spell::delay[$si] = 2.0;
$Spell::recoveryTime[$si] = 8;
$Spell::damageValue[$si] = "DEF 115";
$Spell::ticks[$si] = 150;	//5 minutes
$Spell::manaCost[$si] = 5;
$Spell::startSound[$si] = ActivateTR;
$Spell::endSound[$si] = ActivateTD;
$Spell::groupListCheck[$si] = False;
$Spell::refVal[$si] = -10;
$Spell::graceDistance[$si] = 2;
$SkillType[protect] = $SkillDefensiveCasting;

$lightfpowers = $si;

//-------------------------------------------------------------------------------------------------------------

function DoLightPowers(%clientId, %index, %oldpos, %castPos, %castObj, %player, %w2)
{
	if(%index == $Spell::index[heal2]) //8
	{
		//heal self spell

		Client::sendMessage(%clientId, $MsgBeige, "Healing self");

		%r = $Spell::damageValue[%index] / $TribesDamageToNumericDamage;
		refreshHP(%clientId, %r);

		%castPos = GameBase::getPosition(%clientId);

		%returnFlag = True;
	}
	if(%index == $Spell::index[heal])
	{
		//heal self or other (LOS) 1st, 2nd, 3rd, 4th, 5th, 6th, godly

		if(getObjectType(%castObj) == "Player" && !Player::isAiControlled(%clientId))
			%id = Player::getClient(%castObj);
		else
			%id = %clientId;

		Client::sendMessage(%clientId, $MsgBeige, "Healing " @ Client::getName(%id));
		if(%clientId != %id)
			Client::sendMessage(%id, $MsgBeige, Client::getName(%clientId) @ " is casting " @ $Spell::name[%index] @ " on you.");

		%r = $Spell::damageValue[%index] / $TribesDamageToNumericDamage;

		refreshHP(%id, %r);

		%castPos = GameBase::getPosition(%id);

		%returnFlag = True;
	}
	if(%index == $spell::index[fullheal])
	{
		//full heal self spell

		Client::sendMessage(%clientId, $MsgBeige, "Fully healing self");

		setHP(%clientId, fetchData(%clientId, "MaxHP"));

		%castPos = GameBase::getPosition(%clientId);

		%returnFlag = True;
	}

	if(%index == $Spell::index[massheal] || %index == $Spell::index[massfullheal] || %index == $Spell::index[massabsorb] || %index == $Spell::index[massprotect]) //MassHeal MassFullHeal MassShield MassAbsorb
	{
		//massheal
		//massfullheal
		//massabsorb
		//massprotect

		%b = $Spell::radius[%index] * 2;
		%set = newObject("set", SimSet);
		%n = containerBoxFillSet(%set, $SimPlayerObjectType, GameBase::getPosition(%clientId), %b, %b, %b, 0);

		Group::iterateRecursive(%set, DoBoxFunction, %clientId, %index, %w2);
		deleteObject(%set);

		%overrideEndSound = True;

		%returnFlag = True;
	}

	if(%index == $Spell::index[protect] || %index == $Spell::index[absorb]) //Protect Absorb
	{
		//protect self
		//absorb force, self

		Client::sendMessage(%clientId, $MsgBeige, "Shielding self");

		UpdateBonusState(%clientId, $Spell::damageValue[%index], $Spell::ticks[%index]);

		%castPos = GameBase::getPosition(%clientId);

		%returnFlag = True;
	}

	if(%index == $Spell::index[speed] || %index == 21 || %index == 22 || %index == 23 || %index == 24 || %index == 27 || %index == 28 || %index == 29 || %index == 30 || %index == 31 || %index == 37)
	{
		//shield self or other (LOS) 1st, 2nd, 3rd, 4th, 5th
		//absorb force, self or other (LOS) 1st, 2nd, 3rd, 4th, 5th
		//force speed

		if(getObjectType(%castObj) == "Player" && !Player::isAiControlled(%clientId))
			%id = Player::getClient(%castObj);
		else
			%id = %clientId;

		Client::sendMessage(%clientId, $MsgBeige, "Shielding " @ Client::getName(%id));
		if(%clientId != %id)
			Client::sendMessage(%id, $MsgBeige, Client::getName(%clientId) @ " is casting " @ $Spell::name[%index] @ " on you.");

		UpdateBonusState(%id, $Spell::damageValue[%index], $Spell::ticks[%index]);

		%castPos = GameBase::getPosition(%clientId);

		%returnFlag = True;
	}

	return DoEndSpell(%clientId, %overrideEndSound, %extraDelay, %index, %castPos, %returnFlag);
}