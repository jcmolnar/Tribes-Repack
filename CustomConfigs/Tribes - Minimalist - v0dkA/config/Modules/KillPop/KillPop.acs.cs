function KillPop( %killer, %victim, %damage ) {
    if ( %victim == getManagerId() )
        
    remoteBP( 2048, "<JC><f:KillPop-Font.pft:ff0000ff>KILLED BY: <f:KillPop-Font.pft>" ~ String::escapeFormatting( Client::getName( %killer ) ) ~ "\n<f:KillPop-Font.pft>Weapon: <f:KillPop-Font.pft:ffde00ff>" ~ %damage, 5 );

    if ( %killer != getManagerId() )
        return;

    remoteBP( 2048, "<JC><f:KillPop-Font.pft>YOU <f:KillPop-Font.pft:00ccffff>KILLED: <f:KillPop-Font.pft>" ~ String::escapeFormatting( Client::getName( %victim ) ) ~ "\n<f:KillPop-Font.pft>Weapon: <f:KillPop-Font.pft:ffde00ff>" ~ %damage, 5 );
    //localSound(HailKing);
    localSound(KillSound);
}

function TeamKillPop( %killer, %victim, %damage ) {
    if ( %victim == getManagerId() )

    remoteBP( 2048, "<JC><f:KillPop-Font.pft:ff0000ff>TEAMKILLED BY: <f:KillPop-Font.pft>" ~ String::escapeFormatting( Client::getName( %killer ) ), 5 );

    if ( %killer != getManagerId() )
    return;

    remoteBP( 2048, "<JC><f:KillPop-Font.pft>YOU <f:KillPop-Font.pft:ffde00ff>TEAMKILLED <f:KillPop-Font.pft>" ~ String::escapeFormatting( Client::getName( %victim ) ), 5 );
        localSound(Access_Denied);
}

Event::Attach( eventClientKilled, KillPop );
Event::Attach( eventClientTeamKilled, TeamKillPop );