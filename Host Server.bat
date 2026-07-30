@echo off
REM ============================================================================
REM  Dedicated server launcher -- Tribes 1.5 Modern Client
REM
REM  WHICH MOD does this server run?  Edit modlist.txt in this folder:
REM      base Tribes ......  every line commented out
REM      Tribes RPG .......  -mod rpg
REM      Red Moon RPG .....  -mod rpg -mod rmrpg      (RMRPG stacks on rpg)
REM  There is no separate launcher per mod -- modlist.txt is the switch.
REM
REM  Hostable: base Tribes, Tribes RPG, Red Moon RPG -- the mods that ship
REM  server settings in config\. Being able to PLAY a mod does not mean you can
REM  host it. Kingdom of Kronos in particular CANNOT be hosted: it is a live
REM  persistent server and its world/characters live there, not in this package.
REM  Hosting "-mod rpg" gives you a plain Tribes RPG server of your own.
REM
REM  InfiniteSpawn relaunches the server if it ever crashes. The '*' prefix is
REM  how it is told which program to run -- keep it.
REM
REM  Server name / port / players / passwords live in the config folder:
REM      config\ServerPrefs.cs  shared engine defaults (name, port, max players)
REM      config\rpgserv.cs      Tribes RPG server, port 28001
REM      config\rmrpgserv.cs    Red Moon server, port 28002 -- the admin password
REM                             ships as "changeme", CHANGE IT before hosting
REM                             publicly.  In game:  SAD("yourpassword");
REM
REM  Console output goes to console.log. Because InfiniteSpawn restarts a
REM  crashed server automatically, check its restart counter and console.log
REM  before calling a test clean -- a crash can otherwise look like uptime.
REM ============================================================================
cd /d "%~dp0"
start "Tribes 1.5 Dedicated Server" InfiniteSpawn.exe *ModernTribes.exe -dedicated
