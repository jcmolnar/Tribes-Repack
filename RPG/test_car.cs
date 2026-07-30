//----------------------------------------------------------------------------
// test_car.cs — stand up the (previously unfinished) Car vehicle.
//
// Run on a SERVER you control (host a listen server via Create Server), then drop
// the console (~) and:  exec("test_car.cs");
// Then:  %p = Client::getControlObject(LocalClientId);  spawnCar(%p);  driveCar(%p);
//
// The body shape is "car" -> car.dts (deployed to base/). To smoke-test with a
// different present shape, edit shapeFile below.
//----------------------------------------------------------------------------

CarData TestCar
{
   className   = "Vehicle";
   // VALIDATION: use the known-good vehicle shape "newflyer" to prove the Car code (spawn/render/
   // mount/drive) works. Once car.dts loads in the engine, switch this back to "car".
   shapeFile   = "newflyer";

   // physics (VehicleData() leaves these uninitialized, so they MUST be set)
   mass        = 8.0;
   drag        = 1.0;
   density     = 1.2;
   maxSpeed    = 40;
   minSpeed    = -8;

   maxDamage   = 1.0;
   damageLevel = {1.0, 1.0};
   maxEnergy   = 100;
   repairRate  = 0;

   // car wheel params (Car::CarData)
   wheelCount          = 4;
   wheelRadius         = 1.0;
   wheelRestDist       = 2.5;
   wheelOneGDist       = 2.3;
   wheelSideBrakeForce = 100;
   turnAngle           = 0.2;

   visibleDriver = true;
   driverPose    = 22;
   mapFilter     = 2;
   mapIcon       = "M_vehicle";
   description   = "Test Car";
};

// the real car model (deployed to base/car.dts). Same params, different shape.
CarData CarReal
{
   className   = "Vehicle";
   shapeFile   = "car";
   mass        = 8.0;
   drag        = 1.0;
   density     = 1.2;
   maxSpeed    = 40;
   minSpeed    = -8;
   maxDamage   = 1.0;
   damageLevel = {1.0, 1.0};
   maxEnergy   = 100;
   repairRate  = 0;
   wheelCount          = 4;
   wheelRadius         = 1.0;
   wheelRestDist       = 2.5;
   wheelOneGDist       = 2.3;
   wheelSideBrakeForce = 100;
   turnAngle           = 0.2;
   visibleDriver = true;
   driverPose    = 22;
   description   = "Real Car";
};

//--- spawn a given CarData at an explicit world position "x y z" ---
function spawnCarAt(%pos, %db)
{
   %car = newObject("", Car, %db, true);
   if (%car <= 0)
   {
      echo("spawnCar: creation FAILED (shape '" @ %db @ "' missing/failed to load?)");
      return 0;
   }
   GameBase::setMapName(%car, "TestCar");
   GameBase::startFadeIn(%car);
   GameBase::setPosition(%car, %pos);
   $TestCar::Last = %car;
   echo("spawned Car id " @ %car @ " (" @ %db @ ") at " @ %pos);
   return %car;
}

//--- find a controlled player: local object (listen server), else scan connected clients
//    (dedicated server) — needs a CLIENT joined AND spawned as a character.
function findMyPlayer()
{
   %obj = getLocalObject();
   if (%obj > 0) { $TestCar::Player = %obj; return %obj; }
   // client ids are 2048..2175 (PlayerManager: clientId = readInt(7) + 2048), NOT 0..N
   for (%i = 2048; %i < 2176; %i++)
   {
      %o = Client::getControlObject(%i);
      if (%o > 0) { $TestCar::Player = %o; echo("using connected client " @ %i @ " player " @ %o); return %o; }
   }
   echo("findMyPlayer: no controlled player found — join with a client AND spawn a character first");
   return -1;
}

//--- spawn a given CarData ~18 units in FRONT of the (connected) player ---
function spawnCarDB(%db)
{
   %me = findMyPlayer();
   if (%me <= 0) return 0;
   %xf  = GameBase::getTransform(%me);            // "px py pz rx ry rz"
   %pos = getWord(%xf,0) @ " " @ getWord(%xf,1) @ " " @ getWord(%xf,2);
   %rot = getWord(%xf,3) @ " " @ getWord(%xf,4) @ " " @ getWord(%xf,5);
   %fwd = Vector::getFromRot(%rot, 18);           // 18 units straight ahead
   %at  = Vector::add(%pos, %fwd);
   echo("you=" @ %pos @ "  spawning at=" @ %at);
   return spawnCarAt(%at, %db);
}
function spawnCar()     { return spawnCarDB(TestCar); }   // newflyer placeholder (known-good)
function spawnRealCar() { return spawnCarDB(CarReal); }   // your car.dts model

//--- teleport your control object to the last spawned car (probe: does your view follow?) ---
function gotoCar()
{
   if ($TestCar::Last <= 0) { echo("gotoCar: spawn a car first"); return 0; }
   %me = getLocalObject();
   if (%me <= 0) { echo("gotoCar: no control object"); return 0; }
   %cpos = GameBase::getPosition($TestCar::Last);
   %view = Vector::add(%cpos, "0 -14 6");   // 14 back, 6 up from the car
   GameBase::setPosition(%me, %view);
   echo("moved you to " @ %view @ " (car at " @ %cpos @ ")");
}

//--- get in the last spawned car AND take control of it (so it drives) ---
function driveCar()
{
   if ($TestCar::Last <= 0) { echo("driveCar: spawn a car first"); return 0; }
   %p = $TestCar::Player;
   if (%p <= 0) %p = findMyPlayer();
   if (%p <= 0) return 0;
   $TestCar::Client = Player::getClient(%p);
   Player::setMountObject(%p, $TestCar::Last, 1);            // mountPoint 1 = pilotNode (driver seat); 0 is the EXIT node
   Client::setControlObject($TestCar::Client, $TestCar::Last); // control -> car so your moves drive it
   echo("drive: client " @ $TestCar::Client @ " now controls car " @ $TestCar::Last);
}

//--- give control back to your player + get out ---
function dismountCar()
{
   if ($TestCar::Client > 0 && $TestCar::Player > 0)
      Client::setControlObject($TestCar::Client, $TestCar::Player);
   if ($TestCar::Player > 0)
      Player::setMountObject($TestCar::Player, -1, 0);
   echo("dismounted");
}

echo("test_car.cs loaded.  Just run:   spawnCar();    then:   driveCar();");
