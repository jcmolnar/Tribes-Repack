//
// Half Life 2 HUDs
//

function hl2::create( ) {
	// vhud::create( name, size(width% height%), position(x% y%), onrender );
	
	// health, energy, ammo
	vhud::create( "hl2health", "15% 8%", "3% 90%", hl2health::onrender );
	vhud::add_item( "pixelsize_text", "23%" );
	vhud::add_item( "pixelsize_numbers", "55%" );
	vhud::add_item( "label_text", "8.57% 63%" );
	vhud::add_item( "label_value", "58% 20%" );
	
	vhud::duplicate( "hl2health", "hl2energy", "20% 90%", hl2energy::onrender );
	vhud::duplicate( "hl2health", "hl2ammo", "84% 90%", hl2ammo::onrender );
	
	// health bar, energy bar
	vhud::create( "hl2health_bar", "15% 2%", "2.92% 87.243%", "hl2bar::onrender($Health,health);" );
	vhud::add_item( "label_bar_topleft", "5% 30%" );
	vhud::add_item( "label_bar_bottomright", "95% 70%" );
	
	vhud::duplicate( "hl2health_bar", "hl2energy_bar", "20% 87.243%", "hl2bar::onrender($Energy,energy);" );
	
	// pack
	vhud::create( "hl2pack", "6.25% 8.333%", "75% 90%", hl2pack::onrender );
	vhud::add_item( "pack_location", "0% 0%" );
	
	// fullscreen dummy container so we can adjust the radar based on resolution
	vhud::create( "fullscreen", "100% 100%", "0 0", fullscreen::onrender );
	vhud::add_item( "radar_width", "26%" );
	vhud::add_item( "radar_zoom", "100%" );
}

function fullscreen::onrender( ) {
	$pref::radarWidth = ( vhud::render_value( "radar_width" ) );
	$pref::radarZoom = ( 3000 / vhud::render_value( "radar_zoom" ) );
	%x = ( getword( vhud::render_value( "size" ), 0 ) - $pref::radarWidth );
	Control::setPosition( compassHud, %x - 16, 0 );
}

function hl2hud::backdrop( ) {
	glDisable( $GL_TEXTURE_2D );
	glBlendFunc( $GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA );

	glColor4ub( 0, 0, 0, 96 );
	vhud::render_box( "pos", "size" );
}

function hl2bar::onrender( %percentage, %type ) {
	hl2hud::backdrop( );
	
	%topleft = ( vhud::render_value( "label_bar_topleft" ) );
	%bottomright = ( vhud::render_value( "label_bar_bottomright" ) );
	
	%x = ( getword( %topleft, 0 ) ); 
	%y = ( getword( %topleft, 1 ) );
	
	%width = ( getword( %bottomright, 0 ) - %x );
	%height = ( getword( %bottomright, 1 ) - %y );
	
	%percentage /= 100;
	%pixel_width = ( %width * %percentage );

	if ( %type == energy ) {
		glColor4ub( 0, 255, 255, 192 );
	} else if ( %type == health ) {
		if ( %percentage > 0.660001 )
			glColor4ub( 0, 255, 0, 192 );
		else if ( %percentage > 0.40001 )
			glColor4ub( 255, 255, 0, 192 );
		else
			glColor4ub( 255, 0, 0, 192 );
	} else {
		glColor4ub( 255, 224, 0, 192 );
	}
		
	glRectangle( %x, %y, %pixel_width, %height );
	if ( %pixel_width < %width ) {
		%x += %pixel_width;
		glColor4ub( 128, 128, 128, 64 );
		glRectangle( %x, %y, %width - %pixel_width, %height );
	}
}

function hl2health::onrender( ) {
	hl2hud::backdrop( );

	%percentage = ( $Health / 100 );
	if ( %percentage < 0.40001 )
		glColor4ub( 255, 0, 0, 192 );
	else
		glColor4ub( 255, 224, 0, 192 );

	vhud::render_text( "label_text", "verdana", "pixelsize_text", $GLEX_SMOOTH, "HEALTH" );
	vhud::render_text( "label_value", "halflife2", "pixelsize_numbers", $GLEX_SMOOTH, $Health );
}

function hl2energy::onrender( ) {
	hl2hud::backdrop( );
	
	glColor4ub( 255, 224, 0, 196 );
	vhud::render_text( "label_text", "verdana", "pixelsize_text", $GLEX_SMOOTH, "ENERGY" );
	vhud::render_text( "label_value", "halflife2", "pixelsize_numbers", $GLEX_SMOOTH, $Energy );
}

function hl2ammo::onrender( ) {
	if ( $Ammo < 0 )
		return;

	hl2hud::backdrop( );

	if ( $Ammo != 0 ) {
		%r = 255; %g = 224; %b = 0; %a = 192;
	} else {
		%r = 255; %g = 0; %b = 0; %a = 192;
	}

	if ( $Ammo != $LastAmmo ) {
		$LastAmmo = $Ammo;
		$AmmoTimer = ( GetSimTime( ) );
	}
	
	%duration = ( GetSimTime( ) - $AmmoTimer );
	if (  %duration < 1 && %duration >= 0 ) {
		%r = 255 - ( ( 255 - %r ) * %duration );
		%g = 255 - ( ( 255 - %g ) * %duration );
		%b = 255 - ( ( 255 - %b ) * %duration );
		%a = 255 - ( ( 255 - %a ) * %duration );
	}

	glColor4ub( %r, %g, %b, %a );

	vhud::render_text( "label_text", "verdana", "pixelsize_text", $GLEX_SMOOTH, "AMMO" );
	vhud::render_text( "label_value", "halflife2", "pixelsize_numbers", $GLEX_SMOOTH, $Ammo );
}

// oh yeah, I added getItemType
function hl2pack::choosepack( ) {
	%pack = ( getMountedItem( 1 ) );
	if ( %pack == getItemType( "Energy Pack" ) ) 
		return "hl2.energy.tga";
	else if ( %pack ==  getItemType( "Shield Pack" ) )
		return "hl2.shield.tga";
	else if ( %pack == getItemType( "Ammo Pack" ) )
		return "hl2.ammo.tga";
	else if ( %pack == getItemType( "Repair Pack" ) )
		return "hl2.repair.tga";
	else if ( %pack != -1 )
		return "hl2.misc.tga";
	else
		return "hl2.none.tga";
}

function hl2pack::onrender( ) {
	hl2hud::backdrop( );
	
	glBlendFunc( $GL_SRC_ALPHA, $GL_ONE_MINUS_SRC_ALPHA );
	glTexEnvi( $GL_MODULATE );
	glColor4ub( 255, 255, 255, 192 );

	%size = vhud::render_value( "size" );
	%scale[x] = ( getword( %size, 0 ) / 64 );
	%scale[y] = ( getword( %size, 1 ) / 64 );
	
	%pos = vhud::render_value( "pack_location" );
	%pos[x] = ( getword( %pos, 0 ) );
	%pos[y] = ( getword( %pos, 1 ) );
	
	glDrawTexture( hl2pack::choosepack( ), $GLEX_SCALED, %pos[x], %pos[y], %scale[x], %scale[y] );
}

hl2::create( );