
function cast_summonswordone(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,41);
	Projectile::spawnProjectile("battleswordone",%trans,%player,%vel);
}
function cast_summonswordtwo(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,41);
	Projectile::spawnProjectile("battleswordtwo",%trans,%player,%vel);
}
function cast_summonswordthree(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,41);
	Projectile::spawnProjectile("battleswordthree",%trans,%player,%vel);
}
function cast_summonswordfour(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,41);
	Projectile::spawnProjectile("battleswordfour",%trans,%player,%vel);
}
function cast_blizzardblizzardboltreal(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,41);
	Projectile::spawnProjectile("blizzardsummonbolt",%trans,%player,%vel);
}
function cast_blizzardblizzardboltfake(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,41);
	Projectile::spawnProjectile("blizzardsummonboltfake",%trans,%player,%vel);
}
function cast_truelight(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,41);
	Projectile::spawnProjectile("lightrocket",%trans,%player,%vel);
}
function cast_rocksummon(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,41);
	Projectile::spawnProjectile("rocksummonbolt",%trans,%player,%vel);
}
function cast_surge(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,41);
	Projectile::spawnProjectile("ssurge",%trans,%player,%vel);
}
function cast_waverly(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,41);
	Projectile::spawnProjectile("wave",%trans,%player,%vel);
}

function cast_cheemeyes(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,41);
	Projectile::spawnProjectile("cheeball",%trans,%player,%vel);
}
function cast_bioone(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,41);
	Projectile::spawnProjectile("bioone",%trans,%player,%vel);
}
function cast_abeam(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,41);
	Projectile::spawnProjectile("cheebeam",%trans,%player,%vel);
}
function cast_waterwaterwater(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,41);
	Projectile::spawnProjectile("waterfinal",%trans,%player,%vel);
}
function cast_waterwater(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,41);
	Projectile::spawnProjectile("watershottwo",%trans,%player,%vel);
}

function cast_rangerwind(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,41);
	Projectile::spawnProjectile("rangerwind",%trans,%player,%vel);
}

function cast_rangerfire(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,41);
	Projectile::spawnProjectile("rangerfire",%trans,%player,%vel);
}

function cast_rangerice(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,41);
	Projectile::spawnProjectile("rangerice",%trans,%player,%vel);
}

function cast_blastmeta(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,37);
	Projectile::spawnProjectile("drown",%trans,%player,%vel);
}

function cast_aqua(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,39);
	Projectile::spawnProjectile("drown",%trans,%player,%vel);
}

function cast_fc(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,39);
	Projectile::spawnProjectile("healer",%trans,%player,%vel);
}


function cast_flare(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,40);
	Projectile::spawnProjectile("flare",%trans,%player,%vel);
}

function cast_death(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,39);
	Projectile::spawnProjectile("painbolt",%trans,%player,%vel);
}

function cast_bliz(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,39);
	Projectile::spawnProjectile("blizzagashot",%trans,%player,%vel);
}

function cast_gravity(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,40);
	Projectile::spawnProjectile("Gravityshot",%trans,%player,%vel);
}

function cast_show(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,40);
	Projectile::spawnProjectile("showshockwave",%trans,%player,%vel);
}

function cast_shocklvone(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,40);
	Projectile::spawnProjectile("shocklvone",%trans,%player,%vel);
}

function cast_shocklvtwo(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,40);
	Projectile::spawnProjectile("shocklvtwo",%trans,%player,%vel);
}

function cast_shocklvthree(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,40);
	Projectile::spawnProjectile("shocklvthree",%trans,%player,%vel);
}


function cast_painbolt(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,39);
	Projectile::spawnProjectile("deathdealer",%trans,%player,%vel);
}

function cast_tech(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,42);
	Projectile::spawnProjectile("adminspell",%trans,%player,%vel);
}

function cast_tfist(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,38);
	Projectile::spawnProjectile("roguefist",%trans,%player,%vel);
}


function cast_fireball(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,39);
	Projectile::spawnProjectile("FireBolt",%trans,%player,%vel);
}

function cast_flame(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,39);
	Projectile::spawnProjectile("FlameBolt",%trans,%player,%vel);
}

function cast_iceball(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,39);
	Projectile::spawnProjectile("IceBallBolt",%trans,%player,%vel);
}

function cast_ice(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,39);
	Projectile::spawnProjectile("IceBolt",%trans,%player,%vel);
}

function cast_missile(%Client)//,%id)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,39);
	Projectile::spawnProjectile("MagicBolt",%trans,%player,%vel);
}

function cast_laser(%Client)//,%id)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,39);
	Projectile::spawnProjectile("beam",%trans,%player,%vel);
}

function cast_zap(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,39);
	Projectile::spawnProjectile("turretCharge",%trans,%player,%vel);
}

function cast_mortar(%Client)
{
	%player = Client::getOwnedObject(%Client);
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	$ClientData[%Client, UsingWeapon] = "-1";
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Player::setAnimation(%Client,39);
	Projectile::spawnProjectile("MortarTurretShell",%trans,%player,%vel);
}


//==================================================================================================================

function BeginCastSpell(%Client, %keyword, %spelltype) { //--1 is casted--0 is scroll

	%w1 = GetWord(%keyword, 0);
	%w2 = String::getSubStr(%keyword, String::len(%w1)+1, 99999);

	%i = $Spell::index[%w1];
	if(%i == "") {
		Client::sendMessage(%Client, $MsgWhite, "This spell seems unfamiliar to you.");
		return;
	}

	$ClientData[%Client, SpellType] = "";

	if(SkillCanUseSpell(%Client, %i, %spelltype, 1)) {

		Client::sendMessage(%Client, $MsgBeige, "Casting "@$Spell::name[%i]@".");

		%player = Client::getOwnedObject(%Client);

		if(GameBase::getLOSinfo(%player, $Spell::LOSrange[%i])) {
			%lospos = $los::position;
			%losobj = $los::object;
		}
		else {
			%lospos = "";
			%losobj = "";
		}

		$SpellCastStep[%Client] = 1;
		if(%Client.adminLevel < 5) {
			%tempManaCost = floor($Spell::manaCost[%i] / 2);
			refreshMANA(%Client, %tempManaCost);
		}

		playSound($Spell::startSound[%i], GameBase::getPosition(%Client));
		if(!Player::isAiControlled(%Client))
			Player::unmountItem(%player, $WeaponSlot);
	//	ooo(%Client);
		schedule("DoCastSpell("@%Client@", \""@%i@"\", \""@GameBase::getPosition(%Client)@"\", \""@%lospos@"\", \""@%losobj@"\", \""@%w2@"\", \""@%tempManaCost@"\");", $Spell::delay[%i], %player);
		return True;
	}

	return False;
}

function DoCastSpell(%Client, %index, %oldpos, %castPos, %castObj, %w2, %tempManaCost) {

	//echo("cl:"@%Client@" index:"@%index@" oldpos:"@%oldpos@" pos:"@%castPos@" Obj:"@%castObj@" w2:"@%w2@" ManaC:"@%tempManaCost);

	if(IsDead(%Client)) {
		$SpellCastStep[%Client] = "";
		return False;
	}

	%player = Client::getOwnedObject(%Client);

	if(Vector::getDistance(%oldpos, GameBase::getPosition(%Client)) > $SpellMovementGraceDistance) {
		Client::sendMessage(%Client, $MsgBeige, "You failed to cast the spell.");
		$SpellCastStep[%Client] = "";

		return False;
	}

	//group-list check
	if($Spell::groupListCheck[%index]) {
		%cl = Player::getClient(%castObj);
		if( !(IsInGroupList(%Client, %cl) && IsInGroupList(%cl, %Client)) ) {
			Client::sendMessage(%Client, $MsgBeige, "You are not part of the target's group.");
			$SpellCastStep[%Client] = "";

			return False;
		}
	}

	refreshMANA(%Client, %tempManaCost);
	$CanDoSpellDmg[%Client] = True;
	Schedule("$CanDoSpellDmg["@%Client@"] = \"\";", 3.1);
	$ClientData[%Client, SpellType] = $Spell::Type[%index];
	$ClientData[%Client, SpellDmg] = $Spell::damageValue[%index];
	if(!Player::isAiControlled(%Client))
		Player::unmountItem(%player, $WeaponSlot);
	%info = eval("SpellNum"@%index@"("@%Client@", \""@%castObj@"\", \""@%castPos@"\", \""@%w2@"\");");

	if(String::findSubStr(%info, "reflection 1") != -1)
	{
		%temp_id = %id;
		%id = %Client;
		%Client = %temp_id;
	}

	if(String::findSubStr(%info, "overrideEndSound 1") != -1)
		schedule("playSound("@$Spell::endSound[%index]@", \""@%castPos@"\");", 0.3);

	if((%HealId = String::getSubStr(%info, String::findSubStr(%info, "HealId"), 4)) != "")
		remoteEval(%HealId, "RefreshHPset", Fix(getHP(%HealId), %HealId, HP));

	if(String::findSubStr(%info, "returnFlag 1") != -1) {
		$SpellCastStep[%Client] = 2;
		schedule("SayReadyToCast("@%Client@");", $Spell::recoveryTime[%index]);
		return True;
	}
	else {
		SayReadyToCast(%Client);
		return False;
	}
}

function SayReadyToCast(%Client) {
	Client::sendMessage(%Client, 0, "You are ready to cast.");
	$SpellCastStep[%Client] = "";
}

function ooo(%Client) {
	%ccpos = GameBase::getPosition(%Client);
	%player = Client::getOwnedObject(%Client);
	%minrad = 0;
	%maxrad = 4;
	for(%s = 0; %s <= $Spell::delay[%i]; %s++)
	{
		%tempPos = RandomPositionXY(%minrad, %maxrad);

		%xPos = GetWord(%tempPos, 0) + GetWord(%ccpos, 0);
		%yPos = GetWord(%tempPos, 1) + GetWord(%ccpos, 1);
		%zPos = GetWord(%ccpos, 2) + (%s * 2);

		%newPos = %xPos@" "@%yPos@" "@%zPos;	//Bomb9
       	schedule("CreateAndDetBomb(\""@%Client@"\", \"Bomb3000\", \""@%newPos@"\", \"False\", \""@%i@"\", \"ooo\");", %s / 20);

		%tempPos = RandomPositionXY(%minrad, %maxrad);

		%xPos = GetWord(%tempPos, 0) + GetWord(%ccpos, 0);
		%yPos = GetWord(%tempPos, 1) + GetWord(%ccpos, 1);
		%zPos = GetWord(%ccpos, 2) + (%s * 2);

		%newPos = %xPos@" "@%yPos@" "@%zPos;

		schedule("CreateAndDetBomb(\""@%Client@"\", \"Bomb3002\", \""@%newPos@"\", \"False\", \""@%i@"\", \"ooo\");", %s / 15);

		%tempPos = RandomPositionXY(%minrad, %maxrad);

		%xPos = GetWord(%tempPos, 0) + GetWord(%ccpos, 0);
		%yPos = GetWord(%tempPos, 1) + GetWord(%ccpos, 1);
		%zPos = GetWord(%ccpos, 2) + (%s * 2);

		%newPos = %xPos@" "@%yPos@" "@%zPos;

		schedule("CreateAndDetBomb(\""@%Client@"\", \"Bomb3003\", \""@%newPos@"\", \"False\", \""@%i@"\", \"ooo\");", %s / 10);
	}
}

function CreateAndDetBomb(%Client, %b, %castPos, %doDamage, %index, %opt) {
//echo("CreateAndDetBomb("@%Client@", "@%b@", "@%castPos@", "@%doDamage@", "@%index@", "@%opt@")");
	%player = Client::getOwnedObject(%Client);

	%bomb = newObject("", "Mine", %b);

	addToSet("MissionCleanup", %bomb);

	GameBase::Throw(%bomb, %player, 0, false);
	GameBase::setPosition(%bomb, %castPos);

	if(%doDamage)
		SpellRadiusDamage(%Client, %castPos, %index);

	if(%opt == "ooo")
		playSound(LoopSP, %castPos);
	else
		playSound($Spell::endSound[%index], %castPos);
}

function SpellDamage(%Client, %targetId, %index) {
	GameBase::virtual(%targetId, "onDamage", $SpellDamageType, $Spell::damageValue[%index], "0 0 0", "0 0 0", "0 0 0", "torso", "front_right", %Client);
}

function SpellRadiusDamage(%Client, %pos, %index) {

	%percMin = 5;
	%percMax = 100;

	%list = GetEveryoneIdList();

	for(%i = 0; GetWord(%list, %i) != -1; %i++) {
		%id = GetWord(%list, %i);

		%dist = Vector::getDistance(%pos, GameBase::getPosition(%id));

		if(%dist <= $Spell::radius[%index]) {
			%newDamage = SpellCalcRadiusDamage(%dist, $Spell::radius[%index], $Spell::damageValue[%index], %percMin, %percMax);

			GameBase::virtual(%id, "onDamage", $SpellDamageType, %newDamage, "0 0 0", "0 0 0", "0 0 0", "torso", "front_right", %Client);
		}
	}
}

function SpellCalcRadiusDamage(%dist, %radius, %dmg, %percMin, %percMax) {

	if(%radius == "")
		%radius = 10;

	%newdmg = %dmg - (%dist * (%dmg / %radius));

	%p = (%newdmg * 100) / %dmg;

	if(%p < %percMin)
		%p = %percMin;
	else if(%p > %percMax)
		%p = %percMax;

	%newdmg = (%p * %dmg) / 100;

	return %newdmg;
}

function StaticDoorForceField::onCollision(%this, %object) { //for FWALL
	%Client = Player::getClient(%object);

	if(GameBase::getTeam(%Client) == GameBase::getTeam(%this))
		GameBase::setPosition(%Client,GameBase::getPosition(%this));
	else {
		for(%mynum=1;%mynum<$MYmaxclients;%mynum++) {
			if($fwallid[%mynum]==%this) {
				Item::setVelocity(%Client,GetWord(Item::getVelocity(%Client),0)@" "@GetWord(Item::getVelocity(%Client),1)@" "@-($fwallttl[%mynum]));
				break;
			}
		}
	}
}

function GetBestSpell(%Client, %type, %semiRandomSpell) {
	%bestSpell = 26;

	if(%type==-1)
	{//9,10
		%bestSpell=9;
		if(getMANA(%Client) >= $Spell::manaCost[10])
		{
			if(getRandom()*100>50)
			{
				%bestSpell=10;
			}
		}
	}
	else
	{//1,5,6,7,16,17,18,19,20,21,8
		%bestSpell=26;
		%r=floor(getRandom()*12)+1;
		if(%r==14)
			%r=20;
		if(%r==29)
			%r=14;
		else if(%r==20)
			%r=29;
		else if(%r==39)
			%r=45;
		else if(%r==41)
			%r=41;
		else if(%r==45)
			%r=30;
		else if(%r==26)
			%r=22;     //you were using %r=4 for a test BestChiefSpell
		else if(%r==16)  //that is how you set the var
			%r=30;     //so this func didnt work for shit before!!
		else if(%r==26)
			%r=22;
		else if(%r==16)
			%r=39;
		else if(%r==46)
			%r=46;
		else
			%r=5;
		if(getMANA(%Client) >= $Spell::manaCost[%r])
		{
			%bestSpell=%r;
		}
	}
	return %bestSpell;
}
function BestChiefSpell(%Client, %type, %semiRandomSpell)
{
	%bestSpell = 4;

	if(%type==-1)
	{//9,10
		%bestSpell=4;
		if(getMANA(%Client) >= $Spell::manaCost[10])
		{
			if(getRandom()*100>50)
			{
				%bestSpell=28;
			}
		}
	}
	else
	{//1,5,6,7,16,17,18,19,20,21,8
		%bestSpell=26;
		%r=floor(getRandom()*12)+1;
		if(%r==4)
			%r=28;
		if(%r==4)
			%r=28;
		else if(%r==28)
			%r=4;
		else if(%r==4)
			%r=28;
		else if(%r==4)
			%r=4;
		else if(%r==28)
			%r=28;
		else if(%r==28)
			%r=28;     //you were using %r=4 for a test BestChiefSpell
		else if(%r==28)  //that is how you set the var
			%r=4;     //so this func didnt work for shit before!!
		else if(%r==28)
			%r=28;
		else if(%r==28)
			%r=4;
		else if(%r==48)
			%r=28;
		else
			%r=4;
		if(getMANA(%Client) >= $Spell::manaCost[%r])
		{
			%bestSpell=%r;
		}
	}
	return %bestSpell;
}
function GetBestSpell(%Client, %type, %semiRandomSpell)
{

	%wdelay = 10;	//weights
	%wrecov = 0.5;

	%bestSpell = -1;
	%backupSpell = "";
	%highest = 0.1;

	for(%i = 1; $Spell::keyword[%i] != ""; %i++)
	{
		if(getFinalLVL(%Client) >= $Spell::minLevel[%i])
		{
			if($MANA[%Client] >= $Spell::manaCost[%i])
			{
				%d = ( ($Spell::delay[%i] / %wdelay) + ($Spell::recoveryTime[%i] / %wrecov) );
				%x = (100 / %d) * $Spell::refVal[%i];
				%v =  %x * %type;

				if(%semiRandomSpell)
				{
					%r = getRandom() * 100;
					%rr = getRandom() * 100;
				}
				else
				{
					%r = 1;
					%rr = 0;
				}

				if(%v > %highest)
				{
					if(%r > %rr)
					{
						%bestSpell = %i;
						%highest = %v;
					}
					else
						%backupSpell = %i;
				}
			}
		}
	}
	if(%bestSpell == -1 && %backupSpell != "")
		%bestSpell = %backupSpell;

	return %bestSpell;
}
