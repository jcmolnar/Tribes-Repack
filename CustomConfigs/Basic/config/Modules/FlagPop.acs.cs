function FlagPop( %team, %cl ) {
   if ( %cl != getManagerId())
      return;
   remoteEval(2048, lmsg, "capobj");
   remoteBP( 2048, "<JC><f:white.pft:00fffc>YOU HAVE THE FLAG!", 3 );
}

Event::Attach( eventFlagGrab, FlagPop );
Event::Attach( eventFlagPickup, FlagPop );