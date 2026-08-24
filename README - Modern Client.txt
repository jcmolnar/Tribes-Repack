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
them -- is driven by ONE client (ModernTribes.exe). You pick
the mod in the game itself; there are no per-mod launchers to
hunt for and no text file to edit.


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

The first time you start it, it asks which mod you want. To
change your mind later, see the next section.


-------------------------------------------------------------
 CHOOSING WHAT YOU PLAY
-------------------------------------------------------------

The first launch asks. A window lists every mod this install
has, you pick one and press PLAY. Tick "Remember my choice"
and it stops asking.

To change mod after that, any of these:

  * MODS on the main menu.
  * The MOD panel in the top bar of any menu screen -- it
    always shows what you are running now. Click it.
  * Options -> Configs -> Mod.
  * Hold SHIFT while the game starts to get the first-launch
    picker back.

Changing mod restarts Tribes. That is not a limitation we can
remove: a mod's scripts are loaded once at startup and the
engine has no way to unload them again.

JOINING A MODDED SERVER: you do not have to match it yourself.
Pick the server in the browser and, if it runs a mod you are
not currently in, Tribes restarts into that mod and joins for
you. Your saved default is left alone.

WHY RED MOON IS ONE ENTRY, NOT TWO: Red Moon is built ON TOP
of Tribes RPG, so both mods have to load, in that order. The
selector knows, and picks both for you. (It used to be two
hand-typed lines, and getting the order wrong half-worked in
confusing ways.)

modlist.txt is still there and still works. The game now owns
a block at the top of it, so if you had uncommented a line by
hand, that choice is picked up automatically the first time
you run this version -- your old file is kept as
modlist.txt.pre-modsel.bak.


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
These are NOT mods -- they do not appear in the mod picker. They are
overlays you switch on inside the game, from the Configs tab
in Options, and they apply on top of whichever mod you are
running.

If a HUD ever looks wrong after switching mods or packs,
delete  config\play.gui  -- a fresh one is created next launch.


-------------------------------------------------------------
 HOSTING A SERVER
-------------------------------------------------------------

  TribesHost.exe  -- everything in one window. Start here.
  (Host Server.bat opens the same tool.)

Pick a profile (Base Tribes, Tribes RPG, or Red Moon RPG),
pick a map, type a server name, press Start. TribesHost
writes the same config files listed below, launches the
dedicated server, and keeps it running: if the server
crashes it restarts it after 5 seconds, and if it keeps
crashing it STOPS and shows a red crash-loop warning with
the log, instead of hiding the problem behind endless
restarts. It also shows live status -- players, current
map, uptime, restart count -- and has one-click buttons
for the Windows Firewall rule and a UPnP router
port-forward.

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
  RPG mod, so hosting the Tribes RPG profile gives you a
  plain Tribes RPG server of your own, NOT a copy of Kronos.
  To play Kronos, just join it from the server browser.

THE OLD WAY (still works)
  1. Pick the mod with -mod on the command line (below).
     NOTE: a bare "-dedicated" run still inherits whatever mod
     you picked for PLAYING, exactly as it always has. Naming
     -mod explicitly is what makes a server independent of it.
  2. Set your server name and port in config\ServerPrefs.cs
     (base) or config\rpgserv.cs / rmrpgserv.cs (RPG mods).
  3. Run:  InfiniteSpawn.exe *ModernTribes.exe -dedicated
     or, for a specific mod regardless of your play choice:
           InfiniteSpawn.exe *ModernTribes.exe -mod rpg -dedicated

  InfiniteSpawn restarts a crashed server, but silently --
  a crash-looping server can look like a healthy one, so
  check its restart counter and console.log before calling
  a test clean. TribesHost edits the same files as this
  path, so the two stay in sync.

Server settings, in the config folder (TribesHost edits
these for you; hand-editing still works):

  config\ServerPrefs.cs
      Server name, port, maximum players, and the general
      engine defaults for base Tribes hosting.

  config\rpgserv.cs
      Tribes RPG server settings (default port 28001).
      Admin login requires ALL FIVE $AdminPassword slots to
      be filled -- TribesHost fills all five from its Admin
      password box. They ship blank (admin disabled).

  config\rmrpgserv.cs
      Red Moon server settings (default port 28002).
      The admin password ships as "changeme" -- CHANGE IT
      before hosting publicly. Become admin in game with:
          SAD("yourpassword");

Hosting from home: your router must forward the server's
UDP port to this PC. TribesHost's "Forward port on router
(UPnP)" button does this automatically if your router
allows it, and "Allow in Windows Firewall" opens the port
locally. Your server announces itself to the public Tribes
master list unless you untick "Public server".

Console output goes to console.log in this folder (also
shown live at the bottom of TribesHost).


-------------------------------------------------------------
 UPDATES
-------------------------------------------------------------

The client keeps itself up to date. It checks at startup and,
when a new build is out, offers to download only the files
that actually changed -- each one hash-verified. You never
need to re-download the whole package by hand.

If you host with TribesHost, close it before applying an
update so its own file can be replaced.

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
- A mod will not load: open MODS on the main menu and check it
  is not greyed out as "not installed"; and
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

Upscaled textures in this package were produced with the
4xNomos8kDAT model by Philip Hofmann (Phhofm), used under
CC BY 4.0:

  https://huggingface.co/Phips/4xNomos8kDAT
  https://creativecommons.org/licenses/by/4.0/

Starsiege: TRIBES (c) 1998 Sierra On-Line / Dynamix.
