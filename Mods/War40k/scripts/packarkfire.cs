$TeamItemMax[FlameTurretPack] = 10;
$InvList[ArkFieldPack] = 0;
$RemoteInvList[ArkFieldPack] = 0;

ItemImageData ArkFieldPackImage
{
	shapeFile = "AmmoPack";
	mountPoint = 2;
	mountOffset = { 0, -0.03, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData ArkFieldPack
{
	description = "Arkfire Defense Field";
	shapeFile = "AmmoPack";
	className = "Backpack";
	heading = $InvHead[ihdob];
	imageType = ArkFieldPackImage;
	shadowDetailMask = 4;
	mass = 1.5;
	elasticity = 0.2;
	price = 30;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function ArkFieldPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) Player::mountItem(%player,%item,$BackpackSlot);
	else Player::deployItem(%player,%item);
}

function ArkFieldPack::onDeploy(%player,%item,%pos)
{
	ArkFieldPack::deployShape(%player, 0, 20);
	if(%player.arkfire == "") %player.arkfire = 0;
	%player.arkfire++;
	if(%player.arkfire > 2)
	{
		Player::decItemCount(%player, %item);
		%player.arkfire = "";
	}
	%client = Player::getClient(%player);
	Client::sendMessage(%client,0,"Arkfire Enabled");
	echo("MSG: ",%client," deployed an ArkFire Field");
}

function ArkFieldPack::deployShape(%player, %damage, %time)
{
	%client = Player::getClient(%player);
	%position = GameBase::getPosition(%player);

	if(%damage) %objArkField = newObject("ArkField","StaticShape",ArkDamageField,true);
	else %objArkField = newObject("ArkField","StaticShape",ArkField,true);
	%pos = Vector::add(%position, "-2.5 0 0");
	GameBase::setTeam(%objArkField,GameBase::getTeam(%player));
	GameBase::setPosition(%objArkField,%pos);
	GameBase::setRotation(%objArkField,"0 0 1.57");
	Gamebase::setMapName(%objArkField,"Ark Field");
	GameBase::startFadeIn(%objArkField);
	addToSet("MissionCleanup", %objArkField);
	schedule("GameBase::setDamageLevel(" @ %objArkField @ ", 2);", %time, %objArkField);

	if(%damage) %objArkField = newObject("ArkField","StaticShape",ArkDamageField,true);
	else %objArkField = newObject("ArkField","StaticShape",ArkField,true);
	%pos = Vector::add(%position, "2.5 0 0");
	GameBase::setTeam(%objArkField,GameBase::getTeam(%player));
	GameBase::setPosition(%objArkField,%pos);
	GameBase::setRotation(%objArkField,"0 0 1.57");
	Gamebase::setMapName(%objArkField,"Ark Field");
	GameBase::startFadeIn(%objArkField);
	addToSet("MissionCleanup", %objArkField);
	schedule("GameBase::setDamageLevel(" @ %objArkField @ ", 2);", %time, %objArkField);

	if(%damage) %objArkField = newObject("ArkField","StaticShape",ArkDamageField,true);
	else %objArkField = newObject("ArkField","StaticShape",ArkField,true);
	%pos = Vector::add(%position, "0 -2.5 0");
	GameBase::setTeam(%objArkField,GameBase::getTeam(%player));
	GameBase::setPosition(%objArkField,%pos);
	GameBase::setRotation(%objArkField,"0 0 0");
	Gamebase::setMapName(%objArkField,"Ark Field");
	GameBase::startFadeIn(%objArkField);
	addToSet("MissionCleanup", %objArkField);
	schedule("GameBase::setDamageLevel(" @ %objArkField @ ", 2);", %time, %objArkField);

	if(%damage) %objArkField = newObject("ArkField","StaticShape",ArkDamageField,true);
	else %objArkField = newObject("ArkField","StaticShape",ArkField,true);
	%pos = Vector::add(%position, "0 2.5 0");
	GameBase::setTeam(%objArkField,GameBase::getTeam(%player));
	GameBase::setPosition(%objArkField,%pos);
	GameBase::setRotation(%objArkField,"0 0 0");
	Gamebase::setMapName(%objArkField,"Ark Field");
	GameBase::startFadeIn(%objArkField);
	addToSet("MissionCleanup", %objArkField);
	schedule("GameBase::setDamageLevel(" @ %objArkField @ ", 2);", %time, %objArkField);

	if(%damage) %objArkField = newObject("ArkField","StaticShape",ArkDamageField,true);
	else %objArkField = newObject("ArkField","StaticShape",ArkField,true);
	%pos = Vector::add(%position, "0 2.5 5.0");
	GameBase::setTeam(%objArkField,GameBase::getTeam(%player));
	GameBase::setPosition(%objArkField,%pos);
	GameBase::setRotation(%objArkField,"1.57 0 0");
	Gamebase::setMapName(%objArkField,"Ark Field");
	GameBase::startFadeIn(%objArkField);
	addToSet("MissionCleanup", %objArkField);
	schedule("GameBase::setDamageLevel(" @ %objArkField @ ", 2);", %time, %objArkField);

	if(%damage) %objArkField = newObject("ArkField","StaticShape",ArkDamageField,true);
	else %objArkField = newObject("ArkField","StaticShape",ArkField,true);
	%pos = Vector::add(%position, "0 2.5 0");
	GameBase::setTeam(%objArkField,GameBase::getTeam(%player));
	GameBase::setPosition(%objArkField,%pos);
	GameBase::setRotation(%objArkField,"1.57 0 0");
	Gamebase::setMapName(%objArkField,"Ark Field");
	GameBase::startFadeIn(%objArkField);
	addToSet("MissionCleanup", %objArkField);
	schedule("GameBase::setDamageLevel(" @ %objArkField @ ", 2);", %time, %objArkField);

	playSound(SoundPickupBackpack,%position);
	playSound(ArkFieldOpen,%position);
	return true;
}

StaticShapeData ArkField
{
	shapeFile = "forcefield_5x5";
	debrisId = defaultDebrisSmall;
	maxDamage = 2.00;
	visibleToSensor = true;
	isTranslucent = true;
	description = "Ark Field";
};

function ArkField::onDestroyed(%this)
{
	StaticShape::objectiveDestroyed(%this);
	calcRadiusDamage(%this, $SnrapnelDamageType, 10,0.2,15,10,10,0.1,0.1,100,50); 
}

StaticShapeData ArkDamageField
{
	shapeFile = "forcefield_5x5";
	debrisId = defaultDebrisSmall;
	maxDamage = 2.0;
	visibleToSensor = true;
	isTranslucent = true;
	description = "Ark Field";
	explosionId = flashExpLarge;
};

function ArkDamageField::onDestroyed(%this)
{
	StaticShape::objectiveDestroyed(%this);
	calcRadiusDamage(%this, $ShrapnelDamageType, 10,0.2,15,10,10,0.1,0.1,100,50); 
}