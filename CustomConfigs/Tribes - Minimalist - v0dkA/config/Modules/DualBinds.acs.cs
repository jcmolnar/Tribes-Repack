// poop weps for 1.40

$PoopWep::Wep1A = "Disc Launcher";
$PoopWep::Wep1B = "Grenade Launcher";

$PoopWep::Wep2A = "ChainGun";
$PoopWep::Wep2B = "Plasma Gun";

$PoopWep::Wep3A = "Laser Rifle";
$PoopWep::Wep3B = "ChainGun";

$PoopWep::Wep4A = "Elf Gun";
$PoopWep::Wep4B = "Targeting Laser";

$PoopWep::Wep5A = "Targeting Laser";

$PoopWep::DualWeps = 5;

function PoopWep::Use( %key ) {

	if( GetItemCount( $PoopWep::Wep[ %key @ "A" ] ) == 1 && GetMountedItem( 0 ) != GetItemType( $PoopWep::Wep[%key@"A"]) && $PoopWep::Tapped[%key] != "TRUE") {
		use( $PoopWep::Wep[ %key @ "A" ] );
		$PoopWep::Tapped[ %key ] = "TRUE";
		schedule::add( "$PoopWep::Tapped[ " @ %key @ " ] = \"\";", 0.5);
		
	}
	
	else if( GetItemCount( $PoopWep::Wep[ %key @ "B" ] ) == 1 && GetMountedItem( 0 ) != GetItemType( $PoopWep::Wep[ %key @ "B" ]) ) {
		use( $PoopWep::Wep[ %key @ "B" ] );
		$PoopWep::Tapped[ %key ] = "";
	}
	
}

function PoopWeaps140::InitBinds()	after GameBinds::Init {

	$GameBinds::CurrentMapHandle = GameBinds::GetActionMap2( "playMap.sae" );
	$GameBinds::CurrentMap = "playMap.sae";
	
	for(%i = 1; %i <= $PoopWep::DualWeps; %i++)
		GameBinds::addBindCommand( "POOP " @ $PoopWep::Wep[ %i @ "A" ] @ " " @ $PoopWep::Wep[ %i @ "B" ], "PoopWep::Use( " @ %i @ ");" );
}