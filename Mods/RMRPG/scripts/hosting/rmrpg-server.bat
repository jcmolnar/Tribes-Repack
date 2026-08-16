@echo off
REM Clean dedicated-server spawn for Red Moon RPG (RMRPG).
REM Uses NativeTribes.exe (native rebuild), NOT Tribes.exe: the Borland exe
REM injects the Kronos mem.dll plugin chain (GraphicPlugin) which has no GL
REM context on a dedicated server and crashes it. NativeTribes is plugin-free.
REM Host settings (name/port/players/mission=RMR) live in the config folder,
REM file rmrpgserv.cs. Console output goes to console.log.
cd /d "%~dp0"
start "RMRPG Server" Tribes.exe -mod rmrpg -dedicated
