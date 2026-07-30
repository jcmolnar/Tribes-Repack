//What a POS this is, but it kinda works, somehow.


Event::Attach(eventConnected, WH::Init);

Event::Attach(eventExitStation, WH::Update);
//Dunno why you have to use a schedule here?
Event::Attach(eventItemReceived, "schedule::add(\"WH::Update();\",0);");

//My events that I added to this pile of junk to make it run smoother
Event::Attach(eventUsedItem, WH::Update);
Event::Attach(eventDropItem, WH::Update);
Event::Attach(EventNextWeapon, WH::Update);
Event::Attach(EventPrevWeapon, WH::Update);
Event::Attach(EventYouFired, WH::Update);

//This exists because I can't get an event for all nec. updates, so run it over and over. 
$WH::UpdateTime = 0.5;


function WH::Update()
{
	
	%slot = 0;

	//For all possible weapons
	for (%i=0; %i<=$Weapon::Count; %i++)
	{
		//If the weapon is in your loadout
		if (getItemCount($Weapon::Name[%i]) > 0)
		{
			//Get Ammo
			%ammo = $Weapon::Ammo[%i];
			if (%ammo !="")
				%ammoNum = "<jc><f2>"@getItemCount(%ammo);

			//If you are holding the weapon highlight it
			%mounted = false;
			if (getItemType($Weapon::Name[%i]) == getMountedItem(0))
			{
				%mounted = true;
				//Highlight ammo on firing and increase update time
				if ($AF::Firing == "TRUE")
				{
					%ammoNum = "<jc><f3>"~%ammoNum;
					$WH::UpdateTime = 0.1;				
				}
				else
				{
					%ammoNum = "<jc><f3>"~%ammoNum;
					$WH::UpdateTime = 0.5;
				}
			}
			
			//Draw our weapon
			if(%mounted)
				control::setValue("WeaponHUD::Item"~%slot,"<B0,0:modules\\weaponshud\\"~$Weapon::File[%i]~"on.png>");
			else
				control::setValue("WeaponHUD::Item"~%slot,"<B0,0:modules\\weaponshud\\"~$Weapon::File[%i]~".png>");

			//Draw our ammo
			control::setValue("WeaponHUD::Ammo"~%slot,%ammoNum);

			//Go to the next slut, err slot
			%ammoNum = "";
			%slot++;
		}
	}

	//Clear the rest of the weapon slots
	for (%slot; %slot<=10;%slot++)
	{
		control::setValue("WeaponHUD::Item"~%slot,"");
		control::setValue("WeaponHUD::Ammo"~%slot,"");
		control::setValue("WeaponHUD::BG"~%slot,"");

	}

	//Fuck I want to get rid of this. Someone help!
	schedule::add("WH::Update();", $WH::UpdateTime);

}

//Add our items
function WH::AddItem(%item, %file, %ammo) {
	if(getItemType(%item) != -1) {
		//echo("Added "~%item);

		//ID num for weapon
		%num = getItemType(%item);
		$Weapon::Num[$Weapon::Count] = %num;

		//Name of Weapon
		$Weapon::Name[$Weapon::Count] = %item;

		//Filename for image
		$Weapon::File[$Weapon::Count] = %file;

		//Ammo Name for weapon
		$Weapon::Ammo[$Weapon::Count] = %ammo;

		//++ baby!
		$Weapon::Count++;

		return true;
	}
	return false;
}

//Crappy bubble sort, short list, who cares, only runs on init anyway
function WH::Sort()
{
	for(%i=0;%i<$Weapon::Count;%i++)
	{
		for (%j=1;%j<$Weapon::Count;%j++)
		{
			if ($Weapon::Num[%j] < $Weapon::Num[%j-1])
				WH::Swap(%j,%j-1);
		}
	}
}
			
//Swap two weapons	
function WH::Swap(%one,%two)
{
		%temp[0] = $Weapon::Num[%one];
		%temp[1] = $Weapon::Name[%one];
		%temp[2] = $Weapon::File[%one];
		%temp[3] = $Weapon::Ammo[%one];

		$Weapon::Num[%one] = $Weapon::Num[%two];
		$Weapon::Name[%one] = $Weapon::Name[%two];
		$Weapon::File[%one] = $Weapon::File[%two];
		$Weapon::Ammo[%one] = $Weapon::Ammo[%two];

		$Weapon::Num[%two] = %temp[0];
		$Weapon::Name[%two] = %temp[1];
		$Weapon::File[%two] = %temp[2];
		$Weapon::Ammo[%two] = %temp[3];
}

//Wake Sleep shit
function WH::Wake() {
	$WH::Awake = true;
	WH::Update();
}

function WH::Sleep() {
	Schedule::Cancel("WH::Update();");
	$WH::Awake = false;
}

function WH::Create() {

	if ($WeaponHUD::Loaded)
		return;
	$WeaponHUD::Loaded = true;

	$WeaponHUD::Awake = false;

	HUD::New( "WeaponHUD::Container", 0, 8, 165, 220, WH::Wake, WH::Sleep );

	//11 slots should be enough, any mod that has more than 11 can suck my ass
	for (%i=0;%i<=10;%i++)
	{
		newObject( "WeaponHUD::Item"~%i, FearGuiFormattedText, 0, 15+(60*%i), 70, 35 );
		newObject( "WeaponHUD::Ammo"~%i, FearGuiFormattedText, -50, 31+(60*%i), 70, 35 );

		HUD::Add( "WeaponHUD::Container", "WeaponHUD::Item"~%i );
		HUD::Add( "WeaponHUD::Container", "WeaponHUD::Ammo"~%i );

	}
}

function WH::Init() {


	//Clear out current variables
	DeleteVariables("$Weapon::*");
	$Weapon::Count = 0;

	//LT Items
	
	WH::AddItem("Disc Launcher", "disk", "Disc");
	WH::AddItem("Grenade Launcher", "grenade", "Grenade Ammo");
	WH::AddItem("ChainGun", "chaingun", "Bullet");

	//Base Items
	
	WH::AddItem("Blaster","blaster");
	WH::AddItem("Elf Gun", "elf");
	WH::AddItem("Laser Rifle", "sniper");
	WH::AddItem("Mortar", "mortar", "Mortar Ammo");
	WH::AddItem("Plasma Gun", "plasma", "Plasma Bolt");


	//I don't give a shit about mods, this is a bitch, someone else can write more here

	//Sort the weapons list by ID so they are in order
	WH::Sort();

	//Add to GUI
	WH::Create();
}