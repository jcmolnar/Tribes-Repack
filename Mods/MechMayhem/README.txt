=============================================================================
 MECH MAYHEM  --  a Starsiege total conversion for Tribes 1.5 Modern Client
=============================================================================

31 authentic Starsiege HERC chassis, the full 44-weapon arsenal with real
mined stats, heat management, shields, salvage progression, and bot armies.
Everything below ships with the mod -- no extra downloads.


-----------------------------------------------------------------------------
 PLAYING
-----------------------------------------------------------------------------

YOUR MECH
  - You spawn in a mech, not an armor. Light scouts walk ~15 u/s; the
    Prometheus is a fortress. Every chassis carries its authentic Starsiege
    Combat Value (CV), tech level, and hardpoint rack.
  - FIRST SPAWNS rotate you through the free (tech 1) chassis. Pick your own
    in the Garage (below) -- your choice persists across deaths, maps, and
    sessions.

HEAT IS EVERYTHING
  - Your energy pool is an inverted heat gauge. Weapons, jets, and dashing
    all spend it; the reactor refills it.
  - Run the pool to the floor and your mech SHUTS DOWN -- a statue for ~6
    seconds, then a restart at 30% pool. Manage your alpha strikes.
  - There is no ammo. Heat is the ammo. The HEAT bar on the cockpit HUD is
    the only supply line you have.

FIRING -- CHAINED FIRE
  - Hold the trigger and your ENTIRE rack fires: every weapon in your
    loadout is mounted live on its pod and fires in relay, each at its own
    rate and heat cost. First pull ripples an alpha strike.
  - A weapon the reactor can't afford is skipped until the pool recovers.
  - Next/prev weapon still works -- it changes which weapon LEADS the chain.

MOVEMENT
  - JUMP = DASH: a horizontal lunge in your movement direction. Costs 15%
    of the heat pool, 2.5 s cooldown. Scouts lunge hardest.
  - SCOUT CHASSIS FLY: Talon, Seeker, Goad, and Emancipator have jump jets
    (hold jet). Flying builds heat -- land hot and your lasers hit soft.
  - Shutdown or crippled mechs cannot dash or fly.

DAMAGE MODEL
  - Shields absorb energy fire best; ballistics punch through (the
    Starsiege duality). Watch the SHLD bar.
  - Hits are located: leg hits can CRIPPLE you (60% speed, no dash),
    cockpit hits hurt more, component damage lights the LEGS / GUNS /
    SENS / RCTR lamps on the right of the HUD.

LAST STAND -- THE EJECTION SEAT
  - Every tech-5+ HUMAN-faction chassis (marked +EJECT in the Garage:
    Harabec's Apocalypse, the Knights' Apocalypse and Gorgon) carries an
    ejection system: the blow that would kill it instead blasts you out of
    the cockpit as the mech's reactor cooks off beneath you. Cybrid hulls
    have no cockpit to eject from.
  - You come down in a hardened flight suit with jump jets, an energy pack,
    and an anti-HERC kit:
        HERC Lance     shoulder-fired seeking missile; punches shields
        Demo Charge    lobbed satchel, short fuse -- the real mech-killer
        Pilot Sidearm  fast blaster for the other ejected pilot
    All three feed off the suit capacitor -- no ammo, just recharge.
  - The mech's CV is charged when the hull dies; your time on foot is free.
    Your killer only CONFIRMS the kill (and a salvage bounty) by hunting
    you down. Die on foot and you respawn into a fresh mech as normal.

THE GARAGE  (TAB menu -> "Mech Garage")
  - Browse by class (Light / Medium / Heavy / Assault). Every entry shows
    tech level, CV, and hardpoint rack.
  - Tech 1 chassis are free. Everything else is LOCKED until you buy it
    with SALVAGE (price = 2x its CV). Locked entries show the price; pick
    one and you get a buy-confirm right there.
  - Your pick takes effect on your NEXT spawn and is remembered.

SALVAGE & PROGRESSION
  - Destroying a mech pays you its CV in salvage. BOT kills pay 25% --
    you can grind offline against bots, it is just slower than fighting
    humans.
  - Salvage, kills, unlocks, and best incursion wave are saved on the
    server under your player name, forever.
  - PROTECT YOUR RECORD: anyone using your name gets your progress. Claim
    it with a password -- open the console (~) and run:
        remoteEval(2048, MMPass, "yourpassword");
    First run sets the password; after that the same command unlocks your
    record each time you connect. A locked record earns and spends nothing
    until unlocked.

BATTLE MODES
  - ESCALATION (Arena / Monsoon): team deathmatch with CV tickets. Every
    loss drains the fallen chassis' CV from its team pool; empty pool
    loses. Cheap mechs are cheap deaths -- a Prometheus loss is a crisis.
  - INCURSION: PvE. Survive 10 Cybrid waves, swarm to boss; every fifth
    wave leads with the heavies. A full defender wipe mid-wave loses the
    outpost. Cybrid kills bank team salvage.
  - GROUNDWAR: zone control -- presence captures, your bots take defend
    orders.
  - Vote the next mode from the TAB menu ("Vote next battle mode"): 45 s
    window, majority of connected pilots wins.

THE O KEY
  - Objectives page: mode rules, live CV pools / wave status, your mech's
    controls crib sheet, and the top-pilots table. Also shows the match
    summary at round end.


-----------------------------------------------------------------------------
 HOSTING
-----------------------------------------------------------------------------

QUICK START
  - Run TribesHost.exe, choose the "Mech Mayhem" profile, pick a Mech*
    map, Start. That's it -- bots included.
  - Manual alternative: launch the dedicated server with -mod MechMayhem
    and a Mech* mission.

MISSIONS
  - MechArena1 (Escalation), MechMonsoon1 (Escalation, storm),
    MechIncursion1 (PvE waves), MechGroundwar1 (zones). Rotation and
    mixed-type rotations work normally from TribesHost.

BOTS
  - A full mech roster ships in config\botbrain.cfg: 6 human-faction
    defenders (team 1) and 12 Cybrids swarm-to-boss (team 2). They spawn
    automatically on Mech* missions only; trooper missions keep the
    normal trooper bots.
  - CAP THE COUNTS: TribesHost -> "Bots" section -> Team 1 / Team 2
    fields (blank = full roster). Live equivalent from the server
    console, takes effect on the next spawns:
        $pref::botMaxTeam0 = 4;   // team 1
        $pref::botMaxTeam1 = 8;   // team 2
    On Incursion, team 2 is the Cybrid horde -- its cap sets wave size,
    which makes it a difficulty knob.
  - Bots use the real arsenal, chain fire, dash-dodge, and fly scout
    chassis. They fight at weapon range, not knife range.

PILOT RECORDS (PROGRESSION)
  - Server-side, zero admin: config\mm_pilots\pilots.cs holds every
    pilot's salvage/unlocks, autosaved every 60 s and at round end.
    Back that one file up and you've backed up the ladder.
  - Records are keyed by player name; the MMPass password (see PLAYING)
    stops name-squatting. Passwords are stored plainly in pilots.cs --
    treat the file as private, and tell players not to reuse a real
    password.

TUNING KNOBS (server console or serverPrefs)
  - $MM::TicketPool        (default 40000)  escalation CV per team
  - $MM::BotSalvageFrac    (default 0.25)   bot kills pay this x CV
  - $MM::UnlockCostMult    (default 2)      unlock price = CV x this
  - $MM::FreeTech          (default 1)      tech level free for everyone
  - $MM::MaxWave           (default 10)     incursion length
  - $pref::hercCamScale                     chase camera distance
    (players set their own in the K menu -> Mech Cockpit config)

ZERO-INSTALL CLIENTS
  - Datablocks sync at connect and the mech art ships in base\, so
    updated Modern Client players join with nothing extra to install.
    (Public availability of the mod itself lands with the v17 Mods\
    resolver.)

=============================================================================
