$SellAmmo[MineAmmo] = 5;
$TeamItemMax[mineammo] = 35;
$InvList[MineAmmo] = 1;
$RemoteInvList[MineAmmo] = 1;

addAmmo(Misc, MineAmmo, 1);

function miscMine::Initialize()
{
 $TeamItemCount[0 @ mineammo] = 0;
 $TeamItemCount[1 @ mineammo] = 0;
 $TeamItemCount[2 @ mineammo] = 0;
 $TeamItemCount[3 @ mineammo] = 0;
 $TeamItemCount[4 @ mineammo] = 0;
 $TeamItemCount[5 @ mineammo] = 0;
 $TeamItemCount[6 @ mineammo] = 0;
 $TeamItemCount[7 @ mineammo] = 0;
}

function Mine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object) 
{
  if (%type == $MineDamageType) %value = %value * 0.25;
  %damageLevel = GameBase::getDamageLevel(%this);
  GameBase::setDamageLevel(%this,%damageLevel + %value);
  %damageLevel = GameBase::getDamageLevel(%this);
  %this.mindamage = %damageLevel;
}

function Mine::Detonate(%this) 
{
  %data = GameBase::getDataName(%this);
  GameBase::setDamageLevel(%this, %data.maxDamage);
}
 //-=-=-=-

MineData Handgrenade 
{
  mass = 0.3;
  drag = 1.0;
  density = 2.0;
  elasticity = 0.15;
  friction = 1.0;
  className = "Handgrenade";
  description = "Handgrenade";
  shapeFile = "grenade";
  shadowDetailMask = 4;
  explosionId = grenadeExp;
  explosionRadius = 10.0;
  damageValue = 0.8;
  damageType = $ShrapnelDamageType;
  kickBackStrength = 200;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

function Handgrenade::onAdd(%this) 
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",2.0,%this);
}

 //-=-=-=- MineAmmo

ItemData MineAmmo 
{
  description = "Mine";
  shapeFile = "mineammo";
  heading = $InvHead[ihMis];
  shadowDetailMask = 4;
  price = 10;
  className = "HandAmmo";
};

function MineAmmo::onUse(%player,%item) 
{
   if ($matchStarted && %player.throwTime < getSimTime()) 
  {
    Player::decItemCount(%player,%item);
    %armor = Player::getArmor(%player);
    eval(%armor @ "::onMine(" @ %player @ ");");
  }
}

 //-=-=-=- 

MineData AntipersonelMine 
{
  className = "Mine";
  description = "Antipersonel Mine";
  shapeFile = "mine";
  shadowDetailMask = 4;
  explosionId = rocketExp;
  explosionRadius = 10.0;
  damageValue = 0.75;
  damageType = $MineDamageType;
  kickBackStrength = 150;
  triggerRadius = 2.5;
  maxDamage = 0.5;
  shadowDetailMask = 0;
  destroyDamage = 1.0;
  damageLevel = {1.0, 1.0};
};

function AntipersonelMine::onAdd(%this) 
{
  %this.damage = 0;
  AntipersonelMine::deployCheck(%this);
}

function AntipersonelMine::onCollision(%this,%object) 
{
  %type = getObjectType(%object);
  %data = GameBase::getDataName(%this);
  if ((%type == "Player" || %data == AntipersonelMine || %data == Vehicle || %type == "Moveable") && GameBase::isActive(%this) && (GameBase::getTeam(%this)!=GameBase::getTeam(%object)) ) GameBase::setDamageLevel(%this, %data.maxDamage);
}

function AntipersonelMine::deployCheck(%this) 
{
  if (GameBase::isAtRest(%this)) 
  {
    GameBase::playSequence(%this,1,"deploy");
    GameBase::setActive(%this,true);
    %set = newObject("set",SimSet);
    if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),1,1,1,0)) 
    {
      %data = GameBase::getDataName(%this);
      GameBase::setDamageLevel(%this, %data.maxDamage);
    }
    deleteObject(%set);
  }
  else schedule("AntipersonelMine::deployCheck(" @ %this @ ");", 3, %this);
}

function AntipersonelMine::onDestroyed(%this) 
{
  $TeamItemCount[GameBase::getTeam(%this) @ "mineammo"]--;
}

function AntipersonelMine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object) 
{
  if (%type == $MineDamageType) %value = %value * 0.25;
  %data = GameBase::getDataName(%this);
  if((%data.maxDamage/1.5) < %this.damage+%value) GameBase::setDamageLevel(%this, %data.maxDamage);
  else %this.damage += %value;
}

 //-=-=-=-

MineData DMMine 
{
  className = "Mine";
  description = "Antipersonel Mine";
  shapeFile = "mine";
  shadowDetailMask = 4;
  explosionId = mineExp;
  explosionRadius = 10.0;
  damageValue = 0.65;
  damageType = $ShrapnelDamageType;
  kickBackStrength = 250;
  triggerRadius = 2.5;
  maxDamage = 0.5;
  shadowDetailMask = 0;
  destroyDamage = 1.0;
  damageLevel = {1.0, 1.0};
};

function DMMine::onAdd(%this) 
{
  %this.damage = 0;
  DMMine::deployCheck(%this);
}

function DMMine::onCollision(%this,%object) 
{
  %type = getObjectType(%object);
  %data = GameBase::getDataName(%this);
  if ((%type == "Player" || %data == AntipersonelMine || %data == Vehicle || %type == "Moveable") && GameBase::isActive(%this) ) GameBase::setDamageLevel(%this, %data.maxDamage);
}

function DMMine::deployCheck(%this) 
{
  if (GameBase::isAtRest(%this)) 
  {
    GameBase::playSequence(%this,1,"deploy");
    GameBase::setActive(%this,true);
    %set = newObject("set",SimSet);
    if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),1,1,1,0)) 
    {
      %data = GameBase::getDataName(%this);
      GameBase::setDamageLevel(%this, %data.maxDamage);
    }
    deleteObject(%set);
  }
  else schedule("DMMine::deployCheck(" @ %this @ ");", 3, %this);
}

function DMMine::onDestroyed(%this) 
{
  $TeamItemCount[GameBase::getTeam(%this) @ "mineammo"]--;
}

function DMMine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object) 
{
  if (%type == $MineDamageType) %value = %value * 0.25;
  %data = GameBase::getDataName(%this);
  if((%data.maxDamage/1.5) < %this.damage+%value) GameBase::setDamageLevel(%this, %data.maxDamage);
  else %this.damage += %value;
}

 //-=-=-=-


 //-=-=-=-

MineData Tranqgrenade 
{
  mass = 0.3;
  drag = 1.0;
  density = 2.0;
  elasticity = 0.15;
  friction = 1.0;
  className = "Handgrenade";
  description = "Handgrenade";
  shapeFile = "grenade";
  shadowDetailMask = 4;
  explosionId = Shockwave;
  explosionRadius = 15.0;
  damageValue = 0.45;
  damageType = $EnergyDamageType;
  kickBackStrength = 0;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

function Tranqgrenade::onAdd(%this) 
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",2.0,%this);
}

 //-=-=-=-

MineData Shockgrenade 
{
  mass = 0.3;
  drag = 1.0;
  density = 2.0;
  elasticity = 0.15;
  friction = 1.0;
  className = "Handgrenade";
  description = "Handgrenade";
  shapeFile = "grenade";
  shadowDetailMask = 4;
  explosionId = Shockwave;
  explosionRadius = -70.0;
  damageValue = 0.2;
  damageType = $FlashDamageType;
  kickBackStrength = 50;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

function Shockgrenade::onAdd(%this) 
{
//  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",2.0,%this);
}

 //-=-=-=-

MineData Concussion 
{
  mass = 0.3;
  drag = 1.0;
  density = 2.0;
  elasticity = 0.15;
  friction = 1.0;
  className = "Handgrenade";
  description = "Handgrenade";
  shapeFile = "grenade";
  shadowDetailMask = 4;
  explosionId = grenadeExp;
  explosionRadius = 15.0;
  damageValue = 0.50;
  damageType = $MortarDamageType;
  kickBackStrength = 0;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

function Concussion::onAdd(%this) 
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",2.0,%this);
}

 //-=-=-=-

MineData Nukebomb 
{
  mass = 0.3;
  drag = 1.0;
  density = 2.0;
  elasticity = 0.0;
  friction = 99.0;
  className = "Handgrenade";
  description = "Plastique";
  shapeFile = "sensor_small";
  shadowDetailMask = 4;
  explosionId = isoExp;
  explosionRadius = 15.0;
  damageValue = 20.4;
  damageType = $MeltaDamageType;
  kickBackStrength = 350;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

function Nukebomb::onAdd(%this) 
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",12.0,%this);
}

function Nukebomb::onCollision(%this,%obj) 
{
  if(getObjectType(%obj) != "Player") 
  {
    return;
  }
  if(Player::isDead(%obj)) 
  {
    return;
  }
  %c = Player::getClient(%obj);
  %playerTeam = GameBase::getTeam(%obj);
  %teleTeam = GameBase::getTeam(%this);
  %armor = Player::getArmor(%obj);
  if (%armor == "armormEngineer" || %armor == "armorfEngineer") 
  {
    %rnd = floor(getRandom() * 10);
    if(%rnd > 8) 
    {
      Client::sendMessage(%c,1,"OOPS! You cut the wrong wire...");
      Mine::Detonate(" @ %this @ ");
      return;
    }
    else 
    {
      deleteObject(%this);
      Client::sendMessage(%c,1,"You disarm the Melta Bomb.");
    }
  }
}

 //-=-=-=-
//-=-=-=-

MineData Mortarbomb 
{
  mass = 0.3;
  drag = 1.0;
  density = 2.0;
  elasticity = 0.15;
  friction = 1.0;
  className = "Handgrenade";
  description = "Handgrenade";
  shapeFile = "mortar"; //"grenade";
  shadowDetailMask = 4;
  explosionId = LargeShockwave;
  explosionRadius = 20.0;
  damageValue = 0.8;
  damageType = $MortarDamageType;
  kickBackStrength = 250;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

function Mortarbomb::onAdd(%this) 
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",2.0,%this);
}

 //-=-=-=-

MineData Firebomb 
{
  mass = 0.3;
  drag = 1.0;
  density = 2.0;
  elasticity = 0.15;
  friction = 1.0;
  className = "Handgrenade";
  description = "Handgrenade";
  shapeFile = "sensorjampack";
  shadowDetailMask = 4;
  explosionId = FireExp;
  explosionRadius = 60.0;
  damageValue = 0.95;
  damageType = $PlasmaDamageType;
  kickBackStrength = 150;
  triggerRadius = 0.5;
  maxDamage = 0.1;
};

function Firebomb::onAdd(%this) 
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",6.0,%this);
}

MineData Plasgren 
{
  mass = 0.3;
  drag = 1.0;
  density = 2.0;
  elasticity = 0.15;
  friction = 1.0;
  className = "Handgrenade";
  description = "Plasma Grenade";
  shapeFile = "grenade";
  shadowDetailMask = 4;
  explosionId = plasmaExp;
  explosionRadius = 15.0;
  damageValue = 0.47;
  damageType = $FlamerDamageType;
  kickBackStrength = 10;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

function Plasgren::onAdd(%this) 
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",2.0,%this);
}

MineData Krakgren 
{
  mass = 0.3;
  drag = 1.0;
  density = 2.0;
  elasticity = 0.15;
  friction = 1.0;
  className = "Handgrenade";
  description = "Krak Grenade";
  shapeFile = "grenade";
  shadowDetailMask = 4;
  explosionId = bulletExp0;
  explosionRadius = 5.0;
  damageValue = 2.47;
  damageType = $ShrapnelDamageType;
  kickBackStrength = 10;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

function Krakgren::onAdd(%this) 
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",2.0,%this);
}


function Mine::Detonate(%this)
{
	%data = GameBase::getDataName(%this);
	GameBase::setDamageLevel(%this, %data.maxDamage);
}

MineData Forcegrenade 
{
  mass = 0.3;
  drag = 1.0;
  density = 2.0;
  elasticity = 0.15;
  friction = 1.0;
  className = "Handgrenade";
  description = "Handgrenade";
  shapeFile = "grenade";
  shadowDetailMask = 4;
  explosionId = Shockwave;
  explosionRadius = 20.0;
  damageValue = 0.25;
  damageType = $FlashDamageType;
  kickBackStrength = 400;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

function Forcegrenade::onAdd(%this) 
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",2.0,%this);
}

MineData Psibomb 
{
  mass = 0.3;
  drag = 1.0;
  density = 2.0;
  elasticity = 0.15;
  friction = 1.0;
  className = "Handgrenade";
  description = "Scrambler Bomb";
  shapeFile = "sensor_jammer";
  shadowDetailMask = 4;
  explosionId = rocketExp;
  explosionRadius = 80.0;
  damageValue = 0.2;
  damageType = $FlashDamageType;
  kickBackStrength = 100;
  triggerRadius = 0.5;
  maxDamage = 0.5;
};

function Psibomb::onAdd(%this) 
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",6.0,%this);
}

function Psibomb::onCollision(%this,%obj) 
{
  if(getObjectType(%obj) != "Player") 
  {
    return;
  }
  if(Player::isDead(%obj)) 
  {
    return;
  }
  %c = Player::getClient(%obj);
  %playerTeam = GameBase::getTeam(%obj);
  %teleTeam = GameBase::getTeam(%this);
  %armor = Player::getArmor(%obj);
  if (%armor == "armormEngineer" || %armor == "armorfEngineer") 
  {
    %rnd = floor(getRandom() * 10);
    if(%rnd > 8) 
    {
      Client::sendMessage(%c,1,"OOPS! You cut the wrong wire...");
      Mine::Detonate(" @ %this @ ");
      return;
    }
    else 
    {
      deleteObject(%this);
      Client::sendMessage(%c,1,"You disarm the Scrambler Bomb.");
    }
  }
}

MineData Diegren 
{
  mass = 0.3;
  drag = 1.0;
  density = 2.0;
  elasticity = 0.0;
  friction = 99.0;
  className = "Handgrenade";
  description = "Super Krak";
  shapeFile = "sensor_small";
  shadowDetailMask = 4;
  explosionId = rocketExp;
  explosionRadius = 10.0;
  damageValue = 20.0;
  damageType = $MortarDamageType;
  kickBackStrength = 800;
  triggerRadius = 0.5;
  maxDamage = 0.5;
};

function Diegren::onAdd(%this) 
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",5.0,%this);
}

function Diegren::onCollision(%this,%obj) 
{
  if(getObjectType(%obj) != "Player") 
  {
    return;
  }
  if(Player::isDead(%obj)) 
  {
    return;
  }
  %c = Player::getClient(%obj);
  %playerTeam = GameBase::getTeam(%obj);
  %teleTeam = GameBase::getTeam(%this);
  %armor = Player::getArmor(%obj);
  if (%armor == "earmor" || %armor == "efemale") 
  {
    %rnd = floor(getRandom() * 10);
    if(%rnd > 8) 
    {
      Client::sendMessage(%c,1,"OOPS! You cut the wrong wire...");
      Mine::Detonate(" @ %this @ ");
      return;
    }
    else 
    {
      deleteObject(%this);
      Client::sendMessage(%c,1,"You disarm the Krak Bomb.");
    }
  }
}

MineData Everboom 
{
  mass = 0.3;
  drag = 1.0;
  density = 2.0;
  elasticity = 0.15;
  friction = 1.0;
  className = "Handgrenade";
  description = "Eversor Death";
  shapeFile = "force";
  shadowDetailMask = 4;
  explosionId = mortarExp;
  explosionRadius = 15.0;
  damageValue = 0.8;
  damageType = $ChemDamageType;
  kickBackStrength = 250;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

function Everboom::onAdd(%this) 
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",0.05,%this);
}

MineData Firewall 
{
  mass = 0.3;
  drag = 1.0;
  density = 2.0;
  elasticity = 0.15;
  friction = 1.0;
  className = "Handgrenade";
  description = "Firewall Psi";
  shapeFile = "plasmabolt";
  shadowDetailMask = 4;
  explosionId = mortarExp;
  explosionRadius = 15.0;
  damageValue = 0.50;
  damageType = $PlasmaDamageType;
  kickBackStrength = 10;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

MineData SDBoom 
{
  mass = 0.3;
  drag = 1.0;
  density = 2.0;
  elasticity = 0.15;
  friction = 1.0;
  className = "force";
  description = "Storm Daemon Death";
  shapeFile = "force";
  shadowDetailMask = 4;
  explosionId = mortarExp;
  explosionRadius = 45.0;
  damageValue = 0.3;
  damageType = $FlashDamageType;
  kickBackStrength = -450;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

function SDBoom::onAdd(%this) 
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",0.05,%this);
}

MineData Nanobomb 
{
  mass = 0.3;
  drag = 1.0;
  density = 2.0;
  elasticity = 0.15;
  friction = 1.0;
  className = "force";
  description = "Heal Bomb";
  shapeFile = "force";
  shadowDetailMask = 4;
  explosionId = mortarExp;
  explosionRadius = 35.0;
  damageValue = -1.0;
  damageType = $ExplosionDamageType;
  kickBackStrength = -150;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

function Nanobomb::onAdd(%this) 
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",3.0,%this);
}

//SPECIAL MINES
MineData HeavyMine 
{
  className = "Mine";
  description = "Heavy Mine";
  shapeFile = "mine";
  shadowDetailMask = 4;
  explosionId = mortarExp;
  explosionRadius = 10.0;
  damageValue = 2.00;
  damageType = $MineDamageType;
  kickBackStrength = 350;
  triggerRadius = 2.5;
  maxDamage = 1.0;
  shadowDetailMask = 0;
  destroyDamage = 1.0;
  damageLevel = {1.0, 1.0};
};

function HeavyMine::onAdd(%this) 
{
  %this.damage = 0;
  HeavyMine::deployCheck(%this);    
}

function HeavyMine::onCollision(%this,%object) 
{
  %type = getObjectType(%object);
  %data = GameBase::getDataName(%this);
  if ((%type == "Player" || %data == HeavyMine || %data == Vehicle || %type == "Moveable") && GameBase::isActive(%this) && (GameBase::getTeam(%this)!=GameBase::getTeam(%object)) ) GameBase::setDamageLevel(%this, %data.maxDamage);
}

function HeavyMine::deployCheck(%this) 
{
  if (GameBase::isAtRest(%this)) 
  {
    GameBase::playSequence(%this,1,"deploy");
    GameBase::setActive(%this,true);
    %set = newObject("set",SimSet);
    if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),1,1,1,0)) 
    {
      %data = GameBase::getDataName(%this);
      GameBase::setDamageLevel(%this, %data.maxDamage);
    }
    deleteObject(%set);
  }
  else schedule("HeavyMine::deployCheck(" @ %this @ ");", 3, %this);
}

function HeavyMine::onDestroyed(%this) 
{
  $TeamItemCount[GameBase::getTeam(%this) @ "mineammo"]--;
}

function HeavyMine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object) 
{
  if (%type == $MineDamageType) %value = %value * 0.25;
  %data = GameBase::getDataName(%this);
  if((%data.maxDamage/1.5) < %this.damage+%value) GameBase::setDamageLevel(%this, %data.maxDamage);
  else %this.damage += %value;
}



//-=-=-=-=-=-=DARK REAPER FUSION BOMB-=-=-=-=-==-=-
MineData FusionBomb 
{
  mass = 0.3;
  drag = 1.0;
  density = 2.0;
  elasticity = 0.15;
  friction = 1.0;
  className = "Handgrenade";
  description = "Handgrenade";
  shapeFile = "mortar";
  shadowDetailMask = 4;
  explosionId = LargeShockwave;
  explosionRadius = 80.0;
  damageValue = 1.665;
  damageType = $DDamageType;
  kickBackStrength = 250;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

function Fusionbomb::onAdd(%this) 
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",7.0,%this);
}

MineData EnerPackBoom 
{
  mass = 0.3;
  drag = 1.0;
  density = 2.0;
  elasticity = 0.15;
  friction = 1.0;
  className = "force";
  description = "Energy Pack Boom";
  shapeFile = "force";
  shadowDetailMask = 4;
  explosionId = grenadeExp;
  explosionRadius = 45.0;
  damageValue = 0.1;
  damageType = $FlashDamageType;
  kickBackStrength = -450;
  triggerRadius = 0.5;
  maxDamage = 2.0;
};

function EnerPackBoom::onAdd(%this) 
{
  %data = GameBase::getDataName(%this);
  schedule("Mine::Detonate(" @ %this @ ");",0.05,%this);
}

MineData ViralMine 
{
  className = "Mine";
  description = "Viral Mine";
  shapeFile = "mine";
  shadowDetailMask = 4;
  explosionId = mortarExp;
  explosionRadius = 6.0;
  damageValue = 0.3;
  damageType = $ChemDamageType;
  kickBackStrength = 350;
  triggerRadius = 2.5;
  maxDamage = 1.0;
  shadowDetailMask = 0;
  destroyDamage = 1.0;
  damageLevel = {1.0, 1.0};
};

function ViralMine::onAdd(%this) 
{
  %this.damage = 0;
  ViralMine::deployCheck(%this);    
}

function ViralMine::onCollision(%this,%object) 
{
  %type = getObjectType(%object);
  %data = GameBase::getDataName(%this);
  if ((%type == "Player" || %data == ViralMine || %data == Vehicle || %type == "Moveable") && GameBase::isActive(%this) && (GameBase::getTeam(%this)!=GameBase::getTeam(%object)) ) GameBase::setDamageLevel(%this, %data.maxDamage);
}

function ViralMine::deployCheck(%this) 
{
  if (GameBase::isAtRest(%this)) 
  {
    GameBase::playSequence(%this,1,"deploy");
    GameBase::setActive(%this,true);
    %set = newObject("set",SimSet);
    if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),1,1,1,0)) 
    {
      %data = GameBase::getDataName(%this);
      GameBase::setDamageLevel(%this, %data.maxDamage);
    }
    deleteObject(%set);
  }
  else schedule("ViralMine::deployCheck(" @ %this @ ");", 3, %this);
}

function ViralMine::onDestroyed(%this) 
{
  $TeamItemCount[GameBase::getTeam(%this) @ "mineammo"]--;
}

function ViralMine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object) 
{
  if (%type == $ChemDamageType) %value = %value * 0.25;
  %data = GameBase::getDataName(%this);
  if((%data.maxDamage/1.5) < %this.damage+%value) GameBase::setDamageLevel(%this, %data.maxDamage);
  else %this.damage += %value;
}

//-=-==-=-=-=-=--
MineData KrakMine 
{
  className = "Mine";
  description = "Krak Mine";
  shapeFile = "mine";
  shadowDetailMask = 4;
  explosionId = mortarExp;
  explosionRadius = 4.0;
  damageValue = 1.0;
  damageType = $KrakDamageType;
  kickBackStrength = 0;
  triggerRadius = 2.5;
  maxDamage = 1.0;
  shadowDetailMask = 0;
  destroyDamage = 1.0;
  damageLevel = {1.0, 1.0};
};

function KrakMine::onAdd(%this) 
{
  %this.damage = 0;
  ViralMine::deployCheck(%this);    
}

function KrakMine::onCollision(%this,%object) 
{
  %type = getObjectType(%object);
  %data = GameBase::getDataName(%this);
  if ((%type == "Player" || %data == KrakMine || %data == Vehicle || %type == "Moveable") && GameBase::isActive(%this) && (GameBase::getTeam(%this)!=GameBase::getTeam(%object)) ) GameBase::setDamageLevel(%this, %data.maxDamage);
}

function KrakMine::deployCheck(%this) 
{
  if (GameBase::isAtRest(%this)) 
  {
    GameBase::playSequence(%this,1,"deploy");
    GameBase::setActive(%this,true);
    %set = newObject("set",SimSet);
    if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),1,1,1,0)) 
    {
      %data = GameBase::getDataName(%this);
      GameBase::setDamageLevel(%this, %data.maxDamage);
    }
    deleteObject(%set);
  }
  else schedule("KrakMine::deployCheck(" @ %this @ ");", 3, %this);
}

function KrakMine::onDestroyed(%this) 
{
  $TeamItemCount[GameBase::getTeam(%this) @ "mineammo"]--;
}

function KrakMine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object) 
{
  if (%type == $KrakDamageType) %value = %value * 0.25;
  %data = GameBase::getDataName(%this);
  if((%data.maxDamage/1.5) < %this.damage+%value) GameBase::setDamageLevel(%this, %data.maxDamage);
  else %this.damage += %value;
}

//-=-==-=-=-=-=-=-=
// The Ark Mine
//-=-=-=-=-=-=-=-=-
MineData ArkMine 
{
  className = "Mine";
  description = "Ark Mine";
  shapeFile = "mine";
  shadowDetailMask = 4;
  explosionId = turretExp;
  explosionRadius = 4.0;
  damageValue = 0.1;
  damageType = $WebDamageType;
  kickBackStrength = 0;
  triggerRadius = 2.5;
  maxDamage = 1.0;
  shadowDetailMask = 0;
  destroyDamage = 1.0;
  damageLevel = {1.0, 1.0};
};

function ArkMine::onAdd(%this) 
{
  %this.damage = 0;
  ArkMine::deployCheck(%this);    
}

function ArkMine::onCollision(%this,%object) 
{
  %type = getObjectType(%object);
  %data = GameBase::getDataName(%this);
echo("arkmine collision");
  if ((%type == "Player" || %data == ArkMine || %data == Vehicle || %type == "Moveable") && GameBase::isActive(%this) && (GameBase::getTeam(%this)!=GameBase::getTeam(%object)) ) GameBase::setDamageLevel(%this, %data.maxDamage);
}

function ArkMine::deployCheck(%this) 
{
  if (GameBase::isAtRest(%this)) 
  {
    GameBase::playSequence(%this,1,"deploy");
    GameBase::setActive(%this,true);
    %set = newObject("set",SimSet);
    if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),1,1,1,0)) 
    {
      %data = GameBase::getDataName(%this);
      GameBase::setDamageLevel(%this, %data.maxDamage);
    }
    deleteObject(%set);
  }
  else schedule("ArkMine::deployCheck(" @ %this @ ");", 3, %this);
}

function ArkMine::onDestroyed(%this) 
{
  $TeamItemCount[GameBase::getTeam(%this) @ "mineammo"]--;
}

function ArkMine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object) 
{
  if (%type == $WebDamageType) %value = %value * 0.25;
  %data = GameBase::getDataName(%this);
  if((%data.maxDamage/1.5) < %this.damage+%value) GameBase::setDamageLevel(%this, %data.maxDamage);
  else %this.damage += %value; 
}

//-=-=-=-==-=-=-=-=-=
// The Psi Leech Mine
//-=-=-=-=-=-=-=-=-=-
// This mine is unfinished. Right now it jsut poisons, but it will soon suck Psi from units and give to placer.
MineData LeechMine 
{
  className = "Mine";
  description = "Leech Mine";
  shapeFile = "mine";
  shadowDetailMask = 4;
  explosionId = turretExp;
  explosionRadius = 6.0;
  damageValue = 0.3;
  damageType = $EnergyDamageType;
  kickBackStrength = 0;
  triggerRadius = 2.5;
  maxDamage = 1.0;
  shadowDetailMask = 0;
  destroyDamage = 1.0;
  damageLevel = {1.0, 1.0};
};

function LeechMine::onAdd(%this) 
{
  %this.damage = 0;
  LeechMine::deployCheck(%this);    
}

function LeechMine::onCollision(%this,%object) 
{
  %type = getObjectType(%object);
  %data = GameBase::getDataName(%this);
  if ((%type == "Player" || %data == LeechMine || %data == Vehicle || %type == "Moveable") && GameBase::isActive(%this) && (GameBase::getTeam(%this)!=GameBase::getTeam(%object)) ) GameBase::setDamageLevel(%this, %data.maxDamage);
}

function LeechMine::deployCheck(%this) 
{
  if (GameBase::isAtRest(%this)) 
  {
    GameBase::playSequence(%this,1,"deploy");
    GameBase::setActive(%this,true);
    %set = newObject("set",SimSet);
    if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),1,1,1,0)) 
    {
      %data = GameBase::getDataName(%this);
      GameBase::setDamageLevel(%this, %data.maxDamage);
    }
    deleteObject(%set);
  }
  else schedule("LeechMine::deployCheck(" @ %this @ ");", 3, %this);
}

function LeechMine::onDestroyed(%this) 
{
  $TeamItemCount[GameBase::getTeam(%this) @ "mineammo"]--;
}

function LeechMine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object) 
{
  if (%type == $EnergyDamageType) %value = %value * 0.25;
  %data = GameBase::getDataName(%this);
  if((%data.maxDamage/1.5) < %this.damage+%value) GameBase::setDamageLevel(%this, %data.maxDamage);
  else %this.damage += %value; 
}

//-=-=-=-=-=-=-=-=-=
// Inferno Mine
//-=-=-=-=-=-=-=-=-=
MineData FireMine 
{
  className = "Mine";
  description = "Fire Mine";
  shapeFile = "mine";
  shadowDetailMask = 4;
  explosionId = PlasCanExp;
  explosionRadius = 10.0;
  damageValue = 0.623;
  damageType = $PlasmaDamageType;
  kickBackStrength = 0;
  triggerRadius = 2.5;
  maxDamage = 1.0;
  shadowDetailMask = 0;
  destroyDamage = 1.0;
  damageLevel = {1.0, 1.0};
};

function FireMine::onAdd(%this) 
{
  %this.damage = 0;
  FireMine::deployCheck(%this);    
}

function FireMine::onCollision(%this,%object) 
{
  %type = getObjectType(%object);
  %data = GameBase::getDataName(%this);
  if ((%type == "Player" || %data == FireMine || %data == Vehicle || %type == "Moveable") && GameBase::isActive(%this) && (GameBase::getTeam(%this)!=GameBase::getTeam(%object)) ) GameBase::setDamageLevel(%this, %data.maxDamage);
}

function FireMine::deployCheck(%this) 
{
  if (GameBase::isAtRest(%this)) 
  {
    GameBase::playSequence(%this,1,"deploy");
    GameBase::setActive(%this,true);
    %set = newObject("set",SimSet);
    if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),1,1,1,0)) 
    {
      %data = GameBase::getDataName(%this);
      GameBase::setDamageLevel(%this, %data.maxDamage);
    }
    deleteObject(%set);
  }
  else schedule("FireMine::deployCheck(" @ %this @ ");", 3, %this);
}

function FireMine::onDestroyed(%this) 
{
  $TeamItemCount[GameBase::getTeam(%this) @ "mineammo"]--;
}

function FireMine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object) 
{
  if (%type == $PlasmaDamageType) %value = %value * 0.25;
  %data = GameBase::getDataName(%this);
  if((%data.maxDamage/1.5) < %this.damage+%value) GameBase::setDamageLevel(%this, %data.maxDamage);
  else %this.damage += %value; 
}

//-=-=-=-=-=-=-=-=-=
// Haywire Mine
//-=-=-=-=-=-=-=-=-=
MineData EMPMine 
{
  className = "Mine";
  description = "Haywire Mine";
  shapeFile = "mine";
  shadowDetailMask = 4;
  explosionId = LargeShockwave;
  explosionRadius = 14.0;
  damageValue = 0.23;
  damageType = $FlashDamageType;
  kickBackStrength = 0;
  triggerRadius = 2.5;
  maxDamage = 1.0;
  shadowDetailMask = 0;
  destroyDamage = 1.0;
  damageLevel = {1.0, 1.0};
};

function EMPMine::onAdd(%this) 
{
  %this.damage = 0;
  EMPMine::deployCheck(%this);    
}

function EMPMine::onCollision(%this,%object) 
{
  %type = getObjectType(%object);
  %data = GameBase::getDataName(%this);
  if ((%type == "Player" || %data == EMPMine || %data == Vehicle || %type == "Moveable") && GameBase::isActive(%this) && (GameBase::getTeam(%this)!=GameBase::getTeam(%object)) ) GameBase::setDamageLevel(%this, %data.maxDamage);
}

function EMPMine::deployCheck(%this) 
{
  if (GameBase::isAtRest(%this)) 
  {
    GameBase::playSequence(%this,1,"deploy");
    GameBase::setActive(%this,true);
    %set = newObject("set",SimSet);
    if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),1,1,1,0)) 
    {
      %data = GameBase::getDataName(%this);
      GameBase::setDamageLevel(%this, %data.maxDamage);
    }
    deleteObject(%set);
  }
  else schedule("EMPMine::deployCheck(" @ %this @ ");", 3, %this);
}

function EMPMine::onDestroyed(%this) 
{
  $TeamItemCount[GameBase::getTeam(%this) @ "mineammo"]--;
}

function EMPMine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object) 
{
  if (%type == $FlashDamageType) %value = %value * 0.25;
  %data = GameBase::getDataName(%this);
  if((%data.maxDamage/1.5) < %this.damage+%value) GameBase::setDamageLevel(%this, %data.maxDamage);
  else %this.damage += %value; 
}