// flag sounds for 1.40

$Flag::DropSound[friendlyflag]="FlagDrop_Friendly";
$Flag::DropSound[enemyflag]="FlagDrop_Enemy";

$Flag::PickupSound[friendlyflag]="FlagPickedUp";
$Flag::PickupSound[enemyflag]="FlagPickedUp_Enemy";

$Flag::CapSound[friendlyflag]="FlagCap_Enemy";
$Flag::CapSound[enemyflag]="FlagCap_Friendly";

$Flag::ReturnSound[friendlyflag]="FlagReturn_Friendly";
$Flag::ReturnSound[enemyflag]="FlagReturn_Enemy";

$Flag::GrabSound[friendlyflag]="FlagGrab_Enemy";
$Flag::GrabSound[enemyflag]="FlagGrab_Friendly";


function Flag::DropSounds( %team, %cl ) {

	return ( %team == Client::GetTeam( getManagerId() ) ) ? ( localSound( $Flag::DropSound[friendlyflag] ) ) : ( localSound( $Flag::DropSound[enemyflag] ) );
}


function Flag::PickupSounds( %team, %cl ) {

	return ( %team == Client::GetTeam( getManagerId() ) ) ? ( localSound( $Flag::PickupSound[friendlyflag] ) ) : ( localSound( $Flag::PickupSound[enemyflag] ) );
}


function Flag::CapSounds( %team, %cl ) {

	return ( %team == Client::GetTeam( getManagerId() ) ) ? ( localSound( $Flag::CapSound[friendlyflag] ) ) : ( localSound( $Flag::CapSound[enemyflag] ) );
}

function Flag::ReturnSounds( %team, %cl ) {

	return ( %team == Client::GetTeam( getManagerId() ) ) ? ( localSound( $Flag::ReturnSound[friendlyflag] ) ) : ( localSound( $Flag::ReturnSound[enemyflag] ) );
}

function Flag::GrabSounds( %team, %cl ) {

	return ( %team == Client::GetTeam( getManagerId() ) ) ? ( localSound( $Flag::GrabSound[friendlyflag] ) ) : ( localSound( $Flag::GrabSound[enemyflag] ) );
}


Event::Attach(eventFlagDrop, Flag::DropSounds);
Event::Attach(eventFlagPickup, Flag::PickupSounds);
Event::Attach(eventFlagCap, Flag::CapSounds);
Event::Attach(eventFlagReturn, Flag::ReturnSounds);
Event::Attach(eventFlagGrab, Flag::GrabSounds);