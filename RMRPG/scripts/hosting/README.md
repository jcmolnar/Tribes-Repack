# RMRPG dedicated hosting — reference copies

These are **backup/reference copies** of the loose install files that make Red Moon
RPG host as a clean dedicated server. They are NOT read from this folder by the game
(the mod search path doesn't descend into `scripts\hosting\`) — they exist so the
setup is recoverable if the install is wiped. Edit the **canonical** copies, not these.

## Files & canonical locations

| Repo copy | Canonical (active) location | Purpose |
|---|---|---|
| `rmrpg-server.bat` | `C:\Dynamix\Tribes\rmrpg-server.bat` (Tribes root) | Launcher — `NativeTribes.exe -mod rmrpg -dedicated` |
| `rmrpgserv.cs` | `C:\Dynamix\Tribes\config\rmrpgserv.cs` | Server config, exec'd from `Server.cs` `createServer()` |
| `RMR.mis` | `C:\Dynamix\Tribes\RMRPG\MISSIONS\RMR.mis` | The RMR mission. Backup only — MISSIONS/ is outside the tracked `scripts/`. Contains the fix repointing the 4 Goblin StaticShapes from ItemData `cgoblin` to StaticShapeData `cgoblin__`. |

## How it works

- Host by running **`rmrpg-server.bat`**. It uses `NativeTribes.exe`, NOT the Borland
  `Tribes.exe`: the Borland exe injects the Kronos `mem.dll` plugin chain
  (`GraphicPlugin`) which has no GL context on a dedicated server and crashes it.
  `NativeTribes.exe` is plugin-free and hosts cleanly.
- `Server.cs` `createServer()` does `exec(rmrpgserv)` near the top. The install shares
  one root `config\` folder with the RPG/Kronos mod, whose `ServerPrefs.cs` would
  otherwise force RPG values (`$pref::LastMission = "rpgmap6"` → crash, wrong
  HostName/MaxPlayers, dead Dynamix masters). `rmrpgserv.cs` reclaims all of that for
  RMRPG only, right before the mission is chosen.

## What `rmrpgserv.cs` sets

- HostName / MaxPlayers / **Port 28002** (RMRPG-only; RPG/Kronos uses 28001)
- `$pref::LastMission = "RMR"` (the only real RMRPG mission; `chee4` has no `.mis`)
- Live community master servers (copied from Kronos `ServerPrefs.cs`) so the server
  shows up on the browser list instead of heartbeating the dead Dynamix masters
- `$console::logmode = 1`

## Restore procedure

1. Copy `rmrpg-server.bat` → `C:\Dynamix\Tribes\`
2. Copy `rmrpgserv.cs` → `C:\Dynamix\Tribes\config\`
3. Ensure `Server.cs` `createServer()` contains `exec(rmrpgserv);` (it's committed here).

## Local visibility fallback

If it doesn't appear on the master list, add it directly in the client:
`IP:127.0.0.1:28002` (same PC) or `IP:<host-LAN-ip>:28002` (LAN). The server answers
browser pings directly, so this always works.
