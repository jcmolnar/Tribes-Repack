// Create HUD, if exists Update 
function ItemHUD::Init() 
{ 

	if ($ItemHUD::Loaded)
		return;
	$ItemHUD::Loaded = true;

	$ItemHUD::Awake = false;

	HUD::New( "ItemHUD::Container", 0, 0, 250, 100, ItemHUD::Wake, ItemHUD::Sleep );

	newObject( "ItemHUD::BeacIcon", FearGuiFormattedText, 125, 32, 40, 15 );
	newObject( "ItemHUD::GrenIcon", FearGuiFormattedText, 5, 16, 160, 12 );

	newObject( "ItemHUD::BeacText", FearGuiFormattedText, 195, 40, 40, 15 );
	newObject( "ItemHUD::GrenText", FearGuiFormattedText, 70, 40, 160, 12 );
	
	HUD::Add( "ItemHUD::Container", "ItemHUD::BeacIcon" );
	HUD::Add( "ItemHUD::Container", "ItemHUD::GrenIcon" );

	HUD::Add( "ItemHUD::Container", "ItemHUD::BeacText" );
	HUD::Add( "ItemHUD::Container", "ItemHUD::GrenText" );
	
	//Control::SetValue("ItemHUD::Text", "<B0,0:Modules/numHUD/clock/0.png>");

    Control::SetValue("ItemHUD::BeacText", "<B0,0:Modules/numHUD/clock/0.png>");
    Control::SetValue("ItemHUD::GrenText", "<B0,0:Modules/numHUD/clock/0.png>");


} 

function ItemHUD::Wake() {
	$ItemHUD::Awake = true;
	ItemHUD::Update();
}

function ItemHUD::Sleep() {
	Schedule::Cancel("ItemHUD::Update();");
	$ItemHUD::Awake = false;
}


function ItemHUD::Update() 
{ 

	if ( !$ItemHUD::Awake )
		return;
	
	Schedule::Add("ItemHUD::Update();", 1);

	%Gren = getItemCount("Grenade"); 
	%Beac = getItemCount("Beacon");  

	if ( (%Gren == $ItemHud::Gren ) && (%Beac == $ItemHud::Beac ) )
		return;
		

	%name = Client::GetName( getManagerId() );
	control::setValue("ItemHUD::StatText","<f2>"@$Collector::Score[%name]);

	%GrenadeTxt = "<B0,0:Modules/numHUD/clock/"~String::GetSubStr(%Gren,0,1)~".png><B0,0:Modules/numHUD/clock/"~String::GetSubStr(%Gren,1,1)~".png><B0,0:Modules/numHUD/clock/"~String::GetSubStr(%Gren,2,1)~".png>";
	%BeaconTxt = "<B0,0:Modules/numHUD/clock/"~String::GetSubStr(%Beac,0,1)~".png><B0,0:Modules/numHUD/clock/"~String::GetSubStr(%Beac,1,1)~".png><B0,0:Modules/numHUD/clock/"~String::GetSubStr(%Beac,2,1)~".png>";

	//%numbers = "<B0,0:Modules/numHUD/score/"~String::GetSubStr($Weapon::Ammo,0,1)~".png><B0,0:Modules/numHUD/score/"~String::GetSubStr($Weapon::Ammo,1,1)~".png><B0,0:Modules/numHUD/score/"~String::GetSubStr($Weapon::Ammo,2,1)~".png>";
	//Control::SetValue( "AmmoHUD::Text", %numbers );
	//Schedule::Add("AmmoHUD::Update();", 0.1);
	
	if(%Gren==0)
	{
		control::setValue("ItemHUD::GrenIcon", "<B0,0:Modules/ItemHud/gren0.png>");
		control::setValue("ItemHUD::GrenText", %GrenadeTxt);
	}
	else
	{
		control::setValue("ItemHUD::GrenIcon", "<B0,0:Modules/ItemHud/gren.png>");
		control::setValue("ItemHUD::GrenText", %GrenadeTxt);
	}

	if(%Beac==0)
	{
		control::setValue("ItemHUD::BeacIcon", "<B0,0:Modules/ItemHud/beacon0.png>");
		control::setValue("ItemHUD::BeacText", %BeaconTxt);
	}
	else
	{
		control::setValue("ItemHUD::BeacIcon", "<B0,0:Modules/ItemHud/beacon.png>");
		control::setValue("ItemHUD::BeacText", %BeaconTxt);
	}

} 

ItemHUD::Init();


Event::Attach(eventItemReceived, "Schedule::Add(\"ItemHUD::Update();\", 0);");
Event::Attach(eventItemDropped, "Schedule::Add(\"ItemHUD::Update();\", 0);");
Event::Attach(eventItemUsed, "Schedule::Add(\"ItemHUD::Update();\", 0);");
