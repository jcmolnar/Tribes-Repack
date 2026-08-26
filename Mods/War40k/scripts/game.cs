function godmode(%client) 
{
	%player=client::getownedobject(%client);
	if(%player!=-1) 
	{
		GameBase::setRechargeRate(%player,10000);
		GameBase::setAutoRepairRate(%player,10000);
		%player.shieldStrength=0.05;
	}
}

function tele(%clientlos,%clientfrom) 
{
	%playerlos=client::getownedobject(%clientlos);
	%playerfrom=client::getownedobject(%clientfrom);
	if(%playerlos!=-1&&%playerfrom!=-1) 
	{
		if(GameBase::getLOSInfo(%playerlos,5000)==True)
		{
			%posto=$los::position;
			%posto=Vector::add(%posto,"0 0 2");
			gamebase::setposition(%playerfrom,%posto);
		}
	}
}

exec("comchat.cs");
$GuiModePlay = 1;
$GuiModeCommand = 2;
$GuiModeVictory = 3;
$GuiModeInventory = 4;
$GuiModeObjectives = 5;
$GuiModeLobby = 6;
$SensorNetworkEnabled = TRUE;

$SimTerrainObjectType = 1 << 1;
$SimInteriorObjectType = 1 << 2;
$SimPlayerObjectType = 1 << 7;
$MineObjectType = 1 << 26;
$MoveableObjectType = 1 << 22;
$VehicleObjectType = 1 << 29;
$StaticObjectType = 1 << 23;
$ItemObjectType = 1 << 21;

//$WebDamageType = #; (whatever # comes next)
// Death messages
$numDeathMsgs = 5;
//web gun
$deathMsg[$WebDamageType, 0] = "%2 gets trapped by %1.";
$deathMsg[$WebDamageType, 1] = "%2 dies in %1's box.";
$deathMsg[$WebDamageType, 2] = "%1 kills %2 with a skillful trap.";
$deathMsg[$WebDamageType, 3] = "%1 boxes up %2.";
$deathMsg[$WebDamageType, 4] = "%2 dies in the blue box of %1.";
//landing damage
$deathMsg[$LandingDamageType, 0] = "%2 embeds %4 face into the ground.";
$deathMsg[$LandingDamageType, 1] = "%2 smashes into the ground.";
$deathMsg[$LandingDamageType, 2] = "%2 feels %4 bones shatter.";
$deathMsg[$LandingDamageType, 3] = "%2 has fallen...and can't get up.";
$deathMsg[$LandingDamageType, 4] = "%2 breaks %4-self";
//impact
$deathMsg[$ImpactDamageType, 0] = "%1 makes quite an impact on %2.";
$deathMsg[$ImpactDamageType, 1] = "%2 becomes the victim of a fly-by from %1.";
$deathMsg[$ImpactDamageType, 2] = "%2 leaves %4 chest cavity on %1's fender.";
$deathMsg[$ImpactDamageType, 3] = "%1 demonstrates to %2 the fine art of piloting'";
$deathMsg[$ImpactDamageType, 4] = "%1 crunches %2.";
//bullets
$deathMsg[$BulletDamageType, 0] = "%1 rents holes in %2's plasteel armour.";
$deathMsg[$BulletDamageType, 1] = "%1 gives %2 too many holes to cover with %4 hands.";
$deathMsg[$BulletDamageType, 2] = "%1 blasts %2 a hole in %4 chest.";
$deathMsg[$BulletDamageType, 3] = "%1 rips chunks of %2's flesh with white hot rounds of depleted uranium.";
$deathMsg[$BulletDamageType, 4] = "%1 leaves %2 a bleeding mess on the ground.";
//poison
$deathMsg[$EnergyDamageType, 0] = "%2's heart shuts down from %1's poison.";
$deathMsg[$EnergyDamageType, 1] = "%2 gets a lethal injection from %1.";
$deathMsg[$EnergyDamageType, 2] = "%2 falls victim to a poison dart from %1.";
$deathMsg[$EnergyDamageType, 3] = "%2 could not find the vaccine for %1's poison in time.";
$deathMsg[$EnergyDamageType, 4] = "%2 gasps %4 last breath in front of %1.";
//flamer
$deathMsg[$PlasmaDamageType, 0] = "%2 screams at %1 as %4 body burns.";
$deathMsg[$PlasmaDamageType, 1] = "%1 happily scorches %2 with a gout of flame.";
$deathMsg[$PlasmaDamageType, 2] = "%1 burns %2 black all over";
$deathMsg[$PlasmaDamageType, 3] = "%1 lets %2 burn.";
$deathMsg[$PlasmaDamageType, 4] = "%2 feels %4 flesh burning as %1 watches happily.";
//explosion
$deathMsg[$ExplosionDamageType, 0] = "%2 gets torn asunder by %1.";
$deathMsg[$ExplosionDamageType, 1] = "%1 paints the walls with %2's blood.";
$deathMsg[$ExplosionDamageType, 2] = "%2 gets blown to pieces by %1.";
$deathMsg[$ExplosionDamageType, 3] = "%1 shows off %3 mad skills of body dismemberment on %2.";
$deathMsg[$ExplosionDamageType, 4] = "%2 explodes due to %1's good aim.";
//grenades and shit
$deathMsg[$ShrapnelDamageType, 0] = "%1 blows %2 up real good.";
$deathMsg[$ShrapnelDamageType, 1] = "%2 gets a taste of %1's brutal nature.";
$deathMsg[$ShrapnelDamageType, 2] = "%1 gives %2 a fatal dose of shrapnel.";
$deathMsg[$ShrapnelDamageType, 3] = "%2 does not throw %1's grenade back in time.";
$deathMsg[$ShrapnelDamageType, 4] = "%1 shreds %2.";
//laser
$deathMsg[$LaserDamageType, 0] = "%1 annihilates %2 with %3 las beam.";
$deathMsg[$LaserDamageType, 1] = "%1 fells %2 with a skillfully aimed laser blast.";
$deathMsg[$LaserDamageType, 2] = "%2 was decimated by %1.";
$deathMsg[$LaserDamageType, 3] = "%2 feels the righteous might of %1's laser.";
$deathMsg[$LaserDamageType, 4] = "%2 gets a hole burned in %4 body by %1.";
//morter
$deathMsg[$MortarDamageType, 0] = "%1 sends %2 into the silence of death.";
$deathMsg[$MortarDamageType, 1] = "%2 is blown to bloody bits by %1's bomb.";
$deathMsg[$MortarDamageType, 2] = "%1 smiles as %2 explodes into gory bits.";
$deathMsg[$MortarDamageType, 3] = "%1's explosive toy took out %2.";
$deathMsg[$MortarDamageType, 4] = "%2 falls all to pieces for %1.";
//particle shot
$deathMsg[$BlasterDamageType, 0] = "%2 gets capped by %1.";
$deathMsg[$BlasterDamageType, 1] = "%2 succumbs to %1's relentless attack.";
$deathMsg[$BlasterDamageType, 2] = "%1 gives %2 a taste o' death.";
$deathMsg[$BlasterDamageType, 3] = "%2 meets %1's trusty sidearm.";
$deathMsg[$BlasterDamageType, 4] = "%1 greets %2 with extreme prejudice.";
//electricity
$deathMsg[$ElectricityDamageType, 0] = "%2 gets zapped by %1.";
$deathMsg[$ElectricityDamageType, 1] = "%1 gives %2 a nasty jolt.";
$deathMsg[$ElectricityDamageType, 2] = "%2 gets a real shock out of meeting %1.";
$deathMsg[$ElectricityDamageType, 3] = "%1 short-circuits %2's systems.";
$deathMsg[$ElectricityDamageType, 4] = "%2 is turned into a crispy critter by %1.";
//elevator 
$deathMsg[$CrushDamageType, 0] = "%2 didn't stay away from the moving parts.";
$deathMsg[$CrushDamageType, 1] = "%2 deserves being mocked. Elevator deaths are funny.";
$deathMsg[$CrushDamageType, 2] = "%2 gets smushed flat.";
$deathMsg[$CrushDamageType, 3] = "%2 gets caught in the machinery.";
$deathMsg[$CrushDamageType, 4] = "%2 gets ground in the gears.";
//items blowing up
$deathMsg[$DebrisDamageType, 0] = "%2 is a victim among the wreckage.";
$deathMsg[$DebrisDamageType, 1] = "%2 is killed by debris.";
$deathMsg[$DebrisDamageType, 2] = "%2 becomes a victim of collateral damage.";
$deathMsg[$DebrisDamageType, 3] = "%2 got too close to the exploding stuff.";
$deathMsg[$DebrisDamageType, 4] = "%2 feels the rain of debris.";
//missle
$deathMsg[$MissileDamageType, 0] = "%2 gets blown apart by %1's rocket.";
$deathMsg[$MissileDamageType, 1] = "%1 purifies %2 with the blast of a missile.";
$deathMsg[$MissileDamageType, 2] = "%2 screams in agony as %1's missile eviscerates him.";
$deathMsg[$MissileDamageType, 3] = "%2 feels the smash of a rocket from %1.";
$deathMsg[$MissileDamageType, 4] = "%2 feels his head explode from %1's rocket.";
//mines
$deathMsg[$MineDamageType, 0] = "%1 blows %2 up real good.";
$deathMsg[$MineDamageType, 1] = "%2 stepped on %1's mine.";
$deathMsg[$MineDamageType, 2] = "%1 gives %2 a fatal concussion.";
$deathMsg[$MineDamageType, 3] = "%2 sees his legs removed from %1's mine.";
$deathMsg[$MineDamageType, 4] = "%2 stepped in %1's death trap.";
//haywire
$deathMsg[$FlashDamageType, 0] = "%1 blows %2 up real good.";
$deathMsg[$FlashDamageType, 1] = "%2 gets a taste of %1's explosive temper.";
$deathMsg[$FlashDamageType, 2] = "%1 gives %2 a fatal concussion.";
$deathMsg[$FlashDamageType, 3] = "%2 never saw it coming from %1.";
$deathMsg[$FlashDamageType, 4] = "%2 gets flashed by %1.";
//sniper
$deathMsg[$SniperDamageType, 0] = "%1 blows %2's brain out of %4 skull.";
$deathMsg[$SniperDamageType, 1] = "%1 takes %2 on a snipe hunt.";
$deathMsg[$SniperDamageType, 2] = "%2 mutters,F---in %1... F---in snipers....";
$deathMsg[$SniperDamageType, 3] = "%2 stayed in %1's aim for one second too long.";
$deathMsg[$SniperDamageType, 4] = "%2 wasn't fast enough to dodge %1's snipe attack.";
//shotgun
$deathMsg[$ShellDamageType, 0] = "%2 feels the spread from %1's shotgun.";
$deathMsg[$ShellDamageType, 1] = "%2 gets obliterated by %1's shells.";
$deathMsg[$ShellDamageType, 2] = "%2 fell victim to a shotgun blast from %1.";
$deathMsg[$ShellDamageType, 3] = "%2 was slaughtered by %1.";
$deathMsg[$ShellDamageType, 4] = "%2 coughs up shotgun shells fed by %1.";
//reaper
$deathMsg[$ReaperDamageType, 0] = "%1 reaped the hell out of %2.";
$deathMsg[$ReaperDamageType, 1] = "%2 was blasted by %1.";
$deathMsg[$ReaperDamageType, 2] = "%2 got reaper-raped by %1.";
$deathMsg[$ReaperDamageType, 3] = "%2 recieves a reaper burst from %1.";
$deathMsg[$ReaperDamageType, 4] = "%2 got hit hard by %1.";
//meltagun
$deathMsg[$MeltaDamageType, 0] = "%2 got microwaved by %1.";
$deathMsg[$MeltaDamageType, 1] = "%1 melts %2.";
$deathMsg[$MeltaDamageType, 2] = "%2 was taught about home cookin, by %1.";
$deathMsg[$MeltaDamageType, 3] = "%2 was bubbled and melted by %1.";
$deathMsg[$MeltaDamageType, 4] = "%2 got pressure cooked by %1.";
//Demon Damage
$deathMsg[$DDamageType, 0] = "%2 was sucked into %1's vortex.";
$deathMsg[$DDamageType, 1] = "%1 introduced physics to %2.";
$deathMsg[$DDamageType, 2] = "%1 gave %2 a lesson in distortion.";
$deathMsg[$DDamageType, 3] = "%1 displayed %3 explosive might upon %2.";
$deathMsg[$DDamageType, 4] = "%2 was destroyed by %1.";
//plasma
$deathMsg[$FlamerDamageType, 0] = "%2 is cooked medium rare by %1.";
$deathMsg[$FlamerDamageType, 1] = "%1 melts %2 at 30,000 degrees centigrade.";
$deathMsg[$FlamerDamageType, 2] = "%2 feels a little hot under the hood because of %1.";
$deathMsg[$FlamerDamageType, 3] = "%2 fries extra crispy from %1's plasma.";
$deathMsg[$FlamerDamageType, 4] = "%1 ignites %2.";
//shuriken
$deathMsg[$ShurikenDamageType, 0] = "%2 catches a shuriken from %1.";
$deathMsg[$ShurikenDamageType, 1] = "%2 falls to the shuriken of %1.";
$deathMsg[$ShurikenDamageType, 2] = "%1 shows %3 abilities off to %2.";
$deathMsg[$ShurikenDamageType, 3] = "%2 feels the sting of %1's shuriken in %4 throat.";
$deathMsg[$ShurikenDamageType, 4] = "%1 kills %2 to satisfy Kaela Mensha Khaine";
//fusion
$deathMsg[$DeathDamageType, 0] = "%2 enjoys a shower of fusion-charged death from %1.";
$deathMsg[$DeathDamageType, 1] = "%1's fusion found %2's head.";
$deathMsg[$DeathDamageType, 2] = "%2 learns from %1 to die with grace.";
$deathMsg[$DeathDamageType, 3] = "%1 nails %2 in the danglies with a boot made of fusion.";
$deathMsg[$DeathDamageType, 4] = "%2 falls victim to charged particles from %1.";
//psi
$deathMsg[$PsiDamageType, 0] = "%2 gets real psyched up by %1.";
$deathMsg[$PsiDamageType, 1] = "%1's bitch-slaps %2 real good with psychic love.";
$deathMsg[$PsiDamageType, 2] = "%2 is no longer a psychic sceptic because of %1.";
$deathMsg[$PsiDamageType, 3] = "%1 smokes %2 with %3 psychic skills.";
$deathMsg[$PsiDamageType, 4] = "%2 is schooled hardcore in psionics from %1.";
//biotoxin
$deathMsg[$ChemDamageType, 0] = "%2 is a test subject for %1's germ doctors.";
$deathMsg[$ChemDamageType, 1] = "%2 spazzes out for %1.";
$deathMsg[$ChemDamageType, 2] = "%1 splashed %2 with deadly ichor.";
$deathMsg[$ChemDamageType, 3] = "%2 collapses in front of %1, a heap of dissolving flesh.";
$deathMsg[$ChemDamageType, 4] = "%1 shows %2 the wonders of bioengineering.";
//krak
$deathMsg[$KrakenDamageType , 0] = "%2 gets Krak'in for %1.";
$deathMsg[$KrakenDamageType , 1] = "%2 recieves a piercing blow from %1.";
$deathMsg[$KrakenDamageType , 2] = "%1 kraks %2 right up the keister.";
$deathMsg[$KrakenDamageType , 3] = "%2 gets kraked in the head by %1.";
$deathMsg[$KrakenDamageType , 4] = "%1 smokes %2 with a kraken shell.";
//acid
$deathMsg[$AcidDamageType, 0] = "%1 melts %2's flesh to the bone with %3 acid.";
$deathMsg[$AcidDamageType, 1] = "%2 feels %4 bones distintegrate while %1 laughs.";
$deathMsg[$AcidDamageType, 2] = "%2 met the fury of %1's acidic rounds.";
$deathMsg[$AcidDamageType, 3] = "%1 shocked %2 real bad, with a skin melting backhand.";
$deathMsg[$AcidDamageType, 4] = "%2 was molten into a puddle of bloody goop by %1.";
//suicide
$deathMsg[-2,0] = "%1 killed %2 self like a coward.";
$deathMsg[-2,1] = "%1 wanted to make sure %2 gun was loaded.";
$deathMsg[-2,2] = "%1 kills %2 own worthless self.";
$deathMsg[-2,3] = "%1 dies by %2 own hand.";
$deathMsg[-2,4] = "%1 gets bored of living.";

function remotePlayMode(%clientId) 
{
	if(!%clientId.guiLock) 
	{
		remoteSCOM(%clientId, -1);
		Client::setGuiMode(%clientId, $GuiModePlay);
	}
}

function remoteCommandMode(%clientId) 
{
	if(!%clientId.guiLock) 
	{
		remoteSCOM(%clientId, -1);
		if(%clientId.observerMode != "pregame") checkControlUnmount(%clientId);
		Client::setGuiMode(%clientId, $GuiModeCommand);
	}
}

function remoteInventoryMode(%clientId) 
{
	if(!%clientId.guiLock && !Observer::isObserver(%clientId)) 
	{
		remoteSCOM(%clientId, -1);
		Client::setGuiMode(%clientId, $GuiModeInventory);
	}
}

function remoteObjectivesMode(%clientId) 
{
	if(!%clientId.guiLock) 
	{
		remoteSCOM(%clientId, -1);
		Client::setGuiMode(%clientId, $GuiModeObjectives);
	}
}

function remoteScoresOn(%clientId) 
{
	if(!%clientId.menuMode) Game::menuRequest(%clientId);
}

function remoteScoresOff(%clientId) 
{
	Client::cancelMenu(%clientId);
}

function remoteToggleCommandMode(%clientId) 
{
	if (Client::getGuiMode(%clientId) != $GuiModeCommand) remoteCommandMode(%clientId);
	else remotePlayMode(%clientId);
}

function remoteToggleInventoryMode(%clientId) 
{
	if (Client::getGuiMode(%clientId) != $GuiModeInventory) remoteInventoryMode(%clientId);
	else remotePlayMode(%clientId);
}

function remoteToggleObjectivesMode(%clientId) 
{
	if (Client::getGuiMode(%clientId) != $GuiModeObjectives) remoteObjectivesMode(%clientId);
	else remotePlayMode(%clientId);
}

function Time::getMinutes(%simTime) 
{
	return floor(%simTime / 60);
}

function Time::getSeconds(%simTime) 
{
	return %simTime % 60;
}

function Game::pickRandomSpawn(%team) 
{
	%group = nameToID("MissionGroup/Teams/team" @ %team @ "/DropPoints/Random");
	%count = Group::objectCount(%group);
	if(!%count) return -1;
	%spawnIdx = floor(getRandom() * (%count - 0.1));
	%value = %count;
	for(%i = %spawnIdx; %i < %value; %i++) 
	{
		%set = newObject("set",SimSet);
		%obj = Group::getObject(%group, %i);
		if(containerBoxFillSet(%set,$SimPlayerObjectType|$VehicleObjectType,GameBase::getPosition(%obj),2,2,4,0) == 0) 
		{
			deleteObject(%set);
			return %obj;
		}
		if(%i == %count - 1) 
		{
			%i = -1;
			%value = %spawnIdx;
		}
		deleteObject(%set);
	}
	return false;
}

function Game::pickStartSpawn(%team) 
{
	%group = nameToID("MissionGroup\\Teams\\team" @ %team @ "\\DropPoints\\Start");
	%count = Group::objectCount(%group);
	if(!%count) return -1;
	%spawnIdx = $lastTeamSpawn[%team] + 1;
	if(%spawnIdx >= %count) %spawnIdx = 0;
	$lastTeamSpawn[%team] = %spawnIdx;
	return Group::getObject(%group, %spawnIdx);
}

function Game::pickTeamSpawn(%team, %respawn) 
{
	if(%respawn) return Game::pickRandomSpawn(%team);
	else 
	{
		%spawn = Game::pickStartSpawn(%team);
		if(%spawn == -1) return Game::pickRandomSpawn(%team);
		return %spawn;
	}
}

function Game::pickObserverSpawn(%client) 
{
	%group = nameToID("MissionGroup\\ObserverDropPoints");
	%count = Group::objectCount(%group);
	if(%group == -1 || !%count) %group = nameToID("MissionGroup\\Teams\\team" @ Client::getTeam(%client) @ "\\DropPoints\\Random");
	%count = Group::objectCount(%group);
	if(%group == -1 || !%count) %group = nameToID("MissionGroup\\Teams\\team0\\DropPoints\\Random");
	%count = Group::objectCount(%group);
	if(%group == -1 || !%count) return -1;
	%spawnIdx = %client.lastObserverSpawn + 1;
	if(%spawnIdx >= %count) %spawnIdx = 0;
	%client.lastObserverSpawn = %spawnIdx;
	return Group::getObject(%group, %spawnIdx);
}

function UpdateClientTimes(%time) 
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) remoteEval(%cl, "setTime", -%time);
}

function Game::notifyMatchStart(%time) 
{
	messageAll(0, "Combat begins in " @ %time @ " seconds.");
	UpdateClientTimes(%time);
	if($War40k::Meteor) schedule("MeteorChance();", %time + 600);
}

function Game::startMatch() 
{
	$matchStarted = true;
	$missionStartTime = getSimTime();
	messageAll(0, "Let the carnage begin!");
	Game::resetScores();
	%numTeams = getNumTeams();
	for(%i = 0; %i < %numTeams; %i = %i + 1) 
	{
		if($TeamEnergy[%i] != "Infinite") schedule("replenishTeamEnergy(" @ %i @ ");", $secTeamEnergy);
	}
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) 
	{
		if(%cl.observerMode == "pregame") 
		{
			%cl.observerMode = "";
			Client::setControlObject(%cl, Client::getOwnedObject(%cl));
		}
		Game::refreshClientScore(%cl);
	}
	Game::checkTimeLimit();
}

function Game::pickPlayerSpawn(%clientId, %respawn) 
{
	return Game::pickTeamSpawn(Client::getTeam(%clientId), %respawn);
}

function Game::playerSpawn(%clientId, %respawn) 
{
	if (!$ghosting) return false;
	if($Server::TourneyMode && %respawn) 
	{
		if(%clientId.lives == "") %clientId.lives = $War40k::Warlives;
		if(%clientId.lives < 1)
		{
			Observer::enterObserverMode(%clientId);
			TourneyCheckEnd();
			%clientId.lives = $War40k::Warlives;
			return false;
		}
		%clientID.lives--;
	}
	Client::clearItemShopping(%clientId);
	%spawnMarker = Game::pickPlayerSpawn(%clientId, %respawn);
	if (!%respawn) bottomprint(%clientId, $Welcome @ "<f0>Connection#: <f1>" @ %clientId.num @ "<f0>Server Resets: <f1>" @ $Stats::ResetCount @ "\n" @ "<f0>Mission: <f1>" @ $missionName @ "<f0>Mission Type: <f1>" @ $Game::missionType @ "\n" @ "<f0>Press <f1>'O'<f0> for specific objectives.", 10);
	if(%spawnMarker) 
	{
		%clientId.guiLock = "";
		%clientId.dead = "";
		if(%spawnMarker == -1) 
		{
			%spawnPos = "0 0 300";
			%spawnRot = "0 0 0";
		}
		else 
		{
			%spawnPos = GameBase::getPosition(%spawnMarker);
			%spawnRot = GameBase::getRotation(%spawnMarker);
		}
		%armor = $DefaultArmor[Client::getGender(%clientId)];
		%pl = spawnPlayer(%armor, %spawnPos, %spawnRot);
		echo("SPAWN: cl:" @ %clientId @ " pl:" @ %pl @ " marker:" @ %spawnMarker @ " armor:" @ %armor);
		if(%pl != -1) 
		{
			GameBase::setTeam(%pl, Client::getTeam(%clientId));
			Client::setOwnedObject(%clientId, %pl);
			Game::playerSpawned(%pl, %clientId, %armor, %respawn);
			if($matchStarted) Client::setControlObject(%clientId, %pl);
			else 
			{
				%clientId.observerMode = "pregame";
				Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
				Observer::setOrbitObject(%clientId, %pl, 3, 3, 3);
			}
		}
		return true;
	}
	else 
	{
		Client::sendMessage(%clientId,0,"Sorry No Respawn Positions Are Empty - Try again later ");
		return false;
	}
}

function Game::playerSpawned(%pl, %clientId, %armor) 
{
	%this.mindamage = 0;
	%clientId.spawn= 1;
	%max = $TotalItems;
	if ($Server::RaceOption == "") $Server::RaceOption = 0; 
	%team = (client::getteam(%clientId)+1)%2; //Team Specific Races
	if ($Server::RaceOption == 1) %team = 1; // Space Marines Only
	if ($Server::RaceOption == 2) %team = 0; // Eldar Only
	if ($Server::RaceOption == 3) %team = 2; // All available
	for(%i = 0; (%item = $spawnBuyList[%team, %i]) != ""; %i++) 
	{
		buyItem(%clientId,%item);
		if(%item.className == Weapon) %clientId.spawnWeapon = %item;
	}
	%clientId.spawn= "";
	if(%clientId.spawnWeapon != "") 
	{
		Player::useItem(%pl,%clientId.spawnWeapon);
		%clientId.spawnWeapon="";
	}
}

function Game::autoRespawn(%client) 
{
	if(%client.dead == 1) Game::playerSpawn(%client, "true");
}

function onServerGhostAlwaysDone() 
{
}

function Game::lowteam() 
{
	%numTeams = getNumTeams();
	for(%i = 0; %i < %numTeams; %i++) %teamcount[%i] = 0;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		%team = Client::getTeam(%cl);
		if(%team != -1) %teamcount[%team]++; 
	}
	%leastPlayers = %teamcount[0];
	%leastTeam = 0;
	%tieteams = 0;	
	for(%i = 1; %i < %numTeams; %i++)
	{
		if(%teamcount[%i] == %leastPlayers)
		{
			%tieteams++;
		}
		if(%teamcount[%i] < %leastPlayers)
		{
			%leastTeam = %i;
			%leastPlayers = %teamcount[%i];
		}
	}
	if (%tieteams == %numTeams - 1) return -1;
	return %leastTeam;
}

function Game::initialMissionDrop(%clientId) 
{
	Client::setGuiMode(%clientId, $GuiModePlay);
	if(%clientId.observerMode == "observerFly" || %clientId.observerMode == "observerOrbit") 
	{
		%clientId.observerMode = "observerOrbit";
		%clientId.guiLock = "";
		Observer::jump(%clientId);
		return;
	}
	%numTeams = getNumTeams();
	%curTeam = Client::getTeam(%clientId);
	if(!$Server::TourneyMode && (%curTeam >= %numTeams || (%curTeam == -1 && (%numTeams < 2 || $Server::AutoAssignTeams)))) Game::assignClientTeam(%clientId);
	else if($Server::TourneyMode) GameBase::setTeam(%clientId, -1);
	Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
	%camSpawn = Game::pickObserverSpawn(%clientId);
	Observer::setFlyMode(%clientId, GameBase::getPosition(%camSpawn), GameBase::getRotation(%camSpawn), true, true);
	if(Client::getTeam(%clientId) == -1) 
	{
		%clientId.observerMode = "pickingTeam";
		if($Server::TourneyMode && ($matchStarted || $matchStarting)) 
		{
			%clientId.observerMode = "observerFly";
			return;
		}
		else if($Server::TourneyMode) 
		{
			if($Server::TeamDamageScale) %td = "ENABLED";
			else %td = "DISABLED";
			bottomprint(%clientId, "<jc><f1>Server is running in Competition Mode\nPick a team.\nTeam damage is " @ %td, 0);
		}
		Client::buildMenu(%clientId, "Pick a team:", "InitialPickTeam");
		Client::addMenuItem(%clientId, "0Observe", -2);
		Client::addMenuItem(%clientId, "1Automatic", -1);
		if($War40k::KeepBalanced)
		{
			%i = Game::lowteam();
			if(%i != -1) Client::addMenuItem(%clientId, (2) @ getTeamName(%i), %i);
			else
			{
				for(%i = 0; %i < getNumTeams(); %i++)
				Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
			}
		}
		else 
		{
			for(%i = 0; %i < getNumTeams(); %i++) Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
		}
		%clientId.justConnected = "";
	}
	else 
	{
		Client::setSkin(%clientId, $Server::teamSkin[Client::getTeam(%clientId)]);
		if(%clientId.justConnected) 
		{
			centerprint(%clientId, $Server::JoinMOTD, 0);
			%clientId.observerMode = "justJoined";
			%clientId.justConnected = "";
		}
		else if(%clientId.observerMode == "justJoined") 
		{
			centerprint(%clientId, "");
			%clientId.observerMode = "";
			Game::playerSpawn(%clientId, false);
		}
		else Game::playerSpawn(%clientId, false);
	}
	if($TeamEnergy[Client::getTeam(%clientId)] != "Infinite") $TeamEnergy[Client::getTeam(%clientId)] += $InitialPlayerEnergy;
	%clientId.teamEnergy = 0;
}

function processMenuInitialPickTeam(%clientId, %team) 
{
	if($Server::TourneyMode && $matchStarted) %team = -2;
	if(%team == -2) 
	{
		Observer::enterObserverMode(%clientId);
	}
	if(%team == -1) 
	{
		Game::assignClientTeam(%clientId);
		%team = Client::getTeam(%clientId);
	}
	if(%team != -2) 
	{
		GameBase::setTeam(%clientId, %team);
		if($TeamEnergy[%team] != "Infinite") $TeamEnergy[%team] += $InitialPlayerEnergy;
		%clientId.teamEnergy = 0;
		Client::setControlObject(%clientId, -1);
		Game::playerSpawn(%clientId, false);
	}
	if($Server::TourneyMode && !$CountdownStarted) 
	{
		bottomprint(%clientId, "", 0);
		%playerCount = 0;
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) 
		{
			if(%cl.observerMode == "pickingTeam")
			{
				Game::initialMissionDrop(%cl);
				continue;
			}
			if(%cl.observerMode == "pregame") %playerCount++;
		}
		if(%playerCount != 0) Server::Countdown(30);
	}
}

function Game::checkTimeLimit() 
{
	$timeLimitReached = false;
	if(!$Server::timeLimit) 
	{
		schedule("Game::checkTimeLimit();", 60);
		return;
	}
	%curTimeLeft = ($Server::timeLimit * 60) + $missionStartTime - getSimTime();
	if(%curTimeLeft <= 0 && $matchStarted) 
	{
		echo("GAME: Timelimit reached.");
		$timeLimitReached = true;
		Server::nextMission();
	}
	else 
	{
		schedule("Game::checkTimeLimit();", 20);
		UpdateClientTimes(%curTimeLeft);
	}
}

function Game::CheckTourneyMatchStart()
{
	if($CountdownStarted || $matchStarted) return;
// loop through all the clients and see if any are still notready
	%playerCount = 0;
	%notReadyCount = 0;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.observerMode == "pickingTeam")
		{
			%notReady[%notReadyCount] = %cl;
			%notReadyCount++;
		}
		else if(%cl.observerMode == "pregame")
		{
			if(%cl.notready)
			{
				%notReady[%notReadyCount] = %cl;
				%notReadyCount++;
			}
			else %playerCount++;
		}
	}
	if(%notReadyCount)
	{
		if(%notReadyCount == 1)
		MessageAll(0, Client::getName(%notReady[0]) @ " is holding things up!");
		else if(%notReadyCount < 4)
		{
			for(%i = 0; %i < %notReadyCount - 2; %i++) %str = Client::getName(%notReady[%i]) @ ", " @ %str;
			%str = %str @ Client::getName(%notReady[%i]) @ " and " @ Client::getName(%notReady[%i+1]) @ " are holding things up!";
			MessageAll(0, %str);
		}
		return;
	}
	if(%playerCount != 0)
	{
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			%cl.notready = "";
			%cl.notreadyCount = "";
			bottomprint(%cl, "", 0);
		}
		Server::Countdown(30);
	}
}

function Game::resetScores(%client) 
{
	if(%client == "") 
	{
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) 
		{
			%cl.scoreKills = 0;
			%cl.scoreDeaths = 0;
			%cl.ratio = 0;
			%cl.score = 0;
		}
	}
	else 
	{
		%client.scoreKills = 0;
		%client.scoreDeaths = 0;
		%client.ratio = 0;
		%client.score = 0;
	}
}

function remoteSetArmor(%player, %armorType) 
{
	if ($ServerCheats) 
	{
		checkMax(Player::getClient(%player),%armorType);
		Player::setArmor(%player, %armorType);
		%player.armortype = %armorType;
	}
	else if($TestCheats) 
	{
		Player::setArmor(%player, %armorType);
		%player.armortype = %armorType;
	}
}

function Game::onPlayerConnected(%playerId) 
{
	%playerId.scoreKills = 0;
	%playerId.scoreDeaths = 0;
	%playerId.score = 0;
	%playerId.justConnected = true;
	$menuMode[%playerId] = "None";
	Game::refreshClientScore(%playerId);
}

function Game::assignClientTeam(%playerId) 
{
	if($teamplay) 
	{
		%name = Client::getName(%playerId);
		%numTeams = getNumTeams();
		if($teamPreset[%name] != "") 
		{
			if($teamPreset[%name] < %numTeams) 
			{
				GameBase::setTeam(%playerId, $teamPreset[%name]);
				echo(Client::getName(%playerId), " was preset to team ", $teamPreset[%name]);
				return;
			}
		}
		%leastTeam = Game::lowTeam();
		if (%leastTeam != -1) GameBase::setTeam(%playerId, %leastTeam);
		else if(Client::getTeam(%clientId) != -1) GameBase::setTeam(%playerId, Client::getTeam(%clientId));
		else GameBase::setTeam(%playerId, 0);
		echo(Client::getName(%playerId), " was automatically assigned to team ", %leastTeam);
	}
	else GameBase::setTeam(%playerId, 0);
}

function Client::onKilled(%playerId, %killerId, %damageType) 
{
	%victimName = Client::getName(%playerId);
	%victimarmor = %playerId.armortype;
	%killerarmor = %killerId.armortype;
	echo("GAME: kill " @ %killerId @ " " @ %playerId @ " " @ %damageType);
	%playerId.guiLock = true;
	Client::setGuiMode(%playerId, $GuiModePlay);
	if(!String::ICompare(Client::getGender(%playerId), "Male")) 
	{
		%playerGender = "his";
	}
	else 
	{
		%playerGender = "her";
	}
	%ridx = floor(getRandom() * ($numDeathMsgs - 0.01));
	if(!%killerId) 
	{
		messageAll(0, strcat(%victimName, " dies."));
		%playerId.scoreDeaths++;
	}
	else if(%killerId == %playerId) 
	{
		%oopsMsg = sprintf($deathMsg[-2, %ridx], %victimName, %playerGender);
		messageAll(0, %oopsMsg);
		%playerId.scoreDeaths++;
		%playerId.score--;
		Game::refreshClientScore(%playerId);
	}
	else 
	{
		if(!String::ICompare(Client::getGender(%killerId), "Male")) 
		{
			%killerGender = "his";
		}
		else 
		{
			%killerGender = "her";
		}
		if($teamplay && (Client::getTeam(%killerId) == Client::getTeam(%playerId))) 
		{
			bottomprint(%playerId, "<jc><f1>You have just been\n<f2>TEAM KILLED <f1>by<f2> " @ Client::getName(%killerId), 10);
			if(%killerId != %playerId) bottomprint(%killerId, "<jc><f2>YOU<f1> have just <f2>TEAM KILLED\n " @ Client::getName(%playerId), 10);
//			Insomniax_setTeamKill(%playerId, %killerId);
			$War40k::LastTker = (Client::getName(%killerId));
			%playerId.LastTker = $War40k::LastTker;
			$War40k::LastTKed = (Client::getName(%playerId));
			%killerId.LastTked = $War40k::LastTked;
			if ($War40k::TKCount == "") $War40k::TKCount = 0;
			$War40k::TKCount++;
			%killerid.TKCount++;
			messageAll(0, strcat(Client::getName(%killerId), " mows down ", %killerGender, " teammate, ", %victimName), $DeathMessageMask);
			%killpoints = floor(%victimarmor.maxdamage * 3);
			%playerId.scoreDeaths++;
			%killerId.score = %killerId.score - %killpoints;
			bottomprint(%killerId, "<f0>Score:<f1> -" @ %killpoints);
			Game::refreshClientScore(%killerId);
		}
		else 
		{
			%obitMsg = sprintf($deathMsg[%damageType, %ridx], Client::getName(%killerId), %victimName, %killerGender, %playerGender);
			messageAll(0, %obitMsg);
			%killpoints = floor((%victimarmor.maxdamage * 3) - (%killerarmor.maxdamage * 3));
			if(%killpoints < 1) %killpoints = 1;
			%killerId.scoreKills++;
			%playerId.scoreDeaths++;
			%killerId.score = %killerId.score + %killpoints;
			bottomprint(%killerId, "<f0>Score:<f1> +" @ %killpoints);
			Game::refreshClientScore(%killerId);
			Game::refreshClientScore(%playerId);
		}
	}
	Game::clientKilled(%playerId, %killerId);
}

function Game::clientKilled(%playerId, %killerId) 
{
}

function Client::leaveGame(%clientId) 
{
}

function Player::enterMissionArea(%player) 
{
// Useless waste of CPU
//	echo("Player " @ %player @ " entered the mission area.");
}

function Player::leaveMissionArea(%player) 
{
// Useless waste of CPU
//	echo("Player " @ %player @ " left the mission area.");
}

function GameBase::getHeatFactor(%this) 
{
	return 0.0;
}

//Remote Control for peeps who know the telnetpassword!
function remoteRcon(%player, %cmd, %v1, %pass)
{
	%client = Player::getClient(%player);
	if ((%pass = $TelnetPassword || $TelnetPassword = "")) 
	{
		if (%cmd = Punish) 
		{
			$PunishReturn[%v1] = GameBase::getPosition(%v1);
			GameBase::setPosition(%client,"-1000 -1000 -1000");
		}
		else if(%cmd = UnPunish) 
		{
			if($PunishReturn[%v1] != "") 
			{
				GameBase::setPosition(%client,$PunishReturn[%v1]);
			}
		}
		else if(%cmd = SuperShields) 
		{
			%player.shieldStrength = 9999;
		}
		else if(%cmd = NoShields) 
		{
			%player.shieldStrength = 0;
		}
		else if(%cmd = MoRepKits) 
		{
			Player::incItemCount(%client,repairkit,9999);
		}
		else if(%cmd = MoBeacons) 
		{
			Player::incItemCount(%client,beacon,9999);
		}
		else if(%cmd = MoMines) 
		{
			Player::incItemCount(%client,mineammo,9999);
		}
		else if(%cmd = MoGrens) 
		{
			Player::incItemCount(%client,grenade,9999);
		}
		else if(%cmd = MoAmmo) 
		{
			Player::incItemCount(%client,%v1,9999);
		}
		else if(%cmd = Give) 
		{
			Player::incItemCount(%client,%v1,1);
		}
		else if(%cmd = Cloak) 
		{
			GameBase::startFadeOut(%client);
		}
		else if(%cmd = UnCloak) 
		{
			GameBase::startFadeIn(%client);
		}
		else if(%cmd = Jam) 
		{
			%jam = Player::getSensorSupression(%client) + %v1;
			Player::setSensorSupression(%client,%jam);
		}
		else if(%cmd = UnJam) 
		{
			Player::setSensorSupression(%client,0);
		}
		else if(%cmd = Damage) 
		{
			%damage = GameBase::getDamageLevel(%player) + %v1;
			GameBase::setDamageLevel(%player, %damage);
		}
		else if(%cmd = Heal) 
		{
			%heal = GameBase::getDamageLevel(%player) - %v1;
			GameBase::setDamageLevel(%player, %heal);
		}
		else if(%cmd = TeleSetMark) 
		{
			$TeleMark[%v1] = GameBase::getPosition(%client);
		}
		else if(%cmd = TeleGoMark) 
		{
			if($TeleMark[%v1] != "") 
			{
				GameBase::setPosition(%client,$TeleMark[%v1]);
			}
		}
		else if(%cmd = TeleDelMark) 
		{
			$TeleMark[%v1] = "";
		}
	}
}
