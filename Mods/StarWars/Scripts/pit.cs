function GroupTrigger::onEnter(%this, %object)
{
    %client = Player::getClient(%object);
    %CName = Client::getName(%client);

    if (%this.num == "Doom"){
      Player::blowUp(%client);
      Player::Kill(%client);
      playSound(debrisSmallExplosion, GameBase::getPosition(%object));
      messageall(0, %CName @ " gets eaten!");
    }
}

