
function MeleeAttack(%player, %length, %weapon) {
	//dbecho($dbechoMode, "MeleeAttack(" @ %player @ ", " @ %length @ ")");

	%cl = Player::getClient(%player);
	if(IsDead(%cl))
		return;

	if(!CheckMountWeapon(%cl, %weapon))
		return;

	//remoteEval(%cl, "SetDelayVar", $ItemData[%weapon, Delay]);
	if(!playSound($ItemData[%weapon, Sound], GameBase::getPosition(%cl)))
		echo("SoundData "@$ItemData[%weapon, Sound]@" undefined. Item "@%weapen@" (MeleeAttack)");

	$los::object = "";
	if(GameBase::getLOSinfo(%player, %length)) {
		%obj = getObjectType($los::object);

		if(%obj == "Player") {
			GameBase::virtual($los::object, "onDamage", $ItemData[%weapon, DamageType], 1.0, "0 0 0", "0 0 0", "0 0 0", "torso", "front_right", %cl, %weapon);
		}
		else if(%obj == "StaticShape" && Object::getName($los::object) == "MagicWall") 
		{
			if($los::object.owner == client::getname(%cl))
				$los::object.hp-=10;
			else
				$los::object.hp--;

			if($los::object.hp > 0)
				remoteEval(%cl,"ATKText", "<JC>"@$los::object.owner@"'s magic wall has "@$los::object.hp@" hp.", true);
			else
				Item::Pop($los::object);
		}
	}
}

function ProjectileAttack(%player, %weapon) {
	%Client = Player::getClient(%player);
	%proj = $LoadedProjectile[%Client, %weapon];
//echo(%Client@" proj "@%proj@" | weap "@%weapon);
	if(%proj == "") {
		Client::sendMessage(%Client, 0, "No ammo loaded for "@$ItemData[%weapon, Name]@".");
		return;
	}
	if(Client::getItemCount(%Client, %proj) <= 0) {
		if(String::findSubStr($Quiver[%Client], %proj) != -1) { //Has an quiver filled with this item

			$Quiver[%Client] = String::replace($Quiver[%Client], %proj, "FreeSlot");
			Client::addItemCount(%player, %proj, 99);
			Client::sendMessage(%Client, 0, "You get out an quiver filled with "@$ItemData[%proj, Name]@".");
		}
		else
			Client::sendMessage(%Client, 0, "Out of arrows.");
		return;
	}
	if(!playSound($ItemData[%weapon, Sound], GameBase::getPosition(%Client)))
		echo("SoundData "@$ItemData[%weapon, Sound]@" undefined. Item "@%weapon@" (ProjectileAttack)");

	%vel = $ItemData[%weapon, Range];
	%zoffset = 0.44;

	%arrow = newObject("", "Item", $ItemData[%proj, shape], 1, false);
	%arrow.owner = %Client;
	%arrow.delta = 1;
	%arrow.weapon = %weapon;
	%arrow.RealItem = %proj; //NEEDED! (Itemevents.cs)

	addToSet("MissionCleanup", %arrow);
  	schedule("Item::Pop(" @ %arrow @ ");", 30, %arrow);

	//double-check stuff
	$ProjectileDoubleCheck[%arrow] = True;
	schedule("$ProjectileDoubleCheck[" @ %arrow @ "] = \"\";", 1.5, %arrow);

	%rot = GameBase::getRotation(%Client);
	%newrot = (GetWord(%rot, 0) - %zoffset) @ " " @ GetWord(%rot, 1) @ " " @ GetWord(%rot, 2);

	GameBase::setRotation(%Client, %newrot);
	GameBase::throw(%arrow, Client::getOwnedObject(%Client), %vel, false);
	GameBase::setRotation(%arrow, %rot);
	GameBase::setRotation(%Client, %rot);

	Client::addItemCount(%Client, %proj, -1);
}
