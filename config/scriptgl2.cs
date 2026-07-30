//
//	Variable sized HUDs
//

function vhud::init( ) {
	deletevariables("vhud*");

	// exec( "vhud.halflife2.cs" );
}

//
// vhud Hud Creation/Management functions
//

function vhud::set_current( %name ) {
	$vhud::name = ( %name );
}

function vhud::create( %name, %size, %pos, %onrender ) {
	// only link the hud if it hasn't been created
	if ( !$vhud[%name,size] ) {
		$vhud[%name,next] = $vhud[names];
		$vhud[names] = %name;
	}

	if ( string::findsubstr( %onrender, ";" ) == -1 )
		%onrender = ( %onrender @ "();" );
	
	$vhud[%name,items] = 0;
	$vhud[%name,size] = %size;
	$vhud[%name,pos] = %pos;
	$vhud[%name,onrender] = %onrender;
	$vhud[%name,lastdimensions] = "";
	
	vhud::set_current( %name );
}

function vhud::delete( %name ) {
	%prev = "";
	%cur = $vhud[names];
	while ( %cur != "" ) {
		%next = ( $vhud[%cur,next] );
		
		if ( %cur == %name ) {
			if ( %prev != "" )
				$vhud[%prev,next] = ( %next );
			else
				$vhud[names] = ( %next );
			$vhud[%cur,size] = "";
			return;
		}
		
		%prev = ( %cur );
		%cur = ( %next );
	}
}

function vhud::duplicate( %from, %to, %new_pos, %onrender ) {
	vhud::create( %to, $vhud[%from,size], %new_pos, %onrender );
	
	$vhud[%to,items] = ( $vhud[%from,items] );
	for ( %i = 0; %i < $vhud[%from,items]; %i++ ) {
		%label = ( $vhud[%from,items,%i] );
		$vhud[%to,items,%label] = ( $vhud[%from,items,%label] );
		$vhud[%to,items,%i] = %label;
	}
}

function vhud::add_item( %label, %value ) {
	%name = $vhud::name;
	%cnt = ( $vhud[%name,items]++ ) - 1;
	$vhud[%name,items,%cnt] = %label;
	$vhud[%name,items,%label] = %value;
	$vhud[%name,items,%label,ishud] = false;
}

function vhud::alter_item( %name, %label, %value ) {
	$vhud[%name,items,%label] = ( %value );
}

//
// vhud Rendering & Calculation functions
//

function vhud::render( %dimensions ) {
	// walk the hud linked list
	for ( %name = $vhud[names]; %name != ""; %name = $vhud[%name,next] ) {
		vhud::set_current( %name );
		vhud::calculate( %dimensions );
		eval( $vhud[%name,onrender] );
	}
}

function vhud::set_scalar( %scale, %translate ) {
	$vhud[0] = ( getword( %scale, 0 ) );
	$vhud[1] = ( getword( %scale, 1 ) );
	$vhud[0,offs] = ( getword( %translate, 0 ) );
	$vhud[1,offs] = ( getword( %translate, 1 ) );
}

function vhud::scale( %items ) {
	%item0 = getword( %items, 0 );
	%item1 = getword( %items, 1 );

	// scale based on height if we're only doing one
	if ( %item1 == "-1" ) {
		%result = ( %item0 * $vhud[1] / 100 );
	} else {
		%a = ( %item0 * $vhud[0] / 100 ) + ( $vhud[0,offs] );
		%b = ( %item1 * $vhud[1] / 100 ) + ( $vhud[1,offs] );
		%result = %a @ " " @ %b;
	}
	
	return ( %result );
}

function vhud::calculate( %dimensions ) {
	%name = ( $vhud::name );
	
	// only calculate if the dimensions change
	if ( $vhud[%name,lastdimensions] == %dimensions )
		return;
	$vhud[%name,lastdimensions] = ( %dimensions );
	
	// world
	vhud::set_scalar( %dimensions, "0 0" );
	%size = ( vhud::scale( $vhud[%name,size] ) );
	%pos = ( vhud::scale( $vhud[%name,pos] ) );
	$vhud[%name,render,size] = ( %size );
	$vhud[%name,render,pos] = ( %pos );

	// children
	vhud::set_scalar( %size, %pos );

	for ( %i = 0; %i < $vhud[%name,items]; %i++ ) {
		%label = ( $vhud[%name,items,%i] );
		$vhud[%name,render,%label] = ( vhud::scale( $vhud[%name,items,%label] ) );
	}
}

//
// vhud Render helpers
//

function vhud::render_value( %label ) {
	return ( $vhud[$vhud::name,render,%label] );
}

// vhud::render_text uses 9 pixel radius font glows
function vhud::render_text( %label, %font, %font_size_label, %font_smoothing, %text ) {
	%pos = vhud::render_value( %label );
	// glSetFont( name, pixel height, rendering mode, blur radius
	glSetFont( %font, vhud::render_value( %font_size_label ), %font_smoothing, 9 );
	glDrawString( getword(%pos,0), getword(%pos,1), %text );
}

function vhud::render_box( %label_pos, %label_size ) {
	%pos = ( vhud::render_value( %label_pos ) );
	%size = ( vhud::render_value( %label_size ) );

	glRectangle( getword(%pos,0), getword(%pos,1), getword(%size,0), getword(%size,1) );
}

//
// ScriptGL ondraws
//

function ScriptGL::playGui::onPreDraw( %dimensions ) {
	// renders all the vhud huds
	vhud::render( %dimensions );
}

function ScriptGL::playGui::onPostDraw( %dimensions ) {
}

vhud::init( );