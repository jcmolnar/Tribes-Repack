@echo off
REM ============================================================================
REM  Dedicated server launcher -- Tribes 1.5 Modern Client
REM
REM  This opens TribesHost.exe: pick the mod + map, set the server name /
REM  port / passwords, press Start. It keeps the server running (auto-restart
REM  after a crash) and, unlike the old InfiniteSpawn loop, STOPS and warns
REM  you when the server is crash-looping instead of hiding it.
REM
REM  Hostable: base Tribes, Tribes RPG, Red Moon RPG. Kingdom of Kronos
REM  CANNOT be hosted -- it is a live persistent server; hosting the Tribes
REM  RPG profile gives you a plain Tribes RPG server of your own.
REM
REM  The old way still works and edits the same config files:
REM      InfiniteSpawn.exe *ModernTribes.exe -dedicated
REM  with the mod chosen in modlist.txt and settings in config\ServerPrefs.cs
REM  (base) or config\rpgserv.cs / rmrpgserv.cs (RPG mods).
REM
REM  See "README - Modern Client.txt", section HOSTING A SERVER.
REM ============================================================================
cd /d "%~dp0"
start "" TribesHost.exe
