// =================================TELEPORTER=================================
function GroupTrigger::onEnter(%this, %object)
{
//	if( %this.kill == "true" )
//	{
	Player::blowUp(%this);
	// killBitches(%object, %object);
//	}
//	else if( %this.scream == "true" )
//	{
//		scream(%object);
//	}
}


// =================================GIB THE FUGGERS=================================

function killBitches(%this, %killer, %type)
{
	%client = Player::getClient(%this);
	%killerClient = Player::getClient(%killer);
	%pos = GameBase::getPosition(%this);
	%test = GameBase::setDamageLevel(%this, 1.0);
//      echo(%test);
//	playSound(SoundGibDeath, %pos);
//	Client::sendMessage(%client,1,"~wBlowupDeath.wav");
	Player::blowUp(%this);
	Client::onKilled(%client, %killerClient, %type);
}

// =================================MAKE EM SCREAM=================================
// function scream(%this)
// {
//	%client = Player::getClient(%this);
//	%rand = getRandom();
//	if( %rand >= 0.4)
//	{
//		Client::sendMessage(%client,1,"~wDMJumpDeathCheer.wav");
//	}
//	else
//	{
//		Client::sendMessage(%client,1,"~wDMJumpDeathArg1.wav");
//	}
// }

