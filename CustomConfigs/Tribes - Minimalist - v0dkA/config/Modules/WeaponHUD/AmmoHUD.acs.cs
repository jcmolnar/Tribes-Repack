function AmmoHUD::Init() {
    if ($AmmoHUD::Loaded)
        return;
    $AmmoHUD::Loaded = true;

    $AmmoHUD::Awake = false;
    
    HUD::New( "AmmoHUD::Container", 0, 0, 159, 88, AmmoHUD::Wake, AmmoHUD::Sleep );

    //AmmoHud Background Image
    newObject( "AmmoHUD::HudBG", FearGuiFormattedText, 0, 0, 159, 88 );

    //AmmoHud Images and Ammo Count Text
    newObject( "AmmoHUD::WepIcon", FearGuiFormattedText, 25, 13, 64, 32 );
    newObject( "AmmoHUD::Text", FearGuiFormattedText, 100, 13, 40, 26);

    //ItemHud Elements
    newObject( "ItemHUD::RKitIcon", FearGuiFormattedText, 15, 53, 26, 26 );
    newObject( "ItemHUD::BeacIcon", FearGuiFormattedText, 51, 53, 26, 26 );
    newObject( "ItemHUD::GrenIcon", FearGuiFormattedText, 96, 53, 26, 26 );

    //ItemHud Count Text
    // The original six-pixel controls clip every shipped font, hiding the
    // quantities that belong beside these permanent equipment icons.
    newObject( "ItemHUD::RKitText", FearGuiFormattedText, 34, 53, 30, 26 );
    newObject( "ItemHUD::BeacText", FearGuiFormattedText, 76, 53, 30, 26 );
    newObject( "ItemHUD::GrenText", FearGuiFormattedText, 119, 53, 30, 26 );

    //AmmoHud Container Connection
    HUD::Add( "AmmoHUD::Container", "AmmoHUD::HudBG" );
    HUD::Add( "AmmoHUD::Container", "AmmoHUD::WepIcon" );
    HUD::Add( "AmmoHUD::Container", "AmmoHUD::Text" );

    //ItemHud Image Container Connection
    HUD::Add( "AmmoHUD::Container", "ItemHUD::GrenIcon" );
    HUD::Add( "AmmoHUD::Container", "ItemHUD::BeacIcon" );
    HUD::Add( "AmmoHUD::Container", "ItemHUD::RKitIcon" );

    //ItemHud Count Container Connection
    HUD::Add( "AmmoHUD::Container", "ItemHUD::GrenText" );
    HUD::Add( "AmmoHUD::Container", "ItemHUD::BeacText" );
    HUD::Add( "AmmoHUD::Container", "ItemHUD::RKitText" );

    // MODERNHUD-CONVERT: the four opacity variants collapsed to the 50% default.
    // A static literal is what lets the converter lift this background; the
    // $mj::WeaponAlpha pref had no UI and always fell back to 50 in practice.
    Control::SetValue("AmmoHUD::HudBG", "<B0,0:Modules/WeaponHUD/AmmoHudBG-50.png>");

    // Force the first ItemHUD update to populate even when all counts are 0.
    $ItemHUD::RKit = -1;
    $ItemHUD::Gren = -1;
    $ItemHUD::Beac = -1;

}

function AmmoHUD::Wake() {
    $AmmoHUD::Awake = true;
    AmmoHUD::Update();
    ItemHUD::Update();
}

function AmmoHUD::Sleep() {
    Schedule::Cancel("AmmoHUD::Update();");
    $AmmoHUD::Awake = false;
}

function AmmoHUD::Update() {
    if ( !$AmmoHUD::Awake )
        return;

    if($Weapon::Ammo < 0)
        %ammo = "~";
    else
        %ammo = $Weapon::Ammo;

    Control::SetValue("AmmoHUD::Text", "<jc><f:small-black-stroke.pft:ffffffff:000000ff:1,1>" @ %ammo);

    // MODERNHUD-CONVERT: each branch carries the FULL art path so the converter's
    // path rewrite (which works line-by-line on complete image references) can map
    // them into the pack's asset tree; assembling "dir" @ %file at the SetValue
    // hid the reference from it.
    %weapon = getItemDesc(getMountedItem(0));
    if(%weapon == "Disc Launcher")
        %icon = "Modules/WeaponHUD/disc.png";
    else if(%weapon == "Grenade Launcher")
        %icon = "Modules/WeaponHUD/grenade.png";
    else if(%weapon == "Chaingun")
        %icon = "Modules/WeaponHUD/chaingun.png";
    else if(%weapon == "Mortar")
        %icon = "Modules/WeaponHUD/mortar.png";
    else if(%weapon == "Plasma Gun")
        %icon = "Modules/WeaponHUD/plasma.png";
    else if(%weapon == "Laser Rifle")
        %icon = "Modules/WeaponHUD/sniper.png";
    else if(%weapon == "Blaster")
        %icon = "Modules/WeaponHUD/blaster.png";
    else if(%weapon == "ELF Gun")
        %icon = "Modules/WeaponHUD/elf.png";
    else if(%weapon == "Targeting Laser")
        %icon = "Modules/WeaponHUD/target.png";
    else
        %icon = "";

    if(%icon != "")
        Control::SetValue("AmmoHUD::WepIcon", "<B0,0:" @ %icon @ ">");
    else
        Control::SetValue("AmmoHUD::WepIcon", "");

    Schedule::Add("AmmoHUD::Update();", 0.1);

}

function ItemHUD::Update() 
{ 

  if ( !$AmmoHUD::Awake )
    return;
  
  Schedule::Add("ItemHUD::Update();", 1);

  %Gren = getItemCount("Grenade"); 
  %Beac = getItemCount("Beacon"); 
  %RKit = getItemCount("Repair Kit"); 

  if ( ( %RKit == $ItemHUD::RKit ) && (%Gren == $ItemHud::Gren ) && (%Beac == $ItemHud::Beac ) )
    return;

  $ItemHUD::RKit = %RKit;
  $ItemHUD::Gren = %Gren;
  $ItemHUD::Beac = %Beac;

  // MODERNHUD-CONVERT: dropped the ItemHUD::StatText score write -- that control
  // is never created by THIS module (it was a cross-module leftover), and writing
  // an unknown control is what kept the whole part unconvertible.

  %countFont = "<jc><f:small-black-stroke.pft:ffffffff:000000ff:1,1>";
  %GrenadeTxt = %countFont @ %Gren;
  %BeaconTxt = %countFont @ %Beac;
  %RepKitTxt = %countFont @ %RKit;

    control::SetValue("ItemHUD::RKitIcon", "<B0,0:modules\\WeaponHUD\\repkit.png>");
    control::setValue("ItemHUD::RKitText", %RepKitTxt);

    control::setValue("ItemHUD::GrenIcon", " <B0,0:modules\\WeaponHUD\\gren.png>");
    control::setValue("ItemHUD::GrenText", %GrenadeTxt);

    control::setValue("ItemHUD::BeacIcon", " <B0,0:modules\\WeaponHUD\\beacon.png>");
    control::setValue("ItemHUD::BeacText", %BeaconTxt);

} 

AmmoHUD::Init();

Event::Attach(eventItemReceived, "Schedule::Add(\"ItemHUD::Update();\", 0);");
Event::Attach(eventItemDropped, "Schedule::Add(\"ItemHUD::Update();\", 0);");
Event::Attach(eventItemUsed, "Schedule::Add(\"ItemHUD::Update();\", 0);");
