function KillPop( %killer, %victim, %damage ) {
    if ( %victim == getManagerId() )
        
    remoteBP( 2048, "<JC><F2>Killed By: <F1>" ~ String::escapeFormatting( Client::getName( %killer ) ) ~ "\n<F1>Weapon: <F2>" ~ %damage, 5 );
    
    if ( %killer != getManagerId() )
        return;

    remoteBP( 2048, "<JC><F1>Killed: <F2>" ~ String::escapeFormatting( Client::getName( %victim ) ) ~ "\n<F1>Weapon: <F2>" ~ %damage, 5 );
localSound(gotcha);
}

function TeamKillPop( %killer, %victim, %damage ) {
	if ( %victim == getManagerId() )

	remoteBP( 2048, "<JC><F1>TEAMKILLED By: <F2>" ~ String::escapeFormatting( Client::getName( %killer ) ), 5 );

	if ( %killer != getManagerId() )
	return;

	remoteBP( 2048, "<JC><F2>You <F1>TEAMKILLED <F2>" ~ String::escapeFormatting( Client::getName( %victim ) ), 5 );
        localSound(fart);
}

//function SelfPop( %victim, %damage ) {
//	if ( %victim != getManagerId() )
//		return;
//	remoteBP( 2048, "<JC><F1>SUICIDE", 3 );
//	localSound(Access_Denied);
//}


	
Event::Attach( eventClientKilled, KillPop );
//Event::Attach( eventClientSuicided, SelfPop );
Event::Attach( eventClientTeamKilled, TeamKillPop );
