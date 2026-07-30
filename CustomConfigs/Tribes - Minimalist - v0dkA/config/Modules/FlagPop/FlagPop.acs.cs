function FlagPop( %team, %cl ) {
   if ( %cl != getManagerId())
      return;
   remoteEval(2048, lmsg, "capobj");
   remoteBP( 2048, "<JC><f:FlagPop-Font.pft:00deff>YOU HAVE THE FLAG!", 3 );
}

Event::Attach( eventFlagGrab, FlagPop );
Event::Attach( eventFlagPickup, FlagPop );