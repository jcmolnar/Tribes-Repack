StaticShapeData SpringPad 
 {
   description = "Spring Pad";
   shapeFile = "elevator_4x4";
   className = "Misc";
   debrisId = defaultDebrisLarge; 
   explosionId = debrisExpLarge;
   maxDamage = 10000.0; 
   visibleToSensor = "false";
 }; 

function SpringPad::onDestroyed(%this)
  { 
	StaticShape::objectiveDestroyed(%this);
 }
 

function SpringPad::onCollision(%this,%obj)
 {
   %c = Player::getClient(%obj);
   if (floor(getRandom() * 30) == 0)
    { 
     GameBase::playSound(%this, debrisLargeExplosion, 0);
     Client::SendMessage(%c, 0, "Weeee!");
     %velocity = 200; %zVec = 400;
     %rnd = floor(getRandom() * 3);
     if (%rnd == 0)
      { 
        MessageAll(0,strcat(Client::getName(%c), " takes a trip into outer space"));
      } 
     else if (%rnd == 1) 
      {
        MessageAll(0,strcat(Client::getName(%c), " gets high on life"));
      } 
     else if (%rnd == 2) 
      { 
       MessageAll(0,strcat(Client::getName(%c), " goes to high"));
      }
    } 
   else if (floor(getRandom() * 7) == 0)
    { 
     GameBase::playSound(%this, debrisLargeExplosion, 0);
     Client::SendMessage(%c, 0, "Up, up, and AWAY!");
     %velocity = 200; %zVec = 400;
    } 
   else 
    { 
     GameBase::playSound(%this, SoundFireMortar, 0);
     Client::SendMessage(%c, 0, "Wooohooooo!");
     %velocity = 200;
     %zVec = 400;
    } 
   %jumpDir = Vector::getFromRot(GameBase::getRotation(%obj),%velocity,%zVec);
   Player::applyImpulse(%obj,%jumpDir);
  } 


