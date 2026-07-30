// ---------------------------------------------------------------------------
// inventory_table.cs -- Version 1.7 -- April 3, 1999
// by Vis-DS.Writer (Lorne Laliberte, writer@videon.wave.ca)
//
// http://www.videon.wave.ca/~writer/strategy/tribes.html
// ---------------------------------------------------------------------------


//
// Set up Inventory Table variables
//
function Inv::Init(%itemvarname, %itemname, %ammovarname, %ammoname)
{
    // Assign the type # for %itemname to $Inv::<%itemvarname>
    %itemtype = eval("$Inv::" @ %itemvarname @ "=getItemType(\""@ %itemname @ "\");");

    if(%itemtype != -1) // item exists on this server
    {
        // Assign the item name to our $Inv::Name array
        $Inv::Name[%itemtype] = %itemname;

        if(%ammoname != "") // %ammoname arg provided
        {
            // Associate this ammo name with the item (weapon)
            $Inv::Ammo[%itemtype] = %ammoname;
            
            if(%ammovarname != "") // %ammovarname arg provided
            {
                // Assign the type # for %ammoname to $Inv::<%ammovarname>
                %ammotype = eval("$Inv::" @ %ammovarname @ "=getItemType(\""@ %ammoname @ "\");");
                
                // Assign the ammo name to our $Inv::Name array
                $Inv::Name[%ammotype] = %ammoname;
            }
        }
        else // %ammoname arg not specified
        {
            // In case we need to handle two mods using the same weapon name
            // where one of them doesn't use ammo...in which case we might
            // have to override an existing ammo entry.  Otherwise, we could
            // just leave $Inv::Ammo[%itemtype] undefined.

            // Clear any existing ammo association
            $Inv::Ammo[%itemtype] = "";
        }
        return 1;
    }
    else // item has been replaced or renamed on this server
    {
        // Probably not necessary so long as we remember to
        // test for -1 return values from getMountedItem and such,
        // but it doesn't hurt to be cautious

        $Inv::Name[%itemtype] = "";
        if(%ammoname != "")
        {
            $Inv::Ammo[%itemtype] = "";
        }
        return 0;
    }
}

//
// Set up the inventory table variables
//
function Inv::Initialize()
{
    
    //
    // BASE armor:
    //
    Inv::Init("Light_Armor", "Light Armor");
    Inv::Init("Medium_Armor", "Medium Armor");
    Inv::Init("Heavy_Armor", "Heavy Armor");
    
    //
    // BASE vehicles:
    //
    Inv::Init("Scout", "Scout");
    Inv::Init("LPC", "LPC");
    Inv::Init("HPC", "HPC");
    
    //
    // BASE weapons and ammo:
    //
    Inv::Init("Blaster", "Blaster");
    Inv::Init("Chaingun", "Chaingun", "Bullet", "Bullet");
    Inv::Init("Plasma_Gun", "Plasma Gun", "Plasma_Bolt", "Plasma Bolt");
    
    if(Inv::Init("Grenade_Ammo", "Grenade Ammo"))
    {
        Inv::Init("Grenade_Launcher", "Grenade Launcher", "", "Grenade Ammo");
    }
    else
    {
        Inv::Init("Grenade_Launcher", "Grenade Launcher", "Grenade_Ammo", "GrenadeAmmo");    
    }
    
    if(Inv::Init("Mortar_Ammo", "Mortar Ammo"))
    {
        Inv::Init("Mortar", "Mortar", "", "Mortar Ammo");
    }
    else
    {
        Inv::Init("Mortar", "Mortar", "Mortar_Ammo", "MortarAmmo");
    }
    
    Inv::Init("Disc_Launcher", "Disc Launcher", "Disc_Ammo", "Disc");
    Inv::Init("Laser_Rifle", "Laser Rifle");
    Inv::Init("Targeting_Laser", "Targeting Laser");
    Inv::Init("ELF_Gun", "ELF gun");
    Inv::Init("Repair_Gun", "Repair Gun");
    
    //
    // BASE packs and misc equipment:
    //
    Inv::Init("Inventory_Station", "Inventory Station");
    Inv::Init("Ammo_Station", "Ammo Station");
    Inv::Init("Energy_Pack", "Energy Pack");
    Inv::Init("Repair_Pack", "Repair Pack");
    Inv::Init("Shield_Pack", "Shield Pack");
    Inv::Init("Ammo_Pack", "Ammo Pack");
    Inv::Init("Sensor_Jammer_Pack", "Sensor Jammer Pack");
    Inv::Init("Motion_Sensor", "Motion Sensor");
    Inv::Init("Pulse_Sensor", "Pulse Sensor");
    Inv::Init("Sensor_Jammer", "Sensor Jammer");
    Inv::Init("Camera", "Camera");
    Inv::Init("Turret", "Turret");
    Inv::Init("Repair_Kit", "Repair Kit");
    Inv::Init("Mine", "Mine");
    Inv::Init("Grenade", "Grenade");
    Inv::Init("Beacon", "Beacon");
    
    
    // ---------------------------------------------------------------------------
    // Now for items from the various mods
    
    //
    // MOD armor:
    //
    Inv::Init("Spy", "Spy");
    Inv::Init("Sniper", "Sniper");
    Inv::Init("Mercenary", "Mercenary");
    Inv::Init("Burster", "Burster");
    Inv::Init("Engineer", "Engineer");
    Inv::Init("Cyborg", "Cyborg");
    Inv::Init("Dragoon", "Dragoon");
    
    //
    // MOD vehicles:
    // 
    Inv::Init("Firestorm_Bomber", "Firestorm Bomber");
    Inv::Init("Stealth_LPC", "Stealth LPC");
    Inv::Init("Wraith", "Wraith");
    Inv::Init("Interceptor", "Interceptor");
    Inv::Init("Bomber_LPC", "BomberLPC");
    Inv::Init("Stealth_HPC", "StealthHPC");
    
    //
    // MOD weapons and ammo:
    //
    Inv::Init("Rocket_Launcher", "Rocket Launcher", "Rocket_Ammo", "RocketAmmo");
    Inv::Init("EMP_Grenade_Launcher", "EMP Grenade Launcher", "EMP_Grenade_Ammo", "EMPGrenadeAmmo");
    Inv::Init("MAG_Gun", "MAG Gun");
    Inv::Init("IX_2000_Sniper_Rifle", "IX-2000 Sniper Rifle", "Rifle_Ammo", "Rifle Ammo");
    Inv::Init("Sniper_Rifle", "Sniper Rifle", "Sniper_Bullet", "Sniper Bullet");
    Inv::Init("Pyro_Torch", "Pyro-Torch", "Pyro_Charge", "Pyro Charge");
    Inv::Init("Stinger", "Stinger", "Rockets", "Rockets");
    Inv::Init("Shockwave_Cannon", "Shockwave Cannon");
    Inv::Init("Railgun", "Railgun", "Railgun_Bolt", "Railgun Bolt");
    Inv::Init("Gauss_Gun", "Gauss Gun");
    Inv::Init("Vulcan", "Vulcan", "Vulcan_Bullet", "Vulcan Bullet");
    Inv::Init("Magnum", "Magnum", "Magnum_Bullets", "Magnum Bullets");
    
    //
    // MOD packs and misc equipment:
    //
    Inv::Init("Cloaking_Device", "Cloaking Device");
    Inv::Init("Heat_Sink", "Heat Sink");
    Inv::Init("Rocket_Booster", "Rocket Booster");
    Inv::Init("Sentry", "Sentry");
    Inv::Init("Rocket_Turret", "Rocket Turret");
    Inv::Init("Laser_Turret", "Laser Turret");
    Inv::Init("Plasma_Turret", "Plasma Turret");
    Inv::Init("ELF_Turret", "ELF Turret");
    Inv::Init("Small_Force_Field", "Small Force Field");
    Inv::Init("Large_Force_Field", "Large Force Field");
    Inv::Init("Teleport_Pad", "Teleport Pad");
    Inv::Init("Command_Laptop", "Command Laptop");
    Inv::Init("Mechanical_Tree", "Mechanical Tree");
    Inv::Init("Deployable_Platform", "Deployable Platform");
    Inv::Init("Interceptor_Pack", "Interceptor Pack");
}

Event::Attach(eventConnected, "Inv::Initialize();");
