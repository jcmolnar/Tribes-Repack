=============================================================
 Tribes 1.5 Modern Client -- Public Beta
=============================================================

A modern, from-source rebuild of the Starsiege: TRIBES client.
The classic community mods come already installed -- Tribes
RPG / Kingdom of Kronos, Red Moon RPG, Star Wars RPG and more
-- so one client plays all of them. (Hosting is a separate
question -- see HOSTING A SERVER below.)

No installer. Extract the folder anywhere and play.

Everything here -- every mod, and hosting a server for any of
them -- is driven by ONE client (ModernTribes.exe) and ONE
text file (modlist.txt). There are no per-mod launchers to
hunt for.


-------------------------------------------------------------
 QUICK START
-------------------------------------------------------------

  1. Double-click  ModernTribes.exe
  2. Play.

Windows SmartScreen note: the first time you run a freshly
downloaded copy, Windows may show "Windows protected your PC /
Unknown publisher". That is normal for an unsigned community
build with no download history -- click "More info" and then
"Run anyway". You can avoid it entirely by right-clicking the
downloaded .zip BEFORE extracting, choosing Properties, and
ticking "Unblock". Players who update through the in-game
Update button will never see the warning.

Out of the box it runs whatever modlist.txt selects. To change
mods, see the next section.


-------------------------------------------------------------
 CHOOSING WHAT YOU PLAY  (modlist.txt)
-------------------------------------------------------------

Open  modlist.txt  in this folder with Notepad. Each line is a
launch argument; a line starting with '#' or ';' is ignored.
Uncomment the ONE block you want:

  Base Tribes ......... (leave every line commented out)
  Tribes RPG / Kronos . -mod rpg
  Red Moon RPG ........ -mod rpg
                        -mod rmrpg     <-- BOTH lines
  Star Wars RPG ....... -mod SWRPG
  Star Wars ........... -mod StarWars
  Delta Air Force ..... -mod DeltaAirForce
  Warhammer 40k ....... -mod War40k
  TAC ................. -mod Tac
  TSC ................. -mod TSC

Save the file and start the game. That is the whole procedure.

WHY RED MOON NEEDS TWO LINES: Red Moon is built ON TOP of
Tribes RPG, so both mods must load, in that order. Loading
"-mod rmrpg" on its own will not work properly.

The same file decides what a dedicated server hosts -- see
HOSTING below.


-------------------------------------------------------------
 THE MODS
-------------------------------------------------------------

Base Tribes
  The 1998 game as shipped: CTF, Deathmatch and the rest, on
  the stock maps. Nothing commented in means base.

Tribes RPG / Kingdom of Kronos
  The long-running RPG mod: persistent characters, zones,
  shops, NPCs and quests. Kingdom of Kronos is the live
  server for it.

Red Moon RPG (RMRPG)
  A large expansion built on Tribes RPG -- its own world,
  items and progression. Needs both mod lines (above).

Star Wars RPG (SWRPG)
  RPG-style play in the Star Wars setting.

Star Wars, Delta Air Force, Warhammer 40k, TAC, TSC
  Classic community mods preserved from the repack lineage.
  Each is a straight one-line mod switch.


-------------------------------------------------------------
 HUD / INTERFACE PACKS  (CustomConfigs)
-------------------------------------------------------------

The CustomConfigs folder holds alternative HUD and interface
layouts (ProConfig, Overstep, xLoader, Minimalist and others).
These are NOT mods -- do not put them in modlist.txt. They are
overlays you switch on inside the game, from the Configs tab
in Options, and they apply on top of whichever mod you are
running.

If a HUD ever looks wrong after switching mods or packs,
delete  config\play.gui  -- a fresh one is created next launch.


-------------------------------------------------------------
 HOSTING A SERVER
-------------------------------------------------------------

  Host Server.bat  -- starts a dedicated server.

WHAT YOU CAN HOST
  Base Tribes and the mods that ship with server settings in
  the config folder -- Tribes RPG (config\rpgserv.cs) and Red
  Moon RPG (config\rmrpgserv.cs).

  Being able to PLAY a mod does not mean you can host it.
  Several mods here are client-side content for servers that
  are run elsewhere.

  KINGDOM OF KRONOS IS NOT SOMETHING YOU CAN HOST. It is a
  live, persistent server -- its world, characters and
  server-side systems live there, not in this package. What
  ships here is the client side plus the underlying Tribes
  RPG mod, so selecting "-mod rpg" and hosting gives you a
  plain Tribes RPG server of your own, NOT a copy of Kronos.
  To play Kronos, just join it from the server browser.

TO START ONE
  1. Pick the mod in modlist.txt, exactly as for playing.
  2. Set your server name and port in config\ServerPrefs.cs.
  3. Run Host Server.bat.

The launcher runs the client through InfiniteSpawn.exe, which
restarts the server automatically if it ever crashes. To start
one by hand, note the '*' prefix -- that is how InfiniteSpawn
is told which program to launch, so keep it:

  InfiniteSpawn.exe *ModernTribes.exe -dedicated

IMPORTANT: because a crashed server is restarted for you, a
server that is crash-looping can look like a healthy one.
Before you call a test clean, check InfiniteSpawn's restart
counter and read console.log.

Server settings, in the config folder:

  config\ServerPrefs.cs
      Server name, port, maximum players, and the general
      engine defaults. Start here.

  config\rpgserv.cs
      Tribes RPG server settings (default port 28001).
      Admin login requires ALL FIVE $AdminPassword slots to
      be filled -- they ship blank, which means admin is
      disabled until you set them.

  config\rmrpgserv.cs
      Red Moon server settings (default port 28002).
      The admin password ships as "changeme" -- CHANGE IT
      before hosting publicly. Become admin in game with:
          SAD("yourpassword");

Hosting from home: forward the UDP port your server uses in
your router. Your server announces itself to the public
Tribes master list; set $Server::HostPublicGame = "false" in
config\ServerPrefs.cs if you would rather keep it private.

Console output goes to console.log in this folder.


-------------------------------------------------------------
 UPDATES
-------------------------------------------------------------

The client keeps itself up to date. It checks at startup and,
when a new build is out, offers to download only the files
that actually changed -- each one hash-verified. You never
need to re-download the whole package by hand.

News and downloads:  kingdomofkronos.com/beta


-------------------------------------------------------------
 TROUBLESHOOTING
-------------------------------------------------------------

- Video problems: the client uses OpenGL. If you end up on
  Software rendering, set OpenGL in Options > Video.
- Interface small or blurry on a high-DPI monitor: right-click
  ModernTribes.exe > Properties > Compatibility > Change high
  DPI settings > tick "Override high DPI scaling behaviour",
  set it to Application.
- Custom keybinds fighting the pack's extras: edit
  config\extra-controls.cs.
- HUD stuck from another mod or pack: delete config\play.gui.
- A mod will not load: check modlist.txt for a stray '#', and
  that the mod's folder is still present in this directory.


-------------------------------------------------------------
 BUGS
-------------------------------------------------------------

This is a public beta and bug reports are the point.

  Report bugs on Discord:  https://discord.gg/SqpyvbVq3D

Useful when reporting: which mod you were on, whether you were
playing or hosting, and console.log from this folder.


-------------------------------------------------------------
 CREDITS
-------------------------------------------------------------

  Original Tribes RPG mod ......... the Tribes RPG community
  Red Moon RPG .................... Chee & Deus
  Community repack lineage (r1-41)  phantom
  Native client rebuild, Kronos
  HUD, and this package ........... Jobo

Starsiege: TRIBES (c) 1998 Sierra On-Line / Dynamix.
