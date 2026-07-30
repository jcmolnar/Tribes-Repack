function KillPop( %killer, %victim, %damage ) {
    if ( %victim == getManagerId() )
        
    remoteBP( 2048, "<JC><F0>Killed By: <F2>" ~ String::escapeFormatting( Client::getName( %killer ) ) ~ "\n<F1>Weapon: <F2>" ~ %damage, 5 );

    if ( %killer != getManagerId() )
        return;

    remoteBP( 2048, "<JC><F0>Killed: <F2>" ~ String::escapeFormatting( Client::getName( %victim ) ) ~ "\n<F1>Weapon: <F2>" ~ %damage, 5 );
    //localSound(HailKing);
    localSound(KillSound);
}

function TeamKillPop( %killer, %victim, %damage ) {
    if ( %victim == getManagerId() )

    remoteBP( 2048, "<JC><F0>TEAMKILLED By: <F2>" ~ String::escapeFormatting( Client::getName( %killer ) ), 5 );

    if ( %killer != getManagerId() )
    return;

    remoteBP( 2048, "<JC><F0>You <F1>TEAMKILLED <F2>" ~ String::escapeFormatting( Client::getName( %victim ) ), 5 );
        localSound(Access_Denied);
}

Event::Attach( eventClientKilled, KillPop );
Event::Attach( eventClientTeamKilled, TeamKillPop );