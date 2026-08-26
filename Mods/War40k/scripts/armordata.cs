//-=-=-=-=-=-=-=-=-=-=-=-=-=

DamageSkinData armorDamageSkins 
{
	bmpName[0] = "dskin1_armor";
	bmpName[1] = "dskin2_armor";
	bmpName[2] = "dskin3_armor";
	bmpName[3] = "dskin4_armor";
	bmpName[4] = "dskin5_armor";
	bmpName[5] = "dskin6_armor";
	bmpName[6] = "dskin7_armor";
	bmpName[7] = "dskin8_armor";
	bmpName[8] = "dskin9_armor";
	bmpName[9] = "dskin10_armor";
};

function Armor::specDamage(%client, %player, %damager, %type, %option)
{
	%armor = Player::getArmor(%client);
	$SpecHarm[%player, %type] = %damager;
	if(%type != $webdamagetype) if(!$specdam[%armor, %type]) return;
	%list[$FlashDamageType, 1] = "Your armor systems short-circuited!";
	%list[$EnergyDamageType, 1] = "You feel extremely dizzy...poison!";
	%list[$PlasmaDamageType, 1] = "You catch on fire!";
	%list[$ChemDamageType, 1] = "Your flesh begins itching horribly!";
	%list[$AcidDamageType, 1] = "Acid begins to melt you!";
	%list[$FlashDamageType, 0] = "Your armor systems are back to normal.";
	%list[$EnergyDamageType, 0] = "The poison is nullified.";
	%list[$PlasmaDamageType, 0] = "You stop burning.";
	%list[$ChemDamageType, 0] = "The biotoxins cease their effects on you.";
	%list[$AcidDamageType, 0] = "You stop melting.";
	if(%option)
	{
		if(%type == $WebDamageType)
		{
echo("got to the specdam...");
			ArkFieldPack::deployShape(%player, 1, 5);
			return;
		}
		if ($LastSpecHarm[%player, %type] == "" || $LastSpecHarm[%player, %type] < 0) $LastSpecHarm[%player, %type] = 0;
		%rnd = floor(getRandom() * 20);
		if (%rnd < 5) return;
		$LastSpecHarm[%player, %type]++;
		if($LastSpecHarm[%player, %type] == 1)
		{
			Client::sendMessage(%client, 1, %list[%type, 1]);
			if(%type != $FlashDamageType) Player::setDamageFlash(%player,0.75);
			else
			{
			      %pack = Player::getMountedItem(%client,$BackpackSlot);
			      if (%pack != -1 && Player::isTriggered(%client,$BackpackSlot))
				{
					Player::trigger(%client,$BackpackSlot,FALSE);
				}
				if($LastSpecHarm[%player, %type] == 1) %player.rechRateStr = GameBase::getRechargeRate(%player);
				GameBase::setEnergy(%player,0);
				GameBase::setRechargeRate(%player,0);
			}
		}
		if(%type == $FlashDamageType)
		{
			Player::unmountItem(%player,$WeaponSlot);
			$time[%type, %player] = 8;
		}
		if(%type == $EnergyDamageType) $time[%type, %player] = %rnd + 20;
		if(%type == $PlasmaDamageType) $time[%type, %player] = 10;
		if(%type == $ChemDamageType) $time[%type, %player] = %rnd + 10;
		if(%type == $AcidDamageType) $time[%type, %player] = 10;
	}
	else
	{
		if($LastSpecHarm[%player, %type] > 1)
		{
			$LastSpecHarm[%player, %type]--;
			return;
		}
		if($time[%type, %player] > 0) $time[%type, %player] -= 2;
		else
		{
			$LastSpecHarm[%player, %type] = 0;
			Client::sendMessage(%client, 1, %list[%type, 0]);
			if(%type == $FlashDamageType) GameBase::setRechargeRate(%player, %player.rechRateStr);
			return;
		}
	}
	%dmglev = GameBase::getDamageLevel(%player);
	if (!Player::isDead(%player))
	{
		if(%type != $FlashDamageType)
		{
			if(%type == $ChemDamageType) %dmglev += 0.25 * $damagescale[%armor, %type];
			else %dmglev += 0.1 * $damagescale[%armor, %type];
			GameBase::setDamageLevel(%player, %dmglev);
			Player::setDamageFlash(%player,0.75);
			if (Player::isDead(%player))
			{
				$LastSpecHarm[%player, %type] = 0;
				Client::onKilled(%client, $SpecHarm[%player, %type], %type);
				return;
			}
		}
	}
	else
	{
		$LastSpecHarm[%player, %type] = 0;
		if(%type == $FlashDamageType) GameBase::setRechargeRate(%player, %player.rechRateStr);
		return;
	}
	schedule("Armor::specDamage(" @ %client @ ", " @ %player @ ", " @ %damager @ ", " @ %type @ ", 0);", 2, %player);
}

function Armor::onPlayerContact(%targetPlayer, %sourcePlayer)
{
//
}

function Armor::ThrowGrenade(%player, %obj)
{
	addToSet("MissionCleanup", %obj);
	%client = Player::getClient(%player);
	GameBase::throw(%obj,%player,15 * %client.throwStrength,false);
	%player.throwTime = getSimTime() + 0.5;
	GameBase::setTeam (%obj,GameBase::getTeam (%client));
}

function Armor::onRepairKit(%player)
{
// Heal poison & Biotox with repair kit
	$time[$EnergyDamageType, %player] = 0;
	$time[$ChemDamageType, %player] = 0;
}

function Armor::SpeedBooster(%player, %item, %power)
{
	%vec = Item::getVelocity(%player);
	if (%vec == "0 0 0") %vec = "0 10 0"; // Could use line of site
	%vec = Vector::Normalize(%vec);
	%vec = GetWord(%vec, 0) * %power @ " " @
	GetWord(%vec, 1) * %power @ " " @
	GetWord(%vec, 2) * %power;
	Player::applyImpulse(%player, %vec);
	GameBase::playSound(%this, SoundFireMortar, 0);
	Client::sendMessage(Player::getClient(%player),0, "You use a Nerve Stimulant.");
	Player::decItemCount(%player,%item);
}